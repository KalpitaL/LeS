namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class Rfqmas : IDisposable {
        
        private DataAccess _dataAccess;
        
        public Rfqmas() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet RFQMAS_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_RFQMAS_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet RFQMAS_Select_One(System.Nullable<int> REC_KEY) {
            this._dataAccess.CreateProcedureCommand("sp_RFQMAS_Select_One");
            this._dataAccess.AddParameter("REC_KEY", REC_KEY, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual void RFQMAS_Insert(
                    System.Nullable<int> REC_KEY, 
                    string ORG_ID, 
                    string LOC_ID, 
                    string DOC_ID, 
                    System.Nullable<System.DateTime> DOC_DATE, 
                    string USER_ID, 
                    string USER_NAME, 
                    string EMP_ID, 
                    string EMP_NAME, 
                    string SUPP_ID, 
                    string NAME, 
                    string ATTN_TO, 
                    string CC_TO, 
                    string DEPT_ID, 
                    string DEPT_NAME, 
                    string CURR_ID, 
                    System.Nullable<decimal> CURR_RATE, 
                    string SUPP_REF, 
                    string OUR_REF, 
                    string TERM_ID, 
                    System.Nullable<System.DateTime> DLY_DATE, 
                    string VSL_ID, 
                    string VSL_NAME, 
                    string MARKING, 
                    string REMARK) {
            this._dataAccess.CreateProcedureCommand("sp_RFQMAS_Insert");
            this._dataAccess.AddParameter("REC_KEY", REC_KEY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORG_ID", ORG_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("LOC_ID", LOC_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_ID", DOC_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_DATE", DOC_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("USER_ID", USER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("USER_NAME", USER_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("EMP_ID", EMP_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("EMP_NAME", EMP_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_ID", SUPP_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("NAME", NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("ATTN_TO", ATTN_TO, ParameterDirection.Input);
            this._dataAccess.AddParameter("CC_TO", CC_TO, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEPT_ID", DEPT_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEPT_NAME", DEPT_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_ID", CURR_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_RATE", CURR_RATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_REF", SUPP_REF, ParameterDirection.Input);
            this._dataAccess.AddParameter("OUR_REF", OUR_REF, ParameterDirection.Input);
            this._dataAccess.AddParameter("TERM_ID", TERM_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DLY_DATE", DLY_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("VSL_ID", VSL_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("VSL_NAME", VSL_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("MARKING", MARKING, ParameterDirection.Input);
            this._dataAccess.AddParameter("REMARK", REMARK, ParameterDirection.Input);
            this._dataAccess.ExecuteNonQuery();
        }
        
        public virtual System.Nullable<int> RFQMAS_Delete(System.Nullable<int> REC_KEY) {
            this._dataAccess.CreateProcedureCommand("sp_RFQMAS_Delete");
            this._dataAccess.AddParameter("REC_KEY", REC_KEY, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> RFQMAS_Update(
                    System.Nullable<int> REC_KEY, 
                    string ORG_ID, 
                    string LOC_ID, 
                    string DOC_ID, 
                    System.Nullable<System.DateTime> DOC_DATE, 
                    string USER_ID, 
                    string USER_NAME, 
                    string EMP_ID, 
                    string EMP_NAME, 
                    string SUPP_ID, 
                    string NAME, 
                    string ATTN_TO, 
                    string CC_TO, 
                    string DEPT_ID, 
                    string DEPT_NAME, 
                    string CURR_ID, 
                    System.Nullable<decimal> CURR_RATE, 
                    string SUPP_REF, 
                    string OUR_REF, 
                    string TERM_ID, 
                    System.Nullable<System.DateTime> DLY_DATE, 
                    string VSL_ID, 
                    string VSL_NAME, 
                    string MARKING, 
                    string REMARK) {
            this._dataAccess.CreateProcedureCommand("sp_RFQMAS_Update");
            this._dataAccess.AddParameter("REC_KEY", REC_KEY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORG_ID", ORG_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("LOC_ID", LOC_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_ID", DOC_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_DATE", DOC_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("USER_ID", USER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("USER_NAME", USER_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("EMP_ID", EMP_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("EMP_NAME", EMP_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_ID", SUPP_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("NAME", NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("ATTN_TO", ATTN_TO, ParameterDirection.Input);
            this._dataAccess.AddParameter("CC_TO", CC_TO, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEPT_ID", DEPT_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEPT_NAME", DEPT_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_ID", CURR_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_RATE", CURR_RATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_REF", SUPP_REF, ParameterDirection.Input);
            this._dataAccess.AddParameter("OUR_REF", OUR_REF, ParameterDirection.Input);
            this._dataAccess.AddParameter("TERM_ID", TERM_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DLY_DATE", DLY_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("VSL_ID", VSL_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("VSL_NAME", VSL_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("MARKING", MARKING, ParameterDirection.Input);
            this._dataAccess.AddParameter("REMARK", REMARK, ParameterDirection.Input);
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
