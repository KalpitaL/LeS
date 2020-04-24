using Aspose.Cells;
using HtmlAgilityPack;
using MTML.GENERATOR;
//using HttpRoutines;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Http_Kotc_Routine
{
    public class KotcRoutine
    {
        public HTTPWrapper.HTTPWrapper _wrapper =  new HTTPWrapper.HTTPWrapper();
        public string strLogPath, strSiteURL = "", strUsername = "", strPassword = "", strProcessorName = "", strAuditPath = "", strRFQPath = "", strScreenShotPath = "",
            JSessionID = "", _logpath = "", strBuyer = "", strSupplier = "", strUser = "", strIncludeSiteData = "", strSiteFile = "", TemplateFile="";
        static string strFullList = "", strLoginPage = "", strRedirect = "";
        public int iRetry = 0, iMaxRetry = 5, ProcessPages = 0, CurrentPage = 1,ScheduleCounter=0;
        bool UserAgentApplied = false,loginpageloaded=false;
        public string[] Actions;
        HttpWebResponse response;
        Workbook _workbook = null; Worksheet _sheet = null;
        public Dictionary<string, string> dctAppSettings = new Dictionary<string, string>();
        public string LogText { set { WriteLog(value); } }

        private void WriteLog(string _logText, string _logFile = "")
        {
            if (strLogPath == null || strLogPath == "") strLogPath = AppDomain.CurrentDomain.BaseDirectory + "Log";
            string _logfile = @"Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (_logFile.Length > 0) { _logfile = _logFile; }
            if (!Directory.Exists(strLogPath))
                Directory.CreateDirectory(strLogPath);

            Console.WriteLine(_logText);
            File.AppendAllText(strLogPath + @"\" + @_logfile, DateTime.Now.ToString() + " " + _logText + Environment.NewLine);
        }

        public KotcRoutine()
        {
             TemplateFile = AppDomain.CurrentDomain.BaseDirectory + "\\Templates\\Site_Item_Details.xlsx";
            if (File.Exists(TemplateFile))
            {
                License _lic = new License();
                _lic.SetLicense("Aspose.Total.lic");

           
              
            }
        }

        public void LoadAppSettings()
        {
            LogText = "Loading AppSettings";
            if (dctAppSettings.Count > 0)
            {
                strSiteURL = dctAppSettings["SITE_URL"].Trim();
                strUsername = dctAppSettings["USERNAME"].Trim();
                strPassword = dctAppSettings["PASSWORD"].Trim();
                strAuditPath = dctAppSettings["AUDITPATH"].Trim();
                strRFQPath = dctAppSettings["RFQPATH"].Trim();
                strProcessorName = dctAppSettings["PROCESSOR_NAME"].Trim();
                strBuyer = dctAppSettings["BUYER"].Trim();
                strSupplier = dctAppSettings["SUPPLIER"].Trim();
                ProcessPages = Convert.ToInt32(dctAppSettings["PROCESS_PAGES"].Trim());
                strUser = dctAppSettings["USER"].Trim();
                Actions = dctAppSettings["ACTIONS"].Split(',');
                ScheduleCounter = Convert.ToInt32(dctAppSettings["SCHEDULE_COUNTER"].Trim());
                strIncludeSiteData = dctAppSettings["INCLUDE_SITE_DETAILS"].Trim();
                strScreenShotPath = AppDomain.CurrentDomain.BaseDirectory + "ScreenShots";
                if (!Directory.Exists(strScreenShotPath)) Directory.CreateDirectory(strScreenShotPath);
                if (strRFQPath == "") strRFQPath = AppDomain.CurrentDomain.BaseDirectory + strUser + "\\XML";
                if (!Directory.Exists(strRFQPath)) Directory.CreateDirectory(strRFQPath);
                if (strAuditPath == "") strAuditPath = AppDomain.CurrentDomain.BaseDirectory + "Audit";
                if (dctAppSettings.ContainsKey("LOGINRETRY")) iMaxRetry = Convert.ToInt32(dctAppSettings["LOGINRETRY"]);
                if (strSiteFile == "") strSiteFile = AppDomain.CurrentDomain.BaseDirectory + strUser + "\\SiteFiles";
                if (!Directory.Exists(strSiteFile)) Directory.CreateDirectory(strSiteFile);
            }
        }

        private void UpdateSessionCookies(string Url)
        {
            for (int i = 0; i < _wrapper._CurrentResponse.Headers.Count; i++)
            {
                string name = _wrapper._CurrentResponse.Headers.GetKey(i);
                if (name == "Set-Cookie")
                {
                    string value = _wrapper._CurrentResponse.Headers.Get(i);
                    _wrapper._CookieContainer.SetCookies(new Uri(Url), value);
                    _wrapper.SetSessionID(Url,"JSESSIONID");
                    //string[] _SessionValues = value.Split(',');
                    //foreach (string _session in _SessionValues)
                    //{
                    //    string[] _val = _session.Split('=');
                    //    if (_val[0] == _wrapper.CooKieName)
                    //    {
                    //        if (!_wrapper._dctSetCookie.ContainsKey(_val[0])) _wrapper._dctSetCookie.Add(_val[0], _val[1].Split(';')[0]);
                    //        else _wrapper._dctSetCookie[_wrapper.CooKieName] = _val[1].Split(';')[0];
                    //        _wrapper.SessionID = _val[1].Split(';')[0];
                    //        Cookie _add = new Cookie(_wrapper.CooKieName, _wrapper.SessionID);
                    //        _wrapper._CookieContainer.SetCookies(new Uri(Url),value);
                    //    }
                    //    else if (_val[0] == "TS01c15d28" && !_wrapper._dctSetCookie.ContainsKey("TS01c15d28"))
                    //    {
                    //        _wrapper._dctSetCookie.Add(_val[0], _val[1].Split(';')[0]);
                    //        Cookie _add = new Cookie("TS01c15d28", _val[1].Split(';')[0]);
                    //        _wrapper._CookieContainer.Add(new Uri(Url), _add);
                    //    }
                    //}
                    break;
                }
            }
        }

        private bool LoadLoginPage(string URL)
        {
            LogText = "Loading login page.";
            _wrapper.CooKieName = "JSESSIONID";
            bool isLoaded = false;
            //_wrapper._CookieContainer = new CookieContainer();

            if (dctAppSettings.ContainsKey("USERAGENT") && dctAppSettings["USERAGENT"] != "")
            {               
                _wrapper.UserAgent = dctAppSettings["USERAGENT"]; // added by simmy
                LogText = "User agent set to " + _wrapper.UserAgent;
            }

            if (_wrapper.SessionID != null)
            {
                if (_wrapper._SetRequestHeaders.ContainsKey(HttpRequestHeader.Cookie)) _wrapper._SetRequestHeaders[HttpRequestHeader.Cookie] = @"JSESSIONID=" + _wrapper.SessionID + "; TS01c15d28=" + _wrapper._dctSetCookie["TS01c15d28"] + "; PROD=" + _wrapper._dctSetCookie["PROD"] + "; BIGipServer~DMZ_w_ASM_2018~ext_erp.app~ext_erp_pool=" + _wrapper._dctSetCookie["BIGipServer~DMZ_w_ASM_2018~ext_erp.app~ext_erp_pool"] + "; TS0163343d=" + _wrapper._dctSetCookie["TS0163343d"] + "; oracle.uix=0^^GMT+5:30^p";
                else _wrapper._SetRequestHeaders.Add(HttpRequestHeader.Cookie, @"JSESSIONID=" + _wrapper.SessionID + "; TS01c15d28=" + _wrapper._dctSetCookie["TS01c15d28"] + "; PROD=" + _wrapper._dctSetCookie["PROD"] + "; BIGipServer~DMZ_w_ASM_2018~ext_erp.app~ext_erp_pool=" + _wrapper._dctSetCookie["BIGipServer~DMZ_w_ASM_2018~ext_erp.app~ext_erp_pool"] + "; TS0163343d=" + _wrapper._dctSetCookie["TS0163343d"] + "; oracle.uix=0^^GMT+5:30^p");
            }
            else {
                _wrapper._SetRequestHeaders.Remove(HttpRequestHeader.Cookie);
                _wrapper._CookieContainer = new CookieContainer();
            }

            if (_wrapper.LoadURL(strSiteURL, "", "", "","JSESSIONID","",false))//out response, 
            {
                
                    UpdateSessionCookies("https://erp.kotc.com.kw/OA_HTML/OA.jsp?OAFunc=OAHOMEPAGE");
                
                    _wrapper._CurrentResponseString = _wrapper.ReadResponse(_wrapper._CurrentResponse);
                    _wrapper._CurrentDocument.LoadHtml(_wrapper._CurrentResponseString);
                     _wrapper._dctStateData = getStateInfo(_wrapper._CurrentResponseString, _wrapper._CurrentDocument);
                    if (_wrapper.GetElement("button", "id", "SubmitButton") != null) {
                        isLoaded = true;
                        LogText = "Loaded Successfully.";
                        loginpageloaded = true;
                        }
                
                
                
            }
            else
            {
                
                if (dctAppSettings.ContainsKey("USERAGENT1") && dctAppSettings["USERAGENT1"] != "")
                {
                    _wrapper.UserAgent = dctAppSettings["USERAGENT1"]; // added by simmy                    
                    LogText = "User agent set to " + _wrapper.UserAgent;  
                }
                if (_wrapper.LoadURL(strSiteURL, "button", "id", "SubmitButton","JSESSIONID"))//out response, 
                {
                    LogText = "Loaded Successfully.";                
                    isLoaded = true;
                    UpdateSessionCookies("https://erp.kotc.com.kw/OA_HTML/OA.jsp?OAFunc=OAHOMEPAGE");
                    loginpageloaded = true;
                }
            }
            return isLoaded;
        }

        private Dictionary<string, string> getStateInfo(string _CurrentResponseString, HtmlDocument _doc = null)
        {
            Dictionary<string, string> dctState = new Dictionary<string, string>();
            if (_doc == null)
            {
                _doc = new HtmlAgilityPack.HtmlDocument();
                if (_CurrentResponseString != "") _doc.LoadHtml(_CurrentResponseString);
            }

            HtmlNodeCollection _nodeCollection = _doc.DocumentNode.SelectNodes("//input[@type='hidden']");
            string sKey = "", sValue = "";
            if (_nodeCollection != null)
            {
                foreach (HtmlNode _hiddenNode in _nodeCollection)
                {
                    if ((_hiddenNode.Attributes["value"] != null) && (_hiddenNode.Attributes["id"] != null))
                    {
                       
                            sValue = HttpUtility.UrlEncode(_hiddenNode.Attributes["value"].Value);
                            sKey = HttpUtility.UrlEncode(_hiddenNode.Attributes["id"].Value);
                       
                        
                        if (sKey != "" && !dctState.ContainsKey(sKey)) dctState.Add(sKey, sValue);
                    }
                }
            }
            return dctState;

        }

        public bool doLogin()
        {
            bool isLoggedin = false;
            try
            {
                // LogText = "Login process started";
                //if (_wrapper.LoadURL(out response, strSiteURL, "button", "id", "SubmitButton"))
                //_wrapper.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";

                // if (_wrapper.LoadURL(strSiteURL, "button", "id", "SubmitButton"))//out response, 
                if (!loginpageloaded)
                {
                    loginpageloaded = LoadLoginPage(strSiteURL);
                }
                if (loginpageloaded)
                {
                    LogText = "URL Loaded successfully " + strSiteURL;
                    // response.Close();

                    Dictionary<string, string> dicState = _wrapper._dctStateData;
                    if (dicState != null)
                    {
                        #region post data like fiddler(not used,donot delete)
                        //string strPostData = @"_AM_TX_ID_FIELD=" + dicState["_AM_TX_ID_FIELD"] + "&_FORM=" + dicState["_FORM"] + "&usernameField=" + HttpUtility.UrlEncode(strUsername) + "&passwordField=" + strPassword + "&Accessibility=N&_fwkAbsolutePageName=" + dicState["_fwkAbsolutePageName"] + "&SubmitButton%24%24unvalidated=" + dicState["SubmitButton$$unvalidated"] + "&SubmitButton%24%24serverUnvalidated=" + dicState["SubmitButton$$serverUnvalidated"] + "&SubmitButton%24%24processFormDataCalled=" + dicState["SubmitButton$$processFormDataCalled"] + "&Cancel%24%24unvalidated=" + dicState["Cancel$$unvalidated"] + "&Cancel%24%24serverUnvalidated=" + dicState["Cancel$$serverUnvalidated"] + "&Cancel%24%24processFormDataCalled=" + dicState["Cancel$$processFormDataCalled"] + "&_FORMEVENT=&serverValidate=&evtSrcRowIdx=&evtSrcRowId=&_FORM_SUBMIT_BUTTON=SubmitButton&event=&source=&_ssoLangCode=";
                        #endregion
                        string strPostData = @"_AM_TX_ID_FIELD=" + dicState["_AM_TX_ID_FIELD"] + "&_FORM=" + dicState["_FORM"] + "&usernameField=" + HttpUtility.UrlEncode(strUsername) + "&passwordField=" + strPassword + "&_FORM_SUBMIT_BUTTON=SubmitButton";

                        _wrapper._SetRequestHeaders[HttpRequestHeader.Cookie] = @"JSESSIONID=" + _wrapper.SessionID + "; TS01c15d28=" + _wrapper._dctSetCookie["TS01c15d28"] + "; PROD=" + _wrapper._dctSetCookie["PROD"] + "; BIGipServer~DMZ_w_ASM_2018~ext_erp.app~ext_erp_pool=" + _wrapper._dctSetCookie["BIGipServer~DMZ_w_ASM_2018~ext_erp.app~ext_erp_pool"] + "; TS0163343d=" + _wrapper._dctSetCookie["TS0163343d"] + "; oracle.uix=0^^GMT+5:30^p";

                        HtmlAgilityPack.HtmlNode FrmLoginPage = _wrapper.GetElement("form", "id", "DefaultFormName");
                        if (FrmLoginPage != null) strLoginPage = FrmLoginPage.Attributes["action"].Value;
                        if (strLoginPage != "")
                        {
                            LogText = "strLoginPage : " + strLoginPage;
                            //referer= "https://erp.kotc.com.kw/OA_HTML/RF.jsp?function_id=32875&resp_id=-1&resp_appl_id=-1&security_group_id=0&lang_code=US&params=mMjexmhD-oML0HZkrLMarBuz6LgJBkwo24QiM.xyfmQ-Vdo47.5tk6s2IvJAHAj6"
                            // if (_wrapper.PostURL(out response, "https://erp.kotc.com.kw" + strLoginPage, strPostData, strSiteURL.Replace("&amp;", "&"), false, "Sourcing Supplier"))
                            if (_wrapper.PostURL("https://erp.kotc.com.kw" + strLoginPage, strPostData, "", "", ""))
                            //if (_wrapper.PostURL("https://erp.kotc.com.kw" + strLoginPage, strPostData, strSiteURL.Replace("&amp;", "&"), false, "Sourcing Supplier"))
                            {
                                UpdateSessionCookies("https://erp.kotc.com.kw" + strLoginPage);
                                LogText = "Loading home page";
                                bool HomeLoaded = _wrapper.LoadURL("https://erp.kotc.com.kw/OA_HTML/OA.jsp?OAFunc=OAHOMEPAGE", "tr", "id", "worklistRow", "JSESSIONID", "id", true);
                                if (HomeLoaded) LogText = "Home page loaded successfully.";
                                else //LogText = "Home page not loaded.";//06-05-19
                                    throw new Exception("Home page not loaded.");//06-05-19


                                if (_wrapper._CurrentResponseString.ToUpper().Contains("LOGGED IN AS") && _wrapper._CurrentResponseString.ToUpper().Contains(strUsername.ToUpper()))
                                {
                                    LogText = "Logged in as " + strUsername;
                                    iRetry = 0; // added by simmy 21.02.2019
                                    if (dctAppSettings.ContainsKey("LOGINRETRY")) iMaxRetry = Convert.ToInt32(dctAppSettings["LOGINRETRY"]);
                                    else iMaxRetry = 5; // added by simmy 21.02.2019
                                    loginpageloaded = false;
                                    UpdateSessionCookies("https://erp.kotc.com.kw/OA_HTML/OA.jsp?OAFunc=OAHOMEPAGE");

                                    LogText = "Post url success https://erp.kotc.com.kw" + strLoginPage;

                                    //_wrapper.SetSessionID("https://erp.kotc.com.kw" + strLoginPage, "PROD");
                                    //if (_wrapper.LoadURL(out response, "https://erp.kotc.com.kw/OA_HTML/RF.jsp?function_id=17573&resp_id=23415&resp_appl_id=396&security_group_id=0&lang_code=US&params=Sg2.PvtzvosdAP0Uaf8DLM3-4MilvZgfdr8FqiKj73g", "Your Company's Open Invitations"))//, strSiteURL
                                    if (_wrapper.LoadURL("https://erp.kotc.com.kw/OA_HTML/RF.jsp?function_id=17573&resp_id=23415&resp_appl_id=396&security_group_id=0&lang_code=US&params=Sg2.PvtzvosdAP0Uaf8DLM3-4MilvZgfdr8FqiKj73g", "table", "id", "NegGlanceTableRgn", "JSESSIONID"))//, strSiteURL
                                    {
                                        if (_wrapper._CurrentResponseString.Contains("Your Company's Open Invitations"))
                                        {
                                            UpdateSessionCookies("https://erp.kotc.com.kw/OA_HTML/RF.jsp?function_id=17573&resp_id=23415&resp_appl_id=396&security_group_id=0&lang_code=US&params=Sg2.PvtzvosdAP0Uaf8DLM3-4MilvZgfdr8FqiKj73g");
                                            string c = GetScheduleCounter();//added on 16-4-18
                                            if (c != "") File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + strUser + "\\Schedule_ErrorLog.txt", "");
                                            return true;
                                        }
                                    }
                                    else throw new Exception("Unable to load page after login."); // added by simmy 21.02.2019
                                }
                                else
                                {
                                    if (_wrapper.LoadURL("https://erp.kotc.com.kw/OA_HTML/RF.jsp?function_id=17573&resp_id=23415&resp_appl_id=396&security_group_id=0&lang_code=US&params=Sg2.PvtzvosdAP0Uaf8DLM3-4MilvZgfdr8FqiKj73g", "", "", "", "JSESSIONID"))//, strSiteURL
                                    {
                                        if (_wrapper._CurrentResponseString.Contains("Your Company's Open Invitations"))
                                        {
                                            string c = GetScheduleCounter();//added on 16-4-18
                                            if (c != "") File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + strUser + "\\Schedule_ErrorLog.txt", "");
                                            return true;
                                        }
                                    }
                                    else throw new Exception("Unable to load url https://erp.kotc.com.kw/OA_HTML/RF.jsp?function_id=17573&resp_id=23415&resp_appl_id=396&security_group_id=0&lang_code=US&params=Sg2.PvtzvosdAP0Uaf8DLM3-4MilvZgfdr8FqiKj73g");
                                }
                            }
                            else
                            {
                                if (_wrapper._CurrentResponseString.Contains("Logout"))
                                {
                                    LogText = "1 Post url success https://erp.kotc.com.kw" + strLoginPage;
                                    string strFilename = strScreenShotPath + "\\KotcLogin_Test1_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + strSupplier + ".png";
                                    _wrapper.PrintScreen(strFilename);

                                    _wrapper.SetSessionID("https://erp.kotc.com.kw" + strLoginPage, "PROD");
                                    if (_wrapper.LoadURL("https://erp.kotc.com.kw/OA_HTML/RF.jsp?function_id=17573&resp_id=23415&resp_appl_id=396&security_group_id=0&lang_code=US&params=Sg2.PvtzvosdAP0Uaf8DLM3-4MilvZgfdr8FqiKj73g", "", "", "", "JSESSIONID"))//, strSiteURL
                                    {
                                        if (_wrapper._CurrentResponseString.Contains("Your Company's Open Invitations"))
                                        {
                                            string c = GetScheduleCounter();//added on 16-4-18
                                            if (c != "") File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + strUser + "\\Schedule_ErrorLog.txt", "");
                                            return true;
                                        }
                                    }
                                    else throw new Exception("Unable to load url https://erp.kotc.com.kw/OA_HTML/RF.jsp?function_id=17573&resp_id=23415&resp_appl_id=396&security_group_id=0&lang_code=US&params=Sg2.PvtzvosdAP0Uaf8DLM3-4MilvZgfdr8FqiKj73g");
                                    //else {
                                    //    if (_wrapper._http._CurrentResponseString.Contains("Unable to authenticate session."))
                                    //   return false;
                                    //}
                                }
                                else
                                {
                                    if (iRetry > iMaxRetry)
                                    {
                                        string strFilename = strScreenShotPath + "\\KotcLogin_Error_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + strSupplier + ".png";
                                        _wrapper.PrintScreen(strFilename);
                                        LogText = "Login failed for user " + strUser;
                                        isLoggedin = false;

                                        string c = GetScheduleCounter();//added on 16-4-18
                                        if (c == "") { c = "1"; File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + strUser + "\\Schedule_ErrorLog.txt", c); }
                                        else if (c != "" && Convert.ToInt32(c) < ScheduleCounter) { c = Convert.ToString(Convert.ToInt32(c) + 1); File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + strUser + "\\Schedule_ErrorLog.txt", c); }

                                        if (!_wrapper._CurrentResponseString.ToLower().Contains("you have encountered an unexpected error."))
                                        {
                                            if (Convert.ToInt32(c) == ScheduleCounter)
                                            {
                                                //CreateAuditFile(strFilename, strProcessorName, "", "Error", "Unable to login for user " + strUser);
                                                CreateAuditFile(strFilename, strProcessorName, "", "Error", "LeS-1014:Unable to login for " + strUser);
                                                c = "0";
                                                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + strUser + "\\Schedule_ErrorLog.txt", c);
                                            }
                                        }
                                        else
                                        {
                                            if (LogOut())
                                            {
                                                LogText = "User " + strUsername + " logged out successfully.";
                                            }
                                        }
                                        iRetry = 0;
                                        if (dctAppSettings.ContainsKey("LOGINRETRY")) iMaxRetry = Convert.ToInt32(dctAppSettings["LOGINRETRY"]);
                                        else iMaxRetry = 5;
                                    }
                                    else
                                    {
                                        iRetry++;
                                        LogText = "Login retry";
                                        isLoggedin = doLogin();
                                    }
                                }
                            }
                        }
                        else throw new Exception("Unable to build login url."); // added by simmy 21.02.2019
                    }
                }


                else
                {
                    LogoutPreviousSession();
                    if (iRetry > iMaxRetry)
                    {
                        if (((iRetry - iMaxRetry) == 1) && loginpageloaded)
                        {
                            iRetry++;
                            LogText = "Login page loaded after retry " + iRetry;
                            LogText = "Trying again";
                            isLoggedin = doLogin();
                        }
                        else
                        {
                            string strFilename = strScreenShotPath + "\\KotcLogin_Error_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + strSupplier + ".png";
                            _wrapper.PrintScreen(strFilename);
                            LogText = "Unable to load url for login";
                            isLoggedin = false;
                            //CreateAuditFile(strFilename, strProcessorName, "", "Error", "Unable to load url for login");//commented on 7/3/18, as exception occurs many times
                            CreateAuditFile(strFilename, strProcessorName, "", "Error", "LeS-1016:Unable to load url");
                            iRetry = 0;
                            if (dctAppSettings.ContainsKey("LOGINRETRY")) iMaxRetry = Convert.ToInt32(dctAppSettings["LOGINRETRY"]);
                            else iMaxRetry = 5;
                        }
                    }
                    else
                    {
                        iRetry++;
                        LogText = "Loading Login Page retry.";
                        isLoggedin = doLogin();
                    }
                }
                //    _wrapper.LoadURL("www.google.com","");
            }
            catch (Exception e)
            {
                string strFilename = strScreenShotPath + "\\KotcLogin_Error_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + strSupplier + ".png";
                if (!_wrapper.PrintScreen(strFilename)) strFilename = "";
                LogText = "Exception while login : " + e.GetBaseException().ToString();
                //CreateAuditFile(strFilename, strProcessorName, "", "Error", "Exception while login : " + e.Message);//added by simmy 21.02.2019
                CreateAuditFile(strFilename, strProcessorName, "", "Error", "LeS-1014:Unable to login due to " + e.Message);
            }
            finally { }
            return isLoggedin;
        }

        private void LogoutPreviousSession()
        {
            try {
                LogText = "Logging out previous session";
                string url = "https://erp.kotc.com.kw/OA_HTML/OA.jsp?OAFunc=OAHOMEPAGE";
                _wrapper.LoadURL(url, "", "", "", "JSESSIONID", "id", true);
                UpdateSessionCookies(url);
                if (_wrapper._CurrentResponseString.Contains("Logout")) LogOut();
                //HtmlNode _submitbtn = _wrapper.GetElement("button", "id", "SubmitButton");
                //if (_submitbtn != null)
                //{
                //    LogText = "Redirected to login page after previous session logout.";
                //    loginpageloaded = true;
                //}


            }
            catch (Exception e)
            {
                LogText = "Exception in LogoutPreviousSession : " + e.GetBaseException().ToString();
            }
        }

        public void Process_RFQ(List<string> slProcessedPO)
        {
            LogText = "Processing RFQ";
            bool isLastPage = false;
            string sValue = "", sNegNo = "", sURL = "";
            if (ProcessPages <= 0)
            {
                ProcessPages = 1;
            }
            List<string> slDownloadedRFQ = new List<string>();
            List<string> slHref = new List<string>();
            try
            {
                int iPageCnt = 1;
                while ((!isLastPage) && iPageCnt <= ProcessPages)
                {
                    GetLinkAddress(ref slHref, slProcessedPO, iPageCnt);
                    HtmlAgilityPack.HtmlDocument _doc = new HtmlAgilityPack.HtmlDocument();
                    if (_wrapper._CurrentResponseString.Contains("Next functionality disabled")) isLastPage = true;
                    else if (!_wrapper._CurrentResponseString.Contains("Next")) isLastPage = true;
                    else
                    {
                        int nextRfqRows = 25 * (iPageCnt) + 1;
                        #region post data like fiddler(not used,donot delete)
                        //string strPaging = @"_AM_TX_ID_FIELD=2&_FORM=DefaultFormName&_fwkAbsolutePageName=%2Foracle%2Fapps%2Fpon%2Fhomepages%2Fselling%2Fwebui%2FponOpenInvPG%3A%3A%3A&N4%24%24unvalidated=false&" +
                        //    "N4%24%24serverUnvalidated=false&_FORMEVENT=&serverValidate=&evtSrcRowIdx=&evtSrcRowId=&event=goto&source=N4&value=" + nextRfqRows + "&size=25&partialTargets=ResultTable&partial=true&state=";
                        #endregion
                        string strPaging = @"_AM_TX_ID_FIELD=2&_FORM=DefaultFormName&_fwkAbsolutePageName=%2Foracle%2Fapps%2Fpon%2Fhomepages%2Fselling%2Fwebui%2FponOpenInvPG%3A%3A%3A&event=goto&source=N4&value=" + nextRfqRows + "&size=25&partialTargets=ResultTable";
                        HtmlAgilityPack.HtmlNode FrmRedirect = _wrapper.GetElement("form", "id", "DefaultFormName");
                        if (FrmRedirect != null) strRedirect = "https://erp.kotc.com.kw" + FrmRedirect.Attributes["action"].Value;
                        string[] ArrReferer = strRedirect.Split('&');
                        string strReferer = "https://erp.kotc.com.kw/OA_HTML/OA.jsp?OAFunc=PONENQVOI_OPENINVITATION&app=selling&&addBreadCrumb=Y&" + ArrReferer[4] + "&oapc=4";
                        _wrapper.Referrer = strReferer;
                        if (_wrapper.PostURL(strRedirect, strPaging,"","",""))
                            iPageCnt++;
                    }
                }
            }
            catch (Exception e)
            {
                LogText = "Exception while fetching RFQ links : " + e.GetBaseException().ToString();
            }

            try
            {
                if (slHref.Count == 0) LogText = "No new RFQ(s) found.";
                else LogText = slHref.Count + " new RFQ(s) found.";
                foreach (string slink in slHref)
                {
                    sURL = slink.Split('|')[0];
                    sValue = slink.Split('|')[1];
                    sNegNo = slink.Split('|')[2];
                    if (_wrapper.LoadURL( sURL, "table", "id", "CurrencyHeader","JSESSIONID"))
                    {
                        UpdateSessionCookies(sURL);
                        DownloadAttachment(sNegNo, sValue, sURL, slProcessedPO);
                    }
                    else
                    {
                        if (AcceptParticipation(sURL))
                            DownloadAttachment(sNegNo, sValue, sURL, slProcessedPO);
                        else
                        {
                            string strFilename = strScreenShotPath + "\\KotcRFQ_Error_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + strSupplier + ".png";
                            _wrapper.PrintScreen(strFilename);
                            LogText = "Unable to navigate to link for " + sValue;
                            //CreateAuditFile(strFilename, strProcessorName, "", "Error", "Unable to navigate to link for " + sValue);
                            CreateAuditFile(strFilename, strProcessorName, "", "Error", "LeS-1016:Unable to load url for " + sValue);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogText = "Exception while processing RFQ links : " + e.GetBaseException().ToString();
//                CreateAuditFile("", strProcessorName, sValue, "Downloaded", "Exception while processing RFQ links : " + e.GetBaseException().ToString());
                CreateAuditFile("", strProcessorName, sValue, "Downloaded", "LeS-1004:Unable to process file due to " + e.GetBaseException().ToString());
            }
            LogText = "RFQ Processing Completed.";
        }

        public void GetLinkAddress(ref List<string> slHref, List<string> lstProcessedItem, int iPageCnt)
        {
            List<string> _lstdetails = new List<string>();
            HtmlAgilityPack.HtmlNode btnFullList = _wrapper.GetElement("button", "id", "InvFullListBtn");
            if (btnFullList != null) strFullList = btnFullList.Attributes["onclick"].Value;
           // response.Close();//17-2-2018
            if (iPageCnt == 1)
            {
                if (_wrapper._CurrentResponseString.Contains("Your Company\'s Open Invitations"))
                {
                    string strInvitationLink = strFullList.Replace("document.location='", "https://erp.kotc.com.kw").TrimEnd('\'');//Invitation fulllist 
                    strInvitationLink = strInvitationLink.Replace("&amp;", "&");
                    if (strInvitationLink != "")
                    {
                        _wrapper._SetRequestHeaders[HttpRequestHeader.Cookie] = @"JSESSIONID=" + _wrapper.SessionID + "; TS01c15d28=" + _wrapper._dctSetCookie["TS01c15d28"] + "; PROD=" + _wrapper._dctSetCookie["PROD"] + "; BIGipServer~DMZ_w_ASM_2018~ext_erp.app~ext_erp_pool=" + _wrapper._dctSetCookie["BIGipServer~DMZ_w_ASM_2018~ext_erp.app~ext_erp_pool"] + "; TS0163343d=" + _wrapper._dctSetCookie["TS0163343d"] + "; oracle.uix=0^^GMT+5:30^p";
                        //_wrapper.LoadURL(out response, strInvitationLink, "a", "title", "Return to RFQ/E-Tendering");
                        _wrapper.LoadURL( strInvitationLink, "a", "title", "Return to RFQ/E-Tendering","JSESSIONID");
                    }
                }
            }
            LogText = "Navigate to page " + iPageCnt;
            HtmlAgilityPack.HtmlDocument _doc = new HtmlAgilityPack.HtmlDocument();
            _doc.LoadHtml(_wrapper._CurrentResponseString);
            HtmlNodeCollection _nodes = _doc.DocumentNode.SelectNodes("//span[@id='ResultTable']");//get open invitations table
            if (_nodes!=null && _nodes.Count == 1 && _nodes[0].ChildNodes.Count > 0 && _nodes[0].ChildNodes.Count == 3)
            {
                if (_nodes[0].ChildNodes[1].OuterHtml.Contains("Supplier Site"))
                {
                    HtmlNodeCollection _nodesc = _nodes[0].ChildNodes[1].SelectNodes(".//tr");
                    if (_nodesc.Count > 0)
                    {
                        foreach (HtmlNode _row in _nodesc)
                        {
                            HtmlNodeCollection _rowData = _row.ChildNodes;

                            if (!_rowData[1].InnerText.Contains("Negotiation Number"))
                            {
                                if (!_rowData[0].InnerText.Contains("No results found"))//added on 31-3-2018
                                {
                                    string InqNo = _rowData[1].SelectSingleNode(".//a").GetAttributeValue("title", "");
                                    string Title = _rowData[2].InnerText.Trim().Substring(0, 13);
                                    if (!lstProcessedItem.Contains(Title))//check RFQ already processed or not
                                    {
                                        if (!_lstdetails.Contains(Title))
                                        {
                                            string strOInvLink = "https://erp.kotc.com.kw/OA_HTML/" + _rowData[1].SelectSingleNode(".//a").GetAttributeValue("href", "");
                                            if (strOInvLink != "")
                                            {
                                                slHref.Add(strOInvLink + "|" + Title + "|" + InqNo);
                                                LogText = "RFQ added to list for processing '" + Title + "'";
                                            }
                                        }
                                        else
                                        {
                                            LogText = "RFQ alread added to the list '" + Title + "'";
                                        }
                                    }
                                    else
                                    {
                                        LogText = "RFQ for ref " + Title + " already downloaded.";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool AcceptParticipation(string strOInvLink)
        {
            bool result = false;
            if (_wrapper._CurrentResponseString.Contains("Will your company participate?"))
            {
                Dictionary<string, string> dictState = _wrapper._dctStateData;
                #region post data like fiddler(not used,donot delete)
                //string strAckPsot = @"_AM_TX_ID_FIELD=" + dictState["_AM_TX_ID_FIELD"] + "&_FORM=" + dictState["_FORM"] +
                //    "&AckRadioChoiceGroup=Y&AckNoteText=&_fwkAbsolutePageName=" + dictState["_fwkAbsolutePageName"] + "&cancel%24%24unvalidated=" +
                //    dictState["cancel$$unvalidated"] + "&cancel%24%24serverUnvalidated=" + dictState["cancel$$serverUnvalidated"] + "&cancel%24%24processFormDataCalled=" +
                //    dictState["cancel$$processFormDataCalled"] + "&ApplyBtn%24%24unvalidated=" + dictState["ApplyBtn$$unvalidated"] + "&ApplyBtn%24%24serverUnvalidated=" +
                //    dictState["ApplyBtn$$serverUnvalidated"] + "&ApplyBtn%24%24processFormDataCalled=" +
                //    dictState["ApplyBtn$$processFormDataCalled"] + "&_FORMEVENT=&serverValidate=&evtSrcRowIdx=&evtSrcRowId=&_FORM_SUBMIT_BUTTON=ApplyBtn";
                #endregion
                string strAckPsot = @"_AM_TX_ID_FIELD=" + _wrapper._dctStateData["_AM_TX_ID_FIELD"] + "&_FORM=" + dictState["_FORM"] +
             "&AckRadioChoiceGroup=Y&_fwkAbsolutePageName=" + _wrapper._dctStateData["_fwkAbsolutePageName"] + "&_FORM_SUBMIT_BUTTON=ApplyBtn";
                HtmlAgilityPack.HtmlNode FrmRedirect = _wrapper.GetElement("form", "id", "DefaultFormName");
                if (FrmRedirect != null) strRedirect = "https://erp.kotc.com.kw" + FrmRedirect.Attributes["action"].Value;
               // result = _wrapper.PostURL(out response, strRedirect, strAckPsot, strOInvLink, true, "table", "id", "CurrencyHeader");
                _wrapper.Referrer = strOInvLink;
                result = _wrapper.PostURL(strRedirect, strAckPsot, "table", "id", "CurrencyHeader");
            }
            return result;
        }

        public void DownloadAttachment(string InqNo, string Title, string strOInvLink, List<string> lstProcessedItem)
        {
            bool res = false;
            int i = 0;
            string strFileType = "";
            try
            {
                HtmlNodeCollection _attachRow = _wrapper._CurrentDocument.DocumentNode.SelectNodes("//tr[@id='AttachmentTableRowLayout2']");
                HtmlNodeCollection _attachnodes = _attachRow[0].SelectNodes(".//table[@class='x1o']");//get attachments table
                if (_attachnodes.Count == 1)
                {
                    HtmlNodeCollection _attachRNodes = _attachnodes[0].SelectNodes(".//tr");

                    foreach (HtmlNode _rNodes in _attachRNodes)
                    {
                        string Filename = InqNo + "_" + Title.Replace("/", "_").Trim() + "_" + i + ".pdf";
                        HtmlNodeCollection _rowAttachData = _rNodes.ChildNodes;
                        if (!_rowAttachData[1].InnerText.Contains("Type") && !_rowAttachData[0].InnerText.Contains("Title"))
                        {
                            strFileType = Path.GetExtension(_rowAttachData[0].InnerText.Replace("\"",""));
                            if (!_rowAttachData[0].InnerText.Contains("No results found."))
                            {
                                if (!isValidType(strFileType.ToUpper())) strFileType = "";
                                if (strFileType.ToUpper() == ".PDF" || strFileType == "")
                                {
                                    if (strFileType.ToUpper() == ".PDF") res = true;
                                    Dictionary<string, string> dictState = _wrapper._dctStateData;
                                    if (dictState != null)
                                    {
                                        HtmlAgilityPack.HtmlNode FrmRedirect = _wrapper.GetElement("form", "id", "DefaultFormName");
                                        if (FrmRedirect != null) strRedirect = "https://erp.kotc.com.kw" + FrmRedirect.Attributes["action"].Value;

                                        HtmlAgilityPack.HtmlNode Frmlink = _wrapper.GetElement("a", "id", "AttachmentTable_ATTACH_/oracle/apps/pon/negotiation/inquiry/webui/ponNegSumPG.SubTabRegion.SupplierNegAttachments:AkFileName:" + i);
                                        string _event = "";
                                        if (Frmlink != null)
                                        {
                                            string strlink = Frmlink.Attributes["onclick"].Value;
                                            string[] Arrlink = strlink.Split(',');

                                            foreach (string s in Arrlink)
                                            {
                                                if (s.Contains("_FORMEVENT"))
                                                {
                                                    string[] Arrs = s.Split(':');
                                                    _event = Uri.EscapeDataString(Arrs[1].Trim('\'').Replace("&amp;", "&"));
                                                    break;
                                                }
                                            }
                                        }
                                        #region post data like fiddler(not used,donot delete)
                                        //string strPostData1 = @"_AM_TX_ID_FIELD=" + dictState["_AM_TX_ID_FIELD"] + "&_FORM=" + dictState["_FORM"] + "&ActionListTop=27&_hdfpNoteToBidders=_oa_dis" +
                                        //    "&AttachmentTable_ATTACH_%2Foracle%2Fapps%2Fpon%2Fnegotiation%2Finquiry%2Fwebui%2FponNegSumPG.SubTabRegion.SupplierNegAttachments%3AAkMediaId%3A0=" +
                                        //    dictState["AttachmentTable_ATTACH_/oracle/apps/pon/negotiation/inquiry/webui/ponNegSumPG.SubTabRegion.SupplierNegAttachments:AkMediaId:0"] +
                                        //    "&AttachmentTable_ATTACH_%2Foracle%2Fapps%2Fpon%2Fnegotiation%2Finquiry%2Fwebui%2FponNegSumPG.SubTabRegion.SupplierNegAttachments%3AAkDocumentId%3A0=" +
                                        //    dictState["AttachmentTable_ATTACH_/oracle/apps/pon/negotiation/inquiry/webui/ponNegSumPG.SubTabRegion.SupplierNegAttachments:AkDocumentId:0"] +
                                        //    "&AttachmentTable_ATTACH_%2Foracle%2Fapps%2Fpon%2Fnegotiation%2Finquiry%2Fwebui%2FponNegSumPG.SubTabRegion.SupplierNegAttachments%3AAkDataTypeId%3A0=" +
                                        //    dictState["AttachmentTable_ATTACH_/oracle/apps/pon/negotiation/inquiry/webui/ponNegSumPG.SubTabRegion.SupplierNegAttachments:AkDataTypeId:0"] +
                                        //    "&AttachmentTable_ATTACH_%2Foracle%2Fapps%2Fpon%2Fnegotiation%2Finquiry%2Fwebui%2FponNegSumPG.SubTabRegion.SupplierNegAttachments%3A_pkAttachedDocumentId%3A0=" +
                                        //     dictState["AttachmentTable_ATTACH_/oracle/apps/pon/negotiation/inquiry/webui/ponNegSumPG.SubTabRegion.SupplierNegAttachments:_pkAttachedDocumentId:0"] +
                                        //     "&AttachmentTable_ATTACH_%2Foracle%2Fapps%2Fpon%2Fnegotiation%2Finquiry%2Fwebui%2FponNegSumPG.SubTabRegion.SupplierNegAttachments%3A_pkDocumentId1%3A0=" +
                                        //     dictState["AttachmentTable_ATTACH_/oracle/apps/pon/negotiation/inquiry/webui/ponNegSumPG.SubTabRegion.SupplierNegAttachments:_pkDocumentId1:0"] +
                                        //     "&AttachmentTable_ATTACH_%2Foracle%2Fapps%2Fpon%2Fnegotiation%2Finquiry%2Fwebui%2FponNegSumPG.SubTabRegion.SupplierNegAttachments%3A_hdfpAkCategoryName_ATTACH_%2Foracle%2Fapps%2Fpon%2Fnegotiation%2Finquiry%2Fwebui%2FponNegSumPG.SubTabRegion.SupplierNegAttachments%3A0=" +
                                        //     "_oa_dis&AttachmentTable_ATTACH_%2Foracle%2Fapps%2Fpon%2Fnegotiation%2Finquiry%2Fwebui%2FponNegSumPG.SubTabRegion.SupplierNegAttachments%3A_hdfpUsageTypeColumn%3A0=" +
                                        //     "_oa_dis&AttachmentTable_ATTACH_%2Foracle%2Fapps%2Fpon%2Fnegotiation%2Finquiry%2Fwebui%2FponNegSumPG.SubTabRegion.SupplierNegAttachments%3Alength=1&_hdfpAbstractDescription=" +
                                        //     "_oa_dis&_fwkAbsolutePageName=" + dictState["_fwkAbsolutePageName"] + "&ActionListBottom=27&_OA_CURRENT_SUBTAB_INDEX=" +
                                        //     dictState["_OA_CURRENT_SUBTAB_UINODENAME"] + "&N4%24%24unvalidated=" + dictState["N4$$unvalidated"] + "&N4%24%24serverUnvalidated=" + dictState["N4$$serverUnvalidated"] +
                                        //     "&_fwkActBtnName_ViewRequirementLink_VIEW_REQUIREMENT%24%24serverUnvalidated=" + dictState["_fwkActBtnName_ViewRequirementLink_VIEW_REQUIREMENT$$serverUnvalidated"] +
                                        //     "&N6%24%24unvalidated=" + dictState["N6$$unvalidated"] + "&N6%24%24serverUnvalidated=" + dictState["N6$$serverUnvalidated"] + "&N7%24%24unvalidated=" + dictState["N7$$unvalidated"] +
                                        //     "&N7%24%24serverUnvalidated=" + dictState["N7$$serverUnvalidated"] + "&AttachmentTable_ATTACH_%2Foracle%2Fapps%2Fpon%2Fnegotiation%2Finquiry%2Fwebui%2FponNegSumPG.SubTabRegion.SupplierNegAttachments%24%24unvalidated=" +
                                        //     dictState["AttachmentTable_ATTACH_/oracle/apps/pon/negotiation/inquiry/webui/ponNegSumPG.SubTabRegion.SupplierNegAttachments$$unvalidated"] +
                                        //     "&AttachmentTable_ATTACH_%2Foracle%2Fapps%2Fpon%2Fnegotiation%2Finquiry%2Fwebui%2FponNegSumPG.SubTabRegion.SupplierNegAttachments%24%24serverUnvalidated=" +
                                        //     dictState["AttachmentTable_ATTACH_/oracle/apps/pon/negotiation/inquiry/webui/ponNegSumPG.SubTabRegion.SupplierNegAttachments$$serverUnvalidated"] +
                                        //     "&N97%24%24unvalidated=false&N97%24%24serverUnvalidated=false" +
                                        //     "&_fwkActBtnName_deliverableNameLink_VIEW%24%24serverUnvalidated=" + dictState["_fwkActBtnName_deliverableNameLink_VIEW$$serverUnvalidated"] +
                                        //     "&N199%24%24unvalidated=false&N199%24%24serverUnvalidated=false&_fwkActBtnName_ReturnToLink_RETURNTOLINK%24%24serverUnvalidated=" +
                                        //     dictState["_fwkActBtnName_ReturnToLink_RETURNTOLINK$$serverUnvalidated"] + "&GoBtnBottom%24%24unvalidated=" + dictState["GoBtnBottom$$unvalidated"] + "&GoBtnBottom%24%24serverUnvalidated=" +
                                        //     dictState["GoBtnBottom$$serverUnvalidated"] + "&GoBtnBottom%24%24processFormDataCalled=" + dictState["GoBtnBottom$$processFormDataCalled"] + "&GoBtnTop%24%24unvalidated=" +
                                        //     dictState["GoBtnTop$$unvalidated"] + "&GoBtnTop%24%24serverUnvalidated=" + dictState["GoBtnTop$$serverUnvalidated"] + "&GoBtnTop%24%24processFormDataCalled=" +
                                        //     dictState["GoBtnTop$$processFormDataCalled"] + "&_pkNegSummaryAM.AuctionHeadersAllVOAuctionHeaderId=" + dictState["_pkNegSummaryAM.AuctionHeadersAllVOAuctionHeaderId"] +
                                        //     "&_FORMEVENT=" + _event + "&serverValidate=0&evtSrcRowIdx=&evtSrcRowId=&_FORM_SUBMIT_BUTTON=&event=oaViewAttachment&source=AttachmentTable_ATTACH_%2Foracle%2Fapps%2Fpon%2Fnegotiation%2Finquiry%2Fwebui%2FponNegSumPG.SubTabRegion.SupplierNegAttachments%3AAkFileName%3A" + i + "&_oldStyleSubtabClick=&_OAFSubTabLink=&_OA_SUB_TAB_INDEX=&value=&state=&partialTargets=&partial=&oaDownloadBeanExists=Y&AttachRegKeyNext=ATTACH_%2Foracle%2Fapps%2Fpon%2Fnegotiation%2Finquiry%2Fwebui%2FponNegSumPG.SubTabRegion.SupplierNegAttachments";
                                        #endregion
                                        string strPostData1 = @"_FORM=" + dictState["_FORM"] + "&_FORMEVENT=" + _event +
                                            "&source=AttachmentTable_ATTACH_%2Foracle%2Fapps%2Fpon%2Fnegotiation%2Finquiry%2Fwebui%2FponNegSumPG.SubTabRegion.SupplierNegAttachments%3AAkFileName%3A" + i + "&oaDownloadBeanExists=Y" +
                                            "&AttachRegKeyNext=ATTACH_%2Foracle%2Fapps%2Fpon%2Fnegotiation%2Finquiry%2Fwebui%2FponNegSumPG.SubTabRegion.SupplierNegAttachments";
                                        string fExtension = "";

                                        if (PostDownloadRequest(out response, strRedirect, strPostData1, strOInvLink, true, strRFQPath + "\\" + Filename, out fExtension, res))
                                        {
                                            LogText = "RFQ " + Filename + " downloaded for ref " + Title;
                                            CreateAuditFile(Filename, strProcessorName, Title, "Downloaded", "RFQ " + Filename + ".pdf downloaded for ref " + Title);
                                            lstProcessedItem = GetProcessedItems(eActions.RFQ);
                                            if (!lstProcessedItem.Contains(Title))
                                                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + strUser + "\\RFQ_Downloaded.txt", Title + Environment.NewLine);
                                            //i++;
                                            if (strIncludeSiteData.ToUpper().Trim() == "TRUE")
                                            {
                                                GetSiteData(strOInvLink, Title);
                                            }
                                        }
                                        else
                                        {
                                            if (fExtension == ".PDF")
                                            {
                                                string Audit = "Unable to download RFQ " + Filename + " for ref " + Title;
                                                LogText = Audit;
                                                CreateAuditFile(Filename, strProcessorName, Title, "Error", "LeS-1004:" + Audit);
                                                string eFile = strScreenShotPath + "\\KOTC_RFQ_Error" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".png";
                                                _wrapper.PrintScreen(eFile);
                                            }
                                            else
                                            {
                                                // added on 02-05-2019 to raise error audit ,if pdf not found
                                                List<string> lstWithoutAttachItem = GetWithoutAttachmentItems(eActions.RFQ);
                                                if (!lstWithoutAttachItem.Contains(Title))
                                                {
                                                    File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + strUser + "\\Attachment_NotFound.txt", Title + Environment.NewLine);
                                                    LogText = "Unable to download RFQ " + Filename + " for ref " + Title + " as extension is '" + fExtension + "'";
                                                    string eFile = strScreenShotPath + "\\KOTC_RFQ_Error" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".png";
                                                    _wrapper.PrintScreen(eFile);
                                                    // CreateAuditFile(eFile, strProcessorName, Title, "Error", "Unable to download RFQ " + Filename + " for ref " + Title + " as extension is '" + fExtension + "'");
                                                    CreateAuditFile(eFile, strProcessorName, Title, "Error", "LeS-1004.4:Unable to process file " + Filename + " due to invalid file extension");
                                                }
                                                //
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    // added on 02-05-2019 to raise error audit ,if pdf not found
                                    List<string> lstWithoutAttachItem = GetWithoutAttachmentItems(eActions.RFQ);
                                    if (!lstWithoutAttachItem.Contains(Title))
                                    {
                                        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + strUser + "\\Attachment_NotFound.txt", Title + Environment.NewLine);
                                        LogText = "Unable to download RFQ " + Filename + " for ref " + Title + " as extension is '" + strFileType + "'";
                                        string eFile = strScreenShotPath + "\\KOTC_RFQ_Error" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".png";
                                        _wrapper.PrintScreen(eFile);
                                        //CreateAuditFile(eFile, strProcessorName, Title, "Error", "Unable to download RFQ " + Filename + " for ref " + Title + " as extension is '" + strFileType + "'");
                                        CreateAuditFile(eFile, strProcessorName, Title, "Error", "LeS-1004.4:Unable to process file " + Filename + " due to invalid file extension");
                                    }
                                }
                            }
                            else
                            {
                                // added on 02-05-2019 to raise error audit ,if pdf not found
                                List<string> lstWithoutAttachItem = GetWithoutAttachmentItems(eActions.RFQ);
                                if (!lstWithoutAttachItem.Contains(Title))
                                {
                                    File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + strUser + "\\Attachment_NotFound.txt", Title + Environment.NewLine);
                                    LogText = "No attachment found for Ref " + Title;
                                    string eFile = strScreenShotPath + "\\KOTC_RFQ_Error" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".png";
                                    _wrapper.PrintScreen(eFile);
                                    //CreateAuditFile(eFile, strProcessorName, Title, "Error", "No attahcment found for Ref " + Title);
                                    CreateAuditFile(eFile, strProcessorName, Title, "Error", "LeS-1004:Unable to process file " + Title);
                                }
                            }
                            i++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                string eFile = strScreenShotPath + "\\KOTC_RFQ_Error" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".png";
                _wrapper.PrintScreen(eFile);
                throw;
            }
        }

        public void GetSiteData(string Link, string InqNo)
        {
            bool isResult = false;
                HtmlAgilityPack.HtmlDocument _doc = null;
                if (_doc == null)
                {
                    _doc = new HtmlAgilityPack.HtmlDocument();
                    if (_wrapper._CurrentResponseString != "") _doc.LoadHtml(_wrapper._CurrentResponseString);
                }
                HtmlNode _node = _doc.DocumentNode.SelectSingleNode("//form[@id='DefaultFormName']");
                if (_node != null)
                {
                    string _ItemLink = _node.Attributes["action"].Value;
                    if (_ItemLink != "")
                    {
                        #region getting post data to navigate to Lines tab
                        _wrapper._dctStateData = getStateInfo_ByName(_wrapper._CurrentResponseString, _wrapper._CurrentDocument);
                        string _actionListTop = "", _actionListBottom = "";
                        var _options = _doc.DocumentNode.SelectSingleNode("//select[@id='ActionListTop']//option[@selected]");
                        if (_options != null)
                        {
                            _actionListTop = _options.GetAttributeValue("value", "");
                        }

                        _options = _doc.DocumentNode.SelectSingleNode("//select[@id='ActionListBottom']//option[@selected]");
                        if (_options != null)
                        {
                            _actionListBottom = _options.GetAttributeValue("value", "");
                        }

                        HtmlNode _line = _wrapper.GetElement("a", "title", "Lines");
                        Dictionary<string, string> _dic = new Dictionary<string, string>();
                        string[] arr = null;
                        if (_line != null)
                        {
                            string _click = _line.GetAttributeValue("onclick", "").Replace("submitForm('DefaultFormName',1,{", "").Replace("});return false;", "").Replace("\'", "");
                            arr = _click.Split(',');
                            foreach (string a in arr)
                            {
                                if (!_dic.ContainsKey(a.Split(':')[0].Trim()))
                                    _dic.Add(a.Split(':')[0].Trim(), a.Split(':')[1].Trim());
                            }
                        }
                        string serverValidate = "", oldStyleSubtabClick = "", OAFSubTabLink = "", OASubTabIndex = "", _event = "", source = "";

                        string _postData = "";
                        foreach (KeyValuePair<string, string> k in _wrapper._dctStateData)
                        {
                            _postData += k.Key + "=" + k.Value + "&";
                        }


                        if (_dic.ContainsKey("serverValidate")) serverValidate = _dic["serverValidate"];
                        if (_dic.ContainsKey("_oldStyleSubtabClick")) oldStyleSubtabClick = _dic["_oldStyleSubtabClick"];
                        if (_dic.ContainsKey("_OAFSubTabLink")) OAFSubTabLink = _dic["_OAFSubTabLink"];
                        if (_dic.ContainsKey("_OA_SUB_TAB_INDEX")) OASubTabIndex = _dic["_OA_SUB_TAB_INDEX"];
                        if (_dic.ContainsKey("event")) _event = _dic["event"];
                        if (_dic.ContainsKey("source")) source = _dic["source"];
                        #endregion

                        #region item paging
                        bool isLastPage = false;
                        int iPageCnt = 1, ProPages = 200;
                        if (ProPages <= 0)
                        {
                            ProPages = 1;
                        }

                        Dictionary<string, LineItem> slHref = new Dictionary<string, LineItem>();
                        UpdateSessionCookies("https://erp.kotc.com.kw" + _ItemLink);
                        _postData += "serverValidate=" + serverValidate + "&evtSrcRowIdx=&evtSrcRowId=&_FORM_SUBMIT_BUTTON=&event=" + _event + "&source=" + source + "&_oldStyleSubtabClick=" + oldStyleSubtabClick +
                          "&_OAFSubTabLink=" + OAFSubTabLink + "&_OA_SUB_TAB_INDEX=" + OASubTabIndex + "&value=&state=&partialTargets=&partial=&oaDownloadBeanExists=&AttachRegKeyNext=&ActionListTop=" + _actionListTop + "&ActionListBottom=" + _actionListBottom + "&_FORMEVENT=";
                        if (_wrapper.PostURL("https://erp.kotc.com.kw" + _ItemLink, _postData, "table", "summary", "Lines Table"))//navigate to item tab
                        {
                            while ((!isLastPage) && iPageCnt <= ProPages)
                            {
                                HtmlNode _table = _wrapper.GetElement("table", "summary", "Lines Table");
                                if (_table != null)
                                {
                                    HtmlNodeCollection _nodes = _table.SelectNodes("./tr");
                                    if (_nodes.Count > 1)
                                    {
                                        string strFilename = strScreenShotPath + "\\KotcRFQItems_" + InqNo.Replace("/", "_") + "_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + strSupplier + ".png";
                                        _wrapper.PrintScreen(strFilename);

                                        foreach (HtmlNode _row in _nodes)
                                        {
                                            HtmlNodeCollection _rowData = _row.ChildNodes;
                                            if (!_rowData[3].InnerText.Trim().Contains("Unit"))
                                            {
                                                string _href = "";
                                                LineItem _item = new LineItem();
                                                if (_rowData[0].ChildNodes[0].SelectSingleNode("./a").Id != "")
                                                {
                                                    _href = _rowData[0].ChildNodes[0].SelectSingleNode("./a").GetAttributeValue("href", "").Trim();
                                                }
                                                _item.OriginatingSystemRef = _rowData[0].ChildNodes[0].SelectSingleNode("./span").InnerText;
                                                _item.Number = _rowData[0].ChildNodes[0].SelectSingleNode("./span").InnerText;
                                                _item.SYS_ITEMNO = Convert.ToInt32(_rowData[0].ChildNodes[0].SelectSingleNode("./span").InnerText);
                                                if (!slHref.ContainsKey(_href))
                                                    slHref.Add(_href, _item);//get each item links
                                            }
                                        }
                                    }
                                    else throw new Exception("No item found in line item table.");
                                }
                                else throw new Exception("Line Item table is null.");

                                if (_wrapper._CurrentResponseString.Contains("Next functionality disabled")) isLastPage = true;
                                else if (!_wrapper._CurrentResponseString.Contains("Next")) isLastPage = true;
                                else
                                {
                                    int nextRfqRows = 25 * (iPageCnt) + 1;
                                    _wrapper._dctStateData = getStateInfo_ByName_AllValue(_wrapper._CurrentResponseString, _wrapper._CurrentDocument);
                                    _postData = "";
                                    List<string> _noeInclude = new List<string>() { "event", "source", "value", "size", "partialTargets", "partial" };
                                    foreach (KeyValuePair<string, string> k in _wrapper._dctStateData)
                                    {
                                        if (_noeInclude.IndexOf(k.Key) == -1)
                                            _postData += k.Key + "=" + k.Value + "&";
                                    }


                                    _actionListTop = ""; _actionListBottom = "";
                                    _options = _doc.DocumentNode.SelectSingleNode("//select[@id='ActionListTop']//option[@selected]");
                                    if (_options != null)
                                    {
                                        _actionListTop = _options.GetAttributeValue("value", "");
                                    }

                                    _options = _doc.DocumentNode.SelectSingleNode("//select[@id='ActionListBottom']//option[@selected]");
                                    if (_options != null)
                                    {
                                        _actionListBottom = _options.GetAttributeValue("value", "");
                                    }

                                    string _source = "";
                                    HtmlNode _a = _wrapper.GetElement("a", "class", "x48");
                                    if (_a != null)
                                    {
                                        string _click = _a.GetAttributeValue("onclick", "").Replace("_navBarSubmit('DefaultFormName',", "").Replace(");return false", "").Replace("\'", "");
                                        arr = _click.Split(',');
                                        if (arr.Length == 6)
                                            _source = arr[1];
                                   }

                                    _postData += "ActionListTop=" + _actionListTop + "&ActionListBottom=" + _actionListBottom + "&_FORMEVENT=&serverValidate=&evtSrcRowIdx=&evtSrcRowId=&_FORM_SUBMIT_BUTTON=&event=goto&source=" + _source + "&_oldStyleSubtabClick=&_OAFSubTabLink=&_OA_SUB_TAB_INDEX=&value=" + nextRfqRows + "&size=25&partialTargets=SubTabRegion.ItemsTable&partial=true&state=";
                                    if (!_postData.Contains("_AM_TX_ID_FIELD=3"))
                                    {
                                        _postData += "&_AM_TX_ID_FIELD=3&_FORM=DefaultFormName";
                                    }

                                    HtmlNode FrmRedirect = _wrapper.GetElement("form", "id", "DefaultFormName");
                                    if (FrmRedirect != null) strRedirect = "https://erp.kotc.com.kw" + FrmRedirect.Attributes["action"].Value;
                                    UpdateSessionCookies(strRedirect);

                                    int _pagecount = 1;
                                paging: if (_wrapper.PostURL(strRedirect, _postData, "", "", ""))
                                    {
                                        string paging = "";
                                        if (_wrapper._CurrentResponseString != "") _doc.LoadHtml(_wrapper._CurrentResponseString);
                                        _options = _doc.DocumentNode.SelectSingleNode("//select[@title='Select record set']//option[@selected]");
                                        if (_options != null)
                                        {
                                            paging = _options.GetAttributeValue("value", "").Trim().Split(',')[0];
                                        }
                                   
                                        if (paging != "")
                                        {
                                            if (Convert.ToInt32(paging) == nextRfqRows)
                                            { iPageCnt++; LogText = "Navigate to item page " + iPageCnt; }
                                            else { _pagecount++; if (_pagecount < 5) goto paging; else { throw new Exception("Unable to load page " + (iPageCnt - 1) + " for ref no " + InqNo); isResult = false; } }
                                        }

                                    }
                                }
                            }
                        }
                        #endregion
                        if (slHref.Count > 0)
                        {
                            int i = 1;
                            _workbook = new Workbook(TemplateFile);
                            _sheet = _workbook.Worksheets[0];
                            Aspose.Cells.Cells cells = _sheet.Cells;
                            Aspose.Cells.Cell endCell = cells.EndCellInColumn(1);
                            int maxrow = endCell.Row;
                            try
                            {
                                foreach (KeyValuePair<string, LineItem> _l in slHref)//navigate to each item detail page
                                {

                                    i++;
                                    UpdateSessionCookies("https://erp.kotc.com.kw" + _l.Key);
                                    if (_wrapper.LoadURL("https://erp.kotc.com.kw" + _l.Key, "table", "id", "ItemDetailsHeaderLeft", "JSESSIONID"))
                                    {
                                        HtmlNode _table = _wrapper.GetElement("table", "id", "ItemDetailsHeaderLeft").ChildNodes[0].ChildNodes[1].SelectSingleNode("./table");
                                        if (_table != null)
                                        {

                                            _sheet.Cells[i - 1, 0].Value = Convert.ToInt32(i - 1);
                                            _sheet.Cells[i - 1, 1].Value = i - 1;
                                            _sheet.Cells[i - 1, 2].Value = _wrapper.GetElement("span", "id", "ItemNumber").InnerText.Trim();
                                            _sheet.Cells[i - 1, 3].Value = _wrapper.GetElement("span", "id", "ItemDescription").InnerText.Trim();

                                            HtmlNode _breadcumb = _doc.DocumentNode.SelectSingleNode("//div[@class='xl']");
                                            if (_breadcumb != null)
                                            {
                                                string _link = _breadcumb.ChildNodes[_breadcumb.ChildNodes.Count - 2].ChildNodes[0].GetAttributeValue("href", "").Trim();
                                                UpdateSessionCookies("https://erp.kotc.com.kw" + _link);
                                                _wrapper.LoadURL("https://erp.kotc.com.kw" + _link, "table", "summary", "Attachments of associated record", "JSESSIONID");
                                            }
                                            // _workbook.Save(strSiteFile + "\\RFQ" + InqNo.Replace("/", "_") + ".xlsx");
                                        }
                                    }
                                }
                                _workbook.Save(strSiteFile + "\\RFQ" + InqNo.Replace("/", "_") + ".xlsx");
                            }
                            catch (Exception ex)
                            { throw new Exception("Unable to download site data file for ref no. " + InqNo); }
                        }

                        #region item paging
                        //  Dictionary<string, LineItem> slHref = new Dictionary<string, LineItem>();
                        //UpdateSessionCookies("https://erp.kotc.com.kw" + _ItemLink);
                        //_postData += "serverValidate=" + serverValidate + "&evtSrcRowIdx=&evtSrcRowId=&_FORM_SUBMIT_BUTTON=&event=" + _event + "&source=" + source + "&_oldStyleSubtabClick=" + oldStyleSubtabClick +
                        //  "&_OAFSubTabLink=" + OAFSubTabLink + "&_OA_SUB_TAB_INDEX=" + OASubTabIndex + "&value=&state=&partialTargets=&partial=&oaDownloadBeanExists=&AttachRegKeyNext=&ActionListTop=" + _actionListTop + "&ActionListBottom=" + _actionListBottom + "&_FORMEVENT=";
                        //if (_wrapper.PostURL("https://erp.kotc.com.kw" + _ItemLink, _postData, "table", "summary", "Lines Table"))//navigate to item tab
                        //{
                        //        UpdateSessionCookies("https://erp.kotc.com.kw" + _l.Key);
                        //        if (_wrapper.LoadURL("https://erp.kotc.com.kw" + _l.Key, "table", "id", "ItemDetailsHeaderLeft", "JSESSIONID"))
                        //        {
                        //            HtmlNode _table = _wrapper.GetElement("table", "id", "ItemDetailsHeaderLeft").ChildNodes[0].ChildNodes[1].SelectSingleNode("./table");
                        //            if (_table != null)
                        //            {
                        //                Aspose.Cells.Cells cells = _sheet.Cells;
                        //                Aspose.Cells.Cell endCell = cells.EndCellInColumn(1);
                        //                int maxrow = endCell.Row;
                        //                for (int i = 2; i <= maxrow; i++)
                        //                {

                        //                    _sheet.Cells['A', i].Value = i - 1;
                        //                    _sheet.Cells['B', i].Value = i - 1;
                        //                    _sheet.Cells['c', i].Value = _wrapper.GetElement("span", "id", "ItemNumber").InnerText.Trim();
                        //                    _sheet.Cells['d', i].Value = _wrapper.GetElement("span", "id", "ItemDescription").InnerText.Trim();

                        //                    HtmlNode _breadcumb = _doc.DocumentNode.SelectSingleNode("//div[@class='xl']");
                        //                    if (_breadcumb != null)
                        //                    {
                        //                        string _link = _breadcumb.ChildNodes[_breadcumb.ChildNodes.Count - 2].ChildNodes[0].GetAttributeValue("href", "").Trim();
                        //                        UpdateSessionCookies("https://erp.kotc.com.kw" + _link);
                        //                        _wrapper.LoadURL("https://erp.kotc.com.kw" + _link, "table", "summary", "Attachments of associated record", "JSESSIONID");
                        //                    }
                        //                }
                        //            }
                        //        }
                        //}
                        #endregion

                        
                    }
                }
           // }
        }

        private Dictionary<string, string> getStateInfo_ByName_AllValue(string _CurrentResponseString, HtmlAgilityPack.HtmlDocument _doc = null)
        {
            Dictionary<string, string> dctState = new Dictionary<string, string>();
            if (_doc == null)
            {
                _doc = new HtmlAgilityPack.HtmlDocument();
                if (_CurrentResponseString != "") _doc.LoadHtml(_CurrentResponseString);
            }

            HtmlNodeCollection _nodeCollection = _doc.DocumentNode.SelectNodes("//input[@type='hidden']");
            string sKey = "", sValue = "";
            if (_nodeCollection != null)
            {
                foreach (HtmlNode _hiddenNode in _nodeCollection)
                {
                    if (_hiddenNode.Attributes["name"] != null)
                    {
                        if (_hiddenNode.Attributes.Contains("value"))
                            sValue = (HttpUtility.UrlEncode(HttpUtility.HtmlDecode(_hiddenNode.Attributes["value"].Value))).Replace("(", "%28").Replace(")", "%29");
                        else
                            sValue = "";
                        sKey = Uri.EscapeDataString(_hiddenNode.Attributes["name"].Value);


                        if (sKey != "" && !dctState.ContainsKey(sKey)) dctState.Add(sKey, sValue);
                    }
                }
            }
            return dctState;
        }

        private Dictionary<string, string> getStateInfo_ByName(string _CurrentResponseString, HtmlAgilityPack.HtmlDocument _doc = null)
        {
            Dictionary<string, string> dctState = new Dictionary<string, string>();
            if (_doc == null)
            {
                _doc = new HtmlAgilityPack.HtmlDocument();
                if (_CurrentResponseString != "") _doc.LoadHtml(_CurrentResponseString);
            }

            HtmlNodeCollection _nodeCollection = _doc.DocumentNode.SelectNodes("//input[@type='hidden']");
            string sKey = "", sValue = "";
            if (_nodeCollection != null)
            {
                foreach (HtmlNode _hiddenNode in _nodeCollection)
                {
                    if ((_hiddenNode.Attributes["value"] != null) && (_hiddenNode.Attributes["name"] != null))
                    {

                        sValue = Uri.EscapeDataString(_hiddenNode.Attributes["value"].Value);
                        sKey = Uri.EscapeDataString(_hiddenNode.Attributes["name"].Value);


                        if (sKey != "" && !dctState.ContainsKey(sKey)) dctState.Add(sKey, sValue);
                    }
                }
            }
            return dctState;
        }

        public bool isValidType(string FileType)
        {
            string[] strArrFileType = ConfigurationManager.AppSettings["VALID_FILETYPES"].Trim().Split('|');
            if (strArrFileType.Contains(FileType)) return true;
            else return false;
        }

        public bool PostDownloadRequest(out HttpWebResponse response, string url, string strPostData, string Referer, bool useCookieContainer, string FileName, out string fExtension, bool res)
        {
            bool result = false;
            response = null;
            fExtension = "";
            byte[] b = null;
            try
            {

                _wrapper.PostURL(url, strPostData,"","","","",false);
                response = _wrapper._CurrentResponse;
                if (_wrapper._CurrentResponse.ContentType.ToUpper() == "APPLICATION/PDF" || res)//||fExtension == ""
                {
                    fExtension = ".PDF";
                    FileStream fileStream = File.OpenWrite(FileName);
                    byte[] buffer = new byte[1024];
                    using (Stream input = _wrapper._CurrentResponse.GetResponseStream())
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            byte[] buf = new byte[1024];
                            count = input.Read(buf, 0, 1024);
                            ms.Write(buf, 0, count);
                        } while (input.CanRead && count > 0);
                        b = ms.ToArray();
                    }
                    fileStream.Write(b, 0, b.Length);
                    fileStream.Flush();
                    fileStream.Close();

                    result = true;
                }
                else result = false;
                return result;
                //if (_wrapper._CurrentResponseString.Contains("OADownload.jsp"))
                //{
                //    HtmlNode _tag = _wrapper._CurrentDocument.DocumentNode.SelectSingleNode("a[contains(@href,'OADownload.jsp']");
                //    if (_tag != null)
                //    {
                //        url = _tag.Attributes["href"].Value.ToString();




                //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //        if (useCookieContainer)
                //        {
                //            if (_wrapper.SessionID == null)
                //                request.CookieContainer = _wrapper._CookieContainer;
                //        }
                //        else request.CookieContainer = _wrapper._CookieContainer;
                //        request.KeepAlive = true;
                //        request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0");
                //        request.Headers.Add("Origin", @"https://erp.kotc.com.kw");
                //        request.Headers.Add("Upgrade-Insecure-Requests", @"1");
                //        request.ContentType = "application/x-www-form-urlencoded";
                //        request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
                //        request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                //        request.Referer = Referer;
                //        request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                //        request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");

                //        request.Headers.Set(HttpRequestHeader.Cookie, @"JSESSIONID=" + _wrapper.SessionID + "; PROD=" + _wrapper._dctSetCookie["PROD"] + "; oracle.uix=0^^GMT+5:30^p");

                //        //Set request method
                //        request.Method = "POST";
                //        request.ServicePoint.Expect100Continue = false;

                //        //Set request body.
                //        string body = strPostData;
                //        byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                //        request.ContentLength = postBytes.Length;
                //        Stream stream = request.GetRequestStream();
                //        stream.Write(postBytes, 0, postBytes.Length);
                //        stream.Close();

                //        if (!res)
                //        {
                //            if (_wrapper._CurrentResponse.Headers["Content-Disposition"] != null)
                //            {
                //                var fn = response.Headers["Content-Disposition"].Split(new string[] { "=" }, StringSplitOptions.None)[1];
                //                fExtension = Path.GetExtension(fn).ToUpper();
                //            }
                //        }
                //        _wrapper._CurrentResponse = response;
                //        _wrapper._CurrentResponseString = _wrapper.ReadResponse(response);
                //        if (fExtension == ".PDF" || res)//||fExtension == ""
                //        {
                //            FileStream fileStream = File.OpenWrite(FileName);
                //            byte[] buffer = new byte[1024];
                //            using (Stream input = _wrapper._CurrentResponse.GetResponseStream())
                //            using (MemoryStream ms = new MemoryStream())
                //            {
                //                int count = 0;
                //                do
                //                {
                //                    byte[] buf = new byte[1024];
                //                    count = input.Read(buf, 0, 1024);
                //                    ms.Write(buf, 0, count);
                //                } while (input.CanRead && count > 0);
                //                b = ms.ToArray();
                //            }
                //            fileStream.Write(b, 0, b.Length);
                //            fileStream.Flush();
                //            fileStream.Close();

                //            result = true;
                //        }
                //        else result = false;
                //    }
                //}
            }
            catch (WebException e)
            {
                //ProtocolError indicates a valid HTTP response, but with a non-200 status code (e.g. 304 Not Modified, 404 Not Found)
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                else result = false;
            }
            catch (Exception ex)
            {
                LogText = "Exception while downloading file : " + ex.GetBaseException().ToString();
                if (response != null) response.Close();
                result = false;
            }
            return result;
        }

        public List<string> GetProcessedItems(eActions eAction)
        {
            string strDoneFile = "";
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + strUser)) Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + strUser);
            List<string> lstProcessedItems = new List<string>();
            switch (eAction)
            {
                case eActions.RFQ: strDoneFile = AppDomain.CurrentDomain.BaseDirectory + strUser + "\\RFQ_Downloaded.txt"; break;
                default: break;
            }
            if (File.Exists(strDoneFile))
            {
                string[] _Items = File.ReadAllLines(strDoneFile);
                lstProcessedItems.AddRange(_Items.ToList());
            }
            return lstProcessedItems;
        }

        public List<string> GetWithoutAttachmentItems(eActions eAction)
        {
            string strDoneFile = "";
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + strUser)) Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + strUser);
            List<string> lstWithoutAttachmentItems = new List<string>();
            switch (eAction)
            {
                case eActions.RFQ: strDoneFile = AppDomain.CurrentDomain.BaseDirectory + strUser + "\\Attachment_NotFound.txt"; break;
                default: break;
            }
            if (File.Exists(strDoneFile))
            {
                string[] _Items = File.ReadAllLines(strDoneFile);
                lstWithoutAttachmentItems.AddRange(_Items.ToList());
            }
            return lstWithoutAttachmentItems;
        }

        public string GetScheduleCounter()
        {
            string strDoneFile = "",data="";
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + strUser)) Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + strUser);
             strDoneFile = AppDomain.CurrentDomain.BaseDirectory + strUser + "\\Schedule_ErrorLog.txt";
            if (File.Exists(strDoneFile))
            {
                string[] _Items = File.ReadAllLines(strDoneFile);
                if (_Items.Length>0)
                    data = _Items[0];
                else data = "";
            }
            return data;
        }

        public bool LogOut()
        {
            bool _result = false;
            try
            {
                //if (_wrapper.LoadURL(out response, "https://erp.kotc.com.kw/OA_HTML/AppsLogout", "Login"))//, strSiteURL//old
              //  if (_wrapper.LoadURL( "https://erp.kotc.com.kw/OA_HTML/AppsLogout", "Login"))//, strSiteURL//chrome//2-4-2018
                //if (_wrapper.LoadURL("https://erp.kotc.com.kw/OA_HTML/AppsLocalLogin.jsp?langCode=US&_logoutRedirect=y","","","","JSESSIONID"))
                if (_wrapper.LoadURL("https://erp.kotc.com.kw/OA_HTML/OALogout.jsp?menu=Y", "", "", "", "JSESSIONID"))
                {
                    UpdateSessionCookies("https://erp.kotc.com.kw/OA_HTML/OALogout.jsp?menu=Y");
                    _result = true;
                    HtmlNode _submitbtn = _wrapper.GetElement("button", "id", "SubmitButton");
                    if (_submitbtn != null)
                    {
                        LogText = "Redirected to login page after session logout.";
                        loginpageloaded = true;
                    }
                    else
                    {
                        LogText = "Page not redirected to login page after logout.";
                        LogText = "Loading https://erp.kotc.com.kw/OA_HTML/AppsLocalLogin.jsp?langCode=US&_logoutRedirect=y";
                        if (_wrapper.LoadURL("https://erp.kotc.com.kw/OA_HTML/AppsLocalLogin.jsp?langCode=US&_logoutRedirect=y", "button", "id", "SubmitButton", "JSESSIONID", "id", true))
                        {
                            UpdateSessionCookies("https://erp.kotc.com.kw/OA_HTML/AppsLocalLogin.jsp?langCode=US&_logoutRedirect=y");
                            LogText = "Redirected to login page after session logout.";
                            loginpageloaded = true;
                        }
                    }
                    
                    

                }
            }
            catch (Exception e)
            {
                //
            }
            return _result;
        }

        public void CreateAuditFile(string strFileName, string strModule, string strRefNo, string strLogType, string strAudit)
        {
            try
            {
                if (!Directory.Exists(strAuditPath)) Directory.CreateDirectory(strAuditPath);

                string strAuditData = "";
                if (strAuditData.Trim().Length > 0) strAuditData += Environment.NewLine;
                strAuditData += strBuyer + "|";
                strAuditData += strSupplier + "|";
                strAuditData += strModule + "|";
                strAuditData += Path.GetFileName(strFileName) + "|";
                strAuditData += strRefNo + "|";
                strAuditData += strLogType + "|";
                strAuditData += DateTime.Now.ToString("yy-MM-dd HH:mm") + " : " + strAudit;
                if (strAuditData.Trim().Length > 0)
                {
                    File.WriteAllText(strAuditPath + "\\Audit_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt", strAuditData);
                    System.Threading.Thread.Sleep(5);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void MoveRFQToError(string strMessage, string strRFQNo, HtmlAgilityPack.HtmlDocument htmlDoc)
        {
            CreateAuditFile("", strProcessorName, strRFQNo, "Error", strMessage);
            string strFile = strScreenShotPath + "\\Kotc_RFQ_Error" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
            _wrapper.PrintScreen( strFile);
        }

        public bool PrintScreen(string sFileName)
        {
            try
            {
                bool _result = false;
                _wrapper.PrintScreen(sFileName);
                _result = File.Exists(sFileName);
                return _result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Download_Site()
        {
            try {
            
            }
            catch (Exception ex)
            { 
            
            }
        }
    }

    public enum eActions
    {
        RFQ = 0,
        PO = 1,
    }
}
