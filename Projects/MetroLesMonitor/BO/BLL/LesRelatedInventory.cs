namespace MetroLesMonitor.Bll {
    
    
    public partial class LesRelatedInventory {
        
        private System.Nullable<int> _relateditemid;
        
        private System.Nullable<int> _inventoryid;
        
        private System.Nullable<int> _relatedInventoryid;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private System.Nullable<int> _createdBy;
        
        public virtual System.Nullable<int> Relateditemid {
            get {
                return _relateditemid;
            }
            set {
                _relateditemid = value;
            }
        }
        
        public virtual System.Nullable<int> Inventoryid {
            get {
                return _inventoryid;
            }
            set {
                _inventoryid = value;
            }
        }
        
        public virtual System.Nullable<int> RelatedInventoryid {
            get {
                return _relatedInventoryid;
            }
            set {
                _relatedInventoryid = value;
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
        
        public virtual System.Nullable<int> CreatedBy {
            get {
                return _createdBy;
            }
            set {
                _createdBy = value;
            }
        }
        
        private void Clean() {
            this.Relateditemid = null;
            this.Inventoryid = null;
            this.RelatedInventoryid = null;
            this.CreatedDate = null;
            this.CreatedBy = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["RELATEDITEMID"] != System.DBNull.Value)) {
                this.Relateditemid = ((System.Nullable<int>)(dr["RELATEDITEMID"]));
            }
            if ((dr["INVENTORYID"] != System.DBNull.Value)) {
                this.Inventoryid = ((System.Nullable<int>)(dr["INVENTORYID"]));
            }
            if ((dr["RELATED_INVENTORYID"] != System.DBNull.Value)) {
                this.RelatedInventoryid = ((System.Nullable<int>)(dr["RELATED_INVENTORYID"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value)) {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["CREATED_BY"] != System.DBNull.Value)) {
                this.CreatedBy = ((System.Nullable<int>)(dr["CREATED_BY"]));
            }
        }
        
        public static LesRelatedInventoryCollection GetAll() {
            MetroLesMonitor.Dal.LesRelatedInventory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesRelatedInventory();
                System.Data.DataSet ds = dbo.LES_RELATED_INVENTORY_Select_All();
                LesRelatedInventoryCollection collection = new LesRelatedInventoryCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesRelatedInventory obj = new LesRelatedInventory();
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
        
        public static LesRelatedInventory Load(System.Nullable<int> RELATEDITEMID) {
            MetroLesMonitor.Dal.LesRelatedInventory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesRelatedInventory();
                System.Data.DataSet ds = dbo.LES_RELATED_INVENTORY_Select_One(RELATEDITEMID);
                LesRelatedInventory obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new LesRelatedInventory();
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
            MetroLesMonitor.Dal.LesRelatedInventory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesRelatedInventory();
                System.Data.DataSet ds = dbo.LES_RELATED_INVENTORY_Select_One(this.Relateditemid);
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
            MetroLesMonitor.Dal.LesRelatedInventory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesRelatedInventory();
                dbo.LES_RELATED_INVENTORY_Insert(this.Inventoryid, this.RelatedInventoryid, this.CreatedDate, this.CreatedBy);
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
            MetroLesMonitor.Dal.LesRelatedInventory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesRelatedInventory();
                dbo.LES_RELATED_INVENTORY_Delete(this.Relateditemid);
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
            MetroLesMonitor.Dal.LesRelatedInventory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesRelatedInventory();
                dbo.LES_RELATED_INVENTORY_Update(this.Relateditemid, this.Inventoryid, this.RelatedInventoryid, this.CreatedDate, this.CreatedBy);
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
