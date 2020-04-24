using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Http_Cido_Shipping_Processor
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
                    Http_Cido_Shipping_Routine.CidoRoutine _Routine = new Http_Cido_Shipping_Routine.CidoRoutine();
                    _Routine.LogText = "====================================================";
                    _Routine.LogText = "Http Cido Shipping processor started at "+DateTime.Now;
                    _Routine.Download_Process();
                    _Routine.LogText = "Http Cido Shipping processor stopped at "+DateTime.Now;
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
                if (LogFile.Trim() == "") _logfile = "Log_" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
                if (strLogFile.Length > 0) { _logfile = strLogFile; }
                if (!Directory.Exists(strLogpath)) Directory.CreateDirectory(strLogpath);
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : " + strLogText);
                File.AppendAllText(strLogpath + @"\" + @_logfile, Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " : " + strLogText);
            }
            catch { }
        }
    }
}
