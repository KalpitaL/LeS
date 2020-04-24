namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class LesInventorylocation : IDisposable {
        
        private DataAccess _dataAccess;
        
        public LesInventorylocation() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet Select_LES_INVENTORYLOCATIONs_By_INVENTORYID(System.Nullable<int> INVENTORYID) {
            this._dataAccess.CreateProcedureCommand("sp_Select_LES_INVENTORYLOCATIONs_By_INVENTORYID");
            this._dataAccess.AddParameter("INVENTORYID", INVENTORYID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet Select_LES_INVENTORYLOCATIONs_By_LOCATIONID(System.Nullable<int> LOCATIONID) {
            this._dataAccess.CreateProcedureCommand("sp_Select_LES_INVENTORYLOCATIONs_By_LOCATIONID");
            this._dataAccess.AddParameter("LOCATIONID", LOCATIONID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_INVENTORYLOCATION_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORYLOCATION_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_INVENTORYLOCATION_Select_One(System.Nullable<int> INVENTORYLOCATIONID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORYLOCATION_Select_One");
            this._dataAccess.AddParameter("INVENTORYLOCATIONID", INVENTORYLOCATIONID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> LES_INVENTORYLOCATION_Insert(System.Nullable<int> LOCATIONID, System.Nullable<int> INVENTORYID, System.Nullable<float> STOCK, System.Nullable<System.DateTime> UPDATE_DATE, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<int> UPDATED_BY, System.Nullable<int> CREATED_BY) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORYLOCATION_Insert");
            this._dataAccess.AddParameter("LOCATIONID", LOCATIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVENTORYID", INVENTORYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("STOCK", STOCK, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATED_BY", UPDATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_BY", CREATED_BY, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_INVENTORYLOCATION_Delete(System.Nullable<int> INVENTORYLOCATIONID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORYLOCATION_Delete");
            this._dataAccess.AddParameter("INVENTORYLOCATIONID", INVENTORYLOCATIONID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_INVENTORYLOCATION_Update(System.Nullable<int> INVENTORYLOCATIONID, System.Nullable<int> LOCATIONID, System.Nullable<int> INVENTORYID, System.Nullable<float> STOCK, System.Nullable<System.DateTime> UPDATE_DATE, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<int> UPDATED_BY, System.Nullable<int> CREATED_BY) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORYLOCATION_Update");
            this._dataAccess.AddParameter("INVENTORYLOCATIONID", INVENTORYLOCATIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("LOCATIONID", LOCATIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVENTORYID", INVENTORYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("STOCK", STOCK, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATED_BY", UPDATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_BY", CREATED_BY, ParameterDirection.Input);
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
