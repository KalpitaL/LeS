using DotNetBrowser;
using DotNetBrowser.DOM;
using DotNetBrowserWrapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Cells;
using System.Globalization;
using MTML.GENERATOR;


namespace PacificBasinRoutine
{
    public class PacificRoutine
    {
        public NetBrowser _netWrapper = new NetBrowser();
        PageVariables pagevar = new PageVariables();
        RichTextBox _txtData = new RichTextBox();
        string strLogPath = "", strAuditPath = "", strProcessorName = "", strScreenShotPath = "", strSupplier = "", strBuyer = "", strAttachmentPath = "",
            strBuyerName = "", strRFQPath = "", strExcelPath = "", strMailTextPath = "", strMTMLUploadPath = "", strDocType = "", strMessageNumber = "",
            strCurrency = "", strMsgNumber = "", strMsgRefNumber = "", strUCRefNo = "", strAAGRefNo = "", strLesRecordID = "", BuyerName = "", strBuyerPhone = "", strBuyerEmail = "", strBuyerFax = "",
            strSupplierName = "", strSupplierPhone = "", strSupplierEmail = "", strSupplierFax = "", strPortName = "", strPortCode = "", strVesselName = "", strSupplierComment = "", strPayTerms = "",
            strPackingCost = "", strFreightCharge = "", strTotalLineItemsAmount = "", strGrandTotal = "", strAllowance = "", strDelvDate = "", strExpDate = "", strLeadDays = "", strVendorEmail = "";
        int WaitPeriod = 0, Retry = 0, Maxtry = 2, IsUOMChanged = 0, IsPriceAveraged = 0, IsAltItemAllowed = 0, WaitPeriodRFQTable = 0, count = 0;
        bool IsDecline = false, isSaveQuote = true;
        double AdditionalDiscount = 0;
        List<string> mailFiles = new List<string>();
        List<string> xmlFiles = new List<string>();
        public MTMLInterchange _interchange { get; set; }
        public LineItemCollection _lineitem = null;
        public LeSXML.LeSXML _lesXml = new LeSXML.LeSXML();
        public string strBuyerCode { get; set; }
        public string strSupplierCode { get; set; }

        public PacificRoutine()
        {
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Total.lic");
        }

        public void LoadAppSettings()
        {

            strAuditPath = ConfigurationManager.AppSettings["AuditPath"].Trim();
            WaitPeriod = Convert.ToInt32(ConfigurationManager.AppSettings["WaitPeriod"].Trim());
            strProcessorName = ConfigurationManager.AppSettings["ProcessorName"].Trim();
            strSupplier = ConfigurationManager.AppSettings["Supplier"].Trim();
            strBuyer = ConfigurationManager.AppSettings["Buyer"].Trim();
            strBuyerName = ConfigurationManager.AppSettings["BuyerName"].Trim();
            strRFQPath = ConfigurationManager.AppSettings["RFQPath"].Trim();
            strAttachmentPath = ConfigurationManager.AppSettings["RFQ_AttachmentPath"].Trim();
            strMailTextPath = ConfigurationManager.AppSettings["MailTextFilePath"].Trim();
            strMTMLUploadPath = ConfigurationManager.AppSettings["Mtml_Quote_UploadPath"];
            WaitPeriodRFQTable = Convert.ToInt32(ConfigurationManager.AppSettings["WaitPeriod_RFQTable"].Trim());
            isSaveQuote = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveQuotation"].Trim());

            strLogPath = AppDomain.CurrentDomain.BaseDirectory + "Log";
            if (strAuditPath == "") strAuditPath = AppDomain.CurrentDomain.BaseDirectory + "Audit";
            if (strRFQPath == "") strRFQPath = AppDomain.CurrentDomain.BaseDirectory + "XML";
            strScreenShotPath = AppDomain.CurrentDomain.BaseDirectory + "ScreenShots";
            if (!Directory.Exists(strScreenShotPath)) Directory.CreateDirectory(strScreenShotPath);
            if (strAttachmentPath == "") strAttachmentPath = AppDomain.CurrentDomain.BaseDirectory + "Attachments";
            if (!Directory.Exists(strAttachmentPath)) Directory.CreateDirectory(strAttachmentPath);
            strExcelPath = AppDomain.CurrentDomain.BaseDirectory + "Excel";
            if (!Directory.Exists(strExcelPath)) Directory.CreateDirectory(strExcelPath);
            if (strMailTextPath == "") strMailTextPath = Application.StartupPath + "\\MailTextFile";
            if (strMTMLUploadPath == "") strMTMLUploadPath = Application.StartupPath + "\\MTML_Quote_Upload";
            if (!Directory.Exists(strMTMLUploadPath)) Directory.CreateDirectory(strMTMLUploadPath);
            if (!Directory.Exists(strMTMLUploadPath + "\\Backup")) Directory.CreateDirectory(strMTMLUploadPath + "\\Backup");
            if (!Directory.Exists(strMTMLUploadPath + "\\Error")) Directory.CreateDirectory(strMTMLUploadPath + "\\Error");
        }

        #region RFQ

        public void Read_MailTextFiles()
        {
            try
            {
                LoadAppSettings();
                _netWrapper.LogText = "";
                _netWrapper.LogText = "RFQ processing started.";
                GetMailTextFiles();
                if (mailFiles.Count > 0)
                {
                    for (int j = mailFiles.Count - 1; j >= 0; j--)
                    {
                        Dictionary<string, string> dicURLToken = GetURL(mailFiles[j]);
                        if (dicURLToken != null && dicURLToken.Count > 0)
                        {

                            if (dicURLToken["URL"] != "" && dicURLToken["URL"].Contains("https://supplierportal.dnvgl.com/") && dicURLToken["Token"] != "")
                            {
                                if (NavigateToURL(dicURLToken["URL"].Trim(), dicURLToken["Token"].Trim(), mailFiles[j], strMailTextPath))
                                {
                                    DownloadRFQ(dicURLToken["URL"].Trim(), dicURLToken["Token"].Trim(), mailFiles[j]);
                                    _netWrapper.browser.LoadURL("about:blank"); Thread.Sleep(1500);
                                }
                                else
                                {
                                    _netWrapper.LogText = "Unable to navigate to URL for mail file " + mailFiles[j];
                                }
                            }
                            else
                            {
                                string strMsg = "";
                                if (dicURLToken["URL"] == "") strMsg = "URL not found for navigation for " + mailFiles[j];
                                else if (dicURLToken["Token"] == "") strMsg = "Token not found for " + mailFiles[j];
                                _netWrapper.LogText = strMsg;
                                CreateAuditFile(mailFiles[j], strProcessorName, "", "Error", strMsg);
                                if (File.Exists(strMailTextPath + "\\Error\\" + Path.GetFileName(mailFiles[j]))) File.Delete(strMailTextPath + "\\Error\\" + Path.GetFileName(mailFiles[j]));
                                File.Move(mailFiles[j], strMailTextPath + "\\Error\\" + Path.GetFileName(mailFiles[j]));
                            }
                        }
                    }
                }
                else _netWrapper.LogText = "No file found.";
                _netWrapper.LogText = "RFQ processing stopped.";
            }
            catch (Exception ex)
            {
                string sFile = strScreenShotPath + "\\PacificB_RFQ" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".pdf";
                _netWrapper.PrintPDF(sFile, false);
                _netWrapper.LogText = "Exception while processing RFQ : " + ex.GetBaseException().ToString();
            }
        }

        public void GetMailTextFiles()
        {
            mailFiles.Clear();
            DirectoryInfo _dir = new DirectoryInfo(strMailTextPath);
            FileInfo[] _Files = _dir.GetFiles();
            foreach (FileInfo _MtmlFile in _Files)
            {
                mailFiles.Add(_MtmlFile.FullName);
            }
            Directory.CreateDirectory(strMailTextPath + "\\Error");
            Directory.CreateDirectory(strMailTextPath + "\\Backup");
        }

