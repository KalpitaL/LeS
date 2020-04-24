namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class MtmlDocReference : IDisposable {
        
        private DataAccess _dataAccess;
        
        public MtmlDocReference() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet MTML_DOC_REFERENCE_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_REFERENCE_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet MTML_DOC_REFERENCE_Select_One(System.Nullable<System.Guid> MTMLREFERENCEID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_REFERENCE_Select_One");
            this._dataAccess.AddParameter("MTMLREFERENCEID", MTMLREFERENCEID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<System.Guid> MTML_DOC_REFERENCE_Insert(System.Nullable<System.Guid> MTMLDOCID, string QUALIFIER, string REFERENCENUMBER) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_REFERENCE_Insert");
            this._dataAccess.AddParameter("MTMLDOCID", MTMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUALIFIER", QUALIFIER, ParameterDirection.Input);
            this._dataAccess.AddParameter("REFERENCENUMBER", REFERENCENUMBER, ParameterDirection.Input);
            Guid value = ((Guid)(this._dataAccess.ExecuteScalar()));
            return value;
        }
        
        public virtual System.Nullable<int> MTML_DOC_REFERENCE_Delete(System.Nullable<System.Guid> MTMLREFERENCEID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_REFERENCE_Delete");
            this._dataAccess.AddParameter("MTMLREFERENCEID", MTMLREFERENCEID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> MTML_DOC_REFERENCE_Update(System.Nullable<System.Guid> MTMLREFERENCEID, System.Nullable<System.Guid> MTMLDOCID, string QUALIFIER, string REFERENCENUMBER, System.Nullable<int> AUTOID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_REFERENCE_Update");
            this._dataAccess.AddParameter("MTMLREFERENCEID", MTMLREFERENCEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MTMLDOCID", MTMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUALIFIER", QUALIFIER, ParameterDirection.Input);
            this._dataAccess.AddParameter("REFERENCENUMBER", REFERENCENUMBER, ParameterDirection.Input);
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
