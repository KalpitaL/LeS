namespace MetroLesMonitor.Bll {
    
    
    public partial class SmCurrencyMapped {
        
        private System.Nullable<int> _autoid;
        
        private string _sourceCurrCode;
        
        private string _targetCurrCode;
        
        private System.Nullable<int> _mapid;
        
        public virtual System.Nullable<int> Autoid {
            get {
                return _autoid;
            }
            set {
                _autoid = value;
            }
        }
        
        public virtual string SourceCurrCode {
            get {
                return _sourceCurrCode;
            }
            set {
                _sourceCurrCode = value;
            }
        }
        
        public virtual string TargetCurrCode {
            get {
                return _targetCurrCode;
            }
            set {
                _targetCurrCode = value;
            }
        }
        
        public virtual System.Nullable<int> Mapid {
            get {
                return _mapid;
            }
            set {
                _mapid = value;
            }
        }
        
        private void Clean() {
            this.Autoid = null;
            this.SourceCurrCode = string.Empty;
            this.TargetCurrCode = string.Empty;
            this.Mapid = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["AUTOID"] != System.DBNull.Value)) {
                this.Autoid = ((System.Nullable<int>)(dr["AUTOID"]));
            }
            if ((dr["SOURCE_CURR_CODE"] != System.DBNull.Value)) {
                this.SourceCurrCode = ((string)(dr["SOURCE_CURR_CODE"]));
            }
            if ((dr["TARGET_CURR_CODE"] != System.DBNull.Value)) {
                this.TargetCurrCode = ((string)(dr["TARGET_CURR_CODE"]));
            }
            if ((dr["MAPID"] != System.DBNull.Value)) {
                this.Mapid = ((System.Nullable<int>)(dr["MAPID"]));
            }
        }
        
        public static SmCurrencyMappedCollection GetAll() {
            MetroLesMonitor.Dal.SmCurrencyMapped dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmCurrencyMapped();
                System.Data.DataSet ds = dbo.SM_CURRENCY_MAPPED_Select_All();
                SmCurrencyMappedCollection collection = new SmCurrencyMappedCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmCurrencyMapped obj = new SmCurrencyMapped();
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
        
        public static SmCurrencyMapped Load(System.Nullable<int> AUTOID) {
            MetroLesMonitor.Dal.SmCurrencyMapped dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmCurrencyMapped();
                System.Data.DataSet ds = dbo.SM_CURRENCY_MAPPED_Select_One(AUTOID);
                SmCurrencyMapped obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmCurrencyMapped();
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
            MetroLesMonitor.Dal.SmCurrencyMapped dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmCurrencyMapped();
                System.Data.DataSet ds = dbo.SM_CURRENCY_MAPPED_Select_One(this.Autoid);
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
            MetroLesMonitor.Dal.SmCurrencyMapped dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmCurrencyMapped();
                dbo.SM_CURRENCY_MAPPED_Insert(this.Autoid, this.SourceCurrCode, this.TargetCurrCode, this.Mapid);
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
            MetroLesMonitor.Dal.SmCurrencyMapped dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmCurrencyMapped();
                dbo.SM_CURRENCY_MAPPED_Delete(this.Autoid);
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
            MetroLesMonitor.Dal.SmCurrencyMapped dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmCurrencyMapped();
                dbo.SM_CURRENCY_MAPPED_Update(this.Autoid, this.SourceCurrCode, this.TargetCurrCode, this.Mapid);
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
