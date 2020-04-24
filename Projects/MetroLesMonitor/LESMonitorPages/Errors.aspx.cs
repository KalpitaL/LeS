using Aspose.Cells;
using MetroLesMonitor.Dal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
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
    public partial class Errors : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetErrorsGrid(string UPDATE_DATEFROM, string UPDATE_DATETO,string ERR_STATUS)
        {
            string json = "", cStatus="";  int dtCol=0;
            DataAccess _dataAccess = new DataAccess();
            if (!string.IsNullOrEmpty(UPDATE_DATEFROM) && !string.IsNullOrEmpty(UPDATE_DATETO))
            {
                string dtFrom = Convert.ToDateTime(UPDATE_DATEFROM).ToString("yyyy-MM-dd 00:00:00");
                string dtto = Convert.ToDateTime(UPDATE_DATETO).ToString("yyyy-MM-dd 23:59:59");

                System.Data.DataSet _ds = new DataSet();

                SupplierRoutines _Routine = new SupplierRoutines();
                _ds = _Routine.GetErrorLog(Convert.ToDateTime(dtFrom), Convert.ToDateTime(dtto), ERR_STATUS);
                DataSet dsClone = _ds.Copy();
                dsClone.Tables[0].Columns.Add("UPDATE_DATE");
                dsClone.Tables[0].Columns.Add("STATUS");
                dsClone.Tables[0].Columns.Add("ERR_PROBLEM");
                dsClone.Tables[0].Columns.Add("ERR_SOLUTION");
                foreach (DataRow row in dsClone.Tables[0].Rows)
                {
                    foreach (DataColumn column in dsClone.Tables[0].Columns)
                    {                        
                        if (column.ColumnName == "UPDATEDATE")
                        {                           
                            string value = Convert.ToDateTime(row[column.ColumnName]).ToString("dd/MM/yyyy");
                            row.SetField("UPDATE_DATE", value);
                        }
                        else if (column.ColumnName == "ERROR_STATUS")
                        {
                            string value = Convert.ToString(row[column.ColumnName]);
                            if (value == "0") { cStatus = "Pending"; }
                            else if (value == "1") { cStatus = "Completed"; }
                            row.SetField("STATUS", cStatus);
                        }
                    }
                }
                dsClone.AcceptChanges();
                HttpContext.Current.Session["ErrorDataSet"] = dsClone;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(dsClone.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string ExportExcel()
        {
            string _result = "";
            SupplierRoutines _routines = new SupplierRoutines();
            try
            {
                DataSet ds = (DataSet)HttpContext.Current.Session["ErrorDataSet"];
                string cSavePath = @HttpContext.Current.Server.MapPath(@"..\Downloads\Excel\Error");
                if (!Directory.Exists(cSavePath)) Directory.CreateDirectory(cSavePath);
                string cTemplatePath = HttpContext.Current.Server.MapPath("../Templates/ERRORLOG_TEMPLATE.xls");
                _result = _routines.ExportErrors_Report(cTemplatePath, cSavePath, ds);
            }
            catch (Exception ex)
            {
                _result = "0";
            }
            return _result;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string UpdateErrorDetails(List<string> slErrdet, List<string> slchDet)
        {
            string json = "";
            int nLogID = 0, nErrStatus = 0;
            string  cErrProb = "", cErrSoln = "", cErrorstatus = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            List<string> slresvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slErrdet);

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
                            case "ERROR_PROBLEM": cErrProb = _dictexp[key];
                                break;
                            case "ERROR_SOLUTION": cErrSoln = _dictexp[key];
                                break;
                            case "ERROR_STATUS": cErrorstatus = _dictexp[key];
                                nErrStatus = (cErrorstatus.ToUpper() == "PENDING") ? 0 : 1;
                                break;
                            case "LOGID": nLogID = Convert.ToInt32(_dictexp[key]);
                                break;
                        }
                    }
                    _Routine.UpdateErrorLog(nLogID, cErrProb, cErrSoln, nErrStatus);
                    if (nLogID > 0)
                    {
                        if (cErrProb.Trim() != "" || cErrSoln != "")
                        {
                            if (slchDet.Count > 0)
                            {
                                for (int i = 0; i < slchDet.Count; i++)
                                {
                                    if (convert.ToString(slchDet[i]) == convert.ToString(nLogID)) { }
                                    else
                                    {
                                        _Routine.UpdateErrorLog(convert.ToInt(slchDet[i]), "", "Please refer Error No :" + nLogID + " for this problem.", nErrStatus);
                                    }
                                }
                            }
                        }
                    }
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
        public static string Download(string UPDATEDATE, string MODULENAME, string FILENAME, string SERVERNAME)
        {
            string cdownload = "0";
            SupplierRoutines _Routine = new SupplierRoutines();
            ServiceCallRoutines _webCallRoutine = new ServiceCallRoutines();
            try
            {
                string _path = string.Empty;
                string config_path = MODULENAME; // Module Name
                DateTime logDate = DateTime.MinValue;
                string destfile = HttpContext.Current.Server.MapPath("../Downloads/");
                if (SERVERNAME.Trim() == "" || SERVERNAME.Trim() == convert.ToString(ConfigurationManager.AppSettings["SERVER_NAME"]).Trim())
                {
                    cdownload = DownloadFileMainserver(_path, FILENAME, MODULENAME, UPDATEDATE, destfile);
                }
                else
                {
                    string _sURL = GetServerURL(SERVERNAME);
                    if (_sURL != "")
                    {
                        object[] _obj = new object[3];
                        _obj[0] = FILENAME;
                        _obj[1] = MODULENAME;
                        _obj[2] = UPDATEDATE;
                        byte[] _Data = (byte[])_webCallRoutine.CallWebService(_sURL, "FileMonitorWS", "GetDownloadFile_UpdateDate", _obj);
                        if (_Data.Length > 0)
                        {
                            FileStream fStream = new FileStream(destfile + FILENAME, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                            BinaryWriter bw = new BinaryWriter(fStream);
                            bw.Write(_Data);
                            bw.Close();
                            fStream.Close();
                            cdownload = destfile + FILENAME + "|" + FILENAME;
                        }
                        else
                        {
                            _Routine.SetLog("File " + FILENAME + " not found.");
                        }
                    }
                    else
                    {
                        cdownload = DownloadFileMainserver(_path, FILENAME, MODULENAME, UPDATEDATE, destfile);
                    }
                }
            }
            catch (Exception ex)
            {
                cdownload = "0";
                _Routine.SetLog(ex.StackTrace);
            }
            return cdownload;
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

        public static string DownloadFileMainserver(string _path, string filename, string module, string updateDate, string destfile)
        {
            string cdownload = "0";
            DateTime logDate;
            _path = Convert.ToString(ConfigurationManager.AppSettings["AUDIT_DOC_PATH"]);
            string _backupPath = Convert.ToString(ConfigurationManager.AppSettings["AUDIT_BACKUP_PATH"]);

            string _FPath = "";
            string strFile = Get_Download_Filename(filename, module);
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
                _FPath = GetCorrectPath(_backupPath, strFile); // Search in AuditBackup Path
            /**/
            if (!File.Exists(_FPath + "\\" + strFile))
            {
                if (!string.IsNullOrEmpty(_path))
                {
                    if (_path.Contains('|'))
                    {
                        string[] _arrPath = _path.Split('|');
                        for (int i = 0; i < _arrPath.Length; i++)
                        {
                            if (convert.ToString(_arrPath[i]) != "")
                            {
                                _FPath = SearchInDirectory(_arrPath[i], strFile); // Search in module path
                                if (_FPath.Trim().Length > 0)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else _FPath = SearchInDirectory(_path, strFile); // Search in module path
                }
            }
            FileInfo file = new FileInfo(_FPath + "\\" + strFile);
            if (file.Exists)
            {
                file.CopyTo(destfile + filename, true);
                cdownload = destfile + filename + "|" + filename;
            }
            else
            { }
            return cdownload;
        }

        public static string GetServerURL(string _serverName)
        {
            string _url = "";
            string[] _lstSERVER = convert.ToString(ConfigurationManager.AppSettings["SERVER_NAME_LIST"]).Split('|');
            string[] _lstURL = convert.ToString(ConfigurationManager.AppSettings["SERVER_WEBSERVICE_LIST"]).Split('|');
            for (int i = 0; i < _lstSERVER.Length; i++)
            {
                if (convert.ToString(_serverName).Trim() == convert.ToString(_lstSERVER[i]).Trim())
                {
                    _url = convert.ToString(_lstURL[i]).Trim();
                }
            }
            return _url;
        }

        private static string SearchInDirectory(string _Path, string _File)
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
        
        public static string GetFilePath(string DirectoryName, string FileName)
        {
            if (DirectoryName.Trim().Length > 0 && FileName.Trim().Length > 0)
            {
                if (Directory.Exists(DirectoryName))
                {
                    string[] _files = Directory.GetFiles(DirectoryName.Trim(), FileName.Trim(), SearchOption.AllDirectories);
                    if (_files.Length > 0)
                    {
                        return Path.GetDirectoryName(_files[0]);
                    }
                    else return "";
                }
                else return "";
            }
            else return "";
        }

        private static string Get_Download_Filename(string filename, string module)
        {
            try
            {
                string sentModules = Convert.ToString(ConfigurationManager.AppSettings["sent"]);
                if (!string.IsNullOrEmpty(sentModules))
                {
                    string[] Modules = sentModules.Split(',');
                    foreach (string _mod in Modules)
                    {
                        if (_mod == module)
                        {
                            filename = "sent_" + filename;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            return filename;
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
                    string fPath = cPath.Trim() + "\\" + _File;
                    _Routine.SetLog("Searching file in : " + fPath);
                    FileInfo file = new FileInfo(fPath);
                    {
                        if (file.Exists)
                        {
                            _Routine.SetLog("Found file '" + _File + "' in : " + cPath.Trim());
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

        [WebMethod]
        public static string RemoveFileFromQueue(string FileName, int LOGID)
        {
            try
            {
                string result = "";

                SupplierRoutines _Routine = new SupplierRoutines();
                Dictionary<string, string> lstAudit = _Routine.GetAuditDetails(LOGID);

                if (lstAudit.Count > 0)
                {
                    string cLOGID = LOGID.ToString();
                    string filename = lstAudit["FILE_NAME"];
                    string ModuleName = convert.ToString(lstAudit["MODULE_NAME"]).Trim().ToUpper();
                    string filePath = System.Configuration.ConfigurationManager.AppSettings["AUDIT_DOC_PATH"];
                    string OrgFilePath = filename.Trim();
                    string keyRef2 = lstAudit["KEY_REF2"];
                    string cBuyerID = lstAudit["BUYER_ID"];
                    string cSuppID = lstAudit["SUPPLIER_ID"];

                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                    string groupCode = ModuleName.Replace("_RFQ", "").Replace("_QUOTE", "").Replace("_RFQAck", "").Replace("_PO", "").Replace("_POAck", "").Trim('_').Trim();
                    string DocType = "";
                    if (ModuleName.Contains("RFQ")) DocType = "RFQ";
                    else if (ModuleName.Contains("RFQAck")) DocType = "RFQAck";
                    else if (ModuleName.Contains("QUOTE")) DocType = "QUOTE";
                    else if (ModuleName.Contains("PO")) DocType = "PO";
                    else if (ModuleName.Contains("POAck")) DocType = "POAck";
                    else if (ModuleName.Contains("POC")) DocType = "POC";

                    if (ModuleName.Contains("MTML_IMPORT"))
                    {
                        #region /* MTML_IMPORT PATH */
                        string eSuppImportPath = convert.ToString(ConfigurationManager.AppSettings["ESUPPLIER_INBOX"]);
                        if (Directory.Exists(eSuppImportPath))
                        {
                            if (File.Exists(eSuppImportPath + "\\" + filename))
                            {
                                if (Directory.Exists(eSuppImportPath + "\\PendingFiles"))
                                    Directory.CreateDirectory(eSuppImportPath + "\\PendingFiles");

                                string newFileName = "";
                                if (!File.Exists(eSuppImportPath + "\\PendingFiles\\" + FileName.Trim()))
                                {
                                    newFileName = Path.GetFileNameWithoutExtension(FileName).Trim() + "_" + DateTime.Now.ToString("yyyyMMddHHmmssff") + "." + Path.GetExtension(FileName).Trim('.');
                                }
                                else newFileName = FileName.Trim();

                                File.Move(eSuppImportPath + "\\" + FileName.Trim(), eSuppImportPath + "\\PendingFiles\\" + newFileName.Trim());
                                if (File.Exists(eSuppImportPath + "\\PendingFiles\\" + newFileName.Trim()))
                                    result = "File moved successfully.";
                                else
                                    result = "Unable to move file.";
                            }
                            else result = "File not found";
                        }
                        #endregion
                    }
                    else if (ModuleName.Contains("MTML_EXPORT"))
                    {
                        #region /* MTML_EXPORT PATH */
                        string eSuppExportPath = convert.ToString(ConfigurationManager.AppSettings["ESUPPLIER_OUTBOX"]);
                        if (Directory.Exists(eSuppExportPath))
                        {
                            if (File.Exists(eSuppExportPath + "\\" + filename))
                            {
                                if (Directory.Exists(eSuppExportPath + "\\PendingFiles"))
                                    Directory.CreateDirectory(eSuppExportPath + "\\PendingFiles");

                                string newFileName = "";
                                if (!File.Exists(eSuppExportPath + "\\PendingFiles\\" + FileName.Trim()))
                                {
                                    newFileName = Path.GetFileNameWithoutExtension(FileName).Trim() + "_" + DateTime.Now.ToString("yyyyMMddHHmmssff") + "." + Path.GetExtension(FileName).Trim('.');
                                }
                                else newFileName = FileName.Trim();

                                File.Move(eSuppExportPath + "\\" + FileName.Trim(), eSuppExportPath + "\\PendingFiles\\" + newFileName.Trim());
                                if (File.Exists(eSuppExportPath + "\\PendingFiles\\" + newFileName.Trim()))
                                    result = "File moved successfully.";
                                else
                                    result = "Unable to move file.";
                            }
                            else result = "File not found";
                        }
                        #endregion
                    }
                    else if (ModuleName.Contains("LeSXML_Import"))
                    {
                        #region /* MTML_IMPORT PATH */
                        string LesXMLImportPath = convert.ToString(ConfigurationManager.AppSettings["LES_XML_INBOX"]);
                        if (Directory.Exists(LesXMLImportPath))
                        {
                            if (File.Exists(LesXMLImportPath + "\\" + filename))
                            {
                                if (Directory.Exists(LesXMLImportPath + "\\PendingFiles"))
                                    Directory.CreateDirectory(LesXMLImportPath + "\\PendingFiles");

                                string newFileName = "";
                                if (!File.Exists(LesXMLImportPath + "\\PendingFiles\\" + FileName.Trim()))
                                {
                                    newFileName = Path.GetFileNameWithoutExtension(FileName).Trim() + "_" + DateTime.Now.ToString("yyyyMMddHHmmssff") + "." + Path.GetExtension(FileName).Trim('.');
                                }
                                else newFileName = FileName.Trim();

                                File.Move(LesXMLImportPath + "\\" + FileName.Trim(), LesXMLImportPath + "\\PendingFiles\\" + newFileName.Trim());
                                if (File.Exists(LesXMLImportPath + "\\PendingFiles\\" + newFileName.Trim()))
                                    result = "File moved successfully.";
                                else
                                    result = "Unable to move file.";
                            }
                            else result = "File not found";
                        }
                        #endregion
                    }
                    else
                    {
                        Bll.SmBuyerSupplierLinkCollection linkColl = Bll.SmBuyerSupplierLink.Select_SM_BUYER_SUPPLIER_LINKs_By_GROUP_CODE(groupCode.Trim());
                        if (linkColl.Count > 0)
                        {
                            foreach (Bll.SmBuyerSupplierLink linkObj in linkColl)
                            {
                                string accualFilePath = "";

                                if (convert.ToString(linkObj.ImportPath) != "" && Directory.Exists(convert.ToString(linkObj.ImportPath)) && File.Exists(convert.ToString(linkObj.ImportPath) + "\\" + filename.Trim()))
                                {
                                    accualFilePath = convert.ToString(linkObj.ImportPath);
                                }
                                else if (convert.ToString(linkObj.ExportPath) != "" && Directory.Exists(convert.ToString(linkObj.ExportPath)) && File.Exists(convert.ToString(linkObj.ExportPath) + "\\" + filename.Trim()))
                                {
                                    accualFilePath = convert.ToString(linkObj.ExportPath);
                                }
                                else if (convert.ToString(linkObj.VendorAddress.AddrInbox) != "" && Directory.Exists(convert.ToString(linkObj.VendorAddress.AddrInbox)) && File.Exists(convert.ToString(linkObj.VendorAddress.AddrInbox) + "\\" + filename.Trim()))
                                {
                                    accualFilePath = convert.ToString(linkObj.VendorAddress.AddrInbox);
                                }
                                else if (convert.ToString(linkObj.VendorAddress.AddrOutbox) != "" && Directory.Exists(convert.ToString(linkObj.VendorAddress.AddrOutbox)) && File.Exists(convert.ToString(linkObj.VendorAddress.AddrOutbox) + "\\" + filename.Trim()))
                                {
                                    accualFilePath = convert.ToString(linkObj.VendorAddress.AddrOutbox);
                                }

                                if (accualFilePath.Trim() != "")
                                {
                                    if (!Directory.Exists(accualFilePath + "\\PendingFiles")) Directory.CreateDirectory(accualFilePath + "\\PendingFiles");

                                    string newFileName = "";
                                    if (!File.Exists(accualFilePath + "\\PendingFiles\\" + FileName.Trim()))
                                    {
                                        newFileName = Path.GetFileNameWithoutExtension(FileName).Trim() + "_" + DateTime.Now.ToString("yyyyMMddHHmmssff") + "." + Path.GetExtension(FileName).Trim('.');
                                    }
                                    else newFileName = FileName.Trim();

                                    File.Move(accualFilePath + "\\" + FileName.Trim(), accualFilePath + "\\PendingFiles\\" + newFileName.Trim());
                                    if (File.Exists(accualFilePath + "\\PendingFiles\\" + newFileName.Trim()))
                                    {
                                        result = "File moved successfully.";
                                        break;
                                    }
                                    else result = "Unable to move file.";
                                }
                                else result = "File not found";
                            }
                        }
                        else
                        {
                            if (System.Configuration.ConfigurationManager.AppSettings["REMOVE_FILE_COPY_LOG"] != null)
                            {
                                string CopyLogPath = convert.ToString(System.Configuration.ConfigurationManager.AppSettings["REMOVE_FILE_COPY_LOG"]);
                                if (!Directory.Exists(CopyLogPath))
                                {
                                    Directory.CreateDirectory(CopyLogPath);
                                }
                                string Group = ModuleName.Trim().Split('_')[0].Trim();
                                if (Group.Trim() == "ML") Group += "_" + ModuleName.Trim().Split('_')[1].Trim();
                                string Log = Group + "|" + DocType + "|" + FileName;
                                string copyFileName = Group + "_" + DocType.Trim().ToUpper() + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssff") + ".txt";

                                File.WriteAllText(CopyLogPath + "\\" + copyFileName, Log);

                                result = "File moved successfully.";
                            }
                            else result = "Unable to find Copy Log Path.";
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return "Unable to move file because of internal error";
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetBuyerSupplier_MailDetails(string BuyerCode, string SupplierCode)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                Dictionary<string, string> slmaildet = _Routine.GetMailDetails_Code(BuyerCode, SupplierCode);
                json = JsonConvert.SerializeObject(slmaildet);
            }
            catch (Exception ex)
            {
                json = "";
            }

            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetMailDetails(string LOGID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                Dictionary<string, string> slmaildet = _Routine.GetMailDetails_Code("", "");
                json = JsonConvert.SerializeObject(slmaildet);
            }
            catch (Exception ex)
            {
                json = "";
            }

            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetSendErrorMail(List<string> slErrMaildet)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
              
            }
            catch (Exception ex)
            {
                json = "";
            }

            return json;
        }

        #region Error Solution template

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetSolutionTemplate_details()
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            DataSet _ds = _Routine.GetError_SolutionDetails();
            HttpContext.Current.Session["ErrorSolutionDataSet"] = _ds;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string UpdateErrorSolution_Details(List<string> slErrSdet)//, List<string> slchDet
        {
            string json = "";
            string cErrProb = "", cErrSoln = "", cErrordesc = "", cErrortemp = "", cErrorNo = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            List<string> slresvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slErrSdet);

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
                            case "ERROR_PROBLEM": cErrProb = _dictexp[key];
                                break;
                            case "ERROR_SOLUTION": cErrSoln = _dictexp[key];
                                break;
                            case "ERROR_DESC": cErrordesc = _dictexp[key];
                                break;
                            case "ERROR_TEMPLATE": cErrortemp = _dictexp[key];
                                break;
                            case "ERROR_NO": cErrorNo = _dictexp[key];
                                break;                           
                        }
                    }
                    _Routine.SaveError_SolutionDetails(cErrorNo, cErrordesc, cErrProb, cErrSoln, cErrortemp, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
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
        public static string GetError_Details(string LOGID)
        {
            string json = "";
            DataSet ErrorDataSet = (DataSet)HttpContext.Current.Session["ErrorDataSet"];
            DataSet ErrorSolutionDataSet = (DataSet)HttpContext.Current.Session["ErrorSolutionDataSet"];
            json = SupplierRoutines.GetErrorDetails(LOGID, ErrorDataSet, ErrorSolutionDataSet);
            return json;
        }

        #endregion
    }
}