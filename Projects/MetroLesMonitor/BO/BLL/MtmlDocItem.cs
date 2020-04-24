namespace MetroLesMonitor.Bll {
    
    
    public partial class MtmlDocItem {
        
        private System.Nullable<System.Guid> _mtmldocitemid;
        
        private System.Nullable<System.Guid> _mtmldocid;
        
        private string _itemDescription;
        
        private string _partnumber;
        
        private string _partunit;
        
        private System.Nullable<int> _lineitemnumber;
        
        private string _priority;
        
        private string _quantity;
        
        private string _partcode;
        
        private string _comments;
        
        private System.Nullable<float> _unitprice;
        
        private System.Nullable<float> _listprice;
        
        private System.Nullable<float> _discountedamount;
        
        private string _originatingsystemref;
        
        private System.Nullable<int> _sysItemno;
        
        private string _supplierRefno;
        
        private string _supplierRemarks;
        
        private System.Nullable<int> _deliverytime;
        
        private System.Nullable<int> _autoid;
        
        private string _vendorItemno;
        
        private string _itemtype;
        
        private string _udf1;
        
        private string _udf2;
        
        private string _udf3;
        
        private string _supplierorgref;
        
        private string _vendorItemname;
        
        private string _buyerorgref;
        
        public virtual System.Nullable<System.Guid> Mtmldocitemid {
            get {
                return _mtmldocitemid;
            }
            set {
                _mtmldocitemid = value;
            }
        }
        
        public virtual System.Nullable<System.Guid> Mtmldocid {
            get {
                return _mtmldocid;
            }
            set {
                _mtmldocid = value;
            }
        }
        
        public virtual string ItemDescription {
            get {
                return _itemDescription;
            }
            set {
                _itemDescription = value;
            }
        }
        
        public virtual string Partnumber {
            get {
                return _partnumber;
            }
            set {
                _partnumber = value;
            }
        }
        
        public virtual string Partunit {
            get {
                return _partunit;
            }
            set {
                _partunit = value;
            }
        }
        
        public virtual System.Nullable<int> Lineitemnumber {
            get {
                return _lineitemnumber;
            }
            set {
                _lineitemnumber = value;
            }
        }
        
        public virtual string Priority {
            get {
                return _priority;
            }
            set {
                _priority = value;
            }
        }
        
        public virtual string Quantity {
            get {
                return _quantity;
            }
            set {
                _quantity = value;
            }
        }
        
        public virtual string Partcode {
            get {
                return _partcode;
            }
            set {
                _partcode = value;
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
        
        public virtual System.Nullable<float> Unitprice {
            get {
                return _unitprice;
            }
            set {
                _unitprice = value;
            }
        }
        
        public virtual System.Nullable<float> Listprice {
            get {
                return _listprice;
            }
            set {
                _listprice = value;
            }
        }
        
        public virtual System.Nullable<float> Discountedamount {
            get {
                return _discountedamount;
            }
            set {
                _discountedamount = value;
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
        
        public virtual string SupplierRefno {
            get {
                return _supplierRefno;
            }
            set {
                _supplierRefno = value;
            }
        }
        
        public virtual string SupplierRemarks {
            get {
                return _supplierRemarks;
            }
            set {
                _supplierRemarks = value;
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
        
        public virtual System.Nullable<int> Autoid {
            get {
                return _autoid;
            }
            set {
                _autoid = value;
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
        
        public virtual string Itemtype {
            get {
                return _itemtype;
            }
            set {
                _itemtype = value;
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
        
        private void Clean() {
            this.Mtmldocitemid = null;
            this.Mtmldocid = null;
            this.ItemDescription = string.Empty;
            this.Partnumber = string.Empty;
            this.Partunit = string.Empty;
            this.Lineitemnumber = null;
            this.Priority = string.Empty;
            this.Quantity = string.Empty;
            this.Partcode = string.Empty;
            this.Comments = string.Empty;
            this.Unitprice = null;
            this.Listprice = null;
            this.Discountedamount = null;
            this.Originatingsystemref = string.Empty;
            this.SysItemno = null;
            this.SupplierRefno = string.Empty;
            this.SupplierRemarks = string.Empty;
            this.Deliverytime = null;
            this.Autoid = null;
            this.VendorItemno = string.Empty;
            this.Itemtype = string.Empty;
            this.Udf1 = string.Empty;
            this.Udf2 = string.Empty;
            this.Udf3 = string.Empty;
            this.Supplierorgref = string.Empty;
            this.VendorItemname = string.Empty;
            this.Buyerorgref = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["MTMLDOCITEMID"] != System.DBNull.Value)) {
                this.Mtmldocitemid = ((System.Nullable<System.Guid>)(dr["MTMLDOCITEMID"]));
            }
            if ((dr["MTMLDOCID"] != System.DBNull.Value)) {
                this.Mtmldocid = ((System.Nullable<System.Guid>)(dr["MTMLDOCID"]));
            }
            if ((dr["ITEM_DESCRIPTION"] != System.DBNull.Value)) {
                this.ItemDescription = ((string)(dr["ITEM_DESCRIPTION"]));
            }
            if ((dr["PARTNUMBER"] != System.DBNull.Value)) {
                this.Partnumber = ((string)(dr["PARTNUMBER"]));
            }
            if ((dr["PARTUNIT"] != System.DBNull.Value)) {
                this.Partunit = ((string)(dr["PARTUNIT"]));
            }
            if ((dr["LINEITEMNUMBER"] != System.DBNull.Value)) {
                this.Lineitemnumber = ((System.Nullable<int>)(dr["LINEITEMNUMBER"]));
            }
            if ((dr["PRIORITY"] != System.DBNull.Value)) {
                this.Priority = ((string)(dr["PRIORITY"]));
            }
            if ((dr["QUANTITY"] != System.DBNull.Value)) {
                this.Quantity = ((string)(dr["QUANTITY"]));
            }
            if ((dr["PARTCODE"] != System.DBNull.Value)) {
                this.Partcode = ((string)(dr["PARTCODE"]));
            }
            if ((dr["COMMENTS"] != System.DBNull.Value)) {
                this.Comments = ((string)(dr["COMMENTS"]));
            }
            if ((dr["UNITPRICE"] != System.DBNull.Value)) {
                this.Unitprice = ((System.Nullable<float>)(dr["UNITPRICE"]));
            }
            if ((dr["LISTPRICE"] != System.DBNull.Value)) {
                this.Listprice = ((System.Nullable<float>)(dr["LISTPRICE"]));
            }
            if ((dr["DISCOUNTEDAMOUNT"] != System.DBNull.Value)) {
                this.Discountedamount = ((System.Nullable<float>)(dr["DISCOUNTEDAMOUNT"]));
            }
            if ((dr["ORIGINATINGSYSTEMREF"] != System.DBNull.Value)) {
                this.Originatingsystemref = ((string)(dr["ORIGINATINGSYSTEMREF"]));
            }
            if ((dr["SYS_ITEMNO"] != System.DBNull.Value)) {
                this.SysItemno = ((System.Nullable<int>)(dr["SYS_ITEMNO"]));
            }
            if ((dr["SUPPLIER_REFNO"] != System.DBNull.Value)) {
                this.SupplierRefno = ((string)(dr["SUPPLIER_REFNO"]));
            }
            if ((dr["SUPPLIER_REMARKS"] != System.DBNull.Value)) {
                this.SupplierRemarks = ((string)(dr["SUPPLIER_REMARKS"]));
            }
            if ((dr["DeliveryTime"] != System.DBNull.Value)) {
                this.Deliverytime = ((System.Nullable<int>)(dr["DeliveryTime"]));
            }
            if ((dr["AUTOID"] != System.DBNull.Value)) {
                this.Autoid = ((System.Nullable<int>)(dr["AUTOID"]));
            }
            if ((dr["VENDOR_ITEMNO"] != System.DBNull.Value)) {
                this.VendorItemno = ((string)(dr["VENDOR_ITEMNO"]));
            }
            if ((dr["ItemType"] != System.DBNull.Value)) {
                this.Itemtype = ((string)(dr["ItemType"]));
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
        
        public static MtmlDocItemCollection GetAll() {
            MetroLesMonitor.Dal.MtmlDocItem dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocItem();
                System.Data.DataSet ds = dbo.MTML_DOC_ITEM_Select_All();
                MtmlDocItemCollection collection = new MtmlDocItemCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        MtmlDocItem obj = new MtmlDocItem();
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
        
        public static MtmlDocItem Load(System.Nullable<System.Guid> MTMLDOCITEMID) {
            MetroLesMonitor.Dal.MtmlDocItem dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocItem();
                System.Data.DataSet ds = dbo.MTML_DOC_ITEM_Select_One(MTMLDOCITEMID);
                MtmlDocItem obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new MtmlDocItem();
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
            MetroLesMonitor.Dal.MtmlDocItem dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocItem();
                System.Data.DataSet ds = dbo.MTML_DOC_ITEM_Select_One(this.Mtmldocitemid);
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
            MetroLesMonitor.Dal.MtmlDocItem dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocItem();
                dbo.MTML_DOC_ITEM_Insert(this.Mtmldocid, this.ItemDescription, this.Partnumber, this.Partunit, this.Lineitemnumber, this.Priority, this.Quantity, this.Partcode, this.Comments, this.Unitprice, this.Listprice, this.Discountedamount, this.Originatingsystemref, this.SysItemno, this.SupplierRefno, this.SupplierRemarks, this.Deliverytime, this.VendorItemno, this.Itemtype, this.Udf1, this.Udf2, this.Udf3, this.Supplierorgref, this.VendorItemname, this.Buyerorgref);
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
            MetroLesMonitor.Dal.MtmlDocItem dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocItem();
                dbo.MTML_DOC_ITEM_Delete(this.Mtmldocitemid);
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
            MetroLesMonitor.Dal.MtmlDocItem dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocItem();
                dbo.MTML_DOC_ITEM_Update(this.Mtmldocitemid, this.Mtmldocid, this.ItemDescription, this.Partnumber, this.Partunit, this.Lineitemnumber, this.Priority, this.Quantity, this.Partcode, this.Comments, this.Unitprice, this.Listprice, this.Discountedamount, this.Originatingsystemref, this.SysItemno, this.SupplierRefno, this.SupplierRemarks, this.Deliverytime, this.Autoid, this.VendorItemno, this.Itemtype, this.Udf1, this.Udf2, this.Udf3, this.Supplierorgref, this.VendorItemname, this.Buyerorgref);
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
