namespace MetroLesMonitor.Bll {
    
    
    public partial class SmAttachment {
        
        private System.Nullable<int> _attachmentid;
        
        private System.Nullable<int> _quotationid;
        
        private string _attachmenttype;
        
        private string _fileName;
        
        private System.Nullable<short> _exported;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private string _fileLocation;
        
        public virtual System.Nullable<int> Attachmentid {
            get {
                return _attachmentid;
            }
            set {
                _attachmentid = value;
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
        
        public virtual string Attachmenttype {
            get {
                return _attachmenttype;
            }
            set {
                _attachmenttype = value;
            }
        }
        
        public virtual string FileName {
            get {
                return _fileName;
            }
            set {
                _fileName = value;
            }
        }
        
        public virtual System.Nullable<short> Exported {
            get {
                return _exported;
            }
            set {
                _exported = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> CreatedDate {
            get {
                return _createdDate;
            }
            set {
                _createdDate = value;
            }
        }
        
        public virtual string FileLocation {
            get {
                return _fileLocation;
            }
            set {
                _fileLocation = value;
            }
        }
        
        private void Clean() {
            this.Attachmentid = null;
            this.Quotationid = null;
            this.Attachmenttype = string.Empty;
            this.FileName = string.Empty;
            this.Exported = null;
            this.CreatedDate = null;
            this.FileLocation = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["ATTACHMENTID"] != System.DBNull.Value)) {
                this.Attachmentid = ((System.Nullable<int>)(dr["ATTACHMENTID"]));
            }
            if ((dr["QUOTATIONID"] != System.DBNull.Value)) {
                this.Quotationid = ((System.Nullable<int>)(dr["QUOTATIONID"]));
            }
            if ((dr["ATTACHMENTTYPE"] != System.DBNull.Value)) {
                this.Attachmenttype = ((string)(dr["ATTACHMENTTYPE"]));
            }
            if ((dr["FILE_NAME"] != System.DBNull.Value)) {
                this.FileName = ((string)(dr["FILE_NAME"]));
            }
            if ((dr["EXPORTED"] != System.DBNull.Value)) {
                this.Exported = ((System.Nullable<short>)(dr["EXPORTED"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value)) {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["FILE_LOCATION"] != System.DBNull.Value)) {
                this.FileLocation = ((string)(dr["FILE_LOCATION"]));
            }
        }
        
        public static SmAttachmentCollection GetAll() {
            MetroLesMonitor.Dal.SmAttachment dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmAttachment();
                System.Data.DataSet ds = dbo.SM_ATTACHMENT_Select_All();
                SmAttachmentCollection collection = new SmAttachmentCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmAttachment obj = new SmAttachment();
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
        
        public static SmAttachment Load(System.Nullable<int> ATTACHMENTID) {
            MetroLesMonitor.Dal.SmAttachment dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmAttachment();
                System.Data.DataSet ds = dbo.SM_ATTACHMENT_Select_One(ATTACHMENTID);
                SmAttachment obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmAttachment();
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
            MetroLesMonitor.Dal.SmAttachment dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmAttachment();
                System.Data.DataSet ds = dbo.SM_ATTACHMENT_Select_One(this.Attachmentid);
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
            MetroLesMonitor.Dal.SmAttachment dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmAttachment();
                dbo.SM_ATTACHMENT_Insert(this.Quotationid, this.Attachmenttype, this.FileName, this.Exported, this.CreatedDate, this.FileLocation);
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
            MetroLesMonitor.Dal.SmAttachment dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmAttachment();
                dbo.SM_ATTACHMENT_Delete(this.Attachmentid);
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
            MetroLesMonitor.Dal.SmAttachment dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmAttachment();
                dbo.SM_ATTACHMENT_Update(this.Attachmentid, this.Quotationid, this.Attachmenttype, this.FileName, this.Exported, this.CreatedDate, this.FileLocation);
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
