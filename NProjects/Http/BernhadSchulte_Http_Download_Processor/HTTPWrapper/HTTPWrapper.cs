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
using HTTPRoutines;


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
        private string _requestMethod = "POST";
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
        public bool IsUrlEncoded { get { return _isurlencoded; } set { _isurlencoded = value; _http.IsUrlEncoded = value; } } //namrata
        public string RequestMethod { get { return _requestMethod; } set { _requestMethod = value; _http.RequestMethod = value; } } //simmy 27.02.2018 

        public string SessionID { get; set; }

        public HTTPWrapper()
        {
            this._SetRequestHeaders = new Dictionary<HttpRequestHeader, string>();
            this._AddRequestHeaders = new Dictionary<string, string>();
            this._dctSetCookie = new Dictionary<string, string>();
            this._dctStateData = new Dictionary<string, string>();
            this._CookieContainer = new CookieContainer();
            this._CurrentDocument = new HtmlDocument();
            this.SetDefaultValues();
        }

        public bool DownloadDocument(string Request, string postData, string DownloadFile, string ContentType = "Application/pdf")
        {
            bool flag;
            try
            {
                _http.DownloadFileContentType = ContentType;
                this._http.DownloadDocument(Request, postData, DownloadFile);
                flag = File.Exists(DownloadFile);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return flag;
        }

        public HtmlNode GetElement(string nodeTitle, string _attributeKey, string _attrValue)
        {
            HtmlNode htmlNode;
            try
            {
                HtmlNode documentNode = this._CurrentDocument.DocumentNode;
                string[] strArrays = new string[] { "//", nodeTitle, "[@", _attributeKey.ToString(), "='", _attrValue, "']" };
                htmlNode = documentNode.SelectSingleNode(string.Concat(strArrays));
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return htmlNode;
        }

        public HtmlNodeCollection GetElementCollection(string nodeTitle, string _attributeKey, string _attrValue)
        {
            HtmlNodeCollection htmlNodeCollection;
            try
            {
                HtmlNode documentNode = this._CurrentDocument.DocumentNode;
                string[] strArrays = new string[] { "//", nodeTitle, "[@", _attributeKey.ToString(), "='", _attrValue, "']" };
                htmlNodeCollection = documentNode.SelectNodes(string.Concat(strArrays));
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return htmlNodeCollection;
        }

        public bool LoadURL(string _request, string validateNodeTitle, string validateAttribute, string attributeValue, string CookieName, string HiddenAttributeKey = "", bool _ReadResponse = true)
        {
            bool flag;
            try
            {
                bool flag1 = false;
                this.SetRequestProperties();
                _http.HiddenAttributeKey=HiddenAttributeKey;
                this._http.SendGetRequest(_request);
                this.Referrer = _request;
                this.SetSessionID(_request, CookieName);
                if (this._SetRequestHeaders.ContainsKey(HttpRequestHeader.Cookie))
                {
                    this._SetRequestHeaders[HttpRequestHeader.Cookie] = string.Concat(CookieName, "=", this.SessionID);
                }
                else
                {
                    this._SetRequestHeaders.Add(HttpRequestHeader.Cookie, string.Concat(CookieName, "=", this.SessionID));
                }
                this._CurrentResponse = this._http._CurrentResponse;
                if (!_ReadResponse)
                {
                    flag1 = true;
                }
                else
                {
                    this._CurrentResponseString = this.ReadResponse(this._http._CurrentResponse);
                    this._CurrentDocument.LoadHtml(this._CurrentResponseString);
                    this._dctStateData = this._http.GetStateInfo(this._CurrentDocument, "");
                    if (!(validateNodeTitle != ""))
                    {
                        flag1 = true;
                    }
                    else
                    {
                        HtmlNode documentNode = this._CurrentDocument.DocumentNode;
                        string[] strArrays = new string[] { "//", validateNodeTitle, "[@", validateAttribute.ToString(), "='", attributeValue, "']" };
                        HtmlNode htmlNode = documentNode.SelectSingleNode(string.Concat(strArrays));
                        flag1 = htmlNode != null;
                    }
                }
                flag = flag1;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return flag;
        }

        public bool PostURL(string _request, string _postData, string validateNodeTitle, string validateAttribute, string attributeValue, string HiddenAttributeKey = "")
        {
            bool flag;
            try
            {
                bool flag1 = false;
                this.SetRequestProperties();
                this._http.SendPostRequest(_request, _postData);
                this._CurrentResponse = _http._CurrentResponse;
                this._CurrentResponseString = this.ReadResponse(this._http._CurrentResponse);
                this._CurrentDocument.LoadHtml(this._CurrentResponseString);
                this.SetSessionID(_request, this._CookieName);
                this._dctStateData = this._http.GetStateInfo(this._CurrentDocument, "");
                if (!(validateNodeTitle != ""))
                {
                    flag1 = true;
                }
                else
                {
                    HtmlNode documentNode = this._CurrentDocument.DocumentNode;
                    string[] strArrays = new string[] { "//", validateNodeTitle, "[@", validateAttribute.ToString(), "='", attributeValue, "']" };
                    HtmlNode htmlNode = documentNode.SelectSingleNode(string.Concat(strArrays));
                    flag1 = htmlNode != null;
                }
                this.Referrer = _request;
                flag = flag1;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return flag;
        }

        public bool PrintScreen(string sFileName)
        {
            bool flag;
            try
            {
                this._http.PrintScreen(sFileName, this._CurrentResponseString);
                flag = File.Exists(sFileName);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return flag;
        }

        private string ReadResponse(HttpWebResponse response)
        {
            string end;
            try
            {
                Stream responseStream = response.GetResponseStream();
                try
                {
                    Stream gZipStream = responseStream;
                    if (response.ContentEncoding.ToLower().Contains("gzip"))
                    {
                        gZipStream = new GZipStream(gZipStream, CompressionMode.Decompress);
                    }
                    else if (response.ContentEncoding.ToLower().Contains("deflate"))
                    {
                        gZipStream = new DeflateStream(gZipStream, CompressionMode.Decompress);
                    }
                    StreamReader streamReader = new StreamReader(gZipStream, Encoding.UTF8);
                    try
                    {
                        end = streamReader.ReadToEnd();
                    }
                    finally
                    {
                        if (streamReader != null)
                        {
                            ((IDisposable)streamReader).Dispose();
                        }
                    }
                }
                finally
                {
                    if (responseStream != null)
                    {
                        ((IDisposable)responseStream).Dispose();
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return end;
        }

        private void SetDefaultValues()
        {
            this._SetRequestHeaders.Add(HttpRequestHeader.CacheControl, "max-age=0");
            this._SetRequestHeaders.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            this._SetRequestHeaders.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");
            this._AddRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
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

        public void SetSessionID(string Request, string CookieName)
        {
            try
            {
                this._dctSetCookie.Clear();
                foreach (Cookie cooky in this._CookieContainer.GetCookies(new Uri(Request)))
                {
                    this._dctSetCookie.Add(cooky.Name, cooky.Value);
                    if (cooky.Name == CookieName)
                    {
                        this.SessionID = cooky.Value;
                        _http.SessionID = this.SessionID;
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
