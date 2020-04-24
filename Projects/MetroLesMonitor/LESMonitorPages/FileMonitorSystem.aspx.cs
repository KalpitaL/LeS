using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
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
    public partial class FileMonitorSystem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetServerStatus()
        {
            string json = "";
            try
            {
                string _Path = ConfigurationManager.AppSettings["SERVER_LOG-FILE_DETAIL_PATH"];
                Dictionary<string, string> slservdet = new Dictionary<string, string>();
                slservdet.Clear();
                if (!Directory.Exists(_Path)) { Directory.CreateDirectory(_Path); }
                string[] _Serverfiles = Directory.GetFiles(_Path, "*.xml");
                foreach (string _str in _Serverfiles)
                {
                    string timeStamp = "", status = "";
                    XmlDocument doc = new XmlDocument();
                    doc.Load(_str);
                    XmlNodeList aNodesTime = doc.SelectNodes("//TimeStamp");
                    foreach (XmlNode _timeStamp in aNodesTime)
                    {
                        timeStamp = _timeStamp.InnerText;
                        if (!string.IsNullOrEmpty(timeStamp.Trim()))
                        {
                            DateTime currentTime = DateTime.Now;
                            DateTime dt = DateTime.ParseExact(timeStamp, "dd-MM-yyyy HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None);
                            status = (currentTime.Subtract(dt) >= TimeSpan.FromMinutes(30)) ? "DEACTIVE" : "ACTIVE";
                        }
                    }
                    slservdet.Add(Path.GetFileNameWithoutExtension(_str), status);
                }
                json = JsonConvert.SerializeObject(slservdet);
            }
            catch (Exception)
            {
                json = "";
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetServerDetails(string ServerName)
        {
            string json = "";
            try
            {
                string _Path = ConfigurationManager.AppSettings["SERVER_LOG-FILE_DETAIL_PATH"];
                DataSet ds = new DataSet();
                DataTable _dt = new DataTable(); _dt.Clear();
                _dt.Columns.Add("TIME_STAMP", typeof(string));
                _dt.Columns.Add("ID", typeof(string));
                _dt.Columns.Add("TITLE", typeof(string));
                _dt.Columns.Add("FILE_PATH", typeof(string));
                _dt.Columns.Add("COUNT", typeof(string));
                _dt.Columns.Add("DESC", typeof(string));
                _dt.Columns.Add("FILE_LIST", typeof(string));
                if (!Directory.Exists(_Path))  {  Directory.CreateDirectory(_Path);  }
                string[] _Serverfiles = Directory.GetFiles(_Path, ServerName + ".xml");
                foreach (string _str in _Serverfiles)
                {
                    string timeStamp = "", ids = "", count = "", title = "", filePath = "", _desc = "", _ListData = "";
                    XmlDocument doc = new XmlDocument();
                    doc.Load(_str);
                    XmlNodeList aNodesTime = doc.SelectNodes("//TimeStamp");
                    foreach (XmlNode _timeStamp in aNodesTime)
                    {
                        timeStamp = _timeStamp.InnerText;
                    }
                    XmlNodeList aNodesFileInfo = doc.SelectNodes("//FileInfo");
                    int nCount = 1;
                    foreach (XmlNode _FileInf in aNodesFileInfo)
                    {
                        ids = nCount.ToString();

                        foreach (XmlAttribute _attr in _FileInf.Attributes)
                        {
                            if (_attr != null)
                            {
                                switch (_attr.Name)
                                {
                                    case "Count": count = _attr.Value;
                                        break;
                                    case "Path": filePath = _attr.Value;
                                        break;
                                    case "Title": title = _attr.Value;
                                        break;
                                    case "Description": _desc = _attr.Value;
                                        break;
                                }
                            }
                        }
                        _ListData = "";
                        XmlNodeList aNodesFileList = _FileInf.SelectNodes("FileList//FileData");
                        foreach (XmlNode _FileData in aNodesFileList)
                        {
                            string _fname = "", _timestamp = "";
                            foreach (XmlAttribute _attr in _FileData.Attributes)
                            {
                                if (_attr != null)
                                {
                                    switch (_attr.Name)
                                    {
                                        case "FileName": _fname = _attr.Value;
                                            break;
                                        case "DateTime": _timestamp = _attr.Value;
                                            break;
                                    }
                                }
                            }
                            _ListData += _fname + "#" + _timestamp + "|";
                        }
                        _ListData = _ListData.TrimEnd('|');
                        _dt.Rows.Add(timeStamp, ids, title, filePath, count, _desc, _ListData);
                        nCount++;
                    }
                }
                ds.Tables.Add(_dt);
                XmlDocument docdet = new XmlDocument();
                docdet.LoadXml(ds.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(docdet);
            }
            catch (Exception)
            {
                json="";
            }
            return json;
        }

    }
}