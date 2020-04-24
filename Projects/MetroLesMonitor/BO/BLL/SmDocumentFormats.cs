using System;
namespace MetroLesMonitor.Bll {
    
    
    public partial class SmDocumentFormats {
        
        private System.Nullable<int> _docformatid;
        
        private string _documentFormat;
        
        private string _importPath;
        
        private string _exportPath;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private System.Nullable<System.DateTime> _updatedDate;
        
        private string _addrType;
        
        public virtual System.Nullable<int> Docformatid {
            get {
                return _docformatid;
            }
            set {
                _docformatid = value;
            }
        }
        
        public virtual string DocumentFormat {
            get {
                return _documentFormat;
            }
            set {
                _documentFormat = value;
            }
        }
        
        public virtual string ImportPath {
            get {
                return _importPath;
            }
            set {
                _importPath = value;
            }
        }
        
        public virtual string ExportPath {
            get {
                return _exportPath;
            }
            set {
                _exportPath = value;
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
        
        public virtual string AddrType {
            get {
                return _addrType;
            }
            set {
                _addrType = value;
            }
        }
        
        private void Clean() {
            this.Docformatid = null;
            this.DocumentFormat = string.Empty;
            this.ImportPath = string.Empty;
            this.ExportPath = string.Empty;
            this.CreatedDate = null;
            this.UpdatedDate = null;
            this.AddrType = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["DOCFORMATID"] != System.DBNull.Value)) {
                this.Docformatid = ((System.Nullable<int>)(dr["DOCFORMATID"]));
            }
            if ((dr["DOCUMENT_FORMAT"] != System.DBNull.Value)) {
                this.DocumentFormat = ((string)(dr["DOCUMENT_FORMAT"]));
            }
            if ((dr["IMPORT_PATH"] != System.DBNull.Value)) {
                this.ImportPath = ((string)(dr["IMPORT_PATH"]));
            }
            if ((dr["EXPORT_PATH"] != System.DBNull.Value)) {
                this.ExportPath = ((string)(dr["EXPORT_PATH"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value)) {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["UPDATED_DATE"] != System.DBNull.Value)) {
                this.UpdatedDate = ((System.Nullable<System.DateTime>)(dr["UPDATED_DATE"]));
            }
            if ((dr["ADDR_TYPE"] != System.DBNull.Value)) {
                this.AddrType = ((string)(dr["ADDR_TYPE"]));
            }
        }
        
        public static SmDocumentFormatsCollection GetAll() {
            MetroLesMonitor.Dal.SmDocumentFormats dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDocumentFormats();
                System.Data.DataSet ds = dbo.SM_DOCUMENT_FORMATS_Select_All();
                SmDocumentFormatsCollection collection = new SmDocumentFormatsCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmDocumentFormats obj = new SmDocumentFormats();
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
        
        public static SmDocumentFormats Load(System.Nullable<int> DOCFORMATID) {
            MetroLesMonitor.Dal.SmDocumentFormats dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDocumentFormats();
                System.Data.DataSet ds = dbo.SM_DOCUMENT_FORMATS_Select_One(DOCFORMATID);
                SmDocumentFormats obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmDocumentFormats();
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
            MetroLesMonitor.Dal.SmDocumentFormats dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDocumentFormats();
                System.Data.DataSet ds = dbo.SM_DOCUMENT_FORMATS_Select_One(this.Docformatid);
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

        public virtual void Insert(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmDocumentFormats dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDocumentFormats(_dataAccess);
                if (_dataAccess == null) { dbo._dataAccess.BeginTransaction(); }
                dbo.SM_DOCUMENT_FORMATS_Insert(this.DocumentFormat, this.ImportPath, this.ExportPath, this.CreatedDate, this.UpdatedDate, this.AddrType);
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
                        dbo._dataAccess._Dispose();
                    }
                }
            }
        }

        public virtual void Delete(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmDocumentFormats dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDocumentFormats(_dataAccess);
                if (_dataAccess == null) { dbo._dataAccess.BeginTransaction(); }
                dbo.SM_DOCUMENT_FORMATS_Delete(this.Docformatid);
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
                        dbo._dataAccess._Dispose();
                    }
                }
            }
        }

        public virtual void Update(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmDocumentFormats dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDocumentFormats(_dataAccess);
                if (_dataAccess == null) { dbo._dataAccess.BeginTransaction(); }
                dbo.SM_DOCUMENT_FORMATS_Update(this.Docformatid, this.DocumentFormat, this.ImportPath, this.ExportPath, this.CreatedDate, this.UpdatedDate, this.AddrType);
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
                        dbo._dataAccess._Dispose();
                    }
                }
            }
        }


        //added by kalpita on 17/01/2018
        public static System.Data.DataSet Get_DocumentFormat_AddrType(string cAddrType)
        {
            MetroLesMonitor.Dal.SmDocumentFormats dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDocumentFormats();
                return dbo.SM_DOCUMENT_FORMATS_By_AddrType(cAddrType);
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

        public static string CheckExistingDocFormat(string FORMAT)
        {
            string cExist = "";
            MetroLesMonitor.Dal.SmDocumentFormats dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDocumentFormats();
                System.Data.DataSet ds = dbo.SM_DOCUMENT_FORMATS_By_Format(FORMAT);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cExist = Convert.ToString(ds.Tables[0].Rows[0]["DOCFORMATID"]);
                    }
                }
                return cExist;
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

        public static System.Data.DataSet Get_DocumentFormat_By_Format(string FORMAT)
        {
            MetroLesMonitor.Dal.SmDocumentFormats dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDocumentFormats();
                return dbo.SM_DOCUMENT_FORMATS_By_Format(FORMAT);
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

        public static System.Data.DataSet GetAllDocumentFormats()
        {
            MetroLesMonitor.Dal.SmDocumentFormats dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDocumentFormats();
                System.Data.DataSet ds = dbo.SM_DOCUMENT_FORMATS_Select_All(); System.Data.DataSet dsDocFrmt = new System.Data.DataSet();
                if (GlobalTools.IsSafeDataSet(ds)) { dsDocFrmt = ds; }
                return dsDocFrmt;
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
