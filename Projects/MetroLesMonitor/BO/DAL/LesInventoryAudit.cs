namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class LesInventoryAudit : IDisposable {
        
        private DataAccess _dataAccess;
        
        public LesInventoryAudit() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet LES_INVENTORY_AUDIT_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_AUDIT_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_INVENTORY_AUDIT_Select_One(System.Nullable<int> INVAUDITID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_AUDIT_Select_One");
            this._dataAccess.AddParameter("INVAUDITID", INVAUDITID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> LES_INVENTORY_AUDIT_Insert(System.Nullable<int> INVENTORYID, System.Nullable<int> USERID, System.Nullable<System.DateTime> AUDIT_TIME, string REMARK) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_AUDIT_Insert");
            this._dataAccess.AddParameter("INVENTORYID", INVENTORYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("USERID", USERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("AUDIT_TIME", AUDIT_TIME, ParameterDirection.Input);
            this._dataAccess.AddParameter("REMARK", REMARK, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_INVENTORY_AUDIT_Delete(System.Nullable<int> INVAUDITID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_AUDIT_Delete");
            this._dataAccess.AddParameter("INVAUDITID", INVAUDITID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_INVENTORY_AUDIT_Update(System.Nullable<int> INVAUDITID, System.Nullable<int> INVENTORYID, System.Nullable<int> USERID, System.Nullable<System.DateTime> AUDIT_TIME, string REMARK) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_AUDIT_Update");
            this._dataAccess.AddParameter("INVAUDITID", INVAUDITID, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVENTORYID", INVENTORYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("USERID", USERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("AUDIT_TIME", AUDIT_TIME, ParameterDirection.Input);
            this._dataAccess.AddParameter("REMARK", REMARK, ParameterDirection.Input);
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
