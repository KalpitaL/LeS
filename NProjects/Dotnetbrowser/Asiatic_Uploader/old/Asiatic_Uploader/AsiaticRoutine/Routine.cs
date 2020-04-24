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
using System.Globalization;
using MTML.GENERATOR;


namespace AsiaticRoutine
{
    public class AsiaticRoutine
    {
        public NetBrowser _netWrapper = new NetBrowser();
        string strLogPath = "", strAuditPath = "", strMTMLUploadPath = "", strScreenShotPath = "", strBuyer = "", strSupplier = "", strDocType = "", strUCRefNo = "",
             strMessageNumber = "", strLeadDays = "", strCurrency = "", strMsgNumber = "", strMsgRefNumber = "", strAAGRefNo = "", strLesRecordID = "",
             BuyerName = "", strBuyerPhone = "", strBuyerEmail = "", strBuyerFax = "", strSupplierName = "", strSupplierPhone = "", strSupplierEmail = "",
             strSupplierFax = "", strPortName = "", strPortCode = "", strVesselName = "", strSupplierComment = "", strPayTerms = "", strPackingCost = "",
             strFreightCharge = "", strTotalLineItemsAmount = "", strGrandTotal = "", strAllowance = "", strDelvDate = "", strExpDate = "",
             strBuyerTotal = "", strDepositCost = "", strOtherCost = "", strTermCondition = "", strTransportModeCode = "";
        bool IsDecline = false;
        double AdditionalDiscount = 0;
        int IsUOMChanged = 0, IsPriceAveraged = 0, IsAltItemAllowed = 0, waitPeriod = 0;
        List<LineItem> lstExtraItems = new List<LineItem>();
        public MTMLInterchange _interchange { get; set; }
        public LineItemCollection _lineitem = null;

        public AsiaticRoutine()
        {
        }

        public void LoadAppSettings()
        {
            waitPeriod = Convert.ToInt32(ConfigurationManager.AppSettings["WAITPERIOD"].Trim());
            strLogPath = AppDomain.CurrentDomain.BaseDirectory + "Log";
            strMTMLUploadPath = ConfigurationManager.AppSettings["MTML_QUOTEPATH"].Trim();
            strAuditPath = ConfigurationManager.AppSettings["AUDITPATH"].Trim();
            strScreenShotPath = AppDomain.CurrentDomain.BaseDirectory + "ScreenShots";
            if (!Directory.Exists(strScreenShotPath)) Directory.CreateDirectory(strScreenShotPath);
            if (strAuditPath == "") strAuditPath = AppDomain.CurrentDomain.BaseDirectory + "Audit";
            if (strMTMLUploadPath == "") strMTMLUploadPath = Application.StartupPath + "\\MTML_Quote_Upload";
            if (!Directory.Exists(strMTMLUploadPath)) Directory.CreateDirectory(strMTMLUploadPath);
            if (!Directory.Exists(strMTMLUploadPath + "\\Backup")) Directory.CreateDirectory(strMTMLUploadPath + "\\Backup");
            if (!Directory.Exists(strMTMLUploadPath + "\\Error")) Directory.CreateDirectory(strMTMLUploadPath + "\\Error");
            _netWrapper.WaitPeriod = waitPeriod;
        }

        #region RFQ

        public void Read_MailTextFiles()
        {
            try
            {
                docType = "RFQ";
                isRFQ = true;
                _netWrapper.LogText = "";
                _netWrapper.LogText = "RFQ processing started.";
                GetMailTextFiles();
                if (RFQ_mailFiles.Count > 0)
                {
                    for (int j = RFQ_mailFiles.Count - 1; j >= 0; j--)
                    {
                        Dictionary<string, string> dicURLToken = GetURL(RFQ_mailFiles[j]);
                        if (dicURLToken != null && dicURLToken.Count > 0)
                        {

                            if (dicURLToken["URL"] != "" && dicURLToken["URL"].Contains("https://supplierportal.dnvgl.com/") && dicURLToken["Token"] != "")
                            {
                                if (NavigateToURL(dicURLToken["URL"].Trim(), dicURLToken["Token"].Trim(), RFQ_mailFiles[j], strMailTextPath))
                                {
                                    // if (checkValidFile("RFQ", RFQ_mailFiles[j]))
                                    // {
                                    DownloadRFQ(dicURLToken["URL"].Trim(), dicURLToken["Token"].Trim(), RFQ_mailFiles[j]);
                                    _netWrapper.browser.LoadURL("about:blank"); Thread.Sleep(1500);
                                    // }
                                }
                                else
                                {
                                    _netWrapper.LogText = "Unable to navigate to URL for mail file " + Path.GetFileName(RFQ_mailFiles[j]);
                                }
                            }
                            else
                            {
                                string strMsg = "";
                                if (dicURLToken["URL"] == "") strMsg = "URL not found for navigation for " + Path.GetFileName(RFQ_mailFiles[j]);
                                else if (dicURLToken["Token"] == "") strMsg = "Token not found for " + Path.GetFileName(RFQ_mailFiles[j]);
                                _netWrapper.LogText = strMsg;
                                CreateAuditFile(RFQ_mailFiles[j], strProcessorName, "", "Error", strMsg);
                                if (File.Exists(strMailTextPath + "\\Error\\" + Path.GetFileName(RFQ_mailFiles[j]))) File.Delete(strMailTextPath + "\\Error\\" + Path.GetFileName(RFQ_mailFiles[j]));
                                File.Move(RFQ_mailFiles[j], strMailTextPath + "\\Error\\" + Path.GetFileName(RFQ_mailFiles[j]));
                            }
                        }
                    }
                }
                else _netWrapper.LogText = "No file found.";
                _netWrapper.LogText = "RFQ processing stopped.";
            }
            catch (Exception ex)
            {
                string sFile = strScreenShotPath + "\\PacificB_" + this.docType + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".pdf";
                _netWrapper.PrintPDF(sFile, false);
                _netWrapper.LogText = "Exception while processing RFQ : " + ex.GetBaseException().ToString();
            }
        }

        // public bool checkValidFile(string docType,string MailFile)
        // {
        //     bool result = false;
        //     DOMElement _eleAddress = _netWrapper.GetElementByType("div", pagevar.clsBuyerAddr, "style");
        //     if (_eleAddress != null)
        //     {
        //         var _span = _eleAddress.GetElementsByTagName("span");
        //         if (_span.Count > 0)
        //         {
        //             if (_span[0].TextContent.Trim().Contains("Request for Quotation") && docType=="RFQ")
        //             { result = true; }
        //             else if (_span[0].TextContent.Trim().Contains("Purchase Order") && docType=="PO")
        //             { result = false; }
        //         }
        //     }
        //     if (docType == "RFQ" && result == false) { PO_mailFiles.Add(MailFile); RFQ_mailFiles.Remove(MailFile); }
        //     else if (docType == "PO" && result == true) { RFQ_mailFiles.Add(MailFile); PO_mailFiles.Remove(MailFile); }
        //     return result;
        //}

        public void GetMailTextFiles()
        {
            // mailFiles.Clear();
            RFQ_mailFiles.Clear();
            PO_mailFiles.Clear();
            Directory.CreateDirectory(strMailTextPath + "\\Error");
            Directory.CreateDirectory(strMailTextPath + "\\Backup");

            DirectoryInfo _dir = new DirectoryInfo(strMailTextPath);
            FileInfo[] _Files = _dir.GetFiles();


            foreach (FileInfo _MtmlFile in _Files)
            {
                _txtData.Text = File.ReadAllText(_MtmlFile.FullName);
                if (_txtData.Text.Contains("Subject:"))
                {
                    if (_txtData.Text.Contains("Purchase Order"))
                        PO_mailFiles.Add(_MtmlFile.FullName);
                    else if (_txtData.Text.Contains("request for quotation"))
                        RFQ_mailFiles.Add(_MtmlFile.FullName);
                    else
                    {
                        _netWrapper.LogText = "Invalid mail file " + Path.GetFileName(_MtmlFile.FullName);
                        File.Move(_MtmlFile.FullName, strMailTextPath + "\\Error\\" + Path.GetFileName(_MtmlFile.FullName));
                        CreateAuditFile(Path.GetFileName(_MtmlFile.FullName), strProcessorName, "", "Error", "Invalid mail file " + Path.GetFileName(_MtmlFile.FullName));
                    }
                }
                // mailFiles.Add(_MtmlFile.FullName);
            }
        }

