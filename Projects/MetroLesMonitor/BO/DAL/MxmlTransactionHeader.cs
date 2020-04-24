namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class MxmlTransactionHeader : IDisposable {
        
        private DataAccess _dataAccess;
        
        public MxmlTransactionHeader() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet MXML_TRANSACTION_HEADER_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_MXML_TRANSACTION_HEADER_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet MXML_TRANSACTION_HEADER_Select_One(System.Nullable<System.Guid> MXMLDOCID) {
            this._dataAccess.CreateProcedureCommand("sp_MXML_TRANSACTION_HEADER_Select_One");
            this._dataAccess.AddParameter("MXMLDOCID", MXMLDOCID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<System.Guid> MXML_TRANSACTION_HEADER_Insert(string PAYLOADID, string SERVER_TIMESTAMP, string FROM_PARTYID, string FROM_ADAPTERUID, string TO_PARTYID, string DOCTYPE, string USERAGENT) {
            this._dataAccess.CreateProcedureCommand("sp_MXML_TRANSACTION_HEADER_Insert");
            this._dataAccess.AddParameter("PAYLOADID", PAYLOADID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SERVER_TIMESTAMP", SERVER_TIMESTAMP, ParameterDirection.Input);
            this._dataAccess.AddParameter("FROM_PARTYID", FROM_PARTYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("FROM_ADAPTERUID", FROM_ADAPTERUID, ParameterDirection.Input);
            this._dataAccess.AddParameter("TO_PARTYID", TO_PARTYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOCTYPE", DOCTYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("USERAGENT", USERAGENT, ParameterDirection.Input);
            Guid value = ((Guid)(this._dataAccess.ExecuteScalar()));
            return value;
        }
        
        public virtual System.Nullable<int> MXML_TRANSACTION_HEADER_Delete(System.Nullable<System.Guid> MXMLDOCID) {
            this._dataAccess.CreateProcedureCommand("sp_MXML_TRANSACTION_HEADER_Delete");
            this._dataAccess.AddParameter("MXMLDOCID", MXMLDOCID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> MXML_TRANSACTION_HEADER_Update(System.Nullable<System.Guid> MXMLDOCID, string PAYLOADID, string SERVER_TIMESTAMP, string FROM_PARTYID, string FROM_ADAPTERUID, string TO_PARTYID, string DOCTYPE, string USERAGENT) {
            this._dataAccess.CreateProcedureCommand("sp_MXML_TRANSACTION_HEADER_Update");
            this._dataAccess.AddParameter("MXMLDOCID", MXMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("PAYLOADID", PAYLOADID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SERVER_TIMESTAMP", SERVER_TIMESTAMP, ParameterDirection.Input);
            this._dataAccess.AddParameter("FROM_PARTYID", FROM_PARTYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("FROM_ADAPTERUID", FROM_ADAPTERUID, ParameterDirection.Input);
            this._dataAccess.AddParameter("TO_PARTYID", TO_PARTYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOCTYPE", DOCTYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("USERAGENT", USERAGENT, ParameterDirection.Input);
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
