using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Http_Asiatic_Routine;
using System.IO;
using System.Configuration;

namespace Http_Asiatic_Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            Http_Download_Routine _wrapper = new Http_Download_Routine();
            try
            {
                if (LeSSysInfo.LeSMain.GetLeSSysInfo() == LeSSysInfo.LeSReturn.Success_1)
                {
                    string procrName = Convert.ToString(ConfigurationManager.AppSettings["NO_AUDITNAME"].Trim());
                    if (_wrapper.CheckInstance())
                    {
                        _wrapper.LogText = "================" + DateTime.Now.ToString() + "================";
                        _wrapper.LogText = procrName + " HTTP Processor started";//Asiatic
                        Console.Title = procrName + " HTTP Processor";
                        _wrapper.Initialise();
                        ProcessTransaction(_wrapper);
                        _wrapper.LogText = procrName + " HTTP Processor stopped at " + DateTime.Now;
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
                else
                {
                    _wrapper.LogText = "Unauthorized access please contact LeS Support";
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
#region commented

//_wrapper.ProcessMTMLFiles();//quote

//delete
//foreach (string sAction in _wrapper.Actions)
//{
//    try
//    {
//        switch (sAction.ToUpper())
//        {
//            //case "RFQ":_wrapper.Read_MailTextFiles(); break;
//            //case "PO": _wrapper.ProcessOrders(); break;
//            case "QUOTE": _wrapper.ProcessMTMLFiles(); break;
//        }
//    }
//    catch (Exception ex)
//    {
//        _wrapper.LogText = "Exception while processing " + sAction.ToUpper() + " : " + ex.GetBaseException().ToString();
//    }
//}

#endregion