namespace MetroLesMonitor.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;


    public partial class SmDefaultRules : IDisposable
    {

        private DataAccess _dataAccess;

        public SmDefaultRules()
        {
            this._dataAccess = new DataAccess();
        }

        public SmDefaultRules(DataAccess _dataAccess)
        {
            // TODO: Complete member initialization
            this._dataAccess = _dataAccess;
        }

        public virtual System.Data.DataSet SM_DEFAULT_RULES_Select_All()
        {
            //this._dataAccess.CreateProcedureCommand("sp_SM_DEFAULT_RULES_Select_All"); 
            this._dataAccess.CreateSQLCommand("SELECT * FROM SMV_BUYER_SUPPLIER_DEFAULT_RULES_UNIQUE");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_DEFAULT_RULES_Select_One(System.Nullable<int> DEF_ID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_DEFAULT_RULES_Select_One");
            this._dataAccess.AddParameter("DEF_ID", DEF_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Nullable<int> SM_DEFAULT_RULES_Insert(System.Nullable<int> ADDRESSID, string GROUP_FORMAT, System.Nullable<int> RULE_ID, string RULE_VALUE)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_DEFAULT_RULES_Insert");
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUP_FORMAT", GROUP_FORMAT, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_ID", RULE_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_VALUE", RULE_VALUE, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_DEFAULT_RULES_Delete(System.Nullable<int> DEF_ID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_DEFAULT_RULES_Delete");
            this._dataAccess.AddParameter("DEF_ID", DEF_ID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_DEFAULT_RULES_Update(System.Nullable<int> DEF_ID, System.Nullable<int> ADDRESSID, string GROUP_FORMAT, System.Nullable<int> RULE_ID, string RULE_VALUE)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_DEFAULT_RULES_Update");
            this._dataAccess.AddParameter("DEF_ID", DEF_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUP_FORMAT", GROUP_FORMAT, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_ID", RULE_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_VALUE", RULE_VALUE, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Data.DataSet SM_DEFAULT_RULES_Select_By_AddressID(System.Nullable<int> ADDRESSID, string GROUP_FORMAT)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_DEFAULT_RULES_Select_By_AddressID");
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUP_FORMAT", GROUP_FORMAT, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_DEFAULT_RULES_Select_By_Address_GroupFormat(System.Nullable<int> ADDRESSID, string GROUP_FORMAT)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SM_DEFAULT_RULES WHERE ADDRESSID = @ADDRESSID AND GROUP_FORMAT=@GROUP_FORMAT AND ISNULL(RULE_ID,0) <> 0 ");
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUP_FORMAT", GROUP_FORMAT, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_DEFAULT_RULES_Select_By_AddressID(System.Nullable<int> ADDRESSID)
        {
            //this._dataAccess.CreateSQLCommand("SELECT * FROM SM_DEFAULT_RULES WHERE ADDRESSID = @ADDRESSID AND ISNULL(RULE_ID,0) <> 0 ");
            this._dataAccess.CreateSQLCommand("SELECT * FROM SMV_BUYER_SUPPLIER_DEFAULT_RULES WHERE ADDRESSID = @ADDRESSID AND ISNULL(RULE_ID,0) <> 0  ORDER BY RULE_CODE"); //changed by kalpita on 09/12/2017
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_DEFAULT_RULES_Select_All_By_Address_GroupFormat(System.Nullable<int> ADDRESSID, string GROUP_FORMAT)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SM_DEFAULT_RULES WHERE ADDRESSID = @ADDRESSID AND GROUP_FORMAT=@GROUP_FORMAT");
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUP_FORMAT", GROUP_FORMAT, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet select_SM_DOC_FORMAT(string ADDR_TYPE)
        {
            this._dataAccess.CreateProcedureCommand("select_SM_DOC_FORMAT");
            this._dataAccess.AddParameter("ADDR_TYPE", ADDR_TYPE, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet select_SM_DOC_FORMAT_All()
        {
            this._dataAccess.CreateProcedureCommand("select_SM_DOC_FORMAT_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual void Dispose()
        {
            if ((this._dataAccess != null))
            {
                this._dataAccess.Dispose();
            }
        }

        public virtual System.Data.DataSet SM_DEFAULT_RULES_Select_By_AddressID_GroupFormat_RuleCode(System.Nullable<int> ADDRESSID, string GROUP_FORMAT, int RULE_ID)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SMV_BUYER_SUPPLIER_DEFAULT_RULES WHERE ADDRESSID=@ADDRESSID AND GROUP_FORMAT=@GROUP_FORMAT AND RULE_ID=@RULE_ID ORDER BY RULE_CODE");
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUP_FORMAT", GROUP_FORMAT, ParameterDirection.Input);
            this._dataAccess.AddParameter("RULE_ID", RULE_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet GET_ESUPPLIER_RULES_LIST_Without_AddressID(int ADDRESSID, string GROUP_FORMAT)
        {
            string cSQL = "SELECT * FROM SM_ESUPPLIER_RULES ";
            if (ADDRESSID > 0 && !string.IsNullOrEmpty(GROUP_FORMAT))
            {
                cSQL += " WHERE RULEID NOT IN (SELECT RULE_ID FROM SMV_BUYER_SUPPLIER_DEFAULT_RULES WHERE ADDRESSID=@ADDRESSID AND GROUP_FORMAT=@GROUP_FORMAT)";
            }
            this._dataAccess.CreateSQLCommand(cSQL+" ORDER BY RULEID");
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUP_FORMAT", GROUP_FORMAT, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet GetDefaultRules_By_DefIDs(string DEFID)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SM_DEFAULT_RULES WHERE DEF_ID IN (" + DEFID + ")");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }


        public virtual System.Data.DataSet SM_DEFAULT_RULES_By_AddressID_Format(System.Nullable<int> ADDRESSID, string GROUP_FORMAT)
        {
            string cSQL = "SELECT * FROM SMV_BUYER_SUPPLIER_DEFAULT_RULES WHERE ADDRESSID=@ADDRESSID ";
            if (!string.IsNullOrEmpty(GROUP_FORMAT))
            {
                cSQL += " AND GROUP_FORMAT = @GROUP_FORMAT ";
            }
            cSQL += " AND ISNULL(RULE_CODE,'') <> '' ORDER BY RULE_NUMBER";
            this._dataAccess.CreateSQLCommand(cSQL);
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUP_FORMAT", GROUP_FORMAT, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

    }
}