namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class MtmlDocComments : IDisposable {
        
        private DataAccess _dataAccess;
        
        public MtmlDocComments() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet MTML_DOC_COMMENTS_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_COMMENTS_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet MTML_DOC_COMMENTS_Select_One(System.Nullable<System.Guid> MTMLDOCCOMMENTID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_COMMENTS_Select_One");
            this._dataAccess.AddParameter("MTMLDOCCOMMENTID", MTMLDOCCOMMENTID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<System.Guid> MTML_DOC_COMMENTS_Insert(System.Nullable<System.Guid> MTMLDOCID, string COMMENTS, string QUALIFIER) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_COMMENTS_Insert");
            this._dataAccess.AddParameter("MTMLDOCID", MTMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("COMMENTS", COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUALIFIER", QUALIFIER, ParameterDirection.Input);
            Guid value = ((Guid)(this._dataAccess.ExecuteScalar()));
            return value;
        }
        
        public virtual System.Nullable<int> MTML_DOC_COMMENTS_Delete(System.Nullable<System.Guid> MTMLDOCCOMMENTID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_COMMENTS_Delete");
            this._dataAccess.AddParameter("MTMLDOCCOMMENTID", MTMLDOCCOMMENTID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> MTML_DOC_COMMENTS_Update(System.Nullable<System.Guid> MTMLDOCCOMMENTID, System.Nullable<System.Guid> MTMLDOCID, string COMMENTS, string QUALIFIER, System.Nullable<int> AUTOID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_COMMENTS_Update");
            this._dataAccess.AddParameter("MTMLDOCCOMMENTID", MTMLDOCCOMMENTID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MTMLDOCID", MTMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("COMMENTS", COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUALIFIER", QUALIFIER, ParameterDirection.Input);
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
