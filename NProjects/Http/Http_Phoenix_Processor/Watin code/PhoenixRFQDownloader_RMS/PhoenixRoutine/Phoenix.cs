/* Updates 
*  12-OCT-2016 : Modified to replace 119.81.76.123 to apps.southnests.com domain
****/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using WebScraper;
using WatiN.Core;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Aspose.Pdf.Text;

namespace PhoenixRoutine
{
    public class Phoenix : WebScraper.WebPage
    {
        string VRNO = "", WebHost = "", currentXMLFile = "", BuyerCode = "", BuyerName = "", SuppCode = "", FileType = "", attachments = "", orgEmlFile = "", currCode = "";
        Dictionary<string, string> lstBuyer = new Dictionary<string, string>();
        Dictionary<string, string> lstSupp = new Dictionary<string, string>();
        double CalculatedTotal = 0;
        int pdfItemStartCount = 0;
        FileInfo _orgDocFile = null;
        string errLog = "";
        bool reprocessed = false, moveToError = false;

        public Phoenix()
        {
            try
            {
                WatiN.Core.Settings.AttachToBrowserTimeOut = 300;
                WatiN.Core.Settings.WaitForCompleteTimeOut = 120;

                Aspose.Pdf.License _lic = new Aspose.Pdf.License();
                _lic.SetLicense("Aspose.Total.lic");
            }
            catch (Exception ex)
            {
                this.SetLog(this.CurrentTimeStamp + " : Error in Phoenix() - " + ex.StackTrace);
            }
        }

