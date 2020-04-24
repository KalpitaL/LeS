using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetBrowser;
using DotNetBrowser.DOM;
using DotNetBrowser.Events;
using DotNetBrowser.Chromium;
using DotNetBrowser.WinForms;
using System.Data;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.IO;

namespace DotNetBrowserWrapper
{
    public partial class NetBrowser : IDisposable
    {
        public string ChromeDataDir = "";//21-07-2017
        private Browser _browser;
        private Browser printbrowser;
        public WinFormsBrowserView browserView;
        private DOMDocument _document;
        private string _SuppressLoad;
        private int _waitPeriod;
        private static string _logpath;
        public long currentFrameId;
        public bool currentdocloaded;
        public Browser browser
        {
            get { return _browser; }
            set { _browser = value; }
        }

        public string SuppressLoad
        {
            get { return _SuppressLoad; }
            set { _SuppressLoad = value; }
        }

        public string LogPath
        {
            get { return _logpath; }
            set { _logpath = value; }
        }

        public int WaitPeriod
        {
            get { return _waitPeriod; }
            set { _waitPeriod = value; }
        }

        // ManualResetEvent _waitEvent;
        public DOMDocument CurrentDocument
        {
            get { return _document; }
            set { _document = value; }
        }

        public string LogText { set { WriteLog(value); } }

        private void WriteLog(string _logText, string _logFile = "")//101117
        {

            if (_logpath == null || _logpath == "")_logpath= System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].Trim();
            else _logpath = AppDomain.CurrentDomain.BaseDirectory + "Log";
            string _logfile = @"Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (_logFile.Length > 0) { _logfile = _logFile; }
            if (string.IsNullOrEmpty(_logpath)) { _logpath = AppDomain.CurrentDomain.BaseDirectory + "Log"; }
            
            Directory.CreateDirectory(_logpath);

