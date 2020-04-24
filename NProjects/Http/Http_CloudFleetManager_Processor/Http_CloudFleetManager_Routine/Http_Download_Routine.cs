using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Windows.Forms;

namespace Http_CloudFleetManager_Routine
{
    public class Http_Download_Routine :LeSCommon.LeSCommon
    {
        string sAuditMesage = "", MailTextFilePath = "", DocType = "", VRNO = "", PDF_Path = "", errStr = "",CompanyName="";
        int _domainIndex = -1;
        string[] Domains;
        RichTextBox _txtData = new RichTextBox();

        public void LoadAppsettings()
        {
            try
            {
                MailTextFilePath = ConfigurationManager.AppSettings["MAILTEXTFILE"].Trim();
                AuditPath = ConfigurationManager.AppSettings["AUDIT_PATH"].Trim();
                if (ConfigurationManager.AppSettings["SCREENSHOT_PATH"].Trim() != "")
                    PrintScreenPath = ConfigurationManager.AppSettings["SCREENSHOT_PATH"].Trim();
                this.PDF_Path = ConfigurationManager.AppSettings["PDF_PATH"].Trim();
                if (PrintScreenPath == "")
                {
                    PrintScreenPath = AppDomain.CurrentDomain.BaseDirectory + "ScreenShots";
                }
                if (AuditPath == "")
                {
                    AuditPath = AppDomain.CurrentDomain.BaseDirectory + "AuditLog";
                }
                if (!Directory.Exists(PrintScreenPath))
                {
                    Directory.CreateDirectory(PrintScreenPath);
                }
                if (!Directory.Exists(AuditPath))
                {
                    Directory.CreateDirectory(AuditPath);
                }
                if (!Directory.Exists(this.PDF_Path))
                {
                    Directory.CreateDirectory(this.PDF_Path);
                }
                this.Domains = ConfigurationManager.AppSettings["DOMAIN"].Split('|');
               if(!Directory.Exists(MailTextFilePath + "\\Error"))Directory.CreateDirectory(MailTextFilePath + "\\Error");
               if (!Directory.Exists(MailTextFilePath + "\\Backup")) Directory.CreateDirectory(MailTextFilePath + "\\Backup");
            }
            catch (Exception e)
            {
                sAuditMesage = "Exception in LoadAppsettings: " + e.GetBaseException().ToString();
                LogText = sAuditMesage;
              //  CreateAuditFile("", "CFM_HTTP",VRNO, "Error", sAuditMesage, BuyerCode, SupplierCode, AuditPath);
            }
        }

