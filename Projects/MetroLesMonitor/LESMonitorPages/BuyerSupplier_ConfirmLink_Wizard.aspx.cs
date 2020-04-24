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
    public partial class BuyerSupplier_ConfirmLink_Wizard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveBuyerSupplierLink(string SUPPLIERID, string BUYERID, List<string> slByrSppdet)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            Dictionary<string, string> slgridvalues = new Dictionary<string, string>(); slgridvalues.Clear();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            try
            {
                for (int k = 0; k < slByrSppdet.Count;k++ )
                {
                    string key = slByrSppdet[k].Split('|')[0]; string value = slByrSppdet[k].Split('|')[1];  slgridvalues.Add(key, value);
                }
                _Routine.Add_BuyerSupplier_Link_Wizard(Convert.ToInt32(SUPPLIERID), Convert.ToInt32(BUYERID), slgridvalues, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
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
        public static string GetConfigDetails()
        {
            string cConfigPath = Convert.ToString(ConfigurationManager.AppSettings["CONFIG_PATH"]);
            string cMailSubj = Convert.ToString(ConfigurationManager.AppSettings["BS_MAIL_SUBJECT"]);
            return cConfigPath + "||" + cMailSubj;
        }

    }
}