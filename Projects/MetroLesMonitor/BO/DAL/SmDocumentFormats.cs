using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Xml;

namespace MetroLesMonitor.Dal {

    public partial class SmDocumentFormats : IDisposable
    {
        public DataAccess _dataAccess;

        public SmDocumentFormats()
        {
            this._dataAccess = new DataAccess();
        }

        public SmDocumentFormats(DataAccess _dataAccess)
        {
            this._dataAccess = (_dataAccess != null) ? _dataAccess : new DataAccess();
        }

        public virtual System.Data.DataSet SM_DOCUMENT_FORMATS_Select_All()
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_DOCUMENT_FORMATS_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_DOCUMENT_FORMATS_Select_One(System.Nullable<int> DOCFORMATID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_DOCUMENT_FORMATS_Select_One");
            this._dataAccess.AddParameter("DOCFORMATID", DOCFORMATID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Nullable<int> SM_DOCUMENT_FORMATS_Insert(string DOCUMENT_FORMAT, string IMPORT_PATH, string EXPORT_PATH, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<System.DateTime> UPDATED_DATE, string ADDR_TYPE)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_DOCUMENT_FORMATS_Insert");
            this._dataAccess.AddParameter("DOCUMENT_FORMAT", DOCUMENT_FORMAT, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_PATH", IMPORT_PATH, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_PATH", EXPORT_PATH, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATED_DATE", UPDATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_TYPE", ADDR_TYPE, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_DOCUMENT_FORMATS_Delete(System.Nullable<int> DOCFORMATID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_DOCUMENT_FORMATS_Delete");
            this._dataAccess.AddParameter("DOCFORMATID", DOCFORMATID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_DOCUMENT_FORMATS_Update(System.Nullable<int> DOCFORMATID, string DOCUMENT_FORMAT, string IMPORT_PATH, string EXPORT_PATH, System.Nullable<System.DateTime> CREATED_DATE, System.Nullable<System.DateTime> UPDATED_DATE, string ADDR_TYPE)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_DOCUMENT_FORMATS_Update");
            this._dataAccess.AddParameter("DOCFORMATID", DOCFORMATID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOCUMENT_FORMAT", DOCUMENT_FORMAT, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_PATH", IMPORT_PATH, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_PATH", EXPORT_PATH, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATED_DATE", UPDATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_TYPE", ADDR_TYPE, ParameterDirection.Input);
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

        //added by kalpita on 17/01/2018
        public virtual System.Data.DataSet SM_DOCUMENT_FORMATS_By_AddrType(string ADDR_TYPE)
        {
            string cSQL = "SELECT DISTINCT DOCUMENT_FORMAT AS 'FORMAT',DOCFORMATID AS 'ID' FROM SM_DOCUMENT_FORMATS WHERE ADDR_TYPE IS NOT NULL AND ADDR_TYPE = @ADDR_TYPE ORDER BY DOCUMENT_FORMAT";
            this._dataAccess.CreateSQLCommand(cSQL);
            this._dataAccess.AddParameter("ADDR_TYPE", ADDR_TYPE, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_DOCUMENT_FORMATS_By_Format(string DOCUMENT_FORMAT)
        {
            string cSQL = "SELECT *  FROM SM_DOCUMENT_FORMATS WHERE DOCUMENT_FORMAT IS NOT NULL AND DOCUMENT_FORMAT = @DOCUMENT_FORMAT";
            this._dataAccess.CreateSQLCommand(cSQL);
            this._dataAccess.AddParameter("DOCUMENT_FORMAT", DOCUMENT_FORMAT, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
    }
}
