namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class LesSupplierSubsupplierLink : IDisposable {
        
        private DataAccess _dataAccess;
        
        public LesSupplierSubsupplierLink() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet Select_LES_SUPPLIER_SUBSUPPLIER_LINKs_By_SUPPLIERID(System.Nullable<int> SUPPLIERID) {
            this._dataAccess.CreateProcedureCommand("sp_Select_LES_SUPPLIER_SUBSUPPLIER_LINKs_By_SUPPLIERID");
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet Select_LES_SUPPLIER_SUBSUPPLIER_LINKs_By_SUBSUPPLIERID(System.Nullable<int> SUBSUPPLIERID) {
            this._dataAccess.CreateProcedureCommand("sp_Select_LES_SUPPLIER_SUBSUPPLIER_LINKs_By_SUBSUPPLIERID");
            this._dataAccess.AddParameter("SUBSUPPLIERID", SUBSUPPLIERID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_SUPPLIER_SUBSUPPLIER_LINK_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_LES_SUPPLIER_SUBSUPPLIER_LINK_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_SUPPLIER_SUBSUPPLIER_LINK_Select_One(System.Nullable<int> SUPPLINKID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_SUPPLIER_SUBSUPPLIER_LINK_Select_One");
            this._dataAccess.AddParameter("SUPPLINKID", SUPPLINKID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> LES_SUPPLIER_SUBSUPPLIER_LINK_Insert(System.Nullable<int> SUPPLIERID, System.Nullable<int> SUBSUPPLIERID, System.Nullable<System.DateTime> UPDATE_DATE, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<int> UPDATED_BY, System.Nullable<int> CREATED_BY, string SUBSUPPLIER_EMAIL, string SUPPLIER_EMAIL, string CC_EMAIL, string BCC_EMAIL, string MAIL_SUBJECT, string REPLY_EMAIL, System.Nullable<int> BILLTO_ADDRESSID, System.Nullable<int> SHIPTO_ADDRESSID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_SUPPLIER_SUBSUPPLIER_LINK_Insert");
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUBSUPPLIERID", SUBSUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATED_BY", UPDATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_BY", CREATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUBSUPPLIER_EMAIL", SUBSUPPLIER_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_EMAIL", SUPPLIER_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CC_EMAIL", CC_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("BCC_EMAIL", BCC_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_SUBJECT", MAIL_SUBJECT, ParameterDirection.Input);
            this._dataAccess.AddParameter("REPLY_EMAIL", REPLY_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("BILLTO_ADDRESSID", BILLTO_ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SHIPTO_ADDRESSID", SHIPTO_ADDRESSID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_SUPPLIER_SUBSUPPLIER_LINK_Delete(System.Nullable<int> SUPPLINKID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_SUPPLIER_SUBSUPPLIER_LINK_Delete");
            this._dataAccess.AddParameter("SUPPLINKID", SUPPLINKID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_SUPPLIER_SUBSUPPLIER_LINK_Update(System.Nullable<int> SUPPLINKID, System.Nullable<int> SUPPLIERID, System.Nullable<int> SUBSUPPLIERID, System.Nullable<System.DateTime> UPDATE_DATE, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<int> UPDATED_BY, System.Nullable<int> CREATED_BY, string SUBSUPPLIER_EMAIL, string SUPPLIER_EMAIL, string CC_EMAIL, string BCC_EMAIL, string MAIL_SUBJECT, string REPLY_EMAIL, System.Nullable<int> BILLTO_ADDRESSID, System.Nullable<int> SHIPTO_ADDRESSID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_SUPPLIER_SUBSUPPLIER_LINK_Update");
            this._dataAccess.AddParameter("SUPPLINKID", SUPPLINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUBSUPPLIERID", SUBSUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATED_BY", UPDATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_BY", CREATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUBSUPPLIER_EMAIL", SUBSUPPLIER_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_EMAIL", SUPPLIER_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CC_EMAIL", CC_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("BCC_EMAIL", BCC_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_SUBJECT", MAIL_SUBJECT, ParameterDirection.Input);
            this._dataAccess.AddParameter("REPLY_EMAIL", REPLY_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("BILLTO_ADDRESSID", BILLTO_ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SHIPTO_ADDRESSID", SHIPTO_ADDRESSID, ParameterDirection.Input);
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
