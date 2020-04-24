using HtmlAgilityPack;
using MTML.GENERATOR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Http_GreatShip_Routine
{
    public class Http_Download_Routine :LeSCommon.LeSCommon
    {
        public int iRetry = 0, IsAltItemAllowed = 0, IsPriceAveraged = 0, IsUOMChanged = 0;
        public string[] Actions;
        bool IsDecline = false;
        string sAuditMesage = "", DocType = "", VRNO = "", ToDate = "", FromDate = "", sDoneFile = "", MessageNumber = "", LeadDays = "", Currency = ""
           , MsgNumber = "", MsgRefNumber = "", UCRefNo = "", AAGRefNo = "", LesRecordID = "", BuyerName = "", BuyerPhone = "", BuyerEmail = "", BuyerFax = "",
            supplierName = "", supplierPhone = "", supplierEmail = "", supplierFax = "", VesselName = "", PortName = "", PortCode = "", SupplierComment = "", PayTerms = "",
            PackingCost = "", FreightCharge = "", GrandTotal = "", Allowance = "", TotalLineItemsAmount = "", BuyerTotal = "", DtDelvDate = "", dtExpDate = "", MTML_QuotePath="";
        public LineItemCollection _lineitem = null;
        List<string> lstNewRFQs = new List<string>();
        List<string> xmlFiles = new List<string>();
        public MTMLInterchange _interchange { get; set; }
        string _dateTo = "", _dateFrom = ""; DateTime dt = new DateTime(); DateTime dt1 = new DateTime();

        public void LoadAppsettings()
        {
            try
            {
                IsUrlEncoded = false;
                SessionIDCookieName = "ASP.NET_SessionId";
                URL = ConfigurationManager.AppSettings["SITE_URL"].Trim();
                Userid = ConfigurationManager.AppSettings["USERNAME"].Trim();
                Password = ConfigurationManager.AppSettings["PASSWORD"].Trim();
                Actions = ConfigurationManager.AppSettings["ACTIONS"].Trim().Split(',');
                ToDate = ConfigurationManager.AppSettings["INQUIRY_TO_DATE"].Trim();
                FromDate = ConfigurationManager.AppSettings["INQUIRY_FROM_DATE"].Trim();
                BuyerCode = ConfigurationManager.AppSettings["BUYER_CODE"].Trim();
                SupplierCode = ConfigurationManager.AppSettings["SUPPLIER_CODE"].Trim();
                //DownloadPath = ConfigurationManager.AppSettings["XLS_RFQPATH"].Trim();
                DownloadPath = ConfigurationManager.AppSettings["XML_PATH"].Trim();
                AuditPath = ConfigurationManager.AppSettings["AUDIT_PATH"].Trim();
                PrintScreenPath = ConfigurationManager.AppSettings["SCREENSHOT_PATH"].Trim();
                MTML_QuotePath = ConfigurationManager.AppSettings["QUOTE_UPLOADPATH"].Trim();
                LoginRetry = convert.ToInt(ConfigurationManager.AppSettings["LOGIN_RETRY"].Trim());
                if (DownloadPath != "" && !Directory.Exists(DownloadPath))
                {
                    Directory.CreateDirectory(DownloadPath);
                }
            }
            catch (Exception e)
            {
                sAuditMesage = "Exception in LoadAppsettings: " + e.GetBaseException().ToString();
                LogText = sAuditMesage;
               // CreateAuditFile("", "GreatShip_HTTP_Processor", "", "Error", sAuditMesage,BuyerCode,SupplierCode,AuditPath);
            }
        }

        public override bool DoLogin(string validateNodeType, string validateAttribute, string attributeValue, bool bload = true)
        {
            bool isLoggedin = false;
            try
            {
                _httpWrapper.ContentType = "";
                _httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.CacheControl);
                URL = ConfigurationManager.AppSettings["SITE_URL"].Trim();
                LoadURL(validateNodeType, validateAttribute, attributeValue);
                //LoadURL(validateNodeType, validateAttribute, attributeValue, false);
                if (_httpWrapper._dctStateData.Count > 0)
                {
                    URL = URL + "/LoginTest";
                    _httpWrapper.Referrer = "https://associates.greatship.com/Login.aspx";
                    _httpWrapper.AcceptMimeType = "";
                    _httpWrapper.UserAgent = "";
                    if(!_httpWrapper._AddRequestHeaders.ContainsKey("origin"))
                    _httpWrapper._AddRequestHeaders.Add("origin", "https://associates.greatship.com");
                    if (!_httpWrapper._AddRequestHeaders.ContainsKey("X-Requested-With"))
                    _httpWrapper._AddRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                    if (_httpWrapper._AddRequestHeaders.ContainsKey("Upgrade-Insecure-Requests"))
                    _httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");
                    _httpWrapper.ContentType = "application/json";
                    _httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.AcceptEncoding);
                    _httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.AcceptLanguage);
                    dctPostDataValues.Clear();
                    dctPostDataValues.Add("userName", Userid);
                    dctPostDataValues.Add("password", Password);
                   // isLoggedin = base.DoLogin("", "", "", true,true);
                    //added after changing wrapper
                    string _postdata = GetPostData(true);
                    isLoggedin = _httpWrapper.PostURL(URL, _postdata, "", "", "");
                   // _CurrentResponseString = _httpWrapper._CurrentResponseString;
             
                   if (isLoggedin)
                    {
                        if (_httpWrapper._CurrentResponseString.Contains("{\"d\":\"Invalid\"}"))
                        {
                            URL = "https://associates.greatship.com/pages/home.aspx";
                            _httpWrapper.Referrer = "https://associates.greatship.com/Login.aspx";

                            //if (LoadURL("a", "id", "ctl00_SupplierInfo1_lbLogOut", false))
                            if (LoadURL("a", "id", "ctl00_SupplierInfo1_lbLogOut"))
                            {
                                isLoggedin = true;
                                LogText = "Logged in successfully";
                            }
                            else isLoggedin = false;
                        }
                        else isLoggedin = false;
                    }
                    if (!isLoggedin)
                    {
                        if (iRetry == LoginRetry)
                        {
                            string filename = PrintScreenPath + "\\GreatShip_LoginFailed_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                            if (!PrintScreen(filename)) filename = "";
                            LogText = "Login failed";
                            //CreateAuditFile(filename, "GreatShip_HTTP_Processor", "", "Error", "Unable to login.",BuyerCode,SupplierCode,AuditPath);
                            CreateAuditFile(filename, "GreatShip_HTTP_Processor", "", "Error", "LeS-1014:Unable to login", BuyerCode, SupplierCode, AuditPath);
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
                    string filename = PrintScreenPath + "\\GreatShip_LoginFailed_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                    if (!PrintScreen(filename)) filename = "";
                    LogText = "Unable to load URL" + URL;
                    //CreateAuditFile(filename, "GreatShip_HTTP_Processor", "", "Error", "Unable to load URL" + URL,BuyerCode,SupplierCode,AuditPath);
                    CreateAuditFile(filename, "GreatShip_HTTP_Processor", "", "Error", "LeS-1016:Unable to load URL" + URL, BuyerCode, SupplierCode, AuditPath);
                }
            }
            catch (Exception e)
            {
                LogText = "Exception while login : " + e.GetBaseException().ToString();
                if (iRetry > LoginRetry)
                {
                    LogText = "Login failed";
                    //CreateAuditFile("", "GreatShip_HTTP_Processor", "", "Error", "Unable to login. Error : " + e.Message,BuyerCode,SupplierCode,AuditPath);
                    CreateAuditFile("", "GreatShip_HTTP_Processor", "", "Error", "LeS-1014:Unable to login due to " + e.Message, BuyerCode, SupplierCode, AuditPath);
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
                URL = "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=N";
                _httpWrapper.Referrer = "https://associates.greatship.com/pages/home.aspx";
                if (LoadURL("input", "id", "ctl00_ContentPlaceHolder1_InquirySearch1_btnInquirySearch"))
                //if (LoadURL("input", "id", "ctl00_ContentPlaceHolder1_InquirySearch1_btnInquirySearch", false))
                {
                    if (_httpWrapper._dctStateData.Count > 0)
                    {
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
                            //dtTo = Convert.ToDateTime(ToDate);//16-3-2018
                            //dtFrom = Convert.ToDateTime(FromDate);//16-3-2018
                            dtTo = DateTime.MinValue;
                            DateTime.TryParseExact(ToDate, "d/M/yyyy", null, DateTimeStyles.None, out dtTo);

                            dtFrom = DateTime.MinValue;
                            DateTime.TryParseExact(FromDate, "d/M/yyyy", null, DateTimeStyles.None, out dtFrom);
                        }


                        if (dtTo != DateTime.MinValue)
                        {
                            dt = DateTime.ParseExact(dtTo.ToShortDateString(), "M/d/yyyy", CultureInfo.InvariantCulture);//dd-MM-yyyy
                            _dateTo = dt.ToString("d/M/yyyy", CultureInfo.InvariantCulture);
                            //_dateTo = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                        }
                        if (dtFrom != DateTime.MinValue)
                        {
                            dt1 = DateTime.ParseExact(dtFrom.ToShortDateString(), "M/d/yyyy", CultureInfo.InvariantCulture);//dd-MM-yyyy
                            //_dateFrom = dt1.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            _dateFrom = dt1.ToString("d/M/yyyy", CultureInfo.InvariantCulture);
                        }

                        dctPostDataValues.Clear();
                        dctPostDataValues.Add("__EVENTTARGET", "");
                        dctPostDataValues.Add("__EVENTARGUMENT", "");
                        dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                        dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                        if (_dateFrom != "")
                            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtFromDate", Uri.EscapeDataString(_dateFrom));
                        dctPostDataValues.Add("ddlCalMonthDivFromDate", Convert.ToString(dt1.Month));
                        dctPostDataValues.Add("ddlCalYearDivFromDate", Convert.ToString(dt1.Year));
                        if (_dateTo != "")
                            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtToDate", Uri.EscapeDataString(_dateTo));
                        dctPostDataValues.Add("ddlCalMonthDivToDate", Convert.ToString(dt.Month));
                        dctPostDataValues.Add("ddlCalYearDivToDate", Convert.ToString(dt.Year));
                        dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtReference", "");
                        dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtItemDescription", "");
                        dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24btnInquirySearch", "Search");

                        _httpWrapper._SetRequestHeaders.Add(HttpRequestHeader.CacheControl, "max-age=0");
                        _httpWrapper._AddRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                        _httpWrapper._AddRequestHeaders.Remove("X-Requested-With");
                        _httpWrapper.ContentType = "application/x-www-form-urlencoded";
                        _httpWrapper.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
                        _httpWrapper.AcceptMimeType = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                        _httpWrapper.Referrer = "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=N";
                        _httpWrapper._SetRequestHeaders.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                        _httpWrapper._SetRequestHeaders.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");
                        #endregion

                        //if (PostURL("input", "id", "ctl00_ContentPlaceHolder1_InquirySearch1_btnInquirySearch", false))
                        if (PostURL("input", "id", "ctl00_ContentPlaceHolder1_InquirySearch1_btnInquirySearch"))
                        {
                            LogText = "Inquiry table filter from date: " + _dateFrom + " to date :" + _dateTo;

                            HtmlNode _message = _httpWrapper.GetElement("span", "id", "ctl00_ContentPlaceHolder1_InquirySearch1_lblmessage");
                            if (_message == null)
                            {
                                HtmlNode _spanPaging = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='ctl00_ContentPlaceHolder1_InquirySearch1_dtPagerInquirySearch']");
                                if (_spanPaging == null)
                                {
                                    DownloadRFQ(_dateFrom, _dateTo);
                                }
                                else
                                {
                                    DownloadRFQ_With_Paging(_dateFrom, _dateTo);
                                    //  WriteErrorLog_With_Screenshot("Unable to process, paging found.");
                                }

                            }
                            else
                            {
                                if (_message.InnerText.Trim().Contains("No Records Found"))
                                {
                                    //if (convert.ToDateTime(_dateFrom) > convert.ToDateTime(_dateTo))
                                    //{
                                    //    WriteErrorLog_With_Screenshot("From date cannot be Greater than To Date");
                                    //}

                                    //else
                                    //{
                                    LogText = _message.InnerText.Trim();//26-2-2018
                                    // }
                                }
                            }
                        }
                        else { //WriteErrorLog_With_Screenshot("Unable to filter Inquiry grid.");
                            WriteErrorLog_With_Screenshot("Unable to filter details", "LeS-1006:");//1006	Unable to filter details

                        }
                    }
                }
                else { //WriteErrorLog_With_Screenshot("Unable to load Inquiry grid.");
                    WriteErrorLog_With_Screenshot("Unable to filter details", "LeS-1006:");
                }
                LogText = "RFQ processing stopped.";
               _httpWrapper._SetRequestHeaders.Clear();
                _httpWrapper._AddRequestHeaders.Clear();
            }
            catch (Exception e)
            {
                //WriteErrorLog_With_Screenshot("Exception in Process RFQ : " + e.GetBaseException().ToString());
                WriteErrorLog_With_Screenshot("Unable to process file due to " + e.GetBaseException().ToString(),"LeS-1004:");
            }
        }

        public void DownloadRFQ(string _dateFrom,string dateTo)
        {
            List<string> lstData = new List<string>();
            List<string> slProcessedItem = GetProcessedItems(eActions.RFQ);
            HtmlNodeCollection _trCol = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@class='table01']/tr");
            if (_trCol != null)
            {
                if (_trCol.Count > 1)
                {
                    int i = 0;
                    LogText = "Total number of RFQs found on site: " + Convert.ToString(_trCol.Count - 1);
                    foreach (HtmlNode _tr in _trCol)
                    {
                        HtmlNodeCollection _td = _tr.ChildNodes;
                        if (!_td[1].InnerText.Contains(" Vessel Name"))
                        {
                            string _vessel = _td[1].InnerText.Trim();
                            string _refno = _td[5].InnerText.Trim();
                            string _date = _td[9].InnerText.Trim();
                            string _status = _td[7].InnerText.Trim();
                            string[] _ahref = HttpUtility.HtmlDecode(_td[5].ChildNodes[1].GetAttributeValue("href", "")).Split('(');

                            string _id = _ahref[1].Replace("','')", "").Trim('\'');
                            if (_status == "New")
                            {
                                string _guid = _vessel + "|" + _refno + "|" + _date;
                                lstData.Add(_vessel + "~" + _refno + "~" + _date + "~" + _status + "~" + _guid + "~" + _id);
                                #region delete
                                //if (!slProcessedItem.Contains(_guid))
                                //{
                                //    if (_httpWrapper._dctStateData.Count > 0)
                                //    {
                                //        #region download xls file
                                //        //string Filename = DownloadPath + "\\" + this.DocType + "_" + _refno.Replace('/', '_') + "_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".xlt";
                                //        //if (DownloadRFQ(i, dateTo, _dateFrom, "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=N", Filename, ""))
                                //        //{
                                //        //    string xlsFilename = "";
                                //        //    #region Rename xlt to xls
                                //        //    if (ConfigurationManager.AppSettings["RENAME_XLT_TO_XLS"].Trim().ToUpper() == "TRUE")
                                //        //    {
                                //        //        if (File.Exists(Filename))
                                //        //        { File.Move(Filename, Path.ChangeExtension(Filename, ".xls")); }
                                //        //        xlsFilename = Path.GetFileNameWithoutExtension(Filename) + ".xls";

                                //        //        if (File.Exists(DownloadPath + "\\" + xlsFilename))
                                //        //        {
                                //        //            LogText = xlsFilename + " downloaded successfully.";
                                //        //            CreateAuditFile(xlsFilename, "Greatship_RFQ", _refno, "Downloaded", xlsFilename + " downloaded successfully.", BuyerCode, SupplierCode, AuditPath);
                                //        //            if (_guid.Length > 0) SetGUIDs(_guid);
                                //        //        }
                                //        //        else
                                //        //        {
                                //        //            LogText = "Unable to change extension from xls to xlt file for ref: " + _refno + ".";
                                //        //            //string filename = PrintScreenPath + "\\GreatShip_RFQError_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                                //        //            CreateAuditFile(Path.GetFileName(Filename), "Greatship_RFQ", _refno, "Error", "Unable to change extension from xls to xlt file for ref: " + _refno + ".", BuyerCode, SupplierCode, AuditPath);
                                //        //        }
                                //        //    }
                                //        //    else
                                //        //    {
                                //        //        if (File.Exists(Filename))
                                //        //        {
                                //        //            LogText = Path.GetFileName(Filename) + " downloaded successfully.";
                                //        //            CreateAuditFile(Path.GetFileName(Filename), "Greatship_RFQ", _refno, "Downloaded", Path.GetFileName(Filename) + " downloaded successfully.", BuyerCode, SupplierCode, AuditPath);
                                //        //            if (_guid.Length > 0) SetGUIDs(_guid);
                                //        //        }
                                //        //        else
                                //        //        {
                                //        //            LogText = "Unable to download xlt file for ref: " + _refno + ".";
                                //        //            // string filename = PrintScreenPath + "\\GreatShip_RFQError_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                                //        //            CreateAuditFile("", "Greatship_RFQ", _refno, "Error", "Unable to download xlt file for ref: " + _refno + ".", BuyerCode, SupplierCode, AuditPath);
                                //        //            //  if (!PrintScreen(filename)) filename = "";
                                //        //        }
                                //        //    }
                                //        //    #endregion
                                //        //}
                                //        //else
                                //        //{
                                //        //    LogText = "Unable to download xlt file for ref: " + _refno + ".";
                                //        //    string filename = PrintScreenPath + "\\GreatShip_RFQError_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                                //        //    CreateAuditFile(filename, "Greatship_RFQ", _refno, "Error", "Unable to download xlt file for ref: " + _refno + ".", BuyerCode, SupplierCode, AuditPath);
                                //        //    if (!PrintScreen(filename)) filename = "";
                                //        //}
                                //        #endregion

                                //        #region open Inquiry View
                                //        DownloadXML(_dateFrom, dateTo, _refno, _guid);
                                //        #endregion
                                //    }
                                //}
                                //else LogText = "RFQ " + _refno + " already processed.";
                                #endregion
                            }
                            else
                                LogText = "Unable to download RFQ " + _refno + " as status is " + _status + ".";
                            //i++;
                        }
                        i++;
                    }

                    if (lstData.Count > 0)
                    {
                        foreach (string str in lstData)
                        {
                            string[] _arr = str.Split('~');
                            if (!slProcessedItem.Contains(_arr[4]))
                            {
                                if (_httpWrapper._dctStateData.Count > 0)
                                {
                                    #region commented on 16-3-2018 download xls file
                                    //string Filename = DownloadPath + "\\" + this.DocType + "_" + _refno.Replace('/', '_') + "_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".xlt";
                                    //if (DownloadRFQ(i, dateTo, _dateFrom, "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=N", Filename, ""))
                                    //{
                                    //    string xlsFilename = "";
                                    //    #region Rename xlt to xls
                                    //    if (ConfigurationManager.AppSettings["RENAME_XLT_TO_XLS"].Trim().ToUpper() == "TRUE")
                                    //    {
                                    //        if (File.Exists(Filename))
                                    //        { File.Move(Filename, Path.ChangeExtension(Filename, ".xls")); }
                                    //        xlsFilename = Path.GetFileNameWithoutExtension(Filename) + ".xls";

                                    //        if (File.Exists(DownloadPath + "\\" + xlsFilename))
                                    //        {
                                    //            LogText = xlsFilename + " downloaded successfully.";
                                    //            CreateAuditFile(xlsFilename, "Greatship_RFQ", _refno, "Downloaded", xlsFilename + " downloaded successfully.", BuyerCode, SupplierCode, AuditPath);
                                    //            if (_guid.Length > 0) SetGUIDs(_guid);
                                    //        }
                                    //        else
                                    //        {
                                    //            LogText = "Unable to change extension from xls to xlt file for ref: " + _refno + ".";
                                    //            //string filename = PrintScreenPath + "\\GreatShip_RFQError_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                                    //            CreateAuditFile(Path.GetFileName(Filename), "Greatship_RFQ", _refno, "Error", "Unable to change extension from xls to xlt file for ref: " + _refno + ".", BuyerCode, SupplierCode, AuditPath);
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        if (File.Exists(Filename))
                                    //        {
                                    //            LogText = Path.GetFileName(Filename) + " downloaded successfully.";
                                    //            CreateAuditFile(Path.GetFileName(Filename), "Greatship_RFQ", _refno, "Downloaded", Path.GetFileName(Filename) + " downloaded successfully.", BuyerCode, SupplierCode, AuditPath);
                                    //            if (_guid.Length > 0) SetGUIDs(_guid);
                                    //        }
                                    //        else
                                    //        {
                                    //            LogText = "Unable to download xlt file for ref: " + _refno + ".";
                                    //            // string filename = PrintScreenPath + "\\GreatShip_RFQError_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                                    //            CreateAuditFile("", "Greatship_RFQ", _refno, "Error", "Unable to download xlt file for ref: " + _refno + ".", BuyerCode, SupplierCode, AuditPath);
                                    //            //  if (!PrintScreen(filename)) filename = "";
                                    //        }
                                    //    }
                                    //    #endregion
                                    //}
                                    //else
                                    //{
                                    //    LogText = "Unable to download xlt file for ref: " + _refno + ".";
                                    //    string filename = PrintScreenPath + "\\GreatShip_RFQError_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                                    //    CreateAuditFile(filename, "Greatship_RFQ", _refno, "Error", "Unable to download xlt file for ref: " + _refno + ".", BuyerCode, SupplierCode, AuditPath);
                                    //    if (!PrintScreen(filename)) filename = "";
                                    //}
                                    #endregion

                                    #region open Inquiry View added on 16-3-2018
                                    DownloadXML(_dateFrom, dateTo, _arr[1], _arr[4],_arr[5]);
                                    #endregion
                                }
                            }
                            else LogText = "RFQ " + _arr[1] + " already processed.";
                        }
                    }

                }
                else LogText="No record found.";
            }
            else
            {
               LogText="Inquiry table is null.";
            }
        }

        public bool GetRFQHeader(ref LeSXML.LeSXML _lesXml,string eFile)
        {
            bool isResult = false;
            LogText = "Start Getting Header details";
            try
            {
                _lesXml.DocID = DateTime.Now.ToString("yyyyMMddhhmmss");
                _lesXml.Created_Date = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Doc_Type = "RFQ";
                _lesXml.Dialect = "GREAT EASTERN SHIPPING";
                _lesXml.Version = "1";
                _lesXml.Date_Document = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Date_Preparation = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Sender_Code = BuyerCode;
                _lesXml.Recipient_Code = SupplierCode;

                HtmlNode _InqNo = _httpWrapper.GetElement("span", "id", "ctl00_ContentPlaceHolder1_InquiryDetails1_lblReference");
                if (_InqNo != null)
                    _lesXml.DocReferenceID = _InqNo.InnerText.Trim();

                _lesXml.DocLinkID = this.URL;

                if (File.Exists(eFile))
                    _lesXml.OrigDocFile = Path.GetFileName(eFile);

                _lesXml.Active = "1";

                HtmlNode _vessel = _httpWrapper.GetElement("span", "id", "ctl00_ContentPlaceHolder1_InquiryDetails1_lblVessel");
                if (_vessel != null)
                    _lesXml.Vessel = _vessel.InnerText.Trim();

                _lesXml.BuyerRef = _InqNo.InnerText.Trim();

                HtmlNode _portname = _httpWrapper.GetElement("span", "id", "ctl00_ContentPlaceHolder1_InquiryDetails1_lblDeliveryPlace");
                if (_portname != null)
                    _lesXml.PortName = _portname.InnerText.Trim();
                _lesXml.Currency = "";

                HtmlNode _remarks = _httpWrapper.GetElement("span", "id", "ctl00_ContentPlaceHolder1_InquiryDetails1_lblPurchaseRemarks");
                if (_remarks != null)
                    _lesXml.Remark_Header = _remarks.InnerText.Trim();

                HtmlNode _deliverydate = _httpWrapper.GetElement("span", "id", "ctl00_ContentPlaceHolder1_InquiryDetails1_lblDateRequired");
                if (_deliverydate != null)
                {
                    if (_deliverydate.InnerText.Trim() != "")
                    {
                        DateTime dt = DateTime.MinValue;

                        string a = _deliverydate.InnerText.Trim();
                        DateTime.TryParseExact(a, "dd/MM/yyyy hh:mm:ss", null, DateTimeStyles.None, out dt);
                        if (dt != DateTime.MinValue)
                        {
                            dt = DateTime.ParseExact(dt.ToShortDateString(), "M/d/yyyy", CultureInfo.InvariantCulture);//dd-MM-yyyy
                            _lesXml.Date_Delivery = _dateTo = dt.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                        }
                    }
                    //_lesXml.Date_Delivery = Convert.ToDateTime(_deliverydate.InnerText.Trim()).ToString("yyyyMMdd");
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
            try
            {
                _lesXml.LineItems.Clear();
                LogText = "Start Getting LineItem details";
                HtmlNodeCollection _nodes = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='ctl00_ContentPlaceHolder1_InquiryDetails1_InquiryDetailsGrid']//tr");
                if (_nodes.Count >= 2)
                {
                    foreach (HtmlNode _row in _nodes)
                    {
                        LeSXML.LineItem _item = new LeSXML.LineItem();
                        try
                        {
                            HtmlNodeCollection _rowData = _row.ChildNodes;
                            if (!_rowData[1].InnerText.Trim().Contains("Sr. No."))
                            {
                                _item.Number = _rowData[1].InnerText.Trim();
                                _item.OrigItemNumber = _rowData[1].InnerText.Trim();
                                //_item.OriginatingSystemRef = _rowData[1].ChildNodes[1].GetAttributeValue("id", "");//16-4-2018
                                _item.OriginatingSystemRef = _rowData[1].ChildNodes[1].GetAttributeValue("id", "").Split('_')[4];
                                _item.Description = _rowData[2].InnerText.Trim();
                                _item.Unit = _rowData[5].InnerText.Trim();
                                if(_rowData[3].InnerText.Trim()!="")
                                _item.Quantity = _rowData[3].InnerText.Trim();
                                _item.Discount = "0";
                                _item.ListPrice = "0";
                                _item.LeadDays = "0";
                                _item.SystemRef = _rowData[1].InnerText.Trim();
                                HtmlNode _iRemarks = _rowData[10];
                                if (_iRemarks.ChildNodes[1].GetAttributeValue("value", "").Trim() != "")
                                    _item.Remark = _iRemarks.ChildNodes[1].GetAttributeValue("value", "").Trim();
                                _lesXml.LineItems.Add(_item);
                            }
                        }
                        catch (Exception ex)
                        { LogText = ex.GetBaseException().ToString(); }
                    }
                    _lesXml.Total_LineItems = Convert.ToString(_lesXml.LineItems.Count);
                    isResult = true;
                }
                else
                {
                    isResult = false;
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
                _xmlAdd.AddressName = ConfigurationManager.AppSettings["BUYER_NAME"].Trim();
                HtmlNode _footer = _httpWrapper.GetElement("td", "class", "footercont01");
                if (_footer != null)
                {
                    if (_footer.InnerText.Trim().Contains("\r\n"))
                    {
                        string[] _arrFooter = _footer.InnerText.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        if (_arrFooter.Length <= 2) _xmlAdd.Address1 = _arrFooter[0].Replace("Registered Office :", "").Trim();
                    }
                }
                _lesXml.Addresses.Add(_xmlAdd);

                _xmlAdd = new LeSXML.Address();
                _xmlAdd.Qualifier = "VN";
                _xmlAdd.AddressName = ConfigurationManager.AppSettings["SUPPLIER_NAME"].Trim();
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

        public void DownloadXML(string _dateFrom, string dateTo, string _refno, string _guid, string id)
        {
            URL = "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=N";
            dctPostDataValues.Clear();
            dctPostDataValues.Add("__EVENTTARGET", HttpUtility.UrlEncode(id));
            dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
            dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
            dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
            if (_dateFrom != "")
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtFromDate", System.Web.HttpUtility.UrlEncode(_dateFrom));
            if (dateTo != "")
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtToDate", System.Web.HttpUtility.UrlEncode(dateTo));
            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtReference", _refno);//
            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtItemDescription", "");
            //if (PostURL("table", "class", "table01", false))
            if (PostURL("table", "class", "table01"))
            {
                HtmlNode _nodeRef = _httpWrapper.GetElement("span", "id", "ctl00_ContentPlaceHolder1_InquiryView1_lblReference");
                if (_nodeRef != null)
                {
                    if (_nodeRef.InnerText.Trim() == _refno)
                    {
                        URL = "https://associates.greatship.com/Pages/InquiryView.aspx";
                        _httpWrapper.Referrer = "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=N";
                        IsUrlEncoded = true;
                        if (LoadURL("input", "id", "ctl00_ContentPlaceHolder1_InquiryView1_btnEnterQuote"))
                        {
                            LogText = "Redirect to Inquiry View page of ref no: " + _refno;
                            URL = "https://associates.greatship.com/Pages/InquiryDetails.aspx";
                            _httpWrapper.Referrer = "https://associates.greatship.com/Pages/InquiryView.aspx";
                            if (LoadURL("input", "id", "ctl00_ContentPlaceHolder1_InquiryDetails1_btnSaveQuote"))
                            {
                                LogText = "Redirect to inquiry detail page of ref no: " + _refno;
                                string eFile = PrintScreenPath + "\\" + _refno.Replace("/", "_") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
                                if (!PrintScreen(eFile)) eFile = "";
                                LeSXML.LeSXML _lesXml = new LeSXML.LeSXML();
                                if (GetRFQHeader(ref _lesXml, eFile))
                                {
                                    if (GetRFQItems(ref _lesXml))
                                    {
                                        if (GetAddress(ref _lesXml))
                                        {
                                            string xmlfile = "RFQ_" + _refno.Replace("/", "_") + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xml";
                                            if (_lesXml.Total_LineItems.Length > 0)
                                            {
                                                _lesXml.FileName = xmlfile;
                                                _lesXml.WriteXML();
                                                if (File.Exists(DownloadPath + "\\" + xmlfile))
                                                {
                                                    LogText = xmlfile + " downloaded successfully.";
                                                    LogText = "";
                                                    CreateAuditFile(xmlfile, "Greatship_RFQ", _refno, "Downloaded", xmlfile + " downloaded successfully.", BuyerCode, SupplierCode, AuditPath);
                                                    if (_guid.Length > 0) SetGUIDs(_guid);
                                                    #region go to filtered page
                                                    URL = "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=N";
                                                    _httpWrapper.Referrer = "https://associates.greatship.com/pages/home.aspx";
                                                    IsUrlEncoded = false;
                                                    if (LoadURL("input", "id", "ctl00_ContentPlaceHolder1_InquirySearch1_btnInquirySearch"))
                                                    {
                                                        dctPostDataValues.Clear();
                                                        dctPostDataValues.Add("__EVENTTARGET", "");
                                                        dctPostDataValues.Add("__EVENTARGUMENT", "");
                                                        dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                                                        dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                                                        if (_dateFrom != "")
                                                            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtFromDate", Uri.EscapeDataString(_dateFrom));
                                                        dctPostDataValues.Add("ddlCalMonthDivFromDate", Convert.ToString(dt1.Month));
                                                        dctPostDataValues.Add("ddlCalYearDivFromDate", Convert.ToString(dt1.Year));
                                                        if (_dateTo != "")
                                                            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtToDate", Uri.EscapeDataString(_dateTo));
                                                        dctPostDataValues.Add("ddlCalMonthDivToDate", Convert.ToString(dt.Month));
                                                        dctPostDataValues.Add("ddlCalYearDivToDate", Convert.ToString(dt.Year));
                                                        dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtReference", "");
                                                        dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtItemDescription", "");
                                                        dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24btnInquirySearch", "Search");

                                                        //_httpWrapper._SetRequestHeaders.Add(HttpRequestHeader.CacheControl, "max-age=0");
                                                        //_httpWrapper._AddRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                                                        _httpWrapper._AddRequestHeaders.Remove("X-Requested-With");
                                                        _httpWrapper.ContentType = "application/x-www-form-urlencoded";
                                                        _httpWrapper.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
                                                        _httpWrapper.AcceptMimeType = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                                                        _httpWrapper.Referrer = "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=N";
                                                        //_httpWrapper._SetRequestHeaders.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                                                        //_httpWrapper._SetRequestHeaders.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");
                                                        URL = "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=N";
                                                        if (PostURL("input", "id", "ctl00_ContentPlaceHolder1_InquirySearch1_btnInquirySearch"))
                                                        { }
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    LogText = "Unable to download file " + xmlfile;
                                                    string filename = PrintScreenPath + "\\GreatShip_RFQError_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                                                    //CreateAuditFile(filename, "Greatship_RFQ", _refno, "Error", "Unable to download file " + xmlfile + " for ref " + _refno + ".", BuyerCode, SupplierCode, AuditPath);
                                                    CreateAuditFile(filename, "Greatship_RFQ", _refno, "Error", "LeS-1004:Unable to process file " + xmlfile + " for ref " + _refno + ".", BuyerCode, SupplierCode, AuditPath);
                                                    if (PrintScreen(filename)) filename = "";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            LogText = "Unable to get address details";
                                            //CreateAuditFile(eFile, "Http_GreatShip_Processor", VRNO, "Error", "Unable to get address details", BuyerCode, SupplierCode, AuditPath);
                                            CreateAuditFile(eFile, "Http_GreatShip_Processor", VRNO, "Error", "LeS-1040:Unable to details - address Field(s) not present", BuyerCode, SupplierCode, AuditPath);
                                        }
                                    }
                                    else
                                    {
                                        LogText = "Unable to get RFQ item details";
                                        //CreateAuditFile(eFile, "Http_GreatShip_Processor", VRNO, "Error", "Unable to get RFQ item details", BuyerCode, SupplierCode, AuditPath);
                                        CreateAuditFile(eFile, "Http_GreatShip_Processor", VRNO, "Error", "LeS-1040:Unable to details - RFQ item Field(s) not present", BuyerCode, SupplierCode, AuditPath);
                                    }
                                }
                                else
                                {
                                    LogText = "Unable to get RFQ header details";
                                    //CreateAuditFile(eFile, "Http_GreatShip_Processor", VRNO, "Error", "Unable to get RFQ header details", BuyerCode, SupplierCode, AuditPath);
                                    CreateAuditFile(eFile, "Http_GreatShip_Processor", VRNO, "Error", "LeS-1040:Unable to details - RFQ header Field(s) not present", BuyerCode, SupplierCode, AuditPath);
                                }
                            }
                            else LogText = "Unable to redirect to inquiry detail page.";
                        }
                        else LogText = "Unable to redirect to Inquiry View Page.";
                    }
                    else
                    {
                        LogText = "Mismatch Ref no with site reference number.";
                        string eFile = PrintScreenPath + "\\" + _refno.Replace("/", "_") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
                        if (!PrintScreen(eFile)) eFile = "";
                        //CreateAuditFile(eFile, "Http_GreatShip_Processor", VRNO, "Error", "Mismatch Ref no with site reference number.", BuyerCode, SupplierCode, AuditPath);
                        CreateAuditFile(eFile, "Http_GreatShip_Processor", VRNO, "Error", "LeS-1004.3:Unable to process file due to VRNO mismatch", BuyerCode, SupplierCode, AuditPath);
                    }
                }
                else LogText = "Reference no. field not found on inquiry view page";
            }
            else LogText = "Unable to redirect to Inquiry View Page.";
        }

        public void DownloadRFQ_With_Paging(string _dateFrom,string _dateTo)
        {
            bool result = true;
            string _guid = "";
            try
            {
                List<string> slProcessedItem = GetProcessedItems(eActions.RFQ);
                HtmlNode _spanPaging = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='ctl00_ContentPlaceHolder1_InquirySearch1_dtPagerInquirySearch']");
                if (_spanPaging != null)
                {
                    HtmlNodeCollection _pagingNodes = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//span[@id='ctl00_ContentPlaceHolder1_InquirySearch1_dtPagerInquirySearch']//a");
                    if (_pagingNodes != null)
                    {
                        int TotalPages = 0;
                        foreach (HtmlNode _page in _pagingNodes)
                        {
                            if (_page.InnerText.Trim() == "Next")
                            {
                                TotalPages = convert.ToInt(_page.PreviousSibling.PreviousSibling.InnerText.Trim());
                                LogText = "Total " + TotalPages + " pages found.";
                                break;
                            }
                        }

                        int j = 0; 
                        for (int i = 1; i <= TotalPages; i++)
                        {
                            #region next page
                            if (i > 1)
                            {
                                if (_httpWrapper._dctStateData.Count > 0)
                                {
                                    _httpWrapper.Referrer = URL;
                                    dctPostDataValues.Clear();
                                    if(i<=9)
                                    dctPostDataValues.Add("__EVENTTARGET","ctl00%24ContentPlaceHolder1%24InquirySearch1%24dtPagerInquirySearch%24ctl01%24ctl0"+(i-1));//(j+1)
                                    else
                                        dctPostDataValues.Add("__EVENTTARGET", "ctl00%24ContentPlaceHolder1%24InquirySearch1%24dtPagerInquirySearch%24ctl01%24ctl1" + (i-1));//(j + 1)
                                    dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                                    dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                                    dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtFromDate", _dateFrom);
                                    dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtToDate", _dateTo);
                                    IsUrlEncoded = false;
                                    if (PostURL("span", "class", "currentPage"))
                                    {
                                        result = true;
                                    }
                                }
                            }
                            #endregion

                            if (result == true)
                            {
                                HtmlNodeCollection _trCol = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@class='table01']//tr");
                                if (_trCol != null)
                                {
                                    if (_trCol.Count > 1)
                                    {
                                      //  int c = -1;
                                        foreach (HtmlNode _tr in _trCol)
                                        {
                                            HtmlNodeCollection _td = _tr.ChildNodes;
                                            if (!_td[1].InnerText.Contains(" Vessel Name"))
                                            {
                                                string _vessel = _td[1].InnerText.Trim();
                                                string _refno = _td[5].InnerText.Trim();
                                                string _date = _td[9].InnerText.Trim();
                                                string _status = _td[7].InnerText.Trim();
                                                string[] _ahref = HttpUtility.HtmlDecode(_td[5].ChildNodes[1].GetAttributeValue("href", "")).Split('(');

                                                string _id = _ahref[1].Replace("','')", "").Trim('\'');
                                                if (_status == "New")
                                                {
                                                    _guid = _vessel + "|" + _refno + "|" + _date;
                                                    if (!slProcessedItem.Contains(_guid))
                                                    {
                                                     //testing
                                                       // lstNewRFQs.Add(j + "|" + _refno + "|" + _vessel + "|" + _date +"|"+_id);
                                                        //LogText = "RFQ " + _refno + " added into list.";
                                                        DownloadXML(_dateFrom, _dateTo, _refno, _vessel + "|" + _refno + "|" + _date, _id);
                                                        #region next page
                                                        if (i > 1)
                                                        {
                                                            if (_httpWrapper._dctStateData.Count > 0)
                                                            {
                                                                _httpWrapper.Referrer = URL;
                                                                dctPostDataValues.Clear();
                                                                if (i <= 9)
                                                                    dctPostDataValues.Add("__EVENTTARGET", "ctl00%24ContentPlaceHolder1%24InquirySearch1%24dtPagerInquirySearch%24ctl01%24ctl0" + (i - 1));//(j+1)
                                                                else
                                                                    dctPostDataValues.Add("__EVENTTARGET", "ctl00%24ContentPlaceHolder1%24InquirySearch1%24dtPagerInquirySearch%24ctl01%24ctl1" + (i - 1));//(j + 1)
                                                                dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                                                                dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                                                                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtFromDate", _dateFrom);
                                                                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtToDate", _dateTo);
                                                                if (PostURL("span", "class", "currentPage"))
                                                                {
                                                                    result = true;
                                                                }
                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                    else LogText = "RFQ " + _refno + " already processed.";
                                                }
                                                else
                                                    LogText = "Unable to download RFQ " + _refno + " as status is " + _status + ".";
                                                j++;
                                            }
                                           // c++;
                                        }
                                        j = 0; //c = -1;
                                        result = false;
                                    }
                                }
                            }
                        }
                       // j = 0;
                      //  c = 0;

                        //testing
                        //if (lstNewRFQs.Count > 0)
                        //{
                        //    LogText = "Total number of RFQs found on site: " + lstNewRFQs.Count;
                        //    foreach (string _record in lstNewRFQs)
                        //    {
                        //        int counter = convert.ToInt(_record.Split('|')[0]);
                        //        string refno = _record.Split('|')[1];
                        //        string vessel = _record.Split('|')[2];
                        //        string date = _record.Split('|')[3];
                        //        string id = _record.Split('|')[4];
                        //        //  int cter = convert.ToInt(_record.Split('|')[4]);

                        //        #region commented on 16-3-2018(xls download part)
                        //        //if (DownloadRFQ(counter, _dateTo, _dateFrom, "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=N", DownloadPath + "\\" + this.DocType + "_" + refno.Replace('/','_') + "_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".xlt", ""))
                        //        //{
                        //        //    LogText = this.DocType + "_" + refno.Replace('/', '_') + "_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".xlt downloaded successfully.";
                        //        //    CreateAuditFile(this.DocType + "_" + refno.Replace('/', '_') + "_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".xlt", "Greatship_RFQ", refno, "Downloaded", this.DocType + "_" + refno.Replace('/', '_') + "_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".xlt downloaded successfully.",BuyerCode,SupplierCode,AuditPath);
                        //        //    _guid = vessel + "|" + refno + "|" + date;
                        //        //    if (_guid.Length > 0) SetGUIDs(_guid);
                        //        //}
                        //        //else
                        //        //{
                        //        //    LogText = "Unable to download xlt file for ref: " + refno + ".";
                        //        //    string filename = PrintScreenPath + "\\GreatShip_RFQError_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                        //        //    CreateAuditFile(filename, "Greatship_RFQ", refno, "Error", "Unable to download xlt file for ref: " + refno + ".",BuyerCode,SupplierCode,AuditPath);
                        //        //    if (!PrintScreen(filename)) filename = "";
                        //        //}
                        //        #endregion

                        //        #region added on 16-3-2018 to ownload xml file
                        //        DownloadXML(_dateFrom, _dateTo, refno, vessel + "|" + refno + "|" + date, id);
                        //        #endregion
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteErrorLog_With_Screenshot(ex.Message.ToString());
                WriteErrorLog_With_Screenshot("Unable to process file due to " + ex.Message.ToString(), "LeS-1004:");
            }
        }

        public void SetGUIDs(string GUID)
        {
            using (StreamWriter sw = new StreamWriter(@AppDomain.CurrentDomain.BaseDirectory + "RFQ_Downloaded.txt", true))
            {
                sw.WriteLine(GUID);
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
        }      

        public bool DownloadRFQ(int i,string dateTo,string dateFrom,string RequestURL, string DownloadFileName, string ContentType = "Application/pdf")
        {
            bool _result = false;
            try
            {
                dctPostDataValues.Clear();
                dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]); ;
                dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtFromDate", dateFrom);
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtToDate", dateTo);
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24lvInquirySearch%24ctrl" + i + "%24ibDownload_Excel.x", "6");
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24lvInquirySearch%24ctrl" + i + "%24ibDownload_Excel.y", "6");
                URL = RequestURL;
                //if (DownloadUrl("", "", "", DownloadFileName, false))
                //{
                if (DownloadRFQ(URL, DownloadFileName, "Application/unknown"))
                    _result = (File.Exists(DownloadFileName));
               // }
            }
            catch (Exception e)
            {
                throw e;
            }
            return _result;
        }

        public List<string> GetProcessedItems(eActions eAction)
        {
            List<string> slProcessedItems = new List<string>();
            switch (eAction)
            {
                case eActions.RFQ: sDoneFile = AppDomain.CurrentDomain.BaseDirectory + "RFQ_Downloaded.txt"; break;
                default: break;
            }
            if (File.Exists(sDoneFile))
            {
                string[] _Items = File.ReadAllLines(sDoneFile);
                slProcessedItems.AddRange(_Items.ToList());
            }
            return slProcessedItems;
        }

        public override bool DownloadRFQ(string RequestURL, string DownloadFileName, string ContentType = "Application/pdf")
        {
            bool _result = false;
            try
            {
                URL = RequestURL;
                //if (PostURL("", "", "", false, false))
                if (PostURL("", "", ""))//added on 23-3-2018 as wrapper is changed
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

        #region Quote
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
                        URL = "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=N";
                        _httpWrapper.Referrer = "https://associates.greatship.com/pages/home.aspx";
                        IsUrlEncoded = false;
                        if (LoadURL("input", "id", "ctl00_ContentPlaceHolder1_InquirySearch1_btnInquirySearch"))
                        {
                            ProcessQuoteMTML(xmlFiles[j]);
                        }
                    }
                }
                else LogText = "No quote file found.";
                LogText = "Quote processing stopped.";
            }
            catch (Exception e)
            {
                //WriteErrorLogQuote_With_Screenshot("Exception in Process Quote : " + e.GetBaseException().ToString(),xmlFiles[j]);
                WriteErrorLogQuote_With_Screenshot("Unable to process file due to " + e.GetBaseException().ToString(), xmlFiles[j],"LeS-1004");
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

        public void ProcessQuoteMTML(string MTML_QuoteFile)
        {
            try
            {
                MTMLClass _mtml = new MTMLClass();
                _interchange = _mtml.Load(MTML_QuoteFile);
                LoadInterchangeDetails();
                if (UCRefNo != "")
                {
                    #region filter inquiry table by ref no
                    string _fromDate = "", _toDate = "";
                    filter_InquiryTable_For_NewRefNo(ref _fromDate, ref _toDate);
                    #endregion

                    IsUrlEncoded = false;
                    if (PostURL("table", "class", "table01"))
                    {
                        HtmlNodeCollection _nodeTable = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@class='table01']//tr");
                        if (_nodeTable != null)
                        {
                            if (_nodeTable.Count >= 2)
                            {
                                bool res = false;
                                foreach (HtmlNode _node in _nodeTable)
                                {

                                    //HtmlNode _tr = _nodeTable[1];
                                    HtmlNode _tr = _node;
                                    if (_tr != null)
                                    {
                                        if (_tr.ChildNodes[1].InnerText.ToUpper().Trim() != "VESSEL NAME")
                                        {
                                            HtmlNodeCollection _td = _tr.ChildNodes;
                                            string _status = _td[7].InnerText.Trim().ToUpper();
                                            string RefNo = _td[5].InnerText.Trim();
                                            string Vessel = _td[1].InnerText.Trim();
                                            if (RefNo == UCRefNo && Vessel == VesselName)
                                            {
                                                res = true;
                                                if (_status == "NEW")
                                                {
                                                    _httpWrapper.Referrer = URL = "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=N";
                                                    if (_httpWrapper._dctStateData.Count > 0)
                                                    {
                                                        dctPostDataValues.Clear();
                                                        dctPostDataValues.Add("__EVENTTARGET", "ctl00%24ContentPlaceHolder1%24InquirySearch1%24lvInquirySearch%24ctrl0%24lnkShipsref");
                                                        dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
                                                        dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                                                        dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                                                        if (_fromDate != "")
                                                            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtFromDate", System.Web.HttpUtility.UrlEncode(_fromDate));
                                                        if (_toDate != "")
                                                            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtToDate", System.Web.HttpUtility.UrlEncode(_toDate));
                                                        dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtReference", UCRefNo);
                                                        dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtItemDescription", "");
                                                        if (PostURL("table", "class", "table01"))
                                                        {
                                                            URL = "https://associates.greatship.com/Pages/InquiryView.aspx";
                                                            _httpWrapper.Referrer = "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=N";

                                                            if (LoadURL("input", "id", "ctl00_ContentPlaceHolder1_InquiryView1_btnEnterQuote"))
                                                            {
                                                                LogText = "Redirect to Inquiry View page of ref no: " + UCRefNo;

                                                                URL = "https://associates.greatship.com/Pages/InquiryDetails.aspx";
                                                                _httpWrapper.Referrer = "https://associates.greatship.com/Pages/InquiryView.aspx";
                                                                IsUrlEncoded = true;
                                                                if (LoadURL("input", "id", "ctl00_ContentPlaceHolder1_InquiryDetails1_btnSaveQuote"))
                                                                {

                                                                    LogText = "Redirect to inquiry detail page of ref no: " + UCRefNo;
                                                                    HtmlNodeCollection _trItems = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='ctl00_ContentPlaceHolder1_InquiryDetails1_InquiryDetailsGrid']//tr");
                                                                    if (_trItems != null)
                                                                    {
                                                                        if (_trItems.Count > 1)
                                                                        {
                                                                            int _itemcount = _trItems.Count - 1;
                                                                            if (_itemcount == _lineitem.Count)
                                                                            {
                                                                                // IsUrlEncoded = false;
                                                                                if (SaveQuote(_trItems, MTML_QuoteFile, _fromDate, _toDate))
                                                                                {
                                                                                    if (IsDecline)
                                                                                    {
                                                                                        DeclineQuote(MTML_QuoteFile);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        SubmitQuote(MTML_QuoteFile);
                                                                                    }
                                                                                }
                                                                            }
                                                                            else throw new Exception("Website line item count not matched with MTML file line item count.");
                                                                        }
                                                                        else throw new Exception("no item found for RFQ " + UCRefNo);
                                                                    }
                                                                }
                                                                else
                                                                    LogText = "Unable to redirect to inquiry detail page for ref " + UCRefNo;
                                                            }
                                                            else LogText = "Unable to redirect to inquiry view page for ref " + UCRefNo;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    //WriteErrorLogQuote_With_Screenshot("Unable to process quote as status is '" + _status + "' for ref no " + UCRefNo, Path.GetFileName(MTML_QuoteFile)); 
                                                    WriteErrorLogQuote_With_Screenshot("Unable to Save Quote since Quote is in '" + _status + "' status for ref no " + UCRefNo, Path.GetFileName(MTML_QuoteFile), "LeS-1008.1:");
                                                }
                                            }
                                            //else
                                            //{
                                            //    if (_status != "NEW")
                                            //        WriteErrorLogQuote_With_Screenshot("Unable to process quote as status is '" + _status + "' for ref no " + UCRefNo, Path.GetFileName(MTML_QuoteFile));
                                            //    //else if (RefNo == UCRefNo)
                                            //    //    WriteErrorLogQuote_With_Screenshot("Error while filtering ref no " + UCRefNo, Path.GetFileName(MTML_QuoteFile));
                                            //}
                                        }
                                    }
                                }
                                if (res == false)
                                {
                                    //WriteErrorLogQuote_With_Screenshot("Error while filtering ref no: " + UCRefNo, Path.GetFileName(MTML_QuoteFile));
                                    WriteErrorLogQuote_With_Screenshot("Unable to filter details for " + UCRefNo, Path.GetFileName(MTML_QuoteFile), "LeS-1006:");
                                }
                            }
                            //else if (_nodeTable.Count > 2)//new condition
                            //    WriteErrorLogQuote_With_Screenshot("Multiple records found for same ref no.: " + UCRefNo, Path.GetFileName(MTML_QuoteFile));
                            else if (_nodeTable.Count < 2)
                            {// WriteErrorLogQuote_With_Screenshot("No record found for ref no: " + UCRefNo, Path.GetFileName(MTML_QuoteFile));
                                WriteErrorLogQuote_With_Screenshot("Unable to filter details for " + UCRefNo, Path.GetFileName(MTML_QuoteFile), "LeS-1006:");
                            }
                        }
                    }
                    else
                    {
                        LogText = "Unable to filter inquery table for ref "+UCRefNo;
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteErrorLogQuote_With_Screenshot("Exception while processing Quote MTML: " + ex.GetBaseException().Message.ToString(), Path.GetFileName(MTML_QuoteFile));
                WriteErrorLogQuote_With_Screenshot("Unable to process file due to " + ex.GetBaseException().Message.ToString(), Path.GetFileName(MTML_QuoteFile),"LeS-1004:");
            }
        }

        public void filter_InquiryTable_For_NewRefNo(ref string _fromDate,ref string _toDate)
        {
            LogText = "Filter table by ref no " + UCRefNo;
            _httpWrapper.Referrer = URL = "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=N";

            if (_httpWrapper._dctStateData.Count > 0)
            {
                 _fromDate = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//input[@id='ctl00_ContentPlaceHolder1_InquirySearch1_txtFromDate']").GetAttributeValue("value", "");
                 _toDate = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//input[@id='ctl00_ContentPlaceHolder1_InquirySearch1_txtToDate']").GetAttributeValue("value", "");

                _httpWrapper._SetRequestHeaders.Add(HttpRequestHeader.CacheControl, "max-age=0");
                _httpWrapper._AddRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                _httpWrapper.ContentType = "application/x-www-form-urlencoded";
                _httpWrapper.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
                _httpWrapper.AcceptMimeType = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                _httpWrapper._SetRequestHeaders.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                _httpWrapper._SetRequestHeaders.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");

                dctPostDataValues.Clear();
                dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                if (_fromDate != "")
                    dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtFromDate", System.Web.HttpUtility.UrlEncode(_fromDate));
                if (_toDate != "")
                    dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtToDate", System.Web.HttpUtility.UrlEncode(_toDate));
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtReference", UCRefNo);
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24btnInquirySearch", "Search");
            }
        }

        public bool SaveQuote(HtmlNodeCollection _trItems, string MTML_QuoteFile, string _fromDate, string _toDate)
        {
            bool result = false;
            try
            {
                HtmlNode _spanRefno = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='ctl00_ContentPlaceHolder1_InquiryDetails1_lblReference']");
                if (_spanRefno != null)
                {
                    if (_spanRefno.InnerText.Trim() == UCRefNo)
                    {
                        if (_httpWrapper._dctStateData.Count > 0)
                        {
                            URL = _httpWrapper.Referrer = "https://associates.greatship.com/Pages/InquiryDetails.aspx";
                            _httpWrapper._AddRequestHeaders["Origin"] = @"https://associates.greatship.com";
                            dctPostDataValues.Clear();
                            dctPostDataValues.Add("__EVENTTARGET", "");
                            dctPostDataValues.Add("__EVENTARGUMENT", "");
                            dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                            dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                            dctPostDataValues.Add("__EVENTVALIDATION", _httpWrapper._dctStateData["__EVENTVALIDATION"]);

                            Assign_HeaderDetails();
                            Assign_LineItemDetails(_trItems);
                            Assign_MonetoryAmounts();

                            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24btnSaveQuote", "Save");
                            string message = "";
                            if (PostURL("span", "id", "ctl00_ContentPlaceHolder1_lblPostMessage"))
                            {
                                message = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='ctl00_ContentPlaceHolder1_lblPostMessage']").InnerText.Trim();
                                LogText = message;
                                if (message.Contains("successfully saved"))
                                {
                                    //#region going back to partially saved page
                                    //_httpWrapper.Referrer = "https://associates.greatship.com/Pages/Home.aspx";
                                    //URL = "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=P";
                                    //if (LoadURL("input", "id", "ctl00_ContentPlaceHolder1_InquirySearch1_btnInquirySearch", false))
                                    //{
                                    #region filter on partially saved page
                                    if (_httpWrapper._dctStateData.Count > 0)
                                    {
                                        _httpWrapper._AddRequestHeaders.Remove("X-Requested-With");

                                        dctPostDataValues.Clear();
                                        dctPostDataValues.Add("__EVENTTARGET", "");
                                        dctPostDataValues.Add("__EVENTARGUMENT", "");
                                        dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                                        dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                                        if (_fromDate != "")
                                            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtFromDate", Uri.EscapeDataString(_fromDate));
                                        if (_toDate != null)
                                            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtToDate", Uri.EscapeDataString(_toDate));
                                        dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtReference", UCRefNo);
                                        dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtItemDescription", "");
                                        dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24btnInquirySearch", "Search");
                                        _httpWrapper.Referrer = URL = "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=P";
                                        if (PostURL("input", "id", "ctl00_ContentPlaceHolder1_InquirySearch1_btnInquirySearch"))
                                        {

                                            #region Going to rfq detail page from partially saved page
                                            _httpWrapper.Referrer = URL = "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=P";
                                            if (_httpWrapper._dctStateData.Count > 0)
                                            {
                                                LogText = "Redirect from partially saved page to inquiry detail page for ref " + UCRefNo;
                                                dctPostDataValues.Clear();
                                                dctPostDataValues.Add("__EVENTTARGET", "ctl00%24ContentPlaceHolder1%24InquirySearch1%24lvInquirySearch%24ctrl0%24lnkShipsref");
                                                dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                                                dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                                                if (_fromDate != "")
                                                    dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtFromDate", System.Web.HttpUtility.UrlEncode(_fromDate));
                                                if (_toDate != "")
                                                    dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtToDate", System.Web.HttpUtility.UrlEncode(_toDate));
                                                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtReference", UCRefNo);
                                                if (PostURL("input", "id", "ctl00_ContentPlaceHolder1_InquiryDetails1_btnSubmitQuote"))
                                                {
                                                    HtmlNode pageTitle = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//h1[@class='pagetitle02']");
                                                    if (pageTitle != null)
                                                    {
                                                        if (pageTitle.InnerText.Trim().Contains("Enter Quotation"))
                                                        {
                                                            result = true;
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                        else LogText = "Unable to redirect from partially saved page to inquiry detail page for ref " + UCRefNo;
                                    }
                                    #endregion
                                    // }
                                    // #endregion
                                }
                                else WriteErrorLogQuote_With_Screenshot("Unable to Save Quote due to "+message, MTML_QuoteFile, "LeS-1008");

                            }
                            //else WriteErrorLogQuote_With_Screenshot("Error in saving quote details", MTML_QuoteFile);
                            else WriteErrorLogQuote_With_Screenshot("Unable to Save Quote as Post URL loading failed", MTML_QuoteFile, "LeS-1008.7:");
                        }//1008.7	Unable to Save  quote, Post URL loading failed

                    }
                }//1011.2	Unable to submit quote, Post URL loading failed

                else { LogText = "Reference no is null."; result = false; }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public void SubmitQuote(string MTML_QuoteFile)
        {
            string _fromDate = "", _toDate = "";
            try
            {
                double IGrandTotal = 0.0f, PackingCost = 0.0f, FreightCost = 0.0f, PTotal = 0.0f;
                if (_httpWrapper.GetElement("input", "id", "ctl00_ContentPlaceHolder1_InquiryDetails1_txtGrandtotal").GetAttributeValue("value", "").Trim() != null)
                    IGrandTotal = Convert.ToDouble(_httpWrapper.GetElement("input", "id", "ctl00_ContentPlaceHolder1_InquiryDetails1_txtGrandtotal").GetAttributeValue("value", "").Trim());

                if (_httpWrapper.GetElement("input", "id", "ctl00_ContentPlaceHolder1_InquiryDetails1_txtPackingCharges") != null)
                    PackingCost = Convert.ToDouble(_httpWrapper.GetElement("input", "id", "ctl00_ContentPlaceHolder1_InquiryDetails1_txtPackingCharges").GetAttributeValue("value", "").Trim());

                if (_httpWrapper.GetElement("input", "id", "ctl00_ContentPlaceHolder1_InquiryDetails1_txtInlandFreightCharges") != null && _httpWrapper.GetElement("input", "id", "ctl00_ContentPlaceHolder1_InquiryDetails1_txtInlandFreightCharges").GetAttributeValue("value", "").Trim() != "")
                    FreightCost = Convert.ToDouble(_httpWrapper.GetElement("input", "id", "ctl00_ContentPlaceHolder1_InquiryDetails1_txtInlandFreightCharges").GetAttributeValue("value", "").Trim());

                PTotal = IGrandTotal + PackingCost + FreightCost;
                if (Convert.ToInt32(Convert.ToDouble(GrandTotal)) == Convert.ToInt32(PTotal))
                {
                    string eFile = PrintScreenPath + "\\GreatshipQuote_BeforeSubmit_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                    if (!PrintScreen(eFile)) eFile = "";

                    URL = _httpWrapper.Referrer = "https://associates.greatship.com/Pages/InquiryDetails.aspx";

                    if (_httpWrapper._dctStateData.Count > 0)
                    {
                        SubmitQuote_PostData();
                        if (PostURL("span", "id", "ctl00_ContentPlaceHolder1_lblPostMessage"))
                        {
                            string message = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='ctl00_ContentPlaceHolder1_lblPostMessage']").InnerText.Trim();
                            LogText = message;
                            if (message.Contains("Your quote has been successfully submited"))
                            {
                                #region filter on submitted page
                                URL = "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=N";
                                _httpWrapper.Referrer = "https://associates.greatship.com/pages/home.aspx";
                                if (LoadURL("input", "id", "ctl00_ContentPlaceHolder1_InquirySearch1_btnInquirySearch"))
                                {
                                    _httpWrapper.Referrer = URL = "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=N";

                                    if (_httpWrapper._dctStateData.Count > 0)
                                    {
                                        _fromDate = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//input[@id='ctl00_ContentPlaceHolder1_InquirySearch1_txtFromDate']").GetAttributeValue("value", "");
                                        _toDate = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//input[@id='ctl00_ContentPlaceHolder1_InquirySearch1_txtToDate']").GetAttributeValue("value", "");

                                        dctPostDataValues.Clear();
                                        dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                                        dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                                        if (_fromDate != "")
                                            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtFromDate", System.Web.HttpUtility.UrlEncode(_fromDate));
                                        if (_toDate != "")
                                            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtToDate", System.Web.HttpUtility.UrlEncode(_toDate));
                                        dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtReference", UCRefNo);
                                        dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24btnInquirySearch", "Search");
                                        if (PostURL("table", "class", "table01"))
                                        {
                                            SuccessQuote(MTML_QuoteFile, _fromDate, _toDate);
                                        }
                                        else LogText = "Unable to redirect to inquiry detail page after successful submission of quote.";
                                    }

                                #endregion
                                }
                            }
                            else
                            {
                                eFile = PrintScreenPath + "\\GreatshipQuote_QuoteError_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                                if (!PrintScreen(eFile)) eFile = "";
                                if (!File.Exists(Path.GetDirectoryName(MTML_QuoteFile) + "\\Error\\" + Path.GetFileName(MTML_QuoteFile)))
                                    File.Delete(Path.GetDirectoryName(MTML_QuoteFile) + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
                                File.Move(MTML_QuoteFile, Path.GetDirectoryName(MTML_QuoteFile) + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
                                //CreateAuditFile(Path.GetFileName(MTML_QuoteFile), "Http_Greatship_Processor", UCRefNo, "Error", "Unable to submit quote for ref " + UCRefNo, BuyerCode, SupplierCode, AuditPath);
                                CreateAuditFile(Path.GetFileName(MTML_QuoteFile), "Http_Greatship_Processor", UCRefNo, "Error", "LeS-1011:Unable to submit quote for ref " + UCRefNo, BuyerCode, SupplierCode, AuditPath);
                            }
                        }
                        else LogText = "Unable to redirect to meassage page after submission of quote";
                    }
                }
                else
                {
                    //WriteErrorLogQuote_With_Screenshot("Unable to upload Quote for Ref : '" + UCRefNo + "' due to Total Amount '" + GrandTotal + "' mismatch on site Grand Total '" + PTotal + "'.", MTML_QuoteFile);
                    WriteErrorLogQuote_With_Screenshot("Unable to save quote due to amount mismatch", MTML_QuoteFile, "LeS-1008.1:");
                }
            }
            catch (Exception ex)
            {
                //WriteErrorLogQuote_With_Screenshot("Error while submitting quote: " + ex.Message + Environment.NewLine + ex.StackTrace, Path.GetFileName(MTML_QuoteFile));
                WriteErrorLogQuote_With_Screenshot("Unable to Submit Quote due to " + ex.Message + Environment.NewLine + ex.StackTrace, Path.GetFileName(MTML_QuoteFile), "LeS-1011:");
            }
        }

        public void SubmitQuote_PostData()
        {
            dctPostDataValues.Clear();
            dctPostDataValues.Add("__EVENTTARGET", _httpWrapper._dctStateData["__EVENTTARGET"]);
            dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
            dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
            dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
            dctPostDataValues.Add("__EVENTVALIDATION", _httpWrapper._dctStateData["__EVENTVALIDATION"]);
            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24ddlCurrency", Currency);
            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24txtDeliveryPeriod", LeadDays);
            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24txtoffervalid", dtExpDate);
            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24txtPaymentTerms", Uri.EscapeDataString(PayTerms));
            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24txtSupplierReference", Uri.EscapeDataString(AAGRefNo));
            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24rbtn_AsperSpec", "Y");
            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24ddlPriceBasis", "P00001");
            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24ddlCountry", Uri.EscapeDataString("China, Republic Of").Replace("%20", "+"));
            HtmlNodeCollection _trItems = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='ctl00_ContentPlaceHolder1_InquiryDetails1_InquiryDetailsGrid']//tr");
            if (_trItems != null)
            {
                if (_trItems.Count > 1)
                {
                    int _itemcount = _trItems.Count - 1;
                    if (_itemcount == _lineitem.Count)
                    {
                        Assign_LineItemDetails(_trItems);
                    }
                }
            }
            Assign_MonetoryAmounts();
            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24btnSubmitQuote", "Submit");
        }

        public void SuccessQuote(string MTML_QuoteFile,string _fromDate,string _toDate)
        {
             HtmlNodeCollection _nodeTable = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@class='table01']//tr");
             if (_nodeTable != null)
             {
                 if (_nodeTable.Count == 2)
                 {
                     HtmlNode _tr = _nodeTable[1];
                     if (_tr != null)
                     {
                         HtmlNodeCollection _td = _tr.ChildNodes;
                         string _status = _td[7].InnerText.Trim().ToUpper();
                         string RefNo = _td[5].InnerText.Trim();
                         if (_status == "SUBMITTED" && RefNo == UCRefNo)
                         {
                             URL = _httpWrapper.Referrer = "https://associates.greatship.com/Pages/InquirySearch.aspx?Type=S";
                             if (_httpWrapper._dctStateData.Count > 0)
                             {
                                 dctPostDataValues.Clear();
                                 dctPostDataValues.Add("__EVENTTARGET", "ctl00%24ContentPlaceHolder1%24InquirySearch1%24lvInquirySearch%24ctrl0%24lnkShipsref");
                                 dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
                                 dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                                 dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                                 if (_fromDate != "")
                                     dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtFromDate", System.Web.HttpUtility.UrlEncode(_fromDate));
                                 if (_toDate != "")
                                     dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtToDate", System.Web.HttpUtility.UrlEncode(_toDate));
                                 dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtReference", UCRefNo);
                                 dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquirySearch1%24txtItemDescription", "");
                                 if (PostURL("h1", "class", "pagetitle02"))
                                 {
                                     string message1 = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//h1[@class='pagetitle02']").InnerText.Trim();
                                     if (message1.Contains("Quotation View"))
                                     {
                                         string eFile = PrintScreenPath + "\\GreatshipQuote_AfterSubmit_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                                         if (!PrintScreen(eFile)) eFile = "";
                                         if (!Directory.Exists(Path.GetDirectoryName(MTML_QuoteFile) + "\\Backup")) Directory.CreateDirectory(Path.GetDirectoryName(MTML_QuoteFile) + "\\Backup");
                                         if (File.Exists(Path.GetDirectoryName(MTML_QuoteFile) + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile)))
                                             File.Delete(Path.GetDirectoryName(MTML_QuoteFile) + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile));
                                         File.Move(MTML_QuoteFile, Path.GetDirectoryName(MTML_QuoteFile) + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile));
                                         CreateAuditFile(Path.GetFileName(MTML_QuoteFile), "Http_Greatship_Processor", UCRefNo, "Success", UCRefNo + " quote submitted successfully.", BuyerCode, SupplierCode, AuditPath);
                                     }
                                 }
                             }
                         }
                     }
                 }
             }
        }

        public void DeclineQuote(string MTML_QuoteFile)
        {
            try
            {

            }
            catch (Exception ex)
            {
                //WriteErrorLogQuote_With_Screenshot("Error while decline quote: " + ex.Message + Environment.NewLine + ex.StackTrace, Path.GetFileName(MTML_QuoteFile));
                WriteErrorLogQuote_With_Screenshot("Unable to decline quote due to " + ex.Message + Environment.NewLine + ex.StackTrace, Path.GetFileName(MTML_QuoteFile), "Les-1021L");
            }
        }

        public void Assign_HeaderDetails()
        {
            #region Header details
            LogText = "Started assigning Header Details.";
            if (Currency != "")
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24ddlCurrency", Currency);
            else throw new Exception("Currency is blank.");

            if (LeadDays != "")
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24txtDeliveryPeriod", LeadDays);
            else throw new Exception("Delivery Period is blank.");

            if (dtExpDate != "")
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24txtoffervalid", Uri.EscapeDataString(dtExpDate));
            else if (ConfigurationManager.AppSettings["QUOTE_VALID_UNTIL_DAYS"].Trim() != null)
            {
                DateTime dt=DateTime.Now.AddDays(Convert.ToInt32(ConfigurationManager.AppSettings["QUOTE_VALID_UNTIL_DAYS"].Trim()));
                dtExpDate=dt.ToString("d/M/yyyy");//15-3-2018
               // dtExpDate = dt.ToShortDateString();//15-3-2018
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24txtoffervalid", Uri.EscapeDataString(dtExpDate));
            }
            else
                throw new Exception("Quote Valid till is blank.");

            DateTime dt1 = DateTime.MinValue;
            DateTime.TryParseExact(dtExpDate, "d/M/yyyy", null, DateTimeStyles.None, out dt1);
            if (dt1 != DateTime.MinValue)
            {
                dctPostDataValues.Add("ddlCalMonthDivoffervalid", Convert.ToString(dt1.Month));
                dctPostDataValues.Add("ddlCalYearDivoffervalid", Convert.ToString(dt1.Year));
            }
            else throw new Exception("Unable to convert expiration date.");

            if (PayTerms != "")
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24txtPaymentTerms", Uri.EscapeDataString(PayTerms.Trim()));
            else throw new Exception("PaymentTerms is blank.");

            if (AAGRefNo != "")
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24txtSupplierReference", Uri.EscapeDataString(AAGRefNo));
            else throw new Exception("Supplier Reference is blank");

            string AsperSpec = "Y";// ‘As per Specification’ - Yes
            if (AsperSpec != "")
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24rbtn_AsperSpec", AsperSpec);
            else throw new Exception("As per specification is blank.");

            string priceBasis = "P00001";// ‘Price Basis’ - EX-WORKS(EXW)
            if (priceBasis != "")
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24ddlPriceBasis", priceBasis);
            else throw new Exception("Price Basis is blank.");

            string country = "China, Republic Of";// ‘Country’ - CHINA, REPUBLIC OF
            if (country != "")
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24ddlCountry", Uri.EscapeDataString(country).Replace("%20","+"));
            else throw new Exception("Country is blank.");

            LogText = "Header Details assignment stopped.";
            #endregion
        }

        public void Assign_LineItemDetails(HtmlNodeCollection _trItems)
        {
            #region Line items;
            LogText = "Assigning Line Item Details.";
            LineItem _item = null;
            int i = 0;
            foreach (HtmlNode _tr in _trItems)
            {
                i++;
                HtmlNodeCollection _td = _tr.ChildNodes;
                if (!_td[1].InnerText.Contains("Sr. No."))
                {
                    foreach (LineItem item in _lineitem)
                    {
                        //if (item.Number.ToString() == _td[1].InnerText.Trim().ToString()) { _item = item; }
                        HtmlNode _span = _td[1].ChildNodes[1];
                        if (_span != null)
                        {
                            string orgSysNo = _span.GetAttributeValue("id", "").Trim();
                            if ("ctl00_ContentPlaceHolder1_InquiryDetails1_InquiryDetailsGrid_"+item.OriginatingSystemRef.ToString()+"_lblserialNo" == orgSysNo) { _item = item; }
                        }
                        else {
                            throw new Exception("span not found in childnodes for "+item.Number);
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

                        dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24InquiryDetailsGrid%24ctl0" + i + "%24txtQtyOrder", convert.ToString(_item.Quantity));
                        dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24InquiryDetailsGrid%24ctl0" + i + "%24ddlunit", _item.MeasureUnitQualifier);
                        dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24InquiryDetailsGrid%24ctl0" + i + "%24txtUnitPrice", _price);
                        if(_discount!="0.00")
                        dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24InquiryDetailsGrid%24ctl0" + i + "%24txtDiscount", _discount);
                        else
                            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24InquiryDetailsGrid%24ctl0" + i + "%24txtDiscount", "0");
                        dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24InquiryDetailsGrid%24ctl0" + i + "%24txtTotal", convert.ToString(_item.MonetaryAmount));
                     dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24InquiryDetailsGrid%24ctl0" + i + "%24txtRemarks", Uri.EscapeDataString(_item.LineItemComment.Value).Replace("%20","+"));
                    }
                }
            }
            i = 0;
            LogText = "Line Item Details assignment stopped.";
            #endregion
        }

        public void Assign_MonetoryAmounts()
        {
            #region Monetory Amount
            LogText = "Started assigning Monetory Amount.";

            if (GrandTotal != "")
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24txtGrandtotal", GrandTotal);
            else throw new Exception("Grand Total is blank.");

            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24txtPackingCharges", PackingCost);

            if (Uri.EscapeDataString(SupplierComment).Replace("%20", "+").Length <= 3500)
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24txtSupplierRemark", Uri.EscapeDataString(SupplierComment).Replace("%20","+"));
            else
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24txtSupplierRemark", Uri.EscapeDataString(SupplierComment).Replace("%20", "+").Substring(0, 3499));
            if (FreightCharge != "0")
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24txtInlandFreightCharges", "+" + FreightCharge);
            else
                dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24txtInlandFreightCharges", "");
            dctPostDataValues.Add("ctl00%24ContentPlaceHolder1%24InquiryDetails1%24txtOthers", "");

            LogText = "Monetory Amount assignment stopped.";
            #endregion
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
                    else throw new Exception("No items found in MTML file.");

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
                                      dtExpDate=  ExpDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);//15-3-18
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
                throw new Exception("Exception on LoadInterchangeDetails : " + ex.Message.ToString());
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

        public void ClearCommonVariables()
        { 
        
        }
        #endregion

        public override bool PrintScreen(string sFileName)//12-2-2018
        {
            if (!Directory.Exists(Path.GetDirectoryName(sFileName) + "\\Temp")) Directory.CreateDirectory(Path.GetDirectoryName(sFileName) + "\\Temp");
            sFileName = Path.GetDirectoryName(sFileName) + "\\Temp\\" + Path.GetFileName(sFileName);
            if (base.PrintScreen(sFileName))
            {
                MoveFiles(sFileName, PrintScreenPath + "\\" + Path.GetFileName(sFileName));
                return (File.Exists(PrintScreenPath + "\\" + Path.GetFileName(sFileName)));
            }
            else return false;
        }

        public void WriteErrorLog_With_Screenshot(string AuditMsg, string ErrorNo)
        {
            LogText = AuditMsg;
            string eFile = PrintScreenPath + "\\Greatship_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            if (!PrintScreen(eFile)) eFile = "";
            CreateAuditFile(eFile, "Http_GreatShip_Processor", VRNO, "Error", ErrorNo + AuditMsg, BuyerCode, SupplierCode, AuditPath);
        }

        public void WriteErrorLogQuote_With_Screenshot(string AuditMsg, string _File, string ErrorNo)
        {
            LogText = AuditMsg;
            string eFile = PrintScreenPath + "\\Greatship_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            if (!PrintScreen(eFile)) eFile = "";
            CreateAuditFile(eFile, "Http_GreatShip_Processor", VRNO, "Error", ErrorNo + AuditMsg, BuyerCode, SupplierCode, AuditPath);

            if (!Directory.Exists(MTML_QuotePath + "\\Error")) Directory.CreateDirectory(MTML_QuotePath + "\\Error");
            File.Move(MTML_QuotePath + "\\" + _File, MTML_QuotePath + "\\Error\\" + _File);
        }

        //public void WriteErrorLog_With_Screenshot(string AuditMsg)
        //{
        //    LogText = AuditMsg;
        //    string eFile = PrintScreenPath + "\\Greatship_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss")  + ".png";
        //    if (!PrintScreen(eFile)) eFile = "";
        //    CreateAuditFile(eFile, "Http_GreatShip_Processor", VRNO, "Error", AuditMsg,BuyerCode,SupplierCode,AuditPath);
        //}

        //public void WriteErrorLogQuote_With_Screenshot(string AuditMsg,string _File)
        //{
        //    LogText = AuditMsg;
        //    string eFile = PrintScreenPath + "\\Greatship_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        //    if (!PrintScreen(eFile)) eFile = "";
        //    CreateAuditFile(eFile, "Http_GreatShip_Processor", VRNO, "Error", AuditMsg,BuyerCode,SupplierCode,AuditPath);

        //    if (!Directory.Exists(MTML_QuotePath + "\\Error")) Directory.CreateDirectory(MTML_QuotePath + "\\Error");
        //    File.Move(MTML_QuotePath + "\\" + _File, MTML_QuotePath + "\\Error\\" + _File);
        //}
    }
}
