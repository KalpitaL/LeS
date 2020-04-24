using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace MetroLesMonitor.LESMonitorPages
{
    public partial class Buyers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {         
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillBuyerGrid()
        {
            string json = "";
            int nSupplierID=0;
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = new DataSet();
            if (HttpContext.Current.Session["ADDRESSID"] != null && HttpContext.Current.Session["ADDRESSID"] != "")
            {
                nSupplierID = Convert.ToInt32(HttpContext.Current.Session["ADDRESSID"]);
            }
            if (nSupplierID > 0)
            {
                _ds = _Routine.GetLinkedBuyers(nSupplierID);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_ds.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetNextBuyerNo()
        {
            string json = "";
            string cUplDwlpath = Convert.ToString(ConfigurationManager.AppSettings["UPL_DWNL_PATH"]);
            string cConfigPath = Convert.ToString(ConfigurationManager.AppSettings["CONFIG_PATH"]);
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            json = _Routine.GetNextBuyerCode();
            return json + "||" + cUplDwlpath + "||" + cConfigPath;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DeleteBuyers(string BUYERID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                if (!string.IsNullOrEmpty(BUYERID) && Convert.ToInt32(BUYERID) > 0)
                {
                    _Routine.DeleteAddress(Convert.ToInt32(BUYERID), Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
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
        public static string SaveBuyerDetails(List<string> slBuydet)
        {
            string json = "";            
            string Addresstype = "Buyer";
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
                   // _Routine.SaveAddress_New(Addresstype, _dictexp, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
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
        public static string CheckExistingBuyer(string ADDR_CODE, string BUYERID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            bool IsExist = _Routine.CheckDuplicateAddrCode(ADDR_CODE, Convert.ToInt32(BUYERID));
            if (IsExist) {json = "Buyer code already exists in database"; }          
            return json;
        }
    }
}