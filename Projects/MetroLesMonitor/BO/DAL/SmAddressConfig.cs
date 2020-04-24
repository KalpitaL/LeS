using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Xml;
      
namespace MetroLesMonitor.Dal {
    
    public partial class SmAddressConfig : IDisposable {

        public DataAccess _dataAccess;
        
        public SmAddressConfig() {
            this._dataAccess = new DataAccess();
        }

        public SmAddressConfig(DataAccess _dataAccess)
        {
            this._dataAccess = (_dataAccess != null) ? _dataAccess : new DataAccess();
        }
        
        public virtual System.Data.DataSet Select_SM_ADDRESS_CONFIGs_By_ADDRESSID(System.Nullable<int> ADDRESSID) {
            this._dataAccess.CreateProcedureCommand("sp_Select_SM_ADDRESS_CONFIGs_By_ADDRESSID");
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_ADDRESS_CONFIG_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_ADDRESS_CONFIG_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_ADDRESS_CONFIG_Select_One(System.Nullable<int> ADDRCONFIGID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_ADDRESS_CONFIG_Select_One");
            this._dataAccess.AddParameter("ADDRCONFIGID", ADDRCONFIGID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_ADDRESS_CONFIG_By_Buyer(int ADDRESSID)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SM_ADDRESS_CONFIG WHERE ADDRESSID=@ADDRESSID");
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }


        public virtual System.Data.DataSet SM_ADDRESS_CONFIG_By_ID_Format(string FORMAT, int ADDRESSID)
        {
            string cSQL = "SELECT * FROM SM_ADDRESS_CONFIG WHERE ADDRESSID=@ADDRESSID";
            if (!string.IsNullOrEmpty(FORMAT)) { cSQL = cSQL + " AND DEFAULT_FORMAT = @FORMAT"; }
            this._dataAccess.CreateSQLCommand(cSQL);
            this._dataAccess.AddParameter("FORMAT", FORMAT, ParameterDirection.Input); 
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Nullable<int> SM_ADDRESS_CONFIG_Insert(
                    System.Nullable<int> ADDRESSID,
                    string PARTY_MAPPING,                   
                    string DEFAULT_FORMAT,
                    string EXPORT_PATH,
                    string IMPORT_PATH,
                    System.Nullable<short> IMPORT_RFQ,
                    System.Nullable<short> EXPORT_RFQ,
                    System.Nullable<short> EXPORT_RFQ_ACK,
                    System.Nullable<short> IMPORT_QUOTE,
                    System.Nullable<short> EXPORT_QUOTE,
                    System.Nullable<short> IMPORT_PO,
                    System.Nullable<short> EXPORT_PO,
                    System.Nullable<short> EXPORT_PO_ACK,
                    System.Nullable<short> EXPORT_POC,                    
                    System.Nullable<short> MAIL_NOTIFY,
                    System.Nullable<float> DEFAULT_PRICE,
                    string UPLOAD_FILE_TYPE,
                    string MAIL_SUBJECT,
                    string SUP_WEB_SERVICE_URL,
                    System.Nullable<System.DateTime> CREATED_DATE,
                    System.Nullable<System.DateTime> UPDATE_DATE,
                    string CC_EMAIL, string IDENTIFICATION_CODE,System.Nullable<short> IMPORT_POC)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_ADDRESS_CONFIG_Insert");
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTY_MAPPING", PARTY_MAPPING, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEFAULT_FORMAT", DEFAULT_FORMAT, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_PATH", EXPORT_PATH, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_PATH", IMPORT_PATH, ParameterDirection.Input);            
            this._dataAccess.AddParameter("IMPORT_RFQ", IMPORT_RFQ, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_RFQ", EXPORT_RFQ, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_RFQ_ACK", EXPORT_RFQ_ACK, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_QUOTE", IMPORT_QUOTE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_QUOTE", EXPORT_QUOTE, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_PO", IMPORT_PO, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_PO", EXPORT_PO, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_PO_ACK", EXPORT_PO_ACK, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_POC", EXPORT_POC, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_NOTIFY", MAIL_NOTIFY, ParameterDirection.Input);            
            this._dataAccess.AddParameter("DEFAULT_PRICE", DEFAULT_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPLOAD_FILE_TYPE", UPLOAD_FILE_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_SUBJECT", MAIL_SUBJECT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUP_WEB_SERVICE_URL", SUP_WEB_SERVICE_URL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CC_EMAIL", CC_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("IDENTIFICATION_CODE", IDENTIFICATION_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_POC", IMPORT_POC, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_ADDRESS_CONFIG_Delete(System.Nullable<int> ADDRCONFIGID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_ADDRESS_CONFIG_Delete");
            this._dataAccess.AddParameter("ADDRCONFIGID", ADDRCONFIGID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_ADDRESS_CONFIG_Update(
                    System.Nullable<int> ADDRCONFIGID, 
                    System.Nullable<int> ADDRESSID,
                    string PARTY_MAPPING, 
                    string DEFAULT_FORMAT, 
                    string EXPORT_PATH, 
                    string IMPORT_PATH, 
                    System.Nullable<short> IMPORT_RFQ, 
                    System.Nullable<short> EXPORT_RFQ, 
                    System.Nullable<short> EXPORT_RFQ_ACK, 
                    System.Nullable<short> IMPORT_QUOTE, 
                    System.Nullable<short> EXPORT_QUOTE, 
                    System.Nullable<short> IMPORT_PO, 
                    System.Nullable<short> EXPORT_PO, 
                    System.Nullable<short> EXPORT_PO_ACK, 
                    System.Nullable<short> EXPORT_POC,
                    System.Nullable<short> MAIL_NOTIFY, 
                    System.Nullable<float> DEFAULT_PRICE, 
                    string UPLOAD_FILE_TYPE, 
                    string MAIL_SUBJECT, 
                    string SUP_WEB_SERVICE_URL, 
                    System.Nullable<System.DateTime> CREATED_DATE, 
                    System.Nullable<System.DateTime> UPDATE_DATE,
                    string CC_EMAIL, string IDENTIFICATION_CODE, System.Nullable<short> IMPORT_POC)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_ADDRESS_CONFIG_Update");
            this._dataAccess.AddParameter("ADDRCONFIGID", ADDRCONFIGID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTY_MAPPING", PARTY_MAPPING, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEFAULT_FORMAT", DEFAULT_FORMAT, ParameterDirection.Input);            
            this._dataAccess.AddParameter("EXPORT_PATH", EXPORT_PATH, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_PATH", IMPORT_PATH, ParameterDirection.Input);            
            this._dataAccess.AddParameter("IMPORT_RFQ", IMPORT_RFQ, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_RFQ", EXPORT_RFQ, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_RFQ_ACK", EXPORT_RFQ_ACK, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_QUOTE", IMPORT_QUOTE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_QUOTE", EXPORT_QUOTE, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_PO", IMPORT_PO, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_PO", EXPORT_PO, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_PO_ACK", EXPORT_PO_ACK, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_POC", EXPORT_POC, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_NOTIFY", MAIL_NOTIFY, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEFAULT_PRICE", DEFAULT_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPLOAD_FILE_TYPE", UPLOAD_FILE_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_SUBJECT", MAIL_SUBJECT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUP_WEB_SERVICE_URL", SUP_WEB_SERVICE_URL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CC_EMAIL", CC_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("IDENTIFICATION_CODE", IDENTIFICATION_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_POC", IMPORT_POC, ParameterDirection.Input);
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
