using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroLesMonitor.MasterPages
{
    public partial class Base : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (HttpContext.Current.Session["USERID"] == null)
                {
                    Response.Redirect("~/Common/Login.aspx");
                }
                else
                {
                    //if (!string.IsNullOrEmpty(Request.Url.Query))
                    //{
                    //    Session["URL_QUERY"] = Request.Url.Query.ToString().TrimStart('?');
                    //    string EncryptionKey = convert.ToString(ConfigurationManager.AppSettings["ENCRYPT_KEY"]);
                    //    Common.Login.DoLogin_Redirect(EncryptionKey);
                    //}
                }
                int _timer = convert.ToInt(ConfigurationManager.AppSettings["SessionTimmer"]);
                if (_timer == 0) _timer = 15;
                Session["SessionStart"] = DateTime.Now.AddMinutes(_timer);
            }
            catch (Exception)
            { }
        }


       
    }

}