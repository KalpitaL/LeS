namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmBuyerSupplierItemRefTemp : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmBuyerSupplierItemRefTemp() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_ITEM_REF_TEMP_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_ITEM_REF_TEMP_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_BUYER_SUPPLIER_ITEM_REF_TEMP_Insert(System.Nullable<int> REFID, string REFTYPE, string BUYER_REF, string SUPPLIER_REF, string ITEM_DESC, string COMMENTS, System.Nullable<int> BUYER_ID, System.Nullable<int> SUPPLIER_ID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_ITEM_REF_TEMP_Insert");
            this._dataAccess.AddParameter("REFID", REFID, ParameterDirection.Input);
            this._dataAccess.AddParameter("REFTYPE", REFTYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_REF", BUYER_REF, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_REF", SUPPLIER_REF, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_DESC", ITEM_DESC, ParameterDirection.Input);
            this._dataAccess.AddParameter("COMMENTS", COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_ID", BUYER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_ID", SUPPLIER_ID, ParameterDirection.Input);
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
