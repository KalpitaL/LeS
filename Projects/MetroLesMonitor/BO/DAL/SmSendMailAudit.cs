namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmSendMailAudit : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmSendMailAudit() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_SEND_MAIL_AUDIT_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_SEND_MAIL_AUDIT_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_SEND_MAIL_AUDIT_Select_One(System.Nullable<int> MAIL_AUDITID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_SEND_MAIL_AUDIT_Select_One");
            this._dataAccess.AddParameter("MAIL_AUDITID", MAIL_AUDITID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_SEND_MAIL_AUDIT_Insert(System.Nullable<int> QUOTATIONID, System.Nullable<int> BYR_SUPP_LINKID, string MAIL_TO, string MAIL_CC, string MAIL_BCC, string MAIL_REPLY, string MAIL_SUBJECT, string MAIL_BODY, string MAIL_ATTACHMENTS, System.Nullable<System.DateTime> MAIL_DATE, System.Nullable<System.DateTime> RECEIVED_DATE, string EML_FILE, string MAIL_GUID, string REMARKS) {
            this._dataAccess.CreateProcedureCommand("sp_SM_SEND_MAIL_AUDIT_Insert");
            this._dataAccess.AddParameter("QUOTATIONID", QUOTATIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BYR_SUPP_LINKID", BYR_SUPP_LINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_TO", MAIL_TO, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_CC", MAIL_CC, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_BCC", MAIL_BCC, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_REPLY", MAIL_REPLY, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_SUBJECT", MAIL_SUBJECT, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_BODY", MAIL_BODY, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_ATTACHMENTS", MAIL_ATTACHMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_DATE", MAIL_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("RECEIVED_DATE", RECEIVED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EML_FILE", EML_FILE, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_GUID", MAIL_GUID, ParameterDirection.Input);
            this._dataAccess.AddParameter("REMARKS", REMARKS, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_SEND_MAIL_AUDIT_Delete(System.Nullable<int> MAIL_AUDITID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_SEND_MAIL_AUDIT_Delete");
            this._dataAccess.AddParameter("MAIL_AUDITID", MAIL_AUDITID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_SEND_MAIL_AUDIT_Update(System.Nullable<int> MAIL_AUDITID, System.Nullable<int> QUOTATIONID, System.Nullable<int> BYR_SUPP_LINKID, string MAIL_TO, string MAIL_CC, string MAIL_BCC, string MAIL_REPLY, string MAIL_SUBJECT, string MAIL_BODY, string MAIL_ATTACHMENTS, System.Nullable<System.DateTime> MAIL_DATE, System.Nullable<System.DateTime> RECEIVED_DATE, string EML_FILE, string MAIL_GUID, string REMARKS) {
            this._dataAccess.CreateProcedureCommand("sp_SM_SEND_MAIL_AUDIT_Update");
            this._dataAccess.AddParameter("MAIL_AUDITID", MAIL_AUDITID, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTATIONID", QUOTATIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BYR_SUPP_LINKID", BYR_SUPP_LINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_TO", MAIL_TO, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_CC", MAIL_CC, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_BCC", MAIL_BCC, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_REPLY", MAIL_REPLY, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_SUBJECT", MAIL_SUBJECT, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_BODY", MAIL_BODY, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_ATTACHMENTS", MAIL_ATTACHMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_DATE", MAIL_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("RECEIVED_DATE", RECEIVED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EML_FILE", EML_FILE, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_GUID", MAIL_GUID, ParameterDirection.Input);
            this._dataAccess.AddParameter("REMARKS", REMARKS, ParameterDirection.Input);
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
