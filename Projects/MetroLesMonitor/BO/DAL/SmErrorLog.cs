namespace MetroLesMonitor.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmErrorLog : IDisposable {
        
        private DataAccess _dataAccess;

        public SmErrorLog()
        {
            this._dataAccess = new DataAccess("AuditConnection");
        }
        
        public virtual System.Data.DataSet SM_ERROR_LOG_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_ERROR_LOG_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_ERROR_LOG_Select_One(System.Nullable<int> ERROR_LOGID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_ERROR_LOG_Select_One");
            this._dataAccess.AddParameter("ERROR_LOGID", ERROR_LOGID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_ERROR_LOG_Select_By_LOGID(System.Nullable<int> LOGID)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SM_ERROR_LOG WHERE LOGID=" + LOGID);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual void SM_ERROR_LOG_Insert(System.Nullable<int> ERROR_LOGID, System.Nullable<int> LOGID, string ERROR_PROBLEM, string ERROR_SOLUTION, System.Nullable<int> ERROR_STATUS)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_ERROR_LOG_Insert");
            this._dataAccess.AddParameter("ERROR_LOGID", ERROR_LOGID, ParameterDirection.Input);
            this._dataAccess.AddParameter("LOGID", LOGID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ERROR_PROBLEM", ERROR_PROBLEM, ParameterDirection.Input);
            this._dataAccess.AddParameter("ERROR_SOLUTION", ERROR_SOLUTION, ParameterDirection.Input);
            this._dataAccess.AddParameter("ERROR_STATUS", ERROR_STATUS, ParameterDirection.Input);
            this._dataAccess.ExecuteNonQuery();
        }
        
        public virtual System.Nullable<int> SM_ERROR_LOG_Delete(System.Nullable<int> ERROR_LOGID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_ERROR_LOG_Delete");
            this._dataAccess.AddParameter("ERROR_LOGID", ERROR_LOGID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_ERROR_LOG_Update(System.Nullable<int> ERROR_LOGID, string ERROR_PROBLEM, string ERROR_SOLUTION, System.Nullable<int> ERROR_STATUS)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_ERROR_LOG_Update");
            this._dataAccess.AddParameter("ERROR_LOGID", ERROR_LOGID, ParameterDirection.Input);
            //this._dataAccess.AddParameter("LOGID", LOGID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ERROR_PROBLEM", ERROR_PROBLEM, ParameterDirection.Input);
            this._dataAccess.AddParameter("ERROR_SOLUTION", ERROR_SOLUTION, ParameterDirection.Input);
            this._dataAccess.AddParameter("ERROR_STATUS", ERROR_STATUS, ParameterDirection.Input);
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
