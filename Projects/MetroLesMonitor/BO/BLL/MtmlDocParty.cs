namespace MetroLesMonitor.Bll {
    
    
    public partial class MtmlDocParty {
        
        private System.Nullable<System.Guid> _mtmlpartyid;
        
        private System.Nullable<System.Guid> _mtmldocid;
        
        private string _qualifier;
        
        private string _partyName;
        
        private string _identification;
        
        private string _streetaddress;
        
        private string _city;
        
        private string _postcode;
        
        private string _state;
        
        private string _countrycode;
        
        private string _functioncode;
        
        private string _contactName;
        
        private string _phonenumber;
        
        private string _fax;
        
        private string _email;
        
        private string _partyLocation;
        
        private string _streetaddress2;
        
        private string _streetaddress3;
        
        private string _emailCc;
        
        private string _emailBcc;
        
        private System.Nullable<int> _autoid;
        
        public virtual System.Nullable<System.Guid> Mtmlpartyid {
            get {
                return _mtmlpartyid;
            }
            set {
                _mtmlpartyid = value;
            }
        }
        
        public virtual System.Nullable<System.Guid> Mtmldocid {
            get {
                return _mtmldocid;
            }
            set {
                _mtmldocid = value;
            }
        }
        
        public virtual string Qualifier {
            get {
                return _qualifier;
            }
            set {
                _qualifier = value;
            }
        }
        
        public virtual string PartyName {
            get {
                return _partyName;
            }
            set {
                _partyName = value;
            }
        }
        
        public virtual string Identification {
            get {
                return _identification;
            }
            set {
                _identification = value;
            }
        }
        
        public virtual string Streetaddress {
            get {
                return _streetaddress;
            }
            set {
                _streetaddress = value;
            }
        }
        
        public virtual string City {
            get {
                return _city;
            }
            set {
                _city = value;
            }
        }
        
        public virtual string Postcode {
            get {
                return _postcode;
            }
            set {
                _postcode = value;
            }
        }
        
        public virtual string State {
            get {
                return _state;
            }
            set {
                _state = value;
            }
        }
        
        public virtual string Countrycode {
            get {
                return _countrycode;
            }
            set {
                _countrycode = value;
            }
        }
        
        public virtual string Functioncode {
            get {
                return _functioncode;
            }
            set {
                _functioncode = value;
            }
        }
        
        public virtual string ContactName {
            get {
                return _contactName;
            }
            set {
                _contactName = value;
            }
        }
        
        public virtual string Phonenumber {
            get {
                return _phonenumber;
            }
            set {
                _phonenumber = value;
            }
        }
        
        public virtual string Fax {
            get {
                return _fax;
            }
            set {
                _fax = value;
            }
        }
        
        public virtual string Email {
            get {
                return _email;
            }
            set {
                _email = value;
            }
        }
        
        public virtual string PartyLocation {
            get {
                return _partyLocation;
            }
            set {
                _partyLocation = value;
            }
        }
        
        public virtual string Streetaddress2 {
            get {
                return _streetaddress2;
            }
            set {
                _streetaddress2 = value;
            }
        }
        
        public virtual string Streetaddress3 {
            get {
                return _streetaddress3;
            }
            set {
                _streetaddress3 = value;
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
        
        public virtual System.Nullable<int> Autoid {
            get {
                return _autoid;
            }
            set {
                _autoid = value;
            }
        }
        
        private void Clean() {
            this.Mtmlpartyid = null;
            this.Mtmldocid = null;
            this.Qualifier = string.Empty;
            this.PartyName = string.Empty;
            this.Identification = string.Empty;
            this.Streetaddress = string.Empty;
            this.City = string.Empty;
            this.Postcode = string.Empty;
            this.State = string.Empty;
            this.Countrycode = string.Empty;
            this.Functioncode = string.Empty;
            this.ContactName = string.Empty;
            this.Phonenumber = string.Empty;
            this.Fax = string.Empty;
            this.Email = string.Empty;
            this.PartyLocation = string.Empty;
            this.Streetaddress2 = string.Empty;
            this.Streetaddress3 = string.Empty;
            this.EmailCc = string.Empty;
            this.EmailBcc = string.Empty;
            this.Autoid = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["MTMLPARTYID"] != System.DBNull.Value)) {
                this.Mtmlpartyid = ((System.Nullable<System.Guid>)(dr["MTMLPARTYID"]));
            }
            if ((dr["MTMLDOCID"] != System.DBNull.Value)) {
                this.Mtmldocid = ((System.Nullable<System.Guid>)(dr["MTMLDOCID"]));
            }
            if ((dr["QUALIFIER"] != System.DBNull.Value)) {
                this.Qualifier = ((string)(dr["QUALIFIER"]));
            }
            if ((dr["PARTY_NAME"] != System.DBNull.Value)) {
                this.PartyName = ((string)(dr["PARTY_NAME"]));
            }
            if ((dr["IDENTIFICATION"] != System.DBNull.Value)) {
                this.Identification = ((string)(dr["IDENTIFICATION"]));
            }
            if ((dr["STREETADDRESS"] != System.DBNull.Value)) {
                this.Streetaddress = ((string)(dr["STREETADDRESS"]));
            }
            if ((dr["CITY"] != System.DBNull.Value)) {
                this.City = ((string)(dr["CITY"]));
            }
            if ((dr["POSTCODE"] != System.DBNull.Value)) {
                this.Postcode = ((string)(dr["POSTCODE"]));
            }
            if ((dr["STATE"] != System.DBNull.Value)) {
                this.State = ((string)(dr["STATE"]));
            }
            if ((dr["COUNTRYCODE"] != System.DBNull.Value)) {
                this.Countrycode = ((string)(dr["COUNTRYCODE"]));
            }
            if ((dr["FUNCTIONCODE"] != System.DBNull.Value)) {
                this.Functioncode = ((string)(dr["FUNCTIONCODE"]));
            }
            if ((dr["CONTACT_NAME"] != System.DBNull.Value)) {
                this.ContactName = ((string)(dr["CONTACT_NAME"]));
            }
            if ((dr["PHONENUMBER"] != System.DBNull.Value)) {
                this.Phonenumber = ((string)(dr["PHONENUMBER"]));
            }
            if ((dr["FAX"] != System.DBNull.Value)) {
                this.Fax = ((string)(dr["FAX"]));
            }
            if ((dr["EMAIL"] != System.DBNull.Value)) {
                this.Email = ((string)(dr["EMAIL"]));
            }
            if ((dr["PARTY_LOCATION"] != System.DBNull.Value)) {
                this.PartyLocation = ((string)(dr["PARTY_LOCATION"]));
            }
            if ((dr["STREETADDRESS2"] != System.DBNull.Value)) {
                this.Streetaddress2 = ((string)(dr["STREETADDRESS2"]));
            }
            if ((dr["STREETADDRESS3"] != System.DBNull.Value)) {
                this.Streetaddress3 = ((string)(dr["STREETADDRESS3"]));
            }
            if ((dr["EMAIL_CC"] != System.DBNull.Value)) {
                this.EmailCc = ((string)(dr["EMAIL_CC"]));
            }
            if ((dr["EMAIL_BCC"] != System.DBNull.Value)) {
                this.EmailBcc = ((string)(dr["EMAIL_BCC"]));
            }
            if ((dr["AUTOID"] != System.DBNull.Value)) {
                this.Autoid = ((System.Nullable<int>)(dr["AUTOID"]));
            }
        }
        
        public static MtmlDocPartyCollection GetAll() {
            MetroLesMonitor.Dal.MtmlDocParty dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocParty();
                System.Data.DataSet ds = dbo.MTML_DOC_PARTY_Select_All();
                MtmlDocPartyCollection collection = new MtmlDocPartyCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        MtmlDocParty obj = new MtmlDocParty();
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
        
        public static MtmlDocParty Load(System.Nullable<System.Guid> MTMLPARTYID) {
            MetroLesMonitor.Dal.MtmlDocParty dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocParty();
                System.Data.DataSet ds = dbo.MTML_DOC_PARTY_Select_One(MTMLPARTYID);
                MtmlDocParty obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new MtmlDocParty();
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
            MetroLesMonitor.Dal.MtmlDocParty dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocParty();
                System.Data.DataSet ds = dbo.MTML_DOC_PARTY_Select_One(this.Mtmlpartyid);
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
            MetroLesMonitor.Dal.MtmlDocParty dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocParty();
                dbo.MTML_DOC_PARTY_Insert(this.Mtmldocid, this.Qualifier, this.PartyName, this.Identification, this.Streetaddress, this.City, this.Postcode, this.State, this.Countrycode, this.Functioncode, this.ContactName, this.Phonenumber, this.Fax, this.Email, this.PartyLocation, this.Streetaddress2, this.Streetaddress3, this.EmailCc, this.EmailBcc);
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
            MetroLesMonitor.Dal.MtmlDocParty dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocParty();
                dbo.MTML_DOC_PARTY_Delete(this.Mtmlpartyid);
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
            MetroLesMonitor.Dal.MtmlDocParty dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocParty();
                dbo.MTML_DOC_PARTY_Update(this.Mtmlpartyid, this.Mtmldocid, this.Qualifier, this.PartyName, this.Identification, this.Streetaddress, this.City, this.Postcode, this.State, this.Countrycode, this.Functioncode, this.ContactName, this.Phonenumber, this.Fax, this.Email, this.PartyLocation, this.Streetaddress2, this.Streetaddress3, this.EmailCc, this.EmailBcc, this.Autoid);
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
