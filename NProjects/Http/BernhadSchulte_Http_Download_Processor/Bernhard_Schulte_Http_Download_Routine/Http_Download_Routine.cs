using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeSCommon;
using System.IO;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using System.Globalization;
using System.Xml;
using MTML.GENERATOR;
using System.Threading;
using System.Configuration;
using System.IO.Compression;

namespace Bernhard_Schulte_Http_Download_Routine
{  
    public class Http_Download_Routine : LeSCommon.LeSCommon
    {
        #region Variables 
        public int iRetry = 0,IsAltItemAllowed = 0, IsPriceAveraged = 0, IsUOMChanged = 0, count = 0,cFilter=0,cMax=2;
        public string currentDate = "", ProcessorName = "", sDoneFile = "", sAuditMesage = "", buyer_Link_code = "", eSupp_SuppAddCode = "", pendingPath="", processedRFQ="",
            eSupp_SuppAddCode_Singapore="";//for rms
        public string[] BuyerNames, Buyer_Supplier_LinkID, Buyer_Supplier_LinkID_Singapore, Actions;
      
    
        List<string> xmlFiles = new List<string>();
        public MTMLInterchange _interchange { get; set; }
        public LineItemCollection _lineitem = null;
        string siteURL = "", Username = "", Password = "", LogPath = "", UnameElement = "", PswdElement = "",  AuditPath = "",BuyerTotal="",
      Quote_UploadPath = "", MTML_Quote_UploadPath = "",  ScreenShotPath = "", MailFilePath = "", Mail_Template = "", FROM_EMAIL_ID = "", MAIL_BCC = "",
      DocType = "", UCRefNo = "", AAGRefNo = "", LesRecordID = "", MessageNumber = "", LeadDays = "", Currency = "", BuyerName = "", BuyerPhone = "", BuyerEmail = "", BuyerFax = "",
      supplierName = "", supplierPhone = "", supplierEmail = "", DtDelvDate = "", dtExpDate = "", supplierFax = "", VesselName = "", PortName = "", PortCode = "", SupplierComment = "", PayTerms = "", DeliveryTerms = "", PackingCost = "", FreightCharge = "",
      GrandTotal = "", Allowance = "", TotalLineItemsAmount = "", VendorStatus = "", MsgNumber = "", MsgRefNumber = "", Attachment_Inbox = "", Rejected_msg = "", Price_Validity_Date = "", SearchTextInXLS = "";
        bool IsDecline = false, IsQuote = false,IsProcess_Seachef=false;

        #endregion

        public void LoadAppsettings()
        {
            try
            {
                iRetry = 0;
                Userid = dctAppSettings["USERNAME"];
                Password = dctAppSettings["PASSWORD"];
                LoginRetry = convert.ToInt(dctAppSettings["LOGINRETRY"]);
                SupplierCode = dctAppSettings["SUPPLIER"];
                SessionIDCookieName = "__RequestVerificationToken";
                URL = dctAppSettings["SITE_URL"];

                if (dctAppSettings["AUDITPATH"] != "") AuditPath = dctAppSettings["AUDITPATH"];
                if (dctAppSettings["RFQ_PATH"] != "") DownloadPath = dctAppSettings["RFQ_PATH"];
                if (dctAppSettings["LOGPATH"] != "") LogPath = dctAppSettings["LOGPATH"];

                currentDate = dctAppSettings["RFQRECEIVED_DATE"];
                ProcessorName = dctAppSettings["PROCESSOR_NAME"];
                BuyerNames = dctAppSettings["BUYER_NAME"].Split('|');
                Buyer_Supplier_LinkID = dctAppSettings["BUYER_SUPPLIER_LINK"].Split('|');
                Actions = dctAppSettings["ACTIONS"].Split(',');
                eSupp_SuppAddCode = dctAppSettings["SUPPLIER"];
                SupplierCode = eSupp_SuppAddCode;

                if (dctAppSettings.ContainsKey("SUPPLIER_SINGAPORE"))//17-01-2018
                {
                    eSupp_SuppAddCode_Singapore = dctAppSettings["SUPPLIER_SINGAPORE"];
                    Buyer_Supplier_LinkID_Singapore = dctAppSettings["BUYER_SUPPLIER_LINK_SINGAPORE"].Split('|');
                }
                pendingPath = dctAppSettings["PENDINGRFQFILE"];
                processedRFQ = dctAppSettings["PROCESSEDRFQFILE"];

                if (currentDate == null || currentDate == "")
                    currentDate = DateTime.Today.AddDays(-1).ToString("dd-MMM-yyyy");
                else
                    currentDate = DateTime.ParseExact(currentDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).AddDays(-1).ToString("dd-MMM-yyyy");
                if (LogPath == "") LogPath = AppDomain.CurrentDomain.BaseDirectory + "Log";
                if (AuditPath == "") AuditPath = AppDomain.CurrentDomain.BaseDirectory + "Audit";

                //  PrintScreenPath = dctAppSettings[]
                if (!Directory.Exists(PrintScreenPath)) Directory.CreateDirectory(PrintScreenPath);
                if (!Directory.Exists(DownloadPath)) Directory.CreateDirectory(DownloadPath);
                if (!Directory.Exists(AuditPath)) Directory.CreateDirectory(AuditPath);
                if (!Directory.Exists(LogPath)) Directory.CreateDirectory(LogPath);

                HiddenAttributeKey = "name";
                if (dctAppSettings.ContainsKey("QUOTE_UPLOADPATH")) Quote_UploadPath = dctAppSettings["QUOTE_UPLOADPATH"];
                if (dctAppSettings.ContainsKey("MTML_QUOTE_UPLOADPATH")) MTML_Quote_UploadPath = dctAppSettings["MTML_QUOTE_UPLOADPATH"];

                IsProcess_Seachef = (dctAppSettings.ContainsKey("PROCESS_SEACHEF")) ? Convert.ToBoolean(dctAppSettings["PROCESS_SEACHEF"]) : false;//changed by kalpita on 07/02/2020
            }
            catch (Exception e)
            {
                // sAuditMesage = "Exception in Initialise: " + e.GetBaseException().ToString();
                LogText = sAuditMesage = "Parameters not configured" + e.GetBaseException().ToString();//added by Kalpita on 18/10/2019
                CreateAuditFile("", "BernhardSchulte_HTTP_Downloader", "", "Error", "LeS-1012:" + sAuditMesage, buyer_Link_code, SupplierCode, AuditPath);
            }
        }

        public bool LogOut()
        {
            bool _result = false;
            try
            {
                URL = "https://paleconnect.bs-shipmanagement.com/Account/Logout";
                _httpWrapper._SetRequestHeaders[HttpRequestHeader.Cookie] = @"__RequestVerificationToken=" +
                  _httpWrapper._dctSetCookie["__RequestVerificationToken"] +
                  "; .ASPXAUTH=" + _httpWrapper._dctSetCookie[".ASPXAUTH"] + ";AUTH_T=" + _httpWrapper._dctSetCookie["AUTH_T"];
                _result = LoadURL("input", "name", "UserId");
                _httpWrapper._CookieContainer = null;
                _httpWrapper._CookieContainer = new CookieContainer();
                _httpWrapper.ContentType = "application/x-www-form-urlencoded";

            }
            catch (Exception e)
            {
                //
            }
            return _result;
        }

        #region RFQ

        public void ProcessRFQ()
        {
            try
            {
                if (GetFilterScreenAddHeaders("div", "id", "enquiry-grid"))
                {
                    ReadFilterData();
                }
                else
                {
                    // sAuditMesage = "Unable to load Filter screen"; 
                    LogText = sAuditMesage = "Unable to filter Details";//added by Kalpita on 18/10/2019
                    string filename = PrintScreenPath + "\\BernhardSchulte_RFQFilterFailed_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + eSupp_SuppAddCode + ".png";
                    if (!PrintScreen(filename)) filename = "";
                    CreateAuditFile(filename, "BernhardSchulte_HTTP_Downloader", "", "Error","LeS-1006:"+ sAuditMesage, buyer_Link_code, SupplierCode, AuditPath);
                }
            }
            catch (Exception e)
            {
              //  sAuditMesage = "Exception in Process RFQ : " + e.GetBaseException().ToString();
                LogText = sAuditMesage = "Unable to process file due to " + e.GetBaseException().ToString();//added by Kalpita on 18/10/2019
                string filename = PrintScreenPath + "\\BernhardSchulte_Exception_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + eSupp_SuppAddCode + ".png";
                if (!PrintScreen(filename)) filename = "";
                CreateAuditFile(filename, "BernhardSchulte_HTTP_Downloader", "", "Error", "LeS-1004:"+sAuditMesage, buyer_Link_code, SupplierCode, AuditPath);
            }
        }

