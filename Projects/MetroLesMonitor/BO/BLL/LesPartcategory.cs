namespace MetroLesMonitor.Bll {
    
    
    public partial class LesPartcategory {
        
        private System.Nullable<int> _categoryid;
        
        private string _categoryCode;
        
        private string _description;
        
        private LesParttypeCollection _lesParttypeCollection;
        
        public virtual System.Nullable<int> Categoryid {
            get {
                return _categoryid;
            }
            set {
                _categoryid = value;
            }
        }
        
        public virtual string CategoryCode {
            get {
                return _categoryCode;
            }
            set {
                _categoryCode = value;
            }
        }
        
        public virtual string Description {
            get {
                return _description;
            }
            set {
                _description = value;
            }
        }
        
        public virtual LesParttypeCollection LesParttypeCollection {
            get {
                if ((this._lesParttypeCollection == null)) {
                    _lesParttypeCollection = MetroLesMonitor.Bll.LesParttype.Select_LES_PARTTYPEs_By_CATEGORYID(this.Categoryid);
                }
                return this._lesParttypeCollection;
            }
        }
        
        private void Clean() {
            this.Categoryid = null;
            this.CategoryCode = string.Empty;
            this.Description = string.Empty;
            this._lesParttypeCollection = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["CATEGORYID"] != System.DBNull.Value)) {
                this.Categoryid = ((System.Nullable<int>)(dr["CATEGORYID"]));
            }
            if ((dr["CATEGORY_CODE"] != System.DBNull.Value)) {
                this.CategoryCode = ((string)(dr["CATEGORY_CODE"]));
            }
            if ((dr["DESCRIPTION"] != System.DBNull.Value)) {
                this.Description = ((string)(dr["DESCRIPTION"]));
            }
        }
        
        public static LesPartcategoryCollection GetAll() {
            MetroLesMonitor.Dal.LesPartcategory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesPartcategory();
                System.Data.DataSet ds = dbo.LES_PARTCATEGORY_Select_All();
                LesPartcategoryCollection collection = new LesPartcategoryCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesPartcategory obj = new LesPartcategory();
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
        
        public static LesPartcategory Load(System.Nullable<int> CATEGORYID) {
            MetroLesMonitor.Dal.LesPartcategory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesPartcategory();
                System.Data.DataSet ds = dbo.LES_PARTCATEGORY_Select_One(CATEGORYID);
                LesPartcategory obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new LesPartcategory();
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
            MetroLesMonitor.Dal.LesPartcategory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesPartcategory();
                System.Data.DataSet ds = dbo.LES_PARTCATEGORY_Select_One(this.Categoryid);
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
            MetroLesMonitor.Dal.LesPartcategory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesPartcategory();
                dbo.LES_PARTCATEGORY_Insert(this.CategoryCode, this.Description);
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
            MetroLesMonitor.Dal.LesPartcategory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesPartcategory();
                dbo.LES_PARTCATEGORY_Delete(this.Categoryid);
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
            MetroLesMonitor.Dal.LesPartcategory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesPartcategory();
                dbo.LES_PARTCATEGORY_Update(this.Categoryid, this.CategoryCode, this.Description);
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
