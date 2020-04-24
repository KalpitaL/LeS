using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HTTP_KGJS_Routine;
using System.IO;

namespace HTTP_KGJS_Processor
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            HTTP_KGJS_Routine.HTTP_KGJS_Routine _wrapper = new HTTP_KGJS_Routine.HTTP_KGJS_Routine();
            try
            {
                if (_wrapper.CheckInstance())
                {
                    _wrapper.LogText = "================" + DateTime.Now.ToString() + "================";
                    _wrapper.LogText = "KGJS HTTP Processor started";
                    Console.Title = "KGJS HTTP Processor";
                    _wrapper.Initialise();
                    ProcessTransaction(_wrapper);
                    _wrapper.LogText = "KGJS HTTP Processor stopped";
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

        public static void ProcessTransaction(HTTP_KGJS_Routine.HTTP_KGJS_Routine  _wrapper)
        {
            _wrapper.LoadAppsettings();
            _wrapper.Read_HTMLFiles();
            _wrapper.UploadFiles();
        }
    }
}
