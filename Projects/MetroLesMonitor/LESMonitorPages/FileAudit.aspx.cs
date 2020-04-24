using System;
using System.Collections.Generic;
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
    public partial class FileAudit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetFileAuditGrid(string SELECTFILTER, string LOG_DATEFROM, string LOG_DATETO, string SEARCH)
        {
            string json = "", SELECTCOND="";
            string dtFrom = Convert.ToDateTime(LOG_DATEFROM).ToString("yyyy-MM-dd 00:00:00");
            string dtto = Convert.ToDateTime(LOG_DATETO).ToString("yyyy-MM-dd 23:59:59");

            System.Data.DataSet _ds = new DataSet();

            SupplierRoutines _Routine = new SupplierRoutines();
//SELECTCOND = (SEARCH == "") ? " TOP 500" : string.Empty;
            //if (!string.IsNullOrEmpty(SELECTFILTER))
            {
                //if (SELECTFILTER == "0")
                //{
                //    _ds = _Routine.GetFileAudit(Convert.ToDateTime(dtFrom), Convert.ToDateTime(dtto));
                //}
                //else
                //{
                    string ccboSearchCtr = GetSearchQueryExp(SELECTFILTER);

                    _ds = _Routine.GetFileAudit_FilterDet(Convert.ToDateTime(dtFrom), Convert.ToDateTime(dtto), ccboSearchCtr, SEARCH, SELECTCOND);
                //}

                _ds.Tables[0].Columns.Add("UPDATEDATE");

                foreach (DataRow row in _ds.Tables[0].Rows)
                {
                    row["UPDATEDATE"] = Convert.ToDateTime(row["UPDATE_DATE"]).ToString("dd/MM/yyyy");
                }

                _ds.AcceptChanges();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_ds.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);

               // json = Newtonsoft.Json.JsonConvert.SerializeObject(_ds.Tables[0]);
            }
            return json;
        }


        private  static string GetSearchQueryExp(string SelectFilterIndx)
        {
            string Filter = "";
            switch (SelectFilterIndx)
            {
                case "1":  Filter = " AND RFQ_EXP is Null AND RFQ_ID > 0 AND RFQ_END_STATE > 2 AND QUOTE_ID is Null AND PO_ID is Null AND POC_ID is Null ";
                    break;
                case "2": Filter = " AND RFQ_UPLOAD is Null AND RFQ_ID > 0 AND RFQ_END_STATE > 3 AND QUOTE_ID is Null AND PO_ID is Null AND POC_ID is Null ";                  
                    break;
                case "3": Filter = " AND QUOTE_EXP is Null AND QUOTE_ID > 0 AND QUOTE_END_STATE > 2 AND PO_ID is Null AND POC_ID is Null";
                    break;
                case "4": Filter = " AND QUOTE_UPLOAD is Null AND QUOTE_ID > 0 AND QUOTE_END_STATE > 3 AND PO_ID is Null AND POC_ID is Null";
                    break;
                case "5": Filter = " AND PO_EXP is Null AND PO_ID > 0 AND PO_END_STATE > 2 AND POC_ID is Null";                   
                    break;
                case "6":  Filter = " AND PO_UPLOAD is Null AND PO_ID > 0 AND PO_END_STATE > 3 AND POC_ID is Null";
                    break;
                case "7": Filter = " AND POC_EXP is Null AND POC_ID > 0 AND POC_END_STATE > 2";                  
                    break;
                case "8": Filter = " AND POC_UPLOAD is Null AND POC_ID > 0 AND POC_END_STATE > 3";                   
                    break;
            }
            return Filter;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string ViewDetailedInfo(string RECORDID)
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();

            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(RECORDID))
            {
                _ds = _Routine.GetFileAuditInfo(Convert.ToInt32(RECORDID));
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }
    }
}