        private bool IsPhoenixExeRunning()
        {
            try
            {
                int count = 0;
                System.Diagnostics.Process[] Prcs = System.Diagnostics.Process.GetProcesses();
                foreach (Process prc in Prcs)
                {
                    try
                    {
                        if (Process.GetCurrentProcess().Id != prc.Id)
                        {
                            string prdName = prc.MainModule.FileVersionInfo.ProductName;
                            if (prdName.ToString().ToUpper().Trim() == "WEBSCRAPPER")
                            {
                                count++;
                            }
                        }
                    }
                    catch { }
                }

                if (count > 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void KillIE()
        {
            try
            {
                Thread.Sleep(5000);
                System.Diagnostics.Process[] Prcs = System.Diagnostics.Process.GetProcesses();
                foreach (Process prc in Prcs)
                {
                    try
                    {
                        string prdName = prc.ProcessName;
                        if (prdName.Trim() == "iexplore")
                        {
                            prc.Kill();
                            this.SetLog(this.CurrentTimeStamp + " : 1 instance of ie is killed.");
                            Thread.Sleep(2000);
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void SetBuyerSupplierCodes()
        {
            try
            {
                string[] _bvalues = convert.ToString(ConfigurationSettings.AppSettings["BUYER_CODE"]).Trim().Split('|');
                string[] _svalues = convert.ToString(ConfigurationSettings.AppSettings["SUPP_CODE"]).Trim().Split('|');

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
                            // UPDATED ON 26-APR-16 set email with lower characters
                            lstSupp.Add(arr[0].Trim().ToLower(), arr[1]);
                        }
                    }
                }
            }
            catch { }
        }

        public void ProcessLinkFiles()
        {
            bool exeInRunningState = false;
            try
            {
                if (!IsPhoenixExeRunning())
                {
                    SetBuyerSupplierCodes(); // Set Buyer & Supplier Links

                    SetLog(this.CurrentTimeStamp + " : Phoenix RFQ & PO Downloader Started.");

                    string LinkPath = convert.ToString(ConfigurationSettings.AppSettings["LINK_PATH"]);
                    if (LinkPath.Trim().Length == 0)
                    {
                        errLog = "Link Path not found";
                        throw new Exception("Link Path not found");
                    }
                    string[] _files = Directory.GetFiles(LinkPath, "*.txt");

                    foreach (string sFile in _files)
                    {
                        moveToError = true;
                        errLog = "";
                        this.orgEmlFile = sFile; // Full path of eml file

                        BuyerCode = ""; BuyerName = ""; SuppCode = ""; VRNO = ""; attachments = "";
                        FileInfo file = new FileInfo(sFile);

                        SetLog(this.CurrentTimeStamp + " : Processing File " + Path.GetFileName(sFile));
                        Console.WriteLine(this.CurrentTimeStamp + " : Processing File " + Path.GetFileName(sFile));
                        this._URL = GetURL(sFile);

                        if (this._URL == "")
                        {
                            this._URL = GetURLWithHttp(sFile);
                        }

                        Console.WriteLine(this.CurrentTimeStamp + " : URL : " + this._URL);

                        if (this._URL != "" && this._URL.Contains("PurchaseQuotationItems"))
                        {
                            this.DocType = "RFQ";
                            this.Module = "PHOENIX_RFQ";
                            bool isFileProcessed = GetRFQDetails(this._URL);

                            if (isFileProcessed)
                            {
                                Console.WriteLine(this.CurrentTimeStamp + " : File is processed successfully.");

                                // move to backup
                                MoveFile(LinkPath + "\\Backup", file);
                                Console.WriteLine(this.CurrentTimeStamp + " : RFQ '" + VRNO.Trim() + "' downloaded successfully.");
                            }
                            else
                            {
                                // move to error_files
                                Console.WriteLine(this.CurrentTimeStamp + " :  Unable to process link file '" + file.Name + "'.");
                                this.SetLog(this.CurrentTimeStamp + " : Unable to process link file '" + file.Name + "'.");
                                if (moveToError)
                                {
                                    MoveFile(LinkPath + "\\Error_files", file);
                                }
                                AddToAudit(BuyerCode, SuppCode, this.Module, file.Name, VRNO.Trim(), "Error", "Unable to process link file.");
                            }
                        }
                        else if (this._URL != "" && this._URL.Contains("PurchaseVendorRemark"))
                        {
                            this.DocType = "PO";
                            this.Module = "PHOENIX_PO";
                            bool isFileProcessed = GetPODetails(this._URL);

                            if (isFileProcessed)
                            {
                                // move to backup
                                MoveFile(LinkPath + "\\Backup", file);
                                Console.WriteLine(this.CurrentTimeStamp + " : PO '" + VRNO.Trim() + "' downloaded successfully.");
                            }
                            else
                            {
                                // move to error_files
                                Console.WriteLine(this.CurrentTimeStamp + " :  Unable to process link file '" + file.Name + "'.");
                                this.SetLog(this.CurrentTimeStamp + " : Unable to process link file '" + file.Name + "'.");
                                if (moveToError)
                                {
                                    MoveFile(LinkPath + "\\Error_files", file);
                                }
                                AddToAudit(BuyerCode, SuppCode, this.Module, file.Name, VRNO.Trim(), "Error", "Unable to process link file." + errLog);
                            }
                        }
                        else
                        {
                            Console.WriteLine(this.CurrentTimeStamp + " : Unable to process link file; Link not found in file " + Path.GetFileName(sFile));
                            this.SetLog(this.CurrentTimeStamp + " : Unable to process link file; Link not found in file " + Path.GetFileName(sFile));
                            AddToAudit(BuyerCode, SuppCode, this.Module, file.Name, "", "Error", "Unable to process link file; Link not found in file " + file.Name);

                            MoveFile(LinkPath + "\\Error_files", file);
                        }
                    }

                    // Updated on 01-APR-2017
                    RichTextBox txt = new RichTextBox();
                    string skipTextFiles = convert.ToString(ConfigurationSettings.AppSettings["SKIP_TEXT_FILES"]);
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
                            if (txt.Lines.Length > 0)
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

                            if (textFound)
                            {
                                FileInfo _movepdf = new FileInfo(pdfFile);

                                // Move file to Error Files
                                MoveFile(_movepdf.Directory.FullName + "\\InvalidFiles", _movepdf);
                                SetLog(this.CurrentTimeStamp + " : Invalid File " + _movepdf.Name + " moved to invalidFiles folder");
                            }
                        }
                    }
                }
                else
                {
                    this.SetLog(this.CurrentTimeStamp + " : Another instance of WEBSCRAPPER is running!");
                    exeInRunningState = true;
                }
            }
            catch (Exception ex)
            {
                SetLog(this.CurrentTimeStamp + " : Error in GetRFQPODetails() - " + ex.Message + ex.StackTrace);
            }
            finally
            {
                SetLog(this.CurrentTimeStamp + " : Phoenix RFQ Downloader Stopped.\r\n");
                SetLog(Environment.NewLine);
                try
                {
                    if (!exeInRunningState) KillIE();
                }
                catch { }
            }
        }

        private string GetURL(string emlFile)
        {
            string URL = "";
            try
            {
                int searchCriteria = 0;
                URL = File.ReadAllText(emlFile);
                RichTextBox txt = new RichTextBox();
                txt.Text = URL.Trim();
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
                SetLog("Error GetURL() : " + ex.Message);
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
                SetLog("Error GetURL() : " + ex.Message);
            }
            return URL;
        }

        private bool GetRFQDetails(string URL)
        {
            bool result = false;
            try
            {
                using (IEBrowser _ie = new IEBrowser())
                {
                    try
                    {
                        this.SetLog(this.CurrentTimeStamp + " : Navigating To URL ..");
                        _ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Minimize);
                        _ie.gotopage(URL.Trim().Replace("\r\n", ""));
                        _ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Minimize);

                        Frame _frame = _ie.Frame("filterandsearch");
                        int Counter = 0;
                        while (!_frame.TextField("txtnopage").Exists)
                        {
                            Counter++;
                            Thread.Sleep(1000);
                            if (Counter > 60) break;
                        }

                        currentXMLFile = "";
                        this.PageGUID = URL.Trim();

                        this.SetLog(this.CurrentTimeStamp + " : Page Loaded ..");
                        FetchData(_ie);

                        if (File.Exists(currentXMLFile))
                        {
                            this.SetLog(this.CurrentTimeStamp + " : File saved ..");
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        result = false;
                    }
                    finally
                    {
                        try
                        {
                            //_ie.ForceClose();
                            _ie.Dispose();
                            Thread.Sleep(5000);
                        }
                        catch { }
                    }
                }

                Thread.Sleep(7000);
                KillIE();

                return result;
            }
            catch (Exception ex)
            {
                SetLog(this.CurrentTimeStamp + " : Error in GetRFQDetails() - " + ex.StackTrace);
                return false;
            }
        }

        private bool GetPODetails(string URL)
        {
            bool result = false;
            try
            {
                using (IEBrowser _ie = new IEBrowser())
                {
                    try
                    {
                        if (convert.ToString(ConfigurationSettings.AppSettings["VIEW_BROWSER"]).Trim().ToUpper() == "TRUE")
                            _ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Minimize);
                        else
                            _ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Hide);

                        string PO_URL = URL.Trim().Replace("PurchaseVendorRemark.aspx", "PurchaseQuotationItems.aspx");
                        _ie.gotopage(PO_URL.Trim());

                        //_ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Minimize);
                        if (convert.ToString(ConfigurationSettings.AppSettings["VIEW_BROWSER"]).Trim().ToUpper() == "TRUE")
                            _ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Minimize);
                        else
                            _ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Hide);

                        // Search PO PDF                        
                        string LinkPath = convert.ToString(ConfigurationSettings.AppSettings["LINK_PATH"]);
                        DirectoryInfo _dirFiles = new DirectoryInfo(LinkPath);
                        if (_dirFiles.Exists)
                        {
                            FileInfo[] _pdfFiles = _dirFiles.GetFiles("*.pdf");
                            foreach (FileInfo _pdf in _pdfFiles)
                            {
                                if (_pdf.Name.Contains(this.VRNO) && _pdf.Name.Contains(Path.GetFileNameWithoutExtension(this.orgEmlFile)))
                                {
                                    _orgDocFile = _pdf;
                                    break;
                                }
                            }
                        }

                        Frame _frame = _ie.Frame("filterandsearch");
                        int Counter = 0;
                        while (_frame != null && !_frame.TextField("txtnopage").Exists)
                        {
                            Counter++;
                            Thread.Sleep(1000);
                            _frame = _ie.Frame("filterandsearch");
                            if (Counter > 60) break;
                        }

                        currentXMLFile = "";
                        this.PageGUID = URL.Trim();
                        FetchData(_ie);

                        if (File.Exists(currentXMLFile)) result = true;
                        else
                        {
                            // Added on 19-JUL-16 FOR PO
                            LeSXML.LeSXML _lesXML = new LeSXML.LeSXML();
                            Dictionary<string, string> _xmlHeader = this.GetOrderDetails(_orgDocFile.FullName);
                            base._xmlItems = this.GetOrderItems(_orgDocFile.FullName, ref base._xmlItems);
                            _lesXML.LeadTimeDays = "";
                            _lesXML.LineItems.Clear();
                            _lesXML.Addresses.Clear();

                            if (SuppCode.Trim() == "")
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
                                            SuppCode = convert.ToString(lstSupp[domain.Trim().ToLower()]);
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
                    }
                    catch (Exception ex)
                    {
                        SetLog(this.CurrentTimeStamp + " : Error in GetPODetails: using block- " + ex.StackTrace);
                        throw ex;
                    }
                    finally
                    {
                        try
                        {
                            //_ie.ForceClose();
                            _ie.Dispose();
                            Thread.Sleep(5000);
                            KillIE();
                        }
                        catch { }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                SetLog(this.CurrentTimeStamp + " : Error in GetRFQDetails() - " + ex.StackTrace);
                return false;
            }
        }

        public override Dictionary<string, string> GetHeaderData(IEBrowser _ie)
        {
            try
            {
                this.SetLog(this.CurrentTimeStamp + " : Reading Header Details ..");
                Dictionary<string, string> _xmlHeader = base.GetHeaderData(_ie);
                if (this.DocType == "RFQ")
                {
                    if (File.Exists(@MAPPath + @"\" + HeaderMapFile))
                    {
                        _xmlHeader.Clear();
                        string[] _lines = File.ReadAllLines(@MAPPath + @"\" + HeaderMapFile);
                        for (int i = 0; i < _lines.Length; i++)
                        {
                            string[] _keys = _lines[i].Split('=');
                            if (_keys.Length > 1)
                            {
                                string _value = "";
                                Frame _frame = _ie.Frame("filterandsearch");
                                if (_frame.Element(_keys[1]).Exists)
                                {
                                    _value = _frame.Element(_keys[1]).Text;
                                    if (convert.ToString(_value).Trim() == "" && _frame.Element(_keys[1]).TagName == "INPUT") _value = convert.ToString(_frame.TextField(_keys[1]).Text);
                                }
                                if (_ie.Element(_keys[1]).Exists)
                                {
                                    _value = _frame.Element(_keys[1]).Text;
                                }
                                if (String.IsNullOrEmpty(_value)) { _value = string.Empty; }

                                if (!_xmlHeader.ContainsKey(_keys[0]))
                                {
                                    _xmlHeader.Add(_keys[0], _value);
                                }
                                else _xmlHeader[_keys[0]] = _value.Trim();

                                if (_keys[0] == "VRNO") PageReference = _value;
                            }
                        }
                    }
                    _xmlHeader.Add("DOC_TYPE", this.DocType);

                    this.VRNO = _xmlHeader["VRNO"];
                    this.VRNO = this.VRNO.Replace("Quotation Details", "").Trim();
                    this.VRNO = this.VRNO.TrimStart('[').TrimEnd(']').Trim();
                    _xmlHeader["VRNO"] = this.VRNO;

                    this.HtmlFile = this.Domain + "_" + this.VRNO + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".htm";
                    this.ImgFile = this.Domain + "_" + this.VRNO + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                }
                else if (this.DocType == "PO")
                {
                    if (File.Exists(@MAPPath + @"\" + HeaderMapFile))
                    {
                        _xmlHeader.Clear();
                        string[] _lines = File.ReadAllLines(@MAPPath + @"\" + HeaderMapFile);
                        for (int i = 0; i < _lines.Length; i++)
                        {
                            string[] _keys = _lines[i].Split('=');
                            if (_keys.Length > 1)
                            {
                                string _value = "";
                                Frame _frame = _ie.Frame("filterandsearch");
                                if (_frame.Element(_keys[1]).Exists)
                                {
                                    _value = _frame.Element(_keys[1]).Text;
                                    if (_frame.Element(_keys[1]).TagName == "INPUT") _value = convert.ToString(_frame.TextField(_keys[1]).Text);
                                    if (_frame.Element(_keys[1]).TagName == "SELECT") _value = convert.ToString(_frame.SelectList(_keys[1]).SelectedItem);
                                }

                                if (_ie.Element(_keys[1]).Exists) _value = convert.ToString(_frame.Element(_keys[1]).Text);
                                if (String.IsNullOrEmpty(_value)) { _value = string.Empty; }
                                if (!_xmlHeader.ContainsKey(_keys[0])) _xmlHeader.Add(_keys[0], _value);
                                else _xmlHeader[_keys[0]] = _value.Trim();
                                if (_keys[0] == "VRNO") PageReference = _value;
                            }
                        }
                    }

                    _xmlHeader.Add("DOC_TYPE", this.DocType);

                    #region // Get VRNO, Order Date & Quote Ref. No //
                    string cOrderDate = "", cVRNO = "", cQuoteRef = "";
                    using (RichTextBox txt = new RichTextBox())
                    {
                        txt.Text = File.ReadAllText(this.orgEmlFile);
                        foreach (string line in txt.Lines)
                        {
                            if (line.Trim().Contains("Purchase Order:"))
                            {
                                if (line.Trim().StartsWith("Purchase Order:")) cVRNO = line.Replace("Purchase Order:", "");
                                else
                                {
                                    int indx = line.IndexOf("Purchase Order:");
                                    if (indx > -1) cVRNO = line.Substring(indx);
                                    cVRNO = cVRNO.Replace("Purchase Order:", "").Trim();
                                    indx = cVRNO.Trim().IndexOf(" ");
                                    if (indx > -1) cVRNO = cVRNO.Trim().Substring(0, indx + 1).Trim();
                                }
                            }
                            else if (line.Trim().StartsWith("Quotation Ref No:"))
                            {
                                cQuoteRef = line.Replace("Quotation Ref No:", "");
                            }
                            else if (line.Trim().StartsWith("Ordered Date:"))
                            {
                                cOrderDate = line.Replace("Ordered Date:", "");
                            }

                            if (cVRNO.Trim() != "" && cQuoteRef.Trim() != "" && cOrderDate.Trim() != "")
                            {
                                break;
                            }
                        }
                    }
                    #endregion

                    // Validate File Values with RFQ Link
                    if (convert.ToString(_xmlHeader["VRNO"]).Contains(cVRNO.Trim()) && cQuoteRef.Trim() == convert.ToString(_xmlHeader["QUOTE_REF"]))
                    {
                        _xmlHeader["ORDER_DATE"] = cOrderDate.Trim();
                        this.VRNO = cVRNO.Trim();
                        _xmlHeader["VRNO"] = this.VRNO;
                    }
                    else
                    {
                        _xmlHeader = GetOrderDetails(_orgDocFile.FullName);
                        if (_xmlHeader["VRNO"].Trim() != "" && _xmlHeader["VRNO"] != cVRNO.Trim())
                        {
                            errLog = "PO details mismatched";
                            throw new Exception("PO details mismatched");
                        }
                    }

                    this.HtmlFile = this.Domain + "_" + this.VRNO + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".htm";
                    this.ImgFile = this.Domain + "_" + this.VRNO + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";
                }

                #region // Set Buyer Code //
                string Host = _ie.Uri.Host;
                if (lstBuyer.ContainsKey(Host))
                {
                    string[] byrInfo = lstBuyer[Host].Trim().Split(',');
                    BuyerCode = convert.ToString(byrInfo[0]).Trim();
                    if (byrInfo.Length > 1) BuyerName = convert.ToString(byrInfo[1]);
                }
                #endregion

                #region // Set Supplier Code //
                string email = convert.ToString(_xmlHeader["VENDOR_EMAIL"]);
                if (email.Trim().Length > 0)
                {
                    int indx = email.Split(',')[0].Split(';')[0].Trim().IndexOf('@');
                    if (indx > 0)
                    {
                        string domain = email.Split(',')[0].Split(';')[0].Substring(indx);
                        if (lstSupp.ContainsKey(domain.Trim().ToLower()))
                        {
                            SuppCode = convert.ToString(lstSupp[domain.Trim().ToLower()]);

                            if (convert.ToString(_xmlHeader["PORT_NAME"]).Trim() != "")
                            {
                                string PORT = convert.ToString(_xmlHeader["PORT_NAME"]).Trim().ToUpper();
                                if (ConfigurationSettings.AppSettings[PORT + "_" + SuppCode] != null)
                                {
                                    string[] newSuppCode = ConfigurationSettings.AppSettings[PORT + "_" + SuppCode].Trim().Split('_');
                                    if (newSuppCode.Length > 1)
                                    {
                                        SuppCode = newSuppCode[1].Trim().ToUpper();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    errLog = "Unable to get supplier code.";
                    throw new Exception("Unable to get supplier code.");
                }
                #endregion

                return _xmlHeader;
            }
            catch (Exception ex)
            {
                this.SetLog(this.CurrentTimeStamp + " : Error while reading header details - " + ex.StackTrace);
                if (ex.Message.Contains("Timeout while waiting")) moveToError = false;
                throw ex;
            }
        }

        public override List<List<string>> GetItemsData(IEBrowser _ie)
        {
            try
            {
                this.SetLog(this.CurrentTimeStamp + " : Reading Item Details ..");
                if (this.DocType == "RFQ")
                {
                    return GetRFQItems(_ie);
                }
                else
                {
                    CalculatedTotal = 0;
                    List<List<string>> items = GetPOItems(_ie);
                    if (items.Count == 0)
                    {
                        items = GetOrderItems(_orgDocFile.FullName, ref items);
                    }
                    return items;
                }
            }
            catch (Exception ex)
            {
                this.SetLog(this.CurrentTimeStamp + " : Error while reading items - " + ex.StackTrace);
                if (ex.Message.Contains("Timeout while waiting")) moveToError = false;
                throw ex;
            }
        }

        private List<List<string>> GetRFQItems(IEBrowser _ie)
        {
            List<List<string>> lstItems = new List<List<string>>();

            Frame _frame = _ie.Frame("filterandsearch");
            Table tblItems = _frame.Table("gvVendorItem");
            if (tblItems.Exists)
            {
                if (tblItems.TableRows.Count > 1)
                {
                    int Counter = 02, itemCount = 0, pageCounter = 1;
                    string _strCounter = "";
                    if (Counter < tblItems.TableRows.Count) _strCounter = "0" + Counter;
                    else if (Counter > 10) _strCounter = Counter.ToString();
                    Span itemNo = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblSNo");
                    do
                    {
                        if (itemNo.Exists)
                        {
                            itemCount++;
                            Span spPartNo = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblNumber");
                            Span spRefNo = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblMakerReference");
                            Span spPos = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblPosition");
                            Span spDetails = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblDetails");
                            Link lnkDescr = _frame.Link("gvVendorItem_ctl" + _strCounter + "_lnkStockItemCode");
                            Span spDescr = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblComponentName");
                            Span spQty = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblOrderQuantity");
                            Span spUnit = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblunit");
                            WatiN.Core.Image _imgRemarks = _frame.Image("gvVendorItem_ctl" + _strCounter + "_cmdDetails");

                            #region /* Item Remarks */
                            string ItemRemarks = "";
                            if (_imgRemarks.Exists && !convert.ToString(_imgRemarks.OuterHtml).Contains("disabled=\"disabled\""))
                            {
                                string _viewRemarks = _imgRemarks.GetAttributeValue("onclick");
                                int _startIndx = _viewRemarks.IndexOf("PurchaseFormItemComment.aspx?");
                                string _remarksURL = _viewRemarks.Substring(_startIndx);

                                int _endIndx = _remarksURL.IndexOf("viewonly=Y");
                                if (_endIndx > 0)
                                {
                                    _remarksURL = _remarksURL.Substring(0, _endIndx + 10);
                                }

                                using (IEBrowser _ieItemRemark = new IEBrowser())
                                {
                                    _ieItemRemark.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Minimize);
                                    _ieItemRemark.gotopage("http://apps.southnests.com/Phoenix/Purchase/" + _remarksURL);
                                    _ieItemRemark.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Minimize);
                                    if (_ieItemRemark.Frames.Count > 1)
                                    {
                                        ItemRemarks = convert.ToString(_ieItemRemark.Frames[2].Body.Text);
                                    }

                                    try
                                    {
                                        _ieItemRemark.Close();
                                        _ieItemRemark.Dispose();
                                    }
                                    catch { }
                                    finally
                                    {
                                        Thread.Sleep(1000);
                                    }
                                }
                            }
                            #endregion

                            string Descr = convert.ToString(lnkDescr.Text);
                            string PartNo = convert.ToString(spPartNo.Text).Replace("__.__.__", "");
                            if (PartNo.Trim().Length > 0)
                            {
                                ItemRemarks += Environment.NewLine + "Number : " + PartNo;
                            }

                            string equipDetails = convert.ToString(spDetails.Text);
                            if (spPos.Exists && convert.ToString(spPos.Text).Trim() != "/")
                            {
                                equipDetails += Environment.NewLine + "Drawing No/Position : " + convert.ToString(spPos.Text).Trim();
                            }

                            string RefNo = "";
                            if (spRefNo.Exists)
                            {
                                RefNo = convert.ToString(spRefNo.Text).Trim();
                            }

                            string EquipName = "";
                            if (spDescr.Exists) EquipName = convert.ToString(spDescr.Text);

                            List<string> item = new List<string>();
                            item.Add(convert.ToString(itemNo.Text).Trim());
                            item.Add(RefNo); // Item Ref No
                            item.Add(Descr.Trim()); // Item Description
                            item.Add(convert.ToDouble(spQty.Text).ToString("0.00")); // Qty
                            item.Add(convert.ToString(spUnit.Text)); // UOM

                            item.Add(""); //Equipment Maker 
                            item.Add(equipDetails); //Equipment Remarks
                            item.Add(EquipName.Trim()); // Equip Name                                
                            item.Add(convert.ToString(ItemRemarks).Trim()); // Item Remarks

                            lstItems.Add(item);
                            Counter++;

                            #region // Download Attachments //
                            WatiN.Core.Image _imgAttachment = _frame.Image("gvVendorItem_ctl" + _strCounter + "_cmdViewAttachment");
                            List<string> _attachments = new List<string>();
                            if (_imgAttachment.Exists)
                            {
                                _imgAttachment.Click(); Thread.Sleep(2000);

                                Frame _frameAttachment = _ie.Frames[1];
                                Table _grAttachments = _frameAttachment.Table("gvAttachment");
                                if (_grAttachments.Exists)
                                {
                                    foreach (TableRow _tblRow in _grAttachments.TableRows)
                                    {
                                        if (_tblRow.TableCells.Count > 0 && _tblRow.TableCells[2].Text == "View")
                                        {
                                            string filename = _tblRow.TableCells[1].Text;

                                            Link lnk = _tblRow.TableCells[2].Links[0];
                                            string _fileURL = lnk.GetAttributeValue("href");
                                            _attachments.Add(filename + "|" + _fileURL);
                                        }
                                    }
                                }

                                Div _divClose = _ie.Div(WatiN.Core.Find.ByTitle("Close"));
                                if (_divClose.Exists) _divClose.Click(); Thread.Sleep(500);

                                // WebClient to download files 
                                foreach (string _fileInfo in _attachments)
                                {
                                    string[] fileInfo = _fileInfo.Split('|');
                                    try
                                    {
                                        using (System.Net.WebClient _client = new System.Net.WebClient())
                                        {
                                            _client.DownloadFile(fileInfo[1].Trim(), this.ImagePath + "\\" + fileInfo[0].Trim());
                                            if (File.Exists(this.ImagePath + "\\" + fileInfo[0].Trim()))
                                            {
                                                attachments += "|" + fileInfo[0].Trim();
                                            }
                                            _client.Dispose();
                                        }
                                    }
                                    catch { }
                                }
                            }
                            #endregion
                        }

                        if (Counter.ToString().Length == 1) _strCounter = "0" + Counter;
                        else _strCounter = Counter.ToString();

                        if (itemCount >= (tblItems.TableRows.Count - 1))
                        {
                            // Check Total Item Count
                            Span _spnTotalItemCount = _frame.Span("lblRecords");
                            if (_spnTotalItemCount.Exists)
                            {
                                string _totalItems = convert.ToString(_spnTotalItemCount.Text).Trim().Replace("(", "").Replace(")", "").Replace("records found", "").Trim();
                                if (convert.ToInt(_totalItems) == lstItems.Count)
                                {
                                    break;
                                }
                            }

                            // Move to next Grid Page
                            WatiN.Core.Link lnkNext = _ie.Frames[0].Link("cmdNext");
                            if (lnkNext.Exists && lnkNext.GetAttributeValue("disabled").Trim().ToLower() != "disabled" && lnkNext.GetAttributeValue("disabled").Trim().ToLower() != "true")
                            {
                                pageCounter++;
                                lnkNext.Click();
                                int waitCounter = 0;
                                while (_frame.Span("lblPageNumber").Exists && !_frame.Span("lblPageNumber").Text.Contains("Page " + pageCounter))
                                {
                                    if (waitCounter > 100) throw new Exception(pageCounter + " Page not found ");
                                    waitCounter++;
                                    Thread.Sleep(500);
                                }
                            }

                            // Get Frame Page
                            _frame = _ie.Frames[0];

                            // Reset Counter
                            Counter = 02; itemCount = 0;
                            if (Counter.ToString().Length == 1) _strCounter = "0" + Counter;
                            else _strCounter = Counter.ToString();
                        }

                        itemNo = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblSNo");
                    }
                    while (itemNo.Exists);
                }
            }

            return lstItems;
        }

        private List<List<string>> GetPOItems(IEBrowser _ie)
        {
            List<List<string>> lstItems = new List<List<string>>();

            Frame _frame = _ie.Frame("filterandsearch");
            Table tblItems = _frame.Table("gvVendorItem");
            if (tblItems.Exists)
            {
                CalculatedTotal = 0;
                if (tblItems.TableRows.Count > 1)
                {
                    int Counter = 02, itemCount = 0, pageCounter = 1;
                    string _strCounter = "";
                    if (Counter < tblItems.TableRows.Count) _strCounter = "0" + Counter;
                    else if (Counter > 10) _strCounter = Counter.ToString();
                    Span itemNo = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblSNo");
                    do
                    {
                        if (itemNo.Exists)
                        {
                            itemCount++;
                            Span spPartNo = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblNumber");
                            Span spRefNo = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblMakerReference");
                            Span spPos = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblPosition");
                            Span spDetails = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblDetails");
                            Link lnkDescr = _frame.Link("gvVendorItem_ctl" + _strCounter + "_lnkStockItemCode");
                            Span spDescr = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblComponentName");
                            Span spQtdQty = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblQuantity");
                            Span spOrdQty = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblOrderQuantity");
                            Span spUnit = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblunit");
                            Span spPrice = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblQuotedPrice");
                            Span spDisc = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblDiscount");
                            Span spTotalPrice = _frame.Span("gvVendorItem_ctl" + _strCounter + "_txtTotal");
                            Span spLeadDays = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblDeliveryTime");
                            WatiN.Core.Image _imgRemarks = _frame.Image("gvVendorItem_ctl" + _strCounter + "_cmdDetails");

                            #region /* Item Remarks */
                            string ItemRemarks = "";
                            if (_imgRemarks.Exists && !convert.ToString(_imgRemarks.OuterHtml).Contains("disabled=\"disabled\""))
                            {
                                string _viewRemarks = _imgRemarks.GetAttributeValue("onclick");
                                int _startIndx = _viewRemarks.IndexOf("PurchaseFormItemComment.aspx?");
                                string _remarksURL = _viewRemarks.Substring(_startIndx);
                                int _endIndx = _remarksURL.IndexOf("viewonly=Y");
                                if (_endIndx > 0) _remarksURL = _remarksURL.Substring(0, _endIndx + 10);

                                using (IEBrowser _ieItemRemark = new IEBrowser())
                                {
                                    _ieItemRemark.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Minimize);
                                    _ieItemRemark.gotopage("http://apps.southnests.com/Phoenix/Purchase/" + _remarksURL);
                                    _ieItemRemark.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Minimize);
                                    if (_ieItemRemark.Frames.Count > 1)
                                    {
                                        ItemRemarks = convert.ToString(_ieItemRemark.Frames[2].Body.Text);
                                    }
                                    try
                                    {
                                        _ieItemRemark.Close();
                                        _ieItemRemark.Dispose();
                                    }
                                    catch { }
                                    finally
                                    {
                                        Thread.Sleep(1000);
                                    }
                                }
                            }
                            #endregion

                            string Descr = convert.ToString(lnkDescr.Text).Trim();
                            string PartNo = convert.ToString(spPartNo.Text).Replace("__.__.__", "").Trim();
                            if (PartNo.Trim().Length > 0) ItemRemarks += Environment.NewLine + "Number : " + PartNo;

                            string equipDetails = convert.ToString(spDetails.Text);
                            if (spPos.Exists && convert.ToString(spPos.Text).Trim() != "/")
                            {
                                equipDetails += Environment.NewLine + "Drawing No/Position : " + convert.ToString(spPos.Text).Trim();
                            }

                            string RefNo = "";
                            if (spRefNo.Exists) RefNo = convert.ToString(spRefNo.Text).Trim();

                            string EquipName = "";
                            if (spDescr.Exists) EquipName = convert.ToString(spDescr.Text);

                            if (spQtdQty.Exists && convert.ToString(spQtdQty.Text).Trim().Length > 0)
                            {
                                ItemRemarks += Environment.NewLine + "Quoted Qty : " + convert.ToString(spQtdQty.Text).Trim();
                            }

                            if (spTotalPrice.Exists)
                            {
                                CalculatedTotal = CalculatedTotal + (convert.ToDouble(spOrdQty.Text) * convert.ToDouble(spPrice.Text)); //convert.ToDouble(spTotalPrice.Text);
                            }

                            List<string> item = new List<string>();
                            item.Add(convert.ToString(itemNo.Text).Trim());
                            item.Add(RefNo); // Item Ref No
                            item.Add(Descr.Trim()); // Item Description
                            item.Add(convert.ToDouble(spOrdQty.Text).ToString("0.00")); // Qty
                            item.Add(convert.ToString(spUnit.Text)); // UOM
                            item.Add(convert.ToDouble(spQtdQty.Text).ToString("0.00")); // Qtd Qty
                            item.Add(convert.ToDouble(spPrice.Text).ToString("0.00")); // Price
                            item.Add(convert.ToDouble(spDisc.Text).ToString("0.00")); // Disc Qty

                            item.Add(""); //Equipment Maker 
                            item.Add(equipDetails); //Equipment Remarks
                            item.Add(EquipName.Trim()); // Equip Name                                
                            item.Add(convert.ToString(ItemRemarks).Trim()); // Item Remarks

                            lstItems.Add(item);
                            Counter++;
                        }

                        if (Counter.ToString().Length == 1) _strCounter = "0" + Counter;
                        else _strCounter = Counter.ToString();

                        if (itemCount >= (tblItems.TableRows.Count - 1))
                        {
                            // Check Total Item Count
                            Span _spnTotalItemCount = _frame.Span("lblRecords");
                            if (_spnTotalItemCount.Exists)
                            {
                                string _totalItems = convert.ToString(_spnTotalItemCount.Text).Trim().Replace("(", "").Replace(")", "").Replace("records found", "").Trim();
                                if (convert.ToInt(_totalItems) == lstItems.Count)
                                {
                                    break;
                                }
                            }

                            // Move to next Grid Page
                            WatiN.Core.Link lnkNext = _ie.Frames[0].Link("cmdNext");
                            if (lnkNext.Exists && lnkNext.GetAttributeValue("disabled").Trim().ToLower() != "disabled" && lnkNext.GetAttributeValue("disabled").Trim().ToLower() != "true")
                            {
                                pageCounter++;
                                lnkNext.Click();
                                int pageWaitCounter = 0;
                                while (_frame.Span("lblPageNumber").Exists && !_frame.Span("lblPageNumber").Text.Contains("Page " + pageCounter))
                                {
                                    if (pageWaitCounter > 100) break;
                                    Thread.Sleep(500);
                                    pageWaitCounter++;
                                }
                            }

                            // Get Frame Page
                            _frame = _ie.Frames[0];

                            // Reset Counter
                            Counter = 02; itemCount = 0;
                            if (Counter.ToString().Length == 1) _strCounter = "0" + Counter;
                            else _strCounter = Counter.ToString();
                        }

                        itemNo = _frame.Span("gvVendorItem_ctl" + _strCounter + "_lblSNo");
                    }
                    while (itemNo.Exists);
                }
            }
            return lstItems;
        }

        public override void ExporttoHeader(Dictionary<string, string> _xmlHeader, LeSXML.LeSXML _lesXML)
        {
            try
            {
                this.VRNO = _xmlHeader["VRNO"].Trim();
                _lesXML.Active = "1";
                _lesXML.Doc_Type = this.DocType;
                _lesXML.BuyerRef = _xmlHeader["VRNO"].Trim();
                _lesXML.Vessel = _xmlHeader["VESSEL"].Trim();
                _lesXML.IMONO = _xmlHeader["IMO"].Trim();
                _lesXML.PortName = _xmlHeader["PORT_NAME"].Trim();

                _lesXML.Recipient_Code = SuppCode.Trim(); //SuppCode;
                _lesXML.Sender_Code = BuyerCode.Trim(); //BuyerCode;

                _lesXML.FileName = this.DocType + "_" + convert.ToFileName(this.VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xml";
                _lesXML.OrigDocFile = this.ImgFile + attachments;

                string BuyerRemarks = "";
                if (_xmlHeader.ContainsKey("VESSEL_TYPE")) BuyerRemarks += "Vessel Type : " + _xmlHeader["VESSEL_TYPE"].Trim();
                if (_xmlHeader.ContainsKey("YARD") && _xmlHeader["YARD"].Trim().Length > 0) BuyerRemarks += Environment.NewLine + "Yard : " + _xmlHeader["YARD"].Trim();
                if (_xmlHeader.ContainsKey("HULLNO") && _xmlHeader["HULLNO"].Trim().Length > 0) BuyerRemarks += Environment.NewLine + "Hull No. : " + _xmlHeader["HULLNO"].Trim();
                if (_xmlHeader.ContainsKey("YEARBUILT") && _xmlHeader["YEARBUILT"].Trim().Length > 0) BuyerRemarks += Environment.NewLine + "Year Built : " + _xmlHeader["YEARBUILT"].Trim();
                if (_xmlHeader.ContainsKey("REMARKS")) BuyerRemarks += Environment.NewLine + _xmlHeader["REMARKS"].Trim();
                string BlanksLines = Environment.NewLine + Environment.NewLine;
                while (BuyerRemarks.Contains(BlanksLines)) BuyerRemarks = BuyerRemarks.Replace(BlanksLines, Environment.NewLine);
                _lesXML.Remark_Sender = BuyerRemarks.Trim();

                #region /* address */
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
                #endregion

                if (this.DocType == "RFQ")
                {
                    string MsgNumber = this._URL;
                    _lesXML.DocLinkID = MsgNumber; // MessageNumber in eSupplier
                    _lesXML.DocReferenceID = MsgNumber; // MessageReferenceNumber in eSupplier

                    // Dates                 
                    _lesXML.Date_Document = GetDate(_xmlHeader["RFQ_DATE"].Trim());  /* RFQ Date */
                }
                else
                {
                    _lesXML.DocLinkID = this.VRNO;
                    if (_xmlHeader.ContainsKey("CURRENCY")) _lesXML.Currency = _xmlHeader["CURRENCY"].Trim();
                    else _lesXML.Currency = currCode.Trim();

                    if (_xmlHeader.ContainsKey("QUOTE_REF")) _lesXML.Reference_Document = _xmlHeader["QUOTE_REF"].Trim();
                    if (_xmlHeader.ContainsKey("PAY_TERMS") && !_xmlHeader["PAY_TERMS"].Trim().Contains("Select")) _lesXML.Remark_PaymentTerms = _xmlHeader["PAY_TERMS"].Trim();
                    if (_xmlHeader.ContainsKey("DEL_TERMS") && !_xmlHeader["DEL_TERMS"].Trim().Contains("Select")) _lesXML.Remark_DeliveryTerms = _xmlHeader["DEL_TERMS"].Trim();

                    if (_xmlHeader.ContainsKey("GRANT_TOTAL")) _lesXML.Total_LineItems_Net = convert.ToDouble(_xmlHeader["GRANT_TOTAL"].Trim()).ToString("0.00");
                    if (_xmlHeader.ContainsKey("LEAD_DAYS") && convert.ToInt(_xmlHeader["LEAD_DAYS"]) > 0) _lesXML.LeadTimeDays = _xmlHeader["LEAD_DAYS"].Trim();

                    // Dates                 
                    if (_xmlHeader.ContainsKey("ORDER_DATE")) _lesXML.Date_Document = GetDate(_xmlHeader["ORDER_DATE"].Trim());  /* ORDER Date */
                    else _lesXML.Date_Document = DateTime.Now.ToString("yyyyMMdd");

                    // Read PDF File And Compare Total
                    bool isItemTotalCorrect = false, isGrantTotalCorrect = false;
                    double FrightCharges = 0; double pdfGrantTotal = 0, pdfItemTotal = 0;
                    string Remarks = "";
                    if (_orgDocFile != null && _orgDocFile.Name.Trim() != "")
                    {
                        RichTextBox txtPDF = new RichTextBox();
                        txtPDF.Text = GetPDFText(_orgDocFile.FullName);

                        for (int i = 0; i < txtPDF.Lines.Length; i++)
                        {
                            string line = txtPDF.Lines[i];
                            if (line.Trim().StartsWith("Line Item Sub Total:"))
                            {
                                pdfItemTotal = convert.ToDouble(line.Replace("Line Item Sub Total:", "").Trim());
                                if (Math.Round(pdfItemTotal, 2) == Math.Round(CalculatedTotal, 2)) isItemTotalCorrect = true;
                            }
                            else if (line.Trim().StartsWith("Total for this Order"))
                            {
                                pdfGrantTotal = convert.ToDouble(line.Replace("Total for this Order", "").Trim());
                            }
                            else if (line.Trim().StartsWith("Tax and Charges"))
                            {
                                FrightCharges = convert.ToDouble(line.Replace("Tax and Charges", "").Trim());
                            }
                            else if (line.Trim().StartsWith("Please invoice the order to"))
                            {
                                for (int k = i; k < txtPDF.Lines.Length; k++)
                                {
                                    if (txtPDF.Lines[k].Trim().StartsWith("Following are the terms and conditions")) break;
                                    if (txtPDF.Lines[k].Trim().Length > 0) Remarks += txtPDF.Lines[k].Trim() + Environment.NewLine;
                                }
                            }
                        }
                    }

                    _lesXML.Total_Freight = FrightCharges.ToString("0.00"); // Set Feight Charges
                    _lesXML.Remark_Sender += Environment.NewLine + Remarks.Trim();
                    _lesXML.Total_LineItems_Net = pdfItemTotal.ToString("0.00");
                    _lesXML.Total_Net_Final = pdfGrantTotal.ToString("0.00");
                    _lesXML.OrigDocFile = Path.GetFileName(_orgDocFile.Name);
                    
                    //if (Math.Round(pdfGrantTotal, 2) == Math.Round(convert.ToDouble(_lesXML.Total_LineItems_Net), 2)) isGrantTotalCorrect = true;
                    if (!isItemTotalCorrect)
                    {
                        if (reprocessed == false)
                        {
                            reprocessed = true;
                            _xmlHeader = this.GetOrderDetails(_orgDocFile.FullName);
                            base._xmlItems = this.GetOrderItems(_orgDocFile.FullName, ref base._xmlItems);
                            _lesXML.LeadTimeDays = "";
                            _lesXML.LineItems.Clear();
                            _lesXML.Addresses.Clear();
                            ExporttoHeader(_xmlHeader, _lesXML);
                        }
                        else
                        {
                            errLog = "Item Total Mismatched.";
                            throw new Exception("Item Total Mismatched.");
                        }
                    }

                    // Copy File to Attachments
                    if (File.Exists(_orgDocFile.FullName))
                    {
                        if (File.Exists(this.ImagePath + "\\" + _orgDocFile.Name)) File.Delete(this.ImagePath + "\\" + _orgDocFile.Name);
                        _orgDocFile.CopyTo(this.ImagePath + "\\" + _orgDocFile.Name);
                        if (File.Exists(this.ImagePath + "\\" + _orgDocFile.Name))
                        {
                            File.Move(_orgDocFile.FullName, _orgDocFile.Directory.FullName + "\\Backup\\" + _orgDocFile.Name);
                        }
                    }
                }

                currentXMLFile = _lesXML.FilePath + "\\" + _lesXML.FileName;
            }
            catch (Exception ex)
            {
                SetLog(this.CurrentTimeStamp + " : Error in ExporttoHeader() - " + ex.StackTrace);
                throw ex;
            }
        }

        public override void ExporttoItems(List<List<string>> _xmlItems, LeSXML.LeSXML _lesXML)
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
                        if (_lItem[7].Trim().Length > 0)
                        {
                            _item.Equipment = _lItem[7].Trim();
                        }
                        if (_lItem[5].Trim().Length > 0)
                        {
                            _item.EquipMaker = _lItem[5];
                        }
                        _item.EquipRemarks += Environment.NewLine + _lItem[6];
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
                        _item.ListPrice = _lItem[6].Trim();
                        _item.Discount = _lItem[7].Trim();
                        if (_lItem[8].Trim().Length > 0) _item.EquipMaker = _lItem[8].Trim();
                        _item.EquipRemarks = _lItem[9].Trim();
                        if (_lItem[10].Trim().Length > 0) _item.Equipment = _lItem[10].Trim();

                        _item.Remark = convert.ToString(_lItem[11]).Trim();
                        _item.OriginatingSystemRef = _lItem[0];

                        _lesXML.LineItems.Add(_item);
                    }
                }

                _lesXML.Total_LineItems = _lesXML.LineItems.Count.ToString();
            }
            catch (Exception ex)
            {
                this.SetLog(this.CurrentTimeStamp + " : Error in ExporttoItems() - " + ex.StackTrace);
            }
        }

        private Dictionary<string, string> GetOrderDetails(string pdfFile)
        {
            RichTextBox txtData = new RichTextBox();
            try
            {
                pdfItemStartCount = 0;
                Dictionary<string, string> _xmlHeader = new Dictionary<string, string>();
                txtData.Text = GetPDFText(pdfFile);

                if (txtData.Lines.Length > 0 && txtData.Lines[4].Trim() == "PURCHASE ORDER")
                {
                    int counter = 0;
                    _xmlHeader["BUYER_NAME"] = txtData.Lines[0].Trim();
                    _xmlHeader["BUYER_ADDR"] = txtData.Lines[1].Trim();
                    _xmlHeader["VRNO"] = "";

                    string _strLine = txtData.Lines[6];
                    if (_strLine.Length > 90 && _strLine.Substring(65, 20).Trim() == "Purchase Order")
                        _xmlHeader["VRNO"] = _strLine.Substring(85).Trim();

                    if (_strLine.Length > 67 && _strLine.Substring(0, 9).Trim() == "TO")
                        _xmlHeader["VENDOR_NAME"] = _strLine.Substring(9, (67 - 9)).Trim();

                    _strLine = txtData.Lines[8];
                    if (_strLine.Length > 65 && _strLine.Substring(65).Trim() == "Quotation Reference")
                        _xmlHeader["QUOTE_REF"] = txtData.Lines[7].Substring(85).Trim();

                    if (_strLine.Length > 67 && _strLine.Substring(0, 9).Trim() == "Address")
                    {
                        _xmlHeader["VENDOR_ADDRESS1"] = _strLine.Substring(9, (67 - 9)).Trim();

                        _strLine = txtData.Lines[9];
                        _xmlHeader["VENDOR_ADDRESS1"] += " " + _strLine.Trim();
                    }

                    int iordDate = 10;
                    for (int c = 0; c < txtData.Lines.Length; c++) { if (txtData.Lines[c].Trim().Contains("Date")) { iordDate = c; break; } }
                    _strLine = txtData.Lines[iordDate];
                    if (_strLine.Length > 85 && _strLine.Substring(65, (85 - 65)).Trim() == "Date")
                    {
                        _xmlHeader["ORDER_DATE"] = _strLine.Substring(85).Trim();
                    }

                    int iPayTerm = 11;
                    for (int c = 0; c < txtData.Lines.Length; c++) { if (txtData.Lines[c].Trim().Contains("Payment Terms")) { iPayTerm = c; break; } }
                    _strLine = txtData.Lines[iPayTerm];
                    if (_strLine.Length >= 80 && _strLine.Substring(65, (80 - 65)).Trim() == "Payment Terms")
                        _xmlHeader["PAY_TERMS"] = _strLine.Substring(80).Trim();
                    else _xmlHeader["PAY_TERMS"] = "";

                    int iPhoneDel = 12;
                    for (int c = 0; c < txtData.Lines.Length; c++) { if (txtData.Lines[c].Trim().StartsWith("Phone")) { iPhoneDel = c; break; } }
                    _strLine = txtData.Lines[iPhoneDel];
                    if (_strLine.Length > 67 && _strLine.Substring(0, 9).Trim() == "Phone")
                        _xmlHeader["VENDOR_PHONE"] = _strLine.Substring(9, (67 - 9)).Trim().Trim(',').Trim();

                    if (_strLine.Length > 85 && _strLine.Substring(65, (85 - 65)).Trim() == "Delivery Terms")
                        _xmlHeader["DEL_TERMS"] = _strLine.Substring(85).Trim();
                    else _xmlHeader["DEL_TERMS"] = "";
                    
                    int iFaxPort = 13;
                    for (int c = 0; c < txtData.Lines.Length; c++) { if (txtData.Lines[c].Trim().StartsWith("Fax")) { iFaxPort = c; break; } }
                    _strLine = txtData.Lines[iFaxPort];
                    if (_strLine.Length > 67 && _strLine.Substring(0, 9).Trim() == "Fax")
                        _xmlHeader["VENDOR_FAX"] = _strLine.Substring(9, (67 - 9)).Trim();                  
                    
                    if (_strLine.Length > 85 && _strLine.Substring(65, (85 - 65)).Trim() == "Port")
                        _xmlHeader["PORT_NAME"] = _strLine.Substring(85).Trim();
                    else _xmlHeader["PORT_NAME"] = "";

                    int iVendorEmail = 14;
                    for (int c = 0; c < txtData.Lines.Length; c++) { if (txtData.Lines[c].Trim().StartsWith("Mail")) { iVendorEmail = c; break; } }
                    _strLine = txtData.Lines[iVendorEmail];
                    if (_strLine.Length > 67 && _strLine.Substring(0, 9).Trim() == "Mail")
                    {
                        _xmlHeader["VENDOR_EMAIL"] = _strLine.Substring(9, (67 - 9)).Trim();
                        if (txtData.Lines[iVendorEmail + 1].Length > 67 && txtData.Lines[15].Substring(9, (67 - 9)).Trim() != "")
                        {
                            //counter++;
                            _xmlHeader["VENDOR_EMAIL"] += txtData.Lines[15].Substring(9, (67 - 9)).Trim();
                        }
                    }

                    if (_strLine.Length > 85 && _strLine.Substring(65, (85 - 65)).Trim() == "ETA")
                        _xmlHeader["ETA"] = _strLine.Substring(85).Trim();
                    else _xmlHeader["ETA"] = "";

                    //_strLine = txtData.Lines[30];
                    //if (_strLine.Trim() == "Delivery Instruction:")
                    //{
                        _xmlHeader["REMARKS"] = "";                    
                        for (int k = (30 + counter); k < txtData.Lines.Length; k++)
                        {
                            if (txtData.Lines[k].Trim().StartsWith("Vessel Name"))
                            {
                                counter = k;
                                break;
                            }
                            _xmlHeader["REMARKS"] += " " + txtData.Lines[k].Trim();                            
                        }                        
                    //}

                    _strLine = txtData.Lines[counter];
                    if (_strLine.Length > 15 && _strLine.Substring(0, 15).Trim() == "Vessel Name")
                        _xmlHeader["VESSEL"] = _strLine.Substring(15, (65 - 15)).Trim();

                    _strLine = txtData.Lines[counter + 2];
                    if (_strLine.Length > 15 && _strLine.Substring(0, 15).Trim() == "IMO NO")
                        _xmlHeader["IMO"] = _strLine.Substring(15, (65 - 15)).Trim();

                    int remStartIndx = 0;
                    for (int k = (counter + 3); k < txtData.Lines.Length; k++)
                    {
                        _strLine = txtData.Lines[k];
                        if (_strLine.Length > 0 && _strLine.Trim().StartsWith("MAKER'S NAME"))
                        {
                            remStartIndx = k;
                            break;
                        }
                    }

                    if (!_xmlHeader.ContainsKey("REMARKS")) _xmlHeader["REMARKS"] = "";
                    for (int k = remStartIndx; k < txtData.Lines.Length; k++)
                    {
                        if (txtData.Lines[k].Trim().StartsWith("Component Name"))
                        {
                            pdfItemStartCount = k;
                            break;
                        }
                        _xmlHeader["REMARKS"] += Environment.NewLine + txtData.Lines[k].Trim();
                    }

                    //pdfItemStartCount = 42 + counter;
                }

                return _xmlHeader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                try
                { txtData.Dispose(); }
                catch { }
            }
        }

        private List<List<string>> GetOrderItems(string pdfFile, ref List<List<string>> _lineItems)
        {
            RichTextBox txtData = new RichTextBox();
            try
            {
                if (_lineItems == null) _lineItems = new List<List<string>>();
                else _lineItems.Clear();
                txtData.Text = GetPDFText(pdfFile);
                CalculatedTotal = 0;

                if (pdfItemStartCount > 0)
                {
                    string compName = "", modal = "", SerNo = "";
                    Dictionary<string, string> _iDetails = new Dictionary<string, string>();

                    for (int i = pdfItemStartCount; i < txtData.Lines.Length; i++)
                    {
                        string line = txtData.Lines[i];
                        if (line.Trim().StartsWith("Line Item Sub Total:")) break;
                        if (line.Trim().StartsWith("Component"))
                        {
                            compName = ""; modal = ""; SerNo = "";
                            line = line.PadRight(150);
                            if (line.Substring(64, (75 - 64)).Trim() == "MODEL") modal = line.Substring(75).Trim();
                            i++;

                            compName = line.Substring(0, 64).Trim();
                            line = txtData.Lines[i].PadRight(150);
                            if (line.Length > 75 && line.Substring(64, (75 - 64)).Trim() == "SERIAL NO") SerNo = line.Substring(75).Trim();
                        }
                        else if (line.Trim().StartsWith("Item"))
                        {
                            i = i + 2;
                        }
                        else if (line.Trim().Length > 0)
                        {
                            string no = line.Trim().Split(' ')[0];
                            if (convert.ToInt(no) > 0 && line.Contains("Name:"))
                            {
                                if (_iDetails.Count > 0)
                                {
                                    List<string> _item = new List<string>();
                                    //_iDetails
                                    _item.Add(_iDetails["ITEM_NO"]);
                                    if (_iDetails.ContainsKey("REF_NO")) _item.Add(_iDetails["REF_NO"]); // Item Ref No
                                    else _item.Add("");
                                    _item.Add(_iDetails["NAME"].Trim()); // Item Description
                                    _item.Add(convert.ToDouble(_iDetails["QTY"]).ToString("0.00")); // Qty
                                    _item.Add(_iDetails["UNIT"].Trim()); // UOM
                                    _item.Add(convert.ToDouble(0).ToString("0.00")); // Qtd Qty
                                    _item.Add(convert.ToDouble(_iDetails["PRICE"]).ToString("0.00")); // Price
                                    _item.Add(convert.ToDouble(_iDetails["DISC"]).ToString("0.00")); // Disc

                                    _item.Add(""); //Equipment Maker 
                                    string equipRem = "";
                                    if (modal != "") equipRem = "Modal : " + modal;
                                    if (SerNo != "") equipRem += "Serial No. : " + SerNo;
                                    _item.Add(equipRem); //Equipment Remarks
                                    _item.Add(compName.Trim()); // Equip Name   

                                    string itemRem = "";
                                    if (_iDetails["PART_NO"].Trim() != "") itemRem = "Part No. : " + _iDetails["PART_NO"].Trim() + "; ";

                                    itemRem += "Draw No/ Pos No : " + (_iDetails.ContainsKey("DRAW_NO") ? _iDetails["DRAW_NO"].Trim() : "") + "/" + (_iDetails.ContainsKey("POS_NO") ? _iDetails["POS_NO"].Trim() : "") + "; ";
                                    if (_iDetails.ContainsKey("REMARKS")) itemRem += Environment.NewLine + _iDetails["REMARKS"].Trim();
                                    _item.Add(itemRem);
                                    _item.Add(_iDetails["ITEM_NO"].Trim()); // OrgSysRef

                                    CalculatedTotal = CalculatedTotal + convert.ToDouble(_iDetails["TOTAL"].Trim());

                                    _lineItems.Add(_item);
                                    _item = new List<string>();
                                    _iDetails.Clear();
                                }
                                
                                _iDetails.Add("ITEM_NO", no.Trim());
                                if (line.Substring(23).Trim().StartsWith("Name:"))
                                {
                                    _iDetails.Add("PART_NO", line.Trim().Split(' ')[1].Trim());
                                    _iDetails.Add("NAME", line.Substring(23, (71 - 23)).Replace("Name:", "").Trim());
                                    _iDetails.Add("REMARKS", "");

                                    string line1 = line.Substring(71).Trim();
                                    while (line1.Contains("  ")) line1 = line1.Replace("  ", " ");

                                    string[] arr = line1.Split(' ');
                                    if (arr.Length == 6)
                                    {
                                        _iDetails.Add("QTY", convert.ToDouble(arr[0]).ToString("0.00"));
                                        _iDetails.Add("UNIT", arr[1].Trim());
                                        _iDetails.Add("PRICE", convert.ToDouble(arr[2]).ToString("0.00"));
                                        if (currCode.Trim().Length == 0) currCode = convert.ToString(arr[3]).Trim();
                                        _iDetails.Add("DISC", convert.ToDouble(arr[4]).ToString("0.00"));
                                        _iDetails.Add("TOTAL", convert.ToDouble(arr[5]).ToString("0.00"));
                                    }
                                    else
                                    {
                                        _iDetails.Add("QTY", convert.ToDouble(arr[0]).ToString("0.00"));
                                        _iDetails.Add("UNIT", arr[1].Trim());
                                        _iDetails.Add("PRICE", convert.ToDouble(arr[2]).ToString("0.00"));
                                        if (currCode.Trim().Length == 0) currCode = convert.ToString(arr[3]).Trim();
                                        _iDetails.Add("DISC", convert.ToDouble(0).ToString("0.00"));
                                        _iDetails.Add("TOTAL", convert.ToDouble(arr[4]).ToString("0.00"));
                                    }
                                }
                                else if (line.Substring(18).Trim().StartsWith("Name:"))
                                {
                                    _iDetails.Add("PART_NO", line.Trim().Split(' ')[1].Trim());
                                    _iDetails.Add("NAME", line.Substring(19, (68 - 19)).Replace("Name:", "").Trim());
                                    _iDetails.Add("REMARKS", "");

                                    string line1 = line.Substring(68).Trim();
                                    while (line1.Contains("  ")) line1 = line1.Replace("  ", " ");

                                    string[] arr = line1.Split(' ');
                                    if (arr.Length == 5)
                                    {
                                        _iDetails.Add("QTY", convert.ToDouble(arr[0]).ToString("0.00"));
                                        _iDetails.Add("UNIT", arr[1].Trim());
                                        _iDetails.Add("PRICE", convert.ToDouble(arr[2]).ToString("0.00"));
                                        if (currCode.Trim().Length == 0) currCode = convert.ToString(arr[3]).Trim();
                                        _iDetails.Add("DISC", convert.ToDouble(0).ToString("0.00"));
                                        _iDetails.Add("TOTAL", convert.ToDouble(arr[4]).ToString("0.00"));
                                    }
                                    else
                                    {
                                        _iDetails.Add("QTY", convert.ToDouble(arr[0]).ToString("0.00"));
                                        _iDetails.Add("UNIT", arr[1].Trim());
                                        _iDetails.Add("PRICE", convert.ToDouble(arr[2]).ToString("0.00"));
                                        if (currCode.Trim().Length == 0) currCode = convert.ToString(arr[3]).Trim();
                                        _iDetails.Add("DISC", convert.ToDouble(arr[4]).ToString("0.00"));
                                        _iDetails.Add("TOTAL", convert.ToDouble(arr[5]).ToString("0.00"));
                                    }
                                }
                            }
                            else if (line.Trim().StartsWith("Maker Ref:"))
                            {
                                _iDetails.Add("REF_NO", line.Trim().Replace("Maker Ref:", "").Trim());
                            }
                            else if (line.Trim().StartsWith("Product Code:"))
                            {
                                _iDetails.Add("REF_NO", line.Trim().Replace("Maker Ref:", "").Trim());
                            }
                            else if (line.Trim().StartsWith("Dwg No:"))
                            {
                                _iDetails.Add("DRAW_NO", line.Trim().Replace("Dwg No:", "").Trim());
                            }
                            else if (line.Trim().StartsWith("Pos No:"))
                            {
                                _iDetails.Add("POS_NO", line.Trim().Replace("Pos No:", "").Trim());
                            }
                            else
                            {
                                // line.Length > 6 additional condition included on 28-JUL-16
                                if (line.Length > 6 && line.Substring(0, 6).Trim() == "" && _iDetails.ContainsKey("NAME"))
                                {
                                    line = line.PadRight(150);
                                    _iDetails["NAME"] += " " + line.Substring(23, (71 - 23)).Trim();
                                }
                                else
                                {
                                    if (_iDetails.ContainsKey("REMARKS")) _iDetails["REMARKS"] += Environment.NewLine + line.Trim();
                                    else _iDetails.Add("REMARKS", line.Trim());
                                }
                            }
                        }
                    }

                    if (_iDetails.Count > 0)
                    {
                        List<string> _item = new List<string>();
                        //_iDetails
                        _item.Add(_iDetails["ITEM_NO"]);
                        if (_iDetails.ContainsKey("REF_NO")) _item.Add(_iDetails["REF_NO"]); // Item Ref No
                        else _item.Add("");
                        _item.Add(_iDetails["NAME"].Trim()); // Item Description
                        _item.Add(convert.ToDouble(_iDetails["QTY"]).ToString("0.00")); // Qty
                        _item.Add(_iDetails["UNIT"].Trim()); // UOM
                        _item.Add(convert.ToDouble(0).ToString("0.00")); // Qtd Qty
                        _item.Add(convert.ToDouble(_iDetails["PRICE"]).ToString("0.00")); // Price
                        _item.Add(convert.ToDouble(_iDetails["DISC"]).ToString("0.00")); // Disc

                        _item.Add(""); //Equipment Maker 
                        string equipRem = "";
                        if (modal != "") equipRem = "Modal : " + modal;
                        if (SerNo != "") equipRem += "Serial No. : " + SerNo;
                        _item.Add(equipRem); //Equipment Remarks
                        _item.Add(compName.Trim()); // Equip Name   

                        string itemRem = "";
                        if (_iDetails["PART_NO"].Trim() != "") itemRem = "Part No. : " + _iDetails["PART_NO"].Trim() + "; ";
                        if (_iDetails.ContainsKey("DRAW_NO") || _iDetails.ContainsKey("POS_NO"))
                        {
                            itemRem += "Draw No/ Pos No : " + (_iDetails.ContainsKey("DRAW_NO") ? _iDetails["DRAW_NO"].Trim() : "") + "/" + (_iDetails.ContainsKey("POS_NO") ? _iDetails["POS_NO"].Trim() : "") + "; ";
                        }
                        if (_iDetails.ContainsKey("REMARKS")) itemRem += Environment.NewLine + _iDetails["REMARKS"].Trim();
                        _item.Add(itemRem);
                        _item.Add(_iDetails["ITEM_NO"].Trim()); // OrgSysRef

                        CalculatedTotal = CalculatedTotal + convert.ToDouble(_iDetails["TOTAL"].Trim());

                        _lineItems.Add(_item);
                    }
                }

                return _lineItems;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                try
                { txtData.Dispose(); }
                catch { }
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

        public override void SavePage(IEBrowser _ie, bool SavePNG, string PrintURL)
        {
            try
            {
                Console.WriteLine(this.CurrentTimeStamp + " : Saving Page");
                if (this.DocType == "RFQ")
                {
                    this.WebHost = _ie.Uri.Scheme + "://" + _ie.Uri.Host;
                    if (string.IsNullOrEmpty(this.HtmlFile)) this.HtmlFile = this.Domain + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".htm";
                    if (string.IsNullOrEmpty(this.ImgFile)) this.ImgFile = this.Domain + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".png";

                    // METHOD 1 //
                    //string _OuterContent = _ie.Frames[0].Body.OuterHtml;
                    //string _finalContent = "<html><head><title>#VRNO#</title></head>#INNER_BODY#</html>";
                    //_finalContent = _finalContent.Replace("#VRNO#", this.VRNO);
                    //_finalContent = _finalContent.Replace("#INNER_BODY#", _OuterContent.Trim());
                    //_finalContent = _finalContent.Replace("\"/Phoenix/css/Theme1", "\"" + this.WebHost + "/Phoenix/css/Theme1");
                    //File.WriteAllText(this.HTMLPath + "\\" + this.HtmlFile, _finalContent);

                    #region   // MATHOD 2  //
                    int pageCounter = 1; bool componentexist = true;
                    string reportURL = ""; string details = "";
                    WatiN.Core.Link _lnkRFQ = _ie.Frames[0].Link("MenuRegistersStockItem_dlstTabs_ctl02_btnMenu");
                    if (_lnkRFQ.Exists)
                    {
                        int _indx = _lnkRFQ.OuterHtml.IndexOf("../Reports/");
                        if (_indx > 0)
                        {
                            string _url1 = _lnkRFQ.OuterHtml.Substring(_indx);
                            reportURL = _url1.Substring(0, _url1.IndexOf("'"));
                            reportURL = reportURL.Replace("&amp;", "&");
                            reportURL = reportURL.Replace("..", this.WebHost + "/Phoenix");

                            using (IEBrowser ieReport = new IEBrowser())
                            {
                                ieReport.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Minimize);
                                ieReport.gotopage(reportURL);
                                ieReport.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Minimize);

                                Table tblReport = ieReport.Table("CrystalReportViewer1");
                                if (tblReport.Exists)
                                {
                                    WatiN.Core.Image _imgLogo = ieReport.Image(WatiN.Core.Find.ByAlt("Image"));
                                    if (_imgLogo.Exists)
                                    {
                                        _imgLogo.SetAttributeValue("src", "https://apps.southnests.com:443/Phoenix/css/Theme1/images/esmlogo4_small.png");
                                    }

                                    details = tblReport.OuterHtml;
                                    if (!details.Contains("Component&nbsp;Name‎"))
                                    {
                                        componentexist = false;
                                        details = tblReport.OuterHtml.Replace("width: 773px; height: 1039px; display: inline-block;", "width: 773px; height: 955px; display: inline-block;");
                                    }
                                    else
                                    {
                                        details = tblReport.OuterHtml.Replace("width: 773px; height: 1039px; display: inline-block;", "width: 773px; height: 1000px; display: inline-block;");
                                    }
                                    details = details.Replace("RFQ", "RFQ [ " + this.VRNO + " ] ");
                                }

                                string currentPageCount = "";
                                WatiN.Core.Div _divToolbar = ieReport.Div(WatiN.Core.Find.ByClass("crtoolbar"));
                                if (_divToolbar.Exists)
                                {
                                    currentPageCount = convert.ToString(_divToolbar.Tables[0].TableRows[0].TableCells[9].Text);
                                }

                                WatiN.Core.Image _imgNext = ieReport.Image(WatiN.Core.Find.ByTitle("Next"));

                                int btnClickCount = 0;
                                while (btnClickCount < 20 && _imgNext.Exists && (_imgNext.GetAttributeValue("disabled").Trim().ToLower() != "true" && _imgNext.GetAttributeValue("disabled").Trim().ToLower() != "disabled"))
                                {
                                    btnClickCount++;
                                    _imgNext.Click();
                                    Thread.Sleep(convert.ToInt(ConfigurationSettings.AppSettings["NEXT_CLICK_TIMER"]));

                                    // Wait till it loads new page //
                                    string newPageCount = currentPageCount;
                                    int counter = 0;
                                    while (true)
                                    {
                                        if (newPageCount != currentPageCount)
                                        {
                                            currentPageCount = newPageCount;
                                            break;
                                        }
                                        if (counter > 5) throw new Exception("Unable to load next page while saving data");
                                        Thread.Sleep(10000);
                                        counter++;
                                        _divToolbar = ieReport.Div(WatiN.Core.Find.ByClass("crtoolbar"));
                                        if (_divToolbar.Exists)
                                        {
                                            newPageCount = convert.ToString(_divToolbar.Tables[0].TableRows[0].TableCells[9].Text);
                                        }
                                    }
                                    
                                    pageCounter++;
                                    tblReport = ieReport.Table("CrystalReportViewer1");
                                    if (tblReport.Exists)
                                    {
                                        string _pageData = tblReport.OuterHtml;
                                        if (pageCounter == 2)
                                        {
                                            if (componentexist) _pageData = _pageData.Replace("width: 773px; height: 1039px; display: inline-block;", "width: 773px; height: 890px; display: inline-block;");
                                            else _pageData = _pageData.Replace("width: 773px; height: 1039px; display: inline-block;", "width: 773px; height: 955px; display: inline-block;");
                                        }
                                        else _pageData = _pageData.Replace("width: 773px; height: 1039px; display: inline-block;", "width: 773px; height: 890px; display: inline-block;");
                                        details += _pageData;
                                    }
                                    _imgNext = ieReport.Image(WatiN.Core.Find.ByTitle("Next"));
                                }

                                try
                                {
                                    ieReport.Close();
                                    //ieReport.Dispose();
                                }
                                catch { }
                                finally
                                {
                                    Thread.Sleep(1000);
                                }

                            } //using block end
                        }
                    }

                    string _finalContent = "<html><meta http-equiv=\"Content-type\" content=\"text/html;charset=UTF-8\"><head><title>#VRNO#</title><style>.crtoolbar{display:none !important;}</style></head><body>#INNER_BODY#</body></html>";
                    _finalContent = _finalContent.Replace("#VRNO#", this.VRNO);
                    _finalContent = _finalContent.Replace("#INNER_BODY#", details.Trim());

                    File.WriteAllText(this.HTMLPath + "\\" + this.HtmlFile, _finalContent);
                    Thread.Sleep(3000);
                    #endregion

                    if (File.Exists(this.HTMLPath + "\\" + this.HtmlFile))
                    {
                        Console.WriteLine(this.CurrentTimeStamp + " : HTML file saved successfully.");
                        Thread.Sleep(500);

                        using (System.Windows.Forms.WebBrowser wb = new System.Windows.Forms.WebBrowser())
                        {
                            wb.Width = 900;
                            wb.ScrollBarsEnabled = false;
                            wb.ScriptErrorsSuppressed = true;
                            wb.Navigate(this.HTMLPath + "\\" + this.HtmlFile);
                            while (wb.ReadyState != System.Windows.Forms.WebBrowserReadyState.Complete) System.Windows.Forms.Application.DoEvents();
                            wb.Width = wb.Document.Body.ScrollRectangle.Size.Width - 100;
                            wb.Height = wb.Document.Body.ScrollRectangle.Size.Height;
                            using (Bitmap bitmap = new Bitmap(wb.Width, wb.Height))
                            {
                                wb.DrawToBitmap(bitmap, new Rectangle(0, 0, wb.Width, wb.Height));
                                Rectangle rect = new Rectangle(0, 0, wb.Width, wb.Height);
                                Bitmap cropped = bitmap.Clone(rect, bitmap.PixelFormat);
                                cropped.Save(this.ImagePath + @"\" + this.ImgFile, System.Drawing.Imaging.ImageFormat.Png);
                                Thread.Sleep(2000);

                                if (File.Exists(this.ImagePath + @"\" + this.ImgFile))
                                {
                                    Console.WriteLine(this.CurrentTimeStamp + " : PNG file saved successfully.");
                                }
                                else this.ImgFile = "";
                            }
                        }
                    }
                }
            }
            catch (System.AccessViolationException exAcc)
            {
                this.ImgFile = "";
                SetLog("AccessViolationException Error in SavePage() : " + exAcc.StackTrace);
            }
            catch (Exception ex)
            {
                this.ImgFile = "";
                SetLog("Error in SavePage() : " + ex.Message);
            }
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
                    this.SetLog(this.CurrentTimeStamp + " : " + fileinfo.Name + " File moved to " + DestinationPath);
                }
                else
                {
                    errLog = "Destination path is blank.";
                    throw new Exception("Destination path is blank.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(this.CurrentTimeStamp + " : Error in MoveFile() - " + ex.Message);
                this.SetLog(this.CurrentTimeStamp + " : Error in MoveFile() - " + ex.Message);
            }
        }

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
                extractedText = extractedText.Replace("\0", " ");

                // updated //
                //extractedText = extractedText.Replace("\r\r", "\r");
                //string BlanckLines = Environment.NewLine + Environment.NewLine;
                //while (extractedText.Contains(BlanckLines))
                //{
                //    extractedText = extractedText.Replace(BlanckLines, Environment.NewLine);
                //}
                return extractedText.Trim();
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