        #region RFQ
        public void Read_MailTextFiles()
        {
            string filename = "";
            try
            {
             //   LogText = "";
               DirectoryInfo _dir = new DirectoryInfo(MailTextFilePath);
                if (_dir.GetFiles().Length > 0)
                {
                    FileInfo[] _Files = _dir.GetFiles();
                    foreach (FileInfo _MtmlFile in _Files)
                    {
                       filename = _MtmlFile.FullName;
                       URL = GetURL(filename);
                       if (URL != "")
                       {

                           LogText = "Processing " + this.DocType + " file ";
                           try
                           {
                               DownloadPDFs(filename);
                           }
                           catch (Exception e)
                           {
                               //WriteErrorLog_With_Screenshot("Exception while processing file : " + e.GetBaseException().Message.ToString(), filename,CompanyName);
                               WriteErrorLog_With_Screenshot("Unable to process file due to " + e.GetBaseException().Message.ToString(), filename, CompanyName, "LeS-1004:");
                               if (File.Exists(MailTextFilePath + "\\Error\\" + Path.GetFileName(filename))) File.Delete(MailTextFilePath + "\\Error\\" + Path.GetFileName(filename));
                               File.Move(filename, MailTextFilePath + "\\Error\\" + Path.GetFileName(filename));
                           }
                       }
                       else
                       {
                           LogText = "URL not found for navigation in file: " + Path.GetFileName(filename);
                           if (CompanyName == "") CompanyName = "CFM";
                           //CreateAuditFile(Path.GetFileName(filename), CompanyName+"_HTTP", VRNO, "Error", "URL not found for navigation in file: " + Path.GetFileName(filename), BuyerCode, SupplierCode, AuditPath);
                           CreateAuditFile(Path.GetFileName(filename), CompanyName + "_HTTP", VRNO, "Error", "LeS-1001:Unable to find URL in file " + Path.GetFileName(filename), BuyerCode, SupplierCode, AuditPath);
                           if (File.Exists(MailTextFilePath + "\\Error\\" + Path.GetFileName(filename))) File.Delete(MailTextFilePath + "\\Error\\" + Path.GetFileName(filename));
                           // File.Copy(filename, PrintScreenPath+"\\" + Path.GetFileName(filename));
                           File.Move(filename, MailTextFilePath + "\\Error\\" + Path.GetFileName(filename));
                       }
                    }
                }
                else LogText = "No file found.";
            }
            catch (Exception e)
            {
                //WriteErrorLog_With_Screenshot("Exception while processing file : " + e.GetBaseException().Message.ToString(), filename,CompanyName);
                WriteErrorLog_With_Screenshot("Unable to process file due to  " + e.GetBaseException().Message.ToString(), filename, CompanyName, "LeS-1004:");
                if (File.Exists(MailTextFilePath + "\\Error\\" + Path.GetFileName(filename))) File.Delete(MailTextFilePath + "\\Error\\" + Path.GetFileName(filename));
                File.Move(filename, MailTextFilePath + "\\Error\\" + Path.GetFileName(filename));
            }
        }

