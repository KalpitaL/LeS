namespace MetroLesMonitor.Bll {
    
    
    public partial class LesInventorySupplierLink {
        
        private System.Nullable<int> _invsupid;
        
        private System.Nullable<int> _inventoryid;
        
        private System.Nullable<int> _supplierid;
        
        private System.Nullable<int> _ispreferred;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private System.Nullable<int> _updatedBy;
        
        private System.Nullable<int> _createdBy;
        
        private LesInventory _lesInventory;
        
        private SmAddress _smAddress;
        
        public virtual System.Nullable<int> Invsupid {
            get {
                return _invsupid;
            }
            set {
                _invsupid = value;
            }
        }
        
        public virtual System.Nullable<int> Ispreferred {
            get {
                return _ispreferred;
            }
            set {
                _ispreferred = value;
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
            this.Invsupid = null;
            this._inventoryid = null;
            this._supplierid = null;
            this.Ispreferred = null;
            this.UpdateDate = null;
            this.CreatedDate = null;
            this.UpdatedBy = null;
            this.CreatedBy = null;
            this.LesInventory = null;
            this.SmAddress = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["INVSUPID"] != System.DBNull.Value)) {
                this.Invsupid = ((System.Nullable<int>)(dr["INVSUPID"]));
            }
            if ((dr["INVENTORYID"] != System.DBNull.Value)) {
                this._inventoryid = ((System.Nullable<int>)(dr["INVENTORYID"]));
            }
            if ((dr["SUPPLIERID"] != System.DBNull.Value)) {
                this._supplierid = ((System.Nullable<int>)(dr["SUPPLIERID"]));
            }
            if ((dr["ISPREFERRED"] != System.DBNull.Value)) {
                this.Ispreferred = ((System.Nullable<int>)(dr["ISPREFERRED"]));
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
        
        public static LesInventorySupplierLinkCollection Select_LES_INVENTORY_SUPPLIER_LINKs_By_INVENTORYID(System.Nullable<int> INVENTORYID) {
            MetroLesMonitor.Dal.LesInventorySupplierLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventorySupplierLink();
                System.Data.DataSet ds = dbo.Select_LES_INVENTORY_SUPPLIER_LINKs_By_INVENTORYID(INVENTORYID);
                LesInventorySupplierLinkCollection collection = new LesInventorySupplierLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesInventorySupplierLink obj = new LesInventorySupplierLink();
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
        
        public static LesInventorySupplierLinkCollection Select_LES_INVENTORY_SUPPLIER_LINKs_By_SUPPLIERID(System.Nullable<int> SUPPLIERID) {
            MetroLesMonitor.Dal.LesInventorySupplierLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventorySupplierLink();
                System.Data.DataSet ds = dbo.Select_LES_INVENTORY_SUPPLIER_LINKs_By_SUPPLIERID(SUPPLIERID);
                LesInventorySupplierLinkCollection collection = new LesInventorySupplierLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesInventorySupplierLink obj = new LesInventorySupplierLink();
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
        
        public static LesInventorySupplierLinkCollection GetAll() {
            MetroLesMonitor.Dal.LesInventorySupplierLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventorySupplierLink();
                System.Data.DataSet ds = dbo.LES_INVENTORY_SUPPLIER_LINK_Select_All();
                LesInventorySupplierLinkCollection collection = new LesInventorySupplierLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesInventorySupplierLink obj = new LesInventorySupplierLink();
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
        
        public static LesInventorySupplierLink Load(System.Nullable<int> INVSUPID) {
            MetroLesMonitor.Dal.LesInventorySupplierLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventorySupplierLink();
                System.Data.DataSet ds = dbo.LES_INVENTORY_SUPPLIER_LINK_Select_One(INVSUPID);
                LesInventorySupplierLink obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new LesInventorySupplierLink();
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
            MetroLesMonitor.Dal.LesInventorySupplierLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventorySupplierLink();
                System.Data.DataSet ds = dbo.LES_INVENTORY_SUPPLIER_LINK_Select_One(this.Invsupid);
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
            MetroLesMonitor.Dal.LesInventorySupplierLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventorySupplierLink();
                dbo.LES_INVENTORY_SUPPLIER_LINK_Insert(this._inventoryid, this._supplierid, this.Ispreferred, this.UpdateDate, this.CreatedDate, this.UpdatedBy, this.CreatedBy);
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
            MetroLesMonitor.Dal.LesInventorySupplierLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventorySupplierLink();
                dbo.LES_INVENTORY_SUPPLIER_LINK_Delete(this.Invsupid);
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
            MetroLesMonitor.Dal.LesInventorySupplierLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventorySupplierLink();
                dbo.LES_INVENTORY_SUPPLIER_LINK_Update(this.Invsupid, this._inventoryid, this._supplierid, this.Ispreferred, this.UpdateDate, this.CreatedDate, this.UpdatedBy, this.CreatedBy);
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
