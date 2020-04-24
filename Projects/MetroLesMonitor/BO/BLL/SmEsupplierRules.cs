using System.Data;
namespace MetroLesMonitor.Bll
{
    public partial class SmEsupplierRules
    {
        private System.Nullable<int> _ruleid;
        private string _ruleNumber;
        private string _docType;
        private string _ruleCode;
        private string _ruleDesc;
        private string _ruleComments;
        private string _ruleValue;
        private SmBuyerSupplierRulesCollection _smBuyerSupplierRulesCollection;

        public virtual System.Nullable<int> Ruleid
        {
            get
            {
                return _ruleid;
            }
            set
            {
                _ruleid = value;
            }
        }

        public virtual string RuleNumber
        {
            get
            {
                return _ruleNumber;
            }
            set
            {
                _ruleNumber = value;
            }
        }

        public virtual string DocType
        {
            get
            {
                return _docType;
            }
            set
            {
                _docType = value;
            }
        }

        public virtual string RuleCode
        {
            get
            {
                return _ruleCode;
            }
            set
            {
                _ruleCode = value;
            }
        }

        public virtual string RuleDesc
        {
            get
            {
                return _ruleDesc;
            }
            set
            {
                _ruleDesc = value;
            }
        }

        public virtual string RuleComments
        {
            get
            {
                return _ruleComments;
            }
            set
            {
                _ruleComments = value;
            }
        }

        public virtual string RuleValue
        {
            get
            {
                return _ruleValue;
            }
            set
            {
                _ruleValue = value;
            }
        }

        public virtual SmBuyerSupplierRulesCollection SmBuyerSupplierRulesCollection
        {
            get
            {
                if ((this._smBuyerSupplierRulesCollection == null))
                {
                    _smBuyerSupplierRulesCollection = MetroLesMonitor.Bll.SmBuyerSupplierRules.Select_SM_BUYER_SUPPLIER_RULESs_By_RULE_ID(this.Ruleid);
                }
                return this._smBuyerSupplierRulesCollection;
            }
        }

        private void Clean()
        {
            this.Ruleid = null;
            this.RuleNumber = string.Empty;
            this.DocType = string.Empty;
            this.RuleCode = string.Empty;
            this.RuleDesc = string.Empty;
            this.RuleComments = string.Empty;
            this._smBuyerSupplierRulesCollection = null;
        }

        private void Fill(System.Data.DataRow dr)
        {
            this.Clean();
            if ((dr["RULEID"] != System.DBNull.Value))
            {
                this.Ruleid = ((System.Nullable<int>)(dr["RULEID"]));
            }
            if ((dr["RULE_NUMBER"] != System.DBNull.Value))
            {
                this.RuleNumber = ((string)(dr["RULE_NUMBER"]));
            }
            if ((dr["DOC_TYPE"] != System.DBNull.Value))
            {
                this.DocType = ((string)(dr["DOC_TYPE"]));
            }
            if ((dr["RULE_CODE"] != System.DBNull.Value))
            {
                this.RuleCode = ((string)(dr["RULE_CODE"]));
            }
            if ((dr["RULE_DESC"] != System.DBNull.Value))
            {
                this.RuleDesc = ((string)(dr["RULE_DESC"]));
            }
            if ((dr["RULE_COMMENTS"] != System.DBNull.Value))
            {
                this.RuleComments = ((string)(dr["RULE_COMMENTS"]));
            }
        }

