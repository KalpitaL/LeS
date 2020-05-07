/* Updates
 * 16-JUN-2017 : Unable to add extra items properly. Modified & uploaded on server
 * 11-JUL-2018 : Unable to select currency as regex pattern was not matching with live ie 
 * 12-MAR-2019 : Updated to throw 'Quotation is already approved' error instead of throwing 'Quote total mismatched' error 
 * 05-MAR-2020 : Updated to increase wait period for loading Remark's Popup Window & Send Quote Button click. Replaced '<br>' with new line char while updating Declined Quote's Remark
 */

using MTML.GENERATOR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DotNetBrowserWrapper;
using DotNetBrowser;
using DotNetBrowser.DOM;
using LeSDataMain;

namespace Phoenix_QuotePOC_Uploader
{
    public class PhoenixRoutine
    {
        string VRNO = "", QuoteRef = "", errorImg = "",
            _Domain = "", UploaderPath = "",
            AuditLogPath = "", SuppCode = "",
            Module = "", ImagePath = "", BuyerCode = "",
          Server = "", Processor = "";
        bool saveAsDraft = false, notifyByText = false;
        List<int> uploadedItems = new List<int>();
        MTMLClass _mtmlClass = new MTMLClass();

        public NetBrowser _netBrowser = null;

        public PhoenixRoutine()
        {
            try
            {
                _netBrowser = new NetBrowser();

                this.Module = "PHOENIX_QUOTE";

                UploaderPath = convert.ToString(ConfigurationManager.AppSettings["QUOTE_POC_PATH"]);
                AuditLogPath = convert.ToString(ConfigurationManager.AppSettings["AUDIT_LOG_PATH"]);
                ImagePath = convert.ToString(ConfigurationManager.AppSettings["IMAGE_PATH"]);
                Server = convert.ToString(ConfigurationManager.AppSettings["SERVER"]);
                Processor = convert.ToString(ConfigurationManager.AppSettings["PROCESSOR"]);
            }
            catch
            { }
        }

        public void UploadFiles(System.Windows.Forms.Form frm)
        {
            if (!IsPhoenixExeRunning())
            {
                try
                {
                    // Check Upload Files Directory //
                    DirectoryInfo _dirQuotePoc = new DirectoryInfo(this.UploaderPath);
                    if (!_dirQuotePoc.Exists) _dirQuotePoc.Create();

                    if (_dirQuotePoc.Exists)
                    {
                        FileInfo[] files = _dirQuotePoc.GetFiles("*.xml");
                        if (files.Length > 0)
                        {
                            foreach (FileInfo xmlfile in files)
                            {
                                this.ProcessQuotefile(xmlfile, frm);
                            }
                        }
                        else
                        {
                            LeSDM.AddConsoleLog("No file found to upload.");
                        }
                    }
                    else
                    {
                        LeSDM.AddConsoleLog("Upload files directory does not exists.");
                    }
                }
                catch (Exception ex)
                {
                    LeSDM.AddConsoleLog("Error in UploadFiles - " + ex.Message);
                }
            }
        }

