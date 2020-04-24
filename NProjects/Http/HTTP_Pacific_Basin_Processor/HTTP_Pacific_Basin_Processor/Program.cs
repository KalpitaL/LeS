using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacificRoutine;
using System.Xml;

namespace HTTP_Pacific_Basin_Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            PBRoutine _wrapper = new PBRoutine();
            if (LeSSysInfo.LeSMain.GetLeSSysInfo() == LeSSysInfo.LeSReturn.Success_1)
            {               
                _wrapper.LogText = "================" + DateTime.Now.ToString() + "================";
                _wrapper.LogText = "DNVGL HTTP Processor started";
                Console.Title = "DNVGL HTTP Processor";
                _wrapper.Initialise();
                string AppsettingsConfig = AppDomain.CurrentDomain.BaseDirectory + "Appsettings.xml";
                ReadConfig(AppsettingsConfig, _wrapper);
                _wrapper.LogText = "DNVGL HTTP Processor stopped";
                _wrapper.LogText = "====================================================";
            }
            else
            {
                _wrapper.LogText = "Unauthorized access please contact LeS Support";
            }
        }

        public static void ReadConfig(string sConfigFile, PBRoutine _wrapper)
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
                //string sAuditMesage = "Exception while processing RFQ download " + e.GetBaseException().ToString();
                string sAuditMesage = "Unable to process file due to " + e.GetBaseException().ToString();
                _wrapper.LogText = sAuditMesage;
                _wrapper.CreateAuditFile("", "DNVGL_HTTP", "", "Error", "LeS-1004:"+sAuditMesage, _wrapper.BuyerCode, _wrapper.SupplierCode, _wrapper.AuditPath);
            }            
        }

        public static bool ProcessUser(PBRoutine _wrapper)
        {
            bool _result = true;
            _wrapper.LoadAppsettings();
            _wrapper.LogText = "";
            _wrapper.LogText = "Supplier: " + _wrapper.SupplierCode;
            _wrapper.LogText = "Process started for " + _wrapper.BuyerCode;
            foreach (string strAction in _wrapper.Actions)
            {
                try
                {
                    switch (strAction.ToUpper())
                    {
                        case "RFQ": _wrapper.Read_MailTextFiles(); break;
                        case "PO": _wrapper.ProcessPO(); break;
                        case "QUOTE": _wrapper.ProcessQuote(); break;
                        case "POC": _wrapper.ProcessPOC(); break;
                    }
                }
                catch (Exception ex)
                {
                    _wrapper.LogText = "Exception while processing " + strAction.ToUpper() + " : " + ex.GetBaseException().ToString();
                }
            }

            //if (_wrapper.SiteLogout())
            //{
            //    _wrapper.LogText = "User " + _wrapper.Userid + " logged out successfully." + Environment.NewLine;
            //    _result = true;
            //}
            //else
            //{
            //    string sAudit = "Unable to log out User " + _wrapper.Userid + ". Multi login process stopped.";
            //    _wrapper.LogText = sAudit;

            //    string filename = _wrapper.ImgPath + "\\Sertica_LoginOutFailed_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
            //    if (!_wrapper.PrintScreen(filename)) filename = "";

            //    _wrapper.CreateAuditFile(filename, "Sertica_HTTP_Downloader", "", "Error", sAudit, _wrapper.BuyerCode, _wrapper.SupplierCode, _wrapper.AuditPath);
            //    _result = false;
            //}

            return _result;
        }
    }
}
