using System.Data;
namespace MetroLesMonitor.Bll
{
    public partial class SmAuditlog
    {
        private System.Nullable<int> _logid;

        private string _modulename;

        private string _filename;

        private string _auditvalue;

        private string _keyref1;

        private string _keyref2;

        private string _logtype;

        private System.Nullable<System.DateTime> _updatedate;

        private System.Nullable<int> _buyerId;

        private System.Nullable<int> _supplierId;

        public virtual System.Nullable<int> Logid
        {
            get
            {
                return _logid;
            }
            set
            {
                _logid = value;
            }
        }

        public virtual string Modulename
        {
            get
            {
                return _modulename;
            }
            set
            {
                _modulename = value;
            }
        }

        public virtual string Filename
        {
            get
            {
                return _filename;
            }
            set
            {
                _filename = value;
            }
        }

        public virtual string Auditvalue
        {
            get
            {
                return _auditvalue;
            }
            set
            {
                _auditvalue = value;
            }
        }

        public virtual string Keyref1
        {
            get
            {
                return _keyref1;
            }
            set
            {
                _keyref1 = value;
            }
        }

        public virtual string Keyref2
        {
            get
            {
                return _keyref2;
            }
            set
            {
                _keyref2 = value;
            }
        }

        public virtual string Logtype
        {
            get
            {
                return _logtype;
            }
            set
            {
                _logtype = value;
            }
        }

        public virtual System.Nullable<System.DateTime> Updatedate
        {
            get
            {
                return _updatedate;
            }
            set
            {
                _updatedate = value;
            }
        }

        public virtual System.Nullable<int> BuyerId
        {
            get
            {
                return _buyerId;
            }
            set
            {
                _buyerId = value;
            }
        }

        public virtual System.Nullable<int> SupplierId
        {
            get
            {
                return _supplierId;
            }
            set
            {
                _supplierId = value;
            }
        }

        private void Clean()
        {
            this.Logid = null;
            this.Modulename = string.Empty;
            this.Filename = string.Empty;
            this.Auditvalue = string.Empty;
            this.Keyref1 = string.Empty;
            this.Keyref2 = string.Empty;
            this.Logtype = string.Empty;
            this.Updatedate = null;
            this.BuyerId = null;
            this.SupplierId = null;
        }

        private void Fill(System.Data.DataRow dr)
        {
            this.Clean();
            if ((dr["LogID"] != System.DBNull.Value))
            {
                this.Logid = ((System.Nullable<int>)(dr["LogID"]));
            }
            if ((dr["ModuleName"] != System.DBNull.Value))
            {
                this.Modulename = ((string)(dr["ModuleName"]));
            }
            if ((dr["FileName"] != System.DBNull.Value))
            {
                this.Filename = ((string)(dr["FileName"]));
            }
            if ((dr["AuditValue"] != System.DBNull.Value))
            {
                this.Auditvalue = ((string)(dr["AuditValue"]));
            }
            if ((dr["KeyRef1"] != System.DBNull.Value))
            {
                this.Keyref1 = ((string)(dr["KeyRef1"]));
            }
            if ((dr["KeyRef2"] != System.DBNull.Value))
            {
                this.Keyref2 = ((string)(dr["KeyRef2"]));
            }
            if ((dr["LogType"] != System.DBNull.Value))
            {
                this.Logtype = ((string)(dr["LogType"]));
            }
            if ((dr["UpdateDate"] != System.DBNull.Value))
            {
                this.Updatedate = ((System.Nullable<System.DateTime>)(dr["UpdateDate"]));
            }
            if ((dr["BUYER_ID"] != System.DBNull.Value))
            {
                this.BuyerId = ((System.Nullable<int>)(dr["BUYER_ID"]));
            }
            if ((dr["SUPPLIER_ID"] != System.DBNull.Value))
            {
                this.SupplierId = ((System.Nullable<int>)(dr["SUPPLIER_ID"]));
            }
        }

