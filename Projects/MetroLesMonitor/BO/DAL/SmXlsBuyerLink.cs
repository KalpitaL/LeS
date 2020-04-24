namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmXlsBuyerLink : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmXlsBuyerLink() {
            this._dataAccess = new DataAccess();
        }

        public SmXlsBuyerLink(DataAccess _dataAccess)
        {
            // TODO: Complete member initialization
            this._dataAccess = _dataAccess;
        }


        public virtual System.Data.DataSet Select_SM_XLS_BUYER_LINKs_By_EXCEL_MAPID(System.Nullable<int> EXCEL_MAPID)
        {
            this._dataAccess.CreateProcedureCommand("sp_Select_SM_XLS_BUYER_LINKs_By_EXCEL_MAPID");
            this._dataAccess.AddParameter("EXCEL_MAPID", EXCEL_MAPID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet Select_SM_XLS_BUYER_LINKs_By_LINKID(System.Nullable<int> LINKID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_BUYER_LINK_Select_By_LINKID");
            this._dataAccess.AddParameter("BUYER_SUPP_LINKID", LINKID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_XLS_BUYER_LINK_Select_All()
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_BUYER_LINK_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet Select_All_DocTypes()//added  by kalpita on 23/05/2017
        {
            this._dataAccess.CreateProcedureCommand("sp_Select_All_DocTypes");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_XLS_BUYER_LINK_Select_One(System.Nullable<int> XLS_BUYER_MAPID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_BUYER_LINK_Select_One");
            this._dataAccess.AddParameter("XLS_BUYER_MAPID", XLS_BUYER_MAPID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual int SM_XLS_BUYER_LINK_Insert(System.Nullable<int> XLS_BUYER_MAPID, System.Nullable<int> EXCEL_MAPID, System.Nullable<int> BUYER_SUPP_LINKID,
            System.Nullable<int> BUYERID, System.Nullable<int> SUPPLIERID, string MAP_CELL1, string MAP_CELL1_VAL1, string MAP_CELL1_VAL2, string MAP_CELL2,
            string MAP_CELL2_VAL, string MAP_CELL_NODISC, string MAP_CELL_NODISC_VAL, string DOC_TYPE, string FORMAT_MAP_CODE)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_BUYER_LINK_Insert_New");
            this._dataAccess.AddParameter("XLS_BUYER_MAPID", XLS_BUYER_MAPID, ParameterDirection.Output);
            this._dataAccess.AddParameter("EXCEL_MAPID", EXCEL_MAPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_SUPP_LINKID", BUYER_SUPP_LINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYERID", BUYERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAP_CELL1", MAP_CELL1, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAP_CELL1_VAL1", MAP_CELL1_VAL1, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAP_CELL1_VAL2", MAP_CELL1_VAL2, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAP_CELL2", MAP_CELL2, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAP_CELL2_VAL", MAP_CELL2_VAL, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAP_CELL_NODISC", MAP_CELL_NODISC, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAP_CELL_NODISC_VAL", MAP_CELL_NODISC_VAL, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_TYPE", DOC_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("FORMAT_MAP_CODE", FORMAT_MAP_CODE, ParameterDirection.Input);
            this._dataAccess.ExecuteNonQuery();
            int Value = convert.ToInt(this._dataAccess.Command.Parameters["XLS_BUYER_MAPID"].Value);
            return Value;
        }

        public virtual System.Nullable<int> SM_XLS_BUYER_LINK_Delete(System.Nullable<int> XLS_BUYER_MAPID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_BUYER_LINK_Delete");
            this._dataAccess.AddParameter("XLS_BUYER_MAPID", XLS_BUYER_MAPID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_XLS_BUYER_LINK_Update(System.Nullable<int> XLS_BUYER_MAPID, System.Nullable<int> EXCEL_MAPID,
            System.Nullable<int> BUYER_SUPP_LINKID, System.Nullable<int> BUYERID, System.Nullable<int> SUPPLIERID, string MAP_CELL1, string MAP_CELL1_VAL1,
            string MAP_CELL1_VAL2, string MAP_CELL2, string MAP_CELL2_VAL, string MAP_CELL_NODISC, string MAP_CELL_NODISC_VAL, string DOC_TYPE, string FORMAT_MAP_CODE)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_XLS_BUYER_LINK_Update_New");
            this._dataAccess.AddParameter("XLS_BUYER_MAPID", XLS_BUYER_MAPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXCEL_MAPID", EXCEL_MAPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_SUPP_LINKID", BUYER_SUPP_LINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYERID", BUYERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAP_CELL1", MAP_CELL1, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAP_CELL1_VAL1", MAP_CELL1_VAL1, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAP_CELL1_VAL2", MAP_CELL1_VAL2, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAP_CELL2", MAP_CELL2, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAP_CELL2_VAL", MAP_CELL2_VAL, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAP_CELL_NODISC", MAP_CELL_NODISC, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAP_CELL_NODISC_VAL", MAP_CELL_NODISC_VAL, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_TYPE", DOC_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("FORMAT_MAP_CODE", FORMAT_MAP_CODE, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual void Dispose() {
            if ((this._dataAccess != null)) {
                this._dataAccess.Dispose();
            }
        }

        //ADDED BY KALPITA ON 04/01/2018
        public virtual System.Data.DataSet SM_XLS_BUYER_LINK_AddressId(string ADDRESSID,string ADDRTYPE)
        {
            string cSQL = "";
            if (ADDRTYPE.ToUpper() == "SUPPLIER")
            {
              //  cSQL = "SELECT * FROM SMV_XLS_MAPPING WHERE SUPPLIERID = @ADDRESSID";
                cSQL = " SELECT SM_XLS_BUYER_LINK.*,SM_ADDRESS.ADDR_CODE AS 'BUYER_CODE',SM_ADDRESS.ADDR_NAME AS 'BUYER_NAME' FROM SM_XLS_BUYER_LINK INNER JOIN SM_ADDRESS ON SM_XLS_BUYER_LINK.BUYERID= SM_ADDRESS.ADDRESSID WHERE BUYERID IN (SELECT BUYERID FROM SM_BUYER_SUPPLIER_LINK WHERE SUPPLIERID = @ADDRESSID)";
            }
            else if  (ADDRTYPE.ToUpper() == "WIZ")
            {
                cSQL = "SELECT * FROM SMV_XLS_MAPPING WHERE BUYERID = @ADDRESSID";
            }
            else if (ADDRTYPE.ToUpper() == "BUYER")
            {
                cSQL = "SELECT * FROM SMV_XLS_MAPPING_NEW WHERE BUYERID = @ADDRESSID";
            }
            this._dataAccess.CreateSQLCommand(cSQL);
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        //ADDED BY KALPITA ON 22/02/2018
        public virtual System.Data.DataSet SM_XLS_BUYER_LINK_MapCode()
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SMV_XLS_MAPPING_NEW ORDER BY XLS_BUYER_MAPID");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual string GetFormatMapCode(string DOCTYPE, string FORMAT_MAPCODE)
        {
            string cFormatMapCode = "";
            string cSQL = "SELECT COUNT(FORMAT_MAP_CODE) AS COUNT FROM SM_XLS_BUYER_LINK WHERE DOC_TYPE = @DOCTYPE AND FORMAT_MAP_CODE LIKE '" + FORMAT_MAPCODE + "%'";
            this._dataAccess.CreateSQLCommand(cSQL);
            this._dataAccess.AddParameter("DOCTYPE", DOCTYPE, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            if (value != null && value.Tables.Count > 0 && value.Tables[0].Rows.Count > 0)
            {
                int nSeqno = Convert.ToInt32(value.Tables[0].Rows[0]["COUNT"]) + 1;
                cFormatMapCode = FORMAT_MAPCODE + "_" + nSeqno;
            }
            return cFormatMapCode;
        }


        public virtual System.Data.DataSet Select_SM_XLS_BUYER_LINKs_By_EXCEL_MAPID_BUYERID(System.Nullable<int> EXCEL_MAPID,int BUYERID)
        {
            this._dataAccess.CreateProcedureCommand("sp_Select_SM_XLS_BUYER_LINKs_By_EXCEL_MAPID_BUYERID");
            this._dataAccess.AddParameter("EXCEL_MAPID", EXCEL_MAPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYERID", BUYERID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet GetExcelBuyerLink_By_AddressID(string ADDRESSID)
        {
            string cSQL = "";
            cSQL = "SELECT * FROM SM_XLS_BUYER_LINK WHERE BUYERID = @ADDRESSID";
            this._dataAccess.CreateSQLCommand(cSQL);
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet GetExcelBuyerLinks_By_MAPID(string XLS_BUYER_MAPID)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SM_XLS_BUYER_LINK WHERE XLS_BUYER_MAPID IN (" + XLS_BUYER_MAPID + ")");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_XLS_BUYER_LINK_Select_All_New()
        {
            string cSQL="SELECT XLS_BUYER_MAPID,X.DOC_TYPE,BUYER_SUPP_LINKID,BUYERID,SUPPLIERID,GROUP_CODE,B.ADDR_CODE AS BUYER_CODE,B.ADDR_NAME AS BUYER_NAME,"+
                " S.ADDR_CODE AS SUPPLIER_CODE,S.ADDR_NAME AS SUPPLIER_NAME,MAP_CELL1,MAP_CELL1_VAL1,MAP_CELL1_VAL2,MAP_CELL2,MAP_CELL2_VAL,MAP_CELL_NODISC,"+
                " MAP_CELL_NODISC_VAL,M.EXCEL_MAPID,M.GROUP_ID FROM (SM_XLS_BUYER_LINK X JOIN SM_XLS_GROUP_MAPPING M ON X.EXCEL_MAPID = M.EXCEL_MAPID "+
                "JOIN SM_BUYER_SUPPLIER_GROUPS G ON M.GROUP_ID = G.GROUP_ID JOIN SM_ADDRESS B ON B.ADDRESSID=X.BUYERID JOIN SM_ADDRESS S ON S.ADDRESSID=X.SUPPLIERID)";
            this._dataAccess.CreateSQLCommand("sp_SM_XLS_BUYER_LINK_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
    }
}
