namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmXlsBuyerMapping : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmXlsBuyerMapping() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_XLS_BUYER_MAPPING_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_BUYER_MAPPING_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_XLS_BUYER_MAPPING_Select_One(System.Nullable<int> MAPID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_BUYER_MAPPING_Select_One");
            this._dataAccess.AddParameter("MAPID", MAPID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_XLS_BUYER_MAPPING_Insert(
                    System.Nullable<int> BUYERID, 
                    System.Nullable<int> SUPPLIERID, 
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
                    string CELL_MAPPING, 
                    string MAPPING_VAL1, 
                    string MAPPING_VAL2, 
                    string CELL2_MAPPING, 
                    string CELL2_MAP_VALUE, 
                    System.Nullable<short> EXIT_FOR_NOITEM, 
                    string COL_ITEM_CURR, 
                    System.Nullable<int> DYN_SUP_RMRK_OFFSET, 
                    System.Nullable<int> Override_ALT_QTY, 
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
                    string COL_ITEM_COMMENTS) {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_BUYER_MAPPING_Insert");
            this._dataAccess.AddParameter("BUYERID", BUYERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
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
            this._dataAccess.AddParameter("CELL_MAPPING", CELL_MAPPING, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAPPING_VAL1", MAPPING_VAL1, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAPPING_VAL2", MAPPING_VAL2, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL2_MAPPING", CELL2_MAPPING, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL2_MAP_VALUE", CELL2_MAP_VALUE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXIT_FOR_NOITEM", EXIT_FOR_NOITEM, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_CURR", COL_ITEM_CURR, ParameterDirection.Input);
            this._dataAccess.AddParameter("DYN_SUP_RMRK_OFFSET", DYN_SUP_RMRK_OFFSET, ParameterDirection.Input);
            this._dataAccess.AddParameter("Override_ALT_QTY", Override_ALT_QTY, ParameterDirection.Input);
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
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_XLS_BUYER_MAPPING_Delete(System.Nullable<int> MAPID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_BUYER_MAPPING_Delete");
            this._dataAccess.AddParameter("MAPID", MAPID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_XLS_BUYER_MAPPING_Update(
                    System.Nullable<int> MAPID, 
                    System.Nullable<int> BUYERID, 
                    System.Nullable<int> SUPPLIERID, 
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
                    string CELL_MAPPING, 
                    string MAPPING_VAL1, 
                    string MAPPING_VAL2, 
                    string CELL2_MAPPING, 
                    string CELL2_MAP_VALUE, 
                    System.Nullable<short> EXIT_FOR_NOITEM, 
                    string COL_ITEM_CURR, 
                    System.Nullable<int> DYN_SUP_RMRK_OFFSET, 
                    System.Nullable<int> Override_ALT_QTY, 
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
                    string COL_ITEM_COMMENTS) {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_BUYER_MAPPING_Update");
            this._dataAccess.AddParameter("MAPID", MAPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYERID", BUYERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
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
            this._dataAccess.AddParameter("CELL_MAPPING", CELL_MAPPING, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAPPING_VAL1", MAPPING_VAL1, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAPPING_VAL2", MAPPING_VAL2, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL2_MAPPING", CELL2_MAPPING, ParameterDirection.Input);
            this._dataAccess.AddParameter("CELL2_MAP_VALUE", CELL2_MAP_VALUE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXIT_FOR_NOITEM", EXIT_FOR_NOITEM, ParameterDirection.Input);
            this._dataAccess.AddParameter("COL_ITEM_CURR", COL_ITEM_CURR, ParameterDirection.Input);
            this._dataAccess.AddParameter("DYN_SUP_RMRK_OFFSET", DYN_SUP_RMRK_OFFSET, ParameterDirection.Input);
            this._dataAccess.AddParameter("Override_ALT_QTY", Override_ALT_QTY, ParameterDirection.Input);
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
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual void Dispose() {
            if ((this._dataAccess != null)) {
                this._dataAccess.Dispose();
            }
        }
    }
}
