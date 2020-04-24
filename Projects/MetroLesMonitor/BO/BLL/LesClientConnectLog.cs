namespace MetroLesMonitor.Bll {
    
    
    public partial class LesClientConnectLog {
        
        private System.Nullable<int> _connectid;
        
        private System.Nullable<int> _clientid;
        
        private System.Nullable<System.DateTime> _lastConnect;
        
        private System.Nullable<System.DateTime> _lastConnect1;
        
        private string _lisenceKey;
        
        private System.Nullable<float> _notifyCnt;
        
        public virtual System.Nullable<int> Connectid {
            get {
                return _connectid;
            }
            set {
                _connectid = value;
            }
        }
        
        public virtual System.Nullable<int> Clientid {
            get {
                return _clientid;
            }
            set {
                _clientid = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> LastConnect {
            get {
                return _lastConnect;
            }
            set {
                _lastConnect = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> LastConnect1 {
            get {
                return _lastConnect1;
            }
            set {
                _lastConnect1 = value;
            }
        }
        
        public virtual string LisenceKey {
            get {
                return _lisenceKey;
            }
            set {
                _lisenceKey = value;
            }
        }
        
        public virtual System.Nullable<float> NotifyCnt {
            get {
                return _notifyCnt;
            }
            set {
                _notifyCnt = value;
            }
        }
        
        private void Clean() {
            this.Connectid = null;
            this.Clientid = null;
            this.LastConnect = null;
            this.LastConnect1 = null;
            this.LisenceKey = string.Empty;
            this.NotifyCnt = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["CONNECTID"] != System.DBNull.Value)) {
                this.Connectid = ((System.Nullable<int>)(dr["CONNECTID"]));
            }
            if ((dr["CLIENTID"] != System.DBNull.Value)) {
                this.Clientid = ((System.Nullable<int>)(dr["CLIENTID"]));
            }
            if ((dr["LAST_CONNECT"] != System.DBNull.Value)) {
                this.LastConnect = ((System.Nullable<System.DateTime>)(dr["LAST_CONNECT"]));
            }
            if ((dr["LAST_CONNECT1"] != System.DBNull.Value)) {
                this.LastConnect1 = ((System.Nullable<System.DateTime>)(dr["LAST_CONNECT1"]));
            }
            if ((dr["LISENCE_KEY"] != System.DBNull.Value)) {
                this.LisenceKey = ((string)(dr["LISENCE_KEY"]));
            }
            if ((dr["NOTIFY_CNT"] != System.DBNull.Value)) {
                this.NotifyCnt = ((System.Nullable<float>)(dr["NOTIFY_CNT"]));
            }
        }
        
        public static LesClientConnectLogCollection GetAll() {
            MetroLesMonitor.Dal.LesClientConnectLog dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesClientConnectLog();
                System.Data.DataSet ds = dbo.LES_CLIENT_CONNECT_LOG_Select_All();
                LesClientConnectLogCollection collection = new LesClientConnectLogCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesClientConnectLog obj = new LesClientConnectLog();
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
            MetroLesMonitor.Dal.LesClientConnectLog dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesClientConnectLog();
                dbo.LES_CLIENT_CONNECT_LOG_Insert(this.Clientid, this.LastConnect, this.LastConnect1, this.LisenceKey, this.NotifyCnt);
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

        public virtual System.Data.DataSet SelectAll(int ADDRESSID)
        {
            MetroLesMonitor.Dal.LesClientConnectLog dbo = null;
            try
            {
                string cmdSQL = "";
                dbo = new MetroLesMonitor.Dal.LesClientConnectLog();
                SmAddress _address = SmAddress.Load(ADDRESSID);

                if (_address.AddrType.ToLower().Contains("admin"))
                {
                    cmdSQL = "SELECT SMV_LES_CLIENTS_LOG.ADDRESSID, SMV_LES_CLIENTS_LOG.ADDR_CODE, SMV_LES_CLIENTS_LOG.ADDR_NAME, SMV_LES_CLIENTS_LOG.LAST_CONNECT, SMV_LES_CLIENTS_LOG.LAST_CONNECT1 , SMV_LES_CLIENTS_LOG.NEXT_CONNECT, SMV_LES_CLIENTS_LOG.INTERVAL, SMV_LES_CLIENTS_LOG.ADDR_TYPE FROM SMV_LES_CLIENTS_LOG ";
                }
                else
                {
                    cmdSQL = "SELECT SMV_LES_CLIENTS_LOG.ADDRESSID, SMV_LES_CLIENTS_LOG.ADDR_CODE, SMV_LES_CLIENTS_LOG.ADDR_NAME, SMV_LES_CLIENTS_LOG.LAST_CONNECT, SMV_LES_CLIENTS_LOG.LAST_CONNECT1 , SMV_LES_CLIENTS_LOG.NEXT_CONNECT, SMV_LES_CLIENTS_LOG.INTERVAL, SMV_LES_CLIENTS_LOG.ADDR_TYPE FROM SMV_LES_CLIENTS_LOG WHERE SMV_LES_CLIENTS_LOG.ADDRESSID=" + ADDRESSID;
                }

                System.Data.DataSet ds = dbo.LES_CLIENT_CONNECT_LOG_Select_All(cmdSQL);
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
    }
}
