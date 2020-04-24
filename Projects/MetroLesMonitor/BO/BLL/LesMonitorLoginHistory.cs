namespace MetroLesMonitor.Bll {
    
    
    public partial class LesMonitorLoginHistory {
        
        private System.Nullable<int> _loginTrackId;
        
        private System.Nullable<int> _userid;
        
        private string _sessionid;
        
        private string _clientServerIp;
        
        private System.Nullable<System.DateTime> _loggedIn;
        
        private string _loggedInRemarks;
        
        private System.Nullable<System.DateTime> _loggedOut;
        
        private string _loggedOutRemarks;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        public virtual System.Nullable<int> LoginTrackId {
            get {
                return _loginTrackId;
            }
            set {
                _loginTrackId = value;
            }
        }
        
        public virtual System.Nullable<int> Userid {
            get {
                return _userid;
            }
            set {
                _userid = value;
            }
        }
        
        public virtual string Sessionid {
            get {
                return _sessionid;
            }
            set {
                _sessionid = value;
            }
        }
        
        public virtual string ClientServerIp
 {
            get {
                return _clientServerIp;
            }
            set {
                _clientServerIp = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> LoggedIn {
            get {
                return _loggedIn;
            }
            set {
                _loggedIn = value;
            }
        }
        
        public virtual string LoggedInRemarks {
            get {
                return _loggedInRemarks;
            }
            set {
                _loggedInRemarks = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> LoggedOut {
            get {
                return _loggedOut;
            }
            set {
                _loggedOut = value;
            }
        }
        
        public virtual string LoggedOutRemarks {
            get {
                return _loggedOutRemarks;
            }
            set {
                _loggedOutRemarks = value;
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
        
        private void Clean() {
            this.LoginTrackId = null;
            this.Userid = null;
            this.Sessionid = string.Empty;
            this.ClientServerIp = string.Empty;
            this.LoggedIn = null;
            this.LoggedInRemarks = string.Empty;
            this.LoggedOut = null;
            this.LoggedOutRemarks = string.Empty;
            this.UpdateDate = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["LOGIN_TRACK_ID"] != System.DBNull.Value)) {
                this.LoginTrackId = ((System.Nullable<int>)(dr["LOGIN_TRACK_ID"]));
            }
            if ((dr["USERID"] != System.DBNull.Value)) {
                this.Userid = ((System.Nullable<int>)(dr["USERID"]));
            }
            if ((dr["SESSIONID"] != System.DBNull.Value)) {
                this.Sessionid = ((string)(dr["SESSIONID"]));
            }
            if ((dr["CLIENT_SERVER_IP"] != System.DBNull.Value)) {
                this.ClientServerIp = ((string)(dr["CLIENT_SERVER_IP"]));
            }
            if ((dr["LOGGED_IN"] != System.DBNull.Value)) {
                this.LoggedIn = ((System.Nullable<System.DateTime>)(dr["LOGGED_IN"]));
            }
            if ((dr["LOGGED_IN_REMARKS"] != System.DBNull.Value)) {
                this.LoggedInRemarks = ((string)(dr["LOGGED_IN_REMARKS"]));
            }
            if ((dr["LOGGED_OUT"] != System.DBNull.Value)) {
                this.LoggedOut = ((System.Nullable<System.DateTime>)(dr["LOGGED_OUT"]));
            }
            if ((dr["LOGGED_OUT_REMARKS"] != System.DBNull.Value)) {
                this.LoggedOutRemarks = ((string)(dr["LOGGED_OUT_REMARKS"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value)) {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
        }
        
        public static LesMonitorLoginHistoryCollection GetAll() {
            MetroLesMonitor.Dal.LesMonitorLoginHistory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesMonitorLoginHistory();
                System.Data.DataSet ds = dbo.LES_MONITOR_LOGIN_HISTORY_Select_All();
                LesMonitorLoginHistoryCollection collection = new LesMonitorLoginHistoryCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesMonitorLoginHistory obj = new LesMonitorLoginHistory();
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
        
        public static LesMonitorLoginHistory Load(System.Nullable<int> LOGIN_TRACK_ID) {
            MetroLesMonitor.Dal.LesMonitorLoginHistory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesMonitorLoginHistory();
                System.Data.DataSet ds = dbo.LES_MONITOR_LOGIN_HISTORY_Select_One(LOGIN_TRACK_ID);
                LesMonitorLoginHistory obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new LesMonitorLoginHistory();
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
            MetroLesMonitor.Dal.LesMonitorLoginHistory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesMonitorLoginHistory();
                System.Data.DataSet ds = dbo.LES_MONITOR_LOGIN_HISTORY_Select_One(this.LoginTrackId);
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
            MetroLesMonitor.Dal.LesMonitorLoginHistory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesMonitorLoginHistory();
                dbo.LES_MONITOR_LOGIN_HISTORY_Insert(this.Userid, this.Sessionid, this.ClientServerIp, this.LoggedIn, this.LoggedInRemarks, this.LoggedOut, this.LoggedOutRemarks, this.UpdateDate);
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
            MetroLesMonitor.Dal.LesMonitorLoginHistory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesMonitorLoginHistory();
                dbo.LES_MONITOR_LOGIN_HISTORY_Delete(this.LoginTrackId);
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
            MetroLesMonitor.Dal.LesMonitorLoginHistory dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesMonitorLoginHistory();
                dbo.LES_MONITOR_LOGIN_HISTORY_Update(this.LoginTrackId, this.Userid, this.Sessionid, this.ClientServerIp, this.LoggedIn, this.LoggedInRemarks, this.LoggedOut, this.LoggedOutRemarks, this.UpdateDate);
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

        public static System.Data.DataSet GetAllByDate(System.Nullable<System.DateTime> FromDate, System.Nullable<System.DateTime> ToDate)
        {
            MetroLesMonitor.Dal.LesMonitorLoginHistory dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.LesMonitorLoginHistory();
                System.Data.DataSet ds = dbo.LES_MONITOR_LOGIN_HISTORY_Select_All_ByDate(FromDate, ToDate);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    return ds;
                }
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

        public static LesMonitorLoginHistory Load(string SESSIONID)
        {
            MetroLesMonitor.Dal.LesMonitorLoginHistory dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.LesMonitorLoginHistory();
                System.Data.DataSet ds = dbo.LES_MONITOR_LOGIN_HISTORY_Select_One(SESSIONID);
                LesMonitorLoginHistory obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new LesMonitorLoginHistory();
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

    }
}
