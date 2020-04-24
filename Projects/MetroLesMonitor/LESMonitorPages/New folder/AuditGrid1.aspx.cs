using MetroLesMonitor.Bll;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroLesMonitor.LESMonitorPages
{
    public partial class AuditGrid : System.Web.UI.Page
    {
        public string cWHERECOND = "", dtFrom = "", dtTo = "", cFilterstr = "", cCOND = "";
        public  DateTime cToday = DateTime.Now.Date;
        public int ncount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {           
            if (!IsPostBack)
            {
                dtFrom = Convert.ToDateTime(cToday).ToString("yyyy-MM-dd 00:00:00"); dtTo = Convert.ToDateTime(cToday).ToString("yyyy-MM-dd 23:59:59");
                Bind();
            }
        }

        void Bind()
        {
            SupplierRoutines _routines = new SupplierRoutines();
            List<string> slFilter = new List<string>();
            SetValues();
            string cFin_Cond = cWHERECOND;
            DataTable dataTable = new DataTable();

            try
            {
                DataSet ds = _routines.GetAuditLog_Filter(cFin_Cond, dtFrom, dtTo);
                if (ds != null && ds.Tables.Count > 0)
                {
                    
                    dataTable = ds.Tables[0];
                    Session["AuditData"] = dataTable;
                    gvData.DataSource = dataTable;             
                    gvData.DataBind();
                }
                Literal1.Text = dataTable.Rows.Count.ToString();
             
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private List<string> ConvertString(string cFCond)
        {
            List<string> slList = new List<string>(); slList.Clear();
            string cRes = "";
            if (!string.IsNullOrEmpty(cFCond))
            {
                cRes = cFCond.Replace("[", "").Replace("]", "").Replace("\"", "");
                slList = cRes.Split(',').ToList();
            }
            return slList;
        }

        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl = (DropDownList)gvData.HeaderRow.FindControl("ddlModule");
                BindCombo(ddl);
                e.Row.Cells[3].Attributes.Add("style", "word-break:break-all;word-wrap:break-word;");
                e.Row.Cells[9].Attributes.Add("style", "word-break:break-all;word-wrap:break-word;");
                e.Row.Cells[10].Attributes.Add("style", "word-break:break-all;word-wrap:break-word;");
            }
        }

        private void BindCombo(DropDownList ddl)
        {
            //if (ddl != null && ddl.Items.Count == 0)
            //{
            //    ddl.DataSourceID = string.Empty;
            //    ddl.DataSource = (DataSet)ViewState["_dsMouleList"];

            //    ddl.DataValueField = "MODULENAME";
            //    ddl.DataTextField = "MODULENAME";
            //    ddl.DataBind();
            //}
            string ddlVal = convert.ToString(ViewState["ddlModuleValue"]);
            if (ddl != null && ddl.Items.Count == 0 && string.IsNullOrEmpty(ddlVal))
            {
                DataTable dataTable = (DataTable)Session["AuditData"];
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    DataTable dtView = dataTable.DefaultView.ToTable(true, "MODULENAME");
                    dtView.DefaultView.Sort = "MODULENAME ASC";
                    DataSet dsModule = new DataSet();
                    dsModule.Tables.Add(dtView.DefaultView.ToTable());
                    ddl.DataSourceID = string.Empty;
                    ddl.DataSource = dsModule;

                    ddl.DataValueField = "MODULENAME";
                    ddl.DataTextField = "MODULENAME";
                    ddl.DataBind();

                    ddl.Items.FindByText(Convert.ToString(ViewState["ddlModuleValue"])).Selected = true;
                }
            }
        }

        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvData.PageIndex = e.NewPageIndex;
            Bind();
        }

        protected void txtHeaderChanged(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox txtCtrl = (TextBox)sender;
                switch (txtCtrl.ID)
                {
                    case "txtBuyerCode": cWHERECOND += " AND  BUYERCODE LIKE '%" + txtCtrl.Text + "%'"; ViewState["txtBuyerCode"] = txtCtrl.Text;
                        break;
                    case "txtServerName": cWHERECOND += " AND SERVERNAME LIKE '%" + txtCtrl.Text + "%'"; ViewState["txtServerName"] = txtCtrl.Text;
                        break;
                    case "txtDocType": cWHERECOND += " AND DOCTYPE LIKE '%" + txtCtrl.Text + "%'"; ViewState["txtDocType"] = txtCtrl.Text;
                        break;
                    case "txtLOGTYPE": cWHERECOND += " AND LOGTYPE LIKE '%" + txtCtrl.Text + "%'"; ViewState["txtLOGTYPE"] = txtCtrl.Text;
                        break;
                    case "txtSupplierCode": cWHERECOND += " AND  SUPPLIERCODE LIKE '%" + txtCtrl.Text + "%'"; ViewState["txtSupplierCode"] = txtCtrl.Text;
                        break;
                    case "txtKeyRef": cWHERECOND += " AND KEYREF2 LIKE '%" + txtCtrl.Text + "%'"; ViewState["txtKeyRef"] = txtCtrl.Text;
                        break;
                    case "txtRemark": cWHERECOND += " AND AUDITVALUE LIKE '%" + txtCtrl.Text + "%'"; ViewState["txtRemark"] = txtCtrl.Text;
                        break;
                    case "txtFileName": cWHERECOND += " AND FILENAME LIKE '%" + txtCtrl.Text + "%'"; ViewState["txtFileName"] = txtCtrl.Text;
                        break;
                }
            }
            else if (sender is DropDownList)
            {
                DropDownList ddlCtrl = (DropDownList)sender;
                switch (ddlCtrl.ID)
                {
                    case "ddlModule": cWHERECOND += " AND MODULENAME LIKE '%" + ddlCtrl.SelectedValue + "%'"; ViewState["ddlModuleValue"] = ddlCtrl.SelectedValue;
                        break;
                }
            }
            else if (sender is Button)
            {
                string[] str = Hidden1.Value.Split('|');   ViewState["dtLogFromDate"] = dtFrom = str[0]; ViewState["dtLogToDate"] = dtTo = str[1];
            }
            Bind(); 
        }

        public void GetModuleCondition()
        {
            string ddlval = Convert.ToString(ViewState["ddlModuleValue"]);
            if (!string.IsNullOrEmpty(ddlval)) cWHERECOND += " AND MODULENAME LIKE '%" + ddlval + "%'";
        }

        private void SetValues()
        {           
            if (ViewState["txtBuyerCode"] != null)
                ((TextBox)gvData.HeaderRow.FindControl("txtBuyerCode")).Text = ViewState["txtBuyerCode"].ToString();
            if (ViewState["txtSupplierCode"] != null)
                ((TextBox)gvData.HeaderRow.FindControl("txtSupplierCode")).Text = ViewState["txtSupplierCode"].ToString();
            if (ViewState["txtLOGTYPE"] != null)
                ((TextBox)gvData.HeaderRow.FindControl("txtLOGTYPE")).Text = ViewState["txtLOGTYPE"].ToString();        
            if (ViewState["txtDocType"] != null)
                ((TextBox)gvData.HeaderRow.FindControl("txtDocType")).Text = ViewState["txtDocType"].ToString();
            if (ViewState["txtServerName"] != null)
                ((TextBox)gvData.HeaderRow.FindControl("txtServerName")).Text = ViewState["txtServerName"].ToString();
            if (ViewState["txtKeyRef"] != null)
                ((TextBox)gvData.HeaderRow.FindControl("txtKeyRef")).Text = ViewState["txtKeyRef"].ToString();
            if (ViewState["txtRemark"] != null)
                ((TextBox)gvData.HeaderRow.FindControl("txtRemark")).Text = ViewState["txtRemark"].ToString();
            if (ViewState["txtFileName"] != null)
                ((TextBox)gvData.HeaderRow.FindControl("txtFileName")).Text = ViewState["txtFileName"].ToString();
            dtFrom = (!string.IsNullOrEmpty(Convert.ToString(ViewState["dtLogFromDate"]))) ? Convert.ToString(ViewState["dtLogFromDate"]) : Convert.ToDateTime(cToday).ToString("yyyy-MM-dd");
            dtTo = (!string.IsNullOrEmpty(Convert.ToString(ViewState["dtLogToDate"]))) ? Convert.ToString(ViewState["dtLogToDate"]) : Convert.ToDateTime(cToday).ToString("yyyy-MM-dd");
            GetModuleCondition();
        }

        private void Clear()
        {
            if (ViewState["txtBuyerCode"] != null) ViewState["txtBuyerCode"] = null;
            if (ViewState["txtSupplierCode"] != null) ViewState["txtSupplierCode"] = null;
            if (ViewState["txtLOGTYPE"] != null) ViewState["txtLOGTYPE"] = null;
            if (ViewState["ddlModuleValue"] != null) ViewState["ddlModuleValue"] = null;
            if (ViewState["txtDocType"] != null) ViewState["txtDocType"] = null;
            if (ViewState["txtServerName"] != null) ViewState["txtServerName"] = null;
            if (ViewState["txtKeyRef"] != null) ViewState["txtKeyRef"] = null;
            if (ViewState["txtRemark"] != null) ViewState["txtRemark"] = null;
            if (ViewState["txtFileName"] != null) ViewState["txtFileName"] = null;
            if (ViewState["dtLogFromDate"] != null) ViewState["dtLogFromDate"] = null;
            if (ViewState["dtLogToDate"] != null) ViewState["dtLogToDate"] = null;
            Bind(); dtFrom = Convert.ToDateTime(cToday).ToString("yyyy-MM-dd"); dtTo = Convert.ToDateTime(cToday).ToString("yyyy-MM-dd");
            Bind();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "<script language=javascript>self.close();</script>");
        }

        protected void gvData_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvData.Rows[index];
                string _updateDate = ((Label)row.Cells[0].FindControl("lblUpdateDate")).Text;
                 
                string _filename = ((LinkButton)row.Cells[10].FindControl("lnkFile")).Text;

                string _serverName = ((Label)row.Cells[1].FindControl("lblServerName")).Text;
                string _module = ((Label)row.Cells[3].FindControl("lblModule")).Text;
                DownloadFile(_updateDate, _module, _filename, _serverName);
            }
        }

        public  string DownloadFile(string UPDATEDATE, string MODULENAME, string FILENAME, string SERVERNAME)
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
                            HttpContext.Current.Response.Clear();
                            HttpContext.Current.Response.ContentType = "application/octet-stream";
                            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + FILENAME);
                            HttpContext.Current.Response.AddHeader("Content-Length", Convert.ToString(_Data.Length));
                            HttpContext.Current.Response.Flush();
                            HttpContext.Current.Response.BinaryWrite(_Data);
                            HttpContext.Current.Response.End();
                        }
                        else
                        {
                            Label lblmsg = new Label();
                            lblmsg.Text = "<script language='javascript'>alert('File " + FILENAME + " not found.')</script> ";
                            this.Page.Controls.Add(lblmsg);
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

        public  string DownloadFileMainserver(string _path, string filename, string module, string updateDate, string destfile)
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
               HttpContext.Current.Response.Clear();
               HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
               HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
               HttpContext.Current.Response.ContentType = "application/octet-stream";
               HttpContext.Current.Response.Flush();
               HttpContext.Current.Response.WriteFile(file.FullName);
               HttpContext.Current.Response.End();
            }
            else
            {
                Label lblmsg = new Label();
                lblmsg.Text = "<script language='javascript'>alert('File "+ filename + " not found.')</script> ";
                this.Page.Controls.Add(lblmsg);
            }
            return cdownload;
        }

        public  string GetServerURL(string _serverName)
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

        private  string SearchInDirectory(string _Path, string _File)
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

        private  string SearchFileOnServer(int Buyerid, int Suppid, string ModuleName, string LogType, string FileName)
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

        public  string GetFilePath(string DirectoryName, string FileName)
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

        private  string Get_Download_Filename(string filename, string module)
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

        private  string GetCorrectPath(string _Path, string _File)
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

        private  void DeleteFiles(string DestFile)
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
    }
}