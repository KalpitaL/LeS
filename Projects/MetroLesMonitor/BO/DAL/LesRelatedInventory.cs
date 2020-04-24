namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class LesRelatedInventory : IDisposable {
        
        private DataAccess _dataAccess;
        
        public LesRelatedInventory() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet LES_RELATED_INVENTORY_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_LES_RELATED_INVENTORY_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_RELATED_INVENTORY_Select_One(System.Nullable<int> RELATEDITEMID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_RELATED_INVENTORY_Select_One");
            this._dataAccess.AddParameter("RELATEDITEMID", RELATEDITEMID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> LES_RELATED_INVENTORY_Insert(System.Nullable<int> INVENTORYID, System.Nullable<int> RELATED_INVENTORYID, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<int> CREATED_BY) {
            this._dataAccess.CreateProcedureCommand("sp_LES_RELATED_INVENTORY_Insert");
            this._dataAccess.AddParameter("INVENTORYID", INVENTORYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RELATED_INVENTORYID", RELATED_INVENTORYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_BY", CREATED_BY, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_RELATED_INVENTORY_Delete(System.Nullable<int> RELATEDITEMID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_RELATED_INVENTORY_Delete");
            this._dataAccess.AddParameter("RELATEDITEMID", RELATEDITEMID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_RELATED_INVENTORY_Update(System.Nullable<int> RELATEDITEMID, System.Nullable<int> INVENTORYID, System.Nullable<int> RELATED_INVENTORYID, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<int> CREATED_BY) {
            this._dataAccess.CreateProcedureCommand("sp_LES_RELATED_INVENTORY_Update");
            this._dataAccess.AddParameter("RELATEDITEMID", RELATEDITEMID, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVENTORYID", INVENTORYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RELATED_INVENTORYID", RELATED_INVENTORYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
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
