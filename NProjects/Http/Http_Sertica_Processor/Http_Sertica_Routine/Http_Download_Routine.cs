using HtmlAgilityPack;
using LeSCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Globalization;

namespace Http_Sertica_Routine
{
    public class Http_Download_Routine :LeSCommon.LeSCommon
    {
        public int iRetry = 0, itemIdx = 0, ItemIncreament = 0;
        public string sAuditMesage = "", sDoneFile = "", VRNO = "", DocCreadted = "", DocTitle = "", ReplyBefore = "", Vessel = "", ASPXAUTH = "", Domain = "", DocType = "",
            ImgFile = "", CurrenctXMLFile = "", ImgPath = "", startText = "", endText = "";
        public string[] Actions;
        List<string> _skipLines = new List<string>();
        Dictionary<string, string> dctPostAttachment = new Dictionary<string, string>();
       
        public Http_Download_Routine()
        {
            GetSkipText();
        }

        public void LoadAppsettings()
        {
            try
            {
                iRetry = 0;
                IsUrlEncoded = false;
                SessionIDCookieName = "ASP.NET_SessionId";//12-2-2018
                URL = dctAppSettings["SITE_URL"].Trim();
                Userid = dctAppSettings["USERNAME"].Trim();
                Password = dctAppSettings["PASSWORD"].Trim();
                Domain = dctAppSettings["DOMAIN"].Trim();
                BuyerCode = dctAppSettings["BUYERCODE"].Trim();
                SupplierCode = dctAppSettings["SUPPLIERCODE"].Trim();
                Actions = dctAppSettings["ACTIONS"].Trim().Split(',');
                ImgPath = dctAppSettings["IMAGE_PATH"].Trim();
                AuditPath = dctAppSettings["AUDITPATH"].Trim();
                LoginRetry = convert.ToInt(dctAppSettings["LOGINRETRY"].Trim());
                if (AuditPath == "") AuditPath = AppDomain.CurrentDomain.BaseDirectory + "AuditLog";
                if (ImgPath == "") ImgPath = AppDomain.CurrentDomain.BaseDirectory + "Attachments";
                if (!Directory.Exists(ImgPath)) Directory.CreateDirectory(ImgPath);
            }
            catch (Exception e)
            {
                sAuditMesage = "Exception in Initialise: " + e.GetBaseException().ToString();
                LogText = sAuditMesage;
                //CreateAuditFile("", "Sertica_HTTP_Downloader", "", "Error", sAuditMesage,BuyerCode,SupplierCode,AuditPath);
            }
        }

        public override bool DoLogin(string validateNodeType, string validateAttribute, string attributeValue, bool bload = true)
        {
            bool isLoggedin = false;
            try
            {
                LoadURL("input", "id", "ButtonLogin");//true
                if (_httpWrapper._dctStateData.Count > 0)
                {
                    dctPostDataValues.Clear();
                    dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                    dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                    dctPostDataValues.Add("__EVENTVALIDATION", "");
                    dctPostDataValues.Add("TextBoxUsername", Uri.EscapeDataString(Userid));
                    dctPostDataValues.Add("TextBoxPassword", Uri.EscapeDataString(Password));
                    dctPostDataValues.Add("ButtonLogin", "Login");
                    isLoggedin = base.DoLogin(validateNodeType, validateAttribute, attributeValue, false);

                    if (isLoggedin)
                    {
                        LogText = "Logged in successfully";
                    }
                    if (!isLoggedin)
                    {
                        if (iRetry == LoginRetry)
                        {
                            string filename = ImgPath + "\\Sertica_LoginFailed_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + Domain + ".png";
                            if (!PrintScreen(filename)) filename = "";
                            LogText = "Login failed";
                            CreateAuditFile(filename, "Sertica_HTTP_Downloader", "", "Error", "LeS-1014:Unable to login.", BuyerCode, SupplierCode, AuditPath);
                        }
                        else
                        {
                            iRetry++;
                            LogText = "Login retry";
                            isLoggedin = DoLogin(validateNodeType, validateAttribute, attributeValue, false);
                        }
                    }
                }
                else
                {
                    string filename = ImgPath + "\\Sertica_LoginFailed_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + Domain + ".png";
                    if (!PrintScreen(filename)) filename = "";
                    LogText = "Unable to load URL" + URL;
                    CreateAuditFile(filename, "Sertica_HTTP_Downloader", "", "Error", "LeS-1016:Unable to load URL" + URL, BuyerCode, SupplierCode, AuditPath);
                }
            }
            catch (Exception e)
            {
                LogText = "Exception while login : " + e.GetBaseException().ToString();
                if (iRetry > LoginRetry)
                {
                    LogText = "Login failed";
                    //CreateAuditFile("", "Sertica_HTTP_Downloader", "", "Error", "Unable to login. Error : " + e.Message, BuyerCode, SupplierCode, AuditPath);
                    CreateAuditFile("", "Sertica_HTTP_Downloader", "", "Error", "LeS-1014:Unable to login due to " + e.Message, BuyerCode, SupplierCode, AuditPath);
                }
                else
                {
                    iRetry++;
                    LogText = "Login retry";
                    isLoggedin = DoLogin(validateNodeType, validateAttribute, attributeValue);
                }
            }
            return isLoggedin;
        }

        #region RFQ
        public void ProcessRFQ()
        
        {
            try
            {
                this.DocType = "RFQ";
                List<string> _lstNewRFQs = GetNewRFQs();
                if (_lstNewRFQs.Count > 0)
                {
                    DownloadRFQ(_lstNewRFQs);
                }
                else
                {
                    LogText = "No new RFQ found.";
                }
            }
            catch (Exception e)
            {
                //WriteErrorLog_With_Screenshot("Exception in Process RFQ : " + e.GetBaseException().ToString());
                WriteErrorLog_With_Screenshot("Unable to process file due to " + e.GetBaseException().ToString(),"LeS-1004:");
            }
        }

        public List<string> GetNewRFQs()
        {
            List<string> _lstNewRFQs = new List<string>();
            List<string> slProcessedItem = GetProcessedItems(eActions.RFQ);
            _lstNewRFQs.Clear();
            _httpWrapper._CurrentDocument.LoadHtml(_httpWrapper._CurrentResponseString);
            HtmlNodeCollection _nodes = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='MainPage1_QuotationListView1_DataList1']//tr[@class='quotationLineBackground1' or @class='quotationLineBackground2']");
            if (_nodes != null)
            {
                if (_nodes.Count > 0)
                {
                    foreach (HtmlNode _row in _nodes)
                    {
                        HtmlNodeCollection _rowData = _row.ChildNodes;
                        string VRNo = _rowData[1].InnerText.Trim();
                        string Vessel = _rowData[3].InnerText.Trim();
                        string Title = _rowData[5].InnerText.Trim();
                        string ReceivedDate = _rowData[7].InnerText.Trim();
                        string ReplyBefore = _rowData[9].InnerText.Trim();
                        string _guid = VRNo + "|" + Vessel + "|" + ReceivedDate + "|" + Title + "|" + ReplyBefore;
                        if (!_lstNewRFQs.Contains(_guid) && !slProcessedItem.Contains("RFQ" + "|" + VRNo + "|" + ReceivedDate))
                        {
                            _lstNewRFQs.Add(_guid);
                        }
                    }
                }
                else
                    LogText = "No new RFQs found.";
            }
            return _lstNewRFQs;
        }

