namespace MetroLesMonitor.Bll {
    
    
    public partial class LesLocations {
        
        private System.Nullable<int> _locationid;
        
        private string _loccode;
        
        private string _locDescription;
        
        private LesInventorylocationCollection _lesInventorylocationCollection;
        
        private LesSupplierLocationLinkCollection _lesSupplierLocationLinkCollection;
        
        public virtual System.Nullable<int> Locationid {
            get {
                return _locationid;
            }
            set {
                _locationid = value;
            }
        }
        
        public virtual string Loccode {
            get {
                return _loccode;
            }
            set {
                _loccode = value;
            }
        }
        
        public virtual string LocDescription {
            get {
                return _locDescription;
            }
            set {
                _locDescription = value;
            }
        }
        
        public virtual LesInventorylocationCollection LesInventorylocationCollection {
            get {
                if ((this._lesInventorylocationCollection == null)) {
                    _lesInventorylocationCollection = MetroLesMonitor.Bll.LesInventorylocation.Select_LES_INVENTORYLOCATIONs_By_LOCATIONID(this.Locationid);
                }
                return this._lesInventorylocationCollection;
            }
        }
        
        public virtual LesSupplierLocationLinkCollection LesSupplierLocationLinkCollection {
            get {
                if ((this._lesSupplierLocationLinkCollection == null)) {
                    _lesSupplierLocationLinkCollection = MetroLesMonitor.Bll.LesSupplierLocationLink.Select_LES_SUPPLIER_LOCATION_LINKs_By_LOCATIONID(this.Locationid);
                }
                return this._lesSupplierLocationLinkCollection;
            }
        }
        
        private void Clean() {
            this.Locationid = null;
            this.Loccode = string.Empty;
            this.LocDescription = string.Empty;
            this._lesInventorylocationCollection = null;
            this._lesSupplierLocationLinkCollection = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["LOCATIONID"] != System.DBNull.Value)) {
                this.Locationid = ((System.Nullable<int>)(dr["LOCATIONID"]));
            }
            if ((dr["LOCCODE"] != System.DBNull.Value)) {
                this.Loccode = ((string)(dr["LOCCODE"]));
            }
            if ((dr["LOC_DESCRIPTION"] != System.DBNull.Value)) {
                this.LocDescription = ((string)(dr["LOC_DESCRIPTION"]));
            }
        }
        
        public static LesLocationsCollection GetAll() {
            MetroLesMonitor.Dal.LesLocations dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesLocations();
                System.Data.DataSet ds = dbo.LES_LOCATIONS_Select_All();
                LesLocationsCollection collection = new LesLocationsCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesLocations obj = new LesLocations();
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
        
        public static LesLocations Load(System.Nullable<int> LOCATIONID) {
            MetroLesMonitor.Dal.LesLocations dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesLocations();
                System.Data.DataSet ds = dbo.LES_LOCATIONS_Select_One(LOCATIONID);
                LesLocations obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new LesLocations();
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
            MetroLesMonitor.Dal.LesLocations dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesLocations();
                System.Data.DataSet ds = dbo.LES_LOCATIONS_Select_One(this.Locationid);
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
            MetroLesMonitor.Dal.LesLocations dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesLocations();
                dbo.LES_LOCATIONS_Insert(this.Loccode, this.LocDescription);
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
            MetroLesMonitor.Dal.LesLocations dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesLocations();
                dbo.LES_LOCATIONS_Delete(this.Locationid);
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
            MetroLesMonitor.Dal.LesLocations dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesLocations();
                dbo.LES_LOCATIONS_Update(this.Locationid, this.Loccode, this.LocDescription);
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
