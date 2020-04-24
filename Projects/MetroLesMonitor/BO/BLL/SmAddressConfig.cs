using System;
using System.Data;
namespace MetroLesMonitor.Bll {
        
    public partial class SmAddressConfig {
        
        private System.Nullable<int> _addrconfigid;
        
        private System.Nullable<int> _addressid;
        
        private string _partyMapping;       
        
        private string _defaultFormat;
        
        private string _ExportPath;
        
        private string _ImportPath;
        
        private System.Nullable<short> _importRfq;
        
        private System.Nullable<short> _exportRfq;
        
        private System.Nullable<short> _exportRfqAck;
        
        private System.Nullable<short> _importQuote;
        
        private System.Nullable<short> _exportQuote;
        
        private System.Nullable<short> _importPo;
        
        private System.Nullable<short> _exportPo;
        
        private System.Nullable<short> _exportPoAck;
        
        private System.Nullable<short> _exportPoc;

        private System.Nullable<short> _mailNotify;        
        
        private System.Nullable<float> _defaultPrice;
        
        private string _uploadFileType;
        
        private string _mailSubject;
        
        private string _supWebServiceUrl;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        private string _ccEmail;

        private string _identificationCode;

        private System.Nullable<short> _importPoc;

        private string _byrSenderCode;
        
        private SmAddress _smAddress;
        
        public virtual System.Nullable<int> Addrconfigid {
            get {
                return _addrconfigid;
            }
            set {
                _addrconfigid = value;
            }
        }
        
        public virtual string PartyMapping {
            get
            {
                return _partyMapping;
            }
            set {
                _partyMapping = value;
            }
        }
        
        public virtual string DefaultFormat {
            get {
                return _defaultFormat;
            }
            set {
                _defaultFormat = value;
            }
        }
                
        public virtual string ExportPath {
            get {
                return _ExportPath;
            }
            set {
                _ExportPath = value;
            }
        }
        
        public virtual string ImportPath {
            get {
                return _ImportPath;
            }
            set {
                _ImportPath = value;
            }
        }
        
        public virtual System.Nullable<short> ImportRfq {
            get {
                return _importRfq;
            }
            set {
                _importRfq = value;
            }
        }
        
        public virtual System.Nullable<short> ExportRfq {
            get {
                return _exportRfq;
            }
            set {
                _exportRfq = value;
            }
        }
        
        public virtual System.Nullable<short> ExportRfqAck {
            get {
                return _exportRfqAck;
            }
            set {
                _exportRfqAck = value;
            }
        }
        
        public virtual System.Nullable<short> ImportQuote {
            get {
                return _importQuote;
            }
            set {
                _importQuote = value;
            }
        }
        
        public virtual System.Nullable<short> ExportQuote {
            get {
                return _exportQuote;
            }
            set {
                _exportQuote = value;
            }
        }
        
        public virtual System.Nullable<short> ImportPo {
            get {
                return _importPo;
            }
            set {
                _importPo = value;
            }
        }
        
        public virtual System.Nullable<short> ExportPo {
            get {
                return _exportPo;
            }
            set {
                _exportPo = value;
            }
        }
        
        public virtual System.Nullable<short> ExportPoAck {
            get {
                return _exportPoAck;
            }
            set {
                _exportPoAck = value;
            }
        }
        
        public virtual System.Nullable<short> ExportPoc {
            get {
                return _exportPoc;
            }
            set {
                _exportPoc = value;
            }
        }
        
        public virtual System.Nullable<short> MailNotify {
            get {
                return _mailNotify;
            }
            set {
                _mailNotify = value;
            }
        }

        public virtual System.Nullable<float> DefaultPrice {
            get {
                return _defaultPrice;
            }
            set {
                _defaultPrice = value;
            }
        }
        
        public virtual string UploadFileType {
            get {
                return _uploadFileType;
            }
            set {
                _uploadFileType = value;
            }
        }
        
