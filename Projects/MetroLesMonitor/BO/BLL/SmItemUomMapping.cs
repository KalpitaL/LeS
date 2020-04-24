using System.Data;
namespace MetroLesMonitor.Bll
{
    
    
    public partial class SmItemUomMapping {
        
        private System.Nullable<int> _itemUomMapid;
        
        private System.Nullable<int> _buyerId;
        
        private System.Nullable<int> _supplierId;
        
        private string _buyerItemUom;
        
        private string _supplierItemUom;
        
        public virtual System.Nullable<int> ItemUomMapid {
            get {
                return _itemUomMapid;
            }
            set {
                _itemUomMapid = value;
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
        
        public virtual string BuyerItemUom {
            get {
                return _buyerItemUom;
            }
            set {
                _buyerItemUom = value;
            }
        }
        
        public virtual string SupplierItemUom {
            get {
                return _supplierItemUom;
            }
            set {
                _supplierItemUom = value;
            }
        }
        
        private void Clean() {
            this.ItemUomMapid = null;
            this.BuyerId = null;
            this.SupplierId = null;
            this.BuyerItemUom = string.Empty;
            this.SupplierItemUom = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["ITEM_UOM_MAPID"] != System.DBNull.Value)) {
                this.ItemUomMapid = ((System.Nullable<int>)(dr["ITEM_UOM_MAPID"]));
            }
            if ((dr["BUYER_ID"] != System.DBNull.Value)) {
                this.BuyerId = ((System.Nullable<int>)(dr["BUYER_ID"]));
            }
            if ((dr["SUPPLIER_ID"] != System.DBNull.Value)) {
                this.SupplierId = ((System.Nullable<int>)(dr["SUPPLIER_ID"]));
            }
            if ((dr["BUYER_ITEM_UOM"] != System.DBNull.Value)) {
                this.BuyerItemUom = ((string)(dr["BUYER_ITEM_UOM"]));
            }
            if ((dr["SUPPLIER_ITEM_UOM"] != System.DBNull.Value)) {
                this.SupplierItemUom = ((string)(dr["SUPPLIER_ITEM_UOM"]));
            }
        }
        
        public static SmItemUomMappingCollection GetAll() {
            MetroLesMonitor.Dal.SmItemUomMapping dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmItemUomMapping();
                System.Data.DataSet ds = dbo.SM_ITEM_UOM_MAPPING_Select_All();
                SmItemUomMappingCollection collection = new SmItemUomMappingCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmItemUomMapping obj = new SmItemUomMapping();
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
        
        public static SmItemUomMapping Load(System.Nullable<int> ITEM_UOM_MAPID) {
            MetroLesMonitor.Dal.SmItemUomMapping dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmItemUomMapping();
                System.Data.DataSet ds = dbo.SM_ITEM_UOM_MAPPING_Select_One(ITEM_UOM_MAPID);
                SmItemUomMapping obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmItemUomMapping();
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
            MetroLesMonitor.Dal.SmItemUomMapping dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmItemUomMapping();
                System.Data.DataSet ds = dbo.SM_ITEM_UOM_MAPPING_Select_One(this.ItemUomMapid);
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

        public DataSet GetItemUOM_Mapping_By_BuyerID_SupplierID(int SupplierID, int BuyerID)
        {
            DataSet Ds = new DataSet();
            MetroLesMonitor.Dal.SmItemUomMapping dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmItemUomMapping();
                System.Data.DataSet ds = dbo.SM_ITEM_UOM_MAPPING_Select_By_BuyerID_SupplierId( SupplierID,  BuyerID);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    Ds = ds;
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
            return Ds;
        }
        
        public virtual void Insert() {
            MetroLesMonitor.Dal.SmItemUomMapping dbo = null;
            try {
                this.ItemUomMapid = GetNewItemUOMID();
                dbo = new MetroLesMonitor.Dal.SmItemUomMapping();
                dbo.SM_ITEM_UOM_MAPPING_Insert(this.ItemUomMapid, this.BuyerId, this.SupplierId, this.BuyerItemUom, this.SupplierItemUom);
            }
            catch (System.Exception ) {
                throw;
            }
            finally {
                if ((dbo != null)) {
                   
                }
            }
        }

        public virtual void Insert(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmItemUomMapping dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmItemUomMapping(_dataAccess);
                dbo.SM_ITEM_UOM_MAPPING_Insert(this.ItemUomMapid, this.BuyerId, this.SupplierId, this.BuyerItemUom, this.SupplierItemUom);
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if ((dbo != null))
                {
                    
                }
            }
        }
        
        public virtual void Delete() {
            MetroLesMonitor.Dal.SmItemUomMapping dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmItemUomMapping();
                dbo.SM_ITEM_UOM_MAPPING_Delete(this.ItemUomMapid);
            }
            catch (System.Exception ) {
                throw;
            }
            finally {
                if ((dbo != null)) {
                   
                }
            }
        }

        public virtual void Delete(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmItemUomMapping dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmItemUomMapping(_dataAccess);
                dbo.SM_ITEM_UOM_MAPPING_Delete(this.ItemUomMapid);
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if ((dbo != null))
                {
                   
                }
            }
        }
        
        public virtual void Update() {
            MetroLesMonitor.Dal.SmItemUomMapping dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmItemUomMapping();
                dbo.SM_ITEM_UOM_MAPPING_Update(this.ItemUomMapid, this.BuyerId, this.SupplierId, this.BuyerItemUom, this.SupplierItemUom);
            }
            catch (System.Exception ) {
                throw;
            }
            finally {
                if ((dbo != null)) {
                   
                }
            }
        }

        public virtual void Update(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmItemUomMapping dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmItemUomMapping(_dataAccess);
                dbo.SM_ITEM_UOM_MAPPING_Update(this.ItemUomMapid, this.BuyerId, this.SupplierId, this.BuyerItemUom, this.SupplierItemUom);
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                
            }
        }

        public int GetNewItemUOMID()
        {
            int _itemID = 0;
            MetroLesMonitor.Dal.SmItemUomMapping dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmItemUomMapping();
                int _Val = dbo.Get_Last_Item_UOM_ID();
                _itemID = _Val + 1;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
               
            }
            return _itemID;
        }

        public virtual void DeleteUOM_MAPPING_ByBuySuppID(int SupplierID , int BuyerID, Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmItemUomMapping dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmItemUomMapping(_dataAccess);
                dbo.SM_BUYER_SUPPLIER_ITEM_UOM_DeleteBySupplierIDBuyerID(BuyerID,SupplierID);
            }
            catch (System.Exception)
            { throw; }
        }
    }
}
