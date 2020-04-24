using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Http_GreatShip_Processor
{
    class Program
    {
        static string strLogpath = "";
        static void Main(string[] args)
        {
            Http_GreatShip_Routine.Http_Download_Routine _wrapper = new Http_GreatShip_Routine.Http_Download_Routine();
            try
            {
                if (_wrapper.CheckInstance())
                {
                    _wrapper.LogText = "====================================================";
                    _wrapper.LogText = "Http GreatShip Processor started at " + DateTime.Now;
                    Console.Title = "Http GreatShip RFQ,Quote Processor";
                    _wrapper.Initialise();
                    ProcessTransaction(_wrapper);
                    _wrapper.LogText = "Http GreatShip Processor stopped at " + DateTime.Now;
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
            catch (Exception ex)
            {
                _wrapper.LogText = "ERROR : " + ex.Message;
            }
            finally
            {
                Environment.Exit(0);
            }
        }

        public static void ProcessTransaction(Http_GreatShip_Routine.Http_Download_Routine _wrapper)
        {
            _wrapper.LoadAppsettings();
            if (_wrapper.DoLogin("input", "id", "btnLogin"))
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
                        _wrapper.LogText = "Exception while processing " + sAction.ToUpper() + " : " + ex.GetBaseException().ToString();
                    }
                }
            }

        }
    }
}
