namespace MetroLesMonitor.Bll {
    
    
    public partial class SmReqnInvoiceItem {
        
        private System.Nullable<int> _invoiceItemid;
        
        private System.Nullable<int> _reqninvoiceid;
        
        private System.Nullable<short> _itemno;
        
        private string _partname;
        
        private string _drawingno;
        
        private string _posno;
        
        private string _refno;
        
        private string _partunit;
        
        private string _itemRemarks;
        
        private System.Nullable<float> _invoiceQty;
        
        private System.Nullable<float> _invoicePrice;
        
        private System.Nullable<float> _discount;
        
        private System.Nullable<float> _exchrate;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private System.Nullable<byte> _finalDelivery;
        
        private string _unitCode;
        
        private System.Nullable<short> _exported;
        
        public virtual System.Nullable<int> InvoiceItemid {
            get {
                return _invoiceItemid;
            }
            set {
                _invoiceItemid = value;
            }
        }
        
        public virtual System.Nullable<int> Reqninvoiceid {
            get {
                return _reqninvoiceid;
            }
            set {
                _reqninvoiceid = value;
            }
        }
        
        public virtual System.Nullable<short> Itemno {
            get {
                return _itemno;
            }
            set {
                _itemno = value;
            }
        }
        
        public virtual string Partname {
            get {
                return _partname;
            }
            set {
                _partname = value;
            }
        }
        
        public virtual string Drawingno {
            get {
                return _drawingno;
            }
            set {
                _drawingno = value;
            }
        }
        
        public virtual string Posno {
            get {
                return _posno;
            }
            set {
                _posno = value;
            }
        }
        
        public virtual string Refno {
            get {
                return _refno;
            }
            set {
                _refno = value;
            }
        }
        
        public virtual string Partunit {
            get {
                return _partunit;
            }
            set {
                _partunit = value;
            }
        }
        
        public virtual string ItemRemarks {
            get {
                return _itemRemarks;
            }
            set {
                _itemRemarks = value;
            }
        }
        
        public virtual System.Nullable<float> InvoiceQty {
            get {
                return _invoiceQty;
            }
            set {
                _invoiceQty = value;
            }
        }
        
        public virtual System.Nullable<float> InvoicePrice {
            get {
                return _invoicePrice;
            }
            set {
                _invoicePrice = value;
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
        
        public virtual System.Nullable<float> Exchrate {
            get {
                return _exchrate;
            }
            set {
                _exchrate = value;
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
        
        public virtual System.Nullable<byte> FinalDelivery {
            get {
                return _finalDelivery;
            }
            set {
                _finalDelivery = value;
            }
        }
        
        public virtual string UnitCode {
            get {
                return _unitCode;
            }
            set {
                _unitCode = value;
            }
        }
        
        public virtual System.Nullable<short> Exported {
            get {
                return _exported;
            }
            set {
                _exported = value;
            }
        }
        
        private void Clean() {
            this.InvoiceItemid = null;
            this.Reqninvoiceid = null;
            this.Itemno = null;
            this.Partname = string.Empty;
            this.Drawingno = string.Empty;
            this.Posno = string.Empty;
            this.Refno = string.Empty;
            this.Partunit = string.Empty;
            this.ItemRemarks = string.Empty;
            this.InvoiceQty = null;
            this.InvoicePrice = null;
            this.Discount = null;
            this.Exchrate = null;
            this.UpdateDate = null;
            this.CreatedDate = null;
            this.FinalDelivery = null;
            this.UnitCode = string.Empty;
            this.Exported = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["INVOICE_ITEMID"] != System.DBNull.Value)) {
                this.InvoiceItemid = ((System.Nullable<int>)(dr["INVOICE_ITEMID"]));
            }
            if ((dr["REQNINVOICEID"] != System.DBNull.Value)) {
                this.Reqninvoiceid = ((System.Nullable<int>)(dr["REQNINVOICEID"]));
            }
            if ((dr["ITEMNO"] != System.DBNull.Value)) {
                this.Itemno = ((System.Nullable<short>)(dr["ITEMNO"]));
            }
            if ((dr["PARTNAME"] != System.DBNull.Value)) {
                this.Partname = ((string)(dr["PARTNAME"]));
            }
            if ((dr["DRAWINGNO"] != System.DBNull.Value)) {
                this.Drawingno = ((string)(dr["DRAWINGNO"]));
            }
            if ((dr["POSNO"] != System.DBNull.Value)) {
                this.Posno = ((string)(dr["POSNO"]));
            }
            if ((dr["REFNO"] != System.DBNull.Value)) {
                this.Refno = ((string)(dr["REFNO"]));
            }
            if ((dr["PARTUNIT"] != System.DBNull.Value)) {
                this.Partunit = ((string)(dr["PARTUNIT"]));
            }
            if ((dr["ITEM_REMARKS"] != System.DBNull.Value)) {
                this.ItemRemarks = ((string)(dr["ITEM_REMARKS"]));
            }
            if ((dr["INVOICE_QTY"] != System.DBNull.Value)) {
                this.InvoiceQty = ((System.Nullable<float>)(dr["INVOICE_QTY"]));
            }
            if ((dr["INVOICE_PRICE"] != System.DBNull.Value)) {
                this.InvoicePrice = ((System.Nullable<float>)(dr["INVOICE_PRICE"]));
            }
            if ((dr["DISCOUNT"] != System.DBNull.Value)) {
                this.Discount = ((System.Nullable<float>)(dr["DISCOUNT"]));
            }
            if ((dr["EXCHRATE"] != System.DBNull.Value)) {
                this.Exchrate = ((System.Nullable<float>)(dr["EXCHRATE"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value)) {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value)) {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["FINAL_DELIVERY"] != System.DBNull.Value)) {
                this.FinalDelivery = ((System.Nullable<byte>)(dr["FINAL_DELIVERY"]));
            }
            if ((dr["UNIT_CODE"] != System.DBNull.Value)) {
                this.UnitCode = ((string)(dr["UNIT_CODE"]));
            }
            if ((dr["EXPORTED"] != System.DBNull.Value)) {
                this.Exported = ((System.Nullable<short>)(dr["EXPORTED"]));
            }
        }
        
        public static SmReqnInvoiceItemCollection GetAll() {
            MetroLesMonitor.Dal.SmReqnInvoiceItem dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmReqnInvoiceItem();
                System.Data.DataSet ds = dbo.SM_REQN_INVOICE_ITEM_Select_All();
                SmReqnInvoiceItemCollection collection = new SmReqnInvoiceItemCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmReqnInvoiceItem obj = new SmReqnInvoiceItem();
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
            MetroLesMonitor.Dal.SmReqnInvoiceItem dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmReqnInvoiceItem();
                dbo.SM_REQN_INVOICE_ITEM_Insert(this.InvoiceItemid, this.Reqninvoiceid, this.Itemno, this.Partname, this.Drawingno, this.Posno, this.Refno, this.Partunit, this.ItemRemarks, this.InvoiceQty, this.InvoicePrice, this.Discount, this.Exchrate, this.UpdateDate, this.CreatedDate, this.FinalDelivery, this.UnitCode, this.Exported);
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
