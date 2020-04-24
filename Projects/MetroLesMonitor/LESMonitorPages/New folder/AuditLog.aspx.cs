using MetroLesMonitor.Bll;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;

namespace MetroLesMonitor.LESMonitorPages
{
    public partial class AuditLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Filter Criteria

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetAuditLogGrid(List<string> slftrdet)
        {
            string json = "", SELECTCOND = "", cWHERECOND = "", dtFrom = "", dtto = "";
            int AddressID = 0; int Totalrows = 0;
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            // string PCName = Default.GetIPDetails().Split('|')[1];
            List<string> slvalues = new List<string>(); slvalues.Clear();
          //  DataAccess _dataAccess = new DataAccess();
            int TO = 0, FROM = 0;

            List<string[]> _lstdet = _def.ConvertListToListArray(slftrdet);
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
                        case "LOG_FROM": dtFrom = (!string.IsNullOrEmpty(_dictexp[key]))?Convert.ToDateTime(_dictexp[key]).ToString("yyyy-MM-dd 00:00:00"):string.Empty;
                            break;
                        case "LOG_TO": dtto = (!string.IsNullOrEmpty(_dictexp[key]))?Convert.ToDateTime(_dictexp[key]).ToString("yyyy-MM-dd 23:59:59"):string.Empty;
                            break;
                        case "FROM": FROM = Convert.ToInt32(_dictexp[key]);
                            break;
                        case "TO": TO = Convert.ToInt32(_dictexp[key]);
                            break;
                    }
                }
                cWHERECOND = GetFilterCondition(_dictexp);
            }
            object obj = new object();
            System.Data.DataTable _dt = new DataTable(); System.Data.DataSet _ds = new DataSet();
            if (HttpContext.Current.Session["ADDRESSID"] != null && HttpContext.Current.Session["ADDRESSID"] != "")
            {
                AddressID = Convert.ToInt32(HttpContext.Current.Session["ADDRESSID"]);
            }
            if (AddressID > 0)
            {
                SupplierRoutines _Routine = new SupplierRoutines();
                if (!string.IsNullOrEmpty(dtFrom) && !string.IsNullOrEmpty(dtto))
                {
                    _ds = _Routine.GetAuditLog(AddressID, Convert.ToDateTime(dtFrom), Convert.ToDateTime(dtto), cWHERECOND, FROM, TO, out Totalrows);
                    HttpContext.Current.Session["AUDIT_FILTERCOND"] = String.Join(",", new string[] { dtFrom, dtto, cWHERECOND });
                    if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                    {
                        GetFilterCriteria(_ds.Tables[0]);
                        HttpContext.Current.Session["AuditDataSet"] = _ds;
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(_ds.GetXml());
                        json = Totalrows + "||" + Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
                    }
                }
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetBuyerSupplier_Details(string SUPPLIERID, string BUYERID)
        {
            string json = "";
            try
            {
                SupplierRoutines _Routine = new SupplierRoutines();
                json = _Routine.GetBuyerSupplierdetails(SUPPLIERID, BUYERID);
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }

        private static void GetFilterCriteria(DataTable _dt)
        {
            #region ModuleName

            DataTable dtView = _dt.DefaultView.ToTable(true, "MODULENAME");
            dtView.DefaultView.Sort = "MODULENAME ASC";
            DataTable dt = dtView.DefaultView.ToTable();
            DataSet dsModule = new DataSet();
            dsModule.Tables.Add(dt);
            HttpContext.Current.Session["AUDIT_MODULES"] = dsModule;

            #endregion

            #region DocType
            DataTable dtDocView = _dt.DefaultView.ToTable(true, "DOCTYPE");
            dtDocView.DefaultView.Sort = "DOCTYPE ASC";
            DataTable dtdoc = dtDocView.DefaultView.ToTable();
            DataSet dsDoc = new DataSet();
            dsDoc.Tables.Add(dtdoc);
            HttpContext.Current.Session["AUDIT_DOCTYPE"] = dsDoc;
            #endregion

            #region LogType
            DataTable dtLogView = _dt.DefaultView.ToTable(true, "LOGTYPE");
            dtLogView.DefaultView.Sort = "LOGTYPE ASC";
            DataTable dtLog = dtLogView.DefaultView.ToTable();
            DataSet dsLog = new DataSet();
            dsLog.Tables.Add(dtLog);
            HttpContext.Current.Session["AUDIT_LOGTYPE"] = dsLog;
            #endregion
        }

        private static string GetFilterCondition(Dictionary<string, string> _dictexp)
        {
            string cWHERECOND = "";
            foreach (string key in _dictexp.Keys)
            {
                switch (key.ToUpper())
                {
                    case "SERVER_NAME": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND SERVERNAME LIKE '%" + _dictexp[key] + "%'"; }
                        break;
                    case "DOC_TYPE": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND DOCTYPE LIKE '%" + _dictexp[key] + "%'"; }
                        break;
                    case "MODULE": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND MODULENAME LIKE '%" + _dictexp[key] + "%'"; }
                        break;
                    case "LOG_TYPE": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND LOGTYPE LIKE '%" + _dictexp[key] + "%'"; }
                        break;
                    case "BUYER_CODE": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND  BUYERCODE LIKE '%" + _dictexp[key] + "%'"; }
                        break;
                    case "SUPPLIER_CODE": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND  SUPPLIERCODE LIKE '%" + _dictexp[key] + "%'"; }
                        break;
                    case "KEY_REF": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND KEYREF2 LIKE '%" + _dictexp[key] + "%'"; }
                        break;
                    case "REMARKS": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND AUDITVALUE LIKE '%" + _dictexp[key] + "%'"; }
                        break;
                    case "FILENAME": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND FILENAME LIKE '%" + _dictexp[key] + "%'"; }
                        break;
                    case "QUICK_SEARCH": if (!string.IsNullOrEmpty(_dictexp[key])) {
                        string cSearch = "AND (SERVERNAME  LIKE '%" + _dictexp[key] + "%' OR DOCTYPE  LIKE '%" + _dictexp[key] + "%' OR " +
                            " MODULENAME  LIKE '%" + _dictexp[key] + "%' OR LOGTYPE  LIKE '%" + _dictexp[key] + "%' OR KEYREF2  LIKE '%" + _dictexp[key] + "%' OR " +
                            " AUDITVALUE  LIKE '%" + _dictexp[key] + "%' OR FILENAME  LIKE '%" + _dictexp[key] + "%' OR BUYERCODE  LIKE '%" + _dictexp[key] + "%' OR SUPPLIERCODE  LIKE '%" + _dictexp[key] + "%' )";
                        cWHERECOND += cSearch; }
                        break;
                }
            }
            return cWHERECOND;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillModules()
        {
            string json = ""; DataSet dsModule = new DataSet();
            dsModule.Clear();
            if (HttpContext.Current.Session["AUDIT_MODULES"] != null && HttpContext.Current.Session["AUDIT_MODULES"] != "")
            {
                dsModule = (DataSet)HttpContext.Current.Session["AUDIT_MODULES"];
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(dsModule.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillLogType()
        {
            string json = ""; DataSet dsLogType = new DataSet(); dsLogType.Clear(); string cdet=GetLog_Doctype_Details().Split('@')[0];
            if (HttpContext.Current.Session["AUDIT_LOGTYPE"] != null && HttpContext.Current.Session["AUDIT_LOGTYPE"] != "")
            {
                dsLogType = (DataSet)HttpContext.Current.Session["AUDIT_LOGTYPE"];              
            }
            if (dsLogType != null && dsLogType.Tables.Count > 0 && dsLogType.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(dsLogType.Tables[0].Rows[0][0]) != "") { } else { dsLogType = ConvertDataSet(cdet, "LOGTYPE"); }
            }
            else { dsLogType = ConvertDataSet(cdet, "LOGTYPE"); }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(dsLogType.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillDocType()
        {
            string json = ""; DataSet dsdoctype = new DataSet(); dsdoctype.Clear(); string cdet = GetLog_Doctype_Details().Split('@')[1];
            if (HttpContext.Current.Session["AUDIT_DOCTYPE"] != null && HttpContext.Current.Session["AUDIT_DOCTYPE"] != "")
            {
                dsdoctype = (DataSet)HttpContext.Current.Session["AUDIT_DOCTYPE"];
            }
            if (dsdoctype != null && dsdoctype.Tables.Count > 0 && dsdoctype.Tables[0].Rows.Count > 0) {
                if (Convert.ToString(dsdoctype.Tables[0].Rows[0][0]) != "") { } else { dsdoctype = ConvertDataSet(cdet, "DOCTYPE"); }  } 
            else { dsdoctype = ConvertDataSet(cdet,"DOCTYPE"); }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(dsdoctype.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        //added by kalpita on 03/02/2018
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetAuditLog_AddressID(List<string> slftrdet)
        {
            string json = "", cWHERECOND = "", dtFrom = "", dtto = "";
            int Totalrows = 0; int TO = 0, FROM = 0, ADDRESSID = 0;
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>(); slvalues.Clear(); List<string[]> _lstdet = _def.ConvertListToListArray(slftrdet);
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
                        case "LOG_FROM": dtFrom = Convert.ToDateTime(_dictexp[key]).ToString("yyyy-MM-dd 00:00:00");
                            break;
                        case "LOG_TO": dtto = Convert.ToDateTime(_dictexp[key]).ToString("yyyy-MM-dd 23:59:59");
                            break;
                        case "FROM": FROM = Convert.ToInt32(_dictexp[key]);
                            break;
                        case "TO": TO = Convert.ToInt32(_dictexp[key]);
                            break;
                        case "ADDRESSID": ADDRESSID = Convert.ToInt32(_dictexp[key]);
                            break;
                    }
                }
            }
            SupplierRoutines _Routine = new SupplierRoutines(); System.Data.DataSet _ds = new DataSet();
            _ds = _Routine.GetAuditLog_AddressID(ADDRESSID, Convert.ToDateTime(dtFrom), Convert.ToDateTime(dtto), cWHERECOND, FROM, TO, out Totalrows);
            if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
            {
                GetFilterCriteria(_ds.Tables[0]);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_ds.GetXml());
                json = Totalrows + "||" + Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }

        #endregion

        [WebMethod]
        public static bool CheckFileExist(int LOGID)
        {
            try
            {
                bool fileFound = false;
                SupplierRoutines _Routine = new SupplierRoutines();
                Dictionary<string, string> lstAudit = _Routine.GetAuditDetails(LOGID);

                if (lstAudit.Count > 0)
                {
                    string filename = lstAudit["FILE_NAME"];
                    string ModuleName = convert.ToString(lstAudit["MODULE_NAME"]).Trim();
                    string LogType = convert.ToString(lstAudit["LOG_TYPE"]).Trim();
                    string cBuyerID = convert.ToString(lstAudit["BUYER_ID"]).Trim();
                    string cSuppID = convert.ToString(lstAudit["SUPPLIER_ID"]).Trim();

                    AuditLog auditlog = new AuditLog();
                    string filePath = convert.ToString(SearchFileOnServer(convert.ToInt(cBuyerID), convert.ToInt(cSuppID), ModuleName.Trim(), LogType.Trim(), filename));
                    if (File.Exists(filePath + "\\" + filename))
                    {
                        fileFound = true;
                    }
                }
                return fileFound;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DownloadFile(string UPDATEDATE, string MODULENAME, string FILENAME,string SERVERNAME)
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
                cdownload="0";
                _Routine.SetLog(ex.StackTrace);
            }
            return cdownload;
        }


        public static string DownloadFileMainserver(string _path, string filename, string module, string updateDate, string destfile)
        {
            string cdownload = "0";
            SupplierRoutines _Routine = new SupplierRoutines();
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
            {
                _Routine.SetLog("Search in AuditDoc Path");
                _FPath = GetCorrectPath(_path, strFile); // Search in AuditDoc Path
            }
            if (!File.Exists(_FPath + "\\" + strFile))
            {
                _Routine.SetLog("Search in AuditBackup Path");
                _FPath = GetCorrectPath(_backupPath, strFile); // Search in AuditBackup Path
            }

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
            {
            }
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

        private static string SearchFileOnServer(int Buyerid, int Suppid, string ModuleName, string LogType, string FileName)
        {
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                string orgFilePath = "";

                // Search in eSupplier Inbox & Outbox & LesXML Inbox
                string eSupplierInbox = convert.ToString(ConfigurationManager.AppSettings["ESUPPLIER_INBOX"]).Trim();
                string eSupplierOutbox = convert.ToString(ConfigurationManager.AppSettings["ESUPPLIER_OUTBOX"]).Trim();
                string LesXmlInbox = convert.ToString(ConfigurationManager.AppSettings["LES_XML_INBOX"]).Trim();

                //DirectoryInfo fileDirInfo = null;
                orgFilePath = GetFilePath(eSupplierInbox, FileName.Trim());
                if (orgFilePath.Trim().Length == 0)
                {
                    orgFilePath = GetFilePath(LesXmlInbox, FileName.Trim());
                }

                // Search By 
                if (orgFilePath.Trim().Length == 0)
                {
                    SmBuyerSupplierLinkCollection linkColl = new SmBuyerSupplierLinkCollection();
                    if (Buyerid > 0 && Suppid > 0)
                    {
                        linkColl = SmBuyerSupplierLink.Select_SM_BUYER_SUPPLIER_LINKs_By_BUYERID_SUPPLIERID(Buyerid, Suppid);
                        foreach (SmBuyerSupplierLink linkObj in linkColl)
                        {
                            orgFilePath = GetFilePath(convert.ToString(linkObj.ImportPath), FileName.Trim());
                            if (orgFilePath.Trim().Length == 0)
                            {
                                orgFilePath = GetFilePath(convert.ToString(linkObj.ExportPath), FileName.Trim());
                                if (orgFilePath.Trim().Length == 0)
                                {
                                    orgFilePath = GetFilePath(convert.ToString(linkObj.VendorAddress.AddrInbox), FileName.Trim());
                                    if (orgFilePath.Trim().Length == 0)
                                    {
                                        orgFilePath = GetFilePath(convert.ToString(linkObj.VendorAddress.AddrOutbox), FileName.Trim());
                                        if (orgFilePath.Trim().Length == 0)
                                        {
                                            orgFilePath = GetFilePath(convert.ToString(linkObj.SuppImportPath), FileName.Trim());
                                            if (orgFilePath.Trim().Length == 0)
                                            {
                                                orgFilePath = GetFilePath(convert.ToString(linkObj.SuppExportPath), FileName.Trim());
                                            }
                                        }
                                    }
                                }
                            }
                            if (orgFilePath.Trim().Length > 0) break;
                        }
                    }
                }

                if (orgFilePath.Trim().Length == 0)
                {
                    string grpCode = ModuleName.Trim().ToUpper().Replace("_RFQ", "")
                         .Replace("_QUOTE", "")
                         .Replace("_ORDER", "")
                         .Replace("_PO", "")
                         .Replace("_RFQAck", "")
                         .Replace("_POC", "")
                         .Replace("_POAck", "")
                         .Replace("_ORDERAck", "");

                    SmBuyerSupplierLinkCollection grpLinkCollection = SmBuyerSupplierLink.Select_SM_BUYER_SUPPLIER_LINKs_By_GROUP_CODE(grpCode.Trim());
                    if (grpLinkCollection.Count > 0)
                    {
                        foreach (SmBuyerSupplierLink linkObj in grpLinkCollection)
                        {
                            orgFilePath = GetFilePath(convert.ToString(linkObj.ImportPath), FileName.Trim());
                            if (orgFilePath.Trim().Length == 0)
                            {
                                orgFilePath = GetFilePath(convert.ToString(linkObj.ExportPath), FileName.Trim());
                                if (orgFilePath.Trim().Length == 0)
                                {
                                    orgFilePath = GetFilePath(convert.ToString(linkObj.VendorAddress.AddrInbox), FileName.Trim());
                                    if (orgFilePath.Trim().Length == 0)
                                    {
                                        orgFilePath = GetFilePath(convert.ToString(linkObj.VendorAddress.AddrOutbox), FileName.Trim());
                                        if (orgFilePath.Trim().Length == 0)
                                        {
                                            orgFilePath = GetFilePath(convert.ToString(linkObj.SuppImportPath), FileName.Trim());
                                            if (orgFilePath.Trim().Length == 0)
                                            {
                                                orgFilePath = GetFilePath(convert.ToString(linkObj.SuppExportPath), FileName.Trim());
                                            }
                                        }
                                    }
                                }
                            }
                            if (orgFilePath.Trim().Length > 0) break;
                        }
                    }
                    else
                    {
                        SmAddress addr = SmAddress.LoadByGroup(grpCode.Trim());
                        if (addr != null && addr.Addressid > 0)
                        {
                            orgFilePath = GetFilePath(convert.ToString(addr.AddrInbox), FileName.Trim());
                            if (orgFilePath.Trim().Length == 0)
                            {
                                orgFilePath = GetFilePath(convert.ToString(addr.AddrOutbox), FileName.Trim());
                            }
                        }
                    }
                }

                if (orgFilePath.Trim().Length == 0)
                {
                    // Seach on PDF & XLS Path
                    if (ModuleName.Trim() == "PDF")
                    {
                        string PDFCommonPath = convert.ToString(ConfigurationManager.AppSettings["PDF_COMMON_PATH"]).Trim();
                        orgFilePath = GetFilePath(PDFCommonPath.Trim(), FileName.Trim());
                    }
                }

                return orgFilePath;
            }
            catch (Exception ex)
            {
                return "";
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
                    string fPath=cPath.Trim() + "\\" + _File;
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
        public static string ExportExcel()
        {
            string _result = "",cFilterCond="";
            SupplierRoutines _routines = new SupplierRoutines();
            try
            {
                cFilterCond = Convert.ToString(HttpContext.Current.Session["AUDIT_FILTERCOND"]);
                string[] _filterArr = cFilterCond.Split(',');


                DataSet ds = _routines.GetAuditLog_Filter(_filterArr[2], _filterArr[0], _filterArr[1]);
                string cSavePath = @HttpContext.Current.Server.MapPath(@"..\Downloads\Excel\Audit");
                if (!Directory.Exists(cSavePath)) Directory.CreateDirectory(cSavePath);
                string cTemplatePath = HttpContext.Current.Server.MapPath("../Templates/AUDITLOG_TEMPLATE.xls");
                _result = _routines.ExportAudit_Report(cTemplatePath, cSavePath, ds);
            }
            catch (Exception ex)
            {
                _result = "0";
            }
            return _result;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string ReSubmitFile(string LOGID)
        {
            string res = "";
            Dictionary<string,string> lstAudit=new Dictionary<string,string>();
            lstAudit.Clear();
            try
            {
                SupplierRoutines _Routine = new SupplierRoutines();
                lstAudit = _Routine.GetAuditDetails(Convert.ToInt32(LOGID));

                if (lstAudit.Count > 0)
                {
                    string FilePath = "";
                    string newMoveTodir = "";

                    string cLOGID = LOGID;
                    string filename = convert.ToString(lstAudit["FILE_NAME"]).Trim();
                    string ModuleName = convert.ToString(lstAudit["MODULE_NAME"]).Trim();
                    string LogType = convert.ToString(lstAudit["LOG_TYPE"]).Trim();
                    string keyRef2 = convert.ToString(lstAudit["KEY_REF2"]).Trim();
                    string cBuyerID = convert.ToString(lstAudit["BUYER_ID"]).Trim();
                    string cSuppID = convert.ToString(lstAudit["SUPPLIER_ID"]).Trim();
                    string ResubmitFile = DateTime.Now.ToString("MMddHHmm_") + filename;
                    try
                    {
                        FilePath = SearchFileOnServer(convert.ToInt(cBuyerID), convert.ToInt(cSuppID), ModuleName.Trim(), LogType.Trim(), filename.Trim());

                        if (File.Exists(FilePath + "\\" + filename))
                        {
                            DirectoryInfo currentDir = new DirectoryInfo(FilePath);
                            newMoveTodir = currentDir.Parent.FullName;

                            string Extension = Path.GetExtension(FilePath + "\\" + filename).Replace('.', ' ').Trim().ToLower();
                            if (Extension == "txt")
                            {
                                // Create a file with txtFile Text and Resubmit that Data
                                using (StreamWriter sw = new StreamWriter(newMoveTodir + "\\" + ResubmitFile, false))
                                {
                                    // sw.Write(txtFile.Text.Trim());
                                    sw.Flush();
                                    sw.Close();
                                }
                            }
                            else if (Extension == "xml")
                            {
                                XmlDocument doc = new XmlDocument();
                                // doc.LoadXml(txtFile.Text.Trim());
                                // doc.Save(newMoveTodir + "\\" + ResubmitFile);
                            }
                            else
                            {
                                // Copy File
                                File.Copy(FilePath + "\\" + filename, newMoveTodir + "\\" + ResubmitFile);
                            }

                            if (File.Exists(newMoveTodir + "\\" + ResubmitFile))
                            {
                                // Copy to Audit Path
                                string AuditPath = convert.ToString(ConfigurationManager.AppSettings["AUDIT_DOC_PATH"]).Trim();
                                File.Copy(newMoveTodir + "\\" + ResubmitFile, AuditPath + "\\" + ResubmitFile);

                                // lblError.Text = ResubmitFile + " is resubmitted ";
                                _Routine.SetAuditLog("LeSMonitor", ResubmitFile + " is resubmitted.", "Resubmit", keyRef2, ResubmitFile, cBuyerID, cSuppID);

                                //  popupResubmit.ShowOnPageLoad = false;
                                res = "File '" + filename + "' resubmitted successfully.";
                            }
                            else
                            {
                                //  popupResubmit.ShowOnPageLoad = false;
                                res = "Unable to resubmit file.";
                            }
                        }
                        else
                        {
                            // popupResubmit.ShowOnPageLoad = false;
                            res = "File path not found !";
                        }
                        //Session.Remove("LOG_ID");
                    }
                    catch (Exception ex)
                    {
                        // popupResubmit.ShowOnPageLoad = false;
                        _Routine.SetAuditLog("LeSMonitor", "Unable to resubmit - " + ex.Message, "Error", keyRef2, filename, cBuyerID, cSuppID);
                        // lblError.Text = "Unable to resubmit - " + ex.Message;
                        // ShowMsg(ex.Message);
                    }
                    finally
                    {
                        //Session["LOG_ID"] = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                //popupResubmit.ShowOnPageLoad = false;
                //lblError.Text = "Unable to resubmit - " + ex.Message;
            }
            finally
            {
                //Session["LOG_ID"] = 0;
                //Response.Redirect("AuditLog.aspx");
            }

            return res;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetLog_Doctype_Details()
        {
            string clogType = "", cdocType = "";
            string ctemplate = HttpContext.Current.Server.MapPath("~/Templates/Audit.txt");
            string ctypetxt = File.ReadAllText(ctemplate);
            if (!string.IsNullOrEmpty(ctypetxt))
            {
                cdocType = ctypetxt.Split('#')[0].Split('|')[1]; clogType = ctypetxt.Split('#')[1].Split('|')[1];
            }
            return clogType + "@" + cdocType;
        }

        public static DataSet ConvertDataSet(string input,string name)
        {
            DataSet dataSet = new DataSet(); dataSet.Clear();
            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    string[] strlogtype = input.Split(',');
                    DataTable dt = new DataTable(); dt.Columns.Add(name);   foreach (var item in strlogtype) { dt.Rows.Add(item);}
                    dataSet.Tables.Add(dt);
                }
            }
            catch(Exception ex)
            { }
            return dataSet;
        }

        #region commented
        //[WebMethod]
        //  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        ////public static string GetAuditLogCount(List<string> slftrdet)//, string SEARCH
        ////{
        ////    string json = "", SELECTCOND = "", cWHERECOND = "", dtFrom = "", dtto = "";
        ////    int AddressID = 0,TO = 0, FROM = 0;
        ////    MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
        ////    List<string> slvalues = new List<string>(); slvalues.Clear();
        ////    DataAccess _dataAccess = new DataAccess();
        ////    List<string[]> _lstdet = _def.ConvertListToListArray(slftrdet);
        ////    if (_lstdet != null && _lstdet.Count > 0)
        ////    {
        ////        slvalues.Clear();
        ////        for (int i = 0; i < _lstdet.Count; i++)
        ////        {
        ////            string lstkey = _lstdet[i][0];
        ////            slvalues.Add(lstkey);
        ////        }
        ////        Dictionary<string, string> _dictexp = _def.ConvertListToDictionary(slvalues);
        ////        foreach (string key in _dictexp.Keys)
        ////        {
        ////            switch (key.ToUpper())
        ////            {
        ////                case "SERVER_NAME": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND SERVERNAME LIKE '%" + _dictexp[key] + "%'"; }
        ////                    break;
        ////                case "DOC_TYPE": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND DOCTYPE LIKE '%" + _dictexp[key] + "%'"; }
        ////                    break;
        ////                case "MODULE": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND MODULENAME LIKE '%" + _dictexp[key] + "%'"; }
        ////                    break;
        ////                case "LOG_TYPE": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND LOGTYPE LIKE '%" + _dictexp[key] + "%'"; }
        ////                    break;
        ////                case "BUYER_CODE": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND  BUYER.ADDR_CODE LIKE '%" + _dictexp[key] + "%'"; }
        ////                    break;
        ////                case "SUPPLIER_CODE": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND  VENDOR.ADDR_CODE LIKE '%" + _dictexp[key] + "%'"; }
        ////                    break;
        ////                case "KEY_REF": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND KEYREF2 LIKE '%" + _dictexp[key] + "%'"; }
        ////                    break;
        ////                case "REMARKS": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND AUDITVALUE LIKE '%" + _dictexp[key] + "%'"; }
        ////                    break;
        ////                case "FILENAME": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND FILENAME LIKE '%" + _dictexp[key] + "%'"; }
        ////                    break;
        ////                case "LOG_FROM": dtFrom = Convert.ToDateTime(_dictexp[key]).ToString("yyyy-MM-dd 00:00:00");
        ////                    break;
        ////                case "LOG_TO": dtto = Convert.ToDateTime(_dictexp[key]).ToString("yyyy-MM-dd 23:59:59");
        ////                    break;
        ////                case "FROM": FROM = Convert.ToInt32(_dictexp[key]);
        ////                    break;
        ////                case "TO": TO = Convert.ToInt32(_dictexp[key]);
        ////                    break;
        ////            }
        ////        }
        ////    }
        ////    object obj = new object();
        ////    System.Data.DataSet _ds = new DataSet();
        ////    if (HttpContext.Current.Session["ADDRESSID"] != null && HttpContext.Current.Session["ADDRESSID"] != "")
        ////    {
        ////        AddressID = Convert.ToInt32(HttpContext.Current.Session["ADDRESSID"]);
        ////    }
        ////    if (AddressID > 0)
        ////    {
        ////        SupplierRoutines _Routine = new SupplierRoutines();
        ////       // int Count = _Routine.GetAuditLogCount(AddressID, Convert.ToDateTime(dtFrom), Convert.ToDateTime(dtto), cWHERECOND);
        ////        //json = Count.ToString();
        ////    }
        ////    return json;
        ////}

        //private static void GetFilterCriteria(DataSet _ds)
        //{
        //    #region ModuleName
        //    DataTable dtView = _ds.Tables[0].DefaultView.ToTable(true, "MODULENAME");
        //    dtView.DefaultView.Sort = "MODULENAME ASC";
        //    DataTable dt = dtView.DefaultView.ToTable();
        //    DataSet dsModule = new DataSet();
        //    dsModule.Tables.Add(dt);
        //    HttpContext.Current.Session["AUDIT_MODULES"] = dsModule;
        //    #endregion

        //    #region DocType
        //    DataTable dtDocView = _ds.Tables[0].DefaultView.ToTable(true, "DOCTYPE");
        //    dtDocView.DefaultView.Sort = "DOCTYPE ASC";
        //    DataTable dtdoc = dtDocView.DefaultView.ToTable();
        //    DataSet dsDoc = new DataSet();
        //    dsDoc.Tables.Add(dtdoc);
        //    HttpContext.Current.Session["AUDIT_DOCTYPE"] = dsDoc;
        //    #endregion

        //    #region LogType
        //    DataTable dtLogView = _ds.Tables[0].DefaultView.ToTable(true, "LOGTYPE");
        //    dtLogView.DefaultView.Sort = "LOGTYPE ASC";
        //    DataTable dtLog = dtLogView.DefaultView.ToTable();
        //    DataSet dsLog = new DataSet();
        //    dsLog.Tables.Add(dtLog);
        //    HttpContext.Current.Session["AUDIT_LOGTYPE"] = dsLog;
        //    #endregion
        //}

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static string GetAuditLogGrid_old(List<string> slftrdet)
        //{
        //    string json = "", SELECTCOND = "", cWHERECOND = "", dtFrom = "", dtto = "";
        //    int AddressID = 0; int Totalrows = 0;
        //    MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
        //    // string PCName = Default.GetIPDetails().Split('|')[1];
        //    List<string> slvalues = new List<string>(); slvalues.Clear();
        //    DataAccess _dataAccess = new DataAccess();
        //    int TO = 0, FROM = 0;

        //    List<string[]> _lstdet = _def.ConvertListToListArray(slftrdet);
        //    if (_lstdet != null && _lstdet.Count > 0)
        //    {
        //        slvalues.Clear();
        //        for (int i = 0; i < _lstdet.Count; i++)
        //        {
        //            string lstkey = _lstdet[i][0];
        //            slvalues.Add(lstkey);
        //        }
        //        Dictionary<string, string> _dictexp = _def.ConvertListToDictionary(slvalues);
        //        foreach (string key in _dictexp.Keys)
        //        {
        //            switch (key.ToUpper())
        //            {
        //                case "SERVER_NAME": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND SERVERNAME LIKE '%" + _dictexp[key] + "%'"; }
        //                    break;
        //                case "DOC_TYPE": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND DOCTYPE LIKE '%" + _dictexp[key] + "%'"; }
        //                    break;
        //                case "MODULE": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND MODULENAME LIKE '%" + _dictexp[key] + "%'"; }
        //                    break;
        //                case "LOG_TYPE": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND LOGTYPE LIKE '%" + _dictexp[key] + "%'"; }
        //                    break;
        //                case "BUYER_CODE": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND  BUYER.ADDR_CODE LIKE '%" + _dictexp[key] + "%'"; }
        //                    break;
        //                case "SUPPLIER_CODE": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND  VENDOR.ADDR_CODE LIKE '%" + _dictexp[key] + "%'"; }
        //                    break;
        //                case "KEY_REF": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND KEYREF2 LIKE '%" + _dictexp[key] + "%'"; }
        //                    break;
        //                case "REMARKS": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND AUDITVALUE LIKE '%" + _dictexp[key] + "%'"; }
        //                    break;
        //                case "FILENAME": if (!string.IsNullOrEmpty(_dictexp[key])) { cWHERECOND += " AND FILENAME LIKE '%" + _dictexp[key] + "%'"; }
        //                    break;
        //                case "LOG_FROM": dtFrom = Convert.ToDateTime(_dictexp[key]).ToString("yyyy-MM-dd 00:00:00");
        //                    break;
        //                case "LOG_TO": dtto = Convert.ToDateTime(_dictexp[key]).ToString("yyyy-MM-dd 23:59:59");
        //                    break;
        //                case "FROM": FROM = Convert.ToInt32(_dictexp[key]);
        //                    break;
        //                case "TO": TO = Convert.ToInt32(_dictexp[key]);
        //                    break;
        //            }
        //        }
        //    }
        //    object obj = new object();
        //    System.Data.DataSet _ds = new DataSet();
        //    if (HttpContext.Current.Session["ADDRESSID"] != null && HttpContext.Current.Session["ADDRESSID"] != "")
        //    {
        //        AddressID = Convert.ToInt32(HttpContext.Current.Session["ADDRESSID"]);
        //    }
        //    if (AddressID > 0)
        //    {
        //        SupplierRoutines _Routine = new SupplierRoutines();
        //        _ds = _Routine.GetAuditLog(AddressID, Convert.ToDateTime(dtFrom), Convert.ToDateTime(dtto), cWHERECOND, FROM, TO, out Totalrows);
        //        if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
        //        {
        //            // GetFilterCriteria(_ds);
        //            DataSet dsClone = _ds.Copy();
        //            dsClone.Tables[0].Columns.Add("CLIENT_DETAILS");
        //            foreach (DataRow row in dsClone.Tables[0].Rows)
        //            {
        //                row.SetField("CLIENT_DETAILS", GetBuyerSupplierdetails(row));
        //            }
        //            dsClone.AcceptChanges();
        //            XmlDocument doc = new XmlDocument();
        //            doc.LoadXml(dsClone.GetXml());
        //            json = Totalrows + "||" + Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
        //        }
        //    }
        //    return json;
        //}


        //added by kalpita on 03/01/2018
        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static string GetAuditLog_AddressID(List<string> slftrdet)
        //{
        //    string json = "", cWHERECOND = "", dtFrom = "", dtto = "";
        //    int Totalrows = 0; int TO = 0, FROM = 0, ADDRESSID = 0;
        //    MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
        //    List<string> slvalues = new List<string>(); slvalues.Clear(); List<string[]> _lstdet = _def.ConvertListToListArray(slftrdet);
        //    if (_lstdet != null && _lstdet.Count > 0)
        //    {
        //        slvalues.Clear();
        //        for (int i = 0; i < _lstdet.Count; i++)
        //        {
        //            string lstkey = _lstdet[i][0];
        //            slvalues.Add(lstkey);
        //        }
        //        Dictionary<string, string> _dictexp = _def.ConvertListToDictionary(slvalues);
        //        foreach (string key in _dictexp.Keys)
        //        {
        //            switch (key.ToUpper())
        //            {
        //                case "LOG_FROM": dtFrom = Convert.ToDateTime(_dictexp[key]).ToString("yyyy-MM-dd 00:00:00");
        //                    break;
        //                case "LOG_TO": dtto = Convert.ToDateTime(_dictexp[key]).ToString("yyyy-MM-dd 23:59:59");
        //                    break;
        //                case "FROM": FROM = Convert.ToInt32(_dictexp[key]);
        //                    break;
        //                case "TO": TO = Convert.ToInt32(_dictexp[key]);
        //                    break;
        //                case "ADDRESSID": ADDRESSID = Convert.ToInt32(_dictexp[key]);
        //                    break;
        //            }
        //        }
        //    }
        //    SupplierRoutines _Routine = new SupplierRoutines();
        //    DataSet _ds = _Routine.GetAuditLog_AddressID(ADDRESSID, Convert.ToDateTime(dtFrom), Convert.ToDateTime(dtto), cWHERECOND, FROM, TO, out Totalrows);
        //    if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
        //    {
        //        GetFilterCriteria(_ds); DataSet dsClone = _ds.Copy();
        //        dsClone.Tables[0].Columns.Add("UPDATE_DATE");
        //        foreach (DataRow row in dsClone.Tables[0].Rows)
        //        {
        //            foreach (DataColumn column in dsClone.Tables[0].Columns)
        //            {
        //                if (column.ColumnName == "UPDATEDATE")
        //                {
        //                    string value = (!string.IsNullOrEmpty(row.ItemArray[column.Ordinal].ToString())) ? Convert.ToDateTime(row.ItemArray[column.Ordinal]).ToString("dd/MM/yyyy") : null;
        //                    row.SetField("UPDATE_DATE", value);
        //                }
        //            }
        //        }
        //        dsClone.AcceptChanges();
        //        XmlDocument doc = new XmlDocument();
        //        doc.LoadXml(dsClone.GetXml());
        //        json = Totalrows + "||" + Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
        //    }
        //    return json;
        //}


        #endregion


        #region Existing leSmonitor
   
        //private void ShowFileData()
        //{
        //    bool AddData = false;
        //    if (hdLogID.Count > 0)
        //    {
        //        try
        //        {
        //            int LogID = Convert.ToInt32(hdLogID["LOGID"]);
        //            if (Convert.ToInt32(Session["LOG_ID"]) != LogID)
        //            {
        //                AddData = true;
        //                txtFile.Text = "";
        //                lblError.Text = "";
        //                Session["LOG_ID"] = LogID;
        //            }

        //            SupplierRoutines _Routine = new SupplierRoutines();
        //            lstAudit = _Routine.GetAuditDetails(LogID);

        //            if (lstAudit.Count > 0)
        //            {
        //                string filename = lstAudit["FILE_NAME"];
        //                string ModuleName = convert.ToString(lstAudit["MODULE_NAME"]).Trim();
        //                string LogType = convert.ToString(lstAudit["LOG_TYPE"]).Trim();
        //                string cBuyerID = convert.ToString(lstAudit["BUYER_ID"]).Trim();
        //                string cSuppID = convert.ToString(lstAudit["SUPPLIER_ID"]).Trim();
        //                lblFileName.Text = filename;

        //                try
        //                {
        //                    //string filePath = System.Configuration.ConfigurationManager.AppSettings["AUDIT_DOC_PATH"] + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMM");
        //                    string filePath = SearchFileOnServer(convert.ToInt(cBuyerID), convert.ToInt(cSuppID), ModuleName.Trim(), LogType.Trim(), filename);
        //                    string fileExt = Path.GetExtension(filename).Replace('.', ' ').Trim().ToLower();

        //                    if (!File.Exists(filePath + "\\" + filename))
        //                    {
        //                        lblError.Text = "File not found to resubmit";
        //                        txtFile.Visible = true;
        //                        btnResubmit.Enabled = false;
        //                    }
        //                    else if (File.Exists(filePath + "\\" + filename) && (fileExt == "txt" || fileExt == "xml"))
        //                    {
        //                        StreamReader sr = new StreamReader(filePath + "\\" + filename);
        //                        string FileData = sr.ReadToEnd();
        //                        sr.Close();
        //                        sr.Dispose();

        //                        if (AddData)
        //                        {
        //                            txtFile.Text = FileData;
        //                        }
        //                        txtFile.Visible = true;
        //                    }
        //                    else
        //                    {
        //                        txtFile.Visible = false;
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    ShowMsg(ex.Message);
        //                }
        //            }
        //        }
        //        catch
        //        { }
        //        finally
        //        {
        //            hdLogID.Clear();
        //        }
        //    }
        //}

        //private string SearchFileOnServer(int Buyerid, int Suppid, string ModuleName, string LogType, string FileName)
        //{
        //    try
        //    {
        //        string orgFilePath = "";

        //        // Search in eSupplier Inbox & Outbox & LesXML Inbox
        //        string eSupplierInbox = convert.ToString(ConfigurationManager.AppSettings["ESUPPLIER_INBOX"]).Trim();
        //        string eSupplierOutbox = convert.ToString(ConfigurationManager.AppSettings["ESUPPLIER_OUTBOX"]).Trim();
        //        string LesXmlInbox = convert.ToString(ConfigurationManager.AppSettings["LES_XML_INBOX"]).Trim();

        //        //DirectoryInfo fileDirInfo = null;
        //        orgFilePath = GetFilePath(eSupplierInbox, FileName.Trim());
        //        if (orgFilePath.Trim().Length == 0)
        //        {
        //            orgFilePath = GetFilePath(LesXmlInbox, FileName.Trim());
        //        }

        //        // Search By 
        //        if (orgFilePath.Trim().Length == 0)
        //        {
        //            SmBuyerSupplierLinkCollection linkColl = new SmBuyerSupplierLinkCollection();
        //            if (Buyerid > 0 && Suppid > 0)
        //            {
        //                linkColl = SmBuyerSupplierLink.Select_SM_BUYER_SUPPLIER_LINKs_By_BUYERID_SUPPLIERID(Buyerid, Suppid);
        //                foreach (SmBuyerSupplierLink linkObj in linkColl)
        //                {
        //                    orgFilePath = GetFilePath(convert.ToString(linkObj.ImportPath), FileName.Trim());
        //                    if (orgFilePath.Trim().Length == 0)
        //                    {
        //                        orgFilePath = GetFilePath(convert.ToString(linkObj.ExportPath), FileName.Trim());
        //                        if (orgFilePath.Trim().Length == 0)
        //                        {
        //                            orgFilePath = GetFilePath(convert.ToString(linkObj.VendorAddress.AddrInbox), FileName.Trim());
        //                            if (orgFilePath.Trim().Length == 0)
        //                            {
        //                                orgFilePath = GetFilePath(convert.ToString(linkObj.VendorAddress.AddrOutbox), FileName.Trim());
        //                                if (orgFilePath.Trim().Length == 0)
        //                                {
        //                                    orgFilePath = GetFilePath(convert.ToString(linkObj.SuppImportPath), FileName.Trim());
        //                                    if (orgFilePath.Trim().Length == 0)
        //                                    {
        //                                        orgFilePath = GetFilePath(convert.ToString(linkObj.SuppExportPath), FileName.Trim());
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    if (orgFilePath.Trim().Length > 0) break;
        //                }
        //            }
        //        }

        //        if (orgFilePath.Trim().Length == 0)
        //        {
        //            string grpCode = ModuleName.Trim().ToUpper().Replace("_RFQ", "")
        //                 .Replace("_QUOTE", "")
        //                 .Replace("_ORDER", "")
        //                 .Replace("_PO", "")
        //                 .Replace("_RFQAck", "")
        //                 .Replace("_POC", "")
        //                 .Replace("_POAck", "")
        //                 .Replace("_ORDERAck", "");

        //            SmBuyerSupplierLinkCollection grpLinkCollection = SmBuyerSupplierLink.Select_SM_BUYER_SUPPLIER_LINKs_By_GROUP_CODE(grpCode.Trim());
        //            if (grpLinkCollection.Count > 0)
        //            {
        //                foreach (SmBuyerSupplierLink linkObj in grpLinkCollection)
        //                {
        //                    orgFilePath = GetFilePath(convert.ToString(linkObj.ImportPath), FileName.Trim());
        //                    if (orgFilePath.Trim().Length == 0)
        //                    {
        //                        orgFilePath = GetFilePath(convert.ToString(linkObj.ExportPath), FileName.Trim());
        //                        if (orgFilePath.Trim().Length == 0)
        //                        {
        //                            orgFilePath = GetFilePath(convert.ToString(linkObj.VendorAddress.AddrInbox), FileName.Trim());
        //                            if (orgFilePath.Trim().Length == 0)
        //                            {
        //                                orgFilePath = GetFilePath(convert.ToString(linkObj.VendorAddress.AddrOutbox), FileName.Trim());
        //                                if (orgFilePath.Trim().Length == 0)
        //                                {
        //                                    orgFilePath = GetFilePath(convert.ToString(linkObj.SuppImportPath), FileName.Trim());
        //                                    if (orgFilePath.Trim().Length == 0)
        //                                    {
        //                                        orgFilePath = GetFilePath(convert.ToString(linkObj.SuppExportPath), FileName.Trim());
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    if (orgFilePath.Trim().Length > 0) break;
        //                }
        //            }
        //            else
        //            {
        //                SmAddress addr = SmAddress.LoadByGroup(grpCode.Trim());
        //                if (addr != null && addr.Addressid > 0)
        //                {
        //                    orgFilePath = GetFilePath(convert.ToString(addr.AddrInbox), FileName.Trim());
        //                    if (orgFilePath.Trim().Length == 0)
        //                    {
        //                        orgFilePath = GetFilePath(convert.ToString(addr.AddrOutbox), FileName.Trim());
        //                    }
        //                }
        //            }
        //        }

        //        if (orgFilePath.Trim().Length == 0)
        //        {
        //            // Seach on PDF & XLS Path
        //            if (ModuleName.Trim() == "PDF")
        //            {
        //                string PDFCommonPath = convert.ToString(ConfigurationManager.AppSettings["PDF_COMMON_PATH"]).Trim();
        //                orgFilePath = GetFilePath(PDFCommonPath.Trim(), FileName.Trim());
        //            }
        //        }

        //        return orgFilePath;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }
        //}

     
        #endregion


    }
}