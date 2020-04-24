using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Xml;
        
namespace MetroLesMonitor.Dal {

    public partial class SmDocumentformatRules : IDisposable {

        public DataAccess _dataAccess;
        
        public SmDocumentformatRules() {
            this._dataAccess = new DataAccess();
        }

        public SmDocumentformatRules(DataAccess _dataAccess)
        {
            this._dataAccess = (_dataAccess != null) ? _dataAccess : new DataAccess();
        }
        
        
        public virtual System.Data.DataSet SM_DOCUMENTFORMAT_RULES_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_DOCUMENTFORMAT_RULES_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_DOCUMENTFORMAT_RULES_Select_One(System.Nullable<int> DOCUMENTFORMAT_RULEID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_DOCUMENTFORMAT_RULES_Select_One");
            this._dataAccess.AddParameter("DOCUMENTFORMAT_RULEID", DOCUMENTFORMAT_RULEID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_DOCUMENTFORMAT_RULES_Insert(System.Nullable<int> DOCFORMATID, System.Nullable<int> RULE_ID, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<System.DateTime> UPDATE_DATE, string RULE_VALUE) {
            this._dataAccess.CreateProcedureCommand("sp_SM_DOCUMENTFORMAT_RULES_Insert");
            this._dataAccess.AddParameter("DOCFORMATID", DOCFORMATID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_ID", RULE_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_VALUE", RULE_VALUE, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_DOCUMENTFORMAT_RULES_Delete(System.Nullable<int> DOCUMENTFORMAT_RULEID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_DOCUMENTFORMAT_RULES_Delete");
            this._dataAccess.AddParameter("DOCUMENTFORMAT_RULEID", DOCUMENTFORMAT_RULEID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_DOCUMENTFORMAT_RULES_Update(System.Nullable<int> DOCUMENTFORMAT_RULEID, System.Nullable<int> DOCFORMATID, System.Nullable<int> RULE_ID, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<System.DateTime> UPDATE_DATE, string RULE_VALUE) {
            this._dataAccess.CreateProcedureCommand("sp_SM_DOCUMENTFORMAT_RULES_Update");
            this._dataAccess.AddParameter("DOCUMENTFORMAT_RULEID", DOCUMENTFORMAT_RULEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOCFORMATID", DOCFORMATID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_ID", RULE_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_VALUE", RULE_VALUE, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual void Dispose() {
            if ((this._dataAccess != null)) {
                this._dataAccess.Dispose();
            }
        }

        public virtual System.Data.DataSet SM_DOCUMENTFORMAT_RULES_BY_DOCFORMATID(int DOCFORMATID)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SMV_DOCUMENTFORMAT_RULES WHERE DOCFORMATID=@DOCFORMATID");
            this._dataAccess.AddParameter("DOCFORMATID", DOCFORMATID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_DOCUMENTFORMAT_RULES_BY_DOCFORMAT(string DOCUMENT_FORMAT)
        {
            string cSQL = "SELECT DOCUMENTFORMAT_RULEID,SM_DOCUMENTFORMAT_RULES.DOCFORMATID,RULE_ID,SM_DOCUMENTFORMAT_RULES.CREATED_DATE,SM_DOCUMENTFORMAT_RULES.UPDATE_DATE,RULE_VALUE FROM " +
                "SM_DOCUMENTFORMAT_RULES INNER JOIN SM_DOCUMENT_FORMATS ON SM_DOCUMENTFORMAT_RULES.DOCFORMATID=SM_DOCUMENT_FORMATS.DOCFORMATID  WHERE DOCUMENT_FORMAT=@DOCUMENT_FORMAT";
            this._dataAccess.CreateSQLCommand(cSQL);
            this._dataAccess.AddParameter("DOCUMENT_FORMAT", DOCUMENT_FORMAT, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
    }
}
