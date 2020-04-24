using DotNetBrowser;
using DotNetBrowser.DOM;
using DotNetBrowser.WinForms;
using DotNetBrowserWrapper;
using FILE_Conversion;
using HtmlAgilityPack;
using HTTPRoutines;
using MTML.GENERATOR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;

//using System.Windows.Forms;


namespace GreenWave_Http_Routine
{
    public class Routines : LeSCommon.LeSCommon
    {
        public int iRetry = 0, IsAdditional_Item=0, IsUOM=0, IsAveragePrice=0;
        public bool IsLoggedin, IsDecline;
        public string currentDate = "", SITEURL = "", cMtml_filepath = "", ProcessorName = "", sDoneFile = "", QuotePath="",cProcessRFQ, cProcessPO, errStr, VRNO,
            sAuditMesage = "", cCookieVal = "", MailTextFilePath = "",DocType = "", cDomain_Name = "", SuppCode = "", BuyerCode = "", Buyer_LnkCode = "", AuditName = "";
        public string[] BuyerNames, Buyer_Supplier_LinkID, Actions;
        public List<string> slAnchors =new List<string>();
        int domainIndex = -1;
        string _Refno = "", supplierRefno = "", LeadDays = "", LesRecordID = "", BuyerName = "", VesselName = "",_MessageNumber="",
        BuyerRef = "", RequestDate = "", PortName = "", PortCode = "", Currency = "", IMONo = "",  SupplierCode = "",
        Expredate = "", DelevDate = "", BuyerAddrName = "", BuyerEmail = "", BuyerPhone = "", BuyerFax = "", SupplierAddrName = "",
        SuppEmail = "", SupplierPhone = "", SupplierFax = "", SupplierComments = "", PayTerms = "",
        lineNo = "", PartNo = "", _DocType = "", AddComment = "", pValue = "", iValue = "", pFileName = "", GrandTotal = "",
        Allowance = "", PackingCost = "", FreightAmt = "", FreightAmt1 = "", DepositAmt = "", OtherCost = "", ItemTotal = "", BuyerItemTotal = "",
        HeaderDiscount = "", MailTextPath = "", mailtextUrl = "", _OriginalFile = "", _UploadAttachment = "", strPDFAttachPath="";
        public HTTPWrapper.HTTPWrapper _wrapper = new HTTPWrapper.HTTPWrapper();
        public NetBrowser _dotwrapper = new NetBrowser();
        HTTP _httproutine = new HTTP();
        HtmlAgilityPack.HtmlDocument _htmlDoc;
        LineItemCollection _LineItems = null;
        public MTMLInterchange _interchange { get; set; }
        public ReadXMLRoutine.ReadXML _xmlRoutines = new ReadXMLRoutine.ReadXML();
        System.Windows.Forms.RichTextBox _txtData = new System.Windows.Forms.RichTextBox();

        public Routines()
        {
            GetAppConfigSettings();
        }

        public void GetAppConfigSettings()
        {
            try
            {
                Initialise();
                BuyerCode = Convert.ToString(ConfigurationManager.AppSettings["BUYER_CODE"]);
                SuppCode = Convert.ToString(ConfigurationManager.AppSettings["SUPPLIER_CODE"]);
                Buyer_LnkCode = Convert.ToString(ConfigurationManager.AppSettings["BUYER_LINKCODE"]);
                LoginRetry = convert.ToInt(ConfigurationManager.AppSettings["LOGINRETRY"]);

                ProcessorName = Convert.ToString(ConfigurationManager.AppSettings["PROCESSOR_NAME"]);
                MailTextFilePath = Convert.ToString(ConfigurationManager.AppSettings["MAILTEXTFILE_PATH"]);
                cDomain_Name = Convert.ToString(ConfigurationManager.AppSettings["DOMAIN_NAME"]);

                cProcessRFQ = convert.ToString(ConfigurationManager.AppSettings["PROCESS_RFQ"]).ToUpper();
                cProcessPO = convert.ToString(ConfigurationManager.AppSettings["PROCESS_PO"]).ToUpper();

                if (currentDate == null || currentDate == "")
                    currentDate = DateTime.Today.AddDays(-1).ToString("dd-MMM-yyyy");
                else
                    currentDate = DateTime.ParseExact(currentDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).AddDays(-1).ToString("dd-MMM-yyyy");

                AuditPath = (Convert.ToString(ConfigurationManager.AppSettings["ESUPPLIER_AUDIT"]) != "") ? Convert.ToString(ConfigurationManager.AppSettings["ESUPPLIER_AUDIT"]) : AppDomain.CurrentDomain.BaseDirectory + "Audit";
                LogPath = (Convert.ToString(ConfigurationManager.AppSettings["LOGPATH"]) != "") ? Convert.ToString(ConfigurationManager.AppSettings["LOGPATH"]) : AppDomain.CurrentDomain.BaseDirectory + "Log";
                PrintScreenPath = (Convert.ToString(ConfigurationManager.AppSettings["PRINT_PATH"]) != "") ? Convert.ToString(ConfigurationManager.AppSettings["PRINT_PATH"]) : AppDomain.CurrentDomain.BaseDirectory + "PrintScreen";
                QuotePath = (Convert.ToString(ConfigurationManager.AppSettings["QUOTEFILE_PATH"]) != "") ? Convert.ToString(ConfigurationManager.AppSettings["QUOTEFILE_PATH"]) : AppDomain.CurrentDomain.BaseDirectory + "Quote";
                if (Convert.ToString(ConfigurationManager.AppSettings["DOWNLOAD_ATTACH"]) != "") DownloadPath = Convert.ToString(ConfigurationManager.AppSettings["DOWNLOAD_ATTACH"]);
                if (Convert.ToString(ConfigurationManager.AppSettings["XML_PATH"]) != "") cMtml_filepath = Convert.ToString(ConfigurationManager.AppSettings["XML_PATH"]);

                if (!Directory.Exists(PrintScreenPath)) Directory.CreateDirectory(PrintScreenPath);
                if (!Directory.Exists(DownloadPath)) Directory.CreateDirectory(DownloadPath);
                if (!Directory.Exists(AuditPath)) Directory.CreateDirectory(AuditPath);
                if (!Directory.Exists(cMtml_filepath)) Directory.CreateDirectory(cMtml_filepath);
                if (!Directory.Exists(LogPath)) Directory.CreateDirectory(LogPath);

                if (MailTextFilePath.Trim().Length == 0) throw new Exception("No RFQ File Path present.");
                Actions = ConfigurationManager.AppSettings["ACTIONS"].Split(',');
                HiddenAttributeKey = "name";
                strPDFAttachPath = (Convert.ToString(ConfigurationManager.AppSettings["PDF_ATTACH_PATH"]) != "") ? Convert.ToString(ConfigurationManager.AppSettings["PDF_ATTACH_PATH"]) : AppDomain.CurrentDomain.BaseDirectory + "PDF";//added by Kalpita on 31/12/2019
            }
            catch (Exception e)
            {
                LogText = "Exception during initialise: " + e.GetBaseException().ToString();
            }
        }
      
        #region RFQ
  
        public void ProcessRFQ()
        {
            int j = 0;
            string filename = "", cMsgFile = "";
            DirectoryInfo _dir = new DirectoryInfo(MailTextFilePath);
            if (_dir.GetFiles().Length > 0)
            {
                FileInfo[] _Files = _dir.GetFiles();
                foreach (FileInfo f in _Files)
                {
                    filename = f.FullName; cMsgFile = Path.GetFileNameWithoutExtension(filename) + ".msg";

                    URL = GetURL(filename);
                    if (!string.IsNullOrEmpty(URL))
                    {
                        if (this.DocType == "RFQ")
                        {
                            if (cProcessRFQ == "TRUE")
                            {
                                LogText = "Processing RFQ file " + Path.GetFileName(filename) + " started";
                                try
                                {
                                    File.Copy(filename, PrintScreenPath + "\\" + Path.GetFileName(filename), true);
                                    try
                                    {
                                        LoadURL("", "", "", true);
                                        _htmlDoc = new HtmlAgilityPack.HtmlDocument();
                                        _htmlDoc.OptionAutoCloseOnEnd = true;
                                        HtmlNode.ElementsFlags.Remove("option");
                                        _htmlDoc.LoadHtml(_httpWrapper._CurrentResponseString);
                                        GetRFQHeaders_Items(_htmlDoc, this.URL, filename);
                                    }
                                    catch (Exception e)
                                    {
                                        LogText = "Unable to process RFQ  " + e.GetBaseException().Message.ToString() + Path.GetFileName(filename);
                                        MoveFiles(filename, MailTextFilePath + "\\Error\\" + Path.GetFileName(filename));
                                    }
                                }
                                catch (Exception e)
                                {
                                    LogText = "Exception while processing RFQ : " + e.GetBaseException().Message.ToString() + filename;
                                    MoveFiles(filename, MailTextFilePath + "\\Error\\" + Path.GetFileName(filename));
                                }
                                LogText = "Processing RFQ file " + Path.GetFileName(filename) + " ended";
                                LogText = "------------------------------------------";
                            }
                        }
                    }
                }
            }
            else
            {
                LogText = "No RFQ files present.";
            }
        }     

