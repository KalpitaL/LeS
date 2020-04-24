namespace MetroLesMonitor.Bll {
    
    
    public partial class SmShipmentDocuments {
        
        private System.Nullable<int> _shipmentId;
        
        private string _docid;
        
        private string _docType;
        
        private string _senderCode;
        
        private string _receiverCode;
        
        private string _senderName;
        
        private string _receiverName;
        
        private string _supplierName;
        
        private string _poNo;
        
        private string _shipmentNo;
        
        private string _vesselName;
        
        private string _vesselImono;
        
        private string _portCode;
        
        private string _portName;
        
        private System.Nullable<float> _poTotal;
        
        private System.Nullable<float> _smTotal;
        
        private string _currCode;
        
        private System.Nullable<System.DateTime> _shipmentDate;
        
        private System.Nullable<System.DateTime> _poDate;
        
        private System.Nullable<System.DateTime> _reqDelDate;
        
        private System.Nullable<System.DateTime> _actDelDate;
        
        private string _location;
        
        private string _hdrRemarks;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private string _weightUom;
        
        private string _weightValue;
        
        private string _transportMode;
        
        private System.Nullable<int> _exported;
        
        private string _msgRef;
        
        private SmShipmentItemsCollection _smShipmentItemsCollection;
        
        public virtual System.Nullable<int> ShipmentId {
            get {
                return _shipmentId;
            }
            set {
                _shipmentId = value;
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
        
        public virtual string SenderCode {
            get {
                return _senderCode;
            }
            set {
                _senderCode = value;
            }
        }
        
        public virtual string ReceiverCode {
            get {
                return _receiverCode;
            }
            set {
                _receiverCode = value;
            }
        }
        
        public virtual string SenderName {
            get {
                return _senderName;
            }
            set {
                _senderName = value;
            }
        }
        
        public virtual string ReceiverName {
            get {
                return _receiverName;
            }
            set {
                _receiverName = value;
            }
        }
        
        public virtual string SupplierName {
            get {
                return _supplierName;
            }
            set {
                _supplierName = value;
            }
        }
        
        public virtual string PoNo {
            get {
                return _poNo;
            }
            set {
                _poNo = value;
            }
        }
        
        public virtual string ShipmentNo {
            get {
                return _shipmentNo;
            }
            set {
                _shipmentNo = value;
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
        
        public virtual string VesselImono {
            get {
                return _vesselImono;
            }
            set {
                _vesselImono = value;
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
        
        public virtual System.Nullable<float> PoTotal {
            get {
                return _poTotal;
            }
            set {
                _poTotal = value;
            }
        }
        
        public virtual System.Nullable<float> SmTotal {
            get {
                return _smTotal;
            }
            set {
                _smTotal = value;
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
        
        public virtual System.Nullable<System.DateTime> ShipmentDate {
            get {
                return _shipmentDate;
            }
            set {
                _shipmentDate = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> PoDate {
            get {
                return _poDate;
            }
            set {
                _poDate = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> ReqDelDate {
            get {
                return _reqDelDate;
            }
            set {
                _reqDelDate = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> ActDelDate {
            get {
                return _actDelDate;
            }
            set {
                _actDelDate = value;
            }
        }
        
        public virtual string Location {
            get {
                return _location;
            }
            set {
                _location = value;
            }
        }
        
        public virtual string HdrRemarks {
            get {
                return _hdrRemarks;
            }
            set {
                _hdrRemarks = value;
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
        
        public virtual string WeightUom {
            get {
                return _weightUom;
            }
            set {
                _weightUom = value;
            }
        }
        
        public virtual string WeightValue {
            get {
                return _weightValue;
            }
            set {
                _weightValue = value;
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
        
        public virtual System.Nullable<int> Exported {
            get {
                return _exported;
            }
            set {
                _exported = value;
            }
        }
        
        public virtual string MsgRef {
            get {
                return _msgRef;
            }
            set {
                _msgRef = value;
            }
        }
        
        public virtual SmShipmentItemsCollection SmShipmentItemsCollection {
            get {
                if ((this._smShipmentItemsCollection == null)) {
                    _smShipmentItemsCollection = MetroLesMonitor.Bll.SmShipmentItems.Select_SM_SHIPMENT_ITEMSs_By_SHIPMENT_ID(this.ShipmentId);
                }
                return this._smShipmentItemsCollection;
            }
        }
        
        private void Clean() {
            this.ShipmentId = null;
            this.Docid = string.Empty;
            this.DocType = string.Empty;
            this.SenderCode = string.Empty;
            this.ReceiverCode = string.Empty;
            this.SenderName = string.Empty;
            this.ReceiverName = string.Empty;
            this.SupplierName = string.Empty;
            this.PoNo = string.Empty;
            this.ShipmentNo = string.Empty;
            this.VesselName = string.Empty;
            this.VesselImono = string.Empty;
            this.PortCode = string.Empty;
            this.PortName = string.Empty;
            this.PoTotal = null;
            this.SmTotal = null;
            this.CurrCode = string.Empty;
            this.ShipmentDate = null;
            this.PoDate = null;
            this.ReqDelDate = null;
            this.ActDelDate = null;
            this.Location = string.Empty;
            this.HdrRemarks = string.Empty;
            this.UpdateDate = null;
            this.CreatedDate = null;
            this.WeightUom = string.Empty;
            this.WeightValue = string.Empty;
            this.TransportMode = string.Empty;
            this.Exported = null;
            this.MsgRef = string.Empty;
            this._smShipmentItemsCollection = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["SHIPMENT_ID"] != System.DBNull.Value)) {
                this.ShipmentId = ((System.Nullable<int>)(dr["SHIPMENT_ID"]));
            }
            if ((dr["DOCID"] != System.DBNull.Value)) {
                this.Docid = ((string)(dr["DOCID"]));
            }
            if ((dr["DOC_TYPE"] != System.DBNull.Value)) {
                this.DocType = ((string)(dr["DOC_TYPE"]));
            }
            if ((dr["SENDER_CODE"] != System.DBNull.Value)) {
                this.SenderCode = ((string)(dr["SENDER_CODE"]));
            }
            if ((dr["RECEIVER_CODE"] != System.DBNull.Value)) {
                this.ReceiverCode = ((string)(dr["RECEIVER_CODE"]));
            }
            if ((dr["SENDER_NAME"] != System.DBNull.Value)) {
                this.SenderName = ((string)(dr["SENDER_NAME"]));
            }
            if ((dr["RECEIVER_NAME"] != System.DBNull.Value)) {
                this.ReceiverName = ((string)(dr["RECEIVER_NAME"]));
            }
            if ((dr["SUPPLIER_NAME"] != System.DBNull.Value)) {
                this.SupplierName = ((string)(dr["SUPPLIER_NAME"]));
            }
            if ((dr["PO_NO"] != System.DBNull.Value)) {
                this.PoNo = ((string)(dr["PO_NO"]));
            }
            if ((dr["SHIPMENT_NO"] != System.DBNull.Value)) {
                this.ShipmentNo = ((string)(dr["SHIPMENT_NO"]));
            }
            if ((dr["VESSEL_NAME"] != System.DBNull.Value)) {
                this.VesselName = ((string)(dr["VESSEL_NAME"]));
            }
            if ((dr["VESSEL_IMONO"] != System.DBNull.Value)) {
                this.VesselImono = ((string)(dr["VESSEL_IMONO"]));
            }
            if ((dr["PORT_CODE"] != System.DBNull.Value)) {
                this.PortCode = ((string)(dr["PORT_CODE"]));
            }
            if ((dr["PORT_NAME"] != System.DBNull.Value)) {
                this.PortName = ((string)(dr["PORT_NAME"]));
            }
            if ((dr["PO_TOTAL"] != System.DBNull.Value)) {
                this.PoTotal = ((System.Nullable<float>)(dr["PO_TOTAL"]));
            }
            if ((dr["SM_TOTAL"] != System.DBNull.Value)) {
                this.SmTotal = ((System.Nullable<float>)(dr["SM_TOTAL"]));
            }
            if ((dr["CURR_CODE"] != System.DBNull.Value)) {
                this.CurrCode = ((string)(dr["CURR_CODE"]));
            }
            if ((dr["SHIPMENT_DATE"] != System.DBNull.Value)) {
                this.ShipmentDate = ((System.Nullable<System.DateTime>)(dr["SHIPMENT_DATE"]));
            }
            if ((dr["PO_DATE"] != System.DBNull.Value)) {
                this.PoDate = ((System.Nullable<System.DateTime>)(dr["PO_DATE"]));
            }
            if ((dr["REQ_DEL_DATE"] != System.DBNull.Value)) {
                this.ReqDelDate = ((System.Nullable<System.DateTime>)(dr["REQ_DEL_DATE"]));
            }
            if ((dr["ACT_DEL_DATE"] != System.DBNull.Value)) {
                this.ActDelDate = ((System.Nullable<System.DateTime>)(dr["ACT_DEL_DATE"]));
            }
            if ((dr["LOCATION"] != System.DBNull.Value)) {
                this.Location = ((string)(dr["LOCATION"]));
            }
            if ((dr["HDR_REMARKS"] != System.DBNull.Value)) {
                this.HdrRemarks = ((string)(dr["HDR_REMARKS"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value)) {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value)) {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["WEIGHT_UOM"] != System.DBNull.Value)) {
                this.WeightUom = ((string)(dr["WEIGHT_UOM"]));
            }
            if ((dr["WEIGHT_VALUE"] != System.DBNull.Value)) {
                this.WeightValue = ((string)(dr["WEIGHT_VALUE"]));
            }
            if ((dr["TRANSPORT_MODE"] != System.DBNull.Value)) {
                this.TransportMode = ((string)(dr["TRANSPORT_MODE"]));
            }
            if ((dr["EXPORTED"] != System.DBNull.Value)) {
                this.Exported = ((System.Nullable<int>)(dr["EXPORTED"]));
            }
            if ((dr["MSG_REF"] != System.DBNull.Value)) {
                this.MsgRef = ((string)(dr["MSG_REF"]));
            }
        }
        
        public static SmShipmentDocumentsCollection GetAll() {
            MetroLesMonitor.Dal.SmShipmentDocuments dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmShipmentDocuments();
                System.Data.DataSet ds = dbo.SM_SHIPMENT_DOCUMENTS_Select_All();
                SmShipmentDocumentsCollection collection = new SmShipmentDocumentsCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmShipmentDocuments obj = new SmShipmentDocuments();
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
        
        public static SmShipmentDocuments Load(System.Nullable<int> SHIPMENT_ID) {
            MetroLesMonitor.Dal.SmShipmentDocuments dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmShipmentDocuments();
                System.Data.DataSet ds = dbo.SM_SHIPMENT_DOCUMENTS_Select_One(SHIPMENT_ID);
                SmShipmentDocuments obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmShipmentDocuments();
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
            MetroLesMonitor.Dal.SmShipmentDocuments dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmShipmentDocuments();
                System.Data.DataSet ds = dbo.SM_SHIPMENT_DOCUMENTS_Select_One(this.ShipmentId);
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
            MetroLesMonitor.Dal.SmShipmentDocuments dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmShipmentDocuments();
                dbo.SM_SHIPMENT_DOCUMENTS_Insert(this.Docid, this.DocType, this.SenderCode, this.ReceiverCode, this.SenderName, this.ReceiverName, this.SupplierName, this.PoNo, this.ShipmentNo, this.VesselName, this.VesselImono, this.PortCode, this.PortName, this.PoTotal, this.SmTotal, this.CurrCode, this.ShipmentDate, this.PoDate, this.ReqDelDate, this.ActDelDate, this.Location, this.HdrRemarks, this.UpdateDate, this.CreatedDate, this.WeightUom, this.WeightValue, this.TransportMode, this.Exported, this.MsgRef);
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
            MetroLesMonitor.Dal.SmShipmentDocuments dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmShipmentDocuments();
                dbo.SM_SHIPMENT_DOCUMENTS_Delete(this.ShipmentId);
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
            MetroLesMonitor.Dal.SmShipmentDocuments dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmShipmentDocuments();
                dbo.SM_SHIPMENT_DOCUMENTS_Update(this.ShipmentId, this.Docid, this.DocType, this.SenderCode, this.ReceiverCode, this.SenderName, this.ReceiverName, this.SupplierName, this.PoNo, this.ShipmentNo, this.VesselName, this.VesselImono, this.PortCode, this.PortName, this.PoTotal, this.SmTotal, this.CurrCode, this.ShipmentDate, this.PoDate, this.ReqDelDate, this.ActDelDate, this.Location, this.HdrRemarks, this.UpdateDate, this.CreatedDate, this.WeightUom, this.WeightValue, this.TransportMode, this.Exported, this.MsgRef);
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
