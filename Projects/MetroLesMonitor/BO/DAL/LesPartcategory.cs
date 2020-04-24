namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class LesPartcategory : IDisposable {
        
        private DataAccess _dataAccess;
        
        public LesPartcategory() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet LES_PARTCATEGORY_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_LES_PARTCATEGORY_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_PARTCATEGORY_Select_One(System.Nullable<int> CATEGORYID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_PARTCATEGORY_Select_One");
            this._dataAccess.AddParameter("CATEGORYID", CATEGORYID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> LES_PARTCATEGORY_Insert(string CATEGORY_CODE, string DESCRIPTION) {
            this._dataAccess.CreateProcedureCommand("sp_LES_PARTCATEGORY_Insert");
            this._dataAccess.AddParameter("CATEGORY_CODE", CATEGORY_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DESCRIPTION", DESCRIPTION, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_PARTCATEGORY_Delete(System.Nullable<int> CATEGORYID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_PARTCATEGORY_Delete");
            this._dataAccess.AddParameter("CATEGORYID", CATEGORYID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_PARTCATEGORY_Update(System.Nullable<int> CATEGORYID, string CATEGORY_CODE, string DESCRIPTION) {
            this._dataAccess.CreateProcedureCommand("sp_LES_PARTCATEGORY_Update");
            this._dataAccess.AddParameter("CATEGORYID", CATEGORYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CATEGORY_CODE", CATEGORY_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DESCRIPTION", DESCRIPTION, ParameterDirection.Input);
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
