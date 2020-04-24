namespace MetroLesMonitor.Bll {
    
    
    public partial class MxmlDocHeader {
        
        private System.Nullable<System.Guid> _mxmldocheaderid;
        
        private System.Nullable<System.Guid> _mxmldocid;
        
        private string _docPayloadid;
        
        private System.Nullable<System.DateTime> _reqDeliveryDate;
        
        private string _quoteid;
        
        private string _originatingsystemref;
        
        private System.Nullable<System.DateTime> _quoteDate;
        
        private System.Nullable<System.DateTime> _replyByDate;
        
        private System.Nullable<System.DateTime> _quoteExpiryDate;
        
        private System.Nullable<System.DateTime> _orderDate;
        
        private string _messagenumber;
        
        private string _deliveryPort;
        
        private string _portName;
        
        private string _deliveryPier;
        
        private string _vesselId;
        
        private string _vesselName;
        
        private System.Nullable<System.DateTime> _arrivaldate;
        
        private System.Nullable<System.DateTime> _departuredate;
        
        private string _docCurrency;
        
        private System.Nullable<float> _docTotalAmount;
        
        private string _docComments;
        
        private System.Nullable<int> _exported;
        
        private string _quoteComments;
        
        private System.Nullable<System.DateTime> _quoteSubmitDate;
        
        private string _poMessagenumber;
        
        private string _poPayloadid;
        
        private string _supplierquoteid;
        
        private string _paymentterms;
        
        public virtual System.Nullable<System.Guid> Mxmldocheaderid {
            get {
                return _mxmldocheaderid;
            }
            set {
                _mxmldocheaderid = value;
            }
        }
        
        public virtual System.Nullable<System.Guid> Mxmldocid {
            get {
                return _mxmldocid;
            }
            set {
                _mxmldocid = value;
            }
        }
        
        public virtual string DocPayloadid {
            get {
                return _docPayloadid;
            }
            set {
                _docPayloadid = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> ReqDeliveryDate {
            get {
                return _reqDeliveryDate;
            }
            set {
                _reqDeliveryDate = value;
            }
        }
        
        public virtual string Quoteid {
            get {
                return _quoteid;
            }
            set {
                _quoteid = value;
            }
        }
        
        public virtual string Originatingsystemref {
            get {
                return _originatingsystemref;
            }
            set {
                _originatingsystemref = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> QuoteDate {
            get {
                return _quoteDate;
            }
            set {
                _quoteDate = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> ReplyByDate {
            get {
                return _replyByDate;
            }
            set {
                _replyByDate = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> QuoteExpiryDate {
            get {
                return _quoteExpiryDate;
            }
            set {
                _quoteExpiryDate = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> OrderDate {
            get {
                return _orderDate;
            }
            set {
                _orderDate = value;
            }
        }
        
        public virtual string Messagenumber {
            get {
                return _messagenumber;
            }
            set {
                _messagenumber = value;
            }
        }
        
        public virtual string DeliveryPort {
            get {
                return _deliveryPort;
            }
            set {
                _deliveryPort = value;
            }
        }
        
        public virtual string PortName {
            get {
                return _portName;
            }
            set {
                _portName = value;
            }
        }
        
        public virtual string DeliveryPier {
            get {
                return _deliveryPier;
            }
            set {
                _deliveryPier = value;
            }
        }
        
        public virtual string VesselId {
            get {
                return _vesselId;
            }
            set {
                _vesselId = value;
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
        
        public virtual System.Nullable<System.DateTime> Arrivaldate {
            get {
                return _arrivaldate;
            }
            set {
                _arrivaldate = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> Departuredate {
            get {
                return _departuredate;
            }
            set {
                _departuredate = value;
            }
        }
        
        public virtual string DocCurrency {
            get {
                return _docCurrency;
            }
            set {
                _docCurrency = value;
            }
        }
        
        public virtual System.Nullable<float> DocTotalAmount {
            get {
                return _docTotalAmount;
            }
            set {
                _docTotalAmount = value;
            }
        }
        
        public virtual string DocComments {
            get {
                return _docComments;
            }
            set {
                _docComments = value;
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
        
        public virtual string QuoteComments {
            get {
                return _quoteComments;
            }
            set {
                _quoteComments = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> QuoteSubmitDate {
            get {
                return _quoteSubmitDate;
            }
            set {
                _quoteSubmitDate = value;
            }
        }
        
        public virtual string PoMessagenumber {
            get {
                return _poMessagenumber;
            }
            set {
                _poMessagenumber = value;
            }
        }
        
        public virtual string PoPayloadid {
            get {
                return _poPayloadid;
            }
            set {
                _poPayloadid = value;
            }
        }
        
        public virtual string Supplierquoteid {
            get {
                return _supplierquoteid;
            }
            set {
                _supplierquoteid = value;
            }
        }
        
        public virtual string Paymentterms {
            get {
                return _paymentterms;
            }
            set {
                _paymentterms = value;
            }
        }
        
        private void Clean() {
            this.Mxmldocheaderid = null;
            this.Mxmldocid = null;
            this.DocPayloadid = string.Empty;
            this.ReqDeliveryDate = null;
            this.Quoteid = string.Empty;
            this.Originatingsystemref = string.Empty;
            this.QuoteDate = null;
            this.ReplyByDate = null;
            this.QuoteExpiryDate = null;
            this.OrderDate = null;
            this.Messagenumber = string.Empty;
            this.DeliveryPort = string.Empty;
            this.PortName = string.Empty;
            this.DeliveryPier = string.Empty;
            this.VesselId = string.Empty;
            this.VesselName = string.Empty;
            this.Arrivaldate = null;
            this.Departuredate = null;
            this.DocCurrency = string.Empty;
            this.DocTotalAmount = null;
            this.DocComments = string.Empty;
            this.Exported = null;
            this.QuoteComments = string.Empty;
            this.QuoteSubmitDate = null;
            this.PoMessagenumber = string.Empty;
            this.PoPayloadid = string.Empty;
            this.Supplierquoteid = string.Empty;
            this.Paymentterms = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["MXMLDOCHEADERID"] != System.DBNull.Value)) {
                this.Mxmldocheaderid = ((System.Nullable<System.Guid>)(dr["MXMLDOCHEADERID"]));
            }
            if ((dr["MXMLDOCID"] != System.DBNull.Value)) {
                this.Mxmldocid = ((System.Nullable<System.Guid>)(dr["MXMLDOCID"]));
            }
            if ((dr["DOC_PAYLOADID"] != System.DBNull.Value)) {
                this.DocPayloadid = ((string)(dr["DOC_PAYLOADID"]));
            }
            if ((dr["REQ_DELIVERY_DATE"] != System.DBNull.Value)) {
                this.ReqDeliveryDate = ((System.Nullable<System.DateTime>)(dr["REQ_DELIVERY_DATE"]));
            }
            if ((dr["QUOTEID"] != System.DBNull.Value)) {
                this.Quoteid = ((string)(dr["QUOTEID"]));
            }
            if ((dr["ORIGINATINGSYSTEMREF"] != System.DBNull.Value)) {
                this.Originatingsystemref = ((string)(dr["ORIGINATINGSYSTEMREF"]));
            }
            if ((dr["QUOTE_DATE"] != System.DBNull.Value)) {
                this.QuoteDate = ((System.Nullable<System.DateTime>)(dr["QUOTE_DATE"]));
            }
            if ((dr["REPLY_BY_DATE"] != System.DBNull.Value)) {
                this.ReplyByDate = ((System.Nullable<System.DateTime>)(dr["REPLY_BY_DATE"]));
            }
            if ((dr["QUOTE_EXPIRY_DATE"] != System.DBNull.Value)) {
                this.QuoteExpiryDate = ((System.Nullable<System.DateTime>)(dr["QUOTE_EXPIRY_DATE"]));
            }
            if ((dr["ORDER_DATE"] != System.DBNull.Value)) {
                this.OrderDate = ((System.Nullable<System.DateTime>)(dr["ORDER_DATE"]));
            }
            if ((dr["MESSAGENUMBER"] != System.DBNull.Value)) {
                this.Messagenumber = ((string)(dr["MESSAGENUMBER"]));
            }
            if ((dr["DELIVERY_PORT"] != System.DBNull.Value)) {
                this.DeliveryPort = ((string)(dr["DELIVERY_PORT"]));
            }
            if ((dr["PORT_NAME"] != System.DBNull.Value)) {
                this.PortName = ((string)(dr["PORT_NAME"]));
            }
            if ((dr["DELIVERY_PIER"] != System.DBNull.Value)) {
                this.DeliveryPier = ((string)(dr["DELIVERY_PIER"]));
            }
            if ((dr["VESSEL_ID"] != System.DBNull.Value)) {
                this.VesselId = ((string)(dr["VESSEL_ID"]));
            }
            if ((dr["VESSEL_NAME"] != System.DBNull.Value)) {
                this.VesselName = ((string)(dr["VESSEL_NAME"]));
            }
            if ((dr["ARRIVALDATE"] != System.DBNull.Value)) {
                this.Arrivaldate = ((System.Nullable<System.DateTime>)(dr["ARRIVALDATE"]));
            }
            if ((dr["DEPARTUREDATE"] != System.DBNull.Value)) {
                this.Departuredate = ((System.Nullable<System.DateTime>)(dr["DEPARTUREDATE"]));
            }
            if ((dr["DOC_CURRENCY"] != System.DBNull.Value)) {
                this.DocCurrency = ((string)(dr["DOC_CURRENCY"]));
            }
            if ((dr["DOC_TOTAL_AMOUNT"] != System.DBNull.Value)) {
                this.DocTotalAmount = ((System.Nullable<float>)(dr["DOC_TOTAL_AMOUNT"]));
            }
            if ((dr["DOC_COMMENTS"] != System.DBNull.Value)) {
                this.DocComments = ((string)(dr["DOC_COMMENTS"]));
            }
            if ((dr["EXPORTED"] != System.DBNull.Value)) {
                this.Exported = ((System.Nullable<int>)(dr["EXPORTED"]));
            }
            if ((dr["QUOTE_COMMENTS"] != System.DBNull.Value)) {
                this.QuoteComments = ((string)(dr["QUOTE_COMMENTS"]));
            }
            if ((dr["QUOTE_SUBMIT_DATE"] != System.DBNull.Value)) {
                this.QuoteSubmitDate = ((System.Nullable<System.DateTime>)(dr["QUOTE_SUBMIT_DATE"]));
            }
            if ((dr["PO_MESSAGENUMBER"] != System.DBNull.Value)) {
                this.PoMessagenumber = ((string)(dr["PO_MESSAGENUMBER"]));
            }
            if ((dr["PO_PAYLOADID"] != System.DBNull.Value)) {
                this.PoPayloadid = ((string)(dr["PO_PAYLOADID"]));
            }
            if ((dr["SupplierQuoteID"] != System.DBNull.Value)) {
                this.Supplierquoteid = ((string)(dr["SupplierQuoteID"]));
            }
            if ((dr["PaymentTerms"] != System.DBNull.Value)) {
                this.Paymentterms = ((string)(dr["PaymentTerms"]));
            }
        }
        
        public static MxmlDocHeaderCollection GetAll() {
            MetroLesMonitor.Dal.MxmlDocHeader dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlDocHeader();
                System.Data.DataSet ds = dbo.MXML_DOC_HEADER_Select_All();
                MxmlDocHeaderCollection collection = new MxmlDocHeaderCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        MxmlDocHeader obj = new MxmlDocHeader();
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
        
        public static MxmlDocHeader Load(System.Nullable<System.Guid> MXMLDOCHEADERID) {
            MetroLesMonitor.Dal.MxmlDocHeader dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlDocHeader();
                System.Data.DataSet ds = dbo.MXML_DOC_HEADER_Select_One(MXMLDOCHEADERID);
                MxmlDocHeader obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new MxmlDocHeader();
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
            MetroLesMonitor.Dal.MxmlDocHeader dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlDocHeader();
                System.Data.DataSet ds = dbo.MXML_DOC_HEADER_Select_One(this.Mxmldocheaderid);
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
            MetroLesMonitor.Dal.MxmlDocHeader dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlDocHeader();
                dbo.MXML_DOC_HEADER_Insert(this.Mxmldocid, this.DocPayloadid, this.ReqDeliveryDate, this.Quoteid, this.Originatingsystemref, this.QuoteDate, this.ReplyByDate, this.QuoteExpiryDate, this.OrderDate, this.Messagenumber, this.DeliveryPort, this.PortName, this.DeliveryPier, this.VesselId, this.VesselName, this.Arrivaldate, this.Departuredate, this.DocCurrency, this.DocTotalAmount, this.DocComments, this.Exported, this.QuoteComments, this.QuoteSubmitDate, this.PoMessagenumber, this.PoPayloadid, this.Supplierquoteid, this.Paymentterms);
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
            MetroLesMonitor.Dal.MxmlDocHeader dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlDocHeader();
                dbo.MXML_DOC_HEADER_Delete(this.Mxmldocheaderid);
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
            MetroLesMonitor.Dal.MxmlDocHeader dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.MxmlDocHeader();
                dbo.MXML_DOC_HEADER_Update(this.Mxmldocheaderid, this.Mxmldocid, this.DocPayloadid, this.ReqDeliveryDate, this.Quoteid, this.Originatingsystemref, this.QuoteDate, this.ReplyByDate, this.QuoteExpiryDate, this.OrderDate, this.Messagenumber, this.DeliveryPort, this.PortName, this.DeliveryPier, this.VesselId, this.VesselName, this.Arrivaldate, this.Departuredate, this.DocCurrency, this.DocTotalAmount, this.DocComments, this.Exported, this.QuoteComments, this.QuoteSubmitDate, this.PoMessagenumber, this.PoPayloadid, this.Supplierquoteid, this.Paymentterms);
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
