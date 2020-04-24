namespace MetroLesMonitor.Bll {
    
    
    public partial class SmQuotationsVendorAddress {
        
        private System.Nullable<int> _addressid;
        
        private System.Nullable<int> _quotationid;
        
        private string _addrType;
        
        private string _addrCode;
        
        private string _addrName;
        
        private string _contactPerson;
        
        private string _address1;
        
        private string _address2;
        
        private string _address3;
        
        private string _address4;
        
        private string _addrCity;
        
        private string _addrCountry;
        
        private string _addrZipcode;
        
        private string _addrPhone1;
        
        private string _addrPhone2;
        
        private string _addrFax;
        
        private string _addrTelex;
        
        private string _addrEmail;
        
        private string _addrMobilephone;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        private string _addrRemarks;
        
        private string _emailCc;
        
        private string _emailBcc;
        
        public virtual System.Nullable<int> Addressid {
            get {
                return _addressid;
            }
            set {
                _addressid = value;
            }
        }
        
        public virtual System.Nullable<int> Quotationid {
            get {
                return _quotationid;
            }
            set {
                _quotationid = value;
            }
        }
        
        public virtual string AddrType {
            get {
                return _addrType;
            }
            set {
                _addrType = value;
            }
        }
        
        public virtual string AddrCode {
            get {
                return _addrCode;
            }
            set {
                _addrCode = value;
            }
        }
        
        public virtual string AddrName {
            get {
                return _addrName;
            }
            set {
                _addrName = value;
            }
        }
        
        public virtual string ContactPerson {
            get {
                return _contactPerson;
            }
            set {
                _contactPerson = value;
            }
        }
        
        public virtual string Address1 {
            get {
                return _address1;
            }
            set {
                _address1 = value;
            }
        }
        
        public virtual string Address2 {
            get {
                return _address2;
            }
            set {
                _address2 = value;
            }
        }
        
        public virtual string Address3 {
            get {
                return _address3;
            }
            set {
                _address3 = value;
            }
        }
        
        public virtual string Address4 {
            get {
                return _address4;
            }
            set {
                _address4 = value;
            }
        }
        
        public virtual string AddrCity {
            get {
                return _addrCity;
            }
            set {
                _addrCity = value;
            }
        }
        
        public virtual string AddrCountry {
            get {
                return _addrCountry;
            }
            set {
                _addrCountry = value;
            }
        }
        
        public virtual string AddrZipcode {
            get {
                return _addrZipcode;
            }
            set {
                _addrZipcode = value;
            }
        }
        
        public virtual string AddrPhone1 {
            get {
                return _addrPhone1;
            }
            set {
                _addrPhone1 = value;
            }
        }
        
        public virtual string AddrPhone2 {
            get {
                return _addrPhone2;
            }
            set {
                _addrPhone2 = value;
            }
        }
        
        public virtual string AddrFax {
            get {
                return _addrFax;
            }
            set {
                _addrFax = value;
            }
        }
        
        public virtual string AddrTelex {
            get {
                return _addrTelex;
            }
            set {
                _addrTelex = value;
            }
        }
        
        public virtual string AddrEmail {
            get {
                return _addrEmail;
            }
            set {
                _addrEmail = value;
            }
        }
        
        public virtual string AddrMobilephone {
            get {
                return _addrMobilephone;
            }
            set {
                _addrMobilephone = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> UpdateDate {
            get {
                return _updateDate;
            }
            set {
                _updateDate = value;
            }
        }
        
        public virtual string AddrRemarks {
            get {
                return _addrRemarks;
            }
            set {
                _addrRemarks = value;
            }
        }
        
        public virtual string EmailCc {
            get {
                return _emailCc;
            }
            set {
                _emailCc = value;
            }
        }
        
        public virtual string EmailBcc {
            get {
                return _emailBcc;
            }
            set {
                _emailBcc = value;
            }
        }
        
        private void Clean() {
            this.Addressid = null;
            this.Quotationid = null;
            this.AddrType = string.Empty;
            this.AddrCode = string.Empty;
            this.AddrName = string.Empty;
            this.ContactPerson = string.Empty;
            this.Address1 = string.Empty;
            this.Address2 = string.Empty;
            this.Address3 = string.Empty;
            this.Address4 = string.Empty;
            this.AddrCity = string.Empty;
            this.AddrCountry = string.Empty;
            this.AddrZipcode = string.Empty;
            this.AddrPhone1 = string.Empty;
            this.AddrPhone2 = string.Empty;
            this.AddrFax = string.Empty;
            this.AddrTelex = string.Empty;
            this.AddrEmail = string.Empty;
            this.AddrMobilephone = string.Empty;
            this.UpdateDate = null;
            this.AddrRemarks = string.Empty;
            this.EmailCc = string.Empty;
            this.EmailBcc = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["ADDRESSID"] != System.DBNull.Value)) {
                this.Addressid = ((System.Nullable<int>)(dr["ADDRESSID"]));
            }
            if ((dr["QUOTATIONID"] != System.DBNull.Value)) {
                this.Quotationid = ((System.Nullable<int>)(dr["QUOTATIONID"]));
            }
            if ((dr["ADDR_TYPE"] != System.DBNull.Value)) {
                this.AddrType = ((string)(dr["ADDR_TYPE"]));
            }
            if ((dr["ADDR_CODE"] != System.DBNull.Value)) {
                this.AddrCode = ((string)(dr["ADDR_CODE"]));
            }
            if ((dr["ADDR_NAME"] != System.DBNull.Value)) {
                this.AddrName = ((string)(dr["ADDR_NAME"]));
            }
            if ((dr["CONTACT_PERSON"] != System.DBNull.Value)) {
                this.ContactPerson = ((string)(dr["CONTACT_PERSON"]));
            }
            if ((dr["ADDRESS1"] != System.DBNull.Value)) {
                this.Address1 = ((string)(dr["ADDRESS1"]));
            }
            if ((dr["ADDRESS2"] != System.DBNull.Value)) {
                this.Address2 = ((string)(dr["ADDRESS2"]));
            }
            if ((dr["ADDRESS3"] != System.DBNull.Value)) {
                this.Address3 = ((string)(dr["ADDRESS3"]));
            }
            if ((dr["ADDRESS4"] != System.DBNull.Value)) {
                this.Address4 = ((string)(dr["ADDRESS4"]));
            }
            if ((dr["ADDR_CITY"] != System.DBNull.Value)) {
                this.AddrCity = ((string)(dr["ADDR_CITY"]));
            }
            if ((dr["ADDR_COUNTRY"] != System.DBNull.Value)) {
                this.AddrCountry = ((string)(dr["ADDR_COUNTRY"]));
            }
            if ((dr["ADDR_ZIPCODE"] != System.DBNull.Value)) {
                this.AddrZipcode = ((string)(dr["ADDR_ZIPCODE"]));
            }
            if ((dr["ADDR_PHONE1"] != System.DBNull.Value)) {
                this.AddrPhone1 = ((string)(dr["ADDR_PHONE1"]));
            }
            if ((dr["ADDR_PHONE2"] != System.DBNull.Value)) {
                this.AddrPhone2 = ((string)(dr["ADDR_PHONE2"]));
            }
            if ((dr["ADDR_FAX"] != System.DBNull.Value)) {
                this.AddrFax = ((string)(dr["ADDR_FAX"]));
            }
            if ((dr["ADDR_TELEX"] != System.DBNull.Value)) {
                this.AddrTelex = ((string)(dr["ADDR_TELEX"]));
            }
            if ((dr["ADDR_EMAIL"] != System.DBNull.Value)) {
                this.AddrEmail = ((string)(dr["ADDR_EMAIL"]));
            }
            if ((dr["ADDR_MOBILEPHONE"] != System.DBNull.Value)) {
                this.AddrMobilephone = ((string)(dr["ADDR_MOBILEPHONE"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value)) {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
            if ((dr["ADDR_REMARKS"] != System.DBNull.Value)) {
                this.AddrRemarks = ((string)(dr["ADDR_REMARKS"]));
            }
            if ((dr["EMAIL_CC"] != System.DBNull.Value)) {
                this.EmailCc = ((string)(dr["EMAIL_CC"]));
            }
            if ((dr["EMAIL_BCC"] != System.DBNull.Value)) {
                this.EmailBcc = ((string)(dr["EMAIL_BCC"]));
            }
        }
        
        public static SmQuotationsVendorAddressCollection GetAll() {
            MetroLesMonitor.Dal.SmQuotationsVendorAddress dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmQuotationsVendorAddress();
                System.Data.DataSet ds = dbo.SM_QUOTATIONS_VENDOR_ADDRESS_Select_All();
                SmQuotationsVendorAddressCollection collection = new SmQuotationsVendorAddressCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmQuotationsVendorAddress obj = new SmQuotationsVendorAddress();
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
        
        public static SmQuotationsVendorAddress Load(System.Nullable<int> ADDRESSID) {
            MetroLesMonitor.Dal.SmQuotationsVendorAddress dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmQuotationsVendorAddress();
                System.Data.DataSet ds = dbo.SM_QUOTATIONS_VENDOR_ADDRESS_Select_One(ADDRESSID);
                SmQuotationsVendorAddress obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmQuotationsVendorAddress();
                        obj.Fill(ds.Tables[0].Rows[0]);
                    }
                }
                return obj;
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
        
        public virtual void Load() {
            MetroLesMonitor.Dal.SmQuotationsVendorAddress dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmQuotationsVendorAddress();
                System.Data.DataSet ds = dbo.SM_QUOTATIONS_VENDOR_ADDRESS_Select_One(this.Addressid);
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        this.Fill(ds.Tables[0].Rows[0]);
                    }
                }
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
            MetroLesMonitor.Dal.SmQuotationsVendorAddress dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmQuotationsVendorAddress();
                dbo.SM_QUOTATIONS_VENDOR_ADDRESS_Insert(this.Quotationid, this.AddrType, this.AddrCode, this.AddrName, this.ContactPerson, this.Address1, this.Address2, this.Address3, this.Address4, this.AddrCity, this.AddrCountry, this.AddrZipcode, this.AddrPhone1, this.AddrPhone2, this.AddrFax, this.AddrTelex, this.AddrEmail, this.AddrMobilephone, this.UpdateDate, this.AddrRemarks, this.EmailCc, this.EmailBcc);
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
        
        public virtual void Delete() {
            MetroLesMonitor.Dal.SmQuotationsVendorAddress dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmQuotationsVendorAddress();
                dbo.SM_QUOTATIONS_VENDOR_ADDRESS_Delete(this.Addressid);
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
        
        public virtual void Update() {
            MetroLesMonitor.Dal.SmQuotationsVendorAddress dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmQuotationsVendorAddress();
                dbo.SM_QUOTATIONS_VENDOR_ADDRESS_Update(this.Addressid, this.Quotationid, this.AddrType, this.AddrCode, this.AddrName, this.ContactPerson, this.Address1, this.Address2, this.Address3, this.Address4, this.AddrCity, this.AddrCountry, this.AddrZipcode, this.AddrPhone1, this.AddrPhone2, this.AddrFax, this.AddrTelex, this.AddrEmail, this.AddrMobilephone, this.UpdateDate, this.AddrRemarks, this.EmailCc, this.EmailBcc);
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
