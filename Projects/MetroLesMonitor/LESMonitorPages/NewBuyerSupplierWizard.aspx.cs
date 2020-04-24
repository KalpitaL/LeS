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
    public partial class NewBuyerSupplierWizard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetDocumentFormat(string ADDRTYPE)
        {
            string json = "";
            XmlDocument doc = new XmlDocument();
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = _Routine.GetDocumentFormat_Addrtype(ADDRTYPE);
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetDocumentFormatDetails(string ID)
        {
            string json = "";            
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(ID))
            {
                Dictionary<string, string> sldet = _Routine.GetDocumentFormatDetails(ID);
                json = JsonConvert.SerializeObject(sldet);
            }
            return json;
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveDetails(string ADDRTYPE, List<string> slPartydet, List<string> slDefRuledet)
        {
            string json = "";            
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> sldetvalues = new List<string>(); List<string> slrulevalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slPartydet);
                if (_lstdet != null && _lstdet.Count > 0)
                {
                    sldetvalues.Clear();
                    for (int i = 0; i < _lstdet.Count; i++) { string lstkey = _lstdet[i][0]; sldetvalues.Add(lstkey); }
                    Dictionary<string, string> _dictparty = _def.ConvertListToDictionary(sldetvalues);
                    List<string[]> _lstruledet = _def.ConvertListToListArray(slDefRuledet);
                    json = _Routine.SaveAddress_Wizard(ADDRTYPE, _dictparty, _lstruledet, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
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
        public static string SaveDocumentFormat(List<string> slDocformatdet)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> sldetvalues = new List<string>(); 
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slDocformatdet);
                if (_lstdet != null && _lstdet.Count > 0)
                {
                    sldetvalues.Clear();
                    for (int i = 0; i < _lstdet.Count; i++) { string lstkey = _lstdet[i][0]; sldetvalues.Add(lstkey); }
                    Dictionary<string, string> _dictfmt = _def.ConvertListToDictionary(sldetvalues);
                   _Routine.SaveDocumentFormat(_dictfmt);
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
        public static string GetConfigDetails()
        {
            string cConfigPath = Convert.ToString(ConfigurationManager.AppSettings["CONFIG_PATH"]);
            string cMailSubj = Convert.ToString(ConfigurationManager.AppSettings["BS_MAIL_SUBJECT"]);          
            return cConfigPath + "||" + cMailSubj;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CheckExistingFormat(string FORMAT)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            string IsExist = _Routine.CheckExistingDocumentFormat(FORMAT);
            if (!string.IsNullOrEmpty(IsExist)) { json = "Document Format already exists in database"; }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveExcelMappings(List<string> slXMappingdet)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> sldetvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slXMappingdet);
                if (_lstdet != null && _lstdet.Count > 0)
                {
                    sldetvalues.Clear();
                    for (int i = 0; i < _lstdet.Count; i++) { string lstkey = _lstdet[i][0]; sldetvalues.Add(lstkey); }
                    Dictionary<string, string> _dictfmt = _def.ConvertListToDictionary(sldetvalues);
                    _Routine.CopyExcelMappingDetails(_dictfmt);
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
        public static string SavePDFMappings(List<string> slPMappingdet)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> sldetvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slPMappingdet);
                if (_lstdet != null && _lstdet.Count > 0)
                {
                    sldetvalues.Clear();
                    for (int i = 0; i < _lstdet.Count; i++) { string lstkey = _lstdet[i][0]; sldetvalues.Add(lstkey); }
                    Dictionary<string, string> _dictfmt = _def.ConvertListToDictionary(sldetvalues);
                    _Routine.CopyPDFMappingDetails(_dictfmt);
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
        public static string SyncDetails_server(string ID, List<string> slServdet, List<string> slServpaths)
        {
            string json = "";
            SupplierRoutines _Routines = new SupplierRoutines();
            try
            {
                string cSynQryPath = HttpContext.Current.Server.MapPath("../SyncQuery/");
                json = _Routines.SetBuyerDetails_ForSync(ID, Convert.ToString(HttpContext.Current.Session["UserHostServer"]), slServdet, slServpaths, cSynQryPath);
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetServerName_Details()
        {
            string json = "";
            string cDomain = Convert.ToString(ConfigurationManager.AppSettings["SERVER_DOMAIN"]);
            SupplierRoutines _Routines = new SupplierRoutines();
            try
            {
                json = cDomain + "|" + _Routines.GetBuyerServiceNames();
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveServerDetails(List<string> slServ)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> sldetvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slServ);
                if (_lstdet != null && _lstdet.Count > 0)
                {
                    sldetvalues.Clear();
                    for (int i = 0; i < _lstdet.Count; i++) { string lstkey = _lstdet[i][0]; sldetvalues.Add(lstkey); }
                    Dictionary<string, string> _dictfmt = _def.ConvertListToDictionary(sldetvalues);
                    _Routine.SaveServerdetails(_dictfmt);
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
        public static List<string> GetAddressName_Search(string SEARCHTEXT, string ADDRTYPE)
        {
            SupplierRoutines _Routine = new SupplierRoutines();
            List<string> slresult = new List<string>();
            slresult = _Routine.GetAddress_Search(ADDRTYPE, SEARCHTEXT);
            return slresult;
        }
    }
}