using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroLesMonitor.LESMonitorPages
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        DateTemplate dateTemplate;
        SupplierRoutines _Routine = new SupplierRoutines();

        protected void Page_Load(object sender, EventArgs e)
        {
          if(!IsPostBack)
          {
              FillAuditLog();
              gvAuditLog.AutoFilterByColumn(gvAuditLog.Columns["UPDATEDATE"], (DateTime.Now.ToShortDateString() + " | " + DateTime.Now.ToString()));
          }
        }


        private void FillAuditLog()
        {
            try
            {
                int AddressID = Convert.ToInt32(HttpContext.Current.Session["ADDRESSID"]);
                if (Session["FROM_DATE"] != null && Session["TO_DATE"] != null)
                {
                    DateTime dateFrom = Convert.ToDateTime(Session["FROM_DATE"]);
                    DateTime dateTo = Convert.ToDateTime(Session["TO_DATE"]);
                    gvAuditLog.DataSourceID = string.Empty;
                    DataSet ds = _Routine.GetAuditLog(AddressID, dateFrom, dateTo);
                    gvAuditLog.DataSource = ds;
                    _Routine.SetLog("Dataset populated form " + dateFrom + " To " + dateTo);
                    Session["AuditDataSet"] = ds;
                    DataTable dt = null;
                    DataTable dtView = ds.Tables[0].DefaultView.ToTable(true, "MODULENAME");
                    dtView.DefaultView.Sort = "MODULENAME ASC";
                    dt = dtView.DefaultView.ToTable();
                    DataSet dsModule = new DataSet();
                    dsModule.Tables.Add(dt);
                    Session["_dsMouleList"] = dsModule;
                    gvAuditLog.DataBind();
                }
                else
                {
                    DateTime dtDate = DateTime.Now.AddYears(-1);
                    int _hour = convert.ToInt(dtDate.ToString("HH"));
                    int min = convert.ToInt(dtDate.ToString("mm"));
                    TimeSpan time = new TimeSpan(0, -_hour, -min, 0);

                    DateTime dateFrom = dtDate.Add(time);
                    DateTime dateTo = DateTime.Now;
                    int _dHour = 23 - _hour;
                    int _dmin = 59 - min;
                    TimeSpan time1 = new TimeSpan(0, _dHour, _dmin, 0);
                    dateTo = dateTo.Add(time1);
                    DataSet ds = _Routine.GetAuditLog_Top5000(AddressID, dateFrom, dateTo);
                    gvAuditLog.DataSource = ds;
                    _Routine.SetLog("Dataset populated form " + dateFrom + " To " + dateTo);
                    Session["AuditDataSet"] = ds;
                    DataTable dt = null;
                    DataTable dtView = ds.Tables[0].DefaultView.ToTable(true, "MODULENAME");
                    dtView.DefaultView.Sort = "MODULENAME ASC";
                    dt = dtView.DefaultView.ToTable();
                    DataSet dsModule = new DataSet();
                    dsModule.Tables.Add(dt);
                    Session["_dsMouleList"] = dsModule;
                    gvAuditLog.DataBind();
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        protected void ASPxGridView1_AutoFilterCellEditorCreate(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorCreateEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            if (e.Column.FieldName == "UPDATEDATE")
            {
                DropDownEditProperties dde = new DropDownEditProperties();
                dde.EnableClientSideAPI = true;
                dateTemplate = new DateTemplate();
                dde.DropDownWindowTemplate = dateTemplate;
                e.EditorProperties = dde;
            }
            //else if (e.Column.FieldName == "LOGTYPE" || e.Column.FieldName == "MODULENAME")
            //{
            //    e.EditorProperties = new ComboBoxProperties();
            //    e.EditorProperties.Style.Font.Name = "Verdana";
            //    e.EditorProperties.Style.Font.Size = new FontUnit("8pt");
            //}
            else if (e.Column.FieldName == "MODULENAME")
            {
                e.EditorProperties = new ComboBoxProperties();
                e.EditorProperties.Style.Font.Name = "Verdana";
                e.EditorProperties.Style.Font.Size = new FontUnit("8pt");
            }

        }


        protected void ASPxGridView1_AutoFilterCellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            try
            {
                ASPxGridView grid = sender as ASPxGridView;
                if (e.Column.FieldName == "UPDATEDATE")
                {
                    ASPxDropDownEdit dde = e.Editor as ASPxDropDownEdit;
                    string date = DateTime.Now.Date.Day.ToString();
                    string month = (DateTime.Now.Date.Month - 1).ToString();
                    string year = DateTime.Now.Date.Year.ToString();

                    //dde.ClientSideEvents.CloseUp = String.Format("function (s, e) {{ ApplyFilter(s, {0}, {1}, {2}, {3}, {4}); }}", dateTemplate.cIdFrom, dateTemplate.cIdTo, date, month, year); //Sayli 22Mar16
                    dde.ClientSideEvents.DropDown = String.Format("function (s, e) {{ OnDropDown(s, {0}, {1}, {2}, {3}, {4}); }}", dateTemplate.cIdFrom, dateTemplate.cIdTo, date, month, year);
                    dde.ReadOnly = false;
                }           
                else if (e.Column.FieldName == "MODULENAME")
                {
                    SupplierRoutines _Routine = new SupplierRoutines();
                    ASPxComboBox cboModule = (ASPxComboBox)e.Editor;
                    cboModule.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                    cboModule.DropDownStyle = DropDownStyle.DropDown;
                    cboModule.DataSourceID = string.Empty;
                    if (Session["_dsMouleList"] != null)
                    {
                        cboModule.DataSource = (DataSet)Session["_dsMouleList"];
                    }
                    cboModule.ValueField = "MODULENAME";
                    cboModule.TextField = "MODULENAME";
                    cboModule.DataBind();
                }
            }
            catch (Exception ex)
            {
               // _Routine.SetLog(ex.StackTrace);
            }
        }

        protected void ASPxGridView1_ProcessColumnAutoFilter(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs e)
        {
            try
            {
                int AddressID = Convert.ToInt32(Session["ADDRESSID"]);
                if (e.Value == "|")
                {
                    Session["UpdateDateFilter"] = (DateTime.Now.AddDays(-1).ToString() + " | " + DateTime.Now.ToString());
                    String[] dates = (DateTime.Now.AddDays(-1).ToString() + " | " + DateTime.Now.ToString()).Split('|');
                    DateTime dateFrom = Convert.ToDateTime(dates[0]), dateTo = Convert.ToDateTime(dates[1]);
                    e.Criteria = ((new DevExpress.Data.Filtering.OperandProperty("UPDATEDATE")) >= dateFrom) & ((new DevExpress.Data.Filtering.OperandProperty("UPDATEDATE")) <= dateTo);
                    return;
                }
                if (e.Column.FieldName == "UPDATEDATE")
                {
                    if (e.Kind == GridViewAutoFilterEventKind.CreateCriteria)
                    {
                        String[] dates = new string[2];
                        try
                        {
                            Session["UpdateDateFilter"] = e.Value;
                            dates = e.Value.Split('|');
                            DateTime dateFrom = Convert.ToDateTime(dates[0]), dateTo = Convert.ToDateTime(dates[1]);

                            if (dateFrom < DateTime.Now.AddDays(-2))
                            {
                                gvAuditLog.DataSourceID = string.Empty;
                                DataSet ds = null; //_Routine.GetAuditLog(AddressID, dateFrom, dateTo);
                                //_Routine.SetLog("Dataset populated form " + dateFrom + " To " + dateTo);
                                gvAuditLog.DataSource = ds;
                                Session["AuditDataSet"] = ds;
                                DataTable dt = null;
                                DataTable dtView = ds.Tables[0].DefaultView.ToTable(true, "MODULENAME");
                                dtView.DefaultView.Sort = "MODULENAME ASC";
                                dt = dtView.DefaultView.ToTable();
                                DataSet dsModule = new DataSet();
                                dsModule.Tables.Add(dt);
                                Session["_dsMouleList"] = dsModule;
                                gvAuditLog.DataBind();
                            }

                            Session["FROM_DATE"] = dateFrom;
                            Session["TO_DATE"] = dateTo;
                            e.Criteria = ((new DevExpress.Data.Filtering.OperandProperty("UPDATEDATE")) >= dateFrom) & ((new DevExpress.Data.Filtering.OperandProperty("UPDATEDATE")) <= dateTo);
                        }
                        catch (Exception) { }
                    }
                    else
                    {
                        if (Session["UpdateDateFilter"] != null) e.Value = Session["UpdateDateFilter"].ToString();
                    }
                }        
                else if (e.Column.FieldName == "MODULENAME")
                {
                    if (e.Kind == GridViewAutoFilterEventKind.CreateCriteria)
                    {
                        Session["MODULE_NAME"] = e.Value;
                        if (e.Value.Trim() != string.Empty) e.Criteria = (new DevExpress.Data.Filtering.OperandProperty("MODULENAME") == e.Value.Trim());
                        else e.Criteria = (new DevExpress.Data.Filtering.OperandProperty("MODULENAME") != e.Value.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
              //  _Routine.SetLog("Error : " + ex.Message + " Stack trace : " + ex.StackTrace);
//                ShowMsg(convert.ToString(ex.Message));
            }
        }
    }

    public class DateTemplate : ITemplate
    {
        // client IDs of two ASPxDateEdit controls
        public String cIdFrom;
        public String cIdTo;

        public void InstantiateIn(Control container)
        {
            Table table = new Table();
            container.Controls.Add(table);
            TableRow row = new TableRow();
            table.Rows.Add(row);

            TableCell cell = new TableCell();
            row.Cells.Add(cell);
            ASPxLabel lbl = new ASPxLabel();
            lbl.ID = "lblFrom";
            lbl.Text = "From:";
            cell.Controls.Add(lbl);

            cell = new TableCell();
            row.Cells.Add(cell);
            ASPxDateEdit dateFrom = new ASPxDateEdit();
            dateFrom.EditFormat = EditFormat.Custom;
            dateFrom.EditFormatString = "d";
            dateFrom.DisplayFormatString = "d";
            dateFrom.Font.Name = "Verdana";
            dateFrom.Font.Size = new FontUnit("8pt");
            dateFrom.ID = "dateFrom";
            dateFrom.EnableClientSideAPI = true;
            cell.Controls.Add(dateFrom);

            row = new TableRow();

            table.Rows.Add(row);

            cell = new TableCell();
            row.Cells.Add(cell);
            lbl = new ASPxLabel();
            lbl.ID = "lblTo";
            lbl.Text = "To:";
            cell.Controls.Add(lbl);

            cell = new TableCell();
            row.Cells.Add(cell);
            ASPxDateEdit dateTo = new ASPxDateEdit();
            dateTo.EditFormat = EditFormat.Custom;
            dateTo.EditFormatString = "d";
            dateTo.DisplayFormatString = "d";
            dateTo.Font.Name = "Verdana";
            dateTo.Font.Size = new FontUnit("8pt");
            dateTo.ID = "dateTo";
            dateTo.EnableClientSideAPI = true;
            cell.Controls.Add(dateTo);

            cIdFrom = dateFrom.ClientID;
            cIdTo = dateTo.ClientID;

            row = new TableRow();
            table.Rows.Add(row);

            cell = new TableCell();
            cell.ColumnSpan = 2;
            row.Cells.Add(cell);
            ASPxHyperLink lnk = new ASPxHyperLink();
            lnk.ID = "lnkApply";
            lnk.Text = "Apply";
            lnk.NavigateUrl = "javascript:void(0)";

            string date = DateTime.Now.Date.Day.ToString();
            string month = (DateTime.Now.Date.Month - 1).ToString();
            string year = DateTime.Now.Date.Year.ToString();

            lnk.ClientSideEvents.Click = String.Format("function (s, e) {{ {0}.HideDropDown(); ApplyFilter(s, {1}, {2}, {3}, {4}, {5}); }}",
                container.NamingContainer.NamingContainer.ClientID, cIdFrom, cIdTo, date, month, year);
            cell.Controls.Add(lnk);
        }
    }
}