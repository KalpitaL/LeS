namespace MetroLesMonitor.Bll {
    
    
    public partial class SmShipmentItems {
        
        private System.Nullable<int> _shipmentItemId;
        
        private System.Nullable<int> _shipmentId;
        
        private System.Nullable<int> _itemstatus;
        
        private System.Nullable<int> _itemno;
        
        private string _vendorItemno;
        
        private System.Nullable<float> _qtyOrd;
        
        private System.Nullable<float> _qtyShipment;
        
        private System.Nullable<float> _quotedPrice;
        
        private System.Nullable<float> _quoteExchrate;
        
        private System.Nullable<float> _discount;
        
        private System.Nullable<int> _deliverytime;
        
        private string _partname;
        
        private string _drawingno;
        
        private string _posno;
        
        private string _refno;
        
        private string _unitCode;
        
        private System.Nullable<int> _changedByVendor;
        
        private string _itemRemark;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private string _vendorRefno;
        
        private string _originatingsystemref;
        
        private System.Nullable<int> _sysItemno;
        
        private string _itemType;
        
        private string _udf1;
        
        private string _udf2;
        
        private string _udf3;
        
        private string _supplierorgref;
        
        private string _vendorItemname;
        
        private string _buyerorgref;
        
        private SmShipmentDocuments _smShipmentDocuments;
        
        public virtual System.Nullable<int> ShipmentItemId {
            get {
                return _shipmentItemId;
            }
            set {
                _shipmentItemId = value;
            }
        }
        
        public virtual System.Nullable<int> Itemstatus {
            get {
                return _itemstatus;
            }
            set {
                _itemstatus = value;
            }
        }
        
        public virtual System.Nullable<int> Itemno {
            get {
                return _itemno;
            }
            set {
                _itemno = value;
            }
        }
        
        public virtual string VendorItemno {
            get {
                return _vendorItemno;
            }
            set {
                _vendorItemno = value;
            }
        }
        
        public virtual System.Nullable<float> QtyOrd {
            get {
                return _qtyOrd;
            }
            set {
                _qtyOrd = value;
            }
        }
        
        public virtual System.Nullable<float> QtyShipment {
            get {
                return _qtyShipment;
            }
            set {
                _qtyShipment = value;
            }
        }
        
        public virtual System.Nullable<float> QuotedPrice {
            get {
                return _quotedPrice;
            }
            set {
                _quotedPrice = value;
            }
        }
        
        public virtual System.Nullable<float> QuoteExchrate {
            get {
                return _quoteExchrate;
            }
            set {
                _quoteExchrate = value;
            }
        }
        
        public virtual System.Nullable<float> Discount {
            get {
                return _discount;
            }
            set {
                _discount = value;
            }
        }
        
        public virtual System.Nullable<int> Deliverytime {
            get {
                return _deliverytime;
            }
            set {
                _deliverytime = value;
            }
        }
        
        public virtual string Partname {
            get {
                return _partname;
            }
            set {
                _partname = value;
            }
        }
        
        public virtual string Drawingno {
            get {
                return _drawingno;
            }
            set {
                _drawingno = value;
            }
        }
        
        public virtual string Posno {
            get {
                return _posno;
            }
            set {
                _posno = value;
            }
        }
        
        public virtual string Refno {
            get {
                return _refno;
            }
            set {
                _refno = value;
            }
        }
        
        public virtual string UnitCode {
            get {
                return _unitCode;
            }
            set {
                _unitCode = value;
            }
        }
        
        public virtual System.Nullable<int> ChangedByVendor {
            get {
                return _changedByVendor;
            }
            set {
                _changedByVendor = value;
            }
        }
        
        public virtual string ItemRemark {
            get {
                return _itemRemark;
            }
            set {
                _itemRemark = value;
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
        
        public virtual string VendorRefno {
            get {
                return _vendorRefno;
            }
            set {
                _vendorRefno = value;
            }
        }
        
        public virtual string Originatingsystemref {
            get {
                return _originatingsystemref;
            }
            set {
                _originatingsystemref = value;
            }
        }
        
        public virtual System.Nullable<int> SysItemno {
            get {
                return _sysItemno;
            }
            set {
                _sysItemno = value;
            }
        }
        
        public virtual string ItemType {
            get {
                return _itemType;
            }
            set {
                _itemType = value;
            }
        }
        
        public virtual string Udf1 {
            get {
                return _udf1;
            }
            set {
                _udf1 = value;
            }
        }
        
        public virtual string Udf2 {
            get {
                return _udf2;
            }
            set {
                _udf2 = value;
            }
        }
        
        public virtual string Udf3 {
            get {
                return _udf3;
            }
            set {
                _udf3 = value;
            }
        }
        
        public virtual string Supplierorgref {
            get {
                return _supplierorgref;
            }
            set {
                _supplierorgref = value;
            }
        }
        
        public virtual string VendorItemname {
            get {
                return _vendorItemname;
            }
            set {
                _vendorItemname = value;
            }
        }
        
        public virtual string Buyerorgref {
            get {
                return _buyerorgref;
            }
            set {
                _buyerorgref = value;
            }
        }
        
        public virtual SmShipmentDocuments SmShipmentDocuments {
            get {
                if ((this._smShipmentDocuments == null)) {
                    this._smShipmentDocuments = MetroLesMonitor.Bll.SmShipmentDocuments.Load(this._shipmentId);
                }
                return this._smShipmentDocuments;
            }
            set {
                _smShipmentDocuments = value;
            }
        }
        
        private void Clean() {
            this.ShipmentItemId = null;
            this._shipmentId = null;
            this.Itemstatus = null;
            this.Itemno = null;
            this.VendorItemno = string.Empty;
            this.QtyOrd = null;
            this.QtyShipment = null;
            this.QuotedPrice = null;
            this.QuoteExchrate = null;
            this.Discount = null;
            this.Deliverytime = null;
            this.Partname = string.Empty;
            this.Drawingno = string.Empty;
            this.Posno = string.Empty;
            this.Refno = string.Empty;
            this.UnitCode = string.Empty;
            this.ChangedByVendor = null;
            this.ItemRemark = string.Empty;
            this.UpdateDate = null;
            this.CreatedDate = null;
            this.VendorRefno = string.Empty;
            this.Originatingsystemref = string.Empty;
            this.SysItemno = null;
            this.ItemType = string.Empty;
            this.Udf1 = string.Empty;
            this.Udf2 = string.Empty;
            this.Udf3 = string.Empty;
            this.Supplierorgref = string.Empty;
            this.VendorItemname = string.Empty;
            this.Buyerorgref = string.Empty;
            this.SmShipmentDocuments = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["SHIPMENT_ITEM_ID"] != System.DBNull.Value)) {
                this.ShipmentItemId = ((System.Nullable<int>)(dr["SHIPMENT_ITEM_ID"]));
            }
            if ((dr["SHIPMENT_ID"] != System.DBNull.Value)) {
                this._shipmentId = ((System.Nullable<int>)(dr["SHIPMENT_ID"]));
            }
            if ((dr["ITEMSTATUS"] != System.DBNull.Value)) {
                this.Itemstatus = ((System.Nullable<int>)(dr["ITEMSTATUS"]));
            }
            if ((dr["ITEMNO"] != System.DBNull.Value)) {
                this.Itemno = ((System.Nullable<int>)(dr["ITEMNO"]));
            }
            if ((dr["VENDOR_ITEMNO"] != System.DBNull.Value)) {
                this.VendorItemno = ((string)(dr["VENDOR_ITEMNO"]));
            }
            if ((dr["QTY_ORD"] != System.DBNull.Value)) {
                this.QtyOrd = ((System.Nullable<float>)(dr["QTY_ORD"]));
            }
            if ((dr["QTY_SHIPMENT"] != System.DBNull.Value)) {
                this.QtyShipment = ((System.Nullable<float>)(dr["QTY_SHIPMENT"]));
            }
            if ((dr["QUOTED_PRICE"] != System.DBNull.Value)) {
                this.QuotedPrice = ((System.Nullable<float>)(dr["QUOTED_PRICE"]));
            }
            if ((dr["QUOTE_EXCHRATE"] != System.DBNull.Value)) {
                this.QuoteExchrate = ((System.Nullable<float>)(dr["QUOTE_EXCHRATE"]));
            }
            if ((dr["DISCOUNT"] != System.DBNull.Value)) {
                this.Discount = ((System.Nullable<float>)(dr["DISCOUNT"]));
            }
            if ((dr["DELIVERYTIME"] != System.DBNull.Value)) {
                this.Deliverytime = ((System.Nullable<int>)(dr["DELIVERYTIME"]));
            }
            if ((dr["PARTNAME"] != System.DBNull.Value)) {
                this.Partname = ((string)(dr["PARTNAME"]));
            }
            if ((dr["DRAWINGNO"] != System.DBNull.Value)) {
                this.Drawingno = ((string)(dr["DRAWINGNO"]));
            }
            if ((dr["POSNO"] != System.DBNull.Value)) {
                this.Posno = ((string)(dr["POSNO"]));
            }
            if ((dr["REFNO"] != System.DBNull.Value)) {
                this.Refno = ((string)(dr["REFNO"]));
            }
            if ((dr["UNIT_CODE"] != System.DBNull.Value)) {
                this.UnitCode = ((string)(dr["UNIT_CODE"]));
            }
            if ((dr["CHANGED_BY_VENDOR"] != System.DBNull.Value)) {
                this.ChangedByVendor = ((System.Nullable<int>)(dr["CHANGED_BY_VENDOR"]));
            }
            if ((dr["ITEM_REMARK"] != System.DBNull.Value)) {
                this.ItemRemark = ((string)(dr["ITEM_REMARK"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value)) {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value)) {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["VENDOR_REFNO"] != System.DBNull.Value)) {
                this.VendorRefno = ((string)(dr["VENDOR_REFNO"]));
            }
            if ((dr["ORIGINATINGSYSTEMREF"] != System.DBNull.Value)) {
                this.Originatingsystemref = ((string)(dr["ORIGINATINGSYSTEMREF"]));
            }
            if ((dr["SYS_ITEMNO"] != System.DBNull.Value)) {
                this.SysItemno = ((System.Nullable<int>)(dr["SYS_ITEMNO"]));
            }
            if ((dr["ITEM_TYPE"] != System.DBNull.Value)) {
                this.ItemType = ((string)(dr["ITEM_TYPE"]));
            }
            if ((dr["UDF1"] != System.DBNull.Value)) {
                this.Udf1 = ((string)(dr["UDF1"]));
            }
            if ((dr["UDF2"] != System.DBNull.Value)) {
                this.Udf2 = ((string)(dr["UDF2"]));
            }
            if ((dr["UDF3"] != System.DBNull.Value)) {
                this.Udf3 = ((string)(dr["UDF3"]));
            }
            if ((dr["SupplierORGRef"] != System.DBNull.Value)) {
                this.Supplierorgref = ((string)(dr["SupplierORGRef"]));
            }
            if ((dr["VENDOR_ITEMNAME"] != System.DBNull.Value)) {
                this.VendorItemname = ((string)(dr["VENDOR_ITEMNAME"]));
            }
            if ((dr["BuyerORGRef"] != System.DBNull.Value)) {
                this.Buyerorgref = ((string)(dr["BuyerORGRef"]));
            }
        }
        
        public static SmShipmentItemsCollection Select_SM_SHIPMENT_ITEMSs_By_SHIPMENT_ID(System.Nullable<int> SHIPMENT_ID) {
            MetroLesMonitor.Dal.SmShipmentItems dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmShipmentItems();
                System.Data.DataSet ds = dbo.Select_SM_SHIPMENT_ITEMSs_By_SHIPMENT_ID(SHIPMENT_ID);
                SmShipmentItemsCollection collection = new SmShipmentItemsCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmShipmentItems obj = new SmShipmentItems();
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
        
        public static SmShipmentItemsCollection GetAll() {
            MetroLesMonitor.Dal.SmShipmentItems dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmShipmentItems();
                System.Data.DataSet ds = dbo.SM_SHIPMENT_ITEMS_Select_All();
                SmShipmentItemsCollection collection = new SmShipmentItemsCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmShipmentItems obj = new SmShipmentItems();
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
        
        public static SmShipmentItems Load(System.Nullable<int> SHIPMENT_ITEM_ID) {
            MetroLesMonitor.Dal.SmShipmentItems dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmShipmentItems();
                System.Data.DataSet ds = dbo.SM_SHIPMENT_ITEMS_Select_One(SHIPMENT_ITEM_ID);
                SmShipmentItems obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmShipmentItems();
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
            MetroLesMonitor.Dal.SmShipmentItems dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmShipmentItems();
                System.Data.DataSet ds = dbo.SM_SHIPMENT_ITEMS_Select_One(this.ShipmentItemId);
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
            MetroLesMonitor.Dal.SmShipmentItems dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmShipmentItems();
                dbo.SM_SHIPMENT_ITEMS_Insert(this._shipmentId, this.Itemstatus, this.Itemno, this.VendorItemno, this.QtyOrd, this.QtyShipment, this.QuotedPrice, this.QuoteExchrate, this.Discount, this.Deliverytime, this.Partname, this.Drawingno, this.Posno, this.Refno, this.UnitCode, this.ChangedByVendor, this.ItemRemark, this.UpdateDate, this.CreatedDate, this.VendorRefno, this.Originatingsystemref, this.SysItemno, this.ItemType, this.Udf1, this.Udf2, this.Udf3, this.Supplierorgref, this.VendorItemname, this.Buyerorgref);
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
            MetroLesMonitor.Dal.SmShipmentItems dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmShipmentItems();
                dbo.SM_SHIPMENT_ITEMS_Delete(this.ShipmentItemId);
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
            MetroLesMonitor.Dal.SmShipmentItems dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmShipmentItems();
                dbo.SM_SHIPMENT_ITEMS_Update(this.ShipmentItemId, this._shipmentId, this.Itemstatus, this.Itemno, this.VendorItemno, this.QtyOrd, this.QtyShipment, this.QuotedPrice, this.QuoteExchrate, this.Discount, this.Deliverytime, this.Partname, this.Drawingno, this.Posno, this.Refno, this.UnitCode, this.ChangedByVendor, this.ItemRemark, this.UpdateDate, this.CreatedDate, this.VendorRefno, this.Originatingsystemref, this.SysItemno, this.ItemType, this.Udf1, this.Udf2, this.Udf3, this.Supplierorgref, this.VendorItemname, this.Buyerorgref);
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
