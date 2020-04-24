using HtmlAgilityPack;
using HTTPRoutines;
using MTML.GENERATOR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;


namespace UnionMarine_Http_Routine
{
    public class Routines_old : LeSCommon.LeSCommon
    {
        public int iRetry = 0;
        public string currentDate = "", ProcessorName = "", sDoneFile = "", sAuditMesage = "", buyer_Link_code = "", eSupp_SuppAddCode = "";
        public string[] BuyerNames, Buyer_Supplier_LinkID, Actions;
        HTTP _httproutine = new HTTP();
        public MTMLInterchange _interchange { get; set; }

        public void GetAppConfigSettings()
        {
            try
            {
                Initialise();
                Userid = Convert.ToString(ConfigurationManager.AppSettings["USERNAME"]);
                Password = Convert.ToString(ConfigurationManager.AppSettings["PASSWORD"]);
                LoginRetry = convert.ToInt(ConfigurationManager.AppSettings["LOGINRETRY"]);
                SupplierCode = Convert.ToString(ConfigurationManager.AppSettings["SUPPLIER"]);
                SessionIDCookieName = "__RequestVerificationToken";
                URL = Convert.ToString(ConfigurationManager.AppSettings["SITE_URL"]);
                currentDate = Convert.ToString(ConfigurationManager.AppSettings["RFQRECEIVED_DATE"]);
                ProcessorName = Convert.ToString(ConfigurationManager.AppSettings["PROCESSOR_NAME"]);
                BuyerNames = Convert.ToString(ConfigurationManager.AppSettings["BUYER_NAME"]).Split('|');
                Buyer_Supplier_LinkID = Convert.ToString(ConfigurationManager.AppSettings["BUYER_SUPPLIER_LINK"]).Split('|');
                Actions = Convert.ToString(ConfigurationManager.AppSettings["ACTIONS"]).Split(',');
                eSupp_SuppAddCode = SupplierCode = Convert.ToString(ConfigurationManager.AppSettings["SUPPLIER"]);
                if (currentDate == null || currentDate == "")
                    currentDate = DateTime.Today.AddDays(-1).ToString("dd-MMM-yyyy");
                else
                    currentDate = DateTime.ParseExact(currentDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).AddDays(-1).ToString("dd-MMM-yyyy");

                AuditPath = (Convert.ToString(ConfigurationManager.AppSettings["AUDITPATH"]) != "") ? Convert.ToString(ConfigurationManager.AppSettings["AUDITPATH"]) : AppDomain.CurrentDomain.BaseDirectory + "Audit";
                LogPath = (Convert.ToString(ConfigurationManager.AppSettings["LOGPATH"]) != "") ? Convert.ToString(ConfigurationManager.AppSettings["LOGPATH"]) : AppDomain.CurrentDomain.BaseDirectory + "Log";
                PrintScreenPath = (Convert.ToString(ConfigurationManager.AppSettings["PRINT_PATH"]) != "") ? Convert.ToString(ConfigurationManager.AppSettings["PRINT_PATH"]) : AppDomain.CurrentDomain.BaseDirectory + "PrintScreen";
                if (Convert.ToString(ConfigurationManager.AppSettings["RFQ_PATH"]) != "") DownloadPath = Convert.ToString(ConfigurationManager.AppSettings["RFQ_PATH"]);
                if (!Directory.Exists(PrintScreenPath)) Directory.CreateDirectory(PrintScreenPath);
                if (!Directory.Exists(DownloadPath)) Directory.CreateDirectory(DownloadPath);
                if (!Directory.Exists(AuditPath)) Directory.CreateDirectory(AuditPath);
                if (!Directory.Exists(LogPath)) Directory.CreateDirectory(LogPath);
                HiddenAttributeKey = "name";
            }
            catch (Exception e)
            {
                sAuditMesage = "Exception during initialise: " + e.GetBaseException().ToString();
                LogText = sAuditMesage;
                CreateAuditFile("", "UnionMarine_Http_Processor", "", "Error", sAuditMesage, buyer_Link_code, SupplierCode, AuditPath);
            }
        }

