using Http_Phoenix_Routine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Http_Phoenix_Processor
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Http_Download_Routine _wrapper = new Http_Download_Routine();
            try
            {
                if (LeSSysInfo.LeSMain.GetLeSSysInfo() == LeSSysInfo.LeSReturn.Success_1)
                {
                    if (_wrapper.CheckInstance())
                    {
                        Console.Title = "Phoenix HTTP processor";
                        _wrapper.LogText = "================" + DateTime.Now.ToString() + "================";
                        _wrapper.LogText = "Phoenix HTTP processor started";
                        _wrapper.Initialise();
                        _wrapper.ProcessLinkFiles();
                        //_wrapper.ProcessQuote(); // Commented by Sanjita (13-Dec-18) as not in use //
                        _wrapper.LogText = "Phoenix HTTP processor stopped";
                        _wrapper.LogText = "====================================================";
                        _wrapper.LogText = "";
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
                            _wrapper.LogText = "Killing Process.";
                            _wrapper.KillProcess();
                        }
                    }
                }
                else
                {
                    _wrapper.LogText = "Unauthorized access please contact LeS Support";
                }
            }
            catch (Exception ex)
            {
                 _wrapper.LogText="ERROR : " + ex.Message;
            }
            finally
            {
                Environment.Exit(0);
            }
        }
    }
}