        private void GetRFQHeaders_Items(HtmlAgilityPack.HtmlDocument _htmlDoc, string URL, string RFQFile)
        {
            string cMsgFile = Path.GetFileNameWithoutExtension(RFQFile) + ".msg";
            LeSXML.LeSXML iRFQ = new LeSXML.LeSXML();
            string cHeaderRemarks = "";
            try
            {
                LogText = "Get RFQ Headers & Items started";
                HtmlNode errNode = _htmlDoc.DocumentNode.SelectSingleNode("//div[@class='error-page']");
                if (errNode != null)
                {
                    LogText = "Error page found while processing file " + Path.GetFileName(RFQFile);
                    CreateAuditFile(Path.GetFileName(RFQFile), ProcessorName, "", "Error", "LeS-1004:Unable to process file " + Path.GetFileName(RFQFile), BuyerCode, SupplierCode, AuditPath);
                    MoveFiles(RFQFile, Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile));
                }
                else
                {
                    HtmlNode _node = _htmlDoc.GetElementbyId("idProcurementQuotationSupplierController");
                    if (_node != null && _node.InnerText.ToUpper().Contains("TENDER CLOSED"))
                    {
                        LogText = "Tender is closed for file " + Path.GetFileName(RFQFile);
                        CreateAuditFile(Path.GetFileName(RFQFile), ProcessorName, "", "Error", "LeS-1004:Unable to process file " + Path.GetFileName(RFQFile) + " since tender is closed.", BuyerCode, SupplierCode, AuditPath);
                        MoveFiles(RFQFile, Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile));
                    }
                    else
                    {
                        string cVRNo = _htmlDoc.GetElementbyId("RequestForQuotation.Requisition.Code").GetAttributeValue("value", "");
                        string cSupplierName = _htmlDoc.GetElementbyId("RfqSupplier.Supplier").GetAttributeValue("value", "");
                        string cSupplierEmail = _htmlDoc.GetElementbyId("RfqSupplier.Supplier.Email").GetAttributeValue("value", "");
                        string cReqnType = _htmlDoc.GetElementbyId("RequestForQuotation.Requisition.Type").GetAttributeValue("value", "");
                        string cReqnNature = _htmlDoc.GetElementbyId("RequestForQuotation.Requisition.NatureId").GetAttributeValue("value", "");

                        string cReqnComments = _htmlDoc.GetElementbyId("RequestForQuotation.Requisition.Comments").GetAttributeValue("value", "");
                        string cRFQComments = _htmlDoc.GetElementbyId("RequestForQuotation.Comments").GetAttributeValue("value", "");
                        string cRFQDate = _htmlDoc.GetElementbyId("RequestForQuotation.Requisition.Date").GetAttributeValue("value", "");

                        if (!string.IsNullOrEmpty(cReqnComments)) cHeaderRemarks += " Requisition Comments :" + cReqnComments + Environment.NewLine;
                        if (!string.IsNullOrEmpty(cRFQComments)) cHeaderRemarks += " RFQ Comments :" + cRFQComments + Environment.NewLine;
                        if (!string.IsNullOrEmpty(cReqnType)) cHeaderRemarks += " Requisition Type :" + cReqnType + Environment.NewLine;
                        if (!string.IsNullOrEmpty(cReqnNature)) cHeaderRemarks += " Requisition Nature :" + cReqnNature;

                        iRFQ.Active = "1";

                        iRFQ.Vessel = GetValues("//select[@id='VesselId']/option[@selected]");
                        iRFQ.Currency = GetValues("//select[@id='CurrencyId']/option[@selected]");

                        iRFQ.BuyerRef = cVRNo;
                        iRFQ.Created_Date = DateTime.Now.ToString("yyyyMMdd");
                        iRFQ.DocID = DateTime.Now.ToString("yyyyMMddhhmmss");

                        string deliverydate = _htmlDoc.GetElementbyId("RequestForQuotation.SupplyDate").GetAttributeValue("value", "");
                        string docdate = _htmlDoc.GetElementbyId("RfqDate").GetAttributeValue("value", "");
                        string validitydate = _htmlDoc.GetElementbyId("RequestForQuotation.ExpiryDate").GetAttributeValue("value", "");
                        iRFQ.Date_Delivery = (!string.IsNullOrEmpty(deliverydate)) ? FormatMTMLDate(deliverydate).ToString("yyyyMMdd") : string.Empty;
                        iRFQ.Date_Document = (!string.IsNullOrEmpty(docdate)) ? FormatMTMLDate(docdate).ToString("yyyyMMdd") : string.Empty;
                        iRFQ.Date_Validity = (!string.IsNullOrEmpty(validitydate)) ? FormatMTMLDate(validitydate).ToString("yyyyMMdd") : string.Empty;
                        iRFQ.PortName = _htmlDoc.GetElementbyId("RequestForQuotation.SupplyPort").GetAttributeValue("value", "");
                        LogText = "port" + iRFQ.PortName;
                        iRFQ.Date_Preparation = DateTime.Now.ToString("yyyyMMdd");
                        iRFQ.Dialect = ProcessorName;
                        iRFQ.Doc_Type = "RFQ";
                        iRFQ.Version = "1";
                        iRFQ.DocLinkID = iRFQ.DocReferenceID = URL;
                        iRFQ.Sender_Code = Buyer_LnkCode;
                        iRFQ.Recipient_Code = SuppCode;
                        iRFQ.Remark_Header = cHeaderRemarks;

                        LeSXML.Address _byrAddress = new LeSXML.Address();
                        _byrAddress.Qualifier = "BY";
                        _byrAddress.AddressName = convert.ToString(ConfigurationManager.AppSettings["BUYER_NAME"]);
                        iRFQ.Addresses.Add(_byrAddress);

                        LeSXML.Address _suppAddress = new LeSXML.Address();
                        _suppAddress.Qualifier = "VN";
                        _suppAddress.AddressName = cSupplierName;
                        _suppAddress.eMail = cSupplierEmail;
                        iRFQ.Addresses.Add(_suppAddress);

                        iRFQ.Total_Additional_Discount = "0";
                        iRFQ.Total_Freight = "0";
                        iRFQ.Total_LineItems = "0";
                        iRFQ.Total_LineItems_Discount = "0";
                        iRFQ.Total_LineItems_Net = "0";
                        iRFQ.Total_Net_Final = "0";
                        iRFQ.Total_Other = "0";

                        iRFQ.LineItems = GetItems(_htmlDoc);
                        iRFQ.Total_LineItems = Convert.ToString(iRFQ.LineItems.Count);
                        LogText = "Get RFQ Headers & Items ended";
                        iRFQ.FileName = "RFQ_" + cVRNo.Replace("/", "_") + "_" + BuyerCode + "_" + SuppCode + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xml";
                        iRFQ.FilePath = cMtml_filepath;

                        if (iRFQ.LineItems.Count > 0)
                        {
                            string CurrenctXMLFile = cMtml_filepath + "\\" + iRFQ.FileName;
                            iRFQ.WriteXML();
                            if (File.Exists(CurrenctXMLFile))
                            {
                                LogText = Path.GetFileName(CurrenctXMLFile) + " downloaded successfully for " + cMsgFile;
                                CreateAuditFile(Path.GetFileName(CurrenctXMLFile), ProcessorName, cVRNo, "Downloaded", Path.GetFileName(CurrenctXMLFile) + " downloaded successfully for " + cMsgFile, BuyerCode, SupplierCode, AuditPath);
                                File.Copy(RFQFile, PrintScreenPath + "\\" + Path.GetFileName(RFQFile), true);
                                MoveFiles(RFQFile, Path.GetDirectoryName(RFQFile) + "\\Backup\\" + Path.GetFileName(RFQFile));
                            }
                            else
                            {
                                string eFile = PrintScreenPath + "\\GreenWave_Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
                                LogText = "Unable to download file " + Path.GetFileName(CurrenctXMLFile);
                                CreateAuditFile(eFile, ProcessorName, cVRNo, "Error", "LeS-1004:Unable to process file " + Path.GetFileName(CurrenctXMLFile) + " for ref " + cVRNo + ".", BuyerCode, SupplierCode, AuditPath);
                                if (!PrintScreen(eFile)) eFile = "";
                                MoveFiles(RFQFile, Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogText = "Exception while getting RFQ Header Details : " + ex.StackTrace;
                throw;
            }
        }

        private LeSXML.LineItemCollection GetItems(HtmlAgilityPack.HtmlDocument _htmlDoc)
        {
            LeSXML.LineItemCollection _itemCollect = new LeSXML.LineItemCollection();
            try
            {
                HtmlNode tblItems = _htmlDoc.DocumentNode.SelectSingleNode("//table[@id='ProcurementQuotationEntryDetails']");
                List<List<string>> parsedTblHdr = _htmlDoc.DocumentNode.SelectSingleNode("//table[@id='ProcurementQuotationEntryDetails']").Descendants("tr")
                    .Where(tr => tr.Elements("th").Count() > 1).Select(tr => tr.Elements("th").Select(td => td.InnerText.Trim()).ToList()).ToList();
                List<List<string>> parsedTblBody = _htmlDoc.DocumentNode.SelectSingleNode("//table[@id='ProcurementQuotationEntryDetails']").Descendants("tr")
                    .Where(tr => tr.Elements("td").Count() > 1).Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim() + "|" + td.InnerHtml.Trim()).ToList()).ToList();

                List<string> slhdr = parsedTblHdr[0];
                for (int i = 0; i < parsedTblBody.Count; i++)
                {
                    LeSXML.LineItem _item = new LeSXML.LineItem(); string Remarks = "";
                    List<string> sldet = parsedTblBody[i];
                    for (int j = 0; j < sldet.Count; j++)
                    {
                        switch (slhdr[j])
                        {
                            case "Item": _item.Name = sldet[j].Split('|')[0];
                                break;
                            case @"Parent item \ Category": Remarks += @" ParentItem_Category : " + sldet[j].Split('|')[0] + ",";
                                break;
                            case "Part number": _item.ItemRef = sldet[j].Split('|')[0];
                                break;
                            case "Make": Remarks += " Make : " + sldet[j].Split('|')[0] + ",";
                                break;
                            case "Req Qty": _item.Quantity = (!string.IsNullOrEmpty(sldet[j].Split('|')[0])) ? sldet[j].Split('|')[0] : "0";
                                break;
                            case "Measure": _item.Unit = sldet[j].Split('|')[0];
                                break;
                            case "Available": string id = "ProcurementQuotationEntryDetails" + (i + 1) + "IsAvailable";
                                Boolean Is_Avail = Convert.ToBoolean(_htmlDoc.GetElementbyId(id).GetAttributeValue("value", ""));
                                string cAvailResult = (Is_Avail) ? " Available" : " Not Available";
                                Remarks += " Availability :" + cAvailResult + ",";
                                break;
                            case "Req comments": Remarks += " Reqn Comments : " + sldet[j].Split('|')[0] + ",";
                                break;
                            case "RFQ comments": if (!string.IsNullOrEmpty(sldet[j].Split('|')[0])) { Remarks += " RFQ Comments : " + sldet[j].Split('|')[0] + Environment.NewLine; }
                                break;
                            default: break;
                        }
                    }
                    _item.Number = Convert.ToString(i + 1);
                    _item.OrigItemNumber = Convert.ToString(i+ 1);
                    _item.SystemRef = Convert.ToString(i + 1);
                    _item.Remark = Remarks.TrimEnd(',');
                    _item.Discount = "0";
                    _item.ListPrice = "0";
                    _item.LeadDays = "0";
                    _itemCollect.Add(_item);
                }
            }
            catch (Exception ex)
            {
                LogText = "Exception while getting RFQ Items: " + ex.GetBaseException().ToString();
            }
            return _itemCollect;
        }

        #endregion

        #region PO

        public void ProcessOrders()
        {
            string filename = "", cMsgFile = "";
            DirectoryInfo _dir = new DirectoryInfo(MailTextFilePath);
            if (_dir.GetFiles().Length > 0)
            {
                FileInfo[] _Files = _dir.GetFiles();
                foreach (FileInfo f in _Files)
                {
                    filename = f.FullName; cMsgFile = Path.GetFileNameWithoutExtension(filename) + ".msg";
                    URL = GetURL(filename);
                    if (cProcessPO == "TRUE")
                    {
                        LogText = "Processing PO file " + Path.GetFileName(filename) + " started";
                        try
                        {
                            File.Copy(filename, PrintScreenPath + "\\" + Path.GetFileName(filename), true); Download_POFiles(filename);
                        }
                        catch (Exception e)
                        {
                            LogText = "Exception while processing PO : " + e.GetBaseException().Message.ToString() + filename;
                            MoveFiles(filename, MailTextFilePath + "\\Error\\" + Path.GetFileName(filename));
                        }
                        LogText = "Processing PO file " + Path.GetFileName(filename) + " ended";
                        LogText = "------------------------------------------";
                    }
                }
            }
            else
            {
                LogText = "No PO files present.";
            }
        }

        public void Download_POFiles(string POFile)
        {
            string cMsgFile = Path.GetFileNameWithoutExtension(POFile) + ".msg";
            try
            {
                if (URL != "")
                {
                    List<string> slRes= GetAction_Results(eActions.PO, "PO");
                    if (!slRes.Contains(URL))
                    {
                        string Filename = Path.GetFileNameWithoutExtension(POFile) + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
                        if (DownloadFile(URL, DownloadPath + Filename))
                        {
                            string cAudittxt = "";
                            LogText = cAudittxt = Filename + " downloaded successfully for " + cMsgFile;
                            File.Copy(DownloadPath + "\\" + Filename, PrintScreenPath + "\\" + Path.GetFileName(Filename), true);
                            CreateAuditFile(Filename, ProcessorName, "", "Downloaded", cAudittxt, BuyerCode, SupplierCode, AuditPath);                            
                            MoveFiles(POFile, Path.GetDirectoryName(POFile) + "\\Backup\\" + Path.GetFileName(POFile));
                        }
                        else
                        {
                            string eFile = PrintScreenPath + "\\GreenWave Shipping_HTTP_PO_Processor" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
                            if (!errStr.Contains("The remote server returned an error"))
                            {
                                LogText = "Unable to download file for " + cMsgFile;

                                if (!PrintScreen(eFile)) eFile = "";
                                if (File.Exists(eFile))
                                    CreateAuditFile(eFile, ProcessorName, "", "Error", "Unable to download file for " + cMsgFile + ".", BuyerCode, SupplierCode, AuditPath);
                            }
                            else
                            { CreateAuditFile(Path.GetFileName(POFile), ProcessorName, "", "Error", errStr, BuyerCode, SupplierCode, AuditPath); }                            
                            MoveFiles(POFile, Path.GetDirectoryName(POFile) + "\\Error\\" + Path.GetFileName(POFile));
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
                    LogText = "URL not found for navigation in file " + cMsgFile + ",Invalid file.";
                    CreateAuditFile(Path.GetFileName(POFile), ProcessorName, "", "Error", "URL not found for navigation in file " + cMsgFile + ",Invalid file.", BuyerCode, SupplierCode, AuditPath);
                    if (File.Exists(MailTextFilePath + "\\Error\\" + Path.GetFileName(POFile))) File.Delete(MailTextFilePath + "\\Error\\" + cMsgFile);
                    File.Move(POFile, MailTextFilePath + "\\Error\\" + Path.GetFileName(POFile));
                }
            }
            catch (Exception e)
            {
                LogText = "Exception while getting Order: " + e.GetBaseException().ToString() + " for file " + cMsgFile;
                MoveFiles(cMsgFile, MailTextFilePath + "\\Error\\" + Path.GetFileName(cMsgFile));
            }
        }

        #endregion

        #region Quote
        public void ProcessQuotation()
        {
            int j = 0;
            string Qfilename = "";
            try
            {
                DirectoryInfo _dir = new DirectoryInfo(QuotePath);
                if (_dir.GetFiles().Length > 0)
                {
                    FileInfo[] _Files = _dir.GetFiles();
                    foreach (FileInfo f in _Files)
                    {
                        Qfilename = f.FullName;
                        LoadInterchangeDetails(Qfilename);
                        SaveFileDetails(Path.GetFileName(Qfilename));
                    }
                }
                else
                {
                    LogText = "No Quote files present.";
                }
            }
            catch (Exception ex) { }
        }

        public void LoadInterchangeDetails(string xmlQuoteFile)
        {         
            try
            {
                MTMLClass _mtml = new MTMLClass();
                _interchange = _mtml.Load(xmlQuoteFile);
                if (_interchange != null)
                {
                    if (_interchange.Recipient != null)
                    {
                        BuyerCode = _interchange.Recipient;
                    }
                    if (_interchange.Sender != null)
                    {
                        SupplierCode = _interchange.Sender;
                    }
                    if (_interchange.DocumentHeader.DocType != null)
                    {
                        _DocType = _interchange.DocumentHeader.DocType;
                    }
                    if (_interchange.DocumentHeader != null && _interchange.DocumentHeader.IsDeclined != null)
                    {
                        IsDecline = _interchange.DocumentHeader.IsDeclined;
                    }
                    for (int g = 0; g < _interchange.DocumentHeader.References.Count; g++)
                    {
                        if (_interchange.DocumentHeader.References[g].Qualifier == ReferenceQualifier.UC)
                        {
                            _Refno = _interchange.DocumentHeader.References[g].ReferenceNumber.Trim();
                        }
                        if (_interchange.DocumentHeader.References[g].Qualifier == ReferenceQualifier.AAG)
                        {
                            supplierRefno = _interchange.DocumentHeader.References[g].ReferenceNumber.Trim();
                        }
                    }
                    if (_interchange.DocumentHeader.AdditionalDiscount != null && convert.ToFloat(_interchange.DocumentHeader.AdditionalDiscount) > 0)
                    {
                        HeaderDiscount = convert.ToString(_interchange.DocumentHeader.AdditionalDiscount);
                    }
                    if (_interchange.DocumentHeader != null && _interchange.DocumentHeader.OriginalFile != null && _interchange.DocumentHeader.OriginalFile != "")
                    {
                        _OriginalFile = convert.ToString(_interchange.DocumentHeader.OriginalFile);
                    }
                    if (_interchange.DocumentHeader != null && _interchange.DocumentHeader.MessageNumber != null && _interchange.DocumentHeader.MessageNumber != "")
                    {
                        _MessageNumber = convert.ToString(_interchange.DocumentHeader.MessageNumber);
                    }

                    LesRecordID = Convert.ToString(_interchange.BuyerSuppInfo.RecordID);

                    #region -- Read Interchang Object

                    if (_interchange.DocumentHeader.LeadTimeDays != null) { LeadDays = _interchange.DocumentHeader.LeadTimeDays; }

                    if (_interchange.DocumentHeader.DocType != null) { _DocType = _interchange.DocumentHeader.DocType; }

                    Currency = Convert.ToString(_interchange.DocumentHeader.CurrencyCode);

                    if (_interchange.DocumentHeader.IsAltItemAllowed != null) { IsAdditional_Item = Convert.ToInt32(_interchange.DocumentHeader.IsAltItemAllowed); }

                    if (_interchange.DocumentHeader.IsPriceAveraged != null) { IsAveragePrice = Convert.ToInt32(_interchange.DocumentHeader.IsPriceAveraged); }

                    if (_interchange.DocumentHeader.IsUOMChanged != null) { IsUOM = Convert.ToInt32(_interchange.DocumentHeader.IsUOMChanged); }

                    #endregion

                    #region -- Read Interchange Party Address
                    for (int g = 0; g < _interchange.DocumentHeader.PartyAddresses.Count; g++)
                    {
                        if (_interchange.DocumentHeader.PartyAddresses[g].Qualifier == PartyQualifier.UD)
                        {
                            VesselName = _interchange.DocumentHeader.PartyAddresses[g].Name;
                            if (_interchange.DocumentHeader.PartyAddresses[g].PartyLocation.Berth != null)
                            {
                                PortName = _interchange.DocumentHeader.PartyAddresses[g].PartyLocation.Berth;
                            }
                            if (_interchange.DocumentHeader.PartyAddresses[g].PartyLocation.Port != null)
                            {
                                PortCode = _interchange.DocumentHeader.PartyAddresses[g].PartyLocation.Port;
                            }
                        }
                        if (_interchange.DocumentHeader.PartyAddresses[g].Qualifier == PartyQualifier.BY)
                        {
                            if (_interchange.DocumentHeader.PartyAddresses[g].Name != null)
                            {
                                BuyerName = _interchange.DocumentHeader.PartyAddresses[g].Name;
                                BuyerAddrName = _interchange.DocumentHeader.PartyAddresses[g].Name;
                                if (_interchange.DocumentHeader.PartyAddresses[g].Contacts != null)
                                {
                                    if (_interchange.DocumentHeader.PartyAddresses[g].Contacts.Count > 0)
                                    {
                                        if (_interchange.DocumentHeader.PartyAddresses[g].Contacts[0].CommunMethodList.Count > 0)
                                        {
                                            for (int f = 0; f < _interchange.DocumentHeader.PartyAddresses[g].Contacts[0].CommunMethodList.Count; f++)
                                            {
                                                if (_interchange.DocumentHeader.PartyAddresses[g].Contacts[0].CommunMethodList[f].Qualifier == CommunicationMethodQualifiers.TE)
                                                {
                                                    BuyerPhone = _interchange.DocumentHeader.PartyAddresses[g].Contacts[0].CommunMethodList[f].Number;
                                                }
                                                if (_interchange.DocumentHeader.PartyAddresses[g].Contacts[0].CommunMethodList[f].Qualifier == CommunicationMethodQualifiers.FX)
                                                {
                                                    BuyerFax = _interchange.DocumentHeader.PartyAddresses[g].Contacts[0].CommunMethodList[f].Number;
                                                }
                                                if (_interchange.DocumentHeader.PartyAddresses[g].Contacts[0].CommunMethodList[f].Qualifier == CommunicationMethodQualifiers.EM)
                                                {
                                                    BuyerEmail = _interchange.DocumentHeader.PartyAddresses[g].Contacts[0].CommunMethodList[f].Number;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (_interchange.DocumentHeader.PartyAddresses[g].Qualifier == PartyQualifier.VN)
                        {
                            SupplierAddrName = _interchange.DocumentHeader.PartyAddresses[g].Name;
                            if (_interchange.DocumentHeader.PartyAddresses[g].Contacts != null)
                            {
                                if (_interchange.DocumentHeader.PartyAddresses[g].Contacts.Count > 0)
                                {
                                    if (_interchange.DocumentHeader.PartyAddresses[g].Contacts[0].CommunMethodList.Count > 0)
                                    {
                                        for (int f = 0; f < _interchange.DocumentHeader.PartyAddresses[g].Contacts[0].CommunMethodList.Count; f++)
                                        {
                                            if (_interchange.DocumentHeader.PartyAddresses[g].Contacts[0].CommunMethodList[f].Qualifier == CommunicationMethodQualifiers.TE)
                                            {
                                                SupplierPhone = _interchange.DocumentHeader.PartyAddresses[g].Contacts[0].CommunMethodList[f].Number;
                                            }
                                            if (_interchange.DocumentHeader.PartyAddresses[g].Contacts[0].CommunMethodList[f].Qualifier == CommunicationMethodQualifiers.FX)
                                            {
                                                SupplierFax = _interchange.DocumentHeader.PartyAddresses[g].Contacts[0].CommunMethodList[f].Number;
                                            }
                                            if (_interchange.DocumentHeader.PartyAddresses[g].Contacts[0].CommunMethodList[f].Qualifier == CommunicationMethodQualifiers.EM)
                                            {
                                                SuppEmail = _interchange.DocumentHeader.PartyAddresses[g].Contacts[0].CommunMethodList[f].Number;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region -- Supplier Comment
                    for (int g = 0; g < _interchange.DocumentHeader.Comments.Count; g++)
                    {
                        if (_interchange.DocumentHeader.Comments[g].Qualifier == CommentTypes.SUR)
                        {
                            SupplierComments = _interchange.DocumentHeader.Comments[g].Value;
                        }
                        else if (_interchange.DocumentHeader.Comments[g].Qualifier == CommentTypes.ZTP)
                        {
                            PayTerms = _interchange.DocumentHeader.Comments[g].Value;
                        }
                    }
                    #endregion

                    _LineItems = _interchange.DocumentHeader.LineItems;

                    #region -- Read Iterchange Time Period

                    for (int g = 0; g < _interchange.DocumentHeader.DateTimePeriods.Count; g++)
                    {
                        if (_interchange.DocumentHeader.DateTimePeriods[g].Qualifier != null)
                        {
                            if (_interchange.DocumentHeader.DateTimePeriods[g].Qualifier == DateTimePeroidQualifiers.ExpiryDate_36)
                            {
                                if (_interchange.DocumentHeader.DateTimePeriods[g].Value != null)
                                {
                                    DateTime dtExpredate = Format_QteMTMLDate(_interchange.DocumentHeader.DateTimePeriods[g].Value);
                                    if (dtExpredate != DateTime.MinValue)
                                    {
                                        Expredate = dtExpredate.ToString("yyyy-MM-dd");
                                    }
                                }
                            }
                            if (_interchange.DocumentHeader.DateTimePeriods[g].Qualifier == DateTimePeroidQualifiers.DeliveryDate_69)
                            {
                                if (_interchange.DocumentHeader.DateTimePeriods[g].Value != null)
                                {
                                    if (_interchange.DocumentHeader.DateTimePeriods[g].Value != null)
                                    {
                                        DateTime DtDelevDate = Format_QteMTMLDate(_interchange.DocumentHeader.DateTimePeriods[g].Value);
                                        if (DtDelevDate != DateTime.MinValue)
                                        {
                                            DelevDate = DtDelevDate.ToString("yyyy-MM-dd");
                                        }
                                        if (LeadDays == "" || LeadDays == "0")
                                        {
                                            if (DtDelevDate != DateTime.MinValue)
                                            {
                                                DateTime _current = DateTime.Now;
                                                double _Deldays = 0;
                                                _Deldays = (DtDelevDate - _current).TotalDays;
                                                _Deldays = (DtDelevDate - _current).Days;
                                                if (_Deldays <= 0)
                                                {
                                                    LeadDays = "2";
                                                }
                                                else
                                                {
                                                    LeadDays = _Deldays.ToString();
                                                }
                                            }
                                            else
                                            {
                                                LeadDays = "2";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    #region -- Read Iterchange MonetoryAmounts
                    if (_interchange != null && _interchange.DocumentHeader != null && _interchange.DocumentHeader.MonetoryAmounts != null)
                    {
                        for (int g = 0; g < _interchange.DocumentHeader.MonetoryAmounts.Count; g++)
                        {
                            if (_interchange.DocumentHeader.MonetoryAmounts[g].Qualifier == MonetoryAmountQualifier.GrandTotal_259)
                            {
                                GrandTotal = _interchange.DocumentHeader.MonetoryAmounts[g].Value.ToString();
                            }
                            else if (_interchange.DocumentHeader.MonetoryAmounts[g].Qualifier == MonetoryAmountQualifier.AllowanceAmount_204)
                            {
                                Allowance = _interchange.DocumentHeader.MonetoryAmounts[g].Value.ToString();
                            }
                            else if (_interchange.DocumentHeader.MonetoryAmounts[g].Qualifier == MonetoryAmountQualifier.FreightCharge_64)
                            {
                                FreightAmt = _interchange.DocumentHeader.MonetoryAmounts[g].Value.ToString();
                            }
                            else if (_interchange.DocumentHeader.MonetoryAmounts[g].Qualifier == MonetoryAmountQualifier.PackingCost_106)
                            {
                                PackingCost = _interchange.DocumentHeader.MonetoryAmounts[g].Value.ToString();
                            }
                            else if (_interchange.DocumentHeader.MonetoryAmounts[g].Qualifier == MonetoryAmountQualifier.TaxCost_99)
                            {
                                FreightAmt1 = _interchange.DocumentHeader.MonetoryAmounts[g].Value.ToString();
                            }
                            else if (_interchange.DocumentHeader.MonetoryAmounts[g].Qualifier == MonetoryAmountQualifier.Deposit_97)
                            {
                                DepositAmt = _interchange.DocumentHeader.MonetoryAmounts[g].Value.ToString();
                                SupplierComments += Environment.NewLine + "Deposit Amt : " + String.Format("{0:0.00}", convert.ToFloat(DepositAmt));
                            }
                            else if (_interchange.DocumentHeader.MonetoryAmounts[g].Qualifier == MonetoryAmountQualifier.OtherCost_98)
                            {
                                OtherCost = _interchange.DocumentHeader.MonetoryAmounts[g].Value.ToString();
                            }
                            else if (_interchange.DocumentHeader.MonetoryAmounts[g].Qualifier == MonetoryAmountQualifier.TotalLineItemsAmount_79)
                            {
                                ItemTotal = _interchange.DocumentHeader.MonetoryAmounts[g].Value.ToString();
                            }
                            else if (_interchange.DocumentHeader.MonetoryAmounts[g].Qualifier == MonetoryAmountQualifier.BuyerItemTotal_90)
                            {
                                BuyerItemTotal = _interchange.DocumentHeader.MonetoryAmounts[g].Value.ToString();
                            }                          
                        }
                    }
                    FreightAmt = convert.ToString(convert.ToFloat(FreightAmt) + convert.ToFloat(FreightAmt1));
                    DepositAmt = convert.ToString(convert.ToFloat(DepositAmt) + convert.ToFloat(OtherCost));

                    #endregion

                    ReadAttachments(xmlQuoteFile);//added by Kalpita on 31/12/2019
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error on LoadInterchangeDetails : " + ex);
            }
        }

        public void SaveFileDetails(string filename)
        {
            LogText = "Processing Quote file " + Path.GetFileName(filename) + " started";
            if (_wrapper.LoadURL(_MessageNumber, "input", "id", "Number", "ASP.NET_SessionId"))//SESSION
            {
                Process_QuoteRequest(filename);
            }
            else
            {
                LogText = "Unable to load url for Ref: " + _Refno;
                MoveFiles(filename, QuotePath + "\\Error\\" + Path.GetFileName(filename));
            }
            LogText = "Processing RFQ file " + Path.GetFileName(filename) + " ended";
            LogText = "------------------------------------------";
        }

        public void Process_QuoteRequest(string filename)
        {
            string _IdVal = "", cSiteTotal = "";
            bool _result = false;
            string cQuoteFilePath = QuotePath +"\\"+ filename;//added by kalpita on 14/04/2020
            try
            {
                _htmlDoc = _wrapper._CurrentDocument;

                var boundary = "WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x", System.Globalization.NumberFormatInfo.InvariantInfo);
                HtmlAgilityPack.HtmlNode _eleIdVal = _wrapper.GetElement("input", "name", "Id");
                if (_eleIdVal != null) { _IdVal = convert.ToString(_eleIdVal.Attributes["value"].Value); }

                HtmlAgilityPack.HtmlNode _eleIsNewRecord = _wrapper.GetElement("input", "name", "IsNewRecord");
                string _IsNewRecordVal = (_eleIsNewRecord != null) ? convert.ToString(_eleIsNewRecord.Attributes["value"].Value) : string.Empty;

                HtmlAgilityPack.HtmlNode _eleComparisonId = _wrapper.GetElement("input", "name", "ComparisonId");
                string _ComparisonIdVal = (_eleComparisonId != null) ? convert.ToString(_eleComparisonId.Attributes["value"].Value) : string.Empty;

                HtmlAgilityPack.HtmlNode _eleRfqId = _wrapper.GetElement("input", "name", "RfqId");
                string _RfqIdVal = (_eleRfqId != null) ? convert.ToString(_eleRfqId.Attributes["value"].Value) : string.Empty;

                HtmlAgilityPack.HtmlNode _eleParentQuotationId = _wrapper.GetElement("input", "name", "ParentQuotationId");
                string _ParentQuotationIdVal = (_eleParentQuotationId != null) ? convert.ToString(_eleParentQuotationId.Attributes["value"].Value) : string.Empty;

                string _VesselIdVal = "";
                HtmlAgilityPack.HtmlNode _eleVesselId = _wrapper.GetElement("select", "name", "VesselId");
                HtmlAgilityPack.HtmlNodeCollection _selNodeColl = _eleVesselId.ChildNodes;
                foreach (HtmlAgilityPack.HtmlNode _node in _selNodeColl)
                {
                    if (_node.Attributes["selected"] != null)
                    {
                        _VesselIdVal = convert.ToString(_node.Attributes["value"].Value);
                        break;
                    }
                }

                string _RfqSupplierIdVal = "";
                HtmlAgilityPack.HtmlNode _elerfqsupplierid = _wrapper.GetElement("select", "name", "RfqSupplierId");
                HtmlAgilityPack.HtmlNode _nodeSupplierIdVal = _wrapper.GetElement(_elerfqsupplierid, "option", "selected", "true");
                if (_nodeSupplierIdVal != null) _RfqSupplierIdVal = convert.ToString(_nodeSupplierIdVal.Attributes["value"].Value);

                string _CurrencyIdVal = "";
                HtmlAgilityPack.HtmlNode _eleCurrencyId = _wrapper.GetElement("select", "name", "CurrencyId");
                HtmlAgilityPack.HtmlNode _nodeCurrencyIdVal = _wrapper.GetElement(_eleCurrencyId, "option", Currency.ToUpper());
                if (_nodeCurrencyIdVal != null) _CurrencyIdVal = convert.ToString(_nodeCurrencyIdVal.Attributes["value"].Value);

                string _attachmentOldvalue = "";
                HtmlAgilityPack.HtmlNode _eleAttachmentOldvalue = _wrapper.GetElement("input", "name", "Attachments_OldValue");
                if (_eleAttachmentOldvalue != null)
                {
                    _attachmentOldvalue = convert.ToString(_eleAttachmentOldvalue.Attributes["value"].Value);
                    if (_attachmentOldvalue.Contains("&quot;"))
                    {
                        _attachmentOldvalue = _attachmentOldvalue.Replace("&quot;", "\"");
                    }
                }

                string _sdata = "------" + boundary + Environment.NewLine +

                "Content-Disposition: form-data; name=\"id\"" + Environment.NewLine + Environment.NewLine +
                _IdVal + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"isnewrecord\"" + Environment.NewLine + Environment.NewLine +

                _IsNewRecordVal + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"vesselid\"" + Environment.NewLine + Environment.NewLine +

                _VesselIdVal + Environment.NewLine +
                //"------" + boundary + Environment.NewLine +
                //"Content-Disposition: form-data; name=\"isnewrecord\"" + Environment.NewLine + Environment.NewLine +

                //_IsNewRecordVal + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"comparisonid\"" + Environment.NewLine + Environment.NewLine +

                _ComparisonIdVal + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"rfqid\"" + Environment.NewLine + Environment.NewLine +

                _RfqIdVal + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"parentquotationid\"" + Environment.NewLine + Environment.NewLine +

                _ParentQuotationIdVal + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"rfqsupplierid\"" + Environment.NewLine + Environment.NewLine +

                _RfqSupplierIdVal + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"date\"" + Environment.NewLine + Environment.NewLine +

                DateTime.Now.ToString("dd/MM/yyyy") + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"number\"" + Environment.NewLine + Environment.NewLine +

                supplierRefno + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"currencyid\"" + Environment.NewLine + Environment.NewLine +

                _CurrencyIdVal + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"creditperiod\"" + Environment.NewLine + Environment.NewLine + "0" + Environment.NewLine +

               // "------" + boundary + Environment.NewLine +
               // "Content-Disposition: form-data; name=\"forwardingcost\"" + Environment.NewLine + Environment.NewLine +

               //convert.ToString(convert.ToFloat(FreightAmt) + convert.ToFloat(DepositAmt)) + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"packagingcost\"" + Environment.NewLine + Environment.NewLine +

                convert.ToString(convert.ToFloat(FreightAmt) + convert.ToFloat(DepositAmt)) + Environment.NewLine +//packagingcost
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"alltotalitemcost\"" + Environment.NewLine + Environment.NewLine +

                ItemTotal + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"alltotalquotationcostdisplay\"" + Environment.NewLine + Environment.NewLine +

                GrandTotal + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"alltotalcostinbasecurrency\"" + Environment.NewLine + Environment.NewLine +

                GrandTotal + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"terms\"" + Environment.NewLine + Environment.NewLine +

                SupplierComments + Environment.NewLine +
                //"------" + boundary + Environment.NewLine +
                //"Content-Disposition: form-data; name=\"leadtime\"" + Environment.NewLine + Environment.NewLine +

                //LeadDays + Environment.NewLine +
              
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"taxtype\"" + Environment.NewLine + Environment.NewLine +


                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"taxpercentage\"" + Environment.NewLine + Environment.NewLine +

                "0" + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"taxamount\"" + Environment.NewLine + Environment.NewLine +

                "0" + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"alltotaltax\"" + Environment.NewLine + Environment.NewLine +

                "0.00" + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"attachments_oldvalue\"" + Environment.NewLine + Environment.NewLine +

                 _attachmentOldvalue + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"create\"" + Environment.NewLine+

                 "------" + boundary + Environment.NewLine +
                 "Content-Disposition: form-data; name=\"initialapprovalstatus\"" + Environment.NewLine + Environment.NewLine +

                 "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"initialapprovaldate\"" + Environment.NewLine + Environment.NewLine +

                 "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"initialapprovalbyid\"" + Environment.NewLine + Environment.NewLine +

                 "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"initialapprovalcomments\"" + Environment.NewLine + Environment.NewLine +

                 "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"initialapprovalattachments_oldvalue\"" + Environment.NewLine + Environment.NewLine +

                 "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"finalapprovalstatus\"" + Environment.NewLine + Environment.NewLine +

                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"finalapprovaldate\"" + Environment.NewLine + Environment.NewLine +

                 "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"finalapprovalbyid\"" + Environment.NewLine + Environment.NewLine +

                 "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"finalapprovalcomments\"" + Environment.NewLine + Environment.NewLine +

                 "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"finalapprovalattachments_oldvalue\"" + Environment.NewLine + Environment.NewLine;

              _sdata +=  Process_QuoteItems( boundary)+ Environment.NewLine +//Process_QuoteItems(_sdata, boundary);
            "------" + boundary + Environment.NewLine +
            "Content-Disposition: form-data; name=\"discounttype\"" + Environment.NewLine + Environment.NewLine +

            "" + Environment.NewLine +
            "------" + boundary + Environment.NewLine +
            "Content-Disposition: form-data; name=\"discountpercentage\"" + Environment.NewLine + Environment.NewLine +

            "0" + Environment.NewLine +
            "------" + boundary + Environment.NewLine +
            "Content-Disposition: form-data; name=\"discountamount\"" + Environment.NewLine + Environment.NewLine +

            "0" + Environment.NewLine +
            "------" + boundary + Environment.NewLine +
            "Content-Disposition: form-data; name=\"alltotaldiscount\"" + Environment.NewLine + Environment.NewLine +

            "0" + Environment.NewLine +
            "------" + boundary + Environment.NewLine +
            "Content-Disposition: form-data; name=\"attachments\"; filename=\"" + _OriginalFile + "\"" + Environment.NewLine +
            "Content-Type: application/pdf" + Environment.NewLine + Environment.NewLine +

            @"<!>" + strPDFAttachPath + "\\" + _OriginalFile + "<!>" + Environment.NewLine +
            "------" + boundary + Environment.NewLine +
            "Content-Disposition: form-data; name=\"save\"" + Environment.NewLine + Environment.NewLine +

            "true" + Environment.NewLine +
            "------" + boundary + "--";

                _wrapper._AddRequestHeaders.Clear();
                _wrapper._AddRequestHeaders.Add("Origin", cDomain_Name);
                _wrapper._AddRequestHeaders.Add("X-Requested-With", @"XMLHttpRequest");
                _wrapper._AddRequestHeaders.Add("X-IsXHR-Request", @"true");

                _wrapper.PostMulitContaintURL(_MessageNumber, _sdata, boundary);
                string str = _wrapper._CurrentResponseString;

                JSonSubmitResult _SubmitObj = (JSonSubmitResult)JsonConvert.DeserializeObject(str, typeof(JSonSubmitResult));
                if (_SubmitObj != null && _SubmitObj.Message != null && _SubmitObj.Message.Value.Contains("Thank You for submitting Your quotation."))
                {
                    if (_wrapper.LoadURL(_MessageNumber, "input", "id", "Number", "ASP.NET_SessionId"))
                    {
                        HtmlAgilityPack.HtmlNode eleAllTotalQuotationCostDisplay = _wrapper.GetElement("input", "id", "AllTotalQuotationCostDisplay");
                        if (eleAllTotalQuotationCostDisplay != null) cSiteTotal = eleAllTotalQuotationCostDisplay.Attributes["value"].Value;
                        if (GetIntegerValue(cSiteTotal) == GetIntegerValue(GrandTotal))
                        {
                            _result = true;
                        }
                        else if (GetIntegerValue(cSiteTotal) == GetIntegerValue(BuyerItemTotal))
                        {
                            _result = true;
                        }
                        if (_result)
                        {
                            LogText = "Quotation for Ref No. : " + _Refno + " saved and submitted successfully.";
                            bool IsAttachUploaded = Add_Attachment();
                            if (IsAttachUploaded)
                            {
                                string sAudit = "Quote for Ref No. '" + _Refno + "' submitted successfully.";
                                string afilename = PrintScreenPath + "\\GreenWave_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + SuppCode + ".png";
                                if (!PrintScreen(afilename)) afilename = "";
                                CreateAuditFile(cQuoteFilePath, ProcessorName, _Refno, "Submitted", sAudit, BuyerCode, SupplierCode, AuditPath);//filename
                                MoveFiles(cQuoteFilePath, QuotePath + "\\Backup\\" + Path.GetFileName(cQuoteFilePath));
                            }
                            else
                            {
                                LogText = "Unable to Upload Attachment for buyer ref : " + _Refno;
                            }
                        }
                        else
                        {
                            WriteErrorLog_With_Screenshot("LeS-1008.1:Unable to save quote due to amount mismatch", cQuoteFilePath, _Refno);
                        }
                    }
                    else
                    {
                        WriteErrorLog_With_Screenshot("LeS-1016.1:Unable to load URL " + _MessageNumber, cQuoteFilePath, _Refno);
                    }
                }
                else
                {
                    LogText = "Unable to submit file " + filename + " due to " + _SubmitObj.Message.Value;
                    MoveFiles(cQuoteFilePath, QuotePath + "\\Error\\" + Path.GetFileName(cQuoteFilePath));
                }
            }
            catch (Exception ex)
            {
                string errmsg = LogText = "Unable to process file " + filename + " due to " + ex.Message;
                WriteErrorLog_With_Screenshot("LeS-1004:" + errmsg, cQuoteFilePath, _Refno);
            }
        }

        private string Process_QuoteItems(string boundary)
        {
            string cResult = "";
            _htmlDoc = _wrapper._CurrentDocument;
            HtmlNode tblItems = _htmlDoc.DocumentNode.SelectSingleNode("//table[@id='ProcurementQuotationEntryDetails']");
            List<List<string>> parsedTblHdr = _htmlDoc.DocumentNode.SelectSingleNode("//table[@id='ProcurementQuotationEntryDetails']").Descendants("tr")
                .Where(tr => tr.Elements("th").Count() > 1).Select(tr => tr.Elements("th").Select(td => td.InnerText.Trim()).ToList()).ToList();
            List<List<string>> parsedTblBody = _htmlDoc.DocumentNode.SelectSingleNode("//table[@id='ProcurementQuotationEntryDetails']").Descendants("tr")
                .Where(tr => tr.Elements("td").Count() > 1).Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim() + "|" + td.InnerHtml.Trim()).ToList()).ToList();

            List<string> slhdr = parsedTblHdr[0];
            for (int i = 0; i < parsedTblBody.Count; i++)
            {
                string cRowId = "", cItemNo = "", cItemName = "", cItemRef = "", cReqQty = "", cRfqQty = "", cUOM = "", cMaker = "", cItemCategory = "", cUnitCost = "", cDiscType = "", cIsAvailable = "", cItemComments = "",
                    cDiscAmt = "", cDiscPercentage = "", cTotDisc = "", cTotCost = "", cLeadDays = "", cReqComments = "", cRfqComments = "", cRequisitionDetailId = "",
                    cRequestForQuoteDetailId = "", cIsSelectedInComparison = "", cIsHidden = "", cIndex = "", cQuotationEntryId = "";
                LeSXML.LineItem _item = new LeSXML.LineItem();
                List<string> sldet = parsedTblBody[i];

                cRowId = _htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "Id").GetAttributeValue("value", "");
                cItemNo = _htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "OrderBy").GetAttributeValue("value", "");
                cRequestForQuoteDetailId = _htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "RequestForQuoteDetailId").GetAttributeValue("value", "");
                cRequisitionDetailId = _htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "RequisitionDetailId").GetAttributeValue("value", "");
                cUnitCost = Convert.ToString(_htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "UnitCost").GetAttributeValue("value", ""));
                cDiscType = Convert.ToString(_htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "DiscountType").GetAttributeValue("value", ""));
                cDiscPercentage = Convert.ToString(_htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "DiscountPercentage").GetAttributeValue("value", ""));
                cDiscAmt = Convert.ToString(_htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "DiscountAmount").GetAttributeValue("value", ""));
                cTotDisc = Convert.ToString(_htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "TotalDiscount").GetAttributeValue("value", ""));
                cTotCost = Convert.ToString(_htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "TotalCostDisplay").GetAttributeValue("value", ""));
                cLeadDays = Convert.ToString(_htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "LeadTime").GetAttributeValue("value", ""));
                cItemComments = Convert.ToString(_htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "Comments").GetAttributeValue("value", ""));
                cIsAvailable = Convert.ToString(_htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "IsAvailable").GetAttributeValue("value", ""));
                cIsSelectedInComparison = Convert.ToString(_htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "IsSelectedInComparison").GetAttributeValue("value", ""));
                cQuotationEntryId = Convert.ToString(_htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "QuotationEntryId").GetAttributeValue("value", ""));
                cIsHidden = Convert.ToString(_htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "IsHidden").GetAttributeValue("value", ""));
                cIndex = Convert.ToString(_htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "Index").GetAttributeValue("value", ""));

                for (int j = 0; j < sldet.Count; j++)
                {
                    switch (slhdr[j])
                    {
                        case "Item": cItemName = sldet[j].Split('|')[0];
                            break;
                        case @"Parent item \ Category": cItemCategory = sldet[j].Split('|')[0];
                            break;
                        case "Part number": cItemRef = sldet[j].Split('|')[0];
                            break;
                        case "Make": cMaker = sldet[j].Split('|')[0];
                            break;
                        case "Req Qty": cReqQty = (!string.IsNullOrEmpty(sldet[j].Split('|')[0])) ? sldet[j].Split('|')[0] : "0";
                            break;
                        case "RFQ Qty": cRfqQty = (!string.IsNullOrEmpty(sldet[j].Split('|')[0])) ? sldet[j].Split('|')[0] : "0";
                            break;
                        case "Measure": cUOM = sldet[j].Split('|')[0];
                            break;
                        case "Req comments": cReqComments = sldet[j].Split('|')[0];
                            break;
                        case "RFQ comments": cRfqComments = sldet[j].Split('|')[0];
                            break;
                        default: break;
                    }
                }
                string _UOM = "";
                foreach (LineItem _items in _LineItems)
                {
                    if (_items.Number == cItemNo)
                    {
                        string Unit = _UOM = _items.MeasureUnitQualifier;
                        double Qty = _items.Quantity;
                        foreach (PriceDetails _priceDetails in _items.PriceList)
                        {
                            if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.GRP) cUnitCost = convert.ToString(_priceDetails.Value);
                            else if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.DPR) cDiscAmt = convert.ToString(_priceDetails.Value);
                        }
                        cTotCost = convert.ToString(_items.MonetaryAmount);
                        if (!string.IsNullOrEmpty(Unit))
                        {
                            if (cUOM.ToUpper() == Unit.ToUpper())
                            {
                                _UOM = cUOM;
                                break;
                            }
                        }
                        else
                        {
                            throw new Exception("The Measure unit field is required.");
                        }
                    }
                    cItemComments = "Unit : " + _items.MeasureUnitQualifier + Environment.NewLine + _items.LineItemComment.Value;
                }

                cDiscAmt = Convert.ToString(_htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "DiscountAmount").GetAttributeValue("value", ""));
                cTotDisc = Convert.ToString(_htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "TotalDiscount").GetAttributeValue("value", ""));
                cTotCost = Convert.ToString(_htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "TotalCostDisplay").GetAttributeValue("value", ""));
                cLeadDays = Convert.ToString(_htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "LeadTime").GetAttributeValue("value", ""));
                cItemComments = Convert.ToString(_htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "Comments").GetAttributeValue("value", ""));
                cIsAvailable = Convert.ToString(_htmlDoc.GetElementbyId("ProcurementQuotationEntryDetails" + (i + 1) + "IsAvailable").GetAttributeValue("value", ""));

