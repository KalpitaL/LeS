using Http_Kotc_Routine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Http_Kotc_Processor
{
    class Program
    {
        static string strLogpath = "";
        static void Main(string[] args)
        {
            try
            {
                if (CheckInstance())
                {
                    Http_Kotc_Routine.KotcRoutine _Routine = new Http_Kotc_Routine.KotcRoutine();
                    _Routine.LogText = "====================================================";
                    _Routine.LogText = "Http Kotc processor started at " + DateTime.Now;
                    string AppsettingsConfig = AppDomain.CurrentDomain.BaseDirectory + "Appsettings.xml";
                    ReadConfig(AppsettingsConfig, _Routine);  
                   // _Routine.Download_Process();
                    _Routine.LogText = "Http Kotc processor stopped at " + DateTime.Now;
                    _Routine.LogText = "====================================================";
                    _Routine.LogText = "";
                }
                else
                {
                    string strlogpath = AppDomain.CurrentDomain.BaseDirectory + "Log";
                    string strlogfile = @"Log_" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
                    int iCounter = 0;
                    if (File.Exists(strlogpath + "\\" + strlogfile))
                    {
                        string[] ArrLogLines = File.ReadAllLines(strlogpath + "\\" + strlogfile);
                        if (ArrLogLines.Length > 15)
                        {
                            for (int i = (ArrLogLines.Length - 1); i > (ArrLogLines.Length - 15); i--)
                            {
                                if (ArrLogLines[i].Contains("Application is already running"))
                                {
                                    iCounter++;
                                }
                            }
                        }
                    }
                    if (iCounter > 5)
                    {
                        WriteLog("Killing Process.");
                        KillProcess();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog("ERROR : " + ex.Message);
            }
            finally
            {
                Environment.Exit(0);
            }
        }

        public static void ReadConfig(string sConfigFile, Http_Kotc_Routine.KotcRoutine _routine)
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
                        _routine.dctAppSettings.Clear();
                        XmlNodeList _childAppsettings = _appSetting.ChildNodes;
                        foreach (XmlNode _UserSetting in _childAppsettings)
                        {
                            userSetting = (XmlElement)_UserSetting;
                            _routine.dctAppSettings.Add(userSetting.Name, userSetting.InnerText);
                        }
                        if (!ProcessUser(_routine)) break;
                    }

                }

            }
            catch (Exception e)
            {
                string sAuditMesage = "Exception while processing RFQ download" + e.GetBaseException().ToString();
                _routine.LogText = sAuditMesage;
                string filename = _routine.strScreenShotPath + "\\KOTC_Exception_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                _routine.PrintScreen(filename);
                //_routine.CreateAuditFile(filename, "KTOC_HTTP_Downloader", "", "Error", sAuditMesage);
                _routine.CreateAuditFile(filename, "KOTC_HTTP_Downloader", "", "Error", "LeS-1012:Parameters not configured,"+ e.GetBaseException().ToString());
            }
        }

        public static bool ProcessUser(Http_Kotc_Routine.KotcRoutine _routine)
        {
            bool _result = true;
            _routine.LoadAppSettings();
            _routine.LogText = "Process started for " + _routine.strUsername;
            
            if (_routine.doLogin())
            {
                
                _routine.LogText = "Logged in successfully.";
                foreach (string sAction in _routine.Actions)
                {
                    try
                    {
                        switch (sAction.ToUpper())
                        {
                            case "RFQ":
                                {
                                    List<string> lstProcessedItem = _routine.GetProcessedItems(eActions.RFQ);
                                    _routine.Process_RFQ(lstProcessedItem); break;
                                }
                        }
                    }
                    catch (Exception ex)
                    {
                        _routine.LogText = "Exception while processing " + sAction.ToUpper() + " : " + ex.GetBaseException().ToString();
                    }
                }
                if (_routine.LogOut())
                {
                    _routine.LogText = "User " + _routine.strUsername + " logged out successfully.";
                    _result = true;
                }
                else
                {
                    string sAudit = "Unable to log out User " + _routine.strUser + ". Multi login process stopped.";
                    _routine.LogText = sAudit;
                    string filename = _routine.strScreenShotPath + "\\KOTC_LoginOutFailed_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + _routine.strSupplier + ".png";
                    if (!_routine.PrintScreen(filename)) filename = "";

                    //_routine.CreateAuditFile(filename, "KOTC_HTTP_Downloader", "", "Error", sAudit);
                    _routine.CreateAuditFile(filename, "KOTC_HTTP_Downloader", "", "Error", "LeS-1015:Unable to logout");
                    _result = false;
                }
            }
            _routine.LogText = "Process stopped for " + _routine.strUsername;
       
            return _result;
        }

        public static bool CheckInstance()
        {
            bool isResult = false;
            int iCnt = 0;
            try
            {
                Process current = Process.GetCurrentProcess();
                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.ProcessName == current.ProcessName) iCnt++;
                }
                if (iCnt > 1)
                {
                    WriteLog("Application is already running");
                    isResult = false;
                }
                else
                {
                    isResult = true;
                }
            }
            catch
            {

            }
            return isResult;
        }

        public static void KillProcess()
        {
            Process current = Process.GetCurrentProcess();
            foreach (Process process in Process.GetProcessesByName(current.ProcessName))
            {
                process.Kill();
            }
        }

        public static void WriteLog(string strLogText, string strLogFile = "")
        {
            try
            {
                string LogFile = "";
                if (strLogpath == null || strLogpath == "") strLogpath = AppDomain.CurrentDomain.BaseDirectory + "\\Log";
                string _logfile = LogFile;
                if (LogFile.Trim() == "") _logfile = "Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";//changed date format on 21-06-2019
                if (strLogFile.Length > 0) { _logfile = strLogFile; }
                if (!Directory.Exists(strLogpath)) Directory.CreateDirectory(strLogpath);
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : " + strLogText);
                File.AppendAllText(strLogpath + @"\" + @_logfile, Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " : " + strLogText);
            }
            catch { }
        }
    }
}
