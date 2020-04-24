using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Xml;
        
namespace MetroLesMonitor.Dal {

    public partial class SmServerSync : IDisposable {

        public DataAccess _dataAccess;
        
        public SmServerSync() {
            this._dataAccess = new DataAccess();
        }

        public SmServerSync(DataAccess _dataAccess)
        {
            this._dataAccess = (_dataAccess != null) ? _dataAccess : new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_SERVER_SYNC_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_SERVER_SYNC_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_SERVER_SYNC_Select_One(System.Nullable<int> SERVERID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_SERVER_SYNC_Select_One");
            this._dataAccess.AddParameter("SERVERID", SERVERID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_SERVER_SYNC_Insert(string SERVER_NAME, string SERVICE_URL, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<System.DateTime> UPDATED_DATE) {
            this._dataAccess.CreateProcedureCommand("sp_SM_SERVER_SYNC_Insert");
            this._dataAccess.AddParameter("SERVER_NAME", SERVER_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("SERVICE_URL", SERVICE_URL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATED_DATE", UPDATED_DATE, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_SERVER_SYNC_Delete(System.Nullable<int> SERVERID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_SERVER_SYNC_Delete");
            this._dataAccess.AddParameter("SERVERID", SERVERID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_SERVER_SYNC_Update(System.Nullable<int> SERVERID, string SERVER_NAME, string SERVICE_URL, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<System.DateTime> UPDATED_DATE) {
            this._dataAccess.CreateProcedureCommand("sp_SM_SERVER_SYNC_Update");
            this._dataAccess.AddParameter("SERVERID", SERVERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SERVER_NAME", SERVER_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("SERVICE_URL", SERVICE_URL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATED_DATE", UPDATED_DATE, ParameterDirection.Input);
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
