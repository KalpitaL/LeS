using MTML.GENERATOR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace Excel_SPL_Routine
{
    public class Download_Excel_Routine
    {
        string _logpath = "";
        int _itemStart = -1, _CurrItemRow = -1;
        string Process_File_Name = "";
        Excel_Common_Routine _ExcelRTN;

        string BuyerCode = "", SupplierCode = "", DocType = "", BuyerRef = "", VesselName = "",
            Doc_Date = "", Currency = "", PortCode = "", PortName = "", Imono = "", Hullno = "", ReplyDate = "",
            VendorName = "", BuyerName = "", OtherCost = "", _ItemEndCol = "", _ItemEndSearchPattern = "",
            _addItemComment = "", PackageCost = "", FreightCost = "", HeaderDiscount = "", GrandTotal = "",
            ItemTotal = "", _Terms = "", BuyerRemarks = "",  _DeliveryDate = "", _ExpireDate = "",
            _SupplierRef = "", _ConsigneeName = "", _ConsigneAddress = "", BuyerContactPerson = "", BuyerContactno = "",
            BuyerEmail = "", SupplierEmail = "", BillName = "", BillAddresss = "", BuyerAddress = "", _hManufature = "",
            _hModel = "", _hEquipName = "", _hEquipno = "", _hDrawingNo = "", _hEquipDescription = "", MTML_Path = "", ProcessorName = "";

       // Dictionary<int, string> _ItemDetails = new Dictionary<int, string>();
       Dictionary<int ,List<string>> _ItemDetails = new Dictionary<int,List<string>>();

        public void ClearFieldsValues()
        {
            try
            {
                _itemStart = -1; _CurrItemRow = -1;
                BuyerCode = ""; SupplierCode = ""; DocType = ""; BuyerRef = ""; VesselName = "";
                Doc_Date = ""; Currency = ""; PortCode = ""; PortName = ""; Imono = ""; Hullno = ""; ReplyDate = "";
                VendorName = ""; BuyerName = ""; _ItemEndCol = ""; _ItemEndSearchPattern = "";
                _addItemComment = ""; OtherCost = ""; PackageCost = ""; FreightCost = ""; HeaderDiscount = "";
                GrandTotal = ""; ItemTotal = ""; _Terms = ""; BuyerRemarks = ""; _DeliveryDate = ""; _ExpireDate = "";
                _SupplierRef = ""; _ConsigneeName = ""; _ConsigneAddress = ""; BuyerContactPerson = ""; BuyerContactno = "";
                BuyerEmail = ""; SupplierEmail = ""; BillName = ""; BillAddresss = ""; BuyerAddress = "";
                _hManufature = ""; _hModel = ""; _hEquipName = ""; _hEquipno = ""; _hDrawingNo = ""; _hEquipDescription = "";
                MTML_Path = convert.ToString(ConfigurationManager.AppSettings["MTML_PATH"]).Trim();
                if (MTML_Path == "") { MTML_Path = AppDomain.CurrentDomain.BaseDirectory + "MTML"; }
                if (!Directory.Exists(MTML_Path)) { Directory.CreateDirectory(MTML_Path); }
                ProcessorName = convert.ToString(ConfigurationManager.AppSettings["PROCESSOR_NAME"]).Trim();
            }
            catch (Exception ex)
            {
                throw new Exception("Error on ClearFieldsValues : " + ex.Message);
            }
            _ItemDetails.Clear();
        }

        public string LogText { set { WriteLog(value); } }

        private void WriteLog(string _logText, string _logFile = "")
        {
            if (_logpath == null || _logpath == "") _logpath = AppDomain.CurrentDomain.BaseDirectory + "\\Log";
            string _logfile = @"Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (_logFile.Length > 0) { _logfile = _logFile; }
            Directory.CreateDirectory(_logpath);
            Console.WriteLine(_logText);
            File.AppendAllText(_logpath + @"\" + @_logfile, DateTime.Now.ToString() + " " + _logText + Environment.NewLine);
        }

        public EXCEL_Config ReadConfigFile()
        {
            EXCEL_Config config = null;
            EXCEL_Mapping _mapping = new EXCEL_Mapping();
            try
            {
                string configFile = "";
                string configPath = convert.ToString(ConfigurationManager.AppSettings["EXCEL_MAPPING_PATH"]).Trim();//28-08-2017
                if (configPath != "")
                {
                    if (!Directory.Exists(Path.GetDirectoryName(configPath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(configPath));
                    }
                    configFile = configPath + "\\" + convert.ToString(ConfigurationManager.AppSettings["EXCEL_MAPPING_FILE"]).Trim();//28-08-2017
                }
                else
                {
                    configFile = AppDomain.CurrentDomain.BaseDirectory + "\\" + convert.ToString(ConfigurationManager.AppSettings["EXCEL_MAPPING_FILE"]).Trim();//28-08-2017
                }
                LogText = "Loading config mapping file '" + Path.GetFileName(configFile) + "'";
                config = _mapping.Load(configFile);
            }
            catch (Exception ex)
            {
                throw new Exception("Error on ReadConfigFile : "+ex.Message);
            }
            return config;
        }

        public bool Process_Excel_File(string _sFile)
        {
            bool _result = false;
            try
            {
                Process_File_Name = "";
                LogText = "Process started for '" + Path.GetFileName(_sFile) + "'";
                ClearFieldsValues();
                Process_File_Name = Path.GetFileName(_sFile);
                EXCEL_Config _Mapping = ReadConfigFile();
                LogText = "Loading Excel Routine started for '" + Path.GetFileName(_sFile) + "'";
                _ExcelRTN = new Excel_Common_Routine(_sFile);
                _ExcelRTN.SetFileWorkSheet(_sFile);
                LogText = "Loading Excel Routine successful for '" + Path.GetFileName(_sFile) + "'";
                if (_Mapping != null)
                {
                    _result = Get_Excel_Details(_Mapping);
                }
            }
            catch (Exception ex)
            {
                CreateAuditFile(Path.GetFileName(Process_File_Name), ProcessorName, BuyerRef, "Error", ex.Message);//21-11-2017
                LogText = "Error on Process_Excel_File : " + ex.Message;
            }
            return _result;
        }

        public bool Get_Excel_Details(EXCEL_Config _Mapping)
        {
            bool _result = false;
            try
            {
                DocType = GetDoctype(_Mapping.DocType.ValidateDocType);
                if (DocType.ToUpper().Trim() == "RFQ")
                {
                    LogText = "Processing RFQ details for " + Path.GetFileName(Process_File_Name);
                    RFQ_Mapping _RfqMapping = _Mapping.Rfq_Mapping;
                    List<HDATA> _ValidateMappings = _RfqMapping.HeaderMappings.Validate;
                    if (ValidateFields(_ValidateMappings))
                    {
                        //FOR PORT CONDITION 
                        //BuyerCode = GetBuyerCode(_RfqMapping.HeaderMappings.Validate_Buyer);
                        //SupplierCode = GetSupplierCode(_RfqMapping.HeaderMappings.Validate_Supplier);
                        if (DocType != "")
                        {
                            LogText = "Reading Header Details started.";
                            Get_Header_Details(_Mapping.Rfq_Mapping.HeaderMappings);

                            //added for port condition
                            if (!PortCode.ToUpper().Trim().Contains("SINGAPORE"))
                            {
                                BuyerCode = GetBuyerCode(_RfqMapping.HeaderMappings.Validate_Buyer);
                                SupplierCode = GetSupplierCode(_RfqMapping.HeaderMappings.Validate_Supplier);
                            }
                            else
                            {
                                BuyerCode = GetBuyerCode(_RfqMapping.HeaderMappings.Validate_Buyer_Singapore);
                                SupplierCode = GetSupplierCode(_RfqMapping.HeaderMappings.Validate_Supplier_Singapore);
                            }

                            Get_Item_details(_Mapping.Rfq_Mapping.ItemMappings);
                            Get_Footer_Details(_Mapping.Rfq_Mapping.FooterMappings);
                            if (!GenerateRFQMTML())
                            {
                                throw new Exception("Unable to generate RFQ MTML for file '" + Path.GetFileName(Process_File_Name) + "'");
                            }
                            else
                            {
                                _result = true;
                            }
                        }
                        else
                        {
                            throw new Exception("Unable to get Doctype.");
                        }
                    }
                    else
                    {
                        throw new Exception("Unable to get ValidateFields.");
                    }
                }
                else
                {
                    LogText = "Processing PO details for " + Path.GetFileName(Process_File_Name);
                    PO_Mapping _PoMapping = _Mapping.Po_Mapping;
                    List<HDATA> _ValidateMappings = _PoMapping.HeaderMappings.Validate;
                    if (ValidateFields(_ValidateMappings))
                    {
                        BuyerCode = GetBuyerCode(_PoMapping.HeaderMappings.Validate_Buyer);
                        SupplierCode = GetSupplierCode(_PoMapping.HeaderMappings.Validate_Supplier);
                        if (DocType != "")
                        {
                            LogText = "Reading Header Details started.";
                            Get_Header_Details(_Mapping.Po_Mapping.HeaderMappings);
                            Get_Item_details(_Mapping.Po_Mapping.ItemMappings);
                            Get_Footer_Details(_Mapping.Po_Mapping.FooterMappings);
                            if (!GeneratePOMTML())
                            {
                                throw new Exception("Unable to generate PO MTML for file '" + Path.GetFileName(Process_File_Name) + "'");
                            }
                            else
                            {
                                _result = true;
                            }
                        }
                        else
                        {
                            throw new Exception("Unable to get Doctype.");
                        }
                    }
                    else
                    {
                        throw new Exception("Unable to get ValidateFields.");
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("Unable to get ValidateFields."))//15-12-2017
                    CreateAuditFile(Path.GetFileName(Process_File_Name), ProcessorName, BuyerRef, "Error", ex.Message);//21-11-2017
                LogText = "Error on Get_Excel_Details : " + ex.Message;
            }
            return _result;
        }

        public void Get_Header_Details(HEADER_MAPPINGS _hMapping)
        {
            try
            {
                foreach (HDATA _headerDetails in _hMapping.Fields)
                {
                    GetFieldValue(_headerDetails);
                }
            }
            catch (Exception ex)
            {
                LogText = "Error on Get_Header_Details : " + ex.Message;
            }
        }

        public bool ValidateFields(List<HDATA> _lstValid)
        {
            bool _result = true;
            if (_lstValid.Count > 0)
            {
                foreach (HDATA _validate in _lstValid)
                {
                    string _CellValue = _ExcelRTN.GetExcelCell(_validate.ValidateCell);
                   string _ValidValue = _validate.ValidateValue;
                   if (_CellValue.Trim() != _ValidValue.Trim())
                   {
                       _result = false;
                       break;
                   }
                }
            }
            return _result;
        }

        public string GetBuyerCode(HDATA _ValidBuyer)
        {
            string  _BuyerCode = "";
            if (_ValidBuyer != null && _ValidBuyer.Buyer_Cell != null && _ValidBuyer.Buyer_Val != null && _ValidBuyer.Buyer_Code != null)
            {
                string[] _listCell = convert.ToString(_ValidBuyer.Buyer_Cell).Split('|');
                string[] _listBuyer = convert.ToString(_ValidBuyer.Buyer_Val).Split('|');
                string[] _listBuyerCode = convert.ToString(_ValidBuyer.Buyer_Code).Split('|');
                int counter = 0;
                foreach (string _cellVal in _listCell)
                {
                    string _CellValue = _ExcelRTN.GetExcelCell(_cellVal.Trim());
                    if (_CellValue.Contains(_listBuyer[counter]))
                    {
                        _BuyerCode = convert.ToString(_listBuyerCode[counter]);
                        BuyerName = convert.ToString(_listBuyer[counter]);//28-08-2017
                        break;
                    }
                    counter++;
                }
            }
            if (_BuyerCode == "")
            {
                _BuyerCode = convert.ToString(ConfigurationManager.AppSettings["BUYER_CODE"]);
            }
            return _BuyerCode;
        }

        public string GetSupplierCode(HDATA _ValidSupplier)
        {
            string _SupplierCode = "";
            if (_ValidSupplier != null && _ValidSupplier.Supplier_Cell != null && _ValidSupplier.Supplier_Val != null && _ValidSupplier.Supplier_Code != null)
            {
                string[] _listCell = convert.ToString(_ValidSupplier.Supplier_Cell).Split('|');
                string[] _listSupplier = convert.ToString(_ValidSupplier.Supplier_Val).Split('|');
                string[] _listSupplierCode = convert.ToString(_ValidSupplier.Supplier_Code).Split('|');
                int counter = 0;
                foreach (string _cellVal in _listCell)
                {
                    string _CellValue = _ExcelRTN.GetExcelCell(_cellVal);
                    if (_CellValue.Contains(_listSupplier[counter]))
                    {
                        _SupplierCode = convert.ToString(_listSupplierCode[counter]);
                        break;
                    }
                    counter++;
                }
            }
            if (_SupplierCode == "")
            {
                _SupplierCode = convert.ToString(ConfigurationManager.AppSettings["SUPPLIER_CODE"]);
            }
            return _SupplierCode;
        }

        public string GetDoctype(VDATA _ValidDoctype)
        {
            string _result = "";
            string[] _List_Rfq_Cell = convert.ToString(_ValidDoctype.RFQ_Cell).Split('|');
            string[] _List_Rfq_Value = convert.ToString(_ValidDoctype.RFQ_Val).Split('|');
            string[] _List_Po_Cell = convert.ToString(_ValidDoctype.PO_Cell).Split('|');
            string[] _List_Po_Value = convert.ToString(_ValidDoctype.PO_Val).Split('|');

            int _rfqCounter= 0;
            foreach (string _rfqCell in _List_Rfq_Cell)
            {
                string RfqValue = _ExcelRTN.GetExcelCell(_rfqCell.Trim());
                if (RfqValue.Trim() == convert.ToString(_List_Rfq_Value[_rfqCounter]))
                {
                    _result = "RFQ";
                    break;
                }
                _rfqCounter++;
            }

            if (_result == "")
            {
                int _poCounter = 0;
                foreach (string _poCell in _List_Po_Cell)
                {
                    string PoValue = _ExcelRTN.GetExcelCell(_poCell.Trim());
                    if (PoValue.Trim() == convert.ToString(_List_Po_Value[_poCounter]))
                    {
                        _result = "PO";
                        break;
                    }
                    _poCounter++;
                }
            }
            return _result;
        }

        public void GetFieldValue(HDATA _HeaderFields)
        {
            string qualifier = convert.ToString(_HeaderFields.Qualifier).Trim();
            switch (qualifier)
            {
                case "BUYER_REF":
                    BuyerRef = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Buyer Ref : " + BuyerRef;
                    break;
                case "VESSEL":
                    VesselName = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Vessel : " + VesselName;
                    break;
                case "DOC_DATE":
                    Doc_Date = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Doc_Date : " + Doc_Date;
                    break;
                case "CURRENCY":
                    Currency = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Currency : " + Currency;
                    break;
                case "PORT":
                    PortCode = GetHeaderFieldValue(_HeaderFields);
                    LogText = "PortCode : " + PortCode;
                    break;
                case "PORT_NAME":
                    PortName = GetHeaderFieldValue(_HeaderFields);
                    LogText = "PortName : " + PortName;
                    break;
                case "IMONO":
                    Imono = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Imono : " + Imono;
                    break;
                case "HULLNO":
                    Hullno = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Hullno : " + Hullno;
                    break;
                case "DELIVERY_DATE":
                    _DeliveryDate = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Delivery Date : " + _DeliveryDate;
                    break;
                case "EXPIRE_DATE":
                    _ExpireDate = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Expire Date : " + _ExpireDate;
                    break;
                case "REPLY_BY_DATE":
                    ReplyDate = GetHeaderFieldValue(_HeaderFields);
                    LogText = "ReplyDate : " + ReplyDate;
                    break;
                case "VENDOR_NAME":
                    VendorName = GetHeaderFieldValue(_HeaderFields);
                    LogText = "VendorName : " + VendorName;
                    break;
                case "BUYER_NAME":
                    BuyerName = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Buyer Name : " + BuyerName;
                    break;
                case "FREIGHT_COST":
                    FreightCost = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Fright Cost : " + FreightCost;
                    break;
                case "ALLOUNCE":
                    HeaderDiscount = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Header Discount : " + HeaderDiscount;
                    break;
                case "OTHER_COST":
                    OtherCost = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Other Cost : " + OtherCost;
                    break;
                case "PACKAGE_COST":
                    PackageCost = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Package Cost : " + PackageCost;
                    break;
                case "GRAND_TOTAL":
                    GrandTotal = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Grand Total : " + GrandTotal;
                    break;
                case "ITEM_TOTAL":
                    ItemTotal = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Item Total : " + ItemTotal;
                    break;
                case "TERMS":
                    _Terms = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Terms : " + _Terms;
                    break;
                case "HEADER_REMARKS":
                    BuyerRemarks = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Buyer Remarks : " + BuyerRemarks;
                    break;
                case "CONSIGNEE_NAME":
                    _ConsigneeName = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Consignee Name : " + _ConsigneeName;
                    break;
                case "CONSIGNEE_ADDR":
                    _ConsigneAddress = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Consignee Address : " + _ConsigneAddress;
                    break;
                case "BUYER_CONTACT_PERSON":
                    BuyerContactPerson = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Buyer Contact Person : " + BuyerContactPerson;
                    break;
                case "BUYER_CONTACT_NO":
                    BuyerContactno = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Buyer Contact No : " + BuyerContactno;
                    break;
                case "BUYER_EMAIL":
                    BuyerEmail = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Buyer Email : " + BuyerEmail;
                    break;
                case "SUPPLIER_EMAIL":
                    SupplierEmail = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Supplier Email : " + SupplierEmail;
                    break;
                case "SUPPLIER_REF":
                    _SupplierRef = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Supplier Ref : " + _SupplierRef;
                    break;
                case "BILL_NAME":
                    BillName = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Bill Name : " + BillName;
                    break;
                case "BILL_ADDR":
                    BillAddresss = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Bill Addr : " + BillAddresss;
                    break;
                case "BUYER_ADDR":
                    BuyerAddress = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Buyer Address : " + BuyerAddress;
                    break;
                case "HEADER_EQUIPMENT_NAME":
                    _hEquipName = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Header Equipment Name : " + _hEquipName;
                    break;
                case "HEADER_EQUIPMENT_DESC":
                    _hEquipDescription = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Header Equipment Desc : " + _hEquipDescription;
                    break;
                case "HEADER_EQUIPMENT_MODEL":
                    _hModel = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Header Equipment Model : " + _hModel;
                    break;
                case "HEADER_EQUIPMENT_MANUFACTURE":
                    _hManufature = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Header Equipment Manufature : " + _hManufature;
                    break;
                case "HEADER_EQUIPMENT_NO":
                    _hEquipno = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Header Equipment No : " + _hEquipno;
                    break;
                case "HEADER_EQUIPMENT_DRAWING":
                    _hDrawingNo = GetHeaderFieldValue(_HeaderFields);
                    LogText = "Header Equipment Drawing : " + _hDrawingNo;
                    break;
            }
        }

        public string GetHeaderFieldValue(HDATA _HeaderFields)
        {
            string result = "";
            try
            {
                string type = convert.ToString(_HeaderFields.Type).Trim();
                string isRequired = convert.ToString(_HeaderFields.IsMandataory).Trim();
                string qualifier = convert.ToString(_HeaderFields.Qualifier).Trim();
                string RemoveText = convert.ToString(_HeaderFields.RemoveText).Trim();
                switch (type)
                {
                    case "HEAER_DATA":
                        result = _ExcelRTN.GetExcelCell(_HeaderFields.FieldCell.Trim());
                        break;
                    case "HEAER_DYNAMIC_CELL_DATA":
                        result = Get_Dynamic_Header_Field_Value(_HeaderFields);
                        break;
                }
                if (RemoveText.Trim() != "")
                {
                    string[] _lstRemovetxt = RemoveText.Split('|');
                    foreach (string strRemove in _lstRemovetxt)
                    {
                        result = result.Replace(strRemove, "");
                    }
                }
                if (isRequired.Trim().ToUpper() == "TRUE")
                {
                    if (result == "")
                    {
                        throw new Exception("Unable to get value for mandatory field " + qualifier);
                    }
                }
            }
            catch (Exception ex)
            {
                CreateAuditFile(Path.GetFileName(Process_File_Name), ProcessorName, BuyerRef, "Error", ex.Message);//21-11-2017
                LogText = "Error on GetHeaderFieldValue : " + ex.Message;
            }
            return result;
        }

        public string Get_Dynamic_Header_Field_Value(HDATA _HeaderFields)
        {
            string _result = "";
            try
            {
                string type = convert.ToString(_HeaderFields.Type).Trim();
                string qualifier = convert.ToString(_HeaderFields.Qualifier).Trim();
                string RemoveText = convert.ToString(_HeaderFields.RemoveText).Trim();
                int sStart = convert.ToInt(_HeaderFields.StartRow);
                int sEnd = convert.ToInt(_HeaderFields.EndRow);
                string Col = convert.ToString(_HeaderFields.Column);
                string SearchPattern = convert.ToString(_HeaderFields.SearchText);
                int addRow = convert.ToInt(_HeaderFields.AddRow);
                int addCol = convert.ToInt(_HeaderFields.AddColumn);
                _result = _ExcelRTN.GetCellDynamicData(sStart, sEnd, Col, SearchPattern,
                    addRow, addCol);
            }
            catch (Exception ex)
            {
                throw new Exception("Error on Get_Dynamic_Header_Field_Value : " + ex.Message + " Stack trace : " + ex.StackTrace);
            }
            return _result;
        }

        public void Get_Item_details(ITEM_MAPPINGS _Mapping)
        {
            List<string> lststr=null;
            try
            {
                LogText = "Processing Item details."; 
                _itemStart = convert.ToInt(_Mapping.ItemStart.ItemStartRow);
                if (_itemStart > 0)
                {
                    _ItemEndCol = convert.ToString(_Mapping.ItemEnd.ItemEndCol);
                    _ItemEndSearchPattern = convert.ToString(_Mapping.ItemEnd.ItemEndSearchText);
                    for (int i = _itemStart; i < _ExcelRTN._worksheet.Cells.MaxDataRow; i++)
                    {
                        lststr=new List<string>();
                        string _Partno = "", _PartDesc = "", _Unit = "", _Qty = "", _ItemRmks = "",
                            _EquipName = "", _EquipMaker = "", _EquipType = "",_drawingNo = "",_itemCode = "",
                            _EquipModel = "", _addItemComment = "", itemPrice = "", itemTotal = "", itemDiscount = "",
                            equipDesc = "", equipNo = "", equipSeqNo="",_systemItemNo="";
                        string endValue = convert.ToString(_ExcelRTN.GetExcelCell(_ItemEndCol + i));
                        if (endValue.Trim() == _ItemEndSearchPattern.Trim())
                        {
                            _CurrItemRow = i;
                            break;
                        }
                        string ItemNo = Get_Item_Value_Qualifier("ITEMNO", _Mapping.ItemFields, i);
                        if (convert.ToInt(ItemNo.Trim()) > 0)
                        {
                            _CurrItemRow = i;
                            Dictionary<string, IDATA> _ItemMappings = _Mapping.ItemFields;
                            _Partno = Get_Item_Value_Qualifier("PARTNO", _ItemMappings, i);
                            _PartDesc = Get_Item_Value_Qualifier("DESCRIPTION", _ItemMappings, i);
                            _Unit = Get_Item_Value_Qualifier("UNIT", _ItemMappings, i);
                            _Qty = Get_Item_Value_Qualifier("QTY", _ItemMappings, i);
                            itemPrice = Get_Item_Value_Qualifier("UNIT_PRICE", _ItemMappings, i);
                            itemDiscount = Get_Item_Value_Qualifier("ITEM_DISCOUNT", _ItemMappings, i);
                            itemTotal = Get_Item_Value_Qualifier("ITEM_TOTAL", _ItemMappings, i);
                            //_ItemRmks = Get_Item_Value_Qualifier("REMARKS", _ItemMappings, i);//10-4-2018
                            _ItemRmks = Get_Item_Value_Qualifier("ITEM_REMARKS", _ItemMappings, i);//10-4-2018
                            
                            _EquipName = Get_Item_Value_Qualifier("EQUIPNAME", _ItemMappings, i);
                            _EquipMaker = Get_Item_Value_Qualifier("EQUIPMAKER", _ItemMappings, i);
                            _EquipType = Get_Item_Value_Qualifier("EQUIPTYPE", _ItemMappings, i);
                            equipSeqNo = Get_Item_Value_Qualifier("EQUIPSERIALNO", _ItemMappings, i);
                            _EquipModel = Get_Item_Value_Qualifier("EQUIPMODEL", _ItemMappings, i);
                             equipDesc = Get_Item_Value_Qualifier("EQUIP_DESC", _ItemMappings, i);
                             equipNo = Get_Item_Value_Qualifier("EQUIP_NO", _ItemMappings, i);
                            _drawingNo = Get_Item_Value_Qualifier("DRAWING_NO", _ItemMappings, i);
                            _itemCode = Get_Item_Value_Qualifier("ITEM_CODE", _ItemMappings, i);
                            _systemItemNo = convert.ToString(_CurrItemRow);
                             lststr.Add(ItemNo);
                              lststr.Add(_PartDesc);
                             lststr.Add(_Partno);
                              lststr.Add(_EquipName);
                              lststr.Add(_EquipMaker);
                              lststr.Add(_drawingNo);
                             lststr.Add(_EquipModel);
                              lststr.Add(_Qty);
                             lststr.Add(_Unit);
                              lststr.Add(_ItemRmks);
                             lststr.Add(_EquipType);
                              lststr.Add(itemPrice);
                              lststr.Add(itemDiscount);
                              lststr.Add(itemTotal);
                             lststr.Add(_systemItemNo);
                              lststr.Add("Equip Desc : " + equipDesc + " Equip no : " + equipNo );
                            lststr.Add(equipSeqNo);
                              _ItemDetails.Add(convert.ToInt(ItemNo), lststr);
                            //_ItemDetails.Add(convert.ToInt(ItemNo), ItemNo + "~" + _PartDesc + "~" + _Partno + "~" + _EquipName + "~" + _EquipMaker + "~" + _drawingNo + "~" +
                            //     _EquipModel + "~" + _Qty + "~" + _Unit + "~" + _ItemRmks + "~" + _EquipType + "~" + itemPrice + "~" + itemDiscount + "~" +
                            //     itemTotal +"~"+_systemItemNo+"~ Equip Desc : " + equipDesc + " Equip no : " + equipNo + "~" + equipSeqNo);
                            LogText = "Item details Add to the List : " + ItemNo + "~" + _PartDesc + "~" + _Partno + "~" + _EquipName + "~" + _EquipMaker + "~" + _drawingNo + "~" +
                                  _EquipModel + "~" + _Qty + "~" + _Unit + "~" + _ItemRmks + "~" + _EquipType + "~" + itemPrice + "~" + itemDiscount + "~" +
                                  itemTotal + "~" + _systemItemNo + "~ Equip Desc : " + equipDesc + " Equip no : " + equipNo + "~" + equipSeqNo;
                        }
                    }
                }
                if (_ItemDetails.Count <= 0)
                {
                    throw new Exception("Unable to get Item details."); 
                }
            }
            catch (Exception ex)
            {
                 throw new Exception("Unable to get item details : Error : "+ex.Message);
            }
        }

        public string Get_Item_Value_Qualifier(string _qualifier, Dictionary<string, IDATA> _lstData, int Rowno)
        {
            string _result = "";
            if (_lstData.ContainsKey(_qualifier))
            {
                IDATA _ItemDetails = _lstData[_qualifier];
                _result = Get_Item_Details(_ItemDetails, Rowno);
                if (convert.ToString(_ItemDetails.IsRemarks).ToUpper() == "TRUE")
                {
                    _addItemComment += " " + _qualifier.ToUpper() + " : " + _result;
                }
            }
            return _result;
        }

        public string Get_Item_Details(IDATA _ItemDetails, int Rowno)
        {
            string _result = "";
            try
            {
                string sType = convert.ToString(_ItemDetails.Type);
                string sQualifier = convert.ToString(_ItemDetails.Qualifier);
                string isRequired = convert.ToString(_ItemDetails.IsMandataory).Trim();
                string RemoveText = convert.ToString(_ItemDetails.RemoveText).Trim();
                switch (sType)
                {
                    case "ITEM_DATA":
                        _result = Get_ITEM_DATA_Value(_ItemDetails,Rowno);
                        break;
                    case "ITEM_DYNAMIC_CELL_DATA":
                        _result = Get_ITEM_DYNAMIC_CELL_DATA_Value(_ItemDetails, Rowno);
                        break;

                }
                if (RemoveText.Trim() != "")
                {
                    string[] _lstRemovetxt = RemoveText.Split('|');
                    foreach (string strRemove in _lstRemovetxt)
                    {
                        _result = _result.Replace(strRemove, "");
                    }
                }
                if (isRequired.Trim().ToUpper() == "TRUE")
                {
                    if (_result == "")
                    {
                        throw new Exception("Unable to get value for mandatory field " + sQualifier);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error on Get_Item_Details : " + ex.Message); 
            }
            return _result;
        }

        public string Get_ITEM_DATA_Value(IDATA _ItemDetails, int Rowno)
        {
            string _result = "";
            try
            {
                string sQualifier = convert.ToString(_ItemDetails.Qualifier);
                string ColCell = convert.ToString(_ItemDetails.Coloumn)+Rowno;
                _result = _ExcelRTN.GetExcelCell(ColCell);
            }
            catch (Exception ex)
            {
                throw new Exception("Error on Get_ITEM_DATA_Value : " + ex.Message);
            }
            return _result;
        }

        public string Get_ITEM_DYNAMIC_CELL_DATA_Value(IDATA _ItemDetails, int Rowno)
        {
            string _result = "";
            try
            {
                string sQualifier = convert.ToString(_ItemDetails.Qualifier);
                string Column = convert.ToString(_ItemDetails.Coloumn);
                string SearchText = convert.ToString(_ItemDetails.SearchText);
                int startRow = Rowno;
                int endRow = Rowno + convert.ToInt(_ItemDetails.EndRow);
                int addRow = convert.ToInt(_ItemDetails.AddRow);
                int addCol = convert.ToInt(_ItemDetails.AddColumn);
                _result = _ExcelRTN.GetCellDynamicData(startRow, endRow, Column, SearchText, addRow, addCol);
            }
            catch (Exception ex)
            {
                throw new Exception("Error on Get_ITEM_DYNAMIC_CELL_DATA_Value : " + ex.Message);
            }
            return _result;
        }

        public void Get_Footer_Details(FOOTER_MAPPINGS _fMapping)
        {
            try
            {
                foreach (var _footerMappings in _fMapping.FooterFields.Keys)
                {
                    Get_Footer_Value(_fMapping.FooterFields[convert.ToString(_footerMappings)]);
                }
            }
            catch (Exception ex)
            {
                LogText = "Error on Get_Footer_Details : " + ex.Message;
                throw ex;
            }
        }

        public void Get_Footer_Value(FDATA _FooterFields)
        {
            try
            {
                string qualifier = convert.ToString(_FooterFields.Qualifier).Trim();
                switch (qualifier)
                {
                    case "BUYER_REF":
                        BuyerRef = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Buyer Ref : " + BuyerRef;
                        break;
                    case "VESSEL":
                        VesselName = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Vessel : " + VesselName;
                        break;
                    case "DELIVERY_DATE":
                        _DeliveryDate = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Delivery Date : " + _DeliveryDate;
                        break;
                    case "EXPIRE_DATE":
                        _ExpireDate = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Expire Date : " + _ExpireDate;
                        break;
                    case "DOC_DATE":
                        Doc_Date = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Doc_Date : " + Doc_Date;
                        break;
                    case "CURRENCY":
                        Currency = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Currency : " + Currency;
                        break;
                    case "PORT":
                        PortCode = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Portcode : " + PortCode;
                        break;
                    case "PORT_NAME":
                        PortName = Get_Footer_FieldValue(_FooterFields);
                        LogText = "PortName : " + PortName;
                        break;
                    case "IMONO":
                        Imono = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Imono : " + Imono;
                        break;
                    case "HULLNO":
                        Hullno = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Hullno : " + Hullno;
                        break;
                    case "REPLY_BY_DATE":
                        ReplyDate = Get_Footer_FieldValue(_FooterFields);
                        LogText = "ReplyDate : " + ReplyDate;
                        break;
                    case "VENDOR_NAME":
                        VendorName = Get_Footer_FieldValue(_FooterFields);
                        LogText = "VendorName : " + VendorName;
                        break;
                    case "BUYER_NAME":
                        BuyerName = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Buyer Name : " + BuyerName;
                        break;
                    case "FREIGHT_COST":
                        FreightCost = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Fright Cost : " + FreightCost;
                        break;
                    case "ALLOUNCE":
                        HeaderDiscount = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Header Discount : " + HeaderDiscount;
                        break;
                    case "OTHER_COST":
                        OtherCost = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Other Cost : " + OtherCost;
                        break;
                    case "PACKAGE_COST":
                        PackageCost = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Package Cost : " + PackageCost;
                        break;
                    case "GRAND_TOTAL":
                        GrandTotal = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Grand Total : " + GrandTotal;
                        break;
                    case "ITEM_TOTAL":
                        ItemTotal = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Item Total : " + ItemTotal;
                        break;
                    case "TERMS":
                        _Terms = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Terms : " + _Terms;
                        break;
                    case "HEADER_REMARKS":
                        BuyerRemarks = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Buyer Remarks : " + BuyerRemarks;
                        break;
                    case "CONSIGNEE_NAME":
                        _ConsigneeName = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Consignee Name : " + _ConsigneeName;
                        break;
                    case "CONSIGNEE_ADDR":
                        _ConsigneAddress = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Consignee Address : " + _ConsigneAddress;
                        break;
                    case "BUYER_CONTACT_PERSON":
                        BuyerContactPerson = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Buyer Contact Person : " + BuyerContactPerson;
                        break;
                    case "BUYER_CONTACT_NO":
                        BuyerContactno = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Buyer Contact No : " + BuyerContactno;
                        break;
                    case "BUYER_EMAIL":
                        BuyerEmail = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Buyer Email : " + BuyerEmail;
                        break;
                    case "SUPPLIER_EMAIL":
                        SupplierEmail = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Supplier Email : " + SupplierEmail;
                        break;
                    case "SUPPLIER_REF":
                        _SupplierRef = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Supplier Ref : " + _SupplierRef;
                        break;
                    case "BILL_NAME":
                        BillName = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Bill Name : " + BillName;
                        break;
                    case "BILL_ADDR":
                        BillAddresss = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Bill Addr : " + BillAddresss;
                        break;
                    case "BUYER_ADDR":
                        BuyerAddress = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Buyer Address : " + BuyerAddress;
                        break;
                    case "HEADER_EQUIPMENT_NAME":
                        _hEquipName = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Header Equipment Name : " + _hEquipName;
                        break;
                    case "HEADER_EQUIPMENT_DESC":
                        _hEquipDescription = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Header Equipment Desc : " + _hEquipDescription;
                        break;
                    case "HEADER_EQUIPMENT_MODEL":
                        _hModel = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Header Equipment Model : " + _hModel;
                        break;
                    case "HEADER_EQUIPMENT_MANUFACTURE":
                        _hManufature = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Header Equipment Manufature : " + _hManufature;
                        break;
                    case "HEADER_EQUIPMENT_NO":
                        _hEquipno = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Header Equipment No : " + _hEquipno;
                        break;
                    case "HEADER_EQUIPMENT_DRAWING":
                        _hDrawingNo = Get_Footer_FieldValue(_FooterFields);
                        LogText = "Header Equipment Drawing : " + _hDrawingNo;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Get_Footer_FieldValue( FDATA _FooterMappings)
        {
            string result = "";
            try
            {
                string type = convert.ToString(_FooterMappings.Type).Trim();
                string isRequired = convert.ToString(_FooterMappings.IsMandataory).Trim();
                string qualifier = convert.ToString(_FooterMappings.Qualifier).Trim();
                string RemoveText = convert.ToString(_FooterMappings.RemoveText).Trim();
                result = Get_Dynamic_Footer_Field_Value(_FooterMappings);  
                
                if (RemoveText.Trim() != "")
                {
                    string[] _lstRemovetxt = RemoveText.Split('|');
                    foreach (string strRemove in _lstRemovetxt)
                    {
                        result = result.Replace(strRemove, "");
                    }
                }
                if (isRequired.Trim().ToUpper() == "TRUE")
                {
                    if (result == "")
                    {
                        throw new Exception("Unable to get value for mandatory field " + qualifier);
                    }
                }
            }
            catch (Exception ex)
            {
                LogText = "Error on Get_Footer_FieldValue : " + ex.Message;
                throw ex;
            }
            return result;



        }

        public string Get_Dynamic_Footer_Field_Value(FDATA _FooterMappings)
        {
            string _result = "";
            try
            {
                string type = convert.ToString(_FooterMappings.Type).Trim();
                string qualifier = convert.ToString(_FooterMappings.Qualifier).Trim();
                string RemoveText = convert.ToString(_FooterMappings.RemoveText).Trim();
                int sStart = _CurrItemRow;
                int sEnd = _CurrItemRow + convert.ToInt(_FooterMappings.EndRow);
                string Col = convert.ToString(_FooterMappings.Column);
                string SearchPattern = convert.ToString(_FooterMappings.SearchText);
                int addRow = convert.ToInt(_FooterMappings.AddRow);
                int addCol = convert.ToInt(_FooterMappings.AddColumn);
                _result = _ExcelRTN.GetCellDynamicData(sStart, sEnd, Col, SearchPattern,
                    addRow, addCol);
            }
            catch (Exception ex)
            {
                throw new Exception("Error on Get_Dynamic_Footer_Field_Value : " + ex.Message + " Stack trace : " + ex.StackTrace);
            }
            return _result;
        }

        public DateTimePeriodCollection GetDates()
        {
            DateTimePeriodCollection obj = new DateTimePeriodCollection();
            // RFQ Date
            try
            {
                if (Doc_Date != null && Doc_Date != "")
                {
                    DateTime _date = Convert.ToDateTime(Doc_Date);
                    DateTime dt = StringToValidDate(_date);
                    if (dt != DateTime.MinValue) obj.Add(new DateTimePeriod(DateTimeFormatQualifiers.CCYYMMDD_102, DateTimePeroidQualifiers.DocumentDate_137, dt.ToString("yyyyMMdd")));
                }

                // Late Date
                if (_DeliveryDate != null && _DeliveryDate != "")
                {
                    DateTime _date = Convert.ToDateTime(_DeliveryDate);
                    DateTime dt = StringToValidDate(_date);
                    if (dt != DateTime.MinValue) obj.Add(new DateTimePeriod(DateTimeFormatQualifiers.CCYYMMDD_102, DateTimePeroidQualifiers.LatestDeliveryDate_2, dt.ToString("yyyyMMdd")));
                }

                if (_ExpireDate != null && _ExpireDate != "")
                {
                    DateTime _date = Convert.ToDateTime(_ExpireDate);
                    DateTime dt = StringToValidDate(_date);
                    if (dt != DateTime.MinValue) obj.Add(new DateTimePeriod(DateTimeFormatQualifiers.CCYYMMDD_102, DateTimePeroidQualifiers.ExpiryDate_36, dt.ToString("yyyyMMdd")));
                }

                if (ReplyDate != null && ReplyDate != "")
                {
                    DateTime _date = Convert.ToDateTime(ReplyDate);
                    DateTime dt = StringToValidDate(_date);
                    if (dt != DateTime.MinValue) obj.Add(new DateTimePeriod(DateTimeFormatQualifiers.CCYYMMDD_102, DateTimePeroidQualifiers.AdviseDate_175, dt.ToString("yyyyMMdd")));
                }
            }
            catch (Exception ex)
            {
                LogText = ("Unable To GetDates for Buyer Ref : " + BuyerRef + " Error Message : " + ex.Message + " Stacktrace : " + ex.StackTrace);
                throw new Exception("Unable to GetDates for Buyer Ref : " + BuyerRef + " Error Message : " + ex.Message);
            }
            return obj;
        }

        public DateTime StringToValidDate(object _date)
        {
            DateTime dt = DateTime.MinValue;
            try
            {
                try
                {
                    dt = Convert.ToDateTime(_date);
                }
                catch { }

                if (dt == DateTime.MinValue)
                {
                    try
                    {
                        dt = DateTime.ParseExact(_date.ToString(), "dd/MM/yyyy", null);
                    }
                    catch { }
                }

                if (dt == DateTime.MinValue)
                {
                    try
                    {
                        dt = DateTime.ParseExact(_date.ToString(), "dd-MM-yyyy", null);
                    }
                    catch { }
                }

                if (dt == DateTime.MinValue)
                {
                    try
                    {
                        dt = DateTime.ParseExact(_date.ToString(), "dd.MM.yyyy", null);
                    }
                    catch { }
                }
                return dt;
            }
            catch
            {
                dt = DateTime.MinValue;
            }
            return dt;
        }

        public ReferenceCollection GetReferences()
        {
            string VRNO = "";
            ReferenceCollection obj = new ReferenceCollection();
            try
            {
                if (BuyerRef != null && BuyerRef != "")
                {
                    VRNO = BuyerRef;
                    if (VRNO.Trim() != "") obj.Add(new Reference(ReferenceQualifier.UC, VRNO));
                }
                if (BuyerRef != null && BuyerRef != "" && _SupplierRef != null && _SupplierRef != "")
                {
                    if (_SupplierRef.Trim() != "") obj.Add(new Reference(ReferenceQualifier.AAG, _SupplierRef));
                }

            }
            catch (Exception ex)
            {
                LogText = ("Unable To GetReferences for Buyer Ref : " + BuyerRef + " Error Message : " + ex.Message + " Stacktrace : " + ex.StackTrace);
                throw new Exception("Unable to GetReferences for Buyer Ref : " + BuyerRef + " Error Message : " + ex.Message);
            }
            return obj;
        }

        private PartyCollection GetPartyAddress()
        {
            PartyCollection obj = new PartyCollection();
            try
            {
                #region  // Buyer //
                Party _buyer = new Party();
                _buyer.Qualifier = PartyQualifier.BY;
                _buyer.Name = convert.ToString(BuyerName);
                _buyer.Identification = BuyerCode;
               _buyer.Contacts.Add(new Contact());
                if (convert.ToString(BuyerContactPerson) != "") _buyer.Contacts[0].Name = convert.ToString(BuyerContactPerson);
                if (convert.ToString(BuyerAddress) != "") _buyer.StreetAddress1 = convert.ToString(BuyerAddress);
                if (convert.ToString(BuyerContactno) != "") _buyer.Contacts[0].CommunMethodList.Add(new CommunicationMethods(convert.ToString(BuyerContactno).Trim(), CommunicationMethodQualifiers.TE));
                if (convert.ToString(BuyerEmail) != "") _buyer.Contacts[0].CommunMethodList.Add(new CommunicationMethods(convert.ToString(BuyerEmail).Trim(), CommunicationMethodQualifiers.EM));
                //}
                obj.Add(_buyer);
                #endregion

                #region // Supplier //
                Party _supp = new Party();
                _supp.Qualifier = PartyQualifier.VN;
                _supp.Contacts.Add(new Contact());
                _supp.Contacts[0].FunctionCode = ContactFunction.SR;
                if (SupplierCode != null) _supp.Identification = SupplierCode;

                if (VendorName != null) _supp.Name = VendorName;
                if (SupplierEmail != null && SupplierEmail != "")
                {
                    string _sEmail = SupplierEmail;
                    if (_sEmail.Trim() != "") _supp.Contacts[0].CommunMethodList.Add(new CommunicationMethods(_sEmail.Trim(), CommunicationMethodQualifiers.EM));
                }
                obj.Add(_supp);
                #endregion

                #region // Consignie //

                Party _Consig = new Party();
                _Consig.Qualifier = PartyQualifier.CN;
                _Consig.Name = convert.ToString(_ConsigneeName);
                _Consig.StreetAddress1 = convert.ToString(_ConsigneAddress);
                obj.Add(_Consig);

                #endregion

                #region // Bill Address //

                Party BillAddr = new Party();
                BillAddr.Qualifier = PartyQualifier.BA;
                BillAddr.Name = convert.ToString(BillName);
                BillAddr.StreetAddress1 = convert.ToString(BillAddresss);
                obj.Add(BillAddr);

                #endregion

                #region // Vessel //
                Party _shp = new Party();
                _shp.Qualifier = PartyQualifier.UD;
                _shp.Name = "";
                if (VesselName != null && VesselName != "") { _shp.Name = VesselName; }
                _shp.PartyLocation = new PartyLocation();
                if (PortCode != null && PortCode != "") { _shp.PartyLocation.Port = PortCode; }
                if (PortName != null && PortName != "") { _shp.PartyLocation.Berth = PortName; }
                if (Imono != null && Imono != "") { _shp.Identification = Imono; }
                obj.Add(_shp);
                #endregion
            }
            catch (Exception ex)
            {
                LogText = ("Unable To GetPartyAddress for Buyer Ref : " + BuyerRef + " Error Message : " + ex.Message + " Stacktrace : " + ex.StackTrace);
                throw new Exception("Unable to GetPartyAddress for Buyer Ref : " + BuyerRef + " Error Message : " + ex.Message);
            }
            return obj;
        }

        private LineItemCollection GetItems()
        {
            LineItemCollection obj = new LineItemCollection();
            try
            {
                for (int i = 0; i < (_ItemDetails.Count); i++)
                {
                    LineItem _item = new LineItem();
                   // _item.SYS_ITEMNO = (i + 1);
                    if (_item.LineItemComment == null)
                    {
                        _item.LineItemComment = new Comments();
                        _item.LineItemComment.Qualifier = CommentTypes.LIN;
                    }

                    var _itemData = _ItemDetails.ElementAt(i);
                    string ItemKey = convert.ToString(_itemData.Key);
                    //string sItemData = _itemData.Value;
                   // string[] sItemList = sItemData.Split('~');
                    List<string> sItemData = _itemData.Value;
                    
                    //if (sItemList.Length == 17)
                    if (sItemData.Count == 17)
                    {
                        string eItemNo = sItemData[0],
                            eItemDesc = sItemData[1],
                            eItemPartNo = sItemData[2],
                            eItemEquiDetail = sItemData[3],
                            eItemMaker = sItemData[4],
                            eItemDrawingNum = sItemData[5],
                            eItemModelNum = sItemData[6],
                            eItemQty = sItemData[7],
                            eItemUOM = sItemData[8],
                            eItemComment = sItemData[9],
                            eItemEquiType = sItemData[10],
                            eItemUnitPrice = sItemData[11],
                            eItemUnitDis = sItemData[12],
                            eItemUnitTotal = sItemData[13],
                            eSystemItemNo = sItemData[14],
                            eItemEquipDesc = sItemData[15],
                            eItemEquipSerialNumber = sItemData[16];
                        if (ItemKey == eItemNo)
                        {
                            if (convert.ToFloat(eItemQty) > -1)
                            {
                                if (eItemNo != null && eItemNo != "") _item.Number = convert.ToString(eItemNo.Trim());
                                if (eItemDesc != null && eItemDesc != "") _item.Description = convert.ToString(eItemDesc.Trim());
                                if (eItemPartNo != null && eItemPartNo != "") _item.Identification = convert.ToString(eItemPartNo.Trim());
                                if (eItemQty != null && eItemQty != "") _item.Quantity = convert.ToFloat(eItemQty);
                                if (eItemUOM != null && eItemUOM != "") _item.MeasureUnitQualifier = convert.ToString(eItemUOM.Trim());
                                if (eItemComment != null && eItemComment != "") _item.LineItemComment.Value = convert.ToString(eItemComment.Trim());
                                if (eItemUnitPrice != null) { _item.PriceList.Add(new PriceDetails(PriceDetailsTypeCodes.Quoted_QT, PriceDetailsTypeQualifiers.GRP, convert.ToFloat(eItemUnitPrice))); _item.PriceList.Add(new PriceDetails(PriceDetailsTypeCodes.Quoted_QT, PriceDetailsTypeQualifiers.LIST, convert.ToFloat(eItemUnitPrice))); }
                                if (eItemUnitDis != null) { _item.PriceList.Add(new PriceDetails(PriceDetailsTypeCodes.Quoted_QT, PriceDetailsTypeQualifiers.DPR, convert.ToFloat(eItemUnitDis))); }
                                if (eItemUnitTotal != null && eItemUnitTotal != "") _item.MonetaryAmount = convert.ToFloat(eItemUnitTotal);
                                if (eSystemItemNo != null && eSystemItemNo != "") _item.SYS_ITEMNO = convert.ToInt(eSystemItemNo);

                                Section _section = new Section();
                                if (eItemEquiDetail != null) _section.Name = convert.ToString(eItemEquiDetail);
                                if (eItemEquiType != null) _section.DepartmentType = convert.ToString(eItemEquiType);
                                if (eItemDrawingNum != null) _section.DrawingNumber = convert.ToString(eItemDrawingNum);
                                if (eItemMaker != null) _section.Manufacturer = convert.ToString(eItemMaker);
                                if (eItemModelNum != null) _section.ModelNumber = convert.ToString(eItemModelNum);
                                if (eItemEquipDesc != null) _section.Description = convert.ToString(eItemEquipDesc);
                                if (eItemEquipSerialNumber != null) _section.SerialNumber = convert.ToString(eItemEquipSerialNumber);
                                _item.Section = _section;
                                _item.OriginatingSystemRef = _item.Number;
                                obj.Add(_item);
                            }
                            else
                            {
                                throw new Exception("Unable to get Item. Item Qty is " + convert.ToString(eItemQty));
                            }
                        }
                        else
                        {
                            throw new Exception("Unable to get Item. Item Key " + ItemKey + " and Item no mismatch");
                        }
                    }
                    else
                    {
                        throw new Exception("Unable to get Item Format");
                    }

                }
            }
            catch (Exception ex)
            {
                LogText = ("Unable To GetItems for BuyerRef : " + BuyerRef + " Error Message : " + ex.Message + " Stacktrace : " + ex.StackTrace);
                throw new Exception("Unable to GetItems for BuyerRef : " + BuyerRef + " Error Message : " + ex.Message);
            }
            return obj;
        }

        private MonetaryAmountCollection GetMonetoryAmounts()
        {
            MonetaryAmountCollection obj = new MonetaryAmountCollection();
            try
            {
                if (PackageCost != null && PackageCost != "") { obj.Add(new MonetaryAmount(MonetoryAmountQualifier.PackingCost_106, convert.ToFloat(PackageCost))); }
                if (FreightCost != null && FreightCost != "") { obj.Add(new MonetaryAmount(MonetoryAmountQualifier.FreightCharge_64, convert.ToFloat(FreightCost))); }
                if (ItemTotal != null && ItemTotal != "") { obj.Add(new MonetaryAmount(MonetoryAmountQualifier.TotalLineItemsAmount_79, convert.ToFloat(ItemTotal))); }
                if (GrandTotal != null && GrandTotal != "") { obj.Add(new MonetaryAmount(MonetoryAmountQualifier.GrandTotal_259, convert.ToFloat(GrandTotal))); }
                if (HeaderDiscount != null && HeaderDiscount != "") { obj.Add(new MonetaryAmount(MonetoryAmountQualifier.AllowanceAmount_204, convert.ToFloat(HeaderDiscount))); }
            }
            catch (Exception ex)
            {
                LogText = ("Unable To GetMonetoryAmounts for BuyerRef : " + BuyerRef + " Error Message : " + ex.Message + " Stacktrace : " + ex.StackTrace);
                throw new Exception("Unable to GetMonetoryAmounts for BuyerRef : " + BuyerRef + " Error Message : " + ex.Message);
            }
            return obj;
        }

        private Equipment GetEquipments()
        {
            Equipment _obj = new Equipment();
            try
            {
                _obj.Manufacturer = _hManufature;
                _obj.ModelNumber = _hModel;
                _obj.Name = _hEquipName;
                _obj.SerialNumber = _hEquipno;
                _obj.DrawingNumber = _hDrawingNo;
                _obj.Description = _hEquipDescription;
            }
            catch (Exception ex)
            {
                LogText = ("Unable To GetEquipments for BuyerRef : " + BuyerRef + " Error Message : " + ex.Message + " Stacktrace : " + ex.StackTrace);
                throw new Exception("Unable to GetEquipments for BuyerRef : " + BuyerRef + " Error Message : " + ex.Message);
            }
            return _obj;
        }

        public bool GenerateRFQMTML()
        {
            bool _result = false;
            try
            {
                string RFQFileName = "";
                string TermsCondn = "";
                MTMLInterchange _interChange = new MTMLInterchange();
                DocHeader _docHeader = new DocHeader();

                if (BuyerRef != null && BuyerRef != "") { }
                else throw new Exception("Element RFQ No. not found.");
                {
                    #region // Interchange //
                    _interChange.PreparationDate = DateTime.Now.ToString("yyyy-MMM-dd");
                    _interChange.PreparationTime = DateTime.Now.ToString("HH:mm");
                    _interChange.VersionNumber = "1";
                    _interChange.Identifier = "UNOC";
                    _interChange.Sender = BuyerCode;
                    _interChange.Recipient = SupplierCode;
                    if (_interChange.BuyerSuppInfo == null) _interChange.BuyerSuppInfo = new BuyerSupplierInfo();
                    _interChange.BuyerSuppInfo.REF_No = BuyerRef;
                    _interChange.BuyerSuppInfo.GroupID = convert.ToInt(ConfigurationManager.AppSettings["GROUPID"]);
                    //_interChange.BuyerSuppInfo.GroupID = convert.ToInt(GroupID); // SET GROUP ID                              
                    #endregion

                    // DocHeader //
                    _docHeader.DocType = "RequestForQuote";
                    _docHeader.FunctionCode = TradingMessageFunctions.Original_9;
                    _docHeader.VersionNumber = "1";
                    _docHeader.BuyerRefNo = BuyerRef;
                    _docHeader.MessageReferenceNumber = Process_File_Name;//"RequestForQuote : " + BuyerRef;//28-08-2017
                    _docHeader.MessageNumber = Process_File_Name;// BuyerRef;//28-08-2017
                    _docHeader.OriginalFile = Process_File_Name;

                    if (Currency.Length >= 3) Currency = Currency.Substring(0, 3);
                    _docHeader.CurrencyCode = Currency;

                    _docHeader.DateTimePeriods = GetDates(); // DateTimePeriod
                    _docHeader.References = GetReferences(); // References
                    _docHeader.PartyAddresses = GetPartyAddress(); // Party Address
                    _docHeader.Equipment = GetEquipments(); // Equipments

                    #region /*Comments*/
                    string ByrComments = "";

                    if (BuyerRemarks != null && BuyerRemarks != "") { ByrComments += BuyerRemarks + Environment.NewLine; }
                    _docHeader.Comments.Add(new Comments(CommentTypes.PUR, ByrComments.Trim()));

                    //// Terms & Conditions
                    if (_Terms != null) { TermsCondn += convert.ToString(_Terms); }
                    _docHeader.Comments.Add(new Comments(CommentTypes.ZTC, TermsCondn.Trim()));
                    #endregion

                    //// Items //
                    _docHeader.LineItems = GetItems();
                    _docHeader.LineItemCount = _docHeader.LineItems.Count;
                    _interChange.DocumentHeader = _docHeader;

                    string exportFile = MTML_Path + "\\" + convert.ToFileName("RFQ_" + BuyerCode + "_" + SupplierCode + "_" + BuyerRef + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".xml");
                    if (!Directory.Exists(MTML_Path)) { Directory.CreateDirectory(MTML_Path); }

                    if (_interChange.Sender != "" && _interChange.Recipient != "")
                    {
                        MTMLClass _mtmlClass = new MTMLClass();
                        _mtmlClass.Create(_interChange, exportFile);

                        if (File.Exists(exportFile))
                        {
                            RFQFileName = Path.GetFileName(exportFile);
                           // string filename = convert.ToFileName(ProcessorName + "_" + BuyerRef + "_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".pdf");
                            LogText = "RFQ file '" + RFQFileName + "' with Buyer Ref '" + BuyerRef + "' downloaded successfully.";
                            CreateAuditFile(Path.GetFileName(Process_File_Name), ProcessorName, BuyerRef, "Converted", "RFQ file '" + RFQFileName + "' with Buyer Ref '" + BuyerRef + "' downloaded successfully.");
                            _result = true;
                        }
                    }
                    else
                    {
                        if (_interChange.Sender == "")
                        {
                            LogText = "Unable to create MTML file for Buyer Ref '" + BuyerRef + "' as Sender is blank.";//08-09-17
                            CreateAuditFile(Path.GetFileName(Process_File_Name), ProcessorName, BuyerRef, "Error", "Unable to create MTML file for Buyer Ref '" + BuyerRef + "' as Sender is blank."); }
                        else if (_interChange.Recipient == "")
                        {
                            LogText = "Unable to create MTML file for Buyer Ref '" + BuyerRef + "' as Recipient is blank.";//08-09-17
                            CreateAuditFile(Path.GetFileName(Process_File_Name), ProcessorName, BuyerRef, "Error", "Unable to create MTML file for Buyer Ref '" + BuyerRef + "' as Recipient is blank."); }
                        _result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogText = ("Unable to Generate RFQ MTML for BuyerRef : " + BuyerRef + " Error Message : " + ex.Message + " Stacktrace : " + ex.StackTrace);
                throw new Exception("Unable to Generate RFQ MTML for BuyerRef : " + BuyerRef + " Error Message : " + ex.Message + " Stacktrace : " + ex.StackTrace);
            }
            return _result;
        }

        public bool GeneratePOMTML()
        {
            bool _result = false;
            string POFileName = "";
            try
            {
                MTMLInterchange _interChange = new MTMLInterchange();
                DocHeader _docHeader = new DocHeader();

                if (BuyerRef != null && BuyerRef != "") { }
                else throw new Exception("Element Order No. not found.");
                {
                    #region // Interchange //
                    _interChange.PreparationDate = DateTime.Now.ToString("yyyy-MMM-dd");
                    _interChange.PreparationTime = DateTime.Now.ToString("HH:mm");
                    _interChange.VersionNumber = "1";
                    _interChange.Identifier = "UNOC";
                    _interChange.Sender = BuyerCode;
                    _interChange.Recipient = SupplierCode;
                    if (_interChange.BuyerSuppInfo == null) _interChange.BuyerSuppInfo = new BuyerSupplierInfo();
                    _interChange.BuyerSuppInfo.REF_No = BuyerRef;
                    //_interChange.BuyerSuppInfo.GroupID = convert.ToInt(GroupID); // SET GROUP ID                              
                    #endregion

                    // DocHeader //
                    _docHeader.DocType = "Order";
                    _docHeader.FunctionCode = TradingMessageFunctions.Original_9;
                    _docHeader.VersionNumber = "1";
                    _docHeader.BuyerRefNo = BuyerRef;
                    _docHeader.MessageReferenceNumber = "Order : " + BuyerRef;
                    _docHeader.MessageNumber = BuyerRef;
                    _docHeader.OriginalFile =Path.GetFileName(Process_File_Name);

                    if (Currency.Length >= 3) Currency = Currency.Substring(0, 3);
                    _docHeader.CurrencyCode = Currency;

                    _docHeader.DateTimePeriods = GetDates(); // DateTimePeriod
                    _docHeader.References = GetReferences(); // References
                    _docHeader.PartyAddresses = GetPartyAddress(); // Party Address
                    _docHeader.Equipment = GetEquipments(); // Equimentnts

                    #region /*Comments*/
                    string ByrComments = "";
                    if (BuyerRemarks != null) { ByrComments += BuyerRemarks; }
                    _docHeader.Comments.Add(new Comments(CommentTypes.PUR, ByrComments.Trim()));
                    
                    #endregion

                    //// Items //
                    _docHeader.LineItems = GetItems();
                    _docHeader.LineItemCount = _docHeader.LineItems.Count;

                    _interChange.DocumentHeader = _docHeader;
                    _interChange.DocumentHeader.MonetoryAmounts = GetMonetoryAmounts();
                    _interChange.DocumentHeader.AdditionalDiscount = convert.ToFloat(HeaderDiscount);

                    string exportFile = MTML_Path + "\\" + convert.ToFileName("PO_" + BuyerCode + "_" + SupplierCode + "_" + BuyerRef + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".xml");
                    if (!Directory.Exists(MTML_Path)) { Directory.CreateDirectory(MTML_Path); }

                    MTMLClass _mtmlClass = new MTMLClass();
                    _mtmlClass.Create(_interChange, exportFile);

                    if (File.Exists(exportFile))
                    {
                        POFileName = Path.GetFileName(exportFile);
                       
                        LogText = "Order file '" + POFileName + "' with Order Ref '" + BuyerRef + "' downloaded successfully.";
                        CreateAuditFile(Path.GetFileName(Process_File_Name), ProcessorName, BuyerRef, "Downloaded", "Order file '" + POFileName + "' with Order Ref '" + BuyerRef + "' downloaded successfully.");
                        _result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogText = ("Unable to Generate PO MTML for BuyerRef : " + BuyerRef + " Error Message : " + ex.Message + " Stacktrace : " + ex.StackTrace);
                throw new Exception("Unable to Generate PO MTML for BuyerRef : " + BuyerRef + " Error Message : " + ex.Message + " Stacktrace : " + ex.StackTrace);
            }
            return _result;
        }

        public void CreateAuditFile(string FileName, string Module, string RefNo, string LogType, string Audit)
        {
            try
            {
                if (BuyerCode == null || BuyerCode == "") BuyerCode = convert.ToString(ConfigurationSettings.AppSettings["BUYER_CODE"]);
                if (SupplierCode == null || SupplierCode == "") SupplierCode = convert.ToString(ConfigurationSettings.AppSettings["SUPPLIER_CODE"]);

                string auditPath = convert.ToString(ConfigurationSettings.AppSettings["AUDIT_PATH"]);
                if (auditPath == "") { auditPath = AppDomain.CurrentDomain.BaseDirectory + "Audit_Path"; }
                if (!Directory.Exists(auditPath)) Directory.CreateDirectory(auditPath);
                string auditData = "";
                if (auditData.Trim().Length > 0) auditData += Environment.NewLine;
                auditData += BuyerCode + "|";
                auditData += SupplierCode + "|";
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
                LogText = ("Error on creating Auditlog File for Ref No : '" + RefNo + "'. ERROR : " + ex.Message);
                throw new Exception("Error on creating Auditlog File for Ref No : '" + RefNo + "'. ERROR : "+ex.Message);
            }
        }

        public void MoveFiles(string Source_FilePath, string Destnation_FilePath)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(Destnation_FilePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Destnation_FilePath));
                }
                if (File.Exists(Destnation_FilePath))
                {
                    File.Delete(Destnation_FilePath);
                }
                File.Move(Source_FilePath, Destnation_FilePath);
            }
            catch (Exception ex)
            {
                LogText = ("Error on MoveFiles : " + ex.Message);
            }
        }
    }
}
