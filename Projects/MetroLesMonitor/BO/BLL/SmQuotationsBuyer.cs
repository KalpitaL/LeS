namespace MetroLesMonitor.Bll {
    
    
    public partial class SmQuotationsBuyer {
        
        private System.Nullable<int> _quotationid;
        
        private string _docXml;
        
        private string _docid;
        
        private string _docType;
        
        private string _vrno;
        
        private string _buyerVrno;
        
        private string _supplierVrno;
        
        private System.Nullable<int> _quoteAddressid;
        
        private System.Nullable<int> _buyerAddressid;
        
        private System.Nullable<System.DateTime> _rfqSentDate;
        
        private System.Nullable<System.DateTime> _quoteRecvdDate;
        
        private System.Nullable<int> _currencyid;
        
        private string _currCode;
        
        private System.Nullable<float> _quoteAmount;
        
        private System.Nullable<float> _quoteExchrate;
        
        private System.Nullable<float> _itemTotal;
        
        private System.Nullable<float> _othercosts;
        
        private System.Nullable<float> _freightamt;
        
        private System.Nullable<int> _paymentTerms;
        
        private System.Nullable<float> _quoteDiscount;
        
        private System.Nullable<float> _additionalDisc;
        
        private System.Nullable<byte> _addDiscType;
        
        private System.Nullable<System.DateTime> _quoteValidity;
        
        private string _quoteRemarks;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        private System.Nullable<System.DateTime> _deliverytime;
        
        private string _payloadid;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private System.Nullable<int> _siteid;
        
        private System.Nullable<int> _sentBy;
        
        private string _portCode;
        
        private string _portName;
        
        private System.Nullable<System.DateTime> _quoteApproveddate;
        
        private System.Nullable<int> _deliverydays;
        
        private System.Nullable<int> _quoteSubmitBy;
        
        private string _quoteReference;
        
        private System.Nullable<System.DateTime> _replyByDate;
        
        private System.Nullable<System.DateTime> _quoteSubmitDate;
        
        private System.Nullable<int> _vendorStatus;
        
        private System.Nullable<byte> _changedByVendor;
        
        private System.Nullable<System.DateTime> _latedate;
        
        private System.Nullable<System.DateTime> _rfqAckDate;
        
        private System.Nullable<System.DateTime> _poAckDate;
        
        private string _pocReference;
        
        private System.Nullable<System.DateTime> _podate;
        
        private System.Nullable<System.DateTime> _pocDate;
        
        private System.Nullable<int> _pocBy;
        
        private string _buyerRemarks;
        
        private string _vesselName;
        
        private string _vesselIdno;
        
        private string _vesselOwner;
        
        private string _vesselOwnerCode;
        
        private System.Nullable<short> _exported;
        
        private System.Nullable<byte> _version;
        
        private System.Nullable<byte> _rfqExport;
        
        private string _quoteFileRef;
        
        private System.Nullable<int> _printStatus;
        
        private string _quoteFileStamp;
        
        private System.Nullable<System.DateTime> _deliveryPromised;
        
        private string _generalTerms;
        
        private string _payTerms;
        
        private System.Nullable<float> _taxPercnt;
        
        private System.Nullable<int> _quoteVersion;
        
        private System.Nullable<int> _isDeclined;
        
        private string _quoteSubject;
        
        private string _spMasRemark;
        
        private System.Nullable<int> _byrSuppLinkid;
        
        private string _attachment1;
        
        private string _attachment2;
        
        private System.Nullable<float> _allowance;
        
        private System.Nullable<int> _salesorderid;
        
        private string _udf1;
        
        private string _udf2;
        
        private string _udf3;
        
        private string _revisionnumber;
        
        private System.Nullable<int> _linkRecordid;
        
        private string _transportMode;
        
        private string _orderhandling;
        
        private string _ordertype;
        
        private string _originatingrequestno;
        
        private string _shipcomplete;
        
        private string _supplierorgref;
        
        private string _updtype;
        
        private string _contracttype;
        
        private string _orgsystemref;
        
        private System.Nullable<float> _otherCost2;
        
        private System.Nullable<float> _otherCost3;
        
        private string _vesselArrtime;
        
        private System.Nullable<System.DateTime> _vesselEta;
        
        private System.Nullable<int> _quoteStatus;
        
        private SmBuyerSupplierLink _smBuyerSupplierLink;
        
        public virtual System.Nullable<int> Quotationid {
            get {
                return _quotationid;
            }
            set {
                _quotationid = value;
            }
        }
        
        public virtual string DocXml {
            get {
                return _docXml;
            }
            set {
                _docXml = value;
            }
        }
        
        public virtual string Docid {
            get {
                return _docid;
            }
            set {
                _docid = value;
            }
        }
        
        public virtual string DocType {
            get {
                return _docType;
            }
            set {
                _docType = value;
            }
        }
        
        public virtual string Vrno {
            get {
                return _vrno;
            }
            set {
                _vrno = value;
            }
        }
        
        public virtual string BuyerVrno {
            get {
                return _buyerVrno;
            }
            set {
                _buyerVrno = value;
            }
        }
        
        public virtual string SupplierVrno {
            get {
                return _supplierVrno;
            }
            set {
                _supplierVrno = value;
            }
        }
        
        public virtual System.Nullable<int> QuoteAddressid {
            get {
                return _quoteAddressid;
            }
            set {
                _quoteAddressid = value;
            }
        }
        
        public virtual System.Nullable<int> BuyerAddressid {
            get {
                return _buyerAddressid;
            }
            set {
                _buyerAddressid = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> RfqSentDate {
            get {
                return _rfqSentDate;
            }
            set {
                _rfqSentDate = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> QuoteRecvdDate {
            get {
                return _quoteRecvdDate;
            }
            set {
                _quoteRecvdDate = value;
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
        
        public virtual string CurrCode {
            get {
                return _currCode;
            }
            set {
                _currCode = value;
            }
        }
        
        public virtual System.Nullable<float> QuoteAmount {
            get {
                return _quoteAmount;
            }
            set {
                _quoteAmount = value;
            }
        }
        
        public virtual System.Nullable<float> QuoteExchrate {
            get {
                return _quoteExchrate;
            }
            set {
                _quoteExchrate = value;
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
        
        public virtual System.Nullable<float> Othercosts {
            get {
                return _othercosts;
            }
            set {
                _othercosts = value;
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
        
        public virtual System.Nullable<int> PaymentTerms {
            get {
                return _paymentTerms;
            }
            set {
                _paymentTerms = value;
            }
        }
        
        public virtual System.Nullable<float> QuoteDiscount {
            get {
                return _quoteDiscount;
            }
            set {
                _quoteDiscount = value;
            }
        }
        
        public virtual System.Nullable<float> AdditionalDisc {
            get {
                return _additionalDisc;
            }
            set {
                _additionalDisc = value;
            }
        }
        
        public virtual System.Nullable<byte> AddDiscType {
            get {
                return _addDiscType;
            }
            set {
                _addDiscType = value;
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
        
        public virtual string QuoteRemarks {
            get {
                return _quoteRemarks;
            }
            set {
                _quoteRemarks = value;
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
        
        public virtual System.Nullable<System.DateTime> Deliverytime {
            get {
                return _deliverytime;
            }
            set {
                _deliverytime = value;
            }
        }
        
        public virtual string Payloadid {
            get {
                return _payloadid;
            }
            set {
                _payloadid = value;
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
        
        public virtual System.Nullable<int> SentBy {
            get {
                return _sentBy;
            }
            set {
                _sentBy = value;
            }
        }
        
        public virtual string PortCode {
            get {
                return _portCode;
            }
            set {
                _portCode = value;
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
        
        public virtual System.Nullable<System.DateTime> QuoteApproveddate {
            get {
                return _quoteApproveddate;
            }
            set {
                _quoteApproveddate = value;
            }
        }
        
        public virtual System.Nullable<int> Deliverydays {
            get {
                return _deliverydays;
            }
            set {
                _deliverydays = value;
            }
        }
        
        public virtual System.Nullable<int> QuoteSubmitBy {
            get {
                return _quoteSubmitBy;
            }
            set {
                _quoteSubmitBy = value;
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
        
        public virtual System.Nullable<System.DateTime> ReplyByDate {
            get {
                return _replyByDate;
            }
            set {
                _replyByDate = value;
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
        
        public virtual System.Nullable<int> VendorStatus {
            get {
                return _vendorStatus;
            }
            set {
                _vendorStatus = value;
            }
        }
        
        public virtual System.Nullable<byte> ChangedByVendor {
            get {
                return _changedByVendor;
            }
            set {
                _changedByVendor = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> Latedate {
            get {
                return _latedate;
            }
            set {
                _latedate = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> RfqAckDate {
            get {
                return _rfqAckDate;
            }
            set {
                _rfqAckDate = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> PoAckDate {
            get {
                return _poAckDate;
            }
            set {
                _poAckDate = value;
            }
        }
        
        public virtual string PocReference {
            get {
                return _pocReference;
            }
            set {
                _pocReference = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> Podate {
            get {
                return _podate;
            }
            set {
                _podate = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> PocDate {
            get {
                return _pocDate;
            }
            set {
                _pocDate = value;
            }
        }
        
        public virtual System.Nullable<int> PocBy {
            get {
                return _pocBy;
            }
            set {
                _pocBy = value;
            }
        }
        
        public virtual string BuyerRemarks {
            get {
                return _buyerRemarks;
            }
            set {
                _buyerRemarks = value;
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
        
        public virtual string VesselIdno {
            get {
                return _vesselIdno;
            }
            set {
                _vesselIdno = value;
            }
        }
        
        public virtual string VesselOwner {
            get {
                return _vesselOwner;
            }
            set {
                _vesselOwner = value;
            }
        }
        
        public virtual string VesselOwnerCode {
            get {
                return _vesselOwnerCode;
            }
            set {
                _vesselOwnerCode = value;
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
        
        public virtual System.Nullable<byte> Version {
            get {
                return _version;
            }
            set {
                _version = value;
            }
        }
        
        public virtual System.Nullable<byte> RfqExport {
            get {
                return _rfqExport;
            }
            set {
                _rfqExport = value;
            }
        }
        
        public virtual string QuoteFileRef {
            get {
                return _quoteFileRef;
            }
            set {
                _quoteFileRef = value;
            }
        }
        
        public virtual System.Nullable<int> PrintStatus {
            get {
                return _printStatus;
            }
            set {
                _printStatus = value;
            }
        }
        
        public virtual string QuoteFileStamp {
            get {
                return _quoteFileStamp;
            }
            set {
                _quoteFileStamp = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> DeliveryPromised {
            get {
                return _deliveryPromised;
            }
            set {
                _deliveryPromised = value;
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
        
        public virtual string PayTerms {
            get {
                return _payTerms;
            }
            set {
                _payTerms = value;
            }
        }
        
        public virtual System.Nullable<float> TaxPercnt {
            get {
                return _taxPercnt;
            }
            set {
                _taxPercnt = value;
            }
        }
        
        public virtual System.Nullable<int> QuoteVersion {
            get {
                return _quoteVersion;
            }
            set {
                _quoteVersion = value;
            }
        }
        
        public virtual System.Nullable<int> IsDeclined {
            get {
                return _isDeclined;
            }
            set {
                _isDeclined = value;
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
        
        public virtual string SpMasRemark {
            get {
                return _spMasRemark;
            }
            set {
                _spMasRemark = value;
            }
        }
        
        public virtual string Attachment1 {
            get {
                return _attachment1;
            }
            set {
                _attachment1 = value;
            }
        }
        
        public virtual string Attachment2 {
            get {
                return _attachment2;
            }
            set {
                _attachment2 = value;
            }
        }
        
        public virtual System.Nullable<float> Allowance {
            get {
                return _allowance;
            }
            set {
                _allowance = value;
            }
        }
        
        public virtual System.Nullable<int> Salesorderid {
            get {
                return _salesorderid;
            }
            set {
                _salesorderid = value;
            }
        }
        
        public virtual string Udf1 {
            get {
                return _udf1;
            }
            set {
                _udf1 = value;
            }
        }
        
        public virtual string Udf2 {
            get {
                return _udf2;
            }
            set {
                _udf2 = value;
            }
        }
        
        public virtual string Udf3 {
            get {
                return _udf3;
            }
            set {
                _udf3 = value;
            }
        }
        
        public virtual string Revisionnumber {
            get {
                return _revisionnumber;
            }
            set {
                _revisionnumber = value;
            }
        }
        
        public virtual System.Nullable<int> LinkRecordid {
            get {
                return _linkRecordid;
            }
            set {
                _linkRecordid = value;
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
        
        public virtual string Orderhandling {
            get {
                return _orderhandling;
            }
            set {
                _orderhandling = value;
            }
        }
        
        public virtual string Ordertype {
            get {
                return _ordertype;
            }
            set {
                _ordertype = value;
            }
        }
        
        public virtual string Originatingrequestno {
            get {
                return _originatingrequestno;
            }
            set {
                _originatingrequestno = value;
            }
        }
        
        public virtual string Shipcomplete {
            get {
                return _shipcomplete;
            }
            set {
                _shipcomplete = value;
            }
        }
        
        public virtual string Supplierorgref {
            get {
                return _supplierorgref;
            }
            set {
                _supplierorgref = value;
            }
        }
        
        public virtual string Updtype {
            get {
                return _updtype;
            }
            set {
                _updtype = value;
            }
        }
        
        public virtual string Contracttype {
            get {
                return _contracttype;
            }
            set {
                _contracttype = value;
            }
        }
        
        public virtual string Orgsystemref {
            get {
                return _orgsystemref;
            }
            set {
                _orgsystemref = value;
            }
        }
        
        public virtual System.Nullable<float> OtherCost2 {
            get {
                return _otherCost2;
            }
            set {
                _otherCost2 = value;
            }
        }
        
        public virtual System.Nullable<float> OtherCost3 {
            get {
                return _otherCost3;
            }
            set {
                _otherCost3 = value;
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
        
        public virtual System.Nullable<System.DateTime> VesselEta {
            get {
                return _vesselEta;
            }
            set {
                _vesselEta = value;
            }
        }
        
        public virtual System.Nullable<int> QuoteStatus {
            get {
                return _quoteStatus;
            }
            set {
                _quoteStatus = value;
            }
        }
        
        public virtual SmBuyerSupplierLink SmBuyerSupplierLink {
            get {
                if ((this._smBuyerSupplierLink == null)) {
                    this._smBuyerSupplierLink = MetroLesMonitor.Bll.SmBuyerSupplierLink.Load(this._byrSuppLinkid);
                }
                return this._smBuyerSupplierLink;
            }
            set {
                _smBuyerSupplierLink = value;
            }
        }
        
        private void Clean() {
            this.Quotationid = null;
            this.DocXml = string.Empty;
            this.Docid = string.Empty;
            this.DocType = string.Empty;
            this.Vrno = string.Empty;
            this.BuyerVrno = string.Empty;
            this.SupplierVrno = string.Empty;
            this.QuoteAddressid = null;
            this.BuyerAddressid = null;
            this.RfqSentDate = null;
            this.QuoteRecvdDate = null;
            this.Currencyid = null;
            this.CurrCode = string.Empty;
            this.QuoteAmount = null;
            this.QuoteExchrate = null;
            this.ItemTotal = null;
            this.Othercosts = null;
            this.Freightamt = null;
            this.PaymentTerms = null;
            this.QuoteDiscount = null;
            this.AdditionalDisc = null;
            this.AddDiscType = null;
            this.QuoteValidity = null;
            this.QuoteRemarks = string.Empty;
            this.UpdateDate = null;
            this.Deliverytime = null;
            this.Payloadid = string.Empty;
            this.CreatedDate = null;
            this.Siteid = null;
            this.SentBy = null;
            this.PortCode = string.Empty;
            this.PortName = string.Empty;
            this.QuoteApproveddate = null;
            this.Deliverydays = null;
            this.QuoteSubmitBy = null;
            this.QuoteReference = string.Empty;
            this.ReplyByDate = null;
            this.QuoteSubmitDate = null;
            this.VendorStatus = null;
            this.ChangedByVendor = null;
            this.Latedate = null;
            this.RfqAckDate = null;
            this.PoAckDate = null;
            this.PocReference = string.Empty;
            this.Podate = null;
            this.PocDate = null;
            this.PocBy = null;
            this.BuyerRemarks = string.Empty;
            this.VesselName = string.Empty;
            this.VesselIdno = string.Empty;
            this.VesselOwner = string.Empty;
            this.VesselOwnerCode = string.Empty;
            this.Exported = null;
            this.Version = null;
            this.RfqExport = null;
            this.QuoteFileRef = string.Empty;
            this.PrintStatus = null;
            this.QuoteFileStamp = string.Empty;
            this.DeliveryPromised = null;
            this.GeneralTerms = string.Empty;
            this.PayTerms = string.Empty;
            this.TaxPercnt = null;
            this.QuoteVersion = null;
            this.IsDeclined = null;
            this.QuoteSubject = string.Empty;
            this.SpMasRemark = string.Empty;
            this._byrSuppLinkid = null;
            this.Attachment1 = string.Empty;
            this.Attachment2 = string.Empty;
            this.Allowance = null;
            this.Salesorderid = null;
            this.Udf1 = string.Empty;
            this.Udf2 = string.Empty;
            this.Udf3 = string.Empty;
            this.Revisionnumber = string.Empty;
            this.LinkRecordid = null;
            this.TransportMode = string.Empty;
            this.Orderhandling = string.Empty;
            this.Ordertype = string.Empty;
            this.Originatingrequestno = string.Empty;
            this.Shipcomplete = string.Empty;
            this.Supplierorgref = string.Empty;
            this.Updtype = string.Empty;
            this.Contracttype = string.Empty;
            this.Orgsystemref = string.Empty;
            this.OtherCost2 = null;
            this.OtherCost3 = null;
            this.VesselArrtime = string.Empty;
            this.VesselEta = null;
            this.QuoteStatus = null;
            this.SmBuyerSupplierLink = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["QUOTATIONID"] != System.DBNull.Value)) {
                this.Quotationid = ((System.Nullable<int>)(dr["QUOTATIONID"]));
            }
            if ((dr["DOC_XML"] != System.DBNull.Value)) {
                this.DocXml = ((string)(dr["DOC_XML"]));
            }
            if ((dr["DOCID"] != System.DBNull.Value)) {
                this.Docid = ((string)(dr["DOCID"]));
            }
            if ((dr["DOC_TYPE"] != System.DBNull.Value)) {
                this.DocType = ((string)(dr["DOC_TYPE"]));
            }
            if ((dr["VRNO"] != System.DBNull.Value)) {
                this.Vrno = ((string)(dr["VRNO"]));
            }
            if ((dr["BUYER_VRNO"] != System.DBNull.Value)) {
                this.BuyerVrno = ((string)(dr["BUYER_VRNO"]));
            }
            if ((dr["SUPPLIER_VRNO"] != System.DBNull.Value)) {
                this.SupplierVrno = ((string)(dr["SUPPLIER_VRNO"]));
            }
            if ((dr["QUOTE_ADDRESSID"] != System.DBNull.Value)) {
                this.QuoteAddressid = ((System.Nullable<int>)(dr["QUOTE_ADDRESSID"]));
            }
            if ((dr["BUYER_ADDRESSID"] != System.DBNull.Value)) {
                this.BuyerAddressid = ((System.Nullable<int>)(dr["BUYER_ADDRESSID"]));
            }
            if ((dr["RFQ_SENT_DATE"] != System.DBNull.Value)) {
                this.RfqSentDate = ((System.Nullable<System.DateTime>)(dr["RFQ_SENT_DATE"]));
            }
            if ((dr["QUOTE_RECVD_DATE"] != System.DBNull.Value)) {
                this.QuoteRecvdDate = ((System.Nullable<System.DateTime>)(dr["QUOTE_RECVD_DATE"]));
            }
            if ((dr["CURRENCYID"] != System.DBNull.Value)) {
                this.Currencyid = ((System.Nullable<int>)(dr["CURRENCYID"]));
            }
            if ((dr["CURR_CODE"] != System.DBNull.Value)) {
                this.CurrCode = ((string)(dr["CURR_CODE"]));
            }
            if ((dr["QUOTE_AMOUNT"] != System.DBNull.Value)) {
                this.QuoteAmount = ((System.Nullable<float>)(dr["QUOTE_AMOUNT"]));
            }
            if ((dr["QUOTE_EXCHRATE"] != System.DBNull.Value)) {
                this.QuoteExchrate = ((System.Nullable<float>)(dr["QUOTE_EXCHRATE"]));
            }
            if ((dr["ITEM_TOTAL"] != System.DBNull.Value)) {
                this.ItemTotal = ((System.Nullable<float>)(dr["ITEM_TOTAL"]));
            }
            if ((dr["OTHERCOSTS"] != System.DBNull.Value)) {
                this.Othercosts = ((System.Nullable<float>)(dr["OTHERCOSTS"]));
            }
            if ((dr["FREIGHTAMT"] != System.DBNull.Value)) {
                this.Freightamt = ((System.Nullable<float>)(dr["FREIGHTAMT"]));
            }
            if ((dr["PAYMENT_TERMS"] != System.DBNull.Value)) {
                this.PaymentTerms = ((System.Nullable<int>)(dr["PAYMENT_TERMS"]));
            }
            if ((dr["QUOTE_DISCOUNT"] != System.DBNull.Value)) {
                this.QuoteDiscount = ((System.Nullable<float>)(dr["QUOTE_DISCOUNT"]));
            }
            if ((dr["ADDITIONAL_DISC"] != System.DBNull.Value)) {
                this.AdditionalDisc = ((System.Nullable<float>)(dr["ADDITIONAL_DISC"]));
            }
            if ((dr["ADD_DISC_TYPE"] != System.DBNull.Value)) {
                this.AddDiscType = ((System.Nullable<byte>)(dr["ADD_DISC_TYPE"]));
            }
            if ((dr["QUOTE_VALIDITY"] != System.DBNull.Value)) {
                this.QuoteValidity = ((System.Nullable<System.DateTime>)(dr["QUOTE_VALIDITY"]));
            }
            if ((dr["QUOTE_REMARKS"] != System.DBNull.Value)) {
                this.QuoteRemarks = ((string)(dr["QUOTE_REMARKS"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value)) {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
            if ((dr["DELIVERYTIME"] != System.DBNull.Value)) {
                this.Deliverytime = ((System.Nullable<System.DateTime>)(dr["DELIVERYTIME"]));
            }
            if ((dr["PAYLOADID"] != System.DBNull.Value)) {
                this.Payloadid = ((string)(dr["PAYLOADID"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value)) {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["SITEID"] != System.DBNull.Value)) {
                this.Siteid = ((System.Nullable<int>)(dr["SITEID"]));
            }
            if ((dr["SENT_BY"] != System.DBNull.Value)) {
                this.SentBy = ((System.Nullable<int>)(dr["SENT_BY"]));
            }
            if ((dr["PORT_CODE"] != System.DBNull.Value)) {
                this.PortCode = ((string)(dr["PORT_CODE"]));
            }
            if ((dr["PORT_NAME"] != System.DBNull.Value)) {
                this.PortName = ((string)(dr["PORT_NAME"]));
            }
            if ((dr["QUOTE_APPROVEDDATE"] != System.DBNull.Value)) {
                this.QuoteApproveddate = ((System.Nullable<System.DateTime>)(dr["QUOTE_APPROVEDDATE"]));
            }
            if ((dr["DELIVERYDAYS"] != System.DBNull.Value)) {
                this.Deliverydays = ((System.Nullable<int>)(dr["DELIVERYDAYS"]));
            }
            if ((dr["QUOTE_SUBMIT_BY"] != System.DBNull.Value)) {
                this.QuoteSubmitBy = ((System.Nullable<int>)(dr["QUOTE_SUBMIT_BY"]));
            }
            if ((dr["QUOTE_REFERENCE"] != System.DBNull.Value)) {
                this.QuoteReference = ((string)(dr["QUOTE_REFERENCE"]));
            }
            if ((dr["REPLY_BY_DATE"] != System.DBNull.Value)) {
                this.ReplyByDate = ((System.Nullable<System.DateTime>)(dr["REPLY_BY_DATE"]));
            }
            if ((dr["QUOTE_SUBMIT_DATE"] != System.DBNull.Value)) {
                this.QuoteSubmitDate = ((System.Nullable<System.DateTime>)(dr["QUOTE_SUBMIT_DATE"]));
            }
            if ((dr["VENDOR_STATUS"] != System.DBNull.Value)) {
                this.VendorStatus = ((System.Nullable<int>)(dr["VENDOR_STATUS"]));
            }
            if ((dr["CHANGED_BY_VENDOR"] != System.DBNull.Value)) {
                this.ChangedByVendor = ((System.Nullable<byte>)(dr["CHANGED_BY_VENDOR"]));
            }
            if ((dr["LATEDATE"] != System.DBNull.Value)) {
                this.Latedate = ((System.Nullable<System.DateTime>)(dr["LATEDATE"]));
            }
            if ((dr["RFQ_ACK_DATE"] != System.DBNull.Value)) {
                this.RfqAckDate = ((System.Nullable<System.DateTime>)(dr["RFQ_ACK_DATE"]));
            }
            if ((dr["PO_ACK_DATE"] != System.DBNull.Value)) {
                this.PoAckDate = ((System.Nullable<System.DateTime>)(dr["PO_ACK_DATE"]));
            }
            if ((dr["POC_REFERENCE"] != System.DBNull.Value)) {
                this.PocReference = ((string)(dr["POC_REFERENCE"]));
            }
            if ((dr["PODATE"] != System.DBNull.Value)) {
                this.Podate = ((System.Nullable<System.DateTime>)(dr["PODATE"]));
            }
            if ((dr["POC_DATE"] != System.DBNull.Value)) {
                this.PocDate = ((System.Nullable<System.DateTime>)(dr["POC_DATE"]));
            }
            if ((dr["POC_BY"] != System.DBNull.Value)) {
                this.PocBy = ((System.Nullable<int>)(dr["POC_BY"]));
            }
            if ((dr["BUYER_REMARKS"] != System.DBNull.Value)) {
                this.BuyerRemarks = ((string)(dr["BUYER_REMARKS"]));
            }
            if ((dr["VESSEL_NAME"] != System.DBNull.Value)) {
                this.VesselName = ((string)(dr["VESSEL_NAME"]));
            }
            if ((dr["VESSEL_IDNO"] != System.DBNull.Value)) {
                this.VesselIdno = ((string)(dr["VESSEL_IDNO"]));
            }
            if ((dr["VESSEL_OWNER"] != System.DBNull.Value)) {
                this.VesselOwner = ((string)(dr["VESSEL_OWNER"]));
            }
            if ((dr["VESSEL_OWNER_CODE"] != System.DBNull.Value)) {
                this.VesselOwnerCode = ((string)(dr["VESSEL_OWNER_CODE"]));
            }
            if ((dr["EXPORTED"] != System.DBNull.Value)) {
                this.Exported = ((System.Nullable<short>)(dr["EXPORTED"]));
            }
            if ((dr["VERSION"] != System.DBNull.Value)) {
                this.Version = ((System.Nullable<byte>)(dr["VERSION"]));
            }
            if ((dr["RFQ_EXPORT"] != System.DBNull.Value)) {
                this.RfqExport = ((System.Nullable<byte>)(dr["RFQ_EXPORT"]));
            }
            if ((dr["QUOTE_FILE_REF"] != System.DBNull.Value)) {
                this.QuoteFileRef = ((string)(dr["QUOTE_FILE_REF"]));
            }
            if ((dr["PRINT_STATUS"] != System.DBNull.Value)) {
                this.PrintStatus = ((System.Nullable<int>)(dr["PRINT_STATUS"]));
            }
            if ((dr["QUOTE_FILE_STAMP"] != System.DBNull.Value)) {
                this.QuoteFileStamp = ((string)(dr["QUOTE_FILE_STAMP"]));
            }
            if ((dr["DELIVERY_PROMISED"] != System.DBNull.Value)) {
                this.DeliveryPromised = ((System.Nullable<System.DateTime>)(dr["DELIVERY_PROMISED"]));
            }
            if ((dr["GENERAL_TERMS"] != System.DBNull.Value)) {
                this.GeneralTerms = ((string)(dr["GENERAL_TERMS"]));
            }
            if ((dr["PAY_TERMS"] != System.DBNull.Value)) {
                this.PayTerms = ((string)(dr["PAY_TERMS"]));
            }
            if ((dr["TAX_PERCNT"] != System.DBNull.Value)) {
                this.TaxPercnt = ((System.Nullable<float>)(dr["TAX_PERCNT"]));
            }
            if ((dr["QUOTE_VERSION"] != System.DBNull.Value)) {
                this.QuoteVersion = ((System.Nullable<int>)(dr["QUOTE_VERSION"]));
            }
            if ((dr["IS_DECLINED"] != System.DBNull.Value)) {
                this.IsDeclined = ((System.Nullable<int>)(dr["IS_DECLINED"]));
            }
            if ((dr["QUOTE_SUBJECT"] != System.DBNull.Value)) {
                this.QuoteSubject = ((string)(dr["QUOTE_SUBJECT"]));
            }
            if ((dr["SP_MAS_REMARK"] != System.DBNull.Value)) {
                this.SpMasRemark = ((string)(dr["SP_MAS_REMARK"]));
            }
            if ((dr["BYR_SUPP_LINKID"] != System.DBNull.Value)) {
                this._byrSuppLinkid = ((System.Nullable<int>)(dr["BYR_SUPP_LINKID"]));
            }
            if ((dr["ATTACHMENT1"] != System.DBNull.Value)) {
                this.Attachment1 = ((string)(dr["ATTACHMENT1"]));
            }
            if ((dr["ATTACHMENT2"] != System.DBNull.Value)) {
                this.Attachment2 = ((string)(dr["ATTACHMENT2"]));
            }
            if ((dr["ALLOWANCE"] != System.DBNull.Value)) {
                this.Allowance = ((System.Nullable<float>)(dr["ALLOWANCE"]));
            }
            if ((dr["SALESORDERID"] != System.DBNull.Value)) {
                this.Salesorderid = ((System.Nullable<int>)(dr["SALESORDERID"]));
            }
            if ((dr["UDF1"] != System.DBNull.Value)) {
                this.Udf1 = ((string)(dr["UDF1"]));
            }
            if ((dr["UDF2"] != System.DBNull.Value)) {
                this.Udf2 = ((string)(dr["UDF2"]));
            }
            if ((dr["UDF3"] != System.DBNull.Value)) {
                this.Udf3 = ((string)(dr["UDF3"]));
            }
            if ((dr["RevisionNumber"] != System.DBNull.Value)) {
                this.Revisionnumber = ((string)(dr["RevisionNumber"]));
            }
            if ((dr["LINK_RECORDID"] != System.DBNull.Value)) {
                this.LinkRecordid = ((System.Nullable<int>)(dr["LINK_RECORDID"]));
            }
            if ((dr["TRANSPORT_MODE"] != System.DBNull.Value)) {
                this.TransportMode = ((string)(dr["TRANSPORT_MODE"]));
            }
            if ((dr["OrderHandling"] != System.DBNull.Value)) {
                this.Orderhandling = ((string)(dr["OrderHandling"]));
            }
            if ((dr["OrderType"] != System.DBNull.Value)) {
                this.Ordertype = ((string)(dr["OrderType"]));
            }
            if ((dr["OriginatingRequestNo"] != System.DBNull.Value)) {
                this.Originatingrequestno = ((string)(dr["OriginatingRequestNo"]));
            }
            if ((dr["ShipComplete"] != System.DBNull.Value)) {
                this.Shipcomplete = ((string)(dr["ShipComplete"]));
            }
            if ((dr["SupplierORGRef"] != System.DBNull.Value)) {
                this.Supplierorgref = ((string)(dr["SupplierORGRef"]));
            }
            if ((dr["UpdType"] != System.DBNull.Value)) {
                this.Updtype = ((string)(dr["UpdType"]));
            }
            if ((dr["ContractType"] != System.DBNull.Value)) {
                this.Contracttype = ((string)(dr["ContractType"]));
            }
            if ((dr["OrgSystemRef"] != System.DBNull.Value)) {
                this.Orgsystemref = ((string)(dr["OrgSystemRef"]));
            }
            if ((dr["OTHER_COST2"] != System.DBNull.Value)) {
                this.OtherCost2 = ((System.Nullable<float>)(dr["OTHER_COST2"]));
            }
            if ((dr["OTHER_COST3"] != System.DBNull.Value)) {
                this.OtherCost3 = ((System.Nullable<float>)(dr["OTHER_COST3"]));
            }
            if ((dr["VESSEL_ARRTIME"] != System.DBNull.Value)) {
                this.VesselArrtime = ((string)(dr["VESSEL_ARRTIME"]));
            }
            if ((dr["VESSEL_ETA"] != System.DBNull.Value)) {
                this.VesselEta = ((System.Nullable<System.DateTime>)(dr["VESSEL_ETA"]));
            }
            if ((dr["QUOTE_STATUS"] != System.DBNull.Value)) {
                this.QuoteStatus = ((System.Nullable<int>)(dr["QUOTE_STATUS"]));
            }
        }
        
        public static SmQuotationsBuyerCollection Select_SM_QUOTATIONS_BUYERs_By_BYR_SUPP_LINKID(System.Nullable<int> BYR_SUPP_LINKID) {
            MetroLesMonitor.Dal.SmQuotationsBuyer dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmQuotationsBuyer();
                System.Data.DataSet ds = dbo.Select_SM_QUOTATIONS_BUYERs_By_BYR_SUPP_LINKID(BYR_SUPP_LINKID);
                SmQuotationsBuyerCollection collection = new SmQuotationsBuyerCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmQuotationsBuyer obj = new SmQuotationsBuyer();
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
        
        public static SmQuotationsBuyerCollection GetAll() {
            MetroLesMonitor.Dal.SmQuotationsBuyer dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmQuotationsBuyer();
                System.Data.DataSet ds = dbo.SM_QUOTATIONS_BUYER_Select_All();
                SmQuotationsBuyerCollection collection = new SmQuotationsBuyerCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmQuotationsBuyer obj = new SmQuotationsBuyer();
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
        
        public static SmQuotationsBuyer Load(System.Nullable<int> QUOTATIONID) {
            MetroLesMonitor.Dal.SmQuotationsBuyer dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmQuotationsBuyer();
                System.Data.DataSet ds = dbo.SM_QUOTATIONS_BUYER_Select_One(QUOTATIONID);
                SmQuotationsBuyer obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmQuotationsBuyer();
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
            MetroLesMonitor.Dal.SmQuotationsBuyer dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmQuotationsBuyer();
                System.Data.DataSet ds = dbo.SM_QUOTATIONS_BUYER_Select_One(this.Quotationid);
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
            MetroLesMonitor.Dal.SmQuotationsBuyer dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmQuotationsBuyer();
                dbo.SM_QUOTATIONS_BUYER_Insert(this.DocXml, this.Docid, this.DocType, this.Vrno, this.BuyerVrno, this.SupplierVrno, this.QuoteAddressid, this.BuyerAddressid, this.RfqSentDate, this.QuoteRecvdDate, this.Currencyid, this.CurrCode, this.QuoteAmount, this.QuoteExchrate, this.ItemTotal, this.Othercosts, this.Freightamt, this.PaymentTerms, this.QuoteDiscount, this.AdditionalDisc, this.AddDiscType, this.QuoteValidity, this.QuoteRemarks, this.UpdateDate, this.Deliverytime, this.Payloadid, this.CreatedDate, this.Siteid, this.SentBy, this.PortCode, this.PortName, this.QuoteApproveddate, this.Deliverydays, this.QuoteSubmitBy, this.QuoteReference, this.ReplyByDate, this.QuoteSubmitDate, this.VendorStatus, this.ChangedByVendor, this.Latedate, this.RfqAckDate, this.PoAckDate, this.PocReference, this.Podate, this.PocDate, this.PocBy, this.BuyerRemarks, this.VesselName, this.VesselIdno, this.VesselOwner, this.VesselOwnerCode, this.Exported, this.Version, this.RfqExport, this.QuoteFileRef, this.PrintStatus, this.QuoteFileStamp, this.DeliveryPromised, this.GeneralTerms, this.PayTerms, this.TaxPercnt, this.QuoteVersion, this.IsDeclined, this.QuoteSubject, this.SpMasRemark, this._byrSuppLinkid, this.Attachment1, this.Attachment2, this.Allowance, this.Salesorderid, this.Udf1, this.Udf2, this.Udf3, this.Revisionnumber, this.LinkRecordid, this.TransportMode, this.Orderhandling, this.Ordertype, this.Originatingrequestno, this.Shipcomplete, this.Supplierorgref, this.Updtype, this.Contracttype, this.Orgsystemref, this.OtherCost2, this.OtherCost3, this.VesselArrtime, this.VesselEta, this.QuoteStatus);
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
            MetroLesMonitor.Dal.SmQuotationsBuyer dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmQuotationsBuyer();
                dbo.SM_QUOTATIONS_BUYER_Delete(this.Quotationid);
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
            MetroLesMonitor.Dal.SmQuotationsBuyer dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmQuotationsBuyer();
                dbo.SM_QUOTATIONS_BUYER_Update(this.Quotationid, this.DocXml, this.Docid, this.DocType, this.Vrno, this.BuyerVrno, this.SupplierVrno, this.QuoteAddressid, this.BuyerAddressid, this.RfqSentDate, this.QuoteRecvdDate, this.Currencyid, this.CurrCode, this.QuoteAmount, this.QuoteExchrate, this.ItemTotal, this.Othercosts, this.Freightamt, this.PaymentTerms, this.QuoteDiscount, this.AdditionalDisc, this.AddDiscType, this.QuoteValidity, this.QuoteRemarks, this.UpdateDate, this.Deliverytime, this.Payloadid, this.CreatedDate, this.Siteid, this.SentBy, this.PortCode, this.PortName, this.QuoteApproveddate, this.Deliverydays, this.QuoteSubmitBy, this.QuoteReference, this.ReplyByDate, this.QuoteSubmitDate, this.VendorStatus, this.ChangedByVendor, this.Latedate, this.RfqAckDate, this.PoAckDate, this.PocReference, this.Podate, this.PocDate, this.PocBy, this.BuyerRemarks, this.VesselName, this.VesselIdno, this.VesselOwner, this.VesselOwnerCode, this.Exported, this.Version, this.RfqExport, this.QuoteFileRef, this.PrintStatus, this.QuoteFileStamp, this.DeliveryPromised, this.GeneralTerms, this.PayTerms, this.TaxPercnt, this.QuoteVersion, this.IsDeclined, this.QuoteSubject, this.SpMasRemark, this._byrSuppLinkid, this.Attachment1, this.Attachment2, this.Allowance, this.Salesorderid, this.Udf1, this.Udf2, this.Udf3, this.Revisionnumber, this.LinkRecordid, this.TransportMode, this.Orderhandling, this.Ordertype, this.Originatingrequestno, this.Shipcomplete, this.Supplierorgref, this.Updtype, this.Contracttype, this.Orgsystemref, this.OtherCost2, this.OtherCost3, this.VesselArrtime, this.VesselEta, this.QuoteStatus);
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
