namespace MetroLesMonitor.Bll
{
    public partial class SmBuyerSupplierGroups
    {
        private System.Nullable<int> _groupId;

        private string _groupCode;

        private string _groupDesc;

        private string _buyerFormat;

        private string _buyerExportFormat;

        private string _supplierFormat;

        private string _suppExportFormat;

        public virtual System.Nullable<int> GroupId
        {
            get
            {
                return _groupId;
            }
            set
            {
                _groupId = value;
            }
        }

        public virtual string GroupCode
        {
            get
            {
                return _groupCode;
            }
            set
            {
                _groupCode = value;
            }
        }

        public virtual string GroupDesc
        {
            get
            {
                return _groupDesc;
            }
            set
            {
                _groupDesc = value;
            }
        }

        public virtual string BuyerFormat
        {
            get
            {
                return _buyerFormat;
            }
            set
            {
                _buyerFormat = value;
            }
        }

        public virtual string BuyerExportFormat
        {
            get
            {
                return _buyerExportFormat;
            }
            set
            {
                _buyerExportFormat = value;
            }
        }

        public virtual string SupplierExportFormat
        {
            get
            {
                return _suppExportFormat;
            }
            set
            {
                _suppExportFormat = value;
            }
        }


        public virtual string SupplierFormat
        {
            get
            {
                return _supplierFormat;
            }
            set
            {
                _supplierFormat = value;
            }
        }
        
        private void Clean()
        {
            this.GroupId = null;
            this.GroupCode = string.Empty;
            this.GroupDesc = string.Empty;
        }

        private void Fill(System.Data.DataRow dr)
        {
            this.Clean();
            if ((dr["GROUP_ID"] != System.DBNull.Value))
            {
                this.GroupId = ((System.Nullable<int>)(dr["GROUP_ID"]));
            }
            if ((dr["GROUP_CODE"] != System.DBNull.Value))
            {
                this.GroupCode = ((string)(dr["GROUP_CODE"]));
            }
            if ((dr["GROUP_DESC"] != System.DBNull.Value))
            {
                this.GroupDesc = ((string)(dr["GROUP_DESC"]));
            }
            if ((dr["BUYER_FORMAT"] != System.DBNull.Value))
            {
                this.BuyerFormat = ((string)(dr["BUYER_FORMAT"]));
            }
            if ((dr["SUPPLIER_FORMAT"] != System.DBNull.Value))
            {
                this.SupplierFormat = ((string)(dr["SUPPLIER_FORMAT"]));
            }
            if ((dr["BUYER_EXPORT_FORMAT"] != System.DBNull.Value))
            {
                this.BuyerExportFormat = ((string)(dr["BUYER_EXPORT_FORMAT"]));
            }
        }

        public static SmBuyerSupplierGroupsCollection GetAll()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierGroups dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroups();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_GROUPS_Select_All();
                SmBuyerSupplierGroupsCollection collection = new SmBuyerSupplierGroupsCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmBuyerSupplierGroups obj = new SmBuyerSupplierGroups();
                        obj.Fill(ds.Tables[0].Rows[i]);
                        if ((obj != null))
                        {
                            collection.Add(obj);
                        }
                    }
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

        public static System.Data.DataSet GetAllGroups()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierGroups dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroups();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_GROUPS_Select_All();
                return ds;
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

        public static System.Data.DataSet GetAllExcelGroups()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierGroups dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroups();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_GROUPS_Select_All_Excel();
                return ds;
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

        public static System.Data.DataSet GetExcelGroupsOnly()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierGroups dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroups();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_GROUPS_Select_Excel_Only();
                return ds;
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

        public static System.Data.DataSet GetPDFGroupsOnly()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierGroups dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroups();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_GROUPS_Select_PDF_Only();
                return ds;
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

        public static SmBuyerSupplierGroups Load(System.Nullable<int> GROUP_ID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierGroups dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroups();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_GROUPS_Select_One(GROUP_ID);
                SmBuyerSupplierGroups obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmBuyerSupplierGroups();
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

        public virtual void Load()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierGroups dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroups();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_GROUPS_Select_One(this.GroupId);
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

        public virtual void Insert()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierGroups dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroups();
                dbo.SM_BUYER_SUPPLIER_GROUPS_Insert(this.GroupId, this.GroupCode, this.GroupDesc, this.BuyerFormat, this.SupplierFormat, this.BuyerExportFormat, this.SupplierExportFormat);
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

        public virtual void Delete()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierGroups dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroups();
                dbo.SM_BUYER_SUPPLIER_GROUPS_Delete(this.GroupId);
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

        public virtual void Update()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierGroups dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroups();
                dbo.SM_BUYER_SUPPLIER_GROUPS_Update(this.GroupId, this.GroupCode, this.GroupDesc, this.BuyerFormat, this.SupplierFormat, this.BuyerExportFormat, this.SupplierExportFormat);
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
            MetroLesMonitor.Dal.SmBuyerSupplierGroups dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroups(_dataAccess);
                return dbo.SM_BUYER_SUPPLIER_GROUPS_Insert(this.GroupId, this.GroupCode, this.GroupDesc, this.BuyerFormat, this.SupplierFormat, this.BuyerExportFormat, this.SupplierExportFormat);
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
            MetroLesMonitor.Dal.SmBuyerSupplierGroups dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroups(_dataAccess);
                dbo.SM_BUYER_SUPPLIER_GROUPS_Update(this.GroupId, this.GroupCode, this.GroupDesc, this.BuyerFormat, this.SupplierFormat, this.BuyerExportFormat, this.SupplierExportFormat);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public static int ValidateGroup(string GroupCode, int GroupID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierGroups dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroups();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_GROUPS_Select_One(GroupCode);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        SmBuyerSupplierGroups _group = new SmBuyerSupplierGroups();
                        _group.Fill(ds.Tables[0].Rows[0]);

                        if (GroupID == _group.GroupId) return 0;
                        else return ds.Tables[0].Rows.Count;
                    }
                    else return 0;
                }
                else return 0;
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

        public int Get_GroupId_BY_GroupCode(string GroupCode)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierGroups dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroups();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_GROUPS_Select_One(GroupCode);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        SmBuyerSupplierGroups _group = new SmBuyerSupplierGroups();
                        _group.Fill(ds.Tables[0].Rows[0]);
                        int GroupID =convert.ToInt(_group.GroupId);
                        return GroupID;
                    }
                    else return 0;
                }
                else return 0;
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

        //added by kalpita on 21/11/2017
        public  static System.Data.DataSet Get_GroupFormat_AddrType(string cAddrType)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierGroups dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierGroups();
                return dbo.SM_BUYER_SUPPLIER_GROUPS_AddrType(cAddrType);              
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
    }

}