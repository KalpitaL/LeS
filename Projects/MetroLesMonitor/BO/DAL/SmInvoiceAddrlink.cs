namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmInvoiceAddrlink : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmInvoiceAddrlink() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_INVOICE_ADDRLINK_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_INVOICE_ADDRLINK_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_INVOICE_ADDRLINK_Insert(System.Nullable<int> LINKID, System.Nullable<int> BUYERID, System.Nullable<int> SUPPLIERID, string BANK_DETAILS, string ACCOUNT_NUM, string IBAN_NUM, string SWIFT_NUM) {
            this._dataAccess.CreateProcedureCommand("sp_SM_INVOICE_ADDRLINK_Insert");
            this._dataAccess.AddParameter("LINKID", LINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYERID", BUYERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BANK_DETAILS", BANK_DETAILS, ParameterDirection.Input);
            this._dataAccess.AddParameter("ACCOUNT_NUM", ACCOUNT_NUM, ParameterDirection.Input);
            this._dataAccess.AddParameter("IBAN_NUM", IBAN_NUM, ParameterDirection.Input);
            this._dataAccess.AddParameter("SWIFT_NUM", SWIFT_NUM, ParameterDirection.Input);
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
