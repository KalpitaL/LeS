namespace MetroLesMonitor.Bll {
    
    
    public partial class MtmlDocDatetime {
        
        private System.Nullable<System.Guid> _mtmldocdatetimeid;
        
        private System.Nullable<System.Guid> _mtmldocid;
        
        private string _dateValue;
        
        private string _qualifier;
        
        private string _formatqualifier;
        
        private System.Nullable<int> _autoid;
        
        public virtual System.Nullable<System.Guid> Mtmldocdatetimeid {
            get {
                return _mtmldocdatetimeid;
            }
            set {
                _mtmldocdatetimeid = value;
            }
        }
        
        public virtual System.Nullable<System.Guid> Mtmldocid {
            get {
                return _mtmldocid;
            }
            set {
                _mtmldocid = value;
            }
        }
        
        public virtual string DateValue {
            get {
                return _dateValue;
            }
            set {
                _dateValue = value;
            }
        }
        
        public virtual string Qualifier {
            get {
                return _qualifier;
            }
            set {
                _qualifier = value;
            }
        }
        
        public virtual string Formatqualifier {
            get {
                return _formatqualifier;
            }
            set {
                _formatqualifier = value;
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
            this.Mtmldocdatetimeid = null;
            this.Mtmldocid = null;
            this.DateValue = string.Empty;
            this.Qualifier = string.Empty;
            this.Formatqualifier = string.Empty;
            this.Autoid = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["MTMLDOCDATETIMEID"] != System.DBNull.Value)) {
                this.Mtmldocdatetimeid = ((System.Nullable<System.Guid>)(dr["MTMLDOCDATETIMEID"]));
            }
            if ((dr["MTMLDOCID"] != System.DBNull.Value)) {
                this.Mtmldocid = ((System.Nullable<System.Guid>)(dr["MTMLDOCID"]));
            }
            if ((dr["DATE_VALUE"] != System.DBNull.Value)) {
                this.DateValue = ((string)(dr["DATE_VALUE"]));
            }
            if ((dr["QUALIFIER"] != System.DBNull.Value)) {
                this.Qualifier = ((string)(dr["QUALIFIER"]));
            }
            if ((dr["FORMATQUALIFIER"] != System.DBNull.Value)) {
                this.Formatqualifier = ((string)(dr["FORMATQUALIFIER"]));
            }
            if ((dr["AUTOID"] != System.DBNull.Value)) {
                this.Autoid = ((System.Nullable<int>)(dr["AUTOID"]));
            }
        }
        
        public static MtmlDocDatetimeCollection GetAll() {
            MetroLesMonitor.Dal.MtmlDocDatetime dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocDatetime();
                System.Data.DataSet ds = dbo.MTML_DOC_DATETIME_Select_All();
                MtmlDocDatetimeCollection collection = new MtmlDocDatetimeCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        MtmlDocDatetime obj = new MtmlDocDatetime();
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
        
        public static MtmlDocDatetime Load(System.Nullable<System.Guid> MTMLDOCDATETIMEID) {
            MetroLesMonitor.Dal.MtmlDocDatetime dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocDatetime();
                System.Data.DataSet ds = dbo.MTML_DOC_DATETIME_Select_One(MTMLDOCDATETIMEID);
                MtmlDocDatetime obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new MtmlDocDatetime();
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
            MetroLesMonitor.Dal.MtmlDocDatetime dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocDatetime();
                System.Data.DataSet ds = dbo.MTML_DOC_DATETIME_Select_One(this.Mtmldocdatetimeid);
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
            MetroLesMonitor.Dal.MtmlDocDatetime dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocDatetime();
                dbo.MTML_DOC_DATETIME_Insert(this.Mtmldocid, this.DateValue, this.Qualifier, this.Formatqualifier);
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
            MetroLesMonitor.Dal.MtmlDocDatetime dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocDatetime();
                dbo.MTML_DOC_DATETIME_Delete(this.Mtmldocdatetimeid);
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
            MetroLesMonitor.Dal.MtmlDocDatetime dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocDatetime();
                dbo.MTML_DOC_DATETIME_Update(this.Mtmldocdatetimeid, this.Mtmldocid, this.DateValue, this.Qualifier, this.Formatqualifier, this.Autoid);
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
