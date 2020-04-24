namespace MetroLesMonitor.Bll {
    
    
    public partial class MxmlAddress {
        
        private System.Nullable<System.Guid> _mxmladdressid;
        
        private System.Nullable<System.Guid> _mxmldocid;
        
        private string _addrType;
        
        private string _addrName;
        
        private string _contactPerson;
        
        private string _addrStreet11;
        
        private string _addrStreet2;
        
        private string _addrCity;
        
        private string _addrPostcode;
        
        private string _addrCountry;
        
        private string _addrEmail;
        
        private string _addrPhoneCountry;
        
        private string _addrPhoneAreacode;
        
        private string _addrPhoneNumber;
        
        private string _addrFaxCountry;
        
        private string _addrFaxAreacode;
        
        private string _addrFaxNumber;
        
        private string _addrComments;
        
        private MxmlTransactionHeader _mxmlTransactionHeader;
        
        public virtual System.Nullable<System.Guid> Mxmladdressid {
            get {
                return _mxmladdressid;
            }
            set {
                _mxmladdressid = value;
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
        
        public virtual string AddrStreet11 {
            get {
                return _addrStreet11;
            }
            set {
                _addrStreet11 = value;
            }
        }
        
        public virtual string AddrStreet2 {
            get {
                return _addrStreet2;
            }
            set {
                _addrStreet2 = value;
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
        
        public virtual string AddrPostcode {
            get {
                return _addrPostcode;
            }
            set {
                _addrPostcode = value;
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
        
        public virtual string AddrEmail {
            get {
                return _addrEmail;
            }
            set {
                _addrEmail = value;
            }
        }
        
        public virtual string AddrPhoneCountry {
            get {
                return _addrPhoneCountry;
            }
            set {
                _addrPhoneCountry = value;
            }
        }
        
        public virtual string AddrPhoneAreacode {
            get {
                return _addrPhoneAreacode;
            }
            set {
                _addrPhoneAreacode = value;
            }
        }
        
        public virtual string AddrPhoneNumber {
            get {
                return _addrPhoneNumber;
            }
            set {
                _addrPhoneNumber = value;
            }
        }
        
        public virtual string AddrFaxCountry {
            get {
                return _addrFaxCountry;
            }
            set {
                _addrFaxCountry = value;
            }
        }
        
        public virtual string AddrFaxAreacode {
            get {
                return _addrFaxAreacode;
            }
            set {
                _addrFaxAreacode = value;
            }
        }
        
        public virtual string AddrFaxNumber {
            get {
                return _addrFaxNumber;
            }
            set {
                _addrFaxNumber = value;
            }
        }
        
        public virtual string AddrComments {
            get {
                return _addrComments;
            }
            set {
                _addrComments = value;
            }
        }
        
        public virtual MxmlTransactionHeader MxmlTransactionHeader {
            get {
                if ((this._mxmlTransactionHeader == null)) {
                    this._mxmlTransactionHeader = MetroLesMonitor.Bll.MxmlTransactionHeader.Load(this._mxmldocid);
                }
                return this._mxmlTransactionHeader;
            }
            set {
                _mxmlTransactionHeader = value;
            }
        }
        
        private void Clean() {
            this.Mxmladdressid = null;
            this._mxmldocid = null;
            this.AddrType = string.Empty;
            this.AddrName = string.Empty;
            this.ContactPerson = string.Empty;
            this.AddrStreet11 = string.Empty;
            this.AddrStreet2 = string.Empty;
            this.AddrCity = string.Empty;
            this.AddrPostcode = string.Empty;
            this.AddrCountry = string.Empty;
            this.AddrEmail = string.Empty;
            this.AddrPhoneCountry = string.Empty;
            this.AddrPhoneAreacode = string.Empty;
            this.AddrPhoneNumber = string.Empty;
            this.AddrFaxCountry = string.Empty;
            this.AddrFaxAreacode = string.Empty;
            this.AddrFaxNumber = string.Empty;
            this.AddrComments = string.Empty;
            this.MxmlTransactionHeader = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["MXMLADDRESSID"] != System.DBNull.Value)) {
                this.Mxmladdressid = ((System.Nullable<System.Guid>)(dr["MXMLADDRESSID"]));
            }
            if ((dr["MXMLDOCID"] != System.DBNull.Value)) {
                this._mxmldocid = ((System.Nullable<System.Guid>)(dr["MXMLDOCID"]));
            }
            if ((dr["ADDR_TYPE"] != System.DBNull.Value)) {
                this.AddrType = ((string)(dr["ADDR_TYPE"]));
            }
            if ((dr["ADDR_NAME"] != System.DBNull.Value)) {
                this.AddrName = ((string)(dr["ADDR_NAME"]));
            }
            if ((dr["CONTACT_PERSON"] != System.DBNull.Value)) {
                this.ContactPerson = ((string)(dr["CONTACT_PERSON"]));
            }
            if ((dr["ADDR_STREET11"] != System.DBNull.Value)) {
                this.AddrStreet11 = ((string)(dr["ADDR_STREET11"]));
            }
            if ((dr["ADDR_STREET2"] != System.DBNull.Value)) {
                this.AddrStreet2 = ((string)(dr["ADDR_STREET2"]));
            }
            if ((dr["ADDR_CITY"] != System.DBNull.Value)) {
                this.AddrCity = ((string)(dr["ADDR_CITY"]));
            }
            if ((dr["ADDR_POSTCODE"] != System.DBNull.Value)) {
                this.AddrPostcode = ((string)(dr["ADDR_POSTCODE"]));
            }
            if ((dr["ADDR_COUNTRY"] != System.DBNull.Value)) {
                this.AddrCountry = ((string)(dr["ADDR_COUNTRY"]));
            }
            if ((dr["ADDR_EMAIL"] != System.DBNull.Value)) {
                this.AddrEmail = ((string)(dr["ADDR_EMAIL"]));
            }
            if ((dr["ADDR_PHONE_COUNTRY"] != System.DBNull.Value)) {
                this.AddrPhoneCountry = ((string)(dr["ADDR_PHONE_COUNTRY"]));
            }
            if ((dr["ADDR_PHONE_AREACODE"] != System.DBNull.Value)) {
                this.AddrPhoneAreacode = ((string)(dr["ADDR_PHONE_AREACODE"]));
            }
            if ((dr["ADDR_PHONE_NUMBER"] != System.DBNull.Value)) {
                this.AddrPhoneNumber = ((string)(dr["ADDR_PHONE_NUMBER"]));
            }
            if ((dr["ADDR_FAX_COUNTRY"] != System.DBNull.Value)) {
                this.AddrFaxCountry = ((string)(dr["ADDR_FAX_COUNTRY"]));
            }
            if ((dr["ADDR_FAX_AREACODE"] != System.DBNull.Value)) {
                this.AddrFaxAreacode = ((string)(dr["ADDR_FAX_AREACODE"]));
            }
            if ((dr["ADDR_FAX_NUMBER"] != System.DBNull.Value)) {
                this.AddrFaxNumber = ((string)(dr["ADDR_FAX_NUMBER"]));
            }
            if ((dr["ADDR_COMMENTS"] != System.DBNull.Value)) {
                this.AddrComments = ((string)(dr["ADDR_COMMENTS"]));
            }
        }
        
        public static MxmlAddressCollection Select_MXML_ADDRESSs_By_MXMLDOCID(System.Nullable<System.Guid> MXMLDOCID) {
            MetroLesMonitor.Dal.MxmlAddress dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlAddress();
                System.Data.DataSet ds = dbo.Select_MXML_ADDRESSs_By_MXMLDOCID(MXMLDOCID);
                MxmlAddressCollection collection = new MxmlAddressCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        MxmlAddress obj = new MxmlAddress();
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
        
        public static MxmlAddressCollection GetAll() {
            MetroLesMonitor.Dal.MxmlAddress dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlAddress();
                System.Data.DataSet ds = dbo.MXML_ADDRESS_Select_All();
                MxmlAddressCollection collection = new MxmlAddressCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        MxmlAddress obj = new MxmlAddress();
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
        
        public static MxmlAddress Load(System.Nullable<System.Guid> MXMLADDRESSID) {
            MetroLesMonitor.Dal.MxmlAddress dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlAddress();
                System.Data.DataSet ds = dbo.MXML_ADDRESS_Select_One(MXMLADDRESSID);
                MxmlAddress obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new MxmlAddress();
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
            MetroLesMonitor.Dal.MxmlAddress dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlAddress();
                System.Data.DataSet ds = dbo.MXML_ADDRESS_Select_One(this.Mxmladdressid);
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
            MetroLesMonitor.Dal.MxmlAddress dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlAddress();
                dbo.MXML_ADDRESS_Insert(this._mxmldocid, this.AddrType, this.AddrName, this.ContactPerson, this.AddrStreet11, this.AddrStreet2, this.AddrCity, this.AddrPostcode, this.AddrCountry, this.AddrEmail, this.AddrPhoneCountry, this.AddrPhoneAreacode, this.AddrPhoneNumber, this.AddrFaxCountry, this.AddrFaxAreacode, this.AddrFaxNumber, this.AddrComments);
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
            MetroLesMonitor.Dal.MxmlAddress dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlAddress();
                dbo.MXML_ADDRESS_Delete(this.Mxmladdressid);
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
            MetroLesMonitor.Dal.MxmlAddress dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlAddress();
                dbo.MXML_ADDRESS_Update(this.Mxmladdressid, this._mxmldocid, this.AddrType, this.AddrName, this.ContactPerson, this.AddrStreet11, this.AddrStreet2, this.AddrCity, this.AddrPostcode, this.AddrCountry, this.AddrEmail, this.AddrPhoneCountry, this.AddrPhoneAreacode, this.AddrPhoneNumber, this.AddrFaxCountry, this.AddrFaxAreacode, this.AddrFaxNumber, this.AddrComments);
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
