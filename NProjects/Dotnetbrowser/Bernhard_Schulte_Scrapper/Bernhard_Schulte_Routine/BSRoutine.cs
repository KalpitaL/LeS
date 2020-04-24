using DotNetBrowserWrapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using DotNetBrowser.DOM;
using System.Windows.Forms;
using MTML.GENERATOR;
using DotNetBrowser;
using DotNetBrowser.Events;
using Aspose.Cells;

namespace Bernhard_Schulte_Routine
{
    public class BSRoutine : PluginInterface.IWrapperPlugin
    {
        //public NetBrowser _netWrapper = new NetBrowser();
        public NetBrowser _netWrapper;
        int nCount = 0;//added
        PageVariables pagevar = new PageVariables();
        string siteURL = "", Username = "", Password = "", LogPath = "", UnameElement = "", PswdElement = "", currentDate = "", sDoneFile = "", AuditPath = "",
        Quote_UploadPath = "", MTML_Quote_UploadPath = "", ProcessorName = "", ScreenShotPath = "", MailFilePath = "", Mail_Template = "", FROM_EMAIL_ID = "", MAIL_BCC = "",
        DocType = "", UCRefNo = "", AAGRefNo = "", LesRecordID = "", MessageNumber = "", LeadDays = "", Currency = "", BuyerName = "", BuyerPhone = "", BuyerEmail = "", BuyerFax = "",
        supplierName = "", supplierPhone = "", supplierEmail = "", DtDelvDate = "", dtExpDate = "", supplierFax = "", VesselName = "", PortName = "", PortCode = "", SupplierComment = "", PayTerms = "", DeliveryTerms = "", PackingCost = "", FreightCharge = "",
        GrandTotal = "", Allowance = "", TotalLineItemsAmount = "", eSupp_SuppAddCode = "", buyer_Link_code = "", VendorStatus = "", MsgNumber = "", MsgRefNumber = "", Attachment_Inbox = "", Rejected_msg = "", Price_Validity_Date = "", SearchTextInXLS = "",
        BuyerTotal = "",cFreightcell="";
        int Retry = 0, Maxtry = 2, waitPeriod = 0, IsAltItemAllowed = 0, IsPriceAveraged = 0, IsUOMChanged = 0, count = 0,cFilter=0,cMax=2;//,FirstCount=0;
        bool IsLoad = false;// NotValidStatus = false,

        bool IsDecline = false, IsQuote = false,IsDraftQuote=false,IsSubmitQuote=false;
        List<string> xmlFiles = new List<string>();
        public MTMLInterchange _interchange { get; set; }
        public LineItemCollection _lineitem = null;
        public string BuyerCode { get; set; }
        public string SupplierCode { get; set; }
        public string[] BuyerNames, Buyer_Supplier_LinkID;

        public BSRoutine()
        {
            _netWrapper = new NetBrowser();
            //_netWrapper.browser.CacheStorage.ClearCache();//added by Kalpita on 19/12/2019
        }

        public void LoadAppSettings()
        {
            siteURL = ConfigurationManager.AppSettings["SITE_URL"].Trim();
            Username = ConfigurationManager.AppSettings["USERNAME"].Trim();
            Password = ConfigurationManager.AppSettings["PASSWORD"].Trim();
            LogPath = ConfigurationManager.AppSettings["LOGPATH"].Trim();
            AuditPath = ConfigurationManager.AppSettings["AUDITPATH"].Trim();
            Quote_UploadPath = ConfigurationManager.AppSettings["QUOTE_UPLOADPATH"];
            MTML_Quote_UploadPath = ConfigurationManager.AppSettings["MTML_QUOTE_UPLOADPATH"];
            Attachment_Inbox = ConfigurationManager.AppSettings["ATTACHMENT_INBOX"];
            waitPeriod = Convert.ToInt32(ConfigurationManager.AppSettings["WAITPERIOD"].Trim());
            currentDate = Convert.ToString(ConfigurationManager.AppSettings["RFQRECEIVED_DATE"].Trim());
            ProcessorName = ConfigurationManager.AppSettings["PROCESSOR_NAME"].Trim();
            BuyerNames = ConfigurationManager.AppSettings["BUYER_NAME"].Trim().Split('|');
            Buyer_Supplier_LinkID = ConfigurationManager.AppSettings["BUYER_SUPPLIER_LINK"].Trim().Split('|');
            eSupp_SuppAddCode = ConfigurationManager.AppSettings["SUPPLIER"].Trim();
            Rejected_msg = ConfigurationManager.AppSettings["REJECTED_MSG"].Trim();
            Price_Validity_Date = ConfigurationManager.AppSettings["PRICE_VALIDITY_DAYS"].Trim();
            SearchTextInXLS = ConfigurationManager.AppSettings["SEARCH_TEXT_XLSQUOTE"].Trim();
            if (currentDate == null || currentDate == "")
                currentDate = DateTime.Today.ToString("dd-MMM-yyyy");
            else
                currentDate = Convert.ToDateTime(currentDate).ToString("dd-MMM-yyyy");
            if (LogPath == "") LogPath = AppDomain.CurrentDomain.BaseDirectory + "Log";
            if (AuditPath == "") AuditPath = AppDomain.CurrentDomain.BaseDirectory + "Audit";
            if (Quote_UploadPath == "") Quote_UploadPath = AppDomain.CurrentDomain.BaseDirectory + "Quote_Upload";
            if (MTML_Quote_UploadPath == "") MTML_Quote_UploadPath = AppDomain.CurrentDomain.BaseDirectory + "MTML_Quote_Upload";
            if (Attachment_Inbox == "") Attachment_Inbox = AppDomain.CurrentDomain.BaseDirectory + "ATTACHMENT_INBOX";
            ScreenShotPath = AppDomain.CurrentDomain.BaseDirectory + "ScreenShots";
            UnameElement = "UserId";
            PswdElement = "Password";

            if (!Directory.Exists(Quote_UploadPath + "\\Backup")) Directory.CreateDirectory(Quote_UploadPath + "\\Backup");
            if (!Directory.Exists(Quote_UploadPath + "\\Error")) Directory.CreateDirectory(Quote_UploadPath + "\\Error");
            if (!Directory.Exists(MTML_Quote_UploadPath + "\\Backup")) Directory.CreateDirectory(MTML_Quote_UploadPath + "\\Backup");
            if (!Directory.Exists(MTML_Quote_UploadPath + "\\Error")) Directory.CreateDirectory(MTML_Quote_UploadPath + "\\Error");
            if (!Directory.Exists(ScreenShotPath)) Directory.CreateDirectory(ScreenShotPath);
            _netWrapper.LogPath = LogPath;
            _netWrapper.WaitPeriod = waitPeriod;
            IsDraftQuote = Convert.ToBoolean(ConfigurationManager.AppSettings["DRAFTQUOTE"]);//added by Kalpita on 19/12/2019
            IsSubmitQuote = Convert.ToBoolean(ConfigurationManager.AppSettings["SUBMITQUOTE"]);
            cFreightcell = Convert.ToString(ConfigurationManager.AppSettings["FREIGHT_CELL"].Trim());//added by Kalpita on 07/01/2020
        }

        #region Login
        public bool LogIn()
        {
            bool isLoggedIn = false;
            bool loaded;
            try
            {
                LoadAppSettings();
               // _netWrapper.LogPath = LogPath;// commented by Kalpita on 18/11/2019
               // _netWrapper.WaitPeriod = waitPeriod;
                if (Retry == 0)
                {
                    // _netWrapper.LogText = Environment.NewLine;// commented by Kalpita on 18/11/2019
                    //_netWrapper.LogText = "Bernhard Schulte Processor Started.";
                }

                if (Retry == 0)
                    loaded = _netWrapper.LoadUrl(siteURL, pagevar.divloginCon);
                else
                {
                    if (checkLoginError())
                        loaded = _netWrapper.LoadUrl(siteURL, pagevar.divloginCon);
                    else
                    {
                        loaded = _netWrapper.LoadUrl("https://paleconnect.bs-shipmanagement.com/Enquiry/Index"); if (loaded) isLoggedIn = true; Thread.Sleep(60000);
                    }
                }

                if (loaded)
                {
                    if (Retry == 0 || (Retry > 0 && checkLoginError()))
                    {
                        string[] strURL = new string[1];
                        strURL[0] = "https://paleconnect.bs-shipmanagement.com/Dashboard|https://paleconnect.bs-shipmanagement.com/Enquiry/Index";
                        DOMElement Login = _netWrapper.GetElementByType("input", "LOG IN", "value");//GetElementByType("input", "submit");
                       // string a = Login.GetAttribute("value");
                        Thread.Sleep(200);
                        _netWrapper.SetValuebyName(UnameElement, Username);
                        #region added on 27-3-2017
                        DOMElement _eleUsername = _netWrapper.GetElementByName(UnameElement);
                        if (_eleUsername != null)
                        {
                            string _val = ((DOMInputElement)_eleUsername).Value;
                            if (_val == "")
                                _netWrapper.SetValuebyName(UnameElement, Username);
                        }
                        #endregion
                        _netWrapper.SetValuebyName(PswdElement, Password);
                        isLoggedIn = _netWrapper.ClickElementbyID(Login, strURL, "", pagevar.divEnquiryGrid, false, false);
                    }

                    Thread.Sleep(2000);
                    if (isLoggedIn && Retry == 0)
                    { _netWrapper.LogText = "Login Successfully."; }
                    else if (isLoggedIn && Retry > 0)
                    { _netWrapper.LogText = "Login Successfully."; }
                    else if (Retry > Maxtry)
                    {
                        SetLoginError();
                    }
                    else
                    {
                        Retry++;
                        _netWrapper.LogText = "Login Retry.";
                        isLoggedIn = LogIn();
                    }
                }
                else
                {
                    if (Retry <= Maxtry)
                    {
                        Retry++;
                        _netWrapper.LogText = "Login Retry.";
                        isLoggedIn = LogIn();
                    }
                }
            }
            catch (Exception ex)
            {
                string sFile = ScreenShotPath + "\\BS_Login" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + eSupp_SuppAddCode + ".pdf";
                _netWrapper.PrintPDF(sFile, false);
                _netWrapper.LogText = "Exception while LogIn : " + ex.GetBaseException().ToString();
            }
            return isLoggedIn;
        }

        public void SetLoginError()
        {
            string msg = "";
            DOMElement eleErrMsg = _netWrapper.GetElementbyClass(pagevar.divLoginErrClass);
            if (eleErrMsg != null) msg = eleErrMsg.InnerText;
            if (msg == "") { DOMElement _grid = _netWrapper.GetElementbyId(pagevar.divEnquiryGrid); if (_grid == null) msg = "enquiry-grid not found"; }
            _netWrapper.LogText = "Login failed due to " + msg;
            string sFile = ScreenShotPath + "\\BS_Login" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + eSupp_SuppAddCode + ".pdf";
            _netWrapper.PrintPDF(sFile, false);
        }

        public bool checkLoginError()
        {
            DOMElement eleErrMsg = _netWrapper.GetElementbyClass(pagevar.divLoginErrClass);
            if (eleErrMsg != null)
                return true;
            else return false;
        }
        #endregion

        #region RFQ

        /*Get RFQ Downloaded list from RFQ_Downloaded text file */
        public List<string> GetProcessedItems(PluginInterface.eActions eAction)
        {
            List<string> slProcessedItems = new List<string>();
            switch (eAction)
            {
                case PluginInterface.eActions.RFQ: sDoneFile = AppDomain.CurrentDomain.BaseDirectory + "RFQ_Downloaded.txt"; break;
                default: break;
            }
            if (File.Exists(sDoneFile))
            {
                string[] _Items = File.ReadAllLines(sDoneFile);
                slProcessedItems.AddRange(_Items.ToList());
            }
            return slProcessedItems;
        }

        public bool ProcessRFQ(List<string> slProcessRFQ, string DownloadPath)
        {
            bool result = false;
            try
            {
                _netWrapper.LogText = "RFQ processing started.";
                //  result = LoadRFQTable(slProcessRFQ, DownloadPath);
                result = ProcessRFQTable(slProcessRFQ, DownloadPath);
                #region added on 23-05-2017(for datetime issue, some rfqs may not downloaded,so download 2 days rfqs)
                if (Convert.ToString(ConfigurationManager.AppSettings["RFQRECEIVED_DATE"].Trim()) == null || Convert.ToString(ConfigurationManager.AppSettings["RFQRECEIVED_DATE"].Trim()) == "")
                {
                    currentDate = Convert.ToDateTime(currentDate).AddDays(-1).ToString("dd-MMM-yyyy");
                    GetProcessedItems(PluginInterface.eActions.RFQ);
                    Clear_SearchBox();
                    _netWrapper.LogText = "Filter RFQ Table with previous date started.";
                    DOMInputElement _inputSearch = (DOMInputElement)_netWrapper.GetElementByType("input", "search");
                    for (int j = 0; j < currentDate.Length; j++)
                    {
                        if (checkChar_AlphaNumeric(currentDate[j]))
                        {
                            SetAlphaNumeric(currentDate[j]);
                            Thread.Sleep(300);
                        }
                        else
                        {
                            KeyParams paramers = new KeyParams(VirtualKeyCode.OEM_MINUS, '-');
                            _netWrapper.browser.KeyDown(paramers);
                            _netWrapper.browser.KeyUp(paramers);
                            Thread.Sleep(300);
                        }
                    }
                    string dateText = _inputSearch.Value;
                    if (dateText != "")
                        _netWrapper.LogText = "Filter RFQ Table completed.";
                    result = LoadRFQTable(slProcessRFQ, DownloadPath);
                }
                #endregion
                _netWrapper.LogText = "RFQ processing stopped.";
            }
            catch (Exception ex)
            {
                string sFile = ScreenShotPath + "\\BS_RFQ" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + eSupp_SuppAddCode + ".pdf";
                _netWrapper.PrintPDF(sFile, false);
                _netWrapper.LogText = "Exception while processing RFQ : " + ex.GetBaseException().ToString();
            }
            return result;
        }

        public void GetPendingRFQTab()
        {
            DOMElement enquiryClass = _netWrapper.GetElementbyClass(pagevar.ulEnquiryClass);
            if (enquiryClass != null)
            {
                var anchors = enquiryClass.GetElementsByTagName("a");
                foreach (DOMElement a in anchors)
                {
                    if (a.InnerText == "Pending")
                    {
                        a.Click();
                        if (IsQuote)
                        {
                            VendorStatus = a.InnerText;
                        }
                    }
                }
            }
            else _netWrapper.LogText = "Pending tab is null.";
        }

        public void Filter_RFQ_By_Date()
        {
            _netWrapper.LogText = "Filter RFQ Table started.";
            DOMInputElement _inputSearch = (DOMInputElement)_netWrapper.GetElementByType("input", "search");
            _inputSearch.Focus();
            Thread.Sleep(500);
            //for (int i = 0; i <= 8; i++)
            //{
            //    KeyParams paramersa = new KeyParams(VirtualKeyCode.TAB, ' ');
            //    _netWrapper.browser.KeyDown(paramersa);
            //    _netWrapper.browser.KeyUp(paramersa);
            //    Thread.Sleep(500);
            //}

            for (int j = 0; j < currentDate.Length; j++)
            {
                if (checkChar_AlphaNumeric(currentDate[j]))
                {
                    SetAlphaNumeric(currentDate[j]);
                    Thread.Sleep(300);
                }
                else
                {
                    KeyParams paramers = new KeyParams(VirtualKeyCode.OEM_MINUS, '-');
                    _netWrapper.browser.KeyDown(paramers);
                    _netWrapper.browser.KeyUp(paramers);
                    Thread.Sleep(300);
                }
            }
            string dateText = _inputSearch.Value;
            if (dateText != "")
                _netWrapper.LogText = "Filter RFQ Table completed.";
        }

