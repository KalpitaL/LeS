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
    public partial class DefaultRules : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetDefaultRulesGrid()
        {
            string json = "";
            DataAccess _dataAccess = new DataAccess();
            DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.GetAllDefaultRules();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetPartyCode()
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.GetPartyCode();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetAllRules()
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.GetAllRules();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetGroupFormatList(string KEY,string VALUE)
        {
            string json = "";
            DataAccess _dataAccess = new DataAccess();
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(VALUE))
            {
                if (KEY.ToUpper() == "RULE_ADDR_TYPE")
                {
                    _ds = _Routine.GetGroupFormatList(VALUE);
                }
                else if (KEY.ToUpper() == "ADDRESSID")
                {
                    _ds = _Routine.GetGroupFormatList(Convert.ToInt32(VALUE));
                }
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillRulesGrid(string ADDRESSID, string GROUP_FORMAT)
        {
            string json = "";
            DataAccess _dataAccess = new DataAccess();
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.Get_Party_Specific_Default_Rules(Convert.ToInt32(ADDRESSID), GROUP_FORMAT);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveDefaultRuleDetails(List<string> slDefRuledet)
        {
            string json = "";
            string cADDRESSID = "", cGROUP_FORMAT = "", cRULE_VALUE = "";
            int nRuleID = 0, nAddressid = 0, nDefid=0;
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slDefRuledet);
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
                            case "ADDRESSID": cADDRESSID = _dictexp[key];
                                nAddressid = Convert.ToInt32(cADDRESSID);
                                break;
                            case "GROUP_FORMAT": cGROUP_FORMAT = _dictexp[key];
                                break;
                        }
                    }                    
                    _Routine.SaveDefaultRule(nDefid, nAddressid, cGROUP_FORMAT, nRuleID, cRULE_VALUE, Convert.ToString(HttpContext.Current.Session["UserHostServer"]),null);
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
        public static string DeleteDefaultRule(string ADDRESSID,string GROUP_FORMAT)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                if (!string.IsNullOrEmpty(ADDRESSID) && Convert.ToInt32(ADDRESSID) > 0)
                {
                    _Routine.DeleteDefaultRuleAddress(Convert.ToInt32(ADDRESSID),GROUP_FORMAT, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
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
        public static string SaveRuleDetails(List<string> slRuledet)
        {
            string json = "";
            string cADDRESSID = "", cGROUP_FORMAT = "", cRULE_VALUE = "", cRULE_CODE = "";
            int nRuleID = 0, nAddressid = 0, nDefid = 0,nhidval=0;
            bool IsAudit = true;
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
                            case "DEFAULT_RULE_ADDRESSID": cADDRESSID = _dictexp[key];
                                nAddressid = Convert.ToInt32(cADDRESSID);
                                break;
                            case "DEFAULT_RULE_GROUP_FORMAT": cGROUP_FORMAT = _dictexp[key];
                                break;
                            case "RULE_CODE": cRULE_CODE = _dictexp[key];
                                break;
                            case "RULEID": nRuleID = Convert.ToInt32(_dictexp[key]);
                                break;
                            case "RULE_VALUE": cRULE_VALUE = _dictexp[key];
                                break;
                            case "HID_VALUE": nhidval = Convert.ToInt32(_dictexp[key]);
                                break;
                            case "DEF_ID": nDefid = Convert.ToInt32(_dictexp[key]);
                                break;
                        }
                    }
                    _Routine.SaveDefaultRule(nDefid, nAddressid, cGROUP_FORMAT, nRuleID, cRULE_VALUE, Convert.ToString(HttpContext.Current.Session["UserHostServer"]),null);
                    if (nhidval == 1)
                    {
                        DataSet dsLinkedAddr = _Routine.GetLinkedBuyersSupplier(nAddressid);
                        int cKvalueFormat = _Routine.GetGroupFormatBuyersSupplierValue(nAddressid);
                        if (dsLinkedAddr != null && dsLinkedAddr.Tables.Count > 0 && dsLinkedAddr.Tables[0].Rows.Count > 0)
                        {
                            int count = 0;
                            foreach (DataRow dr in dsLinkedAddr.Tables[0].Rows)
                            {
                                string Format = "";
                                int LinkID = convert.ToInt(dr["LINKID"]);
                                if (cKvalueFormat == 0) Format = convert.ToString(dr["VENDOR_FORMAT"]).Trim().ToUpper();
                                else Format = convert.ToString(dr["BUYER_FORMAT"]).Trim().ToUpper();

                                if (Format == cGROUP_FORMAT.Trim().ToUpper())
                                {
                                    System.Data.DataSet _dsLinkRule = _Routine.GetBuyerSupplier_Save_LinkRule(LinkID, nRuleID);
                                    if (_dsLinkRule != null && _dsLinkRule.Tables.Count > 0 && _dsLinkRule.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow drLinkRule in _dsLinkRule.Tables[0].Rows)
                                        {
                                            string Inherite = drLinkRule["INHERIT_RULE"].ToString();
                                            if (Inherite != "-1")
                                            {
                                                _Routine.UpdateInsertLinkRuleValue(LinkID, nRuleID, cRULE_VALUE, "1", Convert.ToString(HttpContext.Current.Session["UserHostServer"]),false);
                                                count++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        _Routine.UpdateInsertLinkRuleValue(LinkID, nRuleID, cRULE_VALUE, "1", Convert.ToString(HttpContext.Current.Session["UserHostServer"]), false);
                                        count++;
                                    }
                                }
                            }
                            string AuditValue = "Link-Rule (" + cRULE_CODE + ")  RuleValue : " + cRULE_VALUE + " LinkRules Updated for (" + count + ") records by [" + Convert.ToString(HttpContext.Current.Session["UserHostServer"]) + "]";
                            _Routine.SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "");
                        }
                    }
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
        public static string DeleteRule(string DEF_ID, string ADDRESSID, string RULE_CODE, string DELALL)
        {
            string json = "";
            int nAddressID = 0;
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                if (!string.IsNullOrEmpty(DEF_ID) && Convert.ToInt32(DEF_ID) > 0)
                {
                    nAddressID = Convert.ToInt32(ADDRESSID);
                    string Addr_name = _Routine.GetAddressName(nAddressID);
                    _Routine.DeleteDefaultRule(Convert.ToInt32(DEF_ID), Convert.ToString(HttpContext.Current.Session["UserHostServer"]), convert.ToString(RULE_CODE), Addr_name, convert.ToInt(DELALL));
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
        public static string ValidateRule(List<string> slRuledet)
        {
            string json = "", cRULE_CODE = "", cGROUP_FORMAT = "";
            int nAddressID = 0, nRuleID = 0;
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
                            case "DEFAULT_RULE_ADDRESSID": nAddressID = Convert.ToInt32(_dictexp[key]);
                                break;
                            case "DEFAULT_RULE_GROUP_FORMAT": cGROUP_FORMAT = _dictexp[key];
                                break;
                            case "RULE_CODE": cRULE_CODE = _dictexp[key];
                                break;
                            case "RULEID": nRuleID = (!string.IsNullOrEmpty(_dictexp[key])) ? Convert.ToInt32(_dictexp[key]) : 0;
                                break;
                        }
                    }
                    if (nRuleID == 0)
                    {
                        nRuleID = convert.ToInt(_Routine.Get_RuleID_By_RuleCode(cRULE_CODE));
                    }
                    if (nRuleID <= 0)
                    {
                        json = "Please select Correct Rule Code";
                    }
                    else
                    {
                        DataSet _ds = _Routine.GetRuleData_By_AddressID_GroupFormat_RuleCode(nAddressID, cGROUP_FORMAT, nRuleID);
                        if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                        {
                            json = "Rule Code '" + cRULE_CODE + "' already present for this Address. Please select another Rule Code.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }

        //added by kalpita on 04/10/2017
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CheckExistingDefaultRule(string ADDRESSID, string GROUP_FORMAT)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            bool IsExist = _Routine.CheckDuplicateDefaultRule(Convert.ToInt32(ADDRESSID), GROUP_FORMAT);
            if (IsExist) { json = "Default Rule already exists in database"; }
            return json;
        }

        //added by kalpita on 13/12/2017
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillUnLinkedDefaultRules(string ADDRESSID, string GROUP_FORMAT)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = new DataSet();
           // if (Convert.ToInt32(ADDRESSID) > 0)
            {
                _ds = _Routine.GET_ESUPPLIER_RULES_LIST_Without_AddressID(Convert.ToInt32(ADDRESSID), GROUP_FORMAT);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_ds.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }

    }
}