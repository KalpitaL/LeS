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
    public partial class XLS_Buyer_Config : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetXLSBuyerConfigGrid()
        {
            string json = "";           
            DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.Get_XLS_Mapping();
            //DataSet dsClone = _ds.Copy();
            //dsClone.Tables[0].Columns.Add("SampleFile");
            //foreach (DataRow row in dsClone.Tables[0].Rows)
            //{
            //    foreach (DataColumn column in dsClone.Tables[0].Columns)
            //    {
            //        string value = Convert.ToString(row["GROUP_CODE"]);
            //        row.SetField("SampleFile", GetReferenceFiles(value));
            //    }
            //}
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetXLSGroupsOnly()
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.GetExcelGroupsOnly();

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
        public static string UpdateXLSMapping(List<string> slXLSMapdet)
        {
            string json = "";
            string cXLS_BUYER_MAPID = "", cGROUP_ID = "", cMAP_CELL1 = "", cMAP_CELL1_VAL1 = "", cMAP_CELL1_VAL2 = "", cMAP_CELL2 = "",
                cMAP_CELL2_VAL = "", cMAP_CELL_NODISC = "", cMAP_CELL_NODISC_VAL = "", cDOC_TYPE="";
            int nGroupID = 0, nXLS_BUYER_MAPID = 0;
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
                    Dictionary<string, string> _dictexp = _def.ConvertListToDictionary(slvalues);

                    foreach (string key in _dictexp.Keys)
                    {
                        switch (key.ToUpper())
                        {
                            case "XLS_BUYER_MAPID": cXLS_BUYER_MAPID = _dictexp[key];
                                nXLS_BUYER_MAPID = Convert.ToInt32(cXLS_BUYER_MAPID);
                                break;
                            case "GROUP_ID": cGROUP_ID = _dictexp[key];
                                nGroupID = (!string.IsNullOrEmpty(cGROUP_ID))?Convert.ToInt32(cGROUP_ID):0;
                                break;
                            case "MAP_CELL1": cMAP_CELL1 = _dictexp[key];
                                break;
                            case "MAP_CELL1_VAL1": cMAP_CELL1_VAL1 = _dictexp[key];
                                break;
                            case "MAP_CELL1_VAL2": cMAP_CELL1_VAL2 = _dictexp[key];
                                break;
                            case "MAP_CELL2": cMAP_CELL2 = _dictexp[key];
                                break;
                            case "MAP_CELL2_VAL": cMAP_CELL2_VAL = _dictexp[key];
                                break;
                            case "MAP_CELL_NODISC": cMAP_CELL_NODISC = _dictexp[key];
                                break;
                            case "MAP_CELL_NODISC_VAL": cMAP_CELL_NODISC_VAL = _dictexp[key];
                                break;
                            case "DOC_TYPE": cDOC_TYPE = _dictexp[key];
                                break;
                        }
                    }
                    _Routine.Update_XLS_Mapping(nXLS_BUYER_MAPID, nGroupID, cMAP_CELL1, cMAP_CELL1_VAL1, cMAP_CELL1_VAL2, cMAP_CELL2, cMAP_CELL2_VAL,
                       cMAP_CELL_NODISC, cMAP_CELL_NODISC_VAL, cDOC_TYPE,Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                    json = "1";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DeleteXLSMapping(string XLS_BUYER_MAPID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines(); ;
            List<string> slvalues = new List<string>();
            try
            {
                if (!string.IsNullOrEmpty(XLS_BUYER_MAPID) && Convert.ToInt32(XLS_BUYER_MAPID) > 0)
                {
                    _Routine.RemoveXLSMapping(Convert.ToInt32(XLS_BUYER_MAPID), Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                    json = "1";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CopyXLSMapping(List<string> slXLSMapdet)
        {
            string json = "";
            string cBUYERID = "", cSUPPLIERID = "",cGROUP_ID = "", cMAP_CELL1 = "", cMAP_CELL1_VAL1 = "", cMAP_CELL1_VAL2 = "", cMAP_CELL2 = "",
                cMAP_CELL2_VAL = "", cMAP_CELL_NODISC = "", cMAP_CELL_NODISC_VAL = "", cDOC_TYPE = "";            
            int nGroupID = 0, nBuyerID = 0, nSupplierID = 0;
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
                            case "MAP_CELL1": cMAP_CELL1 = _dictexp[key];
                                break;
                            case "MAP_CELL1_VAL1": cMAP_CELL1_VAL1 = _dictexp[key];
                                break;
                            case "MAP_CELL1_VAL2": cMAP_CELL1_VAL2 = _dictexp[key];
                                break;
                            case "MAP_CELL2": cMAP_CELL2 = _dictexp[key];
                                break;
                            case "MAP_CELL2_VAL": cMAP_CELL2_VAL = _dictexp[key];
                                break;
                            case "MAP_CELL_NODISC": cMAP_CELL_NODISC = _dictexp[key];
                                break;
                            case "MAP_CELL_NODISC_VAL": cMAP_CELL_NODISC_VAL = _dictexp[key];
                                break;
                            case "DOC_TYPE": cDOC_TYPE = _dictexp[key];
                                break;
                        }
                    }
                    _Routine.Add_XLS_Mapping(nBuyerID, nSupplierID, nGroupID, cMAP_CELL1, cMAP_CELL1_VAL1, cMAP_CELL1_VAL2, cMAP_CELL2,
                        cMAP_CELL2_VAL, cMAP_CELL_NODISC, cMAP_CELL_NODISC_VAL, Convert.ToString(HttpContext.Current.Session["UserHostServer"]), cDOC_TYPE);
                    json = "1";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
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
        public static string DownloadFormat(string EXCEL_MAP_ID, string GROUPID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                string filename = "";
                string destfile = HttpContext.Current.Server.MapPath("../Downloads/");

                DeleteFiles(destfile);                
                string SessionID = HttpContext.Current.Session.SessionID;

                string templateFile = HttpContext.Current.Server.MapPath("../Templates/XLS_Mapping.xls");
                filename = _Routine.SetXLSMapping(Convert.ToInt32(EXCEL_MAP_ID), templateFile, SessionID);

                //string cRefFilePath = Convert.ToString(ConfigurationManager.AppSettings["REF_FILEPATH"]) + "\\EXCEL\\" + GROUPNAME;
                //string cSiteUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
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
                _result = _Routine.Update_XLS_Group_Mapping(FILEUPLOADPATH, Convert.ToString(HttpContext.Current.Session["UserHostServer"]), REFFILE_UPLOADPATH);
            }
            catch (Exception ex)
            { throw; }
            return _result;
        }

     

    }
}