namespace MetroLesMonitor.Bll {
    
    
    public partial class SmSendMailQueue {
        
        private System.Nullable<int> _queueId;
        
        private System.Nullable<int> _quotationid;
        
        private System.Nullable<int> _quoteExported;
        
        private string _docType;
        
        private string _refKey;
        
        private string _mailFrom;
        
        private string _mailTo;
        
        private string _mailCc;
        
        private string _mailBcc;
        
        private string _mailSubject;
        
        private string _mailBody;
        
        private string _attachments;
        
        private System.Nullable<System.DateTime> _mailDate;
        
        private System.Nullable<byte> _notToSent;
        
        private System.Nullable<int> _buyerId;
        
        private System.Nullable<int> _supplierId;
        
        private string _replyEmail;
        
        private string _senderName;
        
        private string _receiverName;
        
        private string _actionType;
        
        private System.Nullable<int> _htmlNotToSend;
        
        private System.Nullable<int> _sendHtmlMsg;
        
        private System.Nullable<int> _useHtmlFileMsg;
        
        private System.Nullable<int> _delayMailMin;
        
        private string _module;
        
        private string _suppRef;
        
        private string _byrCode;
        
        private string _suppCode;
        
        public virtual System.Nullable<int> QueueId {
            get {
                return _queueId;
            }
            set {
                _queueId = value;
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
        
        public virtual System.Nullable<int> QuoteExported {
            get {
                return _quoteExported;
            }
            set {
                _quoteExported = value;
            }
        }
        
        public virtual string DocType {
            get {
                return _docType;
            }
            set {
                _docType = value;
            }
        }
        
        public virtual string RefKey {
            get {
                return _refKey;
            }
            set {
                _refKey = value;
            }
        }
        
        public virtual string MailFrom {
            get {
                return _mailFrom;
            }
            set {
                _mailFrom = value;
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
        
        public virtual string Attachments {
            get {
                return _attachments;
            }
            set {
                _attachments = value;
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
        
        public virtual System.Nullable<byte> NotToSent {
            get {
                return _notToSent;
            }
            set {
                _notToSent = value;
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
        
        public virtual string ReplyEmail {
            get {
                return _replyEmail;
            }
            set {
                _replyEmail = value;
            }
        }
        
        public virtual string SenderName {
            get {
                return _senderName;
            }
            set {
                _senderName = value;
            }
        }
        
        public virtual string ReceiverName {
            get {
                return _receiverName;
            }
            set {
                _receiverName = value;
            }
        }
        
        public virtual string ActionType {
            get {
                return _actionType;
            }
            set {
                _actionType = value;
            }
        }
        
        public virtual System.Nullable<int> HtmlNotToSend {
            get {
                return _htmlNotToSend;
            }
            set {
                _htmlNotToSend = value;
            }
        }
        
        public virtual System.Nullable<int> SendHtmlMsg {
            get {
                return _sendHtmlMsg;
            }
            set {
                _sendHtmlMsg = value;
            }
        }
        
        public virtual System.Nullable<int> UseHtmlFileMsg {
            get {
                return _useHtmlFileMsg;
            }
            set {
                _useHtmlFileMsg = value;
            }
        }
        
        public virtual System.Nullable<int> DelayMailMin {
            get {
                return _delayMailMin;
            }
            set {
                _delayMailMin = value;
            }
        }
        
        public virtual string Module {
            get {
                return _module;
            }
            set {
                _module = value;
            }
        }
        
        public virtual string SuppRef {
            get {
                return _suppRef;
            }
            set {
                _suppRef = value;
            }
        }
        
        public virtual string ByrCode {
            get {
                return _byrCode;
            }
            set {
                _byrCode = value;
            }
        }
        
        public virtual string SuppCode {
            get {
                return _suppCode;
            }
            set {
                _suppCode = value;
            }
        }
        
        private void Clean() {
            this.QueueId = null;
            this.Quotationid = null;
            this.QuoteExported = null;
            this.DocType = string.Empty;
            this.RefKey = string.Empty;
            this.MailFrom = string.Empty;
            this.MailTo = string.Empty;
            this.MailCc = string.Empty;
            this.MailBcc = string.Empty;
            this.MailSubject = string.Empty;
            this.MailBody = string.Empty;
            this.Attachments = string.Empty;
            this.MailDate = null;
            this.NotToSent = null;
            this.BuyerId = null;
            this.SupplierId = null;
            this.ReplyEmail = string.Empty;
            this.SenderName = string.Empty;
            this.ReceiverName = string.Empty;
            this.ActionType = string.Empty;
            this.HtmlNotToSend = null;
            this.SendHtmlMsg = null;
            this.UseHtmlFileMsg = null;
            this.DelayMailMin = null;
            this.Module = string.Empty;
            this.SuppRef = string.Empty;
            this.ByrCode = string.Empty;
            this.SuppCode = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["QUEUE_ID"] != System.DBNull.Value)) {
                this.QueueId = ((System.Nullable<int>)(dr["QUEUE_ID"]));
            }
            if ((dr["QUOTATIONID"] != System.DBNull.Value)) {
                this.Quotationid = ((System.Nullable<int>)(dr["QUOTATIONID"]));
            }
            if ((dr["QUOTE_EXPORTED"] != System.DBNull.Value)) {
                this.QuoteExported = ((System.Nullable<int>)(dr["QUOTE_EXPORTED"]));
            }
            if ((dr["DOC_TYPE"] != System.DBNull.Value)) {
                this.DocType = ((string)(dr["DOC_TYPE"]));
            }
            if ((dr["REF_KEY"] != System.DBNull.Value)) {
                this.RefKey = ((string)(dr["REF_KEY"]));
            }
            if ((dr["MAIL_FROM"] != System.DBNull.Value)) {
                this.MailFrom = ((string)(dr["MAIL_FROM"]));
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
            if ((dr["MAIL_SUBJECT"] != System.DBNull.Value)) {
                this.MailSubject = ((string)(dr["MAIL_SUBJECT"]));
            }
            if ((dr["MAIL_BODY"] != System.DBNull.Value)) {
                this.MailBody = ((string)(dr["MAIL_BODY"]));
            }
            if ((dr["ATTACHMENTS"] != System.DBNull.Value)) {
                this.Attachments = ((string)(dr["ATTACHMENTS"]));
            }
            if ((dr["MAIL_DATE"] != System.DBNull.Value)) {
                this.MailDate = ((System.Nullable<System.DateTime>)(dr["MAIL_DATE"]));
            }
            if ((dr["NOT_TO_SENT"] != System.DBNull.Value)) {
                this.NotToSent = ((System.Nullable<byte>)(dr["NOT_TO_SENT"]));
            }
            if ((dr["BUYER_ID"] != System.DBNull.Value)) {
                this.BuyerId = ((System.Nullable<int>)(dr["BUYER_ID"]));
            }
            if ((dr["SUPPLIER_ID"] != System.DBNull.Value)) {
                this.SupplierId = ((System.Nullable<int>)(dr["SUPPLIER_ID"]));
            }
            if ((dr["REPLY_EMAIL"] != System.DBNull.Value)) {
                this.ReplyEmail = ((string)(dr["REPLY_EMAIL"]));
            }
            if ((dr["SENDER_NAME"] != System.DBNull.Value)) {
                this.SenderName = ((string)(dr["SENDER_NAME"]));
            }
            if ((dr["RECEIVER_NAME"] != System.DBNull.Value)) {
                this.ReceiverName = ((string)(dr["RECEIVER_NAME"]));
            }
            if ((dr["ACTION_TYPE"] != System.DBNull.Value)) {
                this.ActionType = ((string)(dr["ACTION_TYPE"]));
            }
            if ((dr["HTML_NOT_TO_SEND"] != System.DBNull.Value)) {
                this.HtmlNotToSend = ((System.Nullable<int>)(dr["HTML_NOT_TO_SEND"]));
            }
            if ((dr["SEND_HTML_MSG"] != System.DBNull.Value)) {
                this.SendHtmlMsg = ((System.Nullable<int>)(dr["SEND_HTML_MSG"]));
            }
            if ((dr["USE_HTML_FILE_MSG"] != System.DBNull.Value)) {
                this.UseHtmlFileMsg = ((System.Nullable<int>)(dr["USE_HTML_FILE_MSG"]));
            }
            if ((dr["DELAY_MAIL_MIN"] != System.DBNull.Value)) {
                this.DelayMailMin = ((System.Nullable<int>)(dr["DELAY_MAIL_MIN"]));
            }
            if ((dr["MODULE"] != System.DBNull.Value)) {
                this.Module = ((string)(dr["MODULE"]));
            }
            if ((dr["SUPP_REF"] != System.DBNull.Value)) {
                this.SuppRef = ((string)(dr["SUPP_REF"]));
            }
            if ((dr["BYR_CODE"] != System.DBNull.Value)) {
                this.ByrCode = ((string)(dr["BYR_CODE"]));
            }
            if ((dr["SUPP_CODE"] != System.DBNull.Value)) {
                this.SuppCode = ((string)(dr["SUPP_CODE"]));
            }
        }
        
        public static SmSendMailQueueCollection GetAll() {
            MetroLesMonitor.Dal.SmSendMailQueue dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmSendMailQueue();
                System.Data.DataSet ds = dbo.SM_SEND_MAIL_QUEUE_Select_All();
                SmSendMailQueueCollection collection = new SmSendMailQueueCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmSendMailQueue obj = new SmSendMailQueue();
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
            MetroLesMonitor.Dal.SmSendMailQueue dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmSendMailQueue();
                dbo.SM_SEND_MAIL_QUEUE_Insert(this.RefKey, this.MailFrom, this.MailTo, this.MailCc, this.MailBcc, this.MailSubject, this.MailBody, this.MailDate,  this.BuyerId, this.SupplierId);
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
