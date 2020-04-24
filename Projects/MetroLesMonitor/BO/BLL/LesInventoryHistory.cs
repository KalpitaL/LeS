namespace MetroLesMonitor.Bll {
    
    
    public partial class LesInventoryHistory {
        
        private System.Nullable<int> _invhistoryid;
        
        private System.Nullable<int> _inventoryid;
        
        private System.Nullable<int> _currencyid;
        
        private System.Nullable<int> _historyType;
        
        private System.Nullable<float> _quantity;
        
        private System.Nullable<float> _unitPrice;
        
        private System.Nullable<int> _supplierid;
        
        private System.Nullable<int> _supunitid;
        
        private System.Nullable<int> _used;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private System.Nullable<int> _updatedBy;
        
        private System.Nullable<int> _createdBy;
        
        public virtual System.Nullable<int> Invhistoryid {
            get {
                return _invhistoryid;
            }
            set {
                _invhistoryid = value;
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
        
        public virtual System.Nullable<int> Currencyid {
            get {
                return _currencyid;
            }
            set {
                _currencyid = value;
            }
        }
        
        public virtual System.Nullable<int> HistoryType {
            get {
                return _historyType;
            }
            set {
                _historyType = value;
            }
        }
        
        public virtual System.Nullable<float> Quantity {
            get {
                return _quantity;
            }
            set {
                _quantity = value;
            }
        }
        
        public virtual System.Nullable<float> UnitPrice {
            get {
                return _unitPrice;
            }
            set {
                _unitPrice = value;
            }
        }
        
        public virtual System.Nullable<int> Supplierid {
            get {
                return _supplierid;
            }
            set {
                _supplierid = value;
            }
        }
        
        public virtual System.Nullable<int> Supunitid {
            get {
                return _supunitid;
            }
            set {
                _supunitid = value;
            }
        }
        
        public virtual System.Nullable<int> Used {
            get {
                return _used;
            }
            set {
                _used = value;
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
        
        private void Clean() {
            this.Invhistoryid = null;
            this.Inventoryid = null;
            this.Currencyid = null;
            this.HistoryType = null;
            this.Quantity = null;
            this.UnitPrice = null;
            this.Supplierid = null;
            this.Supunitid = null;
            this.Used = null;
            this.UpdateDate = null;
            this.CreatedDate = null;
            this.UpdatedBy = null;
            this.CreatedBy = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["INVHISTORYID"] != System.DBNull.Value)) {
                this.Invhistoryid = ((System.Nullable<int>)(dr["INVHISTORYID"]));
            }
            if ((dr["INVENTORYID"] != System.DBNull.Value)) {
                this.Inventoryid = ((System.Nullable<int>)(dr["INVENTORYID"]));
            }
            if ((dr["CURRENCYID"] != System.DBNull.Value)) {
                this.Currencyid = ((System.Nullable<int>)(dr["CURRENCYID"]));
            }
            if ((dr["HISTORY_TYPE"] != System.DBNull.Value)) {
                this.HistoryType = ((System.Nullable<int>)(dr["HISTORY_TYPE"]));
            }
            if ((dr["QUANTITY"] != System.DBNull.Value)) {
                this.Quantity = ((System.Nullable<float>)(dr["QUANTITY"]));
            }
            if ((dr["UNIT_PRICE"] != System.DBNull.Value)) {
                this.UnitPrice = ((System.Nullable<float>)(dr["UNIT_PRICE"]));
            }
            if ((dr["SUPPLIERID"] != System.DBNull.Value)) {
                this.Supplierid = ((System.Nullable<int>)(dr["SUPPLIERID"]));
            }
            if ((dr["SUPUNITID"] != System.DBNull.Value)) {
                this.Supunitid = ((System.Nullable<int>)(dr["SUPUNITID"]));
            }
            if ((dr["USED"] != System.DBNull.Value)) {
                this.Used = ((System.Nullable<int>)(dr["USED"]));
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
        
        public static LesInventoryHistoryCollection GetAll() {
            MetroLesMonitor.Dal.LesInventoryHistory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventoryHistory();
                System.Data.DataSet ds = dbo.LES_INVENTORY_HISTORY_Select_All();
                LesInventoryHistoryCollection collection = new LesInventoryHistoryCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesInventoryHistory obj = new LesInventoryHistory();
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
        
        public static LesInventoryHistory Load(System.Nullable<int> INVHISTORYID) {
            MetroLesMonitor.Dal.LesInventoryHistory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventoryHistory();
                System.Data.DataSet ds = dbo.LES_INVENTORY_HISTORY_Select_One(INVHISTORYID);
                LesInventoryHistory obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new LesInventoryHistory();
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
            MetroLesMonitor.Dal.LesInventoryHistory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventoryHistory();
                System.Data.DataSet ds = dbo.LES_INVENTORY_HISTORY_Select_One(this.Invhistoryid);
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
            MetroLesMonitor.Dal.LesInventoryHistory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventoryHistory();
                dbo.LES_INVENTORY_HISTORY_Insert(this.Inventoryid, this.Currencyid, this.HistoryType, this.Quantity, this.UnitPrice, this.Supplierid, this.Supunitid, this.Used, this.UpdateDate, this.CreatedDate, this.UpdatedBy, this.CreatedBy);
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
            MetroLesMonitor.Dal.LesInventoryHistory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventoryHistory();
                dbo.LES_INVENTORY_HISTORY_Delete(this.Invhistoryid);
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
            MetroLesMonitor.Dal.LesInventoryHistory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventoryHistory();
                dbo.LES_INVENTORY_HISTORY_Update(this.Invhistoryid, this.Inventoryid, this.Currencyid, this.HistoryType, this.Quantity, this.UnitPrice, this.Supplierid, this.Supunitid, this.Used, this.UpdateDate, this.CreatedDate, this.UpdatedBy, this.CreatedBy);
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
