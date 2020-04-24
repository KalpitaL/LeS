namespace MetroLesMonitor.Bll {
    
    
    public partial class SmPartunit {
        
        private System.Nullable<int> _partunitid;
        
        private string _unitCode;
        
        private string _unitDescr;
        
        private System.Nullable<byte> _exported;
        
        private System.Nullable<int> _updateSite;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private System.Nullable<int> _siteid;
        
        private LesInventoryCollection _lesInventoryCollection;
        
        private LesInventoryCollection _lesInventoryCollection2;
        
        public virtual System.Nullable<int> Partunitid {
            get {
                return _partunitid;
            }
            set {
                _partunitid = value;
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
        
        public virtual string UnitDescr {
            get {
                return _unitDescr;
            }
            set {
                _unitDescr = value;
            }
        }
        
        public virtual System.Nullable<byte> Exported {
            get {
                return _exported;
            }
            set {
                _exported = value;
            }
        }
        
        public virtual System.Nullable<int> UpdateSite {
            get {
                return _updateSite;
            }
            set {
                _updateSite = value;
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
        
        public virtual System.Nullable<int> Siteid {
            get {
                return _siteid;
            }
            set {
                _siteid = value;
            }
        }
        
        public virtual LesInventoryCollection LesInventoryCollection {
            get {
                if ((this._lesInventoryCollection == null)) {
                    _lesInventoryCollection = MetroLesMonitor.Bll.LesInventory.Select_LES_INVENTORYs_By_PARTUNITID(this.Partunitid);
                }
                return this._lesInventoryCollection;
            }
        }
        
        public virtual LesInventoryCollection LesInventoryCollection2 {
            get {
                if ((this._lesInventoryCollection2 == null)) {
                    _lesInventoryCollection2 = MetroLesMonitor.Bll.LesInventory.Select_LES_INVENTORYs_By_PCK_UNITID(this.Partunitid);
                }
                return this._lesInventoryCollection2;
            }
        }
        
        private void Clean() {
            this.Partunitid = null;
            this.UnitCode = string.Empty;
            this.UnitDescr = string.Empty;
            this.Exported = null;
            this.UpdateSite = null;
            this.UpdateDate = null;
            this.CreatedDate = null;
            this.Siteid = null;
            this._lesInventoryCollection = null;
            this._lesInventoryCollection2 = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["PARTUNITID"] != System.DBNull.Value)) {
                this.Partunitid = ((System.Nullable<int>)(dr["PARTUNITID"]));
            }
            if ((dr["UNIT_CODE"] != System.DBNull.Value)) {
                this.UnitCode = ((string)(dr["UNIT_CODE"]));
            }
            if ((dr["UNIT_DESCR"] != System.DBNull.Value)) {
                this.UnitDescr = ((string)(dr["UNIT_DESCR"]));
            }
            if ((dr["EXPORTED"] != System.DBNull.Value)) {
                this.Exported = ((System.Nullable<byte>)(dr["EXPORTED"]));
            }
            if ((dr["UPDATE_SITE"] != System.DBNull.Value)) {
                this.UpdateSite = ((System.Nullable<int>)(dr["UPDATE_SITE"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value)) {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value)) {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["SITEID"] != System.DBNull.Value)) {
                this.Siteid = ((System.Nullable<int>)(dr["SITEID"]));
            }
        }
        
        public static SmPartunitCollection GetAll() {
            MetroLesMonitor.Dal.SmPartunit dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmPartunit();
                System.Data.DataSet ds = dbo.SM_PARTUNIT_Select_All();
                SmPartunitCollection collection = new SmPartunitCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmPartunit obj = new SmPartunit();
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
        
        public static SmPartunit Load(System.Nullable<int> PARTUNITID) {
            MetroLesMonitor.Dal.SmPartunit dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmPartunit();
                System.Data.DataSet ds = dbo.SM_PARTUNIT_Select_One(PARTUNITID);
                SmPartunit obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmPartunit();
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
            MetroLesMonitor.Dal.SmPartunit dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmPartunit();
                System.Data.DataSet ds = dbo.SM_PARTUNIT_Select_One(this.Partunitid);
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
            MetroLesMonitor.Dal.SmPartunit dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmPartunit();
                dbo.SM_PARTUNIT_Insert(this.Partunitid, this.UnitCode, this.UnitDescr, this.Exported, this.UpdateSite, this.UpdateDate, this.CreatedDate, this.Siteid);
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
            MetroLesMonitor.Dal.SmPartunit dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmPartunit();
                dbo.SM_PARTUNIT_Delete(this.Partunitid);
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
            MetroLesMonitor.Dal.SmPartunit dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmPartunit();
                dbo.SM_PARTUNIT_Update(this.Partunitid, this.UnitCode, this.UnitDescr, this.Exported, this.UpdateSite, this.UpdateDate, this.CreatedDate, this.Siteid);
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
