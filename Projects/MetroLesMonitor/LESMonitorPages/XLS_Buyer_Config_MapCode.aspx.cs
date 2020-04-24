﻿using AjaxControlToolkit;
using System;
using System.Collections.Generic;
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
    public partial class XLS_Buyer_Config_MapCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetXLSBuyerConfigGrid()
        {
            string json = "";           
            SupplierRoutines _Routine = new SupplierRoutines();
            DataSet _ds = _Routine.Get_XLS_Mapping_MapCode();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetXLSBuyersOnly()
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.GetAllBuyers_Format("XLS");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetXLSDoctypes()
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.Get_XLS_Doctypes();
            //_ds.Tables[0].DefaultView.Sort = "DOCTYPE DESC";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetXLSBuyerConfig_Addressid(string ADDRESSID, string ADDRTYPE)
        {
            string json = "";
            DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.Get_XLS_Mapping_AddressId(ADDRESSID, ADDRTYPE);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveXLSMapping(List<string> slXLSMapdet)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slXLSMapdet);
                if (_lstdet != null && _lstdet.Count > 0)
                {
                    slvalues.Clear();
                    for (int i = 0; i < _lstdet.Count; i++) { string lstkey = _lstdet[i][0]; slvalues.Add(lstkey); }
                    Dictionary<string, string> _dictexp = _def.ConvertListToDictionary(slvalues);
                    _Routine.SaveXLSMapping_New(_dictexp, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                    json = "1";
                }
            }
            catch (Exception ex) { json = ""; }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DeleteXLSMapping(List<string> slMAPID)
        {
            string json = "";
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            SupplierRoutines _Routine = new SupplierRoutines(); ;
            List<string> slvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slMAPID);
                if (_lstdet != null && _lstdet.Count > 0)
                {
                    slvalues.Clear();
                    for (int i = 0; i < _lstdet.Count; i++)
                    {
                        _Routine.RemoveXLSMapping_New(Convert.ToInt32(_lstdet[i][0]), Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                    }
                    json = "1";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CopyXLSMapping(string BUYERID, List<string> slXLSMapdet)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slXLSMapdet);
                if (_lstdet != null && _lstdet.Count > 0)
                {
                    slvalues.Clear();
                    for (int i = 0; i < _lstdet.Count; i++)
                    {
                        string lstkey = _lstdet[i][0];
                        slvalues.Add(lstkey);
                    }
                    _Routine.CopyXLSMapping_New(Convert.ToInt32(BUYERID), slvalues, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                    json = "1";
                }
            }
            catch
            {
                json = "";
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DownloadXLSFormat(string MAPID)
        {
            string json = "", filename = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                string destfile = HttpContext.Current.Server.MapPath("../Downloads/"); DeleteFiles(destfile);
                string SessionID = HttpContext.Current.Session.SessionID;
                string templateFile = HttpContext.Current.Server.MapPath("../Templates/XLS_Mapping.xls");
                filename = _Routine.SetXLSMapping_New(Convert.ToInt32(MAPID), templateFile, SessionID);
                System.IO.FileInfo file = new System.IO.FileInfo(filename);
                if (file.Exists) { json = file.FullName + "|" + file.Name; file.CopyTo(destfile + file.Name); }
            }
            catch (Exception ex) { json = ""; }
            return json;
        }

        private static void DeleteFiles(string DestFile)
        {
            DirectoryInfo di = new DirectoryInfo(DestFile);
            foreach (FileInfo file in di.GetFiles()) { file.Delete(); }
            foreach (DirectoryInfo dir in di.GetDirectories()) { dir.Delete(true); }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SetUploadPath(string FILENAME)
        {
            string uploadedFile = System.Configuration.ConfigurationManager.AppSettings["UPLOAD_ATTACHMENT"] + "\\" + HttpContext.Current.Session.SessionID + "\\";// +FILENAME;
            if (!Directory.Exists(Path.GetDirectoryName(uploadedFile))) Directory.CreateDirectory(Path.GetDirectoryName(uploadedFile));
            HttpContext.Current.Session["Upload_Path"] = Convert.ToString(uploadedFile);
            return uploadedFile;
        }

        public void File_UploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                AjaxFileUpload uControl = (AjaxFileUpload)sender;
                string FILENAME = e.FileName;
                if (uControl.IsInFileUploadPostBack)
                {
                    string uploadedFile = System.Configuration.ConfigurationManager.AppSettings["UPLOAD_ATTACHMENT"] + "\\" + HttpContext.Current.Session.SessionID;
                    if (!Directory.Exists(uploadedFile))
                    {
                        Directory.CreateDirectory(uploadedFile);
                    }
                    string FileUploadPath = uploadedFile + "\\" + FILENAME;
                    HttpContext.Current.Session["FileUploadPath"] = FileUploadPath;
                    uControl.SaveAs(FileUploadPath);
                }
            }
            catch (Exception ex)
            { }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string UploadFileMapping()
        {
            string json = "";
            string FILEUPLOADPATH = Convert.ToString(HttpContext.Current.Session["FileUploadPath"]);
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                json = _Routine.UploadXLSMapping_New(FILEUPLOADPATH, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
            }
            catch (Exception ex)
            { throw; }
            return json;
        }
    }
}