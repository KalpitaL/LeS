using System;
namespace MetroLesMonitor.Bll
{        
    public partial class SmServerSync {
        
        private System.Nullable<int> _serverid;
        
        private string _serverName;
        
        private string _serviceUrl;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private System.Nullable<System.DateTime> _updatedDate;
        
        public virtual System.Nullable<int> Serverid {
            get {
                return _serverid;
            }
            set {
                _serverid = value;
            }
        }
        
        public virtual string ServerName {
            get {
                return _serverName;
            }
            set {
                _serverName = value;
            }
        }
        
        public virtual string ServiceUrl {
            get {
                return _serviceUrl;
            }
            set {
                _serviceUrl = value;
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
        
        public virtual System.Nullable<System.DateTime> UpdatedDate {
            get {
                return _updatedDate;
            }
            set {
                _updatedDate = value;
            }
        }
        
        private void Clean() {
            this.Serverid = null;
            this.ServerName = string.Empty;
            this.ServiceUrl = string.Empty;
            this.CreatedDate = null;
            this.UpdatedDate = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["SERVERID"] != System.DBNull.Value)) {
                this.Serverid = ((System.Nullable<int>)(dr["SERVERID"]));
            }
            if ((dr["SERVER_NAME"] != System.DBNull.Value)) {
                this.ServerName = ((string)(dr["SERVER_NAME"]));
            }
            if ((dr["SERVICE_URL"] != System.DBNull.Value)) {
                this.ServiceUrl = ((string)(dr["SERVICE_URL"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value)) {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["UPDATED_DATE"] != System.DBNull.Value)) {
                this.UpdatedDate = ((System.Nullable<System.DateTime>)(dr["UPDATED_DATE"]));
            }
        }
        
        public static SmServerSyncCollection GetAll() {
            MetroLesMonitor.Dal.SmServerSync dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmServerSync();
                System.Data.DataSet ds = dbo.SM_SERVER_SYNC_Select_All();
                SmServerSyncCollection collection = new SmServerSyncCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmServerSync obj = new SmServerSync();
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
        
        public static SmServerSync Load(System.Nullable<int> SERVERID) {
            MetroLesMonitor.Dal.SmServerSync dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmServerSync();
                System.Data.DataSet ds = dbo.SM_SERVER_SYNC_Select_One(SERVERID);
                SmServerSync obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmServerSync();
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
            MetroLesMonitor.Dal.SmServerSync dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmServerSync();
                System.Data.DataSet ds = dbo.SM_SERVER_SYNC_Select_One(this.Serverid);
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

        public virtual void Load_ID(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmServerSync dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmServerSync(_dataAccess);
                System.Data.DataSet ds = dbo.SM_SERVER_SYNC_Select_One(this.Serverid);
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

            }
        }

        public virtual void Insert(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmServerSync dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmServerSync(_dataAccess);
                if (_dataAccess == null) { dbo._dataAccess.BeginTransaction(); }
                dbo.SM_SERVER_SYNC_Insert(this.ServerName, this.ServiceUrl, this.CreatedDate, this.UpdatedDate);
                if (_dataAccess == null) { dbo._dataAccess.CommitTransaction(); }
            }
            catch (System.Exception)
            {
                if (_dataAccess == null) { dbo._dataAccess.RollbackTransaction(); }
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

        public virtual void Delete(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmServerSync dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmServerSync(_dataAccess);
                if (_dataAccess == null) { dbo._dataAccess.BeginTransaction(); }
                dbo.SM_SERVER_SYNC_Delete(this.Serverid);
                if (_dataAccess == null) { dbo._dataAccess.CommitTransaction(); }
            }
            catch (System.Exception)
            {
                if (_dataAccess == null) { dbo._dataAccess.RollbackTransaction(); }
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

        public virtual void Update(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmServerSync dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmServerSync(_dataAccess);
                if (_dataAccess == null) { dbo._dataAccess.BeginTransaction(); }
                dbo.SM_SERVER_SYNC_Update(this.Serverid, this.ServerName, this.ServiceUrl, this.CreatedDate, this.UpdatedDate);
                if (_dataAccess == null) { dbo._dataAccess.CommitTransaction(); }
            }
            catch (System.Exception)
            {
                if (_dataAccess == null) { dbo._dataAccess.RollbackTransaction(); }
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
    }
}
