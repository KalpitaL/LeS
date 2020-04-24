using HtmlAgilityPack;
using MTML.GENERATOR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Http_zodiac_Routine
{
    public class Http_Download_Routine : LeSCommon.LeSCommon
    {
        string sAuditMesage = "", ImgPath = "", DocType = "", sDoneFile = "", VRNO = "", BuyerContact = "", PageGUID = "", MTML_QuotePath = "",
            MessageNumber = "", LeadDays = "", Currency = "", MsgNumber = "", MsgRefNumber = "", UCRefNo = "", AAGRefNo = "", LesRecordID = "",
            BuyerName = "", BuyerPhone = "", BuyerEmail = "", BuyerFax = "", supplierName = "", supplierPhone = "", supplierEmail = "",
            supplierFax = "", VesselName = "", PortName = "", PortCode = "", SupplierComment = "", PayTerms = "",
            PackingCost = "", FreightCharge = "", GrandTotal = "", Allowance = "", TotalLineItemsAmount = "", BuyerTotal = "", DtDelvDate = "", dtExpDate = "";
        double maxDateRange;
        bool IsDecline = false;
        public string[] Actions;
        public int iRetry = 0, IsAltItemAllowed = 0, IsPriceAveraged = 0, IsUOMChanged = 0;
        List<string> xmlFiles = new List<string>();
        public MTMLInterchange _interchange { get; set; }
        public LineItemCollection _lineitem = null;

        public void LoadAppsettings()
        {
            try
            {
                iRetry = 0;
                HiddenAttributeKey = "name";
                URL = dctAppSettings["SITE_URL"].Trim();
                Userid = dctAppSettings["USERNAME"].Trim();
                Password = dctAppSettings["PASSWORD"].Trim();
                Domain = dctAppSettings["DOMAIN"].Trim();
                BuyerCode = dctAppSettings["BUYERCODE"].Trim();
                SupplierCode = dctAppSettings["SUPPLIERCODE"].Trim();
                Actions = dctAppSettings["ACTIONS"].Trim().Split(',');
                ImgPath = dctAppSettings["IMAGE_PATH"].Trim();
                AuditPath = dctAppSettings["AUDITPATH"].Trim();
                MTML_QuotePath = dctAppSettings["QUOTE_UPLOADPATH"].Trim();
                maxDateRange = convert.ToDouble(dctAppSettings["MAX_DATE_RANGE"].Trim());
                LoginRetry = convert.ToInt(dctAppSettings["LOGINRETRY"].Trim());
                if (maxDateRange == 0) maxDateRange = 1;
                if (AuditPath == "") AuditPath = AppDomain.CurrentDomain.BaseDirectory + "\\AuditLog";
                if (ImgPath == "") ImgPath = AppDomain.CurrentDomain.BaseDirectory + "\\Attachments";
                if (!Directory.Exists(ImgPath)) Directory.CreateDirectory(ImgPath);
            }
            catch (Exception e)
            {
                LogText  = "Exception in LoadAppsettings: " + e.GetBaseException().ToString();
                //LogText = sAuditMesage; //changed by kalpita on 25/10/2019
            //    CreateAuditFile("", "Zodiac_HTTP", "", "Error", sAuditMesage, BuyerCode, SupplierCode, AuditPath);
            }
        }

        public override bool DoLogin(string validateNodeType, string validateAttribute, string attributeValue, bool bload = true)
        {
            bool isLoggedin = false; string strAuditmsg = "";
            try
            {
                _httpWrapper.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
                _httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.AcceptLanguage);
                _httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.AcceptEncoding);
                _httpWrapper._SetRequestHeaders.Add(HttpRequestHeader.AcceptLanguage, "en-IN");
                _httpWrapper._SetRequestHeaders.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                LoadURL("button", "id", "logonForm:j_id_2a");
                _httpWrapper.CooKieName = "";
                foreach (KeyValuePair<string, string> c in _httpWrapper._dctSetCookie)
                {
                    _httpWrapper.CooKieName += c.Key + "=" + c.Value + ";";
                }
                if (_httpWrapper._dctStateData.Count > 0)
                {

                    URL = "https://www.zodiac-maritime.com/extranet/jsp/suppliers/logon.iface";
                    dctPostDataValues.Clear();
                    dctPostDataValues.Add("javax.faces.partial.ajax", "true");
                    dctPostDataValues.Add("javax.faces.source", HttpUtility.UrlEncode("logonForm: j_id_2a"));
                    dctPostDataValues.Add("javax.faces.partial.execute", HttpUtility.UrlEncode("@all"));
                    dctPostDataValues.Add("javax.faces.partial.render", "initMessages");
                    dctPostDataValues.Add("logonForm%3Aj_id_2a", HttpUtility.UrlEncode("logonForm: j_id_2a"));
                    dctPostDataValues.Add("logonForm%3Aj_id_25", HttpUtility.UrlEncode(Userid));
                    dctPostDataValues.Add("logonForm%3Aj_id_27", HttpUtility.UrlEncode(Password));
                    dctPostDataValues.Add("logonForm_SUBMIT", "1");
                    dctPostDataValues.Add("javax.faces.ViewState", _httpWrapper._dctStateData["javax.faces.ViewState"]);
                    isLoggedin = base.DoLogin("redirect", "url", "https://www.zodiac-maritime.com/extranet/suppliers/getQuotations.do", false);
                    if (isLoggedin)
                    {
                        HtmlNode _urlRedirect = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//redirect");
                        if (_urlRedirect != null) URL = _urlRedirect.GetAttributeValue("url", "").Trim();
                        { if (LoadURL(validateNodeType, validateAttribute, attributeValue))isLoggedin = true; }
                    }

                    if (isLoggedin)
                    {
                        LogText = "Logged in successfully";
                    }
                    else
                    {
                        if (iRetry == LoginRetry)
                        {
                            string filename = ImgPath + "\\Zodiac_LoginFailed_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + Domain + ".png";
                            if (!PrintScreen(filename)) filename = "";
                            //LogText = "Login failed";
                            LogText = strAuditmsg = "Unable to login.";
                            CreateAuditFile(filename, "Zodiac_HTTP", "", "Error", "LeS-1014:" + strAuditmsg, BuyerCode, SupplierCode, AuditPath);
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
                    string filename = ImgPath + "\\Zodiac_LoginFailed_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + Domain + ".png";
                    if (!PrintScreen(filename)) filename = "";
                    LogText = strAuditmsg = "Unable to load URL" + URL;
                    CreateAuditFile(filename, "Zodiac_HTTP", "", "Error", "LeS-1016:" + strAuditmsg, BuyerCode, SupplierCode, AuditPath);
                }
            }
            catch (Exception e)
            {
                LogText = "Exception while login : " + e.GetBaseException().Message.ToString();
                if (iRetry > LoginRetry)
                {
                    string filename = ImgPath + "\\Zodiac_LoginFailed_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + Domain + ".png";
                    if (_CurrentResponseString.Contains("The web server is unavailable. Please wait a moment and then refresh the page"))
                    {
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(_CurrentResponseString);
                        var nodes = doc.DocumentNode.SelectNodes("//div[@id='j_id_t:j_id_u']");
                        if (nodes != null)
                        {
                            foreach (HtmlNode node in nodes)
                            {
                                node.Remove();
                            }
                        }

                        nodes = doc.DocumentNode.SelectNodes("//span[@id='j_id_s_title']");
                        if (nodes != null)
                        {
                            foreach (HtmlNode node in nodes)
                            {
                                node.Remove();
                            }
                        }
                        _CurrentResponseString = doc.DocumentNode.InnerHtml;
                        doc = null;

                    }
                    if (!PrintScreen(filename)) filename = "";

                    //LogText = "Login failed";
                    LogText = strAuditmsg = "Unable to login.";
                    CreateAuditFile(filename, "Zodiac_HTTP", "", "Error", "LeS-1014:" + strAuditmsg +" due to : " + e.Message, BuyerCode, SupplierCode, AuditPath);
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
               // WriteErrorLog_With_Screenshot("Exception in Process RFQ : " + e.GetBaseException().ToString());//, 
                WriteErrorLog_With_Screenshot("Unable to process file due to " + e.GetBaseException().ToString(), "LeS-1004:");
            }
        }

        public List<string> GetNewRFQs()
        {
            HtmlNode QuoteTable = null;
            List<string> _lstNewRFQs = new List<string>();
            List<string> slProcessedItem = GetProcessedItems(eActions.RFQ);
            _lstNewRFQs.Clear();
            if (!_httpWrapper._CurrentDocument.DocumentNode.InnerText.Contains("No outstanding requests for quotations"))
            {
                _httpWrapper._CurrentDocument.LoadHtml(_httpWrapper._CurrentResponseString);
                HtmlNodeCollection _Tables = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@class='formatted']");
                if (_Tables != null)
                {
                    foreach (HtmlNode _table in _Tables)
                    {
                        HtmlNodeCollection _tr = _table.SelectNodes("./tr");
                        if (_tr.Count > 1) { QuoteTable = _table; break; }
                    }

                    if (QuoteTable.SelectNodes("./tr").Count > 0)
                    {
                        foreach (HtmlNode _row in QuoteTable.SelectNodes("./tr"))
                        {
                            HtmlNodeCollection _rowData = _row.ChildNodes;
                            if (_rowData[1].InnerText.Trim().ToUpper() != "REQUISITION")
                            {
                                string _vrno = _rowData[1].InnerText.Trim();
                                string _url = "https://www.zodiac-maritime.com/extranet/suppliers/" + _rowData[1].SelectSingleNode("./a").GetAttributeValue("href", "").Trim();
                                string _rfqdate = _rowData[5].InnerText.Trim();
                                string _buyercontact = _rowData[13].InnerText.Trim();
                                string _itemCount = _rowData[11].InnerText.Trim();
                                //  string _guid = _vrno + "|" + _rfqdate;
                                string _info = _vrno + "|" + _rfqdate + "|" + _buyercontact + "|" + _url + "|" + _itemCount;
                                string sRFQDate = GetDate(_rfqdate);
                                DateTime dtRFQDate = DateTime.MinValue;
                                if (sRFQDate != "")
                                {
                                    DateTime.TryParseExact(sRFQDate.Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out dtRFQDate);
                                }

                                if (!_lstNewRFQs.Contains(_info) && !slProcessedItem.Contains(this.DocType + "|" + _vrno + "|" + _rfqdate) && (dtRFQDate != DateTime.MinValue && dtRFQDate > DateTime.Now.AddDays(-maxDateRange)))
                                {
                                    _lstNewRFQs.Add(_info);
                                }
                            }
                        }
                    }
                }

            }
          //  else LogText = "No outstanding requests for quotations";
            return _lstNewRFQs;
        }

        public void DownloadRFQ(List<string> _lstNewRFQs)
        {
            string strAuditmsg = "";
            foreach (string strRFQ in _lstNewRFQs)
            {
                try
                {
                    string[] lst = strRFQ.Split('|');
                    this.VRNO = lst[0];
                    this.URL = lst[3];
                    this.BuyerContact = lst[2];
                    string RFQDate = lst[1];
                    string ItemCount = lst[4];

                    LogText = "Processing RFQ for ref no " + this.VRNO;
                    if (LoadURL("input", "id", "subm") || LoadURL("input", "id", "next"))//LoadURL("input","id","next")//16-718 for multipages
                    {
                        if (IsValidPage())
                        {
                            string eFile = this.ImgPath + "\\" + this.VRNO.Replace("/", "_") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
                            //added as png generated blank ,if script tag present in html
                            HtmlDocument _doc = new HtmlAgilityPack.HtmlDocument();
                            _doc.LoadHtml(_CurrentResponseString);

                            _doc.DocumentNode.Descendants()
                .Where(n => n.Name == "script")
                .ToList()
                .ForEach(n => n.Remove());

                            _CurrentResponseString = _doc.DocumentNode.OuterHtml;
                            if (!PrintScreen(eFile)) eFile = "";
                            _doc = null;
                            this.PageGUID = this.DocType + "|" + VRNO + "|" + RFQDate;



                            LeSXML.LeSXML _lesXml = new LeSXML.LeSXML();
                            if (GetRFQHeader(ref _lesXml, eFile))
                            {
                                if (GetRFQItems(ref _lesXml))
                                {
                                    if (convert.ToInt(ItemCount) == convert.ToInt(_lesXml.Total_LineItems))
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
                                                    CreateAuditFile(xmlfile, "Zodiac_HTTP_RFQ", VRNO, "Downloaded", xmlfile + " downloaded successfully.", BuyerCode, SupplierCode, AuditPath);
                                                    if (this.PageGUID.Trim().Length > 0)
                                                        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + this.Domain + "_" + this.SupplierCode + "_GUID.txt", this.PageGUID + Environment.NewLine);
                                                }
                                                else
                                                {
                                                    LogText = strAuditmsg = "Unable to process file " + xmlfile + " for ref " + VRNO + ".";//"Unable to download file " + xmlfile + " for ref " + VRNO + "."
                                                    string filename = ImgPath + "\\Zodiac_RFQError_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                                                    CreateAuditFile(filename, "Zodiac_HTTP_RFQ", VRNO, "Error", "LeS-1004:" + strAuditmsg, BuyerCode, SupplierCode, AuditPath);
                                                    if (PrintScreen(filename)) filename = "";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //LogText = strAuditmsg = "Unable to get address details";
                                            LogText = strAuditmsg = "Unable to get details for " + this.DocType + "- address Field(s) not present";
                                            CreateAuditFile(eFile, "Zodiac_HTTP_Processor", VRNO, "Error", "LeS-1040:" + strAuditmsg, BuyerCode, SupplierCode, AuditPath);
                                        }
                                    }
                                    else
                                    {
                                        //WriteErrorLog_With_Screenshot("Total line item count mismatched");
                                        WriteErrorLog_With_Screenshot("Unable to save file, item count mismatch", "LeS-1008.4:");
                                    }
                                }
                                else
                                {
                                    //LogText = "Unable to get RFQ item details";
                                    LogText = strAuditmsg = "Unable to filter details";
                                    CreateAuditFile(eFile, "Zodiac_HTTP_RFQ", VRNO, "Error", "LeS-1006:" + strAuditmsg, BuyerCode, SupplierCode, AuditPath);
                                }
                            }
                            else
                            {
                                //LogText = "Unable to get RFQ header details";
                                LogText = strAuditmsg = "Unable to filter details";
                                CreateAuditFile(eFile, "Zodiac_HTTP_RFQ", VRNO, "Error", "LeS-1006:" + strAuditmsg, BuyerCode, SupplierCode, AuditPath);
                            }
                        }
                        else
                        {
                            //WriteErrorLog_With_Screenshot("Unable to get RFQ page for RFQ no: " + VRNO, "LeS-1004:");
                            WriteErrorLog_With_Screenshot("Unable to filter details for RFQ no: " + VRNO, "LeS-1006:");
                        }
                    }
                    else
                    {
                        //WriteErrorLog_With_Screenshot("Unable to get RFQ page for ref no: " + VRNO, "LeS-1004:");
                        WriteErrorLog_With_Screenshot("Unable to filter details for ref no: " + VRNO, "LeS-1006:");
                    }
                }
                catch (Exception ex)
                {
                    //WriteErrorLog_With_Screenshot("Unable to Download RFQ '" + VRNO + "' details due to " + ex.GetBaseException().Message.ToString(), "LeS-1004:");
                    WriteErrorLog_With_Screenshot("Unable to Unable to process file for '" + VRNO + "' due to " + ex.GetBaseException().Message.ToString(), "LeS-1004:");
                }
            }
        }

        public bool GetRFQHeader(ref LeSXML.LeSXML _lesXml, string eFile)
        {
            bool isResult = false;
            LogText = "Start Getting Header details";
            try
            {
                string _date = DateTime.Now.ToString("yyyyMMddHHmmssff");
                _lesXml.DocID = _date;
                _lesXml.Created_Date = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Doc_Type = "RFQ";
                _lesXml.Dialect = "Zodiac Maritime Ltd";
                _lesXml.Version = "1";
                _lesXml.Date_Preparation = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Sender_Code = BuyerCode;
                _lesXml.Recipient_Code = SupplierCode;
                _lesXml.DocLinkID = this.URL;

                if (File.Exists(eFile))
                    _lesXml.OrigDocFile = Path.GetFileName(eFile);

                _lesXml.Active = "1";
                HtmlNode _tblHeader = null;
                HtmlNodeCollection _tables = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@class='formatted']");
                foreach (HtmlNode _table in _tables)
                {
                    HtmlNodeCollection _tr = _table.SelectNodes("./tr");
                    if (_tr.Count == 4) { _tblHeader = _table; break; }
                }

                foreach (HtmlNode _row in _tblHeader.SelectNodes("./tr"))
                {
                    HtmlNodeCollection _rowData = _row.ChildNodes;
                    if (_rowData[1].InnerText.ToLower().Trim().Contains("requisition number"))
                    {
                        _lesXml.DocReferenceID = _date;
                        _lesXml.BuyerRef = _rowData[3].InnerText.Trim();
                        _lesXml.Vessel = _rowData[7].InnerText.Trim();
                        _lesXml.Date_Document = GetDate(_rowData[11].InnerText.Trim());
                        string port = _rowData[15].InnerText.Trim();
                        if (port.ToLower().Trim() == "not specified") _lesXml.PortName = "";
                        else _lesXml.PortName = port;
                        string vesseleta = _rowData[19].InnerText.Trim();
                        if (vesseleta != "-") _lesXml.Date_ETA = GetDate(vesseleta);
                    }
                    if (_rowData[1].InnerText.ToLower().Trim().Contains("additional info"))
                    {
                        _lesXml.Remark_Header = _rowData[3].InnerText.Trim();
                    }
                }
                _lesXml.Currency = "";

                if (_httpWrapper._dctStateData.Count > 0)
                {
                    _lesXml.OrigDocReference = _httpWrapper._dctStateData["requisition.documentCode"];
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
            string EquipName = "";
            string EquipRemarks = "";
            string SerialNo = "";
            try
            {
                _lesXml.LineItems.Clear();
                LogText = "Start Getting LineItem details";
                if (this.DocType == "RFQ")
                {
                    HtmlNode _tblItems = null; int counter = 0, ItemCounter = 0,iNumber=0;
                    HtmlNodeCollection _tables = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@class='formatted']");
                    foreach (HtmlNode _table in _tables)
                    {
                        counter++;
                        if (counter == 2) { _tblItems = _table; break; }
                    }

                    if (_tblItems.SelectNodes("./tr").Count > 1)
                    {
                        int count = 0;
                        bool isSubmit = true,isProcess=false;
                        HtmlNode _submit = null;
                        do
                        {
                            if (count == 0)
                            {
                                 _submit = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//input[@id='subm']");
                                if (_submit == null) isSubmit = false;
                            }
                            else if (count > 0)
                            {
                                URL = this.URL + "&pageNo=" + count + "&saved=true";
                                if (LoadURL("input", "id", "subm") || LoadURL("input", "id", "next"))
                                {
                                     _submit = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//input[@id='subm']");
                                     if (_submit == null) isSubmit = false;
                                     else isSubmit = true;
                                    isProcess = true;
                                    _tblItems = null; counter = 0; ItemCounter = 0;
                                     _tables = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@class='formatted']");
                                    foreach (HtmlNode _table in _tables)
                                    {
                                        counter++;
                                        if (counter == 2) { _tblItems = _table; break; }
                                    }
                                }
                            }

                            if ((count == 0 || isProcess) && _tblItems.SelectNodes("./tr").Count > 1)
                            {
                                foreach (HtmlNode _row in _tblItems.SelectNodes("./tr"))
                                {
                                    try
                                    {
                                        HtmlNodeCollection _rowData = _row.ChildNodes;
                                        if (_rowData.Count > 1)
                                        {
                                            if (_rowData[1].InnerText.ToLower().Trim() != "part no.")
                                            {
                                                if (_rowData.Count == 3 && _rowData[1].GetAttributeValue("style", "").Trim() == "background-color:cfe")
                                                {
                                                    EquipName = "";
                                                    EquipRemarks = "";
                                                    SerialNo = "";
                                                    if (_rowData[1].InnerText.Trim().StartsWith("Store"))
                                                    {
                                                        string[] _values = _rowData[1].InnerText.Trim().Replace("Store:", "").Replace("Group:", "|Group:").Split('|');
                                                        EquipName = _values[0].Trim();
                                                        EquipRemarks = _values[1].Trim();
                                                    }
                                                    else
                                                    {
                                                        string[] _values = _rowData[1].InnerText.Trim().Replace("Function:", "")
                                                                   .Replace("SubSystem Desc:", "|")
                                                               .Replace("System Desc:", "|")
                                                               .Replace("Location:", "|")
                                                               .Replace("SerialNo:", "|").Split('|');

                                                        if (_values.Length == 5)
                                                        {
                                                            EquipName = _values[0];
                                                            if (_values[1].Trim().Length > 0) EquipRemarks += "System Desc: " + _values[1].Trim();
                                                            if (_values[2].Trim().Length > 0) EquipRemarks += Environment.NewLine + "SubSystem Desc: " + _values[2].Trim();
                                                            if (_values[3].Trim().Length > 0) EquipRemarks += Environment.NewLine + "Location: " + _values[3].Trim();
                                                            if (_values[4].Trim().Length > 0) SerialNo = _values[4].Trim();

                                                            EquipRemarks = EquipRemarks.Trim();
                                                        }

                                                    }
                                                }
                                                else if (_rowData.Count > 3 && !_rowData[1].InnerText.Trim().ToLower().Contains("balance b/f"))
                                                {
                                                    LeSXML.LineItem _item = new LeSXML.LineItem();
                                                    if (_httpWrapper._dctStateData.Count > 0)
                                                    {
                                                        if (_httpWrapper._dctStateData.ContainsKey(HttpUtility.UrlEncode("quotedItem[" + ItemCounter.ToString() + "].itemSerialNo")))
                                                        {
                                                            iNumber++;
                                                            _item.Number = Convert.ToString(iNumber);//ItemCounter + 1
                                                            _item.OriginatingSystemRef = _httpWrapper._dctStateData[HttpUtility.UrlEncode("quotedItem[" + ItemCounter.ToString() + "].itemSerialNo")];
                                                            _item.ItemRef = _rowData[1].InnerText.Trim();
                                                            _item.Name = _rowData[5].InnerText.Trim();
                                                            _item.Quantity = _rowData[7].InnerText.Trim();
                                                            _item.Unit = _rowData[9].InnerText.Trim();
                                                            _item.Equipment = EquipName.Trim();
                                                            _item.EquipRemarks = EquipRemarks;
                                                            if (SerialNo.Trim().Length > 0) _item.EquipRemarks += " Serial No: " + SerialNo.Trim();
                                                            if (_rowData[3].InnerText.Trim().Length > 0) _item.Remark += "Draw No: " + _rowData[3].InnerText.Trim();
                                                            ItemCounter++;
                                                            _lesXml.LineItems.Add(_item);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    { LogText = ex.GetBaseException().ToString(); }
                                }
                                
                            }
                            else isResult = false;
                            count++;
                        }
                        while (!isSubmit);
                        _lesXml.Total_LineItems = Convert.ToString(_lesXml.LineItems.Count);
                        isResult = true;
                    }
                    else isResult = false;
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
                HtmlNode _bName = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//div[@id='header']//h1");
                if (_bName != null)
                {
                    _xmlAdd.AddressName = _bName.InnerText.Trim();
                    _lesXml.Sender_Name = _bName.InnerText.Trim();
                }
                _xmlAdd.ContactPerson = BuyerContact;
                _lesXml.Addresses.Add(_xmlAdd);


                _xmlAdd = new LeSXML.Address();
                _xmlAdd.Qualifier = "VN";
                if (_httpWrapper._dctStateData.Count > 0)
                {
                    if (_httpWrapper._dctStateData.ContainsKey("requisition.quotationSupplierName"))
                        _xmlAdd.AddressName = HttpUtility.UrlDecode(_httpWrapper._dctStateData["requisition.quotationSupplierName"]);
                }

                HtmlNode _logout = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//div[@id='menubar']//a[@style='color: #888; font-size: xx-small;']");
                if (_logout != null)
                {
                    if (_logout.InnerText.Trim().StartsWith("Logout"))
                    {
                        string[] values = _logout.InnerText.Trim().Replace(" @ ", "|").Split('|');
                        _xmlAdd.AddressName = values[1].Trim();
                    }
                }
                _lesXml.Recipient_Name = _xmlAdd.AddressName;
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

        public bool IsValidPage()
        {
            bool res = false;
            if (this.DocType == "RFQ")
            {
                if (_httpWrapper._CurrentResponseString.Contains(this.VRNO)) res = true;
                else res = false;
            }
            return res;
        }

        private string GetDate(string DateValue)
        {
            if (DateValue.Trim().Length > 0)
            {
                string _dtValue = "";
                DateTime dt = DateTime.MinValue;
                DateTime.TryParseExact(DateValue, "dd/MM/yy", null, System.Globalization.DateTimeStyles.None, out dt);
                if (dt == DateTime.MinValue) DateTime.TryParseExact(DateValue, "d/MM/yy", null, System.Globalization.DateTimeStyles.None, out dt);
                if (dt == DateTime.MinValue) DateTime.TryParseExact(DateValue, "dd/M/yy", null, System.Globalization.DateTimeStyles.None, out dt);
                if (dt == DateTime.MinValue) DateTime.TryParseExact(DateValue, "d/M/yy", null, System.Globalization.DateTimeStyles.None, out dt);
                if (dt == DateTime.MinValue) DateTime.TryParse(DateValue, out dt);
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
        #endregion

        #region Quote
        public void ProcessQuote()
        {
            int j = 0;
            try
            {
                this.DocType = "Quote";
                LogText = "";
                LogText = "Quote processing started.";
                GetXmlFiles();
                if (xmlFiles.Count > 0)
                {
                    LogText = xmlFiles.Count + " Quote files found to process.";
                    for (j = 0; j < xmlFiles.Count; j++)
                    {
                        ProcessQuoteMTML(xmlFiles[j]);
                    }
                }
                else LogText = "No quote file found.";
                LogText = "Quote processing stopped.";
            }
            catch (Exception e)
            {
                //WriteErrorLogQuote_With_Screenshot("Exception in Process Quote : " + e.GetBaseException().ToString(), xmlFiles[j]);
                WriteErrorLogQuote_With_Screenshot("Unable to process file due to " + e.GetBaseException().ToString(), xmlFiles[j], "LeS-1004:");
            }
        }

        public void ProcessQuoteMTML(string MTML_QuoteFile)
        {
            try
            {
                MTMLClass _mtml = new MTMLClass();
                _interchange = _mtml.Load(MTML_QuoteFile);
                LoadInterchangeDetails();
                if (UCRefNo != "")
                {
                    URL = MessageNumber;
                    if (LoadURL("input", "id", "subm"))
                    {
                        if (!_CurrentResponseString.Contains("Status: Quotation successful"))
                        {

                        }
                        else
                        {
                            //WriteErrorLogQuote_With_Screenshot("Unable to process quote as status is Quotation successful.", Path.GetFileName(MTML_QuoteFile));
                            WriteErrorLogQuote_With_Screenshot("Unable to Save Quote since Quote is in 'success' status", Path.GetFileName(MTML_QuoteFile), "LeS-1008.2:");
                        }
                    }
                    else
                    {
                        //WriteErrorLogQuote_With_Screenshot("Unable to process quote as Submit Quote button is not found on page.", Path.GetFileName(MTML_QuoteFile));
                        WriteErrorLogQuote_With_Screenshot("Unable to Submit Quote due to missing controls", Path.GetFileName(MTML_QuoteFile), "LeS-1011.4:");
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteErrorLogQuote_With_Screenshot("Exception while processing Quote MTML: " + ex.GetBaseException().ToString(), Path.GetFileName(MTML_QuoteFile));
                WriteErrorLogQuote_With_Screenshot("Unable to process file due to " + ex.GetBaseException().ToString(), Path.GetFileName(MTML_QuoteFile),"LeS-1004:");
            }
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

        public bool Logout()
        {
            bool result = false;
            URL = "https://www.zodiac-maritime.com/extranet/jsp/suppliers/logon.iface?logoff=true";
            if (LoadURL("button","id","logonForm:j_id_2a"))
            {
                result = true;
            }
            return result;
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
            string eFile = ImgPath + "\\Zodiac_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
            if (!PrintScreen(eFile)) eFile = "";
            CreateAuditFile(eFile, "Zodiac_HTTP", VRNO, "Error", ErrorNo + AuditMsg, BuyerCode, SupplierCode, AuditPath);
        }

        public void WriteErrorLogQuote_With_Screenshot(string AuditMsg, string _File, string ErrorNo)
        {
            LogText = AuditMsg;
            string eFile = ImgPath + "\\Zodiac_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            _httpWrapper._CurrentDocument.DocumentNode.Descendants()
              .Where(n => n.Name == "script")
              .ToList()
              .ForEach(n => n.Remove());

            _CurrentResponseString = _httpWrapper._CurrentDocument.DocumentNode.OuterHtml;
            if (!PrintScreen(eFile)) eFile = "";
            CreateAuditFile(eFile, "Zodiac_HTTP", VRNO, "Error", ErrorNo+AuditMsg, BuyerCode, SupplierCode, AuditPath);

            if (!Directory.Exists(MTML_QuotePath + "\\Error")) Directory.CreateDirectory(MTML_QuotePath + "\\Error");
            File.Move(MTML_QuotePath + "\\" + _File, MTML_QuotePath + "\\Error\\" + _File);
        }


        //public void WriteErrorLog_With_Screenshot(string AuditMsg)
        //{
        //    LogText = AuditMsg;
        //    string eFile = ImgPath + "\\Zodiac_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
        //    if (!PrintScreen(eFile)) eFile = "";
        //    CreateAuditFile(eFile, "Zodiac_HTTP", VRNO, "Error", AuditMsg, BuyerCode, SupplierCode, AuditPath);
        //}

        //public void WriteErrorLogQuote_With_Screenshot(string AuditMsg, string _File)
        //{
        //    LogText = AuditMsg;
        //    string eFile = ImgPath + "\\Zodiac_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        //    _httpWrapper._CurrentDocument.DocumentNode.Descendants()
        //      .Where(n => n.Name == "script")
        //      .ToList()
        //      .ForEach(n => n.Remove());

        //    _CurrentResponseString = _httpWrapper._CurrentDocument.DocumentNode.OuterHtml;
        //    if (!PrintScreen(eFile)) eFile = "";
        //    CreateAuditFile(eFile, "Zodiac_HTTP", VRNO, "Error", AuditMsg, BuyerCode, SupplierCode, AuditPath);

        //    if (!Directory.Exists(MTML_QuotePath + "\\Error")) Directory.CreateDirectory(MTML_QuotePath + "\\Error");
        //    File.Move(MTML_QuotePath + "\\" + _File, MTML_QuotePath + "\\Error\\" + _File);
        //}
    }
}
