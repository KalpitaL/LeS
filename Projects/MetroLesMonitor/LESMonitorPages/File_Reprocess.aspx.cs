using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace MetroLesMonitor.LESMonitorPages
{
    public partial class File_Reprocess : System.Web.UI.Page
    {
        public string cMTML_PATH, cLeSXML_PATH;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string LoadSuppliers()
        {
            string json = "";
            int AddressID = 0;
            System.Data.DataSet _ds = new DataSet();
            if (HttpContext.Current.Session["ADDRESSID"] != null && HttpContext.Current.Session["ADDRESSID"] != "")
            {
                AddressID = Convert.ToInt32(HttpContext.Current.Session["ADDRESSID"]);
            }
            if (AddressID > 0)
            {
                SupplierRoutines _Routine = new SupplierRoutines();
                _ds = _Routine.GetLinkedSuppliers(AddressID);
         
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_ds.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string LoadBuyers(string SUPPLIERID)
        {
            string json = "";
            if (SUPPLIERID != "")
            {
                int nSUPPLIERID = Convert.ToInt32(SUPPLIERID);
                HttpContext.Current.Session["SUPPLIERID"] = nSUPPLIERID;
                System.Data.DataSet _ds = new DataSet();

                if (nSUPPLIERID > 0)
                {
                    SupplierRoutines _Routine = new SupplierRoutines();
                    _ds = _Routine.Get_Supplier_Specific_Buyers(nSUPPLIERID);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(_ds.GetXml());
                    json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
                }
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetLes_MtmlImportPaths()
        {
            List<string> slSessionList = new List<string>();
            JavaScriptSerializer js = new JavaScriptSerializer();
            slSessionList.Add(Convert.ToString(ConfigurationManager.AppSettings["MTML_PATH"]));
            slSessionList.Add(Convert.ToString(ConfigurationManager.AppSettings["LeSXML_PATH"]));
            if (HttpContext.Current.Session["SUPPLIERID"] != null)
            {
                slSessionList.Add(HttpContext.Current.Session["SUPPLIERID"].ToString());
            }
            return js.Serialize(slSessionList);
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
        public static string GetUploadPath(string FILEPATH)
        {
            string uploadedFile = "-1";
            if (!string.IsNullOrEmpty(FILEPATH) && (!FILEPATH.ToUpper().Contains("NO FILE PATH FOUND")))
            {
                uploadedFile = FILEPATH.Replace('?', '\\');
                if (!Directory.Exists(Path.GetDirectoryName(uploadedFile))) Directory.CreateDirectory(Path.GetDirectoryName(uploadedFile));
                HttpContext.Current.Session["FILEUPLOADPATH"] = Convert.ToString(uploadedFile);
            }
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
                    string uploadedFile = Convert.ToString(HttpContext.Current.Session["FILEUPLOADPATH"]);
                    if (!string.IsNullOrEmpty(uploadedFile))
                    {
                        if (!Directory.Exists(uploadedFile))
                        {
                            Directory.CreateDirectory(uploadedFile);
                        }
                        string FileUploadPath = uploadedFile + "\\" + FILENAME;
                        uControl.SaveAs(FileUploadPath);
                        _Routine.SetAuditLog("LesMonitor", "Import File uploaded for reprocess successfully. by : " + Convert.ToString(Session["UserHostServer"]), "Updated", "", FILENAME, "", "");
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string UploadFileMapping()
        {
            string _result = "";
            try
            { }
            catch (Exception ex)
            { _result = "-1"; }
            return _result;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetExportMarkerGrid(string LINKID,string LOG_DATEFROM, string LOG_DATETO)
        {
            string json = "";
            string dtFrom = Convert.ToDateTime(LOG_DATEFROM).ToString("yyyy-MM-dd 00:00:00");
            string dtto = Convert.ToDateTime(LOG_DATETO).ToString("yyyy-MM-dd 23:59:59");

            System.Data.DataSet _ds = new DataSet();
            if (!string.IsNullOrEmpty(LINKID) && LINKID !="-1")
            {
                int nLinkID = convert.ToInt(LINKID);

                if (nLinkID > 0)
                {
                    SupplierRoutines _Routine = new SupplierRoutines();
                    _ds = _Routine.Get_SMV_Quotation_Vendor_by_LinkID(nLinkID, Convert.ToDateTime(dtFrom), Convert.ToDateTime(dtto));
                    DataSet dsClone = _ds.Copy();
                    dsClone.Tables[0].Columns.Add("UPDATEDATE");
                    foreach (DataRow row in dsClone.Tables[0].Rows)
                    {
                        foreach (DataColumn column in dsClone.Tables[0].Columns)
                        {
                            if (column.ColumnName == "UPDATE_DATE")
                            {
                                string value = Convert.ToDateTime(row.ItemArray[87]).ToString("dd/MM/yyyy");
                                row.SetField("UPDATEDATE", value);
                            }
                        }
                    }

                    dsClone.AcceptChanges();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(dsClone.GetXml());
                    json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
                }
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string UpdateExportMarker(List<string> lstExpdet)
        {
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            string _result = "",resval="";
            string DOCTYPE="",VRNO="",VERSION="",QUOTATIONID="",EXPORTMARKER="";
            List<string> slvalues = new List<string>();
            List<string> slresvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(lstExpdet);

                if (_lstdet != null && _lstdet.Count > 0)
                {
                    for (int i = 0; i < _lstdet.Count; i++)
                    {
                        slvalues.Clear();
                        string lstkey = _lstdet[i][0];
                        string val = _lstdet[i][1];
                        slvalues.Add(val);
                        Dictionary<string, string> _dictexp = _def.ConvertListDic(slvalues, ',');
                        foreach (string key in _dictexp.Keys)
                        {
                            switch (key.ToUpper())
                            {
                                case "DOCTYPE": DOCTYPE = _dictexp[key];
                                    break;
                                case "VRNO": VRNO = _dictexp[key];
                                    break;
                                case "VERSION": VERSION = _dictexp[key];
                                    break;
                                case "QUOTATIONID": QUOTATIONID = _dictexp[key];
                                    break;
                                case "EXPORTMARKER": EXPORTMARKER = _dictexp[key];
                                    break;
                            }
                        }
                        resval = _Routine.UpdateExportMarker(QUOTATIONID, DOCTYPE, convert.ToInt(EXPORTMARKER));
                        if (resval == "1")
                        {
                            _Routine.SetAuditLog("LesMonitor", "Export Marker updated successfully for VrNo : " + VRNO + " Version : " + VERSION + ". by : " + Convert.ToString(HttpContext.Current.Session["UserHostServer"]), "Updated", VRNO.ToString(), "", "", "");
                            slresvalues.Add("1");
                        }
                    }
                }
                if (slresvalues.Count > 0)
                {
                    _result = "Export Marker updated successfully";
                }
                //string val = _Routine.UpdateExportMarker(QUOTATIONID, DOCTYPE, convert.ToInt(EXPORTMARKER));

                //if (val == "1")
                //{
                //    _Routine.SetAuditLog("LesMonitor", "Export Marker updated successfully for VrNo : " + VRNO + " Version : " + VERSION + ". by : " + Convert.ToString(HttpContext.Current.Session["UserHostServer"]), "Updated", VRNO.ToString(), "", "", "");
                //    
                //}
            }
            catch (Exception ex)
            { _result = "-1"; }
            return _result;
        }
    }
}