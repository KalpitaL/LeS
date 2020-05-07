using MTML.GENERATOR;
//using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Http_Asiatic_Routine
{
    public class Http_Download_Routine : LeSCommon.LeSCommon
    {
        string sAuditMesage = "", MailTextFilePath = "", DocType = "", VRNO = "", PO_Path = "", QuotePath = "", errStr = "", AuditName = "", BuyerName = "", SupplierName = "",
            FileType = "", EncyptUrls = "", cMsgFile = "", NoAuditName = "", LinkId="",cAutoItemNo="";
        //string strBuyerCode = "", strSupplierCode = "", strMessageNumber = "", strLeadDays = "", strCurrency = "", strMsgNumber = "", strMsgRefNumber = "", strUCRefNo = "",
        //    strAAGRefNo = "", strLesRecordID = "", BuyerName = "", strBuyerPhone = "", strBuyerEmail = "", strBuyerFax = "", strSupplierName = "",
        //    strSupplierPhone = "", strSupplierEmail = "", strSupplierFax = "", strPortName = "", strPortCode = "", strVesselName = "", strSupplierComment = "", strPayTerms = "",
        //    strPackingCost = "", strFreightCharge = "", strTotalLineItemsAmount = "", strGrandTotal = "", strAllowance = "", strDelvDate = "", strExpDate = ""
        //    , strBuyerTotal = "", strDepositCost = "", strOtherCost = "", AdditionalDiscount = "0", strTermCondition = "", strTransportModeCode="";
        //bool IsDecline = false; bool IsOrderItemGuid = false;
        //double itemsTotal=0;
        //int IsUOMChanged = 0, IsPriceAveraged = 0, IsAltItemAllowed = 0;
        int domainIndex = -1;
        string[] RFQDomains,PODomains;
        //public string[] Actions;
        //List<string> RFQ_mailFiles = new List<string>();
        //List<string> PO_mailFiles = new List<string>();
        List<LineItem> lstExtraItems = new List<LineItem>();
        //List<string> Quote_mtmlFiles = new List<string>();
        //List<string> POC_mtmlFiles = new List<string>();
        RichTextBox _txtData = new RichTextBox();
        //public MTMLInterchange _interchange { get; set; }
        //public LineItemCollection _lineitem = null;

        public void LoadAppsettings()
        {
            try
            {
                MailTextFilePath = ConfigurationManager.AppSettings["MAILTEXTFILE"].Trim();
                AuditPath = ConfigurationManager.AppSettings["AUDIT_PATH"].Trim();
                if (ConfigurationManager.AppSettings["SCREENSHOT_PATH"].Trim() != "")
                    PrintScreenPath = ConfigurationManager.AppSettings["SCREENSHOT_PATH"].Trim();
                DownloadPath = ConfigurationManager.AppSettings["XML_PATH"].Trim();
                //BuyerCode = ConfigurationManager.AppSettings["BUYER_CODE"].Trim();
                //SupplierCode = ConfigurationManager.AppSettings["SUPPLIER_CODE"].Trim();
                PO_Path = ConfigurationManager.AppSettings["PO_PATH"].Trim();
                //QuotePath = ConfigurationManager.AppSettings["MTML_QUOTEPATH"].Trim();
                if (PrintScreenPath == "") PrintScreenPath = AppDomain.CurrentDomain.BaseDirectory + "ScreenShots";
                if (DownloadPath == "") DownloadPath = AppDomain.CurrentDomain.BaseDirectory + "Download";
                if (AuditPath == "") AuditPath = AppDomain.CurrentDomain.BaseDirectory + "AuditLog";
                if (QuotePath == "") QuotePath = AppDomain.CurrentDomain.BaseDirectory + "MTML_Upload";

                if (!Directory.Exists(PrintScreenPath)) Directory.CreateDirectory(PrintScreenPath);
                if (!Directory.Exists(AuditPath)) Directory.CreateDirectory(AuditPath);
                if (!Directory.Exists(DownloadPath)) Directory.CreateDirectory(DownloadPath);
                if (!Directory.Exists(DownloadPath + "\\Backup")) Directory.CreateDirectory(DownloadPath + "\\Backup");
                if (!Directory.Exists(DownloadPath + "\\Error")) Directory.CreateDirectory(DownloadPath + "\\Error");
                if (!Directory.Exists(PO_Path)) Directory.CreateDirectory(PO_Path);
                if (!Directory.Exists(MailTextFilePath + "\\Backup")) Directory.CreateDirectory(MailTextFilePath + "\\Backup");//added on 13-2-19
                if (!Directory.Exists(MailTextFilePath + "\\Error")) Directory.CreateDirectory(MailTextFilePath + "\\Error");//added on 13-2-19
                this.RFQDomains = ConfigurationManager.AppSettings["RFQ_DOMAIN"].Split('|');
                this.PODomains = ConfigurationManager.AppSettings["PO_DOMAIN"].Split('|');

                EncyptUrls = Convert.ToString(ConfigurationManager.AppSettings["ENCYRPTURL"]);//added by Kalpita on 26/06/2019
                NoAuditName = Convert.ToString(ConfigurationManager.AppSettings["NO_AUDITNAME"].Trim());//added by Kalpita on 27/11/2019

                cAutoItemNo = Convert.ToString(ConfigurationManager.AppSettings["AUTO_ITEMNO"].Trim());//added by Kalpita on 29/01/2020
            }
            catch (Exception e)
            {
                LogText = sAuditMesage; sAuditMesage = "Exception in LoadAppsettings: " + e.GetBaseException().ToString();
                // CreateAuditFile("", "Asiatic_HTTP", "", "Error", sAuditMesage, BuyerCode, SupplierCode, AuditPath);//changed by Kalpita on 01/11/2019
            }
        }

        #region RFQ
        public void Read_MailTextFiles()
        {
            int j = 0;
            string filename = "";
            try
            {
                LogText = "";
                DirectoryInfo _dir = new DirectoryInfo(MailTextFilePath);              
                if (_dir.GetFiles().Length > 0)
                {
                    FileInfo[] _Files = _dir.GetFiles();
                    foreach (FileInfo _MtmlFile in _Files)
                    {
                        filename = _MtmlFile.FullName; cMsgFile = Path.GetFileNameWithoutExtension(filename) + ".msg"; AuditName = NoAuditName;
                     //   URL = GetURL_test(filename); //testing by kalpita on 14/02/2020
                        
                        URL = GetURL(filename);

                        //Added By Anita - 2019-09-06
                        //'</a' was picked up along with RFQ Url while extracting the url from msg/txt file
                        URL = RemoveHtmlTagIfPresentInUrl(URL);

                        if (URL != "")
                        {
                            if (domainIndex >= 0)
                            {
                                GetAppSettings();
                                if (this.DocType == "RFQ")
                                {
                                    LogText = "Processing RFQ file '" + Path.GetFileName(filename) + "'";
                                    LogText = URL;  //Added By Anita Vishwasrao - 2019-09-06
                                    File.Copy(filename, PrintScreenPath + "\\" + Path.GetFileName(filename), true);//added by Kalpita on 26/06/2019
                                    try
                                    {
                                        DownloadRFQ(filename);
                                    }
                                    catch (Exception e)
                                    {
                                        // WriteErrorLog_With_Screenshot("Exception while processing RFQ -" + cMsgFile + ": " + e.GetBaseException().Message.ToString(), filename);
                                        WriteErrorLog_With_Screenshot("Unable to process file " + cMsgFile + " due to " + e.GetBaseException().Message.ToString(), filename, "LeS-1004:");
                                        if (File.Exists(MailTextFilePath + "\\Error\\" + Path.GetFileName(filename))) File.Delete(MailTextFilePath + "\\Error\\" + Path.GetFileName(filename));
                                        File.Copy(filename, PrintScreenPath + "\\" + Path.GetFileName(filename), true); File.Move(filename, MailTextFilePath + "\\Error\\" + Path.GetFileName(filename));
                                    }
                                }
                                else if (this.DocType == "PO")
                                {
                                    LogText = "Processing PO file '" + Path.GetFileName(filename);
                                    try
                                    {
                                        File.Copy(filename, PrintScreenPath + "\\" + Path.GetFileName(filename), true);//added by Kalpita on 26/06/2019
                                        ProcessOrders(filename);
                                    }
                                    catch (Exception e)
                                    {
                                        //WriteErrorLog_With_Screenshot("Exception while processing PO -" + cMsgFile + ": " + e.GetBaseException().Message.ToString(), filename);
                                        WriteErrorLog_With_Screenshot("Unable to process file " + cMsgFile + " due to " + e.GetBaseException().Message.ToString(), filename, "LeS-1004:");
                                        if (File.Exists(MailTextFilePath + "\\Error\\" + Path.GetFileName(filename))) File.Delete(MailTextFilePath + "\\Error\\" + Path.GetFileName(filename));
                                        File.Move(filename, MailTextFilePath + "\\Error\\" + Path.GetFileName(filename));
                                    }
                                }
                            }
                            else
                            {
                                LogText = "Unable to get domain index for file: " + Path.GetFileName(filename);
                                //CreateAuditFile(Path.GetFileName(filename), "Asiatic_HTTP", "", "Error", "Unable to get domain index for file: " + Path.GetFileName(filename), BuyerCode, SupplierCode, AuditPath);
                                CreateAuditFile(Path.GetFileName(filename), AuditName + "_HTTP", "", "Error", "LeS-1004:Unable to process file: " + Path.GetFileName(filename), BuyerCode, SupplierCode, AuditPath);
                                if (File.Exists(MailTextFilePath + "\\Error\\" + Path.GetFileName(filename))) File.Delete(MailTextFilePath + "\\Error\\" + Path.GetFileName(filename));
                                File.Copy(filename, PrintScreenPath + "\\" + Path.GetFileName(filename)); File.Move(filename, MailTextFilePath + "\\Error\\" + Path.GetFileName(filename));
                            }
                        }
                        else
                        {
                            LogText = "URL not found for navigation in file: " + Path.GetFileName(filename);
                            //CreateAuditFile(Path.GetFileName(filename), "Asiatic_HTTP", "", "Error", "URL not found for navigation in file: " + Path.GetFileName(filename), BuyerCode, SupplierCode, AuditPath);
                            CreateAuditFile(Path.GetFileName(filename), AuditName + "_HTTP", "", "Error", "LeS-1001:Unable to find URL in file: " + Path.GetFileName(filename), BuyerCode, SupplierCode, AuditPath);
                            if (File.Exists(MailTextFilePath + "\\Error\\" + Path.GetFileName(filename))) File.Delete(MailTextFilePath + "\\Error\\" + Path.GetFileName(filename));
                            File.Copy(filename, PrintScreenPath + "\\" + Path.GetFileName(filename)); File.Move(filename, MailTextFilePath + "\\Error\\" + Path.GetFileName(filename));
                        }
                        clearValues();
                        LogText = "--------------------------------------";
                    }
                }
                else LogText = "No file found.";
            }
            catch (Exception e)
            {
                //WriteErrorLog_With_Screenshot("Exception while processing file " : " + e.GetBaseException().Message.ToString(), filename);
                //  WriteErrorLog_With_Screenshot("Exception while processing file - " + cMsgFile + " : " + e.GetBaseException().Message.ToString(), filename);
                WriteErrorLog_With_Screenshot("Unable to process file " + cMsgFile + " due to " + e.GetBaseException().Message.ToString(), filename, "LeS-1004:");
                if (File.Exists(MailTextFilePath + "\\Error\\" + Path.GetFileName(filename))) File.Delete(MailTextFilePath + "\\Error\\" + Path.GetFileName(filename));
                File.Move(filename, MailTextFilePath + "\\Error\\" + Path.GetFileName(filename));
            }
        }

        private string GetURL_test(string filename)
        {
            string _url = "", ResultURL = ""; bool IsRFQ = false;
            try
            {
                LogText = "";
                LogText = "Reading file " + Path.GetFileName(filename);
                _txtData.Text = File.ReadAllText(filename);//.Replace(Environment.NewLine,"");
                
                if (_txtData.Lines[0].ToLower().Contains("request for quotation") || _txtData.Lines[0].ToLower().Contains("inquiry") || _txtData.Lines[0].ToLower().Contains("rfq no") || _txtData.Lines[0].ToLower().Contains("rfq"))
                {
                    IsRFQ = true;
                }
                if (IsRFQ)
                {
                    foreach (string _rdomain in RFQDomains)
                    {
                        if (_rdomain != "")
                        {
                            string domname = _rdomain.Replace("https://", "");
                            for (int i = 0; i < _txtData.Lines.Length; i++)
                            {
                                if (_txtData.Lines[i].Contains(domname))
                                {
                                    this.DocType = "RFQ";
                                    domainIndex = Array.IndexOf(RFQDomains, domname);
                                    string line = _txtData.Lines[i]; string nxtline = _txtData.Lines[i + 1];
                                    if (!string.IsNullOrEmpty(nxtline))
                                    {
                                        line += nxtline;
                                    }
                                    else { break; }
                                }

                            }
                        }
                    }
                }
                else
                {
                    string purl = "";
                    foreach (string _pdomain in PODomains)
                    {
                        if (_pdomain != "")
                        {
                            string domname = _pdomain.Replace("https://", "");
                            for (int i = 0; i < _txtData.Lines.Length; i++)
                            {
                                if (_txtData.Lines[i].Contains(domname))
                                {
                                    this.DocType = "PO";
                                    domainIndex = Array.IndexOf(PODomains, _pdomain);             
                                    string[] _arr = Regex.Matches(_txtData.Lines[i], @"\<https(.+?)\>", RegexOptions.Multiline)
                                            .Cast<Match>()
                                            .Select(m => m.Value).ToArray();
                                   this.URL  = _arr[0].Replace('<',' ').Replace('>',' ').Trim();
                                    try
                                    {
                                        if (LoadURL("", "", "", true))
                                        {
                                            purl = Convert.ToString(_httpWrapper._CurrentResponse.ResponseUri);
                                        }
                                    }
                                    catch (System.Net.WebException ex)
                                    {
                                        purl = (ex.Response).ResponseUri.AbsoluteUri;
                                    }
                                    if (!string.IsNullOrEmpty(purl)) {  break; }
                                }

                            }
                        }
                        _url = purl;
                    }

                }
                _url = _url.Trim().Replace("&amp;", "&").Trim().TrimStart('<').TrimStart('"').TrimEnd('>').TrimEnd('"').Trim();
                if (_url.Contains(' ')) _url = _url.Split(' ')[0].Trim('.').Trim('\"');//updated .Trim('\"'); on 08-04-2019

                if (IsRFQ && _url == "") LogText = "RFQ Domain not found in list.";//changed by Kalpita on 27/11/2019
                else if (!IsRFQ && _url == "") LogText = "PO Domain not found in list.";//changed by Kalpita on 27/11/2019
            }
            catch (Exception ex)
            {
            }
            return _url;
        }

        public void DownloadRFQ(string RFQFile)
        {
            string cMsgFile = Path.GetFileNameWithoutExtension(RFQFile) + ".msg";
            try
            {
                List<string> slProcessedItem = GetProcessedItems(eActions.RFQ, "RFQ");
                if (!slProcessedItem.Contains(URL))
                {
                    if (LoadURL("", "", ""))
                    {
                        string ActualURL = URL;
                        URL = URL.Replace("apps/quote", "exapi/AnswerInquiry");
                        SetDownloadHeaders();
                        if (LoadURL("", "", ""))
                        {
                            string _remarks = "", _payTerms = "", _suppContact = "", _currency = "", _deliveryDate = "", _etaDate = "", _etdDate = "", _billingCompany = "", _billingAddress = "";
                            var _library = (RootObject)JsonConvert.DeserializeObject(_httpWrapper._CurrentResponseString, typeof(RootObject));
                            if (_library != null)
                            {
                                LogText = "RFQ data extracted successfully.";

                                #region header details
                                string _inqno = _library.Result.InquiryNumber;
                                VRNO = _inqno;
                                string _vessel = _library.Result.VesselName;
                                if (_library.Result.RequestedInquiryTerms.CurrencyCode != null)
                                    _currency = _library.Result.RequestedInquiryTerms.CurrencyCode;

                                if (_library.Result.ReplyRequestedUntil != null)
                                {
                                    _remarks = "Please reply before: " + Convert.ToDateTime(_library.Result.ReplyRequestedUntil).ToString("M/d/yyyy");
                                }

                                if (_library.Result.RequestedInquiryTerms.PaymentTargetDays != null && _library.Result.RequestedInquiryTerms.PaymentTermText != null && _library.Result.RequestedInquiryTerms.DeliveryLeadTimeUnit != null)
                                {
                                    string val = "";
                                    switch (_library.Result.RequestedInquiryTerms.DeliveryLeadTimeUnit)
                                    {
                                        case "d": val = "Days"; break;
                                        case "m": val = "Months"; break;
                                    }

                                    _payTerms = _library.Result.RequestedInquiryTerms.PaymentTermText + " (" + _library.Result.RequestedInquiryTerms.PaymentTargetDays + " " + val + ")";
                                }

                                if (_library.Result.RequestedInquiryTerms.GeneralTermsConditions != null)
                                    _remarks = " General Terms & conditions: " + (string)_library.Result.RequestedInquiryTerms.GeneralTermsConditions;

                                string _port = _library.Result.DeliveryTo;
                                string _documentDate = _library.Result.InquirySentDate.ToString("yyyyMMdd");

                                if (_library.Result.RequestedDeliveryDate != null)
                                {
                                    if (_library.Result.RequestedDeliveryDate.ToString().ToLower() == "as soon as possible")
                                        _deliveryDate = Convert.ToDateTime(_library.Result.RequestedDeliveryDate).ToString("yyyyMMdd");
                                }

                                if (_library.Result.DeliveryETA != null)
                                {
                                    _etaDate = Convert.ToDateTime(_library.Result.DeliveryETA).ToString("yyyyMMdd");
                                }

                                if (_library.Result.DeliveryETD != null)
                                    _etdDate = Convert.ToDateTime(_library.Result.DeliveryETD).ToString("yyyyMMdd");

                                if (_library.Result.Remarks != "")
                                    _remarks += " Inquiry Remarks: " + _library.Result.Remarks;
                                #endregion

                                #region address details

                                if (_library.Result.ResponsibleUserName != null)
                                    _suppContact = _library.Result.ResponsibleUserName;

                                if (_library.Result.BillingCompany != null)
                                    _billingCompany = _library.Result.BillingCompany;

                                if (_library.Result.BillingAddress != null)
                                    _billingAddress = _library.Result.BillingAddress;
                                #endregion

                                LeSXML.LeSXML _lesXml = new LeSXML.LeSXML();
                                if (GetRFQHeader(ref _lesXml, _inqno, _vessel, _currency, _remarks, _payTerms, _port, _documentDate, _deliveryDate, _etaDate, _etdDate, ActualURL))
                                {
                                    if (_library.Result.Items.Count > 0)
                                    {
                                        if (GetRFQItems(ref _lesXml, _library.Result.Items))
                                        {
                                            if (GetAddress(ref _lesXml, _suppContact, _billingCompany, _billingAddress))
                                            {
                                                _lesXml.FileName = Path.GetFileNameWithoutExtension(RFQFile) + "_RFQ_" + VRNO.Replace("/", "_") + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xml";
                                                if (_lesXml.Total_LineItems.Length > 0)
                                                {
                                                    string CurrenctXMLFile = _lesXml.FilePath + "\\" + _lesXml.FileName;

                                                    _lesXml.WriteXML();
                                                    if (File.Exists(CurrenctXMLFile))
                                                    {
                                                        // LogText = Path.GetFileName(CurrenctXMLFile) + " downloaded successfully.";
                                                        //CreateAuditFile(Path.GetFileName(CurrenctXMLFile), AuditName+"_HTTP_RFQ", _inqno, "Downloaded", Path.GetFileName(CurrenctXMLFile) + " downloaded successfully.", BuyerCode, SupplierCode, AuditPath);
                                                        //if (File.Exists(Path.GetDirectoryName(RFQFile) + "\\Backup\\" + Path.GetFileName(RFQFile))) File.Delete(Path.GetDirectoryName(RFQFile) + "\\Backup\\" + Path.GetFileName(RFQFile));
                                                        //File.Move(RFQFile, Path.GetDirectoryName(RFQFile) + "\\Backup\\" + Path.GetFileName(RFQFile));
                                                        //SetGUIDs(ActualURL, "RFQ");

                                                        //added by kalpita on 26/06/2019   
                                                        LogText = Path.GetFileName(CurrenctXMLFile) + " downloaded successfully for " + cMsgFile;
                                                        CreateAuditFile(Path.GetFileName(CurrenctXMLFile), AuditName + "_HTTP_RFQ", _inqno, "Downloaded", Path.GetFileName(CurrenctXMLFile) + " downloaded successfully for " + cMsgFile, BuyerCode, SupplierCode, AuditPath);
                                                        File.Copy(RFQFile, PrintScreenPath + "\\" + Path.GetFileName(RFQFile), true);
                                                        if (File.Exists(Path.GetDirectoryName(RFQFile) + "\\Backup\\" + Path.GetFileName(RFQFile))) File.Delete(Path.GetDirectoryName(RFQFile) + "\\Backup\\" + Path.GetFileName(RFQFile));
                                                        File.Move(RFQFile, Path.GetDirectoryName(RFQFile) + "\\Backup\\" + Path.GetFileName(RFQFile));
                                                        SetGUIDs(ActualURL, "RFQ");
                                                        //clearValues();
                                                    }
                                                    else
                                                    {
                                                        //string eFile = PrintScreenPath + AuditName+"\\_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
                                                        //LogText = "Unable to download file " + Path.GetFileName(CurrenctXMLFile);
                                                        //CreateAuditFile(eFile, AuditName+"_HTTP_RFQ", _inqno, "Error", "Unable to download file " + Path.GetFileName(CurrenctXMLFile) + " for ref " + _inqno + ".", BuyerCode, SupplierCode, AuditPath);
                                                        //if (!PrintScreen(eFile)) eFile = "";
                                                        //if (File.Exists(Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile))) File.Delete(Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile));
                                                        //File.Move(RFQFile, Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile));

                                                        //added by kalpita on 26/06/2019   
                                                        string eFile = PrintScreenPath + "\\" + NoAuditName + "\\_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
                                                        LogText = "Unable to download file " + Path.GetFileName(CurrenctXMLFile);
                                                      //  CreateAuditFile(eFile, AuditName + "_HTTP_RFQ", _inqno, "Error", "Unable to download file " + Path.GetFileName(CurrenctXMLFile) + " for ref " + _inqno + ".", BuyerCode, SupplierCode, AuditPath);
                                                        CreateAuditFile(eFile, AuditName + "_HTTP_RFQ", _inqno, "Error", "LeS-1004:Unable to process file " + Path.GetFileName(CurrenctXMLFile) + " for ref " + _inqno + ".", BuyerCode, SupplierCode, AuditPath);
                                                        if (!PrintScreen(eFile)) eFile = "";
                                                        if (File.Exists(Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile))) File.Delete(Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile));
                                                        File.Move(RFQFile, Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                LogText = "Unable to get address details";
                                                //CreateAuditFile(Path.GetFileName(RFQFile), AuditName + "_HTTP_RFQ", _inqno, "Error", "Unable to get address details", BuyerCode, SupplierCode, AuditPath);
                                                CreateAuditFile(Path.GetFileName(RFQFile), AuditName + "_HTTP_RFQ", _inqno, "Error", "LeS-1040:Unable to get detail- address  Field(s) not present", BuyerCode, SupplierCode, AuditPath);
                                                File.Move(RFQFile, Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile));
                                            }
                                        }
                                        else
                                        {
                                            LogText = "Unable to get RFQ item details";
                                            //CreateAuditFile(Path.GetFileName(RFQFile), AuditName + "_HTTP_RFQ", _inqno, "Error", "Unable to get RFQ item details", BuyerCode, SupplierCode, AuditPath);
                                            CreateAuditFile(Path.GetFileName(RFQFile), AuditName + "_HTTP_RFQ", _inqno, "Error", "LeS-1040:Unable to get detail - item Field(s) not present", BuyerCode, SupplierCode, AuditPath);
                                            File.Move(RFQFile, Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile));
                                        }
                                    }
                                }
                                else
                                {
                                    LogText = "Unable to get RFQ header details";
                                    //CreateAuditFile(Path.GetFileName(RFQFile), AuditName + "_HTTP_RFQ", _inqno, "Error", "Unable to get RFQ header details", BuyerCode, SupplierCode, AuditPath);
                                    CreateAuditFile(Path.GetFileName(RFQFile), AuditName + "_HTTP_RFQ", _inqno, "Error", "LeS-1040:Unable to get detail - header Field(s) not present", BuyerCode, SupplierCode, AuditPath);
                                    File.Move(RFQFile, Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile));
                                }
                            }
                        }
                        else
                        {
                            //WriteErrorLog_With_Screenshot("Unable to get page data: " + URL, RFQFile);
                            WriteErrorLog_With_Screenshot("Unable to Open site - " + URL, RFQFile, "LeS-1006:");
                        }
                    }
                    else
                    {
                       // WriteErrorLog_With_Screenshot("Unable to load Url: " + URL, RFQFile);
                        WriteErrorLog_With_Screenshot("Unable to load Url: " + URL, RFQFile, "LeS-1016:");
                    }
                }
                else
                {
                    LogText = "RFQ for url '" + URL + "' already processed.";
                    File.Move(RFQFile, Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetGUIDs(string URL, string doctype)
        {
            using (StreamWriter sw = new StreamWriter(@AppDomain.CurrentDomain.BaseDirectory +AuditName+"_"+ doctype + "_Downloaded.txt", true))
            {
                sw.WriteLine(URL);
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
        }

        public List<string> GetProcessedItems(eActions eAction, string doctype)
        {
            string sDoneFile = "";
            List<string> slProcessedItems = new List<string>();
            switch (eAction)
            {
                case eActions.RFQ: sDoneFile = AppDomain.CurrentDomain.BaseDirectory + AuditName+"_"+doctype + "_Downloaded.txt"; break;
                case eActions.PO: sDoneFile = AppDomain.CurrentDomain.BaseDirectory + AuditName + "_" + doctype + "_Downloaded.txt"; break;
                default: break;
            }
            if (File.Exists(sDoneFile))
            {
                string[] _Items = File.ReadAllLines(sDoneFile);
                slProcessedItems.AddRange(_Items.ToList());
            }
            return slProcessedItems;
        }

        private void SetDownloadHeaders()
        {
            _httpWrapper.AcceptMimeType = "application/json, text/javascript, */*; q=0.01";
            _httpWrapper.ContentType = "application/json; charset=utf-8";
            _httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.CacheControl);
            _httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");
            if (!_httpWrapper._AddRequestHeaders.ContainsKey("X-Requested-With"))
            _httpWrapper._AddRequestHeaders.Add("X-Requested-With", @"XMLHttpRequest");
            if (!_httpWrapper._AddRequestHeaders.ContainsKey("X-Api-Version"))
            _httpWrapper._AddRequestHeaders.Add("X-Api-Version", @"2");
        }

        public bool GetRFQHeader(ref LeSXML.LeSXML _lesXml, string _inqno, string _vessel, string _currency, string _remarks, string _payTerms, string _port, string _documentDate, string _deliveryDate, string _etaDate, string _etdDate, string _url)
        {
            bool isResult = false;
            LogText = "Start Getting Header details";
            try
            {
                _lesXml.DocID = DateTime.Now.ToString("yyyyMMddhhmmss");
                _lesXml.Created_Date = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Doc_Type = "RFQ";
                _lesXml.Dialect = AuditName;//"Asiatic Lloyd Shipmanagement";
                _lesXml.Version = "1";
                _lesXml.Date_Document = _documentDate;
                _lesXml.Date_Preparation = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Sender_Code = BuyerCode;
                _lesXml.Recipient_Code = SupplierCode;
                _lesXml.DocReferenceID = _inqno;
                _lesXml.BuyerRef = _inqno;
                _lesXml.DocLinkID = _url;
                _lesXml.Vessel = _vessel;
                _lesXml.Currency = _currency;
                _lesXml.Remark_Header = _remarks;
                _lesXml.Remark_PaymentTerms = _payTerms;
                _lesXml.PortName = _port;
                _lesXml.Date_Delivery = _deliveryDate;
                _lesXml.Date_ETA = _etaDate;
                _lesXml.Date_ETD = _etdDate;
                _lesXml.Active = "1";
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

        public bool GetRFQItems(ref LeSXML.LeSXML _lesXml, List<Item> Items)
        {
            bool isResult = false;
            try
            {
                _lesXml.LineItems.Clear();
                LogText = "Start Getting LineItem details";
                for (int i = 0; i < Items.Count; i++)
                {
                    Item _data = Items[i];
                    LeSXML.LineItem _item = new LeSXML.LineItem();
                    try
                    {
                        string Remarks = "", IsAvail = "";
                        //_item.Number = Convert.ToString(i + 1);
                        //_item.OrigItemNumber = Convert.ToString(i + 1);
                        //_item.SystemRef = Convert.ToString(i + 1);
                        if (cAutoItemNo.ToUpper() == "FALSE") //changed by kalpita on 29/01/2020 for Kloska
                        {
                            _item.Number = Convert.ToString(_data.LineNumber);
                        }
                        else
                        {
                            _item.Number = Convert.ToString(i + 1);
                        }
                        _item.OrigItemNumber = Convert.ToString(i + 1);
                        _item.SystemRef = Convert.ToString(i + 1);
                        if (_data.OrderItemGuid != null)
                            _item.OriginatingSystemRef = _data.OrderItemGuid;
                        else
                            _item.OriginatingSystemRef = _data.InquiryItemGuid;

                        _item.Name = _data.Name;
                        _item.ItemRef = _data.ItemNumber;
                        if (_data.Quantity != null)
                            _item.Quantity = convert.ToString(_data.Quantity);
                        else _item.Quantity = convert.ToString(0);

                        if (_data.Description != "")
                            Remarks = "Description: " + _data.Description;

                        if (_data.CatalogName != "")
                            Remarks += "Catalog: " + _data.CatalogName + ".";

                        if (_data.IsAvailable) IsAvail = "Available";
                        else IsAvail = "Not Available";

                        Remarks += " Availability: " + IsAvail;

                        _item.Remark = Remarks;
                        _item.Unit = _data.Unit;
                        _item.Discount = "0";
                        _item.ListPrice = "0";
                        _item.LeadDays = "0";
                        _lesXml.LineItems.Add(_item);
                    }
                    catch (Exception ex)
                    { LogText = ex.GetBaseException().ToString(); }
                }
                _lesXml.Total_LineItems = Convert.ToString(_lesXml.LineItems.Count);
                isResult = true;
                LogText = "Getting LineItem details successfully";
                return isResult;
            }
            catch (Exception ex)
            {
                LogText = "Exception while getting RFQ Items: " + ex.GetBaseException().ToString(); isResult = false; return isResult;
            }
        }

        public bool GetAddress(ref LeSXML.LeSXML _lesXml, string _suppContact, string _billingCompany, string _billingAddress)
        {
            bool isResult = false;
            try
            {
                _lesXml.Addresses.Clear();
                LogText = "Start Getting address details";
                LeSXML.Address _xmlAdd = new LeSXML.Address();

                _xmlAdd.Qualifier = "BY";
                _xmlAdd.AddressName = BuyerName; //ConfigurationManager.AppSettings["BUYER_NAME"].Trim();
                _lesXml.Addresses.Add(_xmlAdd);

                _xmlAdd = new LeSXML.Address();
                _xmlAdd.Qualifier = "VN";
                _xmlAdd.AddressName = SupplierName; //ConfigurationManager.AppSettings["SUPPLIER_NAME"].Trim();
                _xmlAdd.ContactPerson = _suppContact;
                _lesXml.Addresses.Add(_xmlAdd);

                _xmlAdd = new LeSXML.Address();
                _xmlAdd.Qualifier = "BA";
                _xmlAdd.AddressName = _billingCompany;
                _xmlAdd.Address1 = _billingAddress;
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

        public void GetMailTextFiles()
        {
            Directory.CreateDirectory(MailTextFilePath + "\\Error");
            Directory.CreateDirectory(MailTextFilePath + "\\Backup");
        }

        private string GetURL(string emlFile)
        {
            string _url = "", ResultURL = ""; bool IsRFQ = false;
            try
            {
                LogText = "";
                LogText = "Reading file " + Path.GetFileName(emlFile);
                _txtData.Text = File.ReadAllText(emlFile);

                if (_txtData.Lines[0].ToLower().Contains("request for quotation") || _txtData.Lines[0].ToLower().Contains("inquiry") || _txtData.Lines[0].ToLower().Contains("rfq no") || _txtData.Lines[0].ToLower().Contains("rfq"))
                {
                    IsRFQ = true;
                }
                if (IsRFQ)
                {
                    foreach (string _domain in RFQDomains)
                    {
                        if (_domain != "")
                        {
                            if (_txtData.Text.Contains(_domain))
                            {
                                domainIndex = Array.IndexOf(RFQDomains, _domain);
                                if (_txtData.Text.Contains(_domain + "/apps/quote"))
                                {
                                    for (int i = 0; i < _txtData.Lines.Length; i++)
                                    {
                                        string line = _txtData.Lines[i];
                                        if (line.Contains(_domain + "/apps/quote"))
                                        {
                                            //get URL
                                            int startIndex = line.IndexOf(_domain + "/apps/quote");
                                            if (line.Length > startIndex)
                                            {
                                                _url = line.Substring(startIndex);
                                            }
                                            else _url = line.Trim();

                                            if (_url.Contains(">"))
                                            {
                                                int endIndex = _url.IndexOf(">");
                                                if (line.Length > endIndex)
                                                {
                                                    _url = _url.Substring(0, endIndex);
                                                }
                                            }                                         
                                            else if (_url.EndsWith("."))
                                                _url = _url.Trim('.');

                                            //else _url = line.Trim().Replace(".", "");
                                            _url += Get_Url(_txtData, i);//added by kalpita on 01/11/2019
                                            if (!string.IsNullOrEmpty(_url)) { ResultURL = _url; }
                                            break;
                                        }
                                    }
                                    this.DocType = "RFQ";
                                }
                                if (!string.IsNullOrEmpty(ResultURL)) { break; }
                            }
                            else
                            {
                              //  _url = GetCorrectURL(_txtData, _domain, RFQDomains); //added on 26/06/2019 by Kalpita
                                if (!string.IsNullOrEmpty(_url))
                                {
                                    this.DocType = "RFQ";
                                    break;
                                }
                            }

                        }
                    }
                }
                else
                {
                    //if (domainIndex > 0 || domainIndex < 0)
                    if (domainIndex < 0)
                    {
                        foreach (string _domain in PODomains)
                        {
                            if (_domain != "")
                            {                               
                                if (_txtData.Text.Contains(_domain))
                                {
                                    domainIndex = Array.IndexOf(PODomains, _domain);
                                    if (_txtData.Text.Contains(_domain + "/private/exports/purchase/orders"))
                                    {
                                        for (int i = 0; i < _txtData.Lines.Length; i++)
                                        {
                                            string line = _txtData.Lines[i];
                                            if (line.Contains(_domain + "/private/exports/purchase/orders"))
                                            {
                                                //get URL
                                                int startIndex = line.IndexOf(_domain + "/private/exports/purchase/orders");
                                                if (line.Length > startIndex)
                                                {
                                                    _url = line.Substring(startIndex);
                                                }
                                                else _url = line.Trim();

                                                int endIndex = _url.IndexOf(">");
                                                if (endIndex != -1)
                                                {
                                                    if (line.Length > endIndex)
                                                    {
                                                        _url = _url.Substring(0, endIndex);
                                                    }
                                                    else _url = line.Trim();
                                                }
                                                else _url = _url.Trim();
                                                _url += Get_Url(_txtData, i);//added by kalpita on 01/11/2019
                                                this.DocType = "PO";
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(_url)) { }
                                    else
                                    {
                                       // _url = GetCorrectURL(_txtData, _domain, PODomains);//added on 26/06/2019 by Kalpita
                                        if (!string.IsNullOrEmpty(_url))
                                        {
                                            this.DocType = "PO";
                                            break;
                                        }
                                    }
                                }
                                //}
                            }
                        }
                    }
                }
                // }
                _url = _url.Trim().Replace("&amp;", "&").Trim().TrimStart('<').TrimStart('"').TrimEnd('>').TrimEnd('"').Trim();
                if (_url.Contains(' ')) _url = _url.Split(' ')[0].Trim('.').Trim('\"');//updated .Trim('\"'); on 08-04-2019

                if (IsRFQ && _url == "") LogText = "RFQ Domain not found in list.";//changed by Kalpita on 27/11/2019
                else if (!IsRFQ && _url == "") LogText = "PO Domain not found in list.";//changed by Kalpita on 27/11/2019
            }
            catch (Exception ex)
            {
            }
            return _url;
        }



        private string GetCompleteUrl(RichTextBox txtData, int startIndex)
        {
            string _url = ""; int urlindx = 0;
            for (int i = startIndex + 1; i < _txtData.Lines.Length; i++)//
            {
                string line = _txtData.Lines[i];
                if (!string.IsNullOrEmpty(line))
                {
                    _url += line.Trim().Replace(">", "").Replace(".", "");
                }
                else { break; }
            }
            return _url;
        }

        private string Get_Url(RichTextBox txtData, int startIndex)
        {
            string _url = "";
            for (int i = startIndex + 1; i < _txtData.Lines.Length; i++)//
            {
                string line = _txtData.Lines[i];
                if (!string.IsNullOrEmpty(line))
                {
                    _url += line;
                }
                else { break; }
            }
            return _url;
        }

        public void clearValues()
        { VRNO = ""; domainIndex = -1; BuyerCode = ""; SupplierCode = ""; AuditName = ""; BuyerName = ""; SupplierName = ""; }

        public void GetAppSettings()
        {
            try
            {
                if (this.DocType == "RFQ")
                {
                    BuyerCode = ConfigurationManager.AppSettings["BUYER_RFQ_CODE"].Trim().Split('|')[domainIndex];
                }
                else if (this.DocType == "PO")
                {
                    BuyerCode = ConfigurationManager.AppSettings["BUYER_PO_CODE"].Trim().Split('|')[domainIndex];
                }
                SupplierCode = ConfigurationManager.AppSettings["SUPPLIER_CODE"].Trim().Split('|')[domainIndex];
                AuditName = ConfigurationManager.AppSettings["AUDITNAME"].Trim().Split('|')[domainIndex];
                BuyerName = ConfigurationManager.AppSettings["BUYER_NAME"].Trim().Split('|')[domainIndex];
                SupplierName = ConfigurationManager.AppSettings["SUPPLIER_NAME"].Trim().Split('|')[domainIndex];
                LinkId = ConfigurationManager.AppSettings["BUYER_SUPP_LINKID"].Trim().Split('|')[domainIndex];      
                if(BuyerCode=="" || SupplierCode=="")
                {
                    throw new Exception("BuyerCode or SupplierCode not found in list.");
                }
            }
            catch(Exception ex)
            {
            throw;
            }
        }
        #endregion

        #region Order
        public void ProcessOrders(string POFile)
        {
            string cMsgFile = Path.GetFileNameWithoutExtension(POFile) + ".msg";
            try
            {
                if (URL != "")
                {
                    List<string> slProcessedItem = GetProcessedItems(eActions.PO, "PO");
                    if (!slProcessedItem.Contains(URL))
                    {
                        string Filename = LinkId + "_" + Path.GetFileNameWithoutExtension(POFile) + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";//changed by Kalpita on 27/11/2019 (include Linkid)
                        if (DownloadPO(URL, PO_Path + "\\" + Filename))
                        {
                            //LogText = Filename + " downloaded successfully.";
                            //CreateAuditFile(Filename, AuditName + "_HTTP_PO", "", "Downloaded", Filename + " downloaded successfully.", BuyerCode, SupplierCode, AuditPath);
                            //if (File.Exists(Path.GetDirectoryName(POFile) + "\\Backup\\" + Path.GetFileName(POFile))) File.Delete(Path.GetDirectoryName(POFile) + "\\Backup\\" + Path.GetFileName(POFile));
                            //File.Move(POFile, Path.GetDirectoryName(POFile) + "\\Backup\\" + Path.GetFileName(POFile));
                            //SetGUIDs(URL, "PO");

                            //added on 26/06/2019 by Kalpita
                            string cAudittxt = "";
                            LogText = cAudittxt = Filename + " downloaded successfully for " + cMsgFile;
                            File.Copy(PO_Path + "\\" + Filename, PrintScreenPath + "\\" + Path.GetFileName(Filename), true);
                            CreateAuditFile(Filename, AuditName + "_HTTP_PO", "", "Downloaded", cAudittxt, BuyerCode, SupplierCode, AuditPath);
                            if (File.Exists(Path.GetDirectoryName(POFile) + "\\Backup\\" + Path.GetFileName(POFile))) File.Delete(Path.GetDirectoryName(POFile) + "\\Backup\\" + Path.GetFileName(POFile));
                            File.Move(POFile, Path.GetDirectoryName(POFile) + "\\Backup\\" + Path.GetFileName(POFile));
                            SetGUIDs(URL, "PO");
                        }
                        else
                        {
                            //  string eFile = PrintScreenPath + AuditName + "\\+_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
                            string eFile = PrintScreenPath + "\\" + NoAuditName + "_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";//changed by kalpita on 26/06/2019
                            if (!errStr.Contains("The remote server returned an error"))
                            {
                                LogText = "Unable to download file for " + Path.GetFileName(POFile);
                                if (!PrintScreen(eFile)) eFile = "";
                                if (File.Exists(eFile))
                                    // CreateAuditFile(eFile, AuditName + "_HTTP_PO", "", "Error", "Unable to download file for " + Path.GetFileName(POFile) + ".", BuyerCode, SupplierCode, AuditPath);
                                    CreateAuditFile(eFile, AuditName + "_HTTP_PO", "", "Error", "LeS-1004:Unable to process file " + Path.GetFileName(POFile) + ".", BuyerCode, SupplierCode, AuditPath);
                            }
                            else
                            {
                                // CreateAuditFile(Path.GetFileName(POFile), AuditName + "_HTTP_PO", "", "Error", errStr, BuyerCode, SupplierCode, AuditPath); 
                                CreateAuditFile(Path.GetFileName(POFile), AuditName + "_HTTP_PO", "", "Error", "LeS-1004:Unable to process file " + Path.GetFileName(POFile) + " due to " + errStr, BuyerCode, SupplierCode, AuditPath);
                            }
                            if (File.Exists(Path.GetDirectoryName(POFile) + "\\Error\\" + Path.GetFileName(POFile))) File.Delete(Path.GetDirectoryName(POFile) + "\\Error\\" + Path.GetFileName(POFile));
                            File.Move(POFile, Path.GetDirectoryName(POFile) + "\\Error\\" + Path.GetFileName(POFile));
                        }
                    }
                    else
                    {
                        LogText = "PO for url '" + URL + "' already processed.";
                        File.Move(POFile, Path.GetDirectoryName(POFile) + "\\Error\\" + Path.GetFileName(POFile));
                    }
                }
                else
                {
                    LogText = "URL not found for navigation in file: " + Path.GetFileName(POFile) + ",Invalid file.";
                  //  CreateAuditFile(Path.GetFileName(POFile), AuditName + "_HTTP", "", "Error", "URL not found for navigation in file: " + Path.GetFileName(POFile) + ",Invalid file.", BuyerCode, SupplierCode, AuditPath);
                    CreateAuditFile(Path.GetFileName(POFile), AuditName + "_HTTP", "", "Error", "LeS-1001:Unable to find URL in file: " + Path.GetFileName(POFile), BuyerCode, SupplierCode, AuditPath);
                    if (File.Exists(MailTextFilePath + "\\Error\\" + Path.GetFileName(POFile))) File.Delete(MailTextFilePath + "\\Error\\" + Path.GetFileName(POFile));
                    File.Move(POFile, MailTextFilePath + "\\Error\\" + Path.GetFileName(POFile));
                }
            }
            catch (Exception e)
            {
                WriteErrorLog_With_Screenshot("Exception while processing Order - 119" + Path.GetFileName(POFile) + ": " + e.GetBaseException().ToString(), POFile);
            }
        }

        public bool DownloadPO(string RequestURL, string DownloadFileName, string ContentType = "Application/pdf")
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
                errStr = e.GetBaseException().Message.ToString();
                LogText = "Exception while downloading PO: " + e.GetBaseException().Message.ToString();
                _result = false;
            }
            return _result;
        }

        #endregion

        #region Quote

        //public void ProcessMTMLFiles()
        //{
        //    try
        //    {
        //        LogText = "";

        //        List<string> xmlFiles = GetXmlFiles();
        //        if (xmlFiles.Count > 0)
        //        {
        //            for (int i = xmlFiles.Count - 1; i >= 0; i--)
        //            {
        //                MTMLClass _mtml = new MTMLClass();
        //                _interchange = _mtml.Load(xmlFiles[i]);
        //                LoadInterchangeDetails();
        //                if (DocType.ToUpper() == "QUOTE")
        //                {
        //                    LogText = "Quote processing started.";
        //                    ProcessQuote(xmlFiles[i]);
        //                    LogText = "Quote processing stooped.";
        //                }
        //                else if (DocType.ToUpper() == "POC")
        //                {
        //                    LogText = "POC processing started.";
        //                }
        //                else
        //                {
        //                    if (File.Exists(QuotePath + "\\Error\\" + xmlFiles[i])) File.Delete(QuotePath + "\\Error\\" + xmlFiles[i]);
        //                    File.Move(QuotePath + "\\" + xmlFiles[i], QuotePath + "\\Error\\" + xmlFiles[i]);
        //                    throw new Exception("Invalid doctype : " + DocType + " for ref " + strUCRefNo);
        //                }
        //            }
        //        }
        //        else LogText = "No MTML file found.";
        //    }
        //    catch (Exception ex)
        //    {
        //        LogText = "Exception while processing Quote : " + ex.GetBaseException().ToString();
        //    }
        //}

        //public void ProcessQuote(string MTML_QuoteFile)
        //{
        //    try
        //    {
        //        URL = strMessageNumber.Replace("apps/quote", "exapi/AnswerInquiry");
        //        SetDownloadHeaders();
        //        if (LoadURL("", "", ""))
        //        {
        //            var _library = (RootObject)JsonConvert.DeserializeObject(_httpWrapper._CurrentResponseString, typeof(RootObject));
        //            if (_library != null)
        //            {
        //                if (_library.Result.InquiryNumber == strUCRefNo)
        //                {
        //                    if (!_library.Result.IsQuoteDownloadAvailable)
        //                    {
        //                        if (!IsDecline)
        //                        {
        //                            if (convert.ToDouble(strGrandTotal) != convert.ToDouble(0))
        //                            {
        //                                try
        //                                {
        //                                    string _itemData = SetItemDetails(_library.Result.Items, _library.Result.CostTypeCodes);
        //                                    if (_itemData != null)
        //                                    {
        //                                        if (lstExtraItems.Count > 0)
        //                                        {
        //                                            MoveFileToError(MTML_QuoteFile, "Extra item found for ref no. " + strUCRefNo, this.DocType);
        //                                        }
        //                                        else
        //                                        {
        //                                            bool _IsSubmit = false;

        //                                            if (Math.Round(itemsTotal, 0) == Math.Round(convert.ToDouble(strGrandTotal), 0)) _IsSubmit = true;
        //                                            else
        //                                            {
        //                                                itemsTotal = Math.Round(itemsTotal, 2);
        //                                                double gTotal = Math.Round(convert.ToDouble(strGrandTotal), 2);
        //                                                if (itemsTotal >= gTotal && ((itemsTotal+Convert.ToDouble(strDepositCost)+Convert.ToDouble(strFreightCharge)) - gTotal) <= 1) _IsSubmit = true;
        //                                                else if (gTotal >= itemsTotal && (gTotal - (itemsTotal+Convert.ToDouble(strDepositCost)+Convert.ToDouble(strFreightCharge))) <= 1) _IsSubmit = true;
        //                                            }
        //                                            if (_IsSubmit)
        //                                            {
        //                                                if (SubmitQuote(_library, _itemData))
        //                                                {
        //                                                    string pdfFile="Asiatic_QSuccess_"+strUCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") +".pdf";
        //                                                    DownloadRFQ(URL + "/download/quote", PrintScreenPath + "\\" + pdfFile, "");
        //                                                    LogText = "Quote for RefNo '" + strUCRefNo + "' submitted successfully.";
        //                                                    CreateAuditFile(Path.GetFileName(pdfFile), "Asiatic_HTTP_"+this.DocType, strUCRefNo, "Submitted", "Quote for RefNo '" + strUCRefNo + "' submitted successfully.",
        //                                                        BuyerCode, SupplierCode, AuditPath);
        //                                                    MoveToBackup(MTML_QuoteFile);
        //                                                }
        //                                                else
        //                                                {
        //                                                    MoveFileToError(MTML_QuoteFile, "Unable to submit quote for ref no. " + strUCRefNo, this.DocType);
        //                                                }
        //                                            }
        //                                            else { MoveFileToError(MTML_QuoteFile, "Quote total mismatch for ref no. " + strUCRefNo, this.DocType); }
        //                                        }
        //                                    }
        //                                }
        //                                catch (Exception ex)
        //                                {
        //                                    MoveFileToError(MTML_QuoteFile, "Unable to ProcessQuote() " + ex.Message, this.DocType);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                MoveFileToError(MTML_QuoteFile, "Grand total is zero for Ref  " + strUCRefNo, this.DocType);
        //                            }
        //                        }
        //                        else
        //                        {

        //                        }
        //                    }
        //                    else {
        //                        //MoveFileToError(MTML_QuoteFile, "Quote already submitted for ref no. "+strUCRefNo, this.DocType);
        //                        string eFile = "Asiatic_QError_" + strUCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
        //                        DownloadRFQ(URL + "/download/quote", PrintScreenPath + "\\" + eFile, "");
        //                        LogText = "Quote already submitted for ref no. " + strUCRefNo;
        //                        if (File.Exists(MTML_QuoteFile))
        //                        {
        //                            if (File.Exists(QuotePath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile))) File.Delete(QuotePath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
        //                            File.Move(MTML_QuoteFile, QuotePath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
        //                            if (File.Exists(QuotePath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile)))
        //                                CreateAuditFile(eFile, "Asiatic_HTTP_" + DocType, QuotePath, "Error", "Quote already submitted for ref no. " + strUCRefNo, BuyerCode, SupplierCode, AuditPath);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    MoveFileToError(MTML_QuoteFile, "Ref No. " + strUCRefNo + " mismatch with website ref " + _library.Result.InquiryNumber, this.DocType);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteErrorLog_With_Screenshot("Exception while processing Quote MTML: " + ex.GetBaseException().ToString(), Path.GetFileName(MTML_QuoteFile));
        //    }
        //}

        //public bool SubmitQuote(RootObject _library,string _itemData)
        //{
        //    bool result = false;
        //    try
        //    {
        //        if (AdditionalDiscount == "0") AdditionalDiscount = null;
        //        if (strExpDate == "")
        //        {

        //            DateTime dt = DateTime.Now.AddDays(convert.ToDouble(ConfigurationManager.AppSettings["VALIDITY_DAYS"].Trim()));
        //            strExpDate = dt.ToString("yyyy-MM-ddT00:00:00.000Z");//("d/M/yyyy");
        //            //DateTime dt1 = DateTime.MinValue;
        //            //DateTime.TryParseExact(strExpDate, "yyyy-MM-ddT00:00:00:000Z", null, DateTimeStyles.None, out dt1);
        //        }
        //        string gtCondition = "";
        //        if (_library.Result.RequestedInquiryTerms.GeneralTermsConditions == null) gtCondition = null;
        //        else gtCondition = "\"" + _library.Result.RequestedInquiryTerms.GeneralTermsConditions + "\"";

        //        string[] arrDelCoTermCode ={"EXW-Ex Works","FCA-Free Carrier","FAS-Free Alongside Ship","FOB-Free On Board","CFR-Cost And Freight","CIF-Cost Insurance Freight",
        //                                                 "DAT-Delivered At Terminal","DAP-Delivered At Place","CPT-Carriage Paid To","CIP-Carriage Insurance Paid","DAF-Delivered At Frontier","DES-Delivered Ex Ship",                         
        //                                                 "DEQ-Delivered Ex Quay","DDU-Delivered Duty Unpaid","DDP-Delivered Duty Paid"};

        //        string _delInCoTermCode = "";
        //        if (strTermCondition != "")
        //        {
        //            foreach (string _delCode in arrDelCoTermCode)
        //            {
        //                //if (gtCondition != null)
        //                //{
        //                    if (strTermCondition.Trim().ToUpper().Contains(_delCode.Split('-')[1].Trim().ToUpper()))
        //                    {
        //                        _delInCoTermCode = _delCode.Split('-')[0].Trim();
        //                        break;
        //                    }
        //               // }
        //            }
        //        }
        //        else
        //        {
        //            _delInCoTermCode = Convert.ToString(ConfigurationManager.AppSettings["OFFER_DELIVERY_TERMS"]).Trim();
        //        }

        //        string QuoteReceivedDate = "", OfferTypeIDCode = "", offerTypeName = "", offerText = "", offerDisplayName = "", DelAddress = "", ReqDelDate = "", OfferAttachmentGuid = "",
        //            DeliveryAgentID = "", DeliveryAgentContactPersonGuid = "", SparePartData = "",InquirySendDate="";
        //        if (_library.Result.QuoteReceivedDate != null)
        //        {
        //            QuoteReceivedDate = "\"" + _library.Result.QuoteReceivedDate + "\"";
        //        }
        //        else QuoteReceivedDate = null;

        //        if (_library.Result.OfferedPaymentTerms.TypeIDCode != null) OfferTypeIDCode = "\"" + _library.Result.OfferedPaymentTerms.TypeIDCode + "\"";
        //        else OfferTypeIDCode = null;

        //        if (_library.Result.OfferedPaymentTerms.TypeName != null) offerTypeName = "\"" + _library.Result.OfferedPaymentTerms.TypeName + "\"";
        //        else offerTypeName = null;

        //        if (_library.Result.OfferedPaymentTerms.Text != null) offerText = "\"" + _library.Result.OfferedPaymentTerms.Text + "\"";
        //        else offerText = null;

        //        if (_library.Result.OfferedPaymentTerms.DisplayName != null) offerDisplayName = "\"" + _library.Result.OfferedPaymentTerms.DisplayName + "\"";
        //        else offerDisplayName = null;

        //        if (_library.Result.DeliveryAddress != null) DelAddress = "\"" + _library.Result.DeliveryAddress + "\"";
        //        else DelAddress = null;

        //        if (_library.Result.RequestedDeliveryDate != null) ReqDelDate = "\"" + _library.Result.RequestedDeliveryDate + "\"";
        //        else ReqDelDate = null;

        //        if (_library.Result.OfferAttachmentGuid != null) OfferAttachmentGuid = "\"" + _library.Result.OfferAttachmentGuid + "\"";
        //        else OfferAttachmentGuid = null;

        //        if (_library.Result.DeliveryAgentID != null) DeliveryAgentID =  convert.ToString(_library.Result.DeliveryAgentID);
        //        else DeliveryAgentID = null;

        //        if (_library.Result.DeliveryAgentContactPersonGuid != null) DeliveryAgentContactPersonGuid = "\"" + _library.Result.DeliveryAgentContactPersonGuid + "\"";
        //        else DeliveryAgentContactPersonGuid = null;

        //        if (_library.Result.SparePartData != null) SparePartData = "\"" + _library.Result.SparePartData + "\"";
        //        else SparePartData = null;

        //        if (_library.Result.InquirySentDate != null) InquirySendDate = _library.Result.InquirySentDate.ToString("yyyy-MM-ddThh:mm:ss.ff0Z");

        //        string _data = "{\"InquiryGuid\":\"" + _library.Result.InquiryGuid + "\",\"QuotedPriceTotal\":" + strGrandTotal + "," +
        //            "\"QuotedDiscountPercentageTotal\":null," + _itemData + "\"InquiryNumber\":\"" + _library.Result.InquiryNumber + "\"," +
        //            "\"InquirySentDate\":\"" + InquirySendDate+"\",\"OfferValidUntil\":\"" + strExpDate + "\"," +
        //            "\"DeliveryLeadTime\":" + strLeadDays + ",\"DeliveryLeadTimeUnit\":\"d\",\"Remarks\":\"" + _library.Result.Remarks + "\"," +
        //           "\"RequestedInquiryTerms\":{\"ID\":" + _library.Result.RequestedInquiryTerms.ID + ",\"CurrencyCode\":\"" + _library.Result.RequestedInquiryTerms.CurrencyCode + "\"," +
        //           "\"PaymentTermText\":\"" + _library.Result.RequestedInquiryTerms.PaymentTermText + "\",\"DeliveryInCoTerm\":{\"IDCode\":\"" +
        //           _library.Result.RequestedInquiryTerms.DeliveryInCoTerm.IDCode+"\",\"Description\":\"" + _library.Result.RequestedInquiryTerms.DeliveryInCoTerm.Description + "\"," +
        //           "\"DisplayName\":\"" + _library.Result.RequestedInquiryTerms.DeliveryInCoTerm.DisplayName + "\"},\"DeliveryInCoTermCode\":\"" + _library.Result.RequestedInquiryTerms.DeliveryInCoTermCode + "\"," +
        //           "\"PaymentTargetDays\":" + _library.Result.RequestedInquiryTerms.PaymentTargetDays;

        //        if (_library.Result.RequestedInquiryTerms.DeliveryLeadTime != null)
        //            _data += ",\"DeliveryLeadTime\":" + convert.ToString(_library.Result.RequestedInquiryTerms.DeliveryLeadTime).Replace('(', ' ').Replace(')', ' ').Trim() + ",";
        //        else _data += ",\"DeliveryLeadTime\":null,";

        //         _data += "\"DeliveryLeadTimeUnit\":\"d\",";

        //         if (gtCondition != null)
        //             _data += "\"GeneralTermsConditions\":" + gtCondition + "},";
        //         else _data += "\"GeneralTermsConditions\":null},";

        //            _data += "\"OfferedInquiryTerms\":{\"ID\":" + _library.Result.OfferedInquiryTerms.ID + ",\"CurrencyCode\":\"" + strCurrency + "\"," +
        //           "\"PaymentTermText\":\"" + _library.Result.OfferedInquiryTerms.PaymentTermText + "\",\"DeliveryInCoTerm\":{\"IDCode\":\"" + _library.Result.OfferedInquiryTerms.DeliveryInCoTerm.IDCode + "\"," +
        //           "\"Description\":\"" + _library.Result.OfferedInquiryTerms.DeliveryInCoTerm.Description + "\",\"DisplayName\":\"" + _library.Result.OfferedInquiryTerms.DeliveryInCoTerm.DisplayName + "\"}," +
        //           "\"DeliveryInCoTermCode\":\"" + _delInCoTermCode + "\",\"PaymentTargetDays\":" + _library.Result.OfferedInquiryTerms.PaymentTargetDays;

        //        if (_library.Result.RequestedInquiryTerms.DeliveryLeadTime != null)
        //         _data+=   ",\"DeliveryLeadTime\":" + _library.Result.OfferedInquiryTerms.DeliveryLeadTime + ",";
        //       else _data += ",\"DeliveryLeadTime\":null,";

        //         _data+=  "\"DeliveryLeadTimeUnit\":\"d\",\"GeneralTermsConditions\":\"" + strTermCondition + "\"},";

        //        if(QuoteReceivedDate!=null)
        //            _data += "\"QuoteReceivedDate\":" + QuoteReceivedDate;
        //        else _data += "\"QuoteReceivedDate\":null";

        //        string _billAddres = _library.Result.BillingAddress.Replace("\r\n", " ");

        //        //if(IsOrderItemGuid)
        //        //_data += ",\"StatusCode\":\"InquirySent\"";
        //        //else    _data += ",\"StatusCode\":\"PurchaseInquirySent\"";
        //        _data += ",\"StatusCode\":\"" + _library.Result.StatusCode + "\"";
        //        _data += ",\"OfferReferenceNumber\":\"" + strAAGRefNo + "\",\"ResponsibleUserName\":\"" + _library.Result.ResponsibleUserName + "\",\"DeliveryTo\":\"" + _library.Result.DeliveryTo + "\",\"VesselName\":\"" + _library.Result.VesselName + "\",\"BillingAddress\":" +
        //               "\"" + _billAddres + "\",\"SupplierRemark\":\"" + strSupplierComment.Replace("\r\n", " ").Replace("\n\n", " ").Replace("\n"," ") + "\",\"SparePartQualityLevel\":\"" + _library.Result.SparePartQualityLevel + "\",\"OfferedPaymentTerms\":{\"ID\":1";// + _library.Result.OfferedPaymentTerms.ID;

        //        if (OfferTypeIDCode != null)
        //            _data += ",\"TypeIDCode\":" + OfferTypeIDCode;
        //        else _data += ",\"TypeIDCode\":null";

        //        if (offerTypeName != null)
        //            _data += ",\"TypeName\":" + offerTypeName;
        //        else _data += ",\"TypeName\":null";

        //        if (offerText != null)
        //            _data += ",\"Text\":" + offerText;
        //        else _data += ",\"Text\":null";

        //        _data += ",\"IsDefault\":" + _library.Result.OfferedPaymentTerms.IsDefault.ToString().ToLower() + ",\"PaymentTargetDays\":" + _library.Result.OfferedPaymentTerms.PaymentTargetDays;
        //        if(offerDisplayName!=null)
        //         _data+=   ",\"DisplayName\":" + offerDisplayName + "},";
        //        else _data += ",\"DisplayName\":null},";

        //        _data += "\"CanUserWrite\":" + _library.Result.CanUserWrite.ToString().ToLower() + ",\"ReplyRequestedUntil\":\"" + _library.Result.ReplyRequestedUntil.ToString("yyyy-MM-ddTHH:mm:ss.ff0Z");
        //        if (DelAddress != null)
        //            _data += "\",\"DeliveryAddress\":" + DelAddress+",";
        //        else _data += "\",\"DeliveryAddress\":null,";

        //        _data += "\"IsOrdered\":" + _library.Result.IsOrdered.ToString().ToLower();

        //        if (ReqDelDate!=null)
        //           _data+= ",\"RequestedDeliveryDate\":" + ReqDelDate;
        //        else _data += ",\"RequestedDeliveryDate\":null";

        //        _data += ",\"RequisitionGuid\":\"" + _library.Result.RequisitionGuid + "\",\"Attachments\":[],";

        //        if (OfferAttachmentGuid != null)
        //           _data+= "\"OfferAttachmentGuid\":" + OfferAttachmentGuid;
        //        else _data += "\"OfferAttachmentGuid\":null";

        //               _data+= ",\"BillingCompany\":\"" + _library.Result.BillingCompany + "\",\"CostTypeCodes\":[";

        //        if (_library.Result.CostTypeCodes.Count > 0)
        //        {
        //            foreach (CostTypeCode _cost in _library.Result.CostTypeCodes)
        //            {
        //                _data += "{\"IDCode\":\"" + _cost.IDCode + "\",\"Name\":\"" + _cost.Name + "\"},";
        //            }
        //            _data = _data.TrimEnd(',');
        //            _data += "],";
        //        }
        //        _data += "\"UseQuotedPriceTotal\":" + _library.Result.UseQuotedPriceTotal.ToString().ToLower();

        //        if (DeliveryAgentID != null)
        //            _data += ",\"DeliveryAgentID\":" + DeliveryAgentID;
        //        else _data += ",\"DeliveryAgentID\":null";

        //        _data += ",\"DeliveryAgentText\":\"" + _library.Result.DeliveryAgentText.Replace("\n\n"," ").Replace("\n","\\n") + "\",";

        //        if (DeliveryAgentContactPersonGuid != null)
        //            _data += "\"DeliveryAgentContactPersonGuid\":" + DeliveryAgentContactPersonGuid;
        //        else _data += "\"DeliveryAgentContactPersonGuid\":null";

        //        _data += ",\"IsQuoteDownloadAvailable\":" + _library.Result.IsQuoteDownloadAvailable.ToString().ToLower();

        //        if (SparePartData != null)
        //            _data += ",\"SparePartData\":" + SparePartData;
        //        else _data += ",\"SparePartData\":null";

        //        _data += ",\"AdditionalTerms\":[],";

        //        if (_library.Result.DeliveryETA != null)
        //            _data += "\"DeliveryETA\":\"" + Convert.ToDateTime(_library.Result.DeliveryETA).ToString("yyyy-MM-ddTHH:mm:ss.ff0Z") + "\"";
        //        else
        //            _data += "\"DeliveryETA\":null";

        //        if (_library.Result.DeliveryETD != null)
        //            _data += ",\"DeliveryETD\":\"" + Convert.ToDateTime(_library.Result.DeliveryETD).ToString("yyyy-MM-ddTHH:mm:ss.ff0Z") + "\"";
        //        else _data += ",\"DeliveryETD\":null";

        //        _data += ",\"IsDirectToVessel\":" + _library.Result.IsDirectToVessel.ToString().ToLower();

        //        if (!IsOrderItemGuid)
        //        {
        //            _data += ",\"IsRejected\":" + _library.Result.IsRejected.ToString().ToLower() + ",\"IsCancelled\":"+_library.Result.IsCancelled.ToString().ToLower();
        //        }
        //            _data+= "}";

        //        URL = URL.Replace("apps/quote", "exapi/AnswerInquiry");
        //        SetDownloadHeaders();
        //       if(_httpWrapper.PostURL(URL,_data,"","",""))
        //        {
        //             _library = (RootObject)JsonConvert.DeserializeObject(_httpWrapper._CurrentResponseString, typeof(RootObject));
        //             if (_library != null)
        //             {
        //                 if (_library.Result.IsQuoteDownloadAvailable) result = true;
        //                 else result = false;
        //             }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogText = "Exception at SubmitQuote(): " + ex.Message.ToString();
        //        throw ex;
        //    }
        //    return result;
        //}

        //public string SetItemDetails(List<Item> Items, List<CostTypeCode> costtypecode)
        //{
        //    string _itemData = ""; 
        //    if (Items.Count > 0)
        //    {
        //        _itemData = "\"Items\":[";
        //        foreach (Item _item in Items)
        //        {
        //            LineItem _itm = null;
        //            foreach (LineItem _litem in _lineitem)
        //            {
        //                string ItemID = "";
        //                if (_item.OrderItemGuid != null) ItemID = _item.OrderItemGuid;
        //                else ItemID = _item.InquiryItemGuid;

        //                if (_litem.OriginatingSystemRef == ItemID)
        //                {
        //                    _itm = _litem;
        //                    break;
        //                }
        //            }
        //            if (_itm != null)
        //            {
        //                if (_itm.IsExtraItem == 0)
        //                {
        //                    string _price = "", _discount = "";
        //                    foreach (PriceDetails _priceDetails in _itm.PriceList)
        //                    {
        //                        if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.GRP) _price = _priceDetails.Value.ToString("0.00");
        //                        else if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.DPR) _discount = _priceDetails.Value.ToString("0.00");
        //                    }

        //                    string supplierRemarks = "";
        //                    if (_itm.LineItemComment.Value != "") supplierRemarks = _itm.LineItemComment.Value;
        //                    else supplierRemarks = null;

        //                    bool isVariance = false; string statusClass = "success";
        //                    if (_item.VarianceQuantity != _itm.Quantity || _item.UnitCode != _itm.MeasureUnitQualifier || _item.VarianceName != _itm.Description)
        //                    {
        //                        isVariance = true; statusClass = "warning";
        //                    }
        //                    if (_item.OrderItemGuid != null)
        //                    { _itemData += "{\"OrderItemGuid\":\"" + _item.OrderItemGuid + "\""; IsOrderItemGuid = true; }
        //                    else if (_item.InquiryItemGuid != null)
        //                    { _itemData += "{\"InquiryItemGuid\":\"" + _item.InquiryItemGuid + "\""; IsOrderItemGuid = false; }

        //                    _itemData += ",\"ItemNumber\":\"" + _item.ItemNumber + "\",\"PositionSum\":" + Math.Round(_itm.MonetaryAmount,2) +
        //                        ",\"Quantity\":" + _item.Quantity + ",\"VarianceQuantity\":" + _itm.Quantity + ",\"Unit\":\"" + _item.Unit + "\",\"VarianceUnit\":\"" + _itm.MeasureUnitQualifier +
        //                        "\",\"Name\":\"" + _item.Name + "\",\"Description\":\"" + _item.Description.Replace("\n"," ") + "\",\"VarianceName\":\"" + _itm.Description + "\",\"IsAvailable\":" +convert.ToString( _item.IsAvailable).ToLower() + ",\"HasVariance\":" + convert.ToString(_item.HasVariance).ToLower() + "," +
        //                        "\"UnitCode\":\"" + _item.UnitCode + "\",\"CatalogName\":\"" + _item.CatalogName + "\",\"LineNumber\":" + _item.LineNumber + ",\"DeliveryLeadTime\":" + _itm.DeleiveryTime + "," +
        //                        "\"DeliveryLeadTimeUnit\":\"" + _item.DeliveryLeadTimeUnit+"\"";
        //                    if(supplierRemarks!=null)
        //                    _itemData += ",\"SupplierQuoteRemark\":\"" + supplierRemarks+"\""; 
        //                    else
        //                        _itemData += ",\"SupplierQuoteRemark\":null"; 
        //                    _itemData+=",\"CostTypeCode\":null," +
        //                          "\"IsAddedBySupplier\":" + convert.ToString(_item.IsAddedBySupplier).ToLower() + ",\"isAutofilled\":false,\"confirmedAutofill\":false,\"isVariance\":" + convert.ToString(isVariance).ToLower() + ",\"isRejected\":false,\"statusClass\":\"" + statusClass + "\"" +
        //                      ",\"positionSumString\":\"" + Math.Round(_itm.MonetaryAmount,2) + "\",\"pricePerUnit\":" + convert.ToDouble(_price) + ",\"pricePerUnitString\":\"" + _price + "\",\"varianceQuantityString\":\"" + _itm.Quantity + "\",\"deliveryLeadTimeValid\":true," +
        //                      "\"deliveryLeadTimeString\":\"" + _itm.DeleiveryTime + "\",\"isValid\":true},";

        //                    itemsTotal += _itm.MonetaryAmount;
        //                }
        //                else lstExtraItems.Add(_itm);
        //            }
        //            else
        //            {
        //                throw new Exception("Item not found on site or in mtml with id " + _item.OrderItemGuid + " for refNo " + strUCRefNo);
        //            }
        //        }

        //    }
        //    else throw new Exception("No item found on webpage");

        //    if (costtypecode.Count > 0)
        //    {
        //        int i = 0; Nullable<double> positionsum = null;
        //        string positionsumstring = null;

             

               
        //        foreach (CostTypeCode _cost in costtypecode) 
        //        {
        //            if (_cost.IDCode != "Tax")
        //            {
        //                if (_cost.IDCode == "Discount") { if (Convert.ToDouble(AdditionalDiscount) > 0) { positionsum = Convert.ToDouble(AdditionalDiscount); positionsumstring = "\"" + convert.ToString(AdditionalDiscount) + "\""; } }
        //                else if (_cost.IDCode == "Freight") { if (strTransportModeCode != "Air") { if (Convert.ToDouble(strFreightCharge) > 0) { positionsum = Convert.ToDouble(strFreightCharge); positionsumstring = "\"" + strFreightCharge + "\""; } } }
        //                else if (_cost.IDCode == "AirFreight") { if (strTransportModeCode == "Air") { if (Convert.ToDouble(strFreightCharge) > 0) { positionsum = Convert.ToDouble(strFreightCharge); positionsumstring = "\"" + strFreightCharge + "\""; } } }
        //                else if (_cost.IDCode == "Packing") { if (Convert.ToDouble(strPackingCost) > 0) { positionsum = Convert.ToDouble(strPackingCost); positionsumstring = "\"" + strPackingCost + "\""; } }

        //                if (IsOrderItemGuid)
        //                _itemData += "{\"OrderItemGuid\":\"new-item-" + i + "\"";
        //                else
        //                    _itemData += "{\"InquiryItemGuid\":\"new-item-" + i + "\"";

        //                if (positionsum != null)
        //                    _itemData += ",\"PositionSum\":" + positionsum;
        //                else _itemData += ",\"PositionSum\":null";
        //                _itemData += ",\"VarianceQuantity\":null,\"VarianceUnit\":\"PCE\",\"Description\":\"\",\"VarianceName\":\"" + _cost.Name + "\",\"IsAvailable\":true," +
        //                       "\"DeliveryLeadTime\":null,\"DeliveryLeadTimeUnit\":\"d\",\"SupplierQuoteRemark\":\"\",\"CostTypeCode\":\"" + _cost.IDCode + "\",\"IsAddedBySupplier\":true,\"isAutofilled\":false,\"confirmedAutofill\":false," +
        //                       "\"isVariance\":true,\"isRejected\":false,\"statusClass\":\"warning\"";
        //                if (positionsumstring != null)
        //                    _itemData += ",\"positionSumString\":" + positionsumstring;
        //                else _itemData += ",\"positionSumString\":null";
        //                _itemData += ",\"pricePerUnit\":null,\"pricePerUnitString\":null,\"varianceQuantityString\":null," +
        //                             "\"deliveryLeadTimeValid\":true,\"deliveryLeadTimeString\":null,\"isValid\":true},";
        //            }
        //            else
        //            {
        //                if (IsOrderItemGuid)
        //                    _itemData += "{\"OrderItemGuid\":\"new-item-" + i + "\"";
        //                else
        //                    _itemData += "{\"InquiryItemGuid\":\"new-item-" + i + "\"";

        //                _itemData += ",\"PositionSum\":null,\"VarianceQuantity\":null,\"VarianceUnit\":\"PCE\",\"Description\":\"\",\"VarianceName\":\"" + _cost.Name + "\",\"IsAvailable\":true," +
        //                       "\"DeliveryLeadTime\":null,\"DeliveryLeadTimeUnit\":\"d\",\"SupplierQuoteRemark\":\"\",\"CostTypeCode\":\"" + _cost.IDCode + "\",\"IsAddedBySupplier\":true,\"isAutofilled\":false,\"confirmedAutofill\":false," +
        //                       "\"isVariance\":true,\"isRejected\":false,\"statusClass\":\"warning\",\"positionSumString\":null,\"pricePerUnit\":null,\"pricePerUnitString\":null,\"varianceQuantityString\":null," +
        //                     "\"deliveryLeadTimeValid\":true,\"deliveryLeadTimeString\":null,\"isValid\":true},";
        //            }
        //                i++;
        //                positionsum = null; positionsumstring = null;
        //        }

        //        if (strDepositCost != "")
        //        {
        //            if (Convert.ToDouble(strDepositCost) > 0)
        //            {
        //                Nullable<double> priceperunit = null; string priceperunitstring = null;
        //                positionsum = Convert.ToDouble(strDepositCost); positionsumstring = "\"" + convert.ToString(strDepositCost) + "\"";
        //                priceperunit = Convert.ToDouble(strDepositCost); priceperunitstring = "\"" + convert.ToString(strDepositCost) + "\"";

        //                if (IsOrderItemGuid)
        //                    _itemData += "{\"OrderItemGuid\":\"new-item-" + i + "\"";
        //                else
        //                    _itemData += "{\"InquiryItemGuid\":\"new-item-" + i + "\"";

        //                _itemData += ",\"PositionSum\":" + positionsum + ",\"VarianceQuantity\":1,\"VarianceUnit\":\"PCE\",\"Description\":\"\",\"VarianceName\":\"Deposit Cost\",\"IsAvailable\":true," +
        //                     "\"DeliveryLeadTime\":" + strLeadDays + ",\"DeliveryLeadTimeUnit\":\"d\",\"SupplierQuoteRemark\":\"\",\"CostTypeCode\":null,\"IsAddedBySupplier\":true,\"isAutofilled\":false,\"confirmedAutofill\":false," +
        //                     "\"isVariance\":true,\"isRejected\":false,\"statusClass\":\"warning\",\"positionSumString\":" + positionsumstring + ",\"pricePerUnit\":" + priceperunit + ",\"pricePerUnitString\"" + priceperunitstring + ",\"varianceQuantityString\":\"1\"," +
        //                     "\"deliveryLeadTimeValid\":true,\"deliveryLeadTimeString\":\"" + strLeadDays + "\",\"isValid\":true},";
        //            }
        //        }

        //        if (strOtherCost != "")
        //        {
        //            if (Convert.ToDouble(strOtherCost) > 0)
        //            {
        //                Nullable<double> priceperunit = null; string priceperunitstring = null;
        //                positionsum = Convert.ToDouble(strOtherCost); positionsumstring = "\"" + convert.ToString(strOtherCost) + "\"";
        //                priceperunit = Convert.ToDouble(strOtherCost); priceperunitstring = "\"" + convert.ToString(strOtherCost) + "\"";

        //                if (IsOrderItemGuid)
        //                    _itemData += "{\"OrderItemGuid\":\"new-item-" + i + "\"";
        //                else
        //                    _itemData += "{\"InquiryItemGuid\":\"new-item-" + i + "\"";

        //                _itemData += ",\"PositionSum\":" + positionsum + ",\"VarianceQuantity\":null,\"VarianceUnit\":\"PCE\",\"Description\":\"\",\"VarianceName\":\"Other Cost\",\"IsAvailable\":true," +
        //                     "\"DeliveryLeadTime\":" + strLeadDays + ",\"DeliveryLeadTimeUnit\":\"d\",\"SupplierQuoteRemark\":\"\",\"CostTypeCode\":null,\"IsAddedBySupplier\":true,\"isAutofilled\":false,\"confirmedAutofill\":false," +
        //                     "\"isVariance\":true,\"isRejected\":false,\"statusClass\":\"warning\",\"positionSumString\":" + positionsumstring + ",\"pricePerUnit\":" + priceperunit + ",\"pricePerUnitString\"" + priceperunitstring + ",\"varianceQuantityString\":\"1\"," +
        //                     "\"deliveryLeadTimeValid\":true,\"deliveryLeadTimeString\":\"" + strLeadDays + "\",\"isValid\":true},";
        //            }
        //        }
        //        _itemData = _itemData.TrimEnd(',');
        //         _itemData += "],";
        //    }
        //    return _itemData;
        //}

        //public List<string> GetXmlFiles()
        //{
        //    List<string> xmlFiles = new List<string>();
        //    xmlFiles.Clear();
        //    DirectoryInfo _dir = new DirectoryInfo(QuotePath);
        //    FileInfo[] _Files = _dir.GetFiles();
        //    foreach (FileInfo _MtmlFile in _Files)
        //    {
        //        xmlFiles.Add(_MtmlFile.FullName);
        //    }
        //    return xmlFiles;
        //    //Quote_mtmlFiles.Clear();
        //    //POC_mtmlFiles.Clear();
        //    //DirectoryInfo _dir = new DirectoryInfo(QuotePath);
        //    //FileInfo[] _Files = _dir.GetFiles();
        //    //foreach (FileInfo _MtmlFile in _Files)
        //    //{
        //    //    MTMLClass _mtml = new MTMLClass();
        //    //    _interchange = _mtml.Load(_MtmlFile.FullName);
        //    //    LoadInterchangeDetails();
        //    //    if (DocType.ToUpper() == "QUOTE")
        //    //        Quote_mtmlFiles.Add(_MtmlFile.FullName);
        //    //    else if (DocType.ToUpper() == "ORDERRESPONSE")
        //    //        POC_mtmlFiles.Add(_MtmlFile.FullName);
        //    //    else
        //    //    {
        //    //        MoveFileToError(_MtmlFile.FullName, "Invalid doctype : " + DocType + " for ref " + strUCRefNo, "Error");
        //    //        throw new Exception("Invalid doctype : " + DocType + " for ref " + strUCRefNo);
        //    //    }

        //    //}
        //}

        //public void LoadInterchangeDetails()
        //{
        //    try
        //    {
        //        LogText = "Started Loading interchange object.";
        //        if (_interchange != null)
        //        {
        //            if (_interchange.Recipient != null)
        //                strBuyerCode = _interchange.Recipient;

        //            if (_interchange.Sender != null)
        //                strSupplierCode = _interchange.Sender;

        //            if (_interchange.DocumentHeader.DocType != null)
        //                DocType = _interchange.DocumentHeader.DocType;

        //            if (_interchange.DocumentHeader != null)
        //            {
        //                if (_interchange.DocumentHeader.IsDeclined)
        //                    IsDecline = _interchange.DocumentHeader.IsDeclined;

        //                if (_interchange.DocumentHeader.MessageNumber != null)
        //                    strMessageNumber = _interchange.DocumentHeader.MessageNumber;

        //                if (_interchange.DocumentHeader.LeadTimeDays != null)
        //                    strLeadDays = _interchange.DocumentHeader.LeadTimeDays;

        //                if (_interchange.DocumentHeader.TransportModeCode != null)
        //                    strTransportModeCode = convert.ToString(_interchange.DocumentHeader.TransportModeCode);
                        

        //                strCurrency = _interchange.DocumentHeader.CurrencyCode;

        //                strMsgNumber = _interchange.DocumentHeader.MessageNumber;
        //                strMsgRefNumber = _interchange.DocumentHeader.MessageReferenceNumber;

        //                if (_interchange.DocumentHeader.IsAltItemAllowed != null) IsAltItemAllowed = Convert.ToInt32(_interchange.DocumentHeader.IsAltItemAllowed);
        //                if (_interchange.DocumentHeader.IsPriceAveraged != null) IsPriceAveraged = Convert.ToInt32(_interchange.DocumentHeader.IsPriceAveraged);
        //                if (_interchange.DocumentHeader.IsUOMChanged != null) IsUOMChanged = Convert.ToInt32(_interchange.DocumentHeader.IsUOMChanged);
        //                if (_interchange.DocumentHeader.AdditionalDiscount != null) AdditionalDiscount = convert.ToString(_interchange.DocumentHeader.AdditionalDiscount);


        //                for (int i = 0; i < _interchange.DocumentHeader.References.Count; i++)
        //                {
        //                    if (_interchange.DocumentHeader.References[i].Qualifier == ReferenceQualifier.UC)
        //                        strUCRefNo = _interchange.DocumentHeader.References[i].ReferenceNumber.Trim();
        //                    else if (_interchange.DocumentHeader.References[i].Qualifier == ReferenceQualifier.AAG)
        //                        strAAGRefNo = _interchange.DocumentHeader.References[i].ReferenceNumber.Trim();
        //                }
        //            }
        //            if (_interchange.BuyerSuppInfo != null)
        //            {
        //                strLesRecordID = Convert.ToString(_interchange.BuyerSuppInfo.RecordID);
        //            }

        //            #region read interchange party addresses

        //            for (int j = 0; j < _interchange.DocumentHeader.PartyAddresses.Count; j++)
        //            {
        //                if (_interchange.DocumentHeader.PartyAddresses[j].Qualifier == PartyQualifier.BY)
        //                {
        //                    BuyerName = _interchange.DocumentHeader.PartyAddresses[j].Name;
        //                    if (_interchange.DocumentHeader.PartyAddresses[j].Contacts != null)
        //                    {
        //                        if (_interchange.DocumentHeader.PartyAddresses[j].Contacts.Count > 0)
        //                        {
        //                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count > 0)
        //                            {
        //                                for (int k = 0; k < _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count; k++)
        //                                {
        //                                    if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.TE)
        //                                        strBuyerPhone = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
        //                                    if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.EM)
        //                                        strBuyerEmail = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
        //                                    if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.FX)
        //                                        strBuyerFax = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //                else if (_interchange.DocumentHeader.PartyAddresses[j].Qualifier == PartyQualifier.VN)
        //                {
        //                    strSupplierName = _interchange.DocumentHeader.PartyAddresses[j].Name;
        //                    if (_interchange.DocumentHeader.PartyAddresses[j].Contacts != null)
        //                    {
        //                        if (_interchange.DocumentHeader.PartyAddresses[j].Contacts.Count > 0)
        //                        {
        //                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count > 0)
        //                            {
        //                                for (int k = 0; k < _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count; k++)
        //                                {
        //                                    if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.TE)
        //                                        strSupplierPhone = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
        //                                    if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.EM)
        //                                        strSupplierEmail = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
        //                                    if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.FX)
        //                                        strSupplierFax = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //                else if (_interchange.DocumentHeader.PartyAddresses[j].Qualifier == PartyQualifier.UD)
        //                {
        //                    strVesselName = _interchange.DocumentHeader.PartyAddresses[j].Name;
        //                    if (_interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Berth != "")
        //                        strPortName = _interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Berth;

        //                    if (_interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Port != null)
        //                        strPortCode = _interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Port;
        //                }
        //            }

        //            #endregion

        //            #region read comments

        //            if (_interchange.DocumentHeader.Comments != null)
        //            {
        //                for (int i = 0; i < _interchange.DocumentHeader.Comments.Count; i++)
        //                {
        //                    if (_interchange.DocumentHeader.Comments[i].Qualifier == CommentTypes.SUR)
        //                        strSupplierComment = _interchange.DocumentHeader.Comments[i].Value;
        //                    else if (_interchange.DocumentHeader.Comments[i].Qualifier == CommentTypes.ZTP)
        //                        strPayTerms = _interchange.DocumentHeader.Comments[i].Value;
        //                    else if (_interchange.DocumentHeader.Comments[i].Qualifier == CommentTypes.ZTC)
        //                        strTermCondition = _interchange.DocumentHeader.Comments[i].Value;
        //                }
        //            }

        //            #endregion

        //            #region read Line Items

        //            if (_interchange.DocumentHeader.LineItemCount > 0)
        //            {
        //                _lineitem = _interchange.DocumentHeader.LineItems;
        //            }

        //            #endregion

        //            #region read Interchange Monetory Amount

        //            if (_interchange.DocumentHeader.MonetoryAmounts != null)
        //            {
        //                for (int i = 0; i < _interchange.DocumentHeader.MonetoryAmounts.Count; i++)
        //                {
        //                    if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.PackingCost_106)
        //                        strPackingCost = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
        //                    else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.FreightCharge_64)
        //                        strFreightCharge = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
        //                    else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.TotalLineItemsAmount_79)
        //                        strTotalLineItemsAmount = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
        //                    else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.AllowanceAmount_204)
        //                        strAllowance = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
        //                    else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.GrandTotal_259)
        //                        strGrandTotal = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
        //                    else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.BuyerItemTotal_90)//16-12-2017
        //                        strBuyerTotal = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
        //                    else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.Deposit_97)//16-2-2018
        //                        strDepositCost = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
        //                    else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.TaxCost_99)
        //                        strFreightCharge = Convert.ToString(Convert.ToDouble(strFreightCharge)+Convert.ToDouble(_interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString()));
        //                    else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.OtherCost_98)
        //                        strOtherCost = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();//
        //                }
        //            }

        //            #endregion

        //            #region read date time period

        //            if (_interchange.DocumentHeader.DateTimePeriods != null)
        //            {
        //                for (int i = 0; i < _interchange.DocumentHeader.DateTimePeriods.Count; i++)
        //                {
        //                    if (_interchange.DocumentHeader.DateTimePeriods[i].Qualifier == DateTimePeroidQualifiers.DocumentDate_137)
        //                    {
        //                        if (_interchange.DocumentHeader.DateTimePeriods[i].Value != null)
        //                        { DateTime dtDocDate = FormatMTMLDate(_interchange.DocumentHeader.DateTimePeriods[i].Value); }
        //                    }

        //                    else if (_interchange.DocumentHeader.DateTimePeriods[i].Qualifier == DateTimePeroidQualifiers.DeliveryDate_69)
        //                    {
        //                        if (_interchange.DocumentHeader.DateTimePeriods[i].Value != null)
        //                        {
        //                            DateTime dtDelDate = FormatMTMLDate(_interchange.DocumentHeader.DateTimePeriods[i].Value);
        //                            if (strLeadDays == "0")
        //                            {
        //                                TimeSpan t = dtDelDate.Subtract(DateTime.Today);
        //                                double totalDays = t.TotalDays;
        //                                strLeadDays = convert.ToString(totalDays);
        //                            }
        //                            //if (dtDelDate != DateTime.MinValue)
        //                            //{
        //                            //    strDelvDate = dtDelDate.ToString("MM/dd/yyyy");
        //                            //}
        //                            //if (dtDelDate == null)
        //                            //{
        //                            //    DateTime dt = FormatMTMLDate(DateTime.Now.AddDays(Convert.ToDouble(strLeadDays)).ToString());
        //                            //    if (dt != DateTime.MinValue)
        //                            //    {
        //                            //        strDelvDate = dt.ToString("MM/dd/yyyy");
        //                            //    }
        //                            //}
        //                        }
        //                    }

        //                    if (_interchange.DocumentHeader.DateTimePeriods[i].Qualifier == DateTimePeroidQualifiers.ExpiryDate_36)
        //                    {
        //                        if (_interchange.DocumentHeader.DateTimePeriods[i].Value != null)
        //                        {
        //                            DateTime ExpDate = FormatMTMLDate(_interchange.DocumentHeader.DateTimePeriods[i].Value);
        //                            if (ExpDate != DateTime.MinValue)
        //                            {
        //                                //strExpDate = ExpDate.ToString("dd-MM-yyyy");
        //                                strExpDate = ExpDate.ToString("yyyy-MM-ddT00:00:00:000Z");
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            #endregion
        //            LogText = "stopped Loading interchange object.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Exception on LoadInterchangeDetails : " + ex.GetBaseException().ToString());
        //    }
        //}

        //public DateTime FormatMTMLDate(string DateValue)
        //{
        //    DateTime Dt = DateTime.MinValue;
        //    if (DateValue != null && DateValue != "")
        //    {
        //        if (DateValue.Length > 5)
        //        {
        //            int year = Convert.ToInt32(DateValue.Substring(0, 4));
        //            int Month = Convert.ToInt32(DateValue.Substring(4, 2));
        //            int day = Convert.ToInt32(DateValue.Substring(6, 2));
        //            Dt = new DateTime(year, Month, day);
        //        }
        //    }
        //    return Dt;
        //}

        //public override bool DownloadRFQ(string RequestURL, string DownloadFileName, string ContentType = "Application/pdf")
        //{
        //    bool _result = false;
        //    try
        //    {
        //        URL = RequestURL;
        //        if (LoadURL("", "", "", false))
        //        {
        //            byte[] b = null;
        //            FileStream fileStream = File.OpenWrite(DownloadFileName);
        //            byte[] buffer = new byte[1024];
        //            using (Stream input = _httpWrapper._CurrentResponse.GetResponseStream())
        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                int count = 0;
        //                do
        //                {
        //                    byte[] buf = new byte[1024];
        //                    count = input.Read(buf, 0, 1024);
        //                    ms.Write(buf, 0, count);
        //                } while (input.CanRead && count > 0);
        //                b = ms.ToArray();
        //            }
        //            fileStream.Write(b, 0, b.Length);
        //            fileStream.Flush();
        //            fileStream.Close();
        //        }
        //        _result = (File.Exists(DownloadFileName));
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    return _result;
        //}

        //private void MoveToBackup(string FileToMove)
        //{
        //    try
        //    {
        //        if (File.Exists(FileToMove))
        //        {
        //            string orgFile = Path.GetFileName(FileToMove);
        //            string orgDir = Path.GetDirectoryName(FileToMove);

        //            if (!Directory.Exists(orgDir + "\\Backup")) Directory.CreateDirectory(orgDir + "\\Backup");
        //            if (File.Exists(orgDir + "\\Backup\\" + orgFile))
        //            {
        //                File.Delete(orgDir + "\\Backup\\" + orgFile);
        //            }
        //            File.Move(FileToMove, orgDir + "\\Backup\\" + orgFile);

        //            LogText = "File " + Path.GetFileName(FileToMove) + " moved to backup files.";
        //        }
        //    }
        //    catch { }
        //}

      
        #endregion

        public override bool PrintScreen(string sFileName)
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

        public void WriteErrorLog_With_Screenshot(string AuditMsg, string Filename)
        {
            // if (string.IsNullOrEmpty(AuditName)) { AuditName = NoAuditName; }//changed by kalpita on 28/06/2019(Asiatic),27/11/2019   
            string cDocType = (!string.IsNullOrEmpty(this.DocType)) ? "_" + this.DocType : "";

            LogText = AuditMsg;
            if (!AuditMsg.Contains("(404) Not Found"))
            {//AuditName
                string eFile = PrintScreenPath + "\\" + NoAuditName + "\\_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";//changed by kalpita on 01/11/2019
                if (!PrintScreen(eFile)) eFile = "";
                {
                    if (this.DocType != "PO")
                        CreateAuditFile(eFile, AuditName + "_HTTP" + cDocType, VRNO, "Error", AuditMsg, BuyerCode, SupplierCode, AuditPath);
                    else
                        CreateAuditFile(eFile, AuditName + "_HTTP" + cDocType, VRNO, "Error", AuditMsg, BuyerCode, SupplierCode, AuditPath);
                }
            }
            else
            {
                if (this.DocType != "PO")
                    CreateAuditFile(Path.GetFileName(Filename), AuditName + "_HTTP" + cDocType, VRNO, "Error", AuditMsg, BuyerCode, SupplierCode, AuditPath);
                else
                    CreateAuditFile(Path.GetFileName(Filename), AuditName + "_HTTP" + cDocType, VRNO, "Error", AuditMsg, BuyerCode, SupplierCode, AuditPath);
            }
        }

        //added by kalpita on 01/11/2019
        public void WriteErrorLog_With_Screenshot(string AuditMsg, string Filename, string ErrorNo)
        {
            // if (string.IsNullOrEmpty(AuditName)) { AuditName = NoAuditName; }//changed by kalpita on 28/06/2019(Asiatic),27/11/2019
            LogText = AuditMsg;
            string cDocType = (!string.IsNullOrEmpty(this.DocType)) ? "_" + this.DocType : "";
            if (!AuditMsg.Contains("(404) Not Found"))
            {
                string eFile = PrintScreenPath + "\\" + NoAuditName + "\\_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";//AuditName
                if (!PrintScreen(eFile)) eFile = "";
                {
                    if (this.DocType != "PO")
                        CreateAuditFile(eFile, AuditName + "_HTTP" + cDocType, VRNO, "Error", ErrorNo + AuditMsg, BuyerCode, SupplierCode, AuditPath);
                    else
                        CreateAuditFile(eFile, AuditName + "_HTTP" + cDocType, VRNO, "Error", ErrorNo + AuditMsg, BuyerCode, SupplierCode, AuditPath);
                }
            }
            else
            {
                if (this.DocType != "PO")
                    CreateAuditFile(Path.GetFileName(Filename), AuditName + "_HTTP" + cDocType, VRNO, "Error", AuditMsg, BuyerCode, SupplierCode, AuditPath);
                else
                    CreateAuditFile(Path.GetFileName(Filename), AuditName + "_HTTP" + cDocType, VRNO, "Error", AuditMsg, BuyerCode, SupplierCode, AuditPath);
            }
        }

        //Added By Anita - 2019-09-06
        //Used for removing anchor '<a></a>' tags from RFQ Url
        private string RemoveHtmlTagIfPresentInUrl(string url)
        {
            string _url = url;
            if (!string.IsNullOrWhiteSpace(_url))
            {
                if (_url.StartsWith("<a>") || _url.StartsWith("<A>"))
                    _url = _url.Substring("<a>".Length);

                if (_url.EndsWith("</a"))
                    _url = _url.Substring(0, _url.Length - "</a".Length);

                if (_url.EndsWith("Agents"))
                    _url = _url.Replace("Agents", "").Trim();

                if (_url.StartsWith("<") && _url.EndsWith(">"))
                    _url = _url.Replace(">", "").Replace("<", "").Trim();
            }
            return _url;
        }

        //public void MoveFileToError(string MTML_QuoteFile, string message, string DocType)
        //{
        //    string eFile = PrintScreenPath + "Asiatic" + DocType + "_Error_" + strUCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
        //    if (!PrintScreen(eFile)) eFile = "";
        //    LogText = message;
        //    if (File.Exists(MTML_QuoteFile))
        //    {
        //        if (File.Exists(QuotePath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile))) File.Delete(QuotePath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
        //        File.Move(MTML_QuoteFile, QuotePath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
        //        if (File.Exists(QuotePath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile)))
        //            CreateAuditFile(eFile, "Asiatic_HTTP_" + DocType, QuotePath, "Error", message, BuyerCode, SupplierCode, AuditPath);
        //    }
        //}
    }

    public class Item
    {
        public string OrderItemGuid { get; set; }
        public string ItemNumber { get; set; }
        public object PositionSum { get; set; }
        public object Quantity { get; set; }
        public double VarianceQuantity { get; set; }
        public string Unit { get; set; }
        public string VarianceUnit { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string VarianceName { get; set; }
        public bool IsAvailable { get; set; }
        public bool HasVariance { get; set; }
        public string UnitCode { get; set; }
        public string CatalogName { get; set; }
        public int LineNumber { get; set; }
        public object DeliveryLeadTime { get; set; }
        public string DeliveryLeadTimeUnit { get; set; }
        public object SupplierQuoteRemark { get; set; }
        public object CostTypeCode { get; set; }
        public bool IsAddedBySupplier { get; set; }
        public string InquiryItemGuid { get; set; }
    }

    public class DeliveryInCoTerm
    {
        public string IDCode { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
    }

    public class RequestedInquiryTerms
    {
        public int ID { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentTermText { get; set; }
        public DeliveryInCoTerm DeliveryInCoTerm { get; set; }
        public string DeliveryInCoTermCode { get; set; }
        public int PaymentTargetDays { get; set; }
        public object DeliveryLeadTime { get; set; }
        public string DeliveryLeadTimeUnit { get; set; }
        public object GeneralTermsConditions { get; set; }
    }

    public class DeliveryInCoTerm2
    {
        public string IDCode { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
    }

    public class OfferedInquiryTerms
    {
        public int ID { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentTermText { get; set; }
        public DeliveryInCoTerm2 DeliveryInCoTerm { get; set; }
        public string DeliveryInCoTermCode { get; set; }
        public int PaymentTargetDays { get; set; }
        public object DeliveryLeadTime { get; set; }
        public string DeliveryLeadTimeUnit { get; set; }
        public object GeneralTermsConditions { get; set; }
    }

    public class OfferedPaymentTerms
    {
        public int ID { get; set; }
        public object TypeIDCode { get; set; }
        public object TypeName { get; set; }
        public object Text { get; set; }
        public bool IsDefault { get; set; }
        public int PaymentTargetDays { get; set; }
        public object DisplayName { get; set; }
    }

    public class CostTypeCode
    {
        public string IDCode { get; set; }
        public string Name { get; set; }
    }

    public class Result
    {
        public string InquiryGuid { get; set; }
        public object QuotedPriceTotal { get; set; }
        public object QuotedDiscountPercentageTotal { get; set; }
        public List<Item> Items { get; set; }
        public string InquiryNumber { get; set; }
        public DateTime InquirySentDate { get; set; }
        public object OfferValidUntil { get; set; }
        public object DeliveryLeadTime { get; set; }
        public string DeliveryLeadTimeUnit { get; set; }
        public string Remarks { get; set; }
        public RequestedInquiryTerms RequestedInquiryTerms { get; set; }
        public OfferedInquiryTerms OfferedInquiryTerms { get; set; }
        public object QuoteReceivedDate { get; set; }
        public string StatusCode { get; set; }
        public object OfferReferenceNumber { get; set; }
        public string ResponsibleUserName { get; set; }
        public string DeliveryTo { get; set; }
        public string VesselName { get; set; }
        public string BillingAddress { get; set; }
        public object SupplierRemark { get; set; }
        public string SparePartQualityLevel { get; set; }
        public OfferedPaymentTerms OfferedPaymentTerms { get; set; }
        public bool CanUserWrite { get; set; }
        public DateTime ReplyRequestedUntil { get; set; }
        public object DeliveryAddress { get; set; }
        public bool IsOrdered { get; set; }
        public object RequestedDeliveryDate { get; set; }
        public string RequisitionGuid { get; set; }
        public List<object> Attachments { get; set; }
        public object OfferAttachmentGuid { get; set; }
        public string BillingCompany { get; set; }
        public List<CostTypeCode> CostTypeCodes { get; set; }
        public bool UseQuotedPriceTotal { get; set; }
        public object DeliveryAgentID { get; set; }
        public string DeliveryAgentText { get; set; }
        public object DeliveryAgentContactPersonGuid { get; set; }
        public bool IsQuoteDownloadAvailable { get; set; }
        public object SparePartData { get; set; }
        public List<object> AdditionalTerms { get; set; }
        public object DeliveryETA { get; set; }
        public object DeliveryETD { get; set; }
        public bool IsDirectToVessel { get; set; }
        public bool IsRejected { get; set; }
        public bool IsCancelled { get; set; }
    }

    public class RootObject
    {
        public object ErrorCode { get; set; }
        public bool InputError { get; set; }
        public object ResultMessage { get; set; }
        public bool Success { get; set; }
        public Result Result { get; set; }
    }
}

#region commented


//added by kalpita on 11/12/2019



//private string GetCorrectURL(RichTextBox _txtData, string[] DomainList, string emlFile, bool IsRFQ)
//{
//    string _url = "", strFilter = "";
//    foreach (string Domain in DomainList)
//    {
//        string cDomain = Domain.Replace("https://", "");
//        if (_txtData.Text.Contains(cDomain))
//        {
//            for (int i = 0; i < _txtData.Lines.Length; i++)
//            {
//                string line = _txtData.Lines[i]; strFilter = (IsRFQ) ? "/apps/quote" : "/private/exports/purchase/orders";
//                if (line.Contains(cDomain))
//                {
//                    if (line.Contains(EncyptUrls))
//                    {
//                        _url = GetURLDetails(_txtData.Text, cDomain);
//                        _url = GetURLDetails(line, cDomain);
//                    }
//                    else
//                    {
//                        if (line.Contains(Domain + strFilter))
//                        {
//                            _url = GetURLDetails(line, cDomain);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(_url)) { break; }
//                }
//            }
//        }
//    }
//    return _url;
//}


//private string GetCorrectURL(RichTextBox _txtData, string Domain, string[] DomainList)
//{
//    string _resultURL = "", _url = "",_aftrloadurl="";
//    string cDomain = Domain.Replace("https://", "");
//    if (_txtData.Text.Contains(cDomain))
//    {
//        for (int i = 0; i < _txtData.Lines.Length; i++)
//        {
//            string line = _txtData.Lines[i];
//            if (line.Contains(EncyptUrls) && line.Contains(cDomain))
//            {
//                domainIndex = Array.IndexOf(DomainList, Domain);
//                int startIndex = line.IndexOf("<https"); if (startIndex == -1) { startIndex = line.IndexOf("https"); }

//                if (line.Length > startIndex)
//                {
//                    this.URL = line.Substring(startIndex);//changed on 18/11/2019
//                    string[] testing1 = Regex.Matches(this.URL, @"\<(.+?)\>")
//                                  .Cast<Match>()
//                                  .Select(s => s.Groups[1].Value).ToArray();

//                    try
//                    {
//                        if (LoadURL("", "", "", true))
//                        {
//                            _aftrloadurl = Convert.ToString(_httpWrapper._CurrentResponse.ResponseUri);
//                        }
//                    }
//                    catch (System.Net.WebException ex)
//                    {
//                        _aftrloadurl = (ex.Response).ResponseUri.AbsoluteUri;
//                    }
//                    _url = _aftrloadurl;
//                    for (int j = i + 1; j < _txtData.Lines.Length; j++)
//                    {
//                        if (!string.IsNullOrEmpty(_txtData.Lines[j]))
//                        {
//                            _url += _txtData.Lines[j].Trim();
//                        }
//                        else { break; }
//                    }
//                }
//                if (!string.IsNullOrEmpty(_url)) { _url = _url.TrimEnd('.'); break; }
//            }
//        }
//        int nhttpCount = Regex.Matches(_url, "https://").Count;
//        if (nhttpCount > 1)
//        {
//            string[] testing = Regex.Matches(_url, @"\<(.+?)\>")
//            .Cast<Match>()
//            .Select(s => s.Groups[1].Value).ToArray();
//            _resultURL = _url.Replace(testing[0], "").Replace("<>", "");
//        }
//        else
//        {
//            _resultURL = _url;
//        }
//    }
//    else { }
//    return _resultURL;
//}

//private string GetPattern(RichTextBox txtData, int startIndex)
//{
//    string _str = "";
//    for (int i = startIndex; i < startIndex + 5; i++)
//    {
//        string line = _txtData.Lines[i];
//        bool pattn = Regex.IsMatch(line, "(([A-Za-z0-9]{3})-[a-zA-Z0-9]{12}\\.)");
//        if (pattn) { _str = line; break; }
//    }
//    return _str;
//}

//changed by kalpita on 07/11/2019









//private string GetCorrectURL(RichTextBox _txtData, string Domain, string[] DomainList)
//{
//    string _resultURL = "", appendStr = "",_url="";
//    string cDomain = Domain.Replace("https://", "");

//    int nCount = Regex.Matches(_txtData.Text, "https://").Count;
//    if (_txtData.Text.Contains(cDomain))
//    {
//        for (int i = 0; i < _txtData.Lines.Length; i++)
//        {
//            string line = _txtData.Lines[i];
//            if (line.Contains(EncyptUrls) && line.Contains(cDomain))
//            {
//                this.URL = line; domainIndex = Array.IndexOf(DomainList, Domain);
//                int startIndex = line.IndexOf("https"); if (startIndex == -1) { startIndex = line.IndexOf("http"); }
//                else
//                {
//                    if (line.Length > startIndex)
//                    {
//                        if (line.Contains("<"))
//                        {
//                            // _url = line.Substring(startIndex); 
//                            int startindx = 0;
//                            startindx = line.IndexOf("<https"); if (startindx < 0) { startindx = startIndex; }//changed by kalpita on 07/10/2019(Checking)

//                            _url = line.Substring(startindx);
//                            appendStr += GetCompleteUrl(_txtData, i);

//                            this.URL = _url.Replace(">", "").Replace("<", "");
//                            try
//                            {
//                                if (LoadURL("", "", "", true))
//                                {
//                                    _resultURL = Convert.ToString(_httpWrapper._CurrentResponse.ResponseUri);
//                                    break;
//                                }
//                            }
//                            catch (System.Net.WebException ex)
//                            {
//                                _resultURL = (ex.Response).ResponseUri.AbsoluteUri;
//                                break;
//                            }
//                        }
//                        else
//                        {
//                            this.URL = line.Substring(startIndex);//commented for checking 06/11/2019
//                            // appendStr = GetPattern(_txtData, i).TrimEnd('.');

//                            // appendStr += GetCompleteUrl(_txtData, i);//commented for checking 06/11/2019
//                            appendStr = GetCompleteUrl(_txtData, i);

//                            //if (!string.IsNullOrEmpty(appendStr))
//                            //{
//                            try
//                            {
//                                if (LoadURL("", "", "", true))
//                                {
//                                    _resultURL = Convert.ToString(_httpWrapper._CurrentResponse.ResponseUri);
//                                    break;
//                                }
//                            }
//                            catch (System.Net.WebException ex)
//                            {
//                                _resultURL = (ex.Response).ResponseUri.AbsoluteUri;
//                                //break;
//                            }
//                            //}
//                        }
//                    }
//                }
//            }
//        }

//    }
//    else { }
//    return _resultURL + appendStr;
//}


//private string GetCompleteUrl(RichTextBox txtData, int startIndex)
//{
//    string _url = "";
//    for (int i = startIndex + 1; i < _txtData.Lines.Length; i++)//
//    {
//        string line = _txtData.Lines[i]; _url += line;
//        if (line.Contains(">"))
//        {
//            int endIndex = line.IndexOf(">");
//            if (line.Length > endIndex)
//            {
//                _url = _url.Replace(">", "");
//                break;
//            }
//        }
//        //bool pattn = Regex.IsMatch(line, "(([A-Za-z0-9]{3})-[a-zA-Z0-9]{12}\.)");
//        //if (pattn) { _str = line; break; }
//    }
//    if (!_url.Contains("https")) { _url = string.Empty; }
//    return _url;
//}


//added on 26/06/2019 by Kalpita
//private string GetCorrectURL(RichTextBox _txtData, string Domain,string[] DomainList)
//{
//    string _resultURL = "",appendStr="";
//    string cDomain = Domain.Replace("https://", ""); 
//    if (_txtData.Text.Contains(cDomain))
//    {
//        for (int i = 0; i < _txtData.Text.Length; i++)
//        {
//            string line = _txtData.Lines[i];
//            if (line.Contains(EncyptUrls) && line.Contains(cDomain))
//            {
//                this.URL = line; domainIndex = Array.IndexOf(DomainList, Domain);
//                int startIndex = line.IndexOf("https"); if (startIndex == -1) { startIndex = line.IndexOf("http"); }
//                else
//                {
//                    if (line.Length > startIndex)
//                    {
//                        this.URL = line.Substring(startIndex);
//                       appendStr = GetPattern(_txtData, i).TrimEnd('.');
//                    }
//                    if (LoadURL("", "", "", true))
//                    {
//                        _resultURL = Convert.ToString(_httpWrapper._CurrentResponse.ResponseUri);
//                        break;
//                    }
//                }
//            }
//        }
//    }
//    else{ }
//    return _resultURL + appendStr;
//}

//private string GetPattern(RichTextBox txtData, int startIndex)
//{
//    string _str = "";
//    for (int i = startIndex; i < _txtData.Text.Length; i++)
//    {
//        string line = _txtData.Lines[i];
//        bool pattn = Regex.IsMatch(line, "([A-Za-z0-9]{3})-[a-zA-Z0-9]{12}");
//        if (pattn) { _str = line; break; }
//    }
//    return _str;
//}

//private string GetCorrectURL(RichTextBox _txtData, string Domain, string[] DomainList)
//{
//    string _resultURL = "", appendStr = "", _url = "";
//    string cDomain = Domain.Replace("https://", "");

//    int nCount = Regex.Matches(_txtData.Text, "https://").Count;
//    if (_txtData.Text.Contains(cDomain))
//    {
//        for (int i = 0; i < _txtData.Lines.Length; i++)
//        {
//            string line = _txtData.Lines[i];
//            if (line.Contains(EncyptUrls) && line.Contains(cDomain))
//            {
//                this.URL = line; domainIndex = Array.IndexOf(DomainList, Domain);
//                int startIndex = line.IndexOf("https"); if (startIndex == -1) { startIndex = line.IndexOf("http"); }
//                else
//                {
//                    if (line.Length > startIndex)
//                    {
//                        if (line.Contains("<"))
//                        {
//                            // _url = line.Substring(startIndex); 
//                            int startindx = 0;
//                            startindx = line.IndexOf("<https"); if (startindx < 0) { startindx = startIndex; }//changed by kalpita on 07/10/2019(Checking)

//                            _url = line.Substring(startindx);
//                            appendStr += GetCompleteUrl(_txtData, i);

//                            this.URL = _url.Replace(">", "").Replace("<", "");
//                            try
//                            {
//                                if (LoadURL("", "", "", true))
//                                {
//                                    _resultURL = Convert.ToString(_httpWrapper._CurrentResponse.ResponseUri);
//                                    break;
//                                }
//                            }
//                            catch (System.Net.WebException ex)
//                            {
//                                _resultURL = (ex.Response).ResponseUri.AbsoluteUri;
//                                break;
//                            }
//                        }
//                        else
//                        {
//                            this.URL = line.Substring(startIndex);//commented for checking 06/11/2019
//                            // appendStr = GetPattern(_txtData, i).TrimEnd('.');

//                            // appendStr += GetCompleteUrl(_txtData, i);//commented for checking 06/11/2019
//                            appendStr = GetCompleteUrl(_txtData, i);

//                            //if (!string.IsNullOrEmpty(appendStr))
//                            //{
//                            try
//                            {
//                                if (LoadURL("", "", "", true))
//                                {
//                                    _resultURL = Convert.ToString(_httpWrapper._CurrentResponse.ResponseUri);
//                                    break;
//                                }
//                            }
//                            catch (System.Net.WebException ex)
//                            {
//                                _resultURL = (ex.Response).ResponseUri.AbsoluteUri;
//                                //break;
//                            }
//                            //}
//                        }
//                    }
//                }
//            }
//        }

//    }
//    else { }
//    return _resultURL + appendStr;
//}


        //private string GetCompleteUrl_(RichTextBox txtData, int startIndex)
        //{
        //    string _url = ""; int urlindx = 0;
        //    for (int i = startIndex + 1; i < _txtData.Lines.Length; i++)//
        //    {
        //        string line = _txtData.Lines[i];
        //        int startindx = line.IndexOf("<https"); int endindex = line.IndexOf(">");
        //        if (startindx > -1)
        //        {
        //            urlindx = i;
        //        }
        //        else
        //        {
        //            if (i > urlindx)
        //            {
        //                _url += line.Trim();
        //                if (endindex > 0 && line.Length > endindex)
        //                {
        //                    _url = _url.Replace(">", "");
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    return _url;
        //}


#region commented on 07/10/2019 by Kalpita
//private string GetURL(string emlFile)
//{
//    string _url = "",ResultURL="";bool IsRFQ=false;
//    try
//    {
//        LogText = "";
//        LogText = "Reading file " + Path.GetFileName(emlFile);
//        _txtData.Text = File.ReadAllText(emlFile);

//        if (_txtData.Lines[0].ToLower().Contains("request for quotation") || _txtData.Lines[0].ToLower().Contains("inquiry") || _txtData.Lines[0].ToLower().Contains("rfq no"))
//        {
//            IsRFQ = true;
//        }
//        foreach (string _domain in RFQDomains)
//        {
//            if (_domain != "")
//            {
//                if (_txtData.Text.Contains(_domain))
//                {
//                    domainIndex = Array.IndexOf(RFQDomains, _domain);
//                    if (_txtData.Text.Contains(_domain + "/apps/quote"))
//                    {
//                        for (int i = 0; i < _txtData.Lines.Length; i++)
//                        {
//                            string line = _txtData.Lines[i];
//                            if (line.Contains(_domain + "/apps/quote"))
//                            {
//                                //get URL
//                                int startIndex = line.IndexOf(_domain + "/apps/quote");
//                                if (line.Length > startIndex)
//                                {
//                                    _url = line.Substring(startIndex);
//                                }
//                                else _url = line.Trim();

//                                if (_url.Contains(">"))
//                                {
//                                    int endIndex = _url.IndexOf(">");
//                                    if (line.Length > endIndex)
//                                    {
//                                        _url = _url.Substring(0, endIndex);
//                                    }
//                                }
//                                //else if (_url.Contains(" "))
//                                //{
//                                //    int endIndex = _url.IndexOf(" ");
//                                //    if (line.Length > endIndex)
//                                //    {
//                                //        _url = _url.Substring(0, endIndex);
//                                //    }
//                                //}
//                                else if (_url.EndsWith("."))
//                                    _url = _url.Trim('.');

//                                //else _url = line.Trim().Replace(".", "");

//                                if (!string.IsNullOrEmpty(_url)) { ResultURL = _url; }
//                                break;
//                            }
//                        }
//                        this.DocType = "RFQ";
//                    }
//                    if (!string.IsNullOrEmpty(ResultURL)) { break; }
//                }
//                else
//                {
//                    _url = GetCorrectURL(_txtData, _domain, RFQDomains); //added on 26/06/2019 by Kalpita
//                    if (!string.IsNullOrEmpty(_url))
//                    {
//                        this.DocType = "RFQ";
//                        break;
//                    }
//                }

//            }
//        }

//        if (!IsRFQ)
//        {
//            //if (domainIndex > 0 || domainIndex < 0)
//            if (domainIndex < 0)
//            {
//                foreach (string _domain in PODomains)
//                {
//                    if (_domain != "")
//                    {
//                        if (_txtData.Text.Contains(_domain))
//                        {
//                            domainIndex = Array.IndexOf(PODomains, _domain);
//                            if (_txtData.Text.Contains(_domain + "/private/exports/purchase/orders"))
//                            {
//                                for (int i = 0; i < _txtData.Lines.Length; i++)
//                                {
//                                    string line = _txtData.Lines[i];
//                                    if (line.Contains(_domain + "/private/exports/purchase/orders"))
//                                    {
//                                        //get URL
//                                        int startIndex = line.IndexOf(_domain + "/private/exports/purchase/orders");
//                                        if (line.Length > startIndex)
//                                        {
//                                            _url = line.Substring(startIndex);
//                                        }
//                                        else _url = line.Trim();

//                                        int endIndex = _url.IndexOf(">");
//                                        if (endIndex != -1)
//                                        {
//                                            if (line.Length > endIndex)
//                                            {
//                                                _url = _url.Substring(0, endIndex);
//                                            }
//                                            else _url = line.Trim();
//                                        }
//                                        else _url = _url.Trim();
//                                        this.DocType = "PO";
//                                        break;
//                                    }
//                                }
//                            }
//                        }
//                        else
//                        {
//                            if (!string.IsNullOrEmpty(_url))
//                            { }
//                            else
//                            {
//                                _url = GetCorrectURL(_txtData, _domain, PODomains);//added on 26/06/2019 by Kalpita
//                                if (!string.IsNullOrEmpty(_url))
//                                {
//                                    this.DocType = "PO";
//                                    break;
//                                }
//                            }
//                        }
//                        //}
//                    }
//                }
//            }
//        }
//        // }
//        _url = _url.Trim().Replace("&amp;", "&").Trim().TrimStart('<').TrimStart('"').TrimEnd('>').TrimEnd('"').Trim();
//        if (_url.Contains(' ')) _url = _url.Split(' ')[0].Trim('.').Trim('\"');//updated .Trim('\"'); on 08-04-2019

//        if (IsRFQ && _url == "") throw new Exception("RFQ Domain not found in list.");
//        else if (!IsRFQ && _url == "") throw new Exception("PO Domain not found in list.");
//    }
//    catch (Exception ex)
//    {
//        throw;
//        //LogText = "Exception while getting URL for RFQ : " + ex.GetBaseException().ToString();
//    }
//    return _url;
//}
#endregion

#endregion