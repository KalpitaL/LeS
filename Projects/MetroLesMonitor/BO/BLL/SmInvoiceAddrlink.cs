namespace MetroLesMonitor.Bll {
    
    
    public partial class SmInvoiceAddrlink {
        
        private System.Nullable<int> _linkid;
        
        private System.Nullable<int> _buyerid;
        
        private System.Nullable<int> _supplierid;
        
        private string _bankDetails;
        
        private string _accountNum;
        
        private string _ibanNum;
        
        private string _swiftNum;
        
        public virtual System.Nullable<int> Linkid {
            get {
                return _linkid;
            }
            set {
                _linkid = value;
            }
        }
        
        public virtual System.Nullable<int> Buyerid {
            get {
                return _buyerid;
            }
            set {
                _buyerid = value;
            }
        }
        
        public virtual System.Nullable<int> Supplierid {
            get {
                return _supplierid;
            }
            set {
                _supplierid = value;
            }
        }
        
        public virtual string BankDetails {
            get {
                return _bankDetails;
            }
            set {
                _bankDetails = value;
            }
        }
        
        public virtual string AccountNum {
            get {
                return _accountNum;
            }
            set {
                _accountNum = value;
            }
        }
        
        public virtual string IbanNum {
            get {
                return _ibanNum;
            }
            set {
                _ibanNum = value;
            }
        }
        
        public virtual string SwiftNum {
            get {
                return _swiftNum;
            }
            set {
                _swiftNum = value;
            }
        }
        
        private void Clean() {
            this.Linkid = null;
            this.Buyerid = null;
            this.Supplierid = null;
            this.BankDetails = string.Empty;
            this.AccountNum = string.Empty;
            this.IbanNum = string.Empty;
            this.SwiftNum = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["LINKID"] != System.DBNull.Value)) {
                this.Linkid = ((System.Nullable<int>)(dr["LINKID"]));
            }
            if ((dr["BUYERID"] != System.DBNull.Value)) {
                this.Buyerid = ((System.Nullable<int>)(dr["BUYERID"]));
            }
            if ((dr["SUPPLIERID"] != System.DBNull.Value)) {
                this.Supplierid = ((System.Nullable<int>)(dr["SUPPLIERID"]));
            }
            if ((dr["BANK_DETAILS"] != System.DBNull.Value)) {
                this.BankDetails = ((string)(dr["BANK_DETAILS"]));
            }
            if ((dr["ACCOUNT_NUM"] != System.DBNull.Value)) {
                this.AccountNum = ((string)(dr["ACCOUNT_NUM"]));
            }
            if ((dr["IBAN_NUM"] != System.DBNull.Value)) {
                this.IbanNum = ((string)(dr["IBAN_NUM"]));
            }
            if ((dr["SWIFT_NUM"] != System.DBNull.Value)) {
                this.SwiftNum = ((string)(dr["SWIFT_NUM"]));
            }
        }
        
        public static SmInvoiceAddrlinkCollection GetAll() {
            MetroLesMonitor.Dal.SmInvoiceAddrlink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmInvoiceAddrlink();
                System.Data.DataSet ds = dbo.SM_INVOICE_ADDRLINK_Select_All();
                SmInvoiceAddrlinkCollection collection = new SmInvoiceAddrlinkCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmInvoiceAddrlink obj = new SmInvoiceAddrlink();
                        obj.Fill(ds.Tables[0].Rows[i]);
                        if ((obj != null)) {
                            collection.Add(obj);
                        }
                    }
                }
                return collection;
            }
            catch (System.Exception ) {
                throw;
            }
            finally {
                if ((dbo != null)) {
                    dbo.Dispose();
                }
            }
        }
        
        public virtual void Insert() {
            MetroLesMonitor.Dal.SmInvoiceAddrlink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmInvoiceAddrlink();
                dbo.SM_INVOICE_ADDRLINK_Insert(this.Linkid, this.Buyerid, this.Supplierid, this.BankDetails, this.AccountNum, this.IbanNum, this.SwiftNum);
            }
            catch (System.Exception ) {
                throw;
            }
            finally {
                if ((dbo != null)) {
                    dbo.Dispose();
                }
            }
        }
    }
}
