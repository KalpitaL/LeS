namespace MetroLesMonitor.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;

    public partial class SmSendMailQueue : IDisposable
    {
        private DataAccess _dataAccess;

        public SmSendMailQueue()
        {
            this._dataAccess = new DataAccess();
        }

        public SmSendMailQueue(DataAccess _dataAccess)
        {
            this._dataAccess = _dataAccess;
        }

        public virtual System.Data.DataSet SM_SEND_MAIL_QUEUE_Select_All()
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_SEND_MAIL_QUEUE_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Nullable<int> SM_SEND_MAIL_QUEUE_Insert(string REF_KEY, string MAIL_FROM, string MAIL_TO, string MAIL_CC, string MAIL_BCC, string MAIL_SUBJECT, string MAIL_BODY, System.Nullable<System.DateTime> MAIL_DATE, System.Nullable<int> BUYER_ID, System.Nullable<int> SUPPLIER_ID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_SEND_MAIL_QUEUE_Insert_LesMonitor");
            this._dataAccess.AddParameter("REF_KEY", REF_KEY, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_FROM", MAIL_FROM, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_TO", MAIL_TO, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_CC", MAIL_CC, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_BCC", MAIL_BCC, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_SUBJECT", MAIL_SUBJECT, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_BODY", MAIL_BODY, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_DATE", MAIL_DATE, ParameterDirection.Input);
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
    }
}