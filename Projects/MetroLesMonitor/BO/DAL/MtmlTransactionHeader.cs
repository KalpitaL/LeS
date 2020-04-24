namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class MtmlTransactionHeader : IDisposable {
        
        private DataAccess _dataAccess;
        
        public MtmlTransactionHeader() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet MTML_TRANSACTION_HEADER_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_MTML_TRANSACTION_HEADER_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet MTML_TRANSACTION_HEADER_Select_One(System.Nullable<System.Guid> MTMLDOCID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_TRANSACTION_HEADER_Select_One");
            this._dataAccess.AddParameter("MTMLDOCID", MTMLDOCID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<System.Guid> MTML_TRANSACTION_HEADER_Insert(string CONTROLREFERENCE, string IDENTIFIER, string PREPARATIONDATE, string PREPARATIONTIME, string RECIPEINT, string RECIPIENTCODEQUALIFIER, string SENDER, string SENDERCODEQUALIFIER, string VERSIONNUMBER, string ReferenceNumber, string DOCType) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_TRANSACTION_HEADER_Insert");
            this._dataAccess.AddParameter("CONTROLREFERENCE", CONTROLREFERENCE, ParameterDirection.Input);
            this._dataAccess.AddParameter("IDENTIFIER", IDENTIFIER, ParameterDirection.Input);
            this._dataAccess.AddParameter("PREPARATIONDATE", PREPARATIONDATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PREPARATIONTIME", PREPARATIONTIME, ParameterDirection.Input);
            this._dataAccess.AddParameter("RECIPEINT", RECIPEINT, ParameterDirection.Input);
            this._dataAccess.AddParameter("RECIPIENTCODEQUALIFIER", RECIPIENTCODEQUALIFIER, ParameterDirection.Input);
            this._dataAccess.AddParameter("SENDER", SENDER, ParameterDirection.Input);
            this._dataAccess.AddParameter("SENDERCODEQUALIFIER", SENDERCODEQUALIFIER, ParameterDirection.Input);
            this._dataAccess.AddParameter("VERSIONNUMBER", VERSIONNUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ReferenceNumber", ReferenceNumber, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOCType", DOCType, ParameterDirection.Input);
            Guid value = ((Guid)(this._dataAccess.ExecuteScalar()));
            return value;
        }
        
        public virtual System.Nullable<int> MTML_TRANSACTION_HEADER_Delete(System.Nullable<System.Guid> MTMLDOCID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_TRANSACTION_HEADER_Delete");
            this._dataAccess.AddParameter("MTMLDOCID", MTMLDOCID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> MTML_TRANSACTION_HEADER_Update(System.Nullable<System.Guid> MTMLDOCID, string CONTROLREFERENCE, string IDENTIFIER, string PREPARATIONDATE, string PREPARATIONTIME, string RECIPEINT, string RECIPIENTCODEQUALIFIER, string SENDER, string SENDERCODEQUALIFIER, string VERSIONNUMBER, string ReferenceNumber, string DOCType, System.Nullable<int> AUTOID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_TRANSACTION_HEADER_Update");
            this._dataAccess.AddParameter("MTMLDOCID", MTMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONTROLREFERENCE", CONTROLREFERENCE, ParameterDirection.Input);
            this._dataAccess.AddParameter("IDENTIFIER", IDENTIFIER, ParameterDirection.Input);
            this._dataAccess.AddParameter("PREPARATIONDATE", PREPARATIONDATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PREPARATIONTIME", PREPARATIONTIME, ParameterDirection.Input);
            this._dataAccess.AddParameter("RECIPEINT", RECIPEINT, ParameterDirection.Input);
            this._dataAccess.AddParameter("RECIPIENTCODEQUALIFIER", RECIPIENTCODEQUALIFIER, ParameterDirection.Input);
            this._dataAccess.AddParameter("SENDER", SENDER, ParameterDirection.Input);
            this._dataAccess.AddParameter("SENDERCODEQUALIFIER", SENDERCODEQUALIFIER, ParameterDirection.Input);
            this._dataAccess.AddParameter("VERSIONNUMBER", VERSIONNUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ReferenceNumber", ReferenceNumber, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOCType", DOCType, ParameterDirection.Input);
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
