using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Xml;
using MetroLesMonitor.Dal;

namespace MetroLesMonitor.Dal {

    public partial class SmErrorDetail : IDisposable {

        public DataAccess _dataAccess;

        public SmErrorDetail()
        {
            this._dataAccess = new DataAccess("AuditConnection");
        }
        
        public virtual System.Data.DataSet SM_ERROR_DETAIL_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_ERROR_DETAIL_Select_All_LesMonitor");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_ERROR_DETAIL_Select_One(System.Nullable<int> ERROR_ID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_ERROR_DETAIL_Select_One_LesMonitor");
            this._dataAccess.AddParameter("ERROR_ID", ERROR_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_ERROR_DETAIL_Select_By_ERROR_NO(string ERROR_NO)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_ERROR_DETAIL_Select_By_ERROR_NO_LesMonitor");
            this._dataAccess.AddParameter("ERROR_NO", ERROR_NO, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_ERROR_DETAIL_Insert(string ERROR_NO, string ERROR_DESC, string ERROR_PROBLEM, string ERROR_SOLUTION, string ERROR_TEMPLATE, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<System.DateTime> UPDATED_DATE) {
            this._dataAccess.CreateProcedureCommand("sp_SM_ERROR_DETAIL_Insert_LesMonitor");
            this._dataAccess.AddParameter("ERROR_NO", ERROR_NO, ParameterDirection.Input);
            this._dataAccess.AddParameter("ERROR_DESC", ERROR_DESC, ParameterDirection.Input);
            this._dataAccess.AddParameter("ERROR_PROBLEM", ERROR_PROBLEM, ParameterDirection.Input);
            this._dataAccess.AddParameter("ERROR_SOLUTION", ERROR_SOLUTION, ParameterDirection.Input);
            this._dataAccess.AddParameter("ERROR_TEMPLATE", ERROR_TEMPLATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATED_DATE", UPDATED_DATE, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_ERROR_DETAIL_Delete(System.Nullable<int> ERROR_ID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_ERROR_DETAIL_Delete_LesMonitor");
            this._dataAccess.AddParameter("ERROR_ID", ERROR_ID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_ERROR_DETAIL_Update(System.Nullable<int> ERROR_ID, string ERROR_NO, string ERROR_DESC, string ERROR_PROBLEM, string ERROR_SOLUTION, string ERROR_TEMPLATE, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<System.DateTime> UPDATED_DATE) {
            this._dataAccess.CreateProcedureCommand("sp_SM_ERROR_DETAIL_Update_LesMonitor");
            this._dataAccess.AddParameter("ERROR_ID", ERROR_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ERROR_NO", ERROR_NO, ParameterDirection.Input);
            this._dataAccess.AddParameter("ERROR_DESC", ERROR_DESC, ParameterDirection.Input);
            this._dataAccess.AddParameter("ERROR_PROBLEM", ERROR_PROBLEM, ParameterDirection.Input);
            this._dataAccess.AddParameter("ERROR_SOLUTION", ERROR_SOLUTION, ParameterDirection.Input);
            this._dataAccess.AddParameter("ERROR_TEMPLATE", ERROR_TEMPLATE, ParameterDirection.Input);
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

        public virtual System.Data.DataSet Get_Error_Details()
        {
            int val = 1;
            this._dataAccess.CreateSQLCommand("SELECT * FROM SM_ERROR_DETAIL Where 1 = @VALUE");
            this._dataAccess.AddParameter("VALUE", val, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
    }
}
