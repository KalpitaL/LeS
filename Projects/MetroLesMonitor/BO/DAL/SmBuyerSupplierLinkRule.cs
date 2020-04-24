namespace MetroLesMonitor.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmBuyerSupplierLinkRule : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmBuyerSupplierLinkRule() {
            this._dataAccess = new DataAccess();
        }

        public SmBuyerSupplierLinkRule(DataAccess _dataAccess)
        {
            // TODO: Complete member initialization
            this._dataAccess = _dataAccess;
        }

        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_LINK_RULE_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_LINK_RULE_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_LINK_RULE_Select_One(System.Nullable<int> LINK_RULE_ID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_LINK_RULE_Select_One");
            this._dataAccess.AddParameter("LINK_RULE_ID", LINK_RULE_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SMV_BUYER_SUPPLIER_LINK_RULE_Select_By_LinkID(System.Nullable<int> LINKID)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SMV_BUYER_SUPPLIER_LINK_RULE WHERE LINKID=@LINKID ORDER BY RULE_NUMBER");
            this._dataAccess.AddParameter("LINKID", LINKID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet GET_ESUPPLIER_RULES_LIST_Without_LinkID(System.Nullable<int> LINKID)
        {
            this._dataAccess.CreateSQLCommand("select * from SM_ESUPPLIER_RULES where Ruleid not in (select Ruleid from SMV_BUYER_SUPPLIER_LINK_RULE WHERE LINKID=@LINKID)");
            this._dataAccess.AddParameter("LINKID", LINKID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual void DeleteLinkRule(System.Nullable<int> LINKID)
        {
            this._dataAccess.CreateSQLCommand("Delete from SM_BUYER_SUPPLIER_LINK_RULE where Linkid = " + LINKID );
            this._dataAccess.ExecuteNonQuery();
        }

        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_Save_LINK_RULE_Select_By_LinkID(System.Nullable<int> LINKID, System.Nullable<int> RULEID)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SM_BUYER_SUPPLIER_LINK_RULE WHERE LINKID = " + LINKID + " and RULEID =" + RULEID);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_BUYER_SUPPLIER_LINK_RULE_Insert(System.Nullable<int> LINKID, System.Nullable<int> RULEID, string RULE_VALUE, System.Nullable<System.DateTime> UPDATE_DATE, System.Nullable<int> INHERIT_RULE) {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_LINK_RULE_Insert");
            this._dataAccess.AddParameter("LINKID", LINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULEID", RULEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_VALUE", RULE_VALUE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("INHERIT_RULE", INHERIT_RULE, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_BUYER_SUPPLIER_LINK_RULE_Delete(System.Nullable<int> LINK_RULE_ID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_LINK_RULE_Delete");
            this._dataAccess.AddParameter("LINK_RULE_ID", LINK_RULE_ID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_BUYER_SUPPLIER_LINK_RULE_Update(System.Nullable<int> LINK_RULE_ID, System.Nullable<int> LINKID, System.Nullable<int> RULEID, string RULE_VALUE, System.Nullable<System.DateTime> UPDATE_DATE, System.Nullable<int> INHERIT_RULE) {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_LINK_RULE_Update");
            this._dataAccess.AddParameter("LINK_RULE_ID", LINK_RULE_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINKID", LINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULEID", RULEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_VALUE", RULE_VALUE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("INHERIT_RULE", INHERIT_RULE, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_LINK_RULE_Select_By_LinkIDRuleID(System.Nullable<int> LinkID, System.Nullable<int> RULE_ID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_LINK_RULE_Select_By_LinkIDRuleID"); //sp_SM_BUYER_SUPPLIER_RULES_Select_ByLinkRule
            this._dataAccess.AddParameter("LINKID", LinkID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULEID", RULE_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        
        public virtual void Dispose() {
            if ((this._dataAccess != null)) {
                this._dataAccess.Dispose();
            }
        }
    }
}
