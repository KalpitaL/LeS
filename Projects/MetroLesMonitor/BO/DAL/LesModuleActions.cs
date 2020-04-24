namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class LesModuleActions : IDisposable {
        
        private DataAccess _dataAccess;
        
        public LesModuleActions() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet LES_MODULE_ACTIONS_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_LES_MODULE_ACTIONS_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_MODULE_ACTIONS_Select_One(System.Nullable<int> MODULEACTIONID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_MODULE_ACTIONS_Select_One");
            this._dataAccess.AddParameter("MODULEACTIONID", MODULEACTIONID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> LES_MODULE_ACTIONS_Insert(string ACTION_NAME, System.Nullable<int> MODULEID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_MODULE_ACTIONS_Insert");
            this._dataAccess.AddParameter("ACTION_NAME", ACTION_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("MODULEID", MODULEID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_MODULE_ACTIONS_Delete(System.Nullable<int> MODULEACTIONID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_MODULE_ACTIONS_Delete");
            this._dataAccess.AddParameter("MODULEACTIONID", MODULEACTIONID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_MODULE_ACTIONS_Update(System.Nullable<int> MODULEACTIONID, string ACTION_NAME, System.Nullable<int> MODULEID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_MODULE_ACTIONS_Update");
            this._dataAccess.AddParameter("MODULEACTIONID", MODULEACTIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ACTION_NAME", ACTION_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("MODULEID", MODULEID, ParameterDirection.Input);
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
