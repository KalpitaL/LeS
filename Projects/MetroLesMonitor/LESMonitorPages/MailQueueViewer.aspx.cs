using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace MetroLesMonitor.LESMonitorPages
{
    public partial class MailQueueViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetMailQueueViewerGrid()
        {
            string json = "",cStatus="";
            System.Data.DataSet _ds = new DataSet();

            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.GetMailQueueData();
            DataSet dsClone = _ds.Copy();
            dsClone.Tables[0].Columns.Add("MAILDATE");
            dsClone.Tables[0].Columns.Add("STATUS");
            foreach (DataRow row in dsClone.Tables[0].Rows)
            {
                foreach (DataColumn column in dsClone.Tables[0].Columns)
                {
                    if (column.ColumnName == "MAIL_DATE")
                    {
                        string value = Convert.ToDateTime(row.ItemArray[12]).ToString("dd-MM-yyyy HH:mm:ss");
                        row.SetField("MAILDATE", value);
                    }
                    else if (column.ColumnName == "NOT_TO_SENT")
                    {
                        string value = Convert.ToString(row.ItemArray[13]);
                        if (value == "0") { cStatus = "In Queue"; }
                        else if (value == "1") { cStatus = "Not to send"; }
                        row.SetField("STATUS", cStatus);
                    }
                }
            }
            dsClone.AcceptChanges();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(dsClone.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);

            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string UpdateMailDetails(List<string> slMaildet)
        {
            string json = "";
            string oQueueID = "", oMailTo = "", oMailCc = "", oMailBcc = "", oMailSubject = "", oMailBody = "", oMailAttach = "", oMailStatus = "", nQueueID = "",
            nMailTo = "", nMailCc = "", nMailBcc = "", nMailSubject = "", nMailBody = "", nMailAttach = "", nMailStatus = "", _updatedData = "";
            string cMail_status = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            List<string> slresvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slMaildet);

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
                            case "O_MAILTO": oMailTo = _dictexp[key];
                                break;
                            case "O_MAILCC": oMailCc = _dictexp[key];
                                break;
                            case "O_MAILBCC": oMailBcc = _dictexp[key];
                                break;
                            case "O_MAILSUB": oMailSubject = _dictexp[key];
                                break;
                            case "O_MAILBODY": oMailBody = _dictexp[key];
                                break;
                            case "O_MAILATTACH": oMailAttach = _dictexp[key];
                                break;
                            case "O_MAILSTATUS": oMailStatus = _dictexp[key];
                                break;
                            case "O_QUEUEID": oQueueID = _dictexp[key];
                                break;
                            case "N_MAILTO": nMailTo = _dictexp[key];
                                break;
                            case "N_MAILCC": nMailCc = _dictexp[key];
                                break;
                            case "N_MAILBCC": nMailBcc = _dictexp[key];
                                break;
                            case "N_MAILSUB": nMailSubject = _dictexp[key];
                                break;
                            case "N_MAILBODY": nMailBody = _dictexp[key];
                                break;
                            case "N_MAILATTACH": nMailAttach = _dictexp[key];
                                break;
                            case "N_MAILSTATUS": nMailStatus = _dictexp[key];
                                cMail_status = (nMailStatus.ToUpper() == "IN QUEUE") ? "0" : "1";
                                break;
                            case "N_QUEUEID": nQueueID = _dictexp[key];
                                break;
                        }
                    }                 
                    string _result = _Routine.UpdateMailQueueDetails(nQueueID, nMailTo, nMailCc, nMailBcc, nMailSubject, nMailBody, nMailAttach, cMail_status);
                    if (Convert.ToInt32(_result) > 0)
                    {
                        _Routine.SetUpdateLog(ref _updatedData, "Mail To", convert.ToString(oMailTo), convert.ToString(nMailTo));
                        _Routine.SetUpdateLog(ref _updatedData, "Mail Cc", convert.ToString(oMailCc), convert.ToString(nMailCc));
                        _Routine.SetUpdateLog(ref _updatedData, "Mail Bcc", convert.ToString(oMailBcc), convert.ToString(oMailBcc));
                        _Routine.SetUpdateLog(ref _updatedData, "Subject", convert.ToString(oMailSubject), convert.ToString(nMailSubject));
                        _Routine.SetUpdateLog(ref _updatedData, "Mail Body", convert.ToString(oMailBody), convert.ToString(nMailBody));
                        _Routine.SetUpdateLog(ref _updatedData, "Attachment", convert.ToString(oMailAttach), convert.ToString(nMailAttach));
                        _Routine.SetUpdateLog(ref _updatedData, "Status", convert.ToString(oMailStatus), convert.ToString(nMailStatus));
                        if (_updatedData.Trim().Length == 0)
                        {
                            _updatedData = "No changes made";
                        }
                        _Routine.SetAuditLog("LesMonitor", "Mail queue updated successfully " + _updatedData + " . by : " + Convert.ToString(HttpContext.Current.Session["UserHostServer"]), "Updated", "", "", "", "");
                        json = "1";
                    }
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
        public static string DeleteMailDetails(List<string> slMaildet)
        {
            string json = "";
            string QueueID = "", MailTo = "", MailCc = "", MailBcc = "", MailSubject = "", MailBody = "", MailAttach = "", MailStatus = "" , _updatedData = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slMaildet);
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
                            case "O_MAILTO": MailTo = _dictexp[key];
                                break;
                            case "O_MAILCC": MailCc = _dictexp[key];
                                break;
                            case "O_MAILBCC": MailBcc = _dictexp[key];
                                break;
                            case "O_MAILSUB": MailSubject = _dictexp[key];
                                break;
                            case "O_MAILBODY": MailBody = _dictexp[key];
                                break;
                            case "O_MAILATTACH": MailAttach = _dictexp[key];
                                break;
                            case "O_MAILSTATUS": MailStatus = _dictexp[key];
                                break;
                            case "O_QUEUEID": QueueID = _dictexp[key];
                                break;
                        }
                    }
                    string _result = _Routine.DeleteMailQueueDetails(QueueID);
                    if (Convert.ToInt32(_result) > 0)
                    {
                        _Routine.SetDataLog(ref _updatedData, "Mail To", convert.ToString(MailTo));
                        _Routine.SetDataLog(ref _updatedData, "Mail Cc", convert.ToString(MailCc));
                        _Routine.SetDataLog(ref _updatedData, "Mail Bcc", convert.ToString(MailBcc));
                        _Routine.SetDataLog(ref _updatedData, "Subject", convert.ToString(MailSubject));
                        _Routine.SetDataLog(ref _updatedData, "Mail Body", convert.ToString(MailBody));
                        _Routine.SetDataLog(ref _updatedData, "Attachment", convert.ToString(MailAttach));
                        _Routine.SetDataLog(ref _updatedData, "Status", convert.ToString(MailStatus));

                        _Routine.SetAuditLog("LesMonitor", "Mail Queue deleted successfully for " + _updatedData + " . by : " + Convert.ToString(HttpContext.Current.Session["UserHostServer"]), "Deleted", "", "", "", "");
                        json = "1";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return json;
        }

    }
}