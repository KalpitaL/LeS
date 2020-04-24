namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class LesClientConnectLog : IDisposable {
        
        private DataAccess _dataAccess;
        
        public LesClientConnectLog() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet LES_CLIENT_CONNECT_LOG_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_LES_CLIENT_CONNECT_LOG_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }


        public virtual System.Data.DataSet LES_CLIENT_CONNECT_LOG_Select_All(string cmdSQL)
        {
            this._dataAccess.CreateSQLCommand(cmdSQL);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Nullable<int> LES_CLIENT_CONNECT_LOG_Insert(System.Nullable<int> CLIENTID, System.Nullable<System.DateTime> LAST_CONNECT, System.Nullable<System.DateTime> LAST_CONNECT1, string LISENCE_KEY, System.Nullable<float> NOTIFY_CNT) {
            this._dataAccess.CreateProcedureCommand("sp_LES_CLIENT_CONNECT_LOG_Insert");
            this._dataAccess.AddParameter("CLIENTID", CLIENTID, ParameterDirection.Input);
            this._dataAccess.AddParameter("LAST_CONNECT", LAST_CONNECT, ParameterDirection.Input);
            this._dataAccess.AddParameter("LAST_CONNECT1", LAST_CONNECT1, ParameterDirection.Input);
            this._dataAccess.AddParameter("LISENCE_KEY", LISENCE_KEY, ParameterDirection.Input);
            this._dataAccess.AddParameter("NOTIFY_CNT", NOTIFY_CNT, ParameterDirection.Input);
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