        public void DownloadRFQ(List<string> _lstNewRFQs)
        {
            foreach (string strRFQ in _lstNewRFQs)
            {
                try
                {
                    string[] lst = strRFQ.Split('|');
                    this.VRNO = lst[0];
                    this.Vessel = lst[1];
                    string RfqDate = lst[2];
                    this.DocCreadted = RfqDate.Trim();
                    this.DocTitle = lst[3].Trim();
                    this.ReplyBefore = lst[4].Trim();
                    LogText = "Processing RFQ " + VRNO.Trim();
                    if (_httpWrapper._dctStateData.Count == 0)
                    {
                        this.URL = URL.Replace("Pages/Login.aspx", "Default.aspx");
                        //LoadURL("table", "id", "MainPage1_QuotationListView1_DataList1", false);
                        LoadURL("table", "id", "MainPage1_QuotationListView1_DataList1");
                    }

                    if (_httpWrapper._dctStateData.Count > 0)
                    {
                        GetDataForScreenshot();

                        #region /* Post Data for opening RFQ details page
                        dctPostDataValues.Clear();
                        if (_httpWrapper._dctStateData.ContainsKey("__VIEWSTATEFIELDCOUNT")) dctPostDataValues.Add("__VIEWSTATEFIELDCOUNT", _httpWrapper._dctStateData["__VIEWSTATEFIELDCOUNT"]);//new
                        dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                        if (_httpWrapper._dctStateData.ContainsKey("__VIEWSTATEFIELDCOUNT"))
                        {
                            for (int i = 1; i <= Convert.ToInt32(_httpWrapper._dctStateData["__VIEWSTATEFIELDCOUNT"]); i++)
                                if (_httpWrapper._dctStateData.ContainsKey("__VIEWSTATE" + i)) dctPostDataValues.Add("__VIEWSTATE" + i, _httpWrapper._dctStateData["__VIEWSTATE" + i]);//new
                        }
                        dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                        if (_httpWrapper._dctStateData.ContainsKey("__VIEWSTATEENCRYPTED"))
                            dctPostDataValues.Add("__VIEWSTATEENCRYPTED", _httpWrapper._dctStateData["__VIEWSTATEENCRYPTED"]);
            
                        if (_httpWrapper._dctStateData.ContainsKey("__EVENTVALIDATION")) dctPostDataValues.Add("__EVENTVALIDATION", _httpWrapper._dctStateData["__EVENTVALIDATION"]);
                        dctPostDataValues.Add("MainPage1%24TextBoxSearch", this.VRNO);
                        dctPostDataValues.Add("MainPage1_menuWebDataTree_clientState", "%5B%5B%5B%5Bnull%2Cnull%2C%22loading...%22%2C1%2Cnull%2Cnull%2Cnull%2C0%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2C%220%22%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2C0%5D%5D%2C%5B%5B%5B%5B%5Bnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%5D%5D%2C%5B%5D%2C%5B%5D%5D%2C%5B%7B%7D%2C%5B%5D%5D%2Cnull%5D%2C%5B%5B%5B%5Bnull%2Cnull%2Cnull%2Cnull%5D%5D%2C%5B%5D%2C%5B%5D%5D%2C%5B%7B%7D%2C%5B%5D%5D%2Cnull%5D%5D%2Cnull%5D%2C%5B%7B%7D%2C%5B%7B%7D%5D%5D%2C%5B%7B%22scrollTop%22%3A0%2C%22scrollLeft%22%3A0%7D%2C%5B%5B%5D%2C%7B%7D%5D%5D%5D");
                        if (_httpWrapper._dctStateData.ContainsKey("_IG_CSS_LINKS_"))//20-2-2018
                        dctPostDataValues.Add("_IG_CSS_LINKS_", _httpWrapper._dctStateData["_IG_CSS_LINKS_"]);
                        dctPostDataValues.Add("hiddenInputToUpdateATBuffer_CommonToolkitScripts", "0");
                        dctPostDataValues.Add("__ASYNCPOST", "true");
                        dctPostDataValues.Add("MainPage1%24ButtonSearch", "Search");
                        #endregion

                        this.URL = URL.Replace("Pages/Login.aspx", "Default.aspx");
                        if (PostURL("input", "id", "MainPage1_QuotationDetailsView1_QuotationLinesEditor1_ButtonSave"))//, false)
                        {
                            string PageGUID = this.DocType + "|" + VRNO + "|" + RfqDate;
                            Dictionary<string, string> lstHeaderData = GetHeaderData();
                            if (lstHeaderData.Count > 0)
                            {
                                List<List<string>> lstItems = GetItemsData();
                                if (lstItems.Count > 0)
                                {
                                    LeSXML.LeSXML _lesXML = new LeSXML.LeSXML();
                                    ExporttoHeader(lstHeaderData, ref _lesXML);
                                    ExporttoItems(lstItems, ref _lesXML);
                                    GetAttachments(ref _lesXML);
                                    _lesXML.WriteXML();

                                    if (File.Exists(CurrenctXMLFile))
                                    {
                                        LogText = "RFQ " + VRNO + " downloaded successfully.";
                                        string Audit = "RFQ '" + _lesXML.FileName + "' for ref '" + VRNO + "' downloaded successfully.";
                                        CreateAuditFile(_lesXML.FileName, "Sertica_HTTP_Downloader", VRNO, "Downloaded", Audit, BuyerCode, SupplierCode, AuditPath);
                                        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "GUID_Files\\" + this.Domain + "_" + this.SupplierCode + "_GUID.txt", PageGUID + Environment.NewLine);
                                    }
                                    else
                                    {
                                        //WriteErrorLog_With_Screenshot("Unable to download RFQ '" + _lesXML.FileName + "' for ref '" + VRNO + "'.");
                                        WriteErrorLog_With_Screenshot("Unable to process file " + _lesXML.FileName + " for ref " + VRNO, "LeS-1004:");
                                    }
                                }
                                else
                                {
                                    //WriteErrorLog("Unable to get RFQ Item Details for ref '" + VRNO + "'.");
                                    WriteErrorLog("Unable to filter details for ref " + VRNO, "LeS-1006:");
                                }
                            }
                            else
                            {
                                //WriteErrorLog("Unable to get RFQ Details for ref '" + VRNO + "'.");
                                WriteErrorLog("Unable to filter details for ref " + VRNO, "LeS-1006:");
                            }
                        }
                        else
                        {
                            //WriteErrorLog_With_Screenshot("Unable to load RFQ " + VRNO + " details.");
                            WriteErrorLog_With_Screenshot("Unable to filter details for ref " + VRNO, "LeS-1006:");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _httpWrapper._CurrentResponseString = _httpWrapper._CurrentResponseString.Replace("Images/Logo.gif", "Images/" + Domain + "/Logo.gif");
                    //WriteErrorLog_With_Screenshot("Unable to load RFQ '" + VRNO + "' details due to " + ex.GetBaseException().Message.ToString());
                    WriteErrorLog_With_Screenshot("Unable to filter details for ref" + VRNO + " due to " + ex.GetBaseException().Message.ToString(), "LeS-1006:");
                }
            }
        }
        #endregion

        #region PO
        public void ProcessPO()
        {
            try
            {
                this.DocType = "PO";
                List<string> _lstNewPOs = GetNewPOs();
                if (_lstNewPOs.Count > 0)
                {
                    DownloadPO(_lstNewPOs);
                }
                else
                {
                    LogText = "No new PO found.";
                }
            }
            catch (Exception e)
            {
                //WriteErrorLog_With_Screenshot("Exception in Process PO : " + e.GetBaseException().ToString());
                WriteErrorLog_With_Screenshot("Unable to process file due to " + e.GetBaseException().ToString(),"LeS-1004:");
            }
        }

        public List<string> GetNewPOs()
        {
            List<string> _lstNewPOs = new List<string>();
            List<string> slProcessedItem = GetProcessedItems(eActions.PO);
            _lstNewPOs.Clear();
            this.URL = URL.Replace("Pages/Login.aspx", "Default.aspx");
            LoadURL("table", "id", "MainPage1_QuotationListView1_DataList1");//, false
            _httpWrapper._CurrentDocument.LoadHtml(_httpWrapper._CurrentResponseString);
            HtmlNodeCollection _nodes = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='MainPage1_PurchaseOrderListView1_DataList1']//tr[@class='quotationLineBackground1' or @class='quotationLineBackground2']");
            if (_nodes != null)
            {
                if (_nodes.Count > 0)
                {
                    foreach (HtmlNode _row in _nodes)
                    {
                        HtmlNodeCollection _rowData = _row.ChildNodes;
                        string VRNo = _rowData[1].InnerText.Trim();
                        string Vessel = _rowData[3].InnerText.Trim();
                        string Title = _rowData[5].InnerText.Trim();
                        string OrderDate = _rowData[7].InnerText.Trim();
                        string _guid = VRNo + "|" + Vessel + "|" + OrderDate + "|" + Title;
                        if (!_lstNewPOs.Contains(_guid) && !slProcessedItem.Contains("PO" + "|" + VRNo + "|" + OrderDate))
                        {
                            _lstNewPOs.Add(_guid);
                        }
                    }
                }
                else
                    LogText = "No new POs found.";
            }
            return _lstNewPOs;
        }

