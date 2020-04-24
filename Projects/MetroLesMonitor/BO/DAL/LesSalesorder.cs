namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class LesSalesorder : IDisposable {
        
        private DataAccess _dataAccess;
        
        public LesSalesorder() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet LES_SALESORDER_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_LES_SALESORDER_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_SALESORDER_Select_One(System.Nullable<int> SALESORDERID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_SALESORDER_Select_One");
            this._dataAccess.AddParameter("SALESORDERID", SALESORDERID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> LES_SALESORDER_Insert(
                    string SALESORDERNO, 
                    System.Nullable<int> CUSTOMERID, 
                    System.Nullable<int> STATUS, 
                    System.Nullable<int> CURRENCYID, 
                    string BUYERREF, 
                    string SHIPNAME, 
                    System.Nullable<System.DateTime> RFQ_DEL_DATE, 
                    System.Nullable<int> DELIVERY_DAYS, 
                    System.Nullable<float> DISCOUNT, 
                    string SUPPLIER_REMARKS, 
                    string PAY_TERMS, 
                    string GENERAL_TERMS, 
                    System.Nullable<float> PROFITMARGIN, 
                    System.Nullable<float> AMOUNT, 
                    System.Nullable<float> FREIGHTAMT, 
                    System.Nullable<float> OTHERCOSTS, 
                    string QUOTE_REFERENCE, 
                    System.Nullable<System.DateTime> QUOTE_VALIDITY, 
                    string QUOTE_SUBJECT, 
                    System.Nullable<int> QUOTATIONID, 
                    System.Nullable<System.DateTime> UPDATE_DATE, 
                    System.Nullable<System.DateTime> CREATED_DATE, 
                    System.Nullable<int> UPDATED_BY, 
                    System.Nullable<int> CREATED_BY, 
                    System.Nullable<System.DateTime> VESSEL_ETA, 
                    string VESSEL_ARRTIME, 
                    System.Nullable<float> ITEM_TOTAL) {
            this._dataAccess.CreateProcedureCommand("sp_LES_SALESORDER_Insert");
            this._dataAccess.AddParameter("SALESORDERNO", SALESORDERNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("CUSTOMERID", CUSTOMERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("STATUS", STATUS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURRENCYID", CURRENCYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYERREF", BUYERREF, ParameterDirection.Input);
            this._dataAccess.AddParameter("SHIPNAME", SHIPNAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("RFQ_DEL_DATE", RFQ_DEL_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERY_DAYS", DELIVERY_DAYS, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISCOUNT", DISCOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_REMARKS", SUPPLIER_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("PAY_TERMS", PAY_TERMS, ParameterDirection.Input);
            this._dataAccess.AddParameter("GENERAL_TERMS", GENERAL_TERMS, ParameterDirection.Input);
            this._dataAccess.AddParameter("PROFITMARGIN", PROFITMARGIN, ParameterDirection.Input);
            this._dataAccess.AddParameter("AMOUNT", AMOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("FREIGHTAMT", FREIGHTAMT, ParameterDirection.Input);
            this._dataAccess.AddParameter("OTHERCOSTS", OTHERCOSTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_REFERENCE", QUOTE_REFERENCE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_VALIDITY", QUOTE_VALIDITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_SUBJECT", QUOTE_SUBJECT, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTATIONID", QUOTATIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATED_BY", UPDATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_BY", CREATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_ETA", VESSEL_ETA, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_ARRTIME", VESSEL_ARRTIME, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_TOTAL", ITEM_TOTAL, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_SALESORDER_Delete(System.Nullable<int> SALESORDERID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_SALESORDER_Delete");
            this._dataAccess.AddParameter("SALESORDERID", SALESORDERID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_SALESORDER_Update(
                    System.Nullable<int> SALESORDERID, 
                    string SALESORDERNO, 
                    System.Nullable<int> CUSTOMERID, 
                    System.Nullable<int> STATUS, 
                    System.Nullable<int> CURRENCYID, 
                    string BUYERREF, 
                    string SHIPNAME, 
                    System.Nullable<System.DateTime> RFQ_DEL_DATE, 
                    System.Nullable<int> DELIVERY_DAYS, 
                    System.Nullable<float> DISCOUNT, 
                    string SUPPLIER_REMARKS, 
                    string PAY_TERMS, 
                    string GENERAL_TERMS, 
                    System.Nullable<float> PROFITMARGIN, 
                    System.Nullable<float> AMOUNT, 
                    System.Nullable<float> FREIGHTAMT, 
                    System.Nullable<float> OTHERCOSTS, 
                    string QUOTE_REFERENCE, 
                    System.Nullable<System.DateTime> QUOTE_VALIDITY, 
                    string QUOTE_SUBJECT, 
                    System.Nullable<int> QUOTATIONID, 
                    System.Nullable<System.DateTime> UPDATE_DATE, 
                    System.Nullable<System.DateTime> CREATED_DATE, 
                    System.Nullable<int> UPDATED_BY, 
                    System.Nullable<int> CREATED_BY, 
                    System.Nullable<System.DateTime> VESSEL_ETA, 
                    string VESSEL_ARRTIME, 
                    System.Nullable<float> ITEM_TOTAL) {
            this._dataAccess.CreateProcedureCommand("sp_LES_SALESORDER_Update");
            this._dataAccess.AddParameter("SALESORDERID", SALESORDERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SALESORDERNO", SALESORDERNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("CUSTOMERID", CUSTOMERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("STATUS", STATUS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURRENCYID", CURRENCYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYERREF", BUYERREF, ParameterDirection.Input);
            this._dataAccess.AddParameter("SHIPNAME", SHIPNAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("RFQ_DEL_DATE", RFQ_DEL_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERY_DAYS", DELIVERY_DAYS, ParameterDirection.Input);
            this._dataAccess.AddParameter("DISCOUNT", DISCOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_REMARKS", SUPPLIER_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("PAY_TERMS", PAY_TERMS, ParameterDirection.Input);
            this._dataAccess.AddParameter("GENERAL_TERMS", GENERAL_TERMS, ParameterDirection.Input);
            this._dataAccess.AddParameter("PROFITMARGIN", PROFITMARGIN, ParameterDirection.Input);
            this._dataAccess.AddParameter("AMOUNT", AMOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("FREIGHTAMT", FREIGHTAMT, ParameterDirection.Input);
            this._dataAccess.AddParameter("OTHERCOSTS", OTHERCOSTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_REFERENCE", QUOTE_REFERENCE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_VALIDITY", QUOTE_VALIDITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_SUBJECT", QUOTE_SUBJECT, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTATIONID", QUOTATIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATED_BY", UPDATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_BY", CREATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_ETA", VESSEL_ETA, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_ARRTIME", VESSEL_ARRTIME, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_TOTAL", ITEM_TOTAL, ParameterDirection.Input);
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