        private Dictionary<string, string> GetURL(string emlFile)
        {
            string strUrl = "", strToken = "";
            Dictionary<string, string> dicData = new Dictionary<string, string>();
            try
            {
                _netWrapper.LogText = "Reading file " + Path.GetFileName(emlFile);

                _txtData.Text = File.ReadAllText(emlFile);

                _txtData.Text = _txtData.Text.Replace("%2F", "//");
                _txtData.Text = _txtData.Text.Replace("%3F", "?");
                _txtData.Text = _txtData.Text.Replace("%3A", ":");
                _txtData.Text = _txtData.Text.Replace("%40", "@");
                _txtData.Text = _txtData.Text.Replace("%3D", "=");
                _txtData.Text = _txtData.Text.Replace("%26", "&");

                if (_txtData.Text.Contains("https://supplierportal.dnvgl.com/"))
                {
                    for (int i = 0; i < _txtData.Lines.Length; i++)
                    {
                        string line = _txtData.Lines[i];
                        if (line.Contains("https://supplierportal.dnvgl.com/"))
                        {
                            //get URL
                            int startIndex = line.IndexOf("https://supplierportal.dnvgl.com/");
                            if (line.Length > startIndex)
                            {
                                strUrl = line.Substring(startIndex);
                            }
                            else strUrl = line.Trim();

                            int endIndex = strUrl.IndexOf(">");
                            if (line.Length > endIndex)
                            {
                                strUrl = strUrl.Substring(0, endIndex);
                            }
                            else strUrl = line.Trim();

                            //get Token
                            int tstartIndex = line.IndexOf("token");
                            if (line.Length > tstartIndex)
                            {
                                strToken = line.Substring(tstartIndex);
                                strToken = strToken.Substring(6);
                                strToken.Trim();
                            }

                            break;
                        }
                    }
                }
                strUrl = strUrl.Trim().Replace("&amp;", "&").Trim().TrimStart('<').TrimStart('"').TrimEnd('>').TrimEnd('"').Trim();
                dicData.Add("URL", strUrl);
                dicData.Add("Token", strToken);
            }
            catch (Exception ex)
            {
                _netWrapper.LogText = "Exception while getting URL for RFQ : " + ex.GetBaseException().ToString();
            }
            return dicData;
        }

        public bool NavigateToURL(string strURL, string strToken, string strFileName, string strMailTextPath)
        {
            bool loaded = false, isLoggedIn = false;
            try
            {
                loaded = _netWrapper.LoadUrl(strURL, pagevar.txtPassword, false, true);
                if (loaded)
                {
                    DOMInputElement _inputPass = (DOMInputElement)_netWrapper.GetElementByType("input", pagevar.txtPassword);
                    _inputPass.Focus();
                    Thread.Sleep(300);
                    for (int j = 0; j < strToken.Length; j++)
                    {
                        SetAlphaNumeric(strToken[j]);
                        Thread.Sleep(200);
                    }
                    DOMElement _elebtnLogin = _netWrapper.GetElementByType("button", pagevar.btnLogin, "type");
                    if (_elebtnLogin != null)
                    {
                        _elebtnLogin.Click();
                        if (WaitPeriodRFQTable != 0)
                            Thread.Sleep(WaitPeriodRFQTable);
                        else
                            Thread.Sleep(7000);
                        isLoggedIn = loading();
                    }

                    if (isLoggedIn && Retry == 0)
                    { _netWrapper.LogText = "Login Successfully."; }
                    else if (isLoggedIn && Retry > 0)
                    { _netWrapper.LogText = "Login Successfully."; }
                    else if (Retry > Maxtry)
                    {
                        SetLoginError(strFileName, strMailTextPath);
                    }
                    else
                    {
                        DOMElement _eleErrMsg=  _netWrapper.GetElementByType("div","margin:100px;", "style");//18-12-2017
                        if (_eleErrMsg != null)
                        {
                            var _h2 = _eleErrMsg.GetElementsByTagName("h2");
                            if (_h2.Count == 1)
                            {
                                if (_h2[0].TextContent.Contains("request could not be found or is no longer valid"))
                                {
                                    _netWrapper.LogText = _h2[0].TextContent;
                                    string sFile = strScreenShotPath + "\\PacificB_Login" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".pdf";
                                    _netWrapper.PrintPDF(sFile, false);
                                    CreateAuditFile(sFile, strProcessorName, "", "Error", _h2[0].TextContent);
                                    if (File.Exists(strMailTextPath + "\\Error\\" + Path.GetFileName(strFileName))) File.Delete(strMailTextPath + "\\Error\\" + Path.GetFileName(strFileName));
                                    File.Move(strFileName, strMailTextPath + "\\Error\\" + Path.GetFileName(strFileName));
                                }
                            }//Les@2015#

                        }
                        else
                        {
                            Retry++;
                            _netWrapper.LogText = "Login Retry.";
                            isLoggedIn = NavigateToURL(strURL, strToken, strFileName, strMailTextPath);
                        }
                    }

                }
                else
                {
                    if (Retry <= Maxtry)
                    {
                        Retry++;
                        _netWrapper.LogText = "Login Retry.";
                        isLoggedIn = NavigateToURL(strURL, strToken, strFileName, strMailTextPath);
                    }
                }
            }
            catch (Exception ex)
            {
                string sFile = strScreenShotPath + "\\PacificB_Login" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".pdf";
                _netWrapper.PrintPDF(sFile, false);
                _netWrapper.LogText = "Exception while LogIn : " + ex.GetBaseException().ToString();
            }
            return isLoggedIn;
        }

        public void DownloadRFQ(string strURL, string strToken, string MailTextFile)
        {
            string[] arrRFQNo;
            string strRFQNo = "";
            try
            {
                DOMElement dRFQNo = _netWrapper.GetElementbyId(pagevar.lblRFQNoId);
                arrRFQNo = dRFQNo.InnerText.Split(new string[] { " - " }, StringSplitOptions.None);
                string strExcelRFQ = Download_RFQAttachment(arrRFQNo[0]);

                if (GetRFQHeader(arrRFQNo[0], arrRFQNo[1], strURL, strToken))
                {
                    if (GetRFQItems(arrRFQNo[0], strExcelRFQ))
                    {
                        if (GetAddress(arrRFQNo[0]))
                        {
                            string lesXMLFile = "RFQ_" + arrRFQNo[0] + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xml";
                            if (GenerateXMLFile(arrRFQNo[0], lesXMLFile))
                            {
                                _netWrapper.LogText = "RFQ " + arrRFQNo[0] + " downloaded successfully.";
                                string Audit = "RFQ '" + lesXMLFile + "' for ref '" + arrRFQNo[0] + "' downloaded successfully.";
                                CreateAuditFile(lesXMLFile, strProcessorName, arrRFQNo[0], "Downloaded", Audit);
                                if (File.Exists(strMailTextPath + "\\Backup\\" + Path.GetFileName(MailTextFile))) File.Delete(strMailTextPath + "\\Backup\\" + Path.GetFileName(MailTextFile));
                                File.Move(MailTextFile, strMailTextPath + "\\Backup\\" + Path.GetFileName(MailTextFile));
                            }
                            else
                            {
                                string Audit = "Unable to download RFQ '" + lesXMLFile + "' for ref '" + arrRFQNo[0] + "'.";
                                CreateAuditFile(lesXMLFile, strProcessorName, arrRFQNo[0], "Downloaded", Audit);
                                string sFile = strScreenShotPath + "\\PacificB_RFQ" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".pdf";
                                _netWrapper.PrintPDF(sFile, false);
                                if (File.Exists(strMailTextPath + "\\Error\\" + Path.GetFileName(MailTextFile))) File.Delete(strMailTextPath + "\\Error\\" + Path.GetFileName(MailTextFile));
                                File.Move(MailTextFile, strMailTextPath + "\\Error\\" + Path.GetFileName(MailTextFile));
                            }
                        }
                        else
                        {
                            MoveRFQToError("Unable to get address details.", arrRFQNo[0], MailTextFile);
                        }
                    }
                    else
                    {
                        MoveRFQToError("Unable to get RFQ item details.", arrRFQNo[0], MailTextFile);
                    }
                }
                else
                {
                    MoveRFQToError("Unable to get RFQ header details.", arrRFQNo[0], MailTextFile);
                }

            }
            catch (Exception ex)
            {
                string sFile = strScreenShotPath + "\\PacificB_RFQ" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".pdf";
                _netWrapper.PrintPDF(sFile, false);
                _netWrapper.LogText = "Exception while processing RFQ Items : " + ex.GetBaseException().ToString();
                CreateAuditFile("", strProcessorName, strRFQNo, "Error", "Exception while processing RFQ Items : " + ex.GetBaseException().ToString());
            }
        }

        public string Download_RFQAttachment(string RFQNo)
        {
            string dwndFile = "";
            try
            {
                string FileName = "RFQ_" + RFQNo + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                DOMElement _eleExportExcel = _netWrapper.GetElementByType("button", pagevar.ClkExportExcel, "click.delegate");
                if (_eleExportExcel != null)
                {
                    dwndFile = _netWrapper.DownloadFileOnClick(_eleExportExcel, strExcelPath, FileName, false, "");
                }

                if (File.Exists(strExcelPath + "\\" + FileName))
                    _netWrapper.LogText = "RFQ XLS Attachment " + FileName + " downloaded for " + RFQNo;
                else
                    _netWrapper.LogText = " Unable to download RFQ XLS attachment " + FileName + " for " + RFQNo;
            }
            catch (Exception ex)
            {
                _netWrapper.LogText = "Exception while downloading attachment : " + ex.Message.ToString();
            }
            return dwndFile;
        }

        public void MoveRFQToError(string msg, string RFQNo, string MailTextFile)
        {
            CreateAuditFile("", strProcessorName, RFQNo, "Error", msg);
            string sFile = strScreenShotPath + "\\PacificB_RFQ" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".pdf";
            _netWrapper.PrintPDF(sFile, false);
            if (File.Exists(strMailTextPath + "\\Error\\" + Path.GetFileName(MailTextFile))) File.Delete(strMailTextPath + "\\Error\\" + Path.GetFileName(MailTextFile));
            File.Move(MailTextFile, strMailTextPath + "\\Error\\" + Path.GetFileName(MailTextFile));
        }