        public void SetGUIDs(string URL, string doctype)
        {
            using (StreamWriter sw = new StreamWriter(@AppDomain.CurrentDomain.BaseDirectory + doctype +"_"+BuyerCode+ "_Downloaded.txt", true))
            {
                sw.WriteLine(URL);
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
        }

        public List<string> GetProcessedItems(eActions eAction, string doctype)
        {
            string sDoneFile = "";
            List<string> slProcessedItems = new List<string>();
            switch (eAction)
            {
                case eActions.RFQ: sDoneFile = AppDomain.CurrentDomain.BaseDirectory + doctype + "_" + BuyerCode + "_Downloaded.txt"; break;
                case eActions.PO: sDoneFile = AppDomain.CurrentDomain.BaseDirectory + doctype + "_" + BuyerCode + "_Downloaded.txt"; break;
                default: break;
            }
            if (File.Exists(sDoneFile))
            {
                string[] _Items = File.ReadAllLines(sDoneFile);
                slProcessedItems.AddRange(_Items.ToList());
            }
            return slProcessedItems;
        }

      private string GetURL(string emlFile)
        {
            string _url = "",_matchText="";
            
            try
            {
                LogText = "";
                LogText = "Reading file " + Path.GetFileName(emlFile);
                _txtData.Text = File.ReadAllText(emlFile);

              foreach (string _domain in Domains)
                {
                   
                    if (_txtData.Text.Contains(_domain))
                    {
                        _domainIndex = Array.IndexOf(Domains, _domain);
                        if (_txtData.Text.Contains("purchase/orders"))
                        {
                            this.DocType = "PO";
                            _matchText="/private/exports/purchase/orders";
                        }
                        else if (_txtData.Text.Contains("purchase/inquiries"))
                        {
                            this.DocType = "RFQ";
                            _matchText = "/private/exports/purchase/inquiries";
                        }
                        else throw new Exception("Invalid Doctype.");

                        if (_matchText != "")
                        {
                            try
                            {
                                if (this.DocType == "PO")
                                {
                                    if (_txtData.Lines[0].ToLower().Contains("order"))
                                    {
                                        VRNO = Convert.ToString(_txtData.Lines[0].ToLower().Split(new string[] { "order" }, StringSplitOptions.None)[1]).Trim();
                                        VRNO = VRNO.Split(' ')[0];
                                    }
                                }
                                else if (this.DocType == "RFQ")
                                {
                                    if (_txtData.Lines[0].ToLower().Contains("request for quotation") || _txtData.Lines[0].ToLower().Contains("inquiry"))
                                    {
                                        VRNO = Convert.ToString(_txtData.Lines[0].ToLower().Split(new string[] { "inquiry - " }, StringSplitOptions.None)[1]).Replace("#","").Trim();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogText = "Unable to get VRNo for doctype " + this.DocType +" for "+emlFile;
                            }

                            for (int i = 0; i < _txtData.Lines.Length; i++)
                            {
                                string line = _txtData.Lines[i];
                              
                                if (line.Contains(_domain+_matchText))
                                {
                                    //get URL
                                    int startIndex = line.IndexOf(_domain+_matchText);
                                    if (line.Length > startIndex)
                                    {
                                        _url = line.Substring(startIndex);
                                    }
                                    else _url = line.Trim();

                                    int endIndex = _url.IndexOf(">");
                                    if (endIndex != -1)
                                    {
                                        if (line.Length > endIndex)
                                        {
                                            _url = _url.Substring(0, endIndex);
                                        }
                                        else _url = line.Trim();
                                    }
                                    else _url = _url.Trim();
                                    break;
                                }
                            }
                        }
                        else throw new Exception("Invalid Doctype.");
                    }
                }
                _url = _url.Trim().Replace("&amp;", "&").Trim().TrimStart('<').TrimStart('"').TrimEnd('>').TrimEnd('"').Trim();
                if (_url.Contains(' ')) _url = _url.Split(' ')[0].Trim('.');
            }
            catch (Exception ex)
            {
                LogText = "Exception while getting URL : " + ex.GetBaseException().ToString();
            }
            return _url;
        }

       #endregion

        #region Order
      public void DownloadPDFs(string FileName)
      {
          try
          {
              if (URL != "")
              {
                  eActions eAction = new eActions();
                  if (this.DocType == "RFQ") eAction = eActions.RFQ;
                  else if (this.DocType == "PO") eAction = eActions.PO;
                  GetAppSettings();
                  List<string> slProcessedItem = GetProcessedItems(eAction, this.DocType);

                  if (!slProcessedItem.Contains(URL))
                  {
                      string Filename = Path.GetFileNameWithoutExtension(FileName) + "_" + VRNO.Replace("\\", "_").Replace("/", "_") + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";

                      if (DownloadPO(URL, PDF_Path + "\\" + Filename))
                      {
                          LogText = "'" + Filename + "' downloaded successfully.";
                          if (CompanyName == "") CompanyName = "CFM";
                          CreateAuditFile(Filename, CompanyName + "_HTTP_" + this.DocType, VRNO, "Downloaded", Filename + " downloaded successfully.", BuyerCode.Trim(), SupplierCode, AuditPath);
                          if (File.Exists(Path.GetDirectoryName(FileName) + "\\Backup\\" + Path.GetFileName(FileName))) File.Delete(Path.GetDirectoryName(FileName) + "\\Backup\\" + Path.GetFileName(FileName));
                          File.Move(FileName, Path.GetDirectoryName(FileName) + "\\Backup\\" + Path.GetFileName(FileName));
                          SetGUIDs(URL, this.DocType);
                      }
                      else
                      {
                          string eFile = PrintScreenPath + "\\CFM_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";

                          if (!errStr.Contains("The remote server returned an error"))
                          {
                              LogText = "Unable to download file for " + Path.GetFileName(FileName);
                              if (!PrintScreen(eFile)) eFile = "";
                              if (File.Exists(eFile))
                                  if (CompanyName == "") CompanyName = "CFM";
                              //CreateAuditFile(eFile, CompanyName + "_HTTP_" + this.DocType, VRNO, "Error", "Unable to download file for " + Path.GetFileName(FileName) + ".", BuyerCode, SupplierCode, AuditPath);
                              CreateAuditFile(eFile, CompanyName + "_HTTP_" + this.DocType, VRNO, "Error", "LeS-1004:Unable to process file " + Path.GetFileName(FileName), BuyerCode, SupplierCode, AuditPath);
                          }
                          else
                          {
                              if (CompanyName == "") CompanyName = "CFM";
                              //CreateAuditFile(Path.GetFileName(FileName), CompanyName + "_HTTP_" + this.DocType, VRNO, "Error", errStr, BuyerCode.Trim(), SupplierCode, AuditPath);
                              CreateAuditFile(Path.GetFileName(FileName), CompanyName + "_HTTP_" + this.DocType, VRNO, "Error", "LeS-1004:Unable to process file due to "+errStr, BuyerCode.Trim(), SupplierCode, AuditPath);
                          }
                          if (File.Exists(Path.GetDirectoryName(FileName) + "\\Error\\" + Path.GetFileName(FileName))) File.Delete(Path.GetDirectoryName(FileName) + "\\Error\\" + Path.GetFileName(FileName));
                          File.Move(FileName, Path.GetDirectoryName(FileName) + "\\Error\\" + Path.GetFileName(FileName));
                      }
                  }
                  else
                  {
                      LogText = this.DocType + " for url '" + URL + "' already processed.";
                      File.Move(FileName, Path.GetDirectoryName(FileName) + "\\Error\\" + Path.GetFileName(FileName));
                  }
              }
              else
              {
                  LogText = "URL not found for navigation in file: " + Path.GetFileName(FileName) + ",Invalid file.";
                  if (CompanyName == "") CompanyName = "CFM";
                  //CreateAuditFile(Path.GetFileName(FileName), CompanyName+"_HTTP",VRNO, "Error", "URL not found for navigation in file: " + Path.GetFileName(FileName) + ",Invalid file.", BuyerCode.Trim(), SupplierCode, AuditPath);
                  CreateAuditFile(Path.GetFileName(FileName), CompanyName + "_HTTP", VRNO, "Error", "LeS-1001:Unable to find URL in file " + Path.GetFileName(FileName), BuyerCode.Trim(), SupplierCode, AuditPath);
                  if (File.Exists(MailTextFilePath + "\\Error\\" + Path.GetFileName(FileName))) File.Delete(MailTextFilePath + "\\Error\\" + Path.GetFileName(FileName));
                  File.Move(FileName, MailTextFilePath + "\\Error\\" + Path.GetFileName(FileName));
              }
          }
          catch (Exception e)
          {
              LogText = "Exception while processing " + this.DocType + " : " + e.GetBaseException().ToString();
            //  WriteErrorLog_With_Screenshot("Exception while processing " + this.DocType + " : " + e.GetBaseException().ToString(), FileName);
          }
      }

        public bool DownloadPO(string RequestURL, string DownloadFileName, string ContentType = "Application/pdf")
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
                errStr = e.GetBaseException().Message.ToString();
                LogText = "Exception while downloading "+this.DocType+": " + e.GetBaseException().Message.ToString();
                _result = false;
            }
            return _result;
        }

        public void GetAppSettings()
        {
            //if (this.DocType == "RFQ")
            //{
                //BuyerCode = ConfigurationManager.AppSettings["BUYER_RFQ_CODE"].Trim().Split('|')[_domainIndex];
            //}
            //else if (this.DocType == "PO")
            //{
            //    BuyerCode = ConfigurationManager.AppSettings["BUYER_PO_CODE"].Trim().Split('|')[_domainIndex];
            //}
            BuyerCode = ConfigurationManager.AppSettings["BUYER_CODE"].Trim().Split('|')[_domainIndex];
            SupplierCode=ConfigurationManager.AppSettings["SUPPLIER_CODE"].Trim().Split('|')[_domainIndex];
            CompanyName = ConfigurationManager.AppSettings["COMPANY_NAME"].Trim().Split('|')[_domainIndex];
        }

        #endregion

        public override bool PrintScreen(string sFileName)
        {
            if (!Directory.Exists(Path.GetDirectoryName(sFileName) + "\\Temp")) Directory.CreateDirectory(Path.GetDirectoryName(sFileName) + "\\Temp");
            sFileName = Path.GetDirectoryName(sFileName) + "\\Temp\\" + Path.GetFileName(sFileName);
            if (base.PrintScreen(sFileName))
            {
                MoveFiles(sFileName, PrintScreenPath + "\\" + Path.GetFileName(sFileName));
                return (File.Exists(PrintScreenPath + "\\" + Path.GetFileName(sFileName)));
            }
            else return false;
        }

        public void WriteErrorLog_With_Screenshot(string AuditMsg, string Filename, string CompanyName, string ErrorNo)
        {

            LogText = AuditMsg;
            if (!AuditMsg.Contains("(404) Not Found"))
            {
                string eFile = PrintScreenPath + "\\CFM_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
                if (!PrintScreen(eFile)) eFile = "";
                {
                    if (CompanyName == "") CompanyName = "CFM";
                    CreateAuditFile(eFile, CompanyName + "_HTTP_" + this.DocType, VRNO, "Error", ErrorNo + AuditMsg, BuyerCode, SupplierCode, AuditPath);
                }
            }
            else
            {
                if (CompanyName == "") CompanyName = "CFM";
                CreateAuditFile(Path.GetFileName(Filename), CompanyName + "_HTTP_" + this.DocType, VRNO, "Error", ErrorNo + AuditMsg, BuyerCode, SupplierCode, AuditPath);
            }
        }


        //public void WriteErrorLog_With_Screenshot(string AuditMsg, string Filename,string CompanyName)
        //{

        //    LogText = AuditMsg;
        //    if (!AuditMsg.Contains("(404) Not Found"))
        //    {
        //        string eFile = PrintScreenPath + "\\CFM_" + this.DocType + "Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
        //        if (!PrintScreen(eFile)) eFile = "";
        //        {
        //            if (CompanyName == "") CompanyName = "CFM";
        //                CreateAuditFile(eFile, CompanyName+"_HTTP_" + this.DocType,VRNO, "Error", AuditMsg, BuyerCode, SupplierCode, AuditPath);
        //        }
        //    }
        //    else
        //    {
        //        if (CompanyName == "") CompanyName = "CFM";
        //            CreateAuditFile(Path.GetFileName(Filename),CompanyName+ "_HTTP_" + this.DocType,VRNO, "Error", AuditMsg,BuyerCode, SupplierCode, AuditPath);
        //    }
        //}

        //public void MoveFileToError(string MTML_QuoteFile, string message, string DocType)
        //{
        //    string eFile = PrintScreenPath + "Asiatic" + DocType + "_Error_" + strUCRefNo.Replace('/', '_') + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + SupplierCode + ".png";
        //    if (!PrintScreen(eFile)) eFile = "";
        //    LogText = message;
        //    if (File.Exists(MTML_QuoteFile))
        //    {
        //        if (File.Exists(QuotePath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile))) File.Delete(QuotePath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
        //        File.Move(MTML_QuoteFile, QuotePath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile));
        //        if (File.Exists(QuotePath + "\\Error\\" + Path.GetFileName(MTML_QuoteFile)))
        //            CreateAuditFile(eFile, "Asiatic_HTTP_" + DocType, QuotePath, "Error", message, BuyerCode, SupplierCode, AuditPath);
        //    }
        //}
    }