        public virtual string MailSubject {
            get {
                return _mailSubject;
            }
            set {
                _mailSubject = value;
            }
        }
        
        public virtual string SupWebServiceUrl {
            get {
                return _supWebServiceUrl;
            }
            set {
                _supWebServiceUrl = value;
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
        
        public virtual System.Nullable<System.DateTime> UpdateDate {
            get {
                return _updateDate;
            }
            set {
                _updateDate = value;
            }
        }
        
        public virtual string CcEmail {
            get {
                return _ccEmail;
            }
            set {
                _ccEmail = value;
            }
        }

        public virtual string IdentificationCode
        {
            get
            {
                return _identificationCode;
            }
            set
            {
                _identificationCode = value;
            }
        }

        public virtual System.Nullable<short> ImportPoc
        {
            get
            {
                return _importPoc;
            }
            set
            {
                _importPoc = value;
            }
        }
   
        public virtual SmAddress SmAddress {
            get {
                if ((this._smAddress == null)) {
                    this._smAddress = MetroLesMonitor.Bll.SmAddress.Load(this._addressid);
                }
                return this._smAddress;
            }
            set {
                _smAddress = value;
            }
        }
        
        private void Clean() {
            this.Addrconfigid = null;
            this._addressid = null;
            this.PartyMapping = string.Empty;
            this.DefaultFormat = string.Empty;
            this.ExportPath = string.Empty;
            this.ImportPath = string.Empty;
            this.ImportRfq = null;
            this.ExportRfq = null;
            this.ExportRfqAck = null;
            this.ImportQuote = null;
            this.ExportQuote = null;
            this.ImportPo = null;
            this.ExportPo = null;
            this.ExportPoAck = null;
            this.ExportPoc = null;
            this.MailNotify = null;
            this.DefaultPrice = null;
            this.UploadFileType = string.Empty;
            this.MailSubject = string.Empty;
            this.SupWebServiceUrl = string.Empty;
            this.CreatedDate = null;
            this.UpdateDate = null;
            this.CcEmail = string.Empty;
            this.IdentificationCode = string.Empty;
            this.SmAddress = null;
            this.ImportPoc = null;
        }

        private void Fill(System.Data.DataRow dr)
        {
            this.Clean();
            if ((dr["ADDRCONFIGID"] != System.DBNull.Value))
            {
                this.Addrconfigid = ((System.Nullable<int>)(dr["ADDRCONFIGID"]));
            }
            if ((dr["ADDRESSID"] != System.DBNull.Value))
            {
                this._addressid = ((System.Nullable<int>)(dr["ADDRESSID"]));
            }
            if ((dr["PARTY_MAPPING"] != System.DBNull.Value))
            {
                this.PartyMapping = ((string)(dr["PARTY_MAPPING"]));
            }
            if ((dr["DEFAULT_FORMAT"] != System.DBNull.Value))
            {
                this.DefaultFormat = ((string)(dr["DEFAULT_FORMAT"]));
            }
            if ((dr["EXPORT_PATH"] != System.DBNull.Value))
            {
                this.ExportPath = ((string)(dr["EXPORT_PATH"]));
            }
            if ((dr["IMPORT_PATH"] != System.DBNull.Value))
            {
                this.ImportPath = ((string)(dr["IMPORT_PATH"]));
            }
            if ((dr["IMPORT_RFQ"] != System.DBNull.Value))
            {
                this.ImportRfq = Convert.ToInt16(dr["IMPORT_RFQ"]);
            }
            if ((dr["EXPORT_RFQ"] != System.DBNull.Value))
            {
                this.ExportRfq = Convert.ToInt16(dr["EXPORT_RFQ"]);
            }
            if ((dr["EXPORT_RFQ_ACK"] != System.DBNull.Value))
            {
                this.ExportRfqAck = Convert.ToInt16(dr["EXPORT_RFQ_ACK"]);
            }
            if ((dr["IMPORT_QUOTE"] != System.DBNull.Value))
            {
                this.ImportQuote = Convert.ToInt16(dr["IMPORT_QUOTE"]);
            }
            if ((dr["EXPORT_QUOTE"] != System.DBNull.Value))
            {
                this.ExportQuote = Convert.ToInt16(dr["EXPORT_QUOTE"]);
            }
            if ((dr["IMPORT_PO"] != System.DBNull.Value))
            {
                this.ImportPo = Convert.ToInt16(dr["IMPORT_PO"]);
            }
            if ((dr["EXPORT_PO"] != System.DBNull.Value))
            {
                this.ExportPo = Convert.ToInt16(dr["EXPORT_PO"]);
            }
            if ((dr["EXPORT_PO_ACK"] != System.DBNull.Value))
            {
                this.ExportPoAck = Convert.ToInt16(dr["EXPORT_PO_ACK"]);
            }
            if ((dr["EXPORT_POC"] != System.DBNull.Value))
            {
                this.ExportPoc = Convert.ToInt16(dr["EXPORT_POC"]);
            }
            if ((dr["MAIL_NOTIFY"] != System.DBNull.Value))
            {
                this.MailNotify = Convert.ToInt16(dr["MAIL_NOTIFY"]);
            }
            if ((dr["DEFAULT_PRICE"] != System.DBNull.Value))
            {
                this.DefaultPrice = Convert.ToSingle(dr["DEFAULT_PRICE"]);
            }
            if ((dr["UPLOAD_FILE_TYPE"] != System.DBNull.Value))
            {
                this.UploadFileType = ((string)(dr["UPLOAD_FILE_TYPE"]));
            }
            if ((dr["MAIL_SUBJECT"] != System.DBNull.Value))
            {
                this.MailSubject = ((string)(dr["MAIL_SUBJECT"]));
            }
            if ((dr["SUP_WEB_SERVICE_URL"] != System.DBNull.Value))
            {
                this.SupWebServiceUrl = ((string)(dr["SUP_WEB_SERVICE_URL"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value))
            {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value))
            {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
            if ((dr["CC_EMAIL"] != System.DBNull.Value))
            {
                this.CcEmail = ((string)(dr["CC_EMAIL"]));
            }
            if ((dr["IDENTIFICATION_CODE"] != System.DBNull.Value))
            {
                this.IdentificationCode = ((string)(dr["IDENTIFICATION_CODE"]));
            }
            if ((dr["IMPORT_POC"] != System.DBNull.Value))
            {
                this.ImportPoc = Convert.ToInt16(dr["IMPORT_POC"]);
            }
        }
        
        public static SmAddressConfigCollection Select_SM_ADDRESS_CONFIGs_By_ADDRESSID(System.Nullable<int> ADDRESSID) {
            MetroLesMonitor.Dal.SmAddressConfig dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmAddressConfig();
                System.Data.DataSet ds = dbo.Select_SM_ADDRESS_CONFIGs_By_ADDRESSID(ADDRESSID);
                SmAddressConfigCollection collection = new SmAddressConfigCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmAddressConfig obj = new SmAddressConfig();
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
        
        public static SmAddressConfigCollection GetAll() {
            MetroLesMonitor.Dal.SmAddressConfig dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmAddressConfig();
                System.Data.DataSet ds = dbo.SM_ADDRESS_CONFIG_Select_All();
                SmAddressConfigCollection collection = new SmAddressConfigCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmAddressConfig obj = new SmAddressConfig();
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
        
        public static SmAddressConfig Load(System.Nullable<int> ADDRCONFIGID) {
            MetroLesMonitor.Dal.SmAddressConfig dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmAddressConfig();
                System.Data.DataSet ds = dbo.SM_ADDRESS_CONFIG_Select_One(ADDRCONFIGID);
                SmAddressConfig obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmAddressConfig();
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
            MetroLesMonitor.Dal.SmAddressConfig dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmAddressConfig();
                System.Data.DataSet ds = dbo.SM_ADDRESS_CONFIG_Select_One(this.Addrconfigid);
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

        public static SmAddressConfig LoadID_Format(string FORMAT, int ADDRESSID)
        {
            MetroLesMonitor.Dal.SmAddressConfig dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAddressConfig();
                System.Data.DataSet ds = dbo.SM_ADDRESS_CONFIG_By_ID_Format(FORMAT, ADDRESSID);
                SmAddressConfig obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmAddressConfig();
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

        public string CheckExistingConfig(string FORMAT, int ADDRESSID)
        {
            string cExist = "";
            MetroLesMonitor.Dal.SmAddressConfig dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAddressConfig();
                System.Data.DataSet ds = dbo.SM_ADDRESS_CONFIG_By_ID_Format(FORMAT,ADDRESSID);                
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cExist = Convert.ToString(ds.Tables[0].Rows[0]["ADDRCONFIGID"]);
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

        public virtual void Load_ID(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmAddressConfig dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAddressConfig(_dataAccess);
                System.Data.DataSet ds = dbo.SM_ADDRESS_CONFIG_Select_One(this.Addrconfigid);
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
            MetroLesMonitor.Dal.SmAddressConfig dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAddressConfig(_dataAccess);
                if (SmAddress != null) this._addressid = SmAddress.Addressid;
                if (_dataAccess == null) { dbo._dataAccess.BeginTransaction(); }
                dbo.SM_ADDRESS_CONFIG_Insert(this._addressid, this.PartyMapping, this.DefaultFormat, this.ExportPath, this.ImportPath,
                     this.ImportRfq, this.ExportRfq, this.ExportRfqAck, this.ImportQuote, this.ExportQuote, this.ImportPo,
                    this.ExportPo, this.ExportPoAck, this.ExportPoc, this.MailNotify, this.DefaultPrice, this.UploadFileType, this.MailSubject,
                    this.SupWebServiceUrl, this.CreatedDate, this.UpdateDate, this.CcEmail, this.IdentificationCode, this.ImportPoc);
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
            MetroLesMonitor.Dal.SmAddressConfig dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmAddressConfig(_dataAccess);
                dbo.SM_ADDRESS_CONFIG_Delete(this.Addrconfigid);
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

        public virtual void Update(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmAddressConfig dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmAddressConfig(_dataAccess);
                if (_dataAccess == null) { dbo._dataAccess.BeginTransaction(); }
                dbo.SM_ADDRESS_CONFIG_Update(this.Addrconfigid, this._addressid, this.PartyMapping, this.DefaultFormat, this.ExportPath,
                    this.ImportPath,  this.ImportRfq, this.ExportRfq, this.ExportRfqAck, this.ImportQuote, this.ExportQuote,
                    this.ImportPo, this.ExportPo, this.ExportPoAck, this.ExportPoc, this.MailNotify, this.DefaultPrice, this.UploadFileType,
                    this.MailSubject, this.SupWebServiceUrl, this.CreatedDate, this.UpdateDate, this.CcEmail, this.IdentificationCode, this.ImportPoc);
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

        public static DataSet GetConfig_By_ADDRESSID(int ADDRESSID)
        {
            MetroLesMonitor.Dal.SmAddressConfig dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAddressConfig();
                System.Data.DataSet ds = dbo.Select_SM_ADDRESS_CONFIGs_By_ADDRESSID(ADDRESSID);
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

        public static SmAddressConfig Load_AddressID(int ADDRESSID)
        {
            MetroLesMonitor.Dal.SmAddressConfig dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAddressConfig();
                System.Data.DataSet ds = GetConfig_By_ADDRESSID(ADDRESSID);
                SmAddressConfig obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmAddressConfig();
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

        public static DataSet Get_AddressConfig_By_AddressID(int ADDRESSID)
        {
            MetroLesMonitor.Dal.SmAddressConfig dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmAddressConfig();
                return GetConfig_By_ADDRESSID(ADDRESSID);
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
