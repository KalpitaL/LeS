namespace MetroLesMonitor.Bll {
    
    
    public partial class LesInventorylocation {
        
        private System.Nullable<int> _inventorylocationid;
        
        private System.Nullable<int> _locationid;
        
        private System.Nullable<int> _inventoryid;
        
        private System.Nullable<float> _stock;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private System.Nullable<int> _updatedBy;
        
        private System.Nullable<int> _createdBy;
        
        private LesInventory _lesInventory;
        
        private LesLocations _lesLocations;
        
        public virtual System.Nullable<int> Inventorylocationid {
            get {
                return _inventorylocationid;
            }
            set {
                _inventorylocationid = value;
            }
        }
        
        public virtual System.Nullable<float> Stock {
            get {
                return _stock;
            }
            set {
                _stock = value;
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
        
        public virtual LesInventory LesInventory {
            get {
                if ((this._lesInventory == null)) {
                    this._lesInventory = MetroLesMonitor.Bll.LesInventory.Load(this._inventoryid);
                }
                return this._lesInventory;
            }
            set {
                _lesInventory = value;
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
        
        private void Clean() {
            this.Inventorylocationid = null;
            this._locationid = null;
            this._inventoryid = null;
            this.Stock = null;
            this.UpdateDate = null;
            this.CreatedDate = null;
            this.UpdatedBy = null;
            this.CreatedBy = null;
            this.LesInventory = null;
            this.LesLocations = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["INVENTORYLOCATIONID"] != System.DBNull.Value)) {
                this.Inventorylocationid = ((System.Nullable<int>)(dr["INVENTORYLOCATIONID"]));
            }
            if ((dr["LOCATIONID"] != System.DBNull.Value)) {
                this._locationid = ((System.Nullable<int>)(dr["LOCATIONID"]));
            }
            if ((dr["INVENTORYID"] != System.DBNull.Value)) {
                this._inventoryid = ((System.Nullable<int>)(dr["INVENTORYID"]));
            }
            if ((dr["STOCK"] != System.DBNull.Value)) {
                this.Stock = ((System.Nullable<float>)(dr["STOCK"]));
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
        
        public static LesInventorylocationCollection Select_LES_INVENTORYLOCATIONs_By_INVENTORYID(System.Nullable<int> INVENTORYID) {
            MetroLesMonitor.Dal.LesInventorylocation dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventorylocation();
                System.Data.DataSet ds = dbo.Select_LES_INVENTORYLOCATIONs_By_INVENTORYID(INVENTORYID);
                LesInventorylocationCollection collection = new LesInventorylocationCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesInventorylocation obj = new LesInventorylocation();
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
        
        public static LesInventorylocationCollection Select_LES_INVENTORYLOCATIONs_By_LOCATIONID(System.Nullable<int> LOCATIONID) {
            MetroLesMonitor.Dal.LesInventorylocation dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventorylocation();
                System.Data.DataSet ds = dbo.Select_LES_INVENTORYLOCATIONs_By_LOCATIONID(LOCATIONID);
                LesInventorylocationCollection collection = new LesInventorylocationCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesInventorylocation obj = new LesInventorylocation();
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
        
        public static LesInventorylocationCollection GetAll() {
            MetroLesMonitor.Dal.LesInventorylocation dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventorylocation();
                System.Data.DataSet ds = dbo.LES_INVENTORYLOCATION_Select_All();
                LesInventorylocationCollection collection = new LesInventorylocationCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesInventorylocation obj = new LesInventorylocation();
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
        
        public static LesInventorylocation Load(System.Nullable<int> INVENTORYLOCATIONID) {
            MetroLesMonitor.Dal.LesInventorylocation dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventorylocation();
                System.Data.DataSet ds = dbo.LES_INVENTORYLOCATION_Select_One(INVENTORYLOCATIONID);
                LesInventorylocation obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new LesInventorylocation();
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
            MetroLesMonitor.Dal.LesInventorylocation dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventorylocation();
                System.Data.DataSet ds = dbo.LES_INVENTORYLOCATION_Select_One(this.Inventorylocationid);
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
            MetroLesMonitor.Dal.LesInventorylocation dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventorylocation();
                dbo.LES_INVENTORYLOCATION_Insert(this._locationid, this._inventoryid, this.Stock, this.UpdateDate, this.CreatedDate, this.UpdatedBy, this.CreatedBy);
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
            MetroLesMonitor.Dal.LesInventorylocation dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventorylocation();
                dbo.LES_INVENTORYLOCATION_Delete(this.Inventorylocationid);
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
            MetroLesMonitor.Dal.LesInventorylocation dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventorylocation();
                dbo.LES_INVENTORYLOCATION_Update(this.Inventorylocationid, this._locationid, this._inventoryid, this.Stock, this.UpdateDate, this.CreatedDate, this.UpdatedBy, this.CreatedBy);
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
