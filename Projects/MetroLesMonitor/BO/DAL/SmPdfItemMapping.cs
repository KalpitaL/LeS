namespace MetroLesMonitor.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;

    public partial class SmPdfItemMapping : IDisposable
    {
        private DataAccess _dataAccess;

        public SmPdfItemMapping()
        {
            this._dataAccess = new DataAccess();
        }

        public SmPdfItemMapping(DataAccess _dataAccess)
        {
            // TODO: Complete member initialization
            this._dataAccess = _dataAccess;
        }

        public virtual System.Data.DataSet SM_PDF_ITEM_MAPPING_Select_All()
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_PDF_ITEM_MAPPING_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_PDF_ITEM_MAPPING_Select_One(System.Nullable<int> ITEM_MAPID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_PDF_ITEM_MAPPING_Select_One");
            this._dataAccess.AddParameter("ITEM_MAPID", ITEM_MAPID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_PDF_ITEM_MAPPING_Select_One_By_PDF_MAPID(System.Nullable<int> PDF_MAPID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_PDF_ITEM_MAPPING_Select_One_By_PDF_MAPID");
            this._dataAccess.AddParameter("PDF_MAPID", PDF_MAPID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_PDF_ITEM_MAPPING_Select_One_By_PDFMAPID_MAPNUMBER(System.Nullable<int> PDF_MAPID, System.Nullable<int> MAP_NUMBER)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_PDF_ITEM_MAPPING_Select_One_By_PDFMAPID_MAPNUMBER");
            this._dataAccess.AddParameter("PDF_MAPID", PDF_MAPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAP_NUMBER", MAP_NUMBER, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual void SM_PDF_ITEM_MAPPING_Insert(
                   System.Nullable<int> ITEM_MAPID,
                   System.Nullable<int> PDF_MAPID,
                   System.Nullable<int> MAP_NUMBER,
                   System.Nullable<int> ITEM_HEADER_LINE_COUNT,
                   string ITEM_HEADER_CONTENT,
                   string ITEM_END_CONTENT,
                   string ITEM_EQUIPMENT,
                   string ITEM_GROUP_HEADER,
                   string ITEM_NO,
                   string ITEM_QTY,
                   string ITEM_UNIT,
                   string ITEM_DESCR,
                   string ITEM_REMARK,
                   string ITEM_REFNO,
                   string ITEM_UNITPRICE,
                   string ITEM_DISCOUNT,
                   string ITEM_LEADDAYS,
                   string ITEM_TOTAL,
                   System.Nullable<int> LEADDAYS_IN_DATE,
                   System.Nullable<int> ITEM_MIN_LINES,
                   string ITEM_REMARKS_APPEND_TEXT,
                   string ITEM_REMARKS_INITIALS,
                   System.Nullable<int> HAS_NO_EQUIP_HEADER,
                   System.Nullable<int> MAX_EQUIP_ROWS,
                   System.Nullable<int> MAX_EQUIP_RANGE,
                   System.Nullable<int> CHECK_CONTENT_BELOW_ITEM,
                   string EXTRA_COLUMNS,
                   string EXTRA_COLUMNS_HEADER,
                   System.Nullable<int> READ_ITEMNO_UPTO_MINLINES,
                   string EQUIP_NAME_RANGE,
                   string EQUIP_TYPE_RANGE,
                   string EQUIP_SERNO_RANGE,
                   string EQUIP_MAKER_RANGE,
                   string EQUIP_NOTE_RANGE,
                   System.Nullable<int> APPEND_UOM,
                   System.Nullable<int> MAKERREF_EXTRANO_LINE_COUNT,
                   string MAKERREF_RANGE,
                   string EXTRANO_RANGE,
                   System.Nullable<int> READ_PARTNO_FROM_LAST_LINE,
                   string ITEM_CURRENCY,
                   System.Nullable<int> APPEND_REF_NO,
                   System.Nullable<int> IS_BOLD_TEXT,
                   System.Nullable<int> IS_QTY_UOM_MERGED,
                   System.Nullable<int> REMOVE_DIGIT_IN_UOM,
                   string ITEM_REF_NO_HEADER )
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_PDF_ITEM_MAPPING_Insert");
            this._dataAccess.AddParameter("ITEM_MAPID", ITEM_MAPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("PDF_MAPID", PDF_MAPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAP_NUMBER", MAP_NUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_HEADER_LINE_COUNT", ITEM_HEADER_LINE_COUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_HEADER_CONTENT", ITEM_HEADER_CONTENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_END_CONTENT", ITEM_END_CONTENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_EQUIPMENT", ITEM_EQUIPMENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_GROUP_HEADER", ITEM_GROUP_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_NO", ITEM_NO, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_QTY", ITEM_QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_UNIT", ITEM_UNIT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_DESCR", ITEM_DESCR, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_REMARK", ITEM_REMARK, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_REFNO", ITEM_REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_UNITPRICE", ITEM_UNITPRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_DISCOUNT", ITEM_DISCOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_TOTAL", ITEM_TOTAL, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_MIN_LINES", ITEM_MIN_LINES, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_REMARKS_APPEND_TEXT", ITEM_REMARKS_APPEND_TEXT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_REMARKS_INITIALS", ITEM_REMARKS_INITIALS, ParameterDirection.Input);
            this._dataAccess.AddParameter("HAS_NO_EQUIP_HEADER", HAS_NO_EQUIP_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAX_EQUIP_ROWS", MAX_EQUIP_ROWS, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAX_EQUIP_RANGE", MAX_EQUIP_RANGE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_LEADDAYS", ITEM_LEADDAYS, ParameterDirection.Input);
            this._dataAccess.AddParameter("LEADDAYS_IN_DATE", LEADDAYS_IN_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CHECK_CONTENT_BELOW_ITEM", CHECK_CONTENT_BELOW_ITEM, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXTRA_COLUMNS", EXTRA_COLUMNS, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXTRA_COLUMNS_HEADER", EXTRA_COLUMNS_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("READ_ITEMNO_UPTO_MINLINES", READ_ITEMNO_UPTO_MINLINES, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_NAME_RANGE", EQUIP_NAME_RANGE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_TYPE_RANGE", EQUIP_TYPE_RANGE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_SERNO_RANGE", EQUIP_SERNO_RANGE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_MAKER_RANGE", EQUIP_MAKER_RANGE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_NOTE_RANGE", EQUIP_NOTE_RANGE, ParameterDirection.Input);
            this._dataAccess.AddParameter("APPEND_UOM", APPEND_UOM, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAKERREF_EXTRANO_LINE_COUNT", MAKERREF_EXTRANO_LINE_COUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAKERREF_RANGE", MAKERREF_RANGE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXTRANO_RANGE", EXTRANO_RANGE, ParameterDirection.Input);
            this._dataAccess.AddParameter("READ_PARTNO_FROM_LAST_LINE", READ_PARTNO_FROM_LAST_LINE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_CURRENCY", ITEM_CURRENCY, ParameterDirection.Input);
            this._dataAccess.AddParameter("APPEND_REF_NO", APPEND_REF_NO, ParameterDirection.Input);
            this._dataAccess.AddParameter("IS_BOLD_TEXT", IS_BOLD_TEXT, ParameterDirection.Input);
            this._dataAccess.AddParameter("IS_QTY_UOM_MERGED", IS_QTY_UOM_MERGED, ParameterDirection.Input);
            this._dataAccess.AddParameter("REMOVE_DIGIT_IN_UOM", REMOVE_DIGIT_IN_UOM, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_REF_NO_HEADER", ITEM_REF_NO_HEADER, ParameterDirection.Input);
            this._dataAccess.ExecuteNonQuery();
        }

        public virtual System.Nullable<int> SM_PDF_ITEM_MAPPING_Delete(System.Nullable<int> ITEM_MAPID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_PDF_ITEM_MAPPING_Delete");
            this._dataAccess.AddParameter("ITEM_MAPID", ITEM_MAPID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_PDF_ITEM_MAPPING_Update(
                    System.Nullable<int> ITEM_MAPID,
                    System.Nullable<int> PDF_MAPID,
                    System.Nullable<int> MAP_NUMBER,
                    System.Nullable<int> ITEM_HEADER_LINE_COUNT,
                    string ITEM_HEADER_CONTENT,
                    string ITEM_END_CONTENT,
                    string ITEM_EQUIPMENT,
                    string ITEM_GROUP_HEADER,
                    string ITEM_NO,
                    string ITEM_QTY,
                    string ITEM_UNIT,
                    string ITEM_DESCR,
                    string ITEM_REMARK,
                    string ITEM_REFNO,
                    string ITEM_UNITPRICE,
                    string ITEM_DISCOUNT,
                     string ITEM_LEADDAYS,
                    string ITEM_TOTAL,
                    System.Nullable<int> LEADDAYS_IN_DATE,
                    System.Nullable<int> ITEM_MIN_LINES,
                    string ITEM_REMARKS_APPEND_TEXT,
                    string ITEM_REMARKS_INITIALS,
                    System.Nullable<int> HAS_NO_EQUIP_HEADER,
                    System.Nullable<int> MAX_EQUIP_ROWS,
                    System.Nullable<int> MAX_EQUIP_RANGE,
                    System.Nullable<int> CHECK_CONTENT_BELOW_ITEM,
                    string EXTRA_COLUMNS,
                    string EXTRA_COLUMNS_HEADER,
                    System.Nullable<int> READ_ITEMNO_UPTO_MINLINES,
                    string EQUIP_NAME_RANGE,
                    string EQUIP_TYPE_RANGE,
                    string EQUIP_SERNO_RANGE,
                    string EQUIP_MAKER_RANGE,
                    string EQUIP_NOTE_RANGE,
                    System.Nullable<int> APPEND_UOM,
                    System.Nullable<int> MAKERREF_EXTRANO_LINE_COUNT,
                    string MAKERREF_RANGE,
                    string EXTRANO_RANGE,
                    System.Nullable<int> READ_PARTNO_FROM_LAST_LINE,
                    string ITEM_CURRENCY,
                    System.Nullable<int> APPEND_REF_NO,
                    System.Nullable<int> IS_BOLD_TEXT,
                    System.Nullable<int> IS_QTY_UOM_MERGED,
                    System.Nullable<int> REMOVE_DIGIT_IN_UOM,
                    string ITEM_REF_NO_HEADER )
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_PDF_ITEM_MAPPING_Update");
            this._dataAccess.AddParameter("ITEM_MAPID", ITEM_MAPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("PDF_MAPID", PDF_MAPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAP_NUMBER", MAP_NUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_HEADER_LINE_COUNT", ITEM_HEADER_LINE_COUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_HEADER_CONTENT", ITEM_HEADER_CONTENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_END_CONTENT", ITEM_END_CONTENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_EQUIPMENT", ITEM_EQUIPMENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_GROUP_HEADER", ITEM_GROUP_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_NO", ITEM_NO, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_QTY", ITEM_QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_UNIT", ITEM_UNIT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_DESCR", ITEM_DESCR, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_REMARK", ITEM_REMARK, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_REFNO", ITEM_REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_UNITPRICE", ITEM_UNITPRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_DISCOUNT", ITEM_DISCOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_TOTAL", ITEM_TOTAL, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_MIN_LINES", ITEM_MIN_LINES, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_REMARKS_APPEND_TEXT", ITEM_REMARKS_APPEND_TEXT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_REMARKS_INITIALS", ITEM_REMARKS_INITIALS, ParameterDirection.Input);
            this._dataAccess.AddParameter("HAS_NO_EQUIP_HEADER", HAS_NO_EQUIP_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAX_EQUIP_ROWS", MAX_EQUIP_ROWS, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAX_EQUIP_RANGE", MAX_EQUIP_RANGE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_LEADDAYS", ITEM_LEADDAYS, ParameterDirection.Input);
            this._dataAccess.AddParameter("LEADDAYS_IN_DATE", LEADDAYS_IN_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CHECK_CONTENT_BELOW_ITEM", CHECK_CONTENT_BELOW_ITEM, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXTRA_COLUMNS", EXTRA_COLUMNS, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXTRA_COLUMNS_HEADER", EXTRA_COLUMNS_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("READ_ITEMNO_UPTO_MINLINES", READ_ITEMNO_UPTO_MINLINES, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_NAME_RANGE", EQUIP_NAME_RANGE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_TYPE_RANGE", EQUIP_TYPE_RANGE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_SERNO_RANGE", EQUIP_SERNO_RANGE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_MAKER_RANGE", EQUIP_MAKER_RANGE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_NOTE_RANGE", EQUIP_NOTE_RANGE, ParameterDirection.Input);
            this._dataAccess.AddParameter("APPEND_UOM", APPEND_UOM, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAKERREF_EXTRANO_LINE_COUNT", MAKERREF_EXTRANO_LINE_COUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAKERREF_RANGE", MAKERREF_RANGE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXTRANO_RANGE", EXTRANO_RANGE, ParameterDirection.Input);
            this._dataAccess.AddParameter("READ_PARTNO_FROM_LAST_LINE", READ_PARTNO_FROM_LAST_LINE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_CURRENCY", ITEM_CURRENCY, ParameterDirection.Input);
            this._dataAccess.AddParameter("APPEND_REF_NO", APPEND_REF_NO, ParameterDirection.Input);
            this._dataAccess.AddParameter("IS_BOLD_TEXT", IS_BOLD_TEXT, ParameterDirection.Input);
            this._dataAccess.AddParameter("IS_QTY_UOM_MERGED", IS_QTY_UOM_MERGED, ParameterDirection.Input);
            this._dataAccess.AddParameter("REMOVE_DIGIT_IN_UOM", REMOVE_DIGIT_IN_UOM, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_REF_NO_HEADER", ITEM_REF_NO_HEADER, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual void Dispose()
        {
            if ((this._dataAccess != null))
            {
                this._dataAccess.Dispose();
            }
        }
    }
}