        public bool GetRFQHeader(string strRFQNo, string strRemarksTitle, string strURL, string strToken)
        {
            bool result = false;
            try
            {
                _netWrapper.LogText = "Start getting header details for RFQ with VRNo " + strRFQNo + ".";
                _lesXml.DocID = DateTime.Now.ToString("yyyyMMddhhmmss");
                _lesXml.Created_Date = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Doc_Type = "RFQ";
                _lesXml.Dialect = "Pacific_Basin";
                _lesXml.Version = "1";
                _lesXml.Date_Document = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Date_Preparation = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Sender_Code = strBuyer;
                _lesXml.Recipient_Code = strSupplier;
                _lesXml.DocReferenceID = strRFQNo;
                _lesXml.DocLinkID = strURL + "|" + strToken;

                string PrintFile = GetAttachment(strRFQNo);
                if (File.Exists(strAttachmentPath + "\\" + PrintFile))
                    _lesXml.OrigDocFile = PrintFile;

                _lesXml.Active = "1";

                DOMElement dVessel = _netWrapper.GetElementbyId(pagevar.lblVesselId);
                string strVessel = dVessel.InnerText;
                _lesXml.Vessel = strVessel;

                GetEquipmentDetails();

                _lesXml.BuyerRef = strRFQNo;
                DOMElement dPort = _netWrapper.GetElementbyId(pagevar.lblPortId);
                if (dPort != null)
                {
                    if (dPort.InnerText != "")
                    {
                        if (dPort.InnerText.Contains('/'))
                        {
                            int count = dPort.InnerText.Split('/').Length;
                            if (count > 2)
                            {
                                string strPortCode = dPort.InnerText.Split('/')[0].Trim();
                                string strPortName = dPort.InnerText.Split('/')[1].Trim();
                                _lesXml.PortCode = strPortCode;
                                _lesXml.PortName = strPortName;
                            }
                        }
                        else
                        {
                            string strPortCode = dPort.InnerText;
                            _lesXml.PortCode = strPortCode;
                        }
                    }
                }

                DOMElement dETADate = _netWrapper.GetElementbyId(pagevar.lbletaDateId);
                if (dETADate != null)
                {
                    string strEtaDate = dETADate.InnerText;
                    if (strEtaDate != "" && strEtaDate != "-")
                    {
                        DateTime etaDate = DateTime.ParseExact(strEtaDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        _lesXml.Date_ETA = etaDate.ToString("yyyyMMdd");
                    }
                }

                DOMElement dDelDate = _netWrapper.GetElementbyId(pagevar.lblDelDateId);
                if (dDelDate != null)
                {
                    string strDelDate = dDelDate.InnerText;
                    if (strDelDate != "-" && strDelDate != "")
                    {
                        DateTime delDate = DateTime.ParseExact(strDelDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        _lesXml.Date_Delivery = delDate.ToString("yyyyMMdd");
                    }
                }

                DOMElement dPlaceDel = _netWrapper.GetElementbyId(pagevar.lblPlaceDelId);
                if (dPlaceDel != null)
                {
                    string strPlaceDel = dPlaceDel.InnerText;
                    _lesXml.Remark_Header = "Place of Delivery: " + strPlaceDel;
                }
                _lesXml.Remark_Title = strRemarksTitle;

                _netWrapper.LogText = "Getting header details completed successfully.";
                result = true;
                return result;
            }
            catch (Exception ex) { _netWrapper.LogText = "Unable to get header details." + ex.GetBaseException().ToString(); result = false; return result; }
        }

        public void GetEquipmentDetails()
        {
            string strEquipment = "", strMaker = "", strSerial = "";
            try
            {
                _netWrapper.LogText = "Start getting equipment details.";
                DOMElement dEquipment = _netWrapper.GetElementbyId(pagevar.lblEquipNameId);
                if (dEquipment != null)
                    strEquipment = dEquipment.InnerText;

                DOMElement dMaker = _netWrapper.GetElementbyId(pagevar.lblMakerId);
                if (dMaker != null)
                    strMaker = dMaker.InnerText;

                DOMElement dSerial = _netWrapper.GetElementbyId(pagevar.lblSerialId);
                if (dSerial != null)
                    strSerial = dSerial.InnerText;

                if (strEquipment != "")
                    _lesXml.Equipment = strEquipment;
                if (strMaker != "")
                    _lesXml.EquipMaker = strMaker;
                if (strSerial != "")
                    _lesXml.EquipRemarks = "Serial No: " + strSerial;
                _netWrapper.LogText = "Getting equipment completed successfully.";
            }
            catch (Exception ex) { _netWrapper.LogText = "Unable to get equipment details." + ex.GetBaseException().ToString(); }
        }

        public bool GetRFQItems(string strRFQNo, string strExelRFQFile)
        {
            int LinItemCount = 0;
            bool result = false;
            try
            {
                if (File.Exists(strExcelPath + "\\" + strExelRFQFile))
                {
                    #region Read Excel RFQ
                    _lesXml.LineItems.Clear();
                    _netWrapper.LogText = "Start Getting LineItem details for RFQ with VRNo " + strRFQNo + ".";
                    Aspose.Cells.Workbook _workbook = new Aspose.Cells.Workbook(strExcelPath + "\\" + strExelRFQFile);
                    Aspose.Cells.Worksheet _worksheet = _workbook.Worksheets[0];
                    Aspose.Cells.Cells cells = _worksheet.Cells;
                    string strCurrency = cells[24, 8].StringValue;
                    _lesXml.Currency = strCurrency;

                    short _colItemName = Convert.ToInt16(ConfigurationManager.AppSettings["ItemName_Col"].Trim());
                    Aspose.Cells.Cell endCell = cells.EndCellInColumn(_colItemName);

                    int maxrow = endCell.Row;
                    for (int i = Convert.ToInt32(ConfigurationManager.AppSettings["ItemName_Row"].Trim()); i <= maxrow; i++)
                    {
                        if (cells[i, _colItemName].StringValue == "Item Name")
                            i++;
                        LeSXML.LineItem _item = new LeSXML.LineItem();
                        try
                        {
                            if (cells[i, _colItemName].StringValue.Trim() != "")
                            {
                                _item.Number = Convert.ToString(LinItemCount + 1);
                                _item.OrigItemNumber = Convert.ToString(LinItemCount + 1);
                                //_item.OriginatingSystemRef = Convert.ToString(LinItemCount + 1);//5-12-2017
                                _item.Name = cells[i, _colItemName].StringValue.Trim();
                                _item.ItemRef = cells[i, _colItemName + 2].StringValue.TrimStart(' ').TrimStart('\'');
                                i++;
                                _item.OriginatingSystemRef = cells[i, _colItemName + 2].StringValue.Trim().Split(':')[1];//5-12-2017
                                _item.Quantity = cells[i, _colItemName + 4].StringValue;
                                _item.Unit = cells[i, _colItemName + 5].StringValue;

                                string strDescr = "";
                                if (cells[i, _colItemName].StringValue.StartsWith("Description: ")) strDescr = cells[i, _colItemName].StringValue.Substring(13);
                                else strDescr = cells[i, _colItemName].StringValue;
                                _item.Remark = strDescr;

                                _item.Discount = "0";
                                _item.ListPrice = "0";
                                _item.LeadDays = "0";
                                _lesXml.LineItems.Add(_item);
                                LinItemCount++;
                            }
                            else break;
                        }
                        catch (Exception ex)
                        { _netWrapper.LogText = ex.GetBaseException().ToString(); }
                    }
                    _lesXml.Total_LineItems = Convert.ToString(LinItemCount);
                    if (cells[11, 6].StringValue.StartsWith("E-Mail: ")) strVendorEmail = cells[11, 6].StringValue.Substring(7);
                    else strVendorEmail = cells[11, 6].StringValue;
                    #endregion
                }
                else
                {
                    #region Read from site
                    _lesXml.LineItems.Clear();
                    _netWrapper.LogText = "Start Getting LineItem details for RFQ with VRNo " + strRFQNo + ".";
                    DOMElement _tblItems = _netWrapper.GetElementbyId(pagevar.tblItemId);
                    if (_tblItems != null)
                    {
                        DOMElement eletbody = _tblItems.GetElementByTagName("tbody");
                        if (eletbody != null)
                        {
                            List<DOMNode> eleRow = eletbody.GetElementsByTagName("tr");
                            if (eleRow.Count > 0)
                            {
                                foreach (DOMNode row in eleRow)
                                {
                                    DOMElement eRow = (DOMElement)row;
                                    LeSXML.LineItem _item = new LeSXML.LineItem();
                                    try
                                    {
                                        List<DOMNode> _data = eRow.GetElementsByTagName("td");
                                        if (_data.Count > 0)
                                        {
                                            _item.Number = Convert.ToString(LinItemCount + 1);
                                            _item.OrigItemNumber = Convert.ToString(LinItemCount + 1);
                                          //  _item.OriginatingSystemRef = Convert.ToString(LinItemCount + 1);//5-12-2017
                                              DOMElement _eleAddFields = _data[13].GetElementByTagName("button");//5-12-2017
                                              if (_eleAddFields != null)
                                              {
                                                  if (_netWrapper.ClickElementbyType(_eleAddFields, "textarea", pagevar.ItemSupRemarks, "value.bind"))
                                                  {
                                                      DOMElement _eleItemNo = _netWrapper.GetElementByType("div", "col-xs-8 indent23", "class");
                                                      if (_eleItemNo != null)
                                                      {
                                                          _item.OriginatingSystemRef = _eleItemNo.TextContent;
                                                      }
                                                  }
                                              }
                                            string strItemName = _data[1].TextContent.Trim();
                                            _item.Name = strItemName;
                                            string strItemRef = _data[2].TextContent.Trim();
                                            _item.ItemRef = strItemRef;
                                            DOMSelectElement PUnitInput = null;
                                            var uPUInput = _data[7].GetElementsByTagName("select");
                                            if (uPUInput.Count > 0)
                                            {
                                                PUnitInput = (DOMSelectElement)uPUInput[0];
                                                _item.Unit = PUnitInput.Value;
                                            }
                                            DOMInputElement PQtyInput = null;
                                            var uPQInput = _data[6].GetElementsByTagName("input");
                                            if (uPQInput.Count > 0)
                                            {
                                                PQtyInput = (DOMInputElement)uPQInput[0];
                                                _item.Quantity = PQtyInput.Value;
                                            }
                                            string _ReqQuantity = _data[3].TextContent.Trim();
                                            string _ReqUnit = _data[4].TextContent.Trim();
                                            _item.Remark = "Required Qty: " + _ReqQuantity + " " + "Required unit: " + _ReqUnit;
                                            _item.Discount = "0";
                                            _item.ListPrice = "0";
                                            _item.LeadDays = "0";
                                            _lesXml.LineItems.Add(_item);
                                            LinItemCount++;
                                        }
                                    }
                                    catch (Exception ex)
                                    { _netWrapper.LogText = ex.GetBaseException().ToString(); }
                                }
                                _lesXml.Total_LineItems = Convert.ToString(LinItemCount);
                            }
                        }
                    }
                    #endregion
                    DOMSelectElement _Currency = (DOMSelectElement)_netWrapper.GetElementbyId(pagevar.selCurrencyId);
                    if (_Currency != null)
                    {
                        _lesXml.Currency = _Currency.Value;
                    }
                }
                _netWrapper.LogText = "Getting LineItem details successfully";
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                _netWrapper.LogText = "Exception while getting RFQ Items: " + ex.GetBaseException().ToString();
                result = false;
                return result;
            }
        }

        public bool GetAddress(string strRFQNo)
        {
            bool result = false;
            try
            {
                _lesXml.Addresses.Clear();
                _netWrapper.LogText = "Start Getting address details for RFQ with VRNo " + strRFQNo + ".";
                LeSXML.Address _xmlAdd = new LeSXML.Address();

                _xmlAdd.Qualifier = "BY";
                _xmlAdd.AddressName = ConfigurationManager.AppSettings["BuyerName"].Trim();
                DOMElement _eleBuyer = _netWrapper.GetElementbyClass(pagevar.divBuyerCls);
                if (_eleBuyer != null)
                {
                    var p = _eleBuyer.GetElementsByTagName("p");
                    if (p.Count > 0)
                    {
                        _xmlAdd.ContactPerson = p[0].TextContent.Trim();
                        _xmlAdd.Phone = p[1].TextContent.Split(':')[1].Trim();
                        _xmlAdd.eMail = p[2].TextContent.Split(':')[1].Trim();
                    }
                }
                _lesXml.Addresses.Add(_xmlAdd);

                _xmlAdd = new LeSXML.Address();
                _xmlAdd.Qualifier = "VN";
                DOMElement _eleSupplier = _netWrapper.GetElementbyClass(pagevar.divSuppCls);
                if (_eleSupplier != null)
                {
                    var p = _eleSupplier.GetElementsByTagName("p");
                    if (p.Count > 0)
                    {
                        _xmlAdd.AddressName = p[0].TextContent.Trim();
                        _xmlAdd.Address1 = p[2].TextContent.Trim() + ", " + p[3].TextContent.Trim() + ", " + p[4].TextContent.Trim();
                    }
                    if (strVendorEmail != "")
                        _xmlAdd.eMail = strVendorEmail;
                }
                _lesXml.Addresses.Add(_xmlAdd);

                _netWrapper.LogText = "Getting address details successfully";
                result = true; return result;
            }
            catch (Exception ex)
            {
                _netWrapper.LogText = "Exception while getting address details: " + ex.GetBaseException().ToString(); result = false; return result;
            }
        }

        public bool GenerateXMLFile(string RFQNo, string lesXMLFile)
        {
            bool result = false;
            try
            {
                if (Convert.ToInt32(_lesXml.Total_LineItems) > 0)
                {
                    _lesXml.FileName = lesXMLFile;
                    _lesXml.WriteXML();
                    if (File.Exists(strRFQPath + "\\" + lesXMLFile)) result = true;
                    else result = false;
                }
            }
            catch (Exception ex) { _netWrapper.LogText = "Exception while generating XML file for VRNO: " + RFQNo + ex.GetBaseException().ToString(); }
            return result;
        }

        public string GetAttachment(string strRFQNo)
        {
            string strFileName = "RFQ_" + strRFQNo + ".pdf";

            try
            {
                _netWrapper.PrintPDF(strAttachmentPath + "\\" + strFileName, false);
                if (File.Exists(strAttachmentPath + "\\" + strFileName))
                    _netWrapper.LogText = "Attachment " + strFileName + " downloaded for " + strRFQNo;
                else
                    _netWrapper.LogText = " Unable to download attachment " + strFileName + " for " + strRFQNo;
            }
            catch (Exception ex)
            {
                _netWrapper.LogText = "Exception while downloading attachment : " + ex.Message.ToString();
            }
            return strFileName;
        }

        public bool loading()
        {
            bool isload = false; ; int maxcount = 60000, count = 0;
            while (count < maxcount)
            {
                DOMElement _eleItemTable = _netWrapper.GetElementbyId(pagevar.tblItemId);
                if (_eleItemTable != null)
                {
                    isload = true;
                    break;

                }
                count++;
            }
            return isload;
        }

        public void SetLoginError(string strFileName, string strMailTextPath)
        {
            string msg = "";
            DOMElement eleErrMsg = _netWrapper.GetElementbyId(pagevar.lblErrorId);
            if (eleErrMsg != null)
            {
                msg = eleErrMsg.InnerText;
                _netWrapper.LogText = "Login failed due to " + msg;
                string sFile = strScreenShotPath + "\\PacificB_Login" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".pdf";
                _netWrapper.PrintPDF(sFile, false);
                CreateAuditFile(strFileName, strProcessorName, "", "Error", msg);
                if (File.Exists(strMailTextPath + "\\Error\\" + Path.GetFileName(strFileName))) File.Delete(strMailTextPath + "\\Error\\" + Path.GetFileName(strFileName));
                File.Move(strFileName, strMailTextPath + "\\Error\\" + Path.GetFileName(strFileName));
            }
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
                case '-':
                    KeyParams paramers_dash = new KeyParams(VirtualKeyCode.OEM_MINUS, '-');
                    _netWrapper.browser.KeyDown(paramers_dash);
                    _netWrapper.browser.KeyUp(paramers_dash);
                    break;
                case '/':
                    KeyParams paramers_slash = new KeyParams(VirtualKeyCode.OEM_2, '/');
                    _netWrapper.browser.KeyDown(paramers_slash);
                    _netWrapper.browser.KeyUp(paramers_slash);
                    break;
                case ':':
                    KeyParams paramers_col = new KeyParams(VirtualKeyCode.OEM_1, ':');
                    _netWrapper.browser.KeyDown(paramers_col);
                    _netWrapper.browser.KeyUp(paramers_col);
                    break;
                case ';':
                     KeyParams paramers_semi = new KeyParams(VirtualKeyCode.OEM_1, ';');
                     _netWrapper.browser.KeyDown(paramers_semi);
                     _netWrapper.browser.KeyUp(paramers_semi);
                    break;
                case '*':
                    KeyParams paramers_multi = new KeyParams(VirtualKeyCode.MULTIPLY, '*');
                    _netWrapper.browser.KeyDown(paramers_multi);
                    _netWrapper.browser.KeyUp(paramers_multi);
                    break;
            }
        }

        #endregion

        #region Quote
        public void ProcessQuote()
        {
            try
            {
                LoadAppSettings();
                _netWrapper.LogText = "";
                _netWrapper.LogText = "Quote processing started.";
                /*get xml files from quote upload path*/
                GetXmlFiles();
                if (xmlFiles.Count > 0)
                {
                    _netWrapper.LogText = xmlFiles.Count + " Quote files found to process.";
                    for (int j = 0; j < xmlFiles.Count; j++)
                    {
                        ProcessQuoteMTML(xmlFiles[j]);
                        ClearCommonVariables();
                    }
                }
                else
                    _netWrapper.LogText = "No Quote files found to process.";
                _netWrapper.LogText = "Quote processing stopped.";
                _netWrapper.LogText = "PacificBasin Processor Stopped.";
            }
            catch (Exception ex)
            {
                _netWrapper.LogText = "Exception while processing Quote : " + ex.GetBaseException().ToString();
            }
            finally
            {
                _netWrapper.browserView.Dispose();
                _netWrapper.Dispose();
            }
        }

        /*get xml files from quote upload path*/
        public void GetXmlFiles()
        {
            xmlFiles.Clear();
            DirectoryInfo _dir = new DirectoryInfo(strMTMLUploadPath);
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
                _netWrapper.LogText = "'" + Path.GetFileName(MTML_QuoteFile) + "' Quote file processing started.";
                MTMLClass _mtml = new MTMLClass();
                _interchange = _mtml.Load(MTML_QuoteFile);
                LoadInterchangeDetails();
                string strURL = strMessageNumber.Split('|')[0];
                string strToken = strMessageNumber.Split('|')[1];
                if (NavigateToURL(strURL, strToken, Path.GetFileName(MTML_QuoteFile), strMTMLUploadPath) && strUCRefNo != "")
                {
                    if (!IsDisabledElement())
                    {
                        if (Fill_QuoteHeader_details(MTML_QuoteFile))
                        {
                            if (Fill_Item_Details(MTML_QuoteFile))
                            {
                                if (FillDeliveryDays())
                                {
                                    if (FillAdditionalChanrges())
                                    {
                                        if (FillHeaderDiscount())
                                        {
                                            if (isSaveQuote)
                                            {
                                                DOMElement txtTotal = _netWrapper.GetElementbyClass("tdSumValueGrand");
                                                string s = txtTotal.TextContent.Replace(",","").Trim();
                                                if (Convert.ToInt32(Convert.ToDouble(s)) == Convert.ToInt32(Convert.ToDouble(strGrandTotal)))
                                                {
                                                    SaveQuotation(MTML_QuoteFile);
                                                }
                                                else
                                                {
                                                    MoveFileToError(MTML_QuoteFile, "Unable to upload Quote for Ref : '" + strUCRefNo + "' due to Total Amount '" + strGrandTotal + "' mismatch on site Grand Total '" + txtTotal.InnerText + "'.");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            MoveFileToError(MTML_QuoteFile, "Unable to fill additional charges for Ref  " + strUCRefNo);
                                        }
                                    }
                                    else
                                    {
                                        MoveFileToError(MTML_QuoteFile, "Unable to fill header discount for Ref  " + strUCRefNo);
                                    }
                                }
                                else
                                {
                                    MoveFileToError(MTML_QuoteFile, "Unable to fill header delivery days for Ref  " + strUCRefNo);
                                }
                            }
                        }
                    }
                    else
                    {
                        MoveFileToError(MTML_QuoteFile, "Save and/or submit quote button is null for Ref  " + strUCRefNo);
                    }
                    _netWrapper.browser.LoadURL("about:blank"); Thread.Sleep(1500);
                }
                else
                {
                    _netWrapper.LogText = "Unable to navigate to URL for MTML " + strUCRefNo;
                }
            }
            catch (Exception ex)
            {
                MoveFileToError(MTML_QuoteFile, "Exception while processing Quote MTML : " + ex.GetBaseException().ToString());
            }
        }

        public bool IsDisabledElement()
        {
            bool result = false;
            DOMElement _eleSaveQuote = _netWrapper.GetElementByType("button", pagevar.ClkbtnSaveQuote, "click.delegate");
            DOMElement _eleSubmitQuote = _netWrapper.GetElementByType("button", pagevar.ClkbtnSendQuote, "click.delegate");
            if (_eleSaveQuote != null && _eleSubmitQuote != null)
                result = false;
            else
                result = true;

            return result;
        }

        public bool Fill_QuoteHeader_details(string MTML_QuoteFile)
        {
            bool result = false, resultCurr = false, resultSup = false, resultSupRemarks = false, resultValOffer = false;
            try
            {
                // setting currency
                DOMSelectElement _Currency = (DOMSelectElement)_netWrapper.GetElementbyId(pagevar.selCurrencyId);
                if (_Currency != null)
                {
                    _Currency.Focus();
                    Thread.Sleep(300);
                    for (int j = 0; j < strCurrency.Length; j++)
                    {
                        SetAlphaNumeric(strCurrency[j]);
                        Thread.Sleep(200);
                    }
                    _netWrapper.LogText = "Currency " + strCurrency + " set successfully"; resultCurr = true;
                }

                //setting supp. quot ref.
                DOMInputElement _SuppRef = (DOMInputElement)_netWrapper.GetElementbyId(pagevar.txtSuppRefId);
                if (_SuppRef != null)
                {
                    _SuppRef.Focus();
                    _SuppRef.Value = "";
                    for (int j = 0; j < strAAGRefNo.Length; j++)
                    {
                        SetAlphaNumeric(strAAGRefNo[j]);
                        Thread.Sleep(100);
                    }
                    if (_SuppRef.Value == strAAGRefNo) { _netWrapper.LogText = "Supplier Ref " + strAAGRefNo + " set successfully"; resultSup = true; }
                }

                //setting supplier remarks
                DOMTextAreaElement _SuppRemarks = (DOMTextAreaElement)_netWrapper.GetElementbyId(pagevar.txtSuppRemarksId);
                if (_SuppRemarks != null)
                {
                    _SuppRemarks.Focus();
                    _SuppRemarks.Value = "";

                    if (strDelvDate != "")
                    {
                        string delDate = " Delivery Date: " + strDelvDate;
                        strSupplierComment = strSupplierComment + delDate;

                    }

                    if (strPackingCost != null)
                        strSupplierComment += " Packing Cost: " + strPackingCost;


                    if (strFreightCharge != null)
                        strSupplierComment += " Freight Cost: " + strFreightCharge;

                    for (int j = 0; j < strSupplierComment.Length; j++)
                    {
                        SetAlphaNumeric(strSupplierComment[j]);
                        Thread.Sleep(100);
                    }
                    _netWrapper.LogText = "Supplier Remarks set successfully"; resultSupRemarks = true;
                }

                //setting validity of offer
                DOMInputElement _ValidityOfferDate = (DOMInputElement)_netWrapper.GetElementbyId(pagevar.txtValidityOfferId);
                if (_ValidityOfferDate != null)
                {
                    _ValidityOfferDate.Focus();
                    _ValidityOfferDate.Value = "";
                    for (int j = 0; j < strExpDate.Length; j++)
                    {
                        SetAlphaNumeric(strExpDate[j]);
                        Thread.Sleep(100);
                    }
                    if (_ValidityOfferDate.Value == strExpDate) { _netWrapper.LogText = "Validity Offer set successfully"; resultValOffer = true; }
                }
                else
                {
                    DOMElement _lblValidateOfferDate = _netWrapper.GetElementByType("label", "inputValidity", "for");
                    if (_lblValidateOfferDate == null) resultValOffer = true;
                    else resultValOffer = false;
                }


                if (resultCurr && resultSup && resultSupRemarks && resultValOffer) result = true;//&& resultQuoteDate
                else
                {
                    if (!resultCurr) MoveFileToError(MTML_QuoteFile, "Unable to set currency for Ref No. " + strUCRefNo);
                    else if (!resultSup) MoveFileToError(MTML_QuoteFile, "Unable to set supplier ref no for Ref No. " + strUCRefNo);
                    else if (!resultSupRemarks) MoveFileToError(MTML_QuoteFile, "Unable to set supplier remarks for Ref No. " + strUCRefNo);
                    else if (!resultValOffer) MoveFileToError(MTML_QuoteFile, "Unable to set validity offer for Ref No. " + strUCRefNo);
                    result = false;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Fill_Item_Details(string MTML_QuoteFile)
        {
            bool result = true,nresult=true;
            string msg="";
            _netWrapper.LogText = "Start Filling LineItem details";
            DOMElement _tblItems = _netWrapper.GetElementbyId(pagevar.tblItemId);
            if (_tblItems != null)
            {
                DOMElement eletbody = _tblItems.GetElementByTagName("tbody");
                if (eletbody != null)
                {
                    List<DOMNode> eleRow = eletbody.GetElementsByTagName("tr");
                    if (eleRow.Count > 0)
                    {
                        foreach (DOMNode row in eleRow)
                        {
                            DOMElement eRow = (DOMElement)row;
                            try
                            {
                                List<DOMNode> _data = eRow.GetElementsByTagName("td");
                                if (_data.Count > 0)
                                {
                                    string PartNo = _data[2].TextContent;
                                    string PartName = _data[1].TextContent;
                                    string ItemNo = "";
                                   
                                    LineItem _item = null;
                                    foreach (LineItem item in _lineitem)
                                    {
                                        if (item.Identification != null)//5-12-2017
                                        { if (item.Identification.Trim().ToString() == PartNo.Trim().ToString()) { _item = item; } }
                                        //else
                                        //{
                                        //  //  if (item.Description.ToUpper().Trim().ToString().Contains(PartName.ToUpper().Trim().ToString())) { _item = item; }//5-12-2017
                                        //}
                                    }
                                    if (_item == null)
                                    {
                                        foreach (LineItem item in _lineitem)
                                        {
                                            if (item.Identification != null)
                                            {
                                                if (item.Identification.Replace(" ", "").Trim().ToString() == PartNo.Trim().ToString()) { _item = item; }
                                            }
                                        }
                                    }
                                    if (_item == null)
                                   {
                                        DOMElement _eleAddFields = _data[13].GetElementByTagName("button");//5-12-2017
                                        if (_eleAddFields != null)
                                        {
                                            if (_netWrapper.ClickElementbyType(_eleAddFields, "textarea", pagevar.ItemSupRemarks, "value.bind"))
                                            {
                                                DOMElement _eleItemNo = _netWrapper.GetElementByType("div", "col-xs-8 indent23", "class");
                                                if (_eleItemNo != null)
                                                {
                                                    ItemNo = _eleItemNo.TextContent;
                                                }
                                            }
                                            DOMElement _eleFCancel = _netWrapper.GetElementByType("button", "close( 'Cancel') ", "click.delegate");
                                            if (_eleFCancel != null)
                                            {
                                                _netWrapper.ClickElement(_eleFCancel, false);
                                            }
                                        }
                                        if (ItemNo != "")//5-12-2017
                                        {
                                            foreach (LineItem item in _lineitem)
                                            {
                                                if (item.OriginatingSystemRef != null)
                                                {
                                                    if (ItemNo.Trim() == item.OriginatingSystemRef.Trim()) { _item = item; }
                                                }
                                            }
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

                                        //#region setting purchase quantity
                                        //DOMInputElement PQtyInput = null;
                                        //var uPQInput = _data[6].GetElementsByTagName("input");
                                        //if (uPQInput.Count > 0)
                                        //{
                                        //    PQtyInput = (DOMInputElement)uPQInput[0];

                                        //    if (Convert.ToDouble(PQtyInput.Value) != Convert.ToDouble(_item.Quantity))//4-1-2018
                                        //    {
                                        //        PQtyInput.Focus();
                                        //        PQtyInput.Value = ""; PQtyInput.Value = Convert.ToString(_item.Quantity);
                                        //        Thread.Sleep(200);

                                        //        if (PQtyInput.Value != Convert.ToString(_item.Quantity)) result = false;
                                        //    }
                                        //}
                                        //#endregion

                                        #region setting purchase unit

                                        DOMSelectElement PUnitInput = null;
                                        var uPUInput = _data[7].GetElementsByTagName("select");
                                        if (uPUInput.Count > 0)
                                        {
                                            PUnitInput = (DOMSelectElement)uPUInput[0];
                                            if (PUnitInput != null)
                                            {
                                                if (PUnitInput.Value != _item.MeasureUnitQualifier)
                                                {
                                                    PUnitInput.Focus();
                                                    Thread.Sleep(300);
                                                    for (int j = 0; j < _item.MeasureUnitQualifier.Length; j++)
                                                    {
                                                        SetAlphaNumeric(_item.MeasureUnitQualifier[j]);
                                                        Thread.Sleep(100);
                                                    }
                                                }
                                                else result = true;
                                            }
                                            if (PUnitInput.Value != _item.MeasureUnitQualifier) result = false;
                                            if (!result)
                                            {
                                                string[] strArr = _item.LineItemComment.Value.Split('|');
                                                if (strArr.Length > 0)
                                                {
                                                    foreach (string str in strArr)
                                                    {
                                                        if (str.Contains("UOM"))
                                                        {
                                                            result = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        #endregion

                                        #region setting lead days
                                        string strDeltime = _item.DeleiveryTime;
                                        if (strDeltime != "0" && strDeltime != null)
                                        {
                                            DOMInputElement dayInput = null;
                                            var udayInput = _data[12].GetElementsByTagName("input");
                                            if (udayInput.Count > 0)
                                            {
                                                dayInput = (DOMInputElement)udayInput[0];
                                                if (strLeadDays == null || strLeadDays == "0" || strLeadDays == "")
                                                {
                                                    dayInput.Focus(); dayInput.Value = ""; dayInput.Value = Convert.ToString(strDeltime);
                                                    Thread.Sleep(200);
                                                    if (dayInput.Value != strDeltime) result = false;
                                                }
                                                else
                                                {
                                                    dayInput.Focus(); dayInput.Value = ""; dayInput.Value = strLeadDays;
                                                    if (dayInput.Value != strLeadDays) result = false;
                                                }
                                            }
                                        }
                                        #endregion

                                        Thread.Sleep(1000);
                                        // DOMElement _eleAddFields = _netWrapper.GetElementByType("button", "editItem(it.id,ig.id)", "click.trigger");
                                        DOMElement _eleAddFields1 = _data[13].GetElementByTagName("button");//5-12-2017
                                        if (_eleAddFields1 != null)
                                        {
                                            if (_netWrapper.ClickElementbyType(_eleAddFields1, "textarea", pagevar.ItemSupRemarks, "value.bind"))
                                            {
                                                Thread.Sleep(200);
                                                DOMTextAreaElement _ItemSupRemarks = (DOMTextAreaElement)_netWrapper.GetElementByType("textarea", pagevar.ItemSupRemarks, "value.bind");
                                                if (_ItemSupRemarks != null)
                                                {
                                                    _ItemSupRemarks.Value = "";
                                                    //  _ItemSupRemarks.Value = _item.LineItemComment.Value;
                                                    _ItemSupRemarks.Focus();
                                                    Thread.Sleep(300);
                                                    string SuppRemarks = _item.LineItemComment.Value.Replace('|', ',').Replace('(', ' ').Replace(')', ' ').Replace('\"', ' ').Replace('&', ' ');
                                                    //_ItemSupRemarks.Value = SuppRemarks;//5-12-2017
                                                    for (int j = 0; j < SuppRemarks.Length; j++)//5-12-2017
                                                    {
                                                        SetAlphaNumeric(SuppRemarks[j]);
                                                        Thread.Sleep(100);
                                                    }
                                                    DOMElement _eleFSave = _netWrapper.GetElementByType("button", "close( 'Save') ", "click.delegate");
                                                    if (_eleFSave != null)
                                                    {
                                                        _netWrapper.ClickElement(_eleFSave, false);
                                                    }
                                                }
                                            }
                                        }

                                        #region setting purchase quantity
                                        DOMInputElement PQtyInput = null;
                                        var uPQInput = _data[6].GetElementsByTagName("input");
                                        if (uPQInput.Count > 0)
                                        {
                                            PQtyInput = (DOMInputElement)uPQInput[0];

                                            if (Convert.ToDouble(PQtyInput.Value) != Convert.ToDouble(_item.Quantity))//4-1-2018
                                            {
                                                PQtyInput.Focus();
                                                PQtyInput.Value = ""; PQtyInput.Value = Convert.ToString(_item.Quantity);
                                                Thread.Sleep(200);

                                                if (PQtyInput.Value != Convert.ToString(_item.Quantity)) result = false;
                                            }
                                        }
                                        #endregion

                                        #region setting unit price
                                        DOMInputElement PPInput = null;
                                        var uPPInput = _data[8].GetElementsByTagName("input");
                                        if (uPPInput.Count > 0)
                                        {
                                            PPInput = (DOMInputElement)uPPInput[0];
                                            PPInput.Focus(); PPInput.Value = ""; PPInput.Value = Convert.ToString(_price);
                                            Thread.Sleep(200);
                                            if (PPInput.Value != _price) result = false;
                                        }
                                        #endregion

                                        #region setting discount
                                        if (_discount != "0.00" && _discount != "0")
                                        {
                                            DOMInputElement disInput = null;
                                            var udisInput = _data[10].GetElementsByTagName("input");
                                            if (udisInput.Count > 0)
                                            {
                                                disInput = (DOMInputElement)udisInput[0];
                                                disInput.Focus(); disInput.Value = ""; disInput.Value = Convert.ToString(_discount);
                                                Thread.Sleep(200);
                                                if (disInput.Value != _discount) result = false;
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        // MoveFileToError(MTML_QuoteFile, "Item not found on site with Part No " + PartNo + " for refNo " + strUCRefNo);
                                        _netWrapper.LogText = msg = "Item not found on site or in mtml with Part No " + PartNo + " for refNo " + strUCRefNo;
                                        nresult = false;
                                        break;
                                        //    string _price = "", _discount = "";
                                        //    foreach (PriceDetails _priceDetails in _item.PriceList)
                                        //    {
                                        //        if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.GRP) _price = _priceDetails.Value.ToString("0.00");
                                        //        else if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.DPR) _discount = _priceDetails.Value.ToString("0.00");
                                        //    }

                                        //    //setting supplier item remarks
                                        //    string remarks = "Name-" + _item.Description + ",Part No-" + _item.Identification + ",Quantity-" + _item.Quantity + ",Unit-" + _item.MeasureUnitQualifier +
                                        //        ",Unit Price" + _price + ",Discount-" + _discount + ",Total Price-" + _item.MonetaryAmount;

                                        //    DOMElement _eleAddFields = _netWrapper.GetElementByType("button", "editItem(it.id,ig.id)", "click.trigger");
                                        //    if (_eleAddFields != null)
                                        //    {
                                        //        if (_netWrapper.ClickElementbyType(_eleAddFields, "textarea", pagevar.ItemSupRemarks, "value.bind"))
                                        //        {
                                        //            DOMTextAreaElement _ItemSupRemarks=(DOMTextAreaElement)_netWrapper.GetElementByType("textarea",pagevar.ItemSupRemarks, "value.bind");
                                        //            if (_ItemSupRemarks != null)
                                        //            {
                                        //                _ItemSupRemarks.Value = remarks;
                                        //                DOMElement _eleFSave = _netWrapper.GetElementByType("button", "close( 'Save') ", "click.delegate");
                                        //                if (_eleFSave != null)
                                        //                {
                                        //                    _eleAddFields.Click();
                                        //            }
                                        //        }
                                        //    }
                                        //    //
                                    }
                                }
                            }
                            catch (Exception ex)
                            { _netWrapper.LogText = ex.GetBaseException().ToString(); }
                            if (!result) { break; }
                        }
                        if (!result)
                        {
                            count++;
                            if (count <= 2)
                            {
                                _netWrapper.LogText = "Retry filling of lineItem details";
                                Fill_Item_Details(MTML_QuoteFile);
                            }
                            else if (count > 2)
                            {
                                MoveFileToError(MTML_QuoteFile, "Unable to set item details");
                            }
                        }
                        if (result && nresult) _netWrapper.LogText = "LineItem details filling completed";
                        else if (!nresult) { MoveFileToError(MTML_QuoteFile, msg); result = false; }
                    }
                }
            }
            return result;
        }

        public bool FillDeliveryDays()
        {
            bool result = false;
            try
            {
                //setting Delivery Time (days)
                if (strLeadDays != null || strLeadDays != "0" || strLeadDays != "")
                {
                    DOMInputElement _DelDays = (DOMInputElement)_netWrapper.GetElementbyId(pagevar.txtLeadDaysId);
                    if (_DelDays != null)
                    {
                        _DelDays.Focus();
                        _DelDays.Value = "";
                        Thread.Sleep(300);
                        for (int j = 0; j < strLeadDays.Length; j++)
                        {
                            SetAlphaNumeric(strLeadDays[j]);
                            Thread.Sleep(100);
                        }

                        KeyParams paramers = new KeyParams(VirtualKeyCode.RETURN, '\n');
                        _netWrapper.browser.KeyDown(paramers);
                        _netWrapper.browser.KeyUp(paramers);
                        Thread.Sleep(500);

                        DOMElement _eleDiv = _netWrapper.GetElementbyId(pagevar.divUpdateLeadDayId);
                        if (_eleDiv != null)
                        {
                            string a = _eleDiv.GetAttribute("style");
                            if (a.Contains("display: block;"))
                            {
                                DOMElement _eleDayUpdateCancel = _netWrapper.GetElementByType("button", "btn btn-default btn-sm  au-target", "class", "closeUpdate( 'Cancel') ", "click.delegate");
                                if (_eleDayUpdateCancel != null)
                                {
                                    _eleDayUpdateCancel.Click();
                                    Thread.Sleep(500);
                                }
                            }
                        }
                        if (_DelDays.Value == strLeadDays) { _netWrapper.LogText = "Delivery Time (Days) set successfully"; result = true; }
                        else { _netWrapper.LogText = "Unable to set Delivery Time (Days)"; result = false; }
                    }
                }
            }
            catch (Exception ex)
            { _netWrapper.LogText = ex.GetBaseException().ToString(); }
            return result;
        }

        public bool FillHeaderDiscount()
        {
            bool result = false;
            try
            {//setting header discount
                DOMInputElement _headerDisc = (DOMInputElement)_netWrapper.GetElementByType("input", pagevar.valHeaderDisc, "value.bind");
                if (_headerDisc != null)
                {
                    _headerDisc.Focus();
                    _headerDisc.Value = "";
                    _headerDisc.Value = Convert.ToString(AdditionalDiscount);

                    if (_headerDisc.Value == Convert.ToString(AdditionalDiscount)) { _netWrapper.LogText = "Header discount set successfully"; result = true; }
                    DOMInputElement _addCharges = (DOMInputElement)_netWrapper.GetElementByType("input", pagevar.valAddCharges, "value.bind");
                    if (_addCharges != null)
                        _addCharges.Focus();
                }
            }
            catch (Exception ex)
            { _netWrapper.LogText = ex.GetBaseException().ToString(); }
            return result;
        }

        public bool FillAdditionalChanrges()
        {
            bool result = false;
            try
            {
                //setting Add. charges
                DOMInputElement _addCharges = (DOMInputElement)_netWrapper.GetElementByType("input", pagevar.valAddCharges, "value.bind");
                if (_addCharges != null)
                {
                    _addCharges.Focus();
                    _addCharges.Value = "";
                    string strCharges = Convert.ToString(Convert.ToDouble(strFreightCharge) + Convert.ToDouble(strPackingCost));
                    _addCharges.Value = strCharges;
                    if (_addCharges.Value == strCharges) { _netWrapper.LogText = "Additional charges set successfully"; result = true; }

                }
            }
            catch (Exception ex)
            { _netWrapper.LogText = ex.GetBaseException().ToString(); }
            return result;
        }

        public void SaveQuotation(string MTML_QuoteFile)
        {
            try
            {
                DOMElement _eleSaveQuote = _netWrapper.GetElementByType("button", pagevar.ClkbtnSaveQuote, "click.delegate");
                if (_eleSaveQuote != null)
                {
                    string a = _eleSaveQuote.GetAttribute("disabled.bind");
                    if (_netWrapper.ClickElementbyType(_eleSaveQuote, "label", pagevar.SaveStatusId, "id"))
                    {
                        DOMElement _infolabel = _netWrapper.GetElementbyId("infoLabel");
                        Thread.Sleep(5000);
                        if (_infolabel != null && _infolabel.TextContent.Contains("Successfully saved input to database"))
                        {
                            string sFile = strScreenShotPath + "\\PacificB_Quote_Draft_" + strUCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".pdf";
                            _netWrapper.PrintPDF(sFile, false);
                            MoveFileToBackup(MTML_QuoteFile, _infolabel.TextContent);
                            SendMailNotification(_interchange, "QUOTE", strUCRefNo, "SUBMITTED", "Quote '" + strUCRefNo + "' submitted successfully.");
                        }
                        else
                        {
                            MoveFileToError(MTML_QuoteFile, "Unable to save quotation due to " + _infolabel.TextContent + " for Ref No: " + strUCRefNo);
                        }
                    }
                }
            }
            catch (Exception)
            { throw; }
        }

        public void LoadInterchangeDetails()
        {
            try
            {
                _netWrapper.LogText = "Started Loading interchange object.";
                if (_interchange != null)
                {
                    if (_interchange.Recipient != null)
                        strBuyerCode = _interchange.Recipient;

                    if (_interchange.Sender != null)
                        strSupplierCode = _interchange.Sender;

                    if (_interchange.DocumentHeader.DocType != null)
                        strDocType = _interchange.DocumentHeader.DocType;

                    if (_interchange.DocumentHeader != null)
                    {
                        if (_interchange.DocumentHeader.IsDeclined)
                            IsDecline = _interchange.DocumentHeader.IsDeclined;

                        if (_interchange.DocumentHeader.MessageNumber != null)
                            strMessageNumber = _interchange.DocumentHeader.MessageNumber;

                        if (_interchange.DocumentHeader.LeadTimeDays != null)
                            strLeadDays = _interchange.DocumentHeader.LeadTimeDays;

                        strCurrency = _interchange.DocumentHeader.CurrencyCode;

                        strMsgNumber = _interchange.DocumentHeader.MessageNumber;
                        strMsgRefNumber = _interchange.DocumentHeader.MessageReferenceNumber;

                        if (_interchange.DocumentHeader.IsAltItemAllowed != null) IsAltItemAllowed = Convert.ToInt32(_interchange.DocumentHeader.IsAltItemAllowed);
                        if (_interchange.DocumentHeader.IsPriceAveraged != null) IsPriceAveraged = Convert.ToInt32(_interchange.DocumentHeader.IsPriceAveraged);
                        if (_interchange.DocumentHeader.IsUOMChanged != null) IsUOMChanged = Convert.ToInt32(_interchange.DocumentHeader.IsUOMChanged);
                        if (_interchange.DocumentHeader.AdditionalDiscount != null) AdditionalDiscount = Convert.ToDouble(_interchange.DocumentHeader.AdditionalDiscount);


                        for (int i = 0; i < _interchange.DocumentHeader.References.Count; i++)
                        {
                            if (_interchange.DocumentHeader.References[i].Qualifier == ReferenceQualifier.UC)
                                strUCRefNo = _interchange.DocumentHeader.References[i].ReferenceNumber.Trim();
                            else if (_interchange.DocumentHeader.References[i].Qualifier == ReferenceQualifier.AAG)
                                strAAGRefNo = _interchange.DocumentHeader.References[i].ReferenceNumber.Trim();
                        }
                    }
                    if (_interchange.BuyerSuppInfo != null)
                    {
                        strLesRecordID = Convert.ToString(_interchange.BuyerSuppInfo.RecordID);
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
                                                strBuyerPhone = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.EM)
                                                strBuyerEmail = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.FX)
                                                strBuyerFax = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                        }
                                    }
                                }
                            }
                        }

                        else if (_interchange.DocumentHeader.PartyAddresses[j].Qualifier == PartyQualifier.VN)
                        {
                            strSupplierName = _interchange.DocumentHeader.PartyAddresses[j].Name;
                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts != null)
                            {
                                if (_interchange.DocumentHeader.PartyAddresses[j].Contacts.Count > 0)
                                {
                                    if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count > 0)
                                    {
                                        for (int k = 0; k < _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count; k++)
                                        {
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.TE)
                                                strSupplierPhone = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.EM)
                                                strSupplierEmail = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.FX)
                                                strSupplierFax = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                        }
                                    }
                                }
                            }
                        }

                        else if (_interchange.DocumentHeader.PartyAddresses[j].Qualifier == PartyQualifier.UD)
                        {
                            strVesselName = _interchange.DocumentHeader.PartyAddresses[j].Name;
                            if (_interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Berth != "")
                                strPortName = _interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Berth;

                            if (_interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Port != null)
                                strPortCode = _interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Port;
                        }
                    }

                    #endregion

                    #region read comments

                    if (_interchange.DocumentHeader.Comments != null)
                    {
                        for (int i = 0; i < _interchange.DocumentHeader.Comments.Count; i++)
                        {
                            if (_interchange.DocumentHeader.Comments[i].Qualifier == CommentTypes.SUR)
                                strSupplierComment = _interchange.DocumentHeader.Comments[i].Value;
                            else if (_interchange.DocumentHeader.Comments[i].Qualifier == CommentTypes.ZTP)
                                strPayTerms = _interchange.DocumentHeader.Comments[i].Value;
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
                                strPackingCost = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.FreightCharge_64)
                                strFreightCharge = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.TotalLineItemsAmount_79)
                                strTotalLineItemsAmount = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.AllowanceAmount_204)
                                strAllowance = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.GrandTotal_259)
                                strGrandTotal = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
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
                                        strDelvDate = dtDelDate.ToString("MM/dd/yyyy");
                                    }
                                    if (dtDelDate == null)
                                    {
                                        DateTime dt = FormatMTMLDate(DateTime.Now.AddDays(Convert.ToDouble(strLeadDays)).ToString());
                                        if (dt != DateTime.MinValue)
                                        {
                                            strDelvDate = dt.ToString("MM/dd/yyyy");
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
                                        strExpDate = ExpDate.ToString("dd-MM-yyyy");
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

        public void MoveFileToError(string MTML_QuoteFile, string message)
        {
            string sFile = strScreenShotPath + "\\PacificB_Quote_Error_" + strUCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".pdf";
            _netWrapper.PrintPDF(sFile, false);
            _netWrapper.LogText = message;
            if (File.Exists(strMTMLUploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile))) File.Delete(strMTMLUploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
            File.Move(MTML_QuoteFile, strMTMLUploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
            Thread.Sleep(1000);

            if (File.Exists(strMTMLUploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile)))
                CreateQuoteAuditFile(sFile, "Pacific Basin Quote", strUCRefNo, "Error", message);
        }

        public void MoveFileToBackup(string MTML_QuoteFile, string message)
        {
            if (File.Exists(strMTMLUploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile))) File.Delete(strMTMLUploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile));
            File.Move(MTML_QuoteFile, strMTMLUploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile));
            Thread.Sleep(1000);


            if (File.Exists(strMTMLUploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile)))
                CreateQuoteAuditFile(MTML_QuoteFile, "Pacific Basin Quote", strUCRefNo, "Success", message);
            _netWrapper.LogText = message;
        }

