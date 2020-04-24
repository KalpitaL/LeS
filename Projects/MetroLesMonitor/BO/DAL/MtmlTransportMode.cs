namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class MtmlTransportMode : IDisposable {
        
        private DataAccess _dataAccess;
        
        public MtmlTransportMode() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet MTML_TRANSPORT_MODE_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_MTML_TRANSPORT_MODE_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet MTML_TRANSPORT_MODE_Select_One(System.Nullable<int> TRANSPORT_ID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_TRANSPORT_MODE_Select_One");
            this._dataAccess.AddParameter("TRANSPORT_ID", TRANSPORT_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> MTML_TRANSPORT_MODE_Insert(string TRANSPORT_MODE, string TRANSPORT_DESC) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_TRANSPORT_MODE_Insert");
            this._dataAccess.AddParameter("TRANSPORT_MODE", TRANSPORT_MODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("TRANSPORT_DESC", TRANSPORT_DESC, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> MTML_TRANSPORT_MODE_Delete(System.Nullable<int> TRANSPORT_ID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_TRANSPORT_MODE_Delete");
            this._dataAccess.AddParameter("TRANSPORT_ID", TRANSPORT_ID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> MTML_TRANSPORT_MODE_Update(System.Nullable<int> TRANSPORT_ID, string TRANSPORT_MODE, string TRANSPORT_DESC) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_TRANSPORT_MODE_Update");
            this._dataAccess.AddParameter("TRANSPORT_ID", TRANSPORT_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("TRANSPORT_MODE", TRANSPORT_MODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("TRANSPORT_DESC", TRANSPORT_DESC, ParameterDirection.Input);
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
