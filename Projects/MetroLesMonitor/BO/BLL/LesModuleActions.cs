namespace MetroLesMonitor.Bll {
    
    
    public partial class LesModuleActions {
        
        private System.Nullable<int> _moduleactionid;
        
        private string _actionName;
        
        private System.Nullable<int> _moduleid;
        
        private LesModuleAccessCollection _lesModuleAccessCollection;
        
        public virtual System.Nullable<int> Moduleactionid {
            get {
                return _moduleactionid;
            }
            set {
                _moduleactionid = value;
            }
        }
        
        public virtual string ActionName {
            get {
                return _actionName;
            }
            set {
                _actionName = value;
            }
        }
        
        public virtual System.Nullable<int> Moduleid {
            get {
                return _moduleid;
            }
            set {
                _moduleid = value;
            }
        }
        
        public virtual LesModuleAccessCollection LesModuleAccessCollection {
            get {
                if ((this._lesModuleAccessCollection == null)) {
                    _lesModuleAccessCollection = MetroLesMonitor.Bll.LesModuleAccess.Select_LES_MODULE_ACCESSs_By_MODULEACTIONID(this.Moduleactionid);
                }
                return this._lesModuleAccessCollection;
            }
        }
        
        private void Clean() {
            this.Moduleactionid = null;
            this.ActionName = string.Empty;
            this.Moduleid = null;
            this._lesModuleAccessCollection = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["MODULEACTIONID"] != System.DBNull.Value)) {
                this.Moduleactionid = ((System.Nullable<int>)(dr["MODULEACTIONID"]));
            }
            if ((dr["ACTION_NAME"] != System.DBNull.Value)) {
                this.ActionName = ((string)(dr["ACTION_NAME"]));
            }
            if ((dr["MODULEID"] != System.DBNull.Value)) {
                this.Moduleid = ((System.Nullable<int>)(dr["MODULEID"]));
            }
        }
        
        public static LesModuleActionsCollection GetAll() {
            MetroLesMonitor.Dal.LesModuleActions dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesModuleActions();
                System.Data.DataSet ds = dbo.LES_MODULE_ACTIONS_Select_All();
                LesModuleActionsCollection collection = new LesModuleActionsCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesModuleActions obj = new LesModuleActions();
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
        
        public static LesModuleActions Load(System.Nullable<int> MODULEACTIONID) {
            MetroLesMonitor.Dal.LesModuleActions dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesModuleActions();
                System.Data.DataSet ds = dbo.LES_MODULE_ACTIONS_Select_One(MODULEACTIONID);
                LesModuleActions obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new LesModuleActions();
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
            MetroLesMonitor.Dal.LesModuleActions dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesModuleActions();
                System.Data.DataSet ds = dbo.LES_MODULE_ACTIONS_Select_One(this.Moduleactionid);
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
            MetroLesMonitor.Dal.LesModuleActions dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesModuleActions();
                dbo.LES_MODULE_ACTIONS_Insert(this.ActionName, this.Moduleid);
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
            MetroLesMonitor.Dal.LesModuleActions dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesModuleActions();
                dbo.LES_MODULE_ACTIONS_Delete(this.Moduleactionid);
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
            MetroLesMonitor.Dal.LesModuleActions dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesModuleActions();
                dbo.LES_MODULE_ACTIONS_Update(this.Moduleactionid, this.ActionName, this.Moduleid);
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
