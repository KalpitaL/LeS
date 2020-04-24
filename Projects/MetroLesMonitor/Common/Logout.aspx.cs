using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroLesMonitor.Common
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null) Response.Redirect("Default.aspx");
            else
            {
                if (Request.QueryString != null && convert.ToString(Request.QueryString).ToUpper() == "LOGOUT")
                {
                    SupplierRoutines _routine = new SupplierRoutines();
                    if (Application["LOGGED_OUT_USERS"] == null)
                    {
                        Application["LOGGED_OUT_USERS"] = new List<string>();
                    }
                    List<string> lstUsers = (List<string>)Application["LOGGED_OUT_USERS"];
                    lstUsers.Add(convert.ToString(Session.SessionID));
                    Application["LOGGED_OUT_USERS"] = lstUsers;
                    _routine.SetAuditLog("LeSMonitor", "User '" + Session["LOGIN_NAME"] + "' logout using session logout.", "Updated", "", "", "", "");

                    Session.Abandon();
                    Session.Clear();
                }
                else
                {
                    SupplierRoutines _routine = new SupplierRoutines();
                    if (Application["LOGGED_OUT_USERS"] == null)
                    {
                        Application["LOGGED_OUT_USERS"] = new List<string>();
                    }
                    Session.Abandon();
                    Session.Clear();
                }
            }
        }
    }
}