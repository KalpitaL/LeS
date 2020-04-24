/* Updates
 *  22-MAY-2015 : Modified to handle certificate issue in LoadPage() function * 
 *  28-APR-2016 : Modified to separate Freight Cost & Other Costs while updating quote
****/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.IO;
using System.Windows.Forms;
using WatiN.Core;
using System.Threading;

namespace WebScraper
{
    public class WebPage : IDisposable
    {
        private string _domain = "", _dialect = "", _senderCode = "", _recipientCode = "", _buyerName = "", _supplierName = "", _downloadedFileName="";
        public string dateformat = "HH:mm:ss",fileDateTimeFormat="yyyyMMddHHmmssfff",logDatetimeFormat="yyyy-MM-dd HH:mm:ss";
        public string cRfqType="",cItemsPresent="";//added by kalpita on 18/12/2019
          public int nItemCount = 0;

        public string LogDateTimeStamp
        {
            get
            {
                return DateTime.Now.ToString(logDatetimeFormat);
            }
        }
        public string CurrentTimeStamp
        {
            get
            {
                return DateTime.Now.ToString(dateformat);
            }
        }
        public string FileDateTimeStamp
        {
            get
            {
                return DateTime.Now.ToString(fileDateTimeFormat);
            }
        }
        public string Domain
        {
            get
            {
                string _newValue = convert.ToString(ConfigurationManager.AppSettings["DOMAIN"]).Trim();
                if (!String.IsNullOrEmpty(_newValue)) _domain = _newValue;
                return _domain;
            }
            set
            {
                _domain = value;
            }
        }
        public string HeaderMapFile
        {
            get
            {
                string _mapfile = "HEADER_MAP";
                string _doc = this.Domain;
                if (!String.IsNullOrEmpty(_mapfile)) { _mapfile = _mapfile + "_" + _doc; }
                return _mapfile + ".txt";
            }
        }
        public string EquipmentMapFile
        {
            get
            {
                string _mapfile = "EQUIP_MAP";
                string _doc = this.Domain;
                if (!String.IsNullOrEmpty(_mapfile)) { _mapfile = _mapfile + "_" + _doc; }
                return _mapfile + ".txt";
            }
        }
        public string ItemsMapFile
        {
            get
            {
                string _mapfile = "ITEMS_MAP";
                string _doc = this.Domain;
                if (!String.IsNullOrEmpty(_mapfile)) { _mapfile = _mapfile + "_" + _doc; }
                return _mapfile + ".txt";
            }
        }
        public string websiteMapFile
        {
            get
            {
                string _mapfile = "WEBSITE_MAPPING";
                string _doc = this.Domain;
                if (!String.IsNullOrEmpty(_mapfile)) { _mapfile = _mapfile + "_" + _doc; }
                return _mapfile + ".txt";
            }
        }
        public string ValidateMapFile
        {
            get
            {
                string _mapfile = "RFQ_VALIDATE";
                string _doc = ConfigurationManager.AppSettings.Get("DOMAIN").Trim();
                if (!String.IsNullOrEmpty(_mapfile)) { _mapfile = _mapfile + "_" + _doc; }
                return _mapfile + ".txt";
            }
        }
        public string LogPath
        {
            get
            {
                string _path = convert.ToString(ConfigurationManager.AppSettings["LOG_PATH"]).Trim();
                if (String.IsNullOrEmpty(_path)) { _path = Path.GetDirectoryName(@AppDomain.CurrentDomain.BaseDirectory) + @"\LOG"; }
                Directory.CreateDirectory(_path);
                return _path;
            }
        }
        public string LinksPath
        {
            get
            {
                string _path = convert.ToString(ConfigurationManager.AppSettings["LINKS_PATH"]).Trim();
                if (String.IsNullOrEmpty(_path)) { _path = Path.GetDirectoryName(@AppDomain.CurrentDomain.BaseDirectory) + @"\LINKS"; }
                Directory.CreateDirectory(_path);
                return _path;
            }
        }
        public string NotificationPath
        {
            get
            {
                string _path = convert.ToString(ConfigurationManager.AppSettings["NOTIFICATION_PATH"]).Trim();
                if (String.IsNullOrEmpty(_path)) { _path = Path.GetDirectoryName(@AppDomain.CurrentDomain.BaseDirectory) + @"\NOTIFY_FILES"; }
                Directory.CreateDirectory(_path);
                return _path;
            }
        }
        public string MAPPath
        {
            get
            {
                string _path = convert.ToString(ConfigurationManager.AppSettings["MAP_PATH"]).Trim();
                if (String.IsNullOrEmpty(_path)) { _path = Path.GetDirectoryName(@AppDomain.CurrentDomain.BaseDirectory) + @"\MAP_FILES"; }
                Directory.CreateDirectory(_path);
                return _path;
            }
        }
        public string XMLPath
        {
            get
            {
                string _path = convert.ToString(ConfigurationManager.AppSettings["XML_PATH"]).Trim();
                if (String.IsNullOrEmpty(_path)) { _path = Path.GetDirectoryName(@AppDomain.CurrentDomain.BaseDirectory) + @"\XML"; }
                Directory.CreateDirectory(_path);
                return _path;
            }
        }
        public string XLSPath
        {
            get
            {
                string _path = convert.ToString(ConfigurationManager.AppSettings["XLS_PATH"]).Trim();
                if (String.IsNullOrEmpty(_path)) { _path = Path.GetDirectoryName(@AppDomain.CurrentDomain.BaseDirectory) + @"\XLS"; }
                Directory.CreateDirectory(_path);
                return _path;
            }
        }

