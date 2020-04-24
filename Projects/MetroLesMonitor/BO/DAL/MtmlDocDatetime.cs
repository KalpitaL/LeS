namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class MtmlDocDatetime : IDisposable {
        
        private DataAccess _dataAccess;
        
        public MtmlDocDatetime() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet MTML_DOC_DATETIME_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_DATETIME_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet MTML_DOC_DATETIME_Select_One(System.Nullable<System.Guid> MTMLDOCDATETIMEID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_DATETIME_Select_One");
            this._dataAccess.AddParameter("MTMLDOCDATETIMEID", MTMLDOCDATETIMEID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<System.Guid> MTML_DOC_DATETIME_Insert(System.Nullable<System.Guid> MTMLDOCID, string DATE_VALUE, string QUALIFIER, string FORMATQUALIFIER) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_DATETIME_Insert");
            this._dataAccess.AddParameter("MTMLDOCID", MTMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DATE_VALUE", DATE_VALUE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUALIFIER", QUALIFIER, ParameterDirection.Input);
            this._dataAccess.AddParameter("FORMATQUALIFIER", FORMATQUALIFIER, ParameterDirection.Input);
            Guid value = ((Guid)(this._dataAccess.ExecuteScalar()));
            return value;
        }
        
        public virtual System.Nullable<int> MTML_DOC_DATETIME_Delete(System.Nullable<System.Guid> MTMLDOCDATETIMEID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_DATETIME_Delete");
            this._dataAccess.AddParameter("MTMLDOCDATETIMEID", MTMLDOCDATETIMEID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> MTML_DOC_DATETIME_Update(System.Nullable<System.Guid> MTMLDOCDATETIMEID, System.Nullable<System.Guid> MTMLDOCID, string DATE_VALUE, string QUALIFIER, string FORMATQUALIFIER, System.Nullable<int> AUTOID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_DATETIME_Update");
            this._dataAccess.AddParameter("MTMLDOCDATETIMEID", MTMLDOCDATETIMEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MTMLDOCID", MTMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DATE_VALUE", DATE_VALUE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUALIFIER", QUALIFIER, ParameterDirection.Input);
            this._dataAccess.AddParameter("FORMATQUALIFIER", FORMATQUALIFIER, ParameterDirection.Input);
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
