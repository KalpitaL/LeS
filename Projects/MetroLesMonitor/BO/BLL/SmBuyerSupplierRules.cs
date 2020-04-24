namespace MetroLesMonitor.Bll
{

    public partial class SmBuyerSupplierRules
    {

        private System.Nullable<int> _groupRuleId;

        private System.Nullable<int> _groupId;

        private System.Nullable<int> _ruleId;

        private string _ruleValue;

        private SmBuyerSupplierGroups _smBuyerSupplierGroups;

        private SmEsupplierRules _smEsupplierRules;

        public virtual System.Nullable<int> GroupRuleId
        {
            get
            {
                return _groupRuleId;
            }
            set
            {
                _groupRuleId = value;
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

        public virtual System.Nullable<int> RuleId
        {
            get
            {
                return _ruleId;
            }
            set
            {
                _ruleId = value;
            }
        }

        public virtual SmBuyerSupplierGroups SmBuyerSupplierGroups
        {
            get
            {
                if ((this._smBuyerSupplierGroups == null))
                {
                    this._smBuyerSupplierGroups = MetroLesMonitor.Bll.SmBuyerSupplierGroups.Load(this._groupId);
                }
                return this._smBuyerSupplierGroups;
            }
            set
            {
                _smBuyerSupplierGroups = value;
            }
        }

        public virtual SmEsupplierRules SmEsupplierRules
        {
            get
            {
                if ((this._smEsupplierRules == null))
                {
                    this._smEsupplierRules = MetroLesMonitor.Bll.SmEsupplierRules.Load(this._ruleId);
                }
                return this._smEsupplierRules;
            }
            set
            {
                _smEsupplierRules = value;
            }
        }

        private void Clean()
        {
            this.GroupRuleId = null;
            this._groupId = null;
            this._ruleId = null;
            this.RuleValue = string.Empty;           
        }

        private void Fill(System.Data.DataRow dr)
        {
            this.Clean();
            if ((dr["GROUP_RULE_ID"] != System.DBNull.Value))
            {
                this.GroupRuleId = ((System.Nullable<int>)(dr["GROUP_RULE_ID"]));
            }
            if ((dr["GROUP_ID"] != System.DBNull.Value))
            {
                this._groupId = ((System.Nullable<int>)(dr["GROUP_ID"]));
            }
            if ((dr["RULE_ID"] != System.DBNull.Value))
            {
                this._ruleId = ((System.Nullable<int>)(dr["RULE_ID"]));
            }
            if ((dr["RULE_VALUE"] != System.DBNull.Value))
            {
                this.RuleValue = ((string)(dr["RULE_VALUE"]));
            }
        }

        public static SmBuyerSupplierRulesCollection Select_SM_BUYER_SUPPLIER_RULESs_By_GROUPID(System.Nullable<int> GROUP_ID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierRules();
                System.Data.DataSet ds = dbo.Select_SM_BUYER_SUPPLIER_RULESs_By_GROUP_ID(GROUP_ID);
                SmBuyerSupplierRulesCollection collection = new SmBuyerSupplierRulesCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmBuyerSupplierRules obj = new SmBuyerSupplierRules();
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

        //public static SmBuyerSupplierRulesCollection Select_SM_BUYER_SUPPLIER_RULESs_for_GROUP(System.Nullable<int> GROUP_ID)
        //{
        //    MetroLesMonitor.Dal.SmBuyerSupplierRules dbo = null;
        //    try
        //    {
        //        dbo = new MetroLesMonitor.Dal.SmBuyerSupplierRules();
        //        System.Data.DataSet ds = dbo.Select_SM_BUYER_SUPPLIER_RULESs_for_Group(GROUP_ID);
        //        SmBuyerSupplierRulesCollection collection = new SmBuyerSupplierRulesCollection();
        //        if (GlobalTools.IsSafeDataSet(ds))
        //        {
        //            for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
        //            {
        //                SmBuyerSupplierRules obj = new SmBuyerSupplierRules();
        //                obj.Fill(ds.Tables[0].Rows[i]);
        //                if ((obj != null))
        //                {
        //                    collection.Add(obj);
        //                }
        //            }
        //        }
        //        return collection;
        //    }
        //    catch (System.Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if ((dbo != null))
        //        {
        //            dbo.Dispose();
        //        }
        //    }
        //}

        public static SmBuyerSupplierRulesCollection Select_SM_BUYER_SUPPLIER_RULESs_By_RULE_ID(System.Nullable<int> RULE_ID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierRules();
                System.Data.DataSet ds = dbo.Select_SM_BUYER_SUPPLIER_RULESs_By_RULE_ID(RULE_ID);
                SmBuyerSupplierRulesCollection collection = new SmBuyerSupplierRulesCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmBuyerSupplierRules obj = new SmBuyerSupplierRules();
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

        public static System.Data.DataSet Sm_BuyerSupplier_Rules_Select_Unlinked_Rules(System.Nullable<int> GROUP_ID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierRules();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_RULES_Select_Unlinked_Rules(GROUP_ID);
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

        public static System.Data.DataSet Sm_BuyerSupplier_Rules_Select_by_Group(System.Nullable<int> GROUP_ID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierRules();
                System.Data.DataSet ds = dbo.Select_SM_BUYER_SUPPLIER_RULESs_for_Group(GROUP_ID);
                SmBuyerSupplierRulesCollection collection = new SmBuyerSupplierRulesCollection();
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
        } // For List of All Rules

        public static SmBuyerSupplierRulesCollection GetAll()
        {
            MetroLesMonitor.Dal.SmBuyerSupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierRules();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_RULES_Select_All();
                SmBuyerSupplierRulesCollection collection = new SmBuyerSupplierRulesCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmBuyerSupplierRules obj = new SmBuyerSupplierRules();
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

        public static SmBuyerSupplierRules Load(System.Nullable<int> GROUP_RULE_ID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierRules();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_RULES_Select_One(GROUP_RULE_ID);
                SmBuyerSupplierRules obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmBuyerSupplierRules();
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

        public static SmBuyerSupplierRules LoadByGroupRule(int GROUP_ID, int RULE_ID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierRules();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_RULES_Select_ByGroupRule(GROUP_ID,RULE_ID);
                SmBuyerSupplierRules obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmBuyerSupplierRules();
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
            MetroLesMonitor.Dal.SmBuyerSupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierRules();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_RULES_Select_One(this.GroupRuleId);
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
            MetroLesMonitor.Dal.SmBuyerSupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierRules();
                dbo.SM_BUYER_SUPPLIER_RULES_Insert(this._groupId, this._ruleId, this.RuleValue);
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
            MetroLesMonitor.Dal.SmBuyerSupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierRules();
                dbo.SM_BUYER_SUPPLIER_RULES_Delete(this.GroupRuleId);
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
            MetroLesMonitor.Dal.SmBuyerSupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierRules();
                dbo.SM_BUYER_SUPPLIER_RULES_Update(this.GroupRuleId, this._groupId, this._ruleId, this.RuleValue);
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
            MetroLesMonitor.Dal.SmBuyerSupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierRules(_dataAccess);
                dbo.SM_BUYER_SUPPLIER_RULES_Insert(this._groupId, this._ruleId, this.RuleValue);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public virtual void Update(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierRules(_dataAccess);
                dbo.SM_BUYER_SUPPLIER_RULES_Update(this.GroupRuleId, this._groupId, this._ruleId, this.RuleValue);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public virtual void Delete(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierRules dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierRules(_dataAccess);
                dbo.SM_BUYER_SUPPLIER_RULES_Delete(this.GroupRuleId);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