        public string HTMLPathRFQ
        {
            get
            {
                string _path = convert.ToString(ConfigurationManager.AppSettings["HTML_PATH_RFQ"]).Trim();
                if (String.IsNullOrEmpty(_path)) { _path = Path.GetDirectoryName(@AppDomain.CurrentDomain.BaseDirectory) + @"\HTML_RFQ"; }
                Directory.CreateDirectory(_path);
                return _path;
            }
        }
        public string HTMLPathQuote
        {
            get
            {
                string _path = convert.ToString(ConfigurationManager.AppSettings["HTML_PATH_QUOTE"]).Trim();
                if (String.IsNullOrEmpty(_path)) { _path = Path.GetDirectoryName(@AppDomain.CurrentDomain.BaseDirectory) + @"\HTML_Quote"; }
                Directory.CreateDirectory(_path);
                return _path;
            }
        }
        public string ImagePathRFQ
        {
            get
            {
                string _path = convert.ToString(ConfigurationManager.AppSettings["IMAGE_PATH_RFQ"]).Trim();
                if (String.IsNullOrEmpty(_path)) { _path = Path.GetDirectoryName(@AppDomain.CurrentDomain.BaseDirectory) + @"\IMAGE_RFQ"; }
                Directory.CreateDirectory(_path);
                return _path;
            }
        }
        public string ImagePathQuote
        {
            get
            {
                string _path = convert.ToString(ConfigurationManager.AppSettings["IMAGE_PATH_QUOTE"]).Trim();
                if (String.IsNullOrEmpty(_path)) { _path = Path.GetDirectoryName(@AppDomain.CurrentDomain.BaseDirectory) + @"\IMAGE_QUOTE"; }
                Directory.CreateDirectory(_path);
                return _path;
            }
        }

        public string HTMLPathRFQError
        {
            get
            {
                string _path = convert.ToString(ConfigurationManager.AppSettings["HTML_PATH_RFQ"]).Trim();
                if (String.IsNullOrEmpty(_path)) { _path = Path.GetDirectoryName(@AppDomain.CurrentDomain.BaseDirectory) + @"\HTML_RFQ"; }
                Directory.CreateDirectory(_path + "\\Error");
                return _path;
            }
        }
        public string HTMLPathQuoteError
        {
            get
            {
                string _path = convert.ToString(ConfigurationManager.AppSettings["HTML_PATH_QUOTE"]).Trim();
                if (String.IsNullOrEmpty(_path)) { _path = Path.GetDirectoryName(@AppDomain.CurrentDomain.BaseDirectory) + @"\HTML_Quote"; }
                Directory.CreateDirectory(_path + "\\Error");
                return _path;
            }
        }
        public string ImagePathRFQError
        {
            get
            {
                string _path = convert.ToString(ConfigurationManager.AppSettings["IMAGE_PATH_RFQ"]).Trim();
                if (String.IsNullOrEmpty(_path)) { _path = Path.GetDirectoryName(@AppDomain.CurrentDomain.BaseDirectory) + @"\IMAGE_RFQ"; }
                Directory.CreateDirectory(_path + "\\Error");
                return _path;
            }
        }
        public string ImagePathQuoteError
        {
            get
            {
                string _path = convert.ToString(ConfigurationManager.AppSettings["IMAGE_PATH_QUOTE"]).Trim();
                if (String.IsNullOrEmpty(_path)) { _path = Path.GetDirectoryName(@AppDomain.CurrentDomain.BaseDirectory) + @"\IMAGE_QUOTE"; }
                Directory.CreateDirectory(_path + "\\Error");
                return _path;
            }
        }      