        public bool DownloadFile(string rfqno,string fileName,string referenceNo)
        {
            bool _result = false;
            try
            {                
                string reqURL = "https://paleconnect.bs-shipmanagement.com/api/DocumentDownloader/DownloadRFQExcel?id=" + rfqno;
                _result = DownloadRFQ(reqURL, fileName, "");
            }
            catch (Exception e)
            {
              //  sAuditMesage = "Exception in DownloadFile : " + e.GetBaseException().ToString();
                LogText = sAuditMesage = "Unable to process file due to " + e.GetBaseException().ToString();//added by Kalpita on 18/10/2019
                string filename = PrintScreenPath + "\\BernhardSchulte_DownloadException_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + eSupp_SuppAddCode + ".png";
                if (!PrintScreen(filename)) filename = "";
                if (referenceNo == "") referenceNo = "";
                CreateAuditFile(filename, "BernhardSchulte_HTTP_Downloader", referenceNo, "Error", "LeS-1004:"+sAuditMesage, buyer_Link_code, SupplierCode, AuditPath);
            }
            return _result;
        }

        public bool DownloadFile(string rfqno, string fileName,string rfqdate,string vessel,string recdate,string Port,string refno)
        {
            string CompanyName = "", newFileName="";
            bool _result = false;
            try
            {
                string reqURL = "https://paleconnect.bs-shipmanagement.com/api/DocumentDownloader/DownloadRFQExcel?id=" + rfqno;
                _result = DownloadRFQ(reqURL, fileName, "");
                if (_result)
                {
                    CompanyName = Read_RFQXlsFile(fileName); string cEmail =  Read_XlsFile_Seachef(fileName);
                    if (CompanyName != null)
                    {
                        if (!dctAppSettings.ContainsKey("SUPPLIER_SINGAPORE"))//17-01-2017
                        {
                            if (IsProcess_Seachef && cEmail.Contains("seachef"))//added by kalpita on 07/02/2020 for fuji (seachef & BSM)
                            {                               
                                buyer_Link_code = GetBuyerLinkDetails_Seachef();
                            }
                            else
                            {
                                if (dctAppSettings.ContainsKey("SEACHEF_BUYER_NAME") && refno.ToUpper().StartsWith("EQ/SCF")) //aaded on 8-10-2018 for amos seachef requirement
                                {
                                    buyer_Link_code = getBuyerLinkCode(CompanyName, true);
                                }
                                else
                                {
                                    buyer_Link_code = getBuyerLinkCode(CompanyName);
                                }
                            }
                        }
                        else
                            buyer_Link_code = getBuyerLinkCode(CompanyName, Port);//


                        newFileName = buyer_Link_code + "_RFQ_" + refno.Replace('/', '_') + "_" + DateTime.Now.ToString("ddMMyyyyhhmmssfff") + ".xlsm";
                        if (buyer_Link_code != "")
                        {
                            DeleteRFQ_PendingRFQList(refno, CompanyName);
                            File.Move(fileName, DownloadPath + "\\" + newFileName);
                            if (File.Exists(DownloadPath + "\\" + newFileName))
                            {
                                WriteToRFQDownloadedFile(refno, rfqdate, vessel, Port, recdate, newFileName);
                                _result = true;
                            }
                        }
                        else
                        {
                            if (!Read_PendingRFQList(refno, CompanyName))//31-3-2017
                            {
                                //pending
                              //  string Audit = DateTime.Now.ToString("yy-MM-dd HH:mm") + " RFQ '" + newFileName + "' for VRNO '" + refno + "' download cancelled because '" + CompanyName + "' not found in list.";
                                string Audit ="LeS-1004.3:Unable to process file since '" + CompanyName + "' not found in list.";
                                CreateAuditFile(fileName, ProcessorName, refno, "Error", Audit, buyer_Link_code, SupplierCode, AuditPath);
                                AddToPendingRFQList(refno, CompanyName);//31-3-2017
                            }
                            LogText = "RFQ '" + newFileName + "' for VRNO '" + refno + "' downloading cancelled because '" + CompanyName + "' not found in list.";
                            File.Delete(fileName);
                        }
                    }
                }
            }
            catch (Exception e)
            {
               // sAuditMesage = "Exception in DownloadFile : " + e.GetBaseException().ToString();
                LogText = sAuditMesage = "Unable to process file due to " + e.GetBaseException().ToString();//added by Kalpita on 18/10/2019
                string filename = PrintScreenPath + "\\BernhardSchulte_DownloadException_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + eSupp_SuppAddCode + ".png";
                if (!PrintScreen(filename)) filename = "";
                CreateAuditFile(filename, "BernhardSchulte_HTTP_Downloader", "", "Error", "LeS-1004:" + sAuditMesage, buyer_Link_code, SupplierCode, AuditPath);
            }
            return _result;
        }

        #region FUJI Seachef
        public string Read_XlsFile_Seachef(string FileName)
        {
            string cEmail = "";
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Total.lic");
            Aspose.Cells.Workbook _workbook = new Aspose.Cells.Workbook(FileName);
            Aspose.Cells.Worksheet _worksheet = _workbook.Worksheets[0];
            Aspose.Cells.Cell _cell = _worksheet.Cells["C10"];
            string cEmailDetail = (string)_cell.Value;
            string[] stringSeparators = new string[] { "\r\n" };
            string[] lines = null;
            if (!string.IsNullOrEmpty(cEmailDetail)) { lines = cEmailDetail.Split(stringSeparators, StringSplitOptions.None); cEmail = lines[0]; } else { cEmail = ""; }
            return cEmail;
        }

        public string GetBuyerLinkDetails_Seachef()
        {
            string cBSLinkID = "";
            if (dctAppSettings["SEACHEF_LINKS"] != null)
            {
                //foreach (string b in dctAppSettings["SEACHEF_LINKS"].Split('|'))
                //{
                //    if (b.ToUpper().Contains(buyerName.ToUpper()))
                //    {
                //        cBSLinkID = b.Split('#')[1];
                //        break;
                //    }
                //}
                foreach (string b in dctAppSettings["SEACHEF_LINKS"].Split('|'))
                {
                    cBSLinkID = b;
                    break;
                }
            }
            return cBSLinkID;
        }

        public string Download_ReadXlsFile_Seachef(string rfqno, string fileName)
        {
            string reqURL = "https://paleconnect.bs-shipmanagement.com/api/DocumentDownloader/DownloadRFQExcel?id=" + rfqno;
           bool _result = DownloadRFQ(reqURL, fileName, "");
            if(_result)
            {
                return Read_XlsFile_Seachef(fileName);
            }
            else { return ""; }
        }

        #endregion

        public string Read_RFQXlsFile(string FileName)
        {
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Total.lic");
            Aspose.Cells.Workbook _workbook = new Aspose.Cells.Workbook(FileName);
            Aspose.Cells.Worksheet _worksheet = _workbook.Worksheets[0];
            Aspose.Cells.Cell A4 = _worksheet.Cells["A4"];
            string buyerDetail = (string)A4.Value;
            string[] stringSeparators = new string[] { "\r\n" };
            string[] lines = buyerDetail.Split(stringSeparators, StringSplitOptions.None);
            return lines[0];
        }

        public string getBuyerLinkCode(string buyerName)
        {
            int c = 0;
            string buyer_Link_code = "";
            foreach (string b in BuyerNames)
            {
                if (b.ToUpper() == buyerName.ToUpper())
                {
                    buyer_Link_code = Buyer_Supplier_LinkID.ElementAt(c);
                    break;
                }
                c++;
            }
            return buyer_Link_code;
        }

        public string getBuyerLinkCode(string buyerName, string Port)
        {
            int c = 0;
            string buyer_Link_code = "";
            if (!Port.ToUpper().Contains("SINGAPORE"))
            {
                foreach (string b in BuyerNames)
                {
                    if (b.ToUpper() == buyerName.ToUpper())
                    {
                        buyer_Link_code = Buyer_Supplier_LinkID.ElementAt(c);
                        break;
                    }
                    c++;
                }
            }
            else
            {
                foreach (string b in BuyerNames)
                {
                    if (b.ToUpper() == buyerName.ToUpper())
                    {
                        buyer_Link_code = Buyer_Supplier_LinkID_Singapore.ElementAt(c);
                        break;
                    }
                    c++;
                }
            }
            return buyer_Link_code;
        }

        public string getBuyerLinkCode(string buyerName, bool isSeachef)
        {
            int c = 0;
            string buyer_Link_code = "";
            if (isSeachef)
            {
                if (dctAppSettings["SEACHEF_BUYER_NAME"] != null)
                {
                    foreach (string b in dctAppSettings["SEACHEF_BUYER_NAME"].Split('|'))
                    {
                        if (b.ToUpper() == buyerName.ToUpper())
                        {
                            buyer_Link_code = dctAppSettings["SEACHEF_BUYER_SUPPLIER_LINK"].Split('|').ElementAt(c);
                            break;
                        }
                        c++;
                    }
                }

                if (buyer_Link_code == "")
                {
                    buyer_Link_code = getBuyerLinkCode(buyerName);
                }
            }
            return buyer_Link_code;
        }

