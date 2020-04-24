namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmQuotationsBuyer : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmQuotationsBuyer() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet Select_SM_QUOTATIONS_BUYERs_By_BYR_SUPP_LINKID(System.Nullable<int> BYR_SUPP_LINKID) {
            this._dataAccess.CreateProcedureCommand("sp_Select_SM_QUOTATIONS_BUYERs_By_BYR_SUPP_LINKID");
            this._dataAccess.AddParameter("BYR_SUPP_LINKID", BYR_SUPP_LINKID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_QUOTATIONS_BUYER_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONS_BUYER_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_QUOTATIONS_BUYER_Select_One(System.Nullable<int> QUOTATIONID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONS_BUYER_Select_One");
            this._dataAccess.AddParameter("QUOTATIONID", QUOTATIONID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_QUOTATIONS_BUYER_Insert(
                    string DOC_XML, 
                    string DOCID, 
                    string DOC_TYPE, 
                    string VRNO, 
                    string BUYER_VRNO, 
                    string SUPPLIER_VRNO, 
                    System.Nullable<int> QUOTE_ADDRESSID, 
                    System.Nullable<int> BUYER_ADDRESSID, 
                    System.Nullable<System.DateTime> RFQ_SENT_DATE, 
                    System.Nullable<System.DateTime> QUOTE_RECVD_DATE, 
                    System.Nullable<int> CURRENCYID, 
                    string CURR_CODE, 
                    System.Nullable<float> QUOTE_AMOUNT, 
                    System.Nullable<float> QUOTE_EXCHRATE, 
                    System.Nullable<float> ITEM_TOTAL, 
                    System.Nullable<float> OTHERCOSTS, 
                    System.Nullable<float> FREIGHTAMT, 
                    System.Nullable<int> PAYMENT_TERMS, 
                    System.Nullable<float> QUOTE_DISCOUNT, 
                    System.Nullable<float> ADDITIONAL_DISC, 
                    System.Nullable<byte> ADD_DISC_TYPE, 
                    System.Nullable<System.DateTime> QUOTE_VALIDITY, 
                    string QUOTE_REMARKS, 
                    System.Nullable<System.DateTime> UPDATE_DATE, 
                    System.Nullable<System.DateTime> DELIVERYTIME, 
                    string PAYLOADID, 
                    System.Nullable<System.DateTime> CREATED_DATE, 
                    System.Nullable<int> SITEID, 
                    System.Nullable<int> SENT_BY, 
                    string PORT_CODE, 
                    string PORT_NAME, 
                    System.Nullable<System.DateTime> QUOTE_APPROVEDDATE, 
                    System.Nullable<int> DELIVERYDAYS, 
                    System.Nullable<int> QUOTE_SUBMIT_BY, 
                    string QUOTE_REFERENCE, 
                    System.Nullable<System.DateTime> REPLY_BY_DATE, 
                    System.Nullable<System.DateTime> QUOTE_SUBMIT_DATE, 
                    System.Nullable<int> VENDOR_STATUS, 
                    System.Nullable<byte> CHANGED_BY_VENDOR, 
                    System.Nullable<System.DateTime> LATEDATE, 
                    System.Nullable<System.DateTime> RFQ_ACK_DATE, 
                    System.Nullable<System.DateTime> PO_ACK_DATE, 
                    string POC_REFERENCE, 
                    System.Nullable<System.DateTime> PODATE, 
                    System.Nullable<System.DateTime> POC_DATE, 
                    System.Nullable<int> POC_BY, 
                    string BUYER_REMARKS, 
                    string VESSEL_NAME, 
                    string VESSEL_IDNO, 
                    string VESSEL_OWNER, 
                    string VESSEL_OWNER_CODE, 
                    System.Nullable<short> EXPORTED, 
                    System.Nullable<byte> VERSION, 
                    System.Nullable<byte> RFQ_EXPORT, 
                    string QUOTE_FILE_REF, 
                    System.Nullable<int> PRINT_STATUS, 
                    string QUOTE_FILE_STAMP, 
                    System.Nullable<System.DateTime> DELIVERY_PROMISED, 
                    string GENERAL_TERMS, 
                    string PAY_TERMS, 
                    System.Nullable<float> TAX_PERCNT, 
                    System.Nullable<int> QUOTE_VERSION, 
                    System.Nullable<int> IS_DECLINED, 
                    string QUOTE_SUBJECT, 
                    string SP_MAS_REMARK, 
                    System.Nullable<int> BYR_SUPP_LINKID, 
                    string ATTACHMENT1, 
                    string ATTACHMENT2, 
                    System.Nullable<float> ALLOWANCE, 
                    System.Nullable<int> SALESORDERID, 
                    string UDF1, 
                    string UDF2, 
                    string UDF3, 
                    string RevisionNumber, 
                    System.Nullable<int> LINK_RECORDID, 
                    string TRANSPORT_MODE, 
                    string OrderHandling, 
                    string OrderType, 
                    string OriginatingRequestNo, 
                    string ShipComplete, 
                    string SupplierORGRef, 
                    string UpdType, 
                    string ContractType, 
                    string OrgSystemRef, 
                    System.Nullable<float> OTHER_COST2, 
                    System.Nullable<float> OTHER_COST3, 
                    string VESSEL_ARRTIME, 
                    System.Nullable<System.DateTime> VESSEL_ETA, 
                    System.Nullable<int> QUOTE_STATUS) {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONS_BUYER_Insert");
            this._dataAccess.AddParameter("DOC_XML", DOC_XML, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOCID", DOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_TYPE", DOC_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("VRNO", VRNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_VRNO", BUYER_VRNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_VRNO", SUPPLIER_VRNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_ADDRESSID", QUOTE_ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_ADDRESSID", BUYER_ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RFQ_SENT_DATE", RFQ_SENT_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_RECVD_DATE", QUOTE_RECVD_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURRENCYID", CURRENCYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_CODE", CURR_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_AMOUNT", QUOTE_AMOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_EXCHRATE", QUOTE_EXCHRATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_TOTAL", ITEM_TOTAL, ParameterDirection.Input);
            this._dataAccess.AddParameter("OTHERCOSTS", OTHERCOSTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("FREIGHTAMT", FREIGHTAMT, ParameterDirection.Input);
            this._dataAccess.AddParameter("PAYMENT_TERMS", PAYMENT_TERMS, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_DISCOUNT", QUOTE_DISCOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDITIONAL_DISC", ADDITIONAL_DISC, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADD_DISC_TYPE", ADD_DISC_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_VALIDITY", QUOTE_VALIDITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_REMARKS", QUOTE_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERYTIME", DELIVERYTIME, ParameterDirection.Input);
            this._dataAccess.AddParameter("PAYLOADID", PAYLOADID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SITEID", SITEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SENT_BY", SENT_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("PORT_CODE", PORT_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PORT_NAME", PORT_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_APPROVEDDATE", QUOTE_APPROVEDDATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERYDAYS", DELIVERYDAYS, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_SUBMIT_BY", QUOTE_SUBMIT_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_REFERENCE", QUOTE_REFERENCE, ParameterDirection.Input);
            this._dataAccess.AddParameter("REPLY_BY_DATE", REPLY_BY_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_SUBMIT_DATE", QUOTE_SUBMIT_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_STATUS", VENDOR_STATUS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CHANGED_BY_VENDOR", CHANGED_BY_VENDOR, ParameterDirection.Input);
            this._dataAccess.AddParameter("LATEDATE", LATEDATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("RFQ_ACK_DATE", RFQ_ACK_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PO_ACK_DATE", PO_ACK_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("POC_REFERENCE", POC_REFERENCE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PODATE", PODATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("POC_DATE", POC_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("POC_BY", POC_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_REMARKS", BUYER_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_NAME", VESSEL_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_IDNO", VESSEL_IDNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_OWNER", VESSEL_OWNER, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_OWNER_CODE", VESSEL_OWNER_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORTED", EXPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("VERSION", VERSION, ParameterDirection.Input);
            this._dataAccess.AddParameter("RFQ_EXPORT", RFQ_EXPORT, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_FILE_REF", QUOTE_FILE_REF, ParameterDirection.Input);
            this._dataAccess.AddParameter("PRINT_STATUS", PRINT_STATUS, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_FILE_STAMP", QUOTE_FILE_STAMP, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERY_PROMISED", DELIVERY_PROMISED, ParameterDirection.Input);
            this._dataAccess.AddParameter("GENERAL_TERMS", GENERAL_TERMS, ParameterDirection.Input);
            this._dataAccess.AddParameter("PAY_TERMS", PAY_TERMS, ParameterDirection.Input);
            this._dataAccess.AddParameter("TAX_PERCNT", TAX_PERCNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_VERSION", QUOTE_VERSION, ParameterDirection.Input);
            this._dataAccess.AddParameter("IS_DECLINED", IS_DECLINED, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_SUBJECT", QUOTE_SUBJECT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SP_MAS_REMARK", SP_MAS_REMARK, ParameterDirection.Input);
            this._dataAccess.AddParameter("BYR_SUPP_LINKID", BYR_SUPP_LINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ATTACHMENT1", ATTACHMENT1, ParameterDirection.Input);
            this._dataAccess.AddParameter("ATTACHMENT2", ATTACHMENT2, ParameterDirection.Input);
            this._dataAccess.AddParameter("ALLOWANCE", ALLOWANCE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SALESORDERID", SALESORDERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF1", UDF1, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF2", UDF2, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF3", UDF3, ParameterDirection.Input);
            this._dataAccess.AddParameter("RevisionNumber", RevisionNumber, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINK_RECORDID", LINK_RECORDID, ParameterDirection.Input);
            this._dataAccess.AddParameter("TRANSPORT_MODE", TRANSPORT_MODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("OrderHandling", OrderHandling, ParameterDirection.Input);
            this._dataAccess.AddParameter("OrderType", OrderType, ParameterDirection.Input);
            this._dataAccess.AddParameter("OriginatingRequestNo", OriginatingRequestNo, ParameterDirection.Input);
            this._dataAccess.AddParameter("ShipComplete", ShipComplete, ParameterDirection.Input);
            this._dataAccess.AddParameter("SupplierORGRef", SupplierORGRef, ParameterDirection.Input);
            this._dataAccess.AddParameter("UpdType", UpdType, ParameterDirection.Input);
            this._dataAccess.AddParameter("ContractType", ContractType, ParameterDirection.Input);
            this._dataAccess.AddParameter("OrgSystemRef", OrgSystemRef, ParameterDirection.Input);
            this._dataAccess.AddParameter("OTHER_COST2", OTHER_COST2, ParameterDirection.Input);
            this._dataAccess.AddParameter("OTHER_COST3", OTHER_COST3, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_ARRTIME", VESSEL_ARRTIME, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_ETA", VESSEL_ETA, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_STATUS", QUOTE_STATUS, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_QUOTATIONS_BUYER_Delete(System.Nullable<int> QUOTATIONID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONS_BUYER_Delete");
            this._dataAccess.AddParameter("QUOTATIONID", QUOTATIONID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_QUOTATIONS_BUYER_Update(
                    System.Nullable<int> QUOTATIONID, 
                    string DOC_XML, 
                    string DOCID, 
                    string DOC_TYPE, 
                    string VRNO, 
                    string BUYER_VRNO, 
                    string SUPPLIER_VRNO, 
                    System.Nullable<int> QUOTE_ADDRESSID, 
                    System.Nullable<int> BUYER_ADDRESSID, 
                    System.Nullable<System.DateTime> RFQ_SENT_DATE, 
                    System.Nullable<System.DateTime> QUOTE_RECVD_DATE, 
                    System.Nullable<int> CURRENCYID, 
                    string CURR_CODE, 
                    System.Nullable<float> QUOTE_AMOUNT, 
                    System.Nullable<float> QUOTE_EXCHRATE, 
                    System.Nullable<float> ITEM_TOTAL, 
                    System.Nullable<float> OTHERCOSTS, 
                    System.Nullable<float> FREIGHTAMT, 
                    System.Nullable<int> PAYMENT_TERMS, 
                    System.Nullable<float> QUOTE_DISCOUNT, 
                    System.Nullable<float> ADDITIONAL_DISC, 
                    System.Nullable<byte> ADD_DISC_TYPE, 
                    System.Nullable<System.DateTime> QUOTE_VALIDITY, 
                    string QUOTE_REMARKS, 
                    System.Nullable<System.DateTime> UPDATE_DATE, 
                    System.Nullable<System.DateTime> DELIVERYTIME, 
                    string PAYLOADID, 
                    System.Nullable<System.DateTime> CREATED_DATE, 
                    System.Nullable<int> SITEID, 
                    System.Nullable<int> SENT_BY, 
                    string PORT_CODE, 
                    string PORT_NAME, 
                    System.Nullable<System.DateTime> QUOTE_APPROVEDDATE, 
                    System.Nullable<int> DELIVERYDAYS, 
                    System.Nullable<int> QUOTE_SUBMIT_BY, 
                    string QUOTE_REFERENCE, 
                    System.Nullable<System.DateTime> REPLY_BY_DATE, 
                    System.Nullable<System.DateTime> QUOTE_SUBMIT_DATE, 
                    System.Nullable<int> VENDOR_STATUS, 
                    System.Nullable<byte> CHANGED_BY_VENDOR, 
                    System.Nullable<System.DateTime> LATEDATE, 
                    System.Nullable<System.DateTime> RFQ_ACK_DATE, 
                    System.Nullable<System.DateTime> PO_ACK_DATE, 
                    string POC_REFERENCE, 
                    System.Nullable<System.DateTime> PODATE, 
                    System.Nullable<System.DateTime> POC_DATE, 
                    System.Nullable<int> POC_BY, 
                    string BUYER_REMARKS, 
                    string VESSEL_NAME, 
                    string VESSEL_IDNO, 
                    string VESSEL_OWNER, 
                    string VESSEL_OWNER_CODE, 
                    System.Nullable<short> EXPORTED, 
                    System.Nullable<byte> VERSION, 
                    System.Nullable<byte> RFQ_EXPORT, 
                    string QUOTE_FILE_REF, 
                    System.Nullable<int> PRINT_STATUS, 
                    string QUOTE_FILE_STAMP, 
                    System.Nullable<System.DateTime> DELIVERY_PROMISED, 
                    string GENERAL_TERMS, 
                    string PAY_TERMS, 
                    System.Nullable<float> TAX_PERCNT, 
                    System.Nullable<int> QUOTE_VERSION, 
                    System.Nullable<int> IS_DECLINED, 
                    string QUOTE_SUBJECT, 
                    string SP_MAS_REMARK, 
                    System.Nullable<int> BYR_SUPP_LINKID, 
                    string ATTACHMENT1, 
                    string ATTACHMENT2, 
                    System.Nullable<float> ALLOWANCE, 
                    System.Nullable<int> SALESORDERID, 
                    string UDF1, 
                    string UDF2, 
                    string UDF3, 
                    string RevisionNumber, 
                    System.Nullable<int> LINK_RECORDID, 
                    string TRANSPORT_MODE, 
                    string OrderHandling, 
                    string OrderType, 
                    string OriginatingRequestNo, 
                    string ShipComplete, 
                    string SupplierORGRef, 
                    string UpdType, 
                    string ContractType, 
                    string OrgSystemRef, 
                    System.Nullable<float> OTHER_COST2, 
                    System.Nullable<float> OTHER_COST3, 
                    string VESSEL_ARRTIME, 
                    System.Nullable<System.DateTime> VESSEL_ETA, 
                    System.Nullable<int> QUOTE_STATUS) {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONS_BUYER_Update");
            this._dataAccess.AddParameter("QUOTATIONID", QUOTATIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_XML", DOC_XML, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOCID", DOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_TYPE", DOC_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("VRNO", VRNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_VRNO", BUYER_VRNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_VRNO", SUPPLIER_VRNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_ADDRESSID", QUOTE_ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_ADDRESSID", BUYER_ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RFQ_SENT_DATE", RFQ_SENT_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_RECVD_DATE", QUOTE_RECVD_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURRENCYID", CURRENCYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_CODE", CURR_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_AMOUNT", QUOTE_AMOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_EXCHRATE", QUOTE_EXCHRATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_TOTAL", ITEM_TOTAL, ParameterDirection.Input);
            this._dataAccess.AddParameter("OTHERCOSTS", OTHERCOSTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("FREIGHTAMT", FREIGHTAMT, ParameterDirection.Input);
            this._dataAccess.AddParameter("PAYMENT_TERMS", PAYMENT_TERMS, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_DISCOUNT", QUOTE_DISCOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDITIONAL_DISC", ADDITIONAL_DISC, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADD_DISC_TYPE", ADD_DISC_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_VALIDITY", QUOTE_VALIDITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_REMARKS", QUOTE_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERYTIME", DELIVERYTIME, ParameterDirection.Input);
            this._dataAccess.AddParameter("PAYLOADID", PAYLOADID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SITEID", SITEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SENT_BY", SENT_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("PORT_CODE", PORT_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PORT_NAME", PORT_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_APPROVEDDATE", QUOTE_APPROVEDDATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERYDAYS", DELIVERYDAYS, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_SUBMIT_BY", QUOTE_SUBMIT_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_REFERENCE", QUOTE_REFERENCE, ParameterDirection.Input);
            this._dataAccess.AddParameter("REPLY_BY_DATE", REPLY_BY_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_SUBMIT_DATE", QUOTE_SUBMIT_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_STATUS", VENDOR_STATUS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CHANGED_BY_VENDOR", CHANGED_BY_VENDOR, ParameterDirection.Input);
            this._dataAccess.AddParameter("LATEDATE", LATEDATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("RFQ_ACK_DATE", RFQ_ACK_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PO_ACK_DATE", PO_ACK_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("POC_REFERENCE", POC_REFERENCE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PODATE", PODATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("POC_DATE", POC_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("POC_BY", POC_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_REMARKS", BUYER_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_NAME", VESSEL_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_IDNO", VESSEL_IDNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_OWNER", VESSEL_OWNER, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_OWNER_CODE", VESSEL_OWNER_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORTED", EXPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("VERSION", VERSION, ParameterDirection.Input);
            this._dataAccess.AddParameter("RFQ_EXPORT", RFQ_EXPORT, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_FILE_REF", QUOTE_FILE_REF, ParameterDirection.Input);
            this._dataAccess.AddParameter("PRINT_STATUS", PRINT_STATUS, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_FILE_STAMP", QUOTE_FILE_STAMP, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERY_PROMISED", DELIVERY_PROMISED, ParameterDirection.Input);
            this._dataAccess.AddParameter("GENERAL_TERMS", GENERAL_TERMS, ParameterDirection.Input);
            this._dataAccess.AddParameter("PAY_TERMS", PAY_TERMS, ParameterDirection.Input);
            this._dataAccess.AddParameter("TAX_PERCNT", TAX_PERCNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_VERSION", QUOTE_VERSION, ParameterDirection.Input);
            this._dataAccess.AddParameter("IS_DECLINED", IS_DECLINED, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_SUBJECT", QUOTE_SUBJECT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SP_MAS_REMARK", SP_MAS_REMARK, ParameterDirection.Input);
            this._dataAccess.AddParameter("BYR_SUPP_LINKID", BYR_SUPP_LINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ATTACHMENT1", ATTACHMENT1, ParameterDirection.Input);
            this._dataAccess.AddParameter("ATTACHMENT2", ATTACHMENT2, ParameterDirection.Input);
            this._dataAccess.AddParameter("ALLOWANCE", ALLOWANCE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SALESORDERID", SALESORDERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF1", UDF1, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF2", UDF2, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF3", UDF3, ParameterDirection.Input);
            this._dataAccess.AddParameter("RevisionNumber", RevisionNumber, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINK_RECORDID", LINK_RECORDID, ParameterDirection.Input);
            this._dataAccess.AddParameter("TRANSPORT_MODE", TRANSPORT_MODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("OrderHandling", OrderHandling, ParameterDirection.Input);
            this._dataAccess.AddParameter("OrderType", OrderType, ParameterDirection.Input);
            this._dataAccess.AddParameter("OriginatingRequestNo", OriginatingRequestNo, ParameterDirection.Input);
            this._dataAccess.AddParameter("ShipComplete", ShipComplete, ParameterDirection.Input);
            this._dataAccess.AddParameter("SupplierORGRef", SupplierORGRef, ParameterDirection.Input);
            this._dataAccess.AddParameter("UpdType", UpdType, ParameterDirection.Input);
            this._dataAccess.AddParameter("ContractType", ContractType, ParameterDirection.Input);
            this._dataAccess.AddParameter("OrgSystemRef", OrgSystemRef, ParameterDirection.Input);
            this._dataAccess.AddParameter("OTHER_COST2", OTHER_COST2, ParameterDirection.Input);
            this._dataAccess.AddParameter("OTHER_COST3", OTHER_COST3, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_ARRTIME", VESSEL_ARRTIME, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_ETA", VESSEL_ETA, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_STATUS", QUOTE_STATUS, ParameterDirection.Input);
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
