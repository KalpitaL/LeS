namespace MetroLesMonitor.Bll {
    
    
    public partial class LesSupplierLocationLink {
        
        private System.Nullable<int> _supploclinkid;
        
        private System.Nullable<int> _supplierid;
        
        private System.Nullable<int> _locationid;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private System.Nullable<int> _updatedBy;
        
        private System.Nullable<int> _createdBy;
        
        private LesLocations _lesLocations;
        
        private SmAddress _smAddress;
        
        public virtual System.Nullable<int> Supploclinkid {
            get {
                return _supploclinkid;
            }
            set {
                _supploclinkid = value;
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
        
        public virtual System.Nullable<System.DateTime> CreatedDate {
            get {
                return _createdDate;
            }
            set {
                _createdDate = value;
            }
        }
        
        public virtual System.Nullable<int> UpdatedBy {
            get {
                return _updatedBy;
            }
            set {
                _updatedBy = value;
            }
        }
        
        public virtual System.Nullable<int> CreatedBy {
            get {
                return _createdBy;
            }
            set {
                _createdBy = value;
            }
        }
        
        public virtual LesLocations LesLocations {
            get {
                if ((this._lesLocations == null)) {
                    this._lesLocations = MetroLesMonitor.Bll.LesLocations.Load(this._locationid);
                }
                return this._lesLocations;
            }
            set {
                _lesLocations = value;
            }
        }
        
        public virtual SmAddress SmAddress {
            get {
                if ((this._smAddress == null)) {
                    this._smAddress = MetroLesMonitor.Bll.SmAddress.Load(this._supplierid);
                }
                return this._smAddress;
            }
            set {
                _smAddress = value;
            }
        }
        
        private void Clean() {
            this.Supploclinkid = null;
            this._supplierid = null;
            this._locationid = null;
            this.UpdateDate = null;
            this.CreatedDate = null;
            this.UpdatedBy = null;
            this.CreatedBy = null;
            this.LesLocations = null;
            this.SmAddress = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["SUPPLOCLINKID"] != System.DBNull.Value)) {
                this.Supploclinkid = ((System.Nullable<int>)(dr["SUPPLOCLINKID"]));
            }
            if ((dr["SUPPLIERID"] != System.DBNull.Value)) {
                this._supplierid = ((System.Nullable<int>)(dr["SUPPLIERID"]));
            }
            if ((dr["LOCATIONID"] != System.DBNull.Value)) {
                this._locationid = ((System.Nullable<int>)(dr["LOCATIONID"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value)) {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value)) {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["UPDATED_BY"] != System.DBNull.Value)) {
                this.UpdatedBy = ((System.Nullable<int>)(dr["UPDATED_BY"]));
            }
            if ((dr["CREATED_BY"] != System.DBNull.Value)) {
                this.CreatedBy = ((System.Nullable<int>)(dr["CREATED_BY"]));
            }
        }
        
        public static LesSupplierLocationLinkCollection Select_LES_SUPPLIER_LOCATION_LINKs_By_LOCATIONID(System.Nullable<int> LOCATIONID) {
            MetroLesMonitor.Dal.LesSupplierLocationLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSupplierLocationLink();
                System.Data.DataSet ds = dbo.Select_LES_SUPPLIER_LOCATION_LINKs_By_LOCATIONID(LOCATIONID);
                LesSupplierLocationLinkCollection collection = new LesSupplierLocationLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesSupplierLocationLink obj = new LesSupplierLocationLink();
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
        
        public static LesSupplierLocationLinkCollection Select_LES_SUPPLIER_LOCATION_LINKs_By_SUPPLIERID(System.Nullable<int> SUPPLIERID) {
            MetroLesMonitor.Dal.LesSupplierLocationLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSupplierLocationLink();
                System.Data.DataSet ds = dbo.Select_LES_SUPPLIER_LOCATION_LINKs_By_SUPPLIERID(SUPPLIERID);
                LesSupplierLocationLinkCollection collection = new LesSupplierLocationLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesSupplierLocationLink obj = new LesSupplierLocationLink();
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
        
        public static LesSupplierLocationLinkCollection GetAll() {
            MetroLesMonitor.Dal.LesSupplierLocationLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSupplierLocationLink();
                System.Data.DataSet ds = dbo.LES_SUPPLIER_LOCATION_LINK_Select_All();
                LesSupplierLocationLinkCollection collection = new LesSupplierLocationLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesSupplierLocationLink obj = new LesSupplierLocationLink();
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
        
        public static LesSupplierLocationLink Load(System.Nullable<int> SUPPLOCLINKID) {
            MetroLesMonitor.Dal.LesSupplierLocationLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSupplierLocationLink();
                System.Data.DataSet ds = dbo.LES_SUPPLIER_LOCATION_LINK_Select_One(SUPPLOCLINKID);
                LesSupplierLocationLink obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new LesSupplierLocationLink();
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
            MetroLesMonitor.Dal.LesSupplierLocationLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSupplierLocationLink();
                System.Data.DataSet ds = dbo.LES_SUPPLIER_LOCATION_LINK_Select_One(this.Supploclinkid);
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
            MetroLesMonitor.Dal.LesSupplierLocationLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSupplierLocationLink();
                dbo.LES_SUPPLIER_LOCATION_LINK_Insert(this._supplierid, this._locationid, this.UpdateDate, this.CreatedDate, this.UpdatedBy, this.CreatedBy);
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
            MetroLesMonitor.Dal.LesSupplierLocationLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSupplierLocationLink();
                dbo.LES_SUPPLIER_LOCATION_LINK_Delete(this.Supploclinkid);
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
            MetroLesMonitor.Dal.LesSupplierLocationLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSupplierLocationLink();
                dbo.LES_SUPPLIER_LOCATION_LINK_Update(this.Supploclinkid, this._supplierid, this._locationid, this.UpdateDate, this.CreatedDate, this.UpdatedBy, this.CreatedBy);
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
