namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class MtmlDocItem : IDisposable {
        
        private DataAccess _dataAccess;
        
        public MtmlDocItem() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet MTML_DOC_ITEM_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_ITEM_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet MTML_DOC_ITEM_Select_One(System.Nullable<System.Guid> MTMLDOCITEMID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_ITEM_Select_One");
            this._dataAccess.AddParameter("MTMLDOCITEMID", MTMLDOCITEMID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<System.Guid> MTML_DOC_ITEM_Insert(
                    System.Nullable<System.Guid> MTMLDOCID, 
                    string ITEM_DESCRIPTION, 
                    string PARTNUMBER, 
                    string PARTUNIT, 
                    System.Nullable<int> LINEITEMNUMBER, 
                    string PRIORITY, 
                    string QUANTITY, 
                    string PARTCODE, 
                    string COMMENTS, 
                    System.Nullable<float> UNITPRICE, 
                    System.Nullable<float> LISTPRICE, 
                    System.Nullable<float> DISCOUNTEDAMOUNT, 
                    string ORIGINATINGSYSTEMREF, 
                    System.Nullable<int> SYS_ITEMNO, 
                    string SUPPLIER_REFNO, 
                    string SUPPLIER_REMARKS, 
                    System.Nullable<int> DeliveryTime, 
                    string VENDOR_ITEMNO, 
                    string ItemType, 
                    string UDF1, 
                    string UDF2, 
                    string UDF3, 
                    string SupplierORGRef, 
                    string VENDOR_ITEMNAME, 
                    string BuyerORGRef) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_ITEM_Insert");
            this._dataAccess.AddParameter("MTMLDOCID", MTMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_DESCRIPTION", ITEM_DESCRIPTION, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTNUMBER", PARTNUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTUNIT", PARTUNIT, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINEITEMNUMBER", LINEITEMNUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("PRIORITY", PRIORITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUANTITY", QUANTITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTCODE", PARTCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("COMMENTS", COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("UNITPRICE", UNITPRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("LISTPRICE", LISTPRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISCOUNTEDAMOUNT", DISCOUNTEDAMOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORIGINATINGSYSTEMREF", ORIGINATINGSYSTEMREF, ParameterDirection.Input);
            this._dataAccess.AddParameter("SYS_ITEMNO", SYS_ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_REFNO", SUPPLIER_REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_REMARKS", SUPPLIER_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("DeliveryTime", DeliveryTime, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_ITEMNO", VENDOR_ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("ItemType", ItemType, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF1", UDF1, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF2", UDF2, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF3", UDF3, ParameterDirection.Input);
            this._dataAccess.AddParameter("SupplierORGRef", SupplierORGRef, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_ITEMNAME", VENDOR_ITEMNAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("BuyerORGRef", BuyerORGRef, ParameterDirection.Input);
            Guid value = ((Guid)(this._dataAccess.ExecuteScalar()));
            return value;
        }
        
        public virtual System.Nullable<int> MTML_DOC_ITEM_Delete(System.Nullable<System.Guid> MTMLDOCITEMID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_ITEM_Delete");
            this._dataAccess.AddParameter("MTMLDOCITEMID", MTMLDOCITEMID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> MTML_DOC_ITEM_Update(
                    System.Nullable<System.Guid> MTMLDOCITEMID, 
                    System.Nullable<System.Guid> MTMLDOCID, 
                    string ITEM_DESCRIPTION, 
                    string PARTNUMBER, 
                    string PARTUNIT, 
                    System.Nullable<int> LINEITEMNUMBER, 
                    string PRIORITY, 
                    string QUANTITY, 
                    string PARTCODE, 
                    string COMMENTS, 
                    System.Nullable<float> UNITPRICE, 
                    System.Nullable<float> LISTPRICE, 
                    System.Nullable<float> DISCOUNTEDAMOUNT, 
                    string ORIGINATINGSYSTEMREF, 
                    System.Nullable<int> SYS_ITEMNO, 
                    string SUPPLIER_REFNO, 
                    string SUPPLIER_REMARKS, 
                    System.Nullable<int> DeliveryTime, 
                    System.Nullable<int> AUTOID, 
                    string VENDOR_ITEMNO, 
                    string ItemType, 
                    string UDF1, 
                    string UDF2, 
                    string UDF3, 
                    string SupplierORGRef, 
                    string VENDOR_ITEMNAME, 
                    string BuyerORGRef) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_ITEM_Update");
            this._dataAccess.AddParameter("MTMLDOCITEMID", MTMLDOCITEMID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MTMLDOCID", MTMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_DESCRIPTION", ITEM_DESCRIPTION, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTNUMBER", PARTNUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTUNIT", PARTUNIT, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINEITEMNUMBER", LINEITEMNUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("PRIORITY", PRIORITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUANTITY", QUANTITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTCODE", PARTCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("COMMENTS", COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("UNITPRICE", UNITPRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("LISTPRICE", LISTPRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISCOUNTEDAMOUNT", DISCOUNTEDAMOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORIGINATINGSYSTEMREF", ORIGINATINGSYSTEMREF, ParameterDirection.Input);
            this._dataAccess.AddParameter("SYS_ITEMNO", SYS_ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_REFNO", SUPPLIER_REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_REMARKS", SUPPLIER_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("DeliveryTime", DeliveryTime, ParameterDirection.Input);
            this._dataAccess.AddParameter("AUTOID", AUTOID, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_ITEMNO", VENDOR_ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("ItemType", ItemType, ParameterDirection.Input);
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
