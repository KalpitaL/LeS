namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmQuotationsVendorAddress : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmQuotationsVendorAddress() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_QUOTATIONS_VENDOR_ADDRESS_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONS_VENDOR_ADDRESS_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_QUOTATIONS_VENDOR_ADDRESS_Select_One(System.Nullable<int> ADDRESSID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONS_VENDOR_ADDRESS_Select_One");
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_QUOTATIONS_VENDOR_ADDRESS_Insert(
                    System.Nullable<int> QUOTATIONID, 
                    string ADDR_TYPE, 
                    string ADDR_CODE, 
                    string ADDR_NAME, 
                    string CONTACT_PERSON, 
                    string ADDRESS1, 
                    string ADDRESS2, 
                    string ADDRESS3, 
                    string ADDRESS4, 
                    string ADDR_CITY, 
                    string ADDR_COUNTRY, 
                    string ADDR_ZIPCODE, 
                    string ADDR_PHONE1, 
                    string ADDR_PHONE2, 
                    string ADDR_FAX, 
                    string ADDR_TELEX, 
                    string ADDR_EMAIL, 
                    string ADDR_MOBILEPHONE, 
                    System.Nullable<System.DateTime> UPDATE_DATE, 
                    string ADDR_REMARKS, 
                    string EMAIL_CC, 
                    string EMAIL_BCC) {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONS_VENDOR_ADDRESS_Insert");
            this._dataAccess.AddParameter("QUOTATIONID", QUOTATIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_TYPE", ADDR_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_CODE", ADDR_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_NAME", ADDR_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONTACT_PERSON", CONTACT_PERSON, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESS1", ADDRESS1, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESS2", ADDRESS2, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESS3", ADDRESS3, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESS4", ADDRESS4, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_CITY", ADDR_CITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_COUNTRY", ADDR_COUNTRY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_ZIPCODE", ADDR_ZIPCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_PHONE1", ADDR_PHONE1, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_PHONE2", ADDR_PHONE2, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_FAX", ADDR_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_TELEX", ADDR_TELEX, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_EMAIL", ADDR_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_MOBILEPHONE", ADDR_MOBILEPHONE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_REMARKS", ADDR_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("EMAIL_CC", EMAIL_CC, ParameterDirection.Input);
            this._dataAccess.AddParameter("EMAIL_BCC", EMAIL_BCC, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_QUOTATIONS_VENDOR_ADDRESS_Delete(System.Nullable<int> ADDRESSID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONS_VENDOR_ADDRESS_Delete");
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_QUOTATIONS_VENDOR_ADDRESS_Update(
                    System.Nullable<int> ADDRESSID, 
                    System.Nullable<int> QUOTATIONID, 
                    string ADDR_TYPE, 
                    string ADDR_CODE, 
                    string ADDR_NAME, 
                    string CONTACT_PERSON, 
                    string ADDRESS1, 
                    string ADDRESS2, 
                    string ADDRESS3, 
                    string ADDRESS4, 
                    string ADDR_CITY, 
                    string ADDR_COUNTRY, 
                    string ADDR_ZIPCODE, 
                    string ADDR_PHONE1, 
                    string ADDR_PHONE2, 
                    string ADDR_FAX, 
                    string ADDR_TELEX, 
                    string ADDR_EMAIL, 
                    string ADDR_MOBILEPHONE, 
                    System.Nullable<System.DateTime> UPDATE_DATE, 
                    string ADDR_REMARKS, 
                    string EMAIL_CC, 
                    string EMAIL_BCC) {
            this._dataAccess.CreateProcedureCommand("sp_SM_QUOTATIONS_VENDOR_ADDRESS_Update");
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTATIONID", QUOTATIONID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_TYPE", ADDR_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_CODE", ADDR_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_NAME", ADDR_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONTACT_PERSON", CONTACT_PERSON, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESS1", ADDRESS1, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESS2", ADDRESS2, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESS3", ADDRESS3, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESS4", ADDRESS4, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_CITY", ADDR_CITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_COUNTRY", ADDR_COUNTRY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_ZIPCODE", ADDR_ZIPCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_PHONE1", ADDR_PHONE1, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_PHONE2", ADDR_PHONE2, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_FAX", ADDR_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_TELEX", ADDR_TELEX, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_EMAIL", ADDR_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_MOBILEPHONE", ADDR_MOBILEPHONE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_REMARKS", ADDR_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("EMAIL_CC", EMAIL_CC, ParameterDirection.Input);
            this._dataAccess.AddParameter("EMAIL_BCC", EMAIL_BCC, ParameterDirection.Input);
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