        public bool LoadRFQTable(List<string> slProcessRFQ, string DownloadPath)
        {
            bool result = false, gotoNext = true;
            try
            {
                Thread.Sleep(2000);
                DOMElement PendingRFQTable = _netWrapper.GetElementbyClass(pagevar.tblPendingRFQClass);
                if (PendingRFQTable != null)
                {
                    DOMElement eletbody = PendingRFQTable.GetElementByTagName("tbody");
                    if (eletbody != null)
                    {
                        List<DOMNode> eleRow = eletbody.GetElementsByTagName("tr");
                        if (eleRow.Count > 0)
                        {
                            foreach (DOMNode row in eleRow)
                            {
                                string RFQRecDate = "", RFQNo = "", RFQDt = "", Vessel = "", Port = "", CompanyName = "", oldFileName = "", newFileName = "";// buyerName="";
                                try
                                {
                                    DOMElement eRow = (DOMElement)row;

                                    /*check RFq already processed or not*/
                                    GetProcessedItems(PluginInterface.eActions.RFQ);
                                    if (!IsProcessedRFQ(eRow, slProcessRFQ, out RFQRecDate, out RFQNo, out RFQDt, out Vessel, out Port, out CompanyName))
                                    {
                                        /* check RFQ received date and current date is same or not*/
                                        if (CheckRFQReceivedDate(RFQRecDate))
                                        {
                                            _netWrapper.LogText = "Processing RFQ for VRNO '" + RFQNo + "'.";
                                            oldFileName = "RFQ_" + RFQNo.Replace('/', '_') + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsm";

                                            /*Download RFQ File*/
                                            if (CompanyName != "")
                                            {
                                                buyer_Link_code = getBuyerLinkCode(CompanyName);
                                                newFileName = buyer_Link_code + "_RFQ_" + RFQNo.Replace('/', '_') + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsm";
                                                if (buyer_Link_code != "")
                                                {
                                                    DeleteRFQ_PendingRFQList(RFQNo, CompanyName);//31-3-2017
                                                    try
                                                    {
                                                        string dwndFile = DownloadFile(eRow, RFQNo, DownloadPath, oldFileName);
                                                        if (dwndFile != "")
                                                        {

                                                            File.Move(DownloadPath + "\\" + oldFileName, DownloadPath + "\\" + newFileName);
                                                            if (File.Exists(DownloadPath + "\\" + newFileName))
                                                            {
                                                                WriteToRFQDownloadedFile(RFQNo, RFQDt, Vessel, Port, RFQRecDate, newFileName);
                                                                result = true;
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)//31-7-2017
                                                    {
                                                        string sFile = ScreenShotPath + "\\BS_RFQ" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + eSupp_SuppAddCode + ".pdf";
                                                        _netWrapper.PrintPDF(sFile, false);
                                                        _netWrapper.LogText = "Exception while downloading RFQ file : " + ex.GetBaseException().ToString();
                                                        //CreateAuditFile("", ProcessorName, RFQNo, "Error", "Exception while downloading RFQ file : " + ex.GetBaseException().ToString());
                                                        CreateAuditFile("", ProcessorName, RFQNo, "Error", "LeS-1004:Unable to process file due to " + ex.GetBaseException().ToString());
                                                    }
                                                }
                                                else
                                                {
                                                    if (!Read_PendingRFQList(RFQNo, CompanyName))//31-3-2017
                                                    {
                                                        //string Audit = DateTime.Now.ToString("yy-MM-dd HH:mm") + " RFQ '" + newFileName + "' for VRNO '" + RFQNo + "' download failed because '" + CompanyName + "' not found in list.";
                                                        string Audit = "LeS-1004:Unable to process file " + newFileName;
                                                        CreateAuditFile("", ProcessorName, RFQNo, "Error", Audit);
                                                        AddToPendingRFQList(RFQNo, CompanyName);//31-3-2017
                                                    }
                                                    _netWrapper.LogText = "RFQ '" + newFileName + "' for VRNO '" + RFQNo + "' downloading failed because '" + CompanyName + "' not found in list.";
                                                }
                                            }
                                            else
                                            {
                                                result = DownloadFile(eRow, RFQNo, DownloadPath, oldFileName, CompanyName, newFileName, RFQDt, Vessel, Port, RFQRecDate);
                                            }
                                          
                                        }
                                    }
                                    else
                                        _netWrapper.LogText = "RFQ for VRNO '" + RFQNo + "' already downloaded.";
                                }
                                catch (Exception ex)//31-7-2017
                                {
                                    string sFile = ScreenShotPath + "\\BS_RFQ" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + eSupp_SuppAddCode + ".pdf";
                                    _netWrapper.PrintPDF(sFile, false);
                                    _netWrapper.LogText = "Exception while processing RFQ Items : " + ex.GetBaseException().ToString();
                                    //CreateAuditFile("", ProcessorName, RFQNo, "Error", "Exception while processing RFQ Items : " + ex.GetBaseException().ToString());
                                    CreateAuditFile("", ProcessorName, RFQNo, "Error", "LeS-1040:Unable to get details - RFQ Items Field(s) not present");
                                }
                            }
                        }
                        else { _netWrapper.LogText = "No Pending RFQ(s) found."; gotoNext = false; }
                    }
                    else { _netWrapper.LogText = "RFQ table body not loaded."; gotoNext = false; }
                }
                else { _netWrapper.LogText = "RFQ table not loaded."; gotoNext = false; }
                if (gotoNext)
                {
                    count++;
                    if (count <= 2)
                    {
                        bool value = changePager();
                        if (value)
                        {
                            _netWrapper.LogText = "Navigate to page " + (count + 1);
                            GetProcessedItems(PluginInterface.eActions.RFQ);
                            LoadRFQTable(slProcessRFQ, DownloadPath);
                        }
                    }
                    else count = 0;
                }
            }
            catch (Exception ex)
            {
                string sFile = ScreenShotPath + "\\BS_RFQ" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + eSupp_SuppAddCode + ".pdf";
                _netWrapper.PrintPDF(sFile, false);
                _netWrapper.LogText = "Exception while loading RFQ table : " + ex.GetBaseException().ToString();
            }
            return result;
        }

        public void AddToPendingRFQList(string RFQNo, string CompanyName)
        {
            string pendingPath = Application.StartupPath + "\\Pending_RFQ.txt";
            string strpendingText = RFQNo + "|" + CompanyName;
            File.AppendAllText(pendingPath, strpendingText + Environment.NewLine);
        }

        public bool Read_PendingRFQList(string RFQNo, string CompanyName)
        {
            bool isPresent = false;
            string pendingPath = Application.StartupPath + "\\Pending_RFQ.txt";
            string strpendingText = RFQNo + "|" + CompanyName;
            string[] _text = File.ReadAllLines(pendingPath);
            List<string> strList = _text.ToList();
            isPresent = (strList.IndexOf(RFQNo + "|" + CompanyName) > -1);
            return isPresent;
        }

        public void DeleteRFQ_PendingRFQList(string RFQNo, string CompanyName)
        {
            bool isPresent = false;
            string pendingPath = Application.StartupPath + "\\Pending_RFQ.txt";
            string strpendingText = RFQNo + "|" + CompanyName;
            string[] _text = File.ReadAllLines(pendingPath);
            List<string> strList = _text.ToList();
            isPresent = (strList.IndexOf(RFQNo + "|" + CompanyName) > -1);
            if (isPresent)
            {
                int index = strList.IndexOf(RFQNo + "|" + CompanyName);
                strList.RemoveAt(index);
            }
            File.WriteAllLines(pendingPath, strList);
        }

        public bool DownloadFile(DOMElement eRow, string RFQNo, string DownloadPath, string oldFileName, string CompanyName, string newFileName, string RFQDt, string Vessel, string Port, string RFQRecDate)
        {
            bool result = false;
            string dwndFile = DownloadFile(eRow, RFQNo, DownloadPath, oldFileName);
            if (dwndFile != "")
            {
                if (File.Exists(DownloadPath + "\\" + oldFileName))
                {
                    CompanyName = Read_RFQXlsFile(DownloadPath + "\\" + oldFileName);
                    if (CompanyName != null)
                    {
                        buyer_Link_code = getBuyerLinkCode(CompanyName);
                    }

                    newFileName = buyer_Link_code + "_RFQ_" + RFQNo.Replace('/', '_') + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsm";
                    if (buyer_Link_code != "")
                    {
                        DeleteRFQ_PendingRFQList(RFQNo, CompanyName);//31-3-2017
                        File.Move(DownloadPath + "\\" + oldFileName, DownloadPath + "\\" + newFileName);
                        if (File.Exists(DownloadPath + "\\" + newFileName))
                        {
                            WriteToRFQDownloadedFile(RFQNo, RFQDt, Vessel, Port, RFQRecDate, newFileName);
                            result = true;
                        }
                    }
                    else
                    {
                        if (!Read_PendingRFQList(RFQNo, CompanyName))//31-3-2017
                        {
                           // string Audit = DateTime.Now.ToString("yy-MM-dd HH:mm") + " RFQ '" + newFileName + "' for VRNO '" + RFQNo + "' download failed because '" + CompanyName + "' not found in list.";
                            string Audit = "LeS-1004:Unable to process file " + newFileName;
                            CreateAuditFile("", ProcessorName, RFQNo, "Error", Audit);
                            AddToPendingRFQList(RFQNo, CompanyName);//31-3-2017
                        }
                        _netWrapper.LogText = "RFQ '" + newFileName + "' for VRNO '" + RFQNo + "' downloading failed because '" + CompanyName + "' not found in list.";
                        File.Delete(DownloadPath + "\\" + oldFileName);
                    }
                }
                else
                {
                    _netWrapper.LogText = "RFQ File " + DownloadPath + "\\" + oldFileName + " not downloaded for for RFQ No. " + RFQNo + ".";
                    //string Audit = DateTime.Now.ToString("yy-MM-dd HH:mm") + " RFQ File " + DownloadPath + "\\" + oldFileName + " not downloaded for for RFQ No. " + RFQNo + ".";
                    string Audit = "LeS-1004:Unable to process file " + oldFileName;
                    CreateAuditFile("", ProcessorName, RFQNo, "Error", Audit);
                }
            }
            return result;
        }

        public bool ProcessRFQTable(List<string> slProcessRFQ, string DownloadPath)
        {
            bool result = false;
            try
            {
                GetPendingRFQTab();
                Filter_RFQ_By_Date();
                result = LoadRFQTable(slProcessRFQ, DownloadPath);
            }
            catch (Exception ex)
            {
                string sFile = ScreenShotPath + "\\BS_RFQ" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + eSupp_SuppAddCode + ".pdf";
                _netWrapper.PrintPDF(sFile, false);
                _netWrapper.LogText = "Exception while processing RFQ table : " + ex.GetBaseException().ToString();
            }
            return result;
        }

        public bool checkChar_AlphaNumeric(char value)
        {
            string a = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            if (a.Contains(value)) return true;
            else return false;
        }

        public void SetAlphaNumeric(char value)
        {
            switch (value)
            {
                case 'A':
                    KeyParams paramers_A = new KeyParams(VirtualKeyCode.VK_A, 'A');
                    _netWrapper.browser.KeyDown(paramers_A);
                    _netWrapper.browser.KeyUp(paramers_A);
                    break;
                case 'a':
                    KeyParams paramers_a = new KeyParams(VirtualKeyCode.VK_A, 'a');
                    _netWrapper.browser.KeyDown(paramers_a);
                    _netWrapper.browser.KeyUp(paramers_a);
                    break;
                case 'B':
                    KeyParams paramers_B = new KeyParams(VirtualKeyCode.VK_B, 'B');
                    _netWrapper.browser.KeyDown(paramers_B);
                    _netWrapper.browser.KeyUp(paramers_B);
                    break;
                case 'b':
                    KeyParams paramers_b = new KeyParams(VirtualKeyCode.VK_B, 'b');
                    _netWrapper.browser.KeyDown(paramers_b);
                    _netWrapper.browser.KeyUp(paramers_b);
                    break;
                case 'C':
                    KeyParams paramers_C = new KeyParams(VirtualKeyCode.VK_C, 'C');
                    _netWrapper.browser.KeyDown(paramers_C);
                    _netWrapper.browser.KeyUp(paramers_C);
                    break;
                case 'c':
                    KeyParams paramers_c = new KeyParams(VirtualKeyCode.VK_C, 'c');
                    _netWrapper.browser.KeyDown(paramers_c);
                    _netWrapper.browser.KeyUp(paramers_c);
                    break;
                case 'D':
                    KeyParams paramers_D = new KeyParams(VirtualKeyCode.VK_D, 'D');
                    _netWrapper.browser.KeyDown(paramers_D);
                    _netWrapper.browser.KeyUp(paramers_D);
                    break;
                case 'd':
                    KeyParams paramers_d = new KeyParams(VirtualKeyCode.VK_D, 'd');
                    _netWrapper.browser.KeyDown(paramers_d);
                    _netWrapper.browser.KeyUp(paramers_d);
                    break;
                case 'E':
                    KeyParams paramers_E = new KeyParams(VirtualKeyCode.VK_E, 'E');
                    _netWrapper.browser.KeyDown(paramers_E);
                    _netWrapper.browser.KeyUp(paramers_E);
                    break;
                case 'e':
                    KeyParams paramers_e = new KeyParams(VirtualKeyCode.VK_E, 'e');
                    _netWrapper.browser.KeyDown(paramers_e);
                    _netWrapper.browser.KeyUp(paramers_e);
                    break;
                case 'F':
                    KeyParams paramers_F = new KeyParams(VirtualKeyCode.VK_F, 'F');
                    _netWrapper.browser.KeyDown(paramers_F);
                    _netWrapper.browser.KeyUp(paramers_F);
                    break;
                case 'f':
                    KeyParams paramers_f = new KeyParams(VirtualKeyCode.VK_F, 'f');
                    _netWrapper.browser.KeyDown(paramers_f);
                    _netWrapper.browser.KeyUp(paramers_f);
                    break;
                case 'G':
                    KeyParams paramers_G = new KeyParams(VirtualKeyCode.VK_G, 'G');
                    _netWrapper.browser.KeyDown(paramers_G);
                    _netWrapper.browser.KeyUp(paramers_G);
                    break;
                case 'g':
                    KeyParams paramers_g = new KeyParams(VirtualKeyCode.VK_G, 'g');
                    _netWrapper.browser.KeyDown(paramers_g);
                    _netWrapper.browser.KeyUp(paramers_g);
                    break;
                case 'H':
                    KeyParams paramers_H = new KeyParams(VirtualKeyCode.VK_H, 'H');
                    _netWrapper.browser.KeyDown(paramers_H);
                    _netWrapper.browser.KeyUp(paramers_H);
                    break;
                case 'h':
                    KeyParams paramers_h = new KeyParams(VirtualKeyCode.VK_H, 'h');
                    _netWrapper.browser.KeyDown(paramers_h);
                    _netWrapper.browser.KeyUp(paramers_h);
                    break;
                case 'I':
                    KeyParams paramers_I = new KeyParams(VirtualKeyCode.VK_I, 'I');
                    _netWrapper.browser.KeyDown(paramers_I);
                    _netWrapper.browser.KeyUp(paramers_I);
                    break;
                case 'i':
                    KeyParams paramers_i = new KeyParams(VirtualKeyCode.VK_I, 'i');
                    _netWrapper.browser.KeyDown(paramers_i);
                    _netWrapper.browser.KeyUp(paramers_i);
                    break;
                case 'J':
                    KeyParams paramers_J = new KeyParams(VirtualKeyCode.VK_J, 'J');
                    _netWrapper.browser.KeyDown(paramers_J);
                    _netWrapper.browser.KeyUp(paramers_J);
                    break;
                case 'j':
                    KeyParams paramers_j = new KeyParams(VirtualKeyCode.VK_J, 'j');
                    _netWrapper.browser.KeyDown(paramers_j);
                    _netWrapper.browser.KeyUp(paramers_j);
                    break;
                case 'K':
                    KeyParams paramers_K = new KeyParams(VirtualKeyCode.VK_K, 'K');
                    _netWrapper.browser.KeyDown(paramers_K);
                    _netWrapper.browser.KeyUp(paramers_K);
                    break;
                case 'k':
                    KeyParams paramers_k = new KeyParams(VirtualKeyCode.VK_K, 'k');
                    _netWrapper.browser.KeyDown(paramers_k);
                    _netWrapper.browser.KeyUp(paramers_k);
                    break;
                case 'L':
                    KeyParams paramers_L = new KeyParams(VirtualKeyCode.VK_L, 'L');
                    _netWrapper.browser.KeyDown(paramers_L);
                    _netWrapper.browser.KeyUp(paramers_L);
                    break;
                case 'l':
                    KeyParams paramers_l = new KeyParams(VirtualKeyCode.VK_L, 'l');
                    _netWrapper.browser.KeyDown(paramers_l);
                    _netWrapper.browser.KeyUp(paramers_l);
                    break;
                case 'M':
                    KeyParams paramers_M = new KeyParams(VirtualKeyCode.VK_M, 'M');
                    _netWrapper.browser.KeyDown(paramers_M);
                    _netWrapper.browser.KeyUp(paramers_M);
                    break;
                case 'm':
                    KeyParams paramers_m = new KeyParams(VirtualKeyCode.VK_M, 'm');
                    _netWrapper.browser.KeyDown(paramers_m);
                    _netWrapper.browser.KeyUp(paramers_m);
                    break;
                case 'N':
                    KeyParams paramers_N = new KeyParams(VirtualKeyCode.VK_N, 'N');
                    _netWrapper.browser.KeyDown(paramers_N);
                    _netWrapper.browser.KeyUp(paramers_N);
                    break;
                case 'n':
                    KeyParams paramers_n = new KeyParams(VirtualKeyCode.VK_N, 'n');
                    _netWrapper.browser.KeyDown(paramers_n);
                    _netWrapper.browser.KeyUp(paramers_n);
                    break;
                case 'O':
                    KeyParams paramers_O = new KeyParams(VirtualKeyCode.VK_O, 'O');
                    _netWrapper.browser.KeyDown(paramers_O);
                    _netWrapper.browser.KeyUp(paramers_O);
                    break;
                case 'o':
                    KeyParams paramers_o = new KeyParams(VirtualKeyCode.VK_O, 'o');
                    _netWrapper.browser.KeyDown(paramers_o);
                    _netWrapper.browser.KeyUp(paramers_o);
                    break;
                case 'P':
                    KeyParams paramers_P = new KeyParams(VirtualKeyCode.VK_P, 'P');
                    _netWrapper.browser.KeyDown(paramers_P);
                    _netWrapper.browser.KeyUp(paramers_P);
                    break;
                case 'p':
                    KeyParams paramers_p = new KeyParams(VirtualKeyCode.VK_P, 'p');
                    _netWrapper.browser.KeyDown(paramers_p);
                    _netWrapper.browser.KeyUp(paramers_p);
                    break;
                case 'Q':
                    KeyParams paramers_Q = new KeyParams(VirtualKeyCode.VK_Q, 'Q');
                    _netWrapper.browser.KeyDown(paramers_Q);
                    _netWrapper.browser.KeyUp(paramers_Q);
                    break;
                case 'q':
                    KeyParams paramers_q = new KeyParams(VirtualKeyCode.VK_Q, 'q');
                    _netWrapper.browser.KeyDown(paramers_q);
                    _netWrapper.browser.KeyUp(paramers_q);
                    break;
                case 'R':
                    KeyParams paramers_R = new KeyParams(VirtualKeyCode.VK_R, 'R');
                    _netWrapper.browser.KeyDown(paramers_R);
                    _netWrapper.browser.KeyUp(paramers_R);
                    break;
                case 'r':
                    KeyParams paramers_r = new KeyParams(VirtualKeyCode.VK_R, 'r');
                    _netWrapper.browser.KeyDown(paramers_r);
                    _netWrapper.browser.KeyUp(paramers_r);
                    break;
                case 'S':
                    KeyParams paramers_S = new KeyParams(VirtualKeyCode.VK_S, 'S');
                    _netWrapper.browser.KeyDown(paramers_S);
                    _netWrapper.browser.KeyUp(paramers_S);
                    break;
                case 's':
                    KeyParams paramers_s = new KeyParams(VirtualKeyCode.VK_S, 's');
                    _netWrapper.browser.KeyDown(paramers_s);
                    _netWrapper.browser.KeyUp(paramers_s);
                    break;
                case 'T':
                    KeyParams paramers_T = new KeyParams(VirtualKeyCode.VK_T, 'T');
                    _netWrapper.browser.KeyDown(paramers_T);
                    _netWrapper.browser.KeyUp(paramers_T);
                    break;
                case 't':
                    KeyParams paramers_t = new KeyParams(VirtualKeyCode.VK_T, 't');
                    _netWrapper.browser.KeyDown(paramers_t);
                    _netWrapper.browser.KeyUp(paramers_t);
                    break;
                case 'U':
                    KeyParams paramers_U = new KeyParams(VirtualKeyCode.VK_U, 'U');
                    _netWrapper.browser.KeyDown(paramers_U);
                    _netWrapper.browser.KeyUp(paramers_U);
                    break;
                case 'u':
                    KeyParams paramers_u = new KeyParams(VirtualKeyCode.VK_U, 'u');
                    _netWrapper.browser.KeyDown(paramers_u);
                    _netWrapper.browser.KeyUp(paramers_u);
                    break;
                case 'V':
                    KeyParams paramers_V = new KeyParams(VirtualKeyCode.VK_V, 'V');
                    _netWrapper.browser.KeyDown(paramers_V);
                    _netWrapper.browser.KeyUp(paramers_V);
                    break;
                case 'v':
                    KeyParams paramers_v = new KeyParams(VirtualKeyCode.VK_V, 'v');
                    _netWrapper.browser.KeyDown(paramers_v);
                    _netWrapper.browser.KeyUp(paramers_v);
                    break;
                case 'W':
                    KeyParams paramers_W = new KeyParams(VirtualKeyCode.VK_W, 'W');
                    _netWrapper.browser.KeyDown(paramers_W);
                    _netWrapper.browser.KeyUp(paramers_W);
                    break;
                case 'w':
                    KeyParams paramers_w = new KeyParams(VirtualKeyCode.VK_W, 'w');
                    _netWrapper.browser.KeyDown(paramers_w);
                    _netWrapper.browser.KeyUp(paramers_w);
                    break;
                case 'X':
                    KeyParams paramers_X = new KeyParams(VirtualKeyCode.VK_X, 'X');
                    _netWrapper.browser.KeyDown(paramers_X);
                    _netWrapper.browser.KeyUp(paramers_X);
                    break;
                case 'x':
                    KeyParams paramers_x = new KeyParams(VirtualKeyCode.VK_X, 'x');
                    _netWrapper.browser.KeyDown(paramers_x);
                    _netWrapper.browser.KeyUp(paramers_x);
                    break;
                case 'Y':
                    KeyParams paramers_Y = new KeyParams(VirtualKeyCode.VK_Y, 'Y');
                    _netWrapper.browser.KeyDown(paramers_Y);
                    _netWrapper.browser.KeyUp(paramers_Y);
                    break;
                case 'y':
                    KeyParams paramers_y = new KeyParams(VirtualKeyCode.VK_Y, 'y');
                    _netWrapper.browser.KeyDown(paramers_y);
                    _netWrapper.browser.KeyUp(paramers_y);
                    break;
                case 'Z':
                    KeyParams paramers_Z = new KeyParams(VirtualKeyCode.VK_Z, 'Z');
                    _netWrapper.browser.KeyDown(paramers_Z);
                    _netWrapper.browser.KeyUp(paramers_Z);
                    break;
                case 'z':
                    KeyParams paramers_z = new KeyParams(VirtualKeyCode.VK_Z, 'z');
                    _netWrapper.browser.KeyDown(paramers_z);
                    _netWrapper.browser.KeyUp(paramers_z);
                    break;
                case '1':
                    KeyParams paramers_1 = new KeyParams(VirtualKeyCode.VK_1, '1');
                    _netWrapper.browser.KeyDown(paramers_1);
                    _netWrapper.browser.KeyUp(paramers_1);
                    break;
                case '2':
                    KeyParams paramers_2 = new KeyParams(VirtualKeyCode.VK_2, '2');
                    _netWrapper.browser.KeyDown(paramers_2);
                    _netWrapper.browser.KeyUp(paramers_2);
                    break;
                case '3':
                    KeyParams paramers_3 = new KeyParams(VirtualKeyCode.VK_3, '3');
                    _netWrapper.browser.KeyDown(paramers_3);
                    _netWrapper.browser.KeyUp(paramers_3);
                    break;
                case '4':
                    KeyParams paramers_4 = new KeyParams(VirtualKeyCode.VK_4, '4');
                    _netWrapper.browser.KeyDown(paramers_4);
                    _netWrapper.browser.KeyUp(paramers_4);
                    break;
                case '5':
                    KeyParams paramers_5 = new KeyParams(VirtualKeyCode.VK_5, '5');
                    _netWrapper.browser.KeyDown(paramers_5);
                    _netWrapper.browser.KeyUp(paramers_5);
                    break;
                case '6':
                    KeyParams paramers_6 = new KeyParams(VirtualKeyCode.VK_6, '6');
                    _netWrapper.browser.KeyDown(paramers_6);
                    _netWrapper.browser.KeyUp(paramers_6);
                    break;
                case '7':
                    KeyParams paramers_7 = new KeyParams(VirtualKeyCode.VK_7, '7');
                    _netWrapper.browser.KeyDown(paramers_7);
                    _netWrapper.browser.KeyUp(paramers_7);
                    break;
                case '8':
                    KeyParams paramers_8 = new KeyParams(VirtualKeyCode.VK_8, '8');
                    _netWrapper.browser.KeyDown(paramers_8);
                    _netWrapper.browser.KeyUp(paramers_8);
                    break;
                case '9':
                    KeyParams paramers_9 = new KeyParams(VirtualKeyCode.VK_9, '9');
                    _netWrapper.browser.KeyDown(paramers_9);
                    _netWrapper.browser.KeyUp(paramers_9);
                    break;
                case '0':
                    KeyParams paramers_0 = new KeyParams(VirtualKeyCode.VK_0, '0');
                    _netWrapper.browser.KeyDown(paramers_0);
                    _netWrapper.browser.KeyUp(paramers_0);
                    break;
            }
        }

        public void SetCharacters(char value)
        {
            switch (value)
            {
                case 'A':
                    KeyParams paramers_A = new KeyParams(VirtualKeyCode.VK_A, 'A');
                    _netWrapper.browser.KeyDown(paramers_A);
                    _netWrapper.browser.KeyUp(paramers_A);
                    break;
                case 'a':
                    KeyParams paramers_a = new KeyParams(VirtualKeyCode.VK_A, 'a');
                    _netWrapper.browser.KeyDown(paramers_a);
                    _netWrapper.browser.KeyUp(paramers_a);
                    break;
                case 'B':
                    KeyParams paramers_B = new KeyParams(VirtualKeyCode.VK_B, 'B');
                    _netWrapper.browser.KeyDown(paramers_B);
                    _netWrapper.browser.KeyUp(paramers_B);
                    break;
                case 'b':
                    KeyParams paramers_b = new KeyParams(VirtualKeyCode.VK_B, 'b');
                    _netWrapper.browser.KeyDown(paramers_b);
                    _netWrapper.browser.KeyUp(paramers_b);
                    break;
                case 'C':
                    KeyParams paramers_C = new KeyParams(VirtualKeyCode.VK_C, 'C');
                    _netWrapper.browser.KeyDown(paramers_C);
                    _netWrapper.browser.KeyUp(paramers_C);
                    break;
                case 'c':
                    KeyParams paramers_c = new KeyParams(VirtualKeyCode.VK_C, 'c');
                    _netWrapper.browser.KeyDown(paramers_c);
                    _netWrapper.browser.KeyUp(paramers_c);
                    break;
                case 'D':
                    KeyParams paramers_D = new KeyParams(VirtualKeyCode.VK_D, 'D');
                    _netWrapper.browser.KeyDown(paramers_D);
                    _netWrapper.browser.KeyUp(paramers_D);
                    break;
                case 'd':
                    KeyParams paramers_d = new KeyParams(VirtualKeyCode.VK_D, 'd');
                    _netWrapper.browser.KeyDown(paramers_d);
                    _netWrapper.browser.KeyUp(paramers_d);
                    break;
                case 'E':
                    KeyParams paramers_E = new KeyParams(VirtualKeyCode.VK_E, 'E');
                    _netWrapper.browser.KeyDown(paramers_E);
                    _netWrapper.browser.KeyUp(paramers_E);
                    break;
                case 'e':
                    KeyParams paramers_e = new KeyParams(VirtualKeyCode.VK_E, 'e');
                    _netWrapper.browser.KeyDown(paramers_e);
                    _netWrapper.browser.KeyUp(paramers_e);
                    break;
                case 'F':
                    KeyParams paramers_F = new KeyParams(VirtualKeyCode.VK_F, 'F');
                    _netWrapper.browser.KeyDown(paramers_F);
                    _netWrapper.browser.KeyUp(paramers_F);
                    break;
                case 'f':
                    KeyParams paramers_f = new KeyParams(VirtualKeyCode.VK_F, 'f');
                    _netWrapper.browser.KeyDown(paramers_f);
                    _netWrapper.browser.KeyUp(paramers_f);
                    break;
                case 'G':
                    KeyParams paramers_G = new KeyParams(VirtualKeyCode.VK_G, 'G');
                    _netWrapper.browser.KeyDown(paramers_G);
                    _netWrapper.browser.KeyUp(paramers_G);
                    break;
                case 'g':
                    KeyParams paramers_g = new KeyParams(VirtualKeyCode.VK_G, 'g');
                    _netWrapper.browser.KeyDown(paramers_g);
                    _netWrapper.browser.KeyUp(paramers_g);
                    break;
                case 'H':
                    KeyParams paramers_H = new KeyParams(VirtualKeyCode.VK_H, 'H');
                    _netWrapper.browser.KeyDown(paramers_H);
                    _netWrapper.browser.KeyUp(paramers_H);
                    break;
                case 'h':
                    KeyParams paramers_h = new KeyParams(VirtualKeyCode.VK_H, 'h');
                    _netWrapper.browser.KeyDown(paramers_h);
                    _netWrapper.browser.KeyUp(paramers_h);
                    break;
                case 'I':
                    KeyParams paramers_I = new KeyParams(VirtualKeyCode.VK_I, 'I');
                    _netWrapper.browser.KeyDown(paramers_I);
                    _netWrapper.browser.KeyUp(paramers_I);
                    break;
                case 'i':
                    KeyParams paramers_i = new KeyParams(VirtualKeyCode.VK_I, 'i');
                    _netWrapper.browser.KeyDown(paramers_i);
                    _netWrapper.browser.KeyUp(paramers_i);
                    break;
                case 'J':
                    KeyParams paramers_J = new KeyParams(VirtualKeyCode.VK_J, 'J');
                    _netWrapper.browser.KeyDown(paramers_J);
                    _netWrapper.browser.KeyUp(paramers_J);
                    break;
                case 'j':
                    KeyParams paramers_j = new KeyParams(VirtualKeyCode.VK_J, 'j');
                    _netWrapper.browser.KeyDown(paramers_j);
                    _netWrapper.browser.KeyUp(paramers_j);
                    break;
                case 'K':
                    KeyParams paramers_K = new KeyParams(VirtualKeyCode.VK_K, 'K');
                    _netWrapper.browser.KeyDown(paramers_K);
                    _netWrapper.browser.KeyUp(paramers_K);
                    break;
                case 'k':
                    KeyParams paramers_k = new KeyParams(VirtualKeyCode.VK_K, 'k');
                    _netWrapper.browser.KeyDown(paramers_k);
                    _netWrapper.browser.KeyUp(paramers_k);
                    break;
                case 'L':
                    KeyParams paramers_L = new KeyParams(VirtualKeyCode.VK_L, 'L');
                    _netWrapper.browser.KeyDown(paramers_L);
                    _netWrapper.browser.KeyUp(paramers_L);
                    break;
                case 'l':
                    KeyParams paramers_l = new KeyParams(VirtualKeyCode.VK_L, 'l');
                    _netWrapper.browser.KeyDown(paramers_l);
                    _netWrapper.browser.KeyUp(paramers_l);
                    break;
                case 'M':
                    KeyParams paramers_M = new KeyParams(VirtualKeyCode.VK_M, 'M');
                    _netWrapper.browser.KeyDown(paramers_M);
                    _netWrapper.browser.KeyUp(paramers_M);
                    break;
                case 'm':
                    KeyParams paramers_m = new KeyParams(VirtualKeyCode.VK_M, 'm');
                    _netWrapper.browser.KeyDown(paramers_m);
                    _netWrapper.browser.KeyUp(paramers_m);
                    break;
                case 'N':
                    KeyParams paramers_N = new KeyParams(VirtualKeyCode.VK_N, 'N');
                    _netWrapper.browser.KeyDown(paramers_N);
                    _netWrapper.browser.KeyUp(paramers_N);
                    break;
                case 'n':
                    KeyParams paramers_n = new KeyParams(VirtualKeyCode.VK_N, 'n');
                    _netWrapper.browser.KeyDown(paramers_n);
                    _netWrapper.browser.KeyUp(paramers_n);
                    break;
                case 'O':
                    KeyParams paramers_O = new KeyParams(VirtualKeyCode.VK_O, 'O');
                    _netWrapper.browser.KeyDown(paramers_O);
                    _netWrapper.browser.KeyUp(paramers_O);
                    break;
                case 'o':
                    KeyParams paramers_o = new KeyParams(VirtualKeyCode.VK_O, 'o');
                    _netWrapper.browser.KeyDown(paramers_o);
                    _netWrapper.browser.KeyUp(paramers_o);
                    break;
                case 'P':
                    KeyParams paramers_P = new KeyParams(VirtualKeyCode.VK_P, 'P');
                    _netWrapper.browser.KeyDown(paramers_P);
                    _netWrapper.browser.KeyUp(paramers_P);
                    break;
                case 'p':
                    KeyParams paramers_p = new KeyParams(VirtualKeyCode.VK_P, 'p');
                    _netWrapper.browser.KeyDown(paramers_p);
                    _netWrapper.browser.KeyUp(paramers_p);
                    break;
                case 'Q':
                    KeyParams paramers_Q = new KeyParams(VirtualKeyCode.VK_Q, 'Q');
                    _netWrapper.browser.KeyDown(paramers_Q);
                    _netWrapper.browser.KeyUp(paramers_Q);
                    break;
                case 'q':
                    KeyParams paramers_q = new KeyParams(VirtualKeyCode.VK_Q, 'q');
                    _netWrapper.browser.KeyDown(paramers_q);
                    _netWrapper.browser.KeyUp(paramers_q);
                    break;
                case 'R':
                    KeyParams paramers_R = new KeyParams(VirtualKeyCode.VK_R, 'R');
                    _netWrapper.browser.KeyDown(paramers_R);
                    _netWrapper.browser.KeyUp(paramers_R);
                    break;
                case 'r':
                    KeyParams paramers_r = new KeyParams(VirtualKeyCode.VK_R, 'r');
                    _netWrapper.browser.KeyDown(paramers_r);
                    _netWrapper.browser.KeyUp(paramers_r);
                    break;
                case 'S':
                    KeyParams paramers_S = new KeyParams(VirtualKeyCode.VK_S, 'S');
                    _netWrapper.browser.KeyDown(paramers_S);
                    _netWrapper.browser.KeyUp(paramers_S);
                    break;
                case 's':
                    KeyParams paramers_s = new KeyParams(VirtualKeyCode.VK_S, 's');
                    _netWrapper.browser.KeyDown(paramers_s);
                    _netWrapper.browser.KeyUp(paramers_s);
                    break;
                case 'T':
                    KeyParams paramers_T = new KeyParams(VirtualKeyCode.VK_T, 'T');
                    _netWrapper.browser.KeyDown(paramers_T);
                    _netWrapper.browser.KeyUp(paramers_T);
                    break;
                case 't':
                    KeyParams paramers_t = new KeyParams(VirtualKeyCode.VK_T, 't');
                    _netWrapper.browser.KeyDown(paramers_t);
                    _netWrapper.browser.KeyUp(paramers_t);
                    break;
                case 'U':
                    KeyParams paramers_U = new KeyParams(VirtualKeyCode.VK_U, 'U');
                    _netWrapper.browser.KeyDown(paramers_U);
                    _netWrapper.browser.KeyUp(paramers_U);
                    break;
                case 'u':
                    KeyParams paramers_u = new KeyParams(VirtualKeyCode.VK_U, 'u');
                    _netWrapper.browser.KeyDown(paramers_u);
                    _netWrapper.browser.KeyUp(paramers_u);
                    break;
                case 'V':
                    KeyParams paramers_V = new KeyParams(VirtualKeyCode.VK_V, 'V');
                    _netWrapper.browser.KeyDown(paramers_V);
                    _netWrapper.browser.KeyUp(paramers_V);
                    break;
                case 'v':
                    KeyParams paramers_v = new KeyParams(VirtualKeyCode.VK_V, 'v');
                    _netWrapper.browser.KeyDown(paramers_v);
                    _netWrapper.browser.KeyUp(paramers_v);
                    break;
                case 'W':
                    KeyParams paramers_W = new KeyParams(VirtualKeyCode.VK_W, 'W');
                    _netWrapper.browser.KeyDown(paramers_W);
                    _netWrapper.browser.KeyUp(paramers_W);
                    break;
                case 'w':
                    KeyParams paramers_w = new KeyParams(VirtualKeyCode.VK_W, 'w');
                    _netWrapper.browser.KeyDown(paramers_w);
                    _netWrapper.browser.KeyUp(paramers_w);
                    break;
                case 'X':
                    KeyParams paramers_X = new KeyParams(VirtualKeyCode.VK_X, 'X');
                    _netWrapper.browser.KeyDown(paramers_X);
                    _netWrapper.browser.KeyUp(paramers_X);
                    break;
                case 'x':
                    KeyParams paramers_x = new KeyParams(VirtualKeyCode.VK_X, 'x');
                    _netWrapper.browser.KeyDown(paramers_x);
                    _netWrapper.browser.KeyUp(paramers_x);
                    break;
                case 'Y':
                    KeyParams paramers_Y = new KeyParams(VirtualKeyCode.VK_Y, 'Y');
                    _netWrapper.browser.KeyDown(paramers_Y);
                    _netWrapper.browser.KeyUp(paramers_Y);
                    break;
                case 'y':
                    KeyParams paramers_y = new KeyParams(VirtualKeyCode.VK_Y, 'y');
                    _netWrapper.browser.KeyDown(paramers_y);
                    _netWrapper.browser.KeyUp(paramers_y);
                    break;
                case 'Z':
                    KeyParams paramers_Z = new KeyParams(VirtualKeyCode.VK_Z, 'Z');
                    _netWrapper.browser.KeyDown(paramers_Z);
                    _netWrapper.browser.KeyUp(paramers_Z);
                    break;
                case 'z':
                    KeyParams paramers_z = new KeyParams(VirtualKeyCode.VK_Z, 'z');
                    _netWrapper.browser.KeyDown(paramers_z);
                    _netWrapper.browser.KeyUp(paramers_z);
                    break;
                case '1':
                    KeyParams paramers_1 = new KeyParams(VirtualKeyCode.VK_1, '1');
                    _netWrapper.browser.KeyDown(paramers_1);
                    _netWrapper.browser.KeyUp(paramers_1);
                    break;
                case '2':
                    KeyParams paramers_2 = new KeyParams(VirtualKeyCode.VK_2, '2');
                    _netWrapper.browser.KeyDown(paramers_2);
                    _netWrapper.browser.KeyUp(paramers_2);
                    break;
                case '3':
                    KeyParams paramers_3 = new KeyParams(VirtualKeyCode.VK_3, '3');
                    _netWrapper.browser.KeyDown(paramers_3);
                    _netWrapper.browser.KeyUp(paramers_3);
                    break;
                case '4':
                    KeyParams paramers_4 = new KeyParams(VirtualKeyCode.VK_4, '4');
                    _netWrapper.browser.KeyDown(paramers_4);
                    _netWrapper.browser.KeyUp(paramers_4);
                    break;
                case '5':
                    KeyParams paramers_5 = new KeyParams(VirtualKeyCode.VK_5, '5');
                    _netWrapper.browser.KeyDown(paramers_5);
                    _netWrapper.browser.KeyUp(paramers_5);
                    break;
                case '6':
                    KeyParams paramers_6 = new KeyParams(VirtualKeyCode.VK_6, '6');
                    _netWrapper.browser.KeyDown(paramers_6);
                    _netWrapper.browser.KeyUp(paramers_6);
                    break;
                case '7':
                    KeyParams paramers_7 = new KeyParams(VirtualKeyCode.VK_7, '7');
                    _netWrapper.browser.KeyDown(paramers_7);
                    _netWrapper.browser.KeyUp(paramers_7);
                    break;
                case '8':
                    KeyParams paramers_8 = new KeyParams(VirtualKeyCode.VK_8, '8');
                    _netWrapper.browser.KeyDown(paramers_8);
                    _netWrapper.browser.KeyUp(paramers_8);
                    break;
                case '9':
                    KeyParams paramers_9 = new KeyParams(VirtualKeyCode.VK_9, '9');
                    _netWrapper.browser.KeyDown(paramers_9);
                    _netWrapper.browser.KeyUp(paramers_9);
                    break;
                case '0':
                    KeyParams paramers_0 = new KeyParams(VirtualKeyCode.VK_0, '0');
                    _netWrapper.browser.KeyDown(paramers_0);
                    _netWrapper.browser.KeyUp(paramers_0);
                    break;
                case ' ':
                    KeyParams paramers = new KeyParams(VirtualKeyCode.SPACE, ' ');
                    _netWrapper.browser.KeyDown(paramers);
                    _netWrapper.browser.KeyUp(paramers);
                    break;
                case '.':
                    KeyParams paramers_per = new KeyParams(VirtualKeyCode.OEM_PERIOD, '.');
                    _netWrapper.browser.KeyDown(paramers_per);
                    _netWrapper.browser.KeyUp(paramers_per);
                    break;
                case ',':
                    KeyParams paramers_comma = new KeyParams(VirtualKeyCode.OEM_COMMA, ',');
                    _netWrapper.browser.KeyDown(paramers_comma);
                    _netWrapper.browser.KeyUp(paramers_comma);
                    break;
                case '"':
                    KeyParams paramers_quote = new KeyParams(VirtualKeyCode.OEM_7, '"');
                    _netWrapper.browser.KeyDown(paramers_quote);
                    _netWrapper.browser.KeyUp(paramers_quote);
                    break;
            }
        }

        public bool changePager()
        {
            bool result = false;
            DOMElement _element = _netWrapper.GetElementbyClass(pagevar.divPager);
            if (_element != null)
            {
                string a = _element.GetAttribute("style");
                if (!a.Contains("display: none"))
                {

                    DOMElement _aNextPage = _netWrapper.GetElementByTitle("a", "Go to the next page", _element);
                    if (_aNextPage.GetAttribute("class") != pagevar.btnPagerNextDis)
                    {
                        _aNextPage.Click();
                        result = true;
                    }
                }
            }
            return result;
        }

        public string getBuyerLinkCode(string buyerName)
        {
            int c = 0;
            string buyer_Link_code = "";
            foreach (string b in BuyerNames)
            {

                if (b.ToUpper() == buyerName.ToUpper())
                {
                    buyer_Link_code = Buyer_Supplier_LinkID.ElementAt(c);
                    break;
                }
                c++;
            }
            return buyer_Link_code;
        }

        public bool IsProcessedRFQ(DOMElement eRow, List<string> slProcessRFQ, out string RFQRecDate, out string RfqNo, out string RFQDt, out string Vessel, out string Port, out string CompanyName)
        {
            bool isProcessed = true;
            RFQRecDate = ""; RfqNo = ""; RFQDt = ""; Vessel = ""; Port = ""; CompanyName = "";
            try
            {
                List<DOMNode> _data = eRow.GetElementsByTagName("td");
                if (_data.Count > 0)
                {
                    var spans = _data[0].GetElementsByTagName("span");
                    if (spans.Count > 0)
                    {
                        RfqNo = spans[1].TextContent.Trim();
                    }

                    //RFQRecDate = _data[11].GetElementByTagName("div").InnerText.Trim();//11-9-2017
                    RFQRecDate = _data[12].GetElementByTagName("div").InnerText.Trim();//11-9-2017
                    RFQDt = _data[3].GetElementByTagName("div").InnerText.Trim();
                    //Vessel = _data[5].TextContent.Trim();//11-9-2017
                    Vessel = _data[6].TextContent.Trim();//11-9-2017
                    //Port = _data[6].TextContent.Trim();//11-9-2017
                    Port = _data[7].TextContent.Trim();//11-9-2017
                    //CompanyName = _data[7].TextContent.Trim();//11-9-2017
                    CompanyName = _data[8].TextContent.Trim();//11-9-2017

                    isProcessed = (slProcessRFQ.IndexOf(RfqNo + "|" + RFQDt + "|" + Vessel + "|" + Port + "|" + RFQRecDate) > -1);
                }
                return isProcessed;
            }
            catch (Exception ex)
            {
                _netWrapper.LogText = "Exception at IsProcessedRFQ: " + ex.Message.ToString();
                throw;
            }
        }

        public string DownloadFile(DOMElement eRow, string RFQNo, string DownloadPath, string FileName)
        {
            DOMElement btnDownload = null;
            List<DOMNode> _data = eRow.GetElementsByTagName("td");
            if (_data.Count > 0)
            {
                //var btn = _data[10].GetElementsByTagName("a");//11-9-2017
                var btn = _data[11].GetElementsByTagName("a");//11-9-2017
                if (btn.Count > 0)
                {
                    btnDownload = (DOMElement)btn[1];
                    //  string title = btnDownload.GetAttribute("Title");
                }

            }
            string dwndFile = _netWrapper.DownloadFileOnClick(btnDownload, DownloadPath, FileName, false, "");
            return dwndFile;
        }

        public bool CheckRFQReceivedDate(string RFQRecDate)
        {
            if (Convert.ToDateTime(RFQRecDate).ToString("dd-MMM-yyyy") == Convert.ToDateTime(currentDate).ToString("dd-MMM-yyyy"))
                return true;
            else
                return false;
        }

        public void WriteToRFQDownloadedFile(string RFQNo, string RFQDt, string Vessel, string Port, string RFQRecDate, string FileName)
        {
            string strRFQDwnld = RFQNo + "|" + RFQDt + "|" + Vessel + "|" + Port + "|" + RFQRecDate;
            File.AppendAllText(Application.StartupPath + "\\RFQ_Downloaded.txt", strRFQDwnld + Environment.NewLine);
            string Audit = DateTime.Now.ToString("yy-MM-dd HH:mm") + "RFQ '" + FileName + "' for ref '" + RFQNo + "' downloaded successfully.";
            CreateAuditFile(FileName, ProcessorName, RFQNo, "Downloaded", Audit);
            _netWrapper.LogText = "RFQ '" + FileName + "' for VRNO '" + RFQNo + "' downloaded successfully.";
        }

        public string Read_RFQXlsFile(string FileName)
        {
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Total.lic");
            Aspose.Cells.Workbook _workbook = new Aspose.Cells.Workbook(FileName);
            Aspose.Cells.Worksheet _worksheet = _workbook.Worksheets[0];
            Aspose.Cells.Cell A4 = _worksheet.Cells["A4"];
            string buyerDetail = (string)A4.Value;
            string[] stringSeparators = new string[] { "\r\n" };
            string[] lines = buyerDetail.Split(stringSeparators, StringSplitOptions.None);
            return lines[0];
        }

        #endregion

        #region Quote

        //public void ProcessQuote(string MailPath, string MailTemplate, string FROM_EMAILID, string MailBcc)
        //{
        //    MailFilePath = MailPath;
        //    Mail_Template = MailTemplate;
        //    FROM_EMAIL_ID = FROM_EMAILID;
        //    MAIL_BCC = MailBcc;
        //   // if (_netWrapper = null)
        //    {
        //        try
        //        {
        //            _netWrapper.LogText = "";
        //            _netWrapper.LogText = "Quote processing started.";
        //            /*get xml files from quote upload path*/
        //            LoadAppSettings();
        //            GetXmlFiles();
        //            if (xmlFiles.Count > 0)
        //            {
        //                if (LogIn())
        //                {
        //                    _netWrapper.LogText = xmlFiles.Count + " Quote files found to process.";
        //                    IsQuote = true;
        //                    IsLoad = true;

        //                    for (int j = 0; j < xmlFiles.Count; j++)
        //                    {
        //                        ProcessQuoteMTML(xmlFiles[j]);
        //                        ClearCommonVariables();
        //                    }
        //                    IsQuote = false;
        //                }
        //            }
        //            else
        //                _netWrapper.LogText = "No Quote files found to process.";
        //            _netWrapper.LogText = "Quote processing stopped.";
        //            _netWrapper.LogText = "Bernhard Schulte Processor Stopped.";
        //        }
        //        catch (Exception ex)
        //        {
        //            _netWrapper.LogText = "Exception while processing Quote : " + ex.GetBaseException().ToString();
        //        }
        //        finally
        //        {
        //            _netWrapper.browserView.Dispose();
        //            _netWrapper.Dispose();
        //        }
        //    }
        //}

        public void ProcessQuote(string MailPath, string MailTemplate, string FROM_EMAILID, string MailBcc)
        {
            MailFilePath = MailPath;
            Mail_Template = MailTemplate;
            FROM_EMAIL_ID = FROM_EMAILID;
            MAIL_BCC = MailBcc;
            try
            {
                _netWrapper.LogText = Environment.NewLine;
                _netWrapper.LogText = "Bernhard Schulte Processor Started.";// changed by Kalpita on 18/11/2019
                if (LogIn())
                {                   
                    _netWrapper.LogText = "Quote processing started.";
                    GetXmlFiles();
                    if (xmlFiles.Count > 0)
                    {
                        _netWrapper.LogText = xmlFiles.Count + " Quote files found to process.";
                        IsQuote = true;
                        IsLoad = true;

                        for (int j = 0; j < xmlFiles.Count; j++)
                        {
                            ProcessQuoteMTML(xmlFiles[j]);
                            ClearCommonVariables();
                        }
                        IsQuote = false;
                    }
                    else
                        _netWrapper.LogText = "No Quote files found to process.";
                    _netWrapper.LogText = "Quote processing stopped.";
                    //_netWrapper.LogText = "Bernhard Schulte Processor Stopped.";
                }
                _netWrapper.LogText = "Bernhard Schulte Processor Stopped.";// changed by Kalpita on 18/11/2019
            }
            catch (Exception ex)
            {
                _netWrapper.LogText = "Exception while processing Quote : " + ex.GetBaseException().ToString();
            }
            finally
            {
                //
                _netWrapper.browserView.Dispose();
                _netWrapper.Dispose();
            }
        }

        /*get xml files from quote upload path*/
        public void GetXmlFiles()
        {
            xmlFiles.Clear();
            DirectoryInfo _dir = new DirectoryInfo(MTML_Quote_UploadPath);
            FileInfo[] _Files = _dir.GetFiles();
            foreach (FileInfo _MtmlFile in _Files)
            {
                xmlFiles.Add(_MtmlFile.FullName);
            }
        }

        public void ProcessQuoteMTML(string MTML_QuoteFile)
        {
            int count = 0, maxcount = 1;
            try
            {
                _netWrapper.LogText = "'" + Path.GetFileName(MTML_QuoteFile) + "' Quote file processing started.";
                /* go to All tab page*/
                //GetAllRFQTab();
                GetPendingRFQTab();//14-03-19 
                if (_netWrapper.WaitForLoading("pace-progress", "data-progress-text", "100%", pagevar.tblPendingRFQClass))//15-12-2017
                {
                    MTMLClass _mtml = new MTMLClass();
                    _interchange = _mtml.Load(MTML_QuoteFile);
                    LoadInterchangeDetails();
                    //Add price validity date in remarks
                    if (Add_ValidDate_Remarks(Quote_UploadPath + @"\" + MsgNumber, MTML_QuoteFile))
                    {
                        if (UCRefNo != "")
                        {
                            LoadQuoteTable(MTML_QuoteFile);
                        }
                        else
                        {
                            count++;
                            if (count <= maxcount)
                            {
                                ProcessQuoteMTML(MTML_QuoteFile);
                            }
                            else
                            {
                                MoveFileToError_WithoutScreenShot(MTML_QuoteFile, "Unable to save validity date in Vendor Remarks for Quote VRNo '" + UCRefNo + "'", Quote_UploadPath + @"\" + MsgNumber);
                            }
                        }
                    }
                }
                else MoveFileToError(MTML_QuoteFile, "Unable to filter Quote for VRNo '" + UCRefNo + "'", Quote_UploadPath + @"\" + MsgNumber);
                Clear_SearchBox();
            }
            catch (Exception ex)
            {
                MoveFileToError(MTML_QuoteFile, "Exception while processing Quote MTML : " + ex.GetBaseException().ToString(), Quote_UploadPath + @"\" + MsgNumber);
            }
        }

        public void GetAllRFQTab()
        {
            DOMElement enquiryClass = _netWrapper.GetElementbyClass(pagevar.ulEnquiryClass);
            if (enquiryClass != null)
            {
                var anchors = enquiryClass.GetElementsByTagName("a");
                foreach (DOMElement a in anchors)
                {
                    if (a.InnerText == "All")
                    {
                        a.Click();
                        break;
                    }
                }
            }
            else  _netWrapper.LogText = "All tab is null"; 
        }

        public void GetDraftRFQTab()
        {
            DOMElement enquiryClass = _netWrapper.GetElementbyClass(pagevar.ulEnquiryClass);
            if (enquiryClass != null)
            {
                var anchors = enquiryClass.GetElementsByTagName("a");
                foreach (DOMElement a in anchors)
                {
                    if (a.InnerText == "Drafted")
                    {
                        a.Click();
                        break;
                    }
                }
            }
            else _netWrapper.LogText = "Drafted tab is null";
        }

        public void ClearCommonVariables()
        {
            AAGRefNo = ""; LesRecordID = ""; MessageNumber = ""; LeadDays = ""; Currency = ""; BuyerName = ""; BuyerPhone = ""; BuyerEmail = ""; BuyerFax = "";
            supplierName = ""; supplierPhone = ""; supplierEmail = ""; supplierFax = ""; VesselName = ""; PortName = ""; PortCode = ""; SupplierComment = ""; PayTerms = "";
            PackingCost = ""; FreightCharge = ""; GrandTotal = ""; Allowance = ""; TotalLineItemsAmount = ""; DtDelvDate = ""; dtExpDate = "";
            supplierFax = ""; VesselName = ""; PortName = ""; PortCode = ""; SupplierComment = ""; PayTerms = ""; PackingCost = ""; FreightCharge = "";
            GrandTotal = ""; Allowance = ""; TotalLineItemsAmount = ""; eSupp_SuppAddCode = ""; buyer_Link_code = ""; VendorStatus = ""; MsgNumber = ""; MsgRefNumber = ""; Attachment_Inbox = "";
            IsDecline = false; IsQuote = false;
            IsAltItemAllowed = 0; IsPriceAveraged = 0; IsUOMChanged = 0; count = 0;cFilter=0;cMax=2;
        }

        public void LoadInterchangeDetails()
        {
            try
            {
                _netWrapper.LogText = "Started Loading interchange object.";
                if (_interchange != null)
                {
                    if (_interchange.Recipient != null)
                        BuyerCode = _interchange.Recipient;

                    if (_interchange.Sender != null)
                        SupplierCode = _interchange.Sender;

                    if (_interchange.DocumentHeader.DocType != null)
                        DocType = _interchange.DocumentHeader.DocType;

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
                            supplierName = _interchange.DocumentHeader.PartyAddresses[j].Name;
                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts != null)
                            {
                                if (_interchange.DocumentHeader.PartyAddresses[j].Contacts.Count > 0)
                                {
                                    if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count > 0)
                                    {
                                        for (int k = 0; k < _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count; k++)
                                        {
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.TE)
                                                supplierPhone = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.EM)
                                                supplierEmail = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.FX)
                                                supplierFax = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
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
                            else if(_interchange.DocumentHeader.Comments[i].Qualifier==CommentTypes.ZTC)//09-02-2018//rotterdam req
                                DeliveryTerms = _interchange.DocumentHeader.Comments[i].Value;
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
                                { DateTime dtDocDate = FormatMTMLDate(_interchange.DocumentHeader.DateTimePeriods[i].Value); }
                            }

                            else if (_interchange.DocumentHeader.DateTimePeriods[i].Qualifier == DateTimePeroidQualifiers.DeliveryDate_69)
                            {
                                if (_interchange.DocumentHeader.DateTimePeriods[i].Value != null)
                                {
                                    DateTime dtDelDate = FormatMTMLDate(_interchange.DocumentHeader.DateTimePeriods[i].Value);
                                    if (dtDelDate != DateTime.MinValue)
                                    {
                                        DtDelvDate = dtDelDate.ToString("MM/dd/yyyy");
                                    }
                                    if (dtDelDate == null)
                                    {
                                        DateTime dt = FormatMTMLDate(DateTime.Now.AddDays(Convert.ToDouble(LeadDays)).ToString());
                                        if (dt != DateTime.MinValue)
                                        {
                                            DtDelvDate = dt.ToString("MM/dd/yyyy");
                                        }
                                    }
                                }
                            }

                            if (_interchange.DocumentHeader.DateTimePeriods[i].Qualifier == DateTimePeroidQualifiers.ExpiryDate_36)
                            {
                                if (_interchange.DocumentHeader.DateTimePeriods[i].Value != null)
                                {
                                    DateTime ExpDate = FormatMTMLDate(_interchange.DocumentHeader.DateTimePeriods[i].Value);
                                    if (ExpDate != DateTime.MinValue)
                                    {
                                        dtExpDate = ExpDate.ToString("dd-MMM-yyyy");//.ToString("MM/dd/yyyy");
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    _netWrapper.LogText = "stopped Loading interchange object.";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception on LoadInterchangeDetails : " + ex.GetBaseException().ToString());
            }
        }

        public void LoadQuoteTable(string MTML_QuoteFile)
        {
            try
            {
                string Quote_File = Quote_UploadPath + @"\" + MsgNumber;//added on 31-7-2017
                /*filter by RFQ No*/
                if (Filter_Quote_By_RFQNo(UCRefNo))
                {
                    DOMElement AllRFQTable = _netWrapper.GetElementbyClass(pagevar.tblPendingRFQClass);
                    if (AllRFQTable != null)
                    {
                        DOMElement eletbody = AllRFQTable.GetElementByTagName("tbody");
                        if (eletbody != null)
                        {
                            List<DOMNode> eleRow = eletbody.GetElementsByTagName("tr");
                            if (eleRow.Count > 0)
                            {
                                if (eleRow.Count > 1)
                                {
                                    string message = "multiple records found for same VRNo " + UCRefNo;
                                    _netWrapper.LogText = message;
                                    Process_MultipleRecordForSameVRNO(eleRow, MTML_QuoteFile);//added on 25-04-2017
                                }
                                else
                                {
                                    foreach (DOMNode erow in eleRow)
                                    {
                                        DOMElement eRow = (DOMElement)erow;
                                        string DeliveryPort = "", RFQStatus = "";
                                        try
                                        {
                                            string RFQNo = GetGrid_RfqNo(eRow);
                                            if (RFQNo != "")
                                            {
                                                if (RFQNo == UCRefNo)
                                                {
                                                    /*check vendor,rfq status is valid or not */
                                                    if (ifValidStatus(eRow, UCRefNo, out DeliveryPort, out RFQStatus))
                                                    {
                                                        List<DOMNode> _data = eRow.GetElementsByTagName("td");
                                                        if (_data.Count > 0)
                                                        {
                                                            try
                                                            {
                                                                processQuoteFile(MTML_QuoteFile);
                                                                IsLoad = true;
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                throw new Exception("Unable to process Quote file for VRNo. '" + UCRefNo + "'" + ex.GetBaseException().ToString());
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Thread.Sleep(2000);
                                                        if (VendorStatus.ToUpper().Contains("QUOTE") || VendorStatus.ToUpper().Contains("DRAFT") || VendorStatus.ToUpper().Contains("REJECT") || RFQStatus.ToUpper() == "CLOSED")
                                                        {
                                                            string xlsQuote_File = Quote_UploadPath + @"\" + MsgNumber;
                                                            GetScreenShots(UCRefNo, RFQStatus, VendorStatus, MTML_QuoteFile, xlsQuote_File);
                                                            break;
                                                        }
                                                        else _netWrapper.LogText = "Quote for VRNo '" + RFQNo + "' has RFQ Status as " + RFQStatus + " and Vendor Status as " + VendorStatus;//B
                                                    }
                                                }
                                            }
                                            else _netWrapper.LogText = "Unable to get RFQNo from table row";
                                        }
                                        catch (Exception ex)
                                        {
                                            MoveFileToError_WithoutScreenShot(MTML_QuoteFile, "Exception on LoadQuotetable: " + ex.ToString(), Quote_File);//31-7-2017
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MoveFileToError(MTML_QuoteFile, "Data not available in pending tab.", Quote_File); //23-12-2017
                            }
                        }
                        else _netWrapper.LogText = "Quote table body not loaded.";
                    }
                    else _netWrapper.LogText = "Quote table not loaded.";
                }
                else
                {
                    string sFile = ScreenShotPath + "\\BS_Quote_" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
                    _netWrapper.PrintPDF(sFile, false);
                    _netWrapper.LogText = "Unable to filter quote after 3 attempts for no " + UCRefNo;
                    CreateQuoteAuditFile(sFile, "Bernhard Schulte Quote", UCRefNo, "Error", "Unable to filter quote after 3 attepts for no " + UCRefNo);
                    //MoveFileToError(MTML_QuoteFile, "Unable to filter quote after 3 attepts for no " + UCRefNo, Quote_File); 
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Process_MultipleRecordForSameVRNO(List<DOMNode> eleRow, string MTML_QuoteFile)
        {
            foreach (DOMNode erow in eleRow)
            {
                DOMElement eRow = (DOMElement)erow;
                string DeliveryPort = "", RFQStatus = "";
                try
                {
                    //if (File.Exists(Quote_UploadPath + @"\" + MsgNumber))
                    //{
                    if (File.Exists(MTML_QuoteFile))
                    {
                        string RFQNo = GetGrid_RfqNo(eRow);
                        if (RFQNo != "")
                        {
                            if (RFQNo == UCRefNo)
                            {
                                ///*check vendor,rfq status is valid or not */
                                //if (ifValidStatus(eRow, UCRefNo, out DeliveryPort, out RFQStatus))
                                //{
                                if (Get_Port(eRow, out DeliveryPort))
                                {
                                    if (PortCode.ToUpper().Contains(DeliveryPort.ToUpper()) || DeliveryPort.ToUpper().Contains(PortCode.ToUpper()))//changed PortName to portCode//08052017
                                    {
                                        /*check vendor,rfq status is valid or not */
                                        if (ifValidStatus(eRow, UCRefNo, out DeliveryPort, out RFQStatus))
                                        {
                                            List<DOMNode> _data = eRow.GetElementsByTagName("td");
                                            if (_data.Count > 0)
                                            {
                                                try
                                                {
                                                    var btns = _data[11].GetElementsByTagName("button");//31-7-2017
                                                    DOMElement BTN = null;

                                                    if (btns.Count > 0)
                                                    {
                                                        BTN = (DOMElement)btns[2];
                                                    }
                                                    if (BTN != null)
                                                    {
                                                        if (BTN.HasAttribute("class"))
                                                        {
                                                            if (BTN.GetAttribute("class") == pagevar.btnQuoteClass)
                                                            {
                                                                try
                                                                {
                                                                    BTN.Click();
                                                                    _netWrapper.WaitForElementbyId(pagevar.contactRemarksID);
                                                                    Thread.Sleep(20000);
                                                                    _netWrapper.LogText = ("Processing started for quote  '" + UCRefNo + "'");

                                                                    /*Open quote page*/
                                                                    ImportQuote(MTML_QuoteFile);
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    throw new Exception("Exception while importing quote file for VRNo. '" + UCRefNo + "," + ex.GetBaseException().ToString());
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        _netWrapper.LogText = "Unable to click on quote button,Retry";
                                                    }
                                                    // processQuoteFile(MTML_QuoteFile);//commented on 23-05-2017(not picking second row,only picking first row)
                                                    IsLoad = true;
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw new Exception("Unable to process Quote file for VRNo. '" + UCRefNo + "'" + ex.GetBaseException().ToString());
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Thread.Sleep(2000);
                                            if (VendorStatus.ToUpper().Contains("QUOTE") || VendorStatus.ToUpper().Contains("DRAFT") || VendorStatus.ToUpper().Contains("REJECT") || RFQStatus.ToUpper() == "CLOSED")
                                            {
                                                string xlsQuote_File = Quote_UploadPath + @"\" + MsgNumber;
                                                GetScreenShots(UCRefNo, RFQStatus, VendorStatus, MTML_QuoteFile, xlsQuote_File);
                                                break;
                                            }
                                            else _netWrapper.LogText = "Quote for VRNo '" + RFQNo + "' has RFQ Status as " + RFQStatus + " and Vendor Status as " + VendorStatus;//A
                                        }
                                    }
                                }
                                //}
                                //else
                                //{
                                //    Thread.Sleep(2000);
                                //    if (VendorStatus.ToUpper().Contains("QUOTE") || VendorStatus.ToUpper().Contains("DRAFT") || VendorStatus.ToUpper().Contains("REJECT") || RFQStatus.ToUpper() == "CLOSED")
                                //    {
                                //        string xlsQuote_File = Quote_UploadPath + @"\" + MsgNumber;
                                //        GetScreenShots(UCRefNo, RFQStatus, VendorStatus, MTML_QuoteFile, xlsQuote_File);
                                //        break;
                                //    }
                                //    else _netWrapper.LogText = "Quote for VRNo '" + RFQNo + "' has RFQ Status as " + RFQStatus + " and Vendor Status as " + VendorStatus;
                                //}
                            }
                        }
                    }
                    //}
                    //else
                    //{
                    //    _netWrapper.LogText = Quote_UploadPath + @"\" + MsgNumber + " not found in quote upload path for VRNo " + UCRefNo;
                    //    CreateQuoteAuditFile(Quote_UploadPath + @"\" + MsgNumber, "Bernhard Schulte Quote", UCRefNo, "Error", Quote_UploadPath + @"\" + MsgNumber + " not found in quote upload path for VRNo " + UCRefNo);
                    //}

                }
                catch (Exception ex)
                { _netWrapper.LogText = "Exception on Process_MultipleRecordForSameVRNO: " + ex.ToString(); }
            }
        }
   

        public bool Filter_Quote_By_RFQNo(string RFQNo)
        {
            bool result = false; 
            _netWrapper.LogText = "Filter Quote Table started. for VRNo." + RFQNo;
            DOMInputElement _inputSearch = (DOMInputElement)_netWrapper.GetElementByType("input", "search");
            if (IsLoad)
            {
                _inputSearch.Value = string.Empty;
                _inputSearch.Focus();//21-7-2017
                Thread.Sleep(1000);//21-7-2017
            }

            for (int j = 0; j < RFQNo.Length; j++)
            {
                if (checkChar_AlphaNumeric(RFQNo[j]))
                {
                    SetAlphaNumeric(RFQNo[j]);
                    Thread.Sleep(500);
                }
                else
                {
                    KeyParams paramers = new KeyParams(VirtualKeyCode.OEM_2, '/');
                    _netWrapper.browser.KeyDown(paramers);
                    _netWrapper.browser.KeyUp(paramers);
                    Thread.Sleep(500);
                }
            }
            string RFQNoText = _inputSearch.Value;
            if (RFQNoText != "")
            { _netWrapper.LogText = "Filter Quote Table completed."; result = true; }//09-11-2017
            else
            {
               // _netWrapper.LogText = "Unable to filter Quote Table.";//09-11-2017
                cFilter++;//09-11-2017
                if (cFilter <= cMax)//09-11-2017
                {
                    Clear_SearchBox();
                    Filter_Quote_By_RFQNo(RFQNo);
                }
                else result = false;//09-11-2017
            }
            IsLoad = false;
            return result;//09-11-2017
        }

        public void Clear_SearchBox()
        {
            DOMInputElement _inputSearch = (DOMInputElement)_netWrapper.GetElementByType("input", "search");
            if (_inputSearch != null)
            {
                _inputSearch.Value = "";
                KeyParams paramers = new KeyParams(VirtualKeyCode.RETURN, '\n');
                _netWrapper.browser.KeyDown(paramers);
                _netWrapper.browser.KeyUp(paramers);
                Thread.Sleep(500);
            }
        }

        public string GetGrid_RfqNo(DOMElement eRow)
        {
            string RFQNo = "";
            List<DOMNode> _data = eRow.GetElementsByTagName("td");
            if (_data.Count > 0)
            {
                var spans = _data[0].GetElementsByTagName("span");
                if (spans.Count > 0)
                {
                    RFQNo = spans[1].TextContent.Trim();
                }
            }
            return RFQNo;
        }

        /*chek status is valid or not*/
        public bool ifValidStatus(DOMElement eRow, string RFQNo, out string DeliveryPort, out string RFQStatus)
        {
            DeliveryPort = ""; RFQStatus = "";
            List<DOMNode> _data = eRow.GetElementsByTagName("td");
            if (_data.Count > 0)
            {
                DeliveryPort = _data[7].TextContent;

                RFQStatus = _data[0].TextContent.Split(' ')[4].Trim();

                VendorStatus = ((DOMElement)(_data[0].Children[0])).Attributes["title"].ToString();//changed on 31-7-2017
                //string status = ((DOMElement)(_data[9].Children[7])).Attributes["data-bind"].ToString();//31-7-2017
                //VendorStatus = status.Split(',')[4].Trim();
                //VendorStatus = VendorStatus.Substring(1, VendorStatus.Length - 5);

                if (VendorStatus.ToUpper().Contains("PENDING") && RFQStatus.ToUpper() == "OPEN")
                    return true;
                else
                {
                    _data.Clear();
                    return false;
                }
            }
            else { _data.Clear(); return false; }
        }

        public bool Get_Port(DOMElement eRow, out string DeliveryPort)
        {
            DeliveryPort = "";
            List<DOMNode> _data = eRow.GetElementsByTagName("td");
            if (_data.Count > 0)
            {
                //DeliveryPort = _data[6].TextContent;
                DeliveryPort = _data[7].TextContent;
            }
            if (DeliveryPort == "" || DeliveryPort.Length > 0)
                return true;
            else return false;
        }

        /*check port is valid or not*/
        public bool isValidPort(string DeliveryPort)
        {
            if (PortCode == null) PortCode = "";
            if (PortCode.ToUpper() == DeliveryPort.ToUpper())
                return true;
            else return false;
        }

        public void processQuoteFile(string MTML_QuoteFile)
        {
            bool result = false;
            int maxcount = 2, count = 0; ;
            /*click on Create Quote button*/
            if (_netWrapper.ClickElementbyClass(pagevar.btnQuoteClass))
            {
                result = true;
                try
                {
                    _netWrapper.WaitForElementbyId(pagevar.contactRemarksID);
                    Thread.Sleep(20000);
                    _netWrapper.LogText = ("Processing started for quote  '" + UCRefNo + "'");
                    /*Open quote page*/
                    ImportQuote(MTML_QuoteFile);
                }
                catch (Exception ex)
                {
                    throw new Exception("Exception while importing quote file for VRNo. '" + UCRefNo + "," + ex.GetBaseException().ToString());
                }
            }
            else
            {
                _netWrapper.LogText = "Unable to click on quote button,Retry";
            }
            if (!result)
            {
                count++;
                if (count <= maxcount)
                    processQuoteFile(MTML_QuoteFile);
                else
                {
                    MoveFileToError(MTML_QuoteFile, "Unable to click on quote button for VRNo.'" + UCRefNo + "'", Quote_UploadPath + @"\" + MsgNumber);
                }
            }
        }

        public void ImportQuote(string MTML_QuoteFile)
        {
            int maxcount = 1, count = 0;
            string xlsQuote_File = "", msg = "";
            try
            {
                DOMElement btnImport = _netWrapper.GetElementbyId(pagevar.btnImportID);
                if (btnImport != null)
                {
                    xlsQuote_File = Quote_UploadPath + @"\" + MsgNumber;
                    if (File.Exists(xlsQuote_File))
                    {
                        ///* upload file */
                        bool result = _netWrapper.SetFile(pagevar.btnImportID, xlsQuote_File, false, false);
                        if (result)
                        {
                            if (Convert.ToBoolean(ConfigurationManager.AppSettings["IS_INVALID_RFQ"].Trim()))
                            {
                                msg = loading();
                                if (msg != "" && !msg.Contains("Successfully imported RFQ"))
                                {
                                    MoveTo_Error("Unable to process file for ref no " + UCRefNo + " due to " + msg, MTML_QuoteFile, xlsQuote_File, "");

                                    DOMInputElement _btnCancel = (DOMInputElement)_netWrapper.GetElementByType("input", "click: onCancelQuoteCreation,enable:hasEnableButton", "data-bind");//10-04-2017
                                    if (_btnCancel != null)
                                    {
                                        _btnCancel.Click();
                                        _netWrapper.WaitForElementbyId(pagevar.ulEnquiryClass, true, false);
                                        if (!FilterInQuotedTab(MTML_QuoteFile, xlsQuote_File))
                                        {
                                            GetPendingRFQTab();
                                            Thread.Sleep(10000);
                                        }
                                    }
                                    else _netWrapper.LogText = "SubmitQuote Cancel button is null.";
                                }
                            }
                            if (msg==""||msg.Contains("Successfully imported RFQ"))
                            {
                                _netWrapper.LogText = "Quote for VRNo " + UCRefNo + " imported.";
                                Thread.Sleep(30000);
                                if (Fill_Price_Validity_Date(MTML_QuoteFile))
                                {
                                    if (SetLeadDays(xlsQuote_File))
                                    {
                                        /*check declied true or false*/
                                        if (!IsDecline)
                                        {
                                            /* if mtml grand total zero,decline quote*/
                                            if (Convert.ToInt32(Convert.ToDouble(GrandTotal)) != 0)//17-05-2017
                                            {
                                                if (Apply_Discount_On_Items(MTML_QuoteFile))
                                                {
                                                    DOMElement liGrandTotal = _netWrapper.GetElementbyClass(pagevar.liGrandTotal);
                                                    if (liGrandTotal != null)
                                                    {
                                                        DOMElement _span = _netWrapper.GetElementByType("span", liGrandTotal);
                                                        if (_span != null)
                                                        {
                                                            Double GTotal = Convert.ToDouble(_span.TextContent);
                                                            Double GTotal1 = Convert.ToDouble(_span.InnerText);

                                                            DOMElement _chk = _netWrapper.GetElementbyClass("col-md-12 total-bx text-right total-bx");
                                                            var _str_tot = _chk.InnerText;
                                                            DOMElement _str1 = (DOMElement)_span.Parent.Parent;
                                                            string _str = _str1.InnerHTML;
                                                            if (Convert.ToInt32(Convert.ToDouble(GrandTotal)) == Convert.ToInt32(GTotal))
                                                            {
                                                                DOMInputElement _btnDraft = (DOMInputElement)_netWrapper.GetElementByType("input", "Save as Draft", "value");
                                                                if (_btnDraft != null)
                                                                {
                                                                    //if (Username.ToUpper().Trim() == "SUPPLY@NEKOSHIP.NL")//17-05-2017
                                                                    if (IsDraftQuote)//changed by Kalpita on 19/12/2019
                                                                    {
                                                                        bool res = DraftQuote(MTML_QuoteFile, xlsQuote_File, _btnDraft);//17-05-2017

                                                                        #region Commented
                                                                        //if (ConfigurationManager.AppSettings["SUBMIT_QUOTE"].ToUpper().Trim() == "TRUE")
                                                                        //{
                                                                        //    if (res)//remove comment after approval from neko
                                                                        //    {
                                                                        //        process_Drafted_Quote(MTML_QuoteFile, xlsQuote_File);//17-05-2017
                                                                        //    }
                                                                        //}
                                                                        #endregion
                                                                    }

                                                                    //changed by Kalpita on 19/12/2019                                                                       
                                                                    if (IsSubmitQuote) { SubmitQuote(MTML_QuoteFile, xlsQuote_File); }
                                                                }

                                                                #region draft button code
                                                                //DOMInputElement _btnDraft = (DOMInputElement)_netWrapper.GetElementByType("input", "Save as Draft", "value");
                                                                //if (_btnDraft != null)
                                                                //{
                                                                //    _btnDraft.Click();
                                                                //    Thread.Sleep(1000);
                                                                //    DOMElement _divPopoup = _netWrapper.GetElementbyClass(pagevar.DivPopup);
                                                                //    if (_divPopoup != null)
                                                                //    {
                                                                //        DOMElement _spanMsg = _netWrapper.GetElementByType("span", _divPopoup);
                                                                //        if (_spanMsg != null)
                                                                //        {
                                                                //            msg = _spanMsg.TextContent;
                                                                //            res = true;
                                                                //        }
                                                                //    }
                                                                //    res = Process_After_Draft(MTML_QuoteFile,xlsQuote_File);

                                                                // }
                                                                //  else
                                                                //  {

                                                                //  }
                                                                #endregion
                                                            }
                                                            else if (BuyerTotal != "")//16-12-2017
                                                            {
                                                                if (Convert.ToInt32(Convert.ToDouble(BuyerTotal)) == Convert.ToInt32(GTotal))
                                                                {
                                                                    DOMInputElement _btnDraft = (DOMInputElement)_netWrapper.GetElementByType("input", "Save as Draft", "value");
                                                                    if (_btnDraft != null)
                                                                    {
                                                                        //if (Username.ToUpper().Trim() == "SUPPLY@NEKOSHIP.NL")//17-05-2017
                                                                        if (IsDraftQuote)//changed by Kalpita on 19/12/2019
                                                                        {
                                                                            bool res = DraftQuote(MTML_QuoteFile, xlsQuote_File, _btnDraft);//17-05-2017

                                                                            #region Commented
                                                                            //if (ConfigurationManager.AppSettings["SUBMIT_QUOTE"].ToUpper().Trim() == "TRUE")
                                                                            //{
                                                                            //    if (res)//remove comment after approval from neko
                                                                            //    {
                                                                            //        process_Drafted_Quote(MTML_QuoteFile, xlsQuote_File);//17-05-2017
                                                                            //    }
                                                                            //}
                                                                            #endregion
                                                                        }
                                                                        //changed by Kalpita on 19/12/2019                                                                       
                                                                        if (IsSubmitQuote) { SubmitQuote(MTML_QuoteFile, xlsQuote_File); }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    MoveFileToError(MTML_QuoteFile, "Unable to upload quote for Ref '" + UCRefNo + "' due to Buyer Total '" + BuyerTotal + "' mismatch on site Grand Total '" + GTotal + "'.", xlsQuote_File);
                                                                    Thread.Sleep(500);
                                                                }
                                                            }//16-12-2017
                                                            else
                                                            {
                                                                MoveFileToError(MTML_QuoteFile, "Unable to upload Quote for Ref : '" + UCRefNo + "' due to Total Amount '" + GrandTotal + "' mismatch on site Grand Total '" + GTotal + "'.", xlsQuote_File);
                                                                Thread.Sleep(500);

                                                                // DOMInputElement _btnCancel = (DOMInputElement)_netWrapper.GetElementByType("input", "click: onCancelQuoteCreation", "data-bind");//10-04-2017
                                                                DOMInputElement _btnCancel = (DOMInputElement)_netWrapper.GetElementByType("input", "click: onCancelQuoteCreation,enable:hasEnableButton", "data-bind");//10-04-2017
                                                                if (_btnCancel != null)
                                                                {
                                                                    _btnCancel.Click();
                                                                    _netWrapper.WaitForElementbyId(pagevar.ulEnquiryClass, true, false);//3-4-2017
                                                                    // GetAllRFQTab();
                                                                    GetPendingRFQTab();//09-11-2017
                                                                    Thread.Sleep(10000);
                                                                }
                                                                else _netWrapper.LogText = "ImportQuote Cancel button is null.";//10-04-2017
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                IsDeclineQuote(xlsQuote_File, MTML_QuoteFile);
                                            }
                                        }
                                        else
                                        {
                                            IsDeclineQuote(xlsQuote_File, MTML_QuoteFile);
                                        }
                                    }
                                    else
                                    {
                                        MoveFileToError(MTML_QuoteFile, "Unable to set LeadDays for Ref No. " + UCRefNo, Quote_UploadPath + @"\" + MsgNumber);
                                        DOMInputElement _btnCancel = (DOMInputElement)_netWrapper.GetElementByType("input", "click: onCancelQuoteCreation,enable:hasEnableButton", "data-bind");//10-04-2017
                                        if (_btnCancel != null)
                                        {
                                            _btnCancel.Click();
                                            _netWrapper.WaitForElementbyId(pagevar.ulEnquiryClass, true, false);
                                            if (!FilterInQuotedTab(MTML_QuoteFile, xlsQuote_File))
                                            {
                                                GetPendingRFQTab();
                                                Thread.Sleep(10000);
                                            }
                                        }
                                        else _netWrapper.LogText = "SubmitQuote Cancel button is null.";
                                    }
                                }
                                else
                                {
                                    MoveFileToError(MTML_QuoteFile, "Unable to set Price validity date for Ref No. " + UCRefNo, Quote_UploadPath + @"\" + MsgNumber);
                                    DOMInputElement _btnCancel = (DOMInputElement)_netWrapper.GetElementByType("input", "click: onCancelQuoteCreation,enable:hasEnableButton", "data-bind");//10-04-2017
                                    if (_btnCancel != null)
                                    {
                                        _btnCancel.Click();
                                        _netWrapper.WaitForElementbyId(pagevar.ulEnquiryClass, true, false);
                                        if (!FilterInQuotedTab(MTML_QuoteFile, xlsQuote_File))
                                        {
                                            GetPendingRFQTab();
                                            Thread.Sleep(10000);
                                        }
                                    }
                                    else _netWrapper.LogText = "SubmitQuote Cancel button is null.";
                                }
                            }
                        }
                        else /*if file not uploaded successfully,perform same for two times*/
                        {
                            count++;
                            if (count <= maxcount)
                            {
                                ImportQuote(MTML_QuoteFile);
                            }
                            else
                            {
                                MoveFileToError_WithoutScreenShot(MTML_QuoteFile, "Unable to upload Quote File '" + MTML_QuoteFile + "' for VRNo." + UCRefNo, Quote_UploadPath + @"\" + MsgNumber);
                            }
                        }
                    }
                    else
                    {
                        // Upload Quote on next scheduler //
                        _netWrapper.LogText = "Excel Quote file '" + Path.GetFileName(xlsQuote_File) + "' not found on " + Quote_UploadPath;
                    }
                }
                else
                {
                    _netWrapper.LogText = "Import button not found for uploading Quote file for VRNo. " + UCRefNo;
                }
            }
            catch (Exception ex)
            {
                _netWrapper.LogText = "Error in ImportQuote() - " + ex.Message;
                _netWrapper.LogText = ex.StackTrace;
                throw ex;
            }
        }

        #region commented 19/12/2019
        //public void ImportQuote(string MTML_QuoteFile)
        //{
        //    int maxcount = 1, count = 0;
        //    string xlsQuote_File = "", msg = "";
        //    try
        //    {
        //        DOMElement btnImport = _netWrapper.GetElementbyId(pagevar.btnImportID);
        //        if (btnImport != null)
        //        {
        //            xlsQuote_File = Quote_UploadPath + @"\" + MsgNumber;
        //            if (File.Exists(xlsQuote_File))
        //            {
        //                ///* upload file */
        //                bool result = _netWrapper.SetFile(pagevar.btnImportID, xlsQuote_File, false, false);
        //                if (result)
        //                {
        //                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["IS_INVALID_RFQ"].Trim()))
        //                    {
        //                        msg = loading();
        //                        if (msg != "" && !msg.Contains("Successfully imported RFQ"))
        //                        {
        //                            MoveTo_Error("Unable to process file for ref no " + UCRefNo + " due to " + msg, MTML_QuoteFile, xlsQuote_File, "");

        //                            DOMInputElement _btnCancel = (DOMInputElement)_netWrapper.GetElementByType("input", "click: onCancelQuoteCreation,enable:hasEnableButton", "data-bind");//10-04-2017
        //                            if (_btnCancel != null)
        //                            {
        //                                _btnCancel.Click();
        //                                _netWrapper.WaitForElementbyId(pagevar.ulEnquiryClass, true, false);
        //                                if (!FilterInQuotedTab(MTML_QuoteFile, xlsQuote_File))
        //                                {
        //                                    GetPendingRFQTab();
        //                                    Thread.Sleep(10000);
        //                                }
        //                            }
        //                            else _netWrapper.LogText = "SubmitQuote Cancel button is null.";
        //                        }
        //                    }
        //                    if (msg == "" || msg.Contains("Successfully imported RFQ"))
        //                    {
        //                        _netWrapper.LogText = "Quote for VRNo " + UCRefNo + " imported.";
        //                        Thread.Sleep(30000);
        //                        if (Fill_Price_Validity_Date(MTML_QuoteFile))
        //                        {
        //                            if (SetLeadDays(xlsQuote_File))
        //                            {
        //                                /*check declied true or false*/
        //                                if (!IsDecline)
        //                                {
        //                                    /* if mtml grand total zero,decline quote*/
        //                                    if (Convert.ToInt32(Convert.ToDouble(GrandTotal)) != 0)//17-05-2017
        //                                    {
        //                                        if (Apply_Discount_On_Items(MTML_QuoteFile))
        //                                        {
        //                                            DOMElement liGrandTotal = _netWrapper.GetElementbyClass(pagevar.liGrandTotal);
        //                                            if (liGrandTotal != null)
        //                                            {
        //                                                DOMElement _span = _netWrapper.GetElementByType("span", liGrandTotal);
        //                                                if (_span != null)
        //                                                {
        //                                                    Double GTotal = Convert.ToDouble(_span.TextContent);
        //                                                    if (Convert.ToInt32(Convert.ToDouble(GrandTotal)) == Convert.ToInt32(GTotal))
        //                                                    {
        //                                                        DOMInputElement _btnDraft = (DOMInputElement)_netWrapper.GetElementByType("input", "Save as Draft", "value");
        //                                                        if (_btnDraft != null)
        //                                                        {
        //                                                            //if (Username.ToUpper().Trim() == "SUPPLY@NEKOSHIP.NL")//17-05-2017
        //                                                            if (IsDraftQuote)//changed by Kalpita on 19/12/2019
        //                                                            {
        //                                                                bool res = DraftQuote(MTML_QuoteFile, xlsQuote_File, _btnDraft);//17-05-2017

        //                                                                #region Commented
        //                                                                //if (ConfigurationManager.AppSettings["SUBMIT_QUOTE"].ToUpper().Trim() == "TRUE")
        //                                                                //{
        //                                                                //    if (res)//remove comment after approval from neko
        //                                                                //    {
        //                                                                //        process_Drafted_Quote(MTML_QuoteFile, xlsQuote_File);//17-05-2017
        //                                                                //    }
        //                                                                //}
        //                                                                #endregion
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                //changed by Kalpita on 19/12/2019                                                                       
        //                                                                if (IsSubmitQuote) { SubmitQuote(MTML_QuoteFile, xlsQuote_File); }
        //                                                            }
        //                                                        }

        //                                                        #region draft button code
        //                                                        //DOMInputElement _btnDraft = (DOMInputElement)_netWrapper.GetElementByType("input", "Save as Draft", "value");
        //                                                        //if (_btnDraft != null)
        //                                                        //{
        //                                                        //    _btnDraft.Click();
        //                                                        //    Thread.Sleep(1000);
        //                                                        //    DOMElement _divPopoup = _netWrapper.GetElementbyClass(pagevar.DivPopup);
        //                                                        //    if (_divPopoup != null)
        //                                                        //    {
        //                                                        //        DOMElement _spanMsg = _netWrapper.GetElementByType("span", _divPopoup);
        //                                                        //        if (_spanMsg != null)
        //                                                        //        {
        //                                                        //            msg = _spanMsg.TextContent;
        //                                                        //            res = true;
        //                                                        //        }
        //                                                        //    }
        //                                                        //    res = Process_After_Draft(MTML_QuoteFile,xlsQuote_File);

        //                                                        // }
        //                                                        //  else
        //                                                        //  {

        //                                                        //  }
        //                                                        #endregion
        //                                                    }
        //                                                    else if (BuyerTotal != "")//16-12-2017
        //                                                    {
        //                                                        if (Convert.ToInt32(Convert.ToDouble(BuyerTotal)) == Convert.ToInt32(GTotal))
        //                                                        {
        //                                                            DOMInputElement _btnDraft = (DOMInputElement)_netWrapper.GetElementByType("input", "Save as Draft", "value");
        //                                                            if (_btnDraft != null)
        //                                                            {
        //                                                                //if (Username.ToUpper().Trim() == "SUPPLY@NEKOSHIP.NL")//17-05-2017
        //                                                                if (IsDraftQuote)//changed by Kalpita on 19/12/2019
        //                                                                {
        //                                                                    bool res = DraftQuote(MTML_QuoteFile, xlsQuote_File, _btnDraft);//17-05-2017

        //                                                                    #region Commented
        //                                                                    //if (ConfigurationManager.AppSettings["SUBMIT_QUOTE"].ToUpper().Trim() == "TRUE")
        //                                                                    //{
        //                                                                    //    if (res)//remove comment after approval from neko
        //                                                                    //    {
        //                                                                    //        process_Drafted_Quote(MTML_QuoteFile, xlsQuote_File);//17-05-2017
        //                                                                    //    }
        //                                                                    //}
        //                                                                    #endregion
        //                                                                }
        //                                                                else
        //                                                                { //changed by Kalpita on 19/12/2019                                                                       
        //                                                                    if (IsSubmitQuote) { SubmitQuote(MTML_QuoteFile, xlsQuote_File); }
        //                                                                }
        //                                                            }
        //                                                        }
        //                                                        else
        //                                                        {
        //                                                            MoveFileToError(MTML_QuoteFile, "Unable to upload quote for Ref '" + UCRefNo + "' due to Total Amount '" + GrandTotal + "' mismatch on site Grand Total '" + GTotal + "'.", xlsQuote_File);
        //                                                            Thread.Sleep(500);
        //                                                        }
        //                                                    }//16-12-2017
        //                                                    else
        //                                                    {
        //                                                        MoveFileToError(MTML_QuoteFile, "Unable to upload Quote for Ref : '" + UCRefNo + "' due to Total Amount '" + GrandTotal + "' mismatch on site Grand Total '" + GTotal + "'.", xlsQuote_File);
        //                                                        Thread.Sleep(500);

        //                                                        // DOMInputElement _btnCancel = (DOMInputElement)_netWrapper.GetElementByType("input", "click: onCancelQuoteCreation", "data-bind");//10-04-2017
        //                                                        DOMInputElement _btnCancel = (DOMInputElement)_netWrapper.GetElementByType("input", "click: onCancelQuoteCreation,enable:hasEnableButton", "data-bind");//10-04-2017
        //                                                        if (_btnCancel != null)
        //                                                        {
        //                                                            _btnCancel.Click();
        //                                                            _netWrapper.WaitForElementbyId(pagevar.ulEnquiryClass, true, false);//3-4-2017
        //                                                            // GetAllRFQTab();
        //                                                            GetPendingRFQTab();//09-11-2017
        //                                                            Thread.Sleep(10000);
        //                                                        }
        //                                                        else _netWrapper.LogText = "ImportQuote Cancel button is null.";//10-04-2017
        //                                                    }
        //                                                }
        //                                            }
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        IsDeclineQuote(xlsQuote_File, MTML_QuoteFile);
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    IsDeclineQuote(xlsQuote_File, MTML_QuoteFile);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                MoveFileToError(MTML_QuoteFile, "Unable to set LeadDays for Ref No. " + UCRefNo, Quote_UploadPath + @"\" + MsgNumber);
        //                                DOMInputElement _btnCancel = (DOMInputElement)_netWrapper.GetElementByType("input", "click: onCancelQuoteCreation,enable:hasEnableButton", "data-bind");//10-04-2017
        //                                if (_btnCancel != null)
        //                                {
        //                                    _btnCancel.Click();
        //                                    _netWrapper.WaitForElementbyId(pagevar.ulEnquiryClass, true, false);
        //                                    if (!FilterInQuotedTab(MTML_QuoteFile, xlsQuote_File))
        //                                    {
        //                                        GetPendingRFQTab();
        //                                        Thread.Sleep(10000);
        //                                    }
        //                                }
        //                                else _netWrapper.LogText = "SubmitQuote Cancel button is null.";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            MoveFileToError(MTML_QuoteFile, "Unable to set Price validity date for Ref No. " + UCRefNo, Quote_UploadPath + @"\" + MsgNumber);
        //                            DOMInputElement _btnCancel = (DOMInputElement)_netWrapper.GetElementByType("input", "click: onCancelQuoteCreation,enable:hasEnableButton", "data-bind");//10-04-2017
        //                            if (_btnCancel != null)
        //                            {
        //                                _btnCancel.Click();
        //                                _netWrapper.WaitForElementbyId(pagevar.ulEnquiryClass, true, false);
        //                                if (!FilterInQuotedTab(MTML_QuoteFile, xlsQuote_File))
        //                                {
        //                                    GetPendingRFQTab();
        //                                    Thread.Sleep(10000);
        //                                }
        //                            }
        //                            else _netWrapper.LogText = "SubmitQuote Cancel button is null.";
        //                        }
        //                    }
        //                }
        //                else /*if file not uploaded successfully,perform same for two times*/
        //                {
        //                    count++;
        //                    if (count <= maxcount)
        //                    {
        //                        ImportQuote(MTML_QuoteFile);
        //                    }
        //                    else
        //                    {
        //                        MoveFileToError_WithoutScreenShot(MTML_QuoteFile, "Unable to upload Quote File '" + MTML_QuoteFile + "' for VRNo." + UCRefNo, Quote_UploadPath + @"\" + MsgNumber);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                // Upload Quote on next scheduler //
        //                _netWrapper.LogText = "Excel Quote file '" + Path.GetFileName(xlsQuote_File) + "' not found on " + Quote_UploadPath;
        //            }
        //        }
        //        else
        //        {
        //            _netWrapper.LogText = "Import button not found for uploading Quote file for VRNo. " + UCRefNo;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _netWrapper.LogText = "Error in ImportQuote() - " + ex.Message;
        //        _netWrapper.LogText = ex.StackTrace;
        //        throw ex;
        //    }
        //}
        #endregion

        public bool DraftQuote(string MTML_QuoteFile, string xls_QuoteFile, DOMInputElement _btnDraft)
        {
            string msg = "";
            bool result = false;
            try
            {
                //string sFile1 = ScreenShotPath + "\\BS_Quote_BeforeDraft_" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
                //_netWrapper.PrintPDF(sFile1, false);
                _btnDraft.Click();
            }
            catch (Exception ex)
            {
                _netWrapper.LogText = ex.ToString();
                Thread.Sleep(700);
                string sFile = ScreenShotPath + "\\BS_Quote" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
                _netWrapper.PrintPDF(sFile, false);
            }

            msg = loading();

            if (msg.Contains("Drafted successfully"))
            {
                result = true;
                _netWrapper.WaitForElementbyId(pagevar.ulEnquiryClass, true, false);//3-4-2017
                string sFile = ScreenShotPath + "\\BS_Quote_Draft_" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
                _netWrapper.PrintPDF(sFile, false);
                MoveFileToBackup(MTML_QuoteFile, UCRefNo + " Quote drafted successfully", xls_QuoteFile);//18-05-2017//comment afterwards when submit processing starts.
                SendMailNotification(_interchange, "QUOTE", UCRefNo, "SUBMITTED", "Quote '" + UCRefNo + "' submitted successfully.");//02-08-2017
                DOMElement _ele = _netWrapper.GetElementbyClass(pagevar.tblPendingRFQClass);
                DOMInputElement _inputSearch = (DOMInputElement)_netWrapper.GetElementByType("input", "search");
                if (_ele != null && _inputSearch != null)
                {
                    Clear_SearchBox(_inputSearch);

                }
            }
            else
            {
                string sFile = "";
                if (msg != "Unit Price should be less than or equal to Contract Unit Price")//20-12-2017
                {
                     sFile = ScreenShotPath + "\\BS_Quote_" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
                    _netWrapper.PrintPDF(sFile, false);
                }
                DOMInputElement _btnCancel = (DOMInputElement)_netWrapper.GetElementByType("input", "click: onCancelQuoteCreation,enable:hasEnableButton", "data-bind");//10-04-2017
                if (_btnCancel != null)
                {
                    _btnCancel.Click();
                    _netWrapper.WaitForElementbyId(pagevar.ulEnquiryClass, true, false);//3-4-2017
                    if (!FilterInDraftTab(MTML_QuoteFile, xls_QuoteFile))//09-11-2017
                    {
                        if (msg != "")//09-11-2017
                            MoveTo_Error("Unable to draft quote for VRNo " + UCRefNo + " due to " + msg, MTML_QuoteFile, xls_QuoteFile, sFile);
                        else MoveTo_Error("Unable to draft quote for VRNo " + UCRefNo, MTML_QuoteFile, xls_QuoteFile, sFile);
                        // GetAllRFQTab();//09-11-2017
                        GetPendingRFQTab();//09-11-2017
                        Thread.Sleep(10000);
                    }
                }
                else _netWrapper.LogText = "Draft Quote Cancel button is null.";//10-04-2017
            }
            return result;
        }

        public void process_Drafted_Quote(string MTML_QuoteFile, string xls_QuoteFile)
        {
            GetDraftRFQTab();
            Thread.Sleep(10000);
            Filter_Quote_By_RFQNo(UCRefNo);
            DOMElement AllRFQTable = _netWrapper.GetElementbyClass(pagevar.tblPendingRFQClass);
            if (AllRFQTable != null)
            {
                DOMElement eletbody = AllRFQTable.GetElementByTagName("tbody");
                if (eletbody != null)
                {
                    List<DOMNode> eleRow = eletbody.GetElementsByTagName("tr");
                    if (eleRow.Count > 0)
                    {
                        if (eleRow.Count > 1)
                        {
                            string message = "multiple records found for same VRNo " + UCRefNo + " in drafted tab.";
                            _netWrapper.LogText = message;
                            foreach (DOMNode erow in eleRow)
                            {
                                DOMElement eRow = (DOMElement)erow;
                                string RFQNo = GetGrid_RfqNo(eRow);
                                if (RFQNo != "")
                                {
                                    if (RFQNo == UCRefNo)
                                    {
                                        List<DOMNode> _data = eRow.GetElementsByTagName("td");
                                        if (_data.Count > 0)
                                        {
                                            string DeliveryPort = _data[6].TextContent;
                                            if (PortCode.ToUpper().Contains(DeliveryPort.ToUpper()))
                                            {
                                                /*click on Create Quote button*/
                                                if (_netWrapper.ClickElementbyClass(pagevar.btnQuoteClass))
                                                {
                                                    try
                                                    {
                                                        _netWrapper.WaitForElementbyId(pagevar.contactRemarksID);
                                                        Thread.Sleep(20000);
                                                        _netWrapper.LogText = ("Quote details page opened for quote  '" + UCRefNo + "'");
                                                        /*Open quote page*/
                                                       //changed by Kalpita on 19/12/2019                                                                       
                                                        if (IsSubmitQuote) { SubmitQuote(MTML_QuoteFile, xls_QuoteFile); }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        throw new Exception("Exception while clicking on Drafted quote button for VRNo. '" + UCRefNo + "," + ex.GetBaseException().ToString());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (DOMNode erow in eleRow)
                            {
                                DOMElement eRow = (DOMElement)eleRow;
                                string RFQNo = GetGrid_RfqNo(eRow);
                                if (RFQNo != "")
                                {
                                    if (RFQNo == UCRefNo)
                                    {
                                        /*click on Create Quote button*/
                                        if (_netWrapper.ClickElementbyClass(pagevar.btnQuoteClass))
                                        {
                                            try
                                            {
                                                _netWrapper.WaitForElementbyId(pagevar.contactRemarksID);
                                                Thread.Sleep(20000);
                                                _netWrapper.LogText = ("Quote details page opened for quote  '" + UCRefNo + "'");
                                                /*Open quote page*/
                                               //changed by Kalpita on 19/12/2019                                                                       
                                                if (IsSubmitQuote) { SubmitQuote(MTML_QuoteFile, xls_QuoteFile); }
                                            }
                                            catch (Exception ex)
                                            {
                                                throw new Exception("Exception while clicking on Drafted quote button for VRNo. '" + UCRefNo + "," + ex.GetBaseException().ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else _netWrapper.LogText = "Data not available for drafted quote.";
                }
            }
        }

        public void SubmitQuote(string MTML_QuoteFile, string xls_QuoteFile)
        {
            string msg = "";
            DOMInputElement _btnSubmit = (DOMInputElement)_netWrapper.GetElementByType("input", "click: onClickSubmitEnquiry,enable:hasEnableButton,visible:smcButtonHide()&&hasEnableAdmin()", "data-bind");//20-06-2018//click: onClickSubmitEnquiry,enable:hasEnableButton,visible:smcButtonHide
            if (_btnSubmit != null)
            {
                try
                {
                    _btnSubmit.Click();
                }
                catch (Exception ex)
                {
                    _netWrapper.LogText = ex.ToString();
                    Thread.Sleep(700);
                    string sFile = ScreenShotPath + "\\BS_Quote" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
                    _netWrapper.PrintPDF(sFile, false);
                }

                msg = loading();


                if (msg.Contains("Quoted successfully"))
                {
                    _netWrapper.WaitForElementbyId(pagevar.ulEnquiryClass, true, false);//3-4-2017
                    string sFile = ScreenShotPath + "\\BS_Quote_Success_" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
                    _netWrapper.PrintPDF(sFile, false);
                    MoveFileToBackup(MTML_QuoteFile, UCRefNo + " Quote submitted successfully", xls_QuoteFile);
                    SendMailNotification(_interchange, "QUOTE", UCRefNo, "SUBMITTED", "Quote '" + UCRefNo + "' submitted successfully.");//30-05-2018
                    DOMElement _ele = _netWrapper.GetElementbyClass(pagevar.tblPendingRFQClass);
                    DOMInputElement _inputSearch = (DOMInputElement)_netWrapper.GetElementByType("input", "search");
                    if (_ele != null && _inputSearch != null)
                    {
                        Clear_SearchBox(_inputSearch);
                    }
                }
                else
                {
                    string sFile = "";
                    if (msg != "Unit Price should be less than or equal to Contract Unit Price")//20-12-2017
                    {
                         sFile = ScreenShotPath + "\\BS_Quote_" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
                        _netWrapper.PrintPDF(sFile, false);
                    }
                    DOMInputElement _btnCancel = (DOMInputElement)_netWrapper.GetElementByType("input", "click: onCancelQuoteCreation,enable:hasEnableButton", "data-bind");//10-04-2017

                    if (_btnCancel != null)
                    {
                        _btnCancel.Click();
                        _netWrapper.WaitForElementbyId(pagevar.ulEnquiryClass, true, false);//3-4-2017
                        if (msg != "Unit Price should be less than or equal to Contract Unit Price")//09-04-19
                        {
                            if (!FilterInQuotedTab(MTML_QuoteFile, xls_QuoteFile))//09-11-2017
                            {
                                if (msg != "")//09-11-2017
                                    MoveTo_Error("Unable to submit Quote for VRNo " + UCRefNo + " due to " + msg, MTML_QuoteFile, xls_QuoteFile, sFile);
                                else MoveTo_Error("Unable to submit Quote for VRNo " + UCRefNo, MTML_QuoteFile, xls_QuoteFile, sFile);
                                //GetAllRFQTab();
                                GetPendingRFQTab();//09-11-2017
                                Thread.Sleep(10000);
                            }
                        }
                        else
                        { 
                         if (msg != "")//09-11-2017
                                    MoveTo_Error("Unable to submit Quote for VRNo " + UCRefNo + " due to " + msg, MTML_QuoteFile, xls_QuoteFile, sFile);
                                else MoveTo_Error("Unable to submit Quote for VRNo " + UCRefNo, MTML_QuoteFile, xls_QuoteFile, sFile);
                        
                        }

                    }
                    else _netWrapper.LogText = "SubmitQuote Cancel button is null.";//10-04-2017

                }
            }
            else
                _netWrapper.LogText = "Submit button is null";//10-04-2017
        }

        public bool FilterInDraftTab(string MTML_QuoteFile, string xls_QuoteFile)//09-11-2017
        {
            bool result = false;
            DOMElement enquiryClass = _netWrapper.GetElementbyClass(pagevar.ulEnquiryClass);
            if (enquiryClass != null)
            {
                if (_netWrapper.WaitForLoading("pace-progress", "data-progress-text", "100%", pagevar.tblPendingRFQClass))
                {
                    var anchors = enquiryClass.GetElementsByTagName("a");
                    foreach (DOMElement a in anchors)
                    {
                        if (a.InnerText == "Drafted")
                        {
                            a.Click();
                            // Thread.Sleep(4000);
                            break;
                        }
                        // Thread.Sleep(500);
                    }
                    if (_netWrapper.WaitForLoading("pace-progress", "data-progress-text", "100%", pagevar.tblPendingRFQClass))//15-12-2017
                    {
                        IsLoad = true;//20-12-2017
                        Filter_Quote_By_RFQNo(UCRefNo);//20-12-2017
                        //    Thread.Sleep(10000);
                        DOMElement DraftedTable = _netWrapper.GetElementbyClass(pagevar.tblPendingRFQClass);
                        if (DraftedTable != null)
                        {
                            DOMElement eletbody = DraftedTable.GetElementByTagName("tbody");
                            if (eletbody != null)
                            {
                                List<DOMNode> eleRow = eletbody.GetElementsByTagName("tr");
                                if (eleRow.Count >= 1)
                                {
                                    string sFile = ScreenShotPath + "\\BS_Quote_Draft_" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
                                    _netWrapper.PrintPDF(sFile, false);
                                    MoveFileToBackup(MTML_QuoteFile, UCRefNo + " Quote drafted successfully", xls_QuoteFile);
                                    GetPendingRFQTab();//09-11-2017
                                    Thread.Sleep(10000);
                                    result = true;
                                }
                                else
                                {
                                    result = false;

                                }
                            }
                        }
                        Clear_SearchBox();//20-17-2017
                    }
                }
            }
            else { _netWrapper.LogText = "Drafted tab is null"; result = false; }
            return result;
        }

        public bool FilterInQuotedTab(string MTML_QuoteFile, string xls_QuoteFile)//09-11-2017
        {
            bool result = false;
            DOMElement enquiryClass = _netWrapper.GetElementbyClass(pagevar.ulEnquiryClass);
            if (enquiryClass != null)
            {
                if (_netWrapper.WaitForLoading("pace-progress", "data-progress-text", "100%", pagevar.tblPendingRFQClass))
                {
                    var anchors = enquiryClass.GetElementsByTagName("a");
                    foreach (DOMElement a in anchors)
                    {
                        if (a.InnerText == "Quoted")
                        {
                            a.Click();
                           
                            string b = a.InnerText;
                            if (a.InnerText != "Quoted") a.Click();
                            break;
                        }
                    }
                    if (_netWrapper.WaitForLoading("pace-progress", "data-progress-text", "100%", pagevar.tblPendingRFQClass))//15-12-2017
                    {
                        IsLoad = true;//20-12-2017
                        Filter_Quote_By_RFQNo(UCRefNo);//20-12-2017
                        DOMElement QuotedTable = _netWrapper.GetElementbyClass(pagevar.tblPendingRFQClass);
                        if (QuotedTable != null)
                        {
                            DOMElement eletbody = QuotedTable.GetElementByTagName("tbody");
                            if (eletbody != null)
                            {
                                List<DOMNode> eleRow = eletbody.GetElementsByTagName("tr");
                                if (eleRow.Count >= 1)
                                {
                                    string sFile = ScreenShotPath + "\\BS_Quote_Success_" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
                                    _netWrapper.PrintPDF(sFile, false);
                                    MoveFileToBackup(MTML_QuoteFile, UCRefNo + " Quote submitted successfully", xls_QuoteFile);
                                    GetPendingRFQTab();//09-11-2017
                                    Thread.Sleep(10000);
                                    result = true;
                                }
                            }
                        }
                        Clear_SearchBox();//20-17-2017
                    }
                }
            }
            else { _netWrapper.LogText = "Quoted tab is null"; result = false; }
            return result;
        }

        public void MoveTo_Error(string message, string MTML_QuoteFile, string xls_QuoteFile, string sFile)
        {
            _netWrapper.LogText = message;

            if (File.Exists(MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile)))
                File.Delete(MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
            File.Move(MTML_QuoteFile, MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
            Thread.Sleep(1000);

            if (File.Exists(Quote_UploadPath + "\\Error\\" + Path.GetFileName(xls_QuoteFile)))
                File.Delete(Quote_UploadPath + "\\Error\\" + Path.GetFileName(xls_QuoteFile));
            File.Move(xls_QuoteFile, Quote_UploadPath + "\\Error\\" + Path.GetFileName(xls_QuoteFile));
            Thread.Sleep(1000);

            if (File.Exists(MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile))) // && File.Exists(Quote_UploadPath + "\\Error\\" + Path.GetFileName(xls_QuoteFile)))
                CreateQuoteAuditFile(sFile, "Bernhard Schulte Quote", UCRefNo, "Error", message);
        }

        public string loading()
        {
            string msg = ""; int maxcount = 60000, count = 0;
            while (count < maxcount)
            {
                DOMElement _divPopoup = _netWrapper.GetElementbyClass(pagevar.DivPopup);
                if (_divPopoup != null)
                {
                    DOMElement _spanMsg = _netWrapper.GetElementByType("span", _divPopoup);
                    if (_spanMsg != null)
                    {
                        msg = _spanMsg.TextContent;
                        break;
                    }
                }
                count++;
            }
            return msg;
        }

        public void Clear_SearchBox(DOMInputElement _inputSearch)
        {
            _inputSearch.Value = string.Empty;
        }

        public bool SetLeadDays(string QuoteFile)
        {
            bool result = false;
            DOMInputElement _txtLeadDays = (DOMInputElement)_netWrapper.GetElementByType("input", pagevar.bindLeadDays, "data-bind");
            //if (Convert.ToInt32(LeadDays) <= 150)//06-07-2018 commented as site accepting greater than 150 days
            //{
                if (_txtLeadDays != null)
                {

                    //KeyParams paramersa = new KeyParams(VirtualKeyCode.TAB, ' ');
                    //_netWrapper.browser.KeyDown(paramersa);
                    //_netWrapper.browser.KeyUp(paramersa);
                    _txtLeadDays.Value = string.Empty;
                    _txtLeadDays.Focus();//21-07-2017
                    Thread.Sleep(500);

                    for (int i = 0; i < LeadDays.Length; i++)//enter lead days in textbox
                    {
                        SetAlphaNumeric(LeadDays[i]);
                        Thread.Sleep(300);
                    }
                    if (_txtLeadDays.Value == LeadDays)
                    {
                        result = true;
                        _netWrapper.LogText = LeadDays + " no. of Lead Days added.";
                    }
                }
                else
                {
                    result = false;
                    MoveFileToError(QuoteFile, "Unable to get lead days inputbox for VRNo '" + UCRefNo, Quote_UploadPath + @"\" + MsgNumber);
                }
            //}
            //else
            //{
            //    result = false;
            //    MoveFileToError(QuoteFile, "Unable to update Lead Days for VRNo '" + UCRefNo + "' greater than 150 days", Quote_UploadPath + @"\" + MsgNumber);
            //}
            return result;
        }

        public bool Fill_Price_Validity_Date(string QuoteFile)
        {
            string validDate = "";
           // int maxcount = 0;
            bool result = false;
            DateTime dt = DateTime.Now.Date;
            DateTime dtValidDate = dt.AddDays(Convert.ToDouble(Price_Validity_Date));
            bool res = _netWrapper.WaitForElementbyId("import-options");
            if (res == true)
            {
                DOMInputElement _elePriceValid = (DOMInputElement)_netWrapper.GetElementbyId(pagevar.txtPriceValidity);
                if (_elePriceValid != null)
                {
                    _elePriceValid.Value = "";
                    _elePriceValid.Focus();//21-07-2017
                    Thread.Sleep(1000);//21-07-2017
                    //if (VendorStatus.ToUpper().Contains("QUOTE") || VendorStatus.ToUpper().Contains("REJECT") || VendorStatus.ToUpper().Contains("DRAFT"))
                    //    maxcount = 13;
                    //else if (VendorStatus.ToUpper().Contains("PENDING"))
                    //    maxcount = 14;
                    //Thread.Sleep(3000);
                    //for (int i = 0; i <= maxcount; i++)//To get price valid until datetimepicker 
                    //{
                    //    KeyParams paramersa = new KeyParams(VirtualKeyCode.TAB, ' ');
                    //    _netWrapper.browser.KeyDown(paramersa);
                    //    _netWrapper.browser.KeyUp(paramersa);
                    //    Thread.Sleep(1000);
                    //}

                    if (dtExpDate != "")
                        validDate = dtExpDate;
                    else
                        validDate = dtValidDate.ToString("dd-MMM-yyyy");

                    Thread.Sleep(1000);

                    for (int j = 0; j < validDate.Length; j++)//enter date in datetimepicker
                    {
                        if (checkChar_AlphaNumeric(validDate[j]))
                        {
                            SetAlphaNumeric(validDate[j]);
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            KeyParams paramers = new KeyParams(VirtualKeyCode.OEM_MINUS, '-');
                            _netWrapper.browser.KeyDown(paramers);
                            _netWrapper.browser.KeyUp(paramers);
                            Thread.Sleep(1000);
                        }
                    }
                    //KeyParams paramers1 = new KeyParams(VirtualKeyCode.RETURN, '\n');
                    //_netWrapper.browser.KeyDown(paramers1);
                    //_netWrapper.browser.KeyUp(paramers1);
                    //Thread.Sleep(1000);
                   // _elePriceValid.Blur();
                    result = true;

                    //var doc = _netWrapper.browser.GetDocument().DocumentElement.Children;
                    //List<DOMNode> _lstNodes = doc[2].Children;
                    //foreach (DOMNode a in _lstNodes)
                    //{
                    //    if (a.NodeType == DOMNodeType.ElementNode)
                    //    {
                    //        DOMElement _ele = (DOMElement)a;
                    //        if (_ele.HasAttribute("class"))
                    //        {
                    //            if (_ele.GetAttribute("class") == "daterangepicker dropdown-menu ltr single show-calendar opensleft")
                    //            {
                    //                DOMElement _eleActive = _ele.GetElementByClassName("active start-date active end-date available");
                    //                if (_eleActive == null)
                    //                {
                    //                    _eleActive = _ele.GetElementByClassName("weekend active start-date active end-date available");
                    //                }
                    //                if (_eleActive != null)
                    //                {
                    //                   // _eleActive.Click();
                    //                    string _class = _eleActive.GetAttribute("class");
                    //                    _netWrapper.ClickElementbyClass(_class);
                    //                    Thread.Sleep(10000);
                    //                    result = true;
                    //                    break;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    // _netWrapper.browser.ExecuteJavaScript("alert(test)");
                    if (result)
                    {
                        string PriceValidDate = _elePriceValid.Value;
                        if (PriceValidDate == validDate)
                        {
                            _netWrapper.LogText = "Price validity date added."; result = true;
                        }
                        else
                        {
                            result = false;
                          //  MoveFileToError(QuoteFile, "Unable to fill Price Validity Date for VRNo. " + UCRefNo, Quote_UploadPath + @"\" + MsgNumber);
                            string sFile = ScreenShotPath + "\\BS_Quote_" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
                            _netWrapper.PrintPDF(sFile, false);
                            _netWrapper.LogText = "Unable to fill Price Validity Date for VRNo. " + UCRefNo;
                            CreateQuoteAuditFile(sFile, "Bernhard Schulte Quote", UCRefNo, "Error", "Unable to fill Price Validity Date for VRNo. " + UCRefNo);
                        }

                    }
                    else
                    {
                        result = false;
                       // MoveFileToError(QuoteFile, "Unable to fill Price Validity Date for VRNo. " + UCRefNo, Quote_UploadPath + @"\" + MsgNumber);
                        string sFile = ScreenShotPath + "\\BS_Quote_" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
                        _netWrapper.PrintPDF(sFile, false);
                        _netWrapper.LogText = "Unable to fill Price Validity Date for VRNo. " + UCRefNo;
                        CreateQuoteAuditFile(sFile, "Bernhard Schulte Quote", UCRefNo, "Error", "Unable to fill Price Validity Date for VRNo. " + UCRefNo);
                    }
                }
                else
                {
                    result = false;
                    MoveFileToError_WithoutScreenShot(QuoteFile, "Unable to get price valid until datetimepicker for VRNo '" + UCRefNo, Quote_UploadPath + @"\" + MsgNumber);
                }
            }
            return result;
        }

        public bool Apply_Discount_On_Items(string QuoteFile)
        {
            bool result = false;
            DOMInputElement _eleDisc = (DOMInputElement)_netWrapper.GetElementByType("input", "Disc %", "placeholder");
            DOMInputElement _eleVat = (DOMInputElement)_netWrapper.GetElementByType("input", "VAT %", "placeholder");

            if (_eleDisc.Value != "0" || _eleVat.Value != "0")
            {
                DOMElement _btnApply = _netWrapper.GetElementByType("button", "btn btn-primary", "class");
                if (_btnApply != null)
                {
                    _btnApply.Click();
                    Thread.Sleep(5000);
                    result = true;
                    _netWrapper.LogText = "Discount/VAT applied";
                }
                else
                {
                    MoveFileToError(QuoteFile, "Apply button not found for calculate discount/vat for Ref No. " + UCRefNo, Quote_UploadPath + @"\" + MsgNumber);
                    result = false;
                }
            }
            else result = true;
            return result;
        }

        public bool Add_ValidDate_Remarks(string FileName, string MTML_FileName)
        {
            bool result = false;
            string validDate = "";
             string vendorRemarks="";

             if (File.Exists(FileName))
             {
                 if (dtExpDate != "")
                 {

                     validDate = dtExpDate;
                 }
                 else
                 {
                     DateTime dt = DateTime.Now.Date;
                     DateTime dtValidDate = dt.AddDays(Convert.ToDouble(Price_Validity_Date));
                     validDate = dtValidDate.ToString("dd-MMM-yyyy");
                 }

                 License license = new Aspose.Cells.License();
                 license.SetLicense("Aspose.Total.lic");
                 Workbook _workbook = new Workbook(FileName);
                 Worksheet _worksheet = _workbook.Worksheets[0];
                 FindOptions findOptions = new FindOptions();
                 findOptions.CaseSensitive = false;
                 findOptions.LookInType = LookInType.Values;
                 if (SearchTextInXLS != "")
                 {
                     Cell foundCell = _worksheet.Cells.Find(SearchTextInXLS, null, findOptions);
                     if (foundCell != null)
                     {
                         int row = foundCell.Row;
                         Aspose.Cells.Cell _cell = _worksheet.Cells["E" + (row + 2)];
                         string a = (string)_cell.Value;
                         if (a == null)
                         {
                             if (PayTerms != "") a = PayTerms;//9-2-2018
                             if (DeliveryTerms != "") a += " Delivery Terms:" + DeliveryTerms+".";
                             if (SupplierComment != "") a += SupplierComment;  //24-07-2017
                         }//24-07-2017
                         string b = "Price Valid Until: " + validDate;
                         if (a != null)
                         {
                        if (!a.ToString().Contains(b))
                                 vendorRemarks = a + "\n" + b;
                             else
                                 vendorRemarks = a;
                         }
                         else vendorRemarks = b;
                         
                       
                         //add price validity date in vendor remarks
                         _cell.Value = vendorRemarks;
                         if (_cell.Value.ToString().Contains("Price Valid Until:"))
                         {
                             _workbook.Save(FileName);
                             result = true;
                         }
                     }
                 }

                 #region for adding packing cost and freight Cost 08-09-17
                 int row1=0;
                 if (Convert.ToDouble(PackingCost) != 0.0)
                 {
                     Cell foundCell1 = _worksheet.Cells.Find("Packing cost", null, findOptions);
                     if (foundCell1 != null)
                     {
                          row1 = foundCell1.Row;
                         Aspose.Cells.Cell _cell = _worksheet.Cells["J" + (row1 + 1)];
                         Double a = Convert.ToDouble(_cell.Value);

                         if (a == 0.0 && Convert.ToDouble(PackingCost) != 0.0)
                         {
                             _cell.Value = PackingCost;
                             _workbook.Save(FileName);
                             result = true;
                         }
                     }
                 }
                 if (Convert.ToDouble(FreightCharge) != 0.0)
                 {
                     Cell foundCell1 = _worksheet.Cells.Find("Freight charge", null, findOptions);
                     if (foundCell1 != null)
                     {
                         row1 = foundCell1.Row;
                         Aspose.Cells.Cell _cell1 = _worksheet.Cells["C" + (row1 + 1)];
                         string strFreight = (string)_cell1.Value;
                         if (strFreight == "Freight charge")
                         {
                             //Aspose.Cells.Cell _cell2 = _worksheet.Cells["M" + (row1 + 1)];//J
                             //Aspose.Cells.Cell _cellChk = _worksheet.Cells["M" + (row1 - 1)];//added by kalpita on 07/01/2020

                             Aspose.Cells.Cell _cell2 = _worksheet.Cells[cFreightcell + (row1 + 1)];//J
                             Aspose.Cells.Cell _cellChk = _worksheet.Cells[cFreightcell + (row1 - 1)];
                             if (_cellChk != null)
                             {
                                 string _cellval = Convert.ToString(_cellChk.Value);
                                 if (!string.IsNullOrEmpty(_cellval) && _cellval == "Amount")
                                 {
                                     Double b = Convert.ToDouble(_cell2.Value);
                                     if (b == 0.0 && Convert.ToDouble(FreightCharge) != 0.0)
                                     {
                                         _cell2.Value = FreightCharge;
                                         _workbook.Save(FileName);
                                         result = true;
                                     }
                                 }
                             }
                           
                          
                         }
                     }
                 }
                     
                 
                 #endregion
             }
             else
             {
                 _netWrapper.LogText = FileName + " not found in quote upload path for VRNo " + UCRefNo;
                 CreateQuoteAuditFile(FileName, "Bernhard Schulte Quote", UCRefNo, "Error", FileName + " not found in quote upload path for VRNo " + UCRefNo);//25-04-2017
             }
            return result;
        }

        public void MoveFileToError(string MTML_QuoteFile, string message, string xls_QuoteFile)
        {
            string sFile = ScreenShotPath + "\\BS_Quote_" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
            _netWrapper.PrintPDF(sFile, false);
            _netWrapper.LogText = message;

            try
            {
                if (File.Exists(MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile)))
                    File.Delete(MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
                File.Move(MTML_QuoteFile, MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
                Thread.Sleep(1000);

                if (File.Exists(Quote_UploadPath + "\\Error\\" + Path.GetFileName(xls_QuoteFile)))
                    File.Delete(Quote_UploadPath + "\\Error\\" + Path.GetFileName(xls_QuoteFile));
                File.Move(xls_QuoteFile, Quote_UploadPath + "\\Error\\" + Path.GetFileName(xls_QuoteFile));
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                _netWrapper.LogText = "Error in MoveFileToError() - " + ex.Message;
            }

            //if (File.Exists(MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile)) && File.Exists(Quote_UploadPath + "\\Error\\" + Path.GetFileName(xls_QuoteFile)))
            CreateQuoteAuditFile(sFile, "Bernhard Schulte Quote", UCRefNo, "Error", message);
        }

        public void MoveFileToError_WithoutScreenShot(string MTML_QuoteFile, string message, string xls_QuoteFile)
        {
            if (File.Exists(MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile)))
                File.Delete(MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
            File.Move(MTML_QuoteFile, MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
            Thread.Sleep(1000);

            if (File.Exists(Quote_UploadPath + "\\Error\\" + Path.GetFileName(xls_QuoteFile)))
                File.Delete(Quote_UploadPath + "\\Error\\" + Path.GetFileName(xls_QuoteFile));
            File.Move(xls_QuoteFile, Quote_UploadPath + "\\Error\\" + Path.GetFileName(xls_QuoteFile));
            Thread.Sleep(1000);

            if (File.Exists(MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile))) // && File.Exists(Quote_UploadPath + "\\Error\\" + Path.GetFileName(xls_QuoteFile)))
                CreateQuoteAuditFile(MTML_QuoteFile, "Bernhard Schulte Quote", UCRefNo, "Error", message);

            _netWrapper.LogText = message;
        }

        public void MoveFileToBackup(string MTML_QuoteFile, string message, string xls_QuoteFile)
        {
            _netWrapper.LogText = message;

            try
            {
                if (File.Exists(MTML_Quote_UploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile)))
                    File.Delete(MTML_Quote_UploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile));
                File.Move(MTML_QuoteFile, MTML_Quote_UploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile));
                Thread.Sleep(1000);

                if (File.Exists(Quote_UploadPath + "\\Backup\\" + Path.GetFileName(xls_QuoteFile)))
                    File.Delete(Quote_UploadPath + "\\Backup\\" + Path.GetFileName(xls_QuoteFile));
                File.Move(xls_QuoteFile, Quote_UploadPath + "\\Backup\\" + Path.GetFileName(xls_QuoteFile));
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                _netWrapper.LogText = "Unable to move file " + ex.Message;
            }

            if (File.Exists(MTML_Quote_UploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile))) //&& File.Exists(Quote_UploadPath + "\\Backup\\" + Path.GetFileName(xls_QuoteFile)))
                CreateQuoteAuditFile(MTML_QuoteFile, "Bernhard Schulte Quote", UCRefNo, "Success", message);
        }

        public bool IsDeclineQuote(string QuoteFile, string MTML_QuoteFile)
        {
            bool result = false;
            IsDecline = true;
            if (IsDecline)
            {
                if (Username.ToUpper().Trim() != "SUPPLY@NEKOSHIP.NL")//30-05-2018
                {
                    result = true;
                    if (SupplierComment == "")
                    {
                        SupplierComment = Rejected_msg;
                    }
                    /*click on reject button*/
                    DOMElement _btnReject = _netWrapper.GetElementByType("input", "Reject", "value");
                    if (_btnReject != null)
                    {
                        if (!_btnReject.HasAttribute("style") )
                        {
                            _btnReject.Click();
                            Thread.Sleep(2000);
                            DOMElement _divReject = _netWrapper.GetElementbyClass(pagevar.divRejectTxt);
                            if (_divReject != null)
                            {
                                DOMTextAreaElement _txtRemarks = (DOMTextAreaElement)_netWrapper.GetElementByType("textarea", "Describe reason here", "placeholder");
                                if (_txtRemarks != null)
                                {
                                    Thread.Sleep(300);
                                    /*set value to reject remarks tetxarea*/
                                    KeyParams paramersa = new KeyParams(VirtualKeyCode.TAB, ' ');
                                    _netWrapper.browser.KeyDown(paramersa);
                                    _netWrapper.browser.KeyUp(paramersa);
                                    Thread.Sleep(500);
                                    for (int i = 0; i < SupplierComment.Length; i++)
                                    { SetCharacters(SupplierComment[i]); Thread.Sleep(300); }

                                    /*lost focus*/
                                    DOMElement _eleLabel = _netWrapper.GetElementbyClass(pagevar.divRejectBox);
                                    _eleLabel.Click();
                                    Thread.Sleep(500);
                                    /*again open rejection box*/
                                    _btnReject.Click();
                                    Thread.Sleep(1000);
                                    DOMElement _btnSubmit = _netWrapper.GetElementByType("input", pagevar.btnRejectSubmit, "class");
                                    if (_btnSubmit != null)
                                    {
                                        /*click on reject submit button*/
                                        _btnSubmit.Click();
                                        string message = loading();
                                        if (message.ToUpper().Contains("SUCCESSFULLY"))
                                        {
                                            string sFile = ScreenShotPath + "\\BS_Quote_Decline_" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
                                            _netWrapper.PrintPDF(sFile, false);
                                            _netWrapper.WaitForElementbyId(pagevar.ulEnquiryClass, true, false);//3-4-2017
                                            string msg = "QUOTE for REF : " + UCRefNo + " for VESSEL : " + VesselName + " is rejected.";
                                            MoveDeclineFileToBackup(MTML_QuoteFile, msg, QuoteFile);
                                            //   if (Username.ToUpper().Trim() == "SUPPLY@NEKOSHIP.NL")//30-05-2018
                                            SendMailNotification(_interchange, "QUOTE", UCRefNo, "DECLINED", "Quote '" + UCRefNo + "' declined.");//30-05-2018               
                                            DOMElement _ele = _netWrapper.GetElementbyClass(pagevar.tblPendingRFQClass);
                                            DOMInputElement _inputSearch = (DOMInputElement)_netWrapper.GetElementByType("input", "search");
                                            if (_ele != null && _inputSearch != null)
                                            {
                                                Clear_SearchBox(_inputSearch);
                                            }
                                            //  CreateMailFile("Rejected");
                                        }
                                        else
                                        {
                                            string sFile = ScreenShotPath + "\\BS_Quote_" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
                                            _netWrapper.PrintPDF(sFile, false);
                                            MoveTo_Error("Unable to submit Quote for VRNo " + UCRefNo + " due to " + message, MTML_QuoteFile, QuoteFile, sFile);
                                            // DOMInputElement _btnCancel = (DOMInputElement)_netWrapper.GetElementByType("input", "click: onCancelQuoteCreation", "data-bind");//10-04-2017
                                            DOMInputElement _btnCancel = (DOMInputElement)_netWrapper.GetElementByType("input", "click: onCancelQuoteCreation,enable:hasEnableButton", "data-bind");//10-04-2017
                                            if (_btnCancel != null)
                                            {
                                                _btnCancel.Click();
                                                _netWrapper.WaitForElementbyId(pagevar.ulEnquiryClass, true, false);//3-4-2017
                                                // GetAllRFQTab();//09-11-2017
                                                GetPendingRFQTab();//09-11-2017
                                                Thread.Sleep(10000);
                                            }
                                            else _netWrapper.LogText = "DeclineQuote Cancel button is null.";//10-04-2017
                                        }
                                    }
                                    else
                                        MoveFileToError(MTML_QuoteFile, "Reject submit button not loaded for VRNo. " + UCRefNo, Quote_UploadPath + @"\" + MsgNumber);
                                }
                                else
                                    MoveFileToError_WithoutScreenShot(MTML_QuoteFile, "Reject Remarks textbox not loaded for VRNo." + UCRefNo, Quote_UploadPath + @"\" + MsgNumber);
                            }
                        }
                        else
                            MoveFileToError_WithoutScreenShot(MTML_QuoteFile, "Reject button is not visible for " + UCRefNo, Quote_UploadPath + @"\" + MsgNumber);//14-2-19
                    }
                    else
                        MoveFileToError_WithoutScreenShot(MTML_QuoteFile, "Reject button not loaded for VRNo." + UCRefNo, Quote_UploadPath + @"\" + MsgNumber);
                }
                else//30-5-2018
                {
                    if (Apply_Discount_On_Items(MTML_QuoteFile))
                    {
                        DOMElement liGrandTotal = _netWrapper.GetElementbyClass(pagevar.liGrandTotal);
                        if (liGrandTotal != null)
                        {
                            DOMElement _span = _netWrapper.GetElementByType("span", liGrandTotal);
                            if (_span != null)
                            {
                                Double GTotal = Convert.ToDouble(_span.TextContent);
                                if (Convert.ToInt32(Convert.ToDouble(GrandTotal)) == Convert.ToInt32(GTotal))
                                {
                                    DOMInputElement _btnDraft = (DOMInputElement)_netWrapper.GetElementByType("input", "Save as Draft", "value");
                                    if (_btnDraft != null)
                                    {//changed by Kalpita on 19/12/2019
                                        if (IsDraftQuote) { bool res = DraftQuote(MTML_QuoteFile, QuoteFile, _btnDraft); }
                                    }
                                }
                                else if (BuyerTotal != "")
                                {
                                    if (Convert.ToInt32(Convert.ToDouble(BuyerTotal)) == Convert.ToInt32(GTotal))
                                    {
                                        DOMInputElement _btnDraft = (DOMInputElement)_netWrapper.GetElementByType("input", "Save as Draft", "value");
                                        if (_btnDraft != null)
                                        {//changed by Kalpita on 19/12/2019
                                            if (IsDraftQuote) { bool res = DraftQuote(MTML_QuoteFile, QuoteFile, _btnDraft); }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            IsDecline = false;
            return result;
        }

        public void MoveDeclineFileToBackup(string QuoteFile, string message, string xlsQuote_File)
        {
            _netWrapper.LogText = message;
            try
            {

                if (File.Exists(MTML_Quote_UploadPath + "\\Backup\\" + Path.GetFileName(QuoteFile)))
                    File.Delete(MTML_Quote_UploadPath + "\\Backup\\" + Path.GetFileName(QuoteFile));
                File.Move(QuoteFile, MTML_Quote_UploadPath + "\\Backup\\" + Path.GetFileName(QuoteFile));

                if (File.Exists(Quote_UploadPath + "\\Backup\\" + Path.GetFileName(xlsQuote_File)))
                {
                    File.Delete(Quote_UploadPath + "\\Backup\\" + Path.GetFileName(xlsQuote_File));
                }
                File.Move(xlsQuote_File, Quote_UploadPath + "\\Backup\\" + Path.GetFileName(xlsQuote_File));
            }
            catch (Exception ex)
            {
                _netWrapper.LogText = "Unable to move file -" + ex.Message;
            }

            if (File.Exists(Quote_UploadPath + "\\Backup\\" + Path.GetFileName(QuoteFile)))
                CreateQuoteAuditFile(QuoteFile, "Bernhard Schulte Quote", UCRefNo, "Declined", message);
        }

        public void CreateMailFile(string status)
        {
            if (status == "Submitted" || status == "Rejected")
            {
                string strMailTemp = File.ReadAllText(Mail_Template);
                if (strMailTemp.Length > 0)
                {
                    strMailTemp = strMailTemp.Replace("#BUYER_NAME#", BuyerName);
                    strMailTemp = strMailTemp.Replace("#SUPPLIER_NAME#", supplierName);
                    string msg = "QUOTE for REF : " + UCRefNo + " VESSEL : " + VesselName + status + "  From : " + supplierName + " To : " + BuyerName;
                    strMailTemp = strMailTemp.Replace("#MESSAGE#", msg);

                    string TopMsg = LesRecordID + "|" + DocType + "|" + UCRefNo + "|" + FROM_EMAIL_ID + "|" + supplierEmail + "||" + MAIL_BCC + "|" + msg;
                    string BottomMsg = "|||0||||||" + status + "|||";
                    if (FROM_EMAIL_ID.Length != 0 && supplierEmail.Length != 0)
                    {
                        string FileName = MailFilePath + "\\Mail_Txt_" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".txt";
                        if (!File.Exists(FileName))
                        {
                            try
                            {
                                File.WriteAllText(FileName, TopMsg + Environment.NewLine + strMailTemp + Environment.NewLine + Environment.NewLine + Environment.NewLine + BottomMsg);
                            }
                            catch (Exception)
                            { }
                        }
                    }
                    else
                    {
                        _netWrapper.LogText = "Unable to send Quote mail for Buyer Ref : " + UCRefNo;
                        string sFile = "RFQ_" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls";
                        CreateQuoteAuditFile(sFile, "Bernhard Schulte Quote", UCRefNo, "Error", "Unable to send Quote mail for Buyer Ref : " + UCRefNo);
                    }
                }
            }
        }

        public void GetScreenShots(string RFQNo, string RFQStatus, string VendorStatus, string QuoteFile, string xlsQuote_File)
        {
            if (File.Exists(MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(QuoteFile)))
                File.Delete(MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(QuoteFile));
            File.Move(QuoteFile, MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(QuoteFile));

            if (File.Exists(Quote_UploadPath + "\\Error\\" + Path.GetFileName(xlsQuote_File)))
                File.Delete(Quote_UploadPath + "\\Error\\" + Path.GetFileName(xlsQuote_File));
            File.Move(xlsQuote_File, Quote_UploadPath + "\\Error\\" + Path.GetFileName(xlsQuote_File));


            if (VendorStatus.ToUpper().Contains("QUOTE") || VendorStatus.ToUpper().Contains("DRAFT") || VendorStatus.ToUpper().Contains("REJECT"))
            {
                _netWrapper.LogText = "Quote file '" + Path.GetFileName(QuoteFile) + "' move to Error folder as its vendor status is " + VendorStatus + ".";
                _netWrapper.LogText = "XLS Quote file '" + Path.GetFileName(xlsQuote_File) + "' move to Error folder as its vendor status is " + VendorStatus + ".";
            }
            else if (RFQStatus.Contains("Close"))
            {
                _netWrapper.LogText = "Quote file '" + Path.GetFileName(QuoteFile) + "' move to Error folder as its RFQ status is Closed.";
                _netWrapper.LogText = "XLS Quote file '" + Path.GetFileName(QuoteFile) + "' move to Error folder as its RFQ status is Closed.";
            }
            string sFile = ScreenShotPath + "\\BS_Quote_" + RFQNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
            _netWrapper.PrintPDF(sFile, false);
            if (File.Exists(sFile))
            {
                if (RFQStatus.Contains("Close"))
                    CreateQuoteAuditFile(sFile, "Bernhard Schulte Quote", RFQNo, "Error", "Unable to process Quote for VRNo '" + RFQNo + "' due to RFQ Status is already " + RFQStatus);
                else
                    CreateQuoteAuditFile(sFile, "Bernhard Schulte Quote", RFQNo, "Error", "Unable to process Quote for VRNo '" + RFQNo + "' due to Vendor Status is already " + VendorStatus);
            }
            else
            {
                if (RFQStatus.Contains("Close"))
                    CreateQuoteAuditFile("", "Bernhard Schulte Quote", RFQNo, "Error", "Unable to process Quote for VRNo '" + RFQNo + "' due to RFQ Status is already " + RFQStatus);
                else
                    CreateQuoteAuditFile("", "Bernhard Schulte Quote", RFQNo, "Error", "Unable to process Quote for VRNo '" + RFQNo + "' due to Vendor Status is already " + VendorStatus);
            }
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

 
        #endregion

        #region Common function
        public void CreateAuditFile(string FileName, string Module, string RefNo, string LogType, string Audit)
        {
            try
            {
                // string ESupplierBuyerAddressCode = Convert.ToString(ConfigurationManager.AppSettings["BUYER"]);
                // string ESupplierSupplierAddressCode = Convert.ToString(ConfigurationManager.AppSettings["SUPPLIER"]);

                string auditPath = AuditPath;
                if (!Directory.Exists(auditPath)) Directory.CreateDirectory(auditPath);

                string auditData = "";
                if (auditData.Trim().Length > 0) auditData += Environment.NewLine;
                auditData += buyer_Link_code + "|"; //ESupplierBuyerAddressCode + "|";
                auditData += eSupp_SuppAddCode + "|";
                auditData += Module + "|";
                auditData += Path.GetFileName(FileName) + "|";
                auditData += RefNo + "|";
                auditData += LogType + "|";
                auditData += DateTime.Now.ToString("yy-MM-dd HH:mm") + " : " + Audit;
                if (auditData.Trim().Length > 0)
                {
                    File.WriteAllText(auditPath + "\\Audit_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt", auditData);
                    System.Threading.Thread.Sleep(5);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateQuoteAuditFile(string FileName, string Module, string RefNo, string LogType, string Audit)
        {
            try
            {
                // string ESupplierBuyerAddressCode = Convert.ToString(ConfigurationManager.AppSettings["BUYER"]);
                //string ESupplierSupplierAddressCode = Convert.ToString(ConfigurationManager.AppSettings["SUPPLIER"]);

                string auditPath = AuditPath;
                if (!Directory.Exists(auditPath)) Directory.CreateDirectory(auditPath);

                string auditData = "";
                if (auditData.Trim().Length > 0) auditData += Environment.NewLine;
                auditData += BuyerCode + "|";
                auditData += SupplierCode + "|";
                auditData += Module + "|";
                auditData += Path.GetFileName(FileName) + "|";
                auditData += RefNo + "|";
                auditData += LogType + "|";
                auditData += DateTime.Now.ToString("yy-MM-dd HH:mm") + " : " + Audit;
                if (auditData.Trim().Length > 0)
                {
                    File.WriteAllText(auditPath + "\\Audit_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt", auditData);
                    System.Threading.Thread.Sleep(5);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
#region commented code
#region delete
//if (CompanyName != "")
//{
//    string dwndFile = DownloadFile(eRow, RFQNo, DownloadPath, oldFileName);
//    if (dwndFile != "")
//    {
//        if (File.Exists(DownloadPath + "\\" + oldFileName))
//        {
//            //string buyer_Link_code = "";
//            if (CompanyName == "")
//            {
//                CompanyName = Read_RFQXlsFile(DownloadPath + "\\" + oldFileName);
//                if (CompanyName != null)
//                {
//                    buyer_Link_code = getBuyerLinkCode(CompanyName);
//                }
//            }
//            else
//                buyer_Link_code = getBuyerLinkCode(CompanyName);
//            newFileName = buyer_Link_code + "_RFQ_" + RFQNo.Replace('/', '_') + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsm";
//            if (buyer_Link_code != "")
//            {

//                File.Move(DownloadPath + "\\" + oldFileName, DownloadPath + "\\" + newFileName);
//                if (File.Exists(DownloadPath + "\\" + newFileName))
//                {
//                    WriteToRFQDownloadedFile(RFQNo, RFQDt, Vessel, Port, RFQRecDate, newFileName);
//                    result = true;
//                }
//            }
//            else
//            {
//                _netWrapper.LogText = "RFQ '" + newFileName + "' for VRNO '" + RFQNo + "' downloading failed because '" + CompanyName + "' not found in list.";
//                string Audit = DateTime.Now.ToString("yy-MM-dd HH:mm") + " RFQ '" + newFileName + "' for VRNO '" + RFQNo + "' download failed because '" + CompanyName + "' not found in list.";
//                CreateAuditFile("", ProcessorName, RFQNo, "Error", Audit);
//            }
//        }
//        else
//        {
//            _netWrapper.LogText = "RFQ File " + DownloadPath + "\\" + oldFileName + " not downloaded for for RFQ No. " + RFQNo + ".";
//            string Audit = DateTime.Now.ToString("yy-MM-dd HH:mm") + " RFQ File " + DownloadPath + "\\" + oldFileName + " not downloaded for for RFQ No. " + RFQNo + ".";
//            CreateAuditFile("", ProcessorName, RFQNo, "Error", Audit);
//        }
//    }
//}
#endregion

//public void Process_MultipleRecordForSameVRNO(List<DOMNode> eleRow, string MTML_QuoteFile)
//{
//    foreach (DOMNode erow in eleRow)
//    {
//        DOMElement eRow = (DOMElement)erow;
//        string DeliveryPort = "", RFQStatus = "";
//        try
//        {
//            string RFQNo = GetGrid_RfqNo(eRow);
//            if (RFQNo != "")
//            {
//                if (RFQNo == UCRefNo)
//                {
//                    /*check vendor,rfq status is valid or not */
//                    if (ifValidStatus(eRow, UCRefNo, out DeliveryPort, out RFQStatus))
//                    {
//                      //  if (PortName.ToUpper().Contains(DeliveryPort.ToUpper()))
//                      //  {
//                            List<DOMNode> _data = eRow.GetElementsByTagName("td");
//                            if (_data.Count > 0)
//                            {
//                                try
//                                {
//                                    processQuoteFile(MTML_QuoteFile);
//                                    IsLoad = true;
//                                }
//                                catch (Exception ex)
//                                {
//                                    throw new Exception("Unable to process Quote file for VRNo. '" + UCRefNo + "'" + ex.GetBaseException().ToString());
//                                }
//                            }
//                       // }
//                    }
//                    else
//                    {
//                        Thread.Sleep(2000);
//                        if (VendorStatus.ToUpper().Contains("QUOTE") || VendorStatus.ToUpper().Contains("DRAFT") || VendorStatus.ToUpper().Contains("REJECT") || RFQStatus.ToUpper() == "CLOSED")
//                        {
//                            string xlsQuote_File = Quote_UploadPath + @"\" + MsgNumber;
//                            GetScreenShots(UCRefNo, RFQStatus, VendorStatus, MTML_QuoteFile, xlsQuote_File);
//                            break;
//                        }
//                        else _netWrapper.LogText = "Quote for VRNo '" + RFQNo + "' has RFQ Status as " + RFQStatus + " and Vendor Status as " + VendorStatus;
//                    }
//                }
//            }
//        }
//        catch (Exception ex)
//        { _netWrapper.LogText = "Exception on Process_MultipleRecordForSameVRNO: " + ex.ToString(); }
//    }
//}


//public bool Process_After_Draft(string MTML_QuoteFile,string xls_QuoteFile)
//{
//    DOMElement _ele = _netWrapper.GetElementbyClass(pagevar.tblPendingRFQClass);
//    DOMInputElement _inputSearch = (DOMInputElement)_netWrapper.GetElementByType("input", "search");
//    if (_ele != null && _inputSearch != null)
//    {
//        GetAllRFQTab();
//        Thread.Sleep(2000);
//        Filter_Quote_By_RFQNo(UCRefNo);
//        DOMElement AllRFQTable = _netWrapper.GetElementbyClass(pagevar.tblPendingRFQClass);
//        if (AllRFQTable != null)
//        {
//            DOMElement eletbody = AllRFQTable.GetElementByTagName("tbody");
//            if (eletbody != null)
//            {
//                List<DOMNode> eleRow = eletbody.GetElementsByTagName("tr");
//                if (eleRow.Count > 0)
//                {
//                    foreach (DOMNode erow in eleRow)
//                    {
//                        DOMElement eRow = (DOMElement)erow;
//                        List<DOMNode> _data = eRow.GetElementsByTagName("td");
//                        if (_data.Count > 0)
//                        {
//                            string RFQNo = GetGrid_RfqNo(eRow);
//                            if (RFQNo != "")
//                            {
//                                if (RFQNo == UCRefNo)
//                                {
//                                    /*click on Create Quote button*/
//                                    if (_netWrapper.ClickElementbyClass(pagevar.btnQuoteClass))
//                                    {
//                                        try
//                                        {
//                                            Thread.Sleep(10000);
//                                            if (Check_SaveTo_Draft_Present())
//                                            {
//                                                DOMInputElement _btnDraft = (DOMInputElement)_netWrapper.GetElementByType("input", "Save as Draft", "value");
//                                            }
//                                            else
//                                            { 
//                                            MoveFileToError(MTML_QuoteFile,"Save To Draft button present after clicked on draft button for VRNo "+UCRefNo,xls_QuoteFile);
//                                            }
//                                        }
//                                        catch (Exception ex)
//                                        {
//                                            _netWrapper.LogText = "Exception while importing quote file for VRNo. '" + UCRefNo + "," + ex.GetBaseException().ToString();
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//                else _netWrapper.LogText = "Data not available";
//            }
//            else _netWrapper.LogText = "Quote table body not loaded after save to draft for VRNo. "+UCRefNo;
//        }
//        else _netWrapper.LogText = "Quote table not loaded after save to draft for VRNo. " + UCRefNo;

//    }
//}

//public bool Check_SaveTo_Draft_Present()
//{
//    DOMInputElement _btnDraft = (DOMInputElement)_netWrapper.GetElementByType("input", "Save as Draft", "value");
//    if (_btnDraft == null)
//        return true;
//    else return false;
//}

//public bool Fill_Price_Validity_Date(string QuoteFile)
//{
//    bool result = false;
//    DateTime dt = DateTime.Now.Date;
//    DateTime dtValidDate = dt.AddDays(Convert.ToDouble(Price_Validity_Date));
//    DOMInputElement _elePriceValid = (DOMInputElement)_netWrapper.GetElementbyId(pagevar.txtPriceValidity);
//    DOMElement _eleCalender = _netWrapper.GetElementByType("label", "PriceValidId", "for");
//    if (_eleCalender != null)
//    {
//        string a = _eleCalender.GetAttribute("for");
//        _eleCalender.Click();
//        bool res = _netWrapper.WaitForElementbyId(pagevar.PriceValid_calender, true, false);
//        if (res == true)
//        {
//            if (_elePriceValid != null)
//            {
//                string validDate = dtValidDate.ToString("dd-MMM-yyyy");
//                DOMInputElement c = (DOMInputElement)_netWrapper.GetElementbyClass("input-mini form-control active");
//                if (c != null)
//                {
//                    //c.SetAttribute("style", "display: block;");
//                    //JSValue _value = _netWrapper.browser.ExecuteJavaScriptAndReturnValue("function B() {var NewDate = kendo.toString(27-Apr-2017, 'dd-MMM-yyyy'); $('#PriceValidId').data('kendoDatePicker').value(NewDate); $('#PriceValidId').change();}");
//                    //_netWrapper.ExecuteJavaScript("function B(a) { var NewDate = kendo.toString(27-Apr-2017, 'dd-MMM-yyyy'); $('#PriceValidId').data('kendoDatePicker').value(NewDate); $('#PriceValidId').change(); alert(a); }; B(); return true;", validDate);

//                    _netWrapper.browser.ExecuteJavaScript("alert('test');");

//                    System.Threading.ManualResetEvent waitEvent = new System.Threading.ManualResetEvent(false);
//                    _netWrapper.browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
//                    {
//                      //  this._document = e.Browser.GetDocument();
//                       // this.CurrentURL = e.ValidatedURL;
//                    };

//                    bool resa = waitEvent.WaitOne(7000);

//                    c.Value = validDate;
//                    _elePriceValid.Value = validDate;
//                    string _validDate = c.Value;

//                    KeyParams paramersa = new KeyParams(VirtualKeyCode.RETURN, '\n');
//                    _netWrapper.browser.KeyDown(paramersa);
//                    _netWrapper.browser.KeyUp(paramersa);

//                    if (_validDate != "")
//                    { _netWrapper.LogText = "Price validity date added."; result = true; }
//                    else
//                    {
//                        result = false;
//                        MoveFileToError(QuoteFile, "Unable to fill Price Validity Date for VRNo. " + UCRefNo, Quote_UploadPath + @"\" + MsgNumber);
//                    }
//                }
//            }
//        }
//        else
//        {
//            result = false;
//            MoveFileToError(QuoteFile, "Unable to get Price Validity calendar for VRNo. " + UCRefNo, Quote_UploadPath + @"\" + MsgNumber);
//        }
//    }
//    else
//    {
//        result = false;
//        MoveFileToError_WithoutScreenShot(QuoteFile, "Unable to get price valid until datetimepicker for VRNo '" + UCRefNo, Quote_UploadPath + @"\" + MsgNumber);
//    }
//    return result;
//}
//public bool Add_ValidDate_Remarks()
//{
//    string validDate = "";

//    if (dtExpDate != "")
//    {
//        validDate = Convert.ToDateTime(dtExpDate).ToString("dd-MMM-yyyy");
//    }
//    else
//    {
//        DateTime dt = DateTime.Now.Date;
//        DateTime dtValidDate = dt.AddDays(Convert.ToDouble(Price_Validity_Date));
//        validDate = dtValidDate.ToString("dd-MMM-yyyy");
//    }

//    DOMTextAreaElement _ele = (DOMTextAreaElement)_netWrapper.GetElementByType("textarea", pagevar.txtVendorRemarks, "data-bind");
//    if (_ele != null)
//    {
//        string a = _ele.Value;
//        string b = "Price Valid Until: " + validDate;
//        string sFile = ScreenShotPath + "\\BS_Quote" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
//        _netWrapper.PrintPDF(sFile, false);
//        _ele.Value = a + "\n" + b;
//        Thread.Sleep(500);
//        if (_ele.Value.Contains(b)) { _netWrapper.LogText = "Price validity date added in vendor remarks."; return true; }
//        else { _netWrapper.LogText = "Unable to add Price validity date in vendor remarks."; return false; }
//    }
//    else { _netWrapper.LogText = "Unable to add Price validity date in vendor remarks."; return false; }

//}

#endregion