using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace MetroLesMonitor.LESMonitorPages
{
    public partial class LogFileViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetLogFileDetailsGrid()
        {
            string json = "";
            string LOG_FILE_PATH = convert.ToString(ConfigurationManager.AppSettings["LOG_FILE_PATH"]);

            if (!string.IsNullOrEmpty(LOG_FILE_PATH))
            {
                string[] _pathDes = LOG_FILE_PATH.Split(';');
                DataSet _ds = new DataSet();
                DataTable dt = new DataTable();
                dt.Clear();
                dt.Columns.Add("LOG_ID");
                dt.Columns.Add("DESCRIPTION");
                dt.Columns.Add("PATH");
                dt.Columns.Add("MODULE");
                for (int i = 0; i < _pathDes.Length; i++)
                {
                    string[] _details = _pathDes[i].Split('|');

                    {
                        DataRow _rawData = dt.NewRow();
                        _rawData["LOG_ID"] = i + 1;
                        _rawData["DESCRIPTION"] = convert.ToString(_details[2]);
                        _rawData["PATH"] = convert.ToString(_details[1]);
                        _rawData["MODULE"] = convert.ToString(_details[0]);
                        dt.Rows.Add(_rawData);
                    }
                }
                _ds.Tables.Add(dt);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_ds.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetFileDetails(string PATH)
        {
            string json = "";
            string _cpath = PATH.Replace('?', '\\');
            if (!string.IsNullOrEmpty(_cpath))
            {
                DataSet _ds = new DataSet();
                DataTable dt = new DataTable();
                dt.Clear();
                dt.Columns.Add("LOG_FILE_ID");
                dt.Columns.Add("FILENAME");
                dt.Columns.Add("TIME_STAMP");

                DirectoryInfo dir = new DirectoryInfo(_cpath);

                FileInfo[] files = (dir.Exists) ? dir.GetFiles("*.*").OrderByDescending(p => p.CreationTime).ToArray() : null;
                if (files == null) { json = "Path not found : " + Environment.NewLine + _cpath; }
                else
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (i < 10)
                        {
                            FileInfo _details = (FileInfo)files[i];
                            if (_details != null)
                            {
                                DataRow _rawData = dt.NewRow();
                                _rawData["LOG_FILE_ID"] = i + 1;
                                _rawData["FILENAME"] = Convert.ToString(_details.Name);
                                _rawData["TIME_STAMP"] = Convert.ToString(_details.CreationTime.ToString("yy-MM-dd HH:mm:ss"));
                                dt.Rows.Add(_rawData);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    _ds.Tables.Add(dt);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(_ds.GetXml());
                    json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
                }
            }
            return json;
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DownloadFile(string PATH, string FILENAME, string MODULE)
        {
            string json = "";
            FileUpload _upd = new FileUpload();
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                string _path = string.Empty;
                string config_path = MODULE; // Module Name
                string SessionID = HttpContext.Current.Session.SessionID;
                string destfile = HttpContext.Current.Server.MapPath("../Downloads/");
                string updateDate = convert.ToString(DateTime.Now.ToString("yy-MM-dd HH:mm"));
                DateTime logDate = DateTime.MinValue;

                _path = PATH.Replace('?', '\\');
                string _FPath = "";
                string strFile = FILENAME;
                if (!string.IsNullOrEmpty(updateDate))
                {
                    try
                    {
                        logDate = Convert.ToDateTime(updateDate);
                        if (logDate != DateTime.MinValue)
                            _FPath = GetCorrectPath(_path + "\\" + logDate.Year + "\\" + logDate.ToString("MMM"), strFile);  // Search for Current Year and Month
                        else _FPath = GetCorrectPath(_path, strFile);
                    }
                    catch { _FPath = GetCorrectPath(_path, strFile); }
                }
                else _FPath = GetCorrectPath(_path, strFile); // Search in AuditDoc Path

                if (!File.Exists(_FPath + "\\" + strFile))
                    _FPath = GetCorrectPath(_path, strFile); // Search in AuditDoc Path

                if (!File.Exists(_FPath + "\\" + strFile))
                    if (!string.IsNullOrEmpty(_path))
                        if (_path.Contains('|'))
                            _FPath = SearchInDirectory(_path.Split('|')[0], strFile); // Search in module path
                        else _FPath = SearchInDirectory(_path, strFile); // Search in module path
                System.IO.FileInfo file = new System.IO.FileInfo(_FPath + "\\" + strFile);
                if (file.Exists)
                {
                    file.CopyTo(destfile + file.Name);
                }
                json = file.FullName + "|" + file.Name;
            }
            catch (Exception ex)
            {
                _Routine.SetLog(ex.StackTrace);
            }
            return json;
        }


        private static void DeleteFiles(string DestFile)
        {
            DirectoryInfo di = new DirectoryInfo(DestFile);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        protected static string SearchInDirectory(string _Path, string _File)
        {
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                string[] slFiles = Directory.GetFiles(_Path, _File, SearchOption.AllDirectories);
                string _return = "";

                if (slFiles.Length > 0)
                    _return = slFiles[0];

                if (File.Exists(_return))
                {
                    FileInfo fInfo = new FileInfo(_return);
                    _return = fInfo.DirectoryName;
                }
                return _return;
            }
            catch (Exception ex)
            {
                _Routine.SetLog(ex.StackTrace);
                throw ex;
            }
        }

        private static string GetCorrectPath(string _Path, string _File)
        {
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                string cReturn = "";
                string[] slPaths;

                slPaths = _Path.Split('|');
                foreach (string cPath in slPaths)
                {
                    FileInfo file = new FileInfo(cPath + "\\" + _File);
                    {
                        if (file.Exists)
                        {
                            cReturn = cPath;
                            break;
                        }
                    }
                }
                return cReturn;
            }
            catch (Exception ex)
            {
                _Routine.SetLog(ex.StackTrace);
                throw ex;
            }
        }

    }
}