﻿using System;
using System.Net;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using System.IO.Compression;
using HTTPRoutines;
using System.Text.RegularExpressions;


namespace HTTPWrapper
{
    

    public class HTTPWrapper
    {  

        #region private varialbles
        private string _cachecontrol = "max-age=0";
        private string _contentType = "application/x-www-form-urlencoded";
        private string _userAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
        private string _acceptrequests = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
        private string _referrer = "";
        private bool _keepAlive = true;
        private bool _expect100Continue = false;
        private string _CookieName = "SESSION";
        private bool _isurlencoded = true;
        #endregion private varialbles

        HTTP _http = new HTTP();


        public Dictionary<HttpRequestHeader, string> _SetRequestHeaders { get; set; }
        public Dictionary<string, string> _AddRequestHeaders { get; set; }
        public Dictionary<string, string> _dctStateData { get; set; }
        public Dictionary<string, string> _dctSetCookie { get; set; }
        public CookieContainer _CookieContainer { get; set; }
        public HtmlDocument _CurrentDocument { get; set; }
        public string AcceptMimeType { get { return _acceptrequests; } set { _acceptrequests = value; _http.AcceptMimeType = value; } }
        public string ContentType { get { return _contentType; } set { _contentType = value; _http.ContentType = value; } }
        public string CacheControl { get { return _cachecontrol; } set { _cachecontrol = value; } }
        public string UserAgent { get { return _userAgent; } set { _userAgent = value; _http.UserAgent = value; } }
        public string Referrer { get { return _referrer; } set { _referrer = value; _http.Referrer = value; } }
        public string CooKieName { get { return _CookieName; } set { _CookieName = value; } }
        public bool KeepAlive { get { return _keepAlive; } set { _keepAlive = value; _http.KeepAlive = value; } }
        public bool Expect100Continue { get { return _expect100Continue; } set { _expect100Continue = value; _http.Expect100Continue = value; } }
        public string _CurrentResponseString { get; set; }
        public HttpWebResponse _CurrentResponse { get; set; }
        public bool IsUrlEncoded { get { return _isurlencoded; } set { _isurlencoded = value; _http.IsUrlEncoded = value; } } 

        public string SessionID { get; set; }

       
        public HTTPWrapper()
        {
            _SetRequestHeaders = new Dictionary<HttpRequestHeader, string>();
            _AddRequestHeaders = new Dictionary<string, string>();
            _dctSetCookie = new Dictionary<string,string>();
            _dctStateData = new Dictionary<string, string>();
            _CookieContainer = new CookieContainer();
            _CurrentDocument = new HtmlAgilityPack.HtmlDocument();
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            _SetRequestHeaders.Add(HttpRequestHeader.CacheControl, "max-age=0");
            _SetRequestHeaders.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            _SetRequestHeaders.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");
            _AddRequestHeaders.Add("Upgrade-Insecure-Requests", @"1");        
        }

