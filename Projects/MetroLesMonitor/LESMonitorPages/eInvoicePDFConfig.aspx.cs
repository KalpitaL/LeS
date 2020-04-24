using AjaxControlToolkit;
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
    public partial class eInvoicePDFConfig : System.Web.UI.Page
    {
        public static string eInvoiceURL, eInvoiceExportDefault, AddressId;
     
        protected void Page_Load(object sender, EventArgs e)
        {
            eInvoiceURL = ConfigurationManager.AppSettings["EINVOICE_WEB_SERVICE"];
            eInvoiceExportDefault = ConfigurationManager.AppSettings["INVOICE_EXPORT_PATH_DEFAULT"];
            AddressId = ConfigurationManager.AppSettings["ADMINID"].ToString();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GeteInvoicePDFConfigGrid()
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = _Routine.Get_INV_PDF_Mapping();
            DataSet dsClone = _ds.Copy();
            dsClone.Tables[0].Columns.Add("SampleFile");
            foreach (DataRow row in dsClone.Tables[0].Rows)
            {
                foreach (DataColumn column in dsClone.Tables[0].Columns)
                {
                    string value = Convert.ToString(row["INV_MAP_CODE"]);
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
        public static string GetInvMapCodeOnly()
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = _Routine.Get_INV_PDF_MAPCODE();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetSuppliersOnly()
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds =_Routine.Get_eInvoice_Supplier(eInvoiceURL);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }
    
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetInvoiceMapCode_SupplierId(string SUPPLIERID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            json = _Routine.GetInvoiceMapCode_Supplierid(Convert.ToInt32(SUPPLIERID));         
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Delete_eInvoicePDFMapping(string MAPID, string SUPPLIERID, string INVOICEMAP_CODE)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines(); 
            try
            {
                if (!string.IsNullOrEmpty(MAPID) && Convert.ToInt32(MAPID) > 0)
                {
                    _Routine.Delete_Invoice_PDF_Config(Convert.ToInt32(MAPID), Convert.ToInt32(SUPPLIERID), INVOICEMAP_CODE, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
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
        public static string Save_eInvoicePDFMapping(List<string> sleInvMappdet)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines(); 
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(sleInvMappdet);
                if (_lstdet != null && _lstdet.Count > 0)
                {
                    slvalues.Clear();
                    for (int i = 0; i < _lstdet.Count; i++){ string lstkey = _lstdet[i][0]; slvalues.Add(lstkey);  }
                    Dictionary<string, string> _dictexp = _def.ConvertListToDictionary(slvalues);
                   _Routine.Save_Invoice_PDF_Config(_dictexp, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
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
        public static string CheckExistingeInvoice_Mapping(string MAP_CODE)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            bool IsExist = _Routine.Check_Inv_MapCode(MAP_CODE);
            if (IsExist) { json = "Map Code already exists"; }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DownloadFormat(string INV_PDF_MAPID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                string filename = "";
                string destfile = HttpContext.Current.Server.MapPath("../Downloads/");

                DeleteFiles(destfile);
                string SessionID = HttpContext.Current.Session.SessionID;
                string templateFile = HttpContext.Current.Server.MapPath("../Templates/VOUCHER_PDF_MAPPING_TEMPLATE.xls");
                filename = _Routine.SetVoucherPdfMapping(Convert.ToInt32(INV_PDF_MAPID), templateFile, SessionID);
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
                _result = _Routine.Update_Voucher_PDF_Mapping(FILEUPLOADPATH, Convert.ToString(HttpContext.Current.Session["UserHostServer"]), REFFILE_UPLOADPATH);
            }
            catch (Exception ex)
            { throw; }
            return _result;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetReferenceFiles(string INV_MAP_CODE)
        {
            string _result = "";
            try
            {
                string cDirPath = Convert.ToString(ConfigurationManager.AppSettings["REF_FILEPATH"]) + "\\VOUCHER_PDF\\" + INV_MAP_CODE;
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