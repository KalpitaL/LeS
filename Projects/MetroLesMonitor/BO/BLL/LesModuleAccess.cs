namespace MetroLesMonitor.Bll {
    
    
    public partial class LesModuleAccess {
        
        private System.Nullable<int> _moduleaccessid;
        
        private System.Nullable<int> _moduleactionid;
        
        private System.Nullable<int> _extUserid;
        
        private System.Nullable<int> _accessLevel;
        
        private System.Nullable<int> _exported;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private System.Nullable<int> _createdBy;
        
        private System.Nullable<int> _updateSite;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        private System.Nullable<int> _updatedBy;
        
        private LesModuleActions _lesModuleActions;
        
        public virtual System.Nullable<int> Moduleaccessid {
            get {
                return _moduleaccessid;
            }
            set {
                _moduleaccessid = value;
            }
        }
        
        public virtual System.Nullable<int> ExtUserid {
            get {
                return _extUserid;
            }
            set {
                _extUserid = value;
            }
        }
        
        public virtual System.Nullable<int> AccessLevel {
            get {
                return _accessLevel;
            }
            set {
                _accessLevel = value;
            }
        }
        
        public virtual System.Nullable<int> Exported {
            get {
                return _exported;
            }
            set {
                _exported = value;
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
        
        public virtual System.Nullable<int> CreatedBy {
            get {
                return _createdBy;
            }
            set {
                _createdBy = value;
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
        
        public virtual System.Nullable<int> UpdatedBy {
            get {
                return _updatedBy;
            }
            set {
                _updatedBy = value;
            }
        }
        
        public virtual LesModuleActions LesModuleActions {
            get {
                if ((this._lesModuleActions == null)) {
                    this._lesModuleActions = MetroLesMonitor.Bll.LesModuleActions.Load(this._moduleactionid);
                }
                return this._lesModuleActions;
            }
            set {
                _lesModuleActions = value;
            }
        }
        
        private void Clean() {
            this.Moduleaccessid = null;
            this._moduleactionid = null;
            this.ExtUserid = null;
            this.AccessLevel = null;
            this.Exported = null;
            this.CreatedDate = null;
            this.CreatedBy = null;
            this.UpdateSite = null;
            this.UpdateDate = null;
            this.UpdatedBy = null;
            this.LesModuleActions = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["MODULEACCESSID"] != System.DBNull.Value)) {
                this.Moduleaccessid = ((System.Nullable<int>)(dr["MODULEACCESSID"]));
            }
            if ((dr["MODULEACTIONID"] != System.DBNull.Value)) {
                this._moduleactionid = ((System.Nullable<int>)(dr["MODULEACTIONID"]));
            }
            if ((dr["EXT_USERID"] != System.DBNull.Value)) {
                this.ExtUserid = ((System.Nullable<int>)(dr["EXT_USERID"]));
            }
            if ((dr["ACCESS_LEVEL"] != System.DBNull.Value)) {
                this.AccessLevel = ((System.Nullable<int>)(dr["ACCESS_LEVEL"]));
            }
            if ((dr["EXPORTED"] != System.DBNull.Value)) {
                this.Exported = ((System.Nullable<int>)(dr["EXPORTED"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value)) {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["CREATED_BY"] != System.DBNull.Value)) {
                this.CreatedBy = ((System.Nullable<int>)(dr["CREATED_BY"]));
            }
            if ((dr["UPDATE_SITE"] != System.DBNull.Value)) {
                this.UpdateSite = ((System.Nullable<int>)(dr["UPDATE_SITE"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value)) {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
            if ((dr["UPDATED_BY"] != System.DBNull.Value)) {
                this.UpdatedBy = ((System.Nullable<int>)(dr["UPDATED_BY"]));
            }
        }
        
        public static LesModuleAccessCollection Select_LES_MODULE_ACCESSs_By_MODULEACTIONID(System.Nullable<int> MODULEACTIONID) {
            MetroLesMonitor.Dal.LesModuleAccess dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesModuleAccess();
                System.Data.DataSet ds = dbo.Select_LES_MODULE_ACCESSs_By_MODULEACTIONID(MODULEACTIONID);
                LesModuleAccessCollection collection = new LesModuleAccessCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesModuleAccess obj = new LesModuleAccess();
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
        
        public static LesModuleAccessCollection GetAll() {
            MetroLesMonitor.Dal.LesModuleAccess dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesModuleAccess();
                System.Data.DataSet ds = dbo.LES_MODULE_ACCESS_Select_All();
                LesModuleAccessCollection collection = new LesModuleAccessCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesModuleAccess obj = new LesModuleAccess();
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
        
        public static LesModuleAccess Load(System.Nullable<int> MODULEACCESSID) {
            MetroLesMonitor.Dal.LesModuleAccess dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesModuleAccess();
                System.Data.DataSet ds = dbo.LES_MODULE_ACCESS_Select_One(MODULEACCESSID);
                LesModuleAccess obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new LesModuleAccess();
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
            MetroLesMonitor.Dal.LesModuleAccess dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesModuleAccess();
                System.Data.DataSet ds = dbo.LES_MODULE_ACCESS_Select_One(this.Moduleaccessid);
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
            MetroLesMonitor.Dal.LesModuleAccess dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesModuleAccess();
                dbo.LES_MODULE_ACCESS_Insert(this._moduleactionid, this.ExtUserid, this.AccessLevel, this.Exported, this.CreatedDate, this.CreatedBy, this.UpdateSite, this.UpdateDate, this.UpdatedBy);
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
            MetroLesMonitor.Dal.LesModuleAccess dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesModuleAccess();
                dbo.LES_MODULE_ACCESS_Delete(this.Moduleaccessid);
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
            MetroLesMonitor.Dal.LesModuleAccess dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesModuleAccess();
                dbo.LES_MODULE_ACCESS_Update(this.Moduleaccessid, this._moduleactionid, this.ExtUserid, this.AccessLevel, this.Exported, this.CreatedDate, this.CreatedBy, this.UpdateSite, this.UpdateDate, this.UpdatedBy);
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
