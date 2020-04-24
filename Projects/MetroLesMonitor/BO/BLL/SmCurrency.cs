namespace MetroLesMonitor.Bll {
    
    
    public partial class SmCurrency {
        
        private System.Nullable<int> _currencyid;
        
        private string _currCode;
        
        private string _currDescr;
        
        private System.Nullable<float> _exchRate;
        
        private System.Nullable<System.DateTime> _currValidityDate;
        
        private string _currRemarks;
        
        private System.Nullable<int> _exported;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        private System.Nullable<int> _updateSite;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private System.Nullable<int> _siteid;
        
        private string _currSymbol;
        
        private System.Nullable<System.DateTime> _validFrom;
        
        private System.Nullable<int> _createdBy;
        
        private System.Nullable<int> _updateBy;
        
        public virtual System.Nullable<int> Currencyid {
            get {
                return _currencyid;
            }
            set {
                _currencyid = value;
            }
        }
        
        public virtual string CurrCode {
            get {
                return _currCode;
            }
            set {
                _currCode = value;
            }
        }
        
        public virtual string CurrDescr {
            get {
                return _currDescr;
            }
            set {
                _currDescr = value;
            }
        }
        
        public virtual System.Nullable<float> ExchRate {
            get {
                return _exchRate;
            }
            set {
                _exchRate = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> CurrValidityDate {
            get {
                return _currValidityDate;
            }
            set {
                _currValidityDate = value;
            }
        }
        
        public virtual string CurrRemarks {
            get {
                return _currRemarks;
            }
            set {
                _currRemarks = value;
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
        
        public virtual System.Nullable<System.DateTime> UpdateDate {
            get {
                return _updateDate;
            }
            set {
                _updateDate = value;
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
        
        public virtual string CurrSymbol {
            get {
                return _currSymbol;
            }
            set {
                _currSymbol = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> ValidFrom {
            get {
                return _validFrom;
            }
            set {
                _validFrom = value;
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
        
        public virtual System.Nullable<int> UpdateBy {
            get {
                return _updateBy;
            }
            set {
                _updateBy = value;
            }
        }
        
        private void Clean() {
            this.Currencyid = null;
            this.CurrCode = string.Empty;
            this.CurrDescr = string.Empty;
            this.ExchRate = null;
            this.CurrValidityDate = null;
            this.CurrRemarks = string.Empty;
            this.Exported = null;
            this.UpdateDate = null;
            this.UpdateSite = null;
            this.CreatedDate = null;
            this.Siteid = null;
            this.CurrSymbol = string.Empty;
            this.ValidFrom = null;
            this.CreatedBy = null;
            this.UpdateBy = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["CURRENCYID"] != System.DBNull.Value)) {
                this.Currencyid = ((System.Nullable<int>)(dr["CURRENCYID"]));
            }
            if ((dr["CURR_CODE"] != System.DBNull.Value)) {
                this.CurrCode = ((string)(dr["CURR_CODE"]));
            }
            if ((dr["CURR_DESCR"] != System.DBNull.Value)) {
                this.CurrDescr = ((string)(dr["CURR_DESCR"]));
            }
            if ((dr["EXCH_RATE"] != System.DBNull.Value)) {
                this.ExchRate = ((System.Nullable<float>)(dr["EXCH_RATE"]));
            }
            if ((dr["CURR_VALIDITY_DATE"] != System.DBNull.Value)) {
                this.CurrValidityDate = ((System.Nullable<System.DateTime>)(dr["CURR_VALIDITY_DATE"]));
            }
            if ((dr["CURR_REMARKS"] != System.DBNull.Value)) {
                this.CurrRemarks = ((string)(dr["CURR_REMARKS"]));
            }
            if ((dr["EXPORTED"] != System.DBNull.Value)) {
                this.Exported = ((System.Nullable<int>)(dr["EXPORTED"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value)) {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
            if ((dr["UPDATE_SITE"] != System.DBNull.Value)) {
                this.UpdateSite = ((System.Nullable<int>)(dr["UPDATE_SITE"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value)) {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["SITEID"] != System.DBNull.Value)) {
                this.Siteid = ((System.Nullable<int>)(dr["SITEID"]));
            }
            if ((dr["CURR_SYMBOL"] != System.DBNull.Value)) {
                this.CurrSymbol = ((string)(dr["CURR_SYMBOL"]));
            }
            if ((dr["VALID_FROM"] != System.DBNull.Value)) {
                this.ValidFrom = ((System.Nullable<System.DateTime>)(dr["VALID_FROM"]));
            }
            if ((dr["CREATED_BY"] != System.DBNull.Value)) {
                this.CreatedBy = ((System.Nullable<int>)(dr["CREATED_BY"]));
            }
            if ((dr["UPDATE_BY"] != System.DBNull.Value)) {
                this.UpdateBy = ((System.Nullable<int>)(dr["UPDATE_BY"]));
            }
        }
        
        public static SmCurrencyCollection GetAll() {
            MetroLesMonitor.Dal.SmCurrency dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmCurrency();
                System.Data.DataSet ds = dbo.SM_CURRENCY_Select_All();
                SmCurrencyCollection collection = new SmCurrencyCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmCurrency obj = new SmCurrency();
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
        
        public static SmCurrency Load(System.Nullable<int> CURRENCYID) {
            MetroLesMonitor.Dal.SmCurrency dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmCurrency();
                System.Data.DataSet ds = dbo.SM_CURRENCY_Select_One(CURRENCYID);
                SmCurrency obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmCurrency();
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
            MetroLesMonitor.Dal.SmCurrency dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmCurrency();
                System.Data.DataSet ds = dbo.SM_CURRENCY_Select_One(this.Currencyid);
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
            MetroLesMonitor.Dal.SmCurrency dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmCurrency();
                dbo.SM_CURRENCY_Insert(this.Currencyid, this.CurrCode, this.CurrDescr, this.ExchRate, this.CurrValidityDate, this.CurrRemarks, this.Exported, this.UpdateDate, this.UpdateSite, this.CreatedDate, this.Siteid, this.CurrSymbol, this.ValidFrom, this.CreatedBy, this.UpdateBy);
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
            MetroLesMonitor.Dal.SmCurrency dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmCurrency();
                dbo.SM_CURRENCY_Delete(this.Currencyid);
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
            MetroLesMonitor.Dal.SmCurrency dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmCurrency();
                dbo.SM_CURRENCY_Update(this.Currencyid, this.CurrCode, this.CurrDescr, this.ExchRate, this.CurrValidityDate, this.CurrRemarks, this.Exported, this.UpdateDate, this.UpdateSite, this.CreatedDate, this.Siteid, this.CurrSymbol, this.ValidFrom, this.CreatedBy, this.UpdateBy);
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