        public void DeleteRFQ_PendingRFQList(string RFQNo, string CompanyName)
        {
            try
            {
                bool isPresent = false;
                if (pendingPath=="") pendingPath = AppDomain.CurrentDomain.BaseDirectory + "Pending_RFQ.txt";
                string strpendingText = RFQNo + "|" + CompanyName;
                if (File.Exists(pendingPath))
                {
                    string[] _text = File.ReadAllLines(pendingPath);
                    List<string> strList = _text.ToList();
                    isPresent = (strList.IndexOf(RFQNo + "|" + CompanyName) > -1);
                    if (isPresent)
                    {
                        int index = strList.IndexOf(RFQNo + "|" + CompanyName);
                        strList.RemoveAt(index);
                    }
                    File.WriteAllLines(pendingPath, strList);
                }
                else File.WriteAllText(pendingPath, "");
            }
            catch (Exception e)
            {
               // sAuditMesage = "Exception while deleting Pending RFQ List: " + e.GetBaseException().ToString();
                //LogText = sAuditMesage;//1004	Unable to process file
               // CreateAuditFile("", "BernhardSchulte_HTTP_Downloader", RFQNo, "Error", sAuditMesage, buyer_Link_code, SupplierCode, AuditPath);
                LogText = "Exception while deleting Pending RFQ List: " + e.GetBaseException().ToString();//added by Kalpita on 18/10/2019
            }
        }

        public void WriteToRFQDownloadedFile(string RFQNo, string RFQDt, string Vessel, string Port, string RFQRecDate, string FileName)
        {
            try
            {
                string strRFQDwnld = RFQNo + "|" + RFQDt + "|" + Vessel + "|" + Port + "|" + RFQRecDate;
                File.AppendAllText(processedRFQ, strRFQDwnld + Environment.NewLine);
                string Audit = DateTime.Now.ToString("yy-MM-dd HH:mm") + "RFQ '" + FileName + "' for ref '" + RFQNo + "' downloaded successfully.";
                CreateAuditFile(FileName, ProcessorName, RFQNo, "Downloaded", Audit, buyer_Link_code, SupplierCode, AuditPath);
                LogText = "RFQ '" + FileName + "' for VRNO '" + RFQNo + "' downloaded successfully.";
            }
            catch (Exception e)
            {
                //sAuditMesage = "Exception while writing RFQ Downloaded file : " + e.GetBaseException().ToString();
                //LogText = sAuditMesage;
                //CreateAuditFile("", "BernhardSchulte_HTTP_Downloader", RFQNo, "Error", sAuditMesage, buyer_Link_code, SupplierCode, AuditPath);
                LogText = "Exception while writing RFQ Downloaded file: " + e.GetBaseException().ToString();//added by Kalpita on 18/10/2019
            }
        }

        public bool Read_PendingRFQList(string RFQNo, string CompanyName)
        {
            bool isPresent = false;
            try
            {
                if (pendingPath == "") pendingPath = AppDomain.CurrentDomain.BaseDirectory + "Pending_RFQ.txt";
                string strpendingText = RFQNo + "|" + CompanyName;
                string[] _text = File.ReadAllLines(pendingPath);
                List<string> strList = _text.ToList();
                isPresent = (strList.IndexOf(RFQNo + "|" + CompanyName) > -1);
            }
            catch (Exception e)
            {
                //sAuditMesage = "Exception while reading Pending RFQ List: " + e.GetBaseException().ToString();
                //LogText = sAuditMesage;
                //CreateAuditFile("", "BernhardSchulte_HTTP_Downloader", RFQNo, "Error", sAuditMesage, buyer_Link_code, SupplierCode, AuditPath);
                LogText = "Exception while reading Pending RFQ List: " + e.GetBaseException().ToString();//added by Kalpita on 18/10/2019
            }
            return isPresent;
        }

        public void AddToPendingRFQList(string RFQNo, string CompanyName)
        {
            try
            {
                if (pendingPath == "") pendingPath = AppDomain.CurrentDomain.BaseDirectory + "Pending_RFQ.txt";
                string strpendingText = RFQNo + "|" + CompanyName;
                File.AppendAllText(pendingPath, strpendingText + Environment.NewLine);
            }
            catch (Exception e)
            {
                //sAuditMesage = "Exception while adding to Pending RFQ List: " + e.GetBaseException().ToString();
                //LogText = sAuditMesage;
                //CreateAuditFile("", "BernhardSchulte_HTTP_Downloader", "", "Error", sAuditMesage, buyer_Link_code, SupplierCode, AuditPath);
                LogText = "Exception while adding to Pending RFQ List: " + e.GetBaseException().ToString();//added by Kalpita on 18/10/2019
            }
        }

        private void SetDownloadHeaders()
        {
            _httpWrapper._AddRequestHeaders.Remove("X-Requested-With");
            _httpWrapper._AddRequestHeaders.Remove("servicepath");
            _httpWrapper.AcceptMimeType="text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
        }

