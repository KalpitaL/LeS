namespace MetroLesMonitor.Bll {
    
    
    public partial class SmBuyerSupplierItemRefTemp {
        
        private System.Nullable<int> _refid;
        
        private string _reftype;
        
        private string _buyerRef;
        
        private string _supplierRef;
        
        private string _itemDesc;
        
        private string _comments;
        
        private System.Nullable<int> _buyerId;
        
        private System.Nullable<int> _supplierId;
        
        public virtual System.Nullable<int> Refid {
            get {
                return _refid;
            }
            set {
                _refid = value;
            }
        }
        
        public virtual string Reftype {
            get {
                return _reftype;
            }
            set {
                _reftype = value;
            }
        }
        
        public virtual string BuyerRef {
            get {
                return _buyerRef;
            }
            set {
                _buyerRef = value;
            }
        }
        
        public virtual string SupplierRef {
            get {
                return _supplierRef;
            }
            set {
                _supplierRef = value;
            }
        }
        
        public virtual string ItemDesc {
            get {
                return _itemDesc;
            }
            set {
                _itemDesc = value;
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
        
        public virtual System.Nullable<int> BuyerId {
            get {
                return _buyerId;
            }
            set {
                _buyerId = value;
            }
        }
        
        public virtual System.Nullable<int> SupplierId {
            get {
                return _supplierId;
            }
            set {
                _supplierId = value;
            }
        }
        
        private void Clean() {
            this.Refid = null;
            this.Reftype = string.Empty;
            this.BuyerRef = string.Empty;
            this.SupplierRef = string.Empty;
            this.ItemDesc = string.Empty;
            this.Comments = string.Empty;
            this.BuyerId = null;
            this.SupplierId = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["REFID"] != System.DBNull.Value)) {
                this.Refid = ((System.Nullable<int>)(dr["REFID"]));
            }
            if ((dr["REFTYPE"] != System.DBNull.Value)) {
                this.Reftype = ((string)(dr["REFTYPE"]));
            }
            if ((dr["BUYER_REF"] != System.DBNull.Value)) {
                this.BuyerRef = ((string)(dr["BUYER_REF"]));
            }
            if ((dr["SUPPLIER_REF"] != System.DBNull.Value)) {
                this.SupplierRef = ((string)(dr["SUPPLIER_REF"]));
            }
            if ((dr["ITEM_DESC"] != System.DBNull.Value)) {
                this.ItemDesc = ((string)(dr["ITEM_DESC"]));
            }
            if ((dr["COMMENTS"] != System.DBNull.Value)) {
                this.Comments = ((string)(dr["COMMENTS"]));
            }
            if ((dr["BUYER_ID"] != System.DBNull.Value)) {
                this.BuyerId = ((System.Nullable<int>)(dr["BUYER_ID"]));
            }
            if ((dr["SUPPLIER_ID"] != System.DBNull.Value)) {
                this.SupplierId = ((System.Nullable<int>)(dr["SUPPLIER_ID"]));
            }
        }
        
        public static SmBuyerSupplierItemRefTempCollection GetAll() {
            MetroLesMonitor.Dal.SmBuyerSupplierItemRefTemp dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierItemRefTemp();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_ITEM_REF_TEMP_Select_All();
                SmBuyerSupplierItemRefTempCollection collection = new SmBuyerSupplierItemRefTempCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmBuyerSupplierItemRefTemp obj = new SmBuyerSupplierItemRefTemp();
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
        
        public virtual void Insert() {
            MetroLesMonitor.Dal.SmBuyerSupplierItemRefTemp dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierItemRefTemp();
                dbo.SM_BUYER_SUPPLIER_ITEM_REF_TEMP_Insert(this.Refid, this.Reftype, this.BuyerRef, this.SupplierRef, this.ItemDesc, this.Comments, this.BuyerId, this.SupplierId);
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
