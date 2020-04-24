using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using LeSCommon;
using System.IO;

namespace Http_Phoenix_Routine
{
    public class PhoenixPDFRoutinecs
    {
        public Dictionary<string, string> GetOrderHeader(RichTextBox txtData,string MsgFile)
        {
            Dictionary<string, string> _xmlHeader = new Dictionary<string, string>();
            try
            {
                _xmlHeader.Add("HEADER_REMARKS", "");
                _xmlHeader.Add("YARD", "");
                _xmlHeader.Add("HULLNO", "");
                _xmlHeader.Add("YEARBUILT", "");
                _xmlHeader.Add("LEAD_DAYS", "");

                #region // Set Buyer Name & Address //
                _xmlHeader.Add("BUYER_NAME", txtData.Lines[0].Trim());
                _xmlHeader.Add("BUYER_ADDR", txtData.Lines[1].Trim() + ", " + txtData.Lines[2].Trim());
                #endregion

                #region // Get VRNO & Supplier Name //
                string VRNO = "", SuppName = "";
                int indTo = txtData.Text.ToUpper().IndexOf("TO ");
                if (indTo > 0)
                {
                    int lnTo = txtData.GetLineFromCharIndex(indTo);
                    if (lnTo > 0)
                    {
                        string line = "";
                        if (txtData.Lines[lnTo].Contains("Purchase") && txtData.Lines[lnTo].Contains("Order"))
                        {
                            line = Regex.Replace(txtData.Lines[lnTo], @"\s+", "|");
                            string[] values = line.Split('|');
                            if (values.Length > 0)
                            {
                                VRNO = values[values.Length - 1].Trim();
                                if (VRNO.Length < 10)
                                {
                                    VRNO = values[values.Length - 2].Trim().Trim(')') + VRNO.Trim();
                                    string tempVRNO = "";

                                    // Get VRNO From Link File
                                    string[] content = File.ReadAllLines(MsgFile);
                                    foreach (string _fLine in content)
                                    {
                                        if (_fLine.Trim().StartsWith("Purchase Order:"))
                                        {
                                            tempVRNO = _fLine.Split(':')[1].Trim();
                                            break;
                                        }
                                    }

                                    if (tempVRNO.Trim() != VRNO.Trim())
                                    {
                                        throw new Exception("Unable to read purchase order number from PDF file");
                                    }
                                }
                            }
                        }

                        line = txtData.Lines[lnTo];
                        if (Regex.IsMatch(line.Trim(), @"To\s+Purchase"))
                        {
                            string line1 = txtData.Lines[lnTo - 1].Trim();
                            string line2 = txtData.Lines[lnTo + 1].Trim();
                            SuppName = line1 + line2;
                            int spltIndx = SuppName.ToUpper().IndexOf("AS");
                            if (spltIndx > 0) SuppName = SuppName.Substring(0, spltIndx + 2).Trim();
                        }
                        else
                        {
                            int indPurchase = line.IndexOf("Purchase");
                            if (indPurchase > 0)
                            {
                                SuppName = line.Substring(0, indPurchase).Trim();
                                SuppName = SuppName.Replace("To ", "").Replace("TO ", "").Trim();

                                int spltIndx = SuppName.ToUpper().IndexOf("AS");
                                if (spltIndx > 0) SuppName = SuppName.Substring(0, spltIndx + 2).Trim();
                            }
                        }
                    }
                }
                if (Regex.IsMatch(VRNO, @"([A-Z]{3,4}[-][\d]{2,}-[\d]{4}[-a-z\d]{0,})"))
                {
                    _xmlHeader.Add("VRNO", VRNO.Trim());
                }
                else throw new Exception("Invalid VRNO");

                _xmlHeader.Add("VENDOR_NAME", SuppName.Trim());
                #endregion

                #region // Vessel //
                _xmlHeader.Add("VESSEL", "");
                int indVessel = txtData.Text.IndexOf("Vessel Name");
                if (indVessel > 0)
                {
                    int lnVessel = txtData.GetLineFromCharIndex(indVessel);
                    if (lnVessel > 0)
                    {
                        string line = txtData.Lines[lnVessel];
                        if (line.Length > 70)
                        {
                            _xmlHeader["VESSEL"] = line.Substring(14, line.IndexOf("Building Yard") - 14).Trim();
                        }
                        else if (line.Length > 14)
                        {
                            _xmlHeader["VESSEL"] = line.Substring(14).Trim();
                        }
                    }
                }
                else throw new Exception("Unable to get 'Vessel Name' field");
                #endregion

                #region // IMO //
                _xmlHeader.Add("IMO", "");
                int indIMO = txtData.Text.IndexOf("IMO NO");
                if (indIMO > 0)
                {
                    int lnIMO = txtData.GetLineFromCharIndex(indIMO);
                    if (lnIMO > 0)
                    {
                        string line = txtData.Lines[lnIMO];
                        if (line.Length > 70)
                        {
                            _xmlHeader["IMO"] = line.Substring(14, line.IndexOf("Year Built") - 14).Trim();
                        }
                        else if (line.Length > 14)
                        {
                            _xmlHeader["IMO"] = line.Substring(14).Trim();
                        }
                    }
                }
                #endregion

                #region // Quote Ref //
                _xmlHeader.Add("QUOTE_REF", "");
                string header = "Quotation Reference";
                int indQtRef = txtData.Text.IndexOf(header);
                if (indQtRef == -1)
                {
                    header = "Quatation Reference";
                    indQtRef = txtData.Text.IndexOf(header);
                }
                if (indQtRef > 0)
                {
                    int lnQtRef = txtData.GetLineFromCharIndex(indQtRef);
                    if (lnQtRef > 0)
                    {
                        string line = txtData.Lines[lnQtRef];
                        indQtRef = line.IndexOf("Reference");
                        _xmlHeader["QUOTE_REF"] = line.Substring(indQtRef + "Reference".Length).Trim();
                    }
                    else
                    {
                        throw new Exception("Unabel to get line index of 'Quotation Reference' ");
                    }
                }
                else throw new Exception("Quotation Reference not found in pdf file");
                #endregion

                #region  // Vendor Address //
                _xmlHeader.Add("VENDOR_ADDRESS1", "");
                int indVenAddr = txtData.Text.IndexOf("Address");
                if (indVenAddr > 0)
                {
                    int lnAddr = txtData.GetLineFromCharIndex(indVenAddr);
                    if (lnAddr > 0)
                    {
                        string addr = "";
                        string line = txtData.Lines[lnAddr];
                        if (line.Length > 9)
                            if (line.Length > 65) addr = line.Substring(9, line.IndexOf("  Qu") - 10).Trim();
                            else addr = line.Substring(9).Trim();

                        line = txtData.Lines[lnAddr + 1];
                        if (line.Length > 9 && line.Length < 70)
                            if (line.Length > 65) addr += ", " + line.Substring(9, 65).Trim();
                            else addr += ", " + line.Substring(9).Trim();
                        else if (line.Length > 9)
                            addr += ", " + line.Substring(9).Trim();

                        _xmlHeader["VENDOR_ADDRESS1"] = addr.Trim();
                    }
                }
                #endregion

                #region // Vendor Phone //
                _xmlHeader.Add("VENDOR_PHONE", "");
                int indPhone = txtData.Text.IndexOf("Phone");
                if (indPhone > 0)
                {
                    int lnPhone = txtData.GetLineFromCharIndex(indPhone);
                    if (lnPhone > 0)
                    {
                        string line = txtData.Lines[lnPhone];
                        if (line.Length > 9 && line.Length > 65)
                        {
                            _xmlHeader["VENDOR_PHONE"] = line.Substring(9, line.IndexOf("Delivery") - 10).Trim().Trim(',');
                        }
                        else if (line.Length > 9) _xmlHeader["VENDOR_PHONE"] = line.Substring(9).Trim().Trim(',');
                    }
                }
                #endregion

                #region // Vendor Fax //
                _xmlHeader.Add("VENDOR_FAX", "");
                int indVenFax = txtData.Text.IndexOf("Fax");
                if (indVenFax > 0)
                {
                    int lnFax = txtData.GetLineFromCharIndex(indVenFax);
                    if (lnFax > 0)
                    {
                        string line = txtData.Lines[lnFax];
                        if (line.Length > 9 && line.Length > 70)
                        {
                            _xmlHeader["VENDOR_FAX"] = line.Substring(9, line.IndexOf("Port") - 10).Trim().Trim(',');
                        }
                        else if (line.Length > 9) _xmlHeader["VENDOR_FAX"] = line.Substring(9).Trim().Trim(',');
                    }
                }
                #endregion

                #region // Vendor Email //
                _xmlHeader.Add("VENDOR_EMAIL", "");
                int indMail = txtData.Text.IndexOf("Mail");
                if (indMail > 0)
                {
                    int lnMail = txtData.GetLineFromCharIndex(indMail);
                    if (lnMail > 0)
                    {
                        string line = txtData.Lines[lnMail];
                        if (Regex.IsMatch(line.Trim(), @"Mail\s+ETA"))
                        {
                            string line1 = txtData.Lines[lnMail - 1].Trim();
                            string line2 = txtData.Lines[lnMail + 1].Trim();
                            string email = line1 + line2;
                            _xmlHeader["VENDOR_EMAIL"] = email.Trim();
                        }
                        else
                        {
                            int inxETA = line.IndexOf("ETA");
                            if (inxETA > 0)
                            {
                                _xmlHeader["VENDOR_EMAIL"] = line.Substring(9, inxETA - 10).Trim();
                            }
                        }
                    }
                }
                #endregion

                #region // Order Date //
                _xmlHeader.Add("ORDER_DATE", "");
                int indOrdDate = txtData.Text.IndexOf("Date");
                if (indOrdDate > 0)
                {
                    int lnOrderDt = txtData.GetLineFromCharIndex(indOrdDate);
                    if (lnOrderDt > 0)
                    {
                        indOrdDate = txtData.Lines[lnOrderDt].IndexOf("Date");
                        if (indOrdDate > 0)
                        {
                            _xmlHeader["ORDER_DATE"] = txtData.Lines[lnOrderDt].Substring(indOrdDate + "Date".Length).Trim();
                        }
                    }
                }
                if (_xmlHeader["ORDER_DATE"].Length == 0) _xmlHeader["ORDER_DATE"] = DateTime.Now.ToString("dd/MMM/yyyy");
                #endregion

                #region // Payterms //
                _xmlHeader.Add("PAY_TERMS", "");
                int indPayTerms = txtData.Text.IndexOf("Payment Terms");
                if (indPayTerms > 0)
                {
                    int lnPayTerm = txtData.GetLineFromCharIndex(indPayTerms);
                    if (lnPayTerm > 0)
                    {
                        indPayTerms = txtData.Lines[lnPayTerm].IndexOf("Payment Terms");
                        if (indPayTerms > 0)
                        {
                            _xmlHeader["PAY_TERMS"] = txtData.Lines[lnPayTerm].Substring(indPayTerms + "Payment Terms".Length).Trim();
                        }
                    }
                }
                #endregion

                #region // Delivery Terms //
                _xmlHeader.Add("DEL_TERMS", "");
                int indDelterms = txtData.Text.IndexOf("Delivery Terms");
                if (indDelterms > 0)
                {
                    int lnDelTrm = txtData.GetLineFromCharIndex(indDelterms);
                    if (lnDelTrm > 0)
                    {
                        indDelterms = txtData.Lines[lnDelTrm].IndexOf("Delivery Terms");
                        _xmlHeader["DEL_TERMS"] = txtData.Lines[lnDelTrm].Substring(indDelterms + "Delivery Terms".Length).Trim();
                    }
                }
                #endregion

                #region // PORT //
                _xmlHeader.Add("PORT_NAME", "");
                int indPort = txtData.Text.IndexOf("Port");
                if (indPort > 0)
                {
                    int lnPort = txtData.GetLineFromCharIndex(indPort);
                    if (lnPort > 0)
                    {
                        indPort = txtData.Lines[lnPort].IndexOf("Port");
                        _xmlHeader["PORT_NAME"] = txtData.Lines[lnPort].Substring(indPort + "Port".Length).Trim();
                    }
                }
                #endregion

                #region // ETA //
                _xmlHeader.Add("ETA", "");
                int indETA = txtData.Text.IndexOf("ETA");
                if (indETA > 0)
                {
                    int lnETA = txtData.GetLineFromCharIndex(indETA);
                    if (lnETA > 0)
                    {
                        indETA = txtData.Lines[lnETA].IndexOf("ETA");
                        _xmlHeader["ETA"] = txtData.Lines[lnETA].Substring(indETA + "ETA".Length).Trim();
                    }
                }
                #endregion

                #region // ETD //
                _xmlHeader.Add("ETD", "");
                int indETD = txtData.Text.IndexOf("ETD");
                if (indETD > 0)
                {
                    int lnETD = txtData.GetLineFromCharIndex(indETD);
                    if (lnETD > 0)
                    {
                        indETD = txtData.Lines[lnETD].IndexOf("ETD");
                        _xmlHeader["ETD"] = txtData.Lines[lnETD].Substring(indETD + "ETD".Length).Trim();
                    }
                }
                #endregion

                #region // DELIVERY ADDRESS //
                _xmlHeader.Add("SHIP_NAME", "");
                _xmlHeader.Add("SHIP_ADDRESS1", "");
                _xmlHeader.Add("SHIP_PHONE", "");
                _xmlHeader.Add("SHIP_EMAIL", "");

                int indDelAddr = txtData.Text.IndexOf("DELIVERY ADDRESS");
                if (indDelAddr > 0)
                {
                    int lnDelAddr = txtData.GetLineFromCharIndex(indDelAddr);
                    if (lnDelAddr > 0)
                    {
                        for (int i = lnDelAddr + 1; i < txtData.Lines.Length; i++)
                        {
                            string line = txtData.Lines[i];
                            if (line.Trim().StartsWith("Delivery Instruction")) break;
                            if (line.Trim().StartsWith("Deliver To"))
                            {
                                if (line.Length > 65) _xmlHeader["SHIP_NAME"] = line.Substring(11, 50).Trim();
                                else if (line.Length > 11) _xmlHeader["SHIP_NAME"] = line.Substring(11).Trim();
                            }
                            if (line.Trim().StartsWith("Phone"))
                            {
                                if (line.Length > 65) _xmlHeader["SHIP_PHONE"] = line.Substring(11, line.IndexOf("  Phone") - 12).Trim();
                                else if (line.Length > 11) _xmlHeader["SHIP_PHONE"] = line.Substring(11).Trim();
                            }
                            if (line.Trim().StartsWith("Mail"))
                            {
                                if (line.Length > 65) _xmlHeader["SHIP_EMAIL"] = line.Substring(11, line.IndexOf("  Mail") - 12).Trim();
                                else if (line.Length > 11) _xmlHeader["SHIP_EMAIL"] = line.Substring(11).Trim();
                            }
                            else if (line.Trim().StartsWith("Address"))
                            {
                                for (int k = i; k < txtData.Lines.Length; k++, i++)
                                {
                                    line = txtData.Lines[k];
                                    if (line.Trim().StartsWith("Phone")) { i--; break; }
                                    if (line.Length > 65) _xmlHeader["SHIP_ADDRESS1"] += "," + line.Substring(11, line.IndexOf("   Address") - 12).Trim();
                                    else if (line.Length > 11) _xmlHeader["SHIP_ADDRESS1"] += "," + line.Substring(11).Trim();
                                }
                            }
                        }
                    }
                }
                #endregion

                #region // Delivery Instruction //
                int indDelInst = txtData.Text.IndexOf("Delivery Instruction:");
                if (indDelInst > 0)
                {
                    int lnDelInst = txtData.GetLineFromCharIndex(indDelInst);
                    if (lnDelInst > 0)
                    {
                        for (int i = lnDelInst; i < txtData.Lines.Length; i++)
                        {
                            string line = txtData.Lines[i];
                            if (line.Trim().StartsWith("Vessel Name")) break;
                            _xmlHeader["HEADER_REMARKS"] += Environment.NewLine + line.Trim();
                        }
                    }
                }
                #endregion

                #region // Invoice Address //
                _xmlHeader.Add("BILL_NAME", "");
                _xmlHeader.Add("BILL_ADDRESS1", "");
                _xmlHeader.Add("BILL_CONTACT", "");
                _xmlHeader.Add("BILL_PHONE", "");
                _xmlHeader.Add("BILL_EMAIL", "");

                int indInvTo = txtData.Text.IndexOf("Please invoice the order");
                if (indInvTo > 0)
                {
                    int lnInvAddr = txtData.GetLineFromCharIndex(indInvTo);
                    if (lnInvAddr > 0)
                    {
                        for (int i = (lnInvAddr + 1); i < txtData.Lines.Length; i++)
                        {
                            if (txtData.Lines[i].Trim().Length > 0)
                            {
                                if (txtData.Lines[i].Trim().StartsWith("We prefer")) break;

                                if (txtData.Lines[i].Trim().StartsWith("Master"))
                                {
                                    string BillName = txtData.Lines[i].Trim();
                                    BillName = Regex.Replace(BillName, @"\s+", " ");
                                    if (BillName.Trim().Length > 0)
                                    {
                                        _xmlHeader["BILL_NAME"] = BillName.Trim();
                                    }
                                }
                                else
                                {
                                    _xmlHeader["BILL_ADDRESS1"] += "," + txtData.Lines[i].Trim();
                                }
                            }
                        }
                    }
                }

                _xmlHeader["BILL_ADDRESS1"] = _xmlHeader["BILL_ADDRESS1"].Trim().Trim(',').Trim();

                // Invoicee Contact
                int indInvCont = txtData.Text.IndexOf("For all clarification");
                if (indInvCont > 0)
                {
                    int lnContPer = txtData.GetLineFromCharIndex(indInvCont);
                    if (lnContPer > 0)
                    {
                        int indxContact = txtData.Lines[lnContPer].IndexOf(", please");
                        if (indxContact > 0)
                        {
                            _xmlHeader["BILL_CONTACT"] = txtData.Lines[lnContPer].Substring(indxContact + ", please".Length).Trim();
                        }
                    }
                }

                _xmlHeader["BILL_CONTACT"] = _xmlHeader["BILL_CONTACT"].Replace("contact", "").Trim();

                // Invoicee Phone
                int indInvTel = txtData.Text.IndexOf("at Telephone Number");
                if (indInvTel > 0)
                {
                    int lnInvTel = txtData.GetLineFromCharIndex(indInvTel);
                    if (lnInvTel > 0)
                    {
                        string line = txtData.Lines[lnInvTel];
                        if (line.Length > 30)
                        {
                            _xmlHeader["BILL_PHONE"] = line.Substring(25).Trim();
                        }
                    }
                }

                // Invoicee Email
                int indInvEmail = txtData.Text.IndexOf("at the Email ID");
                if (indInvEmail > 0)
                {
                    int lnInvEmail = txtData.GetLineFromCharIndex(indInvEmail);
                    if (lnInvEmail > 0)
                    {
                        string line = txtData.Lines[lnInvEmail];
                        if (line.Length > 30)
                        {
                            _xmlHeader["BILL_EMAIL"] = line.Substring(25).Trim().Replace("/", ";");
                        }
                    }
                }

                // Invoice Mail ID to Buyer Mail ID 
                _xmlHeader["BUYER_EMAIL"] = _xmlHeader["BILL_EMAIL"];

                #endregion

                #region // MAKER'S NAME/STATE-COUNTRY/FAX-TLX ETC //
                int indMakers = txtData.Text.IndexOf("MAKER'S NAME/STATE-COUNTRY/FAX-TLX ETC");
                if (indMakers > 0)
                {
                    int lnMakers = txtData.GetLineFromCharIndex(indMakers);
                    if (lnMakers > 0)
                    {
                        for (int i = lnMakers; i < txtData.Lines.Length; i++)
                        {
                            string line = txtData.Lines[i];
                            if (line.Trim().StartsWith("Item")) break;
                            _xmlHeader["HEADER_REMARKS"] += Environment.NewLine + line.Trim();
                        }
                    }
                }
                #endregion

                #region // Currency & Monetory Amounts //
                _xmlHeader.Add("CURRENCY", "");
                _xmlHeader.Add("ITEM_TOTAL", "");
                _xmlHeader.Add("TAX_COST", "");
                _xmlHeader.Add("GRANT_TOTAL", "");

                int indxItemTot = txtData.Text.IndexOf("Line Item Sub Total");
                if (indxItemTot > 0)
                {
                    int lnItemTotal = txtData.GetLineFromCharIndex(indxItemTot);
                    if (lnItemTotal > 0)
                    {
                        indxItemTot = txtData.Lines[lnItemTotal].IndexOf("Line Item Sub Total");
                        if (indxItemTot > 0)
                        {
                            string itemTotal = txtData.Lines[lnItemTotal].Substring(indxItemTot + "Line Item Sub Total".Length);
                            itemTotal = itemTotal.Trim(':').Trim();

                            double _itemTotal = LeSCommon.LeSCommon.convert.ToDouble(itemTotal.Trim());
                            if (_itemTotal > 0)
                            {
                                _xmlHeader["ITEM_TOTAL"] = _itemTotal.ToString();
                            }
                        }
                    }
                }
                else throw new Exception("Field 'Line Item Sub Total' not found");

                int indxTaxCost = txtData.Text.IndexOf("Tax and Charges");
                if (indxTaxCost > 0)
                {
                    int lnTaxCost = txtData.GetLineFromCharIndex(indxTaxCost);
                    if (lnTaxCost > 0)
                    {
                        indxTaxCost = txtData.Lines[lnTaxCost].IndexOf("Tax and Charges");
                        if (indxTaxCost > 0)
                        {
                            string taxCost = txtData.Lines[lnTaxCost].Substring(indxTaxCost + "Tax and Charges".Length);
                            taxCost = taxCost.Trim(':').Trim();

                            double _taxCost = LeSCommon.LeSCommon.convert.ToDouble(taxCost.Trim());
                            if (_taxCost > 0)
                            {
                                _xmlHeader["TAX_COST"] = _taxCost.ToString();
                            }
                        }
                    }
                }
                else throw new Exception("Field 'Tax and Charges' not found");

                int indxGrantTotal = txtData.Text.IndexOf("Total for this Order");
                if (indxGrantTotal > 0)
                {
                    int lnGrantTot = txtData.GetLineFromCharIndex(indxGrantTotal);
                    if (lnGrantTot > 0)
                    {
                        indxGrantTotal = txtData.Lines[lnGrantTot].IndexOf("Total for this Order");
                        if (indxGrantTotal > 0)
                        {
                              string grandTotal="";
                            if(indxTaxCost==0)//added on 11-2-19 as it is throwing an error
                             grandTotal = txtData.Lines[lnGrantTot].Trim().Substring(indxTaxCost + "Total for this Order".Length);
                            else
                                grandTotal = txtData.Lines[lnGrantTot].Trim().Substring("Total for this Order".Length);
                            grandTotal = grandTotal.Trim(':').Trim();

                            double _grdtotal = LeSCommon.LeSCommon.convert.ToDouble(grandTotal.Trim());
                            if (_grdtotal > 0)
                            {
                                _xmlHeader["GRANT_TOTAL"] = _grdtotal.ToString();
                            }
                        }
                    }
                }
                else throw new Exception("Field 'Total for this Order' not found");
                #endregion

                _xmlHeader["HEADER_REMARKS"] = _xmlHeader["HEADER_REMARKS"].Replace("±", "-");                

                return _xmlHeader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<List<string>> GetOrderItems(RichTextBox txtData)
        {
            List<List<string>> _lineItems = new List<List<string>>();int nItemRmks = 0;
            try
            {
              //  string ItemStringPattern = @"([\d]{1})\s+(.+)\s+Name[\s]{0,}[:]";
                string ItemStringPattern= @"([\d]{1})\s+(.+)\s+\s\sName[\s]{0,}[:]";//changed by kalpita on 15/01/2020
                string ItemRefStringPattern = @"([\d]{1})\s+(.+)\s+Maker Ref[\s]{0,}[:]";
                double CalculatedTotal = 0;
                int itemStartLine = 0, descrLineCounter = 0;

                int indxItem = txtData.Text.IndexOf("Item");
                if (indxItem > 0)
                {
                    itemStartLine = txtData.GetLineFromCharIndex(indxItem);
                }

                if (itemStartLine > 0)
                {
                    itemStartLine = itemStartLine + 1;
                    List<string> itemData = new List<string>();
                    for (int i = itemStartLine; i < txtData.Lines.Length; i++)
                    {
                        string line = txtData.Lines[i];

                        if (line.Trim().Contains("Line Item Sub Total"))
                        {
                            if (itemData.Count > 0) _lineItems.Add(itemData);
                            break;
                        }

                       if (line.Trim().StartsWith("PHOENIX 2.0")) continue;

                        string itemNum = line.Trim().Split(' ')[0].Trim();
                        bool IsNumeric = Regex.IsMatch(line.Trim().Split(' ')[0].Trim(), @"^\d+$");

                        if (Regex.IsMatch(line.Trim(), ItemStringPattern))
                        {
                            if (itemData.Count > 0)
                            {
                                _lineItems.Add(itemData);
                                descrLineCounter = 0;
                            }
                            itemData = new List<string>();
                            string qtyUomRegex = @"\s\s\s([\d.]{1,})\s+([A-Za-z\d]{2,})\s+([\d.,]{2,})\s+([A-Z]{3})\s+([\d.]{0,4})\s+([\d.]{1,})";
                            string qtyUomRegex1 = @"\s\s\s([\d.]{1,})\s+([A-Za-z\d]{2,})\s+([A-Z]{3})\s+([\d.]{0,4})\s+([\d.]{1,})";
                            string qtyUOMREgex2 = @"\s\s\s([\d.]{1,})\s+([A-Za-z\d]{2,})";
                            if (IsNumeric)
                            {
                                string PartNum = line.Substring(line.IndexOf(itemNum) + itemNum.Length + 1, line.IndexOf("Name") - 5).Trim();
                                string ItemDesc = "", ItemQty = "", ItemUom = "", ItemPrice = "", ItemCurr = "", ItemTotal = "", ItemDisc = "";

                                line = line.Substring(line.IndexOf("Name") + 4);
                                Match matchedString = Regex.Match(line.Trim(), qtyUomRegex);
                                Match matchedString1 = Regex.Match(line.Trim(), qtyUomRegex1);
                                Match matchedString2 = Regex.Match(line.Trim(), qtyUOMREgex2);

                                if (matchedString.Success)       //    matchedString                   
                                {
                                    int itemDescrEndCol = line.IndexOf(matchedString.Value) - 1;
                                    int grpCount = matchedString.Groups.Count;

                                    if (grpCount == 7) // Discount is Missing
                                    {
                                        ItemQty = LeSCommon.LeSCommon.convert.ToDouble(matchedString.Groups[1].Value).ToString();
                                        ItemUom = matchedString.Groups[2].Value;
                                        ItemPrice = LeSCommon.LeSCommon.convert.ToDouble(matchedString.Groups[3].Value).ToString();
                                        ItemCurr = matchedString.Groups[4].Value;
                                        ItemDisc = LeSCommon.LeSCommon.convert.ToDouble(matchedString.Groups[5].Value).ToString();
                                        ItemTotal = LeSCommon.LeSCommon.convert.ToDouble(matchedString.Groups[6].Value).ToString();

                                        // UPDATED ON 31-JAN-19
                                        if (LeSCommon.LeSCommon.convert.ToDouble(ItemTotal) == 0 && LeSCommon.LeSCommon.convert.ToDouble(ItemPrice) > 0)
                                        {
                                            // Search price & Item total in above line                                        
                                            if (txtData.Lines[i - 1].Length > 100) ItemTotal = LeSCommon.LeSCommon.convert.ToDouble(txtData.Lines[i - 1].Substring(100).Trim()).ToString();
                                        }
                                    }

                                    // Set item description //
                                    ItemDesc += " " + line.Substring(0, itemDescrEndCol).Trim().Trim(':').Trim();
                                    CalculatedTotal = CalculatedTotal + Convert.ToDouble(ItemTotal);

                                    itemData.Add(itemNum); //0
                                    itemData.Add(PartNum.Trim()); //1
                                    itemData.Add(ItemDesc.Trim()); //2
                                    itemData.Add(ItemQty); //3
                                    itemData.Add(ItemUom); //4
                                    itemData.Add(ItemPrice); //5
                                    itemData.Add(ItemDisc); //6
                                    itemData.Add(""); //7
                                    itemData.Add(""); //8
                                    itemData.Add(""); //9
                                    itemData.Add(""); //10 Remarks                              
                                    itemData.Add(ItemCurr.Trim()); //11 Currency 

                                    descrLineCounter++;
                                }
                                else if (matchedString1.Success)
                                {
                                    int itemDescrEndCol = line.IndexOf(matchedString1.Value) - 1;
                                    int grpCount = matchedString1.Groups.Count;

                                    if (grpCount == 6) // Discount is Missing
                                    {
                                        // UPDATED ON 31-JAN-19
                                        string itemDetail = "";
                                        if (ItemDesc.Length > 60)
                                        {
                                            itemDetail = ItemDesc.Substring(60).Trim();
                                            ItemDesc = ItemDesc.Substring(0, 60);
                                        }
                                        itemDetail = Regex.Replace(itemDetail.Trim(), @"\s+", " ");
                                        string[] values = itemDetail.Split(' ');

                                        ItemQty = LeSCommon.LeSCommon.convert.ToDouble(matchedString1.Groups[1].Value).ToString();
                                        ItemUom = matchedString1.Groups[2].Value;
                                        int count = ItemDesc.Split(' ').Length;
                                        if (values.Length > 0)
                                        {
                                            ItemPrice = values[0]; if (ItemPrice == "") { ItemPrice = (matchedString1.Groups[5].Length > 0) ? Convert.ToString(matchedString1.Groups[5].Value) : ""; }//changed by kalpita on 26/06/2019

                                        }//ItemDesc.Split(' ')[count - 1];//LeSCommon.LeSCommon.convert.ToDouble(matchedString1.Groups[3].Value).ToString();                                   
                                        ItemCurr = matchedString1.Groups[3].Value;
                                        ItemDisc = LeSCommon.LeSCommon.convert.ToDouble(matchedString1.Groups[4].Value).ToString();
                                        ItemTotal = LeSCommon.LeSCommon.convert.ToDouble(matchedString1.Groups[5].Value).ToString();

                                        // UPDATED ON 31-JAN-19
                                        if (LeSCommon.LeSCommon.convert.ToDouble(ItemTotal) == 0 && values.Length > 1)
                                        {
                                            // Search price & Item total in above line
                                            ItemTotal = LeSCommon.LeSCommon.convert.ToDouble(values[1]).ToString();
                                        }
                                    }
                                  
                                    // Set item description //
                                    ItemDesc += " " + line.Substring(0, itemDescrEndCol).Trim().Trim(':').Trim();
                                    CalculatedTotal = CalculatedTotal + Convert.ToDouble(ItemTotal);

                                    itemData.Add(itemNum); //0
                                    itemData.Add(PartNum.Trim()); //1
                                    itemData.Add(ItemDesc.Trim()); //2
                                    itemData.Add(ItemQty); //3
                                    itemData.Add(ItemUom); //4
                                    itemData.Add(ItemPrice); //5
                                    itemData.Add(ItemDisc); //6
                                    itemData.Add(""); //7
                                    itemData.Add(""); //8
                                    itemData.Add(""); //9
                                    itemData.Add(""); //10 Remarks                              
                                    itemData.Add(ItemCurr.Trim()); //11 Currency 

                                    descrLineCounter++;
                                }                              
                               else throw new Exception("Unable to read item data for item number -" + itemNum);
                            }
                        }
                        else
                        {
                            // string nextLine = txtData.Lines[i + 1].Trim(); string nextsecLine = txtData.Lines[i + 2].Trim();
                            if (!string.IsNullOrEmpty(line))
                            {
                                string cPrevLine = txtData.Lines[i - 1].Trim();
                                if (!Regex.IsMatch(line, ItemStringPattern) && itemData.Count > 0 && line.Trim().Length > 00)
                                {
                                    if (descrLineCounter < 3)
                                    {
                                        // append to existing item
                                        itemData[2] = (itemData[2].Trim() + " " + line.Trim()).Trim();
                                        descrLineCounter++;
                                    }
                                    else
                                    {
                                        // itemData[10] = (itemData[10].Trim() + " " + line.Trim()).Trim();
                                    }
                                }

                                if (line.Contains("Product Code"))
                                {
                                    string cNxtLine = txtData.Lines[i + 2].Trim();
                                    itemData[10] = (itemData[10].Trim() + " " + cNxtLine.Trim()).Trim();
                                    nItemRmks += 2;
                                }
                            }
                        }
                    }
                }

                // Get Line Item Total //
                double _itemTotal = 0;
                int indxItemTot = txtData.Text.IndexOf("Line Item Sub Total");
                if (indxItemTot > 0)
                {
                    int lnItemTotal = txtData.GetLineFromCharIndex(indxItemTot);
                    if (lnItemTotal > 0)
                    {
                        indxItemTot = txtData.Lines[lnItemTotal].IndexOf("Line Item Sub Total");
                        if (indxItemTot > 0)
                        {
                            string itemTotal = txtData.Lines[lnItemTotal].Substring(indxItemTot + "Line Item Sub Total".Length).Trim().Trim(':').Trim();
                            _itemTotal = LeSCommon.LeSCommon.convert.ToDouble(itemTotal.Trim());
                        }
                    }
                }
                else throw new Exception("Field 'Line Item Sub Total' not found while getting items");

                if (SplitDecimal(_itemTotal.ToString()) != SplitDecimal(CalculatedTotal.ToString())) // Added by sayak on 16-07-2019
                {
                    throw new Exception("Item Total mismatched with PDF Item Total");
                }

                // Get Total Item Count //
                //MatchCollection itemMatched = Regex.Matches(itemContent, ItemStringPattern);
                string itemContent = txtData.Text.Substring(indxItem, txtData.Text.IndexOf("Line Item Sub Total") - indxItem);

                MatchCollection itemMatched1 = Regex.Matches(itemContent, ItemStringPattern);
                MatchCollection itemMatched2 = Regex.Matches(itemContent, @"([\d]{1})+\s+(.\d\.)");
                MatchCollection itemMatched3 = Regex.Matches(itemContent, ItemRefStringPattern);
                MatchCollection itemMatched = null;
                //if (itemMatched1.Count == 0) { if (itemMatched2.Count > 0) { itemMatched = itemMatched2; } }
                //else { itemMatched = itemMatched1; }

                //if (itemMatched!=null && itemMatched.Count > 0)
                //{
                //    if (itemMatched.Count != _lineItems.Count)
                //    {
                //        throw new Exception("Item Count mismatched.");
                //    }
                //}
                int errcnt = 0; //added by Kalpita on 13/11/2019
                if (itemMatched1.Count > 0)
                {
                    if (itemMatched3.Count > 0 && (itemMatched3.Count == _lineItems.Count)) { itemMatched = itemMatched3; }
                    else
                    {
                        if (itemMatched1.Count != _lineItems.Count) { errcnt++; } else { itemMatched = itemMatched1; }
                    }
                }
                else
                {
                    if (itemMatched3.Count > 0 && (itemMatched3.Count == _lineItems.Count)) { itemMatched = itemMatched3; } else { errcnt++; }
                }
                if (errcnt > 0) { throw new Exception("Item Count mismatched."); }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _lineItems;
        }

        public string SplitDecimal(string Amtvalue) // Added by sayak on 16-07-2019
        {
            string Result = "";
            if (Amtvalue != "")
            {
                string[] _arrVal = Amtvalue.Split('.');
                Result = LeSCommon.LeSCommon.convert.ToString(_arrVal[0]);
            }
            return Result;

        }
    }
}

#region commented
//public List<List<string>> GetOrderItems(RichTextBox txtData)
//{
//    List<List<string>> _lineItems = new List<List<string>>();
//    try
//    {
//        string ItemStringPattern = @"([\d]{1})\s+(.+)\s+Name[\s]{0,}[:]";
//        double CalculatedTotal = 0;
//        int itemStartLine = 0, descrLineCounter = 0;

//        int indxItem = txtData.Text.IndexOf("Item");
//        if (indxItem > 0)
//        {
//            itemStartLine = txtData.GetLineFromCharIndex(indxItem);
//        }

//        if (itemStartLine > 0)
//        {
//            itemStartLine = itemStartLine + 1;
//            List<string> itemData = new List<string>();
//            for (int i = itemStartLine; i < txtData.Lines.Length; i++)
//            {
//                string line = txtData.Lines[i];
//                if (line.Trim().Contains("Line Item Sub Total"))
//                {
//                    if (itemData.Count > 0) _lineItems.Add(itemData);
//                    break;
//                }

//                if (line.Trim().StartsWith("PHOENIX 2.0")) continue;
//                if (line.Trim().StartsWith("UNICOOL")) continue;

//                if (Regex.IsMatch(line.Trim(), ItemStringPattern))
//                {
//                    // Add Old Item Data to List //
//                    if (itemData.Count > 0)
//                    {
//                        _lineItems.Add(itemData);
//                        descrLineCounter = 0;
//                    }
//                    itemData = new List<string>();

//                    //if ((txtData.Lines[i - 1].Trim().Contains("Product Code")))
//                    //{
//                    //    itemData[10] = (itemData[10].Trim() + " " + line.Trim()).Trim();
//                    //}




//                    string qtyUomRegex = @"\s\s\s([\d.]{1,})\s+([A-Za-z\d]{2,})\s+([\d.,]{2,})\s+([A-Z]{3})\s+([\d.]{0,4})\s+([\d.]{1,})";

//                    string qtyUomRegex1 = @"\s\s\s([\d.]{1,})\s+([A-Za-z\d]{2,})\s+([A-Z]{3})\s+([\d.]{0,4})\s+([\d.]{1,})";

//                    string itemNum = line.Trim().Split(' ')[0].Trim();
//                    string PartNum = line.Substring(line.IndexOf(itemNum) + itemNum.Length + 1, line.IndexOf("Name") - 5).Trim();
//                    string ItemDesc = "", ItemQty = "", ItemUom = "", ItemPrice = "", ItemCurr = "", ItemTotal = "", ItemDisc = "";

//                    if (txtData.Lines[i - 1].Trim().Length > 0)
//                    {
//                        if (txtData.Lines[i - 1].Length > 100) ItemDesc = txtData.Lines[i - 1].Substring(0, 70);
//                        else ItemDesc = txtData.Lines[i - 1].Trim();
//                    }

//                    line = line.Substring(line.IndexOf("Name") + 4);

//                    Match matchedString = Regex.Match(line.Trim(), qtyUomRegex);
//                    Match matchedString1 = Regex.Match(line.Trim(), qtyUomRegex1);
//                    if (matchedString.Success)
//                    {
//                        int itemDescrEndCol = line.IndexOf(matchedString.Value) - 1;
//                        int grpCount = matchedString.Groups.Count;

//                        if (grpCount == 7) // Discount is Missing
//                        {
//                            ItemQty = LeSCommon.LeSCommon.convert.ToDouble(matchedString.Groups[1].Value).ToString();
//                            ItemUom = matchedString.Groups[2].Value;
//                            ItemPrice = LeSCommon.LeSCommon.convert.ToDouble(matchedString.Groups[3].Value).ToString();
//                            ItemCurr = matchedString.Groups[4].Value;
//                            ItemDisc = LeSCommon.LeSCommon.convert.ToDouble(matchedString.Groups[5].Value).ToString();
//                            ItemTotal = LeSCommon.LeSCommon.convert.ToDouble(matchedString.Groups[6].Value).ToString();

//                            // UPDATED ON 31-JAN-19
//                            if (LeSCommon.LeSCommon.convert.ToDouble(ItemTotal) == 0 && LeSCommon.LeSCommon.convert.ToDouble(ItemPrice) > 0)
//                            {
//                                // Search price & Item total in above line                                        
//                                if (txtData.Lines[i - 1].Length > 100) ItemTotal = LeSCommon.LeSCommon.convert.ToDouble(txtData.Lines[i - 1].Substring(100).Trim()).ToString();
//                            }
//                        }

//                        // Set item description //
//                        ItemDesc += " " + line.Substring(0, itemDescrEndCol).Trim().Trim(':').Trim();
//                        CalculatedTotal = CalculatedTotal + Convert.ToDouble(ItemTotal);

//                        itemData.Add(itemNum); //0
//                        itemData.Add(PartNum.Trim()); //1
//                        itemData.Add(ItemDesc.Trim()); //2
//                        itemData.Add(ItemQty); //3
//                        itemData.Add(ItemUom); //4
//                        itemData.Add(ItemPrice); //5
//                        itemData.Add(ItemDisc); //6
//                        itemData.Add(""); //7
//                        itemData.Add(""); //8
//                        itemData.Add(""); //9
//                        itemData.Add(""); //10 Remarks                              
//                        itemData.Add(ItemCurr.Trim()); //11 Currency 

//                        descrLineCounter++;
//                    }
//                    else if (matchedString1.Success)
//                    {
//                        int itemDescrEndCol = line.IndexOf(matchedString1.Value) - 1;
//                        int grpCount = matchedString1.Groups.Count;

//                        if (grpCount == 6) // Discount is Missing
//                        {
//                            // UPDATED ON 31-JAN-19
//                            string itemDetail = "";
//                            if (ItemDesc.Length > 60)
//                            {
//                                itemDetail = ItemDesc.Substring(60).Trim();
//                                ItemDesc = ItemDesc.Substring(0, 60);
//                            }
//                            itemDetail = Regex.Replace(itemDetail.Trim(), @"\s+", " ");
//                            string[] values = itemDetail.Split(' ');

//                            ItemQty = LeSCommon.LeSCommon.convert.ToDouble(matchedString1.Groups[1].Value).ToString();
//                            ItemUom = matchedString1.Groups[2].Value;
//                            int count = ItemDesc.Split(' ').Length;
//                            if (values.Length > 0)
//                            {
//                                ItemPrice = values[0]; if (ItemPrice == "") { ItemPrice = (matchedString1.Groups[5].Length > 0) ? Convert.ToString(matchedString1.Groups[5].Value) : ""; }//changed by kalpita on 26/06/2019

//                            }//ItemDesc.Split(' ')[count - 1];//LeSCommon.LeSCommon.convert.ToDouble(matchedString1.Groups[3].Value).ToString();                                   
//                            ItemCurr = matchedString1.Groups[3].Value;
//                            ItemDisc = LeSCommon.LeSCommon.convert.ToDouble(matchedString1.Groups[4].Value).ToString();
//                            ItemTotal = LeSCommon.LeSCommon.convert.ToDouble(matchedString1.Groups[5].Value).ToString();

//                            // UPDATED ON 31-JAN-19
//                            if (LeSCommon.LeSCommon.convert.ToDouble(ItemTotal) == 0 && values.Length > 1)
//                            {
//                                // Search price & Item total in above line
//                                ItemTotal = LeSCommon.LeSCommon.convert.ToDouble(values[1]).ToString();
//                            }
//                        }

//                        // Set item description //
//                        ItemDesc += " " + line.Substring(0, itemDescrEndCol).Trim().Trim(':').Trim();
//                        CalculatedTotal = CalculatedTotal + Convert.ToDouble(ItemTotal);

//                        itemData.Add(itemNum); //0
//                        itemData.Add(PartNum.Trim()); //1
//                        itemData.Add(ItemDesc.Trim()); //2
//                        itemData.Add(ItemQty); //3
//                        itemData.Add(ItemUom); //4
//                        itemData.Add(ItemPrice); //5
//                        itemData.Add(ItemDisc); //6
//                        itemData.Add(""); //7
//                        itemData.Add(""); //8
//                        itemData.Add(""); //9
//                        itemData.Add(""); //10 Remarks                              
//                        itemData.Add(ItemCurr.Trim()); //11 Currency 

//                        descrLineCounter++;
//                    }
//                   //  else throw new Exception("Unable to read item data for item number -" + itemNum);
//                }
//                else
//                {
//                    // Append data to existing Item //
//                    string nextLine = txtData.Lines[i + 1].Trim();

//                    if (!Regex.IsMatch(nextLine, ItemStringPattern) && itemData.Count > 0 && line.Trim().Length > 00)
//                    {
//                        if (descrLineCounter < 3)
//                        {
//                            // append to existing item
//                            itemData[2] = (itemData[2].Trim() + " " + line.Trim()).Trim();
//                            descrLineCounter++;
//                        }
//                        else
//                        {
//                            itemData[10] = (itemData[10].Trim() + " " + line.Trim()).Trim();
//                        }
//                    }
//                }
//            }
//        }

//        // Get Line Item Total //
//        double _itemTotal = 0;
//        int indxItemTot = txtData.Text.IndexOf("Line Item Sub Total");
//        if (indxItemTot > 0)
//        {
//            int lnItemTotal = txtData.GetLineFromCharIndex(indxItemTot);
//            if (lnItemTotal > 0)
//            {
//                indxItemTot = txtData.Lines[lnItemTotal].IndexOf("Line Item Sub Total");
//                if (indxItemTot > 0)
//                {
//                    string itemTotal = txtData.Lines[lnItemTotal].Substring(indxItemTot + "Line Item Sub Total".Length).Trim().Trim(':').Trim();
//                    _itemTotal = LeSCommon.LeSCommon.convert.ToDouble(itemTotal.Trim());
//                }
//            }
//        }
//        else throw new Exception("Field 'Line Item Sub Total' not found while getting items");

//        if (SplitDecimal(_itemTotal.ToString()) != SplitDecimal(CalculatedTotal.ToString())) // Added by sayak on 16-07-2019
//        {
//            throw new Exception("Item Total mismatched with PDF Item Total");
//        }

//        // Get Total Item Count //
//        string itemContent = txtData.Text.Substring(indxItem, txtData.Text.IndexOf("Line Item Sub Total") - indxItem);
//        MatchCollection itemMatched = Regex.Matches(itemContent, ItemStringPattern);
//        if (itemMatched.Count > 0)
//        {
//            if (itemMatched.Count != _lineItems.Count)
//            {
//                // throw new Exception("Item Count mismatched.");
//            }
//        }
//    }
//    catch (Exception ex)
//    {
//        throw ex;
//    }

//    return _lineItems;
//}
#endregion

