
using System.Data;
namespace MetroLesMonitor.Bll
{
    
    
    public partial class SmBuyerSupplierLinkRule {

        private System.Nullable<int> _linkRuleId;
        
        private System.Nullable<int> _linkid;
        
        private System.Nullable<int> _ruleid;

        public System.Nullable<int> UpdateInsertVal
        {
            get;
            set;
        }

        public string AuditValTxt
        {
            get;
            set;
        }
        
        private string _ruleValue;
        
        private System.Nullable<System.DateTime> _udateDate;
        
        private System.Nullable<int> _inheritRule;
        
        public virtual System.Nullable<int> LinkRuleId {
            get {
                return _linkRuleId;
            }
            set {
                _linkRuleId = value;
            }
        }
        
        public virtual System.Nullable<int> Linkid {
            get {
                return _linkid;
            }
            set {
                _linkid = value;
            }
        }
        
        public virtual System.Nullable<int> Ruleid {
            get {
                return _ruleid;
            }
            set {
                _ruleid = value;
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
        
        public virtual System.Nullable<System.DateTime> UdateDate {
            get {
                return _udateDate;
            }
            set {
                _udateDate = value;
            }
        }
        
        public virtual System.Nullable<int> InheritRule {
            get {
                return _inheritRule;
            }
            set {
                _inheritRule = value;
            }
        }
        
        private void Clean() {
            this.LinkRuleId = null;
            this.Linkid = null;
            this.Ruleid = null;
            this.RuleValue = string.Empty;
            this.UdateDate = null;
            this.InheritRule = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["LINK_RULE_ID"] != System.DBNull.Value)) {
                this.LinkRuleId = ((System.Nullable<int>)(dr["LINK_RULE_ID"]));
            }
            if ((dr["LINKID"] != System.DBNull.Value)) {
                this.Linkid = ((System.Nullable<int>)(dr["LINKID"]));
            }
            if ((dr["RULEID"] != System.DBNull.Value)) {
                this.Ruleid = ((System.Nullable<int>)(dr["RULEID"]));
            }
            if ((dr["RULE_VALUE"] != System.DBNull.Value)) {
                this.RuleValue = ((string)(dr["RULE_VALUE"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value)) {
                this.UdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
            if ((dr["INHERIT_RULE"] != System.DBNull.Value)) {
                this.InheritRule = ((System.Nullable<int>)(dr["INHERIT_RULE"]));
            }
        }
        
        public static SmBuyerSupplierLinkRuleCollection GetAll() {
            MetroLesMonitor.Dal.SmBuyerSupplierLinkRule dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLinkRule();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_LINK_RULE_Select_All();
                SmBuyerSupplierLinkRuleCollection collection = new SmBuyerSupplierLinkRuleCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmBuyerSupplierLinkRule obj = new SmBuyerSupplierLinkRule();
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
        
        public static SmBuyerSupplierLinkRule Load(System.Nullable<int> LINK_RULE_ID) {
            MetroLesMonitor.Dal.SmBuyerSupplierLinkRule dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLinkRule();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_LINK_RULE_Select_One(LINK_RULE_ID);
                SmBuyerSupplierLinkRule obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmBuyerSupplierLinkRule();
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
            MetroLesMonitor.Dal.SmBuyerSupplierLinkRule dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLinkRule();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_LINK_RULE_Select_One(this.LinkRuleId);
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
            MetroLesMonitor.Dal.SmBuyerSupplierLinkRule dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLinkRule();
                dbo.SM_BUYER_SUPPLIER_LINK_RULE_Insert(this.Linkid, this.Ruleid, this.RuleValue, this.UdateDate, this.InheritRule);
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
            MetroLesMonitor.Dal.SmBuyerSupplierLinkRule dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLinkRule(_dataAccess);
                dbo.SM_BUYER_SUPPLIER_LINK_RULE_Insert(this.Linkid, this.Ruleid, this.RuleValue, this.UdateDate, this.InheritRule);
            }
            catch (System.Exception)
            {
                throw;
            }        
        }

        public virtual void Delete() {
            MetroLesMonitor.Dal.SmBuyerSupplierLinkRule dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLinkRule();
                dbo.SM_BUYER_SUPPLIER_LINK_RULE_Delete(this.LinkRuleId);
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

        public virtual void Delete(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLinkRule dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLinkRule(_dataAccess);
                dbo.SM_BUYER_SUPPLIER_LINK_RULE_Delete(this.LinkRuleId);
            }
            catch (System.Exception)
            {
                throw;
            }
            //finally
            //{
            //    if ((dbo != null))
            //    {
            //        dbo.Dispose();
            //    }
            //}
        }
        
        public virtual void Update() {
            MetroLesMonitor.Dal.SmBuyerSupplierLinkRule dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLinkRule();
                dbo.SM_BUYER_SUPPLIER_LINK_RULE_Update(this.LinkRuleId, this.Linkid, this.Ruleid, this.RuleValue, this.UdateDate, this.InheritRule);
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
            MetroLesMonitor.Dal.SmBuyerSupplierLinkRule dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLinkRule(_dataAccess);
                dbo.SM_BUYER_SUPPLIER_LINK_RULE_Update(this.LinkRuleId, this.Linkid, this.Ruleid, this.RuleValue, this.UdateDate, this.InheritRule);
            }
            catch (System.Exception)
            {
                throw;
            }
            //finally
            //{
            //    if ((dbo != null))
            //    {
            //        dbo.Dispose();
            //    }
            //}
        }

        public static SmBuyerSupplierLinkRule LoadByLinkRule(int LinkID, int RULE_ID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLinkRule dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLinkRule();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_LINK_RULE_Select_By_LinkIDRuleID(LinkID, RULE_ID);
                SmBuyerSupplierLinkRule obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmBuyerSupplierLinkRule();
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

        public static SmBuyerSupplierLinkRuleCollection LoadByLinkID(int LINKID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLinkRule dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLinkRule();
                System.Data.DataSet ds = dbo.SMV_BUYER_SUPPLIER_LINK_RULE_Select_By_LinkID(LINKID);
                SmBuyerSupplierLinkRule obj = null;
                SmBuyerSupplierLinkRuleCollection collection = new SmBuyerSupplierLinkRuleCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        obj = new SmBuyerSupplierLinkRule();
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

        public DataSet Get_BuyerSupplierLinkRule_LinkID_RuleID(int LinkID, int RULE_ID)
        {
            DataSet _result = null;
            MetroLesMonitor.Dal.SmBuyerSupplierLinkRule dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLinkRule();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_LINK_RULE_Select_By_LinkIDRuleID(LinkID, RULE_ID);
                SmBuyerSupplierLinkRule obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        _result = ds;
                    }
                }
                return _result;
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


        public DataSet GET_SMV_LINK_RULE_Select_By_LinkID(System.Nullable<int> LINKID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLinkRule dbo = null;
            try
            {
                DataSet obj = new DataSet();
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLinkRule();
                System.Data.DataSet ds = dbo.SMV_BUYER_SUPPLIER_LINK_RULE_Select_By_LinkID(LINKID);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    obj = ds;
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

        public DataSet GET_ESUPPLIER_RULES_LIST_Without_LinkID(System.Nullable<int> LINKID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLinkRule dbo = null;
            try
            {
                DataSet obj = new DataSet();
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLinkRule();
                System.Data.DataSet ds = dbo.GET_ESUPPLIER_RULES_LIST_Without_LinkID(LINKID);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    obj = ds;
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

        public void DeleteLinkRule(System.Nullable<int> LINKID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLinkRule dbo = null;
            try
            {
                DataSet obj = new DataSet();
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLinkRule();
                dbo.DeleteLinkRule(LINKID);
               
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

        public void DeleteLinkRule(System.Nullable<int> LINKID, Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLinkRule dbo = null;
            try
            {
                DataSet obj = new DataSet();
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLinkRule(_dataAccess);
                dbo.DeleteLinkRule(LINKID);

            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if ((dbo != null))
                {
                    //dbo.Dispose();
                }
            }
        }

        public DataSet GET_Save_LINK_RULE_Select_By_LinkID(System.Nullable<int> LINKID, System.Nullable<int> RULEID)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLinkRule dbo = null;
            try
            {
                DataSet obj = new DataSet();
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLinkRule();
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_Save_LINK_RULE_Select_By_LinkID(LINKID, RULEID);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    obj = ds;
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

        public static SmBuyerSupplierLinkRule LoadByLinkRule_(int LinkID, int RULE_ID, Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmBuyerSupplierLinkRule dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmBuyerSupplierLinkRule(_dataAccess);
                System.Data.DataSet ds = dbo.SM_BUYER_SUPPLIER_LINK_RULE_Select_By_LinkIDRuleID(LinkID, RULE_ID);
                SmBuyerSupplierLinkRule obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmBuyerSupplierLinkRule();
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
            }
        }
    }
}
