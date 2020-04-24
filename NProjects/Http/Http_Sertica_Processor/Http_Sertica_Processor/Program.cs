using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Http_Sertica_Routine;
using System.Xml;

namespace Http_Sertica_Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            Http_Download_Routine _wrapper = new Http_Download_Routine();
            _wrapper.LogText = "================" + DateTime.Now.ToString() + "================";
            _wrapper.LogText = "Sertica RFQ Download HTTP processor started";
            Console.Title = "Sertica RFQ,PO Download HTTP processor";
            _wrapper.Initialise();
            string AppsettingsConfig = AppDomain.CurrentDomain.BaseDirectory + "Appsettings.xml";
            ReadConfig(AppsettingsConfig, _wrapper);
            _wrapper.LogText = "Sertica RFQ Download HTTP processor stopped";
            _wrapper.LogText = "====================================================";
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
                      //  ProcessUser(_wrapper);
                    }

                }

            }
            catch (Exception e)
            {
                ////"Exception while processing RFQ download " + e.GetBaseException().ToString();
                string sAuditMesage = "Unable to process file due to "+ e.GetBaseException().ToString();
                _wrapper.LogText = sAuditMesage;
                string filename = _wrapper.ImgPath + "\\Sertica_Exception_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                if (!_wrapper.PrintScreen(filename)) filename = "";
                _wrapper.CreateAuditFile(filename, "Sertica_HTTP_Downloader", "", "Error", "LeS-1004:" + sAuditMesage, _wrapper.BuyerCode, _wrapper.SupplierCode, _wrapper.AuditPath);
            }
        }

        public static bool ProcessUser(Http_Download_Routine _wrapper)
        {
            bool _result = true;
            _wrapper.LoadAppsettings();
            _wrapper.LogText = "Domain: " + _wrapper.Domain + Environment.NewLine;
            _wrapper.LogText = "Process started for " + _wrapper.Userid;
            if (_wrapper.DoLogin("table", "id", "MainPage1_QuotationListView1_DataList1"))
            {
                foreach (string sAction in _wrapper.Actions)
                {
                    try
                    {
                        switch (sAction.ToUpper())
                        {
                            case "RFQ": _wrapper.ProcessRFQ(); break;
                            case "PO": _wrapper.ProcessPO(); break;
                        }
                    }
                    catch (Exception ex)
                    {
                        _wrapper.LogText = "Exception while processing " + sAction.ToUpper() + " : " + ex.GetBaseException().ToString();
                    }
                }
                if (_wrapper.SiteLogout())
                {
                    _wrapper.LogText = "User " + _wrapper.Userid + " logged out successfully." + Environment.NewLine;
                    _result = true;
                }
                else
                {
                    //string sAudit = "Unable to log out User " + _wrapper.Userid + ". Multi login process stopped.";
                    string sAudit = "Unable to logout User " + _wrapper.Userid + ", Multi login process stopped.";
                    _wrapper.LogText = sAudit;                  
                    string filename = _wrapper.ImgPath + "\\Sertica_LoginOutFailed_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                    if (!_wrapper.PrintScreen(filename)) filename = "";

                    _wrapper.CreateAuditFile(filename, "Sertica_HTTP_Downloader", "", "Error", "LeS-1015.1:"+sAudit, _wrapper.BuyerCode, _wrapper.SupplierCode, _wrapper.AuditPath);
                    _result = false;
                }
            }
            return _result;
        }
    }
}