        public void Download_FileProcess()
        {
            GetAppConfigSettings();
            if (Login())
            {
                LogText = "Union Marine has Login Successfully.";
                GetFilterRecords(_CurrentResponseString);
                DownloadFile(_CurrentResponseString);
            }
        }

        public bool Login()
        {
            bool result = false;
            try
            {
                _httpWrapper.Referrer = URL + "users/login";
                _httpWrapper._SetRequestHeaders[HttpRequestHeader.Cookie] = @"CAKEPHP = ea14b09169102e146280262c02ffe795";
                Post_Data = @"data%5BUser%5D%5BUsername%5D=" + Userid + "&data%5BUser%5D%5BPassword%5D=" + Password + "&Login=Login";
                result = PostURL("", "", "");
                string sid = _httpWrapper._CurrentResponse.Cookies["SESSIONID"].ToString();
                string cookies = _httpWrapper.CooKieName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public void GetFilterRecords(string _CurrentResponseString)
        {
            List<string> slHeader = new List<string>(); slHeader.Clear();
            Dictionary<int, string> slRecords = new Dictionary<int, string>(); slRecords.Clear();

            string cType = "", cNumber = "", cInvStatus = "", cPOStatus = "", cPOStatus1 = "", cKeyword = "", cStartDate = "", cEndDate = "";

            //  HtmlNodeCollection _collect = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//div[@class='table table-bordered table-hover']");

            // header row and records
            HtmlNodeCollection _collectRFQHeadr = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='fixedhead1']/thead/tr/th");//[0].ChildNodes;
            if (_collectRFQHeadr != null && _collectRFQHeadr.Count > 0)
            {
                for (int i = 0; i < _collectRFQHeadr.Count; i++)
                {
                    slHeader.Add(_collectRFQHeadr[i].InnerText);
                }
            }
            HtmlNodeCollection _collectRFQData = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='fixedhead1']/tbody/tr");
            if (_collectRFQData != null && _collectRFQData.Count > 0)
            {
                for (int i = 0; i < _collectRFQData.Count; i++)
                {
                    HtmlNodeCollection _arr = _collectRFQData[i].ChildNodes;
                    slRecords.Add(i, GetNodedetails(_arr));
                }
            }

           

            List<string> slanchor = ExtractAllAHrefTags(_httpWrapper._CurrentDocument);

            #region commented
            // HtmlNodeCollection _collectRFQData = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='fixedhead1']/tbody/tr");

            //  HtmlNodeCollection _collectPO_Inv = _httpWrapper._CurrentDocument.DocumentNode.SelectNodes("//table[@id='fixedhead2']/tbody/tr");

            //HtmlNode eleType = _httpWrapper._CurrentDocument.GetElementbyId("SearchSearchFor");
            //if (eleType != null) { cType = HttpUtility.UrlEncode(Convert.ToString(eleType.GetAttributeValue("value", true))); }
            //HtmlNode eleNumber = _httpWrapper._CurrentDocument.GetElementbyId("SearchNumber");
            //if (eleNumber != null) { cNumber = HttpUtility.UrlEncode(Convert.ToString(eleNumber.GetAttributeValue("value", true))); }
            //HtmlNode eleInvoiceStatus = _httpWrapper._CurrentDocument.GetElementbyId("SearchInvoiceStatus");
            //if (eleInvoiceStatus != null) { cInvStatus = HttpUtility.UrlEncode(Convert.ToString(eleInvoiceStatus.GetAttributeValue("value", true))); }
            //HtmlNode elePOStatus = _httpWrapper._CurrentDocument.GetElementbyId("SearchPoStatus");
            //if (elePOStatus != null) { cPOStatus = HttpUtility.UrlEncode(Convert.ToString(elePOStatus.GetAttributeValue("value", true))); }
            //HtmlNode elePOStatus1 = _httpWrapper._CurrentDocument.GetElementbyId("SearchPoStatus1");
            //if (elePOStatus != null) { cPOStatus1 = HttpUtility.UrlEncode(Convert.ToString(elePOStatus1.GetAttributeValue("value", true))); }
            //HtmlNode eleStartDate = _httpWrapper._CurrentDocument.GetElementbyId("SearchDateStart");
            //if (eleStartDate != null) { cStartDate = HttpUtility.UrlEncode(Convert.ToString(eleStartDate.GetAttributeValue("value", true))); }
            //HtmlNode eleEndDate = _httpWrapper._CurrentDocument.GetElementbyId("SearchDateEnd");
            //if (eleEndDate != null) { cEndDate = HttpUtility.UrlEncode(Convert.ToString(eleEndDate.GetAttributeValue("value", true))); }
            //HtmlNode eleKeyWord = _httpWrapper._CurrentDocument.GetElementbyId("SearchKeyword");
            //if (eleKeyWord != null) { cKeyword = HttpUtility.UrlEncode(Convert.ToString(eleKeyWord.GetAttributeValue("value", true))); }

            //Post_Data = @"data%5BSearch%5D%5BSearchFor%5D=" + cType + "&data%5BSearch%5D%5BNumber%5D=" + cNumber +
            //"&data%5BSearch%5D%5BInvoiceStatus%5D=" + cInvStatus + "&data%5BSearch%5D%5BPoStatus%5D=" + cPOStatus + "&data%5BSearch%5D%5BPoStatus1%5D=" + cPOStatus1 +
            //"&data%5BSearch%5D%5BDateStart%5D=" + cStartDate + "&data%5BSearch%5D%5BDateEnd%5D=" + cEndDate + "&data%5BSearch%5D%5BKeyword%5D=" + cKeyword;
            //bool result = PostURL("", "", "");
            //string strval = _CurrentResponseString;

            //string DefaultValDate = convert.ToString(eleQuoteExpDate.InnerText);
            //DateTime.TryParse(DefaultValDate.Trim(), System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtExpiry);
            //if (dtExpiry != DateTime.MinValue && dtExpiry < DateTime.Now) dtExpiry = DateTime.Now;
            //_quoteExp = dtExpiry.Month + "/" + (dtExpiry.Day + 1) + "/" + dtExpiry.Year;
            #endregion

        }

