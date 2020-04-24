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
    public partial class DocumentFormat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillDocumentFormats()
        {
            string json = "";
            string cUplDwlpath = Convert.ToString(ConfigurationManager.AppSettings["UPL_DWNL_PATH"]);
            string cConfigPath = Convert.ToString(ConfigurationManager.AppSettings["CONFIG_PATH"]);
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = _Routine.GetAllDocumentFormats();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json + "||" + cUplDwlpath + "||" + cConfigPath;
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

        #region Document Format Rules
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetAllDocumentFormatRules()
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = _Routine.Display_AllDocumentFormatRules();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetUnlinkedRules(string DOCFORMATID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(DOCFORMATID))
            {
                System.Data.DataSet _ds = _Routine.GetUnAssigned_Rules(DOCFORMATID);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_ds.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetAssigned_DocumentFormatRules(string DOCFORMATID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(DOCFORMATID))
            {
                System.Data.DataSet _ds = _Routine.GetDocumentFormatRules_DocformatId(DOCFORMATID);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_ds.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveDocFormatRule(List<string> slDocformatRuledet)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> sldetvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slDocformatRuledet);
                if (_lstdet != null && _lstdet.Count > 0)
                {
                    sldetvalues.Clear();
                    for (int i = 0; i < _lstdet.Count; i++) { string lstkey = _lstdet[i][0]; sldetvalues.Add(lstkey); }
                    Dictionary<string, string> _dictfmt = _def.ConvertListToDictionary(sldetvalues);
                    _Routine.SaveDocumentFormatRules(_dictfmt, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
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
        public static string DeleteDocFormatRule(string DOCFORMATRULEID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                if (!string.IsNullOrEmpty(DOCFORMATRULEID))
                {
                    _Routine.DeleteDocumentFormatRules(DOCFORMATRULEID, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                    json = "1";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }


        #endregion
    }
}