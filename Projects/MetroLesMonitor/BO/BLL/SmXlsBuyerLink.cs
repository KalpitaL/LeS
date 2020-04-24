namespace MetroLesMonitor.Bll
{
    public partial class SmXlsBuyerLink
    {
        private System.Nullable<int> _xlsBuyerMapid;

        public System.Nullable<int> _excelMapid;

        private System.Nullable<int> _buyerSuppLinkid;

        private System.Nullable<int> _buyerid;

        private System.Nullable<int> _supplierid;

        private string _mapCell1;

        private string _mapCell1Val1;

        private string _mapCell1Val2;

        private string _mapCell2;

        private string _mapCell2Val;

        private string _mapCellNoDisc;

        private string _mapCellNoDiscVal;

        private string _doctype;

        private SmXlsGroupMapping _smXlsGroupMapping;

        private string _formatmapcode;//added by kalpita on 19/01/2017

        public virtual System.Nullable<int> XlsBuyerMapid
        {
            get
            {
                return _xlsBuyerMapid;
            }
            set
            {
                _xlsBuyerMapid = value;
            }
        }

        public virtual System.Nullable<int> BuyerSuppLinkid
        {
            get
            {
                return _buyerSuppLinkid;
            }
            set
            {
                _buyerSuppLinkid = value;
            }
        }

        public virtual System.Nullable<int> Buyerid
        {
            get
            {
                return _buyerid;
            }
            set
            {
                _buyerid = value;
            }
        }

        public virtual System.Nullable<int> Supplierid
        {
            get
            {
                return _supplierid;
            }
            set
            {
                _supplierid = value;
            }
        }

        public virtual string MapCell1
        {
            get
            {
                return _mapCell1;
            }
            set
            {
                _mapCell1 = value;
            }
        }

        public virtual string MapCell1Val1
        {
            get
            {
                return _mapCell1Val1;
            }
            set
            {
                _mapCell1Val1 = value;
            }
        }

        public virtual string MapCell1Val2
        {
            get
            {
                return _mapCell1Val2;
            }
            set
            {
                _mapCell1Val2 = value;
            }
        }

        public virtual string MapCell2
        {
            get
            {
                return _mapCell2;
            }
            set
            {
                _mapCell2 = value;
            }
        }

        public virtual string MapCell2Val
        {
            get
            {
                return _mapCell2Val;
            }
            set
            {
                _mapCell2Val = value;
            }
        }

        public virtual string MapCellNoDisc
        {
            get
            {
                return _mapCellNoDisc;
            }
            set
            {
                _mapCellNoDisc = value;
            }
        }

        public virtual string MapCellNoDiscVal
        {
            get
            {
                return _mapCellNoDiscVal;
            }
            set
            {
                _mapCellNoDiscVal = value;
            }
        }

        public virtual string DocType
        {
            get
            {
                return _doctype;
            }
            set
            {
                _doctype = value;
            }
        }

        public virtual SmXlsGroupMapping SmXlsGroupMapping
        {
            get
            {
                if ((this._smXlsGroupMapping == null))
                {
                    this._smXlsGroupMapping = MetroLesMonitor.Bll.SmXlsGroupMapping.Load(this._excelMapid);
                }
                return this._smXlsGroupMapping;
            }
            set
            {
                _smXlsGroupMapping = value;
                if (_smXlsGroupMapping != null) this._excelMapid = _smXlsGroupMapping.ExcelMapid;

            }
        }

        public virtual string FormatMapCode
        {
            get
            {
                return _formatmapcode;
            }
            set
            {
                _formatmapcode = value;
            }
        }
        
        private void Clean()
        {
            this.XlsBuyerMapid = null;
            this._excelMapid = null;
            this.BuyerSuppLinkid = null;
            this.Buyerid = null;
            this.Supplierid = null;
            this.MapCell1 = string.Empty;
            this.MapCell1Val1 = string.Empty;
            this.MapCell1Val2 = string.Empty;
            this.MapCell2 = string.Empty;
            this.MapCell2Val = string.Empty;
            this.DocType = string.Empty;
            this.SmXlsGroupMapping = null;
            this.FormatMapCode = string.Empty;
        }

        private void Fill(System.Data.DataRow dr)
        {
            this.Clean();
            if ((dr["XLS_BUYER_MAPID"] != System.DBNull.Value))
            {
                this.XlsBuyerMapid = ((System.Nullable<int>)(dr["XLS_BUYER_MAPID"]));
            }
            if ((dr["EXCEL_MAPID"] != System.DBNull.Value))
            {
                this._excelMapid = ((System.Nullable<int>)(dr["EXCEL_MAPID"]));
            }
            if ((dr["BUYER_SUPP_LINKID"] != System.DBNull.Value))
            {
                this.BuyerSuppLinkid = ((System.Nullable<int>)(dr["BUYER_SUPP_LINKID"]));
            }
            if ((dr["BUYERID"] != System.DBNull.Value))
            {
                this.Buyerid = ((System.Nullable<int>)(dr["BUYERID"]));
            }
            if ((dr["SUPPLIERID"] != System.DBNull.Value))
            {
                this.Supplierid = ((System.Nullable<int>)(dr["SUPPLIERID"]));
            }
            if ((dr["MAP_CELL1"] != System.DBNull.Value))
            {
                this.MapCell1 = ((string)(dr["MAP_CELL1"]));
            }
            if ((dr["MAP_CELL1_VAL1"] != System.DBNull.Value))
            {
                this.MapCell1Val1 = ((string)(dr["MAP_CELL1_VAL1"]));
            }
            if ((dr["MAP_CELL1_VAL2"] != System.DBNull.Value))
            {
                this.MapCell1Val2 = ((string)(dr["MAP_CELL1_VAL2"]));
            }
            if ((dr["MAP_CELL2"] != System.DBNull.Value))
            {
                this.MapCell2 = ((string)(dr["MAP_CELL2"]));
            }
            if ((dr["MAP_CELL2_VAL"] != System.DBNull.Value))
            {
                this.MapCell2Val = ((string)(dr["MAP_CELL2_VAL"]));
            }
            if ((dr["DOC_TYPE"] != System.DBNull.Value))
            {
                this.DocType = ((string)(dr["DOC_TYPE"]));
            }
            if ((dr["FORMAT_MAP_CODE"] != System.DBNull.Value))
            {
                this.FormatMapCode = ((string)(dr["FORMAT_MAP_CODE"]));
            }
        }

        public static System.Data.DataSet Select_SM_XLS_BUYER_LINKs()
        {
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink();
                //System.Data.DataSet ds = dbo.SM_XLS_BUYER_LINK_Select_All();
                System.Data.DataSet ds = dbo.SM_XLS_BUYER_LINK_Select_All_New();
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

        public static SmXlsBuyerLinkCollection Select_SM_XLS_BUYER_LINKs_By_EXCEL_MAPID(System.Nullable<int> EXCEL_MAPID)
        {
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink();
                System.Data.DataSet ds = dbo.Select_SM_XLS_BUYER_LINKs_By_EXCEL_MAPID(EXCEL_MAPID);
                SmXlsBuyerLinkCollection collection = new SmXlsBuyerLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmXlsBuyerLink obj = new SmXlsBuyerLink();
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

        public static SmXlsBuyerLink Select_SM_XLS_BUYER_LINKs_By_LINKID(System.Nullable<int> LINKID)
        {
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                SmXlsBuyerLink obj = null;
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink();
                System.Data.DataSet ds = dbo.Select_SM_XLS_BUYER_LINKs_By_LINKID(LINKID);               
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        obj = new SmXlsBuyerLink();
                        obj.Fill(ds.Tables[0].Rows[i]);
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

        public static SmXlsBuyerLinkCollection GetAll()
        {
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink();
                System.Data.DataSet ds = dbo.SM_XLS_BUYER_LINK_Select_All();
                SmXlsBuyerLinkCollection collection = new SmXlsBuyerLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmXlsBuyerLink obj = new SmXlsBuyerLink();
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

        public static SmXlsBuyerLink Load(System.Nullable<int> XLS_BUYER_MAPID)
        {
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink();
                System.Data.DataSet ds = dbo.SM_XLS_BUYER_LINK_Select_One(XLS_BUYER_MAPID);
                SmXlsBuyerLink obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmXlsBuyerLink();
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
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink();
                System.Data.DataSet ds = dbo.SM_XLS_BUYER_LINK_Select_One(this.XlsBuyerMapid);
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

        //added  by kalpita on 23/05/2017
        public static System.Data.DataSet GetDocTypes()
        {
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink();
                System.Data.DataSet ds = dbo.Select_All_DocTypes();
                if (GlobalTools.IsSafeDataSet(ds)) return ds;
                else return null;
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
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink();
                dbo.SM_XLS_BUYER_LINK_Insert(this.XlsBuyerMapid, this._excelMapid, this.BuyerSuppLinkid, this.Buyerid, this.Supplierid, this.MapCell1, this.MapCell1Val1, this.MapCell1Val2, this.MapCell2, this.MapCell2Val, this.MapCellNoDisc, this.MapCellNoDiscVal, this.DocType, this.FormatMapCode);
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

        public virtual void Insert(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink(_dataAccess);
                this.XlsBuyerMapid = dbo.SM_XLS_BUYER_LINK_Insert(0, this._excelMapid, this.BuyerSuppLinkid, this.Buyerid, this.Supplierid, this.MapCell1, this.MapCell1Val1, this.MapCell1Val2, this.MapCell2, this.MapCell2Val, this.MapCellNoDisc, this.MapCellNoDiscVal, this.DocType,this.FormatMapCode);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public virtual void Delete()
        {
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink();
                dbo.SM_XLS_BUYER_LINK_Delete(this.XlsBuyerMapid);
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
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink(_dataAccess);
                dbo.SM_XLS_BUYER_LINK_Delete(this.XlsBuyerMapid);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public virtual void Update()
        {
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink();
                dbo.SM_XLS_BUYER_LINK_Update(this.XlsBuyerMapid, this._excelMapid, this.BuyerSuppLinkid, this.Buyerid, this.Supplierid, this.MapCell1, this.MapCell1Val1, this.MapCell1Val2, this.MapCell2, this.MapCell2Val, this.MapCellNoDisc, this.MapCellNoDiscVal, this.DocType, this.FormatMapCode);
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
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink(_dataAccess);
                dbo.SM_XLS_BUYER_LINK_Update(this.XlsBuyerMapid, this._excelMapid, this.BuyerSuppLinkid, this.Buyerid, this.Supplierid, this.MapCell1, this.MapCell1Val1, this.MapCell1Val2, this.MapCell2, this.MapCell2Val, this.MapCellNoDisc, this.MapCellNoDiscVal, this.DocType, this.FormatMapCode);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        //ADDED BY KALPITA ON 04/01/2018
        public static System.Data.DataSet Select_SM_XLS_BUYER_LINK_AddressId(string ADDRESSID, string ADDRTYPE)
        {
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink();
                System.Data.DataSet ds = dbo.SM_XLS_BUYER_LINK_AddressId(ADDRESSID, ADDRTYPE);
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

        public static string GetNextFormatMapCode(string DOCTYPE, string FORMAT_MAPCODE, Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink(_dataAccess);
                return dbo.GetFormatMapCode(DOCTYPE, FORMAT_MAPCODE);
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if (_dataAccess == null)
                {
                    if ((dbo != null))
                    {
                        dbo.Dispose();
                    }
                }
            }
        }

        public static SmXlsBuyerLinkCollection Select_SM_XLS_BUYER_LINKs_By_EXCEL_MAPID_BUYERID(System.Nullable<int> EXCEL_MAPID, int BUYERID)
        {
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink();
                System.Data.DataSet ds = dbo.Select_SM_XLS_BUYER_LINKs_By_EXCEL_MAPID_BUYERID(EXCEL_MAPID, BUYERID);
                SmXlsBuyerLinkCollection collection = new SmXlsBuyerLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmXlsBuyerLink obj = new SmXlsBuyerLink();
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

        public static SmXlsBuyerLinkCollection Get_SM_XLS_BUYER_LINK_AddressId(string ADDRESSID, string ADDRTYPE)
        {
            try
            {
                System.Data.DataSet ds = SmXlsBuyerLink.Select_SM_XLS_BUYER_LINK_AddressId(ADDRESSID, ADDRTYPE);
                SmXlsBuyerLinkCollection collection = new SmXlsBuyerLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmXlsBuyerLink obj = new SmXlsBuyerLink();
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
        }

        public static System.Data.DataSet Get_ExcelBuyerLink_AddressId(string ADDRESSID, string ADDRTYPE)
        {
            try
            {
                return SmXlsBuyerLink.Select_SM_XLS_BUYER_LINK_AddressId(ADDRESSID, ADDRTYPE);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public static System.Data.DataSet Get_ExcelBuyerLink_Wiz(string ADDRESSID)
        {
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink();
                return dbo.GetExcelBuyerLink_By_AddressID(ADDRESSID);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public static SmXlsBuyerLinkCollection Get_XLSBuyerLink_By_Mapid(string XLS_BUYER_MAPID)
        {
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink();
                System.Data.DataSet ds = dbo.GetExcelBuyerLinks_By_MAPID(XLS_BUYER_MAPID);
                SmXlsBuyerLinkCollection collection = new SmXlsBuyerLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmXlsBuyerLink obj = new SmXlsBuyerLink();
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

        //ADDED BY KALPITA ON 22/02/2018
        public static System.Data.DataSet SM_XLS_BUYER_LINK_MapCode()
        {
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink();
                System.Data.DataSet ds = dbo.SM_XLS_BUYER_LINK_MapCode();
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

        public static SmXlsBuyerLink Load_new(System.Nullable<int> XLS_BUYER_MAPID, Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmXlsBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmXlsBuyerLink(_dataAccess);
                System.Data.DataSet ds = dbo.SM_XLS_BUYER_LINK_Select_One(XLS_BUYER_MAPID);
                SmXlsBuyerLink obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmXlsBuyerLink();
                        obj.Fill(ds.Tables[0].Rows[0]);
                    }
                }
                return obj;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

    }
}