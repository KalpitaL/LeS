using HtmlAgilityPack;
using HTTPRoutines;
using MTML.GENERATOR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;


namespace UnionMarine_Http_Routine
{
    public class Routines : LeSCommon.LeSCommon
    {
        public int iRetry = 0;
        public bool IsLoggedin;
        public string currentDate = "", SITEURL = "", cQuote_filepath = "",cQuote_Attachpath="", ProcessorName = "", sDoneFile = "", sAuditMesage = "", cUploadAttachment="",
            cTemp_PDFPath="",cErrorPath="", cCookieVal = "", ModuleName="",cQte_href="",cBackup_Path="";
        public double dGrandTotal, dFile_GrandTotal;
        public string[] BuyerNames, Buyer_Supplier_LinkID, Actions, OnlySubmitSupp, SuppCodes, Login_Credentials;
        public List<string> slAnchors =new List<string>();
        HTTP _httproutine = new HTTP();
        HtmlDocument _htmlInvDoc;
        public MTMLInterchange _interchange { get; set; }
        public Dictionary<string, string> slQteheader = new Dictionary<string, string>();
        public Dictionary<string, string> slQteitems = new Dictionary<string, string>();

        public Routines()
        {
            GetAppConfigSettings();
        }

        public void GetAppConfigSettings()
        {
            try
            {
                Initialise();
                ModuleName = "UnionMarine_HTTP_Quote_Processor";
              //  Userid = Convert.ToString(ConfigurationManager.AppSettings["USERNAME"]);
              //  Password = Convert.ToString(ConfigurationManager.AppSettings["PASSWORD"]);

                Login_Credentials = Convert.ToString(ConfigurationManager.AppSettings["LOGIN_CREDENTIALS"]).Split('|');
                LoginRetry = convert.ToInt(ConfigurationManager.AppSettings["LOGINRETRY"]);
                SITEURL = URL = Convert.ToString(ConfigurationManager.AppSettings["SITE_URL"]);
                ProcessorName = Convert.ToString(ConfigurationManager.AppSettings["PROCESSOR_NAME"]);
                Actions = Convert.ToString(ConfigurationManager.AppSettings["ACTIONS"]).Split(',');

                OnlySubmitSupp = Convert.ToString(ConfigurationManager.AppSettings["SUPP_ONLY_SUBMIT"]).Split(',');
                SuppCodes = Convert.ToString(ConfigurationManager.AppSettings["SUPPLIER_CODE"]).Split(',');
                BuyerCode = Convert.ToString(ConfigurationManager.AppSettings["BUYER_CODE"]);

                cUploadAttachment = Convert.ToString(ConfigurationManager.AppSettings["UPLOAD_ATTACHMENT"]);

                if (currentDate == null || currentDate == "")
                    currentDate = DateTime.Today.AddDays(-1).ToString("dd-MMM-yyyy");
                else
                    currentDate = DateTime.ParseExact(currentDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).AddDays(-1).ToString("dd-MMM-yyyy");
                AuditPath = (Convert.ToString(ConfigurationManager.AppSettings["AUDITPATH"]) != "") ? Convert.ToString(ConfigurationManager.AppSettings["AUDITPATH"]) : AppDomain.CurrentDomain.BaseDirectory + "Audit";
                LogPath = (Convert.ToString(ConfigurationManager.AppSettings["LOGPATH"]) != "") ? Convert.ToString(ConfigurationManager.AppSettings["LOGPATH"]) : AppDomain.CurrentDomain.BaseDirectory + "Log";
                PrintScreenPath = (Convert.ToString(ConfigurationManager.AppSettings["PRINT_PATH"]) != "") ? Convert.ToString(ConfigurationManager.AppSettings["PRINT_PATH"]) : AppDomain.CurrentDomain.BaseDirectory + "PrintScreen";
                if (Convert.ToString(ConfigurationManager.AppSettings["RFQ_PATH"]) != "") DownloadPath = Convert.ToString(ConfigurationManager.AppSettings["RFQ_PATH"]);
                if (Convert.ToString(ConfigurationManager.AppSettings["QUOTE_FILE_PATH"]) != "") cQuote_filepath = Convert.ToString(ConfigurationManager.AppSettings["QUOTE_FILE_PATH"]);
                if (Convert.ToString(ConfigurationManager.AppSettings["QUOTE_ATTACH_PATH"]) != "") cQuote_Attachpath = Convert.ToString(ConfigurationManager.AppSettings["QUOTE_ATTACH_PATH"]);
                cBackup_Path = cQuote_filepath + "Backup";
                if (!Directory.Exists(PrintScreenPath)) Directory.CreateDirectory(PrintScreenPath);
                if (!Directory.Exists(DownloadPath)) Directory.CreateDirectory(DownloadPath);
                if (!Directory.Exists(AuditPath)) Directory.CreateDirectory(AuditPath);
                if (!Directory.Exists(LogPath)) Directory.CreateDirectory(LogPath);
                if (!Directory.Exists(cBackup_Path)) Directory.CreateDirectory(cBackup_Path);
                if (cQuote_filepath.Trim().Length == 0) throw new Exception("No Quotation files present.");
                cErrorPath = Environment.CurrentDirectory + "\\Error"; if (!Directory.Exists(cErrorPath)) Directory.CreateDirectory(cErrorPath);
                HiddenAttributeKey = "name";
            }
            catch (Exception e)
            {
                sAuditMesage = "Exception during initialise: " + e.GetBaseException().ToString();
                LogText = sAuditMesage;
                SetAuditLogFile("", "", ModuleName, "", "", "Error", "Exception while processing Quote file : " + e.Message);
            }
        }

        public void Download_FileProcess()
        {
            LoadInterchange();           
        }

