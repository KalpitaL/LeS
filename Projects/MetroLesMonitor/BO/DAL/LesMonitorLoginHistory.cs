namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class LesMonitorLoginHistory : IDisposable {
        
        private DataAccess _dataAccess;
        
        public LesMonitorLoginHistory() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet LES_MONITOR_LOGIN_HISTORY_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_LES_MONITOR_LOGIN_HISTORY_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_MONITOR_LOGIN_HISTORY_Select_One(System.Nullable<int> LOGIN_TRACK_ID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_MONITOR_LOGIN_HISTORY_Select_One");
            this._dataAccess.AddParameter("LOGIN_TRACK_ID", LOGIN_TRACK_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> LES_MONITOR_LOGIN_HISTORY_Insert(System.Nullable<int> USERID, string SESSIONID, string CLIENT_SERVER_IP, System.Nullable<System.DateTime> LOGGED_IN, string LOGGED_IN_REMARKS, System.Nullable<System.DateTime> LOGGED_OUT, string LOGGED_OUT_REMARKS, System.Nullable<System.DateTime> UPDATE_DATE) {
            this._dataAccess.CreateProcedureCommand("sp_LES_MONITOR_LOGIN_HISTORY_Insert");
            this._dataAccess.AddParameter("USERID", USERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SESSIONID", SESSIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CLIENT_SERVER_IP", CLIENT_SERVER_IP, ParameterDirection.Input);
            this._dataAccess.AddParameter("LOGGED_IN", LOGGED_IN, ParameterDirection.Input);
            this._dataAccess.AddParameter("LOGGED_IN_REMARKS", LOGGED_IN_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("LOGGED_OUT", LOGGED_OUT, ParameterDirection.Input);
            this._dataAccess.AddParameter("LOGGED_OUT_REMARKS", LOGGED_OUT_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_MONITOR_LOGIN_HISTORY_Delete(System.Nullable<int> LOGIN_TRACK_ID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_MONITOR_LOGIN_HISTORY_Delete");
            this._dataAccess.AddParameter("LOGIN_TRACK_ID", LOGIN_TRACK_ID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_MONITOR_LOGIN_HISTORY_Update(System.Nullable<int> LOGIN_TRACK_ID, System.Nullable<int> USERID, string SESSIONID, string CLIENT_SERVER_IP, System.Nullable<System.DateTime> LOGGED_IN, string LOGGED_IN_REMARKS, System.Nullable<System.DateTime> LOGGED_OUT, string LOGGED_OUT_REMARKS, System.Nullable<System.DateTime> UPDATE_DATE) {
            this._dataAccess.CreateProcedureCommand("sp_LES_MONITOR_LOGIN_HISTORY_Update");
            this._dataAccess.AddParameter("LOGIN_TRACK_ID", LOGIN_TRACK_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("USERID", USERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SESSIONID", SESSIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CLIENT_SERVER_IP", CLIENT_SERVER_IP, ParameterDirection.Input);
            this._dataAccess.AddParameter("LOGGED_IN", LOGGED_IN, ParameterDirection.Input);
            this._dataAccess.AddParameter("LOGGED_IN_REMARKS", LOGGED_IN_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("LOGGED_OUT", LOGGED_OUT, ParameterDirection.Input);
            this._dataAccess.AddParameter("LOGGED_OUT_REMARKS", LOGGED_OUT_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual void Dispose() {
            if ((this._dataAccess != null)) {
                this._dataAccess.Dispose();
            }
        }

        public virtual System.Data.DataSet LES_MONITOR_LOGIN_HISTORY_Select_One(string SESSIONID)
        {
            this._dataAccess.CreateProcedureCommand("sp_LES_MONITOR_LOGIN_HISTORY_Select_One_BySessionID");
            this._dataAccess.AddParameter("SESSIONID", SESSIONID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet LES_MONITOR_LOGIN_HISTORY_Select_All_ByDate(System.Nullable<System.DateTime> FROM_DATE, System.Nullable<System.DateTime> TO_DATE)
        {
            this._dataAccess.CreateProcedureCommand("sp_LES_MONITOR_LOGIN_HISTORY_Select_All_By_Date");
            this._dataAccess.AddParameter("FROM_DATE", FROM_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("TO_DATE", TO_DATE, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
    }
}
