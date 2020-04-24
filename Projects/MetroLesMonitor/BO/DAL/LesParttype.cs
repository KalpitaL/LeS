namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class LesParttype : IDisposable {
        
        private DataAccess _dataAccess;
        
        public LesParttype() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet Select_LES_PARTTYPEs_By_CATEGORYID(System.Nullable<int> CATEGORYID) {
            this._dataAccess.CreateProcedureCommand("sp_Select_LES_PARTTYPEs_By_CATEGORYID");
            this._dataAccess.AddParameter("CATEGORYID", CATEGORYID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_PARTTYPE_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_LES_PARTTYPE_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_PARTTYPE_Select_One(System.Nullable<int> PARTTYPEID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_PARTTYPE_Select_One");
            this._dataAccess.AddParameter("PARTTYPEID", PARTTYPEID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> LES_PARTTYPE_Insert(string PARTTYPE_CODE, string DESCRIPTION, System.Nullable<int> CATEGORYID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_PARTTYPE_Insert");
            this._dataAccess.AddParameter("PARTTYPE_CODE", PARTTYPE_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DESCRIPTION", DESCRIPTION, ParameterDirection.Input);
            this._dataAccess.AddParameter("CATEGORYID", CATEGORYID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_PARTTYPE_Delete(System.Nullable<int> PARTTYPEID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_PARTTYPE_Delete");
            this._dataAccess.AddParameter("PARTTYPEID", PARTTYPEID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_PARTTYPE_Update(System.Nullable<int> PARTTYPEID, string PARTTYPE_CODE, string DESCRIPTION, System.Nullable<int> CATEGORYID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_PARTTYPE_Update");
            this._dataAccess.AddParameter("PARTTYPEID", PARTTYPEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTTYPE_CODE", PARTTYPE_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DESCRIPTION", DESCRIPTION, ParameterDirection.Input);
            this._dataAccess.AddParameter("CATEGORYID", CATEGORYID, ParameterDirection.Input);
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
