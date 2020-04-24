using Http_Navibulgar_Routine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Http_Navibulgar_Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            Http_Download_Routine _wrapper = new Http_Download_Routine();
            if (LeSSysInfo.LeSMain.GetLeSSysInfo() == LeSSysInfo.LeSReturn.Success_1)
            {
                _wrapper.LogText = "================" + DateTime.Now.ToString() + "================";
                _wrapper.LogText = "Navibulgar HTTP Processor started";
                Console.Title = "Navibulgar HTTP Processor";
                _wrapper.Initialise();
                string AppsettingsConfig = AppDomain.CurrentDomain.BaseDirectory + "Appsettings.xml";
                ReadConfig(AppsettingsConfig, _wrapper);
                _wrapper.LogText = "Navibulgar HTTP Processor stopped";
                _wrapper.LogText = "====================================================";
            }
            else
            {
                _wrapper.LogText = "Unauthorized access please contact LeS Support";
            }
        }

        public static void ReadConfig(string sConfigFile, Http_Download_Routine _wrapper)
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
                //string sAuditMesage = "Exception while processing RFQ download " + e.GetBaseException().ToString();
                string sAuditMesage = "Unable to process file due to " + e.GetBaseException().ToString();
                _wrapper.LogText = sAuditMesage;
                _wrapper.CreateAuditFile("", "Navibulgar_HTTP_Processor", "", "Error", "LeS-1004:"+sAuditMesage, _wrapper.BuyerCode, _wrapper.SupplierCode, _wrapper.AuditPath);
            }
        }

        public static bool ProcessUser(Http_Download_Routine _wrapper)
        {
            bool _result = true;
            _wrapper.LoadAppsettings();
            _wrapper.LogText = "Domain: " + _wrapper.Domain + Environment.NewLine;
            _wrapper.LogText = "Process started for " + _wrapper.Userid;
            if (_wrapper.DoLogin("input", "id", "MainContent_txtFrom"))
            {
                foreach (string sAction in _wrapper.Actions)
                {
                    try
                    {
                        switch (sAction.ToUpper())
                        {
                            case "RFQ": _wrapper.ProcessRFQ(); break;
                            case "QUOTE": _wrapper.ProcessQuote(); break;
                        }
                    }
                    catch (Exception ex)
                    {
                        _wrapper.LogText = "Exception while processing user actions " + sAction.ToUpper() + " : " + ex.GetBaseException().ToString();
                    }
                }
            }
            return _result;
        }
    }
}
