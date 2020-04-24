namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class LesInventoryHistory : IDisposable {
        
        private DataAccess _dataAccess;
        
        public LesInventoryHistory() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet LES_INVENTORY_HISTORY_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_HISTORY_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_INVENTORY_HISTORY_Select_One(System.Nullable<int> INVHISTORYID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_HISTORY_Select_One");
            this._dataAccess.AddParameter("INVHISTORYID", INVHISTORYID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> LES_INVENTORY_HISTORY_Insert(System.Nullable<int> INVENTORYID, System.Nullable<int> CURRENCYID, System.Nullable<int> HISTORY_TYPE, System.Nullable<float> QUANTITY, System.Nullable<float> UNIT_PRICE, System.Nullable<int> SUPPLIERID, System.Nullable<int> SUPUNITID, System.Nullable<int> USED, System.Nullable<System.DateTime> UPDATE_DATE, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<int> UPDATED_BY, System.Nullable<int> CREATED_BY) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_HISTORY_Insert");
            this._dataAccess.AddParameter("INVENTORYID", INVENTORYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURRENCYID", CURRENCYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("HISTORY_TYPE", HISTORY_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUANTITY", QUANTITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("UNIT_PRICE", UNIT_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPUNITID", SUPUNITID, ParameterDirection.Input);
            this._dataAccess.AddParameter("USED", USED, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATED_BY", UPDATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_BY", CREATED_BY, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_INVENTORY_HISTORY_Delete(System.Nullable<int> INVHISTORYID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_HISTORY_Delete");
            this._dataAccess.AddParameter("INVHISTORYID", INVHISTORYID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_INVENTORY_HISTORY_Update(System.Nullable<int> INVHISTORYID, System.Nullable<int> INVENTORYID, System.Nullable<int> CURRENCYID, System.Nullable<int> HISTORY_TYPE, System.Nullable<float> QUANTITY, System.Nullable<float> UNIT_PRICE, System.Nullable<int> SUPPLIERID, System.Nullable<int> SUPUNITID, System.Nullable<int> USED, System.Nullable<System.DateTime> UPDATE_DATE, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<int> UPDATED_BY, System.Nullable<int> CREATED_BY) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_HISTORY_Update");
            this._dataAccess.AddParameter("INVHISTORYID", INVHISTORYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVENTORYID", INVENTORYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURRENCYID", CURRENCYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("HISTORY_TYPE", HISTORY_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUANTITY", QUANTITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("UNIT_PRICE", UNIT_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPUNITID", SUPUNITID, ParameterDirection.Input);
            this._dataAccess.AddParameter("USED", USED, ParameterDirection.Input);
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
