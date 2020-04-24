using Aspose.Pdf.Text;
using HtmlAgilityPack;
using LeSMTML.MTML;
using MTML.GENERATOR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Http_MTM_Routine
{
    public class Http_Download_Routine : LeSCommon.LeSCommon
    {
        string BuyerLinkCode = "", SuppLinkCode = "", AttachmentPath = "", CurrentURL = "", AuditDocPath = "", DATE_FORMAT = "dd-MMM-yyyy", isPortCheck = "false",
           DocType = "", MessageNumber = "", LeadDays = "", Currency = "", MsgNumber = "", MsgRefNumber = "", UCRefNo = "",
           AAGRefNo = "", LesRecordID = "", BuyerName = "", BuyerPhone = "", BuyerEmail = "", BuyerFax = "", supplierName = "",
           supplierPhone = "", supplierEmail = "", supplierFax = "", VesselName = "", PortName = "", PortCode = "", SupplierComment = "", PayTerms = "",
            PackingCost = "", FreightCharge = "", GrandTotal = "", Allowance = "", OtherCost = "", DepositCost = "", TotalLineItemsAmount = "", BuyerTotal = "", TaxCost = "", DtDelvDate = "", dtExpDate = "",
            BCode = "", SCode = "", DoneStatusPath = "",_errorLog="";
        string[] QuotePath;
        int RFQ_Counter = -1, IsAltItemAllowed = 0, IsPriceAveraged = 0, IsUOMChanged = 0;
        string[] settings = null;
        double extraItemsAmt = 0, additionalCost_freight = 0;

        bool SaveDoc = true, isQuote = false, IsDecline = false, isSaveAsDraft = false, isSubmitQuote = false, isDoneStatus = false,isChangedUrl=false;
        // Dictionary<string, string> _itemDetails = new Dictionary<string, string>();
        List<string> lstLinkPaths = new List<string>();
        List<string> lstUrls = new List<string>();
        LeSMTML.MTML.Section _section = null;
        FileInfo orgAttachFile = null;
        PDFRoutine _pdfRoutine = new PDFRoutine();
        public MTMLInterchange _interchange { get; set; }
        public MTML.GENERATOR.LineItemCollection _lineitem = null;

        public Http_Download_Routine()
        {
           // _section = new LeSMTML.MTML.Section();
        }

        public void SetConfigSettings()
        {
            IsUrlEncoded = true;
            // Link Path //
            if (ConfigurationManager.AppSettings["LINK_PATH"] != null)
            {
                string[] linkPaths = convert.ToString(ConfigurationManager.AppSettings["LINK_PATH"]).Trim().Split('|');
                lstLinkPaths.Clear();
                lstLinkPaths.AddRange(linkPaths);
                foreach (string _linkPath in lstLinkPaths)
                {
                    if (!Directory.Exists(_linkPath) && _linkPath.Trim().Length > 0) Directory.CreateDirectory(_linkPath);
                }
            }

            // XML Download Path
            if (ConfigurationManager.AppSettings["ESUPPLIER_INBOX"].Trim() != null)
            {
                DownloadPath = ConfigurationManager.AppSettings["ESUPPLIER_INBOX"].Trim();
                if (DownloadPath == "") DownloadPath = AppDomain.CurrentDomain.BaseDirectory + "XML";
                if (!Directory.Exists(DownloadPath) && DownloadPath.Trim().Length > 0) Directory.CreateDirectory(DownloadPath);

            }

            // Attachment Path
            if (ConfigurationManager.AppSettings["ATTACHMENT_PATH"].Trim() != null)
            {
                AttachmentPath = ConfigurationManager.AppSettings["ATTACHMENT_PATH"].Trim();
                if (AttachmentPath == "") AttachmentPath = AppDomain.CurrentDomain.BaseDirectory + "Attachments";
                if (!Directory.Exists(AttachmentPath) && AttachmentPath.Trim().Length > 0) Directory.CreateDirectory(AttachmentPath);
            }

            // Audit Path
            if (ConfigurationManager.AppSettings["ESUPPLIER_AUDIT"].Trim() != null)
            {
                AuditPath = ConfigurationManager.AppSettings["ESUPPLIER_AUDIT"].Trim();
                if (AuditPath == "") AuditPath = AppDomain.CurrentDomain.BaseDirectory + "AuditLog";
                if (!Directory.Exists(AuditPath) && AuditPath.Trim().Length > 0) Directory.CreateDirectory(AuditPath);
            }
            // Audit Doc Path
            if (ConfigurationManager.AppSettings["AUDIT_DOC_PATH"] != null)
            {
                AuditDocPath = ConfigurationManager.AppSettings["AUDIT_DOC_PATH"].Trim();
                if (!Directory.Exists(AuditDocPath) && AuditDocPath.Trim().Length > 0) Directory.CreateDirectory(AuditDocPath);
            }

            BuyerCode = convert.ToString(ConfigurationManager.AppSettings["BUYER_CODE"]).Trim().ToUpper();

            //Quotation Path
            if (ConfigurationManager.AppSettings["QUOTE_PATH"] != null)
            {
                QuotePath = ConfigurationManager.AppSettings["QUOTE_PATH"].Trim().Split('|');
                foreach (string _qPath in QuotePath)
                {
                    if (!Directory.Exists(_qPath) && _qPath.Trim().Length > 0) Directory.CreateDirectory(_qPath);
                }
            }

            //done status path
            if (ConfigurationManager.AppSettings["DONESTATUS_PATH"] != null)
            {
                DoneStatusPath = ConfigurationManager.AppSettings["DONESTATUS_PATH"].Trim();
                if (DoneStatusPath == "") DoneStatusPath = AppDomain.CurrentDomain.BaseDirectory + "Done_Status";
                if (!Directory.Exists(DoneStatusPath) && DoneStatusPath.Trim().Length > 0) Directory.CreateDirectory(DoneStatusPath);
            }
        }

        public void ReadRFQFromMail()
        {
            string strAuditmsg = "";
            try
            {
                SetConfigSettings();
                orgAttachFile = null;
                lstUrls.Clear();

                List<string> lstTempFileList = new List<string>();
                foreach (string linkPath in lstLinkPaths)
                {
                    if (linkPath.Trim().Length > 0)
                    {
                        string[] files = Directory.GetFiles(linkPath, "*.txt");
                        foreach (string strFile in files)
                        {
                            if (!lstTempFileList.Contains(strFile)) lstTempFileList.Add(strFile);
                            try
                            {
                                // Copy To Audit
                                File.Copy(strFile, AttachmentPath + "\\" + Path.GetFileName(strFile), true);
                            }
                            catch { }
                        }
                    }
                }

                // Scan txt files //
                foreach (string sFile in lstTempFileList)
                {
                    string _url = GetURL(sFile);
                    if (_url.Trim().Length > 0)
                    {
                        lstUrls.Add(sFile);
                    }
                    else
                    {
                        string fileType = GetFileType(sFile);
                        string orgFile = Path.GetFileName(sFile);
                        if (fileType == "MSG")
                        {
                            GetOriginalMailFiles(sFile, orgFile);
                            MovePdfToImportPath(sFile, orgFile);
                        }
                        else
                        {
                            // LogText = "Unable to process file - Link not found in file " + orgFile;
                            LogText = strAuditmsg = "Unable to process file" + orgFile + "due to link not found ";
                            CreateAuditFile(orgFile.Trim(), "MTM_HTTP_RFQ_Downloader", "", "Error", "LeS-1004.5:" + strAuditmsg, "0", "0", AuditPath);
                            MoveToError("", "", sFile, "");
                        }
                    }
                }

                // Process Files
                RFQ_Counter = lstUrls.Count - 1;
                if (RFQ_Counter > -1)
                {
                    string _URL = GetURL(lstUrls[RFQ_Counter]);
                    CurrentURL = _URL;
                    if (CurrentURL.Trim().Length > 0)
                    {
                        try
                        {
                            //if (_httpWrapper.LoadURL(CurrentURL, "input", "id", "btnSubmit", "", true)) //changed by simmy 14022018 : Error Uri string too long when isURLEncode=false
                            URL = CurrentURL;
                            if(isChangedUrl)//21-06-19
                            {
                                bool Isloaded = LoadURL("", "", "", true);//added by Kalpita on 05/08/2019 for fetching correct client URLs

                                if (_httpWrapper.GetElement("input", "id", "btnSubmit") != null)
                                {
                                    isChangedUrl = false;
                                    ProcessRFQ();
                                }
                            }
                            else if (LoadURL("input", "id", "btnSubmit"))
                            {
                                ProcessRFQ();
                            }
                            else
                            {
                                HtmlAgilityPack.HtmlNode _divMsg = _httpWrapper.GetElement("div", "id", "dvmodal");
                                if (_divMsg != null)
                                {
                                    if (_divMsg.InnerText.ToString().Trim().Contains("OOPS"))
                                        MovePdfToError("Invalid Link", "", true);
                                }
                            }
                            //isChangedUrl = false; 
                        }
                        catch (Exception ex)
                        {
                            LogText = "Error while loading URL : " + ex.Message;
                            //MovePdfToError("Error while loading URL : " + ex.Message, "", true);
                            MovePdfToError(ex.Message, "", true);
                        }
                    }
                }
                else
                {
                    LogText = "No RFQ Found !";
                    //  if (isQuote)
                    UploadQuote();
                }
            }
            catch (Exception ex)
            {
                LogText = "Error ReadRFQFromMail() : " + ex.Message;
                string eFile = "MTM_SHIP_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName("") + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                PrintScreen(AttachmentPath + "\\" + eFile);
            }
        }

        public void GetOriginalMailFiles(string sFile, string orgFile)
        {
            try
            {
                string[] oMsgFiles = Directory.GetFiles(AttachmentPath, Path.GetFileName(sFile).Replace(".txt", ".msg").Replace(".TXT", ".msg"));
                if (oMsgFiles.Length > 0)
                {
                    orgFile = orgFile.ToLower().Replace(".txt", ".msg");
                }
                else
                {
                    if (orgFile.Split('_').Length >= 2)
                    {
                        string newMsgFile = "";
                        string[] txtFiles = orgFile.Split('_');
                        foreach (string spltFileText in txtFiles)
                        {
                            string msgFile = Path.GetFileNameWithoutExtension(spltFileText) + ".msg";
                            string[] msgFiles = null;
                            if (Directory.Exists(AuditDocPath))
                            {
                                string AuditPathMonth = AuditDocPath + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMM");
                                if (Directory.Exists(AuditPathMonth)) msgFiles = Directory.GetFiles(AuditPathMonth, msgFile);
                                if (msgFiles == null || (msgFiles != null && msgFiles.Length == 0))
                                {
                                    string AuditPathLastMonth = AuditDocPath + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.AddMonths(-1).ToString("MMM");
                                    if (Directory.Exists(AuditPathLastMonth)) msgFiles = Directory.GetFiles(AuditPathLastMonth, msgFile);
                                }
                            }

                            if (msgFiles == null || (msgFiles != null && msgFiles.Length == 0))
                            {
                                if (Directory.Exists(AttachmentPath)) msgFiles = Directory.GetFiles(AttachmentPath, msgFile, SearchOption.AllDirectories);
                            }
                            if (msgFiles != null && msgFiles.Length > 0)
                            {
                                newMsgFile = msgFile;
                            }
                        }

                        if (newMsgFile.Length > 0) orgFile = Path.GetFileName(newMsgFile);
                        else
                        {
                            string[] spltFile = orgFile.Split('_');
                            for (int i = 0; i < spltFile.Length - 1; i++)
                            {
                                string msgFile = Path.GetFileNameWithoutExtension(spltFile[i]) + "_" + Path.GetFileNameWithoutExtension(spltFile[1]).Trim() + ".msg";
                                string[] msgFiles = Directory.GetFiles(AuditDocPath + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMM"), msgFile);
                               
                                if (msgFiles.Length == 0) msgFiles = Directory.GetFiles(AuditDocPath + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.AddMonths(-1).ToString("MMM"), msgFile);
                                if (msgFiles.Length == 0) msgFiles = Directory.GetFiles(AttachmentPath, msgFile);
                                if (msgFiles.Length > 0)
                                {
                                    newMsgFile = msgFile;
                                }
                            }
                            if (newMsgFile.Length > 0) orgFile = Path.GetFileName(newMsgFile);
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        public void MovePdfToImportPath(string sFile, string orgFile)
        {
            string srAuditmsg = "";
            // Search PDF file //
            string lnkFile = orgFile;
            string importDir = Path.GetDirectoryName(sFile);
            DirectoryInfo _importDir = new DirectoryInfo(importDir);
            if (!_importDir.Exists) _importDir.Create();

            FileInfo[] _pdfFiles = _importDir.GetFiles("*" + Path.GetFileNameWithoutExtension(lnkFile) + "*.pdf");
            if (_pdfFiles.Length > 0)
            {
                string linkFileContent = File.ReadAllText(sFile);
                bool moved = false;
                foreach (FileInfo pdfFile in _pdfFiles)
                {
                    string pdfPO = Path.GetFileName(pdfFile.Name);
                    string[] arr = pdfPO.Split('_');
                    string _supplierID = arr[0];
                    if (convert.ToString(ConfigurationManager.AppSettings[_supplierID + "_PDF"]) != "" && !pdfPO.Contains("RFQ"))
                    {
                        string PathSetting = convert.ToString(ConfigurationManager.AppSettings[_supplierID + "_PDF"]).Trim();
                        string[] settings = PathSetting.Trim().Split('|');

                        string COPY_Path = settings[0].Trim();
                        string textToSearch = "";
                        if (settings.Length > 1)
                            textToSearch = settings[1].Trim();

                        bool copyFile = true;
                        if (textToSearch.Trim().Length > 0) copyFile = false;
                        if (textToSearch.Trim().Length > 0 && linkFileContent.ToUpper().Contains(textToSearch.Trim().ToUpper()))
                        {
                            copyFile = true;
                        }

                        // Save to That Path //
                        if (COPY_Path.Trim().Length > 0 && !Directory.Exists(COPY_Path.Trim())) Directory.CreateDirectory(COPY_Path.Trim());
                        if (pdfFile.Exists)
                        {
                            if (copyFile)
                            {
                                pdfFile.CopyTo(COPY_Path + "\\" + pdfFile.Name, true);
                            }

                            MoveToBackup(pdfFile.FullName); // Move PDF to Backup
                            MoveToBackup(sFile);  // Move Link to Backup
                            LogText = "File '" + Path.GetFileName(sFile) + "' processed successfully.";
                        }
                        else
                        {
                            LogText = "No PDF file found for file " + Path.GetFileName(sFile);
                            // LogText = "Unable to process file - Link not found in file " + orgFile;
                            LogText = srAuditmsg = "Unable to process file" + orgFile + " due to link not found";//changed by Kalpita on 21/10/2019
                            CreateAuditFile(orgFile.Trim(), "MTM_HTTP_RFQ_Downloader", "", "Error", "LeS-1004.1:" + srAuditmsg, "", "", AuditPath);
                            MoveToError("", "", sFile, "");
                        }
                        moved = true;
                    }
                    else
                    {
                        if (arr.Length > 2)
                        {
                            _supplierID = arr[2];
                            if (convert.ToString(ConfigurationManager.AppSettings[_supplierID + "_PDF"]) != "" && !pdfPO.Contains("RFQ"))
                            {
                                string COPY_Path = convert.ToString(ConfigurationManager.AppSettings[_supplierID + "_PDF"]);
                                // Save to That Path
                                if (!Directory.Exists(COPY_Path.Trim())) Directory.CreateDirectory(COPY_Path.Trim());
                                if (pdfFile.Exists)
                                {
                                    pdfFile.CopyTo(COPY_Path + "\\" + pdfFile.Name, true);

                                    MoveToBackup(pdfFile.FullName); // Move PDF to Backup
                                    MoveToBackup(sFile);  // Move Link to Backup
                                    LogText = "File '" + Path.GetFileName(pdfFile.Name) + "' processed successfully.";
                                }
                                else
                                {
                                    LogText = "No PDF file found for file " + Path.GetFileName(sFile);
                                   // LogText = "Unable to process file - Link not found in file " + orgFile;
                                    LogText = srAuditmsg = "Unable to process file" + orgFile + " due to link not found";//changed by Kalpita on 21/10/2019
                                    MoveToError("LeS-1004.1:" + srAuditmsg, "", sFile, "");
                                }
                                moved = true;
                            }
                        }
                    }
                }

                if (_pdfFiles.Length > 0 && moved == false)
                {
                    foreach (FileInfo pdfFile in _pdfFiles)
                    {
                        if (pdfFile.Exists)
                        {
                            MoveToError("", "", pdfFile.FullName, "");
                        }
                    }
                }
                else if (_pdfFiles.Length > 0 && moved)
                {
                    LogText = "File '" + Path.GetFileName(sFile) + "' processed successfully.";
                }
            }
            else
            {
                LogText = "No PDF file found for file " + Path.GetFileName(sFile);
               // LogText = "Unable to process file - Link not found in file " + orgFile;
                LogText = srAuditmsg = "Unable to process file" + orgFile + " due to link not found";//changed by Kalpita on 21/10/2019
                CreateAuditFile(orgFile.Trim(), "MTM_HTTP_RFQ_Downloader", "", "Error", "LeS-1004.1:" + srAuditmsg, "", "", AuditPath);
                MoveToError("", "", sFile, "");
            }
        }

        private string GetURL(string emlFile)
        {
            string URL = "";
            this.URL = URL;
            try
            {
                string MTM_DOMAIN = convert.ToString(ConfigurationManager.AppSettings["MTM_DOMAIN"]);

                if (MTM_DOMAIN.Trim().Length > 0)
                {
                    //this.URL = File.ReadAllText(emlFile);
                    RichTextBox txt = new RichTextBox();
                    txt.Text = File.ReadAllText(emlFile);
                    //if (txt.Lines.Length > 1)
                    if (txt.Lines.Length > 0)
                    {
                        if (txt.Text.Contains("https://protect-de.mimecast.com"))
                        {
                            int Idx = txt.Text.IndexOf("\"" + "https://protect-de.mimecast.com");
                            if (Idx == -1) Idx = txt.Text.IndexOf("<" + "https://protect-de.mimecast.com");
                            if (Idx > 0)
                            {
                                int endIdx = txt.Text.Substring(Idx + 1).Trim().IndexOf("\"");
                                int greatInx = txt.Text.Substring(Idx + 1).Trim().IndexOf(">");
                                if (endIdx > greatInx) endIdx = -1;
                                if (endIdx == -1) endIdx = txt.Text.Substring(Idx + 1).Trim().IndexOf(">");
                                if (endIdx > 0)
                                {
                                    string oURL = txt.Text.Substring(Idx + 1).Substring(0, endIdx);
                                    this.URL = oURL.Trim('"').Trim().Replace("&amp;", "&").Replace(Environment.NewLine, "");
                                    if (LoadURL("input", "id", "btnSubmit"))
                                    {
                                        this.URL = _httpWrapper.GetElement("form", "id", "form1").GetAttributeValue("action", "").Trim();
                                        //   this.URL = "http://portal.mtmsm.com/public/pos/" + this.URL.Replace("./", "");
                                        this.URL = "http://portal.mtmsm.com/public/pos/" + this.URL.Replace("./", "").Trim('"').Trim().Replace("&amp;", "&").Replace(Environment.NewLine, "");//added by Kalpita on 05/08/2019
                                        isChangedUrl = true;
                                    }
                                    else this.URL = null;
                                }
                            }
                        }

                        if (this.URL == null || this.URL == "")
                        {
                            #region commented

                            // int urlIdx = txt.Text.IndexOf("\"" + MTM_DOMAIN);
                            // if (urlIdx == -1) urlIdx = txt.Text.IndexOf("<" + MTM_DOMAIN);

                            #endregion

                            int urlStartIdx = txt.Text.IndexOf(MTM_DOMAIN);//changed by Kalpita on 04/07/2019
                            if (urlStartIdx == 0)
                            {
                                this.URL = txt.Text.Trim().Replace("&amp;", "&").Replace(Environment.NewLine, "");
                            }
                            else if (urlStartIdx != 0)
                            {
                                int urlIdx = txt.Text.IndexOf("\"" + MTM_DOMAIN);
                                if (urlIdx == -1) urlIdx = txt.Text.IndexOf("<" + MTM_DOMAIN);

                                if (urlIdx > 0)
                                {
                                    string part1 = txt.Text.Substring(urlIdx + 1);

                                    int endUrlIndx = part1.Trim().IndexOf("\"");
                                    int greaterIndex = part1.Trim().IndexOf(">");
                                    if (endUrlIndx > greaterIndex) endUrlIndx = -1;
                                    if (endUrlIndx == -1) endUrlIndx = part1.Trim().IndexOf(">");
                                    if (endUrlIndx > 0)
                                    {
                                        string orgURL = part1.Substring(0, endUrlIndx);
                                        this.URL = orgURL.Trim('"').Trim().Replace("&amp;", "&").Replace(Environment.NewLine, "");
                                    }

                                    if (!this.URL.ToLower().Contains("bidid"))
                                    {
                                        if (!this.URL.ToLower().Contains("quotemanager.aspx?key="))
                                        {
                                            this.URL = "";
                                        }
                                    }
                                }
                                else
                                {
                                    #region on 26-2-2018 To move file in error, if domain not found in file
                                    LogText = "MTM Domain not found in " + Path.GetFileName(emlFile);
                                    //commented as 
                                    //File.Move(emlFile, Path.GetDirectoryName(emlFile) + "\\Error_Files\\" + Path.GetFileName(emlFile));//
                                    //string importDir = Path.GetDirectoryName(emlFile);
                                    //DirectoryInfo _importDir = new DirectoryInfo(importDir);
                                    //FileInfo[] _pdfFiles = _importDir.GetFiles("*" + Path.GetFileNameWithoutExtension(emlFile) + "*.pdf");
                                    //if (_pdfFiles.Length > 0)
                                    //{
                                    //    orgAttachFile = _pdfFiles[0];
                                    //}
                                    //if (orgAttachFile != null)
                                    //{
                                    //    if (File.Exists(Path.GetDirectoryName(orgAttachFile.FullName) + "\\Error_Files\\" + Path.GetFileName(orgAttachFile.FullName)))
                                    //        File.Delete(Path.GetDirectoryName(orgAttachFile.FullName) + "\\Error_Files\\" + Path.GetFileName(orgAttachFile.FullName));

                                    //    File.Move(orgAttachFile.FullName, Path.GetDirectoryName(orgAttachFile.FullName) + "\\Error_Files\\" + Path.GetFileName(orgAttachFile.FullName));
                                    //}
                                    //orgAttachFile = null;
                                    #endregion
                                    this.URL = "";
                                }
                            }
                            else
                            {
                                LogText = "MTM Domain not found in " + Path.GetFileName(emlFile); this.URL = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogText = "Error GetURL() : " + ex.Message;
            }
            return this.URL;
        }


        public void Cleanvariables()
        {

        }

        public void ProcessRFQ()
        {
            orgAttachFile = null;
            //SupplierCode = ""; BuyerCode = ""; SuppLinkCode = ""; isPortCheck = "false";//18-3-19
            string VRNO = "", SuppEmail = "";
            //if (CurrentURL.Contains("singapore.mtmshipmanagement.com") && RFQ_Counter >= 0)//22-02-2018//domain change
            if (CurrentURL.Contains(ConfigurationManager.AppSettings["MTM_DOMAIN"].Trim()) && RFQ_Counter >= 0)//22-02-2018//domain change
            {
                try
                {
                    #region // Search Attached PDF file //
                    string lnkFile = lstUrls[RFQ_Counter];
                    string importDir = Path.GetDirectoryName(lnkFile);
                    DirectoryInfo _importDir = new DirectoryInfo(importDir);
                    if (!_importDir.Exists) _importDir.Create();
                    FileInfo[] _pdfFiles = _importDir.GetFiles("*" + Path.GetFileNameWithoutExtension(lnkFile) + "*.pdf");
                    if (_pdfFiles.Length > 0)
                    {
                        orgAttachFile = _pdfFiles[0];
                    }
                    #endregion




                    HtmlAgilityPack.HtmlNode _divMsg = _httpWrapper.GetElement("div", "id", "dvmodal");
                    if (_divMsg == null)
                    {
                        #region //RFQ NO
                        HtmlAgilityPack.HtmlNode _lblRFQNO = _httpWrapper.GetElement("span", "id", "lblRFQNO");
                        if (_lblRFQNO != null) VRNO = convert.ToString(_lblRFQNO.InnerText).Trim().Replace(" ", "");
                        else throw new Exception("VRNO field not found!");
                        #endregion

                        #region //Supplier Email ID
                        HtmlAgilityPack.HtmlNode _lblSuppEmail = _httpWrapper.GetElement("span", "id", "lblVendorEmail");
                        if (_lblSuppEmail != null)
                        {
                            SuppEmail = convert.ToString(_lblSuppEmail.InnerText).Trim().Split(';')[0];
                            if (SuppEmail.Trim().Length > 0)
                            {
                                int _idx = SuppEmail.IndexOf("@");
                                if (_idx > 0) SuppEmail = SuppEmail.Substring(_idx + 1);
                            }
                        }
                        #endregion

                        GetBuyerSupplierDetail(SuppEmail.Trim()); // Set Buyer-Supplier Settings

                        if (CurrentURL.Trim().Contains("public/pos/quotemanager.aspx"))
                        {
                            ReadBrowserData_NewSite(); // Process New RFQ Formats
                            orgAttachFile = null;
                        }
                        else
                        {
                            string LinkFile = Path.GetFileName(lstUrls[RFQ_Counter]);
                            string eFile = "MTM_SHIP_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName("") + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                            if (PrintScreen(AttachmentPath + "\\" + eFile)) eFile = lstUrls[RFQ_Counter];
                            //MoveToError("Unable to process file " + LinkFile + "; Error - Invalid Webpage!", VRNO, LinkFile, eFile);
                            MoveToError("LeS-1004.2:Unable to process file " + LinkFile + " due to invalid link", VRNO, LinkFile, eFile);
                        }

                        RFQ_Counter--;
                        if (RFQ_Counter >= 0)
                        {
                            string _URL = GetURL(lstUrls[RFQ_Counter]);
                            CurrentURL = _URL.Trim();
                            if (CurrentURL.Trim().Length > 0)
                            {
                                //if (_httpWrapper.LoadURL(CurrentURL, "input", "id", "btnSubmit", "", true)) //changed by simmy 14022018 : Error Uri string too long when isURLEncode=false
                                URL = CurrentURL;
                                if (isChangedUrl)//21-06-19
                                {
                                    bool Isloaded = LoadURL("", "", "", true);//added by Kalpita on 05/08/2019

                                    if (_httpWrapper.GetElement("input", "id", "btnSubmit") != null)
                                    {
                                        isChangedUrl = false;
                                        ProcessRFQ();
                                    }
                                }
                                else if (LoadURL("input", "id", "btnSubmit"))
                                {
                                    ProcessRFQ();
                                }
                                else if (_httpWrapper.GetElement("div", "id", "dvmodal").InnerText.ToString().Trim().Contains("OOPS"))
                                {
                                    MovePdfToError("Invalid Link", VRNO, false);
                                }
                               // isChangedUrl = false;
                            }
                        }
                    }
                    else
                    {
                        if (_httpWrapper.GetElement("div", "id", "dvmodal").InnerText.ToString().Trim().Contains("OOPS"))
                            MovePdfToError("Invalid Link", VRNO, false);
                    }
                }
                catch (Exception ex)
                {
                    MoveToNextRFQ(ex.Message, VRNO);
                }
            }
        }

        private void GetBuyerSupplierDetail(string SuppDomain)
        {
            try
            {
                Uri _Uri = new Uri(CurrentURL);
                string BuyerDomain = _Uri.Host;
                if (convert.ToString(ConfigurationManager.AppSettings["MTM_DOMAIN"]).Trim().Contains(BuyerDomain))
                {
                    if (ConfigurationManager.AppSettings[SuppDomain] != null)
                    {
                        SuppLinkCode = convert.ToString(ConfigurationManager.AppSettings[SuppDomain]).Trim();

                        if (ConfigurationManager.AppSettings[BuyerCode + "-" + SuppLinkCode] != null)
                        {
                            settings = convert.ToString(ConfigurationManager.AppSettings[BuyerCode + "-" + SuppLinkCode]).Trim().Split('|');
                            if (settings.Length == 3)
                            {
                                SupplierCode = settings[0].Trim().ToUpper();
                                BuyerLinkCode = settings[1].Trim().ToUpper();
                                SuppLinkCode = settings[2].Trim().ToUpper();
                            }
                            else if (settings.Length > 3)//for rms//08-02-2018
                            {
                                SupplierCode = settings[0].Trim().ToUpper();
                                BuyerLinkCode = settings[1].Trim().ToUpper();
                                SuppLinkCode = settings[2].Trim().ToUpper();
                                isPortCheck = settings[3].Trim();
                                //   isQuote = convert.ToBoolean(settings[4].Trim());
                            }
                        }
                        else throw new Exception("Buyer-Supplier Link Settings not found.");
                    }
                    else throw new Exception("Supplier Domain not found");
                }
                else throw new Exception("Invalid Buyer Domain.");
            }
            catch (Exception ex)
            {
                LogText = "Error GetBuyerSupplierDetail() : " + ex.Message + Environment.NewLine + ex.StackTrace;
                throw ex;
            }
        }

        #region // function of New Format //
        private void ReadBrowserData_NewSite()
        {
            string LinkFile = "",strAuditmsg="";
            string VRNO = "";
            try
            {
                // Copy Files to Audit Files 
                File.Copy(lstUrls[RFQ_Counter], AuditDocPath + "\\" + Path.GetFileName(lstUrls[RFQ_Counter]), true);

                MTMRFQ _mtmRFQ = new MTMRFQ();

                string fileType = GetFileType(lstUrls[RFQ_Counter]);
                LinkFile = Path.GetFileName(lstUrls[RFQ_Counter]);

                GetCommonDetails(ref _mtmRFQ, ref VRNO);
                GetEquipmentDetails(ref _mtmRFQ);

                #region // Get Vessel, Port, Vessel ETA & ETD //
                string Vessel = "", Port = "";
                List<string> byrAddr = new List<string>();
                GetVesselDetails(ref _mtmRFQ, out Vessel, out Port, out byrAddr);
                SetPort(Port);//for rms //08-02-2018
                #endregion

                GetAddress_NewSite(ref _mtmRFQ, Vessel, Port, byrAddr); // Party Address
                _mtmRFQ.RFQ.LineItems = GetItems_NewSite(); // Line Items

                if (SaveDoc)
                {
                    // Search PDF file
                    if (orgAttachFile != null && orgAttachFile.Exists == true)
                    {
                        _mtmRFQ.RFQ.OriginalFile = orgAttachFile.Name;
                        if (orgAttachFile.Exists)
                        {
                            if (!Directory.Exists(Path.GetDirectoryName(AttachmentPath))) Directory.CreateDirectory(Path.GetDirectoryName(AttachmentPath));
                            orgAttachFile.CopyTo(AttachmentPath + "\\" + orgAttachFile.Name, true);
                        }
                    }
                    else
                    {
                        // if pdf file not found then process with screen shot
                        string eFile = "MTM_SHIP_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                        if (PrintScreen(AttachmentPath + "\\" + eFile))
                        {
                            _mtmRFQ.RFQ.OriginalFile = eFile;
                        }
                    }
                }

                //string _exportfile = DownloadPath + "\\RFQ_" + VRNO + "_" + BuyerCode + "_" + SupplierCode + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".xml";
                string _exportfile = DownloadPath + "\\"+Path.GetFileNameWithoutExtension(LinkFile)+"_RFQ_" + VRNO + "_" + BuyerCode + "_" + SupplierCode + ".xml";
                _mtmRFQ.WriteRFQ(_exportfile);

                if (File.Exists(_exportfile))
                {
                    DownloadFile(VRNO, LinkFile);
                }
                else
                {
                    string eFile = "MTM_SHIP_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                    if (PrintScreen(AttachmentPath + "\\" + eFile)) eFile = lstUrls[RFQ_Counter];
                    LogText = strAuditmsg = "Unable to process file for RFQ " + VRNO + ".";
                    MoveToError("LeS-1004:" + strAuditmsg, VRNO, lstUrls[RFQ_Counter], eFile);
                }
            }
            catch (Exception ex)
            {
                string eFile = "MTM_SHIP_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                if (PrintScreen(AttachmentPath + "\\" + eFile)) eFile = lstUrls[RFQ_Counter];
                LogText = "Error in ReadBrowserData_NewSite() : " + ex.Message;
                MoveToError("LeS-1004:Unable to process file " + LinkFile, "", lstUrls[RFQ_Counter], eFile);
            }

        }

        public void GetCommonDetails(ref MTMRFQ _mtmRFQ, ref string VRNO)
        {
            #region // VRNO //
            HtmlAgilityPack.HtmlNode _lblRFQNO = _httpWrapper.GetElement("span", "id", "lblRFQNO");
            if (_lblRFQNO != null) VRNO = convert.ToString(_lblRFQNO.InnerText).Trim().Replace(" ", "");
            else throw new Exception("VRNO field not found!");
            #endregion

            #region // Doc Date //
            string DocDate = "";
            HtmlAgilityPack.HtmlNode _lblDateCreated = _httpWrapper.GetElement("span", "id", "lblDateCreated");
            if (_lblDateCreated != null) DocDate = convert.ToString(_lblDateCreated.InnerText);
            else DocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            #endregion

            #region // bidID / KeyID //
            string[] strURL = CurrentURL.Split('&');
            string bid = strURL[0];
            int index = bid.IndexOf('=');
            string bidid = "";
            if (index > 0) bidid = bid.Substring(index + 1);
            #endregion

            _mtmRFQ.RFQ.BuyerRefNo = VRNO;

            #region //Currency
            string _currCode = "";
            var options = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='ddlCurrency']/option");
            if (options.Count > 0)
            {
                //string strCurrency = "";
                for (int i = 0; i < options.Count; i++)
                {
                    var val = options[i].OuterHtml;
                    if (val.Contains("selected"))
                    {
                        _currCode = (options[i].NextSibling).InnerText;
                        if (_currCode == "\r\n\t\t\t") _currCode = "";
                        break;
                    }
                }
            }
            #endregion

            #region // Interchange //
            _mtmRFQ.Interchange.Recipient = SuppLinkCode;
            _mtmRFQ.Interchange.Sender = BuyerLinkCode;
            _mtmRFQ.BuyerSupplierInfo.REF_No = _mtmRFQ.RFQ.BuyerRefNo;
            #endregion

            #region // DocHeader //
            if (_currCode.Trim().Length > 3) _currCode.Trim().Substring(0, 3);
            _mtmRFQ.RFQ.CurrencyCode = _currCode.Trim();
            _mtmRFQ.RFQ.FunctionCode = LeSMTML.MTML.TradingMessageFunctions.Original_9;
            _mtmRFQ.RFQ.MessageNumber = CurrentURL;
            _mtmRFQ.RFQ.MessageReferenceNumber = bidid;
            #endregion

            #region // Document Date //
            DateTime dt = DateTime.MinValue;
            DateTime.TryParseExact(DocDate.Trim(), DATE_FORMAT, null, System.Globalization.DateTimeStyles.None, out dt);
            if (dt == DateTime.MinValue) DateTime.TryParseExact(DocDate.Trim(), DATE_FORMAT.Replace("dd", "d"), null, System.Globalization.DateTimeStyles.None, out dt);
            if (dt != DateTime.MinValue) _mtmRFQ.RFQ.DocumentDate.Value = dt.ToString("yyyyMMdd");
            #endregion
        }

        public void GetEquipmentDetails(ref MTMRFQ _mtmRFQ)
        {
            LeSMTML.MTML.Section _section = new LeSMTML.MTML.Section();
            LeSMTML.MTML.Equipment _equip = new LeSMTML.MTML.Equipment();

            #region // Equipment Name //
            HtmlAgilityPack.HtmlNode _lblEquipName = _httpWrapper.GetElement("span", "id", "lblEquipName");
            if (_lblEquipName != null)
            {
                _equip.Name = convert.ToString(_lblEquipName.InnerText).Trim();
                _section.Name = convert.ToString(_lblEquipName.InnerText).Trim();
            }
            #endregion

            #region // Maker //
            HtmlAgilityPack.HtmlNode _lblMaker = _httpWrapper.GetElement("span", "id", "lblMaker");
            if (_lblMaker != null)
            {
                _equip.Manufacturer = convert.ToString(_lblMaker.InnerText).Trim();
                _section.Manufacturer = convert.ToString(_lblMaker.InnerText).Trim();
            }
            #endregion

            #region // Model //
            HtmlAgilityPack.HtmlNode _lblModel = _httpWrapper.GetElement("span", "id", "lblEquipModel");
            if (_lblMaker != null)
            {
                _equip.Description = "Model / Type : " + convert.ToString(_lblModel.InnerText).Trim();
                _section.Description = "Model / Type : " + convert.ToString(_lblModel.InnerText).Trim();
            }
            #endregion

            #region // Serial //
            HtmlAgilityPack.HtmlNode _lblSerial = _httpWrapper.GetElement("span", "id", "lblSerial");
            if (_lblSerial != null)
            {
                _equip.SerialNumber = convert.ToString(_lblSerial.InnerText).Trim();
                _section.SerialNumber = convert.ToString(_lblSerial.InnerText).Trim();
            }
            #endregion
            _mtmRFQ.RFQ.Equipment = _equip;
        }

        public void GetVesselDetails(ref MTMRFQ _mtmRFQ, out string Vessel, out string Port, out List<string> byrAddr)
        {
            Vessel = ""; Port = ""; byrAddr = new List<string>();
            try
            {
                if (orgAttachFile != null)
                {
                    string pdfLines = _pdfRoutine.GetText(orgAttachFile.FullName, true);
                    using (RichTextBox txt = new RichTextBox())
                    {
                        txt.Text = pdfLines;
                        for (int i = 0; i < txt.Lines.Length; i++)
                        {
                            if (txt.Lines[i].Trim().Replace(" ", "").Contains("Vessel:"))
                            {
                                // Get Vessel & Buyer Name //
                                int sIndx = txt.Lines[i].IndexOf("Vessel");
                                string[] values = txt.Lines[i].Substring(sIndx).Split(':');
                                if (values.Length > 1)
                                {
                                    Vessel = values[1].Trim();
                                }

                                string byrName = txt.Lines[i].Substring(0, sIndx);
                                values = byrName.Trim().Replace("Vessel", "|").Trim().Split('|');
                                if (values.Length > 0)
                                {
                                    byrAddr.Add(values[0].Trim());
                                    byrAddr.Add(txt.Lines[i + 1].Trim());
                                }
                            }
                            else if (txt.Lines[i].Trim().Replace(" ", "").Contains("Telephone:"))
                            {
                                // Get Tel
                                int sIndx = txt.Lines[i].IndexOf("Telephone");
                                string line = txt.Lines[i].Substring(sIndx + 9);
                                line = line.Replace(":", "").Trim().Replace("RFQ No", "|").Trim();
                                byrAddr.Add(line.Split('|')[0].Trim());
                            }
                            else if (txt.Lines[i].Trim().Replace(" ", "").Contains("Fax:"))
                            {
                                // Get Fax
                                int sIndx = txt.Lines[i].IndexOf("Fax");
                                string line = txt.Lines[i].Substring(sIndx + 3).Trim().Trim(':').Trim();
                                byrAddr.Add(line.Split('|')[0].Trim());
                            }
                            else if (txt.Lines[i].Trim().Replace(" ", "").Contains("Email:"))
                            {
                                // Get Email
                                int sIndx = txt.Lines[i].IndexOf("Email");
                                string line = txt.Lines[i].Substring(sIndx + 5);
                                line = line.Replace(":", "").Trim().Replace("DATE", "|").Trim();
                                byrAddr.Add(line.Split('|')[0].Trim());
                            }
                            else if (txt.Lines[i].Trim().Replace(" ", "").Contains("Port:") && txt.Lines[i].Trim().Replace(" ", "").Contains("Vendor:"))
                            {
                                // Get Port
                                int sIndx = txt.Lines[i].IndexOf("Port");
                                string line = txt.Lines[i].Substring(sIndx + 4);
                                line = line.Replace(":", "").Trim().Replace("     ", "|").Trim();
                                Port = line.Split('|')[0].Trim();

                            }
                            else if (txt.Lines[i].Trim().Replace(" ", "").Contains("VesselETA:"))
                            {
                                // Get Vessel ETA
                                int sIndx = txt.Lines[i].IndexOf("Vessel ETA");
                                string line = txt.Lines[i].Substring(sIndx + 10).Trim();
                                line = line.Replace(":", "").Trim().Replace("     ", "|").Trim();
                                string VesselETA = line.Split('|')[0].Trim();

                                DateTime dtETA = DateTime.MinValue;
                                DateTime dt = DateTime.MinValue;
                                DateTime.TryParseExact(VesselETA.Trim(), DATE_FORMAT, null, System.Globalization.DateTimeStyles.None, out dt);
                                if (dtETA == DateTime.MinValue) DateTime.TryParseExact(VesselETA.Trim(), DATE_FORMAT.Replace("dd", "d"), null, System.Globalization.DateTimeStyles.None, out dtETA);
                                if (dtETA != DateTime.MinValue) _mtmRFQ.RFQ.ArrivalDate.Value = dtETA.ToString("yyyyMMdd");

                                break;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogText = "Unable to process PDF file " + ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }

        public void SetPort(string Port)
        {
            if (settings.Length > 3 && isPortCheck == "true")//for rms//08-02-2018
            {
                if (Port != "")
                {
                    if (ConfigurationManager.AppSettings["PORT_SETTINGS"].Trim() != null)
                    {
                        string[] portSettings = ConfigurationManager.AppSettings["PORT_SETTINGS"].Trim().Split('=');
                        if (portSettings.Length > 1)
                        {
                            if (Port.ToUpper().Trim() == portSettings[1].Trim().ToUpper())
                            {
                                SuppLinkCode = portSettings[0].Trim();
                                if (ConfigurationManager.AppSettings[BuyerCode + "-" + SuppLinkCode] != null)
                                {
                                    settings = convert.ToString(ConfigurationManager.AppSettings[BuyerCode + "-" + SuppLinkCode]).Trim().Split('|');
                                    if (settings.Length > 3)//for rms//08-02-2018
                                    {
                                        SupplierCode = settings[0].Trim().ToUpper();
                                        BuyerLinkCode = settings[1].Trim().ToUpper();
                                        SuppLinkCode = settings[2].Trim().ToUpper();
                                        isPortCheck = settings[3].Trim();
                                    }
                                }
                                else throw new Exception("Buyer-Supplier Link Settings not found.");
                            }
                        }
                    }
                    else throw new Exception("Port Settings not found.");
                }
            }
        }

        private void GetAddress_NewSite(ref MTMRFQ _mtmRFQ, string Vessel, string Port, List<string> byrAddress)
        {
            try
            {
                #region /* Buyer details */
                HtmlAgilityPack.HtmlNode _divHeader = _httpWrapper.GetElement("div", "class", "FixedHeader");
                HtmlNode _nTable = _divHeader.SelectSingleNode("//table");
                if (_nTable != null)
                {
                    string[] lines = convert.ToString(_divHeader.SelectSingleNode("//table").InnerText).Trim().Split("\r\n".ToCharArray());
                    _mtmRFQ.RFQ.PartyBY.Name = lines[0].Trim();
                }

                if (byrAddress.Count >= 5)
                {
                    _mtmRFQ.RFQ.PartyBY.Name = byrAddress[0].Trim();
                    _mtmRFQ.RFQ.PartyBY.StreetAddress1 = byrAddress[1].Trim();
                    _mtmRFQ.RFQ.PartyBY.Contact.TEL = byrAddress[2].Trim();
                    _mtmRFQ.RFQ.PartyBY.Contact.FAX = byrAddress[3].Trim();
                    _mtmRFQ.RFQ.PartyBY.Contact.EMAIL = byrAddress[4].Trim();
                }
                #endregion

                #region /* Supplier Details */
                HtmlAgilityPack.HtmlNode _lblVendorName = _httpWrapper.GetElement("span", "id", "lblVendorName");
                if (_lblVendorName != null)
                {
                    _mtmRFQ.RFQ.PartyVN.Name = convert.ToString(_lblVendorName.InnerText).Trim();
                }

                HtmlAgilityPack.HtmlNode _lblVendorContactName = _httpWrapper.GetElement("span", "id", "lblVendorContactName");
                if (_lblVendorContactName != null)
                {
                    _mtmRFQ.RFQ.PartyVN.Contact.Name = convert.ToString(_lblVendorContactName.InnerText).Trim();
                }

                HtmlAgilityPack.HtmlNode _lblVendorPhone = _httpWrapper.GetElement("span", "id", "lblVendorPhone");
                if (_lblVendorPhone != null)
                {
                    _mtmRFQ.RFQ.PartyVN.Contact.TEL = convert.ToString(_lblVendorPhone.InnerText).Trim();
                }

                HtmlAgilityPack.HtmlNode _lblVendorEmail = _httpWrapper.GetElement("span", "id", "lblVendorEmail");
                if (_lblVendorEmail != null)
                {
                    _mtmRFQ.RFQ.PartyVN.Contact.EMAIL = convert.ToString(_lblVendorEmail.InnerText).Trim();
                }
                #endregion

                if (Vessel.Trim().Length > 0)
                {
                    _mtmRFQ.RFQ.PartyUD.Name = Vessel.Trim();
                    if (Port.Trim().Length > 0)
                    {
                        _mtmRFQ.RFQ.PartyUD.PartyLocation = new LeSMTML.MTML.PartyLocation();
                        _mtmRFQ.RFQ.PartyUD.PartyLocation.Qualifier = LeSMTML.MTML.PartyLocationQualifier.TBA;
                        _mtmRFQ.RFQ.PartyUD.PartyLocation.Berth = Port.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private LeSMTML.MTML.LineItemCollection GetItems_NewSite()
        {
            LeSMTML.MTML.LineItemCollection _items = new LeSMTML.MTML.LineItemCollection();
            try
            {
                HtmlAgilityPack.HtmlNode _divContent = _httpWrapper.GetElement("div", "class", "content");
                HtmlNode _ncTable = _divContent.SelectSingleNode(".//table");
                if (_ncTable != null)
                {
                    HtmlNodeCollection _nodes = _ncTable.SelectNodes("//tr[@class='row']");
                    if (_nodes.Count > 0)
                    {
                        int i = 0;
                        foreach (HtmlNode _node in _nodes)
                        {
                            string ctr = "";
                            HtmlNodeCollection _rowData = _node.ChildNodes;

                            #region Set Counter
                            if (_nodes.Count < 1000)
                            {
                                if (i < 10)
                                {
                                    ctr = "ctl0" + i;
                                }
                                else
                                {
                                    ctr = "ctl" + i;
                                }
                            }
                            else
                            {
                                if (i < 10)
                                {
                                    ctr = "ctl00" + i;
                                }
                                else if (i < 100)
                                {
                                    ctr = "ctl0" + i;
                                }
                                else
                                {
                                    ctr = "ctl" + i;
                                }
                            }
                            #endregion

                            string rowid = convert.ToString(_node.Id).Trim().Replace("tr", "");
                            string biditemid = convert.ToString(_httpWrapper.GetElement("rptItems_" + ctr + "_hfItemID").GetAttributeValue("value", "").Trim());
                            LeSMTML.MTML.LineItem item = new LeSMTML.MTML.LineItem();

                            string _number = convert.ToString(_rowData[1].InnerText).Trim('.').Trim();
                            int ItemNo = 0;
                            if (_number.Length > 0) ItemNo = convert.ToInt(_number);

                            string _partname = convert.ToString(_httpWrapper.GetElement("rptItems_" + ctr + "_txtDesc").InnerText.Trim());
                            string _partNo = convert.ToString(_rowData[5].InnerText).Trim();
                            string drawNo = convert.ToString(_rowData[7].InnerText).Trim();
                            string codeNo = convert.ToString(_rowData[9].InnerText).Trim();
                            string unit = convert.ToString(_rowData[13].InnerText).Trim();
                            HtmlNode _eleQty = _httpWrapper.GetElement("rptItems_" + ctr + "_txtBidQty");

                            item.Number = ItemNo.ToString();
                            item.SYS_ITEMNO = (i + 1);
                            item.Description = _partname.Trim();
                            item.MeasureUnitQualifier = unit.Trim();
                            item.Identification = _partNo.Trim();

                            if (_eleQty != null) item.Quantity = convert.ToFloat(_eleQty.GetAttributeValue("value", "").Trim());

                            if (item.LineItemComment == null) item.LineItemComment = new LeSMTML.MTML.Comments();
                            item.LineItemComment.Value = "";
                            if (drawNo.Trim().Length > 0)
                            {
                                item.LineItemComment.Value += "DrawNo : " + drawNo.Trim();
                            }
                            if (codeNo.Trim().Length > 0)
                            {
                                item.LineItemComment.Value += "Code : " + codeNo.Trim();
                            }

                       
                            item.OriginatingSystemRef = convert.ToString(rowid + "_" + biditemid);
                            item.Description = item.Description.Replace("modify/extend this description", "").Replace("&#xD;&#xA;", "").Trim();
                            item.Identification = item.Identification.Replace("your part no.", "").Replace("unitor product No:", "").Replace("UNITOR CODE", "").Replace("IMPA", "").Replace("/", "").Replace("&#xD;&#xA;", "").Trim();
                            //added on 14-1-2018 //maker,model not going in file-query by mtm
                            _section = new LeSMTML.MTML.Section();
                            HtmlNode _tdNode = _httpWrapper.GetElement("rptItems_" + ctr + "_txtDesc").ParentNode;
                            if (_tdNode != null)
                            {
                                if (_tdNode.ChildNodes[5].InnerText.ToUpper().Contains("MAKER : "))
                                {_section.Manufacturer = _tdNode.ChildNodes[5].InnerText.Replace("Maker : ", "");
                                }
                                if (_tdNode.ChildNodes[7].InnerText.ToUpper().Contains("MODAL : "))
                                {
                                    _section.ModelNumber = _tdNode.ChildNodes[7].InnerText.Replace("Modal : ", "");
                                }
                            }
                            item.Section = _section;
                            //
                            item.Prices.Clear();

                            _items.Add(item);
                            i++;
                        }

                    }
                }
                else throw new Exception("Item table not found");
                return _items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DownloadFile(string VRNO, string LinkFile)
        {
            LogText = "RFQ '" + VRNO + "' downloaded successfully.";
            CreateAuditFile(LinkFile, "MTM_SHIP_RFQ", VRNO, "Downloaded", "RFQ '" + VRNO + "' downloaded successfully.", BuyerCode, SupplierCode, AuditPath);
            string BackupFile = Path.GetDirectoryName(lstUrls[RFQ_Counter]) + "\\Backup\\" + Path.GetFileName(lstUrls[RFQ_Counter]);
            if (!Directory.Exists(Path.GetDirectoryName(BackupFile))) Directory.CreateDirectory(Path.GetDirectoryName(BackupFile));
            if (File.Exists(BackupFile)) File.Delete(BackupFile);
            LogText = "File " + Path.GetFileName(lstUrls[RFQ_Counter]) + " moved to backup folder.";

            // Move Attachment file to Backup
            if (orgAttachFile != null && orgAttachFile.Exists)
            {
                if (File.Exists(Path.GetDirectoryName(BackupFile) + "\\" + orgAttachFile.Name))
                    File.Delete(Path.GetDirectoryName(BackupFile) + "\\" + orgAttachFile.Name);
                orgAttachFile.MoveTo(Path.GetDirectoryName(BackupFile) + "\\" + orgAttachFile.Name);
            }

            // Move File
            File.Move(lstUrls[RFQ_Counter], BackupFile);
        }
        #endregion

        private void MoveToNextRFQ(string errLog, string VRNO)
        {
            string cMsgNo = "";
            try
            {
                string LinkFile = Path.GetFileName(lstUrls[RFQ_Counter]);
                string eFile = "MTM_SHIP_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName("") + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                //if (PrintScreen(AttachmentPath + "\\" + eFile))
                //{
                //    if (File.Exists(AttachmentPath + "\\" + eFile))
                //        MoveToError("Unable to process file " + LinkFile + "; Error - " + errLog + "!", VRNO, lstUrls[RFQ_Counter], eFile);
                //    else
                //        MoveToError("Unable to process file " + LinkFile + "; Error - " + errLog + "!", VRNO, lstUrls[RFQ_Counter], "");
                //}
                //else
                //    MoveToError("Unable to process file " + LinkFile + "; Error - " + errLog + "!", VRNO, lstUrls[RFQ_Counter], "");

                if (errLog.Contains("Invalid Link")) { cMsgNo = "1004.2"; } else if (errLog.Contains("Link not")) { cMsgNo = "1004.5"; } else { cMsgNo = "1004"; }

                if (PrintScreen(AttachmentPath + "\\" + eFile))
                {
                    string cFilearg = (File.Exists(AttachmentPath + "\\" + eFile)) ? eFile : string.Empty;
                    MoveToError(cMsgNo + "Unable to process file " + LinkFile + " due to " + errLog, VRNO, lstUrls[RFQ_Counter], cFilearg);
                }
                else
                    MoveToError(cMsgNo + "Unable to process file " + LinkFile + " due to " + errLog, VRNO, lstUrls[RFQ_Counter], "");

                RFQ_Counter--;
                if (RFQ_Counter >= 0)
                {
                    string _URL = GetURL(lstUrls[RFQ_Counter]);
                    CurrentURL = _URL.Trim();
                    try
                    {
                        //if (_httpWrapper.LoadURL(CurrentURL, "input", "id", "btnSubmit", "", true)) //changed by simmy 14022018 : Error Uri string too long when isURLEncode=false
                        URL = CurrentURL;
                        if (isChangedUrl)//21-06-19
                        {
                            if (_httpWrapper.GetElement("input", "id", "btnSubmit") != null)  {  isChangedUrl = false; ProcessRFQ(); }
                        }
                        else if (LoadURL("input", "id", "btnSubmit"))
                        {
                            ProcessRFQ();
                        }
                        else
                        {
                            HtmlAgilityPack.HtmlNode _divMsg = _httpWrapper.GetElement("div", "id", "dvmodal");
                            if (_divMsg != null)
                            {
                                if (_divMsg.InnerText.ToString().Trim().Contains("OOPS"))
                                    MovePdfToError("Invalid Link", VRNO, true);
                            }
                        }
                       // isChangedUrl = false;
                    }
                    catch (Exception ex)
                    {
                        LogText = "Error while loading URL : " + ex.Message;
                        //MovePdfToError("Error while loading URL : " + ex.Message, "", true);
                        MovePdfToError(ex.Message, "", true);
                    }
                }
            }
            catch (Exception EX)
            {
                throw new Exception(EX.ToString());
            }
        }

        private string GetFileType(string emlFile)
        {
            try
            {
                RichTextBox txt = new RichTextBox();
                txt.Text = File.ReadAllText(emlFile).Trim();
                if (txt.Lines.Length > 1)
                {
                    return "MSG";
                }
                else
                {
                    return "TXT";
                }
            }
            catch
            {
                return "";
            }
        }

        public override bool PrintScreen(string sFileName)
        {
            if (!Directory.Exists(Path.GetDirectoryName(sFileName) + "\\Temp")) Directory.CreateDirectory(Path.GetDirectoryName(sFileName) + "\\Temp");
            sFileName = Path.GetDirectoryName(sFileName) + "\\Temp\\" + Path.GetFileName(sFileName);
            if (base.PrintScreen(sFileName))
            {
                MoveFiles(sFileName, AttachmentPath + "\\" + Path.GetFileName(sFileName));
                return (File.Exists(AttachmentPath + "\\" + Path.GetFileName(sFileName)));
            }
            else return false;
        }

        private void MoveToError(string Msg, string VRNO, string FileToMove, string screenShot)
        {
            if (Msg.Trim() != "") //changed by simmy 14022018 avoid audit with blank message.
            {
                LogText = Msg;
                if (screenShot.Trim().Length > 0)
                {
                    if(File.Exists(screenShot))//24-01-2019 added as if png not converted,then take html page file name in audit log.
                    CreateAuditFile(Path.GetFileName(screenShot), "MTM_HTTP_Processor", VRNO, "Error", Msg, BuyerCode, SupplierCode, AuditPath);
                    else
                        CreateAuditFile(Path.GetFileName(screenShot).Replace(".png",".html"), "MTM_HTTP_Processor", VRNO, "Error", Msg, BuyerCode, SupplierCode, AuditPath);
                }
                else
                    CreateAuditFile(Path.GetFileName(FileToMove), "MTM_HTTP_Processor", VRNO, "Error", Msg, BuyerCode, SupplierCode, AuditPath);
            }
            if (File.Exists(FileToMove))
            {
                string orgFile = Path.GetFileName(FileToMove);
                string orgDir = Path.GetDirectoryName(FileToMove);

                if (!Directory.Exists(orgDir + "\\Error_Files")) Directory.CreateDirectory(orgDir + "\\Error_Files");
                if (File.Exists(orgDir + "\\Error_Files\\" + orgFile))
                {
                    File.Delete(orgDir + "\\Error_Files\\" + orgFile);
                }
                File.Move(FileToMove, orgDir + "\\Error_Files\\" + orgFile);
                LogText = "File " + Path.GetFileName(FileToMove) + " moved to error files.";
            }
        }

        private void MoveToBackup(string FileToMove)
        {
            try
            {
                if (File.Exists(FileToMove))
                {
                    string orgFile = Path.GetFileName(FileToMove);
                    string orgDir = Path.GetDirectoryName(FileToMove);

                    if (!Directory.Exists(orgDir + "\\Backup")) Directory.CreateDirectory(orgDir + "\\Backup");
                    if (File.Exists(orgDir + "\\Backup\\" + orgFile))
                    {
                        File.Delete(orgDir + "\\Backup\\" + orgFile);
                    }
                    File.Move(FileToMove, orgDir + "\\Backup\\" + orgFile);

                    LogText = "File " + Path.GetFileName(FileToMove) + " moved to backup files.";
                }
            }
            catch { }
        }

        private void MoveMTMLToError(string FileToMove)
        {
            try
            {
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
            catch { }
        }

        private void MovePdfToError(string msg, string VRNO, bool isCheckImportDir)
        {
            if (isCheckImportDir)
            {
                string importDir = Path.GetDirectoryName(lstUrls[RFQ_Counter]);
                DirectoryInfo _importDir = new DirectoryInfo(importDir);
                FileInfo[] _pdfFiles = _importDir.GetFiles("*" + Path.GetFileNameWithoutExtension(lstUrls[RFQ_Counter]) + "*.pdf");
                if (_pdfFiles.Length > 0)
                {
                    orgAttachFile = _pdfFiles[0];
                }
            }

            if (orgAttachFile != null)
            {
                if (File.Exists(Path.GetDirectoryName(orgAttachFile.FullName) + "\\Error_Files\\" + Path.GetFileName(orgAttachFile.FullName)))
                    File.Delete(Path.GetDirectoryName(orgAttachFile.FullName) + "\\Error_Files\\" + Path.GetFileName(orgAttachFile.FullName));

                File.Move(orgAttachFile.FullName, Path.GetDirectoryName(orgAttachFile.FullName) + "\\Error_Files\\" + Path.GetFileName(orgAttachFile.FullName));
            }
            orgAttachFile = null;
            MoveToNextRFQ(msg, VRNO);
        }

        #region Quote
        public void UploadQuote()
        {
            List<string> lstQuotes = new List<string>(); string strAuditmsg = "";
            try
            {
                #region get all files from quote upload path
                foreach (string quotePath in QuotePath)
                {
                    if (quotePath.Trim().Length > 0)
                    {
                        string[] quotes = Directory.GetFiles(quotePath.Trim(), "*.xml");
                        lstQuotes.AddRange(quotes);
                    }
                }
                #endregion

                if (lstQuotes.Count > 0)
                {
                    foreach (string quotefile in lstQuotes)
                    {
                        MTMLClass _mtml = new MTMLClass();
                        _interchange = _mtml.Load(quotefile);
                        if (_interchange != null)
                        {
                            string eFile = "";
                            LoadInterchangeDetails();
                            string errText = SetQuoteBuyerSuppCode();
                            if (isQuote)
                            {
                                URL = MessageNumber.Trim().Replace("&amp;", "&");//changed by Kalpita on 03/07/2019
                                if (Convert.ToDouble(GrandTotal) > 0)//check for declined quote
                                {
                                    if (URL != "")
                                    {
                                        if (LoadURL("input", "id", "btnSubmit"))
                                        {
                                            HtmlNode _dvModal = _httpWrapper.GetElement("div", "id", "dvmodal");
                                            if (_dvModal == null)//check if bid is close or not
                                            {
                                                HtmlNode siteRefNo = _httpWrapper.GetElement("span", "id", "lblRFQNO");
                                                if (siteRefNo != null)
                                                {
                                                    if (siteRefNo.InnerText.Trim().Replace(" ", "") == UCRefNo)
                                                    {
                                                        LogText = "Processing quote started for ref no: " + UCRefNo;
                                                        if (isSaveAsDraft)
                                                        {
                                                            #region save quote
                                                            if (SaveQuotation(ref eFile, quotefile))
                                                            {
                                                                LogText = "Quote for RefNo " + UCRefNo + " saved successfully.";
                                                                CreateAuditFile(Path.GetFileName(eFile), "MTM_HTTP_Quote", UCRefNo, "Submitted", "Quote for RefNo '" + UCRefNo + "' saved successfully.", BuyerCode, SupplierCode, AuditPath);
                                                                MoveToBackup(quotefile);
                                                            }
                                                            else
                                                            {
                                                                eFile = "MTM_SHIP_Error" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName("") + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                                                                PrintScreen(AttachmentPath + "\\" + eFile);
                                                                if (_errorLog != "")
                                                                    MoveToError(_errorLog, UCRefNo, quotefile, eFile);
                                                                else
                                                                    //MoveToError("Unable to save quote for RefNo " + UCRefNo, UCRefNo, quotefile, eFile);
                                                                    MoveToError("LeS:1008-Unable to save quote for RefNo " + UCRefNo, UCRefNo, quotefile, eFile);
                                                            }
                                                            #endregion
                                                        }
                                                        else if (isSubmitQuote)
                                                        {
                                                            #region submit quote
                                                            if (SaveQuotation(ref eFile, quotefile))
                                                            {
                                                                if (!IsDecline)
                                                                {
                                                                    LogText = "Quote for RefNo '" + UCRefNo + "' saved successfully.";
                                                                    if (SubmitQuote(quotefile, ref eFile))
                                                                    {
                                                                        LogText = strAuditmsg = "Quote for RefNo '" + UCRefNo + "' submitted successfully.";
                                                                        CreateAuditFile(Path.GetFileName(eFile), "MTM_HTTP_Quote", UCRefNo, "Submitted", strAuditmsg, BuyerCode, SupplierCode, AuditPath);
                                                                        MoveToBackup(quotefile);
                                                                        if (isDoneStatus)
                                                                            Write_DoneStatus(UCRefNo, AAGRefNo, "Success");
                                                                        //added on 29-1-2019
                                                                        if (convert.ToString(ConfigurationManager.AppSettings["SEND_MAIL_FOR_SUBMIT"]).Trim().ToUpper() == "TRUE")
                                                                        {
                                                                            LogText = "Generating mail notification...";
                                                                            SendMailNotification(_interchange, "QUOTE", UCRefNo.Trim(), "SUBMITTED", strAuditmsg);
                                                                        }//
                                                                    }
                                                                    else
                                                                    {
                                                                        eFile = "MTM_SHIP_Error" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName("") + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                                                                        PrintScreen(AttachmentPath + "\\" + eFile);
                                                                        //MoveToError("Unable to submit quote for RefNo " + UCRefNo, UCRefNo, quotefile, eFile);
                                                                        MoveToError("LeS-1011:Unable to submit quote", UCRefNo, quotefile, eFile);
                                                                        if (isDoneStatus)
                                                                            Write_DoneStatus(UCRefNo, AAGRefNo, "Fail");
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    LogText = "Quote for RefNo '" + UCRefNo + "' declined successfully.";
                                                                    CreateAuditFile(Path.GetFileName(quotefile), "MTM_HTTP_Quote", UCRefNo, "Declined", "Quote for RefNo '" + UCRefNo + "' declined successfully.",
                                                                        BuyerCode, SupplierCode, AuditPath);
                                                                    MoveMTMLToError(quotefile);
                                                                    if (isDoneStatus)
                                                                        Write_DoneStatus(UCRefNo, AAGRefNo, "Fail");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                eFile = "MTM_SHIP_Error" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName("") + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                                                                PrintScreen(AttachmentPath + "\\" + eFile);
                                                                if (_errorLog != "")
                                                                    MoveToError(_errorLog, UCRefNo, quotefile, eFile);
                                                                else
                                                                    //MoveToError("Unable to save quote for RefNo " + UCRefNo, UCRefNo, quotefile, eFile);
                                                                    MoveToError("LeS:1008-Unable to save quote", UCRefNo, quotefile, eFile);
                                                            }
                                                            #endregion
                                                        }
                                                    }
                                                    else
                                                    {
                                                        eFile = "MTM_SHIP_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                                                        PrintScreen(AttachmentPath + "\\" + eFile);
                                                        // MoveToError("Ref No. mismatch betwwen site ref no " + siteRefNo.InnerText.Trim() + " and quote file ref no " + UCRefNo + ".", UCRefNo, quotefile, eFile);
                                                        MoveToError("LeS-1004.3:Unable to process file due to Ref No. Mismatch with Site Ref No." + UCRefNo + ".", UCRefNo, quotefile, eFile);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (_dvModal.InnerText.Trim().Contains("OOPS"))
                                                {
                                                    eFile = "MTM_SHIP_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                                                    PrintScreen(AttachmentPath + "\\" + eFile);
                                                    MoveToError("LeS:1003-Issue after loading URL since Requested Bid [" + UCRefNo + "] is not available for quotation.", UCRefNo, quotefile, AttachmentPath + "\\" + eFile);
                                                    if (isDoneStatus)
                                                        Write_DoneStatus(UCRefNo, AAGRefNo, "Fail");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            HtmlNode _dvModal = _httpWrapper.GetElement("div", "id", "dvmodal");
                                            if (_dvModal.InnerText.Trim().Contains("OOPS"))
                                            {
                                                eFile = "MTM_SHIP_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                                                HtmlAgilityPack.HtmlDocument _doc = new HtmlAgilityPack.HtmlDocument();
                                                _doc.LoadHtml(_CurrentResponseString);
                                                foreach (HtmlNode a in _doc.DocumentNode.ChildNodes["html"].ChildNodes["body"].ChildNodes)
                                                {
                                                    if (a.Name == "div")
                                                    {
                                                        if (a.Attributes["style"] != null)
                                                        {
                                                            a.Attributes["style"].Remove();
                                                        }
                                                    }
                                                }
                                                _CurrentResponseString = _doc.DocumentNode.InnerHtml;
                                                PrintScreen(AttachmentPath + "\\" + eFile);
                                                MoveToError("LeS:1003-Issue after loading URL since Requested Bid [" + UCRefNo + "] is not available for quotation.", UCRefNo, quotefile, AttachmentPath + "\\" + eFile);
                                                if (isDoneStatus)
                                                    Write_DoneStatus(UCRefNo, AAGRefNo, "Fail");
                                            }
                                        }
                                    }
                                    else
                                    //MoveToError("Unable to upload quote '" + UCRefNo + "'. Link URL is missing in MESSAGENUMBER.", UCRefNo, quotefile, "");
                                    { MoveToError("LeS-1001:Unable to find URL in file", UCRefNo, quotefile, ""); }
                                }
                                else
                                {
                                    //  MoveToError("Unable to upload quote '" + UCRefNo + "'. Quote total is '0'.", UCRefNo, quotefile, "");
                                    MoveToError("LeS-1008.5:Unable to save quote since quote total is zero", UCRefNo, quotefile, eFile);
                                    if (isDoneStatus)
                                        Write_DoneStatus(UCRefNo, AAGRefNo, "Fail");
                                }

                            }
                            else
                            {
                                //if (errText.Trim() == "")
                                //    MoveToError("Upload Quote flag is set as false for Buyer-Supplier (" + BuyerCode + "-" + SupplierCode + ") in config file.", UCRefNo, quotefile, "");
                                //else
                                //    MoveToError("Upload Quote flag is set as false for Buyer-Supplier (" + BuyerCode + "-" + SupplierCode + ") in config file.", UCRefNo, quotefile, "");
                                MoveToError("LeS-1012:Parameters not configured, Upload Quote flag is set as false for Buyer-Supplier (" + BuyerCode + "-" + SupplierCode + ").", UCRefNo, quotefile, "");
                                //	

                            }
                        }
                        else
                        {
                            //MoveToError("Unable to read quote file '" + Path.GetFileName(quotefile) + "'.", UCRefNo.Trim(), quotefile, "");
                            MoveToError("LeS-1004:Unable to process file " + Path.GetFileName(quotefile) + ".", UCRefNo.Trim(), quotefile, "");
                        }
                    }
                }
                else LogText = "No quotes found.";
            }
            catch (Exception ex)
            {
                LogText = "Error at UploadQuote() : " + ex.GetBaseException().Message.ToString();
                string eFile = "MTM_SHIP_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                PrintScreen(AttachmentPath + "\\" + eFile);
                CreateAuditFile(eFile, "MTM_HTTP_Quote", "", "Error", "LeS-1008:Unable to save quote due to " + ex.GetBaseException().Message.ToString(), BuyerCode, SupplierCode, AuditPath);
            }
            _errorLog = "";
        }

        private bool SaveQuotation(ref string eFile, string quoteFile)
        {
            bool result = false;
            try
            {
                if (SetCurrency())
                {
                    string strURL = URL;

                    HtmlNode _extrCharge= _httpWrapper.GetElement("span", "id", "txtLCRow1");//added by Kalpita on 22/08/2019
                    if (_extrCharge != null && !string.IsNullOrEmpty(_extrCharge.InnerText.Trim()))
                    {
                        _extrCharge.SetAttributeValue("value",string.Empty);
                    }

                    if (SetLineItemDetails(strURL))
                    {
                        if (SetOtherHeaderDetails(ref eFile, quoteFile))
                        {
                            result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogText = "Error at SaveQuoatation() : " + ex.GetBaseException().Message.ToString();
                result = false;
            }
            return result;
        }

        public bool SetCurrency()
        {
            bool currSuccess = false;
            try
            {

                //_httpWrapper.IsUrlEncoded = false;//commented as uri too big error occurs on 4-7-2018
                _httpWrapper.IsUrlEncoded = true;//added as uri too big error occurs on 4-7-2018
                if (Currency != "")
                {
                    dctPostDataValues.Clear();
                    dctPostDataValues.Add("__EVENTTARGET", "ddlCurrency");
                    dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
                    dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", _httpWrapper._dctStateData["ToolkitScriptManager1_HiddenField"]);
                    dctPostDataValues.Add("__LASTFOCUS", _httpWrapper._dctStateData["__LASTFOCUS"]);
                    dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                    dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                    dctPostDataValues.Add("__EVENTVALIDATION", _httpWrapper._dctStateData["__EVENTVALIDATION"]);
                    dctPostDataValues.Add("ddlCurrency", Currency);
                    if (PostURL("select", "name", "ddlCurrency"))
                    {
                        HtmlNode _currency = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ddlCurrency']/option[@selected='selected']");
                        if (_currency != null)
                        {
                            if (Currency == _currency.GetAttributeValue("value", "").Trim())
                            {
                                currSuccess = true;
                                LogText = "Currency saved.";
                            }
                        }
                    }
                    if (!currSuccess) { _errorLog = "Unable to set currency " + Currency; throw new Exception(_errorLog); }

                }
                else { currSuccess = false; _errorLog = "Unable to set Currency; Currency missing in quote file"; throw new Exception(_errorLog); }
                return currSuccess;
            }
            catch (Exception ex)
            {
                _errorLog= LogText = "Error while setting currency: " + ex.Message.ToString();
                return false;
            }
        }

        public bool SetLineItemDetails(string strURL)
        {
            bool result = false;
            try
            {
                Dictionary<string, string> dicItem = new Dictionary<string, string>();
                _httpWrapper.AcceptMimeType = "application/json, text/javascript, */*; q=0.01";
                _httpWrapper.ContentType = "application/json; charset=utf-8";
                if (!_httpWrapper._AddRequestHeaders.ContainsKey("X-Requested-With"))
                    _httpWrapper._AddRequestHeaders.Add("X-Requested-With", @"XMLHttpRequest");
                if (!_httpWrapper._SetRequestHeaders.ContainsKey(HttpRequestHeader.Pragma))
                    _httpWrapper._SetRequestHeaders.Add(HttpRequestHeader.Pragma, "no-cache");
                _httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.CacheControl);
                _httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");

                URL = "http://portal.mtmsm.com/public/pos/QuoteManager.aspx/Update_unitpriceAndquantity";

                //check if site contains all items of mtml.
                HtmlAgilityPack.HtmlNode _divContent = _httpWrapper.GetElement("div", "class", "content");
                HtmlNode _ncTable = _divContent.SelectSingleNode(".//table");
                if (_ncTable != null)
                {
                    HtmlNodeCollection _nodes = _ncTable.SelectNodes("//tr[@class='row']");
                    if (_nodes.Count > 0)
                    {
                        foreach (MTML.GENERATOR.LineItem _litem in _lineitem)
                        {
                            if (_litem.IsExtraItem == 0)
                            {
                                string bidItemID = "", rowId = "";
                                string[] ids = convert.ToString(_litem.OriginatingSystemRef).Split('_');
                                if (ids.Length > 1)
                                {
                                    rowId = ids[0].Trim();
                                    bidItemID = ids[1];
                                }
                                HtmlNode _tr = _httpWrapper.GetElement("tr", "id", "tr" + rowId);
                                if (_tr != null)
                                {
                                    string no = "", _siteBidItemId = "", _sitebidid = "", currentRate = "", Quantity = "";
                                    string _siteRowId = _tr.GetAttributeValue("id", "").Trim().Replace("tr", "");
                                    if (_tr.ChildNodes[11].ChildNodes[3].GetAttributeValue("id", "").Contains("hfItemID"))
                                    {
                                        no = _tr.ChildNodes[11].ChildNodes[3].GetAttributeValue("id", "").Replace("_hfItemID", "").Replace("rptItems_ctl", "");
                                        _siteBidItemId = _tr.ChildNodes[11].ChildNodes[1].GetAttributeValue("biditemid", "").Trim();
                                        _sitebidid = _tr.ChildNodes[11].ChildNodes[1].GetAttributeValue("bidid", "").Trim();
                                        currentRate = _httpWrapper.GetElement("span", "id", "lblCurrRate").InnerText.Trim();
                                    }
                                    if (_litem.OriginatingSystemRef == _siteRowId + "_" + _siteBidItemId)
                                    {
                                        string _price = "";
                                        foreach (MTML.GENERATOR.PriceDetails _priceDetails in _litem.PriceList)
                                        {
                                            if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.GRP) { _price = _priceDetails.Value.ToString("0.00"); break; }
                                        }

                                        string comments = _litem.LineItemComment.Value.ToString().Trim();
                                        if (convert.ToInt(_litem.DeleiveryTime) != 0)
                                        {
                                            if (!convert.ToString(ConfigurationManager.AppSettings["NOT_SET_DEL_DAYS"]).Contains(this.SupplierCode.ToUpper()))
                                            {
                                                if (comments.Length > 0)
                                                {
                                                    comments += "; Del Days : " + _litem.DeleiveryTime + "; ";
                                                }
                                                else
                                                {
                                                    comments += "Del Days : " + _litem.DeleiveryTime + "; ";
                                                }
                                            }
                                        }
                                        if (comments.Length > 0) comments += "; Quoted Qty: " + _litem.Quantity;
                                        else comments += "Quoted Qty: " + _litem.Quantity;

                                        if (comments.Length > 0) comments += "; Quoted Unit: " + _litem.MeasureUnitQualifier;
                                        else comments += "Quoted Unit: " + _litem.MeasureUnitQualifier;

                                        Quantity = convert.ToString(_litem.Quantity);
                                        dicItem.Add("tr" + rowId, _siteBidItemId + "~" + Quantity + "~" + _price + "~" + _sitebidid + "~" + rowId + "~" + currentRate + "~" + comments);
                                    }
                                    else
                                        throw new Exception("Item not found on site for OriginatingSystemRef " + _litem.OriginatingSystemRef + ".");
                                }
                                else throw new Exception("Item not found on site for rowid " + rowId + ".");
                            }
                            else
                            {
                                if (convert.ToString(_litem.ItemType).Trim() == "")
                                    extraItemsAmt += _litem.MonetaryAmount;
                            }
                        }

                        //create body to post
                        string _postData = "";
                        if (dicItem.Count > 0)
                        {
                            foreach (KeyValuePair<string, string> item in dicItem)
                            {
                                List<string> lstData = item.Value.Split('~').ToList();
                                if (lstData.Count == 7)
                                {
                                    _postData = "";
                                    _postData += "{";
                                    string _name = lstData[6].Replace("'", "`");
                                    _postData += "\'BidItemID\': \'" + lstData[0] + "\',\'BidQty\': \'" + lstData[1] + "\',\'UnitPrice\': \'" + lstData[2] + "\'," +
                                              "\'BidId\': \'" + lstData[3] + "\',\'RecID\': \'" + lstData[4] + "\',\'curentrate\': \'" + lstData[5] +
                                              "\',\'vendordescription\': \'" + _name + "\'";
                                    _postData += "}";

                                    if (!_httpWrapper.PostURL(URL, _postData, "", "", ""))
                                        throw new Exception("Unable to fill item details");
                                }
                            }
                        }

                        //item details saving on site
                        URL = strURL;
                        _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
                        _httpWrapper.ContentType = "application/x-www-form-urlencoded";
                        _httpWrapper._AddRequestHeaders.Remove("X-Requested-With");
                        if (LoadURL("input", "id", "btnSubmit"))
                        {
                            HtmlNode _hTotal = _httpWrapper.GetElement("span", "id", "lblLCRow2");
                            if (_hTotal != null)
                            {
                                if (convert.ToFloat(_hTotal.InnerText.Trim()) != convert.ToFloat(0))
                                { LogText = "Item Details saved."; result = true; }
                                else
                                {
                                    if (convert.ToFloat(OtherCost) != 0 || convert.ToFloat(DepositCost) != 0 || convert.ToFloat(TaxCost) != 0|| convert.ToFloat(TotalLineItemsAmount)!=0)
                                    { LogText = "Item Details saved."; result = true; }
                                    else { result = false; throw new Exception("Unable to save item details"); }
                                }
                                //double Total = convert.ToFloat(GrandTotal) - convert.ToFloat(OtherCost) - convert.ToFloat(DepositCost) - extraItemsAmt;
                                //if (convert.ToInt(Total) == convert.ToInt(convert.ToFloat(_hTotal.InnerText.Trim())))
                                //{ LogText = "Item Details saved."; result = true; }
                                //else { result = false; throw new Exception("Unable to save item details"); }
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                LogText = "Error while setting LineItem Details: " + ex.Message.ToString();
                return false;
                // throw;
            }
        }

        public bool SetOtherHeaderDetails(ref string eFile, string quoteFile)
        {
            bool result = false;
            try
            {
                _httpWrapper.IsUrlEncoded = true;//added for uri string is too long//25-6-18
                dctPostDataValues.Clear();
                dctPostDataValues.Add("__EVENTTARGET", _httpWrapper._dctStateData["__EVENTTARGET"]);
                dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
                dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", _httpWrapper._dctStateData["ToolkitScriptManager1_HiddenField"]);
                dctPostDataValues.Add("__LASTFOCUS", _httpWrapper._dctStateData["__LASTFOCUS"]);
                dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                dctPostDataValues.Add("__EVENTVALIDATION", _httpWrapper._dctStateData["__EVENTVALIDATION"]);

                _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
                _httpWrapper.ContentType = "application/x-www-form-urlencoded";
                _httpWrapper._AddRequestHeaders.Remove("X-Requested-With");

                //additional cost
                if (FreightCharge != "" || TaxCost != "")
                {
                    if (FreightCharge != "")
                        additionalCost_freight += Convert.ToDouble(FreightCharge);
                    if (TaxCost != "")
                        additionalCost_freight += Convert.ToDouble(TaxCost);
                }
                dctPostDataValues.Add("txtLCRow1", convert.ToString(Math.Round(additionalCost_freight, 2)));//

                //dates
                if (DtDelvDate == "")
                {
                    DateTime delDate = DateTime.MinValue;
                    if (convert.ToInt(LeadDays) > 0)
                    {
                        delDate = DateTime.Now.AddDays(convert.ToInt(LeadDays));
                        DtDelvDate = delDate.ToString("dd-MMM-yyyy");
                    }
                        if (delDate == DateTime.MinValue)
                        {
                            string DefaultDelDays = convert.ToString(ConfigurationManager.AppSettings[this.SupplierCode + "_DEL_DAYS"]);
                            if (DefaultDelDays.Trim() != "" && convert.ToInt(DefaultDelDays) > 0)
                            {
                                delDate = DateTime.Now.AddDays(convert.ToInt(DefaultDelDays));
                                DtDelvDate = delDate.ToString("dd-MMM-yyyy");
                            }
                            else
                            {
                                MoveToError("", "", quoteFile, "");
                                _errorLog = "Unable to set delivery date; Delivery Date missing in quote file";
                                throw new Exception(_errorLog);
                            }
                        }


                        if (DtDelvDate != "")
                        {
                            dctPostDataValues.Add("txtDeliveryDate", DtDelvDate);
                        }
                        else { _errorLog = "Unable to read delivery date for RefNo " + UCRefNo + "."; throw new Exception(_errorLog); }
                }
                else
                {
                    dctPostDataValues.Add("txtDeliveryDate", DtDelvDate);
                }

                if (dtExpDate != "")
                {
                    dctPostDataValues.Add("txtExpires", dtExpDate);
                }//

                //Delivery Port
                if (PortName != null)
                {
                    PortName = (PortName.Length > 15) ? PortName.Substring(0, 15) : PortName;
                }
                else if (PortCode != "") PortName = PortCode;
                if(PortName!=null)dctPostDataValues.Add("txtDeliveryPort", PortName);
                else
                {
                    _errorLog = "Unable to set Port; Port is missing in quote file"; throw new Exception(_errorLog);//dctPostDataValues.Add("txtDeliveryPort", "");
                }
                //

                //Vendor Ref
                if (AAGRefNo != "")
                    dctPostDataValues.Add("txtVenRef", AAGRefNo);
                else { _errorLog = "Unable to set Quote Ref No; Quote Ref No missing in quote file"; throw new Exception(_errorLog); }
                //

                //Vendor Comments
                string comm = SupplierComment.Trim().Replace("'", "");
                comm = comm.Replace("\r\n", " ");
                comm = comm.Replace("\n", " ");
                comm = comm.Replace("<", "(").Replace(">", ")");
                do { comm = comm.Replace("  ", " "); } while (comm.Contains("  "));
                do { comm = comm.Replace("====", "=="); } while (comm.Contains("===="));
                do { comm = comm.Replace("****", "***"); } while (comm.Contains("****"));
               // SupplierComment = (comm.Length > 1500) ? comm.Substring(0, 1500) : comm;
            
                //added by kalpita on 07/08/2019 (Item Added section is truncated while adding Supplier Comments)
                string cModComm = "", cFinComm = "";
                int extItemIndx = comm.IndexOf("Item Added");
                if (extItemIndx > 0)
                {
                    cModComm = comm.Substring(0, extItemIndx);
                    string cRemComm = comm.Substring(extItemIndx, comm.Length - extItemIndx);
                    cFinComm = cRemComm + string.Empty + cModComm;
                }
                else
                {
                    cFinComm = comm;
                }
                SupplierComment = (cFinComm.Length > 1500) ? cFinComm.Substring(0, 1500) : cFinComm;
                dctPostDataValues.Add("txtVendorComments", SupplierComment);
                dctPostDataValues.Add("btnSubmit", "Preview&Submit");
                //

                if (PostURL("input", "id", "btnFinal"))// if (PostURL("input", "id", "btnSubmit"))               
                {
                    LogText = "Header details saved.";
                    HtmlNode _hTotal = _httpWrapper.GetElement("span", "id", "lblTotLC_P");//    HtmlNode _hTotal = _httpWrapper.GetElement("span", "id", "lblLCRow2");                
                    if (_hTotal != null)
                    {
                        double Total = convert.ToFloat(GrandTotal) - convert.ToFloat(OtherCost) - convert.ToFloat(DepositCost) - extraItemsAmt;
                        if (convert.ToInt(Total) == convert.ToInt(convert.ToFloat(_hTotal.InnerText.Trim())))
                        { result = true; }
                        else if (convert.ToInt(convert.ToFloat(BuyerTotal) + convert.ToFloat(OtherCost) + convert.ToFloat(DepositCost) + extraItemsAmt) == convert.ToInt(convert.ToFloat(_hTotal.InnerText.Trim())))
                        {result=true;}
                        else if (Total > convert.ToFloat(_hTotal.InnerText.Trim()))
                        {
                            if (Total - convert.ToFloat(_hTotal.InnerText.Trim()) <= 5) result = true;
                            else { result = false; _errorLog = "Mismatch total of mtml " + Total + " with site total " + _hTotal.InnerText.Trim(); throw new Exception(_errorLog); }
                        }
                        else if (Total < convert.ToFloat(_hTotal.InnerText.Trim()))
                        {
                            if ((convert.ToFloat(_hTotal.InnerText.Trim()) - Total) <= 5) result = true;
                            else { result = false; _errorLog = "Mismatch total of mtml " + Total + " with site total " + _hTotal.InnerText.Trim(); throw new Exception(_errorLog); }
                        }
                        else { result = false; _errorLog = "Mismatch total of mtml " + Total + " with site total " + _hTotal.InnerText.Trim(); throw new Exception(_errorLog); }
                    }

                    eFile = "MTM_SHIP_Save_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                    if (!PrintScreen(AttachmentPath + "\\" + eFile))
                    {
                        SavePage_NewWebPage(AttachmentPath + "\\Temp\\" + Path.ChangeExtension(eFile, "html"), AttachmentPath + "\\" + eFile);
                    }
                    return result;
                }
                else
                {
                    if (IsDecline)
                    {
                        eFile = "MTM_SHIP_Decline_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                        PrintScreen(AttachmentPath + "\\" + eFile);
                        return true;
                    }
                    else throw new Exception("Unable to save & preview quotation.");
                }
            }
            catch (Exception ex)
            {
                LogText = "Error while setting other header details: " + ex.Message.ToString();
                return false;
            }
        }

        private bool SubmitQuote(string quotefile, ref string eFile)
        {
            bool result = false;
            Double _siteTotal = 0, GrantTotal = 0;
            try
            {
                HtmlNode _hsiteTotal = _httpWrapper.GetElement("span", "id", "lblTotLC_P");
                if (_hsiteTotal != null)
                {
                    _siteTotal = convert.ToFloat(_hsiteTotal.InnerText.Trim());
                }
                if (_siteTotal > 0 && convert.ToFloat(GrandTotal) > 0)
                {
                    GrantTotal = convert.ToFloat(GrandTotal) - convert.ToFloat(OtherCost) - convert.ToFloat(DepositCost) - extraItemsAmt;

                    bool submitquote = false;

                    if (_siteTotal == GrantTotal) submitquote = true;
                    else if (convert.ToFloat(BuyerTotal) + convert.ToFloat(OtherCost) + convert.ToFloat(DepositCost) + extraItemsAmt == convert.ToFloat(_siteTotal))
                    { submitquote = true; }
                    else if (_siteTotal > GrantTotal)
                    {
                        if ((_siteTotal - GrantTotal) <= 5) submitquote = true;
                    }
                    else
                    {
                        if ((GrantTotal - _siteTotal) <= 5) submitquote = true;
                    }

                    if (submitquote)
                    {
                        dctPostDataValues.Clear();
                        dctPostDataValues.Add("__EVENTTARGET", _httpWrapper._dctStateData["__EVENTTARGET"]);
                        dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
                        dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", _httpWrapper._dctStateData["ToolkitScriptManager1_HiddenField"]);
                        dctPostDataValues.Add("__LASTFOCUS", _httpWrapper._dctStateData["__LASTFOCUS"]);
                        dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                        dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                        dctPostDataValues.Add("__EVENTVALIDATION", _httpWrapper._dctStateData["__EVENTVALIDATION"]);
                        dctPostDataValues.Add("btnFinal", "Yes - Submit My Quote");
                        if (PostURL("input", "id", "btnPrint"))
                        {
                            eFile = "MTM_SHIP_AfterSubmit_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                            if (!PrintScreen(AttachmentPath + "\\" + eFile))
                            {
                                SavePage_NewWebPage(AttachmentPath + "\\Temp\\" + Path.ChangeExtension(eFile, "html"), AttachmentPath + "\\" + eFile);
                            }
                            if (LoadURL("input", "id", "btnSubmit"))
                            {
                                result = true;
                            }
                        }
                    }
                    else
                    {
                        eFile = "MTM_SHIP_Error_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                        PrintScreen(AttachmentPath + "\\" + eFile);
                        //MoveToError("Quote Total Mismatched !", UCRefNo, quotefile, eFile);
                        MoveToError("LeS-1008.1:Unable to save quote due to amount mismatch", UCRefNo, quotefile, eFile);
                        result = false;
                    }
                }
                else
                {
                    eFile = "MTM_SHIP_Error_" + SupplierCode + "_" + BuyerCode + "_" + convert.ToFileName(UCRefNo) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                    PrintScreen(AttachmentPath + "\\" + eFile);
                 //   MoveToError("Quote Total is zero !", UCRefNo, quotefile, eFile);
                    MoveToError("LeS-1008.5:Unable to save quote since quote total is zero", UCRefNo, quotefile, eFile);
                    result = false;
                }
            }
            catch (Exception ex)
            {
                LogText = "Error at SubmitQuote() : " + ex.GetBaseException().Message.ToString();
                result = false;
            }
            return result;
        }

        public void Write_DoneStatus(string RFQNo, string AAGRefNo, string Status)
        {
            string FileName = DoneStatusPath + "\\DoneStatus_MTM_" + SupplierCode + "_" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".log";
            if (!File.Exists(FileName))
            {
                try
                {
                    File.WriteAllText(FileName, RFQNo + "|" + AAGRefNo + "=" + Status);
                }
                catch (Exception ex)
                { LogText = "Exception on Write_DoneStatus(): " + ex.Message; }
            }
        }

        private string SetQuoteBuyerSuppCode()
        {
            try
            {
                string senderCode = SCode;
                if (ConfigurationManager.AppSettings[BuyerCode + "-" + senderCode] != null)
                {
                    string[] settings = convert.ToString(ConfigurationManager.AppSettings[BuyerCode + "-" + senderCode]).Trim().Split('|');
                    if (settings.Length == 8)
                    {
                        SupplierCode = settings[0].Trim().ToUpper();
                        BuyerLinkCode = settings[1].Trim().ToUpper();
                        SuppLinkCode = settings[2].Trim().ToUpper();
                        if (settings[4].Trim().ToUpper() == "TRUE") isQuote = true;
                        if (settings[5].Trim().ToUpper() == "TRUE") isSaveAsDraft = true;
                        if (settings[6].Trim().ToUpper() == "TRUE") isSubmitQuote = true;
                        if (settings[7].Trim().ToUpper() == "TRUE") isDoneStatus = true;
                    }
                    return "";
                }
                else
                {
                    return "Buyer-Supplier Link Settings not found.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
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
                        BCode = _interchange.Recipient;

                    if (_interchange.Sender != null)
                        SCode = _interchange.Sender;

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
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.TaxCost_99)
                                TaxCost = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.OtherCost_98)
                                OtherCost = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.Deposit_97)
                                DepositCost = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
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
                                        DtDelvDate = dtDelDate.ToString("dd-MMM-yyyy");//ToString("MM/dd/yyyy");
                                    }
                                    if (dtDelDate == null)
                                    {
                                        DateTime dt = FormatMTMLDate(DateTime.Now.AddDays(Convert.ToDouble(LeadDays)).ToString());
                                        if (dt != DateTime.MinValue)
                                        {
                                            DtDelvDate = dt.ToString("dd-MMM-yyyy");//.ToString("MM/dd/yyyy");
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
                                        dtExpDate = ExpDate.ToString("dd-MMM-yyyy");//ExpDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);//15-3-18
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
              //  else throw new Exception("MAIL_AUDIT_PATH value is not defined in config file.");

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
                    foreach (MTML.GENERATOR.Party _partyObj in _interchange.DocumentHeader.PartyAddresses)
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
        #endregion

        private string SavePage_NewWebPage(string htmlFile, string Attachment)
        {
            try
            {

                HtmlRenderer.Render(htmlFile, Attachment, new Rectangle(0, 0, 0, 0));
                if (File.Exists(Attachment)) return Path.GetFileName(Attachment);
                else return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }

    public class MTMRFQ : LeSMTML.RequestForQuote
    {
        public MTMRFQ()
        {
            this.RFQ.VersionNumber = "1";
            this.Interchange.VersionNumber = "1";
            this.Interchange.Identifier = "UNOC";
            this.RFQ.PartyBY.Contact.FunctionCode = LeSMTML.MTML.ContactFunction.PD;
            this.RFQ.PartyVN.Contact.FunctionCode = LeSMTML.MTML.ContactFunction.SR;
            this.RFQ.Amounts.Clear();
            this.RFQ.Parties.Remove(this.RFQ.PartyBA);
            this.RFQ.Parties.Remove(this.RFQ.PartyCN);
            this.RFQ.Equipment = new LeSMTML.MTML.Equipment();
            if (this.BuyerSupplierInfo == null) this.BuyerSupplierInfo = new LeSMTML.MTML.BuyerSupplierInfo();
        }
    }

    public class PDFRoutine
    {
        public PDFRoutine()
        {
            try
            {
                Aspose.Pdf.License lic = new Aspose.Pdf.License();
                lic.SetLicense("Aspose.Total.lic");
            }
            catch { }
        }

        public string GetText(string FileName, bool RemoveBlankLines)
        {
            string extractedText = "";
            try
            {
                string ext = Path.GetExtension(FileName);
                ext = ext.Trim('.').ToLower();
                if (ext == "pdf")
                {
                    using (Aspose.Pdf.Document _pdf = new Aspose.Pdf.Document(FileName))
                    {
                        TextAbsorber _obj = new TextAbsorber();
                        _obj.ExtractionOptions = new Aspose.Pdf.Text.TextOptions.TextExtractionOptions(Aspose.Pdf.Text.TextOptions.TextExtractionOptions.TextFormattingMode.Pure);

                        _pdf.Pages.Accept(_obj);
                        extractedText = _obj.Text;
                        _pdf.FreeMemory();
                        _pdf.Dispose();
                    }
                }
                else if (ext == "txt")
                {
                    extractedText = File.ReadAllText(FileName);
                }

                extractedText = extractedText.Replace("\0", " ");
                if (RemoveBlankLines)
                {
                    // updated //
                    extractedText = extractedText.Replace("\r\r", "\r");
                    string BlanckLines = Environment.NewLine + Environment.NewLine;
                    while (extractedText.Contains(BlanckLines))
                    {
                        extractedText = extractedText.Replace(BlanckLines, Environment.NewLine);
                    }
                }
                return extractedText;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
