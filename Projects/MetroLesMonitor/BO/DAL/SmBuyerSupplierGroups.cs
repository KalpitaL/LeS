namespace MetroLesMonitor.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmBuyerSupplierGroups : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmBuyerSupplierGroups() {
            this._dataAccess = new DataAccess();
        }

        public SmBuyerSupplierGroups(DataAccess _dataAccess)
        {
            // TODO: Complete member initialization
            this._dataAccess = _dataAccess;
        }

        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_GROUPS_Select_One(string GROUP_CODE)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_GROUPS_Select_By_GroupCode");
            this._dataAccess.AddParameter("GROUP_CODE", GROUP_CODE, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_GROUPS_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_GROUPS_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_GROUPS_Select_All_Excel()
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_GROUPS_Select_All_Excel");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_GROUPS_Select_Excel_Only()
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SMV_BUYER_SUPPLIER_EXCEL_GROUPS ORDER BY GROUP_CODE");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_GROUPS_Select_PDF_Only()
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SMV_BUYER_SUPPLIER_PDF_GROUPS ORDER BY GROUP_CODE");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_GROUPS_Select_One(System.Nullable<int> GROUP_ID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_GROUPS_Select_One");
            this._dataAccess.AddParameter("GROUP_ID", GROUP_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }


        public virtual System.Nullable<int> SM_BUYER_SUPPLIER_GROUPS_Insert(System.Nullable<int> GROUP_ID, string GROUP_CODE, string GROUP_DESC, string BUYER_FORMAT, string SUPPLIER_FORMAT, string BUYER_EXPORT_FORMAT,string SUPPLIER_EXPORT_FORMAT)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_GROUPS_Insert");
            this._dataAccess.AddParameter("GROUP_ID", 0, ParameterDirection.Output);
            this._dataAccess.AddParameter("GROUP_CODE", GROUP_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUP_DESC", GROUP_DESC, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_FORMAT", BUYER_FORMAT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_FORMAT", SUPPLIER_FORMAT, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_EXPORT_FORMAT", BUYER_EXPORT_FORMAT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_EXPORT_FORMAT", SUPPLIER_EXPORT_FORMAT, ParameterDirection.Input);
            int value = (int)this._dataAccess.ExecuteNonQuery();
            value = convert.ToInt(this._dataAccess.Command.Parameters["GROUP_ID"].Value);
            return value;
        }

        public virtual System.Nullable<int> SM_BUYER_SUPPLIER_GROUPS_Delete(System.Nullable<int> GROUP_ID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_GROUPS_Delete");
            this._dataAccess.AddParameter("GROUP_ID", GROUP_ID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_BUYER_SUPPLIER_GROUPS_Update(System.Nullable<int> GROUP_ID, string GROUP_CODE, string GROUP_DESC, string BUYER_FORMAT, string SUPPLIER_FORMAT, string BUYER_EXPORT_FORMAT, string SUPPLIER_EXPORT_FORMAT)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_GROUPS_Update");
            this._dataAccess.AddParameter("GROUP_ID", GROUP_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUP_CODE", GROUP_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUP_DESC", GROUP_DESC, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_FORMAT", BUYER_FORMAT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_FORMAT", SUPPLIER_FORMAT, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_EXPORT_FORMAT", BUYER_EXPORT_FORMAT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_EXPORT_FORMAT", SUPPLIER_EXPORT_FORMAT, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual void Dispose() {
            if ((this._dataAccess != null)) {
                this._dataAccess.Dispose();
            }
        }

        //added by kalpita on 21/11/2017
        public virtual  System.Data.DataSet SM_BUYER_SUPPLIER_GROUPS_AddrType(string cAddrType)
        {
            string cSQL = "";
            if (cAddrType.ToUpper() =="BUYER")
            {
                cSQL = "SELECT DISTINCT BUYER_FORMAT AS 'FORMAT' FROM SM_BUYER_SUPPLIER_GROUPS WHERE BUYER_FORMAT IS NOT NULL ORDER BY BUYER_FORMAT";
            }
            else
            {
                cSQL = "SELECT DISTINCT SUPPLIER_FORMAT  AS 'FORMAT'  FROM SM_BUYER_SUPPLIER_GROUPS WHERE SUPPLIER_FORMAT IS NOT NULL ORDER BY SUPPLIER_FORMAT";
            }
            this._dataAccess.CreateSQLCommand(cSQL);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

    }
}
