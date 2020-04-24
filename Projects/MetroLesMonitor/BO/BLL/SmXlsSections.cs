namespace MetroLesMonitor.Bll {
    
    
    public partial class SmXlsSections {
        
        private System.Nullable<int> _sectionid;
        
        private string _sectionCode;
        
        private string _sectionDesc;
        
        public virtual System.Nullable<int> Sectionid {
            get {
                return _sectionid;
            }
            set {
                _sectionid = value;
            }
        }
        
        public virtual string SectionCode {
            get {
                return _sectionCode;
            }
            set {
                _sectionCode = value;
            }
        }
        
        public virtual string SectionDesc {
            get {
                return _sectionDesc;
            }
            set {
                _sectionDesc = value;
            }
        }
        
        private void Clean() {
            this.Sectionid = null;
            this.SectionCode = string.Empty;
            this.SectionDesc = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["SECTIONID"] != System.DBNull.Value)) {
                this.Sectionid = ((System.Nullable<int>)(dr["SECTIONID"]));
            }
            if ((dr["SECTION_CODE"] != System.DBNull.Value)) {
                this.SectionCode = ((string)(dr["SECTION_CODE"]));
            }
            if ((dr["SECTION_DESC"] != System.DBNull.Value)) {
                this.SectionDesc = ((string)(dr["SECTION_DESC"]));
            }
        }
        
        public static SmXlsSectionsCollection GetAll() {
            MetroLesMonitor.Dal.SmXlsSections dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmXlsSections();
                System.Data.DataSet ds = dbo.SM_XLS_SECTIONS_Select_All();
                SmXlsSectionsCollection collection = new SmXlsSectionsCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmXlsSections obj = new SmXlsSections();
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
        
        public static SmXlsSections Load(System.Nullable<int> SECTIONID) {
            MetroLesMonitor.Dal.SmXlsSections dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmXlsSections();
                System.Data.DataSet ds = dbo.SM_XLS_SECTIONS_Select_One(SECTIONID);
                SmXlsSections obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmXlsSections();
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
            MetroLesMonitor.Dal.SmXlsSections dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmXlsSections();
                System.Data.DataSet ds = dbo.SM_XLS_SECTIONS_Select_One(this.Sectionid);
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
            MetroLesMonitor.Dal.SmXlsSections dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmXlsSections();
                dbo.SM_XLS_SECTIONS_Insert(this.SectionCode, this.SectionDesc);
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
            MetroLesMonitor.Dal.SmXlsSections dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmXlsSections();
                dbo.SM_XLS_SECTIONS_Delete(this.Sectionid);
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
            MetroLesMonitor.Dal.SmXlsSections dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmXlsSections();
                dbo.SM_XLS_SECTIONS_Update(this.Sectionid, this.SectionCode, this.SectionDesc);
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
