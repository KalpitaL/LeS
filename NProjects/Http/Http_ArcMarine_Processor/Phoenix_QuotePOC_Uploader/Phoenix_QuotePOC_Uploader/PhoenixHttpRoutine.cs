using HtmlAgilityPack;
using LeSDataMain;
using MTML.GENERATOR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Phoenix_QuotePOC_Uploader
{
    class PhoenixHttpRoutine : LeSCommon.LeSCommon
    {
        string VRNO = "", QuoteRef = "", errorImg = "",
           _Domain = "", UploaderPath = "",
           AuditLogPath = "", SuppCode = "",
           Module = "", ImagePath = "", BuyerCode = "",
         Server = "", Processor = "";
        bool saveAsDraft = false, notifyByText = false;
        List<int> uploadedItems = new List<int>();
        MTMLClass _mtmlClass = new MTMLClass();

        HtmlAgilityPack.HtmlDocument CurrentDoc = new HtmlAgilityPack.HtmlDocument();
        public HTTPWrapper.HTTPWrapper _wrapper = new HTTPWrapper.HTTPWrapper();

        HttpWebResponse response = null;

        public PhoenixHttpRoutine()
        {
            Initialise();
        }

        public override void Initialise()
        {
            try
            {
                //base.Initialise();
                // Clear Old List //

                UploaderPath = convert.ToString(ConfigurationManager.AppSettings["QUOTE_POC_PATH"]);
                AuditLogPath = convert.ToString(ConfigurationManager.AppSettings["AUDIT_LOG_PATH"]);
                ImagePath = convert.ToString(ConfigurationManager.AppSettings["IMAGE_PATH"]);
                Server = convert.ToString(ConfigurationManager.AppSettings["SERVER"]);
                Processor = convert.ToString(ConfigurationManager.AppSettings["PROCESSOR"]);

                dctPostDataValues = new Dictionary<string, string>();
                dctAppSettings = new Dictionary<string, string>();
                foreach (string Key in ConfigurationManager.AppSettings.AllKeys)
                {
                    dctAppSettings.Add(Key.ToUpper().Trim(), convert.ToString(ConfigurationManager.AppSettings[Key]).Trim());
                }

            }
            catch (Exception ex)
            {
                LogText = "Error while setting lists -" + ex.Message;
            }
        }

        public void UploadQuote(MTMLInterchange obj, FileInfo quoteFile)
        {
            string VRNO = "", suppRef = "",
              QuoteExp = "", LatestDeliveryDate = "",
              Remarks = "", TermsCond = "",
              Currency = "", BuyerCode = "",
               VendorCode = "";
            double AddDiscount = 0;

            try
            {
                this.QuoteRef = "";
                int LeadDays = 0;
                double FrieghtCost = 0,
                    PackingCost = 0,
                    OtherCost = 0,
                    DepositCost = 0,
                    AdditionalCost = 0,
                    TaxCost = 0,
                    FinalAmt = 0,
                    AllowanceAmount = 0,
                    extraItemCost = 0;

                BuyerCode = obj.Recipient.ToString();
                VendorCode = obj.Sender.ToString();
                this.SuppCode = VendorCode.ToUpper();

                string orgURL = convert.ToString(obj.DocumentHeader.MessageReferenceNumber).Trim();
                string orgRFQURL = orgURL;

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
                //orgURL = orgURL.Replace("PurchaseQuotationRFQ", "PurchaseQuotationItems");

                _httpWrapper = new HTTPWrapper.HTTPWrapper();
                this.URL = orgURL;
                dctPostDataValues.Clear();
                SessionIDCookieName = "ASP.NET_SessionId";

                Dictionary<string, string> itemPageHiddenData = new Dictionary<string, string>();

                #region /* Validate Loaded WebPage */
                bool saveButtonFound = false;
                bool pageLoaded = LoadURL("form", "id", "frmPurchaseQuotationRFQ", true);
                if (pageLoaded)
                {
                    CurrentDoc = _httpWrapper._CurrentDocument;
                    HtmlAgilityPack.HtmlNode lnkSaveQuote = CurrentDoc.GetElementbyId("MenuQuotationLineItem_dlstTabs_ctl00_btnMenu");
                    if (convert.ToString(lnkSaveQuote.InnerText).Trim().ToLower() == "save")
                    {
                        saveButtonFound = true;
                    }
                }
                #endregion

                if (pageLoaded && saveButtonFound)
                {
                    #region // Set Quote Info //
                    Currency = convert.ToString(obj.DocumentHeader.CurrencyCode).Trim();
                    LeadDays = convert.ToInt(obj.DocumentHeader.LeadTimeDays);
                    AddDiscount = convert.ToDouble(obj.DocumentHeader.AdditionalDiscount);

                    foreach (Comments _comm in obj.DocumentHeader.Comments)
                    {
                        if (_comm.Qualifier == CommentTypes.SUR) Remarks = Convert.ToString(_comm.Value);
                        else if (_comm.Qualifier == CommentTypes.ZTC) TermsCond = "Terms & Condition : " + convert.ToString(_comm.Value);
                    }
                    Remarks = TermsCond + Environment.NewLine + Remarks;

                    DateTime dtQuoteVal = DateTime.MinValue;
                    foreach (DateTimePeriod dt in obj.DocumentHeader.DateTimePeriods)
                    {
                        if (dt.Qualifier == DateTimePeroidQualifiers.ExpiryDate_36)
                        {
                            dtQuoteVal = convert.ToDateTime(dt.Value, "yyyyMMddHHmm");
                            QuoteExp = dtQuoteVal.ToString("dd/MM/yyyy");
                        }
                        else if (dt.Qualifier == DateTimePeroidQualifiers.LatestDeliveryDate_2) LatestDeliveryDate = dt.Value;
                    }

                    // Check for Default values also //
                    if (dtQuoteVal == DateTime.MinValue)
                    {
                        string[] defaultQuoteExpDays = convert.ToString(ConfigurationManager.AppSettings["QUOTE_VALID"]).Trim().Split('|');
                        foreach (string sQuoteExpDays in defaultQuoteExpDays)
                        {
                            string[] values = sQuoteExpDays.Split('=');
                            if (values[0].Trim() == VendorCode.Trim() && values.Length > 1 && convert.ToInt(values[1]) > 0)
                            {
                                dtQuoteVal = DateTime.Now.AddDays(convert.ToInt(values[1]));
                                QuoteExp = dtQuoteVal.ToString("yyyy/MM/dd");
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
                        if (_amount.Qualifier == MonetoryAmountQualifier.PackingCost_106) PackingCost = convert.ToDouble(_amount.Value);
                        else if (_amount.Qualifier == MonetoryAmountQualifier.FreightCharge_64) FrieghtCost += convert.ToDouble(_amount.Value);
                        else if (_amount.Qualifier == MonetoryAmountQualifier.OtherCost_98) OtherCost = convert.ToDouble(_amount.Value);
                        else if (_amount.Qualifier == MonetoryAmountQualifier.Deposit_97) DepositCost = convert.ToDouble(_amount.Value);
                        else if (_amount.Qualifier == MonetoryAmountQualifier.PackingCost_106) AdditionalCost = convert.ToDouble(_amount.Value);
                        else if (_amount.Qualifier == MonetoryAmountQualifier.TaxCost_99) TaxCost = convert.ToDouble(_amount.Value);
                        else if (_amount.Qualifier == MonetoryAmountQualifier.GrandTotal_259) FinalAmt = convert.ToDouble(_amount.Value);
                        else if (_amount.Qualifier == MonetoryAmountQualifier.AllowanceAmount_204) AllowanceAmount = convert.ToDouble(_amount.Value);
                    }
                    #endregion

                    // Post Quote Data //
                    AddFixedHeaderFieldsForHeader();

                    /* Quote Ref */
                    string venderReference = System.Web.HttpUtility.UrlEncode(QuoteRef);
                    dctPostDataValues.Add("txtVenderReference", venderReference);

                    #region /* Quote Validity Date */
                    string val = dtQuoteVal.ToString("dd/MMM/yyyy").Replace("-", "/");
                    string quoteValidityShort = System.Web.HttpUtility.UrlEncode(val);
                    dctPostDataValues.Add("txtOrderDate", quoteValidityShort);
                    #endregion

                    #region /* Delivery Terms */
                    string[] delTerms = convert.ToString(ConfigurationManager.AppSettings["DEL_TERMS"]).Trim().Split('|');
                    Dictionary<string, string> lstDelTerms = new Dictionary<string, string>();
                    foreach (string s in delTerms)
                    {
                        string[] strValues = s.Trim().Split('=');
                        lstDelTerms.Add(strValues[0], strValues[1]);
                    }
                    if (!lstDelTerms.ContainsKey(VendorCode)) throw new Exception("No setting found for Delivery Terms of Vendor [" + VendorCode + "] ");
                    string currDelTerms = lstDelTerms[VendorCode];
                    if (currDelTerms.Trim().Length > 0)
                    {
                        string deliveryTermText = System.Web.HttpUtility.UrlEncode(currDelTerms);
                        string deliveryTermValue = "";

                        if (currDelTerms.ToUpper().StartsWith("EX-WORKS")) deliveryTermValue = "404";
                        else if (currDelTerms.ToUpper().StartsWith("CARRIAGE")) deliveryTermValue = "79";
                        else if (currDelTerms.ToUpper().StartsWith("EX-WHARF")) deliveryTermValue = "78";
                        else if (currDelTerms.ToUpper().StartsWith("FREE")) deliveryTermValue = "77";
                        else if (currDelTerms.ToUpper().StartsWith("OTHER")) deliveryTermValue = "279";

                        dctPostDataValues.Add("UCDeliveryTerms%24ddlQuick", deliveryTermValue);
                        //dctPostDataValues.Add("UCDeliveryTerms_ddlQuick_ClientState", "%7B%22logEntries%22%3A%5B%5D%2C%22value%22%3A%22" + deliveryTermValue + "%22%2C%22text%22%3A%22" + deliveryTermText + "%22%2C%22enabled%22%3Atrue%2C%22checkedIndices%22%3A%5B%5D%2C%22checkedItemsTextOverflows%22%3Afalse%7D");
                    }
                    else
                    {
                        dctPostDataValues.Add("UCDeliveryTerms%24ddlQuick", "Dummy");
                        //dctPostDataValues.Add("UCDeliveryTerms_ddlQuick_ClientState", "");
                    }
                    #endregion

                    /* Lead Time Days */
                    dctPostDataValues.Add("txtDeliveryTime%24txtNumber", LeadDays.ToString());

                    #region /* Payment Terms */
                    string[] payTerms = convert.ToString(ConfigurationManager.AppSettings["PAY_TERMS"]).Trim().Split('|');
                    Dictionary<string, string> lstPayTerms = new Dictionary<string, string>();
                    foreach (string s in payTerms)
                    {
                        string[] strValues = s.Trim().Split('=');
                        lstPayTerms.Add(strValues[0], strValues[1]);
                    }

                    if (!lstPayTerms.ContainsKey(VendorCode)) throw new Exception("No setting found for Payment Terms of Vendor [" + VendorCode + "] ");
                    string currPayTerms = lstPayTerms[VendorCode];
                    if (currPayTerms.Trim().Length > 0)
                    {
                        string paymentTerms = System.Web.HttpUtility.UrlPathEncode(currPayTerms);
                        string paymentTermValue = "";

                        if (currPayTerms.ToUpper().Trim().StartsWith("CASH ON")) paymentTermValue = "69";
                        else if (currPayTerms.ToUpper().Trim().StartsWith("ADVANCE PAYMENT")) paymentTermValue = "76";
                        else if (currPayTerms.ToUpper().Trim().StartsWith("NET 30")) paymentTermValue = "74";
                        else if (currPayTerms.ToUpper().Trim().StartsWith("NET 60")) paymentTermValue = "75";
                        else if (currPayTerms.ToUpper().Trim().StartsWith("NET 45")) paymentTermValue = "298";
                        else if (currPayTerms.ToUpper().Trim().StartsWith("NET 90")) paymentTermValue = "299";
                        else if (currPayTerms.ToUpper().Trim().StartsWith("OTHER")) paymentTermValue = "300";

                        dctPostDataValues.Add("UCPaymentTerms%24ddlQuick", paymentTermValue);
                    }
                    else
                    {
                        dctPostDataValues.Add("UCPaymentTerms%24ddlQuick", "Dummy");
                    }
                    #endregion

                    #region /* Mode Of Transport */
                    string optModeOfTransport = "", optTextModeOfTransport = "";
                    if (obj.DocumentHeader.TransportModeCode == TransportModeCode.Air)
                    {
                        // Select Air
                        optModeOfTransport = "344";
                        optTextModeOfTransport = "By Air Freight";
                    }
                    else if (obj.DocumentHeader.TransportModeCode == TransportModeCode.Maritime)
                    {
                        // select Sea
                        optModeOfTransport = "345";
                        optTextModeOfTransport = "By Sea";
                    }
                    else if (obj.DocumentHeader.TransportModeCode == TransportModeCode.Road)
                    {
                        // select Road
                        optModeOfTransport = "346";
                        optTextModeOfTransport = "By Roadways";
                    }
                    if (optModeOfTransport.Trim() != "")
                    {
                        string modeOfTransport = System.Web.HttpUtility.UrlEncode(optTextModeOfTransport);
                        dctPostDataValues.Add("ucModeOfTransport%24ddlQuick", optModeOfTransport);
                    }
                    else
                    {
                        dctPostDataValues.Add("ucModeOfTransport%24ddlQuick", "Dummy");
                    }
                    #endregion

                    #region /* Currency */
                    string currValue = GetCurrencyCode(Currency);
                    if (currValue.Trim().Length == 0) throw new Exception("Currency not found on site");
                    dctPostDataValues.Add("ucCurrency%24ddlCurrency", currValue);
                    #endregion

                    #region /* Header Discount */
                    if (AddDiscount > 0)
                    {
                        // Append Additional Discount to header comments
                        Remarks = "Additional Discount in % : " + AddDiscount + Environment.NewLine + Remarks;

                        dctPostDataValues.Add("txtTotalDiscount", "0.00");
                    }
                    else if (AllowanceAmount > 0) // Discounted Amount (Fuji)
                    {
                        dctPostDataValues.Add("txtTotalDiscount", AllowanceAmount.ToString("0.00"));
                        Remarks = "Allowance : " + AllowanceAmount + " " + Currency + Environment.NewLine + Remarks;
                    }
                    else
                    {
                        dctPostDataValues.Add("txtTotalDiscount", "0.00");
                    }
                    #endregion

                    /* Price  without charges */
                    dctPostDataValues.Add("txtPrice", "0.00");
                    /* Total Price */
                    dctPostDataValues.Add("txtTotalPrice", "0.00");

                    dctPostDataValues.Add("__EVENTTARGET", "MenuQuotationLineItem%24dlstTabs%24ctl00%24btnMenu");

                    // Post URL with Data //
                    bool upddated = this.PostData("input", "id", "txtVenderReference", true);
                    if (upddated)
                    {
                        // Check Quote Ref is updated //
                        if (_httpWrapper._CurrentDocument.GetElementbyId("txtVenderReference") != null)
                        {
                            if (_httpWrapper._CurrentDocument.GetElementbyId("txtVenderReference").GetAttributeValue("Value", "").Trim().Length == 0)
                            {
                                throw new Exception("Unable to update header details");
                            }
                        }

                        AddQuoteRemarks(Remarks.Trim(), orgRFQURL, orgURL);

                        List<LineItem> extraItems = FillItems(obj, orgRFQURL, orgURL);
                        LoadSite(orgURL, orgRFQURL);
                        LoadAllItems(obj.DocumentHeader.LineItemCount);

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
                        if (extraItems.Count > 0)
                        {
                            // Calculate Extra Item Cost //
                            foreach (LineItem eItem in extraItems)
                            {
                                if (convert.ToString(eItem.ItemType) == "")
                                {
                                    extraItemCost += eItem.MonetaryAmount;
                                }
                            }

                            FillExtraItems(extraItems, orgRFQURL, orgURL);

                            LoadSite(orgURL, orgRFQURL);
                            LoadAllItems(obj.DocumentHeader.LineItemCount);
                        }

                        // Match Quote Total //
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

                        // Send / Decline Quote //
                        if (saveAsDraft == false)
                        {
                            if (convert.ToInt(obj.DocumentHeader.IsDeclined) == 0) // QUOTE IS NOT DECLINED //
                            {
                                SendQuote(obj, orgURL, orgRFQURL, suppRef.Trim(), FinalAmt, extraItemCost, quoteFile, BuyerCode.Trim(), VendorCode.Trim());
                            }
                            else
                            {
                                //DeclineQuote(obj, suppRef.Trim(), xmlFile, BuyerCode.Trim(), VendorCode.Trim(), Remarks.Trim());
                                throw new Exception("Unable to decline quote");
                            }
                        }
                        else
                        {
                            #region // Match Total & Send Audit Log //
                            double totalPrice = 0;
                            HtmlAgilityPack.HtmlNode txtTotalPrice = _httpWrapper._CurrentDocument.GetElementbyId("txtTotalPrice");
                            if (txtTotalPrice != null) totalPrice = Math.Round(convert.ToDouble(txtTotalPrice.GetAttributeValue("Value", "0")), 2);

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
                                    string quoteImg = this.ImagePath + "\\PhoenixTelerik_DraftQuote_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".png";
                                    if (SavePage(quoteImg)) quoteImg = Path.GetFileName(quoteImg);
                                    else quoteImg = "";

                                    LeSDM.AddConsoleLog("Quote for '" + VRNO + "' saved as draft successfully.");
                                    MoveFile(quoteFile.Directory.FullName + "\\Backup", quoteFile); // Move File to Backup
                                    LeSDM.SetAuditLog(this.Module, quoteImg, VRNO.Trim(), "Uploaded", "Quote for '" + VRNO + "' saved as draft successfully.", BuyerCode, SuppCode, Server, Processor);
                                    this.SendTextNotification(VRNO.Trim(), QuoteRef, "SUCCESS", SuppCode);
                                    #endregion

                                    // Send Mail Notification
                                    if (convert.ToString(ConfigurationManager.AppSettings["SEND_MAIL_FOR_SUBMIT"]).Trim().ToUpper() == "TRUE")
                                    {
                                        LeSDM.AddConsoleLog("Generating mail notification...");
                                        SendMailNotification(obj, "QUOTE", VRNO.Trim(), "SUBMITTED", "Quote for REF '" + VRNO.Trim() + "' saved as draft successfully.");
                                    }
                                }
                                else
                                {
                                    #region // Quote total is mismatched //
                                    string errImg = this.ImagePath + "\\PhoenixTelerik_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                                    if (SavePage(errImg)) errImg = Path.GetFileName(errImg);
                                    else errImg = "";

                                    LeSDM.AddConsoleLog("Quote total is mismatched for '" + VRNO + "'.");

                                    // Set Error Log //
                                    TrackErrors.TrackError.SetErrorCount(SuppCode, VRNO.Trim(), "TOTAL_MISMATCHED");
                                    int errCount = TrackErrors.TrackError.GetErrorCount(SuppCode, VRNO.Trim(), "TOTAL_MISMATCHED");
                                    if (errCount == 3)
                                    {
                                        LeSDM.SetAuditLog(this.Module, errImg, VRNO.Trim(), "Error", "Unable to save quote for Ref '" + this.VRNO + "'; Quote total is mismatched.", BuyerCode, SuppCode, Server, Processor);
                                        TrackErrors.TrackError.DeleteErrorCountFile(SuppCode, VRNO.Trim(), "TOTAL_MISMATCHED");
                                        MoveFile(quoteFile.Directory.FullName + "\\Error_Files", quoteFile);
                                        this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                #region Quote total is 0
                                string errImg = this.ImagePath + "\\PhoenixTelerik_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".png";
                                if (SavePage(errImg)) errImg = Path.GetFileName(errImg);
                                else errImg = "";

                                LeSDM.AddConsoleLog("Quote total is 0 for '" + VRNO + "'.");

                                // Set Error Log //
                                TrackErrors.TrackError.SetErrorCount(SuppCode, VRNO.Trim(), "QUOTE_AMT_ZERO");
                                int errCount = TrackErrors.TrackError.GetErrorCount(SuppCode, VRNO.Trim(), "QUOTE_AMT_ZERO");
                                if (errCount == 3)
                                {
                                    LeSDM.SetAuditLog(this.Module, errImg, VRNO.Trim(), "Error", "Unable to save quote for Ref '" + this.VRNO + "'; Quote total is 0.", BuyerCode, SuppCode, Server, Processor);
                                    TrackErrors.TrackError.DeleteErrorCountFile(SuppCode, VRNO.Trim(), "QUOTE_AMT_ZERO");
                                    MoveFile(quoteFile.Directory.FullName + "\\Error_Files", quoteFile); // Move File to Error Files Folder
                                    this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                                }
                                #endregion
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    #region // Quote Already Submitted //
                    string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".png";
                    if (SavePage(errImg)) errImg = Path.GetFileName(errImg);
                    else errImg = "";

                    LeSDM.AddConsoleLog("Send Quote button not found on webpage for '" + VRNO + "'. ");
                    LeSDM.AddConsoleLog("Quote is already submitted.");

                    LeSDM.SetAuditLog(this.Module, errImg, VRNO.Trim(), "Error", "Unable to save quote for Ref '" + this.VRNO + "'; Save button not found on webpage.", BuyerCode, SuppCode, Server, Processor);
                    MoveFile(quoteFile.Directory.FullName + "\\Error_Files", quoteFile);
                    this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (errorImg.Length == 0) errorImg = quoteFile.Name;

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
                            // Move File to Error Files Folder
                            MoveFile(quoteFile.Directory.FullName + "\\Error_Files", quoteFile);
                            this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                        }
                    }
                }
                finally { }
            }
        }

        private void AddFixedHeaderFieldsForHeader()
        {
            CurrentDoc = _httpWrapper._CurrentDocument;

            dctPostDataValues.Clear();
            dctPostDataValues.Add("ToolkitScriptManager1_HiddenField", "");
            dctPostDataValues.Add("__EVENTARGUMENT", "");
            dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
            dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
            dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);

            string vessel = System.Web.HttpUtility.UrlEncode(CurrentDoc.GetElementbyId("txtVessel").GetAttributeValue("Value", "").ToString());
            dctPostDataValues.Add("txtVessel", vessel);

            string imo = CurrentDoc.GetElementbyId("txtIMONo").GetAttributeValue("Value", "").ToString().Trim();
            dctPostDataValues.Add("txtIMONo", imo);

            string hullNo = System.Web.HttpUtility.UrlEncode(CurrentDoc.GetElementbyId("txtHullNo").GetAttributeValue("Value", "")).Replace(",", "%2C");
            dctPostDataValues.Add("txtHullNo", hullNo);

            string vesselType = System.Web.HttpUtility.UrlEncode(CurrentDoc.GetElementbyId("txtVesseltype").GetAttributeValue("Value", "").ToString());
            vesselType = vesselType.Replace(",", "%2C");
            dctPostDataValues.Add("txtVesseltype", vesselType);

            string yard = System.Web.HttpUtility.UrlEncode(CurrentDoc.GetElementbyId("txtYard").GetAttributeValue("Value", "").ToString()).Replace(",", "%2C");
            dctPostDataValues.Add("txtYard", yard);

            string yearBuilt = System.Web.HttpUtility.UrlEncode(CurrentDoc.GetElementbyId("txtYearBuilt").GetAttributeValue("Value", "").ToString()).Replace(",", "%2C");
            dctPostDataValues.Add("txtYearBuilt", yearBuilt);

            string senderName = System.Web.HttpUtility.UrlEncode(CurrentDoc.GetElementbyId("txtSenderName").GetAttributeValue("Value", "").ToString());
            dctPostDataValues.Add("txtSenderName", senderName);

            string deliveryPlace = System.Web.HttpUtility.UrlEncode(CurrentDoc.GetElementbyId("txtDeliveryPlace").GetAttributeValue("Value", "").ToString());
            dctPostDataValues.Add("txtDeliveryPlace", deliveryPlace);

            string contactNo = System.Web.HttpUtility.UrlEncode(CurrentDoc.GetElementbyId("txtContactNo").GetAttributeValue("Value", "").ToString());
            dctPostDataValues.Add("txtContactNo", contactNo);

            string senderEmailId = System.Web.HttpUtility.UrlEncode(CurrentDoc.GetElementbyId("txtSenderEmailId").GetAttributeValue("Value", "").ToString());
            dctPostDataValues.Add("txtSenderEmailId", senderEmailId);

            string portName = System.Web.HttpUtility.UrlEncode(CurrentDoc.GetElementbyId("txtPortName").GetAttributeValue("Value", "").ToString());
            dctPostDataValues.Add("txtPortName", portName);

            string sentDate = System.Web.HttpUtility.UrlEncode(CurrentDoc.GetElementbyId("txtSentDate").GetAttributeValue("Value", "").ToString());
            dctPostDataValues.Add("txtSentDate", sentDate);

            string componentName = System.Web.HttpUtility.UrlEncode(CurrentDoc.GetElementbyId("txtComponentName").GetAttributeValue("Value", "").ToString());
            dctPostDataValues.Add("txtComponentName", componentName);

            string componentModel = System.Web.HttpUtility.UrlEncode(CurrentDoc.GetElementbyId("txtComponentModel").GetAttributeValue("Value", "").ToString());
            dctPostDataValues.Add("txtComponentModel", componentModel);

            string componentSerialNo = System.Web.HttpUtility.UrlEncode(CurrentDoc.GetElementbyId("txtComponentSerialNo").GetAttributeValue("Value", "").ToString());
            dctPostDataValues.Add("txtComponentSerialNo", componentSerialNo);

            string vendorName = System.Web.HttpUtility.UrlEncode(CurrentDoc.GetElementbyId("txtVendorName").GetAttributeValue("Value", "").ToString());
            dctPostDataValues.Add("txtVendorName", vendorName);

            string telephone = System.Web.HttpUtility.UrlEncode(CurrentDoc.GetElementbyId("txtTelephone").GetAttributeValue("Value", "").ToString());
            dctPostDataValues.Add("txtTelephone", telephone);

            string fax = System.Web.HttpUtility.UrlEncode(CurrentDoc.GetElementbyId("txtFax").GetAttributeValue("Value", "").ToString()).Replace(",", "%2C");
            dctPostDataValues.Add("txtFax", fax);

            string vendorAddress = System.Web.HttpUtility.UrlEncode(convert.ToString(CurrentDoc.GetElementbyId("txtVendorAddress").InnerText).Trim()).Replace(",", "%2C");
            dctPostDataValues.Add("txtVendorAddress", vendorAddress);

            string email = System.Web.HttpUtility.UrlEncode(CurrentDoc.GetElementbyId("txtEmail").GetAttributeValue("Value", "").ToString());
            dctPostDataValues.Add("txtEmail", email);

            string priority = System.Web.HttpUtility.UrlEncode(CurrentDoc.GetElementbyId("txtPriority").GetAttributeValue("Value", "").ToString());
            dctPostDataValues.Add("txtPriority", priority);

            dctPostDataValues.Add("ddlType%24ddlHard", "Dummy");
            dctPostDataValues.Add("txtSupplierDiscount", "0.00");
            dctPostDataValues.Add("txtDiscount", "0.00");
            dctPostDataValues.Add("meeDiscount_ClientState", "");
            dctPostDataValues.Add("gvTax%24ctl03%24txtDescriptionAdd", "");
            dctPostDataValues.Add("gvTax%24ctl03%24ucTaxTypeAdd%24rblValuePercentage", "2");
            dctPostDataValues.Add("gvTax%24ctl03%24txtValueAdd", "");
            dctPostDataValues.Add("gvTax%24ctl03%24MaskedEditTotalPayableAmout_ClientState", "");
            dctPostDataValues.Add("isouterpage", "");
            dctPostDataValues.Add("txtnopage", "");
        }

        private void AddQuoteRemarks(string Remarks, string orgRFQURL, string orgURL)
        {
            this.URL = orgURL;
            bool pageLoaded = LoadURL("form", "id", "frmPurchaseQuotationRFQ", true);

            string vesselID = "";
            int indx = _httpWrapper._CurrentDocument.DocumentNode.InnerHtml.IndexOf("VESSELID=");
            if (indx > 0)
            {
                vesselID = _httpWrapper._CurrentDocument.DocumentNode.InnerHtml.Substring(indx, 15);
                vesselID = vesselID.Split('&')[0].Trim().Split('=')[1].Trim();
            }

            if (pageLoaded)
            {
                // Get Remarks Window URL  //
                //https://apps.southnests.com/Phoenix/Purchase/PurchaseVendorDetail.aspx?QUOTATIONID=414F8164-D82A-EA11-80D9-065916D1083C&STOCKTYPE=STORE&editable=true

                Uri _uri = new Uri(orgURL);
                string remarksURL = _uri.Scheme + "://" + _uri.Host;
                foreach (string segment in _uri.Segments)
                {
                    remarksURL += segment;
                }
                remarksURL += "?";
                remarksURL += "QUOTATIONID=" + _uri.Query.Split('&')[0].Split('=')[1].Trim();
                remarksURL += "&VESSELID=" + vesselID;
                remarksURL += "&" + _uri.Query.Split('&')[1].Trim();                
                remarksURL += "&editable=true";
                remarksURL = remarksURL.Replace("PurchaseQuotationRFQ", "PurchaseVendorDetail");

                this.URL = remarksURL;
                bool frameLoaded = LoadURL("form", "id", "frmPurchaseFormDetail", true);
                if (frameLoaded)
                {
                    dctPostDataValues.Clear();

                    //string encodedRemarks = System.Web.HttpUtility.UrlEncode(System.Web.HttpUtility.HtmlEncode(System.Web.HttpUtility.UrlPathEncode(Remarks)));//.Replace(",", "%2C");
                    Remarks = Remarks.Replace(Environment.NewLine, " ").Replace("&", "and");
                    string encodedRemarks = System.Web.HttpUtility.HtmlAttributeEncode(Remarks);
                    encodedRemarks = System.Web.HttpUtility.UrlPathEncode(encodedRemarks).Replace("&quot;", "%2522");                    

                    string postdata = "-----------------------------7e43a1110632\r\n" +
                        "Content-Disposition: form-data; name=\"\"toolkitscriptmanager1_hiddenfield\"\"\r\n" +
                        "\r\n" +
                        "\r\n" +
                        "-----------------------------7e43a1110632\r\n" +
                        "Content-Disposition: form-data; name=\"\"__eventtarget\"\"\r\n" +
                        "\r\n" +
                        "MenuFormDetail$dlstTabs$ctl00$btnMenu\r\n" +
                        "-----------------------------7e43a1110632\r\n" +
                        "Content-Disposition: form-data; name=\"\"__eventargument\"\"\r\n" +
                        "\r\n" +
                        "\r\n" +
                        "-----------------------------7e43a1110632\r\n" +
                        "Content-Disposition: form-data; name=\"\"_contentchanged_txtformdetails_uccustomeditor_ctl02\"\"\r\n" +
                        "\r\n" +
                        "\r\n" +
                        "-----------------------------7e43a1110632\r\n" +
                        "Content-Disposition: form-data; name=\"\"_contentforce_txtformdetails_uccustomeditor_ctl02\"\"\r\n" +
                        "\r\n" +
                        "1\r\n" +
                        "-----------------------------7e43a1110632\r\n" +
                        "Content-Disposition: form-data; name=\"\"_content_txtformdetails_uccustomeditor_ctl02\"\"\r\n" +
                        "\r\n" +
                        "#REMARKS#\r\n" +
                        "-----------------------------7e43a1110632\r\n" +
                        "Content-Disposition: form-data; name=\"\"_activemode_txtformdetails_uccustomeditor_ctl02\"\"\r\n" +
                        "\r\n" +
                        "0\r\n" +
                        "-----------------------------7e43a1110632\r\n" +
                        "Content-Disposition: form-data; name=\"\"__viewstate\"\"\r\n" +
                        "\r\n" +
                        "#VIEWSTATE#\r\n" +
                        "-----------------------------7e43a1110632\r\n" +
                        "Content-Disposition: form-data; name=\"\"__viewstategenerator\"\"\r\n" +
                        "\r\n" +
                        "#VIEWSTATEGEN#\r\n" +
                        "-----------------------------7e43a1110632\r\n" +
                        "Content-Disposition: form-data; name=\"\"__eventvalidation\"\"\r\n" +
                        "\r\n" +
                        "#EVENTVALIDATION#\r\n" +
                        "-----------------------------7e43a1110632\r\n" +
                        "Content-Disposition: form-data; name=\"\"txtformdetails$uccustomeditor$ctl01$ctl47$ctl05\"\"\r\n" +
                        "\r\n" +
                        "\r\n" +
                        "-----------------------------7e43a1110632\r\n" +
                        "Content-Disposition: form-data; name=\"\"txtformdetails$uccustomeditor$ctl01$ctl47$ctl07\"\"\r\n" +
                        "\r\n" +
                        "_blank\r\n" +
                        "-----------------------------7e43a1110632\r\n" +
                        "Content-Disposition: form-data; name=\"\"txtformdetails$btnimageupload\"\"; filename=\"\"\"\"\r\n" +
                        "Content-Type: application/octet-stream\r\n" +
                        "\r\n" +
                        "\r\n" +
                        "-----------------------------7e43a1110632--\r\n";

                    postdata = postdata.Replace("#REMARKS#", ".." + System.Web.HttpUtility.HtmlEncode(Remarks.Trim()));
                    postdata = postdata.Replace("#VIEWSTATE#", _httpWrapper._dctStateData["__VIEWSTATE"]);
                    postdata = postdata.Replace("#VIEWSTATEGEN#", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                    postdata = postdata.Replace("#EVENTVALIDATION#", _httpWrapper._dctStateData["__EVENTVALIDATION"]);

                    //bool remarksUpdated = this.PostData("span", "id", "ucStatus_litMessage", true, postdata);
                    bool remarksUpdated = this.PostDataWithMultipart(remarksURL, postdata, "span", "id", "ucStatus_litMessage");
                    if (remarksUpdated)
                    {
                        HtmlAgilityPack.HtmlNode ndSpan = _httpWrapper._CurrentDocument.GetElementbyId("ucStatus_litMessage");
                        string result = convert.ToString(ndSpan.InnerText).Trim().ToUpper();
                        if (result == "DATA HAS BEEN SAVED")
                        {
                            LeSDM.AddLog("Quote Remarks Updated.");
                        }
                        else throw new Exception("Unable to update quote remarks.");
                    }
                }
            }
            else throw new Exception("Unable to reload RFQ page");
        }

        private List<LineItem> FillItems(MTMLInterchange obj, string orgRFQURL, string orgURL)
        {
            Dictionary<int, LineItem> _items = new Dictionary<int, LineItem>();
            Dictionary<string, string> _itemRemarks = new Dictionary<string, string>();
            List<LineItem> _extraItems = new List<LineItem>();

            #region  // Reload Page //
            LeSDM.AddConsoleLog("Reload WebPage ");

            this.URL = orgURL;
            bool pageLoaded = LoadURL("form", "id", "frmPurchaseQuotationItems", true);
            this.URL = orgRFQURL;
            pageLoaded = LoadURL("form", "id", "frmPurchaseQuotationRFQ", true);
            #endregion

            if (pageLoaded)
            {
                LeSDM.AddConsoleLog("Filling items ");

                uploadedItems.Clear();
                LoadAllItems(obj.DocumentHeader.LineItemCount);

                #region // Sort Items //
                foreach (LineItem item in obj.DocumentHeader.LineItems)
                {
                    if (convert.ToInt(item.IsExtraItem) == 1)
                    {
                        _extraItems.Add(item);
                    }
                    else _items.Add(convert.ToInt(item.OriginatingSystemRef), item);
                }
                #endregion

                CurrentDoc = _httpWrapper._CurrentDocument;

                HtmlAgilityPack.HtmlNode tblItems = CurrentDoc.GetElementbyId("gvVendorItem_ctl00");
                if (tblItems != null)
                {
                    if (convert.ToString(tblItems.InnerText).ToString().ToUpper().Contains("NO RECORDS FOUND"))
                    {
                        throw new Exception("No item is present in grid");
                    }

                    HtmlAgilityPack.HtmlNodeCollection rows = tblItems.SelectNodes("tbody/tr");
                    int itemCount = rows.Count;

                    for (int i = 0; i < itemCount; i++)
                    {
                        string itemCtlID = "";
                        LineItem iObj = null;
                        HtmlAgilityPack.HtmlNode nRow = CurrentDoc.GetElementbyId("gvVendorItem_ctl00__" + i);

                        HtmlAgilityPack.HtmlNodeCollection cells = nRow.SelectNodes("td");
                        HtmlAgilityPack.HtmlNode spn = cells[0].SelectSingleNode("span");
                        string _content = convert.ToString(spn.InnerText).Trim();
                        if (_content.Trim() == "") _content = convert.ToString(cells[0].InnerText).Trim();

                        if (_content.Trim().Length > 0)
                        {
                            int ItemNo = convert.ToInt(_content);

                            // Check Item exists in Item List //
                            if (_items.ContainsKey(ItemNo))
                            {
                                iObj = _items[ItemNo];
                                if (iObj.Quantity > 0 && convert.ToFloat(iObj.MonetaryAmount) > 0)
                                {
                                    HtmlAgilityPack.HtmlNode spnTotal = cells[11].SelectSingleNode("span");
                                    HtmlAgilityPack.HtmlNode spnId = cells[0].SelectSingleNode("span");
                                    string id = spnId.Id;
                                    itemCtlID = id.Split('_')[2];
                                    bool uomFound = false;

                                    #region // Get Item data //
                                    string uom = convert.ToString(iObj.MeasureUnitQualifier);
                                    double Qty = convert.ToDouble(iObj.Quantity);
                                    double price = 0, discount = 0;
                                    int leadDays = 0;
                                    if (convert.ToInt(iObj.DeleiveryTime) > 0)
                                    {
                                        leadDays = convert.ToInt(iObj.DeleiveryTime);
                                    }
                                    foreach (PriceDetails amt in iObj.PriceList)
                                    {
                                        if (amt.TypeQualifier == PriceDetailsTypeQualifiers.GRP) price = amt.Value;
                                        else if (amt.TypeQualifier == PriceDetailsTypeQualifiers.DPR) discount = amt.Value;
                                    }
                                    #endregion

                                    #region // Edit Item //
                                    dctPostDataValues.Clear();
                                    dctPostDataValues.Add("__EVENTTARGET", "gvVendorItem%24ctl00%24" + itemCtlID + "%24cmdEdit");
                                    dctPostDataValues.Add("__EVENTARGUMENT", "");
                                    dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                                    dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                                    dctPostDataValues.Add("__SCROLLPOSITIONX", "0");
                                    dctPostDataValues.Add("__SCROLLPOSITIONY", "526.66");
                                    dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);

                                    bool editItemEnable = PostData("input", "id", "gvVendorItem_ctl00_" + itemCtlID + "_txtQuantityEdit_txtDecimal", true);
                                    if (editItemEnable == false) throw new Exception("Unable to edit item.");
                                    #endregion

                                    #region // Search UOM in List //
                                    HtmlAgilityPack.HtmlNode nUnits = CurrentDoc.GetElementbyId("gvVendorItem_ctl00_" + itemCtlID + "_ucUnit_ddlUnit_DropDown");
                                    if (nUnits != null)
                                    {
                                        HtmlAgilityPack.HtmlNodeCollection lstUnits = nUnits.SelectNodes("div/ul/li");
                                        if (lstUnits.Count == 1) throw new Exception("Units not present for item -" + ItemNo);
                                        for (int c = 0; c < lstUnits.Count; c++)
                                        {
                                            if (convert.ToString(lstUnits[c].InnerText).Trim().ToUpper() == convert.ToString(iObj.MeasureUnitQualifier).Trim().ToUpper())
                                            {
                                                uomFound = true; break;
                                            }
                                        }
                                    }
                                    #endregion

                                    if (!uomFound)
                                    {
                                        // Calculate AVG Price //
                                        double unitPrice = price; // Qty Price //
                                        string orgQtyStr = CurrentDoc.GetElementbyId("gvVendorItem_ctl00_" + itemCtlID + "_txtQuantityEdit_txtDecimal").GetAttributeValue("Value", "");
                                        if (orgQtyStr.Trim().Length == 0) throw new Exception("Item Site Quantity is 0.");

                                        else if (orgQtyStr.Trim().Length > 0 && convert.ToFloat(orgQtyStr) != convert.ToFloat(iObj.Quantity))
                                        {
                                            price = (convert.ToFloat(iObj.Quantity) * convert.ToFloat(price)) / convert.ToDouble(orgQtyStr);
                                        }
                                        uom = iObj.Byr_UOM;
                                    }

                                    #region // Save Item Data //
                                    dctPostDataValues.Clear();
                                    dctPostDataValues.Add("__EVENTTARGET", "gvVendorItem%24ctl00%24" + itemCtlID + "%24cmdUpdate");
                                    dctPostDataValues.Add("__EVENTARGUMENT", "");
                                    dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                                    dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                                    dctPostDataValues.Add("__SCROLLPOSITIONX", _httpWrapper._dctStateData["__SCROLLPOSITIONX"]);
                                    dctPostDataValues.Add("__SCROLLPOSITIONY", _httpWrapper._dctStateData["__SCROLLPOSITIONY"]);
                                    dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);
                                    dctPostDataValues.Add("gvVendorItem%24ctl00%24" + itemCtlID + "%24txtQuantityEdit%24txtDecimal", Qty.ToString());
                                    dctPostDataValues.Add("gvVendorItem_ctl00_" + itemCtlID + "_txtQuantityEdit_txtDecimal_ClientState", "%7B%22enabled%22%3Atrue%2C%22emptyMessage%22%3A%22%22%2C%22validationText%22%3A%22" + Qty + "%22%2C%22valueAsString%22%3A%22" + Qty + "%22%2C%22minValue%22%3A0%2C%22maxValue%22%3A70368744177664%2C%22lastSetTextBoxValue%22%3A%22" + Qty + "%22%7D");
                                    dctPostDataValues.Add("gvVendorItem%24ctl00%24" + itemCtlID + "%24ucUnit%24ddlUnit", uom);
                                    dctPostDataValues.Add("gvVendorItem_ctl00_" + itemCtlID + "_ucUnit_ddlUnit_ClientState", "%7B%22logEntries%22%3A%5B%5D%2C%22value%22%3A%22" + uom + "%22%2C%22text%22%3A%22" + uom + "%22%2C%22enabled%22%3Atrue%2C%22checkedIndices%22%3A%5B%5D%2C%22checkedItemsTextOverflows%22%3Afalse%7D");
                                    dctPostDataValues.Add("gvVendorItem%24ctl00%24" + itemCtlID + "%24txtQuotedPriceEdit%24txtDecimal", price.ToString("0.00"));
                                    dctPostDataValues.Add("gvVendorItem_ctl00_" + itemCtlID + "_txtQuotedPriceEdit_txtDecimal_ClientState", "%7B%22enabled%22%3Atrue%2C%22emptyMessage%22%3A%22%22%2C%22validationText%22%3A%22" + price + "%22%2C%22valueAsString%22%3A%22" + price + "%22%2C%22minValue%22%3A0%2C%22maxValue%22%3A70368744177664%2C%22lastSetTextBoxValue%22%3A%22" + price + "%22%7D");
                                    dctPostDataValues.Add("gvVendorItem%24ctl00%24" + itemCtlID + "%24txtDiscountEdit%24txtDecimal", discount.ToString());
                                    dctPostDataValues.Add("gvVendorItem_ctl00_" + itemCtlID + "_txtDiscountEdit_txtDecimal_ClientState", "%7B%22enabled%22%3Atrue%2C%22emptyMessage%22%3A%22%22%2C%22validationText%22%3A%22" + discount.ToString() + "%22%2C%22valueAsString%22%3A%22" + discount.ToString() + "%22%2C%22minValue%22%3A0%2C%22maxValue%22%3A99.99%2C%22lastSetTextBoxValue%22%3A%22" + discount.ToString() + "%22%7D");
                                    dctPostDataValues.Add("gvVendorItem%24ctl00%24" + itemCtlID + "%24txtDeliveryTimeEdit%24txtDecimal", leadDays.ToString());
                                    dctPostDataValues.Add("gvVendorItem_ctl00_" + itemCtlID + "_txtDeliveryTimeEdit_txtDecimal_ClientState", "%7B%22enabled%22%3Atrue%2C%22emptyMessage%22%3A%22%22%2C%22validationText%22%3A%22" + leadDays.ToString() + "%22%2C%22valueAsString%22%3A%22" + leadDays.ToString() + "%22%2C%22minValue%22%3A0%2C%22maxValue%22%3A999%2C%22lastSetTextBoxValue%22%3A%22" + leadDays.ToString() + "%22%7D");

                                    bool itemSaved = PostData("a", "id", "gvVendorItem_ctl00_" + itemCtlID + "_cmdEdit", true);
                                    if (itemSaved)
                                    {
                                        // Check For Error Msg //
                                        HtmlAgilityPack.HtmlNode errorWindow = _httpWrapper._CurrentDocument.GetElementbyId("ucError_spnHeaderMessage");
                                        if (errorWindow != null)
                                        {
                                            throw new Exception("Error while saving item -" + convert.ToString(errorWindow.InnerText).Replace("\n", " "));
                                        }
                                        else
                                        {
                                            LeSDM.AddConsoleLog("Item - " + ItemNo + " updated.");
                                            uploadedItems.Add(ItemNo);

                                            #region // Add Remarks link to list //
                                            if (convert.ToString(iObj.LineItemComment.Value).Trim().Length > 0)
                                            {
                                                HtmlAgilityPack.HtmlNode lnkRemarks = _httpWrapper._CurrentDocument.GetElementbyId("gvVendorItem_ctl00_" + itemCtlID + "_imgVendorNotes");
                                                if (lnkRemarks != null)
                                                {
                                                    // Get Link OnClick Event //
                                                    string remLink = lnkRemarks.GetAttributeValue("onclick", "");
                                                    int _startIndx = remLink.IndexOf("PurchaseFormItemMoreInfo.aspx?");
                                                    if (_startIndx > 0)
                                                    {
                                                        int _endIndx = remLink.Substring(_startIndx).IndexOf("Y&");
                                                        if (_endIndx > 0) remLink = remLink.Substring(_startIndx, _endIndx);
                                                    }
                                                    else remLink = "";

                                                    // Recreate Remarks URL //
                                                    if (remLink.Trim().Length > 0)
                                                    {
                                                        remLink = remLink.Replace("&amp;", "&") + "Y";

                                                        Uri _url = (new Uri(orgURL));
                                                        string host = _url.Host;
                                                        string _itemRemURL = _url.Scheme + "://" + host;
                                                        for (int seg = 0; seg < _url.Segments.Length - 1; seg++)
                                                        {
                                                            _itemRemURL += _url.Segments[seg];
                                                        }
                                                        _itemRemURL = _itemRemURL + remLink.Trim();
                                                        _itemRemarks.Add(ItemNo + "|" + _itemRemURL, convert.ToString(iObj.LineItemComment.Value).Trim());
                                                    }
                                                    else
                                                    {
                                                        throw new Exception("Item Remarks URL not found ");
                                                    }
                                                }
                                                else throw new Exception("Item Remarks link button not found");
                                            }
                                            #endregion
                                        }
                                    }
                                    else throw new Exception("Unable to update item -" + ItemNo);
                                    #endregion
                                }
                            }
                            else throw new Exception("Item no. " + ItemNo + " not found in MTML Quote file.");
                        }
                        else LeSDM.AddConsoleLog("Unable to get content at index " + i);
                    }
                }
                else throw new Exception("Item grid not found");
            }
            else throw new Exception("Unable to reload RFQ page while filling items");

            // Update Item Remarks //
            foreach (KeyValuePair<string, string> pair in _itemRemarks)
            {
                string itemNo = pair.Key.Split('|')[0];

                // Load Item Remakrs Page //
                string remURL = pair.Key.Split('|')[1];
                this.URL = remURL;
                bool iRemPageLoaded = LoadURL("span", "title", "Save", true);
                if (iRemPageLoaded)
                {
                    string Remarks = pair.Value.Replace(Environment.NewLine, " ");
                    string encodedRemarks = System.Web.HttpUtility.HtmlEncode(Remarks);
                    encodedRemarks = System.Web.HttpUtility.UrlEncode(encodedRemarks);

                    // Update Item Remakrs Page //
                    dctPostDataValues.Clear();
                    dctPostDataValues.Add("__EVENTTARGET", "MenuLineItemDetail%24dlstTabs");
                    dctPostDataValues.Add("__EVENTARGUMENT", "0");
                    dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
                    dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
                    dctPostDataValues.Add("txtItemDetails", Remarks);
                    dctPostDataValues.Add("txtItemDetails_ClientState", "");

                    bool remUpdated = this.PostData("span", "title", "Save", true);
                    if (remUpdated) LeSDM.AddLog("Item Remarks Updated for -" + itemNo);
                    else throw new Exception("Unable to update items remarks for -" + itemNo);
                }
                else throw new Exception("Unable to open item remarks window for item -" + itemNo);
            }

            return _extraItems;
        }

        private void FillExtraItems(List<LineItem> extraitems, string orgRFQURL, string orgURL)
        {
            LoadSite(orgURL, orgRFQURL);

            foreach (LineItem eItem in extraitems)
            {
                dctPostDataValues.Clear();
                dctPostDataValues.Add("__EVENTTARGET", "");
                dctPostDataValues.Add("__EVENTTARGET", "");
                dctPostDataValues.Add("__EVENTTARGET", "");
                dctPostDataValues.Add("__EVENTTARGET", "");
                dctPostDataValues.Add("__EVENTTARGET", "");
                dctPostDataValues.Add("__EVENTTARGET", "");
                dctPostDataValues.Add("__EVENTTARGET", "");
            }

            throw new Exception("Unable to add extra cost...");
        }

        private void LoadAllItems(int Count)
        {
            dctPostDataValues.Clear();

            dctPostDataValues.Add("RadScriptManager1_TSM", "");
            dctPostDataValues.Add("__EVENTTARGET", "");
            dctPostDataValues.Add("__EVENTARGUMENT", "");
            dctPostDataValues.Add("__VIEWSTATE", _httpWrapper._dctStateData["__VIEWSTATE"]);
            dctPostDataValues.Add("__VIEWSTATEGENERATOR", _httpWrapper._dctStateData["__VIEWSTATEGENERATOR"]);
            dctPostDataValues.Add("__SCROLLPOSITIONX", "0");
            dctPostDataValues.Add("__SCROLLPOSITIONY", "700");
            dctPostDataValues.Add("__PREVIOUSPAGE", _httpWrapper._dctStateData["__PREVIOUSPAGE"]);
            dctPostDataValues.Add("RadWindowManager1_ClientState", "");
            dctPostDataValues.Add("MenuQuotationLineItem_dlstTabs_ClientState", "");
            dctPostDataValues.Add("MenuRegistersStockItem_dlstTabs_ClientState", "");

            dctPostDataValues.Add("gvVendorItem%24ctl00%24ctl02%24ctl00%24GoToPageTextBox", "1");
            dctPostDataValues.Add("gvVendorItem_ctl00_ctl02_ctl00_GoToPageTextBox_ClientState", "%7B%22enabled%22%3Atrue%2C%22emptyMessage%22%3A%22%22%2C%22validationText%22%3A%221%22%2C%22valueAsString%22%3A%221%22%2C%22minValue%22%3A1%2C%22maxValue%22%3A5%2C%22lastSetTextBoxValue%22%3A%221%22%7D");
            dctPostDataValues.Add("gvVendorItem%24ctl00%24ctl02%24ctl00%24ChangePageSizeTextBox", Count.ToString());
            dctPostDataValues.Add("gvVendorItem_ctl00_ctl02_ctl00_ChangePageSizeTextBox_ClientState", "%7B%22enabled%22%3Atrue%2C%22emptyMessage%22%3A%22%22%2C%22validationText%22%3A%22" + Count + "%22%2C%22valueAsString%22%3A%22" + Count + "%22%2C%22minValue%22%3A1%2C%22maxValue%22%3A" + Count + "%2C%22lastSetTextBoxValue%22%3A%22" + Count + "%22%7D");
            dctPostDataValues.Add("gvVendorItem%24ctl00%24ctl02%24ctl00%24ChangePageSizeLinkButton", "Change");
            dctPostDataValues.Add("gvVendorItem_ClientState", "");
            dctPostDataValues.Add("RadCommonToolTipManager1_ClientState", "");

            bool itemPageLoaded = PostData("table", "id", "gvVendorItem_ctl00_TopPager", true);
            if (itemPageLoaded)
            {
                HtmlAgilityPack.HtmlNode ndTotalPages = _httpWrapper._CurrentDocument.GetElementbyId("gvVendorItem_ctl00_ctl02_ctl00_PageOfLabel");
                if (convert.ToString(ndTotalPages.InnerText) == "of 1")
                {
                    // Set Log
                    LeSDM.AddLog("All items loaded successfully.");
                }
                else throw new Exception("Unable to load all items");
            }
        }

        private void LoadSite(string orgURL, string orgRFQURL)
        {
            this.URL = orgURL;
            bool loaded = LoadURL("form", "id", "frmPurchaseQuotationItems", true);
            if (loaded)
            {
                this.URL = orgRFQURL;
                loaded = LoadURL("form", "id", "frmPurchaseQuotationRFQ", true);
                if (!loaded) throw new Exception("Unable to load web page");
            }
        }

        private void SendQuote(MTMLInterchange _interchange, string orgURL, string orgRFQURL, string QuoteRefNo, double FinalAmt, double extraItemCost, FileInfo xmlFile, string BuyerCode, string VendorCode)
        {
            try
            {
                LoadSite(orgURL, orgRFQURL);

                #region // Check Send Quote Button //
                HtmlAgilityPack.HtmlNode lnkSendQuote = null;
                HtmlAgilityPack.HtmlNode mnu = _httpWrapper._CurrentDocument.GetElementbyId("MenuQuotationLineItem_dlstTabs");
                if (mnu != null)
                {
                    HtmlAgilityPack.HtmlNodeCollection coll = mnu.SelectNodes("ul/li");
                    foreach (HtmlAgilityPack.HtmlNode n in coll)
                    {
                        if (n.SelectSingleNode("span") != null && n.SelectSingleNode("span").GetAttributeValue("title", "") == "Send Quote")
                        {
                            lnkSendQuote = n;
                            break;
                        }
                    }
                }
                #endregion

                if (lnkSendQuote != null)
                {
                    if (extraItemCost > 0) FinalAmt = FinalAmt - extraItemCost;

                    double totalPrice = 0;
                    double webPrice = 0;
                    HtmlAgilityPack.HtmlNode txtTotalPrice = _httpWrapper._CurrentDocument.GetElementbyId("txtTotalPrice");
                    if (txtTotalPrice != null) totalPrice = Math.Round(convert.ToDouble(txtTotalPrice.GetAttributeValue("Value", "0")), 2);
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
                            LeSDM.AddConsoleLog("Sending Quote '" + this.VRNO + "' ");

                            #region // Send TO EMS //
                            dctPostDataValues.Clear();
                            AddFixedHeaderFieldsForHeader();

                            dctPostDataValues.Add("__EVENTTARGET", "MenuQuotationLineItem%24dlstTabs%24ctl00%24btnMenu");

                            /* Price  without charges */
                            double price = convert.ToDouble(_httpWrapper._CurrentDocument.GetElementbyId("txtPrice").GetAttributeValue("Value", "0"));
                            dctPostDataValues.Add("txtPrice", price.ToString("0.00"));

                            /* Total Price */
                            double _totalPrice = convert.ToDouble(_httpWrapper._CurrentDocument.GetElementbyId("txtTotalPrice").GetAttributeValue("Value", "0"));
                            dctPostDataValues.Add("txtTotalPrice", _totalPrice.ToString("0.00"));

                            bool sentQuote = PostData("span", "id", "lbltext", true);
                            #endregion

                            if (sentQuote)
                            {
                                if (convert.ToString(_httpWrapper._CurrentDocument.DocumentNode.InnerText).Contains(QuoteRefNo.Trim()))
                                {
                                    //"Your bid " + QuoteRefNo.Trim() + " is registered and confirmed in our database"

                                    #region // Success - Quote Submitted Successfully //
                                    //LoadSite(orgURL, orgRFQURL);
                                    //LoadAllItems(_interchange.DocumentHeader.LineItemCount);
                                    string quoteSubImg = this.ImagePath + "\\PhoenixTelerik_QuoteSubmitted_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".png";
                                    if (SavePage(quoteSubImg)) quoteSubImg = Path.GetFileName(quoteSubImg);
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
                                    #region // Error While Sending Quote //
                                    // Get Error Screen Shot //
                                    string errImg = this.ImagePath + "\\PhoenixTelerik_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".png";
                                    if (!SavePage(errImg)) errImg = xmlFile.Name;
                                    else errImg = Path.GetFileName(errImg);

                                    LeSDM.AddConsoleLog("Unable to send quote for '" + VRNO + "'.");
                                    MoveFile(xmlFile.Directory.FullName + "Error_Files", xmlFile);
                                    LeSDM.SetAuditLog(this.Module, errImg, VRNO.Trim(), "Error", "Unable to send quote for '" + VRNO + "'.", BuyerCode, SuppCode, Server, Processor);
                                    this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                                    #endregion
                                }
                            }
                            else
                            {
                                #region // Check Send Quote Link //
                                lnkSendQuote = null;
                                mnu = _httpWrapper._CurrentDocument.GetElementbyId("MenuQuotationLineItem_dlstTabs");
                                if (mnu != null)
                                {
                                    HtmlAgilityPack.HtmlNodeCollection coll = mnu.SelectNodes("ul/li");
                                    foreach (HtmlAgilityPack.HtmlNode n in coll)
                                    {
                                        if (n.SelectSingleNode("span") != null && n.SelectSingleNode("span").GetAttributeValue("title", "") == "Send Quote")
                                        {
                                            lnkSendQuote = n;
                                        }
                                    }
                                }
                                #endregion

                                if (lnkSendQuote == null)
                                {
                                    #region // Send log for Quote Submitted //
                                    string quoteSubImg = this.ImagePath + "\\PhoenixTelerik_QuoteSubmitted_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".png";
                                    if (SavePage(quoteSubImg)) quoteSubImg = Path.GetFileName(quoteSubImg);
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
                                    #region // Error While Sending Quote //
                                    // Get Error Msg //
                                    string ErrorMsg = "";
                                    HtmlAgilityPack.HtmlNode errorNode = _httpWrapper._CurrentDocument.GetElementbyId("ucError_spnErrorMessage");
                                    if (errorNode != null)
                                    {
                                        ErrorMsg = convert.ToString(errorNode.InnerText);
                                    }

                                    // Get Error Screen Shot //
                                    string errImg = this.ImagePath + "\\PhoenixTelerik_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".png";
                                    if (!SavePage(errImg)) errImg = xmlFile.Name;
                                    else errImg = Path.GetFileName(errImg);

                                    LeSDM.AddConsoleLog("Unable to send quote for '" + VRNO + "'. " + ErrorMsg.Trim());
                                    MoveFile(xmlFile.Directory.FullName + "Error_Files", xmlFile);
                                    LeSDM.SetAuditLog(this.Module, errImg, VRNO.Trim(), "Error", "Unable to send quote for '" + VRNO + "'. " + ErrorMsg.Trim(), BuyerCode, SuppCode, Server, Processor);
                                    this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            #region // Error - Quote Total mismatched //
                            string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".png";
                            if (SavePage(errImg)) errImg = Path.GetFileName(errImg);
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
                        string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".png";
                        if (SavePage(errImg)) errImg = Path.GetFileName(errImg);
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
                    #region // Quote Already Submitted //
                    string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".png";
                    if (SavePage(errImg)) errImg = Path.GetFileName(errImg);
                    else errImg = "";

                    LeSDM.AddConsoleLog("Send Quote button not found on webpage for '" + VRNO + "'. ");
                    LeSDM.AddConsoleLog("Quote is already submitted.");

                    LeSDM.SetAuditLog(this.Module, errImg, VRNO.Trim(), "Error", "Unable to save quote for Ref '" + this.VRNO + "'; Save button not found on webpage.", BuyerCode, SuppCode, Server, Processor);
                    MoveFile(xmlFile.Directory.FullName + "\\Error_Files", xmlFile);
                    this.SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region // Quote Exception //
                string errImg = this.ImagePath + "\\Phoenix_QuoteError_" + convert.ToFileName(VRNO) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".png";
                if (SavePage(errImg)) errImg = Path.GetFileName(errImg);
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
                    SendTextNotification(VRNO.Trim(), QuoteRef, "FAIL", SuppCode);
                }
                #endregion
            }
        }

        private string GetCurrencyCode(string code)
        {
            string codeValue = "";
            HtmlAgilityPack.HtmlNode selCurrency = _httpWrapper._CurrentDocument.GetElementbyId("ucCurrency_ddlCurrency");

            if (selCurrency != null)
            {
                HtmlAgilityPack.HtmlNodeCollection coll = selCurrency.ChildNodes;
                for (int i = 0; i < coll.Count; i++)
                {
                    HtmlAgilityPack.HtmlNode node = coll[i];
                    if (convert.ToString(node.InnerText).Trim().ToUpper() == code.ToUpper())
                    {
                        codeValue = coll[i - 1].GetAttributeValue("Value", "");
                        break;
                    }
                }
            }
            else throw new Exception("Currency Field not found");
            return codeValue;
        }

        private bool PostData(string validateNodeType, string validateAttribute, string attributeValue, bool _readResponse = true, string PostData = "")
        {
            bool _result = false;

            string _postdata = "";
            if (PostData.Trim().Length > 0) _postdata = PostData;
            else _postdata = GetPostData();

            _result = _httpWrapper.PostURL(URL, _postdata, validateNodeType, validateAttribute, attributeValue, HiddenAttributeKey);
            _CurrentResponseString = _httpWrapper._CurrentResponseString;
            return _result;
        }

        private bool PostDataWithMultipart(string URL, string postData, string validateNodeTitle, string validateAttribute, string attributeValue)
        {
            bool success = false;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);

            //Set request headers.
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-IN");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
            request.ContentType = "multipart/form-data; boundary=---------------------------7e43a1110632"; // fixed
            request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.Headers.Add("DNT", @"1");
            request.Headers.Set(HttpRequestHeader.CacheControl, "no-cache");
            request.Headers.Set(HttpRequestHeader.Cookie, @"ASP.NET_SessionId=" + _httpWrapper.SessionID);

            //Set request method
            request.Method = "POST";

            WriteMultipartBodyToRequest(request, postData);
            response = (HttpWebResponse)request.GetResponse();

            _httpWrapper._CurrentDocument.LoadHtml(_httpWrapper.ReadResponse(response));

            _httpWrapper._dctStateData = _httpWrapper._http.GetStateInfo(_httpWrapper._CurrentDocument);
            if (validateNodeTitle != "")
            {
                HtmlNode _node = _httpWrapper._CurrentDocument.DocumentNode.SelectSingleNode("//" + validateNodeTitle + "[@" + validateAttribute.ToString() + "='" + attributeValue + "']");
                success = (_node != null);
            }
            else success = true;

            return success;
        }

        private static void WriteMultipartBodyToRequest(HttpWebRequest request, string body)
        {
            string[] multiparts = Regex.Split(body, @"<!>");
            byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                foreach (string part in multiparts)
                {
                    //Determine if part is plain text or "<!>" line.
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

                    LeSDM.AddConsoleLog("" + fileinfo.Name + " File moved to " + DestinationPath);
                }
                else throw new Exception("Destination path is blank.");
            }
            catch (Exception ex)
            {
                LeSDM.AddConsoleLog("Error in MoveFile() - " + ex.Message);
            }
        }

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

        private bool SavePage(string saveAsFile)
        {
            string outerHTML = _httpWrapper._CurrentDocument.DocumentNode.OuterHtml;

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "HTML"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "HTML");

            string htmlFile = AppDomain.CurrentDomain.BaseDirectory + "HTML\\" + Path.GetFileNameWithoutExtension(saveAsFile) + ".htm";
            File.WriteAllText(htmlFile, outerHTML);
            if (File.Exists(htmlFile))
            {
                using (System.Windows.Forms.WebBrowser wb = new System.Windows.Forms.WebBrowser())
                {
                    wb.Width = 1024;
                    wb.ScrollBarsEnabled = false;
                    wb.ScriptErrorsSuppressed = true;
                    wb.Navigate(htmlFile);
                    while (wb.ReadyState != System.Windows.Forms.WebBrowserReadyState.Complete) System.Windows.Forms.Application.DoEvents();
                    wb.Height = wb.Document.Body.ScrollRectangle.Size.Height;
                    using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(wb.Width, wb.Height))
                    {
                        wb.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, wb.Width, wb.Height));
                        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, wb.Width, wb.Height);
                        System.Drawing.Bitmap cropped = bitmap.Clone(rect, bitmap.PixelFormat);
                        cropped.Save(saveAsFile, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
                if (File.Exists(saveAsFile))
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }
    }
}
