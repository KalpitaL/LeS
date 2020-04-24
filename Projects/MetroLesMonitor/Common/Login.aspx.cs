using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Providers.Entities;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroLesMonitor.Common
{
    public partial class Login : System.Web.UI.Page
    {
        public static string cServerName, cRedirect_ServerName,cIPAddr_details;

        protected static void Page_Load(object sender, EventArgs e)
        {
          
        }

      

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string ValidateUser(string cEmail, string PassWord)
        {
            return DoLogIn(cEmail, PassWord);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DoLogIn(string cEmail, string cPassword)
        {
            string _userid = "", _password = "", _result = "";
           _userid = cEmail.Trim();
            _password = cPassword.Trim();
            {
                SupplierRoutines _supRoutine = new SupplierRoutines();
                
                int result = _supRoutine.DoLogin(_userid, _password);
                if (result > 0)
                {
                    SetSessionDetails(result);
                }
                else
                {
                    BlockUser_IP(_userid);  _result = "-1";
                }
            }
            return _result;
        }

        private static void SetSessionDetails(int result)
        {
            string cServerName = "";
            string ConfigAddressId = ConfigurationManager.AppSettings["ADMINID"].ToString();
            cServerName = Convert.ToString(ConfigurationManager.AppSettings["SERVER_NAME"]).Split('_')[0];
            SupplierRoutines _supRoutine = new SupplierRoutines();
            Default.GetUserIP();
            List<string> lstDetails = _supRoutine.GetUserDetails(result);
            HttpContext.Current.Session["USERID"] = lstDetails[0];
            HttpContext.Current.Session["ADDRESSID"] = lstDetails[1];
            HttpContext.Current.Session["USERNAME"] = lstDetails[2];
            HttpContext.Current.Session["LOGIN_NAME"] = lstDetails[3];
            HttpContext.Current.Session["ADDRTYPE"] = lstDetails[4];
            HttpContext.Current.Session["USERHOSTSERVER"] += " USER NAME : " + lstDetails[2];
            HttpContext.Current.Session["CONFIGADDRESSID"] = ConfigAddressId;
            HttpContext.Current.Session["PASSWORD"] = lstDetails[5];
            _supRoutine.CreateLoginHistory();
            if (!string.IsNullOrEmpty(cRedirect_ServerName))
            {
                HttpContext.Current.Session["SERVERNAME"] = cRedirect_ServerName;
                HttpContext.Current.Session["LOGO_SERVER_NAME"] = cRedirect_ServerName;
            }
            else
            {
                HttpContext.Current.Session["SERVERNAME"] = Convert.ToString(ConfigurationManager.AppSettings["SERVER_NAME"]);
                HttpContext.Current.Session["LOGO_SERVER_NAME"] = cServerName;
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            SupplierRoutines _supRoutine = new SupplierRoutines();
            if (Application["LOGGED_OUT_USERS"] == null)
            {
                Application["LOGGED_OUT_USERS"] = new List<string>();
            }
            List<string> lstUsers = (List<string>)Application["LOGGED_OUT_USERS"];
            lstUsers.Add(convert.ToString(Session.SessionID));

            int ntimeout = Session.Timeout;
            _supRoutine.UpdateLoginHistory(false, convert.ToString(Session.SessionID));
            Application["LOGGED_OUT_USERS"] = lstUsers;

            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));//added by kalpita on 07/09/2017
            Session.Clear();
        }
        public void ShowMessage(string Msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'> window.alert('" + Msg + "') </script>";
            Page.Controls.Add(lbl);
        }
        [WebMethod]
        public static string SetTime()
        {
            return System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SetUserSession_ID(string UserId)
        {
            string _result = "";
            string ConfigAddressId = ConfigurationManager.AppSettings["ADMINID"].ToString();
            cServerName = Convert.ToString(ConfigurationManager.AppSettings["SERVER_NAME"]).Split('_')[0];
            SupplierRoutines _supRoutine = new SupplierRoutines();
            if (Convert.ToInt32(UserId) > 0)
            {
                try
                {
                    Default.GetUserIP();
                    List<string> lstDetails = _supRoutine.GetUserDetails(Convert.ToInt32(UserId));
                    HttpContext.Current.Session["USERID"] = lstDetails[0];
                    HttpContext.Current.Session["ADDRESSID"] = lstDetails[1];
                    HttpContext.Current.Session["USERNAME"] = lstDetails[2];
                    HttpContext.Current.Session["LOGIN_NAME"] = lstDetails[3];
                    HttpContext.Current.Session["ADDRTYPE"] = lstDetails[4];
                    HttpContext.Current.Session["USERHOSTSERVER"] += " USER NAME : " + lstDetails[2];
                    HttpContext.Current.Session["CONFIGADDRESSID"] = ConfigAddressId;
                    HttpContext.Current.Session["PASSWORD"] = lstDetails[5];
                    _supRoutine.CreateLoginHistory();
                    HttpContext.Current.Session["SERVERNAME"] = Convert.ToString(ConfigurationManager.AppSettings["SERVER_NAME"]);
                    HttpContext.Current.Session["LOGO_SERVER_NAME"] = (!string.IsNullOrEmpty(cRedirect_ServerName)) ? cRedirect_ServerName : cServerName;
                }
                catch { }
            }
            else{  _result = "-1";}
            return _result;
        }

        public static void DoLogin_Redirect(string Encrypdata)
        {
          string _det= Common.Default.Decrypt_ServerURL(Encrypdata);
          string[] _lstDetails = _det.Split('&');
          string _userid = "", _password = "";
          _userid = convert.ToString(_lstDetails[0].Split('=')[1]).Trim();_password = convert.ToString(_lstDetails[1].Split('=')[1]).Trim();
          cRedirect_ServerName = convert.ToString(_lstDetails[2].Split('=')[1]).Trim();
          SupplierRoutines _supRoutine = new SupplierRoutines();
          int result = _supRoutine.DoLogin(_userid, _password);
          if (result > 0)
          {
              SetSessionDetails(result);HttpContext.Current.Response.Redirect("~/LESMonitorPages/Adaptors.aspx"); 
          }
          else{ BlockUser_IP(_userid);}
        }

        private static void BlockUser_IP(string _userid)
        {
            SupplierRoutines _supRoutine = new SupplierRoutines();
            if (ConfigurationManager.AppSettings["BLOCKIP"] != null && convert.ToString(ConfigurationManager.AppSettings["BLOCKIP"]).ToUpper() == "TRUE")
            {
                _supRoutine.SetLog("Block IP address is set to true.");
                if (HttpContext.Current.Session["E_COUNTER"] != null)
                {
                    if (convert.ToInt(HttpContext.Current.Session["E_COUNTER"]) < 5)
                    {
                        _supRoutine.SetLog("Block IP address counter " + convert.ToInt(HttpContext.Current.Session["E_COUNTER"]));
                        HttpContext.Current.Session["E_COUNTER"] = convert.ToInt(HttpContext.Current.Session["E_COUNTER"]) + 1;
                    }
                    else
                    {
                        string BlockIdDetails = "USER_ID : " + _userid + " IP : " + cIPAddr_details;
                        _supRoutine.SetLog("Block IP address details " + BlockIdDetails); _supRoutine.BlockIP(BlockIdDetails);
                    }
                }
                else{ HttpContext.Current.Session["E_COUNTER"] = 1;}
            }
        }

     
    }
}