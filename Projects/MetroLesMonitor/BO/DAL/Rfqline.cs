namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class Rfqline : IDisposable {
        
        private DataAccess _dataAccess;
        
        public Rfqline() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet Select_RFQLINEs_By_MAS_REC_KEY(System.Nullable<int> MAS_REC_KEY) {
            this._dataAccess.CreateProcedureCommand("sp_Select_RFQLINEs_By_MAS_REC_KEY");
            this._dataAccess.AddParameter("MAS_REC_KEY", MAS_REC_KEY, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet RFQLINE_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_RFQLINE_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet RFQLINE_Select_One(System.Nullable<int> REC_KEY) {
            this._dataAccess.CreateProcedureCommand("sp_RFQLINE_Select_One");
            this._dataAccess.AddParameter("REC_KEY", REC_KEY, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual void RFQLINE_Insert(
                    System.Nullable<int> MAS_REC_KEY, 
                    System.Nullable<int> REC_KEY, 
                    System.Nullable<int> LINE_NO, 
                    string LINE_REF, 
                    System.Nullable<char> LINE_TYPE, 
                    string STK_ID, 
                    string NAME, 
                    string MODEL, 
                    System.Nullable<decimal> UOM_QTY, 
                    string UOM, 
                    System.Nullable<decimal> LIST_PRICE, 
                    string DISC_CHR, 
                    System.Nullable<decimal> DISC_NUM, 
                    System.Nullable<decimal> NET_PRICE, 
                    string REMARK, 
                    string SRC_CODE, 
                    string SRC_LOC_ID, 
                    System.Nullable<int> SRC_REC_KEY, 
                    System.Nullable<int> SRC_LINE_REC_KEY, 
                    string SRC_DOC_ID, 
                    System.Nullable<int> ORI_REC_KEY) {
            this._dataAccess.CreateProcedureCommand("sp_RFQLINE_Insert");
            this._dataAccess.AddParameter("MAS_REC_KEY", MAS_REC_KEY, ParameterDirection.Input);
            this._dataAccess.AddParameter("REC_KEY", REC_KEY, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINE_NO", LINE_NO, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINE_REF", LINE_REF, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINE_TYPE", LINE_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("STK_ID", STK_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("NAME", NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("MODEL", MODEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("UOM_QTY", UOM_QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("UOM", UOM, ParameterDirection.Input);
            this._dataAccess.AddParameter("LIST_PRICE", LIST_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISC_CHR", DISC_CHR, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISC_NUM", DISC_NUM, ParameterDirection.Input);
            this._dataAccess.AddParameter("NET_PRICE", NET_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("REMARK", REMARK, ParameterDirection.Input);
            this._dataAccess.AddParameter("SRC_CODE", SRC_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SRC_LOC_ID", SRC_LOC_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SRC_REC_KEY", SRC_REC_KEY, ParameterDirection.Input);
            this._dataAccess.AddParameter("SRC_LINE_REC_KEY", SRC_LINE_REC_KEY, ParameterDirection.Input);
            this._dataAccess.AddParameter("SRC_DOC_ID", SRC_DOC_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORI_REC_KEY", ORI_REC_KEY, ParameterDirection.Input);
            this._dataAccess.ExecuteNonQuery();
        }
        
        public virtual System.Nullable<int> RFQLINE_Delete(System.Nullable<int> REC_KEY) {
            this._dataAccess.CreateProcedureCommand("sp_RFQLINE_Delete");
            this._dataAccess.AddParameter("REC_KEY", REC_KEY, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> RFQLINE_Update(
                    System.Nullable<int> MAS_REC_KEY, 
                    System.Nullable<int> REC_KEY, 
                    System.Nullable<int> LINE_NO, 
                    string LINE_REF, 
                    System.Nullable<char> LINE_TYPE, 
                    string STK_ID, 
                    string NAME, 
                    string MODEL, 
                    System.Nullable<decimal> UOM_QTY, 
                    string UOM, 
                    System.Nullable<decimal> LIST_PRICE, 
                    string DISC_CHR, 
                    System.Nullable<decimal> DISC_NUM, 
                    System.Nullable<decimal> NET_PRICE, 
                    string REMARK, 
                    string SRC_CODE, 
                    string SRC_LOC_ID, 
                    System.Nullable<int> SRC_REC_KEY, 
                    System.Nullable<int> SRC_LINE_REC_KEY, 
                    string SRC_DOC_ID, 
                    System.Nullable<int> ORI_REC_KEY) {
            this._dataAccess.CreateProcedureCommand("sp_RFQLINE_Update");
            this._dataAccess.AddParameter("MAS_REC_KEY", MAS_REC_KEY, ParameterDirection.Input);
            this._dataAccess.AddParameter("REC_KEY", REC_KEY, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINE_NO", LINE_NO, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINE_REF", LINE_REF, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINE_TYPE", LINE_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("STK_ID", STK_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("NAME", NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("MODEL", MODEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("UOM_QTY", UOM_QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("UOM", UOM, ParameterDirection.Input);
            this._dataAccess.AddParameter("LIST_PRICE", LIST_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISC_CHR", DISC_CHR, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISC_NUM", DISC_NUM, ParameterDirection.Input);
            this._dataAccess.AddParameter("NET_PRICE", NET_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("REMARK", REMARK, ParameterDirection.Input);
            this._dataAccess.AddParameter("SRC_CODE", SRC_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SRC_LOC_ID", SRC_LOC_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SRC_REC_KEY", SRC_REC_KEY, ParameterDirection.Input);
            this._dataAccess.AddParameter("SRC_LINE_REC_KEY", SRC_LINE_REC_KEY, ParameterDirection.Input);
            this._dataAccess.AddParameter("SRC_DOC_ID", SRC_DOC_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORI_REC_KEY", ORI_REC_KEY, ParameterDirection.Input);
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
