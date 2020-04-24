namespace MetroLesMonitor.Bll {
    
    
    public partial class SmMailDownloadLog {
        
        private System.Nullable<int> _logid;
        
        private string _messageid;
        
        private string _modulename;
        
        private string _logtype;
        
        private string _frommail;
        
        private string _tomail;
        
        private string _auditvalue;
        
        private string _keyref1;
        
        private string _keyref2;
        
        private System.Nullable<System.DateTime> _updatedate;
        
        private System.Nullable<int> _buyerId;
        
        private System.Nullable<int> _supplierId;
        
        private string _mailsubject;
        
        public virtual System.Nullable<int> Logid {
            get {
                return _logid;
            }
            set {
                _logid = value;
            }
        }
        
        public virtual string Messageid {
            get {
                return _messageid;
            }
            set {
                _messageid = value;
            }
        }
        
        public virtual string Modulename {
            get {
                return _modulename;
            }
            set {
                _modulename = value;
            }
        }
        
        public virtual string Logtype {
            get {
                return _logtype;
            }
            set {
                _logtype = value;
            }
        }
        
        public virtual string Frommail {
            get {
                return _frommail;
            }
            set {
                _frommail = value;
            }
        }
        
        public virtual string Tomail {
            get {
                return _tomail;
            }
            set {
                _tomail = value;
            }
        }
        
        public virtual string Auditvalue {
            get {
                return _auditvalue;
            }
            set {
                _auditvalue = value;
            }
        }
        
        public virtual string Keyref1 {
            get {
                return _keyref1;
            }
            set {
                _keyref1 = value;
            }
        }
        
        public virtual string Keyref2 {
            get {
                return _keyref2;
            }
            set {
                _keyref2 = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> Updatedate {
            get {
                return _updatedate;
            }
            set {
                _updatedate = value;
            }
        }
        
        public virtual System.Nullable<int> BuyerId {
            get {
                return _buyerId;
            }
            set {
                _buyerId = value;
            }
        }
        
        public virtual System.Nullable<int> SupplierId {
            get {
                return _supplierId;
            }
            set {
                _supplierId = value;
            }
        }
        
        public virtual string Mailsubject {
            get {
                return _mailsubject;
            }
            set {
                _mailsubject = value;
            }
        }
        
        private void Clean() {
            this.Logid = null;
            this.Messageid = string.Empty;
            this.Modulename = string.Empty;
            this.Logtype = string.Empty;
            this.Frommail = string.Empty;
            this.Tomail = string.Empty;
            this.Auditvalue = string.Empty;
            this.Keyref1 = string.Empty;
            this.Keyref2 = string.Empty;
            this.Updatedate = null;
            this.BuyerId = null;
            this.SupplierId = null;
            this.Mailsubject = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["LogID"] != System.DBNull.Value)) {
                this.Logid = ((System.Nullable<int>)(dr["LogID"]));
            }
            if ((dr["MessageID"] != System.DBNull.Value)) {
                this.Messageid = ((string)(dr["MessageID"]));
            }
            if ((dr["ModuleName"] != System.DBNull.Value)) {
                this.Modulename = ((string)(dr["ModuleName"]));
            }
            if ((dr["LogType"] != System.DBNull.Value)) {
                this.Logtype = ((string)(dr["LogType"]));
            }
            if ((dr["FromMail"] != System.DBNull.Value)) {
                this.Frommail = ((string)(dr["FromMail"]));
            }
            if ((dr["ToMail"] != System.DBNull.Value)) {
                this.Tomail = ((string)(dr["ToMail"]));
            }
            if ((dr["AuditValue"] != System.DBNull.Value)) {
                this.Auditvalue = ((string)(dr["AuditValue"]));
            }
            if ((dr["KeyRef1"] != System.DBNull.Value)) {
                this.Keyref1 = ((string)(dr["KeyRef1"]));
            }
            if ((dr["KeyRef2"] != System.DBNull.Value)) {
                this.Keyref2 = ((string)(dr["KeyRef2"]));
            }
            if ((dr["UpdateDate"] != System.DBNull.Value)) {
                this.Updatedate = ((System.Nullable<System.DateTime>)(dr["UpdateDate"]));
            }
            if ((dr["BUYER_ID"] != System.DBNull.Value)) {
                this.BuyerId = ((System.Nullable<int>)(dr["BUYER_ID"]));
            }
            if ((dr["SUPPLIER_ID"] != System.DBNull.Value)) {
                this.SupplierId = ((System.Nullable<int>)(dr["SUPPLIER_ID"]));
            }
            if ((dr["MailSubject"] != System.DBNull.Value)) {
                this.Mailsubject = ((string)(dr["MailSubject"]));
            }
        }
        
        public static SmMailDownloadLogCollection GetAll() {
            MetroLesMonitor.Dal.SmMailDownloadLog dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmMailDownloadLog();
                System.Data.DataSet ds = dbo.SM_MAIL_DOWNLOAD_LOG_Select_All();
                SmMailDownloadLogCollection collection = new SmMailDownloadLogCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmMailDownloadLog obj = new SmMailDownloadLog();
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
        
        public static SmMailDownloadLog Load(System.Nullable<int> LogID) {
            MetroLesMonitor.Dal.SmMailDownloadLog dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmMailDownloadLog();
                System.Data.DataSet ds = dbo.SM_MAIL_DOWNLOAD_LOG_Select_One(LogID);
                SmMailDownloadLog obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmMailDownloadLog();
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
            MetroLesMonitor.Dal.SmMailDownloadLog dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmMailDownloadLog();
                System.Data.DataSet ds = dbo.SM_MAIL_DOWNLOAD_LOG_Select_One(this.Logid);
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
            MetroLesMonitor.Dal.SmMailDownloadLog dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmMailDownloadLog();
                dbo.SM_MAIL_DOWNLOAD_LOG_Insert(this.Messageid, this.Modulename, this.Logtype, this.Frommail, this.Tomail, this.Auditvalue, this.Keyref1, this.Keyref2, this.Updatedate, this.BuyerId, this.SupplierId, this.Mailsubject);
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
            MetroLesMonitor.Dal.SmMailDownloadLog dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmMailDownloadLog();
                dbo.SM_MAIL_DOWNLOAD_LOG_Delete(this.Logid);
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
            MetroLesMonitor.Dal.SmMailDownloadLog dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmMailDownloadLog();
                dbo.SM_MAIL_DOWNLOAD_LOG_Update(this.Logid, this.Messageid, this.Modulename, this.Logtype, this.Frommail, this.Tomail, this.Auditvalue, this.Keyref1, this.Keyref2, this.Updatedate, this.BuyerId, this.SupplierId, this.Mailsubject);
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
