using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKShipping;
using WebScraper;
using System.Configuration;

namespace RMS_SKShipping_Watin_WebScrapper
{
    class SKShippingScrapper 
    { 
        SKShipping_Routine _skroutine = new SKShipping_Routine();
        public void StartProcess()
        {

            string task = convert.ToString(ConfigurationManager.AppSettings["TASK"]).Trim();
            if (task.ToUpper().Contains("DOWNLOAD"))
            {
                Console.WriteLine("******* SKShipping RMD RFQ Downloader Started " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + " *******");
                _skroutine.taskToPerform = "DOWNLOAD";
                _skroutine.SetLog("Task to perform is '" + _skroutine.taskToPerform + "'");
                downloaduploadRFQ();
            }
            if (task.ToUpper().Contains("UPLOAD"))
            {
                Console.WriteLine("******* SKShipping RMD Quote Uploader Started " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + " *******");
                _skroutine.taskToPerform = "UPLOAD";
                _skroutine.SetLog("Task to perform is '" + _skroutine.taskToPerform + "'");
                downloaduploadRFQ();
            }
        }
      
        public void downloaduploadRFQ()
        {
            try
            {
                _skroutine.Start();
            }
            //catch (Exception ex) { _skroutine.SetLog(ex.Message.ToString()); }
            finally
            {
                if (_skroutine.taskToPerform == "DOWNLOAD")
                    Console.WriteLine("******* SKShipping RMD RFQ Downloader Stopped " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + " *******");
                else if (_skroutine.taskToPerform == "UPLOAD")
                    Console.WriteLine("******* SKShipping RMD Quote Uploader Stopped " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + " *******");
            }
        }
    }
}
