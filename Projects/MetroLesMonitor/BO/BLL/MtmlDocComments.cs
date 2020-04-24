namespace MetroLesMonitor.Bll {
    
    
    public partial class MtmlDocComments {
        
        private System.Nullable<System.Guid> _mtmldoccommentid;
        
        private System.Nullable<System.Guid> _mtmldocid;
        
        private string _comments;
        
        private string _qualifier;
        
        private System.Nullable<int> _autoid;
        
        public virtual System.Nullable<System.Guid> Mtmldoccommentid {
            get {
                return _mtmldoccommentid;
            }
            set {
                _mtmldoccommentid = value;
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
        
        public virtual string Comments {
            get {
                return _comments;
            }
            set {
                _comments = value;
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
        
        public virtual System.Nullable<int> Autoid {
            get {
                return _autoid;
            }
            set {
                _autoid = value;
            }
        }
        
        private void Clean() {
            this.Mtmldoccommentid = null;
            this.Mtmldocid = null;
            this.Comments = string.Empty;
            this.Qualifier = string.Empty;
            this.Autoid = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["MTMLDOCCOMMENTID"] != System.DBNull.Value)) {
                this.Mtmldoccommentid = ((System.Nullable<System.Guid>)(dr["MTMLDOCCOMMENTID"]));
            }
            if ((dr["MTMLDOCID"] != System.DBNull.Value)) {
                this.Mtmldocid = ((System.Nullable<System.Guid>)(dr["MTMLDOCID"]));
            }
            if ((dr["COMMENTS"] != System.DBNull.Value)) {
                this.Comments = ((string)(dr["COMMENTS"]));
            }
            if ((dr["QUALIFIER"] != System.DBNull.Value)) {
                this.Qualifier = ((string)(dr["QUALIFIER"]));
            }
            if ((dr["AUTOID"] != System.DBNull.Value)) {
                this.Autoid = ((System.Nullable<int>)(dr["AUTOID"]));
            }
        }
        
        public static MtmlDocCommentsCollection GetAll() {
            MetroLesMonitor.Dal.MtmlDocComments dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocComments();
                System.Data.DataSet ds = dbo.MTML_DOC_COMMENTS_Select_All();
                MtmlDocCommentsCollection collection = new MtmlDocCommentsCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        MtmlDocComments obj = new MtmlDocComments();
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
        
        public static MtmlDocComments Load(System.Nullable<System.Guid> MTMLDOCCOMMENTID) {
            MetroLesMonitor.Dal.MtmlDocComments dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocComments();
                System.Data.DataSet ds = dbo.MTML_DOC_COMMENTS_Select_One(MTMLDOCCOMMENTID);
                MtmlDocComments obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new MtmlDocComments();
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
            MetroLesMonitor.Dal.MtmlDocComments dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocComments();
                System.Data.DataSet ds = dbo.MTML_DOC_COMMENTS_Select_One(this.Mtmldoccommentid);
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
            MetroLesMonitor.Dal.MtmlDocComments dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocComments();
                dbo.MTML_DOC_COMMENTS_Insert(this.Mtmldocid, this.Comments, this.Qualifier);
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
            MetroLesMonitor.Dal.MtmlDocComments dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocComments();
                dbo.MTML_DOC_COMMENTS_Delete(this.Mtmldoccommentid);
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
            MetroLesMonitor.Dal.MtmlDocComments dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocComments();
                dbo.MTML_DOC_COMMENTS_Update(this.Mtmldoccommentid, this.Mtmldocid, this.Comments, this.Qualifier, this.Autoid);
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
