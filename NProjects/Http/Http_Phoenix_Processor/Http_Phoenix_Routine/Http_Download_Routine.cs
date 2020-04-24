using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using Aspose.Pdf.Text;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;
using MTML.GENERATOR;
using System.Globalization;

namespace Http_Phoenix_Routine
{
    public class Http_Download_Routine : LeSCommon.LeSCommon
    {
        string Module { get; set; }
        string DocType { get; set; }
        string PageGUID { get; set; }
        FileInfo _orgDocFile = null;
       public  bool moveToError = false, reprocessed = false, IsRFQ = false, IsPO = false, IsDecline = false,IsLink_NewVersion=false,IsProcess_PO_URL=false;
        public string sAuditMesage = "", MsgFile = "", orgEmlFile = "", FileType = "", currentLinkFile = "", currentXMLFile = "", MapPath = "", currentURL = "",
            LinkPath = "", ImagePath = "", VRNO = "", BuyerName = "", errLog = "", MsgNumber = "", pdfBuyerEmail = "", VendorEmail = "", currCode = "",
            QuotePOCPath = "", MessageNumber = "", LeadDays = "", Currency = "", MsgRefNumber = "", UCRefNo = "", AAGRefNo = "",NewConfigURL="",EncryptURL="",OrgConfigURL="",
            LesRecordID = "", BuyerPhone = "", BuyerEmail = "", BuyerFax = "", supplierName = "", supplierPhone = "", supplierEmail = "", supplierFax = "",
            VesselName = "", PortName = "", PortCode = "", SupplierComment = "", PayTerms = "", PackingCost = "", FreightCharge = "", GrandTotal = "", Allowance = ""
             , TotalLineItemsAmount = "", BuyerTotal = "", DtDelvDate = "", dtExpDate = "", TransportMode = "", Remarks = "", TaxCost = "", TermsCond = "", ImoNo = "",
             ItemMismatchScreenShot = "";
        int pdfItemStartCount = 0, IsAltItemAllowed = 0, IsPriceAveraged = 0, IsUOMChanged = 0, counter = 0;
        double CalculatedTotal = 0, AddDiscount = 0;
        Dictionary<string, string> lstBuyer = new Dictionary<string, string>();
        Dictionary<string, string> lstSupp = new Dictionary<string, string>();
        List<List<string>> _xmlItems = null;
        List<string> xmlFiles = new List<string>();
        public LineItemCollection _lineitem = null;
        public MTMLInterchange _interchange { get; set; }
        public string[] _urlVersions;
        // string prevURL = "";

        public Http_Download_Routine()
        {
            try
            {
                Aspose.Pdf.License _lic = new Aspose.Pdf.License();
                _lic.SetLicense("Aspose.Total.lic");
            }
            catch (Exception ex)
            {
                LogText = "Exception at constructor: " + ex.Message;
            }
        }

        public void LoadAppSettings()
        {
            SessionIDCookieName = "ASP.NET_SessionId";
            MapPath = AppDomain.CurrentDomain.BaseDirectory + "Map_Files";
            LinkPath = ConfigurationManager.AppSettings["LINK_PATH"].Trim();
            ImagePath = ConfigurationManager.AppSettings["IMAGE_PATH"].Trim();
            AuditPath = ConfigurationManager.AppSettings["AUDIT_PATH"].Trim();
            QuotePOCPath = ConfigurationManager.AppSettings["QUOTE_POC_PATH"].Trim();
            if (QuotePOCPath == "") QuotePOCPath = AppDomain.CurrentDomain.BaseDirectory + "MTML_Uploads";
            NewConfigURL = ConfigurationManager.AppSettings["NEW_MAILURL"].Trim();//added by kalpita on 13/12/2019
            _urlVersions = ConfigurationManager.AppSettings["URLVersion"].Split(',');//added by kalpita on 13/12/2019
            EncryptURL = ConfigurationManager.AppSettings["ENCRYPTURL"].Trim();//added by kalpita on 13/12/2019
            OrgConfigURL = ConfigurationManager.AppSettings["ORG_MAILURL"].Trim();//added by kalpita on 13/12/2019
            IsProcess_PO_URL =(!string.IsNullOrEmpty(ConfigurationManager.AppSettings["PROCESS_PO_LINK"]))?Convert.ToBoolean(ConfigurationManager.AppSettings["PROCESS_PO_LINK"].Trim()):false;//added by kalpita on 13/12/2019
        }

