namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmXlsSections : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmXlsSections() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_XLS_SECTIONS_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_SECTIONS_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_XLS_SECTIONS_Select_One(System.Nullable<int> SECTIONID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_SECTIONS_Select_One");
            this._dataAccess.AddParameter("SECTIONID", SECTIONID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_XLS_SECTIONS_Insert(string SECTION_CODE, string SECTION_DESC) {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_SECTIONS_Insert");
            this._dataAccess.AddParameter("SECTION_CODE", SECTION_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SECTION_DESC", SECTION_DESC, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_XLS_SECTIONS_Delete(System.Nullable<int> SECTIONID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_SECTIONS_Delete");
            this._dataAccess.AddParameter("SECTIONID", SECTIONID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_XLS_SECTIONS_Update(System.Nullable<int> SECTIONID, string SECTION_CODE, string SECTION_DESC) {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_SECTIONS_Update");
            this._dataAccess.AddParameter("SECTIONID", SECTIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SECTION_CODE", SECTION_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SECTION_DESC", SECTION_DESC, ParameterDirection.Input);
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
