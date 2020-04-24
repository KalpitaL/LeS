namespace MetroLesMonitor.Bll {
    
    
    public partial class SmBuyerSupplierGroupFlow {
        
        private System.Nullable<int> _groupFlowid;
        
        private System.Nullable<int> _groupId;
        
        private System.Nullable<int> _rfq;
        
        private System.Nullable<int> _quote;
        
        private System.Nullable<int> _po;
        
        private System.Nullable<int> _poc;
        
        private System.Nullable<int> _rfqEndState;
        
        private System.Nullable<int> _quoteEndState;
        
        private System.Nullable<int> _poEndState;
        
        private System.Nullable<int> _pocEndState;
        
        private System.Nullable<int> _quoteExportMarker;
        
        private System.Nullable<int> _quoteBuyerExportMarker;
        
        private System.Nullable<int> _pocExportMarker;
        
        private System.Nullable<int> _pocBuyerExportMarker;
        
        public virtual System.Nullable<int> GroupFlowid {
            get {
                return _groupFlowid;
            }
            set {
                _groupFlowid = value;
            }
        }
        
        public virtual System.Nullable<int> GroupId {
            get {
                return _groupId;
            }
            set {
                _groupId = value;
            }
        }
        
        public virtual System.Nullable<int> Rfq {
            get {
                return _rfq;
            }
            set {
                _rfq = value;
            }
        }
        
        public virtual System.Nullable<int> Quote {
            get {
                return _quote;
            }
            set {
                _quote = value;
            }
        }
        
        public virtual System.Nullable<int> Po {
            get {
                return _po;
            }
            set {
                _po = value;
            }
        }
        
        public virtual System.Nullable<int> Poc {
            get {
                return _poc;
            }
            set {
                _poc = value;
            }
        }
        
        public virtual System.Nullable<int> RfqEndState {
            get {
                return _rfqEndState;
            }
            set {
                _rfqEndState = value;
            }
        }
        
        public virtual System.Nullable<int> QuoteEndState {
            get {
                return _quoteEndState;
            }
            set {
                _quoteEndState = value;
            }
        }
        
        public virtual System.Nullable<int> PoEndState {
            get {
                return _poEndState;
            }
            set {
                _poEndState = value;
            }
        }
        
        public virtual System.Nullable<int> PocEndState {
            get {
                return _pocEndState;
            }
            set {
                _pocEndState = value;
            }
        }
        
        public virtual System.Nullable<int> QuoteExportMarker {
            get {
                return _quoteExportMarker;
            }
            set {
                _quoteExportMarker = value;
            }
        }
        
        public virtual System.Nullable<int> QuoteBuyerExportMarker {
            get {
                return _quoteBuyerExportMarker;
            }
            set {
                _quoteBuyerExportMarker = value;
            }
        }
        
        public virtual System.Nullable<int> PocExportMarker {
            get {
                return _pocExportMarker;
            }
            set {
                _pocExportMarker = value;
            }
        }
        
        public virtual System.Nullable<int> PocBuyerExportMarker {
            get {
                return _pocBuyerExportMarker;
            }
            set {
                _pocBuyerExportMarker = value;
            }
        }
        
        private void Clean() {
            this.GroupFlowid = null;
            this.GroupId = null;
            this.Rfq = null;
            this.Quote = null;
            this.Po = null;
            this.Poc = null;
            this.RfqEndState = null;
            this.QuoteEndState = null;
            this.PoEndState = null;
            this.PocEndState = null;
            this.QuoteExportMarker = null;
            this.QuoteBuyerExportMarker = null;
            this.PocExportMarker = null;
            this.PocBuyerExportMarker = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["GROUP_FLOWID"] != System.DBNull.Value)) {
                this.GroupFlowid = ((System.Nullable<int>)(dr["GROUP_FLOWID"]));
            }
            if ((dr["GROUP_ID"] != System.DBNull.Value)) {
                this.GroupId = ((System.Nullable<int>)(dr["GROUP_ID"]));
            }
            if ((dr["RFQ"] != System.DBNull.Value)) {
                this.Rfq = ((System.Nullable<int>)(dr["RFQ"]));
            }
            if ((dr["QUOTE"] != System.DBNull.Value)) {
                this.Quote = ((System.Nullable<int>)(dr["QUOTE"]));
            }
            if ((dr["PO"] != System.DBNull.Value)) {
                this.Po = ((System.Nullable<int>)(dr["PO"]));
            }
            if ((dr["POC"] != System.DBNull.Value)) {
                this.Poc = ((System.Nullable<int>)(dr["POC"]));
            }
            if ((dr["RFQ_END_STATE"] != System.DBNull.Value)) {
                this.RfqEndState = ((System.Nullable<int>)(dr["RFQ_END_STATE"]));
            }
            if ((dr["QUOTE_END_STATE"] != System.DBNull.Value)) {
                this.QuoteEndState = ((System.Nullable<int>)(dr["QUOTE_END_STATE"]));
            }
            if ((dr["PO_END_STATE"] != System.DBNull.Value)) {
                this.PoEndState = ((System.Nullable<int>)(dr["PO_END_STATE"]));
            }
            if ((dr["POC_END_STATE"] != System.DBNull.Value)) {
                this.PocEndState = ((System.Nullable<int>)(dr["POC_END_STATE"]));
            }
            if ((dr["QUOTE_EXPORT_MARKER"] != System.DBNull.Value)) {
                this.QuoteExportMarker = ((System.Nullable<int>)(dr["QUOTE_EXPORT_MARKER"]));
            }
            if ((dr["QUOTE_BUYER_EXPORT_MARKER"] != System.DBNull.Value)) {
                this.QuoteBuyerExportMarker = ((System.Nullable<int>)(dr["QUOTE_BUYER_EXPORT_MARKER"]));
            }
            if ((dr["POC_EXPORT_MARKER"] != System.DBNull.Value)) {
                this.PocExportMarker = ((System.Nullable<int>)(dr["POC_EXPORT_MARKER"]));
            }
            if ((dr["POC_BUYER_EXPORT_MARKER"] != System.DBNull.Value)) {
                this.PocBuyerExportMarker = ((System.Nullable<int>)(dr["POC_BUYER_EXPORT_MARKER"]));
            }
        }
        
        public static SmBuyerSupplierGroupFlowCollection GetAll() {
            MetroLesMonitor.Dal.SmBuyerSupplierGroupFlow dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroupFlow();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_GROUP_FLOW_Select_All();
                SmBuyerSupplierGroupFlowCollection collection = new SmBuyerSupplierGroupFlowCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmBuyerSupplierGroupFlow obj = new SmBuyerSupplierGroupFlow();
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
        
        public static SmBuyerSupplierGroupFlow Load(System.Nullable<int> GROUP_FLOWID) {
            MetroLesMonitor.Dal.SmBuyerSupplierGroupFlow dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroupFlow();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_GROUP_FLOW_Select_One(GROUP_FLOWID);
                SmBuyerSupplierGroupFlow obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmBuyerSupplierGroupFlow();
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
            MetroLesMonitor.Dal.SmBuyerSupplierGroupFlow dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroupFlow();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_GROUP_FLOW_Select_One(this.GroupFlowid);
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
            MetroLesMonitor.Dal.SmBuyerSupplierGroupFlow dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroupFlow();
                dbo.SM_BUYER_SUPPLIER_GROUP_FLOW_Insert(this.GroupFlowid,this.GroupId, this.Rfq, this.Quote, this.Po, this.Poc, this.RfqEndState, this.QuoteEndState, this.PoEndState, this.PocEndState, this.QuoteExportMarker, this.QuoteBuyerExportMarker, this.PocExportMarker, this.PocBuyerExportMarker);
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
            MetroLesMonitor.Dal.SmBuyerSupplierGroupFlow dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroupFlow();
                dbo.SM_BUYER_SUPPLIER_GROUP_FLOW_Delete(this.GroupFlowid);
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
            MetroLesMonitor.Dal.SmBuyerSupplierGroupFlow dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroupFlow();
                dbo.SM_BUYER_SUPPLIER_GROUP_FLOW_Update(this.GroupFlowid, this.GroupId, this.Rfq, this.Quote, this.Po, this.Poc, this.RfqEndState, this.QuoteEndState, this.PoEndState, this.PocEndState, this.QuoteExportMarker, this.QuoteBuyerExportMarker, this.PocExportMarker, this.PocBuyerExportMarker);
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

        public static SmBuyerSupplierGroupFlow LoadByGroup(System.Nullable<int> GROUP_ID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierGroupFlow dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroupFlow();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_GROUP_FLOW_Select_By_GroupID(GROUP_ID);
                SmBuyerSupplierGroupFlow obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmBuyerSupplierGroupFlow();
                        obj.Fill(ds.Tables[0].Rows[0]);
                    }
                }
                return obj;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if ((dbo != null))
                {
                    dbo.Dispose();
                }
            }
        }

        public virtual System.Nullable<int> Insert(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierGroupFlow dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroupFlow(_dataAccess);
                return dbo.SM_BUYER_SUPPLIER_GROUP_FLOW_Insert(this.GroupFlowid, this.GroupId, this.Rfq, this.Quote, this.Po, this.Poc, this.RfqEndState, this.QuoteEndState, this.PoEndState, this.PocEndState, this.QuoteExportMarker, this.QuoteBuyerExportMarker, this.PocExportMarker, this.PocBuyerExportMarker);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public virtual void Delete(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierGroups dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroups(_dataAccess);
                dbo.SM_BUYER_SUPPLIER_GROUPS_Delete(this.GroupId);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public virtual void Update(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierGroupFlow dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroupFlow(_dataAccess);
                dbo.SM_BUYER_SUPPLIER_GROUP_FLOW_Update(this.GroupFlowid, this.GroupId, this.Rfq, this.Quote, this.Po, this.Poc, this.RfqEndState, this.QuoteEndState, this.PoEndState, this.PocEndState, this.QuoteExportMarker, this.QuoteBuyerExportMarker, this.PocExportMarker, this.PocBuyerExportMarker);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

    }
}
