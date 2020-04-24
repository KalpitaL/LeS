namespace MetroLesMonitor.Bll {
    
    
    public partial class MtmlTransactionHeader {
        
        private System.Nullable<System.Guid> _mtmldocid;
        
        private string _controlreference;
        
        private string _identifier;
        
        private string _preparationdate;
        
        private string _preparationtime;
        
        private string _recipeint;
        
        private string _recipientcodequalifier;
        
        private string _sender;
        
        private string _sendercodequalifier;
        
        private string _versionnumber;
        
        private string _referencenumber;
        
        private string _doctype;
        
        private System.Nullable<int> _autoid;
        
        public virtual System.Nullable<System.Guid> Mtmldocid {
            get {
                return _mtmldocid;
            }
            set {
                _mtmldocid = value;
            }
        }
        
        public virtual string Controlreference {
            get {
                return _controlreference;
            }
            set {
                _controlreference = value;
            }
        }
        
        public virtual string Identifier {
            get {
                return _identifier;
            }
            set {
                _identifier = value;
            }
        }
        
        public virtual string Preparationdate {
            get {
                return _preparationdate;
            }
            set {
                _preparationdate = value;
            }
        }
        
        public virtual string Preparationtime {
            get {
                return _preparationtime;
            }
            set {
                _preparationtime = value;
            }
        }
        
        public virtual string Recipeint {
            get {
                return _recipeint;
            }
            set {
                _recipeint = value;
            }
        }
        
        public virtual string Recipientcodequalifier {
            get {
                return _recipientcodequalifier;
            }
            set {
                _recipientcodequalifier = value;
            }
        }
        
        public virtual string Sender {
            get {
                return _sender;
            }
            set {
                _sender = value;
            }
        }
        
        public virtual string Sendercodequalifier {
            get {
                return _sendercodequalifier;
            }
            set {
                _sendercodequalifier = value;
            }
        }
        
        public virtual string Versionnumber {
            get {
                return _versionnumber;
            }
            set {
                _versionnumber = value;
            }
        }
        
        public virtual string Referencenumber {
            get {
                return _referencenumber;
            }
            set {
                _referencenumber = value;
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
        
        public virtual System.Nullable<int> Autoid {
            get {
                return _autoid;
            }
            set {
                _autoid = value;
            }
        }
        
        private void Clean() {
            this.Mtmldocid = null;
            this.Controlreference = string.Empty;
            this.Identifier = string.Empty;
            this.Preparationdate = string.Empty;
            this.Preparationtime = string.Empty;
            this.Recipeint = string.Empty;
            this.Recipientcodequalifier = string.Empty;
            this.Sender = string.Empty;
            this.Sendercodequalifier = string.Empty;
            this.Versionnumber = string.Empty;
            this.Referencenumber = string.Empty;
            this.Doctype = string.Empty;
            this.Autoid = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["MTMLDOCID"] != System.DBNull.Value)) {
                this.Mtmldocid = ((System.Nullable<System.Guid>)(dr["MTMLDOCID"]));
            }
            if ((dr["CONTROLREFERENCE"] != System.DBNull.Value)) {
                this.Controlreference = ((string)(dr["CONTROLREFERENCE"]));
            }
            if ((dr["IDENTIFIER"] != System.DBNull.Value)) {
                this.Identifier = ((string)(dr["IDENTIFIER"]));
            }
            if ((dr["PREPARATIONDATE"] != System.DBNull.Value)) {
                this.Preparationdate = ((string)(dr["PREPARATIONDATE"]));
            }
            if ((dr["PREPARATIONTIME"] != System.DBNull.Value)) {
                this.Preparationtime = ((string)(dr["PREPARATIONTIME"]));
            }
            if ((dr["RECIPEINT"] != System.DBNull.Value)) {
                this.Recipeint = ((string)(dr["RECIPEINT"]));
            }
            if ((dr["RECIPIENTCODEQUALIFIER"] != System.DBNull.Value)) {
                this.Recipientcodequalifier = ((string)(dr["RECIPIENTCODEQUALIFIER"]));
            }
            if ((dr["SENDER"] != System.DBNull.Value)) {
                this.Sender = ((string)(dr["SENDER"]));
            }
            if ((dr["SENDERCODEQUALIFIER"] != System.DBNull.Value)) {
                this.Sendercodequalifier = ((string)(dr["SENDERCODEQUALIFIER"]));
            }
            if ((dr["VERSIONNUMBER"] != System.DBNull.Value)) {
                this.Versionnumber = ((string)(dr["VERSIONNUMBER"]));
            }
            if ((dr["ReferenceNumber"] != System.DBNull.Value)) {
                this.Referencenumber = ((string)(dr["ReferenceNumber"]));
            }
            if ((dr["DOCType"] != System.DBNull.Value)) {
                this.Doctype = ((string)(dr["DOCType"]));
            }
            if ((dr["AUTOID"] != System.DBNull.Value)) {
                this.Autoid = ((System.Nullable<int>)(dr["AUTOID"]));
            }
        }
        
        public static MtmlTransactionHeaderCollection GetAll() {
            MetroLesMonitor.Dal.MtmlTransactionHeader dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlTransactionHeader();
                System.Data.DataSet ds = dbo.MTML_TRANSACTION_HEADER_Select_All();
                MtmlTransactionHeaderCollection collection = new MtmlTransactionHeaderCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        MtmlTransactionHeader obj = new MtmlTransactionHeader();
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
        
        public static MtmlTransactionHeader Load(System.Nullable<System.Guid> MTMLDOCID) {
            MetroLesMonitor.Dal.MtmlTransactionHeader dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlTransactionHeader();
                System.Data.DataSet ds = dbo.MTML_TRANSACTION_HEADER_Select_One(MTMLDOCID);
                MtmlTransactionHeader obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new MtmlTransactionHeader();
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
            MetroLesMonitor.Dal.MtmlTransactionHeader dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlTransactionHeader();
                System.Data.DataSet ds = dbo.MTML_TRANSACTION_HEADER_Select_One(this.Mtmldocid);
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
            MetroLesMonitor.Dal.MtmlTransactionHeader dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlTransactionHeader();
                dbo.MTML_TRANSACTION_HEADER_Insert(this.Controlreference, this.Identifier, this.Preparationdate, this.Preparationtime, this.Recipeint, this.Recipientcodequalifier, this.Sender, this.Sendercodequalifier, this.Versionnumber, this.Referencenumber, this.Doctype);
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
            MetroLesMonitor.Dal.MtmlTransactionHeader dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlTransactionHeader();
                dbo.MTML_TRANSACTION_HEADER_Delete(this.Mtmldocid);
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
            MetroLesMonitor.Dal.MtmlTransactionHeader dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlTransactionHeader();
                dbo.MTML_TRANSACTION_HEADER_Update(this.Mtmldocid, this.Controlreference, this.Identifier, this.Preparationdate, this.Preparationtime, this.Recipeint, this.Recipientcodequalifier, this.Sender, this.Sendercodequalifier, this.Versionnumber, this.Referencenumber, this.Doctype, this.Autoid);
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
