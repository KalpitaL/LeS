using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroLesMonitor.LESMonitorPages
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {           
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetSessionVal(string KEY)
        {
            try
            {
                if (HttpContext.Current.Session[KEY] != null)
                {
                    object sessionVal = HttpContext.Current.Session[KEY];
                    return sessionVal;
                }
                else return "";
            }
            catch (Exception ex) { return ""; }
        }

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static string GetServerList()
        //{
        //    string json = "";
        //    try
        //    {            
        //        string serverlist = Convert.ToString(ConfigurationManager.AppSettings["SERVERLIST"]);
        //        json = serverlist;
        //    }
        //    catch (Exception ex) { }
        //    return json;
        //}

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetSessionValues()
        {
            List<string> slSessionList = new List<string>();
            string ConfigAddressId = ConfigurationManager.AppSettings["ADMINID"].ToString();
            JavaScriptSerializer js = new JavaScriptSerializer();
            slSessionList.Add(HttpContext.Current.Session["USERID"].ToString());
            slSessionList.Add(HttpContext.Current.Session["USERNAME"].ToString());
            slSessionList.Add(HttpContext.Current.Session["ADDRESSID"].ToString());
            slSessionList.Add(HttpContext.Current.Session["LOGIN_NAME"].ToString());
            slSessionList.Add(HttpContext.Current.Session["ADDRTYPE"].ToString());
            slSessionList.Add(HttpContext.Current.Session["UserHostServer"].ToString());
            slSessionList.Add(ConfigAddressId);
            return js.Serialize(slSessionList);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Logout()
        {
            try
            {
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.RemoveAll();
                System.Web.Security.FormsAuthentication.SignOut();
                HttpContext.Current.Request.Cookies.Clear();
                HttpContext.Current.Response.Cookies.Clear();

                HttpContext.Current.Response.Expires = 60;
                HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.AddHeader("pragma", "no-cache");
                HttpContext.Current.Response.AddHeader("cache-control", "private");
                HttpContext.Current.Response.CacheControl = "no-cache";

                return "../Common/Login.aspx";
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.RemoveAll();
                return "";
            }
        }

     

    }
}