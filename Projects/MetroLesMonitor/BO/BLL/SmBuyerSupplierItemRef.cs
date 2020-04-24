namespace MetroLesMonitor.Bll
{
    
    
    public partial class SmBuyerSupplierItemRef {
        
        private System.Nullable<int> _refid;
        
        private string _reftype;
        
        private string _buyerRef;
        
        private string _supplierRef;
        
        private string _itemDesc;
        
        private string _comments;
        
        private System.Nullable<int> _buyerId;
        
        private System.Nullable<int> _supplierId;

        private System.Nullable<int> _buySuppLinkId;
        
        public virtual System.Nullable<int> Refid {
            get {
                return _refid;
            }
            set {
                _refid = value;
            }
        }
        
        public virtual string Reftype {
            get {
                return _reftype;
            }
            set {
                _reftype = value;
            }
        }
        
        public virtual string BuyerRef {
            get {
                return _buyerRef;
            }
            set {
                _buyerRef = value;
            }
        }
        
        public virtual string SupplierRef {
            get {
                return _supplierRef;
            }
            set {
                _supplierRef = value;
            }
        }
        
        public virtual string ItemDesc {
            get {
                return _itemDesc;
            }
            set {
                _itemDesc = value;
            }
        }
        
        public virtual string Comments {
            get {
                return _comments;
            }
            set {
                _comments = value;
            }
        }
        
        public virtual System.Nullable<int> BuyerId {
            get {
                return _buyerId;
            }
            set {
                _buyerId = value;
            }
        }
        
        public virtual System.Nullable<int> SupplierId {
            get {
                return _supplierId;
            }
            set {
                _supplierId = value;
            }
        }

        public virtual System.Nullable<int> BuyerSuppLinkId
        {
            get
            {
                return _buySuppLinkId;
            }
            set
            {
                _buySuppLinkId = value;
            }
        }
        
        private void Clean() {
            this.Refid = null;
            this.Reftype = string.Empty;
            this.BuyerRef = string.Empty;
            this.SupplierRef = string.Empty;
            this.ItemDesc = string.Empty;
            this.Comments = string.Empty;
            this.BuyerId = null;
            this.SupplierId = null;
            this.BuyerSuppLinkId = null; 
        }

        private void Fill(System.Data.DataRow dr)
        {
            this.Clean();
            if ((dr["REFID"] != System.DBNull.Value))
            {
                this.Refid = ((System.Nullable<int>)(dr["REFID"]));
            }
            if ((dr["REFTYPE"] != System.DBNull.Value))
            {
                this.Reftype = ((string)(dr["REFTYPE"]));
            }
            if ((dr["BUYER_REF"] != System.DBNull.Value))
            {
                this.BuyerRef = ((string)(dr["BUYER_REF"]));
            }
            if ((dr["SUPPLIER_REF"] != System.DBNull.Value))
            {
                this.SupplierRef = ((string)(dr["SUPPLIER_REF"]));
            }
            if ((dr["ITEM_DESC"] != System.DBNull.Value))
            {
                this.ItemDesc = ((string)(dr["ITEM_DESC"]));
            }
            if ((dr["COMMENTS"] != System.DBNull.Value))
            {
                this.Comments = ((string)(dr["COMMENTS"]));
            }
            if ((dr["BUYER_ID"] != System.DBNull.Value))
            {
                this.BuyerId = ((System.Nullable<int>)(dr["BUYER_ID"]));
            }
            if ((dr["SUPPLIER_ID"] != System.DBNull.Value))
            {
                this.SupplierId = ((System.Nullable<int>)(dr["SUPPLIER_ID"]));
            }
            if ((dr["BUYER_SUPPLIER_LINKID"] != System.DBNull.Value))
            {
                this.BuyerSuppLinkId = ((System.Nullable<int>)(dr["BUYER_SUPPLIER_LINKID"]));
            }
        }

        public virtual void Load()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierItemRef dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierItemRef();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_ITEM_REF_Select_One(this.Refid);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        this.Fill(ds.Tables[0].Rows[0]);
                    }
                }
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

        public static SmBuyerSupplierItemRef Load(System.Nullable<int> RefID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierItemRef dbo = null;
            SmBuyerSupplierItemRef obj = new SmBuyerSupplierItemRef();
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierItemRef();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_ITEM_REF_Select_One(RefID);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {                       
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
        
        public static SmBuyerSupplierItemRefCollection GetAll() {
            MetroLesMonitor.Dal.SmBuyerSupplierItemRef dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierItemRef();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_ITEM_REF_Select_All();
                SmBuyerSupplierItemRefCollection collection = new SmBuyerSupplierItemRefCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmBuyerSupplierItemRef obj = new SmBuyerSupplierItemRef();
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
            MetroLesMonitor.Dal.SmBuyerSupplierItemRef dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierItemRef();
                dbo.SM_BUYER_SUPPLIER_ITEM_REF_Insert(this.Refid, this.Reftype, this.BuyerRef, this.SupplierRef, this.ItemDesc, this.Comments, this.BuyerId, this.SupplierId, this.BuyerSuppLinkId); 
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

        public virtual void Insert(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierItemRef dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierItemRef(_dataAccess);
                dbo.SM_BUYER_SUPPLIER_ITEM_REF_Insert(this.Refid, this.Reftype, this.BuyerRef, this.SupplierRef, this.ItemDesc,  this.Comments, this.BuyerId, this.SupplierId, this.BuyerSuppLinkId); 
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public virtual void Delete()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierItemRef dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierItemRef();
                dbo.SM_BUYER_SUPPLIER_ITEM_REF_Delete(this.Refid);
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

        public virtual void Delete(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierItemRef dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierItemRef(_dataAccess);
                dbo.SM_BUYER_SUPPLIER_ITEM_REF_Delete(this.Refid);             
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
            }            
        }

        public virtual void Update()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierItemRef dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierItemRef();
                dbo.SM_BUYER_SUPPLIER_ITEM_REF_Update(this.Refid, this.Reftype, this.BuyerRef, this.SupplierRef, this.ItemDesc, this.Comments);
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

        public virtual void Update(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierItemRef dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierItemRef(_dataAccess);
                dbo.SM_BUYER_SUPPLIER_ITEM_REF_Update(this.Refid, this.Reftype, this.BuyerRef, this.SupplierRef, this.ItemDesc, this.Comments);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public virtual System.Data.DataSet GetItemMapping(int SupplierId, int BuyerId)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierItemRef dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierItemRef();
                System.Data.DataSet ds = dbo.GetAllMapping(SupplierId, BuyerId);
                System.Data.DataSet collection = new System.Data.DataSet();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    collection = ds;
                }
                return collection;
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

        public virtual System.Data.DataSet GetItemMapping(int LinkID) 
        {
            MetroLesMonitor.Dal.SmBuyerSupplierItemRef dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierItemRef();
                System.Data.DataSet ds = dbo.GetAllMapping(LinkID);
                System.Data.DataSet collection = new System.Data.DataSet();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    collection = ds;
                }
                return collection;
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

        public virtual void DeleteByLinkID(int LINKID, Dal.DataAccess _dataAccess) 
        {
            MetroLesMonitor.Dal.SmBuyerSupplierItemRef dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierItemRef(_dataAccess);
                dbo.SM_BUYER_SUPPLIER_ITEM_REF_DeleteByLinkID(LINKID);
            }
            catch (System.Exception)
            { throw; }
        }
   
    }
}
