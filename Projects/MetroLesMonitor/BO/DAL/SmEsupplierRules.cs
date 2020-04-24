namespace MetroLesMonitor.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    public partial class SmEsupplierRules : IDisposable
    {
        private DataAccess _dataAccess;

        public SmEsupplierRules()
        {
            this._dataAccess = new DataAccess();
        }

        public SmEsupplierRules(DataAccess _dataAccess)
        {
            this._dataAccess = (_dataAccess != null) ? _dataAccess : new DataAccess();
        }

        public virtual System.Data.DataSet SM_ESUPPLIER_RULES_Select_All()
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_ESUPPLIER_RULES_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_ESUPPLIER_RULES_Select_One(System.Nullable<int> RULEID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_ESUPPLIER_RULES_Select_One");
            this._dataAccess.AddParameter("RULEID", RULEID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_ESUPPLIER_RULES_Select_By_RuleCode(string RULE_CODE)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_ESUPPLIER_RULES_Select_By_RuleCode");
            this._dataAccess.AddParameter("RULE_CODE", RULE_CODE, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual void SM_ESUPPLIER_RULES_Insert(System.Nullable<int> RULEID, string RULE_NUMBER, string DOC_TYPE, string RULE_CODE, string RULE_DESC, string RULE_COMMENTS)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_ESUPPLIER_RULES_Insert");
            this._dataAccess.AddParameter("RULEID", RULEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_NUMBER", RULE_NUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_TYPE", DOC_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_CODE", RULE_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_DESC", RULE_DESC, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_COMMENTS", RULE_COMMENTS, ParameterDirection.Input);
            this._dataAccess.ExecuteNonQuery();
        }

        public virtual System.Nullable<int> SM_ESUPPLIER_RULES_Delete(System.Nullable<int> RULEID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_ESUPPLIER_RULES_Delete");
            this._dataAccess.AddParameter("RULEID", RULEID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_ESUPPLIER_RULES_Update(System.Nullable<int> RULEID, string RULE_NUMBER, string DOC_TYPE, string RULE_CODE, string RULE_DESC, string RULE_COMMENTS)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_ESUPPLIER_RULES_Update");
            this._dataAccess.AddParameter("RULEID", RULEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_NUMBER", RULE_NUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_TYPE", DOC_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_CODE", RULE_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_DESC", RULE_DESC, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_COMMENTS", RULE_COMMENTS, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual void Dispose()
        {
            if ((this._dataAccess != null))
            {
                this._dataAccess.Dispose();
            }
        }

        public virtual System.Data.DataSet Select_UnAssigned_ESupplierRules(string DOCFORMATID)
        {
            string cSQL = "SELECT * FROM SM_ESUPPLIER_RULES WHERE RULEID NOT IN (SELECT RULE_ID FROM SM_DOCUMENTFORMAT_RULES WHERE DOCFORMATID=@DOCFORMATID)";
            this._dataAccess.CreateSQLCommand(cSQL);
            this._dataAccess.AddParameter("DOCFORMATID", DOCFORMATID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
    }
}
