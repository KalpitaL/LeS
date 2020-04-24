namespace MetroLesMonitor.Bll {
    
    
    public partial class LesSalesorder {
        
        private System.Nullable<int> _salesorderid;
        
        private string _salesorderno;
        
        private System.Nullable<int> _customerid;
        
        private System.Nullable<int> _status;
        
        private System.Nullable<int> _currencyid;
        
        private string _buyerref;
        
        private string _shipname;
        
        private System.Nullable<System.DateTime> _rfqDelDate;
        
        private System.Nullable<int> _deliveryDays;
        
        private System.Nullable<float> _discount;
        
        private string _supplierRemarks;
        
        private string _payTerms;
        
        private string _generalTerms;
        
        private System.Nullable<float> _profitmargin;
        
        private System.Nullable<float> _amount;
        
        private System.Nullable<float> _freightamt;
        
        private System.Nullable<float> _othercosts;
        
        private string _quoteReference;
        
        private System.Nullable<System.DateTime> _quoteValidity;
        
        private string _quoteSubject;
        
        private System.Nullable<int> _quotationid;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private System.Nullable<int> _updatedBy;
        
        private System.Nullable<int> _createdBy;
        
        private System.Nullable<System.DateTime> _vesselEta;
        
        private string _vesselArrtime;
        
        private System.Nullable<float> _itemTotal;
        
        public virtual System.Nullable<int> Salesorderid {
            get {
                return _salesorderid;
            }
            set {
                _salesorderid = value;
            }
        }
        
        public virtual string Salesorderno {
            get {
                return _salesorderno;
            }
            set {
                _salesorderno = value;
            }
        }
        
        public virtual System.Nullable<int> Customerid {
            get {
                return _customerid;
            }
            set {
                _customerid = value;
            }
        }
        
        public virtual System.Nullable<int> Status {
            get {
                return _status;
            }
            set {
                _status = value;
            }
        }
        
        public virtual System.Nullable<int> Currencyid {
            get {
                return _currencyid;
            }
            set {
                _currencyid = value;
            }
        }
        
        public virtual string Buyerref {
            get {
                return _buyerref;
            }
            set {
                _buyerref = value;
            }
        }
        
        public virtual string Shipname {
            get {
                return _shipname;
            }
            set {
                _shipname = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> RfqDelDate {
            get {
                return _rfqDelDate;
            }
            set {
                _rfqDelDate = value;
            }
        }
        
        public virtual System.Nullable<int> DeliveryDays {
            get {
                return _deliveryDays;
            }
            set {
                _deliveryDays = value;
            }
        }
        
        public virtual System.Nullable<float> Discount {
            get {
                return _discount;
            }
            set {
                _discount = value;
            }
        }
        
        public virtual string SupplierRemarks {
            get {
                return _supplierRemarks;
            }
            set {
                _supplierRemarks = value;
            }
        }
        
        public virtual string PayTerms {
            get {
                return _payTerms;
            }
            set {
                _payTerms = value;
            }
        }
        
        public virtual string GeneralTerms {
            get {
                return _generalTerms;
            }
            set {
                _generalTerms = value;
            }
        }
        
        public virtual System.Nullable<float> Profitmargin {
            get {
                return _profitmargin;
            }
            set {
                _profitmargin = value;
            }
        }
        
        public virtual System.Nullable<float> Amount {
            get {
                return _amount;
            }
            set {
                _amount = value;
            }
        }
        
        public virtual System.Nullable<float> Freightamt {
            get {
                return _freightamt;
            }
            set {
                _freightamt = value;
            }
        }
        
        public virtual System.Nullable<float> Othercosts {
            get {
                return _othercosts;
            }
            set {
                _othercosts = value;
            }
        }
        
        public virtual string QuoteReference {
            get {
                return _quoteReference;
            }
            set {
                _quoteReference = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> QuoteValidity {
            get {
                return _quoteValidity;
            }
            set {
                _quoteValidity = value;
            }
        }
        
        public virtual string QuoteSubject {
            get {
                return _quoteSubject;
            }
            set {
                _quoteSubject = value;
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
        
        public virtual System.Nullable<System.DateTime> UpdateDate {
            get {
                return _updateDate;
            }
            set {
                _updateDate = value;
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
        
        public virtual System.Nullable<int> UpdatedBy {
            get {
                return _updatedBy;
            }
            set {
                _updatedBy = value;
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
        
        public virtual System.Nullable<System.DateTime> VesselEta {
            get {
                return _vesselEta;
            }
            set {
                _vesselEta = value;
            }
        }
        
        public virtual string VesselArrtime {
            get {
                return _vesselArrtime;
            }
            set {
                _vesselArrtime = value;
            }
        }
        
        public virtual System.Nullable<float> ItemTotal {
            get {
                return _itemTotal;
            }
            set {
                _itemTotal = value;
            }
        }
        
        private void Clean() {
            this.Salesorderid = null;
            this.Salesorderno = string.Empty;
            this.Customerid = null;
            this.Status = null;
            this.Currencyid = null;
            this.Buyerref = string.Empty;
            this.Shipname = string.Empty;
            this.RfqDelDate = null;
            this.DeliveryDays = null;
            this.Discount = null;
            this.SupplierRemarks = string.Empty;
            this.PayTerms = string.Empty;
            this.GeneralTerms = string.Empty;
            this.Profitmargin = null;
            this.Amount = null;
            this.Freightamt = null;
            this.Othercosts = null;
            this.QuoteReference = string.Empty;
            this.QuoteValidity = null;
            this.QuoteSubject = string.Empty;
            this.Quotationid = null;
            this.UpdateDate = null;
            this.CreatedDate = null;
            this.UpdatedBy = null;
            this.CreatedBy = null;
            this.VesselEta = null;
            this.VesselArrtime = string.Empty;
            this.ItemTotal = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["SALESORDERID"] != System.DBNull.Value)) {
                this.Salesorderid = ((System.Nullable<int>)(dr["SALESORDERID"]));
            }
            if ((dr["SALESORDERNO"] != System.DBNull.Value)) {
                this.Salesorderno = ((string)(dr["SALESORDERNO"]));
            }
            if ((dr["CUSTOMERID"] != System.DBNull.Value)) {
                this.Customerid = ((System.Nullable<int>)(dr["CUSTOMERID"]));
            }
            if ((dr["STATUS"] != System.DBNull.Value)) {
                this.Status = ((System.Nullable<int>)(dr["STATUS"]));
            }
            if ((dr["CURRENCYID"] != System.DBNull.Value)) {
                this.Currencyid = ((System.Nullable<int>)(dr["CURRENCYID"]));
            }
            if ((dr["BUYERREF"] != System.DBNull.Value)) {
                this.Buyerref = ((string)(dr["BUYERREF"]));
            }
            if ((dr["SHIPNAME"] != System.DBNull.Value)) {
                this.Shipname = ((string)(dr["SHIPNAME"]));
            }
            if ((dr["RFQ_DEL_DATE"] != System.DBNull.Value)) {
                this.RfqDelDate = ((System.Nullable<System.DateTime>)(dr["RFQ_DEL_DATE"]));
            }
            if ((dr["DELIVERY_DAYS"] != System.DBNull.Value)) {
                this.DeliveryDays = ((System.Nullable<int>)(dr["DELIVERY_DAYS"]));
            }
            if ((dr["DISCOUNT"] != System.DBNull.Value)) {
                this.Discount = ((System.Nullable<float>)(dr["DISCOUNT"]));
            }
            if ((dr["SUPPLIER_REMARKS"] != System.DBNull.Value)) {
                this.SupplierRemarks = ((string)(dr["SUPPLIER_REMARKS"]));
            }
            if ((dr["PAY_TERMS"] != System.DBNull.Value)) {
                this.PayTerms = ((string)(dr["PAY_TERMS"]));
            }
            if ((dr["GENERAL_TERMS"] != System.DBNull.Value)) {
                this.GeneralTerms = ((string)(dr["GENERAL_TERMS"]));
            }
            if ((dr["PROFITMARGIN"] != System.DBNull.Value)) {
                this.Profitmargin = ((System.Nullable<float>)(dr["PROFITMARGIN"]));
            }
            if ((dr["AMOUNT"] != System.DBNull.Value)) {
                this.Amount = ((System.Nullable<float>)(dr["AMOUNT"]));
            }
            if ((dr["FREIGHTAMT"] != System.DBNull.Value)) {
                this.Freightamt = ((System.Nullable<float>)(dr["FREIGHTAMT"]));
            }
            if ((dr["OTHERCOSTS"] != System.DBNull.Value)) {
                this.Othercosts = ((System.Nullable<float>)(dr["OTHERCOSTS"]));
            }
            if ((dr["QUOTE_REFERENCE"] != System.DBNull.Value)) {
                this.QuoteReference = ((string)(dr["QUOTE_REFERENCE"]));
            }
            if ((dr["QUOTE_VALIDITY"] != System.DBNull.Value)) {
                this.QuoteValidity = ((System.Nullable<System.DateTime>)(dr["QUOTE_VALIDITY"]));
            }
            if ((dr["QUOTE_SUBJECT"] != System.DBNull.Value)) {
                this.QuoteSubject = ((string)(dr["QUOTE_SUBJECT"]));
            }
            if ((dr["QUOTATIONID"] != System.DBNull.Value)) {
                this.Quotationid = ((System.Nullable<int>)(dr["QUOTATIONID"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value)) {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value)) {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["UPDATED_BY"] != System.DBNull.Value)) {
                this.UpdatedBy = ((System.Nullable<int>)(dr["UPDATED_BY"]));
            }
            if ((dr["CREATED_BY"] != System.DBNull.Value)) {
                this.CreatedBy = ((System.Nullable<int>)(dr["CREATED_BY"]));
            }
            if ((dr["VESSEL_ETA"] != System.DBNull.Value)) {
                this.VesselEta = ((System.Nullable<System.DateTime>)(dr["VESSEL_ETA"]));
            }
            if ((dr["VESSEL_ARRTIME"] != System.DBNull.Value)) {
                this.VesselArrtime = ((string)(dr["VESSEL_ARRTIME"]));
            }
            if ((dr["ITEM_TOTAL"] != System.DBNull.Value)) {
                this.ItemTotal = ((System.Nullable<float>)(dr["ITEM_TOTAL"]));
            }
        }
        
        public static LesSalesorderCollection GetAll() {
            MetroLesMonitor.Dal.LesSalesorder dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSalesorder();
                System.Data.DataSet ds = dbo.LES_SALESORDER_Select_All();
                LesSalesorderCollection collection = new LesSalesorderCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesSalesorder obj = new LesSalesorder();
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
        
        public static LesSalesorder Load(System.Nullable<int> SALESORDERID) {
            MetroLesMonitor.Dal.LesSalesorder dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSalesorder();
                System.Data.DataSet ds = dbo.LES_SALESORDER_Select_One(SALESORDERID);
                LesSalesorder obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new LesSalesorder();
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
            MetroLesMonitor.Dal.LesSalesorder dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSalesorder();
                System.Data.DataSet ds = dbo.LES_SALESORDER_Select_One(this.Salesorderid);
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
            MetroLesMonitor.Dal.LesSalesorder dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSalesorder();
                dbo.LES_SALESORDER_Insert(this.Salesorderno, this.Customerid, this.Status, this.Currencyid, this.Buyerref, this.Shipname, this.RfqDelDate, this.DeliveryDays, this.Discount, this.SupplierRemarks, this.PayTerms, this.GeneralTerms, this.Profitmargin, this.Amount, this.Freightamt, this.Othercosts, this.QuoteReference, this.QuoteValidity, this.QuoteSubject, this.Quotationid, this.UpdateDate, this.CreatedDate, this.UpdatedBy, this.CreatedBy, this.VesselEta, this.VesselArrtime, this.ItemTotal);
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
            MetroLesMonitor.Dal.LesSalesorder dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSalesorder();
                dbo.LES_SALESORDER_Delete(this.Salesorderid);
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
            MetroLesMonitor.Dal.LesSalesorder dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSalesorder();
                dbo.LES_SALESORDER_Update(this.Salesorderid, this.Salesorderno, this.Customerid, this.Status, this.Currencyid, this.Buyerref, this.Shipname, this.RfqDelDate, this.DeliveryDays, this.Discount, this.SupplierRemarks, this.PayTerms, this.GeneralTerms, this.Profitmargin, this.Amount, this.Freightamt, this.Othercosts, this.QuoteReference, this.QuoteValidity, this.QuoteSubject, this.Quotationid, this.UpdateDate, this.CreatedDate, this.UpdatedBy, this.CreatedBy, this.VesselEta, this.VesselArrtime, this.ItemTotal);
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
