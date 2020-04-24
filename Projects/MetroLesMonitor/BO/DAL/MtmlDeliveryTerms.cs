namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class MtmlDeliveryTerms : IDisposable {
        
        private DataAccess _dataAccess;
        
        public MtmlDeliveryTerms() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet MTML_DELIVERY_TERMS_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DELIVERY_TERMS_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> MTML_DELIVERY_TERMS_Insert(string DEL_TERM_CODE, string DEL_TERM_DESC) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DELIVERY_TERMS_Insert");
            this._dataAccess.AddParameter("DEL_TERM_CODE", DEL_TERM_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEL_TERM_DESC", DEL_TERM_DESC, ParameterDirection.Input);
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
