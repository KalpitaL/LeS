namespace MetroLesMonitor.Bll {
    
    
    public partial class MtmlDocItemEquipment {
        
        private System.Nullable<System.Guid> _itemequipid;
        
        private System.Nullable<System.Guid> _mtmlitemid;
        
        private System.Nullable<System.Guid> _mtmldocid;
        
        private string _equipName;
        
        private string _manufacturer;
        
        private string _serialnumber;
        
        private string _modeltype;
        
        private string _drawingnumber;
        
        private string _description;
        
        private System.Nullable<int> _autoid;
        
        public virtual System.Nullable<System.Guid> Itemequipid {
            get {
                return _itemequipid;
            }
            set {
                _itemequipid = value;
            }
        }
        
        public virtual System.Nullable<System.Guid> Mtmlitemid {
            get {
                return _mtmlitemid;
            }
            set {
                _mtmlitemid = value;
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
        
        public virtual string EquipName {
            get {
                return _equipName;
            }
            set {
                _equipName = value;
            }
        }
        
        public virtual string Manufacturer {
            get {
                return _manufacturer;
            }
            set {
                _manufacturer = value;
            }
        }
        
        public virtual string Serialnumber {
            get {
                return _serialnumber;
            }
            set {
                _serialnumber = value;
            }
        }
        
        public virtual string Modeltype {
            get {
                return _modeltype;
            }
            set {
                _modeltype = value;
            }
        }
        
        public virtual string Drawingnumber {
            get {
                return _drawingnumber;
            }
            set {
                _drawingnumber = value;
            }
        }
        
        public virtual string Description {
            get {
                return _description;
            }
            set {
                _description = value;
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
        
        private void Clean() {
            this.Itemequipid = null;
            this.Mtmlitemid = null;
            this.Mtmldocid = null;
            this.EquipName = string.Empty;
            this.Manufacturer = string.Empty;
            this.Serialnumber = string.Empty;
            this.Modeltype = string.Empty;
            this.Drawingnumber = string.Empty;
            this.Description = string.Empty;
            this.Autoid = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["ITEMEQUIPID"] != System.DBNull.Value)) {
                this.Itemequipid = ((System.Nullable<System.Guid>)(dr["ITEMEQUIPID"]));
            }
            if ((dr["MTMLITEMID"] != System.DBNull.Value)) {
                this.Mtmlitemid = ((System.Nullable<System.Guid>)(dr["MTMLITEMID"]));
            }
            if ((dr["MTMLDOCID"] != System.DBNull.Value)) {
                this.Mtmldocid = ((System.Nullable<System.Guid>)(dr["MTMLDOCID"]));
            }
            if ((dr["EQUIP_NAME"] != System.DBNull.Value)) {
                this.EquipName = ((string)(dr["EQUIP_NAME"]));
            }
            if ((dr["MANUFACTURER"] != System.DBNull.Value)) {
                this.Manufacturer = ((string)(dr["MANUFACTURER"]));
            }
            if ((dr["SERIALNUMBER"] != System.DBNull.Value)) {
                this.Serialnumber = ((string)(dr["SERIALNUMBER"]));
            }
            if ((dr["MODELTYPE"] != System.DBNull.Value)) {
                this.Modeltype = ((string)(dr["MODELTYPE"]));
            }
            if ((dr["DRAWINGNUMBER"] != System.DBNull.Value)) {
                this.Drawingnumber = ((string)(dr["DRAWINGNUMBER"]));
            }
            if ((dr["DESCRIPTION"] != System.DBNull.Value)) {
                this.Description = ((string)(dr["DESCRIPTION"]));
            }
            if ((dr["AUTOID"] != System.DBNull.Value)) {
                this.Autoid = ((System.Nullable<int>)(dr["AUTOID"]));
            }
        }
        
        public static MtmlDocItemEquipmentCollection GetAll() {
            MetroLesMonitor.Dal.MtmlDocItemEquipment dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocItemEquipment();
                System.Data.DataSet ds = dbo.MTML_DOC_ITEM_EQUIPMENT_Select_All();
                MtmlDocItemEquipmentCollection collection = new MtmlDocItemEquipmentCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        MtmlDocItemEquipment obj = new MtmlDocItemEquipment();
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
        
        public static MtmlDocItemEquipment Load(System.Nullable<System.Guid> ITEMEQUIPID) {
            MetroLesMonitor.Dal.MtmlDocItemEquipment dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocItemEquipment();
                System.Data.DataSet ds = dbo.MTML_DOC_ITEM_EQUIPMENT_Select_One(ITEMEQUIPID);
                MtmlDocItemEquipment obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new MtmlDocItemEquipment();
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
            MetroLesMonitor.Dal.MtmlDocItemEquipment dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocItemEquipment();
                System.Data.DataSet ds = dbo.MTML_DOC_ITEM_EQUIPMENT_Select_One(this.Itemequipid);
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
            MetroLesMonitor.Dal.MtmlDocItemEquipment dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocItemEquipment();
                dbo.MTML_DOC_ITEM_EQUIPMENT_Insert(this.Mtmlitemid, this.Mtmldocid, this.EquipName, this.Manufacturer, this.Serialnumber, this.Modeltype, this.Drawingnumber, this.Description);
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
            MetroLesMonitor.Dal.MtmlDocItemEquipment dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocItemEquipment();
                dbo.MTML_DOC_ITEM_EQUIPMENT_Delete(this.Itemequipid);
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
            MetroLesMonitor.Dal.MtmlDocItemEquipment dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocItemEquipment();
                dbo.MTML_DOC_ITEM_EQUIPMENT_Update(this.Itemequipid, this.Mtmlitemid, this.Mtmldocid, this.EquipName, this.Manufacturer, this.Serialnumber, this.Modeltype, this.Drawingnumber, this.Description, this.Autoid);
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
