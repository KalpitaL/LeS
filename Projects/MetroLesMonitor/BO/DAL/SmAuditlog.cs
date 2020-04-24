namespace MetroLesMonitor.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;


    public partial class SmAuditlog : IDisposable
    {

        private DataAccess _dataAccess;

        public SmAuditlog()
        {
            this._dataAccess = new DataAccess("AuditConnection");
        }

        public SmAuditlog(DataAccess _dataAccess)
        {
            // TODO: Complete member initialization
            this._dataAccess = _dataAccess;
        }

        public virtual System.Data.DataSet SM_AUDITLOG_Select_All()
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_AUDITLOG_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_AUDITLOG_Select_All(System.Nullable<System.DateTime> FromDate, System.Nullable<System.DateTime> ToDate)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_AUDITLOG_Select_All_By_Dates");
            this._dataAccess.AddParameter("FROM_DATE", FromDate, ParameterDirection.Input);
            this._dataAccess.AddParameter("TO_DATE", ToDate, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_AUDITLOG_Select_All(System.Nullable<System.DateTime> FromDate, System.Nullable<System.DateTime> ToDate,
           string FILTERCOND, int FROM, int TO)
        {
            string cWHERECOND = "";
            string csql = "SELECT LOGID,UPDATEDATE,MODULENAME,FILENAME,AUDITVALUE,KEYREF1,KEYREF2, LOGTYPE,BUYER_CODE, VENDOR_CODE, BUYER_ID,SUPPLIER_ID,DOCTYPE,SERVERNAME,BUYER_NAME,VENDOR_NAME,FILENAME2,FILENAME3  FROM " +
                " (SELECT LOGID,UPDATEDATE,MODULENAME,FILENAME,AUDITVALUE,KEYREF1,KEYREF2, LOGTYPE,BUYER.ADDR_CODE AS BUYER_CODE,  VENDOR.ADDR_CODE AS VENDOR_CODE, " +
                " BUYER_ID,   SUPPLIER_ID,DOCTYPE,SERVERNAME,BUYER.ADDR_NAME AS BUYER_NAME,VENDOR.ADDR_NAME AS VENDOR_NAME ,FILENAME2,FILENAME3,ROW_NUMBER() OVER (ORDER BY LOGID DESC) AS ROWNUMBER " +
                " FROM SM_AUDITLOG AU LEFT JOIN SM_ADDRESS BUYER  ON AU.BUYER_ID=BUYER.ADDRESSID LEFT JOIN SM_ADDRESS VENDOR ON AU.SUPPLIER_ID=VENDOR.ADDRESSID " +
                " WHERE (1=1) AND UPDATEDATE >= @FromDate AND UPDATEDATE <= @ToDate " + FILTERCOND + ") AS TEMP ";
            //  " WHERE ROWNUMBER  BETWEEN " + FROM + "  AND " + TO;
            _dataAccess.CreateSQLCommand(csql + " ORDER BY LOGID DESC");
            _dataAccess.AddParameter("FromDate", FromDate, ParameterDirection.Input);
            _dataAccess.AddParameter("ToDate", ToDate, ParameterDirection.Input);
            DataSet value = _dataAccess.ExecuteDataSet();
            return value;
        }

        #region Custom Paging

        public virtual System.Data.DataSet SM_AUDITLOG_Select(System.Nullable<System.DateTime> FromDate, System.Nullable<System.DateTime> ToDate,
         string FILTERCOND, int FROM, int TO, out int TotalRows)
        {
            TotalRows = 0; TotalRows = 0;
            string csql = "SELECT LOGID,CONVERT(VARCHAR(10), UPDATEDATE, 103)  AS UPDATE_DATE,MODULENAME,FILENAME,AUDITVALUE,KEYREF1,KEYREF2, LOGTYPE,BUYERCODE, SUPPLIERCODE, BUYER_ID,SUPPLIER_ID,DOCTYPE,SERVERNAME,FILENAME2,FILENAME3  FROM " +
                " (SELECT LOGID,UPDATEDATE,MODULENAME,FILENAME,AUDITVALUE,KEYREF1,KEYREF2, LOGTYPE,BUYERCODE, SUPPLIERCODE, " +
                " BUYER_ID,   SUPPLIER_ID,DOCTYPE,SERVERNAME,FILENAME2,FILENAME3,ROW_NUMBER() OVER (ORDER BY LOGID DESC) AS ROWNUMBER  FROM SM_AUDITLOG " +
                " WHERE  UPDATEDATE >= @FromDate AND UPDATEDATE <= @ToDate " + FILTERCOND + ") AS TEMP " +
                " WHERE ROWNUMBER  BETWEEN " + FROM + "  AND " + TO;
            //_dataAccess.CreateSQLCommand(csql + " ORDER BY LOGID DESC");
            _dataAccess.CreateSQLCommand(csql);
            _dataAccess.AddParameter("FromDate", FromDate, ParameterDirection.Input);
            _dataAccess.AddParameter("ToDate", ToDate, ParameterDirection.Input);
            DataSet value = _dataAccess.ExecuteDataSet();

            string csqlcnt = "SELECT COUNT_BIG(LOGID) as _COUNT  FROM SM_AUDITLOG  WHERE UPDATEDATE >= @FromDate AND UPDATEDATE <= @ToDate " + FILTERCOND;      
            _dataAccess.CreateSQLCommand(csqlcnt);
            _dataAccess.AddParameter("FromDate", FromDate, ParameterDirection.Input);
            _dataAccess.AddParameter("ToDate", ToDate, ParameterDirection.Input);
            DataSet ds = _dataAccess.ExecuteDataSet();
            if (GlobalTools.IsSafeDataSet(ds))
            {
                TotalRows = (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["_COUNT"]))) ? Convert.ToInt32(ds.Tables[0].Rows[0]["_COUNT"]) : 0;
            }
        
            return value;
        }

        public virtual System.Data.DataSet SM_AUDITLOG_Select_By_AddressID(int ADDRESSID, string ADDRTYPE, System.Nullable<System.DateTime> FromDate,
            System.Nullable<System.DateTime> ToDate, string FILTERCOND, int FROM, int TO, out int TotalRows)
        {
            string cSQL = "", cLogCnt = ""; TotalRows = 0;
            if (ADDRTYPE.ToUpper() == "BUYER")
            {
                cSQL = "SELECT LOGID,CONVERT(VARCHAR(10), UPDATEDATE, 103)  AS UPDATE_DATE,MODULENAME,FILENAME,AUDITVALUE,KEYREF1,KEYREF2, LOGTYPE,BUYERCODE, SUPPLIERCODE, BUYER_ID,SUPPLIER_ID,DOCTYPE,SERVERNAME,FILENAME2,FILENAME3  FROM " +
             " (SELECT LOGID,UPDATEDATE,MODULENAME,FILENAME,AUDITVALUE,KEYREF1,KEYREF2, LOGTYPE,BUYERCODE,  SUPPLIERCODE, " +
             " BUYER_ID,   SUPPLIER_ID,DOCTYPE,SERVERNAME,FILENAME2,FILENAME3,ROW_NUMBER() OVER (ORDER BY LOGID DESC) AS ROWNUMBER " +
             " FROM SM_AUDITLOG WHERE BUYER_ID =@ADDRESSID  AND UPDATEDATE >= @FromDate AND UPDATEDATE <= @ToDate " + FILTERCOND + ") AS TEMP " +
             "  WHERE ROWNUMBER BETWEEN " + FROM + "  AND " + TO;

                cLogCnt = "SELECT COUNT (LOGID) as _COUNT  FROM SM_AUDITLOG  WHERE BUYER_ID =@ADDRESSID AND UPDATEDATE >= @FromDate AND UPDATEDATE <= @ToDate " + FILTERCOND;
            }
            if (ADDRTYPE.ToUpper() == "SUPPLIER")
            {
                cSQL = "SELECT LOGID,CONVERT(VARCHAR(10), UPDATEDATE, 103)  AS UPDATE_DATE,MODULENAME,FILENAME,AUDITVALUE,KEYREF1,KEYREF2, LOGTYPE,BUYERCODE, SUPPLIERCODE, BUYER_ID,SUPPLIER_ID,DOCTYPE,SERVERNAME,FILENAME2,FILENAME3  FROM " +
           " (SELECT LOGID,UPDATEDATE,MODULENAME,FILENAME,AUDITVALUE,KEYREF1,KEYREF2, LOGTYPE,BUYERCODE,  SUPPLIERCODE, BUYER_ID,  SUPPLIER_ID,DOCTYPE,SERVERNAME,FILENAME2,FILENAME3,ROW_NUMBER() OVER (ORDER BY LOGID DESC) AS ROWNUMBER " +
           "  FROM SM_AUDITLOG WHERE SUPPLIER_ID =@ADDRESSID   AND UPDATEDATE >= @FromDate AND UPDATEDATE <= @ToDate " + FILTERCOND + ") AS TEMP " +
           "  WHERE ROWNUMBER  BETWEEN " + FROM + "  AND " + TO;
                cLogCnt = "SELECT COUNT (LOGID) as _COUNT FROM SM_AUDITLOG WHERE SUPPLIER_ID =@ADDRESSID AND UPDATEDATE >= @FromDate AND UPDATEDATE <= @ToDate " + FILTERCOND;
            }
            cSQL += "  ORDER BY UPDATEDATE DESC ";
            _dataAccess.CreateSQLCommand(cSQL);
            _dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            _dataAccess.AddParameter("FromDate", FromDate, ParameterDirection.Input);
            _dataAccess.AddParameter("ToDate", ToDate, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();

            //fetch auditlog count
            _dataAccess.CreateSQLCommand(cLogCnt);
            _dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            _dataAccess.AddParameter("FromDate", FromDate, ParameterDirection.Input);
            _dataAccess.AddParameter("ToDate", ToDate, ParameterDirection.Input);
            DataSet ds = _dataAccess.ExecuteDataSet();
            if (GlobalTools.IsSafeDataSet(ds))
            {
                TotalRows = Convert.ToInt32(ds.Tables[0].Rows[0]["_COUNT"]);
            }
            return value;
        }

        #region test
        public virtual System.Data.DataSet SM_AUDITLOG_Select_test(System.Nullable<System.DateTime> FromDate, System.Nullable<System.DateTime> ToDate,
       string FILTERCOND, int FROM, int TO, out int TotalRows)
        {
            TotalRows = 0; TotalRows = 0;
            string csql = "SELECT CONVERT(VARCHAR(10), UPDATEDATE, 103)  AS UPDATE_DATE,SERVERNAME,PROCESSOR_NAME,MODULENAME,DOCTYPE, LOGTYPE,BUYERCODE,SUPPLIERCODE,KEYREF2,AUDITVALUE,FILENAME," +
                " KEYREF1, LOGID, BUYER_ID,SUPPLIER_ID,FILENAME2,FILENAME3  FROM SM_AUDITLOG  WHERE  UPDATEDATE >= @FromDate AND UPDATEDATE <= @ToDate " + FILTERCOND + " ORDER BY LOGID DESC OFFSET (" + FROM + "-1) ROWS FETCH NEXT 100 ROWS ONLY";            
            _dataAccess.CreateSQLCommand(csql);
            _dataAccess.AddParameter("FromDate", FromDate, ParameterDirection.Input);
            _dataAccess.AddParameter("ToDate", ToDate, ParameterDirection.Input);
            DataSet value = _dataAccess.ExecuteDataSet();

            string csqlcnt = "SELECT COUNT_BIG(LOGID) as _COUNT  FROM SM_AUDITLOG  WHERE UPDATEDATE >= @FromDate AND UPDATEDATE <= @ToDate " + FILTERCOND;
            _dataAccess.CreateSQLCommand(csqlcnt);
            _dataAccess.AddParameter("FromDate", FromDate, ParameterDirection.Input);
            _dataAccess.AddParameter("ToDate", ToDate, ParameterDirection.Input);
            DataSet ds = _dataAccess.ExecuteDataSet();
            if (GlobalTools.IsSafeDataSet(ds))
            {
                TotalRows = (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["_COUNT"]))) ? Convert.ToInt32(ds.Tables[0].Rows[0]["_COUNT"]) : 0;
            }

            return value;
        }

        #endregion




        #endregion


        public virtual System.Data.DataSet SM_AUDITLOG_Select_One(System.Nullable<int> LOGID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_AUDITLOG_Select_One");
            this._dataAccess.AddParameter("LOGID", LOGID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_AUDITLOG_Select_By_AddressID(int ADDRESSID, string ADDRTYPE)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_AUDITLOG_Select_By_AddressID");
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_TYPE", ADDRTYPE, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_AUDITLOG_Select_By_BuyerSupplier(int BuyerID, int SupplierID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_AUDITLOG_Select_By_BuyerSupplier");
            this._dataAccess.AddParameter("BUYERID", BuyerID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERID", SupplierID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_AUDITLOG_Select_All_LogTypes()
        {
            this._dataAccess.CreateSQLCommand("SELECT NULL AS LOGTYPE FROM SM_AUDITLOG UNION SELECT DISTINCT LTRIM(RTRIM(LOGTYPE)) FROM SM_AUDITLOG ORDER BY LOGTYPE");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_AUDITLOG_Select_All_ByLinkCode(string BuyerLinkCode, string VendorLinkCode)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SM_AUDITLOG WHERE AuditValue LIKE '%(" + BuyerLinkCode + " - " + VendorLinkCode + ")%' ");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_AUDITLOG_Select_All_ByLinkCode_By_Days(string BuyerLinkCode, string VendorLinkCode, int Days)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SM_AUDITLOG WHERE AuditValue LIKE '%(" + BuyerLinkCode + " - " + VendorLinkCode + ")%' AND UpdateDate > convert(datetime, dateadd(d, -" + Days + ", getdate()), 120) ");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_AUDITLOG_Select_All_Modules()
        {
            this._dataAccess.CreateSQLCommand("SELECT NULL AS MODULENAME FROM SM_AUDITLOG UNION SELECT DISTINCT LTRIM(RTRIM(MODULENAME)) FROM SM_AUDITLOG ORDER BY MODULENAME");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual DataSet SM_AUDITLOG_Select_ErrorLogs()
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SMV_ERROR_LOG ORDER BY UPDATEDATE DESC");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_AUDITLOG_Select_ErrorLogs_Dates(System.Nullable<System.DateTime> FromDate, System.Nullable<System.DateTime> ToDate, string ERR_STATUS)
        {
            string cSQL = "SELECT UPDATEDATE,ServerName,PROCESSOR_NAME,LogID,MODULENAME,DocType,BUYER_CODE,VENDOR_CODE,KEYREF2,AUDITVALUE,FILENAME,ERROR_PROBLEM,ERROR_SOLUTION,"+
                " ERROR_STATUS,PRIORITY_FLAG,ERROR_LOGID  FROM SMV_ERROR_LOG WHERE UPDATEDATE <= @TO_DATE AND UPDATEDATE >= @FROM_DATE ";
            if (!string.IsNullOrEmpty(ERR_STATUS)) { cSQL += " AND ERROR_STATUS = @ERR_STATUS"; }
            this._dataAccess.CreateSQLCommand(cSQL+" ORDER BY UPDATEDATE DESC ");
            this._dataAccess.AddParameter("FROM_DATE", FromDate, ParameterDirection.Input);
            this._dataAccess.AddParameter("TO_DATE", ToDate, ParameterDirection.Input);
            this._dataAccess.AddParameter("ERR_STATUS", ERR_STATUS, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Nullable<int> SM_AUDITLOG_Insert(string ModuleName, string FileName, string AuditValue, string KeyRef1, string KeyRef2, string LogType, System.Nullable<System.DateTime> UpdateDate, System.Nullable<int> BUYER_ID, System.Nullable<int> SUPPLIER_ID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_AUDITLOG_Insert");
            this._dataAccess.AddParameter("ModuleName", ModuleName, ParameterDirection.Input);
            this._dataAccess.AddParameter("FileName", FileName, ParameterDirection.Input);
            this._dataAccess.AddParameter("AuditValue", AuditValue, ParameterDirection.Input);
            this._dataAccess.AddParameter("KeyRef1", KeyRef1, ParameterDirection.Input);
            this._dataAccess.AddParameter("KeyRef2", KeyRef2, ParameterDirection.Input);
            this._dataAccess.AddParameter("LogType", LogType, ParameterDirection.Input);
            this._dataAccess.AddParameter("UpdateDate", UpdateDate, ParameterDirection.Input);
            this._dataAccess.AddParameter("BuyerID", BUYER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SupplierID", SUPPLIER_ID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_AUDITLOG_Delete(System.Nullable<int> LogID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_AUDITLOG_Delete");
            this._dataAccess.AddParameter("LogID", LogID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_AUDITLOG_Update(System.Nullable<int> LogID, string ModuleName, string FileName, string AuditValue, string KeyRef1, string KeyRef2, string LogType, System.Nullable<System.DateTime> UpdateDate, System.Nullable<int> BUYER_ID, System.Nullable<int> SUPPLIER_ID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_AUDITLOG_Update");
            this._dataAccess.AddParameter("LogID", LogID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ModuleName", ModuleName, ParameterDirection.Input);
            this._dataAccess.AddParameter("FileName", FileName, ParameterDirection.Input);
            this._dataAccess.AddParameter("AuditValue", AuditValue, ParameterDirection.Input);
            this._dataAccess.AddParameter("KeyRef1", KeyRef1, ParameterDirection.Input);
            this._dataAccess.AddParameter("KeyRef2", KeyRef2, ParameterDirection.Input);
            this._dataAccess.AddParameter("LogType", LogType, ParameterDirection.Input);
            this._dataAccess.AddParameter("UpdateDate", UpdateDate, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_ID", BUYER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_ID", SUPPLIER_ID, ParameterDirection.Input);
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

        public virtual DataSet GetAudit_Filter(string cFin_Cond, string FROMDATE,string TODATE)
        {
            string cSQL = "SELECT TOP 5000 CONVERT(VARCHAR(10),UPDATEDATE,103) AS UPDATEDATE,ServerName,PROCESSOR_NAME,MODULENAME,DOCTYPE,LOGTYPE,BuyerCode, SupplierCode,KEYREF2, AUDITVALUE,FILENAME,LOGID," +
            " BUYER_ID,SUPPLIER_ID FROM SM_AUDITLOG   WHERE   UPDATEDATE >= @FROMDATE AND UPDATEDATE <= @TODATE " + cFin_Cond;
            this._dataAccess.CreateSQLCommand(cSQL );
            this._dataAccess.AddParameter("FROMDATE", FROMDATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("TODATE", TODATE, ParameterDirection.Input);       
            this._dataAccess.AddParameter("cFin_Cond", cFin_Cond, ParameterDirection.Input);
            return this._dataAccess.ExecuteDataSet();
        }

        public virtual System.Data.DataSet SM_AUDITLOG_Select_All_Top5000(System.Nullable<System.DateTime> FromDate, System.Nullable<System.DateTime> ToDate)
        {
            this._dataAccess.CreateSQLCommand("SELECT TOP 5000 LOGID,UPDATEDATE,MODULENAME,FILENAME,AUDITVALUE,KEYREF1,KEYREF2,LOGTYPE,BuyerCode AS BUYER_CODE, SupplierCode AS VENDOR_CODE,BUYER_ID,SUPPLIER_ID,ServerName,BuyerCode,SupplierCode,DocType,PROCESSOR_NAME FROM SM_AUDITLOG WHERE UPDATEDATE <= @TO_DATE AND UPDATEDATE >= @FROM_DATE");
            this._dataAccess.AddParameter("FROM_DATE", FromDate, ParameterDirection.Input);
            this._dataAccess.AddParameter("TO_DATE", ToDate, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

    }
}

//SELECT top(1) logid FROM SM_AUDITLOG  WHERE UPDATEDATE >= '2018-03-01' AND UPDATEDATE <= '2018-04-30'  ORDER BY LOGID DESC --18287913
//SELECT top(1) logid FROM SM_AUDITLOG  WHERE UPDATEDATE >= '2018-03-01' AND UPDATEDATE <= '2018-04-30'  ORDER BY LOGID asc --18287661