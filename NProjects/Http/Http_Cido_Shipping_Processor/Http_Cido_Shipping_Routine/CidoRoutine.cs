﻿using HtmlAgilityPack;
using HttpRoutines;
using MTML.GENERATOR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Http_Cido_Shipping_Routine
{
    public class CidoRoutine
    {
        HTTP _httproutine = new HTTP();
        public string strAuditPath = "", strSiteURL = "", strUsername = "", strPassword = "", strProcessorName = "", strBuyer = "", strSupplier = "",
            strScreenShotPath = "", strAttachPath = "", strRFQPath = "", strCurrentDate = "", strQuotePath = "", strUCRefNo = "", strDocType = "", strMessageNumber = ""
            , strLeadDays = "", strCurrency = "", strMsgNumber = "", strMsgRefNumber = "", strAAGRefNo = "", strLesRecordID = "", BuyerName = "", strBuyerPhone = "", strBuyerEmail = "", strBuyerFax = "", strSupplierName = "", strSupplierPhone = "", strSupplierEmail = "", strSupplierFax = "",
            strPortName = "", strPortCode = "", strVesselName = "", strSupplierComment = "", strPayTerms = "", strPackingCost = "", strFreightCharge = "", strTotalLineItemsAmount = "", strGrandTotal = "",
            strAllowance = "", strDelvDate = "", strExpDate = "", xmlData = "", strBuyerTotal="";
        static string strLogPath;
        bool IsDecline = false;
        public int iRetry = 0; int iMaxRetry = 2, IsUOMChanged = 0, IsPriceAveraged = 0, IsAltItemAllowed = 0;
            double AdditionalDiscount = 0;
        public string strBuyerCode { get; set; }
        public string strSupplierCode { get; set; }
        List<string> xmlFiles = new List<string>();
        public LeSXML.LeSXML _lesXml = new LeSXML.LeSXML();
        public MTMLInterchange _interchange { get; set; }
        public LineItemCollection _lineitem = null;

        public string LogText { set { WriteLog(value); } }
        private void WriteLog(string _logText, string _logFile = "")
        {
            if (strLogPath == null || strLogPath == "") strLogPath = AppDomain.CurrentDomain.BaseDirectory + "Log";
            string _logfile = @"Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (_logFile.Length > 0) { _logfile = _logFile; }
            if (!Directory.Exists(strLogPath))
                Directory.CreateDirectory(strLogPath);

            Console.WriteLine(_logText);
            File.AppendAllText(strLogPath + @"\" + @_logfile, DateTime.Now.ToString() + " " + _logText + Environment.NewLine);
        }

        public void LoadAppSettings()
        {
            LogText = "Loading AppSettings";
            strSiteURL = ConfigurationManager.AppSettings["SITE_URL"].Trim();
            strUsername = ConfigurationManager.AppSettings["USERNAME"].Trim();
            strPassword = ConfigurationManager.AppSettings["PASSWORD"].Trim();
            strProcessorName = ConfigurationManager.AppSettings["PROCESSOR_NAME"].Trim();
            strBuyer = ConfigurationManager.AppSettings["BUYER"].Trim();
            strSupplier = ConfigurationManager.AppSettings["SUPPLIER"].Trim();
            strAuditPath = ConfigurationManager.AppSettings["AUDITPATH"].Trim();
            strRFQPath = ConfigurationManager.AppSettings["RFQPATH"].Trim();
            strCurrentDate = ConfigurationManager.AppSettings["CURRENTDATE"].Trim();
            strQuotePath = ConfigurationManager.AppSettings["MTML_QUOTE_PATH"].Trim();
            if (strAuditPath == "") strAuditPath = AppDomain.CurrentDomain.BaseDirectory + "Audit";
            strScreenShotPath = AppDomain.CurrentDomain.BaseDirectory + "ScreenShots";
            if (!Directory.Exists(strScreenShotPath)) Directory.CreateDirectory(strScreenShotPath);
            if (strAttachPath == "") strAttachPath = AppDomain.CurrentDomain.BaseDirectory + "Attachments";
            if (!Directory.Exists(strAttachPath)) Directory.CreateDirectory(strAttachPath);
            if (strRFQPath == "") strRFQPath = AppDomain.CurrentDomain.BaseDirectory + "XML";
            if (strQuotePath == "") strQuotePath = AppDomain.CurrentDomain.BaseDirectory + "MTML_Quote_Upload";
            if (!Directory.Exists(strQuotePath)) Directory.CreateDirectory(strQuotePath);
            if (!Directory.Exists(strQuotePath + "\\Backup")) Directory.CreateDirectory(strQuotePath + "\\Backup");
            if (!Directory.Exists(strQuotePath + "\\Error")) Directory.CreateDirectory(strQuotePath + "\\Error");
        }

        public bool doLogin()
        {
            bool isLoggedin = false;
            try
            {
                LogText = "Login process started";
                Dictionary<string, string> dictState = _httproutine.GetStateInfo(strSiteURL);
                if (dictState != null)
                {
                    string strPostData = @"__LASTFOCUS=&__VIEWSTATE=" + dictState["__VIEWSTATE"] + "&__VIEWSTATEGENERATOR=" + dictState["__VIEWSTATEGENERATOR"] + "&__SCROLLPOSITIONX=0&__SCROLLPOSITIONY=0&__EVENTTARGET=&__EVENTARGUMENT=&__EVENTVALIDATION=" + dictState["__EVENTVALIDATION"] + "&txtID=" + strUsername + "&txtPassword=" + strPassword + "&btnLogin=Sign+In&ttxtHiddenUsbKey%24ThinkBaseTextBox=&txtHiddenUserKeyType=30";
                    string strData = _httproutine.SendRequestFormData(strSiteURL, strPostData);
                    if (strData.Contains("id=\"toTop\"")) return true;
                    else if (strData.Contains("id=\"alertMessage\"") && strData.Contains("id=\"btnNotNow\"")) return true;
                    else
                    {
                        if (iRetry > iMaxRetry)
                        {
                            if (!strData.Contains("alert(\"Wrong password.\");"))
                            {
                                string strFilename = strScreenShotPath + "\\CidoLogin_Error_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                                _httproutine.PrintScreen(strData.Replace("<script>alert(\"ID does not exist.\");  </script>", "<h6>ID does not exist.</h6>"), strFilename);
                                LogText = "Login failed";
                                isLoggedin = false;
                                CreateAuditFile(strFilename, "Http_Cido_Shipping_proessor", "", "Error", "LeS-1014:Unable to login.");
                            }
                            else
                            {
                                string strFilename = strScreenShotPath + "\\CidoLogin_Error_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                                _httproutine.PrintScreen(strData.Replace("<script>alert(\"Wrong password.\");  </script>", "<h6>Wrong password.</h6>"), strFilename);
                                LogText = "Login failed";
                                isLoggedin = false;
                                //CreateAuditFile(strFilename, "Http_Cido_Shipping_proessor", "", "Error", "Wrong password.");
                                CreateAuditFile(strFilename, "Http_Cido_Shipping_proessor", "", "Error", "LeS-1014:Unable to login.");
                            }
                        }
                        else
                        {
                            iRetry++;
                            LogText = "Login retry";
                            isLoggedin = doLogin();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogText = "Exception while login : " + e.GetBaseException().ToString();
            }
            return isLoggedin;
        }

        public void Download_Process()
        {
            try
            {
                LoadAppSettings();

                if (doLogin())
                {
                    LogText = "Logged in successfully.";
                    if (Convert.ToString(ConfigurationManager.AppSettings["PROCESS_RFQ"]).ToUpper().Trim() == "TRUE")
                    {
                        LogText = "RFQ processing started.";
                        DownloadRFQ();
                        LogText = "RFQ processing completed.";
                        LogText = "";
                    }
                    if (Convert.ToString(ConfigurationManager.AppSettings["PROCESS_QUOTE"]).ToUpper().Trim() == "TRUE")
                    {
                        LogText = "Quote processing started.";
                        ProcessQuote();
                        LogText = "Quote processing completed.";
                    }
                }
            }
            catch (Exception ex)
            {
                //CreateAuditFile("", strProcessorName, "", "Error", "Unable to Download Data from Cido site ERROR : " + ex.Message);
                CreateAuditFile("", strProcessorName, "", "Error", "LeS-1004:Unable to process file due to " + ex.Message);
            }
        }

        #region RFQ
        public void DownloadRFQ()
        {
            string strPostData = "";
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            try
            {
                Dictionary<string, string> dictState = _httproutine.Get_StateInfo("http://client.cidoship.com/SHORE/SUP/Purchase/frmPurVenReqInqList.aspx?MENUSEQ=1009&TABID=tab2");
                if (dictState != null)
                {
                    if (strCurrentDate == "")
                        strPostData = @"radScript_TSM=%3B%3BSystem.Web.Extensions%2C+Version%3D4.0.0.0%2C+Culture%3Dneutral%2C+PublicKeyToken%3D31bf3856ad364e35%3Ako-KR%3Abcca958d-0b33-4edf-8c7b-050821c334aa%3Aea597d4b%3Ab25378d2%3BTelerik.Web.UI%2C+Version%3D2017.1.118.45%2C+Culture%3Dneutral%2C+PublicKeyToken%3D121fae78165ba3d4%3Ako-KR%3Ae5f799c1-ae8d-47dd-a4eb-e98a7cefaaeb%3A16e4e7cd%3Af7645509%3A88144a7a%3Aed16cbdc%3A33715776%3A58366029%3A24ee1bba&__EVENTTARGET=timgbtnSearch&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=" + dictState["__VIEWSTATE"] + "&__VIEWSTATEGENERATOR=" + dictState["__VIEWSTATEGENERATOR"] + "&__SCROLLPOSITIONX=0&__SCROLLPOSITIONY=0&__EVENTVALIDATION=" + dictState["__EVENTVALIDATION"] + "&selectMyAllVessel%24tlblddlVslName%24ThinkBaseDropDownList=&selectMyAllVessel%24tddlVsl%24ThinkBaseDropDownList=CIDO&selectMyAllVessel%24trdbtnlstVsl%24RadioButtonList=My&selectMyAllVessel%24ttxtVslListXml%24ThinkBaseTextBox=%26lt%3B%3Fxml+version%3D%221.0%22+encoding%3D%22euc-kr%22%3F%26gt%3B+%26lt%3BThinkData%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0085%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0091%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0098%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0101%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0157%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0158%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0093%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0281%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0161%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0163%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0147%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0156%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0299%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0295%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0027%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0012%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0080%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0103%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0159%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0097%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0026%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0077%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0028%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0102%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0160%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0013%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0014%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0079%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0081%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0015%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0105%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0016%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0017%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0065%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0018%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0019%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0099%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0100%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0301%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0082%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0162%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0155%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0088%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0094%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0133%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0049%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0132%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0276%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0294%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0273%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0153%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0154%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0057%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0251%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0258%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0005%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3B%2FThinkData%26gt%3B&selectMyAllVessel%24ttxtEnabled%24ThinkBaseTextBox=&ttxtFrom_Due_Date%24ThinkBaseTextBox=" + dictState["ttxtFrom_Due_Date%24ThinkBaseTextBox"] + "&ttxtTo_Due_Date%24ThinkBaseTextBox=" + dictState["ttxtTo_Due_Date%24ThinkBaseTextBox"] + "&trdbtnlstSearch%24RadioButtonList=1&tddlPartKind%24ThinkBaseDropDownList=000&ttxtDUE_PORT%24ThinkBaseTextBox=&ttxtPortName%24ThinkBaseTextBox=&tddlDetail%24ThinkBaseDropDownList=000&ttxtDetail%24ThinkBaseTextBox=&uwgInquiryList_ClientState=&txtHidePort=";
                    else
                        strPostData = @"radScript_TSM=%3B%3BSystem.Web.Extensions%2C+Version%3D4.0.0.0%2C+Culture%3Dneutral%2C+PublicKeyToken%3D31bf3856ad364e35%3Ako-KR%3Abcca958d-0b33-4edf-8c7b-050821c334aa%3Aea597d4b%3Ab25378d2%3BTelerik.Web.UI%2C+Version%3D2017.1.118.45%2C+Culture%3Dneutral%2C+PublicKeyToken%3D121fae78165ba3d4%3Ako-KR%3Ae5f799c1-ae8d-47dd-a4eb-e98a7cefaaeb%3A16e4e7cd%3Af7645509%3A88144a7a%3Aed16cbdc%3A33715776%3A58366029%3A24ee1bba&__EVENTTARGET=timgbtnSearch&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=" + dictState["__VIEWSTATE"] + "&__VIEWSTATEGENERATOR=" + dictState["__VIEWSTATEGENERATOR"] + "&__SCROLLPOSITIONX=0&__SCROLLPOSITIONY=0&__EVENTVALIDATION=" + dictState["__EVENTVALIDATION"] + "&selectMyAllVessel%24tlblddlVslName%24ThinkBaseDropDownList=&selectMyAllVessel%24tddlVsl%24ThinkBaseDropDownList=CIDO&selectMyAllVessel%24trdbtnlstVsl%24RadioButtonList=My&selectMyAllVessel%24ttxtVslListXml%24ThinkBaseTextBox=%26lt%3B%3Fxml+version%3D%221.0%22+encoding%3D%22euc-kr%22%3F%26gt%3B+%26lt%3BThinkData%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0085%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0091%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0098%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0101%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0157%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0158%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0093%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0281%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0161%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0163%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0147%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0156%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0299%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0295%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0027%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0012%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0080%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0103%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0159%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0097%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0026%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0077%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0028%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0102%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0160%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0013%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0014%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0079%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0081%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0015%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0105%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0016%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0017%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0065%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0018%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0019%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0099%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0100%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0301%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0082%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0162%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0155%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0088%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0094%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0133%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0049%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0132%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0276%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0294%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0273%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0153%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0154%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0057%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0251%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0258%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3BP_DATA%26gt%3B%26lt%3BV_CODE%26gt%3B0005%26lt%3B%2FV_CODE%26gt%3B%26lt%3B%2FP_DATA%26gt%3B%26lt%3B%2FThinkData%26gt%3B&selectMyAllVessel%24ttxtEnabled%24ThinkBaseTextBox=&ttxtFrom_Due_Date%24ThinkBaseTextBox=" + strCurrentDate + "&ttxtTo_Due_Date%24ThinkBaseTextBox=" + strCurrentDate + "&trdbtnlstSearch%24RadioButtonList=1&tddlPartKind%24ThinkBaseDropDownList=000&ttxtDUE_PORT%24ThinkBaseTextBox=&ttxtPortName%24ThinkBaseTextBox=&tddlDetail%24ThinkBaseDropDownList=000&ttxtDetail%24ThinkBaseTextBox=&uwgInquiryList_ClientState=&txtHidePort=";
                    string strData = _httproutine.SendRequestFormData("http://client.cidoship.com/SHORE/SUP/Purchase/frmPurVenReqInqList.aspx?MENUSEQ=1009&TABID=tab2", strPostData);
                    List<string> lstProcessedItem = GetProcessedItems(eActions.RFQ);
                    HtmlAgilityPack.HtmlDocument _doc = new HtmlAgilityPack.HtmlDocument();
                    _doc.LoadHtml(strData);
                    HtmlNodeCollection _nodes = _doc.DocumentNode.SelectNodes("//table[@id='uwgInquiryList_ctl00']//tr[@class='rgRow' or @class='rgAltRow']");
                    if (_nodes.Count > 0)
                    {
                        foreach (HtmlNode _row in _nodes)
                        {
                            HtmlNodeCollection _rowData = _row.ChildNodes;
                            string strState = (_rowData[2]).InnerText.Trim();
                            string strVessel = (_rowData[3]).InnerText.Trim();
                            string strInquiryNo = (_rowData[5]).InnerText.Trim();
                            string strPort = (_rowData[7]).InnerText.Trim();
                            string strInqDate = (_rowData[8]).InnerText.Trim();
                           if (!lstProcessedItem.Contains(strState + "|" + strVessel + "|" + strInquiryNo + "|" + strPort + "|" + strInqDate))
                            {
                                if (Convert.ToDateTime(strInqDate).ToString("yyyy-MM-dd") == Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd")).ToString("yyyy-MM-dd") || strCurrentDate != "")
                                {
                                    string strNavURL = @"http://client.cidoship.com/SHORE/SUP/Purchase/frmPurVenQutMgt.aspx?SysReqNo=" + (_rowData[13]).InnerText.Trim() + "&SysInqNo=" + (_rowData[15]).InnerText.Trim() + "&VdrCode=" + (_rowData[16]).InnerText.Trim() + "&Vcode=" + (_rowData[14]).InnerText.Trim() + "&POPUP=Y&MENUSEQ=1009";
                                    string strRFQData = _httproutine.SendRequest(strNavURL);
                                    htmlDoc.LoadHtml(strRFQData);

                                    string strFile = strAttachPath + "\\" + strInquiryNo + "_" + strVessel + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".png";
                                    _httproutine.PrintScreen(htmlDoc.DocumentNode.OuterHtml.Replace("SHORE/WebResource.axd?d=", "SHORE/WebResource.axdd=").Replace("<div id=\"pnlInfo\" style=\"margin-top: 6px; display: none;\">", "<div id=\"pnlInfo\" style=\"margin-top: 6px; display: block;\">"), strFile);

                                    if (GetRFQHeader(htmlDoc, strNavURL, strFile))
                                    {
                                        if (GetRFQItems(htmlDoc))
                                        {
                                            if (GetAddress(htmlDoc))
                                            {
                                                string strLesXMLFile = "RFQ_" + strInquiryNo + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xml";
                                                if (GenerateXMLFile(strInquiryNo, strLesXMLFile))
                                                {
                                                    LogText = "RFQ " + strInquiryNo + " downloaded successfully.";
                                                    string Audit = "RFQ '" + strLesXMLFile + "' for ref '" + strInquiryNo + "' downloaded successfully.";
                                                    CreateAuditFile(strLesXMLFile, strProcessorName, strInquiryNo, "Downloaded", Audit);
                                                    string strRFQDwnld = strState + "|" + strVessel + "|" + strInquiryNo + "|" + strPort + "|" + strInqDate;
                                                    File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\RFQ_Downloaded.txt", strRFQDwnld + Environment.NewLine);
                                                }
                                                else
                                                {
                                                    //string Audit = "Unable to download RFQ '" + strLesXMLFile + "' for ref '" + strInquiryNo + "'.";
                                                    string Audit = "LeS-1004:Unable to process file '" + strLesXMLFile + "' for ref '" + strInquiryNo + "'.";
                                                    CreateAuditFile(strLesXMLFile, strProcessorName, strInquiryNo, "Downloaded", Audit);
                                                    string eFile = strScreenShotPath + "\\Cido_RFQ" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".png";
                                                    _httproutine.PrintScreen(htmlDoc.DocumentNode.OuterHtml.Replace("SHORE/WebResource.axd?d=", "SHORE/WebResource.axdd=").Replace("<div id=\"pnlInfo\" style=\"margin-top: 6px; display: none;\">", "<div id=\"pnlInfo\" style=\"margin-top: 6px; display: block;\">"), eFile);
                                                }
                                            }
                                            else
                                            {
                                                //MoveRFQToError("Unable to get address details", strInquiryNo, htmlDoc);
                                                MoveRFQToError("Unable to get detail - address Field(s) not present", strInquiryNo, htmlDoc, "LeS-1040:");
                                            }
                                        }
                                        else
                                        {
                                            //MoveRFQToError("Unable to get RFQ item details", strInquiryNo, htmlDoc);
                                            MoveRFQToError("Unable to get detail - RFQ item Field(s) not present", strInquiryNo, htmlDoc, "LeS-1040:");
                                        }
                                    }
                                    else
                                    {
                                        //MoveRFQToError("Unable to get RFQ header details", strInquiryNo, htmlDoc);
                                        MoveRFQToError("Unable to get detail - RFQ header Field(s) not present", strInquiryNo, htmlDoc, "LeS-1040:");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool GetRFQHeader(HtmlAgilityPack.HtmlDocument RFQData, string strNavURL, string strFile)
        {
            bool isResult = false;
            LogText = "Start Getting Header details";
            try
            {
                _lesXml.DocID = DateTime.Now.ToString("yyyyMMddhhmmss");
                _lesXml.Created_Date = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Doc_Type = "RFQ";
                _lesXml.Dialect = "Cido Shipping";
                _lesXml.Version = "1";
                _lesXml.Date_Document = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Date_Preparation = DateTime.Now.ToString("yyyyMMdd");
                _lesXml.Sender_Code = strBuyer;
                _lesXml.Recipient_Code = strSupplier;

                string strInqNo = RFQData.GetElementbyId("ttxtInq_NO_ThinkBaseTextBox").GetAttributeValue("Value", "");
                if (strInqNo != "")
                    _lesXml.DocReferenceID = strInqNo.Trim();

                _lesXml.DocLinkID = strNavURL;

                if (File.Exists(strFile))
                    _lesXml.OrigDocFile = Path.GetFileName(strFile);

                _lesXml.Active = "1";

                string strVessel = RFQData.GetElementbyId("ttxtVesselInq_ThinkBaseTextBox").GetAttributeValue("Value", "");
                if (strVessel != "")
                    _lesXml.Vessel = strVessel.Trim();
                _lesXml.BuyerRef = strInqNo.Trim();
                string strPortCode = RFQData.GetElementbyId("ttxtDUE_PORTInq_ThinkBaseTextBox").GetAttributeValue("Value", "");
                if (strPortCode != "")
                    _lesXml.PortCode = strPortCode.Trim();
                string strPortName = RFQData.GetElementbyId("ttxtPortNameInq_ThinkBaseTextBox").GetAttributeValue("Value", "");
                if (strPortName != "")
                    _lesXml.PortName = strPortName.Trim();
                var options = RFQData.DocumentNode.SelectNodes("//select[@id='tddlQUT_CURR_CODE_ThinkBaseDropDownList']/option");
                if (options.Count > 0)
                {
                    string strCurrency = "";
                    for (int i = 0; i < options.Count; i++)
                    {
                        var val = options[i].OuterHtml;
                        if (val.Contains("selected"))
                        {
                            strCurrency = val.Split(new string[] { "selected=" }, StringSplitOptions.None)[1].Trim('>');
                            break;
                        }
                    }
                    if (strCurrency == "\"\"") strCurrency = "";
                    _lesXml.Currency = strCurrency;
                }
                string strRemarks = RFQData.GetElementbyId("ttxtRemarkInq_ThinkBaseTextBox").GetAttributeValue("Value", "");
                _lesXml.Remark_Header = strRemarks.Trim();
                string strSubject = RFQData.GetElementbyId("ttxtSubjectInq_ThinkBaseTextBox").GetAttributeValue("Value", "");
                _lesXml.Remark_Title = strSubject.Trim();
                _lesXml.Reference_Document = "";
                HtmlNodeCollection _node = RFQData.DocumentNode.SelectNodes("//table[@id='tlbCount']//td");
                if (_node.Count > 0)
                {
                    if (_node[1].InnerText.Contains(':'))
                        _lesXml.Total_LineItems = _node[1].InnerText.Split(':')[1].Trim();
                }
                string strDelDate = RFQData.GetElementbyId("ttxtToTO_DUE_DATEInq_ThinkBaseTextBox").GetAttributeValue("Value", "");
                if (strDelDate != "")
                    _lesXml.Date_Delivery = Convert.ToDateTime(strDelDate.Trim()).ToString("yyyyMMdd");
                _lesXml.Total_LineItems_Discount = "0";
                _lesXml.Total_LineItems_Net = "0";
                _lesXml.Total_Additional_Discount = "0";
                _lesXml.Total_Freight = "0";
                _lesXml.Total_Other = "0";
                _lesXml.Total_Net_Final = "0";
                LogText = "Getting Header details completed successfully.";
                isResult = true;
                return isResult;
            }
            catch (Exception ex)
            {
                LogText = "Unable to get header details." + ex.GetBaseException().ToString(); isResult = false;
                return isResult;
            }
        }

        public bool GetRFQItems(HtmlAgilityPack.HtmlDocument RFQData)
        {
            bool isResult = false;
            try
            {
                _lesXml.LineItems.Clear();
                LogText = "Start Getting LineItem details";
                HtmlNodeCollection _nodes = RFQData.DocumentNode.SelectNodes("//table[@id='tmgridInqList']//tr[@part_key]");
                if (_nodes.Count > 0)
                {
                    foreach (HtmlNode _row in _nodes)
                    {
                        LeSXML.LineItem _item = new LeSXML.LineItem();
                        try
                        {
                            HtmlNodeCollection _rowData = _row.ChildNodes;
                            _item.Number = (_rowData[1]).InnerText.Trim();
                            _item.OrigItemNumber = (_rowData[1]).InnerText.Trim();
                            if (_row.OuterHtml.Contains("part_key"))
                            {
                                string strPartKey = _row.GetAttributeValue("part_key", "");
                                _item.OriginatingSystemRef = strPartKey;
                            }
                            _item.Name = (_rowData[3]).InnerText.Trim().Replace("&nbsp;", "");
                            _item.ItemRef = (_rowData[2]).InnerText.Trim().Replace("&nbsp;", "");
                            _item.Unit = (_rowData[4]).InnerText.Trim();
                            _item.Quantity = (_rowData[5]).InnerText.Trim().Replace("&nbsp;", "");
                            _item.Discount = "0";
                            _item.ListPrice = "0";
                            _item.LeadDays = "0";
                            _item.SystemRef = (_rowData[1]).InnerText.Trim();
                            HtmlNode hRemarks = (_rowData[14]).SelectSingleNode(".//input");
                            string strRemarks = hRemarks.GetAttributeValue("Value", "");
                            _item.Remark = strRemarks.Trim();
                            if (_row.OuterHtml.Contains("s_sort"))
                            {
                                string strsSort = _row.GetAttributeValue("s_sort", "");
                                _item.EquipRemarks = strsSort;
                            }
                            _lesXml.LineItems.Add(_item);
                        }
                        catch (Exception ex)
                        { LogText = ex.GetBaseException().ToString(); }
                    }
                }
                LogText = "Getting LineItem details successfully";
                isResult = true; return isResult;
            }
            catch (Exception ex)
            {
                LogText = "Exception while getting RFQ Items: " + ex.GetBaseException().ToString(); isResult = false; return isResult;
            }
        }

        public bool GetAddress(HtmlAgilityPack.HtmlDocument RFQData)
        {
            bool isResult = false;
            try
            {
                _lesXml.Addresses.Clear();
                LogText = "Start Getting address details";
                LeSXML.Address _xmlAdd = new LeSXML.Address();

                _xmlAdd.Qualifier = "BY";
                _xmlAdd.AddressName = ConfigurationManager.AppSettings["BuyerName"].Trim();
                string strContact = RFQData.GetElementbyId("ttxtIssued_NameInq_ThinkBaseTextBox").GetAttributeValue("Value", "");
                _xmlAdd.ContactPerson = strContact;
                _lesXml.Addresses.Add(_xmlAdd);

                _xmlAdd = new LeSXML.Address();
                _xmlAdd.Qualifier = "VN";
                _xmlAdd.AddressName = ConfigurationManager.AppSettings["SupplierName"].Trim();
                _lesXml.Addresses.Add(_xmlAdd);
                LogText = "Getting address details successfully";
                isResult = true;
                return isResult;

            }
            catch (Exception ex)
            {
                LogText = "Exception while getting address details: " + ex.GetBaseException().ToString(); isResult = false;
                return isResult;
            }
        }

        public bool GenerateXMLFile(string strRFQNo, string strLesXMLFile)
        {
            bool isResult = false;
            try
            {
                if (Convert.ToInt32(_lesXml.Total_LineItems) > 0)
                {
                    _lesXml.FileName = strLesXMLFile;
                    _lesXml.WriteXML();
                    if (File.Exists(strRFQPath + "\\" + strLesXMLFile)) isResult = true;
                    else isResult = false;
                }
            }
            catch (Exception ex) { LogText = "Exception while generating XML file for VRNO: " + strRFQNo + ex.GetBaseException().ToString(); }
            return isResult;
        }

        public List<string> GetProcessedItems(eActions eAction)
        {
            string strDoneFile = "";
            List<string> lstProcessedItems = new List<string>();
            switch (eAction)
            {
                case eActions.RFQ: strDoneFile = AppDomain.CurrentDomain.BaseDirectory + "RFQ_Downloaded.txt"; break;
                default: break;
            }
            if (File.Exists(strDoneFile))
            {
                string[] _Items = File.ReadAllLines(strDoneFile);
                lstProcessedItems.AddRange(_Items.ToList());
            }
            return lstProcessedItems;
        }
        #endregion

        #region Quote
        public void ProcessQuote()
        {
            try
            {
                /*get xml files from quote upload path*/
                GetXmlFiles();
                if (xmlFiles.Count > 0)
                {
                    LogText = xmlFiles.Count + " Quote files found to process.";
                    for (int j = 0; j < xmlFiles.Count; j++)
                    {
                        ProcessQuoteMTML(xmlFiles[j]);
                    }
                }
            }
            catch (Exception ex)
            {
                LogText = "Exception while processing Quote : " + ex.GetBaseException().ToString();
            }
        }

        public void ProcessQuoteMTML(string MTML_QuoteFile)
        {
            int IsNotZeroUnitPrice = 0; string strFile = "";
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            try
            {
                LogText = "'" + Path.GetFileName(MTML_QuoteFile) + "' Quote file processing started.";
                MTMLClass _mtml = new MTMLClass();
                _interchange = _mtml.Load(MTML_QuoteFile);
                LoadInterchangeDetails();
                if (strUCRefNo != "")
                {
                    string strNavURL = strMsgNumber;
                    string strRFQData = _httproutine.SendRequest(strNavURL);
                    htmlDoc.LoadHtml(strRFQData);
                    HtmlAgilityPack.HtmlNode _savebtn = htmlDoc.GetElementbyId("timgbtnSave");
                    if (_savebtn != null)
                    {
                        foreach (LineItem item in _lineitem)
                        {
                            string _price = "", _discount = "";
                            foreach (PriceDetails _priceDetails in item.PriceList)
                            {
                                if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.GRP) _price = _priceDetails.Value.ToString("0.00");
                                else if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.DPR) _discount = _priceDetails.Value.ToString("0.00");
                            }
                            if (Convert.ToDouble(_price) > 0.00) IsNotZeroUnitPrice++;

                        }
                        if (IsNotZeroUnitPrice > 0)
                        {
                            if (SaveQuote(htmlDoc, strNavURL, MTML_QuoteFile))
                            {
                                strFile = strScreenShotPath + "\\Cido_Quote_before" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".png";
                                _httproutine.PrintScreen(htmlDoc.DocumentNode.OuterHtml, strFile);
                                LogText = "Quote saved.";
                                if (!IsDecline && Convert.ToInt32(strGrandTotal) != 0)
                                {
                                    if (Process_Offer(htmlDoc, strNavURL, MTML_QuoteFile))
                                    {
                                        LogText = "Processed offer.";
                                        HtmlNode _trState = htmlDoc.DocumentNode.SelectSingleNode("//table[@id='tlblSTATE']//td[@align='left']");
                                        if (_trState != null)
                                        {

                                            if (_trState.InnerText == "[ Offer ]")
                                            {
                                                strFile = strScreenShotPath + "\\Cido_Quote_Success" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".png";
                                                _httproutine.PrintScreen(htmlDoc.DocumentNode.OuterHtml, strFile);
                                                MoveToBackup(MTML_QuoteFile, strUCRefNo + " Quote submitted successfully");
                                            }
                                            else
                                            {
                                                //MoveToError("Unable to process offer for Ref : '" + strUCRefNo + "' as status is " + _trState.InnerText + ".", strUCRefNo, htmlDoc, MTML_QuoteFile);
                                                MoveToError("Unable to process file for '" + strUCRefNo + "' as it is in " + _trState.InnerText + "status.", strUCRefNo, htmlDoc, MTML_QuoteFile, "LeS-1004.9:");
                                            }
                                        }
                                        else
                                            MoveToError("Unable to process file for '" + strUCRefNo + "'", strUCRefNo, htmlDoc, MTML_QuoteFile, "LeS-1004:");
                                    }
                                    else
                                    {
                                        //MoveToError("Unable to process offer for Ref : '" + strUCRefNo + "'", strUCRefNo, htmlDoc, MTML_QuoteFile);
                                        MoveToError("Unable to process file for '" + strUCRefNo + "'", strUCRefNo, htmlDoc, MTML_QuoteFile, "LeS-1004:");
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(strGrandTotal) == 0)
                                        MoveToError("Unable to Save Quote for '" + strUCRefNo + "' as quotation  total is zero.", strUCRefNo, htmlDoc, MTML_QuoteFile, "LeS-1008.5:");
                                    else
                                        MoveToError("Unable to process file for Ref : '" + strUCRefNo + "' as it is in decline state.", strUCRefNo, htmlDoc, MTML_QuoteFile, "LeS-1004.7:");
                                }
                            }
                            else
                            {
                                MoveToError("Unable to save Quote.", strUCRefNo, htmlDoc, MTML_QuoteFile, "LeS-1008:");
                            }
                        }
                        else
                        {
                            //MoveToError("Unable to upload Quote for Ref : '" + strUCRefNo + "' as unit price of all items are zero.", strUCRefNo, null, MTML_QuoteFile);
                            MoveToError("Unable to Save Quote for Ref : '" + strUCRefNo + "' since unit price of all items are zero.", strUCRefNo, null, MTML_QuoteFile, "LeS-1008.6:");
                        }
                        //  }
                    }
                    else MoveToError("Unable to Save Quote as save button is disabled.", strUCRefNo, htmlDoc, MTML_QuoteFile, "LeS-1008.4:"); //1008.4	Unable to save quote, Save button is disabled

                }
            }
            catch (Exception ex)
            {
                //MoveToError("Exception while processing Quote MTML : " + ex.GetBaseException().ToString(), strUCRefNo, null, MTML_QuoteFile);
                MoveToError("Unable to process file due to " + ex.GetBaseException().ToString(), strUCRefNo, null, MTML_QuoteFile, "LeS-1004:");
            }
        }

        //public bool SaveQuote(HtmlAgilityPack.HtmlDocument htmlDoc,string strNavURL,string MTML_QuoteFile)
        //{
        //    bool result = false;
        //    try {
        //        string strCharges = Convert.ToString(Convert.ToDouble(strFreightCharge) + Convert.ToDouble(strPackingCost));
        //        string grandTotal = Convert.ToString(Convert.ToDouble(strTotalLineItemsAmount) + Convert.ToDouble(strCharges));
        //        if (Convert.ToInt32(grandTotal) == Convert.ToInt32(strGrandTotal))
        //        {
        //            var boundary = "WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x", System.Globalization.NumberFormatInfo.InvariantInfo);// "WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x");//
        //            Dictionary<string, string> dictState = _httproutine.GetStateInfo(htmlDoc);
        //            if (dictState != null)
        //            {
        //                string _inqVessel = "", _inqIssueName = "", _frmDue = "", _toDue = "", _issueDate = "", _inqPortCode = "", _portName = "", _inqSub = "", _inqRemark = "", _issueName = "", _frmDueQuote = "", _toDueQuote = "", _issueDateQuote = "", _portCodeQuote = "", _portNameQuote = "", _attchQuoteFile = "", _value = "";

        //                HtmlNode _hinqVessel = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtVesselInq_ThinkBaseTextBox']");
        //                if (_hinqVessel != null) _inqVessel = _hinqVessel.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hinqIssueName = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssued_NameInq_ThinkBaseTextBox']");
        //                if (_hinqIssueName != null) _inqIssueName = _hinqIssueName.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hfrmDue = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtFrTO_DUE_DATEInq_ThinkBaseTextBox']");
        //                if (_hfrmDue != null) _frmDue = _hfrmDue.GetAttributeValue("value", "").Trim();

        //                HtmlNode _htoDue = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtToTO_DUE_DATEInq_ThinkBaseTextBox']");
        //                if (_htoDue != null) _toDue = _htoDue.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hissueDate = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssued_DateInq_ThinkBaseTextBox']");
        //                if (_hissueDate != null) _issueDate = _hissueDate.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hinqPortCode = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtDUE_PORTInq_ThinkBaseTextBox']");
        //                if (_hinqPortCode != null) _inqPortCode = _hinqPortCode.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hportName = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtPortNameInq_ThinkBaseTextBox']");
        //                if (_hportName != null) _portName = _hportName.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hinqSub = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtSubjectInq_ThinkBaseTextBox']");
        //                if (_hinqSub != null) _inqSub = _hinqSub.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hinqRemark = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtRemarkInq_ThinkBaseTextBox']");
        //                if (_hinqRemark != null) _inqRemark = _hinqRemark.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hissueName = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssued_NameQut_ThinkBaseTextBox']");
        //                if (_hissueName != null) _issueName = _hissueName.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hfrmDueQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtFrTO_DUE_DATEQut_ThinkBaseTextBox']");
        //                if (_hfrmDueQuote != null) _frmDueQuote = _hfrmDueQuote.GetAttributeValue("value", "").Trim();

        //                HtmlNode _htoDueQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtToTO_DUE_DATEQut_ThinkBaseTextBox']");
        //                if (_htoDueQuote != null) _toDueQuote = _htoDueQuote.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hissueDateQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssued_DateQut_ThinkBaseTextBox']");
        //                if (_hissueDateQuote != null) _issueDateQuote = _hissueDateQuote.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hportCodeQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtDUE_PORTQut_ThinkBaseTextBox']");
        //                if (_hportCodeQuote != null) _portCodeQuote = _hportCodeQuote.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hportNameQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtPortNameQut_ThinkBaseTextBox']");
        //                if (_hportNameQuote != null) _portNameQuote = _hportNameQuote.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hattachQuoteFile = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='tftlstbxReportFile_SelectTextBox']");
        //                if (_hattachQuoteFile != null) _attchQuoteFile = _hattachQuoteFile.GetAttributeValue("value", "").Trim();


        //                HtmlNodeCollection _selectGeim = htmlDoc.DocumentNode.SelectNodes("//select[@id='tddlGeIm_ThinkBaseDropDownList']/option[@selected]");
        //                if (_selectGeim != null)
        //                {
        //                    _value = _selectGeim[0].GetAttributeValue("value", "").Trim();
        //                }

        //                string strPostData = @"------" + boundary +Environment.NewLine+ "Content-Disposition: form-data; name=\"radscript_tsm\"" + Environment.NewLine + Environment.NewLine + ";;System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35:ko-KR:c97801cf-c4e9-421a-bd07-262d424faf76:ea597d4b:b25378d2" + Environment.NewLine + "------" +
        //                    boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__eventtarget\"" + Environment.NewLine + Environment.NewLine + "timgbtnSave" + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__eventargument\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine +
        //                    "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__viewstate\"" + Environment.NewLine + Environment.NewLine + Uri.UnescapeDataString(dictState["__VIEWSTATE"]) + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__viewstategenerator\"" + Environment.NewLine + Environment.NewLine + dictState["__VIEWSTATEGENERATOR"] +
        //                    Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__scrollpositionx\"" + Environment.NewLine + Environment.NewLine + "0" + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__scrollpositiony\"" + Environment.NewLine + Environment.NewLine + "0" + Environment.NewLine + "------" + boundary +
        //                    Environment.NewLine + "Content-Disposition: form-data; name=\"__eventvalidation\"" + Environment.NewLine + Environment.NewLine +  Uri.UnescapeDataString(dictState["__EVENTVALIDATION"]) + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtinq_no$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + strUCRefNo + Environment.NewLine + "------" + boundary+Environment.NewLine +
        //                    "Content-Disposition: form-data; name=\"ttxtvesselinq$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + _inqVessel + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtissued_nameinq$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + _inqIssueName + Environment.NewLine + "------" + boundary + Environment.NewLine +
        //                "Content-Disposition: form-data; name=\"ttxtfrto_due_dateinq$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + _frmDue + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxttoto_due_dateinq$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + _toDue + Environment.NewLine + "------" + boundary + Environment.NewLine +
        //                "Content-Disposition: form-data; name=\"ttxtissued_dateinq$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + _issueDate + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtdue_portinq$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + _inqPortCode + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtportnameinq$thinkbasetextbox\"" +
        //                Environment.NewLine + Environment.NewLine + _portName + Environment.NewLine+"------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtsubjectinq$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + _inqSub + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtremarkinq$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + _inqRemark + Environment.NewLine +
        //                "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtqut_no$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + strAAGRefNo + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtvesselqut$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + strVesselName +Environment.NewLine+ "------" + boundary + Environment.NewLine +
        //                "Content-Disposition: form-data; name=\"ttxtissued_namequt$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + _issueName + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtfrto_due_datequt$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + _frmDueQuote + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxttoto_due_datequt$thinkbasetextbox\"" +
        //                Environment.NewLine + Environment.NewLine + _toDueQuote + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtissued_datequt$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + _issueDateQuote + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtdue_portqut$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + _portCodeQuote +
        //                Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtportnamequt$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + _portNameQuote + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tftlstbxreportfile$selecttextbox\"" + Environment.NewLine + Environment.NewLine + _attchQuoteFile + Environment.NewLine + "------" + boundary + Environment.NewLine +
        //                "Content-Disposition: form-data; name=\"tftlstbxreportfile$fileupload\"; filename=\"\"" + Environment.NewLine + "Content-Type: application/octet-stream" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtvalidperiod$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + strExpDate + Environment.NewLine + "------" + boundary + Environment.NewLine +
        //                "Content-Disposition: form-data; name=\"ttxtpaymentcond$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + strPayTerms + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtsubjectqut$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + ConfigurationManager.AppSettings["QUOTESUBJECT"].Trim() + Environment.NewLine + "------" + boundary + Environment.NewLine +
        //                "Content-Disposition: form-data; name=\"ttxtremarkqut$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + strSupplierComment.Trim() + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tddlqut_curr_code$thinkbasedropdownlist\"" + Environment.NewLine + Environment.NewLine + strCurrency + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtqut_amt$thinkbasetextbox\"" +
        //                Environment.NewLine + Environment.NewLine + strTotalLineItemsAmount + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtqut_shg$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + strCharges + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtqut_ttl$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + grandTotal + Environment.NewLine +
        //                "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtqut_shg_rmk$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + "Freight Cost: " + strFreightCharge + " Packing Cost: " + strPackingCost + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtdelivery$thinkbasetextbox\"" + Environment.NewLine + Environment.NewLine + ""+Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tddlgeim$thinkbasedropdownlist\"" +
        //                Environment.NewLine + Environment.NewLine + _value + Environment.NewLine;

        //                string LeadDays = "", inq_item_rmk = "", qut_item_rmk = "", counter = ""
        //                    ,xmlData="<?xml version=\"1.0\" encoding=\"euc-kr\"?><xml_data>";
        //                #region Items
        //                HtmlNodeCollection _itemsColl = htmlDoc.DocumentNode.SelectNodes("//table[@id='tmgridInqList']//tr[@part_key]");
        //                if (_itemsColl != null)
        //                {
        //                    if (_itemsColl.Count > 0)
        //                    {
        //                        if (_lineitem.Count > 0)
        //                        {
        //                            if (_itemsColl.Count == _lineitem.Count)
        //                            {
        //                                foreach (HtmlNode _row in _itemsColl)
        //                                {
        //                                    HtmlNodeCollection _td = _row.ChildNodes;
        //                                    LineItem _item = null;
        //                                    string partKey = _row.GetAttributeValue("part_key", "").Trim();
        //                                    foreach (LineItem item in _lineitem)
        //                                    {
        //                                        if (item.OriginatingSystemRef.ToString() == partKey.ToString()) { _item = item; }
        //                                    }
        //                                    if (_item != null)
        //                                    {
        //                                        string _price = "", _discount = "";
        //                                        foreach (PriceDetails _priceDetails in _item.PriceList)
        //                                        {
        //                                            if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.GRP) _price = _priceDetails.Value.ToString("0.00");
        //                                            else if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.DPR) _discount = _priceDetails.Value.ToString("0.00");
        //                                        }

        //                                        if (_item.DeleiveryTime != "0" || _item.DeleiveryTime != null)
        //                                            LeadDays = _item.DeleiveryTime;
        //                                        else LeadDays = "";

        //                                        if (_td[14].ChildNodes.Count > 1)
        //                                        {
        //                                            inq_item_rmk = _td[14].ChildNodes[1].GetAttributeValue("value", "").Trim();
        //                                            string _counter = _td[14].ChildNodes[1].GetAttributeValue("name", "").Trim();
        //                                            string[] arrCounter = _counter.Split('$');
        //                                            counter = arrCounter[1];
        //                                        }
        //                                        if (_td[15].ChildNodes.Count > 1)
        //                                        {
        //                                            qut_item_rmk = _td[15].ChildNodes[1].GetAttributeValue("value", "").Trim();
        //                                        }

        //                                        #region add xml data
        //                                        xmlData = xmlData + "<detail><PART_NO>";
        //                                        string _partNo = _td[1].InnerText.Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
        //                                        string _partDescr = _td[3].InnerText.Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
        //                                        string _unit = _td[4].InnerText.Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
        //                                        string _qtyInq = _td[5].InnerText.Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
        //                                        string _qtyQut = Convert.ToString(_item.Quantity);// _td[6].InnerText.Replace("&nbsp;", "").Trim();
        //                                        string _contPrice = _td[7].InnerText.Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
        //                                        string _qutUnitPrice = Convert.ToString(_price);
        //                                        string _qutUnitSum = Convert.ToString(_item.MonetaryAmount);
        //                                        string _ohEdition = _td[10].InnerText.Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
        //                                        string _suppEdition = _td[11].InnerText.Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
        //                                        string _delivery = _item.DeleiveryTime + "Days";
        //                                        string _chkGeEq = htmlDoc.DocumentNode.SelectSingleNode("//select[@name='tmgridInqList$" + counter + "$CHK_GE_EQ']/option[@selected]").GetAttributeValue("value", "").Trim();
        //                                        string _eCode = _row.GetAttributeValue("E_CODE", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
        //                                        string _sCode = _row.GetAttributeValue("S_CODE", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
        //                                        string _partKey = _row.GetAttributeValue("PART_KEY", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
        //                                        string _sSort = _row.GetAttributeValue("S_SORT", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
        //                                        string _mCode = _row.GetAttributeValue("M_CODE", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
        //                                        string _kCode = _row.GetAttributeValue("K_CODE", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
        //                                        string _tCode = _row.GetAttributeValue("T_CODE", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
        //                                        string _partCode = _row.GetAttributeValue("PART_CODE", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
        //                                        string _equKind = _row.GetAttributeValue("EQU_KIND", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();

        //                                        xmlData = xmlData + _partNo + "</PART_NO><PART_DESC>" + _partDescr + "</PART_DESC><UNIT>" + _unit + "</UNIT><QTY_INQ>" + _qtyInq + "</QTY_INQ><QTY_QUT>" +
        //                                          _qtyQut + "</QTY_QUT><CONT_PRICE>" + _contPrice + "</CONT_PRICE><QUT_UNIT_PRICE>" + _qutUnitPrice + "</QUT_UNIT_PRICE><QUT_UNIT_SUM>" + _qutUnitSum +
        //                                          "</QUT_UNIT_SUM><OH_EDITION>" + _ohEdition + "</OH_EDITION><SUPPLY_EDITION>" + _suppEdition + "</SUPPLY_EDITION><DELIVERY_KIND>" + _delivery +
        //                                          "</DELIVERY_KIND><CHK_GE_EQ>" + _chkGeEq + "</CHK_GE_EQ><INQ_ITEM_RMK>" + inq_item_rmk + "</INQ_ITEM_RMK><QUT_ITEM_RMK>" + qut_item_rmk +
        //                                          "</QUT_ITEM_RMK><E_CODE>" + _eCode + "</E_CODE><S_CODE>" + _sCode + "</S_CODE><PART_KEY>" + _partKey + "</PART_KEY><S_SORT>" + _sSort + "</S_SORT><M_CODE>" +
        //                                          _mCode + "</M_CODE><K_CODE>" + _kCode + "</K_CODE><T_CODE>" + _tCode + "</T_CODE><PART_CODE>" + _partCode + "</PART_CODE><EQU_KIND>" + _equKind + "</EQU_KIND></detail>";
        //                                        #endregion

        //                                        strPostData = strPostData + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridinqlist$" + counter + "$qty_qut\"" + Environment.NewLine + Environment.NewLine + _item.Quantity + Environment.NewLine +
        //                                            "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridinqlist$" + counter + "$qut_unit_price\"" + Environment.NewLine + Environment.NewLine + _price + Environment.NewLine + "------" + boundary + Environment.NewLine +
        //                                            "Content-Disposition: form-data; name=\"tmgridinqlist$" + counter + "$supply_edition\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridinqlist$" + counter + "$delivery_kind\"" +
        //                                            Environment.NewLine + Environment.NewLine + LeadDays + "Days" + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridinqlist$" + counter + "$chk_ge_eq\"" + Environment.NewLine + Environment.NewLine + "GE" + Environment.NewLine +
        //                                            "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridinqlist$" + counter + "$inq_item_rmk\"" + Environment.NewLine + Environment.NewLine + inq_item_rmk + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridinqlist$" + counter + "$qut_item_rmk\"" +
        //                                            Environment.NewLine + Environment.NewLine + qut_item_rmk + Environment.NewLine;

        //                                    }
        //                                    // counter = counter + 2;
        //                                }
        //                                xmlData =xmlData+ "</xml_data>";
        //                            }
        //                            else
        //                            {
        //                                MoveToError("Unable to upload Quote due to item count mismatched.", strUCRefNo, htmlDoc, MTML_QuoteFile);
        //                                result = false;
        //                            }
        //                        }
        //                    }
        //                    //strRFQData = _httproutine.SendSaveRequestFormData(strNavURL, strPostData, "----" + boundary);
        //                    //htmlDoc.LoadHtml(strRFQData);
        //                }
        //                #endregion

        //                string _reqSysNo = "", _inqSysNo = "", _qutSysNo = "", _inqSys = "", _vdrCode = "", _vdrName = "", _subject = "", _partKind = "", _vessel = "", _cus_odr_no = "",
        //                    _dept="",_issuedNameEng="",_odr_fm_email="",_odr_to_email="",_reportUrl="";
        //                HtmlNode _hreqSysNo = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtReqSysNo']");
        //                if (_hreqSysNo != null) _reqSysNo = _hreqSysNo.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hinqSysNo = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtInqSysNo']");
        //                if (_hinqSysNo != null) _inqSysNo = _hinqSysNo.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hqutSysNo = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtQutSysNo']");
        //                if (_hqutSysNo != null) _qutSysNo = _hqutSysNo.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hinqSys = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtInqSys']");
        //                if (_hinqSys != null) _inqSys = _hinqSys.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hvdrCode = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtVdrCode']");
        //                if (_hvdrCode != null) _vdrCode = _hvdrCode.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hvdrName = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtVDR_NAME']");
        //                if (_hvdrName != null) _vdrName = _hvdrName.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hsubject = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtSUBJECT']");
        //                if (_hsubject != null) _subject = _hsubject.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hpartKind = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='hiddtxtPartKind']");
        //                if (_hpartKind != null) _partKind = _hpartKind.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hvessel = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtVessel']");
        //                if (_hvessel != null) _vessel = _hvessel.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hcus_odr_no = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtCUS_ODR_NO']");
        //                if (_hcus_odr_no != null) _cus_odr_no = _hcus_odr_no.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hdept = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtDept']");
        //                if (_hdept != null) _dept = _hdept.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hissuedNameEng = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssuedNameEng']");
        //                if (_hissuedNameEng != null) _issuedNameEng = _hissuedNameEng.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hodr_fm_email = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtODR_FM_Email']");
        //                if (_hodr_fm_email != null) _odr_fm_email = _hodr_fm_email.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hodr_to_email = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtODR_TO_Email']");
        //                if (_hodr_to_email != null) _odr_to_email = _hodr_to_email.GetAttributeValue("value", "").Trim();

        //                HtmlNode _hreportUrl = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ReportUrl']");
        //                if (_hreportUrl != null) _reportUrl = _hreportUrl.GetAttributeValue("value", "").Trim();

        //                strPostData = strPostData + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtreqsysno\"" + Environment.NewLine + Environment.NewLine + _reqSysNo + Environment.NewLine +
        //                    "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtinqsysno\"" + Environment.NewLine + Environment.NewLine + _inqSysNo + Environment.NewLine + "------" + boundary + Environment.NewLine +
        //                    "Content-Disposition: form-data; name=\"ttxtqutsysno\"" + Environment.NewLine + Environment.NewLine + _qutSysNo + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtinqsys\"" +
        //                    Environment.NewLine + Environment.NewLine + _inqSys + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtvdrcode\"" + Environment.NewLine + Environment.NewLine +
        //                    _vdrCode + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtvdr_name\"" + Environment.NewLine + Environment.NewLine + _vdrName + Environment.NewLine +
        //                    "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtsubject\"" + Environment.NewLine + Environment.NewLine + _subject + Environment.NewLine + "------" + boundary +
        //                    Environment.NewLine + "Content-Disposition: form-data; name=\"hiddtxtpartkind\"" + Environment.NewLine + Environment.NewLine + _partKind + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtvessel\"" +
        //                    Environment.NewLine + Environment.NewLine + _vessel + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtcus_odr_no\"" + Environment.NewLine + Environment.NewLine +
        //                    _cus_odr_no + Environment.NewLine+"------"+boundary+Environment.NewLine+"Content-Disposition: form-data; name=\"ttxtdept\""+Environment.NewLine+Environment.NewLine+_dept+Environment.NewLine+"------"+boundary+Environment.NewLine+
        //                    "Content-Disposition: form-data; name=\"ttxtissuednameeng\"" + Environment.NewLine + Environment.NewLine + _issuedNameEng+Environment.NewLine+"------"+boundary+Environment.NewLine+"Content-Disposition: form-data; name=\"ttxtodr_fm_email\""+
        //                    Environment.NewLine+Environment.NewLine+_odr_fm_email+Environment.NewLine+"------"+boundary+Environment.NewLine+"Content-Disposition: form-data; name=\"ttxtodr_to_email\""+Environment.NewLine+Environment.NewLine+_odr_to_email+
        //                    "------"+boundary+Environment.NewLine+"Content-Disposition: form-data; name=\"txthiddxmldata\""+Environment.NewLine+Environment.NewLine+xmlData+Environment.NewLine+"------"+boundary+Environment.NewLine+"Content-Disposition: form-data; name=\"reporturl\""+
        //                    Environment.NewLine + Environment.NewLine + System.Web.HttpUtility.HtmlDecode(_reportUrl) + Environment.NewLine + "------" + boundary + "--";

        //                string strData = _httproutine.SendSaveRequestFormData(strNavURL, strPostData, "----" + boundary);
        //             //   _httproutine.Request_client_cidoship_com(strPostData,strNavURL);
        //            }
        //        }
        //        else
        //        {
        //            MoveToError("Unable to upload Quote due to total amount mismatched.", strUCRefNo, null, MTML_QuoteFile);
        //        }
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogText = "Exception while saving quote " + ex.Message;
        //        return false;
        //    }
           
        //}

        public bool SaveQuote(HtmlAgilityPack.HtmlDocument htmlDoc, string strNavURL, string MTML_QuoteFile)
        {
            bool result = false; int mismatch = 0;
            try
            {
                string strCharges = Convert.ToString(Convert.ToDouble(strFreightCharge) + Convert.ToDouble(strPackingCost));
                string grandTotal = Convert.ToString(Convert.ToDouble(strTotalLineItemsAmount) + Convert.ToDouble(strCharges));
                if (Convert.ToInt32(grandTotal) == Convert.ToInt32(strGrandTotal)){ mismatch = 0; }
                else if (strBuyerTotal != "")//added by kalpita on 18/11/2019
                {
                    if (Convert.ToInt32(Convert.ToDouble(grandTotal)) == Convert.ToInt32(Convert.ToDouble(strBuyerTotal)))
                    {
                        mismatch = 0;
                    }
                }
                else { mismatch = 1; }
                if(mismatch ==0)
                {
                    var boundary = "WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x", System.Globalization.NumberFormatInfo.InvariantInfo);// "WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x");//
                    Dictionary<string, string> dictState = _httproutine.GetStateInfo(htmlDoc,true);
                    if (dictState != null)
                    {
                        string _inqVessel = "", _inqIssueName = "", _frmDue = "", _toDue = "", _issueDate = "", _inqPortCode = "", _portName = "", _inqSub = "", _inqRemark = "", _issueName = "", _frmDueQuote = "", _toDueQuote = "", _issueDateQuote = "", _portCodeQuote = "", _portNameQuote = "", _attchQuoteFile = "", _value = "";

                        HtmlNode _hinqVessel = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtVesselInq_ThinkBaseTextBox']");
                        if (_hinqVessel != null) _inqVessel = _hinqVessel.GetAttributeValue("value", "").Trim();

                        HtmlNode _hinqIssueName = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssued_NameInq_ThinkBaseTextBox']");
                        if (_hinqIssueName != null) _inqIssueName = _hinqIssueName.GetAttributeValue("value", "").Trim();

                        HtmlNode _hfrmDue = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtFrTO_DUE_DATEInq_ThinkBaseTextBox']");
                        if (_hfrmDue != null) _frmDue = _hfrmDue.GetAttributeValue("value", "").Trim();

                        HtmlNode _htoDue = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtToTO_DUE_DATEInq_ThinkBaseTextBox']");
                        if (_htoDue != null) _toDue = _htoDue.GetAttributeValue("value", "").Trim();

                        HtmlNode _hissueDate = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssued_DateInq_ThinkBaseTextBox']");
                        if (_hissueDate != null) _issueDate = _hissueDate.GetAttributeValue("value", "").Trim();

                        HtmlNode _hinqPortCode = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtDUE_PORTInq_ThinkBaseTextBox']");
                        if (_hinqPortCode != null) _inqPortCode = _hinqPortCode.GetAttributeValue("value", "").Trim();

                        HtmlNode _hportName = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtPortNameInq_ThinkBaseTextBox']");
                        if (_hportName != null) _portName = _hportName.GetAttributeValue("value", "").Trim();

                        HtmlNode _hinqSub = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtSubjectInq_ThinkBaseTextBox']");
                        if (_hinqSub != null) _inqSub = _hinqSub.GetAttributeValue("value", "").Trim();

                        HtmlNode _hinqRemark = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtRemarkInq_ThinkBaseTextBox']");
                        if (_hinqRemark != null) _inqRemark = _hinqRemark.GetAttributeValue("value", "").Trim();

                        HtmlNode _hissueName = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssued_NameQut_ThinkBaseTextBox']");
                        if (_hissueName != null) _issueName = _hissueName.GetAttributeValue("value", "").Trim();

                        HtmlNode _hfrmDueQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtFrTO_DUE_DATEQut_ThinkBaseTextBox']");
                        if (_hfrmDueQuote != null) _frmDueQuote = _hfrmDueQuote.GetAttributeValue("value", "").Trim();

                        HtmlNode _htoDueQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtToTO_DUE_DATEQut_ThinkBaseTextBox']");
                        if (_htoDueQuote != null) _toDueQuote = _htoDueQuote.GetAttributeValue("value", "").Trim();

                        HtmlNode _hissueDateQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssued_DateQut_ThinkBaseTextBox']");
                        if (_hissueDateQuote != null) _issueDateQuote = _hissueDateQuote.GetAttributeValue("value", "").Trim();

                        HtmlNode _hportCodeQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtDUE_PORTQut_ThinkBaseTextBox']");
                        if (_hportCodeQuote != null) _portCodeQuote = _hportCodeQuote.GetAttributeValue("value", "").Trim();

                        HtmlNode _hportNameQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtPortNameQut_ThinkBaseTextBox']");
                        if (_hportNameQuote != null) _portNameQuote = _hportNameQuote.GetAttributeValue("value", "").Trim();

                        HtmlNode _hattachQuoteFile = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='tftlstbxReportFile_SelectTextBox']");
                        if (_hattachQuoteFile != null) _attchQuoteFile = _hattachQuoteFile.GetAttributeValue("value", "").Trim();


                        HtmlNodeCollection _selectGeim = htmlDoc.DocumentNode.SelectNodes("//select[@id='tddlGeIm_ThinkBaseDropDownList']/option[@selected]");
                        if (_selectGeim != null)
                        {
                            _value = _selectGeim[0].GetAttributeValue("value", "").Trim();
                        }

                        string strPostData = @"------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"radScript_TSM\"" + Environment.NewLine + Environment.NewLine + ";;System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35:ko-KR:c97801cf-c4e9-421a-bd07-262d424faf76:ea597d4b:b25378d2" + Environment.NewLine + "------" +
                            boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__EVENTTARGET\"" + Environment.NewLine + Environment.NewLine + "timgbtnSave" + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__EVENTARGUMENT\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine +
                            "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__VIEWSTATE\"" + Environment.NewLine + Environment.NewLine + Uri.UnescapeDataString(dictState["__VIEWSTATE"]) + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__VIEWSTATEGENERATOR\"" + Environment.NewLine + Environment.NewLine + dictState["__VIEWSTATEGENERATOR"] +
                            Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__SCROLLPOSITIONX\"" + Environment.NewLine + Environment.NewLine + "0" + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__SCROLLPOSITIONY\"" + Environment.NewLine + Environment.NewLine + "0" + Environment.NewLine + "------" + boundary +
                            Environment.NewLine + "Content-Disposition: form-data; name=\"__EVENTVALIDATION\"" + Environment.NewLine + Environment.NewLine + Uri.UnescapeDataString(dictState["__EVENTVALIDATION"]) + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtInq_NO$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strUCRefNo + Environment.NewLine + "------" + boundary + Environment.NewLine +
                            "Content-Disposition: form-data; name=\"ttxtVesselInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _inqVessel + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtIssued_NameInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _inqIssueName + Environment.NewLine + "------" + boundary + Environment.NewLine +
                        "Content-Disposition: form-data; name=\"ttxtFrTO_DUE_DATEInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _frmDue + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtToTO_DUE_DATEInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _toDue + Environment.NewLine + "------" + boundary + Environment.NewLine +
                        "Content-Disposition: form-data; name=\"ttxtIssued_DateInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _issueDate + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtDUE_PORTInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _inqPortCode + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtPortNameInq$ThinkBaseTextBox\"" +
                        Environment.NewLine + Environment.NewLine + _portName + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtSubjectInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _inqSub + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtRemarkInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _inqRemark + Environment.NewLine +
                        "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtQut_NO$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strAAGRefNo + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtVesselQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strVesselName + Environment.NewLine + "------" + boundary + Environment.NewLine +
                        "Content-Disposition: form-data; name=\"ttxtIssued_NameQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _issueName + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtFrTO_DUE_DATEQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _frmDueQuote + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtToTO_DUE_DATEQut$ThinkBaseTextBox\"" +
                        Environment.NewLine + Environment.NewLine + _toDueQuote + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtIssued_DateQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _issueDateQuote + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtDUE_PORTQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _portCodeQuote +
                        Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtPortNameQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _portNameQuote + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tftlstbxReportFile$SelectTextBox\"" + Environment.NewLine + Environment.NewLine + _attchQuoteFile + Environment.NewLine + "------" + boundary + Environment.NewLine +
                        "Content-Disposition: form-data; name=\"tftlstbxReportFile$FileUpload\"; filename=\"\"" + Environment.NewLine + "Content-Type: application/octet-stream" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtValidperiod$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strExpDate + Environment.NewLine + "------" + boundary + Environment.NewLine +
                        "Content-Disposition: form-data; name=\"ttxtPaymentCond$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strPayTerms + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtSubjectQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + ConfigurationManager.AppSettings["QUOTESUBJECT"].Trim() + Environment.NewLine + "------" + boundary + Environment.NewLine +
                        "Content-Disposition: form-data; name=\"ttxtRemarkQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strSupplierComment.Trim() + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tddlQUT_CURR_CODE$ThinkBaseDropDownList\"" + Environment.NewLine + Environment.NewLine + strCurrency + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtQUT_AMT$ThinkBaseTextBox\"" +
                        Environment.NewLine + Environment.NewLine + strTotalLineItemsAmount + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtQUT_SHG$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strCharges + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtQUT_TTL$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + grandTotal + Environment.NewLine +
                        "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtQUT_SHG_RMK$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + "Freight Cost: " + strFreightCharge + " Packing Cost: " + strPackingCost + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtDelivery$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tddlGeIm$ThinkBaseDropDownList\"" +
                        Environment.NewLine + Environment.NewLine + _value + Environment.NewLine;

                        string LeadDays = "", inq_item_rmk = "", qut_item_rmk = "", counter = "";
                        xmlData = "";
                             xmlData = "<?xml version=\"1.0\" encoding=\"euc-kr\"?><xml_data>";
                        #region Items
                        HtmlNodeCollection _itemsColl = htmlDoc.DocumentNode.SelectNodes("//table[@id='tmgridInqList']//tr[@part_key]");
                        if (_itemsColl != null)
                        {
                            if (_itemsColl.Count > 0)
                            {
                                if (_lineitem.Count > 0)
                                {
                                    if (_itemsColl.Count == _lineitem.Count)
                                    {
                                        foreach (HtmlNode _row in _itemsColl)
                                        {
                                            HtmlNodeCollection _td = _row.ChildNodes;
                                            LineItem _item = null;
                                            string partKey = _row.GetAttributeValue("part_key", "").Trim();
                                            foreach (LineItem item in _lineitem)
                                            {
                                                if (item.OriginatingSystemRef.ToString() == partKey.ToString()) { _item = item; }
                                            }
                                            if (_item != null)
                                            {
                                                string _price = "", _discount = "";
                                                foreach (PriceDetails _priceDetails in _item.PriceList)
                                                {
                                                    if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.GRP) _price = _priceDetails.Value.ToString("0.00");
                                                    else if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.DPR) _discount = _priceDetails.Value.ToString("0.00");
                                                }

                                                if (_item.DeleiveryTime != "0" || _item.DeleiveryTime != null)
                                                    LeadDays = _item.DeleiveryTime;
                                                else LeadDays = "";

                                                if (_td[14].ChildNodes.Count > 1)
                                                {
                                                    inq_item_rmk = _td[14].ChildNodes[1].GetAttributeValue("value", "").Trim();
                                                    string _counter = _td[14].ChildNodes[1].GetAttributeValue("name", "").Trim();
                                                    string[] arrCounter = _counter.Split('$');
                                                    counter = arrCounter[1];
                                                }
                                                if (_td[15].ChildNodes.Count > 1)
                                                {
                                                    qut_item_rmk = _td[15].ChildNodes[1].GetAttributeValue("value", "").Trim();
                                                }

                                                #region add xml data
                                                xmlData = xmlData + "<detail><PART_NO>";
                                                string _partNo = _td[2].InnerText.Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
                                                string _partDescr = _td[3].InnerText.Replace("&nbsp;", "");//.Replace("&amp;", "&").Trim();
                                                string _unit = _td[4].InnerText.Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
                                                string _qtyInq = _td[5].InnerText.Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
                                                string _qtyQut = Convert.ToString(_item.Quantity);// _td[6].InnerText.Replace("&nbsp;", "").Trim();
                                                string _contPrice = _td[7].InnerText.Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
                                                string _qutUnitPrice = Convert.ToString(_price);
                                                string _qutUnitSum = Convert.ToString(_item.MonetaryAmount);
                                                string _ohEdition = _td[10].InnerText.Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
                                                string _suppEdition = _td[11].InnerText.Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
                                                string _delivery = _item.DeleiveryTime + "Days";
                                                string _chkGeEq = htmlDoc.DocumentNode.SelectSingleNode("//select[@name='tmgridInqList$" + counter + "$CHK_GE_EQ']/option[@selected]").GetAttributeValue("value", "").Trim();
                                                string _eCode = _row.GetAttributeValue("E_CODE", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
                                                string _sCode = _row.GetAttributeValue("S_CODE", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
                                                string _partKey = _row.GetAttributeValue("PART_KEY", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
                                                string _sSort = _row.GetAttributeValue("S_SORT", "").Replace("&nbsp;", "").Trim();//.Replace("&amp;", "&")
                                                string _mCode = _row.GetAttributeValue("M_CODE", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
                                                string _kCode = _row.GetAttributeValue("K_CODE", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
                                                string _tCode = _row.GetAttributeValue("T_CODE", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
                                                string _partCode = _row.GetAttributeValue("PART_CODE", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
                                                string _equKind = _row.GetAttributeValue("EQU_KIND", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();

                                                xmlData = xmlData + _partNo + "</PART_NO><PART_DESC>" + _partDescr + "</PART_DESC><UNIT>" + _unit + "</UNIT><QTY_INQ>" + _qtyInq + "</QTY_INQ><QTY_QUT>" +
                                                  _qtyQut + "</QTY_QUT><CONT_PRICE>" + _contPrice + "</CONT_PRICE><QUT_UNIT_PRICE>" + _qutUnitPrice + "</QUT_UNIT_PRICE><QUT_UNIT_SUM>" + _qutUnitSum +
                                                  "</QUT_UNIT_SUM><OH_EDITION>" + _ohEdition + "</OH_EDITION><SUPPLY_EDITION>" + _suppEdition + "</SUPPLY_EDITION><DELIVERY_KIND>" + _delivery +
                                                  "</DELIVERY_KIND><CHK_GE_EQ>" + _chkGeEq + "</CHK_GE_EQ><INQ_ITEM_RMK>" + inq_item_rmk + "</INQ_ITEM_RMK><QUT_ITEM_RMK>" + qut_item_rmk +
                                                  "</QUT_ITEM_RMK><E_CODE>" + _eCode + "</E_CODE><S_CODE>" + _sCode + "</S_CODE><PART_KEY>" + _partKey + "</PART_KEY><S_SORT>" + _sSort + "</S_SORT><M_CODE>" +
                                                  _mCode + "</M_CODE><K_CODE>" + _kCode + "</K_CODE><T_CODE>" + _tCode + "</T_CODE><PART_CODE>" + _partCode + "</PART_CODE><EQU_KIND>" + _equKind + "</EQU_KIND></detail>";
                                                #endregion

                                                strPostData = strPostData + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridInqList$" + counter + "$QTY_QUT\"" + Environment.NewLine + Environment.NewLine + _item.Quantity + Environment.NewLine +
                                                    "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridInqList$" + counter + "$QUT_UNIT_PRICE\"" + Environment.NewLine + Environment.NewLine + _price + Environment.NewLine + "------" + boundary + Environment.NewLine +
                                                    "Content-Disposition: form-data; name=\"tmgridInqList$" + counter + "$SUPPLY_EDITION\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridInqList$" + counter + "$DELIVERY_KIND\"" +
                                                    Environment.NewLine + Environment.NewLine + LeadDays + "Days" + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridInqList$" + counter + "$CHK_GE_EQ\"" + Environment.NewLine + Environment.NewLine + "GE" + Environment.NewLine +
                                                    "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridInqList$" + counter + "$INQ_ITEM_RMK\"" + Environment.NewLine + Environment.NewLine + inq_item_rmk + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridInqList$" + counter + "$QUT_ITEM_RMK\"" +
                                                    Environment.NewLine + Environment.NewLine + qut_item_rmk + Environment.NewLine;

                                            }
                                            // counter = counter + 2;
                                        }
                                        xmlData = xmlData + "</xml_data>";
                                    }
                                    else
                                    {
                                        //MoveToError("Unable to upload Quote due to item count mismatched.", strUCRefNo, htmlDoc, MTML_QuoteFile);
                                        MoveToError("Unable to save file due to item count mismatch.", strUCRefNo, htmlDoc, MTML_QuoteFile, "LeS-1008.4:");                                     
                                        result = false;
                                    }
                                }
                            }
                            //strRFQData = _httproutine.SendSaveRequestFormData(strNavURL, strPostData, "----" + boundary);
                            //htmlDoc.LoadHtml(strRFQData);
                        }
                        #endregion

                        string _reqSysNo = "", _inqSysNo = "", _qutSysNo = "", _inqSys = "", _vdrCode = "", _vdrName = "", _subject = "", _partKind = "", _vessel = "", _cus_odr_no = "",
                            _dept = "", _issuedNameEng = "", _odr_fm_email = "", _odr_to_email = "", _reportUrl = "";
                        HtmlNode _hreqSysNo = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtReqSysNo']");
                        if (_hreqSysNo != null) _reqSysNo = _hreqSysNo.GetAttributeValue("value", "").Trim();

                        HtmlNode _hinqSysNo = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtInqSysNo']");
                        if (_hinqSysNo != null) _inqSysNo = _hinqSysNo.GetAttributeValue("value", "").Trim();

                        HtmlNode _hqutSysNo = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtQutSysNo']");
                        if (_hqutSysNo != null) _qutSysNo = _hqutSysNo.GetAttributeValue("value", "").Trim();

                        HtmlNode _hinqSys = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtInqSys']");
                        if (_hinqSys != null) _inqSys = _hinqSys.GetAttributeValue("value", "").Trim();

                        HtmlNode _hvdrCode = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtVdrCode']");
                        if (_hvdrCode != null) _vdrCode = _hvdrCode.GetAttributeValue("value", "").Trim();

                        HtmlNode _hvdrName = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtVDR_NAME']");
                        if (_hvdrName != null) _vdrName = _hvdrName.GetAttributeValue("value", "").Trim();

                        HtmlNode _hsubject = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtSUBJECT']");
                        if (_hsubject != null) _subject = _hsubject.GetAttributeValue("value", "").Trim();

                        HtmlNode _hpartKind = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='hiddtxtPartKind']");
                        if (_hpartKind != null) _partKind = _hpartKind.GetAttributeValue("value", "").Trim();

                        HtmlNode _hvessel = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtVessel']");
                        if (_hvessel != null) _vessel = _hvessel.GetAttributeValue("value", "").Trim();

                        HtmlNode _hcus_odr_no = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtCUS_ODR_NO']");
                        if (_hcus_odr_no != null) _cus_odr_no = _hcus_odr_no.GetAttributeValue("value", "").Trim();

                        HtmlNode _hdept = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtDept']");
                        if (_hdept != null) _dept = _hdept.GetAttributeValue("value", "").Trim();

                        HtmlNode _hissuedNameEng = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssuedNameEng']");
                        if (_hissuedNameEng != null) _issuedNameEng = _hissuedNameEng.GetAttributeValue("value", "").Trim();

                        HtmlNode _hodr_fm_email = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtODR_FM_Email']");
                        if (_hodr_fm_email != null) _odr_fm_email = _hodr_fm_email.GetAttributeValue("value", "").Trim();

                        HtmlNode _hodr_to_email = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtODR_TO_Email']");
                        if (_hodr_to_email != null) _odr_to_email = _hodr_to_email.GetAttributeValue("value", "").Trim();

                        HtmlNode _hreportUrl = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ReportUrl']");
                        if (_hreportUrl != null) _reportUrl = _hreportUrl.GetAttributeValue("value", "").Trim();

                        strPostData = strPostData + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtReqSysNo\"" + Environment.NewLine + Environment.NewLine + _reqSysNo + Environment.NewLine +
                            "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtInqSysNo\"" + Environment.NewLine + Environment.NewLine + _inqSysNo + Environment.NewLine + "------" + boundary + Environment.NewLine +
                            "Content-Disposition: form-data; name=\"ttxtQutSysNo\"" + Environment.NewLine + Environment.NewLine + _qutSysNo + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtInqSys\"" +
                            Environment.NewLine + Environment.NewLine + _inqSys + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtVdrCode\"" + Environment.NewLine + Environment.NewLine +
                            _vdrCode + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtVDR_NAME\"" + Environment.NewLine + Environment.NewLine + _vdrName + Environment.NewLine +
                            "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtSUBJECT\"" + Environment.NewLine + Environment.NewLine + _subject + Environment.NewLine + "------" + boundary +
                            Environment.NewLine + "Content-Disposition: form-data; name=\"hiddtxtPartKind\"" + Environment.NewLine + Environment.NewLine + _partKind + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtVessel\"" +
                            Environment.NewLine + Environment.NewLine + _vessel + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtCUS_ODR_NO\"" + Environment.NewLine + Environment.NewLine +
                            _cus_odr_no + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtDept\"" + Environment.NewLine + Environment.NewLine + _dept + Environment.NewLine + "------" + boundary + Environment.NewLine +
                            "Content-Disposition: form-data; name=\"ttxtIssuedNameEng\"" + Environment.NewLine + Environment.NewLine + _issuedNameEng + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtODR_FM_Email\"" +
                            Environment.NewLine + Environment.NewLine + _odr_fm_email + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtODR_TO_Email\"" + Environment.NewLine + Environment.NewLine + _odr_to_email +
                            "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"txthiddXmlData\"" + Environment.NewLine + Environment.NewLine + xmlData + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ReportUrl\"" +
                            Environment.NewLine + Environment.NewLine + System.Web.HttpUtility.HtmlDecode(_reportUrl) + Environment.NewLine + "------" + boundary + "--";

                        string strData = _httproutine.SendSaveRequestFormData(strNavURL, strPostData, "----" + boundary);
                        if (strData != "")
                        {
                            htmlDoc.LoadHtml(strData); HtmlNode _btnOffer = htmlDoc.DocumentNode.SelectSingleNode("//a[@id='timgbtnOffer']");
                            if (_btnOffer != null) result = true;
                            else result = false;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }
                else
                {
                    //MoveToError("Unable to upload Quote due to total amount mismatched.", strUCRefNo, null, MTML_QuoteFile);
                    MoveToError("Unable to save quote due to amount mismatch.", strUCRefNo, null, MTML_QuoteFile, "LeS-1008.1:");
                }
                return result;
            }
            catch (Exception ex)
            {
                LogText = "Exception while saving quote " + ex.Message;
                return false;
            }

        }

        public bool Process_Offer(HtmlAgilityPack.HtmlDocument htmlDoc, string strNavURL, string MTML_QuoteFile)
        {
            bool result = false;
            try
            {
                 string strCharges = Convert.ToString(Convert.ToDouble(strFreightCharge) + Convert.ToDouble(strPackingCost));
                    string grandTotal = Convert.ToString(Convert.ToDouble(strTotalLineItemsAmount) + Convert.ToDouble(strCharges));

                var boundary = "WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x", System.Globalization.NumberFormatInfo.InvariantInfo);// "WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x");//
                Dictionary<string, string> dictState = _httproutine.GetStateInfo(htmlDoc,true);
                if (dictState != null)
                {
                    string _inqVessel = "", _inqIssueName = "", _frmDue = "", _toDue = "", _issueDate = "", _inqPortCode = "", _portName = "", _inqSub = "", _inqRemark = "", _issueName = "", _frmDueQuote = "", _toDueQuote = "", _issueDateQuote = "", _portCodeQuote = "", _portNameQuote = "", _attchQuoteFile = "", _value = "";

                    HtmlNode _hinqVessel = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtVesselInq_ThinkBaseTextBox']");
                    if (_hinqVessel != null) _inqVessel = _hinqVessel.GetAttributeValue("value", "").Trim();

                    HtmlNode _hinqIssueName = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssued_NameInq_ThinkBaseTextBox']");
                    if (_hinqIssueName != null) _inqIssueName = _hinqIssueName.GetAttributeValue("value", "").Trim();

                    HtmlNode _hfrmDue = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtFrTO_DUE_DATEInq_ThinkBaseTextBox']");
                    if (_hfrmDue != null) _frmDue = _hfrmDue.GetAttributeValue("value", "").Trim();

                    HtmlNode _htoDue = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtToTO_DUE_DATEInq_ThinkBaseTextBox']");
                    if (_htoDue != null) _toDue = _htoDue.GetAttributeValue("value", "").Trim();

                    HtmlNode _hissueDate = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssued_DateInq_ThinkBaseTextBox']");
                    if (_hissueDate != null) _issueDate = _hissueDate.GetAttributeValue("value", "").Trim();

                    HtmlNode _hinqPortCode = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtDUE_PORTInq_ThinkBaseTextBox']");
                    if (_hinqPortCode != null) _inqPortCode = _hinqPortCode.GetAttributeValue("value", "").Trim();

                    HtmlNode _hportName = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtPortNameInq_ThinkBaseTextBox']");
                    if (_hportName != null) _portName = _hportName.GetAttributeValue("value", "").Trim();

                    HtmlNode _hinqSub = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtSubjectInq_ThinkBaseTextBox']");
                    if (_hinqSub != null) _inqSub = _hinqSub.GetAttributeValue("value", "").Trim();

                    HtmlNode _hinqRemark = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtRemarkInq_ThinkBaseTextBox']");
                    if (_hinqRemark != null) _inqRemark = _hinqRemark.GetAttributeValue("value", "").Trim();

                    HtmlNode _hissueName = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssued_NameQut_ThinkBaseTextBox']");
                    if (_hissueName != null) _issueName = _hissueName.GetAttributeValue("value", "").Trim();

                    HtmlNode _hfrmDueQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtFrTO_DUE_DATEQut_ThinkBaseTextBox']");
                    if (_hfrmDueQuote != null) _frmDueQuote = _hfrmDueQuote.GetAttributeValue("value", "").Trim();

                    HtmlNode _htoDueQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtToTO_DUE_DATEQut_ThinkBaseTextBox']");
                    if (_htoDueQuote != null) _toDueQuote = _htoDueQuote.GetAttributeValue("value", "").Trim();

                    HtmlNode _hissueDateQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssued_DateQut_ThinkBaseTextBox']");
                    if (_hissueDateQuote != null) _issueDateQuote = _hissueDateQuote.GetAttributeValue("value", "").Trim();

                    HtmlNode _hportCodeQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtDUE_PORTQut_ThinkBaseTextBox']");
                    if (_hportCodeQuote != null) _portCodeQuote = _hportCodeQuote.GetAttributeValue("value", "").Trim();

                    HtmlNode _hportNameQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtPortNameQut_ThinkBaseTextBox']");
                    if (_hportNameQuote != null) _portNameQuote = _hportNameQuote.GetAttributeValue("value", "").Trim();

                    string strPostData = @"------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"radScript_TSM\"" + Environment.NewLine + Environment.NewLine + ";;System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35:ko-KR:c97801cf-c4e9-421a-bd07-262d424faf76:ea597d4b:b25378d2" + Environment.NewLine + "------" +
                          boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__EVENTTARGET\"" + Environment.NewLine + Environment.NewLine + "timgbtnOffer" + Environment.NewLine +
                          "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__EVENTARGUMENT\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine +
                          "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__VIEWSTATE\"" + Environment.NewLine + Environment.NewLine + Uri.UnescapeDataString(dictState["__VIEWSTATE"]) + Environment.NewLine +
                           "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__VIEWSTATEGENERATOR\"" + Environment.NewLine + Environment.NewLine + dictState["__VIEWSTATEGENERATOR"] +
                          Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__EVENTVALIDATION\"" + Environment.NewLine + Environment.NewLine + Uri.UnescapeDataString(dictState["__EVENTVALIDATION"]) +
                          Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtInq_NO$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strUCRefNo + Environment.NewLine + "------" + boundary + Environment.NewLine +
                          "Content-Disposition: form-data; name=\"ttxtVesselInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _inqVessel + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtIssued_NameInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _inqIssueName + Environment.NewLine + "------" + boundary + Environment.NewLine +
                      "Content-Disposition: form-data; name=\"ttxtFrTO_DUE_DATEInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _frmDue + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtToTO_DUE_DATEInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _toDue + Environment.NewLine + "------" + boundary + Environment.NewLine +
                      "Content-Disposition: form-data; name=\"ttxtIssued_DateInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _issueDate + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtDUE_PORTInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _inqPortCode + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtPortNameInq$ThinkBaseTextBox\"" +
                      Environment.NewLine + Environment.NewLine + _portName + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtSubjectInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _inqSub + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtRemarkInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _inqRemark + Environment.NewLine +
                      "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtQut_NO$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strAAGRefNo + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtVesselQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strVesselName + Environment.NewLine + "------" + boundary + Environment.NewLine +
                      "Content-Disposition: form-data; name=\"ttxtIssued_NameQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _issueName + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtFrTO_DUE_DATEQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _frmDueQuote + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtToTO_DUE_DATEQut$ThinkBaseTextBox\"" +
                      Environment.NewLine + Environment.NewLine + _toDueQuote + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtIssued_DateQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _issueDateQuote + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtDUE_PORTQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _portCodeQuote +
                      Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtPortNameQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _portNameQuote + Environment.NewLine +
                       "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtValidperiod$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strExpDate + Environment.NewLine + "------" + boundary + Environment.NewLine +
                      "Content-Disposition: form-data; name=\"ttxtPaymentCond$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strPayTerms + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtSubjectQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + ConfigurationManager.AppSettings["QUOTESUBJECT"].Trim() + Environment.NewLine + "------" + boundary + Environment.NewLine +
                      "Content-Disposition: form-data; name=\"ttxtRemarkQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strSupplierComment.Trim() + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtQUT_AMT$ThinkBaseTextBox\"" +
                      Environment.NewLine + Environment.NewLine + strTotalLineItemsAmount + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtQUT_SHG$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strCharges + Environment.NewLine +
                       "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtQUT_TTL$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + grandTotal + Environment.NewLine +
                       "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtQUT_SHG_RMK$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + "Freight Cost: " + strFreightCharge + " Packing Cost: " + strPackingCost + Environment.NewLine +
                        "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtDelivery$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine;
                    #region Items
                    string inq_item_rmk = "", qut_item_rmk = "", counter = "";
                    HtmlNodeCollection _itemsColl = htmlDoc.DocumentNode.SelectNodes("//table[@id='tmgridInqList']//tr[@part_key]");
                    if (_itemsColl != null)
                    {
                        if (_itemsColl.Count > 0)
                        {
                            if (_lineitem.Count > 0)
                            {
                                if (_itemsColl.Count == _lineitem.Count)
                                {
                                    foreach (HtmlNode _row in _itemsColl)
                                    {
                                        HtmlNodeCollection _td = _row.ChildNodes;
                                        LineItem _item = null;
                                        string partKey = _row.GetAttributeValue("part_key", "").Trim();
                                        foreach (LineItem item in _lineitem)
                                        {
                                            if (item.OriginatingSystemRef.ToString() == partKey.ToString()) { _item = item; }
                                        }
                                        if (_item != null)
                                        {
                                            if (_td[14].ChildNodes.Count > 1)
                                            {
                                                inq_item_rmk = _td[14].ChildNodes[1].GetAttributeValue("value", "").Trim();
                                                string _counter = _td[14].ChildNodes[1].GetAttributeValue("name", "").Trim();
                                                string[] arrCounter = _counter.Split('$');
                                                counter = arrCounter[1];
                                            }
                                            if (_td[15].ChildNodes.Count > 1)
                                            {
                                                qut_item_rmk = _td[15].ChildNodes[1].GetAttributeValue("value", "").Trim();
                                            }
                                            strPostData = strPostData + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridInqList$" + counter + "$INQ_ITEM_RMK\"" + Environment.NewLine + Environment.NewLine + inq_item_rmk + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridInqList$" + counter + "$QUT_ITEM_RMK\"" +
                                              Environment.NewLine + Environment.NewLine + qut_item_rmk + Environment.NewLine;

                                        }

                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    string _reqSysNo = "", _inqSysNo = "", _qutSysNo = "", _inqSys = "", _vdrCode = "", _vdrName = "", _subject = "", _partKind = "", _vessel = "", _cus_odr_no = "",
                        _dept = "", _issuedNameEng = "", _odr_fm_email = "", _odr_to_email = "", _reportUrl = "";
                    HtmlNode _hreqSysNo = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtReqSysNo']");
                    if (_hreqSysNo != null) _reqSysNo = _hreqSysNo.GetAttributeValue("value", "").Trim();

                    HtmlNode _hinqSysNo = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtInqSysNo']");
                    if (_hinqSysNo != null) _inqSysNo = _hinqSysNo.GetAttributeValue("value", "").Trim();

                    HtmlNode _hqutSysNo = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtQutSysNo']");
                    if (_hqutSysNo != null) _qutSysNo = _hqutSysNo.GetAttributeValue("value", "").Trim();

                    HtmlNode _hinqSys = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtInqSys']");
                    if (_hinqSys != null) _inqSys = _hinqSys.GetAttributeValue("value", "").Trim();

                    HtmlNode _hvdrCode = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtVdrCode']");
                    if (_hvdrCode != null) _vdrCode = _hvdrCode.GetAttributeValue("value", "").Trim();

                    HtmlNode _hvdrName = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtVDR_NAME']");
                    if (_hvdrName != null) _vdrName = _hvdrName.GetAttributeValue("value", "").Trim();

                    HtmlNode _hsubject = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtSUBJECT']");
                    if (_hsubject != null) _subject = _hsubject.GetAttributeValue("value", "").Trim();

                    HtmlNode _hpartKind = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='hiddtxtPartKind']");
                    if (_hpartKind != null) _partKind = _hpartKind.GetAttributeValue("value", "").Trim();

                    HtmlNode _hvessel = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtVessel']");
                    if (_hvessel != null) _vessel = _hvessel.GetAttributeValue("value", "").Trim();

                    HtmlNode _hcus_odr_no = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtCUS_ODR_NO']");
                    if (_hcus_odr_no != null) _cus_odr_no = _hcus_odr_no.GetAttributeValue("value", "").Trim();

                    HtmlNode _hdept = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtDept']");
                    if (_hdept != null) _dept = _hdept.GetAttributeValue("value", "").Trim();

                    HtmlNode _hissuedNameEng = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssuedNameEng']");
                    if (_hissuedNameEng != null) _issuedNameEng = _hissuedNameEng.GetAttributeValue("value", "").Trim();

                    HtmlNode _hodr_fm_email = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtODR_FM_Email']");
                    if (_hodr_fm_email != null) _odr_fm_email = _hodr_fm_email.GetAttributeValue("value", "").Trim();

                    HtmlNode _hodr_to_email = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtODR_TO_Email']");
                    if (_hodr_to_email != null) _odr_to_email = _hodr_to_email.GetAttributeValue("value", "").Trim();

                    HtmlNode _hreportUrl = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ReportUrl']");
                    if (_hreportUrl != null) _reportUrl = _hreportUrl.GetAttributeValue("value", "").Trim();

                    strPostData = strPostData + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtReqSysNo\"" + Environment.NewLine + Environment.NewLine + _reqSysNo + Environment.NewLine +
                        "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtInqSysNo\"" + Environment.NewLine + Environment.NewLine + _inqSysNo + Environment.NewLine + "------" + boundary + Environment.NewLine +
                        "Content-Disposition: form-data; name=\"ttxtQutSysNo\"" + Environment.NewLine + Environment.NewLine + _qutSysNo + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtInqSys\"" +
                        Environment.NewLine + Environment.NewLine + _inqSys + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtVdrCode\"" + Environment.NewLine + Environment.NewLine +
                        _vdrCode + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtVDR_NAME\"" + Environment.NewLine + Environment.NewLine + _vdrName + Environment.NewLine +
                        "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtSUBJECT\"" + Environment.NewLine + Environment.NewLine + _subject + Environment.NewLine + "------" + boundary +
                        Environment.NewLine + "Content-Disposition: form-data; name=\"hiddtxtPartKind\"" + Environment.NewLine + Environment.NewLine + _partKind + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtVessel\"" +
                        Environment.NewLine + Environment.NewLine + _vessel + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtCUS_ODR_NO\"" + Environment.NewLine + Environment.NewLine +
                        _cus_odr_no + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtDept\"" + Environment.NewLine + Environment.NewLine + _dept + Environment.NewLine + "------" + boundary + Environment.NewLine +
                        "Content-Disposition: form-data; name=\"ttxtIssuedNameEng\"" + Environment.NewLine + Environment.NewLine + _issuedNameEng + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtODR_FM_Email\"" +
                        Environment.NewLine + Environment.NewLine + _odr_fm_email + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtODR_TO_Email\"" + Environment.NewLine + Environment.NewLine + _odr_to_email +
                        "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"txthiddXmlData\"" + Environment.NewLine + Environment.NewLine + xmlData + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ReportUrl\"" +
                        Environment.NewLine + Environment.NewLine + System.Web.HttpUtility.HtmlDecode(_reportUrl) + Environment.NewLine + "------" + boundary + "--";
                    string strData = _httproutine.SendSaveRequestFormData(strNavURL, strPostData, "----" + boundary);
                    if (strData != "")
                    {
                        htmlDoc.LoadHtml(strData); string _iQuoteNo = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtQut_NO_ThinkBaseTextBox']").GetAttributeValue("readonly", "").Trim();
                        if (_iQuoteNo.ToLower() == "readonly") result = true;
                        else result = false;
                    }
                    else
                    {
                        result = false;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                LogText = "Exception while processing offer " + ex.Message;
                return false;
            }
        }

        /*get xml files from quote upload path*/
        public void GetXmlFiles()
        {
            xmlFiles.Clear();
            DirectoryInfo _dir = new DirectoryInfo(strQuotePath);
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
                        strBuyerCode = _interchange.Recipient;

                    if (_interchange.Sender != null)
                        strSupplierCode = _interchange.Sender;

                    if (_interchange.DocumentHeader.DocType != null)
                        strDocType = _interchange.DocumentHeader.DocType;

                    if (_interchange.DocumentHeader != null)
                    {
                        if (_interchange.DocumentHeader.IsDeclined)
                            IsDecline = _interchange.DocumentHeader.IsDeclined;
                        else IsDecline = false;

                        if (_interchange.DocumentHeader.MessageNumber != null)
                            strMessageNumber = _interchange.DocumentHeader.MessageNumber;

                        if (_interchange.DocumentHeader.LeadTimeDays != null)
                            strLeadDays = _interchange.DocumentHeader.LeadTimeDays;

                        strCurrency = _interchange.DocumentHeader.CurrencyCode;

                        strMsgNumber = _interchange.DocumentHeader.MessageNumber;
                        strMsgRefNumber = _interchange.DocumentHeader.MessageReferenceNumber;

                        if (_interchange.DocumentHeader.IsAltItemAllowed != null) IsAltItemAllowed = Convert.ToInt32(_interchange.DocumentHeader.IsAltItemAllowed);
                        if (_interchange.DocumentHeader.IsPriceAveraged != null) IsPriceAveraged = Convert.ToInt32(_interchange.DocumentHeader.IsPriceAveraged);
                        if (_interchange.DocumentHeader.IsUOMChanged != null) IsUOMChanged = Convert.ToInt32(_interchange.DocumentHeader.IsUOMChanged);
                        if (_interchange.DocumentHeader.AdditionalDiscount != null) AdditionalDiscount = Convert.ToDouble(_interchange.DocumentHeader.AdditionalDiscount);


                        for (int i = 0; i < _interchange.DocumentHeader.References.Count; i++)
                        {
                            if (_interchange.DocumentHeader.References[i].Qualifier == ReferenceQualifier.UC)
                                strUCRefNo = _interchange.DocumentHeader.References[i].ReferenceNumber.Trim();
                            else if (_interchange.DocumentHeader.References[i].Qualifier == ReferenceQualifier.AAG)
                                strAAGRefNo = _interchange.DocumentHeader.References[i].ReferenceNumber.Trim();
                        }
                    }
                    if (_interchange.BuyerSuppInfo != null)
                    {
                        strLesRecordID = Convert.ToString(_interchange.BuyerSuppInfo.RecordID);
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
                                                strBuyerPhone = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.EM)
                                                strBuyerEmail = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.FX)
                                                strBuyerFax = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                        }
                                    }
                                }
                            }
                        }

                        else if (_interchange.DocumentHeader.PartyAddresses[j].Qualifier == PartyQualifier.VN)
                        {
                            strSupplierName = _interchange.DocumentHeader.PartyAddresses[j].Name;
                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts != null)
                            {
                                if (_interchange.DocumentHeader.PartyAddresses[j].Contacts.Count > 0)
                                {
                                    if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count > 0)
                                    {
                                        for (int k = 0; k < _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count; k++)
                                        {
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.TE)
                                                strSupplierPhone = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.EM)
                                                strSupplierEmail = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.FX)
                                                strSupplierFax = _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number;
                                        }
                                    }
                                }
                            }
                        }

                        else if (_interchange.DocumentHeader.PartyAddresses[j].Qualifier == PartyQualifier.UD)
                        {
                            strVesselName = _interchange.DocumentHeader.PartyAddresses[j].Name;
                            if (_interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Berth != "")
                                strPortName = _interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Berth;

                            if (_interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Port != null)
                                strPortCode = _interchange.DocumentHeader.PartyAddresses[j].PartyLocation.Port;
                        }
                    }

                    #endregion

                    #region read comments

                    if (_interchange.DocumentHeader.Comments != null)
                    {
                        for (int i = 0; i < _interchange.DocumentHeader.Comments.Count; i++)
                        {
                            if (_interchange.DocumentHeader.Comments[i].Qualifier == CommentTypes.SUR)
                                strSupplierComment = _interchange.DocumentHeader.Comments[i].Value;
                            else if (_interchange.DocumentHeader.Comments[i].Qualifier == CommentTypes.ZTP)
                                strPayTerms = _interchange.DocumentHeader.Comments[i].Value;
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
                                strPackingCost = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.FreightCharge_64)
                                strFreightCharge = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.TotalLineItemsAmount_79)
                                strTotalLineItemsAmount = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.AllowanceAmount_204)
                                strAllowance = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.GrandTotal_259)
                                strGrandTotal = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
                            else if (_interchange.DocumentHeader.MonetoryAmounts[i].Qualifier == MonetoryAmountQualifier.BuyerItemTotal_90)//added by kalpita on 18/11/2019
                                strBuyerTotal = _interchange.DocumentHeader.MonetoryAmounts[i].Value.ToString();
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
                                        strDelvDate = dtDelDate.ToString("MM/dd/yyyy");
                                    }
                                    if (dtDelDate == null)
                                    {
                                        DateTime dt = FormatMTMLDate(DateTime.Now.AddDays(Convert.ToDouble(strLeadDays)).ToString());
                                        if (dt != DateTime.MinValue)
                                        {
                                            strDelvDate = dt.ToString("MM/dd/yyyy");
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
                                        strExpDate = ExpDate.ToString("dd-MM-yyyy");
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

        #endregion

        public void CreateAuditFile(string strFileName, string strModule, string strRefNo, string strLogType, string strAudit)
        {
            try
            {
                if (!Directory.Exists(strAuditPath)) Directory.CreateDirectory(strAuditPath);

                string strAuditData = "";
                if (strAuditData.Trim().Length > 0) strAuditData += Environment.NewLine;
                strAuditData += strBuyer + "|";
                strAuditData += strSupplier + "|";
                strAuditData += strModule + "|";
                strAuditData += Path.GetFileName(strFileName) + "|";
                strAuditData += strRefNo + "|";
                strAuditData += strLogType + "|";
                strAuditData += DateTime.Now.ToString("yy-MM-dd HH:mm") + " : " + strAudit;
                if (strAuditData.Trim().Length > 0)
                {
                    File.WriteAllText(strAuditPath + "\\Audit_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt", strAuditData);
                    System.Threading.Thread.Sleep(5);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void MoveRFQToError(string strMessage, string strRFQNo, HtmlAgilityPack.HtmlDocument htmlDoc, string ErrorNo)
        {
            CreateAuditFile("", strProcessorName, strRFQNo, "Error", ErrorNo + strMessage);
            string strFile = strScreenShotPath + "\\Cido_RFQ" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".png";
            _httproutine.PrintScreen(htmlDoc.DocumentNode.OuterHtml, strFile);
        }

        public void MoveToError(string strMessage, string strRFQNo, HtmlAgilityPack.HtmlDocument htmlDoc, string MTMLFile,string ErrorNo)
        {
            string strFile = "";
            if (htmlDoc != null)
            {
                strFile = strScreenShotPath + "\\Cido_Quote" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".png";
                _httproutine.PrintScreen(htmlDoc.DocumentNode.OuterHtml, strFile);
            }
            if (File.Exists(Path.GetDirectoryName(MTMLFile) + "\\Error\\" + Path.GetFileName(MTMLFile)))
            { File.Delete(Path.GetDirectoryName(MTMLFile) + "\\Error\\" + Path.GetFileName(MTMLFile)); }
            File.Move(MTMLFile, Path.GetDirectoryName(MTMLFile) + "\\Error\\" + Path.GetFileName(MTMLFile));
            if (File.Exists(Path.GetDirectoryName(MTMLFile) + "\\Error\\" + Path.GetFileName(MTMLFile)))
            {
                if (htmlDoc != null)
                    CreateAuditFile(strFile, strProcessorName + "_Quote", strRFQNo, "Error", ErrorNo + strMessage);
                else
                    CreateAuditFile("", strProcessorName + "_Quote", strRFQNo, "Error", ErrorNo + strMessage);
            }
        }

        //public void MoveRFQToError(string strMessage, string strRFQNo, HtmlAgilityPack.HtmlDocument htmlDoc)
        //{
        //    CreateAuditFile("", strProcessorName, strRFQNo, "Error", strMessage);
        //    string strFile = strScreenShotPath + "\\Cido_RFQ" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".png";
        //    _httproutine.PrintScreen(htmlDoc.DocumentNode.OuterHtml, strFile);
        //}

        //public void MoveToError(string strMessage, string strRFQNo, HtmlAgilityPack.HtmlDocument htmlDoc,string MTMLFile)
        //{
        //    string strFile = "";
        //    if (htmlDoc != null)
        //    {
        //         strFile = strScreenShotPath + "\\Cido_Quote" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + strSupplier + ".png";
        //        _httproutine.PrintScreen(htmlDoc.DocumentNode.OuterHtml, strFile);
        //    }
        //    if (File.Exists(Path.GetDirectoryName(MTMLFile) + "\\Error\\" + Path.GetFileName(MTMLFile))) 
        //    { File.Delete(Path.GetDirectoryName(MTMLFile) + "\\Error\\" + Path.GetFileName(MTMLFile)); }
        //    File.Move(MTMLFile, Path.GetDirectoryName(MTMLFile) + "\\Error\\" + Path.GetFileName(MTMLFile));
        //    if (File.Exists(Path.GetDirectoryName(MTMLFile) + "\\Error\\" + Path.GetFileName(MTMLFile)))
        //    {
        //        if (htmlDoc != null)
        //            CreateAuditFile(strFile, strProcessorName + "_Quote", strRFQNo, "Error", strMessage);
        //        else
        //            CreateAuditFile("", strProcessorName + "_Quote", strRFQNo, "Error", strMessage);
        //    }
        //}

        public void MoveToBackup(string MTML_QuoteFile, string message)
        {
            if (File.Exists(Path.GetDirectoryName(MTML_QuoteFile) + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile)))
                File.Delete(Path.GetDirectoryName(MTML_QuoteFile) + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile));
            File.Move(MTML_QuoteFile, Path.GetDirectoryName(MTML_QuoteFile) + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile));

            if (File.Exists(Path.GetDirectoryName(MTML_QuoteFile) + "\\Backup\\" + Path.GetFileName(MTML_QuoteFile)))
                CreateAuditFile(MTML_QuoteFile, "Cido Shipping Quote", strUCRefNo, "Success", message);
            LogText = message;
        }
    }

    public enum eActions
    {
        RFQ = 0,
        PO = 1,
    }
}

#region commented
   //public bool SaveQuote(HtmlAgilityPack.HtmlDocument htmlDoc, string strNavURL, string MTML_QuoteFile)
   //     {
   //         bool result = false;
   //         try
   //         {
   //             string strCharges = Convert.ToString(Convert.ToDouble(strFreightCharge) + Convert.ToDouble(strPackingCost));
   //             string grandTotal = Convert.ToString(Convert.ToDouble(strTotalLineItemsAmount) + Convert.ToDouble(strCharges));
   //             if (Convert.ToInt32(grandTotal) == Convert.ToInt32(strGrandTotal))
   //             {
   //                 var boundary = "WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x", System.Globalization.NumberFormatInfo.InvariantInfo);// "WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x");//
   //                 Dictionary<string, string> dictState = _httproutine.GetStateInfo(htmlDoc,true);
   //                 if (dictState != null)
   //                 {
   //                     string _inqVessel = "", _inqIssueName = "", _frmDue = "", _toDue = "", _issueDate = "", _inqPortCode = "", _portName = "", _inqSub = "", _inqRemark = "", _issueName = "", _frmDueQuote = "", _toDueQuote = "", _issueDateQuote = "", _portCodeQuote = "", _portNameQuote = "", _attchQuoteFile = "", _value = "";

   //                     HtmlNode _hinqVessel = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtVesselInq_ThinkBaseTextBox']");
   //                     if (_hinqVessel != null) _inqVessel = _hinqVessel.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hinqIssueName = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssued_NameInq_ThinkBaseTextBox']");
   //                     if (_hinqIssueName != null) _inqIssueName = _hinqIssueName.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hfrmDue = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtFrTO_DUE_DATEInq_ThinkBaseTextBox']");
   //                     if (_hfrmDue != null) _frmDue = _hfrmDue.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _htoDue = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtToTO_DUE_DATEInq_ThinkBaseTextBox']");
   //                     if (_htoDue != null) _toDue = _htoDue.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hissueDate = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssued_DateInq_ThinkBaseTextBox']");
   //                     if (_hissueDate != null) _issueDate = _hissueDate.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hinqPortCode = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtDUE_PORTInq_ThinkBaseTextBox']");
   //                     if (_hinqPortCode != null) _inqPortCode = _hinqPortCode.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hportName = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtPortNameInq_ThinkBaseTextBox']");
   //                     if (_hportName != null) _portName = _hportName.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hinqSub = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtSubjectInq_ThinkBaseTextBox']");
   //                     if (_hinqSub != null) _inqSub = _hinqSub.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hinqRemark = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtRemarkInq_ThinkBaseTextBox']");
   //                     if (_hinqRemark != null) _inqRemark = _hinqRemark.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hissueName = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssued_NameQut_ThinkBaseTextBox']");
   //                     if (_hissueName != null) _issueName = _hissueName.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hfrmDueQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtFrTO_DUE_DATEQut_ThinkBaseTextBox']");
   //                     if (_hfrmDueQuote != null) _frmDueQuote = _hfrmDueQuote.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _htoDueQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtToTO_DUE_DATEQut_ThinkBaseTextBox']");
   //                     if (_htoDueQuote != null) _toDueQuote = _htoDueQuote.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hissueDateQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssued_DateQut_ThinkBaseTextBox']");
   //                     if (_hissueDateQuote != null) _issueDateQuote = _hissueDateQuote.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hportCodeQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtDUE_PORTQut_ThinkBaseTextBox']");
   //                     if (_hportCodeQuote != null) _portCodeQuote = _hportCodeQuote.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hportNameQuote = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtPortNameQut_ThinkBaseTextBox']");
   //                     if (_hportNameQuote != null) _portNameQuote = _hportNameQuote.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hattachQuoteFile = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='tftlstbxReportFile_SelectTextBox']");
   //                     if (_hattachQuoteFile != null) _attchQuoteFile = _hattachQuoteFile.GetAttributeValue("value", "").Trim();


   //                     HtmlNodeCollection _selectGeim = htmlDoc.DocumentNode.SelectNodes("//select[@id='tddlGeIm_ThinkBaseDropDownList']/option[@selected]");
   //                     if (_selectGeim != null)
   //                     {
   //                         _value = _selectGeim[0].GetAttributeValue("value", "").Trim();
   //                     }

   //                     string strPostData = @"------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"radScript_TSM\"" + Environment.NewLine + Environment.NewLine + ";;System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35:ko-KR:c97801cf-c4e9-421a-bd07-262d424faf76:ea597d4b:b25378d2" + Environment.NewLine + "------" +
   //                         boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__EVENTTARGET\"" + Environment.NewLine + Environment.NewLine + "timgbtnSave" + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__EVENTARGUMENT\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine +
   //                         "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__VIEWSTATE\"" + Environment.NewLine + Environment.NewLine + Uri.UnescapeDataString(dictState["__VIEWSTATE"]) + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__VIEWSTATEGENERATOR\"" + Environment.NewLine + Environment.NewLine + dictState["__VIEWSTATEGENERATOR"] +
   //                         Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__SCROLLPOSITIONX\"" + Environment.NewLine + Environment.NewLine + "0" + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"__SCROLLPOSITIONY\"" + Environment.NewLine + Environment.NewLine + "0" + Environment.NewLine + "------" + boundary +
   //                         Environment.NewLine + "Content-Disposition: form-data; name=\"__EVENTVALIDATION\"" + Environment.NewLine + Environment.NewLine + Uri.UnescapeDataString(dictState["__EVENTVALIDATION"]) + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtInq_NO$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strUCRefNo + Environment.NewLine + "------" + boundary + Environment.NewLine +
   //                         "Content-Disposition: form-data; name=\"ttxtVesselInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _inqVessel + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtIssued_NameInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _inqIssueName + Environment.NewLine + "------" + boundary + Environment.NewLine +
   //                     "Content-Disposition: form-data; name=\"ttxtFrTO_DUE_DATEInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _frmDue + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtToTO_DUE_DATEInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _toDue + Environment.NewLine + "------" + boundary + Environment.NewLine +
   //                     "Content-Disposition: form-data; name=\"ttxtIssued_DateInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _issueDate + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtDUE_PORTInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _inqPortCode + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtPortNameInq$ThinkBaseTextBox\"" +
   //                     Environment.NewLine + Environment.NewLine + _portName + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtSubjectInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _inqSub + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtRemarkInq$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _inqRemark + Environment.NewLine +
   //                     "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtQut_NO$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strAAGRefNo + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtVesselQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strVesselName + Environment.NewLine + "------" + boundary + Environment.NewLine +
   //                     "Content-Disposition: form-data; name=\"ttxtIssued_NameQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _issueName + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtFrTO_DUE_DATEQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _frmDueQuote + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtToTO_DUE_DATEQut$ThinkBaseTextBox\"" +
   //                     Environment.NewLine + Environment.NewLine + _toDueQuote + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtIssued_DateQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _issueDateQuote + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtDUE_PORTQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _portCodeQuote +
   //                     Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtPortNameQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + _portNameQuote + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tftlstbxReportFile$SelectTextBox\"" + Environment.NewLine + Environment.NewLine + _attchQuoteFile + Environment.NewLine + "------" + boundary + Environment.NewLine +
   //                     "Content-Disposition: form-data; name=\"tftlstbxReportFile$FileUpload\"; filename=\"\"" + Environment.NewLine + "Content-Type: application/octet-stream" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtValidperiod$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strExpDate + Environment.NewLine + "------" + boundary + Environment.NewLine +
   //                     "Content-Disposition: form-data; name=\"ttxtPaymentCond$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strPayTerms + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtSubjectQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + ConfigurationManager.AppSettings["QUOTESUBJECT"].Trim() + Environment.NewLine + "------" + boundary + Environment.NewLine +
   //                     "Content-Disposition: form-data; name=\"ttxtRemarkQut$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strSupplierComment.Trim() + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tddlQUT_CURR_CODE$ThinkBaseDropDownList\"" + Environment.NewLine + Environment.NewLine + strCurrency + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtQUT_AMT$ThinkBaseTextBox\"" +
   //                     Environment.NewLine + Environment.NewLine + strTotalLineItemsAmount + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtQUT_SHG$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + strCharges + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtQUT_TTL$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + grandTotal + Environment.NewLine +
   //                     "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtQUT_SHG_RMK$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + "Freight Cost: " + strFreightCharge + " Packing Cost: " + strPackingCost + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtDelivery$ThinkBaseTextBox\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tddlGeIm$ThinkBaseDropDownList\"" +
   //                     Environment.NewLine + Environment.NewLine + _value + Environment.NewLine;

   //                     string LeadDays = "", inq_item_rmk = "", qut_item_rmk = "", counter = "";
   //                     xmlData = "";
   //                          xmlData = "<?xml version=\"1.0\" encoding=\"euc-kr\"?><xml_data>";
   //                     #region Items
   //                     HtmlNodeCollection _itemsColl = htmlDoc.DocumentNode.SelectNodes("//table[@id='tmgridInqList']//tr[@part_key]");
   //                     if (_itemsColl != null)
   //                     {
   //                         if (_itemsColl.Count > 0)
   //                         {
   //                             if (_lineitem.Count > 0)
   //                             {
   //                                 if (_itemsColl.Count == _lineitem.Count)
   //                                 {
   //                                     foreach (HtmlNode _row in _itemsColl)
   //                                     {
   //                                         HtmlNodeCollection _td = _row.ChildNodes;
   //                                         LineItem _item = null;
   //                                         string partKey = _row.GetAttributeValue("part_key", "").Trim();
   //                                         foreach (LineItem item in _lineitem)
   //                                         {
   //                                             if (item.OriginatingSystemRef.ToString() == partKey.ToString()) { _item = item; }
   //                                         }
   //                                         if (_item != null)
   //                                         {
   //                                             string _price = "", _discount = "";
   //                                             foreach (PriceDetails _priceDetails in _item.PriceList)
   //                                             {
   //                                                 if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.GRP) _price = _priceDetails.Value.ToString("0.00");
   //                                                 else if (_priceDetails.TypeQualifier == PriceDetailsTypeQualifiers.DPR) _discount = _priceDetails.Value.ToString("0.00");
   //                                             }

   //                                             if (_item.DeleiveryTime != "0" || _item.DeleiveryTime != null)
   //                                                 LeadDays = _item.DeleiveryTime;
   //                                             else LeadDays = "";

   //                                             if (_td[14].ChildNodes.Count > 1)
   //                                             {
   //                                                 inq_item_rmk = _td[14].ChildNodes[1].GetAttributeValue("value", "").Trim();
   //                                                 string _counter = _td[14].ChildNodes[1].GetAttributeValue("name", "").Trim();
   //                                                 string[] arrCounter = _counter.Split('$');
   //                                                 counter = arrCounter[1];
   //                                             }
   //                                             if (_td[15].ChildNodes.Count > 1)
   //                                             {
   //                                                 qut_item_rmk = _td[15].ChildNodes[1].GetAttributeValue("value", "").Trim();
   //                                             }

   //                                             #region add xml data
   //                                             xmlData = xmlData + "<detail><PART_NO>";
   //                                             string _partNo = _td[2].InnerText.Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
   //                                             string _partDescr = _td[3].InnerText.Replace("&nbsp;", "");//.Replace("&amp;", "&").Trim();
   //                                             string _unit = _td[4].InnerText.Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
   //                                             string _qtyInq = _td[5].InnerText.Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
   //                                             string _qtyQut = Convert.ToString(_item.Quantity);// _td[6].InnerText.Replace("&nbsp;", "").Trim();
   //                                             string _contPrice = _td[7].InnerText.Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
   //                                             string _qutUnitPrice = Convert.ToString(_price);
   //                                             string _qutUnitSum = Convert.ToString(_item.MonetaryAmount);
   //                                             string _ohEdition = _td[10].InnerText.Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
   //                                             string _suppEdition = _td[11].InnerText.Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
   //                                             string _delivery = _item.DeleiveryTime + "Days";
   //                                             string _chkGeEq = htmlDoc.DocumentNode.SelectSingleNode("//select[@name='tmgridInqList$" + counter + "$CHK_GE_EQ']/option[@selected]").GetAttributeValue("value", "").Trim();
   //                                             string _eCode = _row.GetAttributeValue("E_CODE", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
   //                                             string _sCode = _row.GetAttributeValue("S_CODE", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
   //                                             string _partKey = _row.GetAttributeValue("PART_KEY", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
   //                                             string _sSort = _row.GetAttributeValue("S_SORT", "").Replace("&nbsp;", "").Trim();//.Replace("&amp;", "&")
   //                                             string _mCode = _row.GetAttributeValue("M_CODE", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
   //                                             string _kCode = _row.GetAttributeValue("K_CODE", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
   //                                             string _tCode = _row.GetAttributeValue("T_CODE", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
   //                                             string _partCode = _row.GetAttributeValue("PART_CODE", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
   //                                             string _equKind = _row.GetAttributeValue("EQU_KIND", "").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();

   //                                             xmlData = xmlData + _partNo + "</PART_NO><PART_DESC>" + _partDescr + "</PART_DESC><UNIT>" + _unit + "</UNIT><QTY_INQ>" + _qtyInq + "</QTY_INQ><QTY_QUT>" +
   //                                               _qtyQut + "</QTY_QUT><CONT_PRICE>" + _contPrice + "</CONT_PRICE><QUT_UNIT_PRICE>" + _qutUnitPrice + "</QUT_UNIT_PRICE><QUT_UNIT_SUM>" + _qutUnitSum +
   //                                               "</QUT_UNIT_SUM><OH_EDITION>" + _ohEdition + "</OH_EDITION><SUPPLY_EDITION>" + _suppEdition + "</SUPPLY_EDITION><DELIVERY_KIND>" + _delivery +
   //                                               "</DELIVERY_KIND><CHK_GE_EQ>" + _chkGeEq + "</CHK_GE_EQ><INQ_ITEM_RMK>" + inq_item_rmk + "</INQ_ITEM_RMK><QUT_ITEM_RMK>" + qut_item_rmk +
   //                                               "</QUT_ITEM_RMK><E_CODE>" + _eCode + "</E_CODE><S_CODE>" + _sCode + "</S_CODE><PART_KEY>" + _partKey + "</PART_KEY><S_SORT>" + _sSort + "</S_SORT><M_CODE>" +
   //                                               _mCode + "</M_CODE><K_CODE>" + _kCode + "</K_CODE><T_CODE>" + _tCode + "</T_CODE><PART_CODE>" + _partCode + "</PART_CODE><EQU_KIND>" + _equKind + "</EQU_KIND></detail>";
   //                                             #endregion

   //                                             strPostData = strPostData + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridInqList$" + counter + "$QTY_QUT\"" + Environment.NewLine + Environment.NewLine + _item.Quantity + Environment.NewLine +
   //                                                 "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridInqList$" + counter + "$QUT_UNIT_PRICE\"" + Environment.NewLine + Environment.NewLine + _price + Environment.NewLine + "------" + boundary + Environment.NewLine +
   //                                                 "Content-Disposition: form-data; name=\"tmgridInqList$" + counter + "$SUPPLY_EDITION\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridInqList$" + counter + "$DELIVERY_KIND\"" +
   //                                                 Environment.NewLine + Environment.NewLine + LeadDays + "Days" + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridInqList$" + counter + "$CHK_GE_EQ\"" + Environment.NewLine + Environment.NewLine + "GE" + Environment.NewLine +
   //                                                 "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridInqList$" + counter + "$INQ_ITEM_RMK\"" + Environment.NewLine + Environment.NewLine + inq_item_rmk + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"tmgridInqList$" + counter + "$QUT_ITEM_RMK\"" +
   //                                                 Environment.NewLine + Environment.NewLine + qut_item_rmk + Environment.NewLine;

   //                                         }
   //                                         // counter = counter + 2;
   //                                     }
   //                                     xmlData = xmlData + "</xml_data>";
   //                                 }
   //                                 else
   //                                 {
   //                                     //MoveToError("Unable to upload Quote due to item count mismatched.", strUCRefNo, htmlDoc, MTML_QuoteFile);
   //                                     MoveToError("Unable to save file due to item count mismatch.", strUCRefNo, htmlDoc, MTML_QuoteFile, "LeS-1008.4:");                                     
   //                                     result = false;
   //                                 }
   //                             }
   //                         }
   //                         //strRFQData = _httproutine.SendSaveRequestFormData(strNavURL, strPostData, "----" + boundary);
   //                         //htmlDoc.LoadHtml(strRFQData);
   //                     }
   //                     #endregion

   //                     string _reqSysNo = "", _inqSysNo = "", _qutSysNo = "", _inqSys = "", _vdrCode = "", _vdrName = "", _subject = "", _partKind = "", _vessel = "", _cus_odr_no = "",
   //                         _dept = "", _issuedNameEng = "", _odr_fm_email = "", _odr_to_email = "", _reportUrl = "";
   //                     HtmlNode _hreqSysNo = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtReqSysNo']");
   //                     if (_hreqSysNo != null) _reqSysNo = _hreqSysNo.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hinqSysNo = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtInqSysNo']");
   //                     if (_hinqSysNo != null) _inqSysNo = _hinqSysNo.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hqutSysNo = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtQutSysNo']");
   //                     if (_hqutSysNo != null) _qutSysNo = _hqutSysNo.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hinqSys = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtInqSys']");
   //                     if (_hinqSys != null) _inqSys = _hinqSys.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hvdrCode = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtVdrCode']");
   //                     if (_hvdrCode != null) _vdrCode = _hvdrCode.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hvdrName = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtVDR_NAME']");
   //                     if (_hvdrName != null) _vdrName = _hvdrName.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hsubject = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtSUBJECT']");
   //                     if (_hsubject != null) _subject = _hsubject.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hpartKind = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='hiddtxtPartKind']");
   //                     if (_hpartKind != null) _partKind = _hpartKind.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hvessel = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtVessel']");
   //                     if (_hvessel != null) _vessel = _hvessel.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hcus_odr_no = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtCUS_ODR_NO']");
   //                     if (_hcus_odr_no != null) _cus_odr_no = _hcus_odr_no.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hdept = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtDept']");
   //                     if (_hdept != null) _dept = _hdept.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hissuedNameEng = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtIssuedNameEng']");
   //                     if (_hissuedNameEng != null) _issuedNameEng = _hissuedNameEng.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hodr_fm_email = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtODR_FM_Email']");
   //                     if (_hodr_fm_email != null) _odr_fm_email = _hodr_fm_email.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hodr_to_email = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ttxtODR_TO_Email']");
   //                     if (_hodr_to_email != null) _odr_to_email = _hodr_to_email.GetAttributeValue("value", "").Trim();

   //                     HtmlNode _hreportUrl = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='ReportUrl']");
   //                     if (_hreportUrl != null) _reportUrl = _hreportUrl.GetAttributeValue("value", "").Trim();

   //                     strPostData = strPostData + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtReqSysNo\"" + Environment.NewLine + Environment.NewLine + _reqSysNo + Environment.NewLine +
   //                         "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtInqSysNo\"" + Environment.NewLine + Environment.NewLine + _inqSysNo + Environment.NewLine + "------" + boundary + Environment.NewLine +
   //                         "Content-Disposition: form-data; name=\"ttxtQutSysNo\"" + Environment.NewLine + Environment.NewLine + _qutSysNo + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtInqSys\"" +
   //                         Environment.NewLine + Environment.NewLine + _inqSys + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtVdrCode\"" + Environment.NewLine + Environment.NewLine +
   //                         _vdrCode + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtVDR_NAME\"" + Environment.NewLine + Environment.NewLine + _vdrName + Environment.NewLine +
   //                         "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtSUBJECT\"" + Environment.NewLine + Environment.NewLine + _subject + Environment.NewLine + "------" + boundary +
   //                         Environment.NewLine + "Content-Disposition: form-data; name=\"hiddtxtPartKind\"" + Environment.NewLine + Environment.NewLine + _partKind + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtVessel\"" +
   //                         Environment.NewLine + Environment.NewLine + _vessel + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtCUS_ODR_NO\"" + Environment.NewLine + Environment.NewLine +
   //                         _cus_odr_no + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtDept\"" + Environment.NewLine + Environment.NewLine + _dept + Environment.NewLine + "------" + boundary + Environment.NewLine +
   //                         "Content-Disposition: form-data; name=\"ttxtIssuedNameEng\"" + Environment.NewLine + Environment.NewLine + _issuedNameEng + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtODR_FM_Email\"" +
   //                         Environment.NewLine + Environment.NewLine + _odr_fm_email + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ttxtODR_TO_Email\"" + Environment.NewLine + Environment.NewLine + _odr_to_email +
   //                         "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"txthiddXmlData\"" + Environment.NewLine + Environment.NewLine + xmlData + Environment.NewLine + "------" + boundary + Environment.NewLine + "Content-Disposition: form-data; name=\"ReportUrl\"" +
   //                         Environment.NewLine + Environment.NewLine + System.Web.HttpUtility.HtmlDecode(_reportUrl) + Environment.NewLine + "------" + boundary + "--";

   //                     string strData = _httproutine.SendSaveRequestFormData(strNavURL, strPostData, "----" + boundary);
   //                     if (strData != "")
   //                     {
   //                         htmlDoc.LoadHtml(strData); HtmlNode _btnOffer = htmlDoc.DocumentNode.SelectSingleNode("//a[@id='timgbtnOffer']");
   //                         if (_btnOffer != null) result = true;
   //                         else result = false;
   //                     }
   //                     else
   //                     {
   //                         result = false;
   //                     }
   //                 }
   //             }
   //             else
   //             {
   //                 //MoveToError("Unable to upload Quote due to total amount mismatched.", strUCRefNo, null, MTML_QuoteFile);
   //                 MoveToError("Unable to save quote due to amount mismatch.", strUCRefNo, null, MTML_QuoteFile, "LeS-1008.1:");
   //             }
   //             return result;
   //         }
   //         catch (Exception ex)
   //         {
   //             LogText = "Exception while saving quote " + ex.Message;
   //             return false;
   //         }

   //     }



#endregion