        private Dictionary<string, string> GetURL(string emlFile)
        {
            string strUrl = "", strToken = "";
            Dictionary<string, string> dicData = new Dictionary<string, string>();
            try
            {
                _netWrapper.LogText = "Reading file " + Path.GetFileName(emlFile);

                _txtData.Text = File.ReadAllText(emlFile);

                if (!_txtData.Text.Contains("<html>"))
                {
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
                else
                {
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

                                int endIndex = strUrl.IndexOf("\"");
                                if (line.Length > endIndex)
                                {
                                    strUrl = strUrl.Substring(0, endIndex);
                                }
                                else strUrl = line.Trim();

                            }
                            //get Token
                            if (line.Contains("security token"))
                            {
                                i++;
                                string line1 = _txtData.Lines[i];

                                if (line1.Contains("<b>"))
                                {
                                    int tstartIndex = line1.IndexOf("<b>");
                                    if (line1.Length > tstartIndex)
                                    {
                                        strToken = line1.Substring(tstartIndex);
                                        strToken = strToken.Substring(3);
                                        int endindex = strToken.IndexOf("</b>");
                                        if (endindex > -1)
                                        {
                                            strToken = strToken.Substring(0, endindex);
                                        }
                                        strToken.Trim();
                                    }
                                }
                                else//19-6-18
                                {
                                    i--;
                                    line1 = _txtData.Lines[i];
                                    int tstartIndex = line1.IndexOf("<b>");
                                    if (line1.Length > tstartIndex)
                                    {
                                        strToken = line1.Substring(tstartIndex);
                                        strToken = strToken.Substring(3);
                                        int endindex = strToken.IndexOf("</b>");
                                        if (endindex > -1)
                                        {
                                            strToken = strToken.Substring(0, endindex);
                                        }
                                        strToken.Trim();
                                    }
                                }
                            }
                            if (strUrl != "" && strToken != "") { isHtml = true; break; }
                            //  }
                        }
                    }
                    strUrl = strUrl.Trim().Replace("&amp;", "&").Trim().TrimStart('<').TrimStart('"').TrimEnd('>').TrimEnd('"').Trim();
                    dicData.Add("URL", strUrl);
                    dicData.Add("Token", strToken);
                }
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
                        DOMElement _eleErrMsg = _netWrapper.GetElementByType("div", "margin:100px;", "style");//18-12-2017
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
                                    CreateAuditFile(sFile, strProcessorName, strUCRefNo, "Error", _h2[0].TextContent + " for MTML File: " + Path.GetFileName(strFileName));
                                    _netWrapper.LoadUrl("www.google.com");
                                    if (File.Exists(strMailTextPath + "\\Error\\" + Path.GetFileName(strFileName))) File.Delete(strMailTextPath + "\\Error\\" + Path.GetFileName(strFileName));
                                    File.Move(strMailTextPath + "\\" + Path.GetFileName(strFileName), strMailTextPath + "\\Error\\" + Path.GetFileName(strFileName));

                                }
                            }
                        }
                        else
                        {
                            if (Retry <= Maxtry)
                            {
                                Retry++;
                                _netWrapper.LogText = "Login Retry.";
                                _netWrapper.browser.LoadURL("about:blank"); Thread.Sleep(700);
                                isLoggedIn = NavigateToURL(strURL, strToken, strFileName, strMailTextPath);

                            }
                        }
                    }

                }
                else
                {
                    if (Retry <= Maxtry)
                    {
                        Retry++;
                        _netWrapper.LogText = "Login Retry.";
                        _netWrapper.browser.LoadURL("about:blank"); Thread.Sleep(700);
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
                    if (GetRFQItems(strExcelRFQ))
                    {
                        if (GetAddress())
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
                            if (File.Exists(strExcelPath + "\\" + strExcelRFQ)) File.Delete(strExcelPath + "\\" + strExcelRFQ);
                        }
                    }
                    else
                    {
                        MoveRFQToError("Unable to get RFQ item details.", arrRFQNo[0], MailTextFile);
                        if (File.Exists(strExcelPath + "\\" + strExcelRFQ)) File.Delete(strExcelPath + "\\" + strExcelRFQ);
                    }
                }
                else
                {
                    MoveRFQToError("Unable to get RFQ header details.", arrRFQNo[0], MailTextFile);
                    if (File.Exists(strExcelPath + "\\" + strExcelRFQ)) File.Delete(strExcelPath + "\\" + strExcelRFQ);
                }

            }
            catch (Exception ex)
            {
                string sFile = strScreenShotPath + "\\PacificB_RFQ" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".pdf";
                _netWrapper.PrintPDF(sFile, false);
                _netWrapper.LogText = "Exception while processing RFQ : " + ex.GetBaseException().ToString();
                CreateAuditFile("", strProcessorName, strRFQNo, "Error", "Exception while processing RFQ : " + ex.GetBaseException().ToString());
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
            _netWrapper.LogText = msg + " for ref " + RFQNo;
            CreateAuditFile("", strProcessorName, RFQNo, "Error", msg);
            string sFile = strScreenShotPath + "\\PacificB_" + this.docType + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".pdf";
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
                _lesXml = new LeSXML.LeSXML();
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

        public bool GetRFQItems(string strExelRFQFile)
        {
            int LinItemCount = 0;
            bool result = false;
            try
            {
                if (File.Exists(strExcelPath + "\\" + strExelRFQFile))
                {
                    #region Read Excel RFQ
                    _lesXml.LineItems.Clear();
                    _netWrapper.LogText = "Start Getting RFQ LineItem details.";
                    Aspose.Cells.Workbook _workbook = new Aspose.Cells.Workbook(strExcelPath + "\\" + strExelRFQFile);
                    Aspose.Cells.Worksheet _worksheet = _workbook.Worksheets[0];
                    Aspose.Cells.Cells cells = _worksheet.Cells;
                    string strCurrency = cells[24, 8].StringValue;
                    _lesXml.Currency = strCurrency;

                    short _colItemName = Convert.ToInt16(dctAppSettings["ItemName_Col"].Trim());
                    Aspose.Cells.Cell endCell = cells.EndCellInColumn(_colItemName);

                    int maxrow = endCell.Row;
                    for (int i = Convert.ToInt32(dctAppSettings["ItemName_Row"].Trim()); i <= maxrow; i++)
                    {
                        if (cells[i, _colItemName].StringValue == "Item Name")
                            i++;
                        LeSXML.LineItem _item = new LeSXML.LineItem();
                        try
                        {
                            if (!isHtml)
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
                            else
                            {
                                if (cells[i, _colItemName].StringValue.Trim() != "" || cells[i, _colItemName + 2].StringValue.Trim() != "")
                                {
                                    if (cells[i, _colItemName].StringValue.Trim() == "" && cells[i, _colItemName + 2].StringValue.Trim() != "") { }
                                    else
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
                                }
                                else break;
                            }
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
                    _netWrapper.LogText = "Start Getting RFQ LineItem details.";
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

        public bool GetAddress()
        {
            bool result = false;
            try
            {
                _lesXml.Addresses.Clear();
                _netWrapper.LogText = "Start Getting RFQ address details.";
                LeSXML.Address _xmlAdd = new LeSXML.Address();

                _xmlAdd.Qualifier = "BY";
                _xmlAdd.AddressName = dctAppSettings["BuyerName"].Trim();
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
            DOMElement _eleItemTable = null;
            bool isload = false; ; int maxcount = 20, count = 0;
            while (count < maxcount)
            {
                _netWrapper.CurrentDocument = _netWrapper.browser.GetDocument();

                if (isRFQ)
                    _eleItemTable = _netWrapper.GetElementbyId(pagevar.tblItemId);
                else
                    _eleItemTable = _netWrapper.GetElementbyId(pagevar.tblOrderItemId); // Updated By Sanjita 
                if (_eleItemTable == null) { _eleItemTable = _netWrapper.GetElementbyId(pagevar.tblItemId); }

                DOMElement _eleErrMsg = _netWrapper.GetElementByType("div", "margin:100px;", "style");//18-12-2017

                if (_eleItemTable != null)
                {
                    isload = true;
                    break;
                }
                else if (_eleErrMsg != null)
                {
                    var _h2 = _eleErrMsg.GetElementsByTagName("h2");
                    if (_h2.Count == 1)
                    {
                        if (_h2[0].TextContent.Contains("request could not be found or is no longer valid"))
                        {
                            break;
                        }
                    }
                }
                count++;
                Thread.Sleep(1000);
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
                if (msg != "")
                    CreateAuditFile(strFileName, strProcessorName, "", "Error", msg);
                else
                {
                    DOMElement _tblItems = _netWrapper.GetElementbyId(pagevar.tblItemId);
                    if (_tblItems == null)
                        CreateAuditFile(sFile, strProcessorName, "", "Error", "No items found in item table.");
                }
                if (File.Exists(strMailTextPath + "\\Error\\" + Path.GetFileName(strFileName))) File.Delete(strMailTextPath + "\\Error\\" + Path.GetFileName(strFileName));
                File.Move(strMailTextPath + "\\" + strFileName, strMailTextPath + "\\Error\\" + Path.GetFileName(strFileName));
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
                case '\'':
                    KeyParams paramers_squote = new KeyParams(VirtualKeyCode.OEM_7, '\'');
                    _netWrapper.browser.KeyDown(paramers_squote);
                    _netWrapper.browser.KeyUp(paramers_squote);
                    break;
                case '|':
                    KeyParams paramers_pipe = new KeyParams(VirtualKeyCode.OEM_5, '|');
                    _netWrapper.browser.KeyDown(paramers_pipe);
                    _netWrapper.browser.KeyUp(paramers_pipe);
                    break;
            }
        }

        #endregion

        #region Quote
        public void ProcessQuote()
        {
            try
            {
                _netWrapper.LogText = "";
                _netWrapper.LogText = "Quote processing started.";
                DirectoryInfo _dir = new DirectoryInfo(strMTMLUploadPath);
                FileInfo[] _Files = _dir.GetFiles();
                if (_Files.Length > 0)
                {

                    foreach (FileInfo _MtmlFile in _Files)
                    {
                        //_netWrapper.LogText = "Quote file pending";
                        //if (File.Exists(strMTMLUploadPath + "\\" + Path.GetFileName(_MtmlFile.FullName)))
                        //{
                        //    if (File.Exists(strMTMLUploadPath + "\\Error\\" + Path.GetFileName(_MtmlFile.FullName))) File.Delete(strMTMLUploadPath + "\\Error\\" + Path.GetFileName(_MtmlFile.FullName));
                        //    File.Move(strMTMLUploadPath + "\\" + Path.GetFileName(_MtmlFile.FullName), strMTMLUploadPath + "\\Error\\" + Path.GetFileName(_MtmlFile.FullName));
                        //    Thread.Sleep(1000);
                        //}

                        MTMLClass _mtml = new MTMLClass();
                        _interchange = _mtml.Load(_MtmlFile.FullName);
                        LoadInterchangeDetails();
                        if (strDocType.ToUpper() == "QUOTE")
                        {
                            ProcessQuoteMTML(Path.GetFileName(_MtmlFile.FullName));
                            ClearCommonVariables();
                        }
                        else
                        {
                            MoveFileToError(_MtmlFile.FullName, "Invalid doctype : " + strDocType + " for ref " + strUCRefNo, "Quote");
                            throw new Exception("Invalid doctype : " + strDocType + " for ref " + strUCRefNo);
                        }
                    }
                }
                else
                {
                    _netWrapper.LogText = "No Quote files found to process.";
                }
                _netWrapper.LogText = "Quote processing stopped.";
            }
            catch (Exception ex)
            {
                _netWrapper.LogText = "Exception while processing Quote : " + ex.GetBaseException().ToString();
            }
        }

        public void ProcessQuoteMTML(string MTML_QuoteFile)
        {
            try
            {
                _netWrapper.LogText = "'" + Path.GetFileName(MTML_QuoteFile) + "' Quote file processing started.";
                if (NavigateToURL(strMessageNumber, Path.GetFileName(MTML_QuoteFile), strMTMLUploadPath) && strUCRefNo != "")
                {
                    DOMElement _eleInquiryNo = _netWrapper.GetElement("phone-1of2", "data-bind", "text:InquiryNumber ");
                    if (_eleInquiryNo != null)
                    {
                        if (strUCRefNo == _eleInquiryNo.TextContent.Trim())
                        {
                            if (!IsDisabledElement())
                            {
                                if (Fill_QuoteHeader_details(MTML_QuoteFile))
                                {

                                    if (Fill_Item_Details(MTML_QuoteFile))
                                    {
                                        if (lstExtraItems.Count > 0 || strFreightCharge != "0" || strOtherCost != "" || strDepositCost != "")
                                        {
                                            if (!Fill_ExtraItem_Details(MTML_QuoteFile))
                                            {
                                                MoveFileToError(MTML_QuoteFile, "Unable to fill extra item details for Ref  " + strUCRefNo, strDocType);
                                            }
                                        }

                                        if (Fill_Supplier_Remarks())
                                        {
                                            string sFile = strScreenShotPath + "\\Asiatic_Before_Submit_" + strUCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".pdf";
                                            _netWrapper.PrintPDF(sFile, true);

                                            if (!IsDecline)
                                            {
                                                DOMElement _finalSection = _netWrapper.GetElementByType("section", "finish-offer no-shrink", "class");
                                                if (_finalSection != null)
                                                {
                                                    DOMElement _TotalSum = _netWrapper.GetElementByTag(_finalSection, "form-field", "params", "value: totalSumString, label: 'Total sum', readonly: !$data.CanUserWrite() || !$component.inquiry().UseQuotedPriceTotal()", "input");
                                                    if (_TotalSum != null)
                                                    {
                                                        double _TSum = Convert.ToDouble(((DOMInputElement)_TotalSum).Value);
                                                        double _diff = 0; bool res = false;
                                                        if (_TSum == Convert.ToDouble(strGrandTotal)) res = true;
                                                        else if (_TSum > Convert.ToDouble(strGrandTotal)) { _diff = _TSum - Convert.ToDouble(strGrandTotal); if (_diff <= 1)res = true; }
                                                        else if (Convert.ToDouble(strGrandTotal) > _TSum) { _diff = Convert.ToDouble(strGrandTotal) - _TSum; if (_diff <= 1)res = true; }
                                                        if (res)
                                                            SubmitQuote(MTML_QuoteFile);
                                                        else MoveFileToError(MTML_QuoteFile, "Grand Total mismatched for Ref  " + strUCRefNo, strDocType);


                                                    }

                                                }
                                                else MoveFileToError(MTML_QuoteFile, "Unable to get final section for Ref  " + strUCRefNo, strDocType);
                                            }
                                            else
                                            {
                                                //DeclineQuote();
                                                MoveFileToError(MTML_QuoteFile, "Decline quote for Ref  " + strUCRefNo + ",manual process", strDocType);
                                            }
                                        }
                                        else
                                        {
                                            MoveFileToError(MTML_QuoteFile, "Unable to fill supplier remarks for Ref  " + strUCRefNo, strDocType);
                                        }
                                    }
                                    else
                                    {
                                        MoveFileToError(MTML_QuoteFile, "Unable to fill item details for Ref  " + strUCRefNo, strDocType);
                                    }
                                }
                                else
                                {
                                    MoveFileToError(MTML_QuoteFile, "Unable to fill quotation header details for Ref  " + strUCRefNo, strDocType);
                                }
                            }
                            else
                            {
                                MoveFileToError(MTML_QuoteFile, "Quotation is in readonly mode for Ref  " + strUCRefNo, strDocType);
                            }
                        }
                        else
                        {
                            MoveFileToError(MTML_QuoteFile, "Unable to navigate to URL,Ref no not match.", strDocType);
                        }
                    }
                }
                else
                {
                    MoveFileToError(MTML_QuoteFile, "Unable to navigate to URL or The inquiry was not found. ", strDocType);
                }
            }
            catch (Exception ex)
            {
                MoveFileToError(MTML_QuoteFile, "Exception while processing Quote MTML : " + ex.GetBaseException().ToString(), "Quote");
            }
        }

        public bool NavigateToURL(string strURL, string strFileName, string strMailTextPath)
        {
            bool loaded = false;
            try
            {

                loaded = _netWrapper.LoadUrl(strURL,"");

                if (loaded)
                {
                    if (_netWrapper.WaitForElementbyId("general-information"))
                    {
                        loaded = true;
                        //Thread.Sleep(30000);
                    }
                    else loaded = false;
                }
            }
            catch (Exception ex)
            {
                string sFile = strScreenShotPath + "\\Asiatic" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".pdf";
                _netWrapper.PrintPDF(sFile, false);
                _netWrapper.LogText = "Exception while Navigating to URL : " + ex.GetBaseException().ToString();
            }
            return loaded;
        }

        public bool IsDisabledElement()
        {
            bool result = true;
            DOMNode _btnCancel = _netWrapper.GetElementByType("button", "click:$component.cancel.bind($component)", "data-bind");
            if (_btnCancel != null) result = false;
            return result;
        }

        public bool Fill_QuoteHeader_details(string MTML_QuoteFile)
        {
            bool result = false, resRef = false, resValid = false, resCurr = false, resTerms = false, resDel = false, resDelTerms = false;
            try
            {
                Thread.Sleep(Convert.ToInt32(ConfigurationManager.AppSettings["THREAD_SLEEPTIME"].Trim()));
                _netWrapper.LogText = "Start filling header details";

                #region reference no
                DOMElement _refno = _netWrapper.GetElementByTag(null, "form-field", "params", "value: OfferReferenceNumber, readonly: !$data.CanUserWrite(), label: 'Reference number'", "input");
                if (_refno != null)
                { SetValue(_refno, strAAGRefNo,"input"); }
                else { throw new Exception("reference no field not found on page"); }
                #endregion

                #region offer valid until
                if (strExpDate == "")
                {
                    DateTime dt = DateTime.Now.AddDays(Convert.ToDouble(ConfigurationManager.AppSettings["VALIDITY_DAYS"].Trim()));
                    strExpDate = dt.ToString("M/d/yyyy");
                }
                DOMElement _validUntil = null;
                if (strExpDate != "")
                {
                    _validUntil = _netWrapper.GetElementByTag(null, "form-field", "params", "value: offerValidUntilString, label: 'Offer valid until', readonly: !$data.CanUserWrite(), placeholder: $component.datePlaceholder", "input");
                    if (_validUntil != null)
                    { SetValue(_validUntil, strExpDate,"input"); }
                    else
                    { throw new Exception("offer valid until field not found on page"); }
                }
                #endregion

                #region currency
                int selIndex = -1;
                DOMSelectElement _currency = (DOMSelectElement)_netWrapper.GetElementByTag(null, "form-field", "params", "value: OfferedInquiryTerms.CurrencyCode, readonly: !$component.canChangeCurrency(), options: $component.possibleCurrencies, label: 'Offered currency'", "select");//value: OfferedInquiryTerms.CurrencyCode, readonly: !$data.CanUserWrite(), options: $component.possibleCurrencies, label: 'Offered currency'
                if (_currency != null)
                {
                    _currency.Focus();
                    string a = _currency.Children[0].TextContent;
                    if (_currency.Children[0].TextContent != strCurrency)
                    {
                        List<DOMNode> children = _currency.Children;
                        List<DOMElement> options = new List<DOMElement>();
                        foreach (DOMNode child in children)
                        {
                            if (child is DOMElement)
                            {
                                options.Add((DOMElement)child);
                            }
                        }
                        int index = -1;

                        foreach (DOMElement c in options)
                        {
                            index++;
                            if (strCurrency == c.GetAttribute("value"))
                            {
                                selIndex = index;
                                break;
                            }
                        }
                        if (selIndex != -1)
                        {
                            if (_netWrapper.SelectElementItem(_currency, selIndex))
                            {
                                resCurr = true;
                            }
                        }
                    }
                }
                else throw new Exception("currency field not found on page");
                #endregion

                #region General Terms and condition
                DOMTextAreaElement _Gterms = (DOMTextAreaElement)_netWrapper.GetElementByType("textarea", "textinput:OfferedInquiryTerms.GeneralTermsConditions,attr.readonly: !$data.CanUserWrite()", "data-bind");
                if (_Gterms != null)
                { SetValue(_Gterms, strTermCondition,"textarea"); resTerms = true; }
                else { throw new Exception("General Terms and condition field not found on page"); }
                #endregion

                #region delivery time
                DOMElement _delDate = _netWrapper.GetElementByTag(null, "form-field", "params", "value: deliveryLeadTimeString, label: 'Delivery time', readonly: !$data.CanUserWrite()", "input");
                if (_delDate != null)
                { SetValue(_delDate, strLeadDays,"input"); }
                else { throw new Exception("Delivery Time field not found on page"); }
                #endregion

                #region offered delivery terms
                DOMSelectElement _delTerms = (DOMSelectElement)_netWrapper.GetElementByTag(null, "form-field", "params", "value: DeliveryInCoTermCode, readonly: !$parent.CanUserWrite(), options: $component.possibleIncoTerms, label: 'Offered delivery terms'", "select");
                if (_delTerms != null)
                {
                    _delTerms.Focus();
                    string _text = "";
                    if (strTermCondition == "")
                    {
                        strTermCondition = Convert.ToString(ConfigurationManager.AppSettings["OFFER_DELIVERY_TERMS"]).Trim();
                    }
                    List<DOMNode> children = _delTerms.Children;
                    List<DOMElement> options = new List<DOMElement>();
                    foreach (DOMNode child in children)
                    {
                        if (child is DOMElement)
                        {
                            options.Add((DOMElement)child);
                        }
                    }
                  //  int index = -1;

                    foreach (DOMElement c in options)
                    {
                      //  index++;
                        if (c.GetAttribute("value") != "")
                        {
                            if (strTermCondition.Trim().ToUpper().Contains(c.TextContent.Split('-')[1].ToUpper().Trim()))
                            {
                                _text = c.InnerText;
                              //  selIndex = index;
                                break;
                            }
                        }
                    }
                    if (_text != "")
                    {
                        for (int j = 0; j < _text.Length; j++)
                        {
                            SetAlphaNumeric(_text[j]);
                            Thread.Sleep(100);
                        }
                        resDelTerms = true;
                    }
                    //if (selIndex != -1)
                    //{
                    //    if (_netWrapper.SelectElementItem(_delTerms, selIndex))
                    //    {
                    //        resDelTerms = true;
                    //    }
                    //}

                }
                else throw new Exception("Offered Delivery Terms field not found on page");
                #endregion

                if (((DOMInputElement)_refno).Value == strAAGRefNo) resRef = true;
                if (((DOMInputElement)_validUntil).Value == strExpDate) resValid = true;
                string b = _Gterms.Value.ToUpper().Trim();

                if (((DOMInputElement)_delDate).Value == strLeadDays) resDel = true;

                if (resRef && resValid && resCurr && resTerms && resDel && resDelTerms) result = true;
                _netWrapper.LogText = "Stop filling header details";
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool Fill_Item_Details(string MTML_QuoteFile)
        {
            string msg;
            bool result = true;
            _netWrapper.LogText = "Start filling line item details";
            List<DOMElement> lstItems = _netWrapper.GetElementsByTagName("section", "class", "inquiry-item no-shrink");//
            if (lstItems.Count > 0)
            {
               try
                {
                    LineItem _item = null;
                    foreach (LineItem item in _lineitem)
                    {
                        _item = item;
                       if (_item.IsExtraItem == 0)
                        {
                            if (Convert.ToString(item.OriginatingSystemRef) != "")
                            {
                                // Load Item Section By ID
                                DOMElement eRow = (DOMElement)_netWrapper.GetElementbyId(Convert.ToString(item.OriginatingSystemRef));
                                if (eRow == null) eRow = (DOMElement)_netWrapper.GetElementbyId("item-" + item.OriginatingSystemRef);
                                if (eRow != null)
                                {
                                   
                                    string _price = "", _discount = "", TotalPrice = _item.MonetaryAmount.ToString("0.00");
                                    foreach (PriceDetails _priceDetails in _item.PriceList)
                                    {
                                        if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.GRP) _price = _priceDetails.Value.ToString("0.00");
                                        else if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.DPR) _discount = _priceDetails.Value.ToString("0.00");
                                    }

                                    #region offered quantity
                                    if (_item.Quantity != 0)
                                    {
                                        DOMElement _qty = _netWrapper.GetElementByTag(eRow, "form-field", "params", "value: varianceQuantity, placeholder: quantity, label: 'Offered qty.', autofilled: pendingAcceptAutofill(), readonly: !$data.canUserWrite(), disabled: !available()", "input");
                                        if (_qty != null)
                                        {
                                            DOMInputElement txtQffQty = ((DOMInputElement)_qty);
                                            string qty = Convert.ToString(_item.Quantity);
                                            SetValue(_qty, qty, "input");
                                        }
                                        else { result = false; throw new Exception("Quantity fileld for item '" + _item.Description + "' not found."); }
                                    }
                                
                                    #endregion

                                    #region offered unit //commented as it becomes readonly on site//on 09-04-19
                                    //DOMSelectElement _unit = (DOMSelectElement)_netWrapper.GetElementByTag(eRow, "form-field", "params", "value: varianceUnit, options: units, placeholder: unit, label: 'Offered unit', disabled: !available() || !$data.canUserWrite()", "select");
                                    //if (_unit != null)
                                    //{
                                    //    _unit.Focus();
                                    //    Thread.Sleep(300);
                                    //    string _text = "";
                                    //    if (_unit.Value != _item.MeasureUnitQualifier.Trim())
                                    //    {
                                    //        List<DOMNode> children = _unit.Children;
                                    //        List<DOMElement> options = new List<DOMElement>();
                                    //        foreach (DOMNode child in children)
                                    //        {
                                    //            if (child is DOMElement)
                                    //            {
                                    //                options.Add((DOMElement)child);
                                    //            }
                                    //        }
                                    //        int index = -1;

                                    //        foreach (DOMElement c in options)
                                    //        {
                                    //            index++;
                                    //            if (c.GetAttribute("value") != "")
                                    //            {
                                    //                if (_item.MeasureUnitQualifier == c.GetAttribute("value"))
                                    //                {
                                    //                    _text = c.InnerText;
                                    //                    //   selIndex = index;
                                    //                    break;
                                    //                }
                                    //            }
                                    //        }
                                    //        if (_text != "")
                                    //        {
                                    //            for (int j = 0; j < _text.Length; j++)
                                    //            {
                                    //                SetAlphaNumeric(_text[j]);
                                    //                Thread.Sleep(100);
                                    //            }
                                    //        }
                                    //        //    if (selIndex != -1)
                                    //        //    {
                                    //        //        if (_netWrapper.SelectElementItem(_unit, selIndex))
                                    //        //        {
                                    //        //            if (_unit.Value != _item.MeasureUnitQualifier) result = false;
                                    //        //        }
                                    //        //    }
                                    //    }
                                    //}
                                    #endregion

                                    #region Delivery Time
                                    string delTime = item.DeleiveryTime;
                                    if (delTime == "0")
                                    {
                                        delTime = strLeadDays;
                                    }
                                    DOMElement _delTime = _netWrapper.GetElementByTag(eRow, "form-field", "params", "value: deliveryLeadTimeString, label: 'Delivery time', readonly: !$data.canUserWrite(), disabled: !available(), description: deliveryLeadTimeDescription", "input");
                                    if (_delTime != null)
                                    {
                                        ((DOMInputElement)_delTime).Value = delTime;
                                        if (((DOMInputElement)_delTime).Value != delTime) result = false;
                                    }

                                    #endregion

                                    #region Price
                                    DOMElement _pricePerUnit = _netWrapper.GetElementByTag(eRow, "form-field", "params", "value: pricePerUnit, label: 'Price per unit', autofilled: pendingAcceptAutofill(), readonly: !$data.canUserWrite(), disabled: !available() || $component.useQuotedPriceTotal()", "input");
                                    if (_pricePerUnit != null)
                                    {
                                        SetValue(_pricePerUnit, _price, "input");
                                        //DOMInputElement txtPrice = ((DOMInputElement)_pricePerUnit);

                                        //txtPrice.Focus();
                                        //Thread.Sleep(300);
                                        //for (int j = 0; j < _price.Length; j++)
                                        //{
                                        //    SetAlphaNumeric(_price[j]);
                                        //    Thread.Sleep(100);
                                        //}
                                        //if (((DOMInputElement)_pricePerUnit).Value != _price) result = false;
                                    }
                                    #endregion
                                    //DOMElement _pricePerUnit1 = _netWrapper.GetElementByTag(eRow, "form-field", "params", "value: positionSum, label: 'Position sum', autofilled: pendingAcceptAutofill(), readonly: !$data.canUserWrite(), disabled: !available() || $component.useQuotedPriceTotal()", "input");
                                    //_pricePerUnit1.Click();

                                    result = true;
                                }
                                else
                                {
                                    _netWrapper.LogText = msg = "Item not found on site or in mtml with id " + item.OriginatingSystemRef + " for refNo " + strUCRefNo;
                                    result = false;
                                    break;
                                }
                            }
                            else
                            {
                                _netWrapper.LogText = msg = "Item not found on site or in mtml with id " + item.OriginatingSystemRef + " for refNo " + strUCRefNo;
                                result = false;
                                break;
                            }
                        }
                        else lstExtraItems.Add(_item);
                    }
                    _netWrapper.LogText = "Stop filling lineitem details";
                }
                catch (Exception ex)
                {
                    _netWrapper.LogText = ex.GetBaseException().ToString();
                }
            }
          
            return result;
        }

        public bool Fill_ExtraItem_Details(string MTML_QuoteFile)
        {
            bool result = true;
            int counter = 4;
            foreach (LineItem lItem in lstExtraItems)
            {
                counter = counter + 1;
                string _price = "", _discount = "";
                foreach (PriceDetails _priceDetails in lItem.PriceList)
                {
                    if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.GRP) _price = _priceDetails.Value.ToString("0.00");
                    else if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.DPR) _discount = _priceDetails.Value.ToString("0.00");
                }
                fillExtraItems(counter, _price, lItem.Description,lItem.Quantity,lItem.MeasureUnitQualifier);
                //throw new Exception("Extra item found for refno " + strUCRefNo);
            }

            #region Deposite Cost
            //int counter = 4;
           
            //if (strDepositCost != "")
            //{
            //    counter=counter+1;
            //    fillExtraItems(counter, strDepositCost, "DEPOSIT CHARGE");
            //    #region delete
            //    //DOMElement _btnAddItem = _netWrapper.GetElementByType("button", "click:$component.addItem.bind($component)", "data-bind");
            //    //if (_btnAddItem != null)
            //    //{
            //    //    counter = counter + 1;
            //    //    if (_netWrapper.ClickElement(_btnAddItem, true, "", "section", "id", "item-new-item-" + counter))
            //    //    {

            //    //       DOMElement eRow = (DOMElement)_netWrapper.GetElementbyId("item-new-item-" + counter);
            //    //        if (eRow != null)
            //    //        {
            //    //            DOMElement _itemName = _netWrapper.GetElementByTag(eRow, "form-field", "params", "value: varianceName, placeholder: name, label: 'Offered item', readonly: !$data.canUserWrite(), disabled: !available()", "input");
            //    //            if (_itemName != null)
            //    //            {
            //    //                DOMInputElement txtItemName = ((DOMInputElement)_itemName);
            //    //                if (txtItemName != null)
            //    //                {
            //    //                    txtItemName.Focus();
            //    //                    string _typeText="DEPOSIT CHARGE";
            //    //                    for (int j = 0; j < _typeText.Length; j++)
            //    //                    {
            //    //                        SetAlphaNumeric(_typeText[j]);
            //    //                        Thread.Sleep(100);
            //    //                    }
            //    //                }
            //    //            }

            //    //            DOMElement _qty = _netWrapper.GetElementByTag(eRow, "form-field", "params", "value: varianceQuantity, placeholder: quantity, label: 'Offered qty.', autofilled: pendingAcceptAutofill(), readonly: !$data.canUserWrite(), disabled: !available()", "input");
            //    //             if (_qty != null)
            //    //             {
            //    //                 DOMInputElement txtQffQty = ((DOMInputElement)_qty);
            //    //                 txtQffQty.Focus();
            //    //                SetAlphaNumeric('1');
            //    //                     Thread.Sleep(100);
                                 
            //    //             }

            //    //             DOMElement _pricePerUnit = _netWrapper.GetElementByTag(eRow, "form-field", "params", "value: pricePerUnit, label: 'Price per unit', autofilled: pendingAcceptAutofill(), readonly: !$data.canUserWrite(), disabled: !available() || $component.useQuotedPriceTotal()", "input");
            //    //             if (_pricePerUnit != null)
            //    //             {
            //    //                 DOMInputElement txtPrice = ((DOMInputElement)_pricePerUnit);
            //    //                 //((DOMInputElement)_pricePerUnit).Value = _price;
            //    //                 //((DOMInputElement)_pricePerUnit).SetAttribute("value", _price);

            //    //                 txtPrice.Focus();
            //    //                 Thread.Sleep(300);
            //    //                 for (int j = 0; j < strDepositCost.Length; j++)
            //    //                 {
            //    //                     SetAlphaNumeric(strDepositCost[j]);
            //    //                     Thread.Sleep(100);
            //    //                 }
            //    //                 //if (((DOMInputElement)_pricePerUnit).Value != _price) result = false;                                       
            //    //             }
            //    //        }
            //    //    }

            //    //}
            //    //else throw new Exception("Add Item button not found for refno " + strUCRefNo);
            //    #endregion
            //}
            #endregion
          

            #region Other Cost
            //if (strOtherCost != "")
            //{
            //    counter = counter + 1;
            //    fillExtraItems(counter, strOtherCost, "OTHER CHARGE");
            //}
            #endregion

            #region Freight Charge
            //if (strFreightCharge != "")
            //{
            //    if (strTransportModeCode != "4")//all others
            //    {
            //        DOMElement _Freight = _netWrapper.GetElementByTagName("section", "class", "inquiry-item additional-positions-item no-shrink", "Freight Charge (Default)",
            //            "form-field", "params", "value: positionSum, label: 'Amount', readonly: !$data.canUserWrite()", "input");
            //        if (_Freight != null)
            //        {
            //            SetValue(_Freight, strFreightCharge, "input");
            //            //   ((DOMInputElement)_Freight).Value = strFreightCharge;
            //            if (((DOMInputElement)_Freight).Value != strFreightCharge) result = false;
            //        }
            //    }
            //    else //Air
            //    {
            //        DOMElement _AirFreight = _netWrapper.GetElementByTagName("section", "class", "inquiry-item additional-positions-item no-shrink", "Freight Charge (Air Freight)",
            //                "form-field", "params", "value: positionSum, label: 'Amount', readonly: !$data.canUserWrite()", "input");
            //        if (_AirFreight != null)
            //        {
            //            SetValue(_AirFreight, strFreightCharge, "input");
            //            // ((DOMInputElement)_AirFreight).Value = strFreightCharge;
            //            if (((DOMInputElement)_AirFreight).Value != strFreightCharge) result = false;
            //        }
            //    }
            //}
            #endregion

            #region Header Discount
            if (AdditionalDiscount != Convert.ToDouble(0))
            {
                throw new Exception("Additional Discount found for refno " + strUCRefNo);
            }
            #endregion

            return result;
        }

        public void fillExtraItems(int counter, string _str, string _tag,double _qty,string _unit)
        {
            DOMElement _btnAddItem = _netWrapper.GetElementByType("button", "click:$component.addItem.bind($component)", "data-bind");
            if (_btnAddItem != null)
            {
                if (_netWrapper.ClickElement(true, false, _btnAddItem))
                {
                    if (_netWrapper.WaitForElementbyId("item-new-item-" + counter))
                    {
                        DOMElement eRow = (DOMElement)_netWrapper.GetElementbyId("item-new-item-" + counter);
                        if (eRow != null)
                        {
                            DOMElement _itemName = _netWrapper.GetElementByTag(eRow, "form-field", "params", "value: varianceName, placeholder: name, label: 'Offered item', readonly: !$data.canUserWrite() || isContractedItem, disabled: !available()", "input");//value: varianceName, placeholder: name, label: 'Offered item', readonly: !$data.canUserWrite(), disabled: !available()//site change on 09-04-19
                            if (_itemName != null)
                            {
                                DOMInputElement txtItemName = ((DOMInputElement)_itemName);
                                if (txtItemName != null)
                                {

                                    string _typeText = _tag;
                                    SetValue(_itemName, _typeText, "input");
                                }
                            }

                            DOMElement _itemQty = _netWrapper.GetElementByTag(eRow, "form-field", "params", "value: varianceQuantity, placeholder: quantity, label: 'Offered qty.', autofilled: pendingAcceptAutofill(), readonly: !$data.canUserWrite(), disabled: !available()", "input");
                            if (_itemQty != null)
                            {
                                SetValue(_itemQty, Convert.ToString(_qty), "input");
                            }

                            DOMSelectElement _itemUnit = (DOMSelectElement)_netWrapper.GetElementByTag(eRow, "form-field", "params", "value: varianceUnit, options: units, placeholder: unit, label: 'Offered unit', disabled: !available() || !$data.canUserWrite()", "select");
                            if (_itemUnit != null)
                            {
                                _itemUnit.Focus();
                                Thread.Sleep(300);
                                string _text = "";
                                if (_itemUnit.Value != _unit.Trim())
                                {
                                    List<DOMNode> children = _itemUnit.Children;
                                    List<DOMElement> options = new List<DOMElement>();
                                    foreach (DOMNode child in children)
                                    {
                                        if (child is DOMElement)
                                        {
                                            options.Add((DOMElement)child);
                                        }
                                    }
                                    int index = -1;

                                    foreach (DOMElement c in options)
                                    {
                                        index++;
                                        if (c.GetAttribute("value") != "")
                                        {
                                            if (_unit == c.GetAttribute("value"))
                                            {
                                                _text = c.InnerText;
                                                //   selIndex = index;
                                                break;
                                            }
                                        }
                                    }
                                    if (_text != "")
                                    {
                                        for (int j = 0; j < _text.Length; j++)
                                        {
                                            SetAlphaNumeric(_text[j]);
                                            Thread.Sleep(100);
                                        }
                                    }
                                }
                            }

                            DOMElement _pricePerUnit = _netWrapper.GetElementByTag(eRow, "form-field", "params", "value: pricePerUnit, label: 'Price per unit', autofilled: pendingAcceptAutofill(), readonly: !$data.canUserWrite(), disabled: !available() || $component.useQuotedPriceTotal()", "input");
                            if (_pricePerUnit != null)
                            {
                                SetValue(_pricePerUnit, _str, "input");
                            }
                        }
                    }
                }
                else throw new Exception("Unable to open new item window for refno " + strUCRefNo);
            }
            else throw new Exception("Add Item button not found for refno " + strUCRefNo);
        }

        //public void fillExtraItems(int counter,string _str,string _tag)
        //{
        //    DOMElement _btnAddItem = _netWrapper.GetElementByType("button", "click:$component.addItem.bind($component)", "data-bind");
        //    if (_btnAddItem != null)
        //    {
        //        //if (_netWrapper.ClickElement(_btnAddItem, true, "", "section", "id", "item-new-item-" + counter))
        //        if(_netWrapper.ClickElement(true,false,_btnAddItem))
        //        {
        //            if (_netWrapper.WaitForElementbyId("item-new-item-" + counter))
        //            {
        //                DOMElement eRow = (DOMElement)_netWrapper.GetElementbyId("item-new-item-" + counter);
        //                if (eRow != null)
        //                {
        //                    DOMElement _itemName = _netWrapper.GetElementByTag(eRow, "form-field", "params", "value: varianceName, placeholder: name, label: 'Offered item', readonly: !$data.canUserWrite(), disabled: !available()", "input");
        //                    if (_itemName != null)
        //                    {
        //                        DOMInputElement txtItemName = ((DOMInputElement)_itemName);
        //                        if (txtItemName != null)
        //                        {

        //                            string _typeText = _tag;
        //                            SetValue(_itemName, _typeText, "input");
        //                            //txtItemName.Focus();
        //                            //for (int j = 0; j < _typeText.Length; j++)
        //                            //{
        //                            //    SetAlphaNumeric(_typeText[j]);
        //                            //    Thread.Sleep(100);
        //                            //}
        //                        }
        //                    }

        //                    DOMElement _qty = _netWrapper.GetElementByTag(eRow, "form-field", "params", "value: varianceQuantity, placeholder: quantity, label: 'Offered qty.', autofilled: pendingAcceptAutofill(), readonly: !$data.canUserWrite(), disabled: !available()", "input");
        //                    if (_qty != null)
        //                    {
        //                        SetValue(_qty, "1", "input");
        //                        //DOMInputElement txtQffQty = ((DOMInputElement)_qty);
        //                        //txtQffQty.Focus();
        //                        //SetAlphaNumeric('1');
        //                        //Thread.Sleep(100);

        //                    }

        //                    DOMElement _pricePerUnit = _netWrapper.GetElementByTag(eRow, "form-field", "params", "value: pricePerUnit, label: 'Price per unit', autofilled: pendingAcceptAutofill(), readonly: !$data.canUserWrite(), disabled: !available() || $component.useQuotedPriceTotal()", "input");
        //                    if (_pricePerUnit != null)
        //                    {
        //                        SetValue(_pricePerUnit, _str, "input");
        //                        //DOMInputElement txtPrice = ((DOMInputElement)_pricePerUnit);

        //                        //txtPrice.Focus();
        //                        //Thread.Sleep(300);
        //                        //for (int j = 0; j < _str.Length; j++)
        //                        //{
        //                        //    SetAlphaNumeric(_str[j]);
        //                        //    Thread.Sleep(100);
        //                        //}
        //                        //if (((DOMInputElement)_pricePerUnit).Value != _price) result = false;                                       
        //                    }
        //                }
        //            }
        //        }
        //        else throw new Exception("Unable to open new item window for refno " + strUCRefNo);
        //    }
        //    else throw new Exception("Add Item button not found for refno " + strUCRefNo);
        //}

        public bool Fill_Supplier_Remarks()
        {
            bool result = true;
            _netWrapper.LogText = "Filling supplier remarks";
            try
            {
                DOMElement _eleRemark = null;
                List<DOMElement> lstremSection = _netWrapper.GetElementsByTagName("section", "class", "remarks no-shrink");
                if (lstremSection.Count > 0)
                {
                    List<DOMNode> remarksNodes = lstremSection[0].GetElementsByTagName("textarea");

                    foreach (DOMNode textNode in remarksNodes)
                    {
                        string dataBindValue = ((DOMElement)textNode).GetAttribute("data-bind");
                        if (dataBindValue.Contains("SupplierRemark"))
                        {
                            _eleRemark = (DOMTextAreaElement)textNode;
                            break;
                        }
                    }
                }

                // COMMENTED BY SANJITA
                //DOMElement _eleRemark = _netWrapper.GetElementByType("textarea", "data-bind", "attr.readonly: !$data.CanUserWrite() ,textinput:SupplierRemark");
                if (_eleRemark != null)
                {
                    _eleRemark.Focus();
                    Thread.Sleep(300);
                    for (int j = 0; j < strSupplierComment.Length; j++)
                    {
                        if (strSupplierComment[j] == '\\')
                        {
                        }
                        SetAlphaNumeric(strSupplierComment[j]);
                        Thread.Sleep(100);
                    }

                    string updatedRem = Convert.ToString(((DOMTextAreaElement)_eleRemark).Value);
                    if (updatedRem.Trim().Length == 0) result = false;
                }
                else
                {
                    throw new Exception("Supplier remark field not found on page.");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public void SubmitQuote(string MTML_QuoteFile)
        {
            string sFile = "";
            Dictionary<string, object> _dicAlert = new Dictionary<string, object>();
            //write submit code here
            DOMElement _eleSubmit = _netWrapper.GetElementByType("button", "click:$component.submit.bind($component),attr.disabled: !isValid()", "data-bind");
            if (_eleSubmit != null)
            {
                string LoadedURL = _netWrapper.browser.URL;
                string _isDisable = _eleSubmit.GetAttribute("disabled");
                if (_isDisable == "")
                {
                    if (_netWrapper.ClickElement(true, false, _eleSubmit))
                    {
                        //   _dicAlert = _netWrapper.dictCustomDialogueParams_OnAlert;
                        if (_netWrapper.WaitForElementbyId("modal modal-dialog modal-dialog-warning", true, false))
                        {
                            DOMElement _eleModal = _netWrapper.GetElementbyClass("modal modal-dialog modal-dialog-warning");
                            if (_eleModal != null)
                            {
                                DOMElement _eleOK = _netWrapper.GetElementByType("button", "ok", "title");
                                if (_eleOK != null)
                                {
                                    _eleOK.Click();
                                    Thread.Sleep(Convert.ToInt32(ConfigurationManager.AppSettings["THREAD_SLEEPTIME"].Trim()));
                                    DOMElement _eleSuccess = _netWrapper.GetElementbyClass("modal modal-dialog modal-dialog-info");
                                    if (_eleSuccess != null)
                                    {
                                        DOMElement _eleSuccessMsg = _netWrapper.GetElementByType("content", "html:content", "data-bind");
                                        if (_eleSuccessMsg != null)
                                        {
                                            string _msg = _eleSuccessMsg.TextContent;
                                            if (_msg.Contains("We successfully saved your offer."))
                                            {
                                                DOMElement _eleOK1 = _netWrapper.GetElementByType("button", "ok", "title");
                                                if (_eleOK1 != null)
                                                {
                                                    //_eleOK1.Click();
                                                    //if (loading())
                                                    if (NavigateToURL(strMessageNumber, Path.GetFileName(MTML_QuoteFile), strMTMLUploadPath) && strUCRefNo != "")//check
                                                    {

                                                        //download pdf file
                                                        DOMElement _btnDownld = _netWrapper.GetElementByType("button", "click:$component.downloadQuote.bind($component)", "data-bind");
                                                        if (_btnDownld != null)
                                                        {
                                                            LoadedURL = _netWrapper.browser.URL;
                                                            sFile = "AsiaticSuccess_" + strUCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                                                            string[] arrURL = LoadedURL.Split('/');
                                                            string URL = arrURL[0] + "//" + arrURL[2] + "/exapi/AnswerInquiry/" + arrURL[5] + "/download/quote";
                                                            _netWrapper.DownloadFile(URL, strScreenShotPath, sFile);
                                                            //added on 29-1-2019//added for wss requirement
                                                            if (Convert.ToString(ConfigurationManager.AppSettings["SEND_MAIL_FOR_SUBMIT"]).Trim().ToUpper() == "TRUE")
                                                            {
                                                                _netWrapper.LogText = "Generating mail notification...";
                                                                SendMailNotification(_interchange, "QUOTE", strUCRefNo.Trim(), "SUBMITTED", "Quote for REF '" + strUCRefNo.Trim() + "' submitted successfully.");
                                                            }//
                                                        }
                                                    }
                                                    else
                                                    {
                                                        sFile = strScreenShotPath + "\\AsiaticSuccess_" + strUCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".pdf";
                                                        _netWrapper.PrintPDF(sFile, true);
                                                    }
                                                    MoveFileToBackup(MTML_QuoteFile,sFile, "Quote submitted successfully.");
                                                }
                                                else MoveFileToError(MTML_QuoteFile, "Success ok button not found", "Quote");
                                            }
                                            else MoveFileToError(MTML_QuoteFile, "Unable to submit offer", "Quote");
                                        }
                                    }
                                    else MoveFileToError(MTML_QuoteFile, "Unable to get success message after submit offer", "Quote");
                                }
                            }
                        } else MoveFileToError(MTML_QuoteFile, "Unable to get warning dialog for Ref  " + strUCRefNo, strDocType);
                    }
                    else MoveFileToError(MTML_QuoteFile, "Unable to click submit button for Ref  " + strUCRefNo, strDocType);
                    //}
                    //    }
                    //}
                }
                else
                {
                    MoveFileToError(MTML_QuoteFile, "Submit button is disabled for Ref  " + strUCRefNo, strDocType);
                }
            }
        }

        //public bool loading()
        //{
        //  bool res = false; int maxcount = 60000, count = 0;
        //    while (count < maxcount)
        //    {
        //        DOMElement _btnDownloadPdf = _netWrapper.GetElementByType("button", "data-bind", "click:$component.downloadQuote.bind($component)");
        //        if (_btnDownloadPdf != null)
        //            {
        //                 res=true;
        //            }
                
        //    }
        //    return res;
        //}

        //public void DeclineQuote()
        //{
        //    string sFile = "";

        //    //write decline code here

        //    DOMElement _btnDownld = _netWrapper.GetElementByType("button", "data-bind", "click:$component.downloadQuote.bind($component)");
        //    if (_btnDownld != null)
        //    {
        //        sFile = "AsiaticDecline_" + strUCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
        //        _netWrapper.DownloadFileOnClick(_btnDownld, strScreenShotPath, sFile, false);
        //    }
        //    else
        //    {
        //        sFile = strScreenShotPath + "\\AsiaticDecline_" + strUCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".pdf";
        //        _netWrapper.PrintPDF(sFile, false);
        //    }
        //    MoveFileToBackup(sFile "");
        //}

        public void SetValue(DOMElement _element,string _eleValue,string _tag)
        {
            _element.Focus();
            if(_tag=="input")
            ((DOMInputElement)_element).Value = string.Empty;
            Thread.Sleep(300);
            for (int j = 0; j < _eleValue.Length; j++)
            {
                SetAlphaNumeric(_eleValue[j]);
                Thread.Sleep(100);
            }
        }

        public void MoveFileToError(string MTML_QuoteFile, string message, string DocType)
        {
            string sFile = strScreenShotPath + "\\Asiatic_" + DocType + "_Error_" + strUCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".pdf";
            _netWrapper.PrintPDF(sFile, false);
            _netWrapper.LogText = message;
            if (File.Exists(strMTMLUploadPath + "\\" + Path.GetFileName(MTML_QuoteFile)))
            {
                if (File.Exists(strMTMLUploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile))) File.Delete(strMTMLUploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
                File.Move(strMTMLUploadPath + "\\" + Path.GetFileName(MTML_QuoteFile), strMTMLUploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
                Thread.Sleep(1000);
            }

            if (File.Exists(strMTMLUploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile)))
                CreateQuoteAuditFile(sFile, "Asiatic Quote", strUCRefNo, "Error", message);
        }

        public void LoadInterchangeDetails()
        {
            try
            {
                _netWrapper.LogText = "Started Loading interchange object.";
                if (_interchange != null)
                {
                    if (_interchange.Recipient != null)
                        strBuyer = _interchange.Recipient;

                    if (_interchange.Sender != null)
                        strSupplier = _interchange.Sender;

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

                        if (_interchange.DocumentHeader.TransportModeCode != null)
                            strTransportModeCode = Convert.ToString(_interchange.DocumentHeader.TransportModeCode);

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
                            else if (_interchange.DocumentHeader.Comments[i].Qualifier == CommentTypes.ZTC)
                                strTermCondition = _interchange.DocumentHeader.Comments[i].Value;
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
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.BuyerItemTotal_90)//16-12-2017
                                strBuyerTotal = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.Deposit_97)//16-2-2018
                                strDepositCost = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.TaxCost_99)
                                strFreightCharge = Convert.ToString(Convert.ToDouble(strFreightCharge) + Convert.ToDouble(_interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString()));
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.OtherCost_98)
                                strOtherCost = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();//
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
                                    if (strLeadDays == "0")
                                    {
                                        TimeSpan t = dtDelDate.Subtract(DateTime.Today);
                                        double totalDays = t.TotalDays;
                                        strLeadDays = Convert.ToString(totalDays);
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
                                        strExpDate = ExpDate.ToString("M/d/yyyy");
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

        public void ClearCommonVariables()
        {
            strDocType = ""; strMessageNumber = ""; strCurrency = ""; strMsgNumber = ""; strMsgRefNumber = ""; strUCRefNo = ""; strAAGRefNo = "";
            strLesRecordID = ""; BuyerName = ""; strBuyerPhone = ""; strBuyerEmail = ""; strBuyerFax = ""; strSupplierName = ""; strSupplierPhone = "";
            strSupplierEmail = ""; strSupplierFax = ""; strPortName = ""; strPortCode = ""; strVesselName = ""; strSupplierComment = ""; strPayTerms = "";
            strPackingCost = ""; strFreightCharge = ""; strTotalLineItemsAmount = ""; strGrandTotal = ""; strAllowance = ""; strDelvDate = "";
            strExpDate = ""; strLeadDays = ""; strBuyerTotal = ""; strDepositCost = ""; strOtherCost = ""; strTermCondition = ""; strTransportModeCode = "";
            IsDecline = false;
            IsAltItemAllowed = 0; IsPriceAveraged = 0; IsUOMChanged = 0;
            AdditionalDiscount = 0;
            lstExtraItems.Clear();
        }

        private void SendMailNotification(MTMLInterchange _interchange, string DocType, string VRNO, string ActionType, string Message)
        {
            try
            {
                string MailFromDefault = Convert.ToString(ConfigurationManager.AppSettings["MAIL_FROM"]);
                string MailBccDefault = Convert.ToString(ConfigurationManager.AppSettings["MAIL_BCC"]);
                string MailCcDefault = Convert.ToString(ConfigurationManager.AppSettings["MAIL_CC"]);

                string BuyerCode = Convert.ToString(_interchange.Recipient).Trim();
                string SuppCode = Convert.ToString(_interchange.Sender).Trim();
                string BuyerID = Convert.ToString(_interchange.BuyerSuppInfo.BuyerID).Trim();
                string SupplierID = Convert.ToString(_interchange.BuyerSuppInfo.SupplierID).Trim();

                string MailAuditPath = Convert.ToString(ConfigurationManager.AppSettings["MAIL_AUDIT_PATH"]);
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
                    foreach (MTML.GENERATOR.Party _partyObj in _interchange.DocumentHeader.PartyAddresses)
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

                    #region // NOTIFY TO BUYER //
                    if (notifyBuyer == "YES")
                    {
                        // Send Mail Notification for Buyer
                        string MailFrom = "", MailTo = ByrMailID.Trim().Replace("E-mail:", "").Trim(), mailBody = "";

                        if (MailTo.Trim() != "")
                        {
                            int QuotationID = Convert.ToInt32(_interchange.BuyerSuppInfo.RecordID);
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
                            _netWrapper.LogText = "Mail Send to Buyer Email -" + MailTo.Trim() + " .";
                        }
                        else
                        {
                            _netWrapper.LogText = "Unable to send mail notification to buyer; Buyer Mailid is empty.";
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
                            _netWrapper.LogText = "Mail Send to Supplier Email -" + MailTo.Trim() + " .";
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
                _netWrapper.LogText = "Unable to create Mail notification template. Error : " + ex.StackTrace;
            }
        }
        #endregion

        #region Common Routine
        public void CreateQuoteAuditFile(string FileName, string Module, string RefNo, string LogType, string Audit)
        {
            try
            {
                string Server = Convert.ToString(ConfigurationManager.AppSettings["SERVER"]);
                string Processor = Convert.ToString(ConfigurationManager.AppSettings["PROCESSNAME"]);

                string auditPath = strAuditPath;
                if (!Directory.Exists(auditPath)) Directory.CreateDirectory(auditPath);

                string auditData = "";
                if (auditData.Trim().Length > 0) auditData += Environment.NewLine;
                auditData += "" + "|";
                auditData += "" + "|";
                auditData += Module + "|";
                auditData += Path.GetFileName(FileName) + "|";
                auditData += RefNo + "|";
                auditData += LogType + "|";
                auditData += DateTime.Now.ToString("yy-MM-dd HH:mm") + " : " + Audit+"|";
                auditData += "" + "|";
                auditData += Server + "|";
                auditData += strBuyer + "|";
                auditData += strSupplier + "|";
                auditData += Processor;
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

        public void MoveFileToBackup(string MTML_QuoteFile,string PrintFile, string message)
        {
            if (File.Exists(strMTMLUploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile))) File.Delete(strMTMLUploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile));
            File.Move(strMTMLUploadPath+"\\"+MTML_QuoteFile, strMTMLUploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile));
            Thread.Sleep(1000);


            if (File.Exists(strMTMLUploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile)))
            {
                if (!IsDecline)
                {
                    if (PrintFile != "") CreateQuoteAuditFile(PrintFile, "Asiatic Quote", strUCRefNo, "Success", message);
                    else CreateQuoteAuditFile(MTML_QuoteFile, "Asiatic Quote", strUCRefNo, "Success", message);
                }
                else
                    CreateQuoteAuditFile(MTML_QuoteFile, "Asiatic Quote", strUCRefNo, "Declined", message);
            }
            _netWrapper.LogText = message;
        }

        //public void SetAlphaNumeric(char value)
        //{
        //    switch (value)
        //    {
        //        case 'A':
        //            KeyParams paramers_A = new KeyParams(VirtualKeyCode.VK_A, 'A');
        //            _netWrapper.browser.KeyDown(paramers_A);
        //            _netWrapper.browser.KeyUp(paramers_A);
        //            break;
        //        case 'a':
        //            KeyParams paramers_a = new KeyParams(VirtualKeyCode.VK_A, 'a');
        //            _netWrapper.browser.KeyDown(paramers_a);
        //            _netWrapper.browser.KeyUp(paramers_a);
        //            break;
        //        case 'B':
        //            KeyParams paramers_B = new KeyParams(VirtualKeyCode.VK_B, 'B');
        //            _netWrapper.browser.KeyDown(paramers_B);
        //            _netWrapper.browser.KeyUp(paramers_B);
        //            break;
        //        case 'b':
        //            KeyParams paramers_b = new KeyParams(VirtualKeyCode.VK_B, 'b');
        //            _netWrapper.browser.KeyDown(paramers_b);
        //            _netWrapper.browser.KeyUp(paramers_b);
        //            break;
        //        case 'C':
        //            KeyParams paramers_C = new KeyParams(VirtualKeyCode.VK_C, 'C');
        //            _netWrapper.browser.KeyDown(paramers_C);
        //            _netWrapper.browser.KeyUp(paramers_C);
        //            break;
        //        case 'c':
        //            KeyParams paramers_c = new KeyParams(VirtualKeyCode.VK_C, 'c');
        //            _netWrapper.browser.KeyDown(paramers_c);
        //            _netWrapper.browser.KeyUp(paramers_c);
        //            break;
        //        case 'D':
        //            KeyParams paramers_D = new KeyParams(VirtualKeyCode.VK_D, 'D');
        //            _netWrapper.browser.KeyDown(paramers_D);
        //            _netWrapper.browser.KeyUp(paramers_D);
        //            break;
        //        case 'd':
        //            KeyParams paramers_d = new KeyParams(VirtualKeyCode.VK_D, 'd');
        //            _netWrapper.browser.KeyDown(paramers_d);
        //            _netWrapper.browser.KeyUp(paramers_d);
        //            break;
        //        case 'E':
        //            KeyParams paramers_E = new KeyParams(VirtualKeyCode.VK_E, 'E');
        //            _netWrapper.browser.KeyDown(paramers_E);
        //            _netWrapper.browser.KeyUp(paramers_E);
        //            break;
        //        case 'e':
        //            KeyParams paramers_e = new KeyParams(VirtualKeyCode.VK_E, 'e');
        //            _netWrapper.browser.KeyDown(paramers_e);
        //            _netWrapper.browser.KeyUp(paramers_e);
        //            break;
        //        case 'F':
        //            KeyParams paramers_F = new KeyParams(VirtualKeyCode.VK_F, 'F');
        //            _netWrapper.browser.KeyDown(paramers_F);
        //            _netWrapper.browser.KeyUp(paramers_F);
        //            break;
        //        case 'f':
        //            KeyParams paramers_f = new KeyParams(VirtualKeyCode.VK_F, 'f');
        //            _netWrapper.browser.KeyDown(paramers_f);
        //            _netWrapper.browser.KeyUp(paramers_f);
        //            break;
        //        case 'G':
        //            KeyParams paramers_G = new KeyParams(VirtualKeyCode.VK_G, 'G');
        //            _netWrapper.browser.KeyDown(paramers_G);
        //            _netWrapper.browser.KeyUp(paramers_G);
        //            break;
        //        case 'g':
        //            KeyParams paramers_g = new KeyParams(VirtualKeyCode.VK_G, 'g');
        //            _netWrapper.browser.KeyDown(paramers_g);
        //            _netWrapper.browser.KeyUp(paramers_g);
        //            break;
        //        case 'H':
        //            KeyParams paramers_H = new KeyParams(VirtualKeyCode.VK_H, 'H');
        //            _netWrapper.browser.KeyDown(paramers_H);
        //            _netWrapper.browser.KeyUp(paramers_H);
        //            break;
        //        case 'h':
        //            KeyParams paramers_h = new KeyParams(VirtualKeyCode.VK_H, 'h');
        //            _netWrapper.browser.KeyDown(paramers_h);
        //            _netWrapper.browser.KeyUp(paramers_h);
        //            break;
        //        case 'I':
        //            KeyParams paramers_I = new KeyParams(VirtualKeyCode.VK_I, 'I');
        //            _netWrapper.browser.KeyDown(paramers_I);
        //            _netWrapper.browser.KeyUp(paramers_I);
        //            break;
        //        case 'i':
        //            KeyParams paramers_i = new KeyParams(VirtualKeyCode.VK_I, 'i');
        //            _netWrapper.browser.KeyDown(paramers_i);
        //            _netWrapper.browser.KeyUp(paramers_i);
        //            break;
        //        case 'J':
        //            KeyParams paramers_J = new KeyParams(VirtualKeyCode.VK_J, 'J');
        //            _netWrapper.browser.KeyDown(paramers_J);
        //            _netWrapper.browser.KeyUp(paramers_J);
        //            break;
        //        case 'j':
        //            KeyParams paramers_j = new KeyParams(VirtualKeyCode.VK_J, 'j');
        //            _netWrapper.browser.KeyDown(paramers_j);
        //            _netWrapper.browser.KeyUp(paramers_j);
        //            break;
        //        case 'K':
        //            KeyParams paramers_K = new KeyParams(VirtualKeyCode.VK_K, 'K');
        //            _netWrapper.browser.KeyDown(paramers_K);
        //            _netWrapper.browser.KeyUp(paramers_K);
        //            break;
        //        case 'k':
        //            KeyParams paramers_k = new KeyParams(VirtualKeyCode.VK_K, 'k');
        //            _netWrapper.browser.KeyDown(paramers_k);
        //            _netWrapper.browser.KeyUp(paramers_k);
        //            break;
        //        case 'L':
        //            KeyParams paramers_L = new KeyParams(VirtualKeyCode.VK_L, 'L');
        //            _netWrapper.browser.KeyDown(paramers_L);
        //            _netWrapper.browser.KeyUp(paramers_L);
        //            break;
        //        case 'l':
        //            KeyParams paramers_l = new KeyParams(VirtualKeyCode.VK_L, 'l');
        //            _netWrapper.browser.KeyDown(paramers_l);
        //            _netWrapper.browser.KeyUp(paramers_l);
        //            break;
        //        case 'M':
        //            KeyParams paramers_M = new KeyParams(VirtualKeyCode.VK_M, 'M');
        //            _netWrapper.browser.KeyDown(paramers_M);
        //            _netWrapper.browser.KeyUp(paramers_M);
        //            break;
        //        case 'm':
        //            KeyParams paramers_m = new KeyParams(VirtualKeyCode.VK_M, 'm');
        //            _netWrapper.browser.KeyDown(paramers_m);
        //            _netWrapper.browser.KeyUp(paramers_m);
        //            break;
        //        case 'N':
        //            KeyParams paramers_N = new KeyParams(VirtualKeyCode.VK_N, 'N');
        //            _netWrapper.browser.KeyDown(paramers_N);
        //            _netWrapper.browser.KeyUp(paramers_N);
        //            break;
        //        case 'n':
        //            KeyParams paramers_n = new KeyParams(VirtualKeyCode.VK_N, 'n');
        //            _netWrapper.browser.KeyDown(paramers_n);
        //            _netWrapper.browser.KeyUp(paramers_n);
        //            break;
        //        case 'O':
        //            KeyParams paramers_O = new KeyParams(VirtualKeyCode.VK_O, 'O');
        //            _netWrapper.browser.KeyDown(paramers_O);
        //            _netWrapper.browser.KeyUp(paramers_O);
        //            break;
        //        case 'o':
        //            KeyParams paramers_o = new KeyParams(VirtualKeyCode.VK_O, 'o');
        //            _netWrapper.browser.KeyDown(paramers_o);
        //            _netWrapper.browser.KeyUp(paramers_o);
        //            break;
        //        case 'P':
        //            KeyParams paramers_P = new KeyParams(VirtualKeyCode.VK_P, 'P');
        //            _netWrapper.browser.KeyDown(paramers_P);
        //            _netWrapper.browser.KeyUp(paramers_P);
        //            break;
        //        case 'p':
        //            KeyParams paramers_p = new KeyParams(VirtualKeyCode.VK_P, 'p');
        //            _netWrapper.browser.KeyDown(paramers_p);
        //            _netWrapper.browser.KeyUp(paramers_p);
        //            break;
        //        case 'Q':
        //            KeyParams paramers_Q = new KeyParams(VirtualKeyCode.VK_Q, 'Q');
        //            _netWrapper.browser.KeyDown(paramers_Q);
        //            _netWrapper.browser.KeyUp(paramers_Q);
        //            break;
        //        case 'q':
        //            KeyParams paramers_q = new KeyParams(VirtualKeyCode.VK_Q, 'q');
        //            _netWrapper.browser.KeyDown(paramers_q);
        //            _netWrapper.browser.KeyUp(paramers_q);
        //            break;
        //        case 'R':
        //            KeyParams paramers_R = new KeyParams(VirtualKeyCode.VK_R, 'R');
        //            _netWrapper.browser.KeyDown(paramers_R);
        //            _netWrapper.browser.KeyUp(paramers_R);
        //            break;
        //        case 'r':
        //            KeyParams paramers_r = new KeyParams(VirtualKeyCode.VK_R, 'r');
        //            _netWrapper.browser.KeyDown(paramers_r);
        //            _netWrapper.browser.KeyUp(paramers_r);
        //            break;
        //        case 'S':
        //            KeyParams paramers_S = new KeyParams(VirtualKeyCode.VK_S, 'S');
        //            _netWrapper.browser.KeyDown(paramers_S);
        //            _netWrapper.browser.KeyUp(paramers_S);
        //            break;
        //        case 's':
        //            KeyParams paramers_s = new KeyParams(VirtualKeyCode.VK_S, 's');
        //            _netWrapper.browser.KeyDown(paramers_s);
        //            _netWrapper.browser.KeyUp(paramers_s);
        //            break;
        //        case 'T':
        //            KeyParams paramers_T = new KeyParams(VirtualKeyCode.VK_T, 'T');
        //            _netWrapper.browser.KeyDown(paramers_T);
        //            _netWrapper.browser.KeyUp(paramers_T);
        //            break;
        //        case 't':
        //            KeyParams paramers_t = new KeyParams(VirtualKeyCode.VK_T, 't');
        //            _netWrapper.browser.KeyDown(paramers_t);
        //            _netWrapper.browser.KeyUp(paramers_t);
        //            break;
        //        case 'U':
        //            KeyParams paramers_U = new KeyParams(VirtualKeyCode.VK_U, 'U');
        //            _netWrapper.browser.KeyDown(paramers_U);
        //            _netWrapper.browser.KeyUp(paramers_U);
        //            break;
        //        case 'u':
        //            KeyParams paramers_u = new KeyParams(VirtualKeyCode.VK_U, 'u');
        //            _netWrapper.browser.KeyDown(paramers_u);
        //            _netWrapper.browser.KeyUp(paramers_u);
        //            break;
        //        case 'V':
        //            KeyParams paramers_V = new KeyParams(VirtualKeyCode.VK_V, 'V');
        //            _netWrapper.browser.KeyDown(paramers_V);
        //            _netWrapper.browser.KeyUp(paramers_V);
        //            break;
        //        case 'v':
        //            KeyParams paramers_v = new KeyParams(VirtualKeyCode.VK_V, 'v');
        //            _netWrapper.browser.KeyDown(paramers_v);
        //            _netWrapper.browser.KeyUp(paramers_v);
        //            break;
        //        case 'W':
        //            KeyParams paramers_W = new KeyParams(VirtualKeyCode.VK_W, 'W');
        //            _netWrapper.browser.KeyDown(paramers_W);
        //            _netWrapper.browser.KeyUp(paramers_W);
        //            break;
        //        case 'w':
        //            KeyParams paramers_w = new KeyParams(VirtualKeyCode.VK_W, 'w');
        //            _netWrapper.browser.KeyDown(paramers_w);
        //            _netWrapper.browser.KeyUp(paramers_w);
        //            break;
        //        case 'X':
        //            KeyParams paramers_X = new KeyParams(VirtualKeyCode.VK_X, 'X');
        //            _netWrapper.browser.KeyDown(paramers_X);
        //            _netWrapper.browser.KeyUp(paramers_X);
        //            break;
        //        case 'x':
        //            KeyParams paramers_x = new KeyParams(VirtualKeyCode.VK_X, 'x');
        //            _netWrapper.browser.KeyDown(paramers_x);
        //            _netWrapper.browser.KeyUp(paramers_x);
        //            break;
        //        case 'Y':
        //            KeyParams paramers_Y = new KeyParams(VirtualKeyCode.VK_Y, 'Y');
        //            _netWrapper.browser.KeyDown(paramers_Y);
        //            _netWrapper.browser.KeyUp(paramers_Y);
        //            break;
        //        case 'y':
        //            KeyParams paramers_y = new KeyParams(VirtualKeyCode.VK_Y, 'y');
        //            _netWrapper.browser.KeyDown(paramers_y);
        //            _netWrapper.browser.KeyUp(paramers_y);
        //            break;
        //        case 'Z':
        //            KeyParams paramers_Z = new KeyParams(VirtualKeyCode.VK_Z, 'Z');
        //            _netWrapper.browser.KeyDown(paramers_Z);
        //            _netWrapper.browser.KeyUp(paramers_Z);
        //            break;
        //        case 'z':
        //            KeyParams paramers_z = new KeyParams(VirtualKeyCode.VK_Z, 'z');
        //            _netWrapper.browser.KeyDown(paramers_z);
        //            _netWrapper.browser.KeyUp(paramers_z);
        //            break;
        //        case '1':
        //            KeyParams paramers_1 = new KeyParams(VirtualKeyCode.VK_1, '1');
        //            _netWrapper.browser.KeyDown(paramers_1);
        //            _netWrapper.browser.KeyUp(paramers_1);
        //            break;
        //        case '2':
        //            KeyParams paramers_2 = new KeyParams(VirtualKeyCode.VK_2, '2');
        //            _netWrapper.browser.KeyDown(paramers_2);
        //            _netWrapper.browser.KeyUp(paramers_2);
        //            break;
        //        case '3':
        //            KeyParams paramers_3 = new KeyParams(VirtualKeyCode.VK_3, '3');
        //            _netWrapper.browser.KeyDown(paramers_3);
        //            _netWrapper.browser.KeyUp(paramers_3);
        //            break;
        //        case '4':
        //            KeyParams paramers_4 = new KeyParams(VirtualKeyCode.VK_4, '4');
        //            _netWrapper.browser.KeyDown(paramers_4);
        //            _netWrapper.browser.KeyUp(paramers_4);
        //            break;
        //        case '5':
        //            KeyParams paramers_5 = new KeyParams(VirtualKeyCode.VK_5, '5');
        //            _netWrapper.browser.KeyDown(paramers_5);
        //            _netWrapper.browser.KeyUp(paramers_5);
        //            break;
        //        case '6':
        //            KeyParams paramers_6 = new KeyParams(VirtualKeyCode.VK_6, '6');
        //            _netWrapper.browser.KeyDown(paramers_6);
        //            _netWrapper.browser.KeyUp(paramers_6);
        //            break;
        //        case '7':
        //            KeyParams paramers_7 = new KeyParams(VirtualKeyCode.VK_7, '7');
        //            _netWrapper.browser.KeyDown(paramers_7);
        //            _netWrapper.browser.KeyUp(paramers_7);
        //            break;
        //        case '8':
        //            KeyParams paramers_8 = new KeyParams(VirtualKeyCode.VK_8, '8');
        //            _netWrapper.browser.KeyDown(paramers_8);
        //            _netWrapper.browser.KeyUp(paramers_8);
        //            break;
        //        case '9':
        //            KeyParams paramers_9 = new KeyParams(VirtualKeyCode.VK_9, '9');
        //            _netWrapper.browser.KeyDown(paramers_9);
        //            _netWrapper.browser.KeyUp(paramers_9);
        //            break;
        //        case '0':
        //            KeyParams paramers_0 = new KeyParams(VirtualKeyCode.VK_0, '0');
        //            _netWrapper.browser.KeyDown(paramers_0);
        //            _netWrapper.browser.KeyUp(paramers_0);
        //            break;
        //        case ' ':
        //            KeyParams paramers = new KeyParams(VirtualKeyCode.SPACE, ' ');
        //            _netWrapper.browser.KeyDown(paramers);
        //            _netWrapper.browser.KeyUp(paramers);
        //            break;
        //        case '.':
        //            KeyParams paramers_per = new KeyParams(VirtualKeyCode.OEM_PERIOD, '.');
        //            _netWrapper.browser.KeyDown(paramers_per);
        //            _netWrapper.browser.KeyUp(paramers_per);
        //            break;
        //        case ',':
        //            KeyParams paramers_comma = new KeyParams(VirtualKeyCode.OEM_COMMA, ',');
        //            _netWrapper.browser.KeyDown(paramers_comma);
        //            _netWrapper.browser.KeyUp(paramers_comma);
        //            break;
        //        case '"':
        //            KeyParams paramers_quote = new KeyParams(VirtualKeyCode.OEM_7, '"');
        //            _netWrapper.browser.KeyDown(paramers_quote);
        //            _netWrapper.browser.KeyUp(paramers_quote);
        //            break;
        //        case '-':
        //            KeyParams paramers_dash = new KeyParams(VirtualKeyCode.OEM_MINUS, '-');
        //            _netWrapper.browser.KeyDown(paramers_dash);
        //            _netWrapper.browser.KeyUp(paramers_dash);
        //            break;
        //        case '/':
        //            KeyParams paramers_slash = new KeyParams(VirtualKeyCode.OEM_2, '/');
        //            _netWrapper.browser.KeyDown(paramers_slash);
        //            _netWrapper.browser.KeyUp(paramers_slash);
        //            break;
        //        case ':':
        //            KeyParams paramers_col = new KeyParams(VirtualKeyCode.OEM_1, ':');
        //            _netWrapper.browser.KeyDown(paramers_col);
        //            _netWrapper.browser.KeyUp(paramers_col);
        //            break;
        //        case ';':
        //            KeyParams paramers_semi = new KeyParams(VirtualKeyCode.OEM_1, ';');
        //            _netWrapper.browser.KeyDown(paramers_semi);
        //            _netWrapper.browser.KeyUp(paramers_semi);
        //            break;
        //        case '*':
        //            KeyParams paramers_multi = new KeyParams(VirtualKeyCode.MULTIPLY, '*');
        //            _netWrapper.browser.KeyDown(paramers_multi);
        //            _netWrapper.browser.KeyUp(paramers_multi);
        //            break;
        //        case '\'':
        //            KeyParams paramers_squote = new KeyParams(VirtualKeyCode.OEM_7, '\'');
        //            _netWrapper.browser.KeyDown(paramers_squote);
        //            _netWrapper.browser.KeyUp(paramers_squote);
        //            break;
        //        case '|':
        //            KeyParams paramers_pipe = new KeyParams(VirtualKeyCode.OEM_5, '|');
        //            _netWrapper.browser.KeyDown(paramers_pipe);
        //            _netWrapper.browser.KeyUp(paramers_pipe);
        //            break;
        //        case '\n':
        //            KeyParams paramers_enter = new KeyParams(VirtualKeyCode.RETURN, '\'');
        //            _netWrapper.browser.KeyDown(paramers_enter);
        //            _netWrapper.browser.KeyUp(paramers_enter);
        //            break;
        //    }
        //}
        #endregion
    }
}