        public void ReadFilterData()
        {
            string strAudit = "";
            try
            {
                var _library = (RFQDataJson)JsonConvert.DeserializeObject(_CurrentResponseString, typeof(RFQDataJson));
                SetDownloadHeaders();
                string oldFileName = "", newFileName = "";
                int iCnt = 0;
                if (_library != null)
                {
                    LogText = "RFQ data extracted successfully.";
                    List<string> slProcessedItem = GetProcessedItems(eActions.RFQ);
                    for (int i = 0; i < _library.result.Count; i++)
                    {
                        RFQData _data = _library.result[i];
                        if (dctAppSettings.ContainsKey("SUPPLIER_SINGAPORE"))//17-01-2018
                        { if (_data.port.ToUpper().Contains("SINGAPORE")) SupplierCode = eSupp_SuppAddCode_Singapore; }//

                        string _recDate = _data.receivedDate.Substring(0, 10);
                        string _rfqDate = _data.rfqdate.Substring(0, 10);
                        DateTime _dtRecDate = DateTime.ParseExact(_recDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        DateTime _dtRfqDate = DateTime.ParseExact(_rfqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        DateTime _dtDateFlag = DateTime.ParseExact(currentDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                        if (_dtRecDate >= _dtDateFlag)
                        {
                            if (_dtRecDate != DateTime.MinValue) _recDate = _dtRecDate.ToString("dd-MMM-yyyy");
                            if (_dtRfqDate != DateTime.MinValue) _rfqDate = _dtRfqDate.ToString("dd-MMM-yyyy");
                            bool isProcessed = (slProcessedItem.IndexOf(_data.referenceNo + "|" + _rfqDate + "|" + _data.vessel + "|" + _data.port + "|" + _recDate) > -1);
                            if (!isProcessed)
                            {
                                LogText = "Processing RFQ for VRNO '" + _data.referenceNo + "'.";
                                oldFileName = "RFQ_" + _data.referenceNo.Replace('/', '_') + "_" + DateTime.Now.ToString("ddMMyyyyhhmmssfff") + ".xlsm";
                                if (_data.company != "")
                                {
                                    if (!dctAppSettings.ContainsKey("SUPPLIER_SINGAPORE"))//17-01-2017
                                    {
                                        string _refNo = _data.referenceNo.Trim();
                                        bool isres = _refNo.ToUpper().StartsWith("EQ/SCF"); string _category = _data.category.ToUpper();
                                        string cEmail = Download_ReadXlsFile_Seachef(Convert.ToString(_data.rfqNo), DownloadPath + "\\" + oldFileName).ToUpper();
                                        string cSeachef_key = (_category == "SEACHEF") ? _category : cEmail;

                                       // if (IsProcess_Seachef && isres && (_category == "SEACHEF"))//added by kalpita on 07/02/2020 for fuji (seachef & BSM)
                                        if (IsProcess_Seachef && (cSeachef_key.Contains("SEACHEF")))//added by kalpita on 07/02/2020 for fuji (seachef & BSM),RMS
                                        {
                                            buyer_Link_code = GetBuyerLinkDetails_Seachef();
                                        }
                                        else
                                        {
                                            if (dctAppSettings.ContainsKey("SEACHEF_BUYER_NAME") && _refNo.ToUpper().StartsWith("EQ/SCF")) //aaded on 8-10-2018 for amos seachef requirement
                                            {
                                                buyer_Link_code = getBuyerLinkCode(_data.company, true);
                                            }
                                            else
                                            {
                                                buyer_Link_code = getBuyerLinkCode(_data.company);
                                            }
                                        }
                                    }
                                    else
                                        buyer_Link_code = getBuyerLinkCode(_data.company, _data.port);//
                                    newFileName = buyer_Link_code + "_RFQ_" + _data.referenceNo.Replace('/', '_') + "_" + DateTime.Now.ToString("ddMMyyyyhhmmssfff") + ".xlsm";
                                    if (buyer_Link_code != "")
                                    {
                                        DeleteRFQ_PendingRFQList(_data.referenceNo, _data.company);//31-3-2017
                                        try
                                        {
                                            if (DownloadFile(convert.ToString(_data.rfqNo), DownloadPath + "\\" + oldFileName,_data.referenceNo))
                                            {
                                                iCnt++;
                                                File.Move(DownloadPath + "\\" + oldFileName, DownloadPath + "\\" + newFileName);
                                                if (File.Exists(DownloadPath + "\\" + newFileName))
                                                {
                                                    WriteToRFQDownloadedFile(_data.referenceNo, _rfqDate, _data.vessel, _data.port, _recDate, newFileName);
                                                }
                                            }
                                        }
                                        catch (Exception ex)//31-7-2017
                                        {
                                            string sFile = PrintScreenPath + "\\BS_RFQ" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + eSupp_SuppAddCode + ".png";
                                            PrintScreen(sFile);
                                            //LogText = "Exception while downloading RFQ file : " + ex.GetBaseException().ToString();
                                            LogText = strAudit = "Unable to process file due to " + ex.GetBaseException().ToString();//added by Kalpita on 18/10/2019
                                            CreateAuditFile(sFile, "Bernhard_Schulte_Http_Download_Processor", _data.referenceNo, "Error","LeS-1004:" + strAudit, buyer_Link_code, SupplierCode, AuditPath);
                                        }
                                    }
                                    else
                                    {
                                        if (!Read_PendingRFQList(_data.referenceNo, _data.company))//31-3-2017
                                        {
                                            //string Audit = DateTime.Now.ToString("yy-MM-dd HH:mm") + " RFQ '" + newFileName + "' for VRNO '" + _data.referenceNo + "' download cancelled because '" + _data.company + "' not found in list.";
                                            //CreateAuditFile("", ProcessorName, _data.referenceNo, "Error", Audit, buyer_Link_code, SupplierCode, AuditPath);
                                            string Audit = "LeS-1004.3:Unable to process file since '" + _data.company + "' not found in list.";
                                            CreateAuditFile(oldFileName, ProcessorName, _data.referenceNo, "Error", Audit, buyer_Link_code, SupplierCode, AuditPath);
                                            AddToPendingRFQList(_data.referenceNo, _data.company);//31-3-2017
                                        }
                                        LogText = "RFQ '" + newFileName + "' for VRNO '" + _data.referenceNo + "' downloading cancelled because '" + _data.company + "' not found in list.";
                                    }
                                }
                                else
                                {
                                   // if (DownloadFile(convert.ToString(_data.rfqNo), DownloadPath + "\\" + oldFileName)) iCnt++;//17-01-2018
                                    if(DownloadFile(convert.ToString(_data.rfqNo), DownloadPath + "\\" + oldFileName,_rfqDate,_data.vessel,_recDate,_data.port,_data.referenceNo))iCnt++;//17-01-2018
                                }

                            }
                        }
                    }
                    if (iCnt == 0) LogText = "No new RFQ(s) downloaded.";
                    else  LogText = iCnt + " RFQ(s) downloaded.";
                }
                else
                {
                    //sAuditMesage = "RFQ data not found";
                    //LogText = sAuditMesage;
                    LogText = "RFQ data not found";//added by Kalpita on 18/10/2019
                    string filename = PrintScreenPath + "\\BernhardSchulte_NoRFQDATA_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + eSupp_SuppAddCode + ".png";
                    if (!PrintScreen(filename)) filename = "";
                }
            }
            catch (Exception e)
            {
                //sAuditMesage = "Exception while reading RFQ data "+e.GetBaseException().ToString();
                LogText = sAuditMesage = "Unable to process file due to " + e.GetBaseException().ToString();//added by Kalpita on 18/10/2019
                string filename = PrintScreenPath + "\\BernhardSchulte_Exception_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + eSupp_SuppAddCode + ".png";
                if (!PrintScreen(filename)) filename = "";
                CreateAuditFile(filename, "BernhardSchulte_HTTP_Downloader", "", "Error", "LeS-1004:" + sAuditMesage, buyer_Link_code, SupplierCode, AuditPath);
            }
        }
     
        public List<string> GetProcessedItems(eActions eAction)
        {
            List<string> slProcessedItems = new List<string>();
            switch (eAction)
            {
                case eActions.RFQ: sDoneFile =processedRFQ; break;
                default: break;
            }
            if (File.Exists(sDoneFile))
            {
                string[] _Items = File.ReadAllLines(sDoneFile);
                slProcessedItems.AddRange(_Items.ToList());
            }
            return slProcessedItems;
        }

        public override bool DoLogin(string validateNodeType, string validateAttribute, string attributeValue, bool bload = true)
        {
            bool isLoggedin = false; string strAudit = "";
            try
            {
                
                dctPostDataValues.Clear();
                dctPostDataValues.Add("__RequestVerificationToken", "");
                dctPostDataValues.Add("UserId", HttpUtility.UrlEncode(Userid));
                dctPostDataValues.Add("Password", Password);
                _httpWrapper.Referrer = "https://paleconnect.bs-shipmanagement.com/";
                if (iRetry == 0) LoadURL("input", "name", "UserId");
                URL = "https://paleconnect.bs-shipmanagement.com/Account/Login";
                isLoggedin = base.DoLogin("div", "id", "supplierName", false);                
                if (isLoggedin)
                {
                    LogText = "Logged in successfully";
                }
                if (!isLoggedin)
                {
                    if (iRetry == LoginRetry)
                    {
                        string filename = PrintScreenPath + "\\BernhardSchulte_LoginFailed_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + eSupp_SuppAddCode + ".png";
                        if (!PrintScreen(filename)) filename = "";
                        LogText = "Login failed";
                        CreateAuditFile(filename, "BernhardSchulte_HTTP_Downloader", "", "Error", "LeS-1014:Unable to login.", buyer_Link_code, SupplierCode, AuditPath);
                    }
                    else
                    {
                        iRetry++;
                        LogText = "Login retry";
                        isLoggedin = DoLogin(validateNodeType, validateAttribute, attributeValue,false);
                    }
                }

            }
            catch (Exception e)
            {
                //LogText = "Exception while login : " + e.GetBaseException().ToString();
                strAudit = "Unable to login : " + e.GetBaseException().ToString();//added by Kalpita on 18/10/2019
                if (iRetry > LoginRetry)
                {
                    LogText = "Login failed";
                    CreateAuditFile("", "BernhardSchulte_HTTP_Downloader", "", "Error", "LeS-1014:"+strAudit, BuyerCode, SupplierCode, AuditPath);
                }
                else
                {
                    iRetry++;
                    LogText = "Login retry";
                    isLoggedin = DoLogin(validateNodeType, validateAttribute, attributeValue);
                }
            }
            return isLoggedin;
        }

        public override bool GetFilterScreenAddHeaders(string validateNodeType, string validateAttribute, string attributeValue)
        {
            bool _result = false;
            try
            {
                URL = "https://paleconnect.bs-shipmanagement.com/api/ServiceRouter/GET?pData=status%3DOpen%26supplierStatus%3DPending%26advancedFilterData%3D%2522%2522";
                _httpWrapper._SetRequestHeaders[HttpRequestHeader.Cookie] = @"__RequestVerificationToken=" +
                    _httpWrapper._dctSetCookie["__RequestVerificationToken"] +
                    "; .ASPXAUTH=" + _httpWrapper._dctSetCookie[".ASPXAUTH"] + ";AUTH_T=" + _httpWrapper._dctSetCookie["AUTH_T"];
                _httpWrapper.AcceptMimeType = "application/json, text/javascript, */*; q=0.01";
                if (!_httpWrapper._AddRequestHeaders.ContainsKey("X-Requested-With")) _httpWrapper._AddRequestHeaders.Add("X-Requested-With", @"XMLHttpRequest");
                if (!_httpWrapper._AddRequestHeaders.ContainsKey("servicepath")) _httpWrapper._AddRequestHeaders.Add("servicepath", @"Enquiry/GetEnquiry/");
                _httpWrapper.ContentType = "application/json; charset=utf-8";
                _result = LoadURL("", "", "");
            }
            catch (Exception e)
            {
                // sAuditMesage = "Unable to load filter screen " + e.GetBaseException().ToString();
                LogText = sAuditMesage = "Unable to filter Details" + e.GetBaseException().ToString();//added by Kalpita on 18/10/2019
                string filename = PrintScreenPath + "\\BernhardSchulte_Exception_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + "_" + eSupp_SuppAddCode + ".png";
                if (!PrintScreen(filename)) filename = "";
                CreateAuditFile(filename, "BernhardSchulte_HTTP_Downloader", "", "Error", "LeS-1006:"+sAuditMesage, buyer_Link_code, SupplierCode, AuditPath);
            }
            return _result;
        }

        public override bool DownloadRFQ(string RequestURL, string DownloadFileName, string ContentType = "Application/pdf")
        {
            bool _result = false;
            try
            {
                URL = RequestURL;
                if (LoadURL("", "", "",false))
                {
                    byte[] b = null;
                    FileStream fileStream = File.OpenWrite(DownloadFileName);
                    byte[] buffer = new byte[1024];
                    using (Stream input = _httpWrapper._CurrentResponse.GetResponseStream())
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            byte[] buf = new byte[1024];
                            count = input.Read(buf, 0, 1024);
                            ms.Write(buf, 0, count);
                        } while (input.CanRead && count > 0);
                        b = ms.ToArray();
                    }
                    fileStream.Write(b, 0, b.Length);
                    fileStream.Flush();
                    fileStream.Close();
                }
                _result = (File.Exists(DownloadFileName));
            }
            catch (Exception e)
            {
                throw e;
            }
            return _result;
        }

        public override bool PrintScreen(string sFileName)
        {
            if (!Directory.Exists(Path.GetDirectoryName(sFileName) + "\\Temp")) Directory.CreateDirectory(Path.GetDirectoryName(sFileName) + "\\Temp");
            sFileName = Path.GetDirectoryName(sFileName) + "\\Temp\\" + Path.GetFileName(sFileName);
            if (base.PrintScreen(sFileName))
            //if (File.Exists(sFileName))
            {
                MoveFiles(sFileName, PrintScreenPath + "\\" + Path.GetFileName(sFileName));
                return (File.Exists(PrintScreenPath + "\\" + Path.GetFileName(sFileName)));               
            }
            else return false;

        }

        #endregion

        #region Quote

        public void ProcessQuote()
        {
            try
            {
                GetXmlFiles();
                if (xmlFiles.Count > 0)
                {
                    this.URL = "https://paleconnect.bs-shipmanagement.com/api/ServiceRouter/GET?pData=status%3DOpen%26supplierStatus%3DPending%26advancedFilterData%3D%2522%2522";
                    _httpWrapper._SetRequestHeaders[HttpRequestHeader.Cookie] = @"__RequestVerificationToken=" + _httpWrapper._dctSetCookie["__RequestVerificationToken"] + "; .ASPXAUTH=" + _httpWrapper._dctSetCookie[".ASPXAUTH"] + ";AUTH_T=" + _httpWrapper._dctSetCookie["AUTH_T"];
                    _httpWrapper.AcceptMimeType = "application/json, text/javascript, */*; q=0.01";
                    if (!_httpWrapper._AddRequestHeaders.ContainsKey("X-Requested-With")) _httpWrapper._AddRequestHeaders.Add("X-Requested-With", @"XMLHttpRequest");
                    if (!_httpWrapper._AddRequestHeaders.ContainsKey("servicepath")) _httpWrapper._AddRequestHeaders.Add("servicepath", @"Enquiry/GetEnquiry/");
                    _httpWrapper.ContentType = "application/json; charset=utf-8";
                    if (LoadURL("", "", ""))
                    {
                        var _library = (RFQDataJson)JsonConvert.DeserializeObject(_CurrentResponseString, typeof(RFQDataJson));
                        if (_library != null)
                        {
                            LogText = "Filter Data extracted successfully.";
                            List<string> slProcessedItem = GetProcessedItems(eActions.RFQ);
                       
                                foreach (string Qtefile in xmlFiles)
                                {
                                    MTMLClass _mtml = new MTMLClass();
                                    _interchange = _mtml.Load(Qtefile);
                                    LoadInterchangeDetails();

                                    for (int i = 0; i < _library.result.Count; i++)
                                    {
                                        RFQData _data = _library.result[i];
                                        //UCRefNo = "EQ/SCF/RFA/0049";
                                        if (_data.referenceNo == UCRefNo)
                                        {
                                            string value = Convert.ToString(_data.rfqNo);

                                            //this.URL = "https://paleconnect.bs-shipmanagement.com/api/ServiceRouter/GET?pData=formName%3D%252FEnquiry%252FIndex%252FQuotation";
                                            //_httpWrapper._SetRequestHeaders[HttpRequestHeader.Cookie] = @"__RequestVerificationToken=" + _httpWrapper._dctSetCookie["__RequestVerificationToken"] + "; .ASPXAUTH=" + _httpWrapper._dctSetCookie[".ASPXAUTH"] + ";AUTH_T=" + _httpWrapper._dctSetCookie["AUTH_T"];
                                            //_httpWrapper.AcceptMimeType = "application/json, text/javascript, */*; q=0.01";
                                            //_httpWrapper.Referrer = "https://paleconnect.bs-shipmanagement.com/Enquiry/Index";
                                            //if (!_httpWrapper._AddRequestHeaders.ContainsKey("X-Requested-With")) _httpWrapper._AddRequestHeaders.Add("X-Requested-With", @"XMLHttpRequest");
                                            //if (!_httpWrapper._AddRequestHeaders.ContainsKey("servicepath")) _httpWrapper._AddRequestHeaders.Add("servicepath", @"ResponsibilityTasks/GetPageFunctions");
                                            //_httpWrapper.ContentType = "application/json; charset=utf-8";
                                            //if (LoadURL("", "", ""))
                                            //{
                                            //    var qtelib = (RFQDataJson)JsonConvert.DeserializeObject(_CurrentResponseString, typeof(RFQDataJson));
                                            //}

                                            // HttpWebResponse response = null;
                                            string URL1 = "https://paleconnect.bs-shipmanagement.com/api/ServiceRouter/GET?pData=formName%3D%252FEnquiry%252FIndex%252FQuotation";
                                            string URL2 = "https://paleconnect.bs-shipmanagement.com/api/ServiceRouter/GET?pData=id%3D1020747691%26code%3DParty.Qualifier.BA";
                                            string result = GetRequestData(URL2);
                                        }
                                    }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private string GetRequestData(string Url)
        {
            HttpWebResponse response = null; string result = "";
        
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                request.KeepAlive = true;
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.Headers.Add("X-Requested-With", @"XMLHttpRequest");
                request.Headers.Add("servicepath", @"ResponsibilityTasks/GetPageFunctions");
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.97 Safari/537.36";
                request.ContentType = "application/json; charset=utf-8";
                request.Headers.Add("Sec-Fetch-Site", @"same-origin");
                request.Headers.Add("Sec-Fetch-Mode", @"cors");
                request.Referer = "https://paleconnect.bs-shipmanagement.com/Enquiry/Index";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");
                request.Headers.Set(HttpRequestHeader.Cookie, @"__RequestVerificationToken=yUseasPnMWKz7KbiohASSk8CUECJw4HjfjbiTskdSsfF_hBGAxV6CnFsuBOWIImLxDFLXBii6QD2Fa1gdKttBv4FMzgIO7kkMkUZukb1Xag1; .ASPXAUTH=5B9121A2A977DC8099168B3B2C2166EDFA70894DA6D25FB0E27327358CAAF149B6C6D2A41C3DA11D1394871934A0733C4CC42569E757523779DAB208E4CF952C6038CB414C478BF7A842A28441313CFD50B7120E320B23A15A71A5E864D738A5465B4FB1A8D157DA91D987192AB9F933BC269FCF86B38E2F65D6DE2FCFC73F87; AUTH_T=5OoCYLZSsG_QLgeQHt2vFQYjbimfBKq6lAAJmE3Sb8Y4w0nolDyv6tFtYcMQJQVtLvr3hv09MOy1RvOf96TY5DC9xjaBdWfQJVS7IeokfmqrUg_51VOWio_CsF9UZbHViUhkIRsb3iRoTXvz2o0aY_mR-G8wgs-EIRKIcjrRWgualRkPHBjUUODcFaEZq_WU_M6vHaPuZpEI9yo6DoOaMZHp6mki74Dfyxkkdj4wvEjw4NtSuo6EBHR-Gv_-EGhLQ0kZD1M6h-ex69niw9EuI1djjs-SlFnQgAR6IAwWBHSJ5B6FPWDLjKje-bgkd5lSmbUgfCfZVYIuT28whkxQvHDqerJmxUQ73SKJ_3E5mX1WOd9MSwsAn6Y36M47QbdK");

                response = (HttpWebResponse)request.GetResponse();

                result = JsonConvert.DeserializeObject(ReadResponse(response)).ToString();

              
                
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                result = "";
            }
            finally
            {
                if (response != null) response.Close();
            }
            return result;
        }

        private string ReadResponse(HttpWebResponse response)
        {
            string end;
            try
            {
                Stream responseStream = response.GetResponseStream();
                try
                {
                    Stream gZipStream = responseStream;
                    if (response.ContentEncoding.ToLower().Contains("gzip"))
                    {
                        gZipStream = new GZipStream(gZipStream, CompressionMode.Decompress);
                    }
                    else if (response.ContentEncoding.ToLower().Contains("deflate"))
                    {
                        gZipStream = new DeflateStream(gZipStream, CompressionMode.Decompress);
                    }
                    StreamReader streamReader = new StreamReader(gZipStream, Encoding.UTF8);
                    try
                    {
                        end = streamReader.ReadToEnd();
                    }
                    finally
                    {
                        if (streamReader != null)
                        {
                            ((IDisposable)streamReader).Dispose();
                        }
                    }
                }
                finally
                {
                    if (responseStream != null)
                    {
                        ((IDisposable)responseStream).Dispose();
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return end;
        }

        public void GetXmlFiles()
        {
            xmlFiles.Clear();
            DirectoryInfo _dir = new DirectoryInfo(MTML_Quote_UploadPath);
            FileInfo[] _Files = _dir.GetFiles();
            foreach (FileInfo _MtmlFile in _Files)
            {
                xmlFiles.Add(_MtmlFile.FullName);
            }
        }

        public void LoadInterchangeDetails()
        {
            try
            {
                LogText = "Started Loading interchange object.";
                if (_interchange != null)
                {
                    if (_interchange.Recipient != null)
                        BuyerCode = _interchange.Recipient;

                    if (_interchange.Sender != null)
                        SupplierCode = _interchange.Sender;

                    if (_interchange.DocumentHeader.DocType != null)
                        DocType = _interchange.DocumentHeader.DocType;

                    if (_interchange.DocumentHeader != null)
                    {
                        if (_interchange.DocumentHeader.IsDeclined)
                            IsDecline = _interchange.DocumentHeader.IsDeclined;

                        if (_interchange.DocumentHeader.MessageNumber != null)
                            MessageNumber = _interchange.DocumentHeader.MessageNumber;

                        if (_interchange.DocumentHeader.LeadTimeDays != null)
                            LeadDays = _interchange.DocumentHeader.LeadTimeDays;

                        Currency = _interchange.DocumentHeader.CurrencyCode;

                        MsgNumber = _interchange.DocumentHeader.MessageNumber;
                        MsgRefNumber = _interchange.DocumentHeader.MessageReferenceNumber;

                        if (_interchange.DocumentHeader.IsAltItemAllowed != null) IsAltItemAllowed = Convert.ToInt32(_interchange.DocumentHeader.IsAltItemAllowed);
                        if (_interchange.DocumentHeader.IsPriceAveraged != null) IsPriceAveraged = Convert.ToInt32(_interchange.DocumentHeader.IsPriceAveraged);
                        if (_interchange.DocumentHeader.IsUOMChanged != null) IsUOMChanged = Convert.ToInt32(_interchange.DocumentHeader.IsUOMChanged);


                        for (int i = 0; i < _interchange.DocumentHeader.References.Count; i++)
                        {
                            if (_interchange.DocumentHeader.References[i].Qualifier == ReferenceQualifier.UC)
                                UCRefNo = _interchange.DocumentHeader.References[i].ReferenceNumber.Trim();
                            else if (_interchange.DocumentHeader.References[i].Qualifier == ReferenceQualifier.AAG)
                                AAGRefNo = _interchange.DocumentHeader.References[i].ReferenceNumber.Trim();
                        }
                    }
                    if (_interchange.BuyerSuppInfo != null)
                    {
                        LesRecordID = Convert.ToString(_interchange.BuyerSuppInfo.RecordID);
                    }

                    #region read interchange party addresses

                    for (int j = 0; j < _interchange.DocumentHeader.PartyAddresses.Count; j++)
                    {
                        if (_interchange.DocumentHeader.PartyAddresses[j].Qualifier == PartyQualifier.BY)
                        {
                            BuyerName = _interchange.DocumentHeader.PartyAddresses[j].Name;
                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts != null)
                            {
                                if (_interchange.DocumentHeader.PartyAddresses[j].Contacts.Count > 0)
                                {
                                    if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count > 0)
                                    {
                                        for (int k = 0; k < _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count; k++)
                                        {
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.TE)
                                                BuyerPhone = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.EM)
                                                BuyerEmail = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.FX)
                                                BuyerFax = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                        }
                                    }
                                }
                            }
                        }

                        else if (_interchange.DocumentHeader.PartyAddresses[j].Qualifier == PartyQualifier.VN)
                        {
                            supplierName = _interchange.DocumentHeader.PartyAddresses[j].Name;
                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts != null)
                            {
                                if (_interchange.DocumentHeader.PartyAddresses[j].Contacts.Count > 0)
                                {
                                    if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count > 0)
                                    {
                                        for (int k = 0; k < _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count; k++)
                                        {
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.TE)
                                                supplierPhone = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.EM)
                                                supplierEmail = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.FX)
                                                supplierFax = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                        }
                                    }
                                }
                            }
                        }

                        else if (_interchange.DocumentHeader.PartyAddresses[j].Qualifier == PartyQualifier.UD)
                        {
                            VesselName = _interchange.DocumentHeader.PartyAddresses[j].Name;
                            if (_interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Berth != "")
                                PortName = _interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Berth;

                            if (_interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Port != null)
                                PortCode = _interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Port;
                        }
                    }

                    #endregion

                    #region read comments

                    if (_interchange.DocumentHeader.Comments != null)
                    {
                        for (int i = 0; i < _interchange.DocumentHeader.Comments.Count; i++)
                        {
                            if (_interchange.DocumentHeader.Comments[i].Qualifier == CommentTypes.SUR)
                                SupplierComment = _interchange.DocumentHeader.Comments[i].Value;
                            else if (_interchange.DocumentHeader.Comments[i].Qualifier == CommentTypes.ZTP)
                                PayTerms = _interchange.DocumentHeader.Comments[i].Value;
                            else if (_interchange.DocumentHeader.Comments[i].Qualifier == CommentTypes.ZTC)//09-02-2018//rotterdam req
                                DeliveryTerms = _interchange.DocumentHeader.Comments[i].Value;
                        }
                    }

                    #endregion

                    #region read Line Items

                    if (_interchange.DocumentHeader.LineItemCount > 0)
                    {
                        _lineitem = _interchange.DocumentHeader.LineItems;
                    }

                    #endregion

                    #region read Interchange Monetory Amount

                    if (_interchange.DocumentHeader.MonetoryAmounts != null)
                    {
                        for (int i = 0; i < _interchange.DocumentHeader.MonetoryAmounts.Count; i++)
                        {
                            if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.PackingCost_106)
                                PackingCost = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.FreightCharge_64)
                                FreightCharge = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.TotalLineItemsAmount_79)
                                TotalLineItemsAmount = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.AllowanceAmount_204)
                                Allowance = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.GrandTotal_259)
                                GrandTotal = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.BuyerItemTotal_90)//16-12-2017
                                BuyerTotal = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                        }
                    }

                    #endregion

                    #region read date time period

                    if (_interchange.DocumentHeader.DateTimePeriods != null)
                    {
                        for (int i = 0; i < _interchange.DocumentHeader.DateTimePeriods.Count; i++)
                        {
                            if (_interchange.DocumentHeader.DateTimePeriods[i].Qualifier == DateTimePeroidQualifiers.DocumentDate_137)
                            {
                                if (_interchange.DocumentHeader.DateTimePeriods[i].Value != null)
                                { DateTime dtDocDate = FormatMTMLDate(_interchange.DocumentHeader.DateTimePeriods[i].Value); }
                            }

                            else if (_interchange.DocumentHeader.DateTimePeriods[i].Qualifier == DateTimePeroidQualifiers.DeliveryDate_69)
                            {
                                if (_interchange.DocumentHeader.DateTimePeriods[i].Value != null)
                                {
                                    DateTime dtDelDate = FormatMTMLDate(_interchange.DocumentHeader.DateTimePeriods[i].Value);
                                    if (dtDelDate != DateTime.MinValue)
                                    {
                                        DtDelvDate = dtDelDate.ToString("MM/dd/yyyy");
                                    }
                                    if (dtDelDate == null)
                                    {
                                        DateTime dt = FormatMTMLDate(DateTime.Now.AddDays(Convert.ToDouble(LeadDays)).ToString());
                                        if (dt != DateTime.MinValue)
                                        {
                                            DtDelvDate = dt.ToString("MM/dd/yyyy");
                                        }
                                    }
                                }
                            }

                            if (_interchange.DocumentHeader.DateTimePeriods[i].Qualifier == DateTimePeroidQualifiers.ExpiryDate_36)
                            {
                                if (_interchange.DocumentHeader.DateTimePeriods[i].Value != null)
                                {
                                    DateTime ExpDate = FormatMTMLDate(_interchange.DocumentHeader.DateTimePeriods[i].Value);
                                    if (ExpDate != DateTime.MinValue)
                                    {
                                        dtExpDate = ExpDate.ToString("dd-MMM-yyyy");//.ToString("MM/dd/yyyy");
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    LogText = "stopped Loading interchange object.";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception on LoadInterchangeDetails : " + ex.GetBaseException().ToString());
            }
        }

        private void GetQuoteDetails(string VrNo)
        {
            
        }

        public void MoveFileToError(string MTML_QuoteFile, string message, string xls_QuoteFile)
        {
            string sFile = ScreenShotPath + "\\BS_Quote_" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
           //PrintPDF(sFile, false);
            LogText = message;

            try
            {
                if (File.Exists(MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile)))
                    File.Delete(MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
                File.Move(MTML_QuoteFile, MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
                Thread.Sleep(1000);

                if (File.Exists(Quote_UploadPath + "\\Error\\" + Path.GetFileName(xls_QuoteFile)))
                    File.Delete(Quote_UploadPath + "\\Error\\" + Path.GetFileName(xls_QuoteFile));
                File.Move(xls_QuoteFile, Quote_UploadPath + "\\Error\\" + Path.GetFileName(xls_QuoteFile));
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                LogText = "Error in MoveFileToError() - " + ex.Message;
            }      
            CreateAuditFile(sFile, "Bernhard Schulte Quote", UCRefNo, "Error", message, buyer_Link_code, SupplierCode, AuditPath);
        }

        public void MoveFileToError_WithoutScreenShot(string MTML_QuoteFile, string message, string xls_QuoteFile)
        {
            if (File.Exists(MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile)))
                File.Delete(MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
            File.Move(MTML_QuoteFile, MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
            Thread.Sleep(1000);

            if (File.Exists(Quote_UploadPath + "\\Error\\" + Path.GetFileName(xls_QuoteFile)))
                File.Delete(Quote_UploadPath + "\\Error\\" + Path.GetFileName(xls_QuoteFile));
            File.Move(xls_QuoteFile, Quote_UploadPath + "\\Error\\" + Path.GetFileName(xls_QuoteFile));
            Thread.Sleep(1000);

            if (File.Exists(MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile))) // && File.Exists(Quote_UploadPath + "\\Error\\" + Path.GetFileName(xls_QuoteFile)))
                CreateAuditFile(MTML_QuoteFile, "Bernhard Schulte Quote", UCRefNo, "Error", message, buyer_Link_code, SupplierCode, AuditPath);

            LogText = message;
        }

        public void MoveFileToBackup(string MTML_QuoteFile, string message, string xls_QuoteFile)
        {
            LogText = message;

            try
            {
                if (File.Exists(MTML_Quote_UploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile)))
                    File.Delete(MTML_Quote_UploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile));
                File.Move(MTML_QuoteFile, MTML_Quote_UploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile));
                Thread.Sleep(1000);

                if (File.Exists(Quote_UploadPath + "\\Backup\\" + Path.GetFileName(xls_QuoteFile)))
                    File.Delete(Quote_UploadPath + "\\Backup\\" + Path.GetFileName(xls_QuoteFile));
                File.Move(xls_QuoteFile, Quote_UploadPath + "\\Backup\\" + Path.GetFileName(xls_QuoteFile));
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                LogText = "Unable to move file " + ex.Message;
            }

            if (File.Exists(MTML_Quote_UploadPath + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile))) //&& File.Exists(Quote_UploadPath + "\\Backup\\" + Path.GetFileName(xls_QuoteFile)))
                CreateAuditFile(MTML_QuoteFile, "Bernhard Schulte Quote", UCRefNo, "Success", message, buyer_Link_code, SupplierCode, AuditPath);
        }

        public void MoveDeclineFileToBackup(string QuoteFile, string message, string xlsQuote_File)
        {
            LogText = message;
            try
            {

                if (File.Exists(MTML_Quote_UploadPath + "\\Backup\\" + Path.GetFileName(QuoteFile)))
                    File.Delete(MTML_Quote_UploadPath + "\\Backup\\" + Path.GetFileName(QuoteFile));
                File.Move(QuoteFile, MTML_Quote_UploadPath + "\\Backup\\" + Path.GetFileName(QuoteFile));

                if (File.Exists(Quote_UploadPath + "\\Backup\\" + Path.GetFileName(xlsQuote_File)))
                {
                    File.Delete(Quote_UploadPath + "\\Backup\\" + Path.GetFileName(xlsQuote_File));
                }
                File.Move(xlsQuote_File, Quote_UploadPath + "\\Backup\\" + Path.GetFileName(xlsQuote_File));
            }
            catch (Exception ex)
            {
                LogText = "Unable to move file -" + ex.Message;
            }

            if (File.Exists(Quote_UploadPath + "\\Backup\\" + Path.GetFileName(QuoteFile)))
                CreateAuditFile(QuoteFile, "Bernhard Schulte Quote", UCRefNo, "Declined", message, buyer_Link_code, SupplierCode, AuditPath);
        }

        public void CreateMailFile(string status)
        {
            if (status == "Submitted" || status == "Rejected")
            {
                string strMailTemp = File.ReadAllText(Mail_Template);
                if (strMailTemp.Length > 0)
                {
                    strMailTemp = strMailTemp.Replace("#BUYER_NAME#", BuyerName);
                    strMailTemp = strMailTemp.Replace("#SUPPLIER_NAME#", supplierName);
                    string msg = "QUOTE for REF : " + UCRefNo + " VESSEL : " + VesselName + status + "  From : " + supplierName + " To : " + BuyerName;
                    strMailTemp = strMailTemp.Replace("#MESSAGE#", msg);

                    string TopMsg = LesRecordID + "|" + DocType + "|" + UCRefNo + "|" + FROM_EMAIL_ID + "|" + supplierEmail + "||" + MAIL_BCC + "|" + msg;
                    string BottomMsg = "|||0||||||" + status + "|||";
                    if (FROM_EMAIL_ID.Length != 0 && supplierEmail.Length != 0)
                    {
                        string FileName = MailFilePath + "\\Mail_Txt_" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".txt";
                        if (!File.Exists(FileName))
                        {
                            try
                            {
                                File.WriteAllText(FileName, TopMsg + Environment.NewLine + strMailTemp + Environment.NewLine + Environment.NewLine + Environment.NewLine + BottomMsg);
                            }
                            catch (Exception)
                            { }
                        }
                    }
                    else
                    {
                        LogText = "Unable to send Quote mail for Buyer Ref : " + UCRefNo;
                        string sFile = "RFQ_" + UCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls";
                        CreateAuditFile(sFile, "Bernhard Schulte Quote", UCRefNo, "Error", "Unable to send Quote mail for Buyer Ref : " + UCRefNo, buyer_Link_code, SupplierCode, AuditPath);
                    }
                }
            }
        }

        public void GetScreenShots(string RFQNo, string RFQStatus, string VendorStatus, string QuoteFile, string xlsQuote_File)
        {
            if (File.Exists(MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(QuoteFile)))
                File.Delete(MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(QuoteFile));
            File.Move(QuoteFile, MTML_Quote_UploadPath + "\\Error\\" + Path.GetFileName(QuoteFile));

            if (File.Exists(Quote_UploadPath + "\\Error\\" + Path.GetFileName(xlsQuote_File)))
                File.Delete(Quote_UploadPath + "\\Error\\" + Path.GetFileName(xlsQuote_File));
            File.Move(xlsQuote_File, Quote_UploadPath + "\\Error\\" + Path.GetFileName(xlsQuote_File));


            if (VendorStatus.ToUpper().Contains("QUOTE") || VendorStatus.ToUpper().Contains("DRAFT") || VendorStatus.ToUpper().Contains("REJECT"))
            {
                LogText = "Quote file '" + Path.GetFileName(QuoteFile) + "' move to Error folder as its vendor status is " + VendorStatus + ".";
                LogText = "XLS Quote file '" + Path.GetFileName(xlsQuote_File) + "' move to Error folder as its vendor status is " + VendorStatus + ".";
            }
            else if (RFQStatus.Contains("Close"))
            {
                LogText = "Quote file '" + Path.GetFileName(QuoteFile) + "' move to Error folder as its RFQ status is Closed.";
                LogText = "XLS Quote file '" + Path.GetFileName(QuoteFile) + "' move to Error folder as its RFQ status is Closed.";
            }
            string sFile = ScreenShotPath + "\\BS_Quote_" + RFQNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".pdf";
            //PrintPDF(sFile, false);
            if (File.Exists(sFile))
            {
                if (RFQStatus.Contains("Close"))
                    CreateAuditFile(sFile, "Bernhard Schulte Quote", RFQNo, "Error", "Unable to process Quote for VRNo '" + RFQNo + "' due to RFQ Status is already " + RFQStatus, buyer_Link_code, SupplierCode, AuditPath);
                else
                    CreateAuditFile(sFile, "Bernhard Schulte Quote", RFQNo, "Error", "Unable to process Quote for VRNo '" + RFQNo + "' due to Vendor Status is already " + VendorStatus, buyer_Link_code, SupplierCode, AuditPath);
            }
            else
            {
                if (RFQStatus.Contains("Close"))
                    CreateAuditFile("", "Bernhard Schulte Quote", RFQNo, "Error", "Unable to process Quote for VRNo '" + RFQNo + "' due to RFQ Status is already " + RFQStatus, buyer_Link_code, SupplierCode, AuditPath);
                else
                    CreateAuditFile("", "Bernhard Schulte Quote", RFQNo, "Error", "Unable to process Quote for VRNo '" + RFQNo + "' due to Vendor Status is already " + VendorStatus, buyer_Link_code, SupplierCode, AuditPath);
            }
        }

        public DateTime FormatMTMLDate(string DateValue)
        {
            DateTime Dt = DateTime.MinValue;
            if (DateValue != null && DateValue != "")
            {
                if (DateValue.Length > 5)
                {
                    int year = Convert.ToInt32(DateValue.Substring(0, 4));
                    int Month = Convert.ToInt32(DateValue.Substring(4, 2));
                    int day = Convert.ToInt32(DateValue.Substring(6, 2));
                    Dt = new DateTime(year, Month, day);
                }
            }
            return Dt;
        }

        private void SendMailNotification(MTMLInterchange _interchange, string DocType, string VRNO, string ActionType, string Message)
        {
            try
            {
                string MailFromDefault = Convert.ToString(ConfigurationManager.AppSettings["FROM_EMAIL_ID"]);
                if (MailFromDefault == null) MailFromDefault = "";
                string MailBccDefault = Convert.ToString(ConfigurationManager.AppSettings["MAIL_BCC"]);
                if (MailBccDefault == null) MailBccDefault = "";
                string MailCcDefault = Convert.ToString(ConfigurationManager.AppSettings["MAIL_CC"]);
                if (MailCcDefault == null) MailCcDefault = "";

                string BuyerCode = Convert.ToString(_interchange.Recipient).Trim();
                string SuppCode = Convert.ToString(_interchange.Sender).Trim();
                string BuyerID = Convert.ToString(_interchange.BuyerSuppInfo.BuyerID).Trim();
                string SupplierID = Convert.ToString(_interchange.BuyerSuppInfo.SupplierID).Trim();

                string MailAuditPath = Convert.ToString(ConfigurationManager.AppSettings["MAIL_AUDIT_PATH"]);
                if (MailAuditPath != null)
                {
                    if (MailAuditPath.Trim() != "")
                    {
                        if (!Directory.Exists(MailAuditPath.Trim())) Directory.CreateDirectory(MailAuditPath.Trim());
                    }
                    else throw new Exception("MAIL_AUDIT_PATH value is not defined in config file.");
                }
                else throw new Exception("No mail sending required.");


                string MailSettings = Convert.ToString(ConfigurationManager.AppSettings[SuppCode + "-" + BuyerCode]);
                if (MailSettings.Trim() != "")
                {
                    string[] arr = MailSettings.Trim().Split('|');

                    string notifySupp = arr[0].Trim().ToUpper();
                    string notifyBuyer = arr[1].Trim().ToUpper();
                    string notifyCC = arr[2].Trim().ToUpper();
                    string notifyBCC = arr[3].Trim().ToUpper();
                    string sendHTML = arr[4].Trim().ToUpper();
                    string sendAttachment = arr[5].Trim().ToUpper();
                    string byrFromMailID = arr[6].Trim().ToUpper();
                    string useDefaultFromMailID = arr[7].Trim().ToUpper();
                    string supplierID = arr[8].Trim().ToUpper();
                    string byrMailTemplate = arr[9].Trim().ToUpper();

                    if (SupplierID.Trim().Length == 0) SupplierID = supplierID;

                    string MailBodyTemplate = arr[10].Trim().ToUpper();
                    string SuppLinkMailID = arr[11].Trim();

                    MailBodyTemplate = AppDomain.CurrentDomain.BaseDirectory + "\\MAIL_TEMPLATES\\" + MailBodyTemplate.Trim();
                    if (!File.Exists(MailBodyTemplate)) throw new Exception("Mail Body Template '" + Path.GetFileName(MailBodyTemplate) + "' not found under MAIL_TEMPLATES folder.");

                    string SubjectTempate = AppDomain.CurrentDomain.BaseDirectory + "\\MAIL_TEMPLATES\\MAIL_SUBJECT.txt";
                    if (!File.Exists(SubjectTempate)) throw new Exception("Subject Template 'MAIL_SUBJECT.txt' not found under MAIL_TEMPLATES folder.");

                    string attachmentFile = "";
                    if (_interchange.DocumentHeader.OriginalFile != null)
                    {
                        attachmentFile = Convert.ToString(_interchange.DocumentHeader.OriginalFile);
                        if (attachmentFile.Trim() != "" && Path.GetExtension(attachmentFile).ToUpper().Contains("XML"))
                        {
                            attachmentFile = ""; // DO NOT SEND XML FILE AS ATTACHMENT
                        }
                    }

                    string Vessel = "", SenderName = "", RecipientName = "", ByrMailID = "", SuppMailID = "";

                    #region // Get Part Address Details //
                    foreach (Party _partyObj in _interchange.DocumentHeader.PartyAddresses)
                    {
                        if (_partyObj.Qualifier == PartyQualifier.BY)
                        {
                            RecipientName = Convert.ToString(_partyObj.Name).Trim(); // Buyer Name
                            if (_partyObj.Contacts.Count > 0 && _partyObj.Contacts[0].CommunMethodList.Count > 0)
                            {
                                foreach (CommunicationMethods commMethod in _partyObj.Contacts[0].CommunMethodList)
                                {
                                    if (commMethod.Qualifier == CommunicationMethodQualifiers.EM)
                                    {
                                        ByrMailID = Convert.ToString(commMethod.Number).Trim();
                                        break;
                                    }
                                }
                            }
                        }
                        else if (_partyObj.Qualifier == PartyQualifier.VN)
                        {
                            SenderName = Convert.ToString(_partyObj.Name).Trim(); // Vendor Name
                            if (_partyObj.Contacts.Count > 0 && _partyObj.Contacts[0].CommunMethodList.Count > 0)
                            {
                                foreach (CommunicationMethods commMethod in _partyObj.Contacts[0].CommunMethodList)
                                {
                                    if (commMethod.Qualifier == CommunicationMethodQualifiers.EM)
                                    {
                                        SuppMailID = Convert.ToString(commMethod.Number).Trim();
                                        SuppMailID = "";//2-11-17
                                        break;
                                    }
                                }
                            }
                        }
                        else if (_partyObj.Qualifier == PartyQualifier.UD)
                        {
                            Vessel = Convert.ToString(_partyObj.Name).Trim();
                        }
                    }
                    #endregion

                    if (SuppMailID.Trim() == "")
                    {
                        SuppMailID = SuppLinkMailID.Trim();
                    }

                    #region // NOTIFY TO SUPPLIER //
                    if (notifySupp == "YES")
                    {
                        // Send Mail Notification for Supplier
                        string MailFrom = MailFromDefault, MailTo = SuppMailID.Trim().Replace("E-mail:", "").Trim(), mailBody = "";

                        if (MailTo.Trim() != "")
                        {
                            int QuotationID = Convert.ToInt32(_interchange.BuyerSuppInfo.RecordID);

                            #region // Set Subject //
                            string Subject = File.ReadAllText(SubjectTempate);
                            Subject = Subject.Replace("#DOC_TYPE#", DocType.Trim().ToUpper());
                            Subject = Subject.Replace("#REF_NO#", VRNO.Trim());
                            if (Vessel.Trim().Length > 0)
                            {
                                Subject = Subject.Replace("#VESSEL_NAME#", Vessel.Trim());
                            }
                            else
                            {
                                Subject = Subject.Replace("VESSEL : #VESSEL_NAME# ", "");
                            }
                            Subject = Subject.Replace("#ACTION_TYPE#", ActionType.Trim());
                            Subject = Subject.Replace("#SENDER#", SenderName.Trim());
                            Subject = Subject.Replace("#RECEIVER#", RecipientName.Trim());
                            Subject = Subject.Replace("#BUYER_CODE#", BuyerCode.Trim().ToUpper().Replace("_NEKOXLS", ""));
                            #endregion

                            #region // Set MailBody //
                            mailBody = File.ReadAllText(MailBodyTemplate);
                            mailBody = mailBody.Replace("#DOC_TYPE#", DocType.Trim().ToUpper());
                            mailBody = mailBody.Replace("#SENDER#", SenderName.Trim());
                            mailBody = mailBody.Replace("#BUYER_NAME#", RecipientName.Trim());
                            mailBody = mailBody.Replace("#BUYER_CODE#", BuyerCode.Trim().ToUpper().Replace("_NEKOXLS", ""));
                            mailBody = mailBody.Replace("#SUPPLIER_NAME#", SenderName.Trim().ToUpper());
                            mailBody = mailBody.Replace("#MESSAGE#", Message.Trim());
                            #endregion

                            string mailText = QuotationID + "|" +
                                DocType.Trim().ToUpper() + "|" +
                                VRNO.Trim() + "|" +
                                MailFrom.Trim() + "|" +
                                MailTo.Trim() + "|" +
                                (notifyCC == "YES" ? MailCcDefault : "") + "|" +
                                (notifyBCC == "YES" ? MailBccDefault : "") + "|" +
                                Subject.Trim() + "|" +
                                mailBody + "|" +
                                (sendAttachment == "YES" ? attachmentFile : "") + "|" +
                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "|" +
                                "0" + "|" + // NOT TO SEND FLAG
                                BuyerID + "|" + // BUYERID
                                SupplierID + "|" + // SUPPLIERID
                                "|" + // Reply Email ID
                                SenderName + "|" + // Supplier Name
                                RecipientName + "|" + // Buyer Name
                                ActionType.Trim().ToUpper() + "|" + // ACTION TYPE
                                "0" + "|" + // DO NOT SEND HTML
                                "1" + "|" + // Send Html Msg
                                "1"; // Use Html File Msg

                            // Write To File
                            File.WriteAllText(MailAuditPath + "\\MailNotify_" + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".txt", mailText.Trim());
                            LogText = "Mail Send to Supplier Email -" + MailTo.Trim() + ".";
                        }
                        else
                        {
                            LogText = "Unable to send mail notification to supplier; Supplier Mailid is empty.";
                        }
                    }
                    #endregion
                }
                else
                {
                    LogText = "Unable to send mail notification; No mail setting found for Supplier-Buyer (" + SuppCode + "-" + BuyerCode + ") link combination.";
                }
            }
            catch (Exception ex)
            {
                LogText = "Unable to create Mail notification template. Error : " + ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }


        #endregion

    }

    public class RFQData
    {              
        public string title { get; set; }
        public string referenceNo { get; set; }
        public string quoteby { get; set; }
        public string port { get; set; }
        public string rfqdate { get; set; }
        public string priority { get; set; }
        public string category { get; set; }
        public string categoryCode { get; set; }
        public string quotationdate { get; set; }        
        public string vessel { get; set; }
        public string remarks { get; set; }
        public string quotestatus { get; set; }
        public string etaetd { get; set; }
        public string eta { get; set; }
        public string etd { get; set; }
        public int rfqNo { get; set; }
        public int headerId { get; set; }
        public string rfqStatus { get; set; }
        public string rfqRevNo { get; set; }
        public string company { get; set; }
        public string vendor { get; set; }
        public int attachmentFileCount { get; set; }
        public string rejectReason { get; set; }
        public string receivedDate { get; set; }
        public string quoteRevision { get; set; }
        public int supplierId { get; set; }
        public int quoteRevisionNo { get; set; }
        public int orgId { get; set; }
    }

    public class RFQDataJson
    {
        public string version { get; set; }
        public int statusCode { get; set; }
        public bool isError { get; set; }
        public string requestToken { get; set; }
        public List<RFQData> result { get; set; }
    }

}

