using Http_CloudFleetManager_Routine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Http_CloudFleetManager_Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            Http_Download_Routine _wrapper = new Http_Download_Routine();
            try
            {
                if (_wrapper.CheckInstance())
                {
                    _wrapper.LogText = "================" + DateTime.Now.ToString() + "================";
                    _wrapper.LogText = "CFM HTTP Processor started";
                    Console.Title = "CFM HTTP Processor";
                    _wrapper.Initialise();
                    ProcessTransaction(_wrapper);
                    _wrapper.LogText = "CFM HTTP Processor stopped";
                    _wrapper.LogText = "====================================================";
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
            catch (Exception ex)
            {
                _wrapper.LogText = "ERROR : " + ex.Message;
            }
            finally
            {
                Environment.Exit(0);
            }
        }

        public static void ProcessTransaction(Http_Download_Routine _wrapper)
        {
            _wrapper.LoadAppsettings();
            _wrapper.Read_MailTextFiles();
        }
    }
}
