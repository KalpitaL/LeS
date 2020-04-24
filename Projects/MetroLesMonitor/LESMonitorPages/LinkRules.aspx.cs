using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace MetroLesMonitor.LESMonitorPages
{
    public partial class LinkRules : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillSupplierGrid()
        {
            string json = "";
            int nBuyerID = 0;
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = new DataSet();
            if (HttpContext.Current.Session["ADDRESSID"] != null && HttpContext.Current.Session["ADDRESSID"] != "")
            {
                nBuyerID = Convert.ToInt32(HttpContext.Current.Session["ADDRESSID"]);
            }
            if (nBuyerID > 0)
            {
                _ds = _Routine.GetLinkedSuppliers(nBuyerID);              
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_ds.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillBuyerSupplierGrid(string ADDRESSID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = new DataSet();
            if (Convert.ToInt32(ADDRESSID) > 0)
            {
                _ds = _Routine.Get_Supplier_Specific_Buyers(Convert.ToInt32(ADDRESSID));
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_ds.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillRulesGrid(string LINKID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = new DataSet();
            if (Convert.ToInt32(LINKID) > 0)
            {
                _ds = _Routine.Get_SMV_BuyerSupplierLinkRule(Convert.ToInt32(LINKID));
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_ds.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillUnLinkedRulesGrid(string LINKID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = new DataSet();
            if (Convert.ToInt32(LINKID) > 0)
            {
                _ds = _Routine.GET_ESUPPLIER_RULES_LIST_Without_LinkID(Convert.ToInt32(LINKID));
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_ds.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveBSRuleDetails(List<string> slBSRuledet)
        {
            string json = "", cRULE_VALUE = "",cINHERIT_VALUE="";
            int nLinkID = 0, nRuleID = 0;
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slBSRuledet);
                if (_lstdet != null && _lstdet.Count > 0)
                {
                    slvalues.Clear();
                    for (int i = 0; i < _lstdet.Count; i++)
                    {
                        string lstkey = _lstdet[i][0];
                        slvalues.Add(lstkey);
                    }
                    Dictionary<string, string> _dictexp = _def.ConvertListToDictionary(slvalues);
                    foreach (string key in _dictexp.Keys)
                    {
                        switch (key.ToUpper())
                        {
                            case "LINKID": nLinkID = Convert.ToInt32(_dictexp[key]);
                                break;
                            case "RULEID": nRuleID = Convert.ToInt32(_dictexp[key]);
                                break;
                            case "RULE_VALUE": cRULE_VALUE = _dictexp[key];
                                break;
                            case "INHERIT_RULE": cINHERIT_VALUE = _dictexp[key];
                                break;
                        }
                    }
                      _Routine.UpdateInsertLinkRuleValue(nLinkID, nRuleID, cRULE_VALUE, cINHERIT_VALUE, Convert.ToString(HttpContext.Current.Session["UserHostServer"]),true);
                }
                  json = "1";
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string AddNewRuleDetails(string LINKID, List<string> slChkdet)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            try
            {
                if (slChkdet.Count > 0)
                {
                    for (int i = 0; i < slChkdet.Count; i++)
                    {
                        _Routine.AddLinkRule(convert.ToInt(LINKID), convert.ToInt(slChkdet[i]));
                    }
                }
                json = "1";
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }


    }
}