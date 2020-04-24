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
    public partial class Overview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {        
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetClient_TransactionOverview_Url()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["CLIENT_TRANSACTION_OVERVIEW"]);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SetOverview(string DURATION, string ADDRTYPE)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            DataSet ds = new DataSet();  ds.Clear();
            switch (DURATION)
            {
                case "This Week": ds = _Routine.GetDocsCount_ThisWeek(ADDRTYPE);
                    break;
                case "Last Week": ds = _Routine.GetDocsCount_LastWeek(ADDRTYPE);
                    break;
                case "This Month": ds = _Routine.GetDocsCount_ByMonth(ADDRTYPE, DateTime.Now.Month, DateTime.Now.Year);
                    break;
                case "Last Month": ds = _Routine.GetDocsCount_ByMonth(ADDRTYPE, DateTime.Now.AddMonths(-1).Month, DateTime.Now.AddMonths(-1).Year);
                    break;
                case "This Year": ds = _Routine.GetDocsCount_ThisYear(ADDRTYPE);
                    break;
                case "All": ds = _Routine.GetDocsCount_All(ADDRTYPE);
                    break;
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SetOverview_Addressid(string ADDRESSID, string ADDRTYPE)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            string cThisWk = _Routine.GetDocsCount_ThisWeek_AddressId(ADDRESSID, ADDRTYPE);
            string cLastWk = _Routine.GetDocsCount_LastWeek_AddressId(ADDRESSID, ADDRTYPE);
            string cThisMonth = _Routine.GetDocsCount_ThisMonth_AddressId(ADDRESSID, ADDRTYPE);
            string cLastMonth = _Routine.GetDocsCount_LastMonth_AddressId(ADDRESSID, ADDRTYPE);
            string cThisYear = _Routine.GetDocsCount_ThisYear_AddressId(ADDRESSID, ADDRTYPE);
            Dictionary<string, string> slOverview = new Dictionary<string, string>();slOverview.Clear();
            slOverview.Add("THISWEEK", cThisWk);slOverview.Add("LASTWEEK", cLastWk);
            slOverview.Add("THISMONTH", cThisMonth);slOverview.Add("LASTMONTH", cLastMonth); slOverview.Add("THISYEAR", cThisYear);
            json = JsonConvert.SerializeObject(slOverview);
            return json;
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SetLinkedOverview_Addressid(string DURATION, string ADDRESSID, string ADDRTYPE)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines(); DataSet ds = new DataSet(); ds.Clear();            
            switch (DURATION)
            {
                case "This Week": ds = _Routine.GetLinked_DocsCount_ThisWeek_AddressId(ADDRESSID,ADDRTYPE);
                    break;
                case "Last Week": ds = _Routine.GetLinked_DocsCount_LastWeek_AddressId(ADDRESSID,ADDRTYPE);
                    break;
                case "This Month": ds = _Routine.GetLinked_DocsCount_ThisMonth_AddressId(ADDRESSID,ADDRTYPE);
                    break;
                case "Last Month": ds = _Routine.GetLinked_DocsCount_LastMonth_AddressId(ADDRESSID,ADDRTYPE);
                    break;
                case "This Year": ds = _Routine.GetLinked_DocsCount_ThisYear_AddressId(ADDRESSID,ADDRTYPE);
                    break;
                case "All": ds = _Routine.GetDocsCount_All(ADDRTYPE);
                    break;
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }
    }
}