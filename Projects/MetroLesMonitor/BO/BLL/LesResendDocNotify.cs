namespace MetroLesMonitor.Bll {
    
    
    public partial class LesResendDocNotify {
        
        private System.Nullable<int> _resendId;
        
        private System.Nullable<int> _quotationId;
        
        private string _docType;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        public virtual System.Nullable<int> ResendId {
            get {
                return _resendId;
            }
            set {
                _resendId = value;
            }
        }
        
        public virtual System.Nullable<int> QuotationId {
            get {
                return _quotationId;
            }
            set {
                _quotationId = value;
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
        
        public virtual System.Nullable<System.DateTime> UpdateDate {
            get {
                return _updateDate;
            }
            set {
                _updateDate = value;
            }
        }
        
        private void Clean() {
            this.ResendId = null;
            this.QuotationId = null;
            this.DocType = string.Empty;
            this.UpdateDate = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["RESEND_ID"] != System.DBNull.Value)) {
                this.ResendId = ((System.Nullable<int>)(dr["RESEND_ID"]));
            }
            if ((dr["QUOTATION_ID"] != System.DBNull.Value)) {
                this.QuotationId = ((System.Nullable<int>)(dr["QUOTATION_ID"]));
            }
            if ((dr["DOC_TYPE"] != System.DBNull.Value)) {
                this.DocType = ((string)(dr["DOC_TYPE"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value)) {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
        }
        
        public static LesResendDocNotifyCollection GetAll() {
            MetroLesMonitor.Dal.LesResendDocNotify dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesResendDocNotify();
                System.Data.DataSet ds = dbo.LES_RESEND_DOC_NOTIFY_Select_All();
                LesResendDocNotifyCollection collection = new LesResendDocNotifyCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesResendDocNotify obj = new LesResendDocNotify();
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
        
        public static LesResendDocNotify Load(System.Nullable<int> RESEND_ID) {
            MetroLesMonitor.Dal.LesResendDocNotify dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesResendDocNotify();
                System.Data.DataSet ds = dbo.LES_RESEND_DOC_NOTIFY_Select_One(RESEND_ID);
                LesResendDocNotify obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new LesResendDocNotify();
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
            MetroLesMonitor.Dal.LesResendDocNotify dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesResendDocNotify();
                System.Data.DataSet ds = dbo.LES_RESEND_DOC_NOTIFY_Select_One(this.ResendId);
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
            MetroLesMonitor.Dal.LesResendDocNotify dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesResendDocNotify();
                dbo.LES_RESEND_DOC_NOTIFY_Insert(this.QuotationId, this.DocType, this.UpdateDate);
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
            MetroLesMonitor.Dal.LesResendDocNotify dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesResendDocNotify();
                dbo.LES_RESEND_DOC_NOTIFY_Delete(this.ResendId);
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
            MetroLesMonitor.Dal.LesResendDocNotify dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesResendDocNotify();
                dbo.LES_RESEND_DOC_NOTIFY_Update(this.ResendId, this.QuotationId, this.DocType, this.UpdateDate);
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
