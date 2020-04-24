namespace MetroLesMonitor.Bll {
    
    
    public partial class LesInventoryAudit {
        
        private System.Nullable<int> _invauditid;
        
        private System.Nullable<int> _inventoryid;
        
        private System.Nullable<int> _userid;
        
        private System.Nullable<System.DateTime> _auditTime;
        
        private string _remark;
        
        public virtual System.Nullable<int> Invauditid {
            get {
                return _invauditid;
            }
            set {
                _invauditid = value;
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
        
        public virtual System.Nullable<int> Userid {
            get {
                return _userid;
            }
            set {
                _userid = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> AuditTime {
            get {
                return _auditTime;
            }
            set {
                _auditTime = value;
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
        
        private void Clean() {
            this.Invauditid = null;
            this.Inventoryid = null;
            this.Userid = null;
            this.AuditTime = null;
            this.Remark = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["INVAUDITID"] != System.DBNull.Value)) {
                this.Invauditid = ((System.Nullable<int>)(dr["INVAUDITID"]));
            }
            if ((dr["INVENTORYID"] != System.DBNull.Value)) {
                this.Inventoryid = ((System.Nullable<int>)(dr["INVENTORYID"]));
            }
            if ((dr["USERID"] != System.DBNull.Value)) {
                this.Userid = ((System.Nullable<int>)(dr["USERID"]));
            }
            if ((dr["AUDIT_TIME"] != System.DBNull.Value)) {
                this.AuditTime = ((System.Nullable<System.DateTime>)(dr["AUDIT_TIME"]));
            }
            if ((dr["REMARK"] != System.DBNull.Value)) {
                this.Remark = ((string)(dr["REMARK"]));
            }
        }
        
        public static LesInventoryAuditCollection GetAll() {
            MetroLesMonitor.Dal.LesInventoryAudit dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventoryAudit();
                System.Data.DataSet ds = dbo.LES_INVENTORY_AUDIT_Select_All();
                LesInventoryAuditCollection collection = new LesInventoryAuditCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesInventoryAudit obj = new LesInventoryAudit();
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
        
        public static LesInventoryAudit Load(System.Nullable<int> INVAUDITID) {
            MetroLesMonitor.Dal.LesInventoryAudit dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventoryAudit();
                System.Data.DataSet ds = dbo.LES_INVENTORY_AUDIT_Select_One(INVAUDITID);
                LesInventoryAudit obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new LesInventoryAudit();
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
            MetroLesMonitor.Dal.LesInventoryAudit dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventoryAudit();
                System.Data.DataSet ds = dbo.LES_INVENTORY_AUDIT_Select_One(this.Invauditid);
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
            MetroLesMonitor.Dal.LesInventoryAudit dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventoryAudit();
                dbo.LES_INVENTORY_AUDIT_Insert(this.Inventoryid, this.Userid, this.AuditTime, this.Remark);
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
            MetroLesMonitor.Dal.LesInventoryAudit dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventoryAudit();
                dbo.LES_INVENTORY_AUDIT_Delete(this.Invauditid);
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
            MetroLesMonitor.Dal.LesInventoryAudit dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesInventoryAudit();
                dbo.LES_INVENTORY_AUDIT_Update(this.Invauditid, this.Inventoryid, this.Userid, this.AuditTime, this.Remark);
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
