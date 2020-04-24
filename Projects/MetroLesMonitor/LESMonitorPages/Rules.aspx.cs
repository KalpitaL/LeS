using MetroLesMonitor.Dal;
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
    public partial class Rules : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetRulesGrid()
        {
            string json = "";
            DataAccess _dataAccess = new DataAccess();
            DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.GetAllRules();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetXLSGroupsOnly()
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.GetExcelGroupsOnly();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string UpdateRuleDetails(List<string> slRuledet)
        {
            string json = "";
            string cRULEID = "", cRULE_NUMBER = "", cDOC_TYPE = "", cRULE_CODE = "", cRULE_DESC = "",cRULE_COMMENTS = "";
            int nRuleID = 0;
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slRuledet);

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
                            case "RULEID": cRULEID = _dictexp[key];
                                nRuleID = Convert.ToInt32(cRULEID);
                                break;
                            case "RULE_NUMBER": cRULE_NUMBER = _dictexp[key];
                                break;
                            case "DOC_TYPE": cDOC_TYPE = _dictexp[key];
                                break;
                            case "RULE_CODE": cRULE_CODE = _dictexp[key];
                                break;
                            case "RULE_DESC": cRULE_DESC = _dictexp[key];
                                break;
                            case "RULE_COMMENTS": cRULE_COMMENTS = _dictexp[key];
                                break;                         
                        }
                    }
                    _Routine.SaveRule(nRuleID, cRULE_NUMBER.Trim(), cDOC_TYPE.Trim(), cRULE_CODE.Trim(), cRULE_DESC.Trim(), cRULE_COMMENTS.Trim(), Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                    json = "1";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DeleteRule(string RULEID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();          
            try
            {
                if (!string.IsNullOrEmpty(RULEID) && Convert.ToInt32(RULEID) > 0)
                {
                    _Routine.DeleteRule(Convert.ToInt32(RULEID), Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                    json = "1";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string AddRuleDetails(List<string> slRuledet)
        {
            string json = "";
            string cRULE_NUMBER = "", cDOC_TYPE = "", cRULE_CODE = "", cRULE_DESC = "", cRULE_COMMENTS = "";       
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slRuledet);

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
                            case "RULE_NUMBER": cRULE_NUMBER = _dictexp[key];
                                break;
                            case "DOC_TYPE": cDOC_TYPE = _dictexp[key];
                                break;
                            case "RULE_CODE": cRULE_CODE = _dictexp[key];
                                break;
                            case "RULE_DESC": cRULE_DESC = _dictexp[key];
                                break;
                            case "RULE_COMMENTS": cRULE_COMMENTS = _dictexp[key];
                                break;
                        }
                    }
                    _Routine.SaveRule(0, cRULE_NUMBER.Trim(), cDOC_TYPE.Trim(), cRULE_CODE.Trim(), cRULE_DESC.Trim(), cRULE_COMMENTS.Trim(), Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                    json = "1";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string LinkRuleDetails(string RULEID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = new DataSet();
            if (!string.IsNullOrEmpty(RULEID) && Convert.ToInt32(RULEID) > 0)
            {
                _ds = _Routine.Get_Buyer_Supplier_Link_By_RuleID(Convert.ToInt32(RULEID));
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);           
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CheckExistingRule(string RULE_CODE, string RULEID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            int nvalid = _Routine.ValidateRule(RULE_CODE, Convert.ToInt32(RULEID));
            json = Convert.ToString(nvalid);
            return json;
        }
    }
}