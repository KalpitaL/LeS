namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmQuotationdetailBuyer : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmQuotationdetailBuyer() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_QUOTATIONDETAIL_BUYER_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONDETAIL_BUYER_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_QUOTATIONDETAIL_BUYER_Select_One(System.Nullable<int> QUOTATIONDETAILID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONDETAIL_BUYER_Select_One");
            this._dataAccess.AddParameter("QUOTATIONDETAILID", QUOTATIONDETAILID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_QUOTATIONDETAIL_BUYER_Insert(
                    System.Nullable<int> QUOTATIONID, 
                    System.Nullable<int> ITEMSTATUS, 
                    System.Nullable<int> ITEMNO, 
                    string VENDOR_ITEMNO, 
                    System.Nullable<float> QTY_REQ, 
                    System.Nullable<float> QTY_QUOTED, 
                    System.Nullable<float> QTY_ORD, 
                    System.Nullable<float> QUOTED_PRICE, 
                    System.Nullable<float> QUOTE_EXCHRATE, 
                    System.Nullable<float> DISCOUNT, 
                    System.Nullable<int> DELIVERYTIME, 
                    string PARTNAME, 
                    string DRAWINGNO, 
                    string POSNO, 
                    string REFNO, 
                    string UNIT_CODE, 
                    string EQUIP_NAME, 
                    string EQUIP_MAKER, 
                    string EQUIP_TYPE, 
                    string EQUIP_SERNO, 
                    string EQUIP_REMARKS, 
                    string QUOTEITEM_REMARK, 
                    string ITEM_REMARK, 
                    System.Nullable<System.DateTime> UPDATE_DATE, 
                    System.Nullable<System.DateTime> CREATED_DATE, 
                    System.Nullable<int> CHANGED_BY_VENDOR, 
                    System.Nullable<int> SITEID, 
                    string DOCITEMID, 
                    string QUOTE_FILE, 
                    string VENDOR_REFNO, 
                    string ORIGINATINGSYSTEMREF, 
                    string ITEM_MARKED_REMARK, 
                    System.Nullable<int> SYS_ITEMNO, 
                    string BUYER_UNIT_CODE, 
                    string ITEM_TYPE, 
                    System.Nullable<int> LINK_RECORD_ID, 
                    string UDF1, 
                    string UDF2, 
                    string UDF3, 
                    string SupplierORGRef, 
                    System.Nullable<int> SALESORDERID, 
                    System.Nullable<int> SALESORDERITEMID, 
                    string VENDOR_ITEMNAME, 
                    string BuyerORGRef) {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONDETAIL_BUYER_Insert");
            this._dataAccess.AddParameter("QUOTATIONID", QUOTATIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEMSTATUS", ITEMSTATUS, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEMNO", ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_ITEMNO", VENDOR_ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("QTY_REQ", QTY_REQ, ParameterDirection.Input);
            this._dataAccess.AddParameter("QTY_QUOTED", QTY_QUOTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("QTY_ORD", QTY_ORD, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTED_PRICE", QUOTED_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_EXCHRATE", QUOTE_EXCHRATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISCOUNT", DISCOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERYTIME", DELIVERYTIME, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTNAME", PARTNAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("DRAWINGNO", DRAWINGNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("POSNO", POSNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("REFNO", REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("UNIT_CODE", UNIT_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_NAME", EQUIP_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_MAKER", EQUIP_MAKER, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_TYPE", EQUIP_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_SERNO", EQUIP_SERNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_REMARKS", EQUIP_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTEITEM_REMARK", QUOTEITEM_REMARK, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_REMARK", ITEM_REMARK, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CHANGED_BY_VENDOR", CHANGED_BY_VENDOR, ParameterDirection.Input);
            this._dataAccess.AddParameter("SITEID", SITEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOCITEMID", DOCITEMID, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_FILE", QUOTE_FILE, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_REFNO", VENDOR_REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORIGINATINGSYSTEMREF", ORIGINATINGSYSTEMREF, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_MARKED_REMARK", ITEM_MARKED_REMARK, ParameterDirection.Input);
            this._dataAccess.AddParameter("SYS_ITEMNO", SYS_ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_UNIT_CODE", BUYER_UNIT_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_TYPE", ITEM_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINK_RECORD_ID", LINK_RECORD_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF1", UDF1, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF2", UDF2, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF3", UDF3, ParameterDirection.Input);
            this._dataAccess.AddParameter("SupplierORGRef", SupplierORGRef, ParameterDirection.Input);
            this._dataAccess.AddParameter("SALESORDERID", SALESORDERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SALESORDERITEMID", SALESORDERITEMID, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_ITEMNAME", VENDOR_ITEMNAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("BuyerORGRef", BuyerORGRef, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_QUOTATIONDETAIL_BUYER_Delete(System.Nullable<int> QUOTATIONDETAILID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONDETAIL_BUYER_Delete");
            this._dataAccess.AddParameter("QUOTATIONDETAILID", QUOTATIONDETAILID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_QUOTATIONDETAIL_BUYER_Update(
                    System.Nullable<int> QUOTATIONDETAILID, 
                    System.Nullable<int> QUOTATIONID, 
                    System.Nullable<int> ITEMSTATUS, 
                    System.Nullable<int> ITEMNO, 
                    string VENDOR_ITEMNO, 
                    System.Nullable<float> QTY_REQ, 
                    System.Nullable<float> QTY_QUOTED, 
                    System.Nullable<float> QTY_ORD, 
                    System.Nullable<float> QUOTED_PRICE, 
                    System.Nullable<float> QUOTE_EXCHRATE, 
                    System.Nullable<float> DISCOUNT, 
                    System.Nullable<int> DELIVERYTIME, 
                    string PARTNAME, 
                    string DRAWINGNO, 
                    string POSNO, 
                    string REFNO, 
                    string UNIT_CODE, 
                    string EQUIP_NAME, 
                    string EQUIP_MAKER, 
                    string EQUIP_TYPE, 
                    string EQUIP_SERNO, 
                    string EQUIP_REMARKS, 
                    string QUOTEITEM_REMARK, 
                    string ITEM_REMARK, 
                    System.Nullable<System.DateTime> UPDATE_DATE, 
                    System.Nullable<System.DateTime> CREATED_DATE, 
                    System.Nullable<int> CHANGED_BY_VENDOR, 
                    System.Nullable<int> SITEID, 
                    string DOCITEMID, 
                    string QUOTE_FILE, 
                    string VENDOR_REFNO, 
                    string ORIGINATINGSYSTEMREF, 
                    string ITEM_MARKED_REMARK, 
                    System.Nullable<int> SYS_ITEMNO, 
                    string BUYER_UNIT_CODE, 
                    string ITEM_TYPE, 
                    System.Nullable<int> LINK_RECORD_ID, 
                    string UDF1, 
                    string UDF2, 
                    string UDF3, 
                    string SupplierORGRef, 
                    System.Nullable<int> SALESORDERID, 
                    System.Nullable<int> SALESORDERITEMID, 
                    string VENDOR_ITEMNAME, 
                    string BuyerORGRef) {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONDETAIL_BUYER_Update");
            this._dataAccess.AddParameter("QUOTATIONDETAILID", QUOTATIONDETAILID, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTATIONID", QUOTATIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEMSTATUS", ITEMSTATUS, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEMNO", ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_ITEMNO", VENDOR_ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("QTY_REQ", QTY_REQ, ParameterDirection.Input);
            this._dataAccess.AddParameter("QTY_QUOTED", QTY_QUOTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("QTY_ORD", QTY_ORD, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTED_PRICE", QUOTED_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_EXCHRATE", QUOTE_EXCHRATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISCOUNT", DISCOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERYTIME", DELIVERYTIME, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTNAME", PARTNAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("DRAWINGNO", DRAWINGNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("POSNO", POSNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("REFNO", REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("UNIT_CODE", UNIT_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_NAME", EQUIP_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_MAKER", EQUIP_MAKER, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_TYPE", EQUIP_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_SERNO", EQUIP_SERNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_REMARKS", EQUIP_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTEITEM_REMARK", QUOTEITEM_REMARK, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_REMARK", ITEM_REMARK, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CHANGED_BY_VENDOR", CHANGED_BY_VENDOR, ParameterDirection.Input);
            this._dataAccess.AddParameter("SITEID", SITEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOCITEMID", DOCITEMID, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_FILE", QUOTE_FILE, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_REFNO", VENDOR_REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORIGINATINGSYSTEMREF", ORIGINATINGSYSTEMREF, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_MARKED_REMARK", ITEM_MARKED_REMARK, ParameterDirection.Input);
            this._dataAccess.AddParameter("SYS_ITEMNO", SYS_ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_UNIT_CODE", BUYER_UNIT_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_TYPE", ITEM_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINK_RECORD_ID", LINK_RECORD_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF1", UDF1, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF2", UDF2, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF3", UDF3, ParameterDirection.Input);
            this._dataAccess.AddParameter("SupplierORGRef", SupplierORGRef, ParameterDirection.Input);
            this._dataAccess.AddParameter("SALESORDERID", SALESORDERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SALESORDERITEMID", SALESORDERITEMID, ParameterDirection.Input);
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
