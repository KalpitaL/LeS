using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroLesMonitor
{
    public partial class Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.Url.Query))
            {
                string cEncryptURL_det = Request.Url.Query.ToString().TrimStart('?').Substring(8);
                Common.Login.DoLogin_Redirect(cEncryptURL_det);
            }
            else { Response.Redirect("~/Common/Login.aspx"); }


        }
    }
}