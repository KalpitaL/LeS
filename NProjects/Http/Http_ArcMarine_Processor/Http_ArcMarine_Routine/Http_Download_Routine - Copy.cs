using HtmlAgilityPack;
using MTML.GENERATOR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Http_ArcMarine_Routine
{
    public class Http_Download_Routine1 : LeSCommon.LeSCommon
    {
        string Module { get; set; }
        string DocType { get; set; }
        bool moveToError = false, IsRFQ = false, IsPO = false;
        string MapPath = "", LinkPath = "", ImagePath = "", QuotePOCPath = "", errLog = "", VRNO = "", MsgFile = "", orgEmlFile = "", BuyerName = "", currentURL = ""
            , FileType = "", currentXMLFile = "", UCRefNo, cQuoteURL, cPrevPageData, cViewStateGen, cViewState,cEventArg,cScrollYPst;
        Dictionary<string, string> lstBuyer = new Dictionary<string, string>();
        Dictionary<string, string> lstSupp = new Dictionary<string, string>();
        List<List<string>> _xmlItems = null;

        //Quote
        List<string> xmlFiles = new List<string>();
        public LineItemCollection _lineitem = null;
        public MTMLInterchange _interchange { get; set; }
        bool IsDecline = false;
        string MessageNumber = "", LeadDays = "", Currency = "", MsgRefNumber = "", MsgNumber = "", AAGRefNo = "", LesRecordID = "", TransportMode = "", BuyerPhone = "",
            BuyerEmail = "", BuyerFax = "", supplierName = "", supplierPhone = "", supplierEmail = "", supplierFax = "", VesselName = "", PortName = "", PortCode = ""
            , ImoNo = "", SupplierComment = "", PayTerms = "", TermsCond = "", PackingCost = "", FreightCharge = "", GrandTotal = "", Allowance = "",
            TotalLineItemsAmount = "", BuyerTotal = "", TaxCost = "", DtDelvDate = "", dtExpDate = "", Remarks = "",fixedUrl="",_errorlog="",_nxtpageData="",strnxtpgdetails="";
        double AddDiscount = 0;
        int IsAltItemAllowed = 0, IsPriceAveraged = 0, IsUOMChanged = 0, counter = 0;
        HtmlAgilityPack.HtmlDocument _nxtDoc;
        Dictionary<string, string> slnxtPages = new Dictionary<string, string>();


        public void LoadAppSettings()
        {
            SessionIDCookieName = "ASP.NET_SessionId";
            MapPath = AppDomain.CurrentDomain.BaseDirectory + "Map_Files";
            LinkPath = ConfigurationManager.AppSettings["LINK_PATH"].Trim();
            ImagePath = ConfigurationManager.AppSettings["IMAGE_PATH"].Trim();
            AuditPath = ConfigurationManager.AppSettings["AUDIT_PATH"].Trim();
            QuotePOCPath = ConfigurationManager.AppSettings["QUOTE_PATH"].Trim();
        }

        public void ProcessLinkFiles()
        {
            try
            {
                LoadAppSettings();
                SetBuyerSupplierCodes();
                LogText = "ArcMarine Processor Started.";
                if (LinkPath.Trim().Length == 0)
                {
                    errLog = "Link Path not found";
                    throw new Exception("Link Path not found");
                }

                string[] _files = Directory.GetFiles(LinkPath, "*.txt");
                if (_files.Length > 0)
                {
                    foreach (string sFile in _files)
                    {
                        FileInfo file = new FileInfo(sFile);
                        try
                        {
                            #region Copy Link File to Attachment Path
                            if (this.ImagePath.Trim() != "" && Directory.Exists(this.ImagePath))
                            {
                                file.CopyTo(this.ImagePath + "\\" + file.Name, true);
                            }
                            #endregion
                            this.MsgFile = file.Name;

                            moveToError = true;
                            errLog = "";
                            this.orgEmlFile = sFile; // Full path of eml file

                            BuyerCode = ""; BuyerName = ""; SupplierCode = ""; VRNO = "";

                            LogText = "Processing File " + Path.GetFileName(sFile);

                            currentURL = this.URL = GetURL(sFile);

                            if (this.URL == "")
                            {
                                this.URL = GetURLWithHttp(sFile);
                            }
                            LogText = " URL : " + this.URL;
                            if (this.URL != "" && (this.URL.Contains("PurchaseQuotationItems")))
                            {
                                #region /* Process RFQ */
                                this.DocType = "RFQ";
                                this.Module = "ArcMarine_RFQ";                                
                                bool isFileProcessed = GetRFQDetails(this.URL);
                                if (isFileProcessed)
                                {
                                    // move to backup
                                    MoveFile(LinkPath + "\\Backup", file);
                                }
                                else
                                {
                                    LogText = "Unable to process link file '" + file.Name + "'.";
                                    if (moveToError)
                                    {
                                        MoveFile(LinkPath + "\\Error_files", file);
                                    }
                                    CreateAuditFile(file.Name, "ArcMarine_Http_RFQ", VRNO.Trim(), "Error", "Unable to process link file.", BuyerCode, SupplierCode, AuditPath);
                                }
                                #endregion
                            }
                            else
                            {
                                LogText = "Unable to process link file; Link not found in file " + Path.GetFileName(sFile);
                                CreateAuditFile(file.Name, "ArcMarine_Http", VRNO.Trim(), "Error", "Unable to process link file; Link not found in file " + Path.GetFileName(sFile), BuyerCode, SupplierCode, AuditPath);
                                MoveFile(LinkPath + "\\Error_files", file);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (convert.ToString(ex.Message).Trim().ToLower().Contains("could not find a frame or iframe") || convert.ToString(ex.Message).Trim().ToLower().Contains("creating an instance of the com"))
                            {
                                moveToError = false;
                            }
                            else moveToError = true;

                            // Move to error_files //
                            LogText = "Unable to process link file '" + file.Name + "'. Error - " + ex.Message;

                            if (this.Module == "ArcMarine_RFQ")
                            {
                                if (moveToError != false) CreateAuditFile(file.Name, "ArcMarine_Http_RFQ", VRNO.Trim(), "Error", "Unable to process link file '" + file.Name + "'. Error - " + ex.Message, BuyerCode, SupplierCode, AuditPath);
                            }
                            else
                            {
                                moveToError = true;
                                CreateAuditFile(file.Name, "ArcMarine_Http", this.VRNO.Trim(), "Error", "Unable to process link file '" + file.Name + "'. Error -" + ex.Message, BuyerCode, SupplierCode, AuditPath);
                            }

                            if (moveToError)
                            {
                                MoveFile(LinkPath + "\\Error_files", file);
                            }
                            string eFile = ImagePath + "\\ArcMarine_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                            _httpWrapper.GetElement("div", "id", "navigation").Attributes["class"].Remove();

                            _CurrentResponseString = _httpWrapper._CurrentDocument.DocumentNode.OuterHtml;
                            if (!PrintScreen(eFile)) eFile = "";
                        }
                    }
                    MoveInvalidFilesToFolder();
                }
                else LogText = "No files found.";
            }
            catch (Exception ex)
            {
                LogText = "Error in ProcessLinkFiles() - " + ex.Message + ex.StackTrace;
            }
        }

        public void MoveInvalidFilesToFolder()
        {
            string[] pdfFiles = Directory.GetFiles(LinkPath, "*.pdf");
            foreach (string pdfFile in pdfFiles)
            {
                bool textFound = false;
                if (File.Exists(pdfFile))
                {
                    textFound = true;
                }

                if (textFound)
                {
                    FileInfo _movepdf = new FileInfo(pdfFile);

                    // Move file to Error Files
                    MoveFile(_movepdf.Directory.FullName + "\\InvalidFiles", _movepdf);
                    LogText = "Invalid File " + _movepdf.Name + " moved to invalidFiles folder";
                }
            }
        }

        private bool GetRFQDetails(string URL)
        {
            bool result = false;
            try
            {
                this.LogText = "Navigating To URL ..";
                _httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.CacheControl);
                _httpWrapper.ContentType = "";
                if (LoadURL("iframe", "id", "filterandsearch", true))
                {
                    string _RFQLink = _httpWrapper.GetElement("iframe", "id", "filterandsearch").GetAttributeValue("src", "").Trim();
                    string[] arrLink = this.URL.Split('/');
                    if (arrLink.Length > 0)
                    {
                        _RFQLink = arrLink[arrLink.Length - 1].Split('?')[1];
                        //currentURL = this.URL = @"https://apps2.southnests.com/ArchangelReports/Purchase/PurchaseQuotationRFQ.aspx?" + HttpUtility.HtmlDecode(_RFQLink);
                        currentURL = this.URL =  @"https://"+arrLink[2]+"/ArchangelReports/Purchase/PurchaseQuotationRFQ.aspx?" + HttpUtility.HtmlDecode(_RFQLink);//04052020
                        if (LoadURL("input", "id", "txtnopage", true))
                        {
                            HtmlNode _btnSave = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//a[@id='MenuQuotationLineItem_dlstTabs_ctl00_btnMenu'][@title='Save']");
                            if (_btnSave != null)
                            {
                                LogText = "Page Loaded ..";
                                FetchData();

                                if (File.Exists(currentXMLFile))
                                {
                                    LogText = "File saved ..";
                                    result = true;
                                }
                                else
                                {
                                    result = false;
                                }
                            }
                            else
                            {
                                throw new Exception("RFQ is in closed status");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }
            return result;
        }

        public void FetchData()
        {
            string strerr = "";
            try
            {
                _xmlItems = new List<List<string>>();
                strerr = "Unable to get header data!";
                Dictionary<string, string> _xmlHeader = GetHeaderData();

                strerr = "Unable to get item data!";
                _xmlItems = GetItemsData(_xmlHeader);

                #region get  png
                string filename = ImagePath + "\\" + this.DocType + "_" + this.SupplierCode + "_" + this.VRNO + "_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";//GetAttachment();
                _httpWrapper.GetElement("div", "id", "navigation").Attributes["class"].Remove();

                _CurrentResponseString = _httpWrapper._CurrentDocument.DocumentNode.OuterHtml;
                if (!PrintScreen(filename)) filename = "";
                this.URL = currentURL;
                if (LoadURL("input", "id", "txtnopage", true))
                {
                }
                #endregion

                if (IsRFQ || IsPO)
                {
                    strerr = "Unable to export xml!";
                    ExportToLESML(_xmlHeader, _xmlItems, filename);
                }

                Logout();
                IsRFQ = false; IsPO = false;
            }
            catch (Exception ex)
            {
                LogText = strerr + " Error - " + ex.Message;
            }
        }

        public Dictionary<string, string> GetHeaderData()
        {
            Dictionary<string, string> _xmlHeader = new Dictionary<string, string>();
            try
            {
                LogText = "Reading Header Details ..";
                if (this.DocType == "RFQ")
                {
                    #region read RFQ Header map files
                    if (File.Exists(MapPath + "\\" + this.DocType + "HEADER_MAP_ARCMARINE.txt"))
                    {
                        string[] _lines = File.ReadAllLines(MapPath + "\\" + this.DocType + "HEADER_MAP_ARCMARINE.txt");
                        for (int i = 0; i < _lines.Length; i++)
                        {
                            string[] _keys = _lines[i].Split('=');
                            if (_keys.Length > 1)
                            {
                                if (_keys[0] != "DOC_HEADER_VALUE")
                                {
                                    string _value = "";
                                    if (_keys[1].Split('|')[0] == "input")
                                    {
                                        _value = _httpWrapper.GetElement(_keys[1].Split('|')[0], "id", _keys[1].Split('|')[1]).GetAttributeValue("value", "").Trim();
                                    }

                                    else if (_keys[1].Split('|')[0] == "span" || _keys[1].Split('|')[0] == "textarea")
                                    { 
                                        if(_keys[0].ToUpper() == "VRNO" )//changed by kalpita on 13/02/2020
                                        {
                                            string[] _Titleval = _keys[1].Split('|')[1].Split(',');
                                            _value = _httpWrapper.GetElement(_keys[1].Split('|')[0], "id", _Titleval[0]).InnerText.Trim();
                                            if (string.IsNullOrEmpty(_value))
                                            {
                                                _value = _httpWrapper.GetElement(_keys[1].Split('|')[0], "id", _Titleval[1]).InnerText.Trim();
                                            }
                                        }
                                        else
                                        {
                                            _value = _httpWrapper.GetElement(_keys[1].Split('|')[0], "id", _keys[1].Split('|')[1]).InnerText.Trim();
                                        }

                                      
                                    }
                                    else if (_keys[1].Split('|')[0] == "select")
                                    {
                                        var options = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='" + _keys[1].Split('|')[1] + "']/option[@selected='selected']");
                                        if (options.Count == 1)
                                            _value = (options[0].NextSibling).InnerText.Trim();

                                    }
                                    if (String.IsNullOrEmpty(_value)) { _value = string.Empty; }
                                    _xmlHeader.Add(_keys[0], _value);

                                }
                                else _xmlHeader.Add(_keys[0], _keys[1]);
                            }
                        }
                    }
                    _xmlHeader.Add("DOC_TYPE", this.DocType);
                    #endregion

                    string HeaderDetails = "";
                    #region // Get Details //
                    HtmlNode _lnkDelInstr = _httpWrapper.GetElement("a", "id", "MenuQuotationLineItem_dlstTabs_ctl02_btnMenu");
                    if (_lnkDelInstr != null)
                    {
                        if (_lnkDelInstr.InnerText.Trim() == "Details")
                        {
                            _httpWrapper.ContentType = "application/x-www-form-urlencoded";

                            dctPostDataValues.Clear();
                            dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", "");
                            dctPostDataValues.Add("__EVENTTARGET", "MenuQuotationLineItem%24dlstTabs%24ctl02%24btnMenu");
                            dctPostDataValues.Add("__EVENTARGUMENT", "");
                            dctPostDataValues.Add("__LASTFOCUS", "");
                            dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                            dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                            dctPostDataValues.Add("__SCROLLPOSITIONX", "0");
                            dctPostDataValues.Add("__SCROLLPOSITIONY", "0");
                            dctPostDataValues.Add("txtEnquiryNumber", _httpWrapper.GetElement("input", "id", "txtEnquiryNumber").GetAttributeValue("value", "").Trim());
                            dctPostDataValues.Add("txtDeliveryPlace", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtDeliveryPlace").GetAttributeValue("value", "").Trim()));
                            dctPostDataValues.Add("txtContactNo", _httpWrapper.GetElement("input", "id", "txtContactNo").GetAttributeValue("value", "").Trim());
                            dctPostDataValues.Add("txtSenderName", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSenderName").GetAttributeValue("value", "").Trim()));
                            dctPostDataValues.Add("txtSentDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSentDate").GetAttributeValue("value", "").Trim()));
                            dctPostDataValues.Add("txtSenderEmailId", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSenderEmailId").GetAttributeValue("value", "").Trim()));
                            dctPostDataValues.Add("txtVendorName", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVendorName").GetAttributeValue("value", "").Trim()));
                            dctPostDataValues.Add("txtTelephone", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtTelephone").GetAttributeValue("value", "").Trim()));
                            dctPostDataValues.Add("txtFax", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtFax").GetAttributeValue("value", "").Trim()));
                            dctPostDataValues.Add("txtVendorAddress", Uri.EscapeDataString(_httpWrapper.GetElement("textarea", "id", "txtVendorAddress").InnerText.Trim()));
                            dctPostDataValues.Add("txtEmail", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtEmail").InnerText.Trim()));
                            dctPostDataValues.Add("txtVenderReference", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVenderReference").InnerText.Trim()));
                            dctPostDataValues.Add("txtOrderDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtOrderDate").GetAttributeValue("value", "").Trim()));
                            dctPostDataValues.Add("txtPriority", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtPriority").GetAttributeValue("value", "").Trim()));
                            var delOptions = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='UCDeliveryTerms_ddlQuick']/option[@selected='selected']");
                            if (delOptions.Count == 1)
                            {
                                string _DelTerms = delOptions[0].GetAttributeValue("value", "").Trim();
                                dctPostDataValues.Add("UCDeliveryTerms%24ddlQuick", _DelTerms);
                            }

                            var payOptions = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='UCPaymentTerms_ddlQuick']/option[@selected='selected']");
                            if (payOptions.Count == 1)
                            {
                                string _PayTerms = payOptions[0].GetAttributeValue("value", "").Trim();
                                dctPostDataValues.Add("UCPaymentTerms%24ddlQuick", _PayTerms);
                            }
                            dctPostDataValues.Add("txtDeliveryTime%24txtNumber", _httpWrapper.GetElement("input", "id", "txtDeliveryTime_txtNumber").GetAttributeValue("value", "").Trim());

                            var ModeTransport = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='ucModeOfTransport_ddlQuick']/option[@selected='selected']");
                            if (ModeTransport.Count == 1)
                            {
                                string _mode = ModeTransport[0].GetAttributeValue("value", "").Trim();
                                dctPostDataValues.Add("ucModeOfTransport%24ddlQuick", _mode);
                            }
                            var PreferQlty = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='ddlPreferedquality_ddlHard']/option[@selected='selected']");
                            if (PreferQlty.Count == 1)
                            {
                                string _qlty = PreferQlty[0].GetAttributeValue("value", "").Trim();
                                dctPostDataValues.Add("ddlPreferedquality%24ddlHard", _qlty);
                            }
                            var Currency = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='ucCurrency_ddlCurrency']/option[@selected='selected']");
                            if (Currency.Count == 1)
                            {
                                string _curr = Currency[0].GetAttributeValue("value", "").Trim();
                                dctPostDataValues.Add("ucCurrency%24ddlCurrency", _curr);
                            }
                            dctPostDataValues.Add("txtSupplierDiscount%24txtNumber", _httpWrapper.GetElement("input", "id", "txtSupplierDiscount_txtNumber").GetAttributeValue("value", "").Trim());
                            dctPostDataValues.Add("txtPrice", _httpWrapper.GetElement("input", "id", "txtPrice").GetAttributeValue("value", "").Trim());
                            dctPostDataValues.Add("txtDiscount%24txtNumber", _httpWrapper.GetElement("input", "id", "txtDiscount_txtNumber").GetAttributeValue("value", "").Trim());
                            dctPostDataValues.Add("txtTotalPrice", _httpWrapper.GetElement("input", "id", "txtTotalPrice").GetAttributeValue("value", "").Trim());
                            dctPostDataValues.Add("txtTotalDiscount", _httpWrapper.GetElement("input", "id", "txtTotalDiscount").GetAttributeValue("value", "").Trim());
                            dctPostDataValues.Add("isouterpage", "");
                            dctPostDataValues.Add("txtnopage", "");
                            if (PostURL("a", "id", "MenuQuotationLineItem_dlstTabs_ctl00_btnMenu"))
                            {
                                bool Res = false;

                                List<HtmlNode> lstScripts = _httpWrapper._CurrentDocument.DocumentNode.Descendants()
                        .Where(n => n.Name == "script")
                        .ToList();
                                foreach (HtmlNode a in lstScripts)
                                {
                                    if (a.InnerText.Trim().Contains("javascript:parent.Openpopup") && a.InnerText.Contains("PurchaseFormDetail.aspx?orderid"))
                                    {
                                        int startIdx = a.InnerText.Trim().IndexOf("PurchaseFormDetail.aspx?") + 2;
                                        int _length = (a.InnerText.Trim().Length - 6) - startIdx;
                                        URL = a.InnerText.ToString().Substring(startIdx, _length).Trim();
                                        Res = true;
                                        break;
                                    }
                                }

                                URL = "https://apps2.southnests.com/ArchangelReports/Purchase/" + URL;
                                if (Res && LoadURL("input", "id", "_content_txtFormDetails_ucCustomEditor_ctl02", true))
                                {
                                    HeaderDetails = _httpWrapper.GetElement("input", "id", "_content_txtFormDetails_ucCustomEditor_ctl02").GetAttributeValue("value", "").Trim();
                                }
                            }
                        }
                    }
                    #endregion

                    #region Get Delivery Instruction

                    if (dctPostDataValues.ContainsKey("__EVENTTARGET"))
                        dctPostDataValues["__EVENTTARGET"] = "MenuQuotationLineItem%24dlstTabs%24ctl04%24btnMenu";
                    else
                        dctPostDataValues.Add("__EVENTTARGET", "MenuQuotationLineItem%24dlstTabs%24ctl04%24btnMenu");

                    URL = currentURL;
                    if (PostURL("a", "id", "MenuQuotationLineItem_dlstTabs_ctl04_btnMenu"))
                    {
                        bool result = false;
                        List<HtmlNode> lstScripts = _httpWrapper._CurrentDocument.DocumentNode.Descendants()
                         .Where(n => n.Name == "script")
                         .ToList();
                        foreach (HtmlNode a in lstScripts)
                        {
                            if (a.InnerText.Trim().Contains("javascript:parent.Openpopup") && a.InnerText.Contains("PurchaseQuotationDeliveryInstruction.aspx?QUOTATIONID"))
                            {
                                int startIdx = a.InnerText.Trim().IndexOf("PurchaseQuotationDeliveryInstruction") + 2;
                                int _length = (a.InnerText.Trim().Length - 6) - startIdx;
                                URL = a.InnerText.ToString().Substring(startIdx, _length).Trim();
                                result = true;
                                break;
                            }
                        }
                        if (result)
                        {
                            URL = "https://apps2.southnests.com/ArchangelReports/Purchase/" + URL;
                            if (LoadURL("textarea", "id", "txtDeliveryInstruction"))
                            {
                                HeaderDetails += _httpWrapper.GetElement("textarea", "id", "txtDeliveryInstruction").InnerText.Trim();
                            }
                            else LogText = "Unable to open delivery instruction tab.";
                        }
                    }
                    #endregion

                    HeaderDetails = HeaderDetails.Replace(@"&amp;", "&");
                    HeaderDetails = HeaderDetails.Replace(@"&lt;", "<");
                    HeaderDetails = HeaderDetails.Replace(@"&gt;", ">");
                    HeaderDetails = HeaderDetails.Replace(@"&quot;", "\"");
                    HeaderDetails = HeaderDetails.Replace(@"&apos;", "'");
                    HeaderDetails = HeaderDetails.Replace(@"&amp;", "&");

                    if (HeaderDetails.Contains("<!--"))
                        HeaderDetails = Regex.Replace(HeaderDetails, "<!--.*?-->", String.Empty, RegexOptions.Multiline);

                    while (HeaderDetails.Trim().Contains("&"))
                    {
                        if (HeaderDetails.Trim().Contains("&") && !HeaderDetails.Trim().Contains(" & "))
                        {
                            int count = HeaderDetails.Count(f => f == '&');

                            HeaderDetails = HttpUtility.HtmlDecode(HeaderDetails.Trim());
                            if (count == 1) break;
                        }
                        else break;
                    }
                    _xmlHeader.Add("HEADER_REMARKS", HeaderDetails.Trim());

                    if (currentURL != "") this.URL = currentURL;
                    if (LoadURL("input", "id", "txtnopage", true))
                    {
                        this.VRNO = _xmlHeader["VRNO"];
                        this.VRNO = this.VRNO.Replace("Quotation Details", "").Trim();
                        this.VRNO = this.VRNO.TrimStart('[').TrimEnd(']').Trim();
                        _xmlHeader["VRNO"] = this.VRNO;
                    }
                }

                #region set buyer code
                Uri _url = new Uri(this.URL);
                string Host = _url.Host;
                if (lstBuyer.ContainsKey(Host))
                {
                    string[] byrInfo = lstBuyer[Host].Trim().Split(',');
                    BuyerCode = convert.ToString(byrInfo[0]).Trim();
                    if (byrInfo.Length > 1) BuyerName = convert.ToString(byrInfo[1]);
                }
                #endregion

                #region set supplier code
                string _vemail = convert.ToString(_xmlHeader["VENDOR_EMAIL"]);
                if (_vemail.Trim().Length > 0)
                {
                    int indx = -1;
                    if (_vemail.Trim().Contains(',')) indx = _vemail.Trim().Split(',')[0].Trim().IndexOf('@');
                    else if (_vemail.Trim().Contains(';')) indx = _vemail.Trim().Split(';')[0].Trim().IndexOf('@');
                    else indx = _vemail.Trim().Split(',')[0].Trim().IndexOf('@');
                    if (indx > 0)
                    {
                        string domain = "";
                        if (_vemail.Trim().Contains(',')) domain = _vemail.Trim().Split(',')[0].Substring(indx).Trim();
                        else if (_vemail.Trim().Contains(';')) domain = _vemail.Trim().Split(';')[0].Substring(indx).Trim();
                        else domain = _vemail.Trim().Split(',')[0].Substring(indx).Trim();

                        if (lstSupp.ContainsKey(domain.Trim().ToLower()))
                        {
                            SupplierCode = convert.ToString(lstSupp[domain.Trim().ToLower()]);
                        }

                        #region to decide actions
                        if (ConfigurationManager.AppSettings["ACTIONS"].Trim() != null)
                        {
                            string[] lstActions = ConfigurationManager.AppSettings["ACTIONS"].Trim().Split('|');
                            if (lstActions != null)
                            {
                                foreach (string strActions in lstActions)
                                {
                                    if (strActions.Trim().Contains(SupplierCode))
                                    {
                                        if (strActions.Split('=')[1].Contains(","))
                                        {
                                            if (strActions.Split('=')[1].Split(',')[0].ToUpper().Trim() == "RFQ")
                                                IsRFQ = true;
                                            if (strActions.Split('=')[1].Split(',')[1].ToUpper().Trim() == "PO")
                                                IsPO = true;
                                        }
                                        else
                                        {
                                            if (strActions.Split('=')[1].ToUpper().Trim() == "RFQ")
                                                IsRFQ = true;
                                            if (strActions.Split('=')[1].ToUpper().Trim() == "PO")
                                                IsPO = true;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    errLog = "Unable to get supplier code.";
                    throw new Exception("Unable to get supplier code.");
                }
                #endregion

                if (!IsRFQ && !IsPO) _xmlHeader = _xmlHeader = new Dictionary<string, string>();
                return _xmlHeader;
            }
            catch (Exception ex)
            {
                LogText = "Error while reading " + this.DocType + " header details - " + ex.Message;
                if (ex.Message.Contains("Timeout while waiting")) moveToError = false;
                throw ex;
            }
        }

        public List<List<string>> GetItemsData(Dictionary<string, string> _xmlHeader)
        {
            List<List<string>> lstItems = new List<List<string>>();
            try
            {
                LogText = "Reading Item Details ..";
                if (this.DocType == "RFQ")
                {
                    if (IsRFQ)
                        lstItems = GetRFQItems(_xmlHeader);
                }
            }
            catch (Exception ex)
            {
                LogText = "Error while reading items - " + ex.StackTrace;
                throw ex;
            }
            return lstItems;
        }

        private HtmlAgilityPack.HtmlDocument GetNextPageItems()
        {
            HtmlAgilityPack.HtmlDocument _htmlDoc = null;
            {
                string cEventArguement = (!string.IsNullOrEmpty(cEventArg)) ? cEventArg : _httpWrapper._dctStateData["__EVENTARGUMENT"];
                string cViewStatedet = (!string.IsNullOrEmpty(cViewState)) ? cViewState : _httpWrapper._dctStateData["__VIEWSTATE"];
                string cViewStateGendet = (!string.IsNullOrEmpty(cViewStateGen)) ? cViewStateGen : _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"];
                dctPostDataValues.Clear();
                dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", "");
                dctPostDataValues.Add("__EVENTTARGET", "cmdNext");
                dctPostDataValues.Add("__EVENTARGUMENT", cEventArguement);
                dctPostDataValues.Add("__LASTFOCUS", "");
                dctPostDataValues.Add("__VIEWSTATE", cViewStatedet);
                dctPostDataValues.Add("__VIEWSTATEGENERATOR", cViewStateGendet);
                dctPostDataValues.Add("__SCROLLPOSITIONX", "0");
                dctPostDataValues.Add("__SCROLLPOSITIONY", "3678");
                if (_httpWrapper._dctStateData.ContainsKey("__PREVIOUSPAGE"))
                {
                    dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);
                }
                else
                {
                    if (!string.IsNullOrEmpty(cPrevPageData))
                    {
                        dctPostDataValues.Add("__PREVIOUSPAGE", cPrevPageData);
                    }
                }
                dctPostDataValues.Add("isouterpage", "");
                dctPostDataValues.Add("txtnopage", "");

                _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
                _httpWrapper._SetRequestHeaders[HttpRequestHeader.AcceptLanguage] = "en-IN";
                _httpWrapper._SetRequestHeaders[HttpRequestHeader.CacheControl] = "no-cache";

                if (PostURL("input", "id", "txtEnquiryNumber"))
                {
                    _htmlDoc = _httpWrapper._CurrentDocument;
                }
            }
            return _htmlDoc;
        }

        private List<List<string>> GetRFQItems(Dictionary<string, string> _xmlHeader)
        {
            List<List<string>> lstItems = new List<List<string>>(); List<List<string>> lstTempItems = new List<List<string>>(); 
            HtmlNodeCollection _tr = null; HtmlAgilityPack.HtmlDocument doc = null;
            lstItems.Clear(); lstTempItems.Clear();
            HtmlNode _currPage = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//a[@id='cmdPrevious']");
            if (_currPage != null)
            {
                string _disCurr = _currPage.GetAttributeValue("disabled", "").Trim();
                if (_disCurr == "disabled")
                {
                    _tr = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='gvVendorItem']//tr[@tabindex='-1']");
                    lstItems = GetItemDetails(_tr, _httpWrapper._CurrentDocument, lstTempItems);
                }
            }
           

            return lstItems;
        }

        private List<List<string>> GetItemDetails(HtmlNodeCollection _tr, HtmlAgilityPack.HtmlDocument doc, List<List<string>> lstItems)
        {
             _httpWrapper._CurrentDocument = doc;
            if (_tr != null)
            {
                if (_tr.Count > 0)
                {
                    int Counter = 03, itemCount = 0;
                    string strCounter = "";
                    if (Counter < _tr.Count + 1) strCounter = "0" + Counter;
                    else if (Counter > 10) strCounter = Counter.ToString();
                    string _i = "gvVendorItem_ctl" + strCounter + "_lblSNo";
                    HtmlNode itemNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='" + _i + "']");

                    do
                    {
                        if (itemNo != null)
                        {
                            string RefNo = "";
                            itemCount++;
                            string _gst = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//input[@id='gvVendorItem_ctl" + strCounter + "_chkIncludeGST']").GetAttributeValue("checked", "").Trim();
                            if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblMakerReference']") != null)
                                RefNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblMakerReference']").InnerText.Trim();

                            string Descr = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//a[@id='gvVendorItem_ctl" + strCounter + "_lnkStockItemCode']").InnerText.Trim();

                            string Qty = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblOrderQuantity']").InnerText.Trim();
                            string Unit = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblCustomerUnit']").InnerText.Trim();
                            string Brand = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblBrand']").InnerText.Trim();

                            # region /* item remarks */
                            string ItemRemarks = "";
                            HtmlNode _details = _httpWrapper.GetElement("input", "id", "gvVendorItem_ctl" + strCounter + "_cmdDetails");
                            if (_details != null)
                            {
                                string _attDis = _details.GetAttributeValue("disabled", "");
                                if (_attDis == "")
                                {
                                    string[] _onclick = _details.GetAttributeValue("onclick", "").Split(',');
                                    if (_onclick.Length == 3)
                                    {
                                        URL = "https://apps2.southnests.com/ArchangelReports/Purchase/" + HttpUtility.HtmlDecode(_onclick[2].Split('\'')[1]);
                                        if (LoadURL("input", "id", "_content_txtItemDetails_ucCustomEditor_ctl02", true))
                                        {
                                            ItemRemarks = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//input[@id='_content_txtItemDetails_ucCustomEditor_ctl02']").GetAttributeValue("value", "").Trim();
                                            ItemRemarks = ItemRemarks.Replace(@"&amp;", "&");
                                            ItemRemarks = ItemRemarks.Replace(@"&lt;", "<");
                                            ItemRemarks = ItemRemarks.Replace(@"&gt;", ">");
                                            ItemRemarks = ItemRemarks.Replace(@"&quot;", "\"");
                                            ItemRemarks = ItemRemarks.Replace(@"&apos;", "'");
                                            ItemRemarks = ItemRemarks.Replace(@"&amp;", "&");

                                            while (ItemRemarks.Trim().Contains("&"))
                                            {
                                                if (ItemRemarks.Trim().Contains("&") && !ItemRemarks.Trim().Contains(" & "))
                                                {
                                                    ItemRemarks = HttpUtility.HtmlDecode(ItemRemarks.Trim());
                                                }
                                                else break;
                                            }

                                            if (ItemRemarks.Contains("<br />")) ItemRemarks = ItemRemarks.Replace("<br />", " ");
                                            if (ItemRemarks.Contains("<!--"))
                                                ItemRemarks = Regex.Replace(ItemRemarks, "<!--.*?-->", String.Empty, RegexOptions.Multiline);

                                            HtmlAgilityPack.HtmlDocument doc1 = new HtmlAgilityPack.HtmlDocument();
                                            doc1.LoadHtml(ItemRemarks);
                                            string data = convert.ToString(doc1.DocumentNode.InnerText);

                                            ItemRemarks = data.Trim();
                                        }
                                    }
                                }
                            }//
                            #endregion

                            //   URL = currentURL;
                            //  if (LoadURL("input", "id", "txtnopage", true))
                            {

                                if (_gst.ToUpper() == "CHECKED")
                                    ItemRemarks += Environment.NewLine + "Included GST : Yes";
                                else ItemRemarks += Environment.NewLine + "Included GST : No";

                                if (Brand != "")
                                    ItemRemarks += Environment.NewLine + "Brand : " + Brand;

                                List<string> item = new List<string>();
                                item.Add(convert.ToString(itemNo.InnerText.Trim()));
                                item.Add(RefNo);
                                item.Add(Descr);
                                item.Add(Qty);
                                item.Add(Unit);
                                item.Add(ItemRemarks);
                                lstItems.Add(item);
                                Counter++;
                            }
                        }

                        int _totalPages = 0; string _totalItems = "";
                        if (Counter.ToString().Length == 1) strCounter = "0" + Counter;
                        else strCounter = Counter.ToString();

                        if (itemCount >= _tr.Count)
                        {
                            // Check Total Item Count
                            if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='lblRecords']") != null)
                            {
                                _totalItems = convert.ToString(_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='lblRecords']").InnerText.Trim()).Trim().Replace("(", "").Replace(")", "").Replace("records found", "").Trim();
                                _totalPages = convert.ToInt(convert.ToString(_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='lblPages']").InnerText.Trim()).Trim().Replace("of", "").Replace("Pages.", "").Trim());

                                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsItemCountMismatch"].Trim()) == false)
                                {
                                    if (convert.ToInt(_totalItems) == lstItems.Count)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    if (_tr.Count == lstItems.Count)
                                    {
                                        break;
                                    }
                                }
                            }

                            if (_totalPages == 1 && (convert.ToInt(_totalItems) != lstItems.Count))
                            {
                                if (itemCount != (_tr.Count - 1))
                                {
                                    throw new Exception("Total Item count not matching with items in grid");
                                }
                                else break;
                            }
                            else
                            {
                                HtmlNode _nextPage = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//a[@id='cmdNext']");
                                if (_nextPage != null)
                                {
                                    string _disNext = _nextPage.GetAttributeValue("disabled", "").Trim();
                                    if (_disNext != "disabled")
                                    {
                                        doc = GetNextPageItems();
                                        _tr = doc.DocumentNode.SelectNodes("//table[@id='gvVendorItem']//tr[@tabindex='-1']");
                                        //lstItems.AddRange();
                                        GetItemDetails(_tr, doc, lstItems);
                                    }
                                }
                            }
                            // Reset Counter
                            Counter = 02; itemCount = 0;
                            if (Counter.ToString().Length == 1) strCounter = "0" + Counter;
                            else strCounter = Counter.ToString();
                        }
                        itemNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblSNo']");
                    }
                    while (itemNo != null);
                }
            }
            return lstItems;
        }

        private HtmlAgilityPack.HtmlDocument GetnextPageItems(int nxt)
        {
            HtmlAgilityPack.HtmlDocument _htmlDoc = null;
            if (nxt == 1)
            {         
                string cEventArguement = (!string.IsNullOrEmpty(cEventArg)) ? cEventArg : _httpWrapper._dctStateData["__EVENTARGUMENT"];
                string cViewStatedet = (!string.IsNullOrEmpty(cViewState)) ? cViewState : _httpWrapper._dctStateData["__VIEWSTATE"];
                string cViewStateGendet = (!string.IsNullOrEmpty(cViewStateGen)) ? cViewStateGen : _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"];            

                dctPostDataValues.Clear();
                dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", "");
                dctPostDataValues.Add("__EVENTTARGET", "cmdNext");
                dctPostDataValues.Add("__EVENTARGUMENT", cEventArguement);
                dctPostDataValues.Add("__LASTFOCUS", "");
                dctPostDataValues.Add("__VIEWSTATE", cViewStatedet);
                dctPostDataValues.Add("__VIEWSTATEGENERATOR", cViewStateGendet);
                dctPostDataValues.Add("__SCROLLPOSITIONX", "0");
                dctPostDataValues.Add("__SCROLLPOSITIONY", "3678");

                if (_httpWrapper._dctStateData.ContainsKey("__PREVIOUSPAGE"))
                {
                    dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);
                }
                else
                {
                    if (!string.IsNullOrEmpty(cPrevPageData))
                    {
                        dctPostDataValues.Add("__PREVIOUSPAGE", cPrevPageData);
                    }
                }
                dctPostDataValues.Add("isouterpage", "");
                dctPostDataValues.Add("txtnopage", "");

                _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
                _httpWrapper._SetRequestHeaders[HttpRequestHeader.AcceptLanguage] = "en-IN";
                _httpWrapper._SetRequestHeaders[HttpRequestHeader.CacheControl] = "no-cache";
              
                if (PostURL("input", "id", "txtEnquiryNumber"))
                {
                    _htmlDoc = _nxtDoc= _httpWrapper._CurrentDocument;
                }
            }
            return _htmlDoc;
        }

        public void ExportToLESML(Dictionary<string, string> _xmlHeader, List<List<string>> _xmlItems, string filename)
        {
            LeSXML.LeSXML _lesXML = new LeSXML.LeSXML();
            try
            {
                ExporttoHeader(_xmlHeader, _lesXML);
                ExporttoItems(_xmlItems, _lesXML);

                bool attachmentFound = false;
                if (File.Exists(this.ImagePath + @"\" + _lesXML.OrigDocFile)) attachmentFound = true;
                else if (File.Exists(this.ImagePath + @"\Backup\" + _lesXML.OrigDocFile)) attachmentFound = true;
                else attachmentFound = false;


                if (!attachmentFound)
                {
                    _lesXML.OrigDocFile = Path.GetFileName(filename);
                }

                _lesXML.WriteXML();

                if (File.Exists(_lesXML.FilePath + "\\" + _lesXML.FileName))
                {
                    LogText = _lesXML.Doc_Type + " '" + _lesXML.BuyerRef + "' downloaded successfully.";
                    CreateAuditFile(Path.GetFileName(_lesXML.FileName), "ArcMarine_Http_" + _lesXML.Doc_Type, _lesXML.BuyerRef, "Downloaded", _lesXML.Doc_Type + " '" + _lesXML.BuyerRef + "' downloaded successfully.", BuyerCode, SupplierCode, AuditPath);
                }
            }
            catch (Exception ex)
            {
                CreateAuditFile("", "ArcMarine_Http_" + _lesXML.Doc_Type, _lesXML.BuyerRef, "Error", "Unable to generate xml for " + DocType + " '" + _lesXML.BuyerRef + "'", BuyerCode, SupplierCode, AuditPath);
                LogText = "Unable to generate xml for " + DocType + " '" + _lesXML.BuyerRef + "'";
                throw ex;
            }
        }

        public void ExporttoHeader(Dictionary<string, string> _xmlHeader, LeSXML.LeSXML _lesXML)
        {
            try
            {
                this.VRNO = _xmlHeader["VRNO"].Trim();
                _lesXML.Active = "1";
                _lesXML.Doc_Type = this.DocType;
                _lesXML.BuyerRef = _xmlHeader["VRNO"].Trim();
                if (_xmlHeader.ContainsKey("PORT_NAME")) _lesXML.PortName = _xmlHeader["PORT_NAME"].Trim();

                _lesXML.Recipient_Code = SupplierCode;
                _lesXML.Sender_Code = BuyerCode;

                _lesXML.FileName = this.DocType + "_" + convert.ToFileName(this.VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xml";

                string BuyerRemarks = "";
                if (_xmlHeader.ContainsKey("VESSEL_TYPE")) BuyerRemarks += "Vessel Type : " + _xmlHeader["VESSEL_TYPE"].Trim();
                if (_xmlHeader.ContainsKey("REMARKS")) BuyerRemarks += Environment.NewLine + _xmlHeader["REMARKS"].Trim();
                if (_xmlHeader.ContainsKey("HEADER_REMARKS")) BuyerRemarks += Environment.NewLine + Environment.NewLine + _xmlHeader["HEADER_REMARKS"].Trim();
                string BlanksLines = Environment.NewLine + Environment.NewLine;
                while (BuyerRemarks.Contains(BlanksLines)) BuyerRemarks = BuyerRemarks.Replace(BlanksLines, Environment.NewLine);
                if (BuyerRemarks.Contains("<br />")) BuyerRemarks = BuyerRemarks.Replace("<br />", " ");
                _lesXML.Remark_Sender = BuyerRemarks.Trim();

                GetAddressDetails(ref _lesXML, _xmlHeader);



                string MsgNumber = "";
                if (this.DocType == "RFQ")
                    MsgNumber = this.URL;
                _lesXML.DocLinkID = MsgNumber; // MessageNumber in eSupplier
                _lesXML.DocReferenceID = MsgNumber; // MessageReferenceNumber in eSupplier

                if (this.DocType == "RFQ")
                {
                    // Dates                 
                    _lesXML.Date_Document = GetDate(_xmlHeader["RFQ_DATE"].Trim());  /* RFQ Date */
                }
                currentXMLFile = _lesXML.FilePath + "\\" + _lesXML.FileName;
            }
            catch (Exception ex)
            {
                LogText = "Error in ExporttoHeader() : " + ex.Message + Environment.NewLine + ex.StackTrace;
                throw ex;
            }
        }

        public void ExporttoItems(List<List<string>> _xmlItems, LeSXML.LeSXML _lesXML)
        {
            try
            {
                if (this.DocType == "RFQ")
                {
                    foreach (List<string> _lItem in _xmlItems)
                    {
                        LeSXML.LineItem _item = new LeSXML.LineItem();
                        _item.Number = _lItem[0];
                        _item.ItemRef = _lItem[1].Trim();
                        _item.Name = _lItem[2];
                        _item.Quantity = _lItem[3];
                        _item.Unit = _lItem[4];
                        _item.Remark = convert.ToString(_lItem[5]).Trim();
                        _item.OriginatingSystemRef = _lItem[0];
                        _lesXML.LineItems.Add(_item);
                    }
                }
                _lesXML.Total_LineItems = _lesXML.LineItems.Count.ToString();
            }
            catch (Exception ex)
            {
                LogText = "Error in ExporttoItems() : " + ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }

        private string GetDate(string DateValue)
        {
            if (DateValue.Trim().Length > 0)
            {
                string _dtValue = "";
                DateTime dt = DateTime.MinValue;
                DateTime.TryParseExact(DateValue.Trim(), "d/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dt);
                if (dt == DateTime.MinValue) DateTime.TryParseExact(DateValue, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dt);
                if (dt == DateTime.MinValue) DateTime.TryParseExact(DateValue, "dd/MMM/yyyy", null, System.Globalization.DateTimeStyles.None, out dt);
                if (dt == DateTime.MinValue) DateTime.TryParseExact(DateValue, "d/MMM/yyyy", null, System.Globalization.DateTimeStyles.None, out dt);
                if (dt == DateTime.MinValue) DateTime.TryParseExact(DateValue, "d/M/yyyy", null, System.Globalization.DateTimeStyles.None, out dt);
                if (dt != DateTime.MinValue) _dtValue = dt.ToString("yyyyMMdd");
                return _dtValue.Trim();
            }
            else return "";
        }

        public void GetAddressDetails(ref LeSXML.LeSXML _lesXML, Dictionary<string, string> _xmlHeader)
        {
            // Buyer Address
            _lesXML.Addresses.Add(new LeSXML.Address());
            _lesXML.Addresses[0].Qualifier = "BY";
            _lesXML.Addresses[0].AddressName = BuyerName.Trim();
            if (_xmlHeader.ContainsKey("BUYER_ADDR")) _lesXML.Addresses[0].Address1 = _xmlHeader["BUYER_ADDR"].Trim();
            if (_xmlHeader.ContainsKey("BUYER_CONTACT")) _lesXML.Addresses[0].ContactPerson = _xmlHeader["BUYER_CONTACT"].Trim();
            if (_xmlHeader.ContainsKey("BUYER_PHONE")) _lesXML.Addresses[0].Phone = _xmlHeader["BUYER_PHONE"].Trim();
            if (_xmlHeader.ContainsKey("BUYER_EMAIL")) _lesXML.Addresses[0].eMail = _xmlHeader["BUYER_EMAIL"].Trim();

            // Vendor Address
            _lesXML.Addresses.Add(new LeSXML.Address());
            _lesXML.Addresses[1].Qualifier = "VN";
            _lesXML.Addresses[1].AddressName = _xmlHeader["VENDOR_NAME"].Trim();
            if (_xmlHeader.ContainsKey("VENDOR_ADDRESS1")) _lesXML.Addresses[1].Address1 = _xmlHeader["VENDOR_ADDRESS1"].Trim();
            if (_xmlHeader.ContainsKey("VENDOR_PHONE")) _lesXML.Addresses[1].Phone = _xmlHeader["VENDOR_PHONE"].Trim();
            if (_xmlHeader.ContainsKey("VENDOR_EMAIL")) _lesXML.Addresses[1].eMail = _xmlHeader["VENDOR_EMAIL"].Trim();

            // Shipping Address
            _lesXML.Addresses.Add(new LeSXML.Address());
            _lesXML.Addresses[2].Qualifier = "CN";
            if (_xmlHeader.ContainsKey("SHIP_ADDRESS1")) _lesXML.Addresses[2].Address1 = _xmlHeader["SHIP_ADDRESS1"].Trim();
        }

        public void Logout()
        { }

        private void SetBuyerSupplierCodes()
        {
            try
            {
                string[] _bvalues = convert.ToString(ConfigurationManager.AppSettings["BUYER_CODE"]).Trim().Split('|');
                string[] _svalues = convert.ToString(ConfigurationManager.AppSettings["SUPP_CODE"]).Trim().Split('|');

                foreach (string sBuyer in _bvalues)
                {
                    if (sBuyer.Trim() != "")
                    {
                        string[] arr = sBuyer.Trim().Split('=');
                        if (arr.Length > 1)
                        {
                            lstBuyer.Add(arr[0], arr[1]);
                        }
                    }
                }

                foreach (string sSupp in _svalues)
                {
                    if (sSupp.Trim() != "")
                    {
                        string[] arr = sSupp.Trim().Split('=');
                        if (arr.Length > 1)
                        {
                            lstSupp.Add(arr[0].Trim().ToLower(), arr[1]);
                        }
                    }
                }
            }
            catch { }
        }

        private string GetURL(string emlFile)
        {
            string URL = "";
            try
            {
                int searchCriteria = 0;
                URL = File.ReadAllText(emlFile);
                RichTextBox txt = new RichTextBox();
                txt.Text = URL;
                if (txt.Lines.Length > 1)
                {
                    FileType = "msg";
                    txt.Text = txt.Text.Replace("<br/>", Environment.NewLine);
                    int urlIdx = txt.Text.IndexOf("\"https");
                    if (urlIdx == -1)
                    {
                        foreach (KeyValuePair<string, string> pair in lstBuyer)
                        {
                            urlIdx = txt.Text.IndexOf("https://" + pair.Key);
                            if (urlIdx == -1) { }
                            if (urlIdx > 0) { urlIdx = urlIdx - 1; searchCriteria = 2; break; }
                        }
                    }
                    else searchCriteria = 1;
                    if (urlIdx > 0)
                    {
                        string part1 = txt.Text.Substring(urlIdx + 1);
                        int endUrlIndx = -1;
                        if (searchCriteria == 1) endUrlIndx = part1.Trim().IndexOf("\"");
                        else if (searchCriteria == 2) endUrlIndx = part1.Trim().IndexOf(">\"");

                        if (endUrlIndx > 0)
                        {
                            string orgURL = part1.Substring(0, endUrlIndx);
                            URL = orgURL.Trim('"').Trim().Replace("&amp;", "&");
                        }
                        else
                        {
                            endUrlIndx = part1.Trim().IndexOf("SPARE");
                            if (endUrlIndx > 0)
                            {
                                string orgURL = part1.Substring(0, endUrlIndx + 5);
                                URL = orgURL.Trim('"').Trim().Replace("&amp;", "&");
                            }
                            else
                            {
                                foreach (string line in txt.Lines)
                                {
                                    if (line.Trim().StartsWith("https://") && !line.Trim().Contains("PurchaseVendorAddressEdit"))
                                    {
                                        URL = line.Trim().Replace("<br>", "");
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        URL = "";
                    }
                }

                URL = URL.Trim().TrimStart('<').TrimEnd('>');

                if (!URL.Trim().StartsWith("https"))
                {
                    foreach (string line in txt.Lines)
                    {
                        if (line.Trim().StartsWith("\"<https"))
                        {
                            URL = line.Trim().Trim('"').Trim().TrimStart('<').TrimEnd('>').Trim();
                        }
                    }
                }

                URL = URL.Replace("\r\n", "");
            }
            catch (Exception ex)
            {
                LogText = "Error GetURL() : " + ex.Message;
            }
            return URL;
        }

        private string GetURLWithHttp(string emlFile)
        {
            string URL = "";
            try
            {
                int searchCriteria = 0;
                URL = File.ReadAllText(emlFile);
                RichTextBox txt = new RichTextBox();
                txt.Text = URL;
                if (txt.Lines.Length > 1)
                {
                    FileType = "msg";
                    txt.Text = txt.Text.Replace("<br/>", Environment.NewLine);
                    int urlIdx = txt.Text.IndexOf("\"http");
                    if (urlIdx == -1)
                    {
                        foreach (KeyValuePair<string, string> pair in lstBuyer)
                        {
                            urlIdx = txt.Text.IndexOf("http://" + pair.Key);
                            if (urlIdx == -1) { }
                            if (urlIdx > 0) { urlIdx = urlIdx - 1; searchCriteria = 2; break; }
                        }
                    }
                    else searchCriteria = 1;
                    if (urlIdx > 0)
                    {
                        string part1 = txt.Text.Substring(urlIdx + 1);
                        int endUrlIndx = -1;
                        if (searchCriteria == 1) endUrlIndx = part1.Trim().IndexOf("\"");
                        else if (searchCriteria == 2) endUrlIndx = part1.Trim().IndexOf(">\"");

                        if (endUrlIndx > 0)
                        {
                            string orgURL = part1.Substring(0, endUrlIndx);
                            URL = orgURL.Trim('"').Trim().Replace("&amp;", "&");
                        }
                        else
                        {
                            endUrlIndx = part1.Trim().IndexOf("SPARE");
                            if (endUrlIndx > 0)
                            {
                                string orgURL = part1.Substring(0, endUrlIndx + 5);
                                URL = orgURL.Trim('"').Trim().Replace("&amp;", "&");
                            }
                            else
                            {
                                foreach (string line in txt.Lines)
                                {
                                    if (line.Trim().StartsWith("http://") && !line.Trim().Contains("PurchaseVendorAddressEdit"))
                                    {
                                        URL = line.Trim();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        URL = "";
                    }
                }
            }
            catch (Exception ex)
            {
                LogText = "Error GetURLWithHttp() : " + ex.Message;
            }
            return URL;
        }

        public override bool PrintScreen(string sFileName)
        {
            if (!Directory.Exists(Path.GetDirectoryName(sFileName) + "\\Temp")) Directory.CreateDirectory(Path.GetDirectoryName(sFileName) + "\\Temp");
            sFileName = Path.GetDirectoryName(sFileName) + "\\Temp\\" + Path.GetFileName(sFileName);
            if (base.PrintScreen(sFileName))
            {
                MoveFiles(sFileName, ImagePath + "\\" + Path.GetFileName(sFileName));
                return (File.Exists(ImagePath + "\\" + Path.GetFileName(sFileName)));
            }
            else return false;
        }

        private void MoveFile(string DestinationPath, FileInfo fileinfo)
        {
            try
            {
                if (DestinationPath.Trim().Length > 0)
                {
                    if (!Directory.Exists(DestinationPath.Trim())) Directory.CreateDirectory(DestinationPath.Trim());

                    string _newFile = (DestinationPath + "\\" + fileinfo.Name).Trim();
                    if (File.Exists(_newFile)) File.Delete(_newFile);
                    File.Move(fileinfo.FullName, _newFile);
                    LogText = fileinfo.Name + " File moved to " + Path.GetFileName(DestinationPath);
                    LogText = "";
                }
                else
                {
                    errLog = "Destination path is blank.";
                    throw new Exception("Destination path is blank.");
                }
            }
            catch (Exception ex)
            {
                LogText = "Error in MoveFile() - " + ex.Message;
            }
        }

        #region Quote

        public void ProcessQuote()
        {
            int j = 0;
            VRNO = "";
            try
            {
                LogText = "";
                GetXmlFiles();
                if (xmlFiles.Count > 0)
                {
                    for (j = xmlFiles.Count - 1; j >= 0; j--)
                    {
                        MTMLClass _mtml = new MTMLClass();
                        _interchange = _mtml.Load(xmlFiles[j]);
                        LoadInterchangeDetails();
                        if (this.DocType.ToUpper() == "QUOTE" || this.DocType.ToUpper() == "QUOTATION")
                        {
                            LogText = "Quote processing started.";
                            ProcessQuoteMTML(xmlFiles[j]);
                            slnxtPages.Clear();
                        }
                    }
                }
                else LogText = "No quotes found.";
            }
            catch (Exception ex)
            {
                LogText = "Exception in Process Quote : " + ex.GetBaseException().Message.ToString();
            }
        }

        public void ProcessQuoteMTML(string MTMLFile)
        {
            double FinalAmt = 0;
            try
            {
                if (UCRefNo != "")
                {
                    LogText = "Quote processing started for refno: " + UCRefNo;
                    URL =cQuoteURL= MessageNumber;
                    if (LoadURL("span", "id", "Title1_lblTitle"))
                    {
                        HtmlNode _title = _httpWrapper.GetElement("span", "id", "Title1_lblTitle");
                        HtmlNode _spntitle = _httpWrapper.GetElement("span", "id", "Title1_spnTitle");
                        HtmlNode _btnSave = _httpWrapper.GetElement("a", "id", "MenuQuotationLineItem_dlstTabs_ctl00_btnMenu");
                        if (_title != null && _btnSave != null)
                        {
                            string cTitle = _title.InnerText.Trim(); if (string.IsNullOrEmpty(cTitle)) { cTitle = _spntitle.InnerText.Trim(); }//28042020
                            if (cTitle.Contains(UCRefNo) && _btnSave.InnerText.Trim().ToUpper() == "SAVE")
                            {
                                if (MessageNumber.Contains("apps2.southnests.com")) fixedUrl = "https://apps2.southnests.com/";
                                else fixedUrl = "https://apps.southnests.com/";
                                LogText = "Filling Quotation.";
                                if (FillQuotation(true))
                                {
                                    try
                                    {
                                        double extraItemCost = 0;
                                        if (convert.ToInt(IsDecline) == 0)
                                        {
                                            //if (FillItems(out extraItemCost))
                                            if (FillItems_(out extraItemCost))
                                            {
                                                #region // Recheck Final Amount if additional discount or allowance is applied //
                                                if ((AddDiscount > 0 || convert.ToFloat(Allowance) > 0) && convert.ToFloat(GrandTotal) > 0)
                                                {
                                                    if (AddDiscount > 0)
                                                    {
                                                        FinalAmt = (convert.ToFloat(GrandTotal) + (AddDiscount * convert.ToFloat(GrandTotal) / 100));
                                                    }
                                                    else if (convert.ToFloat(Allowance) > 0)
                                                    {
                                                        FinalAmt = (convert.ToFloat(GrandTotal) + convert.ToFloat(Allowance));
                                                    }
                                                }
                                                #endregion
                                                if (convert.ToInt(IsDecline) == 0) // QUOTE IS NOT DECLINED //
                                                {
                                                    if (FinalAmt == 0) FinalAmt = convert.ToFloat(GrandTotal);
                                                    SendQuote(MTMLFile, FinalAmt, extraItemCost);
                                                }
                                                else//decline quote
                                                {
                                                    if (DeclineQuote(MTMLFile))
                                                    {
                                                        LogText = "Decline quote for ref no: " + UCRefNo;
                                                        CreateAuditFile("", "Arc_Http_" + this.DocType, UCRefNo, "Error", "Decline quote for ref no: " + UCRefNo, BuyerCode, SupplierCode, AuditPath);
                                                        if (!Directory.Exists(QuotePOCPath + "\\Backup")) Directory.CreateDirectory(QuotePOCPath + "\\Backup");
                                                        File.Move(QuotePOCPath + "\\" + Path.GetFileName(MTMLFile), QuotePOCPath + "\\Backup\\" + Path.GetFileName(MTMLFile));
                                                    }
                                                    else
                                                    {
                                                        LogText = "Unable to decline quote for ref no: " + UCRefNo;
                                                        CreateAuditFile("", "Arc_Http_" + this.DocType, UCRefNo, "Error", "Unable to decline quote for ref no: " + UCRefNo, BuyerCode, SupplierCode, AuditPath);
                                                        if (!Directory.Exists(QuotePOCPath + "\\Error")) Directory.CreateDirectory(QuotePOCPath + "\\Error");
                                                        File.Move(QuotePOCPath + "\\" + Path.GetFileName(MTMLFile), QuotePOCPath + "\\Error\\" + Path.GetFileName(MTMLFile));
                                                    }
                                                }
                                                _errorlog = "";
                                            }
                                            else
                                                if (_errorlog == "")
                                                    Write_Quote_ErrorLog("Unable to save item details for ref no: " + UCRefNo, MTMLFile);
                                                else
                                                    Write_Quote_ErrorLog(_errorlog + " for " + UCRefNo, MTMLFile);
                                        }
                                        else
                                        {
                                            if (DeclineQuote(MTMLFile))
                                            {
                                                LogText = "Decline quote for ref no: " + UCRefNo;
                                                CreateAuditFile("", "Arc_Http_" + this.DocType, UCRefNo, "Error", "Decline quote for ref no: " + UCRefNo, BuyerCode, SupplierCode, AuditPath);
                                                if (!Directory.Exists(QuotePOCPath + "\\Backup")) Directory.CreateDirectory(QuotePOCPath + "\\Backup");
                                                File.Move(QuotePOCPath + "\\" + Path.GetFileName(MTMLFile), QuotePOCPath + "\\Backup\\" + Path.GetFileName(MTMLFile));
                                            }
                                            else
                                            {
                                                LogText = "Unable to decline quote for ref no: " + UCRefNo;
                                                CreateAuditFile("", "Arc_Http_" + this.DocType, UCRefNo, "Error", "Unable to decline quote for ref no: " + UCRefNo, BuyerCode, SupplierCode, AuditPath);
                                                if (!Directory.Exists(QuotePOCPath + "\\Error")) Directory.CreateDirectory(QuotePOCPath + "\\Error");
                                                File.Move(QuotePOCPath + "\\" + Path.GetFileName(MTMLFile), QuotePOCPath + "\\Error\\" + Path.GetFileName(MTMLFile));
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Write_Quote_ErrorLog("Unable to save item details for ref no: " + UCRefNo + ", " + ex.GetBaseException().Message, MTMLFile);
                                    }
                                    //}
                                    //else
                                    //    Write_Quote_ErrorLog("Unable to save quote remarks for ref no: " + UCRefNo, MTMLFile);
                                    // }
                                }
                                else
                                    if (_errorlog == "")
                                        Write_Quote_ErrorLog("Unable to save header deatils for ref no: " + UCRefNo, MTMLFile);
                                    else Write_Quote_ErrorLog(_errorlog + " for " + UCRefNo, MTMLFile);
                            }
                            else
                            {
                                if (_btnSave.InnerText.Trim().ToUpper() != "SAVE")
                                {
                                    Write_Quote_ErrorLog("Quote for ref no: " + UCRefNo + " is already submitted.", MTMLFile);
                                }
                            }
                        }
                        else
                        {
                            if (_btnSave == null)
                                Write_Quote_ErrorLog("Unable to load quote details, Save button not found for ref no: " + UCRefNo, MTMLFile);
                            else { Write_Quote_ErrorLog("Unable to load quote details for ref no: " + UCRefNo, MTMLFile); }
                        }
                    }
                    else
                        Write_Quote_ErrorLog("Unable to load quote details for ref no: " + UCRefNo, MTMLFile);
                }
            }
            catch (Exception ex)
            {
                Write_Quote_ErrorLog("Exception while  processing Quote MTML: " + ex.GetBaseException().Message.ToString(), MTMLFile);
            }
        }

        public bool FillQuotation(bool IsNew)
        {
            bool result = false;
            try
            {
                LogText = "Quotation Header filling started";
                HtmlNode _txtVendorRef = _httpWrapper.GetElement("input", "id", "txtVenderReference");
                if (_txtVendorRef != null)
                {
                    HtmlNode _txtQuoteExp = _httpWrapper.GetElement("input", "id", "txtOrderDate");
                    if (_txtQuoteExp != null)
                    {
                        HtmlNode _txtDelDate = _httpWrapper.GetElement("input", "id", "txtDeliveryTime_txtNumber");
                        if (_txtDelDate != null)
                        {
                            dctPostDataValues.Clear();
                            #region Quotation valid till
                            if (dtExpDate == "")
                            {
                                string[] defaultQuoteExpDays = convert.ToString(ConfigurationManager.AppSettings["QUOTE_VALID"]).Trim().Split('|');
                                foreach (string sQuoteExpDays in defaultQuoteExpDays)
                                {
                                    string[] values = sQuoteExpDays.Split('=');
                                    if (values[0].Trim() == SupplierCode.Trim() && values.Length > 1 && convert.ToInt(values[1]) > 0)
                                    {
                                        dtExpDate = DateTime.Now.AddDays(convert.ToInt(values[1])).ToString("yyyyMMddHHmm");
                                        break;
                                    }
                                }
                            }
                            DateTime dtExpiry = DateTime.Now.AddDays(convert.ToFloat(LeadDays));
                            DateTime.TryParseExact(dtExpDate.Trim(), "yyyyMMddHHmm", null, System.Globalization.DateTimeStyles.None, out dtExpiry);
                            if (dtExpiry != DateTime.MinValue)
                            {
                                if (dtExpiry < DateTime.Now) dtExpiry = DateTime.Now;
                                dtExpDate = dtExpiry.ToString("dd/MMM/yyyy");
                            }
                            #endregion

                            #region Delivery Terms
                            string[] delTerms = convert.ToString(ConfigurationManager.AppSettings["DEL_TERMS"]).Trim().Split('|');
                            Dictionary<string, string> lstDelTerms = new Dictionary<string, string>();
                            foreach (string s in delTerms)
                            {
                                string[] strValues = s.Trim().Split('=');
                                lstDelTerms.Add(strValues[0], strValues[1]);
                            }

                            if (!lstDelTerms.ContainsKey(SupplierCode)) throw new Exception("No setting found for Delivery Terms of Vendor [" + SupplierCode + "] ");
                            string currDelTerms = lstDelTerms[SupplierCode];
                            if (currDelTerms != "")
                            {
                                string _value = "";
                                HtmlNodeCollection _options = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='UCDeliveryTerms_ddlQuick']//option");
                                if (_options != null)
                                {
                                    foreach (HtmlNode _opt in _options)
                                    {
                                        if (_opt.NextSibling.InnerText.Trim().ToUpper().Contains(currDelTerms))
                                        {
                                            _value = _opt.GetAttributeValue("value", "").Trim(); break;
                                        }
                                    }
                                    dctPostDataValues.Add("UCDeliveryTerms%24ddlQuick", _value);
                                }
                            }
                            else
                            {
                                dctPostDataValues.Add("UCDeliveryTerms%24ddlQuick", "Dummy");
                            }
                            #endregion

                            #region Pay Terms
                            string[] payTerms = convert.ToString(ConfigurationManager.AppSettings["PAY_TERMS"]).Trim().Split('|');
                            Dictionary<string, string> lstPayTerms = new Dictionary<string, string>();
                            foreach (string s in payTerms)
                            {
                                string[] strValues = s.Trim().Split('=');
                                lstPayTerms.Add(strValues[0], strValues[1]);
                            }

                            if (!lstPayTerms.ContainsKey(SupplierCode)) throw new Exception("No setting found for Payment Terms of Vendor [" + SupplierCode + "] ");
                            string currPayTerms = lstPayTerms[SupplierCode];
                            if (currPayTerms != "")
                            {
                                string _value = "";
                                HtmlNodeCollection _options = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='UCPaymentTerms_ddlQuick']//option");
                                if (_options != null)
                                {
                                    foreach (HtmlNode _opt in _options)
                                    {
                                        if (_opt.NextSibling.InnerText.Trim().ToUpper().Contains(currPayTerms))
                                        {
                                            _value = _opt.GetAttributeValue("value", "").Trim(); break;
                                        }
                                    }
                                    dctPostDataValues.Add("UCPaymentTerms%24ddlQuick", _value);
                                }
                            }
                            else
                            {
                                dctPostDataValues.Add("UCPaymentTerms%24ddlQuick", "Dummy");
                            }
                            #endregion

                            #region Mode of Transport
                            if (TransportMode != "")
                            {
                                string _value = "";
                                HtmlNodeCollection _options = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='ucModeOfTransport_ddlQuick']//option");
                                if (_options != null)
                                {
                                    foreach (HtmlNode _opt in _options)
                                    {
                                        if (_opt.NextSibling.InnerText.Trim().ToUpper().Contains(currPayTerms))
                                        {
                                            _value = _opt.GetAttributeValue("value", "").Trim(); break;
                                        }
                                    }
                                    dctPostDataValues.Add("ucModeOfTransport%24ddlQuic", _value);
                                }
                            }
                            else
                            {
                                dctPostDataValues.Add("ucModeOfTransport%24ddlQuick", "Dummy");
                            }
                            #endregion

                            #region currency
                            if (Currency != null )
                            {
                                string _value = "";
                                HtmlNodeCollection _options = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='ucCurrency_ddlCurrency']//option");
                                if (_options != null)
                                {
                                    foreach (HtmlNode _opt in _options)
                                    {
                                        if (_opt.NextSibling.InnerText.Trim().ToUpper().Contains(Currency))
                                        {
                                            _value = _opt.GetAttributeValue("value", "").Trim(); break;
                                        }
                                    }
                                    dctPostDataValues.Add("ucCurrency%24ddlCurrency", _value);
                                }
                            }
                            else throw new Exception("Quoted currency is required."); //dctPostDataValues.Add("ucCurrency%24ddlCurrency", "Dummy");
                            #endregion

                            #region Additional Discount
                            if (AddDiscount > 0)
                            {
                                HtmlNode _disc = _httpWrapper.GetElement("input", "id", "txtSupplierDiscount");
                                if (_disc != null)
                                {
                                    dctPostDataValues.Add("txtSupplierDiscount", convert.ToString(AddDiscount));
                                }
                                else Remarks = "Additional Discount in % : " + AddDiscount;
                            }
                            else
                            {
                                dctPostDataValues.Add("txtSupplierDiscount", "0.000000");
                                if (convert.ToFloat(Allowance) > 0)
                                {
                                    HtmlNode _totalDisc = _httpWrapper.GetElement("input", "id", "txtTotalDiscount");
                                    if (_totalDisc != null)
                                    {
                                        dctPostDataValues.Add("txtTotalDiscount", Allowance);
                                    }
                                    else dctPostDataValues.Add("txtTotalDiscount", "0.00");
                                }
                            }
                            #endregion

                            #region Freight Cost

                            HtmlNode _txtFreightDescr = _httpWrapper.GetElement("input", "id", "txtFreightDescr");
                            if (_txtFreightDescr != null && (convert.ToFloat(FreightCharge) > 0) && convert.ToFloat(TaxCost) == 0)
                            {
                                dctPostDataValues.Add("gvTax%24ctl03%24txtDescriptionAdd", "Freight Cost");
                                dctPostDataValues.Add("gvTax%24ctl03%24ucTaxTypeAdd%24rblValuePercentage", "1");
                                HtmlNode _txtFreightValue = _httpWrapper.GetElement("input", "id", "gvTax_ctl03_txtValueAdd");
                                if (_txtFreightValue != null)
                                {
                                    dctPostDataValues.Add("gvTax%24ctl03%24txtValueAdd", convert.ToFloat(FreightCharge).ToString("0.00"));
                                    counter++;
                                }
                                else throw new Exception("Fright cost value field not found");
                            }
                            else
                            {
                                dctPostDataValues.Add("gvTax%24ctl03%24txtDescriptionAdd", "");
                                dctPostDataValues.Add("gvTax%24ctl03%24ucTaxTypeAdd%24rblValuePercentage", "2");
                                dctPostDataValues.Add("gvTax%24ctl03%24txtValueAdd", "");
                            }
                            #endregion

                            dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", "");
                            if (IsNew)
                                dctPostDataValues.Add("__EVENTTARGET", "MenuQuotationLineItem%24dlstTabs%24ctl00%24btnMenu");
                            else
                                dctPostDataValues.Add("__EVENTTARGET", "MenuQuotationLineItem%24dlstTabs%24ctl03%24btnMenu");
                            dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
                            dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                            dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                            dctPostDataValues.Add("__SCROLLPOSITIONX", "0");
                            dctPostDataValues.Add("__SCROLLPOSITIONY", "282");
                            dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);
                            //dctPostDataValues.Add("txtVessel",_httpWrapper.GetElement("input","id","txtVessel").GetAttributeValue("value","").Trim());
                            //dctPostDataValues.Add("txtIMONo",_httpWrapper.GetElement("input","id","txtIMONo").GetAttributeValue("value","").Trim());
                            dctPostDataValues.Add("txtVenderReference", AAGRefNo);
                            dctPostDataValues.Add("txtOrderDate", Uri.EscapeDataString(dtExpDate));
                            dctPostDataValues.Add("txtDeliveryTime%24txtNumber", LeadDays);
                            dctPostDataValues.Add("MaskedEditExtender1_ClientState", "");
                            dctPostDataValues.Add("ddlType%24ddlHard", "Dummy");
                            dctPostDataValues.Add("txtPrice", "0.00");
                            dctPostDataValues.Add("txtTotalPrice", "0.00");
                            dctPostDataValues.Add("txtDiscount", "10.0000000");
                            dctPostDataValues.Add("meeDiscount_ClientState", "");
                            dctPostDataValues.Add("MaskedEditTotalPayableAmout_ClientState", "");
                            dctPostDataValues.Add("isouterpage", "");
                            dctPostDataValues.Add("txtnopage", "");

                            _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
                            _httpWrapper._SetRequestHeaders[HttpRequestHeader.AcceptLanguage] = "en-IN";
                            _httpWrapper._SetRequestHeaders[HttpRequestHeader.CacheControl] = "no-cache";
                            if (PostURL("input", "id", "txtVenderReference")) //checking
                            {
                                HtmlNode _txtVenRef = _httpWrapper.GetElement("input", "id", "txtVenderReference");
                                if (_txtVenRef.GetAttributeValue("value", "").Trim() == AAGRefNo)
                                { if (IsNew) LogText = "Header details saved."; result = true; }
                                else { result = false; }
                            } 
                            result = true;
                        }
                        else throw new Exception("txtDeliveryTime field not found.");
                    }
                    else throw new Exception("txtOrderDate field not found.");
                }
                else throw new Exception("txtSuppRef field not found on webpage.");
            }
            catch (Exception ex)
            {
                LogText = "Exception while filling quotation: " + ex.GetBaseException().Message.ToString();
                _errorlog = ex.GetBaseException().Message.ToString();
                result = false;
            }
            LogText = "Quotation Header filling ended.";
            return result;
        }

        public bool FillQuoteRemarks()
        {
            bool result = false;
            try
            {
                #region get URL
                List<HtmlNode> lstScripts = _httpWrapper._CurrentDocument.DocumentNode.Descendants()
                .Where(n => n.Name == "script")
                .ToList();
                foreach (HtmlNode a in lstScripts)
                {
                    if (a.InnerText.Trim().Contains("javascript:parent.Openpopup") && a.InnerText.Contains("PurchaseVendorDetail.aspx?QUOTATIONID"))
                    {
                        int startIdx = a.InnerText.Trim().IndexOf("PurchaseVendorDetail") + 2;
                        int _length = (a.InnerText.Trim().Length - 6) - startIdx;
                        URL = a.InnerText.ToString().Substring(startIdx, _length).Trim();
                        result = true;
                        break;
                    }
                }
                #endregion

                if (result)
                {
                    URL = fixedUrl+"ArchangelReports/Purchase/" + URL;
                    if (LoadURL("form", "id", "frmPurchaseFormDetail"))
                    {
                        string _tick = DateTime.Now.Ticks.ToString("x").Substring(0, 13);
                        var boundary = "-----------------------------" + _tick;//DateTime.Now.Ticks.ToString("x", System.Globalization.NumberFormatInfo.InvariantInfo);
                        _httpWrapper.ContentType = "multipart/form-data; boundary=---------------------------" + _tick;
                        _httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");
                        _httpWrapper.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";

                        SupplierComment = SupplierComment.Trim().Replace(Environment.NewLine, "<br>");
                        if (TermsCond != "")
                            SupplierComment = "Terms & Condition : " + TermsCond + Environment.NewLine + SupplierComment;
                        if (Remarks != "")
                            SupplierComment = Remarks + Environment.NewLine + SupplierComment;

                        SupplierComment = SupplierComment.Replace("\r\n", " ");
                        SupplierComment = SupplierComment.Replace("'", "");
                        string postData = boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ToolkitScriptManager1_HiddenField\"" + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                        boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__EVENTTARGET\"" + Environment.NewLine + Environment.NewLine + "MenuFormDetail$dlstTabs$ctl00$btnMenu" +
                        Environment.NewLine + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__EVENTARGUMENT\"" + Environment.NewLine + Environment.NewLine +
                        HttpUtility.UrlDecode(_httpWrapper._dctStateData["__EVENTARGUMENT"]) + Environment.NewLine + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"_contentChanged_txtFormDetails_ucCustomEditor_ctl02\"" +
                        Environment.NewLine + Environment.NewLine + _httpWrapper._dctStateData["_contentChanged_txtFormDetails_ucCustomEditor_ctl02"] + Environment.NewLine + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"_contentForce_txtFormDetails_ucCustomEditor_ctl02\"" +
                        Environment.NewLine + Environment.NewLine + _httpWrapper._dctStateData["_contentForce_txtFormDetails_ucCustomEditor_ctl02"] + Environment.NewLine + boundary + Environment.NewLine +
                        "Content-Disposition: form-data; name=\"_content_txtFormDetails_ucCustomEditor_ctl02\"" + Environment.NewLine + Environment.NewLine + HttpUtility.HtmlEncode(SupplierComment.Trim()) + Environment.NewLine + boundary +
                        Environment.NewLine + "Content-Disposition: form-data; name=\"_activeMode_txtFormDetails_ucCustomEditor_ctl02\"" + Environment.NewLine + Environment.NewLine +
                        _httpWrapper._dctStateData["_activeMode_txtFormDetails_ucCustomEditor_ctl02"] + Environment.NewLine + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__VIEWSTATE\"" +
                        Environment.NewLine + Environment.NewLine + HttpUtility.UrlDecode(_httpWrapper._dctStateData["__VIEWSTATE"]) + Environment.NewLine + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__VIEWSTATEGENERATOR\"" +
                        Environment.NewLine + Environment.NewLine + HttpUtility.UrlDecode(_httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]) + Environment.NewLine + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__EVENTVALIDATION\"" +
                        Environment.NewLine + Environment.NewLine + HttpUtility.UrlDecode(_httpWrapper._dctStateData["__EVENTVALIDATION"]) + Environment.NewLine + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"txtFormDetails$ucCustomEditor$ctl01$ctl47$ctl05\"" +
                        Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"txtFormDetails$ucCustomEditor$ctl01$ctl47$ctl07\"" +
                        Environment.NewLine + Environment.NewLine + "_blank" + Environment.NewLine + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"txtFormDetails$btnImageUpload\"; filename=\"\"" +
                        Environment.NewLine + "Content-Type: application/octet-stream" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + boundary + "--";
                        if (_httpWrapper.PostURL(URL, postData, "span", "id", "ucStatus_litMessage"))
                        {
                            result = true;
                        }
                        else result = false;
                    }
                }
                else result = false;
            }
            catch (Exception ex)
            {
                LogText = "Exception while filling quote remarks: " + ex.GetBaseException().Message.ToString();
                result = false;
            }
            return result;
        }

        public HtmlNode GetElement1(string nodeTitle, string _attributeKey, string _attrValue, HtmlAgilityPack.HtmlDocument doc)
        {
            try
            {
                HtmlNode _node = null;
                _node = doc.DocumentNode.SelectSingleNode("//" + nodeTitle + "[@" + _attributeKey.ToString() + "='" + _attrValue + "']");
                return _node;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool FillItems_(out double extraItemCost)
        {
            extraItemCost = 0;
            bool result = false; Dictionary<int, LineItem> _items = new Dictionary<int, LineItem>(); HtmlNodeCollection _tr = null; 
            List<LineItem> _extraItems = new List<LineItem>(); HtmlAgilityPack.HtmlDocument doc = null;
            try
            {
                LogText = "Quotation Items filling started.";
                URL = MessageNumber;
                _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
                _httpWrapper.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                _httpWrapper.ContentType = "application/x-www-form-urlencoded";
                _httpWrapper._SetRequestHeaders[HttpRequestHeader.CacheControl] = "no-cache";
                _httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");
                if (LoadURL("span", "id", "Title1_lblTitle"))
                {
                    HtmlNode _currPage = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//a[@id='cmdPrevious']");
                    if (_currPage != null)
                    {
                        string _disCurr = _currPage.GetAttributeValue("disabled", "").Trim();
                        if (_disCurr == "disabled")
                        {
                            _tr = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='gvVendorItem']//tr[@tabindex='-1']");
                           
                            FillItemDetails(_tr, _extraItems, _items, out extraItemCost, _httpWrapper._CurrentDocument);
                        }
                    }
                    if (_extraItems.Count > 0)
                    {
                        foreach (LineItem eItem in _extraItems)
                        {
                            if (convert.ToString(eItem.ItemType) == "")
                            {
                                extraItemCost += eItem.MonetaryAmount;
                            }
                        }

                        URL = MessageNumber;
                        _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
                        _httpWrapper.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                        _httpWrapper.ContentType = "application/x-www-form-urlencoded";
                        _httpWrapper._SetRequestHeaders[HttpRequestHeader.CacheControl] = "no-cache";
                        _httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");
                        if (LoadURL("span", "id", "Title1_lblTitle"))
                        {
                            FillExtraItems(_extraItems);
                        }
                    }
               
                    result = true;
                }
        
                LogText = "Quotation Items filling ended.";
            }
            catch (Exception ex)
            {
                LogText = "Exception while filling items: " + ex.GetBaseException().Message.ToString();
                _errorlog = ex.GetBaseException().Message.ToString();
                throw;
                // result = false;
            }
            return result;
        }

        private void FillItemDetails(HtmlNodeCollection _tr, List<LineItem> _extraItems, Dictionary<int, LineItem> _items, out double extraItemCost, HtmlAgilityPack.HtmlDocument doc)
        {
            _httpWrapper._CurrentDocument = doc;string _html=_httpWrapper._CurrentResponseString;
           
            bool result = false; extraItemCost = 0; _items.Clear();
            foreach (LineItem item in _lineitem)
            {
                if (convert.ToInt(item.IsExtraItem) == 1)
                {
                    _extraItems.Add(item);
                }
                else _items.Add(convert.ToInt(item.OriginatingSystemRef), item);
            }

            int SrNo = 0;
            #region to fetch include gst value
            //   dctPostDataValues.Clear();
            Dictionary<string, string> dctGstData = new Dictionary<string, string>();
            foreach (HtmlNode _tr1 in doc.DocumentNode.SelectNodes("//table[@id='gvVendorItem']//tr"))
            {
                if (_tr1.ChildNodes.Count > 6)
                {
                    if (_tr1.InnerText != "\r\n\t\t" && _tr1.ChildNodes[1].ChildNodes[1].InnerText.Trim() != "S.No")
                    {
                        if (_tr1.ChildNodes[2].ChildNodes[1].Attributes["checked"] != null)
                        {
                            dctGstData.Add(Uri.EscapeDataString(_tr1.ChildNodes[2].ChildNodes[1].GetAttributeValue("name", "")), "on");
                        }
                    }
                }
            }
            #endregion

            if (_tr != null)
            {
                if (_tr.Count > 0)
                {
                    int Counter = 03, itemCount = 0;
                    string strCounter = "";
                    if (Counter < _tr.Count + 1) strCounter = "0" + Counter;
                    else if (Counter > 10) strCounter = Counter.ToString();
                    string _i = "gvVendorItem_ctl" + strCounter + "_lblSNo"; 
                    HtmlNode itemNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='" + _i + "']");

                    do
                    {
                        if (itemNo != null)
                        {
                            string ctlID = "ctl" + strCounter;
                            LineItem iObj = null;
                            string RefNo = "";
                            itemCount++;

                            SrNo = convert.ToInt(itemNo.InnerText);
                            double _price = 0, _disc = 0;
                            if (_items.ContainsKey(SrNo))
                            {
                                string iAddRemarks = "";
                                iObj = _items[SrNo];
                                if (iObj.Quantity > 0)
                                {
                                    foreach (PriceDetails amt in iObj.PriceList)
                                    {
                                        if (amt.TypeQualifier == PriceDetailsTypeQualifiers.GRP) _price = amt.Value;
                                        else if (amt.TypeQualifier == PriceDetailsTypeQualifiers.DPR) _disc = amt.Value;
                                    }

                                    if (convert.ToFloat(iObj.MonetaryAmount) > 0)
                                    {
                                        #region edit item row
                                        LogText = "Updating item " + convert.ToString(SrNo);
                                        _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
                                        _httpWrapper.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                                        _httpWrapper.ContentType = "application/x-www-form-urlencoded";
                                        _httpWrapper._SetRequestHeaders[HttpRequestHeader.CacheControl] = "no-cache";

                                        if (_httpWrapper._dctStateData.ContainsKey("__PREVIOUSPAGE"))
                                        {

                                            cPrevPageData =  _httpWrapper._dctStateData["__PREVIOUSPAGE"];
                                        }
                                      

                                        dctPostDataValues.Clear();
                                        dctPostDataValues.Add("__EVENTTARGET", _httpWrapper._dctStateData["__EVENTTARGET"]);
                                        dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
                                        dctPostDataValues.Add("__VIEWSTATE", Uri.EscapeDataString(HttpUtility.UrlDecode(_httpWrapper._dctStateData["__VIEWSTATE"])));
                                        dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                                        dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);//
                                        dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24cmdEdit.x", "3");
                                        dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24cmdEdit.y", "7");
                                        dctPostDataValues.Add("isouterpage", "");
                                        dctPostDataValues.Add("txtnopage", "");
                                        dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", string.Empty);
                                        dctPostDataValues.Add("txtEnquiryNumber", _httpWrapper.GetElement("input", "id", "txtEnquiryNumber").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("txtDeliveryPlace", _httpWrapper.GetElement("input", "id", "txtDeliveryPlace").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("txtContactNo", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtContactNo").GetAttributeValue("value", "").Trim()).Replace("%20", "+"));
                                        dctPostDataValues.Add("txtSenderName", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSenderName").GetAttributeValue("value", "").Trim()).Replace("%20", "+"));
                                        dctPostDataValues.Add("txtSentDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSentDate").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtSenderEmailId", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSenderEmailId").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtVendorName", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVendorName").GetAttributeValue("value", "").Trim()).Replace("%20", "+"));
                                        dctPostDataValues.Add("txtTelephone", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtTelephone").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtFax", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtFax").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtVendorAddress", Uri.EscapeDataString(_httpWrapper.GetElement("textarea", "id", "txtVendorAddress").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtEmail", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtEmail").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtVenderReference", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVenderReference").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtOrderDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtOrderDate").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtPriority", _httpWrapper.GetElement("input", "id", "txtPriority").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("UCDeliveryTerms%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCDeliveryTerms_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("UCPaymentTerms%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCPaymentTerms_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("txtDeliveryTime%24txtNumber", _httpWrapper.GetElement("input", "id", "txtDeliveryTime_txtNumber").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("ucModeOfTransport%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucModeOfTransport_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("ddlPreferedquality%24ddlHard", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ddlPreferedquality_ddlHard']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("ucCurrency%24ddlCurrency", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucCurrency_ddlCurrency']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("txtSupplierDiscount%24txtNumber", _httpWrapper.GetElement("input", "id", "txtSupplierDiscount_txtNumber").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("txtPrice", _httpWrapper.GetElement("input", "id", "txtPrice").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("txtDiscount%24txtNumber", _httpWrapper.GetElement("input", "id", "txtDiscount_txtNumber").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("txtTotalPrice", _httpWrapper.GetElement("input", "id", "txtTotalPrice").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("txtTotalDiscount", _httpWrapper.GetElement("input", "id", "txtTotalDiscount").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("gvTax%24ctl03%24txtDescriptionAdd", _httpWrapper.GetElement("input", "id", "gvTax_ctl03_txtDescriptionAdd").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("__SCROLLPOSITIONX", "0");
                                        dctPostDataValues.Add("__SCROLLPOSITIONY", "0");
                                        if (dctGstData.Count > 0)
                                        {
                                            foreach (KeyValuePair<string, string> _gst in dctGstData)
                                            {
                                                dctPostDataValues.Add(_gst.Key, _gst.Value);
                                            }
                                        }
                                        HtmlNodeCollection _col = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//input[@name='gvTax$ctl03$ucTaxTypeAdd$rblValuePercentage']");
                                        if (_col != null)
                                        {
                                            if (_col.Count > 0)
                                            {
                                                foreach (HtmlNode _node in _col)
                                                {
                                                    if (_node.Attributes["checked"] != null)
                                                    {
                                                        dctPostDataValues.Add("gvTax%24ctl03%24ucTaxTypeAdd%24rblValuePercentage", _node.GetAttributeValue("value", "").Trim());
                                                    }
                                                }
                                            }
                                        }
                                        if (_httpWrapper.GetElement("input", "id", "gvTax_ctl03_txtValueAdd_txtNumber").Attributes["value"] != null)
                                            dctPostDataValues.Add("gvTax%24ctl03%24txtValueAdd%24txtNumber", _httpWrapper.GetElement("input", "id", "gvTax_ctl03_txtValueAdd_txtNumber").GetAttributeValue("value", "").Trim());
                                        else
                                            dctPostDataValues.Add("gvTax%24ctl03%24txtValueAdd%24txtNumber", "");

                                        #endregion

                                        if (PostURL("input", "id", "gvVendorItem_" + ctlID + "_txtVendorItemUnitEdit"))
                                        {
                                            #region saving item row
                                            dctPostDataValues.Clear();
                                            dctPostDataValues.Add("__EVENTTARGET", _httpWrapper._dctStateData["__EVENTTARGET"]);
                                            dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
                                            dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", string.Empty);
                                            dctPostDataValues.Add("__VIEWSTATE", Uri.EscapeDataString(HttpUtility.UrlDecode(_httpWrapper._dctStateData["__VIEWSTATE"])));
                                            dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                                            dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);//_httpWrapper._dctStateData["__PREVIOUSPAGE"]
                                            dctPostDataValues.Add("__SCROLLPOSITIONX", "0");
                                            dctPostDataValues.Add("__SCROLLPOSITIONY", "590");
                                            dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtVendorItemUnitEdit", iObj.MeasureUnitQualifier);
                                            dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtQuantityEdit%24txtNumber", convert.ToString(iObj.Quantity));
                                            dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtQuotedPriceEdit%24txtNumber", convert.ToString(_price));
                                            dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtDiscountEdit%24txtNumber", convert.ToString(_disc));
                                            dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtDeliveryTimeEdit%24txtNumber", iObj.DeleiveryTime);
                                            dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24cmdUpdate.x", "0");
                                            dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24cmdUpdate.y", "0");
                                            dctPostDataValues.Add("isouterpage", "");
                                            dctPostDataValues.Add("txtnopage", "");
                                            if (dctGstData.Count > 0)
                                            {
                                                foreach (KeyValuePair<string, string> _gst in dctGstData)
                                                {
                                                    dctPostDataValues.Add(_gst.Key, _gst.Value);

                                                }
                                            }
                                            #endregion

                                            if (PostURL("span", "id", "gvVendorItem_" + ctlID + "_lblVendorItemUnit"))//checking
                                            {
                                                string iRemarks = iAddRemarks + Environment.NewLine + convert.ToString(iObj.LineItemComment.Value).Trim();
                                                iRemarks = iRemarks.Trim().Replace(Environment.NewLine, "<br>");
                                                iRemarks = iRemarks.Replace("\r\n", " ");
                                                iRemarks = iRemarks.Replace("'", "");
                                                HtmlNode _inpRemarks = _httpWrapper.GetElement("input", "id", "gvVendorItem_" + ctlID + "_imgVendorNotes"); //_tr.ChildNodes[7].ChildNodes[1];
                                                FillItemRemarks(iRemarks, _inpRemarks, iObj.Number);

                                              LoadURL("span", "id", "gvVendorItem_" + ctlID + "_lblVendorItemUnit");
                                              
                                                System.Threading.Thread.Sleep(200);
                                                result = true;
                                            }
                                            else throw new Exception("Unable to update item " + iObj.Number + ".");
                                        }
                                        else throw new Exception("Unable to edit item row " + iObj.Number);
                                    }
                                    else
                                    {
                                        LogText = "Unable to update item " + iObj.Number + "; Item Price is zero.";
                                        string iRemarks = iAddRemarks + Environment.NewLine + convert.ToString(iObj.LineItemComment.Value).Trim();
                                        iRemarks = iRemarks.Trim().Replace(Environment.NewLine, "<br>");
                                        iRemarks = iRemarks.Replace("\r\n", " ");
                                        iRemarks = iRemarks.Replace("'", "");
                                        HtmlNode _inpRemarks = _httpWrapper.GetElement("input", "id", "gvVendorItem_" + ctlID + "_imgVendorNotes"); //_tr.ChildNodes[7].ChildNodes[1];
                                       FillItemRemarks(iRemarks, _inpRemarks, iObj.Number);
                                       LoadURL("span", "id", "gvVendorItem_" + ctlID + "_lblVendorItemUnit");
                                    }
                                }
                                else
                                    LogText = "Unable to update item " + iObj.Number + "; Item Quantity is zero.";
                            }
                            else
                            {
                                throw new Exception("Item no. " + SrNo + " not found in MTML Quote file.");
                            }
                        }

                        Counter++;
                        int _totalPages = 0; string _totalItems = "";
                        if (Counter.ToString().Length == 1) strCounter = "0" + (Counter);
                        else strCounter = (Counter).ToString();
                       
                        if (itemCount >= _tr.Count)
                        {
                            // Check Total Item Count
                            if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='lblRecords']") != null)
                            {
                                _totalItems = convert.ToString(_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='lblRecords']").InnerText.Trim()).Trim().Replace("(", "").Replace(")", "").Replace("records found", "").Trim();
                                _totalPages = convert.ToInt(convert.ToString(_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='lblPages']").InnerText.Trim()).Trim().Replace("of", "").Replace("Pages.", "").Trim());
                            }

                            if (_totalPages == 1 && (convert.ToInt(_totalItems) != _items.Count))
                            {
                                if (itemCount != (_tr.Count - 1))
                                {
                                    throw new Exception("Total Item count not matching with items in grid");
                                }
                                else break;
                            }
                            else
                            {
                                HtmlNode _nextPage = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//a[@id='cmdNext']");
                                if (_nextPage != null)
                                {
                                    string _disNext = _nextPage.GetAttributeValue("disabled", "").Trim();
                                    if (_disNext != "disabled")
                                    {
                                        doc = GetNextPageItems();
                                        _tr = doc.DocumentNode.SelectNodes("//table[@id='gvVendorItem']//tr[@tabindex='-1']");                                        
                                        FillItemDetails(_tr, _extraItems, _items, out  extraItemCost, doc);
                                    }
                                    else break;
                                }

                            }
                            // Reset Counter
                            Counter = 02; itemCount = 0;
                            if (Counter.ToString().Length == 1) strCounter = "0" + Counter;
                            else strCounter = Counter.ToString();
                        }
                        itemNo = doc.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblSNo']");
                    }
                    while (itemNo != null);
                }
            }

        }

        public void FillItemRemarks(string iRemarks, HtmlNode _inpRemarks, string ItemNo)
        {
            string cRmkURL = "";
            if (iRemarks.Trim().Length > 0)
            {
                if (_inpRemarks != null)
                {
                    string onClickEvent = _inpRemarks.GetAttributeValue("onclick", "");
                    int _startIndx = onClickEvent.IndexOf("PurchaseFormItemMoreInfo.aspx?");
                    if (_startIndx > 0)
                    {
                        int _endIndx = 0;
                        _endIndx = onClickEvent.Substring(_startIndx).IndexOf("');");
                        if (_endIndx == -1)
                        {
                            _endIndx = onClickEvent.Substring(_startIndx).IndexOf(");");
                        }
                        //   if (_endIndx > 0) URL = onClickEvent.Substring(_startIndx, _endIndx);
                        if (_endIndx > 0) cRmkURL = onClickEvent.Substring(_startIndx, _endIndx);
                    }

                    if (URL.Trim().Length > 0)
                    {
                        try
                        {
                            //URL = fixedUrl+"ArchangelReports/Purchase/" + URL;
                            URL = fixedUrl + "ArchangelReports/Purchase/" + cRmkURL;
                            URL = URL.Replace("&amp;", "&");
                            _httpWrapper.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                            if (!_httpWrapper._AddRequestHeaders.ContainsKey("Upgrade-Insecure-Requests"))
                                _httpWrapper._AddRequestHeaders.Add("Upgrade-Insecure-Requests", @"1");
                            _httpWrapper.AcceptMimeType = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                            _httpWrapper.ContentType = "";
                            _httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.CacheControl);
                            if (LoadURL("a", "id", "MenuLineItemDetail_dlstTabs_ctl00_btnMenu"))
                            {
                                // string a = String.Format("----------{0:N}", Guid.NewGuid());
                                string _tick = DateTime.Now.Ticks.ToString("x").Substring(0, 13);
                                var boundary = "-----------------------------" + _tick; //------WebKitFormBoundary" + DateTime.Now.ToFileTime();// DateTime.Now.Ticks.ToString("x")+"6";//Convert.ToString(a).Substring(0,26);

                                _httpWrapper.ContentType = "multipart/form-data; boundary=---------------------------" + _tick; //----WebKitFormBoundary" + DateTime.Now.ToFileTime();//DateTime.Now.Ticks.ToString("x") + "6";//a.Substring(0, 26); 
                                if (!_httpWrapper._SetRequestHeaders.ContainsKey(HttpRequestHeader.CacheControl))
                                    _httpWrapper._SetRequestHeaders.Add(HttpRequestHeader.CacheControl, "max-age=0");
                                if (!_httpWrapper._AddRequestHeaders.ContainsKey("Origin"))
                                    _httpWrapper._AddRequestHeaders.Add("Origin", fixedUrl);
                                _httpWrapper._SetRequestHeaders[HttpRequestHeader.AcceptEncoding] = "gzip, deflate, br";
                                _httpWrapper._SetRequestHeaders[HttpRequestHeader.AcceptLanguage] = "en-US,en;q=0.9";

                                string postData = boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ToolkitScriptManager1_HiddenField\"" + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                                                                 boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__EVENTTARGET\"" + Environment.NewLine + Environment.NewLine + "MenuLineItemDetail$dlstTabs$ctl00$btnMenu" +
                                                                 Environment.NewLine + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__EVENTARGUMENT\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine +
                                                                 boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"_contentChanged_txtItemDetails_ucCustomEditor_ctl02\"" + Environment.NewLine + Environment.NewLine + HttpUtility.UrlDecode(_httpWrapper._dctStateData["_contentChanged_txtItemDetails_ucCustomEditor_ctl02"]) +
                                                                 Environment.NewLine + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"_contentForce_txtItemDetails_ucCustomEditor_ctl02\"" + Environment.NewLine + Environment.NewLine + HttpUtility.UrlDecode(_httpWrapper._dctStateData["_contentForce_txtItemDetails_ucCustomEditor_ctl02"]) +
                                                                 Environment.NewLine + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"_content_txtItemDetails_ucCustomEditor_ctl02\"" + Environment.NewLine + Environment.NewLine + HttpUtility.HtmlEncode(iRemarks.Trim()) + Environment.NewLine +
                                                                 boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"_activeMode_txtItemDetails_ucCustomEditor_ctl02\"" + Environment.NewLine + Environment.NewLine + HttpUtility.UrlDecode(_httpWrapper._dctStateData["_activeMode_txtItemDetails_ucCustomEditor_ctl02"]) +
                                                                 Environment.NewLine + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__VIEWSTATE\"" + Environment.NewLine + Environment.NewLine + HttpUtility.UrlDecode(_httpWrapper._dctStateData["__VIEWSTATE"]) + Environment.NewLine + boundary +
                                                                 Environment.NewLine + "Content-Disposition: form-data; name=\"__VIEWSTATEGENERATOR\"" + Environment.NewLine + Environment.NewLine + HttpUtility.UrlDecode(_httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]) + Environment.NewLine +
                                                                 boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"txtItemDetails$ucCustomEditor$ctl01$ctl47$ctl05\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine +
                                                                 boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"txtItemDetails$ucCustomEditor$ctl01$ctl47$ctl07\"" + Environment.NewLine + Environment.NewLine + "_blank" + Environment.NewLine +
                                                                 boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"txtItemDetails$btnImageUpload\"; filename=\"\"" + Environment.NewLine + "Content-Type: application/octet-stream" + Environment.NewLine +
                                                                 Environment.NewLine + Environment.NewLine + boundary + "--";
                                URL = URL.Replace("&amp;", "&");
                                if (!_httpWrapper.PostURL(URL, postData, "a", "id", "MenuLineItemDetail_dlstTabs_ctl00_btnMenu"))
                                {
                                    //HtmlNode _hiddenText = _httpWrapper.GetElement("input","id","_content_txtItemDetails_ucCustomEditor_ctl02");
                                    //if (_hiddenText != null)
                                    //{
                                    //    if (_hiddenText.GetAttributeValue("value", "").Trim() == iRemarks.Trim())
                                    //    {
                                    //        LogText = "";
                                    //    }
                                    //}
                                    LogText = "Error while updating item remarks : item no - " + ItemNo + ".";
                                }
                                else
                                {
                                    URL = cQuoteURL; //LoadURL("", "", "");                                    
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            LogText = "Error while updating item remarks : item no - " + ItemNo + "; " + ex.StackTrace;
                        }
                    }
                }
            }
        }

        private int IsExtraItemExists(string itemDescr)
        {
            try
            {
                int returnRowNo = 0, rowCount = 0, nCount = 2;
                HtmlNode _div2 = _httpWrapper.GetElement("div", "id", "div2");
                if (_div2 != null)
                {
                    rowCount = _div2.SelectNodes("//table[@id='gvTax']//tr").Count;
                    if (rowCount > 0)
                    {
                        for (int i = nCount; i < rowCount; i++)
                        {
                            string _itemDescr = "gvTax_ctl0" + i + "_lblDescriptionEdit";
                            HtmlNode _span = _httpWrapper.GetElement("span", "id", _itemDescr);
                            if (_span != null)
                            {
                                if (convert.ToString(_span.InnerText.Trim()).ToUpper() == itemDescr.Trim().ToUpper())
                                {
                                    returnRowNo = i;
                                    break;
                                }
                            }
                        }
                    }
                }
                return returnRowNo;
            }
            catch (Exception ex)
            {
                LogText = "Error in IsExtraItemExists(): " + ex.Message;
                throw ex;
            }
        }

        public void FillExtraItems(List<LineItem> _extraItems)
        {
            if (_extraItems.Count > 0)
            {

                if (_extraItems.Count > 0 && counter == 0)
                {
                    int itemCounter = 0; string PreviousPage = "";
                    foreach (LineItem _eItem in _extraItems)
                    {
                        // Fill Only Delivery/Tax/Others Charge Description 
                        if (convert.ToString(_eItem.ItemType).Trim() != "")
                        {
                            if (itemCounter > 8) throw new Exception("extra items more than 8");
                            LogText = "Filling " + _eItem.Description;

                            int rowNo = IsExtraItemExists(convert.ToString(_eItem.Description));
                            if (rowNo >= 2)
                            {
                                #region Updating existing extra item cost
                                HtmlNode _btnedit = _httpWrapper.GetElement("input", "id", "gvTax_ctl0" + rowNo + "_cmdEdit");
                                if (_btnedit != null)
                                {
                                    URL = MessageNumber;
                                    _httpWrapper.AcceptMimeType = "*/*";
                                    _httpWrapper._AddRequestHeaders["X-Requested-With"] = @"XMLHttpRequest";
                                    _httpWrapper._AddRequestHeaders["X-MicrosoftAjax"] = @"Delta=true";
                                    _httpWrapper.ContentType = "application/x-www-form-urlencoded; charset=utf-8";

                                    //for clicking edit button
                                    dctPostDataValues.Clear();
                                    dctPostDataValues.Add("ToolkitScriptManager1", "pnlLineItemEntry%7CgvTax%24ctl0" + rowNo + "%24cmdEdit");
                                    dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", "");
                                    dctPostDataValues.Add("__EVENTTARGET", "");
                                    dctPostDataValues.Add("__EVENTARGUMENT", "");
                                    dctPostDataValues.Add("__VIEWSTATE", Uri.EscapeDataString(HttpUtility.UrlDecode(_httpWrapper._dctStateData["__VIEWSTATE"])));
                                    dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                                    dctPostDataValues.Add("__SCROLLPOSITIONX", "0");
                                    dctPostDataValues.Add("__SCROLLPOSITIONY", "0");
                                    dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);
                                    PreviousPage = _httpWrapper._dctStateData["__PREVIOUSPAGE"];
                                    dctPostDataValues.Add("txtVenderReference", AAGRefNo);
                                    dctPostDataValues.Add("txtOrderDate", Uri.EscapeDataString(convert.ToDateTime(dtExpDate).ToString("dd/MM/yyyy")));
                                    dctPostDataValues.Add("txtExpirationDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtExpirationDate").GetAttributeValue("value", "").Trim()));
                                    dctPostDataValues.Add("txtPriority", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtPriority").GetAttributeValue("value", "").Trim()));
                                    dctPostDataValues.Add("UCDeliveryTerms%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCDeliveryTerms_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                    dctPostDataValues.Add("UCPaymentTerms%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCPaymentTerms_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                    dctPostDataValues.Add("txtDeliveryTime%24txtNumber", _httpWrapper.GetElement("input", "id", "txtDeliveryTime_txtNumber").GetAttributeValue("value", "").Trim());
                                    dctPostDataValues.Add("MaskedEditExtender1_ClientState", "");
                                    dctPostDataValues.Add("ucModeOfTransport%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucModeOfTransport_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                    dctPostDataValues.Add("ddlType%24ddlHard", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ddlType_ddlHard']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                    dctPostDataValues.Add("ucCurrency%24ddlCurrency", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucCurrency_ddlCurrency']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                    dctPostDataValues.Add("txtSupplierDiscount", _httpWrapper.GetElement("input", "id", "txtSupplierDiscount").GetAttributeValue("value", "").Trim());
                                    dctPostDataValues.Add("txtPrice", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtPrice").GetAttributeValue("value", "").Trim()));
                                    dctPostDataValues.Add("txtTotalDiscount", _httpWrapper.GetElement("input", "id", "txtTotalDiscount").GetAttributeValue("value", "").Trim());
                                    dctPostDataValues.Add("txtTotalPrice", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtTotalPrice").GetAttributeValue("value", "").Trim()));
                                    dctPostDataValues.Add("txtDiscount", _httpWrapper.GetElement("input", "id", "txtDiscount").GetAttributeValue("value", "").Trim());
                                    dctPostDataValues.Add("meeDiscount_ClientState", "");
                                    dctPostDataValues.Add("__ASYNCPOST", "true");
                                    dctPostDataValues.Add("gvTax%24ctl0" + (rowNo + 1) + "%24txtDescriptionAdd", "");
                                    dctPostDataValues.Add("gvTax%24ctl0" + (rowNo + 1) + "%24ucTaxTypeAdd%24rblValuePercentage", "2");
                                    dctPostDataValues.Add("gvTax%24ctl0" + (rowNo + 1) + "%24txtValueAdd", "");
                                    dctPostDataValues.Add("gvTax%24ctl0" + (rowNo + 1) + "%24MaskedEditTotalPayableAmout_ClientState", "");
                                    dctPostDataValues.Add("gvTax%24ctl0" + rowNo + "%24cmdEdit.x", "393.989990234375");
                                    dctPostDataValues.Add("gvTax%24ctl0" + rowNo + "%24cmdEdit.y", "183.0699920654297");
                                    dctPostDataValues.Add("isouterpage", "");
                                    dctPostDataValues.Add("txtnopage", "");
                                    if (PostURL("input", "id", "gvTax_ctl0" + rowNo + "_txtDescriptionEdit"))
                                    {
                                        //for saving extra item details
                                        dctPostDataValues.Clear();
                                        dctPostDataValues.Add("ToolkitScriptManager1", "pnlLineItemEntry%7CgvTax%24ctl0" + rowNo + "%24cmdSave");
                                        dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", "");
                                        dctPostDataValues.Add("__PREVIOUSPAGE", PreviousPage);
                                        dctPostDataValues.Add("txtVessel", Uri.EscapeDataString(VesselName));
                                        dctPostDataValues.Add("txtIMONo", ImoNo);
                                        dctPostDataValues.Add("txtHullNo", _httpWrapper.GetElement("input", "id", "txtHullNo").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("txtVesseltype", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVesseltype").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtYard", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtYard").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtYearBuilt", _httpWrapper.GetElement("input", "id", "txtYearBuilt").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("txtSenderName", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSenderName").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtDeliveryPlace", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtDeliveryPlace").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtContactNo", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtContactNo").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtSenderEmailId", _httpWrapper.GetElement("input", "id", "txtSenderEmailId").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("txtPortName", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtPortName").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtSentDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSentDate").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtVendorName", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVendorName").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtTelephone", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtTelephone").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtFax", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtFax").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtVendorAddress", Uri.EscapeDataString(_httpWrapper.GetElement("textarea", "id", "txtVendorAddress").InnerText.Trim()));
                                        dctPostDataValues.Add("txtEmail", _httpWrapper.GetElement("input", "id", "txtEmail").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("txtVenderReference", AAGRefNo);
                                        dctPostDataValues.Add("txtOrderDate", Uri.EscapeDataString(convert.ToDateTime(dtExpDate).ToString("dd/MM/yyyy")));
                                        dctPostDataValues.Add("txtExpirationDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtExpirationDate").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtPriority", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtPriority").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("UCDeliveryTerms%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCDeliveryTerms_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("UCPaymentTerms%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCPaymentTerms_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("txtDeliveryTime%24txtNumber", _httpWrapper.GetElement("input", "id", "txtDeliveryTime_txtNumber").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("MaskedEditExtender1_ClientState", "");
                                        dctPostDataValues.Add("ucModeOfTransport%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucModeOfTransport_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("ddlType%24ddlHard", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ddlType_ddlHard']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("ucCurrency%24ddlCurrency", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucCurrency_ddlCurrency']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("txtSupplierDiscount", _httpWrapper.GetElement("input", "id", "txtSupplierDiscount").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("txtPrice", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtPrice").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtTotalDiscount", _httpWrapper.GetElement("input", "id", "txtTotalDiscount").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("txtTotalPrice", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtTotalPrice").GetAttributeValue("value", "").Trim()));
                                        dctPostDataValues.Add("txtDiscount", _httpWrapper.GetElement("input", "id", "txtDiscount").GetAttributeValue("value", "").Trim());
                                        dctPostDataValues.Add("meeDiscount_ClientState", "");
                                        dctPostDataValues.Add("gvTax%24ctl0" + rowNo + "%24txtDescriptionEdit", HttpUtility.UrlEncode(_eItem.Description));
                                        dctPostDataValues.Add("gvTax%24ctl0" + rowNo + "%24ucTaxTypeEdit%24rblValuePercentage", "1");
                                        dctPostDataValues.Add("gvTax%24ctl0" + rowNo + "%24txtValueEdit", convert.ToString(_eItem.MonetaryAmount));
                                        dctPostDataValues.Add("gvTax%24ctl0" + rowNo + "%24MaskedEditTotalPayableAmout_ClientState", "");
                                        dctPostDataValues.Add("gvTax%24ctl0" + (rowNo + 1) + "%24txtDescriptionAdd", "");
                                        dctPostDataValues.Add("gvTax%24ctl0" + (rowNo + 1) + "%24ucTaxTypeAdd%24rblValuePercentage", "2");
                                        dctPostDataValues.Add("gvTax%24ctl0" + (rowNo + 1) + "%24txtValueAdd", "");
                                        dctPostDataValues.Add("gvTax%24ctl0" + (rowNo + 1) + "%24MaskedEditTotalPayableAmout_ClientState", "");
                                        dctPostDataValues.Add("isouterpage", "");
                                        dctPostDataValues.Add("txtnopage", "");
                                        dctPostDataValues.Add("__EVENTTARGET", "");
                                        dctPostDataValues.Add("__EVENTARGUMENT", "");
                                        ReadHiddenData();
                                        dctPostDataValues.Add("__SCROLLPOSITIONX", "0");
                                        dctPostDataValues.Add("__SCROLLPOSITIONY", "0");
                                        dctPostDataValues.Add("__ASYNCPOST", "true");
                                        dctPostDataValues.Add("gvTax%24ctl0" + rowNo + "%24cmdSave.x", "-590.989990234375");
                                        dctPostDataValues.Add("gvTax%24ctl0" + rowNo + "%24cmdSave.y", "550.049987792968");
                                        if (PostURL("span", "id", "gvTax_ctl0" + rowNo + "_lblDescriptionEdit"))
                                        {
                                            LogText = "Extra item :" + _eItem.Description + " is updated";
                                            URL = MessageNumber;
                                            _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
                                            _httpWrapper.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                                            _httpWrapper.ContentType = "application/x-www-form-urlencoded";
                                            _httpWrapper._SetRequestHeaders[HttpRequestHeader.CacheControl] = "no-cache";
                                            _httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");
                                            if (!LoadURL("span", "id", "Title1_lblTitle"))
                                            {
                                                throw new Exception("Unable to move to next extra item " + _eItem.Description);
                                            }
                                        }
                                        else throw new Exception("Unable to update extra item " + _eItem.Description);
                                    }
                                }
                                else throw new Exception("Extra item's edit button not found.");
                                #endregion
                            }
                            else
                            {
                                #region // Add New Extra Item Cost //
                                int newRowCount = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='gvTax']//tr").Count - 1;
                                string _rowNum = "";
                                if (newRowCount <= 9) _rowNum = "0" + newRowCount.ToString();
                                else _rowNum = newRowCount.ToString();

                                HtmlNode txtFreightDescr = _httpWrapper.GetElement("input", "id", "gvTax_ctl" + _rowNum + "_txtDescriptionAdd");
                                if (txtFreightDescr != null && _eItem.MonetaryAmount > 0)
                                {
                                    HtmlNode txtFreightCharge = _httpWrapper.GetElement("input", "id", "gvTax_ctl" + _rowNum + "_txtValueAdd");
                                    if (txtFreightCharge != null)
                                    {
                                        HtmlNode rdbValue = _httpWrapper.GetElement("input", "id", "gvTax_ctl" + _rowNum + "_ucTaxTypeAdd_rblValuePercentage_0");
                                        if (rdbValue != null)
                                        {
                                            //adding new row for extra item 
                                            URL = MessageNumber;
                                            dctPostDataValues.Clear();
                                            dctPostDataValues.Add("ToolkitScriptManager1", "pnlLineItemEntry%7CgvTax%24ctl" + _rowNum + "%24cmdAdd");
                                            dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", "");
                                            dctPostDataValues.Add("__EVENTTARGET", "");
                                            dctPostDataValues.Add("__EVENTARGUMENT", "");
                                            dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                                            dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                                            //dctPostDataValues.Add("__SCROLLPOSITIONX", "0");
                                            //dctPostDataValues.Add("__SCROLLPOSITIONY", "0");
                                            dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);
                                            dctPostDataValues.Add("txtVenderReference", AAGRefNo);
                                            dctPostDataValues.Add("txtOrderDate", Uri.EscapeDataString(convert.ToDateTime(dtExpDate).ToString("dd/MM/yyyy")));
                                            dctPostDataValues.Add("txtExpirationDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtExpirationDate").GetAttributeValue("value", "").Trim()));
                                            dctPostDataValues.Add("txtPriority", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtPriority").GetAttributeValue("value", "").Trim()));
                                            dctPostDataValues.Add("UCDeliveryTerms%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCDeliveryTerms_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                            dctPostDataValues.Add("UCPaymentTerms%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCPaymentTerms_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                            dctPostDataValues.Add("txtDeliveryTime%24txtNumber", _httpWrapper.GetElement("input", "id", "txtDeliveryTime_txtNumber").GetAttributeValue("value", "").Trim());
                                            dctPostDataValues.Add("MaskedEditExtender1_ClientState", "");
                                            dctPostDataValues.Add("ucModeOfTransport%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucModeOfTransport_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                            dctPostDataValues.Add("ddlType%24ddlHard", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ddlType_ddlHard']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                            dctPostDataValues.Add("ucCurrency%24ddlCurrency", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucCurrency_ddlCurrency']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                                            dctPostDataValues.Add("txtSupplierDiscount", _httpWrapper.GetElement("input", "id", "txtSupplierDiscount").GetAttributeValue("value", "").Trim());
                                            dctPostDataValues.Add("txtPrice", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtPrice").GetAttributeValue("value", "").Trim()));
                                            dctPostDataValues.Add("txtTotalDiscount", _httpWrapper.GetElement("input", "id", "txtTotalDiscount").GetAttributeValue("value", "").Trim());
                                            dctPostDataValues.Add("txtTotalPrice", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtTotalPrice").GetAttributeValue("value", "").Trim()));
                                            dctPostDataValues.Add("txtDiscount", _httpWrapper.GetElement("input", "id", "txtDiscount").GetAttributeValue("value", "").Trim());
                                            dctPostDataValues.Add("meeDiscount_ClientState", "");
                                            dctPostDataValues.Add("gvTax%24ctl" + _rowNum + "%24txtDescriptionAdd", HttpUtility.UrlEncode(_eItem.Description));
                                            dctPostDataValues.Add("gvTax%24ctl" + _rowNum + "%24ucTaxTypeAdd%24rblValuePercentage", "1");
                                            dctPostDataValues.Add("gvTax%24ctl" + _rowNum + "%24txtValueAdd", convert.ToString(_eItem.MonetaryAmount));
                                            dctPostDataValues.Add("gvTax%24ctl" + _rowNum + "%24MaskedEditTotalPayableAmout_ClientState", "");
                                            dctPostDataValues.Add("isouterpage", "");
                                            dctPostDataValues.Add("txtnopage", "");
                                            dctPostDataValues.Add("__ASYNCPOST", "true");
                                            dctPostDataValues.Add("gvTax%24ctl" + _rowNum + "%24cmdAdd.x", "31279.359375");
                                            dctPostDataValues.Add("gvTax%24ctl" + _rowNum + "%24cmdAdd.y", "32762.068359375");
                                            if (PostURL("span", "id", "gvTax_ctl0" + (convert.ToInt(_rowNum) - 1) + "_lblDescriptionEdit"))
                                            {
                                                //saving extra item
                                                LogText = "Extra item :" + _eItem.Description + " is saved";
                                            }
                                            else throw new Exception("Unable to save extra item " + _eItem.Description);
                                        }
                                        else throw new Exception("Charge radio button not found.");
                                    }
                                    else throw new Exception("Charge value not found.");
                                }
                                else if (txtFreightDescr == null) throw new Exception("Charge Description field not found");
                                #endregion
                            }

                            itemCounter++;
                        }
                    }
                }
            }
        }

        public void ReadHiddenData()
        {
            // dctPostDataValues.Clear();
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

        public void SendQuote(string MTML_File, double FinalAmt, double extraItemCost)
        {
            try
            {
                LogText = "Saving Quote '" + UCRefNo + "'.";
                URL = MessageNumber;
                if (LoadURL("input", "id", "txtTotalPrice"))
                {
                    dctPostDataValues.Clear();
                    dctPostDataValues.Add("__EVENTTARGET", "MenuQuotationLineItem%24dlstTabs%24ctl00%24btnMenu");
                    dctPostDataValues.Add("__EVENTARGUMENT", "");
                    dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                    dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                    dctPostDataValues.Add("__SCROLLPOSITIONX", "0");
                    dctPostDataValues.Add("__SCROLLPOSITIONY", "0");
                    dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);
                 //   dctPostDataValues.Add("txtVessel", Uri.EscapeDataString(VesselName));
                   // dctPostDataValues.Add("txtIMONo", ImoNo);
                   // dctPostDataValues.Add("txtHullNo", _httpWrapper.GetElement("input", "id", "txtHullNo").GetAttributeValue("value", "").Trim());
                  //  dctPostDataValues.Add("txtVesseltype", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVesseltype").GetAttributeValue("value", "").Trim()));
                   // dctPostDataValues.Add("txtYard", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtYard").GetAttributeValue("value", "").Trim()));
                   // dctPostDataValues.Add("txtYearBuilt", _httpWrapper.GetElement("input", "id", "txtYearBuilt").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("txtSenderName", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSenderName").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtDeliveryPlace", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtDeliveryPlace").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtContactNo", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtContactNo").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtSenderEmailId", _httpWrapper.GetElement("input", "id", "txtSenderEmailId").GetAttributeValue("value", "").Trim());
                   // dctPostDataValues.Add("txtPortName", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtPortName").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtSentDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSentDate").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtVendorName", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVendorName").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtTelephone", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtTelephone").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtFax", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtFax").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtVendorAddress", Uri.EscapeDataString(_httpWrapper.GetElement("textarea", "id", "txtVendorAddress").InnerText.Trim()));
                    dctPostDataValues.Add("txtEmail", _httpWrapper.GetElement("input", "id", "txtEmail").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("txtVenderReference", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVenderReference").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtOrderDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtOrderDate").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtPriority", _httpWrapper.GetElement("input", "id", "txtPriority").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("UCDeliveryTerms%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCDeliveryTerms_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("UCPaymentTerms%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCPaymentTerms_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("txtDeliveryTime%24txtNumber", _httpWrapper.GetElement("input", "id", "txtDeliveryTime_txtNumber").GetAttributeValue("value", "").Trim());
                    //dctPostDataValues.Add("MaskedEditExtender1_ClientState", "");
                    dctPostDataValues.Add("ucModeOfTransport%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucModeOfTransport_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                    //dctPostDataValues.Add("ddlType%24ddlHard", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ddlType_ddlHard']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("ucCurrency%24ddlCurrency", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucCurrency_ddlCurrency']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("txtSupplierDiscount", _httpWrapper.GetElement("input", "id", "txtSupplierDiscount_txtNumber").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("txtPrice", _httpWrapper.GetElement("input", "id", "txtPrice").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("txtTotalDiscount", _httpWrapper.GetElement("input", "id", "txtTotalDiscount").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("txtTotalPrice", _httpWrapper.GetElement("input", "id", "txtTotalPrice").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("txtDiscount", _httpWrapper.GetElement("input", "id", "txtDiscount_txtNumber").GetAttributeValue("value", "").Trim());
                   // dctPostDataValues.Add("meeDiscount_ClientState", "");
                    dctPostDataValues.Add("gvTax%24ctl03%24txtDescriptionAdd", "");
                    dctPostDataValues.Add("gvTax%24ctl03%24ucTaxTypeAdd%24rblValuePercentage", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//input[@name='gvTax$ctl03$ucTaxTypeAdd$rblValuePercentage'][@checked='checked']").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("gvTax%24ctl03%24txtValueAdd", "");
                   // dctPostDataValues.Add("gvTax%24ctl03%24MaskedEditTotalPayableAmout_ClientState", "");
                    dctPostDataValues.Add("isouterpage", "");
                    dctPostDataValues.Add("txtnopage", "");
                    if (PostURL("input", "id", "txtTotalPrice"))
                    {
                        if (extraItemCost > 0) FinalAmt = FinalAmt - extraItemCost;
                        if (Math.Round(convert.ToDouble(_httpWrapper.GetElement("input", "id", "txtTotalPrice").GetAttributeValue("value", "").Trim()), 2) > 0)
                        {
                            bool sendQuote = false;
                            if (Math.Round(convert.ToDouble(_httpWrapper.GetElement("input", "id", "txtTotalPrice").GetAttributeValue("value", "").Trim()), 0) == Math.Round(FinalAmt, 0))
                            {
                                sendQuote = true;
                            }
                            else
                            {
                                double webPrice = Math.Round(convert.ToDouble(_httpWrapper.GetElement("input", "id", "txtTotalPrice").GetAttributeValue("value", "").Trim()), 2);
                                double quotePrice = Math.Round(FinalAmt, 2);
                                if (webPrice >= quotePrice && (webPrice - quotePrice) <= 1)
                                {
                                    sendQuote = true;
                                }
                                else if (quotePrice > webPrice && (quotePrice - webPrice) <= 1)
                                {
                                    sendQuote = true;
                                }
                            }

                            if (sendQuote)
                            {
                                LogText = "Sending Quote '" + UCRefNo + "'.";
                                HtmlNode _btnSend = _httpWrapper.GetElement("a", "id", "MenuQuotationLineItem_dlstTabs_ctl01_btnMenu");
                                if (_btnSend != null)
                                {
                                    string eFile = ImagePath + "\\ArcMarine_" + this.DocType + "Save_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                                    //_httpWrapper._CurrentDocument.DocumentNode.Descendants()
                                    //                    .Where(n => n.Name == "link")
                                    //                    .ToList()
                                    //                    .ForEach(n => n.Remove());
                                    _httpWrapper.GetElement("div", "id", "navigation").Attributes["class"].Remove();

                                    _CurrentResponseString = _httpWrapper._CurrentDocument.DocumentNode.OuterHtml;
                                    if (!PrintScreen(eFile)) eFile = "";
                                    if (dctPostDataValues.Count > 0)
                                    {
                                        _httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");
                                        _httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.CacheControl);
                                        _httpWrapper.ContentType = "application/x-www-form-urlencoded";
                                        dctPostDataValues["__EVENTTARGET"] = "MenuQuotationLineItem%24dlstTabs%24ctl01%24btnMenu";
                                        if (PostURL("h1", "id", "H1"))
                                        {
                                            dctPostDataValues.Clear();
                                            counter = 0;
                                            HtmlNode _lblSuccess = _httpWrapper.GetElement("h1", "id", "H1");
                                            if (_lblSuccess != null)
                                            {
                                                LogText = "Quote for '" + UCRefNo + "' submitted successfully.";
                                                //print
                                                // eFile = ImagePath + "\\Arc_" + this.DocType + "Success_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                                                //_httpWrapper._CurrentDocument.DocumentNode.Descendants()
                                                //                    .Where(n => n.Name == "link")
                                                //                    .ToList()
                                                //                    .ForEach(n => n.Remove());

                                                //_CurrentResponseString = _httpWrapper._CurrentDocument.DocumentNode.OuterHtml;
                                                //if (!PrintScreen(eFile)) eFile = "";//
                                                URL = MessageNumber;
                                                if (LoadURL("input", "id", "txtTotalPrice"))
                                                {
                                                 //   string filename = GetAttachment();
                                                    CreateAuditFile(eFile, "Arc_Http_" + this.DocType, UCRefNo, "Uploaded", "Quote for '" + UCRefNo + "' submitted successfully.", BuyerCode, SupplierCode, AuditPath);
                                                }

                                                if (!Directory.Exists(QuotePOCPath + "\\Error")) Directory.CreateDirectory(QuotePOCPath + "\\Error");
                                                File.Move(QuotePOCPath + "\\" + Path.GetFileName(MTML_File), QuotePOCPath + "\\Backup\\" + Path.GetFileName(MTML_File));

                                                if (convert.ToString(ConfigurationManager.AppSettings["SEND_MAIL_FOR_SUBMIT"]).Trim().ToUpper() == "TRUE")
                                                {
                                                    LogText = "Generating mail notification...";
                                                    SendMailNotification(_interchange, "QUOTE", UCRefNo.Trim(), "SUBMITTED", "Quote for REF '" + UCRefNo.Trim() + "' submitted successfully.");
                                                }
                                            }
                                            else Write_Quote_ErrorLog("Unable to get success msg while sending quote for '" + UCRefNo + "'.", MTML_File);
                                        }
                                        else
                                        {
                                            Write_Quote_ErrorLog("Unable to send quote for '" + UCRefNo + "'.", MTML_File);
                                        }
                                    }
                                }
                                else
                                {
                                    Write_Quote_ErrorLog("Send button not found on webpage for '" + UCRefNo + "'.", MTML_File);
                                }
                            }
                            else
                            {
                                Write_Quote_ErrorLog("Quote total is mismatched for '" + UCRefNo + "'.", MTML_File);
                            }
                        }
                        else
                        {
                            Write_Quote_ErrorLog("Quote total is 0 for '" + UCRefNo + "'.", MTML_File);
                        }
                    }
                    else
                    {
                        Write_Quote_ErrorLog("Unable to save quote for '" + UCRefNo + "'.", MTML_File);
                    }
                }
            }
            catch (Exception ex)
            {
                Write_Quote_ErrorLog("Unable to send quote '" + UCRefNo + "'." + ex.Message, MTML_File);
            }
        }

        public bool DeclineQuote(string MTML_File)
        {
            bool result = false;
            try
            {
                URL = MessageNumber;
                _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
                _httpWrapper.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                _httpWrapper.ContentType = "application/x-www-form-urlencoded";
                _httpWrapper._SetRequestHeaders[HttpRequestHeader.CacheControl] = "no-cache";
                _httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");
                if (LoadURL("span", "id", "Title1_lblTitle"))
                {
                    dctPostDataValues.Clear();
                    dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", _httpWrapper._dctStateData["ToolkitScriptManager1_HiddenField"]);
                    dctPostDataValues.Add("__EVENTTARGET", "MenuQuotationLineItem%24dlstTabs%24ctl06%24btnMenu");
                    dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
                    dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                    dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                    dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);
                    dctPostDataValues.Add("txtVessel", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtVessel").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtIMONo", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtIMONo").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtHullNo", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtHullNo").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtVesseltype", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtVesseltype").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtYard", WebUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtYard").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtYearBuilt", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtYearBuilt").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtSenderName", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtSenderName").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtDeliveryPlace", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtDeliveryPlace").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtContactNo", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtContactNo").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtSenderEmailId", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtSenderEmailId").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtPortName", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtPortName").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtSentDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSentDate").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtVendorName", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtVendorName").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtTelephone", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtTelephone").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtFax", WebUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtFax").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtVendorAddress", WebUtility.UrlEncode(_httpWrapper.GetElement("textarea", "id", "txtVendorAddress").InnerText.Trim()));
                    dctPostDataValues.Add("txtEmail", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtEmail").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtVenderReference", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVenderReference").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtOrderDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtOrderDate").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtPriority", _httpWrapper.GetElement("input", "id", "txtPriority").GetAttributeValue("value", "").Trim());
                    string _delTerms = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCDeliveryTerms_ddlQuick']/option[@selected='selected']").GetAttributeValue("value", "").Trim();
                    dctPostDataValues.Add("UCDeliveryTerms%24ddlQuick", _delTerms);
                    string _payTerms = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCPaymentTerms_ddlQuick']/option[@selected='selected']").GetAttributeValue("value", "").Trim();
                    dctPostDataValues.Add("UCPaymentTerms%24ddlQuick", _payTerms);
                    dctPostDataValues.Add("txtDeliveryTime%24txtNumber", _httpWrapper.GetElement("input", "id", "txtDeliveryTime_txtNumber").GetAttributeValue("value", "").Trim());
                    string _mode = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucModeOfTransport_ddlQuick']/option[@selected='selected']").GetAttributeValue("value", "").Trim();
                    dctPostDataValues.Add("ucModeOfTransport%24ddlQuick", _mode);
                    string _ddlType = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ddlType_ddlHard']/option[@selected='selected']").GetAttributeValue("value", "").Trim();
                    dctPostDataValues.Add("ddlType%24ddlHard", _ddlType);
                    string _currency = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucCurrency_ddlCurrency']/option[@selected='selected']").GetAttributeValue("value", "").Trim();
                    dctPostDataValues.Add("ucCurrency%24ddlCurrency", _currency);
                    dctPostDataValues.Add("txtSupplierDiscount", _httpWrapper.GetElement("input", "id", "txtSupplierDiscount").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("txtPrice", _httpWrapper.GetElement("input", "id", "txtPrice").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("txtTotalDiscount", _httpWrapper.GetElement("input", "id", "txtTotalDiscount").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("txtTotalPrice", _httpWrapper.GetElement("input", "id", "txtTotalPrice").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("txtDiscount", _httpWrapper.GetElement("input", "id", "txtDiscount").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("meeDiscount_ClientState", "");
                    dctPostDataValues.Add("gvTax%24ctl03%24txtDescriptionAdd", "");
                    dctPostDataValues.Add("gvTax%24ctl03%24ucTaxTypeAdd%24rblValuePercentage", "2");
                    dctPostDataValues.Add("gvTax%24ctl03%24txtValueAdd", "");
                    dctPostDataValues.Add("gvTax%24ctl03%24MaskedEditTotalPayableAmout_ClientState", "");
                    dctPostDataValues.Add("isouterpage", "");
                    dctPostDataValues.Add("txtnopage", "");
                    if (PostURL("span", "id", "Title1_lblTitle"))
                    {
                        #region get URL
                        List<HtmlNode> lstScripts = _httpWrapper._CurrentDocument.DocumentNode.Descendants()
                      .Where(n => n.Name == "script")
                      .ToList();
                        foreach (HtmlNode a in lstScripts)
                        {
                            if (a.InnerText.Trim().Contains("javascript:parent.Openpopup") && a.InnerText.Contains("PurchaseQuotationDeclineQuote.aspx?QUOTATIONID"))
                            {
                                int startIdx = a.InnerText.Trim().IndexOf("PurchaseQuotationDeclineQuote") + 2;
                                int _length = (a.InnerText.Trim().Length - 6) - startIdx;
                                URL = a.InnerText.ToString().Substring(startIdx, _length).Trim();
                                result = true;
                                break;
                            }
                        }
                        #endregion
                    }
                }

                if (result)
                {
                    IsUrlEncoded = false;
                    URL = "https://apps.southnests.com/ArchangelReports/Purchase/" + URL;
                    if (LoadURL("form", "id", "frmDeliveryInstruction"))
                    {

                        HtmlNode _lblDecline = _httpWrapper.GetElement("span", "id", "Title1_lblTitle");
                        if (_lblDecline != null)
                        {
                            if (_lblDecline.InnerText.Trim().ToUpper() == "DECLINE QUOTE")
                            {
                                _httpWrapper._SetRequestHeaders[HttpRequestHeader.CacheControl] = "max-age=0";
                                if (!_httpWrapper._AddRequestHeaders.ContainsKey("Upgrade-Insecure-Requests"))
                                    _httpWrapper._AddRequestHeaders.Add("Upgrade-Insecure-Requests", @"1");
                                _httpWrapper.AcceptMimeType = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                                _httpWrapper.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";

                                dctPostDataValues.Clear();
                                dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", "");
                                dctPostDataValues.Add("__EVENTTARGET", "MenuDeclineQuote%24dlstTabs%24ctl00%24btnMenu");
                                dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
                                dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                                dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                                dctPostDataValues.Add("__EVENTVALIDATION", _httpWrapper._dctStateData["__EVENTVALIDATION"]);
                                dctPostDataValues.Add("txtDeclineQuote", Uri.EscapeDataString(SupplierComment.Trim()));
                                if (PostURL("span", "id", "ucStatus_litMessage"))
                                {
                                    HtmlNode _successMsg = _httpWrapper.GetElement("span", "id", "ucStatus_litMessage");
                                    if (_successMsg.InnerText.Trim().ToUpper() == "DATA HAS BEEN SAVED")
                                        result = true;
                                    else result = false;

                                }
                                IsUrlEncoded = true;
                            }
                            else
                            {
                                throw new Exception("Unable to open decline quote form");
                            }
                        }
                        else
                        {
                            throw new Exception("Unable to open decline quote form");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Write_Quote_ErrorLog("Unable to decline quote '" + UCRefNo + "'." + ex.Message, MTML_File); return false;
            }
            return result;
        }

        private void SendMailNotification(MTMLInterchange _interchange, string DocType, string VRNO, string ActionType, string Message)
        {
            try
            {
                string MailFromDefault = convert.ToString(ConfigurationManager.AppSettings["MAIL_FROM"]);
                string MailBccDefault = convert.ToString(ConfigurationManager.AppSettings["MAIL_BCC"]);
                string MailCcDefault = convert.ToString(ConfigurationManager.AppSettings["MAIL_CC"]);

                string BuyerCode = convert.ToString(_interchange.Recipient).Trim();
                string SuppCode = convert.ToString(_interchange.Sender).Trim();
                string BuyerID = convert.ToString(_interchange.BuyerSuppInfo.BuyerID).Trim();
                string SupplierID = convert.ToString(_interchange.BuyerSuppInfo.SupplierID).Trim();

                string MailAuditPath = convert.ToString(ConfigurationManager.AppSettings["MAIL_AUDIT_PATH"]);
                if (MailAuditPath.Trim() != "")
                {
                    if (!Directory.Exists(MailAuditPath.Trim())) Directory.CreateDirectory(MailAuditPath.Trim());
                }
                else throw new Exception("MAIL_AUDIT_PATH value is not defined in config file.");

                string MailSettings = convert.ToString(ConfigurationManager.AppSettings[SuppCode + "-" + BuyerCode]);
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

                    string attachmentFile = convert.ToString(_interchange.DocumentHeader.OriginalFile);
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

                    #region // NOTIFY TO BUYER //
                    if (notifyBuyer == "YES")
                    {
                        // Send Mail Notification for Buyer
                        string MailFrom = "", MailTo = ByrMailID.Trim().Replace("E-mail:", "").Trim(), mailBody = "";

                        if (MailTo.Trim() != "")
                        {
                            int QuotationID = convert.ToInt(_interchange.BuyerSuppInfo.RecordID);
                            if (useDefaultFromMailID.Trim() == "YES")
                            {
                                MailFrom = MailFromDefault.Trim();
                            }
                            else
                            {
                                MailFrom = byrFromMailID.Trim();
                            }

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
                            #endregion

                            #region // Set MailBody //
                            mailBody = File.ReadAllText(MailBodyTemplate);
                            mailBody = mailBody.Replace("#DOC_TYPE#", DocType.Trim().ToUpper());
                            mailBody = mailBody.Replace("#SENDER#", SenderName.Trim());
                            mailBody = mailBody.Replace("#BUYER_NAME#", RecipientName.Trim());
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
                                "0" + "|" + // Send Html Msg
                                "0"; // Use Html File Msg

                            // Write To File                            
                            File.WriteAllText(MailAuditPath + "\\MailNotify_" + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".txt", mailText.Trim());
                            LogText = "Mail Send to Buyer Email -" + MailTo.Trim() + " .";
                        }
                        else
                        {
                            LogText = "Unable to send mail notification to buyer; Buyer Mailid is empty.";
                        }
                    }
                    #endregion

                    #region // NOTIFY TO SUPPLIER //
                    if (notifySupp == "YES")
                    {
                        // Send Mail Notification for Supplier
                        string MailFrom = MailFromDefault, MailTo = SuppMailID.Trim().Replace("E-mail:", "").Trim(), mailBody = "";

                        if (MailTo.Trim() != "")
                        {
                            if (SuppLinkMailID.Trim() != "") MailTo = SuppLinkMailID.Trim();

                            int QuotationID = convert.ToInt(_interchange.BuyerSuppInfo.RecordID);

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
                            #endregion

                            #region // Set MailBody //
                            mailBody = File.ReadAllText(MailBodyTemplate);
                            mailBody = mailBody.Replace("#DOC_TYPE#", DocType.Trim().ToUpper());
                            mailBody = mailBody.Replace("#SENDER#", SenderName.Trim());
                            mailBody = mailBody.Replace("#BUYER_NAME#", RecipientName.Trim());
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
                                "0" + "|" + // Send Html Msg
                                "0"; // Use Html File Msg

                            // Write To File
                            File.WriteAllText(MailAuditPath + "\\MailNotify_" + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".txt", mailText.Trim());
                            LogText = "Mail Send to Supplier Email -" + MailTo.Trim() + " .";
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
                LogText = "Unable to create Mail notification template. Error : " + ex.StackTrace;
            }
        }

        public string GetAttachment()
        {
            try
            {
                string filename = "";
                this.URL = currentURL;
                LoadURL("a", "id", "MenuRegistersStockItem_dlstTabs_ctl02_btnMenu");
                HtmlNode _report = _httpWrapper.GetElement("a", "id", "MenuRegistersStockItem_dlstTabs_ctl02_btnMenu");
                if (_report != null)
                {
                    #region Commented as Report.aspx page is not getting load
                    //string _clickevent = _report.GetAttributeValue("onclick", "").Replace("'); return false;", "").Trim();
                    //string[] _arrClick = _clickevent.Split(',');
                    //this.URL = HttpUtility.HtmlDecode(_arrClick[_arrClick.Length - 1].Trim('\'').Replace("../", "https://apps.southnests.com/ArchangelReports/"));
                    //try
                    //{
                    //    if (LoadURL("a", "id", "OrderExportToPDF_dlstTabs_ctl02_btnMenu"))
                    //    {
                    //        HtmlNode _form1 = _httpWrapper.GetElement("form", "id", "form1");
                    //        if (_form1 != null)
                    //        {
                    //            URL = "https://apps.southnests.com/ArchangelReports/Reports/" + _form1.GetAttributeValue("action", "").Trim();
                    //            //if (LoadURL("", "", ""))
                    //            //{
                    //            if (_httpWrapper._dctStateData.Count > 0)
                    //            {
                    //                HtmlNode _select = _httpWrapper.GetElement("select", "name", "CrystalReportViewer1$ctl02$ctl11");
                    //                HtmlNode _select15 = _httpWrapper.GetElement("select", "name", "CrystalReportViewer1$ctl02$ctl15");
                    //                dctPostDataValues.Clear();
                    //                dctPostDataValues.Add("ToolkitScriptManager1", "pnlOrderForm%7COrderExportToPDF%24dlstTabs%24ctl02%24btnMenu");
                    //                dctPostDataValues.Add("__EVENTTARGET", "OrderExportToPDF%24dlstTabs%24ctl02%24btnMenu");
                    //                dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
                    //                dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", _httpWrapper._dctStateData["ToolkitScriptManager1_HiddenField"]);
                    //                dctPostDataValues.Add("__LASTFOCUS", _httpWrapper._dctStateData["__LASTFOCUS"]);
                    //                dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                    //                dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                    //                dctPostDataValues.Add("__EVENTVALIDATION", _httpWrapper._dctStateData["__EVENTVALIDATION"]);
                    //                dctPostDataValues.Add("Title1%24chkShowMenu", "on");
                    //                dctPostDataValues.Add("CrystalReportViewer1%24ctl02%24ctl09", "");
                    //                if (_select != null)
                    //                {
                    //                    var options = _select.SelectNodes(".//option[@selected='selected']");
                    //                    if (options.Count == 1)
                    //                    {
                    //                        string _value = (options[0]).GetAttributeValue("value", "").Trim();
                    //                        dctPostDataValues.Add("CrystalReportViewer1%24ctl02%24ctl11", Uri.EscapeDataString(_value));
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    dctPostDataValues.Add("CrystalReportViewer1%24ctl02%24ctl11", "AAEAAAD%2F%2F%2F%2F%2FAQAAAAAAAAAEAQAAABxTeXN0ZW0uQ29sbGVjdGlvbnMuSGFzaHRhYmxlBwAAAApMb2FkRmFjdG9yB1ZlcnNpb24IQ29tcGFyZXIQSGFzaENvZGVQcm92aWRlcghIYXNoU2l6ZQRLZXlzBlZhbHVlcwAAAwMABQULCBxTeXN0ZW0uQ29sbGVjdGlvbnMuSUNvbXBhcmVyJFN5c3RlbS5Db2xsZWN0aW9ucy5JSGFzaENvZGVQcm92aWRlcgjsUTg%2FAwAAAAoKCwAAAAkCAAAACQMAAAAQAgAAAAMAAAAGBAAAAApQYWdlTnVtYmVyBgUAAAAOTGFzdFBhZ2VOdW1iZXIGBgAAABVJc0xhc3RQYWdlTnVtYmVyS25vd24QAwAAAAMAAAAICAEAAAAICAEAAAAIAQAL");
                    //                }
                    //                dctPostDataValues.Add("CrystalReportViewer1%24ctl02%24ctl13", "");
                    //                if (_select15 != null)
                    //                {
                    //                    var options = _select15.SelectNodes(".//option[@selected='selected']");
                    //                    if (options.Count == 1)
                    //                    {
                    //                        string _value = (options[0]).GetAttributeValue("value", "").Trim();
                    //                        dctPostDataValues.Add("CrystalReportViewer1%24ctl02%24ctl15", Uri.EscapeDataString(_value));
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    dctPostDataValues.Add("CrystalReportViewer1%24ctl02%24ctl15", "100");
                    //                }RFQ
                    //                dctPostDataValues.Add("__ASYNCPOST", "true");

                    //                _httpWrapper.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    //                if (_httpWrapper._AddRequestHeaders.Remove("X-Requested-With")) _httpWrapper._AddRequestHeaders.Add("X-Requested-With", @"XMLHttpRequest");
                    //                if (_httpWrapper._AddRequestHeaders.Remove("X-MicrosoftAjax")) _httpWrapper._AddRequestHeaders.Add("X-MicrosoftAjax", @"Delta=true");
                    //                if (!_httpWrapper._SetRequestHeaders.ContainsKey(HttpRequestHeader.CacheControl))
                    //                    _httpWrapper._SetRequestHeaders.Add(HttpRequestHeader.CacheControl, "no-cache");
                    //                _httpWrapper.AcceptMimeType = "*/*";
                    //                if (PostURL("", "", ""))
                    //                {
                    //                    string[] _getdata = _httpWrapper._CurrentResponseString.Trim('|').Split('|');
                    //                    this.URL = "https://apps.southnests.com/ArchangelReports/Reports/" + _getdata[_getdata.Length - 1];
                    //                    if (this.VRNO == "") this.VRNO = UCRefNo;
                    //                    filename = ImagePath + "\\" + this.DocType + "_" + this.SupplierCode + "_" + this.VRNO + "_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".pdf";
                    //                    bool _res = DownloadRFQ(this.URL, filename, "");
                    //                    if (_res) LogText = "Attachment dowwnloaded successfully.";
                    //                    else LogText = "Unable to download attachment.";
                    //                }
                    //                // }
                    //            }
                    //        }
                    //    }
                    //}
                    //catch (Exception ex)
                    //{ LogText = "Exception at GetAttachment : " + ex.Message; }
                    #endregion

                    if (this.VRNO == "") this.VRNO = UCRefNo;
                    filename = ImagePath + "\\" + this.DocType + "_" + this.SupplierCode + "_" + this.VRNO + "_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                    if (!PrintScreen(filename)) filename = "";
                }
                else
                {
                    if (this.VRNO == "") this.VRNO = UCRefNo;
                    filename = ImagePath + "\\" + this.DocType + "_" + this.SupplierCode + "_" + this.VRNO + "_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                    if (!PrintScreen(filename)) filename = "";
                }
                return filename;
            }
            catch (Exception ex)
            {
                LogText = "Exception at GetAttachment : " + ex.Message;
                return "";
            }
        }

        public void GetXmlFiles()
        {
            xmlFiles.Clear();
            DirectoryInfo _dir = new DirectoryInfo(QuotePOCPath);
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
                        AddDiscount = _interchange.DocumentHeader.AdditionalDiscount;

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

                        if (_interchange.DocumentHeader.TransportModeCode == TransportModeCode.Air) TransportMode = "344";
                        else if (_interchange.DocumentHeader.TransportModeCode == TransportModeCode.Maritime) TransportMode = "345";
                        else if (_interchange.DocumentHeader.TransportModeCode == TransportModeCode.Road) TransportMode = "346";
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
                            ImoNo = _interchange.DocumentHeader.PartyAddresses[j].Identification;
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
                            else if (_interchange.DocumentHeader.Comments[i].Qualifier == CommentTypes.ZTC)
                                TermsCond = _interchange.DocumentHeader.Comments[i].Value;
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
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.TaxCost_99)//16-12-2017
                                TaxCost = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
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
                                    // DateTime ExpDate = FormatMTMLDate(_interchange.DocumentHeader.DateTimePeriods[i].Value);
                                    //if (convert.ToDateTime(_interchange.DocumentHeader.DateTimePeriods[i].Value) != DateTime.MinValue)
                                    //{
                                    dtExpDate = _interchange.DocumentHeader.DateTimePeriods[i].Value.ToString();
                                    //dtExpDate = ExpDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);//15-3-18
                                    //  dtExpDate = ExpDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture);
                                    // }
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

        public void Write_Quote_ErrorLog(string AuditMsg, string _File)
        {
            LogText = AuditMsg;
            string eFile = ImagePath + "\\ArcMarine_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            _httpWrapper.GetElement("div", "id", "navigation").Attributes["class"].Remove();

            _CurrentResponseString = _httpWrapper._CurrentDocument.DocumentNode.OuterHtml;
            if (!PrintScreen(eFile)) eFile = "";
            if (VRNO == "") VRNO = UCRefNo;
            CreateAuditFile(eFile, "Arc_Http_" + this.DocType, VRNO, "Error", AuditMsg, BuyerCode, SupplierCode, AuditPath);

            if (!Directory.Exists(QuotePOCPath + "\\Error")) Directory.CreateDirectory(QuotePOCPath + "\\Error");
            File.Move(QuotePOCPath + "\\" + Path.GetFileName(_File), QuotePOCPath + "\\Error\\" + Path.GetFileName(_File));
        }

        #endregion

        //private void Getnextpage()
        //{
        //    HtmlAgilityPack.HtmlDocument _htmlDoc = null;
        //    if (dctPostDataValues != null && dctPostDataValues.Count > 0)
        //    {
        //        dctPostDataValues["__EVENTTARGET"]= "cmdNext";
        //        if (PostURL("input", "id", "txtEnquiryNumber"))
        //        {
        //            _htmlDoc = _nxtDoc = _httpWrapper._CurrentDocument;
        //        }
        //    }
        //}


        private bool Request_apps2_southnests_com( string dctPost_DataValues)
        {
            HttpWebResponse response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(cQuoteURL);

                request.KeepAlive = true;
                request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0");
                request.Headers.Add("Origin", @"https://apps2.southnests.com");
                request.Headers.Add("Upgrade-Insecure-Requests", @"1");
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36";
                request.Headers.Add("Sec-Fetch-Mode", @"navigate");
                request.Headers.Add("Sec-Fetch-User", @"?1");
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
                request.Headers.Add("Sec-Fetch-Site", @"same-origin");
                request.Referer = cQuoteURL;
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");
                request.Headers.Set(HttpRequestHeader.Cookie, @"ASP.NET_SessionId=534y0jmdt5xbbxoqswady4nu");

                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;

                dctPostDataValues["__EVENTTARGET"] = "cmdNext";

               // string body1 = @"ToolkitScriptManager1_HiddenField=&__EVENTTARGET=cmdNext&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=%2FwEPDwUJMjk0NTE3NTg0DxYaHglTVE9DS1RZUEUFBVNUT1JFHg5DVVNUT01FUkhFQURFUgUhR0lNQVMgU0hJUCBTVVBQTFkgJiBTRVJWSUNFUyBCLlYuHgpQQUdFTlVNQkVSAgEeDlNPUlRFWFBSRVNTSU9OZB4IVkVTU0VMSUQFATAeDlRPVEFMUEFHRUNPVU5UAgIeCFJPV0NPVU5UAp8BHg1TT1JURElSRUNUSU9OZB4HT1JERVJJRAUkOTc5MDE0NWEtMjliYi1lOTExLTgwZTktMDZhYmFkNjFiZTk3HgVEVEtFWQUkZDJhMTFmYjctODNiZC1lOTExLTgwZTktMDZhYmFkNjFiZTk3HhBXRUJTRVNTSU9OU1RBVFVTBQFZHgtxdW90YXRpb25pZAUkRDFBMTFGQjctODNCRC1FOTExLTgwRTktMDZBQkFENjFCRTk3HgxDVVJSRU5USU5ERVgCARYCAgMPZBYUAgMPZBYEAgEPEA8WAh4HQ2hlY2tlZGcWAh4Hb25jbGljawUbamF2YXNjcmlwdDpSZXNpemVNZW51KHRoaXMpZGRkAgUPDxYCHgRUZXh0BSBRdW90YXRpb24gRGV0YWlscyBbRU4tMTktMDEwODM1XWRkAgQPZBYCAgEPFCsACQ8WBh4IRGF0YUtleXMWAB4LXyFJdGVtQ291bnQCBx4NU2VsZWN0ZWRJbmRleGZkFgYeCUJhY2tDb2xvcgpmHglGb3JlQ29sb3IKpAEeBF8hU0ICDBYEHxMKZh8VAghkZGRkZGQWDmYPZBYEAgEPDxYMHgdUb29sVGlwBQRTYXZlHw8FBFNhdmUeBVdpZHRoGwAAAAAAAFRAAQAAAB4LQ29tbWFuZE5hbWUFBFNBVkUfFQKAAh4HVmlzaWJsZWcWAh8OBbMBamF2YXNjcmlwdDp0aGlzLm9uY2xpY2s9ZnVuY3Rpb24oKXtpZiAoZG9jdW1lbnQuZ2V0RWxlbWVudEJ5SWQoJ2xibFByb2dyZXNzJykgIT0gbnVsbCkgZG9jdW1lbnQuZ2V0RWxlbWVudEJ5SWQoJ2xibFByb2dyZXNzJykudmFsdWU9J1Byb2Nlc3Npbmcgbm93LiBQbGVhc2UgV2FpdCEnOyByZXR1cm4gZmFsc2U7fTtkAgMPDxYIHxcbAAAAAAAAVEABAAAAHxYFBFNhdmUfDwUEU2F2ZR8VAoACZGQCAQ9kFgQCAQ8PFgwfFgULU2VuZCB0byBBUkMfDwULU2VuZCB0byBBUkMfFxsAAAAAAABUQAEAAAAfGAUHQ09ORklSTR8VAoACHxlnFgIfDgWzAWphdmFzY3JpcHQ6dGhpcy5vbmNsaWNrPWZ1bmN0aW9uKCl7aWYgKGRvY3VtZW50LmdldEVsZW1lbnRCeUlkKCdsYmxQcm9ncmVzcycpICE9IG51bGwpIGRvY3VtZW50LmdldEVsZW1lbnRCeUlkKCdsYmxQcm9ncmVzcycpLnZhbHVlPSdQcm9jZXNzaW5nIG5vdy4gUGxlYXNlIFdhaXQhJzsgcmV0dXJuIGZhbHNlO307ZAIDDw8WCB8XGwAAAAAAAFRAAQAAAB8WBQtTZW5kIHRvIEFSQx8PBQtTZW5kIHRvIEFSQx8VAoACZGQCAg9kFgQCAQ8PFgwfFgUHRGV0YWlscx8PBQdEZXRhaWxzHxcbAAAAAAAAVEABAAAAHxgFB0RFVEFJTFMfFQKAAh8ZZxYCHw4FswFqYXZhc2NyaXB0OnRoaXMub25jbGljaz1mdW5jdGlvbigpe2lmIChkb2N1bWVudC5nZXRFbGVtZW50QnlJZCgnbGJsUHJvZ3Jlc3MnKSAhPSBudWxsKSBkb2N1bWVudC5nZXRFbGVtZW50QnlJZCgnbGJsUHJvZ3Jlc3MnKS52YWx1ZT0nUHJvY2Vzc2luZyBub3cuIFBsZWFzZSBXYWl0ISc7IHJldHVybiBmYWxzZTt9O2QCAw8PFggfFxsAAAAAAABUQAEAAAAfFgUHRGV0YWlscx8PBQdEZXRhaWxzHxUCgAJkZAIDD2QWBAIBDw8WDB8WBQdSZW1hcmtzHw8FB1JlbWFya3MfFxsAAAAAAABUQAEAAAAfGAUHUkVNQVJLUx8VAoACHxlnFgIfDgWzAWphdmFzY3JpcHQ6dGhpcy5vbmNsaWNrPWZ1bmN0aW9uKCl7aWYgKGRvY3VtZW50LmdldEVsZW1lbnRCeUlkKCdsYmxQcm9ncmVzcycpICE9IG51bGwpIGRvY3VtZW50LmdldEVsZW1lbnRCeUlkKCdsYmxQcm9ncmVzcycpLnZhbHVlPSdQcm9jZXNzaW5nIG5vdy4gUGxlYXNlIFdhaXQhJzsgcmV0dXJuIGZhbHNlO307ZAIDDw8WCB8XGwAAAAAAAFRAAQAAAB8WBQdSZW1hcmtzHw8FB1JlbWFya3MfFQKAAmRkAgQPZBYEAgEPDxYIHxYFFERlbGl2ZXJ5IEluc3RydWN0aW9uHw8FFERlbGl2ZXJ5IEluc3RydWN0aW9uHxgFE0RFTElWRVJZSU5TVFJVQ1RJT04fGWcWAh8OBbMBamF2YXNjcmlwdDp0aGlzLm9uY2xpY2s9ZnVuY3Rpb24oKXtpZiAoZG9jdW1lbnQuZ2V0RWxlbWVudEJ5SWQoJ2xibFByb2dyZXNzJykgIT0gbnVsbCkgZG9jdW1lbnQuZ2V0RWxlbWVudEJ5SWQoJ2xibFByb2dyZXNzJykudmFsdWU9J1Byb2Nlc3Npbmcgbm93LiBQbGVhc2UgV2FpdCEnOyByZXR1cm4gZmFsc2U7fTtkAgMPDxYIHxcbAAAAAADAYkABAAAAHxYFFERlbGl2ZXJ5IEluc3RydWN0aW9uHw8FFERlbGl2ZXJ5IEluc3RydWN0aW9uHxUCgAJkZAIFD2QWBAIBDw8WDB8WBQpBdHRhY2htZW50Hw8FCkF0dGFjaG1lbnQfFxsAAAAAAABUQAEAAAAfGAUKQVRUQUNITUVOVB8VAoACHxlnFgIfDgWzAWphdmFzY3JpcHQ6dGhpcy5vbmNsaWNrPWZ1bmN0aW9uKCl7aWYgKGRvY3VtZW50LmdldEVsZW1lbnRCeUlkKCdsYmxQcm9ncmVzcycpICE9IG51bGwpIGRvY3VtZW50LmdldEVsZW1lbnRCeUlkKCdsYmxQcm9ncmVzcycpLnZhbHVlPSdQcm9jZXNzaW5nIG5vdy4gUGxlYXNlIFdhaXQhJzsgcmV0dXJuIGZhbHNlO307ZAIDDw8WCB8XGwAAAAAAAFRAAQAAAB8WBQpBdHRhY2htZW50Hw8FCkF0dGFjaG1lbnQfFQKAAmRkAgYPZBYEAgEPDxYIHxYFDURlY2xpbmUgUXVvdGUfDwUNRGVjbGluZSBRdW90ZR8YBQxERUNMSU5FUVVPVEUfGWcWAh8OBbMBamF2YXNjcmlwdDp0aGlzLm9uY2xpY2s9ZnVuY3Rpb24oKXtpZiAoZG9jdW1lbnQuZ2V0RWxlbWVudEJ5SWQoJ2xibFByb2dyZXNzJykgIT0gbnVsbCkgZG9jdW1lbnQuZ2V0RWxlbWVudEJ5SWQoJ2xibFByb2dyZXNzJykudmFsdWU9J1Byb2Nlc3Npbmcgbm93LiBQbGVhc2UgV2FpdCEnOyByZXR1cm4gZmFsc2U7fTtkAgMPDxYIHxcbAAAAAADAYkABAAAAHxYFDURlY2xpbmUgUXVvdGUfDwUNRGVjbGluZSBRdW90ZR8VAoACZGQCBQ8PZBYCHgVzdHlsZQURdmlzaWJpbGl0eTpoaWRkZW5kAgYPZBYCZg9kFjYCAQ8PFgIeBUVSUk9SaGQWAmYPD2QWAh8aBXVwb3NpdGlvbjphYnNvbHV0ZTtsZWZ0OjM1MHB4O2ZpbHRlcjphbHBoYShvcGFjaXR5PTk1KTstbW96LW9wYWNpdHk6Ljk1O29wYWNpdHk6Ljk1O3otaW5kZXg6OTk7YmFja2dyb3VuZC1jb2xvcjpXaGl0ZTtkAgUPZBYGAgEPZBYCAgEPDxYCHw8FBkFMSEFOSWRkAgMPZBYCAgEPDxYCHw8FBzkzMzExNTNkZAIFD2QWAgIBDw8WAh8PBQQxNTkzZGQCBw9kFgQCAQ9kFgICAQ8PFgIfDwUlQUZSQU1BWCAoQ1JVREUtIDgwLDAwMCAtIDExOSwwMDAgRFdUKWRkAgUPZBYCAgEPDxYCHw9kZGQCCw8PFgIfDwUMRU4tMTktMDEwODM1ZGQCDQ8PFgIfDwUBLWRkAg8PDxYCHw8FCTY4NDIyODIyIGRkAhEPDxYCHw8FDUtBSkFMIEFEQUdBTEVkZAITDw8WAh8PBQoxMy8wOC8yMDE5ZGQCFQ8PFgIfDwUXc3RvcmVtdW0yQGFyY21hcmluZS5jb21kZAIXDw8WAh8PBSFHSU1BUyBTSElQIFNVUFBMWSAmIFNFUlZJQ0VTIEIuVi5kZAIZDw8WAh8PBRczMTEwMzAyNzgyMCAzMTYxNTI4NjU3NWRkAhsPDxYCHw8FASxkZAIdDw8WAh8PZWRkAh8PDxYCHw8FE3JvdHRlcmRhbUBnaW1hcy5jb21kZAInDw8WAh8PZGRkAikPDxYCHw8FBk5PUk1BTGRkAisPZBYCZg8QDxYCHgtfIURhdGFCb3VuZGdkEBUICi0tU2VsZWN0LS0dQ0FSUklBR0UsIElOU1VSQU5DRSAmIEZSRUlHSFQIRVgtU1RPQ0sIRVgtV0hBUkYIRVgtV09SS1MNRlJFRSBPTiBCT0FSRA5OT1QgQVBQTElDQUJMRQZPVEhFUlMVCAVEdW1teQI3OQM2NDMCNzgDNDA0Ajc3AzY0NQMyOTcUKwMIZ2dnZ2dnZ2cWAWZkAi0PZBYCZg8QDxYCHxxnZBAVCgotLVNlbGVjdC0tD0FEVkFOQ0UgUEFZTUVOVBBDQVNIIE9OIERFTElWRVJZC05FVCAxNCBEQVlTC05FVCAzMCBEQVlTC05FVCA0NSBEQVlTC05FVCA2MCBEQVlTC05FVCA5MCBEQVlTDk5PVCBBUFBMSUNBQkxFBU9USEVSFQoFRHVtbXkCNzYCNjkDNjY4Ajc0AzI5OAI3NQMyOTkDNjQ2AzMwMBQrAwpnZ2dnZ2dnZ2dnFgFmZAIvD2QWAmYPDxYCHw8FATAWBB4Hb25QYXN0ZQUNcmV0dXJuIGZhbHNlOx4Jb25rZXlkb3duBTFyZXR1cm4gdHh0a2V5cHJlc3MoZXZlbnQsIHRoaXMsIHRydWUsbnVsbCxmYWxzZSk7ZAIxD2QWAmYPEA8WAh8cZ2QQFQYKLS1TZWxlY3QtLQ5CeSBBaXIgRnJlaWdodApCeSBDb3VyaWVyC0J5IFJvYWR3YXlzBkJ5IFNlYQ5Ob3QgQXBwbGljYWJsZRUGBUR1bW15AzM0NAMzNDcDMzQ2AzM0NQM2NDQUKwMGZ2dnZ2dnFgFmZAI1D2QWAmYPEA8WAh8cZ2QQFQMKLS1TZWxlY3QtLQRIaWdoBk1lZGl1bRUDBUR1bW15BDE0MzQEMTQzMxQrAwNnZ2cWAQIBZAI3D2QWAmYPEA8WAh8cZ2QQFRQKLS1TZWxlY3QtLQNBRUQDQVVEA0NBRANERU0DRVVSA0dCUANJTlIDSlBZA0tSVwNLV0QDTVlSA05BRANOT0sDTlpEA1BHSwNTRUsDU0dEA1VTRANaQVIVFAVEdW1teQExAjE5AjM3AjUzATIDMTY5ATQBNQMxNDICODcBNgMxMDkDMTE3AzExMwMxMjIDMTQ3ATkCMTADMTQxFCsDFGdnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnFgECBWQCOQ9kFgJmDw8WAh8PBQQwLjAwFgQfHQUNcmV0dXJuIGZhbHNlOx8eBS9yZXR1cm4gdHh0a2V5cHJlc3MoZXZlbnQsIHRoaXMsIGZhbHNlLCAyLHRydWUpO2QCOw8PFgIfDwUINiw1MDIuNzRkZAI%2FD2QWAmYPDxYCHw8FBDAuMDAWBB8dBQ1yZXR1cm4gZmFsc2U7Hx4FL3JldHVybiB0eHRrZXlwcmVzcyhldmVudCwgdGhpcywgZmFsc2UsIDIsdHJ1ZSk7ZAJBDw8WAh8PBQg1LDUyNy4zM2RkAkUPDxYCHw8FBDAuMDBkZAIHD2QWAgIBDxQrAAkPFgYfEBYAHxECAx8SZmQWBh8TCmYfFAqkAR8VAgwWBB8TCmYfFQIIZGRkZGRkFgZmD2QWBAIBDw8WDB8WBQ9FeHBvcnQgdG8gRXhjZWweC1Bvc3RCYWNrVXJsBVQuLi9QdXJjaGFzZS9QdXJjaGFzZVF1b3RhdGlvblJGUS5hc3B4P1NFU1NJT05JRD1EMUExMUZCNy04M0JELUU5MTEtODBFOS0wNkFCQUQ2MUJFOTcfFxsAAAAAAIBBQAEAAAAfGAUFRXhjZWwfFQKAAh8ZZxYCHxoFggFiYWNrZ3JvdW5kLWltYWdlOnVybCgnL0FyY2hhbmdlbFJlcG9ydHMvY3NzL1RoZW1lMS9pbWFnZXMvaWNvbl94bHMucG5nJyk7IGJhY2tncm91bmQtcmVwZWF0Om5vLXJlcGVhdDsgYmFja2dyb3VuZC1wb3NpdGlvbjpjZW50ZXI7ZAIDDw8WBh8XGwAAAAAAwGJAAQAAAB8WBQ9FeHBvcnQgdG8gRXhjZWwfFQKAAmRkAgEPZBYEAgEPDxYIHxYFClByaW50IEdyaWQfFxsAAAAAAIBBQAEAAAAfFQKAAh8ZZxYEHw4FKmphdmFzY3JpcHQ6Q2FsbFByaW50KCdndlZlbmRvckl0ZW0nLCB0cnVlKR8aBYQBYmFja2dyb3VuZC1pbWFnZTp1cmwoJy9BcmNoYW5nZWxSZXBvcnRzL2Nzcy9UaGVtZTEvaW1hZ2VzL2ljb25fcHJpbnQucG5nJyk7IGJhY2tncm91bmQtcmVwZWF0Om5vLXJlcGVhdDsgYmFja2dyb3VuZC1wb3NpdGlvbjpjZW50ZXI7ZAIDDw8WBh8XGwAAAAAAwGJAAQAAAB8WBQpQcmludCBHcmlkHxUCgAJkZAICD2QWBAIBDw8WCh8WBRBCdWxrIFByaWNlIElucHV0HxcbAAAAAACAQUABAAAAHxgFCEJVTEtTQVZFHxUCgAIfGWcWBB8OBYEBamF2YXNjcmlwdDpPcGVucG9wdXAoJ2NvZGVoZWxwMScsJycsJ1B1cmNoYXNlUXVvdGF0aW9uUkZRTGluZUl0ZW1CdWxrU2F2ZS5hc3B4P3F1b3RhdGlvbmlkPUQxQTExRkI3LTgzQkQtRTkxMS04MEU5LTA2QUJBRDYxQkU5NycpHxoFgwFiYWNrZ3JvdW5kLWltYWdlOnVybCgnL0FyY2hhbmdlbFJlcG9ydHMvY3NzL1RoZW1lMS9pbWFnZXMvYnVsa19zYXZlLnBuZycpOyBiYWNrZ3JvdW5kLXJlcGVhdDpuby1yZXBlYXQ7IGJhY2tncm91bmQtcG9zaXRpb246Y2VudGVyO2QCAw8PFgYfFxsAAAAAAMBiQAEAAAAfFgUQQnVsayBQcmljZSBJbnB1dB8VAoACZGQCCQ8PFgIfDwUGUGFnZSAxZGQCCg8PFgIfDwUNIG9mIDIgUGFnZXMuIGRkAgsPDxYCHw8FEygxNTkgcmVjb3JkcyBmb3VuZClkZAIMDw8WAh4HRW5hYmxlZGhkZAIQDw8WBB8baB8ZaGRkGAMFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxaTAwUTZ3ZUYXgkY3RsMDIkY21kRWRpdAUVZ3ZUYXgkY3RsMDIkY21kRGVsZXRlBRJndlRheCRjdGwwMyRjbWRBZGQFIGd2VmVuZG9ySXRlbSRjdGwwMyRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsMDMkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGwwMyRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsMDMkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsMDQkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDA0JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsMDQkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDA0JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDA1JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGwwNSRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDA1JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGwwNSRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGwwNiRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsMDYkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGwwNiRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsMDYkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsMDckY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDA3JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsMDckY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDA3JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDA4JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGwwOCRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDA4JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGwwOCRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGwwOSRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsMDkkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGwwOSRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsMDkkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsMTAkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDEwJGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsMTAkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDEwJGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDExJGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGwxMSRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDExJGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGwxMSRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGwxMiRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsMTIkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGwxMiRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsMTIkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsMTMkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDEzJGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsMTMkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDEzJGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDE0JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGwxNCRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDE0JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGwxNCRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGwxNSRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsMTUkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGwxNSRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsMTUkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsMTYkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDE2JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsMTYkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDE2JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDE3JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGwxNyRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDE3JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGwxNyRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGwxOCRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsMTgkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGwxOCRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsMTgkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsMTkkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDE5JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsMTkkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDE5JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDIwJGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGwyMCRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDIwJGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGwyMCRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGwyMSRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsMjEkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGwyMSRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsMjEkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsMjIkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDIyJGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsMjIkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDIyJGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDIzJGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGwyMyRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDIzJGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGwyMyRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGwyNCRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsMjQkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGwyNCRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsMjQkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsMjUkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDI1JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsMjUkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDI1JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDI2JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGwyNiRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDI2JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGwyNiRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGwyNyRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsMjckaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGwyNyRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsMjckY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsMjgkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDI4JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsMjgkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDI4JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDI5JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGwyOSRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDI5JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGwyOSRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGwzMCRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsMzAkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGwzMCRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsMzAkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsMzEkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDMxJGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsMzEkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDMxJGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDMyJGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGwzMiRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDMyJGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGwzMiRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGwzMyRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsMzMkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGwzMyRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsMzMkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsMzQkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDM0JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsMzQkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDM0JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDM1JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGwzNSRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDM1JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGwzNSRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGwzNiRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsMzYkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGwzNiRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsMzYkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsMzckY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDM3JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsMzckY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDM3JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDM4JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGwzOCRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDM4JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGwzOCRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGwzOSRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsMzkkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGwzOSRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsMzkkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsNDAkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDQwJGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsNDAkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDQwJGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDQxJGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw0MSRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDQxJGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw0MSRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw0MiRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsNDIkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw0MiRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsNDIkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsNDMkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDQzJGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsNDMkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDQzJGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDQ0JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw0NCRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDQ0JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw0NCRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw0NSRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsNDUkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw0NSRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsNDUkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsNDYkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDQ2JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsNDYkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDQ2JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDQ3JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw0NyRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDQ3JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw0NyRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw0OCRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsNDgkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw0OCRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsNDgkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsNDkkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDQ5JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsNDkkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDQ5JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDUwJGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw1MCRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDUwJGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw1MCRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw1MSRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsNTEkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw1MSRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsNTEkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsNTIkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDUyJGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsNTIkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDUyJGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDUzJGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw1MyRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDUzJGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw1MyRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw1NCRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsNTQkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw1NCRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsNTQkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsNTUkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDU1JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsNTUkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDU1JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDU2JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw1NiRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDU2JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw1NiRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw1NyRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsNTckaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw1NyRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsNTckY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsNTgkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDU4JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsNTgkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDU4JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDU5JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw1OSRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDU5JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw1OSRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw2MCRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsNjAkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw2MCRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsNjAkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsNjEkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDYxJGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsNjEkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDYxJGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDYyJGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw2MiRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDYyJGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw2MiRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw2MyRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsNjMkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw2MyRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsNjMkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsNjQkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDY0JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsNjQkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDY0JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDY1JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw2NSRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDY1JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw2NSRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw2NiRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsNjYkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw2NiRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsNjYkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsNjckY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDY3JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsNjckY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDY3JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDY4JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw2OCRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDY4JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw2OCRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw2OSRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsNjkkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw2OSRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsNjkkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsNzAkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDcwJGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsNzAkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDcwJGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDcxJGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw3MSRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDcxJGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw3MSRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw3MiRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsNzIkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw3MiRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsNzIkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsNzMkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDczJGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsNzMkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDczJGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDc0JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw3NCRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDc0JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw3NCRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw3NSRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsNzUkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw3NSRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsNzUkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsNzYkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDc2JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsNzYkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDc2JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDc3JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw3NyRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDc3JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw3NyRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw3OCRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsNzgkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw3OCRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsNzgkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsNzkkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDc5JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsNzkkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDc5JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDgwJGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw4MCRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDgwJGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw4MCRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw4MSRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsODEkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw4MSRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsODEkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsODIkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDgyJGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsODIkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDgyJGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDgzJGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw4MyRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDgzJGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw4MyRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw4NCRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsODQkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw4NCRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsODQkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsODUkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDg1JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsODUkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDg1JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDg2JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw4NiRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDg2JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw4NiRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw4NyRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsODckaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw4NyRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsODckY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsODgkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDg4JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsODgkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDg4JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDg5JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw4OSRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDg5JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw4OSRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw5MCRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsOTAkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw5MCRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsOTAkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsOTEkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDkxJGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsOTEkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDkxJGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDkyJGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw5MiRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDkyJGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw5MiRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw5MyRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsOTMkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw5MyRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsOTMkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsOTQkY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDk0JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsOTQkY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDk0JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDk1JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw5NSRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDk1JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw5NSRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw5NiRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsOTYkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw5NiRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsOTYkY21kU3RvcmVJdGVtRGV0YWlsBSBndlZlbmRvckl0ZW0kY3RsOTckY2hrSW5jbHVkZUdTVAUhZ3ZWZW5kb3JJdGVtJGN0bDk3JGltZ1ZlbmRvck5vdGVzBRpndlZlbmRvckl0ZW0kY3RsOTckY21kRWRpdAUlZ3ZWZW5kb3JJdGVtJGN0bDk3JGNtZFN0b3JlSXRlbURldGFpbAUgZ3ZWZW5kb3JJdGVtJGN0bDk4JGNoa0luY2x1ZGVHU1QFIWd2VmVuZG9ySXRlbSRjdGw5OCRpbWdWZW5kb3JOb3RlcwUaZ3ZWZW5kb3JJdGVtJGN0bDk4JGNtZEVkaXQFJWd2VmVuZG9ySXRlbSRjdGw5OCRjbWRTdG9yZUl0ZW1EZXRhaWwFIGd2VmVuZG9ySXRlbSRjdGw5OSRjaGtJbmNsdWRlR1NUBSFndlZlbmRvckl0ZW0kY3RsOTkkaW1nVmVuZG9yTm90ZXMFGmd2VmVuZG9ySXRlbSRjdGw5OSRjbWRFZGl0BSVndlZlbmRvckl0ZW0kY3RsOTkkY21kU3RvcmVJdGVtRGV0YWlsBSFndlZlbmRvckl0ZW0kY3RsMTAwJGNoa0luY2x1ZGVHU1QFImd2VmVuZG9ySXRlbSRjdGwxMDAkaW1nVmVuZG9yTm90ZXMFG2d2VmVuZG9ySXRlbSRjdGwxMDAkY21kRWRpdAUmZ3ZWZW5kb3JJdGVtJGN0bDEwMCRjbWRTdG9yZUl0ZW1EZXRhaWwFIWd2VmVuZG9ySXRlbSRjdGwxMDEkY2hrSW5jbHVkZUdTVAUiZ3ZWZW5kb3JJdGVtJGN0bDEwMSRpbWdWZW5kb3JOb3RlcwUbZ3ZWZW5kb3JJdGVtJGN0bDEwMSRjbWRFZGl0BSZndlZlbmRvckl0ZW0kY3RsMTAxJGNtZFN0b3JlSXRlbURldGFpbAUhZ3ZWZW5kb3JJdGVtJGN0bDEwMiRjaGtJbmNsdWRlR1NUBSJndlZlbmRvckl0ZW0kY3RsMTAyJGltZ1ZlbmRvck5vdGVzBRtndlZlbmRvckl0ZW0kY3RsMTAyJGNtZEVkaXQFJmd2VmVuZG9ySXRlbSRjdGwxMDIkY21kU3RvcmVJdGVtRGV0YWlsBQxndlZlbmRvckl0ZW0PPCsADAEIAgFkBQVndlRheA88KwAMAQgCAWQ6vSIjJgpRWn2nXoRNyYY%2FQYv0nQ%3D%3D&__VIEWSTATEGENERATOR=E6166CE7&__SCROLLPOSITIONX=0&__SCROLLPOSITIONY=3688&__PREVIOUSPAGE=0sqWw89hrx5RLcpghX8UjgvtPWxmNg4Y-NGiSYWmkJ3aGJM4reeB7byjMgH3vY0b86qOJSvhb16h85WT1kYZQn9NZXo-Rz7U4ZeKZJAQYX0EpK9Rqgrivkyt5m0J-P0sCULSfA2&txtEnquiryNumber=EN-19-010835&txtDeliveryPlace=-&txtContactNo=68422822+&txtSenderName=KAJAL+ADAGALE&txtSentDate=13%2F08%2F2019&txtSenderEmailId=storemum2%40arcmarine.com&txtVendorName=GIMAS+SHIP+SUPPLY+%26+SERVICES+B.V.&txtTelephone=31103027820+31615286575&txtFax=%2C&txtVendorAddress=&txtEmail=rotterdam%40gimas.com&txtVenderReference=19006967&txtOrderDate=13%2F09%2F2019&txtPriority=NORMAL&UCDeliveryTerms%24ddlQuick=Dummy&UCPaymentTerms%24ddlQuick=Dummy&txtDeliveryTime%24txtNumber=0&ucModeOfTransport%24ddlQuick=Dummy&ddlPreferedquality%24ddlHard=1434&ucCurrency%24ddlCurrency=2&txtSupplierDiscount%24txtNumber=0.00&txtPrice=6%2C502.74&txtDiscount%24txtNumber=0.00&txtTotalPrice=5%2C527.33&txtTotalDiscount=0.00&gvTax%24ctl03%24txtDescriptionAdd=&gvTax%24ctl03%24ucTaxTypeAdd%24rblValuePercentage=2&gvTax%24ctl03%24txtValueAdd%24txtNumber=&gvVendorItem%24ctl31%24chkIncludeGST=on&gvVendorItem%24ctl32%24chkIncludeGST=on&gvVendorItem%24ctl38%24chkIncludeGST=on&gvVendorItem%24ctl39%24chkIncludeGST=on&gvVendorItem%24ctl40%24chkIncludeGST=on&gvVendorItem%24ctl41%24chkIncludeGST=on&gvVendorItem%24ctl42%24chkIncludeGST=on&gvVendorItem%24ctl43%24chkIncludeGST=on&gvVendorItem%24ctl44%24chkIncludeGST=on&gvVendorItem%24ctl45%24chkIncludeGST=on&gvVendorItem%24ctl46%24chkIncludeGST=on&gvVendorItem%24ctl47%24chkIncludeGST=on&gvVendorItem%24ctl48%24chkIncludeGST=on&gvVendorItem%24ctl49%24chkIncludeGST=on&gvVendorItem%24ctl50%24chkIncludeGST=on&gvVendorItem%24ctl51%24chkIncludeGST=on&gvVendorItem%24ctl52%24chkIncludeGST=on&gvVendorItem%24ctl53%24chkIncludeGST=on&gvVendorItem%24ctl54%24chkIncludeGST=on&gvVendorItem%24ctl55%24chkIncludeGST=on&gvVendorItem%24ctl56%24chkIncludeGST=on&gvVendorItem%24ctl57%24chkIncludeGST=on&gvVendorItem%24ctl58%24chkIncludeGST=on&gvVendorItem%24ctl59%24chkIncludeGST=on&gvVendorItem%24ctl60%24chkIncludeGST=on&gvVendorItem%24ctl61%24chkIncludeGST=on&gvVendorItem%24ctl62%24chkIncludeGST=on&gvVendorItem%24ctl63%24chkIncludeGST=on&gvVendorItem%24ctl64%24chkIncludeGST=on&gvVendorItem%24ctl65%24chkIncludeGST=on&gvVendorItem%24ctl66%24chkIncludeGST=on&gvVendorItem%24ctl67%24chkIncludeGST=on&gvVendorItem%24ctl68%24chkIncludeGST=on&gvVendorItem%24ctl69%24chkIncludeGST=on&gvVendorItem%24ctl70%24chkIncludeGST=on&gvVendorItem%24ctl71%24chkIncludeGST=on&gvVendorItem%24ctl72%24chkIncludeGST=on&gvVendorItem%24ctl73%24chkIncludeGST=on&gvVendorItem%24ctl74%24chkIncludeGST=on&gvVendorItem%24ctl75%24chkIncludeGST=on&gvVendorItem%24ctl76%24chkIncludeGST=on&gvVendorItem%24ctl77%24chkIncludeGST=on&gvVendorItem%24ctl78%24chkIncludeGST=on&gvVendorItem%24ctl79%24chkIncludeGST=on&gvVendorItem%24ctl80%24chkIncludeGST=on&gvVendorItem%24ctl81%24chkIncludeGST=on&gvVendorItem%24ctl82%24chkIncludeGST=on&gvVendorItem%24ctl83%24chkIncludeGST=on&gvVendorItem%24ctl84%24chkIncludeGST=on&gvVendorItem%24ctl85%24chkIncludeGST=on&gvVendorItem%24ctl86%24chkIncludeGST=on&gvVendorItem%24ctl87%24chkIncludeGST=on&gvVendorItem%24ctl88%24chkIncludeGST=on&gvVendorItem%24ctl89%24chkIncludeGST=on&gvVendorItem%24ctl90%24chkIncludeGST=on&gvVendorItem%24ctl91%24chkIncludeGST=on&gvVendorItem%24ctl92%24chkIncludeGST=on&gvVendorItem%24ctl93%24chkIncludeGST=on&gvVendorItem%24ctl94%24chkIncludeGST=on&gvVendorItem%24ctl95%24chkIncludeGST=on&gvVendorItem%24ctl96%24chkIncludeGST=on&gvVendorItem%24ctl97%24chkIncludeGST=on&gvVendorItem%24ctl98%24chkIncludeGST=on&gvVendorItem%24ctl99%24chkIncludeGST=on&gvVendorItem%24ctl100%24chkIncludeGST=on&gvVendorItem%24ctl101%24chkIncludeGST=on&gvVendorItem%24ctl102%24chkIncludeGST=on&isouterpage=&txtnopage=";

                string body = strnxtpgdetails;
                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                request.ContentLength = postBytes.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
                stream.Close();

                response = (HttpWebResponse)request.GetResponse();
                Stream stream1 = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                string txt = sr.ReadToEnd();
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

        private HtmlAgilityPack.HtmlDocument GetNextPageItems_Quote(int nxt)
        {
            HtmlAgilityPack.HtmlDocument _htmlDoc = null;
            string cEventArguement = (!string.IsNullOrEmpty(cEventArg)) ? cEventArg : _httpWrapper._dctStateData["__EVENTARGUMENT"];
            string cViewStatedet = (!string.IsNullOrEmpty(cViewState)) ? cViewState : _httpWrapper._dctStateData["__VIEWSTATE"];
            string cViewStateGendet = (!string.IsNullOrEmpty(cViewStateGen)) ? cViewStateGen : _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"];
            string cScrollPst = (!string.IsNullOrEmpty(cScrollYPst)) ? cScrollYPst : _httpWrapper._dctStateData["__SCROLLPOSITIONY"];

            dctPostDataValues.Clear();
            dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", "");
            dctPostDataValues.Add("__EVENTTARGET", "cmdNext");
            dctPostDataValues.Add("__EVENTARGUMENT", cEventArguement);
            dctPostDataValues.Add("__LASTFOCUS", "");
            dctPostDataValues.Add("__VIEWSTATE", cViewStatedet);
            dctPostDataValues.Add("__VIEWSTATEGENERATOR", cViewStateGendet);
            dctPostDataValues.Add("__SCROLLPOSITIONX", "0");
            dctPostDataValues.Add("__SCROLLPOSITIONY", cScrollPst);

            if (_httpWrapper._dctStateData.ContainsKey("__PREVIOUSPAGE"))
            {
                dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);
            }
            else
            {
                if (!string.IsNullOrEmpty(cPrevPageData))
                {
                    dctPostDataValues.Add("__PREVIOUSPAGE", cPrevPageData);
                }
            }
            dctPostDataValues.Add("isouterpage", "");
            dctPostDataValues.Add("txtnopage", "");

            _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
            _httpWrapper._SetRequestHeaders[HttpRequestHeader.AcceptLanguage] = "en-IN";
            _httpWrapper._SetRequestHeaders[HttpRequestHeader.CacheControl] = "no-cache";

            if (PostURL("input", "id", "txtEnquiryNumber"))
            {
                _htmlDoc = _nxtDoc = _httpWrapper._CurrentDocument;
            }
            strnxtpgdetails = GetCurrentpage(_htmlDoc);
            return _htmlDoc;
        }

        private string GetCurrentpage(HtmlAgilityPack.HtmlDocument _htmlNxtDoc)
        {
            string _nxtpageDataitm = "";
            slnxtPages.Clear();
            slnxtPages.Add("ToolkitScriptManager1_HiddenField", "");
            slnxtPages.Add("__EVENTTARGET", "cmdNext");
            slnxtPages.Add("__LASTFOCUS", "");
            slnxtPages.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);

            slnxtPages.Add("__VIEWSTATE", Uri.EscapeDataString(HttpUtility.UrlDecode(_httpWrapper._dctStateData["__VIEWSTATE"])));
            slnxtPages.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
            slnxtPages.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);

            slnxtPages.Add("txtEnquiryNumber", _httpWrapper.GetElement("input", "id", "txtEnquiryNumber").GetAttributeValue("value", "").Trim());
            slnxtPages.Add("txtDeliveryPlace", _httpWrapper.GetElement("input", "id", "txtDeliveryPlace").GetAttributeValue("value", "").Trim());
            slnxtPages.Add("txtContactNo", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtContactNo").GetAttributeValue("value", "").Trim()).Replace("%20", "+"));
            slnxtPages.Add("txtSenderName", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSenderName").GetAttributeValue("value", "").Trim()).Replace("%20", "+"));
            slnxtPages.Add("txtSentDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSentDate").GetAttributeValue("value", "").Trim()));
            slnxtPages.Add("txtSenderEmailId", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSenderEmailId").GetAttributeValue("value", "").Trim()));
            slnxtPages.Add("txtVendorName", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVendorName").GetAttributeValue("value", "").Trim()).Replace("%20", "+"));
            slnxtPages.Add("txtTelephone", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtTelephone").GetAttributeValue("value", "").Trim()));
            slnxtPages.Add("txtFax", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtFax").GetAttributeValue("value", "").Trim()));
            slnxtPages.Add("txtVendorAddress", Uri.EscapeDataString(_httpWrapper.GetElement("textarea", "id", "txtVendorAddress").GetAttributeValue("value", "").Trim()));
            slnxtPages.Add("txtEmail", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtEmail").GetAttributeValue("value", "").Trim()));
            slnxtPages.Add("txtVenderReference", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVenderReference").GetAttributeValue("value", "").Trim()));
            slnxtPages.Add("txtOrderDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtOrderDate").GetAttributeValue("value", "").Trim()));
            slnxtPages.Add("txtPriority", _httpWrapper.GetElement("input", "id", "txtPriority").GetAttributeValue("value", "").Trim());
            slnxtPages.Add("UCDeliveryTerms%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCDeliveryTerms_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
            slnxtPages.Add("UCPaymentTerms%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCPaymentTerms_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
            slnxtPages.Add("txtDeliveryTime%24txtNumber", _httpWrapper.GetElement("input", "id", "txtDeliveryTime_txtNumber").GetAttributeValue("value", "").Trim());
            slnxtPages.Add("ucModeOfTransport%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucModeOfTransport_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
            slnxtPages.Add("ddlPreferedquality%24ddlHard", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ddlPreferedquality_ddlHard']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
            slnxtPages.Add("ucCurrency%24ddlCurrency", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucCurrency_ddlCurrency']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
            slnxtPages.Add("txtSupplierDiscount%24txtNumber", _httpWrapper.GetElement("input", "id", "txtSupplierDiscount_txtNumber").GetAttributeValue("value", "").Trim());
            slnxtPages.Add("txtPrice", _httpWrapper.GetElement("input", "id", "txtPrice").GetAttributeValue("value", "").Trim());
            slnxtPages.Add("txtDiscount%24txtNumber", _httpWrapper.GetElement("input", "id", "txtDiscount_txtNumber").GetAttributeValue("value", "").Trim());
            slnxtPages.Add("txtTotalPrice", _httpWrapper.GetElement("input", "id", "txtTotalPrice").GetAttributeValue("value", "").Trim());
            slnxtPages.Add("txtTotalDiscount", _httpWrapper.GetElement("input", "id", "txtTotalDiscount").GetAttributeValue("value", "").Trim());
            slnxtPages.Add("gvTax%24ctl03%24txtDescriptionAdd", _httpWrapper.GetElement("input", "id", "gvTax_ctl03_txtDescriptionAdd").GetAttributeValue("value", "").Trim());
            slnxtPages.Add("__SCROLLPOSITIONX", "0");
            slnxtPages.Add("__SCROLLPOSITIONY", _httpWrapper._dctStateData["__SCROLLPOSITIONY"]);
            slnxtPages.Add("isouterpage", "");
            slnxtPages.Add("txtnopage", "");

            foreach (HtmlNode _tr in _htmlNxtDoc.DocumentNode.SelectNodes("//table[@id='gvVendorItem']//tr"))
            {
                if (_tr.ChildNodes.Count > 6)
                {
                    if (_tr.InnerText != "\r\n\t\t" && _tr.ChildNodes[1].ChildNodes[1].InnerText.Trim() != "S.No")
                    {
                        if (_tr.ChildNodes[2].ChildNodes[1].Attributes["checked"] != null)
                        {
                            slnxtPages.Add(Uri.EscapeDataString(_tr.ChildNodes[2].ChildNodes[1].GetAttributeValue("name", "")), "on");
                        }
                    }
                }
            }

            HtmlNodeCollection _col = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//input[@name='gvTax$ctl03$ucTaxTypeAdd$rblValuePercentage']");
            if (_col != null)
            {
                if (_col.Count > 0)
                {
                    foreach (HtmlNode _node in _col)
                    {
                        if (_node.Attributes["checked"] != null)
                        {
                            slnxtPages.Add("gvTax%24ctl03%24ucTaxTypeAdd%24rblValuePercentage", _node.GetAttributeValue("value", "").Trim());
                        }
                    }
                }
            }
            if (_httpWrapper.GetElement("input", "id", "gvTax_ctl03_txtValueAdd_txtNumber").Attributes["value"] != null)
                slnxtPages.Add("gvTax%24ctl03%24txtValueAdd%24txtNumber", _httpWrapper.GetElement("input", "id", "gvTax_ctl03_txtValueAdd_txtNumber").GetAttributeValue("value", "").Trim());
            else
                slnxtPages.Add("gvTax%24ctl03%24txtValueAdd%24txtNumber", "");

            if (slnxtPages != null && slnxtPages.Count > 0)
            {
                foreach (KeyValuePair<string, string> kv in slnxtPages)
                {
                    _nxtpageDataitm += "&" + kv.Key + "=" + HttpUtility.UrlEncode(kv.Value.Replace(">", "gt;").Replace("<", "lt;"));
                }
            }
            return _nxtpageDataitm;
        }


        //private Boolean FillItemDetails_1(List<LineItem> _extraItems, Dictionary<int, LineItem> _items, out double extraItemCost, HtmlAgilityPack.HtmlDocument doc)
        //{
        //    _httpWrapper._CurrentDocument = doc; string _totalItems = ""; int _totalPages = 0;
        //    bool result = false; extraItemCost = 0;


        //    if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='lblRecords']") != null)
        //    {
        //        _totalItems = convert.ToString(_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='lblRecords']").InnerText.Trim()).Trim().Replace("(", "").Replace(")", "").Replace("records found", "").Trim();
        //        _totalPages = convert.ToInt(convert.ToString(_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='lblPages']").InnerText.Trim()).Trim().Replace("of", "").Replace("Pages.", "").Trim());
        //    }

        //    HtmlNode _tblItems = _httpWrapper.GetElement("table", "id", "gvVendorItem");
        //    if (_tblItems != null)
        //    {
        //        int count = _tblItems.SelectNodes("./tr").Count;
        //        if (count >= 2)
        //        {
        //        }
        //        else
        //        {
        //            if (_tblItems.InnerText.Trim().Contains("NO RECORDS FOUND"))
        //                throw new Exception("No item found on web portal");
        //        }
        //        //    foreach (LineItem item in _lineitem)
        //        //    {
        //        //        if (convert.ToInt(item.IsExtraItem) == 1)
        //        //        {
        //        //            _extraItems.Add(item);
        //        //        }
        //        //        else _items.Add(convert.ToInt(item.OriginatingSystemRef), item);
        //        //    }

        //        //    int SrNo = 0;
        //        //    #region to fetch include gst value
        //        //    //   dctPostDataValues.Clear();
        //        //    Dictionary<string, string> dctGstData = new Dictionary<string, string>();
        //        //    foreach (HtmlNode _tr in doc.DocumentNode.SelectNodes("//table[@id='gvVendorItem']//tr"))
        //        //    {
        //        //        if (_tr.ChildNodes.Count > 6)
        //        //        {
        //        //            if (_tr.InnerText != "\r\n\t\t" && _tr.ChildNodes[1].ChildNodes[1].InnerText.Trim() != "S.No")
        //        //            {
        //        //                if (_tr.ChildNodes[2].ChildNodes[1].Attributes["checked"] != null)
        //        //                {
        //        //                    dctGstData.Add(Uri.EscapeDataString(_tr.ChildNodes[2].ChildNodes[1].GetAttributeValue("name", "")), "on");
        //        //                }
        //        //            }
        //        //        }
        //        //    }
        //        //    #endregion

        //        //foreach (HtmlNode _tr in doc.DocumentNode.SelectNodes("//table[@id='gvVendorItem']//tr"))
        //        //{
        //        //    LineItem iObj = null;
        //        //    if (_tr.ChildNodes.Count > 6)
        //        //    {
        //        //        if (_tr.InnerText != "\r\n\t\t" && _tr.ChildNodes[1].ChildNodes[1].InnerText.Trim() != "S.No")
        //        //        {
        //        //            string id = _tr.ChildNodes[1].ChildNodes[1].GetAttributeValue("id", "").Trim();
        //        //            string[] _strFields = id.Split('_');
        //        //            string ctlID = _strFields[1].Trim();
        //        //            HtmlNode _hID = GetElement1("span", "id", id, doc);

        //        //            SrNo = convert.ToInt(_hID.InnerText.Trim());
        //        //            double _price = 0, _disc = 0;
        //        //            if (_items.ContainsKey(SrNo))
        //        //            {
        //        //                string iAddRemarks = "";
        //        //                iObj = _items[SrNo];
        //        //                if (iObj.Quantity > 0)
        //        //                {
        //        //                    foreach (PriceDetails amt in iObj.PriceList)
        //        //                    {
        //        //                        if (amt.TypeQualifier == PriceDetailsTypeQualifiers.GRP) _price = amt.Value;
        //        //                        else if (amt.TypeQualifier == PriceDetailsTypeQualifiers.DPR) _disc = amt.Value;
        //        //                    }

        //        //                    if (convert.ToFloat(iObj.MonetaryAmount) > 0)
        //        //                    {
        //        //                        #region edit item row
        //        //                        LogText = "Updating item " + convert.ToString(iObj.Number);
        //        //                        _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
        //        //                        _httpWrapper.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
        //        //                        _httpWrapper.ContentType = "application/x-www-form-urlencoded";
        //        //                        _httpWrapper._SetRequestHeaders[HttpRequestHeader.CacheControl] = "no-cache";

        //        //                        dctPostDataValues.Clear();
        //        //                        dctPostDataValues.Add("__EVENTTARGET", _httpWrapper._dctStateData["__EVENTTARGET"]);
        //        //                        dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
        //        //                        dctPostDataValues.Add("__VIEWSTATE", Uri.EscapeDataString(HttpUtility.UrlDecode(_httpWrapper._dctStateData["__VIEWSTATE"])));
        //        //                        dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
        //        //                        dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);
        //        //                        dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24cmdEdit.x", "3");
        //        //                        dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24cmdEdit.y", "7");
        //        //                        dctPostDataValues.Add("isouterpage", "");
        //        //                        dctPostDataValues.Add("txtnopage", "");
        //        //                        dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", string.Empty);
        //        //                        dctPostDataValues.Add("txtEnquiryNumber", _httpWrapper.GetElement("input", "id", "txtEnquiryNumber").GetAttributeValue("value", "").Trim());
        //        //                        dctPostDataValues.Add("txtDeliveryPlace", _httpWrapper.GetElement("input", "id", "txtDeliveryPlace").GetAttributeValue("value", "").Trim());
        //        //                        dctPostDataValues.Add("txtContactNo", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtContactNo").GetAttributeValue("value", "").Trim()).Replace("%20", "+"));
        //        //                        dctPostDataValues.Add("txtSenderName", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSenderName").GetAttributeValue("value", "").Trim()).Replace("%20", "+"));
        //        //                        dctPostDataValues.Add("txtSentDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSentDate").GetAttributeValue("value", "").Trim()));
        //        //                        dctPostDataValues.Add("txtSenderEmailId", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSenderEmailId").GetAttributeValue("value", "").Trim()));
        //        //                        dctPostDataValues.Add("txtVendorName", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVendorName").GetAttributeValue("value", "").Trim()).Replace("%20", "+"));
        //        //                        dctPostDataValues.Add("txtTelephone", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtTelephone").GetAttributeValue("value", "").Trim()));
        //        //                        dctPostDataValues.Add("txtFax", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtFax").GetAttributeValue("value", "").Trim()));
        //        //                        dctPostDataValues.Add("txtVendorAddress", Uri.EscapeDataString(_httpWrapper.GetElement("textarea", "id", "txtVendorAddress").GetAttributeValue("value", "").Trim()));
        //        //                        dctPostDataValues.Add("txtEmail", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtEmail").GetAttributeValue("value", "").Trim()));
        //        //                        dctPostDataValues.Add("txtVenderReference", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVenderReference").GetAttributeValue("value", "").Trim()));
        //        //                        dctPostDataValues.Add("txtOrderDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtOrderDate").GetAttributeValue("value", "").Trim()));
        //        //                        dctPostDataValues.Add("txtPriority", _httpWrapper.GetElement("input", "id", "txtPriority").GetAttributeValue("value", "").Trim());
        //        //                        dctPostDataValues.Add("UCDeliveryTerms%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCDeliveryTerms_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
        //        //                        dctPostDataValues.Add("UCPaymentTerms%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCPaymentTerms_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
        //        //                        dctPostDataValues.Add("txtDeliveryTime%24txtNumber", _httpWrapper.GetElement("input", "id", "txtDeliveryTime_txtNumber").GetAttributeValue("value", "").Trim());
        //        //                        dctPostDataValues.Add("ucModeOfTransport%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucModeOfTransport_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
        //        //                        dctPostDataValues.Add("ddlPreferedquality%24ddlHard", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ddlPreferedquality_ddlHard']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
        //        //                        dctPostDataValues.Add("ucCurrency%24ddlCurrency", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucCurrency_ddlCurrency']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
        //        //                        dctPostDataValues.Add("txtSupplierDiscount%24txtNumber", _httpWrapper.GetElement("input", "id", "txtSupplierDiscount_txtNumber").GetAttributeValue("value", "").Trim());
        //        //                        dctPostDataValues.Add("txtPrice", _httpWrapper.GetElement("input", "id", "txtPrice").GetAttributeValue("value", "").Trim());
        //        //                        dctPostDataValues.Add("txtDiscount%24txtNumber", _httpWrapper.GetElement("input", "id", "txtDiscount_txtNumber").GetAttributeValue("value", "").Trim());
        //        //                        dctPostDataValues.Add("txtTotalPrice", _httpWrapper.GetElement("input", "id", "txtTotalPrice").GetAttributeValue("value", "").Trim());
        //        //                        dctPostDataValues.Add("txtTotalDiscount", _httpWrapper.GetElement("input", "id", "txtTotalDiscount").GetAttributeValue("value", "").Trim());
        //        //                        dctPostDataValues.Add("gvTax%24ctl03%24txtDescriptionAdd", _httpWrapper.GetElement("input", "id", "gvTax_ctl03_txtDescriptionAdd").GetAttributeValue("value", "").Trim());
        //        //                        dctPostDataValues.Add("__SCROLLPOSITIONX", "0");
        //        //                        dctPostDataValues.Add("__SCROLLPOSITIONY", "0");
        //        //                        if (dctGstData.Count > 0)
        //        //                        {
        //        //                            foreach (KeyValuePair<string, string> _gst in dctGstData)
        //        //                            {
        //        //                                dctPostDataValues.Add(_gst.Key, _gst.Value);
        //        //                            }
        //        //                        }
        //        //                        HtmlNodeCollection _col = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//input[@name='gvTax$ctl03$ucTaxTypeAdd$rblValuePercentage']");
        //        //                        if (_col != null)
        //        //                        {
        //        //                            if (_col.Count > 0)
        //        //                            {
        //        //                                foreach (HtmlNode _node in _col)
        //        //                                {
        //        //                                    if (_node.Attributes["checked"] != null)
        //        //                                    {
        //        //                                        dctPostDataValues.Add("gvTax%24ctl03%24ucTaxTypeAdd%24rblValuePercentage", _node.GetAttributeValue("value", "").Trim());
        //        //                                    }
        //        //                                }
        //        //                            }
        //        //                        }
        //        //                        if (_httpWrapper.GetElement("input", "id", "gvTax_ctl03_txtValueAdd_txtNumber").Attributes["value"] != null)
        //        //                            dctPostDataValues.Add("gvTax%24ctl03%24txtValueAdd%24txtNumber", _httpWrapper.GetElement("input", "id", "gvTax_ctl03_txtValueAdd_txtNumber").GetAttributeValue("value", "").Trim());
        //        //                        else
        //        //                            dctPostDataValues.Add("gvTax%24ctl03%24txtValueAdd%24txtNumber", "");

        //        //                        #endregion

        //        //                        if (PostURL("input", "id", "gvVendorItem_" + ctlID + "_txtVendorItemUnitEdit"))
        //        //                        {
        //        //                            #region saving item row
        //        //                            dctPostDataValues.Clear();
        //        //                            dctPostDataValues.Add("__EVENTTARGET", _httpWrapper._dctStateData["__EVENTTARGET"]);
        //        //                            dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
        //        //                            dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", string.Empty);
        //        //                            dctPostDataValues.Add("__VIEWSTATE", Uri.EscapeDataString(HttpUtility.UrlDecode(_httpWrapper._dctStateData["__VIEWSTATE"])));
        //        //                            dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
        //        //                            dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);
        //        //                            dctPostDataValues.Add("__SCROLLPOSITIONX", "0");
        //        //                            dctPostDataValues.Add("__SCROLLPOSITIONY", "590");
        //        //                            dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtVendorItemUnitEdit", iObj.MeasureUnitQualifier);
        //        //                            dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtQuantityEdit%24txtNumber", convert.ToString(iObj.Quantity));
        //        //                            dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtQuotedPriceEdit%24txtNumber", convert.ToString(_price));
        //        //                            dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtDiscountEdit%24txtNumber", convert.ToString(_disc));
        //        //                            dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtDeliveryTimeEdit%24txtNumber", iObj.DeleiveryTime);
        //        //                            dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24cmdUpdate.x", "0");
        //        //                            dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24cmdUpdate.y", "0");
        //        //                            dctPostDataValues.Add("isouterpage", "");
        //        //                            dctPostDataValues.Add("txtnopage", "");
        //        //                            if (dctGstData.Count > 0)
        //        //                            {
        //        //                                foreach (KeyValuePair<string, string> _gst in dctGstData)
        //        //                                {
        //        //                                    dctPostDataValues.Add(_gst.Key, _gst.Value);

        //        //                                }
        //        //                            }
        //        //                            #endregion

        //        //                            if (PostURL("span", "id", "gvVendorItem_" + ctlID + "_lblVendorItemUnit"))//checking
        //        //                            {
        //        //                                string iRemarks = iAddRemarks + Environment.NewLine + convert.ToString(iObj.LineItemComment.Value).Trim();
        //        //                                iRemarks = iRemarks.Trim().Replace(Environment.NewLine, "<br>");
        //        //                                iRemarks = iRemarks.Replace("\r\n", " ");
        //        //                                iRemarks = iRemarks.Replace("'", "");
        //        //                                HtmlNode _inpRemarks = _httpWrapper.GetElement("input", "id", "gvVendorItem_" + ctlID + "_imgVendorNotes"); //_tr.ChildNodes[7].ChildNodes[1];
        //        //                                FillItemRemarks(iRemarks, _inpRemarks, iObj.Number);
        //        //                               // URL = cQuoteURL;
        //        //                                //LoadURL("span", "id", "Title1_lblTitle");

        //        //                                if (_nxtbtn == 1 && !string.IsNullOrEmpty(iRemarks))
        //        //                                {

        //        //                                    // Request_apps2_southnests_com(strnxtpgdetails);
        //        //                                    HtmlNode _nextPage = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//a[@id='cmdNext']");
        //        //                                    if (_nextPage != null)
        //        //                                    {
        //        //                                        string _disNext = _nextPage.GetAttributeValue("disabled", "").Trim();
        //        //                                        if (_disNext != "disabled")
        //        //                                        {
        //        //                                            slnxtPages.Clear();
        //        //                                            doc = GetNextPageItems_Quote(1);
        //        //                                            FillItemDetails(_extraItems, _items, out  extraItemCost, doc, 0);
        //        //                                        }
        //        //                                    }
        //        //                                }
        //        //                                System.Threading.Thread.Sleep(200);
        //        //                                result = true;
        //        //                            }
        //        //                            else throw new Exception("Unable to update item " + iObj.Number + ".");
        //        //                        }
        //        //                        else throw new Exception("Unable to edit item row " + iObj.Number);
        //        //                    }
        //        //                    else
        //        //                    {
        //        //                        LogText = "Unable to update item " + iObj.Number + "; Item Price is zero.";
        //        //                        string iRemarks = iAddRemarks + Environment.NewLine + convert.ToString(iObj.LineItemComment.Value).Trim();
        //        //                        iRemarks = iRemarks.Trim().Replace(Environment.NewLine, "<br>");
        //        //                        iRemarks = iRemarks.Replace("\r\n", " ");
        //        //                        iRemarks = iRemarks.Replace("'", "");
        //        //                        HtmlNode _inpRemarks = _httpWrapper.GetElement("input", "id", "gvVendorItem_" + ctlID + "_imgVendorNotes"); //_tr.ChildNodes[7].ChildNodes[1];
        //        //                        FillItemRemarks(iRemarks, _inpRemarks, iObj.Number);

        //        //                        if (!string.IsNullOrEmpty(iRemarks))
        //        //                        {
        //        //                            HtmlNode _nextPage = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//a[@id='cmdNext']");
        //        //                            if (_nextPage != null)
        //        //                            {
        //        //                                string _disNext = _nextPage.GetAttributeValue("disabled", "").Trim();
        //        //                                if (_disNext != "disabled")
        //        //                                {
        //        //                                    slnxtPages.Clear();
        //        //                                    doc = GetNextPageItems_Quote(1);
        //        //                                    FillItemDetails(_extraItems, _items, out  extraItemCost, doc, 0);
        //        //                                }
        //        //                            }
        //        //                        }
        //        //                    }
        //        //                }
        //        //                else
        //        //                    LogText = "Unable to update item " + iObj.Number + "; Item Quantity is zero.";
        //        //            }
        //        //            else
        //        //            {
        //        //                throw new Exception("Item no. " + SrNo + " not found in MTML Quote file.");
        //        //            }
        //        //        }
        //        //    }
        //        //    // 
        //        //}
        //        //if (_extraItems.Count > 0)
        //        //{
        //        //    foreach (LineItem eItem in _extraItems)
        //        //    {
        //        //        if (convert.ToString(eItem.ItemType) == "")
        //        //        {
        //        //            extraItemCost += eItem.MonetaryAmount;
        //        //        }
        //        //    }

        //        //    URL = MessageNumber;
        //        //    _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
        //        //    _httpWrapper.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
        //        //    _httpWrapper.ContentType = "application/x-www-form-urlencoded";
        //        //    _httpWrapper._SetRequestHeaders[HttpRequestHeader.CacheControl] = "no-cache";
        //        //    _httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");
        //        //    if (LoadURL("span", "id", "Title1_lblTitle"))
        //        //    {
        //        //        FillExtraItems(_extraItems);
        //        //    }
        //        //}

        //    }
        //    else result = false;
        //    return result;
        //}


    }
}


#region commented
#region commented
//        public bool FillItems(out double extraItemCost)
//        {
//            extraItemCost = 0;
//            bool result = false; Dictionary<int, LineItem> _items = new Dictionary<int, LineItem>();
//            List<LineItem> _extraItems = new List<LineItem>();
//            try
//            {
//                LogText = "Filling items.";
//                URL = MessageNumber;
//                _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
//                _httpWrapper.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
//                _httpWrapper.ContentType = "application/x-www-form-urlencoded";
//                _httpWrapper._SetRequestHeaders[HttpRequestHeader.CacheControl] = "no-cache";
//                _httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");






//if (LoadURL("span", "id", "Title1_lblTitle"))
//                {
//                    HtmlNode _tblItems = _httpWrapper.GetElement("table", "id", "gvVendorItem");
//                    if (_tblItems != null)
//                    {
//                        int count = _tblItems.SelectNodes("./tr").Count;
//                        if (count >= 2)
//                        {
//                            foreach (LineItem item in _lineitem)
//                            {
//                                if (convert.ToInt(item.IsExtraItem) == 1)
//                                {
//                                    _extraItems.Add(item);
//                                }
//                                else _items.Add(convert.ToInt(item.OriginatingSystemRef), item);
//                            }

//                            int SrNo = 0;
//                            #region to fetch include gst value
//                            //   dctPostDataValues.Clear();
//                            Dictionary<string, string> dctGstData = new Dictionary<string, string>();
//                            foreach (HtmlNode _tr in _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='gvVendorItem']//tr"))
//                            {
//                                if (_tr.ChildNodes.Count > 6)
//                                {
//                                    if (_tr.InnerText != "\r\n\t\t" && _tr.ChildNodes[1].ChildNodes[1].InnerText.Trim() != "S.No")
//                                    {
//                                        if (_tr.ChildNodes[2].ChildNodes[1].Attributes["checked"] != null)
//                                        {
//                                            dctGstData.Add(Uri.EscapeDataString(_tr.ChildNodes[2].ChildNodes[1].GetAttributeValue("name", "")), "on");
//                                        }
//                                    }
//                                }
//                            }
//                            #endregion
//                            foreach (HtmlNode _tr in _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='gvVendorItem']//tr"))
//                            {

//                                LineItem iObj = null;
//                                URL = MessageNumber;
//                                LoadURL("", "", "");

//                                if (_tr.ChildNodes.Count > 6)
//                                {
//                                    if (_tr.InnerText != "\r\n\t\t" && _tr.ChildNodes[1].ChildNodes[1].InnerText.Trim() != "S.No")
//                                    {
//                                        string id = _tr.ChildNodes[1].ChildNodes[1].GetAttributeValue("id", "").Trim();
//                                        string[] _strFields = id.Split('_');
//                                        string ctlID = _strFields[1].Trim();

//                                        HtmlNode _hID = _httpWrapper.GetElement("span", "id", id);
//                                        SrNo = convert.ToInt(_hID.InnerText.Trim());
//                                        double _price = 0, _disc = 0;
//                                        if (_items.ContainsKey(SrNo))
//                                        {
//                                            string iAddRemarks = "";
//                                            iObj = _items[SrNo];
//                                            if (iObj.Quantity > 0)
//                                            {
//                                                foreach (PriceDetails amt in iObj.PriceList)
//                                                {
//                                                    if (amt.TypeQualifier == PriceDetailsTypeQualifiers.GRP) _price = amt.Value;
//                                                    else if (amt.TypeQualifier == PriceDetailsTypeQualifiers.DPR) _disc = amt.Value;
//                                                }

//                                                // bool uomFound = false;
//                                                //bool updateUOM = true;
//                                                //HtmlNode lblUnit = _httpWrapper.GetElement("span", "id", "gvVendorItem_" + ctlID + "_lblunit");
//                                                //if (lblUnit != null)
//                                                //{
//                                                //    if (iObj.MeasureUnitQualifier.Trim().ToUpper() == lblUnit.InnerText.Trim().ToUpper())//_tr.ChildNodes[_unitCol].ChildNodes[1]
//                                                //    {
//                                                //        uomFound = true;
//                                                //        updateUOM = false;
//                                                //    }
//                                                //}

//                                                if (convert.ToFloat(iObj.MonetaryAmount) > 0)
//                                                {
//                                                    #region edit item row
//                                                    LogText = "Updating item " + convert.ToString(iObj.Number);
//                                                    _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
//                                                    _httpWrapper.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
//                                                    _httpWrapper.ContentType = "application/x-www-form-urlencoded";
//                                                    _httpWrapper._SetRequestHeaders[HttpRequestHeader.CacheControl] = "no-cache";

//                                                    dctPostDataValues.Clear();
//                                                    dctPostDataValues.Add("__EVENTTARGET", _httpWrapper._dctStateData["__EVENTTARGET"]);
//                                                    dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
//                                                    dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", _httpWrapper._dctStateData["ToolkitScriptManager1_HiddenField"]);
//                                                    dctPostDataValues.Add("__VIEWSTATE", Uri.EscapeDataString(HttpUtility.UrlDecode(_httpWrapper._dctStateData["__VIEWSTATE"])));
//                                                    dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
//                                                    dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);
//                                                    //dctPostDataValues.Add("meeDiscount_ClientState", "");
//                                                    // dctPostDataValues.Add("gvTax%24ctl03%24MaskedEditTotalPayableAmout_ClientState", "");
//                                                    // dctPostDataValues.Add("MaskedEditExtender1_ClientState", "");
//                                                    dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24cmdEdit.x", "3");
//                                                    dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24cmdEdit.y", "7");
//                                                    dctPostDataValues.Add("isouterpage", "");
//                                                    dctPostDataValues.Add("txtnopage", "");
//                                                    dctPostDataValues.Add("txtEnquiryNumber", _httpWrapper.GetElement("input", "id", "txtEnquiryNumber").GetAttributeValue("value", "").Trim());
//                                                    dctPostDataValues.Add("txtDeliveryPlace", _httpWrapper.GetElement("input", "id", "txtDeliveryPlace").GetAttributeValue("value", "").Trim());
//                                                    dctPostDataValues.Add("txtContactNo", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtContactNo").GetAttributeValue("value", "").Trim()).Replace("%20", "+"));
//                                                    dctPostDataValues.Add("txtSenderName", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSenderName").GetAttributeValue("value", "").Trim()).Replace("%20", "+"));
//                                                    dctPostDataValues.Add("txtSentDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSentDate").GetAttributeValue("value", "").Trim()));
//                                                    dctPostDataValues.Add("txtSenderEmailId", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSenderEmailId").GetAttributeValue("value", "").Trim()));
//                                                    dctPostDataValues.Add("txtVendorName", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVendorName").GetAttributeValue("value", "").Trim()).Replace("%20", "+"));
//                                                    dctPostDataValues.Add("txtTelephone", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtTelephone").GetAttributeValue("value", "").Trim()));
//                                                    dctPostDataValues.Add("txtFax", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtFax").GetAttributeValue("value", "").Trim()));
//                                                    dctPostDataValues.Add("txtVendorAddress", Uri.EscapeDataString(_httpWrapper.GetElement("textarea", "id", "txtVendorAddress").GetAttributeValue("value", "").Trim()));
//                                                    dctPostDataValues.Add("txtEmail", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtEmail").GetAttributeValue("value", "").Trim()));
//                                                    dctPostDataValues.Add("txtVenderReference", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVenderReference").GetAttributeValue("value", "").Trim()));
//                                                    dctPostDataValues.Add("txtOrderDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtOrderDate").GetAttributeValue("value", "").Trim()));
//                                                    dctPostDataValues.Add("txtPriority", _httpWrapper.GetElement("input", "id", "txtPriority").GetAttributeValue("value", "").Trim());
//                                                    dctPostDataValues.Add("UCDeliveryTerms%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCDeliveryTerms_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
//                                                    dctPostDataValues.Add("UCPaymentTerms%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCPaymentTerms_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
//                                                    dctPostDataValues.Add("txtDeliveryTime%24txtNumber", _httpWrapper.GetElement("input", "id", "txtDeliveryTime_txtNumber").GetAttributeValue("value", "").Trim());
//                                                    dctPostDataValues.Add("ucModeOfTransport%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucModeOfTransport_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
//                                                    dctPostDataValues.Add("ddlPreferedquality%24ddlHard", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ddlPreferedquality_ddlHard']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
//                                                    dctPostDataValues.Add("ucCurrency%24ddlCurrency", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucCurrency_ddlCurrency']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
//                                                    dctPostDataValues.Add("txtSupplierDiscount%24txtNumber", _httpWrapper.GetElement("input", "id", "txtSupplierDiscount_txtNumber").GetAttributeValue("value", "").Trim());
//                                                    dctPostDataValues.Add("txtPrice", _httpWrapper.GetElement("input", "id", "txtPrice").GetAttributeValue("value", "").Trim());
//                                                    dctPostDataValues.Add("txtDiscount%24txtNumber", _httpWrapper.GetElement("input", "id", "txtDiscount_txtNumber").GetAttributeValue("value", "").Trim());
//                                                    dctPostDataValues.Add("txtTotalPrice", _httpWrapper.GetElement("input", "id", "txtTotalPrice").GetAttributeValue("value", "").Trim());
//                                                    dctPostDataValues.Add("txtTotalDiscount", _httpWrapper.GetElement("input", "id", "txtTotalDiscount").GetAttributeValue("value", "").Trim());
//                                                    dctPostDataValues.Add("gvTax%24ctl03%24txtDescriptionAdd", _httpWrapper.GetElement("input", "id", "gvTax_ctl03_txtDescriptionAdd").GetAttributeValue("value", "").Trim());
//                                                    dctPostDataValues.Add("__SCROLLPOSITIONX", "0");
//                                                    dctPostDataValues.Add("__SCROLLPOSITIONY", "0");
//                                                    if (dctGstData.Count > 0)
//                                                    {
//                                                        foreach (KeyValuePair<string, string> _gst in dctGstData)
//                                                        {
//                                                            dctPostDataValues.Add(_gst.Key, _gst.Value);

//                                                        }
//                                                    }


//                                                    HtmlNodeCollection _col = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//input[@name='gvTax$ctl03$ucTaxTypeAdd$rblValuePercentage']");
//                                                    if (_col != null)
//                                                    {
//                                                        if (_col.Count > 0)
//                                                        {
//                                                            foreach (HtmlNode _node in _col)
//                                                            {
//                                                                if (_node.Attributes["checked"] != null)
//                                                                {
//                                                                    dctPostDataValues.Add("gvTax%24ctl03%24ucTaxTypeAdd%24rblValuePercentage", _node.GetAttributeValue("value", "").Trim());
//                                                                }
//                                                            }
//                                                        }
//                                                    }
//                                                    // dctPostDataValues.Add("gvTax%24ctl03%24ucTaxTypeAdd%24rblValuePercentage", _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@name='gvTax$ctl03$ucTaxTypeAdd$rblValuePercentage']//option[@checked='checked']");//.GetAttributeValue("value", "").Trim());
//                                                    if (_httpWrapper.GetElement("input", "id", "gvTax_ctl03_txtValueAdd_txtNumber").Attributes["value"] != null)
//                                                        dctPostDataValues.Add("gvTax%24ctl03%24txtValueAdd%24txtNumber", _httpWrapper.GetElement("input", "id", "gvTax_ctl03_txtValueAdd_txtNumber").GetAttributeValue("value", "").Trim());
//                                                    else
//                                                        dctPostDataValues.Add("gvTax%24ctl03%24txtValueAdd%24txtNumber", "");
//                                                    #endregion

//                                                    if (PostURL("input", "id", "gvVendorItem_" + ctlID + "_txtVendorItemUnitEdit"))
//                                                    {
//                                                        #region saving item row
//                                                        dctPostDataValues.Clear();
//                                                        dctPostDataValues.Add("__EVENTTARGET", _httpWrapper._dctStateData["__EVENTTARGET"]);
//                                                        dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
//                                                        dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", _httpWrapper._dctStateData["ToolkitScriptManager1_HiddenField"]);
//                                                        dctPostDataValues.Add("__VIEWSTATE", Uri.EscapeDataString(HttpUtility.UrlDecode(_httpWrapper._dctStateData["__VIEWSTATE"])));
//                                                        dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
//                                                        dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);
//                                                        dctPostDataValues.Add("__SCROLLPOSITIONX", "0");
//                                                        dctPostDataValues.Add("__SCROLLPOSITIONY", "590");

//                                                        //string _unitVal = "";
//                                                        //var options = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='gvVendorItem_" + ctlID + "_ucUnit_ddlUnit']/option[@selected='selected']");
//                                                        //if (updateUOM)
//                                                        //{
//                                                        //    try
//                                                        //    {
//                                                        //        if (options != null)
//                                                        //        {
//                                                        //            foreach (var _opt in options)
//                                                        //            {
//                                                        //                if (_opt.InnerText.Trim().ToUpper() == iObj.MeasureUnitQualifier.Trim().ToUpper())
//                                                        //                {
//                                                        //                    _unitVal = _opt.GetAttributeValue("value", "").Trim();
//                                                        //                    uomFound = true;
//                                                        //                    break;
//                                                        //                }
//                                                        //            }
//                                                        //        }
//                                                        //    }
//                                                        //    catch (Exception ex)
//                                                        //    {
//                                                        //        LogText = "Unable to update UOM for item " + SrNo + ". Error - " + ex.Message;
//                                                        //    }
//                                                        //}

//                                                        //if (uomFound)
//                                                        //{
//                                                        dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtVendorItemUnitEdit", iObj.MeasureUnitQualifier);
//                                                        dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtQuantityEdit%24txtNumber", convert.ToString(iObj.Quantity));
//                                                        dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtQuotedPriceEdit%24txtNumber", convert.ToString(_price));
//                                                        dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtDiscountEdit%24txtNumber", convert.ToString(_disc));
//                                                        //}
//                                                        //else
//                                                        //{
//                                                        //    if (options[0].InnerText.Trim() == "--Select--")
//                                                        //    {
//                                                        //        if (options.Count == 2)
//                                                        //        {
//                                                        //            _unitVal = options[1].GetAttributeValue("value", "").Trim();
//                                                        //            uomFound = true;
//                                                        //        }
//                                                        //        else if (options.Count > 2)
//                                                        //        {
//                                                        //            //set default 
//                                                        //            foreach (var _opt in options)
//                                                        //            {
//                                                        //                if (_opt.InnerText.Trim().ToUpper() == "PCE" || _opt.InnerText.Trim().ToUpper() == "PCS")
//                                                        //                {
//                                                        //                    uomFound = true;
//                                                        //                    _unitVal = _opt.GetAttributeValue("value", "").Trim();
//                                                        //                    break;
//                                                        //                }
//                                                        //            }
//                                                        //        }
//                                                        //        if (uomFound == false) throw new Exception("Unable to set UOM for item no. " + iObj.Number);
//                                                        //    }

//                                                        //    // Calculate Avarage Price
//                                                        //    string orgQtyStr = "";
//                                                        //    HtmlNode orgQty = _httpWrapper.GetElement("input", "id", "gvVendorItem_" + ctlID + "_txtQuantityEdit_txtNumber");
//                                                        //    if (orgQty != null)
//                                                        //    {
//                                                        //        orgQtyStr = orgQty.GetAttributeValue("Value", "");
//                                                        //        if (orgQtyStr.Trim().Length > 0)
//                                                        //        {
//                                                        //            double unitPrice = iObj.MonetaryAmount / convert.ToDouble(orgQtyStr);
//                                                        //            dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtQuotedPriceEdit%24txtNumber", Math.Round(unitPrice, 3).ToString());
//                                                        //        }
//                                                        //        else throw new Exception("Original Qty is 0.");
//                                                        //    }
//                                                        //    dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtDiscountEdit%24txtNumber", convert.ToString(_disc));
//                                                        //    if (convert.ToFloat(orgQtyStr) != convert.ToFloat(iObj.Quantity))
//                                                        //    {
//                                                        //        iAddRemarks = "Quoted Qty : " + convert.ToFloat(iObj.Quantity) + "; ";
//                                                        //    }

//                                                        //    iAddRemarks += "Quoted UOM : " + convert.ToString(iObj.MeasureUnitQualifier).Trim().ToUpper();
//                                                        //}

//                                                        //if (_unitVal != "")
//                                                        //    dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24ucUnit%24ddlUnit", _unitVal);
//                                                        dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtDeliveryTimeEdit%24txtNumber", iObj.DeleiveryTime);
//                                                        dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24cmdUpdate.x", "0");
//                                                        dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24cmdUpdate.y", "0");
//                                                        dctPostDataValues.Add("isouterpage", "");
//                                                        dctPostDataValues.Add("txtnopage", "");
//                                                        if (dctGstData.Count > 0)
//                                                        {
//                                                            foreach (KeyValuePair<string, string> _gst in dctGstData)
//                                                            {
//                                                                dctPostDataValues.Add(_gst.Key, _gst.Value);

//                                                            }
//                                                        }
//                                                        #endregion
//                                                        if (PostURL("span", "id", "gvVendorItem_" + ctlID + "_lblVendorItemUnit"))
//                                                        {
//                                                            string iRemarks = iAddRemarks + Environment.NewLine + convert.ToString(iObj.LineItemComment.Value).Trim();
//                                                            iRemarks = iRemarks.Trim().Replace(Environment.NewLine, "<br>");
//                                                            iRemarks = iRemarks.Replace("\r\n", " ");
//                                                            iRemarks = iRemarks.Replace("'", "");
//                                                            HtmlNode _inpRemarks = _httpWrapper.GetElement("input", "id", "gvVendorItem_" + ctlID + "_imgVendorNotes"); //_tr.ChildNodes[7].ChildNodes[1];
//                                                            FillItemRemarks(iRemarks, _inpRemarks, iObj.Number);
//                                                            System.Threading.Thread.Sleep(200);
//                                                            result = true;
//                                                        }
//                                                        else throw new Exception("Unable to update item " + iObj.Number + ".");
//                                                    }
//                                                    else throw new Exception("Unable to edit item row " + iObj.Number);
//                                                }
//                                                else
//                                                {
//                                                    LogText = "Unable to update item " + iObj.Number + "; Item Price is zero.";
//                                                    string iRemarks = iAddRemarks + Environment.NewLine + convert.ToString(iObj.LineItemComment.Value).Trim();
//                                                    iRemarks = iRemarks.Trim().Replace(Environment.NewLine, "<br>");
//                                                    iRemarks = iRemarks.Replace("\r\n", " ");
//                                                    iRemarks = iRemarks.Replace("'", "");
//                                                    HtmlNode _inpRemarks = _httpWrapper.GetElement("input", "id", "gvVendorItem_" + ctlID + "_imgVendorNotes"); //_tr.ChildNodes[7].ChildNodes[1];
//                                                    FillItemRemarks(iRemarks, _inpRemarks, iObj.Number);
//                                                }
//                                            }
//                                            else
//                                                LogText = "Unable to update item " + iObj.Number + "; Item Quantity is zero.";
//                                        }
//                                        else
//                                        {
//                                            throw new Exception("Item no. " + SrNo + " not found in MTML Quote file.");
//                                        }
//                                    }
//                                }
//                            }
//                            if (_extraItems.Count > 0)
//                            {
//                                foreach (LineItem eItem in _extraItems)
//                                {
//                                    if (convert.ToString(eItem.ItemType) == "")
//                                    {
//                                        extraItemCost += eItem.MonetaryAmount;
//                                    }
//                                }

//                                URL = MessageNumber;
//                                _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
//                                _httpWrapper.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
//                                _httpWrapper.ContentType = "application/x-www-form-urlencoded";
//                                _httpWrapper._SetRequestHeaders[HttpRequestHeader.CacheControl] = "no-cache";
//                                _httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");
//                                if (LoadURL("span", "id", "Title1_lblTitle"))
//                                {
//                                    FillExtraItems(_extraItems);
//                                }
//                            }
//                        }
//                        else
//                        {
//                            if (_tblItems.InnerText.Trim().Contains("NO RECORDS FOUND"))
//                                throw new Exception("No item found on web portal");
//                        }
//                    }
//                    else result = false;
//                }
//                else result = false;
//            }
//            catch (Exception ex)
//            {
//                LogText = "Exception while filling items: " + ex.GetBaseException().Message.ToString();
//                _errorlog = ex.GetBaseException().Message.ToString();
//                throw;
//                // result = false;
//            }
//            return result;
//        }
#endregion

     



//private List<List<string>> GetRFQItems1(Dictionary<string, string> _xmlHeader)
//{
//    List<List<string>> lstItems = new List<List<string>>();
//    HtmlNodeCollection _tr = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='gvVendorItem']//tr[@tabindex='-1']");
//    if (_tr != null)
//    {
//        if (_tr.Count > 0)
//        {
//            int Counter = 03, itemCount = 0;
//            string strCounter = "";
//            if (Counter < _tr.Count + 1) strCounter = "0" + Counter;
//            else if (Counter > 10) strCounter = Counter.ToString();
//            string _i = "gvVendorItem_ctl" + strCounter + "_lblSNo";
//            HtmlNode itemNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='" + _i + "']");

//            do
//            {
//                if (itemNo != null)
//                {
//                    string RefNo = "";
//                    itemCount++;
//                    string _gst = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//input[@id='gvVendorItem_ctl" + strCounter + "_chkIncludeGST']").GetAttributeValue("checked", "").Trim();
//                    if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblMakerReference']") != null)
//                        RefNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblMakerReference']").InnerText.Trim();

//                    string Descr = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//a[@id='gvVendorItem_ctl" + strCounter + "_lnkStockItemCode']").InnerText.Trim();

//                    string Qty = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblOrderQuantity']").InnerText.Trim();
//                    string Unit = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblCustomerUnit']").InnerText.Trim();
//                    string Brand = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblBrand']").InnerText.Trim();

//                    # region /* item remarks */
//                    string ItemRemarks = "";
//                    HtmlNode _details = _httpWrapper.GetElement("input", "id", "gvVendorItem_ctl" + strCounter + "_cmdDetails");
//                    if (_details != null)
//                    {
//                        string _attDis = _details.GetAttributeValue("disabled", "");
//                        if (_attDis == "")
//                        {
//                            string[] _onclick = _details.GetAttributeValue("onclick", "").Split(',');
//                            if (_onclick.Length == 3)
//                            {
//                                URL = "https://apps2.southnests.com/ArchangelReports/Purchase/" + HttpUtility.HtmlDecode(_onclick[2].Split('\'')[1]);
//                                if (LoadURL("input", "id", "_content_txtItemDetails_ucCustomEditor_ctl02", true))
//                                {
//                                    ItemRemarks = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//input[@id='_content_txtItemDetails_ucCustomEditor_ctl02']").GetAttributeValue("value", "").Trim();
//                                    ItemRemarks = ItemRemarks.Replace(@"&amp;", "&");
//                                    ItemRemarks = ItemRemarks.Replace(@"&lt;", "<");
//                                    ItemRemarks = ItemRemarks.Replace(@"&gt;", ">");
//                                    ItemRemarks = ItemRemarks.Replace(@"&quot;", "\"");
//                                    ItemRemarks = ItemRemarks.Replace(@"&apos;", "'");
//                                    ItemRemarks = ItemRemarks.Replace(@"&amp;", "&");

//                                    while (ItemRemarks.Trim().Contains("&"))
//                                    {
//                                        if (ItemRemarks.Trim().Contains("&") && !ItemRemarks.Trim().Contains(" & "))
//                                        {
//                                            ItemRemarks = HttpUtility.HtmlDecode(ItemRemarks.Trim());
//                                        }
//                                        else break;
//                                    }

//                                    if (ItemRemarks.Contains("<br />")) ItemRemarks = ItemRemarks.Replace("<br />", " ");
//                                    if (ItemRemarks.Contains("<!--"))
//                                        ItemRemarks = Regex.Replace(ItemRemarks, "<!--.*?-->", String.Empty, RegexOptions.Multiline);

//                                    HtmlAgilityPack.HtmlDocument doc1 = new HtmlAgilityPack.HtmlDocument();
//                                    doc1.LoadHtml(ItemRemarks);
//                                    string data = convert.ToString(doc1.DocumentNode.InnerText);

//                                    ItemRemarks = data.Trim();
//                                }
//                            }
//                        }
//                    }//
//                    #endregion

//                    URL = currentURL;
//                    if (LoadURL("input", "id", "txtnopage", true))
//                    {

//                        if (_gst.ToUpper() == "CHECKED")
//                            ItemRemarks += Environment.NewLine + "Included GST : Yes";
//                        else ItemRemarks += Environment.NewLine + "Included GST : No";

//                        if (Brand != "")
//                            ItemRemarks += Environment.NewLine + "Brand : " + Brand;

//                        List<string> item = new List<string>();
//                        item.Add(convert.ToString(itemNo.InnerText.Trim()));
//                        item.Add(RefNo);
//                        item.Add(Descr);
//                        item.Add(Qty);
//                        item.Add(Unit);
//                        item.Add(ItemRemarks);
//                        lstItems.Add(item);
//                        Counter++;
//                    }
//                }

//                int _totalPages = 0; string _totalItems = "";
//                if (Counter.ToString().Length == 1) strCounter = "0" + Counter;
//                else strCounter = Counter.ToString();

//                if (itemCount >= _tr.Count)
//                {
//                     Check Total Item Count
//                    if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='lblRecords']") != null)
//                    {
//                        _totalItems = convert.ToString(_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='lblRecords']").InnerText.Trim()).Trim().Replace("(", "").Replace(")", "").Replace("records found", "").Trim();
//                        _totalPages = convert.ToInt(convert.ToString(_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='lblPages']").InnerText.Trim()).Trim().Replace("of", "").Replace("Pages.", "").Trim());

//                        if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsItemCountMismatch"].Trim()) == false)
//                        {
//                            if (convert.ToInt(_totalItems) == lstItems.Count)
//                            {
//                                break;
//                            }
//                        }
//                        else
//                        {
//                            if (_tr.Count == lstItems.Count)
//                            {
//                                break;
//                            }
//                        }
//                    }

//                    if (_totalPages == 1 && (convert.ToInt(_totalItems) != lstItems.Count))
//                    {
//                        if (itemCount != (_tr.Count - 1))
//                        {
//                            throw new Exception("Total Item count not matching with items in grid");
//                        }
//                        else break;
//                    }

//                     Move to next Grid Page

//                    HtmlNode _nextPage = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//a[@id='cmdNext']");
//                    if (_nextPage != null)
//                    {
//                        string _disNext = _nextPage.GetAttributeValue("disabled", "").Trim();
//                        if (_disNext != "disabled")
//                        {
//                            throw new Exception("multiple pages found for item table.");
//                        }
//                    }

//                     Reset Counter
//                    Counter = 02; itemCount = 0;
//                    if (Counter.ToString().Length == 1) strCounter = "0" + Counter;
//                    else strCounter = Counter.ToString();
//                }
//                itemNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblSNo']");
//            }
//            while (itemNo != null);
//        }
//    }
//    return lstItems;
//}





#endregion