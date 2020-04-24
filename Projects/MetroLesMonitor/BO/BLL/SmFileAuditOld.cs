namespace MetroLesMonitor.Bll {
    
    
    public partial class SmFileAuditOld {
        
        private System.Nullable<int> _recordid;
        
        private string _vrno;
        
        private string _docFilename1;
        
        private string _docFilename2;
        
        private System.Nullable<int> _buyerid;
        
        private System.Nullable<int> _supplierid;
        
        private System.Nullable<System.DateTime> _byrUploaded;
        
        private System.Nullable<System.DateTime> _byrImported;
        
        private System.Nullable<System.DateTime> _supExported;
        
        private System.Nullable<System.DateTime> _supDownloaded;
        
        private System.Nullable<System.DateTime> _supUploaded;
        
        private System.Nullable<System.DateTime> _supImported;
        
        private System.Nullable<System.DateTime> _byrExported;
        
        private System.Nullable<System.DateTime> _byrDownloaded;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        public virtual System.Nullable<int> Recordid {
            get {
                return _recordid;
            }
            set {
                _recordid = value;
            }
        }
        
        public virtual string Vrno {
            get {
                return _vrno;
            }
            set {
                _vrno = value;
            }
        }
        
        public virtual string DocFilename1 {
            get {
                return _docFilename1;
            }
            set {
                _docFilename1 = value;
            }
        }
        
        public virtual string DocFilename2 {
            get {
                return _docFilename2;
            }
            set {
                _docFilename2 = value;
            }
        }
        
        public virtual System.Nullable<int> Buyerid {
            get {
                return _buyerid;
            }
            set {
                _buyerid = value;
            }
        }
        
        public virtual System.Nullable<int> Supplierid {
            get {
                return _supplierid;
            }
            set {
                _supplierid = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> ByrUploaded {
            get {
                return _byrUploaded;
            }
            set {
                _byrUploaded = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> ByrImported {
            get {
                return _byrImported;
            }
            set {
                _byrImported = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> SupExported {
            get {
                return _supExported;
            }
            set {
                _supExported = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> SupDownloaded {
            get {
                return _supDownloaded;
            }
            set {
                _supDownloaded = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> SupUploaded {
            get {
                return _supUploaded;
            }
            set {
                _supUploaded = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> SupImported {
            get {
                return _supImported;
            }
            set {
                _supImported = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> ByrExported {
            get {
                return _byrExported;
            }
            set {
                _byrExported = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> ByrDownloaded {
            get {
                return _byrDownloaded;
            }
            set {
                _byrDownloaded = value;
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
            this.Recordid = null;
            this.Vrno = string.Empty;
            this.DocFilename1 = string.Empty;
            this.DocFilename2 = string.Empty;
            this.Buyerid = null;
            this.Supplierid = null;
            this.ByrUploaded = null;
            this.ByrImported = null;
            this.SupExported = null;
            this.SupDownloaded = null;
            this.SupUploaded = null;
            this.SupImported = null;
            this.ByrExported = null;
            this.ByrDownloaded = null;
            this.UpdateDate = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["RECORDID"] != System.DBNull.Value)) {
                this.Recordid = ((System.Nullable<int>)(dr["RECORDID"]));
            }
            if ((dr["VRNO"] != System.DBNull.Value)) {
                this.Vrno = ((string)(dr["VRNO"]));
            }
            if ((dr["DOC_FILENAME1"] != System.DBNull.Value)) {
                this.DocFilename1 = ((string)(dr["DOC_FILENAME1"]));
            }
            if ((dr["DOC_FILENAME2"] != System.DBNull.Value)) {
                this.DocFilename2 = ((string)(dr["DOC_FILENAME2"]));
            }
            if ((dr["BUYERID"] != System.DBNull.Value)) {
                this.Buyerid = ((System.Nullable<int>)(dr["BUYERID"]));
            }
            if ((dr["SUPPLIERID"] != System.DBNull.Value)) {
                this.Supplierid = ((System.Nullable<int>)(dr["SUPPLIERID"]));
            }
            if ((dr["BYR_UPLOADED"] != System.DBNull.Value)) {
                this.ByrUploaded = ((System.Nullable<System.DateTime>)(dr["BYR_UPLOADED"]));
            }
            if ((dr["BYR_IMPORTED"] != System.DBNull.Value)) {
                this.ByrImported = ((System.Nullable<System.DateTime>)(dr["BYR_IMPORTED"]));
            }
            if ((dr["SUP_EXPORTED"] != System.DBNull.Value)) {
                this.SupExported = ((System.Nullable<System.DateTime>)(dr["SUP_EXPORTED"]));
            }
            if ((dr["SUP_DOWNLOADED"] != System.DBNull.Value)) {
                this.SupDownloaded = ((System.Nullable<System.DateTime>)(dr["SUP_DOWNLOADED"]));
            }
            if ((dr["SUP_UPLOADED"] != System.DBNull.Value)) {
                this.SupUploaded = ((System.Nullable<System.DateTime>)(dr["SUP_UPLOADED"]));
            }
            if ((dr["SUP_IMPORTED"] != System.DBNull.Value)) {
                this.SupImported = ((System.Nullable<System.DateTime>)(dr["SUP_IMPORTED"]));
            }
            if ((dr["BYR_EXPORTED"] != System.DBNull.Value)) {
                this.ByrExported = ((System.Nullable<System.DateTime>)(dr["BYR_EXPORTED"]));
            }
            if ((dr["BYR_DOWNLOADED"] != System.DBNull.Value)) {
                this.ByrDownloaded = ((System.Nullable<System.DateTime>)(dr["BYR_DOWNLOADED"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value)) {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
        }
        
        public static SmFileAuditOldCollection GetAll() {
            MetroLesMonitor.Dal.SmFileAuditOld dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmFileAuditOld();
                System.Data.DataSet ds = dbo.SM_FILE_AUDIT_old_Select_All();
                SmFileAuditOldCollection collection = new SmFileAuditOldCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmFileAuditOld obj = new SmFileAuditOld();
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
        
        public static SmFileAuditOld Load(System.Nullable<int> RECORDID) {
            MetroLesMonitor.Dal.SmFileAuditOld dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmFileAuditOld();
                System.Data.DataSet ds = dbo.SM_FILE_AUDIT_old_Select_One(RECORDID);
                SmFileAuditOld obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmFileAuditOld();
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
            MetroLesMonitor.Dal.SmFileAuditOld dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmFileAuditOld();
                System.Data.DataSet ds = dbo.SM_FILE_AUDIT_old_Select_One(this.Recordid);
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
            MetroLesMonitor.Dal.SmFileAuditOld dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmFileAuditOld();
                dbo.SM_FILE_AUDIT_old_Insert(this.Vrno, this.DocFilename1, this.DocFilename2, this.Buyerid, this.Supplierid, this.ByrUploaded, this.ByrImported, this.SupExported, this.SupDownloaded, this.SupUploaded, this.SupImported, this.ByrExported, this.ByrDownloaded, this.UpdateDate);
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
            MetroLesMonitor.Dal.SmFileAuditOld dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmFileAuditOld();
                dbo.SM_FILE_AUDIT_old_Delete(this.Recordid);
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
            MetroLesMonitor.Dal.SmFileAuditOld dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmFileAuditOld();
                dbo.SM_FILE_AUDIT_old_Update(this.Recordid, this.Vrno, this.DocFilename1, this.DocFilename2, this.Buyerid, this.Supplierid, this.ByrUploaded, this.ByrImported, this.SupExported, this.SupDownloaded, this.SupUploaded, this.SupImported, this.ByrExported, this.ByrDownloaded, this.UpdateDate);
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