        private bool ViewBrowser
        {
            get
            {
                if (ConfigurationSettings.AppSettings["VIEW_BROWSER"] != null && convert.ToString(ConfigurationSettings.AppSettings["VIEW_BROWSER"]).ToLower().Trim() == "true")
                    return true;
                else return false;
            }
        }
        private bool IsNew
        {
            get
            {
                if (ConfigurationSettings.AppSettings["NEW"] != null && convert.ToString(ConfigurationSettings.AppSettings["NEW"]).ToLower().Trim() == "true")
                    return true;
                else return false;
            }
        }
        public string Dialect
        {
            get
            {
                string _newValue = convert.ToString(ConfigurationManager.AppSettings["DIALECT"]).Trim();
                if (!String.IsNullOrEmpty(_newValue)) _dialect = _newValue;
                return _dialect;
            }
        }
        public string Sender_Code
        {
            get
            {
                string _newValue = convert.ToString(ConfigurationManager.AppSettings["SENDER_CODE"]).Trim();
                if (!String.IsNullOrEmpty(_newValue)) _senderCode = _newValue;
                return _senderCode;
            }
        }
        public string Recipient_Code
        {
            get
            {
                string _newValue = convert.ToString(ConfigurationManager.AppSettings["RECIPIENT_CODE"]).Trim();
                if (!String.IsNullOrEmpty(_newValue)) _recipientCode = _newValue;
                return _recipientCode;
            }
        }
        public string Recipient_Code_Singapore
        {
            get
            {
                string _newValue = convert.ToString(ConfigurationManager.AppSettings["RECIPIENT_CODE_SINGAPORE"]).Trim();
                if (!String.IsNullOrEmpty(_newValue)) _recipientCode = _newValue;
                return _recipientCode;
            }
        }
        public string Buyer_Name
        {
            get
            {
                string _newValue = convert.ToString(ConfigurationManager.AppSettings["BUYER_NAME"]).Trim();
                if (!String.IsNullOrEmpty(_newValue)) _buyerName = _newValue;
                return _buyerName;
            }
        }
        public string Supplier_Name
        {
            get
            {
                string _newValue = convert.ToString(ConfigurationManager.AppSettings["SUPPLIER_NAME"]).Trim();
                if (!String.IsNullOrEmpty(_newValue)) _supplierName = _newValue;
                return _supplierName;
            }
        }
        public string Supplier_Name_Singapore
        {
            get
            {
                string _newValue = convert.ToString(ConfigurationManager.AppSettings["SUPPLIER_NAME_SINGAPORE"]).Trim();
                if (!String.IsNullOrEmpty(_newValue)) _supplierName = _newValue;
                return _supplierName;
            }
        }
        public string Downloaded_FileName
        {
            get
            {
                string _newValue = convert.ToString(ConfigurationManager.AppSettings["DOWNLOADED_FILENAME"]).Trim();
                if (!String.IsNullOrEmpty(_newValue)) _downloadedFileName = _newValue;
                return _downloadedFileName;
            }
        }
        public string QuotePOCPath
        {
            get
            {
                string _path = convert.ToString(ConfigurationManager.AppSettings["QUOTE_POC_PATH"]).Trim();
                if (String.IsNullOrEmpty(_path)) { _path = Path.GetDirectoryName(@AppDomain.CurrentDomain.BaseDirectory) + @"\QUOTE"; }
                Directory.CreateDirectory(_path);
                return _path;
            }
        }
        public string Module { get; set; }
        public string DocType { get; set; }
        public string MsgFile { get; set; }
        public bool SendMsgFileNameToAudit { get; set; }
        public string PageGUID { get; set; }
        protected string PageSource { get; set; }
        protected string _URL { get; set; }
        public string PageReference { get; set; }
        public string Attachment { get; set; }
        public string HtmlFile { get; set; }
        public string ImgFile { get; set; }
        public string taskToPerform { get; set; }
        public bool ifErrorFile { get; set; }
        public string cRFQErrorText { get; set; } 
        public List<string> LOG;
        private List<List<string>> lstAudit;
        public List<List<string>> _xmlItems = null;

        public WebPage()
        {
            LOG = new List<string>();
            lstAudit = new List<List<string>>();
            //WatiN.Core.Settings.MakeNewIeInstanceVisible = this.ViewBrowser;
            WatiN.Core.Settings.WaitForCompleteTimeOut = 300;
            this.MsgFile = "";
            this.SendMsgFileNameToAudit = false;
        }

