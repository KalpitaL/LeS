namespace MetroLesMonitor.Bll {
        
    public partial class SmDocumentformatRules {
        
        private System.Nullable<int> _documentformatRuleid;
        
        private System.Nullable<int> _docformatid;
        
        private System.Nullable<int> _ruleId;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        private string _ruleValue;
        
        public virtual System.Nullable<int> DocumentformatRuleid {
            get {
                return _documentformatRuleid;
            }
            set {
                _documentformatRuleid = value;
            }
        }
        
        public virtual System.Nullable<int> Docformatid {
            get {
                return _docformatid;
            }
            set {
                _docformatid = value;
            }
        }
        
        public virtual System.Nullable<int> RuleId {
            get {
                return _ruleId;
            }
            set {
                _ruleId = value;
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
        
        public virtual string RuleValue {
            get {
                return _ruleValue;
            }
            set {
                _ruleValue = value;
            }
        }
        
        private void Clean() {
            this.DocumentformatRuleid = null;
            this.Docformatid = null;
            this.RuleId = null;
            this.CreatedDate = null;
            this.UpdateDate = null;
            this.RuleValue = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["DOCUMENTFORMAT_RULEID"] != System.DBNull.Value)) {
                this.DocumentformatRuleid = ((System.Nullable<int>)(dr["DOCUMENTFORMAT_RULEID"]));
            }
            if ((dr["DOCFORMATID"] != System.DBNull.Value)) {
                this.Docformatid = ((System.Nullable<int>)(dr["DOCFORMATID"]));
            }
            if ((dr["RULE_ID"] != System.DBNull.Value)) {
                this.RuleId = ((System.Nullable<int>)(dr["RULE_ID"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value)) {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value)) {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
            if ((dr["RULE_VALUE"] != System.DBNull.Value)) {
                this.RuleValue = ((string)(dr["RULE_VALUE"]));
            }
        }
        
        public static SmDocumentformatRulesCollection GetAll() {
            MetroLesMonitor.Dal.SmDocumentformatRules dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDocumentformatRules();
                System.Data.DataSet ds = dbo.SM_DOCUMENTFORMAT_RULES_Select_All();
                SmDocumentformatRulesCollection collection = new SmDocumentformatRulesCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmDocumentformatRules obj = new SmDocumentformatRules();
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
        
        public static SmDocumentformatRules Load(System.Nullable<int> DOCUMENTFORMAT_RULEID) {
            MetroLesMonitor.Dal.SmDocumentformatRules dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDocumentformatRules();
                System.Data.DataSet ds = dbo.SM_DOCUMENTFORMAT_RULES_Select_One(DOCUMENTFORMAT_RULEID);
                SmDocumentformatRules obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmDocumentformatRules();
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
            MetroLesMonitor.Dal.SmDocumentformatRules dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDocumentformatRules();
                System.Data.DataSet ds = dbo.SM_DOCUMENTFORMAT_RULES_Select_One(this.DocumentformatRuleid);
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
            MetroLesMonitor.Dal.SmDocumentformatRules dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDocumentformatRules(_dataAccess);
                if (_dataAccess == null) { dbo._dataAccess.BeginTransaction(); }
                dbo.SM_DOCUMENTFORMAT_RULES_Insert(this.Docformatid, this.RuleId, this.CreatedDate, this.UpdateDate, this.RuleValue);
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
            MetroLesMonitor.Dal.SmDocumentformatRules dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDocumentformatRules(_dataAccess);
                if (_dataAccess == null) { dbo._dataAccess.BeginTransaction(); }
                dbo.SM_DOCUMENTFORMAT_RULES_Delete(this.DocumentformatRuleid);
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
            MetroLesMonitor.Dal.SmDocumentformatRules dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDocumentformatRules(_dataAccess);
                if (_dataAccess == null) { dbo._dataAccess.BeginTransaction(); }
                dbo.SM_DOCUMENTFORMAT_RULES_Update(this.DocumentformatRuleid, this.Docformatid, this.RuleId, this.CreatedDate, this.UpdateDate, this.RuleValue);
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

        public static System.Data.DataSet Get_DocumentformatRules_By_DocFormatID(int DOCFORMATID)
        {
            MetroLesMonitor.Dal.SmDocumentformatRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDocumentformatRules();
                return dbo.SM_DOCUMENTFORMAT_RULES_BY_DOCFORMATID(DOCFORMATID);
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

        public static System.Data.DataSet GetAllDocumentFormatRules()
        {
            MetroLesMonitor.Dal.SmDocumentformatRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDocumentformatRules();
                System.Data.DataSet ds = dbo.SM_DOCUMENTFORMAT_RULES_Select_All();
                System.Data.DataSet dsDFRules = new System.Data.DataSet();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    dsDFRules = ds;
                }
                return dsDFRules;
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

        public static System.Data.DataSet Get_DocumentformatRules_By_DocFormat(string DOCUMENT_FORMAT)
        {
            MetroLesMonitor.Dal.SmDocumentformatRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDocumentformatRules();
                return dbo.SM_DOCUMENTFORMAT_RULES_BY_DOCFORMAT(DOCUMENT_FORMAT);
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
