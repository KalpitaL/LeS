using System.Data;
namespace MetroLesMonitor.Bll {

    public partial class SmBuyerSupplierLink
    {
        #region
        private System.Nullable<int> _linkid;

        private System.Nullable<int> _buyerid;

        private System.Nullable<int> _supplierid;

        private string _buyerLinkCode;

        private string _vendorLinkCode;

        private string _buyerFormat;

        private string _vendorFormat;

        private string _suppSenderCode;

        private string _suppReceiverCode;

        private string _byrSenderCode;

        private string _byrReceiverCode;

        private string _buyerExportFormat;

        private string _supplierExportFormat;

        private string _buyerMapping;

        private string _supplierMapping;

        private string _company;

        private string _UploadFileType;

        private System.Nullable<short> _importRfq;

        private System.Nullable<short> _exportRfq;

        private System.Nullable<short> _exportRfqAck;

        private System.Nullable<short> _importQuote;

        private System.Nullable<short> _exportQuote;

        private System.Nullable<short> _importPo;

        private System.Nullable<short> _exportPo;

        private System.Nullable<short> _exportPoAck;

        private System.Nullable<short> _exportPoc;

        private string _exportPath;

        private string _importPath;

        private string _suppExportPath;

        private string _suppImportPath;

        private System.Nullable<short> _notifyBuyer;

        private System.Nullable<short> _notifySupplr;

        private System.Nullable<float> _defaultPrice;

        private System.Nullable<short> _isActive;

        private string _replyEmail;

        private string _buyerContact;

        private string _supplierContact;

        private string _buyerEmail;

        private string _supplierEmail;

        private string _ccEmail;

        private string _bccEmail;

        private string _mailSubject;

        private string _errNotifyEmail;

        private System.Nullable<int> _groupId;

        private SmAddress _smAddress;

        private SmAddress _smAddress2;

        private string _supp_web_service_url;

        private System.Nullable<short> _importPoc;//added by kalpita on 26/12/2017

        public virtual System.Nullable<int> Linkid
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


        public virtual System.Nullable<int> BuyerId
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

        public virtual System.Nullable<int> SupplierId
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
        //

        public virtual string BuyerLinkCode
        {
            get
            {
                return _buyerLinkCode;
            }
            set
            {
                _buyerLinkCode = value;
            }
        }

        public virtual string VendorLinkCode
        {
            get
            {
                return _vendorLinkCode;
            }
            set
            {
                _vendorLinkCode = value;
            }
        }

        public virtual string BuyerFormat
        {
            get
            {
                return _buyerFormat;
            }
            set
            {
                _buyerFormat = value;
            }
        }

        public virtual string VendorFormat
        {
            get
            {
                return _vendorFormat;
            }
            set
            {
                _vendorFormat = value;
            }
        }

        public virtual string SuppSenderCode
        {
            get
            {
                return _suppSenderCode;
            }
            set
            {
                _suppSenderCode = value;
            }
        }

        public virtual string SuppReceiverCode
        {
            get
            {
                return _suppReceiverCode;
            }
            set
            {
                _suppReceiverCode = value;
            }
        }

        public virtual string ByrSenderCode
        {
            get
            {
                return _byrSenderCode;
            }
            set
            {
                _byrSenderCode = value;
            }
        }

        public virtual string ByrReceiverCode
        {
            get
            {
                return _byrReceiverCode;
            }
            set
            {
                _byrReceiverCode = value;
            }
        }

        public virtual string BuyerExportFormat
        {
            get
            {
                return _buyerExportFormat;
            }
            set
            {
                _buyerExportFormat = value;
            }
        }

        public virtual string SupplierExportFormat
        {
            get
            {
                return _supplierExportFormat;
            }
            set
            {
                _supplierExportFormat = value;
            }
        }

        public virtual string BuyerMapping
        {
            get
            {
                return _buyerMapping;
            }
            set
            {
                _buyerMapping = value;
            }
        }

