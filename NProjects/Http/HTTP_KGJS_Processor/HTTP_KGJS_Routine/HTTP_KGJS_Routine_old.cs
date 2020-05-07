using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Web;

namespace HTTP_KGJS_Routine
{
    public class HTTP_KGJS_Routine_old : LeSCommon.LeSCommon
    {
        string HTMLMailTextPath = "", sAuditMesage = "", VRNO = "", DocType = "", AuditName = "",
            QuoteInbox = "";//, Actions = "";
        int _domainIndex = -1;
        string[] Domains;
        RichTextBox _txtData = new RichTextBox();

        public void LoadAppsettings()
        {
            try
            {
                SessionIDCookieName = "__RequestVerificationToken_L1B1cmNoYXNlUG9ydGFs0";
                HTMLMailTextPath = convert.ToString(ConfigurationManager.AppSettings["HTMLFILEPATH"]).Trim();
                AuditPath = convert.ToString(ConfigurationManager.AppSettings["AUDIT_PATH"]);

                PrintScreenPath = convert.ToString(ConfigurationManager.AppSettings["SCREENSHOT_PATH"]).Trim();
                DownloadPath = convert.ToString(ConfigurationManager.AppSettings["XML_PATH"]).Trim();
                QuoteInbox = convert.ToString(ConfigurationManager.AppSettings["QUOTE_PATH"]).Trim();

                if (PrintScreenPath == "")
                {
                    PrintScreenPath = AppDomain.CurrentDomain.BaseDirectory + "ScreenShots";
                }
                if (AuditPath == "")
                {
                    AuditPath = AppDomain.CurrentDomain.BaseDirectory + "AuditLog";
                }
                if (DownloadPath == "")
                {
                    DownloadPath = AppDomain.CurrentDomain.BaseDirectory + "Download";
                }
                if (QuoteInbox == "")
                {
                    QuoteInbox = AppDomain.CurrentDomain.BaseDirectory + "Quotes";
                }

                if (!Directory.Exists(PrintScreenPath))
                {
                    Directory.CreateDirectory(PrintScreenPath);
                }
                if (!Directory.Exists(AuditPath))
                {
                    Directory.CreateDirectory(AuditPath);
                }
                if (!Directory.Exists(QuoteInbox))
                {
                    Directory.CreateDirectory(QuoteInbox);
                }

                if (!Directory.Exists(HTMLMailTextPath + "\\Backup")) Directory.CreateDirectory(HTMLMailTextPath + "\\Backup");
                if (!Directory.Exists(HTMLMailTextPath + "\\Error")) Directory.CreateDirectory(HTMLMailTextPath + "\\Error");
                this.Domains = ConfigurationManager.AppSettings["DOMAIN"].Split('|');
            }
            catch (Exception e)
            {
                sAuditMesage = "Exception in LoadAppsettings: " + e.GetBaseException().ToString();
                LogText = sAuditMesage;
                CreateAuditFile("", "KGJS_HTTP", VRNO, "Error", sAuditMesage, BuyerCode, SupplierCode, AuditPath);
            }
        }

        #region RFQ
        public void Read_HTMLFiles()
        {
            string filename = "";
            try
            {
                DirectoryInfo _dir = new DirectoryInfo(HTMLMailTextPath);
                if (_dir.GetFiles().Length > 0)
                {
                    FileInfo[] _Files = _dir.GetFiles();
                    foreach (FileInfo _File in _Files)
                    {
                        // Copy file to AuditPath (Added By Sanjita 29-NOV-18) //
                        try
                        {
                            _File.CopyTo(PrintScreenPath + "\\" + _File.Name, true);
                        }
                        catch { LogText = "Unable to copy file " + _File.Name + " to attachment path "; }
                        //

                        filename = _File.FullName;

                        //System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                        URL = GetURL(filename);
                        if (URL != "")
                        {
                            if (_domainIndex >= 0)
                            {
                                LogText = "Processing " + this.DocType + " file ";
                                try
                                {
                                    DownloadRFQ(filename);
                                }
                                catch (Exception e)
                                {
                                    WriteErrorLog_With_Screenshot("Exception while processing file : " + e.GetBaseException().Message.ToString(), filename);
                                    if (File.Exists(HTMLMailTextPath + "\\Error\\" + Path.GetFileName(filename))) File.Delete(HTMLMailTextPath + "\\Error\\" + Path.GetFileName(filename));
                                    File.Move(filename, HTMLMailTextPath + "\\Error\\" + Path.GetFileName(filename));
                                }
                            }
                            else
                            {
                                LogText = "Unable to get domain index for file: " + Path.GetFileName(filename);
                                CreateAuditFile(Path.GetFileName(filename), "KGJS_RFQ", "", "Error", "Unable to get domain index for file: " + Path.GetFileName(filename), BuyerCode, SupplierCode, AuditPath);
                                if (File.Exists(HTMLMailTextPath + "\\Error\\" + Path.GetFileName(filename))) File.Delete(HTMLMailTextPath + "\\Error\\" + Path.GetFileName(filename));
                                File.Move(filename, HTMLMailTextPath + "\\Error\\" + Path.GetFileName(filename));
                            }
                        }
                        else
                        {
                            LogText = "URL not found for navigation in file: " + Path.GetFileName(filename);
                            CreateAuditFile(Path.GetFileName(filename), "KGJS_RFQ", VRNO, "Error", "URL not found for navigation in file: " + Path.GetFileName(filename), BuyerCode, SupplierCode, AuditPath);
                            if (File.Exists(HTMLMailTextPath + "\\Error\\" + Path.GetFileName(filename))) File.Delete(HTMLMailTextPath + "\\Error\\" + Path.GetFileName(filename));
                            File.Move(filename, HTMLMailTextPath + "\\Error\\" + Path.GetFileName(filename));
                        }
                    }
                }
                else LogText = "No file found.";
            }
            catch (Exception e)
            {
                WriteErrorLog_With_Screenshot("Exception while processing file : " + e.GetBaseException().Message.ToString(), filename);
                if (File.Exists(HTMLMailTextPath + "\\Error\\" + Path.GetFileName(filename))) File.Delete(HTMLMailTextPath + "\\Error\\" + Path.GetFileName(filename));
                File.Move(filename, HTMLMailTextPath + "\\Error\\" + Path.GetFileName(filename));
            }
        }