                cResult += "" + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"ProcurementQuotationEntryDetails[" + (i + 1) + "].OrderBy\"" + Environment.NewLine + Environment.NewLine +

                cItemNo + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"ProcurementQuotationEntryDetails[" + (i + 1) + "].Id\"" + Environment.NewLine + Environment.NewLine +

                cRowId + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"ProcurementQuotationEntryDetails[" + (i + 1) + "].RequestForQuoteDetailId\"" + Environment.NewLine + Environment.NewLine +

                cRequestForQuoteDetailId + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"ProcurementQuotationEntryDetails[" + (i + 1) + "].RequisitionDetailId\"" + Environment.NewLine + Environment.NewLine +

                cRequisitionDetailId + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"ProcurementQuotationEntryDetails[" + (i + 1) + "].IsSelectedInComparison\"" + Environment.NewLine + Environment.NewLine +

               cIsSelectedInComparison + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"ProcurementQuotationEntryDetails[" + (i + 1) + "].IsHidden\"" + Environment.NewLine + Environment.NewLine +

                cIsHidden + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"ProcurementQuotationEntryDetails[" + (i + 1) + "].QuotationEntryId\"" + Environment.NewLine + Environment.NewLine +

                cQuotationEntryId + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"ProcurementQuotationEntryDetails[" + (i + 1) + "].Index\"" + Environment.NewLine + Environment.NewLine +

