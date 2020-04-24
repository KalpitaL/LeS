namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmPartunit : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmPartunit() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_PARTUNIT_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_PARTUNIT_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_PARTUNIT_Select_One(System.Nullable<int> PARTUNITID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_PARTUNIT_Select_One");
            this._dataAccess.AddParameter("PARTUNITID", PARTUNITID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual void SM_PARTUNIT_Insert(System.Nullable<int> PARTUNITID, string UNIT_CODE, string UNIT_DESCR, System.Nullable<byte> EXPORTED, System.Nullable<int> UPDATE_SITE, System.Nullable<System.DateTime> UPDATE_DATE, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<int> SITEID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_PARTUNIT_Insert");
            this._dataAccess.AddParameter("PARTUNITID", PARTUNITID, ParameterDirection.Input);
            this._dataAccess.AddParameter("UNIT_CODE", UNIT_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UNIT_DESCR", UNIT_DESCR, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORTED", EXPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_SITE", UPDATE_SITE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SITEID", SITEID, ParameterDirection.Input);
            this._dataAccess.ExecuteNonQuery();
        }
        
        public virtual System.Nullable<int> SM_PARTUNIT_Delete(System.Nullable<int> PARTUNITID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_PARTUNIT_Delete");
            this._dataAccess.AddParameter("PARTUNITID", PARTUNITID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_PARTUNIT_Update(System.Nullable<int> PARTUNITID, string UNIT_CODE, string UNIT_DESCR, System.Nullable<byte> EXPORTED, System.Nullable<int> UPDATE_SITE, System.Nullable<System.DateTime> UPDATE_DATE, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<int> SITEID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_PARTUNIT_Update");
            this._dataAccess.AddParameter("PARTUNITID", PARTUNITID, ParameterDirection.Input);
            this._dataAccess.AddParameter("UNIT_CODE", UNIT_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UNIT_DESCR", UNIT_DESCR, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORTED", EXPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_SITE", UPDATE_SITE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SITEID", SITEID, ParameterDirection.Input);
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