        public virtual string SupplierMapping
        {
            get
            {
                return _supplierMapping;
            }
            set
            {
                _supplierMapping = value;
            }
        }

        public virtual string Company
        {
            get
            {
                return _company;
            }
            set
            {
                _company = value;
            }
        }

        public virtual string UploadFileType
        {
            get
            {
                return _UploadFileType;
            }
            set
            {
                _UploadFileType = value;
            }
        }

        public virtual System.Nullable<short> ImportRfq
        {
            get
            {
                return _importRfq;
            }
            set
            {
                _importRfq = value;
            }
        }

        public virtual System.Nullable<short> ExportRfq
        {
            get
            {
                return _exportRfq;
            }
            set
            {
                _exportRfq = value;
            }
        }

        public virtual System.Nullable<short> ExportRfqAck
        {
            get
            {
                return _exportRfqAck;
            }
            set
            {
                _exportRfqAck = value;
            }
        }

        public virtual System.Nullable<short> ImportQuote
        {
            get
            {
                return _importQuote;
            }
            set
            {
                _importQuote = value;
            }
        }

        public virtual System.Nullable<short> ExportQuote
        {
            get
            {
                return _exportQuote;
            }
            set
            {
                _exportQuote = value;
            }
        }

        public virtual System.Nullable<short> ImportPo
        {
            get
            {
                return _importPo;
            }
            set
            {
                _importPo = value;
            }
        }

        public virtual System.Nullable<short> ExportPo
        {
            get
            {
                return _exportPo;
            }
            set
            {
                _exportPo = value;
            }
        }

        public virtual System.Nullable<short> ExportPoAck
        {
            get
            {
                return _exportPoAck;
            }
            set
            {
                _exportPoAck = value;
            }
        }

        public virtual System.Nullable<short> ExportPoc
        {
            get
            {
                return _exportPoc;
            }
            set
            {
                _exportPoc = value;
            }
        }

        public virtual string ExportPath
        {
            get
            {
                return _exportPath;
            }
            set
            {
                _exportPath = value;
            }
        }

        public virtual string ImportPath
        {
            get
            {
                return _importPath;
            }
            set
            {
                _importPath = value;
            }
        }

        public virtual string SuppExportPath
        {
            get
            {
                return _suppExportPath;
            }
            set
            {
                _suppExportPath = value;
            }
        }

        public virtual string SuppImportPath
        {
            get
            {
                return _suppImportPath;
            }
            set
            {
                _suppImportPath = value;
            }
        }

        public virtual System.Nullable<short> NotifyBuyer
        {
            get
            {
                return _notifyBuyer;
            }
            set
            {
                _notifyBuyer = value;
            }
        }

        public virtual System.Nullable<short> NotifySupplr
        {
            get
            {
                return _notifySupplr;
            }
            set
            {
                _notifySupplr = value;
            }
        }

        public virtual System.Nullable<float> DefaultPrice
        {
            get
            {
                return _defaultPrice;
            }
            set
            {
                _defaultPrice = value;
            }
        }