        public static SmEsupplierRulesCollection GetAll()
        {
            MetroLesMonitor.Dal.SmEsupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmEsupplierRules();
                System.Data.DataSet ds = dbo.SM_ESUPPLIER_RULES_Select_All();
                SmEsupplierRulesCollection collection = new SmEsupplierRulesCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmEsupplierRules obj = new SmEsupplierRules();
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

        public static System.Data.DataSet GetAllRules()
        {
            MetroLesMonitor.Dal.SmEsupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmEsupplierRules();
                System.Data.DataSet ds = dbo.SM_ESUPPLIER_RULES_Select_All();
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

        public static SmEsupplierRules Load(System.Nullable<int> RULEID)
        {
            MetroLesMonitor.Dal.SmEsupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmEsupplierRules();
                System.Data.DataSet ds = dbo.SM_ESUPPLIER_RULES_Select_One(RULEID);
                SmEsupplierRules obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmEsupplierRules();
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
            MetroLesMonitor.Dal.SmEsupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmEsupplierRules();
                System.Data.DataSet ds = dbo.SM_ESUPPLIER_RULES_Select_One(this.Ruleid);
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
            MetroLesMonitor.Dal.SmEsupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmEsupplierRules();
                dbo.SM_ESUPPLIER_RULES_Insert(this.Ruleid, this.RuleNumber, this.DocType, this.RuleCode, this.RuleDesc, this.RuleComments);
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
            MetroLesMonitor.Dal.SmEsupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmEsupplierRules();
                dbo.SM_ESUPPLIER_RULES_Delete(this.Ruleid);
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
            MetroLesMonitor.Dal.SmEsupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmEsupplierRules();
                dbo.SM_ESUPPLIER_RULES_Update(this.Ruleid, this.RuleNumber, this.DocType, this.RuleCode, this.RuleDesc, this.RuleComments);
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

        /***/

        public static SmEsupplierRules Load_by_ID(System.Nullable<int> RULEID, Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmEsupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmEsupplierRules(_dataAccess);
                System.Data.DataSet ds = dbo.SM_ESUPPLIER_RULES_Select_One(RULEID);
                SmEsupplierRules obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmEsupplierRules();
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

        public virtual void Insert(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmEsupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmEsupplierRules(_dataAccess);
                dbo.SM_ESUPPLIER_RULES_Insert(this.Ruleid,this.RuleNumber,this.DocType, this.RuleCode, this.RuleDesc, this.RuleComments);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public virtual void Delete(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmEsupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmEsupplierRules(_dataAccess);
                dbo.SM_ESUPPLIER_RULES_Delete(this.Ruleid);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public virtual void Update(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmEsupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmEsupplierRules(_dataAccess);
                dbo.SM_ESUPPLIER_RULES_Update(this.Ruleid, this.RuleNumber, this.DocType, this.RuleCode, this.RuleDesc, this.RuleComments);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public static int ValidateRuleCode(string RULE_CODE, int RuleID)
        {
            int _RES = 0;
            MetroLesMonitor.Dal.SmEsupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmEsupplierRules();
                System.Data.DataSet ds = dbo.SM_ESUPPLIER_RULES_Select_By_RuleCode(RULE_CODE);

                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        SmEsupplierRules _rules = new SmEsupplierRules();
                        _rules.Fill(ds.Tables[0].Rows[0]);

                        if (RuleID == _rules.Ruleid) _RES= 0;
                        _RES = ds.Tables[0].Rows.Count;
                    }
                }
                else
                    _RES= 0;
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
            return _RES;
        }

        public static int Get_RuleId_By_RuleCode(string RuleCode)
        {
            MetroLesMonitor.Dal.SmEsupplierRules dbo = null;
            int RuleID = 0;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmEsupplierRules();
                System.Data.DataSet ds = dbo.SM_ESUPPLIER_RULES_Select_By_RuleCode(RuleCode);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        RuleID = convert.ToInt(dr["RULEID"].ToString());
                    }
                }
                return RuleID;
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

        public static DataSet Get_RuleDetails_By_RuleCode(string RuleCode)
        {
            MetroLesMonitor.Dal.SmEsupplierRules dbo = null;
            DataSet _Ds = new DataSet();
            try
            {
                dbo = new MetroLesMonitor.Dal.SmEsupplierRules();
                System.Data.DataSet ds = dbo.SM_ESUPPLIER_RULES_Select_By_RuleCode(RuleCode);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    _Ds = ds;
                }
                return _Ds;
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

        public static DataSet GetUnAssigned_eSupplierRules(string DOCFORMATID)
        {
            MetroLesMonitor.Dal.SmEsupplierRules dbo = null;
            DataSet _Ds = new DataSet();
            try
            {
                dbo = new MetroLesMonitor.Dal.SmEsupplierRules();
                System.Data.DataSet ds = dbo.Select_UnAssigned_ESupplierRules(DOCFORMATID);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    _Ds = ds;
                }
                return _Ds;
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
