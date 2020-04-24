namespace MetroLesMonitor.Bll
{
    public partial class SmErrorLog
    {
        private System.Nullable<int> _errorLogid;

        private System.Nullable<int> _logid;

        private string _errorProblem;

        private string _errorSolution;

        private System.Nullable<int> _errorStatus;

        public virtual System.Nullable<int> ErrorLogid
        {
            get
            {
                return _errorLogid;
            }
            set
            {
                _errorLogid = value;
            }
        }

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

        public virtual string ErrorProblem
        {
            get
            {
                return _errorProblem;
            }
            set
            {
                _errorProblem = value;
            }
        }

        public virtual string ErrorSolution
        {
            get
            {
                return _errorSolution;
            }
            set
            {
                _errorSolution = value;
            }
        }

        public virtual System.Nullable<int> ErrorStatus
        {
            get
            {
                return _errorStatus;
            }
            set
            {
                _errorStatus = value;
            }
        }

        private void Clean()
        {
            this.ErrorLogid = null;
            this.Logid = null;
            this.ErrorProblem = string.Empty;
            this.ErrorSolution = string.Empty;
            this.ErrorStatus = null;
        }

        private void Fill(System.Data.DataRow dr)
        {
            this.Clean();
            if ((dr["ERROR_LOGID"] != System.DBNull.Value))
            {
                this.ErrorLogid = ((System.Nullable<int>)(dr["ERROR_LOGID"]));
            }
            if ((dr["LOGID"] != System.DBNull.Value))
            {
                this.Logid = ((System.Nullable<int>)(dr["LOGID"]));
            }
            if ((dr["ERROR_PROBLEM"] != System.DBNull.Value))
            {
                this.ErrorProblem = ((string)(dr["ERROR_PROBLEM"]));
            }
            if ((dr["ERROR_SOLUTION"] != System.DBNull.Value))
            {
                this.ErrorSolution = ((string)(dr["ERROR_SOLUTION"]));
            }
            if ((dr["ERROR_STATUS"] != System.DBNull.Value))
            {
                this.ErrorStatus = ((System.Nullable<int>)(dr["ERROR_STATUS"]));
            }
        }

        public static SmErrorLogCollection GetAll()
        {
            MetroLesMonitor.Dal.SmErrorLog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmErrorLog();
                System.Data.DataSet ds = dbo.SM_ERROR_LOG_Select_All();
                SmErrorLogCollection collection = new SmErrorLogCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmErrorLog obj = new SmErrorLog();
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

        public static SmErrorLog Load(System.Nullable<int> ERROR_LOGID)
        {
            MetroLesMonitor.Dal.SmErrorLog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmErrorLog();
                System.Data.DataSet ds = dbo.SM_ERROR_LOG_Select_One(ERROR_LOGID);
                SmErrorLog obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmErrorLog();
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

        public static SmErrorLog LoadByLogID(System.Nullable<int> LOGID)
        {
            MetroLesMonitor.Dal.SmErrorLog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmErrorLog();
                System.Data.DataSet ds = dbo.SM_ERROR_LOG_Select_By_LOGID(LOGID);
                SmErrorLog obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmErrorLog();
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
            MetroLesMonitor.Dal.SmErrorLog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmErrorLog();
                System.Data.DataSet ds = dbo.SM_ERROR_LOG_Select_One(this.ErrorLogid);
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
            MetroLesMonitor.Dal.SmErrorLog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmErrorLog();
                dbo.SM_ERROR_LOG_Insert(this.ErrorLogid, this.Logid, this.ErrorProblem, this.ErrorSolution,this.ErrorStatus);
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
            MetroLesMonitor.Dal.SmErrorLog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmErrorLog();
                dbo.SM_ERROR_LOG_Delete(this.ErrorLogid);
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
            MetroLesMonitor.Dal.SmErrorLog dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmErrorLog();
                dbo.SM_ERROR_LOG_Update(this.ErrorLogid, this.ErrorProblem, this.ErrorSolution,this.ErrorStatus);
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