    public class Item
    {
        public string OrderItemGuid { get; set; }
        public string ItemNumber { get; set; }
        public object PositionSum { get; set; }
        public object Quantity { get; set; }
        public double VarianceQuantity { get; set; }
        public string Unit { get; set; }
        public string VarianceUnit { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string VarianceName { get; set; }
        public bool IsAvailable { get; set; }
        public bool HasVariance { get; set; }
        public string UnitCode { get; set; }
        public string CatalogName { get; set; }
        public int LineNumber { get; set; }
        public object DeliveryLeadTime { get; set; }
        public string DeliveryLeadTimeUnit { get; set; }
        public object SupplierQuoteRemark { get; set; }
        public object CostTypeCode { get; set; }
        public bool IsAddedBySupplier { get; set; }
        public string InquiryItemGuid { get; set; }
    }

    public class DeliveryInCoTerm
    {
        public string IDCode { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
    }

    public class RequestedInquiryTerms
    {
        public int ID { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentTermText { get; set; }
        public DeliveryInCoTerm DeliveryInCoTerm { get; set; }
        public string DeliveryInCoTermCode { get; set; }
        public int PaymentTargetDays { get; set; }
        public object DeliveryLeadTime { get; set; }
        public string DeliveryLeadTimeUnit { get; set; }
        public object GeneralTermsConditions { get; set; }
    }

