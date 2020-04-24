using System.Data;
namespace MetroLesMonitor.Bll
{
    
    public partial class SmDefaultRules {
        
        private System.Nullable<int> _defId;
        
        private System.Nullable<int> _addressid;
        
        private string _groupFormat;
        
        private System.Nullable<int> _ruleId;
        
        private string _ruleValue;
        
        public virtual System.Nullable<int> DefId {
            get {
                return _defId;
            }
            set {
                _defId = value;
            }
        }
        
        public virtual System.Nullable<int> Addressid {
            get {
                return _addressid;
            }
            set {
                _addressid = value;
            }
        }
        
        public virtual string GroupFormat {
            get {
                return _groupFormat;
            }
            set {
                _groupFormat = value;
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
        
        public virtual string RuleValue {
            get {
                return _ruleValue;
            }
            set {
                _ruleValue = value;
            }
        }
        
        private void Clean() {
            this.DefId = null;
            this.Addressid = null;
            this.GroupFormat = string.Empty;
            this.RuleId = null;
            this.RuleValue = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["DEF_ID"] != System.DBNull.Value)) {
                this.DefId = ((System.Nullable<int>)(dr["DEF_ID"]));
            }
            if ((dr["ADDRESSID"] != System.DBNull.Value)) {
                this.Addressid = ((System.Nullable<int>)(dr["ADDRESSID"]));
            }
            if ((dr["GROUP_FORMAT"] != System.DBNull.Value)) {
                this.GroupFormat = ((string)(dr["GROUP_FORMAT"]));
            }
            if ((dr["RULE_ID"] != System.DBNull.Value)) {
                this.RuleId = ((System.Nullable<int>)(dr["RULE_ID"]));
            }
            if ((dr["RULE_VALUE"] != System.DBNull.Value)) {
                this.RuleValue = ((string)(dr["RULE_VALUE"]));
            }
        }
        
