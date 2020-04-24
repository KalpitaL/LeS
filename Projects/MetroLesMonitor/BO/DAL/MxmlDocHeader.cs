namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class MxmlDocHeader : IDisposable {
        
        private DataAccess _dataAccess;
        
        public MxmlDocHeader() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet MXML_DOC_HEADER_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_MXML_DOC_HEADER_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet MXML_DOC_HEADER_Select_One(System.Nullable<System.Guid> MXMLDOCHEADERID) {
            this._dataAccess.CreateProcedureCommand("sp_MXML_DOC_HEADER_Select_One");
            this._dataAccess.AddParameter("MXMLDOCHEADERID", MXMLDOCHEADERID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<System.Guid> MXML_DOC_HEADER_Insert(
                    System.Nullable<System.Guid> MXMLDOCID, 
                    string DOC_PAYLOADID, 
                    System.Nullable<System.DateTime> REQ_DELIVERY_DATE, 
                    string QUOTEID, 
                    string ORIGINATINGSYSTEMREF, 
                    System.Nullable<System.DateTime> QUOTE_DATE, 
                    System.Nullable<System.DateTime> REPLY_BY_DATE, 
                    System.Nullable<System.DateTime> QUOTE_EXPIRY_DATE, 
                    System.Nullable<System.DateTime> ORDER_DATE, 
                    string MESSAGENUMBER, 
                    string DELIVERY_PORT, 
                    string PORT_NAME, 
                    string DELIVERY_PIER, 
                    string VESSEL_ID, 
                    string VESSEL_NAME, 
                    System.Nullable<System.DateTime> ARRIVALDATE, 
                    System.Nullable<System.DateTime> DEPARTUREDATE, 
                    string DOC_CURRENCY, 
                    System.Nullable<float> DOC_TOTAL_AMOUNT, 
                    string DOC_COMMENTS, 
                    System.Nullable<int> EXPORTED, 
                    string QUOTE_COMMENTS, 
                    System.Nullable<System.DateTime> QUOTE_SUBMIT_DATE, 
                    string PO_MESSAGENUMBER, 
                    string PO_PAYLOADID, 
                    string SupplierQuoteID, 
                    string PaymentTerms) {
            this._dataAccess.CreateProcedureCommand("sp_MXML_DOC_HEADER_Insert");
            this._dataAccess.AddParameter("MXMLDOCID", MXMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_PAYLOADID", DOC_PAYLOADID, ParameterDirection.Input);
            this._dataAccess.AddParameter("REQ_DELIVERY_DATE", REQ_DELIVERY_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTEID", QUOTEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORIGINATINGSYSTEMREF", ORIGINATINGSYSTEMREF, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_DATE", QUOTE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("REPLY_BY_DATE", REPLY_BY_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_EXPIRY_DATE", QUOTE_EXPIRY_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORDER_DATE", ORDER_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("MESSAGENUMBER", MESSAGENUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERY_PORT", DELIVERY_PORT, ParameterDirection.Input);
            this._dataAccess.AddParameter("PORT_NAME", PORT_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERY_PIER", DELIVERY_PIER, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_ID", VESSEL_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_NAME", VESSEL_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("ARRIVALDATE", ARRIVALDATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEPARTUREDATE", DEPARTUREDATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_CURRENCY", DOC_CURRENCY, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_TOTAL_AMOUNT", DOC_TOTAL_AMOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_COMMENTS", DOC_COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORTED", EXPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_COMMENTS", QUOTE_COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_SUBMIT_DATE", QUOTE_SUBMIT_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PO_MESSAGENUMBER", PO_MESSAGENUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("PO_PAYLOADID", PO_PAYLOADID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SupplierQuoteID", SupplierQuoteID, ParameterDirection.Input);
            this._dataAccess.AddParameter("PaymentTerms", PaymentTerms, ParameterDirection.Input);
            Guid value = ((Guid)(this._dataAccess.ExecuteScalar()));
            return value;
        }
        
        public virtual System.Nullable<int> MXML_DOC_HEADER_Delete(System.Nullable<System.Guid> MXMLDOCHEADERID) {
            this._dataAccess.CreateProcedureCommand("sp_MXML_DOC_HEADER_Delete");
            this._dataAccess.AddParameter("MXMLDOCHEADERID", MXMLDOCHEADERID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> MXML_DOC_HEADER_Update(
                    System.Nullable<System.Guid> MXMLDOCHEADERID, 
                    System.Nullable<System.Guid> MXMLDOCID, 
                    string DOC_PAYLOADID, 
                    System.Nullable<System.DateTime> REQ_DELIVERY_DATE, 
                    string QUOTEID, 
                    string ORIGINATINGSYSTEMREF, 
                    System.Nullable<System.DateTime> QUOTE_DATE, 
                    System.Nullable<System.DateTime> REPLY_BY_DATE, 
                    System.Nullable<System.DateTime> QUOTE_EXPIRY_DATE, 
                    System.Nullable<System.DateTime> ORDER_DATE, 
                    string MESSAGENUMBER, 
                    string DELIVERY_PORT, 
                    string PORT_NAME, 
                    string DELIVERY_PIER, 
                    string VESSEL_ID, 
                    string VESSEL_NAME, 
                    System.Nullable<System.DateTime> ARRIVALDATE, 
                    System.Nullable<System.DateTime> DEPARTUREDATE, 
                    string DOC_CURRENCY, 
                    System.Nullable<float> DOC_TOTAL_AMOUNT, 
                    string DOC_COMMENTS, 
                    System.Nullable<int> EXPORTED, 
                    string QUOTE_COMMENTS, 
                    System.Nullable<System.DateTime> QUOTE_SUBMIT_DATE, 
                    string PO_MESSAGENUMBER, 
                    string PO_PAYLOADID, 
                    string SupplierQuoteID, 
                    string PaymentTerms) {
            this._dataAccess.CreateProcedureCommand("sp_MXML_DOC_HEADER_Update");
            this._dataAccess.AddParameter("MXMLDOCHEADERID", MXMLDOCHEADERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MXMLDOCID", MXMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_PAYLOADID", DOC_PAYLOADID, ParameterDirection.Input);
            this._dataAccess.AddParameter("REQ_DELIVERY_DATE", REQ_DELIVERY_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTEID", QUOTEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORIGINATINGSYSTEMREF", ORIGINATINGSYSTEMREF, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_DATE", QUOTE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("REPLY_BY_DATE", REPLY_BY_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_EXPIRY_DATE", QUOTE_EXPIRY_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORDER_DATE", ORDER_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("MESSAGENUMBER", MESSAGENUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERY_PORT", DELIVERY_PORT, ParameterDirection.Input);
            this._dataAccess.AddParameter("PORT_NAME", PORT_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERY_PIER", DELIVERY_PIER, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_ID", VESSEL_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_NAME", VESSEL_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("ARRIVALDATE", ARRIVALDATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEPARTUREDATE", DEPARTUREDATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_CURRENCY", DOC_CURRENCY, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_TOTAL_AMOUNT", DOC_TOTAL_AMOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_COMMENTS", DOC_COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORTED", EXPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_COMMENTS", QUOTE_COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_SUBMIT_DATE", QUOTE_SUBMIT_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PO_MESSAGENUMBER", PO_MESSAGENUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("PO_PAYLOADID", PO_PAYLOADID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SupplierQuoteID", SupplierQuoteID, ParameterDirection.Input);
            this._dataAccess.AddParameter("PaymentTerms", PaymentTerms, ParameterDirection.Input);
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
