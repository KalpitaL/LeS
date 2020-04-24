namespace MetroLesMonitor.Bll {
    
    
    public partial class SmMailTransactionLog {
        
        private System.Nullable<int> _transId;
        
        private string _moduleName;
        
        private string _logType;
        
        private string _fromMail;
        
        private string _toMail;
        
        private string _keyRefno;
        
        private System.Nullable<int> _linkid;
        
        private System.Nullable<int> _buyerId;
        
        private System.Nullable<int> _supplierId;
        
        private string _msgfile;
        
        private string _file1;
        
        private string _file2;
        
        private string _mailSubject;
        
        private System.Nullable<int> _transStatus;
        
        private System.Nullable<System.DateTime> _updatedate;
        
        private string _comments;
        
        public virtual System.Nullable<int> TransId {
            get {
                return _transId;
            }
            set {
                _transId = value;
            }
        }
        
        public virtual string ModuleName {
            get {
                return _moduleName;
            }
            set {
                _moduleName = value;
            }
        }
        
        public virtual string LogType {
            get {
                return _logType;
            }
            set {
                _logType = value;
            }
        }
        
        public virtual string FromMail {
            get {
                return _fromMail;
            }
            set {
                _fromMail = value;
            }
        }
        
        public virtual string ToMail {
            get {
                return _toMail;
            }
            set {
                _toMail = value;
            }
        }
        
        public virtual string KeyRefno {
            get {
                return _keyRefno;
            }
            set {
                _keyRefno = value;
            }
        }
        
        public virtual System.Nullable<int> Linkid {
            get {
                return _linkid;
            }
            set {
                _linkid = value;
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
        
        public virtual string Msgfile {
            get {
                return _msgfile;
            }
            set {
                _msgfile = value;
            }
        }
        
        public virtual string File1 {
            get {
                return _file1;
            }
            set {
                _file1 = value;
            }
        }
        
        public virtual string File2 {
            get {
                return _file2;
            }
            set {
                _file2 = value;
            }
        }
        
        public virtual string MailSubject {
            get {
                return _mailSubject;
            }
            set {
                _mailSubject = value;
            }
        }
        
        public virtual System.Nullable<int> TransStatus {
            get {
                return _transStatus;
            }
            set {
                _transStatus = value;
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
        
        public virtual string Comments {
            get {
                return _comments;
            }
            set {
                _comments = value;
            }
        }
        
        private void Clean() {
            this.TransId = null;
            this.ModuleName = string.Empty;
            this.LogType = string.Empty;
            this.FromMail = string.Empty;
            this.ToMail = string.Empty;
            this.KeyRefno = string.Empty;
            this.Linkid = null;
            this.BuyerId = null;
            this.SupplierId = null;
            this.Msgfile = string.Empty;
            this.File1 = string.Empty;
            this.File2 = string.Empty;
            this.MailSubject = string.Empty;
            this.TransStatus = null;
            this.Updatedate = null;
            this.Comments = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["TRANS_ID"] != System.DBNull.Value)) {
                this.TransId = ((System.Nullable<int>)(dr["TRANS_ID"]));
            }
            if ((dr["MODULE_NAME"] != System.DBNull.Value)) {
                this.ModuleName = ((string)(dr["MODULE_NAME"]));
            }
            if ((dr["LOG_TYPE"] != System.DBNull.Value)) {
                this.LogType = ((string)(dr["LOG_TYPE"]));
            }
            if ((dr["FROM_MAIL"] != System.DBNull.Value)) {
                this.FromMail = ((string)(dr["FROM_MAIL"]));
            }
            if ((dr["TO_MAIL"] != System.DBNull.Value)) {
                this.ToMail = ((string)(dr["TO_MAIL"]));
            }
            if ((dr["KEY_REFNO"] != System.DBNull.Value)) {
                this.KeyRefno = ((string)(dr["KEY_REFNO"]));
            }
            if ((dr["LINKID"] != System.DBNull.Value)) {
                this.Linkid = ((System.Nullable<int>)(dr["LINKID"]));
            }
            if ((dr["BUYER_ID"] != System.DBNull.Value)) {
                this.BuyerId = ((System.Nullable<int>)(dr["BUYER_ID"]));
            }
            if ((dr["SUPPLIER_ID"] != System.DBNull.Value)) {
                this.SupplierId = ((System.Nullable<int>)(dr["SUPPLIER_ID"]));
            }
            if ((dr["MSGFILE"] != System.DBNull.Value)) {
                this.Msgfile = ((string)(dr["MSGFILE"]));
            }
            if ((dr["FILE1"] != System.DBNull.Value)) {
                this.File1 = ((string)(dr["FILE1"]));
            }
            if ((dr["FILE2"] != System.DBNull.Value)) {
                this.File2 = ((string)(dr["FILE2"]));
            }
            if ((dr["MAIL_SUBJECT"] != System.DBNull.Value)) {
                this.MailSubject = ((string)(dr["MAIL_SUBJECT"]));
            }
            if ((dr["TRANS_STATUS"] != System.DBNull.Value)) {
                this.TransStatus = ((System.Nullable<int>)(dr["TRANS_STATUS"]));
            }
            if ((dr["UPDATEDATE"] != System.DBNull.Value)) {
                this.Updatedate = ((System.Nullable<System.DateTime>)(dr["UPDATEDATE"]));
            }
            if ((dr["COMMENTS"] != System.DBNull.Value)) {
                this.Comments = ((string)(dr["COMMENTS"]));
            }
        }
        
        public static SmMailTransactionLogCollection GetAll() {
            MetroLesMonitor.Dal.SmMailTransactionLog dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmMailTransactionLog();
                System.Data.DataSet ds = dbo.SM_MAIL_TRANSACTION_LOG_Select_All();
                SmMailTransactionLogCollection collection = new SmMailTransactionLogCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmMailTransactionLog obj = new SmMailTransactionLog();
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
        
        public static SmMailTransactionLog Load(System.Nullable<int> TRANS_ID) {
            MetroLesMonitor.Dal.SmMailTransactionLog dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmMailTransactionLog();
                System.Data.DataSet ds = dbo.SM_MAIL_TRANSACTION_LOG_Select_One(TRANS_ID);
                SmMailTransactionLog obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmMailTransactionLog();
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
            MetroLesMonitor.Dal.SmMailTransactionLog dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmMailTransactionLog();
                System.Data.DataSet ds = dbo.SM_MAIL_TRANSACTION_LOG_Select_One(this.TransId);
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
            MetroLesMonitor.Dal.SmMailTransactionLog dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmMailTransactionLog();
                dbo.SM_MAIL_TRANSACTION_LOG_Insert(this.ModuleName, this.LogType, this.FromMail, this.ToMail, this.KeyRefno, this.Linkid, this.BuyerId, this.SupplierId, this.Msgfile, this.File1, this.File2, this.MailSubject, this.TransStatus, this.Updatedate, this.Comments);
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
            MetroLesMonitor.Dal.SmMailTransactionLog dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmMailTransactionLog();
                dbo.SM_MAIL_TRANSACTION_LOG_Delete(this.TransId);
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
            MetroLesMonitor.Dal.SmMailTransactionLog dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmMailTransactionLog();
                dbo.SM_MAIL_TRANSACTION_LOG_Update(this.TransId, this.ModuleName, this.LogType, this.FromMail, this.ToMail, this.KeyRefno, this.Linkid, this.BuyerId, this.SupplierId, this.Msgfile, this.File1, this.File2, this.MailSubject, this.TransStatus, this.Updatedate, this.Comments);
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