        private void SendMailNotification(MTMLInterchange _interchange, string DocType, string VRNO, string ActionType, string Message)
        {
            try
            {
                string MailFromDefault = Convert.ToString(ConfigurationManager.AppSettings["FromEmailId"]);
                if (MailFromDefault == null) MailFromDefault = "";
                string MailBccDefault = Convert.ToString(ConfigurationManager.AppSettings["MailBcc"]);
                if (MailBccDefault == null) MailBccDefault = "";
                string MailCcDefault = Convert.ToString(ConfigurationManager.AppSettings["MAIL_CC"]);

                string BuyerCode = Convert.ToString(_interchange.Recipient).Trim();
                string SuppCode = Convert.ToString(_interchange.Sender).Trim();
                string BuyerID = Convert.ToString(_interchange.BuyerSuppInfo.BuyerID).Trim();
                string SupplierID = Convert.ToString(_interchange.BuyerSuppInfo.SupplierID).Trim();

                string MailAuditPath = Convert.ToString(ConfigurationManager.AppSettings["MailAuditPath"]);
                if (MailAuditPath.Trim() != "")
                {
                    if (!Directory.Exists(MailAuditPath.Trim())) Directory.CreateDirectory(MailAuditPath.Trim());
                }
                else throw new Exception("MAIL_AUDIT_PATH value is not defined in config file.");

                string MailSettings = Convert.ToString(ConfigurationManager.AppSettings[SuppCode + "-" + BuyerCode]);
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
                            _netWrapper.LogText = "Mail Send to Supplier Email -" + MailTo.Trim() + ".";
                        }
                        else
                        {
                            _netWrapper.LogText = "Unable to send mail notification to supplier; Supplier Mailid is empty.";
                        }
                    }
                    #endregion
                }
                else
                {
                    _netWrapper.LogText = "Unable to send mail notification; No mail setting found for Supplier-Buyer (" + SuppCode + "-" + BuyerCode + ") link combination.";
                }
            }
            catch (Exception ex)
            {
                _netWrapper.LogText = "Unable to create Mail notification template. Error : " + ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }

        public void ClearCommonVariables()
        {
            strDocType = ""; strMessageNumber = ""; strCurrency = ""; strMsgNumber = ""; strMsgRefNumber = ""; strUCRefNo = ""; strAAGRefNo = ""; strLesRecordID = ""; BuyerName = "";
            strBuyerPhone = ""; strBuyerEmail = ""; strBuyerFax = ""; strSupplierName = ""; strSupplierPhone = ""; strSupplierEmail = ""; strSupplierFax = ""; strPortName = ""; strPortCode = ""; strVesselName = "";
            strSupplierComment = ""; strPayTerms = ""; strPackingCost = ""; strFreightCharge = ""; strTotalLineItemsAmount = ""; strGrandTotal = ""; strAllowance = ""; strDelvDate = ""; strExpDate = ""; strLeadDays = "";
            IsDecline = false; isSaveQuote = true;
            IsAltItemAllowed = 0; IsPriceAveraged = 0; IsUOMChanged = 0; count = 0;
        }
        #endregion

        #region Common function

        public void CreateAuditFile(string FileName, string Module, string RefNo, string LogType, string Audit)
        {
            try
            {
                if (!Directory.Exists(strAuditPath)) Directory.CreateDirectory(strAuditPath);

                string auditData = "";
                if (auditData.Trim().Length > 0) auditData += Environment.NewLine;
                auditData += strBuyer + "|";
                auditData += strSupplier + "|";
                auditData += Module + "|";
                auditData += Path.GetFileName(FileName) + "|";
                auditData += RefNo + "|";
                auditData += LogType + "|";
                auditData += DateTime.Now.ToString("yy-MM-dd HH:mm") + " : " + Audit;
                if (auditData.Trim().Length > 0)
                {
                    File.WriteAllText(strAuditPath + "\\Audit_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt", auditData);
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
                string auditPath = strAuditPath;
                if (!Directory.Exists(auditPath)) Directory.CreateDirectory(auditPath);

                string auditData = "";
                if (auditData.Trim().Length > 0) auditData += Environment.NewLine;
                auditData += strBuyer + "|";
                auditData += strSupplier + "|";
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
