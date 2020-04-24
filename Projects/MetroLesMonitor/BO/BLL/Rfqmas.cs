namespace MetroLesMonitor.Bll {
    
    
    public partial class Rfqmas {
        
        private System.Nullable<int> _recKey;
        
        private string _orgId;
        
        private string _locId;
        
        private string _docId;
        
        private System.Nullable<System.DateTime> _docDate;
        
        private string _userId;
        
        private string _userName;
        
        private string _empId;
        
        private string _empName;
        
        private string _suppId;
        
        private string _name;
        
        private string _attnTo;
        
        private string _ccTo;
        
        private string _deptId;
        
        private string _deptName;
        
        private string _currId;
        
        private System.Nullable<decimal> _currRate;
        
        private string _suppRef;
        
        private string _ourRef;
        
        private string _termId;
        
        private System.Nullable<System.DateTime> _dlyDate;
        
        private string _vslId;
        
        private string _vslName;
        
        private string _marking;
        
        private string _remark;
        
        private RfqlineCollection _rfqlineCollection;
        
        public virtual System.Nullable<int> RecKey {
            get {
                return _recKey;
            }
            set {
                _recKey = value;
            }
        }
        
        public virtual string OrgId {
            get {
                return _orgId;
            }
            set {
                _orgId = value;
            }
        }
        
        public virtual string LocId {
            get {
                return _locId;
            }
            set {
                _locId = value;
            }
        }
        
        public virtual string DocId {
            get {
                return _docId;
            }
            set {
                _docId = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> DocDate {
            get {
                return _docDate;
            }
            set {
                _docDate = value;
            }
        }
        
        public virtual string UserId {
            get {
                return _userId;
            }
            set {
                _userId = value;
            }
        }
        
        public virtual string UserName {
            get {
                return _userName;
            }
            set {
                _userName = value;
            }
        }
        
        public virtual string EmpId {
            get {
                return _empId;
            }
            set {
                _empId = value;
            }
        }
        
        public virtual string EmpName {
            get {
                return _empName;
            }
            set {
                _empName = value;
            }
        }
        
        public virtual string SuppId {
            get {
                return _suppId;
            }
            set {
                _suppId = value;
            }
        }
        
        public virtual string Name {
            get {
                return _name;
            }
            set {
                _name = value;
            }
        }
        
        public virtual string AttnTo {
            get {
                return _attnTo;
            }
            set {
                _attnTo = value;
            }
        }
        
        public virtual string CcTo {
            get {
                return _ccTo;
            }
            set {
                _ccTo = value;
            }
        }
        
        public virtual string DeptId {
            get {
                return _deptId;
            }
            set {
                _deptId = value;
            }
        }
        
        public virtual string DeptName {
            get {
                return _deptName;
            }
            set {
                _deptName = value;
            }
        }
        
        public virtual string CurrId {
            get {
                return _currId;
            }
            set {
                _currId = value;
            }
        }
        
        public virtual System.Nullable<decimal> CurrRate {
            get {
                return _currRate;
            }
            set {
                _currRate = value;
            }
        }
        
        public virtual string SuppRef {
            get {
                return _suppRef;
            }
            set {
                _suppRef = value;
            }
        }
        
        public virtual string OurRef {
            get {
                return _ourRef;
            }
            set {
                _ourRef = value;
            }
        }
        
        public virtual string TermId {
            get {
                return _termId;
            }
            set {
                _termId = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> DlyDate {
            get {
                return _dlyDate;
            }
            set {
                _dlyDate = value;
            }
        }
        
        public virtual string VslId {
            get {
                return _vslId;
            }
            set {
                _vslId = value;
            }
        }
        
        public virtual string VslName {
            get {
                return _vslName;
            }
            set {
                _vslName = value;
            }
        }
        
        public virtual string Marking {
            get {
                return _marking;
            }
            set {
                _marking = value;
            }
        }
        
        public virtual string Remark {
            get {
                return _remark;
            }
            set {
                _remark = value;
            }
        }
        
        public virtual RfqlineCollection RfqlineCollection {
            get {
                if ((this._rfqlineCollection == null)) {
                    _rfqlineCollection = MetroLesMonitor.Bll.Rfqline.Select_RFQLINEs_By_MAS_REC_KEY(this.RecKey);
                }
                return this._rfqlineCollection;
            }
        }
        
        private void Clean() {
            this.RecKey = null;
            this.OrgId = string.Empty;
            this.LocId = string.Empty;
            this.DocId = string.Empty;
            this.DocDate = null;
            this.UserId = string.Empty;
            this.UserName = string.Empty;
            this.EmpId = string.Empty;
            this.EmpName = string.Empty;
            this.SuppId = string.Empty;
            this.Name = string.Empty;
            this.AttnTo = string.Empty;
            this.CcTo = string.Empty;
            this.DeptId = string.Empty;
            this.DeptName = string.Empty;
            this.CurrId = string.Empty;
            this.CurrRate = null;
            this.SuppRef = string.Empty;
            this.OurRef = string.Empty;
            this.TermId = string.Empty;
            this.DlyDate = null;
            this.VslId = string.Empty;
            this.VslName = string.Empty;
            this.Marking = string.Empty;
            this.Remark = string.Empty;
            this._rfqlineCollection = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["REC_KEY"] != System.DBNull.Value)) {
                this.RecKey = ((System.Nullable<int>)(dr["REC_KEY"]));
            }
            if ((dr["ORG_ID"] != System.DBNull.Value)) {
                this.OrgId = ((string)(dr["ORG_ID"]));
            }
            if ((dr["LOC_ID"] != System.DBNull.Value)) {
                this.LocId = ((string)(dr["LOC_ID"]));
            }
            if ((dr["DOC_ID"] != System.DBNull.Value)) {
                this.DocId = ((string)(dr["DOC_ID"]));
            }
            if ((dr["DOC_DATE"] != System.DBNull.Value)) {
                this.DocDate = ((System.Nullable<System.DateTime>)(dr["DOC_DATE"]));
            }
            if ((dr["USER_ID"] != System.DBNull.Value)) {
                this.UserId = ((string)(dr["USER_ID"]));
            }
            if ((dr["USER_NAME"] != System.DBNull.Value)) {
                this.UserName = ((string)(dr["USER_NAME"]));
            }
            if ((dr["EMP_ID"] != System.DBNull.Value)) {
                this.EmpId = ((string)(dr["EMP_ID"]));
            }
            if ((dr["EMP_NAME"] != System.DBNull.Value)) {
                this.EmpName = ((string)(dr["EMP_NAME"]));
            }
            if ((dr["SUPP_ID"] != System.DBNull.Value)) {
                this.SuppId = ((string)(dr["SUPP_ID"]));
            }
            if ((dr["NAME"] != System.DBNull.Value)) {
                this.Name = ((string)(dr["NAME"]));
            }
            if ((dr["ATTN_TO"] != System.DBNull.Value)) {
                this.AttnTo = ((string)(dr["ATTN_TO"]));
            }
            if ((dr["CC_TO"] != System.DBNull.Value)) {
                this.CcTo = ((string)(dr["CC_TO"]));
            }
            if ((dr["DEPT_ID"] != System.DBNull.Value)) {
                this.DeptId = ((string)(dr["DEPT_ID"]));
            }
            if ((dr["DEPT_NAME"] != System.DBNull.Value)) {
                this.DeptName = ((string)(dr["DEPT_NAME"]));
            }
            if ((dr["CURR_ID"] != System.DBNull.Value)) {
                this.CurrId = ((string)(dr["CURR_ID"]));
            }
            if ((dr["CURR_RATE"] != System.DBNull.Value)) {
                this.CurrRate = ((System.Nullable<decimal>)(dr["CURR_RATE"]));
            }
            if ((dr["SUPP_REF"] != System.DBNull.Value)) {
                this.SuppRef = ((string)(dr["SUPP_REF"]));
            }
            if ((dr["OUR_REF"] != System.DBNull.Value)) {
                this.OurRef = ((string)(dr["OUR_REF"]));
            }
            if ((dr["TERM_ID"] != System.DBNull.Value)) {
                this.TermId = ((string)(dr["TERM_ID"]));
            }
            if ((dr["DLY_DATE"] != System.DBNull.Value)) {
                this.DlyDate = ((System.Nullable<System.DateTime>)(dr["DLY_DATE"]));
            }
            if ((dr["VSL_ID"] != System.DBNull.Value)) {
                this.VslId = ((string)(dr["VSL_ID"]));
            }
            if ((dr["VSL_NAME"] != System.DBNull.Value)) {
                this.VslName = ((string)(dr["VSL_NAME"]));
            }
            if ((dr["MARKING"] != System.DBNull.Value)) {
                this.Marking = ((string)(dr["MARKING"]));
            }
            if ((dr["REMARK"] != System.DBNull.Value)) {
                this.Remark = ((string)(dr["REMARK"]));
            }
        }
        
        public static RfqmasCollection GetAll() {
            MetroLesMonitor.Dal.Rfqmas dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.Rfqmas();
                System.Data.DataSet ds = dbo.RFQMAS_Select_All();
                RfqmasCollection collection = new RfqmasCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        Rfqmas obj = new Rfqmas();
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
        
        public static Rfqmas Load(System.Nullable<int> REC_KEY) {
            MetroLesMonitor.Dal.Rfqmas dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.Rfqmas();
                System.Data.DataSet ds = dbo.RFQMAS_Select_One(REC_KEY);
                Rfqmas obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new Rfqmas();
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
            MetroLesMonitor.Dal.Rfqmas dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.Rfqmas();
                System.Data.DataSet ds = dbo.RFQMAS_Select_One(this.RecKey);
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
            MetroLesMonitor.Dal.Rfqmas dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.Rfqmas();
                dbo.RFQMAS_Insert(this.RecKey, this.OrgId, this.LocId, this.DocId, this.DocDate, this.UserId, this.UserName, this.EmpId, this.EmpName, this.SuppId, this.Name, this.AttnTo, this.CcTo, this.DeptId, this.DeptName, this.CurrId, this.CurrRate, this.SuppRef, this.OurRef, this.TermId, this.DlyDate, this.VslId, this.VslName, this.Marking, this.Remark);
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
            MetroLesMonitor.Dal.Rfqmas dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.Rfqmas();
                dbo.RFQMAS_Delete(this.RecKey);
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
            MetroLesMonitor.Dal.Rfqmas dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.Rfqmas();
                dbo.RFQMAS_Update(this.RecKey, this.OrgId, this.LocId, this.DocId, this.DocDate, this.UserId, this.UserName, this.EmpId, this.EmpName, this.SuppId, this.Name, this.AttnTo, this.CcTo, this.DeptId, this.DeptName, this.CurrId, this.CurrRate, this.SuppRef, this.OurRef, this.TermId, this.DlyDate, this.VslId, this.VslName, this.Marking, this.Remark);
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
