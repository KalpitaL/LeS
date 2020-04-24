namespace MetroLesMonitor.Bll
{
    public partial class SmExternalUsers
    {
        private System.Nullable<int> _exUserid;

        private System.Nullable<int> _addressid;

        private string _exUsercode;

        private string _exUsername;

        private string _exPassword;

        private string _exEmailid;

        private System.Nullable<System.DateTime> _createdDate;

        private System.Nullable<System.DateTime> _updateDate;

        private System.Nullable<byte> _isactive;

        private System.Nullable<int> _pwdExpired;

        private System.Nullable<int> _accessLevel;

        private System.Nullable<int> _linkid;

        public virtual System.Nullable<int> ExUserid
        {
            get
            {
                return _exUserid;
            }
            set
            {
                _exUserid = value;
            }
        }

        public virtual System.Nullable<int> Addressid
        {
            get
            {
                return _addressid;
            }
            set
            {
                _addressid = value;
            }
        }

        public virtual string ExUsercode
        {
            get
            {
                return _exUsercode;
            }
            set
            {
                _exUsercode = value;
            }
        }

        public virtual string ExUsername
        {
            get
            {
                return _exUsername;
            }
            set
            {
                _exUsername = value;
            }
        }

        public virtual string ExPassword
        {
            get
            {
                return _exPassword;
            }
            set
            {
                _exPassword = value;
            }
        }

        public virtual string ExEmailid
        {
            get
            {
                return _exEmailid;
            }
            set
            {
                _exEmailid = value;
            }
        }

        public virtual System.Nullable<System.DateTime> CreatedDate
        {
            get
            {
                return _createdDate;
            }
            set
            {
                _createdDate = value;
            }
        }

        public virtual System.Nullable<System.DateTime> UpdateDate
        {
            get
            {
                return _updateDate;
            }
            set
            {
                _updateDate = value;
            }
        }

        public virtual System.Nullable<byte> Isactive
        {
            get
            {
                return _isactive;
            }
            set
            {
                _isactive = value;
            }
        }

        public virtual System.Nullable<int> PwdExpired
        {
            get
            {
                return _pwdExpired;
            }
            set
            {
                _pwdExpired = value;
            }
        }

        public virtual System.Nullable<int> AccessLevel
        {
            get
            {
                return _accessLevel;
            }
            set
            {
                _accessLevel = value;
            }
        }

        public virtual System.Nullable<int> LinkID
        {
            get
            {
                return _linkid;
            }
            set
            {
                _linkid = value;
            }
        }

        private void Clean()
        {
            this.ExUserid = null;
            this.Addressid = null;
            this.ExUsercode = null;
            this.ExUsername = string.Empty;
            this.ExPassword = string.Empty;
            this.ExEmailid = string.Empty;
            //this.Usertype = null;
            this.CreatedDate = null;
            //this.Siteid = null;
            //this.Exported = null;
            this.UpdateDate = null;
            //this.UpdateSite = null;
            this.Isactive = null;
            this.PwdExpired = null;
            this.AccessLevel = null;
            this.LinkID = null;
            //this.UpdateInvoice = null;
        }

        private void Fill(System.Data.DataRow dr)
        {
            this.Clean();
            if ((dr["EX_USERID"] != System.DBNull.Value))
            {
                this.ExUserid = ((System.Nullable<int>)(dr["EX_USERID"]));
            }
            if ((dr["ADDRESSID"] != System.DBNull.Value))
            {
                this.Addressid = ((System.Nullable<int>)(dr["ADDRESSID"]));
            }
            if ((dr["EX_USERCODE"] != System.DBNull.Value))
            {
                this.ExUsercode = ((string)(dr["EX_USERCODE"]));
            }
            if ((dr["EX_USERNAME"] != System.DBNull.Value))
            {
                this.ExUsername = ((string)(dr["EX_USERNAME"]));
            }
            if ((dr["EX_PASSWORD"] != System.DBNull.Value))
            {
                this.ExPassword = ((string)(dr["EX_PASSWORD"]));
            }
            if ((dr["EX_EMAILID"] != System.DBNull.Value))
            {
                this.ExEmailid = ((string)(dr["EX_EMAILID"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value))
            {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value))
            {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
            if ((dr["ISACTIVE"] != System.DBNull.Value))
            {
                this.Isactive = ((System.Nullable<byte>)(dr["ISACTIVE"]));
            }
            if ((dr["PWD_EXPIRED"] != System.DBNull.Value))
            {
                this.PwdExpired = ((System.Nullable<int>)(dr["PWD_EXPIRED"]));
            }
            if ((dr["ACCESS_LEVEL"] != System.DBNull.Value))
            {
                this.AccessLevel = ((System.Nullable<int>)(dr["ACCESS_LEVEL"]));
            }
            if ((dr["LINKID"] != System.DBNull.Value))
            {
                this.LinkID = ((System.Nullable<int>)(dr["LINKID"]));
            }
        }

        public static SmExternalUsersCollection GetAll()
        {
            MetroLesMonitor.Dal.SmExternalUsers dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmExternalUsers();
                System.Data.DataSet ds = dbo.SM_EXTERNAL_USERS_Select_All();
                SmExternalUsersCollection collection = new SmExternalUsersCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmExternalUsers obj = new SmExternalUsers();
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

        public static System.Data.DataSet GetPwdByAddressid(System.Nullable<int> ADDRESSID,string SuppEmail)
        {
            MetroLesMonitor.Dal.SmExternalUsers dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmExternalUsers();
                System.Data.DataSet ds = dbo.SM_EXTERNAL_USERS_DISTINCT_PWD_Select_All_By_AddressID(ADDRESSID, SuppEmail);
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

        public static SmExternalUsers Load(System.Nullable<int> EX_USERID)
        {
            MetroLesMonitor.Dal.SmExternalUsers dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmExternalUsers();
                System.Data.DataSet ds = dbo.SM_EXTERNAL_USERS_Select_One(EX_USERID);
                SmExternalUsers obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmExternalUsers();
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

        public static SmExternalUsers Load(string EX_USERID, string EX_PASSWORD)
        {
            MetroLesMonitor.Dal.SmExternalUsers dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmExternalUsers();
                System.Data.DataSet ds = dbo.SM_EXTERNAL_USERS_Select_One(EX_USERID, EX_PASSWORD);
                SmExternalUsers obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmExternalUsers();
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

        public static SmExternalUsers LoadByLinkID(System.Nullable<int> LINKID)
        {
            MetroLesMonitor.Dal.SmExternalUsers dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmExternalUsers();
                System.Data.DataSet ds = dbo.SM_EXTERNAL_USERS_Select_One_By_LINKID(LINKID);
                SmExternalUsers obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmExternalUsers();
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
            MetroLesMonitor.Dal.SmExternalUsers dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmExternalUsers();
                System.Data.DataSet ds = dbo.SM_EXTERNAL_USERS_Select_One(this.ExUserid);
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

        public virtual void Insert(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmExternalUsers dbo = null;
            try
            {
                this.ExUserid = 0;
                dbo = new MetroLesMonitor.Dal.SmExternalUsers(_dataAccess);
                this.ExUserid = dbo.SM_EXTERNAL_USERS_Insert(this.ExUserid, this.Addressid, this.ExUsercode, this.ExUsername, this.ExPassword, this.ExEmailid, this.CreatedDate, this.UpdateDate, this.Isactive, this.PwdExpired, this.AccessLevel, this.LinkID);
            }
            catch (System.Exception)
            {
                throw;
            }          
        }

        public virtual void Delete()
        {
            MetroLesMonitor.Dal.SmExternalUsers dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmExternalUsers();
                dbo.SM_EXTERNAL_USERS_Delete(this.ExUserid);
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
            MetroLesMonitor.Dal.SmExternalUsers dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmExternalUsers();
                dbo.SM_EXTERNAL_USERS_Update(this.ExUserid, this.Addressid, this.ExUsercode, this.ExUsername, this.ExPassword, this.ExEmailid, this.UpdateDate, this.Isactive, this.PwdExpired, this.AccessLevel, this.LinkID);
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
            MetroLesMonitor.Dal.SmExternalUsers dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmExternalUsers(_dataAccess);
                dbo.SM_EXTERNAL_USERS_Update(this.ExUserid, this.Addressid, this.ExUsercode, this.ExUsername, this.ExPassword, this.ExEmailid, this.UpdateDate, this.Isactive, this.PwdExpired, this.AccessLevel, this.LinkID);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public static int DoLogin(string UserEmailID, string Password)
        {
            int Result = 0;

            // Encode Passwordl
            string EncodedPwd = GlobalTools.EncodePassword(Password);

            SmExternalUsers _user = Load(UserEmailID, EncodedPwd);
            if (_user != null)
            {
                Result = (int)_user.ExUserid;
            }
            else Result = -1;

            return Result;
        }
    }
}