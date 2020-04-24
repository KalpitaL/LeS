using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using GreenWave_Http_Routine;
using System.IO;

namespace GreenWave_Http_Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            GreenWave_Http_Routine.Routines _wrapper = new GreenWave_Http_Routine.Routines();
            try
            {
                if (LeSSysInfo.LeSMain.GetLeSSysInfo() == LeSSysInfo.LeSReturn.Success_1)
                {
                    if (_wrapper.CheckInstance())
                    {
                        Console.Title = "GreenWave HTTP processor";
                        _wrapper.LogText = "================" + DateTime.Now.ToString() + "================";
                        _wrapper.LogText = "GreenWave HTTP processor started";
                         ProcessUser(_wrapper);
                        _wrapper.LogText = "GreenWave HTTP processor stopped";
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
                _wrapper.LogText = "ERROR : " + ex.Message;
            }
            finally
            {
                Environment.Exit(0);
            }
        }

        public static bool ProcessUser(GreenWave_Http_Routine.Routines _wrapper)
        {
            bool _result = true;
            _wrapper.GetAppConfigSettings();
          
            foreach (string sAction in _wrapper.Actions)
            {
                try
                {
                    switch (sAction.ToUpper())
                    {
                        case "RFQ": _wrapper.ProcessRFQ();
                            break;
                        case "PO": _wrapper.ProcessOrders();
                            break;
                        case "QUOTE": _wrapper.ProcessQuotation();                           
                            break;
                    }
                }
                catch (Exception ex)
                {
                   _wrapper.LogText = "Exception while processing " + sAction.ToUpper() + " : " + ex.GetBaseException().ToString();
                }
            }           
            return _result;
        }


    }
}