        public void LoadInterchange()
        {
            if (!string.IsNullOrEmpty(cQuote_filepath))
            {
                DirectoryInfo quoteDir = new DirectoryInfo(cQuote_filepath);
                if (quoteDir != null)
                {
                    FileInfo[] quoteFiles = quoteDir.GetFiles("*.xml");
                    if (quoteFiles.Length > 0)
                    {
                        foreach (FileInfo fQuote in quoteFiles)
                        {
                            MTMLClass _mtmlClass = new MTMLClass();
                            _interchange = _mtmlClass.Load(fQuote.FullName);
                            string SupplierCode = _interchange.Sender; string BuyerLinkCode = _interchange.Recipient;

                            if (SuppCodes != null && SuppCodes.Length > 0)
                            {
                                for (int k = 0; k < SuppCodes.Length; k++)
                                {
                                    LoadURL("", "", "", true);
                                    if (SupplierCode == SuppCodes[k])
                                    {
                                        IsLoggedin = Login(SuppCodes[k]);
                                        if (IsLoggedin)
                                        {
                                            LogText = sAuditMesage = "Union Marine has login Successfully."; _httpWrapper.Referrer = SITEURL + "invoice";
                                            _htmlInvDoc = _httpWrapper._CurrentDocument; Print_ScreenShot(_htmlInvDoc, "invoice");
                                            GetSearchFilterList(_htmlInvDoc);
                                            GetFilterRecords(_htmlInvDoc, _interchange, fQuote.FullName, SuppCodes[k]);
                                        }
                                        else
                                        {
                                            if (iRetry == LoginRetry)
                                            {
                                                _htmlInvDoc = _httpWrapper._CurrentDocument; Print_ScreenShot(_htmlInvDoc, "Login failure");
                                            }
                                            else
                                            {
                                                iRetry++;
                                                LogText = sAuditMesage = "Login retry";
                                                IsLoggedin = Login(SuppCodes[k]);
                                            }

                                            LogText = sAuditMesage = "Unable to login Union Marine Url,Login failed.";
                                            SetAuditLogFile(BuyerCode, SupplierCode, ModuleName, "", "", "Error", sAuditMesage);
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else { LogText = "No Quote files present"; }
                }
            }          
        }

        public bool Login(string SuppCode)
        {
            bool result = false;
            try
            {
                if (Login_Credentials != null && Login_Credentials.Length > 0)
                {
                    for (int i = 0; i < Login_Credentials.Length; i++)
                    {
                        string[] cDet = Login_Credentials[i].Split('=');
                        if (cDet[0].ToUpper() == SuppCode)
                        {
                            string[] cLgndet = cDet[1].Split(',');
                            Userid = cLgndet[0].Replace("USERNAME:", ""); Password = cLgndet[1].Replace("PASSWORD:", "");

                            LogText = "Union Marine login process started for User " + Userid;
                            Print_ScreenShot(_httpWrapper._CurrentDocument, "Login");
                            this.URL += "users/login";
                            _httpWrapper._SetRequestHeaders[HttpRequestHeader.Cookie] = "PHPSESSID=" + _httpWrapper._dctSetCookie["PHPSESSID"];
                            Post_Data = @"data%5BUser%5D%5BUsername%5D=" + Userid + "&data%5BUser%5D%5BPassword%5D=" + Password + "&Login=Login";
                            result = PostURL("", "", "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public bool GetSearchFilterList(HtmlDocument _htmlInvDoc)
        {
            bool result = false;
            try
            {
                LogText = "Search records from the List";
                HtmlNode _sType = _htmlInvDoc.DocumentNode.SelectSingleNode("//select[@id='SearchSearchFor']/option[@selected]");
                HtmlNode _sInvstatus = _htmlInvDoc.DocumentNode.SelectSingleNode("//select[@id='SearchInvoiceStatus']/option[@selected]");
                HtmlNode _sPOstatus = _htmlInvDoc.DocumentNode.SelectSingleNode("//select[@id='SearchPoStatus']/option[@selected]");
                HtmlNode _sPOstatus1 = _htmlInvDoc.DocumentNode.SelectSingleNode("//select[@id='SearchPoStatus1']/option[@selected]");

                string _searchType = (_sType != null) ? _sType.OuterHtml : string.Empty;
              //  string _searchNo = _htmlInvDoc.GetElementbyId("SearchNumber").InnerText;
                string _searchInvstatus = (_sInvstatus != null) ? _sInvstatus.OuterHtml : string.Empty;
                string _searchPOstatus = (_sPOstatus != null) ? _sPOstatus.OuterHtml : string.Empty;
                string _searchPOstatus1 = (_sPOstatus1 != null) ? _sPOstatus1.OuterHtml : string.Empty;
                string _searchDateStart = HttpUtility.UrlEncode(_htmlInvDoc.GetElementbyId("SearchDateStart").InnerText);
                string _searchDateEnd = HttpUtility.UrlEncode(_htmlInvDoc.GetElementbyId("SearchDateEnd").InnerText);
                string _searchKeyword = _htmlInvDoc.GetElementbyId("SearchKeyword").InnerText;
                this.URL = SITEURL + "invoice";
                _httpWrapper._SetRequestHeaders[HttpRequestHeader.Cookie] = "PHPSESSID=" + _httpWrapper._dctSetCookie["PHPSESSID"];
                Post_Data = @"data%5BSearch%5D%5BSearchFor%5D=" + _searchType +// "&data%5BSearch%5D%5BNumber%5D=" + _searchNo +
                    "&data%5BSearch%5D%5BInvoiceStatus%5D=" + _searchInvstatus + "&data%5BSearch%5D%5BPoStatus%5D=" + _searchPOstatus + "&data%5BSearch%5D%5BPoStatus1%5D=" + _searchPOstatus1 +
                    "&data%5BSearch%5D%5BDateStart%5D=" + _searchDateStart + "&data%5BSearch%5D%5BDateEnd%5D=" + _searchDateEnd + "&data%5BSearch%5D%5BKeyword%5D=" + _searchKeyword;

                result = PostURL("", "", "");
                Print_ScreenShot(_htmlInvDoc, "Filter");
            }
            catch (Exception ex)
            {
                LogText = sAuditMesage = "Error while Filtering : " + ex.GetBaseException().ToString();
            }
            return result;
        }

        public void GetFilterRecords(HtmlDocument _htmlDoc,MTMLInterchange _interchange,string cXML_FileName,string cSuppCode)
        {
            List<string> slHeader = new List<string>(); slHeader.Clear(); string cFileName = "";
            Dictionary<int, string> slRecords = new Dictionary<int, string>(); slRecords.Clear();
            string cVrNo = "", cHREF = ""; SupplierCode = cSuppCode;
            try
            {
                LogText = "Filter the List";
                HtmlNodeCollection _collectTabData = _htmlDoc.DocumentNode.SelectNodes("//div[@id='tab_requisition']");
                if (_collectTabData != null && _collectTabData.Count > 0)
                {
                    for (int k = 0; k < _collectTabData.Count; k++)
                    {
                        HtmlNodeCollection _collectRFQData = _collectTabData[k].SelectNodes("//table[contains(@class, 'table table-bordered table-hover')]/tbody/tr");
                        if (_collectRFQData != null && _collectRFQData.Count > 0)
                        {
                            for (int i = 0; i < _collectRFQData.Count; i++)
                            {
                                HtmlNodeCollection _arr = _collectRFQData[i].ChildNodes;
                                for (int j = 0; j < _arr.Count; j++)
                                {
                                    if (_arr[j].NextSibling != null)
                                    {
                                        HtmlNode aTags = _arr[j].NextSibling.SelectSingleNode("a");

                                        if (aTags != null)
                                        {
                                            if (!string.IsNullOrEmpty(aTags.InnerHtml) && aTags.InnerHtml.StartsWith("V")) { cVrNo = aTags.InnerHtml; }//to check
                                            cHREF = cQte_href = aTags.Attributes["href"].Value;
                                            HtmlAttribute cTargt = aTags.Attributes["target"]; HtmlAttribute attrtitle = aTags.Attributes["title"];
                                            if (cTargt != null)
                                            {
                                                if (_interchange.DocumentHeader.MessageNumber == cVrNo)
                                                {
                                                    slAnchors.Add(i + ". " + cVrNo + "|" + cHREF);                                                   
                                                    cFileName = _interchange.DocumentHeader.OriginalFile;
                                                    if (DownloadFiles(cVrNo, cHREF, cQuote_Attachpath + cFileName))
                                                    {
                                                        if (File.Exists(cQuote_Attachpath + cFileName))
                                                        {
                                                            WriteRFQDownloadedFile(cVrNo, GetNodedetails(_arr), cFileName);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        LogText = sAuditMesage = "File is already downloaded for VRNO " + cVrNo + ".";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (_interchange.DocumentHeader.MessageNumber == cVrNo)
                                                {
                                                    if (attrtitle != null && attrtitle.Value.ToUpper() == "SUBMIT QUOTE")
                                                    {
                                                        slQteheader.Clear();
                                                        ProcessQuotations(cVrNo, cHREF, _interchange, cXML_FileName, cSuppCode);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MoveFiles(cXML_FileName, cErrorPath + @"\" + Path.GetFileName(cXML_FileName));
                SetAuditLogFile(BuyerCode, SupplierCode, ModuleName, Path.GetFileName(cXML_FileName), cVrNo, "Error", "Error while Processing file : " + ex.GetBaseException().ToString());
            }
        }

        #region Download

        public bool DownloadFiles(string cVrNo, string cHrefUrl, string cFileName)
        {
            bool _result = false; HtmlDocument _htmlDwnldDoc = null;
            try
            {
                LogText = "Download RFQ files for VRNO " + cVrNo + ".";  string cReqURL = cHrefUrl.TrimStart('/');
                _result = DownloadRFQFile(cReqURL, cFileName, "");
            }
            catch (Exception e)
            {
                LogText = sAuditMesage = "Exception in DownloadFile : " + e.GetBaseException().ToString();
                Print_ScreenShot(_htmlDwnldDoc, "DownloadError");//auditfile
            }
            return _result;
        }

        public  bool DownloadRFQFile(string RequestURL, string DownloadFileName,string ContentType = "Application/pdf")
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

        public void WriteRFQDownloadedFile(string cVrNo, string RFQDetails, string FileName)
        {
            try
            {
                string strRFQDwnld = RFQDetails.Replace("&nbsp;", "").TrimEnd('|');
                File.AppendAllText(DownloadPath + "\\RFQ_Downloaded.txt", strRFQDwnld + Environment.NewLine);
                string Audit = DateTime.Now.ToString("yy-MM-dd HH:mm") + "RFQ '" + FileName + "' for ref '" + cVrNo + "' downloaded successfully.";
                //audit file
                LogText = "RFQ '" + FileName + "' for VRNO '" + cVrNo + "' downloaded successfully.";
            }
            catch (Exception e)
            {
                LogText = sAuditMesage = "Exception while writing RFQ Downloaded file : " + e.GetBaseException().ToString();
                //audit file
            }
        }

        #endregion

        #region Process & Submit Quotation
        public bool ProcessQuotations(string cVrNo, string cHrefUrl, MTMLInterchange _interchange, string cXML_FileName, string cSuppCode)
        {
            bool _result = false; HtmlDocument _htmlQuotDoc = null; string cAttach_File = "";
            string cFile = Path.GetFileName(cXML_FileName);
            try
            {
                if (!string.IsNullOrEmpty(cUploadAttachment) && cUploadAttachment.ToUpper() == "TRUE")
                {
                    cAttach_File = _interchange.DocumentHeader.OriginalFile;
                    AttachDetails(cAttach_File);
                }

                LogText = "Processing Quote file for VRNO " + cVrNo + "."; string cReqURL = cHrefUrl.TrimStart('/');
                this.URL = cReqURL; Post_Data = "";
                _httpWrapper._SetRequestHeaders[HttpRequestHeader.Cookie] = "PHPSESSID=" + _httpWrapper._dctSetCookie["PHPSESSID"];
                _result = PostURL("", "", "");
                _htmlQuotDoc = _httpWrapper._CurrentDocument; Print_ScreenShot(_htmlQuotDoc, "QuoteProcess");

                if (!string.IsNullOrEmpty(cAttach_File))
                {
                    _htmlQuotDoc.GetElementbyId("FileFile").SetAttributeValue("value", cAttach_File);
                    _htmlQuotDoc.GetElementbyId("FileHidden").SetAttributeValue("value", cAttach_File);
                }

                HtmlNode _headNode = _htmlQuotDoc.DocumentNode.SelectSingleNode("//h1[@class='heading']");
                if (_headNode != null)
                {
                    string PageHeader = convert.ToString(_headNode.InnerText);
                    if (PageHeader.ToUpper().Contains("QUOTE"))
                    {
                        ProcessHeader_ItemDetails(_htmlQuotDoc, _interchange, cVrNo);
                        HtmlNode _saveNode = _htmlQuotDoc.DocumentNode.SelectSingleNode("//span[@class='btn btn-primary jq_save']");
                        if (_saveNode != null)
                        {
                            bool IsQte_Save = Save_SubmitQuotation(slQteheader, "save", slQteitems, cXML_FileName, _htmlQuotDoc);
                            if (IsQte_Save)
                            {
                                string cResult = _htmlQuotDoc.DocumentNode.InnerText;
                                if (cResult.Contains("updated") || cResult.Contains("submitted"))//Quote has been updated
                                {
                                    Print_ScreenShot(_htmlQuotDoc, "Quotation Save");
                                    SetAuditLogFile(BuyerCode, cSuppCode, ModuleName, cFile, cVrNo, "Success", "Quotation Updated.");
                                    if (dGrandTotal == dFile_GrandTotal)
                                    {
                                        if (OnlySubmitSupp != null && OnlySubmitSupp.Length > 0)//submit vendors
                                        {
                                            for (int i = 0; i < OnlySubmitSupp.Length; i++)
                                            {
                                                if (OnlySubmitSupp[i].ToUpper() == cSuppCode.ToUpper())
                                                {
                                                    this.URL = cReqURL; Post_Data = "";
                                                    _httpWrapper._SetRequestHeaders[HttpRequestHeader.Cookie] = "PHPSESSID=" + _httpWrapper._dctSetCookie["PHPSESSID"];
                                                    _result = LoadURL("", "", ""); _htmlQuotDoc = _httpWrapper._CurrentDocument;

                                                    HtmlNode _submitNode = _htmlQuotDoc.DocumentNode.SelectSingleNode("//input[@class='btn btn-primary jq_submit']");
                                                    if (_submitNode != null)
                                                    {
                                                        bool IsQte_Submit = Save_SubmitQuotation(slQteheader, "submit", slQteitems, cXML_FileName, _htmlQuotDoc);
                                                        if (IsQte_Submit)
                                                        {
                                                            if (cResult.Contains("updated") || cResult.Contains("submitted"))//Quote has been updated
                                                            {
                                                                Print_ScreenShot(_htmlQuotDoc, "Quotation Submit");
                                                                SetAuditLogFile(BuyerCode, OnlySubmitSupp[i], ModuleName, cFile, cVrNo, "Success", "Quotation Submitted Successfully.");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        MoveFiles(cXML_FileName, cBackup_Path + @"\" + Path.GetFileName(cXML_FileName));
                                    }
                                    else
                                    {
                                        LogText = sAuditMesage = "Total amount mismatch for file -" + cFile;
                                        SetAuditLogFile(BuyerCode, cSuppCode, ModuleName, cFile, cVrNo, "Error", sAuditMesage);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogText = sAuditMesage = "Error while Saving and Submitting file : " + e.GetBaseException().ToString();
                Print_ScreenShot(_htmlQuotDoc, "QuoteError");
                SetAuditLogFile(BuyerCode, cSuppCode, ModuleName, cFile, cVrNo, "Error", sAuditMesage);
                MoveFiles(cXML_FileName, cErrorPath + @"\" + Path.GetFileName(cXML_FileName));
            }
            return _result;
        }

        private void ProcessHeader_ItemDetails(HtmlDocument _htmlQuotDoc, MTMLInterchange _interchange, string cVrNo)
        {
            string VRNO = "", QuoteRefNo = "", QuoteExp = "", DeliveryDate = "", cComments="",cDelCharges="",cGrandTotal="",cTaxCost="",
                dtToday = DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year;
            double fGrndTotal = 0.0d,fOtherCharges=0.0d;
            DateTime dtQteExpry = new DateTime(); 
            try
            {
                LogText = "Processing Quote Header and Items  for VRNO " + cVrNo + "."; 
                HtmlNode _SuppCurr = _htmlQuotDoc.GetElementbyId("RequestSupplierCurrencyCode");
                HtmlNode _SuppDis = _htmlQuotDoc.GetElementbyId("RequestSupplierDiscount");
                HtmlNode _SuppDelCharge = _htmlQuotDoc.GetElementbyId("RequestSupplierDeliveryCharges");
                HtmlNode _SuppRefNumber = _htmlQuotDoc.GetElementbyId("RequestSupplierSupplierReferenceNumber");
                HtmlNode _SuppValidity = _htmlQuotDoc.GetElementbyId("RequestSupplierValidityPeriod");
                HtmlNode _SuppDelLeadTime = _htmlQuotDoc.GetElementbyId("RequestSupplierDeliveryLeadTime");
                HtmlNode _SuppDelComment = _htmlQuotDoc.GetElementbyId("RequestSupplierComment");
                HtmlNode _FilePath = _htmlQuotDoc.GetElementbyId("FileHidden");
                
                string cCurrency = convert.ToString(_interchange.DocumentHeader.CurrencyCode).Trim();                 
               _SuppCurr.InnerHtml = cCurrency; slQteheader.Add("CURRENCY", cCurrency);
                int LeadDays = convert.ToInt(_interchange.DocumentHeader.LeadTimeDays); 
                double AddDiscount = convert.ToFloat(_interchange.DocumentHeader.AdditionalDiscount); _SuppDis.SetAttributeValue("value", AddDiscount.ToString("0.00"));
                slQteheader.Add("ADDITIONALDISCOUNT", AddDiscount.ToString("0.00")); fGrndTotal = AddDiscount;
                if (_interchange != null)
                {
                    foreach (Reference refs in _interchange.DocumentHeader.References)
                    {
                        if (refs.Qualifier == ReferenceQualifier.UC) { VRNO = refs.ReferenceNumber; slQteheader.Add("REQN_NO", VRNO); }
                        else if (refs.Qualifier == ReferenceQualifier.AAG) { QuoteRefNo = refs.ReferenceNumber; _SuppRefNumber.SetAttributeValue("value", QuoteRefNo); slQteheader.Add("QUOTE_REFNO", QuoteRefNo); }
                    }
                    foreach (DateTimePeriod dt in _interchange.DocumentHeader.DateTimePeriods)
                    {
                        if (dt.Qualifier == DateTimePeroidQualifiers.ExpiryDate_36)
                        { QuoteExp = SetDate(dt.Value); dtQteExpry = (!string.IsNullOrEmpty(QuoteExp)) ? Convert.ToDateTime(QuoteExp) : DateTime.MinValue; slQteheader.Add("QUOTE_EXPIRY", dtQteExpry.ToString()); }
                        else if (dt.Qualifier == DateTimePeroidQualifiers.DeliveryDate_69) { DeliveryDate = SetDate(dt.Value); }
                    }
                    foreach (Comments cmt in _interchange.DocumentHeader.Comments)
                    {
                        if (cmt.Qualifier == CommentTypes.SUR) { cComments = cmt.Value; _SuppDelComment.InnerHtml = cComments;
                            slQteheader.Add("SUPP_COMMENTS", cComments); }
                    }
                    foreach (MonetaryAmount _amt in _interchange.DocumentHeader.MonetoryAmounts)
                    {
                        switch (_amt.Qualifier)
                        {
                            case MonetoryAmountQualifier.PackingCost_106: fGrndTotal += _amt.Value; fOtherCharges += _amt.Value;
                                break;
                            case MonetoryAmountQualifier.TaxCost_99: fGrndTotal += _amt.Value; fOtherCharges += _amt.Value;
                                break;
                            case MonetoryAmountQualifier.FreightCharge_64: fGrndTotal += _amt.Value; fOtherCharges += _amt.Value;
                                break;
                            case MonetoryAmountQualifier.OtherCost_98: fGrndTotal += _amt.Value; fOtherCharges += _amt.Value;
                                break;
                            case MonetoryAmountQualifier.AllowanceAmount_204: fGrndTotal += _amt.Value; fOtherCharges += _amt.Value;
                                break;
                            case MonetoryAmountQualifier.GrandTotal_259: cGrandTotal = _amt.Value.ToString("0.00"); slQteheader.Add("GRAND_TOTAL", cGrandTotal);
                                dFile_GrandTotal = Convert.ToDouble(cGrandTotal);
                                break;
                        }
                    }
                    cDelCharges = fOtherCharges.ToString("0.00"); _SuppDelCharge.SetAttributeValue("value", cDelCharges); slQteheader.Add("DELIVERY_CHARGES", cDelCharges); 
                    int nValidityPeriod = (dtQteExpry != DateTime.MinValue) ? dtQteExpry.Subtract(DateTime.Now).Days : 0; _SuppValidity.SetAttributeValue("value", nValidityPeriod.ToString()); slQteheader.Add("VALIDITY_PERIOD", convert.ToString(nValidityPeriod));
                    DateTime dtDelvryleadTime = DateTime.Now.AddDays(LeadDays); _SuppDelLeadTime.SetAttributeValue("value", dtDelvryleadTime.ToString("dd/MM/yyyy HH:mm"));//ToString("dd/MM/yyyy HH:mm")
                    slQteheader.Add("DELIVERY_LEADTIME", dtDelvryleadTime.ToString("dd/MM/yyyy HH:mm"));
                 
                    Print_ScreenShot(_htmlQuotDoc, "Quote Header");
                    ProcessLineItems(_htmlQuotDoc, _interchange, cGrandTotal, slQteheader, fGrndTotal);
                    Print_ScreenShot(_htmlQuotDoc, "Quote Line Items");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ProcessLineItems(HtmlDocument _htmlQuoteDoc, MTMLInterchange _interchange, string cQuoteTotal, Dictionary<string, string> slQteheader, double fGrndTotal)
        {
            string cTotalAmount = ""; slQteitems.Clear();
            List<string> slHder = new List<string>(); slHder.Clear();
            List<List<string>> parsedTblHdr = _htmlQuoteDoc.DocumentNode.SelectSingleNode("//table[@class='table table-bordered']").Descendants("tr").Skip(1)
                .Where(tr => tr.Elements("th").Count() > 1).Select(tr => tr.Elements("th").Select(td => td.InnerText.Trim()).ToList()).ToList();

            List<List<string>> parsedTblBody = _htmlQuoteDoc.DocumentNode.SelectSingleNode("//table[@class='table table-bordered']").Descendants("tr").Skip(1)
                .Where(tr => tr.Elements("td").Count() > 1).Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim() + "$" + td.InnerHtml.Trim()).ToList()).ToList();

            if (parsedTblHdr.Count == 1) { slHder = parsedTblHdr[parsedTblHdr.Count - 1]; }

            Dictionary<int, LineItem> _items = new Dictionary<int, LineItem>();
            List<LineItem> _extraItems = new List<LineItem>();
            if (_interchange != null)
            {
                LineItemCollection _itmCollect = _interchange.DocumentHeader.LineItems;

                foreach (LineItem _itm in _itmCollect)
                {
                    for (int i = 0; i < parsedTblBody.Count; i++)
                    {
                        List<string> sldet = parsedTblBody[i];
                        if (sldet.Count > 2 && !sldet[0].Contains("Total"))
                        {
                            HtmlNode _Rmknode = null; HtmlNode _Qtynode = null; HtmlNode _UnitPricenode = null; HtmlNode _Discountnode = null; int id = 0;
                            string cPartName = "", cPartCode = "", cReqQty = "", cReqUOM = "", cReq_AddnInfo = "", cQte_Remark = "", cQte_UOM = "", cQte_Qty = "", cQte_UnitPrice = "", cQte_Discount = "", cItemNo = "";
                            for (int j = 0; j < sldet.Count; j++)
                            {
                                switch (slHder[j])
                                {
                                    case "Part Name": cPartName = sldet[j].Split('$')[0];
                                        break;
                                    case "IMPA Code": 
                                    case "Code": cPartCode = sldet[j].Split('$')[0];
                                        break;
                                    case "QTY": cReqQty = sldet[j].Split('$')[0]; cReqUOM = cReqQty.Split(' ')[1];
                                        break;
                                    case "Additional Information": cReq_AddnInfo = sldet[j].Split('$')[0];
                                        break;
                                    case "Remark": cQte_Remark = sldet[j].Split('$')[0];
                                        _Rmknode = GetRemarksNode(_htmlQuoteDoc, sldet[j].Split('$')[1]);                                          
                                        break;
                                    case "QTY*": cQte_UOM = sldet[j].Split('|')[0];
                                        _Qtynode = GetQtyNode(_htmlQuoteDoc, sldet[j].Split('$')[1]);                                   
                                        break;
                                    case "Unit Price*": cQte_UnitPrice = sldet[j].Split('|')[0];
                                        _UnitPricenode = GetUnitPriceNode(_htmlQuoteDoc, sldet[j].Split('$')[1]);
                                        break;
                                    case "Discount": cQte_Discount = sldet[j].Split('$')[0];
                                        _Discountnode = GetDiscountNode(_htmlQuoteDoc, sldet[j].Split('$')[1]);
                                        break;
                                    case "": cItemNo = sldet[j].Split('$')[0];
                                        break;
                                    default: break;
                                }
                            }
                            string cFItemno = _itm.Number; string cFPartNo = _itm.Identification; string cFPart_Descr = _itm.Description;string cFPartidentification = _itm.Identification;
                            double fQty = _itm.Quantity; double fUnitPrice = 0.000;
                            string cFUOM = _itm.MeasureUnitQualifier; double fDiscPrice = 0.000;
                            foreach (PriceDetails priceDetails in _itm.PriceList)
                            {
                                if (priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.GRP) { fUnitPrice = priceDetails.Value; }
                                if (priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.DPR) { fDiscPrice = priceDetails.Value; }// (percentage)
                            }
                            string cFRemarks = (_itm.LineItemComment != null) ? Convert.ToString(_itm.LineItemComment.Value) : string.Empty;
                            if (cFItemno == cItemNo)
                            {
                                //if (cPartName.Contains(cFPart_Descr))
                                bool Is_PartCode_same = CheckPartIdentification(cFPartidentification, cPartCode);
                                bool IS_PartDesc_same = CheckPartDescription(cFPart_Descr, cPartName);
                                bool Is_PartQty_same = CheckPartQty(fQty.ToString("0.00"), cReqQty.Split(' ')[0]);                                
                                if (IS_PartDesc_same || Is_PartCode_same || Is_PartQty_same)
                                {
                                    id = Convert.ToInt32(_Rmknode.GetAttributeValue("id", "").Replace("QuoteRemark", ""));
                                    if (cQte_UOM.ToUpper() != cFUOM.ToUpper()) { cFRemarks += "UOM :" + cFUOM; }
                                    cQte_Qty = fQty.ToString("0.0000"); cQte_UnitPrice = fUnitPrice.ToString("0.0000"); cQte_Discount = fDiscPrice.ToString("0.0000"); cQte_Remark = cFRemarks;
                                    _Qtynode.SetAttributeValue("value", cQte_Qty); _UnitPricenode.SetAttributeValue("value", cQte_UnitPrice); _Discountnode.SetAttributeValue("value", cQte_Discount);
                                    _Rmknode.SetAttributeValue("value", cFRemarks);
                                    string cResitem = cQte_Remark + "$" + cQte_Qty + "$" + cQte_UnitPrice + "$" + cQte_Discount + "$" + cItemNo;
                                    slQteitems.Add(id.ToString(), cResitem);
                                    fGrndTotal += (fUnitPrice * fQty) * (100 - fDiscPrice) / 100;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            dGrandTotal = Math.Round(fGrndTotal ,2, MidpointRounding.AwayFromZero);
        }

        private bool CheckPartDescription(string cFPart_Descr, string cPartName)
        {
            bool _result = false;
            string cFile_PartDescr = cFPart_Descr.Replace(",", "").Replace('.', ' ').Trim();
            string cSite_PartDescr = cPartName.Replace(",", "").Replace('.', ' ').Trim();
            if (cFile_PartDescr.Contains(cSite_PartDescr))
            {
                _result = true;
            }
            return _result;
        }

        private bool CheckPartIdentification(string cFPartIdentification, string cPartCode)
        {
            bool _result = false;
            string cFile_PartId = cFPartIdentification.Replace(",", "").Trim();
            string cSite_PartIDr = cPartCode.Replace("PN :", "").Trim();
            if (cSite_PartIDr.Contains(cFile_PartId))
            {
                _result = true;
            }
            return _result;
        }

        private bool CheckPartQty(string cFPartQty, string cPartQty)
        {
            bool _result = false;
            double fFile_qty = Convert.ToDouble(cFPartQty);
            double fSite_qty = Convert.ToDouble(cPartQty);
            if (fFile_qty == fSite_qty)
            {
                _result = true;
            }
            return _result;
        }


        private bool Save_SubmitQuotation(Dictionary<string, string> slQteheader, string cAction, Dictionary<string, string> slQteItems,
            string cXML_FileName, HtmlDocument _htmlQuotDoc)
        {
            bool _result = false; string cWebTotal = "", cGrandTotal="";
            try
            {
                _httpWrapper._SetRequestHeaders[HttpRequestHeader.Cookie] = "PHPSESSID=" + _httpWrapper._dctSetCookie["PHPSESSID"];
                Post_Data = @"data%5BRequestSupplier%5D%5BAction%5D=" + cAction;

                if (slQteheader != null)
                {
                    foreach (string key in slQteheader.Keys)
                    {
                        switch (key)
                        {
                            case "CURRENCY": Post_Data += "&data%5BRequestSupplier%5D%5BCurrencyCode%5D=" + slQteheader[key];
                                break;
                            case "ADDITIONALDISCOUNT": Post_Data += "&data%5BRequestSupplier%5D%5BDiscount%5D=" + slQteheader[key];
                                break;
                            case "DELIVERY_CHARGES": Post_Data += "&data%5BRequestSupplier%5D%5BDeliveryCharges%5D=" + slQteheader[key];
                                break;
                            case "QUOTE_REFNO": Post_Data += "&data%5BRequestSupplier%5D%5BSupplierReferenceNumber%5D=" + slQteheader[key];
                                break;
                            case "VALIDITY_PERIOD": Post_Data += "&data%5BRequestSupplier%5D%5BValidityPeriod%5D=" + slQteheader[key];
                                break;
                            case "DELIVERY_LEADTIME": Post_Data += "&data%5BRequestSupplier%5D%5BDeliveryLeadTime%5D=" + HttpUtility.UrlEncode(slQteheader[key]);
                                break;
                            case "SUPP_COMMENTS": Post_Data += "&data%5BRequestSupplier%5D%5BComment%5D=" + HttpUtility.UrlEncode(slQteheader[key]);
                                break;
                            case "ATTACH_FILE": Post_Data += "&data%5BRequestSupplier%5D%5BFileHidden%5D=" + slQteheader[key];
                                break;
                            case "WEB_TOTAL": cWebTotal = slQteheader[key];
                                break;
                            case "GRAND_TOTAL": cGrandTotal = slQteheader[key];
                                break;
                        }
                    }
                }
                if (slQteItems != null)
                {
                    foreach (string id in slQteItems.Keys)
                    {
                        string[] _arrvalues = slQteItems[id].Split('$');
                        string _cQRmkDesc = HttpUtility.UrlEncode(_arrvalues[0]); string cQQty = _arrvalues[1]; string _cQUnitprice = _arrvalues[2]; string _cQDisc = _arrvalues[3];
                        Post_Data += "&data%5BQuote%5D%5BRemark%5D%5B" + id + "%5D=" + _cQRmkDesc.Trim() + "&data%5BQuote%5D%5BQTY_Suggested%5D%5B" + id + "%5D=" + cQQty +
                            "&data%5BQuote%5D%5BUnitPrice%5D%5B" + id + "%5D=" + _cQUnitprice + "&data%5BQuote%5D%5BDiscount%5D%5B" + id + "%5D=" + _cQDisc;
                    }
                }
                _result = PostURL("", "", "");
                if (_result) { }
            }
            catch (Exception ex){ throw; }
            return _result;
        }

        private HtmlNode GetRemarksNode(HtmlDocument _htmlQuoteDoc,string value)
        {
            HtmlNode _node1 = null;
            HtmlNodeCollection  txtareaCollect = _htmlQuoteDoc.DocumentNode.SelectNodes("//textarea[@class='form-control']");
            if (txtareaCollect != null)
            {
                foreach (HtmlNode _node in txtareaCollect)
                {
                    string id = _node.GetAttributeValue("id", "");
                    if (value.Contains(id))
                    {
                        if (_node.GetAttributeValue("id", "").Contains("QuoteRemark"))
                        {
                            _node1 = _node;
                            break;
                        }
                    }
                }
            }          
            return _node1;
        }

        private HtmlNode GetUnitPriceNode(HtmlDocument _htmlQuoteDoc, string value)
        {
            HtmlNode _node1 = null;
            HtmlNodeCollection inpCollect = _htmlQuoteDoc.DocumentNode.SelectNodes("//input[contains(@class, 'jq_unit_price')]");
            if (inpCollect != null)
            {
                foreach (HtmlNode _node in inpCollect)
                {
                    if (_node.GetAttributeValue("id", "").Contains("QuoteUnitPrice"))
                    {
                        _node1 = _node;
                        break;
                    }
                }
            }
            return _node1;
        }

        private HtmlNode GetQtyNode(HtmlDocument _htmlQuoteDoc, string value)
        {
            HtmlNode _node1 = null;
            HtmlNodeCollection inpCollect = _htmlQuoteDoc.DocumentNode.SelectNodes("//input[contains(@class, 'jq_qty')]");
            if (inpCollect != null)
            {
                foreach (HtmlNode _node in inpCollect)
                {
                    if (_node.GetAttributeValue("id", "").Contains("QuoteQTYSuggested"))
                    {
                        _node1 = _node;
                        break;
                    }
                }
            }
            return _node1;
        }

        private HtmlNode GetDiscountNode(HtmlDocument _htmlQuoteDoc, string value)
        {
            HtmlNode _node1 = null;
            HtmlNodeCollection inpCollect = _htmlQuoteDoc.DocumentNode.SelectNodes("//input[contains(@class, 'jq_local_discount')]");
            if (inpCollect != null)
            {
                foreach (HtmlNode _node in inpCollect)
                {
                    if (_node.GetAttributeValue("id", "").Contains("QuoteDiscount"))
                    {
                        _node1 = _node;
                        break;
                    }
                }
            }
            return _node1;
        }
   
        #endregion

        #region Attachments

        private string AttachDetails(string cAttachFile)
        {
            string cServResponse = ""; HttpWebResponse response;
            try
            {
                string cPDF_File = cQuote_Attachpath + cAttachFile;
                this.URL = SITEURL + "ajax/upload_file";

                string boundaryString = "WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x", System.Globalization.NumberFormatInfo.InvariantInfo);//Guid.NewGuid();
                HttpWebRequest oReq = (HttpWebRequest)WebRequest.Create(this.URL);
                string cSessionID = _httpWrapper.SessionID; string cTemp_PDF_file = cTemp_PDFPath + "\\" + cAttachFile;
                oReq.KeepAlive = true;
                oReq.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0");
                oReq.Headers.Add("Origin", SITEURL);
                oReq.Headers.Add("Upgrade-Insecure-Requests", @"1");
                oReq.ContentType = "multipart/form-data; boundary=----" + boundaryString;
                oReq.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
                oReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                oReq.Referer = cQte_href.TrimStart('/');
                oReq.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                oReq.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-GB,en-US;q=0.9,en;q=0.8,und;q=0.7");
                oReq.Headers.Set(HttpRequestHeader.Cookie, "PHPSESSID=" + _httpWrapper._dctSetCookie["PHPSESSID"]);
                oReq.Method = "POST";
                oReq.ServicePoint.Expect100Continue = false;
                string cBody = "\r\n------" + boundaryString + "\r\n";
                cBody += string.Format("Content-Disposition: form-data;name=\"{0}\";filename=\"{1}\"" + "\r\nContent-Type: {2}\r\n\r\n", "document",
                  Path.GetFileName(cPDF_File), FileContentType(Path.GetExtension(cPDF_File)));//.Replace("-", "_")
                cBody += @"<!>" + cPDF_File + "<!>";
                cBody += "\r\n------" + boundaryString + "--\r\n";
                WriteMultipartBodyToRequest(oReq, cBody);
                response = (HttpWebResponse)oReq.GetResponse();

                Stream stream = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream);             
                var tst =JsonConvert.DeserializeObject(sr.ReadToEnd());

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    slQteheader.Add("ATTACH_FILE", cAttachFile);//to check
                } 
            }
            catch (Exception ex)
            {
                throw;
            }
            return cServResponse;
        }

        private string FileContentType(string File_extn)
        {
            string cContentType = "";
            switch(File_extn.TrimStart('.'))
            {
                case "pdf": cContentType = "application/pdf";
                    break;
                case "png": cContentType = "image/png";
                    break;
                case "jpg":
                case "jpeg": cContentType = "image/jpeg";
                    break;
            }
            return cContentType;
        }    

        #endregion

        #region Common Functions

        private string GetNodedetails(HtmlNodeCollection _arrNodes)
        {
            string _str = "";
            if (_arrNodes != null && _arrNodes.Count > 0)
            {
                for (int i = 0; i < _arrNodes.Count; i++)
                {
                    if (_arrNodes[i].Name != "#text" || _arrNodes[i].Name != "td")
                    {
                        _str += _arrNodes[i].InnerText + "|";
                    }
                }
            }
            return _str.TrimEnd('|');
        }

        private void Print_ScreenShot(HtmlDocument _htmlDoc, string cScreen)
        {
            if (_htmlDoc != null)
            {
                string cScreenShot = PrintScreenPath + "\\UnionMarine_" + cScreen + "_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".html";
                if (_htmlDoc.DocumentNode.SelectNodes("//link") != null)
                {
                    foreach (HtmlNode link in _htmlDoc.DocumentNode.SelectNodes("//link"))
                    {
                        string _attr = link.GetAttributeValue("href", "");
                        if (!_attr.Contains("http")) { link.SetAttributeValue("href", SITEURL + _attr.TrimStart('/')); }
                    }
                }
                if (_htmlDoc.DocumentNode.SelectNodes("//a") != null)
                {
                    foreach (HtmlNode atag in _htmlDoc.DocumentNode.SelectNodes("//a"))
                    {
                        string _attr = atag.GetAttributeValue("href", "");
                        if (!_attr.Contains("http")) { atag.SetAttributeValue("href", SITEURL + _attr.TrimStart('/')); }
                    }
                }
                if (_htmlDoc.DocumentNode.SelectNodes("//img") != null)
                {
                    foreach (HtmlNode atag in _htmlDoc.DocumentNode.SelectNodes("//img"))
                    {
                        string _attr = atag.GetAttributeValue("src", "");
                        if (!_attr.Contains("http")) { atag.SetAttributeValue("src", SITEURL + _attr.TrimStart('/')); }
                    }
                }
                using (StringWriter writer = new StringWriter())
                {
                    _htmlDoc.Save(writer);
                    File.WriteAllText(cScreenShot, Convert.ToString(writer));
                }            
            }
        }

        private string SetDate(string cDate)
        {
            DateTime dttemp = new DateTime(); string cResult = "";
            DateTime.TryParseExact(cDate.Trim(), "yyyyMMddHHmm", null, System.Globalization.DateTimeStyles.None, out dttemp);
            if (dttemp != DateTime.MinValue)
            {
                if (dttemp < DateTime.Now) dttemp = DateTime.Now;
                //cResult = dttemp.Month + "/" + (dttemp.Day) + "/" + dttemp.Year;
                cResult = dttemp.Year + "-" + (dttemp.Month) + "-" + dttemp.Day;
            }
            return cResult;
        }

        private string SetDate_Format(string cDate)
        {
            DateTime dttemp = new DateTime(); string cResult = "";
            DateTime.TryParseExact(cDate.Trim(), "yyyyMMddHHmm", null, System.Globalization.DateTimeStyles.None, out dttemp);
            if (dttemp != DateTime.MinValue)
            {
                if (dttemp < DateTime.Now) dttemp = DateTime.Now;
                cResult = dttemp.Month + "/" + (dttemp.Day) + "/" + dttemp.Year;
            }
            return cResult;
        }

        public void SetAuditLogFile(string BuyerID, string SuppID, string ModuleName, string FileName, string BuyerRefNo, string Action, string LogMsg)
        {
            try
            {
                if (!File.Exists(FileName)) FileName = "";
                else FileName = Path.GetFileName(FileName);
                DateTime dt = DateTime.Now;
               // if (BuyerID == null || BuyerID == "") BuyerID = Convert.ToString(ConfigurationManager.AppSettings["BUYER_CODE"]);
                //if (SuppID == null || SuppID == "") SuppID = Convert.ToString(ConfigurationManager.AppSettings["SUPPLIER_CODE"]);

                string _logDir = convert.ToString(ConfigurationManager.AppSettings["AUDIT_PATH"].ToString());
                if (!Directory.Exists(_logDir)) { Directory.CreateDirectory(_logDir); }
                if (convert.ToString(ConfigurationManager.AppSettings["MODULE_NAME"]) != "") { ModuleName = convert.ToString(ConfigurationManager.AppSettings["MODULE_NAME"]); }

                string _logFile = _logDir + @"\Audit_" + DateTime.Now.ToString("ddMMyyyyhhmmssff") + ".txt";
                string _logStr = "";
                if (LogMsg != string.Empty) _logStr = BuyerID + "|" + SuppID + "|" + ModuleName + "|" + FileName + "|" + BuyerRefNo + "|" + Action + "|" + DateTime.Now.ToString("yy-MM-dd HH:mm") + " : " + LogMsg;

                DirectoryInfo df = new DirectoryInfo(_logDir);
                if (!df.Exists) { Directory.CreateDirectory(_logDir); }

                if (Directory.Exists(_logDir))
                {
                    StreamWriter sw = new StreamWriter(_logFile, true);
                    sw.WriteLine(_logStr);
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (Exception ex)
            {
                LogText = "Error on SetAuditLogFile : " + ex.Message + " StackTace : " + ex.StackTrace;
            }
        }

        #endregion

        #region Reference

        private void SaveQuote()
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.intuitships-supplynet.ummsportal.com/requisition_quote/submit/4428");

            request.KeepAlive = true;
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.Headers.Add("Origin", @"http://www.intuitships-supplynet.ummsportal.com");
            request.Headers.Add("X-Requested-With", @"XMLHttpRequest");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = "http://www.intuitships-supplynet.ummsportal.com/requisition_quote/submit/4428";
            request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-GB,en-US;q=0.9,en;q=0.8,und;q=0.7");
            request.Headers.Set(HttpRequestHeader.Cookie, @"CAKEPHP=5ea9e7236dc603c33b6debc733ae61bb");

            request.Method = "POST";
            request.ServicePoint.Expect100Continue = false;

            string body = @"data%5BRequestSupplier%5D%5BAction%5D=save&data%5BRequestSupplier%5D%5BCurrencyCode%5D=USD&data%5BRequestSupplier%5D%5BDiscount%5D=0&data%5BRequestSupplier%5D%5BDeliveryCharges%5D=0.00&data%5BRequestSupplier%5D%5BSupplierReferenceNumber%5D=594017&data%5BRequestSupplier%5D%5BValidityPeriod%5D=0&data%5BRequestSupplier%5D%5BDeliveryLeadTime%5D=04%2F08%2F2018+23%3A55&data%5BRequestSupplier%5D%5BComment%5D=Thanks+for+your+inquiry.+We're+pleased+to+offer+our+quotation+for+your+consideration.+Please+note%3A+Vessel+is+now+under+credit+block.+All+outstanding+payments+need+to+be+settle+in+order+to+release+for+delivery.+All+quoted+items+are+ex-stock+subject+to+prior+sales.+Delivery+Lead+time%3A+1-2+working+days+Subject+to+additional+delivery+charge%2C+hire+of+barge%2Fcrane%2C+custom+fee%2C+etc.+Please+allow+another+1-2+working+days%E2%80%99+notice+for+delivery+arrangement.+All+deliveries+in+Singapore+are+subject+to+7%25+Goods+and+Services+Tax+(GST)+under+local+regulation.+This+GST+could+be+waived+by+providing+Vessel+Stamps+on+D%2FO+or+any+other+relevant+export+documentation+within+14+days.+We+look+forward+to+receiving+your+earliest+order+confirmation.+Best+Regards%2C+Edlyn+Tan+General+Conditions%3A+The+provision+of+products+and%2For+services+by+Wilhelmsen+Ships+Services+(WSS)+is+at+all+times+subject+to+the+WSS+Standard+Terms+and+Conditions+for+the+Supply+of+Products+and+Gas+Cylinders.+These+can+be+found+at%3A+http%3A%2F%2Fwilhelmsen.com%2Fterms-and-conditions%2F+Item+Added+-+3+%3A+FIXED+CHARGE+LAST+MILE+DELIVERY+%7C+QTY+%3A+1+%7C+UOM+%3A+PCS+%7C+Price+%3A+0+USD+Item+Total+%3A+814.58+(including+additional+items)+&data%5BRequestSupplier%5D%5BFileHidden%5D=180719_163342268.pdf&data%5BQuote%5D%5BRemark%5D%5B38700%5D=UOM%3A+PCS&data%5BQuote%5D%5BQTY_Suggested%5D%5B38700%5D=1&data%5BQuote%5D%5BUnitPrice%5D%5B38700%5D=289.19&data%5BQuote%5D%5BDiscount%5D%5B38700%5D=0&data%5BQuote%5D%5BRemark%5D%5B38701%5D=UOM%3A+PCS&data%5BQuote%5D%5BQTY_Suggested%5D%5B38701%5D=1&data%5BQuote%5D%5BUnitPrice%5D%5B38701%5D=525.39&data%5BQuote%5D%5BDiscount%5D%5B38701%5D=0";
            byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
            request.ContentLength = postBytes.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(postBytes, 0, postBytes.Length);
            stream.Close();


        }

        private void Quoteupdated()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.intuitships-supplynet.ummsportal.com/requisition_quote/submit/4428");

            request.KeepAlive = true;
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.Headers.Add("Origin", @"http://www.intuitships-supplynet.ummsportal.com");
            request.Headers.Add("X-Requested-With", @"XMLHttpRequest");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = "http://www.intuitships-supplynet.ummsportal.com/requisition_quote/submit/4428";
            request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-GB,en-US;q=0.9,en;q=0.8,und;q=0.7");
            request.Headers.Set(HttpRequestHeader.Cookie, @"CAKEPHP=5ea9e7236dc603c33b6debc733ae61bb");

            request.Method = "POST";
            request.ServicePoint.Expect100Continue = false;

            string body = @"data%5BRequestSupplier%5D%5BAction%5D=submit&data%5BRequestSupplier%5D%5BCurrencyCode%5D=USD&data%5BRequestSupplier%5D%5BDiscount%5D=0.00&data%5BRequestSupplier%5D%5BDeliveryCharges%5D=0.00&data%5BRequestSupplier%5D%5BSupplierReferenceNumber%5D=594017&data%5BRequestSupplier%5D%5BValidityPeriod%5D=0&data%5BRequestSupplier%5D%5BDeliveryLeadTime%5D=04%2F08%2F2018+23%3A55&data%5BRequestSupplier%5D%5BComment%5D=Thanks+for+your+inquiry.+We're+pleased+to+offer+our+quotation+for+your+consideration.+Please+note%3A+Vessel+is+now+under+credit+block.+All+outstanding+payments+need+to+be+settle+in+order+to+release+for+delivery.+All+quoted+items+are+ex-stock+subject+to+prio&data%5BRequestSupplier%5D%5BFileHidden%5D=&data%5BQuote%5D%5BRemark%5D%5B38700%5D=UOM%3A+PCS&data%5BQuote%5D%5BQTY_Suggested%5D%5B38700%5D=1&data%5BQuote%5D%5BUnitPrice%5D%5B38700%5D=289.19&data%5BQuote%5D%5BDiscount%5D%5B38700%5D=0.00&data%5BQuote%5D%5BRemark%5D%5B38701%5D=UOM%3A+PCS&data%5BQuote%5D%5BQTY_Suggested%5D%5B38701%5D=1&data%5BQuote%5D%5BUnitPrice%5D%5B38701%5D=525.39&data%5BQuote%5D%5BDiscount%5D%5B38701%5D=0.00";
            byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
            request.ContentLength = postBytes.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(postBytes, 0, postBytes.Length);
            stream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        }

        private void SumbitQuote()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.intuitships-supplynet.ummsportal.com/requisition_quote/submit/4428");

            request.KeepAlive = true;
            request.Headers.Add("Upgrade-Insecure-Requests", @"1");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
            request.Referer = "http://www.intuitships-supplynet.ummsportal.com/invoice";
            request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-GB,en-US;q=0.9,en;q=0.8,und;q=0.7");
            request.Headers.Set(HttpRequestHeader.Cookie, @"CAKEPHP=5ea9e7236dc603c33b6debc733ae61bb");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        }

        private void AfterSubmit()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.intuitships-supplynet.ummsportal.com/invoice");

            request.KeepAlive = true;
            request.Headers.Add("Upgrade-Insecure-Requests", @"1");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
            request.Referer = "http://www.intuitships-supplynet.ummsportal.com/requisition_quote/submit/4428";
            request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-GB,en-US;q=0.9,en;q=0.8,und;q=0.7");
            request.Headers.Set(HttpRequestHeader.Cookie, @"CAKEPHP=5ea9e7236dc603c33b6debc733ae61bb");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        }

        private bool Request_www_intuitships_supplynet_ummsportal_com(out HttpWebResponse response)
        {
            response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.intuitships-supplynet.ummsportal.com/ajax/upload_file");

                request.KeepAlive = true;
                request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0");
                request.Headers.Add("Origin", @"http://www.intuitships-supplynet.ummsportal.com");
                request.Headers.Add("Upgrade-Insecure-Requests", @"1");
                request.ContentType = "multipart/form-data; boundary=----WebKitFormBoundaryQSMtf2J0ePcb1ZSn";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                request.Referer = "http://www.intuitships-supplynet.ummsportal.com/requisition_quote/submit/4428";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-GB,en-US;q=0.9,en;q=0.8,und;q=0.7");
                request.Headers.Set(HttpRequestHeader.Cookie, @"CAKEPHP=2ed5705a51d17fb6a941cb543a72e0c6");

                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;

                string body = @"------WebKitFormBoundaryQSMtf2J0ePcb1ZSn
Content-Disposition: form-data; name=""document""; filename=""OrderQuotation-594017_180719_163342268.pdf""
Content-Type: application/pdf

<!>C:\Users\admin\AppData\Local\Temp\n1xfae5i\OrderQuotation-594017_180719_163342268.pdf<!>
------WebKitFormBoundaryQSMtf2J0ePcb1ZSn--
";
                WriteMultipartBodyToRequest(request, body);

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

        private static void WriteMultipartBodyToRequest(HttpWebRequest request, string body)
        {
            try
            {
                string[] multiparts = Regex.Split(body, @"<!>"); byte[] bytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    foreach (string part in multiparts)
                    {
                        if (File.Exists(part))
                        {
                            bytes = File.ReadAllBytes(part);
                        }
                        else
                        {
                            bytes = System.Text.Encoding.UTF8.GetBytes(part.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n"));
                        }

                        ms.Write(bytes, 0, bytes.Length);
                    }
                    request.ContentLength = ms.Length;
                    using (Stream stream = request.GetRequestStream())
                    {
                        ms.WriteTo(stream);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


//        private void Writefile()
//        {
//            HttpWebRequest requestToServerEndpoint =
//(HttpWebRequest)WebRequest.Create("http://localhost/GetPostRequest.php");
 
//string boundaryString = "----SomeRandomText";
//string fileUrl = @"C:\SomeRandomFile.pdf";
 
//// Set the http request header \\
//requestToServerEndpoint.Method = WebRequestMethods.Http.Post;
//requestToServerEndpoint.ContentType = "multipart/form-data; boundary=" + boundaryString;
//requestToServerEndpoint.KeepAlive = true;
//requestToServerEndpoint.Credentials = System.Net.CredentialCache.DefaultCredentials;
 
//// Use a MemoryStream to form the post data request,
//// so that we can get the content-length attribute.
//MemoryStream postDataStream = new MemoryStream();
//StreamWriter postDataWriter = new StreamWriter(postDataStream);
 
//// Include value from the myFileDescription text area in the post data
//postDataWriter.Write("\r\n--" + boundaryString + "\r\n");
//postDataWriter.Write("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}",
//"myFileDescription",
//"A sample file description");
 
//// Include the file in the post data
//postDataWriter.Write("\r\n--" + boundaryString + "\r\n");
//postDataWriter.Write("Content-Disposition: form-data;"
//+ "name=\"{0}\";"
//+ "filename=\"{1}\""
//+ "\r\nContent-Type: {2}\r\n\r\n",
//"myFile",
//Path.GetFileName(fileUrl),
//Path.GetExtension(fileUrl));
//postDataWriter.Flush();
 
//// Read the file
//FileStream fileStream = new FileStream(fileUrl, FileMode.Open, FileAccess.Read);
//byte[] buffer = new byte[1024];
//int bytesRead = 0;
//while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
//{
//    postDataStream.Write(buffer, 0, bytesRead);
//}
//fileStream.Close();
 
//postDataWriter.Write("\r\n--" + boundaryString + "--\r\n");
//postDataWriter.Flush();
 
//// Set the http request body content length
//requestToServerEndpoint.ContentLength = postDataStream.Length;
 
//// Dump the post data from the memory stream to the request stream
//using (Stream s = requestToServerEndpoint.GetRequestStream())
//{
//    postDataStream.WriteTo(s);
//}
//postDataStream.Close();

//WebResponse response = requestToServerEndpoint.GetResponse();
//StreamReader responseReader = new StreamReader(response.GetResponseStream());
//string replyFromServer = responseReader.ReadToEnd();
//        }

        #endregion

    }
}


