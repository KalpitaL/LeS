namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class LesModuleAccess : IDisposable {
        
        private DataAccess _dataAccess;
        
        public LesModuleAccess() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet Select_LES_MODULE_ACCESSs_By_MODULEACTIONID(System.Nullable<int> MODULEACTIONID) {
            this._dataAccess.CreateProcedureCommand("sp_Select_LES_MODULE_ACCESSs_By_MODULEACTIONID");
            this._dataAccess.AddParameter("MODULEACTIONID", MODULEACTIONID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_MODULE_ACCESS_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_LES_MODULE_ACCESS_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_MODULE_ACCESS_Select_One(System.Nullable<int> MODULEACCESSID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_MODULE_ACCESS_Select_One");
            this._dataAccess.AddParameter("MODULEACCESSID", MODULEACCESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> LES_MODULE_ACCESS_Insert(System.Nullable<int> MODULEACTIONID, System.Nullable<int> EXT_USERID, System.Nullable<int> ACCESS_LEVEL, System.Nullable<int> EXPORTED, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<int> CREATED_BY, System.Nullable<int> UPDATE_SITE, System.Nullable<System.DateTime> UPDATE_DATE, System.Nullable<int> UPDATED_BY) {
            this._dataAccess.CreateProcedureCommand("sp_LES_MODULE_ACCESS_Insert");
            this._dataAccess.AddParameter("MODULEACTIONID", MODULEACTIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXT_USERID", EXT_USERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ACCESS_LEVEL", ACCESS_LEVEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORTED", EXPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_BY", CREATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_SITE", UPDATE_SITE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATED_BY", UPDATED_BY, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_MODULE_ACCESS_Delete(System.Nullable<int> MODULEACCESSID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_MODULE_ACCESS_Delete");
            this._dataAccess.AddParameter("MODULEACCESSID", MODULEACCESSID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_MODULE_ACCESS_Update(System.Nullable<int> MODULEACCESSID, System.Nullable<int> MODULEACTIONID, System.Nullable<int> EXT_USERID, System.Nullable<int> ACCESS_LEVEL, System.Nullable<int> EXPORTED, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<int> CREATED_BY, System.Nullable<int> UPDATE_SITE, System.Nullable<System.DateTime> UPDATE_DATE, System.Nullable<int> UPDATED_BY) {
            this._dataAccess.CreateProcedureCommand("sp_LES_MODULE_ACCESS_Update");
            this._dataAccess.AddParameter("MODULEACCESSID", MODULEACCESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MODULEACTIONID", MODULEACTIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXT_USERID", EXT_USERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ACCESS_LEVEL", ACCESS_LEVEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORTED", EXPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_BY", CREATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_SITE", UPDATE_SITE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATED_BY", UPDATED_BY, ParameterDirection.Input);
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
