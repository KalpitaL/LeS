using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bernhard_Schulte_Http_Download_Routine;
using System.Xml;

namespace BernhadSchulte_Http_Download_Processor
{
    class Program
    {
        static void Main(string[] args)
        {            
            Http_Download_Routine _wrapper = new Http_Download_Routine();
            _wrapper.LogText = "================" + DateTime.Now.ToString() + "================";
            _wrapper.LogText = "Bernhard Schulte RFQ Download HTTP processor started";
            _wrapper.Initialise();
            string AppsettingsConfig = AppDomain.CurrentDomain.BaseDirectory+"Appsettings.xml";
            ReadConfig(AppsettingsConfig, _wrapper);          
            _wrapper.LogText = "Bernhard Schulte RFQ Download HTTP processor stopped";
            _wrapper.LogText = "====================================================";
            
        }
        public static void ReadConfig(string sConfigFile,Http_Download_Routine _wrapper)
        {
            XmlDocument configFile = new XmlDocument();
            configFile.Load(sConfigFile);
            XmlElement userSetting;
            XmlNodeList xmlAppSettings = configFile.SelectNodes("//APPSETTINGS");
            try
            {
                if (xmlAppSettings != null)
                {
                    foreach (XmlNode _appSetting in xmlAppSettings)
                    {
                        _wrapper.dctAppSettings.Clear();
                        XmlNodeList _childAppsettings = _appSetting.ChildNodes;
                        foreach (XmlNode _UserSetting in _childAppsettings)
                        {
                            userSetting = (XmlElement)_UserSetting;
                            _wrapper.dctAppSettings.Add(userSetting.Name, userSetting.InnerText);
                        }
                        if (!ProcessUser(_wrapper)) break;
                    }
                }
            }
            catch (Exception e)
            {
                //string sAuditMesage = "Exception while processing RFQ download" + e.GetBaseException().ToString();
                string sAuditMesage = "Parameters not configured" + e.GetBaseException().ToString();//added by Kalpita on 18/10/2019
                _wrapper.LogText = sAuditMesage;
                string filename = _wrapper.PrintScreenPath + "\\BernhardSchulte_Exception_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                if (!_wrapper.PrintScreen(filename)) filename = "";
                _wrapper.CreateAuditFile(filename, "BernhardSchulte_HTTP_Downloader", "", "Error", "LeS-1004:"+sAuditMesage, _wrapper.buyer_Link_code, _wrapper.SupplierCode,_wrapper.AuditPath);
            }
        }

        public static bool ProcessUser(Http_Download_Routine _wrapper)
        {
            bool _result = true;
            _wrapper.LoadAppsettings();
            _wrapper.LogText = "Process started for " + _wrapper.Userid;
            if (_wrapper.DoLogin("div", "id", "supplierName"))
            {
                foreach (string sAction in _wrapper.Actions)
                {
                    try
                    {
                        switch (sAction.ToUpper())
                        {
                            case "RFQ":_wrapper.ProcessRFQ();// testing 19/11/2019
                                break;
                            case "QUOTE": _wrapper.ProcessQuote(); break;//added by Kalpita on 18/11/2019
                        }
                    }
                    catch (Exception ex)
                    {
                        _wrapper.LogText = "Exception while processing " + sAction.ToUpper() + " : " + ex.GetBaseException().ToString();
                    }
                }
                if (_wrapper.LogOut())
                {
                    _wrapper.LogText = "User " + _wrapper.Userid + " logged out successfully.";
                    _result = true;
                }
                else
                {
                    // string sAudit = "Unable to log out User " + _wrapper.Userid + ". Multi login process stopped.";
                    string sAudit = "Unable to Logout" + _wrapper.Userid;//added by Kalpita on 18/10/2019
                    _wrapper.LogText = sAudit;

                    string filename = _wrapper.PrintScreenPath + "\\BernhardSchulte_LoginOutFailed_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                    if (!_wrapper.PrintScreen(filename)) filename = "";
                    _wrapper.CreateAuditFile(filename, "BernhardSchulte_HTTP_Downloader", "", "Error", "LeS-1015:"+sAudit, _wrapper.buyer_Link_code, _wrapper.SupplierCode, _wrapper.AuditPath);
                    _result = false;
                }
            }
            return _result;
        }

    }
}