        public WebPage(IEBrowser ie)
        {
            this.MsgFile = "";
            this.SendMsgFileNameToAudit = false;
        }

        public void Dispose()
        {
            try
            {
              
            }
            catch { }
        }

        public void SetLog(string data)
        {
            try
            {
                //data = data + Environment.NewLine;
                if (data.Trim().Length > 0)
                {
                    File.AppendAllText(@LogPath + @"\WebLog_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt", Environment.NewLine + LogDateTimeStamp + " : " + data);
                    Console.WriteLine(LogDateTimeStamp + " : " + data);
                }
                else
                    File.AppendAllText(@LogPath + @"\WebLog_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt", "\r\n");
            }
            catch (Exception ex)
            {
            }
        }

        public virtual IEBrowser LoadPage(string URL)
        {
            IEBrowser _ie = null;
            try
            {
                if (this.IsNew) _ie = new IEBrowser(this.IsNew);
                else _ie = new IEBrowser();

                if (this.ViewBrowser == false) _ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Hide);
                else
                {
                    //_ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.ForceMinimized);
                    _ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.ShowMaximized);
                }

                _URL = URL;
                _ie.gotopage(URL);

                #region /* Check Certificate Issue */
                int count = 0;
                string bodyText = "";
                if (_ie.Body != null) bodyText = convert.ToString(_ie.Body.Text).Trim();
                while (_ie.Body != null && bodyText.Contains("security certificate") && bodyText.Contains("Continue to this website"))
                {
                    count++;
                    Link lnkContinue = _ie.Link("overridelink");
                    if (lnkContinue.Exists) lnkContinue.Click();
                    Thread.Sleep(2000);
                    if (count > 10) break;
                }
                #endregion

                SetupPage(_ie);
                PageSource = _ie.Body.Parent.OuterHtml;
                SetLog("Page Loaded - " + _URL);
                return _ie;
            }
            catch (WatiN.Core.Exceptions.WatiNException ex)
            {
                SetLog("Error in LoadPage() - " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                SetLog("Error in LoadPage() - " + ex.Message);
                throw;
            }
        }

        public void InvokeAction(IEBrowser _ie, string _elementId, string _action)
        {
            try
            {
                _ie.InvokeAction(_elementId, _action);
            }
            catch (Exception ex)
            {
                SetLog("Unable to invoke " + _action + " for " + _elementId + "! Error - " + ex.Message);
            }
        }

        public bool IsElementVisible(IEBrowser _ie, string _elementId)
        {
            return _ie.IsElementVisible(_elementId);
        }

        public bool IsValidElementByID(IEBrowser _ie, string _elementId)
        {
            return _ie.IsValidElementById(_elementId);
        }

        public virtual void SetupPage(IEBrowser _ie)
        {
            PageLogin(_ie);
        }

