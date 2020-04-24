using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Excel_SPL_Routine
{
    public class EXCEL_Mapping
    {
        private XmlDocument _xmlDoc = null;

        public EXCEL_Config Load(string xmlFile)
        {
            try
            {
                EXCEL_Config _obj = new EXCEL_Config();
                _xmlDoc = new XmlDocument();
                _xmlDoc.Load(xmlFile);
                Set_RFQ_MappingDetails(_obj.Rfq_Mapping);
                Set_PO_MappingDetails(_obj.Po_Mapping);
                return _obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Error on Loading Invoice Config file : " + ex.Message);
            }
        }

        public void Set_RFQ_MappingDetails(RFQ_Mapping _obj)
        {
            XmlNode _xNode = _xmlDoc.SelectSingleNode("EXCEL_MAPPING");
            if (_xNode != null)
            {
                XmlNode xHeaderDetails = _xmlDoc.SelectSingleNode("EXCEL_MAPPING/RFQ_MAPPINGS/HEADER_MAPPINGS");
                if (xHeaderDetails != null)
                {
                    XmlNodeList childNodes = null;
                    childNodes = xHeaderDetails.ChildNodes;
                    foreach (XmlNode node in childNodes)
                    {
                        switch (node.LocalName)
                        {
                            case "HDATA":
                                SetHeaderDataValues(node, _obj.HeaderMappings);
                                break;
                        }
                    }
                }
                XmlNode xItemDetails = _xmlDoc.SelectSingleNode("EXCEL_MAPPING/RFQ_MAPPINGS/ITEMS_MAPPINGS");
                if (xItemDetails != null)
                {
                    XmlNodeList childNodes = null;
                    childNodes = xItemDetails.ChildNodes;
                    foreach (XmlNode node in childNodes)
                    {
                        switch (node.LocalName)
                        {
                            case "IDATA":
                                SetItemDataValues(node, _obj.ItemMappings);
                                break;
                        }
                    }
                }
                XmlNode xFooterDetails = _xmlDoc.SelectSingleNode("EXCEL_MAPPING/RFQ_MAPPINGS/FOOTER_MAPPINGS");
                if (xFooterDetails != null)
                {
                    XmlNodeList childNodes = null;
                    childNodes = xFooterDetails.ChildNodes;
                    foreach (XmlNode node in childNodes)
                    {
                        switch (node.LocalName)
                        {
                            case "FDATA":
                                SetFooterDataValues(node, _obj.FooterMappings);
                                break;
                        }
                    }
                }
            }
        }

        public void Set_PO_MappingDetails(PO_Mapping _obj)
        {
            XmlNode _xNode = _xmlDoc.SelectSingleNode("EXCEL_MAPPING");
            if (_xNode != null)
            {
                XmlNode xHeaderDetails = _xmlDoc.SelectSingleNode("EXCEL_MAPPING/PO_MAPPINGS/HEADER_MAPPINGS");
                if (xHeaderDetails != null)
                {
                    XmlNodeList childNodes = null;
                    childNodes = xHeaderDetails.ChildNodes;
                    foreach (XmlNode node in childNodes)
                    {
                        switch (node.LocalName)
                        {
                            case "HDATA":
                                SetHeaderDataValues(node, _obj.HeaderMappings);
                                break;
                        }
                    }
                }
                XmlNode xItemDetails = _xmlDoc.SelectSingleNode("EXCEL_MAPPING/PO_MAPPINGS/ITEMS_MAPPINGS");
                if (xItemDetails != null)
                {
                    XmlNodeList childNodes = null;
                    childNodes = xItemDetails.ChildNodes;
                    foreach (XmlNode node in childNodes)
                    {
                        switch (node.LocalName)
                        {
                            case "IDATA":
                                SetItemDataValues(node, _obj.ItemMappings);
                                break;
                        }
                    }
                }
                XmlNode xFooterDetails = _xmlDoc.SelectSingleNode("EXCEL_MAPPING/PO_MAPPINGS/FOOTER_MAPPINGS");
                if (xFooterDetails != null)
                {
                    XmlNodeList childNodes = null;
                    childNodes = xFooterDetails.ChildNodes;
                    foreach (XmlNode node in childNodes)
                    {
                        switch (node.LocalName)
                        {
                            case "FDATA":
                                SetFooterDataValues(node, _obj.FooterMappings);
                                break;
                        }
                    }
                }
            }
        }

        public void SetValidateDoctype(XmlNode _Node, Validate_Doctype obj)
        {
            string _type = "";
            VDATA _vdata = new VDATA();
            if (_Node.Attributes["TYPE"] != null) _type = convert.ToString(_Node.Attributes["TYPE"].Value);
            if (_Node.Attributes["TYPE"] != null) { _vdata.Type = convert.ToString(_Node.Attributes["TYPE"].Value); }
            if (_Node.Attributes["RFQ_CELL"] != null) { _vdata.RFQ_Cell = convert.ToString(_Node.Attributes["RFQ_CELL"].Value); }
            if (_Node.Attributes["RFQ_VALUE"] != null) { _vdata.RFQ_Val = convert.ToString(_Node.Attributes["RFQ_VALUE"].Value); }
            if (_Node.Attributes["PO_CELL"] != null) { _vdata.PO_Cell = convert.ToString(_Node.Attributes["PO_CELL"].Value); }
            if (_Node.Attributes["PO_VALUE"] != null) { _vdata.PO_Val = convert.ToString(_Node.Attributes["PO_VALUE"].Value); }
            obj.ValidateDocType = _vdata;
        
        }

        public void SetHeaderDataValues(XmlNode _Node, HEADER_MAPPINGS obj)
        {
            if (_Node != null)
            {
                string _type = "";
                HDATA _hdata = new HDATA();
                if (_Node.Attributes["TYPE"] != null) _type = convert.ToString(_Node.Attributes["TYPE"].Value);

                if (_Node.Attributes["TYPE"] != null) { _hdata.Type = convert.ToString(_Node.Attributes["TYPE"].Value); }
                if (_Node.Attributes["QUALIFIER"] != null) { _hdata.Qualifier = convert.ToString(_Node.Attributes["QUALIFIER"].Value); }
                if (_Node.Attributes["VALIDATE_CELL"] != null) { _hdata.ValidateCell = convert.ToString(_Node.Attributes["VALIDATE_CELL"].Value); }
                if (_Node.Attributes["VALIDATE_VALUE"] != null) { _hdata.ValidateValue = convert.ToString(_Node.Attributes["VALIDATE_VALUE"].Value); }

                if (_Node.Attributes["BUYER_VALIDATE"] != null)
                { _hdata.Buyer_Val = convert.ToString(_Node.Attributes["BUYER_VALIDATE"].Value);  }
                if (_Node.Attributes["BUYER_CELL"] != null) { _hdata.Buyer_Cell = convert.ToString(_Node.Attributes["BUYER_CELL"].Value); }
                if (_Node.Attributes["BUYER_CODE"] != null) { _hdata.Buyer_Code = convert.ToString(_Node.Attributes["BUYER_CODE"].Value); }
                if (_Node.Attributes["SUPPLIER_CELL"] != null) { _hdata.Supplier_Cell = convert.ToString(_Node.Attributes["SUPPLIER_CELL"].Value); }
                if (_Node.Attributes["SUPPLIER_VALIDATE"] != null) { _hdata.Supplier_Val = convert.ToString(_Node.Attributes["SUPPLIER_VALIDATE"].Value); }
                if (_Node.Attributes["SUPPLIER_CODE"] != null) { _hdata.Supplier_Code = convert.ToString(_Node.Attributes["SUPPLIER_CODE"].Value); }
                if (_Node.Attributes["REMOVE_TEXT"] != null) { _hdata.RemoveText = convert.ToString(_Node.Attributes["REMOVE_TEXT"].Value); }
                if (_Node.Attributes["CELL"] != null) { _hdata.FieldCell = convert.ToString(_Node.Attributes["CELL"].Value); }
                if (_Node.Attributes["START"] != null) { _hdata.StartRow = convert.ToString(_Node.Attributes["START"].Value); }
                if (_Node.Attributes["END"] != null) { _hdata.EndRow = convert.ToString(_Node.Attributes["END"].Value); }
                if (_Node.Attributes["COL"] != null) { _hdata.Column = convert.ToString(_Node.Attributes["COL"].Value); }
                if (_Node.Attributes["SEARCH_VALUE"] != null) { _hdata.SearchText = convert.ToString(_Node.Attributes["SEARCH_VALUE"].Value); }
                if (_Node.Attributes["ADD_ROW"] != null) { _hdata.AddRow = convert.ToString(_Node.Attributes["ADD_ROW"].Value); }
                if (_Node.Attributes["ADD_COL"] != null) { _hdata.AddColumn = convert.ToString(_Node.Attributes["ADD_COL"].Value); }
                if (_Node.Attributes["MANDATORY"] != null) { _hdata.IsMandataory = convert.ToString(_Node.Attributes["MANDATORY"].Value); }

                switch (_type)
                {
                    case "VALIDATE":
                        obj.Validate.Add(_hdata);
                        break;
                    case "VALIDATE_BUYER":
                        obj.Validate_Buyer = (_hdata);
                        break;
                    case "VALIDATE_SUPPLIER":
                        obj.Validate_Supplier = (_hdata);
                        break;
                    case "HEAER_DATA":
                        obj.Fields.Add(_hdata);
                        break;
                    case "HEAER_DYNAMIC_CELL_DATA":
                        obj.Fields.Add(_hdata);
                        break;
                }
            }
        }

        public void SetItemDataValues(XmlNode _Node, ITEM_MAPPINGS obj)
        {
            try
            {
                if (_Node != null)
                {
                    string _type = "";
                    IDATA _Idata = new IDATA();
                    if (_Node.Attributes["TYPE"] != null) _type = convert.ToString(_Node.Attributes["TYPE"].Value);

                    if (_Node.Attributes["TYPE"] != null) { _Idata.Type = convert.ToString(_Node.Attributes["TYPE"].Value); }
                    if (_Node.Attributes["QUALIFIER"] != null) { _Idata.Qualifier = convert.ToString(_Node.Attributes["QUALIFIER"].Value); }

                    if (_Node.Attributes["ITEM_START_ROW_NO"] != null) { _Idata.ItemStartRow = convert.ToString(_Node.Attributes["ITEM_START_ROW_NO"].Value); }
                    if (_Node.Attributes["SEARCH_COL"] != null) { _Idata.ItemEndCol = convert.ToString(_Node.Attributes["SEARCH_COL"].Value); }
                    if (_Node.Attributes["SEARCH_VALUE"] != null) { _Idata.ItemEndSearchText = convert.ToString(_Node.Attributes["SEARCH_VALUE"].Value); }

                    if (_Node.Attributes["REMOVE_TEXT"] != null) { _Idata.RemoveText = convert.ToString(_Node.Attributes["REMOVE_TEXT"].Value); }

                    if (_Node.Attributes["START"] != null) { _Idata.StartRow = convert.ToString(_Node.Attributes["START"].Value); }
                    if (_Node.Attributes["END"] != null) { _Idata.EndRow = convert.ToString(_Node.Attributes["END"].Value); }
                    if (_Node.Attributes["COL"] != null) { _Idata.Coloumn = convert.ToString(_Node.Attributes["COL"].Value); }
                    if (_Node.Attributes["SEARCH_VALUE"] != null) { _Idata.SearchText = convert.ToString(_Node.Attributes["SEARCH_VALUE"].Value); }
                    if (_Node.Attributes["ADD_ROW"] != null) { _Idata.AddRow = convert.ToString(_Node.Attributes["ADD_ROW"].Value); }
                    if (_Node.Attributes["ADD_COL"] != null) { _Idata.AddColumn = convert.ToString(_Node.Attributes["ADD_COL"].Value); }
                    if (_Node.Attributes["MANDATORY"] != null) { _Idata.IsMandataory = convert.ToString(_Node.Attributes["MANDATORY"].Value); }
                    if (_Node.Attributes["IS_ITEM_REMARKS"] != null) { _Idata.IsRemarks = convert.ToString(_Node.Attributes["IS_ITEM_REMARKS"].Value); }
                    switch (_type)
                    {
                        case "ITEM_START":
                            obj.ItemStart = (_Idata);
                            break;
                        case "ITEM_END":
                            obj.ItemEnd = (_Idata);
                            break;
                        case "ITEM_DATA":
                            obj.ItemFields.Add(_Idata.Qualifier, _Idata);
                            break;
                        case "ITEM_DYNAMIC_CELL_DATA":
                            obj.ItemFields.Add(_Idata.Qualifier, _Idata);
                            break;
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        public void SetFooterDataValues(XmlNode _Node, FOOTER_MAPPINGS obj)
        {
            if (_Node != null)
            {
                string _type = "";
                FDATA _fdata = new FDATA();
                if (_Node.Attributes["TYPE"] != null) _type = convert.ToString(_Node.Attributes["TYPE"].Value);

                if (_Node.Attributes["TYPE"] != null) { _fdata.Type = convert.ToString(_Node.Attributes["TYPE"].Value); }
                if (_Node.Attributes["QUALIFIER"] != null) { _fdata.Qualifier = convert.ToString(_Node.Attributes["QUALIFIER"].Value); }

                if (_Node.Attributes["REMOVE_TEXT"] != null) { _fdata.RemoveText = convert.ToString(_Node.Attributes["REMOVE_TEXT"].Value); }

                if (_Node.Attributes["START"] != null) { _fdata.StartRow = convert.ToString(_Node.Attributes["START"].Value); }
                if (_Node.Attributes["END"] != null) { _fdata.EndRow = convert.ToString(_Node.Attributes["END"].Value); }
                if (_Node.Attributes["COL"] != null) { _fdata.Column = convert.ToString(_Node.Attributes["COL"].Value); }
                if (_Node.Attributes["SEARCH_VALUE"] != null) { _fdata.SearchText = convert.ToString(_Node.Attributes["SEARCH_VALUE"].Value); }
                if (_Node.Attributes["ADD_ROW"] != null) { _fdata.AddRow = convert.ToString(_Node.Attributes["ADD_ROW"].Value); }
                if (_Node.Attributes["ADD_COL"] != null) { _fdata.AddColumn = convert.ToString(_Node.Attributes["ADD_COL"].Value); }
                if (_Node.Attributes["MANDATORY"] != null) { _fdata.IsMandataory = convert.ToString(_Node.Attributes["MANDATORY"].Value); }

                switch (_type)
                {
                    case "FOOTER_DYNAMIC_CELL_DATA":
                        obj.FooterFields.Add(_fdata.Qualifier, _fdata);
                        break;
                }
            }
        }
    }

    public class EXCEL_Config
    {
        public Validate_Doctype DocType = new Validate_Doctype();
        public RFQ_Mapping Rfq_Mapping = new RFQ_Mapping();
        public PO_Mapping Po_Mapping = new PO_Mapping();
    }

    public class RFQ_Mapping
    {
        public HEADER_MAPPINGS HeaderMappings = new HEADER_MAPPINGS();
        public ITEM_MAPPINGS ItemMappings = new ITEM_MAPPINGS();
        public FOOTER_MAPPINGS FooterMappings = new FOOTER_MAPPINGS();
    }

    public class PO_Mapping
    {
        public HEADER_MAPPINGS HeaderMappings = new HEADER_MAPPINGS();
        public ITEM_MAPPINGS ItemMappings = new ITEM_MAPPINGS();
        public FOOTER_MAPPINGS FooterMappings = new FOOTER_MAPPINGS();
    }

    public class Validate_Doctype
    {
        public VDATA ValidateDocType = new VDATA();
    }

    public class VDATA
    {
        public string Type { get; set; }
        public string RFQ_Cell { get; set; }
        public string RFQ_Val { get; set; }
        public string PO_Cell { get; set; }
        public string PO_Val { get; set; }
    }

    public class HEADER_MAPPINGS
    {
        public HDATA Validate_Supplier = new HDATA();
        public HDATA Validate_Buyer = new HDATA();
        public List<HDATA> Validate = new List<HDATA>();
        public List<HDATA> Fields = new List<HDATA>();
    }

    public class HDATA
    {
        public string Type { get; set; }
        public string Qualifier { get; set; }
        public string ValidateCell { get; set; }
        public string ValidateValue { get; set; }
        public string Buyer_Cell { get; set; }
        public string Buyer_Val { get; set; }
        public string Buyer_Code { get; set; }
        public string Supplier_Cell { get; set; }
        public string Supplier_Val { get; set; }
        public string Supplier_Code { get; set; }
        public string FieldCell { get; set; }
        public string RemoveText { get; set; }
        public string StartRow { get; set; }
        public string EndRow { get; set; }
        public string Column { get; set; }
        public string SearchText { get; set; }
        public string AddRow { get; set; }
        public string AddColumn { get; set; }
        public string IsMandataory { get; set; }
    }

    public class ITEM_MAPPINGS
    {
        public IDATA ItemStart = new IDATA();
        public IDATA ItemEnd = new IDATA();
        public Dictionary<string, IDATA> ItemFields = new Dictionary<string, IDATA>();
    }

    public class IDATA
    {
        public string Type { get; set; }
        public string ItemStartRow { get; set; }
        public string ItemEndCol { get; set; }
        public string ItemEndSearchText { get; set; }
        public string Qualifier { get; set; }
        public string RemoveText { get; set; }
        public string StartRow { get; set; }
        public string EndRow { get; set; }
        public string SearchText { get; set; }
        public string Coloumn { get; set; }
        public string AddRow { get; set; }
        public string AddColumn { get; set; }
        public string IsMandataory { get; set; }
        public string IsRemarks { get; set; }
    }

    public class FOOTER_MAPPINGS
    {
        public Dictionary<string, FDATA> FooterFields = new Dictionary<string, FDATA>();
    }

    public class FDATA
    {
        public string Type { get; set; }
        public string Qualifier { get; set; }
        public string RemoveText { get; set; }
        public string StartRow { get; set; }
        public string EndRow { get; set; }
        public string SearchText { get; set; }
        public string Column { get; set; }
        public string AddRow { get; set; }
        public string AddColumn { get; set; }
        public string IsMandataory { get; set; }
    }
}
