using eSupplierDataMain;
using eSupplierDataMain.Dal;
using MTML.GENERATOR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PacificFujiDemo
{
    public class PacificFujiRoutine
    {
        #region  Variables
        private int nLinkId = 0;
        private List<string> slCurrency = new List<string>();
        private StringBuilder sbItemLines = null;
        private string Vessel, IMO, _byrComments = "", filename = "", PortName, cIMONO = "", cByrSupp_Lnkcode = "", cSuppcode = "", cSuppname = "", cBuyercode = "", cBuyername = "", cPDF_Files = "";
        private MTMLInterchange _interChange = null;
        private DocHeader _docHeader = null;
        private SMData_Routines _smRoutine = null;
        private RichTextBox txtData,txtHeader,txtFooter;
        private CommonRoutine _commonRoutine = new CommonRoutine();
        private DataSet ds = null;
        #endregion

        public PacificFujiRoutine()
        {
            try
            {
                //this._smRoutine = new SMData_Routines();
                this.txtData = new RichTextBox();
                this.txtHeader = new RichTextBox();
                this.txtFooter = new RichTextBox();
                GetCommonSettings();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void GetCommonSettings()
        {
            cSuppcode = ConfigurationManager.AppSettings["SUPPLIER"].Split('|')[0];
            cSuppname = ConfigurationManager.AppSettings["SUPPLIER"].Split('|')[1];
            cBuyercode = ConfigurationManager.AppSettings["BUYER"].Split('|')[0];
            cBuyername = ConfigurationManager.AppSettings["BUYER"].Split('|')[1];
            cPDF_Files = ConfigurationManager.AppSettings["PDF_FILES"];
            cByrSupp_Lnkcode = ConfigurationManager.AppSettings["BYER_SUPP_LINKCODE"];
        }

        public bool CheckInvalidDoc(FileInfo pdfFile)
        {
            bool flag;
            int i;
            char[] chrArray;
            try
            {
                bool invalid = false;
                RichTextBox t = new RichTextBox()
                {
                    Text = this._commonRoutine.GetText(pdfFile.FullName)
                };
                string[] InvalidPDFContent = File.ReadAllLines(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\INVALID_PDF_CONTENT.txt"));
                string[] strArrays = InvalidPDFContent;
                for (i = 0; i < (int)strArrays.Length; i++)
                {
                    string line = strArrays[i];
                    chrArray = new char[] { '=' };
                    string[] keyValues = line.Split(chrArray);
                    if ((int)keyValues.Length > 1)
                    {
                        string actualValue = this.GetValue(t, keyValues[0]);
                        string str = keyValues[1].Trim();
                        chrArray = new char[] { '|' };
                        string[] values = str.Split(chrArray);
                        if (actualValue.Trim().ToUpper().Contains(values[0].Trim().ToUpper()))
                        {
                            if (((int)values.Length <= 1 ? false : t.Text.Trim().ToUpper().Contains(values[1].Trim().ToUpper())))
                            {
                                invalid = true;
                                break;
                            }
                        }
                    }
                }
                if (!invalid)
                {
                    string InvalidPDFName = File.ReadAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\INVALID_PDF_FILENAME.txt"));
                    string str1 = InvalidPDFName.Trim();
                    chrArray = new char[] { '|' };
                    strArrays = str1.Split(chrArray);
                    i = 0;
                    while (i < (int)strArrays.Length)
                    {
                        string Name = strArrays[i];
                        if (!pdfFile.Name.Contains(Name))
                        {
                            i++;
                        }
                        else
                        {
                            invalid = true;
                            break;
                        }
                    }
                }
                flag = invalid;
            }
            catch (Exception exception)
            {
                flag = false;
            }
            return flag;
        }

        public void ProcessFiles()
        {
            FileInfo[] files = null;
            DirectoryInfo dir = new DirectoryInfo(cPDF_Files);
            if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

            files = dir.GetFiles();
            if (files.Length > 0)
            {
                foreach (FileInfo file in files)
                {
                    if (file.Length > 0)
                    {
                        if (file.Extension.ToLower() == ".pdf")
                        {
                            DataLog.AddLog("Processing file - " + file.Name + " started-------------------");
                            bool IsTimeOutOrDeadLock = false;
                            if (this.ConvertPDF(file.FullName))
                            {
                                File.Move(file.FullName, Path.GetDirectoryName(file.FullName) + "\\BACKUP\\" + file.Name);
                            }
                            else if (!IsTimeOutOrDeadLock)
                            {
                                File.Move(file.FullName, Path.GetDirectoryName(file.FullName) + "\\ERROR_FILES\\" + file.Name);
                            }
                        }
                        DataLog.AddLog("Processing file - " + file.Name + " ended.");
                    }
                }
            }
        }

        public bool ConvertPDF(string Filename)
        {
            string cAudit;
            bool _result = false;
            FileInfo f = new FileInfo(Filename);
            try
            {
                string ExportPath = ConfigurationManager.AppSettings["ESUPPLIER_MTML_INBOX"];
                this._interChange = new MTMLInterchange();
                this._docHeader = new DocHeader();
                this.txtData = new RichTextBox()
                {
                    Text = this._commonRoutine.GetText(Filename)
                };
                if (this.txtData.Lines[0].Trim().Contains("Request"))
                {
                    this.SetHeaderDetails( Filename);
                    this._interChange.DocumentHeader = this._docHeader;
                    string xmlFile = string.Concat(ExportPath, "\\RFQ_", Path.GetFileNameWithoutExtension(Filename), ".xml");
                    (new MTMLClass()).Create(this._interChange, xmlFile);
                    if (!File.Exists(xmlFile))
                    {
                        _result = false;
                        this._smRoutine.MoveFiles(f.FullName, string.Concat(f.Directory, "\\ERROR_FILES"));
                        cAudit = string.Concat("363. Unable to convert file ", Path.GetFileName(Filename));
                        DataLog.AddLog(cAudit);
                    }
                    else
                    {
                        cAudit = string.Concat("PDF File ", Path.GetFileName(Filename), " Converted Successfully.");
                        DataLog.AddLog(cAudit);
                        _result = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                cAudit = string.Concat("228. Unable to convert file ", Path.GetFileName(Filename), ". Error - ", ex.Message);
                DataLog.AddLog(cAudit);              
            }
            return _result;
        }

        public void ImportPDF_Files(FileInfo file)
        {
            try
            {

               
            }
            catch (Exception exception1)
            {
                Exception ex1 = exception1;
            }
            finally
            {
                // this.ds.Dispose();
                GC.Collect();
            }
        }

        private void SetHeaderDetails( string Full_Filename)
        {
            this._byrComments = "";
            string[] _data = null;     string VRNO = "", _strData = "";
            string cBuyerComments = ""; char[] chrArray = null;

            MTMLInterchange mTMLInterchange = this._interChange;
            DateTime now = DateTime.Now;
            mTMLInterchange.PreparationDate = now.ToString("yyyyMMdd");
            mTMLInterchange.PreparationTime = now.ToString("HHmmssff");
            mTMLInterchange.Recipient = cSuppcode;
            mTMLInterchange.Sender = cByrSupp_Lnkcode;
            mTMLInterchange.VersionNumber = "1";
            mTMLInterchange.BuyerSuppInfo.REF_No = VRNO;
            mTMLInterchange.BuyerSuppInfo.FileName = Path.GetFileName(Full_Filename);
            mTMLInterchange.BuyerSuppInfo.FileFullName = Full_Filename;
            this._docHeader.DocType = "RequestForQuote";
            this._docHeader.MessageNumber = VRNO;
            this._docHeader.BuyerRefNo = VRNO;
            this._docHeader.VersionNumber = "1";
            this._docHeader.MessageReferenceNumber = string.Concat("RFQ : ", VRNO);


            for (int j = 0; j < 8; j++)//this.txtData.Lines.Length
            {
                _strData = this.txtData.Lines[j].Trim().Replace(" - ", "|");
                while (_strData.Contains("||"))
                {
                    _strData = _strData.Replace("||", "|");
                }
                chrArray = new char[] { '|' };
                _data = _strData.Split(chrArray);
                for (int i = 0; i < (int)_data.Length; i++)
                {
                    if (_data[i].Contains("Request :"))
                    {
                        VRNO = Convert.ToString(_data[i]).Trim();
                        string str = VRNO = VRNO.Replace("Request :", "").Trim();
                        this._docHeader.References.Add(new Reference(ReferenceQualifier.UC, VRNO));
                    }
                    else if (_data[i].Contains("Preffered"))
                    {
                        string deldate = Convert.ToString(_data[i].Split(':')[1]).Trim();
                        this._docHeader.DateTimePeriods.Add(GetDateTimeDetails("Delivery", deldate));
                        PortName = Convert.ToString(_data[i + 1]);
                    }
                    else if (_data[i].Contains("Date"))
                    {
                        string docdate = Convert.ToString(_data[i].Split(':')[1]).Trim().Replace(" ", "|");

                        while (docdate.Contains("||"))
                        {
                            docdate = docdate.Replace("||", "|");
                        }
                        chrArray = new char[] { '|' };
                        _data = docdate.Split(chrArray);

                        this._docHeader.DateTimePeriods.Add(GetDateTimeDetails("Document", _data[0]));
                    }
                    else if (_data[i].Contains("Ship"))
                    {
                        this.Vessel = Convert.ToString(_data[i].Split(':')[1]).Trim();
                    }
                }
            }
            this._docHeader.PartyAddresses = this.GetParties();
            int _index = 0;
            _strData = "";
            this._commonRoutine.GetRow(this.txtData.Lines, "Free", ref _index);
            this._docHeader.LineItems = this.GetLineItems(_index, out cBuyerComments);
            this._docHeader.LineItemCount = this._docHeader.LineItems.Count;
            this._docHeader.Comments.Add(new Comments(CommentTypes.PUR, cBuyerComments));
            this.slCurrency.Clear();
            if (this.slCurrency.Count > 0)
            {
                List<string> distinct = this.slCurrency.Distinct<string>().ToList<string>();
                if (distinct.Count == 1)
                {
                    this._docHeader.CurrencyCode = distinct[0];
                }
            }
        }


        private LineItemCollection GetLineItems(int index, out string cBuyerComments)
        {
            int i;
            string _strData = "";
            string[] _data = null;
            cBuyerComments = "";
            List<int> slRowindx = new List<int>();
            slRowindx.Clear();
            LineItemCollection _items = new LineItemCollection();
            try
            {
                string pattern =@"^\d+\s*";//"^\\d+\\/\\d+\\/\\d+";
                for (i = index + 2; i < (int)this.txtData.Lines.Length; i++)
                {
                    if (Regex.IsMatch(this.txtData.Lines[i], pattern))
                    {
                        slRowindx.Add(i);
                    }
                }
                if (slRowindx.Count > 0)
                {
                    int diff = 0;
                    int nItemNo = 0;
                    for (i = 0; i < slRowindx.Count; i++)
                    {
                        string PartName = "";
                        string Remarks = "";
                        string Unit = "";
                        string UnitPrice = "0";
                        string Disc = "0";
                        string DrawingNo = "";
                        string Total = "0";
                        string Qty = "0";
                        string Equipment = "";
                        nItemNo = i + 1;
                        _strData = "";
                        int nStartindx = slRowindx[i];
                        diff = (i >= slRowindx.Count - 1 ? nStartindx + (this.txtData.Lines.Count<string>() - slRowindx[i]) : slRowindx[i + 1]);
                        for (int k = nStartindx; k < diff; k++)
                        {
                            _strData = string.Concat(_strData, this.txtData.Lines[k].Trim(), "|");
                        }
                        char[] chrArray = new char[] { '|' };
                        _strData = _strData.TrimEnd(chrArray);
                        chrArray = new char[] { '|' };
                        _data = _strData.Split(chrArray);
                        for (int j = 0; j < (int)_data.Length; j++)
                        {
                            if (j == 0)
                            {
                                Match _mtch = Regex.Match(_data[j], pattern);
                                PartName = _data[j].Replace(_mtch.Value, "  ").Trim();
                            }
                            else if (_data[j].Contains("Drawing No."))
                            {
                                string str = _data[j];
                                chrArray = new char[] { ':' };
                                DrawingNo = str.Split(chrArray)[1];
                            }
                            else if (_data[j].Contains("Request Quantity"))
                            {
                                string str1 = _data[j];
                                chrArray = new char[] { ':' };
                                string QtyUM = str1.Split(chrArray)[1].Trim().Replace(' ', '|');
                                chrArray = new char[] { '|' };
                                if ((int)QtyUM.Split(chrArray).Length <= 1)
                                {
                                    chrArray = new char[] { '|' };
                                    string[] _arrQtyUM = Regex.Matches(QtyUM, @"[a-zA-Z]+|\d+")
                                          .Cast<Match>()
                                          .Select(m => m.Value)
                                          .ToArray();
                                    Unit = _arrQtyUM[1]; Qty = _arrQtyUM[0];
                                    // Qty = QtyUM.Split(chrArray)[0];
                                }
                                else
                                {
                                    chrArray = new char[] { '|' };
                                    Qty = QtyUM.Split(chrArray)[0];
                                    chrArray = new char[] { '|' };
                                    Unit = QtyUM.Split(chrArray)[1];
                                }
                            }
                            else if (_data[j].Contains("Text (internal)"))
                            {
                                string str2 = _data[j];
                                chrArray = new char[] { ':' };
                                Equipment =(chrArray.Length>1)? str2.Split(chrArray)[1]:string.Empty;
                            }
                            else if (_data[j].Contains("Text (external)"))
                            {
                                Remarks = string.Concat(_data[j].Replace("Text (external)", "").Trim(), ",");
                            }
                            else if (!string.IsNullOrEmpty(_data[j].Trim()))
                            {
                                Remarks = string.Concat(Remarks, _data[j].Trim(), ",");
                            }
                        }
                        if (Convert.ToDouble(Qty) <= 0)
                        {
                            cBuyerComments = string.Concat(cBuyerComments, "Name :", PartName);
                        }
                        else
                        {
                            LineItem item = new LineItem()
                            {
                                Number = Convert.ToString(nItemNo),
                                Description = PartName,
                                DRAWINGNO = DrawingNo,
                                TypeCode = LineItemTypeCodes.BP,
                                SYS_ITEMNO = nItemNo,
                                LineItemComment = new Comments()
                                {
                                    Qualifier = CommentTypes.LIN
                                }
                            };
                            if (Remarks != "")
                            {
                                Comments lineItemComment = item.LineItemComment;
                                chrArray = new char[] { ',' };
                                lineItemComment.Value = Remarks.TrimEnd(chrArray);
                            }
                            item.Quantity = Convert.ToDouble(Qty);
                            item.MeasureUnitQualifier = Unit;
                            item.Section = new Section()
                            {
                                Description = Equipment
                            };
                            PriceDetails _price = new PriceDetails(PriceDetailsTypeCodes.Quoted_QT, PriceDetailsTypeQualifiers.GRP, Convert.ToDouble(UnitPrice));
                            item.PriceList.Add(_price);
                            PriceDetails _discount = new PriceDetails(PriceDetailsTypeCodes.Quoted_QT, PriceDetailsTypeQualifiers.DPR, Convert.ToDouble(Disc));
                            item.PriceList.Add(_discount);
                            item.MonetaryAmount = Convert.ToDouble(Total);
                            _items.Add(item);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw;
            }
            return _items;
        }

        private PartyCollection GetParties()
        {
            PartyCollection _collection = new PartyCollection();
            Party _buyer = new Party();
            Contact _cBuyer = new Contact()
            {
                FunctionCode = ContactFunction.PD
            };
            _buyer.Qualifier = PartyQualifier.BY;
             _buyer.Name = cBuyername;
            _buyer.Contacts.Add(_cBuyer);
            _collection.Add(_buyer);
            Party _supplier = new Party();
            Contact _cSupplier = new Contact()
            {
                FunctionCode = ContactFunction.SR
            };
            _supplier.Qualifier = PartyQualifier.VN;
            _supplier.Name =cSuppname;
            _supplier.Contacts.Add(_cSupplier);
            _collection.Add(_supplier);

            Party _vessel = new Party()
            {
                Qualifier = PartyQualifier.UD,
                Name = this.Vessel,
                Identification = string.Empty,            
            };
            _vessel.PartyLocation = new PartyLocation();
            _vessel.PartyLocation.Port = PortName;
            _collection.Add(_vessel);
            _collection.Add(new Party()
            {
                Qualifier = PartyQualifier.CN
            });
            return _collection;
        }

        private void GetValidFile(FileInfo file)
        {
            string cAudit;
            if (this.IsScannedDocument(file))
            {
                cAudit = string.Concat("Scanned image PDF file not imported - '", file.Name, "'; Scanned image PDF file.");
                DataLog.AddLog(cAudit);
            }
            else if (!this.CheckInvalidDoc(file))
            {
                cAudit = "Unable to convert  PDF file; Invalid file or Buyer-Supplier mapping details not found in file.";
                DataLog.AddLog(cAudit);
            }
            else
            {
                cAudit = string.Concat("Invalid PDF file not imported - '", file.Name, "'; Invalid PDF file.");
                DataLog.AddLog(cAudit);
            }
        }

        private string GetValue(RichTextBox t, string FieldData)
        {
            int nStartRow = -1;
            int nEndRow = -1;
            int nStartCol = -1;
            int nEndCol = -1;
            string[] Index = convert.ToString(FieldData).Split(new char[] { ':' });
            if (((int)Index.Length <= 0 ? true : !int.TryParse(Index[0], out nStartRow)))
            {
                nStartRow = -1;
            }
            if (((int)Index.Length <= 1 ? true : !int.TryParse(Index[1], out nEndRow)))
            {
                nEndRow = -1;
            }
            if (((int)Index.Length <= 2 ? true : !int.TryParse(Index[2], out nStartCol)))
            {
                nStartCol = -1;
            }
            if (((int)Index.Length <= 3 ? true : !int.TryParse(Index[3], out nEndCol)))
            {
                nEndCol = -1;
            }
            string _value = "";
            if ((nStartRow <= 0 ? false : (int)t.Lines.Length > nStartRow))
            {
                if (!(nEndCol <= nStartCol ? true : t.Lines[nStartRow - 1].Length <= nEndCol))
                {
                    _value = t.Lines[nStartRow - 1].Substring(nStartCol - 1, nEndCol - nStartCol).Trim();
                }
                else if ((nStartCol == -1 ? false : t.Lines[nStartRow - 1].Length >= nStartCol - 1))
                {
                    _value = t.Lines[nStartRow - 1].Substring(nStartCol - 1).Trim();
                }
                if (nEndRow > 0)
                {
                    for (int i = nStartRow; i < nEndRow; i++)
                    {
                        if (!(nEndCol <= nStartCol ? true : t.Lines[i].Length <= nEndCol))
                        {
                            _value = string.Concat(_value, " ", t.Lines[i].Substring(nStartCol - 1, nEndCol - nStartCol).Trim());
                        }
                        else if (!(t.Lines[i].Trim() == "" ? true : t.Lines[i].Length <= nStartCol - 1))
                        {
                            _value = string.Concat(_value, " ", t.Lines[i].Substring(nStartCol - 1).Trim());
                        }
                        else if (nStartCol == 1)
                        {
                            _value = string.Concat(_value, " ", t.Lines[i].Trim());
                        }
                    }
                }
            }
            return _value;
        }

        public bool IsScannedDocument(FileInfo pdfFile)
        {
            bool isScannedFile = false;
            try
            {
                if (this._commonRoutine.GetText(pdfFile.FullName).Trim().Length == 0)
                {
                    if (!pdfFile.Name.StartsWith("RFQ"))
                    {
                        isScannedFile = true;
                    }
                }
            }
            catch
            {
            }
            return isScannedFile;
        }

        private DateTimePeriod GetDateTimeDetails(string DateType, string value)
        {
            DateTime dt = DateTime.MinValue; DateTimePeriod _dtdate = new DateTimePeriod();
            if (DateType == "Delivery")
            {

                DateTime.TryParseExact(value, "yyyy-MM-dd", null, DateTimeStyles.None, out dt);
                _dtdate.FormatQualifier = DateTimeFormatQualifiers.CCYYMMDD_102;
                _dtdate.Qualifier = DateTimePeroidQualifiers.DeliveryDate_69;
                if (dt != DateTime.MinValue)
                {
                    _dtdate.Value = dt.ToString("yyyyMMdd");
                }
            }
            else if (DateType == "Document")
            {
                DateTime.TryParseExact(value, "yyyy-MM-dd", null, DateTimeStyles.None, out dt);
                _dtdate.FormatQualifier = DateTimeFormatQualifiers.CCYYMMDD_102;
                _dtdate.Qualifier = DateTimePeroidQualifiers.DocumentDate_137;
                if (dt != DateTime.MinValue)
                {
                    _dtdate.Value = dt.ToString("yyyyMMdd");
                }
            }
            return _dtdate;
        }

        private void GetFormatdate(string dateValue)
        {
           
            //if(dateValue)
            //char[] chrArray = new char[] { ':' };
            //_data = str2.Split(chrArray);
            //if (((int)_data.Length <= 0 ? false : _data[0].Contains("Date")))
            //{
            //    string[] dtArr = null;
            //    if (_data[1].Contains<char>('/'))
            //    {
            //        string str3 = _data[1];
            //        chrArray = new char[] { '/' };
            //        dtArr = str3.Split(chrArray);
            //    }
            //    else if (_data[1].Contains<char>('.'))
            //    {
            //        string str4 = _data[1];
            //        chrArray = new char[] { '.' };
            //        dtArr = str4.Split(chrArray);
            //    }
            //    else if (_data[1].Contains<char>('-'))
            //    {
            //        string st = _data[1];
            //        chrArray = new char[] { '-' };
            //        dtArr = st.Split(chrArray);
            //    }
            //    string[] strArrays = new string[] { dtArr[2].Trim(), "-", dtArr[1].Trim(), "-", dtArr[0].Trim() };
            //    string date = string.Concat(strArrays);
            //}
        }

        private string TrimedText(string strInput)
        {
            char[] chrArray;
            if (strInput == null)
            {
                strInput = "";
            }
            else
            {
                strInput = strInput.Trim();
                if ((strInput.EndsWith(":") ? true : strInput.StartsWith(":")))
                {
                    string str = Convert.ToString(strInput);
                    chrArray = new char[] { ':' };
                    string str1 = str.TrimStart(chrArray);
                    chrArray = new char[] { ':' };
                    strInput = str1.TrimEnd(chrArray);
                }
                if ((strInput.EndsWith("/") ? true : strInput.StartsWith("/")))
                {
                    string str2 = Convert.ToString(strInput);
                    chrArray = new char[] { '/' };
                    string str3 = str2.TrimStart(chrArray);
                    chrArray = new char[] { '/' };
                    strInput = str3.TrimEnd(chrArray);
                }
                if ((strInput.EndsWith(",") ? true : strInput.StartsWith(",")))
                {
                    string str4 = Convert.ToString(strInput);
                    chrArray = new char[] { ',' };
                    string str5 = str4.TrimStart(chrArray);
                    chrArray = new char[] { ',' };
                    strInput = str5.TrimEnd(chrArray);
                }
                if (strInput.StartsWith("(") & strInput.EndsWith(")"))
                {
                    string str6 = Convert.ToString(strInput);
                    chrArray = new char[] { '(' };
                    string str7 = str6.TrimStart(chrArray);
                    chrArray = new char[] { ')' };
                    strInput = str7.TrimEnd(chrArray);
                }
                if (strInput.StartsWith("[") & strInput.EndsWith("]"))
                {
                    string str8 = Convert.ToString(strInput);
                    chrArray = new char[] { '[' };
                    string str9 = str8.TrimStart(chrArray);
                    chrArray = new char[] { ']' };
                    strInput = str9.TrimEnd(chrArray);
                }
                strInput = strInput.Replace(",,", ",");
            }
            return strInput.Trim();
        }

        ~PacificFujiRoutine()
        {
            try
            {
                if (this.txtHeader != null)
                {
                    this.txtHeader.Dispose();
                }
                if (this.txtFooter != null)
                {
                    this.txtFooter.Dispose();
                }
            }
            finally
            {
                GC.Collect();
            }
        }
  
    }
}
