namespace MetroLesMonitor.Bll {
    
    
    public partial class MtmlDocReference {
        
        private System.Nullable<System.Guid> _mtmlreferenceid;
        
        private System.Nullable<System.Guid> _mtmldocid;
        
        private string _qualifier;
        
        private string _referencenumber;
        
        private System.Nullable<int> _autoid;
        
        public virtual System.Nullable<System.Guid> Mtmlreferenceid {
            get {
                return _mtmlreferenceid;
            }
            set {
                _mtmlreferenceid = value;
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
        
        public virtual string Qualifier {
            get {
                return _qualifier;
            }
            set {
                _qualifier = value;
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
        
        public virtual System.Nullable<int> Autoid {
            get {
                return _autoid;
            }
            set {
                _autoid = value;
            }
        }
        
        private void Clean() {
            this.Mtmlreferenceid = null;
            this.Mtmldocid = null;
            this.Qualifier = string.Empty;
            this.Referencenumber = string.Empty;
            this.Autoid = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["MTMLREFERENCEID"] != System.DBNull.Value)) {
                this.Mtmlreferenceid = ((System.Nullable<System.Guid>)(dr["MTMLREFERENCEID"]));
            }
            if ((dr["MTMLDOCID"] != System.DBNull.Value)) {
                this.Mtmldocid = ((System.Nullable<System.Guid>)(dr["MTMLDOCID"]));
            }
            if ((dr["QUALIFIER"] != System.DBNull.Value)) {
                this.Qualifier = ((string)(dr["QUALIFIER"]));
            }
            if ((dr["REFERENCENUMBER"] != System.DBNull.Value)) {
                this.Referencenumber = ((string)(dr["REFERENCENUMBER"]));
            }
            if ((dr["AUTOID"] != System.DBNull.Value)) {
                this.Autoid = ((System.Nullable<int>)(dr["AUTOID"]));
            }
        }
        
        public static MtmlDocReferenceCollection GetAll() {
            MetroLesMonitor.Dal.MtmlDocReference dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocReference();
                System.Data.DataSet ds = dbo.MTML_DOC_REFERENCE_Select_All();
                MtmlDocReferenceCollection collection = new MtmlDocReferenceCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        MtmlDocReference obj = new MtmlDocReference();
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
        
        public static MtmlDocReference Load(System.Nullable<System.Guid> MTMLREFERENCEID) {
            MetroLesMonitor.Dal.MtmlDocReference dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocReference();
                System.Data.DataSet ds = dbo.MTML_DOC_REFERENCE_Select_One(MTMLREFERENCEID);
                MtmlDocReference obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new MtmlDocReference();
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
            MetroLesMonitor.Dal.MtmlDocReference dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocReference();
                System.Data.DataSet ds = dbo.MTML_DOC_REFERENCE_Select_One(this.Mtmlreferenceid);
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
            MetroLesMonitor.Dal.MtmlDocReference dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocReference();
                dbo.MTML_DOC_REFERENCE_Insert(this.Mtmldocid, this.Qualifier, this.Referencenumber);
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
            MetroLesMonitor.Dal.MtmlDocReference dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocReference();
                dbo.MTML_DOC_REFERENCE_Delete(this.Mtmlreferenceid);
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
            MetroLesMonitor.Dal.MtmlDocReference dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocReference();
                dbo.MTML_DOC_REFERENCE_Update(this.Mtmlreferenceid, this.Mtmldocid, this.Qualifier, this.Referencenumber, this.Autoid);
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
