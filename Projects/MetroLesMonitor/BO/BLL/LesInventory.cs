namespace MetroLesMonitor.Bll {
    
    
    public partial class LesInventory {
        
        private System.Nullable<int> _inventoryid;
        
        private string _invname;
        
        private string _makerref;
        
        private string _drawingno;
        
        private string _posno;
        
        private System.Nullable<int> _partunitid;
        
        private System.Nullable<int> _makerAddressid;
        
        private System.Nullable<float> _minlvl;
        
        private System.Nullable<float> _maxlvl;
        
        private System.Nullable<float> _availStock;
        
        private System.Nullable<int> _stockable;
        
        private string _remark;
        
        private System.Nullable<int> _parttypeid;
        
        private System.Nullable<System.DateTime> _partPriceDate;
        
        private System.Nullable<int> _lastsuppllier;
        
        private System.Nullable<float> _avgPrice;
        
        private string _partsText1;
        
        private string _partsText2;
        
        private string _partsText3;
        
        private string _partsText4;
        
        private System.Nullable<int> _pckUnitid;
        
        private System.Nullable<float> _packQty;
        
        private string _inventoryno;
        
        private System.Nullable<float> _reservedStock;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private System.Nullable<int> _updatedBy;
        
        private System.Nullable<int> _createdBy;
        
        private System.Nullable<int> _preferredSupplierid;
        
        private System.Nullable<int> _defaultSupplierid;
        
        private System.Nullable<int> _ownerid;
        
        private LesParttype _lesParttype;
        
        private SmAddress _smAddress;
        
        private SmAddress _smAddress2;
        
        private SmPartunit _smPartunit;
        
        private SmPartunit _smPartunit2;
        
        private LesInventorySupplierLinkCollection _lesInventorySupplierLinkCollection;
        
        private LesInventorylocationCollection _lesInventorylocationCollection;
        
        public virtual System.Nullable<int> Inventoryid {
            get {
                return _inventoryid;
            }
            set {
                _inventoryid = value;
            }
        }
        
        public virtual string Invname {
            get {
                return _invname;
            }
            set {
                _invname = value;
            }
        }
        
        public virtual string Makerref {
            get {
                return _makerref;
            }
            set {
                _makerref = value;
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
        
        public virtual System.Nullable<float> Minlvl {
            get {
                return _minlvl;
            }
            set {
                _minlvl = value;
            }
        }
        
        public virtual System.Nullable<float> Maxlvl {
            get {
                return _maxlvl;
            }
            set {
                _maxlvl = value;
            }
        }
        
        public virtual System.Nullable<float> AvailStock {
            get {
                return _availStock;
            }
            set {
                _availStock = value;
            }
        }
        
        public virtual System.Nullable<int> Stockable {
            get {
                return _stockable;
            }
            set {
                _stockable = value;
            }
        }
        
        public virtual string Remark {
            get {
                return _remark;
            }
            set {
                _remark = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> PartPriceDate {
            get {
                return _partPriceDate;
            }
            set {
                _partPriceDate = value;
            }
        }
        
        public virtual System.Nullable<float> AvgPrice {
            get {
                return _avgPrice;
            }
            set {
                _avgPrice = value;
            }
        }
        
        public virtual string PartsText1 {
            get {
                return _partsText1;
            }
            set {
                _partsText1 = value;
            }
        }
        
        public virtual string PartsText2 {
            get {
                return _partsText2;
            }
            set {
                _partsText2 = value;
            }
        }
        
        public virtual string PartsText3 {
            get {
                return _partsText3;
            }
            set {
                _partsText3 = value;
            }
        }
        
        public virtual string PartsText4 {
            get {
                return _partsText4;
            }
            set {
                _partsText4 = value;
            }
        }
        
        public virtual System.Nullable<float> PackQty {
            get {
                return _packQty;
            }
            set {
                _packQty = value;
            }
        }
        
        public virtual string Inventoryno {
            get {
                return _inventoryno;
            }
            set {
                _inventoryno = value;
            }
        }
        
        public virtual System.Nullable<float> ReservedStock {
            get {
                return _reservedStock;
            }
            set {
                _reservedStock = value;
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
        
        public virtual System.Nullable<int> PreferredSupplierid {
            get {
                return _preferredSupplierid;
            }
            set {
                _preferredSupplierid = value;
            }
        }
        
        public virtual System.Nullable<int> DefaultSupplierid {
            get {
                return _defaultSupplierid;
            }
            set {
                _defaultSupplierid = value;
            }
        }
        
        public virtual System.Nullable<int> Ownerid {
            get {
                return _ownerid;
            }
            set {
                _ownerid = value;
            }
        }
        
        public virtual LesParttype LesParttype {
            get {
                if ((this._lesParttype == null)) {
                    this._lesParttype = MetroLesMonitor.Bll.LesParttype.Load(this._parttypeid);
                }
                return this._lesParttype;
            }
            set {
                _lesParttype = value;
            }
        }
        
        public virtual SmAddress SmAddress {
            get {
                if ((this._smAddress == null)) {
                    this._smAddress = MetroLesMonitor.Bll.SmAddress.Load(this._lastsuppllier);
                }
                return this._smAddress;
            }
            set {
                _smAddress = value;
            }
        }
        
        public virtual SmAddress SmAddress2 {
            get {
                if ((this._smAddress2 == null)) {
                    this._smAddress2 = MetroLesMonitor.Bll.SmAddress.Load(this._makerAddressid);
                }
                return this._smAddress2;
            }
            set {
                _smAddress2 = value;
            }
        }
        
        public virtual SmPartunit SmPartunit {
            get {
                if ((this._smPartunit == null)) {
                    this._smPartunit = MetroLesMonitor.Bll.SmPartunit.Load(this._partunitid);
                }
                return this._smPartunit;
            }
            set {
                _smPartunit = value;
            }
        }
        
        public virtual SmPartunit SmPartunit2 {
            get {
                if ((this._smPartunit2 == null)) {
                    this._smPartunit2 = MetroLesMonitor.Bll.SmPartunit.Load(this._pckUnitid);
                }
                return this._smPartunit2;
            }
            set {
                _smPartunit2 = value;
            }
        }
        
        public virtual LesInventorySupplierLinkCollection LesInventorySupplierLinkCollection {
            get {
                if ((this._lesInventorySupplierLinkCollection == null)) {
                    _lesInventorySupplierLinkCollection = MetroLesMonitor.Bll.LesInventorySupplierLink.Select_LES_INVENTORY_SUPPLIER_LINKs_By_INVENTORYID(this.Inventoryid);
                }
                return this._lesInventorySupplierLinkCollection;
            }
        }
        
        public virtual LesInventorylocationCollection LesInventorylocationCollection {
            get {
                if ((this._lesInventorylocationCollection == null)) {
                    _lesInventorylocationCollection = MetroLesMonitor.Bll.LesInventorylocation.Select_LES_INVENTORYLOCATIONs_By_INVENTORYID(this.Inventoryid);
                }
                return this._lesInventorylocationCollection;
            }
        }
        
        private void Clean() {
            this.Inventoryid = null;
            this.Invname = string.Empty;
            this.Makerref = string.Empty;
            this.Drawingno = string.Empty;
            this.Posno = string.Empty;
            this._partunitid = null;
            this._makerAddressid = null;
            this.Minlvl = null;
            this.Maxlvl = null;
            this.AvailStock = null;
            this.Stockable = null;
            this.Remark = string.Empty;
            this._parttypeid = null;
            this.PartPriceDate = null;
            this._lastsuppllier = null;
            this.AvgPrice = null;
            this.PartsText1 = string.Empty;
            this.PartsText2 = string.Empty;
            this.PartsText3 = string.Empty;
            this.PartsText4 = string.Empty;
            this._pckUnitid = null;
            this.PackQty = null;
            this.Inventoryno = string.Empty;
            this.ReservedStock = null;
            this.UpdateDate = null;
            this.CreatedDate = null;
            this.UpdatedBy = null;
            this.CreatedBy = null;
            this.PreferredSupplierid = null;
            this.DefaultSupplierid = null;
            this.Ownerid = null;
            this.LesParttype = null;
            this.SmAddress = null;
            this.SmAddress2 = null;
            this.SmPartunit = null;
            this.SmPartunit2 = null;
            this._lesInventorySupplierLinkCollection = null;
            this._lesInventorylocationCollection = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["INVENTORYID"] != System.DBNull.Value)) {
                this.Inventoryid = ((System.Nullable<int>)(dr["INVENTORYID"]));
            }
            if ((dr["INVNAME"] != System.DBNull.Value)) {
                this.Invname = ((string)(dr["INVNAME"]));
            }
            if ((dr["MAKERREF"] != System.DBNull.Value)) {
                this.Makerref = ((string)(dr["MAKERREF"]));
            }
            if ((dr["DRAWINGNO"] != System.DBNull.Value)) {
                this.Drawingno = ((string)(dr["DRAWINGNO"]));
            }
            if ((dr["POSNO"] != System.DBNull.Value)) {
                this.Posno = ((string)(dr["POSNO"]));
            }
            if ((dr["PARTUNITID"] != System.DBNull.Value)) {
                this._partunitid = ((System.Nullable<int>)(dr["PARTUNITID"]));
            }
            if ((dr["MAKER_ADDRESSID"] != System.DBNull.Value)) {
                this._makerAddressid = ((System.Nullable<int>)(dr["MAKER_ADDRESSID"]));
            }
            if ((dr["MINLVL"] != System.DBNull.Value)) {
                this.Minlvl = ((System.Nullable<float>)(dr["MINLVL"]));
            }
            if ((dr["MAXLVL"] != System.DBNull.Value)) {
                this.Maxlvl = ((System.Nullable<float>)(dr["MAXLVL"]));
            }
            if ((dr["AVAIL_STOCK"] != System.DBNull.Value)) {
                this.AvailStock = ((System.Nullable<float>)(dr["AVAIL_STOCK"]));
            }
            if ((dr["STOCKABLE"] != System.DBNull.Value)) {
                this.Stockable = ((System.Nullable<int>)(dr["STOCKABLE"]));
            }
            if ((dr["REMARK"] != System.DBNull.Value)) {
                this.Remark = ((string)(dr["REMARK"]));
            }
            if ((dr["PARTTYPEID"] != System.DBNull.Value)) {
                this._parttypeid = ((System.Nullable<int>)(dr["PARTTYPEID"]));
            }
            if ((dr["PART_PRICE_DATE"] != System.DBNull.Value)) {
                this.PartPriceDate = ((System.Nullable<System.DateTime>)(dr["PART_PRICE_DATE"]));
            }
            if ((dr["LASTSUPPLLIER"] != System.DBNull.Value)) {
                this._lastsuppllier = ((System.Nullable<int>)(dr["LASTSUPPLLIER"]));
            }
            if ((dr["AVG_PRICE"] != System.DBNull.Value)) {
                this.AvgPrice = ((System.Nullable<float>)(dr["AVG_PRICE"]));
            }
            if ((dr["Parts_Text1"] != System.DBNull.Value)) {
                this.PartsText1 = ((string)(dr["Parts_Text1"]));
            }
            if ((dr["Parts_Text2"] != System.DBNull.Value)) {
                this.PartsText2 = ((string)(dr["Parts_Text2"]));
            }
            if ((dr["Parts_Text3"] != System.DBNull.Value)) {
                this.PartsText3 = ((string)(dr["Parts_Text3"]));
            }
            if ((dr["Parts_Text4"] != System.DBNull.Value)) {
                this.PartsText4 = ((string)(dr["Parts_Text4"]));
            }
            if ((dr["PCK_UNITID"] != System.DBNull.Value)) {
                this._pckUnitid = ((System.Nullable<int>)(dr["PCK_UNITID"]));
            }
            if ((dr["PACK_QTY"] != System.DBNull.Value)) {
                this.PackQty = ((System.Nullable<float>)(dr["PACK_QTY"]));
            }
            if ((dr["INVENTORYNO"] != System.DBNull.Value)) {
                this.Inventoryno = ((string)(dr["INVENTORYNO"]));
            }
            if ((dr["RESERVED_STOCK"] != System.DBNull.Value)) {
                this.ReservedStock = ((System.Nullable<float>)(dr["RESERVED_STOCK"]));
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
            if ((dr["PREFERRED_SUPPLIERID"] != System.DBNull.Value)) {
                this.PreferredSupplierid = ((System.Nullable<int>)(dr["PREFERRED_SUPPLIERID"]));
            }
            if ((dr["DEFAULT_SUPPLIERID"] != System.DBNull.Value)) {
                this.DefaultSupplierid = ((System.Nullable<int>)(dr["DEFAULT_SUPPLIERID"]));
            }
            if ((dr["OWNERID"] != System.DBNull.Value)) {
                this.Ownerid = ((System.Nullable<int>)(dr["OWNERID"]));
            }
        }
        
        public static LesInventoryCollection Select_LES_INVENTORYs_By_PARTTYPEID(System.Nullable<int> PARTTYPEID) {
            MetroLesMonitor.Dal.LesInventory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventory();
                System.Data.DataSet ds = dbo.Select_LES_INVENTORYs_By_PARTTYPEID(PARTTYPEID);
                LesInventoryCollection collection = new LesInventoryCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesInventory obj = new LesInventory();
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
        
        public static LesInventoryCollection Select_LES_INVENTORYs_By_LASTSUPPLLIER(System.Nullable<int> LASTSUPPLLIER) {
            MetroLesMonitor.Dal.LesInventory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventory();
                System.Data.DataSet ds = dbo.Select_LES_INVENTORYs_By_LASTSUPPLLIER(LASTSUPPLLIER);
                LesInventoryCollection collection = new LesInventoryCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesInventory obj = new LesInventory();
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
        
        public static LesInventoryCollection Select_LES_INVENTORYs_By_MAKER_ADDRESSID(System.Nullable<int> MAKER_ADDRESSID) {
            MetroLesMonitor.Dal.LesInventory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventory();
                System.Data.DataSet ds = dbo.Select_LES_INVENTORYs_By_MAKER_ADDRESSID(MAKER_ADDRESSID);
                LesInventoryCollection collection = new LesInventoryCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesInventory obj = new LesInventory();
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
        
        public static LesInventoryCollection Select_LES_INVENTORYs_By_PARTUNITID(System.Nullable<int> PARTUNITID) {
            MetroLesMonitor.Dal.LesInventory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventory();
                System.Data.DataSet ds = dbo.Select_LES_INVENTORYs_By_PARTUNITID(PARTUNITID);
                LesInventoryCollection collection = new LesInventoryCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesInventory obj = new LesInventory();
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
        
        public static LesInventoryCollection Select_LES_INVENTORYs_By_PCK_UNITID(System.Nullable<int> PCK_UNITID) {
            MetroLesMonitor.Dal.LesInventory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventory();
                System.Data.DataSet ds = dbo.Select_LES_INVENTORYs_By_PCK_UNITID(PCK_UNITID);
                LesInventoryCollection collection = new LesInventoryCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesInventory obj = new LesInventory();
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
        
        public static LesInventoryCollection GetAll() {
            MetroLesMonitor.Dal.LesInventory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventory();
                System.Data.DataSet ds = dbo.LES_INVENTORY_Select_All();
                LesInventoryCollection collection = new LesInventoryCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesInventory obj = new LesInventory();
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
        
        public static LesInventory Load(System.Nullable<int> INVENTORYID) {
            MetroLesMonitor.Dal.LesInventory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventory();
                System.Data.DataSet ds = dbo.LES_INVENTORY_Select_One(INVENTORYID);
                LesInventory obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new LesInventory();
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
            MetroLesMonitor.Dal.LesInventory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventory();
                System.Data.DataSet ds = dbo.LES_INVENTORY_Select_One(this.Inventoryid);
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
            MetroLesMonitor.Dal.LesInventory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventory();
                dbo.LES_INVENTORY_Insert(this.Invname, this.Makerref, this.Drawingno, this.Posno, this._partunitid, this._makerAddressid, this.Minlvl, this.Maxlvl, this.AvailStock, this.Stockable, this.Remark, this._parttypeid, this.PartPriceDate, this._lastsuppllier, this.AvgPrice, this.PartsText1, this.PartsText2, this.PartsText3, this.PartsText4, this._pckUnitid, this.PackQty, this.Inventoryno, this.ReservedStock, this.UpdateDate, this.CreatedDate, this.UpdatedBy, this.CreatedBy, this.PreferredSupplierid, this.DefaultSupplierid, this.Ownerid);
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
            MetroLesMonitor.Dal.LesInventory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventory();
                dbo.LES_INVENTORY_Delete(this.Inventoryid);
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
            MetroLesMonitor.Dal.LesInventory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventory();
                dbo.LES_INVENTORY_Update(this.Inventoryid, this.Invname, this.Makerref, this.Drawingno, this.Posno, this._partunitid, this._makerAddressid, this.Minlvl, this.Maxlvl, this.AvailStock, this.Stockable, this.Remark, this._parttypeid, this.PartPriceDate, this._lastsuppllier, this.AvgPrice, this.PartsText1, this.PartsText2, this.PartsText3, this.PartsText4, this._pckUnitid, this.PackQty, this.Inventoryno, this.ReservedStock, this.UpdateDate, this.CreatedDate, this.UpdatedBy, this.CreatedBy, this.PreferredSupplierid, this.DefaultSupplierid, this.Ownerid);
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
