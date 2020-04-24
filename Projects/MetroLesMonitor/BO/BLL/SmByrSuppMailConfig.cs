namespace MetroLesMonitor.Bll {
    
    
    public partial class SmByrSuppMailConfig {
        
        private System.Nullable<int> _configId;
        
        private System.Nullable<int> _byrSupLinkid;
        
        private System.Nullable<int> _buyerId;
        
        private System.Nullable<int> _supplierId;
        
        private string _smtpHost;
        
        private System.Nullable<int> _smtpPort;
        
        private string _fromEmail;
        
        private string _fromUser;
        
        private string _fromPwd;
        
        private string _displayName;
        
        private string _replyEmail;
        
        private System.Nullable<int> _isSsl;
        
        private System.Nullable<int> _isAuthorised;
        
        private string _udf1;
        
        private string _udf2;
        
        public virtual System.Nullable<int> ConfigId {
            get {
                return _configId;
            }
            set {
                _configId = value;
            }
        }
        
        public virtual System.Nullable<int> ByrSupLinkid {
            get {
                return _byrSupLinkid;
            }
            set {
                _byrSupLinkid = value;
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
        
        public virtual string SmtpHost {
            get {
                return _smtpHost;
            }
            set {
                _smtpHost = value;
            }
        }
        
        public virtual System.Nullable<int> SmtpPort {
            get {
                return _smtpPort;
            }
            set {
                _smtpPort = value;
            }
        }
        
        public virtual string FromEmail {
            get {
                return _fromEmail;
            }
            set {
                _fromEmail = value;
            }
        }
        
        public virtual string FromUser {
            get {
                return _fromUser;
            }
            set {
                _fromUser = value;
            }
        }
        
        public virtual string FromPwd {
            get {
                return _fromPwd;
            }
            set {
                _fromPwd = value;
            }
        }
        
        public virtual string DisplayName {
            get {
                return _displayName;
            }
            set {
                _displayName = value;
            }
        }
        
        public virtual string ReplyEmail {
            get {
                return _replyEmail;
            }
            set {
                _replyEmail = value;
            }
        }
        
        public virtual System.Nullable<int> IsSsl {
            get {
                return _isSsl;
            }
            set {
                _isSsl = value;
            }
        }
        
        public virtual System.Nullable<int> IsAuthorised {
            get {
                return _isAuthorised;
            }
            set {
                _isAuthorised = value;
            }
        }
        
        public virtual string Udf1 {
            get {
                return _udf1;
            }
            set {
                _udf1 = value;
            }
        }
        
        public virtual string Udf2 {
            get {
                return _udf2;
            }
            set {
                _udf2 = value;
            }
        }
        
        private void Clean() {
            this.ConfigId = null;
            this.ByrSupLinkid = null;
            this.BuyerId = null;
            this.SupplierId = null;
            this.SmtpHost = string.Empty;
            this.SmtpPort = null;
            this.FromEmail = string.Empty;
            this.FromUser = string.Empty;
            this.FromPwd = string.Empty;
            this.DisplayName = string.Empty;
            this.ReplyEmail = string.Empty;
            this.IsSsl = null;
            this.IsAuthorised = null;
            this.Udf1 = string.Empty;
            this.Udf2 = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["CONFIG_ID"] != System.DBNull.Value)) {
                this.ConfigId = ((System.Nullable<int>)(dr["CONFIG_ID"]));
            }
            if ((dr["BYR_SUP_LINKID"] != System.DBNull.Value)) {
                this.ByrSupLinkid = ((System.Nullable<int>)(dr["BYR_SUP_LINKID"]));
            }
            if ((dr["BUYER_ID"] != System.DBNull.Value)) {
                this.BuyerId = ((System.Nullable<int>)(dr["BUYER_ID"]));
            }
            if ((dr["SUPPLIER_ID"] != System.DBNull.Value)) {
                this.SupplierId = ((System.Nullable<int>)(dr["SUPPLIER_ID"]));
            }
            if ((dr["SMTP_HOST"] != System.DBNull.Value)) {
                this.SmtpHost = ((string)(dr["SMTP_HOST"]));
            }
            if ((dr["SMTP_PORT"] != System.DBNull.Value)) {
                this.SmtpPort = ((System.Nullable<int>)(dr["SMTP_PORT"]));
            }
            if ((dr["FROM_EMAIL"] != System.DBNull.Value)) {
                this.FromEmail = ((string)(dr["FROM_EMAIL"]));
            }
            if ((dr["FROM_USER"] != System.DBNull.Value)) {
                this.FromUser = ((string)(dr["FROM_USER"]));
            }
            if ((dr["FROM_PWD"] != System.DBNull.Value)) {
                this.FromPwd = ((string)(dr["FROM_PWD"]));
            }
            if ((dr["DISPLAY_NAME"] != System.DBNull.Value)) {
                this.DisplayName = ((string)(dr["DISPLAY_NAME"]));
            }
            if ((dr["REPLY_EMAIL"] != System.DBNull.Value)) {
                this.ReplyEmail = ((string)(dr["REPLY_EMAIL"]));
            }
            if ((dr["IS_SSL"] != System.DBNull.Value)) {
                this.IsSsl = ((System.Nullable<int>)(dr["IS_SSL"]));
            }
            if ((dr["IS_AUTHORISED"] != System.DBNull.Value)) {
                this.IsAuthorised = ((System.Nullable<int>)(dr["IS_AUTHORISED"]));
            }
            if ((dr["UDF1"] != System.DBNull.Value)) {
                this.Udf1 = ((string)(dr["UDF1"]));
            }
            if ((dr["UDF2"] != System.DBNull.Value)) {
                this.Udf2 = ((string)(dr["UDF2"]));
            }
        }
        
        public static SmByrSuppMailConfigCollection GetAll() {
            MetroLesMonitor.Dal.SmByrSuppMailConfig dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmByrSuppMailConfig();
                System.Data.DataSet ds = dbo.SM_BYR_SUPP_MAIL_CONFIG_Select_All();
                SmByrSuppMailConfigCollection collection = new SmByrSuppMailConfigCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmByrSuppMailConfig obj = new SmByrSuppMailConfig();
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
        
        public static SmByrSuppMailConfig Load(System.Nullable<int> CONFIG_ID) {
            MetroLesMonitor.Dal.SmByrSuppMailConfig dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmByrSuppMailConfig();
                System.Data.DataSet ds = dbo.SM_BYR_SUPP_MAIL_CONFIG_Select_One(CONFIG_ID);
                SmByrSuppMailConfig obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmByrSuppMailConfig();
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
            MetroLesMonitor.Dal.SmByrSuppMailConfig dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmByrSuppMailConfig();
                System.Data.DataSet ds = dbo.SM_BYR_SUPP_MAIL_CONFIG_Select_One(this.ConfigId);
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
            MetroLesMonitor.Dal.SmByrSuppMailConfig dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmByrSuppMailConfig();
                dbo.SM_BYR_SUPP_MAIL_CONFIG_Insert(this.ByrSupLinkid, this.BuyerId, this.SupplierId, this.SmtpHost, this.SmtpPort, this.FromEmail, this.FromUser, this.FromPwd, this.DisplayName, this.ReplyEmail, this.IsSsl, this.IsAuthorised, this.Udf1, this.Udf2);
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
            MetroLesMonitor.Dal.SmByrSuppMailConfig dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmByrSuppMailConfig();
                dbo.SM_BYR_SUPP_MAIL_CONFIG_Delete(this.ConfigId);
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
            MetroLesMonitor.Dal.SmByrSuppMailConfig dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmByrSuppMailConfig();
                dbo.SM_BYR_SUPP_MAIL_CONFIG_Update(this.ConfigId, this.ByrSupLinkid, this.BuyerId, this.SupplierId, this.SmtpHost, this.SmtpPort, this.FromEmail, this.FromUser, this.FromPwd, this.DisplayName, this.ReplyEmail, this.IsSsl, this.IsAuthorised, this.Udf1, this.Udf2);
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
