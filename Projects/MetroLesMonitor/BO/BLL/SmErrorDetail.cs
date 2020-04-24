using MetroLesMonitor;
namespace MetroLesMonitor.Bll
{
    public partial class SmErrorDetail {
        
        private System.Nullable<int> _errorId;
        
        private string _errorNo;
        
        private string _errorDesc;
        
        private string _errorProblem;
        
        private string _errorSolution;
        
        private string _errorTemplate;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private System.Nullable<System.DateTime> _updatedDate;
        
        public virtual System.Nullable<int> ErrorId {
            get {
                return _errorId;
            }
            set {
                _errorId = value;
            }
        }
        
        public virtual string ErrorNo {
            get {
                return _errorNo;
            }
            set {
                _errorNo = value;
            }
        }
        
        public virtual string ErrorDesc {
            get {
                return _errorDesc;
            }
            set {
                _errorDesc = value;
            }
        }
        
        public virtual string ErrorProblem {
            get {
                return _errorProblem;
            }
            set {
                _errorProblem = value;
            }
        }
        
        public virtual string ErrorSolution {
            get {
                return _errorSolution;
            }
            set {
                _errorSolution = value;
            }
        }
        
        public virtual string ErrorTemplate {
            get {
                return _errorTemplate;
            }
            set {
                _errorTemplate = value;
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
            this.ErrorId = null;
            this.ErrorNo = string.Empty;
            this.ErrorDesc = string.Empty;
            this.ErrorProblem = string.Empty;
            this.ErrorSolution = string.Empty;
            this.ErrorTemplate = string.Empty;
            this.CreatedDate = null;
            this.UpdatedDate = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["ERROR_ID"] != System.DBNull.Value)) {
                this.ErrorId = ((System.Nullable<int>)(dr["ERROR_ID"]));
            }
            if ((dr["ERROR_NO"] != System.DBNull.Value)) {
                this.ErrorNo = ((string)(dr["ERROR_NO"]));
            }
            if ((dr["ERROR_DESC"] != System.DBNull.Value)) {
                this.ErrorDesc = ((string)(dr["ERROR_DESC"]));
            }
            if ((dr["ERROR_PROBLEM"] != System.DBNull.Value)) {
                this.ErrorProblem = ((string)(dr["ERROR_PROBLEM"]));
            }
            if ((dr["ERROR_SOLUTION"] != System.DBNull.Value)) {
                this.ErrorSolution = ((string)(dr["ERROR_SOLUTION"]));
            }
            if ((dr["ERROR_TEMPLATE"] != System.DBNull.Value)) {
                this.ErrorTemplate = ((string)(dr["ERROR_TEMPLATE"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value)) {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["UPDATED_DATE"] != System.DBNull.Value)) {
                this.UpdatedDate = ((System.Nullable<System.DateTime>)(dr["UPDATED_DATE"]));
            }
        }
        
        public static SmErrorDetailCollection GetAll() {
            MetroLesMonitor.Dal.SmErrorDetail dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmErrorDetail();
                System.Data.DataSet ds = dbo.SM_ERROR_DETAIL_Select_All();
                SmErrorDetailCollection collection = new SmErrorDetailCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmErrorDetail obj = new SmErrorDetail();
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
        
        public static SmErrorDetail Load(System.Nullable<int> ERROR_ID) {
            MetroLesMonitor.Dal.SmErrorDetail dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmErrorDetail();
                System.Data.DataSet ds = dbo.SM_ERROR_DETAIL_Select_One(ERROR_ID);
                SmErrorDetail obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmErrorDetail();
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

        public static SmErrorDetail LoadByErrSolNo(string ERROR_NO)
        {
            MetroLesMonitor.Dal.SmErrorDetail dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmErrorDetail();
                System.Data.DataSet ds = dbo.SM_ERROR_DETAIL_Select_By_ERROR_NO(ERROR_NO);
                SmErrorDetail obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmErrorDetail();
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
        
        public virtual void Load() {
            MetroLesMonitor.Dal.SmErrorDetail dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmErrorDetail();
                System.Data.DataSet ds = dbo.SM_ERROR_DETAIL_Select_One(this.ErrorId);
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
            MetroLesMonitor.Dal.SmErrorDetail dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmErrorDetail();
                dbo.SM_ERROR_DETAIL_Insert(this.ErrorNo, this.ErrorDesc, this.ErrorProblem, this.ErrorSolution, this.ErrorTemplate, this.CreatedDate, this.UpdatedDate);
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
            MetroLesMonitor.Dal.SmErrorDetail dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmErrorDetail();
                dbo.SM_ERROR_DETAIL_Delete(this.ErrorId);
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
            MetroLesMonitor.Dal.SmErrorDetail dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmErrorDetail();
                dbo.SM_ERROR_DETAIL_Update(this.ErrorId, this.ErrorNo, this.ErrorDesc, this.ErrorProblem, this.ErrorSolution, this.ErrorTemplate, this.CreatedDate, this.UpdatedDate);
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

        public virtual System.Data.DataSet Get_Error_Details()
        {
            System.Data.DataSet _ds = null;
            MetroLesMonitor.Dal.SmErrorDetail dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmErrorDetail();
                System.Data.DataSet ds = dbo.Get_Error_Details();
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
    }
}
