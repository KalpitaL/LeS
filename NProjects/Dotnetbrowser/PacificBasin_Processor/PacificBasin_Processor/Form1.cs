using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacificBasinRoutine;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace PacificBasin_Processor
{
    public partial class Form1 : Form
    {
        PacificRoutine psRoutine = new PacificRoutine();
        string strAuditName = "";
        
       // string strLogPath = "";//strShowForm = ""
       // string[] arrActions;
       // bool isPO = false;

        public Form1()
        {
            InitializeComponent();
          //  PacificRoutine psRoutine = new PacificRoutine();
         
            Controls.Add(psRoutine._netWrapper.browserView);
            strAuditName = Convert.ToString(ConfigurationManager.AppSettings["AUDITNAME"].Trim());//added by kalpita on 28/11/2019
        }

        //public void AppSetup()
        //{
        //    //if (ConfigurationManager.AppSettings["ShowForm"] != null && ConfigurationManager.AppSettings["ShowForm"].Length > 0) strShowForm = ConfigurationManager.AppSettings["ShowForm"].Trim();
        //   // if (ConfigurationManager.AppSettings["Actions"] != null && ConfigurationManager.AppSettings["Actions"].Length > 0) arrActions = ConfigurationManager.AppSettings["Actions"].Split(',');
            
        //   // if (strShowForm == "") strShowForm = "FALSE";
       
        //    strLogPath = Application.StartupPath + "\\Log";
        //}

        //private void ProcessRFQ()
        //{
        //   psRoutine.Read_MailTextFiles();
        //}

        //public void ProcessQuote()
        //{
        //    psRoutine.ProcessQuote();
        //}

        //public void ProcessPO()
        //{
        //    psRoutine.ProcessPO();
        //}

        //public void ProcessPOC()
        //{
        //    psRoutine.ProcessPOC();
        //}

        public void Showform(PacificBasinRoutine.PacificRoutine psRoutine)
        {
            if (psRoutine.dctAppSettings["ShowForm"].Trim().ToUpper() == "FALSE")
                this.Visible = false;
            else
                this.Visible = true;
        }

        //private void Form1_Load(object sender, EventArgs e)
        //{
        //    //  AppSetup();
        //    //  PacificRoutine psRoutine = new PacificRoutine();
        //    //  Console.Title = "Pacific Basin RFQ,PO,Quote,POC HTTP processor";

        //    if (LeSSysInfo.LeSMain.GetLeSSysInfo() == LeSSysInfo.LeSReturn.Success_1)
        //    {
        //        if (CheckAppInstance())//CheckInstance changed  for testing on 26/12/2019
        //        {
        //            psRoutine._netWrapper.LogText = "================" + DateTime.Now.ToString() + "================";
        //            psRoutine._netWrapper.LogText = strAuditName + " RFQ,PO,Quote,POC processor started";           //Pacific Basin,Http
        //            string AppsettingsConfig = AppDomain.CurrentDomain.BaseDirectory + "Appsettings.xml";
        //            ReadConfig(AppsettingsConfig);
        //            psRoutine.dispose();
        //            psRoutine._netWrapper.LogText = strAuditName + " RFQ,PO,Quote,POC processor stopped.";
        //            psRoutine._netWrapper.LogText = "====================================================";
        //            this.Controls.Clear();
        //            this.Hide();
        //            this.Close();//changed by kalpita on 03/12/2019
        //            this.Dispose();
        //            Environment.Exit(0);
        //        }
        //        //else
        //        //{
        //        //    string strlogpath = AppDomain.CurrentDomain.BaseDirectory + "Log";
        //        //    string strlogfile = @"Log_" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
        //        //    int iCounter = 0;
        //        //    if (File.Exists(strlogpath + "\\" + strlogfile))
        //        //    {
        //        //        string[] ArrLogLines = File.ReadAllLines(strlogpath + "\\" + strlogfile);
        //        //        if (ArrLogLines.Length > 15)
        //        //        {
        //        //            for (int i = (ArrLogLines.Length - 1); i > (ArrLogLines.Length - 15); i--)
        //        //            {
        //        //                if (ArrLogLines[i].Contains("Application is already running"))
        //        //                {
        //        //                    iCounter++;
        //        //                }
        //        //            }
        //        //        }
        //        //    }
        //        //    if (iCounter > 5)
        //        //    {
        //        //        psRoutine._netWrapper.LogText = "Killing Process.";
        //        //        KillProcess();
        //        //    }
        //        //}
        //    }
        //    else
        //    {
        //        psRoutine._netWrapper.LogText = "Unauthorized access please contact LeS Support";
        //    }
        //}

        private void Form1_Load(object sender, EventArgs e)
        {
            if (LeSSysInfo.LeSMain.GetLeSSysInfo() == LeSSysInfo.LeSReturn.Success_1)
            {
                if (!CheckAppInstance())//CheckInstance changed  for testing on 26/12/2019
                {
                    psRoutine._netWrapper.LogText = "================" + DateTime.Now.ToString() + "================";
                    psRoutine._netWrapper.LogText = strAuditName + " RFQ,PO,Quote,POC processor started";           //Pacific Basin,Http
                    string AppsettingsConfig = AppDomain.CurrentDomain.BaseDirectory + "Appsettings.xml";
                    ReadConfig(AppsettingsConfig);
                    psRoutine.dispose();
                    psRoutine._netWrapper.LogText = strAuditName + " RFQ,PO,Quote,POC processor stopped.";
                    psRoutine._netWrapper.LogText = "====================================================";
                    this.Controls.Clear();
                    this.Hide();
                    this.Close();//changed by kalpita on 03/12/2019
                    this.Dispose();
                    Environment.Exit(0);
                }
            }
            else
            {
                psRoutine._netWrapper.LogText = "Unauthorized access please contact LeS Support";
            }
        }

        public void ReadConfig(string sConfigFile)
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
                        psRoutine.dctAppSettings.Clear();
                        XmlNodeList _childAppsettings = _appSetting.ChildNodes;
                        foreach (XmlNode _UserSetting in _childAppsettings)
                        {
                            userSetting = (XmlElement)_UserSetting;
                            psRoutine.dctAppSettings.Add(userSetting.Name, userSetting.InnerText);
                        }
                        if (!ProcessUser()) break;
                    }
                }
            }
            catch (Exception e)
            {
                string sAuditMesage = "Exception while processing reading config" + e.GetBaseException().ToString();
                psRoutine._netWrapper.LogText = sAuditMesage;
            }
        }

        public bool ProcessUser()
        {
            bool _result = true;
            Showform(psRoutine);
            psRoutine.LoadAppSettings();
            psRoutine._netWrapper.LogText = "";
            psRoutine._netWrapper.LogText = "Buyer: " + psRoutine.strBuyerName;
            //if (psRoutine.dctAppSettings["Actions"].Trim().Split('|').Length >= 3) isPO = true;
            //else isPO = false;

            foreach (string strAction in psRoutine.dctAppSettings["Actions"].Trim().Split(','))
            {
                try
                {
                    switch (strAction.ToUpper())
                    {
                        case "RFQ": psRoutine.Read_MailTextFiles(); break;
                        case "QUOTE": psRoutine.ProcessQuote(); break;
                        case "PO": psRoutine.ProcessPO(); break;
                        case "POC": psRoutine.ProcessPOC(); break;
                    }
                }
                catch (Exception ex)
                {
                    psRoutine._netWrapper.LogText = "Exception while processing " + strAction.ToUpper() + " : " + ex.GetBaseException().ToString();
                }
            }

            return _result;
        }

        public bool CheckAppInstance()
        {
            Boolean IsExists = false;
            try
            {
                Process current = Process.GetCurrentProcess();
                DateTime CurrentTimeStamp = current.StartTime;
                psRoutine._netWrapper.LogText = current.StartTime.ToString();
                DateTime ValidTimeStamp = CurrentTimeStamp.AddMinutes(35);
                Process[] _procArr = System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location));
                if (_procArr != null)
                {
                    IsExists = _procArr.Count() > 1;
                    if (IsExists)
                    {
                        psRoutine._netWrapper.LogText = "Application is already running";
                        if (CurrentTimeStamp > ValidTimeStamp) { current.Kill(); }
                        foreach (Process process in _procArr)
                        {
                            process.Kill();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                psRoutine._netWrapper.LogText = "Error on CheckAppInstance : " + ex.Message;
            }
            return IsExists;
        }

        public bool CheckInstance()
        {
            bool _result = false;
            int nCnt = 0;
            try
            {
                CheckAppInstance();

                int RunningCounter = 0;
                Process current = Process.GetCurrentProcess();
                string _cTimeStamp = current.StartTime.ToString("dd-MM-yyyy HH:mm:ss:ff");
                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.ProcessName == current.ProcessName) nCnt++;
                }
                if (nCnt > 1)
                {
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Running.txt"))
                    {
                        RunningCounter = Convert.ToInt32(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Running.txt"));
                    }
                    else
                    {
                        File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Running.txt", RunningCounter.ToString());
                    }
                    if (RunningCounter < 4)
                    {
                        psRoutine._netWrapper.LogText = "Application is already running";
                        RunningCounter++;
                        File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Running.txt", RunningCounter.ToString());
                    }
                    else
                    {
                        foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                        {
                            try
                            {
                                if (process.ProcessName == current.ProcessName)
                                {
                                    string _pTimeStamp = process.StartTime.ToString("dd-MM-yyyy HH:mm:ss:ff");
                                    if (_pTimeStamp != _cTimeStamp)
                                    {
                                        File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Running.txt", "0");
                                        psRoutine._netWrapper.LogText = "Killing Process " + process.ProcessName + " : " + process.StartTime.ToString("dd-MM-yyyy HH:mm:ss:ff");
                                        process.Kill();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {}
                        }
                    }
                    _result = false;
                }
                else
                {
                    _result = true;
                }
            }
            catch (Exception ex)
            {
                psRoutine._netWrapper.LogText = "Error on CheckInstance : " + ex.Message;
            }
            return _result;
        }

        public void KillProcess()
        {
            Process current = Process.GetCurrentProcess();
            foreach (Process process in Process.GetProcessesByName(current.ProcessName))
            {
                psRoutine._netWrapper.LogText = "Killing Process " + process.ProcessName + " : " + process.StartTime.ToString("dd-MM-yyyy HH:mm:ss:ff");
                process.Kill();
            }
        }

    }
}

#region commented
//if (arrActions.Length >= 3) isPO = true;
//else isPO = false;

//foreach (string strAction in arrActions)
//{
//    try
//    {
//        switch (strAction.ToUpper())
//        {
//            case "RFQ": ProcessRFQ(); break;
//            case "QUOTE": ProcessQuote(isPO); break;
//            case "PO": ProcessPO(); break;
//        }
//    }
//    catch (Exception ex)
//    {
//        WriteLog("Exception while processing " + strAction.ToUpper() + " : " + ex.GetBaseException().ToString());
//    }
//}

//private void WriteLog(string _logText, string _logFile = "")
//{
//    string _logfile = @"Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
//    if (_logFile.Length > 0) { _logfile = _logFile; }

//    Console.WriteLine(_logText);
//    File.AppendAllText(strLogPath + @"\" + @_logfile, DateTime.Now.ToString() + " " + _logText + Environment.NewLine);
//}



//public void KillProcess()
//{
//    Process current = Process.GetCurrentProcess();
//    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
//    {
//        process.Kill();
//    }
//}

//public bool CheckInstance()//namrata
//{
//    bool isResult = false;
//    int iCnt = 0;
//    try
//    {
//        Process current = Process.GetCurrentProcess();
//        foreach (Process process in Process.GetProcessesByName(current.ProcessName))
//        {
//            if (process.ProcessName == current.ProcessName) iCnt++;
//        }
//        if (iCnt > 1)
//        {
//            psRoutine._netWrapper.LogText = "Application is already running";
//            isResult = false;
//        }
//        else
//        {
//            isResult = true;
//        }
//    }
//    catch
//    {

//    }
//    return isResult;
//}

#endregion