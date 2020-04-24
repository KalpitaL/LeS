namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class MtmlDocParty : IDisposable {
        
        private DataAccess _dataAccess;
        
        public MtmlDocParty() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet MTML_DOC_PARTY_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_PARTY_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet MTML_DOC_PARTY_Select_One(System.Nullable<System.Guid> MTMLPARTYID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_PARTY_Select_One");
            this._dataAccess.AddParameter("MTMLPARTYID", MTMLPARTYID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<System.Guid> MTML_DOC_PARTY_Insert(
                    System.Nullable<System.Guid> MTMLDOCID, 
                    string QUALIFIER, 
                    string PARTY_NAME, 
                    string IDENTIFICATION, 
                    string STREETADDRESS, 
                    string CITY, 
                    string POSTCODE, 
                    string STATE, 
                    string COUNTRYCODE, 
                    string FUNCTIONCODE, 
                    string CONTACT_NAME, 
                    string PHONENUMBER, 
                    string FAX, 
                    string EMAIL, 
                    string PARTY_LOCATION, 
                    string STREETADDRESS2, 
                    string STREETADDRESS3, 
                    string EMAIL_CC, 
                    string EMAIL_BCC) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_PARTY_Insert");
            this._dataAccess.AddParameter("MTMLDOCID", MTMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUALIFIER", QUALIFIER, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTY_NAME", PARTY_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("IDENTIFICATION", IDENTIFICATION, ParameterDirection.Input);
            this._dataAccess.AddParameter("STREETADDRESS", STREETADDRESS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CITY", CITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("POSTCODE", POSTCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("STATE", STATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("COUNTRYCODE", COUNTRYCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("FUNCTIONCODE", FUNCTIONCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONTACT_NAME", CONTACT_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("PHONENUMBER", PHONENUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("FAX", FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("EMAIL", EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTY_LOCATION", PARTY_LOCATION, ParameterDirection.Input);
            this._dataAccess.AddParameter("STREETADDRESS2", STREETADDRESS2, ParameterDirection.Input);
            this._dataAccess.AddParameter("STREETADDRESS3", STREETADDRESS3, ParameterDirection.Input);
            this._dataAccess.AddParameter("EMAIL_CC", EMAIL_CC, ParameterDirection.Input);
            this._dataAccess.AddParameter("EMAIL_BCC", EMAIL_BCC, ParameterDirection.Input);
            Guid value = ((Guid)(this._dataAccess.ExecuteScalar()));
            return value;
        }
        
        public virtual System.Nullable<int> MTML_DOC_PARTY_Delete(System.Nullable<System.Guid> MTMLPARTYID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_PARTY_Delete");
            this._dataAccess.AddParameter("MTMLPARTYID", MTMLPARTYID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> MTML_DOC_PARTY_Update(
                    System.Nullable<System.Guid> MTMLPARTYID, 
                    System.Nullable<System.Guid> MTMLDOCID, 
                    string QUALIFIER, 
                    string PARTY_NAME, 
                    string IDENTIFICATION, 
                    string STREETADDRESS, 
                    string CITY, 
                    string POSTCODE, 
                    string STATE, 
                    string COUNTRYCODE, 
                    string FUNCTIONCODE, 
                    string CONTACT_NAME, 
                    string PHONENUMBER, 
                    string FAX, 
                    string EMAIL, 
                    string PARTY_LOCATION, 
                    string STREETADDRESS2, 
                    string STREETADDRESS3, 
                    string EMAIL_CC, 
                    string EMAIL_BCC, 
                    System.Nullable<int> AUTOID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_PARTY_Update");
            this._dataAccess.AddParameter("MTMLPARTYID", MTMLPARTYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MTMLDOCID", MTMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUALIFIER", QUALIFIER, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTY_NAME", PARTY_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("IDENTIFICATION", IDENTIFICATION, ParameterDirection.Input);
            this._dataAccess.AddParameter("STREETADDRESS", STREETADDRESS, ParameterDirection.Input);
            this._dataAccess.AddParameter("CITY", CITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("POSTCODE", POSTCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("STATE", STATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("COUNTRYCODE", COUNTRYCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("FUNCTIONCODE", FUNCTIONCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONTACT_NAME", CONTACT_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("PHONENUMBER", PHONENUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("FAX", FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("EMAIL", EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTY_LOCATION", PARTY_LOCATION, ParameterDirection.Input);
            this._dataAccess.AddParameter("STREETADDRESS2", STREETADDRESS2, ParameterDirection.Input);
            this._dataAccess.AddParameter("STREETADDRESS3", STREETADDRESS3, ParameterDirection.Input);
            this._dataAccess.AddParameter("EMAIL_CC", EMAIL_CC, ParameterDirection.Input);
            this._dataAccess.AddParameter("EMAIL_BCC", EMAIL_BCC, ParameterDirection.Input);
            this._dataAccess.AddParameter("AUTOID", AUTOID, ParameterDirection.Input);
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
