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
using AjaxControlToolkit;
using System.Configuration;

namespace MetroLesMonitor.LESMonitorPages
{
    public partial class File_Mapping : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillGroups(string FILETYPE)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            DataSet ds = new DataSet();
            ds.Clear();
            DataSet dsClone = new DataSet();
            dsClone.Clear();
            if (convert.ToString(FILETYPE).ToUpper() == "XLS")
            {
                ds = _Routine.GetAllExcelGroups();
            }
            else if (convert.ToString(FILETYPE).ToUpper() == "PDF")
            {
                ds = _Routine.GetPDFGroupsOnly();
            }
            else if (convert.ToString(FILETYPE).ToUpper() == "VOUCHER_PDF")
            {
                ds = _Routine.Get_INV_PDF_MAPCODE();
            }
            dsClone = ds.Copy();
            if (convert.ToString(FILETYPE).ToUpper() == "XLS" || convert.ToString(FILETYPE).ToUpper() == "PDF")
            {
                dsClone.Tables[0].Columns.Add("GROUP_MAPCODE");

                if (dsClone != null && dsClone.Tables.Count > 0 && dsClone.Tables[0].Rows.Count > 0)
                {
                    foreach (System.Data.DataRow dr in dsClone.Tables[0].Rows)
                    {
                        string Item = convert.ToString(dr["GROUP_CODE"]);
                        if (convert.ToString(FILETYPE).ToUpper() == "XLS")
                        {
                            string MapCode = convert.ToString(dr["XLS_MAP_CODE"]).Trim();
                            if (MapCode.Length > 0 && MapCode.ToUpper() != Item.ToUpper())
                            {
                                Item += " (" + MapCode + ")";
                                dr.SetField("GROUP_MAPCODE", Item);
                            }
                            else { dr.SetField("GROUP_MAPCODE", Item); }
                        }
                        else
                        {
                            dr.SetField("GROUP_MAPCODE", Item);
                        }
                    }

                    dsClone.AcceptChanges();
                }
            }
           
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(dsClone.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DownloadGroupFormat(string FILETYPE, string GROUPID, string GROUPNAME)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                string filename = "";
                string destfile = HttpContext.Current.Server.MapPath("../Downloads/");

                DeleteFiles(destfile);
                HttpContext.Current.Session["FILETYPE"] = FILETYPE;
                string SessionID = HttpContext.Current.Session.SessionID;

                string cRefFilePath = Convert.ToString(ConfigurationManager.AppSettings["REF_FILEPATH"])+"\\"+FILETYPE+"\\"+GROUPNAME;
                string cSiteUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

                if (convert.ToString(FILETYPE).ToUpper() == "PDF")
                {
                    string templateFile = HttpContext.Current.Server.MapPath("../Templates/PDF_MAPPING_TEMPLATE.xls");
                    //filename = _Routine.SetPDFMapping(Convert.ToInt32(GROUPID), Convert.ToString(GROUPNAME), templateFile, SessionID);
                    filename = _Routine.SetPDFMapping(Convert.ToInt32(GROUPID), templateFile, SessionID);
                }
                else if (convert.ToString(FILETYPE).ToUpper() == "XLS")
                {
                    string templateFile = HttpContext.Current.Server.MapPath("../Templates/XLS_Mapping.xls");
                    //filename = _Routine.SetXLSMapping(Convert.ToInt32(GROUPID), Convert.ToString(GROUPNAME), templateFile, SessionID);
                    filename = _Routine.SetXLSMapping(Convert.ToInt32(GROUPID), templateFile, SessionID);
                }
                else if (convert.ToString(FILETYPE).ToUpper() == "VOUCHER_PDF")
                {
                    string templateFile = HttpContext.Current.Server.MapPath("../Templates/VOUCHER_PDF_MAPPING_TEMPLATE.xls");
                    //filename = _Routine.SetVoucherPdfMapping(Convert.ToInt32(GROUPID), Convert.ToString(GROUPNAME), templateFile, SessionID);
                    filename = _Routine.SetVoucherPdfMapping(Convert.ToInt32(GROUPID),  templateFile, SessionID);
                }
                System.IO.FileInfo file = new System.IO.FileInfo(filename);
                if (file.Exists)
                {
                    string cRef_Filedet = GetReferenceFiles(cRefFilePath, convert.ToString(FILETYPE)) + "$$" + cSiteUrl;
                    json = file.FullName + "|" + file.Name + "#" + cRef_Filedet;
                    file.CopyTo(destfile + file.Name);                   
                }
            }
            catch (Exception ex)
            { json = ""; }          
            return json;
        }

        private static string GetReferenceFiles(string cRefFilePath,string extn)
        {
            string cFiles = ""; string destfile = HttpContext.Current.Server.MapPath("../Downloads/");
            DirectoryInfo d = new DirectoryInfo(cRefFilePath);
            FileInfo[] f = d.GetFiles("*." + extn);
            if (f != null && f.Length > 0)
            {
                foreach (FileInfo file in f)
                {
                    cFiles += file.Name + ","; file.CopyTo(destfile + file.Name);     
                }
            }
            return cFiles.TrimEnd(',');
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
        public static string UploadFileMapping(string FILETYPE)
        {
            string  _result = "";
            string FILEUPLOADPATH = Convert.ToString(HttpContext.Current.Session["Mapp_FileUploadPath"]);
            string REFFILE_UPLOADPATH = Convert.ToString(HttpContext.Current.Session["Ref_FileUploadPath"]);   
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                if (!string.IsNullOrEmpty(FILETYPE))
                {
                    if (Convert.ToString(FILETYPE).Trim().ToUpper() == "XLS")
                    {
                        _result = _Routine.Update_XLS_Group_Mapping(FILEUPLOADPATH, Convert.ToString(HttpContext.Current.Session["UserHostServer"]), REFFILE_UPLOADPATH);
                    }
                    else if (Convert.ToString(FILETYPE).Trim().ToUpper() == "PDF")
                    {
                        _result = _Routine.Update_PDF_Mapping(FILEUPLOADPATH, Convert.ToString(HttpContext.Current.Session["UserHostServer"]), REFFILE_UPLOADPATH);
                    }
                    else if (Convert.ToString(FILETYPE).Trim().ToUpper() == "VOUCHER_PDF")
                    {
                        _result = _Routine.Update_Voucher_PDF_Mapping(FILEUPLOADPATH, Convert.ToString(HttpContext.Current.Session["UserHostServer"]), REFFILE_UPLOADPATH);
                    }
                }
            }
            catch (Exception ex)
            { throw; }
            return _result;
        }

    }
}