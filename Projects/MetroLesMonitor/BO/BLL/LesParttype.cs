namespace MetroLesMonitor.Bll {
    
    
    public partial class LesParttype {
        
        private System.Nullable<int> _parttypeid;
        
        private string _parttypeCode;
        
        private string _description;
        
        private System.Nullable<int> _categoryid;
        
        private LesInventoryCollection _lesInventoryCollection;
        
        private LesPartcategory _lesPartcategory;
        
        public virtual System.Nullable<int> Parttypeid {
            get {
                return _parttypeid;
            }
            set {
                _parttypeid = value;
            }
        }
        
        public virtual string ParttypeCode {
            get {
                return _parttypeCode;
            }
            set {
                _parttypeCode = value;
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
        
        public virtual LesInventoryCollection LesInventoryCollection {
            get {
                if ((this._lesInventoryCollection == null)) {
                    _lesInventoryCollection = MetroLesMonitor.Bll.LesInventory.Select_LES_INVENTORYs_By_PARTTYPEID(this.Parttypeid);
                }
                return this._lesInventoryCollection;
            }
        }
        
        public virtual LesPartcategory LesPartcategory {
            get {
                if ((this._lesPartcategory == null)) {
                    this._lesPartcategory = MetroLesMonitor.Bll.LesPartcategory.Load(this._categoryid);
                }
                return this._lesPartcategory;
            }
            set {
                _lesPartcategory = value;
            }
        }
        
        private void Clean() {
            this.Parttypeid = null;
            this.ParttypeCode = string.Empty;
            this.Description = string.Empty;
            this._categoryid = null;
            this._lesInventoryCollection = null;
            this.LesPartcategory = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["PARTTYPEID"] != System.DBNull.Value)) {
                this.Parttypeid = ((System.Nullable<int>)(dr["PARTTYPEID"]));
            }
            if ((dr["PARTTYPE_CODE"] != System.DBNull.Value)) {
                this.ParttypeCode = ((string)(dr["PARTTYPE_CODE"]));
            }
            if ((dr["DESCRIPTION"] != System.DBNull.Value)) {
                this.Description = ((string)(dr["DESCRIPTION"]));
            }
            if ((dr["CATEGORYID"] != System.DBNull.Value)) {
                this._categoryid = ((System.Nullable<int>)(dr["CATEGORYID"]));
            }
        }
        
        public static LesParttypeCollection Select_LES_PARTTYPEs_By_CATEGORYID(System.Nullable<int> CATEGORYID) {
            MetroLesMonitor.Dal.LesParttype dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesParttype();
                System.Data.DataSet ds = dbo.Select_LES_PARTTYPEs_By_CATEGORYID(CATEGORYID);
                LesParttypeCollection collection = new LesParttypeCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesParttype obj = new LesParttype();
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
        
        public static LesParttypeCollection GetAll() {
            MetroLesMonitor.Dal.LesParttype dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesParttype();
                System.Data.DataSet ds = dbo.LES_PARTTYPE_Select_All();
                LesParttypeCollection collection = new LesParttypeCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesParttype obj = new LesParttype();
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
        
        public static LesParttype Load(System.Nullable<int> PARTTYPEID) {
            MetroLesMonitor.Dal.LesParttype dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesParttype();
                System.Data.DataSet ds = dbo.LES_PARTTYPE_Select_One(PARTTYPEID);
                LesParttype obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new LesParttype();
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
            MetroLesMonitor.Dal.LesParttype dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesParttype();
                System.Data.DataSet ds = dbo.LES_PARTTYPE_Select_One(this.Parttypeid);
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
            MetroLesMonitor.Dal.LesParttype dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesParttype();
                dbo.LES_PARTTYPE_Insert(this.ParttypeCode, this.Description, this._categoryid);
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
            MetroLesMonitor.Dal.LesParttype dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesParttype();
                dbo.LES_PARTTYPE_Delete(this.Parttypeid);
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
            MetroLesMonitor.Dal.LesParttype dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesParttype();
                dbo.LES_PARTTYPE_Update(this.Parttypeid, this.ParttypeCode, this.Description, this._categoryid);
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
