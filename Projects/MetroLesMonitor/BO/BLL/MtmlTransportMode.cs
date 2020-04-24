namespace MetroLesMonitor.Bll {
    
    
    public partial class MtmlTransportMode {
        
        private System.Nullable<int> _transportId;
        
        private string _transportMode;
        
        private string _transportDesc;
        
        public virtual System.Nullable<int> TransportId {
            get {
                return _transportId;
            }
            set {
                _transportId = value;
            }
        }
        
        public virtual string TransportMode {
            get {
                return _transportMode;
            }
            set {
                _transportMode = value;
            }
        }
        
        public virtual string TransportDesc {
            get {
                return _transportDesc;
            }
            set {
                _transportDesc = value;
            }
        }
        
        private void Clean() {
            this.TransportId = null;
            this.TransportMode = string.Empty;
            this.TransportDesc = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["TRANSPORT_ID"] != System.DBNull.Value)) {
                this.TransportId = ((System.Nullable<int>)(dr["TRANSPORT_ID"]));
            }
            if ((dr["TRANSPORT_MODE"] != System.DBNull.Value)) {
                this.TransportMode = ((string)(dr["TRANSPORT_MODE"]));
            }
            if ((dr["TRANSPORT_DESC"] != System.DBNull.Value)) {
                this.TransportDesc = ((string)(dr["TRANSPORT_DESC"]));
            }
        }
        
        public static MtmlTransportModeCollection GetAll() {
            MetroLesMonitor.Dal.MtmlTransportMode dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlTransportMode();
                System.Data.DataSet ds = dbo.MTML_TRANSPORT_MODE_Select_All();
                MtmlTransportModeCollection collection = new MtmlTransportModeCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        MtmlTransportMode obj = new MtmlTransportMode();
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
        
        public static MtmlTransportMode Load(System.Nullable<int> TRANSPORT_ID) {
            MetroLesMonitor.Dal.MtmlTransportMode dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlTransportMode();
                System.Data.DataSet ds = dbo.MTML_TRANSPORT_MODE_Select_One(TRANSPORT_ID);
                MtmlTransportMode obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new MtmlTransportMode();
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
            MetroLesMonitor.Dal.MtmlTransportMode dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlTransportMode();
                System.Data.DataSet ds = dbo.MTML_TRANSPORT_MODE_Select_One(this.TransportId);
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
            MetroLesMonitor.Dal.MtmlTransportMode dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlTransportMode();
                dbo.MTML_TRANSPORT_MODE_Insert(this.TransportMode, this.TransportDesc);
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
            MetroLesMonitor.Dal.MtmlTransportMode dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlTransportMode();
                dbo.MTML_TRANSPORT_MODE_Delete(this.TransportId);
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
            MetroLesMonitor.Dal.MtmlTransportMode dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlTransportMode();
                dbo.MTML_TRANSPORT_MODE_Update(this.TransportId, this.TransportMode, this.TransportDesc);
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
