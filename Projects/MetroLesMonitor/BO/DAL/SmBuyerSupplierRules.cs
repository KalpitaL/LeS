namespace MetroLesMonitor.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmBuyerSupplierRules : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmBuyerSupplierRules() {
            this._dataAccess = new DataAccess();
        }

        public SmBuyerSupplierRules(DataAccess _dataAccess)
        {
            // TODO: Complete member initialization
            this._dataAccess = _dataAccess;
        }
        
        public virtual System.Data.DataSet Select_SM_BUYER_SUPPLIER_RULESs_for_Group(System.Nullable<int> GROUP_ID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_RULES_Select_By_Group");
            this._dataAccess.AddParameter("GROUP_ID", GROUP_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet Select_SM_BUYER_SUPPLIER_RULESs_By_GROUP_ID(System.Nullable<int> GROUP_ID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_RULES_Select_By_GroupID");
            this._dataAccess.AddParameter("GROUP_ID", GROUP_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet Select_SM_BUYER_SUPPLIER_RULESs_By_RULE_ID(System.Nullable<int> RULE_ID) {
            this._dataAccess.CreateProcedureCommand("sp_Select_SM_BUYER_SUPPLIER_RULESs_By_RULE_ID");
            this._dataAccess.AddParameter("RULE_ID", RULE_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_RULES_Select_Unlinked_Rules(System.Nullable<int> GROUP_ID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_ESUPPLIER_RULES_Select_Unlinked_Rules");
            this._dataAccess.AddParameter("GROUP_ID", GROUP_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_RULES_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_RULES_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_RULES_Select_One(System.Nullable<int> GROUP_RULE_ID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_RULES_Select_One");
            this._dataAccess.AddParameter("GROUP_RULE_ID", GROUP_RULE_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_RULES_Select_ByGroupRule(int GROUP_ID, int RULE_ID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_RULES_Select_ByGroupRule");
            this._dataAccess.AddParameter("GROUP_ID", GROUP_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_ID", RULE_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_BUYER_SUPPLIER_RULES_Insert(System.Nullable<int> GROUP_ID, System.Nullable<int> RULE_ID, string RULE_VALUE) {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_RULES_Insert");
            this._dataAccess.AddParameter("GROUP_ID", GROUP_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_ID", RULE_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_VALUE", RULE_VALUE, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_BUYER_SUPPLIER_RULES_Delete(System.Nullable<int> GROUP_RULE_ID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_RULES_Delete");
            this._dataAccess.AddParameter("GROUP_RULE_ID", GROUP_RULE_ID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_BUYER_SUPPLIER_RULES_Update(System.Nullable<int> GROUP_RULE_ID, System.Nullable<int> GROUP_ID, System.Nullable<int> RULE_ID, string RULE_VALUE) {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_RULES_Update");
            this._dataAccess.AddParameter("GROUP_RULE_ID", GROUP_RULE_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUP_ID", GROUP_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_ID", RULE_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_VALUE", RULE_VALUE, ParameterDirection.Input);
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
