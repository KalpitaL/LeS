namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmMailTransactionLog : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmMailTransactionLog() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_MAIL_TRANSACTION_LOG_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_MAIL_TRANSACTION_LOG_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_MAIL_TRANSACTION_LOG_Select_One(System.Nullable<int> TRANS_ID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_MAIL_TRANSACTION_LOG_Select_One");
            this._dataAccess.AddParameter("TRANS_ID", TRANS_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_MAIL_TRANSACTION_LOG_Insert(string MODULE_NAME, string LOG_TYPE, string FROM_MAIL, string TO_MAIL, string KEY_REFNO, System.Nullable<int> LINKID, System.Nullable<int> BUYER_ID, System.Nullable<int> SUPPLIER_ID, string MSGFILE, string FILE1, string FILE2, string MAIL_SUBJECT, System.Nullable<int> TRANS_STATUS, System.Nullable<System.DateTime> UPDATEDATE, string COMMENTS) {
            this._dataAccess.CreateProcedureCommand("sp_SM_MAIL_TRANSACTION_LOG_Insert");
            this._dataAccess.AddParameter("MODULE_NAME", MODULE_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("LOG_TYPE", LOG_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("FROM_MAIL", FROM_MAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("TO_MAIL", TO_MAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("KEY_REFNO", KEY_REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINKID", LINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_ID", BUYER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_ID", SUPPLIER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MSGFILE", MSGFILE, ParameterDirection.Input);
            this._dataAccess.AddParameter("FILE1", FILE1, ParameterDirection.Input);
            this._dataAccess.AddParameter("FILE2", FILE2, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_SUBJECT", MAIL_SUBJECT, ParameterDirection.Input);
            this._dataAccess.AddParameter("TRANS_STATUS", TRANS_STATUS, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATEDATE", UPDATEDATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("COMMENTS", COMMENTS, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_MAIL_TRANSACTION_LOG_Delete(System.Nullable<int> TRANS_ID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_MAIL_TRANSACTION_LOG_Delete");
            this._dataAccess.AddParameter("TRANS_ID", TRANS_ID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_MAIL_TRANSACTION_LOG_Update(
                    System.Nullable<int> TRANS_ID, 
                    string MODULE_NAME, 
                    string LOG_TYPE, 
                    string FROM_MAIL, 
                    string TO_MAIL, 
                    string KEY_REFNO, 
                    System.Nullable<int> LINKID, 
                    System.Nullable<int> BUYER_ID, 
                    System.Nullable<int> SUPPLIER_ID, 
                    string MSGFILE, 
                    string FILE1, 
                    string FILE2, 
                    string MAIL_SUBJECT, 
                    System.Nullable<int> TRANS_STATUS, 
                    System.Nullable<System.DateTime> UPDATEDATE, 
                    string COMMENTS) {
            this._dataAccess.CreateProcedureCommand("sp_SM_MAIL_TRANSACTION_LOG_Update");
            this._dataAccess.AddParameter("TRANS_ID", TRANS_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MODULE_NAME", MODULE_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("LOG_TYPE", LOG_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("FROM_MAIL", FROM_MAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("TO_MAIL", TO_MAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("KEY_REFNO", KEY_REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINKID", LINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_ID", BUYER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_ID", SUPPLIER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MSGFILE", MSGFILE, ParameterDirection.Input);
            this._dataAccess.AddParameter("FILE1", FILE1, ParameterDirection.Input);
            this._dataAccess.AddParameter("FILE2", FILE2, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_SUBJECT", MAIL_SUBJECT, ParameterDirection.Input);
            this._dataAccess.AddParameter("TRANS_STATUS", TRANS_STATUS, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATEDATE", UPDATEDATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("COMMENTS", COMMENTS, ParameterDirection.Input);
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