        private void MoveFile(string DestinationPath, FileInfo fileinfo)
        {
            try
            {
                DestinationPath += "\\" + DateTime.Now.Year + "\\" + DateTime.Now.Month;
                if (DestinationPath.Trim().Length > 0)
                {
                    if (!Directory.Exists(DestinationPath.Trim())) Directory.CreateDirectory(DestinationPath.Trim());

                    string _newFile = (DestinationPath + "\\" + fileinfo.Name).Trim();
                    if (File.Exists(_newFile)) File.Delete(_newFile);
                    File.Move(fileinfo.FullName, _newFile);

                    LeSDM.AddConsoleLog("" + fileinfo.Name + " File moved to " + DestinationPath);
                }
                else throw new Exception("Destination path is blank.");
            }
            catch (Exception ex)
            {
                LeSDM.AddConsoleLog("Error in MoveFile() - " + ex.Message);
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
                            if (prdName.ToString().ToUpper().Trim() == "Phoenix_QuotePOC_Uploader")
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

        private void ProcessQuotefile(FileInfo _quoteFile,System.Windows.Forms.Form frm)
        {
            try
            {
                this.SuppCode = "";
                this.BuyerCode = "";
                this.VRNO = "";
                this.QuoteRef = "";

                if (_quoteFile.Exists)
                {
                    MTMLInterchange obj = _mtmlClass.Load(_quoteFile.FullName);
                    this.BuyerCode = obj.Recipient.Trim().Split('_')[0].Trim();
                    this.SuppCode = obj.Sender.Trim().Split('_')[0].Trim();
                    string _DOCTYPE = obj.DocumentHeader.DocType.ToUpper().Trim();

                    string msgURL = convert.ToString(obj.DocumentHeader.MessageReferenceNumber).Trim();

                    if (_DOCTYPE == "QUOTE" || _DOCTYPE == "QUOTATION")
                    {
                        this.Module = "PHOENIX_QUOTE";

                        if (msgURL.ToLower().Contains("phoenixtelerik"))
                        {
                            frm.WindowState = System.Windows.Forms.FormWindowState.Minimized;

                            PhoenixTelerikRoutine ptRoutine = new PhoenixTelerikRoutine();
                            ptRoutine.UploadQuote(obj, _quoteFile);

                            frm.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                        }
                        else
                        {
                            UploadQuote(obj, _quoteFile);

                            // Code not yet complete
                            //PhoenixHttpRoutine phRoutine = new PhoenixHttpRoutine();
                            //phRoutine.UploadQuote(obj, _quoteFile);
                        }
                    }
                    else
                    {
                        this.Module = "PHOENIX_POC";

                        #region/* Get VRNO No. */
                        foreach (Reference _ref in obj.DocumentHeader.References)
                        {
                            if (_ref.Qualifier == ReferenceQualifier.UC) VRNO = _ref.ReferenceNumber;
                            else if (_ref.Qualifier == ReferenceQualifier.AAG) QuoteRef = _ref.ReferenceNumber;
                        }
                        #endregion

                        if (obj.DocumentHeader.IsDeclined)
                        {
                            LeSDM.AddConsoleLog("PO '" + VRNO + "' is declined.");
                            LeSDM.SetAuditLog(this.Module, _quoteFile.Name, VRNO.Trim(), "Error", "Unable to confirm PO. PO '" + VRNO + "' is declined.", this.BuyerCode, this.SuppCode, this.Server, this.Processor);
                            MoveFile(this.UploaderPath + "\\Error_Files", _quoteFile);
                            this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                        }
                        else
                        {
                            // Check PO Amount > 0
                            double POAmount = 0;
                            foreach (MonetaryAmount _mAmount in obj.DocumentHeader.MonetoryAmounts)
                            {
                                if (_mAmount.Qualifier == MonetoryAmountQualifier.GrandTotal_259)
                                {
                                    POAmount = _mAmount.Value;
                                    break;
                                }
                            }

                            if (POAmount > 0)
                            {
                                UploadPOC(obj, _quoteFile);
                            }
                            else
                            {
                                LeSDM.AddConsoleLog("PO amount is 0.");
                                LeSDM.SetAuditLog(this.Module, _quoteFile.Name, VRNO.Trim(), "Error", "Unable to confirm PO '" + VRNO.Trim() + "'.PO amount is 0.", this.BuyerCode, this.SuppCode, this.Server, this.Processor);
                                MoveFile(this.UploaderPath + "\\Error_Files", _quoteFile);
                                this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                            }
                        }
                    }
                }
                else
                {
                    LeSDM.AddConsoleLog("'" + _quoteFile.Name + "' file not found.");
                }
            }
            catch (Exception ex)
            {
                LeSDM.AddConsoleLog("Unable to upload file '" + _quoteFile.Name + "'. Error - " + ex.Message);
                LeSDM.SetAuditLog(this.Module, _quoteFile.Name, VRNO, "Error", "Error in UploadFile() - " + ex.Message, this.BuyerCode, SuppCode, Server, Processor);
                MoveFile(this.UploaderPath + "\\Error_Files", _quoteFile);
                this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
            }
        }

        private void UploadQuote(MTMLInterchange obj, FileInfo xmlFile)
        {
            string VRNO = "", suppRef = "", QuoteExp = "", LatestDeliveryDate = "", Remarks = "", TermsCond = "", Currency = "", BuyerCode = "", WebURL = "", VendorCode = "";
            double AddDiscount = 0;

            try
            {
                this.QuoteRef = "";
                int LeadDays = 0;
                double FrieghtCost = 0, PackingCost = 0, OtherCost = 0, DepositCost = 0, AdditionalCost = 0, TaxCost = 0, FinalAmt = 0, AllowanceAmount = 0, extraItemCost = 0;

                BuyerCode = obj.Recipient.ToString();
                VendorCode = obj.Sender.ToString();
                this.SuppCode = VendorCode.ToUpper();

                string orgURL = convert.ToString(obj.DocumentHeader.MessageReferenceNumber).Trim();

                #region/* Get VRNO No. */
                foreach (Reference _ref in obj.DocumentHeader.References)
                {
                    if (_ref.Qualifier == ReferenceQualifier.AAG) { this.QuoteRef = suppRef = _ref.ReferenceNumber; }
                    else if (_ref.Qualifier == ReferenceQualifier.UC) { VRNO = _ref.ReferenceNumber; }
                }
                this.VRNO = VRNO;
                #endregion

                LeSDM.AddConsoleLog("Uploading Quote '" + this.VRNO + "' ");
                if (!(orgURL.Contains("PurchaseQuotationItems.aspx") || orgURL.Contains("PurchaseQuotationRFQ.aspx")))
                {
                    throw new Exception("Invalid URL in MessageReferenceNumber");
                }
                orgURL = orgURL.Replace("PurchaseQuotationRFQ", "PurchaseQuotationItems");

                // Load Web Page
                _netBrowser.LoadUrl(orgURL, "Title1_lblTitle", false, false);
                Thread.Sleep(3000);

                DOMElement _titleField = GetElement("Title1_lblTitle");
                DOMElement lnkSaveQuote = GetElement("MenuQuotationLineItem_dlstTabs_ctl00_btnMenu");

                if (_titleField != null && convert.ToString(_titleField.TextContent).Contains(VRNO)
                    && lnkSaveQuote != null && convert.ToString(lnkSaveQuote.TextContent).Trim().ToUpper() == "SAVE") // validate page //
                {
                    #region // Set Quote Info //
                    Currency = convert.ToString(obj.DocumentHeader.CurrencyCode).Trim();
                    LeadDays = convert.ToInt(obj.DocumentHeader.LeadTimeDays);
                    AddDiscount = convert.ToDouble(obj.DocumentHeader.AdditionalDiscount);

                    foreach (Comments _comm in obj.DocumentHeader.Comments)
                    {
                        if (_comm.Qualifier == CommentTypes.SUR)
                        {
                            Remarks = Convert.ToString(_comm.Value);
                        }
                        else if (_comm.Qualifier == CommentTypes.ZTC)
                        {
                            TermsCond = "Terms & Condition : " + convert.ToString(_comm.Value);
                        }
                    }
                    Remarks = TermsCond + Environment.NewLine + Remarks;

                    foreach (DateTimePeriod dt in obj.DocumentHeader.DateTimePeriods)
                    {
                        if (dt.Qualifier == DateTimePeroidQualifiers.ExpiryDate_36)
                        {
                            QuoteExp = dt.Value;
                        }
                        else if (dt.Qualifier == DateTimePeroidQualifiers.LatestDeliveryDate_2)
                        {
                            LatestDeliveryDate = dt.Value;
                        }
                    }

                    // Check for Default values also
                    if (QuoteExp.Trim() == "")
                    {
                        string[] defaultQuoteExpDays = convert.ToString(ConfigurationManager.AppSettings["QUOTE_VALID"]).Trim().Split('|');
                        foreach (string sQuoteExpDays in defaultQuoteExpDays)
                        {
                            string[] values = sQuoteExpDays.Split('=');
                            if (values[0].Trim() == VendorCode.Trim() && values.Length > 1 && convert.ToInt(values[1]) > 0)
                            {
                                QuoteExp = DateTime.Now.AddDays(convert.ToInt(values[1])).ToString("yyyyMMddHHmm");
                                break;
                            }
                        }
                    }

                    // Check for Save As Draft flag
                    saveAsDraft = false;
                    string[] saveAsDraftValues = convert.ToString(ConfigurationManager.AppSettings["SAVE_AS_DRAFT"]).Trim().Split('|');
                    foreach (string strValue in saveAsDraftValues)
                    {
                        string[] values = strValue.Split('=');
                        if (values[0].Trim().ToUpper() == VendorCode.Trim().ToUpper() && values.Length > 1 && convert.ToInt(values[1]) > 0)
                        {
                            saveAsDraft = true; break;
                        }
                    }

                    foreach (MonetaryAmount _amount in obj.DocumentHeader.MonetoryAmounts)
                    {
                        if (_amount.Qualifier == MonetoryAmountQualifier.PackingCost_106)
                        {
                            PackingCost = convert.ToDouble(_amount.Value);
                        }
                        else if (_amount.Qualifier == MonetoryAmountQualifier.FreightCharge_64)
                        {
                            FrieghtCost += convert.ToDouble(_amount.Value);
                        }
                        else if (_amount.Qualifier == MonetoryAmountQualifier.OtherCost_98)
                        {
                            OtherCost = convert.ToDouble(_amount.Value);
                        }
                        else if (_amount.Qualifier == MonetoryAmountQualifier.Deposit_97)
                        {
                            DepositCost = convert.ToDouble(_amount.Value);
                        }
                        else if (_amount.Qualifier == MonetoryAmountQualifier.PackingCost_106)
                        {
                            AdditionalCost = convert.ToDouble(_amount.Value);
                        }
                        else if (_amount.Qualifier == MonetoryAmountQualifier.TaxCost_99)
                        {
                            TaxCost = convert.ToDouble(_amount.Value);
                        }
                        else if (_amount.Qualifier == MonetoryAmountQualifier.GrandTotal_259)
                        {
                            FinalAmt = convert.ToDouble(_amount.Value);
                        }
                        else if (_amount.Qualifier == MonetoryAmountQualifier.AllowanceAmount_204)
                        {
                            AllowanceAmount = convert.ToDouble(_amount.Value);
                        }
                    }
                    #endregion

                    #region // Fill Quote Header //

                    #region // Quote Reference No. //
                    LeSDM.AddConsoleLog("Filling Supp Ref ");
                    DOMInputElement txtSuppRef = (DOMInputElement)this.GetElement("txtVenderReference");
                    if (txtSuppRef != null)
                    {
                        txtSuppRef.Focus(); Thread.Sleep(500);
                        _netBrowser.ClearText();
                        _netBrowser.InputKeys(suppRef, 50);
                    }
                    else throw new Exception("txtSuppRef field not found on webpage.");
                    #endregion

                    #region  // Quote Validity Date //
                    LeSDM.AddConsoleLog("Filling quote validity date ");
                    DateTime dtExpiry = DateTime.Now.AddDays(LeadDays);  //DateTime.MinValue;
                    DateTime.TryParseExact(QuoteExp.Trim(), "yyyyMMddHHmm", null, System.Globalization.DateTimeStyles.None, out dtExpiry);
                    if (dtExpiry != DateTime.MinValue)
                    {
                        if (dtExpiry < DateTime.Now) dtExpiry = DateTime.Now;
                        string _quoteExp = dtExpiry.ToString("dd/MMM/yyyy");
                        DOMInputElement txtQuoteExp = (DOMInputElement)this.GetElement("txtOrderDate");
                        if (txtQuoteExp != null)
                        {
                            try
                            {
                                txtQuoteExp.Focus(); Thread.Sleep(500);
                                _netBrowser.ClearText();
                                _netBrowser.InputKeys(_quoteExp.Replace("-", "/"), 100);
                            }
                            catch
                            {
                                txtQuoteExp.SetAttribute("Value", _quoteExp);
                            }
                        }
                        else throw new Exception("txtOrderDate field not found.");
                    }
                    #endregion

                    #region // Latest Delivery Days //
                    LeSDM.AddConsoleLog("Filling latest delivery days ");
                    DOMInputElement txtDelDate = (DOMInputElement)this.GetElement("txtDeliveryTime_txtNumber");
                    if (txtDelDate != null)
                    {
                        try
                        {
                            Thread.Sleep(500);
                            txtDelDate.Focus(); Thread.Sleep(500);
                            txtDelDate.SetAttribute("Value", LeadDays.ToString());
                        }
                        catch
                        {
                            txtDelDate.SetAttribute("Value", LeadDays.ToString());
                        }
                    }
                    else throw new Exception("txtDeliveryTime_txtNumber field not found.");
                    #endregion

                    #region // Currency //
                    LeSDM.AddConsoleLog("Filling currency ");
                    if (Currency.Trim().Length > 0)
                    {
                        DOMSelectElement _selCurrency = (DOMSelectElement)this.GetElement("ucCurrency_ddlCurrency");
                        if (_selCurrency != null)
                        {
                            _selCurrency.Focus(); Thread.Sleep(500);
                            string _value = "";

                            #region Get Currency Value
                            string currencies = convert.ToString(_selCurrency.InnerHTML).Trim().ToUpper().Replace("SELECTED=\"SELECTED\"", "").Replace("SELECTED", "");
                            if (currencies.Contains(">" + Currency.Trim().ToUpper() + "<"))
                            {
                                System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(currencies, "<OPTION[\\s]+VALUE=\"[0-9]*\">" + Currency.Trim().ToUpper() + "</OPTION>");

                                // Additional Condition added as inner html is not getting as local test
                                if (match == null || (match != null && match.Success == false))
                                    match = System.Text.RegularExpressions.Regex.Match(currencies, "<OPTION[\\s]+VALUE=[0-9]*>" + Currency.Trim().ToUpper() + "</OPTION>");
                                if (match != null && match.Success)
                                {
                                    string _currValue = convert.ToString(match.Value);
                                    match = System.Text.RegularExpressions.Regex.Match(_currValue, "[0-9]+");
                                    if (match != null && match.Success)
                                    {
                                        _value = convert.ToString(match.Value);
                                    }
                                }
                            }
                            #endregion

                            if (_value.Trim().Length > 0)
                            {
                                _netBrowser.SelectElementItem(_selCurrency, true, _value.Trim());
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                // Updated on 29-NOV-16
                                throw new Exception("Currency code provided in quote file not found in select list.");
                            }
                        }
                        else throw new Exception("ucCurrency_ddlCurrency field not found.");
                    }
                    else throw new Exception("Currecy not found in MTML Quote file");
                    #endregion

                    #region // Mode Of Transport //
                    LeSDM.AddConsoleLog("Filling Mode of Transport ");
                    string optModeOfTransport = "";
                    if (obj.DocumentHeader.TransportModeCode == TransportModeCode.Air)
                    {
                        // Select Air
                        optModeOfTransport = "344";
                    }
                    else if (obj.DocumentHeader.TransportModeCode == TransportModeCode.Maritime)
                    {
                        // select Sea
                        optModeOfTransport = "345";
                    }
                    else if (obj.DocumentHeader.TransportModeCode == TransportModeCode.Road)
                    {
                        // select Road
                        optModeOfTransport = "346";
                    }

                    if (optModeOfTransport.Trim() != "")
                    {
                        DOMSelectElement _selTransportMode = (DOMSelectElement)this.GetElement("ucModeOfTransport_ddlQuick");
                        if (_selTransportMode != null)
                        {
                            _netBrowser.SelectElementItem(_selTransportMode, true, optModeOfTransport);
                            Thread.Sleep(1000);
                        }
                    }
                    #endregion

                    #region // Delivery Terms //
                    LeSDM.AddConsoleLog("Filling Delivery Terms");
                    string[] delTerms = convert.ToString(ConfigurationManager.AppSettings["DEL_TERMS"]).Trim().Split('|');
                    Dictionary<string, string> lstDelTerms = new Dictionary<string, string>();
                    foreach (string s in delTerms)
                    {
                        string[] strValues = s.Trim().Split('=');
                        lstDelTerms.Add(strValues[0], strValues[1]);
                    }

                    if (!lstDelTerms.ContainsKey(VendorCode)) throw new Exception("No setting found for Delivery Terms of Vendor [" + VendorCode + "] ");
                    string currDelTerms = lstDelTerms[VendorCode];

                    string _delTermsValue = "";
                    DOMSelectElement _selDelTerms = (DOMSelectElement)this.GetElement("UCDeliveryTerms_ddlQuick");
                    if (_selDelTerms != null && currDelTerms.Trim().Length > 0)
                    {
                        string _delTerms = convert.ToString(_selDelTerms.InnerHTML).Trim().ToUpper().Replace("SELECTED=\"SELECTED\"", "").Replace("SELECTED", "");
                        System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(_delTerms, "<OPTION[\\s]+VALUE=\"[0-9]+\">" + currDelTerms.Trim().ToUpper().Replace("&", "&AMP;") + "</OPTION>");
                        if (match != null && match.Success == false) match = System.Text.RegularExpressions.Regex.Match(_delTerms, "<OPTION[\\s]+VALUE=[0-9]+>" + currDelTerms.Trim().ToUpper().Replace("&", "&AMP;") + "</OPTION>");
                        if (match != null && match.Success)
                        {
                            string _currValue = convert.ToString(match.Value);
                            match = System.Text.RegularExpressions.Regex.Match(_currValue, "[0-9]+");
                            if (match != null && match.Success)
                            {
                                _delTermsValue = convert.ToString(match.Value);
                            }
                        }
                    }

                    if (_delTermsValue.Trim().Length > 0)
                    {
                        _netBrowser.SelectElementItem(_selDelTerms, true, _delTermsValue.Trim());
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        if (currDelTerms.Trim().Length > 0) throw new Exception(currDelTerms + " Delivery Terms not found");
                    }
                    #endregion

                    #region  // Payterms //
                    LeSDM.AddConsoleLog("Filling Payterms");
                    string[] payTerms = convert.ToString(ConfigurationManager.AppSettings["PAY_TERMS"]).Trim().Split('|');
                    Dictionary<string, string> lstPayTerms = new Dictionary<string, string>();
                    foreach (string s in payTerms)
                    {
                        string[] strValues = s.Trim().Split('=');
                        lstPayTerms.Add(strValues[0], strValues[1]);
                    }

                    if (!lstPayTerms.ContainsKey(VendorCode)) throw new Exception("No setting found for Payment Terms of Vendor [" + VendorCode + "] ");
                    string currPayTerms = lstPayTerms[VendorCode];
                    DOMSelectElement _selPayterms = (DOMSelectElement)this.GetElement("UCPaymentTerms_ddlQuick");
                    if (_selPayterms != null)
                    {
                        string paytermsValue = "";
                        string payterms = convert.ToString(_selPayterms.InnerHTML).Trim().ToUpper().Replace("SELECTED=\"SELECTED\"", "").Replace("SELECTED", "");
                        System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(payterms, "<OPTION[\\s]+VALUE=\"[0-9]+\">" + currPayTerms.Trim().ToUpper().Replace("&", "&AMP;") + "</OPTION>");
                        if (match != null && match.Success == false) match = System.Text.RegularExpressions.Regex.Match(payterms, "<OPTION[\\s]+VALUE=[0-9]+>" + currPayTerms.Trim().ToUpper().Replace("&", "&AMP;") + "</OPTION>");
                        if (match != null && match.Success)
                        {
                            string _Value = convert.ToString(match.Value);
                            match = System.Text.RegularExpressions.Regex.Match(_Value, "[0-9]+");
                            if (match != null && match.Success)
                            {
                                paytermsValue = convert.ToString(match.Value);
                            }
                        }

                        if (paytermsValue.Trim().Length > 0)
                        {
                            //_selPayterms.SelectByValue(paytermsValue.Trim());
                            _netBrowser.SelectElementItem(_selPayterms, true, paytermsValue.Trim());
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            if (currPayTerms.Trim().Length > 0) throw new Exception(currPayTerms + " PayTerms not found");
                        }
                    }
                    #endregion

                    #region // Additional Discount //
                    /* Fill Header Discount (Modified for AMOS/WSS  if field exist then fill it)*/
                    if (AddDiscount > 0)
                    {
                        LeSDM.AddConsoleLog("Filling additional discount ");
                        DOMInputElement txtAddDiscount = (DOMInputElement)this.GetElement("txtSupplierDiscount");
                        if (txtAddDiscount != null)
                        {
                            txtAddDiscount.Focus(); Thread.Sleep(500);
                            _netBrowser.ClearText();
                            _netBrowser.InputKeys(AddDiscount.ToString("0.00"));
                        }
                        else
                        {
                            // Append Additional Discount to header comments
                            Remarks = "Additional Discount in % : " + AddDiscount + Environment.NewLine + Remarks;
                        }
                    }
                    else if (AllowanceAmount > 0) // Discounted Amount (Fuji)
                    {
                        LeSDM.AddConsoleLog("Filling allowance amount ");
                        DOMInputElement txtTotalDiscountAmt = (DOMInputElement)this.GetElement("txtTotalDiscount");
                        if (txtTotalDiscountAmt != null)
                        {
                            txtTotalDiscountAmt.Focus(); Thread.Sleep(500);
                            //_netBrowser.ClearText();
                            //_netBrowser.InputKeys(AllowanceAmount.ToString("0.00"));

                            txtTotalDiscountAmt.SetAttribute("Value", AllowanceAmount.ToString());
                            Remarks = "Allowance : " + AllowanceAmount + " " + Currency + Environment.NewLine + Remarks;
                        }
                    }
                    #endregion

                    SaveQuote();

                    #region // Fill Freight Cost (If it exists) in First Additional Description //

                    LeSDM.AddConsoleLog("Filling extra charges..");

                    if (FrieghtCost > 0 || PackingCost > 0)
                    {
                        _netBrowser.LoadUrl(orgURL);
                        Thread.Sleep(3000);
                    }
                    int counter = 0;

                    AddExtraCost("Freight Cost", FrieghtCost);

                    #region // Commented on 10-OCT-19 //
                    //DOMInputElement txtFreightDescr = null;
                    //DOMElement imgDeleteItem = this.GetElement("gvTax_ctl02_cmdDelete");
                    //if (imgDeleteItem != null) _netBrowser.LoadUrl(orgURL, "Title1_lblTitle", false, false); // Reload page

                    //imgDeleteItem = this.GetElement("gvTax_ctl02_cmdDelete");
                    //while (imgDeleteItem != null)
                    //{
                    //    _netBrowser.ClickElementbyID("gvTax_ctl02_cmdDelete", false, true);
                    //    Thread.Sleep(2000);
                    //    imgDeleteItem = this.GetElement("gvTax_ctl02_cmdDelete");
                    //}

                    //txtFreightDescr = (DOMInputElement)this.GetElement("gvTax_ctl03_txtDescriptionAdd");
                    //if (txtFreightDescr != null && (FrieghtCost > 0) && convert.ToFloat(TaxCost) == 0)
                    //{
                    //    txtFreightDescr.Focus(); Thread.Sleep(500);
                    //    txtFreightDescr.SetAttribute("Value", "Freight Cost");

                    //    DOMInputElement rdbValue = (DOMInputElement)this.GetElement("gvTax_ctl03_ucTaxTypeAdd_rblValuePercentage_0");
                    //    if (rdbValue != null)
                    //    {
                    //        rdbValue.Checked = true;
                    //        rdbValue.Blur(); Thread.Sleep(1000);
                    //    }

                    //    DOMInputElement txtFreightValue = (DOMInputElement)this.GetElement("gvTax_ctl03_txtValueAdd");
                    //    if (txtFreightValue != null)
                    //    {
                    //        // Set Fright Cost in textbox //
                    //        string amount = convert.ToFloat(FrieghtCost).ToString("0.00");
                    //        txtFreightValue.Focus(); Thread.Sleep(500);
                    //        _netBrowser.ClearText();
                    //        _netBrowser.InputKeys("0\t"); Thread.Sleep(1000);

                    //        txtFreightValue.Click(); txtFreightValue.Focus();
                    //        _netBrowser.InputKeys(amount); Thread.Sleep(2000);
                    //        counter++;

                    //        // Save Current Record //
                    //        DOMElement imgAdd = this.GetElement("gvTax_ctl03_cmdAdd");
                    //        if (imgAdd != null)
                    //        {
                    //            imgAdd.Click();
                    //            Thread.Sleep(2000);

                    //            #region // Check Error //
                    //            string ErrorMsg = "";
                    //            DOMElement _divError = this.GetElement("ucError_pnlErrorMessage");
                    //            if (_divError != null)
                    //            {
                    //                DOMElement spnError = this.GetElement("ucError_spnErrorMessage");
                    //                if (spnError != null)
                    //                {
                    //                    ErrorMsg = convert.ToString(spnError.TextContent);
                    //                }
                    //            }

                    //            if (ErrorMsg.Trim().Length > 0)
                    //            {
                    //                // Get Error Screen Shot //
                    //                string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                    //                _netBrowser.PrintPDF(errImg, false);
                    //                if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                    //                else errImg = "";

                    //                throw new Exception(ErrorMsg);
                    //            }
                    //            #endregion
                    //        }
                    //    }
                    //    else
                    //    {
                    //        throw new Exception("Fright Cost value field not found");
                    //    }
                    //}
                    #endregion

                    #region // Packing Cost //
                    if (PackingCost > 0)
                    {
                        LeSDM.AddConsoleLog("Filling Packing Cost ");
                        AddExtraCost("Packing Cost", PackingCost);
                    }
                    #endregion
                    #endregion

                    SaveQuote();

                    #region // Quote Remarks //
                    if (Remarks.Trim().Length > 0)
                    {
                        _netBrowser.LoadUrl(orgURL, "Title1_lblTitle", false, false);
                        Thread.Sleep(5000); // Wait For 5 Seconds After Loading

                        _netBrowser.CurrentDocument = _netBrowser.browser.GetDocument();

                        LeSDM.AddConsoleLog("Filling Quote Remarks ");
                        DOMElement _lnkUpdRemarks = this.GetElement("MenuQuotationLineItem_dlstTabs_ctl03_btnMenu");
                        if (_lnkUpdRemarks != null)
                        {
                            LeSDM.AddConsoleLog("Open Remarks Popup.. ");
                            _lnkUpdRemarks.Click();
                            Thread.Sleep(10000);
                            _netBrowser.CurrentDocument = _netBrowser.browser.GetDocument();

                            int waitCounterPopup = 0;
                            DOMElement _imgHTMLWriter = this.GetElement("txtFormDetails_ucCustomEditor_ctl03_ctl01");
                            while (_imgHTMLWriter == null)
                            {
                                Thread.Sleep(5000);
                                waitCounterPopup++;
                                if (waitCounterPopup > 10) break;
                             
                                _netBrowser.CurrentDocument = _netBrowser.browser.GetDocument();
                                _imgHTMLWriter = this.GetElement("txtFormDetails_ucCustomEditor_ctl03_ctl01");
                            }

                            if (_imgHTMLWriter != null)
                            {
                                #region // Edit Header Remarks //
                                _imgHTMLWriter.Click();
                                Thread.Sleep(1000);

                                DOMElement txtRemarks = this.GetElement("txtFormDetails_ucCustomEditor_ctl02_ctl01");
                                if (txtRemarks != null)
                                {
                                    Remarks = Remarks.Trim().Replace(Environment.NewLine, "<br>");
                                    txtRemarks.Focus(); Thread.Sleep(500);
                                    _netBrowser.ClearText();
                                    _netBrowser.InputKeys(Remarks.Trim(), 70);
                                }

                                DOMElement _imgHTMLDesigner = this.GetElement("txtFormDetails_ucCustomEditor_ctl03_ctl02");
                                if (_imgHTMLDesigner != null)
                                {
                                    _imgHTMLDesigner.Click();
                                    Thread.Sleep(1000);
                                }
                                #endregion

                                #region // Save Header Remarks //
                                DOMElement lnkSaveRemarks = this.GetElement("MenuFormDetail_dlstTabs_ctl00_btnMenu");
                                if (lnkSaveRemarks != null)
                                {
                                    lnkSaveRemarks.Click();
                                    Thread.Sleep(8000);
                                }
                                else
                                {
                                    #region // Search Save Button & Save //
                                    DOMElement tblMenu = this.GetElement("MenuFormDetail_dlstTabs");
                                    if (tblMenu != null)
                                    {
                                        if (tblMenu.GetElementsByTagName("a").Count > 0)
                                        {
                                            lnkSaveRemarks = (DOMElement)tblMenu.GetElementsByTagName("a")[0];
                                            if (lnkSaveRemarks != null)
                                            {
                                                lnkSaveRemarks.Click();
                                                Thread.Sleep(8000);
                                            }
                                            else throw new Exception("Save button not found to update remarks");
                                        }
                                        else throw new Exception("Save button not found to update remarks");
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            else throw new Exception("Unable to open remark popup");

                            #region // Close Quote Remark Window //
                            try
                            {
                                // Close Window 
                                DOMElement closeDialogBtn = _netBrowser.GetElementFromFrames("xfCIco", true, false, false);
                                if (closeDialogBtn != null)
                                {
                                    closeDialogBtn.Click();
                                    Thread.Sleep(1000);
                                }
                            }
                            catch (Exception ex)
                            { }
                            #endregion
                        }
                    }
                    #endregion

                    _netBrowser.LoadUrl(orgURL);
                    Thread.Sleep(3000);
                    #endregion

                    List<LineItem> extraItems = new List<LineItem>();
                    if (convert.ToInt(obj.DocumentHeader.IsDeclined) == 0)
                    {
                        extraItems = FillItems(obj); // Fill Quoted Items //

                        int byrItemCount = obj.DocumentHeader.LineItems.Count - extraItems.Count;
                        if (byrItemCount != uploadedItems.Count)
                        {
                            string missingItems = "";
                            foreach (LineItem item in obj.DocumentHeader.LineItems)
                            {
                                if (item.IsExtraItem == 0 && !uploadedItems.Contains(convert.ToInt(item.Number)))
                                {
                                    missingItems += "," + item.Number;
                                }
                            }
                            throw new Exception("Item(s) " + missingItems.Trim(',') + " missing on web portal");
                        }

                        foreach (LineItem eItem in extraItems)
                        {
                            if (convert.ToString(eItem.ItemType) == "")
                            {
                                extraItemCost += eItem.MonetaryAmount;
                            }
                        }

                        // Relaod after items are saved
                        _netBrowser.LoadUrl(orgURL);
                        Thread.Sleep(3000);

                        if (extraItems.Count > 0 && counter == 0)
                        {
                            FillExtraItems(extraItems);
                            SaveQuote();
                        }

                        // Relaod after items are saved
                        _netBrowser.LoadUrl(orgURL);
                        Thread.Sleep(2000);
                    }

                    #region // Recheck Final Amount if additional discount or allowance is applied //
                    if ((AddDiscount > 0 || AllowanceAmount > 0) && FinalAmt > 0)
                    {
                        if (AddDiscount > 0)
                        {
                            FinalAmt = (FinalAmt + (AddDiscount * FinalAmt / 100));
                        }
                        else if (AllowanceAmount > 0)
                        {
                            FinalAmt = (FinalAmt + AllowanceAmount);
                        }
                    }
                    #endregion

                    LeSDM.AddConsoleLog("SaveAsDraft flag- " + saveAsDraft.ToString());

                    if (saveAsDraft == false)
                    {
                        if (convert.ToInt(obj.DocumentHeader.IsDeclined) == 0) // QUOTE IS NOT DECLINED //
                        {
                            SendQuote(obj, suppRef.Trim(), FinalAmt, extraItemCost, xmlFile, BuyerCode.Trim(), VendorCode.Trim());
                        }
                        else
                        {
                            DeclineQuote(obj, suppRef.Trim(), xmlFile, BuyerCode.Trim(), VendorCode.Trim(), Remarks.Trim());
                        }
                    }
                    else
                    {
                        SaveQuote(obj, suppRef.Trim(), FinalAmt, extraItemCost, xmlFile, BuyerCode.Trim(), VendorCode.Trim());
                    }
                }
                else
                {
                    SetupQuoteError(xmlFile, BuyerCode.Trim(), VendorCode.Trim());
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (errorImg.Length == 0) errorImg = xmlFile.Name;

                    LeSDM.AddConsoleLog("Unable to upload quote '" + VRNO + "'. Error - " + ex.Message);
                    LeSDM.AddConsoleLog(ex.StackTrace);

                    if (!ex.Message.Contains("Index was outside the bounds of the array")
                        && !ex.Message.Contains("Access is denied")
                        && !ex.Message.Contains("Object reference not set"))
                    {
                        // Set Error Log //
                        TrackErrors.TrackError.SetErrorCount(SuppCode, VRNO.Trim(), "WEBSITE_ERROR");
                        int errCount = TrackErrors.TrackError.GetErrorCount(SuppCode, VRNO.Trim(), "WEBSITE_ERROR");
                        if (errCount == 3)
                        {
                            LeSDM.SetAuditLog(this.Module, errorImg, VRNO.Trim(), "Error", "Unable to upload quote '" + VRNO + "'; Error - " + ex.Message, BuyerCode, SuppCode, Server, Processor);
                            TrackErrors.TrackError.DeleteErrorCountFile(SuppCode, VRNO.Trim(), "WEBSITE_ERROR");
                            MoveFile(xmlFile.Directory.FullName + "\\Error_Files", xmlFile); // Move File to Error Files Folder
                            this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                        }
                    }
                }
                finally { }
            }
        }

        private void AddExtraCost(string Description, double Cost)
        {
            int rowNo = IsExtraItemExists(convert.ToString(Description));

            if (rowNo >= 2)
            {
                #region // Update Existing Extra Item Cost //

                DOMElement imgEditItem = this.GetElement("gvTax_ctl0" + rowNo + "_cmdEdit");
                if (imgEditItem != null)
                {
                    imgEditItem.Click();
                    Thread.Sleep(5000);

                    DOMInputElement txtDescr = (DOMInputElement)this.GetElement("gvTax_ctl0" + rowNo + "_txtDescriptionEdit");
                    if (txtDescr != null)
                    {
                        txtDescr.SetAttribute("Value", convert.ToString(Description));
                        Thread.Sleep(500);
                    }

                    DOMInputElement rdvValue = (DOMInputElement)this.GetElement("gvTax_ctl0" + rowNo + "_ucTaxTypeEdit_rblValuePercentage_0");
                    if (rdvValue != null)
                    {
                        rdvValue.Checked = true; Thread.Sleep(2000);
                    }

                    DOMInputElement txtPrice = (DOMInputElement)this.GetElement("gvTax_ctl0" + rowNo + "_txtValueEdit");
                    if (txtPrice != null)
                    {
                        double webPrice = convert.ToDouble(txtPrice.GetAttribute("Value"));

                        if (Math.Round(webPrice, 2) != Math.Round(convert.ToDouble(Cost), 2))
                        {
                            string _price = convert.ToDouble(Cost).ToString("0.00");

                            txtPrice.Focus(); Thread.Sleep(500);
                            _netBrowser.ClearText();
                            _netBrowser.InputKeys("0\t"); Thread.Sleep(1000);

                            txtPrice.Click(); txtPrice.Focus();
                            _netBrowser.InputKeys(_price); Thread.Sleep(2000);
                        }
                    }

                    // Save Current Record //
                    DOMElement imgSaveAmount = GetElement("gvTax_ctl0" + rowNo + "_cmdSave");
                    if (imgSaveAmount != null)
                    {
                        imgSaveAmount.Click();
                        Thread.Sleep(2000);

                        #region // Check Error //
                        string ErrorMsg = "";
                        DOMElement _divError = GetElement("ucError_pnlErrorMessage");
                        if (_divError != null)
                        {
                            DOMElement spnError = GetElement("ucError_spnErrorMessage");
                            if (spnError != null)
                            {
                                ErrorMsg = convert.ToString(spnError.TextContent);
                            }
                        }

                        if (ErrorMsg.Trim().Length > 0)
                        {
                            // Get Error Screen Shot //
                            string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                            _netBrowser.PrintPDF(errImg, false);
                            if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                            else errImg = "";

                            throw new Exception(ErrorMsg);
                        }
                        #endregion
                    }
                }
                else throw new Exception("Extra item's edit button not found.");

                #endregion
            }
            else
            {
                #region // Add New Extra Item Cost //
                DOMElement gvTax = GetElement("gvTax");
                int newRowCount = gvTax.GetElementsByTagName("tr").Count - 1;
                string _rowNum = "";
                if (newRowCount <= 9) _rowNum = "0" + newRowCount.ToString();
                else _rowNum = newRowCount.ToString();

                DOMElement btnAdd = GetElement("gvTax_ctl" + _rowNum + "_cmdAdd");

                DOMInputElement txtFreightDescr = (DOMInputElement)GetElement("gvTax_ctl" + _rowNum + "_txtDescriptionAdd");
                if (txtFreightDescr != null && Cost > 0)
                {
                    txtFreightDescr.Focus(); Thread.Sleep(2000);
                    _netBrowser.ClearText();
                    _netBrowser.InputKeys(convert.ToString(Description).Trim(), 20);
                    Thread.Sleep(500);

                    DOMInputElement txtFreightValue = (DOMInputElement)GetElement("gvTax_ctl" + _rowNum + "_txtValueAdd");
                    Thread.Sleep(500);

                    DOMInputElement rdbValue = (DOMInputElement)GetElement("gvTax_ctl" + _rowNum + "_ucTaxTypeAdd_rblValuePercentage_0");
                    if (rdbValue != null)
                    {
                        rdbValue.Checked = true;
                        Thread.Sleep(500);
                        rdbValue.Blur();
                    }
                    else throw new Exception("Charge radio button not found.");

                    if (txtFreightValue != null)
                    {
                        int matchCounter = 0;
                        while (Math.Round(convert.ToFloat(txtFreightValue.Value)) != Math.Round(Cost))
                        {

                            txtFreightValue.Focus(); Thread.Sleep(500);
                            _netBrowser.ClearText();
                            _netBrowser.InputKeys("0\t"); Thread.Sleep(1000);

                            txtFreightValue.Click(); txtFreightValue.Focus();
                            _netBrowser.InputKeys(Cost.ToString("0.00"));
                            Thread.Sleep(2000);

                            matchCounter++;
                            if (matchCounter > 10) break;
                        }
                        double amt = Math.Round(convert.ToFloat(txtFreightValue.Value), 2);
                        if (amt != Math.Round(Cost, 2))
                        {
                            LeSDM.AddConsoleLog("Extra cost in textbox -" + convert.ToFloat(txtFreightValue.Value) + " & in MTML file " + Cost);
                            throw new Exception("Unable to update extra cost " + Cost + " in charges.");
                        }
                    }
                    else throw new Exception("Charge value not found.");

                    if (txtFreightDescr == null) throw new Exception("Charge Description field not found");

                    // Click Add Item button to add in quote

                    if (btnAdd != null)
                    {
                        btnAdd.Click();
                        Thread.Sleep(7000);

                        #region // Check Error //
                        string ErrorMsg = "";
                        DOMElement _divError = GetElement("ucError_pnlErrorMessage");
                        if (_divError != null)
                        {
                            DOMElement spnError = GetElement("ucError_spnErrorMessage");
                            if (spnError != null)
                            {
                                ErrorMsg = convert.ToString(spnError.TextContent);
                            }
                        }

                        if (ErrorMsg.Trim().Length > 0)
                        {
                            // Get Error Screen Shot //
                            string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                            _netBrowser.PrintPDF(errImg, false);
                            if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                            else errImg = "";

                            throw new Exception(ErrorMsg);
                        }
                        #endregion
                    }
                }
                #endregion
            }
        }

        private void UploadPOC(MTMLInterchange obj, FileInfo xmlFile)
        {
            string suppPOCRef = "", Remarks = "", DelDate = "", BuyerCode = "", VendorCode = "";
            try
            {
                this.VRNO = "";
                LeSDM.AddConsoleLog("Reading file " + xmlFile.Name);
                BuyerCode = obj.Recipient.ToString();
                VendorCode = obj.Sender.ToString();

                this.SuppCode = VendorCode.Trim().ToUpper();
                string MsgNo = convert.ToString(obj.DocumentHeader.MessageReferenceNumber).Trim();

                #region/* Get VRNO & POC Ref No. */
                foreach (Reference _ref in obj.DocumentHeader.References)
                {
                    if (_ref.Qualifier == ReferenceQualifier.AAG) suppPOCRef = _ref.ReferenceNumber.Trim();
                    else if (_ref.Qualifier == ReferenceQualifier.UC) VRNO = _ref.ReferenceNumber.Trim();
                }
                #endregion

                #region /* Get POC Remarks*/
                foreach (Comments _comment in obj.DocumentHeader.Comments)
                {
                    if (_comment.Qualifier == CommentTypes.SUR)
                    {
                        Remarks = convert.ToString(_comment.Value).Trim();
                        break;
                    }
                }
                if (suppPOCRef.Trim().Length > 0)
                {
                    Remarks = "POC Ref No. :" + suppPOCRef + ";" + Remarks.Trim();
                }
                #endregion

                #region /* Get Delivery Date */
                foreach (DateTimePeriod _date in obj.DocumentHeader.DateTimePeriods)
                {
                    if (_date.Qualifier == DateTimePeroidQualifiers.DeliveryDate_69)
                    {
                        if (_date.Value != "")
                        {
                            DateTime _deliveryDate = DateTime.MinValue;
                            DateTime.TryParseExact(_date.Value, "yyyyMMddHHmm", null, System.Globalization.DateTimeStyles.None, out _deliveryDate);
                            if (_deliveryDate != DateTime.MinValue)
                            {
                                DelDate = _deliveryDate.ToString("dd/MMM/yyyy");
                            }
                        }
                        break;
                    }
                }
                #endregion

                _netBrowser.LoadUrl(MsgNo);
                Thread.Sleep(2000);

                LeSDM.AddConsoleLog("Confirming POC '" + this.VRNO + "' ");

                // Enter Delivery Date
                DOMInputElement txtDelDate = (DOMInputElement)this.GetElement("txtDeliveryDate");
                if (txtDelDate != null && DelDate.Trim().Length > 0)
                {
                    txtDelDate.Focus(); Thread.Sleep(500);
                    _netBrowser.ClearText();
                    _netBrowser.InputKeys(DelDate.Replace("-", "/"), 100);
                    Thread.Sleep(500);
                }

                // Setup Remarks //                    
                DOMElement txtRemarks = this.GetElement("txtRemarks_ctl02_ctl00");
                if (txtRemarks != null)
                {
                    txtRemarks.Focus(); Thread.Sleep(500);
                    _netBrowser.ClearText();
                    _netBrowser.InputKeys(Remarks.Trim(), 50);
                }

                // Save Details 
                DOMElement lnkSave = this.GetElement("MenuFormRemarks_dlstTabs_ctl00_btnMenu");
                if (lnkSave != null)
                {
                    lnkSave.Click();
                    Thread.Sleep(5000);

                    #region // Set Audit Log //
                    string quoteImg = this.ImagePath + "\\Phoenix_POC_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                    _netBrowser.PrintPDF(quoteImg, false);
                    if (File.Exists(quoteImg)) quoteImg = Path.GetFileName(quoteImg);
                    else quoteImg = "";

                    LeSDM.AddConsoleLog("Purchase Order '" + VRNO + "' confirmed successfully.");
                    MoveFile(xmlFile.Directory.FullName + "\\Backup", xmlFile); // Move File to Backup
                    LeSDM.SetAuditLog(this.Module, quoteImg, VRNO.Trim(), "Confirmed", "Purchase Order '" + VRNO + "' confirmed successfully.", BuyerCode, SuppCode, Server, Processor);
                    this.SendTextNotification(VRNO.Trim(), QuoteRef, "SUCCESS", SuppCode);
                    #endregion
                }
                else
                {
                    throw new Exception("Save button not found in MsgNo");
                }
            }
            catch (Exception ex)
            {
                try
                {
                    string errImg = this.ImagePath + "\\Phoenix_POCError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                    _netBrowser.PrintPDF(errImg, false);
                    if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                    else errImg = "";

                    LeSDM.AddConsoleLog("Unable to confirm poc '" + VRNO + "'. Error - " + ex.Message);
                    LeSDM.AddLog(ex.StackTrace);
                    LeSDM.SetAuditLog(this.Module, xmlFile.Name, VRNO.Trim(), "Error", "Unable to confirm poc '" + VRNO + "'; Domain - " + this._Domain + ". Error - " + ex.Message, BuyerCode, SuppCode, Server, Processor);
                    this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);

                    MoveFile(xmlFile.Directory.FullName + "\\Error_Files", xmlFile);
                }
                finally { }
            }
        }

        private void SetupQuoteError(FileInfo xmlFile, string BuyerCode, string VendorCode)
        {
            DOMElement eleTitle = this.GetElement("Title1_lblTitle");
            if (eleTitle.TextContent.Contains(VRNO))
            {
                string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                _netBrowser.PrintPDF(errImg, false);
                if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                else errImg = "";

                DOMElement lnkSaveQuote = this.GetElement("MenuQuotationLineItem_dlstTabs_ctl00_btnMenu");
                if (lnkSaveQuote == null || (lnkSaveQuote != null && convert.ToString(lnkSaveQuote.TextContent).Trim().ToUpper() != "SAVE"))
                {
                    LeSDM.AddConsoleLog("Quote for RFQ '" + VRNO + "' is already submitted.");
                    MoveFile(xmlFile.Directory.FullName + "\\Error_Files", xmlFile); // Move File to Error_Files

                    LeSDM.SetAuditLog(this.Module, errImg, VRNO.Trim(), "Error", "Quote for RFQ '" + VRNO + "' is already submitted.", BuyerCode, SuppCode, Server, Processor);
                    this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                }
            }
            else
            {
                string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                _netBrowser.PrintPDF(errImg, false);
                if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                else errImg = "";

                DOMElement lnkSaveQuote = this.GetElement("MenuQuotationLineItem_dlstTabs_ctl00_btnMenu");
                if (lnkSaveQuote == null || (lnkSaveQuote != null && convert.ToString(lnkSaveQuote.TextContent).Trim().ToUpper() != "SAVE"))
                {
                    LeSDM.AddConsoleLog("Unable to load page for RFQ '" + VRNO + "' ");

                    TrackErrors.TrackError.SetErrorCount(VRNO, "OPEN_PAGE");
                    int count = TrackErrors.TrackError.GetErrorCount(VRNO, "OPEN_PAGE");
                    if (count == 3)
                    {
                        LeSDM.SetAuditLog(this.Module, errImg, VRNO.Trim(), "Error", "Unable to load page for RFQ '" + VRNO + "'", BuyerCode, SuppCode, Server, Processor);
                        MoveFile(xmlFile.Directory.FullName + "\\Error_Files", xmlFile); // Move File to Error_Files
                        TrackErrors.TrackError.DeleteErrorCountFile(VRNO.Trim(), "OPEN_PAGE");
                        this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                    }
                }
            }
        }

        private void FillExtraItems(List<LineItem> extraItems)
        {
            Thread.Sleep(2000);

            int nCount = 3, itemCounter = 0;
            foreach (LineItem _eItem in extraItems)
            {
                // Fill Only Delivery/Tax/Others Charge Description //
                if (convert.ToString(_eItem.ItemType).Trim() != "")
                {
                    if (itemCounter > 8) throw new Exception("extra items more than 8");

                    LeSDM.AddConsoleLog("Filling " + _eItem.Description);
                    int rowNo = IsExtraItemExists(convert.ToString(_eItem.Description));

                    if (rowNo >= 2)
                    {
                        #region // Update Existing Extra Item Cost //

                        DOMElement imgEditItem = this.GetElement("gvTax_ctl0" + rowNo + "_cmdEdit");
                        if (imgEditItem != null)
                        {
                            imgEditItem.Click();
                            Thread.Sleep(5000);

                            DOMInputElement txtDescr = (DOMInputElement)this.GetElement("gvTax_ctl0" + rowNo + "_txtDescriptionEdit");
                            if (txtDescr != null)
                            {
                                txtDescr.SetAttribute("Value", convert.ToString(_eItem.Description));
                                Thread.Sleep(500);
                            }

                            DOMInputElement rdvValue = (DOMInputElement)this.GetElement("gvTax_ctl0" + rowNo + "_ucTaxTypeEdit_rblValuePercentage_0");
                            if (rdvValue != null)
                            {
                                rdvValue.Checked = true; Thread.Sleep(2000);
                            }

                            DOMInputElement txtPrice = (DOMInputElement)this.GetElement("gvTax_ctl0" + rowNo + "_txtValueEdit");
                            if (txtPrice != null)
                            {
                                double webPrice = convert.ToDouble(txtPrice.GetAttribute("Value"));

                                if (Math.Round(webPrice, 2) != Math.Round(convert.ToDouble(_eItem.MonetaryAmount), 2))
                                {
                                    string _price = convert.ToDouble(_eItem.MonetaryAmount).ToString("0.00");

                                    txtPrice.Focus(); Thread.Sleep(500);
                                    _netBrowser.ClearText();
                                    _netBrowser.InputKeys("0\t"); Thread.Sleep(1000);

                                    txtPrice.Click(); txtPrice.Focus();
                                    _netBrowser.InputKeys(_price); Thread.Sleep(2000);
                                }
                            }

                            // Save Current Record //
                            DOMElement imgSaveAmount = GetElement("gvTax_ctl0" + rowNo + "_cmdSave");
                            if (imgSaveAmount != null)
                            {
                                imgSaveAmount.Click();
                                Thread.Sleep(2000);

                                #region // Check Error //
                                string ErrorMsg = "";
                                DOMElement _divError = GetElement("ucError_pnlErrorMessage");
                                if (_divError != null)
                                {
                                    DOMElement spnError = GetElement("ucError_spnErrorMessage");
                                    if (spnError != null)
                                    {
                                        ErrorMsg = convert.ToString(spnError.TextContent);
                                    }
                                }

                                if (ErrorMsg.Trim().Length > 0)
                                {
                                    // Get Error Screen Shot //
                                    string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                                    _netBrowser.PrintPDF(errImg, false);
                                    if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                                    else errImg = "";

                                    throw new Exception(ErrorMsg);
                                }
                                #endregion
                            }
                        }
                        else throw new Exception("Extra item's edit button not found.");

                        #endregion
                    }
                    else
                    {
                        #region // Add New Extra Item Cost //
                        DOMElement gvTax = GetElement("gvTax");
                        int newRowCount = gvTax.GetElementsByTagName("tr").Count - 1;
                        string _rowNum = "";
                        if (newRowCount <= 9) _rowNum = "0" + newRowCount.ToString();
                        else _rowNum = newRowCount.ToString();

                        DOMElement btnAdd = GetElement("gvTax_ctl" + _rowNum + "_cmdAdd");

                        DOMInputElement txtFreightDescr = (DOMInputElement)GetElement("gvTax_ctl" + _rowNum + "_txtDescriptionAdd");
                        if (txtFreightDescr != null && _eItem.MonetaryAmount > 0)
                        {
                            txtFreightDescr.Focus(); Thread.Sleep(500);
                            _netBrowser.ClearText();
                            _netBrowser.InputKeys(convert.ToString(_eItem.Description).Trim(), 20);
                            Thread.Sleep(500);

                            DOMInputElement txtFreightValue = (DOMInputElement)GetElement("gvTax_ctl" + _rowNum + "_txtValueAdd");
                            Thread.Sleep(500);

                            DOMInputElement rdbValue = (DOMInputElement)GetElement("gvTax_ctl" + _rowNum + "_ucTaxTypeAdd_rblValuePercentage_0");
                            if (rdbValue != null)
                            {
                                rdbValue.Checked = true;
                                Thread.Sleep(500);
                                rdbValue.Blur();
                            }
                            else throw new Exception("Charge radio button not found.");

                            if (txtFreightValue != null)
                            {
                                int matchCounter = 0;
                                while (Math.Round(convert.ToFloat(txtFreightValue.Value)) != Math.Round(_eItem.MonetaryAmount))
                                {

                                    txtFreightValue.Focus(); Thread.Sleep(500);
                                    _netBrowser.ClearText();
                                    _netBrowser.InputKeys("0\t"); Thread.Sleep(1000);

                                    txtFreightValue.Click(); txtFreightValue.Focus();
                                    _netBrowser.InputKeys(_eItem.MonetaryAmount.ToString("0.00"));
                                    Thread.Sleep(2000);

                                    matchCounter++;
                                    if (matchCounter > 10) break;
                                }
                                double amt = Math.Round(convert.ToFloat(txtFreightValue.Value), 2);
                                if (amt != Math.Round(_eItem.MonetaryAmount, 2))
                                {
                                    LeSDM.AddConsoleLog("Extra cost in textbox -" + convert.ToFloat(txtFreightValue.Value) + " & in MTML file " + _eItem.MonetaryAmount + ", for item -" + _eItem.Number);
                                    throw new Exception("Unable to update extra cost " + _eItem.MonetaryAmount + " in charges.");
                                }
                            }
                            else throw new Exception("Charge value not found.");

                            if (txtFreightDescr != null) nCount++;
                            else throw new Exception("Charge Description field not found");

                            // Click Add Item button to add in quote

                            if (btnAdd != null)
                            {
                                btnAdd.Click();
                                Thread.Sleep(5000);

                                #region // Check Error //
                                string ErrorMsg = "";
                                DOMElement _divError = GetElement("ucError_pnlErrorMessage");
                                if (_divError != null)
                                {
                                    DOMElement spnError = GetElement("ucError_spnErrorMessage");
                                    if (spnError != null)
                                    {
                                        ErrorMsg = convert.ToString(spnError.TextContent);
                                    }
                                }

                                if (ErrorMsg.Trim().Length > 0)
                                {
                                    // Get Error Screen Shot //
                                    string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                                    _netBrowser.PrintPDF(errImg, false);
                                    if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                                    else errImg = "";

                                    throw new Exception(ErrorMsg);
                                }
                                #endregion
                            }
                        }
                        #endregion
                    }

                    itemCounter++;
                }
            }
        }

        private int IsExtraItemExists(string ItemDescr)
        {
            try
            {
                int nCount = 2, rowCount = 0, returnRowNo = 0;
                DOMElement _div2 = GetElement("div2");
                if (_div2 != null) rowCount = _div2.GetElementsByTagName("table")[0].GetElementsByTagName("tr").Count;

                for (int i = nCount; i < rowCount; i++)
                {
                    string _itemDescr = "gvTax_ctl0" + i + "_lblDescriptionEdit";
                    DOMElement spnExItemDescr = GetElement(_itemDescr);

                    if (spnExItemDescr != null)
                    {
                        if (convert.ToString(spnExItemDescr.TextContent).ToUpper() == ItemDescr.Trim().ToUpper())
                        {
                            returnRowNo = i;
                            break;
                        }
                    }
                }

                return returnRowNo;
            }
            catch (Exception ex)
            {
                LeSDM.AddConsoleLog("Error in IsExtraItemExists(); " + ex.Message);
                throw ex;
            }
        }

        private List<LineItem> FillItems(MTMLInterchange _interchange)
        {
            try
            {
                Dictionary<string, string> lstRemarks = new Dictionary<string, string>();

                LeSDM.AddConsoleLog("Reload WebPage ");
                string orgURL = _interchange.DocumentHeader.MessageReferenceNumber;
                _netBrowser.LoadUrl(orgURL);
                Thread.Sleep(1000);

                LeSDM.AddConsoleLog("Filling items ");
                Dictionary<int, LineItem> _items = new Dictionary<int, LineItem>();
                List<LineItem> _extraItems = new List<LineItem>();
                List<string> lstUnits = new List<string>();
                int pageCounter = 1;
                bool itemPageLoaded = false;
                uploadedItems.Clear();

                #region // Sort Items //
                foreach (LineItem item in _interchange.DocumentHeader.LineItems)
                {
                    if (convert.ToInt(item.IsExtraItem) == 1)
                    {
                        _extraItems.Add(item);
                    }
                    else _items.Add(convert.ToInt(item.OriginatingSystemRef), item);
                }
                #endregion

                do
                {
                    _netBrowser.CurrentDocument = _netBrowser.browser.GetDocument();
                    DOMElement tblItems = null;
                    List<DOMNode> rows = null;
                    int itemCount = 0;
                    string ctlID = "";

                    tblItems = GetElement("gvVendorItem");
                    if (tblItems == null) tblItems = _netBrowser.GetElementbyId("gvVendorItem");

                    if (convert.ToString(tblItems.TextContent).Contains("NO RECORDS FOUND") && itemCount == 2)
                    {
                        throw new Exception("No item found on web portal");
                    }
                    rows = tblItems.GetElementsByTagName("tr");
                    itemCount = rows.Count;

                    for (int i = 1; i < itemCount; i++)
                    {
                        LineItem iObj = null;
                        tblItems = GetElement("gvVendorItem");
                        if (tblItems == null) tblItems = _netBrowser.GetElementbyId("gvVendorItem");
                        int counter = 0;
                        while (tblItems == null)
                        {
                            Thread.Sleep(500); counter++;
                            _netBrowser.CurrentDocument = _netBrowser.browser.GetDocument();
                            tblItems = _netBrowser.GetElementbyId("gvVendorItem");
                            if (counter > 20) break;
                        }

                        if (tblItems == null) throw new Exception("Item Grid not found");
                        rows = tblItems.GetElementsByTagName("tr");
                        itemCount = rows.Count;
                        List<DOMNode> cells = rows[i].GetElementsByTagName("td");

                        DOMElement spn = cells[0].GetElementByTagName("span");
                        string _content = convert.ToString(spn.TextContent).Trim();
                        if (_content.Trim() == "")
                        {
                            rows = tblItems.GetElementsByTagName("tr");
                            cells = rows[i].GetElementsByTagName("td");
                            _content = convert.ToString(cells[0].TextContent).Trim();
                        }

                        if (_content.Trim().Length > 0)
                        {
                            int ItemNo = convert.ToInt(_content);
                            double _price = 0, _disc = 0;

                            // Check Item exists in Item List //
                            if (_items.ContainsKey(ItemNo))
                            {
                                string iAddRemarks = "";
                                iObj = _items[ItemNo];

                                if (iObj.Quantity > 0)
                                {
                                    DOMElement spnTotal = cells[11].GetElementByTagName("span");
                                    string priceUpdated = spnTotal.TextContent;
                                    DOMElement _imgRemarks = null;

                                    DOMElement spnId = cells[0].GetElementByTagName("span");
                                    string id = spnId.GetAttribute("Id");
                                    string[] _strFields = id.Split('_');
                                    ctlID = _strFields[1].Trim();

                                    foreach (PriceDetails amt in iObj.PriceList)
                                    {
                                        if (amt.TypeQualifier == PriceDetailsTypeQualifiers.GRP) _price = amt.Value;
                                        else if (amt.TypeQualifier == PriceDetailsTypeQualifiers.DPR) _disc = amt.Value;
                                    }

                                    bool uomFound = false;
                                    bool updateUOM = true;

                                    #region // Get Current UOM //
                                    DOMElement lblUnit = GetElement("gvVendorItem_" + ctlID + "_lblunit");
                                    if (lblUnit != null)
                                    {
                                        if (convert.ToString(lblUnit.TextContent).Trim().ToUpper() == convert.ToString(iObj.MeasureUnitQualifier).Trim().ToUpper())
                                        {
                                            uomFound = true;
                                            updateUOM = false;
                                        }
                                    }
                                    #endregion

                                    LeSDM.AddConsoleLog("Updating item " + convert.ToString(iObj.Number));
                                    if (convert.ToFloat(iObj.MonetaryAmount) > 0)
                                    {
                                        #region // Start editing items - Click Edit button //
                                        DOMElement _imgEditItem = GetElement("gvVendorItem_" + ctlID + "_cmdEdit");
                                        if (_imgEditItem != null)
                                        {
                                            _imgEditItem.Focus();
                                            _imgEditItem.Click(); Thread.Sleep(500);
                                            int maxTry = 5;
                                            while (GetElement("gvVendorItem_" + ctlID + "_ucUnit_ddlUnit") == null)
                                            {
                                                if (maxTry == 0) break; Thread.Sleep(1500);
                                                _netBrowser.CurrentDocument = _netBrowser.browser.GetDocument();
                                                maxTry--;
                                            }
                                        }
                                        #endregion

                                        DOMInputElement txtQty = (DOMInputElement)GetElement("gvVendorItem_" + ctlID + "_txtQuantityEdit_txtNumber");
                                        if (txtQty != null) txtQty.Focus(); Thread.Sleep(200);

                                        DOMSelectElement selUnit = (DOMSelectElement)GetElement("gvVendorItem_" + ctlID + "_ucUnit_ddlUnit");
                                        DOMInputElement txtPrice = (DOMInputElement)GetElement("gvVendorItem_" + ctlID + "_txtQuotedPriceEdit_txtNumber");
                                        DOMInputElement txtDiscount = (DOMInputElement)GetElement("gvVendorItem_" + ctlID + "_txtDiscountEdit_txtNumber");
                                        DOMInputElement txtLeadDays = (DOMInputElement)GetElement("gvVendorItem_" + ctlID + "_txtDeliveryTimeEdit_txtNumber");

                                        if (updateUOM)
                                        {
                                            #region // Set UOM //
                                            try
                                            {
                                                string selectedVal = convert.ToString(selUnit.Value).Trim().ToUpper();
                                                if (selectedVal == convert.ToString(iObj.MeasureUnitQualifier).Trim().ToUpper())
                                                {
                                                    uomFound = true;
                                                }
                                                else
                                                {
                                                    string _units = convert.ToString(selUnit.InnerHTML).Trim().ToUpper().Replace("SELECTED=\"SELECTED\"", ""); ;
                                                    System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(_units.Trim(), "<OPTION[\\s]+VALUE=\"[0-9]+\">" + convert.ToString(iObj.MeasureUnitQualifier).Trim().ToUpper() + "</OPTION>");
                                                    if (match != null && match.Success)
                                                    {
                                                        string _Value = convert.ToString(match.Value);
                                                        match = System.Text.RegularExpressions.Regex.Match(_Value, "[0-9]+");
                                                        if (match != null && match.Success)
                                                        {
                                                            string _uomValue = convert.ToString(match.Value);
                                                            _netBrowser.SelectElementItem(selUnit, true, _uomValue);
                                                            uomFound = true;
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                LeSDM.AddConsoleLog("Unable to update UOM for item " + ItemNo + ". Error - " + ex.Message);
                                            }
                                            #endregion
                                        }

                                        if (uomFound)
                                        {
                                            // Update QTY
                                            //txtQty.Focus(); Thread.Sleep(100);
                                            txtQty.SetAttribute("Value", convert.ToString(convert.ToFloat(iObj.Quantity)));
                                            Thread.Sleep(50);

                                            // Update Price
                                            //txtPrice.Focus(); Thread.Sleep(100);
                                            txtPrice.SetAttribute("Value", _price.ToString());
                                            Thread.Sleep(50);

                                            // Update Discount
                                            //txtDiscount.Focus(); Thread.Sleep(100);
                                            txtDiscount.SetAttribute("Value", _disc.ToString());
                                            Thread.Sleep(50);

                                            // Check UOM //                                            
                                            string selectedValue = convert.ToString(selUnit.Value);
                                            if (selectedValue.Trim().ToLower() == "dummy")
                                            {
                                                throw new Exception("Unable to set UOM for item no. " + iObj.Number);
                                            }
                                        }
                                        else
                                        {
                                            string selectedValue = convert.ToString(selUnit.Value);
                                            if (selectedValue.Trim().ToLower() == "dummy")
                                            {
                                                if (selUnit.Options.Count == 2)
                                                {
                                                    string optText = convert.ToString(selUnit.Options[1].TextContent).Trim().ToUpper();
                                                    if (optText == iObj.Byr_UOM.ToUpper())
                                                    {
                                                        _netBrowser.SelectElementItem(selUnit, true, selUnit.Options[1].GetAttribute("Value"));
                                                        uomFound = true;
                                                    }
                                                }
                                                else if (selUnit.Options.Count > 2)
                                                {
                                                    #region // Set Default PCE/PCS as UOM //
                                                    string _units = convert.ToString(selUnit.InnerHTML).Trim().ToUpper().Replace("SELECTED=\"SELECTED\"", ""); ;
                                                    System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(_units.Trim(), "<OPTION[\\s]+VALUE=\"[0-9]+\">PCS</OPTION>");
                                                    if (match != null && match.Success)
                                                    {
                                                        string _Value = convert.ToString(match.Value);
                                                        match = System.Text.RegularExpressions.Regex.Match(_Value, "[0-9]+");
                                                        if (match != null && match.Success)
                                                        {
                                                            string _uomValue = convert.ToString(match.Value);
                                                            _netBrowser.SelectElementItem(selUnit, true, _uomValue);
                                                            uomFound = true;
                                                        }
                                                    }
                                                    #endregion
                                                }

                                                if (uomFound == false) throw new Exception("Unable to set UOM for item no. " + iObj.Number);
                                            }

                                            // Calculate Avarage Price //
                                            double unitPrice = 0;
                                            string orgQtyStr = txtQty.GetAttribute("Value");
                                            if (orgQtyStr.Trim().Length == 0) throw new Exception("Original Qty is 0.");
                                            else if (orgQtyStr.Trim().Length > 0 && convert.ToFloat(orgQtyStr) != convert.ToFloat(iObj.Quantity))
                                            {
                                                unitPrice = (convert.ToFloat(iObj.Quantity) * convert.ToFloat(_price)) / convert.ToDouble(orgQtyStr);
                                            }
                                            else unitPrice = _price;

                                            //txtPrice.Focus(); Thread.Sleep(100);
                                            txtPrice.SetAttribute("Value", Math.Round(unitPrice, 3).ToString());
                                            Thread.Sleep(50);

                                            //txtDiscount.Focus(); Thread.Sleep(100);
                                            txtDiscount.SetAttribute("Value", _disc.ToString());
                                            Thread.Sleep(50);

                                            if (convert.ToFloat(orgQtyStr) != convert.ToFloat(iObj.Quantity))
                                            {
                                                iAddRemarks = "Quoted Qty : " + convert.ToFloat(iObj.Quantity) + "; ";
                                            }
                                            iAddRemarks += "Quoted UOM : " + convert.ToString(iObj.MeasureUnitQualifier).Trim().ToUpper();
                                        }

                                        // Set LeadDays as per item if exists
                                        if (convert.ToInt(iObj.DeleiveryTime) > 0)
                                        {
                                            string leaddays = convert.ToInt(iObj.DeleiveryTime).ToString();
                                            //txtLeadDays.Focus(); Thread.Sleep(100);
                                            txtLeadDays.SetAttribute("Value", leaddays.ToString());
                                        }

                                        #region // Save Item details before updating item remarks //
                                        DOMElement _imgSaveItem = GetElement("gvVendorItem_" + ctlID + "_cmdUpdate");
                                        if (_imgSaveItem != null)
                                        {
                                            _imgSaveItem.Focus(); _imgSaveItem.Click(); Thread.Sleep(1000);
                                            int maxTry = 5;

                                            while (_netBrowser.GetElementbyId("gvVendorItem_" + ctlID + "_lblunit") == null)
                                            {
                                                if (maxTry == 0) break; Thread.Sleep(1000);
                                                _netBrowser.CurrentDocument = _netBrowser.browser.GetDocument();
                                                maxTry--;
                                            }
                                        }
                                        else throw new Exception("item save button not found");
                                        #endregion
                                    }
                                    else
                                    {
                                        LeSDM.AddConsoleLog("Unable to update item " + iObj.Number + "; Item Price is zero.");
                                    }

                                    #region // Add Item Remarks to list //
                                    string iRemarks = iAddRemarks + Environment.NewLine + convert.ToString(iObj.LineItemComment.Value).Trim();
                                    iRemarks = iRemarks.Trim();
                                    if (iRemarks.Trim().Length > 0)
                                    {
                                        // Get Remarks Field //                                        
                                        _imgRemarks = GetElement("gvVendorItem_" + ctlID + "_imgVendorNotes");
                                        if (_imgRemarks != null)
                                        {
                                            string _remarkURL = "1";
                                            string onClickEvent = _imgRemarks.GetAttribute("onclick");
                                            int _startIndx = onClickEvent.IndexOf("PurchaseFormItemMoreInfo.aspx?");
                                            if (_startIndx > 0)
                                            {
                                                int _endIndx = onClickEvent.Substring(_startIndx).IndexOf("');");
                                                if (_endIndx > 0) _remarkURL = onClickEvent.Substring(_startIndx, _endIndx);
                                            }

                                            if (_remarkURL.Trim().Length > 0)
                                            {
                                                Uri _url = (new Uri(orgURL));
                                                string host = _url.Host;
                                                // Updated on 12-DEC-2019 (Segments picked from URL)
                                                string _itemRemURL = _url.Scheme + "://" + host;
                                                for (int seg = 0; seg < _url.Segments.Length - 1; seg++)
                                                {
                                                    _itemRemURL += _url.Segments[seg];
                                                }
                                                _itemRemURL = _itemRemURL + _remarkURL.Trim();
                                                // "/Phoenix/Purchase/" + _remarkURL.Trim();
                                                lstRemarks.Add(ItemNo + "|" + _itemRemURL, iRemarks.Trim());
                                            }
                                            else
                                            {
                                                throw new Exception("Item Remarks URL not found ");
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    LeSDM.AddConsoleLog("Unable to update item " + iObj.Number + "; Item Quantity is zero.");
                                }

                                uploadedItems.Add(convert.ToInt(ItemNo));
                            }
                            else throw new Exception("Item no. " + ItemNo + " not found in MTML Quote file.");
                        }
                        else
                        {
                            LeSDM.AddConsoleLog("Unable to get content at index " + i);
                        }
                    }

                    #region // Move to next Grid Page //
                    bool nextPageExist = false;
                    DOMElement lnkNext = GetElement("cmdNext");
                    if (lnkNext != null && lnkNext.GetAttribute("disabled").Trim().ToLower() != "disabled"
                        && lnkNext.GetAttribute("disabled").Trim().ToLower() != "true")
                    {
                        nextPageExist = true;
                        pageCounter++;
                        LeSDM.AddConsoleLog("Move to Next page " + pageCounter + " ..");
                        lnkNext.Click(); Thread.Sleep(5000);

                        int pageWaitCounter = 0;
                        DOMElement lblPageNumber = GetElement("lblPagenumber");
                        while ((lblPageNumber != null && !convert.ToString(lblPageNumber.TextContent).Contains("Page " + pageCounter)))
                        {
                            Thread.Sleep(500); pageWaitCounter++;
                            _netBrowser.CurrentDocument = _netBrowser.browser.GetDocument();
                            lblPageNumber = GetElement("lblPagenumber");
                            if (pageWaitCounter > 10) break;
                        }

                        lblPageNumber = GetElement("lblPagenumber");
                        if (lblPageNumber != null
                            && convert.ToString(lblPageNumber.TextContent).Contains("Page " + pageCounter))
                        {
                            itemPageLoaded = true;
                        }
                        else
                        {
                            string footerContent = "";
                            DOMElement tblgridpager = _netBrowser.GetElementbyClass("datagrid_pagestyle");
                            if (tblgridpager != null)
                            {
                                footerContent = convert.ToString(tblgridpager.TextContent).Trim();
                            }

                            if (footerContent.Contains("Page " + pageCounter)) itemPageLoaded = true;
                        }
                    }

                    if (nextPageExist == false)
                    {
                        itemPageLoaded = false;
                        break; // End Of Items
                    }
                    if (itemPageLoaded)
                    {
                        LeSDM.AddConsoleLog("Next Page " + pageCounter + " Loaded");
                    }
                    #endregion
                }
                while (itemPageLoaded);

                _netBrowser.LoadUrl(orgURL);
                Thread.Sleep(2000);

                #region // Update all items remarks //
                foreach (KeyValuePair<string, string> pair in lstRemarks)
                {
                    LeSDM.AddConsoleLog("Updating item remarks for item - " + pair.Key.Split('|')[0]);

                    _netBrowser.LoadUrl(pair.Key.Split('|')[1]);
                    Thread.Sleep(5000);
                    _netBrowser.CurrentDocument = _netBrowser.browser.GetDocument();

                    try
                    {
                        DOMElement _imgHTMLWriter = GetElement("txtItemDetails_ucCustomEditor_ctl03_ctl01");
                        if (_imgHTMLWriter != null)
                        {
                            _imgHTMLWriter.Click(); Thread.Sleep(500);

                            DOMElement txtRemarks = GetElement("txtItemDetails_ucCustomEditor_ctl02_ctl01");
                            if (txtRemarks != null)
                            {
                                string iRemarks = pair.Value.Trim().Replace(Environment.NewLine, "<br>");
                                txtRemarks.Focus(); Thread.Sleep(500);
                                _netBrowser.ClearText();
                                _netBrowser.InputKeys(iRemarks.Trim(), 45);
                            }
                            else throw new Exception("Unable to edit item remarks");

                            DOMElement _imgHTMLDesigner = GetElement("txtItemDetails_ucCustomEditor_ctl03_ctl02");
                            if (_imgHTMLDesigner != null)
                            {
                                _imgHTMLDesigner.Click();
                                Thread.Sleep(500);
                            }

                            #region // Save Item Remarks //
                            DOMElement lnkSaveRemarks = GetElement("MenuLineItemDetail_dlstTabs_ctl00_btnMenu");
                            if (lnkSaveRemarks != null)
                            {
                                lnkSaveRemarks.Click();
                                Thread.Sleep(3000);
                            }
                            else
                            {
                                DOMElement _tblMenu = GetElement("MenuLineItemDetail_dlstTabs");
                                if (_tblMenu != null)
                                {
                                    if (_tblMenu.GetElementsByTagName("a").Count > 0)
                                    {
                                        lnkSaveRemarks = (DOMElement)_tblMenu.GetElementsByTagName("a")[0];
                                        if (convert.ToString(lnkSaveRemarks.TextContent).ToString().ToUpper() == "SAVE")
                                        {
                                            lnkSaveRemarks.Click();
                                            Thread.Sleep(3000);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        else throw new Exception("Item remark dialog not loaded");
                    }
                    catch (Exception ex)
                    {
                        LeSDM.AddConsoleLog("Error while updating item remarks; " + ex.StackTrace);
                        throw ex;
                    }
                }
                #endregion

                return _extraItems;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SendQuote(MTMLInterchange _interchange, string QuoteRefNo, double FinalAmt, double extraItemCost, FileInfo xmlFile, string BuyerCode, string VendorCode)
        {
            try
            {
                string orgURL = _interchange.DocumentHeader.MessageReferenceNumber;

                LeSDM.AddConsoleLog("Saving Quote '" + this.VRNO + "' ");

                DOMElement lnkSaveQuote = GetElement("MenuQuotationLineItem_dlstTabs_ctl00_btnMenu");
                if (lnkSaveQuote != null)
                {
                    lnkSaveQuote.Click();
                    Thread.Sleep(3000);

                    if (extraItemCost > 0) FinalAmt = FinalAmt - extraItemCost;

                    double totalPrice = 0;
                    double webPrice = 0;
                    DOMInputElement txtTotalPrice = (DOMInputElement)GetElement("txtTotalPrice");
                    if (txtTotalPrice != null) totalPrice = Math.Round(convert.ToDouble(txtTotalPrice.GetAttribute("Value")), 2);
                    if (totalPrice > 0)
                    {
                        bool sendQuote = false;
                        if (totalPrice == Math.Round(FinalAmt, 2))
                        {
                            sendQuote = true;
                        }
                        else
                        {
                            webPrice = totalPrice;
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
                            _netBrowser.LoadUrl(orgURL); Thread.Sleep(2000);
                            LeSDM.AddConsoleLog("Sending Quote '" + this.VRNO + "' ");

                            #region // Send TO EMS //
                            DOMElement lnkSendQuote = GetElement("MenuQuotationLineItem_dlstTabs_ctl01_btnMenu");
                            if (lnkSendQuote != null)
                            {
                                lnkSendQuote.Click();
                                Thread.Sleep(12000); // Comment This line Before Testing //
                                _netBrowser.CurrentDocument = _netBrowser.browser.GetDocument();

                                #region // Check Quote uploaded successfully. //
                                string sendMsg = "";
                                DOMElement spnMsg = GetElement("lbltext");
                                int waitCounterSendQuote = 0;
                                while (spnMsg == null)
                                {
                                    Thread.Sleep(5000);
                                    waitCounterSendQuote++;
                                    if (waitCounterSendQuote > 10) break;

                                    _netBrowser.CurrentDocument = _netBrowser.browser.GetDocument();
                                    spnMsg = GetElement("lbltext");
                                }
                                if (spnMsg != null)
                                {
                                    sendMsg = convert.ToString(spnMsg.TextContent);
                                    while (sendMsg.Contains("  ")) sendMsg = sendMsg.Replace("  ", " ");
                                }
                                #endregion

                                if (sendMsg.Contains("Your bid " + QuoteRefNo.Trim() + " is registered and confirmed in our database"))
                                {
                                    #region // Success - Quote Submitted Successfully //
                                    // Screen Shot of Submitted By LeS //
                                    string quoteSubImg = this.ImagePath + "\\Phoenix_QuoteSubmitted_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                                    _netBrowser.PrintPDF(quoteSubImg, false);
                                    if (File.Exists(quoteSubImg)) quoteSubImg = Path.GetFileName(quoteSubImg);
                                    else quoteSubImg = xmlFile.Name;

                                    LeSDM.AddConsoleLog("Sending audit log");

                                    #region // Set Audit Log //
                                    LeSDM.AddConsoleLog("Quote for '" + VRNO + "' submitted successfully.");
                                    MoveFile(xmlFile.Directory.FullName + "\\Backup", xmlFile);
                                    LeSDM.SetAuditLog(this.Module, quoteSubImg, VRNO, "Uploaded", "Quote for '" + VRNO + "' submitted successfully.", BuyerCode.Split('_')[0], VendorCode, this.Server, this.Processor);
                                    this.SendTextNotification(VRNO.Trim(), QuoteRef, "SUCCESS", SuppCode);
                                    #endregion

                                    // Send Mail Notification
                                    if (convert.ToString(ConfigurationManager.AppSettings["SEND_MAIL_FOR_SUBMIT"]).Trim().ToUpper() == "TRUE")
                                    {
                                        LeSDM.AddConsoleLog("Generating mail notification...");
                                        SendMailNotification(_interchange, "QUOTE", VRNO.Trim(), "SUBMITTED", "Quote for REF '" + VRNO.Trim() + "' submitted successfully.");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    // Reload Page //
                                    _netBrowser.LoadUrl(orgURL); Thread.Sleep(2000);

                                    lnkSendQuote = GetElement("MenuQuotationLineItem_dlstTabs_ctl01_btnMenu");
                                    if (lnkSendQuote == null)
                                    {
                                        #region // Send log for Quote Submitted //
                                        string quoteSubImg = this.ImagePath + "\\Phoenix_QuoteSubmitted_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                                        _netBrowser.PrintPDF(quoteSubImg, false);

                                        LeSDM.AddConsoleLog("Sending audit log");

                                        #region // Set Audit Log //
                                        LeSDM.AddConsoleLog("Quote for '" + VRNO + "' submitted successfully.");
                                        MoveFile(xmlFile.Directory.FullName + "\\Backup", xmlFile);
                                        LeSDM.SetAuditLog(this.Module, quoteSubImg, VRNO, "Uploaded", "Quote for '" + VRNO + "' submitted successfully.", BuyerCode.Split('_')[0], VendorCode, this.Server, this.Processor);
                                        this.SendTextNotification(VRNO.Trim(), QuoteRef, "SUCCESS", SuppCode);
                                        #endregion

                                        // Send Mail Notification
                                        if (convert.ToString(ConfigurationManager.AppSettings["SEND_MAIL_FOR_SUBMIT"]).Trim().ToUpper() == "TRUE")
                                        {
                                            LeSDM.AddConsoleLog("Generating mail notification...");
                                            SendMailNotification(_interchange, "QUOTE", VRNO.Trim(), "SUBMITTED", "Quote for REF '" + VRNO.Trim() + "' submitted successfully.");
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region // Error While Sending Quote //
                                        lnkSendQuote.Click(); // Comment This line Before Testing //
                                        Thread.Sleep(5000);

                                        _netBrowser.CurrentDocument = _netBrowser.browser.GetDocument();

                                        // Get Error Screen Shot //
                                        string ErrorMsg = "";
                                        DOMElement _divError = this.GetElement("ucError_pnlErrorMessage");
                                        if (_divError != null)
                                        {
                                            DOMElement spnError = this.GetElement("ucError_spnErrorMessage");
                                            if (spnError != null)
                                            {
                                                ErrorMsg = convert.ToString(spnError.TextContent);
                                            }
                                        }

                                        // Get Error Screen Shot //
                                        string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                                        _netBrowser.PrintPDF(errImg, false);
                                        if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                                        else errImg = "";

                                        LeSDM.AddConsoleLog("Unable to send quote for '" + VRNO + "'. " + ErrorMsg.Trim());
                                        MoveFile(xmlFile.Directory.FullName + "\\Error_Files", xmlFile);
                                        LeSDM.SetAuditLog(this.Module, errImg, VRNO.Trim(), "Error", "Unable to send quote for '" + VRNO + "'. " + ErrorMsg.Trim(), BuyerCode, SuppCode, Server, Processor);
                                        this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                                        #endregion
                                    }
                                }
                            }
                            else
                            {
                                #region // Error - Send Button not found on webpage //
                                string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                                _netBrowser.PrintPDF(errImg, false);
                                if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                                else errImg = "";

                                LeSDM.AddConsoleLog(" Send button not found on webpage for '" + VRNO + "'.");
                                MoveFile(xmlFile.Directory.FullName + "\\Error_Files", xmlFile);
                                LeSDM.SetAuditLog(this.Module, errImg, VRNO.Trim(), "Error", "Unable to send quote for Ref '" + this.VRNO + "'; Send button not found on webpage.", BuyerCode, SuppCode, Server, Processor);
                                this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                                #endregion
                            }
                            #endregion
                        }
                        else
                        {
                            #region // Error - Quote Total mismatched //
                            string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                            _netBrowser.PrintPDF(errImg, false);
                            if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                            else errImg = "";

                            LeSDM.AddConsoleLog("Web total : " + webPrice);
                            LeSDM.AddConsoleLog("MTML total :" + FinalAmt);
                            LeSDM.AddConsoleLog("Quote total is mismatched for '" + VRNO + "'.");

                            // Set Error Log //
                            TrackErrors.TrackError.SetErrorCount(SuppCode, VRNO.Trim(), "QUOTE_MISMATCHED");
                            int errCount = TrackErrors.TrackError.GetErrorCount(SuppCode, VRNO.Trim(), "QUOTE_MISMATCHED");
                            if (errCount == 3)
                            {
                                LeSDM.SetAuditLog(this.Module, errImg, VRNO.Trim(), "Error", "Unable to send quote for Ref '" + this.VRNO + "'; Quote total is mismatched.", BuyerCode, SuppCode, Server, Processor);
                                TrackErrors.TrackError.DeleteErrorCountFile(SuppCode, VRNO.Trim(), "QUOTE_MISMATCHED");
                                MoveFile(xmlFile.Directory.FullName + "\\Error_Files", xmlFile);
                                this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        #region  // Error - Quote total is zero //
                        // Save ScreenShot //
                        string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                        _netBrowser.PrintPDF(errImg, false);
                        if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                        else errImg = "";

                        LeSDM.AddConsoleLog("Quote total is 0 for '" + VRNO + "'.");

                        // Set Error Log //
                        TrackErrors.TrackError.SetErrorCount(SuppCode, VRNO.Trim(), "QUOTE_AMT_ZERO");
                        int errCount = TrackErrors.TrackError.GetErrorCount(SuppCode, VRNO.Trim(), "QUOTE_AMT_ZERO");
                        if (errCount == 3)
                        {
                            LeSDM.SetAuditLog(this.Module, errImg, VRNO.Trim(), "Error", "Unable to save quote for Ref '" + this.VRNO + "'; Quote total is 0.", BuyerCode, SuppCode, Server, Processor);
                            MoveFile(xmlFile.Directory.FullName + "\\Error_Files", xmlFile);
                            TrackErrors.TrackError.DeleteErrorCountFile(SuppCode, VRNO.Trim(), "QUOTE_AMT_ZERO");
                            this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                        }
                        #endregion
                    }
                }
                else
                {
                    // Save Button not found on webpage //
                    string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                    _netBrowser.PrintPDF(errImg, false);
                    if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                    else errImg = "";

                    LeSDM.AddConsoleLog("Save button not found on webpage for '" + VRNO + "'.");

                    // Set Error Log //
                    TrackErrors.TrackError.SetErrorCount(SuppCode, VRNO.Trim(), "SAVE_BTN_MISSING");
                    int errCount = TrackErrors.TrackError.GetErrorCount(SuppCode, VRNO.Trim(), "SAVE_BTN_MISSING");
                    if (errCount == 3)
                    {
                        LeSDM.SetAuditLog(this.Module, errImg, VRNO.Trim(), "Error", "Unable to save quote for Ref '" + this.VRNO + "'; Save button not found on webpage.", BuyerCode, SuppCode, Server, Processor);
                        TrackErrors.TrackError.DeleteErrorCountFile(SuppCode, VRNO.Trim(), "SAVE_BTN_MISSING");
                        MoveFile(xmlFile.Directory.FullName + "\\Error_Files", xmlFile);
                        this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                    }
                }
            }
            catch (Exception ex)
            {
                // Save Screen Shot //
                string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                _netBrowser.PrintPDF(errImg, false);
                if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                else errImg = "";

                LeSDM.AddConsoleLog("Unable to send quote '" + VRNO + "'." + ex.Message);

                // Set Error Log //
                TrackErrors.TrackError.SetErrorCount(SuppCode, VRNO.Trim(), "INTERNAL_ERROR");
                int errCount = TrackErrors.TrackError.GetErrorCount(SuppCode, VRNO.Trim(), "INTERNAL_ERROR");
                if (errCount == 3)
                {
                    LeSDM.SetAuditLog(this.Module, errImg, VRNO.Trim(), "Error", "Unable to send quote for Ref '" + this.VRNO + "'.", BuyerCode, SuppCode, Server, Processor);
                    TrackErrors.TrackError.DeleteErrorCountFile(SuppCode, VRNO.Trim(), "INTERNAL_ERROR");
                    MoveFile(xmlFile.Directory.FullName + "\\Error_Files", xmlFile); // Move File to Error Files Folder
                    this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                }
            }
        }

        private void SaveQuote(MTMLInterchange _interchange, string QuoteRefNo, double FinalAmt, double extraItemCost, FileInfo xmlFile, string BuyerCode, string VendorCode)
        {
            try
            {
                LeSDM.AddConsoleLog("Saving Quote '" + this.VRNO + "' ");
                DOMElement lnkSaveQuote = this.GetElement("MenuQuotationLineItem_dlstTabs_ctl00_btnMenu");
                if (lnkSaveQuote != null)
                {
                    lnkSaveQuote.Click(); Thread.Sleep(3000);

                    if (extraItemCost > 0) FinalAmt = FinalAmt - extraItemCost;

                    double totalPrice = 0;
                    DOMInputElement txtTotalPrice = (DOMInputElement)this.GetElement("txtTotalPrice");
                    if (txtTotalPrice != null) totalPrice = Math.Round(convert.ToDouble(txtTotalPrice.GetAttribute("Value")), 2);

                    if (totalPrice > 0)
                    {
                        bool totalMatched = false;
                        if (totalPrice == Math.Round(FinalAmt, 0))
                        {
                            totalMatched = true;
                        }
                        else
                        {
                            double webPrice = totalPrice;
                            double quotePrice = Math.Round(FinalAmt, 2);

                            if (webPrice >= quotePrice && (webPrice - quotePrice) <= 1)
                            {
                                totalMatched = true;
                            }
                            else if (quotePrice > webPrice && (quotePrice - webPrice) <= 1)
                            {
                                totalMatched = true;
                            }
                        }

                        if (totalMatched)
                        {
                            #region // Set Audit Log //
                            _netBrowser.LoadUrl(_interchange.DocumentHeader.MessageNumber); Thread.Sleep(2000);
                            string quoteImg = this.ImagePath + "\\Phoenix_DraftQuote_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                            _netBrowser.PrintPDF(quoteImg, false);
                            if (File.Exists(quoteImg)) quoteImg = Path.GetFileName(quoteImg);
                            else quoteImg = "";

                            LeSDM.AddConsoleLog("Quote for '" + VRNO + "' saved as draft successfully.");
                            MoveFile(xmlFile.Directory.FullName + "\\Backup", xmlFile); // Move File to Backup
                            LeSDM.SetAuditLog(this.Module, quoteImg, VRNO.Trim(), "Uploaded", "Quote for '" + VRNO + "' saved as draft successfully.", BuyerCode, SuppCode, Server, Processor);
                            this.SendTextNotification(VRNO.Trim(), QuoteRef, "SUCCESS", SuppCode);
                            #endregion

                            // Send Mail Notification
                            if (convert.ToString(ConfigurationManager.AppSettings["SEND_MAIL_FOR_SUBMIT"]).Trim().ToUpper() == "TRUE")
                            {
                                LeSDM.AddConsoleLog("Generating mail notification...");
                                SendMailNotification(_interchange, "QUOTE", VRNO.Trim(), "SUBMITTED", "Quote for REF '" + VRNO.Trim() + "' saved as draft successfully.");
                            }
                        }
                        else
                        {
                            #region // Quote total is mismatched //
                            string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                            _netBrowser.PrintPDF(errImg, false);
                            if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                            else errImg = "";

                            LeSDM.AddConsoleLog("Quote total is mismatched for '" + VRNO + "'.");

                            // Set Error Log //
                            TrackErrors.TrackError.SetErrorCount(SuppCode, VRNO.Trim(), "TOTAL_MISMATCHED");
                            int errCount = TrackErrors.TrackError.GetErrorCount(SuppCode, VRNO.Trim(), "TOTAL_MISMATCHED");
                            if (errCount == 3)
                            {
                                LeSDM.SetAuditLog(this.Module, errorImg, VRNO.Trim(), "Error", "Unable to save quote for Ref '" + this.VRNO + "'; Quote total is mismatched.", BuyerCode, SuppCode, Server, Processor);
                                TrackErrors.TrackError.DeleteErrorCountFile(SuppCode, VRNO.Trim(), "TOTAL_MISMATCHED");
                                MoveFile(xmlFile.Directory.FullName + "\\Error_Files", xmlFile);
                                this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        #region Quote total is 0
                        string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                        _netBrowser.PrintPDF(errImg, false);
                        if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                        else errImg = "";

                        LeSDM.AddConsoleLog("Quote total is 0 for '" + VRNO + "'.");

                        // Set Error Log //
                        TrackErrors.TrackError.SetErrorCount(SuppCode, VRNO.Trim(), "QUOTE_AMT_ZERO");
                        int errCount = TrackErrors.TrackError.GetErrorCount(SuppCode, VRNO.Trim(), "QUOTE_AMT_ZERO");
                        if (errCount == 3)
                        {
                            LeSDM.SetAuditLog(this.Module, errorImg, VRNO.Trim(), "Error", "Unable to save quote for Ref '" + this.VRNO + "'; Quote total is 0.", BuyerCode, SuppCode, Server, Processor);
                            TrackErrors.TrackError.DeleteErrorCountFile(SuppCode, VRNO.Trim(), "QUOTE_AMT_ZERO");
                            MoveFile(xmlFile.Directory.FullName + "\\Error_Files", xmlFile); // Move File to Error Files Folder
                            this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                        }
                        #endregion
                    }
                }
                else
                {
                    #region // Save Button not found on webpage //
                    string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                    _netBrowser.PrintPDF(errImg, false);
                    if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                    else errImg = "";

                    LeSDM.AddConsoleLog("Save button not found on webpage for '" + VRNO + "'.");

                    // Set Error Log //
                    TrackErrors.TrackError.SetErrorCount(SuppCode, VRNO.Trim(), "SAVE_BTN_MISSING");
                    int errCount = TrackErrors.TrackError.GetErrorCount(SuppCode, VRNO.Trim(), "SAVE_BTN_MISSING");
                    if (errCount == 3)
                    {
                        LeSDM.SetAuditLog(this.Module, errorImg, VRNO.Trim(), "Error", "Unable to save quote for Ref '" + this.VRNO + "'; Save button not found on webpage.", BuyerCode, SuppCode, Server, Processor);
                        TrackErrors.TrackError.DeleteErrorCountFile(SuppCode, VRNO.Trim(), "SAVE_BTN_MISSING");
                        MoveFile(xmlFile.Directory.FullName + "\\Error_Files", xmlFile); // Move File to Error Files Folder
                        this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                _netBrowser.PrintPDF(errImg, false);
                if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                else errImg = "";

                LeSDM.AddConsoleLog("Unable to save quote '" + VRNO + "'." + ex.Message);

                // Set Error Log //
                TrackErrors.TrackError.SetErrorCount(SuppCode, VRNO.Trim(), "INTERNAL_ERROR");
                int errCount = TrackErrors.TrackError.GetErrorCount(SuppCode, VRNO.Trim(), "INTERNAL_ERROR");
                if (errCount == 3)
                {
                    LeSDM.SetAuditLog(this.Module, errImg, VRNO.Trim(), "Error", "Unable to save quote for Ref '" + this.VRNO + "'.", BuyerCode, SuppCode, Server, Processor);
                    TrackErrors.TrackError.DeleteErrorCountFile(SuppCode, VRNO.Trim(), "INTERNAL_ERROR");
                    MoveFile(xmlFile.Directory.FullName + "\\Error_Files", xmlFile); // Move File to Error Files Folder
                    this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                }
            }
        }

        private void DeclineQuote(MTMLInterchange _interchange, string QuoteRefNo, FileInfo xmlFile, string BuyerCode, string VendorCode, string headerRemarks)
        {
            try
            {
                LeSDM.AddConsoleLog("Decline quote '" + this.VRNO + "'");

                // Decline Quote //
                DOMElement lnkDeclineQuote = this.GetElement("MenuQuotationLineItem_dlstTabs_ctl06_btnMenu");
                if (lnkDeclineQuote != null)
                {
                    LeSDM.AddConsoleLog("Declining quote ");
                    lnkDeclineQuote.Click();
                    Thread.Sleep(5000);

                    // Fill Remarks //
                    headerRemarks = headerRemarks.Trim().Replace("<br>", Environment.NewLine); // Updated on 05-MAR-2020 //
                    DOMTextAreaElement txtDeclineRem = (DOMTextAreaElement)this.GetElement("txtDeclineQuote");
                    txtDeclineRem.Focus(); Thread.Sleep(500);
                    _netBrowser.ClearText();
                    _netBrowser.InputKeys(headerRemarks.Trim(), 70);
                    Thread.Sleep(5000);

                    // Update Decline Quote Remarks //                    
                    DOMElement _lnkSaveDeclineRem = this.GetElement("MenuDeclineQuote_dlstTabs_ctl00_btnMenu");
                    if (_lnkSaveDeclineRem != null)
                    {
                        _lnkSaveDeclineRem.Click();
                        Thread.Sleep(7000);
                    }

                    // Reload URL
                    _netBrowser.LoadUrl(convert.ToString(_interchange.DocumentHeader.MessageReferenceNumber));
                    Thread.Sleep(2000);

                    #region // Set Audit Log //
                    LeSDM.AddConsoleLog("Sending audit log");
                    string errImg = this.ImagePath + "\\Phoenix_DeclinedQuote_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                    _netBrowser.PrintPDF(errImg, false);
                    if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                    else errImg = "";

                    LeSDM.AddConsoleLog("Quote for '" + VRNO + "' declined successfully.");
                    MoveFile(xmlFile.Directory.FullName + "\\Backup", xmlFile); // Move File to Backup
                    LeSDM.SetAuditLog(this.Module, errImg, VRNO.Trim(), "Declined", "Quote for '" + VRNO + "' declined successfully.", BuyerCode, SuppCode, Server, Processor);
                    this.SendTextNotification(VRNO.Trim(), QuoteRef, "SUCCESS", SuppCode);
                    #endregion

                    // Send Mail Notification
                    if (convert.ToString(ConfigurationManager.AppSettings["SEND_MAIL_FOR_DECLINE"]).Trim().ToUpper() == "TRUE")
                    {
                        LeSDM.AddConsoleLog("Generating mail notification...");
                        SendMailNotification(_interchange, "QUOTE", VRNO.Trim(), "DECLINED", "Quote for REF '" + VRNO.Trim() + "' declined successfully.");
                    }
                }
                else
                {
                    string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                    _netBrowser.PrintPDF(errImg, false);
                    if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                    else errImg = "";

                    LeSDM.AddConsoleLog("Decline button not found on webpage for '" + VRNO + "'.");
                    MoveFile(xmlFile.Directory.FullName + "\\Error_Files", xmlFile); // Move File to Error_Files

                    LeSDM.SetAuditLog(this.Module, errImg, VRNO.Trim(), "Error", "Unable to decline quote for Ref '" + this.VRNO + "'; Decline button not found on webpage.", BuyerCode, SuppCode, Server, Processor);
                    this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                }
            }
            catch (Exception ex)
            {
                string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                _netBrowser.PrintPDF(errImg, false);
                if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                else errImg = "";

                LeSDM.AddConsoleLog("Unable to decline quote '" + VRNO + "'." + ex.Message);
                MoveFile(xmlFile.Directory.FullName + "\\Error_Files", xmlFile); // Move File to Error_Files

                LeSDM.SetAuditLog(this.Module, errImg, VRNO.Trim(), "Error", "Unable to decline quote for Ref '" + this.VRNO + "'.", BuyerCode, SuppCode, Server, Processor);
                this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
            }
        }

        private void SendMailNotification(MTMLInterchange _interchange, string DocType, string VRNO, string ActionType, string Message)
        {
            try
            {
                string MailFromDefault = convert.ToString(ConfigurationSettings.AppSettings["MAIL_FROM"]);
                string MailBccDefault = convert.ToString(ConfigurationSettings.AppSettings["MAIL_BCC"]);
                string MailCcDefault = convert.ToString(ConfigurationSettings.AppSettings["MAIL_CC"]);

                string BuyerCode = convert.ToString(_interchange.Recipient).Trim();
                string SuppCode = convert.ToString(_interchange.Sender).Trim();
                string BuyerID = convert.ToString(_interchange.BuyerSuppInfo.BuyerID).Trim();
                string SupplierID = convert.ToString(_interchange.BuyerSuppInfo.SupplierID).Trim();

                string MailAuditPath = convert.ToString(ConfigurationSettings.AppSettings["MAIL_AUDIT_PATH"]);
                if (MailAuditPath.Trim() != "")
                {
                    if (!Directory.Exists(MailAuditPath.Trim())) Directory.CreateDirectory(MailAuditPath.Trim());
                }
                else throw new Exception("MAIL_AUDIT_PATH value is not defined in config file.");

                string MailSettings = convert.ToString(ConfigurationSettings.AppSettings[SuppCode + "-" + BuyerCode]);
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
                            LeSDM.AddConsoleLog("Mail Send to Buyer.");
                            LeSDM.AddConsoleLog("Mail Send to Buyer Email -" + MailTo.Trim() + " .");
                        }
                        else
                        {
                            LeSDM.AddConsoleLog("Unable to send mail notification to buyer; Buyer Mailid is empty.");
                            LeSDM.AddConsoleLog("Unable to send mail notification to buyer; Buyer Mailid is empty.");
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
                            LeSDM.AddConsoleLog("Mail Send to Supplier.");
                            LeSDM.AddConsoleLog("Mail Send to Supplier Email -" + MailTo.Trim() + " .");
                        }
                        else
                        {
                            LeSDM.AddConsoleLog("Unable to send mail notification to supplier; Supplier Mailid is empty.");
                        }
                    }
                    #endregion
                }
                else
                {
                    LeSDM.AddConsoleLog("Unable to send mail notification; No mail setting found for Supplier-Buyer (" + SuppCode + "-" + BuyerCode + ") link combination.");
                }
            }
            catch (Exception ex)
            {
                LeSDM.AddConsoleLog("Unable to create mail notification file. Error - " + ex.Message);
            }
        }

        private void SaveQuote()
        {
            DOMElement lnkSaveQuote = this.GetElement("MenuQuotationLineItem_dlstTabs_ctl00_btnMenu");
            if (lnkSaveQuote != null)
            {
                LeSDM.AddConsoleLog("Save Quote Header Details ");
                lnkSaveQuote.Click();
                Thread.Sleep(2000);

                string ErrorMsg = "";
                DOMElement _divError = this.GetElement("ucError_pnlErrorMessage");
                if (_divError != null)
                {
                    DOMElement spnError = _divError.GetElementByTagName("Span");
                    if (spnError != null)
                    {
                        ErrorMsg = convert.ToString(spnError.TextContent);
                    }
                }

                if (ErrorMsg.Trim().Length > 0)
                {
                    #region //*** Get Error Screen Shot ***//
                    string pdfFile = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                    _netBrowser.PrintPDF(pdfFile, false);
                    if (File.Exists(pdfFile)) pdfFile = Path.GetFileName(pdfFile);
                    else pdfFile = "";
                    #endregion

                    throw new Exception(ErrorMsg.Trim());
                }
            }
        }

        private DOMElement GetElement(string id)
        {
            DOMElement ele = _netBrowser.GetElementbyId(id);
            if (ele == null) ele = _netBrowser.GetElementFromFrames(id, false, false, false);
            return ele;
        }

        private string CheckError()
        {
            string errorMsg = "", errorImg = "";

            #region // Check Error //
            string ErrorMsg = "";
            DOMElement _divError = this.GetElement("ucError_pnlErrorMessage");
            if (_divError != null)
            {
                DOMElement spnError = this.GetElement("ucError_spnErrorMessage");
                if (spnError != null)
                {
                    ErrorMsg = convert.ToString(spnError.TextContent);
                }
            }

            if (ErrorMsg.Trim().Length > 0)
            {
                // Get Error Screen Shot //
                string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                _netBrowser.PrintPDF(errImg, false);
                if (File.Exists(errImg)) errImg = Path.GetFileName(errImg);
                else errImg = "";

                throw new Exception(ErrorMsg);
            }
            #endregion

            if (errorMsg.Trim().Length > 0)
                return errorImg + "|" + errorMsg;
            else return "";
        }
    
        // Common Functions //
        private void SendTextNotification(string VRNO, string QuoteRefNo, string Status, string SuppCode)
        {
            try
            {
                // Check Send Text Notification flag //
                notifyByText = false;
                string[] notificationValues = convert.ToString(ConfigurationManager.AppSettings["SEND_TEXT_NOTIFICATION"]).Trim().Split('|');
                foreach (string strValue in notificationValues)
                {
                    string[] values = strValue.Split('=');
                    if (values[0].Trim().ToUpper() == SuppCode.Trim().ToUpper() && values.Length > 1 && convert.ToInt(values[1]) > 0)
                    {
                        notifyByText = true; break;
                    }
                }

                if (notifyByText)
                {
                    string folder = convert.ToString(ConfigurationManager.AppSettings["TEXT_NOTIFICATION_PATH"]);
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder.Trim());

                    string textNotifyMsg = VRNO.Trim() + "|" + QuoteRefNo.Trim() + "=" + Status.Trim().ToUpper();
                    string txtNotifyFile = folder + "\\" + SuppCode.Trim() + "_" + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".txt";

                    File.WriteAllText(txtNotifyFile, textNotifyMsg);

                    if (File.Exists(txtNotifyFile))
                    {
                        LeSDM.AddConsoleLog(Status.Trim().ToUpper() + " Notification send to Supplier. ");
                    }
                    else
                    {
                        LeSDM.AddConsoleLog("Unable to send " + Status.Trim().ToUpper() + " text notification. ");
                    }
                }
            }
            catch (Exception ex)
            {
                LeSDM.AddConsoleLog("Unable to send text notification. " + ex.Message);
                LeSDM.SetAuditLogFile("", "", "", "-", VRNO.Trim(), "Error", "Unable to create RMS Status File for Quote - " + QuoteRefNo.Trim(), "", this.BuyerCode, this.SuppCode, this.Server, this.Processor);
            }
        }
    }
}