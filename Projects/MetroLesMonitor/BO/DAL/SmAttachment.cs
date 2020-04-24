namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmAttachment : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmAttachment() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_ATTACHMENT_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_ATTACHMENT_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_ATTACHMENT_Select_One(System.Nullable<int> ATTACHMENTID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_ATTACHMENT_Select_One");
            this._dataAccess.AddParameter("ATTACHMENTID", ATTACHMENTID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_ATTACHMENT_Insert(System.Nullable<int> QUOTATIONID, string ATTACHMENTTYPE, string FILE_NAME, System.Nullable<short> EXPORTED, System.Nullable<System.DateTime> CREATED_DATE, string FILE_LOCATION) {
            this._dataAccess.CreateProcedureCommand("sp_SM_ATTACHMENT_Insert");
            this._dataAccess.AddParameter("QUOTATIONID", QUOTATIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ATTACHMENTTYPE", ATTACHMENTTYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("FILE_NAME", FILE_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORTED", EXPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("FILE_LOCATION", FILE_LOCATION, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_ATTACHMENT_Delete(System.Nullable<int> ATTACHMENTID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_ATTACHMENT_Delete");
            this._dataAccess.AddParameter("ATTACHMENTID", ATTACHMENTID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_ATTACHMENT_Update(System.Nullable<int> ATTACHMENTID, System.Nullable<int> QUOTATIONID, string ATTACHMENTTYPE, string FILE_NAME, System.Nullable<short> EXPORTED, System.Nullable<System.DateTime> CREATED_DATE, string FILE_LOCATION) {
            this._dataAccess.CreateProcedureCommand("sp_SM_ATTACHMENT_Update");
            this._dataAccess.AddParameter("ATTACHMENTID", ATTACHMENTID, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTATIONID", QUOTATIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ATTACHMENTTYPE", ATTACHMENTTYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("FILE_NAME", FILE_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORTED", EXPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("FILE_LOCATION", FILE_LOCATION, ParameterDirection.Input);
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
