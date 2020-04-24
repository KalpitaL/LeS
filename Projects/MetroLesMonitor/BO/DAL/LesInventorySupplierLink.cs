namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class LesInventorySupplierLink : IDisposable {
        
        private DataAccess _dataAccess;
        
        public LesInventorySupplierLink() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet Select_LES_INVENTORY_SUPPLIER_LINKs_By_INVENTORYID(System.Nullable<int> INVENTORYID) {
            this._dataAccess.CreateProcedureCommand("sp_Select_LES_INVENTORY_SUPPLIER_LINKs_By_INVENTORYID");
            this._dataAccess.AddParameter("INVENTORYID", INVENTORYID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet Select_LES_INVENTORY_SUPPLIER_LINKs_By_SUPPLIERID(System.Nullable<int> SUPPLIERID) {
            this._dataAccess.CreateProcedureCommand("sp_Select_LES_INVENTORY_SUPPLIER_LINKs_By_SUPPLIERID");
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_INVENTORY_SUPPLIER_LINK_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_SUPPLIER_LINK_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_INVENTORY_SUPPLIER_LINK_Select_One(System.Nullable<int> INVSUPID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_SUPPLIER_LINK_Select_One");
            this._dataAccess.AddParameter("INVSUPID", INVSUPID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> LES_INVENTORY_SUPPLIER_LINK_Insert(System.Nullable<int> INVENTORYID, System.Nullable<int> SUPPLIERID, System.Nullable<int> ISPREFERRED, System.Nullable<System.DateTime> UPDATE_DATE, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<int> UPDATED_BY, System.Nullable<int> CREATED_BY) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_SUPPLIER_LINK_Insert");
            this._dataAccess.AddParameter("INVENTORYID", INVENTORYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ISPREFERRED", ISPREFERRED, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATED_BY", UPDATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_BY", CREATED_BY, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_INVENTORY_SUPPLIER_LINK_Delete(System.Nullable<int> INVSUPID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_SUPPLIER_LINK_Delete");
            this._dataAccess.AddParameter("INVSUPID", INVSUPID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_INVENTORY_SUPPLIER_LINK_Update(System.Nullable<int> INVSUPID, System.Nullable<int> INVENTORYID, System.Nullable<int> SUPPLIERID, System.Nullable<int> ISPREFERRED, System.Nullable<System.DateTime> UPDATE_DATE, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<int> UPDATED_BY, System.Nullable<int> CREATED_BY) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_SUPPLIER_LINK_Update");
            this._dataAccess.AddParameter("INVSUPID", INVSUPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVENTORYID", INVENTORYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ISPREFERRED", ISPREFERRED, ParameterDirection.Input);
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