                cIndex + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"ProcurementQuotationEntryDetails.Index\"" + Environment.NewLine + Environment.NewLine +

                cIndex + Environment.NewLine +
                //"------" + boundary + Environment.NewLine +
                //"Content-Disposition: form-data; name=\"ProcurementQuotationEntryDetails[" + (i + 1) + "].RequestForQuoteDetail.MeasureUnit\"" + Environment.NewLine + Environment.NewLine +

                //_UOM + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"ProcurementQuotationEntryDetails[" + (i + 1) + "].IsAvailable\"" + Environment.NewLine + Environment.NewLine +

                cIsAvailable + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"ProcurementQuotationEntryDetails[" + (i + 1) + "].UnitCost\"" + Environment.NewLine + Environment.NewLine +

                cUnitCost + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"ProcurementQuotationEntryDetails[" + (i + 1) + "].DiscountType\"" + Environment.NewLine + Environment.NewLine +

                "" + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"ProcurementQuotationEntryDetails[" + (i + 1) + "].DiscountPercentage\"" + Environment.NewLine + Environment.NewLine +

                cDiscPercentage + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"ProcurementQuotationEntryDetails[" + (i + 1) + "].DiscountAmount\"" + Environment.NewLine + Environment.NewLine +

                cDiscAmt + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"ProcurementQuotationEntryDetails[" + (i + 1) + "].TotalDiscount\"" + Environment.NewLine + Environment.NewLine +