        public static System.Data.DataSet GetAll()
        {
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog();
                System.Data.DataSet ds = dbo.SM_AUDITLOG_Select_All();
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

        public static System.Data.DataSet GetAll(System.Nullable<System.DateTime> DateFrom, System.Nullable<System.DateTime> DateTo)
        {
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog();
                System.Data.DataSet ds = dbo.SM_AUDITLOG_Select_All(DateFrom, DateTo);
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

        //public static System.Data.DataSet GetAll(System.Nullable<System.DateTime> DateFrom, System.Nullable<System.DateTime> DateTo,string SEARCH,string SELECTCOND)
        //{
        //    MetroLesMonitor.Dal.SmAuditlog dbo = null;
        //    try
        //    {
        //        dbo = new MetroLesMonitor.Dal.SmAuditlog();
        //        System.Data.DataSet ds = dbo.SM_AUDITLOG_Select_All(DateFrom, DateTo, SEARCH, SELECTCOND);
        //        return ds;
        //    }
        //    catch (System.Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if ((dbo != null))
        //        {
        //            dbo.Dispose();
        //        }
        //    }
        //}


        public static System.Data.DataSet GetAll(System.Nullable<System.DateTime> DateFrom, System.Nullable<System.DateTime> DateTo, string FILTERCOND, int FROM, int TO)
        {
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog();
                System.Data.DataSet ds = dbo.SM_AUDITLOG_Select_All(DateFrom, DateTo, FILTERCOND, FROM, TO);
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


        public static SmAuditlog Load(System.Nullable<int> LOGID)
        {
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog();
                System.Data.DataSet ds = dbo.SM_AUDITLOG_Select_One(LOGID);
                SmAuditlog _obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    _obj = new SmAuditlog();
                    if (ds.Tables[0].Rows.Count > 0) _obj.Fill(ds.Tables[0].Rows[0]);
                }
                return _obj;
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
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog(new Dal.DataAccess("DBConnectionString"));
                dbo.SM_AUDITLOG_Insert(this.Modulename, this.Filename, this.Auditvalue, this.Keyref1, this.Keyref2, this.Logtype, this.Updatedate, this.BuyerId, this.SupplierId);
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
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog(new Dal.DataAccess("DBConnectionString"));
                dbo.SM_AUDITLOG_Update(this.Logid, this.Modulename, this.Filename, this.Auditvalue, this.Keyref1, this.Keyref2, this.Logtype, this.Updatedate, this.BuyerId, this.SupplierId);
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
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog(_dataAccess);
                dbo.SM_AUDITLOG_Insert(this.Modulename, this.Filename, this.Auditvalue, this.Keyref1, this.Keyref2, this.Logtype, this.Updatedate, this.BuyerId, this.SupplierId);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public virtual System.Data.DataSet SM_AUDITLOG_Select_By_AddressID(int ADDRESSID, string ADDR_TYPE)
        {
            System.Data.DataSet _ds = null;
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog();
                System.Data.DataSet ds = dbo.SM_AUDITLOG_Select_By_AddressID(ADDRESSID, ADDR_TYPE);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    _ds = ds;
                }
                return _ds;
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

        public virtual System.Data.DataSet SM_AUDITLOG_Select_By_AddressID(int ADDRESSID, string ADDR_TYPE, System.Nullable<System.DateTime> FromDate,
            System.Nullable<System.DateTime> ToDate, string FILTERCOND, int FROM, int TO, out int Totalrows)
        {
            System.Data.DataSet _ds = null;
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog();
                System.Data.DataSet ds = dbo.SM_AUDITLOG_Select_By_AddressID(ADDRESSID, ADDR_TYPE, FromDate, ToDate, FILTERCOND, FROM, TO, out Totalrows);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    _ds = ds;
                }
                return _ds;
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

        public virtual System.Data.DataSet SM_AUDITLOG_Select_By_BuyerSupplier(int BuyerID, int SupplierID)
        {
            System.Data.DataSet _ds = null;
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog();
                System.Data.DataSet ds = dbo.SM_AUDITLOG_Select_By_BuyerSupplier(BuyerID, SupplierID);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    _ds = ds;
                }
                return _ds;
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

        public virtual System.Data.DataSet SM_AUDITLOG_Select_By_BuyerSupplier(string ByrLinkCode, string SuppLinkCode)
        {
            System.Data.DataSet _ds = null;
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog();
                System.Data.DataSet ds = dbo.SM_AUDITLOG_Select_All_ByLinkCode(ByrLinkCode, SuppLinkCode);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    _ds = ds;
                }
                return _ds;
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

        public virtual System.Data.DataSet SM_AUDITLOG_Select_By_BuyerSupplier_By_Days(string ByrLinkCode, string SuppLinkCode, int Days)
        {
            System.Data.DataSet _ds = null;
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog();
                System.Data.DataSet ds = dbo.SM_AUDITLOG_Select_All_ByLinkCode_By_Days(ByrLinkCode, SuppLinkCode, Days);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    _ds = ds;
                }
                return _ds;
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

        public virtual System.Data.DataSet SM_AUDITLOG_Select_ErrorLogs()
        {
            System.Data.DataSet _ds = null;
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog();
                System.Data.DataSet ds = dbo.SM_AUDITLOG_Select_ErrorLogs();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    _ds = ds;
                }
                return _ds;
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

        public virtual System.Data.DataSet SM_AUDITLOG_Select_ErrorLogs(System.Nullable<System.DateTime> DateFrom, System.Nullable<System.DateTime> DateTo, string ERR_STATUS)
        {
            System.Data.DataSet _ds = null;
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog();
                System.Data.DataSet ds = dbo.SM_AUDITLOG_Select_ErrorLogs_Dates(DateFrom, DateTo, ERR_STATUS);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    _ds = ds;
                }
                return _ds;
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

        public virtual System.Data.DataSet GetLogTypes()
        {
            System.Data.DataSet _ds = null;
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog();
                System.Data.DataSet ds = dbo.SM_AUDITLOG_Select_All_LogTypes();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    _ds = ds;
                }
                return _ds;
            }
            catch
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

        public virtual System.Data.DataSet GetModuleName()
        {
            System.Data.DataSet _ds = null;
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog();
                System.Data.DataSet ds = dbo.SM_AUDITLOG_Select_All_Modules();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    _ds = ds;
                }
                return _ds;
            }
            catch
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

        public static System.Data.DataSet GetAllAuditLog(System.Nullable<System.DateTime> DateFrom, System.Nullable<System.DateTime> DateTo, string FILTERCOND,
            int FROM, int TO, out int TotalRows)
        {
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog();
                System.Data.DataSet ds = dbo.SM_AUDITLOG_Select_test(DateFrom, DateTo, FILTERCOND, FROM, TO, out TotalRows);//changed
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

        public virtual DataSet GetError_Details(int LOGID)
        {
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog();
                System.Data.DataSet ds = dbo.SM_AUDITLOG_Select_One(LOGID);
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

        public static System.Data.DataSet GetAllAuditLog_Filter(string cFin_Cond, string FROMDATE,string TODATE)
        {
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog();
                System.Data.DataSet ds = dbo.GetAudit_Filter(cFin_Cond, FROMDATE, TODATE);
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


        public static System.Data.DataSet GetTop5000(System.Nullable<System.DateTime> DateFrom, System.Nullable<System.DateTime> DateTo)
        {
            MetroLesMonitor.Dal.SmAuditlog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAuditlog();
                System.Data.DataSet ds = dbo.SM_AUDITLOG_Select_All_Top5000(DateFrom, DateTo);
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

    }

}