        public bool DownloadFile(string _CurrentResponseString)
        {
            HttpWebResponse response;

            bool _result = false;
            try
            {
                _httpWrapper.Referrer = URL + "invoice";

                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.intuitships-supplynet.ummsportal.com/requisition_quote/pdf/1092");

                //request.KeepAlive = true;
                //request.Headers.Add("Upgrade-Insecure-Requests", @"1");
                //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
                //request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                //request.Referer = "http://www.intuitships-supplynet.ummsportal.com/invoice";
                //request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                //request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-GB,en-US;q=0.9,en;q=0.8,und;q=0.7");
                //request.Headers.Set(HttpRequestHeader.Cookie, @"CAKEPHP=ea14b09169102e146280262c02ffe795");

                //response = (HttpWebResponse)request.GetResponse();
                //string value = _CurrentResponseString;

            }
            catch (Exception e)
            {
                sAuditMesage = "Exception in DownloadFile : " + e.GetBaseException().ToString();
                LogText = sAuditMesage;
                string filename = PrintScreenPath + "\\Union Marine" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
                if (!PrintScreen(filename)) filename = "";
                // CreateAuditFile(filename, "Union Marine Processor", "", "Error", sAuditMesage, buyer_Link_code, SupplierCode, AuditPath);
            }
            return _result;
        }