                cTotDisc + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"ProcurementQuotationEntryDetails[" + (i + 1) + "].TotalCostDisplay\"" + Environment.NewLine + Environment.NewLine +

                cTotCost + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"ProcurementQuotationEntryDetails[" + (i + 1) + "].LeadTime\"" + Environment.NewLine + Environment.NewLine +

                LeadDays + Environment.NewLine +
                "------" + boundary + Environment.NewLine +
                "Content-Disposition: form-data; name=\"ProcurementQuotationEntryDetails[" + (i + 1) + "].Comments\"" + Environment.NewLine + Environment.NewLine +
                cItemComments + Environment.NewLine;
            }
            //cResult = _sdata;
            return cResult;
        }

        private void ReadAttachments(string fXML)
        {
            string cAttachPath = "";
            try
            {
                DocHeader obj = _interchange.DocumentHeader;
                cAttachPath = strPDFAttachPath;
                string _outstr = "", cFileName = "", cAttchNode = "";
                int n = 0;
                n = _xmlRoutines.GetNodeCount(fXML, @"MTML/Interchange/" + obj.DocType + "/Attachment");
                if (n > 0) cAttchNode = @"MTML/Interchange/" + obj.DocType + "/Attachment";
                else
                {
                    n = _xmlRoutines.GetNodeCount(fXML, @"MTML/Interchange/Quote/Attachment");
                    cAttchNode = @"MTML/Interchange/Quote/Attachment";
                }

                Dictionary<string, string> _Attachments = new Dictionary<string, string>();

                for (int i = 0; i < n; i++)
                {
                    FileConverter _fileConvert = new FileConverter();
                    _Attachments = _xmlRoutines.GetNodeData(fXML, cAttchNode, i);

                    _Attachments.TryGetValue("Attachment", out _outstr);
                    if (!string.IsNullOrEmpty(_outstr)) { _fileConvert.Base64 = _outstr; }

                    _Attachments.TryGetValue("FileName", out _outstr);
                    if (string.IsNullOrEmpty(_outstr))
                        _Attachments.TryGetValue("Filename", out _outstr);

                    if (!string.IsNullOrEmpty(_outstr))
                    {
                        cFileName = convert.ToFileName(_outstr.Trim());
                        cFileName = Path.GetFileNameWithoutExtension(cAttachPath + "\\" + cFileName) + "_" + DateTime.Now.ToString("yyMMdd_HHmmssfff") + Path.GetExtension(cAttachPath + "\\" + cFileName);
                        _fileConvert.FileName = cAttachPath + "\\" + cFileName;
                    }

                    _Attachments.TryGetValue("format", out _outstr);
                    if (!string.IsNullOrEmpty(_outstr)) { _fileConvert.Format = _outstr; }

                    if (_fileConvert.ExportBase64ToFile())
                    {
                        if (i == 0) obj.OriginalFile = cFileName;
                        else if (i == 1) obj.Attachment1 = cFileName;
                        else if (i == 2) obj.Attachment2 = cFileName;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Add_Attachment()
        {
            bool _result = false;
            try
            {
                if (_dotwrapper.LoadUrl(_MessageNumber, "Number", false, false))
                {
                    _dotwrapper.browser.DialogHandler = new CustomDialogHandler((Control)_dotwrapper.browserView, WindowsFormsSynchronizationContext.Current, strPDFAttachPath + "\\" + _OriginalFile);
                    if (!_dotwrapper.browser.GetHTML().Contains(Path.GetFileNameWithoutExtension(_OriginalFile)))
                    {                       
                        List<DOMElement> elements = GetElementsbyTagNameAndAttribute("label", "for", "Attachments",_dotwrapper);
                        DOMElement element = elements[0].GetElementByTagName("a");
                        element.Focus();
                        KeyParams paramers_A = new KeyParams(VirtualKeyCode.RETURN, ' ');
                        _dotwrapper.browser.KeyDown(paramers_A);
                        _dotwrapper.browser.KeyUp(paramers_A);
                        Thread.Sleep(6000);
                        if (_dotwrapper.ClickElementbyName("Save", false, "Number", false, false))
                        {
                            Thread.Sleep(2000);
                            if (_dotwrapper.browser.GetHTML().Contains(Path.GetFileNameWithoutExtension(_OriginalFile)))
                            {
                                _result = true;
                                LogText ="Attachment saved successfully.";
                            }
                            else
                            {
                                _result = false;
                                LogText ="Unable to attach attachment.";
                            }
                            string screenShotPath = PrintScreenPath + "\\" + convert.ToFileName(_Refno + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssff") + ".pdf");
                            _dotwrapper.PrintPDF(PrintScreenPath,true);
                        }
                    }
                    else
                    {
                        _result = true;
                       LogText ="Attachment already added.";
                    }
                }
                else
                {
                    throw new Exception("Unable to load URL : " + _MessageNumber);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error on Add_Attachment : " + ex.Message);
            }
            return _result;
        }

     
        #endregion

        #region Common Functions

        public DateTime FormatMTMLDate(string DateValue)
        {
            int year = 0, Month = 0, day = 0;
            DateTime Dt = DateTime.MinValue;
            if (DateValue != null && DateValue != "")
            {
                if (DateValue.Length > 5)
                {
                    string[] _arr=DateValue.Split('/');
                    if(_arr.Length > 0)
                    
                        int.TryParse(_arr[0], out day);
                        int.TryParse(_arr[1], out Month);
                        int.TryParse(_arr[2], out year);
                       // if (_arr.Length == 4) {year = _arr}
                        //int year = Convert.ToInt32(DateValue.Substring(0, 4));
                        //int Month = Convert.ToInt32(DateValue.Substring(4, 2));
                        //int day = Convert.ToInt32(DateValue.Substring(6, 2));
                        Dt = new DateTime(year, Month, day);
                
                }
            }
            return Dt;
        }

        public DateTime Format_QteMTMLDate(string DateValue)
        {
            int year = 0, Month = 0, day = 0;
            DateTime Dt = DateTime.MinValue;
            if (DateValue != null && DateValue != "")
            {
                if (DateValue.Length > 5)
                {
                    year = Convert.ToInt32(DateValue.Substring(0, 4));
                    Month = Convert.ToInt32(DateValue.Substring(4, 2));
                    day = Convert.ToInt32(DateValue.Substring(6, 2));
                    Dt = new DateTime(year, Month, day);
                }
            }
            return Dt;
        }

        private string GetValues(string NodePath)
        {
            string cValue = "";
            var options = _htmlDoc.DocumentNode.SelectNodes(NodePath);
            if (options!=null && options.Count > 0)
            {
                for (int i = 0; i < options.Count; i++)
                {
                    var val = options[i].OuterHtml;
                    if (val.Contains("selected"))
                    {
                        cValue = options[i].InnerHtml;
                        break;
                    }
                }
            }
            return cValue;
        }

        public List<string> GetAction_Results(eActions eAction, string doctype)
        {
            string sDoneFile = "";
            List<string> slDetails = new List<string>();
            switch (eAction)
            {
                case eActions.RFQ: sDoneFile = AppDomain.CurrentDomain.BaseDirectory + AuditName + "_" + doctype + "_Downloaded.txt"; break;
                case eActions.PO: sDoneFile = AppDomain.CurrentDomain.BaseDirectory + AuditName + "_" + doctype + "_Downloaded.txt"; break;
                default: break;
            }
            if (File.Exists(sDoneFile))
            {
                string[] _Items = File.ReadAllLines(sDoneFile);
                slDetails.AddRange(_Items.ToList());
            }
            return slDetails;
        }

        private string GetNodedetails(HtmlNodeCollection _arrNodes)
        {
            string _str = "";
            if (_arrNodes != null && _arrNodes.Count > 0)
            {
                for (int i = 0; i < _arrNodes.Count; i++)
                {
                    if (_arrNodes[i].Name != "#text")
                    {
                        _str += _arrNodes[i].InnerText + "|";
                    }
                }
            }
            return _str.TrimEnd('|');
        }

        private void Print_ScreenShot(HtmlAgilityPack.HtmlDocument _htmlDoc, string cScreen)
        {
            if (_htmlDoc != null)
            {
                string cScreenShot = PrintScreenPath + "\\GreenWave_" + cScreen + "_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".html";
                foreach (HtmlNode link in _htmlDoc.DocumentNode.SelectNodes("//link"))
                {
                    string _attr = link.GetAttributeValue("href", "");
                    if (!_attr.Contains("http")) { link.SetAttributeValue("href", SITEURL + _attr.TrimStart('/')); }
                }
                foreach (HtmlNode atag in _htmlDoc.DocumentNode.SelectNodes("//a"))
                {
                    string _attr = atag.GetAttributeValue("href", "");
                    if (!_attr.Contains("http")) { atag.SetAttributeValue("href", SITEURL + _attr.TrimStart('/')); }
                }
                foreach (HtmlNode atag in _htmlDoc.DocumentNode.SelectNodes("//img"))
                {
                    string _attr = atag.GetAttributeValue("src", "");
                    if (!_attr.Contains("http")) { atag.SetAttributeValue("src", SITEURL + _attr.TrimStart('/')); }
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
            DateTime.TryParseExact(cDate.Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out dttemp);
            if (dttemp != DateTime.MinValue)
            {
                if (dttemp < DateTime.Now) dttemp = DateTime.Now;
                cResult = dttemp.Month + "/" + (dttemp.Day + 1) + "/" + dttemp.Year;
            }
            return cResult;
        }

        private string GetURL(string emlFile)
        {
            string _url = "", cPO_Key = "", cRFQ_Key = "";
            try
            {
                cPO_Key = Convert.ToString(ConfigurationManager.AppSettings["SEARCH_PO_KEY"]);
                cRFQ_Key = Convert.ToString(ConfigurationManager.AppSettings["SEARCH_RFQ_KEY"]);
                _txtData.Text = File.ReadAllText(emlFile);
                if (cDomain_Name != "" && _txtData.Text.Contains(cDomain_Name))
                {
                    for (int i = 0; i < _txtData.Lines.Length; i++)
                    {
                        string line = _txtData.Lines[i];
                        if (line.Contains(cDomain_Name))
                        {
                            _url = SearchUrl(line, cDomain_Name, cRFQ_Key); 
                            if (string.IsNullOrEmpty(_url))
                            {
                                _url = SearchUrl(line, cDomain_Name, cPO_Key); this.DocType = "PO";
                            }
                            else
                            {
                                this.DocType = "RFQ";
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _url;
        }

        private string SearchUrl(string line, string cDomain_Name,string cSearchKey)
        {
            string _url = "";
            string[] _arr = cSearchKey.Split(',');
            if (_arr != null && _arr.Length > 0)
            {
                for (int l = 0; l < _arr.Length; l++)
                {
                    if (line.Contains(cDomain_Name + "/" + _arr[l]))
                    {
                        //get URL
                        int startIndex = line.IndexOf(cDomain_Name + "/" + _arr[l]);
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
                        break;
                    }
                }
            }
            _url = _url.Replace("\"", "");
            return _url;
        }

        public void WriteErrorLog_With_Screenshot(string AuditMsg, string Filename,string VRNO)
        {
            LogText = AuditMsg;
            if (!AuditMsg.Contains("(404) Not Found"))
            {
                string eFile = PrintScreenPath + "\\" + AuditName + "_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
                if (!PrintScreen(eFile)) eFile = ""; string Err_file = (!string.IsNullOrEmpty(eFile)) ? eFile : Path.GetFileName(Filename);
                {
                    CreateAuditFile(Err_file, AuditName + "_HTTP_" + this.DocType, VRNO, "Error", AuditMsg, BuyerCode, SupplierCode, AuditPath);
                }
            }
            else
            {
                CreateAuditFile(Path.GetFileName(Filename), AuditName + "_HTTP_" + this.DocType, VRNO, "Error", AuditMsg, BuyerCode, SupplierCode, AuditPath);
            }
            MoveFiles(Filename, QuotePath + "\\Error\\" + Path.GetFileName(Filename));
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

        public bool DownloadFile(string RequestURL, string DownloadFileName, string ContentType = "Application/pdf")
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

        //private void MoveFiles(string Source_FilePath, string Destnation_FilePath)
        //{
        //    try
        //    {
        //        if (!Directory.Exists(Path.GetDirectoryName(Destnation_FilePath)))
        //        {
        //            Directory.CreateDirectory(Path.GetDirectoryName(Destnation_FilePath));
        //        }
        //        if (File.Exists(Destnation_FilePath))
        //        {
        //            File.Delete(Destnation_FilePath);
        //        }
        //        if (!File.Exists(Destnation_FilePath))
        //        {
        //            File.Move(Source_FilePath, Destnation_FilePath);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogText ="Error on MoveFiles : " + ex.Message;
        //    }
        //}

        private string GetIntegerValue(string Value)
        {
           return (!string.IsNullOrEmpty(Value))?Value.Split('.')[0]:"0";
        }

        public List<DOMElement> GetElementsbyTagNameAndAttribute(string TagName, string AttributeName, string AttrValue, NetBrowser _dotwrapper)
        {
            List<DOMElement> _ListElements = new List<DOMElement>();
            foreach (DOMNode _node in _dotwrapper.CurrentDocument.GetElementsByTagName(TagName))
            {
                DOMElement _eleNode = (DOMElement)_node;
                if (_eleNode.Attributes.ContainsKey(AttributeName))
                {
                    if (Convert.ToString(_eleNode.Attributes[AttributeName]) == AttrValue)
                    {
                        _ListElements.Add(_eleNode);
                    }
                }
            }
            return _ListElements;
        }   

        #endregion
    }

    class JSonSubmitResult
    {
        public Message Message { get; set; }
        public string ItemId { get; set; }
        public string ItemTitle { get; set; }
        public string redirect { get; set; }
        public bool forceRedirect { get; set; }
        public bool autoclose { get; set; }
        public object changedValuesAfterSave { get; set; }
        public bool result { get; set; }
        public int errorCode { get; set; }
        public object description { get; set; }
        public object data { get; set; }
    }

    public class Message
    {
        public int Type { get; set; }
        public string Value { get; set; }
    }


    public class CustomDialogHandler : DialogHandler
    {
        private WinFormsDefaultDialogHandler handler;
        private Control control;
        private SynchronizationContext synchronizationContext;
        public CloseStatus _closeStatus = CloseStatus.OK;
        string strFileName = "";

        public CustomDialogHandler(Control control, System.Threading.SynchronizationContext synchronizationContext)
        {
            this.control = control;
            this.synchronizationContext = synchronizationContext;
            handler = new WinFormsDefaultDialogHandler(control);
        }

        public CustomDialogHandler(Control control, System.Threading.SynchronizationContext synchronizationContext, string _filename)
        {
            this.control = control;
            this.synchronizationContext = synchronizationContext;
            handler = new WinFormsDefaultDialogHandler(control);
            strFileName = _filename;
        }

        public void OnAlert(DialogParams parameters)
        {
            synchronizationContext.Send(new SendOrPostCallback(
               delegate(object state)
               {
                   String url = parameters.Url;
                   String title = "Message from \"" + url + "\"";
                   String message = parameters.Message;
                   MessageBox.Show(control, message, title);
               }), parameters);
        }

        public CloseStatus OnBeforeUnload(UnloadDialogParams parameters)
        {
            return handler.OnBeforeUnload(parameters);
        }

        public CloseStatus OnConfirmation(DialogParams parameters)
        {
            return handler.OnConfirmation(parameters);
        }

        public CloseStatus OnFileChooser(FileChooserParams parameters)
        {
            parameters.SelectedFiles = strFileName;
            parameters.Status = _closeStatus;
            return CloseStatus.OK;
        }

        public CloseStatus OnPrompt(PromptDialogParams parameters)
        {
            return handler.OnPrompt(parameters);
        }

        public CloseStatus OnReloadPostData(ReloadPostDataParams parameters)
        {
            return handler.OnReloadPostData(parameters);
        }

        public CloseStatus OnColorChooser(ColorChooserParams parameters)
        {
            return handler.OnColorChooser(parameters);
        }

        public CloseStatus OnSelectCertificate(CertificatesDialogParams parameters)
        {
            return handler.OnSelectCertificate(parameters);
        }
    }

}


   //"Content-Disposition: form-data; name=\"quotationenteredby\"" + Environment.NewLine + Environment.NewLine +

   //             "---" + Environment.NewLine +
   //             "------" + boundary + Environment.NewLine +
   //             "Content-Disposition: form-data; name=\"quotationcomments\"" + Environment.NewLine + Environment.NewLine +

   //             "------" + boundary + Environment.NewLine +