        public static SmDefaultRulesCollection GetAll() {
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules();
                System.Data.DataSet ds = dbo.SM_DEFAULT_RULES_Select_All();
                SmDefaultRulesCollection collection = new SmDefaultRulesCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmDefaultRules obj = new SmDefaultRules();
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

        public static System.Data.DataSet GetAllDefaultRules()
        {
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules();
                System.Data.DataSet ds = dbo.SM_DEFAULT_RULES_Select_All();
              
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
        
        public static SmDefaultRules Load(System.Nullable<int> DEF_ID) {
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules();
                System.Data.DataSet ds = dbo.SM_DEFAULT_RULES_Select_One(DEF_ID);
                SmDefaultRules obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmDefaultRules();
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
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules();
                System.Data.DataSet ds = dbo.SM_DEFAULT_RULES_Select_One(this.DefId);
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
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules();
                dbo.SM_DEFAULT_RULES_Insert(this.Addressid, this.GroupFormat, this.RuleId, this.RuleValue);
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
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules();
                dbo.SM_DEFAULT_RULES_Delete(this.DefId);
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
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules();
                dbo.SM_DEFAULT_RULES_Update(this.DefId, this.Addressid, this.GroupFormat, this.RuleId, this.RuleValue);
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
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules(_dataAccess);
                dbo.SM_DEFAULT_RULES_Insert(this.Addressid, this.GroupFormat, this.RuleId, this.RuleValue);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public virtual void Delete(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules(_dataAccess);
                dbo.SM_DEFAULT_RULES_Delete(this.DefId);
            }
            catch (System.Exception)
            {
                throw;
            }           
        }

        public virtual void Update(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules(_dataAccess);
                dbo.SM_DEFAULT_RULES_Update(this.DefId, this.Addressid, this.GroupFormat, this.RuleId,this.RuleValue);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public static System.Data.DataSet select_SM_DOC_FORMAT(string ADDR_TYPE)
        {
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules();
                System.Data.DataSet ds = dbo.select_SM_DOC_FORMAT(ADDR_TYPE);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    return ds;
                }
                return null;
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

        public static System.Data.DataSet select_SM_DOC_FORMAT_All()
        {
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules();
                System.Data.DataSet ds = dbo.select_SM_DOC_FORMAT_All();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    return ds;
                }
                return null;
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

        public static System.Data.DataSet SM_DEFAULT_RULES_Select_By_AddressID(int AddressID,string GroupFormat)
        {
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules();
                System.Data.DataSet ds = dbo.SM_DEFAULT_RULES_Select_By_AddressID(AddressID,GroupFormat);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    return ds;
                }
                return null;
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

        public static System.Data.DataSet SM_DEFAULT_RULES_Select_By_AddressID_GroupFormat_RuleCode(int AddressID, string GroupFormat, int RuleID)
        {
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules();
                System.Data.DataSet ds = dbo.SM_DEFAULT_RULES_Select_By_AddressID_GroupFormat_RuleCode(AddressID, GroupFormat, RuleID);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    return ds;
                }
                return null;
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

        public static System.Data.DataSet SM_DEFAULT_RULES_Select_By_Address_GroupFormat(int ADDRESSID, string GroupFormat)
        {
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules();
                System.Data.DataSet ds = dbo.SM_DEFAULT_RULES_Select_By_Address_GroupFormat(ADDRESSID, GroupFormat);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    return ds;
                }
                return null;
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
        
        public static System.Data.DataSet SM_DEFAULT_RULES_Select_By_AddressID(int ADDRESSID)
        {
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules();
                System.Data.DataSet ds = dbo.SM_DEFAULT_RULES_Select_By_AddressID(ADDRESSID);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    return ds;
                }
                return null;
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

        public static SmDefaultRulesCollection select_SM_DOC_FORMAT_All_By_ADDRESS_GROUP_FORMAT(int ADDRESSID, string GROUP_FORMAT)
        {
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules();
                System.Data.DataSet ds = dbo.SM_DEFAULT_RULES_Select_All_By_Address_GroupFormat(ADDRESSID, GROUP_FORMAT);
                SmDefaultRulesCollection collection = new SmDefaultRulesCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        SmDefaultRules obj = new SmDefaultRules();
                        obj.Fill(dr);
                        collection.Add(obj);
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

        public static System.Data.DataSet GetDefaultRules_Without_Addressid(int ADDRESSID, string GROUP_FORMAT)
        {
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules();
                System.Data.DataSet ds = dbo.GET_ESUPPLIER_RULES_LIST_Without_AddressID(ADDRESSID, GROUP_FORMAT);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    return ds;
                }
                return null;
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

        public static SmDefaultRulesCollection Load_AddressID_Format(int ADDRESSID, string GROUP_FORMAT)
        {
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try
            {
                System.Data.DataSet ds = SM_DEFAULT_RULES_Select_By_Address_GroupFormat(ADDRESSID, GROUP_FORMAT);
                SmDefaultRulesCollection collection = new SmDefaultRulesCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            SmDefaultRules obj = new SmDefaultRules();
                            obj.Fill(dr);
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

        public static System.Data.DataSet GetDefaultRules_AddressID_Format(int ADDRESSID, string GROUP_FORMAT)
        {
            try
            {
                return SM_DEFAULT_RULES_Select_By_Address_GroupFormat(ADDRESSID, GROUP_FORMAT);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public static SmDefaultRulesCollection Get_DefaultRules_By_DefID(string DEFID)
        {
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules();
                System.Data.DataSet ds = dbo.GetDefaultRules_By_DefIDs(DEFID);
                SmDefaultRulesCollection collection = new SmDefaultRulesCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            SmDefaultRules obj = new SmDefaultRules();
                            obj.Fill(dr);
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

        public static System.Data.DataSet GetDefaultRules_By_AddressID_Format(int AddressID, string GroupFormat)
        {
            MetroLesMonitor.Dal.SmDefaultRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmDefaultRules();
                System.Data.DataSet ds = dbo.SM_DEFAULT_RULES_By_AddressID_Format(AddressID, GroupFormat);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    return ds;
                }
                return null;
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
