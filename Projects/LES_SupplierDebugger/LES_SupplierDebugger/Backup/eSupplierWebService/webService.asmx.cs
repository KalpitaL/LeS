using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web.Services;
using System.Xml.Serialization;
using eSupplierDataMain;
using System.IO;
using MTML.GENERATOR;
using System.Web;

namespace eSupplierWebService
{
    /// <summary>
    /// Summary description for eSupplierService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class eSupplierService : System.Web.Services.WebService
    {
        eSupplierDataMain.SMData_Routines _smRoutine = new SMData_Routines();
        //eSupplierDataMain.DataLog _dataLog = new DataLog();
        eSupplierDataMain.DataSetCompressor _compressor = new DataSetCompressor();
        eSupplierDataMain.Dal.DataAccess _dataMain = new eSupplierDataMain.Dal.DataAccess();
        webServiceRoutine _webRoutine = new webServiceRoutine();

        [WebMethod]
        public byte[] GetQueryData(string cSQL)
        {
            DataSet ds = _dataMain.GetQueryDataset(cSQL);
            return _compressor.Compress(ds);
        }

        [WebMethod]
        public byte[] GetQueryDataParam(string cSQL, string[] slParams)
        {
            Dictionary<string, string> dParams = new Dictionary<string, string>();
            foreach (string cStr in slParams)
            {
                string[] slList = cStr.Split('=');
                if (slList.Length > 1)
                    dParams.Add(slList[0], slList[1]);
            }

            DataSet ds = _dataMain.GetQueryDataset(cSQL, dParams);
            return _compressor.Compress(ds);
        }

        [WebMethod]
        public byte[] GetQueryMultipleTables(string[] slSQL)
        {
            DataSet ds = _dataMain.GetQueryMultipleTables(slSQL);
            return _compressor.Compress(ds);
        }

        [WebMethod]
        public string execQueryList(List<string> slSQL)
        {
            return _dataMain.execQueryList(slSQL);
        }

        [WebMethod]
        public int execQueryCount(string cSQL)
        {
            return _dataMain.execQueryCount(cSQL);
        }

        [WebMethod]
        public void AddToDBLog(string cAudit, string cLogType, string cModule, string cFile, string cREFNo)
        {
            eSupplierDataMain.DataLog.DBAuditLog(cAudit, cLogType, cModule, cFile, cREFNo);
        }

        [WebMethod]
        public void AddToDBLog1(string cAudit, string cLogType, string cModule, string cFile, string cREFNo1,string cREFNo2, int nBuyerID, int nSupplierID)
        {
            eSupplierDataMain.DataLog.DBAuditLog(cAudit, cLogType, cModule, cFile, cREFNo1,cREFNo2, nBuyerID, nSupplierID);
        }

        [WebMethod]
        public byte[] GetQueryDataParamConn(string cSQL, string[] slParams, string cConnection)
        {
            Dictionary<string, string> dParams = new Dictionary<string, string>();
            foreach (string cStr in slParams)
            {
                string[] slList = cStr.Split('=');
                if (slList.Length > 1)
                    dParams.Add(slList[0], slList[1]);
            }

            DataSet ds = _webRoutine.GetQueryDataset(cSQL, dParams, cConnection);
            return _compressor.Compress(ds);
        }

        [WebMethod]
        public string UploadFile(string sLicense, string sFileName, byte[] bFile)
        {
            string sReturn = "";
            try
            {
                string sPath = System.Configuration.ConfigurationManager.AppSettings[sLicense];
                if (sPath.Trim() == "") { sReturn = "Upload path not found."; }
                else
                {
                    _webRoutine.SaveFile(sPath, bFile, sFileName, sLicense);
                }
            }
            catch (Exception e)
            {
                sReturn = e.GetBaseException().ToString();
            }
            return sReturn;
        }

        [WebMethod]
        public string GetBuyerSupplierInfo(int nByrSuppLinkID)
        {
            //MTML.GENERATOR.BuyerSupplierInfo _return = new MTML.GENERATOR.BuyerSupplierInfo();
            eSupplierDataMain.Bll.SmBuyerSupplierLink _return = new eSupplierDataMain.Bll.SmBuyerSupplierLink();
            try
            {
                //_return = _smRoutine.GetBuyerSupplierInfo(nByrSuppLinkID, "", null);
                _return = eSupplierDataMain.Bll.SmBuyerSupplierLink.Load(nByrSuppLinkID);
            }
            catch (Exception e)
            {
                //return _return;
            }
            string cReturn = ToXML(_return);

            return cReturn;
        }

        [WebMethod]
        public string NotifyHTMLMail(int nRecordID, string cAttachments)
        {
            string cReturn = "0";
            try
            {
                GSettings.cLogPath = HttpContext.Current.Request.PhysicalApplicationPath.ToString(); //ADDED BY SIMMY
                MTML.GENERATOR.BuyerSupplierInfo _ByrSuppInfo = new MTML.GENERATOR.BuyerSupplierInfo();
                _ByrSuppInfo = _smRoutine.GetBuyerSupplierInfo(nRecordID, 0, "", _ByrSuppInfo);
                _ByrSuppInfo.Use_HTML_File_Msg = 1;

                _smRoutine.SendMailNotify(_ByrSuppInfo, cAttachments);
            }
            catch (Exception ex)
            {
                cReturn = "Error : " + ex.Message;
            }
            return cReturn;
        }

        [WebMethod]
        public string[] ExportMTMLFile(int nRecordID)
        {
            string[] cReturn = new string[2];
            try
            {
                MTML.GENERATOR.MTMLInterchange _mtmlInterchage = new MTML.GENERATOR.MTMLInterchange();
                MTML.EXPORT.MtmlExport _mtmlExport = new MTML.EXPORT.MtmlExport();
                GSettings.cLogPath = HttpContext.Current.Request.PhysicalApplicationPath.ToString();//ADDED BY SIMMY
                _mtmlInterchage = _mtmlExport.Export(nRecordID, "");
                if (File.Exists(_mtmlInterchage.BuyerSuppInfo.FileFullName))
                {
                    StreamReader streamReader = new StreamReader(_mtmlInterchage.BuyerSuppInfo.FileFullName);
                    cReturn[0] = streamReader.ReadToEnd();
                    cReturn[1] = _mtmlInterchage.BuyerSuppInfo.FileName;                    

                    streamReader.Close();
                    streamReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                cReturn[0] = "-1";
            }
            return cReturn;
        }

        public string ToXML(eSupplierDataMain.Bll.SmBuyerSupplierLink ByrSuppInfo)
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(ByrSuppInfo.GetType());
            serializer.Serialize(stringwriter, ByrSuppInfo);
            return stringwriter.ToString();
        }
        
        
        //ADDED BY SIMMY
        [WebMethod]
        public int ExecuteScalar(string cSQL)
        {
            try
            {                
                DataSet ds = new DataSet();
                ds = _dataMain.GetQueryDataset(cSQL);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows.Count;
                }
                else return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        [WebMethod]
        public string GetFieldValue(string AField, string AKeyField, string ATable, string AKeyVal, string AFilter)
        {
            string cSQL, cResult = "";

            try
            {               

                cSQL = " SELECT " + AField + " FROM " + ATable + " WHERE " + AKeyField + "=" + AKeyVal;
                if (AFilter.Length > 0)
                {
                    cSQL = cSQL + " AND " + AFilter;
                }                
                DataSet dataSet = new DataSet();
                dataSet = _dataMain.GetQueryDataset(cSQL);                
                if (dataSet.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                    {
                        if (dataRow[0].ToString().Length > 0)
                            cResult = dataRow[0].ToString();
                    }
                }
            }
            catch (Exception)
            {
                cResult = "";
            }
            return cResult;
        }

      

    }
}
