using HTTPWrapper;
using MTML.GENERATOR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacificRoutine
{
    public class PBRoutine : LeSCommon.LeSCommon
    {
        string sAuditMesage = "", BuyerName = "", RFQPath = "", MailTextPath = "", docType = "", VRNO = "", txtURL = "", MTMLUploadPath = "", MessageNumber = ""
           , LeadDays = "", Currency = "", MsgNumber = "", MsgRefNumber = "", UCRefNo = "", AAGRefNo = "", LesRecordID = "", BuyerPhone = "", BuyerEmail = "", strBuyerFax = "",
           SupplierName = "",SupplierPhone = "", SupplierEmail = "", SupplierFax = "", PortName = "", PortCode = "", VesselName, BuyerFax = "",
           PackingCost = "0", FreightCharge = "0", TotalLineItemsAmount = "0", GrandTotal = "0", Allowance = "0", SupplierComment = "", PayTerms = "",BuyerTotal = "0",
           DepositCost = "0", OtherCost = "0", DelvDate = "", ExpDate = "", DocDate = "", EncyptUrls="";
   

        public string[] Actions;
        bool IsDecline = false, isSendMail = false, isSaveQuote = true, isAcknowledgeUpdates = false,IsSuppEmailDisplay=false;
         double AdditionalDiscount = 0;
        int IsPriceAveraged = 0, IsAltItemAllowed = 0, IsUOMChanged = 0;
        List<string> RFQ_mailFiles = new List<string>();
        List<string> PO_mailFiles = new List<string>();
        List<string> Quote_mtmlFiles = new List<string>();
        List<string> POC_mtmlFiles = new List<string>();
        RichTextBox _txtData = new RichTextBox();
        public MTMLInterchange _interchange { get; set; }
        public LineItemCollection _lineitem = null;

        public void LoadAppsettings()
        {
            try
            {
                #region Commented
                //AuditPath = dctAppSettings["AuditPath"].Trim();
                //SupplierCode = dctAppSettings["Supplier"].Trim();
                //BuyerCode = dctAppSettings["Buyer"].Trim();
                //BuyerName = dctAppSettings["BuyerName"].Trim();
                //RFQPath = ConfigurationManager.AppSettings["XML_PATH"].Trim();
                //PrintScreenPath = dctAppSettings["RFQ_AttachmentPath"].Trim();
                //MailTextPath = dctAppSettings["MailTextFilePath"].Trim();
                //Actions = dctAppSettings["Actions"].Trim().Split(',');
                //MTMLUploadPath = dctAppSettings["Mtml_Quote_UploadPath"].Trim();
                //isSendMail = Convert.ToBoolean(dctAppSettings["SendMail"].Trim());
                //isSaveQuote = Convert.ToBoolean(dctAppSettings["SaveQuotation"].Trim());
                //isAcknowledgeUpdates = Convert.ToBoolean(dctAppSettings["AcknowledgeUpdates"].Trim());

                //if (AuditPath == "") AuditPath = AppDomain.CurrentDomain.BaseDirectory + "Audit";
                //if (RFQPath == "") RFQPath = AppDomain.CurrentDomain.BaseDirectory + "XML";
                //if (PrintScreenPath == "") PrintScreenPath = AppDomain.CurrentDomain.BaseDirectory + "ScreenShots";
                //if (!Directory.Exists(PrintScreenPath)) Directory.CreateDirectory(PrintScreenPath);
                //if (MailTextPath == "") MailTextPath = Application.StartupPath + "\\MailTextFile";
                //if (MTMLUploadPath != "")
                //{
                //    if (!Directory.Exists(MTMLUploadPath + "\\Error")) Directory.CreateDirectory(MTMLUploadPath + "\\Error");
                //    if (!Directory.Exists(MTMLUploadPath + "\\Backup")) Directory.CreateDirectory(MTMLUploadPath + "\\Backup");
                //}
                #endregion

                //changed by kalpita on 27/06/2019
                AuditPath = (dctAppSettings.ContainsKey("AuditPath")) ? dctAppSettings["AuditPath"].Trim() : string.Empty;

                RFQPath = (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["XML_PATH"])) ? ConfigurationManager.AppSettings["XML_PATH"].Trim() : string.Empty;

                SupplierCode = (dctAppSettings.ContainsKey("Supplier")) ? dctAppSettings["Supplier"].Trim() : string.Empty;
                BuyerCode = (dctAppSettings.ContainsKey("Buyer")) ? dctAppSettings["Buyer"].Trim() : string.Empty;
                BuyerName = (dctAppSettings.ContainsKey("BuyerName")) ? dctAppSettings["BuyerName"].Trim() : string.Empty;

                PrintScreenPath = (dctAppSettings.ContainsKey("RFQ_AttachmentPath")) ? dctAppSettings["RFQ_AttachmentPath"].Trim() : string.Empty;
                MailTextPath = (dctAppSettings.ContainsKey("MailTextFilePath")) ? dctAppSettings["MailTextFilePath"].Trim() : string.Empty;
                Actions = (dctAppSettings.ContainsKey("Actions")) ? dctAppSettings["Actions"].Trim().Split(',') : null;
                MTMLUploadPath = (dctAppSettings.ContainsKey("Mtml_Quote_UploadPath")) ? dctAppSettings["Mtml_Quote_UploadPath"].Trim() : string.Empty;
                isSendMail = (dctAppSettings.ContainsKey("SendMail")) ? Convert.ToBoolean(dctAppSettings["SendMail"].Trim()) : false;
                isSaveQuote = (dctAppSettings.ContainsKey("SaveQuotation")) ? Convert.ToBoolean(dctAppSettings["SaveQuotation"].Trim()) : false;
                isAcknowledgeUpdates = (dctAppSettings.ContainsKey("AcknowledgeUpdates")) ? Convert.ToBoolean(dctAppSettings["AcknowledgeUpdates"].Trim()) : false;

                if (AuditPath == "") AuditPath = AppDomain.CurrentDomain.BaseDirectory + "Audit";
                if (RFQPath == "") RFQPath = AppDomain.CurrentDomain.BaseDirectory + "XML";
                if (PrintScreenPath == "") PrintScreenPath = AppDomain.CurrentDomain.BaseDirectory + "ScreenShots";
                if (!Directory.Exists(PrintScreenPath)) Directory.CreateDirectory(PrintScreenPath);
                if (MailTextPath == "") MailTextPath = Application.StartupPath + "\\MailTextFile";
                if (MTMLUploadPath != "")
                {
                    if (!Directory.Exists(MTMLUploadPath + "\\Error")) Directory.CreateDirectory(MTMLUploadPath + "\\Error");
                    if (!Directory.Exists(MTMLUploadPath + "\\Backup")) Directory.CreateDirectory(MTMLUploadPath + "\\Backup");
                }
                EncyptUrls = Convert.ToString(ConfigurationManager.AppSettings["ENCYRPTURL"]);//added by Kalpita on 26/06/2019
                IsSuppEmailDisplay = Convert.ToBoolean(ConfigurationManager.AppSettings["DISPLAY_SUPPLIER_EMAIL"].Trim());//added by Kalpita on 02/08/2019(Kloska)
            }
            catch (Exception e)
            {
                sAuditMesage = "Exception in Initialise: " + e.GetBaseException().ToString();
                LogText = sAuditMesage;
                //CreateAuditFile("", "DNVGL_HTTP", "", "Error", sAuditMesage, BuyerCode, SupplierCode, AuditPath);
            }
        }

        #region RFQ
        public void Read_MailTextFiles()
        {
            try
            {
                docType = "RFQ";

                GetMailTextFiles();
                if (RFQ_mailFiles.Count > 0)
                {
                    LogText = "";
                    LogText = "RFQ processing started.";
                    for (int j = RFQ_mailFiles.Count - 1; j >= 0; j--)
                    {
                        Dictionary<string, string> dicURLToken = GetURL(RFQ_mailFiles[j]);
                        if (dicURLToken != null && dicURLToken.Count > 0)
                        {
                            if (dicURLToken["URL"].Trim() != "" && dicURLToken["URL"].Trim().Contains("https://supplierportal.dnvgl.com/") && dicURLToken["Token"].Trim() != "")
                            {
                                if (!_httpWrapper._AddRequestHeaders.ContainsKey("Origin"))
                                    _httpWrapper._AddRequestHeaders.Add("Origin", @"https://supplierportal.dnvgl.com");
                                _httpWrapper.ContentType = "application/json";
                                //_httpWrapper.AcceptMimeType = "*/*";
                                //_httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.CacheControl);
                                //_httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");
                                //_httpWrapper.Referrer=@"https://supplierportal.dnvgl.com/";
                                dctPostDataValues.Clear();
                                dctPostDataValues.Add("Password", dicURLToken["Token"].Trim());
                                dctPostDataValues.Add("Type", "1");
                                txtURL = dicURLToken["URL"].Trim();
                                string[] ArrURL = dicURLToken["URL"].Trim().Split('/');
                                URL = ArrURL[0] + "//" + ArrURL[2] + "/api/Security/PostLogin/" + ArrURL[4];
                                string _postdata = GetPostData(true);
                                bool isLoggedin = _httpWrapper.PostURL(URL, _postdata, "", "", "");
                                if (isLoggedin && _httpWrapper._CurrentResponseString.Contains("true"))
                                {
                                    DownloadData(dicURLToken["Token"].Trim(), URL.Trim(), RFQ_mailFiles[j]);
                                }
                                else
                                {
                                    LogText = "Unable to navigate to URL for mail file " + Path.GetFileName(RFQ_mailFiles[j]);
                                }
                            }
                            else
                            {//1001	Unable to find URL in file

                                //if (dicURLToken["URL"] == "") sAuditMesage = "URL not found for navigation for " + Path.GetFileName(RFQ_mailFiles[j]);
                                //else if (dicURLToken["Token"] == "") sAuditMesage = "Token not found for " + Path.GetFileName(RFQ_mailFiles[j]);
                                if (dicURLToken["URL"] == "") sAuditMesage = "LeS-1001:Unable to find URL in file " + Path.GetFileName(RFQ_mailFiles[j]);
                                else if (dicURLToken["Token"] == "") sAuditMesage = "LeS-1002:Unable to find Token in file " + Path.GetFileName(RFQ_mailFiles[j]);
                                WriteErrorLog_With_Screenshot(sAuditMesage);
                                if (File.Exists(MailTextPath + "\\Error\\" + Path.GetFileName(RFQ_mailFiles[j]))) File.Delete(MailTextPath + "\\Error\\" + Path.GetFileName(RFQ_mailFiles[j]));
                                File.Move(RFQ_mailFiles[j], MailTextPath + "\\Error\\" + Path.GetFileName(RFQ_mailFiles[j]));
                            }

                        }
                    }
                }
                else LogText = "No RFQ files found.";
                LogText = "RFQ processing stopped.";
            }
            catch (Exception ex)
            {
                string eFile = PrintScreenPath + "\\DNVGL_" + this.docType + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + BuyerCode + "_" + SupplierCode + ".png";
                if (!PrintScreen(eFile)) eFile = "";
                LogText = "Exception while processing RFQ : " + ex.GetBaseException().ToString();
            }
        }

        public void GetMailTextFiles()
        {
            RFQ_mailFiles.Clear();
            PO_mailFiles.Clear();
            Directory.CreateDirectory(MailTextPath + "\\Error");
            Directory.CreateDirectory(MailTextPath + "\\Backup");

            DirectoryInfo _dir = new DirectoryInfo(MailTextPath);
            FileInfo[] _Files = _dir.GetFiles();


            foreach (FileInfo _MtmlFile in _Files)
            {
               // _txtData.Text = File.ReadAllText(_MtmlFile.FullName);
                string[] lines = File.ReadAllLines(_MtmlFile.FullName);
                //if (_txtData.Text.Contains("Subject:"))
                if (lines[0].Contains("Subject:"))
                {
                    //if (_txtData.Text.Contains("Purchase Order"))
                    if (lines[0].Contains("Purchase Order"))
                        PO_mailFiles.Add(_MtmlFile.FullName);
                    //else if (_txtData.Text.Contains("request for quotation"))
                    else if (lines[0].Contains("request for quotation"))
                        RFQ_mailFiles.Add(_MtmlFile.FullName);
                    else
                    {
                        LogText = "Invalid mail file " + Path.GetFileName(_MtmlFile.FullName);
                        File.Move(_MtmlFile.FullName, MailTextPath + "\\Error\\" + Path.GetFileName(_MtmlFile.FullName));
                        //CreateAuditFile(Path.GetFileName(_MtmlFile.FullName), "DNVGL_HTTP", "", "Error", "Invalid mail file " + Path.GetFileName(_MtmlFile.FullName), BuyerCode, SupplierCode, AuditPath);
                        CreateAuditFile(Path.GetFileName(_MtmlFile.FullName), "DNVGL_HTTP", "", "Error", "LeS-1004.1:Unable to process file " + Path.GetFileName(_MtmlFile.FullName) + " - it is invalid", BuyerCode, SupplierCode, AuditPath);
                    }
                }
            }
        }

        #region commented
        //private Dictionary<string, string> GetURL(string emlFile)
        //{
        //    string strUrl = "", strToken = "";
        //    Dictionary<string, string> dicData = new Dictionary<string, string>();
        //    string[] _domains=new string[]{"supplierportal.dnvgl.com"};
        //    try
        //    {
        //        LogText = "Reading file " + Path.GetFileName(emlFile);

        //        _txtData.Text = File.ReadAllText(emlFile);
        //        string cURL = GetCorrectURL(_txtData, EncyptUrls, _domains);


        //        if (!_txtData.Text.Contains("<html>"))
        //        {
        //            _txtData.Text = _txtData.Text.Replace("%2F", "//");
        //            _txtData.Text = _txtData.Text.Replace("%3F", "?");
        //            _txtData.Text = _txtData.Text.Replace("%3A", ":");
        //            _txtData.Text = _txtData.Text.Replace("%40", "@");
        //            _txtData.Text = _txtData.Text.Replace("%3D", "=");
        //            _txtData.Text = _txtData.Text.Replace("%26", "&");

        //            if (_txtData.Text.Contains("https://supplierportal.dnvgl.com/"))
        //            {
        //                for (int i = 0; i < _txtData.Lines.Length; i++)
        //                {
        //                    string line = _txtData.Lines[i];
        //                    if (line.Contains("https://supplierportal.dnvgl.com/"))
        //                    {
        //                        //get URL
        //                        int startIndex = line.IndexOf("https://supplierportal.dnvgl.com/");
        //                        if (line.Length > startIndex)
        //                        {
        //                            strUrl = line.Substring(startIndex);
        //                        }
        //                        else strUrl = line.Trim();

        //                        int endIndex = strUrl.IndexOf(">");
        //                        if (line.Length > endIndex)
        //                        {
        //                            strUrl = strUrl.Substring(0, endIndex);
        //                        }
        //                        else strUrl = line.Trim();

        //                        //get Token
        //                        int tstartIndex = line.IndexOf("token");
        //                        if (line.Length > tstartIndex)
        //                        {
        //                            strToken = line.Substring(tstartIndex);
        //                            strToken = strToken.Substring(6);
        //                            strToken.Trim();
        //                        }

        //                        break;
        //                    }
        //                }
        //            }
        //            strUrl = strUrl.Trim().Replace("&amp;", "&").Trim().TrimStart('<').TrimStart('"').TrimEnd('>').TrimEnd('"').Trim();
        //            dicData.Add("URL", strUrl);
        //            dicData.Add("Token", strToken);
        //        }
        //        else
        //        {
        //            if (_txtData.Text.Contains("https://supplierportal.dnvgl.com/"))
        //            {
        //                for (int i = 0; i < _txtData.Lines.Length; i++)
        //                {
        //                    string line = _txtData.Lines[i];
        //                    if (line.Contains("https://supplierportal.dnvgl.com/"))
        //                    {
        //                        //get URL
        //                        int startIndex = line.IndexOf("https://supplierportal.dnvgl.com/");
        //                        if (line.Length > startIndex)
        //                        {
        //                            strUrl = line.Substring(startIndex);
        //                        }
        //                        else strUrl = line.Trim();

        //                        int endIndex = strUrl.IndexOf("\"");
        //                        if (line.Length > endIndex)
        //                        {
        //                            strUrl = strUrl.Substring(0, endIndex);
        //                        }
        //                        else strUrl = line.Trim();

        //                    }
        //                    //get Token
        //                    if (line.Contains("security token"))
        //                    {
        //                        i++;
        //                        string line1 = _txtData.Lines[i];

        //                        if (line1.Contains("<b>"))
        //                        {
        //                            int tstartIndex = line1.IndexOf("<b>");
        //                            if (line1.Length > tstartIndex)
        //                            {
        //                                strToken = line1.Substring(tstartIndex);
        //                                strToken = strToken.Substring(3);
        //                                int endindex = strToken.IndexOf("</b>");
        //                                if (endindex > -1)
        //                                {
        //                                    strToken = strToken.Substring(0, endindex);
        //                                }
        //                                strToken.Trim();
        //                            }
        //                        }
        //                        else//19-6-18
        //                        {
        //                            i--;
        //                            line1 = _txtData.Lines[i];
        //                            int tstartIndex = line1.IndexOf("<b>");
        //                            if (line1.Length > tstartIndex)
        //                            {
        //                                strToken = line1.Substring(tstartIndex);
        //                                strToken = strToken.Substring(3);
        //                                int endindex = strToken.IndexOf("</b>");
        //                                if (endindex > -1)
        //                                {
        //                                    strToken = strToken.Substring(0, endindex);
        //                                }
        //                                strToken.Trim();
        //                            }
        //                        }
        //                    }
        //                    if (strUrl != "" && strToken != "")
        //                    { //isHtml = true; 
        //                        break;
        //                    }
        //                    //  }
        //                }
        //            }
        //            strUrl = strUrl.Trim().Replace("&amp;", "&").Trim().TrimStart('<').TrimStart('"').TrimEnd('>').TrimEnd('"').Trim();
        //            dicData.Add("URL", strUrl);
        //            dicData.Add("Token", strToken);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogText = "Exception while getting URL for RFQ : " + ex.GetBaseException().ToString();
        //    }
        //    return dicData;
        //}
        #endregion

        //Changed by kalpita on 26/06/2019
        private Dictionary<string, string> GetURL(string emlFile)
        {
            string strUrl = "", strToken = "";
            Dictionary<string, string> dicData = new Dictionary<string, string>();
            string[] _domains = new string[] { "supplierportal.dnvgl.com" };
            try
            {
                LogText = "Reading file " + Path.GetFileName(emlFile);
                _txtData.Text = File.ReadAllText(emlFile);

                if (!_txtData.Text.Contains("<html>"))
                {
                    _txtData.Text = _txtData.Text.Replace("%2F", "//");
                    _txtData.Text = _txtData.Text.Replace("%3F", "?");
                    _txtData.Text = _txtData.Text.Replace("%3A", ":");
                    _txtData.Text = _txtData.Text.Replace("%40", "@");
                    _txtData.Text = _txtData.Text.Replace("%3D", "=");
                    _txtData.Text = _txtData.Text.Replace("%26", "&");
                    strUrl = GetCorrectURL(_txtData, "https://supplierportal.dnvgl.com/", _domains);
                    for (int i = 0; i < _txtData.Lines.Length; i++)
                    {
                        string line = _txtData.Lines[i];
                        if (line.Contains("security token") || line.Contains(" token"))//changed by Kalpita on 02/07/2019
                        {
                            int tstartIndex = line.IndexOf("token");
                            if (line.Length > tstartIndex)
                            {
                                strToken = line.Substring(tstartIndex);
                                strToken = strToken.Substring(6);
                                strToken.Trim();
                                break;
                            }
                        }
                    }
                }
                else
                {
                    LogText = "Correct url started";
                    strUrl = GetCorrectURL(_txtData, "https://supplierportal.dnvgl.com/", _domains);
                    LogText = strUrl +"Correct url ended";
                    for (int i = 0; i < _txtData.Lines.Length; i++)
                    {
                        string line = _txtData.Lines[i];
                        if (line.Contains("security token"))
                        {
                          //  i++;
                          //  string line1 = _txtData.Lines[i];
                            string line1 = line;
                            int tstartIndex = line1.IndexOf("token");
                            string cTokenLine = line1.Substring(tstartIndex);
                            if (cTokenLine.Contains("<b>"))
                            {
                                int tIndex1 = cTokenLine.IndexOf("<b>");
                                if (cTokenLine.Length > tIndex1)
                                {
                                    strToken = cTokenLine.Substring(tIndex1);
                                    strToken = strToken.Substring(3);
                                    int endindex = strToken.IndexOf("</b>");
                                    if (endindex > -1)
                                    {
                                        strToken = strToken.Substring(0, endindex);
                                    }
                                    strToken.Trim();
                                }
                            }

                            #region commented
                            //if (line1.Contains("<b>"))
                            //{
                            //    int tstartIndex = line1.IndexOf("<b>");
                            //    if (line1.Length > tstartIndex)
                            //    {
                            //        strToken = line1.Substring(tstartIndex);
                            //        strToken = strToken.Substring(3);
                            //        int endindex = strToken.IndexOf("</b>");
                            //        if (endindex > -1)
                            //        {
                            //            strToken = strToken.Substring(0, endindex);
                            //        }
                            //        strToken.Trim();
                            //    }
                            //}
                            //else//19-6-18
                            //{
                            //    i--;
                            //    line1 = _txtData.Lines[i];
                            //    int tstartIndex = line1.IndexOf("<b>");
                            //    if (line1.Length > tstartIndex)
                            //    {
                            //        strToken = line1.Substring(tstartIndex);
                            //        strToken = strToken.Substring(3);
                            //        int endindex = strToken.IndexOf("</b>");
                            //        if (endindex > -1)
                            //        {
                            //            strToken = strToken.Substring(0, endindex);
                            //        }
                            //        strToken.Trim();
                            //    }
                            //}
                            #endregion
                        }
                    }
                }
                strUrl = strUrl.Trim().Replace("&amp;", "&").Trim().TrimStart('<').TrimStart('"').TrimEnd('>').TrimEnd('"').Trim();
                dicData.Add("URL", strUrl);
                dicData.Add("Token", strToken);
            }
            catch (Exception ex)
            {
                LogText = ex.InnerException.ToString();
                LogText = "Exception while getting URL  : " + ex.GetBaseException().ToString();
            }
            return dicData;
        }


        //added on 26/06/2019 by Kalpita
        private string GetCorrectURL(RichTextBox _txtData, string Domain, string[] DomainList)
        {
            string _resultURL = "";
            string cDomain = Domain.Replace("https://", "").Replace('/', ' ').Trim();
            if (_txtData.Text.Contains(cDomain))
            {
                for (int i = 0; i < _txtData.Text.Length; i++)
                {
                    string line = _txtData.Lines[i];
                    if (line.Contains(EncyptUrls) && line.Contains(cDomain))
                    {
                        this.URL = line;
                        int startIndex = line.IndexOf("https"); if (startIndex == -1) { startIndex = line.IndexOf("http"); }
                        else
                        {
                            if (line.Length > startIndex)
                            {
                                LogText = line + "|" + startIndex;
                                string text = line.Substring(startIndex).Split('>')[0];
                                this.URL = text.Replace('"', ' ').Trim();
                            }
                            if (LoadURL("", "", "", true))
                            {
                                _resultURL = Convert.ToString(_httpWrapper._CurrentResponse.ResponseUri);
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (line.Contains("https://supplierportal.dnvgl.com/"))
                        {
                            //get URL
                            int startIndex = line.IndexOf("https://supplierportal.dnvgl.com/");
                            if (line.Length > startIndex)
                            {
                                LogText = line + "|" + startIndex;
                                _resultURL = line.Substring(startIndex);
                            }
                            else _resultURL = line.Trim();

                            int endIndex = _resultURL.IndexOf(">");
                            if (endIndex == -1)
                            {
                                endIndex = _resultURL.IndexOf("\"");
                            }
                            if (line.Length > endIndex)
                            {
                                LogText = _resultURL + "|" + endIndex;
                                _resultURL = _resultURL.Substring(0, endIndex);
                            }
                            else _resultURL = line.Trim();
                          if(!string.IsNullOrEmpty(_resultURL)) break;
                        }
                    }
                }
            }
            else { }
            LogText = _resultURL;
            return _resultURL;
        }

        public void DownloadData(string Token, string sURL, string MailTextFile)
        {
            bool isCatch = false;
            try
            {
                this.URL = sURL.Trim().Replace("Security/PostLogin", "Requests/getrequest");
                _httpWrapper.AcceptMimeType = "*/*";
                try
                {
                    LoadURL("", "", "");
                }
                catch (Exception ex)
                {
                    this.URL = sURL.Trim().Replace("Security/PostLogin", "Requests/getrequest");


                    isCatch = true;
                    //WriteErrorLog_With_Screenshot("Exception while loading RFQ page : " + ex.GetBaseException().Message.ToString());
                    WriteErrorLog_With_Screenshot("LeS-1016:Unable to load url due to " + ex.GetBaseException().Message.ToString());
                    if (File.Exists(Path.GetDirectoryName(MailTextFile) + "\\Error\\" + Path.GetFileName(MailTextFile))) File.Delete(Path.GetDirectoryName(MailTextFile) + "\\Error\\" + Path.GetFileName(MailTextFile));
                    File.Move(MailTextFile, Path.GetDirectoryName(MailTextFile) + "\\Error\\" + Path.GetFileName(MailTextFile));
                    LogText = "File moved to Error folder.";
                }

                if (!isCatch)
                {
                    var _library = (RootObject)JsonConvert.DeserializeObject(_httpWrapper._CurrentResponseString, typeof(RootObject));
                    if (_library != null)
                    {
                        LogText = "RFQ data extracted successfully.";

                        #region header details
                        VRNO = _library.requestCode;
                        string Vessel = _library.shipName;

                        string PortName = "", PortCode = "";
                        if (_library.etaPort != "")
                        {
                            if (_library.etaPort.Contains("/"))
                            {
                                PortCode = _library.etaPort.Split('/')[0];
                                PortName = _library.etaPort.Split('/')[1];
                            }
                            else PortCode = _library.etaPort;
                        }

                        DateTime etaDate = new DateTime();
                        if (_library.etaDate != null)
                        {
                            if (convert.ToString(_library.etaDate) != "" && convert.ToString(_library.etaDate) != "-" && _library.etaDate != DateTime.MinValue && !convert.ToString(_library.etaDate).Contains("1900"))
                                etaDate = convert.ToDateTime(_library.etaDate);
                            // etaDate = DateTime.ParseExact(convert.ToString(_library.etaDate), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }

                        DateTime delDate = new DateTime();
                        if (_library.deliveryDate != null)
                        {
                            if (convert.ToString(_library.deliveryDate) != "" && convert.ToString(_library.deliveryDate) != "-" && !convert.ToString(_library.deliveryDate).Contains("1900"))
                            {
                                delDate = convert.ToDateTime(_library.deliveryDate);

                            }
                        }

                        string RemarkHeader = "", RemarkTitle = "";
                        if (_library.delAddName != "" || _library.delAddStreet != "" || _library.delAddTown != "")
                        {
                            RemarkHeader = "Place of Delivery: " + _library.delAddName + "," + _library.delAddStreet + "," + _library.delAddTown;
                        }

                        if (_library.name != "")
                            RemarkTitle = _library.name;
                        #endregion

                        #region Address Details
                        string BName = "", BPhone = "", BEmail = "", sName = "", sAddress = "", sEmail = "", eFile = "";
                        BName = _library.creatorName;
                        BPhone = _library.creatorPhone;
                        BEmail = _library.creatorEmail;
                        sName = _library.supplierAddrName;
                        if (_library.supplierAddrZIP != "")
                            sAddress = _library.supplierAddrStreet + "," + _library.supplierAddrTown + "," + _library.supplierAddrCountry + "," + _library.supplierAddrZIP;
                        else
                            sAddress = _library.supplierAddrStreet + "," + _library.supplierAddrTown + "," + _library.supplierAddrCountry;

                        //  sEmail =  _library.supplierContEmail;
                        sEmail = (IsSuppEmailDisplay) ? _library.supplierContEmail : string.Empty ;//added by kalpita on 02/08/2019(kloska)
                        #endregion

                        //#region Print page
                        //eFile = PrintScreenPath + "\\RFQ_" + VRNO + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                        //if (!PrintScreen(eFile)) eFile = "";

                        //#endregion

                        LeSXML.LeSXML _lesXml = new LeSXML.LeSXML();
                        if (GetRFQHeader(VRNO, Vessel, PortCode, PortName, etaDate.ToString("yyyyMMdd"), delDate.ToString("yyyyMMdd"), RemarkHeader, RemarkTitle, ref _lesXml, Token, txtURL))
                        {
                            //if (File.Exists(eFile))
                            //    _lesXml.OrigDocFile = Path.GetFileName(eFile);
                            if (GetItems(_library.itemGroups, ref _lesXml))
                            {
                                if (GetAddress(ref _lesXml, BName, BPhone, BEmail, sName, sAddress, sEmail))
                                {
                                    _lesXml.FileName = Path.GetFileNameWithoutExtension(MailTextFile)+"_RFQ_" + VRNO + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xml";
                                    if (Convert.ToInt32(_lesXml.Total_LineItems) > 0)
                                    {
                                        #region download pdf attachment
                                        this.URL = "https://supplierportal.dnvgl.com/api/exportsPDF/get/" + _library.id;
                                        string Filename = PrintScreenPath + "\\RFQ_" + VRNO + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                                        DownloadRFQ(this.URL, Filename, "");
                                        #endregion

                                        if (File.Exists(Filename))
                                            _lesXml.OrigDocFile = Path.GetFileName(Filename);

                                        string CurrenctXMLFile = _lesXml.FilePath + "\\" + _lesXml.FileName;                                     
                                        _lesXml.WriteXML();
                                        if (File.Exists(CurrenctXMLFile))
                                        {
                                            LogText = Path.GetFileName(CurrenctXMLFile) + " downloaded successfully.";
                                            CreateAuditFile(Path.GetFileName(CurrenctXMLFile), "DNVGL_HTTP_"+docType, VRNO, "Downloaded", Path.GetFileName(CurrenctXMLFile) + " downloaded successfully.", BuyerCode, SupplierCode, AuditPath);
                                            if (File.Exists(Path.GetDirectoryName(MailTextFile) + "\\Backup\\" + Path.GetFileName(MailTextFile))) File.Delete(Path.GetDirectoryName(MailTextFile) + "\\Backup\\" + Path.GetFileName(MailTextFile));
                                            File.Move(MailTextFile, Path.GetDirectoryName(MailTextFile) + "\\Backup\\" + Path.GetFileName(MailTextFile));
                                            LogText = "File moved to Backup folder.";
                                        }
                                        else
                                        {
                                            eFile = PrintScreenPath + "\\DNVGL_" + this.docType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + BuyerCode + "_" + SupplierCode + ".png";
                                            LogText = "Unable to download file " + Path.GetFileName(CurrenctXMLFile);
                                            //CreateAuditFile(eFile, "DNVGL_HTTP_"+docType, VRNO, "Error", "Unable to download file " + Path.GetFileName(CurrenctXMLFile) + " for ref " + VRNO + ".", BuyerCode, SupplierCode, AuditPath);
                                            CreateAuditFile(eFile, "DNVGL_HTTP_" + docType, VRNO, "Error", "LeS-1004:Unable to process file " + Path.GetFileName(CurrenctXMLFile) + " for " + VRNO + ".", BuyerCode, SupplierCode, AuditPath);
                                            if (!PrintScreen(eFile)) eFile = "";
                                            if (File.Exists(Path.GetDirectoryName(MailTextFile) + "\\Error\\" + Path.GetFileName(MailTextFile))) File.Delete(Path.GetDirectoryName(MailTextFile) + "\\Error\\" + Path.GetFileName(MailTextFile));
                                            File.Move(MailTextFile, Path.GetDirectoryName(MailTextFile) + "\\Error\\" + Path.GetFileName(MailTextFile));
                                            LogText = "File moved to Error folder.";
                                        }
                                    }
                                }
                                else
                                {//1040	Unable to get  details - 'XXXX' Field(s)not present.

                                    //WriteErrorLog_With_Screenshot("Unable to get address details");
                                    WriteErrorLog_With_Screenshot("LeS-1040:Unable to get details - address Field(s) not present");
                                    File.Move(MailTextFile, Path.GetDirectoryName(MailTextFile) + "\\Error\\" + Path.GetFileName(MailTextFile));
                                }
                            }
                            else
                            {
                                //WriteErrorLog_With_Screenshot("Unable to get item details");
                                WriteErrorLog_With_Screenshot("LeS-1040:Unable to get details - item Field(s) not present");
                                File.Move(MailTextFile, Path.GetDirectoryName(MailTextFile) + "\\Error\\" + Path.GetFileName(MailTextFile));
                            }
                        }
                        else
                        {
                            //WriteErrorLog_With_Screenshot("Unable to get RFQ header details");
                            WriteErrorLog_With_Screenshot("LeS-1040:Unable to get details - RFQ header Field(s) not present");
                            File.Move(MailTextFile, Path.GetDirectoryName(MailTextFile) + "\\Error\\" + Path.GetFileName(MailTextFile));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
//                WriteErrorLog_With_Screenshot("Exception while processing RFQ : " + ex.GetBaseException().Message.ToString());
                WriteErrorLog_With_Screenshot("LeS-1004:Unable to process file due to " + ex.GetBaseException().Message.ToString());
            }
        }

        public bool GetRFQHeader(string VRNO, string Vessel, string PortCode, string PortName, string etaDate, string delDate, string remarkHeader, string remarkTitle, ref LeSXML.LeSXML _lesXml, string token, string URL)
        {
            bool result = false;
            try
            {
                LogText = "Start getting header details for RFQ with VRNo " + VRNO + ".";
                _lesXml = new LeSXML.LeSXML();
                _lesXml.DocID = DateTime.Now.ToString("yyyyMMddhhmmss");
                _lesXml.Created_Date = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Doc_Type = "RFQ";
                _lesXml.Dialect = "DNVGL";
                _lesXml.Version = "1";
                _lesXml.Date_Document = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Date_Preparation = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Sender_Code = BuyerCode;
                _lesXml.Recipient_Code = SupplierCode;
                _lesXml.DocReferenceID = VRNO;
                _lesXml.DocLinkID = URL + "|" + token;
                _lesXml.Active = "1";
                _lesXml.Vessel = Vessel;
                _lesXml.BuyerRef = VRNO;
                _lesXml.PortCode = PortCode;
                _lesXml.PortName = PortName;
                _lesXml.Date_ETA = etaDate;
                if (delDate != "00010101")
                _lesXml.Date_Delivery = delDate;
                _lesXml.Remark_Header = remarkHeader;
                _lesXml.Remark_Title = remarkTitle;
                LogText = "Getting header details completed successfully.";
                result = true;
                return result;
            }
            catch (Exception ex) { LogText = "Unable to get header details." + ex.GetBaseException().ToString(); result = false; return result; }
        }

        public bool GetItems(List<ItemGroup> ItemGroups, ref LeSXML.LeSXML _lesXml)
        {
            int LinItemCount = 0;
            bool result = false;
            try
            {
                _lesXml.LineItems.Clear();
                LogText = "Start Getting RFQ LineItem details.";

                foreach (ItemGroup Items in ItemGroups)
                {

                    foreach (Item item in Items.items)
                    {
                        LeSXML.LineItem _item = new LeSXML.LineItem();
                        try
                        {
                            _item.Number = Convert.ToString(LinItemCount + 1);
                            _item.OrigItemNumber = Convert.ToString(LinItemCount + 1);
                            _item.Name = item.name;
                            _item.ItemRef = item.partNumber;
                            _item.OriginatingSystemRef = item.itemOrderNumber;
                            _item.Quantity = convert.ToString(item.stockQuantity);
                            _item.Unit = item.stockUnit;
                            _item.Remark = item.itemText; //"Required Qty: " + item.stockQuantity + " " + "Required unit: " + item.stockUnit;
                            _item.Discount = "0";
                            _item.ListPrice = "0";
                            _item.LeadDays = "0";
                            _item.Equipment = Items.name;
                            _item.EquipMaker = Items.makersName;
                            if (Items.serialNumber != "")
                                _item.EquipRemarks = "Serial No: " + Items.serialNumber;
                            _lesXml.LineItems.Add(_item);
                            LinItemCount++;
                        }
                        catch (Exception ex)
                        { LogText = ex.GetBaseException().ToString(); }
                    }
                }
                _lesXml.Total_LineItems = Convert.ToString(LinItemCount);
                LogText = "Getting LineItem details successfully";
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                LogText = "Exception while getting RFQ Items: " + ex.GetBaseException().ToString();
                result = false;
                return result;
            }
        }

        public bool GetAddress(ref LeSXML.LeSXML _lesXml, string BuyerName, string BuyerPhone, string BuyerEmail, string suppName, string suppAddress, string sEmail)
        {
            bool result = false;
            try
            {
                _lesXml.Addresses.Clear();
                LogText = "Start Getting RFQ address details.";
                LeSXML.Address _xmlAdd = new LeSXML.Address();

                _xmlAdd.Qualifier = "BY";
                _xmlAdd.AddressName = dctAppSettings["BuyerName"].Trim();
                _xmlAdd.ContactPerson = BuyerName;
                _xmlAdd.Phone = BuyerPhone;
                _xmlAdd.eMail = BuyerEmail;
                _lesXml.Addresses.Add(_xmlAdd);

                _xmlAdd = new LeSXML.Address();
                _xmlAdd.Qualifier = "VN";
                _xmlAdd.AddressName = suppName;
                _xmlAdd.Address1 = suppAddress;
                _xmlAdd.eMail = sEmail;
                _lesXml.Addresses.Add(_xmlAdd);
                LogText = "Getting address details successfully";
                result = true; return result;
            }
            catch (Exception ex)
            {
                LogText = "Exception while getting address details: " + ex.GetBaseException().ToString(); result = false; return result;
            }
        }

        #endregion

        #region PO

        public void ProcessPO()
        {
            try
            {
                docType = "PO";
                LogText = "";
                LogText = "Purchase Order processing started.";
                GetMailTextFiles();
                if (PO_mailFiles.Count > 0)
                {
                    for (int j = PO_mailFiles.Count - 1; j >= 0; j--)
                    {
                        Dictionary<string, string> dicURLToken = GetURL(PO_mailFiles[j]);
                        if (dicURLToken != null && dicURLToken.Count > 0)
                        {
                            if (dicURLToken["URL"].Trim() != "" && dicURLToken["URL"].Trim().Contains("https://supplierportal.dnvgl.com/") && dicURLToken["Token"].Trim() != "")
                            {
                                if (!_httpWrapper._AddRequestHeaders.ContainsKey("Origin"))
                                    _httpWrapper._AddRequestHeaders.Add("Origin", @"https://supplierportal.dnvgl.com");
                                _httpWrapper.ContentType = "application/json";

                                dctPostDataValues.Clear();
                                dctPostDataValues.Add("Password", dicURLToken["Token"].Trim());
                                dctPostDataValues.Add("Type", "2");
                                txtURL = dicURLToken["URL"].Trim();
                                string[] ArrURL = dicURLToken["URL"].Trim().Split('/');
                                URL = ArrURL[0] + "//" + ArrURL[2] + "/api/Security/PostLogin/" + ArrURL[4];
                                string _postdata = GetPostData(true);
                                bool isLoggedin = _httpWrapper.PostURL(URL, _postdata, "", "", "");
                                if (isLoggedin && _httpWrapper._CurrentResponseString.Contains("true"))
                                {
                                    DownloadPODocument(URL, dicURLToken["Token"].Trim(), PO_mailFiles[j]);
                                }
                                else
                                {
                                    LogText = "Unable to navigate to URL for mail file " + Path.GetFileName(PO_mailFiles[j]);
                                }
                            }
                            else
                            {
                               // if (dicURLToken["URL"] == "") sAuditMesage = "URL not found for navigation for " + Path.GetFileName(PO_mailFiles[j]);
                                //else if (dicURLToken["Token"] == "") sAuditMesage = "Token not found for " + Path.GetFileName(PO_mailFiles[j]);
                                if (dicURLToken["URL"] == "") sAuditMesage = "LeS-1001:Unable to find URL in file " + Path.GetFileName(RFQ_mailFiles[j]);
                                else if (dicURLToken["Token"] == "") sAuditMesage = "LeS-1002:Unable to find Token in file " + Path.GetFileName(RFQ_mailFiles[j]);
                                WriteErrorLog_With_Screenshot(sAuditMesage);
                                if (File.Exists(MailTextPath + "\\Error\\" + Path.GetFileName(PO_mailFiles[j]))) File.Delete(MailTextPath + "\\Error\\" + Path.GetFileName(PO_mailFiles[j]));
                                File.Move(PO_mailFiles[j], MailTextPath + "\\Error\\" + Path.GetFileName(PO_mailFiles[j]));
                            }
                        }
                    }
                }
                else LogText = "No PO files found.";
                LogText = "Purchase Order processing stopped.";
            }
            catch (Exception ex)
            {
                string eFile = PrintScreenPath + "\\DNVGL_" + this.docType + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + BuyerCode + "_" + SupplierCode + ".png";
                if (!PrintScreen(eFile)) eFile = "";
                LogText = "Exception while processing PO : " + ex.GetBaseException().ToString();
            }
        }

        public void DownloadPODocument(string sURL, string Token, string MailTextFile)
        {
            bool isCatch = false;
            try
            {
                this.URL = sURL.Trim().Replace("Security/PostLogin", "Orders/getorder");
                _httpWrapper.AcceptMimeType = "*/*";
                try
                {
                    LoadURL("", "", "");
                }
                catch (Exception ex)
                {
                    isCatch = true;
                    //WriteErrorLog_With_Screenshot("Exception while loading PO page : " + ex.GetBaseException().Message.ToString());
                    WriteErrorLog_With_Screenshot("LeS-1016:Unable to load url due to " + ex.GetBaseException().Message.ToString());
                    if (File.Exists(Path.GetDirectoryName(MailTextFile) + "\\Error\\" + Path.GetFileName(MailTextFile))) File.Delete(Path.GetDirectoryName(MailTextFile) + "\\Error\\" + Path.GetFileName(MailTextFile));
                    File.Move(MailTextFile, Path.GetDirectoryName(MailTextFile) + "\\Error\\" + Path.GetFileName(MailTextFile));
                    LogText = "File moved to Error folder.";
                }

                if (!isCatch)
                {
                    var _library = (OrderRootObject)JsonConvert.DeserializeObject(_httpWrapper._CurrentResponseString, typeof(OrderRootObject));
                    if (_library != null)
                    {
                        LogText = "PO data extracted successfully.";

                        LeSXML.LeSXML _lesXml = new LeSXML.LeSXML();
                        #region header details
                        LogText = "Start Getting PO header details.";
                        _lesXml.DocID = DateTime.Now.ToString("yyyyMMddhhmmss");
                        _lesXml.Created_Date = DateTime.Now.ToString("yyyyMMdd");
                        _lesXml.Doc_Type = "PO";
                        _lesXml.Sender_Code = BuyerCode;
                        _lesXml.Recipient_Code = SupplierCode;
                        _lesXml.Active = "1";
                        _lesXml.Remark_Title = _library.orderName;
                        _lesXml.DocLinkID = txtURL + "|" + Token;
                        _lesXml.Vessel = _library.shipName;
                        _lesXml.BuyerRef = VRNO = _library.orderCode;
                        DateTime orderDate = new DateTime();
                        if (_library.orderedAt != null)
                        {
                            if (convert.ToString(_library.orderedAt) != "" && convert.ToString(_library.orderedAt) != "-" && _library.orderedAt != DateTime.MinValue && !convert.ToString(_library.orderedAt).Contains("1900"))
                            {
                                orderDate = convert.ToDateTime(_library.orderedAt);
                                _lesXml.Date_Document = orderDate.ToString("yyyyMMdd");
                            }
                        }

                        if (_library.etaPort != "")
                        {
                            if (_library.etaPort.Contains("/"))
                            {
                                _lesXml.PortCode = _library.etaPort.Split('/')[0].Trim();
                                _lesXml.PortName = _library.etaPort.Split('/')[1].Trim();
                            }
                            else _lesXml.PortCode = _library.etaPort.Trim();
                        }

                        _lesXml.Currency = _library.currencyShortName;

                        DateTime delDate = new DateTime();
                        if (_library.deliveryDate != null)
                        {
                            if (convert.ToString(_library.deliveryDate) != "" && convert.ToString(_library.deliveryDate) != "-" && _library.deliveryDate != DateTime.MinValue && !convert.ToString(_library.deliveryDate).Contains("1900"))
                            {
                                delDate = convert.ToDateTime(_library.deliveryDate);
                                _lesXml.Date_Delivery = delDate.ToString("yyyyMMdd");
                            }
                        }

                        DateTime etaDate = new DateTime();
                        if (_library.etaDate != null)
                        {
                            if (convert.ToString(_library.etaDate) != "" && convert.ToString(_library.etaDate) != "-" && _library.etaDate != DateTime.MinValue && !convert.ToString(_library.etaDate).Contains("1900"))
                            {
                                etaDate = convert.ToDateTime(_library.etaDate);
                                _lesXml.Date_ETA = etaDate.ToString("yyyyMMdd");
                            }
                        }

                        _lesXml.LeadTimeDays = "";
                        _lesXml.Remark_DeliveryTerms = "";
                        string Remarks_Header = "";
                        Remarks_Header = _library.supplierReference;
                        if (_library.orderRemark != "")
                            Remarks_Header = Remarks_Header + ", " + _library.orderRemark;
                        _lesXml.Remark_Header = Remarks_Header;
                        _lesXml.Reference_Document = "";

                        //prices
                        _lesXml.Total_LineItems_Net = convert.ToString(_library.amount.ToString("0.00"));
                        _lesXml.Total_Other = convert.ToString(_library.additionalCharges.ToString("0.00"));
                        double GrandTotal = _library.amount + (_library.amount * (_library.discount / 100) + _library.additionalCharges);
                        _lesXml.Total_Net_Final = GrandTotal.ToString("0.00");
                        LogText = "Getting header details successfully";
                        #endregion

                        #region address details
                        Dictionary<string, string> AddDetails = new Dictionary<string, string>();
                        AddDetails.Add("BContact", _library.creatorName.Trim());
                        AddDetails.Add("BPhone", _library.creatorPhone.Trim());
                        AddDetails.Add("BEmail", _library.creatorEmail.Trim());
                        AddDetails.Add("BAddress", _library.shippingCompName.Trim() + "," + _library.shippingCompStreet.Trim() + "," + _library.shippingCompZip.Trim());
                        AddDetails.Add("SContact", _library.supplierContName.Trim());
                        AddDetails.Add("SAddressName", _library.supplierAddrName.Trim());
                        AddDetails.Add("sAddress", _library.supplierAddrStreet.Trim() + "," + _library.supplierAddrZIP.Trim() + " " + _library.supplierAddrTown.Trim() + "," + _library.supplierAddrCountry.Trim());
                        AddDetails.Add("sEmail", _library.supplierContEmail.Trim());
                        //if (_library.agentContactName != "")
                        //{
                        //    if (_library.agentContactName.Contains("."))
                        //    {
                        //        AddDetails.Add("vAdressName", _library.agentContactName.Split('.')[0].Trim());
                        //        AddDetails.Add("vContact", _library.agentContactName.Split('.')[1].Trim());
                        //    }
                        //    else if (_library.agentContactName.Contains(" Mr "))
                        //    {
                        //        AddDetails.Add("vAdressName", _library.agentContactName.Split(new string[] { " Mr " }, StringSplitOptions.None)[0].Trim());
                        //        AddDetails.Add("vContact", _library.agentContactName.Split(new string[] { " Mr " }, StringSplitOptions.None)[1].Trim());
                        //    }
                        //    else if (_library.agentContactName.Contains(" Ms "))
                        //    {
                        //        AddDetails.Add("vAdressName", _library.agentContactName.Split(new string[] { " Ms " }, StringSplitOptions.None)[0].Trim());
                        //        AddDetails.Add("vContact", _library.agentContactName.Split(new string[] { " Ms " }, StringSplitOptions.None)[1].Trim());
                        //    }
                        //    else
                        //    {
                        //        AddDetails.Add("vContact", _library.agentContactName.Trim());
                        //    }
                        //}
                        if (_library.agentContactName.Trim() != "")
                        {
                            AddDetails.Add("vContact", _library.agentContactSalutation + " " + _library.agentContactName);
                            if (_library.agentName.Trim() != "") AddDetails.Add("vAddressName", _library.agentName);
                        }
                        else
                        {
                            if (_library.agentName.Contains(" Mr "))
                            {
                                AddDetails.Add("vAddressName", _library.agentName.Split(new string[] { " Mr " }, StringSplitOptions.None)[0].Trim());
                                AddDetails.Add("vContact", _library.agentName.Split(new string[] { " Mr " }, StringSplitOptions.None)[1].Trim());
                            }
                            else if (_library.agentName.Contains(" Ms "))
                            {
                                AddDetails.Add("vAddressName", _library.agentName.Split(new string[] { " Ms " }, StringSplitOptions.None)[0].Trim());
                                AddDetails.Add("vContact", _library.agentName.Split(new string[] { " Ms " }, StringSplitOptions.None)[1].Trim());
                            }
                        }
                       
                        if (_library.agentStreet.Trim() != "" || _library.agentZipCode.Trim() != "" || _library.agentCountry.Trim() != "")
                            AddDetails.Add("vAddress", _library.agentStreet.Trim() + "," + _library.agentZipCode.Trim() + "," + _library.agentCountry.Trim());
                        if (_library.agentContactEmail != "") AddDetails.Add("vEmail", _library.agentContactEmail);
                        #endregion

                        //#region Print page
                        //string eFile = PrintScreenPath + "\\PO_" + VRNO + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                        //if (!PrintScreen(eFile)) eFile = "";

                        //#endregion
                        //if (File.Exists(eFile))
                        //    _lesXml.OrigDocFile = Path.GetFileName(eFile);
                        if (GetPOItems(_library.orderItemGroups, ref _lesXml))
                        {
                            if (GetPOAddress(ref _lesXml, AddDetails))
                            {
                                _lesXml.FileName = Path.GetFileNameWithoutExtension(MailTextFile) + "_PO_" + VRNO + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xml";
                                if (Convert.ToInt32(_lesXml.Total_LineItems) > 0)
                                {
                                    #region download pdf attachment
                                    this.URL = "https://supplierportal.dnvgl.com/api/orderPDF/get/" + _library.id;
                                    string Filename = PrintScreenPath + "\\PO_" + VRNO + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                                    DownloadRFQ(this.URL, Filename, "");
                                    #endregion

                                    if (File.Exists(Filename))
                                        _lesXml.OrigDocFile = Path.GetFileName(Filename);

                                    string CurrenctXMLFile = _lesXml.FilePath + "\\" + _lesXml.FileName;
                                    _lesXml.WriteXML();
                                    if (File.Exists(CurrenctXMLFile))
                                    {
                                        LogText = Path.GetFileName(CurrenctXMLFile) + " downloaded successfully.";
                                        CreateAuditFile(Path.GetFileName(CurrenctXMLFile), "DNVGL_HTTP_PO", VRNO, "Downloaded", Path.GetFileName(CurrenctXMLFile) + " downloaded successfully.", BuyerCode, SupplierCode, AuditPath);
                                        if (File.Exists(Path.GetDirectoryName(MailTextFile) + "\\Backup\\" + Path.GetFileName(MailTextFile))) File.Delete(Path.GetDirectoryName(MailTextFile) + "\\Backup\\" + Path.GetFileName(MailTextFile));
                                        File.Move(MailTextFile, Path.GetDirectoryName(MailTextFile) + "\\Backup\\" + Path.GetFileName(MailTextFile));
                                        LogText = "File moved to Backup folder.";
                                    }
                                    else
                                    {
                                        string eFile = PrintScreenPath + "\\DNVGL_" + this.docType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + BuyerCode + "_" + SupplierCode + ".png";
                                        LogText = "Unable to download file " + Path.GetFileName(CurrenctXMLFile);
                                        //CreateAuditFile(eFile, "DNVGL_HTTP_PO", VRNO, "Error", "Unable to download file " + Path.GetFileName(CurrenctXMLFile) + " for ref " + VRNO + ".", BuyerCode, SupplierCode, AuditPath);
                                        CreateAuditFile(eFile, "DNVGL_HTTP_PO", VRNO, "Error", "LeS-1004:Unable to process file " + Path.GetFileName(CurrenctXMLFile) + " for " + VRNO, BuyerCode, SupplierCode, AuditPath);
                                        if (!PrintScreen(eFile)) eFile = "";
                                        if (File.Exists(Path.GetDirectoryName(MailTextFile) + "\\Error\\" + Path.GetFileName(MailTextFile))) File.Delete(Path.GetDirectoryName(MailTextFile) + "\\Error\\" + Path.GetFileName(MailTextFile));
                                        File.Move(MailTextFile, Path.GetDirectoryName(MailTextFile) + "\\Error\\" + Path.GetFileName(MailTextFile));
                                        LogText = "File moved to Error folder.";
                                    }
                                }
                            }
                            else
                            {
                                //WriteErrorLog_With_Screenshot("Unable to get PO address details");
                                WriteErrorLog_With_Screenshot("LeS-1040:Unable to get details - PO address Field(s) not present");
                                File.Move(MailTextFile, Path.GetDirectoryName(MailTextFile) + "\\Error\\" + Path.GetFileName(MailTextFile));
                            }
                        }
                        else
                        {
                            //WriteErrorLog_With_Screenshot("Unable to get PO item details");
                            WriteErrorLog_With_Screenshot("LeS-1040:Unable to get details - PO item Field(s) not present");
                            File.Move(MailTextFile, Path.GetDirectoryName(MailTextFile) + "\\Error\\" + Path.GetFileName(MailTextFile));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteErrorLog_With_Screenshot("Exception while processing PO : " + ex.GetBaseException().Message.ToString());
                WriteErrorLog_With_Screenshot("LeS-1004:Unable to process file due to " + ex.GetBaseException().Message.ToString());
            }
        }

        public bool GetPOItems(List<OrderItemGroup> ItemGroups, ref LeSXML.LeSXML _lesXml)
        {
            int LinItemCount = 0;
            bool result = false;
            try
            {
                _lesXml.LineItems.Clear();
                LogText = "Start Getting PO LineItem details.";
                foreach (OrderItemGroup Items in ItemGroups)
                {
                    foreach (OrderItem item in Items.orderItems)
                    {
                        LeSXML.LineItem _item = new LeSXML.LineItem();
                        try
                        {
                            _item.Number = Convert.ToString(LinItemCount + 1);
                            _item.OrigItemNumber = Convert.ToString(LinItemCount + 1);

                            _item.Name = item.itemName;

                            _item.ItemRef = item.itemPartNumber;
                            _item.OriginatingSystemRef = item.itemOrderNumber;
                            _item.Quantity = item.quantity.ToString("0.00");
                            _item.Unit = item.unit;
                            _item.ListPrice = item.priceUnit.ToString("0.00");
                            _item.Discount = convert.ToString(item.discount);
                            if (item.deliveryUntil != null)
                            {
                                DateTime dt = FormatMTMLDate(item.deliveryUntil.ToString("yyyyMMddhhmmss"));
                                _item.Remark = "Delivery Date: " + dt.ToShortDateString(); //item.deliveryUntil;
                                if (item.externalRemark != "")
                                    _item.Remark += " Description: " + item.externalRemark;
                            }

                            _item.Equipment = Items.name;
                            _item.EquipMaker = Items.makersName;
                            if(Items.serialNumber!="")
                            _item.EquipRemarks = "Serial No: " + Items.serialNumber;
                            _lesXml.LineItems.Add(_item);
                            LinItemCount++;
                        }
                        catch (Exception ex)
                        { LogText = ex.GetBaseException().ToString(); }
                    }
                }
                _lesXml.Total_LineItems = Convert.ToString(LinItemCount);
                LogText = "Getting LineItem details successfully";
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                LogText = "Exception while getting PO Items: " + ex.GetBaseException().ToString();
                result = false;
                return result;
            }
        }

        public bool GetPOAddress(ref LeSXML.LeSXML _lesXml, Dictionary<string, string> AddDetails)
        {
            bool result = false;
            try
            {
                _lesXml.Addresses.Clear();
                LogText = "Start Getting PO Address Details.";
                LeSXML.Address _xmlAdd = new LeSXML.Address();

                _xmlAdd.Qualifier = "BY";
                _xmlAdd.AddressName = dctAppSettings["BuyerName"].Trim();
                _xmlAdd.ContactPerson = AddDetails["BContact"];
                _xmlAdd.Phone = AddDetails["BPhone"];
                _xmlAdd.eMail = AddDetails["BEmail"];
                _xmlAdd.Address1 = AddDetails["BAddress"];
                _lesXml.Addresses.Add(_xmlAdd);

                _xmlAdd = new LeSXML.Address();
                _xmlAdd.Qualifier = "VN";
                _xmlAdd.AddressName = AddDetails["SAddressName"];
                _xmlAdd.ContactPerson = AddDetails["SContact"];
                _xmlAdd.Address1 = AddDetails["sAddress"];
                _xmlAdd.eMail = AddDetails["sEmail"];
                _lesXml.Addresses.Add(_xmlAdd);

                _xmlAdd = new LeSXML.Address();
                _xmlAdd.Qualifier = "CN";
                if (AddDetails.ContainsKey("vAddressName"))
                    _xmlAdd.AddressName = AddDetails["vAddressName"];
                if (AddDetails.ContainsKey("vContact"))
                    _xmlAdd.ContactPerson = AddDetails["vContact"];
                if (AddDetails.ContainsKey("vAddress"))
                    _xmlAdd.Address1 = AddDetails["vAddress"];
                if (AddDetails.ContainsKey("vEmail"))
                    _xmlAdd.eMail = AddDetails["vEmail"];
                _lesXml.Addresses.Add(_xmlAdd);

                LogText = "Getting address details successfully";
                result = true; return result;
            }
            catch (Exception ex)
            {
                LogText = "Exception while getting address details: " + ex.GetBaseException().ToString(); result = false; return result;
            }
        }

        public override bool DownloadRFQ(string RequestURL, string DownloadFileName, string ContentType = "Application/pdf")
        {
            bool _result = false;
            try
            {
                URL = RequestURL;
                if (LoadURL("", "", "", false))
                {
                    byte[] b = null;
                    FileStream fileStream = File.OpenWrite(DownloadFileName);
                    byte[] buffer = new byte[1024];
                    using (Stream input = _httpWrapper._CurrentResponse.GetResponseStream())
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
                _result = (File.Exists(DownloadFileName));
            }
            catch (Exception e)
            {
                throw e;
            }
            return _result;
        }
        #endregion

        #region Quotes

        public void ProcessQuote()
        {
            try {
                GetXmlFiles();
                if (Quote_mtmlFiles.Count > 0)
                {
                    LogText = "";
                    LogText = "Quote processing started.";
                    LogText = Quote_mtmlFiles.Count + " files found to process.";
                    for (int j = 0; j < Quote_mtmlFiles.Count; j++)
                    {
                        LogText = "Started Loading interchange object.";
                        MTMLClass _mtml = new MTMLClass();
                        _interchange = _mtml.Load(Quote_mtmlFiles[j]);
                        LoadInterchangeDetails();
                        LogText = "stopped Loading interchange object.";
                        ProcessQuoteMTML(Quote_mtmlFiles[j]);
                      //  ClearCommonVariables();
                    }
                }
                else LogText = "No quote files found to process.";
            }
            catch (Exception ex)
            {
                string eFile = PrintScreenPath + "\\DNVGL_" + this.docType + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + BuyerCode + "_" + SupplierCode + ".png";
                if (!PrintScreen(eFile)) eFile = "";
                LogText = "Exception while processing Quote : " + ex.GetBaseException().Message.ToString();
            }
        }

        public void GetXmlFiles()
        {
            Quote_mtmlFiles.Clear();
            POC_mtmlFiles.Clear();
            DirectoryInfo _dir = new DirectoryInfo(MTMLUploadPath);
            FileInfo[] _Files = _dir.GetFiles();
            foreach (FileInfo _MtmlFile in _Files)
            {
                MTMLClass _mtml = new MTMLClass();
                _interchange = _mtml.Load(_MtmlFile.FullName);
                LoadInterchangeDetails();
                if (docType.ToUpper() == "QUOTE")
                    Quote_mtmlFiles.Add(_MtmlFile.FullName);
                else if (docType.ToUpper() == "ORDERRESPONSE")
                    POC_mtmlFiles.Add(_MtmlFile.FullName);
                else
                {
                    //MoveFileToError(_MtmlFile.FullName, "Invalid doctype : " + docType + " for ref " + UCRefNo, "Error");
                    MoveFileToError(_MtmlFile.FullName, "LeS-1004.1:Unable to process file - invalid doc type " + docType + " for " + UCRefNo, "Error");
                    throw new Exception("Invalid doctype : " + docType + " for ref " + UCRefNo);
                }
            }
        }

        public void LoadInterchangeDetails()
        {
            try
            {
                //LogText = "Started Loading interchange object.";
                if (_interchange != null)
                {
                    if (_interchange.Recipient != null)
                        BuyerCode = _interchange.Recipient;

                    if (_interchange.Sender != null)
                        SupplierCode = _interchange.Sender;

                    if (_interchange.DocumentHeader.DocType != null)
                        docType = _interchange.DocumentHeader.DocType;

                    if (_interchange.DocumentHeader != null)
                    {
                        if (_interchange.DocumentHeader.IsDeclined)
                            IsDecline = _interchange.DocumentHeader.IsDeclined;

                        if (_interchange.DocumentHeader.MessageNumber != null)
                            MessageNumber = _interchange.DocumentHeader.MessageNumber;

                        if (_interchange.DocumentHeader.LeadTimeDays != null)
                            LeadDays = _interchange.DocumentHeader.LeadTimeDays;

                        Currency = _interchange.DocumentHeader.CurrencyCode;

                        MsgNumber = _interchange.DocumentHeader.MessageNumber;
                        MsgRefNumber = _interchange.DocumentHeader.MessageReferenceNumber;

                        if (_interchange.DocumentHeader.IsAltItemAllowed != null) IsAltItemAllowed = Convert.ToInt32(_interchange.DocumentHeader.IsAltItemAllowed);
                        if (_interchange.DocumentHeader.IsPriceAveraged != null) IsPriceAveraged = Convert.ToInt32(_interchange.DocumentHeader.IsPriceAveraged);
                        if (_interchange.DocumentHeader.IsUOMChanged != null) IsUOMChanged = Convert.ToInt32(_interchange.DocumentHeader.IsUOMChanged);
                        if (_interchange.DocumentHeader.AdditionalDiscount != null) AdditionalDiscount = Convert.ToDouble(_interchange.DocumentHeader.AdditionalDiscount);


                        for (int i = 0; i < _interchange.DocumentHeader.References.Count; i++)
                        {
                            if (_interchange.DocumentHeader.References[i].Qualifier == ReferenceQualifier.UC)
                                UCRefNo = _interchange.DocumentHeader.References[i].ReferenceNumber.Trim();
                            else if (_interchange.DocumentHeader.References[i].Qualifier == ReferenceQualifier.AAG)
                                AAGRefNo = _interchange.DocumentHeader.References[i].ReferenceNumber.Trim();
                        }
                    }
                    if (_interchange.BuyerSuppInfo != null)
                    {
                        LesRecordID = Convert.ToString(_interchange.BuyerSuppInfo.RecordID);
                    }

                    #region read interchange party addresses

                    for (int j = 0; j < _interchange.DocumentHeader.PartyAddresses.Count; j++)
                    {
                        if (_interchange.DocumentHeader.PartyAddresses[j].Qualifier == PartyQualifier.BY)
                        {
                            BuyerName = _interchange.DocumentHeader.PartyAddresses[j].Name;
                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts != null)
                            {
                                if (_interchange.DocumentHeader.PartyAddresses[j].Contacts.Count > 0)
                                {
                                    if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count > 0)
                                    {
                                        for (int k = 0; k < _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count; k++)
                                        {
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.TE)
                                                BuyerPhone = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.EM)
                                                BuyerEmail = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.FX)
                                                BuyerFax = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                        }
                                    }
                                }
                            }
                        }

                        else if (_interchange.DocumentHeader.PartyAddresses[j].Qualifier == PartyQualifier.VN)
                        {
                            SupplierName = _interchange.DocumentHeader.PartyAddresses[j].Name;
                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts != null)
                            {
                                if (_interchange.DocumentHeader.PartyAddresses[j].Contacts.Count > 0)
                                {
                                    if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count > 0)
                                    {
                                        for (int k = 0; k < _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count; k++)
                                        {
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.TE)
                                                SupplierPhone = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.EM)
                                                SupplierEmail = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.FX)
                                                SupplierFax = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                        }
                                    }
                                }
                            }
                        }

                        else if (_interchange.DocumentHeader.PartyAddresses[j].Qualifier == PartyQualifier.UD)
                        {
                            VesselName = _interchange.DocumentHeader.PartyAddresses[j].Name;
                            if (_interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Berth != "")
                                PortName = _interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Berth;

                            if (_interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Port != null)
                                PortCode = _interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Port;
                        }
                    }

                    #endregion

                    #region read comments

                    if (_interchange.DocumentHeader.Comments != null)
                    {
                        for (int i = 0; i < _interchange.DocumentHeader.Comments.Count; i++)
                        {
                            if (_interchange.DocumentHeader.Comments[i].Qualifier == CommentTypes.SUR)
                                SupplierComment = _interchange.DocumentHeader.Comments[i].Value;
                            else if (_interchange.DocumentHeader.Comments[i].Qualifier == CommentTypes.ZTP)
                                PayTerms = _interchange.DocumentHeader.Comments[i].Value;
                        }
                    }

                    #endregion

                    #region read Line Items

                    if (_interchange.DocumentHeader.LineItemCount > 0)
                    {
                        _lineitem = _interchange.DocumentHeader.LineItems;
                    }

                    #endregion

                    #region read Interchange Monetory Amount

                    if (_interchange.DocumentHeader.MonetoryAmounts != null)
                    {
                        for (int i = 0; i < _interchange.DocumentHeader.MonetoryAmounts.Count; i++)
                        {
                            if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.PackingCost_106)
                                PackingCost = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.FreightCharge_64)
                                FreightCharge = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.TotalLineItemsAmount_79)
                                TotalLineItemsAmount = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.AllowanceAmount_204)
                                Allowance = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.GrandTotal_259)
                                GrandTotal = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.BuyerItemTotal_90)//16-12-2017
                                BuyerTotal = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.Deposit_97)//16-2-2018
                                DepositCost = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.TaxCost_99)
                                FreightCharge += _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.OtherCost_98)
                                OtherCost = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();//
                        }
                    }

                    #endregion

                    #region read date time period

                    if (_interchange.DocumentHeader.DateTimePeriods != null)
                    {
                        for (int i = 0; i < _interchange.DocumentHeader.DateTimePeriods.Count; i++)
                        {
                            if (_interchange.DocumentHeader.DateTimePeriods[i].Qualifier == DateTimePeroidQualifiers.DocumentDate_137)
                            {
                                if (_interchange.DocumentHeader.DateTimePeriods[i].Value != null)
                                {
                                    DateTime dtDocDate = FormatMTMLDate(_interchange.DocumentHeader.DateTimePeriods[i].Value);
                                    if (dtDocDate != DateTime.MinValue)
                                    {
                                        DocDate = dtDocDate.ToString("MM/dd/yyyy");
                                    }
                                }
                            }

                            else if (_interchange.DocumentHeader.DateTimePeriods[i].Qualifier == DateTimePeroidQualifiers.DeliveryDate_69)
                            {
                                if (_interchange.DocumentHeader.DateTimePeriods[i].Value != null)
                                {
                                    DateTime dtDelDate = FormatMTMLDate(_interchange.DocumentHeader.DateTimePeriods[i].Value);
                                    if (dtDelDate != DateTime.MinValue)
                                    {
                                        DelvDate = dtDelDate.ToString("MM/dd/yyyy");
                                    }
                                    if (dtDelDate == null)
                                    {
                                        DateTime dt = FormatMTMLDate(DateTime.Now.AddDays(Convert.ToDouble(LeadDays)).ToString());
                                        if (dt != DateTime.MinValue)
                                        {
                                            DelvDate = dt.ToString("MM/dd/yyyy");
                                        }
                                    }
                                }
                            }

                            if (_interchange.DocumentHeader.DateTimePeriods[i].Qualifier == DateTimePeroidQualifiers.ExpiryDate_36)
                            {
                                if (_interchange.DocumentHeader.DateTimePeriods[i].Value != null)
                                {
                                    DateTime expDate = FormatMTMLDate(_interchange.DocumentHeader.DateTimePeriods[i].Value);
                                    if (expDate != DateTime.MinValue)
                                    {
                                        ExpDate = expDate.ToString("dd-MM-yyyy");
                                    }
                                }
                            }
                        }
                    }

                    #endregion
                    //  LogText = "stopped Loading interchange object.";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception on LoadInterchangeDetails : " + ex.GetBaseException().ToString());
            }
        }

        public void ProcessQuoteMTML(string MTML_QuoteFile)
        {
            string URL = "", Token = ""; bool isCatch = false;
            try
            {              
                URL = MessageNumber.Split('|')[0];
                Token = MessageNumber.Split('|')[1];
                if (URL != "" && URL.Trim().Contains("https://supplierportal.dnvgl.com/") && Token.Trim() != "")
                {
                    if (!_httpWrapper._AddRequestHeaders.ContainsKey("Origin"))
                        _httpWrapper._AddRequestHeaders.Add("Origin", @"https://supplierportal.dnvgl.com");
                    _httpWrapper.ContentType = "application/json";
                    _httpWrapper.Referrer = "https://supplierportal.dnvgl.com/";
                  
                    dctPostDataValues.Clear();
                    dctPostDataValues.Add("Password", Token);
                    dctPostDataValues.Add("Type", "1");
                    string[] ArrURL = URL.Trim().Split('/');
                    URL = ArrURL[0] + "//" + ArrURL[2] + "/api/Security/PostLogin/" + ArrURL[4];

                    this.URL = MessageNumber.Split('|')[0]; LoadURL("", "", "");
                    string _postdata = GetPostData(true);
                   
                    bool isLoggedin = _httpWrapper.PostURL(URL, _postdata, "", "", "");
                    if (isLoggedin && _httpWrapper._CurrentResponseString.Contains("true"))
                    {
                        this.URL = URL.Trim().Replace("Security/PostLogin", "Requests/getrequest");
                        _httpWrapper.AcceptMimeType = "*/*";
                        try
                        {
                            LoadURL("", "", "");
                        }
                        catch (Exception ex)
                        {
                            isCatch = true;
                            //MoveFileToError(MTML_QuoteFile, "Exception while loading Quote page : " + ex.GetBaseException().Message.ToString(), docType);
                            MoveFileToError(MTML_QuoteFile, "LeS-1016:Unable to load url due to " + ex.GetBaseException().Message.ToString(), docType);
                         }
                        if(!isCatch)
                        {
                            
                           Fill_Quotation(Token,MTML_QuoteFile);

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogText = "Exception while processing Quote MTML : " + ex.GetBaseException().Message.ToString();
            }
        }

        public void Fill_Quotation(string Token, string MTML_QuoteFile)
        {
            try
            {
                string[] Arr = this.URL.Split('/');
                if (Arr.Length > 0)
                {
                    var _library = (RootObject)JsonConvert.DeserializeObject(_httpWrapper._CurrentResponseString, typeof(RootObject));
                    if (_library != null)
                    {
                        string strValidMsg = ValidateQuote();
                        //if (string.IsNullOrEmpty(strValidMsg))
                        {
                            //  _library.sentDateTime = null;_library.communicationStatus =1; //testing
                            if (_library.sentDateTime == null && _library.communicationStatus == 1)
                            {
                                if (isSaveQuote)
                                    SaveQuotation(Arr, _library, MTML_QuoteFile, Token);
                            }
                            else
                            {
                                //MoveFileToError(MTML_QuoteFile, "Save and/or submit quote button is null for Ref  " + UCRefNo,this.docType);
                                LogText = "Save and/or submit quote button is null for Ref  " + UCRefNo;
                                MoveFileToError(MTML_QuoteFile, "LeS-1008.3:Unable to Save Quote due to missing controls for " + UCRefNo, this.docType);
                            }
                        }
                        //else
                        //{
                        //    MoveFileToError(MTML_QuoteFile, "LeS-1007:Unable to Save file for " + this.docType + "-" + strValidMsg + " Field(s) not present.", this.docType);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                //MoveFileToError(MTML_QuoteFile, "Exception while filling Quote MTML : " + ex.GetBaseException().ToString(), this.docType);
                MoveFileToError(MTML_QuoteFile, "LeS-1004:Unable to process file due to " + ex.GetBaseException().ToString(), this.docType);
            }
        }

        private string ValidateQuote()
        {
            string strResult = "";
            if (string.IsNullOrEmpty(Currency))
            {
                LogText = "Unable to set currency for " + UCRefNo;
                strResult = "Currency ,";
            }
             if (string.IsNullOrEmpty(AAGRefNo))
             {
                 LogText = "Unable to set supplier ref for " + UCRefNo;
                 strResult += "Supplier Ref ,";
             }
             if (string.IsNullOrEmpty(SupplierComment))
             {
                 LogText = "Unable to set supplier remarks for " + UCRefNo;
                 strResult += "Supplier Remarks ,";
             }
             if (string.IsNullOrEmpty(ExpDate))
             {
                 LogText = "Unable to set validity offer for " + UCRefNo;
                 strResult += "Validity Offer ,";
             }          
            strResult=strResult.TrimEnd(',');          
            return strResult;
        }
     
        public List<string> GetUnitList()
        {
            List<string> slUnits = new List<string>();
            string sDoneFile = AppDomain.CurrentDomain.BaseDirectory + "UOM_List.txt";

            if (File.Exists(sDoneFile))
            {
                string[] _Items = File.ReadAllLines(sDoneFile);
                slUnits.AddRange(_Items.ToList());
            }
            return slUnits;
        }

        public List<string> GetLongUnitList()
        {
            List<string> slUnits = new List<string>();
            string sDoneFile = AppDomain.CurrentDomain.BaseDirectory + "Long_UOM_List.txt";

            if (File.Exists(sDoneFile))
            {
                string[] _Items = File.ReadAllLines(sDoneFile);
                slUnits.AddRange(_Items.ToList());
            }
            return slUnits;
        }

        public void SaveQuotation(string[] Arr, RootObject _library, string MTML_QuoteFile, string Token)
        {
            double itemTotal = 0.0;
            string amount = "", msg = "", etaDate = "", delDate = "", QuoteDeadline = "", createdAt = "", modifiedDate = "";
            bool isGrand = false, result = true;
            try
            {
                this.URL = Arr[0] + "//" + Arr[2] + "/" + Arr[3] + "/" + Arr[4] + "/putrequest/" + _library.id;
                _httpWrapper.Referrer = "https://supplierportal.dnvgl.com/";
                _httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.CacheControl);
                _httpWrapper._SetRequestHeaders[HttpRequestHeader.AcceptLanguage] = "en-us";
                _httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");
                _httpWrapper.RequestMethod = "PUT";

                List<string> lstUnits = GetUnitList();
                List<string> lstLongUnits = GetLongUnitList();

                #region add to put dictionary
                if (_library.etaDate != null)
                    etaDate = _library.etaDate.ToString("yyyy-MM-ddTHH:mm:ss");
                if (_library.deliveryDate != null)
                    delDate = _library.deliveryDate.ToString("yyyy-MM-ddTHH:mm:ss");
                if (_library.quotationDeadline != null)
                    QuoteDeadline = _library.quotationDeadline.ToString("yyyy-MM-ddTHH:mm:ss");

                _library.additionalCharges = Convert.ToDouble(FreightCharge) + Convert.ToDouble(PackingCost) + Convert.ToDouble(DepositCost) + Convert.ToDouble(OtherCost);//added by kalpita on 09/09/2019
                //  string postData = "";
                string postData = "{\"id\":" + _library.id + ",\"exportTimeToSm\":\"" + _library.exportTimeToSm.ToString("yyyy-MM-ddTHH:mm:ss") + "\",\"creatorName\":\"" + _library.creatorName +
                 "\",\"creatorPhone\":\"" + _library.creatorPhone + "\",\"creatorEmail\":\"" + _library.creatorEmail + "\",\"dsno\":" + _library.dsno + ",\"requestCode\":\"" + _library.requestCode +
                 "\",\"name\":\"" + _library.name + "\",\"remark\":\"" + _library.remark + "\",\"shipName\":\"" + _library.shipName + "\",\"etaPort\":\"" + _library.etaPort +
                 "\",\"etaDate\":\"" + etaDate + "\",\"deliveryDate\":\"" + delDate + "\",\"quotationDeadline\":\"" + QuoteDeadline +
                 "\",\"currencyShortName\":\"" + Currency + "\",\"discount\":" + AdditionalDiscount + ",\"additionalCharges\":" + _library.additionalCharges;

                foreach (LineItem item in _lineitem)
                {
                    foreach (ItemGroup Items in _library.itemGroups)
                    {
                        foreach (Item lItem in Items.items)
                        {
                            if (lItem.itemOrderNumber == item.OriginatingSystemRef)
                            { itemTotal += item.MonetaryAmount; break; }
                        }
                    }
                }
                itemTotal += _library.additionalCharges;

                if (Convert.ToInt32(itemTotal) == Convert.ToInt32(convert.ToDouble(GrandTotal)))
                { amount = convert.ToString(GrandTotal); isGrand = true; }
                else if (BuyerTotal != "")
                {
                    if (Convert.ToInt32(itemTotal) == Convert.ToInt32(convert.ToDouble(BuyerTotal)))
                    { amount = convert.ToString(BuyerTotal); isGrand = false; }
                }
                if (amount != "")
                {
                    if (DelvDate != "")
                        SupplierComment += " Delivery Date: " + DelvDate;
                    if (PackingCost != null) SupplierComment += " Packing Cost: " + PackingCost;
                    if (FreightCharge != null) SupplierComment += " Freight Cost: " + FreightCharge;
                    if (DateTime.Now != null) DocDate = convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
                    if (_library.createdAt != null) createdAt = convert.ToDateTime(_library.createdAt).ToString("yyyy-MM-ddTHH:mm:ss.ff");
                    if (_library.modifiedDateTime != null) modifiedDate = convert.ToDateTime(_library.modifiedDateTime).ToString("yyyy-MM-ddTHH:mm:ss.fff");

                    postData += ",\"amount\":" + amount + ",\"delAddName\":\"" + _library.delAddName + "\",\"delAddName2\":\"" + _library.delAddName2 + "\",\"delAddStreet\":\"" + _library.delAddStreet +
                                      "\",\"delAddZipCode\":\"" + _library.delAddZipCode + "\",\"delAddTown\":\"" + _library.delAddTown + "\",\"supplierAddrName\":\"" + _library.supplierAddrName +
                                      "\",\"supplierContSalutation\":\"" + _library.supplierContSalutation + "\",\"supplierContFirstName\":\"" + _library.supplierContFirstName +
                                      "\",\"supplierContName\":\"" + _library.supplierContName + "\",\"supplierAddrStreet\":\"" + _library.supplierAddrStreet + "\",\"supplierAddrZIP\":\"" + _library.supplierAddrZIP +
                                      "\",\"supplierAddrTown\":\"" + _library.supplierAddrTown + "\",\"supplierAddrCountry\":\"" + _library.supplierAddrCountry + "\",\"supplierContEmail\":\"" + _library.supplierContEmail +
                                      "\",\"supplierReference\":\"" + AAGRefNo + "\",\"quotationDate\":\"" + DocDate + "\",\"supplierRemark\":\"" + SupplierComment.Replace("\n", "").Replace("\r\n", "") +
                                      "\",\"paymentTerms\":\"" + PayTerms.Replace(":", "-") + "\",\"validity\":\"" + ExpDate + "\",";
                    if (Convert.ToInt32(LeadDays) > 0)
                        postData += "\"deliveryTime\":\"" + LeadDays;
                    else
                        postData += "\"deliveryTime\":\"";
                    postData += "\",\"supplierCountryOrigin\":\"" + _library.supplierCountryOrigin + "\",\"supplierPrincipals\":\"" + _library.supplierPrincipals + "\",\"supplierPartQuoted\":\"" + _library.supplierPartQuoted +
                     "\",\"supplierDeliveryTerms\":\"" + _library.supplierDeliveryTerms + "\",\"shippingCompanyID\":\"" + _library.shippingCompanyID + "\",\"shippingCompName\":\"" + _library.shippingCompName +
                     "\",\"shippingCompStreet\":\"" + _library.shippingCompStreet + "\",\"shippingCompZip\":\"" + _library.shippingCompZip + "\",\"shippingCompTown\":\"" + _library.shippingCompTown +
                     "\",\"shippingCompPhone\":\"" + _library.shippingCompPhone + "\",\"shippingCompCountry\":\"" + _library.shippingCompCountry + "\",\"history\":null" +
                     ",\"urgent\":\"" + _library.urgent + "\",\"createdAt\":\"" + createdAt + "\",";

                    int j = 0;

                    string totalItemData = "";
                    foreach (ItemGroup Items in _library.itemGroups)
                    {
                        string itemData = "";

                        itemData = "\"itemGroups\":[{\"id\":" + Items.id + ",\"dsno\":\"" + Items.dsno + "\",\"name\":\"" + Items.name + "\",\"name2\":\"" + Items.name2 + "\",\"makersName\":\"" + Items.makersName +
                            "\",\"serialNumber\":\"" + Items.serialNumber + "\",";
                        foreach (Item item in Items.items)
                        {
                            j++;
                            LineItem _item = null;
                            foreach (LineItem mitem in _lineitem)
                            {
                                if (item.itemOrderNumber == mitem.OriginatingSystemRef)
                                {
                                    _item = mitem;
                                }
                            }
                            if (_item != null)
                            {

                                if (lstUnits != null && lstUnits.Count > 0 && lstLongUnits != null && lstLongUnits.Count > 0)
                                {
                                    string _price = "", _discount = "";
                                    foreach (PriceDetails _priceDetails in _item.PriceList)
                                    {
                                        if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.GRP) _price = _priceDetails.Value.ToString("0.00");
                                        else if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.DPR) _discount = _priceDetails.Value.ToString("0.00");
                                    }
                                    string remarks = _item.LineItemComment.Value.Replace('|', ',').Replace('(', ' ').Replace(')', ' ').Replace('\"', ' ').Replace('&', ' ').Replace("%", "");

                                    if (j == 1)
                                        itemData += "\"items\":[";
                                    itemData += "{\"id\":" + item.id + ",\"dsno\":" + item.dsno + ",\"itemOrderNumber\":\"" + item.itemOrderNumber + "\",\"name\":\"" + item.name + "\",\"name2\":\"" + item.name2 + "\",\"stockQuantity\":" +
                                   item.stockQuantity + ",\"stockUnit\":\"" + item.stockUnit + "\",\"unitConvFactor\":" + item.unitConvFactor + ",\"quantity\":" + _item.Quantity + ",\"purchaseUnit\":\"" + _item.MeasureUnitQualifier + "\",\"price\":" +
                                _price + ",\"discount\":" + _discount + ",\"partNumber\":\"" + item.partNumber + "\",\"drawingNumber\":\"" + item.drawingNumber + "\",\"companyNumber\":\"" + item.companyNumber + "\",\"remark\":\"" +
                                  remarks + "\",\"itemText\":\"" + item.itemText + "\",\"externalRemark\":\"" + item.externalRemark + "\",";
                                    if (_item.DeleiveryTime == null)
                                        itemData += "\"deliveryDays\":0";
                                    else
                                        itemData += "\"deliveryDays\":" + _item.DeleiveryTime;
                                    itemData += ",\"supplierOrderCode\":\"" + item.supplierOrderCode + "\",\"itemRemark\":\"" +
                                   item.itemRemark + "\",\"exContrClassNumber\":\"" + item.exContrClassNumber + "\",\"exContrMilitCode\":\"" + item.exContrMilitCode + "\",\"exContrCofoCode\":\"" + item.exContrCofoCode + "\",\"exContrPercUsContent\":\"" +
                                   item.exContrPercUsContent + "\",\"exContrCustCode\":\"" + item.exContrCustCode + "\",\"itemTotal\":" + _item.MonetaryAmount + ",\"xmlUpdated\":" + convert.ToString(item.xmlUpdated).ToLower() + ",\"changedFields\":null,\"recStatus\":0" +
                                  ",\"visibleFields\":\"{\\\"ExContrClassNumber\\\":false,\\\"ExContrMilitCode\\\":false,\\\"ExContrCofoCode\\\":false,\\\"ExContrPercUsContent\\\":false,\\\"ExContrCustCode\\\":false}\"},";

                                    if (j == Items.items.Count)
                                    {
                                        totalItemData = itemData.TrimEnd(',') + "],"; j = 0;
                                        itemData = totalItemData + "\"xmlUpdated\":" + Convert.ToString(Items.xmlUpdated).ToLower() + ",\"changedFields\":null,\"recStatus\":0},";
                                        totalItemData = "";
                                    }
                                }
                                else
                                {
                                    LogText = "Unit '" + _item.MeasureUnitQualifier + "' of Item '" + _item.Description + "' is not found in UOM List.";
                                    msg = "LeS-1007:Unable to Save file - UOM Field(s) not present";
                                    result = false;
                                    break;
                                }
                            }
                            else
                            {
                                LogText = "Item not found on site or in mtml with Part No " + item.partNumber + " for refNo " + UCRefNo;
                                msg = "LeS-1007:Unable to Save file - Item Field(s) not present";
                                result = false;
                                break;
                            }
                        }
                        totalItemData = totalItemData + itemData;

                    }
                    //     postData = postData + totalItemData;
                    postData = postData + totalItemData.TrimEnd(',') + "]";


                    postData += ",\"password\":\"" + Token + "\",\"sourceXML\":\"";
                    postData += _library.sourceXML.Replace("\r\n", "\\r\\n").Replace("\"", "\\\"") + "\",\"modifiedDateTime\":\"" + modifiedDate + "\",\"sentDateTime\":null,\"communicationStatus\":" + _library.communicationStatus + ",\"changedFields\":null,\"recStatus\":3,\"isNewDB\":" +//" + _library.recStatus + "
                        Convert.ToString(_library.isNewDB).ToLower() + ",\"mailReciepientSupplier\":\"" + _library.mailReciepientSupplier + "\",\"mailReciepientBuyer\":\"" + _library.mailReciepientBuyer + "\",\"requestStatusSM\":\"" + _library.requestStatusSM +
                        "\",\"reminderSent\":" + Convert.ToString(_library.reminderSent).ToLower() + ",\"userIP\":\"" + _library.userIP + "\",\"visibleFields\":\"{\\\"DeliveryTime\\\":false,\\\"Validity\\\":false,\\\"SupplierRemark\\\":true,\\\"SupplierDeliveryTerms\\\":false}\"}";

                    if (!result) { MoveFileToError(MTML_QuoteFile, msg, "Quote"); result = false; }
                }
                else
                {
                    postData = "";
                    // if (isGrand)
                    //    MoveFileToError(MTML_QuoteFile, "Unable to upload Quote for Ref : '" + UCRefNo + "' due to Total Amount '" + GrandTotal + "' mismatch with item total '" + itemTotal + "'.", this.docType);
                    //else MoveFileToError(MTML_QuoteFile, "Unable to upload Quote for Ref : '" + UCRefNo + "' due to Total Amount '" + BuyerTotal + "' mismatch with item total '" + itemTotal + "'.", this.docType);

                    string total = (isGrand) ? GrandTotal : BuyerTotal;
                    LogText = "Unable to upload Quote for Ref : '" + UCRefNo + "' due to Total Amount '" + total + "' mismatch with item total '" + itemTotal;
                    MoveFileToError(MTML_QuoteFile, "LeS-1008.1:Unable to Save Quote due to amount mismatch for " + UCRefNo, this.docType);
                }

                #endregion

                if (_httpWrapper.PostURL(URL, postData, "", "", ""))
                {
                    if (dctAppSettings["SendQuote"].Trim().ToUpper() != "TRUE")
                    {
                        //MoveFileToBackup(MTML_QuoteFile, "successfully saved input to database."); [testing]
                    }

                    if (isSendMail && !IsDecline)//IsDecline condition added on 08-03-17
                        SendMailNotification(_interchange, "QUOTE", UCRefNo, "SUBMITTED", "Quote '" + UCRefNo + "' submitted successfully.");
                    else if (isSendMail && IsDecline)//IsDecline condition added on 08-03-17
                        SendMailNotification(_interchange, "QUOTE", UCRefNo, "DECLINED", "Quote '" + UCRefNo + "' declined.");

                    if (dctAppSettings["SendQuote"].Trim().ToUpper() == "TRUE")
                    {
                        SendQuote(MTML_QuoteFile, postData, URL);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SendQuote(string MTML_QuoteFile, string postData, string _url)
        {
            bool res = false;
            URL = MessageNumber.Split('|')[0];
            string[] ArrURL = URL.Trim().Split('/');
            URL = ArrURL[0] + "//" + ArrURL[2] + "/api/Security/PostLogin/" + ArrURL[4];
            this.URL = URL.Trim().Replace("Security/PostLogin", "Requests/getrequest");
            dctPostDataValues.Clear();
            dctPostDataValues.Add("Password", MessageNumber.Split('|')[1]);
            dctPostDataValues.Add("Type", "1");
            _httpWrapper.AcceptMimeType = "*/*";
            LoadURL("", "", "");

            var _library = (RootObject)JsonConvert.DeserializeObject(_httpWrapper._CurrentResponseString, typeof(RootObject));
            if (_library != null)
            {
                if (_library.communicationStatus == 1 || _library.communicationStatus == 2)//changed by kalpita on 09/09/2019
                {
                    if (_library.amount == Convert.ToDouble(GrandTotal)) { res = true; }
                    else if (BuyerTotal != "")
                    {
                        if (_library.amount == Convert.ToDouble(BuyerTotal))
                        {
                            res = true;
                        }
                        else
                        {
                            int _diff = 0;
                            if (_library.amount > Convert.ToDouble(BuyerTotal))
                            {
                                _diff = Convert.ToInt32(_library.amount) - Convert.ToInt32(Convert.ToDouble(BuyerTotal));
                            }
                            else if (Convert.ToDouble(BuyerTotal) > _library.amount)
                            {
                                _diff = Convert.ToInt32(BuyerTotal) - Convert.ToInt32(_library.amount);
                            }
                            if (_diff <= 1)
                            {
                                res = true;
                            }
                            else
                            {
                                string Total = (BuyerTotal != "") ? BuyerTotal : GrandTotal;
                                //if (BuyerTotal != "")
                                //    MoveFileToError(MTML_QuoteFile, "Unable to send Quote for Ref : '" + UCRefNo + "' due to Buyer Total Amount '" + BuyerTotal + "' mismatch on site Grand Total '" + _library.amount + "'.", this.docType);
                                //else
                                //    MoveFileToError(MTML_QuoteFile, "Unable to send Quote for Ref : '" + UCRefNo + "' due to Total Amount '" + GrandTotal + "' mismatch on site Grand Total '" + _library.amount + "'.", this.docType);
                                LogText = "Unable to send Quote for Ref : " + UCRefNo + " due to Total Amount " + Total + " mismatch on site Grand Total " + _library.amount;
                                MoveFileToError(MTML_QuoteFile, "LeS-1025.1:Unable to send file due to amount mismatch for " + UCRefNo, this.docType);
                            }
                        }
                    }
                }
                else
                {
                    //MoveFileToError(MTML_QuoteFile, "Unable to save quotation for ref no. " + UCRefNo, docType);
                    MoveFileToError(MTML_QuoteFile, "LeS-1008:Unable to Save Quote for " + UCRefNo, docType);
                }

                if (res)
                {
                    //added by Kalpita on 11/09/2019 to perform send quote
                    this.URL = "https://supplierportal.dnvgl.com/api/Requests/putrequest/" + _library.id;
                    _httpWrapper.Referrer = "https://supplierportal.dnvgl.com/";
                    string lastdet = "";
                    if (postData.Contains("communicationStatus"))
                    {
                        string[] slDList = postData.Split(',');
                        for (int i = 0; i < slDList.Length; i++)
                        {
                            if (slDList[i].Contains("sentDateTime"))
                            {
                                lastdet += slDList[i].Replace("null", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")) + ",";
                            }
                            else
                            {
                                lastdet += (slDList[i].Contains("communicationStatus")) ? slDList[i].Replace('1', '2') + "," : slDList[i] + ",";
                            }
                        }
                        postData = string.Empty; postData = lastdet.TrimEnd(',');
                    }
                    Request_supplierportal_dnvgl_com(postData, this.URL);

                    //if (_httpWrapper.PostURL(URL, postData, "", "", ""))
                    //{
                    //    MoveFileToBackup(MTML_QuoteFile, "Quotation successfully sent.");
                    //}
                    //else
                    //{
                    //    MoveFileToError(MTML_QuoteFile, "Unable to send quote for Ref : '" + UCRefNo, this.docType);
                    //}
                }
                else
                {
                    LogText = "Unable to send quote for Ref : '" + UCRefNo + "' due to Total Amount '" + GrandTotal + "' mismatch on site Grand Total '" + -_library.amount + "'.";
                    //MoveFileToError(MTML_QuoteFile, "Unable to send quote for Ref : '" + UCRefNo + "' due to Total Amount '" + GrandTotal + "' mismatch on site Grand Total '" + -_library.amount + "'.",this.docType);
                    MoveFileToError(MTML_QuoteFile, "LeS-1025.1:Unable to send file due to amount mismatch for " + UCRefNo, this.docType);
                }
            }
        }


        private bool Request_supplierportal_dnvgl_com(string postdata, string url)
        {
            HttpWebResponse response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.KeepAlive = true;
                request.Headers.Add("Sec-Fetch-Mode", @"cors");
                request.Headers.Add("Origin", @"https://supplierportal.dnvgl.com");
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.132 Safari/537.36";
                request.ContentType = "application/json";
                request.Accept = "*/*";
                request.Headers.Add("Sec-Fetch-Site", @"same-origin");
                request.Referer = "https://supplierportal.dnvgl.com/";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");

                request.Method = "PUT";

                string body = postdata;
                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                request.ContentLength = postBytes.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
                stream.Close();

                response = (HttpWebResponse)request.GetResponse();
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

        public DateTime FormatMTMLDate(string DateValue)
        {
            DateTime Dt = DateTime.MinValue;
            if (DateValue != null && DateValue != "")
            {
                if (DateValue.Length > 5)
                {
                    int year = Convert.ToInt32(DateValue.Substring(0, 4));
                    int Month = Convert.ToInt32(DateValue.Substring(4, 2));
                    int day = Convert.ToInt32(DateValue.Substring(6, 2));
                    Dt = new DateTime(year, Month, day);
                }
            }
            return Dt;
        }

        public void MoveFileToError(string MTML_QuoteFile, string message, string DocType)
        {
            string eFile = PrintScreenPath + "DNVGL" + DocType + "_Error_" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
            if (!PrintScreen(eFile)) eFile = "";
            LogText = message;
            if (File.Exists(MTML_QuoteFile))
            {
                if (File.Exists(MTMLUploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile))) File.Delete(MTMLUploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
                File.Move(MTML_QuoteFile, MTMLUploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
                if (File.Exists(MTMLUploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile)))
                    CreateAuditFile(eFile, "DNVGL_HTTP_"+DocType, UCRefNo, "Error", message, BuyerCode, SupplierCode, AuditPath);
            }
        }

        public void MoveFileToBackup(string MTML_QuoteFile, string message)
        {
            if (File.Exists(MTMLUploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile))) File.Delete(MTMLUploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile));
            File.Move(MTML_QuoteFile, MTMLUploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile));


            if (File.Exists(MTMLUploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile)))
                CreateAuditFile(MTML_QuoteFile, "DNVGL_HTTP_" + docType, UCRefNo, "Success", message,BuyerCode,SupplierCode,AuditPath);
            LogText = message;
        }

        private void SendMailNotification(MTMLInterchange _interchange, string DocType, string VRNO, string ActionType, string Message)
        {
            try
            {
                string MailFromDefault = Convert.ToString(dctAppSettings["FromEmailId"]);
                if (MailFromDefault == null) MailFromDefault = "";
                string MailBccDefault = Convert.ToString(dctAppSettings["MailBcc"]);
                if (MailBccDefault == null) MailBccDefault = "";
                string MailCcDefault = "";
                if (dctAppSettings.ContainsKey("MAIL_CC"))
                    MailCcDefault = Convert.ToString(dctAppSettings["MAIL_CC"]);

                string BuyerCode = Convert.ToString(_interchange.Recipient).Trim();
                string SuppCode = Convert.ToString(_interchange.Sender).Trim();
                string BuyerID = Convert.ToString(_interchange.BuyerSuppInfo.BuyerID).Trim();
                string SupplierID = Convert.ToString(_interchange.BuyerSuppInfo.SupplierID).Trim();

                string MailAuditPath = Convert.ToString(dctAppSettings["MailAuditPath"]);
                if (MailAuditPath.Trim() != "")
                {
                    if (!Directory.Exists(MailAuditPath.Trim())) Directory.CreateDirectory(MailAuditPath.Trim());
                }
                else throw new Exception("MAIL_AUDIT_PATH value is not defined in config file.");

                //string MailSettings = Convert.ToString(dctAppSettings[SuppCode + "-" + BuyerCode]);//27-2-2018
                string MailSettings = Convert.ToString(dctAppSettings["MailSettings"]);
                if (MailSettings.Trim() != "")
                {
                    string[] arr = MailSettings.Trim().Split('|');

                    string notifySupp = arr[0].Trim().ToUpper();
                    string notifyBuyer = arr[1].Trim().ToUpper();
                    string notifyCC = arr[2].Trim().ToUpper();
                    string notifyBCC = arr[3].Trim().ToUpper();
                    string sendHTML = arr[4].Trim().ToUpper();
                    string sendAttachment = arr[5].Trim().ToUpper();
                    string byrFromMailID = arr[6].Trim().ToUpper();
                    string useDefaultFromMailID = arr[7].Trim().ToUpper();
                    string supplierID = arr[8].Trim().ToUpper();
                    string byrMailTemplate = arr[9].Trim().ToUpper();

                    if (SupplierID.Trim().Length == 0) SupplierID = supplierID;

                    string MailBodyTemplate = arr[10].Trim().ToUpper();
                    string SuppLinkMailID = arr[11].Trim();

                    MailBodyTemplate = System.Windows.Forms.Application.StartupPath + "\\MAIL_TEMPLATES\\" + MailBodyTemplate.Trim();
                    if (!File.Exists(MailBodyTemplate)) throw new Exception("Mail Body Template '" + Path.GetFileName(MailBodyTemplate) + "' not found under MAIL_TEMPLATES folder.");

                    string SubjectTempate = System.Windows.Forms.Application.StartupPath + "\\MAIL_TEMPLATES\\MAIL_SUBJECT.txt";
                    if (!File.Exists(SubjectTempate)) throw new Exception("Subject Template 'MAIL_SUBJECT.txt' not found under MAIL_TEMPLATES folder.");

                    string attachmentFile = Convert.ToString(_interchange.DocumentHeader.OriginalFile);
                    if (attachmentFile.Trim() != "" && Path.GetExtension(attachmentFile).ToUpper().Contains("XML"))
                    {
                        attachmentFile = ""; // DO NOT SEND XML FILE AS ATTACHMENT
                    }

                    string Vessel = "", SenderName = "", RecipientName = "", ByrMailID = "", SuppMailID = "";

                    #region // Get Part Address Details //
                    foreach (Party _partyObj in _interchange.DocumentHeader.PartyAddresses)
                    {
                        if (_partyObj.Qualifier == PartyQualifier.BY)
                        {
                            RecipientName = Convert.ToString(_partyObj.Name).Trim(); // Buyer Name
                            if (_partyObj.Contacts.Count > 0 && _partyObj.Contacts[0].CommunMethodList.Count > 0)
                            {
                                foreach (CommunicationMethods commMethod in _partyObj.Contacts[0].CommunMethodList)
                                {
                                    if (commMethod.Qualifier == CommunicationMethodQualifiers.EM)
                                    {
                                        ByrMailID = Convert.ToString(commMethod.Number).Trim();
                                        break;
                                    }
                                }
                            }
                        }
                        else if (_partyObj.Qualifier == PartyQualifier.VN)
                        {
                            SenderName = Convert.ToString(_partyObj.Name).Trim(); // Vendor Name
                            if (_partyObj.Contacts.Count > 0 && _partyObj.Contacts[0].CommunMethodList.Count > 0)
                            {
                                foreach (CommunicationMethods commMethod in _partyObj.Contacts[0].CommunMethodList)
                                {
                                    if (commMethod.Qualifier == CommunicationMethodQualifiers.EM)
                                    {
                                        SuppMailID = Convert.ToString(commMethod.Number).Trim();
                                        SuppMailID = "";//03-11-2017
                                        break;
                                    }
                                }
                            }
                        }
                        else if (_partyObj.Qualifier == PartyQualifier.UD)
                        {
                            Vessel = Convert.ToString(_partyObj.Name).Trim();
                        }
                    }
                    #endregion

                    if (SuppMailID.Trim() == "")
                    {
                        SuppMailID = SuppLinkMailID.Trim();
                    }

                    #region // NOTIFY TO SUPPLIER //
                    if (notifySupp == "YES")
                    {
                        // Send Mail Notification for Supplier
                        string MailFrom = MailFromDefault, MailTo = SuppMailID.Trim().Replace("E-mail:", "").Trim(), mailBody = "";

                        if (MailTo.Trim() != "")
                        {
                            int QuotationID = Convert.ToInt32(_interchange.BuyerSuppInfo.RecordID);

                            #region // Set Subject //
                            string Subject = File.ReadAllText(SubjectTempate);
                            Subject = Subject.Replace("#DOC_TYPE#", DocType.Trim().ToUpper());
                            Subject = Subject.Replace("#REF_NO#", VRNO.Trim());
                            if (Vessel.Trim().Length > 0)
                            {
                                Subject = Subject.Replace("#VESSEL_NAME#", Vessel.Trim());
                            }
                            else
                            {
                                Subject = Subject.Replace("VESSEL : #VESSEL_NAME# ", "");
                            }
                            Subject = Subject.Replace("#ACTION_TYPE#", ActionType.Trim());
                            Subject = Subject.Replace("#SENDER#", SenderName.Trim());
                            Subject = Subject.Replace("#RECEIVER#", RecipientName.Trim());
                            Subject = Subject.Replace("#BUYER_CODE#", BuyerCode.Trim().ToUpper().Replace("_NEKOXLS", ""));
                            #endregion

                            #region // Set MailBody //
                            mailBody = File.ReadAllText(MailBodyTemplate);
                            mailBody = mailBody.Replace("#DOC_TYPE#", DocType.Trim().ToUpper());
                            mailBody = mailBody.Replace("#SENDER#", SenderName.Trim());
                            mailBody = mailBody.Replace("#BUYER_NAME#", RecipientName.Trim());
                            mailBody = mailBody.Replace("#BUYER_CODE#", BuyerCode.Trim().ToUpper().Replace("_NEKOXLS", ""));
                            mailBody = mailBody.Replace("#SUPPLIER_NAME#", SenderName.Trim().ToUpper());
                            mailBody = mailBody.Replace("#MESSAGE#", Message.Trim());
                            #endregion

                            string mailText = QuotationID + "|" +
                                DocType.Trim().ToUpper() + "|" +
                                VRNO.Trim() + "|" +
                                MailFrom.Trim() + "|" +
                                MailTo.Trim() + "|" +
                                (notifyCC == "YES" ? MailCcDefault : "") + "|" +
                                (notifyBCC == "YES" ? MailBccDefault : "") + "|" +
                                Subject.Trim() + "|" +
                                mailBody + "|" +
                                (sendAttachment == "YES" ? attachmentFile : "") + "|" +
                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "|" +
                                "0" + "|" + // NOT TO SEND FLAG
                                BuyerID + "|" + // BUYERID
                                SupplierID + "|" + // SUPPLIERID
                                "|" + // Reply Email ID
                                SenderName + "|" + // Supplier Name
                                RecipientName + "|" + // Buyer Name
                                ActionType.Trim().ToUpper() + "|" + // ACTION TYPE
                                "0" + "|" + // DO NOT SEND HTML
                                "1" + "|" + // Send Html Msg
                                "1"; // Use Html File Msg

                            // Write To File
                            File.WriteAllText(MailAuditPath + "\\MailNotify_" + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".txt", mailText.Trim());
                            LogText = "Mail Send to Supplier Email -" + MailTo.Trim() + ".";
                        }
                        else
                        {
                            LogText = "Unable to send mail notification to supplier; Supplier Mailid is empty.";
                        }
                    }
                    #endregion
                }
                else
                {
                    LogText = "Unable to send mail notification; No mail setting found for Supplier-Buyer (" + SuppCode + "-" + BuyerCode + ") link combination.";
                }
            }
            catch (Exception ex)
            {
                LogText = "Unable to create Mail notification template. Error : " + ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }

        //public void AcknowledgeButton_Click()
        //{
        //    if (isAcknowledgeUpdates)//add code when button appear in quote                                                
        //    {
        //        var _btn = _netWrapper.CurrentDocument.GetElementsByTagName("button");
        //        if (_btn.Count > 0)
        //        {
        //            foreach (var b in _btn)
        //            {
        //                if (b != null && ((DOMElement)b).InnerText != null)
        //                {
        //                    string txt = ((DOMElement)b).InnerText;
        //                    if (((DOMElement)b).InnerText == "Acknowledge Updates")//added by kalpita on 05/07/2019
        //                    {
        //                        string c = ((DOMElement)b).InnerHTML;
        //                        ((DOMElement)b).Click();
        //                    }
        //                }
        //                // throw new Exception("Acknowledge Updates button found for ref " + strUCRefNo);
        //            }
        //        }
        //    }
        //}
        #endregion

        #region POC
        public void ProcessPOC()
        {
            try
            {
                LogText = "";
                LogText = "POC processing started.";
                if (POC_mtmlFiles.Count == 0) GetXmlFiles();
                if (POC_mtmlFiles.Count > 0)
                {
                    LogText = POC_mtmlFiles.Count + " files found to process.";
                    for (int j = POC_mtmlFiles.Count-1 ; j >= 0; j--)
                    {
                        MTMLClass _mtml = new MTMLClass();
                        _interchange = _mtml.Load(POC_mtmlFiles[j]);
                        LoadInterchangeDetails();
                        ProcessPOCMTML(POC_mtmlFiles[j]);
                    }
                }
                else LogText = "No POC files found.";
                LogText = "POC processing stopped.";
            }
            catch (Exception ex)
            {
                string eFile = PrintScreenPath + "\\DNVGL_" + this.docType + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + BuyerCode + "_" + SupplierCode + ".png";
                if (!PrintScreen(eFile)) eFile = "";
                LogText = "Exception while processing POC: " + ex.GetBaseException().ToString();
            }
        }

        public void ProcessPOCMTML(string MTML_POCFile)
        {
            string URL = "", Token = ""; bool isCatch = false;
            try
            {
                LogText = "'" + Path.GetFileName(MTML_POCFile) + "' POC file processing started.";
                URL = MessageNumber.Split('|')[0];
                Token = MessageNumber.Split('|')[1];
                if (URL != "" && URL.Trim().Contains("https://supplierportal.dnvgl.com/") && Token.Trim() != "" && UCRefNo != "")
                {
                    dctPostDataValues.Clear();
                    dctPostDataValues.Add("Password", Token);
                    dctPostDataValues.Add("Type", "2");

                    _httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.CacheControl);
                    _httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");
                    _httpWrapper.ContentType = "application/json";
                    _httpWrapper.AcceptMimeType = "*/*";

                    string[] ArrURL = URL.Trim().Split('/');
                    URL = ArrURL[0] + "//" + ArrURL[2] + "/api/Security/PostLogin/" + ArrURL[4];
                    string _postdata = GetPostData(true);
                    bool isLoggedin = _httpWrapper.PostURL(URL, _postdata, "", "", "");
                    if (isLoggedin && _httpWrapper._CurrentResponseString.Contains("true"))
                    {
                        this.URL = URL.Trim().Replace("Security/PostLogin", "Orders/getorder");
                        try
                        {
                            LoadURL("", "", "");
                        }
                        catch (Exception ex)
                        {
                            isCatch = true;
                            //MoveFileToError(MTML_POCFile, "Exception while loading POC page : " + ex.GetBaseException().Message.ToString(), docType);
                            MoveFileToError(MTML_POCFile, "LeS-1016:Unable to load url due to " + ex.GetBaseException().Message.ToString(), docType);
                        }

                        if (!isCatch)
                        {
                            Fill_POC(MTML_POCFile, MessageNumber.Split('|')[1], MessageNumber.Split('|')[0]);
                        }
                    }
                    else
                    {
                        //MoveFileToError(MTML_POCFile, "Unable to login", docType);
                        MoveFileToError(MTML_POCFile, "LeS-1014:Unable to login", docType);
                    }
                }
                else
                {
                    LogText = "Unable to navigate to URL for POC " + UCRefNo;
                }
            }
            catch (Exception ex)
            {
                LogText = "Exception while processing POC MTML : " + ex.GetBaseException().ToString();
            }
        }

        public void Fill_POC(string MTML_POCFile,string _Token,string _URL)
        {
            try
            {
                var _library = (OrderRootObject)JsonConvert.DeserializeObject(_httpWrapper._CurrentResponseString, typeof(OrderRootObject));
                if (_library != null)
                {
                    if (_library.orderCode == UCRefNo)
                    {
                        //if (_library.sentDateTime == null && _library.communicationStatus == 1)
                        //{
                        if (Convert.ToDouble(GrandTotal) != Convert.ToDouble(0))
                        {
                            //filling header details and filling item details remaining
                            if (!IsDecline)
                            {
                                bool result = false;
                                if (_library.amount == convert.ToFloat(GrandTotal)) result = true;
                                else if (_library.amount < convert.ToFloat(GrandTotal)) { if (convert.ToFloat(GrandTotal) - _library.amount <= 2) result = true; }
                                else if (convert.ToFloat(GrandTotal) < _library.amount) { if (_library.amount - convert.ToFloat(GrandTotal) <= 2)result = true; }
                                if (result)
                                {
                                    //confirm order
                                    URL = "https://supplierportal.dnvgl.com/api/Orders/putorder/" + _library.id;
                                    #region post data
                                    string orderedAt = "", etaDate = "", origDelDate = "", fetchingDate = "", createdAt = "", modifiedDate = "";//,sendDateTime="";
                                    string delDate = convert.ToDateTime(DelvDate).ToString("yyyy-MM-dd");
                                    if (_library.orderedAt != null) orderedAt = convert.ToDateTime(_library.orderedAt).ToString("yyyy-MM-ddTHH:mm:ss");
                                    if (_library.etaDate != null) etaDate = convert.ToDateTime(_library.etaDate).ToString("yyyy-MM-ddTHH:mm:ss");
                                    if (_library.originalDeliveryDate != null) origDelDate = convert.ToDateTime(_library.originalDeliveryDate).ToString("yyyy-MM-ddTHH:mm:ss");
                                    if (_library.fetchingDate != null) fetchingDate = convert.ToDateTime(_library.fetchingDate).ToString("yyyy-MM-ddTHH:mm:ss.fff");
                                    if (_library.createdAt != null) createdAt = convert.ToDateTime(_library.createdAt).ToString("yyyy-MM-ddTHH:mm:ss.fff");
                                    if (_library.modifiedDateTime != null) modifiedDate = convert.ToDateTime(_library.modifiedDateTime).ToString("yyyy-MM-ddTHH:mm:ss.ff");//_library.createdAt//commented to check,few more poc's

                                    string sourceXML = _library.sourceXML.Replace("\"false\"", "\\\"false\\\"").Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "<?xml version=\\\"1.0\\\" encoding=\\\"utf-8\\\"?>").Replace("\"http://www.w3.org/2001/XMLSchema-instance\"", "\\\"http://www.w3.org/2001/XMLSchema-instance\\\"")
                                        .Replace("\"http://www.w3.org/2001/XMLSchema\"", "\\\"http://www.w3.org/2001/XMLSchema\\\"").Replace("\r\n", "\\r\\n");
                                    if (_library.shippingCompPhone == null) _library.shippingCompPhone = "null"; else _library.shippingCompPhone = "\"" + _library.shippingCompPhone + "\"";

                                    string postData = "{\"id\":" + _library.id + ",\"creatorName\":\"" + _library.creatorName + "\",\"creatorPhone\":\"" + _library.creatorPhone + "\",\"creatorEmail\":\"" + _library.creatorEmail +
                                        "\",\"dsno\":" + _library.dsno + ",\"orderCode\":\"" + _library.orderCode + "\",\"orderName\":\"" + _library.orderName + "\",\"orderRemark\":\"" + _library.orderRemark +
                                        "\",\"orderedAt\":\"" + orderedAt + "\",\"shipName\":\"" + _library.shipName + "\",\"etaPort\":\"" + _library.etaPort + "\",\"etaDate\":\"" + etaDate + "\",\"deliveryDate\":\"" + delDate +
                                        "\",\"originalDeliveryDate\":\"" + origDelDate + "\",\"fetchingDate\":\"" + fetchingDate + "\",\"discount\":" + _library.discount + ",\"additionalCharges\":" + _library.additionalCharges +
                                        ",\"amount\":" + _library.amount + ",\"currencyShortName\":\"" + _library.currencyShortName + "\",\"footerText1\":\"" + _library.footerText1 + "\",\"footerText2\":\"" + _library.footerText2 +
                                        "\",\"termsConditions1\":\"" + _library.termsConditions1 + "\",\"termsConditions2\":\"" + _library.termsConditions2 + "\",\"techContactSalutation\":\"" + _library.techContactSalutation +
                                        "\",\"techContactFirstName\":\"" + _library.techContactFirstName + "\",\"techContactName\":\"" + _library.techContactName + "\",\"techContactPhone\":\"" + _library.techContactPhone +
                                        "\",\"techContactEmail\":\"" + _library.techContactEmail + "\",\"delAddName\":\"" + _library.delAddName + "\",\"delAddContactSalutation\":\"" + _library.delAddContactSalutation + "\",\"delAddContactFirstName\":\"" +
                                        _library.delAddContactFirstName + "\",\"delAddContactName\":\"" + _library.delAddContactName + "\",\"delAddName2\":\"" + _library.delAddName2 + "\",\"delAddStreet\":\"" + _library.delAddStreet +
                                        "\",\"delAddZipCode\":\"" + _library.delAddZipCode + "\",\"delAddTown\":\"" + _library.delAddTown + "\",\"delAddCountry\":\"" + _library.delAddCountry + "\",\"delAddPhone\":\"" + _library.delAddPhone +
                                        "\",\"delAddEmail\":\"" + _library.delAddEmail + "\",\"agentName\":\"" + _library.agentName + "\",\"agentContactSalutation\":\"" + _library.agentContactSalutation + "\",\"agentContactFirstName\":\"" +
                                        _library.agentContactFirstName + "\",\"agentContactName\":\"" + _library.agentContactName + "\",\"agentStreet\":\"" + _library.agentStreet + "\",\"agentZipCode\":\"" + _library.agentZipCode +
                                        "\",\"agentTown\":\"" + _library.agentTown + "\",\"agentCountry\":\"" + _library.agentCountry + "\",\"agentContactEmail\":\"" + _library.agentContactEmail + "\",\"agentContactPhone\":\"" + _library.agentContactPhone +
                                        "\",\"fetchingAddrName\":\"" + _library.fetchingAddrName + "\",\"fetchingAddrName2\":\"" + _library.fetchingAddrName2 + "\",\"fetchingAddrStreet\":\"" + _library.fetchingAddrStreet +
                                        "\",\"fetchingAddrZipCode\":\"" + _library.fetchingAddrZipCode + "\",\"fetchingAddrTown\":\"" + _library.fetchingAddrTown + "\",\"fetchingAddrCountry\":\"" + _library.fetchingAddrCountry + "\",\"fetchingAddrEmail\":\"" + _library.fetchingAddrEmail +
                                        "\",\"fetchingAddrPhone\":\"" + _library.fetchingAddrPhone + "\",\"invoiceName\":\"" + _library.invoiceName + "\",\"invoiceName2\":\"" + _library.invoiceName2 + "\",\"invoiceStreet\":\"" + _library.invoiceStreet +
                                        "\",\"invoiceZipCode\":\"" + _library.invoiceZipCode + "\",\"invoiceTown\":\"" + _library.invoiceTown + "\",\"invoiceCountry\":\"" + _library.invoiceCountry + "\",\"vatNumber\":\"" + _library.vatNumber +
                                        "\",\"supplierAddrName\":\"" + _library.supplierAddrName + "\",\"supplierContSalutation\":\"" + _library.supplierContSalutation + "\",\"supplierContFirstName\":\"" + _library.supplierContFirstName +
                                        "\",\"supplierContName\":\"" + _library.supplierContName + "\",\"supplierAddrStreet\":\"" + _library.supplierAddrStreet + "\",\"supplierAddrZIP\":\"" + _library.supplierAddrZIP + "\",\"supplierAddrTown\":\"" + _library.supplierAddrTown +
                                        "\",\"supplierAddrCountry\":\"" + _library.supplierAddrCountry + "\",\"supplierContEmail\":\"" + _library.supplierContEmail + "\",\"supplierReference\":\"" + _library.supplierReference + "\",\"supplierPaymentTerms\":\"" + _library.supplierPaymentTerms +
                                        "\",\"supplierDeliveryTerms\":\"" + _library.supplierDeliveryTerms + "\",\"shippingCompanyID\":\"" + _library.shippingCompanyID + "\",\"shippingCompName\":\"" + _library.shippingCompName + "\",\"shippingCompStreet\":\"" + _library.shippingCompStreet +
                                        "\",\"shippingCompZip\":\"" + _library.shippingCompZip + "\",\"shippingCompTown\":\"" + _library.shippingCompTown + "\",\"shippingCompPhone\":" + _library.shippingCompPhone + ",\"shippingCompCountry\":\"" + _library.shippingCompCountry +
                                        "\",\"createdAt\":\"" + createdAt + "\",\"password\":\"" + _library.password + "\"";
                                    postData = postData + ",\"sourceXML\":\"" + sourceXML + "\"";
                                    postData = postData + ",\"modifiedDateTime\":\"" + modifiedDate + "\",\"sentDateTime\":\"" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\",\"communicationStatus\":2,\"supplierOrderState\":2" +
                                        ",\"recStatus\":" + _library.recStatus + ",\"isNewDB\":" + convert.ToString(_library.isNewDB).ToLower() + ",\"mailRecipientSupplier\":\"" + _library.mailRecipientSupplier + "\",\"mailRecipientBuyer\":\"" + _library.mailRecipientBuyer +
                                        "\",\"orderStatusSM\":\"" + _library.orderStatusSM + "\",\"reminderSent\":" + Convert.ToString(_library.reminderSent).ToLower() + ",\"userIP\":\"" + _library.userIP + "\"";
                                    postData = postData + ",\"visibleFields\":\"{\\\"FetchingDate\\\":false,\\\"PaymentTerms\\\":true,\\\"DeliveryTerms\\\":true,\\\"TermsConditions1\\\":false,\\\"TermsConditions2\\\":false,\\\"TermsConditions\\\":true," +
                                        "\\\"TechContactSalutation\\\":false,\\\"TechContactFirstName\\\":false,\\\"TechContactName\\\":false,\\\"TechContactEmail\\\":false,\\\"TechContactPhone\\\":false,\\\"TechContact\\\":false,\\\"AgentContactSalutation\\\":true," +
                                        "\\\"AgentContactFirstName\\\":true,\\\"AgentContactName\\\":true,\\\"AgentContactEmail\\\":true,\\\"AgentContactPhone\\\":true,\\\"AgentName\\\":true,\\\"AgentStreet\\\":true,\\\"AgentZipCode\\\":true," +
                                        "\\\"AgentTown\\\":true,\\\"AgentCountry\\\":true,\\\"AgentDetails\\\":true,\\\"FetchingName\\\":false,\\\"FetchingName2\\\":false,\\\"FetchingStreet\\\":false,\\\"FetchingZipCode\\\":false,\\\"FetchingTown\\\":false," +
                                        "\\\"FetchingCountry\\\":false,\\\"FetchingEmail\\\":false,\\\"FetchingPhone\\\":false,\\\"PickUpAddress\\\":false,\\\"Footer1\\\":false,\\\"Footer2\\\":false}\",";//+ _library.visibleFields.Replace("\"\"","\\\"")
                                    int j = 0;
                                    foreach (OrderItemGroup Items in _library.orderItemGroups)
                                    {
                                        string itemData = "";
                                        itemData = "\"orderItemGroups\":[{\"id\":" + Items.id + ",\"dsno\":\"" + Items.dsno + "\",\"name\":\"" + Items.name + "\",\"name2\":\"" + Items.name2 + "\",\"makersName\":\"" + Items.makersName +
                                        "\",\"serialNumber\":\"" + Items.serialNumber + "\"";
                                        foreach (OrderItem item in Items.orderItems)
                                        {
                                            j++;
                                            LineItem _item = null;
                                            foreach (LineItem mitem in _lineitem)
                                            {
                                                if (item.itemOrderNumber == mitem.OriginatingSystemRef)
                                                {
                                                    _item = mitem;
                                                }
                                            }
                                            if (_item != null)
                                            {
                                                string ItemdelDate = "";
                                                if (_item.DeleiveryTime != null && _item.DeleiveryTime != "0")
                                                {
                                                    string delveDate = DateTime.Now.AddDays(Convert.ToDouble(_item.DeleiveryTime)).ToString("MM/dd/yyyy");
                                                    ItemdelDate = convert.ToDateTime(delveDate).ToString("yyyy-MM-dd");
                                                }
                                                else
                                                {
                                                    ItemdelDate = delDate;
                                                }
                                                if (item.orderItems == null) item.orderItems = "null";
                                                if (item.visibleFields == null) item.visibleFields = "null";
                                                if (item.changedFields == null) item.changedFields = "null";
                                                if (item.remark == null) item.remark = "null"; else item.remark = "\"" + item.remark + "\"";

                                                if (j == 1)
                                                    itemData += ",\"orderItems\":[{";
                                                else itemData += ",{";
                                                itemData += "\"id\":" + item.id + ",\"dsno\":" + item.dsno + ",\"itemOrderNumber\":\"" + item.itemOrderNumber + "\",\"itemName\":\"" + item.itemName + "\",\"itemName2\":\"" + item.itemName2 +
                                                    "\",\"remark\":" + item.remark + ",\"itemText\":\"" + item.itemText + "\",\"externalRemark\":\"" + item.externalRemark + "\",\"supplierOrderCode\":\"" + item.supplierOrderCode +
                                                    "\",\"itemPartNumber\":\"" + item.itemPartNumber + "\",\"drawingNumber\":\"" + item.drawingNumber + "\",\"quantity\":" + item.quantity + ",\"unit\":\"" + item.unit +
                                                    "\",\"price\":" + item.price + ",\"priceUnit\":" + item.priceUnit + ",\"discount\":" + item.discount + ",\"itemTotal\":" + item.itemTotal + ",\"deliveryUntil\":\"" + ItemdelDate +
                                                    "\",\"currencyShort\":\"" + item.currencyShort + "\",\"xmlUpdated\":" + convert.ToString(item.xmlUpdated).ToLower() + ",\"changedFields\":" + item.changedFields + ",\"recStatus\":" + item.recStatus +
                                                    ",\"visibleFields\":" + item.visibleFields + ",\"orderItems\":" + item.orderItems + "}";
                                                if (j < Items.orderItems.Count) itemData += "\n";
                                                if (j == Items.orderItems.Count)
                                                {
                                                    itemData = itemData.TrimEnd(',') + "],"; j = 0;
                                                    itemData = itemData + "\"xmlUpdated\":false,\"recStatus\":0}]}";
                                                }
                                            }
                                            else
                                            {
                                                LogText = "Item not found on site or in mtml with Part No " + item.itemPartNumber + " for refNo " + UCRefNo;
                                                throw new Exception("Item not found on site or in mtml with Part No " + item.itemPartNumber + " for refNo " + UCRefNo);
                                            }
                                        }
                                        postData = postData + itemData.TrimEnd(',');
                                    }
                                    #endregion
                                    try
                                    {
                                        ConfirmOrder(postData, _Token, _URL, MTML_POCFile);
                                    }
                                    catch (Exception ex)
                                    {
                                        //MoveFileToError(MTML_POCFile, "Exception while confirming POC for ref no " + UCRefNo, this.docType);
                                        MoveFileToError(MTML_POCFile, "LeS-1004:Unable to process file for " + UCRefNo, this.docType);
                                    }
                                }
                                else
                                {
                                    //MoveFileToError(MTML_POCFile, " Unable to upload POC for Ref : '" + UCRefNo + "' due to Total Amount '" + GrandTotal + "' mismatch on site Grand Total '" + _library.amount + "'.", this.docType);
                                    LogText = " Unable to upload POC for Ref : '" + UCRefNo + "' due to Total Amount '" + GrandTotal + "' mismatch on site Grand Total '" + _library.amount;
                                    MoveFileToError(MTML_POCFile, "LeS-1008.9:Unable to Save POC due to amount mismatch", this.docType);
                                }
                            }
                            else
                            {
                                //decline order
                                LogText = "Decline POC " + MTML_POCFile + " found for buyer " + BuyerName;
                                //CreateAuditFile(MTML_POCFile, "DNVGL_HTTP_" + this.docType, UCRefNo, "Error", "Decline POC " + Path.GetFileName(MTML_POCFile) + " found for buyer " + BuyerName + ", move to error.", BuyerCode, SupplierCode, AuditPath);
                                CreateAuditFile(MTML_POCFile, "DNVGL_HTTP_" + this.docType, UCRefNo, "Error", "LeS-1008.10:Unable to Save POC since POC is in Declined status for " + Path.GetFileName(MTML_POCFile) + " found for buyer " + BuyerName + ", move to error.", BuyerCode, SupplierCode, AuditPath);
                                File.Move(MTML_POCFile, Path.GetDirectoryName(MTML_POCFile) + "\\Error\\" + Path.GetFileName(MTML_POCFile));
                            }
                        }
                        else
                        {
                            //MoveFileToError(MTML_POCFile, "Grand total is zero for Ref  " + UCRefNo, this.docType);
                            MoveFileToError(MTML_POCFile, "LeS-1008.5:Unable to Save Quote since quote total is zero for " + UCRefNo, this.docType);
                        }
                    }
                    else
                    {
                        //MoveFileToError(MTML_POCFile, "Ref No. " + UCRefNo + " mismatch with website ref " + _library.orderCode, this.docType);
                        LogText = "Ref No. " + UCRefNo + " mismatch with website ref " + _library.orderCode;
                        MoveFileToError(MTML_POCFile, "LeS-1007:Unable to Save " + this.docType + " file for  " + _library.orderCode+" - Ref No. Field(s) not present." , this.docType);
                    }
                }
                else
                {
                    //MoveFileToError(MTML_POCFile, "Unable to get POC page details", this.docType);
                    MoveFileToError(MTML_POCFile, "LeS-1006:Unable to filter details", this.docType);
                }
            }
            catch (Exception ex)
            {
                //MoveFileToError(MTML_POCFile, "Exception while filling POC MTML : " + ex.GetBaseException().Message.ToString(), this.docType);
                MoveFileToError(MTML_POCFile, "LeS-1004:Unable to process file due to " + ex.GetBaseException().Message.ToString(), this.docType);
            }
        }

        public void ConfirmOrder(string postdata, string _Token, string _URL, string MTML_POCFile)
        {
            try
            {
                _httpWrapper.RequestMethod = "PUT";
                if (_httpWrapper.PostURL(URL, postdata, "", "", ""))
                {
                    if (_URL != "" && _Token != "")
                    {
                        if (URL != "" && URL.Trim().Contains("https://supplierportal.dnvgl.com/") && _Token.Trim() != "" && UCRefNo != "")
                        {
                            dctPostDataValues.Clear();
                            dctPostDataValues.Add("Password", _Token);
                            dctPostDataValues.Add("Type", "2");

                            _httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.CacheControl);
                            _httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");
                            _httpWrapper.ContentType = "application/json";
                            _httpWrapper.AcceptMimeType = "*/*";

                            string[] ArrURL = URL.Trim().Split('/');
                            URL = ArrURL[0] + "//" + ArrURL[2] + "/api/Security/PostLogin/" + ArrURL[4];
                            string _postdata = GetPostData(true);
                            bool isLoggedin = _httpWrapper.PostURL(URL, _postdata, "", "", "");
                            if (isLoggedin && _httpWrapper._CurrentResponseString.Contains("true"))
                            {
                                this.URL = URL.Trim().Replace("Security/PostLogin", "Orders/getorder");
                                try
                                {
                                    LoadURL("", "", "");
                                    var _library = (OrderRootObject)JsonConvert.DeserializeObject(_httpWrapper._CurrentResponseString, typeof(OrderRootObject));
                                    if (_library != null)
                                    {
                                        if (_library.orderCode == UCRefNo)
                                        {
                                            if (_library.sentDateTime != null && _library.communicationStatus == 2)
                                            {
                                                LogText = "Purchase Order successfully confirmed";
                                                string eFile = PrintScreenPath + "DNVGL" + this.docType + "_" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
                                                if (!PrintScreen(eFile)) eFile = "";
                                                CreateAuditFile(eFile, "DNVGL_HTTP_"+this.docType, VRNO, "Success", "Purchase Order successfully confirmed", BuyerCode, SupplierCode, AuditPath);//error
                                                if (File.Exists(MTMLUploadPath + "\\Backup\\" + Path.GetFileName(MTML_POCFile))) File.Delete(MTMLUploadPath + "\\Backup\\" + Path.GetFileName(MTML_POCFile));
                                                File.Move(MTML_POCFile, MTMLUploadPath + "\\Backup\\" + Path.GetFileName(MTML_POCFile));
                                            }
                                            else
                                            {
                                                //MoveFileToError(MTML_POCFile, "Unable to confirm order for Ref No: " + UCRefNo, this.docType);                                            
                                                MoveFileToError(MTML_POCFile, "LeS-1004:Unable to process file for " + UCRefNo, this.docType);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //MoveFileToError(MTML_POCFile, "Exception while loading POC page : " + ex.GetBaseException().Message.ToString(), docType);
                                    MoveFileToError(MTML_POCFile, "LeS-1016:Unable to load url due to " + ex.GetBaseException().Message.ToString(), docType);
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LogText = "Exetion while confirming POC due to " + ex.Message;
                throw ex;
            }
        }
        #endregion

        public void WriteErrorLog_With_Screenshot(string AuditMsg)
        {
            LogText = AuditMsg;
            string eFile = PrintScreenPath + "\\DNVGL_" + this.docType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + BuyerCode + "_" + SupplierCode + ".png";
            if (!PrintScreen(eFile)) eFile = "";
            CreateAuditFile(eFile, "DNVGL_HTTP_"+this.docType, VRNO, "Error", AuditMsg, BuyerCode, SupplierCode, AuditPath);
        }

        public override bool PrintScreen(string sFileName)
        {
            if (!Directory.Exists(Path.GetDirectoryName(sFileName) + "\\Temp")) Directory.CreateDirectory(Path.GetDirectoryName(sFileName) + "\\Temp");
            sFileName = Path.GetDirectoryName(sFileName) + "\\Temp\\" + Path.GetFileName(sFileName);
            if (base.PrintScreen(sFileName))
            {
                MoveFiles(sFileName, this.PrintScreenPath + "\\" + Path.GetFileName(sFileName));
                return (File.Exists(this.PrintScreenPath + "\\" + Path.GetFileName(sFileName)));
            }
            else return false;
        }

    }

    public class Item
    {
        public int id { get; set; }
        public int dsno { get; set; }
        public string itemOrderNumber { get; set; }
        public string name { get; set; }
        public string name2 { get; set; }
        public double stockQuantity { get; set; }
        public string stockUnit { get; set; }
        public double unitConvFactor { get; set; }
        public double quantity { get; set; }
        public string purchaseUnit { get; set; }
        public double price { get; set; }
        public double discount { get; set; }
        public string partNumber { get; set; }
        public string drawingNumber { get; set; }
        public string companyNumber { get; set; }
        public string remark { get; set; }
        public string itemText { get; set; }
        public string externalRemark { get; set; }
        public int deliveryDays { get; set; }
        public string supplierOrderCode { get; set; }
        public string itemRemark { get; set; }
        public string exContrClassNumber { get; set; }
        public string exContrMilitCode { get; set; }
        public string exContrCofoCode { get; set; }
        public string exContrPercUsContent { get; set; }
        public string exContrCustCode { get; set; }
        public double itemTotal { get; set; }
        public bool xmlUpdated { get; set; }
        public object changedFields { get; set; }
        public int recStatus { get; set; }
        public string visibleFields { get; set; }
    }

    public class ItemGroup
    {
        public int id { get; set; }
        public string dsno { get; set; }
        public string name { get; set; }
        public string name2 { get; set; }
        public string makersName { get; set; }
        public string serialNumber { get; set; }
        public List<Item> items { get; set; }
        public bool xmlUpdated { get; set; }
        public object changedFields { get; set; }
        public int recStatus { get; set; }
    }

    public class RootObject
    {
        public int id { get; set; }
        public DateTime exportTimeToSm { get; set; }
        public string creatorName { get; set; }
        public string creatorPhone { get; set; }
        public string creatorEmail { get; set; }
        public int dsno { get; set; }
        public string requestCode { get; set; }
        public string name { get; set; }
        public string remark { get; set; }
        public string shipName { get; set; }
        public string etaPort { get; set; }
        public DateTime etaDate { get; set; }
        public DateTime deliveryDate { get; set; }
        public DateTime quotationDeadline { get; set; }
        public string currencyShortName { get; set; }
        public double discount { get; set; }
        public double additionalCharges { get; set; }
        public double amount { get; set; }
        public string delAddName { get; set; }
        public string delAddName2 { get; set; }
        public string delAddStreet { get; set; }
        public string delAddZipCode { get; set; }
        public string delAddTown { get; set; }
        public string supplierAddrName { get; set; }
        public string supplierContSalutation { get; set; }
        public string supplierContFirstName { get; set; }
        public string supplierContName { get; set; }
        public string supplierAddrStreet { get; set; }
        public string supplierAddrZIP { get; set; }
        public string supplierAddrTown { get; set; }
        public string supplierAddrCountry { get; set; }
        public string supplierContEmail { get; set; }
        public string supplierReference { get; set; }
        public DateTime quotationDate { get; set; }
        public string supplierRemark { get; set; }
        public string paymentTerms { get; set; }
        public string validity { get; set; }
        public string deliveryTime { get; set; }
        public string supplierCountryOrigin { get; set; }
        public string supplierPrincipals { get; set; }
        public string supplierPartQuoted { get; set; }
        public string supplierDeliveryTerms { get; set; }
        public string shippingCompanyID { get; set; }
        public string shippingCompName { get; set; }
        public string shippingCompStreet { get; set; }
        public string shippingCompZip { get; set; }
        public string shippingCompTown { get; set; }
        public string shippingCompPhone { get; set; }
        public string shippingCompCountry { get; set; }
        public object history { get; set; }
        public string urgent { get; set; }
        public DateTime createdAt { get; set; }
        public List<ItemGroup> itemGroups { get; set; }
        public string password { get; set; }
        public string sourceXML { get; set; }
        public DateTime modifiedDateTime { get; set; }
        public object sentDateTime { get; set; }
        public int communicationStatus { get; set; }
        public object changedFields { get; set; }
        public int recStatus { get; set; }
        public bool isNewDB { get; set; }
        public string mailReciepientSupplier { get; set; }
        public string mailReciepientBuyer { get; set; }
        public string requestStatusSM { get; set; }
        public bool reminderSent { get; set; }
        public object userIP { get; set; }
        public string visibleFields { get; set; }
    }

    public class OrderItem
    {
        public int id { get; set; }
        public int dsno { get; set; }
        public string itemOrderNumber { get; set; }
        public string itemName { get; set; }
        public string itemName2 { get; set; }
        public object remark { get; set; }
        public string itemText { get; set; }
        public string externalRemark { get; set; }
        public string supplierOrderCode { get; set; }
        public string itemPartNumber { get; set; }
        public string drawingNumber { get; set; }
        public double quantity { get; set; }
        public string unit { get; set; }
        public double price { get; set; }
        public double priceUnit { get; set; }
        public double discount { get; set; }
        public double itemTotal { get; set; }
        public DateTime deliveryUntil { get; set; }
        public string currencyShort { get; set; }
        public bool xmlUpdated { get; set; }
        public object changedFields { get; set; }
        public int recStatus { get; set; }
        public object visibleFields { get; set; }
        public object orderItems { get; set; }
    }

    public class OrderItemGroup
    {
        public int id { get; set; }
        public string dsno { get; set; }
        public string name { get; set; }
        public string name2 { get; set; }
        public string makersName { get; set; }
        public string serialNumber { get; set; }
        public List<OrderItem> orderItems { get; set; }
        public bool xmlUpdated { get; set; }
        public int recStatus { get; set; }
    }

    public class OrderRootObject
    {
        public int id { get; set; }
        public string creatorName { get; set; }
        public string creatorPhone { get; set; }
        public string creatorEmail { get; set; }
        public int dsno { get; set; }
        public string orderCode { get; set; }
        public string orderName { get; set; }
        public string orderRemark { get; set; }
        public DateTime orderedAt { get; set; }
        public string shipName { get; set; }
        public string etaPort { get; set; }
        public DateTime etaDate { get; set; }
        public DateTime deliveryDate { get; set; }
        public DateTime originalDeliveryDate { get; set; }
        public DateTime fetchingDate { get; set; }
        public double discount { get; set; }
        public double additionalCharges { get; set; }
        public double amount { get; set; }
        public string currencyShortName { get; set; }
        public string footerText1 { get; set; }
        public string footerText2 { get; set; }
        public string termsConditions1 { get; set; }
        public string termsConditions2 { get; set; }
        public string techContactSalutation { get; set; }
        public string techContactFirstName { get; set; }
        public string techContactName { get; set; }
        public string techContactPhone { get; set; }
        public string techContactEmail { get; set; }
        public string delAddName { get; set; }
        public string delAddContactSalutation { get; set; }
        public string delAddContactFirstName { get; set; }
        public string delAddContactName { get; set; }
        public string delAddName2 { get; set; }
        public string delAddStreet { get; set; }
        public string delAddZipCode { get; set; }
        public string delAddTown { get; set; }
        public string delAddCountry { get; set; }
        public string delAddPhone { get; set; }
        public string delAddEmail { get; set; }
        public string agentName { get; set; }
        public string agentContactSalutation { get; set; }
        public string agentContactFirstName { get; set; }
        public string agentContactName { get; set; }
        public string agentStreet { get; set; }
        public string agentZipCode { get; set; }
        public string agentTown { get; set; }
        public string agentCountry { get; set; }
        public string agentContactEmail { get; set; }
        public string agentContactPhone { get; set; }
        public string fetchingAddrName { get; set; }
        public string fetchingAddrName2 { get; set; }
        public string fetchingAddrStreet { get; set; }
        public string fetchingAddrZipCode { get; set; }
        public string fetchingAddrTown { get; set; }
        public string fetchingAddrCountry { get; set; }
        public string fetchingAddrEmail { get; set; }
        public string fetchingAddrPhone { get; set; }
        public string invoiceName { get; set; }
        public string invoiceName2 { get; set; }
        public string invoiceStreet { get; set; }
        public string invoiceZipCode { get; set; }
        public string invoiceTown { get; set; }
        public string invoiceCountry { get; set; }
        public string vatNumber { get; set; }
        public string supplierAddrName { get; set; }
        public string supplierContSalutation { get; set; }
        public string supplierContFirstName { get; set; }
        public string supplierContName { get; set; }
        public string supplierAddrStreet { get; set; }
        public string supplierAddrZIP { get; set; }
        public string supplierAddrTown { get; set; }
        public string supplierAddrCountry { get; set; }
        public string supplierContEmail { get; set; }
        public string supplierReference { get; set; }
        public string supplierPaymentTerms { get; set; }
        public string supplierDeliveryTerms { get; set; }
        public string shippingCompanyID { get; set; }
        public string shippingCompName { get; set; }
        public string shippingCompStreet { get; set; }
        public string shippingCompZip { get; set; }
        public string shippingCompTown { get; set; }
        public object shippingCompPhone { get; set; }
        public string shippingCompCountry { get; set; }
        public DateTime createdAt { get; set; }
        public string password { get; set; }
        public string sourceXML { get; set; }
        public DateTime modifiedDateTime { get; set; }
        public DateTime sentDateTime { get; set; }
        public int communicationStatus { get; set; }
        public int supplierOrderState { get; set; }
        public int recStatus { get; set; }
        public bool isNewDB { get; set; }
        public string mailRecipientSupplier { get; set; }
        public string mailRecipientBuyer { get; set; }
        public string orderStatusSM { get; set; }
        public bool reminderSent { get; set; }
        public object userIP { get; set; }
        public string visibleFields { get; set; }
        public List<OrderItemGroup> orderItemGroups { get; set; }
    }
}


#region commented 


        public void SaveQuotation(string[] Arr, RootObject _library, string MTML_QuoteFile, string Token)
        {
            double itemTotal = 0.0;
            string amount = "", msg = "", etaDate = "", delDate = "", QuoteDeadline = "", createdAt = "", modifiedDate = "";
            bool isGrand = false, result = true;
            try
            {
                this.URL = Arr[0] + "//" + Arr[2] + "/" + Arr[3] + "/" + Arr[4] + "/putrequest/" + _library.id;
                _httpWrapper.Referrer = "https://supplierportal.dnvgl.com/";
                _httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.CacheControl);
                _httpWrapper._SetRequestHeaders[HttpRequestHeader.AcceptLanguage] = "en-us";
                _httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");
                _httpWrapper.RequestMethod = "PUT";

                List<string> lstUnits = GetUnitList();
                List<string> lstLongUnits = GetLongUnitList();

                #region add to put dictionary
                if (_library.etaDate != null)
                    etaDate = _library.etaDate.ToString("yyyy-MM-ddTHH:mm:ss");
                if (_library.deliveryDate != null)
                    delDate = _library.deliveryDate.ToString("yyyy-MM-ddTHH:mm:ss");
                if (_library.quotationDeadline != null)
                    QuoteDeadline = _library.quotationDeadline.ToString("yyyy-MM-ddTHH:mm:ss");

                _library.additionalCharges = Convert.ToDouble(FreightCharge) + Convert.ToDouble(PackingCost) + Convert.ToDouble(DepositCost) + Convert.ToDouble(OtherCost);//added by kalpita on 09/09/2019
                //  string postData = "";
                string postData = "{\"id\":" + _library.id + ",\"exportTimeToSm\":\"" + _library.exportTimeToSm.ToString("yyyy-MM-ddTHH:mm:ss") + "\",\"creatorName\":\"" + _library.creatorName +
                 "\",\"creatorPhone\":\"" + _library.creatorPhone + "\",\"creatorEmail\":\"" + _library.creatorEmail + "\",\"dsno\":" + _library.dsno + ",\"requestCode\":\"" + _library.requestCode +
                 "\",\"name\":\"" + _library.name + "\",\"remark\":\"" + _library.remark + "\",\"shipName\":\"" + _library.shipName + "\",\"etaPort\":\"" + _library.etaPort +
                 "\",\"etaDate\":\"" + etaDate + "\",\"deliveryDate\":\"" + delDate + "\",\"quotationDeadline\":\"" + QuoteDeadline +
                 "\",\"currencyShortName\":\"" + Currency + "\",\"discount\":" + AdditionalDiscount + ",\"additionalCharges\":" + _library.additionalCharges;

                foreach (LineItem item in _lineitem)
                {
                    foreach (ItemGroup Items in _library.itemGroups)
                    {
                        foreach (Item lItem in Items.items)
                        {
                            if (lItem.itemOrderNumber == item.OriginatingSystemRef)
                            { itemTotal += item.MonetaryAmount; break; }
                        }
                    }
                }
                itemTotal += _library.additionalCharges;

                if (Convert.ToInt32(itemTotal) == Convert.ToInt32(convert.ToDouble(GrandTotal)))
                { amount = convert.ToString(GrandTotal); isGrand = true; }
                else if (BuyerTotal != "")
                {
                    if (Convert.ToInt32(itemTotal) == Convert.ToInt32(convert.ToDouble(BuyerTotal)))
                    { amount = convert.ToString(BuyerTotal); isGrand = false; }
                }
                if (amount != "")
                {
                    if (DelvDate != "")
                        SupplierComment += " Delivery Date: " + DelvDate;
                    if (PackingCost != null) SupplierComment += " Packing Cost: " + PackingCost;
                    if (FreightCharge != null) SupplierComment += " Freight Cost: " + FreightCharge;
                    if (DateTime.Now != null) DocDate = convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
                    if (_library.createdAt != null) createdAt = convert.ToDateTime(_library.createdAt).ToString("yyyy-MM-ddTHH:mm:ss.ff");
                    if (_library.modifiedDateTime != null) modifiedDate = convert.ToDateTime(_library.modifiedDateTime).ToString("yyyy-MM-ddTHH:mm:ss.fff");

                    postData += ",\"amount\":" + amount + ",\"delAddName\":\"" + _library.delAddName + "\",\"delAddName2\":\"" + _library.delAddName2 + "\",\"delAddStreet\":\"" + _library.delAddStreet +
                                      "\",\"delAddZipCode\":\"" + _library.delAddZipCode + "\",\"delAddTown\":\"" + _library.delAddTown + "\",\"supplierAddrName\":\"" + _library.supplierAddrName +
                                      "\",\"supplierContSalutation\":\"" + _library.supplierContSalutation + "\",\"supplierContFirstName\":\"" + _library.supplierContFirstName +
                                      "\",\"supplierContName\":\"" + _library.supplierContName + "\",\"supplierAddrStreet\":\"" + _library.supplierAddrStreet + "\",\"supplierAddrZIP\":\"" + _library.supplierAddrZIP +
                                      "\",\"supplierAddrTown\":\"" + _library.supplierAddrTown + "\",\"supplierAddrCountry\":\"" + _library.supplierAddrCountry + "\",\"supplierContEmail\":\"" + _library.supplierContEmail +
                                      "\",\"supplierReference\":\"" + AAGRefNo + "\",\"quotationDate\":\"" + DocDate + "\",\"supplierRemark\":\"" + SupplierComment.Replace("\n", "").Replace("\r\n", "") +
                                      "\",\"paymentTerms\":\"" + PayTerms.Replace(":", "-") + "\",\"validity\":\"" + ExpDate + "\",";
                    if (Convert.ToInt32(LeadDays) > 0)
                        postData += "\"deliveryTime\":\"" + LeadDays;
                    else
                        postData += "\"deliveryTime\":\"";
                    postData += "\",\"supplierCountryOrigin\":\"" + _library.supplierCountryOrigin + "\",\"supplierPrincipals\":\"" + _library.supplierPrincipals + "\",\"supplierPartQuoted\":\"" + _library.supplierPartQuoted +
                     "\",\"supplierDeliveryTerms\":\"" + _library.supplierDeliveryTerms + "\",\"shippingCompanyID\":\"" + _library.shippingCompanyID + "\",\"shippingCompName\":\"" + _library.shippingCompName +
                     "\",\"shippingCompStreet\":\"" + _library.shippingCompStreet + "\",\"shippingCompZip\":\"" + _library.shippingCompZip + "\",\"shippingCompTown\":\"" + _library.shippingCompTown +
                     "\",\"shippingCompPhone\":\"" + _library.shippingCompPhone + "\",\"shippingCompCountry\":\"" + _library.shippingCompCountry + "\",\"history\":null" +
                     ",\"urgent\":\"" + _library.urgent + "\",\"createdAt\":\"" + createdAt + "\",";

                    int j = 0;

                    string totalItemData = "";
                    foreach (ItemGroup Items in _library.itemGroups)
                    {
                        string itemData = "";

                        itemData = "\"itemGroups\":[{\"id\":" + Items.id + ",\"dsno\":\"" + Items.dsno + "\",\"name\":\"" + Items.name + "\",\"name2\":\"" + Items.name2 + "\",\"makersName\":\"" + Items.makersName +
                            "\",\"serialNumber\":\"" + Items.serialNumber + "\",";
                        foreach (Item item in Items.items)
                        {
                            j++;
                            LineItem _item = null;
                            foreach (LineItem mitem in _lineitem)
                            {
                                if (item.itemOrderNumber == mitem.OriginatingSystemRef)
                                {
                                    _item = mitem;
                                }
                            }
                            if (_item != null)
                            {

                                if (lstUnits != null && lstUnits.Count > 0 && lstLongUnits != null && lstLongUnits.Count > 0)
                                {
                                    string _price = "", _discount = "";
                                    foreach (PriceDetails _priceDetails in _item.PriceList)
                                    {
                                        if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.GRP) _price = _priceDetails.Value.ToString("0.00");
                                        else if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.DPR) _discount = _priceDetails.Value.ToString("0.00");
                                    }
                                    string remarks = _item.LineItemComment.Value.Replace('|', ',').Replace('(', ' ').Replace(')', ' ').Replace('\"', ' ').Replace('&', ' ').Replace("%", "");

                                    if (j == 1)
                                        itemData += "\"items\":[";
                                    itemData += "{\"id\":" + item.id + ",\"dsno\":" + item.dsno + ",\"itemOrderNumber\":\"" + item.itemOrderNumber + "\",\"name\":\"" + item.name + "\",\"name2\":\"" + item.name2 + "\",\"stockQuantity\":" +
                                   item.stockQuantity + ",\"stockUnit\":\"" + item.stockUnit + "\",\"unitConvFactor\":" + item.unitConvFactor + ",\"quantity\":" + _item.Quantity + ",\"purchaseUnit\":\"" + _item.MeasureUnitQualifier + "\",\"price\":" +
                                _price + ",\"discount\":" + _discount + ",\"partNumber\":\"" + item.partNumber + "\",\"drawingNumber\":\"" + item.drawingNumber + "\",\"companyNumber\":\"" + item.companyNumber + "\",\"remark\":\"" +
                                  remarks + "\",\"itemText\":\"" + item.itemText + "\",\"externalRemark\":\"" + item.externalRemark + "\",";
                                    if (_item.DeleiveryTime == null)
                                        itemData += "\"deliveryDays\":0";
                                    else
                                        itemData += "\"deliveryDays\":" + _item.DeleiveryTime;
                                    itemData += ",\"supplierOrderCode\":\"" + item.supplierOrderCode + "\",\"itemRemark\":\"" +
                                   item.itemRemark + "\",\"exContrClassNumber\":\"" + item.exContrClassNumber + "\",\"exContrMilitCode\":\"" + item.exContrMilitCode + "\",\"exContrCofoCode\":\"" + item.exContrCofoCode + "\",\"exContrPercUsContent\":\"" +
                                   item.exContrPercUsContent + "\",\"exContrCustCode\":\"" + item.exContrCustCode + "\",\"itemTotal\":" + _item.MonetaryAmount + ",\"xmlUpdated\":" + convert.ToString(item.xmlUpdated).ToLower() + ",\"changedFields\":null,\"recStatus\":0" +
                                  ",\"visibleFields\":\"{\\\"ExContrClassNumber\\\":false,\\\"ExContrMilitCode\\\":false,\\\"ExContrCofoCode\\\":false,\\\"ExContrPercUsContent\\\":false,\\\"ExContrCustCode\\\":false}\"},";

                                    if (j == Items.items.Count)
                                    {
                                        totalItemData = itemData.TrimEnd(',') + "],"; j = 0;
                                        itemData = totalItemData + "\"xmlUpdated\":" + Convert.ToString(Items.xmlUpdated).ToLower() + ",\"changedFields\":null,\"recStatus\":0},";
                                        totalItemData = "";
                                    }
                                }
                                else
                                {
                                    LogText = "Unit '" + _item.MeasureUnitQualifier + "' of Item '" + _item.Description + "' is not found in UOM List.";
                                    msg = "LeS-1007:Unable to Save file - UOM Field(s) not present";
                                    result = false;
                                    break;
                                }
                            }
                            else
                            {
                                LogText = "Item not found on site or in mtml with Part No " + item.partNumber + " for refNo " + UCRefNo;
                                msg = "LeS-1007:Unable to Save file - Item Field(s) not present";
                                result = false;
                                break;
                            }
                        }
                        totalItemData = totalItemData + itemData;

                    }
                    //     postData = postData + totalItemData;
                    postData = postData + totalItemData.TrimEnd(',') + "]";


                    postData += ",\"password\":\"" + Token + "\",\"sourceXML\":\"";
                    postData += _library.sourceXML.Replace("\r\n", "\\r\\n").Replace("\"", "\\\"") + "\",\"modifiedDateTime\":\"" + modifiedDate + "\",\"sentDateTime\":null,\"communicationStatus\":" + _library.communicationStatus + ",\"changedFields\":null,\"recStatus\":3,\"isNewDB\":" +//" + _library.recStatus + "
                        Convert.ToString(_library.isNewDB).ToLower() + ",\"mailReciepientSupplier\":\"" + _library.mailReciepientSupplier + "\",\"mailReciepientBuyer\":\"" + _library.mailReciepientBuyer + "\",\"requestStatusSM\":\"" + _library.requestStatusSM +
                        "\",\"reminderSent\":" + Convert.ToString(_library.reminderSent).ToLower() + ",\"userIP\":\"" + _library.userIP + "\",\"visibleFields\":\"{\\\"DeliveryTime\\\":false,\\\"Validity\\\":false,\\\"SupplierRemark\\\":true,\\\"SupplierDeliveryTerms\\\":false}\"}";

                    if (!result) { MoveFileToError(MTML_QuoteFile, msg, "Quote"); result = false; }
                }
                else
                {
                    postData = "";
                    // if (isGrand)
                    //    MoveFileToError(MTML_QuoteFile, "Unable to upload Quote for Ref : '" + UCRefNo + "' due to Total Amount '" + GrandTotal + "' mismatch with item total '" + itemTotal + "'.", this.docType);
                    //else MoveFileToError(MTML_QuoteFile, "Unable to upload Quote for Ref : '" + UCRefNo + "' due to Total Amount '" + BuyerTotal + "' mismatch with item total '" + itemTotal + "'.", this.docType);

                    string total = (isGrand) ? GrandTotal : BuyerTotal;
                    LogText = "Unable to upload Quote for Ref : '" + UCRefNo + "' due to Total Amount '" + total + "' mismatch with item total '" + itemTotal;
                    MoveFileToError(MTML_QuoteFile, "LeS-1008.1:Unable to Save Quote due to amount mismatch for " + UCRefNo, this.docType);
                }

                #endregion

                if (_httpWrapper.PostURL(URL, postData, "", "", ""))
                {
                    if (dctAppSettings["SendQuote"].Trim().ToUpper() != "TRUE")
                    {
                        //MoveFileToBackup(MTML_QuoteFile, "successfully saved input to database."); [testing]
                    }

                    if (isSendMail && !IsDecline)//IsDecline condition added on 08-03-17
                        SendMailNotification(_interchange, "QUOTE", UCRefNo, "SUBMITTED", "Quote '" + UCRefNo + "' submitted successfully.");
                    else if (isSendMail && IsDecline)//IsDecline condition added on 08-03-17
                        SendMailNotification(_interchange, "QUOTE", UCRefNo, "DECLINED", "Quote '" + UCRefNo + "' declined.");

                    if (dctAppSettings["SendQuote"].Trim().ToUpper() == "TRUE")
                    {
                        SendQuote(MTML_QuoteFile, postData, URL);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



//private bool Request_supplierportal_dnvgl_com(out HttpWebResponse response)
//{
//    response = null;

//    try
//    {
//        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://supplierportal.dnvgl.com/api/Requests/putrequest/108979");

//        request.KeepAlive = true;
//        request.Headers.Add("Sec-Fetch-Mode", @"cors");
//        request.Headers.Add("Origin", @"https://supplierportal.dnvgl.com");
//        request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.132 Safari/537.36";
//        request.ContentType = "application/json";
//        request.Accept = "*/*";
//        request.Headers.Add("Sec-Fetch-Site", @"same-origin");
//        request.Referer = "https://supplierportal.dnvgl.com/";
//        request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
//        request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");

//        request.Method = "PUT";

//        string body = @"{""id"":108979,""exportTimeToSm"":""1900-01-01T00:00:00"",""creatorName"":""Ms.Mitzi Ayson"",""creatorPhone"":""+65 6322 3136"",""creatorEmail"":""mitzi@x-pressfeeders.com"",""dsno"":35811,""requestCode"":""WH310-2019-ST0063-B(01)"",""name"":""Chemicals"",""remark"":"""",""shipName"":""WAN HAI 310"",""etaPort"":""SINGAPORE / SINGAPORE"",""etaDate"":""1900-01-01T00:00:00"",""deliveryDate"":""1900-01-01T00:00:00"",""quotationDeadline"":""1900-01-01T00:00:00"",""currencyShortName"":""USD"",""discount"":0,""additionalCharges"":0,""amount"":3528,""delAddName"":"""",""delAddName2"":"""",""delAddStreet"":"""",""delAddZipCode"":"""",""delAddTown"":"""",""supplierAddrName"":""WILHELMSEN SHIPS SERVICE AS"",""supplierContSalutation"":"""",""supplierContFirstName"":"""",""supplierContName"":"""",""supplierAddrStreet"":""186 PANDAN LOOP, SINGAPORE 128376"",""supplierAddrZIP"":"""",""supplierAddrTown"":""SINGAPORE"",""supplierAddrCountry"":"""",""supplierContEmail"":""WSS.CS.SINGAPORE.PRODUCTS@wilhelmsen.com"",""supplierReference"":""S94131"",""quotationDate"":""2019-09-11"",""supplierRemark"":""Thanks for your inquiry. We're pleased to offer our quotation for your consideration.All quoted items are ex-stock subject to prior sales.Delivery Charge (alongside) @  Port SINGAPOREDelivery Lead time: 1-2 working daysSubject to additional delivery charge, hire of barge/crane, custom fee, etc.Please allow another 1-2 working days’ notice for delivery arrangement.All deliveries in Singapore are subject to 7% Goods and Services Tax (GST) under local regulation. This GST could be waived by providing Vessel Stamps on D/O or any other relevant export documentation within 14 days.We look forward to receiving your earliest order confirmation.Best Regards,Tan Wen KaiLinks to product catalog:Product information for line item 1 product 779030 https://wilhelmsen.com/product-catalogue/products/marine-chemicals/water-treatment-chemicals/cooling-water-treatment/cooltreat-elc-anti-freezeProduct information for line item 2 product 571687 https://wilhelmsen.com/product-catalogue/products/marine-chemicals/cleaning-chemicals/cleaning-and-maintenance/disclean-25-ltrProduct information for line item 3 product 575613 https://wilhelmsen.com/product-catalogue/products/marine-chemicals/cleaning-chemicals/cleaning-and-maintenance/aquabreak-px-25-ltrProduct information for line item 4 product 764452 https://wilhelmsen.com/product-catalogue/products/marine-chemicals/cleaning-chemicals/cleaning-and-maintenance/air-cooler-cleaner-25-ltrProduct information for line item 5 product 571356 https://wilhelmsen.com/product-catalogue/products/marine-chemicals/water-treatment-chemicals/cooling-water-treatment/rocor-nb-liquid-25-ltrProduct information for line item 6 product 680843 https://wilhelmsen.com/product-catalogue/products/marine-chemicals/water-treatment-chemicals/cooling-water-treatment/cooltreat-al-25-ltrProduct information for line item 7 product 571364 https://wilhelmsen.com/product-catalogue/products/marine-chemicals/water-treatment-chemicals/water-production-treatment/vaptreat-25ltrProduct information for line item 8 product 698720 https://wilhelmsen.com/product-catalogue/products/marine-chemicals/water-treatment-chemicals/boiler-water-treatment/autotreat---25-ltrProduct information for line item 9 product 589945 https://wilhelmsen.com/product-catalogue/products/marine-chemicals/cleaning-chemicals/gamazyme-cleaners/gamazyme-btc-12-x-1-ltrProduct information for line item 10 product 571240 https://wilhelmsen.com/product-catalogue/products/marine-chemicals/fuel-oil-chemicals/heavy-fuel-oil-treatment/fuelpower-soot-remover-25-kgGeneral Conditions: The provision of products and/or services by Wilhelmsen Ships Services (WSS) is at all times subject to the WSS Standard Terms and Conditions for the Supply of Products and Gas Cylinders. These can be found at:  http://wilhelmsen.com/terms-and-conditions/\r\rItem Added - 11 : FIXED CHARGE LAST MILE DELIVERY | QTY : 1 | UOM : PCS | Price : 0 USD\r Item Total : 3528.00 (including additional items)  Delivery Date: 09/18/2019 Packing Cost: 0 Freight Cost: 0"",""paymentTerms"":""General Conditions- The provision of products and/or services by Wilhelmsen Ships Services (WSS) is "",""validity"":"""",""deliveryTime"":""7"",""supplierCountryOrigin"":"""",""supplierPrincipals"":"""",""supplierPartQuoted"":"""",""supplierDeliveryTerms"":"""",""shippingCompanyID"":""CI-003273"",""shippingCompName"":""Sea Consortium Pte Ltd"",""shippingCompStreet"":""11 Duxton Hill"",""shippingCompZip"":""089595"",""shippingCompTown"":"""",""shippingCompPhone"":""6223 9033"",""shippingCompCountry"":""Singapore"",""history"":null,""urgent"":""N"",""createdAt"":""2019-09-11T09:43:19.19"",""itemGroups"":[{""id"":152237,""dsno"":""0"",""name"":"""",""name2"":"""",""makersName"":"""",""serialNumber"":"""",""items"":[{""id"":998478,""dsno"":333702,""itemOrderNumber"":""002/01263/2019"",""name"":""Cooltreat ELC (Anti-Freeze)"",""name2"":"""",""stockQuantity"":50,""stockUnit"":""LTR"",""unitConvFactor"":1,""quantity"":50,""purchaseUnit"":""LTR"",""price"":6.89,""discount"":0,""partNumber"":""779030"",""drawingNumber"":"""",""companyNumber"":""Emergency Aux Engine Cooling - Scania"",""remark"":"""",""itemText"":"""",""externalRemark"":""Chemicals for Engine Room"",""deliveryDays"":7,""supplierOrderCode"":"""",""itemRemark"":"""",""exContrClassNumber"":"""",""exContrMilitCode"":"""",""exContrCofoCode"":"""",""exContrPercUsContent"":""0"",""exContrCustCode"":"""",""itemTotal"":344.5,""xmlUpdated"":false,""changedFields"":null,""recStatus"":3,""visibleFields"":""{\""ExContrClassNumber\"":false,\""ExContrMilitCode\"":false,\""ExContrCofoCode\"":false,\""ExContrPercUsContent\"":false,\""ExContrCustCode\"":false}""},{""id"":998479,""dsno"":333703,""itemOrderNumber"":""002/01264/2019"",""name"":""Disclean"",""name2"":"""",""stockQuantity"":50,""stockUnit"":""LTR"",""unitConvFactor"":1,""quantity"":50,""purchaseUnit"":""LTR"",""price"":4.22,""discount"":0,""partNumber"":""571687"",""drawingNumber"":"""",""companyNumber"":""Purifier Disc Cleaning"",""remark"":"""",""itemText"":"""",""externalRemark"":""Chemicals for Engine Room"",""deliveryDays"":7,""supplierOrderCode"":"""",""itemRemark"":"""",""exContrClassNumber"":"""",""exContrMilitCode"":"""",""exContrCofoCode"":"""",""exContrPercUsContent"":""0"",""exContrCustCode"":"""",""itemTotal"":211,""xmlUpdated"":false,""changedFields"":null,""recStatus"":3,""visibleFields"":""{\""ExContrClassNumber\"":false,\""ExContrMilitCode\"":false,\""ExContrCofoCode\"":false,\""ExContrPercUsContent\"":false,\""ExContrCustCode\"":false}""},{""id"":998480,""dsno"":333704,""itemOrderNumber"":""002/01265/2019"",""name"":""Aquabreak PX"",""name2"":"""",""stockQuantity"":50,""stockUnit"":""LTR"",""unitConvFactor"":1,""quantity"":50,""purchaseUnit"":""LTR"",""price"":2.39,""discount"":0,""partNumber"":""575613"",""drawingNumber"":"""",""companyNumber"":""General Cleaning"",""remark"":"""",""itemText"":"""",""externalRemark"":""Chemicals for Engine Room"",""deliveryDays"":7,""supplierOrderCode"":"""",""itemRemark"":"""",""exContrClassNumber"":"""",""exContrMilitCode"":"""",""exContrCofoCode"":"""",""exContrPercUsContent"":""0"",""exContrCustCode"":"""",""itemTotal"":119.5,""xmlUpdated"":false,""changedFields"":null,""recStatus"":3,""visibleFields"":""{\""ExContrClassNumber\"":false,\""ExContrMilitCode\"":false,\""ExContrCofoCode\"":false,\""ExContrPercUsContent\"":false,\""ExContrCustCode\"":false}""},{""id"":998481,""dsno"":333705,""itemOrderNumber"":""002/01266/2019"",""name"":""Air Cooler Cleaner"",""name2"":"""",""stockQuantity"":75,""stockUnit"":""LTR"",""unitConvFactor"":1,""quantity"":75,""purchaseUnit"":""LTR"",""price"":2.97,""discount"":0,""partNumber"":""764452"",""drawingNumber"":"""",""companyNumber"":""Air Cooler Cleaning"",""remark"":"""",""itemText"":"""",""externalRemark"":""Chemicals for Engine Room"",""deliveryDays"":7,""supplierOrderCode"":"""",""itemRemark"":"""",""exContrClassNumber"":"""",""exContrMilitCode"":"""",""exContrCofoCode"":"""",""exContrPercUsContent"":""0"",""exContrCustCode"":"""",""itemTotal"":222.75000000000003,""xmlUpdated"":false,""changedFields"":null,""recStatus"":3,""visibleFields"":""{\""ExContrClassNumber\"":false,\""ExContrMilitCode\"":false,\""ExContrCofoCode\"":false,\""ExContrPercUsContent\"":false,\""ExContrCustCode\"":false}""},{""id"":998482,""dsno"":333706,""itemOrderNumber"":""002/01267/2019"",""name"":""Rocor NB Liquid"",""name2"":"""",""stockQuantity"":75,""stockUnit"":""LTR"",""unitConvFactor"":1,""quantity"":75,""purchaseUnit"":""LTR"",""price"":1.91,""discount"":0,""partNumber"":""571356"",""drawingNumber"":"""",""companyNumber"":""Cooling Water Treatment"",""remark"":"""",""itemText"":"""",""externalRemark"":""Chemicals for Engine Room"",""deliveryDays"":7,""supplierOrderCode"":"""",""itemRemark"":"""",""exContrClassNumber"":"""",""exContrMilitCode"":"""",""exContrCofoCode"":"""",""exContrPercUsContent"":""0"",""exContrCustCode"":"""",""itemTotal"":143.25,""xmlUpdated"":false,""changedFields"":null,""recStatus"":3,""visibleFields"":""{\""ExContrClassNumber\"":false,\""ExContrMilitCode\"":false,\""ExContrCofoCode\"":false,\""ExContrPercUsContent\"":false,\""ExContrCustCode\"":false}""},{""id"":998483,""dsno"":333707,""itemOrderNumber"":""002/01268/2019"",""name"":""Cooltreat AL"",""name2"":"""",""stockQuantity"":75,""stockUnit"":""LTR"",""unitConvFactor"":1,""quantity"":75,""purchaseUnit"":""LTR"",""price"":14.73,""discount"":0,""partNumber"":""680843"",""drawingNumber"":"""",""companyNumber"":""Aux Engine Cooling - Scania"",""remark"":"""",""itemText"":"""",""externalRemark"":""Chemicals for Engine Room"",""deliveryDays"":7,""supplierOrderCode"":"""",""itemRemark"":"""",""exContrClassNumber"":"""",""exContrMilitCode"":"""",""exContrCofoCode"":"""",""exContrPercUsContent"":""0"",""exContrCustCode"":"""",""itemTotal"":1104.75,""xmlUpdated"":false,""changedFields"":null,""recStatus"":3,""visibleFields"":""{\""ExContrClassNumber\"":false,\""ExContrMilitCode\"":false,\""ExContrCofoCode\"":false,\""ExContrPercUsContent\"":false,\""ExContrCustCode\"":false}""},{""id"":998484,""dsno"":333708,""itemOrderNumber"":""002/01269/2019"",""name"":""Vaptreat"",""name2"":"""",""stockQuantity"":50,""stockUnit"":""LTR"",""unitConvFactor"":1,""quantity"":50,""purchaseUnit"":""LTR"",""price"":2.53,""discount"":0,""partNumber"":""571364"",""drawingNumber"":"""",""companyNumber"":""Fresh Water Generator Chemical"",""remark"":"""",""itemText"":"""",""externalRemark"":""Chemicals for Engine Room"",""deliveryDays"":7,""supplierOrderCode"":"""",""itemRemark"":"""",""exContrClassNumber"":"""",""exContrMilitCode"":"""",""exContrCofoCode"":"""",""exContrPercUsContent"":""0"",""exContrCustCode"":"""",""itemTotal"":126.49999999999999,""xmlUpdated"":false,""changedFields"":null,""recStatus"":3,""visibleFields"":""{\""ExContrClassNumber\"":false,\""ExContrMilitCode\"":false,\""ExContrCofoCode\"":false,\""ExContrPercUsContent\"":false,\""ExContrCustCode\"":false}""},{""id"":998485,""dsno"":333709,""itemOrderNumber"":""002/01270/2019"",""name"":""Autotreat"",""name2"":"""",""stockQuantity"":75,""stockUnit"":""LTR"",""unitConvFactor"":1,""quantity"":75,""purchaseUnit"":""LTR"",""price"":3.37,""discount"":0,""partNumber"":""698720"",""drawingNumber"":"""",""companyNumber"":""Boiler Water Treatment"",""remark"":"""",""itemText"":"""",""externalRemark"":""Chemicals for Engine Room"",""deliveryDays"":7,""supplierOrderCode"":"""",""itemRemark"":"""",""exContrClassNumber"":"""",""exContrMilitCode"":"""",""exContrCofoCode"":"""",""exContrPercUsContent"":""0"",""exContrCustCode"":"""",""itemTotal"":252.75,""xmlUpdated"":false,""changedFields"":null,""recStatus"":3,""visibleFields"":""{\""ExContrClassNumber\"":false,\""ExContrMilitCode\"":false,\""ExContrCofoCode\"":false,\""ExContrPercUsContent\"":false,\""ExContrCustCode\"":false}""},{""id"":998486,""dsno"":333710,""itemOrderNumber"":""002/01271/2019"",""name"":""Gamazyme BTC"",""name2"":"""",""stockQuantity"":50,""stockUnit"":""PCE"",""unitConvFactor"":1,""quantity"":60,""purchaseUnit"":""LTR"",""price"":4.95,""discount"":0,""partNumber"":""589945"",""drawingNumber"":"""",""companyNumber"":""Cabin Toilet Dosing"",""remark"":"""",""itemText"":"""",""externalRemark"":""Chemicals for Engine Room"",""deliveryDays"":7,""supplierOrderCode"":"""",""itemRemark"":"""",""exContrClassNumber"":"""",""exContrMilitCode"":"""",""exContrCofoCode"":"""",""exContrPercUsContent"":""0"",""exContrCustCode"":"""",""itemTotal"":297,""xmlUpdated"":false,""changedFields"":null,""recStatus"":3,""visibleFields"":""{\""ExContrClassNumber\"":false,\""ExContrMilitCode\"":false,\""ExContrCofoCode\"":false,\""ExContrPercUsContent\"":false,\""ExContrCustCode\"":false}""},{""id"":998487,""dsno"":333711,""itemOrderNumber"":""002/01272/2019"",""name"":""FuelPower Soot Remover - Dry Powder Compound (25KG)"",""name2"":"""",""stockQuantity"":50,""stockUnit"":""KGM"",""unitConvFactor"":1,""quantity"":50,""purchaseUnit"":""KGM"",""price"":14.12,""discount"":0,""partNumber"":""571240"",""drawingNumber"":"""",""companyNumber"":"""",""remark"":"""",""itemText"":"""",""externalRemark"":""Chemicals for Engine Room"",""deliveryDays"":7,""supplierOrderCode"":"""",""itemRemark"":"""",""exContrClassNumber"":"""",""exContrMilitCode"":"""",""exContrCofoCode"":"""",""exContrPercUsContent"":""0"",""exContrCustCode"":"""",""itemTotal"":706,""xmlUpdated"":false,""changedFields"":null,""recStatus"":3,""visibleFields"":""{\""ExContrClassNumber\"":false,\""ExContrMilitCode\"":false,\""ExContrCofoCode\"":false,\""ExContrPercUsContent\"":false,\""ExContrCustCode\"":false}""}],""xmlUpdated"":false,""changedFields"":null,""recStatus"":3}],""password"":""DmXWY2r"",""sourceXML"":""<?xml version=\""1.0\"" encoding=\""utf-8\""?>\r\n<INQQUOTEEXCHANGE xmlns:xsi=\""http://www.w3.org/2001/XMLSchema-instance\"" xmlns:xsd=\""http://www.w3.org/2001/XMLSchema\"">\r\n  <META>\r\n    <EXPORT_TIME_FROM_SM>2019-09-11T01:43:18.8053928Z</EXPORT_TIME_FROM_SM>\r\n    <EXPORT_TIME_TO_SM>0001-01-01T00:00:00</EXPORT_TIME_TO_SM>\r\n    <XSD_VERSION>obsolate</XSD_VERSION>\r\n    <SM_VALID_VERSIONS>obsolate</SM_VALID_VERSIONS>\r\n    <SHIPPING_COMPANY_ID>CI-003273</SHIPPING_COMPANY_ID>\r\n    <SUPPLIER_CHANGE_TIME>0001-01-01T00:00:00</SUPPLIER_CHANGE_TIME>\r\n    <INQUIRY_CREATOR>\r\n      <FULL_NAME>Ms.Mitzi Ayson</FULL_NAME>\r\n      <ABBREV />\r\n      <DEPARTMENT>FMD</DEPARTMENT>\r\n      <PHONE>+65 6322 3136</PHONE>\r\n      <MOBILE>+65 90621257</MOBILE>\r\n      <FAX />\r\n      <EMAIL>mitzi@x-pressfeeders.com</EMAIL>\r\n    </INQUIRY_CREATOR>\r\n    <EVENT_RECIPIENTS>\r\n      <FUNCTIONAL_EXCEPTIONS>\r\n        <MAIL_ADDRESSES>jan.garbe@dnvgl.com</MAIL_ADDRESSES>\r\n      </FUNCTIONAL_EXCEPTIONS>\r\n      <TECHNICAL_EXCEPTIONS />\r\n      <INFORMATION>\r\n        <MAIL_ADDRESSES>jan.garbe@dnvgl.com</MAIL_ADDRESSES>\r\n      </INFORMATION>\r\n    </EVENT_RECIPIENTS>\r\n  </META>\r\n  <SHIPPING_COMPANY>\r\n    <NAME>Sea Consortium Pte Ltd</NAME>\r\n    <NAME2 />\r\n    <SHORT_NAME />\r\n    <CREDITOR_NUMBER />\r\n    <STREET>11 Duxton Hill</STREET>\r\n    <STREET2 />\r\n    <TOWN />\r\n    <ZIP_CODE>089595</ZIP_CODE>\r\n    <COUNTRY>Singapore</COUNTRY>\r\n    <PHONE>6223 9033</PHONE>\r\n    <CELLPHONE />\r\n    <FAX>6225 0786</FAX>\r\n    <EMAIL>fmd.purchasing@x-pressfeeders.com</EMAIL>\r\n    <TELEX />\r\n    <LANGUAGE />\r\n    <TMFORMAT />\r\n    <EDI_NUMBER />\r\n  </SHIPPING_COMPANY>\r\n  <HEADER>\r\n    <DECLINED_FROM_SUPPLIER>false</DECLINED_FROM_SUPPLIER>\r\n    <STATUS>UPDATE</STATUS>\r\n    <DSNO>35811</DSNO>\r\n    <INQUIRY_CODE>WH310-2019-ST0063-B(01)</INQUIRY_CODE>\r\n    <QUOTATION_CODE>WH310-2019-ST0063-B(01)</QUOTATION_CODE>\r\n    <CATEGORY_NAME>STORES</CATEGORY_NAME>\r\n    <CATEGORY_SHORTSIGN>ST</CATEGORY_SHORTSIGN>\r\n    <NAME>Chemicals</NAME>\r\n    <REMARK />\r\n    <DELIVERY_DATE>0001-01-01</DELIVERY_DATE>\r\n    <ETA_DATE>0001-01-01</ETA_DATE>\r\n    <ETA_PORT>SINGAPORE / SINGAPORE</ETA_PORT>\r\n    <URGENT>N</URGENT>\r\n    <QUOTATION_DEADLINE>0001-01-01</QUOTATION_DEADLINE>\r\n    <AMOUNT>0</AMOUNT>\r\n    <DISCOUNT>0</DISCOUNT>\r\n    <ADDITIONAL_CHARGES>0</ADDITIONAL_CHARGES>\r\n    <CURRENCY_NAME />\r\n    <CURRENCY_SHORT_NAME>USD</CURRENCY_SHORT_NAME>\r\n    <CURRENCY_EXCHANGE_RATE>1</CURRENCY_EXCHANGE_RATE>\r\n    <CFGOBJSOBTEMPL1 />\r\n    <CFGOBJSOBTOP1 />\r\n    <CFGOBJSOBTOP2 />\r\n    <CFGOBJSOBFOOTER1 />\r\n    <CFGOBJSOBFOOTER2 />\r\n    <SHIP>\r\n      <DSNO>2</DSNO>\r\n      <NAME>WAN HAI 310</NAME>\r\n      <CALL_SIGN>9V2444</CALL_SIGN>\r\n      <COMPANY_NUMBER />\r\n      <IMO_NUMBER>9348924</IMO_NUMBER>\r\n      <SHORT_SIGN>WH310</SHORT_SIGN>\r\n      <SHORT_NUMBER />\r\n    </SHIP>\r\n    <DELIVERY_ADDRESS>\r\n      <NAME />\r\n      <NAME2 />\r\n      <SHORT_NAME />\r\n      <CREDITOR_NUMBER />\r\n      <STREET />\r\n      <STREET2 />\r\n      <TOWN />\r\n      <ZIP_CODE />\r\n      <COUNTRY />\r\n      <PHONE />\r\n      <CELLPHONE />\r\n      <FAX />\r\n      <EMAIL />\r\n      <TELEX />\r\n      <LANGUAGE />\r\n      <TMFORMAT />\r\n      <EDI_NUMBER />\r\n    </DELIVERY_ADDRESS>\r\n    <SUPPLIER>\r\n      <DSNO>35811</DSNO>\r\n      <REMARK />\r\n      <PAYMENT_TERMS />\r\n      <DELIVERY_TERMS />\r\n      <DELIVERY_TIME />\r\n      <PART_QUOTED />\r\n      <PRINCIPALS />\r\n      <VALIDITY />\r\n      <COUNTRY_OF_ORIGIN />\r\n      <SUPPLIER_REFERENCE />\r\n      <QUOTATION_DATE>0001-01-01</QUOTATION_DATE>\r\n      <ADDRESS>\r\n        <NAME>WILHELMSEN SHIPS SERVICE AS</NAME>\r\n        <NAME2 />\r\n        <SHORT_NAME>WILHELMSEN</SHORT_NAME>\r\n        <CREDITOR_NUMBER>CFW007</CREDITOR_NUMBER>\r\n        <STREET>186 PANDAN LOOP, SINGAPORE 128376</STREET>\r\n        <STREET2 />\r\n        <TOWN>SINGAPORE</TOWN>\r\n        <ZIP_CODE />\r\n        <COUNTRY />\r\n        <PHONE />\r\n        <CELLPHONE />\r\n        <FAX />\r\n        <EMAIL>WSS.CS.SINGAPORE.PRODUCTS@wilhelmsen.com</EMAIL>\r\n        <TELEX />\r\n        <LANGUAGE />\r\n        <TMFORMAT />\r\n        <EDI_NUMBER />\r\n      </ADDRESS>\r\n      <CONTACT>\r\n        <FIRST_NAME />\r\n        <NAME />\r\n        <POSITION />\r\n        <SALUTATION />\r\n        <PHONE />\r\n        <MOBILE />\r\n        <FAX />\r\n        <EMAIL>WSS.CS.SINGAPORE.PRODUCTS@wilhelmsen.com</EMAIL>\r\n      </CONTACT>\r\n    </SUPPLIER>\r\n    <ITEMGROUPS>\r\n      <GROUP>\r\n        <ID>0</ID>\r\n        <NAME />\r\n        <NAME2 />\r\n        <MAKERS_NAME />\r\n        <SERIAL_NO />\r\n        <ITEMS>\r\n          <DSNO>333702</DSNO>\r\n          <PRICE>0</PRICE>\r\n          <DISCOUNT>0</DISCOUNT>\r\n          <QUANTITY>50</QUANTITY>\r\n          <PURCHASE_UNIT>LTR</PURCHASE_UNIT>\r\n          <STOCK_QUANTITY>50</STOCK_QUANTITY>\r\n          <STOCK_UNIT>LTR</STOCK_UNIT>\r\n          <UNIT_CONVERTION_FACTOR>1</UNIT_CONVERTION_FACTOR>\r\n          <DELIVERY_DAYS>0</DELIVERY_DAYS>\r\n          <REMARK />\r\n          <SUPPLIER_ORDER_CODE />\r\n          <NAME>Cooltreat ELC (Anti-Freeze)</NAME>\r\n          <NAME2 />\r\n          <ARRAY />\r\n          <IS_FREE_ITEM>N</IS_FREE_ITEM>\r\n          <EAN />\r\n          <COMPANY_NUMBER>Emergency Aux Engine Cooling - Scania</COMPANY_NUMBER>\r\n          <ITEM_NUMBER>006.01.011</ITEM_NUMBER>\r\n          <DRAWING_NUMBER />\r\n          <PART_NUMBER>779030</PART_NUMBER>\r\n          <ITEM_ORDER_ITEM_NUMBER>002/01263/2019</ITEM_ORDER_ITEM_NUMBER>\r\n          <REQUEST_CODE>WH310-2019-ST0063-B</REQUEST_CODE>\r\n          <REQUEST_NAME>Chemicals</REQUEST_NAME>\r\n          <ITEM_REMARK />\r\n          <ITEM_TEXT />\r\n          <ITEM_INTERNAL_REMARK>Chemicals for Engine Room</ITEM_INTERNAL_REMARK>\r\n          <ITEM_EXTERNAL_REMARK>Chemicals for Engine Room</ITEM_EXTERNAL_REMARK>\r\n          <EXPORT_CONTROL>\r\n            <CUSTOMS_CODE Visible=\""false\"" />\r\n            <COFO_ISO_CODE Visible=\""false\"" />\r\n            <COFO_SHORT_NAME Visible=\""false\"" />\r\n            <COFO_NAME Visible=\""false\"" />\r\n            <CLASSIFIC_NUMBER Visible=\""false\"" />\r\n            <MILITARY_CODE Visible=\""false\"" />\r\n            <PERC_US_CONTENT Visible=\""false\"">0</PERC_US_CONTENT>\r\n          </EXPORT_CONTROL>\r\n          <DANGEROUS_CLASS>\r\n            <CLASS />\r\n            <CLASS_NUMBER>0</CLASS_NUMBER>\r\n            <NAME />\r\n            <UN_NUMBER>0</UN_NUMBER>\r\n          </DANGEROUS_CLASS>\r\n          <RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n          <RFQR_ITEM_REFERENCE>\r\n            <ITEM_RF_NAME />\r\n            <ITEM_RF_TYPE_NAME />\r\n            <ITEM_RF_TYPE_NAME2 />\r\n            <ITEM_RF_ELEMENT_NUMBER />\r\n            <ITEM_RF_SERIAL_NO />\r\n            <ITEM_RF_PART_NUMBER />\r\n            <ITEM_RF_ITEM_NUMBER />\r\n            <ITEM_RF_DRAWING_NUMBER />\r\n            <ITEM_RF_COMPANY_NUMBER />\r\n            <ITEM_RF_REMARK />\r\n            <ITEM_RF_TYPE_REMARK />\r\n            <ITEM_RF_TYPE_TEXT />\r\n            <ITEM_RF_CONSTRUCTION_YEAR>0</ITEM_RF_CONSTRUCTION_YEAR>\r\n            <ITEM_RF_GUARANTEE_EXPIR>0001-01-01T00:00:00</ITEM_RF_GUARANTEE_EXPIR>\r\n            <ITEM_RF_READ_FIT_BY>0</ITEM_RF_READ_FIT_BY>\r\n            <ITEM_RF_READ_FIT_BY_DATE>0001-01-01</ITEM_RF_READ_FIT_BY_DATE>\r\n            <ITEM_RF_READ_CUR>0</ITEM_RF_READ_CUR>\r\n            <ITEM_RF_READ_CUR_DATE>0001-01-01</ITEM_RF_READ_CUR_DATE>\r\n            <ITEM_RF_READ_UNIT />\r\n          </RFQR_ITEM_REFERENCE>\r\n          <RFQR_ITEM_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_MANUFACTURER>\r\n        </ITEMS>\r\n        <ITEMS>\r\n          <DSNO>333703</DSNO>\r\n          <PRICE>0</PRICE>\r\n          <DISCOUNT>0</DISCOUNT>\r\n          <QUANTITY>50</QUANTITY>\r\n          <PURCHASE_UNIT>LTR</PURCHASE_UNIT>\r\n          <STOCK_QUANTITY>50</STOCK_QUANTITY>\r\n          <STOCK_UNIT>LTR</STOCK_UNIT>\r\n          <UNIT_CONVERTION_FACTOR>1</UNIT_CONVERTION_FACTOR>\r\n          <DELIVERY_DAYS>0</DELIVERY_DAYS>\r\n          <REMARK />\r\n          <SUPPLIER_ORDER_CODE />\r\n          <NAME>Disclean</NAME>\r\n          <NAME2 />\r\n          <ARRAY />\r\n          <IS_FREE_ITEM>N</IS_FREE_ITEM>\r\n          <EAN />\r\n          <COMPANY_NUMBER>Purifier Disc Cleaning</COMPANY_NUMBER>\r\n          <ITEM_NUMBER>006.01.003</ITEM_NUMBER>\r\n          <DRAWING_NUMBER />\r\n          <PART_NUMBER>571687</PART_NUMBER>\r\n          <ITEM_ORDER_ITEM_NUMBER>002/01264/2019</ITEM_ORDER_ITEM_NUMBER>\r\n          <REQUEST_CODE>WH310-2019-ST0063-B</REQUEST_CODE>\r\n          <REQUEST_NAME>Chemicals</REQUEST_NAME>\r\n          <ITEM_REMARK />\r\n          <ITEM_TEXT />\r\n          <ITEM_INTERNAL_REMARK>Chemicals for Engine Room</ITEM_INTERNAL_REMARK>\r\n          <ITEM_EXTERNAL_REMARK>Chemicals for Engine Room</ITEM_EXTERNAL_REMARK>\r\n          <EXPORT_CONTROL>\r\n            <CUSTOMS_CODE Visible=\""false\"" />\r\n            <COFO_ISO_CODE Visible=\""false\"" />\r\n            <COFO_SHORT_NAME Visible=\""false\"" />\r\n            <COFO_NAME Visible=\""false\"" />\r\n            <CLASSIFIC_NUMBER Visible=\""false\"" />\r\n            <MILITARY_CODE Visible=\""false\"" />\r\n            <PERC_US_CONTENT Visible=\""false\"">0</PERC_US_CONTENT>\r\n          </EXPORT_CONTROL>\r\n          <DANGEROUS_CLASS>\r\n            <CLASS />\r\n            <CLASS_NUMBER>0</CLASS_NUMBER>\r\n            <NAME />\r\n            <UN_NUMBER>0</UN_NUMBER>\r\n          </DANGEROUS_CLASS>\r\n          <RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n          <RFQR_ITEM_REFERENCE>\r\n            <ITEM_RF_NAME />\r\n            <ITEM_RF_TYPE_NAME />\r\n            <ITEM_RF_TYPE_NAME2 />\r\n            <ITEM_RF_ELEMENT_NUMBER />\r\n            <ITEM_RF_SERIAL_NO />\r\n            <ITEM_RF_PART_NUMBER />\r\n            <ITEM_RF_ITEM_NUMBER />\r\n            <ITEM_RF_DRAWING_NUMBER />\r\n            <ITEM_RF_COMPANY_NUMBER />\r\n            <ITEM_RF_REMARK />\r\n            <ITEM_RF_TYPE_REMARK />\r\n            <ITEM_RF_TYPE_TEXT />\r\n            <ITEM_RF_CONSTRUCTION_YEAR>0</ITEM_RF_CONSTRUCTION_YEAR>\r\n            <ITEM_RF_GUARANTEE_EXPIR>0001-01-01T00:00:00</ITEM_RF_GUARANTEE_EXPIR>\r\n            <ITEM_RF_READ_FIT_BY>0</ITEM_RF_READ_FIT_BY>\r\n            <ITEM_RF_READ_FIT_BY_DATE>0001-01-01</ITEM_RF_READ_FIT_BY_DATE>\r\n            <ITEM_RF_READ_CUR>0</ITEM_RF_READ_CUR>\r\n            <ITEM_RF_READ_CUR_DATE>0001-01-01</ITEM_RF_READ_CUR_DATE>\r\n            <ITEM_RF_READ_UNIT />\r\n          </RFQR_ITEM_REFERENCE>\r\n          <RFQR_ITEM_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_MANUFACTURER>\r\n        </ITEMS>\r\n        <ITEMS>\r\n          <DSNO>333704</DSNO>\r\n          <PRICE>0</PRICE>\r\n          <DISCOUNT>0</DISCOUNT>\r\n          <QUANTITY>50</QUANTITY>\r\n          <PURCHASE_UNIT>LTR</PURCHASE_UNIT>\r\n          <STOCK_QUANTITY>50</STOCK_QUANTITY>\r\n          <STOCK_UNIT>LTR</STOCK_UNIT>\r\n          <UNIT_CONVERTION_FACTOR>1</UNIT_CONVERTION_FACTOR>\r\n          <DELIVERY_DAYS>0</DELIVERY_DAYS>\r\n          <REMARK />\r\n          <SUPPLIER_ORDER_CODE />\r\n          <NAME>Aquabreak PX</NAME>\r\n          <NAME2 />\r\n          <ARRAY />\r\n          <IS_FREE_ITEM>N</IS_FREE_ITEM>\r\n          <EAN />\r\n          <COMPANY_NUMBER>General Cleaning</COMPANY_NUMBER>\r\n          <ITEM_NUMBER>006.01.005</ITEM_NUMBER>\r\n          <DRAWING_NUMBER />\r\n          <PART_NUMBER>575613</PART_NUMBER>\r\n          <ITEM_ORDER_ITEM_NUMBER>002/01265/2019</ITEM_ORDER_ITEM_NUMBER>\r\n          <REQUEST_CODE>WH310-2019-ST0063-B</REQUEST_CODE>\r\n          <REQUEST_NAME>Chemicals</REQUEST_NAME>\r\n          <ITEM_REMARK />\r\n          <ITEM_TEXT />\r\n          <ITEM_INTERNAL_REMARK>Chemicals for Engine Room</ITEM_INTERNAL_REMARK>\r\n          <ITEM_EXTERNAL_REMARK>Chemicals for Engine Room</ITEM_EXTERNAL_REMARK>\r\n          <EXPORT_CONTROL>\r\n            <CUSTOMS_CODE Visible=\""false\"" />\r\n            <COFO_ISO_CODE Visible=\""false\"" />\r\n            <COFO_SHORT_NAME Visible=\""false\"" />\r\n            <COFO_NAME Visible=\""false\"" />\r\n            <CLASSIFIC_NUMBER Visible=\""false\"" />\r\n            <MILITARY_CODE Visible=\""false\"" />\r\n            <PERC_US_CONTENT Visible=\""false\"">0</PERC_US_CONTENT>\r\n          </EXPORT_CONTROL>\r\n          <DANGEROUS_CLASS>\r\n            <CLASS />\r\n            <CLASS_NUMBER>0</CLASS_NUMBER>\r\n            <NAME />\r\n            <UN_NUMBER>0</UN_NUMBER>\r\n          </DANGEROUS_CLASS>\r\n          <RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n          <RFQR_ITEM_REFERENCE>\r\n            <ITEM_RF_NAME />\r\n            <ITEM_RF_TYPE_NAME />\r\n            <ITEM_RF_TYPE_NAME2 />\r\n            <ITEM_RF_ELEMENT_NUMBER />\r\n            <ITEM_RF_SERIAL_NO />\r\n            <ITEM_RF_PART_NUMBER />\r\n            <ITEM_RF_ITEM_NUMBER />\r\n            <ITEM_RF_DRAWING_NUMBER />\r\n            <ITEM_RF_COMPANY_NUMBER />\r\n            <ITEM_RF_REMARK />\r\n            <ITEM_RF_TYPE_REMARK />\r\n            <ITEM_RF_TYPE_TEXT />\r\n            <ITEM_RF_CONSTRUCTION_YEAR>0</ITEM_RF_CONSTRUCTION_YEAR>\r\n            <ITEM_RF_GUARANTEE_EXPIR>0001-01-01T00:00:00</ITEM_RF_GUARANTEE_EXPIR>\r\n            <ITEM_RF_READ_FIT_BY>0</ITEM_RF_READ_FIT_BY>\r\n            <ITEM_RF_READ_FIT_BY_DATE>0001-01-01</ITEM_RF_READ_FIT_BY_DATE>\r\n            <ITEM_RF_READ_CUR>0</ITEM_RF_READ_CUR>\r\n            <ITEM_RF_READ_CUR_DATE>0001-01-01</ITEM_RF_READ_CUR_DATE>\r\n            <ITEM_RF_READ_UNIT />\r\n          </RFQR_ITEM_REFERENCE>\r\n          <RFQR_ITEM_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_MANUFACTURER>\r\n        </ITEMS>\r\n        <ITEMS>\r\n          <DSNO>333705</DSNO>\r\n          <PRICE>0</PRICE>\r\n          <DISCOUNT>0</DISCOUNT>\r\n          <QUANTITY>75</QUANTITY>\r\n          <PURCHASE_UNIT>LTR</PURCHASE_UNIT>\r\n          <STOCK_QUANTITY>75</STOCK_QUANTITY>\r\n          <STOCK_UNIT>LTR</STOCK_UNIT>\r\n          <UNIT_CONVERTION_FACTOR>1</UNIT_CONVERTION_FACTOR>\r\n          <DELIVERY_DAYS>0</DELIVERY_DAYS>\r\n          <REMARK />\r\n          <SUPPLIER_ORDER_CODE />\r\n          <NAME>Air Cooler Cleaner</NAME>\r\n          <NAME2 />\r\n          <ARRAY />\r\n          <IS_FREE_ITEM>N</IS_FREE_ITEM>\r\n          <EAN />\r\n          <COMPANY_NUMBER>Air Cooler Cleaning</COMPANY_NUMBER>\r\n          <ITEM_NUMBER>006.01.008</ITEM_NUMBER>\r\n          <DRAWING_NUMBER />\r\n          <PART_NUMBER>764452</PART_NUMBER>\r\n          <ITEM_ORDER_ITEM_NUMBER>002/01266/2019</ITEM_ORDER_ITEM_NUMBER>\r\n          <REQUEST_CODE>WH310-2019-ST0063-B</REQUEST_CODE>\r\n          <REQUEST_NAME>Chemicals</REQUEST_NAME>\r\n          <ITEM_REMARK />\r\n          <ITEM_TEXT />\r\n          <ITEM_INTERNAL_REMARK>Chemicals for Engine Room</ITEM_INTERNAL_REMARK>\r\n          <ITEM_EXTERNAL_REMARK>Chemicals for Engine Room</ITEM_EXTERNAL_REMARK>\r\n          <EXPORT_CONTROL>\r\n            <CUSTOMS_CODE Visible=\""false\"" />\r\n            <COFO_ISO_CODE Visible=\""false\"" />\r\n            <COFO_SHORT_NAME Visible=\""false\"" />\r\n            <COFO_NAME Visible=\""false\"" />\r\n            <CLASSIFIC_NUMBER Visible=\""false\"" />\r\n            <MILITARY_CODE Visible=\""false\"" />\r\n            <PERC_US_CONTENT Visible=\""false\"">0</PERC_US_CONTENT>\r\n          </EXPORT_CONTROL>\r\n          <DANGEROUS_CLASS>\r\n            <CLASS />\r\n            <CLASS_NUMBER>0</CLASS_NUMBER>\r\n            <NAME />\r\n            <UN_NUMBER>0</UN_NUMBER>\r\n          </DANGEROUS_CLASS>\r\n          <RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n          <RFQR_ITEM_REFERENCE>\r\n            <ITEM_RF_NAME />\r\n            <ITEM_RF_TYPE_NAME />\r\n            <ITEM_RF_TYPE_NAME2 />\r\n            <ITEM_RF_ELEMENT_NUMBER />\r\n            <ITEM_RF_SERIAL_NO />\r\n            <ITEM_RF_PART_NUMBER />\r\n            <ITEM_RF_ITEM_NUMBER />\r\n            <ITEM_RF_DRAWING_NUMBER />\r\n            <ITEM_RF_COMPANY_NUMBER />\r\n            <ITEM_RF_REMARK />\r\n            <ITEM_RF_TYPE_REMARK />\r\n            <ITEM_RF_TYPE_TEXT />\r\n            <ITEM_RF_CONSTRUCTION_YEAR>0</ITEM_RF_CONSTRUCTION_YEAR>\r\n            <ITEM_RF_GUARANTEE_EXPIR>0001-01-01T00:00:00</ITEM_RF_GUARANTEE_EXPIR>\r\n            <ITEM_RF_READ_FIT_BY>0</ITEM_RF_READ_FIT_BY>\r\n            <ITEM_RF_READ_FIT_BY_DATE>0001-01-01</ITEM_RF_READ_FIT_BY_DATE>\r\n            <ITEM_RF_READ_CUR>0</ITEM_RF_READ_CUR>\r\n            <ITEM_RF_READ_CUR_DATE>0001-01-01</ITEM_RF_READ_CUR_DATE>\r\n            <ITEM_RF_READ_UNIT />\r\n          </RFQR_ITEM_REFERENCE>\r\n          <RFQR_ITEM_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_MANUFACTURER>\r\n        </ITEMS>\r\n        <ITEMS>\r\n          <DSNO>333706</DSNO>\r\n          <PRICE>0</PRICE>\r\n          <DISCOUNT>0</DISCOUNT>\r\n          <QUANTITY>75</QUANTITY>\r\n          <PURCHASE_UNIT>LTR</PURCHASE_UNIT>\r\n          <STOCK_QUANTITY>75</STOCK_QUANTITY>\r\n          <STOCK_UNIT>LTR</STOCK_UNIT>\r\n          <UNIT_CONVERTION_FACTOR>1</UNIT_CONVERTION_FACTOR>\r\n          <DELIVERY_DAYS>0</DELIVERY_DAYS>\r\n          <REMARK />\r\n          <SUPPLIER_ORDER_CODE />\r\n          <NAME>Rocor NB Liquid</NAME>\r\n          <NAME2 />\r\n          <ARRAY />\r\n          <IS_FREE_ITEM>N</IS_FREE_ITEM>\r\n          <EAN />\r\n          <COMPANY_NUMBER>Cooling Water Treatment</COMPANY_NUMBER>\r\n          <ITEM_NUMBER>006.01.009</ITEM_NUMBER>\r\n          <DRAWING_NUMBER />\r\n          <PART_NUMBER>571356</PART_NUMBER>\r\n          <ITEM_ORDER_ITEM_NUMBER>002/01267/2019</ITEM_ORDER_ITEM_NUMBER>\r\n          <REQUEST_CODE>WH310-2019-ST0063-B</REQUEST_CODE>\r\n          <REQUEST_NAME>Chemicals</REQUEST_NAME>\r\n          <ITEM_REMARK />\r\n          <ITEM_TEXT />\r\n          <ITEM_INTERNAL_REMARK>Chemicals for Engine Room</ITEM_INTERNAL_REMARK>\r\n          <ITEM_EXTERNAL_REMARK>Chemicals for Engine Room</ITEM_EXTERNAL_REMARK>\r\n          <EXPORT_CONTROL>\r\n            <CUSTOMS_CODE Visible=\""false\"" />\r\n            <COFO_ISO_CODE Visible=\""false\"" />\r\n            <COFO_SHORT_NAME Visible=\""false\"" />\r\n            <COFO_NAME Visible=\""false\"" />\r\n            <CLASSIFIC_NUMBER Visible=\""false\"" />\r\n            <MILITARY_CODE Visible=\""false\"" />\r\n            <PERC_US_CONTENT Visible=\""false\"">0</PERC_US_CONTENT>\r\n          </EXPORT_CONTROL>\r\n          <DANGEROUS_CLASS>\r\n            <CLASS />\r\n            <CLASS_NUMBER>0</CLASS_NUMBER>\r\n            <NAME />\r\n            <UN_NUMBER>0</UN_NUMBER>\r\n          </DANGEROUS_CLASS>\r\n          <RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n          <RFQR_ITEM_REFERENCE>\r\n            <ITEM_RF_NAME />\r\n            <ITEM_RF_TYPE_NAME />\r\n            <ITEM_RF_TYPE_NAME2 />\r\n            <ITEM_RF_ELEMENT_NUMBER />\r\n            <ITEM_RF_SERIAL_NO />\r\n            <ITEM_RF_PART_NUMBER />\r\n            <ITEM_RF_ITEM_NUMBER />\r\n            <ITEM_RF_DRAWING_NUMBER />\r\n            <ITEM_RF_COMPANY_NUMBER />\r\n            <ITEM_RF_REMARK />\r\n            <ITEM_RF_TYPE_REMARK />\r\n            <ITEM_RF_TYPE_TEXT />\r\n            <ITEM_RF_CONSTRUCTION_YEAR>0</ITEM_RF_CONSTRUCTION_YEAR>\r\n            <ITEM_RF_GUARANTEE_EXPIR>0001-01-01T00:00:00</ITEM_RF_GUARANTEE_EXPIR>\r\n            <ITEM_RF_READ_FIT_BY>0</ITEM_RF_READ_FIT_BY>\r\n            <ITEM_RF_READ_FIT_BY_DATE>0001-01-01</ITEM_RF_READ_FIT_BY_DATE>\r\n            <ITEM_RF_READ_CUR>0</ITEM_RF_READ_CUR>\r\n            <ITEM_RF_READ_CUR_DATE>0001-01-01</ITEM_RF_READ_CUR_DATE>\r\n            <ITEM_RF_READ_UNIT />\r\n          </RFQR_ITEM_REFERENCE>\r\n          <RFQR_ITEM_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_MANUFACTURER>\r\n        </ITEMS>\r\n        <ITEMS>\r\n          <DSNO>333707</DSNO>\r\n          <PRICE>0</PRICE>\r\n          <DISCOUNT>0</DISCOUNT>\r\n          <QUANTITY>75</QUANTITY>\r\n          <PURCHASE_UNIT>LTR</PURCHASE_UNIT>\r\n          <STOCK_QUANTITY>75</STOCK_QUANTITY>\r\n          <STOCK_UNIT>LTR</STOCK_UNIT>\r\n          <UNIT_CONVERTION_FACTOR>1</UNIT_CONVERTION_FACTOR>\r\n          <DELIVERY_DAYS>0</DELIVERY_DAYS>\r\n          <REMARK />\r\n          <SUPPLIER_ORDER_CODE />\r\n          <NAME>Cooltreat AL</NAME>\r\n          <NAME2 />\r\n          <ARRAY />\r\n          <IS_FREE_ITEM>N</IS_FREE_ITEM>\r\n          <EAN />\r\n          <COMPANY_NUMBER>Aux Engine Cooling - Scania</COMPANY_NUMBER>\r\n          <ITEM_NUMBER>006.01.010</ITEM_NUMBER>\r\n          <DRAWING_NUMBER />\r\n          <PART_NUMBER>680843</PART_NUMBER>\r\n          <ITEM_ORDER_ITEM_NUMBER>002/01268/2019</ITEM_ORDER_ITEM_NUMBER>\r\n          <REQUEST_CODE>WH310-2019-ST0063-B</REQUEST_CODE>\r\n          <REQUEST_NAME>Chemicals</REQUEST_NAME>\r\n          <ITEM_REMARK />\r\n          <ITEM_TEXT />\r\n          <ITEM_INTERNAL_REMARK>Chemicals for Engine Room</ITEM_INTERNAL_REMARK>\r\n          <ITEM_EXTERNAL_REMARK>Chemicals for Engine Room</ITEM_EXTERNAL_REMARK>\r\n          <EXPORT_CONTROL>\r\n            <CUSTOMS_CODE Visible=\""false\"" />\r\n            <COFO_ISO_CODE Visible=\""false\"" />\r\n            <COFO_SHORT_NAME Visible=\""false\"" />\r\n            <COFO_NAME Visible=\""false\"" />\r\n            <CLASSIFIC_NUMBER Visible=\""false\"" />\r\n            <MILITARY_CODE Visible=\""false\"" />\r\n            <PERC_US_CONTENT Visible=\""false\"">0</PERC_US_CONTENT>\r\n          </EXPORT_CONTROL>\r\n          <DANGEROUS_CLASS>\r\n            <CLASS />\r\n            <CLASS_NUMBER>0</CLASS_NUMBER>\r\n            <NAME />\r\n            <UN_NUMBER>0</UN_NUMBER>\r\n          </DANGEROUS_CLASS>\r\n          <RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n          <RFQR_ITEM_REFERENCE>\r\n            <ITEM_RF_NAME />\r\n            <ITEM_RF_TYPE_NAME />\r\n            <ITEM_RF_TYPE_NAME2 />\r\n            <ITEM_RF_ELEMENT_NUMBER />\r\n            <ITEM_RF_SERIAL_NO />\r\n            <ITEM_RF_PART_NUMBER />\r\n            <ITEM_RF_ITEM_NUMBER />\r\n            <ITEM_RF_DRAWING_NUMBER />\r\n            <ITEM_RF_COMPANY_NUMBER />\r\n            <ITEM_RF_REMARK />\r\n            <ITEM_RF_TYPE_REMARK />\r\n            <ITEM_RF_TYPE_TEXT />\r\n            <ITEM_RF_CONSTRUCTION_YEAR>0</ITEM_RF_CONSTRUCTION_YEAR>\r\n            <ITEM_RF_GUARANTEE_EXPIR>0001-01-01T00:00:00</ITEM_RF_GUARANTEE_EXPIR>\r\n            <ITEM_RF_READ_FIT_BY>0</ITEM_RF_READ_FIT_BY>\r\n            <ITEM_RF_READ_FIT_BY_DATE>0001-01-01</ITEM_RF_READ_FIT_BY_DATE>\r\n            <ITEM_RF_READ_CUR>0</ITEM_RF_READ_CUR>\r\n            <ITEM_RF_READ_CUR_DATE>0001-01-01</ITEM_RF_READ_CUR_DATE>\r\n            <ITEM_RF_READ_UNIT />\r\n          </RFQR_ITEM_REFERENCE>\r\n          <RFQR_ITEM_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_MANUFACTURER>\r\n        </ITEMS>\r\n        <ITEMS>\r\n          <DSNO>333708</DSNO>\r\n          <PRICE>0</PRICE>\r\n          <DISCOUNT>0</DISCOUNT>\r\n          <QUANTITY>50</QUANTITY>\r\n          <PURCHASE_UNIT>LTR</PURCHASE_UNIT>\r\n          <STOCK_QUANTITY>50</STOCK_QUANTITY>\r\n          <STOCK_UNIT>LTR</STOCK_UNIT>\r\n          <UNIT_CONVERTION_FACTOR>1</UNIT_CONVERTION_FACTOR>\r\n          <DELIVERY_DAYS>0</DELIVERY_DAYS>\r\n          <REMARK />\r\n          <SUPPLIER_ORDER_CODE />\r\n          <NAME>Vaptreat</NAME>\r\n          <NAME2 />\r\n          <ARRAY />\r\n          <IS_FREE_ITEM>N</IS_FREE_ITEM>\r\n          <EAN />\r\n          <COMPANY_NUMBER>Fresh Water Generator Chemical</COMPANY_NUMBER>\r\n          <ITEM_NUMBER>006.01.012</ITEM_NUMBER>\r\n          <DRAWING_NUMBER />\r\n          <PART_NUMBER>571364</PART_NUMBER>\r\n          <ITEM_ORDER_ITEM_NUMBER>002/01269/2019</ITEM_ORDER_ITEM_NUMBER>\r\n          <REQUEST_CODE>WH310-2019-ST0063-B</REQUEST_CODE>\r\n          <REQUEST_NAME>Chemicals</REQUEST_NAME>\r\n          <ITEM_REMARK />\r\n          <ITEM_TEXT />\r\n          <ITEM_INTERNAL_REMARK>Chemicals for Engine Room</ITEM_INTERNAL_REMARK>\r\n          <ITEM_EXTERNAL_REMARK>Chemicals for Engine Room</ITEM_EXTERNAL_REMARK>\r\n          <EXPORT_CONTROL>\r\n            <CUSTOMS_CODE Visible=\""false\"" />\r\n            <COFO_ISO_CODE Visible=\""false\"" />\r\n            <COFO_SHORT_NAME Visible=\""false\"" />\r\n            <COFO_NAME Visible=\""false\"" />\r\n            <CLASSIFIC_NUMBER Visible=\""false\"" />\r\n            <MILITARY_CODE Visible=\""false\"" />\r\n            <PERC_US_CONTENT Visible=\""false\"">0</PERC_US_CONTENT>\r\n          </EXPORT_CONTROL>\r\n          <DANGEROUS_CLASS>\r\n            <CLASS />\r\n            <CLASS_NUMBER>0</CLASS_NUMBER>\r\n            <NAME />\r\n            <UN_NUMBER>0</UN_NUMBER>\r\n          </DANGEROUS_CLASS>\r\n          <RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n          <RFQR_ITEM_REFERENCE>\r\n            <ITEM_RF_NAME />\r\n            <ITEM_RF_TYPE_NAME />\r\n            <ITEM_RF_TYPE_NAME2 />\r\n            <ITEM_RF_ELEMENT_NUMBER />\r\n            <ITEM_RF_SERIAL_NO />\r\n            <ITEM_RF_PART_NUMBER />\r\n            <ITEM_RF_ITEM_NUMBER />\r\n            <ITEM_RF_DRAWING_NUMBER />\r\n            <ITEM_RF_COMPANY_NUMBER />\r\n            <ITEM_RF_REMARK />\r\n            <ITEM_RF_TYPE_REMARK />\r\n            <ITEM_RF_TYPE_TEXT />\r\n            <ITEM_RF_CONSTRUCTION_YEAR>0</ITEM_RF_CONSTRUCTION_YEAR>\r\n            <ITEM_RF_GUARANTEE_EXPIR>0001-01-01T00:00:00</ITEM_RF_GUARANTEE_EXPIR>\r\n            <ITEM_RF_READ_FIT_BY>0</ITEM_RF_READ_FIT_BY>\r\n            <ITEM_RF_READ_FIT_BY_DATE>0001-01-01</ITEM_RF_READ_FIT_BY_DATE>\r\n            <ITEM_RF_READ_CUR>0</ITEM_RF_READ_CUR>\r\n            <ITEM_RF_READ_CUR_DATE>0001-01-01</ITEM_RF_READ_CUR_DATE>\r\n            <ITEM_RF_READ_UNIT />\r\n          </RFQR_ITEM_REFERENCE>\r\n          <RFQR_ITEM_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_MANUFACTURER>\r\n        </ITEMS>\r\n        <ITEMS>\r\n          <DSNO>333709</DSNO>\r\n          <PRICE>0</PRICE>\r\n          <DISCOUNT>0</DISCOUNT>\r\n          <QUANTITY>75</QUANTITY>\r\n          <PURCHASE_UNIT>LTR</PURCHASE_UNIT>\r\n          <STOCK_QUANTITY>75</STOCK_QUANTITY>\r\n          <STOCK_UNIT>LTR</STOCK_UNIT>\r\n          <UNIT_CONVERTION_FACTOR>1</UNIT_CONVERTION_FACTOR>\r\n          <DELIVERY_DAYS>0</DELIVERY_DAYS>\r\n          <REMARK />\r\n          <SUPPLIER_ORDER_CODE />\r\n          <NAME>Autotreat</NAME>\r\n          <NAME2 />\r\n          <ARRAY />\r\n          <IS_FREE_ITEM>N</IS_FREE_ITEM>\r\n          <EAN />\r\n          <COMPANY_NUMBER>Boiler Water Treatment</COMPANY_NUMBER>\r\n          <ITEM_NUMBER>006.01.014</ITEM_NUMBER>\r\n          <DRAWING_NUMBER />\r\n          <PART_NUMBER>698720</PART_NUMBER>\r\n          <ITEM_ORDER_ITEM_NUMBER>002/01270/2019</ITEM_ORDER_ITEM_NUMBER>\r\n          <REQUEST_CODE>WH310-2019-ST0063-B</REQUEST_CODE>\r\n          <REQUEST_NAME>Chemicals</REQUEST_NAME>\r\n          <ITEM_REMARK />\r\n          <ITEM_TEXT />\r\n          <ITEM_INTERNAL_REMARK>Chemicals for Engine Room</ITEM_INTERNAL_REMARK>\r\n          <ITEM_EXTERNAL_REMARK>Chemicals for Engine Room</ITEM_EXTERNAL_REMARK>\r\n          <EXPORT_CONTROL>\r\n            <CUSTOMS_CODE Visible=\""false\"" />\r\n            <COFO_ISO_CODE Visible=\""false\"" />\r\n            <COFO_SHORT_NAME Visible=\""false\"" />\r\n            <COFO_NAME Visible=\""false\"" />\r\n            <CLASSIFIC_NUMBER Visible=\""false\"" />\r\n            <MILITARY_CODE Visible=\""false\"" />\r\n            <PERC_US_CONTENT Visible=\""false\"">0</PERC_US_CONTENT>\r\n          </EXPORT_CONTROL>\r\n          <DANGEROUS_CLASS>\r\n            <CLASS />\r\n            <CLASS_NUMBER>0</CLASS_NUMBER>\r\n            <NAME />\r\n            <UN_NUMBER>0</UN_NUMBER>\r\n          </DANGEROUS_CLASS>\r\n          <RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n          <RFQR_ITEM_REFERENCE>\r\n            <ITEM_RF_NAME />\r\n            <ITEM_RF_TYPE_NAME />\r\n            <ITEM_RF_TYPE_NAME2 />\r\n            <ITEM_RF_ELEMENT_NUMBER />\r\n            <ITEM_RF_SERIAL_NO />\r\n            <ITEM_RF_PART_NUMBER />\r\n            <ITEM_RF_ITEM_NUMBER />\r\n            <ITEM_RF_DRAWING_NUMBER />\r\n            <ITEM_RF_COMPANY_NUMBER />\r\n            <ITEM_RF_REMARK />\r\n            <ITEM_RF_TYPE_REMARK />\r\n            <ITEM_RF_TYPE_TEXT />\r\n            <ITEM_RF_CONSTRUCTION_YEAR>0</ITEM_RF_CONSTRUCTION_YEAR>\r\n            <ITEM_RF_GUARANTEE_EXPIR>0001-01-01T00:00:00</ITEM_RF_GUARANTEE_EXPIR>\r\n            <ITEM_RF_READ_FIT_BY>0</ITEM_RF_READ_FIT_BY>\r\n            <ITEM_RF_READ_FIT_BY_DATE>0001-01-01</ITEM_RF_READ_FIT_BY_DATE>\r\n            <ITEM_RF_READ_CUR>0</ITEM_RF_READ_CUR>\r\n            <ITEM_RF_READ_CUR_DATE>0001-01-01</ITEM_RF_READ_CUR_DATE>\r\n            <ITEM_RF_READ_UNIT />\r\n          </RFQR_ITEM_REFERENCE>\r\n          <RFQR_ITEM_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_MANUFACTURER>\r\n        </ITEMS>\r\n        <ITEMS>\r\n          <DSNO>333710</DSNO>\r\n          <PRICE>0</PRICE>\r\n          <DISCOUNT>0</DISCOUNT>\r\n          <QUANTITY>50</QUANTITY>\r\n          <PURCHASE_UNIT>PCE</PURCHASE_UNIT>\r\n          <STOCK_QUANTITY>50</STOCK_QUANTITY>\r\n          <STOCK_UNIT>PCE</STOCK_UNIT>\r\n          <UNIT_CONVERTION_FACTOR>1</UNIT_CONVERTION_FACTOR>\r\n          <DELIVERY_DAYS>0</DELIVERY_DAYS>\r\n          <REMARK />\r\n          <SUPPLIER_ORDER_CODE />\r\n          <NAME>Gamazyme BTC</NAME>\r\n          <NAME2 />\r\n          <ARRAY />\r\n          <IS_FREE_ITEM>N</IS_FREE_ITEM>\r\n          <EAN />\r\n          <COMPANY_NUMBER>Cabin Toilet Dosing</COMPANY_NUMBER>\r\n          <ITEM_NUMBER>006.01.016</ITEM_NUMBER>\r\n          <DRAWING_NUMBER />\r\n          <PART_NUMBER>589945</PART_NUMBER>\r\n          <ITEM_ORDER_ITEM_NUMBER>002/01271/2019</ITEM_ORDER_ITEM_NUMBER>\r\n          <REQUEST_CODE>WH310-2019-ST0063-B</REQUEST_CODE>\r\n          <REQUEST_NAME>Chemicals</REQUEST_NAME>\r\n          <ITEM_REMARK />\r\n          <ITEM_TEXT />\r\n          <ITEM_INTERNAL_REMARK>Chemicals for Engine Room</ITEM_INTERNAL_REMARK>\r\n          <ITEM_EXTERNAL_REMARK>Chemicals for Engine Room</ITEM_EXTERNAL_REMARK>\r\n          <EXPORT_CONTROL>\r\n            <CUSTOMS_CODE Visible=\""false\"" />\r\n            <COFO_ISO_CODE Visible=\""false\"" />\r\n            <COFO_SHORT_NAME Visible=\""false\"" />\r\n            <COFO_NAME Visible=\""false\"" />\r\n            <CLASSIFIC_NUMBER Visible=\""false\"" />\r\n            <MILITARY_CODE Visible=\""false\"" />\r\n            <PERC_US_CONTENT Visible=\""false\"">0</PERC_US_CONTENT>\r\n          </EXPORT_CONTROL>\r\n          <DANGEROUS_CLASS>\r\n            <CLASS />\r\n            <CLASS_NUMBER>0</CLASS_NUMBER>\r\n            <NAME />\r\n            <UN_NUMBER>0</UN_NUMBER>\r\n          </DANGEROUS_CLASS>\r\n          <RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n          <RFQR_ITEM_REFERENCE>\r\n            <ITEM_RF_NAME />\r\n            <ITEM_RF_TYPE_NAME />\r\n            <ITEM_RF_TYPE_NAME2 />\r\n            <ITEM_RF_ELEMENT_NUMBER />\r\n            <ITEM_RF_SERIAL_NO />\r\n            <ITEM_RF_PART_NUMBER />\r\n            <ITEM_RF_ITEM_NUMBER />\r\n            <ITEM_RF_DRAWING_NUMBER />\r\n            <ITEM_RF_COMPANY_NUMBER />\r\n            <ITEM_RF_REMARK />\r\n            <ITEM_RF_TYPE_REMARK />\r\n            <ITEM_RF_TYPE_TEXT />\r\n            <ITEM_RF_CONSTRUCTION_YEAR>0</ITEM_RF_CONSTRUCTION_YEAR>\r\n            <ITEM_RF_GUARANTEE_EXPIR>0001-01-01T00:00:00</ITEM_RF_GUARANTEE_EXPIR>\r\n            <ITEM_RF_READ_FIT_BY>0</ITEM_RF_READ_FIT_BY>\r\n            <ITEM_RF_READ_FIT_BY_DATE>0001-01-01</ITEM_RF_READ_FIT_BY_DATE>\r\n            <ITEM_RF_READ_CUR>0</ITEM_RF_READ_CUR>\r\n            <ITEM_RF_READ_CUR_DATE>0001-01-01</ITEM_RF_READ_CUR_DATE>\r\n            <ITEM_RF_READ_UNIT />\r\n          </RFQR_ITEM_REFERENCE>\r\n          <RFQR_ITEM_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_MANUFACTURER>\r\n        </ITEMS>\r\n        <ITEMS>\r\n          <DSNO>333711</DSNO>\r\n          <PRICE>0</PRICE>\r\n          <DISCOUNT>0</DISCOUNT>\r\n          <QUANTITY>50</QUANTITY>\r\n          <PURCHASE_UNIT>KGM</PURCHASE_UNIT>\r\n          <STOCK_QUANTITY>50</STOCK_QUANTITY>\r\n          <STOCK_UNIT>KGM</STOCK_UNIT>\r\n          <UNIT_CONVERTION_FACTOR>1</UNIT_CONVERTION_FACTOR>\r\n          <DELIVERY_DAYS>0</DELIVERY_DAYS>\r\n          <REMARK />\r\n          <SUPPLIER_ORDER_CODE />\r\n          <NAME>FuelPower Soot Remover - Dry Powder Compound (25KG)</NAME>\r\n          <NAME2 />\r\n          <ARRAY />\r\n          <IS_FREE_ITEM>N</IS_FREE_ITEM>\r\n          <EAN />\r\n          <COMPANY_NUMBER />\r\n          <ITEM_NUMBER>006.01.044</ITEM_NUMBER>\r\n          <DRAWING_NUMBER />\r\n          <PART_NUMBER>571240</PART_NUMBER>\r\n          <ITEM_ORDER_ITEM_NUMBER>002/01272/2019</ITEM_ORDER_ITEM_NUMBER>\r\n          <REQUEST_CODE>WH310-2019-ST0063-B</REQUEST_CODE>\r\n          <REQUEST_NAME>Chemicals</REQUEST_NAME>\r\n          <ITEM_REMARK />\r\n          <ITEM_TEXT />\r\n          <ITEM_INTERNAL_REMARK>Chemicals for Engine Room</ITEM_INTERNAL_REMARK>\r\n          <ITEM_EXTERNAL_REMARK>Chemicals for Engine Room</ITEM_EXTERNAL_REMARK>\r\n          <EXPORT_CONTROL>\r\n            <CUSTOMS_CODE Visible=\""false\"" />\r\n            <COFO_ISO_CODE Visible=\""false\"" />\r\n            <COFO_SHORT_NAME Visible=\""false\"" />\r\n            <COFO_NAME Visible=\""false\"" />\r\n            <CLASSIFIC_NUMBER Visible=\""false\"" />\r\n            <MILITARY_CODE Visible=\""false\"" />\r\n            <PERC_US_CONTENT Visible=\""false\"">0</PERC_US_CONTENT>\r\n          </EXPORT_CONTROL>\r\n          <DANGEROUS_CLASS>\r\n            <CLASS />\r\n            <CLASS_NUMBER>0</CLASS_NUMBER>\r\n            <NAME />\r\n            <UN_NUMBER>0</UN_NUMBER>\r\n          </DANGEROUS_CLASS>\r\n          <RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_REFERENCE_MANUFACTURER>\r\n          <RFQR_ITEM_REFERENCE>\r\n            <ITEM_RF_NAME />\r\n            <ITEM_RF_TYPE_NAME />\r\n            <ITEM_RF_TYPE_NAME2 />\r\n            <ITEM_RF_ELEMENT_NUMBER />\r\n            <ITEM_RF_SERIAL_NO />\r\n            <ITEM_RF_PART_NUMBER />\r\n            <ITEM_RF_ITEM_NUMBER />\r\n            <ITEM_RF_DRAWING_NUMBER />\r\n            <ITEM_RF_COMPANY_NUMBER />\r\n            <ITEM_RF_REMARK />\r\n            <ITEM_RF_TYPE_REMARK />\r\n            <ITEM_RF_TYPE_TEXT />\r\n            <ITEM_RF_CONSTRUCTION_YEAR>0</ITEM_RF_CONSTRUCTION_YEAR>\r\n            <ITEM_RF_GUARANTEE_EXPIR>0001-01-01T00:00:00</ITEM_RF_GUARANTEE_EXPIR>\r\n            <ITEM_RF_READ_FIT_BY>0</ITEM_RF_READ_FIT_BY>\r\n            <ITEM_RF_READ_FIT_BY_DATE>0001-01-01</ITEM_RF_READ_FIT_BY_DATE>\r\n            <ITEM_RF_READ_CUR>0</ITEM_RF_READ_CUR>\r\n            <ITEM_RF_READ_CUR_DATE>0001-01-01</ITEM_RF_READ_CUR_DATE>\r\n            <ITEM_RF_READ_UNIT />\r\n          </RFQR_ITEM_REFERENCE>\r\n          <RFQR_ITEM_MANUFACTURER>\r\n            <NAME />\r\n            <NAME2 />\r\n            <SHORT_NAME />\r\n            <CREDITOR_NUMBER />\r\n            <STREET />\r\n            <STREET2 />\r\n            <TOWN />\r\n            <ZIP_CODE />\r\n            <COUNTRY />\r\n            <PHONE />\r\n            <CELLPHONE />\r\n            <FAX />\r\n            <EMAIL />\r\n            <TELEX />\r\n            <LANGUAGE />\r\n            <TMFORMAT />\r\n            <EDI_NUMBER />\r\n          </RFQR_ITEM_MANUFACTURER>\r\n        </ITEMS>\r\n      </GROUP>\r\n    </ITEMGROUPS>\r\n  </HEADER>\r\n</INQQUOTEEXCHANGE>"",""modifiedDateTime"":""2019-09-11T09:43:19.227"",""sentDateTime"":""2019-09-11 10:49:48"",""communicationStatus"":2,""changedFields"":null,""recStatus"":3,""isNewDB"":false,""mailReciepientSupplier"":""WSS.CS.SINGAPORE.PRODUCTS@wilhelmsen.com"",""mailReciepientBuyer"":""mitzi@x-pressfeeders.com"",""requestStatusSM"":""UPDATE"",""reminderSent"":false,""userIP"":""123.201.54.149"",""visibleFields"":""{\""DeliveryTime\"":false,\""Validity\"":false,\""SupplierRemark\"":true,\""SupplierDeliveryTerms\"":false}""}";
//        byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
//        request.ContentLength = postBytes.Length;
//        Stream stream = request.GetRequestStream();
//        stream.Write(postBytes, 0, postBytes.Length);
//        stream.Close();

//        response = (HttpWebResponse)request.GetResponse();
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




#endregion