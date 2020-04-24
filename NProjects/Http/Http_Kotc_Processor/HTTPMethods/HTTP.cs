using System;
using System.Text;
using System.Net;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Web;

namespace HTTPMethods
{
    public class HTTP : WebRequest
    {

        #region Properties



        public CookieContainer _CookieContainer { get; set; }
        public HttpWebResponse _CurrentResponse { get; set; }

        public Dictionary<HttpRequestHeader, string> _SetRequestHeaders { get; set; }
        public Dictionary<string, string> _AddRequestHeaders { get; set; }
        public string _CurrentResponseString { get; set; }
        public string SessionID { get; set; }
        public string FileName { get; set; }
        public bool RequireAuthentication { get; set; }
        public string PROXYURI { get; set; }
        public string ProxyUser { get; set; }
        public string ProxyPassword { get; set; }
        public string UserAgent { get; set; }
        public string AcceptMimeType { get; set; }
        public bool Expect100Continue { get; set; }
        public string Referrer { get; set; }
        public bool KeepAlive { get; set; }
        public string JSessionID = "";

        new public string ContentType { get; set; }
        public string DownloadFileContentType { get; set; }



        #endregion

        public HTTP()
        {
            _SetRequestHeaders = new Dictionary<HttpRequestHeader, string>();
            _AddRequestHeaders = new Dictionary<string, string>();
        }

