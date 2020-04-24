namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmByrSuppMailConfig : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmByrSuppMailConfig() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_BYR_SUPP_MAIL_CONFIG_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_BYR_SUPP_MAIL_CONFIG_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_BYR_SUPP_MAIL_CONFIG_Select_One(System.Nullable<int> CONFIG_ID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_BYR_SUPP_MAIL_CONFIG_Select_One");
            this._dataAccess.AddParameter("CONFIG_ID", CONFIG_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_BYR_SUPP_MAIL_CONFIG_Insert(System.Nullable<int> BYR_SUP_LINKID, System.Nullable<int> BUYER_ID, System.Nullable<int> SUPPLIER_ID, string SMTP_HOST, System.Nullable<int> SMTP_PORT, string FROM_EMAIL, string FROM_USER, string FROM_PWD, string DISPLAY_NAME, string REPLY_EMAIL, System.Nullable<int> IS_SSL, System.Nullable<int> IS_AUTHORISED, string UDF1, string UDF2) {
            this._dataAccess.CreateProcedureCommand("sp_SM_BYR_SUPP_MAIL_CONFIG_Insert");
            this._dataAccess.AddParameter("BYR_SUP_LINKID", BYR_SUP_LINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_ID", BUYER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_ID", SUPPLIER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SMTP_HOST", SMTP_HOST, ParameterDirection.Input);
            this._dataAccess.AddParameter("SMTP_PORT", SMTP_PORT, ParameterDirection.Input);
            this._dataAccess.AddParameter("FROM_EMAIL", FROM_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("FROM_USER", FROM_USER, ParameterDirection.Input);
            this._dataAccess.AddParameter("FROM_PWD", FROM_PWD, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISPLAY_NAME", DISPLAY_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("REPLY_EMAIL", REPLY_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("IS_SSL", IS_SSL, ParameterDirection.Input);
            this._dataAccess.AddParameter("IS_AUTHORISED", IS_AUTHORISED, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF1", UDF1, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF2", UDF2, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_BYR_SUPP_MAIL_CONFIG_Delete(System.Nullable<int> CONFIG_ID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_BYR_SUPP_MAIL_CONFIG_Delete");
            this._dataAccess.AddParameter("CONFIG_ID", CONFIG_ID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_BYR_SUPP_MAIL_CONFIG_Update(System.Nullable<int> CONFIG_ID, System.Nullable<int> BYR_SUP_LINKID, System.Nullable<int> BUYER_ID, System.Nullable<int> SUPPLIER_ID, string SMTP_HOST, System.Nullable<int> SMTP_PORT, string FROM_EMAIL, string FROM_USER, string FROM_PWD, string DISPLAY_NAME, string REPLY_EMAIL, System.Nullable<int> IS_SSL, System.Nullable<int> IS_AUTHORISED, string UDF1, string UDF2) {
            this._dataAccess.CreateProcedureCommand("sp_SM_BYR_SUPP_MAIL_CONFIG_Update");
            this._dataAccess.AddParameter("CONFIG_ID", CONFIG_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BYR_SUP_LINKID", BYR_SUP_LINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_ID", BUYER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_ID", SUPPLIER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SMTP_HOST", SMTP_HOST, ParameterDirection.Input);
            this._dataAccess.AddParameter("SMTP_PORT", SMTP_PORT, ParameterDirection.Input);
            this._dataAccess.AddParameter("FROM_EMAIL", FROM_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("FROM_USER", FROM_USER, ParameterDirection.Input);
            this._dataAccess.AddParameter("FROM_PWD", FROM_PWD, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISPLAY_NAME", DISPLAY_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("REPLY_EMAIL", REPLY_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("IS_SSL", IS_SSL, ParameterDirection.Input);
            this._dataAccess.AddParameter("IS_AUTHORISED", IS_AUTHORISED, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF1", UDF1, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF2", UDF2, ParameterDirection.Input);
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
