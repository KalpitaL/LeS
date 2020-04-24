namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class LesResendDocNotify : IDisposable {
        
        private DataAccess _dataAccess;
        
        public LesResendDocNotify() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet LES_RESEND_DOC_NOTIFY_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_LES_RESEND_DOC_NOTIFY_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_RESEND_DOC_NOTIFY_Select_One(System.Nullable<int> RESEND_ID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_RESEND_DOC_NOTIFY_Select_One");
            this._dataAccess.AddParameter("RESEND_ID", RESEND_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> LES_RESEND_DOC_NOTIFY_Insert(System.Nullable<int> QUOTATION_ID, string DOC_TYPE, System.Nullable<System.DateTime> UPDATE_DATE) {
            this._dataAccess.CreateProcedureCommand("sp_LES_RESEND_DOC_NOTIFY_Insert");
            this._dataAccess.AddParameter("QUOTATION_ID", QUOTATION_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_TYPE", DOC_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_RESEND_DOC_NOTIFY_Delete(System.Nullable<int> RESEND_ID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_RESEND_DOC_NOTIFY_Delete");
            this._dataAccess.AddParameter("RESEND_ID", RESEND_ID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_RESEND_DOC_NOTIFY_Update(System.Nullable<int> RESEND_ID, System.Nullable<int> QUOTATION_ID, string DOC_TYPE, System.Nullable<System.DateTime> UPDATE_DATE) {
            this._dataAccess.CreateProcedureCommand("sp_LES_RESEND_DOC_NOTIFY_Update");
            this._dataAccess.AddParameter("RESEND_ID", RESEND_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTATION_ID", QUOTATION_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_TYPE", DOC_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
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
