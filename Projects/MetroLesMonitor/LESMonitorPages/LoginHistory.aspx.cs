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
    public partial class LoginHistory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetLoginHistoryGrid(string UPDATE_DATEFROM, string UPDATE_DATETO)
        {
            string json = "";
            string dtFrom = Convert.ToDateTime(UPDATE_DATEFROM).ToString("yyyy-MM-dd 00:00:00");
            string dtto = Convert.ToDateTime(UPDATE_DATETO).ToString("yyyy-MM-dd 23:59:59");

            System.Data.DataSet _ds = new DataSet();

            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.GetLoginHistory(Convert.ToDateTime(dtFrom), Convert.ToDateTime(dtto));
            DataSet dsClone = _ds.Copy();
            dsClone.Tables[0].Columns.Add("UPDATED_DATE");
            foreach (DataRow row in dsClone.Tables[0].Rows)
            {
                foreach (DataColumn column in dsClone.Tables[0].Columns)
                {
                    if (column.ColumnName == "UPDATE_DATE")
                    {
                        string value = Convert.ToDateTime(row.ItemArray[9]).ToString("dd/MM/yyyy");
                        row.SetField("UPDATED_DATE", value);
                    }
                }
            }
            dsClone.AcceptChanges();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(dsClone.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);

            return json;
        }

    }
}