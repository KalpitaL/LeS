namespace MetroLesMonitor.Bll {
    
    
    public partial class SmSendMailAudit {
        
        private System.Nullable<int> _mailAuditid;
        
        private System.Nullable<int> _quotationid;
        
        private System.Nullable<int> _byrSuppLinkid;
        
        private string _mailTo;
        
        private string _mailCc;
        
        private string _mailBcc;
        
        private string _mailReply;
        
        private string _mailSubject;
        
        private string _mailBody;
        
        private string _mailAttachments;
        
        private System.Nullable<System.DateTime> _mailDate;
        
        private System.Nullable<System.DateTime> _receivedDate;
        
        private string _emlFile;
        
        private string _mailGuid;
        
        private string _remarks;
        
        public virtual System.Nullable<int> MailAuditid {
            get {
                return _mailAuditid;
            }
            set {
                _mailAuditid = value;
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
        
        public virtual System.Nullable<int> ByrSuppLinkid {
            get {
                return _byrSuppLinkid;
            }
            set {
                _byrSuppLinkid = value;
            }
        }
        
        public virtual string MailTo {
            get {
                return _mailTo;
            }
            set {
                _mailTo = value;
            }
        }
        
        public virtual string MailCc {
            get {
                return _mailCc;
            }
            set {
                _mailCc = value;
            }
        }
        
        public virtual string MailBcc {
            get {
                return _mailBcc;
            }
            set {
                _mailBcc = value;
            }
        }
        
        public virtual string MailReply {
            get {
                return _mailReply;
            }
            set {
                _mailReply = value;
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
        
        public virtual string MailBody {
            get {
                return _mailBody;
            }
            set {
                _mailBody = value;
            }
        }
        
        public virtual string MailAttachments {
            get {
                return _mailAttachments;
            }
            set {
                _mailAttachments = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> MailDate {
            get {
                return _mailDate;
            }
            set {
                _mailDate = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> ReceivedDate {
            get {
                return _receivedDate;
            }
            set {
                _receivedDate = value;
            }
        }
        
        public virtual string EmlFile {
            get {
                return _emlFile;
            }
            set {
                _emlFile = value;
            }
        }
        
        public virtual string MailGuid {
            get {
                return _mailGuid;
            }
            set {
                _mailGuid = value;
            }
        }
        
        public virtual string Remarks {
            get {
                return _remarks;
            }
            set {
                _remarks = value;
            }
        }
        
        private void Clean() {
            this.MailAuditid = null;
            this.Quotationid = null;
            this.ByrSuppLinkid = null;
            this.MailTo = string.Empty;
            this.MailCc = string.Empty;
            this.MailBcc = string.Empty;
            this.MailReply = string.Empty;
            this.MailSubject = string.Empty;
            this.MailBody = string.Empty;
            this.MailAttachments = string.Empty;
            this.MailDate = null;
            this.ReceivedDate = null;
            this.EmlFile = string.Empty;
            this.MailGuid = string.Empty;
            this.Remarks = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["MAIL_AUDITID"] != System.DBNull.Value)) {
                this.MailAuditid = ((System.Nullable<int>)(dr["MAIL_AUDITID"]));
            }
            if ((dr["QUOTATIONID"] != System.DBNull.Value)) {
                this.Quotationid = ((System.Nullable<int>)(dr["QUOTATIONID"]));
            }
            if ((dr["BYR_SUPP_LINKID"] != System.DBNull.Value)) {
                this.ByrSuppLinkid = ((System.Nullable<int>)(dr["BYR_SUPP_LINKID"]));
            }
            if ((dr["MAIL_TO"] != System.DBNull.Value)) {
                this.MailTo = ((string)(dr["MAIL_TO"]));
            }
            if ((dr["MAIL_CC"] != System.DBNull.Value)) {
                this.MailCc = ((string)(dr["MAIL_CC"]));
            }
            if ((dr["MAIL_BCC"] != System.DBNull.Value)) {
                this.MailBcc = ((string)(dr["MAIL_BCC"]));
            }
            if ((dr["MAIL_REPLY"] != System.DBNull.Value)) {
                this.MailReply = ((string)(dr["MAIL_REPLY"]));
            }
            if ((dr["MAIL_SUBJECT"] != System.DBNull.Value)) {
                this.MailSubject = ((string)(dr["MAIL_SUBJECT"]));
            }
            if ((dr["MAIL_BODY"] != System.DBNull.Value)) {
                this.MailBody = ((string)(dr["MAIL_BODY"]));
            }
            if ((dr["MAIL_ATTACHMENTS"] != System.DBNull.Value)) {
                this.MailAttachments = ((string)(dr["MAIL_ATTACHMENTS"]));
            }
            if ((dr["MAIL_DATE"] != System.DBNull.Value)) {
                this.MailDate = ((System.Nullable<System.DateTime>)(dr["MAIL_DATE"]));
            }
            if ((dr["RECEIVED_DATE"] != System.DBNull.Value)) {
                this.ReceivedDate = ((System.Nullable<System.DateTime>)(dr["RECEIVED_DATE"]));
            }
            if ((dr["EML_FILE"] != System.DBNull.Value)) {
                this.EmlFile = ((string)(dr["EML_FILE"]));
            }
            if ((dr["MAIL_GUID"] != System.DBNull.Value)) {
                this.MailGuid = ((string)(dr["MAIL_GUID"]));
            }
            if ((dr["REMARKS"] != System.DBNull.Value)) {
                this.Remarks = ((string)(dr["REMARKS"]));
            }
        }
        
        public static SmSendMailAuditCollection GetAll() {
            MetroLesMonitor.Dal.SmSendMailAudit dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmSendMailAudit();
                System.Data.DataSet ds = dbo.SM_SEND_MAIL_AUDIT_Select_All();
                SmSendMailAuditCollection collection = new SmSendMailAuditCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmSendMailAudit obj = new SmSendMailAudit();
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
        
        public static SmSendMailAudit Load(System.Nullable<int> MAIL_AUDITID) {
            MetroLesMonitor.Dal.SmSendMailAudit dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmSendMailAudit();
                System.Data.DataSet ds = dbo.SM_SEND_MAIL_AUDIT_Select_One(MAIL_AUDITID);
                SmSendMailAudit obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmSendMailAudit();
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
            MetroLesMonitor.Dal.SmSendMailAudit dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmSendMailAudit();
                System.Data.DataSet ds = dbo.SM_SEND_MAIL_AUDIT_Select_One(this.MailAuditid);
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
            MetroLesMonitor.Dal.SmSendMailAudit dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmSendMailAudit();
                dbo.SM_SEND_MAIL_AUDIT_Insert(this.Quotationid, this.ByrSuppLinkid, this.MailTo, this.MailCc, this.MailBcc, this.MailReply, this.MailSubject, this.MailBody, this.MailAttachments, this.MailDate, this.ReceivedDate, this.EmlFile, this.MailGuid, this.Remarks);
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
            MetroLesMonitor.Dal.SmSendMailAudit dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmSendMailAudit();
                dbo.SM_SEND_MAIL_AUDIT_Delete(this.MailAuditid);
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
            MetroLesMonitor.Dal.SmSendMailAudit dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmSendMailAudit();
                dbo.SM_SEND_MAIL_AUDIT_Update(this.MailAuditid, this.Quotationid, this.ByrSuppLinkid, this.MailTo, this.MailCc, this.MailBcc, this.MailReply, this.MailSubject, this.MailBody, this.MailAttachments, this.MailDate, this.ReceivedDate, this.EmlFile, this.MailGuid, this.Remarks);
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