        public void DownloadRFQ(string RFQFile)
        {
            bool _isSameRFQ = false;
            try
            {
                GetAppSettings();
                if (this.DocType == "RFQ")
                {
                    #region # Commented By Sanjita on 29-NOV-18 (To Process each & every html file #
                    //List<string> slProcessedItem = GetProcessedItems(eActions.RFQ, "RFQ");
                    //if (!slProcessedItem.Contains(URL))
                    //{
                    //}
                    //else
                    //{
                    //    LogText = "RFQ for url '" + URL + "' already processed.";
                    //    File.Move(RFQFile, Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile));
                    //}

                    //  URL = URL.Replace("login/token", "Query/Page").Trim();
                    //  _httpWrapper._SetRequestHeaders[HttpRequestHeader.Cookie] = @"__RequestVerificationToken_L1B1cmNoYXNlUG9ydGFs0=" + _httpWrapper._dctSetCookie[SessionIDCookieName];
                    #endregion

                    _httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.CacheControl);
                    _httpWrapper.ContentType = "";
                    if (LoadURL("input", "id", "submitQuery"))
                    {
                        HtmlNodeCollection _h3 = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//h3");
                        foreach (HtmlNode _node in _h3)
                        {
                            if (_node.InnerText.Trim().ToUpper().Contains("QUERY NO"))
                            {
                                if (VRNO == _node.InnerText.Split(':')[1].Trim())
                                { _isSameRFQ = true; break; }
                            }
                        }

                        if (_isSameRFQ)
                        {
                            string eFile = PrintScreenPath + "\\" + VRNO.Replace("/", "_") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
                            _httpWrapper._CurrentResponseString = _httpWrapper._CurrentResponseString.Replace("/PurchasePortal/Content/css?v=Ekgo_uTK_kW377Pwqtq3grDemQ-yQ1lpnU2x90Ofb201", PrintScreenPath + "\\Temp\\css.css");
                           // if (!PrintScreen(eFile)) eFile = RFQFile; // Updated By Sanjita on 29-NOV-18 

                            LeSXML.LeSXML _lesXml = new LeSXML.LeSXML();
                            if (GetHeaderDetails(ref _lesXml, eFile))
                            {
                                if (GetItemDetails(ref _lesXml))
                                {
                                    if (_lesXml.LineItems.Count <= 250)
                                    {
                                        if (!PrintScreen(eFile)) eFile = RFQFile; // Updated By Sanjita on 29-NOV-18 
                                    }
                                    else
                                    {
                                        WebBrowser _browser = new WebBrowser();
                                        _browser.Navigate(URL.Trim());
                                        while (_browser.ReadyState != WebBrowserReadyState.Complete) Application.DoEvents();
                                        string attachment = PrintScreenShot(VRNO, _browser);
                                        string rfq = this.PrintScreenPath + "\\KGJS_RFQ_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssff") + ".png";
                                        if (File.Exists(attachment))
                                        {
                                            File.Copy(attachment, rfq, true);
                                        }
                                        if (_browser != null) _browser.Dispose();
                                    }

                                    if (GetAddress(ref _lesXml))
                                    {
                                        // Commented By Sanjita //
                                        //string xmlfile = "RFQ_" + VRNO.Replace("/", "_") + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xml";
                                        string xmlfile = Path.GetFileNameWithoutExtension(RFQFile) + ".xml";

                                        if (_lesXml.Total_LineItems.Length > 0)
                                        {
                                            _lesXml.FileName = xmlfile;
                                            if (!Directory.Exists(DownloadPath)) Directory.CreateDirectory(DownloadPath);
                                            _lesXml.WriteXML();
                                            if (File.Exists(DownloadPath + "\\" + xmlfile))
                                            {
                                                LogText = xmlfile + " downloaded successfully.";
                                                LogText = "";

                                                // Commented By Sanjita (29-NOV-18) To download original html file from lesmonitor //
                                                //CreateAuditFile(eFile, "KGJS_RFQ", VRNO, "Downloaded", xmlfile + " downloaded successfully.", BuyerCode, SupplierCode, AuditPath);
                                                CreateAuditFile(Path.GetFileName(RFQFile), "KGJS_RFQ", VRNO, "Downloaded", "RFQ file " + xmlfile + " for '" + VRNO + "' downloaded successfully.", BuyerCode, SupplierCode, AuditPath);
                                                if (URL.Length > 0) SetGUIDs(URL);
                                                if (File.Exists(RFQFile + "\\Backup")) File.Delete(RFQFile + "\\Backup");
                                                File.Move(RFQFile, Path.GetDirectoryName(RFQFile) + "\\Backup\\" + Path.GetFileName(RFQFile));
                                            }
                                            else
                                            {
                                                LogText = "Unable to create file.";
                                                CreateAuditFile(eFile, "KGJS_RFQ", VRNO, "Error", "Unable to create file.", BuyerCode, SupplierCode, AuditPath);
                                                if (File.Exists(HTMLMailTextPath + "\\Error\\" + Path.GetFileName(RFQFile))) File.Delete(HTMLMailTextPath + "\\Error\\" + Path.GetFileName(RFQFile));
                                                File.Move(RFQFile, HTMLMailTextPath + "\\Error\\" + Path.GetFileName(RFQFile));
                                            }
                                        }
                                        else
                                        {
                                            LogText = "Lineitem count is zero. Unable to create file";
                                            CreateAuditFile(eFile, "KGJS_RFQ", VRNO, "Error", "Lineitem count is zero.Unable to create file.", BuyerCode, SupplierCode, AuditPath);
                                            if (File.Exists(HTMLMailTextPath + "\\Error\\" + Path.GetFileName(RFQFile))) File.Delete(HTMLMailTextPath + "\\Error\\" + Path.GetFileName(RFQFile));
                                            File.Move(RFQFile, HTMLMailTextPath + "\\Error\\" + Path.GetFileName(RFQFile));
                                        }
                                    }
                                    else
                                    {
                                        LogText = "Unable to get address details";
                                        CreateAuditFile(eFile, "KGJS_RFQ", VRNO, "Error", "Unable to get address details", BuyerCode, SupplierCode, AuditPath);
                                    }
                                }
                                else
                                {
                                    LogText = "Unable to get RFQ item details";
                                    CreateAuditFile(eFile, "KGJS_RFQ", VRNO, "Error", "Unable to get RFQ item details", BuyerCode, SupplierCode, AuditPath);
                                }
                            }
                            else
                            {
                                LogText = "Unable to get RFQ header details";
                                CreateAuditFile(eFile, "KGJS_RFQ", VRNO, "Error", "Unable to get RFQ header details", BuyerCode, SupplierCode, AuditPath);
                            }
                        }
                        else
                        {
                            LogText = "VRNo mismatched.";
                            string filename = PrintScreenPath + "\\KGJS_RFQError_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                            CreateAuditFile(filename, "KGJS_RFQ", VRNO, "Error", "VRNo mismatched", BuyerCode, SupplierCode, AuditPath);
                            if (PrintScreen(filename)) filename = "";
                        }
                    }
                    else
                    {
                        HtmlNode _hTitle = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//div[@class='panel-heading']");
                        if (_hTitle != null)
                        {
                            LogText = _hTitle.InnerText.Trim();
                            if (File.Exists(Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile))) File.Delete(Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile));
                            File.Move(RFQFile, Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile));
                            CreateAuditFile(Path.GetFileName(RFQFile), "KGJS_RFQ", VRNO, "Error", _hTitle.InnerText.Trim(), BuyerCode, SupplierCode, AuditPath);
                            string filename = PrintScreenPath + "\\KGJS_RFQError_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                            if (PrintScreen(filename)) filename = "";
                        }
                    }
                }
                else if (this.DocType == "PO")
                {
                    DownloadPO(RFQFile);
                }
            }
            catch (Exception e)
            {
                LogText = "Exception while processing " + this.DocType + " : " + e.GetBaseException().ToString();
            }
        }

        public bool GetHeaderDetails(ref LeSXML.LeSXML _lesXml, string eFile)
        {
            bool isResult = false;
            try
            {
                LogText = "Start Getting Header details";

                _lesXml.DocID = DateTime.Now.ToString("yyyyMMddhhmmss");
                _lesXml.Created_Date = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Doc_Type = this.DocType;
                _lesXml.Dialect = "Kristian Gerhard Jebsen Skipsrederi A/S";
                _lesXml.Version = "1";
                if (this.DocType == "RFQ") _lesXml.Date_Document = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Date_Preparation = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Sender_Code = BuyerCode;
                _lesXml.Recipient_Code = SupplierCode;
                _lesXml.DocReferenceID = VRNO;
                _lesXml.DocLinkID = this.URL;
                if (File.Exists(eFile)) _lesXml.OrigDocFile = Path.GetFileName(eFile);
                _lesXml.Active = "1";

                HtmlNodeCollection _h4 = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//h4");
                if (_h4.Count > 0)
                {
                    foreach (HtmlNode _node in _h4)
                    {
                        if (_node.InnerText.ToUpper().Contains("VESSEL"))
                        {
                            _lesXml.Vessel = _node.InnerText.Split(':')[2].Trim();
                            // break;
                        }

                        if (_node.InnerText.ToUpper().Contains("TITLE"))
                        {
                            _lesXml.Remark_Title = _node.InnerText.Replace("Title:", "").Replace("&nbsp;", "").Trim();
                        }

                        if (_node.InnerText.ToUpper().Contains("ORDER DATE"))
                        {
                            string _docDate = "";
                            if (this.DocType == "PO")
                                _docDate = _node.InnerText.Replace("Order Date:&nbsp;", "").Trim();
                            if (_docDate != "")
                                _lesXml.Date_Document = Convert.ToDateTime(_docDate).ToString("yyyyMMdd");
                        }
                    }
                }

                _lesXml.BuyerRef = VRNO;


                HtmlNode _dl = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//dl[@class='dl-horizontal kgjs-dl-small']");
                if (_dl != null)
                {
                    if (_dl.ChildNodes.Count > 0)
                    {
                        for (int i = 0; i < _dl.ChildNodes.Count; i++)
                        {
                            if (_dl.ChildNodes[i].InnerText.Trim().ToUpper().Contains("DELIVERY PORT"))
                            {
                                i = i + 2;
                                if (_dl.ChildNodes[i].InnerText.Trim() == "") _lesXml.PortName = "";
                                else
                                { _lesXml.PortName = _dl.ChildNodes[i].InnerText.Trim(); }
                            }

                            if (_dl.ChildNodes[i].InnerText.Trim().ToUpper().Contains("DELIVERY DATE"))
                            {
                                i = i + 2;
                                if (_dl.ChildNodes[i].InnerText.Trim() != "")
                                {
                                    DateTime dt = DateTime.MinValue;

                                    string a = _dl.ChildNodes[i].InnerText.Trim();
                                    //DateTime.TryParseExact(a, "dd/MM/yyyy hh:mm:ss", null, DateTimeStyles.None, out dt);
                                    //if (dt != DateTime.MinValue)
                                    //{
                                    //    dt = DateTime.ParseExact(dt.ToShortDateString(), "M/d/yyyy", CultureInfo.InvariantCulture);//dd-MM-yyyy
                                    _lesXml.Date_Delivery = Convert.ToDateTime(a).ToString("yyyyMMdd");//dt.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                                    //  }
                                }
                            }
                        }
                    }
                }

                HtmlNode _currency = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//select[@id='GlobalCurrency']//option[@selected='selected']");
                if (_currency != null)
                    _lesXml.Currency = _currency.GetAttributeValue("value", "");
                else _lesXml.Currency = "";

                HtmlNode _h5 = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//div[@class='col-md-12 text-center']//h5");
                if (_h5 != null)
                {
                    _lesXml.Remark_Header = _h5.InnerText.Trim();
                }
                if (this.DocType == "RFQ")
                {
                    _lesXml.Total_LineItems_Discount = "0";
                    _lesXml.Total_LineItems_Net = "0";
                    _lesXml.Total_Additional_Discount = "0";
                    _lesXml.Total_Freight = "0";
                    _lesXml.Total_Other = "0";
                    _lesXml.Total_Net_Final = "0";
                    isResult = true;
                }
                else if (this.DocType == "PO")
                {
                    HtmlNodeCollection _dl1 = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//dl[@class='dl-horizontal']");
                    if (_dl1 != null)
                    {
                        if (_dl1.Count > 1)
                        {
                            foreach (HtmlNode dd in _dl1)
                            {
                                if (dd.InnerText.ToUpper().Contains("TOTAL"))
                                {
                                    string GrandTotal = dd.InnerText.Trim().Replace("Total : ", "");
                                    if (GrandTotal.Contains(" "))
                                    {
                                        _lesXml.Total_Net_Final = GrandTotal.Split(' ')[1].Trim();
                                        _lesXml.Currency = GrandTotal.Split(' ')[0].Trim();
                                        isResult = true;
                                    }
                                }
                            }
                        }
                        else { isResult = false; LogText = "Not getting grand total"; }
                    }
                }

                LogText = "Getting Header details completed successfully.";
                return isResult;
            }
            catch (Exception ex)
            {
                LogText = "Unable to get header details." + ex.GetBaseException().ToString(); isResult = false;
                return isResult;
            }
        }

        public bool GetItemDetails(ref LeSXML.LeSXML _lesXml)
        {
            bool isResult = false;
            try
            {
                _lesXml.LineItems.Clear();
                LogText = "Start Getting LineItem details";
                HtmlNodeCollection _nodes = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@class='table table-striped']//tr");
                {
                    if (_nodes.Count >= 2)
                    {
                        foreach (HtmlNode _row in _nodes)
                        {
                            LeSXML.LineItem _item = new LeSXML.LineItem();
                            try
                            {
                                HtmlNodeCollection _rowData = _row.ChildNodes;
                                if (_rowData[1].InnerText.Trim() != "Item" && _rowData[3].InnerText.Trim() != "Amount")
                                {
                                    _item.Number = _rowData[1].InnerText.Trim();
                                    _item.OrigItemNumber = _rowData[1].InnerText.Trim();
                                    _item.SystemRef = _rowData[1].InnerText.Trim();
                                    if (this.DocType == "RFQ") _item.OriginatingSystemRef = _rowData[3].ChildNodes[1].GetAttributeValue("id", "").Trim();
                                    _item.Quantity = _rowData[3].ChildNodes[0].InnerText.Trim();
                                    _item.Unit = _rowData[5].InnerText.Trim();
                                    if (_rowData[7].ChildNodes[1] != null)
                                    {
                                        if (_rowData[7].ChildNodes[1].ChildNodes.Count > 0)
                                        {
                                            for (int i = 0; i < _rowData[7].ChildNodes[1].ChildNodes.Count; i++)
                                            {
                                                if (_rowData[7].ChildNodes[1].ChildNodes[i].InnerText.ToUpper().Trim() == "NAME:")
                                                {
                                                    i = i + 2;
                                                    _item.Name = _rowData[7].ChildNodes[1].ChildNodes[i].InnerText.Trim();
                                                }

                                                if (_rowData[7].ChildNodes[1].ChildNodes[i].InnerText.ToUpper().Trim() == "MAKER REF:")
                                                {
                                                    i = i + 2;
                                                    _item.ItemRef = _rowData[7].ChildNodes[1].ChildNodes[i].InnerText.Trim();
                                                    if (this.DocType == "PO") _item.OriginatingSystemRef = _rowData[7].ChildNodes[1].ChildNodes[i].InnerText.Trim();
                                                }
                                                if (_rowData[7].ChildNodes[1].ChildNodes[i].InnerText.ToUpper().Trim() == "COMP MAKER:")
                                                {
                                                    i = i + 2;
                                                    _item.EquipMaker = _rowData[7].ChildNodes[1].ChildNodes[i].InnerText.Trim();
                                                }
                                                if (_rowData[7].ChildNodes[1].ChildNodes[i].InnerText.ToUpper().Trim() == "COMP NAME:")
                                                {
                                                    i = i + 2;
                                                    _item.Equipment = _rowData[7].ChildNodes[1].ChildNodes[i].InnerText.Trim();
                                                }
                                                if (_rowData[7].ChildNodes[1].ChildNodes[i].InnerText.ToUpper().Trim().Contains("TYPE:"))
                                                {
                                                    i = i + 2;
                                                    _item.EquipType = _rowData[7].ChildNodes[1].ChildNodes[i].InnerText.Trim();
                                                }
                                                if (_rowData[7].ChildNodes[1].ChildNodes[i].InnerText.ToUpper().Trim().Contains("SERIAL:"))
                                                {
                                                    i = i + 2;
                                                    if (_rowData[7].ChildNodes[1].ChildNodes[i].InnerText.Trim() != "")
                                                        _item.EquipRemarks = "Serial:" + _rowData[7].ChildNodes[1].ChildNodes[i].InnerText.Trim();
                                                }
                                                if (_rowData[7].ChildNodes[1].ChildNodes[i].InnerText.ToUpper().Trim() == "MAKER:")
                                                {
                                                    i = i + 2;
                                                    if (_rowData[7].ChildNodes[1].ChildNodes[i].InnerText.Trim() != "")
                                                        _item.Remark = "Maker:" + _rowData[7].ChildNodes[1].ChildNodes[i].InnerText.Trim();
                                                }
                                            }
                                        }
                                    }
                                    _item.Discount = "0";
                                    _item.ListPrice = "0";
                                    _item.LeadDays = "0";
                                    _lesXml.LineItems.Add(_item);
                                }
                            }
                            catch (Exception ex)
                            { LogText = ex.GetBaseException().ToString(); }
                        }
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
                LogText = "Unable to get header details." + ex.GetBaseException().ToString();
                return false;
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

                #region Buyer Address
                HtmlNode _buyerTable = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//table[@style='font-weight:bold; font-style: italic']");
                if (_buyerTable != null)
                {
                    _xmlAdd.Qualifier = "BY";
                    _xmlAdd.AddressName = _buyerTable.ChildNodes[1].InnerText.Trim();

                    string _address = "";
                    for (int i = 3; i < _buyerTable.ChildNodes.Count; i++)
                    {
                        if (_buyerTable.ChildNodes[i].InnerText.Trim() != "")
                            _address += _buyerTable.ChildNodes[i].InnerText.Trim() + ",";
                    }
                    _xmlAdd.Address1 = _address.Trim(',');

                } 

                HtmlNode _buyerContact = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//table[@style='float:left; margin-top: 10px; font-style: italic']");
                if (_buyerContact != null)
                {
                    for (int i = 1; i < _buyerContact.ChildNodes.Count; i++)
                    {
                        if (_buyerContact.ChildNodes[i].InnerText.ToUpper().Contains("PHONE"))
                        {
                            _xmlAdd.Phone = _buyerContact.ChildNodes[i].InnerText.Trim().Split(':')[1].Trim();
                        }
                        if (_buyerContact.ChildNodes[i].InnerText.ToUpper().Contains("EMAIL"))
                        {
                            _xmlAdd.eMail = _buyerContact.ChildNodes[i].InnerText.Trim().Split(':')[1].Trim();
                        }
                    }
                }

                HtmlNodeCollection _contact = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//dl[@class='dl-horizontal']");
                if (_contact != null)
                {
                    foreach (HtmlNode _nde in _contact) //added by kalpita on 06/08/2019
                    {
                        if (_nde.ChildNodes.Count > 7)
                        {
                            _xmlAdd.ContactPerson = (_nde.ChildNodes[9] != null) ? _nde.ChildNodes[9].InnerText.Trim() : string.Empty;
                        }
                    }

                   // _xmlAdd.ContactPerson = _contact[1].ChildNodes[9].InnerText.Trim();
                }

                _lesXml.Addresses.Add(_xmlAdd);
                #endregion

                #region Vendor Address
                _xmlAdd = new LeSXML.Address();
                _xmlAdd.Qualifier = "VN";

                HtmlNodeCollection _div = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//div[@class='table-responsive kgjs-adress']");
                if (_div != null)
                {
                    int count = _div.Count; bool res = false;
                    if (count == 4 && this.DocType == "RFQ") res = true;
                    else if (count == 5 && this.DocType == "PO") res = true;
                    if (res)
                    {
                        HtmlNode _vendorTable = null;
                        if (this.DocType == "RFQ") _vendorTable = _div[2].ChildNodes[1];
                        else if (this.DocType == "PO") _vendorTable = _div[3].ChildNodes[1];
                        if (_vendorTable != null)
                        {
                            HtmlNodeCollection _node = _vendorTable.SelectNodes(".//td");
                            if (_node.Count > 0)
                            {
                                _xmlAdd.AddressName = _node[0].InnerText.Trim();
                                _xmlAdd.Address1 = _node[1].InnerText.Trim() + " , " + _node[4].InnerText.Trim() + " , " + _node[5].InnerText.Trim();
                                if (_node[6].InnerText.Trim() != null)
                                {
                                    if (_node[6].InnerText.Trim().ToUpper().Contains("EMAIL"))
                                    {
                                        _xmlAdd.eMail = _node[7].InnerText.Trim();
                                    }
                                }
                                if (_node[8].InnerText.Trim() != null)
                                {
                                    if (_node[8].InnerText.Trim().ToUpper().Contains("PHONE"))
                                    {
                                        _xmlAdd.Phone = _node[9].InnerText.Trim();
                                    }
                                }
                            }
                            _lesXml.Addresses.Add(_xmlAdd);
                        }
                    }
                #endregion

                    #region Invoice Address
                    _xmlAdd = new LeSXML.Address();
                    _xmlAdd.Qualifier = "BA";

                    if (this.DocType == "PO")
                    {
                        HtmlNode _invoiceTable = _div[2].ChildNodes[1];
                        if (_invoiceTable != null)
                        {
                            HtmlNodeCollection _node = _invoiceTable.SelectNodes(".//tr");
                            if (_node.Count > 0)
                            {
                                _xmlAdd.AddressName = _node[0].InnerText.Trim();
                                _xmlAdd.Address1 = _node[1].InnerText.Trim() + " , " + _node[4].InnerText.Trim() + " , " + _node[5].InnerText.Trim();
                                if (_node[7].InnerText.Trim() != null)
                                {
                                    if (_node[7].InnerText.Trim().ToUpper().Contains("EMAIL"))
                                    {
                                        _xmlAdd.eMail = _node[7].InnerText.Trim().Replace("Email:", "").Trim();
                                    }
                                }
                                if (_node[8].InnerText.Trim() != null)
                                {
                                    if (_node[8].InnerText.Trim().ToUpper().Contains("PHONE"))
                                    {
                                        _xmlAdd.Phone = _node[8].InnerText.Trim().Replace("Phone:", "").Trim();
                                    }
                                }
                            }
                            _lesXml.Addresses.Add(_xmlAdd);
                        }
                    }
                    #endregion
                }

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

        private string GetURL(string emlFile)
        {
            bool _isRFQ = false;
            string _url = "";
            try
            {
                LogText = "";
                LogText = "Reading file " + Path.GetFileName(emlFile);
                _txtData.Text = File.ReadAllText(emlFile);
                foreach (string _domain in Domains)
                {
                    if (_txtData.Text.Contains(_domain))
                    {
                        _domainIndex = Array.IndexOf(Domains, _domain);

                        for (int i = 0; i < _txtData.Lines.Length; i++)
                        {
                            string line = _txtData.Lines[i];
                            if (line.Contains("Request For Quotation"))
                            { this.DocType = "RFQ"; _isRFQ = true; }
                            else if (line.Contains("Purchase Order"))
                            { this.DocType = "PO"; }
                            if (_isRFQ)
                            {
                                if (line.Contains(_domain))
                                {
                                    int startIndex = line.IndexOf(_domain);
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
                                    break;
                                }

                                //for vrno
                                if (line.Contains("QUERY NO:"))
                                {
                                    i = i + 3;
                                    VRNO = _txtData.Lines[i].Trim().Split('<')[2];
                                    if (VRNO.Contains('>'))
                                    {
                                        VRNO = VRNO.Split('>')[1].Trim();
                                    }
                                }
                            }
                            else
                            {
                                if (this.DocType == "PO")
                                {
                                    if (line.Contains(_domain))
                                    {
                                        int startIndex = line.IndexOf(_domain);
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
                                        break;
                                    }

                                    //for vrno
                                    if (line.Contains("ORDER NO:"))
                                    {
                                        i = i + 3;
                                        VRNO = _txtData.Lines[i].Trim('\t').Split('<')[3];
                                        if (VRNO.Contains('>'))
                                        {
                                            VRNO = VRNO.Split('>')[1].Trim();
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
                _url = _url.Trim().Replace("&amp;", "&").Trim().TrimStart('<').TrimStart('"').TrimEnd('>').TrimEnd('"').Trim();
                if (_url.Contains(' ')) _url = _url.Split(' ')[0].Trim('.');


            }
            catch (Exception ex)
            {
                LogText = "Exception while getting URL : " + ex.GetBaseException().ToString();
            }
            return _url;
        }

        public void WriteErrorLog_With_Screenshot(string AuditMsg, string Filename)
        {
            LogText = AuditMsg;
            string eFile = PrintScreenPath + "\\KGJS_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
            if (!PrintScreen(eFile)) eFile = "";
            {
                CreateAuditFile(eFile, "KGJS_HTTP_" + this.DocType, VRNO, "Error", AuditMsg, BuyerCode, SupplierCode, AuditPath);
            }
        }

        public List<string> GetProcessedItems(eActions eAction, string doctype)
        {
            string sDoneFile = "";
            List<string> slProcessedItems = new List<string>();
            switch (eAction)
            {
                case eActions.RFQ: sDoneFile = AppDomain.CurrentDomain.BaseDirectory + AuditName + "_" + doctype + "_Downloaded.txt"; break;
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

        public void SetGUIDs(string GUID)
        {
            using (StreamWriter sw = new StreamWriter(@AppDomain.CurrentDomain.BaseDirectory + AuditName + "_" + DocType + "_Downloaded.txt", true))
            {
                sw.WriteLine(GUID);
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
        }

        public void GetAppSettings()
        {
            try
            {
                BuyerCode = ConfigurationManager.AppSettings["BUYER_CODE"].Trim().Split('|')[_domainIndex];
                SupplierCode = ConfigurationManager.AppSettings["SUPPLIER_CODE"].Trim().Split('|')[_domainIndex];
                AuditName = ConfigurationManager.AppSettings["AUDITNAME"].Trim().Split('|')[_domainIndex];
                //Actions = ConfigurationManager.AppSettings["ACTION"].Trim().Split('|')[_domainIndex];
                if (BuyerCode == "" || SupplierCode == "")
                {
                    throw new Exception("BuyerCode or SupplierCode not found in list.");
                }
            }
            catch (Exception)
            { throw; }
        }

        #endregion

        #region PO
        public void DownloadPO(string RFQFile)
        {
            bool _isSamePO = false;
            try
            {
                #region # Commented By Sanjita (To Process each & every html file #
                //List<string> slProcessedItem = GetProcessedItems(eActions.PO, "PO");
                //if (!slProcessedItem.Contains(URL))
                //{
                //}
                //else
                //{
                //    LogText = "PO for url '" + URL + "' already processed.";
                //    if (File.Exists(Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile))) File.Delete(Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile));
                //    File.Move(RFQFile, Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile));
                //}
                #endregion

                _httpWrapper._SetRequestHeaders.Remove(HttpRequestHeader.CacheControl);
                _httpWrapper.ContentType = "";
                if (LoadURL("input", "id", "btnChoiceAccept"))
                {
                    HtmlNodeCollection _h3 = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//h3");
                    foreach (HtmlNode _node in _h3)
                    {
                        if (_node.InnerText.Trim().ToUpper().Contains("ORDER NO"))
                        {
                            if (VRNO == _node.InnerText.Split(':')[1].Trim())
                            { _isSamePO = true; break; }
                        }
                    }

                    if (_isSamePO)
                    {
                        string eFile = PrintScreenPath + "\\KGJ_" + VRNO.Replace("/", "_") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
                        _httpWrapper._CurrentResponseString = _httpWrapper._CurrentResponseString.Replace("/PurchasePortal/Content/css?v=Ekgo_uTK_kW377Pwqtq3grDemQ-yQ1lpnU2x90Ofb201", PrintScreenPath + "\\Temp\\css.css");
                        if (!PrintScreen(eFile)) eFile = Path.GetFileName(RFQFile); // Updated By Sanjita on 29-NOV-18

                        LeSXML.LeSXML _lesXml = new LeSXML.LeSXML();
                        if (GetHeaderDetails(ref _lesXml, eFile))
                        {
                            if (GetItemDetails(ref _lesXml))
                            {
                                if (GetAddress(ref _lesXml))
                                {
                                    string xmlfile = "PO_" + VRNO.Replace("/", "_") + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xml";
                                    if (_lesXml.Total_LineItems.Length > 0)
                                    {
                                        _lesXml.FileName = xmlfile;
                                        if (!Directory.Exists(DownloadPath)) Directory.CreateDirectory(DownloadPath);
                                        _lesXml.WriteXML();
                                        if (File.Exists(DownloadPath + "\\" + xmlfile))
                                        {
                                            LogText = xmlfile + " downloaded successfully.";
                                            LogText = "";

                                            // Commented By Sanjita (29-NOV-18) To download original html file from lesmonitor //
                                            //CreateAuditFile(eFile, "KGJS_PO", VRNO, "Downloaded", xmlfile + " downloaded successfully.", BuyerCode, SupplierCode, AuditPath);
                                            CreateAuditFile(Path.GetFileName(RFQFile), "KGJS_PO", VRNO, "Downloaded", " Order file " + xmlfile + " for '" + VRNO + "' downloaded successfully.", BuyerCode, SupplierCode, AuditPath);
                                            if (URL.Length > 0) SetGUIDs(URL);
                                            if (File.Exists(RFQFile + "\\Backup")) File.Delete(RFQFile + "\\Backup");
                                            File.Move(RFQFile, Path.GetDirectoryName(RFQFile) + "\\Backup\\" + Path.GetFileName(RFQFile));
                                        }
                                        else
                                        {
                                            LogText = "Unable to create file.";
                                            CreateAuditFile(eFile, "KGJS_PO", VRNO, "Error", "Unable to create file.", BuyerCode, SupplierCode, AuditPath);
                                            if (File.Exists(HTMLMailTextPath + "\\Error\\" + Path.GetFileName(RFQFile))) File.Delete(HTMLMailTextPath + "\\Error\\" + Path.GetFileName(RFQFile));
                                            File.Move(RFQFile, HTMLMailTextPath + "\\Error\\" + Path.GetFileName(RFQFile));
                                        }
                                    }
                                    else
                                    {
                                        LogText = "Lineitem count is zero.Unable to create file.";
                                        CreateAuditFile(eFile, "KGJS_PO", VRNO, "Error", "Lineitem count is zero.Unable to create file.", BuyerCode, SupplierCode, AuditPath);
                                        if (File.Exists(HTMLMailTextPath + "\\Error\\" + Path.GetFileName(RFQFile))) File.Delete(HTMLMailTextPath + "\\Error\\" + Path.GetFileName(RFQFile));
                                        File.Move(RFQFile, HTMLMailTextPath + "\\Error\\" + Path.GetFileName(RFQFile));
                                    }
                                }
                                else
                                {
                                    LogText = "Unable to get address details";
                                    CreateAuditFile(eFile, "KGJS_PO", VRNO, "Error", "Unable to get address details", BuyerCode, SupplierCode, AuditPath);
                                }
                            }
                            else
                            {
                                LogText = "Unable to get PO item details";
                                CreateAuditFile(eFile, "KGJS_PO", VRNO, "Error", "Unable to get PO item details", BuyerCode, SupplierCode, AuditPath);
                            }
                        }
                        else
                        {
                            LogText = "Unable to get PO header details";
                            CreateAuditFile(eFile, "KGJS_PO", VRNO, "Error", "Unable to get PO header details", BuyerCode, SupplierCode, AuditPath);
                        }
                    }
                    else
                    {
                        LogText = "VRNo mismatched.";
                        string filename = PrintScreenPath + "\\KGJS_POError_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                        CreateAuditFile(filename, "KGJS_PO", VRNO, "Error", "VRNo mismatched", BuyerCode, SupplierCode, AuditPath);
                        if (PrintScreen(filename)) filename = "";
                    }
                }
                else
                {
                    HtmlNode _hTitle = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//div[@class='panel-heading']");
                    if (_hTitle != null)
                    {
                        LogText = _hTitle.InnerText.Trim();
                        if (File.Exists(Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile))) File.Delete(Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile));
                        File.Move(RFQFile, Path.GetDirectoryName(RFQFile) + "\\Error\\" + Path.GetFileName(RFQFile));
                        CreateAuditFile(Path.GetFileName(RFQFile), "KGJS_RFQ", VRNO, "Error", _hTitle.InnerText.Trim(), BuyerCode, SupplierCode, AuditPath);
                        string filename = PrintScreenPath + "\\KGJS_RFQError_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                        if (PrintScreen(filename)) filename = "";
                    }
                }

            }
            catch (Exception ex)
            {
                LogText = "Exception in DownloadPO: " + ex.GetBaseException().ToString();
            }
        }
        #endregion

            #region # Quote #
        public void UploadFiles()
        {
            try
            {
                DirectoryInfo _quoteDir = new DirectoryInfo(this.QuoteInbox);
                FileInfo[] files = _quoteDir.GetFiles();

                LogText = "Quote processing started.";
                if (files.Length > 0)
                {
                    LogText = files.Length + " Quote files found to process.";
                    for (int i = 0; i < files.Length; i++)
                    {

                        MTML.GENERATOR.MTMLClass _mtml = new MTML.GENERATOR.MTMLClass();
                        MTML.GENERATOR.MTMLInterchange oInterchange = _mtml.Load(files[i].FullName);

                        if (oInterchange != null)
                        {
                            if (convert.ToString(oInterchange.DocumentHeader.DocType).ToUpper() == "QUOTE")
                            {
                                ProcessQuote(files[i], oInterchange);
                            }
                            else
                            {
                                ProcessOrderConfirmation(files[i], oInterchange);
                            }
                        }
                    }
                }
                else LogText = "No quote file found.";
                LogText = "Quote processing stopped.";
            }
            catch (Exception ex)
            {
                LogText = "Error while uploading quote; " + ex.Message;
                LogText = ex.StackTrace;
                //string eFile = PrintScreenPath + "\\KGJS_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                //if (!PrintScreen(eFile)) eFile = "";
                //CreateAuditFile(Path.GetFileName(eFile), "KGJS_HTTP_Quote", VRNO, "Error", "Error while uploading quote '" + ex.Message + "'.", BuyerCode, SupplierCode, AuditPath);
            }
        }

        private void ProcessQuote(FileInfo file, MTML.GENERATOR.MTMLInterchange interchange)
        {
            WebBrowser _browser = new WebBrowser();
            string VRNO = "";
            try
            {
                Dictionary<string, string> dicSubmit = new Dictionary<string, string>();

                bool isDeclined = interchange.DocumentHeader.IsDeclined;
                string URL = convert.ToString(interchange.DocumentHeader.MessageNumber);

                string QuoteRef = "", QuoteRem = "", PaymentTerms = "", TermCond = "", CurrCode = "";
                int leadDays = 0;
                double QuoteAmount = 0, AdditionalDiscount = 0,ItemTotal=0;
                DateTime dtExpiry = DateTime.MinValue, dtDel = new DateTime();

                #region // Set Quote Details //
                for (int i = 0; i < interchange.DocumentHeader.References.Count; i++)
                {
                    if (interchange.DocumentHeader.References[i].Qualifier == MTML.GENERATOR.ReferenceQualifier.UC)
                    {
                        VRNO = convert.ToString(interchange.DocumentHeader.References[i].ReferenceNumber);
                    }
                    else if (interchange.DocumentHeader.References[i].Qualifier == MTML.GENERATOR.ReferenceQualifier.AAG)
                    {
                        QuoteRef = convert.ToString(interchange.DocumentHeader.References[i].ReferenceNumber);
                    }
                }

                for (int i = 0; i < interchange.DocumentHeader.MonetoryAmounts.Count; i++)
                {
                    if (interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MTML.GENERATOR.MonetoryAmountQualifier.GrandTotal_259)
                    {
                        if (QuoteAmount == 0) QuoteAmount = interchange.DocumentHeader.MonetoryAmounts[i].Value;
                    }
                    else if (interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MTML.GENERATOR.MonetoryAmountQualifier.BuyerItemTotal_90)
                    {
                        //QuoteAmount = interchange.DocumentHeader.MonetoryAmounts[i].Value;
                        ItemTotal = interchange.DocumentHeader.MonetoryAmounts[i].Value;//changed by kalpita on 27/08/2019 (Issue with totals)
                    }
                }

                for (int i = 0; i < interchange.DocumentHeader.Comments.Count; i++)
                {
                    if (interchange.DocumentHeader.Comments[i].Qualifier == MTML.GENERATOR.CommentTypes.SUR)
                    {
                        QuoteRem = convert.ToString(interchange.DocumentHeader.Comments[i].Value).Trim();
                    }
                    else if (interchange.DocumentHeader.Comments[i].Qualifier == MTML.GENERATOR.CommentTypes.ZTP)
                    {
                        PaymentTerms = convert.ToString(interchange.DocumentHeader.Comments[i].Value).Trim();
                    }
                    else if (interchange.DocumentHeader.Comments[i].Qualifier == MTML.GENERATOR.CommentTypes.ZTC)
                    {
                        TermCond = convert.ToString(interchange.DocumentHeader.Comments[i].Value).Trim();
                    }
                }

                for (int i = 0; i < interchange.DocumentHeader.DateTimePeriods.Count; i++)
                {
                    if (interchange.DocumentHeader.DateTimePeriods[i].Qualifier == MTML.GENERATOR.DateTimePeroidQualifiers.DeliveryDate_69)
                    {
                        DateTime.TryParseExact(interchange.DocumentHeader.DateTimePeriods[i].Value, "yyyyMMddHHmm", null, DateTimeStyles.None, out dtDel);
                    }
                    else if (interchange.DocumentHeader.DateTimePeriods[i].Qualifier == MTML.GENERATOR.DateTimePeroidQualifiers.ExpiryDate_36)
                    {
                        DateTime.TryParseExact(interchange.DocumentHeader.DateTimePeriods[i].Value, "yyyyMMddHHmm", null, DateTimeStyles.None, out dtExpiry);
                    }
                }

                AdditionalDiscount = convert.ToDouble(interchange.DocumentHeader.AdditionalDiscount);
                leadDays = convert.ToInt(interchange.DocumentHeader.LeadTimeDays);
                CurrCode = convert.ToString(interchange.DocumentHeader.CurrencyCode).Trim();


                if (interchange.Recipient != null)
                    BuyerCode = interchange.Recipient;

                if (interchange.Sender != null)
                    SupplierCode = interchange.Sender;

                // Set Length for Quote Remarks //
                if (QuoteRem.IndexOf("\"WE HEREBY") != 0)
                {
                    QuoteRem = QuoteRem.Substring(0, QuoteRem.IndexOf("\"WE HEREBY"));
                }
                if (QuoteRem.Length > 120) QuoteRem = QuoteRem.Substring(0, 120);

                if (PaymentTerms.Length > 0) PaymentTerms = "Payment Terms : " + PaymentTerms.Trim();
                if (PaymentTerms.Length > 0)
                {
                    // Append To Quote Remarks //
                    if ((QuoteRem.Length + PaymentTerms.Length) > 120)
                    {
                        QuoteRem = QuoteRem.Substring(0, (120 - PaymentTerms.Length));
                    }
                    QuoteRem = QuoteRem + "; " + PaymentTerms;
                    if (QuoteRem.Length > 120) QuoteRem = QuoteRem.Substring(0, 120);
                }
                if (TermCond.Length > 120) TermCond = TermCond.Substring(0, 120);
                #endregion

                  LogText = "Quote processing started for refno: " + VRNO;

                // Fill & Check Quote Amount Before Sending By HTTP //
                _browser.Navigate(URL.Trim());
                while (_browser.ReadyState != WebBrowserReadyState.Complete) Application.DoEvents();

                System.Windows.Forms.HtmlDocument webDoc = _browser.Document;
                if (webDoc != null)
                {
                    if (!webDoc.Body.InnerText.Contains("This Quotation has already been completed") && !webDoc.Body.InnerText.Contains("This Quotation has been Cancelled") )
                    {
                        #region // Fill Header //
                        System.Windows.Forms.HtmlElement eleVessel = webDoc.GetElementById("Details_Vessel");
                        System.Windows.Forms.HtmlElement eleFromNo = webDoc.GetElementById("Details_FormNo");
                        System.Windows.Forms.HtmlElement eleNoteToVendor = webDoc.GetElementById("Details_NoteToVendor");
                        System.Windows.Forms.HtmlElement eleSendDate = webDoc.GetElementById("Details_SentDate");
                        System.Windows.Forms.HtmlElement eleSignatureName = webDoc.GetElementById("Details_SignatureName");
                        System.Windows.Forms.HtmlElement eleSignatureTitle = webDoc.GetElementById("Details_SignatureTitle");
                        System.Windows.Forms.HtmlElement eleSignatureEmail = webDoc.GetElementById("Details_SignatureEmail");
                        System.Windows.Forms.HtmlElement eleSignaturePhone = webDoc.GetElementById("Details_SignaturePhone");

                        System.Windows.Forms.HtmlElement txtQuoteRef = webDoc.GetElementById("Details_VendorQuotationRef");
                        System.Windows.Forms.HtmlElement txtRemarks = webDoc.GetElementById("Details_VendorNotes");
                        System.Windows.Forms.HtmlElement txtTermsCond = webDoc.GetElementById("Details_VendorTerms");
                        System.Windows.Forms.HtmlElement txtOrderBef = webDoc.GetElementById("Details_OrderWithin");
                        System.Windows.Forms.HtmlElement txtQuoteDisc = webDoc.GetElementById("Details_Discount");
                        System.Windows.Forms.HtmlElement ddlCurrency = webDoc.GetElementById("GlobalCurrency");
                        System.Windows.Forms.HtmlElement txtGlobalDisc = webDoc.GetElementById("GlobalDiscount");

                        dicSubmit.Clear();

                        if (txtRemarks == null) throw new Exception("Field 'Details_VendorNotes' not found on site");
                        else
                        {
                            txtRemarks.SetAttribute("value", QuoteRem.Trim());
                            dicSubmit.Add("Details.VendorNotes", Uri.EscapeDataString(QuoteRem.Trim()).Replace("%20", "+"));//HttpUtility.UrlEncode(QuoteRem.Trim()));//added by namrata on 21-12-2018
                        }

                        if (txtTermsCond == null) throw new Exception("Field 'Details_VendorTerms' not found on site");
                        else
                        {
                            txtTermsCond.SetAttribute("value", TermCond.Trim());
                            dicSubmit.Add("Details.VendorTerms", HttpUtility.UrlEncode(TermCond.Trim()));
                        }

                        if (txtQuoteRef == null) throw new Exception("Field 'Details_VendorQuotationRef' not found on site");
                        else
                        {
                            txtQuoteRef.SetAttribute("value", QuoteRef.Trim());
                            dicSubmit.Add("Details.VendorQuotationRef", Uri.EscapeDataString(QuoteRef.Trim()));
                        }

                        if (txtOrderBef == null) throw new Exception("Field 'Details_OrderWithin' not found on site");
                        else
                        {
                            if (dtExpiry == DateTime.MinValue)
                            {
                                dtExpiry = DateTime.Now.AddDays(30);
                            }
                            string currentDateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                            if (!currentDateFormat.Contains("dd")) currentDateFormat = currentDateFormat.Replace("d", "dd");
                            if (!currentDateFormat.Contains("MM")) currentDateFormat = currentDateFormat.Replace("M", "MM");
                            string sDate = dtExpiry.ToString(currentDateFormat);
                            txtOrderBef.SetAttribute("value", sDate.Trim());
                            dicSubmit.Add("Details.OrderWithin", dtExpiry.ToString("yyyy-MM-dd"));
                        }

                        if (txtQuoteDisc == null) throw new Exception("Field 'Details_Discount' not found on site");
                        else
                        {
                            txtQuoteDisc.SetAttribute("value", "0");
                            dicSubmit.Add("Details.Discount", "0");
                        }

                        if (eleNoteToVendor != null) dicSubmit.Add("Details.NoteToVendor", HttpUtility.UrlEncode(convert.ToString(eleNoteToVendor.GetAttribute("value")))); // Deafult Value
                        else throw new Exception("Field 'Details_NoteToVendor' not found on site");

                        if (eleFromNo != null) dicSubmit.Add("Details.FormNo", convert.ToString(eleFromNo.GetAttribute("value"))); // Deafult Value
                        else throw new Exception("Field 'Details_FormNo' not found on site");

                        if (eleVessel != null) dicSubmit.Add("Details.Vessel", Uri.EscapeDataString(convert.ToString(eleVessel.GetAttribute("value"))).Replace("%20", "+"));//HttpUtility.UrlEncode(convert.ToString(eleVessel.GetAttribute("value")))); // Deafult Value//added by namrata on 21-12-2018
                        else throw new Exception("Field 'Details_Vessel' not found on site");

                        if (eleSendDate != null) dicSubmit.Add("Details.SentDate", Uri.EscapeDataString(convert.ToString(eleSendDate.GetAttribute("value"))).Replace("%20", "+"));//HttpUtility.UrlEncode(convert.ToString(eleSendDate.GetAttribute("value")))); // Deafult Value//added by namrata on 21-12-2018
                        else throw new Exception("Field 'Details_SentDate' not found on site");

                        if (eleSignatureName != null) dicSubmit.Add("Details.SignatureName", HttpUtility.UrlEncode(convert.ToString(eleSignatureName.GetAttribute("value")))); // Deafult Value
                        else throw new Exception("Field 'Details_SignatureName' not found on site");

                        if (eleSignatureTitle != null) dicSubmit.Add("Details.SignatureTitle", HttpUtility.UrlEncode(convert.ToString(eleSignatureTitle.GetAttribute("value")))); // Deafult Value
                        else throw new Exception("Field 'Details_SignatureTitle' not found on site");

                        if (eleSignatureEmail != null) dicSubmit.Add("Details.SignatureEmail", Uri.EscapeDataString(convert.ToString(eleSignatureEmail.GetAttribute("value"))).Replace("%20", "+"));//HttpUtility.UrlEncode(convert.ToString(eleSignatureEmail.GetAttribute("value")))); // Deafult Value
                        else throw new Exception("Field 'Details_SignatureEmail' not found on site");

                        if (eleSignaturePhone != null) dicSubmit.Add("Details.SignaturePhone", Uri.EscapeDataString(convert.ToString(eleSignaturePhone.GetAttribute("value"))).Replace("%20", "+"));//HttpUtility.UrlEncode(convert.ToString(eleSignaturePhone.GetAttribute("value")))); // Deafult Value
                        else throw new Exception("Field 'Details_SignaturePhone' not found on site");

                        if (ddlCurrency == null) throw new Exception("Field 'GlobalCurrency' not found on site");
                        else
                        {
                            ddlCurrency.SetAttribute("value", CurrCode.Trim().ToUpper());
                            dicSubmit.Add("GlobalCurrency", CurrCode.Trim());
                        }

                        if (txtGlobalDisc == null) throw new Exception("Field 'GlobalDiscount' not found on site");
                        else
                        {
                            txtGlobalDisc.SetAttribute("value", AdditionalDiscount.ToString("0.00"));
                            dicSubmit.Add("GlobalDiscount", AdditionalDiscount.ToString("0.00"));
                        }
                        #endregion

                        #region // Fill Items //
                        for (int i = 0; i < interchange.DocumentHeader.LineItems.Count; i++)
                        {
                            MTML.GENERATOR.LineItem item = interchange.DocumentHeader.LineItems[i];
                            if (item != null && item.IsExtraItem == 0 && !string.IsNullOrEmpty(item.OriginatingSystemRef))
                            {
                                double price = 0;
                                int iCount = convert.ToInt(item.OriginatingSystemRef.Split('_')[1]);
                                int iNum = convert.ToInt(item.Number);

                                foreach (MTML.GENERATOR.PriceDetails prc in item.PriceList)
                                {
                                    if (prc.TypeQualifier == MTML.GENERATOR.PriceDetailsTypeQualifiers.GRP)
                                    {
                                        price = prc.Value;
                                        break;
                                    }
                                }

                                // How to update item discount ? //

                                System.Windows.Forms.HtmlElement elehidQuantity = webDoc.GetElementById(item.OriginatingSystemRef);
                                System.Windows.Forms.HtmlElement eleQuotationLineId = webDoc.GetElementById("QueryLines_" + iCount + "__QuotationLineId");
                                System.Windows.Forms.HtmlElement eleNotesCondition = webDoc.GetElementById("QueryLines_" + iCount + "__NotesCondition");
                                System.Windows.Forms.HtmlElement eleNotesHeader = webDoc.GetElementById("QueryLines_" + iCount + "__NotesHeader");
                                System.Windows.Forms.HtmlElement eleVendorRefHeader = webDoc.GetElementById("QueryLines_" + iCount + "__VendorRefHeader");
                                System.Windows.Forms.HtmlElement eleVendorRefCondition = webDoc.GetElementById("QueryLines_" + iCount + "__VendorRefCondition");

                                System.Windows.Forms.HtmlElement txtCurrency = webDoc.GetElementById("QueryLines_" + iCount + "__CurrencyLine");
                                System.Windows.Forms.HtmlElement txtUnitPrice = webDoc.GetElementById("QueryLines_" + iCount + "__UnitPriceLine");
                                System.Windows.Forms.HtmlElement txtDiscount = webDoc.GetElementById("QueryLines_" + iCount + "__DiscountLine");
                                System.Windows.Forms.HtmlElement txtDeliveryTime = webDoc.GetElementById("QueryLines_" + iCount + "__DeliveryTimeLine");
                                System.Windows.Forms.HtmlElement txtItemRemarks = webDoc.GetElementById("QueryLines_" + iCount + "__RemarksLine");

                                if (elehidQuantity == null) throw new Exception("Field '" + item.OriginatingSystemRef + "' not found for Item " + iNum + " on site");
                                else dicSubmit.Add(item.OriginatingSystemRef, convert.ToString(elehidQuantity.GetAttribute("value")));

                                if (txtCurrency == null) throw new Exception("Field 'QueryLines_" + iCount + "__CurrencyLine' not found for Item " + iNum + " on site");
                                else
                                {
                                    txtCurrency.SetAttribute("value", CurrCode.Trim());
                                    dicSubmit.Add("QueryLines%5B" + iCount + "%5D.CurrencyLine", CurrCode);
                                }

                                if (txtUnitPrice == null) throw new Exception("Field 'QueryLines_" + iCount + "__UnitPriceLine' not found for Item " + iNum + " on site");
                                else
                                {
                                    txtUnitPrice.SetAttribute("value", price.ToString());
                                    dicSubmit.Add("QueryLines%5B" + iCount + "%5D.UnitPriceLine", price.ToString());
                                }

                                if (txtDiscount == null) throw new Exception("Field 'QueryLines_" + iCount + "__DiscountLine' not found for Item " + iNum + " on site");
                                else
                                {
                                    txtDiscount.SetAttribute("value", AdditionalDiscount.ToString("0.00"));
                                    dicSubmit.Add("QueryLines%5B" + iCount + "%5D.DiscountLine", AdditionalDiscount.ToString("0.00"));
                                }

                                if (txtDeliveryTime == null) throw new Exception("Field 'QueryLines_" + iCount + "__DeliveryTimeLine' not found for Item " + iNum + " on site");
                                else
                                {
                                    txtDeliveryTime.SetAttribute("value", convert.ToInt(item.DeleiveryTime).ToString());
                                    dicSubmit.Add("QueryLines%5B" + iCount + "%5D.DeliveryTimeLine", convert.ToInt(item.DeleiveryTime).ToString());
                                }

                                if (txtItemRemarks == null) throw new Exception("Field 'QueryLines_" + iCount + "__RemarksLine' not found for Item " + iNum + " on site");
                                else
                                {
                                    string itemRemarks = convert.ToString(item.LineItemComment.Value).Trim();
                                    txtItemRemarks.SetAttribute("value", itemRemarks);
                                    dicSubmit.Add("QueryLines%5B" + iCount + "%5D.RemarksLine", Uri.EscapeDataString(itemRemarks.Replace("%20", "+")));//HttpUtility.UrlEncode(itemRemarks));//added by namrata on 21-12-2018
                                }

                                //QueryLines%5B0%5D.QuotationLineId=1002485491
                                if (eleQuotationLineId == null) throw new Exception("Field 'QueryLines_" + iCount + "__QuotationLineId' not found for Item " + iNum + " on site");
                                else
                                {
                                    dicSubmit.Add("QueryLines%5B" + iCount + "%5D.QuotationLineId", convert.ToString(eleQuotationLineId.GetAttribute("value")));
                                }

                                //QueryLines%5B0%5D.NotesCondition=
                                if (eleNotesCondition == null) throw new Exception("Field 'QueryLines_" + iCount + "__NotesCondition' not found for Item " + iNum + " on site");
                                else
                                {
                                    dicSubmit.Add("QueryLines%5B" + iCount + "%5D.NotesCondition", HttpUtility.UrlEncode(convert.ToString(eleNotesCondition.GetAttribute("value"))));
                                }

                                //QueryLines%5B0%5D.NotesHeader=
                                if (eleNotesHeader == null) throw new Exception("Field 'QueryLines_" + iCount + "__NotesHeader' not found for Item " + iNum + " on site");
                                else
                                {
                                    dicSubmit.Add("QueryLines%5B" + iCount + "%5D.NotesHeader", HttpUtility.UrlEncode(convert.ToString(eleNotesHeader.GetAttribute("value"))));
                                }

                                //QueryLines%5B0%5D.VendorRefHeader=
                                if (eleVendorRefHeader == null) throw new Exception("Field 'QueryLines_" + iCount + "__VendorRefHeader' not found for Item " + iNum + " on site");
                                else
                                {
                                    dicSubmit.Add("QueryLines%5B" + iCount + "%5D.VendorRefHeader", HttpUtility.UrlEncode(convert.ToString(eleVendorRefHeader.GetAttribute("value"))));
                                }

                                //QueryLines%5B0%5D.VendorRefCondition=
                                if (eleVendorRefCondition == null) throw new Exception("Field 'QueryLines_" + iCount + "__VendorRefCondition' not found for Item " + iNum + " on site");
                                else
                                {
                                    dicSubmit.Add("QueryLines%5B" + iCount + "%5D.VendorRefCondition", HttpUtility.UrlEncode(convert.ToString(eleVendorRefCondition.GetAttribute("value"))));
                                }
                            }
                            else
                            {
                                if (item == null)
                                {
                                    throw new Exception("Item is null");
                                }
                                else if (string.IsNullOrEmpty(item.OriginatingSystemRef))
                                {
                                    throw new Exception("OriginatingSystemRef is not defined of item " + item.Number);
                                }
                                // Ignore Extra Items //
                            }
                        }
                        #endregion

                        // Click Calculate Button & Verify Quote Amount //
                        HtmlElement btnCalculate = webDoc.GetElementById("btnCalculate");
                        if (btnCalculate != null)
                        {
                            btnCalculate.InvokeMember("click");
                            System.Threading.Thread.Sleep(1000);
                        }

                        bool sendQuote = false;
                        HtmlElement webTotal = webDoc.GetElementById("txtCalculatePrice");
                        string webTotalCurrency = "";
                        if (webTotal != null)
                        {
                            webTotalCurrency = webTotal.GetAttribute("value");
                            double calculatedAmt = convert.ToDouble(webTotalCurrency.Split(' ')[0]);

                            if (Math.Round(calculatedAmt) != Math.Round(QuoteAmount))
                            {
                                double diff = Math.Abs(calculatedAmt - QuoteAmount);
                                if (diff > 1)
                                {
                                    throw new Exception("Quote total mismatched. Web Quote Amount - " + calculatedAmt);
                                }
                            }

                            LogText = "Quote Amount matched";
                            sendQuote = true;
                        }
                        else throw new Exception("Field 'txtCalculatePrice' not found on web site");

                        string attachment = PrintScreenShot(VRNO, _browser);
                        string QuoteBeforeSubmittion = this.PrintScreenPath + "\\KGJS_QuoteBeforeSubmit_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssff") + ".png";
                        if (File.Exists(attachment))
                        {
                            File.Copy(attachment, QuoteBeforeSubmittion, true);
                        }

                        if (sendQuote)
                        {
                            this.URL = URL.Replace("login/token", "Query/Page");//added by namrata on 21-12-18

                            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                            if (LoadURL("input", "id", "submitQuery"))
                            {
                                HtmlAgilityPack.HtmlNode verifyToken = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//input[@name='__RequestVerificationToken']");
                                HtmlAgilityPack.HtmlNode tokenId = _httpWrapper._CurrentDocument.GetElementbyId("tokenId");

                                dctPostDataValues.Clear();
                                dctPostDataValues.Add("__RequestVerificationToken", verifyToken.GetAttributeValue("value", ""));// _httpWrapper._dctStateData["__RequestVerificationToken"]);
                                foreach (KeyValuePair<string, string> pair in dicSubmit)
                                {
                                    dctPostDataValues.Add(pair.Key, pair.Value);
                                }
                                dctPostDataValues.Add("txtCalculatePrice", HttpUtility.UrlEncode(webTotalCurrency));
                                dctPostDataValues.Add("tokenId", tokenId.GetAttributeValue("value", ""));// _httpWrapper._dctStateData["tokenId"]);
                                dctPostDataValues.Add("btnChoice", "Send");
                            }
                            if (!_httpWrapper._AddRequestHeaders.ContainsKey("Origin"))
                            _httpWrapper._AddRequestHeaders.Add("Origin", @"https://external.kgjs.no");
                            _httpWrapper.KeepAlive = true;
                            //_httpWrapper._SetRequestHeaders.Add(HttpRequestHeader.CacheControl,"max-age=0");//added by namrata on 21-12-18
                            //_httpWrapper._AddRequestHeaders.Add("Upgrade-Insecure-Requests", @"1");
                            //_httpWrapper.ContentType = "application/x-www-form-urlencoded";//
                            if (PostURL("input", "value", "Close")) // Search Close button on Screen
                            {
                                string innerContent = convert.ToString(_httpWrapper._CurrentDocument.DocumentNode.InnerText);

                                if (innerContent.ToLower().Contains("query order response delivered successfully"))
                                {
                                    // Set Success Audit log 
                                    LogText = "Quote for RefNo '" + VRNO + "' submitted successfully.";
                                    CreateAuditFile(Path.GetFileName(QuoteBeforeSubmittion), "KGJS_HTTP_Quote", VRNO, "Submitted", "Quote for RefNo '" + VRNO + "' submitted successfully.",
                                        BuyerCode, SupplierCode, AuditPath);
                                    MoveToBackup(file.FullName);
                                }
                                else
                                {
                                    LogText = "Unable to submit quote for RefNo '" + VRNO + "'.";
                                    string eFile = PrintScreenPath + "\\KGJS_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                                    if (!PrintScreen(eFile)) eFile = "";
                                    CreateAuditFile(Path.GetFileName(eFile), "KGJS_HTTP_Quote", VRNO, "Error", "Unable to submit quote for RefNo '" + VRNO + "'.", BuyerCode, SupplierCode, AuditPath);
                                    MoveToError(file.FullName);
                                    // Set Error Log 
                                }
                            }
                            else
                            {
                                // Set Error log 
                                LogText = "Unable to submit quote while posting data for RefNo '" + VRNO + "'.";
                                string eFile = PrintScreenPath + "\\KGJS_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                                if (!PrintScreen(eFile)) eFile = "";
                                CreateAuditFile(Path.GetFileName(eFile), "KGJS_HTTP_Quote", VRNO, "Error", "Unable to submit quote while posting data for RefNo '" + VRNO + "'.", BuyerCode, SupplierCode, AuditPath);
                                MoveToError(file.FullName);
                            }
                        }
                        else
                        {
                            // Dont Set Log here as already quote mismtached error is thrown //
                        }
                    }
                    else
                    {
                        MoveToError(file.FullName);
                        LogText = webDoc.Body.InnerText;
                        throw new Exception(webDoc.Body.InnerText);
                       
                    }
                }
                else
                {
                    MoveToError(file.FullName);
                    LogText = "Unable to load KGJS URL";
                    throw new Exception("Unable to load KGJS URL");
                }
            }
            catch (Exception ex)
            {
                string errorScreenShot = PrintScreenShot(VRNO, _browser);
                CreateAuditFile(Path.GetFileName(errorScreenShot), "KGJS_HTTP_Quote", VRNO, "Error", "Error while uploading quote '" + ex.Message + "'.", BuyerCode, SupplierCode, AuditPath);
                LogText = ex.Message.ToString();
                MoveToError(file.FullName);
            }
            finally
            {
                if (_browser != null) _browser.Dispose();
            }
        }

        private void ProcessOrderConfirmation(FileInfo file, MTML.GENERATOR.MTMLInterchange interchange)
        {

        }

        private void MoveToBackup(string MTML_QuoteFile)
        {
          
            if (!Directory.Exists(Path.GetDirectoryName(MTML_QuoteFile) + "\\Backup")) Directory.CreateDirectory(Path.GetDirectoryName(MTML_QuoteFile) + "\\Backup");

            if (File.Exists(Path.GetDirectoryName(MTML_QuoteFile) + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile)))
            {
                File.Delete(Path.GetDirectoryName(MTML_QuoteFile) + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile));
            }
            File.Move(MTML_QuoteFile, Path.GetDirectoryName(MTML_QuoteFile) + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile));
        }

        private void MoveToError(string FileToMove)
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

        //public override bool PrintScreen(string sFileName)
        //{
        //    if (!Directory.Exists(Path.GetDirectoryName(sFileName) + "\\Temp")) Directory.CreateDirectory(Path.GetDirectoryName(sFileName) + "\\Temp");
        //    sFileName = Path.GetDirectoryName(sFileName) + "\\Temp\\" + Path.GetFileName(sFileName);
        //    if (base.PrintScreen(sFileName))
        //    {
        //        MoveFiles(sFileName, this.PrintScreenPath + "\\" + Path.GetFileName(sFileName));
        //        return (File.Exists(this.PrintScreenPath + "\\" + Path.GetFileName(sFileName)));
        //    }
        //    else return false;
        //}
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

        private string PrintScreenShot(string VRNO, WebBrowser wb)
        {
            try
            {
                string pngFile = this.PrintScreenPath + "\\KGJS_"+this.DocType+"_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".png";

                wb.Width = 1024;
                wb.ScrollBarsEnabled = false;
                wb.ScriptErrorsSuppressed = true;

                wb.Width = wb.Document.Body.ScrollRectangle.Size.Width;
                wb.Height = wb.Document.Body.ScrollRectangle.Size.Height;
                using (Bitmap bitmap = new Bitmap(wb.Width, wb.Height))
                {
                    wb.DrawToBitmap(bitmap, new Rectangle(0, 0, wb.Width, wb.Height));
                    Rectangle rect = new Rectangle(0, 0, wb.Width, wb.Height);
                    Bitmap cropped = bitmap.Clone(rect, bitmap.PixelFormat);
                    cropped.Save(pngFile, ImageFormat.Png);
                }

                if (File.Exists(pngFile))
                {
                    return pngFile;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                LogText = "Unable to Print Screen for quote '" + VRNO + "'; Error - " + ex.Message;
                return "";
            }
        }
    }
}