        public void DownloadPO(List<string> _lstNewPOs)
        {
            foreach (string strPO in _lstNewPOs)
            {
                try
                {
                    string[] lst = strPO.Split('|');
                    this.VRNO = lst[0];
                    this.Vessel = lst[1];
                    string OrderDate = lst[2];
                    this.DocCreadted = OrderDate.Trim();
                    this.DocTitle = lst[3].Trim();
                    LogText = "Processing PO " + VRNO.Trim();
                    if (_httpWrapper._dctStateData.Count == 0)
                    {
                        this.URL = URL.Replace("Pages/Login.aspx", "Default.aspx");
                        LoadURL("table", "id", "MainPage1_PurchaseOrderDetailsView1_ButtonConfirm");//, false
                    }
                    if (_httpWrapper._dctStateData.Count > 0)
                    {
                        GetDataForScreenshot();

                        #region /* Post Data for opening PO details page
                        dctPostDataValues.Clear();
                        if (_httpWrapper._dctStateData.ContainsKey("__VIEWSTATEFIELDCOUNT")) dctPostDataValues.Add("__VIEWSTATEFIELDCOUNT", _httpWrapper._dctStateData["__VIEWSTATEFIELDCOUNT"]);//new
                        dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                        if (_httpWrapper._dctStateData.ContainsKey("__VIEWSTATEFIELDCOUNT"))
                        {
                            for (int i = 1; i <= Convert.ToInt32(_httpWrapper._dctStateData["__VIEWSTATEFIELDCOUNT"]); i++)
                                if (_httpWrapper._dctStateData.ContainsKey("__VIEWSTATE" + i)) dctPostDataValues.Add("__VIEWSTATE" + i, _httpWrapper._dctStateData["__VIEWSTATE" + i]);//new
                        }
                        dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                        dctPostDataValues.Add("__VIEWSTATEENCRYPTED", _httpWrapper._dctStateData["__VIEWSTATEENCRYPTED"]);
                        if (_httpWrapper._dctStateData.ContainsKey("__EVENTVALIDATION")) dctPostDataValues.Add("__EVENTVALIDATION", _httpWrapper._dctStateData["__EVENTVALIDATION"]);
                        dctPostDataValues.Add("MainPage1%24TextBoxSearch", this.VRNO);
                        dctPostDataValues.Add("MainPage1_menuWebDataTree_clientState", "%5B%5B%5B%5Bnull%2Cnull%2C%22loading...%22%2C1%2Cnull%2Cnull%2Cnull%2C0%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2C%220%22%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2C0%5D%5D%2C%5B%5B%5B%5B%5Bnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%5D%5D%2C%5B%5D%2C%5B%5D%5D%2C%5B%7B%7D%2C%5B%5D%5D%2Cnull%5D%2C%5B%5B%5B%5Bnull%2Cnull%2Cnull%2Cnull%5D%5D%2C%5B%5D%2C%5B%5D%5D%2C%5B%7B%7D%2C%5B%5D%5D%2Cnull%5D%5D%2Cnull%5D%2C%5B%7B%7D%2C%5B%7B%7D%5D%5D%2C%5B%7B%22scrollTop%22%3A0%2C%22scrollLeft%22%3A0%7D%2C%5B%5B%5D%2C%7B%7D%5D%5D%5D");
                        dctPostDataValues.Add("_IG_CSS_LINKS_", _httpWrapper._dctStateData["_IG_CSS_LINKS_"]);
                        dctPostDataValues.Add("hiddenInputToUpdateATBuffer_CommonToolkitScripts", "0");
                        dctPostDataValues.Add("__ASYNCPOST", "true");
                        dctPostDataValues.Add("MainPage1%24ButtonSearch", "Search");
                        #endregion
                        this.URL = URL.Replace("Pages/Login.aspx", "Default.aspx");
                        if (PostURL("input", "id", "MainPage1_PurchaseOrderDetailsView1_ButtonConfirm"))//, false
                        {
                            string PageGUID = this.DocType + "|" + VRNO + "|" + OrderDate;
                            Dictionary<string, string> lstHeaderData = GetHeaderData();
                            if (lstHeaderData.Count > 0)
                            {
                                List<List<string>> lstItems = GetItemsData();
                                if (lstItems.Count > 0)
                                {
                                    LeSXML.LeSXML _lesXML = new LeSXML.LeSXML();
                                    ExporttoHeader(lstHeaderData, ref _lesXML);
                                    ExporttoItems(lstItems, ref _lesXML);
                                    GetAttachments(ref _lesXML);
                                    _lesXML.WriteXML();

                                    if (File.Exists(CurrenctXMLFile))
                                    {
                                        LogText = "PO " + VRNO + " downloaded successfully.";
                                        string Audit = "PO '" + _lesXML.FileName + "' for ref '" + VRNO + "' downloaded successfully.";
                                        CreateAuditFile(_lesXML.FileName, "Sertica_HTTP_Downloader", VRNO, "Downloaded", Audit, BuyerCode, SupplierCode, AuditPath);
                                        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "GUID_Files\\" + this.Domain + "_" + this.SupplierCode + "_GUID.txt", PageGUID + Environment.NewLine);
                                    }
                                    else
                                    {
                                        //WriteErrorLog_With_Screenshot("Unable to download PO '" + _lesXML.FileName + "' for ref '" + VRNO + "'.");
                                        WriteErrorLog_With_Screenshot("Unable to process file " + _lesXML.FileName + " for ref " + VRNO, "LeS-1004:");
                                    }
                                }
                                else
                                {
                                    //WriteErrorLog("Unable to get PO Item Details for ref '" + VRNO + "'.");
                                    WriteErrorLog("Unable to filter details for ref " + VRNO, "LeS-1006:");
                                }
                            }
                            else
                            {
                                //WriteErrorLog("Unable to get PO Details for ref '" + VRNO + "'.");
                                WriteErrorLog("Unable to filter details for ref " + VRNO, "LeS-1006:");
                            }
                        }
                        else
                        {
                            //WriteErrorLog_With_Screenshot("Unable to load PO " + VRNO + " details.");
                            WriteErrorLog_With_Screenshot("Unable to load url for " + VRNO, "LeS-1016:");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _httpWrapper._CurrentResponseString = _httpWrapper._CurrentResponseString.Replace("Images/Logo.gif", "Images/" + Domain + "/Logo.gif");
                    WriteErrorLog_With_Screenshot("Unable to process file for " + VRNO + " due to " + ex.GetBaseException().Message.ToString(), "LeS-1004:");
                }
            }
        }
        #endregion

        public Dictionary<string, string> GetHeaderData()
        {
            Dictionary<string, string> _xmlHeader = new Dictionary<string, string>();
            string HeaderMapFile = this.DocType + "HEADER_MAP_" + this.Domain + ".txt";
            string MAPPath = AppDomain.CurrentDomain.BaseDirectory + "MAP_FILES";
            if (File.Exists(@MAPPath + @"\" + HeaderMapFile))
            {
                string[] _lines = File.ReadAllLines(@MAPPath + @"\" + HeaderMapFile);
                for (int i = 0; i < _lines.Length; i++)
                {
                    string[] _keys = _lines[i].Trim().Trim('=').Trim().Split('=');
                    if (_keys.Length > 1)
                    {
                        if (_httpWrapper.GetElement(_keys[1]) != null)
                        {
                            string _value = _httpWrapper.GetElement(_keys[1]).GetAttributeValue("Value", "");
                            if (_value == "")
                                _value = _httpWrapper.GetElement(_keys[1]).InnerText;
                            if (String.IsNullOrEmpty(_value)) { _value = string.Empty; }
                            _xmlHeader.Add(_keys[0], _value);
                        }
                        else
                        {
                            _xmlHeader.Add(_keys[0], "");
                        }
                    }
                }


                _xmlHeader.Add("DOC_TYPE", this.DocType);
                _xmlHeader.Add("SUBJECT", this.DocTitle);
                if (this.DocType == "RFQ" && _xmlHeader["RFQ_DATE"].Trim() == "")
                {
                    _xmlHeader["RFQ_DATE"] = this.DocCreadted;
                }
                else if (this.DocType == "PO" && _xmlHeader["ORDER_DATE"].Trim() == "")
                {
                    _xmlHeader["ORDER_DATE"] = this.DocCreadted;
                }
                //if (_xmlHeader["LATE_DATE"].Trim() == "")
                //{
                //    _xmlHeader["LATE_DATE"] = this.ReplyBefore;
                //}

                if (_httpWrapper._CurrentResponseString.Contains("MainPage1_QuotationDetailsView1_QuotationLinesEditor1_CurrencyPicker1_DropDownListCurrency"))
                {
                    var options = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='MainPage1_QuotationDetailsView1_QuotationLinesEditor1_CurrencyPicker1_DropDownListCurrency']/option[@selected='selected']");
                   // if (options.Count > 0)
                    if (options.Count == 1 && options[0].NextSibling.InnerText.Trim()!="")
                    {
                        //string strCurrency = "";
                        //for (int i = 0; i < options.Count; i++)
                        //{
                        //    var val = options[i].OuterHtml;
                        //    if (val.Contains("selected"))
                        //    {
                        //        strCurrency = val.Split(new string[] { "value=" }, StringSplitOptions.None)[1].Trim('>');
                        //        break;
                        //    }
                        //}
                      //  if (strCurrency.Contains("\"")) strCurrency = strCurrency.Replace("\"", "");
                        //_xmlHeader["CURRENCY"] = convert.ToString(strCurrency);
                        
                        _xmlHeader["CURRENCY"] = convert.ToString(options[0].NextSibling.InnerText);
                    }
                }
                try
                {
                    if (this.DocType == "PO")
                    {
                        if (_httpWrapper._CurrentResponseString.Contains("MainPage1_PurchaseOrderDetailsView1_DeliveryCostsDataList"))
                        {
                            HtmlNodeCollection _nodes = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='MainPage1_PurchaseOrderDetailsView1_DeliveryCostsDataList']//tr");
                            if (_nodes != null)
                            {
                                if (_nodes.Count > 0)
                                {
                                    double Frieght = 0, PackingCost = 0, OtherCost = 0;
                                    foreach (HtmlNode _row in _nodes)
                                    {
                                        HtmlNodeCollection _rowData = _row.ChildNodes;
                                        if (_rowData[1].InnerHtml.ToUpper().Contains("FREIGHT"))
                                            Frieght = Convert.ToDouble(SplitNumericValue(_rowData[1].InnerText)[0]);
                                        else if (_rowData[1].InnerHtml.ToUpper().Contains("HANDLING"))
                                            PackingCost = Convert.ToDouble(SplitNumericValue(_rowData[1].InnerText)[0]);
                                        else
                                            OtherCost += Convert.ToDouble(SplitNumericValue(_rowData[1].InnerText)[0]);
                                    }
                                    _xmlHeader["FREIGHT_AMT"] = Frieght.ToString();
                                    _xmlHeader["PACKING_COST"] = (OtherCost + PackingCost).ToString();
                                }
                            }
                        }
                        else
                        {
                            double Freight = 0, PackingCost = 0;
                            if (_xmlHeader.ContainsKey("FREIGHT_AMT"))
                            {
                                if (_xmlHeader["FREIGHT_AMT"].Trim().Length > 0)
                                {
                                    string _val = SplitNumericValue(_xmlHeader["FREIGHT_AMT"].Trim())[0].Trim();
                                    Freight = Convert.ToDouble(_val);
                                }
                            }
                            if (_xmlHeader.ContainsKey("PACKING_COST"))
                            {
                                if (_xmlHeader["PACKING_COST"].Trim().Length > 0)
                                {
                                    string _val = SplitNumericValue(_xmlHeader["PACKING_COST"].Trim())[0].Trim();
                                    PackingCost = Convert.ToDouble(_val);
                                }
                            }

                            _xmlHeader["FREIGHT_AMT"] = Freight.ToString();
                            _xmlHeader["PACKING_COST"] = PackingCost.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogText = "Unable to read delivery costs - " + ex.Message;
                }

                this.ReplyBefore = "";
                this.DocTitle = "";
                this.DocCreadted = "";

            }
            else throw new Exception("'" + HeaderMapFile + "' Map file not found.");
            return _xmlHeader;
        }

        public List<List<string>> GetItemsData()
        {
            try
            {
                string HeaderMapFile = this.DocType + "HEADER_MAP_" + this.Domain + ".txt";
                string MAPPath = AppDomain.CurrentDomain.BaseDirectory + "MAP_FILES";

                List<List<string>> lstItems = new List<List<string>>();
                dctPostAttachment.Clear();
                if (this.DocType == "RFQ")
                {

                    _httpWrapper._CurrentDocument.LoadHtml(_httpWrapper._CurrentResponseString);
                    HtmlNodeCollection _nodes = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='MainPage1_QuotationDetailsView1_QuotationLinesEditor1_DataListQuotationLines']//tr[@class='quotationLineBackground1 tr1' or @class='quotationLineBackground2 tr1' or @class='quotationLineBackground1']");//or @class='quotationLineBackground1'//20-2-2018
                    if (_nodes != null)
                    {
                        if (_nodes.Count > 0)
                        {
                            #region /* Item Settings */
                            if (File.Exists(@MAPPath + @"\" + HeaderMapFile))
                            {
                                string[] _lines = File.ReadAllLines(@MAPPath + @"\" + HeaderMapFile);
                                for (int i = 0; i < _lines.Length; i++)
                                {
                                    string[] _keys = _lines[i].Split('=');
                                    if (_keys.Length > 1 && _keys[0].Trim().StartsWith("ITEM"))
                                    {
                                        if (_keys[0].Trim().ToUpper() == "ITEM_START_INDEX")
                                        {
                                            itemIdx = convert.ToInt(_keys[1]);
                                        }
                                        if (_keys[0].Trim().ToUpper() == "ITEM_ROW_INCREAMENT")
                                        {
                                            ItemIncreament = convert.ToInt(_keys[1]);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region /*Get Items*/
                            foreach (HtmlNode _row in _nodes)
                            {
                                HtmlNodeCollection _rowData = _row.ChildNodes;
                              //  int No = convert.ToInt(_rowData[1].InnerText.Trim());//20-02-2018
                                if (_rowData[1].SelectSingleNode(".//span") != null)//20-2-2018
                                {
                                    string _spanId = _rowData[1].SelectSingleNode(".//span").GetAttributeValue("Id", "");//20-02-2018
                                    string[] _strFields = _spanId.Split('_');
                                    string ctlID = _strFields[4].Trim();
                                    int No = convert.ToInt(_httpWrapper.GetElement("MainPage1_QuotationDetailsView1_QuotationLinesEditor1_DataListQuotationLines_" + ctlID + "_LabelItemNo").InnerText.Trim());
                                    if (No > 0)
                                    {
                                        //string _spanId = _rowData[1].SelectSingleNode(".//span").GetAttributeValue("Id", "");20-02-2018
                                        //string[] _strFields = _spanId.Split('_');
                                        //string ctlID = _strFields[4].Trim();

                                        double Qty = 0;
                                        Qty = Convert.ToDouble(_httpWrapper.GetElement("MainPage1_QuotationDetailsView1_QuotationLinesEditor1_DataListQuotationLines_" + ctlID + "_TextBoxQuantity").GetAttributeValue("Value", ""));
                                        dctPostDataValues.Add(Uri.EscapeDataString("MainPage1_QuotationDetailsView1_QuotationLinesEditor1_DataListQuotationLines_" + ctlID + "_TextBoxQuantity"), Convert.ToString(Qty));

                                        string unit = "";
                                        var options = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='MainPage1_QuotationDetailsView1_QuotationLinesEditor1_DataListQuotationLines_" + ctlID + "_MeasureUnitPicker1_DropDownListMeasureUnit']/option");
                                        if (options.Count > 0)
                                        {
                                            for (int i = 0; i < options.Count; i++)
                                            {
                                                var val = options[i].OuterHtml;
                                                if (val.Contains("selected"))
                                                {
                                                    unit = val.Split(new string[] { "value=" }, StringSplitOptions.None)[1].Trim('>');
                                                    if (unit.Contains("\"")) unit = unit.Replace("\"", "");
                                                    break;
                                                }
                                            }
                                        }
                                        string Desc = "";
                                        string ItemName = Desc = _httpWrapper.GetElement("MainPage1_QuotationDetailsView1_QuotationLinesEditor1_DataListQuotationLines_" + ctlID + "_LabelName").InnerText.Trim();

                                        string ItemCombinedDescr = "";
                                        ItemCombinedDescr = _httpWrapper.GetElement("MainPage1_QuotationDetailsView1_QuotationLinesEditor1_DataListQuotationLines_" + ctlID + "_LabelCombinedDescription").InnerText.Trim();

                                        string Remarks = "";
                                        if (_httpWrapper.GetElement("MainPage1_QuotationDetailsView1_QuotationLinesEditor1_DataListQuotationLines_" + ctlID + "_LabelRemarks") != null)//20-2-2018
                                            Remarks = _httpWrapper.GetElement("MainPage1_QuotationDetailsView1_QuotationLinesEditor1_DataListQuotationLines_" + ctlID + "_LabelRemarks").InnerText.Trim();
                                        if (Remarks != "")
                                            ItemName = Desc.Trim() + " " + ItemCombinedDescr.Trim();
                                        else
                                            Remarks = ItemCombinedDescr.Trim();

                                        dctPostAttachment.Add("MainPage1%24QuotationDetailsView1%24QuotationLinesEditor1%24DataListQuotationLines%24" + ctlID + "%24TextBoxName_TextBoxWatermarkExtender_ClientState", "");
                                        dctPostAttachment.Add("MainPage1%24QuotationDetailsView1%24QuotationLinesEditor1%24DataListQuotationLines%24" + ctlID + "%24TextBoxDescription_TextBoxWatermarkExtender_ClientState", "");

                                        #region /* Get Item Ref */
                                        string RefNo = "";
                                        RefNo = _httpWrapper.GetElement("MainPage1_QuotationDetailsView1_QuotationLinesEditor1_DataListQuotationLines_" + ctlID + "_TextBoxYourPartNo").GetAttributeValue("Value", "");

                                        if (RefNo.Trim().Length == 0)
                                        {
                                            string combinedRemarks = ItemCombinedDescr + " " + Remarks;
                                            if (combinedRemarks.Trim().Length > 0)
                                            {
                                                string _tempStr = combinedRemarks.Trim();
                                                if (_tempStr.Trim().ToLower().Contains("supplier spare part no."))
                                                {
                                                    _tempStr = _tempStr.Replace("Supplier Spare Part No.:", "|");
                                                }
                                                if (_tempStr.Trim().ToLower().Contains("supplier item no."))
                                                {
                                                    _tempStr = _tempStr.Replace("supplier item no.", "|");
                                                }
                                                if (_tempStr.Trim().ToLower().Contains("reference no."))
                                                {
                                                    _tempStr = _tempStr.Replace("Reference No.:", "|");
                                                }

                                                if (_tempStr.Trim().Contains("|"))
                                                {
                                                    _tempStr = _tempStr.Split('|')[1].Trim();
                                                    RefNo = _tempStr.Trim();
                                                    if (RefNo.Contains("\r\n")) RefNo = RefNo.Substring(0, RefNo.IndexOf("\r\n")).Trim();
                                                    if (RefNo.Contains("\n")) RefNo = RefNo.Substring(0, RefNo.IndexOf("\n")).Trim();

                                                    foreach (string _content in _skipLines)
                                                    {
                                                        if (RefNo.Contains(_content)) RefNo = RefNo.Substring(0, RefNo.IndexOf(_content)).Trim();
                                                    }

                                                    if (RefNo.Contains(";")) RefNo = RefNo.Substring(0, RefNo.IndexOf(';')).Trim();
                                                    if (RefNo.Contains(",")) RefNo = RefNo.Substring(0, RefNo.IndexOf(',')).Trim();
                                                }
                                            }
                                        }
                                        #endregion

                                        if (RefNo.Trim().Length > 0)
                                        {
                                            RefNo = RefNo.ToUpper().Replace("CAT", "").Replace("NO", "").Replace(".", "").Replace("PART", "").Trim();
                                        }

                                        List<string> item = new List<string>();
                                        item.Add(No.ToString()); //1
                                        item.Add(ItemName.Trim()); //2 //
                                        item.Add(Qty.ToString()); //3
                                        item.Add(unit.Trim()); //4
                                        item.Add(Remarks.Trim()); //5
                                        item.Add("0"); //6
                                        item.Add("0"); //7
                                        item.Add("0"); //8
                                        item.Add(RefNo); //9
                                        lstItems.Add(item);
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
                if (this.DocType == "PO")
                {
                    int indxNo = -1, indxQty = -1, indxDescr = -1, indxPartNo = -1, indxPrice = -1, indxDisc = -1, indxTotal = -1;
                    _httpWrapper._CurrentDocument.LoadHtml(_httpWrapper._CurrentResponseString);
                    HtmlNodeCollection _nodes = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='MainPage1_PurchaseOrderDetailsView1_DataListLineItems']//tr[@class='quotationLineBackground1' or @class='quotationLineBackground2']");
                    if (_nodes != null)
                    {
                        if (_nodes.Count > 0)
                        {
                            #region /* Item Settings */
                            Dictionary<string, string> itemHeaders = new Dictionary<string, string>();
                            if (File.Exists(MAPPath + @"\" + HeaderMapFile))
                            {
                                string[] _lines = File.ReadAllLines(MAPPath + @"\" + HeaderMapFile);
                                for (int i = 0; i < _lines.Length; i++)
                                {
                                    string[] _keys = _lines[i].Split('=');
                                    if (_keys.Length > 1 && _keys[0].Trim().StartsWith("ITEM_HEADER"))
                                    {
                                        itemHeaders.Add(_keys[0].Trim(), _keys[1].Trim().Replace(" ", ""));
                                    }
                                }
                            }
                            #endregion

                            #region /* GetItems */
                            HtmlNodeCollection _nodesHeader = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='MainPage1_PurchaseOrderDetailsView1_DataListLineItems']//tr//table//thead//th");
                            if (_nodesHeader != null)
                            {
                                if (_nodesHeader.Count > 0)
                                {
                                    int i = 0;
                                    foreach (HtmlNode _row in _nodesHeader)
                                    {

                                        if (_row.InnerText.Trim().Length > 0)
                                        {
                                            string header = _row.InnerText.Trim().ToUpper().Replace(" ", "");
                                            if (itemHeaders.ContainsKey("ITEM_HEADER_NO") && itemHeaders["ITEM_HEADER_NO"].Trim().ToUpper() == header)
                                                indxNo = i;
                                            else if (itemHeaders.ContainsKey("ITEM_HEADER_QTY") && itemHeaders["ITEM_HEADER_QTY"].Trim().ToUpper() == header)
                                                indxQty = i;
                                            else if (itemHeaders.ContainsKey("ITEM_HEADER_DESCR") && itemHeaders["ITEM_HEADER_DESCR"].Trim().ToUpper() == header)
                                                indxDescr = i;
                                            else if (itemHeaders.ContainsKey("ITEM_HEADER_PARTNO") && itemHeaders["ITEM_HEADER_PARTNO"].Trim().ToUpper() == header)
                                                indxPartNo = i;
                                            else if (itemHeaders.ContainsKey("ITEM_HEADER_PRICE") && itemHeaders["ITEM_HEADER_PRICE"].Trim().ToUpper() == header)
                                                indxPrice = i;
                                            else if (itemHeaders.ContainsKey("ITEM_HEADER_DISC") && itemHeaders["ITEM_HEADER_DISC"].Trim().ToUpper() == header)
                                                indxDisc = i;
                                            else if (itemHeaders.ContainsKey("ITEM_HEADER_TOTAL") && itemHeaders["ITEM_HEADER_TOTAL"].Trim().ToUpper() == header)
                                                indxTotal = i;
                                        }
                                        i++;
                                    }
                                }
                            }
                            foreach (HtmlNode _row in _nodes)
                            {
                                List<string> item = new List<string>();
                                HtmlNodeCollection _rowData = _row.SelectNodes(".//td");
                                if (_rowData.Count > 1)
                                {
                                    #region Get Order Items
                                    if (indxNo >= 0)
                                    {
                                        item.Add(convert.ToInt(_rowData[indxNo].InnerText).ToString());
                                    }
                                    else
                                    {
                                        throw new Exception("Item No not found in PO. Check Item Header mapping.");
                                    }

                                    string Remarks = "";
                                    if (indxDescr >= 0)
                                    {
                                        string Desc = "";
                                        HtmlNodeCollection lstSpans = _rowData[indxDescr].SelectNodes(".//span");
                                        if (lstSpans.Count > 0)
                                        {
                                            Desc = convert.ToString(lstSpans[0].InnerText).Trim();
                                            if (lstSpans.Count > 1)
                                            {
                                                Remarks = convert.ToString(lstSpans[1].InnerText).Trim();
                                            }
                                        }
                                        item.Add(Desc.Trim());
                                    }
                                    else
                                    {
                                        throw new Exception("Item Descr not found in PO. Check Item Header mapping.");
                                    }

                                    if (indxQty >= 0)
                                    {
                                        string qtyUom = convert.ToString(_rowData[indxQty].InnerText).Trim();
                                        while (qtyUom.Contains("  ")) qtyUom = qtyUom.Replace("  ", " ");
                                        string[] _val = qtyUom.Trim().Split(' ');
                                        double Qty = Convert.ToDouble(_val[0]);
                                        string unit = "";
                                        if (_val.Length > 1) unit = convert.ToString(_val[1]);

                                        item.Add(Qty.ToString());
                                        item.Add(unit.Trim());
                                    }
                                    else
                                    {
                                        throw new Exception("Item Qty & Unit not found in PO. Check Item Header mapping.");
                                    }

                                    // Remarks //
                                    item.Add(Remarks.Trim());

                                    if (indxPrice >= 0)
                                    {
                                        if (_rowData[indxPrice].InnerText.Trim() != "")//30-4-18
                                        {
                                            double UnitPrice = Convert.ToDouble(_rowData[indxPrice].InnerText);
                                            item.Add(UnitPrice.ToString());
                                        }
                                        else item.Add("0.00");
                                    }
                                    else
                                    {
                                        throw new Exception("Item Price not found in PO. Check Item Header mapping.");
                                    }

                                    if (indxDisc >= 0)
                                    {
                                        if (_rowData[indxDisc].InnerText.Trim() != "")//30-4-18
                                        {
                                            double Discount = Convert.ToDouble(_rowData[indxDisc].InnerText);
                                            item.Add(Discount.ToString());
                                        }
                                        else item.Add("0.00");
                                    }
                                    else
                                    {
                                        throw new Exception("Item Disc not found in PO. Check Item Header mapping.");
                                    }

                                    if (indxTotal >= 0)
                                    {
                                        if (_rowData[indxTotal].InnerText.Trim() != "")//30-4-18
                                        {
                                            double TotalPrice = Convert.ToDouble(_rowData[indxTotal].InnerText);
                                            item.Add(TotalPrice.ToString());
                                        }
                                        else item.Add("0.00");
                                    }
                                    else
                                    {
                                        throw new Exception("Item Total not found in PO. Check Item Header mapping.");
                                    }

                                    if (indxPartNo >= 0)
                                    {
                                        string partNo = convert.ToString(_rowData[indxPartNo].InnerText);
                                        item.Add(partNo.Trim());
                                    }
                                    else
                                    {
                                        try
                                        {
                                            string RefNo = "";
                                            if (Remarks.Trim().Length > 0)
                                            {
                                                string _tempStr = Remarks;
                                                if (Remarks.Trim().ToLower().Contains("supplier spare part no."))
                                                    _tempStr = _tempStr.Replace("Supplier Spare Part No.:", "|");
                                                if (Remarks.Trim().ToLower().Contains("reference no."))
                                                    _tempStr = _tempStr.Replace("Reference No.:", "|");

                                                if (_tempStr.Trim().Contains("|"))
                                                {
                                                    _tempStr = _tempStr.Split('|')[1].Trim();
                                                    RefNo = _tempStr;
                                                    foreach (string _content in _skipLines)
                                                    {
                                                        if (RefNo.Trim().ToLower().Contains(_content.Trim().ToLower()))
                                                        {
                                                            if(RefNo.ToLower().IndexOf(_content.ToLower())!=-1)
                                                            RefNo = RefNo.Substring(0, RefNo.ToLower().IndexOf(_content.ToLower())).Trim(); }
                                                            //RefNo = RefNo.Substring(0, RefNo.IndexOf(_content)).Trim();//on 1-3-2018
                                                    }
                                                }

                                                if (RefNo.Contains(";")) RefNo = RefNo.Substring(0, RefNo.IndexOf(';')).Trim();
                                                if (RefNo.Contains(",")) RefNo = RefNo.Substring(0, RefNo.IndexOf(',')).Trim();
                                            }

                                            item.Add(RefNo.Trim()); // Ref  No
                                        }
                                        catch (Exception ex) { throw; }
                                    }
                                    lstItems.Add(item);

                                    #endregion
                                }
                            }
                        }
                    }
                            #endregion
                }
                return lstItems;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void ExporttoHeader(Dictionary<string, string> _xmlHeader, ref LeSXML.LeSXML _lesXML)
        {
            try
            {
                this.VRNO = _xmlHeader["VRNO"].Trim();
                _lesXML.Active = "1";
                _lesXML.Doc_Type = this.DocType;
                _lesXML.BuyerRef = _xmlHeader["VRNO"].Trim();
                _lesXML.Vessel = _xmlHeader["VESSEL"].Trim();
                _lesXML.IMONO = _xmlHeader["IMO"].Replace("IMO No.", "").Replace(":", "").Trim();
                if (_xmlHeader["SHIP_CITY"].Trim().Length > 0)
                {
                    _lesXML.PortName = convert.ToString(_xmlHeader["SHIP_CITY"]).Trim();
                }

                string MsgNumber = _xmlHeader["VRNO"].Trim() + "|" + this.Userid + "|" + this.Password;
                _lesXML.DocLinkID = MsgNumber; // MessageNumber in eSupplier
                _lesXML.DocReferenceID = MsgNumber;// MessageReferenceNumber in eSupplier

                _lesXML.Recipient_Code = SupplierCode;

                //added on 18-4-2019 (for sinwa-euronav change task id=1610 task added by roy)
                if (dctAppSettings.ContainsKey("IS_COMPANYCHECK") && dctAppSettings.ContainsKey("COMPANYWISE_BUYERCODE"))
                {
                    if (dctAppSettings["IS_COMPANYCHECK"].ToUpper().Trim() == "TRUE")
                    {
                        string[] ArrSettings = dctAppSettings["COMPANYWISE_BUYERCODE"].Trim().Split(',');
                        string _value = _httpWrapper.GetElement("MainPage1_QuotationDetailsView1_CompanyControl1_LabelName").InnerText;
                        foreach (string arr in ArrSettings)
                        {
                            if (_value.ToUpper().Trim().Contains(arr.ToUpper().Trim().Split('|')[0]))
                            {
                                BuyerCode = _lesXML.Sender_Code = arr.ToUpper().Trim().Split('|')[1];
                                break;
                            }
                        }
                        if (_lesXML.Sender_Code == "")
                        {
                            throw new Exception("for company " + _value + ", No Buyer code found in file");
                        }
                    }
                    else _lesXML.Sender_Code = BuyerCode;
                }
                else _lesXML.Sender_Code = BuyerCode;
                ////

                _lesXML.FileName = this.DocType + "_" + this.Domain + "_" + convert.ToFileName(this.VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xml";
                _lesXML.OrigDocFile = ImgFile;

                if (_xmlHeader["CURRENCY"].Trim() != "" && _xmlHeader["CURRENCY"].Trim() != "&nbsp;")
                    _lesXML.Currency = _xmlHeader["CURRENCY"].Trim();
                else _lesXML.Currency = "";
                _lesXML.Remark_Title = _xmlHeader["SUBJECT"].Trim();

                string BuyerRemarks = _xmlHeader["COMMENTS_PUR1"].Trim();
                if (this.DocType == "PO")
                {
                    BuyerRemarks += Environment.NewLine + "Technical Ref :" + _xmlHeader["TECH_REF"].Trim();
                    BuyerRemarks += Environment.NewLine + "Terms Of Delivery :" + _xmlHeader["TERMS_OF_DEL"].Trim();
                }
                BuyerRemarks += Environment.NewLine + _xmlHeader["COMMENTS_PUR2"].Trim();
                string BlanksLines = Environment.NewLine + Environment.NewLine;
                while (BuyerRemarks.Contains(BlanksLines)) BuyerRemarks = BuyerRemarks.Replace(BlanksLines, Environment.NewLine);

                if (this.DocType == "RFQ")
                {
                    string _value = _httpWrapper.GetElement("MainPage1_QuotationDetailsView1_QuotationLinesEditor1_txtAdviceDate").GetAttributeValue("Value", "");
                    if (_value != "") BuyerRemarks = BuyerRemarks + "Reply Before: " + _value;
                }
                       _lesXML.Remark_Sender = BuyerRemarks.Trim();
                if (this.DocType == "PO")
                {
                    _lesXML.Remark_PaymentTerms = _xmlHeader["COMMENTS_ZTP"].Trim();
                    _lesXML.Reference_Document = _xmlHeader["QUOTE_REF"].Trim();
                }

                #region /* address */
                // Buyer Address
                _lesXML.Addresses.Add(new LeSXML.Address());
                _lesXML.Addresses[0].Qualifier = "BY";
                _lesXML.Addresses[0].AddressName = _xmlHeader["BUYER_NAME"];
                _lesXML.Addresses[0].Address1 = _xmlHeader["BUYER_ADDRESS1"].Trim() + "," + _xmlHeader["BUYER_ADDRESS2"].Trim();
                _lesXML.Addresses[0].Address2 = _xmlHeader["BUYER_ADDRESS3"].Trim() + "," + _xmlHeader["BUYER_ADDRESS4"].Trim();
                _lesXML.Addresses[0].Country = _xmlHeader["BUYER_COUNTRY"].Trim();
                _lesXML.Addresses[0].Phone = _xmlHeader["BUYER_PHONE"].Replace("Phone", "").Replace(":", "").Trim();
                _lesXML.Addresses[0].eMail = _xmlHeader["BUYER_EMAIL"].Replace("E-mail", "").Replace("Email", "").Replace(":", "").Trim();

                string strData = _xmlHeader["BUYER_CONTACT"].Trim().Replace(Environment.NewLine, "|").Replace("\n", "|");
                string[] _values = strData.Split('|');
                _lesXML.Addresses[0].ContactPerson = _values[0].Trim();
                if (_values.Length > 2) _lesXML.Addresses[0].Phone += "," + _values[1].Replace("Phone", "").Replace(":", "").Trim();
                if (_values[_values.Length - 1].Contains("@"))
                {
                    string _email = _values[_values.Length - 1].Replace("E-mail", "").Replace("Email", "").Replace(":", "").Trim();
                    if (!_lesXML.Addresses[0].eMail.Contains(_email))
                        _lesXML.Addresses[0].eMail += "," + _email;
                }
                _lesXML.Addresses[0].eMail = _lesXML.Addresses[0].eMail.Trim(',');

                // Vendor Address
                _lesXML.Addresses.Add(new LeSXML.Address());
                _lesXML.Addresses[1].Qualifier = "VN";
                _lesXML.Addresses[1].AddressName = _xmlHeader["VENDOR_NAME"].Trim();
                _lesXML.Addresses[1].Address1 = _xmlHeader["VENDOR_ADDRESS1"].Trim();
                _lesXML.Addresses[1].PostCode = _xmlHeader["VENDOR_ZIP"].Trim();
                _lesXML.Addresses[1].City = _xmlHeader["VENDOR_CITY"].Trim();
                _lesXML.Addresses[1].Country = _xmlHeader["VENDOR_COUNTRY"].Trim();
                _lesXML.Addresses[1].Phone = _xmlHeader["VENDOR_PHONE"].Replace("Phone", "").Replace(":", "").Trim();
                _lesXML.Addresses[1].eMail = _xmlHeader["VENDOR_EMAIL"].Replace("E-mail", "").Replace("Email", "").Replace(":", "").Trim();

                // Shipping Address
                _lesXML.Addresses.Add(new LeSXML.Address());
                _lesXML.Addresses[2].Qualifier = "CN";
                _lesXML.Addresses[2].AddressName = _xmlHeader["SHIP_NAME"].Trim();
                _lesXML.Addresses[2].Address1 = _xmlHeader["SHIP_ADDRESS1"].Trim();
                _lesXML.Addresses[2].PostCode = _xmlHeader["SHIP_ZIP"].Trim();
                _lesXML.Addresses[2].City = _xmlHeader["SHIP_CITY"].Trim();
                _lesXML.Addresses[2].Country = _xmlHeader["SHIP_COUNTRY"].Trim();
                _lesXML.Addresses[2].eMail = _xmlHeader["SHIP_EMAIL"].Replace("E-mail", "").Replace("Email", "").Replace(":", "").Trim();
                _lesXML.Addresses[2].Phone = _xmlHeader["SHIP_PHONE"].Replace("Phone", "").Replace(":", "").Trim();
                #endregion

                // Dates
                _lesXML.Date_Delivery = GetDate(_xmlHeader["LATE_DATE"].Trim()); /* LATE DATE */
                if (this.DocType == "PO") _lesXML.Date_Document = GetDate(_xmlHeader["ORDER_DATE"].Trim()); /* Order Date */
                else if (this.DocType == "RFQ") _lesXML.Date_Document = GetDate(_xmlHeader["RFQ_DATE"].Trim());  /* RFQ Date */

                /* ADDED ON 14-3-18 mail from wss for port not received in file (new change in site)*/
                _lesXML.PortCode = _xmlHeader["PORTCODE"];
                _lesXML.PortName = _xmlHeader["PORTNAME"];

                // Amount //
                if (this.DocType == "PO")
                {
                    string ItemTotal = "", AddDiscount = "", NetTotal = "";//30-4-2018
                    if (_xmlHeader["SUB_TOTAL"].Trim() != "" && _xmlHeader["SUB_TOTAL"].Trim()!="&nbsp;")
                        ItemTotal = _xmlHeader["SUB_TOTAL"].Trim();
                    else ItemTotal = "0.00";

                    if (_xmlHeader["ALLOWANCE"].Trim() != "" && _xmlHeader["ALLOWANCE"].Trim() != "&nbsp;")
                        AddDiscount = _xmlHeader["ALLOWANCE"].Trim();
                    else AddDiscount = "0";

                    if (_xmlHeader["NET_TOTAL"].Trim() != "" && _xmlHeader["NET_TOTAL"].Trim() != "&nbsp;")
                        NetTotal = _xmlHeader["NET_TOTAL"].Trim();
                    else NetTotal = "0.00";

                    double itemTotal = Convert.ToDouble(ItemTotal);
                    double discount = Convert.ToDouble(AddDiscount);

                    double PackingCost = 0;
                    if (_xmlHeader.ContainsKey("PACKING_COST")) PackingCost = Convert.ToDouble(_xmlHeader["PACKING_COST"]);

                    double Freight = 0;
                    if (_xmlHeader.ContainsKey("FREIGHT_AMT")) Freight = Convert.ToDouble(_xmlHeader["FREIGHT_AMT"]);

                    _lesXML.Total_Other = PackingCost.ToString();
                    _lesXML.Total_Freight = Freight.ToString();
                    _lesXML.Total_Additional_Discount = discount.ToString();
                    _lesXML.Total_LineItems_Net = itemTotal.ToString();
                    _lesXML.Total_Net_Final = Convert.ToDouble(NetTotal).ToString();
                }
                CurrenctXMLFile = _lesXML.FilePath + "\\" + _lesXML.FileName;
            }
            catch (Exception ex)
            {
                LogText = " : Error in ExporttoHeader - " + ex.Message;
                throw;
            }
        }

        public void ExporttoItems(List<List<string>> _xmlItems, ref LeSXML.LeSXML _lesXML)
        {
            foreach (List<string> _lItem in _xmlItems)
            {
                LeSXML.LineItem _item = new LeSXML.LineItem();
                _item.SystemRef = (_lesXML.LineItems.Count + 1).ToString();
                _item.Number = _lItem[0];
                _item.Name = _lItem[1];
                _item.Quantity = _lItem[2];
                _item.Unit = _lItem[3];
                _item.Remark = _lItem[4];
                _item.ListPrice = _lItem[5];
                _item.Discount = _lItem[6];
                _item.OriginatingSystemRef = _lItem[0];
                _item.ItemRef = _lItem[8].Trim(); // UPDDATED ON 29-MAY-2015
                _lesXML.LineItems.Add(_item);
            }
            _lesXML.Total_LineItems = _lesXML.LineItems.Count.ToString();
        }

        public List<string> DownloadAttachments()
        {
            List<string> lst = new List<string>();
            try
            {
                HtmlNodeCollection _nodes = null;
                if (this.DocType == "RFQ")
                    _nodes = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='MainPage1_QuotationDetailsView1_documents2List_DataListDocuments']//tr[@class='quotationLineBackground1' or @class='quotationLineBackground2']");
                else if (this.DocType == "PO")
                    _nodes = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='MainPage1_PurchaseOrderDetailsView1_documents2List_DataListDocuments']//tr[@class='quotationLineBackground1' or @class='quotationLineBackground2']");
                if (_nodes != null)
                {
                    if (_nodes.Count > 0)
                    {
                        foreach (HtmlNode _row in _nodes)
                        {
                            HtmlNodeCollection _rowData = _row.ChildNodes;
                            string _ahref = _rowData[7].SelectSingleNode(".//a").GetAttributeValue("id", "").Replace("_", "%24");
                            dctPostDataValues.Clear();
                            dctPostDataValues.Add("MainPage1%24QuotationDetailsView1%24QuotationLinesEditor1%24DatePickerDeliveryDate%24RangeValidator1_ValidatorCalloutExtender_ClientState", "");
                            dctPostDataValues.Add("MainPage1%24QuotationDetailsView1%24QuotationLinesEditor1%24DatePickerDeliveryDate%24ValidatorCalloutExtender1_ClientState", "");
                            dctPostDataValues.Add("MainPage1%24QuotationDetailsView1%24QuotationLinesEditor1%24DatePickerValidUntil%24RangeValidator1_ValidatorCalloutExtender_ClientState", "");
                            dctPostDataValues.Add("MainPage1%24QuotationDetailsView1%24QuotationLinesEditor1%24DatePickerValidUntil%24ValidatorCalloutExtender1_ClientState", "");
                            foreach (KeyValuePair<string, string> _pAttach in dctPostAttachment)
                            {
                                dctPostDataValues.Add(_pAttach.Key, _pAttach.Value);
                            }
                            dctPostDataValues.Add("MainPage1%24QuotationDetailsView1%24QuotationLinesEditor1%24ValidatorCalloutExtenderTotalDiscount_ClientState", "");
                            dctPostDataValues.Add("__EVENTTARGET", _ahref);
                            dctPostDataValues.Add("__VIEWSTATEENCRYPTED", "");
                            ReadHiddenData();


                            this.URL = URL.Replace("Pages/Login.aspx", "Default.aspx");
                            // string _postdata = GetPostData();
                            // if (_httpWrapper._DownloadDocument(this.URL, _postdata, ImgPath + "\\" + Convert.ToString(_rowData[5].InnerText).Trim(), "Application/unknown"))
                            // {
                            if (DownloadRFQ(this.URL, ImgPath + "\\" + Convert.ToString(_rowData[5].InnerText).Trim(), "Application/unknown"))
                                lst.Add(Convert.ToString(_rowData[5].InnerText).Trim());
                            // }
                        }

                        //  //Redirect to detail page
                        //this.URL = URL.Replace("Pages/Login.aspx", "Default.aspx");
                        //LoadURL("table", "id", "MainPage1_QuotationListView1_DataList1");
                    }
                }
                return lst;
            }
            catch (Exception)
            {
                return lst;
            }
        }

        public void GetAttachments(ref LeSXML.LeSXML _lesXML)
        {
            try
            {
                #region /* for getting proper screenshot*/
                _httpWrapper._CurrentResponseString = _httpWrapper._CurrentResponseString.Replace("Images/Logo.gif", "Images/" + Domain + "/Logo.gif");
                string[] splitResponse = _httpWrapper._CurrentResponseString.Split(new string[] { "|0|hiddenField|__EVENTTARGET||0|hiddenField|" }, StringSplitOptions.None);
                string Responsestring = startText + splitResponse[0] + endText;
                Responsestring = Responsestring.Replace("overflow:auto;visibility:hidden;height:90%;", "overflow:auto;/* visibility:hidden; */height:90%;");
                #endregion

                ImgFile = "Sertica_" + this.Domain + "_" + VRNO + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                _CurrentResponseString = Responsestring;
                if (PrintScreen(ImgPath + "\\" + ImgFile))
                {
                    LogText = "File " + Path.GetFileName(ImgFile) + " saved successfully.";
                    _lesXML.OrigDocFile = ImgFile;
                    string _attachemtns = _lesXML.OrigDocFile;
                    List<string> lstAttachments = DownloadAttachments();
                    foreach (string strAttach in lstAttachments)
                    {
                        if (_attachemtns.Trim().Length > 0) _attachemtns += "|" + Path.GetFileName(strAttach);
                        else _attachemtns += Path.GetFileName(strAttach);
                    }
                    _lesXML.OrigDocFile = _attachemtns.Trim();

               

                }

             
            }
            catch (Exception)
            { }
        }

        public void GetDataForScreenshot()
        {
            try
            {
                if (startText == "" && endText == "")
                {
                    string[] splitText = _httpWrapper._CurrentDocument.DocumentNode.InnerHtml.Split(new string[] { " <div id=\"MainPage1_UpdatePanel1\">" }, StringSplitOptions.None);
                    startText = splitText[0] + " <div id=\"MainPage1_UpdatePanel1\">";
                    string[] ssplitText = splitText[1].Split(new string[] { "<input type=\"hidden\" name=\"_IG_CSS_LINKS_\" id=\"_IG_CSS_LINKS_\"" }, StringSplitOptions.None);
                    endText = ssplitText[1].Split(new string[] { "|ig_res/Default/ig_shared.css\">" }, StringSplitOptions.None)[1];
                }
            }
            catch (Exception)
            { }
        }

        public void ReadHiddenData()
        {
            List<string> lstStr = new List<string>(new string[] { "__VIEWSTATEGENERATOR", "__EVENTVALIDATION" });
            string[] arr = _httpWrapper._CurrentDocument.DocumentNode.InnerText.Split(new string[] { "|hiddenField" }, StringSplitOptions.None);
            foreach (string str in arr)
            {
                if (str.StartsWith("|"))
                {
                    string[] strArr = str.Split('|');
                    if (lstStr.IndexOf(strArr[1]) > -1)
                    {
                        dctPostDataValues.Add(strArr[1], Uri.EscapeDataString(strArr[2]));
                    }
                    if (str.Contains("__VIEWSTATE") && !str.Contains("__VIEWSTATEGENERATOR") && !str.Contains("__VIEWSTATEENCRYPTED"))
                        dctPostDataValues.Add(strArr[1], Uri.EscapeDataString(strArr[2]));
                }
            }
        }

        public bool SiteLogout()
        {
            bool result = false;
            try
            {

                //if (_httpWrapper.LoadURL("http://connect.bergebulk.com:8081/SerticaConnect/Pages/Login.aspx", "input", "id", "ButtonLogin", "ASP.NET_SessionId"))//,false
                if (_httpWrapper.LoadURL(dctAppSettings["SITE_URL"], "input", "id", "ButtonLogin", "ASP.NET_SessionId"))//,false
                {
                    result = true;
                    startText = ""; endText = "";
                }
                return result;
            }
            catch (Exception ex)//added on 13-3-2018
            {
                LogText = "Exception " + ex.Message + " while logging out for domain : " + Domain;
                return false;
            }
        }

        private string GetDate(string DateValue)
        {
            if (DateValue.Trim().Length > 0)
            {
                string _dtValue = "";
                DateTime dt = DateTime.MinValue;
                DateTime.TryParseExact(DateValue.Trim(), "M/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out dt);
                if (dt == DateTime.MinValue) DateTime.TryParseExact(DateValue, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out dt);
                if (dt == DateTime.MinValue) DateTime.TryParseExact(DateValue, "M/d/yyyy", null, System.Globalization.DateTimeStyles.None, out dt);
                if (dt != DateTime.MinValue) _dtValue = dt.ToString("yyyyMMdd");
                return _dtValue.Trim();
            }
            else return "";
        }

        public List<string> GetProcessedItems(eActions eAction)
        {
            List<string> slProcessedItems = new List<string>();
            switch (eAction)
            {
                case eActions.RFQ: sDoneFile = AppDomain.CurrentDomain.BaseDirectory +"GUID_Files\\"+ this.Domain + "_" + this.SupplierCode + "_GUID.txt"; ; break;
                case eActions.PO: sDoneFile = AppDomain.CurrentDomain.BaseDirectory + "GUID_Files\\" + this.Domain + "_" + this.SupplierCode + "_GUID.txt"; ; break;
                default: break;
            }
            if (File.Exists(sDoneFile))
            {
                string[] _Items = File.ReadAllLines(sDoneFile);
                slProcessedItems.AddRange(_Items.ToList());
            }
            return slProcessedItems;
        }

        private void GetSkipText()
        {
            try
            {
                string _data = "";
                using (StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "MAP_FILES/SKIP_TEXT.txt"))
                {
                    _data = sr.ReadToEnd();
                    sr.Dispose();
                }
                _data = _data.Trim();
                string[] strArr = _data.Trim().Split(Environment.NewLine.ToCharArray());
                foreach (string s in strArr)
                {
                    if (s.Trim().Length > 0 && !_skipLines.Contains(s.Trim())) _skipLines.Add(s.Trim());
                }
            }
            catch (Exception ex)
            { }
        }

        private string[] SplitNumericValue(string cStr)
        {
            string cNumbers = "0123456789.";
            string[] slStr = cStr.Split(' ');
            string[] slReturn = { "", "" };

            foreach (string s1 in slStr)
            {
                for (int i = 0; i < s1.Trim().Length; i++)
                {
                    if (cNumbers.IndexOf(s1[i]) != -1)
                        slReturn[0] += s1[i];
                    else
                        slReturn[1] += s1[i];
                }
            }

            return slReturn;
        }

        public void WriteErrorLog(string AuditMsg, string ErrorNo)
        {
            LogText = AuditMsg;
            CreateAuditFile("", "Sertica_HTTP_Downloader", VRNO, "Error", ErrorNo + AuditMsg, BuyerCode, SupplierCode, AuditPath);
        }

        public void WriteErrorLog_With_Screenshot(string AuditMsg, string ErrorNo)
        {
            LogText = AuditMsg;
            string eFile = ImgPath + "\\Sertica_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
            if (!PrintScreen(eFile)) eFile = "";
            CreateAuditFile(eFile, "Sertica_HTTP_Downloader", VRNO, "Error", ErrorNo + AuditMsg, BuyerCode, SupplierCode, AuditPath);
        }

        //public void WriteErrorLog(string AuditMsg)
        //{
        //    LogText = AuditMsg;
        //    CreateAuditFile("", "Sertica_HTTP_Downloader", VRNO, "Error", AuditMsg, BuyerCode, SupplierCode, AuditPath);
        //}

        //public void WriteErrorLog_With_Screenshot(string AuditMsg)
        //{
        //    LogText = AuditMsg;
        //    string eFile = ImgPath + "\\Sertica_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
        //    if (!PrintScreen(eFile)) eFile = "";
        //    CreateAuditFile(eFile, "Sertica_HTTP_Downloader", VRNO, "Error", AuditMsg, BuyerCode, SupplierCode, AuditPath);
        //}

        public override bool DownloadRFQ(string RequestURL, string DownloadFileName, string ContentType = "Application/pdf")
        {
            bool _result = false;
            try
            {
                URL = RequestURL;
                if (PostURL("", "", "",false))//,false
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

        public override bool PrintScreen(string sFileName)
        {
            if (!Directory.Exists(Path.GetDirectoryName(sFileName) + "\\Temp")) Directory.CreateDirectory(Path.GetDirectoryName(sFileName) + "\\Temp");
            sFileName = Path.GetDirectoryName(sFileName) + "\\Temp\\" + Path.GetFileName(sFileName);
            if (base.PrintScreen(sFileName))
            {
                MoveFiles(sFileName, ImgPath + "\\" + Path.GetFileName(sFileName));
                return (File.Exists(ImgPath + "\\" + Path.GetFileName(sFileName)));
            }
            else return false;
        }
    }
}
