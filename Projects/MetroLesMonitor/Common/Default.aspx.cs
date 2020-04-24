using MetroLesMonitor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Providers.Entities;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroLesMonitor.Common
{
    public partial class Default : System.Web.UI.Page
    {
        public static string cPWDecrypted = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        public static string cPWEncrypted = @".-,+*)(~&%$#}!@?>=<;:98765^]\[ZYXWV_";
        public static string cIPAddr_details;

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
            slSessionList.Add(HttpContext.Current.Session["PASSWORD"].ToString());
            slSessionList.Add(HttpContext.Current.Session["SERVERNAME"].ToString());
            slSessionList.Add(HttpContext.Current.Session["LOGO_SERVER_NAME"].ToString());
            return js.Serialize(slSessionList);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Logout(string sesexpr)
        {
            try
            {
                SupplierRoutines _supRoutine = new SupplierRoutines();
                if (HttpContext.Current.Application["LOGGED_OUT_USERS"] == null)
                {
                    HttpContext.Current.Application["LOGGED_OUT_USERS"] = new List<string>();
                }
                List<string> lstUsers = (List<string>)HttpContext.Current.Application["LOGGED_OUT_USERS"];
                lstUsers.Add(convert.ToString(HttpContext.Current.Session.SessionID));
                bool IsSessionExpr = (sesexpr == "true") ? true : false;
                _supRoutine.UpdateLoginHistory(IsSessionExpr, convert.ToString(HttpContext.Current.Session.SessionID));
                HttpContext.Current.Application["LOGGED_OUT_USERS"] = lstUsers;

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

        public Dictionary<string, string> ConvertListToDictionary(List<string> list)
        {
            Dictionary<string, string> _dict = new Dictionary<string, string>();


            for (int i = 0; i < list.Count; i++)
            {
                string[] _arr = list[i].Split('|');
                _dict.Add(_arr[0], _arr[1]);
            }

            return _dict;
        }

        public Dictionary<string, string> ConvertListDic(List<string> list, char param)
        {
            Dictionary<string, string> _dict = new Dictionary<string, string>();
            _dict.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                string[] _arr = list[i].Split(param);

                for (int j = 0; j < _arr.Length; j++)
                {
                    string key = _arr[j].Split('|')[0];
                    string val = _arr[j].Split('|')[1];
                    _dict.Add(key, val);
                }
            }
            return _dict;
        }

        public List<string[]> ConvertListToListArray(List<string> list)
        {
            List<string[]> _lstarr = new List<string[]>();

            for (int i = 0; i < list.Count; i++)
            {
                string[] _str = list[i].Split('#');
                _lstarr.Add(_str);
            }

            return _lstarr;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string EncryptURL(string ORG_URL)
        {

            try
            {
                if (ORG_URL.Trim().Length > 0)
                {
                    string RANDOM_CODE = EncodePWD(GetRandomString());
                    string _encryptVal = RANDOM_CODE.Replace('|', 'A') + "|" + ORG_URL + "|" + RANDOM_CODE.Replace('|', 'A');
                    byte[] byteArr = System.Text.Encoding.ASCII.GetBytes(_encryptVal.Trim());
                    string _descryptedVal = Convert.ToBase64String(byteArr);
                    return _descryptedVal;
                }
                else return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string GetRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return path;
        }

        public static string EncodePWD(string cPWD)
        {

            string cOut = ""; int i;
            try
            {
                for (i = 0; i < cPWD.Length; i++)
                {
                    cOut = cOut + cPWEncrypted[cPWDecrypted.IndexOf(char.ToUpper(cPWD[i]))];
                }
                return cOut;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string DecodePWD(string cPWD)
        {
            string cOut = ""; int i;
            try
            {
                for (i = 0; i < cPWD.Length; i++)
                {
                    cOut = cOut + cPWDecrypted[cPWEncrypted.IndexOf(char.ToUpper(cPWD[i]))];
                }
                return cOut;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DecryptURL(string KEYURL)
        {
            try
            {
                string ORG_URL = "";
                if (KEYURL.Trim().Length > 0)
                {
                    byte[] byteArr = Convert.FromBase64String(KEYURL);
                    ORG_URL = System.Text.Encoding.UTF8.GetString(byteArr);
                }
                if (ORG_URL.Trim().Length > 0 && ORG_URL.Split('|').Length >= 2)
                {
                    string[] _keys = ORG_URL.Split('|');
                    ORG_URL = _keys[1];
                }
                return ORG_URL;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DecryptURL_redirect(string KEYURL)
        {
            try
            {
                string ORG_URL = "";
                if (KEYURL.Trim().Length > 0)
                {
                    byte[] byteArr = Convert.FromBase64String(KEYURL);
                    ORG_URL = System.Text.Encoding.UTF8.GetString(byteArr);
                }
                if (ORG_URL.Trim().Length > 0 && ORG_URL.Split('|').Length >= 2)
                {
                    string[] _keys = ORG_URL.Split('|');
                    ORG_URL = _keys[1] + "@" + _keys[3];
                }
                return ORG_URL;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetServerList()
        {
            string json = "";
            try
            {
                json = Convert.ToString(ConfigurationManager.AppSettings["SERVERLIST"]);
            }
            catch (Exception ex) { }
            return json;
        }

        public static string GetIPDetails()
        {
            string IPAddress = "";
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            string RemoteAddr = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            // Hostname = System.Environment.MachineName;

            Hostname = System.Net.Dns.GetHostEntry(RemoteAddr).HostName.Split('.')[0];
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                    break;
                }
            }
            return IPAddress + "|" + Hostname;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetServerDate()
        {
            return DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetFileVersion()
        {
            string version = "";
            SupplierRoutines _routines = new SupplierRoutines();
            string cIsParentMonitor = Convert.ToString(ConfigurationManager.AppSettings["IS_PARENT_MONITOR"]);
            version = _routines.Get_Version();
           return version + "|" + cIsParentMonitor;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetTabsList()
        {
            return convert.ToString(ConfigurationManager.AppSettings["TAB_LIST"]);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Encrypt_ServerURL(string ORG_URL)
        {
            string cEncryptedstr = "";
            try
            {             
                string EncryptionKey = convert.ToString(ConfigurationManager.AppSettings["ENCRYPT_KEY"]);
                byte[] clearBytes = Encoding.Unicode.GetBytes(ORG_URL);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        cEncryptedstr = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return cEncryptedstr;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Decrypt_ServerURL(string URL_DET)
        {
            string cDecryptedstr = "";
            try
            {
                string EncryptionKey = convert.ToString(ConfigurationManager.AppSettings["ENCRYPT_KEY"]);
                URL_DET = URL_DET.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(URL_DET);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cDecryptedstr = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cDecryptedstr;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetSessionData()
        {
            string rVAl = "";
            HttpSessionState session = HttpContext.Current.Session;
            DateTime? sStart = session["SessionStart"] as DateTime?;
            if (sStart.HasValue)
            {
                TimeSpan remainingTime = sStart.Value - DateTime.Now;
                int TimeOut = convert.ToInt(remainingTime.TotalSeconds);
                int h = (TimeOut / 3600);
                int m = ((TimeOut / 60) % 60);
                int s = TimeOut % 60;
                rVAl = convert.ToString(m);
            }
            return rVAl;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DisplayBlockedPage()
        {
            string _res = "-1";
            GetUserIP();
            if (ConfigurationManager.AppSettings["BLOCKIP"] != null && convert.ToString(ConfigurationManager.AppSettings["BLOCKIP"]).ToUpper() == "TRUE")
            {
                if (Check_IP_Block(cIPAddr_details)) { _res = ""; }
            }
            return _res;
        }

        private static bool Check_IP_Block(string IP_Details)
        {
            bool _result = false;
            if (IP_Details != "")
            {
                string _path = "";
                if (string.IsNullOrWhiteSpace(_path)) _path = HttpContext.Current.Server.MapPath("~/BlockIP");
                if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
                string _logFile = _path + "\\Log_BlockIP.txt";
                if (File.Exists(_logFile))
                {
                    string _IP_data = File.ReadAllText(_logFile);
                    if (_IP_data.Contains(IP_Details))
                    {
                        _result = true;
                    }
                }
            }
            return _result;
        }

        public static void GetUserIP()
        {
            try
            {
                string IPAdd = string.Empty;
                IPAdd = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                HttpContext.Current.Session["UserHostServer"] = cIPAddr_details = (!string.IsNullOrEmpty(IPAdd)) ? (IPAdd + " : " + HttpContext.Current.Request.UserHostAddress) : HttpContext.Current.Request.UserHostAddress;
            }
            catch (Exception ex)
            { }
        }

   
    }
}