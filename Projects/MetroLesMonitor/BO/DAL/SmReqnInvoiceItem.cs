namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmReqnInvoiceItem : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmReqnInvoiceItem() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_REQN_INVOICE_ITEM_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_REQN_INVOICE_ITEM_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_REQN_INVOICE_ITEM_Insert(
                    System.Nullable<int> INVOICE_ITEMID, 
                    System.Nullable<int> REQNINVOICEID, 
                    System.Nullable<short> ITEMNO, 
                    string PARTNAME, 
                    string DRAWINGNO, 
                    string POSNO, 
                    string REFNO, 
                    string PARTUNIT, 
                    string ITEM_REMARKS, 
                    System.Nullable<float> INVOICE_QTY, 
                    System.Nullable<float> INVOICE_PRICE, 
                    System.Nullable<float> DISCOUNT, 
                    System.Nullable<float> EXCHRATE, 
                    System.Nullable<System.DateTime> UPDATE_DATE, 
                    System.Nullable<System.DateTime> CREATED_DATE, 
                    System.Nullable<byte> FINAL_DELIVERY, 
                    string UNIT_CODE, 
                    System.Nullable<short> EXPORTED) {
            this._dataAccess.CreateProcedureCommand("sp_SM_REQN_INVOICE_ITEM_Insert");
            this._dataAccess.AddParameter("INVOICE_ITEMID", INVOICE_ITEMID, ParameterDirection.Input);
            this._dataAccess.AddParameter("REQNINVOICEID", REQNINVOICEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEMNO", ITEMNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTNAME", PARTNAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("DRAWINGNO", DRAWINGNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("POSNO", POSNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("REFNO", REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTUNIT", PARTUNIT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_REMARKS", ITEM_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVOICE_QTY", INVOICE_QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVOICE_PRICE", INVOICE_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISCOUNT", DISCOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXCHRATE", EXCHRATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("FINAL_DELIVERY", FINAL_DELIVERY, ParameterDirection.Input);
            this._dataAccess.AddParameter("UNIT_CODE", UNIT_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORTED", EXPORTED, ParameterDirection.Input);
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
