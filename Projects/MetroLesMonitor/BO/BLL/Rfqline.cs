namespace MetroLesMonitor.Bll {
    
    
    public partial class Rfqline {
        
        private System.Nullable<int> _masRecKey;
        
        private System.Nullable<int> _recKey;
        
        private System.Nullable<int> _lineNo;
        
        private string _lineRef;
        
        private System.Nullable<char> _lineType;
        
        private string _stkId;
        
        private string _name;
        
        private string _model;
        
        private System.Nullable<decimal> _uomQty;
        
        private string _uom;
        
        private System.Nullable<decimal> _listPrice;
        
        private string _discChr;
        
        private System.Nullable<decimal> _discNum;
        
        private System.Nullable<decimal> _netPrice;
        
        private string _remark;
        
        private string _srcCode;
        
        private string _srcLocId;
        
        private System.Nullable<int> _srcRecKey;
        
        private System.Nullable<int> _srcLineRecKey;
        
        private string _srcDocId;
        
        private System.Nullable<int> _oriRecKey;
        
        private Rfqmas _rfqmas;
        
        public virtual System.Nullable<int> RecKey {
            get {
                return _recKey;
            }
            set {
                _recKey = value;
            }
        }
        
        public virtual System.Nullable<int> LineNo {
            get {
                return _lineNo;
            }
            set {
                _lineNo = value;
            }
        }
        
        public virtual string LineRef {
            get {
                return _lineRef;
            }
            set {
                _lineRef = value;
            }
        }
        
        public virtual System.Nullable<char> LineType {
            get {
                return _lineType;
            }
            set {
                _lineType = value;
            }
        }
        
        public virtual string StkId {
            get {
                return _stkId;
            }
            set {
                _stkId = value;
            }
        }
        
        public virtual string Name {
            get {
                return _name;
            }
            set {
                _name = value;
            }
        }
        
        public virtual string Model {
            get {
                return _model;
            }
            set {
                _model = value;
            }
        }
        
        public virtual System.Nullable<decimal> UomQty {
            get {
                return _uomQty;
            }
            set {
                _uomQty = value;
            }
        }
        
        public virtual string Uom {
            get {
                return _uom;
            }
            set {
                _uom = value;
            }
        }
        
        public virtual System.Nullable<decimal> ListPrice {
            get {
                return _listPrice;
            }
            set {
                _listPrice = value;
            }
        }
        
        public virtual string DiscChr {
            get {
                return _discChr;
            }
            set {
                _discChr = value;
            }
        }
        
        public virtual System.Nullable<decimal> DiscNum {
            get {
                return _discNum;
            }
            set {
                _discNum = value;
            }
        }
        
        public virtual System.Nullable<decimal> NetPrice {
            get {
                return _netPrice;
            }
            set {
                _netPrice = value;
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
        
        public virtual string SrcCode {
            get {
                return _srcCode;
            }
            set {
                _srcCode = value;
            }
        }
        
        public virtual string SrcLocId {
            get {
                return _srcLocId;
            }
            set {
                _srcLocId = value;
            }
        }
        
        public virtual System.Nullable<int> SrcRecKey {
            get {
                return _srcRecKey;
            }
            set {
                _srcRecKey = value;
            }
        }
        
        public virtual System.Nullable<int> SrcLineRecKey {
            get {
                return _srcLineRecKey;
            }
            set {
                _srcLineRecKey = value;
            }
        }
        
        public virtual string SrcDocId {
            get {
                return _srcDocId;
            }
            set {
                _srcDocId = value;
            }
        }
        
        public virtual System.Nullable<int> OriRecKey {
            get {
                return _oriRecKey;
            }
            set {
                _oriRecKey = value;
            }
        }
        
        public virtual Rfqmas Rfqmas {
            get {
                if ((this._rfqmas == null)) {
                    this._rfqmas = MetroLesMonitor.Bll.Rfqmas.Load(this._masRecKey);
                }
                return this._rfqmas;
            }
            set {
                _rfqmas = value;
            }
        }
        
        private void Clean() {
            this._masRecKey = null;
            this.RecKey = null;
            this.LineNo = null;
            this.LineRef = string.Empty;
            this.LineType = null;
            this.StkId = string.Empty;
            this.Name = string.Empty;
            this.Model = string.Empty;
            this.UomQty = null;
            this.Uom = string.Empty;
            this.ListPrice = null;
            this.DiscChr = string.Empty;
            this.DiscNum = null;
            this.NetPrice = null;
            this.Remark = string.Empty;
            this.SrcCode = string.Empty;
            this.SrcLocId = string.Empty;
            this.SrcRecKey = null;
            this.SrcLineRecKey = null;
            this.SrcDocId = string.Empty;
            this.OriRecKey = null;
            this.Rfqmas = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["MAS_REC_KEY"] != System.DBNull.Value)) {
                this._masRecKey = ((System.Nullable<int>)(dr["MAS_REC_KEY"]));
            }
            if ((dr["REC_KEY"] != System.DBNull.Value)) {
                this.RecKey = ((System.Nullable<int>)(dr["REC_KEY"]));
            }
            if ((dr["LINE_NO"] != System.DBNull.Value)) {
                this.LineNo = ((System.Nullable<int>)(dr["LINE_NO"]));
            }
            if ((dr["LINE_REF"] != System.DBNull.Value)) {
                this.LineRef = ((string)(dr["LINE_REF"]));
            }
            if ((dr["LINE_TYPE"] != System.DBNull.Value)) {
                this.LineType = ((System.Nullable<char>)(dr["LINE_TYPE"]));
            }
            if ((dr["STK_ID"] != System.DBNull.Value)) {
                this.StkId = ((string)(dr["STK_ID"]));
            }
            if ((dr["NAME"] != System.DBNull.Value)) {
                this.Name = ((string)(dr["NAME"]));
            }
            if ((dr["MODEL"] != System.DBNull.Value)) {
                this.Model = ((string)(dr["MODEL"]));
            }
            if ((dr["UOM_QTY"] != System.DBNull.Value)) {
                this.UomQty = ((System.Nullable<decimal>)(dr["UOM_QTY"]));
            }
            if ((dr["UOM"] != System.DBNull.Value)) {
                this.Uom = ((string)(dr["UOM"]));
            }
            if ((dr["LIST_PRICE"] != System.DBNull.Value)) {
                this.ListPrice = ((System.Nullable<decimal>)(dr["LIST_PRICE"]));
            }
            if ((dr["DISC_CHR"] != System.DBNull.Value)) {
                this.DiscChr = ((string)(dr["DISC_CHR"]));
            }
            if ((dr["DISC_NUM"] != System.DBNull.Value)) {
                this.DiscNum = ((System.Nullable<decimal>)(dr["DISC_NUM"]));
            }
            if ((dr["NET_PRICE"] != System.DBNull.Value)) {
                this.NetPrice = ((System.Nullable<decimal>)(dr["NET_PRICE"]));
            }
            if ((dr["REMARK"] != System.DBNull.Value)) {
                this.Remark = ((string)(dr["REMARK"]));
            }
            if ((dr["SRC_CODE"] != System.DBNull.Value)) {
                this.SrcCode = ((string)(dr["SRC_CODE"]));
            }
            if ((dr["SRC_LOC_ID"] != System.DBNull.Value)) {
                this.SrcLocId = ((string)(dr["SRC_LOC_ID"]));
            }
            if ((dr["SRC_REC_KEY"] != System.DBNull.Value)) {
                this.SrcRecKey = ((System.Nullable<int>)(dr["SRC_REC_KEY"]));
            }
            if ((dr["SRC_LINE_REC_KEY"] != System.DBNull.Value)) {
                this.SrcLineRecKey = ((System.Nullable<int>)(dr["SRC_LINE_REC_KEY"]));
            }
            if ((dr["SRC_DOC_ID"] != System.DBNull.Value)) {
                this.SrcDocId = ((string)(dr["SRC_DOC_ID"]));
            }
            if ((dr["ORI_REC_KEY"] != System.DBNull.Value)) {
                this.OriRecKey = ((System.Nullable<int>)(dr["ORI_REC_KEY"]));
            }
        }
        
        public static RfqlineCollection Select_RFQLINEs_By_MAS_REC_KEY(System.Nullable<int> MAS_REC_KEY) {
            MetroLesMonitor.Dal.Rfqline dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.Rfqline();
                System.Data.DataSet ds = dbo.Select_RFQLINEs_By_MAS_REC_KEY(MAS_REC_KEY);
                RfqlineCollection collection = new RfqlineCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        Rfqline obj = new Rfqline();
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
        
        public static RfqlineCollection GetAll() {
            MetroLesMonitor.Dal.Rfqline dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.Rfqline();
                System.Data.DataSet ds = dbo.RFQLINE_Select_All();
                RfqlineCollection collection = new RfqlineCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        Rfqline obj = new Rfqline();
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
        
        public static Rfqline Load(System.Nullable<int> REC_KEY) {
            MetroLesMonitor.Dal.Rfqline dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.Rfqline();
                System.Data.DataSet ds = dbo.RFQLINE_Select_One(REC_KEY);
                Rfqline obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new Rfqline();
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
            MetroLesMonitor.Dal.Rfqline dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.Rfqline();
                System.Data.DataSet ds = dbo.RFQLINE_Select_One(this.RecKey);
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
            MetroLesMonitor.Dal.Rfqline dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.Rfqline();
                dbo.RFQLINE_Insert(this._masRecKey, this.RecKey, this.LineNo, this.LineRef, this.LineType, this.StkId, this.Name, this.Model, this.UomQty, this.Uom, this.ListPrice, this.DiscChr, this.DiscNum, this.NetPrice, this.Remark, this.SrcCode, this.SrcLocId, this.SrcRecKey, this.SrcLineRecKey, this.SrcDocId, this.OriRecKey);
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
            MetroLesMonitor.Dal.Rfqline dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.Rfqline();
                dbo.RFQLINE_Delete(this.RecKey);
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
            MetroLesMonitor.Dal.Rfqline dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.Rfqline();
                dbo.RFQLINE_Update(this._masRecKey, this.RecKey, this.LineNo, this.LineRef, this.LineType, this.StkId, this.Name, this.Model, this.UomQty, this.Uom, this.ListPrice, this.DiscChr, this.DiscNum, this.NetPrice, this.Remark, this.SrcCode, this.SrcLocId, this.SrcRecKey, this.SrcLineRecKey, this.SrcDocId, this.OriRecKey);
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
