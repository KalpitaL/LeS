namespace MetroLesMonitor.Bll {
    
    
    public partial class MtmlDocAmounts {
        
        private System.Nullable<System.Guid> _amountid;
        
        private System.Nullable<System.Guid> _mtmldocid;
        
        private string _qualifier;
        
        private System.Nullable<float> _amtValue;
        
        private System.Nullable<int> _autoid;
        
        public virtual System.Nullable<System.Guid> Amountid {
            get {
                return _amountid;
            }
            set {
                _amountid = value;
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
        
        public virtual string Qualifier {
            get {
                return _qualifier;
            }
            set {
                _qualifier = value;
            }
        }
        
        public virtual System.Nullable<float> AmtValue {
            get {
                return _amtValue;
            }
            set {
                _amtValue = value;
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
            this.Amountid = null;
            this.Mtmldocid = null;
            this.Qualifier = string.Empty;
            this.AmtValue = null;
            this.Autoid = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["AMOUNTID"] != System.DBNull.Value)) {
                this.Amountid = ((System.Nullable<System.Guid>)(dr["AMOUNTID"]));
            }
            if ((dr["MTMLDOCID"] != System.DBNull.Value)) {
                this.Mtmldocid = ((System.Nullable<System.Guid>)(dr["MTMLDOCID"]));
            }
            if ((dr["QUALIFIER"] != System.DBNull.Value)) {
                this.Qualifier = ((string)(dr["QUALIFIER"]));
            }
            if ((dr["AMT_VALUE"] != System.DBNull.Value)) {
                this.AmtValue = ((System.Nullable<float>)(dr["AMT_VALUE"]));
            }
            if ((dr["AUTOID"] != System.DBNull.Value)) {
                this.Autoid = ((System.Nullable<int>)(dr["AUTOID"]));
            }
        }
        
        public static MtmlDocAmountsCollection GetAll() {
            MetroLesMonitor.Dal.MtmlDocAmounts dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocAmounts();
                System.Data.DataSet ds = dbo.MTML_DOC_AMOUNTS_Select_All();
                MtmlDocAmountsCollection collection = new MtmlDocAmountsCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        MtmlDocAmounts obj = new MtmlDocAmounts();
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
        
        public static MtmlDocAmounts Load(System.Nullable<System.Guid> AMOUNTID) {
            MetroLesMonitor.Dal.MtmlDocAmounts dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocAmounts();
                System.Data.DataSet ds = dbo.MTML_DOC_AMOUNTS_Select_One(AMOUNTID);
                MtmlDocAmounts obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new MtmlDocAmounts();
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
            MetroLesMonitor.Dal.MtmlDocAmounts dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocAmounts();
                System.Data.DataSet ds = dbo.MTML_DOC_AMOUNTS_Select_One(this.Amountid);
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
            MetroLesMonitor.Dal.MtmlDocAmounts dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocAmounts();
                dbo.MTML_DOC_AMOUNTS_Insert(this.Mtmldocid, this.Qualifier, this.AmtValue);
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
            MetroLesMonitor.Dal.MtmlDocAmounts dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocAmounts();
                dbo.MTML_DOC_AMOUNTS_Delete(this.Amountid);
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
            MetroLesMonitor.Dal.MtmlDocAmounts dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDocAmounts();
                dbo.MTML_DOC_AMOUNTS_Update(this.Amountid, this.Mtmldocid, this.Qualifier, this.AmtValue, this.Autoid);
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
