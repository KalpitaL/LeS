﻿using Excel_SPL_Routine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Excel_SpL_Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (CheckInstance())
            {
                Excel_SPL_Routine.Download_Excel_Routine _routine = new Excel_SPL_Routine.Download_Excel_Routine();
                try
                {
                    _routine.LogText = "========================================================";
                    _routine.LogText = "Excel Special Routine started.";
                    string _Excel_Path = convert.ToString(ConfigurationManager.AppSettings["EXCEL_FILE_PATH"]).Trim();//28-8-2017
                    List<string> _sFilelst = GetExcelList(_Excel_Path);
                    if (_sFilelst.Count > 0)//08-09-17
                    {
                        _routine.LogText = _sFilelst.Count + " Excel file found.";
                        for (int i = 0; i < _sFilelst.Count; i++)
                        {
                            string xlsFile = convert.ToString(_sFilelst[i]);
                            if (_routine.Process_Excel_File(xlsFile))
                            {
                                _routine.MoveFiles(xlsFile, Path.GetDirectoryName(xlsFile) + @"\Backup\" + Path.GetFileName(xlsFile));
                                _routine.LogText = "Excel File '" + Path.GetFileName(xlsFile) + "' move to Backup.";
                            }
                            else
                            {
                                _routine.MoveFiles(xlsFile, Path.GetDirectoryName(xlsFile) + @"\Error\" + Path.GetFileName(xlsFile));
                                _routine.LogText = "Excel File '" + Path.GetFileName(xlsFile) + "' move to Error.";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _routine.LogText = "Error on Main. Error : " + ex.Message + " Stack Trace : " + ex.StackTrace;
                }
                finally
                {
                    _routine.LogText = "Excel Special Routine stopped.";
                    _routine.LogText = "========================================================";
                }
            }
        }

        public static bool CheckInstance()
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
                    _result = false;
                }
                else
                {
                    _result = true;
                }
            }
            catch
            {
                // do nothing
            }
            return _result;
        }

        private static List<string> GetExcelList(string sPath)
        {
            List<string> _FileList = new List<string>();
            _FileList = Directory.GetFiles(sPath, "*.xlsm").ToList();
            return _FileList;
        }
    }
}