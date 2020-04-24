namespace MetroLesMonitor.Bll {
    
    
    public partial class MtmlDeliveryTerms {
        
        private System.Nullable<int> _delTermId;
        
        private string _delTermCode;
        
        private string _delTermDesc;
        
        public virtual System.Nullable<int> DelTermId {
            get {
                return _delTermId;
            }
            set {
                _delTermId = value;
            }
        }
        
        public virtual string DelTermCode {
            get {
                return _delTermCode;
            }
            set {
                _delTermCode = value;
            }
        }
        
        public virtual string DelTermDesc {
            get {
                return _delTermDesc;
            }
            set {
                _delTermDesc = value;
            }
        }
        
        private void Clean() {
            this.DelTermId = null;
            this.DelTermCode = string.Empty;
            this.DelTermDesc = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["DEL_TERM_ID"] != System.DBNull.Value)) {
                this.DelTermId = ((System.Nullable<int>)(dr["DEL_TERM_ID"]));
            }
            if ((dr["DEL_TERM_CODE"] != System.DBNull.Value)) {
                this.DelTermCode = ((string)(dr["DEL_TERM_CODE"]));
            }
            if ((dr["DEL_TERM_DESC"] != System.DBNull.Value)) {
                this.DelTermDesc = ((string)(dr["DEL_TERM_DESC"]));
            }
        }
        
        public static MtmlDeliveryTermsCollection GetAll() {
            MetroLesMonitor.Dal.MtmlDeliveryTerms dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDeliveryTerms();
                System.Data.DataSet ds = dbo.MTML_DELIVERY_TERMS_Select_All();
                MtmlDeliveryTermsCollection collection = new MtmlDeliveryTermsCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        MtmlDeliveryTerms obj = new MtmlDeliveryTerms();
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
        
        public virtual void Insert() {
            MetroLesMonitor.Dal.MtmlDeliveryTerms dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MtmlDeliveryTerms();
                dbo.MTML_DELIVERY_TERMS_Insert(this.DelTermCode, this.DelTermDesc);
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