        public override bool DownloadRFQ(string RequestURL, string DownloadFileName, string ContentType = "Application/pdf")
        {
            bool _result = false;
            try
            {
                URL = RequestURL;
                if (LoadURL("", "", "", false))
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

        private List<string> ExtractAllAHrefTags(HtmlDocument htmldoc)
        {
            List<string> hrefTags = new List<string>();
            string[] urls = htmldoc.DocumentNode.Descendants("a").Select(node => node.Attributes["href"].Value).ToArray();
            foreach (string url in urls)
            {
                Console.WriteLine(url);
            }
            return hrefTags;
        }


        private string PrintScreen(string sHTML, string sFileName)
        {
            if (!Directory.Exists(Path.GetDirectoryName(sFileName) + "\\Temp")) Directory.CreateDirectory(Path.GetDirectoryName(sFileName) + "\\Temp");
            sFileName = Path.GetDirectoryName(sFileName) + "\\Temp\\" + Path.GetFileName(sFileName);
            _httproutine.PrintScreen(sHTML, sFileName);
            if (File.Exists(sFileName))
            {
                MoveFiles(sFileName, PrintScreenPath + "\\" + Path.GetFileName(sFileName));
                if (File.Exists(PrintScreenPath + "\\" + Path.GetFileName(sFileName))) return PrintScreenPath + "\\" + Path.GetFileName(sFileName);
                else return sFileName;
            }
            else return "";

        }

        private string GetNodedetails(HtmlNodeCollection _arrNodes)
        {
            string _str = "";
            if (_arrNodes != null && _arrNodes.Count > 0)
            {
                for (int i = 0; i < _arrNodes.Count; i++)
                {
                    if (_arrNodes[i].Name != "#text")
                    {
                        _str += _arrNodes[i].InnerText + "|";
                    }
                }
            }
            return _str.TrimEnd('|');
        }

    }
}
#region commented
//public bool DoLogin(string validateNodeType, string validateAttribute, string attributeValue, bool bload = true)
//{
//    bool isLoggedin = false;
//    try
//    {
//        dctPostDataValues.Clear();
//        dctPostDataValues.Add("__RequestVerificationToken", "");
//        dctPostDataValues.Add("UserId", HttpUtility.UrlEncode(Userid));
//        dctPostDataValues.Add("Password", Password);
//        if (iRetry == 0) LoadURL("", "", "");
//        URL = "http://www.intuitships-supplynet.ummsportal.com/users/login";
//        isLoggedin = base.DoLogin("div", "id", "supplierName", false);
//        if (isLoggedin)
//        {
//            LogText = "Logged in successfully";
//        }
//        if (!isLoggedin)
//        {
//            if (iRetry == LoginRetry)
//            {
//                string filename = PrintScreenPath + "\\UnionMarine_LoginFailed_" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".png";
//                // if (!PrintScreen(filename)) filename = "";
//                LogText = "Login failed";
//                // CreateAuditFile(filename, "UnionMarine_Http_Processor", "", "Error", "Unable to login.", buyer_Link_code, SupplierCode, AuditPath);
//            }
//            else
//            {
//                iRetry++;
//                LogText = "Login retry";
//                isLoggedin = DoLogin(validateNodeType, validateAttribute, attributeValue, false);
//            }
//        }

//    }
//    catch (Exception e)
//    {
//        LogText = "Exception while login : " + e.GetBaseException().ToString();
//        if (iRetry > LoginRetry)
//        {
//            LogText = "Login failed";
//            //CreateAuditFile("", "UnionMarine_Http_Processor", "", "Error", "Unable to login. Error : " + e.Message, BuyerCode, SupplierCode, AuditPath);
//        }
//        else
//        {
//            iRetry++;
//            LogText = "Login retry";
//         //   isLoggedin = DoLogin(validateNodeType, validateAttribute, attributeValue);
//        }
//    }
//    return isLoggedin;
//}
#endregion
