namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmCurrencyMapped : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmCurrencyMapped() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_CURRENCY_MAPPED_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_CURRENCY_MAPPED_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_CURRENCY_MAPPED_Select_One(System.Nullable<int> AUTOID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_CURRENCY_MAPPED_Select_One");
            this._dataAccess.AddParameter("AUTOID", AUTOID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual void SM_CURRENCY_MAPPED_Insert(System.Nullable<int> AUTOID, string SOURCE_CURR_CODE, string TARGET_CURR_CODE, System.Nullable<int> MAPID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_CURRENCY_MAPPED_Insert");
            this._dataAccess.AddParameter("AUTOID", AUTOID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SOURCE_CURR_CODE", SOURCE_CURR_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("TARGET_CURR_CODE", TARGET_CURR_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAPID", MAPID, ParameterDirection.Input);
            this._dataAccess.ExecuteNonQuery();
        }
        
        public virtual System.Nullable<int> SM_CURRENCY_MAPPED_Delete(System.Nullable<int> AUTOID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_CURRENCY_MAPPED_Delete");
            this._dataAccess.AddParameter("AUTOID", AUTOID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_CURRENCY_MAPPED_Update(System.Nullable<int> AUTOID, string SOURCE_CURR_CODE, string TARGET_CURR_CODE, System.Nullable<int> MAPID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_CURRENCY_MAPPED_Update");
            this._dataAccess.AddParameter("AUTOID", AUTOID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SOURCE_CURR_CODE", SOURCE_CURR_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("TARGET_CURR_CODE", TARGET_CURR_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAPID", MAPID, ParameterDirection.Input);
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
