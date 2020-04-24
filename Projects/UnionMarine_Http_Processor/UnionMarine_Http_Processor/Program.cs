using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using UnionMarine_Http_Routine;

namespace UnionMarine_Http_Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            UnionMarine_Http_Routine.Routines _wrapper = new Routines();
            if (LeSSysInfo.LeSMain.GetLeSSysInfo() == LeSSysInfo.LeSReturn.Success_1)
            {
                if (CheckInstance(_wrapper))
                {
                    _wrapper.LogText = "================" + DateTime.Now.ToString() + "================";
                    _wrapper.LogText = "Union Marine HTTP processor started";
                    _wrapper.Download_FileProcess();
                    _wrapper.LogText = "Union Marine HTTP processor stopped";
                    _wrapper.LogText = "====================================================";
                }
            }
            else
            {
                _wrapper.LogText = "Unauthorized access please contact LeS Support";
            }
        }

        public static bool CheckInstance(UnionMarine_Http_Routine.Routines _wrapper)
        {
            bool _result = false;
            int nCnt = 0;
            try
            {
                Process current = Process.GetCurrentProcess();
                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.ProcessName == current.ProcessName) nCnt++;
                }
                if (nCnt > 1)
                {
                    _wrapper.LogText ="Application is already running";
                    _result = false;
                }
                else
                {
                    _result = true;
                }
            }
            catch
            {

            }
            return _result;
        }

    }
}
