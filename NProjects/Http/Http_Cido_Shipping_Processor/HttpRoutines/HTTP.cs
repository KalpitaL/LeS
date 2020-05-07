using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using asPDF = Aspose.Pdf;
using Aspose.Pdf;
using System.Text.RegularExpressions;
using System.Web;

namespace HttpRoutines
{
    public class HTTP
    {
        #region Properties

        public CookieContainer _CookieContainer { get; set; }
        public HttpWebResponse _CurrentResponse { get; set; }
        public string strSessionID { get; set; }
        public string strFileName { get; set; }
        public string strContentType { get; set; }

        #endregion

        public string SendRequest(string strRequestURL)
        {
            bool isSession = false;
            CookieContainer cookieContainer = null;
            if (this._CookieContainer == null)
            {

                cookieContainer = new CookieContainer();
                isSession = true;
            }
            else cookieContainer = this._CookieContainer;
            string strData = "";
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strRequestURL);
                request.CookieContainer = cookieContainer;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
                request.Headers.Add("Upgrade-Insecure-Requests", @"1");
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");
                if (_CurrentResponse != null) _CurrentResponse.Close();
                _CurrentResponse = (HttpWebResponse)request.GetResponse();
                strData = ReadResponse(_CurrentResponse);
                if (isSession)
                {
                    _CookieContainer = cookieContainer;
                    SetSessionID(strRequestURL);
                }
                return strData;
            }
            catch (Exception e) { throw e; }
        }

        public string ReadResponse(HttpWebResponse response)
        {
            try
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    Stream streamToRead = responseStream;
                    if (response.ContentEncoding.ToLower().Contains("gzip"))
                    {
                        streamToRead = new GZipStream(streamToRead, CompressionMode.Decompress);
                    }
                    else if (response.ContentEncoding.ToLower().Contains("deflate"))
                    {
                        streamToRead = new DeflateStream(streamToRead, CompressionMode.Decompress);
                    }

                    using (StreamReader streamReader = new StreamReader(streamToRead, Encoding.UTF8))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            { throw e; }
        }

        public void SetSessionID(string strRequest)
        {
            try
            {
                foreach (System.Net.Cookie cookie in _CookieContainer.GetCookies(new Uri(strRequest)))
                {
                    if (cookie.Name == "ASP.NET_SessionId")
                    {
                        strSessionID = cookie.Value;
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        public string SendRequestFormData(string strRequest, string strPostData)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strRequest);

                request.KeepAlive = true;
                request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0");
                request.Headers.Add("Origin", @"http://client.cidoship.com");
                request.Headers.Add("Upgrade-Insecure-Requests", @"1");
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                request.Referer = strRequest;
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");
                request.Headers.Set(HttpRequestHeader.Cookie, @"ASP.NET_SessionId=" + strSessionID);

                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;
                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(strPostData);
                request.ContentLength = postBytes.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
                stream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string strData = ReadResponse(response);
                return strData;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string SendSaveRequestFormData(string strRequest, string strPostData, string boundary)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strRequest);

                request.KeepAlive = true;
                request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0");
                request.Headers.Add("Origin", @"http://client.cidoship.com");
                request.Headers.Add("Upgrade-Insecure-Requests", @"1");
             //   request.ContentType = "";
                request.ContentType = "multipart/form-data; boundary=" + boundary;//
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                 request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.Referer = strRequest;
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");
                request.Headers.Set(HttpRequestHeader.Cookie, @"ASP.NET_SessionId=" + strSessionID);

                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;
                WriteMultipartBodyToRequest(request, strPostData);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string strData = ReadResponse(response);
                return strData;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        //public bool Request_client_cidoship_com(string strPostData, string NavURL)
        //{
        //    try
        //    {
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(NavURL);

        //        request.KeepAlive = true;
        //        request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0");
        //        request.Headers.Add("Origin", @"http://client.cidoship.com");
        //        request.Headers.Add("Upgrade-Insecure-Requests", @"1");
        //        request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
        //        request.ContentType = "multipart/form-data; boundary=----WebKitFormBoundary07E9B27RYR3hXEiZ";
        //        request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        //        request.Referer = "http://client.cidoship.com/SHORE/SUP/Purchase/frmPurVenQutMgt.aspx?SysReqNo=FOSU-SUP-PUR-180302-0007&SysInqNo=01&VdrCode=1002176&Vcode=0156&POPUP=Y&MENUSEQ=1009";
        //        request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
        //        request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-us");
        //        request.Headers.Set(HttpRequestHeader.Cookie, @"ASP.NET_SessionId=" + strSessionID);

        //        request.Method = "POST";
        //        request.ServicePoint.Expect100Continue = false;

        //        string body = @"------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"radscript_tsm\"\"" + Environment.NewLine + Environment.NewLine +
        //           ";;System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35:ko-KR:c97801cf-c4e9-421a-bd07-262d424faf76:ea597d4b:b25378d2" + Environment.NewLine +
        //           "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"__eventtarget\"\"" + Environment.NewLine + Environment.NewLine +
        //            "timgbtnSave" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"__eventargument\"\"" +
        //            Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"__viewstate\"\"" +
        //            Environment.NewLine + Environment.NewLine + "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"+
        //            Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"__viewstategenerator\"\"" + Environment.NewLine + Environment.NewLine +
        //            "8C700690" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"__scrollpositionx\"\"" +
        //            Environment.NewLine + Environment.NewLine + "0" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"__scrollpositiony\"\"" +
        //            Environment.NewLine + Environment.NewLine + "0" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"__eventvalidation\"\"" +
        //            Environment.NewLine + Environment.NewLine + "/0K7L3aFl7BFq1PPbAYfHUvokCqMjwN4wjJMl92ijWs1pv82Jq++DoJE85wB7rB+QaARuDpd9WzPsVL8EL/EK+ciFVWw2xRvQtFQxT7mo4bCwpMr6OjPi9sioUY/7VWhvlo3GACnDfB209CRZPcZMv09kq8cf0e9GJ34Cw9IFRNj3T+ROUk3/TBDzcKrX0oT56cy0w0z5uBT+T9SYodoeuk7uY90DPcbmTetP0o1PH1QDRuX6Iw9hk2Ad4zn2Cm6H5+BYCVF74keYONpCtqBHCYNIh/1/l8VRkTqM60sCQ+kg32OlPk/rqhbmdkTrQP2VMbwk+LwhwbF+RhAzYilzhfZsLNiSM8mF3Hn4sSKrJPVL0AOdv0actOKtSX6VbfK74jweTemHHDrjReeua0X3EqQenLB6OIYe1ylVq8n0NV3unzqn/G4aav8oycDI4VSuDrWe11ebubKlaTcEalby3tBy9RVcSBLUvRsj/3t7KxDVEfF0tuksz8C2FUUV/ESXvFGd5sAxzeErAIgqYVWDTnt0UO/L78gIUOcSa24KsuUlpJAyNrVDc2uMbNVwhhlLOkxl9W7VXdL2GSh+bOEAPMFfylJ90l+FvsuxYblRxxEo9/N3suC4CktulUiHFqaULufsczGRfdt5syt/O3nZ4ASIHZnMXZZVC5x14IFG8dzkZZUwqTgG5VksZMaq0QQAUEbIJMTxruC1w01GVIv2tlbenTfupIGZeTpIOWuLqyWswAfBKXWiilj8buMmFIX2uuBQ3Qjg+N5V7Jy7ATUfoWpCpYMZl065goXSUqIOHMo/W6vcRqxhhtHbwPa/nv5TZd69arzbre7uP4poeTVSULnDfLYtVJdxZ7Y0pfRH1/1iIPVSY5W49v2YLIJcYjtBBlG8boRJFZduRCZ3a7HUdbtNiVxn9NIOaP0LW0n2i/OU5hDpoz9z+4emBmxuKXiq/YcZwr8fWwOtdbfa5hTTTOX1Oq9tjB1NBlgeoNAgeeVBEpRyxVThPwaWgzSWBhUv2lQ5YEIjyMl1jx0W4AM4+FtkFGJsI+shca66AmbOFsKT0sgomW9XdkkQSWrm2IsQbU/asgpsgD6q3I3dJ+T6oLT5bwDmGDG4NFVJ/+x6yg7VWvresoghK70XTInK0i6zlNoE05lzoW5KQBEjYyKPVMHZ/tpY8Ra08ngxLKkOJVznWykS7eZbaqsm6DQ0yvRcmKI2C1hxljOiIpVY1Z0tVkB3IfdaELOgdz3L3XmifqfuBT9B3mEdQGOIJR0cGQfpmdja7WPzCYsFa0fmZn1AY0WDwvWQVroxQ/va50cokcav6QvTxSGshLRjrTA9oXf9V2xMDP0dPRGrmVbAu6aCWo8izv5FaX5bjn6XunJEcdlNhkCcAJLyzeKlvPajurXXLdND1CCsbGRkGnhjvugPX2V81yxzCE2CTiiEz7OG0REUNA6vHykzXT4Y49ImTglR30BgQ==" +
        //            Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtinq_no$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "GRDO-1802-0008-01" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtvesselinq$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "GRAND DOLPHIN" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtissued_nameinq$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "김현호" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtfrto_due_dateinq$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "2018-02-08" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxttoto_due_dateinq$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "2018-02-28" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtissued_dateinq$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "2018-02-20" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtdue_portinq$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "KRPUS" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtportnameinq$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "Busan" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtsubjectinq$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "ELEVATOR" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtremarkinq$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtqut_no$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "E6256686" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtvesselqut$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "GRAND DOLPHIN" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtissued_namequt$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "Daisuke Nishikawa" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtfrto_due_datequt$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "2018-02-08" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxttoto_due_datequt$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "2018-02-28" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtissued_datequt$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "2018-03-26" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtdue_portqut$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "KRPUS" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtportnamequt$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "Busan" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"tftlstbxreportfile$selecttextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"tftlstbxreportfile$fileupload\"\"; filename=\"\"\"\"" +
        //            Environment.NewLine + "Content-Type: application/octet-stream" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtvalidperiod$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "29-03-2018" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtpaymentcond$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "60 DAYS AFTER SHIPMENT" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtsubjectqut$thinkbasetextbox\"\"" +
        //            Environment.NewLine + Environment.NewLine + "QUOTATION" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtremarkqut$thinkbasetextbox\"\"" + Environment.NewLine +
        //            Environment.NewLine + "***\"\" WE HEREBY DECLARE THAT ALL OF OUR SUPPLYING PRODUCTS ARE ASBESTOS-FREE.\"\" *** GENUINE PARTS FROM ORIGINAL MANUFACTURE/MAKER ***" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" +
        //            Environment.NewLine + "Content-Disposition: form-data; name=\"\"tddlqut_curr_code$thinkbasedropdownlist\"\"" + Environment.NewLine + Environment.NewLine + "JPY" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" +
        //            Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtqut_amt$thinkbasetextbox\"\"" + Environment.NewLine + Environment.NewLine + "3800" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine +
        //            "Content-Disposition: form-data; name=\"\"ttxtqut_shg$thinkbasetextbox\"\"" + Environment.NewLine + Environment.NewLine + "0" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine +
        //            "Content-Disposition: form-data; name=\"\"ttxtqut_ttl$thinkbasetextbox\"\"" + Environment.NewLine + Environment.NewLine + "3800" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine +
        //            "Content-Disposition: form-data; name=\"\"ttxtqut_shg_rmk$thinkbasetextbox\"\"" + Environment.NewLine + Environment.NewLine + "Freight Cost: 0 Packing Cost: 0" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" +
        //            Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtdelivery$thinkbasetextbox\"\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" +
        //            Environment.NewLine + "Content-Disposition: form-data; name=\"\"tddlgeim$thinkbasedropdownlist\"\"" + Environment.NewLine + Environment.NewLine + "000" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine +
        //            "Content-Disposition: form-data; name=\"\"tmgridinqlist$ctl02$qty_qut\"\"" + Environment.NewLine + Environment.NewLine + "1" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine +
        //            "Content-Disposition: form-data; name=\"\"tmgridinqlist$ctl02$qut_unit_price\"\"" + Environment.NewLine + Environment.NewLine + "3,800.00" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" +
        //            Environment.NewLine + "Content-Disposition: form-data; name=\"\"tmgridinqlist$ctl02$supply_edition\"\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" +
        //            Environment.NewLine + "Content-Disposition: form-data; name=\"\"tmgridinqlist$ctl02$delivery_kind\"\"" + Environment.NewLine + Environment.NewLine + "7Days" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" +
        //            Environment.NewLine + "Content-Disposition: form-data; name=\"\"tmgridinqlist$ctl02$chk_ge_eq\"\"" + Environment.NewLine + Environment.NewLine + "GE" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" +
        //            Environment.NewLine + "Content-Disposition: form-data; name=\"\"tmgridinqlist$ctl02$inq_item_rmk\"\"" + Environment.NewLine + Environment.NewLine + "FOR READY SPARE / 2018년 03월 한국 기항시점에서 TECHNICAIN -정기 점검 대비하여 SPARE PARTS (LIST)재고 조사하여 재고 없는 ITEM입니다." +
        //            Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"tmgridinqlist$ctl02$qut_item_rmk\"\"" + Environment.NewLine + Environment.NewLine +
        //            "" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtreqsysno\"\"" + Environment.NewLine + Environment.NewLine + "GRDO-SUP-PUR-180208-0001" +
        //            Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtinqsysno\"\"" + Environment.NewLine + Environment.NewLine + "01" + Environment.NewLine +
        //            "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtqutsysno\"\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" +
        //            Environment.NewLine + "Content-Disposition: form-data; name=\"\"ttxtinqsys\"\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine +
        //            "Content-Disposition: form-data; name=\"\"ttxtvdrcode\"\"" + Environment.NewLine + Environment.NewLine + "1002176" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine +
        //             "Content-Disposition: form-data; name=\"\"ttxtvdr_name\"\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine +
        //            "Content-Disposition: form-data; name=\"\"ttxtsubject\"\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine +
        //            "Content-Disposition: form-data; name=\"\"hiddtxtpartkind\"\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine +
        //            "Content-Disposition: form-data; name=\"\"ttxtvessel\"\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine +
        //             "Content-Disposition: form-data; name=\"\"ttxtcus_odr_no\"\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine +
        //            "Content-Disposition: form-data; name=\"\"ttxtdept\"\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine +
        //            "Content-Disposition: form-data; name=\"\"ttxtissuednameeng\"\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine +
        //            "Content-Disposition: form-data; name=\"\"ttxtodr_fm_email\"\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine +
        //            "Content-Disposition: form-data; name=\"\"ttxtodr_to_email\"\"" + Environment.NewLine + Environment.NewLine + "" + Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine +
        //            "Content-Disposition: form-data; name=\"\"txthiddxmldata\"\"" + Environment.NewLine + Environment.NewLine + "<?xml version=\"1.0\" encoding=\"euc-kr\"?><xml_data><detail><PART_NO>1</PART_NO><PART_DESC>RELAY(MY4N,AC100V)</PART_DESC><UNIT>PC</UNIT><QTY_INQ>1.00</QTY_INQ><QTY_QUT>1</QTY_QUT><CONT_PRICE></CONT_PRICE><QUT_UNIT_PRICE>3800.00</QUT_UNIT_PRICE><QUT_UNIT_SUM>3800</QUT_UNIT_SUM><OH_EDITION></OH_EDITION><SUPPLY_EDITION></SUPPLY_EDITION><DELIVERY_KIND>7Days</DELIVERY_KIND><CHK_GE_EQ>GE</CHK_GE_EQ><INQ_ITEM_RMK>FOR READY SPARE / 2018년 03월 한국 기항시점에서 TECHNICAIN -정기 점검 대비하여 SPARE PARTS (LIST)재고 조사하여 재고 없는 ITEM입니다.</INQ_ITEM_RMK><QUT_ITEM_RMK></QUT_ITEM_RMK><E_CODE>E0168</E_CODE><S_CODE>S3330</S_CODE><PART_KEY>0026-E0168-S3330-P0017-01</PART_KEY><S_SORT>ELEVATOR (USHIO REINETSU CO., LTD. / (U) ELEVATOR 350KG X 45M/MIN) / ELECTRIC PART</S_SORT><M_CODE>M051</M_CODE><K_CODE>K0049</K_CODE><T_CODE>T0480</T_CODE><PART_CODE>P0017</PART_CODE><EQU_KIND>01</EQU_KIND></detail></xml_data>" +
        //            Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ" + Environment.NewLine + "Content-Disposition: form-data; name=\"\"reporturl\"\"" + Environment.NewLine + Environment.NewLine + "./frmPurApprovalReport.aspx?DocKind=VENINQ&SysReqNo=GRDO-SUP-PUR-180208-0001&SysInqNo=01" +
        //            Environment.NewLine + "------WebKitFormBoundary07E9B27RYR3hXEiZ--";

        //        WriteMultipartBodyToRequest(request, body);

        //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //        string strData = ReadResponse(response);
        //    }
        //    catch (WebException e)
        //    {
        //        throw e;
        //    }
        //    catch (Exception)
        //    {

        //    }

        //    return true;
        //}

//        private bool Request_client_cidoship_com(out HttpWebResponse response)
//        {
//            response = null;

//            try
//            {
//                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://client.cidoship.com/SHORE/SUP/Purchase/frmPurVenQutMgt.aspx?SysReqNo=GRDO-SUP-PUR-180208-0001&SysInqNo=01&VdrCode=1002176&Vcode=0026&POPUP=Y&MENUSEQ=1009");

//                request.KeepAlive = true;
//                request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0");
//                request.Headers.Add("Origin", @"http://client.cidoship.com");
//                request.Headers.Add("Upgrade-Insecure-Requests", @"1");
//                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
//                request.ContentType = "multipart/form-data; boundary=----WebKitFormBoundaryH6QZcbJi74mbiSgF";
//                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
//                request.Referer = "http://client.cidoship.com/SHORE/SUP/Purchase/frmPurVenQutMgt.aspx?SysReqNo=GRDO-SUP-PUR-180208-0001&SysInqNo=01&VdrCode=1002176&Vcode=0026&POPUP=Y&MENUSEQ=1009";
//                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
//                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-us");
//                request.Headers.Set(HttpRequestHeader.Cookie, @"ASP.NET_SessionId=0jcjrli5ado231c3tvol3inb");

//                request.Method = "POST";
//                request.ServicePoint.Expect100Continue = false;

//                string body = @"------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+"Content-Disposition: form-data; name=\"\"radscript_tsm\"\""+
//                    Environment.NewLine+Environment.NewLine+";;System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35:ko-KR:c97801cf-c4e9-421a-bd07-262d424faf76:ea597d4b:b25378d2"+
//                    Environment.NewLine+"------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+"Content-Disposition: form-data; name=\"\"__eventtarget\"\""+
//                    Environment.NewLine+Environment.NewLine+"timgbtnSave"+Environment.NewLine+"------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+
//                    "Content-Disposition: form-data; name=\"\"__eventargument\"\""+Environment.NewLine+Environment.NewLine+""+Environment.NewLine+
//                    "------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+"Content-Disposition: form-data; name=\"\"__viewstate\"\""+Environment.NewLine+
//                    "skvKWVvu7pEB6pEz0zK7ReWw9zqDZr1sxk0n3FpZrXQbwGNOu3dPohkQSg0ndHTQzcqeg9Y0P2AFqsWbJkufJ0rA7OecaQ8t/6HVpTVO5W55hTasrQ1r+HZiUCiQzWREwiar+EnfEwp"+
//                    "D3DkCeo7MRqtjLjNmKhhooDAIPaF/P9n0HyaJFNSVQXX7QmcWYca3yjmfYyPMuflJqslHf4MAsyJ4Ocvm8pvev1/4b1oc9oiJE58GFqTw+tKMMYGw+muGOsoErda7EtZijeEJllNkJv"+
//                "/oK8eNQX2zJP2gJ+F35fDIUuHIzl8YQ/3lMdn8l0cH7BXLVs7z3jUeDExLfe0Yu2HX8k6YucnkawKnUbSJTxaRnGIJry34GpHWRSXk04Ecbxy+nbC4NjE/YSqX4uTMDtvpV88A1cdtR35fd"+
//                "ogaSvP5RE54hQCReQ/JzfYvhKuwg/f+VDDODYXx4kSMgudSJLB4DYdJZtaqlHmDZ8U4UtdR+2lMP7qXN8I6uRl/6xs6VC3YvN+JafUymEKhgN4iSvUCKNOLo/AqZXTHNfSD2W+XFI2X0gZW4"+
//                "erfO7dvQB6BGxBDHwT78INSWyHExwJdAc9ngVAzA40Q9sdM7iue0rIlnQ5XZIaDdsvX2Mv3f0MZr9NNsAMOpi0T29/TibbyfYkbwlotJ+jI3WGutCraH93HzgX9L2QSvLs3aWywnlBC60Y"+
//                "8Z0fDw50FRUdGlJCUIrLySYRSnFpIIHhg8U9HM3nnxfG6s8xeLqT1IKBYaP8VNYFodCRgxsCOR24OyhFlzC7SrqPIoJtmLxPq3n5cY/dPp/F5LTE2jTNihl+uVwkN71z4Lo8PKEPh2jBtS"+
//                "yeSXR9kAwPJjGQ5HeoVzL3GGgXYRKVV7DRVxlNny53qW5YTYqob2BW/P0U0FGGe4IgwPmXhIioeb+soG1h16KHh349Rdyk2/6wmlBi1je1M/uN+eF/Wa0bVGnVN4YPZdunhVbK9tXs/uj"+
//                "n02Q3B14Jw/mQj/sV9FFIl0i97eaqyWqo0XwKnl96+HkTkLqK551PJkNZ1aSSuB9yVOYeWWRJBly0dhs8DZ2vePnxrVwoeJbv+0sMguWXLxzcAHm9WWYgbtO+vYSQVGsBvazNd7If4PCJS0"+
//                "Yx3L9r49wRlJt0OpsgbbeQbIlfQQaR30Qr1il0W8ZuRFMBivZpCzD/r/8rc+IqQqnzbK5ndALJogrL8+dXpsBnc84Qy5BDdLYLNxTE8JMLDgkO022zy4Yyq7f6tIxxhyMz+39X6CC1nTv"+
//                "9XyeLVnx78IFlweqXrJ2fgFeO5YnmRH2qOT/K4KbOEkPw7ourk5DfT6AY9jmrIO/O61jVodv6Mk2E/mUiaJ7s9cC83NE+8k9yYaQetd1G8Ag3vrg0lwIdXAwnHVJYQhWTy5xNrQDLN9gwfjU"+
//                "bws4Lu8GrZk9pJDw9Vn0NJ8ipVyIzaHHfZV0TWHxk1I9vl5C8GCnAAnZHRU4OzeDquNG5mdzqsp8+YnzRWvZM/gM/bFyBfSGGXGLb2uX5wivmNejEu7N1dOdu43SbmEql0m3e2qYQiKzzF"+
//                "6HUqNSiZitd7qGEq3sTJe7RtIbR+iZjM9htOIGpyhqX1up4Ij7HB/BujXYf/6nj4Eeu8BoQF+DTZlsvHxVcu4p0WokZa+eHPAottUED9JPuRJm0vd2DKURyUBCdHoPdEtYAQRPohWooHbR"+
//                "HD+6oS3e+c1778PomNq7qZG5SPdzPNsyiaguibNjHKtyw7PtcM2LTJmtE1amrwTNamk6VuyH1tKxUmTikKeVrH7VgvSJK/dTyVXmY0uZDHNS5mw1oCTSltdfxeYHChBeSHhyAIDbElCy5B"+
//                "VEnbVMZ+iVH3OKQt0/914KODGxql2+j2/pdl29QK8TJuNV10xm7GY+EDbZlITnU+bmiPoUJ9DMB8sh7F6LYFibezpwsVXCsz+3z2VeAViMUKVvXDkcG/c+hPtHvKgOyl4t2GGb4zVQeO1L8"+
//                "SE+zeVV6WabZBEna9z5YT/SUKBoDIjvTql4113rsM4Ksh7mL4tbFPxZtPSw65QAYl5XT9t6Iphz6Z0dBqUwKf6EizgnVWBb7wO+UiL0DNFL/8qdOt2kNXCxtHGPU8vpiFvP0V1chmix/dd"+
//                "sfpsVP6/IfBBlyJAj1SpUg8JaUXiEt9T/7clsFXZH1neAcsyQrXxPO7ERZ2bbg9zdk4+a8RvJbDBAtJl/8M/duzxmHYpHX0jQZLLXiBetR1Fld39t/+egRbQqIB6DfnWAwArD//eSAx5H"+
//                "onD0KyvyimR3nS6MRAY/N3dInmFFE3zQiWSH/sMazu/rNVFhnIHWl8R4qQU+Y/x3+bTamIoVSPtGa4v8bUlDVpm6npI0C22LpJMCDkRCAzxSYdFXhSG+FrkFPbv6g4Y3WG4h13WYecHjv6B"+
//                "Qfz/Teo4TgqNG7sB2QtSgxK8xO5BMYFrWl7Bo3805jKQr5r0BY4FbszWHfIuwTQd8Piuyn/YP2vZOoyNd14QT0jKzFFwVfTYhrnT+PX7D2FiAMLnXyI42gGUpIOysgu4m7axca8WY3aL1CB"+
//                "P0xS4dKn8rttcBX+UWCk2YRMbvJiRLl3UIriAwUpkHdSJ2c+7IivjcIIMAXx02njwwju59ZGJeUrIkNafur2z6jJ5VKkgEMA6cZOxpVxSYX4wqckQvqSv/f7mPxHX5Tl1LdcwX5pn4WL+Jh"+
//                "Vrw52k7g6R38LbQZi/tQcxc5ef+RbTeMKj4SFRAVnqCRX106vsxRWXw5NxzrFjWwBCJEJscnsHDUhg0A4EhV5nQzwqHXmMqHj633b5hD2VIXDIoUK11iUbYz3xxQfr54/wJc0jch+AWfRzk"+
//                "UwRXxWnPe6yCmeOUkwq2jePyMtDKDw4qoL2QY79ObzzHNhU1fWGqmT4JQpn4w42cGN5+wZ8w+OEi9MWD8FUluumD/wI892jUebmkinpERUMY9HbKbZCU5/Y8IyRmWLWkTPJo+wkRW7rfE"+
//                "ErViibTE9PM6zpoZ34CgQBAROoOGlP46MDEPZYDXRri282/lQWSrBFFN1LJnyZJklxNKbg7vAw+D4YeykEWb5jllYdlTN3GJZgI2b8sJreFBUx55370tzyFUG/ZIPhp8+1LzZhLm8nTU0"+
//                "TjJAy398s6WDAtcDdwF8A2D/LSZw//xvTz7QCzo9wU7ZQkpZSKXh4KReINQ3laN14hKAXG3ABeMZr4JhQCNfhgQQShQPhmdV5VYTQ52r7p4pRgJ5e+Tc9LDpc2aZHO1EFQi08imbHbrgfj"+
//                "eaKXgRv7LpcgrhWND9OANrajSsaGiIgtPTd+28GxruDZ+e9+qbztgbf+mKDAu/4+/4Ho7lmrkiZJ0hktbWkbYGg0h8IUu/dJon6VhF0vGzaUTa+ZzLiPZMf45Uiqdu8/dxetxBQMJHCekFc"+
//                "age2RvPMl4VaEC8oT8Lo7y599IQ5hzOer2MqZeKja8a1TLfdIPcrAyz0qrn2m343PzXiR7JRAeALhkdLsrHcQUEeh6mnoqT46DRDsTcgHO7GPWWy6TW/3rcFa3fGo8ElTu1Vm0TmDSJ5wLo"+
//                "oxkCyoKwg+LPHxtE75EkhmJ3B6qCfOPC1sDaeBAtbtnai1UPxTVMIUPBzo+tEWFXCt5baSt7zIKFTHZQgmL2Az70kmQYO9I3effKs994LkWpCFIGgK4hk2z+H/zRO+F1yJwIw3JT1FZOfy"+
//                "buYe2D0JeAaOOx2Ka1IltsWSObeLUJO9hElocKu9rCsEkmh/2KWiFtP8bSsR2HpEPy/38Q1YsKVTtXMgySDR7xzytrjD7o8ZeAm8hqKaw2Jeh6TBSkPHN/7HowhNwtgQLB4+U1sRdDC/"+
//                "NlBG82zuBZLsiSjmRdlJnw1PapH5D7y1J6JoqYMkU9IrgG1LOaxxDixXh3R+1kRwIKcP6kV7Ndm0B1rtQq5HZxISv/yEd0asLwuEF2dB79vqeAdMx91FvJGDne//FLZaqrH8ZhL8pU912"+
//                "KazQw81N7xIOC+NpQhJnf9AyzSYw4U4KiaQDRRkWpPO8snTvHRxe2jUcMfpDXlpXbLcoXke3DOF7MVaFcNjmY+Aq0xs0EjoPZzCIqAU/jj7sgWUIcSVfeEku+eT4VDAI+qVHDArPWgDbb"+
//                "zKkyDNkjeK4cy7Z8y7lbPOlxAui/2fo8NYAIKttX/85qVQ51FH+Ja/fSZHn9OyrHc/H1nmHKURrKXMfi6ODqaB4FY8KoD91ddU38OvZ7Z2V5s92BvTAc5wrOfUEou3yEBkU4Fs7GElsH"+
//                "5Cm0CbaCdBKwSdCbwJvwoSBwg0fczBKrQa2/1/HSjIVWCAImZbBdM7ODP25yHhfm1XIX6fWQ7FdL9EwFCjM+T2nIym/N+TveJA3GCYMgBUsUuIBf5p/LVppH4EMJ/9Ae10veLChUPAvX"+
//                "6OgVjTOyBSpAew/K7kjDO3ggX5cuRwnc1w14ZDdJSutO4Q6zAVWyFVNZL9TJnMWNgoruyG9UpmXYNCm8HnBgXkiCkmQnL0JSrvz3PVi8b7AVte3VZdJWVbbvZL1FvYrTDH7IhZ705FUdK"+
//                "z8PONVnELhxMvwyXLHm4Z2jtIe15nd6VWU9hBEkVITZ6j8LCIP6JXZoyPOJohKnpkkHOecgVm6FqyoKwRHzTCiEeeXzzL/L4NHeU5YNrIrK/2eL5Rx6o3sRUPZ4W6SAkhPFabZITx/fvwC"+
//                "T9fPQS0Lh+z/Lub2Zk7/LPSQYW8DpIKnNctksg8LptcIjHdzl7bG0y/IrSVdiEkmXr0XFCUDfhxXRidFifHs12awTKdtG07qGAchjRR0Y0pvCedwcg9TMUFG/BP9HP+XbqvqvgCntVOKXl"+
//                "ksB/c6dbeS4FNnYTjU+s/i27iLwqC95Id24oEnVSR7KGUkMRAuRdjwIpCUcahV6kF52bg91R3E8dUHmZkNsdYZ18nLgQq4Vvrb/HJrUjanhBT4Exa2PaBg1VmxQpV+LP2mu0QN/LAoI4huC"+
//                "lQy7TaUAsrOm3HX9MhxPqcDjy0m/unqpQ/CkFdsLx5KD5Ez5Loik3te22iBEGGvpDKHg5PeSujgnbVZADs8hmlXPGdcZU6mjuNK76E3aI6kd2ZHpBTZq+Nz+vd4aPtwz00SQwxTlQTXn3"+
//                "CvWFA2hUmwXWSVvs0ZaRZ3uloXHANskY5O71X56q4ktdu3oFgOSBtrO5iJUhwZRcky9qVtfmbf5f5neaQZ3yyN3wvVraAtzTeGq6+W+DI82aDDJUN6RnJLPHX3TSNsb6EUZFfVz/S"+
//                "W5qMG/y1s3DDiF0iW7QD6pyVSfD7TwoBjLwfgzNw+sI4WMvqFk/ISuL651V85Kd8HpvvXw18S47efojVHXt1xfcRaM3yjz7jkWMIyC3RFuYsY3ehC3sVOAFtkyFXt4qhwhOXYTZioAaeW"+
//                "3yix7s8FGJfgOZGyOKXgAed3itQo9F7vT9UlGY+cFMLAQ2K+x5X8xtcsFIRxh0s5qRX8tvYpP5vyIsnIlpkJcm4apOdWGzPcEKBuKgDtfpvi/DgemThB6p65UjHHyJX+G0swGx+T31ooF"+
//                "87y1JF7BJTjBxm/SFlhYbATzXGKG+HWhPuWsz8pehmGRfxiEKujCvZ7FdIEnFe7VRzjgKVPP/IVJfgD/Wux1shESCcXkyzIusuUHnHCUi3miWh8ZHX4VoJhXxFWmtMKd15Bzwj8ouYaLi"+
//                "2jnIt8SN4zRwqPKD2LbErv6YgmvxmPxKnzsLeWTivC4XM0K7Kj0aX3hXKaX4DXX2fDV2hdE8Ge7X1pCaBKieqgvUWzyC2vjWXNMyRK6afabUIlHjuC5hVQAIOWKrDsWFifQ+CR1ZvZjU"+
//                "CE9Y1y78gkNoO6erFdPJzRQlDBtU+4sRDTs2djXfI/rcasvfnZKzgqPmisE3A/sq5gRs7vinyVMOCrouhgYPrVbMBOtOjFfo/4mi2olpMTKh+kTkd6Mtpy1LlWGENXFdVRvMmrsAo2OOD"+
//                "20a6PgulvTUg8006QokIeve0HMipqL2fvNPl6N/I75hv7sSZEAGAwMjQ5ct7MwAk0P1ytBRB4uQhPBZ81RkXJqAe4MvGokuauHKRFlb6gV5i1vWYWQ8AmdUuinBWLorDOhUyYee6x5B4om"+
//                "Zm0VaYAhgz8jZvlDJk5eAZt3ntMZee0XLvS44RZNg8uKOjoOF4OP8dHK6ILGOku2YLE87CsRexVIotGQJSOGM0PXujkQlxA1IewWfQ3pq8eJqEf0B7c5GDyQRQZN9yZevbjOKY12U5atd66"+
//                "DzKLlkZ7FSrujwrvtyxdoXJNifUAN0twcSFBr4Sgkj/5wQusDzGvExDwjyIugwCZWqhtqZ1qSARRSa65khZvG0ef0gHM8krynsdAKxa4UMmJYxtz2VW5TfGp/zsAMTCZWKVCspyUrqenD"+
//                "5TZMZs5O19vVC7Uxys2z0Z3gGYc+aGT2nuVNlgbeuJUwarjZ6JA6ZOsBPx0o+IecWZSgQPW55Wmwi+ABf4Ld9U6svXBnsqX81KTHiX0Zj79B5l5Nq20jKvXGQ+aY+blTuYbprloQDLhS3"+
//                "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"+
//                 Environment.NewLine+"------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+"Content-Disposition: form-data; name=\"\"__viewstategenerator\"\""+
//                 Environment.NewLine+Environment.NewLine+"8C700690"+Environment.NewLine+"------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+"Content-Disposition: form-data; name=\"\"__scrollpositionx\"\""+
//                 Environment.NewLine+Environment.NewLine+"0"+Environment.NewLine+"------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+"Content-Disposition: form-data; name=\"\"__scrollpositiony\"\""+
//                 Environment.NewLine+Environment.NewLine+"0"+Environment.NewLine+"------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+"Content-Disposition: form-data; name=\"\"__eventvalidation\"\""+
//                 Environment.NewLine+Environment.NewLine+"G0GMW2BYfQi/FKQfonuxuT8VRWty4EGms8Qeb8e+/xj4aaX+gXdRkMxLjX2ZSPH5XgZjPYdXtuGcCpavIpcL7C+oxOSHAUyom6uTyDmMuAQIRYOpKv/Mosl74pY3z2yHA+t8QFhw56rwpJF5NAdZ9HnHSMiZDSulrxvN/8AuYx2zFVe5SO2gFJ7GD35GFpQgAAQQmr36/r+kbykQl+pRbCiI2UwarIl3awPAkaaxzLSR2uLwhTfDXCKayqEDmwz9AoCVa9cFxJNZp3sTiVmML4wrSZa4FVzDzzUBO1lCCuQJwwFoV/fxb0hF9gk8gel0152OTX13fP0LM8+4rSuURO/XLkTIV1swMhEI0Ncw2yZcO44PqmXpC0lziTBTHwncKYcHdkPRkGAmqSv8q4ar6/IzKL2fHIRuB6qCovK3ZxBhneSTy4CjzQ2CNLH7aOupbW9JdnFYiqKIJbcDCun4w9bJExo3hjxMX4tRaIU/90xbQ66/bD8ZNGji/AhBNEvOG2XMsD5FuNDmvbjg5eDtp/Xj+FmOsqYi7ryesPE/EtI2l66JkLIRJifvSiyQzX8zfwZpDB4HlGCpcLfQ7uyglaexQRr/v5PxNVXHX8otRwr3X4QJfMiGtCguXiftLc/NhPtz5Syds6TbZQ33ZPmAaZZ5Of0AnbbWtwLK9QFItEB6NOc5YB4OjRf3wZupjdOW5q8vYowmF9dgqyj20yOcMnS4o8qx2AEgwC4lF3xBGvKQR8fvWiCPYDotNX6jt7aWQgHebhvf2KpB3A9rjBsHkf4sNdbmKGBJiwEz0yOm+iuZfKGfDbf6Lt1DeDTPynhzFUhQ7kqNRDYa6oTir3Gr+bXn+r8BnMIXAjI4Ff27GNdJxg/pcHXX68mO03EUtXWvt0pZpRhTDa5L9v19q4TsS1IZ/hbpja4GKvdNf1Qr/Dz6yuZ8pYiCfeK5MJVGYVGBGzkQwXOWEudB5JoAUtD6kohNIIdKszXxRkLJ0u7yjnkEA0cdzZilXjUE/lV8e+aMwNhd5FVWKZs8QWS1kNvwy2Wqa3iQ+jMK5U9KKP6V0PHQFEid4k+HIy4n1qbTcWgDgXQSDEXQGICKj/p+KVddzBo1PMxpDBGVU4UU0NF/fCi7DRiWCFgqOF+HrC8o8k3cFJiT9zanPQIbq2oI1e6KpMiB15+8X7aX2Bn3M4Xs5sYqSVgGNBqZY2aY7ay8T0dJS3Q4ne4Itp72FLXpwyrtkqJ2I787bzCeA7KCTtp6xguSYJwa1T+dJi3HV366yzEGxlnFRTr+vGzUhVN2EW7+kbzzUm1CmLsNzHGcSvPFaPX7pm25G4+mjZC1K8pDIXOWpxNJV03Er6krFsFnrsusdFSCR2z3hiafw4+KzJhSg3kWndcRUDkAQnEVhruKKg42Cfu7uxRVeBfluytOAq/ed3fpBhHMnQ8a6BeqnxnSpIrxmMalofyqeQK9qhmH3jNmTGNndw=="+
//                 Environment.NewLine+"------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+"Content-Disposition: form-data; name=\"\"ttxtinq_no$thinkbasetextbox\"\""+
//                 Environment.NewLine+Environment.NewLine+"GRDO-1802-0008-01"+Environment.NewLine+"------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+
//                 "Content-Disposition: form-data; name=\"\"ttxtvesselinq$thinkbasetextbox\"\""+Environment.NewLine+Environment.NewLine+"GRAND DOLPHIN"+Environment.NewLine+
//                "------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+"Content-Disposition: form-data; name=\"\"ttxtissued_nameinq$thinkbasetextbox\"\""+
//                Environment.NewLine+Environment.NewLine+"김현호"+Environment.NewLine+"------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+
//                "Content-Disposition: form-data; name=\"\"ttxtfrto_due_dateinq$thinkbasetextbox\"\""+Environment.NewLine+Environment.NewLine+"2018-02-08"+Environment.NewLine+
//                "------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+"Content-Disposition: form-data; name=\"\"ttxttoto_due_dateinq$thinkbasetextbox\"\""+Environment.NewLine+
//                Environment.NewLine+"2018-02-28"+Environment.NewLine+"------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+"Content-Disposition: form-data; name=\"\"ttxtissued_dateinq$thinkbasetextbox\"\""+
//                Environment.NewLine+Environment.NewLine+"2018-02-20"+Environment.NewLine+"------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+
//                "Content-Disposition: form-data; name=\"\"ttxtdue_portinq$thinkbasetextbox\"\""+Environment.NewLine+Environment.NewLine+"KRPUS"+Environment.NewLine+
//                 "------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+"Content-Disposition: form-data; name=\"\"ttxtportnameinq$thinkbasetextbox\"\""+Environment.NewLine+
//                 Environment.NewLine+"Busan"+Environment.NewLine+"------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+"Content-Disposition: form-data; name=\"\"ttxtsubjectinq$thinkbasetextbox\"\""+
//                 Environment.NewLine+Environment.NewLine+"ELEVATOR"+Environment.NewLine+"------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+
//                 "Content-Disposition: form-data; name=\"\"ttxtremarkinq$thinkbasetextbox\"\""+Environment.NewLine+Environment.NewLine+""+Environment.NewLine+
//                 "------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+"Content-Disposition: form-data; name=\"\"ttxtqut_no$thinkbasetextbox\"\""+
//                 Environment.NewLine+Environment.NewLine+"E6256686"+Environment.NewLine+"------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+
//                 "Content-Disposition: form-data; name=\"\"ttxtvesselqut$thinkbasetextbox\"\""+Environment.NewLine+Environment.NewLine+"GRAND DOLPHIN"+Environment.NewLine+
//                 "------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+"Content-Disposition: form-data; name=\"\"ttxtissued_namequt$thinkbasetextbox\"\""+Environment.NewLine+Environment.NewLine+
//                 "Daisuke Nishikawa"+Environment.NewLine+"------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+"Content-Disposition: form-data; name=\"\"ttxtfrto_due_datequt$thinkbasetextbox\"\""+
//                 Environment.NewLine+Environment.NewLine+"2018-02-08"+Environment.NewLine+"------WebKitFormBoundaryH6QZcbJi74mbiSgF"+Environment.NewLine+"Content-Disposition: form-data; name=\"\"ttxttoto_due_datequt$thinkbasetextbox\"\""+
//                 Environment.NewLine+Environment.NewLine+"2018-02-28"+
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtissued_datequt$thinkbasetextbox""

//2018-03-29
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtdue_portqut$thinkbasetextbox""

//KRPUS
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtportnamequt$thinkbasetextbox""

//Busan
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""tftlstbxreportfile$selecttextbox""


//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""tftlstbxreportfile$fileupload""; filename=""""
//Content-Type: application/octet-stream


//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtvalidperiod$thinkbasetextbox""

//29-03-2018
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtpaymentcond$thinkbasetextbox""

//60 DAYS AFTER SHIPMENT
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtsubjectqut$thinkbasetextbox""

//QUOTATION
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtremarkqut$thinkbasetextbox""

//***"" WE HEREBY DECLARE THAT ALL OF OUR SUPPLYING PRODUCTS ARE ASBESTOS-FREE."" *** GENUINE PARTS FROM ORIGINAL MANUFACTURE/MAKER ***
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""tddlqut_curr_code$thinkbasedropdownlist""

//JPY
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtqut_amt$thinkbasetextbox""

//3800
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtqut_shg$thinkbasetextbox""

//0
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtqut_ttl$thinkbasetextbox""

//3800
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtqut_shg_rmk$thinkbasetextbox""

//Freight Cost: 0 Packing Cost: 0
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtdelivery$thinkbasetextbox""


//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""tddlgeim$thinkbasedropdownlist""

//000
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""tmgridinqlist$ctl02$qty_qut""

//1
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""tmgridinqlist$ctl02$qut_unit_price""

//3,800.00
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""tmgridinqlist$ctl02$supply_edition""


//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""tmgridinqlist$ctl02$delivery_kind""

//7Days
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""tmgridinqlist$ctl02$chk_ge_eq""

//GE
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""tmgridinqlist$ctl02$inq_item_rmk""

//FOR READY SPARE / 2018년 03월 한국 기항시점에서 TECHNICAIN -정기 점검 대비하여 SPARE PARTS (LIST)재고 조사하여 재고 없는 ITEM입니다.
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""tmgridinqlist$ctl02$qut_item_rmk""


//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtreqsysno""

//GRDO-SUP-PUR-180208-0001
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtinqsysno""

//01
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtqutsysno""


//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtinqsys""


//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtvdrcode""

//1002176
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtvdr_name""


//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtsubject""


//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""hiddtxtpartkind""


//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtvessel""


//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtcus_odr_no""


//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtdept""


//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtissuednameeng""


//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtodr_fm_email""


//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""ttxtodr_to_email""


//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""txthiddxmldata""

//<?xml version=""1.0"" encoding=""euc-kr""?><xml_data><detail><PART_NO>1</PART_NO><PART_DESC>RELAY(MY4N,AC100V)</PART_DESC><UNIT>PC</UNIT><QTY_INQ>1.00</QTY_INQ><QTY_QUT>1</QTY_QUT><CONT_PRICE></CONT_PRICE><QUT_UNIT_PRICE>3800.00</QUT_UNIT_PRICE><QUT_UNIT_SUM>3800</QUT_UNIT_SUM><OH_EDITION></OH_EDITION><SUPPLY_EDITION></SUPPLY_EDITION><DELIVERY_KIND>7Days</DELIVERY_KIND><CHK_GE_EQ>GE</CHK_GE_EQ><INQ_ITEM_RMK>FOR READY SPARE / 2018년 03월 한국 기항시점에서 TECHNICAIN -정기 점검 대비하여 SPARE PARTS (LIST)재고 조사하여 재고 없는 ITEM입니다.</INQ_ITEM_RMK><QUT_ITEM_RMK></QUT_ITEM_RMK><E_CODE>E0168</E_CODE><S_CODE>S3330</S_CODE><PART_KEY>0026-E0168-S3330-P0017-01</PART_KEY><S_SORT>ELEVATOR (USHIO REINETSU CO., LTD. / (U) ELEVATOR 350KG X 45M/MIN) / ELECTRIC PART</S_SORT><M_CODE>M051</M_CODE><K_CODE>K0049</K_CODE><T_CODE>T0480</T_CODE><PART_CODE>P0017</PART_CODE><EQU_KIND>01</EQU_KIND></detail></xml_data>
//------WebKitFormBoundaryH6QZcbJi74mbiSgF
//Content-Disposition: form-data; name=""reporturl""

//./frmPurApprovalReport.aspx?DocKind=VENINQ&SysReqNo=GRDO-SUP-PUR-180208-0001&SysInqNo=01
//------WebKitFormBoundaryH6QZcbJi74mbiSgF--
//";
//                WriteMultipartBodyToRequest(request, body);

//                response = (HttpWebResponse)request.GetResponse();
//            }
//            catch (WebException e)
//            {
//                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
//                else return false;
//            }
//            catch (Exception)
//            {
//                if (response != null) response.Close();
//                return false;
//            }

//            return true;
//        }

        //private static void WriteMultipartBodyToRequest(HttpWebRequest request, string body)
        //{
        //    string[] multiparts = Regex.Split(body, @"<!>");
        //    byte[] bytes;
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        foreach (string part in multiparts)
        //        {
        //            string _file = @"D:\Working\HTTP_POST\Http_Cido_Shipping_Processor\Http_Cido_Shipping_Processor\bin\Debug\Data.xml";
        //            File.WriteAllBytes(_file, Encoding.ASCII.GetBytes(part));
        //            if (File.Exists(_file))
        //            {
        //                bytes = File.ReadAllBytes(_file);
        //            }
        //            else
        //            {
        //                bytes = System.Text.Encoding.UTF8.GetBytes(part.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n"));
        //            }

        //            ms.Write(bytes, 0, bytes.Length);
        //        }

        //        request.ContentLength = ms.Length;
        //        using (Stream stream = request.GetRequestStream())
        //        {
        //            ms.WriteTo(stream);
        //        }
        //    }
        //}

        private static void WriteMultipartBodyToRequest(HttpWebRequest request, string body)
        {
            string[] multiparts = Regex.Split(body, @"<!>");
            byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                foreach (string part in multiparts)
                {
                    if (File.Exists(part))
                    {
                        bytes = File.ReadAllBytes(part);
                    }
                    else
                    {
                        bytes = System.Text.Encoding.UTF8.GetBytes(part.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n"));
                    }

                    ms.Write(bytes, 0, bytes.Length);
                }

                request.ContentLength = ms.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    ms.WriteTo(stream);
                }
            }
        }

        public Dictionary<string, string> GetStateInfo(string strRequest)
        {
            try
            {
                Dictionary<string, string> dicState = new Dictionary<string, string>();
                string sData = SendRequest(strRequest);
                HtmlAgilityPack.HtmlDocument _doc = new HtmlAgilityPack.HtmlDocument();
                _doc.LoadHtml(sData);
                string strState = "";
                HtmlNode _node = _doc.DocumentNode.SelectSingleNode("//input[@id='__VIEWSTATE']");
                if (_node.Attributes["value"].Value.Length <= 65519)
                {
                    if (_node != null) strState = Uri.EscapeDataString(_node.Attributes["value"].Value);
                    else strState = "";
                }
                else
                {
                    int iLimit = 65519;

                    StringBuilder sb = new StringBuilder();
                    int loops = _node.Attributes["value"].Value.Length / iLimit;

                    for (int i = 0; i <= loops; i++)
                    {
                        if (i < loops)
                        {
                            sb.Append(Uri.EscapeDataString(_node.Attributes["value"].Value.Substring(iLimit * i, iLimit)));
                        }
                        else
                        {
                            sb.Append(Uri.EscapeDataString(_node.Attributes["value"].Value.Substring(iLimit * i)));
                        }
                    }
                    if (_node != null) strState = Convert.ToString(sb);
                    else strState = "";
                }

                dicState.Add("__VIEWSTATE", strState);
                _node = _doc.DocumentNode.SelectSingleNode("//input[@id='__VIEWSTATEGENERATOR']");
                if (_node != null) strState = _node.Attributes["value"].Value;
                else strState = "";
                strState = Uri.EscapeDataString(strState);
                dicState.Add("__VIEWSTATEGENERATOR", strState);
                _node = _doc.DocumentNode.SelectSingleNode("//input[@id='__EVENTVALIDATION']");
                if (_node != null) strState = Uri.EscapeDataString(_node.Attributes["value"].Value);
                else strState = "";
                dicState.Add("__EVENTVALIDATION", strState);
                return dicState;
            }
            catch { throw; }

        }

        public Dictionary<string, string> Get_StateInfo(string strRequest)
        {
            try
            {
                Dictionary<string, string> dicState = new Dictionary<string, string>();
                string strData = SendRequest(strRequest);
                HtmlAgilityPack.HtmlDocument _doc = new HtmlAgilityPack.HtmlDocument();
                _doc.LoadHtml(strData);
                string _state = "";
                HtmlNode _node = _doc.DocumentNode.SelectSingleNode("//input[@id='__VIEWSTATE']");
                if (_node.Attributes["value"].Value.Length <= 65519)
                {
                    if (_node != null) _state = Uri.EscapeDataString(_node.Attributes["value"].Value);
                    else _state = "";
                }
                else
                {
                    int iLimit = 65519;

                    StringBuilder sb = new StringBuilder();
                    int loops = _node.Attributes["value"].Value.Length / iLimit;

                    for (int i = 0; i <= loops; i++)
                    {
                        if (i < loops)
                        {
                            sb.Append(Uri.EscapeDataString(_node.Attributes["value"].Value.Substring(iLimit * i, iLimit)));
                        }
                        else
                        {
                            sb.Append(Uri.EscapeDataString(_node.Attributes["value"].Value.Substring(iLimit * i)));
                        }
                    }
                    if (_node != null) _state = Convert.ToString(sb);
                    else _state = "";
                }

                dicState.Add("__VIEWSTATE", _state);
                _node = _doc.DocumentNode.SelectSingleNode("//input[@id='__VIEWSTATEGENERATOR']");
                if (_node != null) _state = _node.Attributes["value"].Value;
                else _state = "";
                _state = Uri.EscapeDataString(_state);
                dicState.Add("__VIEWSTATEGENERATOR", _state);
                _node = _doc.DocumentNode.SelectSingleNode("//input[@id='__EVENTVALIDATION']");
                if (_node != null) _state = Uri.EscapeDataString(_node.Attributes["value"].Value);
                else _state = "";
                dicState.Add("__EVENTVALIDATION", _state);
                _node = _doc.DocumentNode.SelectSingleNode("//input[@id='ttxtFrom_Due_Date_ThinkBaseTextBox']");
                if (_node != null) _state = _node.Attributes["value"].Value;
                else _state = "";
                _state = Uri.EscapeDataString(_state);
                dicState.Add("ttxtFrom_Due_Date%24ThinkBaseTextBox", _state);
                _node = _doc.DocumentNode.SelectSingleNode("//input[@id='ttxtTo_Due_Date_ThinkBaseTextBox']");
                if (_node != null) _state = _node.Attributes["value"].Value;
                else _state = "";
                _state = Uri.EscapeDataString(_state);
                dicState.Add("ttxtTo_Due_Date%24ThinkBaseTextBox", _state);
                return dicState;
            }
            catch { throw; }

        }

        public Dictionary<string, string> GetStateInfo(string strRequest, string strPostData)
        {
            try
            {
                Dictionary<string, string> dicState = new Dictionary<string, string>();
                string sData = SendRequestFormData(strRequest, strPostData);
                HtmlAgilityPack.HtmlDocument _doc = new HtmlAgilityPack.HtmlDocument();
                _doc.LoadHtml(sData);
                string _state = "";
                HtmlNode _node = _doc.DocumentNode.SelectSingleNode("//input[@id='__VIEWSTATE']");
                if (_node != null) _state = Uri.EscapeDataString(_node.Attributes["value"].Value);
                else _state = "";
                dicState.Add("__VIEWSTATE", _state);
                _node = _doc.DocumentNode.SelectSingleNode("//input[@id='__VIEWSTATEGENERATOR']");
                if (_node != null) _state = _node.Attributes["value"].Value;
                else _state = "";
                _state = Uri.EscapeDataString(_state);
                dicState.Add("__VIEWSTATEGENERATOR", _state);
                _node = _doc.DocumentNode.SelectSingleNode("//input[@id='__EVENTVALIDATION']");
                if (_node != null) _state = Uri.EscapeDataString(_node.Attributes["value"].Value);
                else _state = "";
                dicState.Add("__EVENTVALIDATION", _state);
                return dicState;
            }
            catch { throw; }

        }

        public Dictionary<string, string> GetStateInfo(HtmlDocument htmlDoc)
        {
            try
            {
                Dictionary<string, string> dicState = new Dictionary<string, string>();
                string _state = "";
                HtmlNode _node = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='__VIEWSTATE']");
                if (_node != null) _state = Uri.EscapeDataString(_node.Attributes["value"].Value);
                else _state = "";
                dicState.Add("__VIEWSTATE", _state);
                _node = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='__VIEWSTATEGENERATOR']");
                if (_node != null) _state = _node.Attributes["value"].Value;
                else _state = "";
                _state = Uri.EscapeDataString(_state);
                dicState.Add("__VIEWSTATEGENERATOR", _state);
                _node = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='__EVENTVALIDATION']");
                if (_node != null) _state = Uri.EscapeDataString(_node.Attributes["value"].Value);
                else _state = "";
                dicState.Add("__EVENTVALIDATION", _state);
                _node = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='__EVENTTARGET']");
                if (_node != null) _state = Uri.EscapeDataString(_node.Attributes["value"].Value);
                else _state = "";
                dicState.Add("__EVENTTARGET", _state);
                return dicState;
            }
            catch { throw; }

        }

        public Dictionary<string, string> GetStateInfo(HtmlDocument htmlDoc,bool IsUrlEncode)
        {
            try
            {
                
                Dictionary<string, string> dicState = new Dictionary<string, string>();
                string _state = "";
                HtmlNode _node = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='__VIEWSTATE']");
                if (_node != null) _state = HttpUtility.UrlEncode(_node.Attributes["value"].Value);
                else _state = "";
                dicState.Add("__VIEWSTATE", _state);
                _node = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='__VIEWSTATEGENERATOR']");
                if (_node != null) _state = _node.Attributes["value"].Value;
                else _state = "";
                _state = HttpUtility.UrlEncode(_state);
                dicState.Add("__VIEWSTATEGENERATOR", _state);
                _node = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='__EVENTVALIDATION']");
                if (_node != null) _state = HttpUtility.UrlEncode(_node.Attributes["value"].Value);
                else _state = "";
                dicState.Add("__EVENTVALIDATION", _state);
                _node = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='__EVENTTARGET']");
                if (_node != null) _state = HttpUtility.UrlEncode(_node.Attributes["value"].Value);
                else _state = "";
                dicState.Add("__EVENTTARGET", _state);
                return dicState;
            }
            catch { throw; }

        }

        #region Print PDF
        //public void PrintScreen(string sHTML, string sFileName)
        //{
        //    try
        //    {
        //        File.WriteAllText(sFileName, sHTML.Trim());
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //public void PrintScreen(string strHTML, string strFileName)
        //{
        //    try
        //    {
        //        string strHTMLFile = Path.ChangeExtension(strFileName, ".html");
        //        File.WriteAllText(strHTMLFile, strHTML.Trim());
        //        SautinSoft.PdfVision _vision = new SautinSoft.PdfVision();
        //        _vision.ConvertHtmlFileToImageFile(strHTMLFile, strFileName, SautinSoft.PdfVision.eImageFormat.Png);
        //    }
        //    catch (Exception e)
        //    {
        //        //
        //    }
        //}

        public bool PrintScreen(string strHTML,string sFileName)
        {
             if (!Directory.Exists(Path.GetDirectoryName(sFileName) + "\\Temp")) Directory.CreateDirectory(Path.GetDirectoryName(sFileName) + "\\Temp");
            string HTMLFile = Path.ChangeExtension(Path.GetDirectoryName(sFileName) + "\\Temp\\" + Path.GetFileName(sFileName), ".html");
            File.WriteAllText(HTMLFile, strHTML.Trim());
            SautinSoft.PdfVision _vision = new SautinSoft.PdfVision();
            _vision.ConvertHtmlFileToImageFile(HTMLFile, Path.GetDirectoryName(sFileName) + "\\Temp\\" + Path.GetFileName(sFileName), SautinSoft.PdfVision.eImageFormat.Png);
            if (File.Exists(Path.GetDirectoryName(sFileName) + "\\Temp\\" + Path.GetFileName(sFileName)))
            {
                File.Move(Path.GetDirectoryName(sFileName) + "\\Temp\\" + Path.GetFileName(sFileName), sFileName);
                return (File.Exists(sFileName));
            }
            else return false;

        }

        public void DownloadDocument(string strRequest, string strPostData, string strDownloadFile, string strContentType = "")
        {
            try
            {
                strFileName = strDownloadFile;
                this.strContentType = strContentType;
                var request = (HttpWebRequest)WebRequest.Create(strRequest);
                request.KeepAlive = true;
                request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0");
                request.Headers.Add("Upgrade-Insecure-Requests", @"1");
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                request.Referer = strRequest;
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9");
                request.Headers.Set(HttpRequestHeader.Cookie, @"ASP.NET_SessionId=" + strSessionID);
                request.Method = "POST";
                request.ProtocolVersion = HttpVersion.Version10;
                request.ServicePoint.Expect100Continue = false;
                byte[] byteArray = Encoding.UTF8.GetBytes(strPostData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                request.BeginGetResponse(new AsyncCallback(PlayResponeAsync), request);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void PlayResponeAsync(IAsyncResult asyncResult)
        {
            HttpWebRequest webRequest = (HttpWebRequest)asyncResult.AsyncState;
            byte[] b = null;

            using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(asyncResult))
            {
                string strData = ReadResponse(webResponse);
                if (strContentType != "" && webResponse.ContentType != strContentType)
                {
                    return;
                }
                else
                {

                    FileStream fileStream = File.OpenWrite(strFileName);
                    byte[] buffer = new byte[1024];
                    using (Stream input = webResponse.GetResponseStream())
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

            }

        }

        #endregion
    }
}