            Console.WriteLine(_logText);
            if (_logText == Environment.NewLine)
                File.AppendAllText(_logpath + @"\" + @_logfile, _logText);
            else
                File.AppendAllText(_logpath + @"\" + @_logfile, DateTime.Now.ToString() + " " + _logText + Environment.NewLine);

        }

        //private void WriteQuoteLog(string _logText, string _logFile)
        //{
        //    if (_logpath == null || _logpath == "") _logpath = "Quote_Log";
        //    string _logfile = @"Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        //    if (_logFile.Length > 0) { _logfile = _logFile; }
        //    Directory.CreateDirectory(_logpath);
        //    Console.WriteLine(_logText);
        //    if (_logText == Environment.NewLine)
        //        File.AppendAllText(_logpath + @"\" + @_logfile, _logText);
        //    else
        //        File.AppendAllText(_logpath + @"\" + @_logfile, DateTime.Now.ToString() + " " + _logText + Environment.NewLine);
        //}

        public virtual void _browser_FinishLoadingFrame(ref bool currentdocloaded, string _validate, bool isClass, bool isName, ManualResetEvent _waitEvent, FinishLoadingEventArgs e)
        {
            try
            {
                if (!currentdocloaded)
                {
                    if (_validate != "")
                    {
                        // if (e.ValidatedURL != "about:blank")                    
                        //{ CurrentDocument = _browser.GetDocument(e.FrameId); }
                        CurrentDocument = _browser.GetDocument(e.FrameId);
                        Thread.Sleep(1000);
                        DOMElement Element;
                        if (isClass) Element = CurrentDocument.GetElementByClassName(_validate);
                        else if (isName) Element = CurrentDocument.GetElementByName(_validate);
                        else Element = CurrentDocument.GetElementById(_validate);
                        if (Element != null)
                        {
                            currentdocloaded = true;
                            currentFrameId = e.FrameId;
                            if (_waitEvent != null) _waitEvent.Set();
                        }
                    }
                    else
                    {
                        if (e.IsMainFrame)
                        {
                            CurrentDocument = _browser.GetDocument(e.FrameId);
                            if (_validate == "")
                            {
                                currentdocloaded = true;
                                currentFrameId = e.FrameId;
                                if (_waitEvent != null) _waitEvent.Set();
                            }

                        }
                    }
                }
            }
            catch (Exception)
            {
                //
            }
        }

        public virtual void _browser_FinishLoadingDoc(ref bool currentdocloaded, string _validate, bool isClass, bool isName, ManualResetEvent _waitEvent, FrameLoadEventArgs e)
        {
            if (!currentdocloaded)
            {
                if (_validate != "")
                {
                    // if (e.ValidatedURL != "about:blank")                    
                    //{ CurrentDocument = _browser.GetDocument(e.FrameId); }
                    CurrentDocument = _browser.GetDocument(e.FrameId);
                    DOMElement Element;
                    if (isClass) Element = CurrentDocument.GetElementByClassName(_validate);
                    else if (isName) Element = CurrentDocument.GetElementByName(_validate);
                    else Element = CurrentDocument.GetElementById(_validate);
                    if (Element != null)
                    {
                        currentdocloaded = true;
                        currentFrameId = e.FrameId;
                        if (_waitEvent != null) _waitEvent.Set();
                    }

                }
            }

        }

        public string DownloadFileOnClick(string _elementid, string _destfolder, string _destfilename, bool reload = false, string _Validate = "")
        {
            PluginManager pluginManager = _browser.PluginManager;
            pluginManager.PluginFilter = new CustomPluginFilter();

            var downloadHandler = new FileDownloadHandler();

            downloadHandler._destfilename = _destfilename;
            downloadHandler._destFolder = _destfolder;
            downloadHandler._waitPeriod = _waitPeriod;


            if (_destfolder != "") { Directory.CreateDirectory(_destfolder); }

            _browser.DownloadHandler = downloadHandler;

            //ClickElementbyID(_elementid, true);

            FinishLoadingFrameHandler eventHandler = null;
            if (reload)
            {
                currentdocloaded = false;
                eventHandler = delegate(object sender, FinishLoadingEventArgs e)
                {
                    _browser_FinishLoadingFrame(ref currentdocloaded, _Validate, false, false, null, e);

                };
                _browser.FinishLoadingFrameEvent += eventHandler;
                ClickElementbyID(_elementid);
            }
            else ClickElementbyID(_elementid);
            downloadHandler.Wait();

            while (!currentdocloaded)
            {
                Thread.Sleep(1000);
            }


            _browser.FinishLoadingFrameEvent -= eventHandler;
            return downloadHandler._destfilename;
        }

        public string DownloadFileOnClick(DOMElement _element, string _destfolder, string _destfilename, bool reload = false, string _Validate = "")
        {
            PluginManager pluginManager = _browser.PluginManager;
            pluginManager.PluginFilter = new CustomPluginFilter();

            var downloadHandler = new FileDownloadHandler();

            downloadHandler._destfilename = _destfilename;
            downloadHandler._destFolder = _destfolder;
            downloadHandler._waitPeriod = _waitPeriod;


            if (_destfolder != "") { Directory.CreateDirectory(_destfolder); }

            _browser.DownloadHandler = downloadHandler;

            //ClickElementbyID(_elementid, true);

            FinishLoadingFrameHandler eventHandler = null;
            if (reload)
            {
                currentdocloaded = false;
                eventHandler = delegate(object sender, FinishLoadingEventArgs e)
                {
                    _browser_FinishLoadingFrame(ref currentdocloaded, _Validate, false, false, null, e);

                };
                _browser.FinishLoadingFrameEvent += eventHandler;
                _element.Click();
            }
            else _element.Click(); ;
            downloadHandler.Wait();

            while (!currentdocloaded)
            {
                Thread.Sleep(1000);
            }


            _browser.FinishLoadingFrameEvent -= eventHandler;
            return downloadHandler._destfilename;
        }

        public bool LoadUrl(string _url, string _validate = "", bool isClass = false, bool isName = false)
        {

            currentdocloaded = false;
            ManualResetEvent _waitEvent = new ManualResetEvent(false);
            FinishLoadingFrameHandler eventHandler = delegate(object sender, FinishLoadingEventArgs e)
            {
                _browser_FinishLoadingFrame(ref currentdocloaded, _validate, isClass, isName, _waitEvent, e);

            };
            DocumentLoadedInFrameHandler eventDocHandler = delegate(object sender, FrameLoadEventArgs ev)
            {
                _browser_FinishLoadingDoc(ref currentdocloaded, _validate, isClass, isName, _waitEvent, ev);

            };

            _browser.FinishLoadingFrameEvent += eventHandler;
            _browser.DocumentLoadedInFrameEvent += eventDocHandler;


            _browser.LoadURL(_url);

            _waitEvent.WaitOne(new TimeSpan(0, 0, 0, 0, _waitPeriod));
            while (_browser.URL == "about:blank") { Thread.Sleep(1000); }//ADDED ON 16-5-2017
            if (!currentdocloaded)
            {
                DOMNode _ele = _browser.GetDocument().GetElementById(_validate);
                if (_ele != null)
                {
                    CurrentDocument = _browser.GetDocument();
                    //CurrentDocument =
                }
            }

            _browser.FinishLoadingFrameEvent -= eventHandler;
            _browser.DocumentLoadedInFrameEvent -= eventDocHandler;


            return currentdocloaded;
        }

        public DOMElement GetElementbyId(string _id)
        {
            DOMElement _result = null;
            _result = CurrentDocument.GetElementById(_id);
            return _result;
        }

        public DOMElement GetElementbyClass(string _id)
        {
            DOMElement _result = null;
            _result = CurrentDocument.GetElementByClassName(_id);
            return _result;
        }

        public DOMElement GetElementByName(string _name)
        {
            DOMElement _result = null;
            _result = CurrentDocument.GetElementByName(_name);
            return _result;
        }

        public DOMElement GetElementByType(string _type,string _value)//added by namrata 27-02-17
        {
            DOMElement _result = null;
            try
            {
            //   DotNetBrowser.DOM.DOMElement _div = GetElementbyId(_id);
                var inputs = CurrentDocument.GetElementsByTagName(_type);
              //  var length = inputs.Count;
              
               foreach (DOMElement i in inputs)
               {
                   if (i.Attributes["type"] == _value)
                   {
                       _result = i;
                   }
               }
            }
            catch (Exception)
            { }
            return _result;
        }

        public DOMElement GetElementByType(string _type, string _value,string AttributeValue)//added by namrata 27-02-17
        {
            DOMElement _result = null;
            try
            {
                //   DotNetBrowser.DOM.DOMElement _div = GetElementbyId(_id);
                var inputs = CurrentDocument.GetElementsByTagName(_type);
                //  var length = inputs.Count;

                foreach (DOMElement i in inputs)
                {
                    if (i.HasAttribute(AttributeValue))
                    {
                        if (i.Attributes[AttributeValue] == _value)
                        {
                            _result = i;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            { }
            return _result;
        }

        public DOMInputElement GetInputElementByTitle(string _type, string _value,DOMElement _element)//added by namrata 27-02-17
        {
            DOMInputElement _result = null;
            try
            {
                var inputs = _element.GetElementsByTagName(_type);
                //  var length = inputs.Count;

                foreach (DOMInputElement i in inputs)
                {
                    if (i.Attributes["title"].ToString() == _value)
                    {
                        _result = i;
                        break;
                    }
                }
            }
            catch (Exception)
            { }
            return _result;
        }

        public DOMElement GetElementByTitle(string _type, string _value, DOMElement _element)//added by namrata 27-02-17
        {
            DOMElement _result = null;
            try
            {
                var inputs = _element.GetElementsByTagName(_type);

                foreach (DOMElement i in inputs)
                {
                    if (i.HasAttribute("Title"))
                    {
                        if (i.Attributes["title"].ToString() == _value)
                        {
                            _result = i;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            { }
            return _result;
        }

        public DOMElement GetElementByType(string _type, DOMElement _element)//added by namrata 27-02-17
        {
            DOMElement _result = null;
            try
            {
                var _spans = _element.GetElementsByTagName(_type);

                foreach (DOMElement s in _spans)
                {
                    _result = s;
                }
             }
            catch (Exception)
            { }
            return _result;
        }

        public void FireEvent(DOMInputElement _ele, string _event)
        {
           
            if (_event.ToUpper() == "CHANGE")
            {
            }
        }

        //public DOMElement GetElement_By_aria_labelledby(string _type, string _value)
        //{
        //    DOMElement _result = null;
        //    try
        //    {
        //        var inputs = _element.GetElementsByTagName(_type);
        //        //  var length = inputs.Count;

        //        foreach (DOMElement i in inputs)
        //        {
        //            if (i.HasAttribute("Title"))
        //            {
        //                if (i.Attributes["title"].ToString() == _value)
        //                {
        //                    _result = i;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    { }
        //    return _result;
        //}

        public bool SetValuebyId(string _id, string _value)
        {
            bool _result = true; DOMElement _element = null;
            // _element = _browser.GetDocument().GetElementById(_id);
            _element = CurrentDocument.GetElementById(_id);
            if (_element != null && _element is DOMInputElement)
            { ((DOMInputElement)_element).Value = _value; }
            else { _result = false; LogText = "Unable to find element : " + _id; }
            return _result;
        }

        public bool SetValuebyName(string _id, string _value)//added by namrata 0n 25-2-17
        {
            bool _result = true; DOMElement _element = null;
            // _element = _browser.GetDocument().GetElementById(_id);
            _element = CurrentDocument.GetElementByName(_id);
            if (_element != null && _element is DOMInputElement)
            { ((DOMInputElement)_element).Value = _value; }
            else { _result = false; LogText = "Unable to find element : " + _id; }
            return _result;
        }

        public bool SetValuebyElement(DOMElement _element, string _value)//added by namrata 0n 1-3-17
        {
            bool _result = true;
            if (_element != null && _element is DOMInputElement)
            { ((DOMInputElement)_element).Value = _value; }
           
            else { _result = false; LogText = "Unable to find element : " + _element; }
            return _result;
        }
     
        public bool SetTextAreaValuebyId(string _id, string _value)//added by namrata 0n 8-2-17
        {
            bool _result = true; DOMElement _element = null;
            _element = CurrentDocument.GetElementById(_id);
            if (_element != null && _element is DOMTextAreaElement)
            { ((DOMTextAreaElement)_element).Value = _value; }
            else { _result = false; LogText = "Unable to find element : " + _id; }
            return _result;
        }

        public bool SetFile(string _id, string[] _value)
        {
            bool _result = true; DOMElement _element = null;
            _element = CurrentDocument.GetElementById(_id);
            if (_element != null && _element is DOMInputElement)
            { ((DOMInputElement)_element).SetFile(_value); }
            else { _result = false; LogText = "Unable to find element : " + _id; }
            return _result;
        }

        public string SetDate(DOMInputElement _element, string value)
        {
          _element.SetAttribute("datepicker", value);
               string a =_element.GetAttribute("datepicker");
               return a;
        }

        public bool SetFile(string _id, string _value, bool idIsClass = false, bool idIsName = false) 
        {
            bool _result = true; DOMElement _element = null;

            //Changed Sayli
            //_element = CurrentDocument.GetElementById(_id);
            if (idIsClass) _element = CurrentDocument.GetElementByClassName(_id);
            else if (idIsName) _element = CurrentDocument.GetElementByName(_id);
            else _element = CurrentDocument.GetElementById(_id);
            //

            if (_element != null && _element is DOMInputElement)
            { ((DOMInputElement)_element).SetFile(_value); }
            else { _result = false; LogText = "Unable to find element : " + _id; }
            return _result;
        }

        public bool ClickElement_Reload(DOMElement _element, string _validate = "", bool isClass = false, bool isName = false)
        {
            bool _result = true;

            ManualResetEvent _waitEvent = new ManualResetEvent(false);
            bool currentdocloaded = false;

            FinishLoadingFrameHandler eventHandler = delegate(object sender, FinishLoadingEventArgs e)
            {
                // LogText = e.ValidatedURL;
                _browser_FinishLoadingFrame(ref currentdocloaded, _validate, isClass, isName, _waitEvent, e);
            };
            _browser.FinishLoadingFrameEvent += eventHandler;
            _element.Click();
            _waitEvent.WaitOne(_waitPeriod);
             _browser.FinishLoadingFrameEvent -= eventHandler;
            return _result;
        }

        public bool ClickElementbyName(string _id, bool ReLoad, string _validate = "", bool isClass = false, bool isName = false)
        {
            bool _result = true;
            DOMElement _element = CurrentDocument.GetElementByName(_id);
            if (_element == null) _element = browser.GetDocument(currentFrameId).GetElementByName(_id);
            if (_element == null) _element = browser.GetDocument().GetElementByName(_id);

            //  DOMElement _element = browser.GetDocument().GetElementByName(_id);
            if (_element == null) { return false; }
            if (ReLoad)
            {
                ManualResetEvent _waitEvent = new ManualResetEvent(false);
                bool currentdocloaded = false;

                FinishLoadingFrameHandler eventHandler = delegate(object sender, FinishLoadingEventArgs e)
                {
                    LogText = e.ValidatedURL;
                    _browser_FinishLoadingFrame(ref currentdocloaded, _validate, isClass, isName, _waitEvent, e);
                };
                _browser.FinishLoadingFrameEvent += eventHandler;
                _element.Click();
                _waitEvent.WaitOne(_waitPeriod);
                _browser.FinishLoadingFrameEvent -= eventHandler;
            }
            else _element.Click();


            return _result;
        }

        public bool ClickElementbyClass(string _id)
        {
            bool _result = true;
            DOMElement _element = GetElementbyClass(_id);
            if (_element == null) { return false; }
            _element.Click();
            return _result;
        }

        public bool ClickElementbyID(string _id)
        {
            bool _result = true;
            DOMElement _element = GetElementbyId(_id);
            if (_element == null) { return false; }
            _element.Click();

            return _result;
        }

        public bool ClickElementbyID(string _id, bool reLoadDoc, bool hasDialog)
        {
            bool _result = true;
            DOMElement _element = GetElementbyId(_id);
            CustomDialogHandler _CustomDialogHandler = null;
            bool docloaded = false;
            if (_element == null) { return false; }
            if (hasDialog)
            {
                _CustomDialogHandler = new CustomDialogHandler((Control)browserView, WindowsFormsSynchronizationContext.Current);
                _browser.DialogHandler = _CustomDialogHandler;
            }
            if (reLoadDoc)
            {

                DocumentLoadedInFrameHandler eventHandler = delegate(object sender, FrameLoadEventArgs e)
                {
                    CurrentDocument = browser.GetDocument(e.FrameId);
                    docloaded = true;
                };
                _browser.DocumentLoadedInFrameEvent += eventHandler;
            }

            _element.Click();
            if (hasDialog) _CustomDialogHandler.Wait();

            while (!docloaded)
            {
                Thread.Sleep(1000);

            }



            return _result;
        }

        public bool ClickElementbyID(string _id, bool ReLoad, string _validate = "", bool isClass = false, bool isName = false)
        {

            DOMElement _element = GetElementbyId(_id);
            if (_element == null) { return false; }
            if (ReLoad)
            {
                ManualResetEvent _waitEvent = new ManualResetEvent(false);
                currentdocloaded = false;

                FinishLoadingFrameHandler eventHandler = delegate(object sender, FinishLoadingEventArgs e)
                {
                    LogText = e.ValidatedURL;
                    _browser_FinishLoadingFrame(ref currentdocloaded, _validate, isClass, isName, _waitEvent, e);
                };
                _browser.FinishLoadingFrameEvent += eventHandler;
                _element.Click();

                _waitEvent.WaitOne(_waitPeriod);
                if (!currentdocloaded)
                {
                    DOMNode _ele = _browser.GetDocument().GetElementById(_validate);
                    if (_ele != null)
                    {
                        CurrentDocument = _browser.GetDocument();
                        currentdocloaded = true;
                        //CurrentDocument =
                    }
                }

                _browser.FinishLoadingFrameEvent -= eventHandler;
            }
            else _element.Click();


            return currentdocloaded;
        }

        public bool ClickElementbyID(string _id, string[] _ignoreURL, string SuppressURL = "", string _validate = "", bool isClass = false, bool isName = false)
        {

            DOMElement _element = GetElementbyId(_id);
            if (_element == null) { return false; }


            var customLoadHandler = new CustomLoadHandler();

            CustomResponseHandler _customResponseHandler = delegate(object sender, CustomResponseEventArgs e)
            {
                _browser.Stop();
                _browser.LoadURL(e.Url);
            };

            customLoadHandler.CustomResponseEvent += _customResponseHandler;
            customLoadHandler.ignoreURL = _ignoreURL;
            customLoadHandler.SuppressLoad = SuppressURL;
            _browser.LoadHandler = customLoadHandler;

            bool currentdocloaded = false;
            ManualResetEvent _waitEvent = new ManualResetEvent(false);
            FinishLoadingFrameHandler eventHandler = delegate(object sender, FinishLoadingEventArgs e)
            {
                LogText = e.ValidatedURL;
                _browser_FinishLoadingFrame(ref currentdocloaded, _validate, isClass, isName, _waitEvent, e);
            };
            _browser.FinishLoadingFrameEvent += eventHandler;
            _element.Click();
            _waitEvent.WaitOne(_waitPeriod);
            _browser.FinishLoadingFrameEvent -= eventHandler;
            customLoadHandler.CustomResponseEvent -= _customResponseHandler;
            return currentdocloaded;
        }

        public bool ClickElementbyID(DOMElement _element, string[] _ignoreURL, string SuppressURL = "", string _validate = "", bool isClass = false, bool isName = false)//added by namrata 0n 25-2-17
        {

           
            if (_element == null) { return false; }


            var customLoadHandler = new CustomLoadHandler();

            CustomResponseHandler _customResponseHandler = delegate(object sender, CustomResponseEventArgs e)
            {
                _browser.Stop();
                _browser.LoadURL(e.Url);
            };

            customLoadHandler.CustomResponseEvent += _customResponseHandler;
            customLoadHandler.ignoreURL = _ignoreURL;
            customLoadHandler.SuppressLoad = SuppressURL;
            _browser.LoadHandler = customLoadHandler;

            bool currentdocloaded = false;
            ManualResetEvent _waitEvent = new ManualResetEvent(false);
            FinishLoadingFrameHandler eventHandler = delegate(object sender, FinishLoadingEventArgs e)
            {
                LogText = e.ValidatedURL;
                _browser_FinishLoadingFrame(ref currentdocloaded, _validate, isClass, isName, _waitEvent, e);
            };
            _browser.FinishLoadingFrameEvent += eventHandler;
            _element.Click();
            _waitEvent.WaitOne(_waitPeriod);
            _browser.FinishLoadingFrameEvent -= eventHandler;
            customLoadHandler.CustomResponseEvent -= _customResponseHandler;
            return currentdocloaded;
        }

       public string ExecuteJavaScript(string FunctionName, object[] Value)
        {
            string sReturnValue = "";
            JSValue returnValue = _browser.ExecuteJavaScriptAndReturnValue(FunctionName);
            // Make sure that return value is a function.
            if (returnValue.IsFunction())
            {
                //  string val = "";
                JSValue result = returnValue.AsFunction().InvokeAndReturnValue(null, Value);
                if (result.IsString())
                {
                    sReturnValue = result.AsString().GetString();
                }
            }
            else sReturnValue = "Function not found";
            return sReturnValue;
        }

        public string ExecuteJavaScript(string FunctionName, string Value)
        {
            string sReturnValue = "";
            JSValue returnValue = _browser.ExecuteJavaScriptAndReturnValue(FunctionName);
            // Make sure that return value is a function.
            if (returnValue.IsFunction())
            {
                // string val = "";

                JSValue result = returnValue.AsFunction().InvokeAndReturnValue(null, Value);
                if (result.IsString())
                {
                    sReturnValue = result.AsString().GetString();
                }
            }
            else sReturnValue = "Function not found";
            return sReturnValue;
        }

        public bool WaitForElementbyId(string _id)
        {
            //bool _result = true;
            bool _result = false; //simmy
            int _wait = 5; int i = 0;
            if (_waitPeriod > 0) { _wait = _waitPeriod; }
            _result = (GetElementbyId(_id) != null);

            while (!_result)
            {
                _result = (GetElementbyId(_id) != null);
                i = i + 1;
              //  System.Threading.Thread.Sleep(1000);
            //    LogText = "  Waiting " + i.ToString();

                if (i > _wait) { LogText = "Unable to locate " + _id; break; }
                // else { break; }
            }

            if (_result) CurrentDocument = browser.GetDocument();
            return _result;
        }

        public bool WaitForElementbyId(string _id, bool isClass, bool isName)
        {
            //bool _result = true;
            bool _result = false; //simmy
            int _wait = 5; int i = 0;
            if (_waitPeriod > 0) { _wait = _waitPeriod; }
            if (isClass) _result = (GetElementbyClass(_id) != null);
            else if (isName) _result = (CurrentDocument.GetElementByName(_id) != null);
            else _result = (GetElementbyId(_id) != null);

            while (!_result)
            {
                if (isClass) _result = (GetElementbyClass(_id) != null);
                else if (isName) _result = (CurrentDocument.GetElementByName(_id) != null);
                else _result = (GetElementbyId(_id) != null);
                i = i + 1;
             //   System.Threading.Thread.Sleep(1000);
               // LogText = "  Waiting " + i.ToString();

                if (i > _wait) { LogText = "Unable to locate " + _id; break; }
                // else { break; }
            }

            if (_result) CurrentDocument = browser.GetDocument();
            return _result;
        }

        public bool WaitForLoading(string nClass,string attribute,string attValue,string GridClass)
        {
            bool _result = false; //simmy
            int _wait = 5; int i = 0;
            if (_waitPeriod > 0) { _wait = _waitPeriod; }
           DOMElement ele = GetElementbyClass(nClass);
           if (ele != null)
           {
               while (!_result)
               {
                   string a = ele.GetAttribute(attribute);
                   if (a == attValue)
                   {
                       DOMElement AllRFQTable = GetElementbyClass(GridClass);//15-12-2017
                       if (AllRFQTable != null)
                       {
                           DOMElement eletbody = AllRFQTable.GetElementByTagName("tbody");
                           if (eletbody != null)
                           {
                               List<DOMNode> eleRow = eletbody.GetElementsByTagName("tr");
                               if (eleRow.Count > 0)
                               {
                                   _result = true;
                               }
                               else _result = false;
                           }
                       }
                       else _result = false;
                       i = i + 1;
                       if (i > _wait) { LogText = "Unable to locate " + nClass; break; }
                   }
               }
           }
           if (_result) CurrentDocument = browser.GetDocument();
           return _result;
           }

        public DataTable ConvertHTMLTabletoDataTable(string _idTable, string _idHeaderRowid = "", int _ColCount = 0, string _idPager = "", string _idNext = "")
        {

            DataTable _result = new DataTable();

            DotNetBrowser.DOM.DOMElement _table = GetElementbyId(_idTable);
            DotNetBrowser.DOM.DOMElement _tbody = _table.GetElementByTagName("tbody");
            List<DOMNode> _Cols; int Colcount = 0;
            if (_idHeaderRowid == "")
            {
                _Cols = _table.GetElementsByTagName("th");
            }
            else { _Cols = GetNodeList(_idHeaderRowid, "td"); }

            if (_Cols.Count == 0) Colcount = _ColCount;
            else Colcount = _Cols.Count;
            // string head = "";
            for (int i = 0; i < Colcount; i++)
            {
                // LogText = _Cols[i].TextContent;
                _result.Columns.Add();
            }


            if (_tbody != null)
            {
                int i = 0;
                List<DOMNode> _rows = _tbody.GetElementsByTagName("tr");
                foreach (DOMNode _row in _tbody.GetElementsByTagName("tr"))
                {
                    DataRow newrec = _result.NewRow();
                    _result.Rows.Add(newrec);

                    int j = 0;

                    if (_row.NodeType == DOMNodeType.ElementNode)
                    {
                        int p = _row.Children.Count;
                        // MessageBox.Show(_row.Children[0].TextContent);
                        List<DOMNode> cols = null;
                        //DOMElement _rowele = (DOMElement)_row;
                        if (p == 1)
                        {
                            cols = CurrentDocument.GetElementById(_idTable).GetElementByTagName("tbody").GetElementsByTagName("td");
                        }
                        else
                        {
                            cols = _row.GetElementsByTagName("td");
                        }



                        foreach (DOMNode _col in cols) //browser.GetDocument().GetElementById(_idTable).GetElementByTagName("tbody").GetElementsByTagName("td"));//_row.GetElementsByTagName("td"))
                        {

                            if (_col.NodeType == DOMNodeType.ElementNode)
                            {
                                // DOMElement _colElement = (DOMElement)_col;
                                if (_col != null && _col.Children.Count > 0)
                                {
                                    DOMElement _cdiv = ((DOMElement)(_col.Children[0]));
                                    // _result.Rows[i][j] = _colElement.TextContent; // _col.TextContent;
                                    _result.Rows[i][j] = _cdiv.InnerText;

                                    //  LogText = ((DOMElement)(_col.Children[0])).InnerText;
                                }
                            }
                            j = j + 1;
                        }
                    }

                    i = i + 1;
                }
            }

            while (!GetNextPage(_idPager)) //simmy
            {
                DataTable _ds = ConvertHTMLTabletoDataTable(_idTable, _idHeaderRowid);
                _result.Merge(_ds);
            }

            return _result;
        }


        public bool GetNextPage(string _PageElementId) //simmy
        {
            bool _lastpage = true;

            //  DotNetBrowser.DOM.DOMElement _pager = GetElementbyId("untreatedProposals_paginate");
            DotNetBrowser.DOM.DOMElement _pager = GetElementbyId(_PageElementId);
            _lastpage = (_pager != null);
            if (_pager != null)
            {
                DotNetBrowser.DOM.DOMElement _ul = GetElementbyClass("next");

                if (_ul != null)
                {
                    LogText = "GotPager";
                    _lastpage = (_ul.GetAttribute("class").Contains("disable"));
                    if (_lastpage) { LogText = "Last Page"; }
                    while (!_lastpage)
                    {
                        _ul.Click();
                        _lastpage = (_ul.GetAttribute("class").Contains("disable"));

                        if (_lastpage) { LogText = "Last Page"; }
                    }
                }
                else { LogText = "No Pager"; }
            }
            return _lastpage;
        }

        public bool WaitForPrintComplete(string sFilename)
        {
            bool isComplete = false;

            int _wait = 60; int i = 0;

            isComplete = File.Exists(sFilename);

            while (!isComplete)
            {
                isComplete = File.Exists(sFilename);
                i = i + 1;
                System.Threading.Thread.Sleep(1000);

                if (i > _wait) { break; }
                // else { break; }
            }

            LogText = sFilename + " " + isComplete.ToString();

            return isComplete;
        }

        public bool PrintTable(string Tableid, string HeaderRow, string PagingElementid, string styleTag = "")
        {
            bool isComplete = false;
            string _HTML = "<html><body><table><tbody>";
            string cHTML = styleTag + "</tbody></table></body></html>";


            DotNetBrowser.DOM.DOMElement _table = GetElementbyId(Tableid);
            DotNetBrowser.DOM.DOMElement _tbody = _table.GetElementByTagName("tbody");


            if (_tbody != null)
            {
                GetRows(_tbody, ref _HTML);
            }


            DOMElement _Pager = browser.GetDocument().GetElementById(PagingElementid);
            List<DOMNode> _PageItems = _Pager.GetElementsByTagName("a");
            bool _lastpage = false;

            while (!_lastpage)
            {



                object[] _obj = new object[2];
                _obj[0] = "ctl00_body_parentCallbackPanel_ParentGrid_appGrid";
                _obj[1] = "PBN";
                DOMElement _current = _Pager.GetElementByClassName("dxp-current");
                DOMElement _current1 = _Pager.GetElementByClassName("dxp-current");
                ExecuteJavaScript("ASPx.GVPagerOnClick", _obj);
                while (_current.TextContent == _current1.TextContent)
                {
                    _Pager = browser.GetDocument().GetElementById(PagingElementid);
                    _current1 = _Pager.GetElementByClassName("dxp-current");
                    Thread.Sleep(1000);
                }
                LogText = "Reading Page : " + _current1.TextContent;
                DotNetBrowser.DOM.DOMElement _table1 = GetElementbyId(Tableid);
                DotNetBrowser.DOM.DOMElement _tbody1 = _table1.GetElementByTagName("tbody");
                GetRows(_tbody1, ref _HTML);
                LogText = "Page : " + _current1.TextContent + " completed.";
                List<DOMNode> _ul = browser.GetDocument().GetElementsByClassName("dxp-disabledButton");
                if (_ul.Count > 1) _lastpage = true;
                else if (_ul.Count == 1)
                {
                    _lastpage = (!((DOMElement)_ul[0]).GetAttribute("class").Contains("dxp-lead"));
                }

            }

            LogText = "Last Page found";
            _HTML += cHTML;
            if (printbrowser != null && !printbrowser.IsDisposed()) printbrowser.Dispose();

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\TempFiles")) Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\TempFiles");
            BrowserContextParams params2 = new BrowserContextParams(AppDomain.CurrentDomain.BaseDirectory + "\\TempFiles");
            BrowserContext context2 = new BrowserContext(params2);
            printbrowser = BrowserFactory.Create(context2);
            ManualResetEvent _waitEvent = new ManualResetEvent(false);
            FinishLoadingFrameHandler eventHandler2 = delegate(object sender, FinishLoadingEventArgs e)
            {
                // Wait until main document of the web page is loaded completely.
                if (e.IsMainFrame)
                {
                    _waitEvent.Set();
                }
            };


            printbrowser.FinishLoadingFrameEvent += eventHandler2;
            printbrowser.LoadHTML(_HTML);
            _waitEvent.WaitOne(_waitPeriod);
            printbrowser.FinishLoadingFrameEvent -= eventHandler2;
            string sFileName = AppDomain.CurrentDomain.BaseDirectory + "\\TempFiles\\temp_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".mhtml";
            SaveWebPage(sFileName, AppDomain.CurrentDomain.BaseDirectory + "\\TempFiles\\" + DateTime.Now.ToString("ddMMyyyy"));

            return isComplete;
        }

        public void SaveWebPage(string sFile, string resourcePath)
        {
            _browser.SaveWebPage(sFile, resourcePath, SavePageType.MHTML);
            WaitForPrintComplete(sFile);
            Thread.Sleep(3000);
        }

        public void GetScreenImage(string sFile, ImageFormat _ImageFormat)
        {

            if (!Directory.Exists(Path.GetDirectoryName(sFile))) Directory.CreateDirectory(Path.GetDirectoryName(sFile));
            SetScreenSize();
            browserView.GetImage().Save(sFile, _ImageFormat);
            WaitForPrintComplete(sFile);
            Thread.Sleep(3000);
        }

        private void SetScreenSize()
        {
            JSValue documentHeight = browser.ExecuteJavaScriptAndReturnValue(
                   "Math.max(document.body.scrollHeight, " +
                   "document.documentElement.scrollHeight, document.body.offsetHeight, " +
                   "document.documentElement.offsetHeight, document.body.clientHeight, " +
                   "document.documentElement.clientHeight);");
            JSValue documentWidth = browser.ExecuteJavaScriptAndReturnValue(
                    "Math.max(document.body.scrollWidth, " +
                    "document.documentElement.scrollWidth, document.body.offsetWidth, " +
                    "document.documentElement.offsetWidth, document.body.clientWidth, " +
                    "document.documentElement.clientWidth);");

            int scrollBarSize = 25;
            int viewWidth = (int)documentWidth.GetNumber() + scrollBarSize;
            int viewHeight = (int)documentHeight.GetNumber() + scrollBarSize;

            browserView.Browser.SetSize(viewWidth, viewHeight);

        }

        private void GetRows(DOMElement _tbody, ref string _HTML)
        {
            List<DOMNode> _rows = _tbody.GetElementsByTagName("tr");
            foreach (DOMNode _row in _tbody.GetElementsByTagName("tr"))
            {
                _HTML += "<tr>" + ((DOMElement)_row).InnerHTML + "</tr>";
            }
        }

        public void Dispose()
        {
            try
            {

                browserView.Dispose();
                _browser.Dispose();

                if (printbrowser != null && !printbrowser.IsDisposed())
                {
                    printbrowser.Dispose();
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public NetBrowser()
        {
            try
            {
                ChromeDataDir = AppDomain.CurrentDomain.BaseDirectory + "ChromeData";//\\ 
                BrowserContextParams _params = new BrowserContextParams(ChromeDataDir);
                BrowserContext _context = new BrowserContext(_params);
                 _browser = BrowserFactory.Create(_context);
                // _browser = BrowserFactory.Create(BrowserType.HEAVYWEIGHT);
                browserView = new WinFormsBrowserView(_browser);
                browserView.Browser.SetSize(1500, 500);
                _waitPeriod = -1;
            }
            catch (Exception ex)
            {
                //this.LogText = "Exception at NetBrowser Consturctor: " + ex.Message.ToString();
                this.LogText = "Exception at NetBrowser Constructor: " + ex.Message.ToString();
            }
        }

        public string DownloadFile(string _url, string _destfolder = "", string _destfilename = "")
        {
            PluginManager pluginManager = browser.PluginManager;
            pluginManager.PluginFilter = new CustomPluginFilter();

            var downloadHandler = new FileDownloadHandler();

            downloadHandler._destfilename = _destfilename;
            downloadHandler._destFolder = _destfolder;


            if (_destfolder != "") { Directory.CreateDirectory(_destfolder); }

            browser.DownloadHandler = downloadHandler;

            browser.LoadURL(@_url);
            downloadHandler.Wait();

            return downloadHandler._destfilename;
        }

        public List<DOMNode> GetNodeList(string _ID, string TagName)
        {
            DOMElement _table = GetElementbyId(_ID);
            List<DOMNode> _Cols = _table.GetElementsByTagName(TagName);
            return _Cols;
        }

        public void PrintPDF(string sFileName, bool isSetScrrenSize)
        {
            if (isSetScrrenSize)
                SetScreenSize();
            ManualResetEvent waitEvent = new ManualResetEvent(false);

            var handler = new MyPDFPrintHandler((printJob) =>
            {
                var printSettings = printJob.PrintSettings;
                printSettings.PrintToPDF = true;
                printSettings.PDFFilePath = sFileName;
                // printSettings.PaperSize = PaperSize.ISO_A3;
                printSettings.Landscape = false;
                printSettings.PageMargins = new PageMargins(20, 40, 40, 20);//added by namrata on 8-2-17
                printJob.PrintJobEvent += (s, e) =>
                {
                    System.Diagnostics.Debug.WriteLine("Printing done: " + e.Success);
                    waitEvent.Set();
                };
                return printSettings;
            });


            browser.PrintHandler = handler;
            browser.Print();
            waitEvent.WaitOne(_waitPeriod);
        }

        public bool SelectElementItem(string elementID, int selectIndex)
        {
            bool success = false;
            try
            {
                DOMElement selectElement = GetElementbyId(elementID);
                if (selectElement != null)
                {
                    List<DOMNode> children = selectElement.Children;
                    List<DOMElement> options = new List<DOMElement>();
                    foreach (DOMNode child in children)
                    {
                        if (child is DOMElement)
                        {
                            options.Add((DOMElement)child);
                        }
                    }
                    DOMElement option = options[selectIndex];
                    option.SetAttribute("selected", "selected");
                    success = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return success;
        }

        //public bool SelectElementItem(string elementID,string attribute, int selectIndex)
        //{
        //    bool success = false;
        //     DOMElement selectElement=null;
        //    try
        //    {
        //        if(attribute=="id")
        //         selectElement = GetElementbyId(elementID);
        //        else if(attribute=="class")
        //            selectElement =GetElementbyClass(elementID);
        //        if (selectElement != null)
        //        {
        //            List<DOMNode> children = selectElement.Children;
        //            List<DOMElement> options = new List<DOMElement>();
        //            foreach (DOMNode child in children)
        //            {
        //                if (child is DOMElement)
        //                {
        //                    options.Add((DOMElement)child);
        //                }
        //            }
        //            DOMElement option = options[selectIndex];
        //            option.SetAttribute("selected", "selected");
        //            selectElement.SetAttribute("value", "All");
        //            selectElement.AddEventListener("change",handler,false);
        //            success = true;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    return success;
        //}

        public bool CheckElementById(string elementID, bool Check)
        {
            bool success = false;
            try
            {
                DOMInputElement checkElement = (DOMInputElement)GetElementbyId(elementID);

                checkElement.Checked = Check;
            }
            catch (Exception e)
            {
                throw e;
            }
            return success;
        }

        public DOMElement GetElementFromFrames(string sElement, bool isClass, bool isName, bool resetCurrDoc)
        {
            List<long> _Frames = browser.GetFramesIds();
            DOMElement _ele = null;

            foreach (long frameid in _Frames)
            {
                if (isClass) _ele = browser.GetDocument(frameid).GetElementByClassName(sElement);
                else if (isName) _ele = browser.GetDocument(frameid).GetElementByName(sElement);
                else _ele = browser.GetDocument(frameid).GetElementById(sElement);

                if (_ele != null)
                {
                    if (resetCurrDoc) CurrentDocument = browser.GetDocument(frameid);
                    break;
                }
            }
            return _ele;
        }

        #region CustomLoadHandler
        public class CustomResponseEventArgs : EventArgs
        {
            public string Url { get; private set; }

            public CustomResponseEventArgs(string url)
            {
                Url = url;
            }
        }

        delegate void CustomResponseHandler(object sender, CustomResponseEventArgs e);

        private class CustomLoadHandler : DefaultLoadHandler
        {
            public event CustomResponseHandler CustomResponseEvent;

            private string[] _igURL;
            public List<string> _slURL = new List<string>();

            public string[] ignoreURL
            {
                get { return _igURL; }
                set
                {
                    _igURL = new string[value.Length];
                    value.CopyTo(_igURL, 0);
                }
            }
            private string suppLoad;

            public string SuppressLoad
            {
                get { return suppLoad; }
                set
                {
                    suppLoad = value;
                    _slURL.Clear();
                    if (suppLoad != "") _slURL.AddRange(suppLoad.Split(',').ToList());

                }
            }

            public override bool OnLoad(LoadParams loadParams)
            {
                bool sReturn = false;
                if (ignoreURL != null && ignoreURL.Length > 0)
                {
                    foreach (string sURL in ignoreURL)
                    {
                        if (loadParams.Url.IndexOf(sURL.Split('|')[0]) > -1)
                        {
                            if (sURL.Split('|').Length == 2)
                            {
                                var customResponseEvent = CustomResponseEvent;
                                if (customResponseEvent != null)
                                {
                                    customResponseEvent.Invoke(this, new CustomResponseEventArgs(sURL.Split('|')[1]));
                                }
                                break;
                            }
                            return true;
                        }
                    }
                }
                if (_slURL.Any(loadParams.Url.Contains))
                {
                    return true;
                }

                return sReturn;
            }
        }

        #endregion

    }

    public partial class FileDownloadHandler : DownloadHandler
    {
        public string _destFolder = "";
        public string _destfilename = "";
        public string _destfileprefix = "";
        public bool _downloaded = false;
        public int _waitPeriod = -1;

        private ManualResetEvent waitEvent = new ManualResetEvent(false);

        public bool AllowDownload(DownloadItem download)
        {

            if (@_destFolder != "")
            {
                string _filename = Path.GetFileName(download.DestinationFile);
                string sPath = Path.GetDirectoryName(download.DestinationFile);


                if (_destfileprefix != "") { _filename = _destfileprefix + "\\" + _filename; }

                if (_destfilename != "")
                {
                    download.DestinationFile = _destFolder + "\\" + _destfilename;
                    if (Path.GetExtension(_destfilename) == "") download.DestinationFile = _destFolder + "\\" + _destfilename + Path.GetExtension(_filename);
                }
                else download.DestinationFile = @_destFolder + "\\" + _filename;



            }
            else if (_destfilename != "") { download.DestinationFile = Path.GetDirectoryName(download.DestinationFile) + _destfilename; }
            download.DownloadEvent += delegate(object sender, DownloadEventArgs e)
            {
                DownloadItem downloadItem = e.Item;


                if (downloadItem.Completed)
                {
                    _downloaded = true;
                    _destfilename = Path.GetFileName(downloadItem.DestinationFile);
                    waitEvent.Set();
                }
            };
            return true;
        }

        public void Wait()
        {
            waitEvent.WaitOne(_waitPeriod);
        }
    }

    partial class CustomPluginFilter : PluginFilter
    {
        public bool IsPluginAllowed(PluginInfo pluginInfo)
        {
            if (pluginInfo.Name == "Chromium PDF Viewer") return false;
            else return true;
        }
    }

    class MyPDFPrintHandler : PrintHandler
    {

        Func<PrintJob, PrintSettings> func;

        public MyPDFPrintHandler(Func<PrintJob, PrintSettings> func)
        {
            this.func = func;
        }
        public PrintStatus OnPrint(PrintJob printJob)
        {
            PrintSettings printSettings = func(printJob);
            printSettings.PrintToPDF = true;
            printSettings.Landscape = true;
            printSettings.PrintBackgrounds = true;

            return PrintStatus.CONTINUE;
        }
    }


    partial class MyDialogHandler : WinFormsDefaultDialogHandler
    {
        public MyDialogHandler(Control component)
            : base(component)
        { }

        public override void OnAlert(DialogParams parameters)
        {

        }
    }

    class CustomDialogHandler : DialogHandler
    {
        public string sFileName = "";
        private WinFormsDefaultDialogHandler handler;
        private Control control;
        private SynchronizationContext synchronizationContext;
        private ManualResetEvent waitEvent = new ManualResetEvent(false);
        public bool onWait = false;
        public CloseStatus _closeStatus = CloseStatus.OK;

        public CustomDialogHandler(Control control, System.Threading.SynchronizationContext synchronizationContext)
        {
            this.control = control;
            this.synchronizationContext = synchronizationContext;
            handler = new WinFormsDefaultDialogHandler(control);
        }

        public CustomDialogHandler()
        {
            //
        }

        public void Wait()
        {

            waitEvent.WaitOne();
        }

        public void OnAlert(DialogParams parameters)
        {
            parameters.Status = _closeStatus;
            waitEvent.Set();

        }

        public CloseStatus OnBeforeUnload(UnloadDialogParams parameters)
        {
            return handler.OnBeforeUnload(parameters);
        }

        public CloseStatus OnConfirmation(DialogParams parameters)
        {
            return CloseStatus.OK;
            // return handler.OnConfirmation(parameters);
        }

        public CloseStatus OnFileChooser(FileChooserParams parameters)
        {
            if (sFileName != "")
                parameters.SelectedFiles = sFileName;
            parameters.Status = CloseStatus.OK;
            return handler.OnFileChooser(parameters);
        }

        public CloseStatus OnPrompt(PromptDialogParams parameters)
        {
            return handler.OnPrompt(parameters);
        }

        public CloseStatus OnReloadPostData(ReloadPostDataParams parameters)
        {
            return handler.OnReloadPostData(parameters);
        }

        public CloseStatus OnSelectCertificate(CertificatesDialogParams parameters)
        {
            return handler.OnSelectCertificate(parameters);
        }

        public CloseStatus OnColorChooser(ColorChooserParams parameters)
        {
            return handler.OnColorChooser(parameters);
        }
    }
}