        public void SendGetRequest(string RequestURL)
        {
            try
            {

                var _request = (HttpWebRequest)WebRequest.Create(RequestURL);
                _request.CookieContainer = this._CookieContainer;
                if (RequireAuthentication)
                {
                    IWebProxy proxy = _request.Proxy;
                    WebProxy myProxy = new WebProxy();
                    Uri newUri = new Uri(PROXYURI);
                    myProxy.Address = newUri;
                    myProxy.Credentials = new NetworkCredential(ProxyUser, ProxyPassword);
                    _request.Proxy = myProxy;
                }
                if (_CurrentResponse != null) _CurrentResponse.Close();
                _CurrentResponse = (HttpWebResponse)_request.GetResponse();
                _CurrentResponseString = ReadResponse(_CurrentResponse);
                string strCookie = _CurrentResponse.Headers["Set-Cookie"];
                if (strCookie.Length > 0)
                {
                    string[] ArrCookie = strCookie.Split(';');
                    if (ArrCookie.Length > 0)
                    {
                        foreach (string s in ArrCookie)
                        {
                            if (s.Split('=')[0] == "JSESSIONID")
                            {
                                JSessionID = s.Split('=')[1];
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e) { throw e; }
        }

        public void SendPostRequest(string Request, string postData)
        {

            try
            {

                var request = (HttpWebRequest)WebRequest.Create(Request);
                request.KeepAlive = KeepAlive;
                SetRequestHeaders(ref request);
                request.ContentType = ContentType;
                request.UserAgent = UserAgent;
                request.Accept = AcceptMimeType;
                request.Referer = Request;
                request.Method = "POST";
                request.ServicePoint.Expect100Continue = Expect100Continue;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                if (_CurrentResponse != null) _CurrentResponse.Close();
                _CurrentResponse = (HttpWebResponse)response;
                _CurrentResponseString = ReadResponse(_CurrentResponse);
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

                    using (StreamReader streamReader = new StreamReader(streamToRead, Encoding.UTF8))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            { throw e; }
        }

        private void SetRequestHeaders(ref HttpWebRequest _request)
        {
            foreach (KeyValuePair<HttpRequestHeader, string> _kv in _SetRequestHeaders)
            {
                _request.Headers.Set(_kv.Key, _kv.Value);
            }

            foreach (KeyValuePair<string, string> _kv in _AddRequestHeaders)
            {
                _request.Headers.Add(_kv.Key, _kv.Value);
            }
        }

        public Dictionary<string, string> GetStateInfo(string Request)
        {
            try
            {
                Dictionary<string, string> dctState = new Dictionary<string, string>();
                SendGetRequest(Request);
                return getStateInfo();
            }
            catch { throw; }

        }

        public Dictionary<string, string> GetStateInfo(string Request, string postData)
        {
            try
            {
                SendPostRequest(Request, postData);
                return getStateInfo();
            }
            catch { throw; }

        }

        public Dictionary<string, string> GetStateInfo(HtmlDocument _doc=null)
        {
            try
            {
                return getStateInfo(_doc);
            }
            catch { throw; }

        }

        private Dictionary<string, string> getStateInfo(HtmlDocument _doc = null)
        {
            Dictionary<string, string> dctState = new Dictionary<string, string>();
            if (_doc == null) _doc = new HtmlAgilityPack.HtmlDocument();
            _doc.LoadHtml(_CurrentResponseString);
          
            HtmlNodeCollection _nodeCollection = _doc.DocumentNode.SelectNodes("//input[@type='hidden']");
            string sKey = "", sValue = "";
            foreach (HtmlNode _hiddenNode in _nodeCollection)
            {
                if (_hiddenNode.Attributes["value"] != null && _hiddenNode.Attributes["id"] != null)
                {
                    try
                    {
                        sValue = HttpUtility.UrlEncode(_hiddenNode.Attributes["value"].Value);
                        sKey = _hiddenNode.Attributes["id"].Value;
                        if (sKey != "" && !dctState.ContainsKey(sKey)) dctState.Add(sKey, sValue);
                    }
                    catch (Exception) { }
                }
            }
            return dctState;
        }

     //chrome//02-04-2018
        //public bool SendRequest(string uRL)
        //{
        //    HttpWebResponse response = null;
        //    bool result = false;
        //    try
        //    {
        //        //Create request to URL.
        //        var request = (HttpWebRequest)WebRequest.Create(uRL);
        //        if (SessionID == null) request.CookieContainer = this._CookieContainer;
               
        //        //Set request headers.
        //        request.KeepAlive = true;
        //        //request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0");//17-2-2018
        //        request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";//"Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";//commented on 2-4-2018
        //        request.Headers.Add("Upgrade-Insecure-Requests", @"1");
        //        request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
        //        request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
        //        request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");
        //        if (SessionID != null) request.Headers.Set(HttpRequestHeader.Cookie, @"JSESSIONID=" + JSessionID + "; PROD=" + SessionID + "; oracle.uix=0^^GMT+5:30^p");
               
        //        response = (HttpWebResponse)request.GetResponse();
              
        //        _CurrentResponse = response;
        //        _CurrentResponseString = ReadResponse(response);
        //        string strCookie = response.Headers["Set-Cookie"];
        //        if (strCookie.Length > 0)
        //        {
        //            string[] ArrCookie = strCookie.Split(';');
        //            if (ArrCookie.Length > 0)
        //            {
        //                foreach (string s in ArrCookie)
        //                {
        //                    if (s.Split('=')[0] == "JSESSIONID")
        //                    {
        //                        JSessionID = s.Split('=')[1];
        //                        result= true;
        //                        break;
        //                    }
        //                }
        //                if (JSessionID == "") result = false;
        //            }
        //        }
        //    }
        //    catch (WebException e)
        //    {
        //        if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
        //        else result= false;
        //    }
        //    catch (Exception)
        //    {
        //        if (response != null) response.Close();
        //        result= false;
        //    }
        //    return result;
        //}

        public bool SendRequest(string uRL)
        {
            HttpWebResponse response = null;
            bool result = false;
            try
            {
                //Create request to URL.
                var request = (HttpWebRequest)WebRequest.Create(uRL);
                if (SessionID == null) request.CookieContainer = this._CookieContainer;

                //Set request headers.
                request.KeepAlive = true;
               // request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";//"Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";//commented on 2-4-2018
                request.UserAgent = UserAgent;
                request.Headers.Add("DNT", @"1");
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-IN");
                if (SessionID != null) request.Headers.Set(HttpRequestHeader.Cookie, @"JSESSIONID=" + JSessionID + "; PROD=" + SessionID + "; oracle.uix=0^^GMT+5:30^p");

                response = (HttpWebResponse)request.GetResponse();

                _CurrentResponse = response;
                _CurrentResponseString = ReadResponse(response);
                string strCookie = response.Headers["Set-Cookie"];
                if (strCookie.Length > 0)
                {
                    string[] ArrCookie = strCookie.Split(';');
                    if (ArrCookie.Length > 0)
                    {
                        foreach (string s in ArrCookie)
                        {
                            if (s.Split('=')[0] == "JSESSIONID")
                            {
                                JSessionID = s.Split('=')[1];
                                result = true;
                                break;
                            }
                        }
                        if (JSessionID == "") result = false;
                    }
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                else result = false;
            }
            catch (Exception)
            {
                if (response != null) response.Close();
                result = false;
            }
            return result;
        }

        //chrome//04-02-2018
        //public bool PostRequest(string url, string strPostData, string Referer, bool useCookieContainer)
        //{
        //    HttpWebResponse response = null;

        //    try
        //    {
        //        //Create request to URL.
        //        var request = (HttpWebRequest)WebRequest.Create(url);
        //        if (useCookieContainer)
        //        {
        //            if (SessionID == null)
        //                request.CookieContainer = this._CookieContainer;
        //        }
        //        else request.CookieContainer = this._CookieContainer;
        //        request.KeepAlive = true;
        //        request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0");
        //        request.Headers.Add("Origin", @"https://erp.kotc.com.kw");
        //        request.Headers.Add("Upgrade-Insecure-Requests", @"1");
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)"; //"Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";//commente on 2-4-2018
        //        request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
        //        request.Referer = Referer;
        //        request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
        //        request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");
        //        request.Headers.Set(HttpRequestHeader.Cookie, @"JSESSIONID=" + JSessionID + "; PROD=" + SessionID + "; oracle.uix=0^^GMT+5:30^p");

        //        //Set request method
        //        request.Method = "POST";
        //        request.ServicePoint.Expect100Continue = false;

        //        //Set request body.
        //        string body = strPostData;
        //        byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
        //        request.ContentLength = postBytes.Length;
        //        Stream stream = request.GetRequestStream();
        //        stream.Write(postBytes, 0, postBytes.Length);
        //        stream.Close();

        //        //Get response to request.
        //        response = (HttpWebResponse)request.GetResponse();
        //        _CurrentResponse = response;
        //        _CurrentResponseString = ReadResponse(response);
        //        //_wrapper._CurrentDocument.LoadHtml(_wrapper._http._CurrentResponseString);
        //        string strCookie = response.Headers["Set-Cookie"];
        //        if (strCookie.Length > 0)
        //        {
        //            string[] ArrCookie = strCookie.Split(';');
        //            if (ArrCookie.Length > 0)
        //            {
        //                foreach (string s in ArrCookie)
        //                {
        //                    if (s.Split('=')[0] == "JSESSIONID")
        //                    {
        //                        JSessionID = s.Split('=')[1];
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (WebException e)
        //    {
        //        if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
        //        else return false;
        //    }
        //    catch (Exception)
        //    {
        //        if (response != null) response.Close();
        //        return false;
        //    }

        //    return true;
        //}

        public bool PostRequest(string url, string strPostData, string Referer, bool useCookieContainer)
        {
            HttpWebResponse response = null;

            try
            {
                //Create request to URL.
                var request = (HttpWebRequest)WebRequest.Create(url);
                if (useCookieContainer)
                {
                    if (SessionID == null)
                        request.CookieContainer = this._CookieContainer;
                }
                else request.CookieContainer = this._CookieContainer;
                request.KeepAlive = true;
                request.Headers.Set(HttpRequestHeader.CacheControl, "no-cache");
                request.Headers.Add("DNT", @"1");
            
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)"; //"Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";//commente on 2-4-2018
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.Referer = Referer;
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-IN");
                request.Headers.Set(HttpRequestHeader.Cookie, @"JSESSIONID=" + JSessionID + "; PROD=" + SessionID + "; oracle.uix=0^^GMT+5:30^p");

                //Set request method
                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;

                //Set request body.
                string body = strPostData;
                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                request.ContentLength = postBytes.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
                stream.Close();

                //Get response to request.
                response = (HttpWebResponse)request.GetResponse();
                _CurrentResponse = response;
                _CurrentResponseString = ReadResponse(response);
                //_wrapper._CurrentDocument.LoadHtml(_wrapper._http._CurrentResponseString);
                string strCookie = response.Headers["Set-Cookie"];
                if (strCookie.Length > 0)
                {
                    string[] ArrCookie = strCookie.Split(';');
                    if (ArrCookie.Length > 0)
                    {
                        foreach (string s in ArrCookie)
                        {
                            if (s.Split('=')[0] == "JSESSIONID")
                            {
                                JSessionID = s.Split('=')[1];
                                break;
                            }
                        }
                    }
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                else return false;
            }
            catch (Exception)
            {
                if (response != null) response.Close();
                return false;
            }

            return true;
        }

        //chrome//2-4-2018
        //public bool PostDownloadRequest(out HttpWebResponse response, string url, string strPostData, string Referer, bool useCookieContainer, string FileName, out string fExtension,bool res)
        //{
        //    bool result = false;
        //    response = null;
        //    fExtension = "";
        //    byte[] b = null;
        //    try
        //    {
        //        //Create request to URL.
        //        var request = (HttpWebRequest)WebRequest.Create(url);
        //        if (useCookieContainer)
        //        {
        //            if (SessionID == null)
        //                request.CookieContainer = this._CookieContainer;
        //        }
        //        else request.CookieContainer = this._CookieContainer;
        //        request.KeepAlive = true;
        //        request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0");
        //        request.Headers.Add("Origin", @"https://erp.kotc.com.kw");
        //        request.Headers.Add("Upgrade-Insecure-Requests", @"1");
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
        //        request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
        //        request.Referer = Referer;
        //        request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
        //        request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");
        //        request.Headers.Set(HttpRequestHeader.Cookie, @"JSESSIONID=" + JSessionID + "; PROD=" + SessionID + "; oracle.uix=0^^GMT+5:30^p");

        //        //Set request method
        //        request.Method = "POST";
        //        request.ServicePoint.Expect100Continue = false;

        //        //Set request body.
        //        string body = strPostData;
        //        byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
        //        request.ContentLength = postBytes.Length;
        //        Stream stream = request.GetRequestStream();
        //        stream.Write(postBytes, 0, postBytes.Length);
        //        stream.Close();

        //        //Get response to request.
        //        response = (HttpWebResponse)request.GetResponse();
        //        _CurrentResponse = response;
        //        if (!res)
        //        {
        //            if (response.Headers["Content-Disposition"] != null)
        //            {
        //                var fn = response.Headers["Content-Disposition"].Split(new string[] { "=" }, StringSplitOptions.None)[1];
        //                fExtension = Path.GetExtension(fn).ToUpper();
        //            }
        //        }

        //        if (fExtension == ".PDF" || res)//||fExtension == ""
        //        {
        //            FileStream fileStream = File.OpenWrite(FileName);
        //            byte[] buffer = new byte[1024];
        //            using (Stream input =_CurrentResponse.GetResponseStream())
        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                int count = 0;
        //                do
        //                {
        //                    byte[] buf = new byte[1024];
        //                    count = input.Read(buf, 0, 1024);
        //                    ms.Write(buf, 0, count);
        //                } while (input.CanRead && count > 0);
        //                b = ms.ToArray();
        //            }
        //            fileStream.Write(b, 0, b.Length);
        //            fileStream.Flush();
        //            fileStream.Close();

        //            result = true;
        //        }
        //        else result = false;
        //    }
        //    catch (WebException e)
        //    {
        //        //ProtocolError indicates a valid HTTP response, but with a non-200 status code (e.g. 304 Not Modified, 404 Not Found)
        //        if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
        //        else result = false;
        //    }
        //    catch (Exception)
        //    {
        //        if (response != null) response.Close();
        //        result = false;
        //    }
        //    return result;
        //}

        public bool PostDownloadRequest(out HttpWebResponse response, string url, string strPostData, string Referer, bool useCookieContainer, string FileName, out string fExtension, bool res)
        {
            bool result = false;
            response = null;
            fExtension = "";
            byte[] b = null;
            try
            {
                //Create request to URL.
                var request = (HttpWebRequest)WebRequest.Create(url);
                if (useCookieContainer)
                {
                    if (SessionID == null)
                        request.CookieContainer = this._CookieContainer;
                }
                else request.CookieContainer = this._CookieContainer;
                request.KeepAlive = true;
                request.Headers.Set(HttpRequestHeader.CacheControl, "no-cache");
                request.Headers.Add("Origin", @"https://erp.kotc.com.kw");
                request.Headers.Add("DNT", @"1");
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.Referer = Referer;
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-IN");
                request.Headers.Set(HttpRequestHeader.Cookie, @"JSESSIONID=" + JSessionID + "; PROD=" + SessionID + "; oracle.uix=0^^GMT+5:30^p");

                //Set request method
                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;

                //Set request body.
                string body = strPostData;
                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                request.ContentLength = postBytes.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
                stream.Close();

                //Get response to request.
                response = (HttpWebResponse)request.GetResponse();
                _CurrentResponse = response;
                if (!res)
                {
                    if (response.Headers["Content-Disposition"] != null)
                    {
                        var fn = response.Headers["Content-Disposition"].Split(new string[] { "=" }, StringSplitOptions.None)[1];
                        fExtension = Path.GetExtension(fn).ToUpper();
                    }
                }

                if (fExtension == ".PDF" || res)//||fExtension == ""
                {
                    FileStream fileStream = File.OpenWrite(FileName);
                    byte[] buffer = new byte[1024];
                    using (Stream input = _CurrentResponse.GetResponseStream())
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            byte[] buf = new byte[1024];
                            count = input.Read(buf, 0, 1024);
                            ms.Write(buf, 0, count);
                        } while (input.CanRead && count > 0);
                        b = ms.ToArray();
                    }
                    fileStream.Write(b, 0, b.Length);
                    fileStream.Flush();
                    fileStream.Close();

                    result = true;
                }
                else result = false;
            }
            catch (WebException e)
            {
                //ProtocolError indicates a valid HTTP response, but with a non-200 status code (e.g. 304 Not Modified, 404 Not Found)
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                else result = false;
            }
            catch (Exception)
            {
                if (response != null) response.Close();
                result = false;
            }
            return result;
        }


        #region Print PDF
        //public void PrintScreen(string sFileName)
        //{
        //    try
        //    {

        //        string sHTML = _CurrentResponseString;
        //        string HTMLFile = Path.ChangeExtension(sFileName, ".html");
        //        File.WriteAllText(HTMLFile, sHTML.Trim());
        //        SautinSoft.PdfVision _vision = new SautinSoft.PdfVision();
        //        _vision.ConvertHtmlFileToImageFile(HTMLFile, sFileName, SautinSoft.PdfVision.eImageFormat.Png);
        //    }
        //    catch (Exception e)
        //    {
        //        //
        //    }
        //}

        public bool PrintScreen(string sFileName)
        {
            string sHTML = _CurrentResponseString;
            if (!Directory.Exists(Path.GetDirectoryName(sFileName) + "\\Temp")) Directory.CreateDirectory(Path.GetDirectoryName(sFileName) + "\\Temp");
            string HTMLFile = Path.ChangeExtension(Path.GetDirectoryName(sFileName) + "\\Temp\\" + Path.GetFileName(sFileName), ".html");
            File.WriteAllText(HTMLFile, sHTML.Trim());
            SautinSoft.PdfVision _vision = new SautinSoft.PdfVision();
            _vision.ConvertHtmlFileToImageFile(HTMLFile, Path.GetDirectoryName(sFileName) + "\\Temp\\" + Path.GetFileName(sFileName), SautinSoft.PdfVision.eImageFormat.Png);
            if (File.Exists(Path.GetDirectoryName(sFileName) + "\\Temp\\" + Path.GetFileName(sFileName)))
            {
                File.Move(Path.GetDirectoryName(sFileName) + "\\Temp\\" + Path.GetFileName(sFileName), sFileName);
                return (File.Exists(sFileName));
            }
            else return false;

        }

        public void PrintScreen(string strHTML, string strFileName)
        {
            try
            {
                string strHTMLFile = Path.ChangeExtension(strFileName, ".html");
                File.WriteAllText(strHTMLFile, strHTML.Trim());
                SautinSoft.PdfVision _vision = new SautinSoft.PdfVision();
                _vision.ConvertHtmlFileToImageFile(strHTMLFile, strFileName, SautinSoft.PdfVision.eImageFormat.Png);
            }
            catch (Exception e)
            {
                //
            }
        }

        public void DownloadDocument(string Request, string postData, string DownloadFile)
        {
            try
            {
                FileName = DownloadFile;
                var request = (HttpWebRequest)WebRequest.Create(Request);
                request.KeepAlive = KeepAlive;
                SetRequestHeaders(ref request);
                request.Referer = Referrer;
                request.Method = "POST";
                request.ProtocolVersion = HttpVersion.Version10;
                request.ServicePoint.Expect100Continue = Expect100Continue;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = ContentType;
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                request.BeginGetResponse(new AsyncCallback(PlayResponeAsync), request);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void PlayResponeAsync(IAsyncResult asyncResult)
        {
            HttpWebRequest webRequest = (HttpWebRequest)asyncResult.AsyncState;
            byte[] b = null;

            using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(asyncResult))
            {
                if (DownloadFileContentType != "" && webResponse.ContentType != DownloadFileContentType)
                {
                    return;
                }
                else
                {
                    FileStream fileStream = File.OpenWrite(FileName);
                    byte[] buffer = new byte[1024];
                    using (Stream input = webResponse.GetResponseStream())
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            byte[] buf = new byte[1024];
                            count = input.Read(buf, 0, 1024);
                            ms.Write(buf, 0, count);
                        } while (input.CanRead && count > 0);
                        b = ms.ToArray();
                    }
                    fileStream.Write(b, 0, b.Length);
                    fileStream.Flush();
                    fileStream.Close();
                }

            }

        }


        public void DownloadDocument(string RequestURL, string FileName)
        {
            byte[] b = null;
            var _request = (HttpWebRequest)WebRequest.Create(RequestURL);
            _request.CookieContainer = this._CookieContainer;
            if (RequireAuthentication)
            {
                IWebProxy proxy = _request.Proxy;
                WebProxy myProxy = new WebProxy();
                Uri newUri = new Uri(PROXYURI);
                myProxy.Address = newUri;
                myProxy.Credentials = new NetworkCredential(ProxyUser, ProxyPassword);
                _request.Proxy = myProxy;
            }
            if (_CurrentResponse != null) _CurrentResponse.Close();
            _CurrentResponse = (HttpWebResponse)_request.GetResponse();
            _CurrentResponseString = ReadResponse(_CurrentResponse);
            FileStream fileStream = File.OpenWrite(FileName);
            byte[] buffer = new byte[1024];
            using (Stream input = _CurrentResponse.GetResponseStream())
            using (MemoryStream ms = new MemoryStream())
            {
                int count = 0;
                do
                {
                    byte[] buf = new byte[1024];
                    count = input.Read(buf, 0, 1024);
                    ms.Write(buf, 0, count);
                } while (input.CanRead && count > 0);
                b = ms.ToArray();
            }
            fileStream.Write(b, 0, b.Length);
            fileStream.Flush();
            fileStream.Close();
        }

        #endregion
    }

    public class convert
    {
        public static long ToLong(object valueFromDb)
        {
            try
            {
                return valueFromDb != DBNull.Value ? Convert.ToInt64(valueFromDb) : 0;
            }
            catch { return 0; }
        }

        public static double ToFloat(object valueFromDb)
        {
            try
            {
                return valueFromDb != DBNull.Value ? Convert.ToDouble(valueFromDb) : 0.0F;
            }
            catch { return 0.0F; }
        }

        public static Boolean ToBoolean(object valueFromDb)
        {
            try
            {
                if (valueFromDb.ToString() == "1") return true;
                else return false;
            }
            catch { return false; }
        }

        public static int ToInt(object valueFromDb)
        {
            try
            {
                if (valueFromDb.ToString().ToUpper() == "TRUE") return 1;
                else if (valueFromDb.ToString().ToUpper() == "YES") return 1;
                return valueFromDb != DBNull.Value ? Convert.ToInt32(valueFromDb) : 0;
            }
            catch { return 0; }
        }

        public static string ToDBValue(int nValue)
        {
            try
            {
                return nValue > 0 ? nValue.ToString() : "NULL";
            }
            catch { return "NULL"; }
        }

        public static object ToDBValue(string cValue)
        {
            try
            {
                if (cValue.Trim() == "") return DBNull.Value;
                else if (cValue.Trim() == "0") return DBNull.Value;
                else return cValue;
            }
            catch { return DBNull.Value; }
        }

        public static string ToQuote(string Value)
        {
            if (Value == null) Value = "";
            Value = Value.Replace("'", "''");
            return ("'" + Value + "'");
        }

        public static DateTime ToDateTime(object valueFromDb)
        {
            try
            {
                if ((valueFromDb.ToString() != null) && (valueFromDb.ToString().Length > 0))
                    return Convert.ToDateTime(valueFromDb);
                else return DateTime.MinValue;
            }
            catch { return DateTime.MinValue; }
        }

        public static DateTime ToDateTime(object valueFromDb, string DateFormat)
        {
            try
            {
                if (DateFormat != string.Empty)
                {
                    if ((valueFromDb.ToString() != null) && (valueFromDb.ToString().Length > 0))
                        return DateTime.ParseExact(valueFromDb.ToString(), DateFormat, null);
                    else return DateTime.MinValue;
                }
                else
                {
                    return ToDateTime(valueFromDb);
                }
            }
            catch { return DateTime.MinValue; }
        }

        public static string ToString(object valueFromDb)
        {
            try
            {
                return valueFromDb != DBNull.Value ? Convert.ToString(valueFromDb) : "";
            }
            catch { return ""; }
        }

        public static string ToNumericOnly(string cInput)
        {
            string cNUM = "0123456789", cReturn = "";

            for (int i = 0; i < cInput.Length; i++)
            {
                if (cNUM.IndexOf(cInput[i]) != -1)
                    cReturn += cInput[i];
            }
            return cReturn;
        }

        public static string ToXMLString(string cInput)
        {
            cInput = cInput.Replace("<", "&lt;");
            cInput = cInput.Replace(">", "&gt;");
            cInput = cInput.Replace(" & ", "&amp;");
            cInput = cInput.Replace("\"", "&quot;");
            cInput = cInput.Replace("\'", "&apos;");

            return cInput;
        }

        public static string ToPositive(object valueFromDb)
        {
            try
            {
                if (valueFromDb.ToString().ToUpper() == "TRUE") return "1";
                else if (valueFromDb == null) return "-";
                else if (valueFromDb == DBNull.Value) return "-";
                else if (convert.ToInt(valueFromDb) > 0) return convert.ToInt(valueFromDb).ToString();
                else return "-";
            }
            catch { return "-"; }
        }

        public static Boolean CheckForNumericString(string cString)
        {
            Boolean blReturn = true;
            cString = cString.Replace("-", "");

            string cNUM = "0123456789";

            for (int i = 0; i < cString.Length; i++)
            {
                if (cNUM.IndexOf(cString[i]) == -1)
                    return false;
            }

            return blReturn;
        }
    }
}
