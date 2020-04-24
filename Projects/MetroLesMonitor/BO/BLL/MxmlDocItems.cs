namespace MetroLesMonitor.Bll {
    
    
    public partial class MxmlDocItems {
        
        private System.Nullable<System.Guid> _mxmlitemid;
        
        private System.Nullable<System.Guid> _mxmldocid;
        
        private System.Nullable<int> _linenumber;
        
        private string _originatingsystemref;
        
        private string _supplierpartid;
        
        private string _itemDescription;
        
        private System.Nullable<float> _unitPrice;
        
        private string _itemUnit;
        
        private string _itemComment;
        
        private string _equipmentDescription;
        
        private string _equipDrawingno;
        
        private string _equipManufacturer;
        
        private string _equipModel;
        
        private string _equipSerialno;
        
        private string _equipName;
        
        private System.Nullable<float> _quantity;
        
        private System.Nullable<float> _listPrice;
        
        private System.Nullable<float> _itemDiscount;
        
        private MxmlTransactionHeader _mxmlTransactionHeader;
        
        public virtual System.Nullable<System.Guid> Mxmlitemid {
            get {
                return _mxmlitemid;
            }
            set {
                _mxmlitemid = value;
            }
        }
        
        public virtual System.Nullable<int> Linenumber {
            get {
                return _linenumber;
            }
            set {
                _linenumber = value;
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
        
        public virtual string Supplierpartid {
            get {
                return _supplierpartid;
            }
            set {
                _supplierpartid = value;
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
        
        public virtual System.Nullable<float> UnitPrice {
            get {
                return _unitPrice;
            }
            set {
                _unitPrice = value;
            }
        }
        
        public virtual string ItemUnit {
            get {
                return _itemUnit;
            }
            set {
                _itemUnit = value;
            }
        }
        
        public virtual string ItemComment {
            get {
                return _itemComment;
            }
            set {
                _itemComment = value;
            }
        }
        
        public virtual string EquipmentDescription {
            get {
                return _equipmentDescription;
            }
            set {
                _equipmentDescription = value;
            }
        }
        
        public virtual string EquipDrawingno {
            get {
                return _equipDrawingno;
            }
            set {
                _equipDrawingno = value;
            }
        }
        
        public virtual string EquipManufacturer {
            get {
                return _equipManufacturer;
            }
            set {
                _equipManufacturer = value;
            }
        }
        
        public virtual string EquipModel {
            get {
                return _equipModel;
            }
            set {
                _equipModel = value;
            }
        }
        
        public virtual string EquipSerialno {
            get {
                return _equipSerialno;
            }
            set {
                _equipSerialno = value;
            }
        }
        
        public virtual string EquipName {
            get {
                return _equipName;
            }
            set {
                _equipName = value;
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
        
        public virtual System.Nullable<float> ListPrice {
            get {
                return _listPrice;
            }
            set {
                _listPrice = value;
            }
        }
        
        public virtual System.Nullable<float> ItemDiscount {
            get {
                return _itemDiscount;
            }
            set {
                _itemDiscount = value;
            }
        }
        
        public virtual MxmlTransactionHeader MxmlTransactionHeader {
            get {
                if ((this._mxmlTransactionHeader == null)) {
                    this._mxmlTransactionHeader = MetroLesMonitor.Bll.MxmlTransactionHeader.Load(this._mxmldocid);
                }
                return this._mxmlTransactionHeader;
            }
            set {
                _mxmlTransactionHeader = value;
            }
        }
        
        private void Clean() {
            this.Mxmlitemid = null;
            this._mxmldocid = null;
            this.Linenumber = null;
            this.Originatingsystemref = string.Empty;
            this.Supplierpartid = string.Empty;
            this.ItemDescription = string.Empty;
            this.UnitPrice = null;
            this.ItemUnit = string.Empty;
            this.ItemComment = string.Empty;
            this.EquipmentDescription = string.Empty;
            this.EquipDrawingno = string.Empty;
            this.EquipManufacturer = string.Empty;
            this.EquipModel = string.Empty;
            this.EquipSerialno = string.Empty;
            this.EquipName = string.Empty;
            this.Quantity = null;
            this.ListPrice = null;
            this.ItemDiscount = null;
            this.MxmlTransactionHeader = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["MXMLITEMID"] != System.DBNull.Value)) {
                this.Mxmlitemid = ((System.Nullable<System.Guid>)(dr["MXMLITEMID"]));
            }
            if ((dr["MXMLDOCID"] != System.DBNull.Value)) {
                this._mxmldocid = ((System.Nullable<System.Guid>)(dr["MXMLDOCID"]));
            }
            if ((dr["LINENUMBER"] != System.DBNull.Value)) {
                this.Linenumber = ((System.Nullable<int>)(dr["LINENUMBER"]));
            }
            if ((dr["ORIGINATINGSYSTEMREF"] != System.DBNull.Value)) {
                this.Originatingsystemref = ((string)(dr["ORIGINATINGSYSTEMREF"]));
            }
            if ((dr["SUPPLIERPARTID"] != System.DBNull.Value)) {
                this.Supplierpartid = ((string)(dr["SUPPLIERPARTID"]));
            }
            if ((dr["ITEM_DESCRIPTION"] != System.DBNull.Value)) {
                this.ItemDescription = ((string)(dr["ITEM_DESCRIPTION"]));
            }
            if ((dr["UNIT_PRICE"] != System.DBNull.Value)) {
                this.UnitPrice = ((System.Nullable<float>)(dr["UNIT_PRICE"]));
            }
            if ((dr["ITEM_UNIT"] != System.DBNull.Value)) {
                this.ItemUnit = ((string)(dr["ITEM_UNIT"]));
            }
            if ((dr["ITEM_COMMENT"] != System.DBNull.Value)) {
                this.ItemComment = ((string)(dr["ITEM_COMMENT"]));
            }
            if ((dr["EQUIPMENT_DESCRIPTION"] != System.DBNull.Value)) {
                this.EquipmentDescription = ((string)(dr["EQUIPMENT_DESCRIPTION"]));
            }
            if ((dr["EQUIP_DRAWINGNO"] != System.DBNull.Value)) {
                this.EquipDrawingno = ((string)(dr["EQUIP_DRAWINGNO"]));
            }
            if ((dr["EQUIP_MANUFACTURER"] != System.DBNull.Value)) {
                this.EquipManufacturer = ((string)(dr["EQUIP_MANUFACTURER"]));
            }
            if ((dr["EQUIP_MODEL"] != System.DBNull.Value)) {
                this.EquipModel = ((string)(dr["EQUIP_MODEL"]));
            }
            if ((dr["EQUIP_SERIALNO"] != System.DBNull.Value)) {
                this.EquipSerialno = ((string)(dr["EQUIP_SERIALNO"]));
            }
            if ((dr["EQUIP_NAME"] != System.DBNull.Value)) {
                this.EquipName = ((string)(dr["EQUIP_NAME"]));
            }
            if ((dr["QUANTITY"] != System.DBNull.Value)) {
                this.Quantity = ((System.Nullable<float>)(dr["QUANTITY"]));
            }
            if ((dr["LIST_PRICE"] != System.DBNull.Value)) {
                this.ListPrice = ((System.Nullable<float>)(dr["LIST_PRICE"]));
            }
            if ((dr["ITEM_DISCOUNT"] != System.DBNull.Value)) {
                this.ItemDiscount = ((System.Nullable<float>)(dr["ITEM_DISCOUNT"]));
            }
        }
        
        public static MxmlDocItemsCollection Select_MXML_DOC_ITEMSs_By_MXMLDOCID(System.Nullable<System.Guid> MXMLDOCID) {
            MetroLesMonitor.Dal.MxmlDocItems dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlDocItems();
                System.Data.DataSet ds = dbo.Select_MXML_DOC_ITEMSs_By_MXMLDOCID(MXMLDOCID);
                MxmlDocItemsCollection collection = new MxmlDocItemsCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        MxmlDocItems obj = new MxmlDocItems();
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
        
        public static MxmlDocItemsCollection GetAll() {
            MetroLesMonitor.Dal.MxmlDocItems dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlDocItems();
                System.Data.DataSet ds = dbo.MXML_DOC_ITEMS_Select_All();
                MxmlDocItemsCollection collection = new MxmlDocItemsCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        MxmlDocItems obj = new MxmlDocItems();
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
        
        public static MxmlDocItems Load(System.Nullable<System.Guid> MXMLITEMID) {
            MetroLesMonitor.Dal.MxmlDocItems dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlDocItems();
                System.Data.DataSet ds = dbo.MXML_DOC_ITEMS_Select_One(MXMLITEMID);
                MxmlDocItems obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new MxmlDocItems();
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
            MetroLesMonitor.Dal.MxmlDocItems dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlDocItems();
                System.Data.DataSet ds = dbo.MXML_DOC_ITEMS_Select_One(this.Mxmlitemid);
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
            MetroLesMonitor.Dal.MxmlDocItems dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlDocItems();
                dbo.MXML_DOC_ITEMS_Insert(this._mxmldocid, this.Linenumber, this.Originatingsystemref, this.Supplierpartid, this.ItemDescription, this.UnitPrice, this.ItemUnit, this.ItemComment, this.EquipmentDescription, this.EquipDrawingno, this.EquipManufacturer, this.EquipModel, this.EquipSerialno, this.EquipName, this.Quantity, this.ListPrice, this.ItemDiscount);
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
            MetroLesMonitor.Dal.MxmlDocItems dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlDocItems();
                dbo.MXML_DOC_ITEMS_Delete(this.Mxmlitemid);
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
            MetroLesMonitor.Dal.MxmlDocItems dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlDocItems();
                dbo.MXML_DOC_ITEMS_Update(this.Mxmlitemid, this._mxmldocid, this.Linenumber, this.Originatingsystemref, this.Supplierpartid, this.ItemDescription, this.UnitPrice, this.ItemUnit, this.ItemComment, this.EquipmentDescription, this.EquipDrawingno, this.EquipManufacturer, this.EquipModel, this.EquipSerialno, this.EquipName, this.Quantity, this.ListPrice, this.ItemDiscount);
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
