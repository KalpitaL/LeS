using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    public partial class BuyerDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetBuyerDetails(string BUYERID)
        {
            string json = "";           
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(BUYERID))
            {
                Dictionary<string, string> sldet = _Routine.GetAddressDetails(Convert.ToInt32(BUYERID));
               json = JsonConvert.SerializeObject(sldet);
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillLinkedSuppliersGrid(string BUYERID)
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(BUYERID))
            {
                _ds = _Routine.GetLinkedSuppliers(Convert.ToInt32(BUYERID));
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_ds.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveBuyerDetails(List<string> slBuydet)
        {
            string json = "";
            string cBUYERID = "", cADDR_NAME = "", cADDR_CODE = "", cCONTACT_PERSON = "", cADDR_EMAIL = "", cADDR_COUNTRY = "", cWEBLINK = "",
                cADDR_INBOX = "", cADDR_OUTBOX = "";
            string Addresstype = "Buyer";
            int nBuyerID = 0;
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slBuydet);

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
                            case "BUYERID": cBUYERID = _dictexp[key];
                                nBuyerID = Convert.ToInt32(cBUYERID);
                                break;
                            case "ADDR_CODE": cADDR_CODE = _dictexp[key];
                                break;
                            case "ADDR_NAME": cADDR_NAME = _dictexp[key];
                                break;
                            case "CONTACT_PERSON": cCONTACT_PERSON = _dictexp[key];
                                break;
                            case "ADDR_EMAIL": cADDR_EMAIL = _dictexp[key];
                                break;
                            case "ADDR_COUNTRY": cADDR_COUNTRY = _dictexp[key];
                                break;
                            case "WEBLINK": cWEBLINK = _dictexp[key];
                                break;
                            case "ADDR_INBOX": cADDR_INBOX = _dictexp[key];
                                break;
                            case "ADDR_OUTBOX": cADDR_OUTBOX = _dictexp[key];
                                break;
                        }
                    }
                    _Routine.SaveAddress(nBuyerID, cADDR_CODE, cADDR_NAME, cCONTACT_PERSON, cADDR_EMAIL, cADDR_COUNTRY, cADDR_INBOX, cADDR_OUTBOX, Addresstype, cWEBLINK, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                    json = "1";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }

        #region Default Rules  ADDED ON 09/12/2017
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetBuyerDefaultRules(string BUYERID, string GROUP_FORMAT)
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(BUYERID))
            {
                _ds = _Routine.GetDefaultRules_Addressid_Format(Convert.ToInt32(BUYERID), GROUP_FORMAT);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_ds.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetRuleDetails(string RULEID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(RULEID))
            {
                json = _Routine.GetRuleDetails(Convert.ToInt32(RULEID));
            }
            return json;
        }

        #endregion  

        #region Config Settings

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetGroupFormat()
        {
            string json = "";
            XmlDocument doc = new XmlDocument();
            SupplierRoutines _Routine = new SupplierRoutines();
            //System.Data.DataSet _ds = _Routine.GetGroupFormat("Buyer");
            System.Data.DataSet _ds = _Routine.GetDocumentFormat_Addrtype("Buyer");
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetBuyerConfigDetails(string FORMAT, string BUYERID)
        {
            string json = "";
            string cConfigPath = Convert.ToString(ConfigurationManager.AppSettings["CONFIG_PATH"]);
            string cMailSubj = Convert.ToString(ConfigurationManager.AppSettings["BS_MAIL_SUBJECT"]);
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(BUYERID))
            {
                Dictionary<string, string> sldet = _Routine.GetAddressConfigDetails(FORMAT, Convert.ToInt32(BUYERID));
                json = JsonConvert.SerializeObject(sldet);
            }
            return json + "||" + cConfigPath + "||" + cMailSubj;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveBuyerConfigDetails(List<string> sldet)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(sldet);
                if (_lstdet != null && _lstdet.Count > 0)
                {
                    slvalues.Clear();
                    for (int i = 0; i < _lstdet.Count; i++)
                    {
                        string lstkey = _lstdet[i][0];                 
                        slvalues.Add(lstkey);
                    }
                    Dictionary<string, string> _dictexp = _def.ConvertListToDictionary(slvalues);
                    _Routine.SaveAddressConfig("Buyer", _dictexp, null, null);
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
        public static string CheckExistingConfig(string FORMAT,string BUYERID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(BUYERID))
            {
                json = _Routine.CheckExistingConfig(FORMAT, Convert.ToInt32(BUYERID));
            }
            return json;
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetFormat_Buyer(string BUYERID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(BUYERID))
            {
                json = _Routine.GetFormat_By_Addressid(Convert.ToInt32(BUYERID));
            }
            return json;
        }
        #endregion

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetAddressName_Search(string SEARCHTEXT, string ADDRTYPE)
        {
            SupplierRoutines _Routine = new SupplierRoutines();
            List<string> slresult = new List<string>();
            slresult = _Routine.GetAddress_Search(ADDRTYPE, SEARCHTEXT);
            return slresult;
        }
    }
}