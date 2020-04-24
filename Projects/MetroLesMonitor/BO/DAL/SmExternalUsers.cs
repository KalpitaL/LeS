namespace MetroLesMonitor.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;    
    
    public partial class SmExternalUsers : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmExternalUsers() {
            this._dataAccess = new DataAccess();
        }

        public SmExternalUsers(DataAccess _dataAccess)
        {
            // TODO: Complete member initialization
            this._dataAccess = _dataAccess;
        }
        
        public virtual System.Data.DataSet SM_EXTERNAL_USERS_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_EXTERNAL_USERS_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_EXTERNAL_USERS_DISTINCT_PWD_Select_All_By_AddressID(System.Nullable<int> ADDRESSID, string SuppEmail)
        {
            //this._dataAccess.CreateSQLCommand("SELECT DISTINCT EX_PASSWORD,UPDATE_DATE FROM SM_EXTERNAL_USERS WHERE ADDRESSID=@ADDRESSID AND EX_EMAILID LIKE '%" + SuppEmail + "%' ORDER BY UPDATE_DATE DESC ");
            this._dataAccess.CreateSQLCommand("SELECT DISTINCT EX_PASSWORD,UPDATE_DATE FROM SM_EXTERNAL_USERS WHERE ADDRESSID=@ADDRESSID  ORDER BY UPDATE_DATE DESC ");
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_EXTERNAL_USERS_Select_One(string EX_USERNAME, string EX_PASSWORD)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_EXTERNAL_USERS_Select_LOGIN");
            this._dataAccess.AddParameter("EX_USERCODE", EX_USERNAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("EX_PASSWORD", EX_PASSWORD, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_EXTERNAL_USERS_Select_One(System.Nullable<int> EX_USERID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_EXTERNAL_USERS_Select_One");
            this._dataAccess.AddParameter("EX_USERID", EX_USERID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_EXTERNAL_USERS_Select_One_By_LINKID(System.Nullable<int> LINKID)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SM_EXTERNAL_USERS WHERE LINKID=@LINKID");
            this._dataAccess.AddParameter("LINKID", LINKID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual int SM_EXTERNAL_USERS_Insert(System.Nullable<int> EX_USERID, System.Nullable<int> ADDRESSID, string EX_USERCODE, string EX_USERNAME, string EX_PASSWORD, string EX_EMAILID, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<System.DateTime> UPDATE_DATE, System.Nullable<int> ISACTIVE, System.Nullable<int> PWD_EXPIRED, System.Nullable<int> ACCESS_LEVEL, System.Nullable<int> LINKID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_EXTERNAL_USERS_Insert");
            this._dataAccess.AddParameter("EX_USERID", EX_USERID, ParameterDirection.Output);
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("EX_USERCODE", EX_USERCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EX_USERNAME", EX_USERNAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("EX_PASSWORD", EX_PASSWORD, ParameterDirection.Input);
            this._dataAccess.AddParameter("EX_EMAILID", EX_EMAILID, ParameterDirection.Input);         
            this._dataAccess.AddParameter("ISACTIVE", ISACTIVE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PWD_EXPIRED", PWD_EXPIRED, ParameterDirection.Input);
            this._dataAccess.AddParameter("ACCESS_LEVEL", ACCESS_LEVEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINKID", LINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.ExecuteNonQuery();

            int USERID = convert.ToInt(this._dataAccess.Command.Parameters["EX_USERID"].Value);
            return USERID;
        }
        
        public virtual System.Nullable<int> SM_EXTERNAL_USERS_Delete(System.Nullable<int> EX_USERID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_EXTERNAL_USERS_Delete");
            this._dataAccess.AddParameter("EX_USERID", EX_USERID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_EXTERNAL_USERS_Update(System.Nullable<int> EX_USERID, System.Nullable<int> ADDRESSID, string EX_USERCODE, string EX_USERNAME, string EX_PASSWORD, string EX_EMAILID, System.Nullable<System.DateTime> UPDATE_DATE, System.Nullable<int> ISACTIVE, System.Nullable<int> PWD_EXPIRED, System.Nullable<int> ACCESS_LEVEL, System.Nullable<int> LINKID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_EXTERNAL_USERS_Update");
            this._dataAccess.AddParameter("EX_USERID", EX_USERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("EX_USERCODE", EX_USERCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EX_USERNAME", EX_USERNAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("EX_PASSWORD", EX_PASSWORD, ParameterDirection.Input);
            this._dataAccess.AddParameter("EX_EMAILID", EX_EMAILID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ISACTIVE", ISACTIVE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PWD_EXPIRED", PWD_EXPIRED, ParameterDirection.Input);
            this._dataAccess.AddParameter("ACCESS_LEVEL", ACCESS_LEVEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINKID", LINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
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
