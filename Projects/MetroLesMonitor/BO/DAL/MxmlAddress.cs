namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class MxmlAddress : IDisposable {
        
        private DataAccess _dataAccess;
        
        public MxmlAddress() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet Select_MXML_ADDRESSs_By_MXMLDOCID(System.Nullable<System.Guid> MXMLDOCID) {
            this._dataAccess.CreateProcedureCommand("sp_Select_MXML_ADDRESSs_By_MXMLDOCID");
            this._dataAccess.AddParameter("MXMLDOCID", MXMLDOCID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet MXML_ADDRESS_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_MXML_ADDRESS_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet MXML_ADDRESS_Select_One(System.Nullable<System.Guid> MXMLADDRESSID) {
            this._dataAccess.CreateProcedureCommand("sp_MXML_ADDRESS_Select_One");
            this._dataAccess.AddParameter("MXMLADDRESSID", MXMLADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<System.Guid> MXML_ADDRESS_Insert(
                    System.Nullable<System.Guid> MXMLDOCID, 
                    string ADDR_TYPE, 
                    string ADDR_NAME, 
                    string CONTACT_PERSON, 
                    string ADDR_STREET11, 
                    string ADDR_STREET2, 
                    string ADDR_CITY, 
                    string ADDR_POSTCODE, 
                    string ADDR_COUNTRY, 
                    string ADDR_EMAIL, 
                    string ADDR_PHONE_COUNTRY, 
                    string ADDR_PHONE_AREACODE, 
                    string ADDR_PHONE_NUMBER, 
                    string ADDR_FAX_COUNTRY, 
                    string ADDR_FAX_AREACODE, 
                    string ADDR_FAX_NUMBER, 
                    string ADDR_COMMENTS) {
            this._dataAccess.CreateProcedureCommand("sp_MXML_ADDRESS_Insert");
            this._dataAccess.AddParameter("MXMLDOCID", MXMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_TYPE", ADDR_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_NAME", ADDR_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONTACT_PERSON", CONTACT_PERSON, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_STREET11", ADDR_STREET11, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_STREET2", ADDR_STREET2, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_CITY", ADDR_CITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_POSTCODE", ADDR_POSTCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_COUNTRY", ADDR_COUNTRY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_EMAIL", ADDR_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_PHONE_COUNTRY", ADDR_PHONE_COUNTRY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_PHONE_AREACODE", ADDR_PHONE_AREACODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_PHONE_NUMBER", ADDR_PHONE_NUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_FAX_COUNTRY", ADDR_FAX_COUNTRY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_FAX_AREACODE", ADDR_FAX_AREACODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_FAX_NUMBER", ADDR_FAX_NUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_COMMENTS", ADDR_COMMENTS, ParameterDirection.Input);
            Guid value = ((Guid)(this._dataAccess.ExecuteScalar()));
            return value;
        }
        
        public virtual System.Nullable<int> MXML_ADDRESS_Delete(System.Nullable<System.Guid> MXMLADDRESSID) {
            this._dataAccess.CreateProcedureCommand("sp_MXML_ADDRESS_Delete");
            this._dataAccess.AddParameter("MXMLADDRESSID", MXMLADDRESSID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> MXML_ADDRESS_Update(
                    System.Nullable<System.Guid> MXMLADDRESSID, 
                    System.Nullable<System.Guid> MXMLDOCID, 
                    string ADDR_TYPE, 
                    string ADDR_NAME, 
                    string CONTACT_PERSON, 
                    string ADDR_STREET11, 
                    string ADDR_STREET2, 
                    string ADDR_CITY, 
                    string ADDR_POSTCODE, 
                    string ADDR_COUNTRY, 
                    string ADDR_EMAIL, 
                    string ADDR_PHONE_COUNTRY, 
                    string ADDR_PHONE_AREACODE, 
                    string ADDR_PHONE_NUMBER, 
                    string ADDR_FAX_COUNTRY, 
                    string ADDR_FAX_AREACODE, 
                    string ADDR_FAX_NUMBER, 
                    string ADDR_COMMENTS) {
            this._dataAccess.CreateProcedureCommand("sp_MXML_ADDRESS_Update");
            this._dataAccess.AddParameter("MXMLADDRESSID", MXMLADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MXMLDOCID", MXMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_TYPE", ADDR_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_NAME", ADDR_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONTACT_PERSON", CONTACT_PERSON, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_STREET11", ADDR_STREET11, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_STREET2", ADDR_STREET2, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_CITY", ADDR_CITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_POSTCODE", ADDR_POSTCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_COUNTRY", ADDR_COUNTRY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_EMAIL", ADDR_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_PHONE_COUNTRY", ADDR_PHONE_COUNTRY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_PHONE_AREACODE", ADDR_PHONE_AREACODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_PHONE_NUMBER", ADDR_PHONE_NUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_FAX_COUNTRY", ADDR_FAX_COUNTRY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_FAX_AREACODE", ADDR_FAX_AREACODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_FAX_NUMBER", ADDR_FAX_NUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_COMMENTS", ADDR_COMMENTS, ParameterDirection.Input);
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
