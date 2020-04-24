namespace MetroLesMonitor.Bll {
    
    
    public partial class LesVessels {
        
        private System.Nullable<int> _vesselid;
        
        private string _vesselName;
        
        private string _markingNo;
        
        private System.Nullable<System.DateTime> _eta;
        
        private System.Nullable<System.DateTime> _etd;
        
        private string _berth;
        
        private System.Nullable<int> _imoNo;
        
        public virtual System.Nullable<int> Vesselid {
            get {
                return _vesselid;
            }
            set {
                _vesselid = value;
            }
        }
        
        public virtual string VesselName {
            get {
                return _vesselName;
            }
            set {
                _vesselName = value;
            }
        }
        
        public virtual string MarkingNo {
            get {
                return _markingNo;
            }
            set {
                _markingNo = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> Eta {
            get {
                return _eta;
            }
            set {
                _eta = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> Etd {
            get {
                return _etd;
            }
            set {
                _etd = value;
            }
        }
        
        public virtual string Berth {
            get {
                return _berth;
            }
            set {
                _berth = value;
            }
        }
        
        public virtual System.Nullable<int> ImoNo {
            get {
                return _imoNo;
            }
            set {
                _imoNo = value;
            }
        }
        
        private void Clean() {
            this.Vesselid = null;
            this.VesselName = string.Empty;
            this.MarkingNo = string.Empty;
            this.Eta = null;
            this.Etd = null;
            this.Berth = string.Empty;
            this.ImoNo = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["VESSELID"] != System.DBNull.Value)) {
                this.Vesselid = ((System.Nullable<int>)(dr["VESSELID"]));
            }
            if ((dr["VESSEL_NAME"] != System.DBNull.Value)) {
                this.VesselName = ((string)(dr["VESSEL_NAME"]));
            }
            if ((dr["MARKING_NO"] != System.DBNull.Value)) {
                this.MarkingNo = ((string)(dr["MARKING_NO"]));
            }
            if ((dr["ETA"] != System.DBNull.Value)) {
                this.Eta = ((System.Nullable<System.DateTime>)(dr["ETA"]));
            }
            if ((dr["ETD"] != System.DBNull.Value)) {
                this.Etd = ((System.Nullable<System.DateTime>)(dr["ETD"]));
            }
            if ((dr["BERTH"] != System.DBNull.Value)) {
                this.Berth = ((string)(dr["BERTH"]));
            }
            if ((dr["imo_no"] != System.DBNull.Value)) {
                this.ImoNo = ((System.Nullable<int>)(dr["imo_no"]));
            }
        }
        
        public static LesVesselsCollection GetAll() {
            MetroLesMonitor.Dal.LesVessels dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesVessels();
                System.Data.DataSet ds = dbo.LES_VESSELS_Select_All();
                LesVesselsCollection collection = new LesVesselsCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesVessels obj = new LesVessels();
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
        
        public static LesVessels Load(System.Nullable<int> VESSELID) {
            MetroLesMonitor.Dal.LesVessels dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesVessels();
                System.Data.DataSet ds = dbo.LES_VESSELS_Select_One(VESSELID);
                LesVessels obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new LesVessels();
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
            MetroLesMonitor.Dal.LesVessels dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesVessels();
                System.Data.DataSet ds = dbo.LES_VESSELS_Select_One(this.Vesselid);
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
            MetroLesMonitor.Dal.LesVessels dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesVessels();
                dbo.LES_VESSELS_Insert(this.VesselName, this.MarkingNo, this.Eta, this.Etd, this.Berth, this.ImoNo);
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
            MetroLesMonitor.Dal.LesVessels dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesVessels();
                dbo.LES_VESSELS_Delete(this.Vesselid);
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
            MetroLesMonitor.Dal.LesVessels dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesVessels();
                dbo.LES_VESSELS_Update(this.Vesselid, this.VesselName, this.MarkingNo, this.Eta, this.Etd, this.Berth, this.ImoNo);
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
