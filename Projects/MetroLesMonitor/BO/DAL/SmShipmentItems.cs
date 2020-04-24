namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmShipmentItems : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmShipmentItems() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet Select_SM_SHIPMENT_ITEMSs_By_SHIPMENT_ID(System.Nullable<int> SHIPMENT_ID) {
            this._dataAccess.CreateProcedureCommand("sp_Select_SM_SHIPMENT_ITEMSs_By_SHIPMENT_ID");
            this._dataAccess.AddParameter("SHIPMENT_ID", SHIPMENT_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_SHIPMENT_ITEMS_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_SHIPMENT_ITEMS_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_SHIPMENT_ITEMS_Select_One(System.Nullable<int> SHIPMENT_ITEM_ID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_SHIPMENT_ITEMS_Select_One");
            this._dataAccess.AddParameter("SHIPMENT_ITEM_ID", SHIPMENT_ITEM_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_SHIPMENT_ITEMS_Insert(
                    System.Nullable<int> SHIPMENT_ID, 
                    System.Nullable<int> ITEMSTATUS, 
                    System.Nullable<int> ITEMNO, 
                    string VENDOR_ITEMNO, 
                    System.Nullable<float> QTY_ORD, 
                    System.Nullable<float> QTY_SHIPMENT, 
                    System.Nullable<float> QUOTED_PRICE, 
                    System.Nullable<float> QUOTE_EXCHRATE, 
                    System.Nullable<float> DISCOUNT, 
                    System.Nullable<int> DELIVERYTIME, 
                    string PARTNAME, 
                    string DRAWINGNO, 
                    string POSNO, 
                    string REFNO, 
                    string UNIT_CODE, 
                    System.Nullable<int> CHANGED_BY_VENDOR, 
                    string ITEM_REMARK, 
                    System.Nullable<System.DateTime> UPDATE_DATE, 
                    System.Nullable<System.DateTime> CREATED_DATE, 
                    string VENDOR_REFNO, 
                    string ORIGINATINGSYSTEMREF, 
                    System.Nullable<int> SYS_ITEMNO, 
                    string ITEM_TYPE, 
                    string UDF1, 
                    string UDF2, 
                    string UDF3, 
                    string SupplierORGRef, 
                    string VENDOR_ITEMNAME, 
                    string BuyerORGRef) {
            this._dataAccess.CreateProcedureCommand("sp_SM_SHIPMENT_ITEMS_Insert");
            this._dataAccess.AddParameter("SHIPMENT_ID", SHIPMENT_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEMSTATUS", ITEMSTATUS, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEMNO", ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_ITEMNO", VENDOR_ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("QTY_ORD", QTY_ORD, ParameterDirection.Input);
            this._dataAccess.AddParameter("QTY_SHIPMENT", QTY_SHIPMENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTED_PRICE", QUOTED_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_EXCHRATE", QUOTE_EXCHRATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISCOUNT", DISCOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERYTIME", DELIVERYTIME, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTNAME", PARTNAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("DRAWINGNO", DRAWINGNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("POSNO", POSNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("REFNO", REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("UNIT_CODE", UNIT_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CHANGED_BY_VENDOR", CHANGED_BY_VENDOR, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_REMARK", ITEM_REMARK, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_REFNO", VENDOR_REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORIGINATINGSYSTEMREF", ORIGINATINGSYSTEMREF, ParameterDirection.Input);
            this._dataAccess.AddParameter("SYS_ITEMNO", SYS_ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_TYPE", ITEM_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF1", UDF1, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF2", UDF2, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF3", UDF3, ParameterDirection.Input);
            this._dataAccess.AddParameter("SupplierORGRef", SupplierORGRef, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_ITEMNAME", VENDOR_ITEMNAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("BuyerORGRef", BuyerORGRef, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_SHIPMENT_ITEMS_Delete(System.Nullable<int> SHIPMENT_ITEM_ID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_SHIPMENT_ITEMS_Delete");
            this._dataAccess.AddParameter("SHIPMENT_ITEM_ID", SHIPMENT_ITEM_ID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_SHIPMENT_ITEMS_Update(
                    System.Nullable<int> SHIPMENT_ITEM_ID, 
                    System.Nullable<int> SHIPMENT_ID, 
                    System.Nullable<int> ITEMSTATUS, 
                    System.Nullable<int> ITEMNO, 
                    string VENDOR_ITEMNO, 
                    System.Nullable<float> QTY_ORD, 
                    System.Nullable<float> QTY_SHIPMENT, 
                    System.Nullable<float> QUOTED_PRICE, 
                    System.Nullable<float> QUOTE_EXCHRATE, 
                    System.Nullable<float> DISCOUNT, 
                    System.Nullable<int> DELIVERYTIME, 
                    string PARTNAME, 
                    string DRAWINGNO, 
                    string POSNO, 
                    string REFNO, 
                    string UNIT_CODE, 
                    System.Nullable<int> CHANGED_BY_VENDOR, 
                    string ITEM_REMARK, 
                    System.Nullable<System.DateTime> UPDATE_DATE, 
                    System.Nullable<System.DateTime> CREATED_DATE, 
                    string VENDOR_REFNO, 
                    string ORIGINATINGSYSTEMREF, 
                    System.Nullable<int> SYS_ITEMNO, 
                    string ITEM_TYPE, 
                    string UDF1, 
                    string UDF2, 
                    string UDF3, 
                    string SupplierORGRef, 
                    string VENDOR_ITEMNAME, 
                    string BuyerORGRef) {
            this._dataAccess.CreateProcedureCommand("sp_SM_SHIPMENT_ITEMS_Update");
            this._dataAccess.AddParameter("SHIPMENT_ITEM_ID", SHIPMENT_ITEM_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SHIPMENT_ID", SHIPMENT_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEMSTATUS", ITEMSTATUS, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEMNO", ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_ITEMNO", VENDOR_ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("QTY_ORD", QTY_ORD, ParameterDirection.Input);
            this._dataAccess.AddParameter("QTY_SHIPMENT", QTY_SHIPMENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTED_PRICE", QUOTED_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_EXCHRATE", QUOTE_EXCHRATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISCOUNT", DISCOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERYTIME", DELIVERYTIME, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTNAME", PARTNAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("DRAWINGNO", DRAWINGNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("POSNO", POSNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("REFNO", REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("UNIT_CODE", UNIT_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CHANGED_BY_VENDOR", CHANGED_BY_VENDOR, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_REMARK", ITEM_REMARK, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_REFNO", VENDOR_REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORIGINATINGSYSTEMREF", ORIGINATINGSYSTEMREF, ParameterDirection.Input);
            this._dataAccess.AddParameter("SYS_ITEMNO", SYS_ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_TYPE", ITEM_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF1", UDF1, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF2", UDF2, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF3", UDF3, ParameterDirection.Input);
            this._dataAccess.AddParameter("SupplierORGRef", SupplierORGRef, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_ITEMNAME", VENDOR_ITEMNAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("BuyerORGRef", BuyerORGRef, ParameterDirection.Input);
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