    public class DeliveryInCoTerm2
    {
        public string IDCode { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
    }

    public class OfferedInquiryTerms
    {
        public int ID { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentTermText { get; set; }
        public DeliveryInCoTerm2 DeliveryInCoTerm { get; set; }
        public string DeliveryInCoTermCode { get; set; }
        public int PaymentTargetDays { get; set; }
        public object DeliveryLeadTime { get; set; }
        public string DeliveryLeadTimeUnit { get; set; }
        public object GeneralTermsConditions { get; set; }
    }

    public class OfferedPaymentTerms
    {
        public int ID { get; set; }
        public object TypeIDCode { get; set; }
        public object TypeName { get; set; }
        public object Text { get; set; }
        public bool IsDefault { get; set; }
        public int PaymentTargetDays { get; set; }
        public object DisplayName { get; set; }
    }

    public class CostTypeCode
    {
        public string IDCode { get; set; }
        public string Name { get; set; }
    }

    public class Result
    {
        public string InquiryGuid { get; set; }
        public object QuotedPriceTotal { get; set; }
        public object QuotedDiscountPercentageTotal { get; set; }
        public List<Item> Items { get; set; }
        public string InquiryNumber { get; set; }
        public DateTime InquirySentDate { get; set; }
        public object OfferValidUntil { get; set; }
        public object DeliveryLeadTime { get; set; }
        public string DeliveryLeadTimeUnit { get; set; }
        public string Remarks { get; set; }
        public RequestedInquiryTerms RequestedInquiryTerms { get; set; }
        public OfferedInquiryTerms OfferedInquiryTerms { get; set; }
        public object QuoteReceivedDate { get; set; }
        public string StatusCode { get; set; }
        public object OfferReferenceNumber { get; set; }
        public string ResponsibleUserName { get; set; }
        public string DeliveryTo { get; set; }
        public string VesselName { get; set; }
        public string BillingAddress { get; set; }
        public object SupplierRemark { get; set; }
        public string SparePartQualityLevel { get; set; }
        public OfferedPaymentTerms OfferedPaymentTerms { get; set; }
        public bool CanUserWrite { get; set; }
        public DateTime ReplyRequestedUntil { get; set; }
        public object DeliveryAddress { get; set; }
        public bool IsOrdered { get; set; }
        public object RequestedDeliveryDate { get; set; }
        public string RequisitionGuid { get; set; }
        public List<object> Attachments { get; set; }
        public object OfferAttachmentGuid { get; set; }
        public string BillingCompany { get; set; }
        public List<CostTypeCode> CostTypeCodes { get; set; }
        public bool UseQuotedPriceTotal { get; set; }
        public object DeliveryAgentID { get; set; }
        public string DeliveryAgentText { get; set; }
        public object DeliveryAgentContactPersonGuid { get; set; }
        public bool IsQuoteDownloadAvailable { get; set; }
        public object SparePartData { get; set; }
        public List<object> AdditionalTerms { get; set; }
        public object DeliveryETA { get; set; }
        public object DeliveryETD { get; set; }
        public bool IsDirectToVessel { get; set; }
        public bool IsRejected { get; set; }
        public bool IsCancelled { get; set; }
    }

    public class RootObject
    {
        public object ErrorCode { get; set; }
        public bool InputError { get; set; }
        public object ResultMessage { get; set; }
        public bool Success { get; set; }
        public Result Result { get; set; }
    }
}
