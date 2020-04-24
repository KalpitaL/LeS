namespace MetroLesMonitor.Bll {
    
    
    public partial class LesSupplierSubsupplierLink {
        
        private System.Nullable<int> _supplinkid;
        
        private System.Nullable<int> _supplierid;
        
        private System.Nullable<int> _subsupplierid;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private System.Nullable<int> _updatedBy;
        
        private System.Nullable<int> _createdBy;
        
        private string _subsupplierEmail;
        
        private string _supplierEmail;
        
        private string _ccEmail;
        
        private string _bccEmail;
        
        private string _mailSubject;
        
        private string _replyEmail;
        
        private System.Nullable<int> _billtoAddressid;
        
        private System.Nullable<int> _shiptoAddressid;
        
        private SmAddress _smAddress;
        
        private SmAddress _smAddress2;
        
        public virtual System.Nullable<int> Supplinkid {
            get {
                return _supplinkid;
            }
            set {
                _supplinkid = value;
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
        
        public virtual System.Nullable<System.DateTime> CreatedDate {
            get {
                return _createdDate;
            }
            set {
                _createdDate = value;
            }
        }
        
        public virtual System.Nullable<int> UpdatedBy {
            get {
                return _updatedBy;
            }
            set {
                _updatedBy = value;
            }
        }
        
        public virtual System.Nullable<int> CreatedBy {
            get {
                return _createdBy;
            }
            set {
                _createdBy = value;
            }
        }
        
        public virtual string SubsupplierEmail {
            get {
                return _subsupplierEmail;
            }
            set {
                _subsupplierEmail = value;
            }
        }
        
        public virtual string SupplierEmail {
            get {
                return _supplierEmail;
            }
            set {
                _supplierEmail = value;
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
        
        public virtual string BccEmail {
            get {
                return _bccEmail;
            }
            set {
                _bccEmail = value;
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
        
        public virtual string ReplyEmail {
            get {
                return _replyEmail;
            }
            set {
                _replyEmail = value;
            }
        }
        
        public virtual System.Nullable<int> BilltoAddressid {
            get {
                return _billtoAddressid;
            }
            set {
                _billtoAddressid = value;
            }
        }
        
        public virtual System.Nullable<int> ShiptoAddressid {
            get {
                return _shiptoAddressid;
            }
            set {
                _shiptoAddressid = value;
            }
        }
        
        public virtual SmAddress SmAddress {
            get {
                if ((this._smAddress == null)) {
                    this._smAddress = MetroLesMonitor.Bll.SmAddress.Load(this._supplierid);
                }
                return this._smAddress;
            }
            set {
                _smAddress = value;
            }
        }
        
        public virtual SmAddress SmAddress2 {
            get {
                if ((this._smAddress2 == null)) {
                    this._smAddress2 = MetroLesMonitor.Bll.SmAddress.Load(this._subsupplierid);
                }
                return this._smAddress2;
            }
            set {
                _smAddress2 = value;
            }
        }
        
        private void Clean() {
            this.Supplinkid = null;
            this._supplierid = null;
            this._subsupplierid = null;
            this.UpdateDate = null;
            this.CreatedDate = null;
            this.UpdatedBy = null;
            this.CreatedBy = null;
            this.SubsupplierEmail = string.Empty;
            this.SupplierEmail = string.Empty;
            this.CcEmail = string.Empty;
            this.BccEmail = string.Empty;
            this.MailSubject = string.Empty;
            this.ReplyEmail = string.Empty;
            this.BilltoAddressid = null;
            this.ShiptoAddressid = null;
            this.SmAddress = null;
            this.SmAddress2 = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["SUPPLINKID"] != System.DBNull.Value)) {
                this.Supplinkid = ((System.Nullable<int>)(dr["SUPPLINKID"]));
            }
            if ((dr["SUPPLIERID"] != System.DBNull.Value)) {
                this._supplierid = ((System.Nullable<int>)(dr["SUPPLIERID"]));
            }
            if ((dr["SUBSUPPLIERID"] != System.DBNull.Value)) {
                this._subsupplierid = ((System.Nullable<int>)(dr["SUBSUPPLIERID"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value)) {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value)) {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["UPDATED_BY"] != System.DBNull.Value)) {
                this.UpdatedBy = ((System.Nullable<int>)(dr["UPDATED_BY"]));
            }
            if ((dr["CREATED_BY"] != System.DBNull.Value)) {
                this.CreatedBy = ((System.Nullable<int>)(dr["CREATED_BY"]));
            }
            if ((dr["SUBSUPPLIER_EMAIL"] != System.DBNull.Value)) {
                this.SubsupplierEmail = ((string)(dr["SUBSUPPLIER_EMAIL"]));
            }
            if ((dr["SUPPLIER_EMAIL"] != System.DBNull.Value)) {
                this.SupplierEmail = ((string)(dr["SUPPLIER_EMAIL"]));
            }
            if ((dr["CC_EMAIL"] != System.DBNull.Value)) {
                this.CcEmail = ((string)(dr["CC_EMAIL"]));
            }
            if ((dr["BCC_EMAIL"] != System.DBNull.Value)) {
                this.BccEmail = ((string)(dr["BCC_EMAIL"]));
            }
            if ((dr["MAIL_SUBJECT"] != System.DBNull.Value)) {
                this.MailSubject = ((string)(dr["MAIL_SUBJECT"]));
            }
            if ((dr["REPLY_EMAIL"] != System.DBNull.Value)) {
                this.ReplyEmail = ((string)(dr["REPLY_EMAIL"]));
            }
            if ((dr["BILLTO_ADDRESSID"] != System.DBNull.Value)) {
                this.BilltoAddressid = ((System.Nullable<int>)(dr["BILLTO_ADDRESSID"]));
            }
            if ((dr["SHIPTO_ADDRESSID"] != System.DBNull.Value)) {
                this.ShiptoAddressid = ((System.Nullable<int>)(dr["SHIPTO_ADDRESSID"]));
            }
        }
        
        public static LesSupplierSubsupplierLinkCollection Select_LES_SUPPLIER_SUBSUPPLIER_LINKs_By_SUPPLIERID(System.Nullable<int> SUPPLIERID) {
            MetroLesMonitor.Dal.LesSupplierSubsupplierLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSupplierSubsupplierLink();
                System.Data.DataSet ds = dbo.Select_LES_SUPPLIER_SUBSUPPLIER_LINKs_By_SUPPLIERID(SUPPLIERID);
                LesSupplierSubsupplierLinkCollection collection = new LesSupplierSubsupplierLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesSupplierSubsupplierLink obj = new LesSupplierSubsupplierLink();
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
        
        public static LesSupplierSubsupplierLinkCollection Select_LES_SUPPLIER_SUBSUPPLIER_LINKs_By_SUBSUPPLIERID(System.Nullable<int> SUBSUPPLIERID) {
            MetroLesMonitor.Dal.LesSupplierSubsupplierLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSupplierSubsupplierLink();
                System.Data.DataSet ds = dbo.Select_LES_SUPPLIER_SUBSUPPLIER_LINKs_By_SUBSUPPLIERID(SUBSUPPLIERID);
                LesSupplierSubsupplierLinkCollection collection = new LesSupplierSubsupplierLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesSupplierSubsupplierLink obj = new LesSupplierSubsupplierLink();
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
        
        public static LesSupplierSubsupplierLinkCollection GetAll() {
            MetroLesMonitor.Dal.LesSupplierSubsupplierLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSupplierSubsupplierLink();
                System.Data.DataSet ds = dbo.LES_SUPPLIER_SUBSUPPLIER_LINK_Select_All();
                LesSupplierSubsupplierLinkCollection collection = new LesSupplierSubsupplierLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        LesSupplierSubsupplierLink obj = new LesSupplierSubsupplierLink();
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
        
        public static LesSupplierSubsupplierLink Load(System.Nullable<int> SUPPLINKID) {
            MetroLesMonitor.Dal.LesSupplierSubsupplierLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSupplierSubsupplierLink();
                System.Data.DataSet ds = dbo.LES_SUPPLIER_SUBSUPPLIER_LINK_Select_One(SUPPLINKID);
                LesSupplierSubsupplierLink obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new LesSupplierSubsupplierLink();
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
            MetroLesMonitor.Dal.LesSupplierSubsupplierLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSupplierSubsupplierLink();
                System.Data.DataSet ds = dbo.LES_SUPPLIER_SUBSUPPLIER_LINK_Select_One(this.Supplinkid);
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
            MetroLesMonitor.Dal.LesSupplierSubsupplierLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSupplierSubsupplierLink();
                dbo.LES_SUPPLIER_SUBSUPPLIER_LINK_Insert(this._supplierid, this._subsupplierid, this.UpdateDate, this.CreatedDate, this.UpdatedBy, this.CreatedBy, this.SubsupplierEmail, this.SupplierEmail, this.CcEmail, this.BccEmail, this.MailSubject, this.ReplyEmail, this.BilltoAddressid, this.ShiptoAddressid);
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
            MetroLesMonitor.Dal.LesSupplierSubsupplierLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSupplierSubsupplierLink();
                dbo.LES_SUPPLIER_SUBSUPPLIER_LINK_Delete(this.Supplinkid);
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
            MetroLesMonitor.Dal.LesSupplierSubsupplierLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.LesSupplierSubsupplierLink();
                dbo.LES_SUPPLIER_SUBSUPPLIER_LINK_Update(this.Supplinkid, this._supplierid, this._subsupplierid, this.UpdateDate, this.CreatedDate, this.UpdatedBy, this.CreatedBy, this.SubsupplierEmail, this.SupplierEmail, this.CcEmail, this.BccEmail, this.MailSubject, this.ReplyEmail, this.BilltoAddressid, this.ShiptoAddressid);
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
    }
}