        public bool LoadURL(string _request, string validateNodeTitle, string validateAttribute, string attributeValue, string CookieName, string HiddenAttributeKey = "", bool _ReadResponse = true)
        {
            try
            {
                bool _result = false;
                SetRequestProperties();
                _http.HiddenAttributeKey = HiddenAttributeKey;

                _http.SendGetRequest(_request);
                Referrer = _request;
                //  if (SessionID == "" || SessionID==null)
                //  {
                SetSessionID(_request, CookieName);

                if (!_SetRequestHeaders.ContainsKey(HttpRequestHeader.Cookie)) _SetRequestHeaders.Add(HttpRequestHeader.Cookie, @"" + CookieName + "=" + SessionID);
                else _SetRequestHeaders[HttpRequestHeader.Cookie] = @"" + CookieName + "=" + SessionID;
                //  }
                _CurrentResponse = _http._CurrentResponse;
                if (_ReadResponse)
                {
                    _CurrentResponseString = ReadResponse(_http._CurrentResponse);
                    _CurrentDocument.LoadHtml(_CurrentResponseString);
                    _dctStateData = _http.GetStateInfo(_CurrentDocument);
                    if (validateNodeTitle != "")
                    {
                        HtmlNode _node = _CurrentDocument.DocumentNode.SelectSingleNode("//" + validateNodeTitle + "[@" + validateAttribute.ToString() + "='" + attributeValue + "']");
                        _result = (_node != null);
                    }
                    else _result = true;                   
                }
                else _result = true;
                return _result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //public bool LoadURLWithHeaders(string _request, string validateNodeTitle, string validateAttribute, string attributeValue, string CookieName, string HiddenAttributeKey = "",bool _ReadResponse=true)
        //{
        //    try
        //    {
        //        bool _result = false;
        //        _http.HiddenAttributeKey = HiddenAttributeKey;
        //        _http._CookieContainer = _CookieContainer;
        //       // _http.SendGetRequestWithHeaders(_request);
        //        _http.SendGetRequest(_request);
        //        Referrer = _request;

        //        SetSessionID(_request, CookieName);
        //        if (!_SetRequestHeaders.ContainsKey(HttpRequestHeader.Cookie)) _SetRequestHeaders.Add(HttpRequestHeader.Cookie, @"" + CookieName + "=" + SessionID);
        //        else _SetRequestHeaders[HttpRequestHeader.Cookie] = @"" + CookieName + "=" + SessionID;

        //        _CurrentResponse = _http._CurrentResponse;
        //        if (_ReadResponse)
        //        {
        //            _CurrentResponseString = ReadResponse(_http._CurrentResponse);
        //            _CurrentDocument.LoadHtml(_CurrentResponseString);
        //            _dctStateData = _http.GetStateInfo(_CurrentDocument);

        //            if (validateNodeTitle != "")
        //            {
        //                HtmlNode _node = _CurrentDocument.DocumentNode.SelectSingleNode("//" + validateNodeTitle + "[@" + validateAttribute.ToString() + "='" + attributeValue + "']");
        //                _result = (_node != null);
        //            }
        //            else _result = true;
        //        }
        //        else _result = true;
        //        return _result;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public void SetSessionID(string Request,string CookieName)
        {
            try
            {
                _dctSetCookie.Clear();
                foreach (System.Net.Cookie cookie in _CookieContainer.GetCookies(new Uri(Request)))
                {
                    _dctSetCookie.Add(cookie.Name, cookie.Value);
                    if (cookie.Name == CookieName)
                    {
                        SessionID = cookie.Value;
                        _http.SessionID = SessionID;                       
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SetRequestProperties()
        {
            _http._CookieContainer = _CookieContainer;
            _http._SetRequestHeaders = _SetRequestHeaders;
            _http._AddRequestHeaders = _AddRequestHeaders;
            _http.ContentType = ContentType;
            _http.UserAgent = UserAgent;
            _http.AcceptMimeType = AcceptMimeType;
            _http.Referrer = _referrer;
            _http.KeepAlive = KeepAlive;
            _http.Expect100Continue = Expect100Continue;     
        }

        public bool PostURL(string _request, string _postData, string validateNodeTitle, string validateAttribute, string attributeValue,string HiddenAttributeKey="",bool readResponse=true)
        {
            try
            {
                bool _result = false;
                SetRequestProperties();                            
                _http.SendPostRequest(_request, _postData);
                _CurrentResponse = _http._CurrentResponse;
                SetSessionID(_request, _CookieName);
                if (readResponse)
                {
                    _CurrentResponseString = ReadResponse(_http._CurrentResponse);
                    _CurrentDocument.LoadHtml(_CurrentResponseString);


                    _dctStateData = _http.GetStateInfo(_CurrentDocument);
                    if (validateNodeTitle != "")
                    {
                        HtmlNode _node = _CurrentDocument.DocumentNode.SelectSingleNode("//" + validateNodeTitle + "[@" + validateAttribute.ToString() + "='" + attributeValue + "']");
                        _result = (_node != null);
                    }
                    else _result = true;                 
                }
                else _result = true;
                Referrer = _request;
                return _result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public HtmlNode GetElement(string nodeTitle, string _attributeKey, string _attrValue)
        {
            try
            {
                HtmlNode _node = null;
                _node = _CurrentDocument.DocumentNode.SelectSingleNode("//" + nodeTitle + "[@" + _attributeKey.ToString() + "='" + _attrValue + "']");
                return _node;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public HtmlNodeCollection GetElementCollection(string nodeTitle, string _attributeKey, string _attrValue)
        {
            try
            {
                HtmlNodeCollection _node = null;
                _node = _CurrentDocument.DocumentNode.SelectNodes("//" + nodeTitle + "[@" + _attributeKey.ToString() + "='" + _attrValue + "']");
                return _node;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool PrintScreen(string sFileName)
        {
            try
            {
                bool _result = false;
                _http.PrintScreen(sFileName,_CurrentResponseString);
                _result = File.Exists(sFileName);
                return _result;
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        /// <summary>
        /// Download Document using POST request method
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="postData"></param>
        /// <param name="DownloadFile"></param>
        /// <param name="ContentType"></param>
        /// <returns></returns>
        public bool DownloadDocument(string Request, string postData, string DownloadFile, string ContentType = "Application/pdf")
        {
            try
            {
                bool _result = false;
                
                _http.DownloadFileContentType = ContentType;
                _http.DownloadDocument(Request, postData, DownloadFile);
               // _CurrentResponseString = ReadResponse(_http._CurrentResponse);
                _result = File.Exists(DownloadFile);
                return _result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //[Sayli 17Fe18]
        /// <summary>
        /// Download document using GET request method
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="DownloadFile"></param>
        /// <returns></returns>
        public bool DownloadDocument(string Request,  string DownloadFile)
        {
            try
            {
                bool _result = false;

                _http.DownloadFileContentType = ContentType;
               _http.DownloadDocument(Request, DownloadFile);
                //_CurrentResponseString = ReadResponse(_http._CurrentResponse);
                _result = File.Exists(DownloadFile);
                return _result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string ReadResponse(HttpWebResponse response)
        {
            try
            {

                using (Stream responseStream = response.GetResponseStream())
                {
                    Stream streamToRead = responseStream;
                    if (response.ContentEncoding.ToLower().Contains("gzip"))
                    {
                        streamToRead = new GZipStream(streamToRead, CompressionMode.Decompress);
                    }
                    else if (response.ContentEncoding.ToLower().Contains("deflate"))
                    {
                        streamToRead = new DeflateStream(streamToRead, CompressionMode.Decompress);
                    }

                    if (streamToRead.CanRead)
                    {
                        using (StreamReader streamReader = new StreamReader(streamToRead, Encoding.UTF8))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                    else return "";
                }
            }
            catch (Exception e)
            { throw e; }
        }

        public HtmlNode GetElement(string _attrValue)
        {
            try
            {
                HtmlNode _node = null;
                _node = _CurrentDocument.GetElementbyId(_attrValue);
                return _node;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public HtmlNode GetElement(HtmlNode _element, string nodeTitle, string _attributeKey, string _attrValue)
        {
            try
            {
                HtmlNode _node = null;
                HtmlDocument _doc = new HtmlDocument();
                _doc.LoadHtml(_element.InnerHtml);
                _node = _doc.DocumentNode.SelectSingleNode("//" + nodeTitle + "[@" + _attributeKey.ToString() + "='" + _attrValue + "' ]");
                return _node;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public HtmlNode GetElement(HtmlNode _element, string nodeTitle, string _innerText)
        {
            try
            {
                HtmlNode _node = null;
                HtmlDocument _doc = new HtmlDocument();
                _doc.LoadHtml(_element.OuterHtml);
                HtmlNodeCollection _nodeColl = _doc.DocumentNode.SelectNodes(".//text()");
                if (_nodeColl.Count > 0)
                {
                    foreach (HtmlNode _eNode in _nodeColl)
                    {
                        if (convert.ToString(_eNode.OuterHtml).ToUpper() == _innerText.ToUpper())
                        {
                            _node = _eNode.PreviousSibling;
                            break;
                        }
                    }
                }
                return _node;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public HtmlNode GetElement(HtmlNode _element, string nodeTitle, string _attributeKey, string _attrValue, string _attributeKey2, string _attrValue2)
        {
            try
            {
                HtmlNode _node = null;
                HtmlDocument _doc = new HtmlDocument();
                _doc.LoadHtml(_element.InnerHtml);
                _node = _doc.DocumentNode.SelectSingleNode("//" + nodeTitle + "[@" + _attributeKey.ToString() + "='" + _attrValue + "' and @" + _attributeKey2 + "='" + _attrValue2 + "' ]");
                return _node;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private string GetElementValue(string elementTagName, string elementAttribule, string elementAttributeVal, string getAttribute)
        {
            string _result = "";
            HtmlNode _ElementNode = GetElement(elementTagName, elementAttribule, elementAttributeVal);

            if (_ElementNode != null)
            {
                if (getAttribute == "value") _result = _ElementNode.Attributes["value"].Value;
                if (getAttribute == "action") _result = _ElementNode.Attributes["action"].Value;
                if (getAttribute == "innertext") _result = _ElementNode.InnerText.Trim();
            }
            return _result;
        }

        private string GetAttributeValue(string elementTagName, string elementAttribule, string elementAttributeVal, string getAttribute)
        {
            string _result = "";
            HtmlNode _ElementNode = GetElement(elementTagName, elementAttribule, elementAttributeVal);

            if (_ElementNode != null)
            {
                _result = _ElementNode.Attributes[getAttribute].Value;
            }
            return _result;
        }

        public bool PostMulitContaintURL(string _requestUrl, string _postData, string boundary)
        {
            bool _result = false;
            SetRequestProperties(boundary);
            _http.SendPostMulitContaintRequest(_requestUrl, _postData);
            _CurrentResponse = _http._CurrentResponse;
            Stream stream = _CurrentResponse.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            _CurrentResponseString = sr.ReadToEnd();
            return _result;
        }


        public void SetRequestProperties(string boundary)
        {
            _http._CookieContainer = _CookieContainer;
            _http._SetRequestHeaders = _SetRequestHeaders;
            _http._AddRequestHeaders = _AddRequestHeaders;
            _http.ContentType = "multipart/form-data; boundary=----" + boundary;
            _http.UserAgent = UserAgent;
            _http.AcceptMimeType = "*/*";
            _http.Referrer = _referrer;
            _http.KeepAlive = KeepAlive;
            _http.Expect100Continue = Expect100Continue;
        }

        //    public void SendPostRequest(string Request, string postData)
        //{
        //    WebResponse response = null;

        //    try
        //    {

        //        var request = (HttpWebRequest)WebRequest.Create(Request);
        //        request.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
        //        if (ProxyUser!=null&& ProxyUser != "")
        //        {
        //            IWebProxy theProxy = request.Proxy;
        //            if (theProxy != null)
        //            {
        //                theProxy.Credentials = new NetworkCredential(ProxyUser, ProxyPassword);
        //            }
        //        }          
        //        request.CookieContainer = _CookieContainer;
        //        request.KeepAlive = KeepAlive;               
        //        SetRequestHeaders(ref request);
        //        request.ContentType = ContentType;
        //        request.UserAgent = UserAgent;
        //        request.Accept = AcceptMimeType;
        //        request.Referer = Request;
        //        request.Method = _requestMethod;
        //        request.ServicePoint.Expect100Continue = Expect100Continue;
        //        byte[] byteArray = Encoding.UTF8.GetBytes(postData);                
        //        request.ContentLength = byteArray.Length;
        //        Stream dataStream = request.GetRequestStream();
        //        dataStream.Write(byteArray, 0, byteArray.Length);
        //        dataStream.Close();
        //         response = request.GetResponse();
        //        if (_CurrentResponse != null) _CurrentResponse.Close();
        //        _CurrentResponse = (HttpWebResponse)response;
        //        //_CurrentResponseString = ReadResponse(_CurrentResponse);

        //    }
        //    catch (WebException e)
        //    {
        //        if (e.Status == WebExceptionStatus.ProtocolError) {
        //            response = (HttpWebResponse)e.Response;
        //            if (_CurrentResponse != null) _CurrentResponse.Close();
        //            _CurrentResponse = (HttpWebResponse)response;
        //        }

        //        if (e.Status == WebExceptionStatus.Timeout)
        //        {
        //            if (!isReload)
        //            {
        //                isReload = true;
        //                SendPostRequest(Request, postData);
        //            }
        //            else
        //            {
        //                isReload = false;
        //                if (response != null) response.Close();
        //                throw e;
        //            }
        //        }
        //        else
        //        {
        //            isReload = false;
        //            if (response != null) response.Close();
        //            throw e;
        //        }
                
        //    }        
        //    catch (Exception e)
        //    {
        //        if (response != null) response.Close();
        //        throw e;
        //    }
        //}
        
        //public void SendPostMulitContaintRequest(string Request, string postData)
        //{
        //    WebResponse response = null;
        //    try
        //    {
        //        var request = (HttpWebRequest)WebRequest.Create(Request);
        //        request.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
        //        if (ProxyUser != null && ProxyUser != "")
        //        {
        //            IWebProxy theProxy = request.Proxy;
        //            if (theProxy != null)
        //            {
        //                theProxy.Credentials = new NetworkCredential(ProxyUser, ProxyPassword);
        //            }
        //        }
        //        request.CookieContainer = _CookieContainer;
        //        request.KeepAlive = KeepAlive;
        //        SetRequestHeaders(ref request);
        //        request.ContentType = ContentType;
        //        request.UserAgent = UserAgent;
        //        request.Accept = AcceptMimeType;
        //        request.Referer = Request;
        //        request.Method = _requestMethod;
        //        request.ServicePoint.Expect100Continue = Expect100Continue;
        //        WriteMultipartBodyToRequest(request, postData);
        //        //Stream dataStream = request.GetRequestStream();
        //        response = request.GetResponse();
        //        if (_CurrentResponse != null) _CurrentResponse.Close();
        //        _CurrentResponse = (HttpWebResponse)response;
               
        //        //_CurrentResponseString = ReadResponse(_CurrentResponse);

        //    }
        //    catch (WebException e)
        //    {
        //        if (e.Status == WebExceptionStatus.ProtocolError)
        //        {
        //            response = (HttpWebResponse)e.Response;
        //            if (_CurrentResponse != null) _CurrentResponse.Close();
        //            _CurrentResponse = (HttpWebResponse)response;
        //        }

        //        if (e.Status == WebExceptionStatus.Timeout)
        //        {
        //            if (!isReload)
        //            {
        //                isReload = true;
        //                SendPostRequest(Request, postData);
        //            }
        //            else
        //            {
        //                isReload = false;
        //                if (response != null) response.Close();
        //                throw e;
        //            }
        //        }
        //        else
        //        {
        //            isReload = false;
        //            if (response != null) response.Close();
        //            throw e;
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        if (response != null) response.Close();
        //        throw e;
        //    }
        //}

        private static void WriteMultipartBodyToRequest(HttpWebRequest request, string body)
        {
            string[] multiparts = Regex.Split(body, @"<!>");
            byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                foreach (string part in multiparts)
                {
                    if (File.Exists(part))
                    {
                        bytes = File.ReadAllBytes(part);
                    }
                    else
                    {
                        bytes = System.Text.Encoding.UTF8.GetBytes(part.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n"));
                    }

                    ms.Write(bytes, 0, bytes.Length);
                }

                request.ContentLength = ms.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    ms.WriteTo(stream);
                }
            }
        }

    }
}