        public virtual System.Nullable<short> IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
            }
        }

        public virtual string ReplyEmail
        {
            get
            {
                return _replyEmail;
            }
            set
            {
                _replyEmail = value;
            }
        }

        public virtual string BuyerContact
        {
            get
            {
                return _buyerContact;
            }
            set
            {
                _buyerContact = value;
            }
        }

        public virtual string SupplierContact
        {
            get
            {
                return _supplierContact;
            }
            set
            {
                _supplierContact = value;
            }
        }

        public virtual string BuyerEmail
        {
            get
            {
                return _buyerEmail;
            }
            set
            {
                _buyerEmail = value;
            }
        }

        public virtual string SupplierEmail
        {
            get
            {
                return _supplierEmail;
            }
            set
            {
                _supplierEmail = value;
            }
        }

        public virtual string CcEmail
        {
            get
            {
                return _ccEmail;
            }
            set
            {
                _ccEmail = value;
            }
        }

        public virtual string BccEmail
        {
            get
            {
                return _bccEmail;
            }
            set
            {
                _bccEmail = value;
            }
        }

        public virtual string MailSubject
        {
            get
            {
                return _mailSubject;
            }
            set
            {
                _mailSubject = value;
            }
        }

        public virtual string ErrNotifyEmail
        {
            get
            {
                return _errNotifyEmail;
            }
            set
            {
                _errNotifyEmail = value;
            }
        }

        public virtual System.Nullable<int> GroupId
        {
            get
            {
                return _groupId;
            }
            set
            {
                _groupId = value;
            }
        }

        public virtual string Supp_Web_Service_Url 
        {
            get
            {
                return _supp_web_service_url;
            }
            set
            {
                _supp_web_service_url = value;
            }
        }

        public virtual SmAddress BuyerAddress
        {
            get
            {
                if ((this._smAddress == null))
                {
                    this._smAddress = MetroLesMonitor.Bll.SmAddress.Load(this._buyerid);
                }
                return this._smAddress;
            }
            set
            {
                _smAddress = value;
                if (_smAddress != null) this._buyerid = _smAddress.Addressid;
            }
        }

        public virtual SmAddress VendorAddress
        {
            get
            {
                if ((this._smAddress2 == null))
                {
                    this._smAddress2 = MetroLesMonitor.Bll.SmAddress.Load(this._supplierid);
                }
                return this._smAddress2;
            }
            set
            {
                _smAddress2 = value;
                if (_smAddress2 != null) this._supplierid = _smAddress2.Addressid;
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
        #endregion

        private void Clean()
        {
            this.Linkid = null;
            this._buyerid = null;
            this._supplierid = null;
            this.BuyerId = null;
            this.SupplierId = null;
            this.BuyerLinkCode = string.Empty;
            this.VendorLinkCode = string.Empty;
            this.BuyerFormat = string.Empty;
            this.VendorFormat = string.Empty;
            this.SuppSenderCode = string.Empty;
            this.SuppReceiverCode = string.Empty;
            this.ByrSenderCode = string.Empty;
            this.ByrReceiverCode = string.Empty;
            this.BuyerExportFormat = string.Empty;
            this.BuyerMapping = string.Empty;
            this.SupplierMapping = string.Empty;
            this.Company = string.Empty;
            this.UploadFileType = string.Empty;
            this.ImportRfq = null;
            this.ExportRfq = null;
            this.ExportRfqAck = null;
            this.ImportQuote = null;
            this.ExportQuote = null;
            this.ImportPo = null;
            this.ExportPo = null;
            this.ExportPoAck = null;
            this.ExportPoc = null;
            this.ExportPath = string.Empty;
            this.ImportPath = string.Empty;
            this.NotifyBuyer = null;
            this.NotifySupplr = null;
            this.DefaultPrice = null;
            this.IsActive = null;
            this.ReplyEmail = string.Empty;
            this.BuyerContact = string.Empty;
            this.SupplierContact = string.Empty;
            this.BuyerEmail = string.Empty;
            this.SupplierEmail = string.Empty;
            this.CcEmail = string.Empty;
            this.BccEmail = string.Empty;
            this.MailSubject = string.Empty;
            this.ErrNotifyEmail = string.Empty;
            this.GroupId = null;
            this.BuyerAddress = null;
            this.VendorAddress = null;
            this.Supp_Web_Service_Url = string.Empty;
            this.ImportPoc = null;
        }

        private void Fill(System.Data.DataRow dr)
        {
            this.Clean();
            if ((dr["LINKID"] != System.DBNull.Value))
            {
                this.Linkid = ((System.Nullable<int>)(dr["LINKID"]));
            }
            if ((dr["BUYERID"] != System.DBNull.Value))
            {
                this.BuyerId = ((System.Nullable<int>)(dr["BUYERID"])); 
            }
            if ((dr["SUPPLIERID"] != System.DBNull.Value))
            {
                this.SupplierId = ((System.Nullable<int>)(dr["SUPPLIERID"])); 
            }
            if ((dr["BUYER_LINK_CODE"] != System.DBNull.Value))
            {
                this.BuyerLinkCode = ((string)(dr["BUYER_LINK_CODE"]));
            }
            if ((dr["VENDOR_LINK_CODE"] != System.DBNull.Value))
            {
                this.VendorLinkCode = ((string)(dr["VENDOR_LINK_CODE"]));
            }
            if ((dr["BUYER_FORMAT"] != System.DBNull.Value))
            {
                this.BuyerFormat = ((string)(dr["BUYER_FORMAT"]));
            }
            if ((dr["VENDOR_FORMAT"] != System.DBNull.Value))
            {
                this.VendorFormat = ((string)(dr["VENDOR_FORMAT"]));
            }
            if ((dr["SUPP_SENDER_CODE"] != System.DBNull.Value))
            {
                this.SuppSenderCode = ((string)(dr["SUPP_SENDER_CODE"]));
            }
            if ((dr["SUPP_RECEIVER_CODE"] != System.DBNull.Value))
            {
                this.SuppReceiverCode = ((string)(dr["SUPP_RECEIVER_CODE"]));
            }
            if ((dr["BYR_SENDER_CODE"] != System.DBNull.Value))
            {
                this.ByrSenderCode = ((string)(dr["BYR_SENDER_CODE"]));
            }
            if ((dr["BYR_RECEIVER_CODE"] != System.DBNull.Value))
            {
                this.ByrReceiverCode = ((string)(dr["BYR_RECEIVER_CODE"]));
            }
            if ((dr["BUYER_EXPORT_FORMAT"] != System.DBNull.Value))
            {
                this.BuyerExportFormat = ((string)(dr["BUYER_EXPORT_FORMAT"]));
            }
            if ((dr["BUYER_MAPPING"] != System.DBNull.Value))
            {
                this.BuyerMapping = ((string)(dr["BUYER_MAPPING"]));
            }
            if ((dr["SUPPLIER_MAPPING"] != System.DBNull.Value))
            {
                this.SupplierMapping = ((string)(dr["SUPPLIER_MAPPING"]));
            }
            if ((dr["Company"] != System.DBNull.Value))
            {
                this.Company = ((string)(dr["Company"]));
            }
            if ((dr["UPLOAD_FILE_TYPE"] != System.DBNull.Value))
            {
                this.UploadFileType = ((string)(dr["UPLOAD_FILE_TYPE"]));
            }
            if ((dr["IMPORT_RFQ"] != System.DBNull.Value))
            {
                this.ImportRfq = ((System.Nullable<short>)(dr["IMPORT_RFQ"]));
            }
            if ((dr["EXPORT_RFQ"] != System.DBNull.Value))
            {
                this.ExportRfq = ((System.Nullable<short>)(dr["EXPORT_RFQ"]));
            }
            if ((dr["EXPORT_RFQ_ACK"] != System.DBNull.Value))
            {
                this.ExportRfqAck = ((System.Nullable<short>)(dr["EXPORT_RFQ_ACK"]));
            }
            if ((dr["IMPORT_QUOTE"] != System.DBNull.Value))
            {
                this.ImportQuote = ((System.Nullable<short>)(dr["IMPORT_QUOTE"]));
            }
            if ((dr["EXPORT_QUOTE"] != System.DBNull.Value))
            {
                this.ExportQuote = ((System.Nullable<short>)(dr["EXPORT_QUOTE"]));
            }
            if ((dr["IMPORT_PO"] != System.DBNull.Value))
            {
                this.ImportPo = ((System.Nullable<short>)(dr["IMPORT_PO"]));
            }
            if ((dr["EXPORT_PO"] != System.DBNull.Value))
            {
                this.ExportPo = ((System.Nullable<short>)(dr["EXPORT_PO"]));
            }
            if ((dr["EXPORT_PO_ACK"] != System.DBNull.Value))
            {
                this.ExportPoAck = ((System.Nullable<short>)(dr["EXPORT_PO_ACK"]));
            }
            if ((dr["EXPORT_POC"] != System.DBNull.Value))
            {
                this.ExportPoc = ((System.Nullable<short>)(dr["EXPORT_POC"]));
            }
            if ((dr["EXPORT_PATH"] != System.DBNull.Value))
            {
                this.ExportPath = ((string)(dr["EXPORT_PATH"]));
            }
            if ((dr["IMPORT_PATH"] != System.DBNull.Value))
            {
                this.ImportPath = ((string)(dr["IMPORT_PATH"]));
            }
            if ((dr["SUPP_EXPORT_PATH"] != System.DBNull.Value))
            {
                this.SuppExportPath = ((string)(dr["SUPP_EXPORT_PATH"]));
            }
            if ((dr["SUPP_IMPORT_PATH"] != System.DBNull.Value))
            {
                this.SuppImportPath = ((string)(dr["SUPP_IMPORT_PATH"]));
            }
            if ((dr["NOTIFY_BUYER"] != System.DBNull.Value))
            {
                this.NotifyBuyer = ((System.Nullable<short>)(dr["NOTIFY_BUYER"]));
            }
            if ((dr["NOTIFY_SUPPLR"] != System.DBNull.Value))
            {
                this.NotifySupplr = ((System.Nullable<short>)(dr["NOTIFY_SUPPLR"]));
            }
            if ((dr["DEFAULT_PRICE"] != System.DBNull.Value))
            {
                this.DefaultPrice = (System.Convert.ToSingle(dr["DEFAULT_PRICE"]));
            }
            if ((dr["IS_ACTIVE"] != System.DBNull.Value))
            {
                this.IsActive = ((System.Nullable<short>)(dr["IS_ACTIVE"]));
            }
            if ((dr["REPLY_EMAIL"] != System.DBNull.Value))
            {
                this.ReplyEmail = ((string)(dr["REPLY_EMAIL"]));
            }
            if ((dr["BUYER_CONTACT"] != System.DBNull.Value))
            {
                this.BuyerContact = ((string)(dr["BUYER_CONTACT"]));
            }
            if ((dr["SUPPLIER_CONTACT"] != System.DBNull.Value))
            {
                this.SupplierContact = ((string)(dr["SUPPLIER_CONTACT"]));
            }
            if ((dr["BUYER_EMAIL"] != System.DBNull.Value))
            {
                this.BuyerEmail = ((string)(dr["BUYER_EMAIL"]));
            }
            if ((dr["SUPPLIER_EMAIL"] != System.DBNull.Value))
            {
                this.SupplierEmail = ((string)(dr["SUPPLIER_EMAIL"]));
            }
            if ((dr["CC_EMAIL"] != System.DBNull.Value))
            {
                this.CcEmail = ((string)(dr["CC_EMAIL"]));
            }
            if ((dr["BCC_EMAIL"] != System.DBNull.Value))
            {
                this.BccEmail = ((string)(dr["BCC_EMAIL"]));
            }
            if ((dr["MAIL_SUBJECT"] != System.DBNull.Value))
            {
                this.MailSubject = ((string)(dr["MAIL_SUBJECT"]));
            }
            if ((dr["ERR_NOTIFY_EMAIL"] != System.DBNull.Value))
            {
                this.ErrNotifyEmail = ((string)(dr["ERR_NOTIFY_EMAIL"]));
            }
            if ((dr["GROUP_ID"] != System.DBNull.Value))
            {
                this.GroupId = ((System.Nullable<int>)(dr["GROUP_ID"]));
            }
            if ((dr["SUP_WEB_SERVICE_URL"] != System.DBNull.Value))
            {
                this.Supp_Web_Service_Url = ((string)(dr["SUP_WEB_SERVICE_URL"]));  
            }
            if ((dr["IMPORT_POC"] != System.DBNull.Value))
            {
                this.ImportPoc = ((System.Nullable<short>)(dr["IMPORT_POC"]));
            }
        }

        public static SmBuyerSupplierLinkCollection Select_SM_BUYER_SUPPLIER_LINKs_By_GROUPID(System.Nullable<int> GROUP_ID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink();
                System.Data.DataSet ds = dbo.Load_By_Group(GROUP_ID);
                SmBuyerSupplierLinkCollection collection = new SmBuyerSupplierLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmBuyerSupplierLink obj = new SmBuyerSupplierLink();
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

        public static DataSet Select_SMV_BUYER_SUPPLIER_LINKs_By_GROUPID(System.Nullable<int> GROUP_ID)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            DataSet _ds = new DataSet();
            try
            {
                _dataAccess.CreateSQLCommand(" SELECT * FROM SMV_BUYER_SUPPLIER_LINK WHERE GROUP_ID =" + GROUP_ID);
                System.Data.DataSet ds = _dataAccess.ExecuteDataSet();
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

            }
        }

        public static DataSet Select_SMV_BUYER_SUPPLIER_LINKs_By_RuleId(System.Nullable<int> RuleId)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            DataSet _ds = new DataSet();
            try
            {
                _dataAccess.CreateSQLCommand("select * from SMV_BUYER_SUPPLIER_LINK as A join SMV_BUYER_SUPPLIER_LINK_RULE as B on A.LINKID = B.LINKID Where B. RULEID =" + RuleId);
                System.Data.DataSet ds = _dataAccess.ExecuteDataSet();
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

            }
        }

        public static SmBuyerSupplierLinkCollection Select_SM_BUYER_SUPPLIER_LINKs_By_BUYERID(System.Nullable<int> BUYERID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink();
                System.Data.DataSet ds = dbo.Select_SM_BUYER_SUPPLIER_LINKs_By_BUYERID(BUYERID);
                SmBuyerSupplierLinkCollection collection = new SmBuyerSupplierLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmBuyerSupplierLink obj = new SmBuyerSupplierLink();
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

        public static SmBuyerSupplierLinkCollection Select_SM_BUYER_SUPPLIER_LINKs_By_SUPPLIERID(System.Nullable<int> SUPPLIERID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink();
                System.Data.DataSet ds = dbo.Select_SM_BUYER_SUPPLIER_LINKs_By_SUPPLIERID(SUPPLIERID);
                SmBuyerSupplierLinkCollection collection = new SmBuyerSupplierLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmBuyerSupplierLink obj = new SmBuyerSupplierLink();
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

        public static SmBuyerSupplierLinkCollection Select_SM_BUYER_SUPPLIER_LINKs_By_BUYERID_SUPPLIERID(System.Nullable<int> BUYERID, System.Nullable<int> SUPPLIERID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink();
                System.Data.DataSet ds = dbo.Load_By_BuyerSupplierID(BUYERID, SUPPLIERID);
                SmBuyerSupplierLinkCollection collection = new SmBuyerSupplierLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmBuyerSupplierLink obj = new SmBuyerSupplierLink();
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

        public static SmBuyerSupplierLinkCollection Select_SM_BUYER_SUPPLIER_LINKs_By_GROUP_CODE(string GROUP_CODE)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                _dataAccess.CreateSQLCommand(" SELECT * FROM SM_BUYER_SUPPLIER_LINK WHERE GROUP_ID IN (SELECT GROUP_ID FROM SM_BUYER_SUPPLIER_GROUPS WHERE GROUP_CODE LIKE '" + GROUP_CODE + "%') ");
                System.Data.DataSet ds = _dataAccess.ExecuteDataSet();

                SmBuyerSupplierLinkCollection collection = new SmBuyerSupplierLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmBuyerSupplierLink obj = new SmBuyerSupplierLink();
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
                if ((_dataAccess != null))
                {
                    _dataAccess._Dispose();
                }
            }
        }

        public static SmBuyerSupplierLinkCollection GetAll()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_LINK_Select_All();
                SmBuyerSupplierLinkCollection collection = new SmBuyerSupplierLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmBuyerSupplierLink obj = new SmBuyerSupplierLink();
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

        public static SmBuyerSupplierLink Load(System.Nullable<int> LINKID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_LINK_Select_One(LINKID);
                SmBuyerSupplierLink obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmBuyerSupplierLink();
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

        public virtual void Load_byBuyerSupplierID()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink();
                System.Data.DataSet ds = dbo.Load_By_BuyerSupplierID(this._buyerid, this._supplierid);
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

        public virtual void Load_byBuyerSupplierGroup()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink();
                System.Data.DataSet ds = dbo.Load_By_BuyerSupplierGroup(this._buyerid, this._supplierid, this.GroupId);
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

        public virtual void Load_byGroup(int GroupID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink();
                System.Data.DataSet ds = dbo.Load_By_Group(GroupID);
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

        public virtual void Load_byBuyerSupplierFormat()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink();
                System.Data.DataSet ds = dbo.Load_By_BuyerSupplierFormat(this._buyerid, this._supplierid, this.BuyerFormat, this.VendorFormat);
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

        public virtual void Load_byBuyerSupplier()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink();
                System.Data.DataSet ds = dbo.Load_By_BuyerSupplierFormat(this._buyerid, this._supplierid);
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

        public static SmBuyerSupplierLink Load_byBuyerSupplierLinkCode(string BUYERLINKCODE, string SUPPLINKCODE)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink();
                System.Data.DataSet ds = dbo.Load_By_BuyerSupplierLinkCode(BUYERLINKCODE, SUPPLINKCODE);
                SmBuyerSupplierLink obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmBuyerSupplierLink();
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
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_LINK_Select_One(this.Linkid);
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
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink();
                this.Linkid = dbo.SM_BUYER_SUPPLIER_LINK_Insert(this._buyerid, this._supplierid, this.BuyerLinkCode, this.VendorLinkCode, this.BuyerFormat, this.VendorFormat, this.SuppSenderCode, this.SuppReceiverCode, this.ByrSenderCode, this.ByrReceiverCode, this.BuyerExportFormat, this.SupplierExportFormat, this.BuyerMapping, this.SupplierMapping, this.Company, this.ImportRfq, this.ExportRfq, this.ExportRfqAck, this.ImportQuote, this.ExportQuote, this.ImportPo, this.ExportPo, this.ExportPoAck, this.ExportPoc, this.ExportPath, this.ImportPath, this.NotifyBuyer, this.NotifySupplr, this.DefaultPrice, this.IsActive, this.ReplyEmail, this.BuyerContact, this.SupplierContact, this.BuyerEmail, this.SupplierEmail, this.CcEmail, this.BccEmail, this.MailSubject, this.ErrNotifyEmail, this.GroupId, this.UploadFileType, this.SuppImportPath, this.SuppExportPath, this.Supp_Web_Service_Url, this.ImportPoc); 
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
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink(_dataAccess);
                this.Linkid = dbo.SM_BUYER_SUPPLIER_LINK_Insert(this._buyerid, this._supplierid, this.BuyerLinkCode, this.VendorLinkCode, this.BuyerFormat, this.VendorFormat, 
                    this.SuppSenderCode, this.SuppReceiverCode, this.ByrSenderCode, this.ByrReceiverCode, this.BuyerExportFormat, this.SupplierExportFormat, 
                    this.BuyerMapping, this.SupplierMapping, this.Company, this.ImportRfq, this.ExportRfq, this.ExportRfqAck, this.ImportQuote, this.ExportQuote, 
                    this.ImportPo, this.ExportPo, this.ExportPoAck, this.ExportPoc, this.ExportPath, this.ImportPath, this.NotifyBuyer, this.NotifySupplr, this.DefaultPrice, 
                    this.IsActive, this.ReplyEmail, this.BuyerContact, this.SupplierContact, this.BuyerEmail, this.SupplierEmail, this.CcEmail, this.BccEmail, this.MailSubject,
                    this.ErrNotifyEmail, this.GroupId, this.UploadFileType, this.SuppImportPath, this.SuppExportPath, this.Supp_Web_Service_Url,this.ImportPoc);  
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public virtual void Delete()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink();
                dbo.SM_BUYER_SUPPLIER_LINK_Delete(this.Linkid);
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
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink(_dataAccess);
                dbo.SM_BUYER_SUPPLIER_LINK_Delete(this.Linkid);
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                //
            }
        }

        public virtual void Update()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink();
                dbo.SM_BUYER_SUPPLIER_LINK_Update(this.Linkid, this._buyerid, this._supplierid, this.BuyerLinkCode, this.VendorLinkCode, this.BuyerFormat, this.VendorFormat, this.SuppSenderCode, this.SuppReceiverCode, this.ByrSenderCode, this.ByrReceiverCode, this.BuyerExportFormat, this.SupplierExportFormat, this.BuyerMapping, this.SupplierMapping, this.Company, this.ImportRfq, this.ExportRfq, this.ExportRfqAck, this.ImportQuote, this.ExportQuote, this.ImportPo, this.ExportPo, this.ExportPoAck, this.ExportPoc, this.ExportPath, this.ImportPath, this.NotifyBuyer, this.NotifySupplr, this.DefaultPrice, this.IsActive, this.ReplyEmail, this.BuyerContact, this.SupplierContact, this.BuyerEmail, this.SupplierEmail, this.CcEmail, this.BccEmail, this.MailSubject, this.ErrNotifyEmail, this.GroupId, this.UploadFileType, this.SuppImportPath, this.SuppExportPath, this.Supp_Web_Service_Url, this.ImportPoc); 
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
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink(_dataAccess);
                dbo.SM_BUYER_SUPPLIER_LINK_Update(this.Linkid, this._buyerid, this._supplierid, this.BuyerLinkCode, this.VendorLinkCode, this.BuyerFormat, this.VendorFormat, this.SuppSenderCode, this.SuppReceiverCode, this.ByrSenderCode, this.ByrReceiverCode, this.BuyerExportFormat, this.SupplierExportFormat, this.BuyerMapping, this.SupplierMapping, this.Company, this.ImportRfq, this.ExportRfq, this.ExportRfqAck, this.ImportQuote, this.ExportQuote, this.ImportPo, this.ExportPo, this.ExportPoAck, this.ExportPoc, this.ExportPath, this.ImportPath, this.NotifyBuyer, this.NotifySupplr, this.DefaultPrice, this.IsActive, this.ReplyEmail, this.BuyerContact, this.SupplierContact, this.BuyerEmail, this.SupplierEmail, this.CcEmail, this.BccEmail, this.MailSubject, this.ErrNotifyEmail, this.GroupId, this.UploadFileType, this.SuppImportPath, this.SuppExportPath, this.Supp_Web_Service_Url, this.ImportPoc); 
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                //
            }
        }

        //ADDED BY KALPITA ON 23/02/2018
        public static DataSet GetBuyers_Format(string FORMAT)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink();
                return dbo.LoadBuyers_by_Format(FORMAT);
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

        public static SmBuyerSupplierLink Load_new(System.Nullable<int> LINKID, Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLink(_dataAccess);
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_LINK_Select_One(LINKID);
                SmBuyerSupplierLink obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmBuyerSupplierLink();
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
