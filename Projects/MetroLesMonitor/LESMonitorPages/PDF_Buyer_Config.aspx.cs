﻿using MetroLesMonitor.Dal;
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
    public partial class PDF_Buyer_Config : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetPDFBuyerConfigGrid()
        {
            string json = "";
            DataAccess _dataAccess = new DataAccess();            
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.Get_PDF_Mapping();
            DataSet dsClone = _ds.Copy();
            dsClone.Tables[0].Columns.Add("SampleFile");
            foreach (DataRow row in dsClone.Tables[0].Rows)
            {
                foreach (DataColumn column in dsClone.Tables[0].Columns)
                {
                    string value = Convert.ToString(row["GROUP_CODE"]);
                    row.SetField("SampleFile", GetReferenceFiles(value));
                }
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(dsClone.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetPDFGroupsOnly()
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.GetPDFGroupsOnly();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetPDFDoctypes()
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.Get_PDF_Doctypes();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string UpdatePDFMapping(List<string> slPDFMapdet)
        {
            string json = "";
            string cMAPID = "", cGROUP_ID = "", cDOC_TYPE = "", cMAP_1 = "", cMAP_VAL1 = "", cMAP_2 = "", cMAP_VAL2 = "", cMAP_3 = "", cMAP_VAL3 = "";
            int nGroupID = 0,nMapID=0;
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();            
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slPDFMapdet);

                if (_lstdet != null && _lstdet.Count > 0)
                {
                    slvalues.Clear();
                    for (int i = 0; i < _lstdet.Count; i++)
                    {
                        string lstkey = _lstdet[i][0];
                        slvalues.Add(lstkey);
                    }
                    Dictionary<string, string> _dictexp = _def.ConvertListToDictionary(slvalues);

                    foreach (string key in _dictexp.Keys)
                    {
                        switch (key.ToUpper())
                        {
                            case "MAPID": cMAPID = _dictexp[key];
                                nMapID = Convert.ToInt32(cMAPID);
                              break;
                            case "GROUP_ID": cGROUP_ID = _dictexp[key];
                              nGroupID = (!string.IsNullOrEmpty(cGROUP_ID))?Convert.ToInt32(cGROUP_ID):0;
                              break;
                            case "DOC_TYPE": cDOC_TYPE = _dictexp[key];
                              break;
                            case "MAPPING_1": cMAP_1 = _dictexp[key];
                              break;
                            case "MAPPING_1_VALUE": cMAP_VAL1 = _dictexp[key];
                              break;
                            case "MAPPING_2": cMAP_2 = _dictexp[key];
                              break;
                            case "MAPPING_2_VALUE": cMAP_VAL2 = _dictexp[key];
                              break;
                            case "MAPPING_3": cMAP_3 = _dictexp[key];
                              break;
                            case "MAPPING_3_VALUE": cMAP_VAL3 = _dictexp[key];
                              break;
                        }
                    }
                    _Routine.Update_PDF_Mapping(nMapID, nGroupID, cDOC_TYPE, cMAP_1, cMAP_VAL1, cMAP_2, cMAP_VAL2, cMAP_3, cMAP_VAL3, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));          
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
        public static string DeletePDFMapping(string MAPID)
        {
            string json = "";
         
            SupplierRoutines _Routine = new SupplierRoutines();;
            List<string> slvalues = new List<string>();
            try
            {
                if (!string.IsNullOrEmpty(MAPID) && Convert.ToInt32(MAPID) > 0)
                {
                    _Routine.RemovePDFMapping(Convert.ToInt32(MAPID), Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
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
        public static string CopyPDFMapping(List<string> slPDFMapdet)
        {
            string json = "";
            string cBUYERID = "", cSUPPLIERID = "", cGROUP_ID = "", cDOC_TYPE = "", cMAP_1 = "", cMAP_VAL1 = "", cMAP_2 = "", cMAP_VAL2 = "", cMAP_3 = "", cMAP_VAL3 = "";
            int nGroupID = 0, nBuyerID = 0, nSupplierID = 0;
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slPDFMapdet);

                if (_lstdet != null && _lstdet.Count > 0)
                {
                    slvalues.Clear();
                    for (int i = 0; i < _lstdet.Count; i++)
                    {
                        string lstkey = _lstdet[i][0];
                        slvalues.Add(lstkey);
                    }
                    Dictionary<string, string> _dictexp = _def.ConvertListToDictionary(slvalues);
                    foreach (string key in _dictexp.Keys)
                    {
                        switch (key.ToUpper())
                        {
                            case "COPY_BUYERID": cBUYERID = _dictexp[key];
                                nBuyerID = Convert.ToInt32(cBUYERID);
                                break;
                            case "COPY_SUPPLIERID": cSUPPLIERID = _dictexp[key];
                                nSupplierID = Convert.ToInt32(cSUPPLIERID);
                                break;
                            case "GROUP_ID": cGROUP_ID = _dictexp[key];
                                nGroupID = Convert.ToInt32(cGROUP_ID);
                                break;
                            case "DOC_TYPE": cDOC_TYPE = _dictexp[key];
                                break;
                            case "MAPPING_1": cMAP_1 = _dictexp[key];
                                break;
                            case "MAPPING_1_VALUE": cMAP_VAL1 = _dictexp[key];
                                break;
                            case "MAPPING_2": cMAP_2 = _dictexp[key];
                                break;
                            case "MAPPING_2_VALUE": cMAP_VAL2 = _dictexp[key];
                                break;
                            case "MAPPING_3": cMAP_3 = _dictexp[key];
                                break;
                            case "MAPPING_3_VALUE": cMAP_VAL3 = _dictexp[key];
                                break;
                        }
                    }
                    _Routine.Add_PDF_Mapping(nBuyerID, nSupplierID, nGroupID, cDOC_TYPE, cMAP_1, cMAP_VAL1, cMAP_2, cMAP_VAL2, cMAP_3, cMAP_VAL3, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
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
        public static string GetPDFBuyerConfig_Addressid(string ADDRESSID, string ADDRTYPE)
        {
            string json = "";
            DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.Get_PDF_Mapping_AddressId(ADDRESSID, ADDRTYPE);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DownloadFormat( string GROUPID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                string filename = "";
                string destfile = HttpContext.Current.Server.MapPath("../Downloads/");

                DeleteFiles(destfile);
                string SessionID = HttpContext.Current.Session.SessionID;
                string templateFile = HttpContext.Current.Server.MapPath("../Templates/PDF_MAPPING_TEMPLATE.xls");
                filename = _Routine.SetPDFMapping(Convert.ToInt32(GROUPID), templateFile, SessionID);
                System.IO.FileInfo file = new System.IO.FileInfo(filename);
                if (file.Exists)
                {
                    json = file.FullName + "|" + file.Name;
                    file.CopyTo(destfile + file.Name);
                }
            }
            catch (Exception ex)
            { json = ""; }
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

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SetUploadPath(string FILENAME)
        {
            string uploadedFile = System.Configuration.ConfigurationManager.AppSettings["UPLOAD_ATTACHMENT"] + "\\" + HttpContext.Current.Session.SessionID + "\\";// +FILENAME;
            if (!Directory.Exists(Path.GetDirectoryName(uploadedFile))) Directory.CreateDirectory(Path.GetDirectoryName(uploadedFile));
            HttpContext.Current.Session["Upload_Path"] = Convert.ToString(uploadedFile);
            return uploadedFile;
        }

        public void MappFile_UploadComplete(object sender, EventArgs e)
        {
            string filename = System.IO.Path.GetFileName(FileUploadMapp.FileName);
            string uploadedFile = System.Configuration.ConfigurationManager.AppSettings["UPLOAD_ATTACHMENT"] + "\\" + HttpContext.Current.Session.SessionID;
            if (!Directory.Exists(uploadedFile))
            {
                Directory.CreateDirectory(uploadedFile);
            }
            string FileUploadPath = uploadedFile + "\\" + filename;
            HttpContext.Current.Session["Mapp_FileUploadPath"] = FileUploadPath;
            FileUploadMapp.SaveAs(FileUploadPath);
        }

        public void RefFile_UploadComplete(object sender, EventArgs e)
        {
            string filename = System.IO.Path.GetFileName(FileUploadRef.FileName);
            string uploadedFile = System.Configuration.ConfigurationManager.AppSettings["UPLOAD_REF_FILEPATH"] + "\\" + HttpContext.Current.Session.SessionID;
            if (!Directory.Exists(uploadedFile))
            {
                Directory.CreateDirectory(uploadedFile);
            }
            string FileUploadPath = uploadedFile + "\\" + filename;
            HttpContext.Current.Session["Ref_FileUploadPath"] = FileUploadPath;
            FileUploadRef.SaveAs(FileUploadPath);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string UploadFileMapping()
        {
            string _result = "";
            string FILEUPLOADPATH = Convert.ToString(HttpContext.Current.Session["Mapp_FileUploadPath"]);
            string REFFILE_UPLOADPATH = Convert.ToString(HttpContext.Current.Session["Ref_FileUploadPath"]);
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                _result = _Routine.Update_PDF_Mapping(FILEUPLOADPATH, Convert.ToString(HttpContext.Current.Session["UserHostServer"]), REFFILE_UPLOADPATH);
            }
            catch (Exception ex)
            { throw; }
            return _result;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetReferenceFiles(string GROUP_CODE)
        {
            string _result = "";
            try
            {
                string cDirPath = Convert.ToString(ConfigurationManager.AppSettings["REF_FILEPATH"]) + "\\PDF\\" + GROUP_CODE;
                if (Directory.Exists(cDirPath))
                {
                    DirectoryInfo d = new DirectoryInfo(cDirPath);
                    FileInfo[] f = d.GetFiles("*.*");
                    if (f != null && f.Length > 0)
                    {
                        _result = Path.GetFileName(f[0].FullName);
                    }
                }
            }
            catch (Exception ex)
            { throw; }
            return _result;
        }

    }
}