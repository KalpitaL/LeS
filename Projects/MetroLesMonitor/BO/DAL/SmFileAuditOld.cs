namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmFileAuditOld : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmFileAuditOld() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_FILE_AUDIT_old_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_FILE_AUDIT_old_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_FILE_AUDIT_old_Select_One(System.Nullable<int> RECORDID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_FILE_AUDIT_old_Select_One");
            this._dataAccess.AddParameter("RECORDID", RECORDID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_FILE_AUDIT_old_Insert(string VRNO, string DOC_FILENAME1, string DOC_FILENAME2, System.Nullable<int> BUYERID, System.Nullable<int> SUPPLIERID, System.Nullable<System.DateTime> BYR_UPLOADED, System.Nullable<System.DateTime> BYR_IMPORTED, System.Nullable<System.DateTime> SUP_EXPORTED, System.Nullable<System.DateTime> SUP_DOWNLOADED, System.Nullable<System.DateTime> SUP_UPLOADED, System.Nullable<System.DateTime> SUP_IMPORTED, System.Nullable<System.DateTime> BYR_EXPORTED, System.Nullable<System.DateTime> BYR_DOWNLOADED, System.Nullable<System.DateTime> UPDATE_DATE) {
            this._dataAccess.CreateProcedureCommand("sp_SM_FILE_AUDIT_old_Insert");
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
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_FILE_AUDIT_old_Delete(System.Nullable<int> RECORDID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_FILE_AUDIT_old_Delete");
            this._dataAccess.AddParameter("RECORDID", RECORDID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_FILE_AUDIT_old_Update(System.Nullable<int> RECORDID, string VRNO, string DOC_FILENAME1, string DOC_FILENAME2, System.Nullable<int> BUYERID, System.Nullable<int> SUPPLIERID, System.Nullable<System.DateTime> BYR_UPLOADED, System.Nullable<System.DateTime> BYR_IMPORTED, System.Nullable<System.DateTime> SUP_EXPORTED, System.Nullable<System.DateTime> SUP_DOWNLOADED, System.Nullable<System.DateTime> SUP_UPLOADED, System.Nullable<System.DateTime> SUP_IMPORTED, System.Nullable<System.DateTime> BYR_EXPORTED, System.Nullable<System.DateTime> BYR_DOWNLOADED, System.Nullable<System.DateTime> UPDATE_DATE) {
            this._dataAccess.CreateProcedureCommand("sp_SM_FILE_AUDIT_old_Update");
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
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
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
