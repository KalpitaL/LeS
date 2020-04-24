namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmReqnInvoice : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmReqnInvoice() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_REQN_INVOICE_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_REQN_INVOICE_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_REQN_INVOICE_Insert(
                    System.Nullable<int> REQNINVOICEID, 
                    string INVOICENO, 
                    string VESSELNAME, 
                    System.Nullable<float> INVOICE_AMOUNT, 
                    System.Nullable<int> FINAL_INVOICE, 
                    string INVOICE_REMARK, 
                    System.Nullable<System.DateTime> INVOICE_DATE, 
                    System.Nullable<System.DateTime> INVOICE_RECV_DATE, 
                    string INVOICE_COMMENT, 
                    System.Nullable<float> VARIANCE, 
                    System.Nullable<float> INVOICE_EXCHRATE, 
                    System.Nullable<System.DateTime> DUE_DATE, 
                    System.Nullable<float> NET_INVOICE_AMOUNT, 
                    System.Nullable<System.DateTime> APPROVED_DATE, 
                    System.Nullable<System.DateTime> PAID_DATE, 
                    System.Nullable<int> VENDOR_ID, 
                    System.Nullable<int> BUYER_ID, 
                    System.Nullable<int> INVOICE_TYPE, 
                    System.Nullable<int> CURRENCYID, 
                    System.Nullable<float> TOT_TAX, 
                    System.Nullable<float> PAID_AMOUNT, 
                    string AC_IBAN_ID, 
                    string AC_BIC_ID, 
                    string PAYMENT_REF, 
                    string ACCOUNT_OWNER, 
                    System.Nullable<int> INVOICE_STATUS, 
                    string SUPPLIER_EMAIL, 
                    string INV_FILENAME, 
                    string ATTACHMENT1, 
                    string ATTACHMENT2, 
                    System.Nullable<System.DateTime> UPDATE_DATE, 
                    System.Nullable<decimal> EXPORTED, 
                    System.Nullable<System.DateTime> CREATED_DATE, 
                    string PORTAL_STATUS, 
                    string CURR_CODE, 
                    System.Nullable<int> BUYERID, 
                    string Addr_MTS_CODE, 
                    System.Nullable<float> Sum_Line_Item, 
                    string ACCOUNT_NUM, 
                    string IBAN_NUM, 
                    string SWIFT_NUM, 
                    System.Nullable<float> VAT_AMOUNT, 
                    System.Nullable<int> VAT_PRCNT, 
                    string INVOICETYPE, 
                    System.Nullable<System.DateTime> SUBMIT_DATE, 
                    System.Nullable<float> AMOUNT, 
                    string STATUS, 
                    string VESSELCODE, 
                    string PO_NO) {
            this._dataAccess.CreateProcedureCommand("sp_SM_REQN_INVOICE_Insert");
            this._dataAccess.AddParameter("REQNINVOICEID", REQNINVOICEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVOICENO", INVOICENO, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSELNAME", VESSELNAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVOICE_AMOUNT", INVOICE_AMOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("FINAL_INVOICE", FINAL_INVOICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVOICE_REMARK", INVOICE_REMARK, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVOICE_DATE", INVOICE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVOICE_RECV_DATE", INVOICE_RECV_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVOICE_COMMENT", INVOICE_COMMENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("VARIANCE", VARIANCE, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVOICE_EXCHRATE", INVOICE_EXCHRATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DUE_DATE", DUE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("NET_INVOICE_AMOUNT", NET_INVOICE_AMOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("APPROVED_DATE", APPROVED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PAID_DATE", PAID_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_ID", VENDOR_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_ID", BUYER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVOICE_TYPE", INVOICE_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURRENCYID", CURRENCYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("TOT_TAX", TOT_TAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("PAID_AMOUNT", PAID_AMOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("AC_IBAN_ID", AC_IBAN_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("AC_BIC_ID", AC_BIC_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("PAYMENT_REF", PAYMENT_REF, ParameterDirection.Input);
            this._dataAccess.AddParameter("ACCOUNT_OWNER", ACCOUNT_OWNER, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVOICE_STATUS", INVOICE_STATUS, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_EMAIL", SUPPLIER_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("INV_FILENAME", INV_FILENAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("ATTACHMENT1", ATTACHMENT1, ParameterDirection.Input);
            this._dataAccess.AddParameter("ATTACHMENT2", ATTACHMENT2, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORTED", EXPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PORTAL_STATUS", PORTAL_STATUS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_CODE", CURR_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYERID", BUYERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("Addr_MTS_CODE", Addr_MTS_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("Sum_Line_Item", Sum_Line_Item, ParameterDirection.Input);
            this._dataAccess.AddParameter("ACCOUNT_NUM", ACCOUNT_NUM, ParameterDirection.Input);
            this._dataAccess.AddParameter("IBAN_NUM", IBAN_NUM, ParameterDirection.Input);
            this._dataAccess.AddParameter("SWIFT_NUM", SWIFT_NUM, ParameterDirection.Input);
            this._dataAccess.AddParameter("VAT_AMOUNT", VAT_AMOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("VAT_PRCNT", VAT_PRCNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVOICETYPE", INVOICETYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUBMIT_DATE", SUBMIT_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("AMOUNT", AMOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("STATUS", STATUS, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSELCODE", VESSELCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PO_NO", PO_NO, ParameterDirection.Input);
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
