using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using MetroLesMonitor.Bll;
using System.Diagnostics;
using System.Globalization;
using System;


namespace MetroLesMonitor.LESMonitorPages
{
    public partial class Adaptors : System.Web.UI.Page
    {
        MetroLesMonitor.Common.Default Def = new MetroLesMonitor.Common.Default();

        protected void Page_Load(object sender, EventArgs e)
        {
            GetUrlDetails();
        }

        private void GetUrlDetails()
        {         
            if (HttpContext.Current.Session["ADDRESSID"] == null)
            {
                string Query = Request.Url.Query.Replace("?", "");
                if (!string.IsNullOrEmpty(Query))
                {
                    string value = MetroLesMonitor.Common.Default.DecryptURL(Query).Split('&')[0].Split('=')[1];
                    HttpContext.Current.Session["ADDRESSID"] = value;
                }
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetAdaptorGrids(string SERVTIME)
        {
            string json = "";
            int AddressID = 0;
            System.Data.DataSet _ds = new DataSet();
            if (HttpContext.Current.Session["ADDRESSID"] != null && HttpContext.Current.Session["ADDRESSID"]!="")
            {
                AddressID = Convert.ToInt32(HttpContext.Current.Session["ADDRESSID"]);
            }
            if (AddressID > 0)
            {
                LesClientConnectLog _connectLog = new LesClientConnectLog();
                _ds = _connectLog.SelectAll(AddressID);
                DataSet dsClone = _ds.Copy();

                dsClone.Tables[0].Columns.Add("LASTCONNECTDATE");
                dsClone.Tables[0].Columns.Add("NEXTCONNECTDATE");
                dsClone.Tables[0].Columns.Add("ADAPTORSTATUS");
                dsClone.Tables[0].Columns.Add("INTRVLSTATUS");
                dsClone.Tables[0].Columns.Add("ISLESCONNECT");

                foreach (DataRow row in dsClone.Tables[0].Rows)
                {
                    foreach (DataColumn column in dsClone.Tables[0].Columns)
                    {
                      
                        if (column.ColumnName == "LAST_CONNECT")
                        {
                            string value = Convert.ToDateTime(row.ItemArray[3]).ToString("dd/MM/yyyy hh:mm:ss tt");
                            row.SetField("LASTCONNECTDATE", value);
                            row.SetField("ADAPTORSTATUS", SetAdaptorStatus(Convert.ToDateTime(row.ItemArray[3]))); //changed by kalpita on 09/12/2017
                        }
                        else if (column.ColumnName == "NEXT_CONNECT")
                        {
                            string value = Convert.ToDateTime(row.ItemArray[5]).ToString("dd/MM/yyyy hh:mm:ss tt");
                            row.SetField("NEXTCONNECTDATE", value);
                           // row.SetField("ADAPTORSTATUS", SetAdaptorStatus(Convert.ToDateTime(row.ItemArray[5])));
                        }                    
                        row.SetField("INTRVLSTATUS", 0);
                        row.SetField("ISLESCONNECT", CheckLesAdaptor(AddressID.ToString()));
                    }
                }
                dsClone.AcceptChanges();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(dsClone.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetServicesGrids()
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            DataTable dtserv = _Routine.GetServicesLog();
            dtserv.TableName = "Services";
            DataSet dsServ = new DataSet();
            dsServ.Tables.Add(dtserv);
            XmlDocument servdoc = new XmlDocument();
            servdoc.LoadXml(dsServ.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(servdoc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetScheduleGrids()
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            DataTable dtSch = _Routine.GetSchedulerLog();
            dtSch.TableName = "Scheduler";
            DataSet dsSch = new DataSet();
            dsSch.Tables.Add(dtSch);
            XmlDocument Schdoc = new XmlDocument();
            Schdoc.LoadXml(dsSch.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(Schdoc);
            return json;
        }

        private static string SetAdaptorStatus(DateTime dtLastConnect)
        {
            string cStatus = "0";
            DateTime _dtLast = dtLastConnect;
            DateTime _dtNow = DateTime.Now.AddMinutes(-30);

            if (_dtNow > _dtLast)
            {
                cStatus = "1";
            }
            return cStatus;
        }

        /* changed by Kalpita on 05/09/2017 */
        //private static string SetAdaptorStatus(DateTime dtConnect) //dtNextConnect 
        //{
        //    string cStatus = "0"; DateTime _dtNow = DateTime.Now;
        //    //DateTime _dtNext = dtNextConnect; if (_dtNow > _dtNext){ cStatus = "1";}
        //    DateTime _dtCntTime = dtConnect; if (_dtNow > _dtCntTime) { cStatus = "1"; } //changed by Kalpita on 9/12/2017
        //    return cStatus;
        //}

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Run_Application(string APPLNO, string SERV_NAME, string SERV_PATH)
        {
            string _result = "",exec = "", AppName = "";
            try
            {
                if (!string.IsNullOrEmpty(SERV_PATH) && !string.IsNullOrEmpty(SERV_NAME))
                {
                    exec = SERV_PATH.Replace('?', '\\');  AppName = SERV_NAME;
                }
                if (exec != "")
                {
                    StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("../fine.bat"), false);
                    sw.Write(exec);
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                    using (var p = new Process())
                    {
                        p.StartInfo.FileName = HttpContext.Current.Server.MapPath("../fine.bat");
                        p.StartInfo.WorkingDirectory = Path.GetDirectoryName(HttpContext.Current.Server.MapPath("../fine.bat"));
                        p.StartInfo.UseShellExecute = false;
                        bool isStart = p.Start();
                        p.WaitForExit();
                        p.Dispose();                        
                        _result = "1";
                    }
                }
            }
            catch(Exception ex)
            { throw; }
            return _result;
        }

        public static int CheckInterval(string date1,string date2,int interval)
        {
            int nIntrvlstat = 0;
            try
            {
                DateTime dtval1 = Convert.ToDateTime(date1); DateTime dtval2 = Convert.ToDateTime(date2);
               int nDaydiff = Math.Abs(dtval2.Subtract(dtval1).Days);
               nIntrvlstat = (nDaydiff > interval) ? 1 : 0;
            }
            catch(Exception ex) {}
            return nIntrvlstat;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GenerateAdaptorLicense_File(string ADAPTID)
        {
            string json = "";
            try
            {
                json = Common.Default.EncryptURL(ADAPTID);
            }
            catch (Exception ex)
            { throw; }
            return json;
        }
     
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveAdaptorLicensekey(string EncyptLicense, string ADDRCODE)
        {
            string json = "", filename = "";
            try
            {
                filename = "LICENSE_" + ADDRCODE + ".dat";
                string cLicensefolder = HttpContext.Current.Server.MapPath("../License/");
                if (!Directory.Exists(cLicensefolder)) { Directory.CreateDirectory(cLicensefolder); } string filepath = cLicensefolder + filename;
                File.WriteAllText(filepath, EncyptLicense);
                System.IO.FileInfo file = new System.IO.FileInfo(filepath);
                if (file.Exists)
                {
                    json = "1";
                }
            }
            catch (Exception ex)
            { throw; }
            return json;
        }


        private static void DeleteFiles(string DestFile)
        {
            DirectoryInfo di = new DirectoryInfo(DestFile);
            foreach (FileInfo file in di.GetFiles())  {file.Delete();  }
            foreach (DirectoryInfo dir in di.GetDirectories())  { dir.Delete(true);  }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CheckLesAdaptor(string ADAPTID)
        {
            string json = "";
            try
            {
                SupplierRoutines _routine = new SupplierRoutines();
                json = _routine.CheckLeSAdaptor(ADAPTID).ToString();
            }
            catch (Exception ex)
            { throw; }
            return json;
        }

        #region commeneted
        // [WebMethod]
        // [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        // public static string GenerateAdaptorLicense_File(string ADAPTID, string ADDRCODE)
        // {
        //     string json = "",filename = "";
        //     try
        //     {
        //         filename = "LICENSE_" + ADDRCODE + ".dat";
        //         string cLicensefolder = HttpContext.Current.Server.MapPath("../License/");
        //         if (!Directory.Exists(cLicensefolder)) { Directory.CreateDirectory(cLicensefolder); } DeleteFiles(cLicensefolder); string filepath = cLicensefolder + filename;
        //         string destfile = HttpContext.Current.Server.MapPath("../Downloads/"); DeleteFiles(destfile); string filefullpath = destfile + filename;
        //         string cEncryptlicense = Common.Default.EncryptURL(ADAPTID); File.WriteAllText(filepath, cEncryptlicense);
        //         System.IO.FileInfo file = new System.IO.FileInfo(filepath);
        //         if (file.Exists)
        //         {
        //             json = file.FullName + "|" + file.Name;
        //             File.Copy(filepath, filefullpath, true);
        //         }
        //     }
        //     catch (Exception ex)
        //     { throw; }
        //     return json;
        //}
        //var filefullpath = res.split('|')[0]; var filename = res.split('|')[1];
        //if (filename != undefined && filename != '') {
        //    var cVirtualPath = "../Downloads/"; var win = window.open(cVirtualPath + filename, '_blank'); win.focus();
        //    toastr.success("Lighthouse eSolutionsGenerate Pte. Ltd", "License Download success");
        #endregion
    }
}