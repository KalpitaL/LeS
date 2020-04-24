using MetroLesMonitor.Bll;
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
    public partial class Groups : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetGroupGrid()
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = new DataSet();
            _ds = _Routine.GetAllGroups();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetLinkedBuyerSupplier_Group(string GROUPID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = new DataSet();
            _ds = _Routine.Get_Buyer_Supplier_Link_By_GroupID(Convert.ToInt32(GROUPID));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetRules(string GROUPID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = new DataSet();
            _ds = _Routine.Get_Group_Specific_Rules(Convert.ToInt32(GROUPID));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetUnLinkedRules(string GROUPID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = new DataSet();
            _ds = _Routine.GetUnlinkedRules(Convert.ToInt32(GROUPID));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillCopyFromGroups()
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.GetAllGroups();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillBuyers()
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.GetAllDefaultBuyers();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillSuppliers()
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.GetAllDefaultSuppliers();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CheckExistingGroup(string GROUPCODE,string GROUPID)
        {
            string json = "";            
            SupplierRoutines _Routine = new SupplierRoutines();
            int groupCount = _Routine.ValidateGroup(GROUPCODE, Convert.ToInt32(GROUPID));
            if (groupCount > 0) json = "Group Code already exists. Please enter valid group code.";
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveGroupDetails(List<string> slGrpdet)
        {
            string json = "";
            string cGROUPID = "", cGROUP_CODE = "", cGROUP_DESC = "",cBUYER_FORMAT="",cSUPPLIER_FORMAT="",cBUYER_EXPORT_FORMAT="",
                cSUPPLIER_EXPORT_FORMAT="",cCOPY_FROM_GROUP="",cNEW_LINK="",cBUYER="",cSUPPLIER="";
            int nRFQ = 0, nQUOTE = 0,nPO = 0, nPOC = 0, nRFQ_END_STATE = 0, nQUOTE_END_STATE = 0, nPO_END_STATE = 0, nPOC_END_STATE = 0;
            int nGroupID = 0, nCopyFromGroup = 0, nBuyerID = 0,nSupplierID = 0; 
            bool chkLink = false;           
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slGrpdet);
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
                            case "GROUPID": cGROUPID = _dictexp[key];
                                nGroupID = Convert.ToInt32(cGROUPID);
                                break;
                            case "GROUP_CODE": cGROUP_CODE = _dictexp[key];
                                break;
                            case "GROUP_DESC": cGROUP_DESC = _dictexp[key];
                                break;
                            case "BUYER_FORMAT": cBUYER_FORMAT = _dictexp[key];
                                break;
                            case "SUPPLIER_FORMAT": cSUPPLIER_FORMAT = _dictexp[key];
                                break;
                            case "BUYER_EXPORT_FORMAT": cBUYER_EXPORT_FORMAT = _dictexp[key];
                                break;
                            case "SUPPLIER_EXPORT_FORMAT": cSUPPLIER_EXPORT_FORMAT = _dictexp[key];
                                break;
                            case "COPY_FROM_GROUP": cCOPY_FROM_GROUP = _dictexp[key]; nCopyFromGroup = (!string.IsNullOrEmpty(cCOPY_FROM_GROUP)) ? Convert.ToInt32(cCOPY_FROM_GROUP) : 0;
                                break;
                            case "NEW_LINK": cNEW_LINK = _dictexp[key];
                                chkLink = (cNEW_LINK == "1") ? true : false;
                                break;
                            case "BUYER": cBUYER = _dictexp[key];
                                nBuyerID = (!string.IsNullOrEmpty(cBUYER)) ? Convert.ToInt32(cBUYER) : 0;
                                break;
                            case "SUPPLIER": cSUPPLIER = _dictexp[key];
                                nSupplierID = (!string.IsNullOrEmpty(cSUPPLIER)) ? Convert.ToInt32(cSUPPLIER) : 0;
                                break;
                            case "RFQ": nRFQ = Convert.ToInt32(_dictexp[key]);
                                break;
                            case "QUOTE": nQUOTE = Convert.ToInt32(_dictexp[key]);
                                break;
                            case "PO": nPO = Convert.ToInt32(_dictexp[key]);
                                break;
                            case "POC": nPOC = Convert.ToInt32(_dictexp[key]);
                                break;
                            case "RFQ_END_STATE": nRFQ_END_STATE = Convert.ToInt32(_dictexp[key]);
                                break;
                            case "QUOTE_END_STATE": nQUOTE_END_STATE = Convert.ToInt32(_dictexp[key]);
                                break;
                            case "PO_END_STATE": nPO_END_STATE = Convert.ToInt32(_dictexp[key]);
                                break;
                            case "POC_END_STATE": nPOC_END_STATE = Convert.ToInt32(_dictexp[key]);
                                break;
                        }
                    }
                    if (nGroupID == 0)
                    {
                        nGroupID = _Routine.AddNewGroup(nGroupID, cGROUP_CODE, cGROUP_DESC, cBUYER_FORMAT, cSUPPLIER_FORMAT,
                            cBUYER_EXPORT_FORMAT, cSUPPLIER_EXPORT_FORMAT, nCopyFromGroup, nBuyerID, nSupplierID, chkLink, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                    }
                    else
                    {
                        _Routine.SaveGroup(nGroupID, cGROUP_CODE, cGROUP_DESC, cBUYER_FORMAT, cSUPPLIER_FORMAT, cBUYER_EXPORT_FORMAT,
                            cSUPPLIER_EXPORT_FORMAT, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                    }
                    if (nGroupID > 0) _Routine.SaveGroupWorkFlow(nGroupID, nRFQ, nQUOTE, nPO, nPOC, nRFQ_END_STATE, nQUOTE_END_STATE, nPO_END_STATE, nPOC_END_STATE); 
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
        public static string DeleteGroup(string GROUPID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                if (!string.IsNullOrEmpty(GROUPID) && Convert.ToInt32(GROUPID) > 0)
                {
                    _Routine.DeleteGroup(Convert.ToInt32(GROUPID), Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
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
        public static string SaveGroupRuleDetails(List<string> slGRuledet)
        {
            string json = "",cRULE_VALUE = "";
            int nGroupID = 0, nRuleID = 0, nhidval = 0;
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slGRuledet);
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
                            case "GROUPID": nGroupID = Convert.ToInt32(_dictexp[key]);
                                break;
                            case "RULEID": nRuleID = Convert.ToInt32(_dictexp[key]);
                                break;
                            case "RULE_VALUE": cRULE_VALUE = _dictexp[key];
                                break;
                            case "HID_VALUE": nhidval = Convert.ToInt32(_dictexp[key]);
                                break;
                        }
                    }
                    _Routine.UpdateInsertRuleValue(nGroupID, nRuleID, cRULE_VALUE, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                    if (nhidval == 1)
                    {
                        SmBuyerSupplierLinkCollection LinkedBuyerSuppliers = _Routine.Get_Col_BuyerSupplerLink_By_Group(nGroupID);
                        foreach (SmBuyerSupplierLink _Link in LinkedBuyerSuppliers)
                        {
                            int LinkID = convert.ToInt(_Link.Linkid);
                            System.Data.DataSet _dsLinkRule = _Routine.GetBuyerSupplier_Save_LinkRule(LinkID, nRuleID);
                            if (_dsLinkRule != null && _dsLinkRule.Tables.Count > 0 && _dsLinkRule.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow drLinkRule in _dsLinkRule.Tables[0].Rows)
                                {
                                    string Inherit = drLinkRule["INHERIT_RULE"].ToString();
                                    if (Inherit != "-1")
                                    {
                                        _Routine.UpdateInsertLinkRuleValue(LinkID, nRuleID, cRULE_VALUE, "1", Convert.ToString(HttpContext.Current.Session["UserHostServer"]),true);
                                    }
                                }
                            }
                            else
                            {
                                _Routine.UpdateInsertLinkRuleValue(LinkID, nRuleID, cRULE_VALUE, "1", Convert.ToString(HttpContext.Current.Session["UserHostServer"]),true);
                            }
                        }
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

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DeleteGroupRule(string GROUP_RULEID)
        {
            string json = "";
            int nAddressID = 0;
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                if (!string.IsNullOrEmpty(GROUP_RULEID) && Convert.ToInt32(GROUP_RULEID) > 0)
                {
                    _Routine.DeleteGroupRuleLink(Convert.ToInt32(GROUP_RULEID), Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                    json = "1";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }
    }
}