        public void ProcessLinkFiles()
        {
            try
            {
                LoadAppSettings();
                SetBuyerSupplierCodes();

                LogText = "Phoenix Processor Started.";
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
                            #region /* Copy Link File to Attachment Path */
                            if (this.ImagePath.Trim() != "" && Directory.Exists(this.ImagePath))
                            {
                                file.CopyTo(this.ImagePath + "\\" + file.Name, true);
                            }
                            #endregion

                            this.currentXMLFile = "";
                            this.currentLinkFile = file.FullName;
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


                            if (this.URL.StartsWith(EncryptURL) && (this.URL.EndsWith("apps.southnests.com")))//|| this.URL.EndsWith("executiveship.com")//added on 19-6-19 
                            {
                                if (LoadURL("iframe", "id", "filterandsearch", true))//for rfq
                                {
                                    this.URL = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//form").GetAttributeValue("action","").Trim();
                                    this.URL = OrgConfigURL + this.URL;
                                }
                                else if(LoadURL("iframe","id","ifVendorRemarks",true))//for po
                                {
                                    this.URL = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//form").GetAttributeValue("action", "").Trim();
                                    this.URL = OrgConfigURL + this.URL;
                                }
                             }


                            LogText = " URL : " + this.URL;

                            //if (!this.URL.Contains("apac01.safelinks.protection.outlook.com"))//commented on 14-2-19
                            //{
                                if (this.URL != "" && (this.URL.Contains("PurchaseQuotationItems")))
                                {
                                    #region /* Process RFQ */
                                    this.DocType = "RFQ";
                                    this.Module = "PHOENIX_RFQ";
                                    bool isFileProcessed = GetRFQDetails(this.URL);
                                    if (isFileProcessed)
                                    {
                                        // LogText = "File '" + file.Name + "' is processed successfully.";
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
                                        //CreateAuditFile(file.Name, "Phoenix_Http_RFQ", VRNO.Trim(), "Error", "Unable to process link file.", BuyerCode, SupplierCode, AuditPath);
                                        CreateAuditFile(file.Name, "Phoenix_Http_RFQ", VRNO.Trim(), "Error", "LeS-1004:Unable to process file.", BuyerCode, SupplierCode, AuditPath);
                                    }
                                    //Unable to process file

                                    #endregion
                                }
                                else if (this.URL != "" && this.URL.Contains("PurchaseVendorRemark"))
                                {
                                    if (ConfigurationManager.AppSettings["SUPPLIER_BY_PORT"].Trim().ToUpper() != "TRUE")
                                    {
                                        #region /* Process PO */
                                        this.DocType = "PO";
                                        this.Module = "PHOENIX_PO";                                       
                                        bool isFileProcessed = (IsProcess_PO_URL) ? GetPODetails(this.URL) : GetPODetails_No_Url();//changed by kalpita on 07/02/2020
                                        if (isFileProcessed)
                                        {
                                            // move to backup
                                            MoveFile(LinkPath + "\\Backup", file);
                                            MoveFile(_orgDocFile.Directory.FullName + "\\Backup", _orgDocFile);
                                        }
                                        else
                                        {
                                            LogText = "Unable to process link file '" + file.Name + "'.";
                                            if (moveToError)
                                            {
                                                MoveFile(LinkPath + "\\Error_files", file);
                                            }
                                            CreateAuditFile(file.Name, "Phoenix_Http_PO", VRNO.Trim(), "Error", "LeS-1004:Unable to process file.", BuyerCode, SupplierCode, AuditPath);
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region /* for rms*/
                                        string fname = Path.GetFileNameWithoutExtension(file.FullName);
                                        if (fname != null)
                                        {
                                            DirectoryInfo _dInfo = new DirectoryInfo(LinkPath);
                                            FileInfo[] filesInDir = _dInfo.GetFiles("*" + fname + "*.*");
                                            foreach (FileInfo f in filesInDir)
                                            {
                                                if (File.Exists(LinkPath + "\\Error_files\\" + Path.GetFileName(f.FullName))) File.Delete(LinkPath + "\\Error_files\\" + Path.GetFileName(f.FullName));
                                                File.Move(f.FullName, LinkPath + "\\Error_files\\" + Path.GetFileName(f.FullName));
                                                LogText = "Po file " + Path.GetFileName(f.FullName) + " moved to Error_files folder.";
                                                LogText = "";
                                            }
                                        }
                                        //  MoveFile(LinkPath + "\\Error_files", file);
                                        #endregion
                                    }
                                }
                                else
                                {
                                    LogText = "Unable to process link file; Link not found in file " + Path.GetFileName(sFile);
                                    //CreateAuditFile(file.Name, "Phoenix_Http", VRNO.Trim(), "Error", "Unable to process link file; Link not found in file " + Path.GetFileName(sFile), BuyerCode, SupplierCode, AuditPath);
                                    CreateAuditFile(file.Name, "Phoenix_Http", VRNO.Trim(), "Error", "LeS-1001:Unable to find URL in file " + Path.GetFileName(sFile), BuyerCode, SupplierCode, AuditPath);                                  
                                    MoveFile(LinkPath + "\\Error_files", file);
                                }
                           // }
                           //else
                           // {
                           //     LogText = "Unable to process link file; Link not found in file " + Path.GetFileName(sFile);
                           //     CreateAuditFile(file.Name, "Phoenix_Http", VRNO.Trim(), "Error", "Unable to process link file; Link not found in file " + Path.GetFileName(sFile), BuyerCode, SupplierCode, AuditPath);
                           //     MoveFile(LinkPath + "\\Error_files", file);
                           // }
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

                            if (this.Module == "PHOENIX_RFQ")
                            {
                                if (moveToError != false) CreateAuditFile(file.Name, "Phoenix_Http_RFQ", VRNO.Trim(), "Error", "LeS-1004:Unable to process  file '" + file.Name + "'. Error - " + ex.Message, BuyerCode, SupplierCode, AuditPath);
                            }
                            else
                            {
                                moveToError = true;
                                CreateAuditFile(file.Name, "Phoenix_Http", this.VRNO.Trim(), "Error", "LeS-1004:Unable to process file '" + file.Name + "'. Error -" + ex.Message, BuyerCode, SupplierCode, AuditPath);
                            }

                            if (moveToError)
                            {
                                MoveFile(LinkPath + "\\Error_files", file);
                            }
                            string eFile = ImagePath + "\\Phoenix_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                            //_httpWrapper._CurrentDocument.DocumentNode.Descendants()
                            //                    .Where(n => n.Name == "link")
                            //                    .ToList()
                            //                    .ForEach(n => n.Remove());
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

                #region // Get Report PDF //
                string filename = "";
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsItemCountMismatch"].Trim()) == false)
                {
                    filename = GetAttachment();
                    this.URL = currentURL;
                    //LoadURL("input", "id", "txtnopage", true);//commented by Kalpita on 17/12/2019
                   LoadURL("input", "id", "txtVessel", true);
                }
                else filename = Path.GetFileName(ItemMismatchScreenShot);
                #endregion

                if (IsRFQ || IsPO)
                {
                    strerr = "Unable to export xml!";
                    ExportToLESML(_xmlHeader, _xmlItems, filename);
                }

                Logout();
                IsRFQ = false; IsPO = false; ItemMismatchScreenShot = "";

                // if (this.PageGUID.Trim().Length > 0) SetGUIDs(this.PageGUID.Trim());
            }
            catch (Exception ex)
            {
                LogText = strerr + " Error - " + ex.Message;
                throw;
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
                    _xmlHeader = GetRFQHeader();
                }
                else if (this.DocType == "PO")
                {
                    if (_orgDocFile != null)
                    {
                        RichTextBox txtData = new RichTextBox();
                        txtData.Text = GetPDFText(_orgDocFile.FullName);
                        if (txtData.Text.Length > 0)
                        {
                            txtData.WordWrap = false;
                            PhoenixPDFRoutinecs pdfRoutine = new PhoenixPDFRoutinecs();
                            _xmlHeader = pdfRoutine.GetOrderHeader(txtData, this.currentLinkFile);
                        }
                        else
                        {
                            throw new Exception("Invalid PDF Order Document");
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid Email");
                    }
                }

                #region // Set buyer code //
                Uri _url = new Uri(this.URL);
                string Host = _url.Host;
                if (lstBuyer.ContainsKey(Host))
                {
                    string[] byrInfo = lstBuyer[Host].Trim().Split(',');
                    BuyerCode = convert.ToString(byrInfo[0]).Trim();
                    if (byrInfo.Length > 1) BuyerName = convert.ToString(byrInfo[1]);
                }
                #endregion

                #region // Set supplier code //
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

                            //decide supplier from port
                            if (ConfigurationManager.AppSettings["SUPPLIER_BY_PORT"].Trim().ToUpper() == "TRUE")
                            {
                                if (convert.ToString(_xmlHeader["PORT_NAME"]).Trim() != "")
                                {
                                    string PORT = convert.ToString(_xmlHeader["PORT_NAME"]).Trim().ToUpper();
                                    if (ConfigurationSettings.AppSettings[PORT + "_" + SupplierCode] != null)
                                    {
                                        string[] newSuppCode = ConfigurationSettings.AppSettings[PORT + "_" + SupplierCode].Trim().Split('_');
                                        if (newSuppCode.Length > 1)
                                        {
                                            SupplierCode = newSuppCode[1].Trim().ToUpper();
                                        }
                                    }
                                }
                            }
                        }
                        //

                        //to decide actions
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
                        //
                        // }
                        // else throw new Exception("'" + domain + "' not found in config file.");
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

        private bool Request_apps2_southnests_com(string url)
        {
            HttpWebResponse response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.KeepAlive = true;
                request.Headers.Add("Upgrade-Insecure-Requests", @"1");
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
                request.Headers.Add("Sec-Fetch-Site", @"none");
                request.Headers.Add("Sec-Fetch-Mode", @"navigate");
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");
                request.Headers.Set(HttpRequestHeader.Cookie, @"ASP.NET_SessionId=" + _httpWrapper._dctSetCookie["ASP.NET_SessionId"] + "; __AntiXsrfToken=" + _httpWrapper._dctSetCookie["__AntiXsrfToken"]);

                response = (HttpWebResponse)request.GetResponse();
                string res = _httpWrapper.ReadResponse(response);
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

        public List<List<string>> GetItemsData(Dictionary<string, string> _xmlHeader)
        {
            List<List<string>> lstItems = new List<List<string>>();
            try
            {
                LogText = "Reading Item Details ..";
                if (this.DocType == "RFQ")
                {
                    if (IsRFQ)
                    {
                        lstItems = (!IsLink_NewVersion) ? GetRFQItems_org(_xmlHeader) : GetRFQItems_New(_xmlHeader);
                    }
                }
                else if (IsPO)
                {

                    string txtPDFFile = Path.GetFileNameWithoutExtension(_orgDocFile.FullName) + ".txt";
                    string txtPDFPath = Path.GetDirectoryName(_orgDocFile.FullName) + "\\Temp";
                    if (!Directory.Exists(txtPDFPath))
                    {
                        Directory.CreateDirectory(txtPDFPath);
                    }
                    RichTextBox txtData = new RichTextBox();
                    string strtxt = GetPDFText(_orgDocFile.FullName);
                    if (File.Exists(txtPDFPath + "\\" + txtPDFFile))
                    {
                        strtxt = File.ReadAllText(txtPDFPath + "\\" + txtPDFFile);
                        txtData.Text = strtxt;
                    }
                    else
                    {
                        File.WriteAllText(txtPDFPath + "\\" + txtPDFFile, strtxt);
                        txtData.Text = strtxt;
                    }
                    if (txtData.Text.Length > 0)
                    {
                        txtData.WordWrap = false;
                        PhoenixPDFRoutinecs pdfRoutine = new PhoenixPDFRoutinecs();
                        lstItems = pdfRoutine.GetOrderItems(txtData);
                    }
                    //commnted by kalpita on 04/02/2020
                    //RichTextBox txtData = new RichTextBox();
                    //txtData.Text = GetPDFText(_orgDocFile.FullName);
                    //if (txtData.Text.Length > 0)
                    //{
                    //    txtData.WordWrap = false;
                    //    PhoenixPDFRoutinecs pdfRoutine = new PhoenixPDFRoutinecs();
                    //    lstItems = pdfRoutine.GetOrderItems(txtData);
                    //}
                }
            }
            catch (Exception ex)
            {
                LogText = "Error while reading items - " + ex.StackTrace;
                throw ex;
            }
            return lstItems;
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

                    // Commented to add link file name in audit log //
                    //CreateAuditFile(Path.GetFileName(_lesXML.FileName), "Phoenix_Http_" + _lesXML.Doc_Type, _lesXML.BuyerRef, "Downloaded", _lesXML.Doc_Type + " '" + _lesXML.BuyerRef + "' downloaded successfully.", BuyerCode, SupplierCode, AuditPath);
                    CreateAuditFile(Path.GetFileName(this.currentLinkFile), "Phoenix_Http_" + _lesXML.Doc_Type, _lesXML.BuyerRef, "Downloaded", _lesXML.Doc_Type + " '" + _lesXML.BuyerRef + "' downloaded successfully.", BuyerCode, SupplierCode, AuditPath);
                }
            }
            catch (Exception ex)
            {
                LogText = "Unable to generate xml for " + DocType + " '" + _lesXML.BuyerRef + "'";
                CreateAuditFile(Path.GetFileName(this.currentLinkFile), "Phoenix_Http_" + _lesXML.Doc_Type, _lesXML.BuyerRef, "Error", "LeS-1004:Unable to process file for " + DocType + " '" + _lesXML.BuyerRef + "'", BuyerCode, SupplierCode, AuditPath);                
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
                if (_xmlHeader.ContainsKey("VESSEL")) _lesXML.Vessel = _xmlHeader["VESSEL"].Trim();
                if (_xmlHeader.ContainsKey("IMO")) _lesXML.IMONO = _xmlHeader["IMO"].Trim();
                if (_xmlHeader.ContainsKey("PORT_NAME")) _lesXML.PortName = _xmlHeader["PORT_NAME"].Trim();

                _lesXML.Recipient_Code = SupplierCode;
                _lesXML.Sender_Code = BuyerCode;

                if (convert.ToString(this.currentLinkFile).Trim().Length > 0)
                {
                    _lesXML.FileName = Path.GetFileNameWithoutExtension(convert.ToString(this.currentLinkFile)) + "_" + convert.ToFileName(this.VRNO) + ".xml";
                }
                else
                {
                    _lesXML.FileName = this.DocType + "_" + convert.ToFileName(this.VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xml";
                }

                string BuyerRemarks = "";
                if (_xmlHeader.ContainsKey("VESSEL_TYPE")) BuyerRemarks += "Vessel Type : " + _xmlHeader["VESSEL_TYPE"].Trim();
                if (_xmlHeader.ContainsKey("YARD") && _xmlHeader["YARD"].Trim().Length > 0) BuyerRemarks += Environment.NewLine + "Yard : " + _xmlHeader["YARD"].Trim();
                if (_xmlHeader.ContainsKey("HULLNO") && _xmlHeader["HULLNO"].Trim().Length > 0) BuyerRemarks += Environment.NewLine + "Hull No. : " + _xmlHeader["HULLNO"].Trim();
                if (_xmlHeader.ContainsKey("YEARBUILT") && _xmlHeader["YEARBUILT"].Trim().Length > 0) BuyerRemarks += Environment.NewLine + "Year Built : " + _xmlHeader["YEARBUILT"].Trim();
                if (_xmlHeader.ContainsKey("REMARKS")) BuyerRemarks += Environment.NewLine + _xmlHeader["REMARKS"].Trim();
                if (_xmlHeader.ContainsKey("HEADER_REMARKS")) BuyerRemarks += Environment.NewLine + Environment.NewLine + _xmlHeader["HEADER_REMARKS"].Trim();
                string BlanksLines = Environment.NewLine + Environment.NewLine;
                while (BuyerRemarks.Contains(BlanksLines)) BuyerRemarks = BuyerRemarks.Replace(BlanksLines, Environment.NewLine);
                if (BuyerRemarks.Contains("<br />")) BuyerRemarks = BuyerRemarks.Replace("<br />", " ");
                _lesXML.Remark_Sender = BuyerRemarks.Trim();

                GetAddressDetails(ref _lesXML, _xmlHeader);

                string MsgNumber = "";
                if (this.DocType == "RFQ")
                {//changed by Kalpita on 23/01/2020
                    if (IsLink_NewVersion) {
                        MsgNumber = this.URL.Replace("apps2", "apps").Replace("PurchaseQuotationRFQ", "PurchaseQuotationItems");} 
                    else { MsgNumber = this.URL; }//changed by kalpita on 08/01/2020
                }
                else if (this.DocType == "PO") MsgNumber = currentURL;

                _lesXML.DocLinkID = MsgNumber; // MessageNumber in eSupplier
                _lesXML.DocReferenceID = MsgNumber; // MessageReferenceNumber in eSupplier

                if (this.DocType == "RFQ")
                {
                    // Dates                 
                    _lesXML.Date_Document = GetDate(_xmlHeader["RFQ_DATE"].Trim());  /* RFQ Date */
                }
                else
                {
                    if (_xmlHeader.ContainsKey("CURRENCY")) _lesXML.Currency = _xmlHeader["CURRENCY"].Trim();
                    else _lesXML.Currency = currCode.Trim();
                    if (_xmlHeader.ContainsKey("QUOTE_REF")) _lesXML.Reference_Document = _xmlHeader["QUOTE_REF"].Trim();
                    if (_xmlHeader.ContainsKey("PAY_TERMS") && !_xmlHeader["PAY_TERMS"].Trim().Contains("Select")) _lesXML.Remark_PaymentTerms = _xmlHeader["PAY_TERMS"].Trim();
                    if (_xmlHeader.ContainsKey("DEL_TERMS") && !_xmlHeader["DEL_TERMS"].Trim().Contains("Select")) _lesXML.Remark_DeliveryTerms = _xmlHeader["DEL_TERMS"].Trim();
                    if (_xmlHeader.ContainsKey("LEAD_DAYS") && convert.ToInt(_xmlHeader["LEAD_DAYS"]) > 0) _lesXML.LeadTimeDays = _xmlHeader["LEAD_DAYS"].Trim();

                    // Dates //
                    if (_xmlHeader.ContainsKey("ORDER_DATE")) _lesXML.Date_Document = GetDate(_xmlHeader["ORDER_DATE"].Trim());  /* ORDER Date */
                    else _lesXML.Date_Document = DateTime.Now.ToString("yyyyMMdd");
                    if (_xmlHeader.ContainsKey("ETA")) _lesXML.Date_ETA = GetDate(_xmlHeader["ETA"].Trim());
                    if (_xmlHeader.ContainsKey("ETD")) _lesXML.Date_ETD = GetDate(_xmlHeader["ETD"].Trim());

                    // Remarks 
                    if (Remarks.Contains("<br />")) Remarks = Remarks.Replace("<br />", " ");
                    _lesXML.Remark_Sender += Environment.NewLine + Remarks.Trim();

                    // Amounts //
                    if (_xmlHeader.ContainsKey("TAX_COST")) _lesXML.Total_Freight = convert.ToDouble(_xmlHeader["TAX_COST"].Trim()).ToString("0.00");
                    if (_xmlHeader.ContainsKey("ITEM_TOTAL")) _lesXML.Total_LineItems_Net = convert.ToDouble(_xmlHeader["ITEM_TOTAL"].Trim()).ToString("0.00");
                    if (_xmlHeader.ContainsKey("GRANT_TOTAL")) _lesXML.Total_Net_Final = convert.ToDouble(_xmlHeader["GRANT_TOTAL"].Trim()).ToString("0.00");

                    // Original Doc File //
                    _lesXML.OrigDocFile = Path.GetFileName(_orgDocFile.Name);

                    #region // Commented  //
                    #region // Read PDF File And Compare Total (Commented) //
                    //bool isItemTotalCorrect = false; //, isGrantTotalCorrect = false;
                    //double FrightCharges = 0; double pdfGrantTotal = 0, pdfItemTotal = 0;
                    //string Remarks = "";
                    //if (_orgDocFile != null && _orgDocFile.Name.Trim() != "")
                    //{
                    //    RichTextBox txtPDF = new RichTextBox();
                    //    txtPDF.Text = GetPDFText(_orgDocFile.FullName);

                    //    for (int i = 0; i < txtPDF.Lines.Length; i++)
                    //    {
                    //        string line = txtPDF.Lines[i];
                    //        if (line.Trim().StartsWith("Line Item Sub Total:"))
                    //        {
                    //            pdfItemTotal = convert.ToDouble(line.Replace("Line Item Sub Total:", "").Trim());
                    //            if (Math.Round(pdfItemTotal, 2) == Math.Round(CalculatedTotal, 2)) isItemTotalCorrect = true;
                    //            else
                    //            {
                    //                // UPDATED ON 09-MAY-2017 //
                    //                double pdfTotal = Math.Round(pdfItemTotal, 2), calTotal = Math.Round(CalculatedTotal, 2);
                    //                if (pdfTotal > calTotal)
                    //                {
                    //                    if ((pdfTotal - calTotal) < 1) isItemTotalCorrect = true;
                    //                }
                    //                else
                    //                {
                    //                    if ((calTotal - pdfTotal) < 1) isItemTotalCorrect = true;
                    //                }
                    //                //
                    //            }
                    //        }
                    //        else if (line.Trim().StartsWith("Total for this Order"))
                    //        {
                    //            pdfGrantTotal = convert.ToDouble(line.Replace("Total for this Order", "").Trim());
                    //        }
                    //        else if (line.Trim().StartsWith("Tax and Charges"))
                    //        {
                    //            FrightCharges = convert.ToDouble(line.Replace("Tax and Charges", "").Trim());
                    //        }
                    //        else if (line.Trim().StartsWith("Please invoice the order to"))
                    //        {
                    //            for (int k = i; k < txtPDF.Lines.Length; k++)
                    //            {
                    //                if (txtPDF.Lines[k].Trim().StartsWith("Following are the terms and conditions")) break;
                    //                if (txtPDF.Lines[k].Trim().Length > 0) Remarks += txtPDF.Lines[k].Trim() + Environment.NewLine;
                    //            }
                    //        }
                    //    }
                    //}
                    //_lesXML.Total_Freight = FrightCharges.ToString("0.00"); // Set Feight Charges
                    #endregion

                    //if (!isItemTotalCorrect)
                    //{
                    //    if (reprocessed == false)
                    //    {
                    //        reprocessed = true;
                    //        _xmlHeader = this.GetOrderDetails(_lesXML.BuyerRef, _orgDocFile.FullName, convert.ToString(_lesXML.Addresses[0].eMail), convert.ToString(_lesXML.Addresses[1].eMail));
                    //        _xmlItems = this.GetOrderItems(_orgDocFile.FullName, ref _xmlItems);
                    //        _lesXML.LeadTimeDays = "";
                    //        _lesXML.LineItems.Clear();
                    //        _lesXML.Addresses.Clear();
                    //        ExporttoHeader(_xmlHeader, _lesXML);
                    //    }
                    //    else
                    //    {
                    //        errLog = "Item Total Mismatched.";
                    //        throw new Exception("Item Total Mismatched.");
                    //    }
                    //}

                    #endregion

                    #region // Copy File to Attachments //
                    if (File.Exists(_orgDocFile.FullName))
                    {
                        try
                        {
                            if (!Directory.Exists(this.ImagePath)) Directory.CreateDirectory(this.ImagePath);
                            _orgDocFile.CopyTo(this.ImagePath + "\\" + _orgDocFile.Name, true);
                        }
                        catch { }
                    }
                    #endregion
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
                        _item.ItemRef = _lItem[1].Trim(); // UPDDATED ON 29-MAY-2015
                        _item.Name = _lItem[2];
                        _item.Quantity = _lItem[3];
                        _item.Unit = _lItem[4];
                        if (_lItem[5].Trim().Length > 0)
                        {
                            _item.EquipMaker = _lItem[5];
                        }
                        _item.EquipRemarks += Environment.NewLine + _lItem[6];
                        if (_lItem[7].Trim().Length > 0)
                        {
                            _item.Equipment = _lItem[7].Trim();
                        }
                        _item.Remark = convert.ToString(_lItem[8]).Trim();
                        _item.OriginatingSystemRef = _lItem[0];
                        _lesXML.LineItems.Add(_item);
                    }
                }
                else
                {
                    foreach (List<string> _lItem in _xmlItems)
                    {
                        LeSXML.LineItem _item = new LeSXML.LineItem();
                        _item.Number = _lItem[0];
                        _item.ItemRef = _lItem[1].Trim();
                        _item.Name = _lItem[2];
                        _item.Quantity = _lItem[3];
                        _item.Unit = _lItem[4];
                        _item.ListPrice = _lItem[5].Trim();
                        _item.Discount = _lItem[6].Trim();
                        _item.EquipMaker = _lItem[7].Trim();
                        _item.EquipRemarks = _lItem[8].Trim();
                        _item.Equipment = _lItem[9].Trim();
                        _item.Remark = convert.ToString(_lItem[10]).Trim();
                        _item.OriginatingSystemRef = _lItem[0];

                        if (_lesXML.Currency.Length == 0) _lesXML.Currency = _lItem[11].Trim();

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
            if (_xmlHeader.ContainsKey("SHIP_NAME")) _lesXML.Addresses[2].AddressName = _xmlHeader["SHIP_NAME"].Trim();
            if (_xmlHeader.ContainsKey("SHIP_ADDRESS1")) _lesXML.Addresses[2].Address1 = _xmlHeader["SHIP_ADDRESS1"].Trim();
            if (_xmlHeader.ContainsKey("SHIP_PHONE")) _lesXML.Addresses[2].Phone = _xmlHeader["SHIP_PHONE"].Trim();
            if (_xmlHeader.ContainsKey("SHIP_EMAIL")) _lesXML.Addresses[2].eMail = _xmlHeader["SHIP_EMAIL"].Trim();

            // Billing Address (Updated By Sanjita (11-Dec-18))
            _lesXML.Addresses.Add(new LeSXML.Address());
            _lesXML.Addresses[3].Qualifier = "BA";
            if (_xmlHeader.ContainsKey("BILL_NAME")) _lesXML.Addresses[3].AddressName = _xmlHeader["BILL_NAME"].Trim();
            if (_xmlHeader.ContainsKey("BILL_ADDRESS1")) _lesXML.Addresses[3].Address1 = _xmlHeader["BILL_ADDRESS1"].Trim();
            if (_xmlHeader.ContainsKey("BILL_CONTACT")) _lesXML.Addresses[3].ContactPerson = _xmlHeader["BILL_CONTACT"].Trim();
            if (_xmlHeader.ContainsKey("BILL_PHONE")) _lesXML.Addresses[3].Phone = _xmlHeader["BILL_PHONE"].Trim();
            if (_xmlHeader.ContainsKey("BILL_EMAIL")) _lesXML.Addresses[3].eMail = _xmlHeader["BILL_EMAIL"].Trim();
        }

        public bool DownloadAttachments(string RequestURL, string DownloadFileName, string ContentType = "Application/unknown")
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
                    //int urlIdx = txt.Text.IndexOf("https");
                    //if (urlIdx == -1)
                    //{
                       int urlIdx = txt.Text.IndexOf("\"https");
                        if (urlIdx == -1)
                        {
                            urlIdx = txt.Text.IndexOf("\"<https");//on 14-2-19
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
                        }
                        else searchCriteria = 1;
                    //}
                    //else searchCriteria = 3;

                    if (urlIdx > 0)
                    {
                         string part1 ="";
                         //if (searchCriteria == 3)
                         //    part1 = txt.Text.Substring(urlIdx);
                         //else 
                         part1 = txt.Text.Substring(urlIdx + 1);
                        int endUrlIndx = -1;
                        if (searchCriteria == 1) endUrlIndx = part1.Trim().IndexOf("\""); 
                        else if (searchCriteria == 2) endUrlIndx = part1.Trim().IndexOf(">\"");
                        //else if (searchCriteria == 3) endUrlIndx = part1.Trim().IndexOf(">");

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
                            else if ((part1.Trim().IndexOf("STORE") > 0))//added on 05-03-2019
                            {
                                endUrlIndx = part1.Trim().IndexOf("STORE");
                                if (endUrlIndx > 0)
                                {
                                    string orgURL = part1.Substring(0, endUrlIndx + 5);
                                    URL = orgURL.Trim('"').Trim().Replace("&amp;", "&");
                                }
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
                        if (line.Trim().StartsWith("\"<https") && !line.Trim().Contains("PurchaseVendorAddressEdit"))
                        {
                            URL = line.Trim().Trim('"').Trim().TrimStart('<').TrimEnd('>').Trim();

                        }
                    }
                }
                if (!string.IsNullOrEmpty(URL)) { IsLink_NewVersion = (URL.Contains("PhoenixTelerik")) ? true : false; }//added by Kalpita on 16/12/2019
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

        public override bool PrintScreen(string sFileName)//12-2-2018 (Updated on 13-DEC-18)
        {
            if (!Directory.Exists(Path.GetDirectoryName(sFileName) + "\\Temp")) Directory.CreateDirectory(Path.GetDirectoryName(sFileName) + "\\Temp");
            sFileName = Path.GetDirectoryName(sFileName) + "\\Temp\\" + Path.GetFileName(sFileName);
            try
            {
                using (WebBrowser wb = new WebBrowser())
                {
                    string sHTML = _CurrentResponseString;
                    if (sHTML != null) //namrata
                    {
                        sHTML = sHTML.Replace("\"/Phoenix/css", "\"https://apps.southnests.com:443/Phoenix/css");

                        string HTMLFile = Path.ChangeExtension(sFileName, ".html");
                        File.WriteAllText(HTMLFile, sHTML.Trim());

                        wb.ScrollBarsEnabled = false;
                        wb.ScriptErrorsSuppressed = true;
                        wb.Navigate(HTMLFile);
                        while (wb.ReadyState != WebBrowserReadyState.Complete) Application.DoEvents();

                        wb.Width = wb.Document.GetElementById("navigation").ScrollRectangle.Width;
                        wb.Height = wb.Document.GetElementById("navigation").ScrollRectangle.Height + 100;
                        using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(wb.Width, wb.Height))
                        {
                            wb.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, wb.Width, wb.Height));
                            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, wb.Width, wb.Height);
                            System.Drawing.Bitmap cropped = bitmap.Clone(rect, bitmap.PixelFormat);
                            cropped.Save(sFileName, System.Drawing.Imaging.ImageFormat.Png);
                        }
                    }
                }
                if (File.Exists(sFileName))
                {
                    MoveFiles(sFileName, ImagePath + "\\" + Path.GetFileName(sFileName));
                    return (File.Exists(ImagePath + "\\" + Path.GetFileName(sFileName)));
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            //if (base.PrintScreen(sFileName))
            //if (File.Exists(sFileName))
            //{
            //    MoveFiles(sFileName, ImagePath + "\\" + Path.GetFileName(sFileName));
            //    return (File.Exists(ImagePath + "\\" + Path.GetFileName(sFileName)));
            //}
            //else return false;
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

        #region RFQ
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
                        string _text = "";
                        for (int i = 2; i < arrLink.Length - 1; i++)
                        {
                            _text += arrLink[i] + "/";

                        }
                        if (!this.URL.Contains("apac01.safelinks.protection.outlook.com")) _RFQLink = arrLink[arrLink.Length - 1].Split('?')[1];

                        if (_text == "")//https://apps.southnests.com/Phoenix/Purchase/" 
                            currentURL = this.URL = @"" + OrgConfigURL + "PurchaseQuotationRFQ.aspx?" + HttpUtility.HtmlDecode(_RFQLink);//change on 30-8-18 as url redirect giving object ref error
                        else if (this.URL.Contains("apac01.safelinks.protection.outlook.com"))
                            currentURL = this.URL = @"" + OrgConfigURL + HttpUtility.HtmlDecode(_RFQLink);//change on 30-8-18 as url redirect giving object ref error
                        else
                        {
                            currentURL = this.URL = @"https://" + _text + "PurchaseQuotationRFQ.aspx?" + HttpUtility.HtmlDecode(_RFQLink);
                        }
                        //  MsgNumber = this.URL;
                        if (LoadURL("input", "id", "txtnopage", true))
                        {
                            HtmlNode _btnSave = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//a[@id='MenuQuotationLineItem_dlstTabs_ctl00_btnMenu'][@title='Save']");
                            if (_btnSave != null)
                            {
                                this.PageGUID = URL.Trim();
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
                        else
                        {
                            if (IsLink_NewVersion)
                            {
                                if (LoadURL("input", "id", "txtVessel", true))
                                {
                                    HtmlNode _btnSave = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//li/span[@class='rtbButton'][@title='Save']");
                                    if (_btnSave != null)
                                    {
                                        this.PageGUID = URL.Trim();
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
                }
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }
            return result;
        }

        private Dictionary<string, string> GetRFQHeader()
        {
            Dictionary<string, string> _xmlHeader = new Dictionary<string, string>();

            #region // read RFQ Header map files //
            if (File.Exists(MapPath + "\\" + this.DocType + "HEADER_MAP_PHOENIX.txt"))
            {
                string[] _lines = File.ReadAllLines(MapPath + "\\" + this.DocType + "HEADER_MAP_PHOENIX.txt");
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
                                //PageReference = _value;
                            }

                            else if (_keys[1].Split('|')[0] == "span" || _keys[1].Split('|')[0] == "textarea")
                            {
                                string cKey = _keys[1].Split('|')[0]; string cKeyvalue = ""; string attrtype = "";//added by kalpita on 17/12/2019
                                if (_keys[0].ToUpper() == "VRNO")
                                {
                                    string[] _Vrarr = _keys[1].Split('|')[1].Split(','); cKeyvalue = (!IsLink_NewVersion) ? _Vrarr[0] : _Vrarr[1];
                                    attrtype = (!IsLink_NewVersion) ? "id" : "class";
                                }
                                else { cKeyvalue = _keys[1].Split('|')[1]; attrtype = "id"; }
                                HtmlNode nodeVal = _httpWrapper.GetElement(cKey, attrtype, cKeyvalue);
                                _value = (nodeVal != null) ? nodeVal.InnerText.Trim() : "";
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
                        else
                        {
                            string[] _Versarr = _keys[1].Split(','); string cVersionNo = (!IsLink_NewVersion) ? _Versarr[0] : _Versarr[1];//added by kalpita on 17/12/2019
                            _xmlHeader.Add(_keys[0], cVersionNo);
                        }
                    }
                }
            }
            _xmlHeader.Add("DOC_TYPE", this.DocType);
            #endregion

            string prevURL = "";

            #region // Get Details //
            HtmlNode _lnkDelInstr =  _httpWrapper.GetElement("a", "id", "MenuQuotationLineItem_dlstTabs_ctl02_btnMenu");
            if (_lnkDelInstr != null)
            {
                if (_lnkDelInstr.InnerText.Trim() == "Details")
                {
                    HtmlNode _eleMenu = _httpWrapper.GetElement("a", "id", "MenuRegistersStockItem_dlstTabs_ctl02_btnMenu");
                    if (_eleMenu != null)
                    {
                        string _click = _eleMenu.GetAttributeValue("onclick", "").Trim();
                        string[] _arrClick = _click.Split('=');
                        if (_arrClick.Length > 0)
                        {
                            prevURL = this.URL;//"https://apps.southnests.com/Phoenix/Purchase
                            this.URL = OrgConfigURL+"PurchaseFormDetail.aspx?orderid=" + _arrClick[_arrClick.Length - 1].Replace("'); return false;", "").Trim() + "&launchedfrom=VENDOR";                            
                            if (LoadURL("input", "id", "_content_txtFormDetails_ucCustomEditor_ctl02", true))
                            {
                                //HtmlNode _eleRemarks = _httpWrapper.GetElement("input", "id", "_content_txtFormDetails_ucCustomEditor_ctl02");
                                //if (_eleRemarks != null)
                                //{
                                string HeaderDetails = _httpWrapper.GetElement("input", "id", "_content_txtFormDetails_ucCustomEditor_ctl02").GetAttributeValue("value", "").Trim();
                                //HeaderDetails = HeaderDetails.Replace("&", " &").Replace("&amp;","&").Replace("&amp;","&").Replace("&nbsp;"," ").Replace("&lt;","<").Replace("&gt;",">").Replace("<br / >","\r\n");

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

                                // }
                            }
                        }
                    }
                }
            }
            else
            {
                if (IsLink_NewVersion)
                {
                    HtmlNode _lnkDelMenu = _httpWrapper.GetElement("span", "title", "Details");
                    if (_lnkDelMenu != null)
                    {
                        if (_lnkDelMenu.InnerText.Trim() == "Details")
                        {
                            HtmlNode _eleMenu = _httpWrapper.GetElement("span", "title", "RFQ");
                            if (_eleMenu != null)
                            {
                                string _click = _eleMenu.GetAttributeValue("onclick", "").Trim();
                                string[] _arrClick = _click.Split('=');
                                if (_arrClick.Length > 0)
                                {
                                    prevURL = this.URL;

                                    _httpWrapper._SetRequestHeaders[HttpRequestHeader.Cookie] += ";__AntiXsrfToken=" + _httpWrapper._dctSetCookie["__AntiXsrfToken"];
                                    this.URL = "https://apps2.southnests.com/PhoenixTelerik/Purchase/PurchaseFormDetail.aspx?orderid=" + _arrClick[_arrClick.Length - 1].Replace("'); return false;", "").Trim() + "&launchedfrom=VENDOR";

                                    if (LoadURL("div", "id", "txtFormDetails"))
                                    {

                                        string HeaderDetails = _httpWrapper.GetElement("textarea", "id", "txtFormDetailsContentHiddenTextarea").GetAttributeValue("value", "").Trim();
                                        //txtFormDetails_contentIframe

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

                                        // }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            if (prevURL != "") this.URL = prevURL;

            //if (LoadURL("input", "id", "txtnopage", true))//commented by kalpita on 17/12/2019
            if (LoadURL("input", "id", "txtVessel", true))
            {
                this.VRNO = _xmlHeader["VRNO"];
                this.VRNO = this.VRNO.Replace("Quotation Details", "").Trim();
                this.VRNO = this.VRNO.TrimStart('[').TrimEnd(']').Trim();
                _xmlHeader["VRNO"] = this.VRNO;

                #region Commented
                //#region set buyer code
                //Uri _url = new Uri(this.URL);
                //string Host = _url.Host;
                //if (lstBuyer.ContainsKey(Host))
                //{
                //    string[] byrInfo = lstBuyer[Host].Trim().Split(',');
                //    BuyerCode = convert.ToString(byrInfo[0]).Trim();
                //    if (byrInfo.Length > 1) BuyerName = convert.ToString(byrInfo[1]);
                //}
                //#endregion

                //#region set supplier code
                //string _vemail = convert.ToString(_xmlHeader["VENDOR_EMAIL"]);
                //if (_vemail.Trim().Length > 0)
                //{
                //    int indx = _vemail.Trim().Split(',')[0].Trim().IndexOf('@');
                //    if (indx > 0)
                //    {
                //        string domain = _vemail.Trim().Split(',')[0].Substring(indx).Trim();
                //        if (lstSupp.ContainsKey(domain.Trim().ToLower()))
                //        {
                //            SupplierCode = convert.ToString(lstSupp[domain.Trim().ToLower()]);
                //        }
                //        else throw new Exception("'" + domain + "' not found in config file.");
                //    }
                //}
                //else
                //{
                //    errLog = "Unable to get supplier code.";
                //    throw new Exception("Unable to get supplier code.");
                //}
                //#endregion
                #endregion
            }

            return _xmlHeader;
        }

        private List<List<string>> GetRFQItems_org(Dictionary<string, string> _xmlHeader)
        {
            int _icurrpage = 1;
            string ItemRemarks = "";
            List<List<string>> lstItems = new List<List<string>>();
            HtmlNodeCollection _tr = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='gvVendorItem']//tr[@tabindex='-1']");
            if (_tr != null)
            {
                if (_tr.Count > 0)
                {
                    int Counter = 02, itemCount = 0;
                    string strCounter = "";
                    if (Counter < _tr.Count + 1) strCounter = "0" + Counter;
                    else if (Counter > 10) strCounter = Counter.ToString();
                    string _i = "gvVendorItem_ctl" + strCounter + "_lblSNo";
                    HtmlNode itemNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='" + _i + "']");

                    do
                    {
                        if (itemNo != null)
                        {
                            string Pos = "", CompName = "", RefNo = "";
                            itemCount++;
                            string PartNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblNumber']").InnerText.Trim();
                            if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblMakerReference']") != null)
                                RefNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblMakerReference']").InnerText.Trim();

                            if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblPosition']") != null)
                                Pos = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblPosition']").InnerText.Trim();

                            string details = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblDetails']").InnerText.Trim();
                            string Descr = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//a[@id='gvVendorItem_ctl" + strCounter + "_lnkStockItemCode']").InnerText.Trim();

                            if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblComponentName']") != null)
                                CompName = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblComponentName']").InnerText.Trim();

                            string Qty = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblOrderQuantity']").InnerText.Trim();
                            string Unit = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblunit']").InnerText.Trim();

                            #region /* item remarks url */
                            string ItemURL = "";
                            HtmlNode _details = _httpWrapper.GetElement("input", "id", "gvVendorItem_ctl" + strCounter + "_cmdDetails");
                            if (_details != null)
                            {
                                string _attDis = _details.GetAttributeValue("disabled", "");
                                if (_attDis == "")
                                {
                                    string[] _onclick = _details.GetAttributeValue("onclick", "").Split(',');
                                    if (_onclick.Length == 3)
                                    {
                                        ItemURL = OrgConfigURL + HttpUtility.HtmlDecode(_onclick[2].Split('\'')[1]);//"https://apps.southnests.com/Phoenix/Purchase/"
                                    }
                                }
                            }
                            #endregion

                            PartNo = PartNo.Replace("__.__.__", "");
                            if (PartNo.Length > 0)
                            {
                                ItemRemarks += "Number : " + PartNo;
                            }

                            if (Pos != null && Pos != "/" && Pos != "")
                            {
                                details += Environment.NewLine + "Drawing No/Position : " + convert.ToString(Pos);
                            }

                            string EquipName = "";
                            if (CompName != "")
                                EquipName = convert.ToString(CompName);

                            List<string> item = new List<string>();
                            item.Add(convert.ToString(itemNo.InnerText.Trim()));
                            item.Add(RefNo);
                            item.Add(Descr);
                            item.Add(Qty);
                            item.Add(Unit);
                            item.Add("");
                            item.Add(details);
                            item.Add(EquipName);
                            item.Add(ItemRemarks);
                            item.Add(ItemURL);
                            lstItems.Add(item);
                            ItemRemarks = "";

                            Counter++;
                            //DownloadAttachment(_xmlHeader);//commented as by sanjita,because attachment not send to any supplier.//08-03-2018
                            //  }
                        }

                        int _totalPages = 0; string _totalItems = "";
                        if (Counter.ToString().Length == 1) strCounter = "0" + Counter;
                        else strCounter = Counter.ToString();

                        if (itemCount >= _tr.Count)
                        {
                            #region Check Total Item Count
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
                                    ItemMismatchScreenShot = ImagePath + "\\Phoenix_" + this.VRNO + "_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                                    _httpWrapper.GetElement("div", "id", "navigation").Attributes["class"].Remove();
                                    _CurrentResponseString = _httpWrapper._CurrentDocument.DocumentNode.OuterHtml;
                                    if (!PrintScreen(ItemMismatchScreenShot)) ItemMismatchScreenShot = "";
                                    if (_tr.Count == lstItems.Count)
                                    {
                                        break;
                                    }
                                }
                            }
                            #endregion

                            if (_totalPages == 1 && (convert.ToInt(_totalItems) != lstItems.Count))
                            {
                                if (itemCount != (_tr.Count - 1))
                                {
                                    throw new Exception("Total Item count not matching with items in grid");
                                }
                                else break;
                            }

                            #region Move to next Grid Page
                            HtmlNode _nextPage = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//a[@id='cmdNext']");
                            if (_nextPage != null)
                            {
                                string _disNext = _nextPage.GetAttributeValue("disabled", "").Trim();
                                if (_disNext != "disabled")
                                {

                                    IsUrlEncoded = false;
                                    // throw new Exception("multiple pages found for item table.");
                                    #region multiple pages 24-07-2018
                                    URL = currentURL;
                                    dctPostDataValues.Clear();
                                    dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", _httpWrapper._dctStateData["ToolkitScriptManager1_HiddenField"]);
                                    dctPostDataValues.Add("__EVENTTARGET", "cmdNext");
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

                                    _httpWrapper.ContentType = "application/x-www-form-urlencoded";
                                    if (!_httpWrapper._SetRequestHeaders.ContainsKey(HttpRequestHeader.CacheControl))
                                        _httpWrapper._SetRequestHeaders.Add(HttpRequestHeader.CacheControl, "max-age=0");
                                    if (PostURL("a", "id", "cmdPrevious"))
                                    {

                                        _icurrpage += 1;
                                        HtmlNode pageNumber = _httpWrapper.GetElement("span", "id", "lblPagenumber");
                                        if (pageNumber.InnerText.Trim().Contains("Page"))
                                        {
                                            if (_icurrpage != convert.ToInt(pageNumber.InnerText.Trim().Split(' ')[1]))
                                            {
                                                throw new Exception("unable to move to next page " + _icurrpage);
                                            }
                                            else LogText = "Navigate to " + pageNumber.InnerText.Trim();
                                        }
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            // Reset Counter
                            Counter = 02; itemCount = 0;
                            if (Counter.ToString().Length == 1) strCounter = "0" + Counter;
                            else strCounter = Counter.ToString();
                        }
                        itemNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblSNo']");
                    }
                    while (itemNo != null);
                    IsUrlEncoded = true;

                    #region item remarks
                    if (lstItems.Count > 0)
                    {
                        foreach (List<string> _lItem in lstItems)
                        {
                            if (_lItem[9] != "")
                            {
                                URL = _lItem[9];
                                if (LoadURL("input", "id", "_content_txtItemDetails_ucCustomEditor_ctl02", true))
                                {
                                    ItemRemarks = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//input[@id='_content_txtItemDetails_ucCustomEditor_ctl02']").GetAttributeValue("value", "").Trim();
                                    ItemRemarks = ItemRemarks.Replace(@"&amp;", "&");
                                    ItemRemarks = ItemRemarks.Replace(@"&lt;", "<");
                                    ItemRemarks = ItemRemarks.Replace(@"&gt;", ">");
                                    ItemRemarks = ItemRemarks.Replace(@"&quot;", "\"");
                                    ItemRemarks = ItemRemarks.Replace(@"&apos;", "'");
                                    ItemRemarks = ItemRemarks.Replace(@"&amp;", "&");

                                    //while (ItemRemarks.Trim().Contains("&"))
                                    //{
                                    //    if (ItemRemarks.Trim().Contains("&") && !ItemRemarks.Trim().Contains(" & "))
                                    //    {
                                    //        ItemRemarks = HttpUtility.HtmlDecode(ItemRemarks.Trim());
                                    //    }
                                    //    else break;
                                    //}

                                    int matchctr = 0; ;

                                    while (ItemRemarks.Trim().Contains("&"))
                                    {
                                        if (ItemRemarks.Trim().Contains("&") && !ItemRemarks.Trim().Contains(" & "))
                                        {
                                            int count = ItemRemarks.Count(f => f == '&');

                                            ItemRemarks = HttpUtility.HtmlDecode(ItemRemarks.Trim()); matchctr++;
                                            if (count == 1) break;
                                            else if (count == matchctr) break;
                                        }
                                        else break;
                                    }

                                    if (ItemRemarks.Contains("<br />")) ItemRemarks = ItemRemarks.Replace("<br />", " ");
                                    if (ItemRemarks.Contains("<!--"))
                                        ItemRemarks = Regex.Replace(ItemRemarks, "<!--.*?-->", String.Empty, RegexOptions.Multiline);
                                    // ItemRemarks = Regex.Replace(ItemRemarks, "<.*?>", String.Empty, RegexOptions.Multiline);

                                    HtmlAgilityPack.HtmlDocument doc1 = new HtmlAgilityPack.HtmlDocument();
                                    doc1.LoadHtml(ItemRemarks);
                                    string data = convert.ToString(doc1.DocumentNode.InnerText);

                                    ItemRemarks = data.Trim();
                                    if (ItemRemarks != "")
                                    {
                                        _lItem[8] += ItemRemarks; ItemRemarks = "";
                                    }
                                }
                            }
                            else if (ItemRemarks != "") { _lItem[8] += ItemRemarks; ItemRemarks = ""; }


                        }
                    }
                    #endregion
                }
            }
            return lstItems;
        }

        private List<List<string>> GetRFQItems_New(Dictionary<string, string> _xmlHeader)
        {
            int _icurrpage = 1;
            string ItemRemarks = "";
            List<List<string>> lstItems = new List<List<string>>();
            HtmlNodeCollection _tr = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='gvVendorItem_ctl00']/tbody/tr");
            if (_tr != null)
            {
                if (_tr.Count > 0)
                {
                    int Counter = 4, itemCount = 0;
                    string strCounter = "";//gvVendorItem_ctl00_ctl04_lblSNo
                    if (Counter < _tr.Count + 1) strCounter = "_ctl" + "0" + Counter;
                    else if (Counter > 10) strCounter = "_ctl"+ Counter.ToString();
                    string _i = "gvVendorItem_ctl00" + strCounter + "_lblSNo";
                    HtmlNode itemNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='" + _i + "']");

                    do
                    {
                        if (itemNo != null)
                        {
                            string Pos = "", CompName = "", RefNo = "";
                            itemCount++;
                            string PartNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl00" + strCounter + "_lblNumber']").InnerText.Trim();
                            if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl00" + strCounter + "_lblMakerReference']") != null)
                                RefNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl00" + strCounter + "_lblMakerReference']").InnerText.Trim();

                            if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl00" + strCounter + "_lblPosition']") != null)
                                Pos = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl00" + strCounter + "_lblPosition']").InnerText.Trim();

                            string details = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl00" + strCounter + "_lblDetails']").InnerText.Trim();
                            string Descr = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//a[@id='gvVendorItem_ctl00" + strCounter + "_lnkStockItemCode']").InnerText.Trim();

                            if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl00" + strCounter + "_lblComponentName']") != null)
                                CompName = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl00" + strCounter + "_lblComponentName']").InnerText.Trim();

                            string Qty = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl00" + strCounter + "_lblOrderQuantity']").InnerText.Trim();
                            string Unit = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl00" + strCounter + "_Label2']").InnerText.Trim();

                            #region /* item remarks url */
                            string ItemURL = "";
                            //gvVendorItem_ctl00_ctl04_cmdDetail
                            HtmlNode _details = _httpWrapper.GetElement("a", "id", "gvVendorItem_ctl00" + strCounter + "_cmdDetail");
                            if (_details != null)
                            {
                                string _attDis = _details.GetAttributeValue("disabled", "");
                                if (_attDis == "")
                                {
                                    string[] _onclick = _details.GetAttributeValue("onclick", "").Split(',');
                                    if (_onclick.Length == 3)
                                    {
                                        string val = RemoveCharacters_ItemRmk_New(_onclick[2]);
                                        ItemURL = "https://apps2.southnests.com/PhoenixTelerik/" + HttpUtility.HtmlDecode(val);
                                    }
                                }
                            }
                            #endregion

                            PartNo = PartNo.Replace("__.__.__", "");
                            if (PartNo.Length > 0)
                            {
                                ItemRemarks += "Number : " + PartNo;
                            }

                            if (Pos != null && Pos != "/" && Pos != "")
                            {
                                details += Environment.NewLine + "Drawing No/Position : " + convert.ToString(Pos);
                            }

                            string EquipName = "";
                            if (CompName != "")
                                EquipName = convert.ToString(CompName);

                            List<string> item = new List<string>();
                            item.Add(convert.ToString(itemNo.InnerText.Trim()));
                            item.Add(RefNo);
                            item.Add(Descr);
                            item.Add(Qty);
                            item.Add(Unit);
                            item.Add("");
                            item.Add(details);
                            item.Add(EquipName);
                            item.Add(ItemRemarks);
                            item.Add(ItemURL);
                            lstItems.Add(item);
                            ItemRemarks = "";

                            Counter++;
                            //DownloadAttachment(_xmlHeader);//commented as by sanjita,because attachment not send to any supplier.//08-03-2018
                            //  }
                        }

                        int _totalPages = 0; string _totalItems = "";
                        if (Counter.ToString().Length == 1) strCounter = "_ctl"+ "0" + Counter;
                        else strCounter = "_ctl" + Counter.ToString();

                        if (itemCount >= _tr.Count)
                        {
                            #region Check Total Item Count
                            if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//div[@class='rgWrap rgInfoPart']") != null)
                            {
                                _totalItems = convert.ToString(_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//div[@class='rgWrap rgInfoPart']").InnerText.Trim()).Trim().Replace("(", "").Replace(")", "").Replace(" Items matching your search criteria", "").Trim();
                                _totalPages = convert.ToInt(convert.ToString(_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl00_ctl02_ctl00_PageOfLabel']").InnerText.Trim()).Trim().Replace("of", "").Replace("Pages.", "").Trim());

                                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsItemCountMismatch"].Trim()) == false)
                                {
                                    if (convert.ToInt(_totalItems) == lstItems.Count)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    ItemMismatchScreenShot = ImagePath + "\\Phoenix_" + this.VRNO + "_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                                    _httpWrapper.GetElement("div", "id", "navigation").Attributes["class"].Remove();
                                    _CurrentResponseString = _httpWrapper._CurrentDocument.DocumentNode.OuterHtml;
                                    if (!PrintScreen(ItemMismatchScreenShot)) ItemMismatchScreenShot = "";
                                    if (_tr.Count == lstItems.Count)
                                    {
                                        break;
                                    }
                                }
                            }
                            #endregion

                            if (_totalPages == 1 && (convert.ToInt(_totalItems) != lstItems.Count))
                            {
                                if (itemCount != (_tr.Count - 1))
                                {
                                    throw new Exception("Total Item count not matching with items in grid");
                                }
                                else break;
                            }

                            #region Move to next Grid Page
                            HtmlNode _nextPage = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//button[@title='Next Page']");
                            if (_nextPage != null)
                            {
                                string _disNext = _nextPage.GetAttributeValue("disabled", "").Trim();
                                string Nxtpagedet = _nextPage.GetAttributeValue("name", "").Trim();
                                if (_disNext != "disabled")
                                {
                                    IsUrlEncoded = false;
                                    // throw new Exception("multiple pages found for item table.");
                                    #region multiple pages 24-07-2018
                                    URL = currentURL;
                                    dctPostDataValues.Clear();
                                    dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", _httpWrapper._dctStateData["RadScriptManager1_TSM"]);
                                    dctPostDataValues.Add("__EVENTTARGET", Nxtpagedet);//gvVendorItem$ctl00$ctl02$ctl00$ctl09
                                    dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
                                    dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                                    dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                                    dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);
                                    dctPostDataValues.Add("__SCROLLPOSITIONX", _httpWrapper._dctStateData["__SCROLLPOSITIONX"]);
                                    dctPostDataValues.Add("__SCROLLPOSITIONY", _httpWrapper._dctStateData["__SCROLLPOSITIONY"]);

                                    dctPostDataValues.Add("RadWindowManager1_ClientState", "");
                                    dctPostDataValues.Add("MenuQuotationLineItem_dlstTabs_ClientState", "");


                                    dctPostDataValues.Add("txtVessel", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtVessel").GetAttributeValue("value", "").Trim()));
                                    dctPostDataValues.Add("hndVesselID", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "hndVesselID").GetAttributeValue("value", "").Trim()));
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
                                    
                                    dctPostDataValues.Add("txtComponentName", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtComponentName").GetAttributeValue("value", "").Trim()));
                                    
                                    dctPostDataValues.Add("txtComponentModel", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtComponentModel").GetAttributeValue("value", "").Trim()));
                                    
                                    dctPostDataValues.Add("txtComponentSerialNo", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtComponentSerialNo").GetAttributeValue("value", "").Trim()));
                                   
                                    dctPostDataValues.Add("txtVendorName", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtVendorName").GetAttributeValue("value", "").Trim()));
                                  
                                    dctPostDataValues.Add("txtTelephone", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtTelephone").GetAttributeValue("value", "").Trim()));
                                   
                                    dctPostDataValues.Add("txtFax", WebUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtFax").GetAttributeValue("value", "").Trim()));
                                    
                                    dctPostDataValues.Add("txtVendorAddress", WebUtility.UrlEncode(_httpWrapper.GetElement("textarea", "id", "txtVendorAddress").InnerText.Trim()));
                                    
                                    dctPostDataValues.Add("txtEmail", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtEmail").GetAttributeValue("value", "").Trim()));
                                    
                                    dctPostDataValues.Add("txtVenderReference", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVenderReference").GetAttributeValue("value", "").Trim()));
                                   
                                    //  dctPostDataValues.Add("txtOrderDate$txtDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtOrderDate$txtDate").GetAttributeValue("value", "").Trim()));
                                    //  dctPostDataValues.Add("txtOrderDate_txtDate_dateInput_ClientState", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtOrderDate_txtDate_dateInput_ClientState").GetAttributeValue("value", "").Trim()));
                                    dctPostDataValues.Add("txtOrderDate_txtDate_dateInput_ClientState", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtOrderDate_txtDate_dateInput_ClientState").GetAttributeValue("value", "").Trim()));

                                    //dctPostDataValues.Add("txtExpirationDate$txtDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtExpirationDate$txtDate").GetAttributeValue("value", "").Trim()));
                                    dctPostDataValues.Add("txtExpirationDate_txtDate_dateInput_ClientState", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtExpirationDate_txtDate_dateInput_ClientState").GetAttributeValue("value", "").Trim()));

                                    dctPostDataValues.Add("txtPriority", _httpWrapper.GetElement("input", "id", "txtPriority").GetAttributeValue("value", "").Trim());

                                    dctPostDataValues.Add("UCDeliveryTerms_ddlQuick_Input", _httpWrapper.GetElement("input", "id", "UCDeliveryTerms_ddlQuick_Input").GetAttributeValue("value", "").Trim());
                                 //   dctPostDataValues.Add("UCDeliveryTerms_ddlQuick_ClientState", _httpWrapper.GetElement("input", "id", "UCDeliveryTerms_ddlQuick_ClientState").GetAttributeValue("value", "").Trim());

                                    dctPostDataValues.Add("UCPaymentTerms_ddlQuick_Input", _httpWrapper.GetElement("input", "id", "UCPaymentTerms_ddlQuick_Input").GetAttributeValue("value", "").Trim());
                                  //  dctPostDataValues.Add("UCPaymentTerms_ddlQuick_ClientState", _httpWrapper.GetElement("input", "id", "UCPaymentTerms_ddlQuick_ClientState").GetAttributeValue("value", "").Trim());

                                    dctPostDataValues.Add("ucModeOfTransport_ddlQuick_Input", _httpWrapper.GetElement("input", "id", "ucModeOfTransport_ddlQuick_Input").GetAttributeValue("value", "").Trim());
                                 //   dctPostDataValues.Add("ucModeOfTransport_ddlQuick_ClientState", _httpWrapper.GetElement("input", "id", "ucModeOfTransport_ddlQuick_ClientState").GetAttributeValue("value", "").Trim());

                                    dctPostDataValues.Add("ddlType_ddlHard_Input", _httpWrapper.GetElement("input", "id", "ddlType_ddlHard_Input").GetAttributeValue("value", "").Trim());
                                   // dctPostDataValues.Add("ddlType_ddlHard_ClientState", _httpWrapper.GetElement("input", "id", "ddlType_ddlHard_ClientState").GetAttributeValue("value", "").Trim());

                                    dctPostDataValues.Add("txtDeliveryTime_txtNumber", _httpWrapper.GetElement("input", "id", "txtDeliveryTime_txtNumber").GetAttributeValue("value", "").Trim());

                                    dctPostDataValues.Add("ucCurrency_ddlCurrency_Input", _httpWrapper.GetElement("input", "id", "ucCurrency_ddlCurrency_Input").GetAttributeValue("value", "").Trim());
                                  //  dctPostDataValues.Add("ucCurrency_ddlCurrency_ClientState", _httpWrapper.GetElement("input", "id", "ucCurrency_ddlCurrency_ClientState").GetAttributeValue("value", "").Trim());

                                    dctPostDataValues.Add("txtSupplierDiscount_txtDecimal", _httpWrapper.GetElement("input", "id", "txtSupplierDiscount_txtDecimal").GetAttributeValue("value", "").Trim());
                                
                                    dctPostDataValues.Add("txtPrice", _httpWrapper.GetElement("input", "id", "txtPrice").GetAttributeValue("value", "").Trim());
                                    dctPostDataValues.Add("txtTotalDiscount", _httpWrapper.GetElement("input", "id", "txtTotalDiscount").GetAttributeValue("value", "").Trim());                                 
                                    dctPostDataValues.Add("txtTotalPrice", _httpWrapper.GetElement("input", "id", "txtTotalPrice").GetAttributeValue("value", "").Trim());
                                    dctPostDataValues.Add("txtDiscount_txtDecimal", _httpWrapper.GetElement("input", "id", "txtDiscount_txtDecimal").GetAttributeValue("value", "").Trim());
                                    
                                    dctPostDataValues.Add("RadFormDecorator1_ClientState", "");

                                    dctPostDataValues.Add("gvTax$ctl00$ctl03$ctl00$txtDescriptionAdd", "");

                                    dctPostDataValues.Add("gvTax%24ctl03%24ucTaxTypeAdd%24rblValuePercentage", "2");
                                    dctPostDataValues.Add("gvTax$ctl00$ctl03$ctl00$txtValueAdd$txtDecimal", "");//
                                  
                                    dctPostDataValues.Add("gvVendorItem_ClientState", "");
                                    dctPostDataValues.Add("RadCommonToolTipManager1_ClientState", "");

                                    #region  commented
                                 //   dctPostDataValues.Add("txtVessel", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtVessel").GetAttributeValue("value", "").Trim()));
                                 //   dctPostDataValues.Add("txtVessel_ClientState", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtVessel_ClientState").GetAttributeValue("value", "").Trim()));
                                 //   dctPostDataValues.Add("hndVesselID", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "hndVesselID").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtIMONo", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtIMONo").GetAttributeValue("value", "").Trim()));
                                 //  // dctPostDataValues.Add("txtIMONo_ClientState", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtIMONo_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtHullNo", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtHullNo").GetAttributeValue("value", "").Trim()));
                                 // //  dctPostDataValues.Add("txtHullNo_ClientState", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtHullNo_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtVesseltype", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtVesseltype").GetAttributeValue("value", "").Trim()));
                                 // //  dctPostDataValues.Add("txtVesseltype_ClientState", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtVesseltype_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtYard", WebUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtYard").GetAttributeValue("value", "").Trim()));
                                 //   //dctPostDataValues.Add("txtYard_ClientState", WebUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtYard_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtYearBuilt", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtYearBuilt").GetAttributeValue("value", "").Trim()));
                                 //  // dctPostDataValues.Add("txtYearBuilt_ClientState", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtYearBuilt_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtSenderName", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtSenderName").GetAttributeValue("value", "").Trim()));
                                 //  // dctPostDataValues.Add("txtSenderName_ClientState", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtSenderName_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtDeliveryPlace", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtDeliveryPlace").GetAttributeValue("value", "").Trim()));
                                 // //  dctPostDataValues.Add("txtDeliveryPlace_ClientState", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtDeliveryPlace_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtContactNo", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtContactNo").GetAttributeValue("value", "").Trim()));
                                 // //  dctPostDataValues.Add("txtContactNo_ClientState", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtContactNo_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtSenderEmailId", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtSenderEmailId").GetAttributeValue("value", "").Trim()));
                                 // //  dctPostDataValues.Add("txtSenderEmailId_ClientState", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtSenderEmailId_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtPortName", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtPortName").GetAttributeValue("value", "").Trim()));
                                 // //  dctPostDataValues.Add("txtPortName_ClientState", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtPortName_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtSentDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSentDate").GetAttributeValue("value", "").Trim()));
                                 // //  dctPostDataValues.Add("txtSentDate_ClientState", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSentDate_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtComponentName", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtComponentName").GetAttributeValue("value", "").Trim()));
                                 //  // dctPostDataValues.Add("txtComponentName_ClientState", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtComponentName_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtComponentModel", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtComponentModel").GetAttributeValue("value", "").Trim()));
                                 // //  dctPostDataValues.Add("txtComponentModel_ClientState", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtComponentModel_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtComponentSerialNo", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtComponentSerialNo").GetAttributeValue("value", "").Trim()));
                                 //  // dctPostDataValues.Add("txtComponentSerialNo_ClientState", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtComponentSerialNo_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtVendorName", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtVendorName").GetAttributeValue("value", "").Trim()));
                                 //  // dctPostDataValues.Add("txtVendorName_ClientState", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtVendorName_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtTelephone", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtTelephone").GetAttributeValue("value", "").Trim()));
                                 // //  dctPostDataValues.Add("txtTelephone_ClientState", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtTelephone_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtFax", WebUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtFax").GetAttributeValue("value", "").Trim()));
                                 //  /// dctPostDataValues.Add("txtFax_ClientState", WebUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtFax_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtVendorAddress", WebUtility.UrlEncode(_httpWrapper.GetElement("textarea", "id", "txtVendorAddress").InnerText.Trim()));
                                 // //  dctPostDataValues.Add("txtVendorAddress_ClientState", WebUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtVendorAddress_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtEmail", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtEmail").GetAttributeValue("value", "").Trim()));
                                 //  // dctPostDataValues.Add("txtEmail_ClientState", HttpUtility.UrlEncode(_httpWrapper.GetElement("input", "id", "txtEmail_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtVenderReference", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVenderReference").GetAttributeValue("value", "").Trim()));
                                 ////   dctPostDataValues.Add("txtVenderReference_ClientState", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVenderReference_ClientState").GetAttributeValue("value", "").Trim()));

                                 // //  dctPostDataValues.Add("txtOrderDate$txtDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtOrderDate$txtDate").GetAttributeValue("value", "").Trim()));
                                 // //  dctPostDataValues.Add("txtOrderDate_txtDate_dateInput_ClientState", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtOrderDate_txtDate_dateInput_ClientState").GetAttributeValue("value", "").Trim()));
                                 //   dctPostDataValues.Add("txtOrderDate_txtDate_dateInput_ClientState", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtOrderDate_txtDate_dateInput_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   //dctPostDataValues.Add("txtExpirationDate$txtDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtExpirationDate$txtDate").GetAttributeValue("value", "").Trim()));
                                 //   dctPostDataValues.Add("txtExpirationDate_txtDate_dateInput_ClientState", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtExpirationDate_txtDate_dateInput_ClientState").GetAttributeValue("value", "").Trim()));

                                 //   dctPostDataValues.Add("txtPriority", _httpWrapper.GetElement("input", "id", "txtPriority").GetAttributeValue("value", "").Trim());
                                 //   //dctPostDataValues.Add("txtPriority_ClientState", _httpWrapper.GetElement("input", "id", "txtPriority_ClientState").GetAttributeValue("value", "").Trim());

                                 //  // dctPostDataValues.Add("UCDeliveryTerms$ddlQuick", _httpWrapper.GetElement("input", "id", "UCDeliveryTerms$ddlQuick").GetAttributeValue("value", "").Trim());
                                 //  // dctPostDataValues.Add("UCDeliveryTerms_ddlQuick_ClientState", _httpWrapper.GetElement("input", "id", "UCDeliveryTerms_ddlQuick_ClientState").GetAttributeValue("value", "").Trim());

                                 // //  dctPostDataValues.Add("UCPaymentTerms$ddlQuick", _httpWrapper.GetElement("input", "id", "UCPaymentTerms$ddlQuick").GetAttributeValue("value", "").Trim());
                                 //  // dctPostDataValues.Add("UCPaymentTerms_ddlQuick_ClientState", _httpWrapper.GetElement("input", "id", "UCPaymentTerms_ddlQuick_ClientState").GetAttributeValue("value", "").Trim());

                                 // //  dctPostDataValues.Add("ucModeOfTransport$ddlQuick", _httpWrapper.GetElement("input", "id", "ucModeOfTransport$ddlQuick").GetAttributeValue("value", "").Trim());
                                 //  // dctPostDataValues.Add("ucModeOfTransport_ddlQuick_ClientState", _httpWrapper.GetElement("input", "id", "ucModeOfTransport_ddlQuick_ClientState").GetAttributeValue("value", "").Trim());

                                 //  // dctPostDataValues.Add("ddlType%24ddlHard", _httpWrapper.GetElement("input", "id", "ddlType$ddlHard").GetAttributeValue("value", "").Trim());
                                 //   //dctPostDataValues.Add("ddlType_ddlHard_ClientState", _httpWrapper.GetElement("input", "id", "ddlType_ddlHard_ClientState").GetAttributeValue("value", "").Trim());

                                 //  // dctPostDataValues.Add("txtDeliveryTime$txtNumber", _httpWrapper.GetElement("input", "id", "txtDeliveryTime$txtNumber").GetAttributeValue("value", "").Trim());
                                 // //  dctPostDataValues.Add("txtDeliveryTime_txtNumber_ClientState", _httpWrapper.GetElement("input", "id", "txtDeliveryTime_txtNumber_ClientState").GetAttributeValue("value", "").Trim());

                                 //  // dctPostDataValues.Add("ucCurrency$ddlCurrency", _httpWrapper.GetElement("input", "id", "ucCurrency$ddlCurrency").GetAttributeValue("value", "").Trim());
                                 //  // dctPostDataValues.Add("ucCurrency_ddlCurrency_ClientState", _httpWrapper.GetElement("input", "id", "ucCurrency_ddlCurrency_ClientState").GetAttributeValue("value", "").Trim());

                                 //  // dctPostDataValues.Add("txtSupplierDiscount$txtDecimal", _httpWrapper.GetElement("input", "id", "txtSupplierDiscount$txtDecimal").GetAttributeValue("value", "").Trim());
                                 // //  dctPostDataValues.Add("txtSupplierDiscount_txtDecimal_ClientState", _httpWrapper.GetElement("input", "id", "txtSupplierDiscount_txtDecimal_ClientState").GetAttributeValue("value", "").Trim());

                                 //   dctPostDataValues.Add("txtPrice", _httpWrapper.GetElement("input", "id", "txtPrice").GetAttributeValue("value", "").Trim());
                                 // //  dctPostDataValues.Add("txtPrice_ClientState", _httpWrapper.GetElement("input", "id", "txtPrice_ClientState").GetAttributeValue("value", "").Trim());

                                 //   dctPostDataValues.Add("txtTotalDiscount", _httpWrapper.GetElement("input", "id", "txtTotalDiscount").GetAttributeValue("value", "").Trim());
                                 // //  dctPostDataValues.Add("txtTotalDiscount_ClientState", _httpWrapper.GetElement("input", "id", "txtTotalDiscount_ClientState").GetAttributeValue("value", "").Trim());

                                 //   dctPostDataValues.Add("txtTotalPrice", _httpWrapper.GetElement("input", "id", "txtTotalPrice").GetAttributeValue("value", "").Trim());
                                 ////   dctPostDataValues.Add("txtTotalPrice_ClientState", _httpWrapper.GetElement("input", "id", "txtTotalPrice_ClientState").GetAttributeValue("value", "").Trim());

                                 //  // dctPostDataValues.Add("txtDiscount$txtDecimal", _httpWrapper.GetElement("input", "id", "txtDiscount$txtDecimal").GetAttributeValue("value", "").Trim());
                                 //  // dctPostDataValues.Add("txtDiscount_txtDecimal_ClientState", _httpWrapper.GetElement("input", "id", "txtDiscount_txtDecimal_ClientState").GetAttributeValue("value", "").Trim());
                                 //  // dctPostDataValues.Add("RadFormDecorator1_ClientState", "");

                                 //   dctPostDataValues.Add("gvTax$ctl00$ctl03$ctl00$txtDescriptionAdd", "");
                                 //   //dctPostDataValues.Add("gvTax_ctl00_ctl03_ctl00_txtDescriptionAdd_ClientState", "");

                                 //   dctPostDataValues.Add("gvTax%24ctl03%24ucTaxTypeAdd%24rblValuePercentage", "2");
                                 //   dctPostDataValues.Add("gvTax$ctl00$ctl03$ctl00$txtValueAdd$txtDecimal", "");//
                                 //  // dctPostDataValues.Add("gvTax_ctl00_ctl03_ctl00_txtValueAdd_txtDecimal_ClientState", "");
                                 //   dctPostDataValues.Add("gvVendorItem_ClientState", "");

#endregion

                                    
                                    _httpWrapper.ContentType = "application/x-www-form-urlencoded";
                                    if (!_httpWrapper._SetRequestHeaders.ContainsKey(HttpRequestHeader.CacheControl))
                                        _httpWrapper._SetRequestHeaders.Add(HttpRequestHeader.CacheControl, "max-age=0");
                                    if (PostURL("button", "title", "Previous Page"))
                                    {

                                        _icurrpage += 1;
                                        HtmlNode pageNumber = _httpWrapper.GetElement("input", "id", "gvVendorItem_ctl00_ctl02_ctl00_GoToPageTextBox");//div
                                        if (!string.IsNullOrEmpty(pageNumber.GetAttributeValue("value","").Trim()))//    if (pageNumber.InnerText.Trim().Contains("Page"))
                                        {
                                            if (_icurrpage != convert.ToInt(pageNumber.GetAttributeValue("value", "")))
                                            {
                                              //  throw new Exception("unable to move to next page " + _icurrpage);
                                            }
                                            else LogText = "Navigate to " + pageNumber.GetAttributeValue("value", "").Trim();
                                        }
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            // Reset Counter
                            Counter = 04; itemCount = 0;
                            if (Counter.ToString().Length == 1) strCounter = "_ctl" + "0" + Counter;
                            else strCounter = "_ctl" + Counter.ToString();
                        }
                        itemNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl00" + strCounter + "_lblSNo']");
                        ////span[@id='gvVendorItem_ctl00"
                    }
                    while (itemNo != null);
                    IsUrlEncoded = true;

                    #region item remarks
                    if (lstItems.Count > 0)
                    {
                        foreach (List<string> _lItem in lstItems)
                        {
                            if (_lItem[9] != "")
                            {
                                URL = _lItem[9];
                                if (LoadURL("div", "id", "txtItemDetails", true))
                                {
                                    ItemRemarks = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//textarea[@id='txtItemDetailsContentHiddenTextarea']").InnerText.Trim();
                                    ItemRemarks = ItemRemarks.Replace(@"&amp;", "&");
                                    ItemRemarks = ItemRemarks.Replace(@"&lt;", "<");
                                    ItemRemarks = ItemRemarks.Replace(@"&gt;", ">");
                                    ItemRemarks = ItemRemarks.Replace(@"&quot;", "\"");
                                    ItemRemarks = ItemRemarks.Replace(@"&apos;", "'");
                                    ItemRemarks = ItemRemarks.Replace(@"&amp;", "&");

                                    //while (ItemRemarks.Trim().Contains("&"))
                                    //{
                                    //    if (ItemRemarks.Trim().Contains("&") && !ItemRemarks.Trim().Contains(" & "))
                                    //    {
                                    //        ItemRemarks = HttpUtility.HtmlDecode(ItemRemarks.Trim());
                                    //    }
                                    //    else break;
                                    //}

                                    int matchctr = 0; ;

                                    while (ItemRemarks.Trim().Contains("&"))
                                    {
                                        if (ItemRemarks.Trim().Contains("&") && !ItemRemarks.Trim().Contains(" & "))
                                        {
                                            int count = ItemRemarks.Count(f => f == '&');

                                            ItemRemarks = HttpUtility.HtmlDecode(ItemRemarks.Trim()); matchctr++;
                                            if (count == 1) break;
                                            else if (count == matchctr) break;
                                        }
                                        else break;
                                    }

                                    if (ItemRemarks.Contains("<br />")) ItemRemarks = ItemRemarks.Replace("<br />", " ");
                                    if (ItemRemarks.Contains("<!--"))
                                        ItemRemarks = Regex.Replace(ItemRemarks, "<!--.*?-->", String.Empty, RegexOptions.Multiline);
                                    // ItemRemarks = Regex.Replace(ItemRemarks, "<.*?>", String.Empty, RegexOptions.Multiline);

                                    HtmlAgilityPack.HtmlDocument doc1 = new HtmlAgilityPack.HtmlDocument();
                                    doc1.LoadHtml(ItemRemarks);
                                    string data = convert.ToString(doc1.DocumentNode.InnerText);

                                    ItemRemarks = data.Trim();
                                    if (ItemRemarks != "")
                                    {
                                        _lItem[8] += ItemRemarks; ItemRemarks = "";
                                    }
                                }
                            }
                            else if (ItemRemarks != "") { _lItem[8] += ItemRemarks; ItemRemarks = ""; }


                        }
                    }
                    #endregion
                }
            }
            return lstItems;
        }

        static void RemoveComments(HtmlNode node)
        {
            if (!node.HasChildNodes)
            {
                return;
            }

            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].NodeType == HtmlNodeType.Comment)
                {
                    node.ChildNodes.RemoveAt(i);
                    --i;
                }
            }

            foreach (HtmlNode subNode in node.ChildNodes)
            {
                RemoveComments(subNode);
            }
        }

        private static string RemoveCharacters_ItemRmk(string ItemDetails)
        {
            string _res = "";
            string[] _arr = ItemDetails.Replace("&#39;", "").Replace("&amp;", "").Replace("../", "").Trim().Split(';');
            if (_arr != null && _arr.Length > 0)
            {
                _res = _arr[0];
            }
            return _res;
        }

        private static string RemoveCharacters_ItemRmk_New(string ItemDetails)
        {
            string _res = "";
            string[] _arr = ItemDetails.Replace("&#39;", "").Replace("&amp;", "&").Replace("../", "").Trim().Split(';');
            if (_arr != null && _arr.Length > 0)
            {
                _res = _arr[0].Replace(")","");
            }
            return _res;
        }


        #endregion

        #region PO
        private bool GetPODetails(string URL)
        {
            bool result = false;
            try
            {
                LogText = "Navigating To URL ..";

                if (!string.IsNullOrEmpty(URL)) { IsLink_NewVersion = (URL.Contains("PhoenixTelerik")) ? true : false; }//added by Kalpita on 16/12/2019
                this.URL = URL.Trim().Replace("PurchaseVendorRemark.aspx", "PurchaseQuotationRFQ.aspx");
                _httpWrapper.Referrer = URL.Trim().Replace("PurchaseVendorRemark.aspx", "PurchaseQuotationItems.aspx");
                _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
                _httpWrapper._SetRequestHeaders[HttpRequestHeader.AcceptLanguage] = "en-IN";
                _httpWrapper.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                _httpWrapper._SetRequestHeaders[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                _httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.CacheControl);
                //_httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");
                _httpWrapper._AddRequestHeaders["DNT"] = @"1";
                _httpWrapper.ContentType = "";

                if (LoadURL("input", "id", "txtnopage", true))
                {
                    #region // Search PO PDF //
                    DirectoryInfo _dirFiles = new DirectoryInfo(LinkPath);
                    if (_dirFiles.Exists)
                    {
                        FileInfo[] _pdfFiles = _dirFiles.GetFiles("*.pdf");
                        foreach (FileInfo _pdf in _pdfFiles)
                        {
                            if (_pdf.Name.Contains(this.VRNO) && _pdf.Name.Contains(Path.GetFileNameWithoutExtension(this.orgEmlFile)))
                            {
                                if (CheckInvalidPDF(_pdf))
                                {
                                    _orgDocFile = _pdf;
                                    break;
                                }
                                else
                                {
                                    try
                                    {
                                        if (!Directory.Exists(_pdf.Directory.FullName + "\\InvalidFiles"))
                                            Directory.CreateDirectory(_pdf.Directory.FullName + "\\InvalidFiles");
                                        _pdf.MoveTo(_pdf.Directory.FullName + "\\InvalidFiles");
                                    }
                                    catch { LogText = "Unable to move " + _pdf.Name + " to invalidFiles"; }
                                }
                            }
                        }
                    }
                    #endregion

                    #region // Research file in InvalidFiles folder //
                    if (_orgDocFile == null)
                    {
                        string InvalidFilesPath = LinkPath + "\\InvalidFiles";
                        DirectoryInfo _dirInvalidFiles = new DirectoryInfo(InvalidFilesPath);
                        if (_dirInvalidFiles.Exists)
                        {
                            FileInfo[] _pdfFiles = _dirInvalidFiles.GetFiles("*" + this.VRNO + "*.pdf");
                            foreach (FileInfo _pdf in _pdfFiles)
                            {
                                if (_pdf.Name.Contains(this.VRNO) && _pdf.Name.Contains(Path.GetFileNameWithoutExtension(this.orgEmlFile)))
                                {
                                    if (CheckInvalidPDF(_pdf))
                                    {
                                        _pdf.CopyTo(LinkPath + "\\" + _pdf.Name, true);
                                        _orgDocFile = new FileInfo(LinkPath + "\\" + _pdf.Name);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    #endregion


                    this.PageGUID = URL.Trim();
                    //  _orgDocFile = new FileInfo("KMT-19-0096SUP-00000220191113.pdf");//testing
                    FetchData();

                    if (File.Exists(currentXMLFile)) result = true;
                    else throw new Exception("Unable to convert Order PDF");

                    #region  /* Commented By Sanjita (12-DEC-18) for PDF Order file conversion */

                    /*
                    if (File.Exists(currentXMLFile)) result = true;
                    else
                    {
                        //added for PO
                        LeSXML.LeSXML _lesXML = new LeSXML.LeSXML();

                        string VRNO = "";
                        HtmlNode _title = _httpWrapper.GetElement("span", "id", "Title1_lblTitle");
                        if (_title != null)
                        {
                            int vrnoIndx = convert.ToString(_title.InnerText).Trim().IndexOf('[');
                            if (vrnoIndx > 0)
                            {
                                VRNO = convert.ToString(_title.InnerText).Substring(vrnoIndx).Trim();
                                VRNO = VRNO.Replace("[", "").Replace("]", "").Trim();
                            }
                        }

                        Dictionary<string, string> _xmlHeader = this.GetOrderDetails(VRNO, _orgDocFile.FullName, pdfBuyerEmail.Trim(), VendorEmail.Trim());

                        _xmlItems = this.GetOrderItems(_orgDocFile.FullName, ref _xmlItems);
                        _lesXML.LeadTimeDays = "";
                        _lesXML.DocLinkID = URL.Trim();
                        _lesXML.LineItems.Clear();
                        _lesXML.Addresses.Clear();

                        if (SupplierCode.Trim() == "")
                        {
                            string email = convert.ToString(_xmlHeader["VENDOR_EMAIL"]);
                            if (email.Trim().Length > 0)
                            {
                                int indx = email.Split(',')[0].Trim().IndexOf('@');
                                if (indx > 0)
                                {
                                    string domain = email.Split(',')[0].Substring(indx);
                                    if (lstSupp.ContainsKey(domain.Trim().ToLower()))
                                    {
                                        SupplierCode = convert.ToString(lstSupp[domain.Trim().ToLower()]);
                                    }
                                }
                            }
                            else
                            {
                                errLog = "Unable to get supplier code.";
                                throw new Exception("Unable to get supplier code.");
                            }
                        }

                        ExporttoHeader(_xmlHeader, _lesXML);
                        _lesXML.WriteXML();

                        if (File.Exists(currentXMLFile)) result = true;
                        else result = false;
                    }
                    */
                    #endregion
                }
                else
                {
                    if (LoadURL("input", "id", "txtVessel", true))
                    {
                        #region // Search PO PDF //
                        DirectoryInfo _dirFiles = new DirectoryInfo(LinkPath);
                        if (_dirFiles.Exists)
                        {
                            FileInfo[] _pdfFiles = _dirFiles.GetFiles("*.pdf");
                            foreach (FileInfo _pdf in _pdfFiles)
                            {
                                if (_pdf.Name.Contains(this.VRNO) && _pdf.Name.Contains(Path.GetFileNameWithoutExtension(this.orgEmlFile)))
                                {
                                    if (CheckInvalidPDF(_pdf))
                                    {
                                        _orgDocFile = _pdf;
                                        break;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (!Directory.Exists(_pdf.Directory.FullName + "\\InvalidFiles"))
                                                Directory.CreateDirectory(_pdf.Directory.FullName + "\\InvalidFiles");
                                            _pdf.MoveTo(_pdf.Directory.FullName + "\\InvalidFiles");
                                        }
                                        catch { LogText = "Unable to move " + _pdf.Name + " to invalidFiles"; }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region // Research file in InvalidFiles folder //
                        if (_orgDocFile == null)
                        {
                            string InvalidFilesPath = LinkPath + "\\InvalidFiles";
                            DirectoryInfo _dirInvalidFiles = new DirectoryInfo(InvalidFilesPath);
                            if (_dirInvalidFiles.Exists)
                            {
                                FileInfo[] _pdfFiles = _dirInvalidFiles.GetFiles("*" + this.VRNO + "*.pdf");
                                foreach (FileInfo _pdf in _pdfFiles)
                                {
                                    if (_pdf.Name.Contains(this.VRNO) && _pdf.Name.Contains(Path.GetFileNameWithoutExtension(this.orgEmlFile)))
                                    {
                                        if (CheckInvalidPDF(_pdf))
                                        {
                                            _pdf.CopyTo(LinkPath + "\\" + _pdf.Name, true);
                                            _orgDocFile = new FileInfo(LinkPath + "\\" + _pdf.Name);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion


                        this.PageGUID = URL.Trim();
                        //  _orgDocFile = new FileInfo("KMT-19-0096SUP-00000220191113.pdf");//testing
                        FetchData();

                        if (File.Exists(currentXMLFile)) result = true;
                        else throw new Exception("Unable to convert Order PDF");
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

        private bool GetPODetails_No_Url()
        {
            bool result = false;
            try
            {
                LoadURL("", "", "");
                this.URL = _httpWrapper._CurrentResponse.ResponseUri.ToString();

                DirectoryInfo _dirFiles = new DirectoryInfo(LinkPath);
                if (_dirFiles.Exists)
                {
                    FileInfo[] _pdfFiles = _dirFiles.GetFiles("*.pdf");
                    foreach (FileInfo _pdf in _pdfFiles)
                    {
                        if (_pdf.Name.Contains(this.VRNO) && _pdf.Name.Contains(Path.GetFileNameWithoutExtension(this.orgEmlFile)))
                        {
                            if (CheckInvalidPDF(_pdf))
                            {
                                _orgDocFile = _pdf;
                                break;
                            }
                            else
                            {
                                try
                                {
                                    if (!Directory.Exists(_pdf.Directory.FullName + "\\InvalidFiles"))
                                        Directory.CreateDirectory(_pdf.Directory.FullName + "\\InvalidFiles");
                                    _pdf.MoveTo(_pdf.Directory.FullName + "\\InvalidFiles");
                                }
                                catch { LogText = "Unable to move " + _pdf.Name + " to invalidFiles"; }
                            }
                        }
                    }
                }
        #endregion

                #region // Research file in InvalidFiles folder //
                if (_orgDocFile == null)
                {
                    string InvalidFilesPath = LinkPath + "\\InvalidFiles";
                    DirectoryInfo _dirInvalidFiles = new DirectoryInfo(InvalidFilesPath);
                    if (_dirInvalidFiles.Exists)
                    {
                        FileInfo[] _pdfFiles = _dirInvalidFiles.GetFiles("*" + this.VRNO + "*.pdf");
                        foreach (FileInfo _pdf in _pdfFiles)
                        {
                            if (_pdf.Name.Contains(this.VRNO) && _pdf.Name.Contains(Path.GetFileNameWithoutExtension(this.orgEmlFile)))
                            {
                                if (CheckInvalidPDF(_pdf))
                                {
                                    _pdf.CopyTo(LinkPath + "\\" + _pdf.Name, true);
                                    _orgDocFile = new FileInfo(LinkPath + "\\" + _pdf.Name);
                                    break;
                                }
                            }
                        }
                    }
                }
                #endregion



                this.PageGUID = URL.Trim();
                FetchData();

                if (File.Exists(currentXMLFile)) result = true;
                else throw new Exception("Unable to convert Order PDF");
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }
            return result;
        }

        private bool CheckInvalidPDF(FileInfo pdfFile)
        {
            try
            {
                bool validFile = true;
                RichTextBox txt = new RichTextBox();
                string skipTextFiles = convert.ToString(ConfigurationManager.AppSettings["SKIP_TEXT_FILES"]);
                string[] texttoCheck = skipTextFiles.Trim().Split(',');
                List<string> lstText = new List<string>();
                lstText.AddRange(texttoCheck);

                txt.Text = GetPDFText(pdfFile.FullName);
                if (txt.Text.Trim().Length > 0)
                {
                    foreach (string sText in lstText)
                    {
                        if (txt.Lines[0].Replace(" ", "").Contains(sText.Replace(" ", "")))
                        {
                            validFile = false;
                            break;
                        }
                    }
                }
                else validFile = false;
                return validFile;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /* Commented By Sanjita (12-DEC-18) For Order PDF Conversion */
        //private List<List<string>> GetPOItems()
        //{
        //    List<List<string>> lstItems = new List<List<string>>();
        //    HtmlNodeCollection _tr = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='gvVendorItem']//tr[@tabindex='-1']");
        //    if (_tr != null)
        //    {
        //        CalculatedTotal = 0;
        //        if (_tr.Count > 0)
        //        {
        //            int Counter = 02, itemCount = 0;
        //            string strCounter = "";
        //            if (Counter < _tr.Count + 1) strCounter = "0" + Counter;
        //            else if (Counter > 10) strCounter = Counter.ToString();
        //            string _i = "gvVendorItem_ctl" + strCounter + "_lblSNo";
        //            HtmlNode itemNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='" + _i + "']");
        //            do
        //            {
        //                if (itemNo != null)
        //                {
        //                    string Pos = "", RefNo = "", CompName = "", TotalPrice = "";
        //                    itemCount++;
        //                    string PartNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblNumber']").InnerText.Trim();

        //                    if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblMakerReference']") != null)
        //                        RefNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblMakerReference']").InnerText.Trim();

        //                    if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblPosition']") != null)
        //                        Pos = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblPosition']").InnerText.Trim();

        //                    string details = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblDetails']").InnerText.Trim();
        //                    string Descr = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//a[@id='gvVendorItem_ctl" + strCounter + "_lnkStockItemCode']").InnerText.Trim();

        //                    if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblComponentName']") != null)
        //                        CompName = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblComponentName']").InnerText.Trim();

        //                    string QutQty = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblQuantity']").InnerText.Trim();
        //                    string OrdQty = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblOrderQuantity']").InnerText.Trim();
        //                    string Unit = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblunit']").InnerText.Trim();
        //                    string QutPrice = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblQuotedPrice']").InnerText.Trim();
        //                    string Discount = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblDiscount']").InnerText.Trim();
        //                    if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_txtTotal']") != null)
        //                        TotalPrice = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_txtTotal']").InnerText.Trim();
        //                    string LeadDays = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblDeliveryTime']").InnerText.Trim();

        //                    # region /* item remarks */
        //                    string ItemRemarks = "", _attDis = "";
        //                    HtmlNode _details = _httpWrapper.GetElement("input", "id", "gvVendorItem_ctl" + strCounter + "_cmdDetails");
        //                    if (_details != null)
        //                    {
        //                        _attDis = _details.GetAttributeValue("disabled", "");
        //                        if (_attDis == "")
        //                        {
        //                            string[] _onclick = _details.GetAttributeValue("onclick", "").Split(',');
        //                            if (_onclick.Length == 3)
        //                            {
        //                                URL = "https://apps.southnests.com/Phoenix/Purchase/" + HttpUtility.HtmlDecode(_onclick[2].Split('\'')[1]);
        //                                if (LoadURL("input", "id", "_content_txtItemDetails_ucCustomEditor_ctl02", true))//_content_txtItemDetails_ucCustomEditor_ctl" + strCounter
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
        //                                            int count = ItemRemarks.Count(f => f == '&');
        //                                            ItemRemarks = HttpUtility.HtmlDecode(ItemRemarks.Trim());
        //                                            if (count == 1) break;
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

        //                    if (_attDis == "")
        //                    {
        //                        this.URL = currentURL.Trim().Replace("PurchaseVendorRemark.aspx", "PurchaseQuotationRFQ.aspx");
        //                        if (LoadURL("input", "id", "txtnopage", true))
        //                        {
        //                            itemNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblSNo']");
        //                        }
        //                    }
        //                    //else
        //                    //{
        //                    PartNo = PartNo.Replace("__.__.__", "");
        //                    if (PartNo.Length > 0)
        //                    {
        //                        ItemRemarks += Environment.NewLine + "Number : " + PartNo;
        //                    }

        //                    if (Pos != null && Pos != "/")
        //                    {
        //                        details += Environment.NewLine + " Drawing No/Position : " + convert.ToString(Pos);
        //                    }

        //                    string EquipName = "";
        //                    if (CompName != "")
        //                        EquipName = convert.ToString(CompName);

        //                    if (QutQty != "")
        //                        ItemRemarks += Environment.NewLine + "Quoted Qty : " + QutQty.Trim();

        //                    if (TotalPrice != "")
        //                        CalculatedTotal = CalculatedTotal + (convert.ToDouble(OrdQty) * convert.ToDouble(QutPrice));
        //                    //try
        //                    //{
        //                    List<string> item = new List<string>();
        //                    item.Add(convert.ToString(itemNo.InnerText.Trim()));
        //                    item.Add(RefNo); // Item Ref No
        //                    item.Add(Descr.Trim()); // Item Description
        //                    item.Add(convert.ToDouble(OrdQty).ToString("0.00")); // Qty
        //                    item.Add(Unit); // UOM
        //                    item.Add(convert.ToDouble(QutQty).ToString("0.00")); // Qtd Qty
        //                    if (QutPrice == "") QutPrice = null;
        //                    item.Add(convert.ToDouble(QutPrice).ToString("0.00")); // Price
        //                    item.Add(convert.ToDouble(Discount).ToString("0.00")); // Disc Qty
        //                    item.Add(""); //Equipment Maker 
        //                    item.Add(details); //Equipment Remarks
        //                    item.Add(EquipName.Trim()); // Equip Name         
        //                    item.Add(convert.ToString(ItemRemarks).Trim()); // Item Remarks
        //                    lstItems.Add(item);
        //                    //}
        //                    //catch (Exception ex) { }
        //                    Counter++;
        //                    // }
        //                }

        //                int _totalPages = 0; string _totalItems = "";
        //                if (Counter.ToString().Length == 1) strCounter = "0" + Counter;
        //                else strCounter = Counter.ToString();
        //                if (itemCount >= _tr.Count)
        //                {
        //                    // Check Total Item Count
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
        //                            ItemMismatchScreenShot = ImagePath + "\\Phoenix_" + this.VRNO + "_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
        //                            _httpWrapper.GetElement("div", "id", "navigation").Attributes["class"].Remove();
        //                            _CurrentResponseString = _httpWrapper._CurrentDocument.DocumentNode.OuterHtml;
        //                            if (!PrintScreen(ItemMismatchScreenShot)) ItemMismatchScreenShot = "";
        //                            if (_tr.Count == lstItems.Count)
        //                            {
        //                                break;
        //                            }
        //                        }
        //                    }

        //                    if (_totalPages == 1 && (convert.ToInt(_totalItems) != lstItems.Count))
        //                    {                                
        //                        LogText = "Total Item count not matching with items in grid";
        //                        break;
        //                    }

        //                    // Move to next Grid Page
        //                    HtmlNode _nextPage = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//a[@id='cmdNext']");
        //                    if (_nextPage != null)
        //                    {
        //                        string _disNext = _nextPage.GetAttributeValue("disabled", "").Trim();
        //                        if (_disNext != "disabled")
        //                        {
        //                            throw new Exception("multiple pages found for item table.");
        //                        }
        //                    }

        //                    // Reset Counter
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

        /* Commented By Sanjita (12-DEC-18) For Order PDF Conversion */
        //private Dictionary<string, string> GetOrderDetails(string VRNO, string pdfFile, string BuyerEmail, string VendorEmail)
        //{
        //    RichTextBox txtData = new RichTextBox();
        //    try
        //    {
        //        pdfItemStartCount = 0;
        //        Dictionary<string, string> _xmlHeader = new Dictionary<string, string>();


        //        //if (txtData.Lines.Length > 0 && (txtData.Lines[4].Trim() == "PURCHASE ORDER" || txtData.Lines[5].Trim() == "PURCHASE ORDER" || txtData.Lines[3].Trim() == "PURCHASE ORDER"))
        //        if (txtData.Lines.Length > 0 && txtData.Text.Contains("PURCHASE ORDER") && txtData.Text.Contains("Please invoice the order to"))
        //        {                   
        //            int counter = 0;
        //            _xmlHeader["BUYER_NAME"] = txtData.Lines[0].Trim();
        //            _xmlHeader["BUYER_ADDR"] = txtData.Lines[1].Trim();
        //            _xmlHeader["VRNO"] = "";

        //            bool processLine = false;

        //            if (txtData.Lines[6].Contains("Purchase") && txtData.Lines[6].Contains("Order"))
        //            {
        //                processLine = true;
        //            }
        //            else if (txtData.Lines[5].Contains("Purchase") && txtData.Lines[5].Contains("Order"))
        //            {
        //                processLine = false;
        //            }
        //            else
        //            {
        //                throw new Exception("VRNO not found");
        //            }                 

        //            string _strLine = "";
        //            if (processLine) { _strLine = txtData.Lines[6]; }
        //            else _strLine = txtData.Lines[5];

        //            #region // RFQ No //
        //            if (_strLine.Trim() != "")
        //            {
        //                //if (_strLine.Length > 90 && _strLine.Substring(65, 20).Trim() == "Purchase Order")
        //                //    _xmlHeader["VRNO"] = _strLine.Substring(85).Trim();
        //                //else
        //                //{
        //                //    if (_strLine.Substring(65).Trim().Contains("Purchase") && _strLine.Substring(65).Trim().Contains("Order"))
        //                //    {
        //                //        string[] array = _strLine.Substring(65).Trim().Split(' ');
        //                //        _xmlHeader["VRNO"] = array[array.Length - 1].Trim();
        //                //    }
        //                //}

        //                int indx = _strLine.IndexOf("Purchase Order");
        //                if (indx > 0)
        //                {
        //                    _xmlHeader["VRNO"] = _strLine.PadRight(500).Substring(indx + 14).Trim();
        //                }
        //            }

        //            if (_xmlHeader["VRNO"] != VRNO && VRNO != "")//&& _strLine.Contains(VRNO.Trim()))
        //            {
        //                _xmlHeader["VRNO"] = VRNO.Trim();
        //            }

        //            if (_xmlHeader["VRNO"].Trim().Length == 0) throw new Exception("VRNO not found");
        //            #endregion

        //            #region // Vendor Name //
        //            if (_strLine.Length > 67 && _strLine.ToUpper().StartsWith("TO"))
        //            {
        //                _xmlHeader["VENDOR_NAME"] = _strLine.Substring(9, (67 - 9)).Trim();
        //                string str = _xmlHeader["VENDOR_NAME"];
        //                if (str.Contains("    "))
        //                {
        //                    str = str.Replace("    ", "|");
        //                    if (str.Split('|').Length > 1)
        //                    {
        //                        _xmlHeader["VENDOR_NAME"] = str.Split('|')[0].Trim();
        //                    }
        //                }
        //            }
        //            #endregion

        //            //_strLine = txtData.Lines[8];
        //            if (processLine) _strLine = txtData.Lines[8];
        //            else _strLine = txtData.Lines[7];

        //            #region // Quote Ref //
        //            if (_strLine.Length > 65 && _strLine.Substring(65).Trim() == "Quotation Reference")
        //                _xmlHeader["QUOTE_REF"] = txtData.Lines[7].Substring(85).Trim();
        //            #endregion

        //            #region  // Vendor Address //
        //            if (_strLine.Length > 67 && _strLine.Substring(0, 9).Trim() == "Address")
        //            {
        //                _xmlHeader["VENDOR_ADDRESS1"] = _strLine.Substring(9, (67 - 9)).Trim();

        //                //_strLine = txtData.Lines[9];
        //                if (processLine) _strLine = txtData.Lines[9];
        //                else _strLine = txtData.Lines[8];
        //                _xmlHeader["VENDOR_ADDRESS1"] += " " + _strLine.Trim();
        //            }
        //            #endregion

        //            #region // Order Date //
        //            int iordDate = 10;
        //            for (int c = 0; c < txtData.Lines.Length; c++) { if (txtData.Lines[c].Trim().Contains("Date")) { iordDate = c; break; } }
        //            _strLine = txtData.Lines[iordDate];
        //            if (_strLine.Length > 85 && _strLine.Substring(65, (85 - 65)).Trim() == "Date")
        //            {
        //                _xmlHeader["ORDER_DATE"] = _strLine.Substring(85).Trim();
        //            }
        //            #endregion

        //            #region // Payterms //
        //            int iPayTerm = 11;
        //            for (int c = 0; c < txtData.Lines.Length; c++) { if (txtData.Lines[c].Trim().Contains("Payment Terms")) { iPayTerm = c; break; } }
        //            _strLine = txtData.Lines[iPayTerm];
        //            if (_strLine.Length >= 80 && _strLine.Substring(65, (80 - 65)).Trim() == "Payment Terms")
        //                _xmlHeader["PAY_TERMS"] = _strLine.Substring(80).Trim();
        //            else _xmlHeader["PAY_TERMS"] = "";
        //            #endregion

        //            #region // Phone & Del Terms //
        //            int iPhoneDel = 12;
        //            for (int c = 0; c < txtData.Lines.Length; c++) { if (txtData.Lines[c].Trim().StartsWith("Phone")) { iPhoneDel = c; break; } }
        //            _strLine = txtData.Lines[iPhoneDel];
        //            if (_strLine.Length > 67 && _strLine.Substring(0, 9).Trim() == "Phone")
        //            {
        //                _xmlHeader["VENDOR_PHONE"] = _strLine.Substring(9, (67 - 9)).Trim().Trim(',').Trim();

        //                string str = _xmlHeader["VENDOR_PHONE"];
        //                if (str.Contains("    "))
        //                {
        //                    str = str.Replace("    ", "|");
        //                    if (str.Split('|').Length > 1)
        //                    {
        //                        _xmlHeader["VENDOR_PHONE"] = str.Split('|')[0].Trim().Trim(',').Trim('.');
        //                    }
        //                }
        //            }

        //            if (_strLine.Length > 85 && _strLine.Substring(65, (85 - 65)).Trim() == "Delivery Terms")
        //                _xmlHeader["DEL_TERMS"] = _strLine.Substring(85).Trim();
        //            else _xmlHeader["DEL_TERMS"] = "";
        //            #endregion

        //            #region // PORT & FAX //
        //            int iFaxPort = 13;
        //            for (int c = 0; c < txtData.Lines.Length; c++) { if (txtData.Lines[c].Trim().StartsWith("Fax")) { iFaxPort = c; break; } }
        //            _strLine = txtData.Lines[iFaxPort];
        //            if (_strLine.Length > 67 && _strLine.Substring(0, 9).Trim() == "Fax")
        //            {
        //                _xmlHeader["VENDOR_FAX"] = _strLine.Substring(9, (67 - 9)).Trim();
        //                string str = _xmlHeader["VENDOR_FAX"];
        //                if (str.Contains("    "))
        //                {
        //                    str = str.Replace("    ", "|");
        //                    if (str.Split('|').Length > 1)
        //                    {
        //                        _xmlHeader["VENDOR_FAX"] = str.Split('|')[0].Trim().Trim(',').Trim('.');
        //                    }
        //                }
        //            }

        //            if (_strLine.Length > 85 && _strLine.Substring(65, (85 - 65)).Trim() == "Port")
        //            {
        //                _xmlHeader["PORT_NAME"] = _strLine.Substring(85).Trim();
        //            }
        //            else
        //            {
        //                if (_strLine.Contains("Port"))
        //                {
        //                    int portIdex = _strLine.IndexOf("Port");
        //                    string _port = _strLine.Substring(portIdex + 4);
        //                    _xmlHeader["PORT_NAME"] = _port.Trim();
        //                }
        //                else _xmlHeader["PORT_NAME"] = "";
        //            }
        //            #endregion

        //            #region // Vendor Email //
        //            int iVendorEmail = 14;
        //            for (int c = 0; c < txtData.Lines.Length; c++) { if (txtData.Lines[c].Trim().StartsWith("Mail")) { iVendorEmail = c; break; } }
        //            _strLine = txtData.Lines[iVendorEmail];
        //            if (_strLine.Length >= 67 && _strLine.Substring(0, 9).Trim() == "Mail")
        //            {
        //                string _email = _strLine.Substring(9, (67 - 9)).Trim().Replace("  ", "|");
        //                while (_email.Contains("||")) _email = _email.Replace("||", "|").Trim();
        //                string[] _values = _email.Split('|');

        //                _xmlHeader["VENDOR_EMAIL"] = _values[0].Split(',')[0].Trim();
        //                //if (txtData.Lines[iVendorEmail + 1].Length > 67 && txtData.Lines[15].Substring(9, (67 - 9)).Trim() != "")
        //                //{
        //                //    //counter++;
        //                //    _xmlHeader["VENDOR_EMAIL"] += txtData.Lines[15].Substring(9, (67 - 9)).Trim();
        //                //}
        //                //else
        //                //{
        //                if (VendorEmail.Trim().Length > 0 && _xmlHeader["VENDOR_EMAIL"].Trim() == "") _xmlHeader["VENDOR_EMAIL"] = VendorEmail.Trim();
        //                //}
        //            }
        //            else
        //            {
        //                if (VendorEmail.Trim().Length > 0) _xmlHeader["VENDOR_EMAIL"] = VendorEmail.Trim();
        //                else
        //                {
        //                    for (int c = 0; c < txtData.Lines.Length; c++) { if (txtData.Lines[c].Trim().Contains("@")) { iVendorEmail = c; break; } }
        //                    _strLine = txtData.Lines[iVendorEmail];
        //                    string _email = _strLine.Trim().Replace("  ", "|");
        //                    while (_email.Contains("||")) _email = _email.Replace("||", "|").Trim();
        //                    string[] _values = _email.Split('|');

        //                    _xmlHeader["VENDOR_EMAIL"] = _values[0].Split(',')[0].Trim();
        //                }
        //            }
        //            #endregion

        //            #region // ETA & Header remarks //
        //            if (_strLine.Length > 85 && _strLine.Substring(65, (85 - 65)).Trim() == "ETA")
        //                _xmlHeader["ETA"] = _strLine.Substring(85).Trim();
        //            else _xmlHeader["ETA"] = "";

        //            _xmlHeader["REMARKS"] = "";
        //            for (int k = (30 + counter); k < txtData.Lines.Length; k++)
        //            {
        //                if (txtData.Lines[k].Trim().StartsWith("Vessel Name"))
        //                {
        //                    counter = k;
        //                    break;
        //                }
        //                _xmlHeader["REMARKS"] += " " + txtData.Lines[k].Trim();
        //            }
        //            #endregion

        //            #region // Vessel //
        //            _strLine = txtData.Lines[counter];
        //            if (_strLine.Length > 15 && _strLine.Substring(0, 15).Trim() == "Vessel Name")
        //                _xmlHeader["VESSEL"] = _strLine.Substring(15, (65 - 15)).Trim();

        //            if (_xmlHeader["VESSEL"].Trim() == "")
        //            {
        //                for (int c = counter + 1; c < txtData.Lines.Length; c++)
        //                {
        //                    counter++;
        //                    if (txtData.Lines[c].Length > 50)
        //                    {
        //                        if (txtData.Lines[c].Substring(0, 15).Trim() == "" && txtData.Lines[c].Substring(15, 35).Trim().Length > 0)
        //                        {
        //                            _xmlHeader["VESSEL"] = txtData.Lines[c].Substring(15, 35).Trim();
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            #endregion

        //            #region // IMO //
        //            _strLine = txtData.Lines[counter + 2];
        //            while (!txtData.Lines[counter + 2].Trim().StartsWith("IMO"))
        //            {
        //                counter++;
        //                _strLine = txtData.Lines[counter + 2];
        //                if (counter == txtData.Lines.Length) throw new Exception("IMO number not found");
        //            }

        //            if (_strLine.Length > 15 && _strLine.Substring(0, 15).Trim() == "IMO NO")
        //                _xmlHeader["IMO"] = _strLine.Substring(15, (65 - 15)).Trim();
        //            #endregion

        //            #region // Remarks //
        //            int remStartIndx = 0;
        //            for (int k = (counter + 3); k < txtData.Lines.Length; k++)
        //            {
        //                _strLine = txtData.Lines[k];
        //                if (_strLine.Length > 0 && _strLine.Trim().StartsWith("MAKER'S NAME"))
        //                {
        //                    remStartIndx = k;
        //                    break;
        //                }
        //            }

        //            if (!_xmlHeader.ContainsKey("REMARKS")) _xmlHeader["REMARKS"] = "";
        //            for (int k = remStartIndx; k < txtData.Lines.Length; k++)
        //            {
        //                if (txtData.Lines[k].Trim().StartsWith("Component Name"))
        //                {
        //                    pdfItemStartCount = k;
        //                    break;
        //                }
        //                _xmlHeader["REMARKS"] += Environment.NewLine + txtData.Lines[k].Trim();
        //            }
        //            #endregion

        //            #region // Get Buyer Email ID //
        //            for (int k = pdfItemStartCount; k < txtData.Lines.Length; k++)
        //            {
        //                if (txtData.Lines[k].Trim().Contains("Email ID"))
        //                {
        //                    pdfBuyerEmail = txtData.Lines[k + 1].Trim().Replace("/", ",");
        //                    if (pdfBuyerEmail.Trim() == "")
        //                    {
        //                        pdfBuyerEmail = txtData.Lines[k].Substring(txtData.Lines[k].IndexOf("Email ID") + 10).Trim().Replace("/", ",");
        //                    }
        //                    break;
        //                }
        //            }
        //            if (pdfBuyerEmail.Trim() != "") _xmlHeader["BUYER_EMAIL"] = pdfBuyerEmail;
        //            #endregion                  
        //        }
        //        else
        //        {
        //            throw new Exception("Invalid Email");
        //        }

        //        return _xmlHeader;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        try
        //        { txtData.Dispose(); }
        //        catch { }
        //    }
        //}

        //private List<List<string>> GetOrderItems(string pdfFile, ref List<List<string>> _lineItems)
        //{
        //    RichTextBox txtData = new RichTextBox();
        //    try
        //    {
        //        if (_lineItems == null) _lineItems = new List<List<string>>();
        //        else _lineItems.Clear();
        //        txtData.Text = GetPDFText(pdfFile);
        //        CalculatedTotal = 0;

        //        if (pdfItemStartCount > 0)
        //        {
        //            string compName = "", modal = "", SerNo = "";
        //            Dictionary<string, string> _iDetails = new Dictionary<string, string>();

        //            for (int i = pdfItemStartCount; i < txtData.Lines.Length; i++)
        //            {
        //                string line = txtData.Lines[i];
        //                if (line.Trim().StartsWith("Line Item Sub Total:")) break;
        //                if (line.Trim().StartsWith("Component"))
        //                {
        //                    compName = ""; modal = ""; SerNo = "";
        //                    line = line.PadRight(150);
        //                    if (line.Substring(64, (75 - 64)).Trim() == "MODEL") modal = line.Substring(75).Trim();
        //                    else
        //                    {
        //                        if (line.Trim().Length > 75 && txtData.Lines[i + 1].Contains("MODEL")) modal = line.Substring(75).Trim();
        //                    }
        //                    i++;

        //                    line = txtData.Lines[i].PadRight(150);
        //                    compName = line.Substring(0, 60).Trim().Replace("Component Name", "");
        //                    if (line.Length > 75 && line.Substring(60, (75 - 60)).Trim() == "SERIAL NO") SerNo = line.Substring(75).Trim();
        //                }
        //                else if (line.Trim().StartsWith("Item"))
        //                {
        //                    i = i + 2;
        //                    if (txtData.Lines[i].Contains("Name:")) { i--; }
        //                }
        //                else if (line.Trim().Length > 0)
        //                {
        //                    string no = line.Trim().Split(' ')[0];
        //                    if (convert.ToInt(no) > 0 && line.Contains("Name"))
        //                    {
        //                        if (_iDetails.ContainsKey("ITEM_NO"))
        //                        {
        //                            List<string> _item = new List<string>();
        //                            //_iDetails
        //                            _item.Add(_iDetails["ITEM_NO"]);
        //                            if (_iDetails.ContainsKey("REF_NO")) _item.Add(_iDetails["REF_NO"]); // Item Ref No
        //                            else _item.Add("");
        //                            _item.Add(_iDetails["NAME"].Trim()); // Item Description
        //                            _item.Add(convert.ToDouble(_iDetails["QTY"]).ToString("0.00")); // Qty
        //                            _item.Add(_iDetails["UNIT"].Trim()); // UOM
        //                            _item.Add(convert.ToDouble(0).ToString("0.00")); // Qtd Qty
        //                            _item.Add(convert.ToDouble(_iDetails["PRICE"]).ToString("0.00")); // Price
        //                            _item.Add(convert.ToDouble(_iDetails["DISC"]).ToString("0.00")); // Disc

        //                            _item.Add(""); //Equipment Maker 
        //                            string equipRem = "";
        //                            if (modal != "") equipRem = "Modal : " + modal;
        //                            if (SerNo != "") equipRem += "Serial No. : " + SerNo;
        //                            _item.Add(equipRem); //Equipment Remarks
        //                            _item.Add(compName.Trim()); // Equip Name   

        //                            string itemRem = "";
        //                            if (_iDetails["PART_NO"].Trim() != "") itemRem = "Part No. : " + _iDetails["PART_NO"].Trim() + "; ";

        //                            itemRem += "Draw No/ Pos No : " + (_iDetails.ContainsKey("DRAW_NO") ? _iDetails["DRAW_NO"].Trim() : "") + "/" + (_iDetails.ContainsKey("POS_NO") ? _iDetails["POS_NO"].Trim() : "") + "; ";
        //                            if (_iDetails.ContainsKey("REMARKS")) itemRem += Environment.NewLine + _iDetails["REMARKS"].Trim();
        //                            _item.Add(itemRem);
        //                            _item.Add(_iDetails["ITEM_NO"].Trim()); // OrgSysRef

        //                            CalculatedTotal = CalculatedTotal + convert.ToDouble(_iDetails["TOTAL"].Trim());

        //                            _lineItems.Add(_item);
        //                            _item = new List<string>();
        //                            _iDetails.Clear();
        //                        }
        //                        _iDetails.Add("ITEM_NO", no.Trim());
        //                        if (line.Substring(23).Trim().StartsWith("Name:"))
        //                        {
        //                            string _newLine = line.Trim().Replace("  ", " ");

        //                            _iDetails.Add("PART_NO", _newLine.Trim().Split(' ')[1].Trim());
        //                            if (_iDetails.ContainsKey("NAME")) _iDetails["NAME"] = _iDetails["NAME"] + " " + line.Substring(23, (71 - 22)).Replace("Name", "").Trim().Trim(':').Trim();
        //                            _iDetails.Add("NAME", line.Substring(23, (71 - 22)).Replace("Name", "").Trim().Trim(':').Trim());
        //                            _iDetails.Add("REMARKS", "");

        //                            string line1 = line.Substring(72).Trim();
        //                            while (line1.Contains("  ")) line1 = line1.Replace("  ", " ");

        //                            string[] arr = line1.Split(' ');
        //                            if (arr.Length == 6)
        //                            {
        //                                _iDetails.Add("QTY", convert.ToDouble(arr[0]).ToString("0.00"));
        //                                _iDetails.Add("UNIT", arr[1].Trim());
        //                                _iDetails.Add("PRICE", convert.ToDouble(arr[2]).ToString("0.00"));
        //                                if (currCode.Trim().Length == 0) currCode = convert.ToString(arr[3]).Trim();
        //                                _iDetails.Add("DISC", convert.ToDouble(arr[4]).ToString("0.00"));
        //                                _iDetails.Add("TOTAL", convert.ToDouble(arr[arr.Length - 1]).ToString("0.00"));
        //                            }
        //                            else
        //                            {
        //                                _iDetails.Add("QTY", convert.ToDouble(arr[0]).ToString("0.00"));
        //                                _iDetails.Add("UNIT", arr[1].Trim());
        //                                _iDetails.Add("PRICE", convert.ToDouble(arr[2]).ToString("0.00"));
        //                                if (currCode.Trim().Length == 0) currCode = convert.ToString(arr[3]).Trim();
        //                                _iDetails.Add("DISC", convert.ToDouble(0).ToString("0.00"));
        //                                _iDetails.Add("TOTAL", convert.ToDouble(arr[arr.Length - 1]).ToString("0.00"));
        //                            }
        //                        }
        //                        else if (line.Substring(18).Trim().StartsWith("Name:") || line.Contains("Name"))
        //                        {
        //                            _iDetails.Add("PART_NO", line.Trim().Split(' ')[1].Trim());
        //                            _iDetails.Add("NAME", line.Substring(19, (68 - 19)).Replace("Name:", "").Trim());
        //                            _iDetails.Add("REMARKS", "");

        //                            string line1 = line.Substring(68).Trim();
        //                            while (line1.Contains("  ")) line1 = line1.Replace("  ", " ");

        //                            string[] arr = line1.Split(' ');
        //                            if (arr.Length == 5)
        //                            {
        //                                _iDetails.Add("QTY", convert.ToDouble(arr[0]).ToString("0.00"));
        //                                _iDetails.Add("UNIT", arr[1].Trim());
        //                                _iDetails.Add("PRICE", convert.ToDouble(arr[2]).ToString("0.00"));
        //                                if (currCode.Trim().Length == 0) currCode = convert.ToString(arr[3]).Trim();
        //                                _iDetails.Add("DISC", convert.ToDouble(0).ToString("0.00"));
        //                                _iDetails.Add("TOTAL", convert.ToDouble(arr[arr.Length - 1]).ToString("0.00"));
        //                            }
        //                            else
        //                            {
        //                                _iDetails.Add("QTY", convert.ToDouble(arr[0]).ToString("0.00"));
        //                                _iDetails.Add("UNIT", arr[1].Trim());
        //                                _iDetails.Add("PRICE", convert.ToDouble(arr[2]).ToString("0.00"));
        //                                if (currCode.Trim().Length == 0) currCode = convert.ToString(arr[3]).Trim();
        //                                _iDetails.Add("DISC", convert.ToDouble(arr[4]).ToString("0.00"));
        //                                _iDetails.Add("TOTAL", convert.ToDouble(arr[arr.Length - 1]).ToString("0.00"));
        //                            }
        //                        }
        //                    }
        //                    else if (line.Trim().StartsWith("Maker Ref:"))
        //                    {
        //                        if (!_iDetails.ContainsKey("REF_NO")) _iDetails.Add("REF_NO", line.Trim().Replace("Maker Ref:", "").Trim());
        //                        else _iDetails["REF_NO"] = line.Trim().Replace("Maker Ref:", "").Trim();
        //                    }
        //                    else if (line.Trim().StartsWith("Product Code:"))
        //                    {
        //                        if (!_iDetails.ContainsKey("REF_NO")) _iDetails.Add("REF_NO", line.Trim().Replace("Product Code:", "").Trim());
        //                        else _iDetails["REF_NO"] = line.Trim().Replace("Product Code:", "").Trim();
        //                    }
        //                    else if (line.Trim().StartsWith("Dwg No:"))
        //                    {
        //                        if (!_iDetails.ContainsKey("DRAW_NO")) _iDetails.Add("DRAW_NO", line.Trim().Replace("Dwg No:", "").Trim());
        //                        else _iDetails["DRAW_NO"] = line.Trim().Replace("Dwg No:", "").Trim();
        //                    }
        //                    else if (line.Trim().StartsWith("Pos No:"))
        //                    {
        //                        if (!_iDetails.ContainsKey("POS_NO")) _iDetails.Add("POS_NO", line.Trim().Replace("Pos No:", "").Trim());
        //                        else _iDetails["POS_NO"] = line.Trim().Replace("Pos No:", "").Trim();
        //                    }
        //                    else
        //                    {
        //                        if (_iDetails.ContainsKey("ITEM_NO"))
        //                        {
        //                            // line.Length > 6 additional condition included on 28-JUL-16
        //                            if (line.Length > 6 && line.Substring(0, 6).Trim() == "" && _iDetails.ContainsKey("NAME"))
        //                            {
        //                                line = line.PadRight(150);
        //                                _iDetails["NAME"] += " " + line.Substring(23, (71 - 23)).Trim();
        //                            }
        //                            else
        //                            {
        //                                if (_iDetails.ContainsKey("REMARKS")) _iDetails["REMARKS"] += Environment.NewLine + line.Trim();
        //                                else _iDetails.Add("REMARKS", line.Trim());
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (Regex.IsMatch(txtData.Lines[i + 1].Trim(), @"\s*[\d]{1,}\s+[Name]{1,}\s*[:]\s*"))
        //                            {
        //                                _iDetails.Add("NAME", line.Trim());
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            if (_iDetails.Count > 0)
        //            {
        //                List<string> _item = new List<string>();
        //                //_iDetails
        //                _item.Add(_iDetails["ITEM_NO"]);
        //                if (_iDetails.ContainsKey("REF_NO")) _item.Add(_iDetails["REF_NO"]); // Item Ref No
        //                else _item.Add("");
        //                _item.Add(_iDetails["NAME"].Trim()); // Item Description
        //                _item.Add(convert.ToDouble(_iDetails["QTY"]).ToString("0.00")); // Qty
        //                _item.Add(_iDetails["UNIT"].Trim()); // UOM
        //                _item.Add(convert.ToDouble(0).ToString("0.00")); // Qtd Qty
        //                _item.Add(convert.ToDouble(_iDetails["PRICE"]).ToString("0.00")); // Price
        //                _item.Add(convert.ToDouble(_iDetails["DISC"]).ToString("0.00")); // Disc

        //                _item.Add(""); //Equipment Maker 
        //                string equipRem = "";
        //                if (modal != "") equipRem = "Modal : " + modal;
        //                if (SerNo != "") equipRem += "Serial No. : " + SerNo;
        //                _item.Add(equipRem); //Equipment Remarks
        //                _item.Add(compName.Trim()); // Equip Name   

        //                string itemRem = "";
        //                if (_iDetails["PART_NO"].Trim() != "") itemRem = "Part No. : " + _iDetails["PART_NO"].Trim() + "; ";
        //                if (_iDetails.ContainsKey("DRAW_NO") || _iDetails.ContainsKey("POS_NO"))
        //                {
        //                    itemRem += "Draw No/ Pos No : " + (_iDetails.ContainsKey("DRAW_NO") ? _iDetails["DRAW_NO"].Trim() : "") + "/" + (_iDetails.ContainsKey("POS_NO") ? _iDetails["POS_NO"].Trim() : "") + "; ";
        //                }
        //                if (_iDetails.ContainsKey("REMARKS")) itemRem += Environment.NewLine + _iDetails["REMARKS"].Trim();
        //                _item.Add(itemRem);
        //                _item.Add(_iDetails["ITEM_NO"].Trim()); // OrgSysRef

        //                CalculatedTotal = CalculatedTotal + convert.ToDouble(_iDetails["TOTAL"].Trim());

        //                _lineItems.Add(_item);
        //            }
        //        }
        //        return _lineItems;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        try
        //        { txtData.Dispose(); }
        //        catch { }
        //    }
        //}
     

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
                        }
                        else
                        {
                            LogText = "POC processing started.";
                            ProcessPOC(xmlFiles[j]);
                        }
                    }
                }
                else LogText = "No quotes found.";
            }
            catch (Exception ex)
            {
                LogText = "Exception in Process Quote : " + ex.GetBaseException().Message.ToString();
                // WriteErrorLogQuote_With_Screenshot("Exception in Process Quote : " + ex.GetBaseException().Message.ToString(), xmlFiles[j]);
            }
        }

        public void ProcessQuoteMTML(string MTMLFile)
        {
            double FinalAmt = 0; string strAuditmsg = "";
            try
            {
                if (UCRefNo != "")
                {
                    LogText = "Quote processing started for refno: " + UCRefNo;
                    URL = MessageNumber;
                    if (LoadURL("span", "id", "Title1_lblTitle"))
                    {
                        HtmlNode _title = _httpWrapper.GetElement("span", "id", "Title1_lblTitle");
                        HtmlNode _btnSave = _httpWrapper.GetElement("a", "id", "MenuQuotationLineItem_dlstTabs_ctl00_btnMenu");
                        if (_title != null && _btnSave != null)
                        {
                            if (_title.InnerText.Trim().Contains(UCRefNo) && _btnSave.InnerText.Trim().ToUpper() == "SAVE")
                            {
                                LogText = "Filling Quotation.";
                                if (FillQuotation(true))
                                {
                                    if (FillQuotation(false))//added to get Url of quotation remarks 
                                    {
                                        LogText = "Filling quotation remarks.";
                                        if (FillQuoteRemarks())
                                        {
                                            //if (!IsDecline)
                                            //{
                                            try
                                            {
                                                double extraItemCost = 0;
                                                if (convert.ToInt(IsDecline) == 0)
                                                {
                                                    if (FillItems(out extraItemCost))
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
                                                                LogText = strAuditmsg = "Unable to Save Quote for ref no: " + UCRefNo + " since Quote is in declined status";
                                                                //CreateAuditFile("", "Phoenix_Http_" + this.DocType, UCRefNo, "Error", "Decline quote for ref no: " + UCRefNo, BuyerCode, SupplierCode, AuditPath);
                                                                CreateAuditFile("", "Phoenix_Http_" + this.DocType, UCRefNo, "Error", "LeS-1008.2:" + strAuditmsg, BuyerCode, SupplierCode, AuditPath);
                                                                if (!Directory.Exists(QuotePOCPath + "\\Backup")) Directory.CreateDirectory(QuotePOCPath + "\\Backup");
                                                                File.Move(QuotePOCPath + "\\" + Path.GetFileName(MTMLFile), QuotePOCPath + "\\Backup\\" + Path.GetFileName(MTMLFile));
                                                            }
                                                            else
                                                            {
                                                                //LogText = strAuditmsg="Unable to decline quote for ref no: " + UCRefNo;
                                                                //CreateAuditFile("", "Phoenix_Http_" + this.DocType, UCRefNo, "Error", "Unable to decline quote for ref no: " + UCRefNo, BuyerCode, SupplierCode, AuditPath);
                                                                LogText = strAuditmsg = "Unable to decline quote for ref no: " + UCRefNo;
                                                                CreateAuditFile("", "Phoenix_Http_" + this.DocType, UCRefNo, "Error", "LeS-1024:" + strAuditmsg, BuyerCode, SupplierCode, AuditPath);
                                                                if (!Directory.Exists(QuotePOCPath + "\\Error")) Directory.CreateDirectory(QuotePOCPath + "\\Error");
                                                                File.Move(QuotePOCPath + "\\" + Path.GetFileName(MTMLFile), QuotePOCPath + "\\Error\\" + Path.GetFileName(MTMLFile));
                                                            }
                                                        }
                                                    }
                                                    else
                                                        //WriteErrorLogQuote_With_Screenshot("Unable to save item details for ref no: " + UCRefNo, MTMLFile);
                                                        WriteErrorLogQuote_With_Screenshot("Unable to Save file for " + this.DocType + " of ref no: " + UCRefNo + "- item Field(s) not present", MTMLFile, "LeS-1007:");
                                                }
                                                else
                                                {
                                                    if (DeclineQuote(MTMLFile))
                                                    {
                                                        // LogText = "Decline quote for ref no: " + UCRefNo;
                                                        // CreateAuditFile("", "Phoenix_Http_" + this.DocType, UCRefNo, "Error", "Decline quote for ref no: " + UCRefNo, BuyerCode, SupplierCode, AuditPath);
                                                        LogText = strAuditmsg = "Unable to Save Quote for ref no: " + UCRefNo + " since Quote is in declined status";
                                                        CreateAuditFile("", "Phoenix_Http_" + this.DocType, UCRefNo, "Error", "LeS-1008.2:" + strAuditmsg, BuyerCode, SupplierCode, AuditPath);
                                                        if (!Directory.Exists(QuotePOCPath + "\\Backup")) Directory.CreateDirectory(QuotePOCPath + "\\Backup");
                                                        File.Move(QuotePOCPath + "\\" + Path.GetFileName(MTMLFile), QuotePOCPath + "\\Backup\\" + Path.GetFileName(MTMLFile));
                                                    }
                                                    else
                                                    {
                                                        LogText = strAuditmsg = "Unable to decline quote for ref no: " + UCRefNo;
                                                        CreateAuditFile("", "Phoenix_Http_" + this.DocType, UCRefNo, "Error", "LeS-1024:" + strAuditmsg, BuyerCode, SupplierCode, AuditPath);
                                                        if (!Directory.Exists(QuotePOCPath + "\\Error")) Directory.CreateDirectory(QuotePOCPath + "\\Error");
                                                        File.Move(QuotePOCPath + "\\" + Path.GetFileName(MTMLFile), QuotePOCPath + "\\Error\\" + Path.GetFileName(MTMLFile));
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                //WriteErrorLogQuote_With_Screenshot("Unable to save item details for ref no: " + UCRefNo + ", " + ex.GetBaseException().Message, MTMLFile);
                                                WriteErrorLogQuote_With_Screenshot("Unable to Save file for " + this.DocType + " of ref no: " + UCRefNo + "- item detail Field(s) not present", MTMLFile, "LeS-1007:");
                                            }
                                            // }
                                        }
                                        else
                                            //WriteErrorLogQuote_With_Screenshot("Unable to save quote remarks for ref no: " + UCRefNo, MTMLFile);
                                            WriteErrorLogQuote_With_Screenshot("Unable to Save file for " + this.DocType + " of ref no: " + UCRefNo + "- quote remarks Field(s) not present", MTMLFile, "LeS-1007:");
                                    }
                                }
                                else
                                    //WriteErrorLogQuote_With_Screenshot("Unable to save header deatils for ref no: " + UCRefNo, MTMLFile);
                                    WriteErrorLogQuote_With_Screenshot("Unable to Save file for " + this.DocType + " of ref no: " + UCRefNo + "- header detail Field(s) not present", MTMLFile, "LeS-1007:");
                            }
                            else
                            {
                                if (_btnSave.InnerText.Trim().ToUpper() != "SAVE")
                                {
                                    //WriteErrorLogQuote_With_Screenshot("Quote for ref no: " + UCRefNo + " is already submitted.", MTMLFile);
                                    WriteErrorLogQuote_With_Screenshot("Unable to submit quote for ref no: " + UCRefNo + ", quote already submitted.", MTMLFile, "LeS-1011.3:");
                                }
                            }
                        }
                        else
                        {
                            if (_btnSave == null)
                                //WriteErrorLogQuote_With_Screenshot("Save button not found for ref no: " + UCRefNo, MTMLFile);
                                WriteErrorLogQuote_With_Screenshot("Unable to Save Quote due to missing controls for ref no: " + UCRefNo, MTMLFile, "LeS-1008.3:");
                        }
                    }
                    else
                        //WriteErrorLogQuote_With_Screenshot("Unable to load quote details for ref no: " + UCRefNo, MTMLFile);
                        WriteErrorLogQuote_With_Screenshot("Unable to load url for ref no: " + UCRefNo, MTMLFile, "LeS-1016:");
                }
            }
            catch (Exception ex)
            {
                //WriteErrorLogQuote_With_Screenshot("Exception while  processing Quote MTML: " + ex.GetBaseException().Message.ToString(), MTMLFile);
                WriteErrorLogQuote_With_Screenshot("Unable to process file due to " + ex.GetBaseException().Message.ToString(), MTMLFile, "LeS-1004:");
            }
        }

        public bool FillQuotation(bool IsNew)
        {
            bool result = false;
            try
            {
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
                            if (Currency != "")
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
                            else dctPostDataValues.Add("ucCurrency%24ddlCurrency", "Dummy");
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
                            if (PostURL("input", "id", "txtVenderReference"))
                            {
                                HtmlNode _txtVenRef = _httpWrapper.GetElement("input", "id", "txtVenderReference");
                                if (_txtVenRef.GetAttributeValue("value", "").Trim() == AAGRefNo)
                                { if (IsNew) LogText = "Header details saved."; result = true; }
                                else { result = false; }
                            }
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
                result = false;
            }
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
                    URL = OrgConfigURL + URL;// "https://apps.southnests.com/Phoenix/Purchase/"
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

        public bool FillItems(out double extraItemCost)
        {
            extraItemCost = 0;
            bool result = false; Dictionary<int, LineItem> _items = new Dictionary<int, LineItem>();
            List<LineItem> _extraItems = new List<LineItem>();
            try
            {
                LogText = "Filling items.";
                URL = MessageNumber;
                _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
                _httpWrapper.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                _httpWrapper.ContentType = "application/x-www-form-urlencoded";
                _httpWrapper._SetRequestHeaders[HttpRequestHeader.CacheControl] = "no-cache";
                _httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");
                if (LoadURL("span", "id", "Title1_lblTitle"))
                {
                    HtmlNode _tblItems = _httpWrapper.GetElement("table", "id", "gvVendorItem");
                    if (_tblItems != null)
                    {
                        int count = _tblItems.SelectNodes("./tr").Count;
                        if (count >= 2)
                        {
                            foreach (LineItem item in _lineitem)
                            {
                                if (convert.ToInt(item.IsExtraItem) == 1)
                                {
                                    _extraItems.Add(item);
                                }
                                else _items.Add(convert.ToInt(item.OriginatingSystemRef), item);
                            }

                            int SrNo = 0;
                            foreach (HtmlNode _tr in _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='gvVendorItem']//tr"))
                            {

                                LineItem iObj = null;
                                URL = MessageNumber;
                                LoadURL("", "", "");

                                if (_tr.InnerText != "\r\n\t\t" && _tr.ChildNodes[1].ChildNodes[1].InnerText.Trim() != "S.No")
                                {
                                    string id = _tr.ChildNodes[1].ChildNodes[1].GetAttributeValue("id", "").Trim();
                                    string[] _strFields = id.Split('_');
                                    string ctlID = _strFields[1].Trim();

                                    HtmlNode _hID = _httpWrapper.GetElement("span", "id", id);
                                    SrNo = convert.ToInt(_hID.InnerText.Trim());
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

                                            bool uomFound = false;
                                            bool updateUOM = true;
                                            HtmlNode lblUnit = _httpWrapper.GetElement("span", "id", "gvVendorItem_" + ctlID + "_lblunit");
                                            if (lblUnit != null)
                                            {
                                                if (iObj.MeasureUnitQualifier.Trim().ToUpper() == lblUnit.InnerText.Trim().ToUpper())//_tr.ChildNodes[_unitCol].ChildNodes[1]
                                                {
                                                    uomFound = true;
                                                    updateUOM = false;
                                                }
                                            }

                                            if (convert.ToFloat(iObj.MonetaryAmount) > 0)
                                            {
                                                #region edit item row
                                                LogText = "Updating item " + convert.ToString(iObj.Number);
                                                _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
                                                _httpWrapper.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                                                _httpWrapper.ContentType = "application/x-www-form-urlencoded";
                                                _httpWrapper._SetRequestHeaders[HttpRequestHeader.CacheControl] = "no-cache";

                                                dctPostDataValues.Clear();
                                                dctPostDataValues.Add("__EVENTTARGET", _httpWrapper._dctStateData["__EVENTTARGET"]);
                                                dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
                                                dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", _httpWrapper._dctStateData["ToolkitScriptManager1_HiddenField"]);
                                                dctPostDataValues.Add("__VIEWSTATE", Uri.EscapeDataString(HttpUtility.UrlDecode(_httpWrapper._dctStateData["__VIEWSTATE"])));
                                                dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                                                dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);
                                                dctPostDataValues.Add("meeDiscount_ClientState", "");
                                                dctPostDataValues.Add("gvTax%24ctl03%24MaskedEditTotalPayableAmout_ClientState", "");
                                                dctPostDataValues.Add("MaskedEditExtender1_ClientState", "");
                                                dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24cmdEdit.x", "3");
                                                dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24cmdEdit.y", "7");
                                                #endregion
                                                if (PostURL("select", "id", "gvVendorItem_" + ctlID + "_ucUnit_ddlUnit"))
                                                {
                                                    #region saving item row
                                                    dctPostDataValues.Clear();
                                                    dctPostDataValues.Add("__EVENTTARGET", _httpWrapper._dctStateData["__EVENTTARGET"]);
                                                    dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
                                                    dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", _httpWrapper._dctStateData["ToolkitScriptManager1_HiddenField"]);
                                                    dctPostDataValues.Add("__VIEWSTATE", Uri.EscapeDataString(HttpUtility.UrlDecode(_httpWrapper._dctStateData["__VIEWSTATE"])));
                                                    dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                                                    dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);
                                                    dctPostDataValues.Add("__SCROLLPOSITIONX", "0");
                                                    dctPostDataValues.Add("__SCROLLPOSITIONY", "590");

                                                    string _unitVal = "";
                                                    var options = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='gvVendorItem_" + ctlID + "_ucUnit_ddlUnit']/option[@selected='selected']");
                                                    if (updateUOM)
                                                    {
                                                        try
                                                        {
                                                            if (options != null)
                                                            {
                                                                foreach (var _opt in options)
                                                                {
                                                                    if (_opt.InnerText.Trim().ToUpper() == iObj.MeasureUnitQualifier.Trim().ToUpper())
                                                                    {
                                                                        _unitVal = _opt.GetAttributeValue("value", "").Trim();
                                                                        uomFound = true;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            LogText = "Unable to update UOM for item " + SrNo + ". Error - " + ex.Message;
                                                        }
                                                    }

                                                    if (uomFound)
                                                    {
                                                        dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtQuantityEdit%24txtNumber", convert.ToString(iObj.Quantity));
                                                        dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtQuotedPriceEdit%24txtNumber", convert.ToString(_price));
                                                        dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtDiscountEdit%24txtNumber", convert.ToString(_disc));
                                                    }
                                                    else
                                                    {
                                                        if (options[0].InnerText.Trim() == "--Select--")
                                                        {
                                                            if (options.Count == 2)
                                                            {
                                                                _unitVal = options[1].GetAttributeValue("value", "").Trim();
                                                                uomFound = true;
                                                            }
                                                            else if (options.Count > 2)
                                                            {
                                                                //set default 
                                                                foreach (var _opt in options)
                                                                {
                                                                    if (_opt.InnerText.Trim().ToUpper() == "PCE" || _opt.InnerText.Trim().ToUpper() == "PCS")
                                                                    {
                                                                        uomFound = true;
                                                                        _unitVal = _opt.GetAttributeValue("value", "").Trim();
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                            if (uomFound == false) throw new Exception("Unable to set UOM for item no. " + iObj.Number);
                                                        }

                                                        // Calculate Avarage Price
                                                        string orgQtyStr = "";
                                                        HtmlNode orgQty = _httpWrapper.GetElement("input", "id", "gvVendorItem_" + ctlID + "_txtQuantityEdit_txtNumber");
                                                        if (orgQty != null)
                                                        {
                                                            orgQtyStr = orgQty.GetAttributeValue("Value", "");
                                                            if (orgQtyStr.Trim().Length > 0)
                                                            {
                                                                double unitPrice = iObj.MonetaryAmount / convert.ToDouble(orgQtyStr);
                                                                dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtQuotedPriceEdit%24txtNumber", Math.Round(unitPrice, 3).ToString());
                                                            }
                                                            else throw new Exception("Original Qty is 0.");
                                                        }
                                                        dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtDiscountEdit%24txtNumber", convert.ToString(_disc));
                                                        if (convert.ToFloat(orgQtyStr) != convert.ToFloat(iObj.Quantity))
                                                        {
                                                            iAddRemarks = "Quoted Qty : " + convert.ToFloat(iObj.Quantity) + "; ";
                                                        }

                                                        iAddRemarks += "Quoted UOM : " + convert.ToString(iObj.MeasureUnitQualifier).Trim().ToUpper();
                                                    }

                                                    if (_unitVal != "")
                                                        dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24ucUnit%24ddlUnit", _unitVal);
                                                    dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24txtDeliveryTimeEdit%24txtNumber", iObj.DeleiveryTime);
                                                    dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24cmdUpdate.x", "0");
                                                    dctPostDataValues.Add("gvVendorItem%24" + ctlID + "%24cmdUpdate.y", "0");
                                                    dctPostDataValues.Add("isouterpage", "");
                                                    dctPostDataValues.Add("txtnopage", "");
                                                    #endregion
                                                    if (PostURL("span", "id", "gvVendorItem_" + ctlID + "_lblunit"))
                                                    {
                                                        string iRemarks = iAddRemarks + Environment.NewLine + convert.ToString(iObj.LineItemComment.Value).Trim();
                                                        iRemarks = iRemarks.Trim().Replace(Environment.NewLine, "<br>");
                                                        iRemarks = iRemarks.Replace("\r\n", " ");
                                                        iRemarks = iRemarks.Replace("'", "");
                                                        HtmlNode _inpRemarks = _httpWrapper.GetElement("input", "id", "gvVendorItem_" + ctlID + "_imgVendorNotes"); //_tr.ChildNodes[7].ChildNodes[1];
                                                        FillItemRemarks(iRemarks, _inpRemarks, iObj.Number);
                                                        result = true;
                                                    }
                                                    else throw new Exception("Unable to update item " + iObj.Number + ".");
                                                }
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
                        }
                        else
                        {
                            if (_tblItems.InnerText.Trim().Contains("NO RECORDS FOUND"))
                                throw new Exception("No item found on web portal");
                        }
                    }
                    else result = false;
                }
                else result = false;
            }
            catch (Exception ex)
            {
                LogText = "Exception while filling items: " + ex.GetBaseException().Message.ToString();
                throw;
                // result = false;
            }
            return result;
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

        public void FillItemRemarks(string iRemarks, HtmlNode _inpRemarks, string ItemNo)
        {
            if (iRemarks.Trim().Length > 0)
            {
                if (_inpRemarks != null)
                {
                    string onClickEvent = _inpRemarks.GetAttributeValue("onclick", "");
                    int _startIndx = onClickEvent.IndexOf("PurchaseFormItemMoreInfo.aspx?");
                    if (_startIndx > 0)
                    {
                        int _endIndx = onClickEvent.Substring(_startIndx).IndexOf("');");
                        if (_endIndx > 0) URL = onClickEvent.Substring(_startIndx, _endIndx);
                    }

                    if (URL.Trim().Length > 0)
                    {
                        try
                        {
                            URL = OrgConfigURL + URL;// "https://apps.southnests.com/Phoenix/Purchase/"
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
                                    _httpWrapper._AddRequestHeaders.Add("Origin", @"https://apps.southnests.com");
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
                    dctPostDataValues.Add("txtVenderReference", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVenderReference").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtOrderDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtOrderDate").GetAttributeValue("value", "").Trim()));
                    dctPostDataValues.Add("txtPriority", _httpWrapper.GetElement("input", "id", "txtPriority").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("UCDeliveryTerms%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCDeliveryTerms_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("UCPaymentTerms%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='UCPaymentTerms_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("txtDeliveryTime%24txtNumber", _httpWrapper.GetElement("input", "id", "txtDeliveryTime_txtNumber").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("MaskedEditExtender1_ClientState", "");
                    dctPostDataValues.Add("ucModeOfTransport%24ddlQuick", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucModeOfTransport_ddlQuick']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("ddlType%24ddlHard", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ddlType_ddlHard']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("ucCurrency%24ddlCurrency", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='ucCurrency_ddlCurrency']//option[@selected='selected']").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("txtSupplierDiscount", _httpWrapper.GetElement("input", "id", "txtSupplierDiscount").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("txtPrice", _httpWrapper.GetElement("input", "id", "txtPrice").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("txtTotalDiscount", _httpWrapper.GetElement("input", "id", "txtTotalDiscount").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("txtTotalPrice", _httpWrapper.GetElement("input", "id", "txtTotalPrice").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("txtDiscount", _httpWrapper.GetElement("input", "id", "txtDiscount").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("meeDiscount_ClientState", "");
                    dctPostDataValues.Add("gvTax%24ctl03%24txtDescriptionAdd", "");
                    dctPostDataValues.Add("gvTax%24ctl03%24ucTaxTypeAdd%24rblValuePercentage", _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//input[@name='gvTax$ctl03$ucTaxTypeAdd$rblValuePercentage'][@checked='checked']").GetAttributeValue("value", "").Trim());
                    dctPostDataValues.Add("gvTax%24ctl03%24txtValueAdd", "");
                    dctPostDataValues.Add("gvTax%24ctl03%24MaskedEditTotalPayableAmout_ClientState", "");
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
                                    string eFile = ImagePath + "\\Phoenix_" + this.DocType + "Save_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
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
                                                // eFile = ImagePath + "\\Phoenix_" + this.DocType + "Success_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                                                //_httpWrapper._CurrentDocument.DocumentNode.Descendants()
                                                //                    .Where(n => n.Name == "link")
                                                //                    .ToList()
                                                //                    .ForEach(n => n.Remove());

                                                //_CurrentResponseString = _httpWrapper._CurrentDocument.DocumentNode.OuterHtml;
                                                //if (!PrintScreen(eFile)) eFile = "";//
                                                URL = MessageNumber;
                                                if (LoadURL("input", "id", "txtTotalPrice"))
                                                {
                                                    string filename = GetAttachment();
                                                    CreateAuditFile(filename, "Http_Phoenix_processor", UCRefNo, "Uploaded", "Quote for '" + UCRefNo + "' submitted successfully.", BuyerCode, SupplierCode, AuditPath);
                                                }

                                                if (!Directory.Exists(QuotePOCPath + "\\Error")) Directory.CreateDirectory(QuotePOCPath + "\\Error");
                                                File.Move(QuotePOCPath + "\\" + Path.GetFileName(MTML_File), QuotePOCPath + "\\Backup\\" + Path.GetFileName(MTML_File));

                                                if (convert.ToString(ConfigurationManager.AppSettings["SEND_MAIL_FOR_SUBMIT"]).Trim().ToUpper() == "TRUE")
                                                {
                                                    LogText = "Generating mail notification...";
                                                    SendMailNotification(_interchange, "QUOTE", UCRefNo.Trim(), "SUBMITTED", "Quote for REF '" + UCRefNo.Trim() + "' submitted successfully.");
                                                }
                                            }
                                            else WriteErrorLogQuote_With_Screenshot("Unable to get success msg while sending quote for '" + UCRefNo + "'.", MTML_File, "LeS-1008:");
                                        }
                                        else
                                        {
                                            WriteErrorLogQuote_With_Screenshot("Unable to send quote for '" + UCRefNo + "'.", MTML_File, "LeS-1025:");
                                        }
                                    }
                                }
                                else
                                {
                                    //  WriteErrorLogQuote_With_Screenshot("Send button not found on webpage for '" + UCRefNo + "'.", MTML_File);
                                    WriteErrorLogQuote_With_Screenshot("Unable to Save Quote for '" + UCRefNo + "' due to missing controls", MTML_File, "LeS-1008.3:");
                                }
                            }
                            else
                            {
                                WriteErrorLogQuote_With_Screenshot("Unable to Save Quote for '" + UCRefNo + "' due to amount mismatch", MTML_File, "LeS-1008.1:");
                            }
                        }
                        else
                        {
                            WriteErrorLogQuote_With_Screenshot("Unable to Save Quote for '" + UCRefNo + "' since quote total is zero", MTML_File, "LeS-1008.5:");
                        }
                    }
                    else
                    {
                        WriteErrorLogQuote_With_Screenshot("Unable to save quote for '" + UCRefNo + "'.", MTML_File,"LeS-1008:");
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorLogQuote_With_Screenshot("Unable to send quote '" + UCRefNo + "'." + ex.Message, MTML_File, "LeS-1025:");
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
                    URL = OrgConfigURL + URL;// "https://apps.southnests.com/Phoenix/Purchase/"
                   
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
                //WriteErrorLogQuote_With_Screenshot("Unable to decline quote '" + UCRefNo + "'." + ex.Message, MTML_File);
                WriteErrorLogQuote_With_Screenshot("Unable to decline quote for ref no:'" + UCRefNo + "'." + ex.Message, MTML_File, "LeS-1024:");
                return false;
            }
            return result;
        }

        //public void DeclineQuote(string MTML_File)
        //{
        //    try {
        //        LogText = "Decline quote '" + UCRefNo + "'";
        //        HtmlNode declineBtn = _httpWrapper.GetElement("a", "id", "MenuQuotationLineItem_dlstTabs_ctl06_btnMenu");
        //        if (declineBtn != null)
        //        {

        //        }
        //        else { WriteErrorLogQuote_With_Screenshot("Decline button not found on webpage for '" + UCRefNo , MTML_File); }
        //        throw new Exception("Decline quote found.");
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteErrorLogQuote_With_Screenshot("Unable to decline quote '" + VRNO + "'." + ex.Message, MTML_File);
        //    }
        //}

        //public void FillItemRemarks(string iRemarks, HtmlNode _inpRemarks,string ItemNo)
        //{
        //    if (iRemarks.Trim().Length > 0)
        //    {
        //        if (_inpRemarks != null)
        //        {
        //            string onClickEvent = _inpRemarks.GetAttributeValue("onclick", "");
        //            int _startIndx = onClickEvent.IndexOf("PurchaseFormItemMoreInfo.aspx?");
        //            if (_startIndx > 0)
        //            {
        //                int _endIndx = onClickEvent.Substring(_startIndx).IndexOf("');");
        //                if (_endIndx > 0) URL = onClickEvent.Substring(_startIndx, _endIndx);
        //            }

        //            if (URL.Trim().Length > 0)
        //            {
        //                try
        //                {
        //                    URL = "https://apps.southnests.com/Phoenix/Purchase/" + URL;
        //                    URL = URL.Replace("&amp;", "&");
        //                    _httpWrapper.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
        //                    _httpWrapper._AddRequestHeaders.Add("Upgrade-Insecure-Requests", @"1");
        //                    _httpWrapper.AcceptMimeType = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
        //                    _httpWrapper.ContentType = "";
        //                    _httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.CacheControl);
        //                    if (LoadURL("a", "id", "MenuLineItemDetail_dlstTabs_ctl00_btnMenu"))
        //                    {


        //                        var boundary = String.Format("-----------------------------{0:N}", Guid.NewGuid()); //+ DateTime.Now.Ticks.ToString("x").Substring(0, 14);// "------WebKitFormBoundary" + Guid.NewGuid(); //DateTime.Now.Ticks.ToString("X");

        //                        _httpWrapper.ContentType = "multipart/form-data; boundary=" + String.Format("---------------------------{0:N}", Guid.NewGuid());//" + DateTime.Now.ToString("x").Substring(0, 14);
        //                        _httpWrapper.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
        //                        _httpWrapper._SetRequestHeaders[HttpRequestHeader.CacheControl] = "no-cache";
        //                        _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
        //                        if (_httpWrapper._AddRequestHeaders.ContainsKey("Upgrade-Insecure-Requests"))
        //                            _httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");
        //                        //  _httpWrapper._AddRequestHeaders.Add("Upgrade-Insecure-Requests", @"1");
        //                        //  _httpWrapper._SetRequestHeaders[HttpRequestHeader.AcceptLanguage] = "en-US,en;q=0.9";

        //                        string postData = boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ToolkitScriptManager1_HiddenField\"" + Environment.NewLine + Environment.NewLine + Environment.NewLine +
        //                            boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__EVENTTARGET\"" + Environment.NewLine + Environment.NewLine + "MenuLineItemDetail$dlstTabs$ctl00$btnMenu" +
        //                            Environment.NewLine + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__EVENTARGUMENT\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine +
        //                            boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"_contentChanged_txtItemDetails_ucCustomEditor_ctl02\"" + Environment.NewLine + Environment.NewLine + HttpUtility.UrlDecode(_httpWrapper._dctStateData["_contentChanged_txtItemDetails_ucCustomEditor_ctl02"]) +
        //                            Environment.NewLine + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"_contentForce_txtItemDetails_ucCustomEditor_ctl02\"" + Environment.NewLine + Environment.NewLine + HttpUtility.UrlDecode(_httpWrapper._dctStateData["_contentForce_txtItemDetails_ucCustomEditor_ctl02"]) +
        //                            Environment.NewLine + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"_content_txtItemDetails_ucCustomEditor_ctl02\"" + Environment.NewLine + Environment.NewLine + HttpUtility.HtmlEncode(iRemarks.Trim()) + Environment.NewLine +
        //                            boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"_activeMode_txtItemDetails_ucCustomEditor_ctl02\"" + Environment.NewLine + Environment.NewLine + HttpUtility.UrlDecode(_httpWrapper._dctStateData["_activeMode_txtItemDetails_ucCustomEditor_ctl02"]) +
        //                            Environment.NewLine + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__VIEWSTATE\"" + Environment.NewLine + Environment.NewLine + HttpUtility.UrlDecode(_httpWrapper._dctStateData["__VIEWSTATE"]) + Environment.NewLine + boundary +
        //                            Environment.NewLine + "Content-Disposition: form-data; name=\"__VIEWSTATEGENERATOR\"" + Environment.NewLine + Environment.NewLine + HttpUtility.UrlDecode(_httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]) + Environment.NewLine +
        //                            boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"txtItemDetails$ucCustomEditor$ctl01$ctl47$ctl05\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine +
        //                            boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"txtItemDetails$ucCustomEditor$ctl01$ctl47$ctl07\"" + Environment.NewLine + Environment.NewLine + "_blank" + Environment.NewLine +
        //                            boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"txtItemDetails$btnImageUpload\"; filename=\"\"" + Environment.NewLine + "Content-Type: application/octet-stream" + Environment.NewLine +
        //                            Environment.NewLine + Environment.NewLine + boundary + "--";
        //                        URL = URL.Replace("&amp;", "&");
        //                        if (_httpWrapper.PostURL(URL, postData, "a", "id", "MenuLineItemDetail_dlstTabs_ctl00_btnMenu"))
        //                        {
        //                            LogText = "Error while updating item remarks : item no - " + ItemNo + ".";
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    LogText = "Error while updating item remarks : item no - " + ItemNo + "; " + ex.StackTrace;
        //                }
        //            }
        //        }
        //    }
        //}

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

        //public void WriteErrorLogQuote_With_Screenshot(string AuditMsg, string _File)
        //{
        //    LogText = AuditMsg;
        //    string eFile = ImagePath + "\\Phoenix_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        //    //_httpWrapper._CurrentDocument.DocumentNode.Descendants()
        //    //                    .Where(n => n.Name == "link")
        //    //                    .ToList()
        //    //                    .ForEach(n => n.Remove());
        //    _httpWrapper.GetElement("div", "id", "navigation").Attributes["class"].Remove();

        //    _CurrentResponseString = _httpWrapper._CurrentDocument.DocumentNode.OuterHtml;
        //    if (!PrintScreen(eFile)) eFile = "";
        //    if (VRNO == "") VRNO = UCRefNo;
        //    CreateAuditFile(eFile, "Phoenix_Http_" + this.DocType, VRNO, "Error", AuditMsg, BuyerCode, SupplierCode, AuditPath);

        //    if (!Directory.Exists(QuotePOCPath + "\\Error")) Directory.CreateDirectory(QuotePOCPath + "\\Error");
        //    File.Move(QuotePOCPath + "\\" + Path.GetFileName(_File), QuotePOCPath + "\\Error\\" + Path.GetFileName(_File));
        //}

        public void WriteErrorLogQuote_With_Screenshot(string AuditMsg, string _File, string ErrorNo)
        {
            LogText = AuditMsg;
            string eFile = ImagePath + "\\Phoenix_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            //_httpWrapper._CurrentDocument.DocumentNode.Descendants()
            //                    .Where(n => n.Name == "link")
            //                    .ToList()
            //                    .ForEach(n => n.Remove());
            _httpWrapper.GetElement("div", "id", "navigation").Attributes["class"].Remove();

            _CurrentResponseString = _httpWrapper._CurrentDocument.DocumentNode.OuterHtml;
            if (!PrintScreen(eFile)) eFile = "";
            if (VRNO == "") VRNO = UCRefNo;
            CreateAuditFile(eFile, "Phoenix_Http_" + this.DocType, VRNO, "Error", ErrorNo + AuditMsg, BuyerCode, SupplierCode, AuditPath);

            if (!Directory.Exists(QuotePOCPath + "\\Error")) Directory.CreateDirectory(QuotePOCPath + "\\Error");
            File.Move(QuotePOCPath + "\\" + Path.GetFileName(_File), QuotePOCPath + "\\Error\\" + Path.GetFileName(_File));
        }
        #endregion

        #region POC

        public void ProcessPOC(string MTMLFile)
        {
            try
            {
                if (IsDecline)
                {
                    LogText = "PO '" + UCRefNo + "' is declined.";
                    //CreateAuditFile(Path.GetFileName(MTMLFile), "Phoenix_Http_POC", UCRefNo, "Error", "Unable to confirm PO. PO '" + VRNO + "' is declined.", BuyerCode, SupplierCode, AuditPath);
                    CreateAuditFile(Path.GetFileName(MTMLFile), "Phoenix_Http_POC", UCRefNo, "Error", "LeS-1004.7:Unable to process " + this.DocType + " file for '" + VRNO + "' since it is declined.", BuyerCode, SupplierCode, AuditPath);                
                    MoveFiles(MTMLFile, Path.GetDirectoryName(MTMLFile) + "\\Error\\" + Path.GetFileName(MTMLFile));
                }
                else
                {
                    if (convert.ToFloat(GrandTotal) > 0)
                    {
                        UploadPOC(MTMLFile);
                    }
                    else
                    {
                        LogText = "PO amount is 0.";
                        //CreateAuditFile(Path.GetFileName(MTMLFile), "Phoenix_Http_POC", UCRefNo, "Error", "Unable to confirm PO '", BuyerCode, SupplierCode, AuditPath);
                        CreateAuditFile(Path.GetFileName(MTMLFile), "Phoenix_Http_POC", UCRefNo, "Error", "LeS-1004.8:Unable to process PO,amount is zero ", BuyerCode, SupplierCode, AuditPath);
                        MoveFiles(MTMLFile, Path.GetDirectoryName(MTMLFile) + "\\Error\\" + Path.GetFileName(MTMLFile));
//                        1004	Unable to process file

                    }
                }
            }
            catch (Exception ex)
            {
                //WriteErrorLogQuote_With_Screenshot("Exception while  processing POC MTML: " + ex.GetBaseException().Message.ToString(), MTMLFile);
                WriteErrorLogQuote_With_Screenshot("Unable to process file due to " + ex.GetBaseException().Message.ToString(), MTMLFile, "LeS-1004:");
            }
        }

        private void UploadPOC(string MTMLFile)
        {
            string Remarks = "";
            try
            {
                if (MessageNumber.Trim().Contains("PurchaseVendorRemark"))
                {
                    URL = MessageNumber;
                    _httpWrapper.IsUrlEncoded = false;
                    _httpWrapper.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                    _httpWrapper.AcceptMimeType = "text/html, application/xhtml+xml, */*";
                    if (LoadURL("iframe", "id", "ifVendorRemarks"))//
                    {

                        URL =OrgConfigURL +_httpWrapper.GetElement("iframe", "id", "ifVendorRemarks").GetAttributeValue("src", "").Trim();//"https://apps.southnests.com/Phoenix/Purchase/"
                        URL = URL.Replace("&amp;", "&");
                        _httpWrapper.ContentType = "";
                        _httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.CacheControl);
                        if (LoadURL("a", "id", "MenuFormRemarks_dlstTabs_ctl00_btnMenu"))
                        {
                            _httpWrapper.ContentType = "application/x-www-form-urlencoded";
                            if (!_httpWrapper._SetRequestHeaders.ContainsKey(HttpRequestHeader.CacheControl))
                                _httpWrapper._SetRequestHeaders.Add(HttpRequestHeader.CacheControl, "no-cache");
                            _httpWrapper._AddRequestHeaders.Remove("Upgrade-Insecure-Requests");


                            dctPostDataValues.Clear();
                            dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", "");
                            dctPostDataValues.Add("__EVENTTARGET", "MenuFormRemarks%24dlstTabs%24ctl00%24btnMenu");
                            dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
                            dctPostDataValues.Add("_contentChanged_txtRemarks_ctl02", ""); ;
                            dctPostDataValues.Add("_contentForce_txtRemarks_ctl02", "1");
                            if (AAGRefNo != "")
                                Remarks = "POC Ref No.: " + AAGRefNo + "; " + SupplierComment.Trim();
                            dctPostDataValues.Add("_content_txtRemarks_ctl02", Uri.EscapeDataString(Remarks).Replace("%20", "+"));
                            dctPostDataValues.Add("_activeMode_txtRemarks_ctl02", "0");
                            dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                            dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                            dctPostDataValues.Add("__SCROLLPOSITIONX", "0");
                            dctPostDataValues.Add("__SCROLLPOSITIONY", "0");
                            dctPostDataValues.Add("txtDeliveryDate", Uri.EscapeDataString(Convert.ToDateTime(DtDelvDate).ToString("dd/MMM/yyyy")));
                            if (PostURL("span", "id", "ucStatus_litMessage"))
                            {
                                string eFile = ImagePath + "\\Phoenix_POC_" + UCRefNo.Replace("\\", "_") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                                //_httpWrapper._CurrentDocument.DocumentNode.Descendants()
                                //                    .Where(n => n.Name == "link")
                                //                    .ToList()
                                //                    .ForEach(n => n.Remove());
                                _httpWrapper.GetElement("div", "id", "navigation").Attributes["class"].Remove();

                                _CurrentResponseString = _httpWrapper._CurrentDocument.DocumentNode.OuterHtml;
                                if (!PrintScreen(eFile)) eFile = "";
                                LogText = "Purchase Order '" + UCRefNo + "' confirmed successfully.";
                                CreateAuditFile(eFile, "Phoenix_Http_POC", UCRefNo, "Confirmed", "Purchase Order '" + UCRefNo + "' confirmed successfully.", BuyerCode, SupplierCode, AuditPath);
                                MoveFiles(MTMLFile, Path.GetDirectoryName(MTMLFile) + "\\Backup\\" + Path.GetFileName(MTMLFile));

                            }
                        }
                        else throw new Exception("Save button not found on webpage");
                    }
                    else throw new Exception("Unable to get url to navigate.");
                }
                else
                {
                    throw new Exception("link not found in MsgNo");
                }
                _httpWrapper.IsUrlEncoded = true;
            }
            catch (Exception ex)
            {
                LogText = "Erxception at UploadPOC: " + ex.GetBaseException().Message.ToString();
                //throw;
            }
        }
        #endregion

        private string GetPDFText(string FileName)
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
                extractedText = extractedText.Replace("\0", " ").Replace("\r\nɮ", "").Replace("ɮ", "");

                return extractedText.Trim();
            }
            catch (Exception ex) { throw ex; }
        }

        private string GetOriginalEmailFile(string mailTextFile)
        {
            try
            {
                string cEMLPath = "", cEMLFile = "";
                string[] slFname = mailTextFile.Split('_');

                if (slFname.Length > 1)
                {
                    int counter = 1;
                    bool mailFound = false;

                    if (System.Configuration.ConfigurationManager.AppSettings["ESUPPLIER_EMAIL_FILES"] != null)
                    {
                        cEMLPath = System.Configuration.ConfigurationManager.AppSettings["ESUPPLIER_EMAIL_FILES"];
                    }

                    while (counter < (slFname.Length))
                    {
                        cEMLFile = slFname[counter - 1] + "_" + slFname[counter] + ".eml";
                        if (File.Exists(cEMLPath + "\\" + cEMLFile))
                        {
                            mailFound = true;
                        }
                        else
                        {
                            cEMLFile = slFname[counter - 1] + "_" + slFname[counter] + ".msg";
                            if (File.Exists(cEMLPath + "\\" + cEMLFile))
                            {
                                mailFound = true;
                            }
                        }
                        if (mailFound) break;
                        counter++;
                    }
                }

                if (cEMLFile.Length > 0) return Path.GetFileName(cEMLFile);
                else return "";
            }
            catch (Exception ex)
            {
                LogText = "Error while searching Msg/Eml file. Error -" + ex.Message;
                return "";
            }
        }

        public void MoveInvalidFilesToFolder()
        {
            RichTextBox txt = new RichTextBox();
            string skipTextFiles = convert.ToString(ConfigurationManager.AppSettings["SKIP_TEXT_FILES"]);
            string[] texttoCheck = skipTextFiles.Trim().Split(',');
            List<string> lstText = new List<string>();
            lstText.AddRange(texttoCheck);

            string[] pdfFiles = Directory.GetFiles(LinkPath, "*.pdf");
            foreach (string pdfFile in pdfFiles)
            {
                if (File.Exists(pdfFile))
                {
                    bool textFound = false;
                    txt.Text = GetPDFText(pdfFile);
                    if (txt.Lines.Length > 0 && txt.Text.Contains("PURCHASE ORDER"))
                    {
                        string line = txt.Lines[0].Trim();
                        foreach (string sText in lstText)
                        {
                            if (line.ToUpper().Trim().Contains(sText.Trim().ToUpper()))
                            {
                                textFound = true;
                                break;
                            }
                        }
                    }
                    else
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

            // Move Image files //
            string[] imgFiles = Directory.GetFiles(LinkPath, "*.jpg");
            foreach (string imgFile in imgFiles) { try { File.Move(imgFile, LinkPath + "\\InvalidFiles\\" + Path.GetFileName(imgFile)); } catch { } }

            imgFiles = Directory.GetFiles(LinkPath, "*.png");
            foreach (string imgFile in imgFiles) { try { File.Move(imgFile, LinkPath + "\\InvalidFiles\\" + Path.GetFileName(imgFile)); } catch { } }
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
                    //this.URL = HttpUtility.HtmlDecode(_arrClick[_arrClick.Length - 1].Trim('\'').Replace("../", "https://apps.southnests.com/Phoenix/"));
                    //try
                    //{
                    //    if (LoadURL("a", "id", "OrderExportToPDF_dlstTabs_ctl02_btnMenu"))
                    //    {
                    //        HtmlNode _form1 = _httpWrapper.GetElement("form", "id", "form1");
                    //        if (_form1 != null)
                    //        {
                    //            URL = "https://apps.southnests.com/Phoenix/Reports/" + _form1.GetAttributeValue("action", "").Trim();
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
                    //                }
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
                    //                    this.URL = "https://apps.southnests.com/Phoenix/Reports/" + _getdata[_getdata.Length - 1];
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

        public override bool DownloadRFQ(string RequestURL, string DownloadFileName, string ContentType = "Application/pdf")
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

        public void Logout()
        { }

        /* Commented By Sanjita (12-DEC-18) for PO Conversion part */
        //public Dictionary<string, string> GetHeaderData()
        //{
        //    Dictionary<string, string> _xmlHeader = new Dictionary<string, string>();
        //    try
        //    {
        //        LogText = "Reading Header Details ..";

        //        if (this.DocType == "RFQ")
        //        {
        //            //if (IsRFQ)
        //            //{
        //            //}

        //            #region // read RFQ Header map files //
        //            if (File.Exists(MapPath + "\\" + this.DocType + "HEADER_MAP_PHOENIX.txt"))
        //            {
        //                string[] _lines = File.ReadAllLines(MapPath + "\\" + this.DocType + "HEADER_MAP_PHOENIX.txt");
        //                for (int i = 0; i < _lines.Length; i++)
        //                {
        //                    string[] _keys = _lines[i].Split('=');
        //                    if (_keys.Length > 1)
        //                    {
        //                        if (_keys[0] != "DOC_HEADER_VALUE")
        //                        {
        //                            string _value = "";
        //                            if (_keys[1].Split('|')[0] == "input")
        //                            {
        //                                _value = _httpWrapper.GetElement(_keys[1].Split('|')[0], "id", _keys[1].Split('|')[1]).GetAttributeValue("value", "").Trim();
        //                                //PageReference = _value;
        //                            }

        //                            else if (_keys[1].Split('|')[0] == "span" || _keys[1].Split('|')[0] == "textarea")
        //                            {
        //                                _value = _httpWrapper.GetElement(_keys[1].Split('|')[0], "id", _keys[1].Split('|')[1]).InnerText.Trim();
        //                            }
        //                            else if (_keys[1].Split('|')[0] == "select")
        //                            {
        //                                var options = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='" + _keys[1].Split('|')[1] + "']/option[@selected='selected']");
        //                                if (options.Count == 1)
        //                                    _value = (options[0].NextSibling).InnerText.Trim();

        //                            }
        //                            if (String.IsNullOrEmpty(_value)) { _value = string.Empty; }
        //                            _xmlHeader.Add(_keys[0], _value);

        //                        }
        //                        else _xmlHeader.Add(_keys[0], _keys[1]);
        //                    }
        //                }
        //            }
        //            _xmlHeader.Add("DOC_TYPE", this.DocType);
        //            #endregion

        //            string prevURL = "";
        //            #region // Get Details //
        //            HtmlNode _lnkDelInstr = _httpWrapper.GetElement("a", "id", "MenuQuotationLineItem_dlstTabs_ctl02_btnMenu");
        //            if (_lnkDelInstr != null)
        //            {
        //                if (_lnkDelInstr.InnerText.Trim() == "Details")
        //                {
        //                    HtmlNode _eleMenu = _httpWrapper.GetElement("a", "id", "MenuRegistersStockItem_dlstTabs_ctl02_btnMenu");
        //                    if (_eleMenu != null)
        //                    {
        //                        string _click = _eleMenu.GetAttributeValue("onclick", "").Trim();
        //                        string[] _arrClick = _click.Split('=');
        //                        if (_arrClick.Length > 0)
        //                        {
        //                            prevURL = this.URL;
        //                            this.URL = "https://apps.southnests.com/Phoenix/Purchase/PurchaseFormDetail.aspx?orderid=" + _arrClick[_arrClick.Length - 1].Replace("'); return false;", "").Trim() + "&launchedfrom=VENDOR";
        //                            if (LoadURL("input", "id", "_content_txtFormDetails_ucCustomEditor_ctl02", true))
        //                            {
        //                                //HtmlNode _eleRemarks = _httpWrapper.GetElement("input", "id", "_content_txtFormDetails_ucCustomEditor_ctl02");
        //                                //if (_eleRemarks != null)
        //                                //{
        //                                string HeaderDetails = _httpWrapper.GetElement("input", "id", "_content_txtFormDetails_ucCustomEditor_ctl02").GetAttributeValue("value", "").Trim();
        //                                //HeaderDetails = HeaderDetails.Replace("&", " &").Replace("&amp;","&").Replace("&amp;","&").Replace("&nbsp;"," ").Replace("&lt;","<").Replace("&gt;",">").Replace("<br / >","\r\n");

        //                                HeaderDetails = HeaderDetails.Replace(@"&amp;", "&");
        //                                HeaderDetails = HeaderDetails.Replace(@"&lt;", "<");
        //                                HeaderDetails = HeaderDetails.Replace(@"&gt;", ">");
        //                                HeaderDetails = HeaderDetails.Replace(@"&quot;", "\"");
        //                                HeaderDetails = HeaderDetails.Replace(@"&apos;", "'");
        //                                HeaderDetails = HeaderDetails.Replace(@"&amp;", "&");

        //                                if (HeaderDetails.Contains("<!--"))
        //                                    HeaderDetails = Regex.Replace(HeaderDetails, "<!--.*?-->", String.Empty, RegexOptions.Multiline);

        //                                while (HeaderDetails.Trim().Contains("&"))
        //                                {
        //                                    if (HeaderDetails.Trim().Contains("&") && !HeaderDetails.Trim().Contains(" & "))
        //                                    {
        //                                        int count = HeaderDetails.Count(f => f == '&');

        //                                        HeaderDetails = HttpUtility.HtmlDecode(HeaderDetails.Trim());
        //                                        if (count == 1) break;
        //                                    }
        //                                    else break;
        //                                }
        //                                _xmlHeader.Add("HEADER_REMARKS", HeaderDetails.Trim());

        //                                // }
        //                            }
        //                        }
        //                    }
        //                    //HtmlNode _ele = _httpWrapper.GetElement("form", "id", "frmPurchaseQuotationRFQ");
        //                    //if (_ele != null)
        //                    //{
        //                    //    string _action = _ele.GetAttributeValue("action", "").Trim();
        //                    //    this.URL = "https://apps.southnests.com/Phoenix/Purchase/" + _action;
        //                    //    if (LoadURL("input", "id", "txtVessel", true))
        //                    //    {

        //                    //    }
        //                    //}
        //                    ////HtmlNode _eleDetailForm = _httpWrapper.GetElement("form", "id", "frmPurchaseFormDetail");
        //                    //if (_eleDetailForm != null)
        //                    //{
        //                    //    string _action = _eleDetailForm.GetAttributeValue("action", "").Trim();
        //                    //    this.URL="https://apps.southnests.com/Phoenix/Purchase/"+_action;
        //                    //    if(LoadURL("","","",true))
        //                    //    {

        //                    //    }
        //                    //}
        //                }
        //            }
        //            #endregion

        //            if (prevURL != "") this.URL = prevURL;
        //            if (LoadURL("input", "id", "txtnopage", true))
        //            {
        //                this.VRNO = _xmlHeader["VRNO"];
        //                this.VRNO = this.VRNO.Replace("Quotation Details", "").Trim();
        //                this.VRNO = this.VRNO.TrimStart('[').TrimEnd(']').Trim();
        //                _xmlHeader["VRNO"] = this.VRNO;

        //                #region Commented
        //                //#region set buyer code
        //                //Uri _url = new Uri(this.URL);
        //                //string Host = _url.Host;
        //                //if (lstBuyer.ContainsKey(Host))
        //                //{
        //                //    string[] byrInfo = lstBuyer[Host].Trim().Split(',');
        //                //    BuyerCode = convert.ToString(byrInfo[0]).Trim();
        //                //    if (byrInfo.Length > 1) BuyerName = convert.ToString(byrInfo[1]);
        //                //}
        //                //#endregion

        //                //#region set supplier code
        //                //string _vemail = convert.ToString(_xmlHeader["VENDOR_EMAIL"]);
        //                //if (_vemail.Trim().Length > 0)
        //                //{
        //                //    int indx = _vemail.Trim().Split(',')[0].Trim().IndexOf('@');
        //                //    if (indx > 0)
        //                //    {
        //                //        string domain = _vemail.Trim().Split(',')[0].Substring(indx).Trim();
        //                //        if (lstSupp.ContainsKey(domain.Trim().ToLower()))
        //                //        {
        //                //            SupplierCode = convert.ToString(lstSupp[domain.Trim().ToLower()]);
        //                //        }
        //                //        else throw new Exception("'" + domain + "' not found in config file.");
        //                //    }
        //                //}
        //                //else
        //                //{
        //                //    errLog = "Unable to get supplier code.";
        //                //    throw new Exception("Unable to get supplier code.");
        //                //}
        //                //#endregion
        //                #endregion
        //            }
        //        }
        //        else if (this.DocType == "PO")
        //        {
        //            //if (IsPO)
        //            // {
        //            // }

        //            if (_orgDocFile != null)
        //            {
        //                #region // read RFQ Header map files //
        //                if (File.Exists(MapPath + "\\" + this.DocType + "HEADER_MAP_PHOENIX.txt"))
        //                {
        //                    string[] _lines = File.ReadAllLines(MapPath + "\\" + this.DocType + "HEADER_MAP_PHOENIX.txt");
        //                    for (int i = 0; i < _lines.Length; i++)
        //                    {
        //                        string[] _keys = _lines[i].Split('=');
        //                        if (_keys.Length > 1)
        //                        {
        //                            if (_keys[0] != "DOC_HEADER_VALUE")
        //                            {
        //                                string _value = "";
        //                                if (_keys[1].Split('|')[0] == "input")
        //                                {
        //                                    _value = _httpWrapper.GetElement(_keys[1].Split('|')[0], "id", _keys[1].Split('|')[1]).GetAttributeValue("value", "").Trim();
        //                                    //PageReference = _value;
        //                                }

        //                                else if (_keys[1].Split('|')[0] == "span" || _keys[1].Split('|')[0] == "textarea")
        //                                {
        //                                    _value = _httpWrapper.GetElement(_keys[1].Split('|')[0], "id", _keys[1].Split('|')[1]).InnerText.Trim();
        //                                }
        //                                else if (_keys[1].Split('|')[0] == "select")
        //                                {
        //                                    var options = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='" + _keys[1].Split('|')[1] + "']/option[@selected='selected']");
        //                                    if (options.Count == 1)
        //                                        _value = (options[0].NextSibling).InnerText.Trim();
        //                                    if (_value == "--Select--") _value = "";

        //                                }
        //                                if (String.IsNullOrEmpty(_value)) { _value = string.Empty; }
        //                                _xmlHeader.Add(_keys[0], _value);

        //                            }
        //                            else _xmlHeader.Add(_keys[0], _keys[1]);
        //                        }
        //                    }
        //                }
        //                _xmlHeader.Add("DOC_TYPE", this.DocType);
        //                #endregion

        //                #region // Get VRNO, Order Date & Quote Ref. No //
        //                string cOrderDate = "", cVRNO = "", cQuoteRef = "";
        //                int totalLineSearched = 0, msgLines = 0;
        //                using (RichTextBox txt = new RichTextBox())
        //                {
        //                    txt.Text = File.ReadAllText(this.orgEmlFile);
        //                    msgLines = txt.Lines.Length;
        //                    foreach (string line in txt.Lines)
        //                    {
        //                        totalLineSearched++;
        //                        if (line.Trim().Contains("Purchase Order:"))
        //                        {
        //                            if (line.Trim().StartsWith("Purchase Order:")) cVRNO = line.Replace("Purchase Order:", "");
        //                            else
        //                            {
        //                                int indx = line.IndexOf("Purchase Order:");
        //                                if (indx > -1) cVRNO = line.Substring(indx);
        //                                cVRNO = cVRNO.Replace("Purchase Order:", "").Trim();
        //                                indx = cVRNO.Trim().IndexOf(" ");
        //                                if (indx > -1) cVRNO = cVRNO.Trim().Substring(0, indx + 1).Trim();
        //                            }
        //                        }
        //                        else if (line.Trim().StartsWith("Quotation Ref No:"))
        //                        {
        //                            cQuoteRef = line.Replace("Quotation Ref No:", "");
        //                        }
        //                        else if (line.Trim().StartsWith("Ordered Date:"))
        //                        {
        //                            cOrderDate = line.Replace("Ordered Date:", "");
        //                        }
        //                        if (cVRNO.Trim() != "" && cQuoteRef.Trim() != "" && cOrderDate.Trim() != "")
        //                        {
        //                            break;
        //                        }
        //                    }
        //                }
        //                #endregion

        //                // Validate File Values with RFQ Link
        //                if (convert.ToString(_xmlHeader["VRNO"]).Contains(cVRNO.Trim()) && cQuoteRef.Trim() == convert.ToString(_xmlHeader["QUOTE_REF"]))
        //                {
        //                    _xmlHeader["ORDER_DATE"] = cOrderDate.Trim();
        //                    this.VRNO = cVRNO.Trim();
        //                    _xmlHeader["VRNO"] = this.VRNO;
        //                }
        //                else
        //                {
        //                    this.VRNO = cVRNO.Trim();
        //                    if (_xmlHeader.ContainsKey("BUYER_EMAIL"))
        //                    {
        //                        pdfBuyerEmail = convert.ToString(_xmlHeader["BUYER_EMAIL"]);
        //                    }
        //                    if (_xmlHeader.ContainsKey("VENDOR_EMAIL"))
        //                    {
        //                        VendorEmail = convert.ToString(_xmlHeader["VENDOR_EMAIL"]);
        //                    }
        //                    if (_orgDocFile != null)
        //                    {
        //                        _xmlHeader = GetOrderDetails(cVRNO.Trim(), _orgDocFile.FullName, pdfBuyerEmail, VendorEmail.Trim());
        //                    }
        //                    else throw new Exception("Invalid Email");

        //                    if (_xmlHeader["VRNO"].Trim() != "" && _xmlHeader["VRNO"] != cVRNO.Trim().Replace("<br>", ""))
        //                    {
        //                        errLog = "PO details mismatched";
        //                        throw new Exception("PO details mismatched");
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                throw new Exception("Invalid Email");
        //            }
        //        }

        //        #region // Set buyer code //
        //        Uri _url = new Uri(this.URL);
        //        string Host = _url.Host;
        //        if (lstBuyer.ContainsKey(Host))
        //        {
        //            string[] byrInfo = lstBuyer[Host].Trim().Split(',');
        //            BuyerCode = convert.ToString(byrInfo[0]).Trim();
        //            if (byrInfo.Length > 1) BuyerName = convert.ToString(byrInfo[1]);
        //        }
        //        #endregion

        //        #region // Set supplier code //
        //        string _vemail = convert.ToString(_xmlHeader["VENDOR_EMAIL"]);
        //        if (_vemail.Trim().Length > 0)
        //        {
        //            int indx = -1;
        //            if (_vemail.Trim().Contains(',')) indx = _vemail.Trim().Split(',')[0].Trim().IndexOf('@');
        //            else if (_vemail.Trim().Contains(';')) indx = _vemail.Trim().Split(';')[0].Trim().IndexOf('@');
        //            else indx = _vemail.Trim().Split(',')[0].Trim().IndexOf('@');
        //            if (indx > 0)
        //            {
        //                string domain = "";
        //                if (_vemail.Trim().Contains(',')) domain = _vemail.Trim().Split(',')[0].Substring(indx).Trim();
        //                else if (_vemail.Trim().Contains(';')) domain = _vemail.Trim().Split(';')[0].Substring(indx).Trim();
        //                else domain = _vemail.Trim().Split(',')[0].Substring(indx).Trim();

        //                if (lstSupp.ContainsKey(domain.Trim().ToLower()))
        //                {
        //                    SupplierCode = convert.ToString(lstSupp[domain.Trim().ToLower()]);

        //                    //decide supplier from port
        //                    if (ConfigurationManager.AppSettings["SUPPLIER_BY_PORT"].Trim().ToUpper() == "TRUE")
        //                    {
        //                        if (convert.ToString(_xmlHeader["PORT_NAME"]).Trim() != "")
        //                        {
        //                            string PORT = convert.ToString(_xmlHeader["PORT_NAME"]).Trim().ToUpper();
        //                            if (ConfigurationSettings.AppSettings[PORT + "_" + SupplierCode] != null)
        //                            {
        //                                string[] newSuppCode = ConfigurationSettings.AppSettings[PORT + "_" + SupplierCode].Trim().Split('_');
        //                                if (newSuppCode.Length > 1)
        //                                {
        //                                    SupplierCode = newSuppCode[1].Trim().ToUpper();
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                //

        //                //to decide actions
        //                if (ConfigurationManager.AppSettings["ACTIONS"].Trim() != null)
        //                {
        //                    string[] lstActions = ConfigurationManager.AppSettings["ACTIONS"].Trim().Split('|');
        //                    if (lstActions != null)
        //                    {
        //                        foreach (string strActions in lstActions)
        //                        {
        //                            if (strActions.Trim().Contains(SupplierCode))
        //                            {
        //                                if (strActions.Split('=')[1].Contains(","))
        //                                {
        //                                    if (strActions.Split('=')[1].Split(',')[0].ToUpper().Trim() == "RFQ")
        //                                        IsRFQ = true;
        //                                    if (strActions.Split('=')[1].Split(',')[1].ToUpper().Trim() == "PO")
        //                                        IsPO = true;
        //                                }
        //                                else
        //                                {
        //                                    if (strActions.Split('=')[1].ToUpper().Trim() == "RFQ")
        //                                        IsRFQ = true;
        //                                    if (strActions.Split('=')[1].ToUpper().Trim() == "PO")
        //                                        IsPO = true;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                //
        //                // }
        //                // else throw new Exception("'" + domain + "' not found in config file.");
        //            }
        //        }
        //        else
        //        {
        //            errLog = "Unable to get supplier code.";
        //            throw new Exception("Unable to get supplier code.");
        //        }
        //        #endregion

        //        if (!IsRFQ && !IsPO) _xmlHeader = _xmlHeader = new Dictionary<string, string>();
        //        return _xmlHeader;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogText = "Error while reading " + this.DocType + " header details - " + ex.Message;
        //        if (ex.Message.Contains("Timeout while waiting")) moveToError = false;
        //        throw ex;
        //    }
        //}

        #region // Commented Functions //

        //public void SetGUIDs(string GUID)//not required as linkget processed
        //{
        //    using (StreamWriter sw = new StreamWriter(Application.StartupPath + "\\GUID.txt", true))
        //    {
        //        sw.WriteLine(GUID);
        //        sw.Flush();
        //        sw.Close();
        //        sw.Dispose();
        //    }
        //}

        //private string GetPDFText(string FileName)
        //{
        //    string extractedText = "";
        //    try
        //    {
        //        string ext = Path.GetExtension(FileName);
        //        ext = ext.Trim('.').ToLower();
        //        if (ext == "pdf")
        //        {
        //            using (Aspose.Pdf.Document _pdf = new Aspose.Pdf.Document(FileName))
        //            {
        //                TextAbsorber _obj = new TextAbsorber();
        //                _obj.ExtractionOptions = new Aspose.Pdf.Text.TextOptions.TextExtractionOptions(Aspose.Pdf.Text.TextOptions.TextExtractionOptions.TextFormattingMode.Pure);

        //                _pdf.Pages.Accept(_obj);
        //                extractedText = _obj.Text;
        //                _pdf.FreeMemory();
        //                _pdf.Dispose();
        //            }
        //        }
        //        else if (ext == "txt")
        //        {
        //            extractedText = File.ReadAllText(FileName);
        //        }
        //        extractedText = extractedText.Replace("\0", " ");

        //        return extractedText.Trim();
        //    }
        //    catch (Exception ex) { throw ex; }
        //}

        //private List<List<string>> GetRFQItems(Dictionary<string, string> _xmlHeader)
        //{
        //   List<List<string>> lstItems = new List<List<string>>();
        //    HtmlNodeCollection _tr = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='gvVendorItem']//tr[@tabindex='-1']");
        //    if (_tr != null)
        //    {
        //        if (_tr.Count > 0)
        //        {
        //            int Counter = 02, itemCount = 0;
        //            string strCounter = "";
        //            if (Counter < _tr.Count + 1) strCounter = "0" + Counter;
        //            else if (Counter > 10) strCounter = Counter.ToString();
        //            string _i = "gvVendorItem_ctl" + strCounter + "_lblSNo";
        //            HtmlNode itemNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='" + _i + "']");

        //            do
        //            {
        //                if (itemNo != null)
        //                {
        //                    string Pos = "", CompName = "", RefNo = "";
        //                    itemCount++;
        //                    string PartNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblNumber']").InnerText.Trim();
        //                    if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblMakerReference']") != null)
        //                        RefNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblMakerReference']").InnerText.Trim();

        //                    if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblPosition']") != null)
        //                        Pos = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblPosition']").InnerText.Trim();

        //                    string details = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblDetails']").InnerText.Trim();
        //                    string Descr = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//a[@id='gvVendorItem_ctl" + strCounter + "_lnkStockItemCode']").InnerText.Trim();

        //                    if (_httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblComponentName']") != null)
        //                        CompName = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblComponentName']").InnerText.Trim();

        //                    string Qty = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblOrderQuantity']").InnerText.Trim();
        //                    string Unit = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblunit']").InnerText.Trim();

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
        //                                URL = "https://apps.southnests.com/Phoenix/Purchase/" + HttpUtility.HtmlDecode(_onclick[2].Split('\'')[1]);
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
        //                                    // ItemRemarks = Regex.Replace(ItemRemarks, "<.*?>", String.Empty, RegexOptions.Multiline);

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
        //                        PartNo = PartNo.Replace("__.__.__", "");
        //                        if (PartNo.Length > 0)
        //                        {
        //                            ItemRemarks += Environment.NewLine + "Number : " + PartNo;
        //                        }

        //                        if (Pos != null && Pos != "/")
        //                        {
        //                            details += Environment.NewLine + "Drawing No/Position : " + convert.ToString(Pos);
        //                        }

        //                        string EquipName = "";
        //                        if (CompName != "")
        //                            EquipName = convert.ToString(CompName);

        //                        List<string> item = new List<string>();
        //                        item.Add(convert.ToString(itemNo.InnerText.Trim()));
        //                        item.Add(RefNo);
        //                        item.Add(Descr);
        //                        item.Add(Qty);
        //                        item.Add(Unit);
        //                        item.Add("");
        //                        item.Add(details);
        //                        item.Add(EquipName);
        //                        item.Add(ItemRemarks);
        //                        lstItems.Add(item);

        //                        Counter++;

        //                        //DownloadAttachment(_xmlHeader);//commented as by sanjita,because attachment not send to any supplier.//08-03-2018

        //                    }
        //                }

        //                //  URL = currentURL;
        //                //  if (LoadURL("input", "id", "txtnopage", true))
        //                // {
        //                int _totalPages = 0; string _totalItems = "";
        //                if (Counter.ToString().Length == 1) strCounter = "0" + Counter;
        //                else strCounter = Counter.ToString();

        //                if (itemCount >= _tr.Count)
        //                {
        //                    // Check Total Item Count
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
        //                            ItemMismatchScreenShot = ImagePath + "\\Phoenix_" + this.VRNO + "_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
        //                            _httpWrapper.GetElement("div", "id", "navigation").Attributes["class"].Remove();
        //                            _CurrentResponseString = _httpWrapper._CurrentDocument.DocumentNode.OuterHtml;
        //                            if (!PrintScreen(ItemMismatchScreenShot)) ItemMismatchScreenShot = "";
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

        //                    // Move to next Grid Page
        //                    HtmlNode _nextPage = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//a[@id='cmdNext']");
        //                    if (_nextPage != null)
        //                    {
        //                        string _disNext = _nextPage.GetAttributeValue("disabled", "").Trim();
        //                        if (_disNext != "disabled")
        //                        {
        //                            throw new Exception("multiple pages found for item table.");
        //                        }
        //                    }

        //                    // Reset Counter
        //                    Counter = 02; itemCount = 0;
        //                    if (Counter.ToString().Length == 1) strCounter = "0" + Counter;
        //                    else strCounter = Counter.ToString();
        //                }
        //                itemNo = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//span[@id='gvVendorItem_ctl" + strCounter + "_lblSNo']");
        //                //  }
        //            }
        //            while (itemNo != null);
        //        }
        //    }
        //    IsUrlEncoded = true;
        //    return lstItems;
        //}
        //public void DownloadAttachment(Dictionary<string, string> _xmlHeader)
        //{
        //    if (_httpWrapper._dctStateData.Count > 0)
        //    {
        //        //to get attachment url,need to load once again with same url
        //        dctPostDataValues.Clear();
        //        dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", "");
        //        dctPostDataValues.Add("__EVENTTARGET", "MenuQuotationLineItem%24dlstTabs%24ctl05%24btnMenu");
        //        dctPostDataValues.Add("__EVENTARGUMENT", _httpWrapper._dctStateData["__EVENTARGUMENT"]);
        //        dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
        //        dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
        //        dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);
        //        dctPostDataValues.Add("txtVessel", _xmlHeader["VESSEL"].Trim());
        //        dctPostDataValues.Add("txtIMONo", _xmlHeader["IMO"].Trim());
        //        dctPostDataValues.Add("txtHullNo", _xmlHeader["HULLNO"].Trim());
        //        dctPostDataValues.Add("txtVesseltype", _xmlHeader["VESSEL_TYPE"].Trim());
        //        dctPostDataValues.Add("txtYard", _xmlHeader["YARD"].Trim());
        //        dctPostDataValues.Add("txtYearBuilt", Uri.EscapeDataString(_xmlHeader["YEARBUILT"].Trim()));
        //        if (_httpWrapper.GetElement("input", "id", "txtSenderName") != null)
        //            dctPostDataValues.Add("txtSenderName", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtSenderName").GetAttributeValue("value", "")));
        //        else
        //            dctPostDataValues.Add("txtSenderName", "");
        //        dctPostDataValues.Add("txtDeliveryPlace", _xmlHeader["SHIP_ADDRESS1"].Trim());
        //        dctPostDataValues.Add("txtContactNo", _xmlHeader["BUYER_PHONE"].Trim());
        //        dctPostDataValues.Add("txtSenderEmailId", Uri.EscapeDataString(_xmlHeader["BUYER_EMAIL"].Trim()));
        //        dctPostDataValues.Add("txtPortName", _xmlHeader["PORT_NAME"].Trim());
        //        dctPostDataValues.Add("txtSentDate", Uri.EscapeDataString(_xmlHeader["RFQ_DATE"].Trim()));
        //        dctPostDataValues.Add("txtVendorName", Uri.EscapeDataString(_xmlHeader["VENDOR_NAME"].Trim()));
        //        dctPostDataValues.Add("txtTelephone", Uri.EscapeDataString(_xmlHeader["VENDOR_PHONE"].Trim()));
        //        dctPostDataValues.Add("txtFax", Uri.EscapeDataString(_xmlHeader["VENDOR_FAX"].Trim()));
        //        dctPostDataValues.Add("txtVendorAddress", Uri.EscapeDataString(_xmlHeader["VENDOR_ADDRESS1"].Trim()));
        //        dctPostDataValues.Add("txtEmail", Uri.EscapeDataString(_xmlHeader["VENDOR_EMAIL"].Trim()));
        //        if (_httpWrapper.GetElement("input", "id", "txtVenderReference") != null)
        //            dctPostDataValues.Add("txtVenderReference", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtVenderReference").GetAttributeValue("value", "")));
        //        else
        //            dctPostDataValues.Add("txtVenderReference", "");

        //        if (_httpWrapper.GetElement("input", "id", "txtOrderDate") != null)
        //            dctPostDataValues.Add("txtOrderDate", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtOrderDate").GetAttributeValue("value", "")));
        //        else
        //            dctPostDataValues.Add("txtOrderDate", "");

        //        if (_httpWrapper.GetElement("input", "id", "txtPriority") != null)
        //            dctPostDataValues.Add("txtPriority", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtPriority").GetAttributeValue("value", "")));
        //        else
        //            dctPostDataValues.Add("txtPriority", "");

        //        var delOptions = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='UCDeliveryTerms_ddlQuick']/option[@selected='selected']");
        //        if (delOptions.Count == 1)
        //        {
        //            string _DelTerms = delOptions[0].NextSibling.InnerText.Trim();
        //            dctPostDataValues.Add("UCDeliveryTerms%24ddlQuick", _DelTerms);
        //        }
        //        else dctPostDataValues.Add("UCDeliveryTerms%24ddlQuick", "Dummy");

        //        var payOptions = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='UCPaymentTerms_ddlQuick']/option[@selected='selected']");
        //        if (payOptions.Count == 1)
        //        {
        //            string _PayTerms = payOptions[0].NextSibling.InnerText.Trim();
        //            dctPostDataValues.Add("UCPaymentTerms%24ddlQuick", _PayTerms);
        //        }
        //        else dctPostDataValues.Add("UCPaymentTerms%24ddlQuick", "Dummy");

        //        if (_httpWrapper.GetElement("input", "id", "txtDeliveryTime") != null)
        //            dctPostDataValues.Add("txtDeliveryTime", Uri.EscapeDataString(_httpWrapper.GetElement("input", "id", "txtDeliveryTime").GetAttributeValue("value", "")));
        //        else
        //            dctPostDataValues.Add("txtDeliveryTime", "");

        //        dctPostDataValues.Add("MaskedEditExtender1_ClientState", "");

        //        var _modeOptions = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='ucModeOfTransport_ddlQuick']/option[@selected='selected']");
        //        if (_modeOptions.Count == 1)
        //        {
        //            string _modetrans = _modeOptions[0].NextSibling.InnerText.Trim();
        //            dctPostDataValues.Add("ucModeOfTransport%24ddlQuick", _modetrans);
        //        }
        //        else dctPostDataValues.Add("ucModeOfTransport%24ddlQuick", "Dummy");

        //        var _itypeOptions = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='ddlType_ddlHard']/option[@selected='selected']");
        //        if (_itypeOptions.Count == 1)
        //        {
        //            string _itemtype = _itypeOptions[0].NextSibling.InnerText.Trim();
        //            dctPostDataValues.Add("ddlType%24ddlHard", _itemtype);
        //        }
        //        else dctPostDataValues.Add("ddlType%24ddlHard", "Dummy");

        //        var _currOptions = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//select[@id='ucCurrency_ddlCurrency']/option[@selected='selected']");
        //        if (_currOptions.Count == 1)
        //        {
        //            string _currency = _currOptions[0].NextSibling.InnerText.Trim();
        //            dctPostDataValues.Add("ucCurrency%24ddlCurrency", _currency);
        //        }
        //        else dctPostDataValues.Add("ucCurrency%24ddlCurrency", "Dummy");

        //        if (_httpWrapper.GetElement("input", "id", "txtSupplierDiscount") != null)
        //            dctPostDataValues.Add("txtSupplierDiscount", _httpWrapper.GetElement("input", "id", "txtSupplierDiscount").GetAttributeValue("value", "").Trim());

        //        if (_httpWrapper.GetElement("input", "id", "txtPrice") != null)
        //            dctPostDataValues.Add("txtPrice", _httpWrapper.GetElement("input", "id", "txtPrice").GetAttributeValue("value", "").Trim());

        //        if (_httpWrapper.GetElement("input", "id", "txtTotalDiscount") != null)
        //            dctPostDataValues.Add("txtTotalDiscount", _httpWrapper.GetElement("input", "id", "txtTotalDiscount").GetAttributeValue("value", "").Trim());

        //        if (_httpWrapper.GetElement("input", "id", "txtTotalPrice") != null)
        //            dctPostDataValues.Add("txtTotalPrice", _httpWrapper.GetElement("input", "id", "txtTotalPrice").GetAttributeValue("value", "").Trim());

        //        if (_httpWrapper.GetElement("input", "id", "txtDiscount") != null)
        //            dctPostDataValues.Add("txtTotalPrice", _httpWrapper.GetElement("input", "id", "txtDiscount").GetAttributeValue("value", "").Trim());

        //        dctPostDataValues.Add("meeDiscount_ClientState", _httpWrapper._dctStateData["meeDiscount_ClientState"]);

        //        if (_httpWrapper.GetElement("input", "id", "gvTax_ctl03_txtDescriptionAdd") != null)
        //            dctPostDataValues.Add("gvTax%24ctl03%24txtDescriptionAdd", _httpWrapper.GetElement("input", "id", "gvTax_ctl03_txtDescriptionAdd").GetAttributeValue("value", "").Trim());

        //        if (_httpWrapper.GetElement("table", "id", "gvTax_ctl03_ucTaxTypeAdd_rblValuePercentage") != null)
        //        { 
        //        HtmlNodeCollection _eleTaxType==_httpWrapper.cu
        //        }

        //        HtmlNodeCollection _eleScript = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//script[@type='text/javascript']");
        //        foreach (HtmlNode _script in _eleScript)
        //        {
        //            if (_script.InnerText.ToLower().Contains("var dg"))
        //            {

        //            }
        //        }
        //    }
        //    //List<string> _attachments = new List<string>();
        //    //URL = "https://apps.southnests.com/Phoenix/Common/CommonFileAttachment.aspx?DTKEY=0371bfde-1b13-e811-97dd-448a5bae6b1e&MOD=PURCHASE&U=N";
        //    //_httpWrapper.Referrer = currentURL;
        //    //if (LoadURL("table", "id", "gvAttachment",true))
        //    //{
        //    //    HtmlNodeCollection _trAttach = _httpWrapper.GetElement("table", "id", "gvAttachment").SelectNodes("//@ondblclick=''");
        //    //    if (_trAttach.Count > 0)
        //    //    {
        //    //        foreach (HtmlNode _t in _trAttach)
        //    //        {
        //    //            HtmlNodeCollection _td = _t.ChildNodes;
        //    //            if (_td.Count > 0 && _td[3].InnerText.Trim() == "View")
        //    //            {
        //    //                string FileName = _td[2].InnerText.Trim();
        //    //                string FileURL = _td[3].GetAttributeValue("href", "").Trim();
        //    //                _attachments.Add(FileName + "|" + FileURL);
        //    //            }
        //    //        }

        //    //        foreach (string _fileInfo in _attachments)
        //    //        {
        //    //            string[] fileInfo = _fileInfo.Split('|');
        //    //            try
        //    //            {
        //    //                if (DownloadAttachments(fileInfo[1], this.ImagePath + "\\" + fileInfo[0]))
        //    //                {
        //    //                    attachments += "|" + fileInfo[0].Trim();
        //    //                }
        //    //            }
        //    //            catch (Exception)
        //    //            { }
        //    //        }
        //    //    }
        //    //}
        //}
        #endregion
    }
}


#region Commented code


      //public void ProcessLinkFiles()
      //  {
      //      try
      //      {
      //          LoadAppSettings();
      //          SetBuyerSupplierCodes();

      //          LogText = "Phoenix Processor Started.";
      //          if (LinkPath.Trim().Length == 0)
      //          {
      //              errLog = "Link Path not found";
      //              throw new Exception("Link Path not found");
      //          }

      //          string[] _files = Directory.GetFiles(LinkPath, "*.txt");
      //          if (_files.Length > 0)
      //          {
      //              foreach (string sFile in _files)
      //              {
      //                  FileInfo file = new FileInfo(sFile);
      //                  try
      //                  {
      //                      #region /* Copy Link File to Attachment Path */
      //                      if (this.ImagePath.Trim() != "" && Directory.Exists(this.ImagePath))
      //                      {
      //                          file.CopyTo(this.ImagePath + "\\" + file.Name, true);
      //                      }
      //                      #endregion

      //                      this.currentXMLFile = "";
      //                      this.currentLinkFile = file.FullName;
      //                      this.MsgFile = file.Name;

      //                      moveToError = true;
      //                      errLog = "";
      //                      this.orgEmlFile = sFile; // Full path of eml file

      //                      BuyerCode = ""; BuyerName = ""; SupplierCode = ""; VRNO = "";
      //                      LogText = "Processing File " + Path.GetFileName(sFile);

      //                      currentURL = this.URL = GetURL(sFile);

      //                      if (this.URL == "")
      //                      {
      //                          this.URL = GetURLWithHttp(sFile);
      //                      }


      //                      if (this.URL.StartsWith("https://protect-de.mimecast.com") && (this.URL.EndsWith("apps.southnests.com")))//|| this.URL.EndsWith("executiveship.com")//added on 19-6-19 
      //                      {
      //                          if (LoadURL("iframe", "id", "filterandsearch", true))//for rfq
      //                          {
      //                              this.URL = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//form").GetAttributeValue("action","").Trim();
      //                              this.URL = "https://apps.southnests.com/Phoenix/Purchase/" + this.URL;
      //                          }
      //                          else if(LoadURL("iframe","id","ifVendorRemarks",true))//for po
      //                          {
      //                              this.URL = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//form").GetAttributeValue("action", "").Trim();
      //                              this.URL = "https://apps.southnests.com/Phoenix/Purchase/" + this.URL;
      //                          }
      //                       }


      //                      LogText = " URL : " + this.URL;

      //                      //if (!this.URL.Contains("apac01.safelinks.protection.outlook.com"))//commented on 14-2-19
      //                      //{
      //                          if (this.URL != "" && (this.URL.Contains("PurchaseQuotationItems")))
      //                          {
      //                              #region /* Process RFQ */
      //                              this.DocType = "RFQ";
      //                              this.Module = "PHOENIX_RFQ";
      //                              bool isFileProcessed = GetRFQDetails(this.URL);
      //                              if (isFileProcessed)
      //                              {
      //                                  // LogText = "File '" + file.Name + "' is processed successfully.";
      //                                  // move to backup
      //                                  MoveFile(LinkPath + "\\Backup", file);
      //                              }
      //                              else
      //                              {
      //                                  LogText = "Unable to process link file '" + file.Name + "'.";
      //                                  if (moveToError)
      //                                  {
      //                                      MoveFile(LinkPath + "\\Error_files", file);
      //                                  }
      //                                  //CreateAuditFile(file.Name, "Phoenix_Http_RFQ", VRNO.Trim(), "Error", "Unable to process link file.", BuyerCode, SupplierCode, AuditPath);
      //                                  CreateAuditFile(file.Name, "Phoenix_Http_RFQ", VRNO.Trim(), "Error", "LeS-1004:Unable to process file.", BuyerCode, SupplierCode, AuditPath);
      //                              }
      //                              //Unable to process file

      //                              #endregion
      //                          }
      //                          else if (this.URL != "" && this.URL.Contains("PurchaseVendorRemark"))
      //                          {
      //                              if (ConfigurationManager.AppSettings["SUPPLIER_BY_PORT"].Trim().ToUpper() != "TRUE")
      //                              {
      //                                  #region /* Process PO */
      //                                  this.DocType = "PO";
      //                                  this.Module = "PHOENIX_PO";
      //                                  bool isFileProcessed = GetPODetails(this.URL);
      //                                  if (isFileProcessed)
      //                                  {
      //                                      // move to backup
      //                                      MoveFile(LinkPath + "\\Backup", file);
      //                                      MoveFile(_orgDocFile.Directory.FullName + "\\Backup", _orgDocFile);
      //                                  }
      //                                  else
      //                                  {
      //                                      LogText = "Unable to process link file '" + file.Name + "'.";
      //                                      if (moveToError)
      //                                      {
      //                                          MoveFile(LinkPath + "\\Error_files", file);
      //                                      }
      //                                      CreateAuditFile(file.Name, "Phoenix_Http_PO", VRNO.Trim(), "Error", "LeS-1004:Unable to process file.", BuyerCode, SupplierCode, AuditPath);
      //                                  }
      //                                  #endregion
      //                              }
      //                              else
      //                              {
      //                                  #region /* for rms*/
      //                                  string fname = Path.GetFileNameWithoutExtension(file.FullName);
      //                                  if (fname != null)
      //                                  {
      //                                      DirectoryInfo _dInfo = new DirectoryInfo(LinkPath);
      //                                      FileInfo[] filesInDir = _dInfo.GetFiles("*" + fname + "*.*");
      //                                      foreach (FileInfo f in filesInDir)
      //                                      {
      //                                          if (File.Exists(LinkPath + "\\Error_files\\" + Path.GetFileName(f.FullName))) File.Delete(LinkPath + "\\Error_files\\" + Path.GetFileName(f.FullName));
      //                                          File.Move(f.FullName, LinkPath + "\\Error_files\\" + Path.GetFileName(f.FullName));
      //                                          LogText = "Po file " + Path.GetFileName(f.FullName) + " moved to Error_files folder.";
      //                                          LogText = "";
      //                                      }
      //                                  }
      //                                  //  MoveFile(LinkPath + "\\Error_files", file);
      //                                  #endregion
      //                              }
      //                          }
      //                          else
      //                          {
      //                              LogText = "Unable to process link file; Link not found in file " + Path.GetFileName(sFile);
      //                              //CreateAuditFile(file.Name, "Phoenix_Http", VRNO.Trim(), "Error", "Unable to process link file; Link not found in file " + Path.GetFileName(sFile), BuyerCode, SupplierCode, AuditPath);
      //                              CreateAuditFile(file.Name, "Phoenix_Http", VRNO.Trim(), "Error", "LeS-1001:Unable to find URL in file " + Path.GetFileName(sFile), BuyerCode, SupplierCode, AuditPath);                                  
      //                              MoveFile(LinkPath + "\\Error_files", file);
      //                          }
      //                     // }
      //                     //else
      //                     // {
      //                     //     LogText = "Unable to process link file; Link not found in file " + Path.GetFileName(sFile);
      //                     //     CreateAuditFile(file.Name, "Phoenix_Http", VRNO.Trim(), "Error", "Unable to process link file; Link not found in file " + Path.GetFileName(sFile), BuyerCode, SupplierCode, AuditPath);
      //                     //     MoveFile(LinkPath + "\\Error_files", file);
      //                     // }
      //                  }
      //                  catch (Exception ex)
      //                  {
      //                      if (convert.ToString(ex.Message).Trim().ToLower().Contains("could not find a frame or iframe") || convert.ToString(ex.Message).Trim().ToLower().Contains("creating an instance of the com"))
      //                      {
      //                          moveToError = false;
      //                      }
      //                      else moveToError = true;

      //                      // Move to error_files //
      //                      LogText = "Unable to process link file '" + file.Name + "'. Error - " + ex.Message;

      //                      if (this.Module == "PHOENIX_RFQ")
      //                      {
      //                          if (moveToError != false) CreateAuditFile(file.Name, "Phoenix_Http_RFQ", VRNO.Trim(), "Error", "LeS-1004:Unable to process  file '" + file.Name + "'. Error - " + ex.Message, BuyerCode, SupplierCode, AuditPath);
      //                      }
      //                      else
      //                      {
      //                          moveToError = true;
      //                          CreateAuditFile(file.Name, "Phoenix_Http", this.VRNO.Trim(), "Error", "LeS-1004:Unable to process file '" + file.Name + "'. Error -" + ex.Message, BuyerCode, SupplierCode, AuditPath);
      //                      }

      //                      if (moveToError)
      //                      {
      //                          MoveFile(LinkPath + "\\Error_files", file);
      //                      }
      //                      string eFile = ImagePath + "\\Phoenix_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
      //                      //_httpWrapper._CurrentDocument.DocumentNode.Descendants()
      //                      //                    .Where(n => n.Name == "link")
      //                      //                    .ToList()
      //                      //                    .ForEach(n => n.Remove());
      //                      _httpWrapper.GetElement("div", "id", "navigation").Attributes["class"].Remove();

      //                      _CurrentResponseString = _httpWrapper._CurrentDocument.DocumentNode.OuterHtml;
      //                      if (!PrintScreen(eFile)) eFile = "";
      //                  }
      //              }

      //              MoveInvalidFilesToFolder();
      //          }
      //          else LogText = "No files found.";
      //      }
      //      catch (Exception ex)
      //      {
      //          LogText = "Error in ProcessLinkFiles() - " + ex.Message + ex.StackTrace;
      //      }
      //  }



#endregion