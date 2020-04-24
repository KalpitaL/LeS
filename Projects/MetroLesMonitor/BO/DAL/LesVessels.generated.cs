namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class LesVessels : IDisposable {
        
        private DataAccess _dataAccess;
        
        public LesVessels() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet LES_VESSELS_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_LES_VESSELS_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_VESSELS_Select_One(System.Nullable<int> VESSELID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_VESSELS_Select_One");
            this._dataAccess.AddParameter("VESSELID", VESSELID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> LES_VESSELS_Insert(string VESSEL_NAME, string MARKING_NO, System.Nullable<System.DateTime> ETA, System.Nullable<System.DateTime> ETD, string BERTH, System.Nullable<int> imo_no) {
            this._dataAccess.CreateProcedureCommand("sp_LES_VESSELS_Insert");
            this._dataAccess.AddParameter("VESSEL_NAME", VESSEL_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("MARKING_NO", MARKING_NO, ParameterDirection.Input);
            this._dataAccess.AddParameter("ETA", ETA, ParameterDirection.Input);
            this._dataAccess.AddParameter("ETD", ETD, ParameterDirection.Input);
            this._dataAccess.AddParameter("BERTH", BERTH, ParameterDirection.Input);
            this._dataAccess.AddParameter("imo_no", imo_no, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_VESSELS_Delete(System.Nullable<int> VESSELID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_VESSELS_Delete");
            this._dataAccess.AddParameter("VESSELID", VESSELID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_VESSELS_Update(System.Nullable<int> VESSELID, string VESSEL_NAME, string MARKING_NO, System.Nullable<System.DateTime> ETA, System.Nullable<System.DateTime> ETD, string BERTH, System.Nullable<int> imo_no) {
            this._dataAccess.CreateProcedureCommand("sp_LES_VESSELS_Update");
            this._dataAccess.AddParameter("VESSELID", VESSELID, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_NAME", VESSEL_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("MARKING_NO", MARKING_NO, ParameterDirection.Input);
            this._dataAccess.AddParameter("ETA", ETA, ParameterDirection.Input);
            this._dataAccess.AddParameter("ETD", ETD, ParameterDirection.Input);
            this._dataAccess.AddParameter("BERTH", BERTH, ParameterDirection.Input);
            this._dataAccess.AddParameter("imo_no", imo_no, ParameterDirection.Input);
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
