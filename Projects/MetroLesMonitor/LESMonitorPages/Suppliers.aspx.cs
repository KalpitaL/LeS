using AjaxControlToolkit;
using MetroLesMonitor.Bll;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace MetroLesMonitor.LESMonitorPages
{
    public partial class Suppliers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillSuppliers()
        {
            string json = "";
            int nID = 0;
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = new DataSet();
            if (HttpContext.Current.Session["ADDRESSID"] != null && HttpContext.Current.Session["ADDRESSID"] != "")
            {
                nID = Convert.ToInt32(HttpContext.Current.Session["ADDRESSID"]);
            }
            if (nID > 0)
            {
                _ds = _Routine.GetLinkedSuppliers(nID);
                DataSet dsClone = _ds.Copy();
                dsClone.Tables[0].Columns.Add("LINK_COUNT");
                //foreach (DataRow row in dsClone.Tables[0].Rows)
                //{
                //    foreach (DataColumn column in dsClone.Tables[0].Columns)
                //    {
                //        int value = Convert.ToInt32(row["ADDRESSID"]);
                //        row.SetField("LINK_COUNT", _Routine.GetLinkedBuyers_Count(value));
                //    }
                //}
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(dsClone.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }


        #region Supplier Details
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetNextSupplierNo()
        {
            string json = "";
            string cUplDwlpath = Convert.ToString(ConfigurationManager.AppSettings["UPL_DWNL_PATH"]);
            string cConfigPath = Convert.ToString(ConfigurationManager.AppSettings["CONFIG_PATH"]);
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            json = _Routine.GetNextVendorCode();
            return json +"||" + cUplDwlpath + "||" + cConfigPath;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DeleteSupplier(string SUPPLIERID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                if (!string.IsNullOrEmpty(SUPPLIERID) && Convert.ToInt32(SUPPLIERID) > 0)
                {
                    _Routine.DeleteAddress(Convert.ToInt32(SUPPLIERID), Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
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
        public static string SaveSupplierDetails(List<string> slSuppdet)
        {
            string json = "";string Addresstype = "Supplier";
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slSuppdet);
                if (_lstdet != null && _lstdet.Count > 0)
                {
                    slvalues.Clear();
                    for (int i = 0; i < _lstdet.Count; i++)
                    {
                        string lstkey = _lstdet[i][0];
                        slvalues.Add(lstkey);
                    }
                    Dictionary<string, string> _dictexp = _def.ConvertListToDictionary(slvalues);
                  //  _Routine.SaveAddress_New(Addresstype, _dictexp, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
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
        public static string CheckExistingSupplier(string ADDR_CODE, string SUPPLIERID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            bool IsExist = _Routine.CheckDuplicateAddrCode(ADDR_CODE, Convert.ToInt32(SUPPLIERID));
            if (IsExist) { json = "Supplier code already exists in database"; }
            return json;
        }

        #endregion

    
    }
}