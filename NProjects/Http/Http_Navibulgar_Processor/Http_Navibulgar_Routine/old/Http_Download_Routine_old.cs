using HtmlAgilityPack;
using MTML.GENERATOR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Http_Navibulgar_Routine
{
    public class Http_Download_Routine_old : LeSCommon.LeSCommon
    {
        public int iRetry = 0, IsAltItemAllowed = 0, IsPriceAveraged = 0, IsUOMChanged = 0;
        public string sAuditMesage = "", ImgPath = "", DocType = "", VRNO = "", sDoneFile = "", Vessel = "", Port = "", BuyerName = "", SupplierName = "",
            MTML_QuotePath = "", MessageNumber = "", LeadDays = "", Currency = "", MsgNumber = "", MsgRefNumber = "", UCRefNo = "", AAGRefNo = ""
            , LesRecordID = "", BuyerPhone = "", BuyerEmail = "", BuyerFax = "", supplierName = "", supplierPhone = "", supplierEmail = "", supplierFax = "",
             VesselName = "", PortName = "", PortCode = "", SupplierComment = "", PayTerms = "", PackingCost = "", FreightCharge = "", GrandTotal = "", Allowance = ""
             , TotalLineItemsAmount = "", BuyerTotal = "", DtDelvDate = "", dtExpDate = "", ToDate = "", FromDate = "",
             MailTo = "",MailBcc="",MailCC="",NotificationPath="";
        DateTime dt = new DateTime(); DateTime dt1 = new DateTime(); string _dateTo = "", _dateFrom = "";
        bool IsDecline = false, IsSaveQuote = false, IsSubmitQuote = false, SendMail = false;
        public string[] Actions;
        List<string> xmlFiles = new List<string>();
        public LineItemCollection _lineitem = null;
        public MTMLInterchange _interchange { get; set; }
        Dictionary<string, string> dctSubmitValues = new Dictionary<string, string>();

        public void LoadAppsettings()
        {
            try
            {
                iRetry = 0;
                URL = dctAppSettings["SITE_URL"].Trim();
                Userid = dctAppSettings["USERNAME"].Trim();
                Password = dctAppSettings["PASSWORD"].Trim();
                Domain = dctAppSettings["DOMAIN"].Trim();
                BuyerCode = dctAppSettings["BUYERCODE"].Trim();
                SupplierCode = dctAppSettings["SUPPLIERCODE"].Trim();
                BuyerName = dctAppSettings["BUYERNAME"].Trim();
                SupplierName = dctAppSettings["SUPPLIERNAME"].Trim();
                Actions = dctAppSettings["ACTIONS"].Trim().Split(',');
                ImgPath = dctAppSettings["IMAGE_PATH"].Trim();
                AuditPath = dctAppSettings["AUDITPATH"].Trim();
                MTML_QuotePath = dctAppSettings["MTML_QUOTEPATH"].Trim();
                LoginRetry = convert.ToInt(dctAppSettings["LOGINRETRY"].Trim());
                FromDate = dctAppSettings["INQUIRY_FROM_DATE"];
                ToDate = dctAppSettings["INQUIRY_TO_DATE"].Trim();
                if (dctAppSettings.ContainsKey("SAVE_QUOTE"))
                {
                    if (dctAppSettings["SAVE_QUOTE"].Trim().ToUpper() == "YES") IsSaveQuote = true;
                    else IsSaveQuote = false;
                }
                if (dctAppSettings.ContainsKey("SUBMIT_QUOTE"))
                {
                    if (dctAppSettings["SUBMIT_QUOTE"].Trim().ToUpper() == "YES") IsSubmitQuote = true;
                    else IsSubmitQuote = false;
                }
                if (AuditPath == "") AuditPath = AppDomain.CurrentDomain.BaseDirectory + "AuditLog";
                if (ImgPath == "") ImgPath = AppDomain.CurrentDomain.BaseDirectory + "Attachments";
                if (MTML_QuotePath == "") MTML_QuotePath = AppDomain.CurrentDomain.BaseDirectory + "MTML_Quote";
                if (!Directory.Exists(ImgPath)) Directory.CreateDirectory(ImgPath);
                if (!Directory.Exists(MTML_QuotePath + "\\Backup")) Directory.CreateDirectory(MTML_QuotePath + "\\Backup");
                if (!Directory.Exists(MTML_QuotePath + "\\Error")) Directory.CreateDirectory(MTML_QuotePath + "\\Error");

                // Added By Sanjita //
                if (dctAppSettings.ContainsKey("SEND_MAIL_NOTIFICATION"))
                {
                    this.SendMail = (dctAppSettings["SEND_MAIL_NOTIFICATION"] == "YES" ? true : false);
                }
                if (dctAppSettings.ContainsKey("MAIL_TO"))
                {
                    this.MailTo = convert.ToString(dctAppSettings["MAIL_TO"]);
                }
                     if (dctAppSettings.ContainsKey("MAIL_BCC"))
                {
                    this.MailBcc = convert.ToString(dctAppSettings["MAIL_BCC"]);
                }
                     if (dctAppSettings.ContainsKey("MAIL_CC"))
                {
                    this.MailCC = convert.ToString(dctAppSettings["MAIL_CC"]);
                }
                if (dctAppSettings.ContainsKey("MAIL_NOTIFICATION_PATH")){
                    NotificationPath=convert.ToString(dctAppSettings["MAIL_NOTIFICATION_PATH"]).Trim();                    
                }
            }
            catch (Exception e)
            {
                sAuditMesage = "Exception in LoadAppsettings: " + e.GetBaseException().ToString();
                LogText = sAuditMesage;
              // CreateAuditFile("", "Navibulgar_HTTP_Processor", "", "Error", sAuditMesage, BuyerCode, SupplierCode, AuditPath);
            }
        }

        public override bool DoLogin(string validateNodeType, string validateAttribute, string attributeValue, bool bload = true)
        {
            bool isLoggedin = false;
            try
            {
                LoadURL("input", "id", "MainContent_LoginUser_LoginButton");
                if (_httpWrapper._dctStateData.Count > 0)
                {
                    dctPostDataValues.Clear();
                    dctPostDataValues.Add("__EVENTTARGET", _httpWrapper._dctStateData["__EVENTTARGET"]);
                    dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                    dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                    dctPostDataValues.Add("__EVENTVALIDATION", _httpWrapper._dctStateData["__EVENTVALIDATION"]);
                    dctPostDataValues.Add("ctl00%24MainContent%24LoginUser%24UserName", HttpUtility.UrlEncode(Userid));
                    dctPostDataValues.Add("ctl00%24MainContent%24LoginUser%24Password", HttpUtility.UrlEncode(Password));
                    dctPostDataValues.Add("ctl00%24MainContent%24LoginUser%24LoginButton", "Log+In");
                    isLoggedin = base.DoLogin(validateNodeType, validateAttribute, attributeValue, false);

                    if (isLoggedin)
                    {
                        LogText = "Logged in successfully";
                    }
                    else
                    {
                        if (iRetry == LoginRetry)
                        {
                            string filename = ImgPath + "\\Navibulgar_LoginFailed_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + Domain + ".png";
                            if (!PrintScreen(filename)) filename = "";
                            LogText = "Login failed";
                            CreateAuditFile(filename, "Navibulgar_HTTP_Processor", "", "Error", "Unable to login.", BuyerCode, SupplierCode, AuditPath);
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
                    string filename = ImgPath + "\\Navibulgar_LoginFailed_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + Domain + ".png";
                    if (!PrintScreen(filename)) filename = "";
                    LogText = "Unable to load URL" + URL;
                    CreateAuditFile(filename, "Navibulgar_HTTP_Processor", "", "Error", "Unable to load URL" + URL, BuyerCode, SupplierCode, AuditPath);
                }
            }
            catch (Exception e)
            {
                LogText = "Exception while login : " + e.GetBaseException().ToString();
                if (iRetry > LoginRetry)
                {
                    string filename = ImgPath + "\\Navibulgar_LoginFailed_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + Domain + ".png";
                    if (!PrintScreen(filename)) filename = "";
                    LogText = "Login failed";
                    CreateAuditFile(filename, "Navibulgar_HTTP_Processor", "", "Error", "Unable to login. Error : " + e.Message, BuyerCode, SupplierCode, AuditPath);
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

        #region ## RFQ ##
        public void ProcessRFQ()
        {
            try
            {
                this.DocType = "RFQ";
                LogText = "RFQ processing started.";
                List<string> _lstNewRFQs = GetNewRFQs();
                if (_lstNewRFQs.Count > 0)
                {
                    DownloadRFQ(_lstNewRFQs);
                }
                else
                {
                    LogText = "No new RFQ found.";
                }
                LogText = "RFQ processing stopped.";
            }
            catch (Exception e)
            {
                WriteErrorLog_With_Screenshot("Exception in Process RFQ : " + e.GetBaseException().ToString());
            }
        }

        public List<string> GetNewRFQs()
        {
            List<string> _lstNewRFQs = new List<string>();
            List<string> slProcessedItem = GetProcessedItems(eActions.RFQ);
            _lstNewRFQs.Clear();
            _httpWrapper._CurrentDocument.LoadHtml(_httpWrapper._CurrentResponseString);

            #region for filter inqiry table by dates
            DateTime dtTo = new DateTime();
            DateTime dtFrom = new DateTime();
            if (ToDate == "" && FromDate == "")
            {
                dtTo = DateTime.Now;
                dtFrom = DateTime.Now.AddDays(-1);
            }
            else
            {
                dtTo = DateTime.MinValue;
                DateTime.TryParseExact(ToDate, "d/M/yyyy", null, DateTimeStyles.None, out dtTo);

                dtFrom = DateTime.MinValue;
                DateTime.TryParseExact(FromDate, "d/M/yyyy", null, DateTimeStyles.None, out dtFrom);
            }

            if (dtTo != DateTime.MinValue)
            {
                dt = DateTime.ParseExact(dtTo.ToShortDateString(), "M/d/yyyy", CultureInfo.InvariantCulture);
                _dateTo = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            if (dtFrom != DateTime.MinValue)
            {
                dt1 = DateTime.ParseExact(dtFrom.ToShortDateString(), "M/d/yyyy", CultureInfo.InvariantCulture);
                _dateFrom = dt1.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            if (_dateTo != "" && _dateFrom != "")
            {
                if (_httpWrapper._dctStateData.Count > 0)
                {
                    dctPostDataValues.Clear();
                    dctPostDataValues.Add("__EVENTTARGET", "ctl00%24MainContent%24txtFrom");
                    dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
                    dctPostDataValues.Add("__LASTFOCUS", "");
                    dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                    dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                    dctPostDataValues.Add("__VIEWSTATEENCRYPTED", "");
                    dctPostDataValues.Add("__EVENTVALIDATION", _httpWrapper._dctStateData["__EVENTVALIDATION"]);
                    dctPostDataValues.Add("ctl00%24MainContent%24txtFrom", Uri.EscapeDataString(_dateFrom));
                    dctPostDataValues.Add("ctl00%24MainContent%24txtTo", Uri.EscapeDataString(_dateTo));
                    dctPostDataValues.Add("ctl00%24MainContent%24Button1", "Refresh");

                    _httpWrapper._AddRequestHeaders.Add("Origin", @"https://webquote.navbul.com");
                    _httpWrapper.Referrer = "";
                }
            #endregion

                URL = "https://webquote.navbul.com/suppliers/OutReq.aspx";
                if (PostURL("table", "id", "MainContent_GridView1"))
                {
                    HtmlNodeCollection _nodes = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='MainContent_GridView1']//tr[@onmouseout]");
                    if (_nodes != null)
                    {
                        if (_nodes.Count > 0)
                        {
                            foreach (HtmlNode _row in _nodes)
                            {
                                HtmlNodeCollection _rowData = _row.ChildNodes;
                                string VRNo = _rowData[1].InnerText.Trim();
                                string Vessel = _rowData[3].InnerText.Trim();
                                string Port = _rowData[4].InnerText.Trim();
                                string _url = _row.GetAttributeValue("onclick", "").Trim();
                                if (_url.Contains(';'))
                                {
                                    string[] _arrUrl = _url.Split(';');
                                    _url = _arrUrl[1].Replace("&#39", "");
                                }

                                string _guid = VRNo + "|" + Vessel + "|" + Port;
                                if (!_lstNewRFQs.Contains(VRNo + "|" + Vessel + "|" + Port + "|" + _url) && !slProcessedItem.Contains(_guid))
                                {
                                    _lstNewRFQs.Add(VRNo + "|" + Vessel + "|" + Port + "|" + _url);
                                }
                            }
                        }
                        else
                            LogText = "No new RFQs found.";
                    }
                }
            }
            else
            {
                LogText = "To Date or/and From Date formats are wrong.";
                CreateAuditFile("", "Navibulgar_HTTP_Processor", "", "Error", "To Date or/and From Date formats are wrong.", BuyerCode, SupplierCode, AuditPath);

            }
            return _lstNewRFQs;
        }

        public List<string> GetProcessedItems(eActions eAction)
        {
            List<string> slProcessedItems = new List<string>();
            switch (eAction)
            {
                case eActions.RFQ: sDoneFile = AppDomain.CurrentDomain.BaseDirectory + this.Domain + "_" + this.SupplierCode + "_GUID.txt"; ; break;
                default: break;
            }
            if (File.Exists(sDoneFile))
            {
                string[] _Items = File.ReadAllLines(sDoneFile);
                slProcessedItems.AddRange(_Items.ToList());
            }
            return slProcessedItems;
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
                    this.Port = lst[2];
                    URL = lst[3];

                    URL = "https://webquote.navbul.com/suppliers/" + URL;
                    LogText = "Processing RFQ for ref no " + this.VRNO;

                    LoadURL("input", "id", "MainContent_btnUpdate");

                    string eFile = this.ImgPath + "\\" + this.VRNO.Replace("/", "_") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
                    if (!PrintScreen(eFile)) eFile = "";
                    LeSXML.LeSXML _lesXml = new LeSXML.LeSXML();
                    if (GetRFQHeader(ref _lesXml, eFile))
                    {
                        if (GetRFQItems(ref _lesXml))
                        {
                            if (GetAddress(ref _lesXml))
                            {
                                string xmlfile = "RFQ_" + this.VRNO.Replace("/", "_") + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xml";
                                if (_lesXml.Total_LineItems.Length > 0)
                                {
                                    _lesXml.FileName = xmlfile;
                                    _lesXml.WriteXML();
                                    if (File.Exists(_lesXml.FilePath + "\\" + _lesXml.FileName))
                                    {
                                        LogText = xmlfile + " downloaded successfully.";
                                        LogText = "";
                                        CreateAuditFile(xmlfile, "Navibulgar_HTTP_Processor", VRNO, "Downloaded", xmlfile + " downloaded successfully.", BuyerCode, SupplierCode, AuditPath);
                                        if ((this.VRNO + "|" + this.Vessel + "|" + this.Port).Length > 0)
                                            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + this.Domain + "_" + this.SupplierCode + "_GUID.txt", this.VRNO + "|" + this.Vessel + "|" + this.Port + Environment.NewLine);
                                    }
                                    else
                                    {
                                        LogText = "Unable to download file " + xmlfile;
                                        string filename = PrintScreenPath + "\\Navibulgar_RFQError_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                                        CreateAuditFile(filename, "Navibulgar_HTTP_Processor", VRNO, "Error", "Unable to download file " + xmlfile + " for ref " + VRNO + ".", BuyerCode, SupplierCode, AuditPath);
                                        if (PrintScreen(filename)) filename = "";
                                    }
                                }
                            }
                            else
                            {
                                LogText = "Unable to get address details";
                                CreateAuditFile(eFile, "Navibulgar_HTTP_Processor", VRNO, "Error", "Unable to get address details", BuyerCode, SupplierCode, AuditPath);
                            }
                        }
                        else
                        {
                            LogText = "Unable to get RFQ item details";
                            CreateAuditFile(eFile, "Navibulgar_HTTP_Processor", VRNO, "Error", "Unable to get RFQ item details", BuyerCode, SupplierCode, AuditPath);
                        }
                    }
                    else
                    {
                        LogText = "Unable to get RFQ header details";
                        CreateAuditFile(eFile, "Navibulgar_HTTP_Processor", VRNO, "Error", "Unable to get RFQ header details", BuyerCode, SupplierCode, AuditPath);
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorLog_With_Screenshot("Unable to load RFQ '" + VRNO + "' details due to " + ex.GetBaseException().Message.ToString());
                }
            }
        }

        public bool GetRFQHeader(ref LeSXML.LeSXML _lesXml, string eFile)
        {
            bool isResult = false;
            LogText = "Start Getting Header details";
            try
            {
                _lesXml.DocID = DateTime.Now.ToString("yyyyMMddhhmmss");
                _lesXml.Created_Date = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Doc_Type = "RFQ";
                _lesXml.Dialect = "Navigation Maritime Bulgare";
                _lesXml.Version = "1";
                _lesXml.Date_Document = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Date_Preparation = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Sender_Code = BuyerCode;
                _lesXml.Recipient_Code = SupplierCode;

                HtmlNode _InqNo = _httpWrapper.GetElement("span", "id", "MainContent_IDControl");
                if (_InqNo != null)
                    _lesXml.DocReferenceID = _InqNo.InnerText.Trim();

                _lesXml.DocLinkID = URL;

                if (File.Exists(eFile))
                    _lesXml.OrigDocFile = Path.GetFileName(eFile);

                _lesXml.Active = "1";

                HtmlNode _Vessel = _httpWrapper.GetElement("span", "id", "MainContent_IDControl0");
                if (_Vessel != null)
                    _lesXml.Vessel = _Vessel.InnerText.Trim();

                _lesXml.BuyerRef = _InqNo.InnerText.Trim();

                HtmlNode _portname = _httpWrapper.GetElement("span", "id", "MainContent_n_PortN");
                if (_portname != null)
                    _lesXml.PortName = _portname.InnerText.Trim();

                _lesXml.Currency = "";

                if (URL.Contains("Edit_Spares.aspx"))
                {
                    HtmlNode _equipment = _httpWrapper.GetElement("span", "id", "MainContent_lblEq");
                    if (_equipment != null)
                        _lesXml.Equipment = _equipment.InnerText.Trim();

                    HtmlNode _equipSys = _httpWrapper.GetElement("span", "id", "MainContent_lblSys");
                    if (_equipSys != null)
                        _lesXml.EquipRemarks = "System: " + _equipSys.InnerText.Trim();

                    HtmlNode _equipNo = _httpWrapper.GetElement("span", "id", "MainContent_lblSNom");
                    if (_equipNo != null)
                        _lesXml.EquipRemarks += "Ser. No.: " + _equipNo.InnerText.Trim();
                }

                HtmlNode _remarks = _httpWrapper.GetElement("textarea", "id", "MainContent_txtAddInf");
                if (_remarks != null)
                    _lesXml.Remark_Header = _remarks.InnerText.Trim();

                HtmlNode _etaDate = _httpWrapper.GetElement("span", "id", "MainContent_Vess_ETA");
                if (_etaDate != null)
                {
                    string strEtaDate = _etaDate.InnerText.Trim();
                    if (strEtaDate != "" && strEtaDate != "-")
                    {
                        DateTime dt = DateTime.MinValue;
                        DateTime.TryParseExact(strEtaDate, "d.M.yyyy", null, System.Globalization.DateTimeStyles.None, out dt);
                        _lesXml.Date_ETA = dt.ToString("yyyyMMdd");
                    }
                }

                _lesXml.Total_LineItems_Discount = "0";
                _lesXml.Total_LineItems_Net = "0";
                _lesXml.Total_Additional_Discount = "0";
                _lesXml.Total_Freight = "0";
                _lesXml.Total_Other = "0";
                _lesXml.Total_Net_Final = "0";
                LogText = "Getting Header details completed successfully.";
                isResult = true;
                return isResult;
            }
            catch (Exception ex)
            {
                LogText = "Unable to get header details." + ex.GetBaseException().ToString(); isResult = false;
                return isResult;
            }
        }

        public bool GetRFQItems(ref LeSXML.LeSXML _lesXml)
        {
            bool isResult = false;
            string EquipRemarks = "";
            try
            {
                _lesXml.LineItems.Clear();
                LogText = "Start Getting LineItem details";
                if (URL.Contains("Edit_Spares.aspx"))
                {
                    HtmlNodeCollection _nodes = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@class='style2E']//tr");
                    if (_nodes != null)
                    {
                        if (_nodes.Count >= 2)
                        {
                            int i = 0;
                            foreach (HtmlNode _row in _nodes)
                            {

                                LeSXML.LineItem _item = new LeSXML.LineItem();
                                try
                                {
                                    HtmlNodeCollection _rowData = _row.ChildNodes;
                                    if (!_rowData[1].InnerText.Trim().Contains("Part No."))
                                    {
                                        // i += 1;
                                        if (_rowData[1].InnerText.Trim().Contains("Subsystem"))
                                        {
                                            EquipRemarks = _rowData[1].InnerText.Trim();
                                        }
                                        if (_rowData.Count == 21)
                                        {
                                            i += 1;
                                            _item.Number = Convert.ToString(i);
                                            _item.OrigItemNumber = Convert.ToString(i);
                                            string _inpQty = _rowData[11].SelectSingleNode("./input").GetAttributeValue("Id", "").Trim();
                                            _item.OriginatingSystemRef = _inpQty;
                                            //   _item.OriginatingSystemRef = _rowData[1].InnerText.Trim();
                                            // _item.Description = _rowData[5].InnerText.Trim();
                                            _item.Name = _rowData[5].InnerText.Trim();
                                            _item.Unit = _rowData[9].InnerText.Trim();
                                            _item.Quantity = _rowData[7].SelectSingleNode("./input").GetAttributeValue("value", "").Trim();
                                            _item.Discount = "0";
                                            _item.ListPrice = "0";
                                            _item.LeadDays = "0";
                                            //  _item.Remark = "Draw No.: " + _rowData[3].InnerText.Trim();
                                            _item.SystemRef = Convert.ToString(i);
                                            _item.ItemRef = _rowData[1].InnerText.Trim().Replace("TAG NO:", "");
                                            if (EquipRemarks != "")
                                            { _item.EquipRemarks = EquipRemarks; EquipRemarks = ""; }
                                            _lesXml.LineItems.Add(_item);
                                            if (_rowData[3].InnerText.Trim() != "")
                                            {
                                                _item.Remark = "Draw No: " + _rowData[3].InnerText.Trim();
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                { LogText = ex.GetBaseException().ToString(); }
                            }
                            _lesXml.Total_LineItems = Convert.ToString(_lesXml.LineItems.Count);
                            isResult = true;
                        }
                        else isResult = false;
                    }
                    else isResult = false;
                }
                else if (URL.Contains("Edit_Req.aspx"))
                {
                    HtmlNodeCollection _nodes = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='MainContent_GridView1']//tr");
                    if (_nodes != null)
                    {
                        if (_nodes.Count >= 2)
                        {
                            int i = 0;
                            foreach (HtmlNode _row in _nodes)
                            {

                                LeSXML.LineItem _item = new LeSXML.LineItem();
                                try
                                {
                                    HtmlNodeCollection _rowData = _row.ChildNodes;
                                    if (!_rowData[1].InnerText.Trim().Contains("Item Code"))
                                    {
                                        i += 1;
                                        _item.Number = Convert.ToString(i);
                                        _item.OrigItemNumber = Convert.ToString(i);
                                        string _inpQty = _rowData[5].SelectSingleNode("./input").GetAttributeValue("Id", "").Trim();
                                        _item.OriginatingSystemRef = _inpQty;
                                        // _item.Description = _rowData[2].InnerText.Trim();
                                        _item.Unit = _rowData[3].InnerText.Trim();
                                        _item.Quantity = _rowData[4].InnerText.Trim();
                                        _item.Name = _rowData[2].InnerText.Trim();
                                        _item.Discount = "0";
                                        _item.ListPrice = "0";
                                        _item.LeadDays = "0";
                                        _item.SystemRef = Convert.ToString(i);
                                        _item.ItemRef = _rowData[1].InnerText.Trim().Replace("&nbsp;", "");
                                        _lesXml.LineItems.Add(_item);
                                    }
                                }
                                catch (Exception ex)
                                { LogText = ex.GetBaseException().ToString(); }
                            }
                            _lesXml.Total_LineItems = Convert.ToString(_lesXml.LineItems.Count);
                            isResult = true;
                        }
                        else isResult = false;
                    }
                    else isResult = false;
                }
                else
                {
                    WriteErrorLog_With_Screenshot("Invalid url");
                }
                LogText = "Getting LineItem details successfully";
                return isResult;
            }
            catch (Exception ex)
            {
                LogText = "Exception while getting RFQ Items: " + ex.GetBaseException().ToString(); isResult = false; return isResult;
            }
        }

        public bool GetAddress(ref LeSXML.LeSXML _lesXml)
        {
            bool isResult = false;
            try
            {
                _lesXml.Addresses.Clear();
                LogText = "Start Getting address details";
                LeSXML.Address _xmlAdd = new LeSXML.Address();

                _xmlAdd.Qualifier = "BY";
                _xmlAdd.AddressName = BuyerName;
                _lesXml.Addresses.Add(_xmlAdd);

                _xmlAdd = new LeSXML.Address();
                _xmlAdd.Qualifier = "VN";
                _xmlAdd.AddressName = SupplierName;
                _lesXml.Addresses.Add(_xmlAdd);

                LogText = "Getting address details successfully";
                isResult = true;
                return isResult;
            }
            catch (Exception ex)
            {
                LogText = "Exception while getting address details: " + ex.GetBaseException().ToString(); isResult = false;
                return isResult;
            }
        }

        #endregion

        #region ## Quote ##

        public void ProcessQuote()
        {
            int j = 0;
            try
            {
                LogText = "";
                LogText = "Quote processing started.";
                GetXmlFiles();
                if (xmlFiles.Count > 0)
                {
                    LogText = xmlFiles.Count + " Quote files found to process.";
                    for (j = 0; j < xmlFiles.Count; j++)
                    {
                        dctSubmitValues.Clear();
                        ProcessQuoteMTML(xmlFiles[j]);
                    }
                }
                else LogText = "No quote file found.";
                LogText = "Quote processing stopped.";
            }
            catch (Exception e)
            {
                LogText = e.StackTrace;
                WriteErrorLogQuote_With_Screenshot("Exception in Process Quote : " + e.GetBaseException().ToString(), xmlFiles[j]);
            }
        }

        public void ProcessQuoteMTML(string MTML_QuoteFile)
        {
            string eFile = "";
            try
            {
                MTMLClass _mtml = new MTMLClass();
                _interchange = _mtml.Load(MTML_QuoteFile);
                LoadInterchangeDetails();
                if (UCRefNo != "")
                {
                    LogText = "Quote processing started for refno: " + UCRefNo;
                    URL = MessageNumber;
                    if (LoadURL("input", "id", "MainContent_btnUpdate"))
                    {                        
                        HtmlNode _hRefNo = _httpWrapper.GetElement("span", "id", "MainContent_IDControl");
                        if (_hRefNo != null)
                        {
                            if (convert.ToString(_hRefNo.InnerText).Trim() == UCRefNo)
                            {
                                LogText = "Reference Number Matched";
                                HtmlNode _btnSave = _httpWrapper.GetElement("input", "id", "MainContent_btnSave");
                                if (_btnSave != null)
                                {
                                    LogText = "Checking Quote Status";
                                    HtmlNode _lblSubmit = _httpWrapper.GetElement("span", "id", "MainContent_lblSubmit");
                                    if (_lblSubmit == null) // (This Quotation is Submitted!) Label on Edit Page
                                    {
                                        LogText = "Quote is in progress state";

                                        #region  Commented
                                        //if (!_lblSubmit.InnerText.Trim().Contains("This Quotation is Submitted!"))
                                        //{
                                        //}
                                        //else
                                        //{
                                        //    eFile = "Navigation_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                                        //    PrintScreen(ImgPath + "\\" + eFile);
                                        //    MoveToError("Quotation is already submitted for refno " + UCRefNo, UCRefNo, MTML_QuoteFile, eFile);
                                        //}
                                        #endregion

                                        double total = 0;
                                        int result = FillQuotation(ref total);

                                        if (result == 1)
                                        {
                                            eFile = "Navigation_Save_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                                            PrintScreen(ImgPath + "\\" + eFile);

                                            string QuoteRef = "";
                                            foreach (Reference Ref in _interchange.DocumentHeader.References)
                                            {
                                                if (Ref.Qualifier == ReferenceQualifier.AAG)
                                                {
                                                    QuoteRef = convert.ToString(Ref.ReferenceNumber).Trim();
                                                    break;
                                                }
                                            }

                                            if (SubmitQuotation(QuoteRef)) // Updated By Sanjita
                                            {
                                                eFile = "Navigation_Submit_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                                                PrintScreen(ImgPath + "\\" + eFile);

                                                MoveToBakup(MTML_QuoteFile, "Quote '" + UCRefNo + "' submitted successfully.", eFile);

                                                // Send Mail (Added By Sanjita 01-DEC-18) //
                                                SendMailNotification(_interchange, "QUOTE", UCRefNo, "SUBMITTED", "Quote '" + UCRefNo + "' Submitted Successfully.");
                                            }
                                            else
                                            {
                                                // Added By Sanjita on 29-NOV-18 //
                                                eFile = "Navigation_Error_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                                                PrintScreen(ImgPath + "\\" + eFile);
                                                MoveToError("Unable to submit quote for '" + UCRefNo + "'", UCRefNo, MTML_QuoteFile, eFile);
                                            }
                                        }
                                        else if (result == 0)
                                        {
                                            eFile = "Navigation_SaveDiff_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                                            PrintScreen(ImgPath + "\\" + eFile);
                                            MoveToError("Total mismatched as site total " + total + " and mtml total " + GrandTotal, UCRefNo, MTML_QuoteFile, eFile);
                                        }
                                        else if (result == 2)
                                        {
                                            eFile = "Navigation_SaveError_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                                            PrintScreen(ImgPath + "\\" + eFile);
                                            MoveToError("Unable to save quotation." + GrandTotal, UCRefNo, MTML_QuoteFile, eFile);
                                        }
                                    }
                                    else
                                    {
                                        if (convert.ToString(_lblSubmit.InnerText).Trim().Contains("This Quotation is Submitted!"))
                                        {
                                            eFile = "Navigation_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                                            PrintScreen(ImgPath + "\\" + eFile);
                                            MoveToError("Quotation is already submitted for Ref No '" + UCRefNo + "'.", UCRefNo, MTML_QuoteFile, eFile);
                                        }
                                        else
                                        {
                                            eFile = "Navigation_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                                            PrintScreen(ImgPath + "\\" + eFile);
                                            MoveToError("Submit button is disabled for quote '" + UCRefNo + "'", UCRefNo, MTML_QuoteFile, eFile);
                                        }
                                    }
                                }
                                else
                                {
                                    eFile = "Navigation_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                                    PrintScreen(ImgPath + "\\" + eFile);
                                    MoveToError("Unable to get save button for refno " + UCRefNo, UCRefNo, MTML_QuoteFile, eFile);
                                }
                            }
                            else
                            {
                                eFile = "Navigation_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                                PrintScreen(ImgPath + "\\" + eFile);
                                MoveToError("Ref No. mismatch betwwen site ref no " + _hRefNo.InnerText.Trim() + " and quote file ref no " + UCRefNo + ".", UCRefNo, MTML_QuoteFile, eFile);
                            }
                        }
                    }
                    else
                    {
                        eFile = "Navigation_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                        if (PrintScreen(ImgPath + "\\" + eFile)) eFile = "";
                        MoveToError("Unable to load details page.", UCRefNo, MTML_QuoteFile, eFile);
                    }
                }
            }
            catch (Exception ex)
            {
                LogText = "Error - " + ex.Message;
                LogText = ex.StackTrace;
                WriteErrorLogQuote_With_Screenshot("Exception while processing Quote '" + UCRefNo + "', Error - " + ex.Message, Path.GetFileName(MTML_QuoteFile));
            }
        }

        public bool SubmitQuotation(string QuoteRef)
        {
            bool result = false;
            try
            {
                if (IsSubmitQuote)
                {
                    dctPostDataValues.Clear();
                    dctPostDataValues = dctSubmitValues;

                    if (_httpWrapper._dctStateData.ContainsKey("__EVENTTARGET"))
                        dctPostDataValues.Add("__EVENTTARGET", _httpWrapper._dctStateData["__EVENTTARGET"]);

                    if (_httpWrapper._dctStateData.ContainsKey("__EVENTARGUMENT"))
                        dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);

                    if (_httpWrapper._dctStateData.ContainsKey("__LASTFOCUS"))
                        dctPostDataValues.Add("__LASTFOCUS", _httpWrapper._dctStateData["__LASTFOCUS"]);

                    if (_httpWrapper._dctStateData.ContainsKey("__VIEWSTATE"))
                        dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);

                    if (_httpWrapper._dctStateData.ContainsKey("__VIEWSTATEGENERATOR"))
                        dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);

                    if (_httpWrapper._dctStateData.ContainsKey("__VIEWSTATEENCRYPTED"))
                        dctPostDataValues.Add("__VIEWSTATEENCRYPTED", _httpWrapper._dctStateData["__VIEWSTATEENCRYPTED"]);

                    if (_httpWrapper._dctStateData.ContainsKey("__EVENTVALIDATION"))
                        dctPostDataValues.Add("__EVENTVALIDATION", _httpWrapper._dctStateData["__EVENTVALIDATION"]);

                    dctPostDataValues.Add("ctl00%24MainContent%24txtTotRef", Uri.EscapeDataString(AAGRefNo));
                    dctPostDataValues.Add("ctl00%24MainContent%24btnUpdate", "Submit+Quotation");
                    HtmlNode _hHelp = _httpWrapper.GetElement("input", "id", "MainContent_txtHelp");
                    if (_hHelp != null)
                    {
                        dctPostDataValues.Add("ctl00%24MainContent%24txtHelp", _hHelp.GetAttributeValue("value", "").Trim());
                    }
                    else throw new Exception("Help textbox not found");

                    //string[] ArrURL = URL.Split('=');
                    //URL = ArrURL[0] + "=" + HttpUtility.UrlDecode(ArrURL[1]);

                    URL = MessageNumber;
                    if (PostURL("input", "id", "MainContent_txtFrom"))
                    {
                        URL = MessageNumber.Replace("Edit", "View");
                        _httpWrapper.ContentType = "";

                        if (LoadURL("input", "id", "MainContent_txtTotRef"))
                        {
                            #region  // Commented By Sanjita on 29-NOV-18 //
                            //string _chkReadonly = _httpWrapper.GetElement("input", "id", "MainContent_txtTotRef").GetAttributeValue("readonly", "");
                            //if (_chkReadonly == "readonly")
                            //{
                            //    result = true;
                            //}                            
                            #endregion

                            string updatedQuoteRef = _httpWrapper.GetElement("input", "id", "MainContent_txtTotRef").GetAttributeValue("value", "");
                            HtmlNode quoteStatus = _httpWrapper.GetElement("span", "id", "MainContent_lblStatus");
                            if (quoteStatus != null)
                            {
                                string strQuoteStatus = convert.ToString(quoteStatus.InnerText);
                                if (updatedQuoteRef == QuoteRef.Trim() && strQuoteStatus.Trim().ToUpper() == "OPEN")
                                {
                                    result = true;
                                }
                            }
                        }
                    }
                }
                else result = true;
            }
            catch (Exception ex)
            {
                LogText = "Exception while submitting quote; " + ex.Message;
                LogText = ex.StackTrace;
                //LogText = "Exception while submitting quote: " + ex.GetBaseException().Message.ToString(); result = false;
                throw ex;
            }
            return result;
        }

        public int FillQuotation(ref double total)
        {
            //   bool result = false;
            int result = 0;
            try
            {
                // _httpWrapper.IsUrlEncoded = false;
                FillHeaderDetails();
                FillItemDetails();

                dctPostDataValues.Add("ctl00%24MainContent%24btnSave", "Save+%26+Calculate");
                dctPostDataValues.Add("ctl00%24MainContent%24txtHelp", GrandTotal);

                // _httpWrapper.Referrer = URL;
                string[] ArrURL = URL.Split('=');
                URL = ArrURL[0] + "=" + HttpUtility.UrlEncode(ArrURL[1]);

                if (PostURL("span", "id", "MainContent_lblGrandTot"))
                {
                    HtmlNode _total = _httpWrapper.GetElement("span", "id", "MainContent_lblGrandTot");
                    if (_total != null)
                    {
                        total = convert.ToFloat(_total.InnerText.Trim());
                        //double diff = convert.ToFloat(GrandTotal) - convert.ToFloat(_total.InnerText.Trim());
                        //if (diff >= -2 && diff <= 2)
                        //    result = 1;
                        //else
                        //{
                        //    result = 0;
                        //}

                        if (Convert.ToInt32(Convert.ToDouble(total)) == Convert.ToInt32(Convert.ToDouble(GrandTotal))) //added by kalpita on 09/09/2019 to check buyer total
                        { 
                            double diff = convert.ToFloat(GrandTotal) - convert.ToFloat(_total.InnerText.Trim());
                            if (diff >= -2 && diff <= 2)
                                result = 1;
                            else
                            {
                                result = 0;
                            }
                        }
                        else if (BuyerTotal != "")//16-12-2017
                        {
                            if (Convert.ToInt32(Convert.ToDouble(total)) == Convert.ToInt32(Convert.ToDouble(BuyerTotal)))
                            {
                                result = 1;
                            }
                            else
                            {
                                int _diff = 0;
                                if (Convert.ToDouble(total) > Convert.ToDouble(BuyerTotal))
                                    _diff = Convert.ToInt32(Convert.ToDouble(total)) - Convert.ToInt32(Convert.ToDouble(BuyerTotal));
                                else if (Convert.ToDouble(BuyerTotal) > Convert.ToDouble(total))
                                    _diff = Convert.ToInt32(Convert.ToDouble(BuyerTotal)) - Convert.ToInt32(Convert.ToDouble(total));
                                if (_diff <= 1)
                                    result = 1;
                                else
                                    result = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogText = "Exception while filling quote: " + ex.GetBaseException().Message.ToString();
                result = 2;
            }
            return result;
        }

        public void FillHeaderDetails()
        {
            LogText = "Started filling Header Details";
            dctPostDataValues.Clear();

            dctPostDataValues.Add("__EVENTTARGET", _httpWrapper._dctStateData["__EVENTTARGET"]);
            dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
            dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
            dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
            dctPostDataValues.Add("__EVENTVALIDATION", _httpWrapper._dctStateData["__EVENTVALIDATION"]);
            if (_httpWrapper._dctStateData.ContainsKey("__LASTFOCUS"))
                dctPostDataValues.Add("__LASTFOCUS", _httpWrapper._dctStateData["__LASTFOCUS"]);
            if (_httpWrapper._dctStateData.ContainsKey("__VIEWSTATEENCRYPTED"))
                dctPostDataValues.Add("__VIEWSTATEENCRYPTED", _httpWrapper._dctStateData["__VIEWSTATEENCRYPTED"]);
            //supp ref no
            dctPostDataValues.Add("ctl00%24MainContent%24txtTotRef", Uri.EscapeDataString(AAGRefNo));//

            //currency
            string _value = "";
            HtmlNodeCollection _options = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='MainContent_cmbCurr']//option");
            if (_options != null)
            {
                if (_options.Count > 0)
                {
                    foreach (HtmlNode _opt in _options)
                    {
                        if (_opt.NextSibling.InnerText.Trim().Split('|')[0].ToUpper().Trim() == Currency.ToUpper())
                        {
                            _value = _opt.GetAttributeValue("value", "");
                            break;
                        }
                    }
                }
            }
            if (_value != "")
            {
                dctPostDataValues.Add("ctl00%24MainContent%24cmbCurr", _value);
                if (!dctSubmitValues.ContainsKey("ctl00%24MainContent%24cmbCurr"))
                    dctSubmitValues.Add("ctl00%24MainContent%24cmbCurr", _value);
                else
                    dctSubmitValues["ctl00%24MainContent%24cmbCurr"] = _value;
            }
            else
                throw new Exception("Unable to set currency for refno: " + UCRefNo);//

            //delivery charges
            if (FreightCharge != "")
            {
                dctPostDataValues.Add("ctl00%24MainContent%24txtDeliCharg", convert.ToFloat(FreightCharge).ToString("0.00"));
                if (!dctSubmitValues.ContainsKey("ctl00%24MainContent%24txtDeliCharg"))
                    dctSubmitValues.Add("ctl00%24MainContent%24txtDeliCharg", convert.ToFloat(FreightCharge).ToString("0.00"));
            }
            else
            {
                dctPostDataValues.Add("ctl00%24MainContent%24txtDeliCharg", "0.00");//
                if (!dctSubmitValues.ContainsKey("ctl00%24MainContent%24txtDeliCharg"))
                    dctSubmitValues.Add("ctl00%24MainContent%24txtDeliCharg", "0.00");

            }

            //All Discount
            if (Allowance != "")
            {
                dctPostDataValues.Add("ctl00%24MainContent%24txtAllDisc", convert.ToFloat(Allowance).ToString("0.00"));
                if (!dctSubmitValues.ContainsKey("ctl00%24MainContent%24txtAllDisc"))
                dctSubmitValues.Add("ctl00%24MainContent%24txtAllDisc", convert.ToFloat(Allowance).ToString("0.00"));
            }
            else
            {
                dctPostDataValues.Add("ctl00%24MainContent%24txtAllDisc", "0.00");//
                if (!dctSubmitValues.ContainsKey("ctl00%24MainContent%24txtAllDisc"))
                dctSubmitValues.Add("ctl00%24MainContent%24txtAllDisc", "0.00");
            }

            //delivery days
            if (LeadDays != "")
            {
                dctPostDataValues.Add("ctl00%24MainContent%24txtDaysDeli", LeadDays);
                if (!dctSubmitValues.ContainsKey("ctl00%24MainContent%24txtDaysDeli"))
                    dctSubmitValues.Add("ctl00%24MainContent%24txtDaysDeli", LeadDays);
                else
                    dctSubmitValues["ctl00%24MainContent%24txtDaysDeli"] = LeadDays;
            }
            else
            {
                dctPostDataValues.Add("ctl00%24MainContent%24txtDaysDeli", "0.00");//
                if (!dctSubmitValues.ContainsKey("ctl00%24MainContent%24txtDaysDeli"))
                    dctSubmitValues.Add("ctl00%24MainContent%24txtDaysDeli", "0.00");
            }

            //additional info
            HtmlNode _hAddInfo = _httpWrapper.GetElement("textarea", "id", "MainContent_txtAddInf");
            if (_hAddInfo != null)
            {
                string _AddInfo = _hAddInfo.InnerText.Trim();
                dctPostDataValues.Add("ctl00%24MainContent%24txtAddInf", _AddInfo);
                if (!dctSubmitValues.ContainsKey("ctl00%24MainContent%24txtAddInf"))
                    dctSubmitValues.Add("ctl00%24MainContent%24txtAddInf", _AddInfo);
                else
                    dctSubmitValues["ctl00%24MainContent%24txtAddInf"] = _AddInfo;
            }
            else
            {
                dctPostDataValues.Add("ctl00%24MainContent%24txtAddInf", "");//
                if (!dctSubmitValues.ContainsKey("ctl00%24MainContent%24txtAddInf"))
                dctSubmitValues.Add("ctl00%24MainContent%24txtAddInf", "");
            }

            //general info
            string gInfo = SupplierComment.Replace("\r\n", " ");
            gInfo = gInfo.Replace("\n", " ");
            gInfo = gInfo.Replace("&nbsp;", "");

            if (gInfo != "" && dtExpDate != "")
                gInfo = gInfo + ". Expiry Date: " + dtExpDate;
            else if (gInfo == "" && dtExpDate != "")
                gInfo = "Expiry Date: " + dtExpDate;


            if (gInfo != "")
            {
                if (gInfo.Length > 200) gInfo = gInfo.Substring(0, 200);
                {
                    dctPostDataValues.Add("ctl00%24MainContent%24txtGComm", Uri.EscapeDataString(gInfo).Replace("%20", "+"));
                    if (!dctSubmitValues.ContainsKey("ctl00%24MainContent%24txtGComm"))
                    dctSubmitValues.Add("ctl00%24MainContent%24txtGComm", Uri.EscapeDataString(gInfo).Replace("%20", "+"));
                }
            }
            else
            {
                dctPostDataValues.Add("ctl00%24MainContent%24txtGComm", "");//
                if (!dctSubmitValues.ContainsKey("ctl00%24MainContent%24txtGComm"))
                dctSubmitValues.Add("ctl00%24MainContent%24txtGComm", "");
            }

            if (URL.Contains("Edit_Spares.aspx"))
            {
                //hidden element
                dctPostDataValues.Add("ctl00%24MainContent%24hiddenElement", _httpWrapper._dctStateData["MainContent_hiddenElement"]); //
            }

            LogText = "Filling Header Details completed";
        }

        public void FillItemDetails()
        {
            LogText = "Started filling Item Details";
            if (URL.Contains("Edit_Spares.aspx"))
            {
                HtmlNodeCollection _nodes = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@class='style2E']//tr");
                if (_nodes != null)
                {
                    if (_nodes.Count >= 2)
                    {
                        int i = 1;
                        LineItem _item = null;
                        foreach (HtmlNode _tr in _nodes)
                        {
                            HtmlNodeCollection _td = _tr.ChildNodes;
                            if (!_td[1].InnerText.Trim().Contains("Part No.") && !_td[1].InnerText.Trim().Contains("Subsystem") && !_td[1].InnerText.Trim().Contains("Comment:") && _td.Count == 21)
                            {
                                foreach (LineItem item in _lineitem)
                                {

                                    if (_td[11].ChildNodes[1].GetAttributeValue("id", "").Trim() == item.OriginatingSystemRef)
                                    {
                                        _item = item;
                                        break;
                                    }
                                }

                                if (_item != null)
                                {
                                    string _price = "", _discount = "";
                                    foreach (PriceDetails _priceDetails in _item.PriceList)
                                    {
                                        if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.GRP) _price = _priceDetails.Value.ToString("0.00");
                                        else if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.DPR) _discount = _priceDetails.Value.ToString("0.00");
                                    }

                                    if (i <= 9)
                                    {
                                        dctPostDataValues.Add("ctl00%24MainContent%24Repeater1%24ctl0" + i + "%24App_Q", _item.Quantity.ToString("0.00"));
                                        dctPostDataValues.Add("ctl00%24MainContent%24Repeater1%24ctl0" + i + "%24Loc_Unit", _price);
                                        dctPostDataValues.Add("ctl00%24MainContent%24Repeater1%24ctl0" + i + "%24RDisc", _discount);
                                        dctPostDataValues.Add("ctl00%24MainContent%24Repeater1%24ctl0" + i + "%24Sum", _item.MonetaryAmount.ToString("0.00"));
                                        dctPostDataValues.Add("ctl00%24MainContent%24Repeater1%24ctl0" + i + "%24DropDownList2", "OEM"); // Updated By Sanjita for OEM
                                        if (_item.DeleiveryTime != null)
                                            dctPostDataValues.Add("ctl00%24MainContent%24Repeater1%24ctl0" + i + "%24RDaysD", _item.DeleiveryTime);
                                        else
                                            dctPostDataValues.Add("ctl00%24MainContent%24Repeater1%24ctl0" + i + "%24RDaysD", "0");

                                        // Submit dic //                                        
                                        dctSubmitValues.Add("ctl00%24MainContent%24Repeater1%24ctl0" + i + "%24App_Q", _item.Quantity.ToString("0.00"));
                                        dctSubmitValues.Add("ctl00%24MainContent%24Repeater1%24ctl0" + i + "%24Loc_Unit", _price);
                                        dctSubmitValues.Add("ctl00%24MainContent%24Repeater1%24ctl0" + i + "%24RDisc", _discount);
                                        dctSubmitValues.Add("ctl00%24MainContent%24Repeater1%24ctl0" + i + "%24Sum", _item.MonetaryAmount.ToString("0.00"));
                                        dctSubmitValues.Add("ctl00%24MainContent%24Repeater1%24ctl0" + i + "%24DropDownList2", "OEM"); // Updated By Sanjita for OEM
                                        if (_item.DeleiveryTime != null)
                                            dctSubmitValues.Add("ctl00%24MainContent%24Repeater1%24ctl0" + i + "%24RDaysD", _item.DeleiveryTime);
                                        else
                                            dctSubmitValues.Add("ctl00%24MainContent%24Repeater1%24ctl0" + i + "%24RDaysD", "0");
                                    }
                                    else
                                    {
                                        dctPostDataValues.Add("ctl00%24MainContent%24Repeater1%24ctl" + i + "%24App_Q", _item.Quantity.ToString("0.00"));
                                        dctPostDataValues.Add("ctl00%24MainContent%24Repeater1%24ctl" + i + "%24Loc_Unit", _price);
                                        dctPostDataValues.Add("ctl00%24MainContent%24Repeater1%24ctl" + i + "%24RDisc", _discount);
                                        dctPostDataValues.Add("ctl00%24MainContent%24Repeater1%24ctl" + i + "%24Sum", _item.MonetaryAmount.ToString("0.00"));
                                        dctPostDataValues.Add("ctl00%24MainContent%24Repeater1%24ctl" + i + "%24DropDownList2", "OEM"); // Updated By Sanjita for OEM
                                        if (_item.DeleiveryTime != null)
                                            dctPostDataValues.Add("ctl00%24MainContent%24Repeater1%24ctl" + i + "%24RDaysD", _item.DeleiveryTime);
                                        else
                                            dctPostDataValues.Add("ctl00%24MainContent%24Repeater1%24ctl" + i + "%24RDaysD", "0");

                                        // Submit Dic //
                                        dctSubmitValues.Add("ctl00%24MainContent%24Repeater1%24ctl" + i + "%24App_Q", _item.Quantity.ToString("0.00"));
                                        dctSubmitValues.Add("ctl00%24MainContent%24Repeater1%24ctl" + i + "%24Loc_Unit", _price);
                                        dctSubmitValues.Add("ctl00%24MainContent%24Repeater1%24ctl" + i + "%24RDisc", _discount);
                                        dctSubmitValues.Add("ctl00%24MainContent%24Repeater1%24ctl" + i + "%24Sum", _item.MonetaryAmount.ToString("0.00"));
                                        dctSubmitValues.Add("ctl00%24MainContent%24Repeater1%24ctl" + i + "%24DropDownList2", "OEM"); // Updated By Sanjita for OEM
                                        if (_item.DeleiveryTime != null)
                                            dctSubmitValues.Add("ctl00%24MainContent%24Repeater1%24ctl" + i + "%24RDaysD", _item.DeleiveryTime);
                                        else
                                            dctSubmitValues.Add("ctl00%24MainContent%24Repeater1%24ctl" + i + "%24RDaysD", "0");
                                    }
                                    string _comments = "";
                                    if (_item.LineItemComment.Value.Trim() != "")
                                    {
                                        _comments = _item.LineItemComment.Value.Trim().Replace("\r\n", " ");
                                        _comments = _item.LineItemComment.Value.Trim().Replace("\n", " ");
                                        _comments = _item.LineItemComment.Value.Trim().Replace("&nbsp;", " ");
                                    }
                                    if (i <= 9)
                                    {
                                        dctPostDataValues.Add("ctl00%24MainContent%24Repeater1%24ctl0" + i + "%24txtComm", Uri.EscapeDataString(_comments).Replace("%20", "+"));
                                        dctSubmitValues.Add("ctl00%24MainContent%24Repeater1%24ctl0" + i + "%24txtComm", Uri.EscapeDataString(_comments).Replace("%20", "+"));
                                    }
                                    else
                                    {
                                        dctPostDataValues.Add("ctl00%24MainContent%24Repeater1%24ctl" + i + "%24txtComm", Uri.EscapeDataString(_comments).Replace("%20", "+"));
                                        dctSubmitValues.Add("ctl00%24MainContent%24Repeater1%24ctl" + i + "%24txtComm", Uri.EscapeDataString(_comments).Replace("%20", "+"));
                                    }
                                    i++;
                                }
                                else throw new Exception("Item " + _td[1].InnerText.Trim() + " not found on website for ref no" + UCRefNo);
                            }
                        }
                        i = 1;
                    }
                }
            }
            else if (URL.Contains("Edit_Req.aspx"))
            {
                HtmlNodeCollection _nodes = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='MainContent_GridView1']//tr");
                if (_nodes != null)
                {
                    if (_nodes.Count >= 2)
                    {
                        int i = 2;
                        LineItem _item = null;
                        foreach (HtmlNode _tr in _nodes)
                        {
                            HtmlNodeCollection _td = _tr.ChildNodes;
                            if (!_td[1].InnerText.Trim().Contains("Item Code"))
                            {
                                foreach (LineItem item in _lineitem)
                                {
                                    if (_td[5].ChildNodes[1].GetAttributeValue("id", "").Trim() == item.OriginatingSystemRef)
                                    {
                                        // if (_td[1].InnerText.Trim() == item.Identification)
                                        // {
                                        // }

                                        _item = item;
                                        break;                                  
                                    }
                                }
                                if (_item != null)
                                {
                                    string _price = "", _discount = "";
                                    foreach (PriceDetails _priceDetails in _item.PriceList)
                                    {
                                        if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.GRP) _price = _priceDetails.Value.ToString("0.00");
                                        else if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.DPR) _discount = _priceDetails.Value.ToString("0.00");
                                    }

                                    if (i <= 9)
                                    {
                                        dctPostDataValues.Add("ctl00%24MainContent%24GridView1%24ctl0" + i + "%24TextBox1", _price);
                                        dctPostDataValues.Add("ctl00%24MainContent%24GridView1%24ctl0" + i + "%24TextBox2", _discount);
                                        dctPostDataValues.Add("ctl00%24MainContent%24GridView1%24ctl0" + i + "%24TextBox3", LeadDays);

                                        //submit dic
                                        if (!dctSubmitValues.ContainsKey("ctl00%24MainContent%24GridView1%24ctl0" + i + "%24TextBox1"))
                                        dctSubmitValues.Add("ctl00%24MainContent%24GridView1%24ctl0" + i + "%24TextBox1", _price);
                                        else
                                        dctSubmitValues["ctl00%24MainContent%24GridView1%24ctl0" + i + "%24TextBox1"]=_price;

                                        if (!dctSubmitValues.ContainsKey("ctl00%24MainContent%24GridView1%24ctl0" + i + "%24TextBox2"))
                                            dctSubmitValues.Add("ctl00%24MainContent%24GridView1%24ctl0" + i + "%24TextBox2", _discount);
                                        else
                                            dctSubmitValues["ctl00%24MainContent%24GridView1%24ctl0" + i + "%24TextBox2"] = _discount;

                                        if (!dctSubmitValues.ContainsKey("ctl00%24MainContent%24GridView1%24ctl0" + i + "%24TextBox3"))
                                            dctSubmitValues.Add("ctl00%24MainContent%24GridView1%24ctl0" + i + "%24TextBox3", LeadDays);
                                        else
                                            dctSubmitValues["ctl00%24MainContent%24GridView1%24ctl0" + i + "%24TextBox3"] = LeadDays;
                                    }
                                    else
                                    {
                                        dctPostDataValues.Add("ctl00%24MainContent%24GridView1%24ctl" + i + "%24TextBox1", _price);
                                        dctPostDataValues.Add("ctl00%24MainContent%24GridView1%24ctl" + i + "%24TextBox2", _discount);
                                        dctPostDataValues.Add("ctl00%24MainContent%24GridView1%24ctl" + i + "%24TextBox3", LeadDays);

                                        //submit dic
                                        if (!dctSubmitValues.ContainsKey("ctl00%24MainContent%24GridView1%24ctl" + i + "%24TextBox1"))
                                            dctSubmitValues.Add("ctl00%24MainContent%24GridView1%24ctl" + i + "%24TextBox1", _price);
                                        else
                                            dctSubmitValues["ctl00%24MainContent%24GridView1%24ctl" + i + "%24TextBox1"] = _price;

                                        if (!dctSubmitValues.ContainsKey("ctl00%24MainContent%24GridView1%24ctl" + i + "%24TextBox2"))
                                            dctSubmitValues.Add("ctl00%24MainContent%24GridView1%24ctl" + i + "%24TextBox2", _discount);
                                        else
                                            dctSubmitValues["ctl00%24MainContent%24GridView1%24ctl" + i + "%24TextBox2"] = _discount;

                                        if (!dctSubmitValues.ContainsKey("ctl00%24MainContent%24GridView1%24ctl" + i + "%24TextBox3"))
                                            dctSubmitValues.Add("ctl00%24MainContent%24GridView1%24ctl" + i + "%24TextBox3", LeadDays);
                                        else
                                            dctSubmitValues["ctl00%24MainContent%24GridView1%24ctl" + i + "%24TextBox3"] = LeadDays;
                                    }
                                    string _comments = "";
                                    if (_item.LineItemComment.Value.Trim() != "")
                                    {
                                        _comments = _item.LineItemComment.Value.Trim().Replace("\r\n", " ");
                                        _comments = _item.LineItemComment.Value.Trim().Replace("\n", " ");
                                        _comments = _item.LineItemComment.Value.Trim().Replace("&nbsp;", " ");
                                        //  _comments += "Quoted Qty: " + _item.Quantity + ", Quoted Unit: " + _item.MeasureUnitQualifier;
                                    }
                                    if (i <= 9)
                                    {
                                        dctPostDataValues.Add("ctl00%24MainContent%24GridView1%24ctl0" + i + "%24TextBox5", Uri.EscapeDataString(_comments).Replace("%20", "+"));
                                        if (!dctSubmitValues.ContainsKey("ctl00%24MainContent%24GridView1%24ctl0" + i + "%24TextBox5"))
                                            dctSubmitValues.Add("ctl00%24MainContent%24GridView1%24ctl0" + i + "%24TextBox5", Uri.EscapeDataString(_comments).Replace("%20", "+"));
                                        else dctSubmitValues["ctl00%24MainContent%24GridView1%24ctl0" + i + "%24TextBox5"] = Uri.EscapeDataString(_comments).Replace("%20", "+");
                                    }
                                    else
                                    {
                                        dctPostDataValues.Add("ctl00%24MainContent%24GridView1%24ctl" + i + "%24TextBox5", Uri.EscapeDataString(_comments).Replace("%20", "+"));
                                        if (!dctSubmitValues.ContainsKey("ctl00%24MainContent%24GridView1%24ctl" + i + "%24TextBox5"))
                                            dctSubmitValues.Add("ctl00%24MainContent%24GridView1%24ctl" + i + "%24TextBox5", Uri.EscapeDataString(_comments).Replace("%20", "+"));
                                        else
                                            dctSubmitValues["ctl00%24MainContent%24GridView1%24ctl" + i + "%24TextBox5"] = Uri.EscapeDataString(_comments).Replace("%20", "+");
                                    }

                                    i++;
                                }
                                else throw new Exception("Item " + _td[1].InnerText.Trim() + " not found on website for ref no" + UCRefNo);
                            }
                        }
                        i = 2;
                    }
                }
            }
            LogText = "Filling Item Details completed";
        }

        public void GetXmlFiles()
        {
            xmlFiles.Clear();
            DirectoryInfo _dir = new DirectoryInfo(MTML_QuotePath);
            FileInfo[] _Files = _dir.GetFiles();
            foreach (FileInfo _MtmlFile in _Files)
            {
                xmlFiles.Add(_MtmlFile.FullName);
            }
        }

        public void WriteErrorLogQuote_With_Screenshot(string AuditMsg, string _File)
        {
            LogText = AuditMsg;
            string eFile = PrintScreenPath + "\\Navigation_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            if (!PrintScreen(eFile)) eFile = _File;

            //CreateAuditFile(eFile, "Navibulgar_HTTP_Processor", VRNO, "Error", AuditMsg, BuyerCode, SupplierCode, AuditPath);
            string Server = convert.ToString(ConfigurationManager.AppSettings["SERVER"]);
            string Processor = convert.ToString(ConfigurationManager.AppSettings["PROCESSOR"]);
            try
            {
                string auditPath = AuditPath;
                if (!Directory.Exists(auditPath)) Directory.CreateDirectory(auditPath);

                string auditData = "";
                if (auditData.Trim().Length > 0) auditData += Environment.NewLine;
                auditData += "0|"; // Buyer ID
                auditData += "0|"; // Supplier ID            
                auditData += "Navibulgar_HTTP_Processor|";
                auditData += Path.GetFileName(_File) + "|";
                auditData += UCRefNo + "|";
                auditData += "Error" + "|";
                auditData += DateTime.Now.ToString("yy-MM-dd HH:mm") + " : " + AuditMsg + "|";
                auditData += "0|"; // Linkid
                auditData += Server + "|"; // Server 
                auditData += BuyerCode + "|"; // Server 
                auditData += SupplierCode + "|"; // Server 
                auditData += Processor; // Processor 

                if (auditData.Trim().Length > 0)
                {
                    File.WriteAllText(AuditPath + "\\Navibulgar_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt", auditData);
                    System.Threading.Thread.Sleep(5);
                }
            }
            catch (Exception ex) { }

            if (!Directory.Exists(MTML_QuotePath + "\\Error")) Directory.CreateDirectory(MTML_QuotePath + "\\Error");
            File.Move(MTML_QuotePath + "\\" + _File, MTML_QuotePath + "\\Error\\" + _File);
        }

        public void LoadInterchangeDetails()
        {
            try
            {
                LogText = "Started Loading interchange object.";
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
                                        dtExpDate = ExpDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);//15-3-18
                                        //  dtExpDate = ExpDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture);
                                    }
                                }
                            }
                        }
                    }

                    #endregion
                    LogText = "stopped Loading interchange object.";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception on LoadInterchangeDetails : " + ex.GetBaseException().ToString());
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

        private void MoveToError(string Msg, string VRNO, string FileToMove, string screenShot)
        {
            if (Msg.Trim() != "")
            {
                LogText = Msg;
                if (screenShot.Trim().Length > 0)
                    CreateAuditFile(Path.GetFileName(screenShot), "Navibulgar_HTTP_Processo", VRNO, "Error", Msg, BuyerCode, SupplierCode, AuditPath);
                else
                    CreateAuditFile(Path.GetFileName(FileToMove), "Navibulgar_HTTP_Processo", VRNO, "Error", Msg, BuyerCode, SupplierCode, AuditPath);
            }
            if (File.Exists(FileToMove))
            {
                string orgFile = Path.GetFileName(FileToMove);
                string orgDir = Path.GetDirectoryName(FileToMove);

                if (!Directory.Exists(orgDir + "\\Error")) Directory.CreateDirectory(orgDir + "\\Error");
                if (File.Exists(orgDir + "\\Error\\" + orgFile))
                {
                    File.Delete(orgDir + "\\Error\\" + orgFile);
                }
                File.Move(FileToMove, orgDir + "\\Error\\" + orgFile);
                LogText = "File " + Path.GetFileName(FileToMove) + " moved to error files.";
            }
        }

        private void MoveToBakup(string MTML_QuoteFile, string msg, string eFile)
        {
            LogText = msg;

            if (!Directory.Exists(Path.GetDirectoryName(MTML_QuoteFile) + "\\Backup")) Directory.CreateDirectory(Path.GetDirectoryName(MTML_QuoteFile) + "\\Backup");

            if (File.Exists(Path.GetDirectoryName(MTML_QuoteFile) + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile)))
            {
                File.Delete(Path.GetDirectoryName(MTML_QuoteFile) + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile));
            }
            File.Move(MTML_QuoteFile, Path.GetDirectoryName(MTML_QuoteFile) + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile));

            CreateAuditFile(Path.GetFileName(eFile), "Navibulgar_HTTP_Processor", UCRefNo, "Success", msg, BuyerCode, SupplierCode, AuditPath);
        }

        public override bool PrintScreen(string sFileName)
        {
            if (!Directory.Exists(Path.GetDirectoryName(sFileName) + "\\Temp")) Directory.CreateDirectory(Path.GetDirectoryName(sFileName) + "\\Temp");
            sFileName = Path.GetDirectoryName(sFileName) + "\\Temp\\" + Path.GetFileName(sFileName);
            if (base.PrintScreen(sFileName))
            {
                MoveFiles(sFileName, this.ImgPath + "\\" + Path.GetFileName(sFileName));
                return (File.Exists(this.ImgPath + "\\" + Path.GetFileName(sFileName)));
            }
            else return false;
        }

        public void WriteErrorLog_With_Screenshot(string AuditMsg,string ErrorNo)
        {
            LogText = AuditMsg;
            string eFile = ImgPath + "\\Navibulgar_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
            if (!PrintScreen(eFile)) eFile = "";
            CreateAuditFile(eFile, "Navibulgar_HTTP_Processor", VRNO, "Error",ErrorNo+ AuditMsg, BuyerCode, SupplierCode, AuditPath);
        }

        // Added By Sanjita on 01-DEC-18 //
        private void SendMailNotification(MTMLInterchange _interchange, string DocType, string VRNO, string ActionType, string Message)
        {
            try
            {
                int QuotationID = convert.ToInt(_interchange.BuyerSuppInfo.RecordID);
                string BuyerID = convert.ToString(_interchange.BuyerSuppInfo.BuyerID).Trim();
                string SupplierID = convert.ToString(_interchange.BuyerSuppInfo.SupplierID).Trim();

                string MailAuditPath = this.NotificationPath;
                if (MailAuditPath.Trim() != "")
                {
                    if (!Directory.Exists(MailAuditPath.Trim())) Directory.CreateDirectory(MailAuditPath.Trim());
                }
                else throw new Exception("MAIL_NOTIFICATION_PATH is not defined in AppSetting.xml file.");

                string Vessel = "", SenderName = "", RecipientName = "", ByrMailID = "", SuppMailID = "";

                #region // Get Part Address Details //
                foreach (Party _partyObj in _interchange.DocumentHeader.PartyAddresses)
                {
                    if (_partyObj.Qualifier == PartyQualifier.BY)
                    {
                        RecipientName = convert.ToString(_partyObj.Name).Trim(); // Buyer Name
                        if (_partyObj.Contacts.Count > 0 && _partyObj.Contacts[0].CommunMethodList.Count > 0)
                        {
                            foreach (CommunicationMethods commMethod in _partyObj.Contacts[0].CommunMethodList)
                            {
                                if (commMethod.Qualifier == CommunicationMethodQualifiers.EM)
                                {
                                    ByrMailID = convert.ToString(commMethod.Number).Trim();
                                    break;
                                }
                            }
                        }
                    }
                    else if (_partyObj.Qualifier == PartyQualifier.VN)
                    {
                        SenderName = convert.ToString(_partyObj.Name).Trim(); // Vendor Name
                        if (_partyObj.Contacts.Count > 0 && _partyObj.Contacts[0].CommunMethodList.Count > 0)
                        {
                            foreach (CommunicationMethods commMethod in _partyObj.Contacts[0].CommunMethodList)
                            {
                                if (commMethod.Qualifier == CommunicationMethodQualifiers.EM)
                                {
                                    SuppMailID = convert.ToString(commMethod.Number).Trim();
                                    break;
                                }
                            }
                        }
                    }
                    else if (_partyObj.Qualifier == PartyQualifier.UD)
                    {
                        Vessel = convert.ToString(_partyObj.Name).Trim();
                    }
                }
                #endregion

                if (MailTo.Trim() != "")
                {
                    // Write To File
                    LeSDataMain.LeSDM.SendMailQueueFile(QuotationID.ToString(), DocType.Trim().ToUpper(), VRNO.Trim(), Vessel.Trim(), ActionType.Trim().ToUpper(), MailTo.Trim(), "", MailCC.Trim(), MailBcc.Trim(), SenderName.Trim(), RecipientName.Trim(), Message, MailAuditPath.Trim(), "0", "", "", "0");
                }
                else
                {
                    throw new Exception("MAIL_TO is empty.");
                }
            }
            catch (Exception ex)
            {
                LogText = ("Unable to create Mail notification template. Error : " + ex.StackTrace);
                // CreateAuditFile("", "", VRNO, "Error", "Unable to send mail for Ref No '" + VRNO + "'.Error - " + ex.Message, BuyerCode, SupplierCode, this.AuditPath);
                CreateAuditFile("", "", VRNO, "Error", "LeS-1018:Unable to send mail for " + VRNO + " due to  " + ex.Message, BuyerCode, SupplierCode, this.AuditPath);
            }
        }

        //public void WriteErrorLog_With_Screenshot(string AuditMsg)
        //{
        //    LogText = AuditMsg;
        //    string eFile = ImgPath + "\\Navibulgar_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
        //    if (!PrintScreen(eFile)) eFile = "";
        //    CreateAuditFile(eFile, "Navibulgar_HTTP_Processor", VRNO, "Error", AuditMsg, BuyerCode, SupplierCode, AuditPath);
       // }
    }
}
