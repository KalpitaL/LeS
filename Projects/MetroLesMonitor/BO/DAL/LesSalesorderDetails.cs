namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class LesSalesorderDetails : IDisposable {
        
        private DataAccess _dataAccess;
        
        public LesSalesorderDetails() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet LES_SALESORDER_DETAILS_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_LES_SALESORDER_DETAILS_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_SALESORDER_DETAILS_Select_One(System.Nullable<int> SALESORDERDETAILID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_SALESORDER_DETAILS_Select_One");
            this._dataAccess.AddParameter("SALESORDERDETAILID", SALESORDERDETAILID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> LES_SALESORDER_DETAILS_Insert(
                    System.Nullable<int> SALESORDERID, 
                    System.Nullable<int> INVENTORYID, 
                    string ITEMNO, 
                    string PARTNAME, 
                    string DRAWINGNO, 
                    string POSNO, 
                    string REFNO, 
                    System.Nullable<float> VENDOR_UNITPRICE, 
                    System.Nullable<float> BUYER_UNITPRICE, 
                    System.Nullable<float> STOCKINHAND, 
                    string MAKERREF, 
                    System.Nullable<float> ORDERED_QTY, 
                    System.Nullable<float> QTY, 
                    System.Nullable<float> DISCOUNT, 
                    System.Nullable<int> BUYERUNITID, 
                    System.Nullable<int> SUPPLIERUNITID, 
                    System.Nullable<int> CONVERSION_FACTOR, 
                    System.Nullable<int> STOCKABLE, 
                    System.Nullable<int> TOSPLIT, 
                    string UDF1, 
                    string UDF2, 
                    string UDF3, 
                    System.Nullable<int> QUOTATIONDETAILID, 
                    System.Nullable<int> DELIVERY_DAYS, 
                    string ITEM_REMARKS, 
                    System.Nullable<float> PROFITMARGIN, 
                    System.Nullable<int> PCK_UNITID, 
                    System.Nullable<float> PACK_QTY, 
                    System.Nullable<System.DateTime> UPDATE_DATE, 
                    System.Nullable<System.DateTime> CREATED_DATE, 
                    System.Nullable<int> UPDATED_BY, 
                    System.Nullable<int> CREATED_BY, 
                    System.Nullable<float> QTD_STOCK, 
                    System.Nullable<int> BUYER_STATUS, 
                    string BUYER_UNITCODE) {
            this._dataAccess.CreateProcedureCommand("sp_LES_SALESORDER_DETAILS_Insert");
            this._dataAccess.AddParameter("SALESORDERID", SALESORDERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVENTORYID", INVENTORYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEMNO", ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTNAME", PARTNAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("DRAWINGNO", DRAWINGNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("POSNO", POSNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("REFNO", REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_UNITPRICE", VENDOR_UNITPRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_UNITPRICE", BUYER_UNITPRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("STOCKINHAND", STOCKINHAND, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAKERREF", MAKERREF, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORDERED_QTY", ORDERED_QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("QTY", QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISCOUNT", DISCOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYERUNITID", BUYERUNITID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERUNITID", SUPPLIERUNITID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONVERSION_FACTOR", CONVERSION_FACTOR, ParameterDirection.Input);
            this._dataAccess.AddParameter("STOCKABLE", STOCKABLE, ParameterDirection.Input);
            this._dataAccess.AddParameter("TOSPLIT", TOSPLIT, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF1", UDF1, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF2", UDF2, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF3", UDF3, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTATIONDETAILID", QUOTATIONDETAILID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERY_DAYS", DELIVERY_DAYS, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_REMARKS", ITEM_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("PROFITMARGIN", PROFITMARGIN, ParameterDirection.Input);
            this._dataAccess.AddParameter("PCK_UNITID", PCK_UNITID, ParameterDirection.Input);
            this._dataAccess.AddParameter("PACK_QTY", PACK_QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATED_BY", UPDATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_BY", CREATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("QTD_STOCK", QTD_STOCK, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_STATUS", BUYER_STATUS, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_UNITCODE", BUYER_UNITCODE, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_SALESORDER_DETAILS_Delete(System.Nullable<int> SALESORDERDETAILID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_SALESORDER_DETAILS_Delete");
            this._dataAccess.AddParameter("SALESORDERDETAILID", SALESORDERDETAILID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_SALESORDER_DETAILS_Update(
                    System.Nullable<int> SALESORDERDETAILID, 
                    System.Nullable<int> SALESORDERID, 
                    System.Nullable<int> INVENTORYID, 
                    string ITEMNO, 
                    string PARTNAME, 
                    string DRAWINGNO, 
                    string POSNO, 
                    string REFNO, 
                    System.Nullable<float> VENDOR_UNITPRICE, 
                    System.Nullable<float> BUYER_UNITPRICE, 
                    System.Nullable<float> STOCKINHAND, 
                    string MAKERREF, 
                    System.Nullable<float> ORDERED_QTY, 
                    System.Nullable<float> QTY, 
                    System.Nullable<float> DISCOUNT, 
                    System.Nullable<int> BUYERUNITID, 
                    System.Nullable<int> SUPPLIERUNITID, 
                    System.Nullable<int> CONVERSION_FACTOR, 
                    System.Nullable<int> STOCKABLE, 
                    System.Nullable<int> TOSPLIT, 
                    string UDF1, 
                    string UDF2, 
                    string UDF3, 
                    System.Nullable<int> QUOTATIONDETAILID, 
                    System.Nullable<int> DELIVERY_DAYS, 
                    string ITEM_REMARKS, 
                    System.Nullable<float> PROFITMARGIN, 
                    System.Nullable<int> PCK_UNITID, 
                    System.Nullable<float> PACK_QTY, 
                    System.Nullable<System.DateTime> UPDATE_DATE, 
                    System.Nullable<System.DateTime> CREATED_DATE, 
                    System.Nullable<int> UPDATED_BY, 
                    System.Nullable<int> CREATED_BY, 
                    System.Nullable<float> QTD_STOCK, 
                    System.Nullable<int> BUYER_STATUS, 
                    string BUYER_UNITCODE) {
            this._dataAccess.CreateProcedureCommand("sp_LES_SALESORDER_DETAILS_Update");
            this._dataAccess.AddParameter("SALESORDERDETAILID", SALESORDERDETAILID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SALESORDERID", SALESORDERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVENTORYID", INVENTORYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEMNO", ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTNAME", PARTNAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("DRAWINGNO", DRAWINGNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("POSNO", POSNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("REFNO", REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_UNITPRICE", VENDOR_UNITPRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_UNITPRICE", BUYER_UNITPRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("STOCKINHAND", STOCKINHAND, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAKERREF", MAKERREF, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORDERED_QTY", ORDERED_QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("QTY", QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISCOUNT", DISCOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYERUNITID", BUYERUNITID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERUNITID", SUPPLIERUNITID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONVERSION_FACTOR", CONVERSION_FACTOR, ParameterDirection.Input);
            this._dataAccess.AddParameter("STOCKABLE", STOCKABLE, ParameterDirection.Input);
            this._dataAccess.AddParameter("TOSPLIT", TOSPLIT, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF1", UDF1, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF2", UDF2, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF3", UDF3, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTATIONDETAILID", QUOTATIONDETAILID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERY_DAYS", DELIVERY_DAYS, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_REMARKS", ITEM_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("PROFITMARGIN", PROFITMARGIN, ParameterDirection.Input);
            this._dataAccess.AddParameter("PCK_UNITID", PCK_UNITID, ParameterDirection.Input);
            this._dataAccess.AddParameter("PACK_QTY", PACK_QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATED_BY", UPDATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_BY", CREATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("QTD_STOCK", QTD_STOCK, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_STATUS", BUYER_STATUS, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_UNITCODE", BUYER_UNITCODE, ParameterDirection.Input);
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
