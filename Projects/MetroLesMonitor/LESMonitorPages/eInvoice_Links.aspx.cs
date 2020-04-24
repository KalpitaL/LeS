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
    public partial class eInvoice_Links : System.Web.UI.Page
    {
        public static string eInvoiceURL, eInvoiceExportDefault, eInvoiceImportDefault, AddressId;

        protected void Page_Load(object sender, EventArgs e)
        {
            eInvoiceURL = ConfigurationManager.AppSettings["EINVOICE_WEB_SERVICE"];
            eInvoiceExportDefault = ConfigurationManager.AppSettings["INVOICE_EXPORT_PATH_DEFAULT"];//CHANGED BY KALPITA ON 04/12/2017
            eInvoiceImportDefault = ConfigurationManager.AppSettings["INVOICE_IMPORT_PATH_DEFAULT"];
            AddressId = ConfigurationManager.AppSettings["ADMINID"].ToString();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetSupplierGrid()
        {
            string json = "";
            int nBuyerID = 0;
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = new DataSet();
            if (HttpContext.Current.Session["ADDRESSID"] != null && HttpContext.Current.Session["ADDRESSID"] != "")
            {
                nBuyerID = Convert.ToInt32(HttpContext.Current.Session["ADDRESSID"]);
            }
            if (nBuyerID > 0)
            {
                _ds = _Routine.GetLinkedSuppliers(nBuyerID);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_ds.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetCurrency()
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.GeteInvoiceCurrency();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetPaymentMode()
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.GetPaymentDetails();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetInvoiceFormats(string ADDR_TYPE)
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.GeteInvoiceFormats(ADDR_TYPE);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillSuppBuyersGrid(string SUPPLIERID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet dsLeSInvoice = new DataSet();            
            try
            {
                if (Convert.ToInt32(SUPPLIERID) > 0)
                {
                    dsLeSInvoice = _Routine.GetSuppBuyersDetails(SUPPLIERID, eInvoiceExportDefault,eInvoiceImportDefault);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(dsLeSInvoice.GetXml());
                    json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
                }
            }
            catch (Exception ex) { json = ex.Message ; }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveInvoiceDetails(string LinkID,string oBUYER_CODE,string SUPPLIER_CODE,List<string> slOlddet,List<string> slNewdet)
        {
            string json = "";
            int nLinkID=0;          
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            Dictionary<string, string> _Ndictexp = new Dictionary<string, string>();
            Dictionary<string, string> _Odictexp = new Dictionary<string, string>();
            _Ndictexp.Clear(); _Odictexp.Clear();
            List<string> slvalues = new List<string>();
            try
            {
                #region convert into Dictionary
                List<string[]> _lstolddet = _def.ConvertListToListArray(slOlddet);
                if (_lstolddet != null && _lstolddet.Count > 0)
                {
                    slvalues.Clear();
                    for (int i = 0; i < _lstolddet.Count; i++)
                    {
                        string lstkey = _lstolddet[i][0];
                        slvalues.Add(lstkey);
                    }
                    _Odictexp = _def.ConvertListToDictionary(slvalues);
                }

                List<string[]> _lstnewdet = _def.ConvertListToListArray(slNewdet);
                if (_lstnewdet != null && _lstnewdet.Count > 0)
                {
                    slvalues.Clear();
                    for (int i = 0; i < _lstnewdet.Count; i++)
                    {
                        string lstkey = _lstnewdet[i][0];
                        slvalues.Add(lstkey);
                    }
                    _Ndictexp = _def.ConvertListToDictionary(slvalues);
                    foreach (string key in _Ndictexp.Keys)
                    {
                        if (key == "LINKID") { nLinkID = Convert.ToInt32(_Ndictexp[key]); }
                    }
                    _Ndictexp.Add("BUYER_CODE", oBUYER_CODE); _Ndictexp.Add("SUPPLIER_CODE", SUPPLIER_CODE);
                }

                #endregion
                json = _Routine.Save_InvoiceDetails(nLinkID.ToString(), oBUYER_CODE, SUPPLIER_CODE, eInvoiceURL, _Odictexp, _Ndictexp, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
            }
            catch
            {
                json = "";
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CheckExistingBSLinkCode(string LINKID, string BUYER_LINKCODE, string SUPPLIER_LINKCODE)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            bool IsExist = _Routine.GetLinkBy_Buyer_Vendor_LinkCode(Convert.ToInt32(LINKID), BUYER_LINKCODE, SUPPLIER_LINKCODE);
            if (IsExist) json = "Buyer-Supplier Link Codes should be unique.";
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetInvBuyerSupplier_AccountsGrid(string INVLINKID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = _Routine.FillAccountDetailsGrid(INVLINKID);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CheckInvCurrency_AccountDetails(string INVLINKID, string CURR_ACCOUNTID, string CURR_CODE)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                System.Data.DataSet _ds = _Routine.CheckCurrency_Account(Convert.ToInt32(INVLINKID), Convert.ToInt32(CURR_ACCOUNTID), CURR_CODE);
                if(_ds!=null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                {
                    json = "Currency code already present for this link";
                }
            }
            catch (Exception ex)
            {
                json = "";
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetAccount_Currency(string CurrId)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                string Linkid = convert.ToString(HttpContext.Current.Session["LINKID"]);
                if (Linkid != "")
                {
                    json = _Routine.GetAccountDetails_Currency(CurrId, Linkid);
                }
            }
            catch (Exception ex)
            {
                json = "";
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveInvoice_AccountDetails( List<string> slInvAccdet)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            Dictionary<string, string> _dictexp = new Dictionary<string, string>(); _dictexp.Clear(); 
            List<string> slvalues = new List<string>();
            try
            {
                List<string[]> _lstolddet = _def.ConvertListToListArray(slInvAccdet);
                if (_lstolddet != null && _lstolddet.Count > 0)
                {
                    slvalues.Clear();
                    for (int i = 0; i < _lstolddet.Count; i++)
                    {
                        string lstkey = _lstolddet[i][0];
                        slvalues.Add(lstkey);
                    }
                    _dictexp = _def.ConvertListToDictionary(slvalues);
                }
              json = _Routine.Save_InvBuyerSupplier_Account(_dictexp, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
            }
            catch
            {
                json = "";
            }
            return json;
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Delete_Invoice_CurrAccount(string CURR_ACCOUNTID, string CURR_CODE)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                if (!string.IsNullOrEmpty(CURR_ACCOUNTID) && Convert.ToInt32(CURR_ACCOUNTID) > 0)
                {
                    _Routine.Delete_InvCurrency_Account(Convert.ToInt32(CURR_ACCOUNTID), CURR_CODE, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                    json = "1";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetPaymentMode_details(string PAYMENT_CODE)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            json = _Routine.GetPaymentDetails_Code(PAYMENT_CODE);          
            return json;
        }
    }
}