namespace MetroLesMonitor.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmQuotationsVendor : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmQuotationsVendor() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_QUOTATIONS_VENDOR_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONS_VENDOR_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_QUOTATIONS_VENDOR_Select_ByVRNO(string VRNO)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SM_QUOTATIONS_VENDOR WHERE DOC_TYPE='RequestForQuote' AND VRNO LIKE '%" + VRNO + "%' ");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_QUOTATIONS_VENDOR_Select_One(System.Nullable<int> QUOTATIONID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONS_VENDOR_Select_One");
            this._dataAccess.AddParameter("QUOTATIONID", QUOTATIONID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet Select_SM_QUOTATIONS_VENDOR_By_BuyerAddressID(string DocType, int BuyerAddressID, Nullable<DateTime> FromDate, Nullable<DateTime> ToDate) 
        {
            string cSQL = "SELECT COUNT(*) FROM SM_QUOTATIONS_VENDOR WHERE BUYER_ADDRESSID = @BUYER_ADDRESSID AND ";
            if (DocType == "RFQ")
            {
                cSQL = cSQL + "(DOC_TYPE = 'RequestForQuote' OR DOC_TYPE ='RFQ')";
            }
            else if(DocType == "Quote")
            {
                cSQL = cSQL + "(DOC_TYPE = 'Quote' OR DOC_TYPE = 'Quotation')";
            }
            else if (DocType == "PO")
            {
                cSQL = cSQL + "(DOC_TYPE = 'Order' OR DOC_TYPE = 'PurchaseOrder')";
            }
            else if (DocType == "POC")
            {
                cSQL = cSQL + "(DOC_TYPE = 'POC' OR DOC_TYPE = 'OrderResponse' OR DOC_TYPE = 'OrderConfirmation')";
            }

            if (FromDate != null && ToDate != null)
            {
                cSQL = cSQL + " AND CREATED_DATE >= @FROM_DATE AND CREATED_DATE <= @TO_DATE";
            }
            
            this._dataAccess.CreateSQLCommand(cSQL);           
            this._dataAccess.AddParameter("BUYER_ADDRESSID", BuyerAddressID, ParameterDirection.Input);
            this._dataAccess.AddParameter("FROM_DATE", FromDate, ParameterDirection.Input);
            this._dataAccess.AddParameter("TO_DATE", ToDate, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet Select_SM_QUOTATIONS_VENDOR_By_QuoteAddressID(string DocType, int SuppAddressID, Nullable<DateTime> FromDate, Nullable<DateTime> ToDate) 
        {
            string cSQL = "SELECT COUNT(*) FROM SM_QUOTATIONS_VENDOR WHERE QUOTE_ADDRESSID = @QUOTE_ADDRESSID AND ";
            if (DocType == "RFQ")
            {
                cSQL = cSQL + "(DOC_TYPE = 'RequestForQuote' OR DOC_TYPE ='RFQ')";
            }
            else if (DocType == "Quote")
            {
                cSQL = cSQL + "(DOC_TYPE = 'Quote' OR DOC_TYPE = 'Quotation')";
            }
            else if (DocType == "PO")
            {
                cSQL = cSQL + "(DOC_TYPE = 'Order' OR DOC_TYPE = 'PurchaseOrder')";
            }
            else if (DocType == "POC")
            {
                cSQL = cSQL + "(DOC_TYPE = 'POC' OR DOC_TYPE = 'OrderResponse' OR DOC_TYPE = 'OrderConfirmation')";
            }

            if (FromDate != null && ToDate != null)
            {
                cSQL = cSQL + " AND CREATED_DATE >= @FROM_DATE AND CREATED_DATE <= @TO_DATE";
            }
            
            this._dataAccess.CreateSQLCommand(cSQL);
            this._dataAccess.AddParameter("QUOTE_ADDRESSID", SuppAddressID, ParameterDirection.Input);
            this._dataAccess.AddParameter("FROM_DATE", FromDate, ParameterDirection.Input);
            this._dataAccess.AddParameter("TO_DATE", ToDate, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_QUOTATIONS_VENDOR_Insert(
                    string DOC_XML, 
                    string DOCID, 
                    string DOC_TYPE, 
                    string VRNO, 
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
                    System.Nullable<int> BYR_SUPP_LINKID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONS_VENDOR_Insert");
            this._dataAccess.AddParameter("DOC_XML", DOC_XML, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOCID", DOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_TYPE", DOC_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("VRNO", VRNO, ParameterDirection.Input);
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
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_QUOTATIONS_VENDOR_Delete(System.Nullable<int> QUOTATIONID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONS_VENDOR_Delete");
            this._dataAccess.AddParameter("QUOTATIONID", QUOTATIONID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_QUOTATIONS_VENDOR_Update(
                    System.Nullable<int> QUOTATIONID, 
                    string DOC_XML, 
                    string DOCID, 
                    string DOC_TYPE, 
                    string VRNO, 
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
                    System.Nullable<int> BYR_SUPP_LINKID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONS_VENDOR_Update");
            this._dataAccess.AddParameter("QUOTATIONID", QUOTATIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_XML", DOC_XML, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOCID", DOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_TYPE", DOC_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("VRNO", VRNO, ParameterDirection.Input);
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
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual void Dispose() {
            if ((this._dataAccess != null)) {
                this._dataAccess.Dispose();
            }
        }

        public virtual System.Data.DataSet SMV_QUOTATION_VENDOR_BY_LINKID(System.Nullable<int> LINKID, System.Nullable<System.DateTime> FromDate, System.Nullable<System.DateTime> ToDate)
        {
            this._dataAccess.CreateProcedureCommand("sp_SMV_QUOTATION_VENDOR_LINKID_Select_All_By_Dates");
            this._dataAccess.AddParameter("LINKID", LINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("FROM_DATE", FromDate, ParameterDirection.Input);
            this._dataAccess.AddParameter("TO_DATE", ToDate, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet Select_SM_QUOTATIONS_VENDOR_By_Distinct_DOC_TYPE()
        {
            string cSQL = "SELECT DISTINCT DOC_TYPE FROM SMV_QUOTATIONS_VENDOR";
            this._dataAccess.CreateSQLCommand(cSQL);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual string UpdateExportMarker(string QUOTATIONID, string DOC_TYPE,int EXPORTMARKER)
        {
            string value = "";
            try
            {
                if (DOC_TYPE == "RequestForQuote")
                {
                    string cSQL = "UPDATE SM_QUOTATIONS_VENDOR SET RFQ_EXPORT=@EXPORTMARKER,UPDATE_DATE=GETDATE() WHERE QUOTATIONID = @QUOTATIONID";
                    this._dataAccess.CreateSQLCommand(cSQL);
                    this._dataAccess.AddParameter("EXPORTMARKER", EXPORTMARKER, ParameterDirection.Input);
                    this._dataAccess.AddParameter("QUOTATIONID",convert.ToInt(QUOTATIONID), ParameterDirection.Input);
                    value = convert.ToString(this._dataAccess.ExecuteNonQuery());
                }
                else
                {
                    string cSQL = "UPDATE SM_QUOTATIONS_VENDOR SET EXPORTED=@EXPORTMARKER ,UPDATE_DATE=GETDATE() WHERE QUOTATIONID = @QUOTATIONID";
                    this._dataAccess.CreateSQLCommand(cSQL);
                    this._dataAccess.AddParameter("EXPORTMARKER", EXPORTMARKER, ParameterDirection.Input);
                    this._dataAccess.AddParameter("QUOTATIONID", convert.ToInt(QUOTATIONID), ParameterDirection.Input);
                    value = convert.ToString(this._dataAccess.ExecuteNonQuery());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return value;
        }
    }
}
