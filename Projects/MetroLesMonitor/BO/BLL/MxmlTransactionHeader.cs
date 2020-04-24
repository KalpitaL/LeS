namespace MetroLesMonitor.Bll {
    
    
    public partial class MxmlTransactionHeader {
        
        private System.Nullable<System.Guid> _mxmldocid;
        
        private string _payloadid;
        
        private string _serverTimestamp;
        
        private string _fromPartyid;
        
        private string _fromAdapteruid;
        
        private string _toPartyid;
        
        private string _doctype;
        
        private string _useragent;
        
        private MxmlAddressCollection _mxmlAddressCollection;
        
        private MxmlDocItemsCollection _mxmlDocItemsCollection;
        
        public virtual System.Nullable<System.Guid> Mxmldocid {
            get {
                return _mxmldocid;
            }
            set {
                _mxmldocid = value;
            }
        }
        
        public virtual string Payloadid {
            get {
                return _payloadid;
            }
            set {
                _payloadid = value;
            }
        }
        
        public virtual string ServerTimestamp {
            get {
                return _serverTimestamp;
            }
            set {
                _serverTimestamp = value;
            }
        }
        
        public virtual string FromPartyid {
            get {
                return _fromPartyid;
            }
            set {
                _fromPartyid = value;
            }
        }
        
        public virtual string FromAdapteruid {
            get {
                return _fromAdapteruid;
            }
            set {
                _fromAdapteruid = value;
            }
        }
        
        public virtual string ToPartyid {
            get {
                return _toPartyid;
            }
            set {
                _toPartyid = value;
            }
        }
        
        public virtual string Doctype {
            get {
                return _doctype;
            }
            set {
                _doctype = value;
            }
        }
        
        public virtual string Useragent {
            get {
                return _useragent;
            }
            set {
                _useragent = value;
            }
        }
        
        public virtual MxmlAddressCollection MxmlAddressCollection {
            get {
                if ((this._mxmlAddressCollection == null)) {
                    _mxmlAddressCollection = MetroLesMonitor.Bll.MxmlAddress.Select_MXML_ADDRESSs_By_MXMLDOCID(this.Mxmldocid);
                }
                return this._mxmlAddressCollection;
            }
        }
        
        public virtual MxmlDocItemsCollection MxmlDocItemsCollection {
            get {
                if ((this._mxmlDocItemsCollection == null)) {
                    _mxmlDocItemsCollection = MetroLesMonitor.Bll.MxmlDocItems.Select_MXML_DOC_ITEMSs_By_MXMLDOCID(this.Mxmldocid);
                }
                return this._mxmlDocItemsCollection;
            }
        }
        
        private void Clean() {
            this.Mxmldocid = null;
            this.Payloadid = string.Empty;
            this.ServerTimestamp = string.Empty;
            this.FromPartyid = string.Empty;
            this.FromAdapteruid = string.Empty;
            this.ToPartyid = string.Empty;
            this.Doctype = string.Empty;
            this.Useragent = string.Empty;
            this._mxmlAddressCollection = null;
            this._mxmlDocItemsCollection = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["MXMLDOCID"] != System.DBNull.Value)) {
                this.Mxmldocid = ((System.Nullable<System.Guid>)(dr["MXMLDOCID"]));
            }
            if ((dr["PAYLOADID"] != System.DBNull.Value)) {
                this.Payloadid = ((string)(dr["PAYLOADID"]));
            }
            if ((dr["SERVER_TIMESTAMP"] != System.DBNull.Value)) {
                this.ServerTimestamp = ((string)(dr["SERVER_TIMESTAMP"]));
            }
            if ((dr["FROM_PARTYID"] != System.DBNull.Value)) {
                this.FromPartyid = ((string)(dr["FROM_PARTYID"]));
            }
            if ((dr["FROM_ADAPTERUID"] != System.DBNull.Value)) {
                this.FromAdapteruid = ((string)(dr["FROM_ADAPTERUID"]));
            }
            if ((dr["TO_PARTYID"] != System.DBNull.Value)) {
                this.ToPartyid = ((string)(dr["TO_PARTYID"]));
            }
            if ((dr["DOCTYPE"] != System.DBNull.Value)) {
                this.Doctype = ((string)(dr["DOCTYPE"]));
            }
            if ((dr["USERAGENT"] != System.DBNull.Value)) {
                this.Useragent = ((string)(dr["USERAGENT"]));
            }
        }
        
        public static MxmlTransactionHeaderCollection GetAll() {
            MetroLesMonitor.Dal.MxmlTransactionHeader dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlTransactionHeader();
                System.Data.DataSet ds = dbo.MXML_TRANSACTION_HEADER_Select_All();
                MxmlTransactionHeaderCollection collection = new MxmlTransactionHeaderCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        MxmlTransactionHeader obj = new MxmlTransactionHeader();
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
        
        public static MxmlTransactionHeader Load(System.Nullable<System.Guid> MXMLDOCID) {
            MetroLesMonitor.Dal.MxmlTransactionHeader dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlTransactionHeader();
                System.Data.DataSet ds = dbo.MXML_TRANSACTION_HEADER_Select_One(MXMLDOCID);
                MxmlTransactionHeader obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new MxmlTransactionHeader();
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
            MetroLesMonitor.Dal.MxmlTransactionHeader dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlTransactionHeader();
                System.Data.DataSet ds = dbo.MXML_TRANSACTION_HEADER_Select_One(this.Mxmldocid);
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
            MetroLesMonitor.Dal.MxmlTransactionHeader dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlTransactionHeader();
                dbo.MXML_TRANSACTION_HEADER_Insert(this.Payloadid, this.ServerTimestamp, this.FromPartyid, this.FromAdapteruid, this.ToPartyid, this.Doctype, this.Useragent);
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
            MetroLesMonitor.Dal.MxmlTransactionHeader dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlTransactionHeader();
                dbo.MXML_TRANSACTION_HEADER_Delete(this.Mxmldocid);
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
            MetroLesMonitor.Dal.MxmlTransactionHeader dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlTransactionHeader();
                dbo.MXML_TRANSACTION_HEADER_Update(this.Mxmldocid, this.Payloadid, this.ServerTimestamp, this.FromPartyid, this.FromAdapteruid, this.ToPartyid, this.Doctype, this.Useragent);
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
