namespace MetroLesMonitor.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;

    public partial class SmFileAudit : IDisposable
    {
        private DataAccess _dataAccess;

        public SmFileAudit()
        {
            this._dataAccess = new DataAccess("AuditConnection");
        }

        public virtual System.Data.DataSet SM_FILE_AUDIT_Select_All()
        {
            _dataAccess.CreateSQLCommand(" SELECT * FROM SMV_FILE_AUDIT ORDER BY UPDATE_DATE DESC ");
            DataSet value = _dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_FILE_AUDIT_Select_All(DateTime FromDate, DateTime ToDate)
        {
            _dataAccess.CreateSQLCommand(" SELECT * FROM SMV_FILE_AUDIT WHERE UPDATE_DATE >= @FROMDATE AND UPDATE_DATE <= @TODATE ORDER BY UPDATE_DATE DESC ");
            _dataAccess.AddParameter("FROMDATE", FromDate, ParameterDirection.Input);
            _dataAccess.AddParameter("TODATE", ToDate, ParameterDirection.Input);
            DataSet value = _dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_FILE_AUDIT_Select_Filter(DateTime FromDate, DateTime ToDate,string FilterData)
        {
            _dataAccess.CreateSQLCommand(" SELECT * FROM SMV_FILE_AUDIT WHERE UPDATE_DATE >= @FROMDATE AND UPDATE_DATE <= @TODATE "+ FilterData+" ORDER BY UPDATE_DATE DESC ");
            _dataAccess.AddParameter("FROMDATE", FromDate, ParameterDirection.Input);
            _dataAccess.AddParameter("TODATE", ToDate, ParameterDirection.Input);
            DataSet value = _dataAccess.ExecuteDataSet();
            return value;
        }


        //public virtual System.Data.DataSet SM_FILE_AUDIT_SelectDetails(DateTime FromDate, DateTime ToDate,string cSrchFilter,
        //    string SEARCH,string SELECTCOND)
        //{
        //    string cWHERECOND = "";
        //    string csql = "SELECT " + SELECTCOND + " * FROM SMV_FILE_AUDIT WHERE UPDATE_DATE <= @TO_DATE AND UPDATE_DATE >= @FROM_DATE";

        //    if (SEARCH != "")
        //    {
        //        cWHERECOND = " AND VRNO LIKE @SEARCH OR VERSION LIKE @SEARCH OR BUYER_CODE LIKE @SEARCH OR SUPPLIER_CODE LIKE @SEARCH";
        //    }
        //    else
        //    {
        //        cWHERECOND = "";
        //    }

        //    csql += cWHERECOND + cSrchFilter;

        //    _dataAccess.CreateSQLCommand(csql + " ORDER BY UPDATE_DATE DESC ");
        //    _dataAccess.AddParameter("FROM_DATE", FromDate, ParameterDirection.Input);
        //    _dataAccess.AddParameter("TO_DATE", ToDate, ParameterDirection.Input);
        //    _dataAccess.AddParameter("SEARCH", "%" + SEARCH + "%", ParameterDirection.Input);
        //    DataSet value = _dataAccess.ExecuteDataSet();
        //    return value;
        //}

        public virtual System.Data.DataSet SM_FILE_AUDIT_Select_One(System.Nullable<int> RECORDID)
        {
            _dataAccess.CreateSQLCommand(" SELECT * FROM SMV_FILE_AUDIT WHERE RECORDID= " + RECORDID);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Nullable<int> SM_FILE_AUDIT_Insert(string VRNO, string DOC_FILENAME1, string DOC_FILENAME2, System.Nullable<int> BUYERID, System.Nullable<int> SUPPLIERID, System.Nullable<System.DateTime> BYR_UPLOADED, System.Nullable<System.DateTime> BYR_IMPORTED, System.Nullable<System.DateTime> SUP_EXPORTED, System.Nullable<System.DateTime> SUP_DOWNLOADED, System.Nullable<System.DateTime> SUP_UPLOADED, System.Nullable<System.DateTime> SUP_IMPORTED, System.Nullable<System.DateTime> BYR_EXPORTED, System.Nullable<System.DateTime> BYR_DOWNLOADED)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_FILE_AUDIT_Insert");
            this._dataAccess.AddParameter("VRNO", VRNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_FILENAME1", DOC_FILENAME1, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_FILENAME2", DOC_FILENAME2, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYERID", BUYERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BYR_UPLOADED", BYR_UPLOADED, ParameterDirection.Input);
            this._dataAccess.AddParameter("BYR_IMPORTED", BYR_IMPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUP_EXPORTED", SUP_EXPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUP_DOWNLOADED", SUP_DOWNLOADED, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUP_UPLOADED", SUP_UPLOADED, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUP_IMPORTED", SUP_IMPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("BYR_EXPORTED", BYR_EXPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("BYR_DOWNLOADED", BYR_DOWNLOADED, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_FILE_AUDIT_Delete(System.Nullable<int> RECORDID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_FILE_AUDIT_Delete");
            this._dataAccess.AddParameter("RECORDID", RECORDID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_FILE_AUDIT_Update(System.Nullable<int> RECORDID, string VRNO, string DOC_FILENAME1, string DOC_FILENAME2, System.Nullable<int> BUYERID, System.Nullable<int> SUPPLIERID, System.Nullable<System.DateTime> BYR_UPLOADED, System.Nullable<System.DateTime> BYR_IMPORTED, System.Nullable<System.DateTime> SUP_EXPORTED, System.Nullable<System.DateTime> SUP_DOWNLOADED, System.Nullable<System.DateTime> SUP_UPLOADED, System.Nullable<System.DateTime> SUP_IMPORTED, System.Nullable<System.DateTime> BYR_EXPORTED, System.Nullable<System.DateTime> BYR_DOWNLOADED)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_FILE_AUDIT_Update");
            this._dataAccess.AddParameter("RECORDID", RECORDID, ParameterDirection.Input);
            this._dataAccess.AddParameter("VRNO", VRNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_FILENAME1", DOC_FILENAME1, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_FILENAME2", DOC_FILENAME2, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYERID", BUYERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BYR_UPLOADED", BYR_UPLOADED, ParameterDirection.Input);
            this._dataAccess.AddParameter("BYR_IMPORTED", BYR_IMPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUP_EXPORTED", SUP_EXPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUP_DOWNLOADED", SUP_DOWNLOADED, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUP_UPLOADED", SUP_UPLOADED, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUP_IMPORTED", SUP_IMPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("BYR_EXPORTED", BYR_EXPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("BYR_DOWNLOADED", BYR_DOWNLOADED, ParameterDirection.Input);
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

        public virtual System.Data.DataSet SM_FILE_AUDIT_SelectDetails(DateTime FROM_DATE, DateTime TO_DATE, string cSrchFilter,
          string SEARCH, string SELECTCOND)
        {
            string cWHERECOND = "";
            string csql = "SELECT RECORDID,VRNO,VERSION,GROUP_ID,BUYERID,SUPPLIERID,RFQ_DOWNLOAD,RFQ_IMP,RFQ_EXP,RFQ_UPLOAD,RFQ_MAIL_SENT," +
              " QUOTE_DOWNLOAD,QUOTE_IMP,QUOTE_EXP,QUOTE_UPLOAD,QUOTE_MAIL_SENT,PO_DOWNLOAD,PO_IMP,PO_EXP,PO_UPLOAD,PO_MAIL_SENT,POC_DOWNLOAD,POC_IMP,POC_EXP," +
              " POC_UPLOAD,POC_MAIL_SENT,RFQ,QUOTE,PO,POC,BUYER_CODE,SUPPLIER_CODE,RFQ_END_STATE,QUOTE_END_STATE,PO_END_STATE,POC_END_STATE,UPDATE_DATE FROM   " +
              " (SELECT RECORDID,VRNO,VERSION,SM_FILE_AUDIT.GROUP_ID,BUYERID,SUPPLIERID,RFQ_DOWNLOAD,RFQ_IMP,RFQ_EXP,RFQ_UPLOAD,RFQ_MAIL_SENT," +
              " QUOTE_DOWNLOAD,QUOTE_IMP,QUOTE_EXP,QUOTE_UPLOAD,QUOTE_MAIL_SENT,PO_DOWNLOAD,PO_IMP,PO_EXP,PO_UPLOAD,PO_MAIL_SENT," +
              " POC_DOWNLOAD,POC_IMP,POC_EXP,POC_UPLOAD,POC_MAIL_SENT,RFQ,QUOTE,PO,POC,BUYER.ADDR_CODE AS 'BUYER_CODE',SUPP.ADDR_CODE AS 'SUPPLIER_CODE'," +
              " RFQ_END_STATE,QUOTE_END_STATE,PO_END_STATE,POC_END_STATE,SM_FILE_AUDIT.UPDATE_DATE ,ROW_NUMBER() OVER (ORDER BY SM_FILE_AUDIT.UPDATE_DATE DESC) AS ROWNUMBER" +
              " FROM SM_FILE_AUDIT LEFT OUTER JOIN SM_BUYER_SUPPLIER_GROUP_FLOW ON SM_FILE_AUDIT.GROUP_ID=SM_BUYER_SUPPLIER_GROUP_FLOW.GROUP_ID" +
              " INNER JOIN SM_ADDRESS BUYER ON SM_FILE_AUDIT.BUYERID=BUYER.ADDRESSID INNER JOIN SM_ADDRESS SUPP ON SM_FILE_AUDIT.SUPPLIERID=SUPP.ADDRESSID " +
              " WHERE (1=1) AND SM_FILE_AUDIT.UPDATE_DATE >= @FROM_DATE AND SM_FILE_AUDIT.UPDATE_DATE <= @TO_DATE " + cSrchFilter + ") AS TEMP ";

            _dataAccess.CreateSQLCommand(csql + " ORDER BY UPDATE_DATE DESC ");
            _dataAccess.AddParameter("FROM_DATE", FROM_DATE, ParameterDirection.Input);
            _dataAccess.AddParameter("TO_DATE", TO_DATE, ParameterDirection.Input);
            _dataAccess.AddParameter("SEARCH", "%" + SEARCH + "%", ParameterDirection.Input);
            DataSet value = _dataAccess.ExecuteDataSet();
            return value;
        }
    }
}