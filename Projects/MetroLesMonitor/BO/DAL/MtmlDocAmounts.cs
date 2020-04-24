namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class MtmlDocAmounts : IDisposable {
        
        private DataAccess _dataAccess;
        
        public MtmlDocAmounts() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet MTML_DOC_AMOUNTS_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_AMOUNTS_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet MTML_DOC_AMOUNTS_Select_One(System.Nullable<System.Guid> AMOUNTID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_AMOUNTS_Select_One");
            this._dataAccess.AddParameter("AMOUNTID", AMOUNTID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<System.Guid> MTML_DOC_AMOUNTS_Insert(System.Nullable<System.Guid> MTMLDOCID, string QUALIFIER, System.Nullable<float> AMT_VALUE) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_AMOUNTS_Insert");
            this._dataAccess.AddParameter("MTMLDOCID", MTMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUALIFIER", QUALIFIER, ParameterDirection.Input);
            this._dataAccess.AddParameter("AMT_VALUE", AMT_VALUE, ParameterDirection.Input);
            Guid value = ((Guid)(this._dataAccess.ExecuteScalar()));
            return value;
        }
        
        public virtual System.Nullable<int> MTML_DOC_AMOUNTS_Delete(System.Nullable<System.Guid> AMOUNTID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_AMOUNTS_Delete");
            this._dataAccess.AddParameter("AMOUNTID", AMOUNTID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> MTML_DOC_AMOUNTS_Update(System.Nullable<System.Guid> AMOUNTID, System.Nullable<System.Guid> MTMLDOCID, string QUALIFIER, System.Nullable<float> AMT_VALUE, System.Nullable<int> AUTOID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_AMOUNTS_Update");
            this._dataAccess.AddParameter("AMOUNTID", AMOUNTID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MTMLDOCID", MTMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUALIFIER", QUALIFIER, ParameterDirection.Input);
            this._dataAccess.AddParameter("AMT_VALUE", AMT_VALUE, ParameterDirection.Input);
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
