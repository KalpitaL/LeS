namespace MetroLesMonitor.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;

    public partial class SmPdfMapping : IDisposable
    {
        private DataAccess _dataAccess;

        public SmPdfMapping()
        {
            this._dataAccess = new DataAccess();
        }

        public SmPdfMapping(DataAccess _dataAccess)
        {
            // TODO: Complete member initialization
            this._dataAccess = _dataAccess;
        }

        public virtual System.Data.DataSet SM_PDF_MAPPING_Select_All()
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_PDF_MAPPING_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_PDF_MAPPING_Select_One(System.Nullable<int> PDF_MAPID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_PDF_MAPPING_Select_One");
            this._dataAccess.AddParameter("PDF_MAPID", PDF_MAPID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_PDF_MAPPING_Select_One_By_GROUPID(System.Nullable<int> GROUPID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_PDF_MAPPING_Select_One_By_GROUPID");
            this._dataAccess.AddParameter("GROUPID", GROUPID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual void SM_PDF_MAPPING_Insert(
                 System.Nullable<int> PDF_MAPID,
                 System.Nullable<int> GROUPID,
                 string BUYER_NAME,
                 string BUYER_TEL,
                 string BUYER_FAX,
                 string BUYER_EMAIL,
                 string BUYER_CONTACT,
                 string BUYER_ADDRESS,
                 string SUPP_NAME,
                 string SUPP_TEL,
                 string SUPP_FAX,
                 string SUPP_EMAIL,
                 string SUPP_CONTACT,
                 string SUPP_ADDRESS,
                 string BILL_NAME,
                 string BILL_CONTACT,
                 string BILL_TEL,
                 string BILL_FAX,
                 string BILL_EMAIL,
                 string BILL_ADDRESS,
                 string CONSIGN_NAME,
                 string CONSIGN_CONTACT,
                 string CONSIGN_TEL,
                 string CONSIGN_FAX,
                 string CONSIGN_EMAIL,
                 string CONSIGN_ADDRESS,
                 string BUYER_COMMENTS,
                 string VRNO_HEADER,
                 string VRNO,
                 string IMO_HEADER,
                 string IMO,
                 string PO_REFERENCE,
                 string QUOTE_REFERENCE,
                 string CREATED_DATE,
                 string LATE_DATE,
                 string VESSEL_HEADER,
                 string VESSEL,
                 string VESSEL_ETA,
                 string VESSEL_ETD,
                 string DELIVERY_PORT_HEADER,
                 string DELIVERY_PORT,
                 string CURRENCY,
                 System.Nullable<int> DISCOUNT_IN_PRCNT,
                 System.Nullable<char> DECIMAL_SEPRATOR,
                 System.Nullable<int> INCLUDE_BLANCK_LINES,
                 System.Nullable<int> HEADER_LINE_COUNT,
                 System.Nullable<int> FOOTER_LINE_COUNT,
                 string DATE_FORMAT1,
                 string DATE_FORMAT2,
                 string ITEMS_TOTAL_HEADER,
                 string FRIEGHT_AMT_HEADER,
                 string GRANT_TOTAL_HEADER,
                 System.Nullable<int> SPLIT_FILE,
                 string CONSTANT_ROWS,
                 string SPLIT_START_CONTENT,
                 string END_COMMENT_START_CONTENT,
                 string SUPP_COMMENTS,
                 string QUOTE_VALIDITY,
                 string ALLOWANCE_AMT_HEADER,
                 string PACKING_AMT_HEADER,
                 System.Nullable<int> ADD_HEADER_TO_COMMENTS,
                 System.Nullable<int> ADD_FOOTER_TO_COMMENTS,
                 string EXTRA_FIELDS,
                 string EXTRA_FIELDS_HEADER,
                 string FIELDS_FROM_HEADER,
                 string FIELDS_FROM_FOOTER,
                 string SUBJECT,
                 string EQUIP_NAME,
                 string EQUIP_NAME_HEADER,
                 string EQUIP_MAKER,
                 string EQUIP_MAKER_HEADER,
                 string EQUIP_SERNO,
                 string EQUIP_SERNO_HEADER,
                 string EQUIP_TYPE,
                 string EQUIP_TYPE_HEADER,
                 string EQUIP_REMARKS,
                 string EQUIP_REMARKS_HEADER,
                 System.Nullable<int> READ_CONTENT_BELOW_ITEM,
                 System.Nullable<int> OVERRIDE_ITEM_QTY,
                 System.Nullable<int> VALIDATE_ITEM_DESCR,
                 string HEADER_COMMENTS_START_TEXT,
                 string HEADER_COMMENTS_END_TEXT,
                 System.Nullable<int> ITEM_DESCR_UPTO_LINE_COUNT,
                 string DEPARTMENT,
                 System.Nullable<int> ADD_ITEM_DELDATE_TO_HEADER,
                 System.Nullable<int> REM_HEADER_REMARK_SPACE,
                 string HEADER_FLINE_CONTENT,
                 string HEADER_LLINE_CONTENT,
                 string FOOTER_FLINE_CONTENT,
                 string FOOTER_LLINE_CONTENT,
                 string SKIP_TEXT_START,
                 string SKIP_TEXT_END,
                 System.Nullable<int> ADD_SKIPPED_TXT_TO_REMAKRS,
                 string CURR_HEADER,
                 string DEPT_HEADER,
                 string QUOTE_REF_HEADER,
                 string POC_REF_HEADER,
                 string SUBJECT_HEADER,
                 string DEL_DATE_HEADER,
                 string DOC_DATE_HEADER,
                 string QUOTE_EXP_HEADER,
                 string ETA_HEADER,
                 string ETD_HEADER,
                 string BYR_ADDR_HEADER,
                 string SUPP_ADDR_HEADER,
                 string SHIP_ADDR_HEADER,
                 string BILL_ADDR_HEADER,
                 string ITEM_COUNT_HEADER,
                 string ITEM_FORMAT)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_PDF_MAPPING_Insert");
            this._dataAccess.AddParameter("PDF_MAPID", PDF_MAPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUPID", GROUPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_NAME", BUYER_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_TEL", BUYER_TEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_FAX", BUYER_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_EMAIL", BUYER_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_CONTACT", BUYER_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_ADDRESS", BUYER_ADDRESS, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_NAME", SUPP_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_TEL", SUPP_TEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_FAX", SUPP_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_EMAIL", SUPP_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_CONTACT", SUPP_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_ADDRESS", SUPP_ADDRESS, ParameterDirection.Input);
            this._dataAccess.AddParameter("BILL_NAME", BILL_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("BILL_CONTACT", BILL_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("BILL_TEL", BILL_TEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("BILL_FAX", BILL_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("BILL_EMAIL", BILL_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("BILL_ADDRESS", BILL_ADDRESS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONSIGN_NAME", CONSIGN_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONSIGN_CONTACT", CONSIGN_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONSIGN_TEL", CONSIGN_TEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONSIGN_FAX", CONSIGN_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONSIGN_EMAIL", CONSIGN_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONSIGN_ADDRESS", CONSIGN_ADDRESS, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_COMMENTS", BUYER_COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("VRNO_HEADER", VRNO_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("VRNO", VRNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("PO_REFERENCE", PO_REFERENCE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_REFERENCE", QUOTE_REFERENCE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("LATE_DATE", LATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_HEADER", VESSEL_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL", VESSEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMO_NO_HEADER", IMO_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMO_NO", IMO, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_ETA", VESSEL_ETA, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_ETD", VESSEL_ETD, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERY_PORT_HEADER", DELIVERY_PORT_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERY_PORT", DELIVERY_PORT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURRENCY", CURRENCY, ParameterDirection.Input);
            //this._dataAccess.AddParameter("QUOTE_DISCOUNT", QUOTE_DISCOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISCOUNT_IN_PRCNT", DISCOUNT_IN_PRCNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("DECIMAL_SEPRATOR", DECIMAL_SEPRATOR, ParameterDirection.Input);
            this._dataAccess.AddParameter("INCLUDE_BLANCK_LINES", INCLUDE_BLANCK_LINES, ParameterDirection.Input);
            this._dataAccess.AddParameter("HEADER_LINE_COUNT", HEADER_LINE_COUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("FOOTER_LINE_COUNT", FOOTER_LINE_COUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("DATE_FORMAT_1", DATE_FORMAT1, ParameterDirection.Input);
            this._dataAccess.AddParameter("DATE_FORMAT_2", DATE_FORMAT2, ParameterDirection.Input);
            //this._dataAccess.AddParameter("SKIP_LINES", SKIP_LINES, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEMS_TOTAL_HEADER", ITEMS_TOTAL_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("FRIEGHT_AMT_HEADER", FRIEGHT_AMT_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("GRANT_TOTAL_HEADER", GRANT_TOTAL_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("SPLIT_FILE", SPLIT_FILE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONSTANT_ROWS", CONSTANT_ROWS, ParameterDirection.Input);
            this._dataAccess.AddParameter("SPLIT_START_CONTENT", SPLIT_START_CONTENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("END_COMMENT_START_CONTENT", END_COMMENT_START_CONTENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_COMMENTS", SUPP_COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_VALIDITY", QUOTE_VALIDITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ALLOWANCE_AMT_HEADER", ALLOWANCE_AMT_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("PACKING_AMT_HEADER", PACKING_AMT_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXTRA_FIELDS", EXTRA_FIELDS, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXTRA_FIELDS_HEADER", EXTRA_FIELDS_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADD_HEADER_TO_COMMENTS", ADD_HEADER_TO_COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADD_FOOTER_TO_COMMENTS", ADD_FOOTER_TO_COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("FIELDS_FROM_HEADER", FIELDS_FROM_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("FIELDS_FROM_FOOTER", FIELDS_FROM_FOOTER, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUBJECT", SUBJECT, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_NAME", EQUIP_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_NAME_HEADER", EQUIP_NAME_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_MAKER", EQUIP_MAKER, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_MAKER_HEADER", EQUIP_MAKER_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_SERNO", EQUIP_SERNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_SERNO_HEADER", EQUIP_SERNO_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_TYPE", EQUIP_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_TYPE_HEADER", EQUIP_TYPE_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_REMARKS", EQUIP_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_REMARKS_HEADER", EQUIP_REMARKS_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("READ_CONTENT_BELOW_ITEM", READ_CONTENT_BELOW_ITEM, ParameterDirection.Input);
            this._dataAccess.AddParameter("OVERRIDE_ITEM_QTY", OVERRIDE_ITEM_QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("VALIDATE_ITEM_DESCR", VALIDATE_ITEM_DESCR, ParameterDirection.Input);
            this._dataAccess.AddParameter("HEADER_COMMENTS_START_TEXT", HEADER_COMMENTS_START_TEXT, ParameterDirection.Input);
            this._dataAccess.AddParameter("HEADER_COMMENTS_END_TEXT", HEADER_COMMENTS_END_TEXT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_DESCR_UPTO_LINE_COUNT", ITEM_DESCR_UPTO_LINE_COUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEPARTMENT", DEPARTMENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADD_ITEM_DELDATE_TO_HEADER", ADD_ITEM_DELDATE_TO_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("REM_HEADER_REMARK_SPACE", REM_HEADER_REMARK_SPACE, ParameterDirection.Input);
            this._dataAccess.AddParameter("HEADER_FLINE_CONTENT", HEADER_FLINE_CONTENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("HEADER_LLINE_CONTENT", HEADER_LLINE_CONTENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("FOOTER_FLINE_CONTENT", FOOTER_FLINE_CONTENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("FOOTER_LLINE_CONTENT", FOOTER_LLINE_CONTENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SKIP_TEXT_START", SKIP_TEXT_START, ParameterDirection.Input);
            this._dataAccess.AddParameter("SKIP_TEXT_END", SKIP_TEXT_END, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADD_SKIPPED_TXT_TO_REMAKRS", ADD_SKIPPED_TXT_TO_REMAKRS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_HEADER", CURR_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEPT_HEADER", DEPT_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_REF_HEADER", QUOTE_REF_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("POC_REF_HEADER", POC_REF_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUBJECT_HEADER", SUBJECT_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEL_DATE_HEADER", DEL_DATE_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_DATE_HEADER", DOC_DATE_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_EXP_HEADER", QUOTE_EXP_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ETA_HEADER", ETA_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ETD_HEADER", ETD_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("BYR_ADDR_HEADER", BYR_ADDR_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_ADDR_HEADER", SUPP_ADDR_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("SHIP_ADDR_HEADER", SHIP_ADDR_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("BILL_ADDR_HEADER", BILL_ADDR_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_COUNT_HEADER", ITEM_COUNT_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_FORMAT", ITEM_FORMAT, ParameterDirection.Input);
            this._dataAccess.ExecuteNonQuery();
        }

        public virtual System.Nullable<int> SM_PDF_MAPPING_Delete(System.Nullable<int> PDF_MAPID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_PDF_MAPPING_Delete");
            this._dataAccess.AddParameter("PDF_MAPID", PDF_MAPID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_PDF_MAPPING_Update(
            System.Nullable<int> PDF_MAPID,
            System.Nullable<int> GROUPID,
            string BUYER_NAME,
            string BUYER_TEL,
            string BUYER_FAX,
            string BUYER_EMAIL,
            string BUYER_CONTACT,
            string BUYER_ADDRESS,
            string SUPP_NAME,
            string SUPP_TEL,
            string SUPP_FAX,
            string SUPP_EMAIL,
            string SUPP_CONTACT,
            string SUPP_ADDRESS,
            string BILL_NAME,
            string BILL_CONTACT,
            string BILL_TEL,
            string BILL_FAX,
            string BILL_EMAIL,
            string BILL_ADDRESS,
            string CONSIGN_NAME,
            string CONSIGN_CONTACT,
            string CONSIGN_TEL,
            string CONSIGN_FAX,
            string CONSIGN_EMAIL,
            string CONSIGN_ADDRESS,
            string BUYER_COMMENTS,
            string VRNO_HEADER,
            string VRNO,
            string IMO_HEADER,
            string IMO,
            string PO_REFERENCE,
            string QUOTE_REFERENCE,
            string CREATED_DATE,
            string LATE_DATE,
            string VESSEL_HEADER,
            string VESSEL,
            string VESSEL_ETA,
            string VESSEL_ETD,
            string DELIVERY_PORT_HEADER,
            string DELIVERY_PORT,
            string CURRENCY,
            //string QUOTE_DISCOUNT,
            System.Nullable<int> DISCOUNT_IN_PRCNT,
            System.Nullable<char> DECIMAL_SEPRATOR,
            System.Nullable<int> INCLUDE_BLANCK_LINES,
            System.Nullable<int> HEADER_LINE_COUNT,
            System.Nullable<int> FOOTER_LINE_COUNT,
            string DATE_FORMAT1,
            string DATE_FORMAT2,
            //string SKIP_LINES,
            string ITEMS_TOTAL_HEADER,
            string FRIEGHT_AMT_HEADER,
            string GRANT_TOTAL_HEADER,
            System.Nullable<int> SPLIT_FILE,
            string CONSTANT_ROWS,
            string SPLIT_START_CONTENT,
            string END_COMMENT_START_CONTENT,
            string SUPP_COMMENTS,
            string QUOTE_VALIDITY,
            string ALLOWANCE_AMT_HEADER,
            string PACKING_AMT_HEADER,
            System.Nullable<int> ADD_HEADER_TO_COMMENTS,
            System.Nullable<int> ADD_FOOTER_TO_COMMENTS,
            string EXTRA_FIELDS,
            string EXTRA_FIELDS_HEADER,
            string FIELDS_FROM_HEADER,
            string FIELDS_FROM_FOOTER,
            string SUBJECT,
            string EQUIP_NAME,
            string EQUIP_NAME_HEADER,
            string EQUIP_MAKER,
            string EQUIP_MAKER_HEADER,
            string EQUIP_SERNO,
            string EQUIP_SERNO_HEADER,
            string EQUIP_TYPE,
            string EQUIP_TYPE_HEADER,
            string EQUIP_REMARKS,
            string EQUIP_REMARKS_HEADER,
            System.Nullable<int> READ_CONTENT_BELOW_ITEM,
            System.Nullable<int> OVERRIDE_ITEM_QTY,
            System.Nullable<int> VALIDATE_ITEM_DESCR,
            string HEADER_COMMENTS_START_TEXT,
            string HEADER_COMMENTS_END_TEXT,
            System.Nullable<int> ITEM_DESCR_UPTO_LINE_COUNT,
            string DEPARTMENT,
            System.Nullable<int> ADD_ITEM_DELDATE_TO_HEADER,
            System.Nullable<int> REM_HEADER_REMARK_SPACE,
            string HEADER_FLINE_CONTENT,
             string HEADER_LLINE_CONTENT,
             string FOOTER_FLINE_CONTENT,
             string FOOTER_LLINE_CONTENT,
            string SKIP_TEXT_START,
            string SKIP_TEXT_END,
            System.Nullable<int> ADD_SKIPPED_TXT_TO_REMAKRS,
            string CURR_HEADER,
            string DEPT_HEADER,
            string QUOTE_REF_HEADER,
            string POC_REF_HEADER,
            string SUBJECT_HEADER,
            string DEL_DATE_HEADER,
            string DOC_DATE_HEADER,
            string QUOTE_EXP_HEADER,
            string ETA_HEADER,
            string ETD_HEADER,
            string BYR_ADDR_HEADER,
            string SUPP_ADDR_HEADER,
            string SHIP_ADDR_HEADER,
            string BILL_ADDR_HEADER,
            string ITEM_COUNT_HEADER,
            string ITEM_FORMAT)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_PDF_MAPPING_Update");
            this._dataAccess.AddParameter("PDF_MAPID", PDF_MAPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUPID", GROUPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_NAME", BUYER_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_TEL", BUYER_TEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_FAX", BUYER_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_EMAIL", BUYER_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_CONTACT", BUYER_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_ADDRESS", BUYER_ADDRESS, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_NAME", SUPP_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_TEL", SUPP_TEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_FAX", SUPP_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_EMAIL", SUPP_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_CONTACT", SUPP_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_ADDRESS", SUPP_ADDRESS, ParameterDirection.Input);
            this._dataAccess.AddParameter("BILL_NAME", BILL_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("BILL_CONTACT", BILL_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("BILL_TEL", BILL_TEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("BILL_FAX", BILL_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("BILL_EMAIL", BILL_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("BILL_ADDRESS", BILL_ADDRESS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONSIGN_NAME", CONSIGN_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONSIGN_CONTACT", CONSIGN_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONSIGN_TEL", CONSIGN_TEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONSIGN_FAX", CONSIGN_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONSIGN_EMAIL", CONSIGN_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONSIGN_ADDRESS", CONSIGN_ADDRESS, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_COMMENTS", BUYER_COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("VRNO_HEADER", VRNO_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("VRNO", VRNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMO_NO_HEADER", IMO_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMO_NO", IMO, ParameterDirection.Input);
            this._dataAccess.AddParameter("PO_REFERENCE", PO_REFERENCE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_REFERENCE", QUOTE_REFERENCE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("LATE_DATE", LATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_HEADER", VESSEL_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL", VESSEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_ETA", VESSEL_ETA, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_ETD", VESSEL_ETD, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERY_PORT_HEADER", DELIVERY_PORT_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERY_PORT", DELIVERY_PORT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURRENCY", CURRENCY, ParameterDirection.Input);
            //this._dataAccess.AddParameter("QUOTE_DISCOUNT", QUOTE_DISCOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISCOUNT_IN_PRCNT", DISCOUNT_IN_PRCNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("DECIMAL_SEPRATOR", DECIMAL_SEPRATOR, ParameterDirection.Input);
            this._dataAccess.AddParameter("INCLUDE_BLANCK_LINES", INCLUDE_BLANCK_LINES, ParameterDirection.Input);
            this._dataAccess.AddParameter("HEADER_LINE_COUNT", HEADER_LINE_COUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("FOOTER_LINE_COUNT", FOOTER_LINE_COUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("DATE_FORMAT_1", DATE_FORMAT1, ParameterDirection.Input);
            this._dataAccess.AddParameter("DATE_FORMAT_2", DATE_FORMAT2, ParameterDirection.Input);
            //this._dataAccess.AddParameter("SKIP_LINES", SKIP_LINES, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEMS_TOTAL_HEADER", ITEMS_TOTAL_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("FRIEGHT_AMT_HEADER", FRIEGHT_AMT_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("GRANT_TOTAL_HEADER", GRANT_TOTAL_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("SPLIT_FILE", SPLIT_FILE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONSTANT_ROWS", CONSTANT_ROWS, ParameterDirection.Input);
            this._dataAccess.AddParameter("SPLIT_START_CONTENT", SPLIT_START_CONTENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("END_COMMENT_START_CONTENT", END_COMMENT_START_CONTENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_COMMENTS", SUPP_COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_VALIDITY", QUOTE_VALIDITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ALLOWANCE_AMT_HEADER", ALLOWANCE_AMT_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("PACKING_AMT_HEADER", PACKING_AMT_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXTRA_FIELDS", EXTRA_FIELDS, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXTRA_FIELDS_HEADER", EXTRA_FIELDS_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADD_HEADER_TO_COMMENTS", ADD_HEADER_TO_COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADD_FOOTER_TO_COMMENTS", ADD_FOOTER_TO_COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("FIELDS_FROM_HEADER", FIELDS_FROM_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("FIELDS_FROM_FOOTER", FIELDS_FROM_FOOTER, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUBJECT", SUBJECT, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_NAME", EQUIP_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_NAME_HEADER", EQUIP_NAME_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_MAKER", EQUIP_MAKER, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_MAKER_HEADER", EQUIP_MAKER_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_SERNO", EQUIP_SERNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_SERNO_HEADER", EQUIP_SERNO_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_TYPE", EQUIP_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_TYPE_HEADER", EQUIP_TYPE_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_REMARKS", EQUIP_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_REMARKS_HEADER", EQUIP_REMARKS_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("READ_CONTENT_BELOW_ITEM", READ_CONTENT_BELOW_ITEM, ParameterDirection.Input);
            this._dataAccess.AddParameter("OVERRIDE_ITEM_QTY", OVERRIDE_ITEM_QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("VALIDATE_ITEM_DESCR", VALIDATE_ITEM_DESCR, ParameterDirection.Input);
            this._dataAccess.AddParameter("HEADER_COMMENTS_START_TEXT", HEADER_COMMENTS_START_TEXT, ParameterDirection.Input);
            this._dataAccess.AddParameter("HEADER_COMMENTS_END_TEXT", HEADER_COMMENTS_END_TEXT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_DESCR_UPTO_LINE_COUNT", ITEM_DESCR_UPTO_LINE_COUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEPARTMENT", DEPARTMENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADD_ITEM_DELDATE_TO_HEADER", ADD_ITEM_DELDATE_TO_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("REM_HEADER_REMARK_SPACE", REM_HEADER_REMARK_SPACE, ParameterDirection.Input);
            this._dataAccess.AddParameter("HEADER_FLINE_CONTENT", HEADER_FLINE_CONTENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("HEADER_LLINE_CONTENT", HEADER_LLINE_CONTENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("FOOTER_FLINE_CONTENT", FOOTER_FLINE_CONTENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("FOOTER_LLINE_CONTENT", FOOTER_LLINE_CONTENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SKIP_TEXT_START", SKIP_TEXT_START, ParameterDirection.Input);
            this._dataAccess.AddParameter("SKIP_TEXT_END", SKIP_TEXT_END, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADD_SKIPPED_TXT_TO_REMAKRS", ADD_SKIPPED_TXT_TO_REMAKRS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_HEADER", CURR_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEPT_HEADER", DEPT_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_REF_HEADER", QUOTE_REF_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("POC_REF_HEADER", POC_REF_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUBJECT_HEADER", SUBJECT_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEL_DATE_HEADER", DEL_DATE_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_DATE_HEADER", DOC_DATE_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_EXP_HEADER", QUOTE_EXP_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ETA_HEADER", ETA_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ETD_HEADER", ETD_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("BYR_ADDR_HEADER", BYR_ADDR_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_ADDR_HEADER", SUPP_ADDR_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("SHIP_ADDR_HEADER", SHIP_ADDR_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("BILL_ADDR_HEADER", BILL_ADDR_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_COUNT_HEADER", ITEM_COUNT_HEADER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_FORMAT", ITEM_FORMAT, ParameterDirection.Input);
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