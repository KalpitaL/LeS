using System;
using System.Net;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using System.IO.Compression;
using HTTPMethods;
using System.Threading;

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
        #endregion private varialbles

        public Dictionary<HttpRequestHeader, string> _SetRequestHeaders { get; set; }
        public Dictionary<string, string> _AddRequestHeaders { get; set; }
        public Dictionary<string, string> _dctStateData { get; set; }
        public CookieContainer _CookieContainer { get; set; }
        public HtmlDocument _CurrentDocument { get; set; }
        public string AcceptMimeType { get { return _acceptrequests; } set { _acceptrequests = value; } }
        public string ContentType { get { return _contentType; } set { _contentType = value; } }
        public string CacheControl { get { return _cachecontrol; } set { _cachecontrol = value; } }
        public string UserAgent { get { return _userAgent; } set { _userAgent = value; } }
        public string Referrer { get { return _referrer; } set { _referrer = value; } }
        public bool KeepAlive { get { return _keepAlive; } set { _keepAlive = value; } }
        public bool Expect100Continue { get { return _expect100Continue; } set { _expect100Continue = value; } }


        public string SessionID { get; set; }

        public HTTP _http = new HTTP();

        public HTTPWrapper()
        {
            _SetRequestHeaders = new Dictionary<HttpRequestHeader, string>();
            _AddRequestHeaders = new Dictionary<string, string>();
            _dctStateData = new Dictionary<string, string>();
            _CookieContainer = new CookieContainer();
            _CurrentDocument = new HtmlAgilityPack.HtmlDocument();
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            _SetRequestHeaders.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate,br");
            _SetRequestHeaders.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");
            _AddRequestHeaders.Add("Upgrade-Insecure-Requests", @"1");
        }

        public bool LoadURL(string _request, string validateNodeTitle, string validateAttribute, string attributeValue, string CookieName)
        {
            try
            {
                bool _result = false;
                _http._CookieContainer = _CookieContainer;
                _http.UserAgent = UserAgent;// added by simmy 
                _http.SendGetRequest(_request);
                Referrer = _request;
                //if (SessionID == "" || SessionID == null)
                //{
                //    SetSessionID(_request, CookieName);

                //    if (_http.JSessionID != "")
                //        _SetRequestHeaders.Add(HttpRequestHeader.Cookie, @"JSESSIONID=" + _http.JSessionID + "; " + CookieName + "=" + SessionID + "; oracle.uix=0^^GMT+5:30^p");
                //    //     _SetRequestHeaders.Add(HttpRequestHeader.Cookie, @CookieName + "=" + SessionID + "; oracle.uix=0^^GMT+5:30^p");
                //    //else
                //    //    _SetRequestHeaders.Add(HttpRequestHeader.Cookie, @"oracle.uix=0^^GMT+5:30^p");
                //}

                _CurrentDocument.LoadHtml(_http._CurrentResponseString);
                _dctStateData = _http.GetStateInfo(_CurrentDocument);

                if (validateNodeTitle != "")
                {
                    HtmlNode _node = _CurrentDocument.DocumentNode.SelectSingleNode("//" + validateNodeTitle + "[@" + validateAttribute.ToString() + "='" + attributeValue + "']");
                    _result = (_node != null);
                }
                else _result = true;
                return _result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //public bool LoadURL(out HttpWebResponse response, string _request, string validateNodeTitle, string validateAttribute, string attributeValue)
        public bool LoadURL(string _request, string validateNodeTitle, string validateAttribute, string attributeValue)//out HttpWebResponse response, 
        {
            try
            {
                bool _result = false;
                _http._CookieContainer = _CookieContainer;
               // _http._CookieContainer = new CookieContainer();
                //_http.SendRequest(out response, _request);
                _http.UserAgent = UserAgent; // added by simmy
                _http.SendRequest(_request);
                _CurrentDocument.LoadHtml(_http._CurrentResponseString);
              //  SetSessionID(_request, "PROD");
                if (validateNodeTitle != "")
                {
                    HtmlNode _node = _CurrentDocument.DocumentNode.SelectSingleNode("//" + validateNodeTitle + "[@" + validateAttribute.ToString() + "='" + attributeValue + "']");
                    _result = (_node != null);
                }
                else _result = true;
                return _result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public bool LoadURL(out HttpWebResponse response, string _request, string validateString)
        public bool LoadURL(string _request, string validateString)//out HttpWebResponse response, 
        {
            try
            {
                bool _result = false;
                _http._CookieContainer = _CookieContainer;
                _http.SendRequest(_request);//out response,
                _CurrentDocument.LoadHtml(_http._CurrentResponseString);
              //  SetSessionID(_request, "PROD");
                if (validateString != "")
                {
                    if (_http._CurrentResponseString.Contains(validateString))
                        _result = true;
                }
                else _result = true;
                return _result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetSessionID(string Request, string CookieName)
        {
            try
            {
                foreach (System.Net.Cookie cookie in _CookieContainer.GetCookies(new Uri(Request)))
                {
                    if (cookie.Name == CookieName)//ASP.NET_SessionId
                    {
                        SessionID = cookie.Value;
                        _http.SessionID = SessionID;
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool PostURL(string _request, string _postData, string validateNodeTitle, string validateAttribute, string attributeValue,string strReferer, string CookieName)
        {
            try
            {
                bool _result = false;
                _http._SetRequestHeaders = _SetRequestHeaders;
                _http._AddRequestHeaders = _AddRequestHeaders;
                _http.ContentType = ContentType;
                _http.UserAgent = UserAgent;
                _http.AcceptMimeType = AcceptMimeType;
                _http.Referrer = _referrer;
                _http.KeepAlive = KeepAlive;
                _http.Expect100Continue = Expect100Continue;
                //SetSessionID(_request, CookieName);
                _http.SendPostRequest(_request, _postData);
                _CurrentDocument.LoadHtml(_http._CurrentResponseString);

                _dctStateData = _http.GetStateInfo(_CurrentDocument);
                if (validateNodeTitle != "")
                {
                    HtmlNode _node = _CurrentDocument.DocumentNode.SelectSingleNode("//" + validateNodeTitle + "[@" + validateAttribute.ToString() + "='" + attributeValue + "']");
                    _result = (_node != null);
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

        //public bool PostURL(out HttpWebResponse response, string url, string strPostData, string Referer, bool useCookieContainer, string validateString)
        public bool PostURL(string url, string strPostData, string Referer, bool useCookieContainer, string validateString)
        {
            try
            {
                bool _result = false;
                _http._CookieContainer = _CookieContainer;
                //_http.PostRequest(out response, url, strPostData, Referer, useCookieContainer);
                _http.PostRequest(url, strPostData, Referer, useCookieContainer);
                _CurrentDocument.LoadHtml(_http._CurrentResponseString);
                if (validateString != "")
                {
                    if (_http._CurrentResponseString.Contains(validateString))
                        _result = true;
                }
                else _result = true;
                return _result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //public bool PostURL(out HttpWebResponse response, string url, string strPostData, string Referer, bool useCookieContainer, string validateNodeTitle, string validateAttribute, string attributeValue)
        public bool PostURL(string url, string strPostData, string Referer, bool useCookieContainer, string validateNodeTitle, string validateAttribute, string attributeValue)
        {
            try
            {
                bool _result = false;
                _http._CookieContainer = _CookieContainer;
                _http.PostRequest(url, strPostData, Referer, useCookieContainer);
                //_http.PostRequest(out response, url, strPostData, Referer, useCookieContainer);
                _CurrentDocument.LoadHtml(_http._CurrentResponseString);
                if (validateNodeTitle != "")
                {
                    HtmlNode _node = _CurrentDocument.DocumentNode.SelectSingleNode("//" + validateNodeTitle + "[@" + validateAttribute.ToString() + "='" + attributeValue + "']");
                    _result = (_node != null);
                }
                else _result = true;
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
                _node = _CurrentDocument.DocumentNode.SelectNodes("\\" + nodeTitle + "[@" + _attributeKey.ToString() + "='" + _attrValue + "']");
                return _node;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool PrintScreen(string sFileName)
        {
            bool _result = false;
            try
            {
                
                _http.PrintScreen(sFileName);
                _result = File.Exists(sFileName);
                //return _result; //simmy
            }
            catch (Exception e)
            {
                //throw e;
            }
            return _result;
        }

     

        public bool DownloadDocument(string Request, string postData, string DownloadFile, string ContentType = "")
        {
            try
            {
                bool _result = false;
                _http.DownloadFileContentType = "Application/pdf";
                _http.DownloadDocument(Request, postData, DownloadFile);
                _result = File.Exists(DownloadFile);
                return _result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool DownloadDocument(string Request, string DownloadFile)
        {
            try
            {
                bool _result = false;
                _http.DownloadFileContentType = "Application/pdf";
                _http.DownloadDocument(Request, DownloadFile);
                _result = File.Exists(DownloadFile);
                return _result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool DownloadDocument(out HttpWebResponse response, string url, string strPostData, string Referer, bool useCookieContainer, string FileName, out string fExtension, bool res)
        {
            try {
                bool _result = false;
                _http.PostDownloadRequest(out response, url, strPostData, Referer, useCookieContainer, FileName,out fExtension,res);
                _result = File.Exists(FileName);
                return _result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


      
    }
}
