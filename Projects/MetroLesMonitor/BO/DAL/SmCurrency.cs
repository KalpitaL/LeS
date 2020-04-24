namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmCurrency : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmCurrency() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_CURRENCY_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_CURRENCY_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_CURRENCY_Select_One(System.Nullable<int> CURRENCYID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_CURRENCY_Select_One");
            this._dataAccess.AddParameter("CURRENCYID", CURRENCYID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual void SM_CURRENCY_Insert(System.Nullable<int> CURRENCYID, string CURR_CODE, string CURR_DESCR, System.Nullable<float> EXCH_RATE, System.Nullable<System.DateTime> CURR_VALIDITY_DATE, string CURR_REMARKS, System.Nullable<int> EXPORTED, System.Nullable<System.DateTime> UPDATE_DATE, System.Nullable<int> UPDATE_SITE, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<int> SITEID, string CURR_SYMBOL, System.Nullable<System.DateTime> VALID_FROM, System.Nullable<int> CREATED_BY, System.Nullable<int> UPDATE_BY) {
            this._dataAccess.CreateProcedureCommand("sp_SM_CURRENCY_Insert");
            this._dataAccess.AddParameter("CURRENCYID", CURRENCYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_CODE", CURR_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_DESCR", CURR_DESCR, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXCH_RATE", EXCH_RATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_VALIDITY_DATE", CURR_VALIDITY_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_REMARKS", CURR_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORTED", EXPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_SITE", UPDATE_SITE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SITEID", SITEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_SYMBOL", CURR_SYMBOL, ParameterDirection.Input);
            this._dataAccess.AddParameter("VALID_FROM", VALID_FROM, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_BY", CREATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_BY", UPDATE_BY, ParameterDirection.Input);
            this._dataAccess.ExecuteNonQuery();
        }
        
        public virtual System.Nullable<int> SM_CURRENCY_Delete(System.Nullable<int> CURRENCYID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_CURRENCY_Delete");
            this._dataAccess.AddParameter("CURRENCYID", CURRENCYID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_CURRENCY_Update(System.Nullable<int> CURRENCYID, string CURR_CODE, string CURR_DESCR, System.Nullable<float> EXCH_RATE, System.Nullable<System.DateTime> CURR_VALIDITY_DATE, string CURR_REMARKS, System.Nullable<int> EXPORTED, System.Nullable<System.DateTime> UPDATE_DATE, System.Nullable<int> UPDATE_SITE, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<int> SITEID, string CURR_SYMBOL, System.Nullable<System.DateTime> VALID_FROM, System.Nullable<int> CREATED_BY, System.Nullable<int> UPDATE_BY) {
            this._dataAccess.CreateProcedureCommand("sp_SM_CURRENCY_Update");
            this._dataAccess.AddParameter("CURRENCYID", CURRENCYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_CODE", CURR_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_DESCR", CURR_DESCR, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXCH_RATE", EXCH_RATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_VALIDITY_DATE", CURR_VALIDITY_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_REMARKS", CURR_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORTED", EXPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_SITE", UPDATE_SITE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SITEID", SITEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_SYMBOL", CURR_SYMBOL, ParameterDirection.Input);
            this._dataAccess.AddParameter("VALID_FROM", VALID_FROM, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_BY", CREATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_BY", UPDATE_BY, ParameterDirection.Input);
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
