namespace MetroLesMonitor.Bll {
    
    
    public partial class SmDocEquipment {
        
        private System.Nullable<int> _docequipid;
        
        private System.Nullable<int> _quotationid;
        
        private string _equipname;
        
        private string _equipdescription;
        
        private string _equipmaker;
        
        private string _equiptype;
        
        private string _equipserno;
        
        private string _equipdrgno;
        
        private string _equipremarks;
        
        public virtual System.Nullable<int> Docequipid {
            get {
                return _docequipid;
            }
            set {
                _docequipid = value;
            }
        }
        
        public virtual System.Nullable<int> Quotationid {
            get {
                return _quotationid;
            }
            set {
                _quotationid = value;
            }
        }
        
        public virtual string Equipname {
            get {
                return _equipname;
            }
            set {
                _equipname = value;
            }
        }
        
        public virtual string Equipdescription {
            get {
                return _equipdescription;
            }
            set {
                _equipdescription = value;
            }
        }
        
        public virtual string Equipmaker {
            get {
                return _equipmaker;
            }
            set {
                _equipmaker = value;
            }
        }
        
        public virtual string Equiptype {
            get {
                return _equiptype;
            }
            set {
                _equiptype = value;
            }
        }
        
        public virtual string Equipserno {
            get {
                return _equipserno;
            }
            set {
                _equipserno = value;
            }
        }
        
        public virtual string Equipdrgno {
            get {
                return _equipdrgno;
            }
            set {
                _equipdrgno = value;
            }
        }
        
        public virtual string Equipremarks {
            get {
                return _equipremarks;
            }
            set {
                _equipremarks = value;
            }
        }
        
        private void Clean() {
            this.Docequipid = null;
            this.Quotationid = null;
            this.Equipname = string.Empty;
            this.Equipdescription = string.Empty;
            this.Equipmaker = string.Empty;
            this.Equiptype = string.Empty;
            this.Equipserno = string.Empty;
            this.Equipdrgno = string.Empty;
            this.Equipremarks = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["DocEquipId"] != System.DBNull.Value)) {
                this.Docequipid = ((System.Nullable<int>)(dr["DocEquipId"]));
            }
            if ((dr["QuotationId"] != System.DBNull.Value)) {
                this.Quotationid = ((System.Nullable<int>)(dr["QuotationId"]));
            }
            if ((dr["EquipName"] != System.DBNull.Value)) {
                this.Equipname = ((string)(dr["EquipName"]));
            }
            if ((dr["EquipDescription"] != System.DBNull.Value)) {
                this.Equipdescription = ((string)(dr["EquipDescription"]));
            }
            if ((dr["EquipMaker"] != System.DBNull.Value)) {
                this.Equipmaker = ((string)(dr["EquipMaker"]));
            }
            if ((dr["EquipType"] != System.DBNull.Value)) {
                this.Equiptype = ((string)(dr["EquipType"]));
            }
            if ((dr["EquipSerNo"] != System.DBNull.Value)) {
                this.Equipserno = ((string)(dr["EquipSerNo"]));
            }
            if ((dr["EquipDrgNo"] != System.DBNull.Value)) {
                this.Equipdrgno = ((string)(dr["EquipDrgNo"]));
            }
            if ((dr["EquipRemarks"] != System.DBNull.Value)) {
                this.Equipremarks = ((string)(dr["EquipRemarks"]));
            }
        }
        
        public static SmDocEquipmentCollection GetAll() {
            MetroLesMonitor.Dal.SmDocEquipment dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDocEquipment();
                System.Data.DataSet ds = dbo.SM_DOC_EQUIPMENT_Select_All();
                SmDocEquipmentCollection collection = new SmDocEquipmentCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmDocEquipment obj = new SmDocEquipment();
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
        
        public static SmDocEquipment Load(System.Nullable<int> DocEquipId) {
            MetroLesMonitor.Dal.SmDocEquipment dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDocEquipment();
                System.Data.DataSet ds = dbo.SM_DOC_EQUIPMENT_Select_One(DocEquipId);
                SmDocEquipment obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmDocEquipment();
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
            MetroLesMonitor.Dal.SmDocEquipment dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDocEquipment();
                System.Data.DataSet ds = dbo.SM_DOC_EQUIPMENT_Select_One(this.Docequipid);
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
            MetroLesMonitor.Dal.SmDocEquipment dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDocEquipment();
                dbo.SM_DOC_EQUIPMENT_Insert(this.Quotationid, this.Equipname, this.Equipdescription, this.Equipmaker, this.Equiptype, this.Equipserno, this.Equipdrgno, this.Equipremarks);
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
            MetroLesMonitor.Dal.SmDocEquipment dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDocEquipment();
                dbo.SM_DOC_EQUIPMENT_Delete(this.Docequipid);
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
            MetroLesMonitor.Dal.SmDocEquipment dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDocEquipment();
                dbo.SM_DOC_EQUIPMENT_Update(this.Docequipid, this.Quotationid, this.Equipname, this.Equipdescription, this.Equipmaker, this.Equiptype, this.Equipserno, this.Equipdrgno, this.Equipremarks);
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
