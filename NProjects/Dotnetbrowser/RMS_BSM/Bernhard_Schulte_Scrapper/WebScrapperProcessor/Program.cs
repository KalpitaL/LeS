using PluginInterface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapperProcessor
{
    class Program
    {
        private static IWrapperPlugin Plugin;
        static string LOG_PATH = "", RFQ_PATH = "", MAILFILE_PATH = "", Mail_Template = "", FROM_EMAIL_ID = "", MAIL_BCC = "", Processor, ATTACHMENT_INBOX = "";
        static string[] ACTIONS;

      
        static void Main(string[] args)
        {
            AppSetup();
            if (!IsProcessRunning())
            {
               if (PlugInLoad())
                {
                    //if (LogIn())
                    //{
                        foreach (string sAction in ACTIONS)
                        {
                            try
                            {
                                switch (sAction.ToUpper())
                                {
                                    case "RFQ": ProcessRFQ(); break;
                                     case "QUOTE": ProcessQuote(); break;
                                    default: break;
                                }
                            }
                            catch (Exception e)
                            {
                                WriteLog("Exception while processing " + sAction.ToUpper() + " : " + e.GetBaseException().ToString());
                            }
                        }
                       
                    //}
                }
            }
            Environment.Exit(0);
        }

        public static bool IsProcessRunning()
        {
            int nCnt = 0;
            Process p = Process.GetCurrentProcess();
            foreach (Process process in Process.GetProcessesByName(p.ProcessName))
            {
                if (process.ProcessName == p.ProcessName) nCnt++;
            }
            //Process[] p = Process.GetProcesses();
            //string cFormName = "WebScrapperProcessor";
            //var runningProcessByName = Process.GetProcessesByName(cFormName);
            //if (runningProcessByName.Length <= 1)
            if (nCnt <= 1)
            {
                return false;
            }
            else
            {
                WriteLog("Application is already running");
                return true;
            }
        }

        public static void AppSetup()
        {
            if (ConfigurationManager.AppSettings["LOGPATH"] != null && ConfigurationManager.AppSettings["LOGPATH"].Length > 0) LOG_PATH = ConfigurationManager.AppSettings["LOGPATH"].Trim();
            if (ConfigurationManager.AppSettings["RFQ_PATH"] != null && ConfigurationManager.AppSettings["RFQ_PATH"].Length > 0) RFQ_PATH = ConfigurationManager.AppSettings["RFQ_PATH"].Trim();
            if (ConfigurationManager.AppSettings["ACTIONS"] != null && ConfigurationManager.AppSettings["ACTIONS"].Length > 0) ACTIONS = ConfigurationManager.AppSettings["ACTIONS"].Split(',');
            if (ConfigurationManager.AppSettings["MAILFILE_PATH"] != null && ConfigurationManager.AppSettings["MAILFILE_PATH"].Length > 0) MAILFILE_PATH = ConfigurationManager.AppSettings["MAILFILE_PATH"].Trim();
            if (ConfigurationManager.AppSettings["ATTACHMENT_INBOX"] != null && ConfigurationManager.AppSettings["ATTACHMENT_INBOX"].Length > 0) ATTACHMENT_INBOX = ConfigurationManager.AppSettings["ATTACHMENT_INBOX"].Trim();
            if (LOG_PATH == "")
                LOG_PATH = AppDomain.CurrentDomain.BaseDirectory + "Log";

            if (RFQ_PATH == "")
                RFQ_PATH = AppDomain.CurrentDomain.BaseDirectory + "RFQ";

            if (MAILFILE_PATH == "")
                MAILFILE_PATH = AppDomain.CurrentDomain.BaseDirectory + "Mail_Files";

            if (ATTACHMENT_INBOX == "")
                ATTACHMENT_INBOX = AppDomain.CurrentDomain.BaseDirectory + "Attachment_Inbox";
            Processor = ConfigurationManager.AppSettings["PROCESSOR"].Trim();
            Mail_Template = AppDomain.CurrentDomain.BaseDirectory + "Mail_Temp"+"\\ESUPPLIER_MESSAGE.txt";
            FROM_EMAIL_ID = ConfigurationManager.AppSettings["FROM_EMAIL_ID"].Trim();
            MAIL_BCC = ConfigurationManager.AppSettings["MAIL_BCC"].Trim();
             if (ConfigurationManager.AppSettings["CONSOLE_TITLE"] != null && ConfigurationManager.AppSettings["CONSOLE_TITLE"].Length > 0)
                 Console.Title = ConfigurationManager.AppSettings["CONSOLE_TITLE"].Trim();
        }

        public static bool LogIn()
        {
            bool IsLoggedIn = false;
            try
            {
                IsLoggedIn = Plugin.LogIn();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException().ToString());
            }
            return IsLoggedIn;
        }

        private static bool PlugInLoad()
        {
            bool PluginLoaded = false;
            try
            {
                Plugin = LoadAssembly(Processor);
                PluginLoaded = true;
                Console.WriteLine(Processor + " Loaded.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load " + Processor + "!\n" + ex.ToString());
                WriteLog("Failed to load " + Processor + "!\n" + ex.GetBaseException().ToString());
            }
            return PluginLoaded;
        }

        private static PluginInterface.IWrapperPlugin LoadAssembly(string assemblyPath)
        {
            string assembly = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory+@"\"+assemblyPath);
            Assembly ptrAssembly = Assembly.LoadFile(assembly);

            foreach (Type item in ptrAssembly.GetTypes())
            {

                if (!item.IsClass) continue;
                if (item.GetInterfaces().Contains(typeof(PluginInterface.IWrapperPlugin)))
                {
                    return (PluginInterface.IWrapperPlugin)Activator.CreateInstance(item);
                }

            }
            throw new Exception("Invalid DLL, Interface not found!");
        }

        private static void WriteLog(string _logText, string _logFile = "")
        {
            if (!Directory.Exists(LOG_PATH))
                Directory.CreateDirectory(LOG_PATH);
            string _logfile = @"Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (_logFile.Length > 0) { _logfile = _logFile; }

            Console.WriteLine(_logText);
            File.AppendAllText(LOG_PATH + @"\" + @_logfile, DateTime.Now.ToString() + " " + _logText + Environment.NewLine);
        }

        private static void ProcessRFQ()
        {
            if (LogIn())
            {
                if (!Directory.Exists(RFQ_PATH))
                    Directory.CreateDirectory(RFQ_PATH);
                List<string> slProcessRFQ = Plugin.GetProcessedItems(PluginInterface.eActions.RFQ);
                bool result = Plugin.ProcessRFQ(slProcessRFQ, RFQ_PATH);
            }
        }

        private static void ProcessQuote()
        {
            Plugin.ProcessQuote(MAILFILE_PATH, Mail_Template, FROM_EMAIL_ID, MAIL_BCC);
        }
    }
}