        public virtual void SavePage(IEBrowser _ie, bool SavePNG, string PrintURL)
        {
            try
            {
                if (string.IsNullOrEmpty(this.HtmlFile)) this.HtmlFile = "Html_" + FileDateTimeStamp + ".html";
                if (string.IsNullOrEmpty(this.ImgFile)) this.ImgFile = "Img_" + FileDateTimeStamp + ".png";

                string _source = PageSource;

                try
                {
                    if (DocType.ToUpper() == "RFQ")
                    {
                        if (ifErrorFile)
                        {
                            File.WriteAllText(this.HTMLPathRFQError + "\\Error\\" + this.HtmlFile, _source);
                            SetLog("RFQ HTML named '" + this.HtmlFile + "' saved on path '" + this.HTMLPathRFQError + "\\Error'");
                        }
                        else
                        {
                            File.WriteAllText(this.HTMLPathRFQ + "\\" + this.HtmlFile, _source);
                            SetLog("RFQ HTML named '" + this.HtmlFile + "' saved on path '" + this.HTMLPathRFQ + "'");
                        }
                    }
                    else if (DocType.ToUpper() == "QUOTE")
                    {
                        if (ifErrorFile)
                        {
                            File.WriteAllText(this.HTMLPathQuoteError + "\\Error\\" + this.HtmlFile, _source);
                            SetLog("Quote HTML named '" + this.HtmlFile + "' saved on path '" + this.HTMLPathQuoteError + "\\Error'");
                        }
                        else
                        {
                            File.WriteAllText(this.HTMLPathQuote + "\\" + this.HtmlFile, _source);
                            SetLog("Quote HTML named '" + this.HtmlFile + "' saved on path '" + this.HTMLPathQuote + "'");
                        }
                    }
                }
                catch (Exception ex)
                {
                    SetLog("Exception in saving HTML - " + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString());
                }
                if (SavePNG)
                {
                    try
                    {
                        if (DocType.ToUpper() == "RFQ")
                        {
                            if (ifErrorFile)
                            {
                                _ie.SaveAsPNG(this.ImagePathRFQError + "\\Error", this.ImgFile, (this.HTMLPathRFQError + "\\Error\\" + this.HtmlFile), PrintURL);
                                SetLog("RFQ Screenshot named '" + this.ImgFile + "' saved on path '" + this.ImagePathRFQError + "\\Error'");
                            }
                            else
                            {
                                _ie.SaveAsPNG(this.ImagePathRFQ, this.ImgFile, (this.HTMLPathRFQ + "\\" + this.HtmlFile), PrintURL);
                                SetLog("RFQ Screenshot named '" + this.ImgFile + "' saved on path '" + this.ImagePathRFQ + "'");
                            }
                        }
                        else if (DocType.ToUpper() == "QUOTE")
                        {
                            if (ifErrorFile)
                            {
                                _ie.SaveAsPNG(this.ImagePathQuoteError + "\\Error", this.ImgFile, (this.HTMLPathQuoteError + "\\Error\\" + this.HtmlFile), PrintURL);
                                SetLog("Quote Screenshot named '" + this.ImgFile + "' saved on path '" + this.ImagePathQuoteError + "\\Error'");
                            }
                            else
                            {
                                _ie.SaveAsPNG(this.ImagePathQuote, this.ImgFile, (this.HTMLPathQuote + "\\" + this.HtmlFile), PrintURL);
                                SetLog("Quote Screenshot named '" + this.ImgFile + "' saved on path '" + this.ImagePathQuote + "'");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SetLog("Exception in saving Image - " + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                SetLog("Exception in 'SavePage()' function of 'WebPage' class." + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString());
            }
        }

        public virtual bool ValidatePage(IEBrowser _ie)
        {
            bool _result = false;
            if (_ie._document == null) _ie.gotopage(_URL);
            if (File.Exists(@MAPPath + @"\" + ValidateMapFile))
            {
                string[] _lines = File.ReadAllLines(@MAPPath + @"\" + ValidateMapFile);
                for (int i = 0; i < _lines.Length; i++)
                {
                    string[] _keys = _lines[i].Split('=');
                    _result = IsValidElementByID(_ie, _keys[1]);
                    if (!_result)
                    {
                        return _result;
                    }
                }
            }
            return _result;
        }

        public virtual void FetchData(string URL)
        {
            IEBrowser _ie = LoadPage(URL);
            string strerr = "";
            try
            {
                if (_ie != null)
                {
                    _xmlItems = new List<List<string>>();

                    strerr = "Unable to get header data!";
                    Dictionary<string, string> _xmlHeader = GetHeaderData(_ie);
                    strerr = "Unable to get item data!";                    
                    _xmlItems = GetItemsData(_ie);

                    SavePage(_ie, true, "");
                    strerr = "Unable to export xml!";
                    ExportToLESML(_xmlHeader, _xmlItems);

                    PageLogout(_ie);
                    if (this.PageGUID.Trim().Length > 0) SetGUIDs(this.PageGUID.Trim());
                }
            }
            catch (WatiN.Core.Exceptions.WatiNException wex)
            {
                SetLog(strerr + " Error - " + wex.Message);
                throw wex;
            }
            catch (Exception ex)
            {
                SetLog(strerr + " Error - " + ex.Message);
                throw ex;
            }
            finally
            {
                try { if (_ie != null) { _ie.Dispose(); Thread.Sleep(7000); } }
                catch { }
            }
        }

        public virtual void FetchData(IEBrowser _ie)
        {
            string strerr = "";
            try
            {
                _xmlItems = new List<List<string>>();
                strerr = "getting header data.";
                Dictionary<string, string> _xmlHeader = GetHeaderData(_ie);
                strerr = "getting item data.";
                _xmlItems = GetItemsData(_ie);
                ifErrorFile = false;
                SavePage(_ie, true, "");
                strerr = "exporting xml.";                
                ExportToLESML(_xmlHeader, _xmlItems);
                strerr = "logout from website.";
                PageLogout(_ie);                
                if (this.PageGUID.Trim().Length > 0) SetGUIDs(this.PageGUID.Trim());
            }
            catch (Exception ex)
            {               
                cRFQErrorText = "Exception in " + strerr + ex.Message.ToString() +";"+ ex.StackTrace.ToString();
                SetLog(cRFQErrorText);
                throw ex;
            }
        }

        public virtual void PageLogin(IEBrowser ie)
        {
            //
        }

        public virtual void PageLogout(IEBrowser ie)
        {
            //
        }

        public virtual void SiteLogin(IEBrowser ie)
        {
            //
        }

        public virtual void SiteLogout(IEBrowser ie)
        {
            //
        }

        public virtual List<List<string>> GetItemsData(IEBrowser _ie)
        {
            return null;
        }

        public virtual Dictionary<string, string> GetHeaderData(IEBrowser _ie)
        {
            Dictionary<string, string> _xmlHeader = new Dictionary<string, string>();
            try
            {
                if (File.Exists(@MAPPath + @"\" + HeaderMapFile))
                {
                    string[] _lines = File.ReadAllLines(@MAPPath + @"\" + HeaderMapFile);
                    for (int i = 0; i < _lines.Length; i++)
                    {
                        string[] _keys = _lines[i].Split('=');
                        if (_keys.Length > 1)
                        {
                            string[] _values = _keys[1].Split('|');
                            string _value = "";
                            if (_values.Length > 0)
                            {
                                for (int k = 0; k < _values.Length; k++)
                                    _value += _ie.GetText(_values[k]) + Environment.NewLine;
                            }
                            if (String.IsNullOrEmpty(_value)) { _value = string.Empty; }
                            _xmlHeader.Add(_keys[0], _value.Trim());
                        }
                    }
                }
                else SetLog("File named '" + HeaderMapFile + "' not found on path '" + @MAPPath + "'");

            }
            catch(Exception ex) {
                SetLog("Exception in 'GetHeaderData()' function of 'WebPage' class."+ex.Message.ToString()+Environment.NewLine+ex.StackTrace.ToString());
                throw;
            }
            finally
            {
                //
            }
            return _xmlHeader;
        }

        public virtual int GetItemCount(IEBrowser _ie)
        {
            return 0;
        }

        //public void ExportToLESML(Dictionary<string, string> _xmlHeader, List<List<string>> _xmlItems)
        //{
        //    LeSXML.LeSXML _lesXML = new LeSXML.LeSXML();
        //    ExporttoHeader(_xmlHeader, _lesXML);
        //    ExporttoItems(_xmlItems, _lesXML);
        //    try
        //    {
        //        bool attachmentFound = false;

        //        if (File.Exists(this.ImagePathRFQ + @"\" + _lesXML.OrigDocFile)) attachmentFound = true;
        //        else if (File.Exists(this.ImagePathRFQ + @"\Backup\" + _lesXML.OrigDocFile)) attachmentFound = true;
        //        else attachmentFound = false;

        //        if (!attachmentFound) _lesXML.OrigDocFile = "";

        //        _lesXML.WriteXML();
        //        this.PageGUID = _lesXML.BuyerRef.ToUpper() + "|" + _lesXML.Vessel.ToUpper() + "|" + _lesXML.PortName.ToUpper();
        //        if (File.Exists(_lesXML.FilePath + "\\" + _lesXML.FileName))
        //        {

        //            SetLog(_lesXML.Doc_Type + " '" + _lesXML.BuyerRef + "' named " + _lesXML.FileName + " downloaded successfully on path '" + _lesXML.FilePath + "'");
        //            // Add Record to audit 
        //            string sAudit = DateTime.Now.ToString("yy-MM-dd HH:mm") + " : " + _lesXML.Doc_Type + " '" + _lesXML.BuyerRef + "' downloaded successfully.";
        //            if (this.SendMsgFileNameToAudit) AddToAudit(_lesXML.Sender_Code, _lesXML.Recipient_Code, this.Module, this.MsgFile, _lesXML.BuyerRef, "Downloaded", sAudit);
        //            else AddToAudit(_lesXML.Sender_Code, _lesXML.Recipient_Code, this.Module, Path.GetFileName(_lesXML.FileName), _lesXML.BuyerRef, "Downloaded", sAudit);
        //        }
        //        else
        //        {
        //            SetLog(_lesXML.Doc_Type + " '" + _lesXML.BuyerRef + "' named " + _lesXML.FileName + " not downloaded successfully on path '" + _lesXML.FilePath + "'");
        //            string sAudit = DateTime.Now.ToString("yy-MM-dd HH:mm") + " : " + _lesXML.Doc_Type + " '" + _lesXML.BuyerRef + "' not downloaded successfully.";
        //            if (this.SendMsgFileNameToAudit) AddToAudit(_lesXML.Sender_Code, _lesXML.Recipient_Code, this.Module, this.MsgFile, _lesXML.BuyerRef, "Error", sAudit);
        //            else AddToAudit(_lesXML.Sender_Code, _lesXML.Recipient_Code, this.Module, Path.GetFileName(_lesXML.FileName), _lesXML.BuyerRef, "Error", sAudit);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SetLog("Exception in 'ExportToLESML()' function of 'WebPage' class." + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString());
        //        throw ex;
        //    }
        //}

        /*Changed by Kalpita on 18/12/2019*/
        public void ExportToLESML(Dictionary<string, string> _xmlHeader, List<List<string>> _xmlItems)
        {
            LeSXML.LeSXML _lesXML = new LeSXML.LeSXML();
            ExporttoHeader(_xmlHeader, _lesXML);
            ExporttoItems(_xmlItems, _lesXML);
            try
            {
                if (cRfqType.ToUpper() == "REPAIR" && nItemCount == 0)//added by kalpita on 18/12/2019
                {
                    SetLog(cRfqType + " " + _lesXML.Doc_Type + " '" + _lesXML.BuyerRef + " not downloaded since there are no items.");
                    string sAudit = DateTime.Now.ToString("yy-MM-dd HH:mm") + " : " + cRfqType + " " + _lesXML.Doc_Type + " '" + _lesXML.BuyerRef + " not downloaded since there are no items.";
                    AddToAudit(_lesXML.Sender_Code, _lesXML.Recipient_Code, this.Module, this.MsgFile, _lesXML.BuyerRef, "Error", sAudit);
                }
                else
                {
                    bool attachmentFound = false;

                    if (File.Exists(this.ImagePathRFQ + @"\" + _lesXML.OrigDocFile)) attachmentFound = true;
                    else if (File.Exists(this.ImagePathRFQ + @"\Backup\" + _lesXML.OrigDocFile)) attachmentFound = true;
                    else attachmentFound = false;

                    if (!attachmentFound) _lesXML.OrigDocFile = "";

                    _lesXML.WriteXML();
                    this.PageGUID = _lesXML.BuyerRef.ToUpper() + "|" + _lesXML.Vessel.ToUpper() + "|" + _lesXML.PortName.ToUpper();
                    if (File.Exists(_lesXML.FilePath + "\\" + _lesXML.FileName))
                    {

                        SetLog(_lesXML.Doc_Type + " '" + _lesXML.BuyerRef + "' named " + _lesXML.FileName + " downloaded successfully on path '" + _lesXML.FilePath + "'");
                        // Add Record to audit 
                        string sAudit = DateTime.Now.ToString("yy-MM-dd HH:mm") + " : " + _lesXML.Doc_Type + " '" + _lesXML.BuyerRef + "' downloaded successfully.";
                        if (this.SendMsgFileNameToAudit) AddToAudit(_lesXML.Sender_Code, _lesXML.Recipient_Code, this.Module, this.MsgFile, _lesXML.BuyerRef, "Downloaded", sAudit);
                        else AddToAudit(_lesXML.Sender_Code, _lesXML.Recipient_Code, this.Module, Path.GetFileName(_lesXML.FileName), _lesXML.BuyerRef, "Downloaded", sAudit);
                    }
                    else
                    {
                        SetLog(_lesXML.Doc_Type + " '" + _lesXML.BuyerRef + "' named " + _lesXML.FileName + " not downloaded successfully on path '" + _lesXML.FilePath + "'");
                        string sAudit = DateTime.Now.ToString("yy-MM-dd HH:mm") + " : " + _lesXML.Doc_Type + " '" + _lesXML.BuyerRef + "' not downloaded successfully.";
                        if (this.SendMsgFileNameToAudit) AddToAudit(_lesXML.Sender_Code, _lesXML.Recipient_Code, this.Module, this.MsgFile, _lesXML.BuyerRef, "Error", sAudit);
                        else AddToAudit(_lesXML.Sender_Code, _lesXML.Recipient_Code, this.Module, Path.GetFileName(_lesXML.FileName), _lesXML.BuyerRef, "Error", sAudit);
                    }
                }
            }
            catch (Exception ex)
            {
                SetLog("Exception in 'ExportToLESML()' function of 'WebPage' class." + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString());
                throw ex;
            }
        }

        public virtual void ExporttoItems(List<List<string>> _xmlItems, LeSXML.LeSXML _lesXML)
        {
            //
        }

        public virtual void ExporttoHeader(Dictionary<string, string> _xmlHeader, LeSXML.LeSXML _lesXML)
        {
            //
        }

        public virtual string GetXMLDate(string _date, string _type)
        {
            string _result = _date;
            string[] _datearr = _date.Split('/');
            if (_datearr[0].Length == 1) { _datearr[0] = "0" + _datearr[0]; }
            if (_datearr[1].Length == 1) { _datearr[1] = "0" + _datearr[1]; }
            switch (_type)
            {
                case "1": { _result = _datearr[2] + _datearr[1] + _datearr[0]; } break;
                case "2": { _result = _datearr[2] + _datearr[0] + _datearr[1]; } break;
            }
            return _result;
        }

        public bool DownloadFile(string _URL, string _SaveAs)
        {
            try
            {
                FileInfo file = new FileInfo(_SaveAs);
                if (!file.Directory.Exists) file.Directory.Create(); // Create Directory if it does not exists
                System.Net.WebClient _WebClient = new System.Net.WebClient();
                // Downloads the resource with the specified URI to a local file.
                _WebClient.DownloadFile(_URL, _SaveAs);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void AddToAudit(string Buyer, string Supplier, string Module, string Filename, string RefNo, string LogType, string Audit)
        {
            try
            {
                List<string> lst = new List<string>();
                lst.Add(Buyer.Trim());
                lst.Add(Supplier.Trim());
                lst.Add(Module.Trim());
                lst.Add(Filename.Trim());
                lst.Add(RefNo.Trim());
                lst.Add(LogType.Trim());
                lst.Add(Audit.Trim());
                lstAudit.Add(lst);

                SetAuditLog();
            }
            catch (Exception ex) { }
        }

        private void SetAuditLog()
        {
            try
            {
                string _auditPath = convert.ToString(ConfigurationSettings.AppSettings["AUDIT_PATH"]);
                if (_auditPath.Trim().Length > 0 && !Directory.Exists(_auditPath))
                    Directory.CreateDirectory(_auditPath);
                string auditData = "";
                foreach (List<string> lst in lstAudit)
                {
                    if (auditData.Trim().Length > 0) auditData += Environment.NewLine;
                    auditData += lst[0] + "|";// Buyer
                    auditData += lst[1] + "|"; // Supplier
                    auditData += lst[2] + "|"; // Module
                    auditData += lst[3] + "|"; // FileName
                    auditData += lst[4] + "|"; // RefNo
                    auditData += lst[5] + "|"; // LogType
                    auditData += lst[6]; // Audit
                }

                lstAudit.Clear();
                if (auditData.Trim().Length > 0)
                {
                    File.WriteAllText(_auditPath + "\\Audit_" + DateTime.Now.ToString("ddMMyyyyHHmmssff") + ".txt", auditData);
                }
            }
            catch (Exception ex)
            {
                SetLog("Unable to create audit file - " + ex.Message);
            }
        }

        public List<string> GetGUID(string GUID_File)
        {
            List<string> guidList = new List<string>();
            if (File.Exists(@AppDomain.CurrentDomain.BaseDirectory + "\\" + GUID_File))
            {
                string[] _guidPresent = File.ReadAllLines(@AppDomain.CurrentDomain.BaseDirectory + "\\" + GUID_File);
                foreach (string s in _guidPresent) if (s.Trim().Length > 0) guidList.Add(s.Trim());
            }
            else if (File.Exists(@AppDomain.CurrentDomain.BaseDirectory + "\\GUID.txt"))
            {
                string[] _guidPresent = File.ReadAllLines(@AppDomain.CurrentDomain.BaseDirectory + "\\GUID.txt");
                foreach (string s in _guidPresent) if (s.Trim().Length > 0) guidList.Add(s.Trim());
            }
            return guidList;
        }

        public void SetGUIDs(string GUID)
        {
            using (StreamWriter sw = new StreamWriter(@AppDomain.CurrentDomain.BaseDirectory + Downloaded_FileName, true))
            {
                sw.WriteLine(GUID);
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
        }        

        public void SetGUIDs(string GUID, string GUID_File)
        {
            if (File.Exists(@AppDomain.CurrentDomain.BaseDirectory + "\\" + GUID_File))
            {
                using (StreamWriter sw = new StreamWriter(@AppDomain.CurrentDomain.BaseDirectory + "\\" + GUID_File, true))
                {
                    sw.WriteLine(GUID);
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(@AppDomain.CurrentDomain.BaseDirectory + "\\GUID.txt", true))
                {
                    sw.WriteLine(GUID);
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
        }
    }
}