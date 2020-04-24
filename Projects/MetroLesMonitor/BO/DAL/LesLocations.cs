namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class LesLocations : IDisposable {
        
        private DataAccess _dataAccess;
        
        public LesLocations() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet LES_LOCATIONS_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_LES_LOCATIONS_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_LOCATIONS_Select_One(System.Nullable<int> LOCATIONID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_LOCATIONS_Select_One");
            this._dataAccess.AddParameter("LOCATIONID", LOCATIONID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> LES_LOCATIONS_Insert(string LOCCODE, string LOC_DESCRIPTION) {
            this._dataAccess.CreateProcedureCommand("sp_LES_LOCATIONS_Insert");
            this._dataAccess.AddParameter("LOCCODE", LOCCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("LOC_DESCRIPTION", LOC_DESCRIPTION, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_LOCATIONS_Delete(System.Nullable<int> LOCATIONID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_LOCATIONS_Delete");
            this._dataAccess.AddParameter("LOCATIONID", LOCATIONID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_LOCATIONS_Update(System.Nullable<int> LOCATIONID, string LOCCODE, string LOC_DESCRIPTION) {
            this._dataAccess.CreateProcedureCommand("sp_LES_LOCATIONS_Update");
            this._dataAccess.AddParameter("LOCATIONID", LOCATIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("LOCCODE", LOCCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("LOC_DESCRIPTION", LOC_DESCRIPTION, ParameterDirection.Input);
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
