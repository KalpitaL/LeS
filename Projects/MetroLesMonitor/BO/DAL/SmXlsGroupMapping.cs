using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Xml;

namespace MetroLesMonitor.Dal
{
    public partial class SmXlsGroupMapping : IDisposable
    {
        public DataAccess _dataAccess;

        public SmXlsGroupMapping()
        {
            this._dataAccess = new DataAccess();
        }

        public SmXlsGroupMapping(DataAccess _dataAccess)
        {
            // TODO: Complete member initialization
            this._dataAccess = _dataAccess;
        }

        public virtual System.Data.DataSet SM_XLS_GROUP_MAPPING_Select_All()
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_GROUP_MAPPING_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_XLS_GROUP_MAPPING_Select_One(System.Nullable<int> EXCEL_MAPID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_GROUP_MAPPING_Select_One");
            this._dataAccess.AddParameter("EXCEL_MAPID", EXCEL_MAPID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet Select_SM_XLS_GROUP_MAPPINGs_By_GROUP_ID(System.Nullable<int> GROUP_ID)
        {
            this._dataAccess.CreateProcedureCommand("sp_Select_SM_XLS_GROUP_MAPPINGs_By_GROUP_ID");
            this._dataAccess.AddParameter("GROUP_ID", GROUP_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Nullable<int> SM_XLS_GROUP_MAPPING_Insert(
                    System.Nullable<int> GROUP_ID,
                    string XLS_MAP_CODE,
                    System.Nullable<int> SECTION_ROW_START,
                    System.Nullable<int> ITEM_ROW_START,
                    System.Nullable<int> SKIP_ROWS_BEF_ITEM,
                    System.Nullable<int> SKIP_ROWS_AFT_SECTION,
                    string CELL_VRNO,
                    string CELL_RFQ_DT,
                    string CELL_VESSEL,
                    string CELL_PORT,
                    string CELL_LATE_DT,
                    string CELL_SUPP_REF,
                    string CELL_VALID_UPTO,
                    string CELL_CURR_CODE,
                    string CELL_CONTACT,
                    string CELL_PAY_TERMS,
                    string CELL_DEL_TERMS,
                    string CELL_BUYER_REMARKS,
                    string CELL_SUPLR_REMARKS,
                    string COL_SECTION,
                    string COL_ITEMNO,
                    string COL_ITEM_REFNO,
                    string COL_ITEM_NAME,
                    string COL_ITEM_QTY,
                    string COL_ITEM_UNIT,
                    string COL_ITEM_PRICE,
                    string COL_ITEM_DISCOUNT,
                    string COL_ITEM_ALT_QTY,
                    string COL_ITEM_ALT_UNIT,
                    string COL_ITEM_ALT_PRICE,
                    string COL_ITEM_DEL_DAYS,
                    string COL_ITEM_REMARKS,
                    System.Nullable<int> ACTIVE_SHEET,
                    System.Nullable<short> EXIT_FOR_NOITEM,
                    string COL_ITEM_CURR,
                    System.Nullable<int> DYN_SUP_RMRK_OFFSET,
                    System.Nullable<int> OVERRIDE_ALT_QTY,
                    System.Nullable<int> SKIP_HIDDEN_ROWS,
                    System.Nullable<int> ITEM_DISC_PERCNT,
                    string COL_ITEM_TOTAL,
                    System.Nullable<int> APPLY_TOTAL_FORMULA,
                    System.Nullable<int> READ_ITEM_REMARKS_UPTO_NO,
                    string COL_ITEM_BUYER_REMARKS,
                    string ADD_TO_VRNO,
                    string REMOVE_FROM_VRNO,
                    System.Nullable<int> SKIP_ROWS_AFT_ITEM,
                    System.Nullable<int> ITEM_NO_AS_ROWNO,
                    string COL_ITEM_COMMENTS,
                    string SAMPLE_FILE,
                    string CELL_VSL_IMONO,
                    string CELL_PORT_NAME,
                    string CELL_DOC_TYPE,
                    string COL_ITEM_SUPP_REFNO,
                    string CELL_SUPP_EXP_DT,
                    string CELL_SUPP_LATE_DT,
                    string CELL_SUPP_LEAD_DAYS,
                    string CELL_BYR_COMPANY,
                    string CELL_BYR_CONTACT,
                    string CELL_BYR_EMAIL,
                    string CELL_BYR_PHONE,
                    string CELL_BYR_MOB,
                    string CELL_BYR_FAX,
                    string CELL_SUPP_COMPANY,
                    string CELL_SUPP_CONTACT,
                    string CELL_SUPP_EMAIL,
                    string CELL_SUPP_PHONE,
                    string CELL_SUPP_MOB,
                    string CELL_SUPP_FAX,
                    string CELL_FREIGHT_AMT,
                    string CELL_OTHER_AMT,
                    string CELL_DISC_PROVSN,
                    string DISC_PROVSN_VALUE,
                    System.Nullable<int> ALT_ITEM_START_OFFSET,
                    System.Nullable<int> ALT_ITEM_COUNT,
                    string CELL_RFQ_TITLE,
                    string CELL_RFQ_DEPT,
                    string CELL_EQUIP_NAME,
                    string CELL_EQUIP_TYPE,
                    string CELL_EQUIP_MAKER,
                    string CELL_EQUIP_SERNO,
                    string CELL_EQUIP_DTLS,
                    string CELL_MSGNO,
                    System.Nullable<int> DYN_SUP_FREIGHT_OFFSET,
                    System.Nullable<int> DYN_OTHERCOST_OFFSET,
                    System.Nullable<int> DYN_SUP_CURR_OFFSET,
                    System.Nullable<int> DYN_BYR_RMRK_OFFSET,
                    System.Nullable<int> MULTILINE_ITEM_DESCR,
                    string EXCEL_NAME_MANAGER,
                    string DECIMAL_SEPARATOR,
                    string DEFAULT_UOM,
                    System.Nullable<int> DYN_HDR_DISCOUNT_OFFSET,

                    string REMOVE_FROM_VESSEL_NAME,
                    string CELL_BYR_ADDR1,
                    string CELL_BYR_ADDR2,
                    string CELL_SUPP_ADDR1,
                    string CELL_SUPP_ADDR2,
                    string CELL_BILL_COMPANY,
                    string CELL_BILL_CONTACT,
                    string CELL_BILL_EMAIL,
                    string CELL_BILL_PHONE,
                    string CELL_BILL_MOB,
                    string CELL_BILL_FAX,
                    string CELL_BILL_ADDR1,
                    string CELL_BILL_ADDR2,
                    string CELL_SHIP_COMPANY,
                    string CELL_SHIP_CONTACT,
                    string CELL_SHIP_EMAIL,
                    string CELL_SHIP_PHONE,
                    string CELL_SHIP_MOB,
                    string CELL_SHIP_FAX,
                    string CELL_SHIP_ADDR1,
                    string CELL_SHIP_ADDR2,
                    string CELL_ORDER_IDENTIFIER,
                    string CELL_SUPP_QUOTE_DT,
                    string COL_ITEM_ALT_NAME,
                    string CELL_ETA_DATE,
                    string CELL_ETD_DATE)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_GROUP_MAPPING_Insert");
            this._dataAccess.AddParameter("EXCEL_MAPID", 0, ParameterDirection.Output);
            this._dataAccess.AddParameter("GROUP_ID", GROUP_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("XLS_MAP_CODE", XLS_MAP_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SECTION_ROW_START", SECTION_ROW_START, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_ROW_START", ITEM_ROW_START, ParameterDirection.Input);
            this._dataAccess.AddParameter("SKIP_ROWS_BEF_ITEM", SKIP_ROWS_BEF_ITEM, ParameterDirection.Input);
            this._dataAccess.AddParameter("SKIP_ROWS_AFT_SECTION", SKIP_ROWS_AFT_SECTION, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_VRNO", CELL_VRNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_RFQ_DT", CELL_RFQ_DT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_VESSEL", CELL_VESSEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_PORT", CELL_PORT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_LATE_DT", CELL_LATE_DT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_REF", CELL_SUPP_REF, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_VALID_UPTO", CELL_VALID_UPTO, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_CURR_CODE", CELL_CURR_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_CONTACT", CELL_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_PAY_TERMS", CELL_PAY_TERMS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_DEL_TERMS", CELL_DEL_TERMS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BUYER_REMARKS", CELL_BUYER_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPLR_REMARKS", CELL_SUPLR_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_SECTION", COL_SECTION, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEMNO", COL_ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_REFNO", COL_ITEM_REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_NAME", COL_ITEM_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_QTY", COL_ITEM_QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_UNIT", COL_ITEM_UNIT, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_PRICE", COL_ITEM_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_DISCOUNT", COL_ITEM_DISCOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_ALT_QTY", COL_ITEM_ALT_QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_ALT_UNIT", COL_ITEM_ALT_UNIT, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_ALT_PRICE", COL_ITEM_ALT_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_DEL_DAYS", COL_ITEM_DEL_DAYS, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_REMARKS", COL_ITEM_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("ACTIVE_SHEET", ACTIVE_SHEET, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXIT_FOR_NOITEM", EXIT_FOR_NOITEM, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_CURR", COL_ITEM_CURR, ParameterDirection.Input);
            this._dataAccess.AddParameter("DYN_SUP_RMRK_OFFSET", DYN_SUP_RMRK_OFFSET, ParameterDirection.Input);
            this._dataAccess.AddParameter("OVERRIDE_ALT_QTY", OVERRIDE_ALT_QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("SKIP_HIDDEN_ROWS", SKIP_HIDDEN_ROWS, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_DISC_PERCNT", ITEM_DISC_PERCNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_TOTAL", COL_ITEM_TOTAL, ParameterDirection.Input);
            this._dataAccess.AddParameter("APPLY_TOTAL_FORMULA", APPLY_TOTAL_FORMULA, ParameterDirection.Input);
            this._dataAccess.AddParameter("READ_ITEM_REMARKS_UPTO_NO", READ_ITEM_REMARKS_UPTO_NO, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_BUYER_REMARKS", COL_ITEM_BUYER_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADD_TO_VRNO", ADD_TO_VRNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("REMOVE_FROM_VRNO", REMOVE_FROM_VRNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("SKIP_ROWS_AFT_ITEM", SKIP_ROWS_AFT_ITEM, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_NO_AS_ROWNO", ITEM_NO_AS_ROWNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_COMMENTS", COL_ITEM_COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("SAMPLE_FILE", SAMPLE_FILE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_VSL_IMONO", CELL_VSL_IMONO, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_PORT_NAME", CELL_PORT_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_DOC_TYPE", CELL_DOC_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_SUPP_REFNO", COL_ITEM_SUPP_REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_EXP_DT", CELL_SUPP_EXP_DT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_LATE_DT", CELL_SUPP_LATE_DT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_LEAD_DAYS", CELL_SUPP_LEAD_DAYS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BYR_COMPANY", CELL_BYR_COMPANY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BYR_CONTACT", CELL_BYR_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BYR_EMAIL", CELL_BYR_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BYR_PHONE", CELL_BYR_PHONE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BYR_MOB", CELL_BYR_MOB, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BYR_FAX", CELL_BYR_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_COMPANY", CELL_SUPP_COMPANY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_CONTACT", CELL_SUPP_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_EMAIL", CELL_SUPP_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_PHONE", CELL_SUPP_PHONE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_MOB", CELL_SUPP_MOB, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_FAX", CELL_SUPP_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_FREIGHT_AMT", CELL_FREIGHT_AMT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_OTHER_AMT", CELL_OTHER_AMT, ParameterDirection.Input);

            // Added  on 10-MARCH-2015
            this._dataAccess.AddParameter("CELL_DISC_PROVSN", CELL_DISC_PROVSN, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISC_PROVSN_VALUE", DISC_PROVSN_VALUE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ALT_ITEM_START_OFFSET", ALT_ITEM_START_OFFSET, ParameterDirection.Input);
            this._dataAccess.AddParameter("ALT_ITEM_COUNT", ALT_ITEM_COUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_RFQ_TITLE", CELL_RFQ_TITLE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_RFQ_DEPT", CELL_RFQ_DEPT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_EQUIP_NAME", CELL_EQUIP_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_EQUIP_TYPE", CELL_EQUIP_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_EQUIP_MAKER", CELL_EQUIP_MAKER, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_EQUIP_SERNO", CELL_EQUIP_SERNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_EQUIP_DTLS", CELL_EQUIP_DTLS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_MSGNO", CELL_MSGNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("DYN_SUP_FREIGHT_OFFSET", DYN_SUP_FREIGHT_OFFSET, ParameterDirection.Input);

            this._dataAccess.AddParameter("DYN_OTHERCOST_OFFSET", DYN_OTHERCOST_OFFSET, ParameterDirection.Input);
            this._dataAccess.AddParameter("DYN_SUP_CURR_OFFSET", DYN_SUP_CURR_OFFSET, ParameterDirection.Input);
            this._dataAccess.AddParameter("DYN_BYR_RMRK_OFFSET", DYN_BYR_RMRK_OFFSET, ParameterDirection.Input);
            this._dataAccess.AddParameter("MULTILINE_ITEM_DESCR", MULTILINE_ITEM_DESCR, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXCEL_NAME_MANAGER", EXCEL_NAME_MANAGER, ParameterDirection.Input);
            this._dataAccess.AddParameter("DECIMAL_SEPARATOR", DECIMAL_SEPARATOR, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEFAULT_UOM", DEFAULT_UOM, ParameterDirection.Input);

            this._dataAccess.AddParameter("DYN_HDR_DISCOUNT_OFFSET", DYN_HDR_DISCOUNT_OFFSET, ParameterDirection.Input);
            this._dataAccess.AddParameter("REMOVE_FROM_VESSEL_NAME", REMOVE_FROM_VESSEL_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BYR_ADDR1", CELL_BYR_ADDR1, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BYR_ADDR2", CELL_BYR_ADDR2, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_ADDR1", CELL_SUPP_ADDR1, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_ADDR2", CELL_SUPP_ADDR2, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BILL_COMPANY", CELL_BILL_COMPANY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BILL_CONTACT", CELL_BILL_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BILL_EMAIL", CELL_BILL_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BILL_PHONE", CELL_BILL_PHONE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BILL_MOB", CELL_BILL_MOB, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BILL_FAX", CELL_BILL_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BILL_ADDR1", CELL_BILL_ADDR1, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BILL_ADDR2", CELL_BILL_ADDR2, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SHIP_COMPANY", CELL_SHIP_COMPANY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SHIP_CONTACT", CELL_SHIP_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SHIP_EMAIL", CELL_SHIP_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SHIP_PHONE", CELL_SHIP_PHONE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SHIP_MOB", CELL_SHIP_MOB, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SHIP_FAX", CELL_SHIP_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SHIP_ADDR1", CELL_SHIP_ADDR1, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SHIP_ADDR2", CELL_SHIP_ADDR2, ParameterDirection.Input);

            this._dataAccess.AddParameter("CELL_ORDER_IDENTIFIER", CELL_ORDER_IDENTIFIER, ParameterDirection.Input);

            // added on 06-APRIL-2017
            this._dataAccess.AddParameter("CELL_SUPP_QUOTE_DT", CELL_SUPP_QUOTE_DT, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_ALT_NAME", COL_ITEM_ALT_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_ETA_DATE", CELL_ETA_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_ETD_DATE", CELL_ETD_DATE, ParameterDirection.Input);

            int value = this._dataAccess.ExecuteNonQuery();
            value = convert.ToInt(this._dataAccess.Command.Parameters["EXCEL_MAPID"].Value);
            return value;
        }

        public virtual System.Nullable<int> SM_XLS_GROUP_MAPPING_Delete(System.Nullable<int> EXCEL_MAPID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_GROUP_MAPPING_Delete");
            this._dataAccess.AddParameter("EXCEL_MAPID", EXCEL_MAPID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_XLS_GROUP_MAPPING_Update(
                    System.Nullable<int> EXCEL_MAPID,
                    System.Nullable<int> GROUP_ID,
                    string XLS_MAP_CODE,
                    System.Nullable<int> SECTION_ROW_START,
                    System.Nullable<int> ITEM_ROW_START,
                    System.Nullable<int> SKIP_ROWS_BEF_ITEM,
                    System.Nullable<int> SKIP_ROWS_AFT_SECTION,
                    string CELL_VRNO,
                    string CELL_RFQ_DT,
                    string CELL_VESSEL,
                    string CELL_PORT,
                    string CELL_LATE_DT,
                    string CELL_SUPP_REF,
                    string CELL_VALID_UPTO,
                    string CELL_CURR_CODE,
                    string CELL_CONTACT,
                    string CELL_PAY_TERMS,
                    string CELL_DEL_TERMS,
                    string CELL_BUYER_REMARKS,
                    string CELL_SUPLR_REMARKS,
                    string COL_SECTION,
                    string COL_ITEMNO,
                    string COL_ITEM_REFNO,
                    string COL_ITEM_NAME,
                    string COL_ITEM_QTY,
                    string COL_ITEM_UNIT,
                    string COL_ITEM_PRICE,
                    string COL_ITEM_DISCOUNT,
                    string COL_ITEM_ALT_QTY,
                    string COL_ITEM_ALT_UNIT,
                    string COL_ITEM_ALT_PRICE,
                    string COL_ITEM_DEL_DAYS,
                    string COL_ITEM_REMARKS,
                    System.Nullable<int> ACTIVE_SHEET,
                    System.Nullable<short> EXIT_FOR_NOITEM,
                    string COL_ITEM_CURR,
                    System.Nullable<int> DYN_SUP_RMRK_OFFSET,
                    System.Nullable<int> OVERRIDE_ALT_QTY,
                    System.Nullable<int> SKIP_HIDDEN_ROWS,
                    System.Nullable<int> ITEM_DISC_PERCNT,
                    string COL_ITEM_TOTAL,
                    System.Nullable<int> APPLY_TOTAL_FORMULA,
                    System.Nullable<int> READ_ITEM_REMARKS_UPTO_NO,
                    string COL_ITEM_BUYER_REMARKS,
                    string ADD_TO_VRNO,
                    string REMOVE_FROM_VRNO,
                    System.Nullable<int> SKIP_ROWS_AFT_ITEM,
                    System.Nullable<int> ITEM_NO_AS_ROWNO,
                    string COL_ITEM_COMMENTS,
                    string SAMPLE_FILE,
                    string CELL_VSL_IMONO,
                    string CELL_PORT_NAME,
                    string CELL_DOC_TYPE,
                    string COL_ITEM_SUPP_REFNO,
                    string CELL_SUPP_EXP_DT,
                    string CELL_SUPP_LATE_DT,
                    string CELL_SUPP_LEAD_DAYS,
                    string CELL_BYR_COMPANY,
                    string CELL_BYR_CONTACT,
                    string CELL_BYR_EMAIL,
                    string CELL_BYR_PHONE,
                    string CELL_BYR_MOB,
                    string CELL_BYR_FAX,
                    string CELL_SUPP_COMPANY,
                    string CELL_SUPP_CONTACT,
                    string CELL_SUPP_EMAIL,
                    string CELL_SUPP_PHONE,
                    string CELL_SUPP_MOB,
                    string CELL_SUPP_FAX,
                    string CELL_FREIGHT_AMT,
                    string CELL_OTHER_AMT,
                    string CELL_DISC_PROVSN,
                    string DISC_PROVSN_VALUE,
                    System.Nullable<int> ALT_ITEM_START_OFFSET,
                    System.Nullable<int> ALT_ITEM_COUNT,
                    string CELL_RFQ_TITLE,
                    string CELL_RFQ_DEPT,
                    string CELL_EQUIP_NAME,
                    string CELL_EQUIP_TYPE,
                    string CELL_EQUIP_MAKER,
                    string CELL_EQUIP_SERNO,
                    string CELL_EQUIP_DTLS,
                    string CELL_MSGNO,
                    System.Nullable<int> DYN_SUP_FREIGHT_OFFSET,
                    System.Nullable<int> DYN_OTHERCOST_OFFSET,
                    System.Nullable<int> DYN_SUP_CURR_OFFSET,
                    System.Nullable<int> DYN_BYR_RMRK_OFFSET,
                    System.Nullable<int> MULTILINE_ITEM_DESCR,
                    string EXCEL_NAME_MANAGER,
                    string DECIMAL_SEPARATOR,
                    string DEFAULT_UOM,
                    System.Nullable<int> DYN_HDR_DISCOUNT_OFFSET,

                    string REMOVE_FROM_VESSEL_NAME,
                    string CELL_BYR_ADDR1,
                    string CELL_BYR_ADDR2,
                    string CELL_SUPP_ADDR1,
                    string CELL_SUPP_ADDR2,
                    string CELL_BILL_COMPANY,
                    string CELL_BILL_CONTACT,
                    string CELL_BILL_EMAIL,
                    string CELL_BILL_PHONE,
                    string CELL_BILL_MOB,
                    string CELL_BILL_FAX,
                    string CELL_BILL_ADDR1,
                    string CELL_BILL_ADDR2,
                    string CELL_SHIP_COMPANY,
                    string CELL_SHIP_CONTACT,
                    string CELL_SHIP_EMAIL,
                    string CELL_SHIP_PHONE,
                    string CELL_SHIP_MOB,
                    string CELL_SHIP_FAX,
                    string CELL_SHIP_ADDR1,
                    string CELL_SHIP_ADDR2,
                    string CELL_ORDER_IDENTIFIER,
                    string CELL_SUPP_QUOTE_DT,
                    string COL_ITEM_ALT_NAME,
                    string CELL_ETA_DATE,
                    string CELL_ETD_DATE
        )
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_GROUP_MAPPING_Update");
            this._dataAccess.AddParameter("EXCEL_MAPID", EXCEL_MAPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUP_ID", GROUP_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("XLS_MAP_CODE", XLS_MAP_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SECTION_ROW_START", SECTION_ROW_START, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_ROW_START", ITEM_ROW_START, ParameterDirection.Input);
            this._dataAccess.AddParameter("SKIP_ROWS_BEF_ITEM", SKIP_ROWS_BEF_ITEM, ParameterDirection.Input);
            this._dataAccess.AddParameter("SKIP_ROWS_AFT_SECTION", SKIP_ROWS_AFT_SECTION, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_VRNO", CELL_VRNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_RFQ_DT", CELL_RFQ_DT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_VESSEL", CELL_VESSEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_PORT", CELL_PORT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_LATE_DT", CELL_LATE_DT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_REF", CELL_SUPP_REF, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_VALID_UPTO", CELL_VALID_UPTO, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_CURR_CODE", CELL_CURR_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_CONTACT", CELL_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_PAY_TERMS", CELL_PAY_TERMS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_DEL_TERMS", CELL_DEL_TERMS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BUYER_REMARKS", CELL_BUYER_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPLR_REMARKS", CELL_SUPLR_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_SECTION", COL_SECTION, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEMNO", COL_ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_REFNO", COL_ITEM_REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_NAME", COL_ITEM_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_QTY", COL_ITEM_QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_UNIT", COL_ITEM_UNIT, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_PRICE", COL_ITEM_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_DISCOUNT", COL_ITEM_DISCOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_ALT_QTY", COL_ITEM_ALT_QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_ALT_UNIT", COL_ITEM_ALT_UNIT, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_ALT_PRICE", COL_ITEM_ALT_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_DEL_DAYS", COL_ITEM_DEL_DAYS, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_REMARKS", COL_ITEM_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("ACTIVE_SHEET", ACTIVE_SHEET, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXIT_FOR_NOITEM", EXIT_FOR_NOITEM, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_CURR", COL_ITEM_CURR, ParameterDirection.Input);
            this._dataAccess.AddParameter("DYN_SUP_RMRK_OFFSET", DYN_SUP_RMRK_OFFSET, ParameterDirection.Input);
            this._dataAccess.AddParameter("OVERRIDE_ALT_QTY", OVERRIDE_ALT_QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("SKIP_HIDDEN_ROWS", SKIP_HIDDEN_ROWS, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_DISC_PERCNT", ITEM_DISC_PERCNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_TOTAL", COL_ITEM_TOTAL, ParameterDirection.Input);
            this._dataAccess.AddParameter("APPLY_TOTAL_FORMULA", APPLY_TOTAL_FORMULA, ParameterDirection.Input);
            this._dataAccess.AddParameter("READ_ITEM_REMARKS_UPTO_NO", READ_ITEM_REMARKS_UPTO_NO, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_BUYER_REMARKS", COL_ITEM_BUYER_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADD_TO_VRNO", ADD_TO_VRNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("REMOVE_FROM_VRNO", REMOVE_FROM_VRNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("SKIP_ROWS_AFT_ITEM", SKIP_ROWS_AFT_ITEM, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_NO_AS_ROWNO", ITEM_NO_AS_ROWNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_COMMENTS", COL_ITEM_COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("SAMPLE_FILE", SAMPLE_FILE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_VSL_IMONO", CELL_VSL_IMONO, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_PORT_NAME", CELL_PORT_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_DOC_TYPE", CELL_DOC_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_SUPP_REFNO", COL_ITEM_SUPP_REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_EXP_DT", CELL_SUPP_EXP_DT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_LATE_DT", CELL_SUPP_LATE_DT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_LEAD_DAYS", CELL_SUPP_LEAD_DAYS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BYR_COMPANY", CELL_BYR_COMPANY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BYR_CONTACT", CELL_BYR_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BYR_EMAIL", CELL_BYR_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BYR_PHONE", CELL_BYR_PHONE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BYR_MOB", CELL_BYR_MOB, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BYR_FAX", CELL_BYR_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_COMPANY", CELL_SUPP_COMPANY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_CONTACT", CELL_SUPP_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_EMAIL", CELL_SUPP_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_PHONE", CELL_SUPP_PHONE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_MOB", CELL_SUPP_MOB, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_FAX", CELL_SUPP_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_FREIGHT_AMT", CELL_FREIGHT_AMT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_OTHER_AMT", CELL_OTHER_AMT, ParameterDirection.Input);

            // Added  on 10-MARCH-2015
            this._dataAccess.AddParameter("CELL_DISC_PROVSN", CELL_DISC_PROVSN, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISC_PROVSN_VALUE", DISC_PROVSN_VALUE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ALT_ITEM_START_OFFSET", ALT_ITEM_START_OFFSET, ParameterDirection.Input);
            this._dataAccess.AddParameter("ALT_ITEM_COUNT", ALT_ITEM_COUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_RFQ_TITLE", CELL_RFQ_TITLE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_RFQ_DEPT", CELL_RFQ_DEPT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_EQUIP_NAME", CELL_EQUIP_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_EQUIP_TYPE", CELL_EQUIP_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_EQUIP_MAKER", CELL_EQUIP_MAKER, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_EQUIP_SERNO", CELL_EQUIP_SERNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_EQUIP_DTLS", CELL_EQUIP_DTLS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_MSGNO", CELL_MSGNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("DYN_SUP_FREIGHT_OFFSET", DYN_SUP_FREIGHT_OFFSET, ParameterDirection.Input);

            this._dataAccess.AddParameter("DYN_OTHERCOST_OFFSET", DYN_OTHERCOST_OFFSET, ParameterDirection.Input);
            this._dataAccess.AddParameter("DYN_SUP_CURR_OFFSET", DYN_SUP_CURR_OFFSET, ParameterDirection.Input);
            this._dataAccess.AddParameter("DYN_BYR_RMRK_OFFSET", DYN_BYR_RMRK_OFFSET, ParameterDirection.Input);
            this._dataAccess.AddParameter("MULTILINE_ITEM_DESCR", MULTILINE_ITEM_DESCR, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXCEL_NAME_MANAGER", EXCEL_NAME_MANAGER, ParameterDirection.Input);
            this._dataAccess.AddParameter("DECIMAL_SEPARATOR", DECIMAL_SEPARATOR, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEFAULT_UOM", DEFAULT_UOM, ParameterDirection.Input);

            this._dataAccess.AddParameter("DYN_HDR_DISCOUNT_OFFSET", DYN_HDR_DISCOUNT_OFFSET, ParameterDirection.Input);
            this._dataAccess.AddParameter("REMOVE_FROM_VESSEL_NAME", REMOVE_FROM_VESSEL_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BYR_ADDR1", CELL_BYR_ADDR1, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BYR_ADDR2", CELL_BYR_ADDR2, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_ADDR1", CELL_SUPP_ADDR1, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SUPP_ADDR2", CELL_SUPP_ADDR2, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BILL_COMPANY", CELL_BILL_COMPANY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BILL_CONTACT", CELL_BILL_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BILL_EMAIL", CELL_BILL_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BILL_PHONE", CELL_BILL_PHONE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BILL_MOB", CELL_BILL_MOB, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BILL_FAX", CELL_BILL_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BILL_ADDR1", CELL_BILL_ADDR1, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_BILL_ADDR2", CELL_BILL_ADDR2, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SHIP_COMPANY", CELL_SHIP_COMPANY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SHIP_CONTACT", CELL_SHIP_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SHIP_EMAIL", CELL_SHIP_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SHIP_PHONE", CELL_SHIP_PHONE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SHIP_MOB", CELL_SHIP_MOB, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SHIP_FAX", CELL_SHIP_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SHIP_ADDR1", CELL_SHIP_ADDR1, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_SHIP_ADDR2", CELL_SHIP_ADDR2, ParameterDirection.Input);

            this._dataAccess.AddParameter("CELL_ORDER_IDENTIFIER", CELL_ORDER_IDENTIFIER, ParameterDirection.Input);

            // added on 06-APRIL-2017
            this._dataAccess.AddParameter("CELL_SUPP_QUOTE_DT", CELL_SUPP_QUOTE_DT, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_ALT_NAME", COL_ITEM_ALT_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_ETA_DATE", CELL_ETA_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL_ETD_DATE", CELL_ETD_DATE, ParameterDirection.Input);


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
