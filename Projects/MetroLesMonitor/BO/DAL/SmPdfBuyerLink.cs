using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Xml;
        
namespace MetroLesMonitor.Dal
{

    public partial class SmPdfBuyerLink : IDisposable {

        public DataAccess _dataAccess;
        
        public SmPdfBuyerLink() {
            this._dataAccess = new DataAccess();
        }

        public SmPdfBuyerLink(DataAccess _dataAccess)
        {
            this._dataAccess = (_dataAccess != null) ? _dataAccess : new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_PDF_BUYER_LINK_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_PDF_BUYER_LINK_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet Select_All_DocTypes()
        {
            this._dataAccess.CreateProcedureCommand("sp_Select_All_DocTypes");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet Select_SMV_PDF_BUYER_LINK_All()
        {
            this._dataAccess.CreateProcedureCommand("sp_Select_SMV_PDF_BUYER_LINK_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_PDF_BUYER_LINK_Select_One(System.Nullable<int> MAP_ID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_PDF_BUYER_LINK_Select_One");
            this._dataAccess.AddParameter("MAP_ID", MAP_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_PDF_BUYER_LINK_Select_One_By_PDF_MAPID(System.Nullable<int> PDF_MAPID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_PDF_BUYER_LINK_Select_One_PDF_MAPID");
            this._dataAccess.AddParameter("PDF_MAPID", PDF_MAPID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual void SM_PDF_BUYER_LINK_Insert(System.Nullable<int> MAP_ID, System.Nullable<int> PDF_MAPID, System.Nullable<int> BUYER_SUPP_LINKID, System.Nullable<int> BUYERID, System.Nullable<int> SUPPLIERID, string DOC_TYPE, string MAPPING_1, string MAPPING_1_VALUE, string MAPPING_2, string MAPPING_2_VALUE, string MAPPING_3, string MAPPING_3_VALUE, string FORMAT_MAP_CODE)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_PDF_BUYER_LINK_Insert_New");
            this._dataAccess.AddParameter("MAP_ID", MAP_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("PDF_MAPID", PDF_MAPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_SUPP_LINKID", BUYER_SUPP_LINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYERID", BUYERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_TYPE", DOC_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAPPING_1", MAPPING_1, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAPPING_1_VALUE", MAPPING_1_VALUE, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAPPING_2", MAPPING_2, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAPPING_2_VALUE", MAPPING_2_VALUE, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAPPING_3", MAPPING_3, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAPPING_3_VALUE", MAPPING_3_VALUE, ParameterDirection.Input);
            this._dataAccess.AddParameter("FORMAT_MAP_CODE", FORMAT_MAP_CODE, ParameterDirection.Input);
            this._dataAccess.ExecuteNonQuery();
        }
        
        public virtual System.Nullable<int> SM_PDF_BUYER_LINK_Delete(System.Nullable<int> MAP_ID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_PDF_BUYER_LINK_Delete");
            this._dataAccess.AddParameter("MAP_ID", MAP_ID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_PDF_BUYER_LINK_Update(System.Nullable<int> MAP_ID, System.Nullable<int> PDF_MAPID, System.Nullable<int> BUYER_SUPP_LINKID, System.Nullable<int> BUYERID, System.Nullable<int> SUPPLIERID, string DOC_TYPE, string MAPPING_1, string MAPPING_1_VALUE, string MAPPING_2, string MAPPING_2_VALUE, string MAPPING_3, string MAPPING_3_VALUE, string FORMAT_MAP_CODE)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_PDF_BUYER_LINK_Update_New");
            this._dataAccess.AddParameter("MAP_ID", MAP_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("PDF_MAPID", PDF_MAPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_SUPP_LINKID", BUYER_SUPP_LINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYERID", BUYERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_TYPE", DOC_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAPPING_1", MAPPING_1, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAPPING_1_VALUE", MAPPING_1_VALUE, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAPPING_2", MAPPING_2, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAPPING_2_VALUE", MAPPING_2_VALUE, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAPPING_3", MAPPING_3, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAPPING_3_VALUE", MAPPING_3_VALUE, ParameterDirection.Input);
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
        public virtual System.Data.DataSet SM_PDF_BUYER_LINK_AddressId(string ADDRESSID, string ADDRTYPE)
        {
            string cSQL = "";
            if (ADDRTYPE.ToUpper() == "SUPPLIER")
            {
                cSQL = "SELECT SM_PDF_BUYER_LINK.*,SM_ADDRESS.ADDR_CODE AS 'BUYER_CODE',SM_ADDRESS.ADDR_NAME AS 'BUYER_NAME' FROM SM_PDF_BUYER_LINK INNER JOIN SM_ADDRESS ON SM_PDF_BUYER_LINK.BUYERID= SM_ADDRESS.ADDRESSID  WHERE BUYERID IN (SELECT BUYERID FROM SM_BUYER_SUPPLIER_LINK WHERE SUPPLIERID = @ADDRESSID)";
            }
            else if (ADDRTYPE.ToUpper() == "WIZ")
            {
                cSQL = "SELECT * FROM SMV_PDF_BUYER_LINK WHERE BUYERID = @ADDRESSID";
            }
            else if (ADDRTYPE.ToUpper() == "BUYER")
            {
                cSQL = "SELECT * FROM SMV_PDF_BUYER_LINK_NEW WHERE BUYERID = @ADDRESSID";
            }
            this._dataAccess.CreateSQLCommand(cSQL);
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual string GetFormatMapCode(string DOCTYPE, string FORMAT_MAPCODE)
        {
            string cFormatMapCode = "";
            string cSQL = "SELECT COUNT(FORMAT_MAP_CODE) AS COUNT FROM SM_PDF_BUYER_LINK WHERE DOC_TYPE = @DOCTYPE AND FORMAT_MAP_CODE LIKE '" + FORMAT_MAPCODE + "%'";
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

        public virtual System.Data.DataSet GetPdfBuyerLink_By_AddressID(string ADDRESSID)
        {
            string cSQL = "";
            cSQL = "SELECT * FROM SM_PDF_BUYER_LINK WHERE BUYERID = @ADDRESSID";
            this._dataAccess.CreateSQLCommand(cSQL);
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet GetPDFBuyerLinks_By_MAPID(string MAP_ID)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SM_PDF_BUYER_LINK WHERE MAP_ID IN (" + MAP_ID + ")");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        //ADDED BY KALPITA ON 22/02/2018
        public virtual System.Data.DataSet SM_PDF_BUYER_LINK_MapCode()
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SMV_PDF_BUYER_LINK_NEW ORDER BY PDF_MAPID");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet Select_SMV_PDF_BUYER_LINK_All_New()
        {
            string cSQL = "SELECT G.GROUP_ID,GROUP_CODE,B.ADDR_CODE AS BUYER_CODE,B.ADDR_NAME AS BUYER_NAME,S.ADDR_CODE AS SUPPLIER_CODE," +
                    " S.ADDR_NAME AS SUPPLIER_NAME,P.DOC_TYPE,P.MAPPING_1,P.MAPPING_1_VALUE,P.MAPPING_2,P.MAPPING_2_VALUE,P.MAPPING_3,P.MAPPING_3_VALUE," +
                    " P.MAP_ID,P.PDF_MAPID,P.BUYER_SUPP_LINKID,P.BUYERID,P.SUPPLIERID FROM SM_PDF_BUYER_LINK P JOIN SM_PDF_MAPPING M " +
                    " ON P.PDF_MAPID = M.PDF_MAPID JOIN SM_BUYER_SUPPLIER_GROUPS G" +
                    " ON M.GROUPID = G.GROUP_ID JOIN SM_ADDRESS B ON B.ADDRESSID=P.BUYERID JOIN SM_ADDRESS S ON S.ADDRESSID=P.SUPPLIERID";                   
            this._dataAccess.CreateSQLCommand(cSQL);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

    }
}
