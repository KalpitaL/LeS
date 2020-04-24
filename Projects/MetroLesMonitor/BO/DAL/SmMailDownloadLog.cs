namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmMailDownloadLog : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmMailDownloadLog() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_MAIL_DOWNLOAD_LOG_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_MAIL_DOWNLOAD_LOG_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_MAIL_DOWNLOAD_LOG_Select_One(System.Nullable<int> LogID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_MAIL_DOWNLOAD_LOG_Select_One");
            this._dataAccess.AddParameter("LogID", LogID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_MAIL_DOWNLOAD_LOG_Insert(string MessageID, string ModuleName, string LogType, string FromMail, string ToMail, string AuditValue, string KeyRef1, string KeyRef2, System.Nullable<System.DateTime> UpdateDate, System.Nullable<int> BUYER_ID, System.Nullable<int> SUPPLIER_ID, string MailSubject) {
            this._dataAccess.CreateProcedureCommand("sp_SM_MAIL_DOWNLOAD_LOG_Insert");
            this._dataAccess.AddParameter("MessageID", MessageID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ModuleName", ModuleName, ParameterDirection.Input);
            this._dataAccess.AddParameter("LogType", LogType, ParameterDirection.Input);
            this._dataAccess.AddParameter("FromMail", FromMail, ParameterDirection.Input);
            this._dataAccess.AddParameter("ToMail", ToMail, ParameterDirection.Input);
            this._dataAccess.AddParameter("AuditValue", AuditValue, ParameterDirection.Input);
            this._dataAccess.AddParameter("KeyRef1", KeyRef1, ParameterDirection.Input);
            this._dataAccess.AddParameter("KeyRef2", KeyRef2, ParameterDirection.Input);
            this._dataAccess.AddParameter("UpdateDate", UpdateDate, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_ID", BUYER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_ID", SUPPLIER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MailSubject", MailSubject, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_MAIL_DOWNLOAD_LOG_Delete(System.Nullable<int> LogID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_MAIL_DOWNLOAD_LOG_Delete");
            this._dataAccess.AddParameter("LogID", LogID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_MAIL_DOWNLOAD_LOG_Update(System.Nullable<int> LogID, string MessageID, string ModuleName, string LogType, string FromMail, string ToMail, string AuditValue, string KeyRef1, string KeyRef2, System.Nullable<System.DateTime> UpdateDate, System.Nullable<int> BUYER_ID, System.Nullable<int> SUPPLIER_ID, string MailSubject) {
            this._dataAccess.CreateProcedureCommand("sp_SM_MAIL_DOWNLOAD_LOG_Update");
            this._dataAccess.AddParameter("LogID", LogID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MessageID", MessageID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ModuleName", ModuleName, ParameterDirection.Input);
            this._dataAccess.AddParameter("LogType", LogType, ParameterDirection.Input);
            this._dataAccess.AddParameter("FromMail", FromMail, ParameterDirection.Input);
            this._dataAccess.AddParameter("ToMail", ToMail, ParameterDirection.Input);
            this._dataAccess.AddParameter("AuditValue", AuditValue, ParameterDirection.Input);
            this._dataAccess.AddParameter("KeyRef1", KeyRef1, ParameterDirection.Input);
            this._dataAccess.AddParameter("KeyRef2", KeyRef2, ParameterDirection.Input);
            this._dataAccess.AddParameter("UpdateDate", UpdateDate, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_ID", BUYER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_ID", SUPPLIER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MailSubject", MailSubject, ParameterDirection.Input);
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
