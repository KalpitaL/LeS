using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Configuration;
using Aspose.Cells;
using System.Linq.Expressions;
using System.IO.Compression;
using System.Text;
using Dal = MetroLesMonitor.Dal;
using MetroLesMonitor.Bll;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Cells = Aspose.Cells;
using System.Drawing;
using System.Web.Security;

namespace MetroLesMonitor
{
    public class SupplierRoutines
    {
        FindOptions _option = new FindOptions();
        ServiceCallRoutines _WebServiceCall = new ServiceCallRoutines();
        public DataSetCompressor _compressor = new DataSetCompressor();
        string eInvoiceURL = ConfigurationManager.AppSettings["EINVOICE_WEB_SERVICE"];

        public SupplierRoutines()
        {
            License _license = new License();
            _license.SetLicense("Aspose.Total.lic");

            _option.CaseSensitive = true;
            _option.LookAtType = LookAtType.EntireContent;
            _option.SeachOrderByRows = false;
        }

        public String GetRandamString(long input)
        {
            string CharList = "A1BC2DE3FG4HI5JK6LM7NO8PQ9RS0TUVWXYZ";
            if (input < 0) throw new ArgumentOutOfRangeException("input", input, "input cannot be negative");
            char[] clistarr = CharList.ToCharArray();
            var result = new Stack<char>();
            while (input != 0)
            {
                result.Push(clistarr[input % 36]);
                input /= 36;
            }
            return new string(result.ToArray());
        }

        public int DoLogin(string EmailID, string Password)
        {
            int result = 0;
            try
            {
                result = SmExternalUsers.DoLogin(EmailID, Password);
            }
            catch (Exception ex)
            {
                result = -1;
            }
            return result;
        }

        public int GetNextKey(string KeyField, string TableName, Dal.DataAccess DataAccess)
        {
            int KeyID = 0;
            Dal.DataAccess _dataAccess = null;
            if (DataAccess == null) { _dataAccess = new Dal.DataAccess(); } else { _dataAccess = DataAccess; }
            try
            {
                _dataAccess.CreateSQLCommand("SELECT ISNULL(MAX(" + KeyField + "),0) + 1 FROM " + TableName);

                DataSet ds = _dataAccess.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && Convert.ToString(ds.Tables[0].Rows[0][0]) != "")
                {
                    KeyID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                }
            }
            catch { }
            finally
            {
                if (DataAccess == null)
                {
                    _dataAccess._Dispose();
                }
            }
            return KeyID;
        }

        public int GetNextKey(string KeyField, string TableName, string ConnectionString)
        {
            int KeyID = 0;
            Dal.DataAccess _dataAccess = new Dal.DataAccess(ConnectionString);
            try
            {
                _dataAccess.CreateSQLCommand("SELECT ISNULL(MAX(" + KeyField.Trim() + "),0) + 1 FROM " + TableName.Trim());

                DataSet ds = _dataAccess.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && Convert.ToString(ds.Tables[0].Rows[0][0]) != "")
                {
                    KeyID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                }
            }
            catch { }
            finally
            {
                _dataAccess._Dispose();
            }
            return KeyID;
        }

        public System.Data.DataSet GetErrorLog()
        {
            System.Data.DataSet ds = null;
            SmAuditlog _audit = new SmAuditlog();
            ds = _audit.SM_AUDITLOG_Select_ErrorLogs();
            return ds;
        }

        public System.Data.DataSet GetErrorLog(DateTime Fromdate, DateTime ToDate, string ERR_STATUS)
        {
            System.Data.DataSet ds = null;
            SmAuditlog _audit = new SmAuditlog();
            ds = _audit.SM_AUDITLOG_Select_ErrorLogs(Fromdate, ToDate, ERR_STATUS);
            return ds;
        }

        public System.Data.DataSet GetLogTypeList()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SmAuditlog _log = new SmAuditlog();
                ds = _log.GetLogTypes();
            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public System.Data.DataSet GetDocTypeList()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SmQuotationsVendor _Doctype = new SmQuotationsVendor();
                ds = _Doctype.Select_SM_QUOTATIONS_VENDOR_By_Distinct_DOC_TYPE();
            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public string UpdateExportMarker(string QUOTATIONID, string DOC_TYPE, int ExportMarker)
        {
            string _res = "";
            try
            {
                if (!string.IsNullOrEmpty(QUOTATIONID) && !string.IsNullOrEmpty(DOC_TYPE))
                {
                    SmQuotationsVendor _Doctype = new SmQuotationsVendor();
                    _res = _Doctype.UpdateExportMarker(QUOTATIONID, DOC_TYPE, ExportMarker);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _res;
        }

        public System.Data.DataSet GeModulesList()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SmAuditlog _log = new SmAuditlog();
                ds = _log.GetModuleName();
            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public Dictionary<string, string> GetAuditDetails(int LogID)
        {
            Dictionary<string, string> lstAudit = new Dictionary<string, string>();
            try
            {
                SmAuditlog _log = SmAuditlog.Load(LogID);
                if (_log != null)
                {
                    lstAudit.Add("MODULE_NAME", convert.ToString(_log.Modulename).Trim());
                    lstAudit.Add("LOG_TYPE", convert.ToString(_log.Logtype).Trim());
                    lstAudit.Add("KEY_REF1", convert.ToString(_log.Keyref1).Trim());
                    lstAudit.Add("KEY_REF2", convert.ToString(_log.Keyref2).Trim());
                    lstAudit.Add("FILE_NAME", convert.ToString(_log.Filename).Trim());
                    lstAudit.Add("AUDIT_VALUE", convert.ToString(_log.Auditvalue).Trim());
                    lstAudit.Add("BUYER_ID", convert.ToString(_log.BuyerId).Trim());
                    lstAudit.Add("SUPPLIER_ID", convert.ToString(_log.SupplierId).Trim());
                }
            }
            catch
            {
                throw;
            }
            return lstAudit;
        }

        public string GetValideSupplierPath(string cModule, string cBuyerID, string cSupplierID, string eSupplierConfig, string filename)
        {
            string cPath = ""; string[] slConfig = { "", "", "" };
            string cTable = "", cAddrType = "", cColumn = "";
            try
            {
                slConfig = eSupplierConfig.Split('|');
                if (slConfig.Length > 0) cTable = slConfig[0];
                if (slConfig.Length > 1) cAddrType = slConfig[1];
                if (slConfig.Length > 2) cColumn = slConfig[2];

                if (cTable.ToUpper() == "SM_ADDRESS")
                {
                    SmAddress _addrConfig = new SmAddress();
                    if (cAddrType.ToUpper() == "BUYER") _addrConfig.Addressid = Convert.ToInt32(cBuyerID);
                    else if (cAddrType.ToUpper() == "SUPPLIER") _addrConfig.Addressid = Convert.ToInt32(cSupplierID);
                    _addrConfig.Load();

                    if (cColumn.ToUpper() == "INBOX") cPath = convert.ToString(_addrConfig.AddrInbox);
                    if (cColumn.ToUpper() == "OUTBOX") cPath = convert.ToString(_addrConfig.AddrOutbox);
                }
                else
                {
                    SmBuyerSupplierLink _BuyerSupplier = new SmBuyerSupplierLink();
                    _BuyerSupplier.BuyerAddress = SmAddress.Load(Convert.ToInt32(cBuyerID));
                    _BuyerSupplier.VendorAddress = SmAddress.Load(Convert.ToInt32(cSupplierID));
                    _BuyerSupplier.Load_byBuyerSupplierID();

                    if (cColumn.ToUpper() == "INBOX") cPath = convert.ToString(_BuyerSupplier.ImportPath).Trim();
                    if (cColumn.ToUpper() == "OUTBOX") cPath = convert.ToString(_BuyerSupplier.ExportPath).Trim();
                }
            }
            catch (Exception ex)
            {
                cPath = "";
                SetAuditLog("LeSMonitor", "Unable to find the target path, Error-" + ex.Message, "Error", cBuyerID + "-" + cSupplierID, filename, cBuyerID, cSupplierID);
            }
            return cPath;
        }

        public System.Data.DataTable GetServicesLog()
        {
            DataTable dt = new DataTable();

            try
            {
                dt.Columns.Add("SrNo", typeof(int));
                dt.Columns.Add("SeriveName", typeof(string));
                dt.Columns.Add("LastRun", typeof(string));
                dt.Columns.Add("NextRun", typeof(string));
                dt.Columns.Add("ExPath", typeof(string));

                string Services = convert.ToString(ConfigurationManager.AppSettings["SERVICE_LOG"]).Trim();
                if (Convert.ToString(Services).Trim() != "")
                {
                    string[] _services = Services.Split(',');

                    if (_services.Length > 0)
                    {
                        for (int i = 0; i < _services.Length; i++)
                        {
                            string LastRun = "", NextRun = "";
                            string[] _serviceData = _services[i].Trim().Split('|');

                            DataRow dr = dt.NewRow();
                            dr["SrNo"] = (i + 1);
                            dr["SeriveName"] = _serviceData[0].Trim();

                            DirectoryInfo _servicePath = new DirectoryInfo(_serviceData[1].Trim());
                            if (_servicePath.Exists && _servicePath.GetFiles(_serviceData[0].Trim() + '*').Length > 0)
                            {
                                FileInfo file = _servicePath.GetFiles(_serviceData[0].Trim() + '*').OrderByDescending(d => d.LastWriteTimeUtc).First();

                                LastRun = Convert.ToString(file.LastWriteTime);
                                int Interval = convert.ToInt(_serviceData[2].Trim());
                                if (Interval == 0) Interval = 10;
                                NextRun = Convert.ToString(file.LastWriteTime.AddMinutes(Interval));
                            }
                            else
                            {
                                LastRun = "";
                                NextRun = "";
                            }

                            if (_serviceData.Length >= 4)
                            {
                                dr["ExPath"] = _serviceData[3];
                            }
                            else
                            {
                                dr["ExPath"] = "";
                            }

                            dr["LastRun"] = LastRun;
                            dr["NextRun"] = NextRun;
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { }

            return dt;
        }

        public System.Data.DataTable GetSchedulerLog()
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add("SrNo", typeof(int));
                dt.Columns.Add("SchedulerName", typeof(string));
                dt.Columns.Add("LastRun", typeof(string));
                dt.Columns.Add("NextRun", typeof(string));


                string Schedulers = convert.ToString(ConfigurationManager.AppSettings["SCHEDULER_LOG"]).Trim();
                if (Convert.ToString(Schedulers).Trim() != "")
                {
                    string[] _schedulers = Schedulers.Split(',');
                    if (_schedulers.Length > 0)
                    {
                        for (int i = 0; i < _schedulers.Length; i++)
                        {
                            string LastRun = "", NextRun = "";
                            string[] _schedulerData = _schedulers[i].Trim().Split('|');

                            DataRow dr = dt.NewRow();
                            dr["SrNo"] = (i + 1);
                            dr["SchedulerName"] = _schedulerData[0].Trim();

                            DirectoryInfo _schedulerPath = new DirectoryInfo(_schedulerData[1].Trim());
                            if (_schedulerPath.Exists && _schedulerPath.GetFiles(_schedulerData[0].Trim() + '*').Length > 0)
                            {
                                FileInfo file = _schedulerPath.GetFiles(_schedulerData[0].Trim() + '*').OrderByDescending(d => d.LastWriteTimeUtc).First();

                                LastRun = Convert.ToString(file.LastWriteTime);
                                int Interval = convert.ToInt(_schedulerData[2]);
                                if (Interval == 0) Interval = 10;
                                NextRun = Convert.ToString(file.LastWriteTime.AddMinutes(Interval));
                            }
                            else
                            {
                                LastRun = "";
                                NextRun = "";
                            }
                            dr["LastRun"] = LastRun;
                            dr["NextRun"] = NextRun;
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { }
            return dt;
        }

        #region Block IP

        public void BlockIP(string log)
        {
            try
            {
                string _path = "";
                if (string.IsNullOrWhiteSpace(_path)) _path = HttpContext.Current.Server.MapPath("~/BlockIP");
                SetLog("Block IP Path : " + _path);
                SetAuditLog("LesMonitor", log, "Update", "", "", "", "");
                if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
                string _logFile = _path + "\\Log_BlockIP.txt";
                using (StreamWriter sw = new StreamWriter(_logFile, true))
                {
                    log = DateTime.Now.ToString("dd-MM-yy HH:mm:ss") + " : " + log;
                    sw.WriteLine(log);
                    sw.Flush();
                    sw.Dispose();
                }
            }
            catch (Exception ex)
            { }
        }


        #endregion

        #region /* Overview */

        public System.Data.DataSet GetDocsCount_ThisYear(string AddrType)
        {
            return SmAddress.GetOverview_ThisYear(AddrType);
        }

        public System.Data.DataSet GetDocsCount_ThisWeek(string AddrType)
        {
            return SmAddress.GetOverview_ThisWeek(AddrType);
        }

        public System.Data.DataSet GetDocsCount_LastWeek(string AddrType)
        {
            return SmAddress.GetOverview_LastWeek(AddrType);
        }

        public System.Data.DataSet GetDocsCount_ByMonth(string AddrType, int Month, int Year)
        {
            return SmAddress.GetOverview_ByMonth(AddrType, Month, Year);
        }

        public System.Data.DataSet GetDocsCount_All(string AddrType)
        {
            return SmAddress.GetOverview_All(AddrType);
        }

        #region Added by Kalpita on 04/01/2018 
        public string GetDocsCount_ThisWeek_AddressId(string AddressId, string AddrType)
        {
            string _result = "";
            DataSet ds = SmAddress.GetOverview_ThisWeek_By_AddressId(AddressId, AddrType);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                _result = Convert.ToString(ds.Tables[0].Rows[0]["RFQ_COUNT"]) + "|" + Convert.ToString(ds.Tables[0].Rows[0]["QUOTE_COUNT"]) + "|" + Convert.ToString(ds.Tables[0].Rows[0]["PO_COUNT"]) + "|" + Convert.ToString(ds.Tables[0].Rows[0]["POC_COUNT"]);
            }
            else { _result = "0|0|0|0"; }
            return _result;
        }

        public string GetDocsCount_LastWeek_AddressId(string AddressId, string AddrType)
        {
            string _result = "";
            DataSet ds = SmAddress.GetOverview_LastWeek_By_AddressId(AddressId, AddrType);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                _result = Convert.ToString(ds.Tables[0].Rows[0]["RFQ_COUNT"]) + "|" + Convert.ToString(ds.Tables[0].Rows[0]["QUOTE_COUNT"]) + "|" + Convert.ToString(ds.Tables[0].Rows[0]["PO_COUNT"]) + "|" + Convert.ToString(ds.Tables[0].Rows[0]["POC_COUNT"]);
            }
            else { _result = "0|0|0|0"; }
            return _result;
        }

        public string GetDocsCount_ThisMonth_AddressId(string AddressId, string AddrType)
        {
            string _result = "";
            DataSet ds = SmAddress.GetOverview_ThisMonth_By_AddressId(AddressId, AddrType, DateTime.Now.Month, DateTime.Now.Year);
             if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
             {
                 _result = Convert.ToString(ds.Tables[0].Rows[0]["RFQ_COUNT"]) + "|" + Convert.ToString(ds.Tables[0].Rows[0]["QUOTE_COUNT"]) + "|" + Convert.ToString(ds.Tables[0].Rows[0]["PO_COUNT"]) + "|" + Convert.ToString(ds.Tables[0].Rows[0]["POC_COUNT"]);
             }
             else { _result = "0|0|0|0"; }
             return _result;
        }

        public string GetDocsCount_LastMonth_AddressId(string AddressId, string AddrType)
        {
            string _result = "";
            DataSet ds = SmAddress.GetOverview_ThisMonth_By_AddressId(AddressId, AddrType, DateTime.Now.AddMonths(-1).Month, DateTime.Now.AddMonths(-1).Year);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                _result = Convert.ToString(ds.Tables[0].Rows[0]["RFQ_COUNT"]) + "|" + Convert.ToString(ds.Tables[0].Rows[0]["QUOTE_COUNT"]) + "|" + Convert.ToString(ds.Tables[0].Rows[0]["PO_COUNT"]) + "|" + Convert.ToString(ds.Tables[0].Rows[0]["POC_COUNT"]);
            }
            else { _result = "0|0|0|0"; }
            return _result;
        }

        public string GetDocsCount_ThisYear_AddressId(string AddressId, string AddrType)
        {
            string _result = "";
            DataSet ds = SmAddress.GetOverview_ThisYear_By_AddressId(AddressId, AddrType);
             if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
             {
                 _result = Convert.ToString(ds.Tables[0].Rows[0]["RFQ_COUNT"]) + "|" + Convert.ToString(ds.Tables[0].Rows[0]["QUOTE_COUNT"]) + "|" + Convert.ToString(ds.Tables[0].Rows[0]["PO_COUNT"]) + "|" + Convert.ToString(ds.Tables[0].Rows[0]["POC_COUNT"]);
             }
             else { _result = "0|0|0|0"; }
             return _result;
        }

        #endregion

        #region Added by Kalpita on 13/06/2019 (linked)
        public DataSet GetLinked_DocsCount_ThisWeek_AddressId(string AddressId, string AddrType)
        {
            return SmAddress.GetLinkedOverview_ThisWeek_By_AddressId(AddressId, AddrType);
        }

        public DataSet GetLinked_DocsCount_LastWeek_AddressId(string AddressId, string AddrType)
        {
            return SmAddress.GetLinkedOverview_LastWeek_By_AddressId(AddressId, AddrType);
        }

        public DataSet GetLinked_DocsCount_ThisMonth_AddressId(string AddressId, string AddrType)
        {
            return SmAddress.GetLinkedOverview_ThisMonth_By_AddressId(AddressId, AddrType, DateTime.Now.Month, DateTime.Now.Year);
        }

        public DataSet GetLinked_DocsCount_LastMonth_AddressId(string AddressId, string AddrType)
        {
            return SmAddress.GetLinkedOverview_ThisMonth_By_AddressId(AddressId, AddrType, DateTime.Now.AddMonths(-1).Month, DateTime.Now.AddMonths(-1).Year);
        }
        public DataSet GetLinked_DocsCount_ThisYear_AddressId(string AddressId, string AddrType)
        {            
            return SmAddress.GetLinkedOverview_ThisYear_By_AddressId(AddressId, AddrType);
        }

        public System.Data.DataSet GetLinked_DocsCount_All(string AddressId, string AddrType)
        {
            return SmAddress.GetLinked_Overview_All(AddressId,AddrType);
        }
        #endregion


        #endregion

        #region /* Address Details */
        public System.Data.DataSet GetAllBuyers()
        {
            return SmAddress.GetAllBuyers();
        }

        public System.Data.DataSet GetAllSuppliers()
        {
            return SmAddress.GetAllSuppliers();
        }

        public System.Data.DataSet Get_Supplier_Specific_Buyers(int SupplierID)
        {
            return SmAddress.Select_Buyers_By_Supplier(SupplierID);
        }

        public bool CheckDuplicateAddrCode(string AddrCode, int AddressID)
        {
            bool result = false;
            try
            {
                SmAddressCollection _addrCollection = SmAddress.GetAll();
                foreach (SmAddress _address in _addrCollection)
                {
                    if (convert.ToString(_address.AddrCode).Trim().ToUpper() == AddrCode.Trim().ToUpper() && _address.Addressid != AddressID)
                    {
                        result = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            { }
            return result;
        }

        public void SaveAddress(int AddressID, string AddrCode, string AddrName, string ContactPerson, string AddrEmail, string AddrCountry, string AddrInbox, string AddrOutbox, string AddrType, string WebLink, string HostServer)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "";
            try
            {
                SmAddress _address = new SmAddress();
                if (AddressID > 0)
                {
                    _address = SmAddress.Load(AddressID);
                }

                _address.AddrCode = AddrCode.Trim().ToUpper();
                _address.AddrName = AddrName.Trim();
                _address.ContactPerson = ContactPerson.Trim();
                _address.AddrEmail = AddrEmail.Trim();
                _address.AddrCountry = AddrCountry.Trim();
                _address.AddrInbox = AddrInbox.Trim();
                _address.AddrOutbox = AddrOutbox.Trim();
                _address.WebLink = WebLink.Trim();

                _dataAccess.BeginTransaction();

                if (_address.Addressid > 0)
                {
                    _address.Update(_dataAccess);
                    AuditValue = AddrType + " (" + AddrCode + ")' details updated by [" + HostServer + "].";
                }
                else
                {
                    _address.Addressid = GetNextKey("ADDRESSID", "SM_ADDRESS", _dataAccess);
                    _address.Active = 1;
                    _address.AddrType = AddrType.Trim();
                    _address.Esupplier = 1;
                    _address.Einvoice = 0;
                    _address.Epurchase = 0;
                    _address.CreatedDate = DateTime.Now;
                    _address.AddrCurrencyid = 0;
                    _address.XmlAddrNo = 0;

                    _address.Insert(_dataAccess);

                    AuditValue = "New " + AddrType + " (" + AddrCode + ") inserted by [" + HostServer + "].";
                }

                SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "", _dataAccess);
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void SaveAddress(int AddressID, string AddrCode, string AddrName, string ContactPerson, string AddrEmail, string AddrCountry, string AddrCountryCode, string AddrInbox, string AddrOutbox, string AddrType, string WebLink, string HostServer)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "";
            try
            {
                SmAddress _address = new SmAddress();
                if (AddressID > 0)
                {
                    _address = SmAddress.Load(AddressID);
                }

                _address.AddrCode = AddrCode.Trim().ToUpper();
                _address.AddrName = AddrName.Trim();
                _address.ContactPerson = ContactPerson.Trim();
                _address.AddrEmail = AddrEmail.Trim();
                _address.AddrCountry = AddrCountry.Trim();
                _address.AddrCountryCode = AddrCountryCode.Trim();
                _address.AddrInbox = AddrInbox.Trim();
                _address.AddrOutbox = AddrOutbox.Trim();
                _address.WebLink = WebLink.Trim();

                _dataAccess.BeginTransaction();

                if (_address.Addressid > 0)
                {
                    _address.Update(_dataAccess);
                    AuditValue = AddrType + " (" + AddrCode + ")' details updated by [" + HostServer + "].";
                }
                else
                {
                    _address.Addressid = GetNextKey("ADDRESSID", "SM_ADDRESS",_dataAccess);
                    _address.Active = 1;
                    _address.AddrType = AddrType.Trim();
                    _address.Esupplier = 1;
                    _address.Einvoice = 0;
                    _address.Epurchase = 0;
                    _address.CreatedDate = DateTime.Now;
                    _address.AddrCurrencyid = 0;
                    _address.XmlAddrNo = 0;

                    _address.Insert(_dataAccess);

                    AuditValue = "New " + AddrType + " (" + AddrCode + ") inserted by [" + HostServer + "].";
                }

                SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "", _dataAccess);
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void DeleteAddress(int AddressID, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "", AddrCode = "", AddrType = "";
            try
            {
                SmAddress _address = SmAddress.Load(AddressID);
                AddrCode = convert.ToString(_address.AddrCode).Trim();
                AddrType = convert.ToString(_address.AddrType).Trim();

                _dataAccess.CreateConnection();
                _dataAccess.BeginTransaction();

                if (_address != null)
                {
                    _address.Delete(_dataAccess);

                    AuditValue = AddrType + " (" + AddrCode + ") deleted by '[" + UserHostAddress + "].";
                    SetAuditLog("LesMonitor", AuditValue, "Deleted", "", "", "", "", _dataAccess);
                }
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                if (_dataAccess.CurrentTransaction != null) _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public string GetAddressType(int AddressID)
        {
            string result = "";
            try
            {

                SmAddress _addr = SmAddress.Load(AddressID);
                result = _addr.AddrType;
            }
            catch (Exception ex)
            { }
            return result;
        }

        public string GetAddressName(int AddressID)
        {
            string result = "";
            try
            {
                SmAddress _addr = SmAddress.Load(AddressID);
                result = _addr.AddrName;
            }
            catch (Exception ex)
            { }
            return result;
        }

        public List<string> GetAddressInfo(int AddressID)
        {
            List<string> lst = new List<string>();
            try
            {
                SmAddress _addr = SmAddress.Load(AddressID);
                if (_addr != null)
                {
                    lst.Add(_addr.Addressid.ToString());
                    lst.Add(_addr.AddrCode);
                    lst.Add(_addr.AddrName);
                }
            }
            catch (Exception ex)
            { }
            return lst;
        }

        public string GetNextVendorCode()
        {
            Dal.DataAccess _dataAccess = null;
            try
            {
                string AddressCode = "", NewCode = "";
                string _TypeChar = "V";
                if (ConfigurationManager.AppSettings["SUPPLIER_CODE_PREFIX"] != null)
                {
                    _TypeChar = Convert.ToString(ConfigurationManager.AppSettings["SUPPLIER_CODE_PREFIX"]);
                }
                _dataAccess = new Dal.DataAccess();
                _dataAccess.CreateConnection();
                _dataAccess.CreateSQLCommand("SELECT SUBSTRING(ADDR_CODE," + (_TypeChar.Length + 1) + ",LEN(ADDR_CODE)) AS NEW_CODE FROM SM_ADDRESS WHERE ADDR_TYPE='SUPPLIER' " +
                    " AND ADDR_CODE LIKE '" + _TypeChar + "%' " +
                    " AND ISNUMERIC(SUBSTRING(ADDR_CODE," + (_TypeChar.Length + 1) + ",LEN(ADDR_CODE))) = 1 ORDER BY NEW_CODE ");
                DataSet ds = _dataAccess.ExecuteDataSet();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    NewCode = convert.ToString(ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1]["NEW_CODE"]);
                    NewCode = (convert.ToInt(convert.ToInt(NewCode)) + 1).ToString();
                    AddressCode = _TypeChar + NewCode.ToString();
                }

                return AddressCode.Trim();
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                if (_dataAccess != null)
                {
                    _dataAccess._Dispose();
                }
            }
        }

        public string GetNextBuyerCode()
        {
            Dal.DataAccess _dataAccess = null;
            try
            {
                string AddressCode = "", NewCode = "";
                string _TypeChar = "SM";
                if (ConfigurationManager.AppSettings["BUYER_CODE_PREFIX"] != null)
                {
                    _TypeChar = Convert.ToString(ConfigurationManager.AppSettings["BUYER_CODE_PREFIX"]);
                }
                _dataAccess = new Dal.DataAccess();
                _dataAccess.CreateConnection();
                _dataAccess.CreateSQLCommand("SELECT SUBSTRING(ADDR_CODE," + (_TypeChar.Length + 1) + ",LEN(ADDR_CODE)) AS NEW_CODE FROM SM_ADDRESS WHERE ADDR_TYPE='BUYER' " +
                    " AND ADDR_CODE LIKE '" + _TypeChar + "%' " +
                    " AND ISNUMERIC(SUBSTRING(ADDR_CODE," + (_TypeChar.Length + 1) + ",LEN(ADDR_CODE))) = 1 ORDER BY NEW_CODE ");
                DataSet ds = _dataAccess.ExecuteDataSet();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int MaxCount = ds.Tables[0].Rows.Count;
                    NewCode = convert.ToString(ds.Tables[0].Rows[MaxCount - 1]["NEW_CODE"]);
                    int len = NewCode.Length;
                    NewCode = (convert.ToInt(convert.ToInt(NewCode)) + 1).ToString();

                    if (NewCode.Length < len)
                    {
                        while (NewCode.Length < len)
                        {
                            NewCode = "0" + NewCode.Trim();
                        }
                    }
                    AddressCode = _TypeChar + NewCode.ToString();
                }

                return AddressCode.Trim();
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                if (_dataAccess != null)
                {
                    _dataAccess._Dispose();
                }
            }
        }

        public string CheckLeSAdaptor(string ADDRESSID)
        {
            string Islesconnect = ""; 
            try
            {
                SmAddress _obj = new SmAddress();
                _obj.Addressid = Convert.ToInt32(ADDRESSID);
                _obj.Load();
                Islesconnect = (_obj.Islesconnect != null || _obj.Islesconnect == 1) ? "true" : "false";
            }
            catch (Exception ex)
            {   }
            return Islesconnect;         
        }

        //added by Kalpita  on 16/08/2017
        public Dictionary<string, string> GetAddressDetails(int BuyerID)
        {
            Dictionary<string, string> slAddrdet = new Dictionary<string, string>(); slAddrdet.Clear();
            SmAddress _obj = SmAddress.Load(BuyerID);
            if (_obj != null)
            {
                slAddrdet.Add("ADDR_CODE", Convert.ToString(_obj.AddrCode));
                slAddrdet.Add("ADDR_NAME", Convert.ToString(_obj.AddrName));
                slAddrdet.Add("CONTACT_PERSON", Convert.ToString(_obj.ContactPerson));
                slAddrdet.Add("ADDR_EMAIL", Convert.ToString(_obj.AddrEmail));
                slAddrdet.Add("ADDR_COUNTRY", Convert.ToString(_obj.AddrCountry));
                slAddrdet.Add("WEB_LINK", Convert.ToString(_obj.WebLink));
                slAddrdet.Add("ADDR_INBOX", Convert.ToString(_obj.AddrInbox));
                slAddrdet.Add("ADDR_OUTBOX", Convert.ToString(_obj.AddrOutbox));
                slAddrdet.Add("ADDR_CITY", Convert.ToString(_obj.AddrCity));
                slAddrdet.Add("ADDR_ZIPCODE", Convert.ToString(_obj.AddrZipcode));
            }
            return slAddrdet;
        }

        public string GetBuyerSupplierdetails(string SUPPLIERID, string BUYERID)
        {
            string cBuySuppdet = "", cBuyerName = "", cVendorName = "", cBuyerCode = "", cSupplierCode = "";
            if (!string.IsNullOrEmpty(BUYERID) && Convert.ToInt32(BUYERID) > 0) { SmAddress _byrobj = SmAddress.Load(Convert.ToInt32(BUYERID)); if (_byrobj != null) { cBuyerName = "Buyer :" + _byrobj.AddrName; cBuyerCode = _byrobj.AddrCode; } }
            if (!string.IsNullOrEmpty(SUPPLIERID) && Convert.ToInt32(SUPPLIERID) > 0) { SmAddress _sppobj = SmAddress.Load(Convert.ToInt32(SUPPLIERID)); if (_sppobj != null) { cVendorName = "Supplier :" + _sppobj.AddrName; cSupplierCode = _sppobj.AddrCode; } }
            if (cBuyerName != "" && cVendorName != "") { cBuySuppdet = cBuyerName + "," + cVendorName; }
            return cBuySuppdet + "|" + cBuyerCode + "|" + cSupplierCode;
        }

        public List<string> GetAddress_Search(string ADDR_TYPE, string ADDR_NAME)
        {
            List<string> slAddrname = new List<string>(); slAddrname.Clear();
            DataSet ds =SmAddress.GetAddress_By_Search(ADDR_TYPE, ADDR_NAME);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count;i++)
                {
                    slAddrname.Add(Convert.ToString(ds.Tables[0].Rows[i]["ADDR_NAME"]));
                }
            }
            return slAddrname;
        }

        //added by kalpita on 11/12/2017
        #region commented
        ////public void SaveAddress_New(string AddrType, Dictionary<string, string> slAddrDet, string HostServer)
        ////{
        ////    Dal.DataAccess _dataAccess = new Dal.DataAccess();
        ////    string AuditValue = "";
        ////    try
        ////    {
        ////        SmAddress _address = new SmAddress();
        ////        int AddressID = Convert.ToInt32(slAddrDet["ID"]);
        ////        if (AddressID > 0)
        ////        {
        ////            _address = SmAddress.Load(AddressID);
        ////        }
        ////        _address.AddrCode = (slAddrDet.ContainsKey("ADDR_CODE"))? slAddrDet["ADDR_CODE"].Trim().ToUpper():null;
        ////        _address.AddrName =(slAddrDet.ContainsKey("ADDR_NAME"))? slAddrDet["ADDR_NAME"].Trim():null;
        ////        _address.ContactPerson = (slAddrDet.ContainsKey("CONTACT_PERSON"))?slAddrDet["CONTACT_PERSON"].Trim():null;
        ////        _address.AddrEmail = (slAddrDet.ContainsKey("ADDR_EMAIL")) ? slAddrDet["ADDR_EMAIL"].Trim() : null;
        ////        _address.AddrCountry = (slAddrDet.ContainsKey("ADDR_COUNTRY"))?slAddrDet["ADDR_COUNTRY"].Trim():null;
        ////        _address.AddrInbox = (slAddrDet.ContainsKey("ADDR_INBOX"))?slAddrDet["ADDR_INBOX"].Trim():null;
        ////        _address.AddrOutbox = (slAddrDet.ContainsKey("ADDR_OUTBOX"))?slAddrDet["ADDR_OUTBOX"].Trim():null;
        ////        _address.WebLink = (slAddrDet.ContainsKey("WEBLINK"))?slAddrDet["WEBLINK"].Trim():null;
        ////        _address.AddrCity = (slAddrDet.ContainsKey("ADDR_CITY")) ? slAddrDet["ADDR_CITY"].Trim() : null;
        ////        _address.AddrZipcode = (slAddrDet.ContainsKey("ADDR_ZIPCODE")) ? slAddrDet["ADDR_ZIPCODE"].Trim() : null;

        ////        _dataAccess.BeginTransaction();
        ////        if (_address.Addressid > 0)
        ////        {
        ////            _address.Update(_dataAccess);
        ////            AuditValue = AddrType + " (" + slAddrDet["ADDR_CODE"] + ")' details updated by [" + HostServer + "].";
        ////        }
        ////        else
        ////        {
        ////            _address.Addressid = GetNextKey("ADDRESSID", "SM_ADDRESS");
        ////            _address.Active = 1;
        ////            _address.AddrType = AddrType.Trim();
        ////            _address.Esupplier = 1;
        ////            _address.Einvoice = 0;
        ////            _address.Epurchase = 0;
        ////            _address.CreatedDate = DateTime.Now;
        ////            _address.AddrCurrencyid = 0;
        ////            _address.XmlAddrNo = 0;

        ////            _address.Insert(_dataAccess);

        ////            AuditValue = "New " + AddrType + " (" + slAddrDet["ADDR_CODE"] + ") inserted by [" + HostServer + "].";
        ////        }

        ////        SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "", _dataAccess);

        ////        #region insert into config table
        ////        if (AddressID == 0)
        ////        {
        ////            Dictionary<string, string> slDet = new Dictionary<string, string>(); slDet.Clear();
        ////            slDet.Add("ADDRESSCONFIGID", "0");
        ////            slDet.Add("ADDRESSID", Convert.ToString(_address.Addressid));
        ////            slDet.Add("DEFAULT_FORMAT", slAddrDet["DEF_FORMAT"]);
        ////            if (AddrType.ToUpper() == "BUYER")
        ////            {
        ////                slDet.Add("BYR_EXPORT_PATH", slAddrDet["EXPORT_PATH"]);
        ////                slDet.Add("BYR_IMPORT_PATH", slAddrDet["IMPORT_PATH"]);
        ////                slDet.Add("BYR_SENDER_CODE", slAddrDet["ADDR_CODE"]);
        ////            }
        ////            else if (AddrType.ToUpper() == "SUPPLIER")
        ////            {
        ////                slDet.Add("SUPP_EXPORT_PATH", slAddrDet["EXPORT_PATH"]);
        ////                slDet.Add("SUPP_IMPORT_PATH", slAddrDet["IMPORT_PATH"]);
        ////                slDet.Add("SUPP_RECEIVER_CODE", slAddrDet["ADDR_CODE"]);
        ////            }
        ////            SaveAddressConfig(AddrType, slDet, _dataAccess);
        ////        }
        ////        #endregion

        ////        #region insert into default rules table
        ////        if (AddressID == 0)
        ////        {
                  

        ////         //   SaveDefaultRule(AddrType, slDet, _dataAccess);
        ////        }
        ////        #endregion
        ////        _dataAccess.CommitTransaction();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        _dataAccess.RollbackTransaction();
        ////        throw ex;
        ////    }
        ////    finally
        ////    {
        ////        _dataAccess._Dispose();
        ////    }
        ////}
        #endregion

        #endregion

        #region /* Buyer Supplier Item Details */

        public void SaveItemUOMMapping(Dictionary<string, string> _ItemMap, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "";
            string _oldBuyerItemUom = "", _oldSupplierItemUom = "";
            try
            {
                int ID = Convert.ToInt32(_ItemMap["ITEM_UOM_MAP_ID"].ToString());
                SmItemUomMapping _obj = new SmItemUomMapping();

                if (ID > 0)
                {
                    _obj.ItemUomMapid = ID;
                    _obj.Load();
                    _oldBuyerItemUom = _obj.BuyerItemUom;
                    _oldSupplierItemUom = _obj.SupplierItemUom;
                }
                else
                {

                }
                _obj.BuyerItemUom = _ItemMap["BUYER_ITEM_UOM"].ToString();
                _obj.SupplierItemUom = _ItemMap["SUPPLIER_ITEM_UOM"].ToString();
                _obj.SupplierId = convert.ToInt(_ItemMap["SUPPLIERID"]);
                _obj.BuyerId = convert.ToInt(_ItemMap["BUYERID"]);

                SmAddress _buyer = SmAddress.Load(_obj.BuyerId);
                SmAddress _supplier = SmAddress.Load(_obj.SupplierId);

                _dataAccess.BeginTransaction();

                if (_obj.ItemUomMapid > 0)
                {
                    _obj.Update(_dataAccess);
                    AuditValue = "Item UOM for Buyer Code - Supplier Code (" + convert.ToString(_buyer.AddrCode).Trim() + " ) " + convert.ToString(_supplier.AddrCode).Trim() + ") BuyerUOM  from " + _oldBuyerItemUom + " to " + _obj.BuyerItemUom + " and SupplierUOM from " + _oldSupplierItemUom + " to " + _obj.SupplierItemUom + " - updated by [" + UserHostAddress + "].";
                }
                else
                {
                    _obj.ItemUomMapid = _obj.GetNewItemUOMID();
                    _obj.Insert(_dataAccess);
                    AuditValue = "Item UOM for Buyer Code - Supplier Code (" + convert.ToString(_buyer.AddrCode).Trim() + " - " + convert.ToString(_supplier.AddrCode).Trim() + ") BuyerUOM : " + _obj.BuyerItemUom + " and SupplierUOM : " + _obj.SupplierItemUom + " - added by [" + UserHostAddress + "].";
                }

                SetAuditLog("LesMonitor", AuditValue, "", "", "", "", "", _dataAccess);
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }

        }

        public void DeleteItemUOMMapping(int ItemUOMID, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "";
            try
            {
                SmItemUomMapping _itemUOMRef = SmItemUomMapping.Load(ItemUOMID);
                _dataAccess.BeginTransaction();
                if (_itemUOMRef != null)
                {
                    SmAddress _buyer = SmAddress.Load(_itemUOMRef.BuyerId);
                    SmAddress _supplier = SmAddress.Load(_itemUOMRef.SupplierId);

                    _itemUOMRef.Delete(_dataAccess);

                    AuditValue = "Item Ref for Buyer Code - Supplier Code (" + convert.ToString(_buyer.AddrCode).Trim() + " - " + convert.ToString(_supplier.AddrCode).Trim() + ") BuyerUOM : " + _itemUOMRef.BuyerItemUom + " and SupplierUOM : " + _itemUOMRef.SupplierItemUom + "  - deleted by [" + UserHostAddress + "].";

                    SetAuditLog("LesMonitor", AuditValue, "", "", "", "", "", _dataAccess);
                }
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void SaveItemMapping(Dictionary<string, string> _ItemMap, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "";
            try
            {
                int RefID = Convert.ToInt32(_ItemMap["REFID"].ToString());
                SmBuyerSupplierItemRef _obj = new SmBuyerSupplierItemRef();

                if (RefID > 0)
                {
                    _obj.Refid = RefID;
                    _obj.Load();
                }
                _obj.Reftype = _ItemMap["REFTYPE"].ToString();
                _obj.BuyerRef = _ItemMap["BUYER_REF"].ToString();
                _obj.Comments = _ItemMap["COMMENTS"].ToString();
                _obj.ItemDesc = _ItemMap["ITEM_DESCR"].ToString();
                _obj.SupplierRef = _ItemMap["SUPPLIER_REF"].ToString();
                _obj.SupplierId = convert.ToInt(_ItemMap["SUPPLIERID"]);
                _obj.BuyerId = convert.ToInt(_ItemMap["BUYERID"]);
                _obj.BuyerSuppLinkId = convert.ToInt(_ItemMap["LINKID"]);

                SmBuyerSupplierLink _buyerSuppLink = SmBuyerSupplierLink.Load(_obj.BuyerSuppLinkId);
                SmAddress _buyer = SmAddress.Load(_obj.BuyerId);
                SmAddress _supplier = SmAddress.Load(_obj.SupplierId);

                _dataAccess.BeginTransaction();

                if (_obj.Refid > 0)
                {
                    _obj.Update(_dataAccess);
                    AuditValue = "Item Ref for Buyer Code - Supplier Code (" + convert.ToString(_buyer.AddrCode).Trim() + " - " + convert.ToString(_supplier.AddrCode).Trim() + ") and (" + convert.ToString(_buyerSuppLink.BuyerLinkCode).Trim() + " - " + convert.ToString(_buyerSuppLink.VendorLinkCode).Trim() + ") updated by [" + UserHostAddress + "].";
                }
                else
                {
                    _obj.Insert(_dataAccess);
                    AuditValue = "Item Ref for Buyer Code - Supplier Code (" + convert.ToString(_buyer.AddrCode).Trim() + " - " + convert.ToString(_supplier.AddrCode).Trim() + ") and (" + convert.ToString(_buyerSuppLink.BuyerLinkCode).Trim() + " - " + convert.ToString(_buyerSuppLink.VendorLinkCode).Trim() + ") added by [" + UserHostAddress + "].";
                }

                SetAuditLog("LesMonitor", AuditValue, "", "", "", "", "", _dataAccess);
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void DeleteItemMapping(int RefID, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "";
            try
            {
                SmBuyerSupplierItemRef _itemRef = SmBuyerSupplierItemRef.Load(RefID);
                _dataAccess.BeginTransaction();
                if (_itemRef != null)
                {
                    SmBuyerSupplierLink _buyerSuppLink = SmBuyerSupplierLink.Load(_itemRef.BuyerSuppLinkId);
                    SmAddress _buyer = SmAddress.Load(_itemRef.BuyerId);
                    SmAddress _supplier = SmAddress.Load(_itemRef.SupplierId);

                    _itemRef.Delete(_dataAccess);

                    AuditValue = "Item Ref for Buyer Code - Supplier Code (" + convert.ToString(_buyer.AddrCode).Trim() + " - " + convert.ToString(_supplier.AddrCode).Trim() + ") and (" + convert.ToString(_buyerSuppLink.BuyerLinkCode).Trim() + " - " + convert.ToString(_buyerSuppLink.VendorLinkCode).Trim() + ") deleted by [" + UserHostAddress + "].";

                    SetAuditLog("LesMonitor", AuditValue, "", "", "", "", "", _dataAccess);
                }
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public string UploadItemRefFile(bool bDeleteExisting, int LINKID, int BUYID, int SUPPID, string UserHostAddress, string FILENAME)
        {
            string AuditValue = "";
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                SmBuyerSupplierItemRef _itemRef = new SmBuyerSupplierItemRef();
                _dataAccess.BeginTransaction();

                License _license = new License();
                _license.SetLicense("Aspose.Total.lic");

                Workbook _workbook = new Workbook(FILENAME);
                if (_workbook != null)
                {
                    Worksheet _worksheet = _workbook.Worksheets[0];
                    if (_worksheet != null && _worksheet.Cells.MaxDataRow > -1)
                    {
                        bool isValidFileHeaders = IsValidFileHeaders(_worksheet.Cells.Rows[0]);
                        if (isValidFileHeaders)
                        {
                            if (_worksheet.Cells.MaxDataRow > 0)
                            {
                                #region Validation for Empty Columns and Unmatched Link/Buyer/Supplier
                                string strEmptyDataRowNo = "", strMismatchedLinkBuySuppRowNo = "";
                                for (int i = 1; i < (_worksheet.Cells.MaxDataRow + 1); i++)
                                {
                                    string ItemType = Convert.ToString(_worksheet.Cells.Rows[i][1].Value).Trim();
                                    string BuyerItemRef = Convert.ToString(_worksheet.Cells.Rows[i][2].Value).Trim();
                                    string SupplierItemRef = Convert.ToString(_worksheet.Cells.Rows[i][3].Value).Trim();
                                    string ItemDescription = Convert.ToString(_worksheet.Cells.Rows[i][4].Value).Trim();
                                    string SupplierItemComments = Convert.ToString(_worksheet.Cells.Rows[i][5].Value).Trim();
                                    string BuyerLinkCode = Convert.ToString(_worksheet.Cells.Rows[i][6].Value).Trim();
                                    string SupplierLinkCode = Convert.ToString(_worksheet.Cells.Rows[i][7].Value).Trim();

                                    if (ItemType == "" || BuyerItemRef == "" || SupplierItemRef == "" ||
                                       ItemDescription == "" || BuyerLinkCode == "" || SupplierLinkCode == "")
                                    {
                                        strEmptyDataRowNo += (i + 1) + ",";
                                    }
                                    else
                                    {
                                        SmBuyerSupplierLink _fileBuyerSuppLink = SmBuyerSupplierLink.Load_byBuyerSupplierLinkCode(BuyerLinkCode, SupplierLinkCode);
                                        if (_fileBuyerSuppLink == null || _fileBuyerSuppLink.Linkid != LINKID ||
                                            _fileBuyerSuppLink.BuyerId != BUYID || _fileBuyerSuppLink.SupplierId != SUPPID)
                                        {
                                            strMismatchedLinkBuySuppRowNo += (i + 1) + ",";
                                        }
                                    }
                                }

                                strEmptyDataRowNo = strEmptyDataRowNo.Trim(',');
                                strMismatchedLinkBuySuppRowNo = strMismatchedLinkBuySuppRowNo.Trim(',');

                                if (strEmptyDataRowNo != "" || strMismatchedLinkBuySuppRowNo != "")
                                {
                                    if (strEmptyDataRowNo != "" && strMismatchedLinkBuySuppRowNo != "")
                                    {
                                        throw new Exception("File 'Item Type', 'Buyer Item Ref.', 'Supplier Item Ref.', " +
                                            "'Item Description', 'Buyer Link Code' or 'Supplier Link Code' is INVALID for Row No(s).: " + strEmptyDataRowNo + " \n\n" +
                                            "File 'Buyer Link Code' or 'Supplier Link Code' is MISMATCHED with Selected Link for Row No(s).: " + strMismatchedLinkBuySuppRowNo);
                                    }
                                    else if (strEmptyDataRowNo != "")
                                    {
                                        throw new Exception("File 'Item Type', 'Buyer Item Ref.', 'Supplier Item Ref.', " +
                                            "'Item Description', 'Buyer Link Code' or 'Supplier Link Code' is INVALID for Row No(s).: " + strEmptyDataRowNo);

                                    }
                                    else if (strMismatchedLinkBuySuppRowNo != "")
                                    {
                                        throw new Exception("File 'Buyer Link Code' or 'Supplier Link Code' is MISMATCHED with Selected Link for Row No(s).: " + strMismatchedLinkBuySuppRowNo);
                                    }
                                }
                                #endregion

                                SmBuyerSupplierLink _buyerSuppLink = SmBuyerSupplierLink.Load(LINKID);
                                SmAddress _smBuyAddress = SmAddress.Load(BUYID);
                                SmAddress _smSuppAddress = SmAddress.Load(SUPPID);

                                #region Delete Existing Records
                                if (bDeleteExisting)
                                {
                                    DataSet ds = _itemRef.GetItemMapping(LINKID);
                                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        _itemRef.DeleteByLinkID(LINKID, _dataAccess);

                                        AuditValue = "Item Ref(s) for Buyer Code - Supplier Code (" + convert.ToString(_smBuyAddress.AddrCode).Trim() + " - " + convert.ToString(_smSuppAddress.AddrCode).Trim() + ") and (" + convert.ToString(_buyerSuppLink.BuyerLinkCode).Trim() + " - " + convert.ToString(_buyerSuppLink.VendorLinkCode).Trim() + ") deleted by [" + UserHostAddress + "].";
                                        SetAuditLog("LesMonitor", AuditValue, "", "", "", "", "", _dataAccess);
                                    }
                                }
                                #endregion

                                #region Add New Records from Uploaded File

                                for (int i = 1; i < (_worksheet.Cells.MaxDataRow + 1); i++)
                                {
                                    string ItemType = Convert.ToString(_worksheet.Cells.Rows[i][1].Value).Trim();
                                    string BuyerItemRef = Convert.ToString(_worksheet.Cells.Rows[i][2].Value).Trim();
                                    string SupplierItemRef = Convert.ToString(_worksheet.Cells.Rows[i][3].Value).Trim();
                                    string ItemDescription = Convert.ToString(_worksheet.Cells.Rows[i][4].Value).Trim();
                                    string SupplierItemComments = Convert.ToString(_worksheet.Cells.Rows[i][5].Value).Trim();

                                    SmBuyerSupplierItemRef _BuySuppItemRef = new SmBuyerSupplierItemRef();
                                    _BuySuppItemRef.Load();
                                    _BuySuppItemRef.Reftype = ItemType;
                                    _BuySuppItemRef.BuyerRef = BuyerItemRef;
                                    _BuySuppItemRef.SupplierRef = SupplierItemRef;
                                    _BuySuppItemRef.ItemDesc = ItemDescription;
                                    _BuySuppItemRef.Comments = SupplierItemComments;
                                    _BuySuppItemRef.BuyerId = BUYID;
                                    _BuySuppItemRef.SupplierId = SUPPID;
                                    _BuySuppItemRef.BuyerSuppLinkId = LINKID;
                                    _BuySuppItemRef.Insert(_dataAccess);
                                }

                                AuditValue = "Item Ref(s) for Buyer Code - Supplier Code (" + convert.ToString(_smBuyAddress.AddrCode).Trim() + " - " + convert.ToString(_smSuppAddress.AddrCode).Trim() + ") and (" + convert.ToString(_buyerSuppLink.BuyerLinkCode).Trim() + " - " + convert.ToString(_buyerSuppLink.VendorLinkCode).Trim() + ") added by [" + UserHostAddress + "].";
                                SetAuditLog("LesMonitor", AuditValue, "", "", "", "", "", _dataAccess);
                                #endregion
                                //
                            }
                            else throw new Exception("Record Count is 0.");
                        }
                        else throw new Exception("Please Check if File Headers are Proper.");
                    }
                    else throw new Exception("Please Upload Valid File.");
                }
                else throw new Exception("Please Upload Valid File.");

                _dataAccess.CommitTransaction();

                return "SUCCESS|File '" + Path.GetFileName(FILENAME) + "' uploaded successfully.";
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                return ex.Message;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public string UploadItemUOMFile(bool bDeleteExisting, int BUYID, int SUPPID, string UserHostAddress, string FILENAME)
        {
            string AuditValue = "";
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                SmItemUomMapping _itemUOM = new SmItemUomMapping();
                int newItemUOMID = _itemUOM.GetNewItemUOMID();
                _dataAccess.BeginTransaction();

                License _license = new License();
                _license.SetLicense("Aspose.Total.lic");

                Workbook _workbook = new Workbook(FILENAME);
                if (_workbook != null)
                {
                    Worksheet _worksheet = _workbook.Worksheets[0];
                    if (_worksheet != null && _worksheet.Cells.MaxDataRow > -1)
                    {
                        bool isValidFileHeaders = IsValidFileHeaders_ItemUOM(_worksheet.Cells.Rows[0]);
                        if (isValidFileHeaders)
                        {
                            if (_worksheet.Cells.MaxDataRow > 0)
                            {
                                #region Validation for Empty Columns and Unmatched Buyer/Supplier
                                string strEmptyDataRowNo = "", strMismatchedLinkBuySuppRowNo = "";
                                for (int i = 1; i < (_worksheet.Cells.MaxDataRow + 1); i++)
                                {
                                    string Buyer_Item_UOM = Convert.ToString(_worksheet.Cells.Rows[i][1].Value).Trim();
                                    string Supplier_Item_UOM = Convert.ToString(_worksheet.Cells.Rows[i][2].Value).Trim();
                                    string BuyerLinkCode = Convert.ToString(_worksheet.Cells.Rows[i][3].Value).Trim();
                                    string SupplierLinkCode = Convert.ToString(_worksheet.Cells.Rows[i][4].Value).Trim();

                                    if (Buyer_Item_UOM == "" || Supplier_Item_UOM == "")
                                    {
                                        strEmptyDataRowNo += (i + 1) + ",";
                                    }
                                    else
                                    {
                                        SmBuyerSupplierLink _fileBuyerSuppLink = SmBuyerSupplierLink.Load_byBuyerSupplierLinkCode(BuyerLinkCode, SupplierLinkCode);
                                        if (_fileBuyerSuppLink == null || _fileBuyerSuppLink.BuyerId != BUYID || _fileBuyerSuppLink.SupplierId != SUPPID)
                                        {
                                            strMismatchedLinkBuySuppRowNo += (i + 1) + ",";
                                        }
                                    }
                                }

                                strEmptyDataRowNo = strEmptyDataRowNo.Trim(',');
                                strMismatchedLinkBuySuppRowNo = strMismatchedLinkBuySuppRowNo.Trim(',');

                                if (strEmptyDataRowNo != "" || strMismatchedLinkBuySuppRowNo != "")
                                {
                                    if (strEmptyDataRowNo != "" && strMismatchedLinkBuySuppRowNo != "")
                                    {
                                        throw new Exception("File 'Buyer Item UOM', " +
                                            "'Supplier Item UOM', 'Buyer Link Code' or 'Supplier Link Code' is INVALID for Row No(s).: " + strEmptyDataRowNo + " \n\n" +
                                            "File 'Buyer Link Code' or 'Supplier Link Code' is MISMATCHED with Selected Link for Row No(s).: " + strMismatchedLinkBuySuppRowNo);
                                    }
                                    else if (strEmptyDataRowNo != "")
                                    {
                                        throw new Exception("File 'Buyer Item UOM','Supplier Item UOM', " +
                                            " 'Buyer Link Code' or 'Supplier Link Code' is INVALID for Row No(s).: " + strEmptyDataRowNo);

                                    }
                                    else if (strMismatchedLinkBuySuppRowNo != "")
                                    {
                                        throw new Exception("File 'Buyer Link Code' or 'Supplier Link Code' is MISMATCHED with Selected Link for Row No(s).: " + strMismatchedLinkBuySuppRowNo);
                                    }
                                }
                                #endregion

                                SmAddress _smBuyAddress = SmAddress.Load(BUYID);
                                SmAddress _smSuppAddress = SmAddress.Load(SUPPID);

                                #region Delete Existing Records
                                if (bDeleteExisting)
                                {
                                    DataSet ds = _itemUOM.GetItemUOM_Mapping_By_BuyerID_SupplierID(SUPPID, BUYID);
                                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        _itemUOM.DeleteUOM_MAPPING_ByBuySuppID(SUPPID, BUYID, _dataAccess);

                                        AuditValue = "Item UOM for Buyer Code - Supplier Code (" + convert.ToString(_smBuyAddress.AddrCode).Trim() + " - " + convert.ToString(_smSuppAddress.AddrCode).Trim() + ") deleted by [" + UserHostAddress + "].";
                                        SetAuditLog("LesMonitor", AuditValue, "", "", "", "", "", _dataAccess);
                                    }
                                }
                                #endregion

                                #region Add New Records from Uploaded File

                                for (int i = 1; i < (_worksheet.Cells.MaxDataRow + 1); i++)
                                {
                                    string Buyer_Item_UOM = Convert.ToString(_worksheet.Cells.Rows[i][1].Value).Trim();
                                    string Supplier_Item_UOM = Convert.ToString(_worksheet.Cells.Rows[i][2].Value).Trim();

                                    SmItemUomMapping _BuySuppItemUOM = new SmItemUomMapping();
                                    _BuySuppItemUOM.Load();
                                    _BuySuppItemUOM.ItemUomMapid = newItemUOMID;
                                    _BuySuppItemUOM.BuyerItemUom = Convert.ToString(_worksheet.Cells.Rows[i][1].Value).Trim();
                                    _BuySuppItemUOM.SupplierItemUom = Convert.ToString(_worksheet.Cells.Rows[i][2].Value).Trim();
                                    _BuySuppItemUOM.SupplierId = convert.ToInt(SUPPID);
                                    _BuySuppItemUOM.BuyerId = convert.ToInt(BUYID);
                                    _BuySuppItemUOM.Insert(_dataAccess);
                                    AuditValue = "Item UOM(s) for Buyer Code - Supplier Code (" + convert.ToString(_smBuyAddress.AddrCode).Trim() + " - " + convert.ToString(_smSuppAddress.AddrCode).Trim() + ") BuyerUOM : " + _BuySuppItemUOM.BuyerItemUom + " and SupplierUOM : " + _BuySuppItemUOM.SupplierItemUom + " added by [" + UserHostAddress + "].";
                                    SetAuditLog("LesMonitor", AuditValue, "", "", "", "", "", _dataAccess);
                                    newItemUOMID++;
                                }
                                #endregion
                                //
                            }
                            else throw new Exception("Record Count is 0.");
                        }
                        else throw new Exception("Please Check if File Headers are Proper.");
                    }
                    else throw new Exception("Please Upload Valid File.");
                }
                else throw new Exception("Please Upload Valid File.");

                _dataAccess.CommitTransaction();

                return "SUCCESS|File '" + Path.GetFileName(FILENAME) + "' uploaded successfully.";
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                return ex.Message;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        private bool IsValidFileHeaders(Aspose.Cells.Row row)
        {
            bool isValidFileHeaders = true;
            try
            {
                string headerNo = Convert.ToString(row[0].Value).Trim();
                if (headerNo.Replace("\n", "").Trim() != "No.(Number)") isValidFileHeaders = false;
                else
                {
                    string headerItemType = Convert.ToString(row[1].Value).Trim();
                    if (headerItemType.Replace("\n", "").Trim() != "* Item Type(Char - 50)") isValidFileHeaders = false;
                    else
                    {
                        string headerBuyItemRef = Convert.ToString(row[2].Value).Trim();
                        if (headerBuyItemRef.Replace("\n", "").Trim() != "* Buyer Item Ref.(Char - 50)") isValidFileHeaders = false;
                        else
                        {
                            string headerSuppItemRef = Convert.ToString(row[3].Value).Trim();
                            if (headerSuppItemRef.Replace("\n", "").Trim() != "* Supplier Item Ref.(Char - 50)") isValidFileHeaders = false;
                            else
                            {
                                string headerItemDescr = Convert.ToString(row[4].Value).Trim();
                                if (headerItemDescr.Replace("\n", "").Trim() != "* Item Description(Char - 250)") isValidFileHeaders = false;
                                else
                                {
                                    string headerSuppItemcomments = Convert.ToString(row[5].Value).Trim();
                                    if (headerSuppItemcomments.Replace("\n", "").Trim() != "Supplier Item Comments(Char - 250)") isValidFileHeaders = false;
                                    else
                                    {
                                        string headerBuyAddrCode = Convert.ToString(row[6].Value).Trim();
                                        if (headerBuyAddrCode.Replace("\n", "").Trim() != "* Buyer Link Code(Char - 20)") isValidFileHeaders = false;
                                        else
                                        {
                                            string headerSuppAddrCode = Convert.ToString(row[7].Value).Trim();
                                            if (headerSuppAddrCode.Replace("\n", "").Trim() != "* Supplier Link Code(Char - 20)") isValidFileHeaders = false;
                                            else isValidFileHeaders = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return isValidFileHeaders;
        }

        private bool IsValidFileHeaders_ItemUOM(Aspose.Cells.Row row)
        {
            bool isValidFileHeaders = true;
            try
            {
                string headerNo = Convert.ToString(row[0].Value).Trim();
                if (headerNo.Replace("\n", "").Trim() != "Sr No (Number)") isValidFileHeaders = false;
                else
                {
                    string BuyerItemUOM = Convert.ToString(row[1].Value).Trim();
                    if (BuyerItemUOM.Replace("\n", "").Trim() != "Buyer Item UOM * (Char - 50)") isValidFileHeaders = false;
                    else
                    {
                        string SupplierItemUOM = Convert.ToString(row[2].Value).Trim();
                        if (SupplierItemUOM.Replace("\n", "").Trim() != "Supplier Item UOM * (Char - 50)") isValidFileHeaders = false;
                        else
                        {
                            string BuyerLinkCode = Convert.ToString(row[3].Value).Trim();
                            if (BuyerLinkCode.Replace("\n", "").Trim() != "Buyer Link Code * (Char - 20)") isValidFileHeaders = false;
                            else
                            {
                                string SupplierLinkCode = Convert.ToString(row[4].Value).Trim();
                                if (SupplierLinkCode.Replace("\n", "").Trim() != "Supplier Link Code * (Char - 20)") isValidFileHeaders = false;
                                else
                                {
                                    isValidFileHeaders = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return isValidFileHeaders;
        }

        public System.Data.DataSet FillItemMapGrid(int LinkID)
        {
            SmBuyerSupplierItemRef _obj = new SmBuyerSupplierItemRef();
            System.Data.DataSet _ds = _obj.GetItemMapping(LinkID);
            return _ds;
        }

        public System.Data.DataSet FillItemUOM_MAPGrid(int SupplierID, int BuyerID)
        {
            SmItemUomMapping _obj = new SmItemUomMapping();
            System.Data.DataSet _ds = _obj.GetItemUOM_Mapping_By_BuyerID_SupplierID(SupplierID, BuyerID);
            return _ds;
        }

        #endregion

        #region /* Buyer Supplier Link */
        public System.Data.DataSet GetLinkedSuppliers(int BuyerID)
        {
            System.Data.DataSet _ds = new System.Data.DataSet();
            SmAddress _obj = SmAddress.Load(BuyerID);
            if (_obj != null)
            {

                if (_obj.AddrType.ToLower().Contains("admin")) _ds = SmAddress.GetAllSuppliers();
                else _ds = SmAddress.GetLinkedSuppliers((int)_obj.Addressid);
            }
            return _ds;
        }

        public string GetLinkedSuppliers_Count(int BuyerID)
        {
            DataSet ds = GetLinkedSuppliers(BuyerID);
            return (ds != null && ds.Tables.Count > 0) ? Convert.ToString(ds.Tables[0].Rows.Count) : "0";
        }

        public System.Data.DataSet GetLinkedBuyers(int SupplierID)
        {
            System.Data.DataSet _ds = new System.Data.DataSet();
            SmAddress _obj = SmAddress.Load(SupplierID);
            if (_obj != null)
            {

                if (_obj.AddrType.ToLower().Contains("admin")) _ds = SmAddress.GetAllBuyers();
                else _ds = SmAddress.GetLinkedBuyers((int)_obj.Addressid);
            }
            return _ds;
        }

        public string GetLinkedBuyers_Count(int AddressID)
        {
            return SmAddress.GetLinkedBuyers_Count(AddressID);
        }

        public bool GetLinkBy_Buyer_Vendor_LinkCode(int LinkID, string BuyerLinkCode, string SupplierLinkCode)
        {
            bool _isExist = false;
            SmBuyerSupplierLinkCollection _linkCollection = SmBuyerSupplierLink.GetAll();
            foreach (SmBuyerSupplierLink _link in _linkCollection)
            {
                if (_link.BuyerLinkCode == BuyerLinkCode.Trim() && _link.VendorLinkCode == SupplierLinkCode.Trim() && _link.Linkid != LinkID)
                { _isExist = true; break; }
            }
            return _isExist;
        }

        public void Add_BuyerSupplier_Link(int SupplierID, int BuyerID, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "";
            try
            {
                SmAddress _buyer = SmAddress.Load(BuyerID);
                SmAddress _supplier = SmAddress.Load(SupplierID);
                //DataSet ds_DF_SupplierRule = SmDefaultRules.SM_DEFAULT_RULES_Select_By_AddressID(SupplierID);
                //DataSet ds_DF_BuyerRule = SmDefaultRules.SM_DEFAULT_RULES_Select_By_AddressID(BuyerID);

                //List<string> _SupplierRule_ID = new List<string>();
                //List<string> _BuyerRule_ID = new List<string>();

                _dataAccess.BeginTransaction();
                SmBuyerSupplierLink _link = new SmBuyerSupplierLink();
                _link.BuyerAddress = _buyer;
                _link.BuyerLinkCode = convert.ToString(_buyer.AddrCode).Trim();

                // AUTO FILL BUYER IMPORT PATH, EXPORT PATH, EMAIL & CONTACT
                _link.ImportPath = convert.ToString(_buyer.AddrInbox).Trim();
                _link.ExportPath = convert.ToString(_buyer.AddrOutbox).Trim();
                _link.BuyerEmail = convert.ToString(_buyer.AddrEmail).Trim();
                _link.BuyerContact = convert.ToString(_buyer.ContactPerson).Trim();

                // AUTO FILL SUPPLIER IMPORT PATH, EXPORT PATH, EMAIL & CONTACT
                _link.SuppImportPath = convert.ToString(_supplier.AddrInbox).Trim();
                _link.SuppExportPath = convert.ToString(_supplier.AddrOutbox).Trim();
                _link.SupplierContact = convert.ToString(_supplier.ContactPerson).Trim();
                _link.SupplierEmail = convert.ToString(_supplier.AddrEmail).Trim();

                _link.VendorAddress = _supplier;
                _link.VendorLinkCode = convert.ToString(_supplier.AddrCode).Trim();
                _link.Insert(_dataAccess);

                AuditValue = "New Buyer-Supplier (" + convert.ToString(_buyer.AddrCode) + " - " + convert.ToString(_supplier.AddrCode) + ") link added by [" + UserHostAddress + "].";
                SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", convert.ToString(_buyer.Addressid), convert.ToString(_supplier.Addressid), _dataAccess);
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        //changed by Kalpita on 27/11/2017
        public void Add_BuyerSupplier_Link_config(int SupplierID, int BuyerID, string UserHostAddress)
        {
            MetroLesMonitor.Dal.DataAccess _dataAccess = new MetroLesMonitor.Dal.DataAccess();
            string AuditValue = "";
            try
            {
                SmAddress _buyer = SmAddress.Load(BuyerID);   SmAddress _supplier = SmAddress.Load(SupplierID);
                SmDefaultRulesCollection _byrRuleCollect = null;  SmDefaultRulesCollection _sppRuleCollect = null; SmXlsBuyerLinkCollection _xByrCollect = null; SmPdfBuyerLinkCollection _pByrCollect = null;

                SmAddressConfig _byrConfig = SmAddressConfig.Load_AddressID(BuyerID);
                SmAddressConfig _suppConfig = SmAddressConfig.Load_AddressID(SupplierID);

                if (_byrConfig != null) { _byrRuleCollect = SmDefaultRules.Load_AddressID_Format(BuyerID, _byrConfig.DefaultFormat); }
                if (_suppConfig != null) { _sppRuleCollect = SmDefaultRules.Load_AddressID_Format(SupplierID, _suppConfig.DefaultFormat); }

                _dataAccess.BeginTransaction();
                SmBuyerSupplierLink _link = new SmBuyerSupplierLink();
                _link.BuyerAddress = _buyer;
                _link.BuyerLinkCode = Convert.ToString(_buyer.AddrCode).Trim();

                if (_suppConfig != null || _byrConfig != null)
                {
                    if (_suppConfig != null)
                    {
                        _link.ExportRfq = (_suppConfig.ExportRfq == 1) ? _suppConfig.ExportRfq : 0;
                        _link.ImportQuote = (_suppConfig.ImportQuote == 1) ? _suppConfig.ImportQuote : 0;
                        _link.ExportPo = (_suppConfig.ExportPo == 1) ? _suppConfig.ExportPo : 0;
                        _link.NotifySupplr = (_suppConfig.MailNotify == 1) ? _suppConfig.MailNotify : 0;
                        _link.SuppImportPath = convert.ToString(_suppConfig.ImportPath).Trim();
                        _link.SuppExportPath = convert.ToString(_suppConfig.ExportPath).Trim();
                        _link.SupplierMapping = convert.ToString(_suppConfig.PartyMapping).Trim();
                        _link.DefaultPrice = (_suppConfig.DefaultPrice != null) ? _suppConfig.DefaultPrice : 0;
                        _link.UploadFileType = _suppConfig.UploadFileType;
                        _link.Supp_Web_Service_Url = _suppConfig.SupWebServiceUrl;
                        _link.CcEmail = _suppConfig.CcEmail;
                        _link.SuppReceiverCode = _suppConfig.IdentificationCode;
                        _link.MailSubject = _suppConfig.MailSubject;
                        _link.ImportPoc = (_suppConfig.ImportPoc == 1) ? _suppConfig.ImportPoc : 0;
                    }
                    else
                    {
                        _link.ExportRfq = 0; _link.ImportQuote = 0; _link.ExportPo = 0; _link.NotifySupplr = 0; _link.SuppImportPath = null;
                        _link.SuppExportPath = null; _link.SupplierMapping = null; _link.DefaultPrice = null;
                        _link.UploadFileType = null; _link.Supp_Web_Service_Url = null; _link.CcEmail = null; _link.SuppReceiverCode = null; _link.ImportPoc = null;
                    }
                    if (_byrConfig != null)
                    {
                        _link.NotifyBuyer = (_byrConfig.MailNotify == 1) ? _byrConfig.MailNotify : 0;
                        _link.ImportRfq = (_byrConfig.ImportRfq == 1) ? _byrConfig.ImportRfq : 0;
                        _link.ExportQuote = (_byrConfig.ExportQuote == 1) ? _byrConfig.ExportQuote : 0;
                        _link.ExportRfqAck = (_byrConfig.ExportRfqAck == 1) ? _byrConfig.ExportRfqAck : 0;
                        _link.ImportPo = (_byrConfig.ImportPo == 1) ? _byrConfig.ImportPo : 0;
                        _link.ExportPoAck = (_byrConfig.ExportPoAck == 1) ? _byrConfig.ExportPoAck : 0;
                        _link.ExportPoc = (_byrConfig.ExportPoc == 1) ? _byrConfig.ExportPoc : 0;
                        _link.ImportPath = convert.ToString(_byrConfig.ImportPath).Trim();
                        _link.ExportPath = convert.ToString(_byrConfig.ExportPath).Trim();
                        _link.BuyerMapping = convert.ToString(_byrConfig.PartyMapping).Trim();
                        _link.DefaultPrice = (_byrConfig.DefaultPrice != null) ? _byrConfig.DefaultPrice : 0;
                        _link.UploadFileType = _byrConfig.UploadFileType;
                        _link.Supp_Web_Service_Url = _byrConfig.SupWebServiceUrl;
                        _link.CcEmail = _byrConfig.CcEmail;
                        _link.ByrSenderCode = _byrConfig.IdentificationCode;
                        _link.MailSubject = _byrConfig.MailSubject;
                    }
                    else
                    {
                        _link.NotifyBuyer = 0; _link.ImportRfq = 0; _link.ExportQuote = 0; _link.ExportRfqAck = 0; _link.ImportPo = 0; _link.ExportPoAck = 0;
                        _link.ExportPoc = 0; _link.ImportPath = null; _link.ExportPath = null; _link.BuyerMapping = null; _link.DefaultPrice = null;
                        _link.UploadFileType = null; _link.Supp_Web_Service_Url = null; _link.CcEmail = null; _link.ByrSenderCode = null;
                    }

                   // if (_suppConfig.DefaultPrice != null && _byrConfig.DefaultPrice != null) { _link.DefaultPrice = _suppConfig.DefaultPrice; }
                   // if (_suppConfig.UploadFileType != null && _byrConfig.UploadFileType != null) { _link.UploadFileType = _suppConfig.UploadFileType; }
                   // if (_suppConfig.SupWebServiceUrl != null && _byrConfig.SupWebServiceUrl != null) { _link.Supp_Web_Service_Url = _suppConfig.SupWebServiceUrl; }
                    if (_suppConfig != null && _byrConfig != null) {

                        if (!string.IsNullOrEmpty(_suppConfig.CcEmail) && !string.IsNullOrEmpty(_byrConfig.CcEmail))
                        {                            
                            _link.CcEmail = _byrConfig.CcEmail + ";" + _suppConfig.CcEmail;
                        }
                        if (_suppConfig.MailSubject != null && _byrConfig.MailSubject != null)
                        {
                            _link.MailSubject = _suppConfig.MailSubject;
                        }
                    }

                    #region Commented
                    //if (_suppConfig.ImportRfq == 1 && _byrConfig.ImportRfq == 1) { _link.ImportRfq = _suppConfig.ImportRfq; }
                    //else
                    //{
                    //    _link.ImportRfq = (_suppConfig.ImportRfq == 1) ? _suppConfig.ImportRfq : _byrConfig.ImportRfq;
                    //}
                    //if (_suppConfig.ExportRfq == 1 && _byrConfig.ExportRfq == 1) { _link.ExportRfq = _suppConfig.ExportRfq; }
                    //else
                    //{
                    //    _link.ExportRfq = (_suppConfig.ExportRfq == 1) ? _suppConfig.ExportRfq : _byrConfig.ExportRfq;
                    //}
                    //if (_suppConfig.ExportRfqAck == 1 && _byrConfig.ExportRfqAck == 1) { _link.ExportRfqAck = _suppConfig.ExportRfqAck; }
                    //else
                    //{
                    //    _link.ExportRfqAck = (_suppConfig.ExportRfqAck == 1) ? _suppConfig.ExportRfqAck : _byrConfig.ExportRfqAck;
                    //}
                    //if (_suppConfig.ImportQuote == 1 && _byrConfig.ImportQuote == 1) { _link.ImportQuote = _suppConfig.ImportQuote; }
                    //else
                    //{
                    //    _link.ImportQuote = (_suppConfig.ImportQuote == 1) ? _suppConfig.ImportQuote : _byrConfig.ImportQuote;
                    //}
                    //if (_suppConfig.ExportQuote == 1 && _byrConfig.ExportQuote == 1) { _link.ExportQuote = _suppConfig.ExportQuote; }
                    //else
                    //{
                    //    _link.ExportQuote = (_suppConfig.ExportQuote == 1) ? _suppConfig.ExportQuote : _byrConfig.ExportQuote;
                    //}
                    //if (_suppConfig.ImportPo == 1 && _byrConfig.ImportPo == 1) { _link.ImportPo = _suppConfig.ImportPo; }
                    //else
                    //{
                    //    _link.ImportPo = (_suppConfig.ImportPo == 1) ? _suppConfig.ImportPo : _byrConfig.ImportPo;
                    //}
                    //if (_suppConfig.ExportPo == 1 && _byrConfig.ExportPo == 1) { _link.ExportPo = _suppConfig.ExportPo; }
                    //else
                    //{
                    //    _link.ExportPo = (_suppConfig.ExportPo == 1) ? _suppConfig.ExportPo : _byrConfig.ExportPo;
                    //}
                    //if (_suppConfig.ExportPoAck == 1 && _byrConfig.ExportPoAck == 1) { _link.ExportPoAck = _suppConfig.ExportPoAck; }
                    //else
                    //{
                    //    _link.ExportPoAck = (_suppConfig.ExportPoAck == 1) ? _suppConfig.ExportPoAck : _byrConfig.ExportPoAck;
                    //}
                    //if (_suppConfig.ExportPoc == 1 && _byrConfig.ExportPoc == 1) { _link.ExportPoc = _suppConfig.ExportPoc; }
                    //else
                    //{
                    //    _link.ExportPoc = (_suppConfig.ExportPoc == 1) ? _suppConfig.ExportPoc : _byrConfig.ExportPoc;
                    //}

                    //if (_suppConfig.DefaultPrice != null && _byrConfig.DefaultPrice != null) { _link.DefaultPrice = _suppConfig.DefaultPrice; }
                    //else
                    //{
                    //    _link.DefaultPrice = (_suppConfig.DefaultPrice != null) ? _suppConfig.DefaultPrice : _byrConfig.DefaultPrice;
                    //}
                    //if (_suppConfig.UploadFileType != null && _byrConfig.UploadFileType != null) { _link.UploadFileType = _suppConfig.UploadFileType; }
                    //else
                    //{
                    //    _link.UploadFileType = (_suppConfig.UploadFileType != null) ? _suppConfig.UploadFileType : _byrConfig.UploadFileType;
                    //}
                    //if (_suppConfig.SupWebServiceUrl != null && _byrConfig.SupWebServiceUrl != null) { _link.Supp_Web_Service_Url = _suppConfig.SupWebServiceUrl; }
                    //else
                    //{
                    //    _link.Supp_Web_Service_Url = (_suppConfig.SupWebServiceUrl != null) ? _suppConfig.SupWebServiceUrl : _byrConfig.SupWebServiceUrl;
                    //}
                    //if (_suppConfig.CcEmail != null && _byrConfig.CcEmail != null) { _link.CcEmail = _suppConfig.CcEmail; }
                    //else
                    //{
                    //    _link.CcEmail = (_suppConfig.CcEmail != null) ? _suppConfig.CcEmail : _byrConfig.CcEmail;
                    //}
                    #endregion
                }
                else
                {
                    //address paths
                    _link.ImportPath = convert.ToString(_buyer.AddrInbox).Trim();
                    _link.ExportPath = convert.ToString(_buyer.AddrOutbox).Trim();
                    _link.SuppImportPath = convert.ToString(_supplier.AddrInbox).Trim();
                    _link.SuppExportPath = convert.ToString(_supplier.AddrOutbox).Trim();
                }

                _link.BuyerEmail = convert.ToString(_buyer.AddrEmail).Trim();
                _link.BuyerContact = convert.ToString(_buyer.ContactPerson).Trim();
                _link.SupplierContact = convert.ToString(_supplier.ContactPerson).Trim();
                _link.SupplierEmail = convert.ToString(_supplier.AddrEmail).Trim();
                _link.VendorAddress = _supplier;
                _link.VendorLinkCode = convert.ToString(_supplier.AddrCode).Trim();
                _link.IsActive = 0;
                _link.Insert(_dataAccess);

                AuditValue = "New Buyer-Supplier (" + convert.ToString(_buyer.AddrCode) + " - " + convert.ToString(_supplier.AddrCode) + ") link added by [" + UserHostAddress + "].";
                SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", convert.ToString(_buyer.Addressid), convert.ToString(_supplier.Addressid), _dataAccess);

                #region Add Buyer-Supplier Default rules

                if (_byrRuleCollect!=null && _byrRuleCollect.Count > 0)
                {
                    foreach (SmDefaultRules _bObj in _byrRuleCollect)
                    {
                        SmBuyerSupplierLinkRule _blnkRuleobj = new SmBuyerSupplierLinkRule();
                        _blnkRuleobj.LinkRuleId = 0;
                        _blnkRuleobj.Linkid = _link.Linkid;
                        _blnkRuleobj.InheritRule = 0;
                        _blnkRuleobj.Ruleid = _bObj.RuleId;
                        _blnkRuleobj.RuleValue = _bObj.RuleValue;
                        _blnkRuleobj.UdateDate = DateTime.Now;
                        _blnkRuleobj.Insert(_dataAccess);
                    }
                }
                if (_sppRuleCollect != null && _sppRuleCollect.Count > 0)
                {
                    foreach (SmDefaultRules _sObj in _sppRuleCollect)
                    {
                        SmBuyerSupplierLinkRule _slnkRuleobj = new SmBuyerSupplierLinkRule();
                        _slnkRuleobj.UdateDate = DateTime.Now;
                        _slnkRuleobj.Linkid = _link.Linkid;
                        SmBuyerSupplierLinkRule _Lnkrule = SmBuyerSupplierLinkRule.LoadByLinkRule_(convert.ToInt(_link.Linkid), convert.ToInt(_sObj.RuleId), _dataAccess);
                        if (_Lnkrule == null)
                        {                            
                            _slnkRuleobj.InheritRule = 0;
                            _slnkRuleobj.Ruleid = _sObj.RuleId;
                            _slnkRuleobj.RuleValue = _sObj.RuleValue;
                            _slnkRuleobj.Insert(_dataAccess);
                        }
                        else
                        {
                            _slnkRuleobj.InheritRule = 1;
                            _slnkRuleobj.RuleValue = _sObj.RuleValue;
                            _slnkRuleobj.Update(_dataAccess);
                        }
                    }
                }


                #endregion

                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        //added by kalpita on 06/02/2018
        public void Add_BuyerSupplier_Link_Wizard(int SupplierID, int BuyerID, Dictionary<string,string> slgridDetails,string UserHostAddress)
        {
            MetroLesMonitor.Dal.DataAccess _dataAccess = new MetroLesMonitor.Dal.DataAccess();
            string AuditValue = "";
            try
            {
                SmAddress _buyer = SmAddress.Load(BuyerID); SmAddress _supplier = SmAddress.Load(SupplierID);
                SmDefaultRulesCollection _byrRuleCollect = null; SmDefaultRulesCollection _sppRuleCollect = null; 
                SmAddressConfig _byrConfig = SmAddressConfig.Load_AddressID(BuyerID);
                SmAddressConfig _suppConfig = SmAddressConfig.Load_AddressID(SupplierID);
                foreach (string key in slgridDetails.Keys)
                {
                    switch(key)
                    {
                      
                        case "BUYER_DRULE": _byrRuleCollect =(!string.IsNullOrEmpty(slgridDetails[key]))? SmDefaultRules.Get_DefaultRules_By_DefID(slgridDetails[key]):null;
                            break;
                        case "SUPPLIER_DRULE": _sppRuleCollect = (!string.IsNullOrEmpty(slgridDetails[key])) ? SmDefaultRules.Get_DefaultRules_By_DefID(slgridDetails[key]) : null; 
                            break;
                    }
                }
                _dataAccess.BeginTransaction();
                SmBuyerSupplierLink _link = new SmBuyerSupplierLink();
                _link.BuyerAddress = _buyer;
                _link.BuyerLinkCode = Convert.ToString(_buyer.AddrCode).Trim();

                if (_suppConfig != null || _byrConfig != null)
                {
                    if (_suppConfig != null)
                    {
                        _link.ExportRfq = (_suppConfig.ExportRfq == 1) ? _suppConfig.ExportRfq : 0;
                        _link.ImportQuote = (_suppConfig.ImportQuote == 1) ? _suppConfig.ImportQuote : 0;
                        _link.ExportPo = (_suppConfig.ExportPo == 1) ? _suppConfig.ExportPo : 0;
                        _link.NotifySupplr = (_suppConfig.MailNotify == 1) ? _suppConfig.MailNotify : 0;
                        _link.SuppImportPath = convert.ToString(_suppConfig.ImportPath).Trim();
                        _link.SuppExportPath = convert.ToString(_suppConfig.ExportPath).Trim();
                        _link.SupplierMapping = convert.ToString(_suppConfig.PartyMapping).Trim();
                        _link.DefaultPrice = (_suppConfig.DefaultPrice != null) ? _suppConfig.DefaultPrice : 0;
                        _link.UploadFileType = _suppConfig.UploadFileType;
                        _link.Supp_Web_Service_Url = _suppConfig.SupWebServiceUrl;
                        _link.CcEmail = _suppConfig.CcEmail;
                        _link.SuppReceiverCode = _suppConfig.IdentificationCode;
                        _link.MailSubject = _suppConfig.MailSubject;
                        _link.ImportPoc = (_suppConfig.ImportPoc == 1) ? _suppConfig.ImportPoc : 0;
                    }
                    else
                    {
                        _link.ExportRfq = 0; _link.ImportQuote = 0; _link.ExportPo = 0; _link.NotifySupplr = 0; _link.SuppImportPath = null;
                        _link.SuppExportPath = null; _link.SupplierMapping = null; _link.DefaultPrice = null;
                        _link.UploadFileType = null; _link.Supp_Web_Service_Url = null; _link.CcEmail = null; _link.SuppReceiverCode = null; _link.ImportPoc = null;
                    }
                    if (_byrConfig != null)
                    {
                        _link.NotifyBuyer = (_byrConfig.MailNotify == 1) ? _byrConfig.MailNotify : 0;
                        _link.ImportRfq = (_byrConfig.ImportRfq == 1) ? _byrConfig.ImportRfq : 0;
                        _link.ExportQuote = (_byrConfig.ExportQuote == 1) ? _byrConfig.ExportQuote : 0;
                        _link.ExportRfqAck = (_byrConfig.ExportRfqAck == 1) ? _byrConfig.ExportRfqAck : 0;
                        _link.ImportPo = (_byrConfig.ImportPo == 1) ? _byrConfig.ImportPo : 0;
                        _link.ExportPoAck = (_byrConfig.ExportPoAck == 1) ? _byrConfig.ExportPoAck : 0;
                        _link.ExportPoc = (_byrConfig.ExportPoc == 1) ? _byrConfig.ExportPoc : 0;
                        _link.ImportPath = convert.ToString(_byrConfig.ImportPath).Trim();
                        _link.ExportPath = convert.ToString(_byrConfig.ExportPath).Trim();
                        _link.BuyerMapping = convert.ToString(_byrConfig.PartyMapping).Trim();
                        _link.DefaultPrice = (_byrConfig.DefaultPrice != null) ? _byrConfig.DefaultPrice : 0;
                        _link.UploadFileType = _byrConfig.UploadFileType;
                        _link.Supp_Web_Service_Url = _byrConfig.SupWebServiceUrl;
                        _link.CcEmail = _byrConfig.CcEmail;
                        _link.ByrSenderCode = _byrConfig.IdentificationCode;
                        _link.MailSubject = _byrConfig.MailSubject;
                    }
                    else
                    {
                        _link.NotifyBuyer = 0; _link.ImportRfq = 0; _link.ExportQuote = 0; _link.ExportRfqAck = 0; _link.ImportPo = 0; _link.ExportPoAck = 0;
                        _link.ExportPoc = 0; _link.ImportPath = null; _link.ExportPath = null; _link.BuyerMapping = null; _link.DefaultPrice = null;
                        _link.UploadFileType = null; _link.Supp_Web_Service_Url = null; _link.CcEmail = null; _link.ByrSenderCode = null;
                    }
                    if (_suppConfig != null && _byrConfig != null)
                    {
                        if (!string.IsNullOrEmpty(_suppConfig.CcEmail) && !string.IsNullOrEmpty(_byrConfig.CcEmail))
                        {
                            _link.CcEmail = _byrConfig.CcEmail + ";" + _suppConfig.CcEmail;
                        }
                        if (_suppConfig.MailSubject != null && _byrConfig.MailSubject != null)
                        {
                            _link.MailSubject = _suppConfig.MailSubject;
                        }
                    }
                }
                else
                {
                    //address paths
                    _link.ImportPath = convert.ToString(_buyer.AddrInbox).Trim();
                    _link.ExportPath = convert.ToString(_buyer.AddrOutbox).Trim();
                    _link.SuppImportPath = convert.ToString(_supplier.AddrInbox).Trim();
                    _link.SuppExportPath = convert.ToString(_supplier.AddrOutbox).Trim();
                }

                _link.BuyerEmail = convert.ToString(_buyer.AddrEmail).Trim();
                _link.BuyerContact = convert.ToString(_buyer.ContactPerson).Trim();
                _link.SupplierContact = convert.ToString(_supplier.ContactPerson).Trim();
                _link.SupplierEmail = convert.ToString(_supplier.AddrEmail).Trim();
                _link.VendorAddress = _supplier;
                _link.VendorLinkCode = convert.ToString(_supplier.AddrCode).Trim();
                _link.IsActive = 0;
                _link.Insert(_dataAccess);

                AuditValue = "New Buyer-Supplier (" + convert.ToString(_buyer.AddrCode) + " - " + convert.ToString(_supplier.AddrCode) + ") link added by [" + UserHostAddress + "].";
                SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", convert.ToString(_buyer.Addressid), convert.ToString(_supplier.Addressid), _dataAccess);

                #region Add Buyer-Supplier Default rules

                if (_byrRuleCollect != null && _byrRuleCollect.Count > 0)
                {
                    foreach (SmDefaultRules _bObj in _byrRuleCollect)
                    {
                        SmBuyerSupplierLinkRule _blnkRuleobj = new SmBuyerSupplierLinkRule();
                        _blnkRuleobj.LinkRuleId = 0;
                        _blnkRuleobj.Linkid = _link.Linkid;
                        _blnkRuleobj.InheritRule = 0;
                        _blnkRuleobj.Ruleid = _bObj.RuleId;
                        _blnkRuleobj.RuleValue = _bObj.RuleValue;
                        _blnkRuleobj.UdateDate = DateTime.Now;
                        _blnkRuleobj.Insert(_dataAccess);
                    }
                }
                if (_sppRuleCollect != null && _sppRuleCollect.Count > 0)
                {
                    foreach (SmDefaultRules _sObj in _sppRuleCollect)
                    {
                        SmBuyerSupplierLinkRule _slnkRuleobj = new SmBuyerSupplierLinkRule();
                        _slnkRuleobj.UdateDate = DateTime.Now;
                        _slnkRuleobj.Linkid = _link.Linkid;
                        SmBuyerSupplierLinkRule _Lnkrule = SmBuyerSupplierLinkRule.LoadByLinkRule_(convert.ToInt(_link.Linkid), convert.ToInt(_sObj.RuleId), _dataAccess);
                        if (_Lnkrule == null)
                        {
                            _slnkRuleobj.InheritRule = 0;
                            _slnkRuleobj.Ruleid = _sObj.RuleId;
                            _slnkRuleobj.RuleValue = _sObj.RuleValue;
                            _slnkRuleobj.Insert(_dataAccess);
                        }
                        else
                        {
                            _slnkRuleobj.InheritRule = 1;
                            _slnkRuleobj.RuleValue = _sObj.RuleValue;
                            _slnkRuleobj.Update(_dataAccess);
                        }
                    }
                }

                #endregion
             
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public DataSet Get_Buyer_Supplier_Link_By_GroupID(int GroupID)
        {
            DataSet ds = new DataSet();
            ds = SmBuyerSupplierLink.Select_SMV_BUYER_SUPPLIER_LINKs_By_GROUPID(GroupID);
            return ds;
        }

        public DataSet Get_Buyer_Supplier_Link_By_RuleID(int RuleID)
        {
            DataSet ds = new DataSet();
            ds = SmBuyerSupplierLink.Select_SMV_BUYER_SUPPLIER_LINKs_By_RuleId(RuleID);
            return ds;
        }

        public SmBuyerSupplierLink Add_BuyerSupplier_Link(int SupplierID, int BuyerID, int newGroupID, string BuyerFormat, string SuppFormat, string BuyerExportFormat, string SuppExportFormat, Dal.DataAccess _dataAccess, int CopyFromGroupID, bool bCreateNewBuyerSupplierLink, string UserHostAddress)
        {
            string AuditValue = ""; int iLinkId = 0; bool isNewLink = false;
            try
            {
                SmAddress _buyer = SmAddress.Load(BuyerID);
                SmAddress _supplier = SmAddress.Load(SupplierID);
                SmBuyerSupplierLink _CopyLink = new SmBuyerSupplierLink();
                _CopyLink.Load_byGroup(CopyFromGroupID);

                SmBuyerSupplierLink _link = new SmBuyerSupplierLink();

                if (bCreateNewBuyerSupplierLink)
                {
                    _link.BuyerAddress = _buyer;
                    _link.BuyerLinkCode = convert.ToString(_buyer.AddrCode);
                    _link.ImportRfq = 0;
                    _link.ExportRfq = 0;
                    _link.ExportRfqAck = 0;
                    _link.ImportQuote = 0;
                    _link.ExportQuote = 0;
                    _link.ImportPo = 0;
                    _link.ExportPo = 0;
                    _link.ExportPoAck = 0;
                    _link.ExportPoc = 0;
                    _link.NotifyBuyer = 0;
                    _link.NotifySupplr = 0;
                    _link.DefaultPrice = 0;
                    _link.IsActive = 0;
                    _link.GroupId = newGroupID;
                    _link.BuyerFormat = BuyerFormat;
                    _link.VendorFormat = SuppFormat;
                    _link.BuyerExportFormat = BuyerExportFormat;
                    _link.SupplierExportFormat = SuppExportFormat;

                    _link.VendorAddress = _supplier;
                    _link.VendorLinkCode = _supplier.AddrCode;
                    _link.Insert(_dataAccess);
                    isNewLink = true;

                    iLinkId = convert.ToInt(_link.Linkid);
                    AuditValue = "New Buyer-Supplier (" + _buyer.AddrCode + " - " + _supplier.AddrCode + ") link added by [" + UserHostAddress + "].";
                    SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", convert.ToString(_buyer.Addressid), convert.ToString(_supplier.Addressid), _dataAccess);
                }
                else
                {
                    _link.BuyerAddress = _buyer;
                    _link.VendorAddress = _supplier;
                    _link.BuyerFormat = BuyerFormat;
                    _link.VendorFormat = SuppFormat;
                    _link.Load_byBuyerSupplierFormat();

                    if (convert.ToInt(_link.Linkid) == 0)
                    {
                        _link.BuyerAddress = _buyer;
                        _link.VendorAddress = _supplier;
                        _link.BuyerFormat = "";
                        _link.VendorFormat = "";
                        _link.Load_byBuyerSupplier();
                    }

                    if (_link.Linkid > 0)
                    {
                        string buyerFormat = convert.ToString(_link.BuyerFormat).Trim().ToUpper();
                        if (buyerFormat != "PDF" && buyerFormat != "EXCEL_RFQ")
                        {
                            _link.GroupId = newGroupID;
                            _link.BuyerFormat = BuyerFormat;
                            _link.VendorFormat = SuppFormat;
                            _link.BuyerExportFormat = BuyerExportFormat;
                            _link.Update(_dataAccess);
                        }
                    }
                    iLinkId = convert.ToInt(_link.Linkid);
                }

                if (_CopyLink != null && _link != null && _link.Linkid > 0 && isNewLink)
                {
                    _link.BuyerAddress = _buyer;
                    _link.VendorAddress = _supplier;
                    _link.BuyerLinkCode = _buyer.AddrCode;
                    _link.ImportRfq = _CopyLink.ImportRfq;
                    _link.ExportRfq = _CopyLink.ExportRfq;
                    _link.ExportRfqAck = _CopyLink.ExportRfqAck;
                    _link.ImportQuote = _CopyLink.ImportQuote;
                    _link.ExportQuote = _CopyLink.ExportQuote;
                    _link.ImportPo = _CopyLink.ImportPo;
                    _link.ExportPo = _CopyLink.ExportPo;
                    _link.ExportPoAck = _CopyLink.ExportPoAck;
                    _link.ExportPoc = _CopyLink.ExportPoc;
                    _link.NotifyBuyer = _CopyLink.NotifyBuyer;
                    _link.NotifySupplr = _CopyLink.NotifySupplr;
                    _link.DefaultPrice = _CopyLink.DefaultPrice;
                    _link.ImportPath = _CopyLink.ImportPath;

                    string SourceExpPath = _CopyLink.ExportPath;
                    if (SourceExpPath != null)
                    {
                        if (SourceExpPath.IndexOf(_CopyLink.BuyerAddress.AddrCode) > 0)
                        {
                            SourceExpPath = SourceExpPath.Replace(_CopyLink.BuyerAddress.AddrCode, _link.BuyerAddress.AddrCode);
                        }
                        if (SourceExpPath.IndexOf(_CopyLink.VendorAddress.AddrCode) > 0)
                        {
                            SourceExpPath = SourceExpPath.Replace(_CopyLink.VendorAddress.AddrCode, _link.VendorAddress.AddrCode);
                        }
                    }

                    _link.ExportPath = SourceExpPath;
                    _link.MailSubject = _CopyLink.MailSubject;
                    _link.ReplyEmail = _CopyLink.ReplyEmail;
                    _link.BccEmail = _CopyLink.BccEmail;
                    _link.UploadFileType = _CopyLink.UploadFileType;

                    _link.VendorAddress = _supplier;
                    _link.VendorLinkCode = _supplier.AddrCode;
                }

                return _link;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update_Link_Details(int LinkID, Dictionary<string, string> lstLinkDetails, string UserHostAddress)
        {
            int oldActive = 0, newActive = 0;
            string BuyerLinkCode = "", SupplierLinkCode = "";
            bool IsNewGroup = false, IsGroupChange = false;
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                string _updatedData = "";
                SmBuyerSupplierLink _link = SmBuyerSupplierLink.Load(LinkID);
                if (_link != null)
                {
                    SmAddress _buyer = SmAddress.Load(_link.BuyerAddress.Addressid);
                    SmAddress _supplier = SmAddress.Load(_link.VendorAddress.Addressid);

                    // Check Whether Group is new or not
                    int GroupID = 0;
                    GroupID = convert.ToInt(_link.GroupId);
                    if (GroupID == 0) { IsNewGroup = true; IsGroupChange = true; }
                    else if (convert.ToInt(lstLinkDetails["GROUP_ID"]) > 0 && GroupID != convert.ToInt(lstLinkDetails["GROUP_ID"]))
                    {
                        IsNewGroup = false;
                        IsGroupChange = true;
                        GroupID = convert.ToInt(lstLinkDetails["GROUP_ID"]);
                    }

                    _dataAccess.BeginTransaction();

                    #region // Update Link fields //

                    SetUpdateLog(ref _updatedData, "Byr Code", _buyer.AddrCode, lstLinkDetails["BUYER_CODE"]);
                    _buyer.AddrCode = lstLinkDetails["BUYER_CODE"];
                    _buyer.Update(_dataAccess);

                    SetUpdateLog(ref _updatedData, "Byr Email", _link.BuyerEmail, lstLinkDetails["BUYER_EMAIL"]);
                    _link.BuyerEmail = lstLinkDetails["BUYER_EMAIL"];

                    SetUpdateLog(ref _updatedData, "Supp Email", _link.SupplierEmail, lstLinkDetails["SUPPLIER_EMAIL"]);
                    _link.SupplierEmail = lstLinkDetails["SUPPLIER_EMAIL"];

                    SetUpdateLog(ref _updatedData, "Byr Link Code", _link.BuyerLinkCode, lstLinkDetails["BUYER_LINK_CODE"]);
                    _link.BuyerLinkCode = lstLinkDetails["BUYER_LINK_CODE"];

                    SetUpdateLog(ref _updatedData, "Supp Link Code", _link.VendorLinkCode, lstLinkDetails["VENDOR_LINK_CODE"]);
                    _link.VendorLinkCode = lstLinkDetails["VENDOR_LINK_CODE"];

                    SetUpdateLog(ref _updatedData, "Supp Sender Code", _link.SuppSenderCode, lstLinkDetails["SUPP_SENDER_CODE"]);
                    _link.SuppSenderCode = lstLinkDetails["SUPP_SENDER_CODE"];

                    SetUpdateLog(ref _updatedData, "Supp Receiver Code", _link.SuppReceiverCode, lstLinkDetails["SUPP_RECEIVER_CODE"]);
                    _link.SuppReceiverCode = lstLinkDetails["SUPP_RECEIVER_CODE"];

                    SetUpdateLog(ref _updatedData, "Byr Sender Code", _link.ByrSenderCode, lstLinkDetails["BYR_SENDER_CODE"]);
                    _link.ByrSenderCode = lstLinkDetails["BYR_SENDER_CODE"];

                    SetUpdateLog(ref _updatedData, "Byr Receiver Code", _link.ByrReceiverCode, lstLinkDetails["BYR_RECEIVER_CODE"]);
                    _link.ByrReceiverCode = lstLinkDetails["BYR_RECEIVER_CODE"];

                    SetUpdateLog(ref _updatedData, "Byr Mapping", _link.BuyerMapping, lstLinkDetails["BUYER_MAPPING"]);
                    _link.BuyerMapping = lstLinkDetails["BUYER_MAPPING"];

                    SetUpdateLog(ref _updatedData, "Supp Mapping", _link.SupplierMapping, lstLinkDetails["SUPPLIER_MAPPING"]);
                    _link.SupplierMapping = lstLinkDetails["SUPPLIER_MAPPING"];

                    SetUpdateLog(ref _updatedData, "Byr Contact", _link.BuyerContact, lstLinkDetails["BUYER_CONTACT"]);
                    _link.BuyerContact = lstLinkDetails["BUYER_CONTACT"];

                    SetUpdateLog(ref _updatedData, "Supp Contact", _link.SupplierContact, lstLinkDetails["SUPPLIER_CONTACT"]);
                    _link.SupplierContact = lstLinkDetails["SUPPLIER_CONTACT"];

                    // Added by simmy
                    SetUpdateLog(ref _updatedData, "Upload Filetype", _link.UploadFileType, lstLinkDetails["UPLOAD_FILE_TYPE"]);
                    _link.UploadFileType = lstLinkDetails["UPLOAD_FILE_TYPE"];

                    #region /* Path Settings */
                    SetUpdateLog(ref _updatedData, "Import Path", _link.ImportPath, lstLinkDetails["IMPORT_PATH"]);
                    _link.ImportPath = lstLinkDetails["IMPORT_PATH"];

                    SetUpdateLog(ref _updatedData, "Export Path", _link.ExportPath, lstLinkDetails["EXPORT_PATH"]);
                    _link.ExportPath = lstLinkDetails["EXPORT_PATH"];

                    SetUpdateLog(ref _updatedData, "Supp Import Path", _link.SuppImportPath, lstLinkDetails["SUPP_IMPORT_PATH"]);
                    _link.SuppImportPath = lstLinkDetails["SUPP_IMPORT_PATH"];

                    SetUpdateLog(ref _updatedData, "Supp Export Path", _link.SuppExportPath, lstLinkDetails["SUPP_EXPORT_PATH"]);
                    _link.SuppExportPath = lstLinkDetails["SUPP_EXPORT_PATH"];
                    #endregion

                    #region /* Mail Settings */
                    SetUpdateLog(ref _updatedData, "Mail Subject", _link.MailSubject, lstLinkDetails["MAIL_SUBJECT"]);
                    _link.MailSubject = lstLinkDetails["MAIL_SUBJECT"];

                    SetUpdateLog(ref _updatedData, "Reply Mail", _link.ReplyEmail, lstLinkDetails["REPLY_EMAIL"]);
                    _link.ReplyEmail = lstLinkDetails["REPLY_EMAIL"];

                    // Updated on 30-MAY-2015
                    SetUpdateLog(ref _updatedData, "CC Email", _link.CcEmail, lstLinkDetails["CC_EMAIL"]);
                    _link.CcEmail = lstLinkDetails["CC_EMAIL"];

                    SetUpdateLog(ref _updatedData, "BCC Email", _link.BccEmail, lstLinkDetails["BCC_EMAIL"]);
                    _link.BccEmail = lstLinkDetails["BCC_EMAIL"];
                    #endregion

                    BuyerLinkCode = _link.BuyerLinkCode;
                    SupplierLinkCode = _link.VendorLinkCode;

                    #region /* Import-Export Settings */
                    SetUpdateLog(ref _updatedData, "Import RFQ", convert.ToString(_link.ImportRfq), lstLinkDetails["IMPORT_RFQ"]);
                    _link.ImportRfq = (short)convert.ToInt(lstLinkDetails["IMPORT_RFQ"]);

                    SetUpdateLog(ref _updatedData, "Export RFQ", convert.ToString(_link.ExportRfq), lstLinkDetails["EXPORT_RFQ"]);
                    _link.ExportRfq = (short)convert.ToInt(lstLinkDetails["EXPORT_RFQ"]);

                    SetUpdateLog(ref _updatedData, "Import Quote", convert.ToString(_link.ImportQuote), lstLinkDetails["IMPORT_QUOTE"]);
                    _link.ImportQuote = (short)convert.ToInt(lstLinkDetails["IMPORT_QUOTE"]);

                    SetUpdateLog(ref _updatedData, "Export Quote", convert.ToString(_link.ExportQuote), lstLinkDetails["EXPORT_QUOTE"]);
                    _link.ExportQuote = (short)convert.ToInt(lstLinkDetails["EXPORT_QUOTE"]);

                    SetUpdateLog(ref _updatedData, "Import PO", convert.ToString(_link.ImportPo), lstLinkDetails["IMPORT_PO"]);
                    _link.ImportPo = (short)convert.ToInt(lstLinkDetails["IMPORT_PO"]);

                    SetUpdateLog(ref _updatedData, "Export PO", convert.ToString(_link.ExportPo), lstLinkDetails["EXPORT_PO"]);
                    _link.ExportPo = (short)convert.ToInt(lstLinkDetails["EXPORT_PO"]);

                    SetUpdateLog(ref _updatedData, "Export RFQAck", convert.ToString(_link.ExportRfqAck), lstLinkDetails["EXPORT_RFQ_ACK"]);
                    _link.ExportRfqAck = (short)convert.ToInt(lstLinkDetails["EXPORT_RFQ_ACK"]);

                    SetUpdateLog(ref _updatedData, "Import POAck", convert.ToString(_link.ExportPoAck), lstLinkDetails["EXPORT_PO_ACK"]);
                    _link.ExportPoAck = (short)convert.ToInt(lstLinkDetails["EXPORT_PO_ACK"]);

                    SetUpdateLog(ref _updatedData, "Export POC", convert.ToString(_link.ExportPoc), lstLinkDetails["EXPORT_POC"]);
                    _link.ExportPoc = (short)convert.ToInt(lstLinkDetails["EXPORT_POC"]);

                    SetUpdateLog(ref _updatedData, "Notify Buyer", convert.ToString(_link.NotifyBuyer), lstLinkDetails["NOTIFY_BUYER"]);
                    _link.NotifyBuyer = (short)convert.ToInt(lstLinkDetails["NOTIFY_BUYER"]);

                    SetUpdateLog(ref _updatedData, "Notify Supp", convert.ToString(_link.NotifySupplr), lstLinkDetails["NOTIFY_SUPPLR"]);
                    _link.NotifySupplr = (short)convert.ToInt(lstLinkDetails["NOTIFY_SUPPLR"]);
                    #endregion

                    SetUpdateLog(ref _updatedData, "Default Price", convert.ToString(_link.DefaultPrice), lstLinkDetails["DEFAULT_PRICE"]);
                    if (lstLinkDetails["DEFAULT_PRICE"].Trim() != "") _link.DefaultPrice = (float)Convert.ToSingle(lstLinkDetails["DEFAULT_PRICE"]);

                    SetUpdateLog(ref _updatedData, "Supp WebService Url", convert.ToString(_link.Supp_Web_Service_Url), lstLinkDetails["SUP_WEB_SERVICE_URL"]);
                    if (lstLinkDetails["SUP_WEB_SERVICE_URL"].Trim() != "") _link.Supp_Web_Service_Url = lstLinkDetails["SUP_WEB_SERVICE_URL"];

                    if (_link.IsActive != null) oldActive = (short)_link.IsActive;
                    newActive = (short)convert.ToInt(lstLinkDetails["IS_ACTIVE"]);
                    _link.IsActive = (short)convert.ToInt(lstLinkDetails["IS_ACTIVE"]);

                    SetUpdateLog(ref _updatedData, "Import POC", convert.ToString(_link.ImportPoc), lstLinkDetails["IMPORT_POC"]);
                    _link.ImportPoc = (short)convert.ToInt(lstLinkDetails["IMPORT_POC"]);//ADDED by Kalpita on 26/12/2017

                    #endregion

                    #region /* ADD PDF & EXCEL BUYER-SUPPLIER LINK */
                    SmBuyerSupplierGroups _group = null;
                    if (Convert.ToString(lstLinkDetails["GROUP_ID"]).Trim() != "")
                    {
                        int.TryParse(lstLinkDetails["GROUP_ID"], out GroupID);

                        if (GroupID == 0) { GroupID = convert.ToInt(_link.GroupId); }
                        if (GroupID > 0)
                        {
                            // Get Old Group Name
                            string _oldGrpName = "";
                            SmBuyerSupplierGroups _lnkGrpInfo = SmBuyerSupplierGroups.Load(convert.ToInt(_link.GroupId));
                            if (_lnkGrpInfo != null) _oldGrpName = convert.ToString(_lnkGrpInfo.GroupCode);
                            //

                            // Get Selected Group ID & Info
                            _group = SmBuyerSupplierGroups.Load(GroupID);
                            //

                            #region /* Update Buyer-Supplier File import export formats */
                            SetUpdateLog(ref _updatedData, "Group", _oldGrpName, _group.GroupCode);
                            _link.GroupId = _group.GroupId;

                            SetUpdateLog(ref _updatedData, "Byr Format", _link.BuyerFormat, convert.ToString(_group.BuyerFormat));
                            _link.BuyerFormat = _group.BuyerFormat;

                            SetUpdateLog(ref _updatedData, "Supp Format", _link.VendorFormat, convert.ToString(_group.SupplierFormat));
                            _link.VendorFormat = _group.SupplierFormat;

                            SetUpdateLog(ref _updatedData, "Byr Exp Format", _link.BuyerExportFormat, convert.ToString(_group.BuyerExportFormat));
                            _link.BuyerExportFormat = _group.BuyerExportFormat;

                            SetUpdateLog(ref _updatedData, "Supp Exp Format", _link.SupplierExportFormat, convert.ToString(_group.SupplierExportFormat));
                            _link.SupplierExportFormat = _group.SupplierExportFormat;
                            #endregion

                            if (_group.BuyerFormat == "EXCEL_RFQ")
                            {
                                SmXlsGroupMapping _grpMapping = SmXlsGroupMapping.Load_By_GroupID((int)_group.GroupId);
                                SmXlsBuyerLink _xlsLink = SmXlsBuyerLink.Select_SM_XLS_BUYER_LINKs_By_LINKID(_link.Linkid);

                                if (_xlsLink == null)
                                {
                                    _xlsLink = new SmXlsBuyerLink();
                                    _xlsLink.Buyerid = _link.BuyerAddress.Addressid;
                                    _xlsLink.Supplierid = _link.VendorAddress.Addressid;
                                    _xlsLink.BuyerSuppLinkid = _link.Linkid;

                                    if (_grpMapping != null) _xlsLink.SmXlsGroupMapping = _grpMapping;
                                    else
                                    {
                                        _grpMapping = new SmXlsGroupMapping();
                                        _grpMapping.GroupId = _group.GroupId;
                                        _grpMapping.Insert(_dataAccess);

                                        _xlsLink.SmXlsGroupMapping = _grpMapping;
                                    }
                                    _xlsLink.Insert(_dataAccess);

                                    SetAuditLog("LeSMonitor", "New XLS Buyer-Supplier link (" + _buyer.AddrCode + "-" + _supplier.AddrCode + ") added by [" + UserHostAddress + "]. ", "Updated", "", _buyer.Addressid.ToString() + "-" + _supplier.Addressid.ToString(), _buyer.Addressid.ToString(), _supplier.Addressid.ToString(), _dataAccess);
                                }
                            }
                            else if (convert.ToString(_group.BuyerFormat).ToUpper() == "PDF" || convert.ToString(_group.SupplierFormat).ToUpper() == "PDF")
                            {
                                // Modified to include Supplier PDF Format too in Tab.
                                SmPdfMapping _pdfMapping = SmPdfMapping.LoadByGroup((int)_group.GroupId);
                                if (_pdfMapping == null)
                                {
                                    _pdfMapping = new SmPdfMapping();
                                    _pdfMapping.PdfMapid = GetNextKey("PDF_MAPID", "SM_PDF_MAPPING", _dataAccess);
                                    _pdfMapping.Groupid = _group.GroupId;
                                    _pdfMapping.Insert(_dataAccess);

                                    SmPdfBuyerLink _pdfLink = new SmPdfBuyerLink();
                                    _pdfLink.Buyerid = _link.BuyerAddress.Addressid;
                                    _pdfLink.BuyerSuppLinkid = _link.Linkid;
                                    _pdfLink.Supplierid = _link.VendorAddress.Addressid;
                                    _pdfLink.PdfMapid = _pdfMapping.PdfMapid;
                                    _pdfLink.MapId = GetNextKey("MAP_ID", "SM_PDF_BUYER_LINK", _dataAccess);
                                    _pdfLink.Insert(_dataAccess);

                                    SetAuditLog("LeSMonitor", "New PDF Buyer-Supplier link (" + _buyer.AddrCode + "-" + _supplier.AddrCode + ") added by [" + UserHostAddress + "]. ", "Updated", "", _buyer.Addressid.ToString() + "-" + _supplier.Addressid.ToString(), _buyer.Addressid.ToString(), _supplier.Addressid.ToString(), _dataAccess);
                                }
                            }
                        }
                    }
                    #endregion

                    _link.Update(_dataAccess);

                    string _updateLog = UpdateLinkRules(_link, IsGroupChange, IsNewGroup, _dataAccess); // Setup all rules to SmBuyerSupplierLink 

                    _updatedData += " " + _updateLog;

                    if ((oldActive == 0) && (newActive == 1)) // link activated Resubmit error files
                    {
                        // Set AuditLog 
                        if (_updatedData.Trim().Length > 0) SetAuditLog("LeSMonitor", "Buyer Supplier link (" + BuyerLinkCode + "-" + SupplierLinkCode + ") following fields updated by [" + UserHostAddress + "]. " + _updatedData, "Updated", (Convert.ToString(_link.BuyerAddress.Addressid) + "-" + Convert.ToString(_link.VendorAddress.Addressid)), "", Convert.ToString(_link.BuyerAddress.Addressid), Convert.ToString(_link.VendorAddress.Addressid), _dataAccess);
                        else SetAuditLog("LeSMonitor", "Buyer Supplier link (" + BuyerLinkCode + "-" + SupplierLinkCode + ") with no changes updated by [" + UserHostAddress + "]. " + _updatedData, "Updated", (Convert.ToString(_link.BuyerAddress.Addressid) + "-" + Convert.ToString(_link.VendorAddress.Addressid)), "", Convert.ToString(_link.BuyerAddress.Addressid), Convert.ToString(_link.VendorAddress.Addressid), _dataAccess);

                        SetAuditLog("LeSMonitor", "Buyer Supplier link (" + BuyerLinkCode + "-" + SupplierLinkCode + ") activated by [" + UserHostAddress + "].", "Updated", Convert.ToString(_link.BuyerAddress.Addressid) + "-" + Convert.ToString(_link.VendorAddress.Addressid), "", Convert.ToString(_link.BuyerAddress.Addressid), Convert.ToString(_link.VendorAddress.Addressid), _dataAccess);

                        // Resubmit error file waiting for importing
                        if (_dataAccess.CurrentTransaction != null) _dataAccess.CommitTransaction();
                        ResubmitFiles((int)_link.BuyerAddress.Addressid, (int)_link.VendorAddress.Addressid, _link.BuyerLinkCode, _link.VendorLinkCode);

                        // Check Is this link activated for eSupplierLite 
                       // if (GroupID > 0 && _group != null && convert.ToString(_group.SupplierFormat) == "ESUPPLIER_LITE") //chnaged by Kalpita on 06/04/2018
                        {
                            SmExternalUsers _user = SmExternalUsers.LoadByLinkID(_link.Linkid);
                            if (_user == null) AddNewLogin(convert.ToInt(_link.Linkid), UserHostAddress);
                        }
                    }
                    else
                    {
                        if (oldActive == 1 && newActive == 0) SetAuditLog("LeSMonitor", "Buyer Supplier link (" + BuyerLinkCode + "-" + SupplierLinkCode + ") deactivated by [" + UserHostAddress + "]. " + _updatedData, "Updated", (Convert.ToString(_link.BuyerAddress.Addressid) + "-" + Convert.ToString(_link.VendorAddress.Addressid)), "", Convert.ToString(_link.BuyerAddress.Addressid), Convert.ToString(_link.VendorAddress.Addressid), _dataAccess);
                        else
                        {
                            if (_updatedData.Trim().Length > 0) SetAuditLog("LeSMonitor", "Buyer Supplier link (" + BuyerLinkCode + "-" + SupplierLinkCode + ") following fields updated by [" + UserHostAddress + "]. " + _updatedData, "Updated", (Convert.ToString(_link.BuyerAddress.Addressid) + "-" + Convert.ToString(_link.VendorAddress.Addressid)), "", Convert.ToString(_link.BuyerAddress.Addressid), Convert.ToString(_link.VendorAddress.Addressid), _dataAccess);
                            else SetAuditLog("LeSMonitor", "Buyer Supplier link (" + BuyerLinkCode + "-" + SupplierLinkCode + ") with no changes updated by [" + UserHostAddress + "]. " + _updatedData, "Updated", (Convert.ToString(_link.BuyerAddress.Addressid) + "-" + Convert.ToString(_link.VendorAddress.Addressid)), "", Convert.ToString(_link.BuyerAddress.Addressid), Convert.ToString(_link.VendorAddress.Addressid), _dataAccess);
                        }
                        if (_dataAccess.CurrentTransaction != null) _dataAccess.CommitTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                if (_dataAccess.CurrentTransaction != null)
                    _dataAccess.RollbackTransaction();

                SetLog("Error in Update_Link_Details : " + ex.Message + " Stack Trace : " + ex.StackTrace);
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void SetUpdateLog(ref string str, string fieldName, string OldVal, string NewVal)
        {
            if (convert.ToString(OldVal).Trim() != NewVal.Trim())
                str += Environment.NewLine + fieldName + " changed from (" + convert.ToString(OldVal).Trim() + ") to (" + NewVal.Trim() + ")";
        }

        public void SetDataLog(ref string str, string fieldName, string dValue)
        {
            if (convert.ToString(dValue).Trim() != "")
                str += Environment.NewLine + fieldName + " : " + dValue.Trim() + ", ";
        }

        public void ResubmitFiles(int nBuyerID, int nSupplierID, string BuyerLinkCode, string VendorLinkCode)
        {
            try
            {
                SmAuditlog _AuditLog = new SmAuditlog();
                int _days = convert.ToInt(Convert.ToString(ConfigurationManager.AppSettings["Resubmit_Days"])); if (_days <= 0) { _days = 30; }
                DataSet _Resubmit = _AuditLog.SM_AUDITLOG_Select_By_BuyerSupplier_By_Days(BuyerLinkCode, VendorLinkCode, _days);
                if (_Resubmit.Tables.Count > 0)
                {
                    foreach (DataRow dr in _Resubmit.Tables[0].Rows)
                    {
                        if ((dr["FILENAME"].ToString().Length > 0) && (dr["LOGTYPE"].ToString().ToUpper() == "ERROR"))
                            if (!string.IsNullOrEmpty(dr["LOGID"].ToString()))
                                CopyFileToImport(Convert.ToInt32(dr["LOGID"].ToString()), nBuyerID, nSupplierID);
                    }
                }
            }
            catch (Exception ex)
            {
                SetLog("Error in Resubmit Files : " + ex.Message + " Stack Trace : " + ex.StackTrace);
                SetAuditLog("LeSMonitor", "Unable to resubmit files for new activated link, Error-" + ex.Message, "Error", nBuyerID + "-" + nSupplierID, "", Convert.ToString(nBuyerID).Trim(), Convert.ToString(nSupplierID).Trim());
            }
        }

        public void CopyFileToImport(int nLOG_ID, int cBuyerID, int cSuppID)
        {
            if (nLOG_ID != 0)
            {
                SupplierRoutines _Routine = new SupplierRoutines();
                Dictionary<string, string> lstAudit = GetAuditDetails(nLOG_ID);
                string cAudit = "";
                if (lstAudit.Count > 0)
                {
                    string cLOGID = nLOG_ID.ToString();
                    string filename = lstAudit["FILE_NAME"];
                    string ModuleName = lstAudit["MODULE_NAME"];
                    string filePath = System.Configuration.ConfigurationManager.AppSettings["AUDIT_DOC_PATH"] + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMM");
                    string OrgFilePath = System.Configuration.ConfigurationManager.AppSettings[ModuleName];
                    string keyRef2 = lstAudit["KEY_REF2"];
                    //string cBuyerID = lstAudit["BUYER_ID"];
                    //string cSuppID = lstAudit["SUPPLIER_ID"];
                    string ResubmitFile = Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("_HHmmss") + Path.GetExtension(filename);
                    string dir = ""; DirectoryInfo directory = null;
                    SetLog("Resubmitting file for module " + ModuleName);
                    try
                    {
                        filePath = GetCorrectPath(filePath, filename);
                        if (!File.Exists(filePath + "\\" + filename))
                            filePath = GetCorrectPath(ConfigurationManager.AppSettings["AUDIT_DOC_PATH"].ToString(), filename);
                        if (!File.Exists(filePath + "\\" + filename))
                            filePath = GetCorrectPath(ConfigurationManager.AppSettings["AUDIT_BACKUP_PATH"].ToString(), filename);
                        SetLog("CopyFileToImport Started");
                        if (File.Exists(filePath + "\\" + filename))
                        {
                            SetLog("file found " + filePath + "\\" + filename);
                            if (OrgFilePath != null && OrgFilePath.Contains("|"))
                            {
                                dir = GetValideSupplierPath(ModuleName, cBuyerID.ToString(), cSuppID.ToString(), OrgFilePath, filename);
                                if (!string.IsNullOrEmpty(dir)) directory = new DirectoryInfo(dir);
                            }
                            else
                            {
                                SetLog(ModuleName + " Module not found.");
                                if (OrgFilePath != null)
                                {
                                    dir = Path.GetDirectoryName(OrgFilePath + "\\" + filename);
                                    if (!string.IsNullOrEmpty(dir)) directory = new DirectoryInfo(dir);
                                }
                                else
                                {
                                    throw new Exception(ModuleName + " Module not found.");
                                }
                            }

                            if ((directory == null) || (!directory.Exists))
                            {
                                cAudit = "No valid path configured for - " + ModuleName;
                                SetLog(cAudit);
                                SetAuditLog("LeSMonitor", cAudit, "Error", keyRef2, filename, cBuyerID.ToString(), cSuppID.ToString());
                                return;
                            }

                            // Copy File
                            File.Copy(filePath + "\\" + filename, dir + "\\" + ResubmitFile);
                            SetLog("Copy File to " + dir + "\\" + ResubmitFile);
                            if (File.Exists(dir + "\\" + ResubmitFile))
                            {
                                SetAuditLog("LeSMonitor", ResubmitFile + " is resubmitted.", "Resubmit", keyRef2, ResubmitFile, cBuyerID.ToString(), cSuppID.ToString());
                                File.Copy(dir + "\\" + ResubmitFile, filePath + "\\" + ResubmitFile);
                                SetLog("CopyFileToImport from " + dir + "\\" + ResubmitFile + " to " + filePath + "\\" + ResubmitFile);
                                SmAuditlog _auditLog = SmAuditlog.Load(nLOG_ID);
                                if (_auditLog != null)
                                {
                                    _auditLog.Logtype = "Resubmitted";
                                    _auditLog.Update();
                                    SetLog("File Resubmited" + ResubmitFile);
                                }
                            }
                            SetLog("CopyFileToImport Stoped");
                        }
                    }
                    catch (Exception ex)
                    {
                        SetLog("Unable to resubmit for activated link - " + ex.Message + " Stack Trace :" + ex.StackTrace);
                        SetAuditLog("LeSMonitor", "Unable to resubmit for activated link - " + ex.Message, "Error", keyRef2, filename, cBuyerID.ToString(), cSuppID.ToString());
                    }
                }
            }
        }

        public void Delete_Link(int LinkID, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "";
            try
            {
                SmBuyerSupplierLink _link = SmBuyerSupplierLink.Load(LinkID);
                if (_link != null)
                {
                    SmAddress _buyer = SmAddress.Load(_link.BuyerAddress.Addressid);
                    SmAddress _supplier = SmAddress.Load(_link.VendorAddress.Addressid);

                    int SupplierID = (int)_link.VendorAddress.Addressid;
                    int BuyerID = (int)_link.BuyerAddress.Addressid;

                    SmBuyerSupplierItemRef _items = new SmBuyerSupplierItemRef();

                    //System.Data.DataSet dsItems = _items.GetItemMapping(SupplierID, BuyerID);

                    _dataAccess.BeginTransaction();

                    //foreach (System.Data.DataRow dr in dsItems.Tables[0].Rows)
                    //{
                    //    SmBuyerSupplierItemRef obj = new SmBuyerSupplierItemRef();
                    //    obj.Refid = Convert.ToInt32(dr["REFID"]);
                    //    obj.Delete(_dataAccess);
                    //}

                    DeleteLinkRule(convert.ToInt(_link.Linkid), _dataAccess); // Remove All rules related to this link                    

                    _link.Delete(_dataAccess);

                    AuditValue = "Buyer Supplier link (" + _buyer.AddrCode + "-" + _supplier.AddrCode + ") deleted by [" + UserHostAddress + "]";
                    SetAuditLog("LeSMonitor", AuditValue, "Updated", BuyerID + "-" + SupplierID, "", BuyerID.ToString(), SupplierID.ToString(), _dataAccess);

                    _dataAccess.CommitTransaction();
                }
            }
            catch
            {
                _dataAccess.RollbackTransaction();
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public System.Data.DataSet GetLinkedBuyersSupplier(int addressId)
        {
            System.Data.DataSet _ds = new System.Data.DataSet();
            SmAddress _obj = SmAddress.Load(addressId);
            if (_obj != null)
            {
                if (_obj.AddrType.ToLower().Contains("admin")) _ds = GetAllBuyers_Data();
                else if ((_obj.AddrType.ToLower().Contains("supplier"))) _ds = GetLinkedBuyers_Data((int)_obj.Addressid);
                else if ((_obj.AddrType.ToLower().Contains("buyer"))) _ds = GetLinkedSuppliers_Data((int)_obj.Addressid);
            }
            return _ds;
        }

        public DataSet GetLinkedSuppliers_Data(int BuyerID)
        {
            DataSet ds = new DataSet();
            Dal.DataAccess _dataAccess = null;
            try
            {
                _dataAccess = new Dal.DataAccess();
                _dataAccess.CreateConnection();
                _dataAccess.CreateSQLCommand("SELECT * FROM SM_ADDRESS LEFT OUTER JOIN LES_CLIENT_CONNECT_LOG ON SM_ADDRESS.ADDRESSID=LES_CLIENT_CONNECT_LOG.CLIENTID JOIN SM_BUYER_SUPPLIER_LINK on SM_BUYER_SUPPLIER_LINK.SUPPLIERID= SM_ADDRESS.ADDRESSID where SM_BUYER_SUPPLIER_LINK.BUYERID=@BuyerID");
                _dataAccess.AddParameter("BuyerID", BuyerID, ParameterDirection.Input);
                ds = _dataAccess.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dataAccess != null) _dataAccess._Dispose();
            }
            return ds;
        }

        public DataSet GetLinkedBuyers_Data(int SupplierID)
        {
            DataSet ds = new DataSet();
            Dal.DataAccess _dataAccess = null;
            try
            {
                _dataAccess = new Dal.DataAccess();
                _dataAccess.CreateConnection();
                _dataAccess.CreateSQLCommand("SELECT * FROM SM_ADDRESS LEFT OUTER JOIN LES_CLIENT_CONNECT_LOG ON SM_ADDRESS.ADDRESSID=LES_CLIENT_CONNECT_LOG.CLIENTID JOIN SM_BUYER_SUPPLIER_LINK on SM_BUYER_SUPPLIER_LINK.BUYERID= SM_ADDRESS.ADDRESSID where SM_BUYER_SUPPLIER_LINK.SUPPLIERID=@SupplierID");
                _dataAccess.AddParameter("SupplierID", SupplierID, ParameterDirection.Input);
                ds = _dataAccess.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dataAccess != null) _dataAccess._Dispose();
            }
            return ds;
        }

        public DataSet GetAllBuyers_Data()
        {
            DataSet ds = new DataSet();
            Dal.DataAccess _dataAccess = null;
            try
            {
                _dataAccess = new Dal.DataAccess();
                _dataAccess.CreateConnection();
                _dataAccess.CreateSQLCommand("SELECT * FROM SM_ADDRESS LEFT OUTER JOIN LES_CLIENT_CONNECT_LOG ON SM_ADDRESS.ADDRESSID=LES_CLIENT_CONNECT_LOG.CLIENTID JOIN SM_BUYER_SUPPLIER_LINK on SM_BUYER_SUPPLIER_LINK.BUYERID = SM_ADDRESS.ADDRESSID");
                ds = _dataAccess.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dataAccess != null) _dataAccess._Dispose();
            }
            return ds;
        }

        public int GetGroupFormatBuyersSupplierValue(int addressId)
        {
            int value = -1;
            SmAddress _obj = SmAddress.Load(addressId);
            if (_obj != null)
            {
                if (_obj.AddrType.ToLower().Contains("admin")) value = 0;
                else if ((_obj.AddrType.ToLower().Contains("supplier"))) value = 0;
                else if ((_obj.AddrType.ToLower().Contains("buyer"))) value = 1;
            }
            return value;
        }

        public Dictionary<string, string> GetMailDetails_Code(string cBuyerCode, string cSupplierCode)
        {
            string strDef = string.Empty;
            Dictionary<string, string> sldet = new Dictionary<string, string>(); sldet.Clear();

            SmAddress _byrobj = SmAddress.LoadByAddressCode(cBuyerCode);
            SmAddress _suppid = SmAddress.LoadByAddressCode(cSupplierCode);
            SmBuyerSupplierLinkCollection _collect = SmBuyerSupplierLink.Select_SM_BUYER_SUPPLIER_LINKs_By_BUYERID_SUPPLIERID(_byrobj.Addressid, _suppid.Addressid);

            if (_collect != null)
            {
                foreach(SmBuyerSupplierLink _obj in _collect)
                {
                    sldet.Add("SUPPLIER_MAIL",_obj.SupplierEmail);
                    sldet.Add("CC", _obj.CcEmail);
                    sldet.Add("BCC", _obj.BccEmail);
                }
            }
            else
            {
                sldet.Add("SUPPLIER_MAIL", strDef);
                sldet.Add("CC", strDef);
                sldet.Add("BCC", strDef);
            }
            return sldet;

        }

        #endregion

        #region /* GroupFlow */

        public void SaveGroupWorkFlow(int GroupID, int RFQ, int Quote, int PO, int POC, int RfqEndState, int QuoteEndState, int POEndState, int POCEndState)
        {
            try
            {
                string AuditValue = "";
                string _updatedData = "";
                string GroupCode = "";
                SmBuyerSupplierGroups _group = new SmBuyerSupplierGroups();
                if (GroupID > 0)
                {
                    _group.GroupId = GroupID;
                    _group.Load();
                    if (_group != null)
                    {
                        GroupCode = _group.GroupCode;
                    }
                }
                SmBuyerSupplierGroupFlow obj = SmBuyerSupplierGroupFlow.LoadByGroup(GroupID);
                if (obj == null) obj = new SmBuyerSupplierGroupFlow();

                SetUpdateLog(ref _updatedData, "RFq processs", convert.ToString(obj.Rfq), convert.ToString(RFQ));
                SetUpdateLog(ref _updatedData, "Quote processs", convert.ToString(obj.Quote), convert.ToString(Quote));
                SetUpdateLog(ref _updatedData, "PO processs", convert.ToString(obj.Po), convert.ToString(PO));
                SetUpdateLog(ref _updatedData, "POC processs", convert.ToString(obj.Poc), convert.ToString(POC));
                SetUpdateLog(ref _updatedData, "RFQEndState processs", convert.ToString(obj.RfqEndState), convert.ToString(RfqEndState));
                SetUpdateLog(ref _updatedData, "QuoteEndState processs", convert.ToString(obj.QuoteEndState), convert.ToString(QuoteEndState));
                SetUpdateLog(ref _updatedData, "POEndState processs", convert.ToString(obj.PoEndState), convert.ToString(POEndState));
                SetUpdateLog(ref _updatedData, "POCEndState processs", convert.ToString(obj.PocEndState), convert.ToString(POCEndState));

                obj.Rfq = RFQ;
                obj.Quote = Quote;
                obj.Po = PO;
                obj.Poc = POC;
                obj.RfqEndState = RfqEndState;
                obj.QuoteEndState = QuoteEndState;
                obj.PoEndState = POEndState;
                obj.PocEndState = POCEndState;

                //obj.QuoteExportMarker = QuoteExpMarker;
                //obj.QuoteBuyerExportMarker = QuoteByrExpMarker;
                //obj.POCExportMarker = POCExpMarker;
                //obj.POCBuyerExportMarker = POCByrExpMarker; 

                if (obj.GroupFlowid > 0)
                {
                    obj.Update();
                    AuditValue = "Group work flow for Group code (" + GroupCode + ") updated by :" + Convert.ToString(HttpContext.Current.Session["UserHostServer"]);
                    if (_updatedData.Trim().Length > 0)
                    {
                        AuditValue += " following fields updated " + _updatedData;
                    }
                    else
                    {
                        AuditValue += " with no changes";
                    }

                    SetAuditLog("LeSMonitor", AuditValue, "Updated", "", "", "", "");
                }
                else
                {
                    obj.GroupId = GroupID;
                    obj.Insert();
                    AuditValue = "Group Inserted by :" + Convert.ToString(HttpContext.Current.Session["UserHostServer"]);
                    if (_updatedData.Trim().Length > 0)
                    {
                        AuditValue += " following fields inserted " + _updatedData;
                    }
                    else
                    {
                        AuditValue += " with no changes";
                    }
                    SetAuditLog("LeSMonitor", AuditValue, "Updated", "", "", "", "");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region /* Groups */
        public System.Data.DataSet GetAllExcelGroups()
        {
            return SmBuyerSupplierGroups.GetAllExcelGroups();
        }

        public System.Data.DataSet GetExcelGroupsOnly()
        {
            return SmBuyerSupplierGroups.GetExcelGroupsOnly();
        }

        public System.Data.DataSet GetPDFGroupsOnly()
        {
            return SmBuyerSupplierGroups.GetPDFGroupsOnly();
        }

        public System.Data.DataSet GetAllGroups()
        {
            return SmBuyerSupplierGroups.GetAllGroups();
        }

        public void DeleteGroup(int GroupID, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "", GroupCode = "";
            try
            {
                SmBuyerSupplierGroups _group = SmBuyerSupplierGroups.Load(GroupID);
                if (_group != null)
                {
                    GroupCode = _group.GroupCode;
                    _dataAccess.BeginTransaction();

                    // Delete All Rules
                    SmBuyerSupplierRulesCollection _rules = SmBuyerSupplierRules.Select_SM_BUYER_SUPPLIER_RULESs_By_GROUPID(GroupID);
                    foreach (SmBuyerSupplierRules _rule in _rules)
                    {
                        _rule.Delete(_dataAccess);
                    }

                    // Delete Mappings
                    if (convert.ToString(_group.BuyerFormat).ToUpper() == "EXCEL_RFQ")
                    {
                        SmXlsGroupMapping _grpMapp = SmXlsGroupMapping.Load_By_GroupID((int)_group.GroupId);
                        if (_grpMapp != null)
                        {
                            SmXlsBuyerLinkCollection _xlsLinks = SmXlsBuyerLink.Select_SM_XLS_BUYER_LINKs_By_EXCEL_MAPID(_grpMapp.ExcelMapid);
                            for (int i = 0; i < _xlsLinks.Count; i++)
                            {
                                _xlsLinks[i].Delete(_dataAccess);
                            }
                            _grpMapp.Delete(_dataAccess);
                        }
                    }
                    else if (convert.ToString(_group.BuyerFormat).ToUpper() == "PDF")
                    {
                        SmPdfMapping _pdfMapping = SmPdfMapping.LoadByGroup(_group.GroupId);
                        if (_pdfMapping != null && _pdfMapping.PdfMapid > 0)
                        {
                            SmPdfBuyerLink _pdfLink = SmPdfBuyerLink.LoadByPDFMapId(_pdfMapping.PdfMapid);
                            SmPdfItemMappingCollection _itemMapColl = SmPdfItemMapping.LoadByPDF_MapID(_pdfMapping.PdfMapid);
                            for (int i = 0; i < _itemMapColl.Count; i++)
                            {
                                _itemMapColl[i].Delete(_dataAccess);
                            }
                            _pdfMapping.Delete(_dataAccess);
                            _pdfLink.Delete(_dataAccess);
                        }
                    }
                    _group.Delete(_dataAccess);

                    AuditValue = "Group (" + GroupCode + ") deleted by [" + UserHostAddress + "]";
                    SetAuditLog("LeSMonitor", AuditValue, "Updated", "", "", "", "", _dataAccess);

                    _dataAccess.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void SaveGroup(int GroupID, string GroupCode, string GroupDescr, string BuyerFormat, string SuppFormat, string BuyerExportFormat, string SuppExportFormat, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "";
            try
            {
                SmBuyerSupplierGroups _group = new SmBuyerSupplierGroups();
                string _updatedData = "";
                if (GroupID > 0)
                {
                    _group.GroupId = GroupID;
                    _group.Load();
                }
                SetUpdateLog(ref _updatedData, "Group code", convert.ToString(_group.GroupCode), convert.ToString(GroupCode));
                SetUpdateLog(ref _updatedData, "Group Description", convert.ToString(_group.GroupDesc), convert.ToString(GroupDescr));
                SetUpdateLog(ref _updatedData, "Buyer Format", convert.ToString(_group.BuyerFormat), convert.ToString(BuyerFormat));
                SetUpdateLog(ref _updatedData, "Supplier Format", convert.ToString(_group.SupplierFormat), convert.ToString(SuppFormat));
                SetUpdateLog(ref _updatedData, "BuyerExportFormat", convert.ToString(_group.BuyerExportFormat), convert.ToString(BuyerExportFormat));
                SetUpdateLog(ref _updatedData, "SupplierExportFormat", convert.ToString(_group.SupplierExportFormat), convert.ToString(SuppExportFormat));

                _group.GroupCode = GroupCode;
                _group.GroupDesc = GroupDescr;
                _group.BuyerFormat = BuyerFormat;
                _group.SupplierFormat = SuppFormat;
                _group.BuyerExportFormat = BuyerExportFormat;
                _group.SupplierExportFormat = SuppExportFormat;

                _dataAccess.BeginTransaction();

                if (_group.GroupId > 0)
                {
                    _group.Update(_dataAccess);
                    AuditValue = "Group (" + GroupCode + ") updated by [" + UserHostAddress + "]";
                    if (_updatedData.Trim().Length > 0)
                    {
                        AuditValue += " following field updated " + _updatedData;
                    }
                    else
                    {
                        AuditValue += " with no changes ";
                    }
                }
                else
                {
                    _group.GroupId = _group.Insert(_dataAccess);
                    AuditValue = "New Group (" + GroupCode + ") added by [" + UserHostAddress + "]";
                    if (_updatedData.Trim().Length > 0)
                    {
                        AuditValue += " folowing field added " + _updatedData;
                    }
                    else
                    {
                        AuditValue += " with no changes ";
                    }
                }

                if (_group.BuyerFormat == "EXCEL_RFQ")
                {
                    SmXlsGroupMapping _xlsMapping = SmXlsGroupMapping.Load_By_GroupID((int)_group.GroupId);
                    if (_xlsMapping == null)
                    {
                        _xlsMapping = new SmXlsGroupMapping();
                        _xlsMapping.GroupId = _group.GroupId;
                        _xlsMapping.Insert(_dataAccess);

                    }
                }

                // Set Buyer-Supplier Link Formats if group formats are changed
                SmBuyerSupplierLinkCollection _linkColl = SmBuyerSupplierLink.Select_SM_BUYER_SUPPLIER_LINKs_By_GROUPID((int)_group.GroupId);
                if (_linkColl.Count > 0)
                {
                    foreach (SmBuyerSupplierLink link in _linkColl)
                    {
                        link.BuyerFormat = _group.BuyerFormat;
                        link.BuyerExportFormat = _group.BuyerExportFormat;
                        link.VendorFormat = _group.SupplierFormat;
                        link.SupplierExportFormat = _group.SupplierExportFormat;
                        link.Update(_dataAccess);
                        //AuditValue += " Fields updated BuyerFormat :" + _group.BuyerFormat + ", BuyerExportFormat :" + _group.BuyerExportFormat + ", SupplierFormat :" + _group.SupplierFormat + ", SupplierExportFormat :" + _group.SupplierExportFormat;
                    }
                }
                //

                SetAuditLog("LeSMonitor", AuditValue, "Updated", "", "", "", "", _dataAccess);
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public int ValidateGroup(string GroupCode, int GroupID)
        {
            return SmBuyerSupplierGroups.ValidateGroup(GroupCode, GroupID);
        }

        public int AddNewGroup(int GroupID, string GroupCode, string GroupDescr, string BuyerFormat, string SuppFormat, string BuyerExportFormat, string SupplierExportFormat, int CopyFromGroup, int BuyerID, int SupplierID, bool bCreateNewlink, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "", _ruleUpdates = "";
            try
            {
                SmBuyerSupplierLink _link = null;
                SmBuyerSupplierGroups _copyFrmGroup = SmBuyerSupplierGroups.Load(CopyFromGroup);
                SmBuyerSupplierGroups _group = new SmBuyerSupplierGroups();
                if (GroupID > 0)
                {
                    _group.GroupId = GroupID;
                    _group.Load();
                }

                _group.GroupCode = GroupCode.Trim();
                _group.GroupDesc = GroupDescr.Trim();
                _group.BuyerFormat = BuyerFormat.Trim();
                _group.SupplierFormat = SuppFormat.Trim();
                _group.BuyerExportFormat = BuyerExportFormat.Trim();
                _group.SupplierExportFormat = SupplierExportFormat.Trim();

                _dataAccess.BeginTransaction(); // Start Transaction

                // Insert Group 
                _group.GroupId = _group.Insert(_dataAccess);
                AuditValue = "New Group (" + GroupCode.Trim() + ") added by [" + UserHostAddress + "]";

                if (BuyerID > 0 && SupplierID > 0)
                {
                    #region /* Copy Rules of Buyer & Supplier */
                    SmBuyerSupplierRulesCollection _rulesColl = new SmBuyerSupplierRulesCollection();
                    DataSet dsBuyerRules = new DataSet();
                    if (BuyerID > 0)
                    {
                        dsBuyerRules.Clear();
                        dsBuyerRules = Get_Party_Specific_Default_Rules(BuyerID, BuyerFormat);

                        foreach (DataRow _row in dsBuyerRules.Tables[0].Rows)
                        {
                            //UpdateInsertRuleValue(convert.ToInt(_group.GroupId), convert.ToInt(_row["RULE_ID"]), convert.ToString(_row["RULE_VALUE"]),_dataAccess, UserHostAddress);
                            SmEsupplierRules _eRule = SmEsupplierRules.Load(convert.ToInt(_row["RULE_ID"]));
                            SmBuyerSupplierRules _rule = SmBuyerSupplierRules.LoadByGroupRule(GroupID, convert.ToInt(_row["RULE_ID"])); if (_rule == null)
                            {
                                _rule = new SmBuyerSupplierRules();
                                _rule.GroupRuleId = 0;
                                _rule.GroupId = _group.GroupId;
                                _rule.RuleId = convert.ToInt(_row["RULE_ID"]);
                            }

                            string oldValue = _rule.RuleValue;
                            _rule.RuleValue = convert.ToString(_row["RULE_VALUE"]);
                            _rulesColl.Add(_rule);
                        }
                    }

                    if (SupplierID > 0)
                    {
                        dsBuyerRules.Clear();
                        dsBuyerRules = Get_Party_Specific_Default_Rules(SupplierID, SuppFormat);

                        foreach (DataRow _row in dsBuyerRules.Tables[0].Rows)
                        {
                            SmBuyerSupplierRules _rule = SmBuyerSupplierRules.LoadByGroupRule(GroupID, convert.ToInt(_row["RULE_ID"])); if (_rule == null)
                            {
                                _rule = new SmBuyerSupplierRules();
                                _rule.GroupRuleId = 0;
                                _rule.GroupId = _group.GroupId;
                                _rule.RuleId = convert.ToInt(_row["RULE_ID"]);
                            }

                            string oldValue = _rule.RuleValue;
                            _rule.RuleValue = convert.ToString(_row["RULE_VALUE"]);
                            _rulesColl.Add(_rule);
                        }
                    }

                    foreach (SmBuyerSupplierRules obj in _rulesColl)
                    {
                        SmEsupplierRules _eRule = SmEsupplierRules.Load(convert.ToInt(obj.RuleId));
                        if (obj.GroupRuleId > 0) obj.Update(_dataAccess);
                        else obj.Insert(_dataAccess);
                    }
                    #endregion

                    // Add New Link
                    _link = Add_BuyerSupplier_Link(SupplierID, BuyerID, convert.ToInt(_group.GroupId), BuyerFormat, SuppFormat, BuyerExportFormat, SupplierExportFormat, _dataAccess, convert.ToInt(CopyFromGroup), bCreateNewlink, UserHostAddress);

                    #region /* Copy Group Mapping (Only for XLS & PDF) */
                    if (_link != null && convert.ToInt(_link.Linkid) > 0 && CopyFromGroup > 0)
                    {
                        if (convert.ToString(_group.BuyerFormat).Trim().ToUpper() == "EXCEL_RFQ")
                        {
                            SmXlsGroupMapping _CopyMapping = SmXlsGroupMapping.Load_By_GroupID(CopyFromGroup);
                            SmXlsBuyerLinkCollection _xlsCopyLinkColl = SmXlsBuyerLink.Select_SM_XLS_BUYER_LINKs_By_EXCEL_MAPID((int)_CopyMapping.ExcelMapid);
                            SmXlsBuyerLink _xlsLink = SmXlsBuyerLink.Select_SM_XLS_BUYER_LINKs_By_LINKID(_link.Linkid);

                            if (_xlsLink == null && _xlsCopyLinkColl.Count > 0)
                            {
                                _xlsLink = new SmXlsBuyerLink();
                                _xlsLink.Buyerid = BuyerID;
                                _xlsLink.Supplierid = SupplierID;
                                _xlsLink.BuyerSuppLinkid = _link.Linkid;
                                _xlsLink.MapCell1 = _xlsCopyLinkColl[0].MapCell1;
                                _xlsLink.MapCell1Val1 = _xlsCopyLinkColl[0].MapCell1Val1;
                                _xlsLink.MapCell1Val2 = _xlsCopyLinkColl[0].MapCell1Val2;
                                _xlsLink.MapCell2 = _xlsCopyLinkColl[0].MapCell2;
                                _xlsLink.MapCell2Val = _xlsCopyLinkColl[0].MapCell2Val;

                                _xlsLink.MapCellNoDisc = _xlsCopyLinkColl[0].MapCellNoDiscVal;
                                _xlsLink.MapCellNoDiscVal = _xlsCopyLinkColl[0].MapCellNoDiscVal;
                                _xlsLink.MapCell1 = _xlsCopyLinkColl[0].MapCell1;

                                if (_CopyMapping != null)
                                {
                                    _CopyMapping.ExcelMapid = 0;
                                    _CopyMapping.GroupId = _group.GroupId;
                                    _CopyMapping.Insert(_dataAccess);

                                    _xlsLink._excelMapid = _CopyMapping.ExcelMapid;
                                }

                                _xlsLink.Insert(_dataAccess);
                            }
                        }
                        else if (convert.ToString(_group.BuyerFormat).Trim().ToUpper() == "PDF")
                        {
                            SmPdfMapping _pddCopyMapping = SmPdfMapping.LoadByGroup(CopyFromGroup);
                            if (_pddCopyMapping != null)
                            {
                                SmPdfItemMappingCollection _CopyItemMapping = SmPdfItemMapping.LoadByPDF_MapID(_pddCopyMapping.PdfMapid);
                                SmPdfBuyerLink _pdfCopyLink = SmPdfBuyerLink.LoadByPDFMapId(_pddCopyMapping.PdfMapid);

                                _pddCopyMapping.PdfMapid = GetNextKey("PDF_MAPID", "SM_PDF_MAPPING", _dataAccess);
                                _pddCopyMapping.Groupid = _group.GroupId;
                                _pddCopyMapping.Insert(_dataAccess);

                                SmPdfBuyerLink _pdfLink = new SmPdfBuyerLink();
                                _pdfLink.Buyerid = BuyerID;
                                _pdfLink.BuyerSuppLinkid = _link.Linkid;
                                _pdfLink.Supplierid = SupplierID;
                                _pdfLink.PdfMapid = _pddCopyMapping.PdfMapid;
                                _pdfLink.Mapping1 = _pdfCopyLink.Mapping1;
                                _pdfLink.Mapping2 = _pdfCopyLink.Mapping2;
                                _pdfLink.Mapping3 = _pdfCopyLink.Mapping3;
                                _pdfLink.Mapping1Value = _pdfCopyLink.Mapping1Value;
                                _pdfLink.Mapping2Value = _pdfCopyLink.Mapping2Value;
                                _pdfLink.Mapping3Value = _pdfCopyLink.Mapping3Value;
                                _pdfLink.DocType = _pdfCopyLink.DocType;
                                _pdfLink.MapId = GetNextKey("MAP_ID", "SM_PDF_BUYER_LINK", _dataAccess);
                                _pdfLink.Insert(_dataAccess);

                                int newItemID = GetNextKey("ITEM_MAPID", "SM_PDF_ITEM_MAPPING", _dataAccess);
                                foreach (SmPdfItemMapping _copyItem in _CopyItemMapping)
                                {
                                    _copyItem.ItemMapid = newItemID;
                                    _copyItem.PdfMapid = _pddCopyMapping.PdfMapid;
                                    _copyItem.Insert(_dataAccess);
                                    newItemID++;
                                }
                            }
                        }
                    }
                    #endregion
                }
                SetAuditLog("LeSMonitor", AuditValue, "Updated", "", "", "", "", _dataAccess);
                _dataAccess.CommitTransaction();

                Dal.DataAccess _dataAccess1 = null;
                try
                {
                    _dataAccess1 = new Dal.DataAccess();
                    _dataAccess1.BeginTransaction();

                    #region /* Apply Default rules for New Buyer Supplier Link */
                    if (_link != null && _link.Linkid > 0) //&& bCreateNewlink  changed by kalpita on 27/10/2017  (issue: rules not applied)
                    {
                        _ruleUpdates = UpdateLinkRules(_link, true, true, _dataAccess1);
                    }
                    #endregion
                    if (_ruleUpdates.Trim() != "") SetAuditLog("LeSMonitor", _ruleUpdates, "Updated", "", "", "", "", _dataAccess1);
                    _dataAccess1.CommitTransaction();
                }
                catch (Exception ex)
                {
                    string AddrCodes = "(-)";
                    if (_link != null)
                    {
                        AddrCodes = "(" + _link.BuyerAddress.AddrCode + "-" + _link.VendorAddress.AddrCode + ")";
                    }
                    SetLog("Unable to apply Link-Rules for Buyer-Supplier " + AddrCodes + "." + ex.StackTrace);
                    _dataAccess1.RollbackTransaction();
                }
                finally
                {
                    if (_dataAccess1 != null) _dataAccess1._Dispose();
                }
                return (int)_group.GroupId;
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public System.Data.DataSet GetAllDefaultBuyers()
        {
            return SmAddress.GetAllDefaultBuyers();
        }

        public System.Data.DataSet GetAllDefaultSuppliers()
        {
            return SmAddress.GetAllDefaultSuppliers();
        }

        public int Get_GroupID_BY_GroupCode(string GroupCode)
        {
            SmBuyerSupplierGroups _Groups = new SmBuyerSupplierGroups();
            return _Groups.Get_GroupId_BY_GroupCode(GroupCode);
        }


        #endregion

        #region /* Rules */
        public DataSet GetAllRules()
        {
            return SmEsupplierRules.GetAllRules();
        }

        public void SaveRule(int RuleID, string RuleNumber, string DocType, string RuleCode, string RuleDescr, string Comments, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "";
            try
            {
                SmEsupplierRules _eRules = new SmEsupplierRules();
                if (RuleID > 0)
                {
                    _eRules.Ruleid = RuleID;
                    _eRules.Load();
                }
                _eRules.RuleNumber = RuleNumber.Trim();
                _eRules.DocType = DocType.Trim();
                _eRules.RuleCode = RuleCode.Trim();
                _eRules.RuleDesc = RuleDescr.Trim();
                _eRules.RuleComments = Comments.Trim();

                _dataAccess.BeginTransaction();

                if (_eRules.Ruleid > 0)
                {
                    _eRules.Update(_dataAccess);
                    AuditValue = "Rule (" + convert.ToString(_eRules.RuleCode).Trim() + ") updated by [" + UserHostAddress + "]";
                }
                else
                {
                    int id = GetNextKey("RULEID", "SM_ESUPPLIER_RULES", _dataAccess);
                    _eRules.Ruleid = id;
                    _eRules.Insert(_dataAccess);
                    AuditValue = " New Rule (" + convert.ToString(_eRules.RuleCode).Trim() + ") added by [" + UserHostAddress + "]";
                }

                SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "", _dataAccess);
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void DeleteRule(int RuleID, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "";
            try
            {
                SmEsupplierRules _eRule = SmEsupplierRules.Load(RuleID);
                _dataAccess.CreateConnection();
                _dataAccess.BeginTransaction();

                if (_eRule != null)
                {
                    _eRule.Delete(_dataAccess);
                    AuditValue = "Rule (" + convert.ToString(_eRule.RuleCode).Trim() + ") deleted by [" + UserHostAddress + "]";
                    SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "", _dataAccess);
                }
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public int ValidateRule(string RuleCode, int RuleID)
        {
            return SmEsupplierRules.ValidateRuleCode(RuleCode.Trim(), RuleID);
        }

        public int Get_RuleID_By_RuleCode(string _RuleCode)
        {
            int RuleId = 0;
            RuleId = SmEsupplierRules.Get_RuleId_By_RuleCode(_RuleCode);
            return RuleId;
        }

        public DataSet Get_RuleDetails_By_RuleCode(string _RuleCode)
        {
            DataSet RuleData = new DataSet();
            RuleData = SmEsupplierRules.Get_RuleDetails_By_RuleCode(_RuleCode);
            return RuleData;
        }

        public string GetRuleCodeByRuleID(int RuleID)
        {
            SmEsupplierRules _rule = SmEsupplierRules.Load(RuleID);
            if (_rule != null) return convert.ToString(_rule.RuleCode).Trim();
            else return "";
        }

        public string GetRuleDetails(int RuleID)
        {
            string _ruledet="";
            SmEsupplierRules _rule = SmEsupplierRules.Load(RuleID);
            if (_rule != null) { _ruledet = convert.ToString(_rule.RuleDesc).Trim() + "|" + convert.ToString(_rule.RuleComments).Trim() +"|" + convert.ToString(_rule.RuleNumber).Trim(); }
            return _ruledet;
        }

        public DataSet GetUnAssigned_Rules(string DOCFORMATID)
        {
            return SmEsupplierRules.GetUnAssigned_eSupplierRules(DOCFORMATID);
        }


     
        #endregion

        #region /* Default Rules */
        public DataSet GetAllDefaultRules()
        {
            return SmDefaultRules.GetAllDefaultRules();
        }

        public System.Data.DataSet Get_Party_Specific_Default_Rules(int Addressid, string GroupFormat)
        {
            return SmDefaultRules.SM_DEFAULT_RULES_Select_By_AddressID(Addressid, GroupFormat.Trim());
        }

        //changed by kalpita on 17/01/2018
        public void SaveDefaultRule(int DefID, int AddressID, string GroupFormat, int RuleID, string RuleValue, string UserHostAddress,Dal.DataAccess DataAccess)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "";
            try
            {
                if (DataAccess == null) { _dataAccess.BeginTransaction(); } else { _dataAccess = DataAccess; }

                SmDefaultRules _eRules = new SmDefaultRules();
                SmAddress _address = SmAddress.Load_ByID(AddressID, _dataAccess);
                SmEsupplierRules _rule = null;
                if (DefID > 0)
                {
                    _eRules.DefId = DefID;
                    _eRules.Load();
                }
                _eRules.Addressid = AddressID;
                _eRules.GroupFormat = GroupFormat.Trim();
                if (RuleID > 0)
                {
                    _rule = SmEsupplierRules.Load_by_ID(RuleID, _dataAccess);
                    _eRules.RuleId = RuleID;
                    _eRules.RuleValue = convert.ToString(RuleValue).Trim();
                }
                else _eRules.RuleId = null;
                if (_eRules.DefId > 0)
                {
                    _eRules.Update(_dataAccess);
                    if (_rule != null && _rule.Ruleid > 0)
                    {
                        AuditValue = "Default Rule '" + _rule.RuleCode + "' for (" + convert.ToString(_address.AddrCode) + "-" + convert.ToString(_eRules.GroupFormat).Trim() + ") Updated by [" + UserHostAddress + "]";
                    }
                    else
                    {
                        AuditValue = "Default Rule for (" + convert.ToString(_address.AddrCode) + "-" + convert.ToString(_eRules.GroupFormat).Trim() + ") Updated by [" + UserHostAddress + "]";
                    }
                }
                else
                {
                    int id = GetNextKey("DEF_ID", "SM_DEFAULT_RULES", _dataAccess);
                    _eRules.DefId = id;
                    _eRules.Insert(_dataAccess);

                    if (_rule != null && _rule.Ruleid > 0)
                    {
                        AuditValue = "New Default Rule '" + _rule.RuleCode + "' for (" + convert.ToString(_address.AddrCode) + "-" + convert.ToString(_eRules.GroupFormat).Trim() + ") Updated by [" + UserHostAddress + "]";
                    }
                    else
                    {
                        AuditValue = "New Default Rule for (" + convert.ToString(_address.AddrCode) + "-" + convert.ToString(_eRules.GroupFormat).Trim() + ") Updated by [" + UserHostAddress + "]";
                    }
                }
                SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "", _dataAccess);
                if (DataAccess == null) { _dataAccess.CommitTransaction(); }
            }
            catch (Exception ex)
            {
                if (DataAccess == null) { _dataAccess.RollbackTransaction(); }
                throw ex;
            }
            finally
            {
                if (DataAccess == null) { _dataAccess._Dispose(); }
            }
        }

        public void DeleteDefaultRuleAddress(int ADDRESSID, string Group_Format, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "";

            try
            {
                SmDefaultRulesCollection collection = SmDefaultRules.select_SM_DOC_FORMAT_All_By_ADDRESS_GROUP_FORMAT(ADDRESSID, Group_Format);
                /*
                SmBuyerSupplierLinkRuleCollection _relatedLinkRules = new SmBuyerSupplierLinkRuleCollection();
                SmBuyerSupplierRulesCollection _groupRules = new SmBuyerSupplierRulesCollection();

                DataSet dsLinks = GetLinkedBuyersSupplier(ADDRESSID);
                foreach (DataRow dr in dsLinks.Tables[0].Rows)
                {
                    int LINKID = convert.ToInt(dr["LINKID"]);
                    int GROUPID = convert.ToInt(dr["GROUP_ID1"]);
                    foreach (SmDefaultRules defaultRule in collection)
                    {
                        if (convert.ToInt(defaultRule.RuleId) > 0)
                        {
                            SmBuyerSupplierLinkRule _linkRule = SmBuyerSupplierLinkRule.LoadByLinkRule(LINKID, convert.ToInt(defaultRule.RuleId));
                            if (_linkRule != null) _relatedLinkRules.Add(_linkRule);

                            SmBuyerSupplierRules _groupRule = SmBuyerSupplierRules.LoadByGroupRule(GROUPID, convert.ToInt(defaultRule.RuleId));
                            if (_groupRule != null) _groupRules.Add(_groupRule);
                        }
                    }
                } 
                 * */

                _dataAccess.BeginTransaction();

                /*
                foreach (SmBuyerSupplierRules rule in _groupRules)
                {
                    rule.Delete(_dataAccess);
                }

                foreach (SmBuyerSupplierLinkRule linkRule in _relatedLinkRules)
                {
                    linkRule.Delete(_dataAccess);
                }
                 */

                foreach (SmDefaultRules defaultRule in collection)
                {
                    defaultRule.Delete(_dataAccess);
                }

                AuditValue = "Default Rule (" + Group_Format + ") deleted by [" + UserHostAddress + "]";
                SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "", _dataAccess);

                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void DeleteDefaultRule(int Def_ID, string UserHostAddress, string Rule_Code, string Addr_Name, int DeleteFromLink)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "";
            try
            {
                SmDefaultRules _eRule = SmDefaultRules.Load(Def_ID);
                _dataAccess.BeginTransaction();
                if (_eRule != null)
                {
                    SmBuyerSupplierLinkRuleCollection _collection = new SmBuyerSupplierLinkRuleCollection();
                    if (DeleteFromLink == 1) _collection = GetAllLinksByAddress(convert.ToInt(_eRule.Addressid), convert.ToInt(_eRule.RuleId));

                    _eRule.Delete(_dataAccess);
                    if (DeleteFromLink == 1)
                    {
                        // delete this rule from all links
                        foreach (SmBuyerSupplierLinkRule _linkRule in _collection)
                        {
                            _linkRule.Delete(_dataAccess);
                        }
                    }
                }

                AuditValue = "Default Rule (" + Rule_Code + ") for " + Addr_Name + " deleted by [" + UserHostAddress + "]";
                SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "", _dataAccess);

                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public DataSet GetPartyCode()
        {
            return SmAddress.GetAllAddress();
        }

        public DataSet GetGroupFormatList(int AddressId)
        {
            string AddrType = GetAddressType(AddressId);
            return SmDefaultRules.select_SM_DOC_FORMAT(AddrType);
        }

        public DataSet GetGroupFormatList(string AddrType)
        {
            return SmDefaultRules.select_SM_DOC_FORMAT(AddrType);
        }

        public DataSet GetAllGroupFormat()
        {
            return SmDefaultRules.select_SM_DOC_FORMAT_All();
        }

        public DataSet GetRuleData_By_AddressID_GroupFormat_RuleCode(int addressid, string GroupFormat, int RuleID)
        {
            return SmDefaultRules.SM_DEFAULT_RULES_Select_By_AddressID_GroupFormat_RuleCode(addressid, GroupFormat.Trim(), RuleID);
        }

        //added by Kalpita on 04/10/2017
        public bool CheckDuplicateDefaultRule(int AddressID, string GroupFormat)
        {
            bool result = false;
            try
            {
                DataSet ds = Get_Party_Specific_Default_Rules(AddressID, GroupFormat);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            { }
            return result;
        }

     
        //added by Kalpita on 09/12/2017
       // public DataSet GetDefaultRules_Addressid(int AddressID)
//{
        //    return SmDefaultRules.SM_DEFAULT_RULES_Select_By_AddressID(AddressID);
       // }

        public DataSet GetDefaultRules_Addressid_Format(int AddressID, string GROUP_FORMAT)
        {
            return SmDefaultRules.GetDefaultRules_By_AddressID_Format(AddressID, GROUP_FORMAT);
        }

        public System.Data.DataSet GET_ESUPPLIER_RULES_LIST_Without_AddressID(int AddressID, string GroupFormat)
        {
            return SmDefaultRules.GetDefaultRules_Without_Addressid(AddressID, GroupFormat);
        }
        #endregion

        #region /* Buyer_Supplier Rules */
        public System.Data.DataSet Get_Group_Specific_Rules(int GroupID)
        {
            return SmBuyerSupplierRules.Sm_BuyerSupplier_Rules_Select_by_Group(GroupID);
        }

        public System.Data.DataSet GetUnlinkedRules(int GroupID)
        {
            return SmBuyerSupplierRules.Sm_BuyerSupplier_Rules_Select_Unlinked_Rules(GroupID);
        }

        public void AddNewRule(int RuleID, int GroupID, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "";
            try
            {
                SmBuyerSupplierGroups _group = SmBuyerSupplierGroups.Load(GroupID);
                SmEsupplierRules _rule = SmEsupplierRules.Load(RuleID);

                SmBuyerSupplierRules _rules = new SmBuyerSupplierRules();
                _rules.SmBuyerSupplierGroups = SmBuyerSupplierGroups.Load(GroupID);
                _rules.SmEsupplierRules = SmEsupplierRules.Load(RuleID);
                _rules.GroupId = GroupID;
                _rules.RuleId = RuleID;
                _rules.RuleValue = "0";

                _dataAccess.BeginTransaction();
                _rules.Insert(_dataAccess);

                AuditValue = "New Rule (" + _rule.RuleCode + ") linked to Group (" + _group.GroupCode + ")  by [" + UserHostAddress + "]";
                SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "", _dataAccess);

                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void UpdateRuleValue(int GroupRuleID, string RuleValue, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "", oldValue = "";
            try
            {
                SmBuyerSupplierRules _rule = SmBuyerSupplierRules.Load(GroupRuleID);
                if (_rule != null)
                {
                    SmEsupplierRules _eRule = SmEsupplierRules.Load(_rule.SmEsupplierRules.Ruleid);
                    SmBuyerSupplierGroups _group = SmBuyerSupplierGroups.Load(_rule.SmBuyerSupplierGroups.GroupId);

                    _dataAccess.BeginTransaction();

                    oldValue = _rule.RuleValue;
                    _rule.RuleValue = RuleValue;
                    _rule.Update(_dataAccess);

                    AuditValue = "Group-Rule (" + _group.GroupCode + "-" + _eRule.RuleCode + ") RuleValue (" + oldValue + ") is modified to " + RuleValue + " by [" + UserHostAddress + "]";
                    SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "", _dataAccess);

                    _dataAccess.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void UpdateInsertRuleValue(int GroupID, int RuleID, string RuleValue, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "", oldValue = "";
            try
            {
                if ((GroupID > 0) && (RuleID > 0))
                {
                    SmEsupplierRules _eRule = SmEsupplierRules.Load(RuleID);
                    SmBuyerSupplierGroups _group = SmBuyerSupplierGroups.Load(GroupID);

                    SmBuyerSupplierRules _rule = SmBuyerSupplierRules.LoadByGroupRule(GroupID, RuleID);
                    if (_rule == null)
                    {
                        _rule = new SmBuyerSupplierRules();
                        _rule.GroupRuleId = 0;
                        _rule.GroupId = GroupID;
                        _rule.RuleId = RuleID;
                    }

                    _dataAccess.BeginTransaction();

                    oldValue = _rule.RuleValue;
                    _rule.RuleValue = RuleValue;

                    if (_rule.GroupRuleId > 0) _rule.Update(_dataAccess);
                    else _rule.Insert(_dataAccess);

                    AuditValue = "Group-Rule (" + _group.GroupCode + "-" + _eRule.RuleCode + ") RuleValue (" + oldValue + ") is modified to " + RuleValue + " by [" + UserHostAddress + "]";
                    SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "", _dataAccess);

                    _dataAccess.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void UpdateInsertRuleValue(SmBuyerSupplierGroups _group, int GroupID, int RuleID, string RuleValue, Dal.DataAccess _dataAccess, string UserHostAddress)
        {

            string AuditValue = "", oldValue = "";
            try
            {
                if ((GroupID > 0) && (RuleID > 0))
                {
                    SmEsupplierRules _eRule = SmEsupplierRules.Load(RuleID);
                    //SmBuyerSupplierGroups _group = SmBuyerSupplierGroups.Load(GroupID);

                    SmBuyerSupplierRules _rule = SmBuyerSupplierRules.LoadByGroupRule(GroupID, RuleID);
                    if (_rule == null)
                    {
                        _rule = new SmBuyerSupplierRules();
                        _rule.GroupRuleId = 0;
                        _rule.GroupId = GroupID;
                        _rule.RuleId = RuleID;
                    }

                    oldValue = convert.ToString(_rule.RuleValue);
                    _rule.RuleValue = RuleValue;

                    if (_rule.GroupRuleId > 0) _rule.Update(_dataAccess);
                    else _rule.Insert(_dataAccess);

                    AuditValue = "Group-Rule (" + _group.GroupCode + "-" + _eRule.RuleCode + ") RuleValue (" + oldValue + ") is modified to " + RuleValue + " by [" + UserHostAddress + "]";
                    SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "", _dataAccess);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void DeleteGroupRuleLink(int GroupRuleID, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "";
            try
            {
                SmBuyerSupplierRules _rule = SmBuyerSupplierRules.Load(GroupRuleID);

                if (_rule != null)
                {
                    SmEsupplierRules _eRule = SmEsupplierRules.Load(_rule.SmEsupplierRules.Ruleid);
                    SmBuyerSupplierGroups _group = SmBuyerSupplierGroups.Load(_rule.SmBuyerSupplierGroups.GroupId);

                    _dataAccess.BeginTransaction();
                    _rule.Delete(_dataAccess);

                    AuditValue = "Group-Rule (" + _group.GroupCode + "-" + _eRule.RuleCode + ") Link deleted by [" + UserHostAddress + "]";
                    SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "", _dataAccess);
                    _dataAccess.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public SmBuyerSupplierLinkRuleCollection GetAllLinksByAddress(int Addressid, int RuleID)
        {
            try
            {
                SmBuyerSupplierLinkRuleCollection ruleCollection = new SmBuyerSupplierLinkRuleCollection();
                SmAddress _address = SmAddress.Load(Addressid);
                if (_address != null)
                {
                    SmBuyerSupplierLinkCollection collection = null;

                    if (convert.ToString(_address.AddrType).Trim().ToUpper() == "SUPPLIER")
                    {
                        collection = SmBuyerSupplierLink.Select_SM_BUYER_SUPPLIER_LINKs_By_SUPPLIERID(Addressid);
                    }
                    else
                    {
                        collection = SmBuyerSupplierLink.Select_SM_BUYER_SUPPLIER_LINKs_By_BUYERID(Addressid);
                    }

                    if (collection != null)
                    {
                        foreach (SmBuyerSupplierLink link in collection)
                        {
                            SmBuyerSupplierLinkRule _bsLinkRule = SmBuyerSupplierLinkRule.LoadByLinkRule(convert.ToInt(link.Linkid), RuleID);
                            if (_bsLinkRule != null)
                            {
                                ruleCollection.Add(_bsLinkRule);
                            }
                        }
                    }
                }

                return ruleCollection;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region /* Buyer Supplier Link Rules : Added Sayak */
        // COMMENTED BY SANJITA 06-JUN-16
        //public void Update_Link_Rule_Details(int LinkID, Dictionary<string, string> lstLinkDetails, string UserHostAddress, bool IsGroupChanged, List<SmBuyerSupplierLinkRule> _ListRulesCol)
        //{
        //    int oldActive = 0, newActive = 0;
        //    string BuyerLinkCode = "", SupplierLinkCode = "";
        //    Dal.DataAccess _dataAccess = new Dal.DataAccess();
        //    try
        //    {
        //        string _updatedData = "";
        //        SmBuyerSupplierLink _link = SmBuyerSupplierLink.Load(LinkID);
        //        if (_link != null)
        //        {
        //            SmAddress _buyer = SmAddress.Load(_link.BuyerAddress.Addressid);
        //            _dataAccess.BeginTransaction(); // Begin Transaction

        //            #region  UPDATE LINK FIELDS
        //            /* Upadated By Sanjita 24/02/2015 */
        //            SetUpdateLog(ref _updatedData, "Buyer Code", _buyer.AddrCode, lstLinkDetails["BUYER_CODE"]);
        //            _buyer.AddrCode = lstLinkDetails["BUYER_CODE"];
        //            _buyer.Update(_dataAccess);

        //            SetUpdateLog(ref _updatedData, "Buyer Email", _link.BuyerEmail, lstLinkDetails["BUYER_EMAIL"]);
        //            _link.BuyerEmail = lstLinkDetails["BUYER_EMAIL"];

        //            SetUpdateLog(ref _updatedData, "Supp Email", _link.SupplierEmail, lstLinkDetails["SUPPLIER_EMAIL"]);
        //            _link.SupplierEmail = lstLinkDetails["SUPPLIER_EMAIL"];

        //            SetUpdateLog(ref _updatedData, "Buyer Link Code", _link.BuyerLinkCode, lstLinkDetails["BUYER_LINK_CODE"]);
        //            _link.BuyerLinkCode = lstLinkDetails["BUYER_LINK_CODE"];

        //            SetUpdateLog(ref _updatedData, "Supp Link Code", _link.VendorLinkCode, lstLinkDetails["VENDOR_LINK_CODE"]);
        //            _link.VendorLinkCode = lstLinkDetails["VENDOR_LINK_CODE"];

        //            SetUpdateLog(ref _updatedData, "Supp Sender Code", _link.SuppSenderCode, lstLinkDetails["SUPP_SENDER_CODE"]);
        //            _link.SuppSenderCode = lstLinkDetails["SUPP_SENDER_CODE"];

        //            SetUpdateLog(ref _updatedData, "Supp Receiver Code", _link.SuppReceiverCode, lstLinkDetails["SUPP_RECEIVER_CODE"]);
        //            _link.SuppReceiverCode = lstLinkDetails["SUPP_RECEIVER_CODE"];

        //            SetUpdateLog(ref _updatedData, "Buyer Sender Code", _link.ByrSenderCode, lstLinkDetails["BYR_SENDER_CODE"]);
        //            _link.ByrSenderCode = lstLinkDetails["BYR_SENDER_CODE"];

        //            SetUpdateLog(ref _updatedData, "Buyer Receiver Code", _link.ByrReceiverCode, lstLinkDetails["BYR_RECEIVER_CODE"]);
        //            _link.ByrReceiverCode = lstLinkDetails["BYR_RECEIVER_CODE"];

        //            SetUpdateLog(ref _updatedData, "Buyer Mapping", _link.BuyerMapping, lstLinkDetails["BUYER_MAPPING"]);
        //            _link.BuyerMapping = lstLinkDetails["BUYER_MAPPING"];

        //            SetUpdateLog(ref _updatedData, "Supp Mapping", _link.SupplierMapping, lstLinkDetails["SUPPLIER_MAPPING"]);
        //            _link.SupplierMapping = lstLinkDetails["SUPPLIER_MAPPING"];

        //            SetUpdateLog(ref _updatedData, "Buyer Contact", _link.BuyerContact, lstLinkDetails["BUYER_CONTACT"]);
        //            _link.BuyerContact = lstLinkDetails["BUYER_CONTACT"];

        //            SetUpdateLog(ref _updatedData, "Supp Contact", _link.SupplierContact, lstLinkDetails["SUPPLIER_CONTACT"]);
        //            _link.SupplierContact = lstLinkDetails["SUPPLIER_CONTACT"];

        //            // Added by simmy
        //            SetUpdateLog(ref _updatedData, "Upload Filetype", _link.UploadFileType, lstLinkDetails["UPLOAD_FILE_TYPE"]);
        //            _link.UploadFileType = lstLinkDetails["UPLOAD_FILE_TYPE"];

        //            #region /* Path Settings */
        //            SetUpdateLog(ref _updatedData, "Import Path", _link.ImportPath, lstLinkDetails["IMPORT_PATH"]);
        //            _link.ImportPath = lstLinkDetails["IMPORT_PATH"];

        //            SetUpdateLog(ref _updatedData, "Export Path", _link.ExportPath, lstLinkDetails["EXPORT_PATH"]);
        //            _link.ExportPath = lstLinkDetails["EXPORT_PATH"];

        //            SetUpdateLog(ref _updatedData, "Vendor Import Path", _link.SuppImportPath, lstLinkDetails["SUPP_IMPORT_PATH"]);
        //            _link.SuppImportPath = lstLinkDetails["SUPP_IMPORT_PATH"];

        //            SetUpdateLog(ref _updatedData, "Vendor Export Path", _link.SuppExportPath, lstLinkDetails["SUPP_EXPORT_PATH"]);
        //            _link.SuppExportPath = lstLinkDetails["SUPP_EXPORT_PATH"];
        //            #endregion

        //            #region /* Mail Settings */
        //            SetUpdateLog(ref _updatedData, "Mail Subject", _link.MailSubject, lstLinkDetails["MAIL_SUBJECT"]);
        //            _link.MailSubject = lstLinkDetails["MAIL_SUBJECT"];

        //            SetUpdateLog(ref _updatedData, "Reply Mail", _link.ReplyEmail, lstLinkDetails["REPLY_EMAIL"]);
        //            _link.ReplyEmail = lstLinkDetails["REPLY_EMAIL"];

        //            // Updated on 30-MAY-2015
        //            SetUpdateLog(ref _updatedData, "CC Email", _link.CcEmail, lstLinkDetails["CC_EMAIL"]);
        //            _link.CcEmail = lstLinkDetails["CC_EMAIL"];

        //            SetUpdateLog(ref _updatedData, "BCC Email", _link.BccEmail, lstLinkDetails["BCC_EMAIL"]);
        //            _link.BccEmail = lstLinkDetails["BCC_EMAIL"];
        //            #endregion

        //            BuyerLinkCode = _link.BuyerLinkCode;
        //            SupplierLinkCode = _link.VendorLinkCode;

        //            #region /* Import-Export Settings */
        //            SetUpdateLog(ref _updatedData, "Import RFQ", convert.ToString(_link.ImportRfq), lstLinkDetails["IMPORT_RFQ"]);
        //            _link.ImportRfq = (short)convert.ToInt(lstLinkDetails["IMPORT_RFQ"]);

        //            SetUpdateLog(ref _updatedData, "Export RFQ", convert.ToString(_link.ExportRfq), lstLinkDetails["EXPORT_RFQ"]);
        //            _link.ExportRfq = (short)convert.ToInt(lstLinkDetails["EXPORT_RFQ"]);

        //            SetUpdateLog(ref _updatedData, "Import Quote", convert.ToString(_link.ImportQuote), lstLinkDetails["IMPORT_QUOTE"]);
        //            _link.ImportQuote = (short)convert.ToInt(lstLinkDetails["IMPORT_QUOTE"]);

        //            SetUpdateLog(ref _updatedData, "Export Quote", convert.ToString(_link.ExportQuote), lstLinkDetails["EXPORT_QUOTE"]);
        //            _link.ExportQuote = (short)convert.ToInt(lstLinkDetails["EXPORT_QUOTE"]);

        //            SetUpdateLog(ref _updatedData, "Import PO", convert.ToString(_link.ImportPo), lstLinkDetails["IMPORT_PO"]);
        //            _link.ImportPo = (short)convert.ToInt(lstLinkDetails["IMPORT_PO"]);

        //            SetUpdateLog(ref _updatedData, "Export PO", convert.ToString(_link.ExportPo), lstLinkDetails["EXPORT_PO"]);
        //            _link.ExportPo = (short)convert.ToInt(lstLinkDetails["EXPORT_PO"]);

        //            SetUpdateLog(ref _updatedData, "Export RFQAck", convert.ToString(_link.ExportRfqAck), lstLinkDetails["EXPORT_RFQ_ACK"]);
        //            _link.ExportRfqAck = (short)convert.ToInt(lstLinkDetails["EXPORT_RFQ_ACK"]);

        //            SetUpdateLog(ref _updatedData, "Import POAck", convert.ToString(_link.ExportPoAck), lstLinkDetails["EXPORT_PO_ACK"]);
        //            _link.ExportPoAck = (short)convert.ToInt(lstLinkDetails["EXPORT_PO_ACK"]);

        //            SetUpdateLog(ref _updatedData, "Export POC", convert.ToString(_link.ExportPoc), lstLinkDetails["EXPORT_POC"]);
        //            _link.ExportPoc = (short)convert.ToInt(lstLinkDetails["EXPORT_POC"]);

        //            SetUpdateLog(ref _updatedData, "Notify Buyer", convert.ToString(_link.NotifyBuyer), lstLinkDetails["NOTIFY_BUYER"]);
        //            _link.NotifyBuyer = (short)convert.ToInt(lstLinkDetails["NOTIFY_BUYER"]);

        //            SetUpdateLog(ref _updatedData, "Notify Supp", convert.ToString(_link.NotifySupplr), lstLinkDetails["NOTIFY_SUPPLR"]);
        //            _link.NotifySupplr = (short)convert.ToInt(lstLinkDetails["NOTIFY_SUPPLR"]);

        //            #endregion

        //            SetUpdateLog(ref _updatedData, "Default Price", convert.ToString(_link.DefaultPrice), lstLinkDetails["DEFAULT_PRICE"]);
        //            if (lstLinkDetails["DEFAULT_PRICE"].Trim() != "") _link.DefaultPrice = (float)Convert.ToSingle(lstLinkDetails["DEFAULT_PRICE"]);

        //            SetUpdateLog(ref _updatedData, "Supplier Web service Url", convert.ToString(_link.Supp_Web_Service_Url), lstLinkDetails["SUP_WEB_SERVICE_URL"]);
        //            if (lstLinkDetails["SUP_WEB_SERVICE_URL"].Trim() != "") _link.Supp_Web_Service_Url = lstLinkDetails["SUP_WEB_SERVICE_URL"];

        //            if (_link.IsActive != null) oldActive = (short)_link.IsActive;
        //            newActive = (short)convert.ToInt(lstLinkDetails["IS_ACTIVE"]);
        //            _link.IsActive = (short)convert.ToInt(lstLinkDetails["IS_ACTIVE"]);
        //            #endregion

        //            #region /* ADD PDF & EXCEL BUYER-SUPPLIER LINK */
        //            int GroupID = 0;
        //            SmBuyerSupplierGroups _group = null;
        //            if (Convert.ToString(lstLinkDetails["GROUP_ID"]).Trim() != "")
        //            {
        //                int.TryParse(lstLinkDetails["GROUP_ID"], out GroupID);

        //                if (GroupID == 0) { GroupID = convert.ToInt(_link.GroupId); }
        //                if (GroupID > 0)
        //                {
        //                    // Get Old Group Name
        //                    string _oldGrpName = "";
        //                    SmBuyerSupplierGroups _lnkGrpInfo = SmBuyerSupplierGroups.Load(convert.ToInt(_link.GroupId));
        //                    if (_lnkGrpInfo != null) _oldGrpName = convert.ToString(_lnkGrpInfo.GroupCode);
        //                    //

        //                    // Get Selected Group ID & Info
        //                    _group = SmBuyerSupplierGroups.Load(GroupID);

        //                    #region /* Update Buyer-Supplier File import export formats */
        //                    SetUpdateLog(ref _updatedData, "Group", _oldGrpName, _group.GroupCode);
        //                    _link.GroupId = _group.GroupId;

        //                    SetUpdateLog(ref _updatedData, "Buyer Format", _link.BuyerFormat, convert.ToString(_group.BuyerFormat));
        //                    _link.BuyerFormat = _group.BuyerFormat;

        //                    SetUpdateLog(ref _updatedData, "Supp Format", _link.VendorFormat, convert.ToString(_group.SupplierFormat));
        //                    _link.VendorFormat = _group.SupplierFormat;

        //                    SetUpdateLog(ref _updatedData, "Buyer Exp Format", _link.BuyerExportFormat, convert.ToString(_group.BuyerExportFormat));
        //                    _link.BuyerExportFormat = _group.BuyerExportFormat;

        //                    SetUpdateLog(ref _updatedData, "Supp Exp Format", _link.SupplierExportFormat, convert.ToString(_group.SupplierExportFormat));
        //                    _link.SupplierExportFormat = _group.SupplierExportFormat;
        //                    #endregion

        //                    if (_group.BuyerFormat == "EXCEL_RFQ")
        //                    {
        //                        SmXlsGroupMapping _grpMapping = SmXlsGroupMapping.Load_By_GroupID((int)_group.GroupId);
        //                        SmXlsBuyerLink _xlsLink = SmXlsBuyerLink.Select_SM_XLS_BUYER_LINKs_By_LINKID(_link.Linkid);

        //                        if (_xlsLink == null)
        //                        {
        //                            _xlsLink = new SmXlsBuyerLink();
        //                            _xlsLink.Buyerid = _link.BuyerAddress.Addressid;
        //                            _xlsLink.Supplierid = _link.VendorAddress.Addressid;
        //                            _xlsLink.BuyerSuppLinkid = _link.Linkid;

        //                            if (_grpMapping != null) _xlsLink.SmXlsGroupMapping = _grpMapping;
        //                            else
        //                            {
        //                                _grpMapping = new SmXlsGroupMapping();
        //                                _grpMapping.GroupId = _group.GroupId;
        //                                _grpMapping.Insert(_dataAccess);

        //                                _xlsLink.SmXlsGroupMapping = _grpMapping;
        //                            }
        //                            _xlsLink.Insert(_dataAccess);
        //                        }
        //                    }
        //                    else if (convert.ToString(_group.BuyerFormat).ToUpper() == "PDF" || convert.ToString(_group.SupplierFormat).ToUpper() == "PDF")
        //                    {
        //                        // Modified to include Supplier PDF Format too in Tab.
        //                        SmPdfMapping _pdfMapping = SmPdfMapping.LoadByGroup((int)_group.GroupId);
        //                        if (_pdfMapping == null)
        //                        {
        //                            _pdfMapping = new SmPdfMapping();
        //                            _pdfMapping.PdfMapid = GetNextKey("PDF_MAPID", "SM_PDF_MAPPING");
        //                            _pdfMapping.Groupid = _group.GroupId;
        //                            _pdfMapping.Insert(_dataAccess);

        //                            SmPdfBuyerLink _pdfLink = new SmPdfBuyerLink();
        //                            _pdfLink.Buyerid = _link.BuyerAddress.Addressid;
        //                            _pdfLink.BuyerSuppLinkid = _link.Linkid;
        //                            _pdfLink.Supplierid = _link.VendorAddress.Addressid;
        //                            _pdfLink.PdfMapid = _pdfMapping.PdfMapid;
        //                            _pdfLink.MapId = GetNextKey("MAP_ID", "SM_PDF_BUYER_LINK");
        //                            _pdfLink.Insert(_dataAccess);
        //                        }
        //                    }
        //                }
        //            }
        //            #endregion

        //            _link.Update(_dataAccess);

        //            string _AuditData = "";
        //            if (IsGroupChanged == true)
        //            {
        //                foreach (SmBuyerSupplierLinkRule _LinkRule in _ListRulesCol)
        //                {
        //                    if (_LinkRule.UpdateInsertVal == 0)
        //                    {
        //                        _LinkRule.Insert(_dataAccess);
        //                    }
        //                    else if (_LinkRule.UpdateInsertVal == 1)
        //                    {
        //                        _AuditData += _LinkRule.AuditValTxt + " ";
        //                        _LinkRule.Update(_dataAccess);
        //                    }
        //                }
        //            }

        //            if ((oldActive == 0) && (newActive == 1)) // link activated Resubmit error files
        //            {
        //                // Set AuditLog 
        //                if (_updatedData.Trim().Length > 0) SetAuditLog("LeSMonitor", "Buyer Supplier link (" + BuyerLinkCode + "-" + SupplierLinkCode + ") following fields updated by [" + UserHostAddress + "]. " + _updatedData + " " + _AuditData, "Updated", (Convert.ToString(_link.BuyerAddress.Addressid) + "-" + Convert.ToString(_link.VendorAddress.Addressid)), "", Convert.ToString(_link.BuyerAddress.Addressid), Convert.ToString(_link.VendorAddress.Addressid), _dataAccess);
        //                else SetAuditLog("LeSMonitor", "Buyer Supplier link (" + BuyerLinkCode + "-" + SupplierLinkCode + ") with no changes updated by [" + UserHostAddress + "]. " + _updatedData + " " + _AuditData, "Updated", (Convert.ToString(_link.BuyerAddress.Addressid) + "-" + Convert.ToString(_link.VendorAddress.Addressid)), "", Convert.ToString(_link.BuyerAddress.Addressid), Convert.ToString(_link.VendorAddress.Addressid), _dataAccess);

        //                SetAuditLog("LeSMonitor", "Buyer Supplier link (" + BuyerLinkCode + "-" + SupplierLinkCode + ") activated by [" + UserHostAddress + "].", "Updated", Convert.ToString(_link.BuyerAddress.Addressid) + "-" + Convert.ToString(_link.VendorAddress.Addressid), "", Convert.ToString(_link.BuyerAddress.Addressid), Convert.ToString(_link.VendorAddress.Addressid), _dataAccess);

        //                // Resubmit error file waiting for importing
        //                if (_dataAccess.CurrentTransaction != null) _dataAccess.CommitTransaction();
        //                ResubmitFiles((int)_link.BuyerAddress.Addressid, (int)_link.VendorAddress.Addressid, _link.BuyerLinkCode, _link.VendorLinkCode);

        //                // Check Is this link activated for eSupplierLite 
        //                if (GroupID > 0 && _group != null && convert.ToString(_group.SupplierFormat) == "ESUPPLIER_LITE")
        //                {
        //                    SmExternalUsers _user = SmExternalUsers.LoadByLinkID(_link.Linkid);
        //                    if (_user == null) AddNewLogin(convert.ToInt(_link.Linkid), UserHostAddress);
        //                }
        //            }
        //            else
        //            {
        //                if (oldActive == 1 && newActive == 0) SetAuditLog("LeSMonitor", "Buyer Supplier link (" + BuyerLinkCode + "-" + SupplierLinkCode + ") deactivated by [" + UserHostAddress + "]. " + _updatedData + " " + _AuditData, "Updated", (Convert.ToString(_link.BuyerAddress.Addressid) + "-" + Convert.ToString(_link.VendorAddress.Addressid)), "", Convert.ToString(_link.BuyerAddress.Addressid), Convert.ToString(_link.VendorAddress.Addressid), _dataAccess);
        //                else
        //                {
        //                    if (_updatedData.Trim().Length > 0) SetAuditLog("LeSMonitor", "Buyer Supplier link (" + BuyerLinkCode + "-" + SupplierLinkCode + ") following fields updated by [" + UserHostAddress + "]. " + _updatedData + " " + _AuditData, "Updated", (Convert.ToString(_link.BuyerAddress.Addressid) + "-" + Convert.ToString(_link.VendorAddress.Addressid)), "", Convert.ToString(_link.BuyerAddress.Addressid), Convert.ToString(_link.VendorAddress.Addressid), _dataAccess);
        //                    else SetAuditLog("LeSMonitor", "Buyer Supplier link (" + BuyerLinkCode + "-" + SupplierLinkCode + ") with no changes updated by [" + UserHostAddress + "]. " + _updatedData + " " + _AuditData, "Updated", (Convert.ToString(_link.BuyerAddress.Addressid) + "-" + Convert.ToString(_link.VendorAddress.Addressid)), "", Convert.ToString(_link.BuyerAddress.Addressid), Convert.ToString(_link.VendorAddress.Addressid), _dataAccess);
        //                }
        //                if (_dataAccess.CurrentTransaction != null) _dataAccess.CommitTransaction();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (_dataAccess.CurrentTransaction != null)
        //            _dataAccess.RollbackTransaction();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        _dataAccess._Dispose();
        //    }
        //}

        public string UpdateLinkRules(SmBuyerSupplierLink smLink, bool isGroupModified, bool isNewGroup, Dal.DataAccess _dataAccess)
        {
            string _updatedData = "";
            bool isNewConnection = false;
            try
            {
                if (_dataAccess == null)
                {
                    isNewConnection = true;
                    _dataAccess = new Dal.DataAccess();
                }

                if (smLink != null)
                {
                    int LinkID = convert.ToInt(smLink.Linkid);
                    int GroupID = convert.ToInt(smLink.GroupId);

                    if (isGroupModified)
                    {
                        SmBuyerSupplierLinkRule D_LinkRule = new SmBuyerSupplierLinkRule();
                        D_LinkRule.DeleteLinkRule(LinkID, _dataAccess);
                        List<SmBuyerSupplierLinkRule> _ListRulesCol = new List<SmBuyerSupplierLinkRule>();
                        if (isGroupModified)
                        {
                            //if (!isNewGroup) _ListRulesCol = GetGroupLinkRules(GroupID, LinkID);
                            _ListRulesCol = GetNewGroupLinkRules(GroupID, LinkID, smLink);
                        }

                        foreach (SmBuyerSupplierLinkRule _LinkRule in _ListRulesCol)
                        {
                            if (_LinkRule.UpdateInsertVal == 0)
                            {
                                _LinkRule.InheritRule = 1;
                                _LinkRule.Insert(_dataAccess);
                            }
                            else if (_LinkRule.UpdateInsertVal == 1)
                            {
                                _updatedData += _LinkRule.AuditValTxt + " ";
                                _LinkRule.Update(_dataAccess);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (isNewConnection) _dataAccess._Dispose();
            }
            return _updatedData.Trim();
        }

        public List<SmBuyerSupplierLinkRule> GetNewGroupLinkRules(int GroupID, int LinkID, SmBuyerSupplierLink smLink)
        {
            Dictionary<int, SmBuyerSupplierLinkRule> _ArrLisr = new Dictionary<int, SmBuyerSupplierLinkRule>();
            List<SmBuyerSupplierLinkRule> _ListRulesCol = new List<SmBuyerSupplierLinkRule>();
            DataSet dsBuyerDefault = null, dsSupplierDefault = null;
            if (smLink == null) smLink = SmBuyerSupplierLink.Load(LinkID);
            //SmBuyerSupplierLink _Link = SmBuyerSupplierLink.Load(convert.ToInt(LinkID));
            SmBuyerSupplierGroups _groups = SmBuyerSupplierGroups.Load(convert.ToInt(GroupID));
            if (_groups != null)
            {
                dsBuyerDefault = SmDefaultRules.SM_DEFAULT_RULES_Select_By_Address_GroupFormat(convert.ToInt(smLink.BuyerId), _groups.BuyerFormat);  // Buyer's Default Rules by ID & Format
                dsSupplierDefault = SmDefaultRules.SM_DEFAULT_RULES_Select_By_Address_GroupFormat(convert.ToInt(smLink.SupplierId), _groups.SupplierFormat);  // Supplier's Default Rules by ID & Format

            }
            if (dsBuyerDefault != null && dsBuyerDefault.Tables.Count > 0 && dsBuyerDefault.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow rBuyerRules in dsBuyerDefault.Tables[0].Rows)
                {
                    SmBuyerSupplierLinkRule _Lnkrule = null;
                    //SmBuyerSupplierLinkRule _Lnkrule = SmBuyerSupplierLinkRule.LoadByLinkRule(LinkID, convert.ToInt(rBuyerRules["RULE_ID"]));
                    SmEsupplierRules _eRule = SmEsupplierRules.Load(convert.ToInt(rBuyerRules["RULE_ID"]));
                    if (_Lnkrule == null)
                    {
                        _Lnkrule = new SmBuyerSupplierLinkRule();
                        _Lnkrule.RuleValue = rBuyerRules["RULE_VALUE"].ToString();
                        _Lnkrule.Linkid = LinkID;
                        _Lnkrule.Ruleid = convert.ToInt(rBuyerRules["RULE_ID"]);
                        _Lnkrule.UpdateInsertVal = 0;
                    }
                    //else
                    //{
                    //    _Lnkrule.UpdateInsertVal = 1;
                    //    _Lnkrule.AuditValTxt = "Link-Rule (" + _eRule.RuleCode + ") Rule value (" + _Lnkrule.RuleValue + ") is modified to (" + rBuyerRules["RULE_VALUE"].ToString() + ");";
                    //}
                    _Lnkrule.RuleValue = rBuyerRules["RULE_VALUE"].ToString();
                    _Lnkrule.UdateDate = DateTime.Now;
                    _ArrLisr.Add(convert.ToInt(rBuyerRules["RULE_ID"]), _Lnkrule);
                }
            }

            if (dsSupplierDefault != null && dsSupplierDefault.Tables.Count > 0 && dsSupplierDefault.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow rSupplierRules in dsSupplierDefault.Tables[0].Rows)
                {
                    SmBuyerSupplierLinkRule _Lnkrule = null;
                    // SmBuyerSupplierLinkRule _Lnkrule = SmBuyerSupplierLinkRule.LoadByLinkRule(LinkID, convert.ToInt(rSupplierRules["RULE_ID"]));
                    SmEsupplierRules _eRule = SmEsupplierRules.Load(convert.ToInt(rSupplierRules["RULE_ID"]));
                    if (_Lnkrule == null)
                    {
                        _Lnkrule = new SmBuyerSupplierLinkRule();
                        _Lnkrule.RuleValue = rSupplierRules["RULE_VALUE"].ToString();
                        _Lnkrule.Linkid = LinkID;
                        _Lnkrule.Ruleid = convert.ToInt(rSupplierRules["RULE_ID"]);
                        _Lnkrule.UpdateInsertVal = 0;
                    }
                    //else
                    //{
                    //    _Lnkrule.UpdateInsertVal = 1;
                    //    _Lnkrule.AuditValTxt = "Link-Rule (" + _eRule.RuleCode + ") Rule value (" + _Lnkrule.RuleValue + ") is modified to (" + rSupplierRules["RULE_VALUE"].ToString() + "); ";
                    //}

                    _Lnkrule.RuleValue = rSupplierRules["RULE_VALUE"].ToString();
                    _Lnkrule.UdateDate = DateTime.Now;

                    if (_ArrLisr.ContainsKey(convert.ToInt(rSupplierRules["RULE_ID"])))
                    {
                        _ArrLisr[convert.ToInt(rSupplierRules["RULE_ID"])] = _Lnkrule;
                    }
                    else
                    {
                        _ArrLisr.Add(convert.ToInt(rSupplierRules["RULE_ID"]), _Lnkrule);
                    }
                }
            }

            SmBuyerSupplierRulesCollection _RulesCol = SmBuyerSupplierRules.Select_SM_BUYER_SUPPLIER_RULESs_By_GROUPID(GroupID);
            foreach (SmBuyerSupplierRules _rule in _RulesCol)
            {
                SmBuyerSupplierLinkRule _Lnkrule = null;
                //SmBuyerSupplierLinkRule _Lnkrule = SmBuyerSupplierLinkRule.LoadByLinkRule(LinkID, convert.ToInt(_rule.RuleId));
                SmEsupplierRules _eRule = SmEsupplierRules.Load(_rule.RuleId);
                if (_Lnkrule == null)
                {
                    _Lnkrule = new SmBuyerSupplierLinkRule();
                    _Lnkrule.RuleValue = _rule.RuleValue;
                    _Lnkrule.Linkid = LinkID;
                    _Lnkrule.Ruleid = convert.ToInt(_rule.RuleId);
                    _Lnkrule.UpdateInsertVal = 0;
                }
                //else
                //{
                //    _Lnkrule.UpdateInsertVal = 1;
                //    _Lnkrule.AuditValTxt = "Link-Rule (" + _eRule.RuleCode + ") Rule value (" + _Lnkrule.RuleValue + ") is modified to (" + _rule.RuleValue + ");";
                //}
                _Lnkrule.RuleValue = _rule.RuleValue;
                _Lnkrule.UdateDate = DateTime.Now;
                if (_ArrLisr.ContainsKey(convert.ToInt(_rule.RuleId)))
                {
                    _ArrLisr[convert.ToInt(_rule.RuleId)] = _Lnkrule;
                }
                else
                {
                    _ArrLisr.Add(convert.ToInt(_rule.RuleId), _Lnkrule);
                }
            }

            // Get Collection Of All Rules
            foreach (var item in _ArrLisr)
            {
                _ListRulesCol.Add(item.Value);
            }

            return _ListRulesCol;
        }

        public List<SmBuyerSupplierLinkRule> GetGroupLinkRules(int GroupID, int LinkID)
        {
            SmBuyerSupplierRulesCollection _RulesCol = SmBuyerSupplierRules.Select_SM_BUYER_SUPPLIER_RULESs_By_GROUPID(GroupID);
            List<SmBuyerSupplierLinkRule> _ListRulesCol = new List<SmBuyerSupplierLinkRule>();
            foreach (SmBuyerSupplierRules _rule in _RulesCol)
            {
                SmBuyerSupplierLinkRule _Lnkrule = SmBuyerSupplierLinkRule.LoadByLinkRule(LinkID, convert.ToInt(_rule.RuleId));
                SmEsupplierRules _eRule = SmEsupplierRules.Load(_rule.RuleId);
                if (_Lnkrule == null)
                {
                    _Lnkrule = new SmBuyerSupplierLinkRule();
                    _Lnkrule.RuleValue = _rule.RuleValue;
                    _Lnkrule.Linkid = LinkID;
                    _Lnkrule.Ruleid = convert.ToInt(_rule.RuleId);
                    _Lnkrule.UpdateInsertVal = 0;
                }
                else
                {
                    _Lnkrule.UpdateInsertVal = 1;
                    _Lnkrule.AuditValTxt = "Link-Rule (" + _eRule.RuleCode + ") Rule value (" + _Lnkrule.RuleValue + ") is modified to (" + _rule.RuleValue + "); ";
                }
                _Lnkrule.RuleValue = _rule.RuleValue;
                _Lnkrule.UdateDate = DateTime.Now;
                _ListRulesCol.Add(_Lnkrule);
            }
            return _ListRulesCol;
        }

        public System.Data.DataSet Get_SMV_BuyerSupplierLinkRule(int LinkID)
        {
            SmBuyerSupplierLinkRule _obj = new SmBuyerSupplierLinkRule();
            System.Data.DataSet _ds = _obj.GET_SMV_LINK_RULE_Select_By_LinkID(LinkID);
            return _ds;
        }

        public System.Data.DataSet GET_ESUPPLIER_RULES_LIST_Without_LinkID(int LinkID)
        {
            SmBuyerSupplierLinkRule _obj = new SmBuyerSupplierLinkRule();
            System.Data.DataSet _ds = _obj.GET_ESUPPLIER_RULES_LIST_Without_LinkID(LinkID);
            return _ds;
        }

        public System.Data.DataSet GetBuyerSupplier_Save_LinkRule(int LinkID, int RuleID)
        {
            SmBuyerSupplierLinkRule _obj = new SmBuyerSupplierLinkRule();
            //System.Data.DataSet _ds = _obj.GetItemMapping((int)Session["SUPPLIERID"], BuyerID);
            System.Data.DataSet _ds = _obj.GET_Save_LINK_RULE_Select_By_LinkID(LinkID, RuleID);
            return _ds;
        }

        public void UpdateInsertLinkRuleValue(int LinkID, int RuleID, string RuleValue, string INHERIT_RULE, string UserHostAddress,bool IsAudit)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "", oldValue = "", old_Inharite_RULE = "";
            try
            {
                if ((LinkID > 0) && (RuleID > 0))
                {
                    SmEsupplierRules _eRule = SmEsupplierRules.Load(RuleID);
                    //SmBuyerSupplierGroups _group = SmBuyerSupplierGroups.Load(GroupID);
                    SmBuyerSupplierLink _LinkDetails = new SmBuyerSupplierLink();
                    _LinkDetails.Linkid = LinkID;
                    _LinkDetails.Load();

                    SmBuyerSupplierLinkRule _Lnkrule = SmBuyerSupplierLinkRule.LoadByLinkRule(LinkID, RuleID);
                    int isNew = 1;
                    if (_Lnkrule == null)
                    {
                        _Lnkrule = new SmBuyerSupplierLinkRule();
                        _Lnkrule.RuleValue = RuleValue;
                        _Lnkrule.Linkid = LinkID;
                        _Lnkrule.Ruleid = RuleID;
                        old_Inharite_RULE = convert.ToString(INHERIT_RULE);
                        isNew = 0;

                    }
                    else
                    {
                        old_Inharite_RULE = convert.ToString(_Lnkrule.InheritRule);
                    }
                    _dataAccess.BeginTransaction();

                    oldValue = _Lnkrule.RuleValue;
                    _Lnkrule.RuleValue = RuleValue;
                    _Lnkrule.UdateDate = DateTime.Now;
                    _Lnkrule.InheritRule = convert.ToInt(INHERIT_RULE);

                    if (isNew > 0) _Lnkrule.Update(_dataAccess);
                    else _Lnkrule.Insert(_dataAccess);
                    if (IsAudit)
                    {
                        AuditValue = "Link-Rule (" + _eRule.RuleCode + ") for (" + _LinkDetails.BuyerLinkCode + " - " + _LinkDetails.VendorLinkCode + "), RuleValue (" + oldValue + ") is modified to " + RuleValue + " InheritRule (" + old_Inharite_RULE + ") is modified to " + INHERIT_RULE + " by [" + UserHostAddress + "]";
                        SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "", _dataAccess);
                    }
                    _dataAccess.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public SmBuyerSupplierLinkCollection Get_Col_BuyerSupplerLink_By_Group(int GroupId)
        {
            SmBuyerSupplierLinkCollection _linkColl = SmBuyerSupplierLink.Select_SM_BUYER_SUPPLIER_LINKs_By_GROUPID(GroupId);
            return _linkColl;
        }

        public void AddLinkRule(int LinkID, int RULEID)
        {
            try
            {
                SmBuyerSupplierLinkRule _LinkRule = new SmBuyerSupplierLinkRule();
                DataSet _ds = _LinkRule.Get_BuyerSupplierLinkRule_LinkID_RuleID(LinkID, RULEID);
                if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                { }
                else
                {
                    _LinkRule.Linkid = LinkID;
                    _LinkRule.Ruleid = RULEID;
                    //_LinkRule.InheritRule = 0; // Default Value should be yes
                    _LinkRule.InheritRule = 1;
                    _LinkRule.RuleValue = "0";
                    _LinkRule.Insert();

                    SmEsupplierRules _Rule = new SmEsupplierRules();
                    _Rule.Ruleid = convert.ToInt(RULEID);
                    _Rule.Load();
                    SmBuyerSupplierLink _Link = new SmBuyerSupplierLink();
                    _Link.Linkid = LinkID;
                    _Link.Load();
                    int BuyerId = convert.ToInt(_Link.BuyerId);
                    int SupplierId = convert.ToInt(_Link.SupplierId);

                    SmAddress _addSupplier = new SmAddress();
                    _addSupplier.Addressid = SupplierId;
                    _addSupplier.Load();
                    SmAddress _addBuyer = new SmAddress();
                    _addBuyer.Addressid = BuyerId;
                    _addBuyer.Load();

                    string AuditValue = "New Link Rule Code " + _Rule.RuleCode + " Added for Buyer Code : " + _addBuyer.AddrCode + " and Supplier Code : " + _addSupplier.AddrCode + " by : " + Convert.ToString(HttpContext.Current.Session["UserHostServer"]);
                    SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void DeleteLinkRule(int LINKID, Dal.DataAccess _dataAccess)
        {
            try
            {
                SmBuyerSupplierLinkRuleCollection linkRuleColl = SmBuyerSupplierLinkRule.LoadByLinkID(LINKID);
                foreach (SmBuyerSupplierLinkRule linkrule in linkRuleColl)
                {
                    linkrule.Delete(_dataAccess);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region /* XLS Buyer Config */
        public System.Data.DataSet Get_XLS_Mapping()
        {
            DataSet _ds = SmXlsBuyerLink.Select_SM_XLS_BUYER_LINKs();

            DataSet dsClone = _ds.Copy();
            dsClone.Tables[0].Columns.Add("SampleFile");
            foreach (DataRow row in dsClone.Tables[0].Rows)
            {
                foreach (DataColumn column in dsClone.Tables[0].Columns)
                {
                    row.SetField("SampleFile", GetFiles(Convert.ToString(row["GROUP_CODE"])));
                }
            }
            return dsClone;
        }

        public string GetFiles(string GROUP_CODE)
        {
            string _result = "";
            try
            {
                string cDirPath = Convert.ToString(ConfigurationManager.AppSettings["REF_FILEPATH"]) + "\\XLS\\" + GROUP_CODE;
                if (Directory.Exists(cDirPath))
                {
                    DirectoryInfo d = new DirectoryInfo(cDirPath);
                    FileInfo[] f = d.GetFiles("*.*");
                    if (f != null && f.Length > 0)
                    {
                        _result = Path.GetFileName(f[0].FullName);
                    }
                }
            }
            catch (Exception ex)
            { throw; }
            return _result;
        }

       


        public void Update_XLS_Mapping(int _xlsBuyerMapid, int GroupID, string MAP_CELL1, string MAP_CELL1_VAL1, string MAP_CELL1_VAL2, string MAP_CELL2, string MAP_CELL2_VAL, string MAP_CELL_NODISC, string MAP_CELL_NODISC_VAL, string DOC_TYPE, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                SmXlsBuyerLink _xlsLink = SmXlsBuyerLink.Load(_xlsBuyerMapid);

                _dataAccess.BeginTransaction();
                _xlsLink.MapCell1 = MAP_CELL1.Trim();
                _xlsLink.MapCell1Val1 = MAP_CELL1_VAL1.Trim();
                _xlsLink.MapCell1Val2 = MAP_CELL1_VAL2.Trim();
                _xlsLink.MapCell2 = MAP_CELL2.Trim();
                _xlsLink.MapCell2Val = MAP_CELL2_VAL.Trim();
                _xlsLink.MapCellNoDisc = MAP_CELL_NODISC.Trim();
                _xlsLink.MapCellNoDiscVal = MAP_CELL_NODISC_VAL.Trim();
                _xlsLink.DocType = DOC_TYPE;

                SmAddress _buyer = SmAddress.Load(_xlsLink.Buyerid);
                SmAddress _supplier = SmAddress.Load(_xlsLink.Supplierid);
                string cSppCode = (_supplier != null) ? _supplier.AddrCode : "";
                string cSppID = (_supplier != null) ? Convert.ToString(_supplier.Addressid) : "";

                if (GroupID > 0)
                {
                    SmXlsGroupMapping _grpMapping = SmXlsGroupMapping.Load_By_GroupID(GroupID);
                    if (_grpMapping == null)
                    {
                        _grpMapping = new SmXlsGroupMapping();
                        _grpMapping.GroupId = GroupID;
                        _grpMapping.XlsMapCode = "";
                        _grpMapping.Insert(_dataAccess);
                    }
                    _xlsLink.SmXlsGroupMapping = _grpMapping;
                }
                _xlsLink.Update(_dataAccess);

                SetAuditLog("LeSMonitor", "XLS Buyer Supplier link (" + _buyer.AddrCode + "-" + cSppCode + ") updated by [" + UserHostAddress + "]. ", "Updated", _buyer.Addressid + "-" + cSppID, "", _buyer.Addressid.ToString(), cSppID, _dataAccess);

                _dataAccess.CommitTransaction();
                SetLog("Update_XLS_Mapping Successful");
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                SetLog("Update_XLS_Mapping Error :" + ex.Message);
                throw;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void Add_XLS_Mapping(int BuyerID, int SupplierID, int GroupID, string MAP_CELL1, string MAP_CELL1_VAL1, string MAP_CELL1_VAL2, string MAP_CELL2, string MAP_CELL2_VAL, string MAP_CELL_NODISC, string MAP_CELL_NODISC_VAL, string DOC_TYPE, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                SmXlsBuyerLink _xlsLink = new SmXlsBuyerLink();
                _dataAccess.BeginTransaction();

                SmBuyerSupplierLink _link = new SmBuyerSupplierLink();
                _link.BuyerAddress = SmAddress.Load(BuyerID);
                _link.VendorAddress = SmAddress.Load(SupplierID);
                _link.BuyerFormat = "EXCEL_RFQ";
                _link.VendorFormat = "EXCEL";
                _link.Load_byBuyerSupplierFormat();

                _xlsLink.BuyerSuppLinkid = _link.Linkid;
                _xlsLink.Buyerid = BuyerID;
                _xlsLink.Supplierid = SupplierID;
                _xlsLink.MapCell1 = MAP_CELL1.Trim();
                _xlsLink.MapCell1Val1 = MAP_CELL1_VAL1.Trim();
                _xlsLink.MapCell1Val2 = MAP_CELL1_VAL2.Trim();
                _xlsLink.MapCell2 = MAP_CELL2.Trim();
                _xlsLink.MapCell2Val = MAP_CELL2_VAL.Trim();
                _xlsLink.MapCellNoDisc = MAP_CELL_NODISC.Trim();
                _xlsLink.MapCellNoDiscVal = MAP_CELL_NODISC_VAL.Trim();
                _xlsLink.DocType = DOC_TYPE;
                SmAddress _buyer = SmAddress.Load(_xlsLink.Buyerid);
                SmAddress _supplier = SmAddress.Load(_xlsLink.Supplierid);
                if (GroupID > 0)
                {
                    SmXlsGroupMapping _grpMapping = SmXlsGroupMapping.Load_By_GroupID(GroupID);

                    if (_grpMapping != null) _xlsLink.SmXlsGroupMapping = _grpMapping;
                    else
                    {
                        _grpMapping = new SmXlsGroupMapping();
                        _grpMapping.GroupId = GroupID;
                        _grpMapping.Insert(_dataAccess);
                    }

                    _xlsLink.SmXlsGroupMapping = _grpMapping;
                }

                _xlsLink.Insert(_dataAccess);

                SetAuditLog("LeSMonitor", "New XLS Buyer Supplier link (" + _buyer.AddrCode + "-" + _supplier.AddrCode + ") added by [" + UserHostAddress + "]. ", "Updated", "", _buyer.Addressid.ToString() + "-" + _supplier.Addressid.ToString(), _buyer.Addressid.ToString(), _supplier.Addressid.ToString(), _dataAccess);

                _dataAccess.CommitTransaction();
                SetLog("Add_XLS_Mapping successful.");
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                SetLog("Add_XLS_Mapping Error :" + ex.Message);
                throw;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void RemoveXLSMapping(int XLSMapID, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                _dataAccess.CreateConnection();
                _dataAccess.BeginTransaction();

                SmBuyerSupplierGroups _grp = null;
                SmXlsBuyerLink _xlsLink = SmXlsBuyerLink.Load(XLSMapID);
                if (_xlsLink != null && _xlsLink.XlsBuyerMapid > 0)
                {
                    int ExcelMapID = (int)_xlsLink._excelMapid;
                    _grp = SmBuyerSupplierGroups.Load(_xlsLink.SmXlsGroupMapping.GroupId);

                    SmXlsBuyerLinkCollection _xlsLinkColl = SmXlsBuyerLink.Select_SM_XLS_BUYER_LINKs_By_EXCEL_MAPID(ExcelMapID);
                    if (_xlsLinkColl.Count == 1 && _xlsLinkColl[0].XlsBuyerMapid == _xlsLink.XlsBuyerMapid)
                    {
                        SmXlsGroupMapping _grpMapping = SmXlsGroupMapping.Load(ExcelMapID);
                        _grpMapping.Delete(_dataAccess);
                    }

                    _xlsLink.Delete(_dataAccess);
                    SetAuditLog("LeSMonitor", "XLS Mapping for group '" + _grp.GroupCode + "' deleted by [" + UserHostAddress + "]", "Updated", "", "-", "", "", _dataAccess);
                }
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        #region XLS Map code

        //ADDED BY Kalpita on 04/01/2018
        public System.Data.DataSet Get_XLS_Mapping_AddressId(string AddressId, string ADDRTYPE)
        {
            return SmXlsBuyerLink.Select_SM_XLS_BUYER_LINK_AddressId(AddressId, ADDRTYPE);
        }

        //ADDED BY Kalpita on 04/01/2018
        public System.Data.DataSet Get_XLS_Mapping_Wizard(string AddressId)
        {
            return SmXlsBuyerLink.Get_ExcelBuyerLink_Wiz(AddressId);
        }

        //ADDED BY Kalpita on 22/02/2018
        public System.Data.DataSet Get_XLS_Mapping_MapCode()
        {
            return SmXlsBuyerLink.SM_XLS_BUYER_LINK_MapCode();
        }

        public string SetXLSMapping_New(int Mapid, string TemplatePath, string SessionID)
        {
            string downloadPath = TemplatePath;
            DataSet _dsGroups = new DataSet();
            try
            {
                string temp = System.Configuration.ConfigurationManager.AppSettings["DOWNLOAD_ATTACHMENT"];  temp = temp + "\\" + SessionID;
                if (!Directory.Exists(temp)) Directory.CreateDirectory(temp);
                if (File.Exists(TemplatePath))
                {
                    SmXlsBuyerLink _Xlink = SmXlsBuyerLink.Load(Mapid);
                    if (_Xlink != null)
                    {
                        SmXlsGroupMapping obj = SmXlsGroupMapping.Load(_Xlink.SmXlsGroupMapping.ExcelMapid);
                        if (obj != null)
                        {
                            downloadPath = temp + "\\" + _Xlink.FormatMapCode + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssff") + ".xls";
                            License _license = new License(); _license.SetLicense("Aspose.Total.lic");
                            Workbook _xls = new Workbook(TemplatePath);
                            FindOptions _option = new FindOptions();
                            _option.CaseSensitive = true;
                            _option.LookAtType = LookAtType.EntireContent;
                            _option.SeachOrderByRows = true;

                            #region /* Set Values */
                            Worksheet sht = _xls.Worksheets[0];
                            Cell _excelID = _xls.Worksheets[0].Cells.Find("MAP_CODE", null, _option);
                            if (_excelID != null)
                            {
                                _xls.Worksheets[0].Cells[_excelID.Row, _excelID.Column + 1].Value = _Xlink.FormatMapCode;
                                _xls.Worksheets[0].Cells[_excelID.Row, _excelID.Column + 2].Value = _Xlink.XlsBuyerMapid;
                            }
                            else
                            {
                                SmBuyerSupplierGroups _group = SmBuyerSupplierGroups.Load(obj.GroupId);
                                Cell _grpID = _xls.Worksheets[0].Cells.Find("GROUP", null, _option);
                                _xls.Worksheets[0].Cells[_grpID.Row, _grpID.Column + 1].Value = _group.GroupCode;
                                _xls.Worksheets[0].Cells[_grpID.Row, _grpID.Column + 2].Value = _group.GroupId;
                            }

                            SetCellValue(sht, "SECTION_ROW_START", convert.ToInt(obj.SectionRowStart));
                            SetCellValue(sht, "ITEM_ROW_START", convert.ToInt(obj.ItemRowStart));
                            SetCellValue(sht, "SKIP_ROWS_BEF_ITEM", convert.ToInt(obj.SkipRowsBefItem));
                            SetCellValue(sht, "SKIP_ROWS_AFT_SECTION", convert.ToInt(obj.SkipRowsAftSection));
                            SetCellValue(sht, "CELL_VRNO", obj.CellVrno);
                            SetCellValue(sht, "CELL_RFQ_DT", obj.CellRfqDt);
                            SetCellValue(sht, "CELL_VESSEL", obj.CellVessel);
                            SetCellValue(sht, "CELL_PORT", obj.CellPort);
                            SetCellValue(sht, "CELL_LATE_DT", obj.CellLateDt);
                            SetCellValue(sht, "CELL_SUPP_REF", obj.CellSuppRef);
                            SetCellValue(sht, "CELL_VALID_UPTO", obj.CellValidUpto);
                            SetCellValue(sht, "CELL_CURR_CODE", obj.CellCurrCode);
                            SetCellValue(sht, "CELL_CONTACT", obj.CellContact);
                            SetCellValue(sht, "CELL_PAY_TERMS", obj.CellPayTerms);
                            SetCellValue(sht, "CELL_DEL_TERMS", obj.CellDelTerms);
                            SetCellValue(sht, "CELL_BUYER_REMARKS", obj.CellBuyerRemarks);
                            SetCellValue(sht, "CELL_SUPLR_REMARKS", obj.CellSuplrRemarks);
                            SetCellValue(sht, "COL_SECTION", obj.ColSection);
                            SetCellValue(sht, "COL_ITEMNO", obj.ColItemno);
                            SetCellValue(sht, "COL_ITEM_REFNO", obj.ColItemRefno);
                            SetCellValue(sht, "COL_ITEM_NAME", obj.ColItemName);
                            SetCellValue(sht, "COL_ITEM_QTY", obj.ColItemQty);
                            SetCellValue(sht, "COL_ITEM_UNIT", obj.ColItemUnit);
                            SetCellValue(sht, "COL_ITEM_PRICE", obj.ColItemPrice);
                            SetCellValue(sht, "COL_ITEM_DISCOUNT", obj.ColItemDiscount);
                            SetCellValue(sht, "COL_ITEM_ALT_QTY", obj.ColItemAltQty);
                            SetCellValue(sht, "COL_ITEM_ALT_UNIT", obj.ColItemAltUnit);
                            SetCellValue(sht, "COL_ITEM_ALT_PRICE", obj.ColItemAltPrice);
                            SetCellValue(sht, "COL_ITEM_DEL_DAYS", obj.ColItemDelDays);
                            SetCellValue(sht, "COL_ITEM_REMARKS", obj.ColItemRemarks);
                            SetCellValue(sht, "COL_ITEM_CURR", obj.ColItemCurr);
                            SetCellValue(sht, "COL_ITEM_TOTAL", obj.ColItemTotal);
                            SetCellValue(sht, "ACTIVE_SHEET", convert.ToInt(obj.ActiveSheet));
                            SetCellValue(sht, "EXIT_FOR_NOITEM", convert.ToInt(obj.ExitForNoitem));
                            SetCellValue(sht, "DYN_SUP_RMRK_OFFSET", convert.ToInt(obj.DynSupRmrkOffset));
                            SetCellValue(sht, "Override_ALT_QTY", convert.ToInt(obj.OverrideAltQty));
                            SetCellValue(sht, "SKIP_HIDDEN_ROWS", convert.ToInt(obj.SkipHiddenRows));
                            SetCellValue(sht, "ITEM_DISC_PERCNT", convert.ToInt(obj.ItemDiscPercnt));
                            SetCellValue(sht, "APPLY_TOTAL_FORMULA", convert.ToInt(obj.ApplyTotalFormula));
                            SetCellValue(sht, "READ_ITEM_REMARKS_UPTO_NO", convert.ToInt(obj.ReadItemRemarksUptoNo));
                            SetCellValue(sht, "COL_ITEM_BUYER_REMARKS", obj.ColItemBuyerRemarks);
                            SetCellValue(sht, "ADD_TO_VRNO", obj.AddToVrno);
                            SetCellValue(sht, "REMOVE_FROM_VRNO", obj.RemoveFromVrno);
                            SetCellValue(sht, "SKIP_ROWS_AFT_ITEM", convert.ToInt(obj.SkipRowsAftItem));
                            SetCellValue(sht, "ITEM_NO_AS_ROWNO", convert.ToInt(obj.ItemNoAsRowno));
                            SetCellValue(sht, "COL_ITEM_COMMENTS", obj.ColItemComments);
                            SetCellValue(sht, "CELL_VSL_IMONO", obj.CellVslImono);
                            SetCellValue(sht, "CELL_PORT_NAME", obj.CellPortName);
                            SetCellValue(sht, "CELL_DOC_TYPE", obj.CellDocType);
                            SetCellValue(sht, "COL_ITEM_SUPP_REFNO", obj.ColItemSuppRefno);
                            SetCellValue(sht, "CELL_SUPP_EXP_DT", obj.CellSuppExpDt);
                            SetCellValue(sht, "CELL_SUPP_LATE_DT", obj.CellSuppLateDt);
                            SetCellValue(sht, "CELL_SUPP_LEAD_DAYS", obj.CellSuppLeadDays);
                            SetCellValue(sht, "CELL_BYR_COMPANY", obj.CellByrCompany);
                            SetCellValue(sht, "CELL_BYR_CONTACT", obj.CellByrContact);
                            SetCellValue(sht, "CELL_BYR_EMAIL", obj.CellByrEmail);
                            SetCellValue(sht, "CELL_BYR_PHONE", obj.CellByrPhone);
                            SetCellValue(sht, "CELL_BYR_MOB", obj.CellByrMob);
                            SetCellValue(sht, "CELL_BYR_FAX", obj.CellByrFax);
                            SetCellValue(sht, "CELL_SUPP_COMPANY", obj.CellSuppCompany);
                            SetCellValue(sht, "CELL_SUPP_CONTACT", obj.CellSuppContact);
                            SetCellValue(sht, "CELL_SUPP_EMAIL", obj.CellSuppEmail);
                            SetCellValue(sht, "CELL_SUPP_PHONE", obj.CellSuppPhone);
                            SetCellValue(sht, "CELL_SUPP_MOB", obj.CellSuppMob);
                            SetCellValue(sht, "CELL_SUPP_FAX", obj.CellSuppFax);
                            SetCellValue(sht, "CELL_FREIGHT_AMT", obj.CellFreightAmt);
                            SetCellValue(sht, "CELL_OTHER_AMT", obj.CellOtherAmt);

                            SetCellValue(sht, "CELL_DISC_PROVSN", obj.CellDiscProvsn);
                            SetCellValue(sht, "DISC_PROVSN_VALUE", obj.DiscProvsnValue);
                            SetCellValue(sht, "ALT_ITEM_START_OFFSET", convert.ToInt(obj.AltItemStartOffset));
                            SetCellValue(sht, "ALT_ITEM_COUNT", convert.ToInt(obj.AltItemCount));
                            SetCellValue(sht, "CELL_RFQ_TITLE", obj.CellRfqTitle);
                            SetCellValue(sht, "CELL_RFQ_DEPT", obj.CellRfqDept);
                            SetCellValue(sht, "CELL_EQUIP_NAME", obj.CellEquipName);
                            SetCellValue(sht, "CELL_EQUIP_TYPE", obj.CellEquipType);
                            SetCellValue(sht, "CELL_EQUIP_MAKER", obj.CellEquipMaker);
                            SetCellValue(sht, "CELL_EQUIP_SERNO", obj.CellEquipSrno);
                            SetCellValue(sht, "CELL_EQUIP_DTLS", obj.CellEquipDtls);
                            SetCellValue(sht, "CELL_MSGNO", obj.CellMsgNo);
                            SetCellValue(sht, "DYN_SUP_FREIGHT_OFFSET", convert.ToInt(obj.DynSupFreightOffset));
                            SetCellValue(sht, "DYN_OTHERCOST_OFFSET", convert.ToInt(obj.DynOtherCostOffset));
                            SetCellValue(sht, "DYN_SUP_CURR_OFFSET", convert.ToInt(obj.DynSupCurrOffset));
                            SetCellValue(sht, "DYN_BYR_RMRK_OFFSET", convert.ToInt(obj.DynBuyRmrkOffset));
                            SetCellValue(sht, "MULTILINE_ITEM_DESCR", convert.ToInt(obj.MultiLineDynItemDesc));
                            SetCellValue(sht, "EXCEL_NAME_MANAGER", convert.ToString(obj.ExcelNameMgr));
                            SetCellValue(sht, "DECIMAL_SEPARATOR", convert.ToString(obj.DecimalSeprator));
                            SetCellValue(sht, "DEFAULT_UOM", convert.ToString(obj.DefaultUMO));
                            SetCellValue(sht, "DYN_HDR_DISCOUNT_OFFSET", convert.ToInt(obj.DynHdrDiscountOffset));
                            SetCellValue(sht, "REMOVE_FROM_VESSEL_NAME", obj.REMOVE_FROM_VESSEL_NAME);
                            SetCellValue(sht, "CELL_BYR_ADDR1", obj.CELL_BYR_ADDR1);
                            SetCellValue(sht, "CELL_BYR_ADDR2", obj.CELL_BYR_ADDR2);
                            SetCellValue(sht, "CELL_SUPP_ADDR1", obj.CELL_SUPP_ADDR1);
                            SetCellValue(sht, "CELL_SUPP_ADDR2", obj.CELL_SUPP_ADDR2);
                            SetCellValue(sht, "CELL_BILL_COMPANY", obj.CELL_BILL_COMPANY);
                            SetCellValue(sht, "CELL_BILL_CONTACT", obj.CELL_BILL_CONTACT);
                            SetCellValue(sht, "CELL_BILL_EMAIL", obj.CELL_BILL_EMAIL);
                            SetCellValue(sht, "CELL_BILL_PHONE", obj.CELL_BILL_PHONE);
                            SetCellValue(sht, "CELL_BILL_MOB", obj.CELL_BILL_MOB);
                            SetCellValue(sht, "CELL_BILL_FAX", obj.CELL_BILL_FAX);
                            SetCellValue(sht, "CELL_BILL_ADDR1", obj.CELL_BILL_ADDR1);
                            SetCellValue(sht, "CELL_BILL_ADDR2", obj.CELL_BILL_ADDR2);
                            SetCellValue(sht, "CELL_SHIP_COMPANY", obj.CELL_SHIP_COMPANY);
                            SetCellValue(sht, "CELL_SHIP_CONTACT", obj.CELL_SHIP_CONTACT);
                            SetCellValue(sht, "CELL_SHIP_EMAIL", obj.CELL_SHIP_EMAIL);
                            SetCellValue(sht, "CELL_SHIP_PHONE", obj.CELL_SHIP_PHONE);
                            SetCellValue(sht, "CELL_SHIP_MOB", obj.CELL_SHIP_MOB);
                            SetCellValue(sht, "CELL_SHIP_FAX", obj.CELL_SHIP_FAX);
                            SetCellValue(sht, "CELL_SHIP_ADDR1", obj.CELL_SHIP_ADDR1);
                            SetCellValue(sht, "CELL_SHIP_ADDR2", obj.CELL_SHIP_ADDR2);
                            SetCellValue(sht, "CELL_ORDER_IDENTIFIER", obj.CELL_ORDER_IDENTIFIER);
                            SetCellValue(sht, "CELL_SUPP_QUOTE_DT", obj.CELL_SUPP_QUOTE_DT);
                            SetCellValue(sht, "COL_ITEM_ALT_NAME", obj.COL_ITEM_ALT_NAME);
                            SetCellValue(sht, "CELL_ETA_DATE", obj.CELL_ETA_DATE);
                            SetCellValue(sht, "CELL_ETD_DATE", obj.CELL_ETD_DATE);

                            #endregion
                            _xls.Save(downloadPath);
                        }
                        else { throw new Exception("No Mapping Found!"); }
                    }
                }
            }
            catch (Exception ex)
            {
                SetLog(ex.StackTrace);
                throw ex;
            }
            return downloadPath;
        }

        public void RemoveXLSMapping_New(int XLSMapID, string UserHostAddress)
        {
            string cMapCode = "";
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                SmXlsBuyerLink _xlsLink = SmXlsBuyerLink.Load(XLSMapID);
                if (_xlsLink != null && _xlsLink.XlsBuyerMapid > 0)
                {
                    int ExcelMapID = (int)_xlsLink._excelMapid;
                    cMapCode = _xlsLink.FormatMapCode;

                    SmXlsBuyerLinkCollection _xlsLinkColl = SmXlsBuyerLink.Select_SM_XLS_BUYER_LINKs_By_EXCEL_MAPID(ExcelMapID);
                    _dataAccess.BeginTransaction();
                    if (_xlsLinkColl.Count == 1 && _xlsLinkColl[0].XlsBuyerMapid == _xlsLink.XlsBuyerMapid)
                    {
                        SmXlsGroupMapping _grpMapping = SmXlsGroupMapping.Load_new(ExcelMapID, _dataAccess);
                        _grpMapping.Delete(_dataAccess);
                    }
                    _xlsLink.Delete(_dataAccess);
                    SetAuditLog("LeSMonitor", "XLS Mapping for group '" + cMapCode + "' deleted by [" + UserHostAddress + "]", "Updated", "", "-", "", "", _dataAccess);
                }
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void SaveXLSMapping_New(Dictionary<string, string> slXMapDet, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                SmAddress _buyer = null; SmAddress _supplier = null;
                SmXlsBuyerLink _xlsLink = new SmXlsBuyerLink();
                int MapID = Convert.ToInt32(slXMapDet["MAPID"]);
                if (MapID > 0)
                {
                    _xlsLink = SmXlsBuyerLink.Load(MapID);
                    if (_xlsLink != null)
                    {
                        _buyer = SmAddress.Load(_xlsLink.Buyerid);
                        _supplier = SmAddress.Load(_xlsLink.Supplierid);
                    }
                }
                _dataAccess.BeginTransaction();
                _xlsLink.DocType = (slXMapDet.ContainsKey("DOC_TYPE")) ? slXMapDet["DOC_TYPE"].Trim() : null;
                _xlsLink.MapCell1 = (slXMapDet.ContainsKey("MAP_CELL1")) ? slXMapDet["MAP_CELL1"].Trim() : null;
                _xlsLink.MapCell1Val1 = (slXMapDet.ContainsKey("MAP_CELL1_VAL1")) ? slXMapDet["MAP_CELL1_VAL1"].Trim() : null;
                _xlsLink.MapCell1Val2 = (slXMapDet.ContainsKey("MAP_CELL1_VAL2")) ? slXMapDet["MAP_CELL1_VAL2"].Trim() : null;
                _xlsLink.MapCell2 = (slXMapDet.ContainsKey("MAP_CELL2")) ? slXMapDet["MAP_CELL2"].Trim() : null;
                _xlsLink.MapCell2Val = (slXMapDet.ContainsKey("MAP_CELL2_VAL")) ? slXMapDet["MAP_CELL2_VAL"].Trim() : null;
                _xlsLink.MapCellNoDisc = (slXMapDet.ContainsKey("MAP_CELL_NODISC")) ? slXMapDet["MAP_CELL_NODISC"].Trim() : null;
                _xlsLink.MapCellNoDiscVal = (slXMapDet.ContainsKey("MAP_CELL_NODISC_VAL")) ? slXMapDet["MAP_CELL_NODISC_VAL"].Trim() : null;
                _xlsLink.FormatMapCode = (slXMapDet.ContainsKey("FORMAT_MAPCODE")) ? slXMapDet["FORMAT_MAPCODE"].Trim() : null;
                _xlsLink.Update(_dataAccess);
                SetAuditLog("LeSMonitor", "XLS Buyer Config(" + _buyer.AddrCode + ") updated by [" + UserHostAddress + "]. ", "Updated", _buyer.Addressid.ToString(), "", _buyer.Addressid.ToString(), string.Empty, _dataAccess);
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void CopyXLSMapping_New(int BUYERID, List<string> slXMapDet, string UserHostAddress)
        {
            string cMapCode = ""; SmBuyerSupplierLink _bsLnk = null;
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                SmAddress _buyer = SmAddress.Load(BUYERID);
                _dataAccess.BeginTransaction();
                for (int k = 0; k < slXMapDet.Count; k++)
                {
                    SmXlsBuyerLink _xblnkobj = SmXlsBuyerLink.Load_new(Convert.ToInt32(slXMapDet[k]), _dataAccess);
                    if (_xblnkobj != null)
                    {
                        SmXlsGroupMapping _eMapobj = SmXlsGroupMapping.Load_new(_xblnkobj.SmXlsGroupMapping.ExcelMapid, _dataAccess);
                        if (_xblnkobj.BuyerSuppLinkid != null) { _bsLnk = SmBuyerSupplierLink.Load_new(_xblnkobj.BuyerSuppLinkid, _dataAccess); cMapCode = _bsLnk.BuyerFormat + "_" + _buyer.AddrCode; }
                        else
                        {
                            cMapCode = _xblnkobj.FormatMapCode.Split('_')[0] + "_" + _buyer.AddrCode;
                        }
                        string cFormatMapCode = SmXlsBuyerLink.GetNextFormatMapCode(_xblnkobj.DocType, cMapCode, _dataAccess);
                        SmXlsBuyerLink _xlslnk = _xblnkobj;
                        _xlslnk.XlsBuyerMapid = GetNextKey("XLS_BUYER_MAPID", "SM_XLS_BUYER_LINK", _dataAccess);
                        _xlslnk.Buyerid = BUYERID;
                        _xlslnk.Supplierid = null;
                        _xlslnk.BuyerSuppLinkid = null;
                        _xlslnk.SmXlsGroupMapping = _eMapobj;
                        _xlslnk.FormatMapCode = cFormatMapCode;
                        _xlslnk.Insert(_dataAccess);
                        SetAuditLog("LeSMonitor", "XLS Mapping for Map Code '" + cFormatMapCode + "' added by [" + UserHostAddress + "]", "Updated", "", "-", "", "", _dataAccess);
                    }
                }
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public string UploadXLSMapping_New(string Filename, string UserHostAddress)
        {
            string result = "", MapCode = ""; int groupID = 0, MapID = 0;
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                License _license = new License();
                _license.SetLicense("Aspose.Total.lic");
                Workbook _xls = new Workbook(Filename);
                FindOptions _option = new FindOptions();
                _option.CaseSensitive = true;
                _option.LookAtType = LookAtType.EntireContent;
                _option.SeachOrderByRows = false;

                SmXlsBuyerLink _xLink = null;   SmXlsGroupMapping _xlsGroup  = null;

                Cell _gmapID = _xls.Worksheets[0].Cells.Find("GROUP", null, _option);
                if (_gmapID != null) { groupID = convert.ToInt(_xls.Worksheets[0].Cells[_gmapID.Row, _gmapID.Column + 2].Value);
                    _xlsGroup = SmXlsGroupMapping.Load_By_GroupID(groupID);
                    MapCode= SmBuyerSupplierGroups.Load(groupID).GroupCode;
                }

                Cell _xlsMapID = _xls.Worksheets[0].Cells.Find("MAP_CODE", null, _option);
                if (_xlsMapID != null)
                {
                    MapID = convert.ToInt(_xls.Worksheets[0].Cells[_xlsMapID.Row, _xlsMapID.Column + 2].Value);
                    MapCode = convert.ToString(_xls.Worksheets[0].Cells[_xlsMapID.Row, _xlsMapID.Column + 1].Value);
                    _xLink = SmXlsBuyerLink.Load(MapID); _xlsGroup = SmXlsGroupMapping.Load(_xLink.SmXlsGroupMapping.ExcelMapid);
                }
                if (_xlsGroup != null)
                {
                    //#region /* Check Duplicate XLS Map Code */
                    //Dal.DataAccess _dataAccess = new Dal.DataAccess();
                    //_dataAccess.CreateSQLCommand("SELECT * FROM SM_XLS_GROUP_MAPPING WHERE XLS_MAP_CODE LIKE  @xlsMapCode AND EXCEL_MAPID <> @xlsMapID");
                    //_dataAccess.AddParameter("xlsMapCode", xlsMapCode, ParameterDirection.Input);
                    //_dataAccess.AddParameter("xlsMapID", xlsMapID, ParameterDirection.Input);
                    //DataSet ds = _dataAccess.ExecuteDataSet();
                    //if (GlobalTools.IsSafeDataSet(ds) && ds.Tables[0].Rows.Count > 0)
                    //{
                    //    throw new Exception("Please provide different XLS_MAP_CODE in Mapping. It already exists.");
                    //}
                    //#endregion

                    try
                    {
                        Worksheet sht = _xls.Worksheets[0];
                        #region /* Get XLS Values */
                        _xlsGroup.XlsMapCode = GetCellValue(sht, "XLS_MAP_CODE");
                        _xlsGroup.SectionRowStart = convert.ToInt(GetCellValue(sht, "SECTION_ROW_START"));
                        _xlsGroup.ItemRowStart = convert.ToInt(GetCellValue(sht, "ITEM_ROW_START"));
                        _xlsGroup.SkipRowsBefItem = convert.ToInt(GetCellValue(sht, "SKIP_ROWS_BEF_ITEM"));
                        _xlsGroup.SkipRowsAftSection = Convert.ToInt32(GetCellValue(sht, "SKIP_ROWS_AFT_SECTION"));
                        _xlsGroup.CellVrno = GetCellValue(sht, "CELL_VRNO");
                        _xlsGroup.CellRfqDt = GetCellValue(sht, "CELL_RFQ_DT");
                        _xlsGroup.CellVessel = GetCellValue(sht, "CELL_VESSEL");
                        _xlsGroup.CellPort = GetCellValue(sht, "CELL_PORT");
                        _xlsGroup.CellLateDt = GetCellValue(sht, "CELL_LATE_DT");
                        _xlsGroup.CellSuppRef = GetCellValue(sht, "CELL_SUPP_REF");
                        _xlsGroup.CellValidUpto = GetCellValue(sht, "CELL_VALID_UPTO");
                        _xlsGroup.CellCurrCode = GetCellValue(sht, "CELL_CURR_CODE");
                        _xlsGroup.CellContact = GetCellValue(sht, "CELL_CONTACT");
                        _xlsGroup.CellPayTerms = GetCellValue(sht, "CELL_PAY_TERMS");
                        _xlsGroup.CellDelTerms = GetCellValue(sht, "CELL_DEL_TERMS");
                        _xlsGroup.CellBuyerRemarks = GetCellValue(sht, "CELL_BUYER_REMARKS");
                        _xlsGroup.CellSuplrRemarks = GetCellValue(sht, "CELL_SUPLR_REMARKS");
                        _xlsGroup.ColSection = GetCellValue(sht, "COL_SECTION");
                        _xlsGroup.ColItemno = GetCellValue(sht, "COL_ITEMNO");
                        _xlsGroup.ColItemRefno = GetCellValue(sht, "COL_ITEM_REFNO");
                        _xlsGroup.ColItemName = GetCellValue(sht, "COL_ITEM_NAME");
                        _xlsGroup.ColItemQty = GetCellValue(sht, "COL_ITEM_QTY");
                        _xlsGroup.ColItemUnit = GetCellValue(sht, "COL_ITEM_UNIT");
                        _xlsGroup.ColItemPrice = GetCellValue(sht, "COL_ITEM_PRICE");
                        _xlsGroup.ColItemDiscount = GetCellValue(sht, "COL_ITEM_DISCOUNT");
                        _xlsGroup.ColItemAltQty = GetCellValue(sht, "COL_ITEM_ALT_QTY");
                        _xlsGroup.ColItemAltUnit = GetCellValue(sht, "COL_ITEM_ALT_UNIT");
                        _xlsGroup.ColItemAltPrice = GetCellValue(sht, "COL_ITEM_ALT_PRICE");
                        _xlsGroup.ColItemDelDays = GetCellValue(sht, "COL_ITEM_DEL_DAYS");
                        _xlsGroup.ColItemRemarks = GetCellValue(sht, "COL_ITEM_REMARKS");
                        _xlsGroup.ColItemBuyerRemarks = GetCellValue(sht, "COL_ITEM_BUYER_REMARKS");
                        _xlsGroup.ColItemTotal = GetCellValue(sht, "COL_ITEM_TOTAL");
                        _xlsGroup.ColItemCurr = GetCellValue(sht, "COL_ITEM_CURR");
                        _xlsGroup.ColItemComments = GetCellValue(sht, "COL_ITEM_COMMENTS");
                        _xlsGroup.ColItemSuppRefno = GetCellValue(sht, "COL_ITEM_SUPP_REFNO");

                        _xlsGroup.ActiveSheet = convert.ToInt(GetCellValue(sht, "ACTIVE_SHEET"));
                        _xlsGroup.ExitForNoitem = (short)convert.ToInt(GetCellValue(sht, "EXIT_FOR_NOITEM"));
                        _xlsGroup.DynSupRmrkOffset = convert.ToInt(GetCellValue(sht, "DYN_SUP_RMRK_OFFSET"));
                        _xlsGroup.OverrideAltQty = convert.ToInt(GetCellValue(sht, "Override_ALT_QTY"));
                        _xlsGroup.SkipHiddenRows = convert.ToInt(GetCellValue(sht, "SKIP_HIDDEN_ROWS"));
                        _xlsGroup.ApplyTotalFormula = convert.ToInt(GetCellValue(sht, "APPLY_TOTAL_FORMULA"));
                        _xlsGroup.ReadItemRemarksUptoNo = convert.ToInt(GetCellValue(sht, "READ_ITEM_REMARKS_UPTO_NO"));
                        _xlsGroup.AddToVrno = GetCellValue(sht, "ADD_TO_VRNO");
                        _xlsGroup.RemoveFromVrno = GetCellValue(sht, "REMOVE_FROM_VRNO");
                        _xlsGroup.SkipRowsAftItem = convert.ToInt(GetCellValue(sht, "SKIP_ROWS_AFT_ITEM"));
                        _xlsGroup.ItemNoAsRowno = convert.ToInt(GetCellValue(sht, "ITEM_NO_AS_ROWNO"));
                        _xlsGroup.ItemDiscPercnt = convert.ToInt(GetCellValue(sht, "ITEM_DISC_PERCNT"));
                        _xlsGroup.CellVslImono = GetCellValue(sht, "CELL_VSL_IMONO");
                        _xlsGroup.CellPortName = GetCellValue(sht, "CELL_PORT_NAME");
                        _xlsGroup.CellDocType = GetCellValue(sht, "CELL_DOC_TYPE");
                        _xlsGroup.CellSuppExpDt = GetCellValue(sht, "CELL_SUPP_EXP_DT");
                        _xlsGroup.CellSuppLateDt = GetCellValue(sht, "CELL_SUPP_LATE_DT");
                        _xlsGroup.CellSuppLeadDays = GetCellValue(sht, "CELL_SUPP_LEAD_DAYS");
                        _xlsGroup.CellByrCompany = GetCellValue(sht, "CELL_BYR_COMPANY");
                        _xlsGroup.CellByrContact = GetCellValue(sht, "CELL_BYR_CONTACT");
                        _xlsGroup.CellByrEmail = GetCellValue(sht, "CELL_BYR_EMAIL");
                        _xlsGroup.CellByrPhone = GetCellValue(sht, "CELL_BYR_PHONE");
                        _xlsGroup.CellByrMob = GetCellValue(sht, "CELL_BYR_MOB");
                        _xlsGroup.CellByrFax = GetCellValue(sht, "CELL_BYR_FAX");
                        _xlsGroup.CellSuppCompany = GetCellValue(sht, "CELL_SUPP_COMPANY");
                        _xlsGroup.CellSuppContact = GetCellValue(sht, "CELL_SUPP_CONTACT");
                        _xlsGroup.CellSuppEmail = GetCellValue(sht, "CELL_SUPP_EMAIL");
                        _xlsGroup.CellSuppPhone = GetCellValue(sht, "CELL_SUPP_PHONE");
                        _xlsGroup.CellSuppMob = GetCellValue(sht, "CELL_SUPP_MOB");
                        _xlsGroup.CellSuppFax = GetCellValue(sht, "CELL_SUPP_FAX");
                        _xlsGroup.CellFreightAmt = GetCellValue(sht, "CELL_FREIGHT_AMT");
                        _xlsGroup.CellOtherAmt = GetCellValue(sht, "CELL_OTHER_AMT");

                        _xlsGroup.CellDiscProvsn = GetCellValue(sht, "CELL_DISC_PROVSN");
                        _xlsGroup.DiscProvsnValue = GetCellValue(sht, "DISC_PROVSN_VALUE");
                        _xlsGroup.AltItemStartOffset = convert.ToInt(GetCellValue(sht, "ALT_ITEM_START_OFFSET"));
                        _xlsGroup.AltItemCount = convert.ToInt(GetCellValue(sht, "ALT_ITEM_COUNT"));
                        _xlsGroup.CellRfqTitle = GetCellValue(sht, "CELL_RFQ_TITLE");
                        _xlsGroup.CellRfqDept = GetCellValue(sht, "CELL_RFQ_DEPT");
                        _xlsGroup.CellEquipName = GetCellValue(sht, "CELL_EQUIP_NAME");
                        _xlsGroup.CellEquipType = GetCellValue(sht, "CELL_EQUIP_TYPE");
                        _xlsGroup.CellEquipMaker = GetCellValue(sht, "CELL_EQUIP_MAKER");
                        _xlsGroup.CellEquipSrno = GetCellValue(sht, "CELL_EQUIP_SERNO");
                        _xlsGroup.CellEquipDtls = GetCellValue(sht, "CELL_EQUIP_DTLS");
                        _xlsGroup.CellMsgNo = GetCellValue(sht, "CELL_MSGNO");
                        _xlsGroup.DynSupFreightOffset = convert.ToInt(GetCellValue(sht, "DYN_SUP_FREIGHT_OFFSET"));

                        _xlsGroup.DynOtherCostOffset = convert.ToInt(GetCellValue(sht, "DYN_OTHERCOST_OFFSET"));
                        _xlsGroup.DynSupCurrOffset = convert.ToInt(GetCellValue(sht, "DYN_SUP_CURR_OFFSET"));
                        _xlsGroup.DynBuyRmrkOffset = convert.ToInt(GetCellValue(sht, "DYN_BYR_RMRK_OFFSET"));
                        _xlsGroup.MultiLineDynItemDesc = convert.ToInt(GetCellValue(sht, "MULTILINE_ITEM_DESCR"));

                        _xlsGroup.ExcelNameMgr = GetCellValue(sht, "EXCEL_NAME_MANAGER");
                        _xlsGroup.DecimalSeprator = GetCellValue(sht, "DECIMAL_SEPARATOR");
                        _xlsGroup.DefaultUMO = GetCellValue(sht, "DEFAULT_UOM");

                        _xlsGroup.DynHdrDiscountOffset = convert.ToInt(GetCellValue(sht, "DYN_HDR_DISCOUNT_OFFSET"));

                        _xlsGroup.REMOVE_FROM_VESSEL_NAME = GetCellValue(sht, "REMOVE_FROM_VESSEL_NAME");
                        _xlsGroup.CELL_BYR_ADDR1 = GetCellValue(sht, "CELL_BYR_ADDR1");
                        _xlsGroup.CELL_BYR_ADDR2 = GetCellValue(sht, "CELL_BYR_ADDR2");
                        _xlsGroup.CELL_SUPP_ADDR1 = GetCellValue(sht, "CELL_SUPP_ADDR1");
                        _xlsGroup.CELL_SUPP_ADDR2 = GetCellValue(sht, "CELL_SUPP_ADDR2");
                        _xlsGroup.CELL_BILL_COMPANY = GetCellValue(sht, "CELL_BILL_COMPANY");
                        _xlsGroup.CELL_BILL_CONTACT = GetCellValue(sht, "CELL_BILL_CONTACT");
                        _xlsGroup.CELL_BILL_EMAIL = GetCellValue(sht, "CELL_BILL_EMAIL");
                        _xlsGroup.CELL_BILL_PHONE = GetCellValue(sht, "CELL_BILL_PHONE");
                        _xlsGroup.CELL_BILL_MOB = GetCellValue(sht, "CELL_BILL_MOB");
                        _xlsGroup.CELL_BILL_FAX = GetCellValue(sht, "CELL_BILL_FAX");
                        _xlsGroup.CELL_BILL_ADDR1 = GetCellValue(sht, "CELL_BILL_ADDR1");
                        _xlsGroup.CELL_BILL_ADDR2 = GetCellValue(sht, "CELL_BILL_ADDR2");
                        _xlsGroup.CELL_SHIP_COMPANY = GetCellValue(sht, "CELL_SHIP_COMPANY");
                        _xlsGroup.CELL_SHIP_CONTACT = GetCellValue(sht, "CELL_SHIP_CONTACT");
                        _xlsGroup.CELL_SHIP_EMAIL = GetCellValue(sht, "CELL_SHIP_EMAIL");
                        _xlsGroup.CELL_SHIP_PHONE = GetCellValue(sht, "CELL_SHIP_PHONE");
                        _xlsGroup.CELL_SHIP_MOB = GetCellValue(sht, "CELL_SHIP_MOB");
                        _xlsGroup.CELL_SHIP_FAX = GetCellValue(sht, "CELL_SHIP_FAX");
                        _xlsGroup.CELL_SHIP_ADDR1 = GetCellValue(sht, "CELL_SHIP_ADDR1");
                        _xlsGroup.CELL_SHIP_ADDR2 = GetCellValue(sht, "CELL_SHIP_ADDR2");
                        _xlsGroup.CELL_ORDER_IDENTIFIER = GetCellValue(sht, "CELL_ORDER_IDENTIFIER");
                        _xlsGroup.CELL_SUPP_QUOTE_DT = GetCellValue(sht, "CELL_SUPP_QUOTE_DT");
                        _xlsGroup.COL_ITEM_ALT_NAME = GetCellValue(sht, "COL_ITEM_ALT_NAME");
                        _xlsGroup.CELL_ETA_DATE = GetCellValue(sht, "CELL_ETA_DATE");
                        _xlsGroup.CELL_ETD_DATE = GetCellValue(sht, "CELL_ETD_DATE");

                        #endregion
                    }
                    catch
                    { throw new System.Exception("Mapping values are not in correct format. Please Check file and upload it again.."); }
                    _xlsGroup.SampleFile = Path.GetFileName(Filename);
                    _dataAccess.BeginTransaction();
                    _xlsGroup.Update(_dataAccess);
                    SetAuditLog("LeSMonitor", "XLS Mapping " + MapCode + " updated by [" + UserHostAddress + "]. ", "Uploaded", "", "", "", "");
                    result = Path.GetFileName(Filename) + " is uploaded successfully.";
                    _dataAccess.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                result = ex.Message;
            }
            finally
            {
                _dataAccess._Dispose();
            }
            return result;
        }


        #endregion

        #endregion

        #region /* PDF Buyer Config */
        public System.Data.DataSet Get_PDF_Mapping()
        {
            return SmPdfBuyerLink.GetPDFBuyerLinks();
        }

        public System.Data.DataSet Get_PDF_Doctypes()
        {
            return SmPdfBuyerLink.GetDocTypes();
        }

        public System.Data.DataSet Get_XLS_Doctypes()//added by kalpita on 23/05/2017
        {
            return SmXlsBuyerLink.GetDocTypes();
        }

        public void Update_PDF_Mapping(int Mapid, int GroupID, string DocType, string MAP_1, string MAP_VAL1, string MAP_2, string MAP_VAL2, string MAP_3, string MAP_VAL3, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                SmPdfBuyerLink _pdfLink = SmPdfBuyerLink.Load(Mapid);

                SmAddress _buyer = SmAddress.Load(_pdfLink.Buyerid);
                SmAddress _supplier = SmAddress.Load(_pdfLink.Supplierid);
                string cSppCode = (_supplier != null) ? _supplier.AddrCode : "";
                string cSppID = (_supplier != null) ? Convert.ToString(_supplier.Addressid) : "";

                _dataAccess.BeginTransaction();
                _pdfLink.DocType = DocType;
                _pdfLink.Mapping1 = MAP_1.Trim();
                _pdfLink.Mapping1Value = MAP_VAL1.Trim();
                _pdfLink.Mapping2 = MAP_2.Trim();
                _pdfLink.Mapping2Value = MAP_VAL2.Trim();
                _pdfLink.Mapping3 = MAP_3.Trim();
                _pdfLink.Mapping3Value = MAP_VAL3.Trim();

                if (GroupID > 0)
                {
                    SmPdfMapping _mapping = SmPdfMapping.LoadByGroup(GroupID);
                    if (_mapping == null)
                    {
                        _mapping = new SmPdfMapping();
                        _mapping.Groupid = GroupID;
                        _mapping.PdfMapid = GetNextKey("PDF_MAPID", "SM_PDF_MAPPING", _dataAccess);
                        _mapping.Insert(_dataAccess);
                    }
                    _pdfLink.PdfMapid = _mapping.PdfMapid;
                }

                _pdfLink.Update(_dataAccess);
                SetAuditLog("LeSMonitor", "PDF Buyer Supplier link (" + _buyer.AddrCode + "-" + cSppCode + ") updated by [" + UserHostAddress + "]. ", "Updated", _buyer.Addressid + "-" + cSppID, "", _buyer.Addressid.ToString(), cSppID, _dataAccess);

                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void Add_PDF_Mapping(int BuyerID, int SupplierID, int GroupID, string DOC_TYPE, string MAP_1, string MAP_VAL1, string MAP_2, string MAP_VAL2, string MAP_3, string MAP_VAL3, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                SmPdfBuyerLink _pdfLink = new SmPdfBuyerLink();
                _dataAccess.BeginTransaction();

                SmAddress _buyer = SmAddress.Load(BuyerID);
                SmAddress _supplier = SmAddress.Load(SupplierID);

                SmBuyerSupplierLink _link = new SmBuyerSupplierLink();
                _link.BuyerAddress = _buyer;
                _link.VendorAddress = _supplier;
                _link.BuyerFormat = "PDF";
                _link.VendorFormat = "PDF";
                _link.Load_byBuyerSupplierFormat();

                _pdfLink.MapId = GetNextKey("MAP_ID", "SM_PDF_BUYER_LINK", _dataAccess);
                _pdfLink.DocType = DOC_TYPE.Trim();
                _pdfLink.Mapping1 = MAP_1.Trim();
                _pdfLink.Mapping1Value = MAP_VAL1.Trim();
                _pdfLink.Mapping2 = MAP_2.Trim();
                _pdfLink.Mapping2Value = MAP_VAL2.Trim();
                _pdfLink.Mapping3 = MAP_3.Trim();
                _pdfLink.Mapping3Value = MAP_VAL3.Trim();
                _pdfLink.Buyerid = BuyerID;
                _pdfLink.Supplierid = SupplierID;
                if (_link != null) _pdfLink.BuyerSuppLinkid = _link.Linkid;

                if (GroupID > 0)
                {
                    SmPdfMapping _mapping = SmPdfMapping.LoadByGroup(GroupID);
                    if (_mapping == null)
                    {
                        _mapping = new SmPdfMapping();
                        _mapping.Groupid = GroupID;
                        _mapping.PdfMapid = GetNextKey("PDF_MAPID", "SM_PDF_MAPPING", _dataAccess);
                        _mapping.Insert(_dataAccess);
                    }
                    _pdfLink.PdfMapid = _mapping.PdfMapid;

                }
                else throw new Exception("Group is not selected");
                _pdfLink.Insert(_dataAccess);

                SetAuditLog("LeSMonitor", "New PDF Buyer Supplier link (" + _buyer.AddrCode + "-" + _supplier.AddrCode + ") added by [" + UserHostAddress + "]. ", "Updated", "", _buyer.Addressid.ToString() + "-" + _supplier.Addressid.ToString(), _buyer.Addressid.ToString(), _supplier.Addressid.ToString(), _dataAccess);

                _dataAccess.CommitTransaction();
            }
            catch
            {
                _dataAccess.RollbackTransaction();
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void RemovePDFMapping(int MapID, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                _dataAccess.CreateConnection();
                SmPdfBuyerLink _pdfLink = SmPdfBuyerLink.Load(MapID);
                if (_pdfLink != null && _pdfLink.MapId > 0)
                {
                    SmPdfMapping _pdfMapping = SmPdfMapping.Load(_pdfLink.PdfMapid);
                    SmBuyerSupplierGroups _grp = SmBuyerSupplierGroups.Load(_pdfMapping.Groupid);
                    SmPdfItemMappingCollection _itemMapColl = SmPdfItemMapping.LoadByPDF_MapID(_pdfMapping.PdfMapid);
                    _dataAccess.BeginTransaction();
                    for (int i = 0; i < _itemMapColl.Count; i++) { _itemMapColl[i].Delete(_dataAccess); }
                    _pdfMapping.Delete(_dataAccess);
                    _pdfLink.Delete(_dataAccess);
                    SetAuditLog("LeSMonitor", "PDF Mapping for group '" + _grp.GroupCode + "' deleted by [" + UserHostAddress + "]", "Updated", "", "-", "", "", _dataAccess);
                }
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }
   
        #region PDF Map Code

        //ADDED BY Kalpita on 04/01/2018
        public System.Data.DataSet Get_PDF_Mapping_AddressId(string AddressId, string ADDRTYPE)
        {
            return SmPdfBuyerLink.Select_SM_PDF_BUYER_LINK_AddressId(AddressId, ADDRTYPE);
        }

        //ADDED BY Kalpita on 22/02/2018
        public System.Data.DataSet Get_PDF_Mapping_MapCode()
        {
            return SmPdfBuyerLink.SM_PDF_BUYER_LINK_MapCode();
        }

        //ADDED BY Kalpita on 22/02/2018
        public string SetPDFMapping_New(int Mapid, string TemplatePath, string SessionID)
        {
            string downloadPath = TemplatePath;
            try
            {
                string temp = System.Configuration.ConfigurationManager.AppSettings["DOWNLOAD_ATTACHMENT"];temp = temp + "\\" + SessionID;
                if (!Directory.Exists(temp)) Directory.CreateDirectory(temp);
                if (File.Exists(TemplatePath))
                {
                    SmPdfBuyerLink _Plink = SmPdfBuyerLink.Load(Mapid);
                    if (_Plink != null)
                    {
                        SmPdfMapping obj = SmPdfMapping.Load(_Plink.PdfMapid);
                        if (obj != null)
                        {
                            SmBuyerSupplierGroups _group = SmBuyerSupplierGroups.Load(obj.Groupid);
                            SmPdfItemMappingCollection _itemCollection = SmPdfItemMapping.LoadByPDF_MapID(obj.PdfMapid);
                            downloadPath = temp + "\\" + _Plink.FormatMapCode + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssff") + ".xls";
                            Workbook _xls = new Workbook(TemplatePath);  Worksheet sht = _xls.Worksheets[0];
                            Cell _mpCodeID = _xls.Worksheets[0].Cells.Find("MAP_CODE", null, _option);
                            if (_mpCodeID != null)
                            {
                                _xls.Worksheets[0].Cells[_mpCodeID.Row, _mpCodeID.Column + 1].Value = _Plink.FormatMapCode;
                                _xls.Worksheets[0].Cells[_mpCodeID.Row, _mpCodeID.Column + 2].Value = _Plink.MapId;
                            }
                            else
                            {
                                Cell _grpID = _xls.Worksheets[0].Cells.Find("GROUP", null, _option);
                                if (_grpID != null)
                                {
                                    _xls.Worksheets[0].Cells[_grpID.Row, _grpID.Column + 1].Value = _group.GroupCode;
                                    _xls.Worksheets[0].Cells[_grpID.Row, _grpID.Column + 2].Value = _group.GroupId;
                                }
                            }
                            #region /* Mapping & DocTYpe */
                            SetCellValue(sht, "DOCTYPE", _Plink.DocType);
                            SetCellValue(sht, "MAPPING RANGE 1", _Plink.Mapping1);
                            SetCellValue(sht, "MAPPING RANGE 1 VALUE", _Plink.Mapping1Value);
                            SetCellValue(sht, "MAPPING RANGE 2", _Plink.Mapping2);
                            SetCellValue(sht, "MAPPING RANGE 2 VALUE", _Plink.Mapping2Value);
                            SetCellValue(sht, "MAPPING RANGE 3", _Plink.Mapping3);
                            SetCellValue(sht, "MAPPING RANGE 3 VALUE", _Plink.Mapping3Value);
                            #endregion

                            #region /* Quote Header */
                            SetCellValue(sht, "RFQ NO", obj.Vrno);
                            SetCellValue(sht, "RFQ NO HEADER", obj.VrnoHeader);
                            SetCellValue(sht, "VESSEL", obj.Vessel);
                            SetCellValue(sht, "VESSEL HEADER", obj.VesselHeader);
                            SetCellValue(sht, "IMO NO", obj.IMO);
                            SetCellValue(sht, "IMO NO HEADER", obj.ImoHeader);
                            SetCellValue(sht, "DELIVERY PORT", obj.DeliveryPort);
                            SetCellValue(sht, "DELIVERY PORT HEADER", obj.DeliveryPortHeader);
                            SetCellValue(sht, "CURRENCY", obj.Currency);
                            SetCellValue(sht, "QUOTE REFERENCE", obj.QuoteReference);
                            SetCellValue(sht, "PO REFERENCE", obj.PoReference);
                            SetCellValue(sht, "CREATED DATE", obj.CreatedDate);
                            SetCellValue(sht, "LATE DATE", obj.LateDate);
                            SetCellValue(sht, "QUOTE VALIDITY", obj.QuoteValidity);
                            SetCellValue(sht, "VESSEL ETA", obj.VesselEta);
                            SetCellValue(sht, "VESSEL ETD", obj.VesselEtd);
                            SetCellValue(sht, "BUYER COMMENTS", obj.BuyerComments);
                            SetCellValue(sht, "SUPPLIER COMMENTS", obj.SuppComments);

                            SetCellValue(sht, "RFQ/PO TITLE", obj.Subject);
                            SetCellValue(sht, "EQUIP NAME", obj.EquipName);
                            SetCellValue(sht, "EQUIP NAME HEADER", obj.EquipNameHeader);
                            SetCellValue(sht, "EQUIP TYPE", obj.EquipType);
                            SetCellValue(sht, "EQUIP TYPE HEADER", obj.EquipTypeHeader);
                            SetCellValue(sht, "EQUIP MAKER", obj.EquipMaker);
                            SetCellValue(sht, "EQUIP MAKER HEADER", obj.EquipMakerHeader);
                            SetCellValue(sht, "EQUIP SER NO", obj.EquipSerno);
                            SetCellValue(sht, "EQUIP SER NO HEADER", obj.EquipSernoHeader);
                            SetCellValue(sht, "EQUIP REMARKS", obj.EquipRemarks);
                            SetCellValue(sht, "EQUIP REMARKS HEADER", obj.EquipRemarksHeader);
                            SetCellValue(sht, "READ CONTENT BELOW ITEMS", obj.ReadContentBelowItems);
                            #endregion

                            #region /* Buyer Info */
                            SetCellValue(sht, "BUYER NAME", obj.BuyerName);
                            SetCellValue(sht, "BUYER ADDRESS", obj.BuyerAddress);
                            SetCellValue(sht, "BUYER CONTACT", obj.BuyerContact);
                            SetCellValue(sht, "BUYER TEL. NO.", obj.BuyerTel);
                            SetCellValue(sht, "BUYER FAX", obj.BuyerFax);
                            SetCellValue(sht, "BUYER EMAIL", obj.BuyerEmail);
                            #endregion

                            #region /* Supplier Info */
                            SetCellValue(sht, "SUPPLIER NAME", obj.SuppName);
                            SetCellValue(sht, "SUPPLIER ADDRESS", obj.SuppAddress);
                            SetCellValue(sht, "SUPPLIER CONTACT", obj.SuppContact);
                            SetCellValue(sht, "SUPPLIER TEL. NO.", obj.SuppTel);
                            SetCellValue(sht, "SUPPLIER FAX", obj.SuppFax);
                            SetCellValue(sht, "SUPPLIER EMAIL", obj.SuppEmail);
                            #endregion

                            #region /* Billing Info */
                            SetCellValue(sht, "BILLING NAME", obj.BillName);
                            SetCellValue(sht, "BILLING ADDRESS", obj.BillAddress);
                            SetCellValue(sht, "BILLING CONTACT", obj.BillContact);
                            SetCellValue(sht, "BILLING TEL. NO.", obj.BillTel);
                            SetCellValue(sht, "BILLING FAX", obj.BillFax);
                            SetCellValue(sht, "BILLING EMAIL", obj.BillEmail);
                            #endregion

                            #region /* Consignee Info */
                            SetCellValue(sht, "CONSIGNEE NAME", obj.ConsignName);
                            SetCellValue(sht, "CONSIGNEE ADDRESS", obj.ConsignAddress);
                            SetCellValue(sht, "CONSIGNEE CONTACT", obj.ConsignContact);
                            SetCellValue(sht, "CONSIGNEE TEL. NO.", obj.ConsignTel);
                            SetCellValue(sht, "CONSIGNEE FAX", obj.ConsignFax);
                            SetCellValue(sht, "CONSIGNEE EMAIL", obj.ConsignEmail);
                            #endregion

                            #region /* Amount Info */
                            SetCellValue(sht, "ITEM TOTAL HEADER", obj.ItemsTotalHeader);
                            SetCellValue(sht, "FRIGHT AMOUNT HEADER", obj.FrieghtAmtHeader);
                            SetCellValue(sht, "ALLOWANCE AMOUNT HEADER", obj.AllowanceAmtHeader);
                            SetCellValue(sht, "PACKING COST HEADER", obj.PackingAmtHeader);
                            SetCellValue(sht, "GRANT AMOUNT HEADER", obj.GrantTotalHeader);
                            #endregion

                            #region /* Other Info */
                            SetCellValue(sht, "DISCOUNT IN %", convert.ToInt(obj.DiscountInPrcnt));
                            SetCellValue(sht, "DECIMAL SEPERATOR", obj.DecimalSeprator);
                            SetCellValue(sht, "INCLUDE BLANCK LINES", convert.ToInt(obj.IncludeBlanckLines));
                            SetCellValue(sht, "HEADER LINE COUNT", convert.ToInt(obj.HeaderLineCount));
                            SetCellValue(sht, "FOOTER LINE COUNT", convert.ToInt(obj.FooterLineCount));
                            SetCellValue(sht, "DATE FORMAT 1", obj.DateFormat1);
                            SetCellValue(sht, "DATE FORMAT 2", obj.DateFormat2);
                            SetCellValue(sht, "ADD HEADER TO COMMENTS", convert.ToInt(obj.AddHeaderToComments));
                            SetCellValue(sht, "ADD FOOTER TO COMMENTS", convert.ToInt(obj.AddFooterToComments));
                            SetCellValue(sht, "EXTRA FIELDS", obj.ExtraFields);
                            SetCellValue(sht, "EXTRA FIELDS HEADER", obj.ExtraFieldsHeader);
                            SetCellValue(sht, "FIELDS FROM HEADER", obj.FieldsFromHeader);
                            SetCellValue(sht, "FIELDS FROM FOOTER", obj.FieldsFromFooter);
                            SetCellValue(sht, "OVERRIDE ITEM QTY", obj.OverrideItemQty);
                            SetCellValue(sht, "VALIDATE ITEM DESCR", obj.ValidateItemDescr);
                            SetCellValue(sht, "HEADER COMMENTS START TEXT", obj.HeaderCommentsStartText);
                            SetCellValue(sht, "HEADER COMMENTS END TEXT", obj.HeaderCommentsEndText);
                            SetCellValue(sht, "ITEM DESCRIPTION UPTO LINE COUNT", obj.ItemDescrUptoLineCount);
                            SetCellValue(sht, "DEPARTMENT", obj.Department);
                            SetCellValue(sht, "ADD ITEM DEL DATE TO HEADER", obj.AddItemToDelDate);
                            SetCellValue(sht, "REMOVE EXTRA SPACE FROM REMARKS", obj.Remark_Header_RemarkSpace);
                            SetCellValue(sht, "HEADER FIRST LINE CONTENT", obj.Header_fLine_Content);
                            SetCellValue(sht, "HEADER LAST LINE CONTENT", obj.Header_lLine_Content);
                            SetCellValue(sht, "FOOTER FIRST LINE CONTENT", obj.Footer_fLine_Content);
                            SetCellValue(sht, "FOOTER LAST LINE CONTENT", obj.Footer_lLine_Content);

                            #endregion

                            #region /* Split File */
                            SetCellValue(sht, "SPLIT FILE", convert.ToInt(obj.SplitFile));
                            SetCellValue(sht, "CONSTANT ROWS", obj.ConstantRows);
                            SetCellValue(sht, "SPLIT FILE START CONTENT", obj.SplitStartContent);
                            SetCellValue(sht, "END COMMENTS FOR SPLIT FILE", obj.EndCommentStartContent);
                            #endregion

                            #region /* Items */
                            if (_itemCollection.Count > 0)
                            {
                                for (int i = 1; i <= _itemCollection.Count; i++)
                                {
                                    SmPdfItemMapping _item = _itemCollection[i - 1];
                                    if (_xls.Worksheets.Count > i)
                                    {
                                        Worksheet shtItem = _xls.Worksheets[i];

                                        SetCellValue(shtItem, "ITEM HEADER", _item.ItemHeaderContent);
                                        SetCellValue(shtItem, "ITEM HEADER LINE COUNT", convert.ToInt(_item.ItemHeaderLineCount));
                                        SetCellValue(shtItem, "ITEM NO", _item.ItemNo);
                                        SetCellValue(shtItem, "ITEM QUANTITY", _item.ItemQty);
                                        SetCellValue(shtItem, "ITEM UNIT", _item.ItemUnit);
                                        SetCellValue(shtItem, "ITEM REFERENCE NO", _item.ItemRefno);
                                        SetCellValue(shtItem, "ITEM DESCREPTION", _item.ItemDescr);
                                        SetCellValue(shtItem, "ITEM REMARKS", _item.ItemRemark);
                                        SetCellValue(shtItem, "ITEM PRICE", _item.ItemUnitprice);
                                        SetCellValue(shtItem, "ITEM DISCOUNT", _item.ItemDiscount);
                                        SetCellValue(shtItem, "ITEM LEADDAYS", _item.ItemLeadDays);
                                        SetCellValue(shtItem, "ITEM TOTAL", _item.ItemTotal);
                                        SetCellValue(shtItem, "ITEM END CONTENT", _item.ItemEndContent);
                                        SetCellValue(shtItem, "LEADDAYS IN DATE", convert.ToInt(_item.LeadDaysInDate));
                                        SetCellValue(shtItem, "HAS NO EQUIP HEADER", convert.ToInt(_item.HasNoEquipHeader));
                                        SetCellValue(shtItem, "MAX EQUIP ROWS", convert.ToInt(_item.MaxEquipRows));
                                        SetCellValue(shtItem, "MAX EQUIP RANGE", convert.ToInt(_item.MaxEquipRange));
                                        string[] strEquip = Convert.ToString(_item.ItemEquipment).Split('/');
                                        SetCellValue(shtItem, "EQUIP NAME HEADER", strEquip[0]);
                                        if (strEquip.Length > 1) SetCellValue(shtItem, "EQUIP TYPE HEADER", strEquip[1]);
                                        if (strEquip.Length > 2) SetCellValue(shtItem, "EQUIP SER. NO HEADER", strEquip[2]);
                                        if (strEquip.Length > 3) SetCellValue(shtItem, "EQUIP MAKER HEADER", strEquip[3]);
                                        if (strEquip.Length > 4) SetCellValue(shtItem, "EQUIP NOTE HEADER", strEquip[4]);
                                        SetCellValue(shtItem, "ITEM GROUP HEADER", _item.ItemGroupHeader);
                                        SetCellValue(shtItem, "ITEM MIN LINE COUNT", convert.ToInt(_item.ItemMinLines));
                                        SetCellValue(shtItem, "ITEM GROUP HEADER", _item.ItemGroupHeader);
                                        SetCellValue(shtItem, "ITEM REMARKS APPEND TEXT", _item.ItemRemarksAppendText);
                                        SetCellValue(shtItem, "ITEM REMARKS INITIALS", _item.ItemRemarksInitials);
                                        SetCellValue(shtItem, "CHECK REMARKS & EQUIP BELOW ITEM", convert.ToInt(_item.CheckContentBelowItem));
                                        SetCellValue(shtItem, "EXTRA COLUMNS", _item.ExtraColumns);
                                        SetCellValue(shtItem, "EXTRA COLUMNS HEADER", _item.ExtraColumnsHeader);
                                        SetCellValue(shtItem, "READ ITEM NO UPTO MIN LINES", _item.ReadItemNoUptoMinLines);
                                        SetCellValue(shtItem, "EQUIP NAME RANGE", _item.EquipNameRange);
                                        SetCellValue(shtItem, "EQUIP TYPE RANGE", _item.EquipTypeRange);
                                        SetCellValue(shtItem, "EQUIP SER. NO. RANGE", _item.EquipSernoRange);
                                        SetCellValue(shtItem, "EQUIP MAKER RANGE", _item.EquipMakerRange);
                                        SetCellValue(shtItem, "EQUIP NOTE RANGE", _item.EquipNoteRange);
                                        SetCellValue(shtItem, "APPEND UOM", _item.AppendUMO);
                                        SetCellValue(shtItem, "MAKER REF. / EXTRA NO. LINE COUNT", _item.MakerExtraNoLineCount);
                                        SetCellValue(shtItem, "MAKER REF. RANGE", _item.MakerRange);
                                        SetCellValue(shtItem, "EXTRA NO. RANGE", _item.ExtranoRange);
                                        SetCellValue(shtItem, "READ PART NO FROM LAST LINE", _item.ReadPartnoFromLastLine);
                                        SetCellValue(shtItem, "ITEM CURRENCY", _item.ItemCurrency);
                                        SetCellValue(shtItem, "APPEND REF NO", _item.APPEND_REF_NO);
                                        SetCellValue(shtItem, "IS BOLD TEXT", _item.IS_BOLD_TEXT);
                                    }
                                }
                            }
                            #endregion

                            _xls.Save(downloadPath);
                        }
                        else { throw new Exception("No Mapping Found!"); }
                    }
                }
            }
            catch (Exception ex)
            {
                SetLog(ex.StackTrace);
                throw ex;
            }
            return downloadPath;
        }

        public void SavePDFMapping_New(Dictionary<string, string> slPMapDet, string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                SmAddress _buyer = null; SmAddress _supplier = null;
                SmPdfBuyerLink _pdfLink = new SmPdfBuyerLink();

                int MapID = Convert.ToInt32(slPMapDet["MAPID"]);
                if (MapID > 0)
                {
                    _pdfLink = SmPdfBuyerLink.Load(MapID);
                    if (_pdfLink != null)
                    {
                        _buyer = SmAddress.Load(_pdfLink.Buyerid);
                        _supplier = SmAddress.Load(_pdfLink.Supplierid);
                    }
                }
                _dataAccess.BeginTransaction();
                _pdfLink.DocType = (slPMapDet.ContainsKey("DOC_TYPE")) ? slPMapDet["DOC_TYPE"].Trim() : null;
                _pdfLink.Mapping1 = (slPMapDet.ContainsKey("MAPPING_1")) ? slPMapDet["MAPPING_1"].Trim() : null;
                _pdfLink.Mapping1Value = (slPMapDet.ContainsKey("MAPPING_1_VALUE")) ? slPMapDet["MAPPING_1_VALUE"].Trim() : null;
                _pdfLink.Mapping2 = (slPMapDet.ContainsKey("MAPPING_2")) ? slPMapDet["MAPPING_2"].Trim() : null;
                _pdfLink.Mapping2Value = (slPMapDet.ContainsKey("MAPPING_2_VALUE")) ? slPMapDet["MAPPING_2_VALUE"].Trim() : null;
                _pdfLink.Mapping3 = (slPMapDet.ContainsKey("MAPPING_3")) ? slPMapDet["MAPPING_3"].Trim() : null;
                _pdfLink.Mapping3Value = (slPMapDet.ContainsKey("MAPPING_3_VALUE")) ? slPMapDet["MAPPING_3_VALUE"].Trim() : null;
                _pdfLink.FormatMapCode = (slPMapDet.ContainsKey("FORMAT_MAPCODE")) ? slPMapDet["FORMAT_MAPCODE"].Trim() : null;
                _pdfLink.Update(_dataAccess);
                SetAuditLog("LeSMonitor", "PDF Buyer Config(" + _buyer.AddrCode + ") updated by [" + UserHostAddress + "]. ", "Updated", _buyer.Addressid.ToString(), "", _buyer.Addressid.ToString(), string.Empty, _dataAccess);              
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void RemovePDFMapping_New(int MapID, string UserHostAddress)
        {
            string cMapCode = "";
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                SmPdfBuyerLink _pdfLink = SmPdfBuyerLink.Load(MapID);
                if (_pdfLink != null && _pdfLink.MapId > 0)
                {
                    cMapCode = _pdfLink.FormatMapCode;
                    SmPdfMapping _pdfMapping = SmPdfMapping.Load(_pdfLink.PdfMapid);
                    SmPdfItemMappingCollection _itemMapColl = SmPdfItemMapping.LoadByPDF_MapID(_pdfMapping.PdfMapid);
                    _dataAccess.BeginTransaction();
                    for (int i = 0; i < _itemMapColl.Count; i++) { _itemMapColl[i].Delete(_dataAccess); }
                    _pdfMapping.Delete(_dataAccess);
                    _pdfLink.Delete(_dataAccess);
                    SetAuditLog("LeSMonitor", "PDF Mapping for Map Code '" + cMapCode + "' deleted by [" + UserHostAddress + "]", "Updated", "", "-", "", "", _dataAccess);
                }
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void CopyPDFMapping_New(int BUYERID,List<string> slPMapDet, string UserHostAddress)
        {
            string cMapCode = "";SmBuyerSupplierLink _bsLnk=null;
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                SmAddress _buyer = SmAddress.Load(BUYERID);
                _dataAccess.BeginTransaction();
                for (int k = 0; k < slPMapDet.Count;k++ )
                {                    
                    SmPdfBuyerLink _pblnkobj = SmPdfBuyerLink.Load_new(Convert.ToInt32(slPMapDet[k]), _dataAccess);
                    if (_pblnkobj != null)
                    {
                        if (_pblnkobj.BuyerSuppLinkid != null) { _bsLnk = SmBuyerSupplierLink.Load_new(_pblnkobj.BuyerSuppLinkid, _dataAccess); cMapCode = _bsLnk.BuyerFormat + "_" + _buyer.AddrCode; }
                        else
                        {
                            cMapCode = _pblnkobj.FormatMapCode.Split('_')[0] + "_" + _buyer.AddrCode;
                        }
                        string cFormatMapCode = SmPdfBuyerLink.GetNextFormatMapCode(_pblnkobj.DocType, cMapCode, _dataAccess);
                        SmPdfBuyerLink _pdflnk = _pblnkobj;
                        _pdflnk.MapId = GetNextKey("MAP_ID", "SM_PDF_BUYER_LINK", _dataAccess);
                        _pdflnk.Buyerid = BUYERID;
                        _pdflnk.Supplierid = null;
                        _pdflnk.BuyerSuppLinkid = null;
                        _pdflnk.FormatMapCode = cFormatMapCode;
                        _pdflnk.Insert(_dataAccess);
                        SetAuditLog("LeSMonitor", "PDF Mapping for Map Code '" + cFormatMapCode + "' added by [" + UserHostAddress + "]", "Updated", "", "-", "", "", _dataAccess);
                    }
                }
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public string UploadPDFMapping_New(string Filename, string UserHostAddress)
        {
            string result = "",cMapCode=""; int groupID =0,mapid=0;
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                Workbook _xls = new Workbook(Filename);
                Worksheet _sht = _xls.Worksheets[0];
                SmPdfBuyerLink _pLink = null; SmPdfMapping _map = null;
                Cell _mapID = _xls.Worksheets[0].Cells.Find("GROUP", null, _option);
                if (_mapID != null)
                {
                    groupID = convert.ToInt(_xls.Worksheets[0].Cells[_mapID.Row, _mapID.Column + 2].Value); _map = SmPdfMapping.LoadByGroup(groupID);
                    if (_map != null) { _pLink = SmPdfBuyerLink.LoadByPDFMapId(_map.PdfMapid); }
                    cMapCode = SmBuyerSupplierGroups.Load(groupID).GroupCode;
                }

                Cell _mpCodeID = _xls.Worksheets[0].Cells.Find("MAP_CODE", null, _option);
                if (_mpCodeID != null)
                {
                    mapid = convert.ToInt(_xls.Worksheets[0].Cells[_mapID.Row, _mapID.Column + 2].Value);
                    _pLink = SmPdfBuyerLink.Load(mapid); _map = SmPdfMapping.Load(_pLink.PdfMapid); cMapCode = _pLink.FormatMapCode;
                }
                if (_map != null)
                {
                    SmPdfItemMappingCollection ItemColl = new SmPdfItemMappingCollection();

                    try
                    {
                        #region /* Mapping */
                        _pLink.DocType = GetCellValue(_sht, "DOCTYPE"); 
                        _pLink.Mapping1 = GetCellValue(_sht, "MAPPING RANGE 1"); 
                        _pLink.Mapping1Value = GetCellValue(_sht, "MAPPING RANGE 1 VALUE"); 
                        _pLink.Mapping2 = GetCellValue(_sht, "MAPPING RANGE 2"); 
                        _pLink.Mapping2Value = GetCellValue(_sht, "MAPPING RANGE 2 VALUE"); 
                        _pLink.Mapping3 = GetCellValue(_sht, "MAPPING RANGE 3"); 
                        _pLink.Mapping3Value = GetCellValue(_sht, "MAPPING RANGE 3 VALUE"); 
                        #endregion

                        #region /* Quote Header */
                        _map.Vrno = GetCellValue(_sht, "RFQ NO");
                        _map.VrnoHeader = GetCellValue(_sht, "RFQ NO HEADER");
                        _map.Vessel = GetCellValue(_sht, "VESSEL");
                        _map.VesselHeader = GetCellValue(_sht, "VESSEL HEADER");
                        _map.IMO = GetCellValue(_sht, "IMO NO");
                        _map.ImoHeader = GetCellValue(_sht, "IMO NO HEADER");
                        _map.Currency = GetCellValue(_sht, "CURRENCY");
                        _map.DeliveryPort = GetCellValue(_sht, "DELIVERY PORT");
                        _map.DeliveryPortHeader = GetCellValue(_sht, "DELIVERY PORT HEADER");
                        _map.QuoteReference = GetCellValue(_sht, "QUOTE REFERENCE");
                        _map.PoReference = GetCellValue(_sht, "PO REFERENCE");
                        _map.LateDate = GetCellValue(_sht, "LATE DATE");
                        _map.CreatedDate = GetCellValue(_sht, "CREATED DATE");
                        _map.QuoteValidity = GetCellValue(_sht, "QUOTE VALIDITY");
                        _map.VesselEta = GetCellValue(_sht, "VESSEL ETA");
                        _map.VesselEtd = GetCellValue(_sht, "VESSEL ETD");

                        _map.Subject = GetCellValue(_sht, "RFQ/PO TITLE");
                        _map.EquipName = GetCellValue(_sht, "EQUIP NAME");
                        _map.EquipNameHeader = GetCellValue(_sht, "EQUIP NAME HEADER");
                        _map.EquipMaker = GetCellValue(_sht, "EQUIP MAKER");
                        _map.EquipMakerHeader = GetCellValue(_sht, "EQUIP MAKER HEADER");
                        _map.EquipType = GetCellValue(_sht, "EQUIP TYPE");
                        _map.EquipTypeHeader = GetCellValue(_sht, "EQUIP TYPE HEADER");
                        _map.EquipSerno = GetCellValue(_sht, "EQUIP SER NO");
                        _map.EquipSernoHeader = GetCellValue(_sht, "EQUIP SER NO HEADER");
                        _map.EquipRemarks = GetCellValue(_sht, "EQUIP REMARKS");
                        _map.EquipRemarksHeader = GetCellValue(_sht, "EQUIP REMARKS HEADER");

                        //

                        _map.BuyerComments = GetCellValue(_sht, "BUYER COMMENTS");
                        _map.SuppComments = GetCellValue(_sht, "SUPPLIER COMMENTS");
                        #endregion

                        #region /* Buyer Info */
                        _map.BuyerName = GetCellValue(_sht, "BUYER NAME");
                        _map.BuyerAddress = GetCellValue(_sht, "BUYER ADDRESS");
                        _map.BuyerContact = GetCellValue(_sht, "BUYER CONTACT");
                        _map.BuyerTel = GetCellValue(_sht, "BUYER TEL. NO.");
                        _map.BuyerFax = GetCellValue(_sht, "BUYER FAX");
                        _map.BuyerEmail = GetCellValue(_sht, "BUYER EMAIL");
                        #endregion

                        #region /* Supplier Info */
                        _map.SuppName = GetCellValue(_sht, "SUPPLIER NAME");
                        _map.SuppAddress = GetCellValue(_sht, "SUPPLIER ADDRESS");
                        _map.SuppContact = GetCellValue(_sht, "SUPPLIER CONTACT");
                        _map.SuppTel = GetCellValue(_sht, "SUPPLIER TEL. NO.");
                        _map.SuppFax = GetCellValue(_sht, "SUPPLIER FAX");
                        _map.SuppEmail = GetCellValue(_sht, "SUPPLIER EMAIL");
                        #endregion

                        #region /* Billing Info */
                        _map.BillName = GetCellValue(_sht, "BILLING NAME");
                        _map.BillAddress = GetCellValue(_sht, "BILLING ADDRESS");
                        _map.BillContact = GetCellValue(_sht, "BILLING CONTACT");
                        _map.BillTel = GetCellValue(_sht, "BILLING TEL. NO.");
                        _map.BillFax = GetCellValue(_sht, "BILLING FAX");
                        _map.BillEmail = GetCellValue(_sht, "BILLING EMAIL");
                        #endregion

                        #region /* Shipping Info */
                        _map.ConsignName = GetCellValue(_sht, "CONSIGNEE NAME");
                        _map.ConsignAddress = GetCellValue(_sht, "CONSIGNEE ADDRESS");
                        _map.ConsignContact = GetCellValue(_sht, "CONSIGNEE CONTACT");
                        _map.ConsignTel = GetCellValue(_sht, "CONSIGNEE TEL. NO.");
                        _map.ConsignFax = GetCellValue(_sht, "CONSIGNEE FAX");
                        _map.ConsignEmail = GetCellValue(_sht, "CONSIGNEE EMAIL");
                        #endregion

                        #region /* Amount Headers */
                        _map.ItemsTotalHeader = GetCellValue(_sht, "ITEM TOTAL HEADER");
                        _map.FrieghtAmtHeader = GetCellValue(_sht, "FRIGHT AMOUNT HEADER");
                        _map.AllowanceAmtHeader = GetCellValue(_sht, "ALLOWANCE AMOUNT HEADER");
                        _map.PackingAmtHeader = GetCellValue(_sht, "PACKING COST HEADER");
                        _map.GrantTotalHeader = GetCellValue(_sht, "GRANT AMOUNT HEADER");
                        #endregion

                        #region /* Split Files */
                        _map.SplitFile = convert.ToInt(GetCellValue(_sht, "SPLIT FILE"));
                        _map.ConstantRows = GetCellValue(_sht, "CONSTANT ROWS");
                        _map.SplitStartContent = GetCellValue(_sht, "SPLIT FILE START CONTENT");
                        _map.EndCommentStartContent = GetCellValue(_sht, "END COMMENTS FOR SPLIT FILE");
                        #endregion

                        #region /* Other Settings */
                        _map.DiscountInPrcnt = convert.ToInt(GetCellValue(_sht, "DISCOUNT IN %"));
                        string _decimalSep = GetCellValue(_sht, "DECIMAL SEPERATOR");
                        if (_decimalSep.Length >= 1) _map.DecimalSeprator = _decimalSep[0];
                        _map.IncludeBlanckLines = convert.ToInt(GetCellValue(_sht, "INCLUDE BLANCK LINES"));
                        _map.HeaderLineCount = convert.ToInt(GetCellValue(_sht, "HEADER LINE COUNT"));
                        _map.FooterLineCount = convert.ToInt(GetCellValue(_sht, "FOOTER LINE COUNT"));
                        _map.DateFormat1 = GetCellValue(_sht, "DATE FORMAT 1");
                        _map.DateFormat2 = GetCellValue(_sht, "DATE FORMAT 2");
                        _map.AddHeaderToComments = convert.ToInt(GetCellValue(_sht, "ADD HEADER TO COMMENTS"));
                        _map.AddFooterToComments = convert.ToInt(GetCellValue(_sht, "ADD FOOTER TO COMMENTS"));
                        _map.ExtraFields = GetCellValue(_sht, "EXTRA FIELDS");
                        _map.ExtraFieldsHeader = GetCellValue(_sht, "EXTRA FIELDS HEADER");
                        _map.FieldsFromHeader = GetCellValue(_sht, "FIELDS FROM HEADER");
                        _map.FieldsFromFooter = GetCellValue(_sht, "FIELDS FROM FOOTER");

                        _map.ReadContentBelowItems = convert.ToInt(GetCellValue(_sht, "READ CONTENT BELOW ITEMS"));
                        _map.OverrideItemQty = convert.ToInt(GetCellValue(_sht, "OVERRIDE ITEM QTY"));
                        _map.ValidateItemDescr = convert.ToInt(GetCellValue(_sht, "VALIDATE ITEM DESCR"));

                        _map.HeaderCommentsStartText = convert.ToString(GetCellValue(_sht, "HEADER COMMENTS START TEXT"));
                        _map.HeaderCommentsEndText = convert.ToString(GetCellValue(_sht, "HEADER COMMENTS END TEXT"));
                        _map.ItemDescrUptoLineCount = convert.ToInt(GetCellValue(_sht, "ITEM DESCRIPTION UPTO LINE COUNT"));

                        _map.Department = convert.ToString(GetCellValue(_sht, "DEPARTMENT"));
                        _map.AddItemToDelDate = convert.ToInt(GetCellValue(_sht, "ADD ITEM DEL DATE TO HEADER"));
                        _map.Remark_Header_RemarkSpace = convert.ToInt(GetCellValue(_sht, "REMOVE EXTRA SPACE FROM REMARKS"));

                        _map.Header_fLine_Content = convert.ToString(GetCellValue(_sht, "HEADER FIRST LINE CONTENT"));
                        _map.Header_lLine_Content = convert.ToString(GetCellValue(_sht, "HEADER LAST LINE CONTENT"));
                        _map.Footer_fLine_Content = convert.ToString(GetCellValue(_sht, "FOOTER FIRST LINE CONTENT"));
                        _map.Footer_lLine_Content = convert.ToString(GetCellValue(_sht, "FOOTER LAST LINE CONTENT"));
                        #endregion

                        #region /* Item Mapping */
                        for (int i = 1; i < _xls.Worksheets.Count; i++)
                        {
                            Worksheet _shtItem = _xls.Worksheets[i];
                            string ItemHeader = GetCellValue(_shtItem, "ITEM HEADER");
                            if (ItemHeader.Trim().Length > 0)
                            {
                                int MapNumber = convert.ToInt(GetCellValue(_shtItem, "ITEM MAPPING NO."));

                                SmPdfItemMapping item = SmPdfItemMapping.Load(_map.PdfMapid, MapNumber);
                                if (item == null) item = new SmPdfItemMapping();

                                item.ItemHeaderContent = ItemHeader.Trim();
                                item.MapNumber = MapNumber;
                                item.ItemHeaderLineCount = convert.ToInt(GetCellValue(_shtItem, "ITEM HEADER LINE COUNT"));
                                item.ItemNo = GetCellValue(_shtItem, "ITEM NO");
                                item.ItemQty = GetCellValue(_shtItem, "ITEM QUANTITY");
                                item.ItemUnit = GetCellValue(_shtItem, "ITEM UNIT");
                                item.ItemRefno = GetCellValue(_shtItem, "ITEM REFERENCE NO");
                                item.ItemDescr = GetCellValue(_shtItem, "ITEM DESCREPTION");
                                item.ItemRemark = GetCellValue(_shtItem, "ITEM REMARKS");
                                item.ItemUnitprice = GetCellValue(_shtItem, "ITEM PRICE");
                                item.ItemDiscount = GetCellValue(_shtItem, "ITEM DISCOUNT");
                                item.ItemLeadDays = GetCellValue(_shtItem, "ITEM LEADDAYS");
                                item.ItemTotal = GetCellValue(_shtItem, "ITEM TOTAL");
                                item.LeadDaysInDate = convert.ToInt(GetCellValue(_shtItem, "LEADDAYS IN DATE"));
                                item.ItemEndContent = GetCellValue(_shtItem, "ITEM END CONTENT");

                                item.HasNoEquipHeader = convert.ToInt(GetCellValue(_shtItem, "HAS NO EQUIP HEADER"));
                                item.MaxEquipRows = convert.ToInt(GetCellValue(_shtItem, "MAX EQUIP ROWS"));
                                item.MaxEquipRange = convert.ToInt(GetCellValue(_shtItem, "MAX EQUIP RANGE"));

                                string EquipName = GetCellValue(_shtItem, "EQUIP NAME HEADER");
                                string EquipType = GetCellValue(_shtItem, "EQUIP TYPE HEADER");
                                string SerNo = GetCellValue(_shtItem, "EQUIP SER. NO HEADER");
                                string EquipMaker = GetCellValue(_shtItem, "EQUIP MAKER HEADER");
                                string EquipNote = GetCellValue(_shtItem, "EQUIP NOTE HEADER");
                                item.ItemEquipment = EquipName + "/" + EquipType + "/" + SerNo + "/" + EquipMaker + "/" + EquipNote;

                                item.ItemGroupHeader = GetCellValue(_shtItem, "ITEM GROUP HEADER");
                                item.ItemMinLines = convert.ToInt(GetCellValue(_shtItem, "ITEM MIN LINE COUNT"));
                                item.ItemRemarksAppendText = GetCellValue(_shtItem, "ITEM REMARKS APPEND TEXT");
                                item.ItemRemarksInitials = GetCellValue(_shtItem, "ITEM REMARKS INITIALS");
                                item.CheckContentBelowItem = convert.ToInt(GetCellValue(_shtItem, "CHECK REMARKS & EQUIP BELOW ITEM"));
                                item.ExtraColumns = GetCellValue(_shtItem, "EXTRA COLUMNS");
                                item.ExtraColumnsHeader = GetCellValue(_shtItem, "EXTRA COLUMNS HEADER");
                                item.ReadItemNoUptoMinLines = convert.ToInt(GetCellValue(_shtItem, "READ ITEM NO UPTO MIN LINES"));
                                item.EquipNameRange = convert.ToString(GetCellValue(_shtItem, "EQUIP NAME RANGE"));
                                item.EquipTypeRange = convert.ToString(GetCellValue(_shtItem, "EQUIP TYPE RANGE"));
                                item.EquipSernoRange = convert.ToString(GetCellValue(_shtItem, "EQUIP SER. NO. RANGE"));
                                item.EquipMakerRange = convert.ToString(GetCellValue(_shtItem, "EQUIP MAKER RANGE"));
                                item.EquipNoteRange = convert.ToString(GetCellValue(_shtItem, "EQUIP NOTE RANGE"));
                                item.AppendUMO = convert.ToInt(GetCellValue(_shtItem, "APPEND UOM"));
                                item.MakerExtraNoLineCount = convert.ToInt(GetCellValue(_shtItem, "MAKER REF. / EXTRA NO. LINE COUNT"));
                                item.MakerRange = convert.ToString(GetCellValue(_shtItem, "MAKER REF. RANGE"));
                                item.ExtranoRange = convert.ToString(GetCellValue(_shtItem, "EXTRA NO. RANGE"));
                                item.ReadPartnoFromLastLine = convert.ToInt(GetCellValue(_shtItem, "READ PART NO FROM LAST LINE"));
                                item.ItemCurrency = convert.ToString(GetCellValue(_shtItem, "ITEM CURRENCY"));
                                item.APPEND_REF_NO = convert.ToInt(GetCellValue(_shtItem, "APPEND REF NO"));
                                item.IS_BOLD_TEXT = convert.ToInt(GetCellValue(_shtItem, "IS BOLD TEXT"));
                                ItemColl.Add(item);
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        throw new System.Exception("Mapping values are not in correct format. Please Check file and upload it again..");
                    }
                    _dataAccess.BeginTransaction();
                    int ItemMapID = GetNextKey("ITEM_MAPID", "SM_PDF_ITEM_MAPPING", _dataAccess);                    
                    _map.Update(_dataAccess);
                    _pLink.Update(_dataAccess);

                    if (ItemColl.Count > 0)
                    {
                        foreach (SmPdfItemMapping item in ItemColl)
                        {
                            if (item.ItemMapid > 0)
                            {
                                item.Update(_dataAccess);
                            }
                            else
                            {
                                item.PdfMapid = _map.PdfMapid;
                                item.ItemMapid = ItemMapID;
                                item.Insert(_dataAccess);
                                ItemMapID++;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Unable to get Item mapping.");
                    }

                    SetAuditLog("LeSMonitor", "PDF Mapping " + cMapCode + " updated by [" + UserHostAddress + "]. ", "Uploaded", "", "", "", "", _dataAccess);
                    result = Path.GetFileName(Filename) + " is uploaded successfully.";
                    _dataAccess.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                result = ex.Message;
            }
            finally
            {
                _dataAccess._Dispose();
            }
            return result;
        }

        #endregion

        #endregion

        #region /* File Mapping */

        public string Update_XLS_Group_Mapping(string Filename, string UserHostAddress,string REFFILE_UPLOADPATH)
        {
            string result = "";
            try
            {
                 License _license = new License();
                _license.SetLicense("Aspose.Total.lic");

                Workbook _xls = new Workbook(Filename);

                FindOptions _option = new FindOptions();
                _option.CaseSensitive = true;
                _option.LookAtType = LookAtType.EntireContent;
                _option.SeachOrderByRows = false;

                Cell _mapID = _xls.Worksheets[0].Cells.Find("GROUP", null, _option);
                int groupID = convert.ToInt(_xls.Worksheets[0].Cells[_mapID.Row, _mapID.Column + 2].Value);

                Cell _xlsMapID = _xls.Worksheets[0].Cells.Find("XLS_MAP_CODE", null, _option);
                int xlsMapID = convert.ToInt(_xls.Worksheets[0].Cells[_xlsMapID.Row, _xlsMapID.Column + 2].Value);
                string xlsMapCode = convert.ToString(_xls.Worksheets[0].Cells[_xlsMapID.Row, _xlsMapID.Column + 1].Value);

                SmXlsGroupMapping _xlsGroup = SmXlsGroupMapping.Load(xlsMapID);
                if (_xlsGroup != null)
                {
                    #region /* Check Duplicate XLS Map Code */
                    Dal.DataAccess _dataAccess = new Dal.DataAccess();
                    _dataAccess.CreateSQLCommand("SELECT * FROM SM_XLS_GROUP_MAPPING WHERE XLS_MAP_CODE LIKE  @xlsMapCode AND EXCEL_MAPID <> @xlsMapID");
                    _dataAccess.AddParameter("xlsMapCode", xlsMapCode, ParameterDirection.Input);
                    _dataAccess.AddParameter("xlsMapID", xlsMapID, ParameterDirection.Input);
                    DataSet ds = _dataAccess.ExecuteDataSet();
                    if (GlobalTools.IsSafeDataSet(ds) && ds.Tables[0].Rows.Count > 0)
                    {
                        throw new Exception("Please provide different XLS_MAP_CODE in Mapping. It already exists.");
                    }
                    #endregion

                    try
                    {
                        Worksheet sht = _xls.Worksheets[0];
                        #region /* Get XLS Values */
                        _xlsGroup.XlsMapCode = GetCellValue(sht, "XLS_MAP_CODE");
                        _xlsGroup.SectionRowStart = convert.ToInt(GetCellValue(sht, "SECTION_ROW_START"));
                        _xlsGroup.ItemRowStart = convert.ToInt(GetCellValue(sht, "ITEM_ROW_START"));
                        _xlsGroup.SkipRowsBefItem = convert.ToInt(GetCellValue(sht, "SKIP_ROWS_BEF_ITEM"));
                        _xlsGroup.SkipRowsAftSection = Convert.ToInt32(GetCellValue(sht, "SKIP_ROWS_AFT_SECTION"));
                        _xlsGroup.CellVrno = GetCellValue(sht, "CELL_VRNO");
                        _xlsGroup.CellRfqDt = GetCellValue(sht, "CELL_RFQ_DT");
                        _xlsGroup.CellVessel = GetCellValue(sht, "CELL_VESSEL");
                        _xlsGroup.CellPort = GetCellValue(sht, "CELL_PORT");
                        _xlsGroup.CellLateDt = GetCellValue(sht, "CELL_LATE_DT");
                        _xlsGroup.CellSuppRef = GetCellValue(sht, "CELL_SUPP_REF");
                        _xlsGroup.CellValidUpto = GetCellValue(sht, "CELL_VALID_UPTO");
                        _xlsGroup.CellCurrCode = GetCellValue(sht, "CELL_CURR_CODE");
                        _xlsGroup.CellContact = GetCellValue(sht, "CELL_CONTACT");
                        _xlsGroup.CellPayTerms = GetCellValue(sht, "CELL_PAY_TERMS");
                        _xlsGroup.CellDelTerms = GetCellValue(sht, "CELL_DEL_TERMS");
                        _xlsGroup.CellBuyerRemarks = GetCellValue(sht, "CELL_BUYER_REMARKS");
                        _xlsGroup.CellSuplrRemarks = GetCellValue(sht, "CELL_SUPLR_REMARKS");
                        _xlsGroup.ColSection = GetCellValue(sht, "COL_SECTION");
                        _xlsGroup.ColItemno = GetCellValue(sht, "COL_ITEMNO");
                        _xlsGroup.ColItemRefno = GetCellValue(sht, "COL_ITEM_REFNO");
                        _xlsGroup.ColItemName = GetCellValue(sht, "COL_ITEM_NAME");
                        _xlsGroup.ColItemQty = GetCellValue(sht, "COL_ITEM_QTY");
                        _xlsGroup.ColItemUnit = GetCellValue(sht, "COL_ITEM_UNIT");
                        _xlsGroup.ColItemPrice = GetCellValue(sht, "COL_ITEM_PRICE");
                        _xlsGroup.ColItemDiscount = GetCellValue(sht, "COL_ITEM_DISCOUNT");
                        _xlsGroup.ColItemAltQty = GetCellValue(sht, "COL_ITEM_ALT_QTY");
                        _xlsGroup.ColItemAltUnit = GetCellValue(sht, "COL_ITEM_ALT_UNIT");
                        _xlsGroup.ColItemAltPrice = GetCellValue(sht, "COL_ITEM_ALT_PRICE");
                        _xlsGroup.ColItemDelDays = GetCellValue(sht, "COL_ITEM_DEL_DAYS");
                        _xlsGroup.ColItemRemarks = GetCellValue(sht, "COL_ITEM_REMARKS");
                        _xlsGroup.ColItemBuyerRemarks = GetCellValue(sht, "COL_ITEM_BUYER_REMARKS");
                        _xlsGroup.ColItemTotal = GetCellValue(sht, "COL_ITEM_TOTAL");
                        _xlsGroup.ColItemCurr = GetCellValue(sht, "COL_ITEM_CURR");
                        _xlsGroup.ColItemComments = GetCellValue(sht, "COL_ITEM_COMMENTS");
                        _xlsGroup.ColItemSuppRefno = GetCellValue(sht, "COL_ITEM_SUPP_REFNO");

                        _xlsGroup.ActiveSheet = convert.ToInt(GetCellValue(sht, "ACTIVE_SHEET"));
                        _xlsGroup.ExitForNoitem = (short)convert.ToInt(GetCellValue(sht, "EXIT_FOR_NOITEM"));
                        _xlsGroup.DynSupRmrkOffset = convert.ToInt(GetCellValue(sht, "DYN_SUP_RMRK_OFFSET"));
                        _xlsGroup.OverrideAltQty = convert.ToInt(GetCellValue(sht, "Override_ALT_QTY"));
                        _xlsGroup.SkipHiddenRows = convert.ToInt(GetCellValue(sht, "SKIP_HIDDEN_ROWS"));
                        _xlsGroup.ApplyTotalFormula = convert.ToInt(GetCellValue(sht, "APPLY_TOTAL_FORMULA"));
                        _xlsGroup.ReadItemRemarksUptoNo = convert.ToInt(GetCellValue(sht, "READ_ITEM_REMARKS_UPTO_NO"));
                        _xlsGroup.AddToVrno = GetCellValue(sht, "ADD_TO_VRNO");
                        _xlsGroup.RemoveFromVrno = GetCellValue(sht, "REMOVE_FROM_VRNO");
                        _xlsGroup.SkipRowsAftItem = convert.ToInt(GetCellValue(sht, "SKIP_ROWS_AFT_ITEM"));
                        _xlsGroup.ItemNoAsRowno = convert.ToInt(GetCellValue(sht, "ITEM_NO_AS_ROWNO"));
                        _xlsGroup.ItemDiscPercnt = convert.ToInt(GetCellValue(sht, "ITEM_DISC_PERCNT"));
                        _xlsGroup.CellVslImono = GetCellValue(sht, "CELL_VSL_IMONO");
                        _xlsGroup.CellPortName = GetCellValue(sht, "CELL_PORT_NAME");
                        _xlsGroup.CellDocType = GetCellValue(sht, "CELL_DOC_TYPE");
                        _xlsGroup.CellSuppExpDt = GetCellValue(sht, "CELL_SUPP_EXP_DT");
                        _xlsGroup.CellSuppLateDt = GetCellValue(sht, "CELL_SUPP_LATE_DT");
                        _xlsGroup.CellSuppLeadDays = GetCellValue(sht, "CELL_SUPP_LEAD_DAYS");
                        _xlsGroup.CellByrCompany = GetCellValue(sht, "CELL_BYR_COMPANY");
                        _xlsGroup.CellByrContact = GetCellValue(sht, "CELL_BYR_CONTACT");
                        _xlsGroup.CellByrEmail = GetCellValue(sht, "CELL_BYR_EMAIL");
                        _xlsGroup.CellByrPhone = GetCellValue(sht, "CELL_BYR_PHONE");
                        _xlsGroup.CellByrMob = GetCellValue(sht, "CELL_BYR_MOB");
                        _xlsGroup.CellByrFax = GetCellValue(sht, "CELL_BYR_FAX");
                        _xlsGroup.CellSuppCompany = GetCellValue(sht, "CELL_SUPP_COMPANY");
                        _xlsGroup.CellSuppContact = GetCellValue(sht, "CELL_SUPP_CONTACT");
                        _xlsGroup.CellSuppEmail = GetCellValue(sht, "CELL_SUPP_EMAIL");
                        _xlsGroup.CellSuppPhone = GetCellValue(sht, "CELL_SUPP_PHONE");
                        _xlsGroup.CellSuppMob = GetCellValue(sht, "CELL_SUPP_MOB");
                        _xlsGroup.CellSuppFax = GetCellValue(sht, "CELL_SUPP_FAX");
                        _xlsGroup.CellFreightAmt = GetCellValue(sht, "CELL_FREIGHT_AMT");
                        _xlsGroup.CellOtherAmt = GetCellValue(sht, "CELL_OTHER_AMT");

                        _xlsGroup.CellDiscProvsn = GetCellValue(sht, "CELL_DISC_PROVSN");
                        _xlsGroup.DiscProvsnValue = GetCellValue(sht, "DISC_PROVSN_VALUE");
                        _xlsGroup.AltItemStartOffset = convert.ToInt(GetCellValue(sht, "ALT_ITEM_START_OFFSET"));
                        _xlsGroup.AltItemCount = convert.ToInt(GetCellValue(sht, "ALT_ITEM_COUNT"));
                        _xlsGroup.CellRfqTitle = GetCellValue(sht, "CELL_RFQ_TITLE");
                        _xlsGroup.CellRfqDept = GetCellValue(sht, "CELL_RFQ_DEPT");
                        _xlsGroup.CellEquipName = GetCellValue(sht, "CELL_EQUIP_NAME");
                        _xlsGroup.CellEquipType = GetCellValue(sht, "CELL_EQUIP_TYPE");
                        _xlsGroup.CellEquipMaker = GetCellValue(sht, "CELL_EQUIP_MAKER");
                        _xlsGroup.CellEquipSrno = GetCellValue(sht, "CELL_EQUIP_SERNO");
                        _xlsGroup.CellEquipDtls = GetCellValue(sht, "CELL_EQUIP_DTLS");
                        _xlsGroup.CellMsgNo = GetCellValue(sht, "CELL_MSGNO");
                        _xlsGroup.DynSupFreightOffset = convert.ToInt(GetCellValue(sht, "DYN_SUP_FREIGHT_OFFSET"));

                        _xlsGroup.DynOtherCostOffset = convert.ToInt(GetCellValue(sht, "DYN_OTHERCOST_OFFSET"));
                        _xlsGroup.DynSupCurrOffset = convert.ToInt(GetCellValue(sht, "DYN_SUP_CURR_OFFSET"));
                        _xlsGroup.DynBuyRmrkOffset = convert.ToInt(GetCellValue(sht, "DYN_BYR_RMRK_OFFSET"));
                        _xlsGroup.MultiLineDynItemDesc = convert.ToInt(GetCellValue(sht, "MULTILINE_ITEM_DESCR"));

                        _xlsGroup.ExcelNameMgr = GetCellValue(sht, "EXCEL_NAME_MANAGER");
                        _xlsGroup.DecimalSeprator = GetCellValue(sht, "DECIMAL_SEPARATOR");
                        _xlsGroup.DefaultUMO = GetCellValue(sht, "DEFAULT_UOM");

                        _xlsGroup.DynHdrDiscountOffset = convert.ToInt(GetCellValue(sht, "DYN_HDR_DISCOUNT_OFFSET"));
                        _xlsGroup.REMOVE_FROM_VESSEL_NAME = GetCellValue(sht, "REMOVE_FROM_VESSEL_NAME");
                        _xlsGroup.CELL_BYR_ADDR1 = GetCellValue(sht, "CELL_BYR_ADDR1");
                        _xlsGroup.CELL_BYR_ADDR2 = GetCellValue(sht, "CELL_BYR_ADDR2");
                        _xlsGroup.CELL_SUPP_ADDR1 = GetCellValue(sht, "CELL_SUPP_ADDR1");
                        _xlsGroup.CELL_SUPP_ADDR2 = GetCellValue(sht, "CELL_SUPP_ADDR2");
                        _xlsGroup.CELL_BILL_COMPANY = GetCellValue(sht, "CELL_BILL_COMPANY");
                        _xlsGroup.CELL_BILL_CONTACT = GetCellValue(sht, "CELL_BILL_CONTACT");
                        _xlsGroup.CELL_BILL_EMAIL = GetCellValue(sht, "CELL_BILL_EMAIL");
                        _xlsGroup.CELL_BILL_PHONE = GetCellValue(sht, "CELL_BILL_PHONE");
                        _xlsGroup.CELL_BILL_MOB = GetCellValue(sht, "CELL_BILL_MOB");
                        _xlsGroup.CELL_BILL_FAX = GetCellValue(sht, "CELL_BILL_FAX");
                        _xlsGroup.CELL_BILL_ADDR1 = GetCellValue(sht, "CELL_BILL_ADDR1");
                        _xlsGroup.CELL_BILL_ADDR2 = GetCellValue(sht, "CELL_BILL_ADDR2");
                        _xlsGroup.CELL_SHIP_COMPANY = GetCellValue(sht, "CELL_SHIP_COMPANY");
                        _xlsGroup.CELL_SHIP_CONTACT = GetCellValue(sht, "CELL_SHIP_CONTACT");
                        _xlsGroup.CELL_SHIP_EMAIL = GetCellValue(sht, "CELL_SHIP_EMAIL");
                        _xlsGroup.CELL_SHIP_PHONE = GetCellValue(sht, "CELL_SHIP_PHONE");
                        _xlsGroup.CELL_SHIP_MOB = GetCellValue(sht, "CELL_SHIP_MOB");
                        _xlsGroup.CELL_SHIP_FAX = GetCellValue(sht, "CELL_SHIP_FAX");
                        _xlsGroup.CELL_SHIP_ADDR1 = GetCellValue(sht, "CELL_SHIP_ADDR1");
                        _xlsGroup.CELL_SHIP_ADDR2 = GetCellValue(sht, "CELL_SHIP_ADDR2");

                        _xlsGroup.CELL_ORDER_IDENTIFIER = GetCellValue(sht, "CELL_ORDER_IDENTIFIER");
                        _xlsGroup.CELL_SUPP_QUOTE_DT = GetCellValue(sht, "CELL_SUPP_QUOTE_DT");
                        _xlsGroup.COL_ITEM_ALT_NAME = GetCellValue(sht, "COL_ITEM_ALT_NAME");
                        _xlsGroup.CELL_ETA_DATE = GetCellValue(sht, "CELL_ETA_DATE");
                        _xlsGroup.CELL_ETD_DATE = GetCellValue(sht, "CELL_ETD_DATE");

                        #endregion
                    }
                    catch
                    { throw new System.Exception("Mapping values are not in correct format. Please Check file and upload it again.."); }

                    _xlsGroup.SampleFile = Path.GetFileName(Filename);
                    _xlsGroup.Update(_dataAccess);

                    string GROUP_CODE = SmBuyerSupplierGroups.Load(groupID).GroupCode;
                    SetAuditLog("LeSMonitor", "XLS Group mapping " + GROUP_CODE + " updated by [" + UserHostAddress + "]. ", "Uploaded", "", "", "", "", _dataAccess);
                    CopyReferenceFiles(GROUP_CODE, REFFILE_UPLOADPATH, "XLS");
                    result = Path.GetFileName(Filename) + " is uploaded successfully.";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public DataSet GetCountryList()
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            _dataAccess.CreateSQLCommand("SELECT * FROM SM_COUNTRY_CODE WHERE 1002 =@VAL");
            _dataAccess.AddParameter("VAL", 1002, ParameterDirection.Input);
            DataSet _ds = _dataAccess.ExecuteDataSet();
            return _ds;
        }

        public string Update_PDF_Mapping(string Filename, string UserHostAddress,string REFFILE_UPLOADPATH)
        {
            string result = "";
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                Workbook _xls = new Workbook(Filename);
                Worksheet _sht = _xls.Worksheets[0];

                Cell _mapID = _xls.Worksheets[0].Cells.Find("GROUP", null, _option);
                int groupID = convert.ToInt(_xls.Worksheets[0].Cells[_mapID.Row, _mapID.Column + 2].Value);

                SmPdfMapping _map = SmPdfMapping.LoadByGroup(groupID);
                if (_map != null)
                {
                    SmPdfBuyerLink _pLink = SmPdfBuyerLink.LoadByPDFMapId(_map.PdfMapid);
                    SmPdfItemMappingCollection ItemColl = new SmPdfItemMappingCollection();

                    try
                    {
                        #region /* Mapping */
                        _pLink.DocType = GetCellValue(_sht, "DOCTYPE"); // DOCTYPE
                        _pLink.Mapping1 = GetCellValue(_sht, "MAPPING RANGE 1"); // MAPPING RANGE 1
                        _pLink.Mapping1Value = GetCellValue(_sht, "MAPPING RANGE 1 VALUE"); // MAPPING RANGE 1 VALUE
                        _pLink.Mapping2 = GetCellValue(_sht, "MAPPING RANGE 2"); // MAPPING RANGE 2
                        _pLink.Mapping2Value = GetCellValue(_sht, "MAPPING RANGE 2 VALUE"); // MAPPING RANGE 2 VALUE
                        _pLink.Mapping3 = GetCellValue(_sht, "MAPPING RANGE 3"); // MAPPING RANGE 3
                        _pLink.Mapping3Value = GetCellValue(_sht, "MAPPING RANGE 3 VALUE"); // MAPPING RANGE 3 VALUE
                        #endregion

                        #region /* Quote Header */
                        _map.Vrno = GetCellValue(_sht, "RFQ NO");
                        _map.VrnoHeader = GetCellValue(_sht, "RFQ NO HEADER");
                        _map.Vessel = GetCellValue(_sht, "VESSEL");
                        _map.VesselHeader = GetCellValue(_sht, "VESSEL HEADER");
                        _map.IMO = GetCellValue(_sht, "IMO NO");
                        _map.ImoHeader = GetCellValue(_sht, "IMO NO HEADER");
                        _map.Currency = GetCellValue(_sht, "CURRENCY");
                        _map.DeliveryPort = GetCellValue(_sht, "DELIVERY PORT");
                        _map.DeliveryPortHeader = GetCellValue(_sht, "DELIVERY PORT HEADER");
                        _map.QuoteReference = GetCellValue(_sht, "QUOTE REFERENCE");
                        _map.PoReference = GetCellValue(_sht, "PO REFERENCE");
                        _map.LateDate = GetCellValue(_sht, "LATE DATE");
                        _map.CreatedDate = GetCellValue(_sht, "CREATED DATE");
                        _map.QuoteValidity = GetCellValue(_sht, "QUOTE VALIDITY"); 
                        _map.VesselEta = GetCellValue(_sht, "VESSEL ETA");
                        _map.VesselEtd = GetCellValue(_sht, "VESSEL ETD");

                        _map.Subject = GetCellValue(_sht, "RFQ/PO TITLE");
                        _map.EquipName = GetCellValue(_sht, "EQUIP NAME");
                        _map.EquipNameHeader = GetCellValue(_sht, "EQUIP NAME HEADER");
                        _map.EquipMaker = GetCellValue(_sht, "EQUIP MAKER");
                        _map.EquipMakerHeader = GetCellValue(_sht, "EQUIP MAKER HEADER");
                        _map.EquipType = GetCellValue(_sht, "EQUIP TYPE");
                        _map.EquipTypeHeader = GetCellValue(_sht, "EQUIP TYPE HEADER");
                        _map.EquipSerno = GetCellValue(_sht, "EQUIP SER NO");
                        _map.EquipSernoHeader = GetCellValue(_sht, "EQUIP SER NO HEADER");
                        _map.EquipRemarks = GetCellValue(_sht, "EQUIP REMARKS");
                        _map.EquipRemarksHeader = GetCellValue(_sht, "EQUIP REMARKS HEADER");

                        _map.BuyerComments = GetCellValue(_sht, "BUYER COMMENTS");
                        _map.SuppComments = GetCellValue(_sht, "SUPPLIER COMMENTS"); 
                        #endregion

                        #region /* Buyer Info */
                        _map.BuyerName = GetCellValue(_sht, "BUYER NAME");
                        _map.BuyerAddress = GetCellValue(_sht, "BUYER ADDRESS");
                        _map.BuyerContact = GetCellValue(_sht, "BUYER CONTACT");
                        _map.BuyerTel = GetCellValue(_sht, "BUYER TEL. NO.");
                        _map.BuyerFax = GetCellValue(_sht, "BUYER FAX");
                        _map.BuyerEmail = GetCellValue(_sht, "BUYER EMAIL");
                        #endregion

                        #region /* Supplier Info */
                        _map.SuppName = GetCellValue(_sht, "SUPPLIER NAME");
                        _map.SuppAddress = GetCellValue(_sht, "SUPPLIER ADDRESS");
                        _map.SuppContact = GetCellValue(_sht, "SUPPLIER CONTACT");
                        _map.SuppTel = GetCellValue(_sht, "SUPPLIER TEL. NO.");
                        _map.SuppFax = GetCellValue(_sht, "SUPPLIER FAX");
                        _map.SuppEmail = GetCellValue(_sht, "SUPPLIER EMAIL");
                        #endregion

                        #region /* Billing Info */
                        _map.BillName = GetCellValue(_sht, "BILLING NAME");
                        _map.BillAddress = GetCellValue(_sht, "BILLING ADDRESS");
                        _map.BillContact = GetCellValue(_sht, "BILLING CONTACT");
                        _map.BillTel = GetCellValue(_sht, "BILLING TEL. NO.");
                        _map.BillFax = GetCellValue(_sht, "BILLING FAX");
                        _map.BillEmail = GetCellValue(_sht, "BILLING EMAIL");
                        #endregion

                        #region /* Shipping Info */
                        _map.ConsignName = GetCellValue(_sht, "CONSIGNEE NAME");
                        _map.ConsignAddress = GetCellValue(_sht, "CONSIGNEE ADDRESS");
                        _map.ConsignContact = GetCellValue(_sht, "CONSIGNEE CONTACT");
                        _map.ConsignTel = GetCellValue(_sht, "CONSIGNEE TEL. NO.");
                        _map.ConsignFax = GetCellValue(_sht, "CONSIGNEE FAX");
                        _map.ConsignEmail = GetCellValue(_sht, "CONSIGNEE EMAIL");
                        #endregion

                        #region /* Amount Headers */
                        _map.ItemsTotalHeader = GetCellValue(_sht, "ITEM TOTAL HEADER");
                        _map.FrieghtAmtHeader = GetCellValue(_sht, "FRIGHT AMOUNT HEADER");
                        _map.AllowanceAmtHeader = GetCellValue(_sht, "ALLOWANCE AMOUNT HEADER");
                        _map.PackingAmtHeader = GetCellValue(_sht, "PACKING COST HEADER");
                        _map.GrantTotalHeader = GetCellValue(_sht, "GRANT AMOUNT HEADER");
                        #endregion

                        #region /* Split Files */
                        _map.SplitFile = convert.ToInt(GetCellValue(_sht, "SPLIT FILE"));
                        _map.ConstantRows = GetCellValue(_sht, "CONSTANT ROWS");
                        _map.SplitStartContent = GetCellValue(_sht, "SPLIT FILE START CONTENT");
                        _map.EndCommentStartContent = GetCellValue(_sht, "END COMMENTS FOR SPLIT FILE");
                        #endregion

                        #region /* Other Settings */
                        _map.DiscountInPrcnt = convert.ToInt(GetCellValue(_sht, "DISCOUNT IN %"));
                        string _decimalSep = GetCellValue(_sht, "DECIMAL SEPERATOR");
                        if (_decimalSep.Length >= 1) _map.DecimalSeprator = _decimalSep[0];
                        _map.IncludeBlanckLines = convert.ToInt(GetCellValue(_sht, "INCLUDE BLANCK LINES"));
                        _map.HeaderLineCount = convert.ToInt(GetCellValue(_sht, "HEADER LINE COUNT"));
                        _map.FooterLineCount = convert.ToInt(GetCellValue(_sht, "FOOTER LINE COUNT"));
                        _map.DateFormat1 = GetCellValue(_sht, "DATE FORMAT 1");
                        _map.DateFormat2 = GetCellValue(_sht, "DATE FORMAT 2");
                        _map.AddHeaderToComments = convert.ToInt(GetCellValue(_sht, "ADD HEADER TO COMMENTS"));
                        _map.AddFooterToComments = convert.ToInt(GetCellValue(_sht, "ADD FOOTER TO COMMENTS"));
                        _map.ExtraFields = GetCellValue(_sht, "EXTRA FIELDS");
                        _map.ExtraFieldsHeader = GetCellValue(_sht, "EXTRA FIELDS HEADER");
                        _map.FieldsFromHeader = GetCellValue(_sht, "FIELDS FROM HEADER");
                        _map.FieldsFromFooter = GetCellValue(_sht, "FIELDS FROM FOOTER");

                        _map.ReadContentBelowItems = convert.ToInt(GetCellValue(_sht, "READ CONTENT BELOW ITEMS"));
                        _map.OverrideItemQty = convert.ToInt(GetCellValue(_sht, "OVERRIDE ITEM QTY"));
                        _map.ValidateItemDescr = convert.ToInt(GetCellValue(_sht, "VALIDATE ITEM DESCR"));
                        _map.HeaderCommentsStartText = convert.ToString(GetCellValue(_sht, "HEADER COMMENTS START TEXT"));
                        _map.HeaderCommentsEndText = convert.ToString(GetCellValue(_sht, "HEADER COMMENTS END TEXT"));
                        _map.ItemDescrUptoLineCount = convert.ToInt(GetCellValue(_sht, "ITEM DESCRIPTION UPTO LINE COUNT"));

                        _map.Department = convert.ToString(GetCellValue(_sht, "DEPARTMENT"));
                        _map.AddItemToDelDate = convert.ToInt(GetCellValue(_sht, "ADD ITEM DEL DATE TO HEADER"));
                        _map.Remark_Header_RemarkSpace = convert.ToInt(GetCellValue(_sht, "REMOVE EXTRA SPACE FROM REMARKS"));

                        _map.Header_fLine_Content = convert.ToString(GetCellValue(_sht, "HEADER FIRST LINE CONTENT"));
                        _map.Header_lLine_Content = convert.ToString(GetCellValue(_sht, "HEADER LAST LINE CONTENT"));
                        _map.Footer_fLine_Content = convert.ToString(GetCellValue(_sht, "FOOTER FIRST LINE CONTENT"));
                        _map.Footer_lLine_Content = convert.ToString(GetCellValue(_sht, "FOOTER LAST LINE CONTENT"));

                        _map.Start_Of_SkipText = convert.ToString(GetCellValue(_sht, "START OF SKIP TEXT"));
                        _map.End_Of_SkipText = convert.ToString(GetCellValue(_sht, "END OF SKIP TEXT"));
                        _map.Add_Skipped_Text_To_Remarks = convert.ToInt(GetCellValue(_sht, "ADD SKIPPED TEXT TO REMARKS"));

                        _map.CurrencyHeader = convert.ToString(GetCellValue(_sht, "CURRENCY HEADER"));
                        _map.SubjectHeader = convert.ToString(GetCellValue(_sht, "TITLE HEADER"));
                        _map.QuoteRefHeader = convert.ToString(GetCellValue(_sht, "QUOTE REFERENCE HEADER"));
                        _map.PocRefHeader = convert.ToString(GetCellValue(_sht, "PO CONF. REF HADER"));
                        _map.DelDateHeader = convert.ToString(GetCellValue(_sht, "LATE DATE HEADER"));
                        _map.DocDateHeader = convert.ToString(GetCellValue(_sht, "CREATED DATE HEADER"));
                        _map.EtaHeader = convert.ToString(GetCellValue(_sht, "VESSEL ETA HEADER"));
                        _map.EtdHeader = convert.ToString(GetCellValue(_sht, "VESSEL ETD HEADER"));
                        _map.QuoteExpHeader = convert.ToString(GetCellValue(_sht, "QUOTE VALIDITY HEADER"));
                        _map.DeptHeader = convert.ToString(GetCellValue(_sht, "DEPARTMENT HEADER"));
                        _map.BuyAddrHeader = convert.ToString(GetCellValue(_sht, "BUYER ADDRESS HEADER"));
                        _map.SuppAddrHeader = convert.ToString(GetCellValue(_sht, "SUPPLIER ADDRESS HEADER"));
                        _map.BillAddrHeader = convert.ToString(GetCellValue(_sht, "BILLING ADDRESS HEADER"));
                        _map.ShipAddrHeader = convert.ToString(GetCellValue(_sht, "CONSIGNEE ADDRESS HEADER"));

                        _map.ItemHeaderCount = convert.ToString(GetCellValue(_sht, "ITEM COUNT HEADER"));
                        _map.ItemFormat = convert.ToString(GetCellValue(_sht, "ITEM FORMAT"));

                        #endregion

                        #region /* Item Mapping */
                        for (int i = 1; i < _xls.Worksheets.Count; i++)
                        {
                            Worksheet _shtItem = _xls.Worksheets[i];
                            string ItemHeader = GetCellValue(_shtItem, "ITEM HEADER");
                            if (ItemHeader.Trim().Length > 0)
                            {
                                int MapNumber = convert.ToInt(GetCellValue(_shtItem, "ITEM MAPPING NO."));

                                SmPdfItemMapping item = SmPdfItemMapping.Load(_map.PdfMapid, MapNumber);
                                if (item == null) item = new SmPdfItemMapping();

                                item.ItemHeaderContent = ItemHeader.Trim();
                                item.MapNumber = MapNumber;
                                item.ItemHeaderLineCount = convert.ToInt(GetCellValue(_shtItem, "ITEM HEADER LINE COUNT"));
                                item.ItemNo = GetCellValue(_shtItem, "ITEM NO");
                                item.ItemQty = GetCellValue(_shtItem, "ITEM QUANTITY");
                                item.ItemUnit = GetCellValue(_shtItem, "ITEM UNIT");
                                item.ItemRefno = GetCellValue(_shtItem, "ITEM REFERENCE NO");
                                item.ItemDescr = GetCellValue(_shtItem, "ITEM DESCREPTION");
                                item.ItemRemark = GetCellValue(_shtItem, "ITEM REMARKS");
                                item.ItemUnitprice = GetCellValue(_shtItem, "ITEM PRICE");
                                item.ItemDiscount = GetCellValue(_shtItem, "ITEM DISCOUNT");
                                item.ItemLeadDays = GetCellValue(_shtItem, "ITEM LEADDAYS");
                                item.ItemTotal = GetCellValue(_shtItem, "ITEM TOTAL");
                                item.LeadDaysInDate = convert.ToInt(GetCellValue(_shtItem, "LEADDAYS IN DATE"));
                                item.ItemEndContent = GetCellValue(_shtItem, "ITEM END CONTENT");

                                item.HasNoEquipHeader = convert.ToInt(GetCellValue(_shtItem, "HAS NO EQUIP HEADER"));
                                item.MaxEquipRows = convert.ToInt(GetCellValue(_shtItem, "MAX EQUIP ROWS"));
                                item.MaxEquipRange = convert.ToInt(GetCellValue(_shtItem, "MAX EQUIP RANGE"));

                                string EquipName = GetCellValue(_shtItem, "EQUIP NAME HEADER");
                                string EquipType = GetCellValue(_shtItem, "EQUIP TYPE HEADER");
                                string SerNo = GetCellValue(_shtItem, "EQUIP SER. NO HEADER");
                                string EquipMaker = GetCellValue(_shtItem, "EQUIP MAKER HEADER");
                                string EquipNote = GetCellValue(_shtItem, "EQUIP NOTE HEADER");
                                item.ItemEquipment = EquipName + "/" + EquipType + "/" + SerNo + "/" + EquipMaker + "/" + EquipNote;

                                item.ItemGroupHeader = GetCellValue(_shtItem, "ITEM GROUP HEADER");
                                item.ItemMinLines = convert.ToInt(GetCellValue(_shtItem, "ITEM MIN LINE COUNT"));
                                item.ItemRemarksAppendText = GetCellValue(_shtItem, "ITEM REMARKS APPEND TEXT");
                                item.ItemRemarksInitials = GetCellValue(_shtItem, "ITEM REMARKS INITIALS");
                                item.CheckContentBelowItem = convert.ToInt(GetCellValue(_shtItem, "CHECK REMARKS & EQUIP BELOW ITEM"));
                                item.ExtraColumns = GetCellValue(_shtItem, "EXTRA COLUMNS");
                                item.ExtraColumnsHeader = GetCellValue(_shtItem, "EXTRA COLUMNS HEADER");

                                item.ReadItemNoUptoMinLines = convert.ToInt(GetCellValue(_shtItem, "READ ITEM NO UPTO MIN LINES"));
                                item.EquipNameRange = convert.ToString(GetCellValue(_shtItem, "EQUIP NAME RANGE"));
                                item.EquipTypeRange = convert.ToString(GetCellValue(_shtItem, "EQUIP TYPE RANGE"));
                                item.EquipSernoRange = convert.ToString(GetCellValue(_shtItem, "EQUIP SER. NO. RANGE"));
                                item.EquipMakerRange = convert.ToString(GetCellValue(_shtItem, "EQUIP MAKER RANGE"));
                                item.EquipNoteRange = convert.ToString(GetCellValue(_shtItem, "EQUIP NOTE RANGE"));
                                item.AppendUMO = convert.ToInt(GetCellValue(_shtItem, "APPEND UOM"));
                                item.MakerExtraNoLineCount = convert.ToInt(GetCellValue(_shtItem, "MAKER REF. / EXTRA NO. LINE COUNT"));
                                item.MakerRange = convert.ToString(GetCellValue(_shtItem, "MAKER REF. RANGE"));
                                item.ExtranoRange = convert.ToString(GetCellValue(_shtItem, "EXTRA NO. RANGE"));
                                item.ReadPartnoFromLastLine = convert.ToInt(GetCellValue(_shtItem, "READ PART NO FROM LAST LINE"));
                                item.ItemCurrency = convert.ToString(GetCellValue(_shtItem, "ITEM CURRENCY"));
                                item.APPEND_REF_NO = convert.ToInt(GetCellValue(_shtItem, "APPEND REF NO"));

                                item.IS_BOLD_TEXT = convert.ToInt(GetCellValue(_shtItem, "IS BOLD TEXT"));
                                item.Is_Qty_Uom_Merged = convert.ToInt(GetCellValue(_shtItem, "IS QTY & UOM MERGED"));
                                item.Remove_Digit_In_Uom = convert.ToInt(GetCellValue(_shtItem, "REMOVE NUMBER FROM UOM"));

                                item.Item_Ref_No_Header = convert.ToString(GetCellValue(_shtItem, "ITEM REF NO HEADER"));

                                ItemColl.Add(item);
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        throw new System.Exception("Mapping values are not in correct format. Please Check file and upload it again..");
                    }

                    int ItemMapID = GetNextKey("ITEM_MAPID", "SM_PDF_ITEM_MAPPING", _dataAccess);
                    _dataAccess.BeginTransaction();
                    _map.Update(_dataAccess);
                    _pLink.Update(_dataAccess);

                    if (ItemColl.Count > 0)
                    {
                        foreach (SmPdfItemMapping item in ItemColl)
                        {
                            if (item.ItemMapid > 0)
                            {
                                item.Update(_dataAccess);
                            }
                            else
                            {
                                item.PdfMapid = _map.PdfMapid;
                                item.ItemMapid = ItemMapID;
                                item.Insert(_dataAccess);
                                ItemMapID++;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Unable to get Item mapping.");
                    }
                    _dataAccess.CommitTransaction();

                    string GROUP_CODE = SmBuyerSupplierGroups.Load(groupID).GroupCode;
                    SetAuditLog("LeSMonitor", "PDF Group mapping " + GROUP_CODE + " updated by [" + UserHostAddress + "]. ", "Uploaded", "", "", "", "");

                    CopyReferenceFiles(GROUP_CODE, REFFILE_UPLOADPATH,"PDF");

                    result = Path.GetFileName(Filename) + " is uploaded successfully.";
                }
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                result = ex.Message;
            }
            finally
            {
                _dataAccess._Dispose();
            }
            return result;
        }

        private void CopyReferenceFiles(string GROUP_CODE, string REFFILE_UPLOADPATH,string DOCTYPE)
        {
            string cDirPath = Convert.ToString(ConfigurationManager.AppSettings["REF_FILEPATH"]) + "\\" +DOCTYPE+"\\"+ GROUP_CODE;
            if (!Directory.Exists(cDirPath))  { Directory.CreateDirectory(cDirPath); }
            File.Copy(REFFILE_UPLOADPATH, cDirPath + "\\" + Path.GetFileName(REFFILE_UPLOADPATH));
        }

        public string SetXLSMapping(int ExcelMapID, string TemplatePath, string SessionID)// string GroupName,
        {
            string downloadPath = TemplatePath;
            DataSet _dsGroups = new DataSet();

            try
            {
                string temp = System.Configuration.ConfigurationManager.AppSettings["DOWNLOAD_ATTACHMENT"];
                temp = temp + "\\" + SessionID;
                if (!Directory.Exists(temp)) Directory.CreateDirectory(temp);

                if (File.Exists(TemplatePath))
                {
                    SmXlsGroupMapping obj = SmXlsGroupMapping.Load(ExcelMapID);
                    if (obj != null)
                    {
                        SmBuyerSupplierGroups _group = SmBuyerSupplierGroups.Load(obj.GroupId);
                        downloadPath = temp + "\\" + _group.GroupCode + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssff") + ".xls";

                        License _license = new License();
                        _license.SetLicense("Aspose.Total.lic");

                        Workbook _xls = new Workbook(TemplatePath);

                        FindOptions _option = new FindOptions();
                        _option.CaseSensitive = true;
                        _option.LookAtType = LookAtType.EntireContent;
                        _option.SeachOrderByRows = true;

                        #region /* Set Values */
                        Worksheet sht = _xls.Worksheets[0];

                        // GROUP ID 
                        Cell _grpID = _xls.Worksheets[0].Cells.Find("GROUP", null, _option);
                        _xls.Worksheets[0].Cells[_grpID.Row, _grpID.Column + 1].Value = _group.GroupCode;
                        _xls.Worksheets[0].Cells[_grpID.Row, _grpID.Column + 2].Value = _group.GroupId;

                        // EXCEL MAP ID
                        Cell _excelID = _xls.Worksheets[0].Cells.Find("XLS_MAP_CODE", null, _option);
                        _xls.Worksheets[0].Cells[_excelID.Row, _excelID.Column + 1].Value = obj.XlsMapCode;
                        _xls.Worksheets[0].Cells[_excelID.Row, _excelID.Column + 2].Value = obj.ExcelMapid;

                        SetCellValue(sht, "SECTION_ROW_START", convert.ToInt(obj.SectionRowStart));
                        SetCellValue(sht, "ITEM_ROW_START", convert.ToInt(obj.ItemRowStart));
                        SetCellValue(sht, "SKIP_ROWS_BEF_ITEM", convert.ToInt(obj.SkipRowsBefItem));
                        SetCellValue(sht, "SKIP_ROWS_AFT_SECTION", convert.ToInt(obj.SkipRowsAftSection));
                        SetCellValue(sht, "CELL_VRNO", obj.CellVrno);
                        SetCellValue(sht, "CELL_RFQ_DT", obj.CellRfqDt);
                        SetCellValue(sht, "CELL_VESSEL", obj.CellVessel);
                        SetCellValue(sht, "CELL_PORT", obj.CellPort);
                        SetCellValue(sht, "CELL_LATE_DT", obj.CellLateDt);
                        SetCellValue(sht, "CELL_SUPP_REF", obj.CellSuppRef);
                        SetCellValue(sht, "CELL_VALID_UPTO", obj.CellValidUpto);
                        SetCellValue(sht, "CELL_CURR_CODE", obj.CellCurrCode);
                        SetCellValue(sht, "CELL_CONTACT", obj.CellContact);
                        SetCellValue(sht, "CELL_PAY_TERMS", obj.CellPayTerms);
                        SetCellValue(sht, "CELL_DEL_TERMS", obj.CellDelTerms);
                        SetCellValue(sht, "CELL_BUYER_REMARKS", obj.CellBuyerRemarks);
                        SetCellValue(sht, "CELL_SUPLR_REMARKS", obj.CellSuplrRemarks);
                        SetCellValue(sht, "COL_SECTION", obj.ColSection);
                        SetCellValue(sht, "COL_ITEMNO", obj.ColItemno);
                        SetCellValue(sht, "COL_ITEM_REFNO", obj.ColItemRefno);
                        SetCellValue(sht, "COL_ITEM_NAME", obj.ColItemName);
                        SetCellValue(sht, "COL_ITEM_QTY", obj.ColItemQty);
                        SetCellValue(sht, "COL_ITEM_UNIT", obj.ColItemUnit);
                        SetCellValue(sht, "COL_ITEM_PRICE", obj.ColItemPrice);
                        SetCellValue(sht, "COL_ITEM_DISCOUNT", obj.ColItemDiscount);
                        SetCellValue(sht, "COL_ITEM_ALT_QTY", obj.ColItemAltQty);
                        SetCellValue(sht, "COL_ITEM_ALT_UNIT", obj.ColItemAltUnit);
                        SetCellValue(sht, "COL_ITEM_ALT_PRICE", obj.ColItemAltPrice);
                        SetCellValue(sht, "COL_ITEM_DEL_DAYS", obj.ColItemDelDays);
                        SetCellValue(sht, "COL_ITEM_REMARKS", obj.ColItemRemarks);
                        SetCellValue(sht, "COL_ITEM_CURR", obj.ColItemCurr);
                        SetCellValue(sht, "COL_ITEM_TOTAL", obj.ColItemTotal);
                        SetCellValue(sht, "ACTIVE_SHEET", convert.ToInt(obj.ActiveSheet));
                        SetCellValue(sht, "EXIT_FOR_NOITEM", convert.ToInt(obj.ExitForNoitem));
                        SetCellValue(sht, "DYN_SUP_RMRK_OFFSET", convert.ToInt(obj.DynSupRmrkOffset));
                        SetCellValue(sht, "Override_ALT_QTY", convert.ToInt(obj.OverrideAltQty));
                        SetCellValue(sht, "SKIP_HIDDEN_ROWS", convert.ToInt(obj.SkipHiddenRows));
                        SetCellValue(sht, "ITEM_DISC_PERCNT", convert.ToInt(obj.ItemDiscPercnt));
                        SetCellValue(sht, "APPLY_TOTAL_FORMULA", convert.ToInt(obj.ApplyTotalFormula));
                        SetCellValue(sht, "READ_ITEM_REMARKS_UPTO_NO", convert.ToInt(obj.ReadItemRemarksUptoNo));
                        SetCellValue(sht, "COL_ITEM_BUYER_REMARKS", obj.ColItemBuyerRemarks);
                        SetCellValue(sht, "ADD_TO_VRNO", obj.AddToVrno);
                        SetCellValue(sht, "REMOVE_FROM_VRNO", obj.RemoveFromVrno);
                        SetCellValue(sht, "SKIP_ROWS_AFT_ITEM", convert.ToInt(obj.SkipRowsAftItem));
                        SetCellValue(sht, "ITEM_NO_AS_ROWNO", convert.ToInt(obj.ItemNoAsRowno));
                        SetCellValue(sht, "COL_ITEM_COMMENTS", obj.ColItemComments);
                        SetCellValue(sht, "CELL_VSL_IMONO", obj.CellVslImono);
                        SetCellValue(sht, "CELL_PORT_NAME", obj.CellPortName);
                        SetCellValue(sht, "CELL_DOC_TYPE", obj.CellDocType);
                        SetCellValue(sht, "COL_ITEM_SUPP_REFNO", obj.ColItemSuppRefno);
                        SetCellValue(sht, "CELL_SUPP_EXP_DT", obj.CellSuppExpDt);
                        SetCellValue(sht, "CELL_SUPP_LATE_DT", obj.CellSuppLateDt);
                        SetCellValue(sht, "CELL_SUPP_LEAD_DAYS", obj.CellSuppLeadDays);
                        SetCellValue(sht, "CELL_BYR_COMPANY", obj.CellByrCompany);
                        SetCellValue(sht, "CELL_BYR_CONTACT", obj.CellByrContact);
                        SetCellValue(sht, "CELL_BYR_EMAIL", obj.CellByrEmail);
                        SetCellValue(sht, "CELL_BYR_PHONE", obj.CellByrPhone);
                        SetCellValue(sht, "CELL_BYR_MOB", obj.CellByrMob);
                        SetCellValue(sht, "CELL_BYR_FAX", obj.CellByrFax);
                        SetCellValue(sht, "CELL_SUPP_COMPANY", obj.CellSuppCompany);
                        SetCellValue(sht, "CELL_SUPP_CONTACT", obj.CellSuppContact);
                        SetCellValue(sht, "CELL_SUPP_EMAIL", obj.CellSuppEmail);
                        SetCellValue(sht, "CELL_SUPP_PHONE", obj.CellSuppPhone);
                        SetCellValue(sht, "CELL_SUPP_MOB", obj.CellSuppMob);
                        SetCellValue(sht, "CELL_SUPP_FAX", obj.CellSuppFax);
                        SetCellValue(sht, "CELL_FREIGHT_AMT", obj.CellFreightAmt);
                        SetCellValue(sht, "CELL_OTHER_AMT", obj.CellOtherAmt);

                        SetCellValue(sht, "CELL_DISC_PROVSN", obj.CellDiscProvsn);
                        SetCellValue(sht, "DISC_PROVSN_VALUE", obj.DiscProvsnValue);
                        SetCellValue(sht, "ALT_ITEM_START_OFFSET", convert.ToInt(obj.AltItemStartOffset));
                        SetCellValue(sht, "ALT_ITEM_COUNT", convert.ToInt(obj.AltItemCount));
                        SetCellValue(sht, "CELL_RFQ_TITLE", obj.CellRfqTitle);
                        SetCellValue(sht, "CELL_RFQ_DEPT", obj.CellRfqDept);
                        SetCellValue(sht, "CELL_EQUIP_NAME", obj.CellEquipName);
                        SetCellValue(sht, "CELL_EQUIP_TYPE", obj.CellEquipType);
                        SetCellValue(sht, "CELL_EQUIP_MAKER", obj.CellEquipMaker);
                        SetCellValue(sht, "CELL_EQUIP_SERNO", obj.CellEquipSrno);
                        SetCellValue(sht, "CELL_EQUIP_DTLS", obj.CellEquipDtls);
                        SetCellValue(sht, "CELL_MSGNO", obj.CellMsgNo);
                        SetCellValue(sht, "DYN_SUP_FREIGHT_OFFSET", convert.ToInt(obj.DynSupFreightOffset));

                        SetCellValue(sht, "DYN_OTHERCOST_OFFSET", convert.ToInt(obj.DynOtherCostOffset));
                        SetCellValue(sht, "DYN_SUP_CURR_OFFSET", convert.ToInt(obj.DynSupCurrOffset));
                        SetCellValue(sht, "DYN_BYR_RMRK_OFFSET", convert.ToInt(obj.DynBuyRmrkOffset));
                        SetCellValue(sht, "MULTILINE_ITEM_DESCR", convert.ToInt(obj.MultiLineDynItemDesc));
                        SetCellValue(sht, "EXCEL_NAME_MANAGER", convert.ToString(obj.ExcelNameMgr));
                        SetCellValue(sht, "DECIMAL_SEPARATOR", convert.ToString(obj.DecimalSeprator));
                        SetCellValue(sht, "DEFAULT_UOM", convert.ToString(obj.DefaultUMO));

                        SetCellValue(sht, "DYN_HDR_DISCOUNT_OFFSET", convert.ToInt(obj.DynHdrDiscountOffset));

                        SetCellValue(sht, "REMOVE_FROM_VESSEL_NAME", obj.REMOVE_FROM_VESSEL_NAME);
                        SetCellValue(sht, "CELL_BYR_ADDR1", obj.CELL_BYR_ADDR1);
                        SetCellValue(sht, "CELL_BYR_ADDR2", obj.CELL_BYR_ADDR2);
                        SetCellValue(sht, "CELL_SUPP_ADDR1", obj.CELL_SUPP_ADDR1);
                        SetCellValue(sht, "CELL_SUPP_ADDR2", obj.CELL_SUPP_ADDR2);
                        SetCellValue(sht, "CELL_BILL_COMPANY", obj.CELL_BILL_COMPANY);
                        SetCellValue(sht, "CELL_BILL_CONTACT", obj.CELL_BILL_CONTACT);
                        SetCellValue(sht, "CELL_BILL_EMAIL", obj.CELL_BILL_EMAIL);
                        SetCellValue(sht, "CELL_BILL_PHONE", obj.CELL_BILL_PHONE);
                        SetCellValue(sht, "CELL_BILL_MOB", obj.CELL_BILL_MOB);
                        SetCellValue(sht, "CELL_BILL_FAX", obj.CELL_BILL_FAX);
                        SetCellValue(sht, "CELL_BILL_ADDR1", obj.CELL_BILL_ADDR1);
                        SetCellValue(sht, "CELL_BILL_ADDR2", obj.CELL_BILL_ADDR2);
                        SetCellValue(sht, "CELL_SHIP_COMPANY", obj.CELL_SHIP_COMPANY);
                        SetCellValue(sht, "CELL_SHIP_CONTACT", obj.CELL_SHIP_CONTACT);
                        SetCellValue(sht, "CELL_SHIP_EMAIL", obj.CELL_SHIP_EMAIL);
                        SetCellValue(sht, "CELL_SHIP_PHONE", obj.CELL_SHIP_PHONE);
                        SetCellValue(sht, "CELL_SHIP_MOB", obj.CELL_SHIP_MOB);
                        SetCellValue(sht, "CELL_SHIP_FAX", obj.CELL_SHIP_FAX);
                        SetCellValue(sht, "CELL_SHIP_ADDR1", obj.CELL_SHIP_ADDR1);
                        SetCellValue(sht, "CELL_SHIP_ADDR2", obj.CELL_SHIP_ADDR2);

                        SetCellValue(sht, "CELL_ORDER_IDENTIFIER", obj.CELL_ORDER_IDENTIFIER);

                        SetCellValue(sht, "CELL_SUPP_QUOTE_DT", obj.CELL_SUPP_QUOTE_DT);
                        SetCellValue(sht, "COL_ITEM_ALT_NAME", obj.COL_ITEM_ALT_NAME);
                        SetCellValue(sht, "CELL_ETA_DATE", obj.CELL_ETA_DATE);
                        SetCellValue(sht, "CELL_ETD_DATE", obj.CELL_ETD_DATE);

                        #endregion

                        _xls.Save(downloadPath);
                    }
                    else { throw new Exception("No Mapping Found!"); }
                }
            }
            catch (Exception ex)
            {
                SetLog(ex.StackTrace);
                throw ex;
            }
            return downloadPath;
        }

        public string SetPDFMapping(int GroupID, string TemplatePath, string SessionID)
        {
            string downloadPath = TemplatePath;
            try
            {
                string temp = System.Configuration.ConfigurationManager.AppSettings["DOWNLOAD_ATTACHMENT"];
                temp = temp + "\\" + SessionID;
                if (!Directory.Exists(temp)) Directory.CreateDirectory(temp);

                if (File.Exists(TemplatePath))
                {
                    SmPdfMapping obj = SmPdfMapping.LoadByGroup(GroupID);
                    if (obj != null)
                    {
                        SmBuyerSupplierGroups _group = SmBuyerSupplierGroups.Load(obj.Groupid);
                        SmPdfBuyerLink _Plink = SmPdfBuyerLink.LoadByPDFMapId(obj.PdfMapid);
                        SmPdfItemMappingCollection _itemCollection = SmPdfItemMapping.LoadByPDF_MapID(obj.PdfMapid);

                        downloadPath = temp + "\\" + _group.GroupCode + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssff") + ".xls";

                        Workbook _xls = new Workbook(TemplatePath);
                        Worksheet sht = _xls.Worksheets[0];

                        Cell _grpID = _xls.Worksheets[0].Cells.Find("GROUP", null, _option);
                        _xls.Worksheets[0].Cells[_grpID.Row, _grpID.Column + 1].Value = _group.GroupCode;
                        _xls.Worksheets[0].Cells[_grpID.Row, _grpID.Column + 2].Value = _group.GroupId;

                        #region /* Mapping & DocTYpe */
                        SetCellValue(sht, "DOCTYPE", _Plink.DocType);
                        SetCellValue(sht, "MAPPING RANGE 1", _Plink.Mapping1);
                        SetCellValue(sht, "MAPPING RANGE 1 VALUE", _Plink.Mapping1Value);
                        SetCellValue(sht, "MAPPING RANGE 2", _Plink.Mapping2);
                        SetCellValue(sht, "MAPPING RANGE 2 VALUE", _Plink.Mapping2Value);
                        SetCellValue(sht, "MAPPING RANGE 3", _Plink.Mapping3);
                        SetCellValue(sht, "MAPPING RANGE 3 VALUE", _Plink.Mapping3Value);
                        #endregion

                        #region /* Quote Header */
                        SetCellValue(sht, "RFQ NO", obj.Vrno);
                        SetCellValue(sht, "RFQ NO HEADER", obj.VrnoHeader);
                        SetCellValue(sht, "VESSEL", obj.Vessel);
                        SetCellValue(sht, "VESSEL HEADER", obj.VesselHeader);
                        SetCellValue(sht, "IMO NO", obj.IMO);
                        SetCellValue(sht, "IMO NO HEADER", obj.ImoHeader);
                        SetCellValue(sht, "DELIVERY PORT", obj.DeliveryPort);
                        SetCellValue(sht, "DELIVERY PORT HEADER", obj.DeliveryPortHeader);
                        SetCellValue(sht, "CURRENCY", obj.Currency);
                        SetCellValue(sht, "QUOTE REFERENCE", obj.QuoteReference);
                        SetCellValue(sht, "PO REFERENCE", obj.PoReference);
                        SetCellValue(sht, "CREATED DATE", obj.CreatedDate);
                        SetCellValue(sht, "LATE DATE", obj.LateDate);
                        SetCellValue(sht, "QUOTE VALIDITY", obj.QuoteValidity);
                        SetCellValue(sht, "VESSEL ETA", obj.VesselEta);
                        SetCellValue(sht, "VESSEL ETD", obj.VesselEtd);
                        SetCellValue(sht, "BUYER COMMENTS", obj.BuyerComments);
                        SetCellValue(sht, "SUPPLIER COMMENTS", obj.SuppComments);

                        SetCellValue(sht, "RFQ/PO TITLE", obj.Subject);
                        SetCellValue(sht, "EQUIP NAME", obj.EquipName);
                        SetCellValue(sht, "EQUIP NAME HEADER", obj.EquipNameHeader);
                        SetCellValue(sht, "EQUIP TYPE", obj.EquipType);
                        SetCellValue(sht, "EQUIP TYPE HEADER", obj.EquipTypeHeader);
                        SetCellValue(sht, "EQUIP MAKER", obj.EquipMaker);
                        SetCellValue(sht, "EQUIP MAKER HEADER", obj.EquipMakerHeader);
                        SetCellValue(sht, "EQUIP SER NO", obj.EquipSerno);
                        SetCellValue(sht, "EQUIP SER NO HEADER", obj.EquipSernoHeader);
                        SetCellValue(sht, "EQUIP REMARKS", obj.EquipRemarks);
                        SetCellValue(sht, "EQUIP REMARKS HEADER", obj.EquipRemarksHeader);
                        SetCellValue(sht, "READ CONTENT BELOW ITEMS", obj.ReadContentBelowItems);
                        #endregion

                        #region /* Buyer Info */
                        SetCellValue(sht, "BUYER NAME", obj.BuyerName);
                        SetCellValue(sht, "BUYER ADDRESS", obj.BuyerAddress);
                        SetCellValue(sht, "BUYER CONTACT", obj.BuyerContact);
                        SetCellValue(sht, "BUYER TEL. NO.", obj.BuyerTel);
                        SetCellValue(sht, "BUYER FAX", obj.BuyerFax);
                        SetCellValue(sht, "BUYER EMAIL", obj.BuyerEmail);
                        #endregion

                        #region /* Supplier Info */
                        SetCellValue(sht, "SUPPLIER NAME", obj.SuppName);
                        SetCellValue(sht, "SUPPLIER ADDRESS", obj.SuppAddress);
                        SetCellValue(sht, "SUPPLIER CONTACT", obj.SuppContact);
                        SetCellValue(sht, "SUPPLIER TEL. NO.", obj.SuppTel);
                        SetCellValue(sht, "SUPPLIER FAX", obj.SuppFax);
                        SetCellValue(sht, "SUPPLIER EMAIL", obj.SuppEmail);
                        #endregion

                        #region /* Billing Info */
                        SetCellValue(sht, "BILLING NAME", obj.BillName);
                        SetCellValue(sht, "BILLING ADDRESS", obj.BillAddress);
                        SetCellValue(sht, "BILLING CONTACT", obj.BillContact);
                        SetCellValue(sht, "BILLING TEL. NO.", obj.BillTel);
                        SetCellValue(sht, "BILLING FAX", obj.BillFax);
                        SetCellValue(sht, "BILLING EMAIL", obj.BillEmail);
                        #endregion

                        #region /* Consignee Info */
                        SetCellValue(sht, "CONSIGNEE NAME", obj.ConsignName);
                        SetCellValue(sht, "CONSIGNEE ADDRESS", obj.ConsignAddress);
                        SetCellValue(sht, "CONSIGNEE CONTACT", obj.ConsignContact);
                        SetCellValue(sht, "CONSIGNEE TEL. NO.", obj.ConsignTel);
                        SetCellValue(sht, "CONSIGNEE FAX", obj.ConsignFax);
                        SetCellValue(sht, "CONSIGNEE EMAIL", obj.ConsignEmail);
                        #endregion

                        #region /* Amount Info */
                        SetCellValue(sht, "ITEM TOTAL HEADER", obj.ItemsTotalHeader);
                        SetCellValue(sht, "FRIGHT AMOUNT HEADER", obj.FrieghtAmtHeader);
                        SetCellValue(sht, "ALLOWANCE AMOUNT HEADER", obj.AllowanceAmtHeader);
                        SetCellValue(sht, "PACKING COST HEADER", obj.PackingAmtHeader);
                        SetCellValue(sht, "GRANT AMOUNT HEADER", obj.GrantTotalHeader);
                        #endregion

                        #region /* Other Info */
                        SetCellValue(sht, "DISCOUNT IN %", convert.ToInt(obj.DiscountInPrcnt));
                        SetCellValue(sht, "DECIMAL SEPERATOR", obj.DecimalSeprator);
                        SetCellValue(sht, "INCLUDE BLANCK LINES", convert.ToInt(obj.IncludeBlanckLines));
                        SetCellValue(sht, "HEADER LINE COUNT", convert.ToInt(obj.HeaderLineCount));
                        SetCellValue(sht, "FOOTER LINE COUNT", convert.ToInt(obj.FooterLineCount));
                        SetCellValue(sht, "DATE FORMAT 1", obj.DateFormat1);
                        SetCellValue(sht, "DATE FORMAT 2", obj.DateFormat2);
                        SetCellValue(sht, "ADD HEADER TO COMMENTS", convert.ToInt(obj.AddHeaderToComments));
                        SetCellValue(sht, "ADD FOOTER TO COMMENTS", convert.ToInt(obj.AddFooterToComments));
                        SetCellValue(sht, "EXTRA FIELDS", obj.ExtraFields);
                        SetCellValue(sht, "EXTRA FIELDS HEADER", obj.ExtraFieldsHeader);
                        SetCellValue(sht, "FIELDS FROM HEADER", obj.FieldsFromHeader);
                        SetCellValue(sht, "FIELDS FROM FOOTER", obj.FieldsFromFooter);
                        SetCellValue(sht, "OVERRIDE ITEM QTY", obj.OverrideItemQty);
                        SetCellValue(sht, "VALIDATE ITEM DESCR", obj.ValidateItemDescr);

                        SetCellValue(sht, "HEADER COMMENTS START TEXT", obj.HeaderCommentsStartText);
                        SetCellValue(sht, "HEADER COMMENTS END TEXT", obj.HeaderCommentsEndText);
                        SetCellValue(sht, "ITEM DESCRIPTION UPTO LINE COUNT", obj.ItemDescrUptoLineCount);

                        SetCellValue(sht, "DEPARTMENT", obj.Department);
                        SetCellValue(sht, "ADD ITEM DEL DATE TO HEADER", obj.AddItemToDelDate);
                        SetCellValue(sht, "REMOVE EXTRA SPACE FROM REMARKS", obj.Remark_Header_RemarkSpace);

                        SetCellValue(sht, "HEADER FIRST LINE CONTENT", obj.Header_fLine_Content);
                        SetCellValue(sht, "HEADER LAST LINE CONTENT", obj.Header_lLine_Content);
                        SetCellValue(sht, "FOOTER FIRST LINE CONTENT", obj.Footer_fLine_Content);
                        SetCellValue(sht, "FOOTER LAST LINE CONTENT", obj.Footer_lLine_Content);


                        SetCellValue(sht, "START OF SKIP TEXT", obj.Start_Of_SkipText);
                        SetCellValue(sht, "END OF SKIP TEXT", obj.End_Of_SkipText);
                        SetCellValue(sht, "ADD SKIPPED TEXT TO REMARKS", convert.ToInt(obj.Add_Skipped_Text_To_Remarks));

                        // ADDED ON 08-01-2018 -- Sayak
                        SetCellValue(sht, "CURRENCY HEADER", convert.ToString(obj.CurrencyHeader));
                        SetCellValue(sht, "TITLE HEADER", convert.ToString(obj.SubjectHeader));
                        SetCellValue(sht, "QUOTE REFERENCE HEADER", convert.ToString(obj.QuoteRefHeader));
                        SetCellValue(sht, "PO CONF. REF HADER", convert.ToString(obj.PocRefHeader));
                        SetCellValue(sht, "LATE DATE HEADER", convert.ToString(obj.DelDateHeader));
                        SetCellValue(sht, "CREATED DATE HEADER", convert.ToString(obj.DocDateHeader));
                        SetCellValue(sht, "VESSEL ETA HEADER", convert.ToString(obj.EtaHeader));
                        SetCellValue(sht, "VESSEL ETD HEADER", convert.ToString(obj.EtdHeader));
                        SetCellValue(sht, "QUOTE VALIDITY HEADER", convert.ToString(obj.QuoteExpHeader));
                        SetCellValue(sht, "DEPARTMENT HEADER", convert.ToString(obj.DeptHeader));
                        SetCellValue(sht, "BUYER ADDRESS HEADER", convert.ToString(obj.BuyAddrHeader));
                        SetCellValue(sht, "SUPPLIER ADDRESS HEADER", convert.ToString(obj.SuppAddrHeader));
                        SetCellValue(sht, "BILLING ADDRESS HEADER", convert.ToString(obj.BillAddrHeader));
                        SetCellValue(sht, "CONSIGNEE ADDRESS HEADER", convert.ToString(obj.ShipAddrHeader));


                        SetCellValue(sht, "ITEM COUNT HEADER", convert.ToString(obj.ItemHeaderCount));
                        SetCellValue(sht, "ITEM FORMAT", convert.ToString(obj.ItemFormat));

                        #endregion

                        #region /* Split File */
                        SetCellValue(sht, "SPLIT FILE", convert.ToInt(obj.SplitFile));
                        SetCellValue(sht, "CONSTANT ROWS", obj.ConstantRows);
                        SetCellValue(sht, "SPLIT FILE START CONTENT", obj.SplitStartContent);
                        SetCellValue(sht, "END COMMENTS FOR SPLIT FILE", obj.EndCommentStartContent);
                        #endregion

                        #region /* Items */
                        if (_itemCollection.Count > 0)
                        {
                            for (int i = 1; i <= _itemCollection.Count; i++)
                            {
                                SmPdfItemMapping _item = _itemCollection[i - 1];
                                if (_xls.Worksheets.Count > i)
                                {
                                    Worksheet shtItem = _xls.Worksheets[i];

                                    SetCellValue(shtItem, "ITEM HEADER", _item.ItemHeaderContent);
                                    SetCellValue(shtItem, "ITEM HEADER LINE COUNT", convert.ToInt(_item.ItemHeaderLineCount));
                                    SetCellValue(shtItem, "ITEM NO", _item.ItemNo);
                                    SetCellValue(shtItem, "ITEM QUANTITY", _item.ItemQty);
                                    SetCellValue(shtItem, "ITEM UNIT", _item.ItemUnit);
                                    SetCellValue(shtItem, "ITEM REFERENCE NO", _item.ItemRefno);
                                    SetCellValue(shtItem, "ITEM DESCREPTION", _item.ItemDescr);
                                    SetCellValue(shtItem, "ITEM REMARKS", _item.ItemRemark);
                                    SetCellValue(shtItem, "ITEM PRICE", _item.ItemUnitprice);
                                    SetCellValue(shtItem, "ITEM DISCOUNT", _item.ItemDiscount);
                                    SetCellValue(shtItem, "ITEM LEADDAYS", _item.ItemLeadDays);
                                    SetCellValue(shtItem, "ITEM TOTAL", _item.ItemTotal);
                                    SetCellValue(shtItem, "ITEM END CONTENT", _item.ItemEndContent);
                                    SetCellValue(shtItem, "LEADDAYS IN DATE", convert.ToInt(_item.LeadDaysInDate));

                                    SetCellValue(shtItem, "HAS NO EQUIP HEADER", convert.ToInt(_item.HasNoEquipHeader));
                                    SetCellValue(shtItem, "MAX EQUIP ROWS", convert.ToInt(_item.MaxEquipRows));
                                    SetCellValue(shtItem, "MAX EQUIP RANGE", convert.ToInt(_item.MaxEquipRange));

                                    string[] strEquip = Convert.ToString(_item.ItemEquipment).Split('/');
                                    SetCellValue(shtItem, "EQUIP NAME HEADER", strEquip[0]);
                                    if (strEquip.Length > 1) SetCellValue(shtItem, "EQUIP TYPE HEADER", strEquip[1]);
                                    if (strEquip.Length > 2) SetCellValue(shtItem, "EQUIP SER. NO HEADER", strEquip[2]);
                                    if (strEquip.Length > 3) SetCellValue(shtItem, "EQUIP MAKER HEADER", strEquip[3]);
                                    if (strEquip.Length > 4) SetCellValue(shtItem, "EQUIP NOTE HEADER", strEquip[4]);

                                    SetCellValue(shtItem, "ITEM GROUP HEADER", _item.ItemGroupHeader);
                                    SetCellValue(shtItem, "ITEM MIN LINE COUNT", convert.ToInt(_item.ItemMinLines));
                                    SetCellValue(shtItem, "ITEM GROUP HEADER", _item.ItemGroupHeader);
                                    SetCellValue(shtItem, "ITEM REMARKS APPEND TEXT", _item.ItemRemarksAppendText);
                                    SetCellValue(shtItem, "ITEM REMARKS INITIALS", _item.ItemRemarksInitials);
                                    SetCellValue(shtItem, "CHECK REMARKS & EQUIP BELOW ITEM", convert.ToInt(_item.CheckContentBelowItem));
                                    SetCellValue(shtItem, "EXTRA COLUMNS", _item.ExtraColumns);
                                    SetCellValue(shtItem, "EXTRA COLUMNS HEADER", _item.ExtraColumnsHeader);

                                    SetCellValue(shtItem, "READ ITEM NO UPTO MIN LINES", _item.ReadItemNoUptoMinLines);

                                    SetCellValue(shtItem, "EQUIP NAME RANGE", _item.EquipNameRange);
                                    SetCellValue(shtItem, "EQUIP TYPE RANGE", _item.EquipTypeRange);
                                    SetCellValue(shtItem, "EQUIP SER. NO. RANGE", _item.EquipSernoRange);
                                    SetCellValue(shtItem, "EQUIP MAKER RANGE", _item.EquipMakerRange);
                                    SetCellValue(shtItem, "EQUIP NOTE RANGE", _item.EquipNoteRange);
                                    SetCellValue(shtItem, "APPEND UOM", _item.AppendUMO);
                                    SetCellValue(shtItem, "MAKER REF. / EXTRA NO. LINE COUNT", _item.MakerExtraNoLineCount);
                                    SetCellValue(shtItem, "MAKER REF. RANGE", _item.MakerRange);
                                    SetCellValue(shtItem, "EXTRA NO. RANGE", _item.ExtranoRange);
                                    SetCellValue(shtItem, "READ PART NO FROM LAST LINE", _item.ReadPartnoFromLastLine);
                                    SetCellValue(shtItem, "ITEM CURRENCY", _item.ItemCurrency);
                                    SetCellValue(shtItem, "APPEND REF NO", _item.APPEND_REF_NO);

                                    SetCellValue(shtItem, "IS BOLD TEXT", _item.IS_BOLD_TEXT);
                                    SetCellValue(shtItem, "IS QTY & UOM MERGED", _item.Is_Qty_Uom_Merged);
                                    SetCellValue(shtItem, "REMOVE NUMBER FROM UOM", _item.Remove_Digit_In_Uom);

                                    SetCellValue(shtItem, "ITEM REF NO HEADER", _item.Item_Ref_No_Header);
                                }
                            }
                        }
                        #endregion

                        _xls.Save(downloadPath);
                    }
                    else { throw new Exception("No Mapping Found!"); }
                }
            }
            catch (Exception ex)
            {
                SetLog(ex.StackTrace);
                throw ex;
            }
            return downloadPath;
        }

        public string Update_Voucher_PDF_Mapping(string Filename, string UserHostAddress, string REFFILE_UPLOADPATH)
        {
            string result = "";
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                Workbook _xls = new Workbook(Filename);
                Worksheet _sht = _xls.Worksheets[0];

                Cell _InvPdfMap = _xls.Worksheets[0].Cells.Find("MAP_CODE", null, _option);
                int MapID = convert.ToInt(_xls.Worksheets[0].Cells[_InvPdfMap.Row, _InvPdfMap.Column + 2].Value);

                string[] _parm = new string[1];
                _parm[0] = "INV_PDF_MAPID=" + MapID;
                object[] _obj = new object[2];
                _obj[0] = "Select * from SM_INVOICE_PDF_MAPPING where INV_PDF_MAPID = @INV_PDF_MAPID";
                _obj[1] = _parm;
                byte[] _byteData = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _obj);
                DataSet _ds = _compressor.Decompress(_byteData);



                string[] _lparm = new string[1];
                _lparm[0] = "INV_PDF_MAPID=" + MapID;
                object[] _lobj = new object[2];
                _lobj[0] = "Select * from SM_INV_PDF_BUYER_SUPPLIER_LINK where INV_PDF_MAPID = @INV_PDF_MAPID";
                _lobj[1] = _lparm;
                byte[] _byteData1 = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _lobj);
                DataSet _ds1 = _compressor.Decompress(_byteData1);

                if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0 && _ds1 != null && _ds1.Tables.Count > 0 && _ds1.Tables[0].Rows.Count > 0)
                {
                    SmInvPdfBuyerSupplierLink _invHeaderMap = new SmInvPdfBuyerSupplierLink();
                    SmInvoicePdfMapping _invPdfMapping = new SmInvoicePdfMapping();
                    SmInvoicePdfItemMappingCollection ItemColl = new SmInvoicePdfItemMappingCollection();
                    _invPdfMapping.Load(_ds.Tables[0].Rows[0]);
                    _invHeaderMap.Load(_ds1.Tables[0].Rows[0]);
                    if (_invPdfMapping != null && _invHeaderMap != null)
                    {
                        #region Voucher Header PDF Mapping
                        _invHeaderMap.Mapping1 = GetCellValue(_sht, "MAPPING RANGE 1");
                        _invHeaderMap.Mapping1Value = GetCellValue(_sht, "MAPPING RANGE 1 VALUE");
                        _invHeaderMap.Mapping2 = GetCellValue(_sht, "MAPPING RANGE 2");
                        _invHeaderMap.Mapping2Value = GetCellValue(_sht, "MAPPING RANGE 2 VALUE");
                        _invHeaderMap.Mapping3 = GetCellValue(_sht, "MAPPING RANGE 3");
                        _invHeaderMap.Mapping3Value = GetCellValue(_sht, "MAPPING RANGE 3 VALUE");

                        _invPdfMapping.Vouchertype = GetCellValue(_sht, "VOUCHERTYPE");
                        _invPdfMapping.VoucherNo = GetCellValue(_sht, "VOUCHER_NO");
                        _invPdfMapping.VoucherNoOffset = convert.ToInt(GetCellValue(_sht, "VOUCHER_NO OFFSET"));
                        _invPdfMapping.VoucherNoHeader = GetCellValueNoTrim(_sht, "VOUCHER_NO HEADER");
                        _invPdfMapping.PoNo = GetCellValue(_sht, "PO_NO");
                        _invPdfMapping.PoNoOffset = convert.ToInt(GetCellValue(_sht, "PO_NO OFFSET"));
                        _invPdfMapping.PoNoHeader = GetCellValueNoTrim(_sht, "PO_NO HEADER");
                        _invPdfMapping.PoRefNo = GetCellValue(_sht, "PO_REF_NO");
                        _invPdfMapping.PoRefNoOffset = convert.ToInt(GetCellValue(_sht, "PO_REF_NO OFFSET"));
                        _invPdfMapping.PoRefNoHeader = GetCellValueNoTrim(_sht, "PO_REF_NO HEADER");
                        _invPdfMapping.SupplierDispatchNo = GetCellValue(_sht, "SUPPLIER_DISPATCH_NO");
                        _invPdfMapping.SupplierDispatchNoOffset = convert.ToInt(GetCellValue(_sht, "SUPPLIER_DISPATCH_NO OFFSET"));
                        _invPdfMapping.SupplierDispatchNoHeader = GetCellValueNoTrim(_sht, "SUPPLIER_DISPATCH_NO HEADER");
                        _invPdfMapping.ForwarderDispatchNo = GetCellValue(_sht, "FORWARDER_DISPATCH_NO");
                        _invPdfMapping.ForwarderDispatchNoOffset = convert.ToInt(GetCellValue(_sht, "FORWARDER_DISPATCH_NO OFFSET"));
                        _invPdfMapping.ForwarderDispatchNoHeader = GetCellValueNoTrim(_sht, "FORWARDER_DISPATCH_NO HEADER");
                        _invPdfMapping.SupplierCustomerNo = GetCellValue(_sht, "SUPPLIER_CUSTOMER_NO");
                        _invPdfMapping.SupplierCustomerNoOffset = convert.ToInt(GetCellValue(_sht, "SUPPLIER_CUSTOMER_NO OFFSET"));
                        _invPdfMapping.SupplierCustomerNoHeader = GetCellValueNoTrim(_sht, "SUPPLIER_CUSTOMER_NO HEADER");
                        _invPdfMapping.Currency = GetCellValue(_sht, "CURRENCY");
                        _invPdfMapping.CurrencyOffset = convert.ToInt(GetCellValue(_sht, "CURRENCY OFFSET"));
                        _invPdfMapping.CurrencyHeader = GetCellValueNoTrim(_sht, "CURRENCY HEADER");
                        _invPdfMapping.VesselImoNo = GetCellValue(_sht, "VESSEL_IMO_NO");
                        _invPdfMapping.VesselImoNoOffset = convert.ToInt(GetCellValue(_sht, "VESSEL_IMO_NO OFFSET"));
                        _invPdfMapping.VesselImoNoHeader = GetCellValueNoTrim(_sht, "VESSEL_IMO_NO HEADER");
                        _invPdfMapping.VesselName = GetCellValue(_sht, "VESSEL_NAME");
                        _invPdfMapping.VesselNameOffset = convert.ToInt(GetCellValue(_sht, "VESSEL_NAME OFFSET"));
                        _invPdfMapping.VesselNameHeader = GetCellValueNoTrim(_sht, "VESSEL_NAME HEADER");
                        _invPdfMapping.PortCode = GetCellValue(_sht, "PORT_CODE");
                        _invPdfMapping.PortCodeOffset = convert.ToInt(GetCellValue(_sht, "PORT_CODE OFFSET"));
                        _invPdfMapping.PortCodeHeader = GetCellValueNoTrim(_sht, "PORT_CODE HEADER");
                        _invPdfMapping.PortName = GetCellValue(_sht, "PORT_NAME");
                        _invPdfMapping.PortNameOffset = convert.ToInt(GetCellValue(_sht, "PORT_NAME OFFSET"));
                        _invPdfMapping.PortNameHeader = GetCellValueNoTrim(_sht, "PORT_NAME HEADER");
                        _invPdfMapping.PaymentTerms = GetCellValue(_sht, "PAYMENT_TERMS");
                        _invPdfMapping.PaymentTermsOffset = convert.ToInt(GetCellValue(_sht, "PAYMENT_TERMS OFFSET"));
                        _invPdfMapping.PaymentTermsHeader = GetCellValueNoTrim(_sht, "PAYMENT_TERMS HEADER");
                        _invPdfMapping.DeliveryTerms = GetCellValue(_sht, "DELIVERY_TERMS");
                        _invPdfMapping.DeliveryTermsOffset = convert.ToInt(GetCellValue(_sht, "DELIVERY_TERMS OFFSET"));
                        _invPdfMapping.DeliveryTermsHeader = GetCellValueNoTrim(_sht, "DELIVERY_TERMS HEADER");
                        _invPdfMapping.SupplierRemarks = GetCellValue(_sht, "SUPPLIER_REMARKS");
                        _invPdfMapping.SupplierRemarksOffset = convert.ToInt(GetCellValue(_sht, "SUPPLIER_REMARKS OFFSET"));
                        _invPdfMapping.SupplierRemarksHeader = GetCellValueNoTrim(_sht, "SUPPLIER_REMARKS HEADER");
                        _invPdfMapping.ItemsCount = GetCellValue(_sht, "ITEMS_COUNT");
                        _invPdfMapping.ItemsCountOffset = convert.ToInt(GetCellValue(_sht, "ITEMS_COUNT OFFSET"));
                        _invPdfMapping.ItemsCountHeader = GetCellValueNoTrim(_sht, "ITEMS_COUNT HEADER");
                        _invPdfMapping.CourierName = GetCellValue(_sht, "COURIER_NAME");
                        _invPdfMapping.CourierNameOffset = convert.ToInt(GetCellValue(_sht, "COURIER_NAME OFFSET"));
                        _invPdfMapping.CourierNameHeader = GetCellValueNoTrim(_sht, "COURIER_NAME HEADER");
                        _invPdfMapping.BuyerVatNo = GetCellValue(_sht, "BUYER_VAT_NO");
                        _invPdfMapping.BuyerVatNoOffset = convert.ToInt(GetCellValue(_sht, "BUYER_VAT_NO OFFSET"));
                        _invPdfMapping.BuyerVatNoHeader = GetCellValueNoTrim(_sht, "BUYER_VAT_NO HEADER");
                        _invPdfMapping.SupplierVatNo = GetCellValue(_sht, "SUPPLIER_VAT_NO");
                        _invPdfMapping.SupplierVatNoOffset = convert.ToInt(GetCellValue(_sht, "SUPPLIER_VAT_NO OFFSET"));
                        _invPdfMapping.SupplierVatNoHeader = GetCellValueNoTrim(_sht, "SUPPLIER_VAT_NO HEADER");
                        _invPdfMapping.VoucherDate = GetCellValue(_sht, "VOUCHER_DATE");
                        _invPdfMapping.VoucherDateOffset = convert.ToInt(GetCellValue(_sht, "VOUCHER_DATE OFFSET"));
                        _invPdfMapping.VoucherDateHeader = GetCellValueNoTrim(_sht, "VOUCHER_DATE HEADER");
                        _invPdfMapping.VoucherDueDate = GetCellValue(_sht, "VOUCHER_DUE_DATE");
                        _invPdfMapping.VoucherDueDateOffset = convert.ToInt(GetCellValue(_sht, "VOUCHER_DUE_DATE OFFSET"));
                        _invPdfMapping.VoucherDueDateHeader = GetCellValueNoTrim(_sht, "VOUCHER_DUE_DATE HEADER");
                        _invPdfMapping.BuyerPoDate = GetCellValue(_sht, "BUYER_PO_DATE");
                        _invPdfMapping.BuyerPoDateOffset = convert.ToInt(GetCellValue(_sht, "BUYER_PO_DATE OFFSET"));
                        _invPdfMapping.BuyerPoDateHeader = GetCellValueNoTrim(_sht, "BUYER_PO_DATE HEADER");
                        _invPdfMapping.DispatchDate = GetCellValue(_sht, "DISPATCH_DATE");
                        _invPdfMapping.DispatchDateOffset = convert.ToInt(GetCellValue(_sht, "DISPATCH_DATE OFFSET"));
                        _invPdfMapping.DispatchDateHeader = GetCellValueNoTrim(_sht, "DISPATCH_DATE HEADER");
                        _invPdfMapping.LateDate = GetCellValue(_sht, "LATE_DATE");
                        _invPdfMapping.LateDateOffset = convert.ToInt(GetCellValue(_sht, "LATE_DATE OFFSET"));
                        _invPdfMapping.LateDateHeader = GetCellValueNoTrim(_sht, "LATE_DATE HEADER");
                        _invPdfMapping.BankName = GetCellValue(_sht, "BANK_NAME");
                        _invPdfMapping.BankNameOffset = convert.ToInt(GetCellValue(_sht, "BANK_NAME OFFSET"));
                        _invPdfMapping.BankNameHeader = GetCellValueNoTrim(_sht, "BANK_NAME HEADER");
                        _invPdfMapping.AccountName = GetCellValue(_sht, "ACCOUNT_NAME");
                        _invPdfMapping.AccountNameOffset = convert.ToInt(GetCellValue(_sht, "ACCOUNT_NAME OFFSET"));
                        _invPdfMapping.AccountNameHeader = GetCellValueNoTrim(_sht, "ACCOUNT_NAME HEADER");
                        _invPdfMapping.Coc = GetCellValue(_sht, "COC");
                        _invPdfMapping.CocOffset = convert.ToInt(GetCellValue(_sht, "COC OFFSET"));
                        _invPdfMapping.CocHeader = GetCellValueNoTrim(_sht, "COC HEADER");
                        _invPdfMapping.SortCode = GetCellValue(_sht, "SORT_CODE");
                        _invPdfMapping.SortCodeOffset = convert.ToInt(GetCellValue(_sht, "SORT_CODE OFFSET"));
                        _invPdfMapping.SortCodeHeader = GetCellValueNoTrim(_sht, "SORT_CODE HEADER");
                        _invPdfMapping.IbanNo = GetCellValue(_sht, "IBAN_NO");
                        _invPdfMapping.IbanNoOffset = convert.ToInt(GetCellValue(_sht, "IBAN_NO OFFSET"));
                        _invPdfMapping.IbanNoHeader = GetCellValueNoTrim(_sht, "IBAN_NO HEADER");
                        _invPdfMapping.SwiftNo = GetCellValue(_sht, "SWIFT_BIC_NO");
                        _invPdfMapping.SwiftNoOffset = convert.ToInt(GetCellValue(_sht, "SWIFT_BIC_NO OFFSET"));
                        _invPdfMapping.SwiftNoHeader = GetCellValueNoTrim(_sht, "SWIFT_BIC_NO HEADER");
                        _invPdfMapping.AccountNo = GetCellValue(_sht, "ACCOUNT_NO");
                        _invPdfMapping.AccountNoOffset = convert.ToInt(GetCellValue(_sht, "ACCOUNT_NO OFFSET"));
                        _invPdfMapping.AccountNoHeader = GetCellValueNoTrim(_sht, "ACCOUNT_NO HEADER");
                        _invPdfMapping.KvkNo = GetCellValue(_sht, "KVK_NO");
                        _invPdfMapping.KvkNoOffset = convert.ToInt(GetCellValue(_sht, "KVK_NO OFFSET"));
                        _invPdfMapping.KvkNoHeader = GetCellValueNoTrim(_sht, "KVK_NO HEADER");
                        _invPdfMapping.ItemsTotalAmount = GetCellValue(_sht, "ITEMS_TOTAL_AMOUNT");
                        _invPdfMapping.ItemsTotalAmountOffset = convert.ToInt(GetCellValue(_sht, "ITEMS_TOTAL_AMOUNT OFFSET"));
                        _invPdfMapping.ItemsTotalAmountHeader = GetCellValueNoTrim(_sht, "ITEMS_TOTAL_AMOUNT HEADER");
                        _invPdfMapping.AllowanceAmount = GetCellValue(_sht, "ALLOWANCE_AMOUNT");
                        _invPdfMapping.AllowanceAmountOffset = convert.ToInt(GetCellValue(_sht, "ALLOWANCE_AMOUNT OFFSET"));
                        _invPdfMapping.AllowanceAmountHeader = GetCellValueNoTrim(_sht, "ALLOWANCE_AMOUNT HEADER");
                        _invPdfMapping.FrightAmount = GetCellValue(_sht, "FRIGHT_AMOUNT");
                        _invPdfMapping.FrightAmountOffset = convert.ToInt(GetCellValue(_sht, "FRIGHT_AMOUNT OFFSET"));
                        _invPdfMapping.FrightAmountHeader = GetCellValueNoTrim(_sht, "FRIGHT_AMOUNT HEADER");
                        _invPdfMapping.PackingCost = GetCellValue(_sht, "PACKING_COST");
                        _invPdfMapping.PackingCostOffset = convert.ToInt(GetCellValue(_sht, "PACKING_COST OFFSET"));
                        _invPdfMapping.PackingCostHeader = GetCellValueNoTrim(_sht, "PACKING_COST HEADER");
                        _invPdfMapping.VatAmount = GetCellValue(_sht, "VAT_AMOUNT");
                        _invPdfMapping.VatAmountOffset = convert.ToInt(GetCellValue(_sht, "VAT_AMOUNT OFFSET"));
                        _invPdfMapping.VatAmountHeader = GetCellValueNoTrim(_sht, "VAT_AMOUNT HEADER");
                        _invPdfMapping.FcaAmount = GetCellValue(_sht, "FCA_AMOUNT");
                        _invPdfMapping.FcaAmountOffset = convert.ToInt(GetCellValue(_sht, "FCA_AMOUNT OFFSET"));
                        _invPdfMapping.FcaAmountHeader = GetCellValueNoTrim(_sht, "FCA_AMOUNT HEADER");
                        _invPdfMapping.CourierCharges = GetCellValue(_sht, "COURIER_CHARGES");
                        _invPdfMapping.CourierChargesOffset = convert.ToInt(GetCellValue(_sht, "COURIER_CHARGES OFFSET"));
                        _invPdfMapping.CourierChargesHeader = GetCellValueNoTrim(_sht, "COURIER_CHARGES HEADER");
                        _invPdfMapping.InsuranceAmount = GetCellValue(_sht, "INSURANCE_AMOUNT");
                        _invPdfMapping.InsuranceAmountOffset = convert.ToInt(GetCellValue(_sht, "INSURANCE_AMOUNT OFFSET"));
                        _invPdfMapping.InsuranceAmountHeader = GetCellValueNoTrim(_sht, "INSURANCE_AMOUNT HEADER");
                        _invPdfMapping.TransactionCharges = GetCellValue(_sht, "TRANSACTION_CHARGES");
                        _invPdfMapping.TransactionChargesOffset = convert.ToInt(GetCellValue(_sht, "TRANSACTION_CHARGES OFFSET"));
                        _invPdfMapping.TransactionChargesHeader = GetCellValueNoTrim(_sht, "TRANSACTION_CHARGES HEADER");
                        _invPdfMapping.FinalTotalAmount = GetCellValue(_sht, "FINAL_TOTAL_AMOUNT");
                        _invPdfMapping.FinalTotalAmountOffset = convert.ToInt(GetCellValue(_sht, "FINAL_TOTAL_AMOUNT OFFSET"));
                        _invPdfMapping.FinalTotalAmountHeader = GetCellValueNoTrim(_sht, "FINAL_TOTAL_AMOUNT HEADER");
                        _invPdfMapping.BuyerName = GetCellValue(_sht, "BUYER_NAME");
                        _invPdfMapping.BuyerAddress = GetCellValue(_sht, "BUYER_ADDRESS");
                        _invPdfMapping.BuyerContactPerson = GetCellValue(_sht, "BUYER_CONTACT_PERSON");
                        _invPdfMapping.BuyerTelephone = GetCellValue(_sht, "BUYER_TELEPHONE");
                        _invPdfMapping.BuyerFax = GetCellValue(_sht, "BUYER_FAX");
                        _invPdfMapping.BuyerEmail = GetCellValue(_sht, "BUYER_EMAIL");
                        _invPdfMapping.BuyerWebsite = GetCellValue(_sht, "BUYER_WEBSITE");
                        _invPdfMapping.SupplierName = GetCellValue(_sht, "SUPPLIER_NAME");
                        _invPdfMapping.SupplierAddress = GetCellValue(_sht, "SUPPLIER_ADDRESS");
                        _invPdfMapping.SupplierContactPerson = GetCellValue(_sht, "SUPPLIER_CONTACT_PERSON");
                        _invPdfMapping.SupplierTelephone = GetCellValue(_sht, "SUPPLIER_TELEPHONE");
                        _invPdfMapping.SupplierFax = GetCellValue(_sht, "SUPPLIER_FAX");
                        _invPdfMapping.SupplierEmail = GetCellValue(_sht, "SUPPLIER_EMAIL");
                        _invPdfMapping.SupplierWebsite = GetCellValue(_sht, "SUPPLIER_WEBSITE");
                        _invPdfMapping.BillingName = GetCellValue(_sht, "BILLING_NAME");
                        _invPdfMapping.BillingAddress = GetCellValue(_sht, "BILLING_ADDRESS");
                        _invPdfMapping.BillingContactPerson = GetCellValue(_sht, "BILLING_CONTACT_PERSON");
                        _invPdfMapping.BillingTelephone = GetCellValue(_sht, "BILLING_TELEPHONE");
                        _invPdfMapping.BillingFax = GetCellValue(_sht, "BILLING_FAX");
                        _invPdfMapping.BillingEmail = GetCellValue(_sht, "BILLING_EMAIL");
                        _invPdfMapping.BillingWebsite = GetCellValue(_sht, "BILLING_WEBSITE");
                        _invPdfMapping.ConsigneeName = GetCellValue(_sht, "CONSIGNEE_NAME");
                        _invPdfMapping.ConsigneeAddress = GetCellValue(_sht, "CONSIGNEE_ADDRESS");
                        _invPdfMapping.ConsigneeContactPerson = GetCellValue(_sht, "CONSIGNEE_CONTACT_PERSON");
                        _invPdfMapping.ConsigneeTelephone = GetCellValue(_sht, "CONSIGNEE_TELEPHONE");
                        _invPdfMapping.ConsigneeFax = GetCellValue(_sht, "CONSIGNEE_FAX");
                        _invPdfMapping.ConsigneeEmail = GetCellValue(_sht, "CONSIGNEE_EMAIL");
                        _invPdfMapping.ConsigneeWebsite = GetCellValue(_sht, "CONSIGNEE_WEBSITE");
                        _invPdfMapping.FfName = GetCellValue(_sht, "FF_NAME");
                        _invPdfMapping.FfAddress = GetCellValue(_sht, "FF_ADDRESS");
                        _invPdfMapping.FfContactPerson = GetCellValue(_sht, "FF_CONTACT_PERSON");
                        _invPdfMapping.FfTelephone = GetCellValue(_sht, "FF_TELEPHONE");
                        _invPdfMapping.FfFax = GetCellValue(_sht, "FF_FAX");
                        _invPdfMapping.FfEmail = GetCellValue(_sht, "FF_EMAIL");
                        _invPdfMapping.FfWebsite = GetCellValue(_sht, "FF_WEBSITE");
                        _invPdfMapping.HeaderLinesCount = convert.ToInt(GetCellValue(_sht, "HEADER_LINES_COUNT"));
                        _invPdfMapping.FooterLinesCount = convert.ToInt(GetCellValue(_sht, "FOOTER_LINES_COUNT"));
                        _invPdfMapping.DateFormat1 = GetCellValue(_sht, "DATE_FORMAT_1");
                        _invPdfMapping.DateFormat2 = GetCellValue(_sht, "DATE_FORMAT_2");
                        _invPdfMapping.VouStartText = GetCellValue(_sht, "VOUCHER_START_TEXT");
                        _invPdfMapping.VouEndText = GetCellValue(_sht, "VOUCHER_END_TEXT");
                        _invPdfMapping.DecimalSeperator = GetCellValue(_sht, "DECIMAL_SEPERATOR");
                        _invPdfMapping.SkipBlankLines = convert.ToInt(GetCellValue(_sht, "SKIP_BLANK_LINES"));

                        _invPdfMapping.BuyerOrgNo = GetCellValue(_sht, "BUYER_ORG_NO");
                        _invPdfMapping.BuyerOrgNoOffset = convert.ToInt(GetCellValue(_sht, "BUYER_ORG_NO OFFSET"));
                        _invPdfMapping.BuyerOrgNoHeader = GetCellValueNoTrim(_sht, "BUYER_ORG_NO HEADER");

                        _invPdfMapping.SupplierOrgNo = GetCellValue(_sht, "SUPPLIER_CUSTOMER_NO");
                        _invPdfMapping.SupplierOrgNoOffset = convert.ToInt(GetCellValue(_sht, "SUPPLIER_CUSTOMER_NO OFFSET"));
                        _invPdfMapping.SupplierOrgNoHeader = GetCellValueNoTrim(_sht, "SUPPLIER_CUSTOMER_NO HEADER");

                        _invPdfMapping.AddHeaderToComment = convert.ToInt(GetCellValue(_sht, "ADD_HEADER_TO_COMMENTS"));
                        _invPdfMapping.AddFooterToComment = convert.ToInt(GetCellValue(_sht, "ADD_FOOTER_TO_COMMENTS"));
                        _invPdfMapping.FieldsFromHeader = GetCellValueNoTrim(_sht, "FIELDS_FROM_HEADER");
                        _invPdfMapping.FieldsFromFooter = GetCellValue(_sht, "FIELDS_FROM_FOOTER");

                        _invPdfMapping.SkipText = GetCellValue(_sht, "SKIP_TEXT");

                        _invPdfMapping.RemarkStartText = GetCellValue(_sht, "REMARKS_START_TEXT");
                        _invPdfMapping.RemarkEndText = GetCellValue(_sht, "REMARKS_END_TEXT");

                        _invPdfMapping.CreditToInvoiceNo = GetCellValue(_sht, "CREDIT_TO_INVOICE_NO");
                        _invPdfMapping.CreditToInvoiceNoOffset = convert.ToInt(GetCellValue(_sht, "CREDIT_TO_INVOICE_NO OFFSET"));
                        _invPdfMapping.CreditToInvoiceNoHeader = GetCellValueNoTrim(_sht, "CREDIT_TO_INVOICE_NO HEADER");

                        _invPdfMapping.SplitFile = convert.ToInt(GetCellValue(_sht, "SPLIT_FILE"));
                        _invPdfMapping.ConstantRowsStartText = convert.ToString(GetCellValue(_sht, "CONSTANT_ROWS_START_TEXT"));
                        _invPdfMapping.ConstantRowsEndText = convert.ToString(GetCellValue(_sht, "CONSTANT_ROWS_END_TEXT"));

                        _invPdfMapping.SplitStartText = convert.ToString(GetCellValue(_sht, "SPLIT_START_TEXT"));
                        _invPdfMapping.SplitEndText = convert.ToString(GetCellValue(_sht, "SPLIT_END_TEXT"));
                        _invPdfMapping.CurrencyMapping = convert.ToString(GetCellValue(_sht, "CURRENCY_MAPPING"));

                        _invPdfMapping.UseItemMapping = convert.ToInt(GetCellValue(_sht, "USE_ITEM_MAPPING"));
                        _invPdfMapping.StartOfSkipText = convert.ToString(GetCellValue(_sht, "START OF SKIP TEXT"));
                        _invPdfMapping.EndOfSkipText = convert.ToString(GetCellValue(_sht, "END OF SKIP TEXT"));


                        _invPdfMapping.HeaderStartContent = convert.ToString(GetCellValue(_sht, "HEADER_START_CONTENT"));
                        _invPdfMapping.HeaderEndContent = convert.ToString(GetCellValue(_sht, "HEADER_END_CONTENT"));
                        _invPdfMapping.FooterStartContent = convert.ToString(GetCellValue(_sht, "FOOTER_START_CONTENT"));
                        _invPdfMapping.FooterEndContent = convert.ToString(GetCellValue(_sht, "FOOTER_END_CONTENT"));

                        _invPdfMapping.Buyer_Addr_Header = convert.ToString(GetCellValue(_sht, "BUYER ADDR HEADER"));
                        _invPdfMapping.Supplier_Addr_Header = convert.ToString(GetCellValue(_sht, "SUPPLIER ADDR HEADER"));
                        _invPdfMapping.Consignee_Addr_Header = convert.ToString(GetCellValue(_sht, "CONSIGNEE ADDR HEADER"));
                        _invPdfMapping.Billing_Addr_Header = convert.ToString(GetCellValue(_sht, "BILLING ADDR HEADER"));
                        _invPdfMapping.FF_Addr_Header = convert.ToString(GetCellValue(_sht, "FF ADDR HEADER"));

                        _invPdfMapping.Mandatory_Fields = convert.ToString(GetCellValue(_sht, "FIELDS_MANDATORY"));
                        _invPdfMapping.OCR_Web_Service = convert.ToInt(GetCellValue(_sht, "OCR_WEB_SERVICE"));

                        _invPdfMapping.Item_Format = convert.ToString(GetCellValue(_sht, "ITEM_FORMAT"));
                        _invPdfMapping.Regex_Fields = convert.ToString(GetCellValue(_sht, "REGEX_TEXT"));

                        _invPdfMapping.OffsetHeaders_Fields = convert.ToString(GetCellValue(_sht, "FIELDS_OFFSET_HEADER"));
                        _invPdfMapping.Check_Voucher_Type = convert.ToString(GetCellValue(_sht, "CHECK_VOUCHER_TYPE"));

                        _invPdfMapping.Replace_Text_Field = convert.ToString(GetCellValue(_sht, "REPLACE_TEXT"));
                        #endregion

                        #region /* Item Mapping */
                        for (int i = 1; i < _xls.Worksheets.Count; i++)
                        {
                            Worksheet _shtItem = _xls.Worksheets[i];
                            string ItemHeader = GetCellValueNoTrim(_shtItem, "ITEM HEADER");
                            if (ItemHeader.Trim().Length > 0)
                            {
                                int MapNumber = convert.ToInt(GetCellValue(_shtItem, "ITEM MAPPING NO."));

                                SmInvoicePdfItemMapping item = SmInvoicePdfItemMapping.Load(_invPdfMapping.InvPdfMapid, MapNumber);
                                if (item == null) item = new SmInvoicePdfItemMapping();
                                item.InvPdfMapid = _invPdfMapping.InvPdfMapid;
                                item.ItemHeaderContent = ItemHeader;
                                item.MapNumber = MapNumber;
                                item.ItemHeaderLineCount = convert.ToInt(GetCellValue(_shtItem, "ITEM HEADER LINE COUNT"));
                                item.ItemNo = convert.ToString(GetCellValue(_shtItem, "ITEM NO"));
                                item.ItemQty = convert.ToString(GetCellValue(_shtItem, "ITEM QUANTITY"));
                                item.ItemUnit = convert.ToString(GetCellValue(_shtItem, "ITEM UNIT"));
                                item.ItemRefno = convert.ToString(GetCellValue(_shtItem, "ITEM REFERENCE NO"));
                                item.ItemDescr = convert.ToString(GetCellValue(_shtItem, "ITEM DESCREPTION"));
                                item.ItemRemark = convert.ToString(GetCellValue(_shtItem, "ITEM REMARKS"));
                                item.ItemUnitprice = convert.ToString(GetCellValue(_shtItem, "ITEM PRICE"));
                                item.ItemDiscount = convert.ToString(GetCellValue(_shtItem, "ITEM DISCOUNT"));
                                item.ItemLeaddays = convert.ToString(GetCellValue(_shtItem, "ITEM LEADDAYS"));
                                item.ItemTotal = convert.ToString(GetCellValue(_shtItem, "ITEM TOTAL"));
                                item.LeaddaysInDate = convert.ToInt(GetCellValue(_shtItem, "LEADDAYS IN DATE"));
                                item.ItemEndContent = convert.ToString(GetCellValueNoTrim(_shtItem, "ITEM END CONTENT"));

                                item.HasNoEquipHeader = convert.ToInt(GetCellValue(_shtItem, "HAS NO EQUIP HEADER"));
                                item.MaxEquipRows = convert.ToInt(GetCellValue(_shtItem, "MAX EQUIP ROWS"));
                                item.MaxEquipRange = convert.ToInt(GetCellValue(_shtItem, "MAX EQUIP RANGE"));

                                string EquipName = convert.ToString(GetCellValue(_shtItem, "EQUIP NAME HEADER"));
                                string EquipType = convert.ToString(GetCellValue(_shtItem, "EQUIP TYPE HEADER"));
                                string SerNo = convert.ToString(GetCellValue(_shtItem, "EQUIP SER. NO HEADER"));
                                string EquipMaker = convert.ToString(GetCellValue(_shtItem, "EQUIP MAKER HEADER"));
                                string EquipNote = convert.ToString(GetCellValue(_shtItem, "EQUIP NOTE HEADER"));
                                item.ItemEquipment = EquipName + "/" + EquipType + "/" + SerNo + "/" + EquipMaker + "/" + EquipNote;

                                item.ItemGroupHeader = convert.ToString(GetCellValue(_shtItem, "ITEM GROUP HEADER"));
                                item.ItemMinLines = convert.ToInt(GetCellValue(_shtItem, "ITEM MIN LINE COUNT"));
                                item.ItemRemarksAppendText = convert.ToString(GetCellValue(_shtItem, "ITEM REMARKS APPEND TEXT"));
                                item.ItemRemarksInitials = convert.ToString(GetCellValue(_shtItem, "ITEM REMARKS INITIALS"));
                                item.CheckContentBelowItem = convert.ToInt(GetCellValue(_shtItem, "CHECK REMARKS & EQUIP BELOW ITEM"));
                                item.ExtraColumns = convert.ToString(GetCellValue(_shtItem, "EXTRA COLUMNS"));
                                item.ExtraColumnsHeader = convert.ToString(GetCellValue(_shtItem, "EXTRA COLUMNS HEADER"));

                                item.ReadItemnoUptoMinlines = convert.ToInt(GetCellValue(_shtItem, "READ ITEM NO UPTO MIN LINES"));
                                item.EquipNameRange = convert.ToString(GetCellValue(_shtItem, "EQUIP NAME RANGE"));
                                item.EquipTypeRange = convert.ToString(GetCellValue(_shtItem, "EQUIP TYPE RANGE"));
                                item.EquipSernoRange = convert.ToString(GetCellValue(_shtItem, "EQUIP SER. NO. RANGE"));
                                item.EquipMakerRange = convert.ToString(GetCellValue(_shtItem, "EQUIP MAKER RANGE"));
                                item.EquipNoteRange = convert.ToString(GetCellValue(_shtItem, "EQUIP NOTE RANGE"));
                                item.AppendUom = convert.ToInt(GetCellValue(_shtItem, "APPEND UOM"));
                                item.MakerrefExtranoLineCount = convert.ToInt(GetCellValue(_shtItem, "MAKER REF. / EXTRA NO. LINE COUNT"));
                                item.MakerrefRange = convert.ToString(GetCellValue(_shtItem, "MAKER REF. RANGE"));
                                item.ExtranoRange = convert.ToString(GetCellValue(_shtItem, "EXTRA NO. RANGE"));
                                item.ReadPartnoFromLastLine = convert.ToInt(GetCellValue(_shtItem, "READ PART NO FROM LAST LINE"));
                                item.ItemCurrency = convert.ToString(GetCellValue(_shtItem, "ITEM CURRENCY"));
                                item.AppendRefNo = convert.ToInt(GetCellValue(_shtItem, "APPEND REF NO"));

                                item.IsBoldText = convert.ToInt(GetCellValue(_shtItem, "IS BOLD TEXT"));
                                item.IsQtyUomMerged = convert.ToInt(GetCellValue(_shtItem, "IS_QTY_UOM_MERGED"));

                                ItemColl.Add(item);
                            }
                        }
                        #endregion

                        DataSet _dsHeaderMapping = _invHeaderMap.ExportHeader();
                        DataSet _dsMapping = _invPdfMapping.Export();
                        DataSet _dsItemMapping = SmInvoicePdfItemMapping.ExportItems(ItemColl);
                        if (_dsMapping != null && _dsMapping.Tables.Count > 0 && _dsMapping.Tables[0].Rows.Count > 0)
                        {
                            object[] _webObj = new object[3];
                            _webObj[0] = _dsHeaderMapping;
                            _webObj[1] = _dsMapping;
                            _webObj[2] = _dsItemMapping;
                            string _sData = (string)_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "Update_SM_INVOICE_PDF_MAPPING_Details", _webObj);
                            if (_sData == "")
                            {
                                SetAuditLog("LeSMonitor", "Voucher PDF mapping with Map code " + _invPdfMapping.InvMapCode + " updated by [" + UserHostAddress + "]. ", "Uploaded", "", "", "", "");
                                CopyReferenceFiles(_invPdfMapping.InvMapCode, REFFILE_UPLOADPATH, "VOUCHER_PDF");
                                result = Path.GetFileName(Filename) + " is uploaded successfully.";
                            }
                            else
                            {
                                throw new Exception(_sData);
                            }
                        }
                        else
                        {
                            throw new Exception("Unable to fill mapping details.");
                        }
                    }
                }
                else
                {
                    throw new Exception("Unable to get MAPID");
                }
            }
            catch (Exception ex)
            {
                SetLog(ex.StackTrace);
                throw ex;
            }
            return result;
        }

        public string SetVoucherPdfMapping(int MapID,  string TemplatePath, string SessionID)
        {
            string downloadPath = TemplatePath;
            try
            {
                string temp = System.Configuration.ConfigurationManager.AppSettings["DOWNLOAD_ATTACHMENT"];
                temp = temp + "\\" + SessionID;
                if (!Directory.Exists(temp)) Directory.CreateDirectory(temp);

                string[] _parm = new string[1];
                _parm[0] = "INV_PDF_MAPID=" + MapID;
                object[] _obj = new object[2];
                _obj[0] = "SELECT * FROM SM_INVOICE_PDF_MAPPING WHERE INV_PDF_MAPID = @INV_PDF_MAPID";
                _obj[1] = _parm;
                byte[] _byteData = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _obj);
                DataSet _ds = _compressor.Decompress(_byteData);

                string[] _parm1 = new string[1];
                _parm1[0] = "INV_PDF_MAPID=" + MapID;
                object[] _obj1 = new object[2];
                _obj1[0] = "SELECT * FROM SM_INV_PDF_BUYER_SUPPLIER_LINK WHERE INV_PDF_MAPID = @INV_PDF_MAPID";
                _obj1[1] = _parm1;
                byte[] _byteData1 = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _obj1);
                DataSet _ds1 = _compressor.Decompress(_byteData1);

                SmInvoicePdfItemMappingCollection _itemCollection = SmInvoicePdfItemMapping.Load_by_InvPdfMapid(MapID);

                if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                {
                    SmInvoicePdfMapping _invPdfMapping = new SmInvoicePdfMapping();
                    _invPdfMapping.Load(_ds.Tables[0].Rows[0]);

                    SmInvPdfBuyerSupplierLink _invHeaderMapping = new SmInvPdfBuyerSupplierLink();
                    _invHeaderMapping.Load(_ds1.Tables[0].Rows[0]);

                    if (_invPdfMapping != null)
                    {
                        downloadPath = temp + "\\" + _invPdfMapping.InvMapCode + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssff") + ".xls";

                        License _license = new License();
                        _license.SetLicense("Aspose.Total.lic");

                        Workbook _xls = new Workbook(TemplatePath);

                        FindOptions _option = new FindOptions();
                        _option.CaseSensitive = true;
                        _option.LookAtType = LookAtType.EntireContent;
                        _option.SeachOrderByRows = true;

                        #region /* Set Values */
                        Worksheet sht = _xls.Worksheets[0];

                        // GROUP ID 
                        Cell _mapID = _xls.Worksheets[0].Cells.Find("MAP_CODE", null, _option);
                        _xls.Worksheets[0].Cells[_mapID.Row, _mapID.Column + 1].Value = _invPdfMapping.InvMapCode;
                        _xls.Worksheets[0].Cells[_mapID.Row, _mapID.Column + 2].Value = _invPdfMapping.InvPdfMapid;

                        if (_invHeaderMapping != null)
                        {
                            SetCellValue(sht, "MAPPING RANGE 1", convert.ToString(_invHeaderMapping.Mapping1).Trim());
                            SetCellValue(sht, "MAPPING RANGE 1 VALUE", convert.ToString(_invHeaderMapping.Mapping1Value).Trim());
                            SetCellValue(sht, "MAPPING RANGE 2", convert.ToString(_invHeaderMapping.Mapping2).Trim());
                            SetCellValue(sht, "MAPPING RANGE 2 VALUE", convert.ToString(_invHeaderMapping.Mapping2Value).Trim());
                            SetCellValue(sht, "MAPPING RANGE 3", convert.ToString(_invHeaderMapping.Mapping3).Trim());
                            SetCellValue(sht, "MAPPING RANGE 3 VALUE", convert.ToString(_invHeaderMapping.Mapping3Value).Trim());
                        }

                        SetCellValue(sht, "VOUCHERTYPE", convert.ToString(_invPdfMapping.Vouchertype).Trim());
                        SetCellValue(sht, "VOUCHER_NO", convert.ToString(_invPdfMapping.VoucherNo).Trim());
                        SetCellValue(sht, "VOUCHER_NO OFFSET", convert.ToInt(_invPdfMapping.VoucherNoOffset));
                        SetCellValue(sht, "VOUCHER_NO HEADER", convert.ToString(_invPdfMapping.VoucherNoHeader).Trim());
                        SetCellValue(sht, "PO_NO", convert.ToString(_invPdfMapping.PoNo).Trim());
                        SetCellValue(sht, "PO_NO OFFSET", convert.ToInt(_invPdfMapping.PoNoOffset));
                        SetCellValue(sht, "PO_NO HEADER", convert.ToString(_invPdfMapping.PoNoHeader).Trim());
                        SetCellValue(sht, "PO_REF_NO", convert.ToString(_invPdfMapping.PoRefNo).Trim());
                        SetCellValue(sht, "PO_REF_NO OFFSET", convert.ToInt(_invPdfMapping.PoRefNoOffset));
                        SetCellValue(sht, "PO_REF_NO HEADER", convert.ToString(_invPdfMapping.PoRefNoHeader).Trim());
                        SetCellValue(sht, "SUPPLIER_DISPATCH_NO", convert.ToString(_invPdfMapping.SupplierDispatchNo).Trim());
                        SetCellValue(sht, "SUPPLIER_DISPATCH_NO OFFSET", convert.ToInt(_invPdfMapping.SupplierDispatchNoOffset));
                        SetCellValue(sht, "SUPPLIER_DISPATCH_NO HEADER", convert.ToString(_invPdfMapping.SupplierDispatchNoHeader).Trim());
                        SetCellValue(sht, "FORWARDER_DISPATCH_NO", convert.ToString(_invPdfMapping.ForwarderDispatchNo).Trim());
                        SetCellValue(sht, "FORWARDER_DISPATCH_NO OFFSET", convert.ToInt(_invPdfMapping.ForwarderDispatchNoOffset));
                        SetCellValue(sht, "FORWARDER_DISPATCH_NO HEADER", convert.ToString(_invPdfMapping.ForwarderDispatchNoHeader).Trim());
                        SetCellValue(sht, "SUPPLIER_CUSTOMER_NO", convert.ToString(_invPdfMapping.SupplierCustomerNo).Trim());
                        SetCellValue(sht, "SUPPLIER_CUSTOMER_NO OFFSET", convert.ToInt(_invPdfMapping.SupplierCustomerNoOffset));
                        SetCellValue(sht, "SUPPLIER_CUSTOMER_NO HEADER", _invPdfMapping.SupplierCustomerNoHeader);
                        //SetCellValue(sht, "BUYER_REF", convert.ToString(_invPdfMapping.BuyerRef).Trim());
                        //SetCellValue(sht, "BUYER_REF OFFSET", convert.ToInt(_invPdfMapping.BuyerRefOffset));
                        //SetCellValue(sht, "BUYER_REF HEADER", convert.ToString(_invPdfMapping.BuyerRefHeader).Trim());
                        //SetCellValue(sht, "SUPPLIER_REF", convert.ToString(_invPdfMapping.SupplierRef).Trim());
                        //SetCellValue(sht, "SUPPLIER_REF OFFSET", convert.ToInt(_invPdfMapping.SupplierRefOffset));
                        //SetCellValue(sht, "SUPPLIER_REF HEADER", convert.ToString(_invPdfMapping.SupplierRefHeader).Trim());
                        SetCellValue(sht, "CURRENCY", convert.ToString(_invPdfMapping.Currency).Trim());
                        SetCellValue(sht, "CURRENCY OFFSET", convert.ToInt(_invPdfMapping.CurrencyOffset));
                        SetCellValue(sht, "CURRENCY HEADER", convert.ToString(_invPdfMapping.CurrencyHeader).Trim());
                        SetCellValue(sht, "VESSEL_IMO_NO", convert.ToString(_invPdfMapping.VesselImoNo).Trim());
                        SetCellValue(sht, "VESSEL_IMO_NO OFFSET", convert.ToInt(_invPdfMapping.VesselImoNoOffset));
                        SetCellValue(sht, "VESSEL_IMO_NO HEADER", convert.ToString(_invPdfMapping.VesselImoNoHeader).Trim());
                        SetCellValue(sht, "VESSEL_NAME", convert.ToString(_invPdfMapping.VesselName).Trim());
                        SetCellValue(sht, "VESSEL_NAME OFFSET", convert.ToInt(_invPdfMapping.VesselNameOffset));
                        SetCellValue(sht, "VESSEL_NAME HEADER", convert.ToString(_invPdfMapping.VesselNameHeader).Trim());
                        SetCellValue(sht, "PORT_CODE", convert.ToString(_invPdfMapping.PortCode).Trim());
                        SetCellValue(sht, "PORT_CODE OFFSET", convert.ToInt(_invPdfMapping.PortCodeOffset));
                        SetCellValue(sht, "PORT_CODE HEADER", convert.ToString(_invPdfMapping.PortCodeHeader).Trim());
                        SetCellValue(sht, "PORT_NAME", convert.ToString(_invPdfMapping.PortName).Trim());
                        SetCellValue(sht, "PORT_NAME OFFSET", convert.ToInt(_invPdfMapping.PortNameOffset));
                        SetCellValue(sht, "PORT_NAME HEADER", convert.ToString(_invPdfMapping.PortNameHeader).Trim());
                        SetCellValue(sht, "PAYMENT_TERMS", convert.ToString(_invPdfMapping.PaymentTerms).Trim());
                        SetCellValue(sht, "PAYMENT_TERMS OFFSET", convert.ToInt(_invPdfMapping.PaymentTermsOffset));
                        SetCellValue(sht, "PAYMENT_TERMS HEADER", convert.ToString(_invPdfMapping.PaymentTermsHeader).Trim());
                        SetCellValue(sht, "DELIVERY_TERMS", convert.ToString(_invPdfMapping.DeliveryTerms).Trim());
                        SetCellValue(sht, "DELIVERY_TERMS OFFSET", convert.ToInt(_invPdfMapping.DeliveryTermsOffset));
                        SetCellValue(sht, "DELIVERY_TERMS HEADER", convert.ToString(_invPdfMapping.DeliveryTermsHeader).Trim());
                        SetCellValue(sht, "SUPPLIER_REMARKS", convert.ToString(_invPdfMapping.SupplierRemarks).Trim());
                        SetCellValue(sht, "SUPPLIER_REMARKS OFFSET", convert.ToInt(_invPdfMapping.SupplierRemarksOffset));
                        SetCellValue(sht, "SUPPLIER_REMARKS HEADER", convert.ToString(_invPdfMapping.SupplierRemarksHeader).Trim());
                        SetCellValue(sht, "ITEMS_COUNT", convert.ToString(_invPdfMapping.ItemsCount).Trim());
                        SetCellValue(sht, "ITEMS_COUNT OFFSET", convert.ToInt(_invPdfMapping.ItemsCountOffset));
                        SetCellValue(sht, "ITEMS_COUNT HEADER", convert.ToString(_invPdfMapping.ItemsCountHeader).Trim());
                        SetCellValue(sht, "COURIER_NAME", convert.ToString(_invPdfMapping.CourierName).Trim());
                        SetCellValue(sht, "COURIER_NAME OFFSET", convert.ToInt(_invPdfMapping.CourierNameOffset));
                        SetCellValue(sht, "COURIER_NAME HEADER", convert.ToString(_invPdfMapping.CourierNameHeader).Trim());
                        SetCellValue(sht, "BUYER_VAT_NO", convert.ToString(_invPdfMapping.BuyerVatNo).Trim());
                        SetCellValue(sht, "BUYER_VAT_NO OFFSET", convert.ToInt(_invPdfMapping.BuyerVatNoOffset));
                        SetCellValue(sht, "BUYER_VAT_NO HEADER", convert.ToString(_invPdfMapping.BuyerVatNoHeader).Trim());
                        SetCellValue(sht, "SUPPLIER_VAT_NO", convert.ToString(_invPdfMapping.SupplierVatNo).Trim());
                        SetCellValue(sht, "SUPPLIER_VAT_NO OFFSET", convert.ToInt(_invPdfMapping.SupplierVatNoOffset));
                        SetCellValue(sht, "SUPPLIER_VAT_NO HEADER", convert.ToString(_invPdfMapping.SupplierVatNoHeader).Trim());
                        SetCellValue(sht, "VOUCHER_DATE", convert.ToString(_invPdfMapping.VoucherDate).Trim());
                        SetCellValue(sht, "VOUCHER_DATE OFFSET", convert.ToInt(_invPdfMapping.VoucherDateOffset));
                        SetCellValue(sht, "VOUCHER_DATE HEADER", convert.ToString(_invPdfMapping.VoucherDateHeader).Trim());
                        SetCellValue(sht, "VOUCHER_DUE_DATE", convert.ToString(_invPdfMapping.VoucherDueDate).Trim());
                        SetCellValue(sht, "VOUCHER_DUE_DATE OFFSET", convert.ToInt(_invPdfMapping.VoucherDueDateOffset));
                        SetCellValue(sht, "VOUCHER_DUE_DATE HEADER", convert.ToString(_invPdfMapping.VoucherDueDateHeader).Trim());
                        SetCellValue(sht, "BUYER_PO_DATE", convert.ToString(_invPdfMapping.BuyerPoDate).Trim());
                        SetCellValue(sht, "BUYER_PO_DATE OFFSET", convert.ToInt(_invPdfMapping.BuyerPoDateOffset));
                        SetCellValue(sht, "BUYER_PO_DATE HEADER", convert.ToString(_invPdfMapping.BuyerPoDateHeader).Trim());
                        SetCellValue(sht, "DISPATCH_DATE", convert.ToString(_invPdfMapping.DispatchDate).Trim());
                        SetCellValue(sht, "DISPATCH_DATE OFFSET", convert.ToInt(_invPdfMapping.DispatchDateOffset));
                        SetCellValue(sht, "DISPATCH_DATE HEADER", convert.ToString(_invPdfMapping.DispatchDateHeader).Trim());
                        SetCellValue(sht, "LATE_DATE", convert.ToString(_invPdfMapping.LateDate).Trim());
                        SetCellValue(sht, "LATE_DATE OFFSET", convert.ToInt(_invPdfMapping.LateDateOffset));
                        SetCellValue(sht, "LATE_DATE HEADER", convert.ToString(_invPdfMapping.LateDateHeader).Trim());
                        SetCellValue(sht, "BANK_NAME", convert.ToString(_invPdfMapping.BankName).Trim());
                        SetCellValue(sht, "BANK_NAME OFFSET", convert.ToInt(_invPdfMapping.BankNameOffset));
                        SetCellValue(sht, "BANK_NAME HEADER", convert.ToString(_invPdfMapping.BankNameHeader).Trim());
                        SetCellValue(sht, "ACCOUNT_NAME", convert.ToString(_invPdfMapping.AccountName).Trim());
                        SetCellValue(sht, "ACCOUNT_NAME OFFSET", convert.ToInt(_invPdfMapping.AccountNameOffset));
                        SetCellValue(sht, "ACCOUNT_NAME HEADER", convert.ToString(_invPdfMapping.AccountNameHeader).Trim());
                        SetCellValue(sht, "COC", convert.ToString(_invPdfMapping.Coc).Trim());
                        SetCellValue(sht, "COC OFFSET", convert.ToInt(_invPdfMapping.CocOffset));
                        SetCellValue(sht, "COC HEADER", convert.ToString(_invPdfMapping.CocHeader).Trim());
                        SetCellValue(sht, "SORT_CODE", convert.ToString(_invPdfMapping.SortCode).Trim());
                        SetCellValue(sht, "SORT_CODE OFFSET", convert.ToInt(_invPdfMapping.SortCodeOffset));
                        SetCellValue(sht, "SORT_CODE HEADER", convert.ToString(_invPdfMapping.SortCodeHeader).Trim());
                        SetCellValue(sht, "IBAN_NO", convert.ToString(_invPdfMapping.IbanNo).Trim());
                        SetCellValue(sht, "IBAN_NO OFFSET", convert.ToInt(_invPdfMapping.IbanNoOffset));
                        SetCellValue(sht, "IBAN_NO HEADER", convert.ToString(_invPdfMapping.IbanNoHeader).Trim());
                        SetCellValue(sht, "SWIFT_BIC_NO", convert.ToString(_invPdfMapping.SwiftNo).Trim());
                        SetCellValue(sht, "SWIFT_BIC_NO OFFSET", convert.ToInt(_invPdfMapping.SwiftNoOffset));
                        SetCellValue(sht, "SWIFT_BIC_NO HEADER", convert.ToString(_invPdfMapping.SwiftNoHeader).Trim());
                        SetCellValue(sht, "ACCOUNT_NO", convert.ToString(_invPdfMapping.AccountNo).Trim());
                        SetCellValue(sht, "ACCOUNT_NO OFFSET", convert.ToInt(_invPdfMapping.AccountNoOffset));
                        SetCellValue(sht, "ACCOUNT_NO HEADER", convert.ToString(_invPdfMapping.AccountNoHeader).Trim());
                        SetCellValue(sht, "KVK_NO", convert.ToString(_invPdfMapping.KvkNo).Trim());
                        SetCellValue(sht, "KVK_NO OFFSET", convert.ToInt(_invPdfMapping.KvkNoOffset));
                        SetCellValue(sht, "KVK_NO HEADER", convert.ToString(_invPdfMapping.KvkNoHeader).Trim());
                        SetCellValue(sht, "ITEMS_TOTAL_AMOUNT", convert.ToString(_invPdfMapping.ItemsTotalAmount).Trim());
                        SetCellValue(sht, "ITEMS_TOTAL_AMOUNT OFFSET", convert.ToInt(_invPdfMapping.ItemsTotalAmountOffset));
                        SetCellValue(sht, "ITEMS_TOTAL_AMOUNT HEADER", convert.ToString(_invPdfMapping.ItemsTotalAmountHeader).Trim());
                        SetCellValue(sht, "ALLOWANCE_AMOUNT", convert.ToString(_invPdfMapping.AllowanceAmount).Trim());
                        SetCellValue(sht, "ALLOWANCE_AMOUNT OFFSET", convert.ToInt(_invPdfMapping.AllowanceAmountOffset));
                        SetCellValue(sht, "ALLOWANCE_AMOUNT HEADER", convert.ToString(_invPdfMapping.AllowanceAmountHeader).Trim());
                        SetCellValue(sht, "FRIGHT_AMOUNT", convert.ToString(_invPdfMapping.FrightAmount).Trim());
                        SetCellValue(sht, "FRIGHT_AMOUNT OFFSET", convert.ToInt(_invPdfMapping.FrightAmountOffset));
                        SetCellValue(sht, "FRIGHT_AMOUNT HEADER", convert.ToString(_invPdfMapping.FrightAmountHeader).Trim());
                        SetCellValue(sht, "PACKING_COST", convert.ToString(_invPdfMapping.PackingCost).Trim());
                        SetCellValue(sht, "PACKING_COST OFFSET", convert.ToInt(_invPdfMapping.PackingCostOffset));
                        SetCellValue(sht, "PACKING_COST HEADER", convert.ToString(_invPdfMapping.PackingCostHeader).Trim());
                        SetCellValue(sht, "VAT_AMOUNT", convert.ToString(_invPdfMapping.VatAmount).Trim());
                        SetCellValue(sht, "VAT_AMOUNT OFFSET", convert.ToInt(_invPdfMapping.VatAmountOffset));
                        SetCellValue(sht, "VAT_AMOUNT HEADER", convert.ToString(_invPdfMapping.VatAmountHeader).Trim());
                        SetCellValue(sht, "FCA_AMOUNT", convert.ToString(_invPdfMapping.FcaAmount).Trim());
                        SetCellValue(sht, "FCA_AMOUNT OFFSET", convert.ToInt(_invPdfMapping.FcaAmountOffset));
                        SetCellValue(sht, "FCA_AMOUNT HEADER", convert.ToString(_invPdfMapping.FcaAmountHeader).Trim());
                        SetCellValue(sht, "COURIER_CHARGES", convert.ToString(_invPdfMapping.CourierCharges).Trim());
                        SetCellValue(sht, "COURIER_CHARGES OFFSET", convert.ToInt(_invPdfMapping.CourierChargesOffset));
                        SetCellValue(sht, "COURIER_CHARGES HEADER", convert.ToString(_invPdfMapping.CourierChargesHeader).Trim());
                        SetCellValue(sht, "INSURANCE_AMOUNT", convert.ToString(_invPdfMapping.InsuranceAmount).Trim());
                        SetCellValue(sht, "INSURANCE_AMOUNT OFFSET", convert.ToInt(_invPdfMapping.InsuranceAmountOffset));
                        SetCellValue(sht, "INSURANCE_AMOUNT HEADER", convert.ToString(_invPdfMapping.InsuranceAmountHeader).Trim());
                        SetCellValue(sht, "TRANSACTION_CHARGES", convert.ToString(_invPdfMapping.TransactionCharges).Trim());
                        SetCellValue(sht, "TRANSACTION_CHARGES OFFSET", convert.ToInt(_invPdfMapping.TransactionChargesOffset));
                        SetCellValue(sht, "TRANSACTION_CHARGES HEADER", convert.ToString(_invPdfMapping.TransactionChargesHeader).Trim());
                        SetCellValue(sht, "FINAL_TOTAL_AMOUNT", convert.ToString(_invPdfMapping.FinalTotalAmount).Trim());
                        SetCellValue(sht, "FINAL_TOTAL_AMOUNT OFFSET", convert.ToInt(_invPdfMapping.FinalTotalAmountOffset));
                        SetCellValue(sht, "FINAL_TOTAL_AMOUNT HEADER", convert.ToString(_invPdfMapping.FinalTotalAmountHeader).Trim());
                        SetCellValue(sht, "BUYER_NAME", convert.ToString(_invPdfMapping.BuyerName).Trim());
                        SetCellValue(sht, "BUYER_ADDRESS", convert.ToString(_invPdfMapping.BuyerAddress).Trim());
                        SetCellValue(sht, "BUYER_CONTACT_PERSON", convert.ToString(_invPdfMapping.BuyerContactPerson).Trim());
                        SetCellValue(sht, "BUYER_TELEPHONE", convert.ToString(_invPdfMapping.BuyerTelephone).Trim());
                        SetCellValue(sht, "BUYER_FAX", convert.ToString(_invPdfMapping.BuyerFax).Trim());
                        SetCellValue(sht, "BUYER_EMAIL", convert.ToString(_invPdfMapping.BuyerEmail).Trim());
                        SetCellValue(sht, "BUYER_WEBSITE", convert.ToString(_invPdfMapping.BuyerWebsite).Trim());
                        SetCellValue(sht, "SUPPLIER_NAME", convert.ToString(_invPdfMapping.SupplierName).Trim());
                        SetCellValue(sht, "SUPPLIER_ADDRESS", convert.ToString(_invPdfMapping.SupplierAddress).Trim());
                        SetCellValue(sht, "SUPPLIER_CONTACT_PERSON", convert.ToString(_invPdfMapping.SupplierContactPerson).Trim());
                        SetCellValue(sht, "SUPPLIER_TELEPHONE", convert.ToString(_invPdfMapping.SupplierTelephone).Trim());
                        SetCellValue(sht, "SUPPLIER_FAX", convert.ToString(_invPdfMapping.SupplierFax).Trim());
                        SetCellValue(sht, "SUPPLIER_EMAIL", convert.ToString(_invPdfMapping.SupplierEmail).Trim());
                        SetCellValue(sht, "SUPPLIER_WEBSITE", convert.ToString(_invPdfMapping.SupplierWebsite).Trim());
                        SetCellValue(sht, "BILLING_NAME", convert.ToString(_invPdfMapping.BillingName).Trim());
                        SetCellValue(sht, "BILLING_ADDRESS", convert.ToString(_invPdfMapping.BillingAddress).Trim());
                        SetCellValue(sht, "BILLING_CONTACT_PERSON", convert.ToString(_invPdfMapping.BillingContactPerson).Trim());
                        SetCellValue(sht, "BILLING_TELEPHONE", convert.ToString(_invPdfMapping.BillingTelephone).Trim());
                        SetCellValue(sht, "BILLING_FAX", convert.ToString(_invPdfMapping.BillingFax).Trim());
                        SetCellValue(sht, "BILLING_EMAIL", convert.ToString(_invPdfMapping.BillingEmail).Trim());
                        SetCellValue(sht, "BILLING_WEBSITE", convert.ToString(_invPdfMapping.BillingWebsite).Trim());
                        SetCellValue(sht, "CONSIGNEE_NAME", convert.ToString(_invPdfMapping.ConsigneeName).Trim());
                        SetCellValue(sht, "CONSIGNEE_ADDRESS", convert.ToString(_invPdfMapping.ConsigneeAddress).Trim());
                        SetCellValue(sht, "CONSIGNEE_CONTACT_PERSON", convert.ToString(_invPdfMapping.ConsigneeContactPerson).Trim());
                        SetCellValue(sht, "CONSIGNEE_TELEPHONE", convert.ToString(_invPdfMapping.ConsigneeTelephone).Trim());
                        SetCellValue(sht, "CONSIGNEE_FAX", convert.ToString(_invPdfMapping.ConsigneeFax).Trim());
                        SetCellValue(sht, "CONSIGNEE_EMAIL", convert.ToString(_invPdfMapping.ConsigneeEmail).Trim());
                        SetCellValue(sht, "CONSIGNEE_WEBSITE", convert.ToString(_invPdfMapping.ConsigneeWebsite).Trim());
                        SetCellValue(sht, "FF_NAME", convert.ToString(_invPdfMapping.FfName).Trim());
                        SetCellValue(sht, "FF_ADDRESS", convert.ToString(_invPdfMapping.FfAddress).Trim());
                        SetCellValue(sht, "FF_CONTACT_PERSON", convert.ToString(_invPdfMapping.FfContactPerson).Trim());
                        SetCellValue(sht, "FF_TELEPHONE", convert.ToString(_invPdfMapping.FfTelephone).Trim());
                        SetCellValue(sht, "FF_FAX", convert.ToString(_invPdfMapping.FfFax).Trim());
                        SetCellValue(sht, "FF_EMAIL", convert.ToString(_invPdfMapping.FfEmail).Trim());
                        SetCellValue(sht, "FF_WEBSITE", convert.ToString(_invPdfMapping.FfWebsite).Trim());
                        SetCellValue(sht, "HEADER_LINES_COUNT", _invPdfMapping.HeaderLinesCount);
                        SetCellValue(sht, "FOOTER_LINES_COUNT", _invPdfMapping.FooterLinesCount);
                        SetCellValue(sht, "DATE_FORMAT_1", convert.ToString(_invPdfMapping.DateFormat1).Trim());
                        SetCellValue(sht, "DATE_FORMAT_2", convert.ToString(_invPdfMapping.DateFormat2).Trim());
                        SetCellValue(sht, "VOUCHER_START_TEXT", convert.ToString(_invPdfMapping.VouStartText).Trim());
                        SetCellValue(sht, "VOUCHER_END_TEXT", convert.ToString(_invPdfMapping.VouEndText).Trim());
                        SetCellValue(sht, "DECIMAL_SEPERATOR", convert.ToString(_invPdfMapping.DecimalSeperator).Trim());
                        SetCellValue(sht, "SKIP_BLANK_LINES", convert.ToInt(_invPdfMapping.SkipBlankLines));

                        SetCellValue(sht, "BUYER_ORG_NO", convert.ToString(_invPdfMapping.BuyerOrgNo).Trim());
                        SetCellValue(sht, "BUYER_ORG_NO OFFSET", convert.ToInt(_invPdfMapping.BuyerOrgNoOffset));
                        SetCellValue(sht, "BUYER_ORG_NO HEADER", convert.ToString(_invPdfMapping.BuyerOrgNoHeader).Trim());

                        SetCellValue(sht, "SUPPLIER_ORG_NO", convert.ToString(_invPdfMapping.SupplierOrgNo).Trim());
                        SetCellValue(sht, "SUPPLIER_ORG_NO OFFSET", convert.ToInt(_invPdfMapping.SupplierOrgNoOffset));
                        SetCellValue(sht, "SUPPLIER_ORG_NO HEADER", convert.ToString(_invPdfMapping.SupplierOrgNoHeader).Trim());

                        SetCellValue(sht, "ADD_HEADER_TO_COMMENTS", convert.ToString(_invPdfMapping.AddHeaderToComment).Trim());
                        SetCellValue(sht, "ADD_FOOTER_TO_COMMENTS", convert.ToString(_invPdfMapping.AddFooterToComment).Trim());
                        SetCellValue(sht, "FIELDS_FROM_HEADER", convert.ToString(_invPdfMapping.FieldsFromHeader).Trim());
                        SetCellValue(sht, "FIELDS_FROM_FOOTER", convert.ToString(_invPdfMapping.FieldsFromFooter).Trim());

                        SetCellValue(sht, "SKIP_TEXT", convert.ToString(_invPdfMapping.SkipText).Trim());

                        SetCellValue(sht, "REMARKS_START_TEXT", convert.ToString(_invPdfMapping.RemarkStartText).Trim());
                        SetCellValue(sht, "REMARKS_END_TEXT", convert.ToString(_invPdfMapping.RemarkEndText).Trim());


                        SetCellValue(sht, "CREDIT_TO_INVOICE_NO", convert.ToString(_invPdfMapping.CreditToInvoiceNo).Trim());
                        SetCellValue(sht, "CREDIT_TO_INVOICE_NO OFFSET", convert.ToInt(_invPdfMapping.CreditToInvoiceNoOffset));
                        SetCellValue(sht, "CREDIT_TO_INVOICE_NO HEADER", convert.ToString(_invPdfMapping.CreditToInvoiceNoHeader).Trim());

                        SetCellValue(sht, "SPLIT_FILE", convert.ToInt(_invPdfMapping.SplitFile));
                        SetCellValue(sht, "CONSTANT_ROWS_START_TEXT", convert.ToString(_invPdfMapping.ConstantRowsStartText));
                        SetCellValue(sht, "CONSTANT_ROWS_END_TEXT", convert.ToString(_invPdfMapping.ConstantRowsEndText));


                        SetCellValue(sht, "SPLIT_START_TEXT", convert.ToString(_invPdfMapping.SplitStartText).Trim());
                        SetCellValue(sht, "SPLIT_END_TEXT", convert.ToString(_invPdfMapping.SplitEndText).Trim());

                        SetCellValue(sht, "CURRENCY_MAPPING", convert.ToString(_invPdfMapping.CurrencyMapping).Trim());

                        SetCellValue(sht, "USE_ITEM_MAPPING", convert.ToString(_invPdfMapping.UseItemMapping).Trim());
                        SetCellValue(sht, "START OF SKIP TEXT", convert.ToString(_invPdfMapping.StartOfSkipText).Trim());
                        SetCellValue(sht, "END OF SKIP TEXT", convert.ToString(_invPdfMapping.EndOfSkipText).Trim());

                        SetCellValue(sht, "HEADER_START_CONTENT", convert.ToString(_invPdfMapping.HeaderStartContent).Trim());
                        SetCellValue(sht, "HEADER_END_CONTENT", convert.ToString(_invPdfMapping.HeaderEndContent).Trim());
                        SetCellValue(sht, "FOOTER_START_CONTENT", convert.ToString(_invPdfMapping.FooterStartContent).Trim());
                        SetCellValue(sht, "FOOTER_END_CONTENT", convert.ToString(_invPdfMapping.FooterEndContent).Trim());

                        SetCellValue(sht, "BUYER ADDR HEADER", convert.ToString(_invPdfMapping.Buyer_Addr_Header).Trim());
                        SetCellValue(sht, "SUPPLIER ADDR HEADER", convert.ToString(_invPdfMapping.Supplier_Addr_Header).Trim());
                        SetCellValue(sht, "CONSIGNEE ADDR HEADER", convert.ToString(_invPdfMapping.Consignee_Addr_Header).Trim());
                        SetCellValue(sht, "BILLING ADDR HEADER", convert.ToString(_invPdfMapping.Billing_Addr_Header).Trim());
                        SetCellValue(sht, "FF ADDR HEADER", convert.ToString(_invPdfMapping.FF_Addr_Header).Trim());

                        #endregion

                        #region /* Items */
                        if (_itemCollection.Count > 0)
                        {
                            for (int i = 1; i <= _itemCollection.Count; i++)
                            {
                                SmInvoicePdfItemMapping _item = _itemCollection[i - 1];
                                if (_xls.Worksheets.Count > i)
                                {
                                    Worksheet shtItem = _xls.Worksheets[i];

                                    SetCellValue(shtItem, "ITEM HEADER", _item.ItemHeaderContent);
                                    SetCellValue(shtItem, "ITEM HEADER LINE COUNT", convert.ToInt(_item.ItemHeaderLineCount));
                                    //SetCellValue(shtItem, "ITEM INITIAL SPACE", _item.InitialItemSpace); // COMMENTED ON 01-APR-16
                                    SetCellValue(shtItem, "ITEM NO", _item.ItemNo);
                                    SetCellValue(shtItem, "ITEM QUANTITY", _item.ItemQty);
                                    SetCellValue(shtItem, "ITEM UNIT", _item.ItemUnit);
                                    SetCellValue(shtItem, "ITEM REFERENCE NO", _item.ItemRefno);
                                    SetCellValue(shtItem, "ITEM DESCREPTION", _item.ItemDescr);
                                    SetCellValue(shtItem, "ITEM REMARKS", _item.ItemRemark);
                                    SetCellValue(shtItem, "ITEM PRICE", _item.ItemUnitprice);
                                    SetCellValue(shtItem, "ITEM DISCOUNT", _item.ItemDiscount);
                                    SetCellValue(shtItem, "ITEM LEADDAYS", _item.ItemLeaddays);
                                    SetCellValue(shtItem, "ITEM TOTAL", _item.ItemTotal);
                                    SetCellValue(shtItem, "ITEM END CONTENT", _item.ItemEndContent);
                                    SetCellValue(shtItem, "LEADDAYS IN DATE", convert.ToInt(_item.LeaddaysInDate));

                                    SetCellValue(shtItem, "HAS NO EQUIP HEADER", convert.ToInt(_item.HasNoEquipHeader));
                                    SetCellValue(shtItem, "MAX EQUIP ROWS", convert.ToInt(_item.MaxEquipRows));
                                    SetCellValue(shtItem, "MAX EQUIP RANGE", convert.ToInt(_item.MaxEquipRange));

                                    string[] strEquip = Convert.ToString(_item.ItemEquipment).Split('/');
                                    SetCellValue(shtItem, "EQUIP NAME HEADER", strEquip[0]);
                                    if (strEquip.Length > 1) SetCellValue(shtItem, "EQUIP TYPE HEADER", strEquip[1]);
                                    if (strEquip.Length > 2) SetCellValue(shtItem, "EQUIP SER. NO HEADER", strEquip[2]);
                                    if (strEquip.Length > 3) SetCellValue(shtItem, "EQUIP MAKER HEADER", strEquip[3]);
                                    if (strEquip.Length > 4) SetCellValue(shtItem, "EQUIP NOTE HEADER", strEquip[4]);

                                    SetCellValue(shtItem, "ITEM GROUP HEADER", _item.ItemGroupHeader);
                                    SetCellValue(shtItem, "ITEM MIN LINE COUNT", convert.ToInt(_item.ItemMinLines));
                                    SetCellValue(shtItem, "ITEM GROUP HEADER", _item.ItemGroupHeader);
                                    SetCellValue(shtItem, "ITEM REMARKS APPEND TEXT", _item.ItemRemarksAppendText);
                                    SetCellValue(shtItem, "ITEM REMARKS INITIALS", _item.ItemRemarksInitials);
                                    SetCellValue(shtItem, "CHECK REMARKS & EQUIP BELOW ITEM", convert.ToInt(_item.CheckContentBelowItem));
                                    SetCellValue(shtItem, "EXTRA COLUMNS", _item.ExtraColumns);
                                    SetCellValue(shtItem, "EXTRA COLUMNS HEADER", _item.ExtraColumnsHeader);

                                    SetCellValue(shtItem, "READ ITEM NO UPTO MIN LINES", _item.ReadItemnoUptoMinlines);

                                    // ADDED ON 14-APRIL-2016 -- Sayak
                                    SetCellValue(shtItem, "EQUIP NAME RANGE", _item.EquipNameRange);
                                    SetCellValue(shtItem, "EQUIP TYPE RANGE", _item.EquipTypeRange);
                                    SetCellValue(shtItem, "EQUIP SER. NO. RANGE", _item.EquipSernoRange);
                                    SetCellValue(shtItem, "EQUIP MAKER RANGE", _item.EquipMakerRange);
                                    SetCellValue(shtItem, "EQUIP NOTE RANGE", _item.EquipNoteRange);
                                    SetCellValue(shtItem, "APPEND UOM", _item.AppendUom);
                                    SetCellValue(shtItem, "MAKER REF. / EXTRA NO. LINE COUNT", _item.MakerrefExtranoLineCount);
                                    SetCellValue(shtItem, "MAKER REF. RANGE", _item.MakerrefRange);
                                    SetCellValue(shtItem, "EXTRA NO. RANGE", _item.ExtranoRange);
                                    SetCellValue(shtItem, "READ PART NO FROM LAST LINE", _item.ReadPartnoFromLastLine);
                                    SetCellValue(shtItem, "ITEM CURRENCY", _item.ItemCurrency);
                                    SetCellValue(shtItem, "APPEND REF NO", _item.AppendRefNo);
                                    SetCellValue(shtItem, "IS BOLD TEXT", _item.IsBoldText);

                                    SetCellValue(shtItem, "IS_QTY_UOM_MERGED", _item.IsQtyUomMerged);
                                }
                            }
                        }
                        #endregion

                        _xls.Save(downloadPath);
                    }
                    else
                    {
                        throw new Exception("No Mapping Found!");
                    }
                }
                else { throw new Exception("No Mapping Found!"); }
            }
            catch (Exception ex)
            {
                SetLog(ex.StackTrace);
                throw ex;
            }
            return downloadPath;
        }


        #endregion

        #region /* AuditLog */
        public System.Data.DataSet GetAuditLog(int AddressID)
        {
            System.Data.DataSet ds = null;
            SmAddress _address = SmAddress.Load(AddressID);
            if (_address != null)
            {
                SmAuditlog _audit = new SmAuditlog();
                if (!_address.AddrType.ToLower().Contains("admin"))
                    ds = _audit.SM_AUDITLOG_Select_By_AddressID((int)_address.Addressid, convert.ToString(_address.AddrType).Trim());
                else ds = SmAuditlog.GetAll();
            }
            return ds;
        }

        public System.Data.DataSet GetAuditLog(int AddressID, DateTime Fromdate, DateTime ToDate)
        {
            System.Data.DataSet ds = null;
            SmAddress _address = SmAddress.Load(AddressID);
            if (_address != null)
            {
                SmAuditlog _audit = new SmAuditlog();
                if (!_address.AddrType.ToLower().Contains("admin")) ds = _audit.SM_AUDITLOG_Select_By_AddressID((int)_address.Addressid, _address.AddrType);
                else ds = SmAuditlog.GetAll(Fromdate, ToDate);
            }
            return ds;
        }

        public System.Data.DataSet GetAuditLog(int AddressID, DateTime Fromdate, DateTime ToDate, string FILTERCOND, int FROM, int TO, out int TotalRecords)
        {
            System.Data.DataSet ds = null; TotalRecords = 0;
            SmAddress _address = SmAddress.Load(AddressID);
            if (_address != null)
            {
                SmAuditlog _audit = new SmAuditlog();
                if (!_address.AddrType.ToLower().Contains("admin")) ds = _audit.SM_AUDITLOG_Select_By_AddressID((int)_address.Addressid, _address.AddrType, Fromdate, ToDate, FILTERCOND, FROM, TO, out TotalRecords);
                else ds = SmAuditlog.GetAllAuditLog(Fromdate, ToDate, FILTERCOND, FROM, TO, out TotalRecords);
            }
            return ds;
        }

        //added by Kalpita on 03/01/2018
        public System.Data.DataSet GetAuditLog_AddressID(int AddressID, DateTime Fromdate, DateTime ToDate, string FILTERCOND, int FROM, int TO, out int TotalRecords)
        {
            System.Data.DataSet ds = null; TotalRecords = 0;
            SmAddress _address = SmAddress.Load(AddressID);
            if (_address != null)
            {
                SmAuditlog _audit = new SmAuditlog();
                ds = _audit.SM_AUDITLOG_Select_By_AddressID((int)_address.Addressid, _address.AddrType, Fromdate, ToDate, FILTERCOND, FROM, TO, out TotalRecords);
            }
            return ds;
        }

        public string ExportAudit_Report(string cTemplatePath, string cSavePath, DataSet ds)
        {
            string _result = "";
            try
            {
                int rowStart = 1;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string[] existingcols = {"LOGID", "BUYER_ID", "SUPPLIER_ID"};
                    for (int k = 0; k < existingcols.Length; k++)
                    {
                        ds.Tables[0].Columns.Remove(existingcols[k]);
                    }                   
                    Workbook workbook = new Workbook(cTemplatePath);
                    Worksheet _sheet = workbook.Worksheets[0];
                    #region // Set Border Style //
                    workbook.Styles.Add();
                    Cells.Style cellStyle = workbook.Styles[0];
                    cellStyle.Borders[Cells.BorderType.LeftBorder].LineStyle = Cells.CellBorderType.Thin;
                    cellStyle.Borders[Cells.BorderType.RightBorder].LineStyle = Cells.CellBorderType.Thin;
                    cellStyle.Borders[Cells.BorderType.TopBorder].LineStyle = Cells.CellBorderType.Thin;
                    cellStyle.Borders[Cells.BorderType.BottomBorder].LineStyle = Cells.CellBorderType.Thin;

                    cellStyle.Pattern = BackgroundType.Solid;
                    cellStyle.ForegroundColor = Color.White;
                    cellStyle.TextDirection = TextDirectionType.LeftToRight;
                    cellStyle.VerticalAlignment = TextAlignmentType.Top;
                    cellStyle.IsTextWrapped = true;
                    cellStyle.Font.Name = "Verdana";
                    cellStyle.Font.Size = 8;
                    cellStyle.Name = "CellStyle";
                    #endregion

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        _sheet.Cells.InsertRow(i + rowStart);
                        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            _sheet.Cells[i + rowStart, j].Value = ds.Tables[0].Rows[i][j];
                            _sheet.Cells[i + rowStart, j].SetStyle(cellStyle);
                            cellStyle.Update();
                        }
                    }
                    _sheet.Cells.SetRowHeight(1, 15);
                    _sheet.AutoFitRows();
                    string cFileName = cSavePath + "\\AuditLog_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";
                    workbook.Save(cFileName);
                    _result = Path.GetFileName(cFileName);
                }
            }
            catch (Exception ex)
            {
                _result = "0";
            }
            return _result;
        }

        public System.Data.DataSet GetAuditLog_Filter(string cFinCond, string FROMDATE,string TODATE)
        {
            System.Data.DataSet ds = SmAuditlog.GetAllAuditLog_Filter(cFinCond, FROMDATE, TODATE);
            return ds;
        }


        public System.Data.DataSet GetAuditLog_Top5000(int AddressID, DateTime Fromdate, DateTime ToDate)
        {
            System.Data.DataSet ds = null;
            SmAddress _address = SmAddress.Load(AddressID);
            if (_address != null)
            {
                SmAuditlog _audit = new SmAuditlog();
                if (!_address.AddrType.ToLower().Contains("admin")) ds = _audit.SM_AUDITLOG_Select_By_AddressID((int)_address.Addressid, _address.AddrType);
                else ds = SmAuditlog.GetTop5000(Fromdate, ToDate);
            }
            return ds;
        }

        #endregion

        #region /* Error Log */
        public void UpdateErrorLog(int LogID, string ErrProblem, string ErrSolution, int ErrStatus)
        {
            try
            {
                SmErrorLog _errorLog = SmErrorLog.LoadByLogID(LogID);
                if (_errorLog == null) _errorLog = new SmErrorLog();

                _errorLog.ErrorProblem = ErrProblem;
                _errorLog.ErrorSolution = ErrSolution;
                _errorLog.ErrorStatus = ErrStatus;
                _errorLog.Logid = LogID;

                if (_errorLog.ErrorLogid > 0)
                {
                    _errorLog.Update();
                }
                else
                {
                    _errorLog.ErrorLogid = GetNextKey("ERROR_LOGID", "SM_ERROR_LOG", "AuditConnection");
                    _errorLog.Insert();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SendEror_mMails()
        {
            string mailFrom = convert.ToString(ConfigurationManager.AppSettings["MAIL_FROM"]);
            //string mailTo = "";
            //if (convert.ToString(_link.SupplierEmail).Trim().Length > 0) mailTo = convert.ToString(_link.SupplierEmail);
            //else mailTo = convert.ToString(ConfigurationManager.AppSettings["MAIL_TO"]);

            //if (!mailList.Contains(mailTo))
            //{
            //    string mailCC = convert.ToString(ConfigurationManager.AppSettings["CC_EMAIL"]);
            //    string mailBCC = convert.ToString(ConfigurationManager.AppSettings["BCC_EMAIL"]);
            //    string vendorCode = convert.ToString(_link.VendorAddress.AddrCode);

            //    string template = HttpContext.Current.Server.MapPath("~/ESUPPLIER_MESSAGE.txt");
            //    string mailBody = File.ReadAllText(template);
            //    mailBody = mailBody.Replace("#USERNAME#", convert.ToString(_link.VendorAddress.AddrName));
            //    mailBody = mailBody.Replace("#LOGIN_ID#", vendorCode);
            //    mailBody = mailBody.Replace("#PASSWORD#", newPassword.Trim().ToUpper());
            //    mailBody = mailBody.Replace("#MESSAGE#", "New password has been set for user " + convert.ToString(_link.VendorAddress.AddrName) + " (" + convert.ToString(_user.ExUsercode).Trim() + ").");

            //    Dal.SmSendMailQueue _sendMail = new Dal.SmSendMailQueue(_dataAccess);
            //    _sendMail.SM_SEND_MAIL_QUEUE_Insert(vendorCode, mailFrom.Trim(), mailTo.Trim(), mailCC.Trim(), mailBCC.Trim(), "New password has been set for user  " + convert.ToString(_link.VendorAddress.AddrName) + " (" + convert.ToString(_user.ExUsercode).Trim() + ").", mailBody.Trim(), DateTime.Now, ByrID, SuppID);

            //}
        }


        //public System.Data.DataSet GetErrorPriorityLog()
        //{
        //    System.Data.DataSet ds = null;
        //    SmAuditlog _audit = new SmAuditlog();
        //    ds = _audit.SM_AUDITLOG_Select_ErrorPriorityLogs();
        //    return ds;
        //}

        //public static string GetErrorDetails(string errorID, DataSet ErrorDataSet, DataSet ErrorSolutionDataSet)
        //{
        //    string _result = "";
        //    string _txtProblem = "", _txtSolution = "", _txtErrorDesc = "", _txtErrorTemplate = "", _txtErrorSolNo = "";
        //    string _auditVal = "", KeyRef = "", fileName = "";
        //    if (ErrorDataSet != null)
        //    {
        //        DataSet _dsError = ErrorDataSet;
        //        if (_dsError != null && _dsError.Tables.Count > 0 && _dsError.Tables[0].Rows.Count > 0)
        //        {
        //            DataRow[] resultRow = _dsError.Tables[0].Select("LOGID=" + errorID);
        //            if (resultRow != null && resultRow.Length > 0)
        //            {
        //                _auditVal = convert.ToString(resultRow[0]["AUDITVALUE"]);
        //                KeyRef = convert.ToString(resultRow[0]["KeyRef2"]);
        //                fileName = convert.ToString(resultRow[0]["FileName"]);
        //                _auditVal = _auditVal.Replace("\r", "").Replace("\n", "");

        //                string[] sVal = _auditVal.Split(new[] { " : " }, StringSplitOptions.None);
        //                _auditVal = _auditVal.Replace(sVal[0].ToString(), "").Trim().TrimStart(':').Trim();
        //                string[] WordsArray = _auditVal.Split(' ');
        //                string sPattern = "";
        //                if (WordsArray.Length > 4)
        //                {
        //                    sPattern = WordsArray[0] + ' ' + WordsArray[1] + ' ' + WordsArray[2] + ' ' + WordsArray[3];
        //                }
        //                else
        //                {
        //                    sPattern = _auditVal;
        //                }
        //                if (sPattern != "")
        //                {
        //                    if (ErrorSolutionDataSet != null)
        //                    {
        //                        DataSet _dsDetails = ErrorSolutionDataSet;
        //                        if (sPattern.Contains("'"))
        //                        {
        //                            sPattern = sPattern.Replace("'", "''");
        //                        }
        //                        else if (sPattern.Contains("["))
        //                        {
        //                            sPattern = sPattern.Replace("[", "**");
        //                            if (sPattern.Contains("]"))
        //                            {
        //                                sPattern = sPattern.Replace("]", "[]]");
        //                            }
        //                            sPattern = sPattern.Replace("**", "[[]");
        //                        }
        //                        else if (sPattern.Contains("]"))
        //                        {
        //                            sPattern = sPattern.Replace("]", "**");
        //                            if (sPattern.Contains("["))
        //                            {
        //                                sPattern = sPattern.Replace("[", "[[]");
        //                            }
        //                            sPattern = sPattern.Replace("**", "[]]");
        //                        }
        //                        if (_dsDetails != null && _dsDetails.Tables.Count > 0 && _dsDetails.Tables[0].Rows.Count > 0)
        //                        {
        //                            DataRow[] sResultRow = _dsDetails.Tables[0].Select("ERROR_TEMPLATE like '%" + sPattern + "%'");
        //                            if (sResultRow.Length > 0)
        //                            {
        //                                bool _resTempAudit = false;
        //                                foreach (DataRow _dr in sResultRow)
        //                                {
        //                                    bool _resTemp = false;
        //                                    string _errTemp = convert.ToString(_dr["ERROR_TEMPLATE"]);
        //                                    if (_errTemp.IndexOf("##") > 0)
        //                                    {
        //                                        List<string> lstData = new List<string>();
        //                                        Regex reg = new Regex("##(.*?)##");
        //                                        MatchCollection matches = reg.Matches(_errTemp);
        //                                        foreach (var item in matches)
        //                                        {
        //                                            string sData = _errTemp.Split(new[] { item.ToString() }, StringSplitOptions.None)[0];
        //                                            lstData.Add(sData);
        //                                            _errTemp = _errTemp.Replace(sData, "");
        //                                            _errTemp = _errTemp.Replace(item.ToString(), "");
        //                                        }
        //                                        if (_errTemp != "")
        //                                        {
        //                                            lstData.Add(_errTemp);
        //                                        }
        //                                        foreach (string pattern in lstData)
        //                                        {
        //                                            if (_auditVal.Contains(pattern))
        //                                            {
        //                                                _resTemp = true;
        //                                            }
        //                                            else
        //                                            {
        //                                                _resTemp = false;
        //                                                break;
        //                                            }
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        if (_auditVal.Contains(_errTemp))
        //                                        {
        //                                            _resTemp = true;
        //                                            //break;
        //                                        }
        //                                        else
        //                                        {
        //                                            _resTemp = false;
        //                                        }
        //                                    }
        //                                    if (_resTemp == true)
        //                                    {
        //                                        _txtErrorDesc = convert.ToString(_dr["ERROR_DESC"]);
        //                                        _txtProblem = convert.ToString(_dr["ERROR_PROBLEM"]);
        //                                        _txtSolution = convert.ToString(_dr["ERROR_SOLUTION"]);
        //                                        _txtErrorTemplate = convert.ToString(_dr["ERROR_TEMPLATE"]);
        //                                        _txtErrorSolNo = convert.ToString(_dr["ERROR_NO"]);
        //                                        _resTempAudit = true;
        //                                        break;
        //                                    }
        //                                }
        //                                if (_resTempAudit)
        //                                {
        //                                    _result = _auditVal + "|" + _txtErrorDesc + "|" + _txtProblem + "|" + _txtSolution + "|" + _txtErrorTemplate + "|" + _txtErrorSolNo;
        //                                }
        //                                else
        //                                {
        //                                    _txtErrorTemplate = Compiled_Error_Template(_auditVal, KeyRef, fileName);
        //                                    _txtErrorSolNo = Compiled_Error_SolutionNo();
        //                                    _result = _auditVal + "||||" + _txtErrorTemplate + "|" + _txtErrorSolNo;
        //                                }
        //                            }
        //                            else if (sPattern.Contains(KeyRef) || sPattern.Contains(fileName))
        //                            {
        //                                string newPattern = "";

        //                                string[] lstdata;
        //                                if (sPattern.Contains(KeyRef))
        //                                {
        //                                    lstdata = sPattern.Split(new string[] { KeyRef }, StringSplitOptions.None);
        //                                }
        //                                else
        //                                {
        //                                    lstdata = sPattern.Split(new string[] { fileName }, StringSplitOptions.None);
        //                                }
        //                                foreach (string stPatt in lstdata)
        //                                {
        //                                    if (stPatt.Split(' ').Length > 1)
        //                                    {
        //                                        newPattern = stPatt;
        //                                        break;
        //                                    }
        //                                }
        //                                if (newPattern != "")
        //                                {
        //                                    sResultRow = _dsDetails.Tables[0].Select("ERROR_TEMPLATE like '%" + newPattern + "%'");
        //                                    if (sResultRow.Length > 0)
        //                                    {
        //                                        bool _resTempAudit = false;
        //                                        foreach (DataRow _dr in sResultRow)
        //                                        {
        //                                            bool _resTemp = false;
        //                                            string _errTemp = convert.ToString(_dr["ERROR_TEMPLATE"]);
        //                                            if (_errTemp.IndexOf("##") > 0)
        //                                            {
        //                                                List<string> lstData = new List<string>();
        //                                                Regex reg = new Regex("##(.*?)##");
        //                                                MatchCollection matches = reg.Matches(_errTemp);
        //                                                foreach (var item in matches)
        //                                                {
        //                                                    string sData = _errTemp.Split(new[] { item.ToString() }, StringSplitOptions.None)[0];
        //                                                    lstData.Add(sData);
        //                                                    _errTemp = _errTemp.Replace(sData, "");
        //                                                    _errTemp = _errTemp.Replace(item.ToString(), "");
        //                                                }
        //                                                if (_errTemp != "")
        //                                                {
        //                                                    lstData.Add(_errTemp);
        //                                                }
        //                                                foreach (string pattern in lstData)
        //                                                {
        //                                                    if (_auditVal.Contains(pattern))
        //                                                    {
        //                                                        _resTemp = true;
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        _resTemp = false;
        //                                                        break;
        //                                                    }
        //                                                }
        //                                            }
        //                                            else
        //                                            {
        //                                                if (_auditVal.Contains(_errTemp))
        //                                                {
        //                                                    _resTemp = true;
        //                                                    //break;
        //                                                }
        //                                                else
        //                                                {
        //                                                    _resTemp = false;
        //                                                }
        //                                            }
        //                                            if (_resTemp == true)
        //                                            {
        //                                                _txtErrorDesc = convert.ToString(_dr["ERROR_DESC"]);
        //                                                _txtProblem = convert.ToString(_dr["ERROR_PROBLEM"]);
        //                                                _txtSolution = convert.ToString(_dr["ERROR_SOLUTION"]);
        //                                                _txtErrorTemplate = convert.ToString(_dr["ERROR_TEMPLATE"]);
        //                                                _txtErrorSolNo = convert.ToString(_dr["ERROR_NO"]);
        //                                                _resTempAudit = true;
        //                                                break;
        //                                            }
        //                                        }
        //                                        if (_resTempAudit)
        //                                        {
        //                                            _result = _auditVal + "|" + _txtErrorDesc + "|" + _txtProblem + "|" + _txtSolution + "|" + _txtErrorTemplate + "|" + _txtErrorSolNo;
        //                                        }
        //                                        else
        //                                        {
        //                                            _txtErrorTemplate = Compiled_Error_Template(_auditVal, KeyRef, fileName);
        //                                            _txtErrorSolNo = Compiled_Error_SolutionNo();
        //                                            _result = _auditVal + "||||" + _txtErrorTemplate + "|" + _txtErrorSolNo;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        _txtErrorTemplate = Compiled_Error_Template(_auditVal, KeyRef, fileName);
        //                                        _txtErrorSolNo = Compiled_Error_SolutionNo();
        //                                        _result = _auditVal + "||||" + _txtErrorTemplate + "|" + _txtErrorSolNo;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    _txtErrorTemplate = Compiled_Error_Template(_auditVal, KeyRef, fileName);
        //                                    _txtErrorSolNo = Compiled_Error_SolutionNo();
        //                                    _result = _auditVal + "||||" + _txtErrorTemplate + "|" + _txtErrorSolNo;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                _txtErrorTemplate = Compiled_Error_Template(_auditVal, KeyRef, fileName);
        //                                _txtErrorSolNo = Compiled_Error_SolutionNo();
        //                                _result = _auditVal + "||||" + _txtErrorTemplate + "|" + _txtErrorSolNo;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            _txtErrorTemplate = Compiled_Error_Template(_auditVal, KeyRef, fileName);
        //                            _txtErrorSolNo = Compiled_Error_SolutionNo();
        //                            _result = _auditVal + "||||" + _txtErrorTemplate + "|" + _txtErrorSolNo;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        _txtErrorTemplate = Compiled_Error_Template(_auditVal, KeyRef, fileName);
        //                        _txtErrorSolNo = Compiled_Error_SolutionNo();
        //                        _result = _auditVal + "||||" + _txtErrorTemplate + "|" + _txtErrorSolNo;
        //                    }
        //                }
        //                else
        //                {
        //                    _txtErrorTemplate = Compiled_Error_Template(_auditVal, KeyRef, fileName);
        //                    _txtErrorSolNo = Compiled_Error_SolutionNo();
        //                    _result = _auditVal + "||||" + _txtErrorTemplate + "|" + _txtErrorSolNo;
        //                }
        //            }
        //            else
        //            {
        //                _txtErrorTemplate = Compiled_Error_Template(_auditVal, KeyRef, fileName);
        //                _txtErrorSolNo = Compiled_Error_SolutionNo();
        //                _result = _auditVal + "||||" + _txtErrorTemplate + "|" + _txtErrorSolNo;
        //            }
        //        }
        //    }
        //    return _result;
        //}

        public static string GetErrorDetails(string errorID, DataSet ErrorDataSet, DataSet ErrorSolutionDataSet)
        {
            string _result = "";
            string _txtProblem = "", _txtSolution = "", _txtErrorDesc = "", _txtErrorTemplate = "", _txtErrorSolNo = "";
            string _auditVal = "", KeyRef = "", fileName = "";
            if (ErrorDataSet != null)
            {
                DataSet _dsError = ErrorDataSet;
                if (_dsError != null && _dsError.Tables.Count > 0 && _dsError.Tables[0].Rows.Count > 0)
                {
                    DataRow[] resultRow = _dsError.Tables[0].Select("LOGID=" + errorID);
                    if (resultRow != null && resultRow.Length > 0)
                    {
                        _auditVal = convert.ToString(resultRow[0]["AUDITVALUE"]);
                        KeyRef = convert.ToString(resultRow[0]["KeyRef2"]);
                        fileName = convert.ToString(resultRow[0]["FileName"]);
                        _auditVal = _auditVal.Replace("\r", "").Replace("\n", "");

                        string[] sVal = _auditVal.Split(new[] { " : " }, StringSplitOptions.None);
                        _auditVal = _auditVal.Replace(sVal[0].ToString(), "").Trim().TrimStart(':').Trim();
                        string[] WordsArray = _auditVal.Split(' ');
                        string sPattern = "";
                        if (WordsArray.Length > 4)
                        {
                            sPattern = WordsArray[0] + ' ' + WordsArray[1] + ' ' + WordsArray[2] + ' ' + WordsArray[3];
                        }
                        else
                        {
                            sPattern = _auditVal;
                        }
                        if (sPattern != "")
                        {
                            if (ErrorSolutionDataSet != null)
                            {
                                DataSet _dsDetails = ErrorSolutionDataSet;
                                if (sPattern.Contains("'"))
                                {
                                    sPattern = sPattern.Replace("'", "''");
                                }
                                else if (sPattern.Contains("["))
                                {
                                    sPattern = sPattern.Replace("[", "**");
                                    if (sPattern.Contains("]"))
                                    {
                                        sPattern = sPattern.Replace("]", "[]]");
                                    }
                                    sPattern = sPattern.Replace("**", "[[]");
                                }
                                else if (sPattern.Contains("]"))
                                {
                                    sPattern = sPattern.Replace("]", "**");
                                    if (sPattern.Contains("["))
                                    {
                                        sPattern = sPattern.Replace("[", "[[]");
                                    }
                                    sPattern = sPattern.Replace("**", "[]]");
                                }
                                if (_dsDetails != null && _dsDetails.Tables.Count > 0 && _dsDetails.Tables[0].Rows.Count > 0)
                                {
                                    DataRow[] sResultRow = _dsDetails.Tables[0].Select("ERROR_TEMPLATE like '%" + sPattern + "%'");
                                    if (sResultRow.Length > 0)
                                    {
                                        bool _resTempAudit = false;
                                        foreach (DataRow _dr in sResultRow)
                                        {
                                            bool _resTemp = false;
                                            string _errTemp = convert.ToString(_dr["ERROR_TEMPLATE"]);
                                            if (_errTemp.IndexOf("##") > 0)
                                            {
                                                List<string> lstData = new List<string>();
                                                Regex reg = new Regex("##(.*?)##");
                                                MatchCollection matches = reg.Matches(_errTemp);
                                                foreach (var item in matches)
                                                {
                                                    string sData = _errTemp.Split(new[] { item.ToString() }, StringSplitOptions.None)[0];
                                                    lstData.Add(sData);
                                                    _errTemp = _errTemp.Replace(sData, "");
                                                    _errTemp = _errTemp.Replace(item.ToString(), "");
                                                }
                                                if (_errTemp != "")
                                                {
                                                    lstData.Add(_errTemp);
                                                }
                                                foreach (string pattern in lstData)
                                                {
                                                    if (_auditVal.Contains(pattern))
                                                    {
                                                        _resTemp = true;
                                                    }
                                                    else
                                                    {
                                                        _resTemp = false;
                                                        break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (_auditVal.Contains(_errTemp))
                                                {
                                                    _resTemp = true;
                                                    //break;
                                                }
                                                else
                                                {
                                                    _resTemp = false;
                                                }
                                            }
                                            if (_resTemp == true)
                                            {
                                                _txtErrorDesc = convert.ToString(_dr["ERROR_DESC"]);
                                                _txtProblem = convert.ToString(_dr["ERROR_PROBLEM"]);
                                                _txtSolution = convert.ToString(_dr["ERROR_SOLUTION"]);
                                                _txtErrorTemplate = convert.ToString(_dr["ERROR_TEMPLATE"]);
                                                _txtErrorSolNo = convert.ToString(_dr["ERROR_NO"]);
                                                _resTempAudit = true;
                                                break;
                                            }
                                        }
                                        if (_resTempAudit)
                                        {
                                            _result = _auditVal + "|" + _txtErrorDesc + "|" + _txtProblem + "|" + _txtSolution + "|" + _txtErrorTemplate + "|" + _txtErrorSolNo;
                                        }
                                        else
                                        {
                                            _txtErrorTemplate = Compiled_Error_Template(_auditVal, KeyRef, fileName);
                                            _txtErrorSolNo = Compiled_Error_SolutionNo();
                                            _result = _auditVal + "||||" + _txtErrorTemplate + "|" + _txtErrorSolNo;
                                        }
                                    }
                                    else if (sPattern.Contains(KeyRef) || sPattern.Contains(fileName))
                                    {
                                        string newPattern = "";

                                        string[] lstdata;
                                        if (sPattern.Contains(KeyRef))
                                        {
                                            lstdata = sPattern.Split(new string[] { KeyRef }, StringSplitOptions.None);
                                        }
                                        else
                                        {
                                            lstdata = sPattern.Split(new string[] { fileName }, StringSplitOptions.None);
                                        }
                                        foreach (string stPatt in lstdata)
                                        {
                                            if (stPatt.Split(' ').Length > 1)
                                            {
                                                newPattern = stPatt;
                                                break;
                                            }
                                        }
                                        if (newPattern != "")
                                        {
                                            sResultRow = _dsDetails.Tables[0].Select("ERROR_TEMPLATE like '%" + newPattern + "%'");
                                            if (sResultRow.Length > 0)
                                            {
                                                bool _resTempAudit = false;
                                                foreach (DataRow _dr in sResultRow)
                                                {
                                                    bool _resTemp = false;
                                                    string _errTemp = convert.ToString(_dr["ERROR_TEMPLATE"]);
                                                    if (_errTemp.IndexOf("##") > 0)
                                                    {
                                                        List<string> lstData = new List<string>();
                                                        Regex reg = new Regex("##(.*?)##");
                                                        MatchCollection matches = reg.Matches(_errTemp);
                                                        foreach (var item in matches)
                                                        {
                                                            string sData = _errTemp.Split(new[] { item.ToString() }, StringSplitOptions.None)[0];
                                                            lstData.Add(sData);
                                                            _errTemp = _errTemp.Replace(sData, "");
                                                            _errTemp = _errTemp.Replace(item.ToString(), "");
                                                        }
                                                        if (_errTemp != "")
                                                        {
                                                            lstData.Add(_errTemp);
                                                        }
                                                        foreach (string pattern in lstData)
                                                        {
                                                            if (_auditVal.Contains(pattern))
                                                            {
                                                                _resTemp = true;
                                                            }
                                                            else
                                                            {
                                                                _resTemp = false;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (_auditVal.Contains(_errTemp))
                                                        {
                                                            _resTemp = true;
                                                            //break;
                                                        }
                                                        else
                                                        {
                                                            _resTemp = false;
                                                        }
                                                    }
                                                    if (_resTemp == true)
                                                    {
                                                        _txtErrorDesc = convert.ToString(_dr["ERROR_DESC"]);
                                                        _txtProblem = convert.ToString(_dr["ERROR_PROBLEM"]);
                                                        _txtSolution = convert.ToString(_dr["ERROR_SOLUTION"]);
                                                        _txtErrorTemplate = convert.ToString(_dr["ERROR_TEMPLATE"]);
                                                        _txtErrorSolNo = convert.ToString(_dr["ERROR_NO"]);
                                                        _resTempAudit = true;
                                                        break;
                                                    }
                                                }
                                                if (_resTempAudit)
                                                {
                                                    _result = _auditVal + "|" + _txtErrorDesc + "|" + _txtProblem + "|" + _txtSolution + "|" + _txtErrorTemplate + "|" + _txtErrorSolNo;
                                                }
                                                else
                                                {
                                                    _txtErrorTemplate = Compiled_Error_Template(_auditVal, KeyRef, fileName);
                                                    _txtErrorSolNo = Compiled_Error_SolutionNo();
                                                    _result = _auditVal + "||||" + _txtErrorTemplate + "|" + _txtErrorSolNo;
                                                }
                                            }
                                            else
                                            {
                                                _txtErrorTemplate = Compiled_Error_Template(_auditVal, KeyRef, fileName);
                                                _txtErrorSolNo = Compiled_Error_SolutionNo();
                                                _result = _auditVal + "||||" + _txtErrorTemplate + "|" + _txtErrorSolNo;
                                            }
                                        }
                                        else
                                        {
                                            _txtErrorTemplate = Compiled_Error_Template(_auditVal, KeyRef, fileName);
                                            _txtErrorSolNo = Compiled_Error_SolutionNo();
                                            _result = _auditVal + "||||" + _txtErrorTemplate + "|" + _txtErrorSolNo;
                                        }
                                    }
                                    else
                                    {
                                        _txtErrorTemplate = Compiled_Error_Template(_auditVal, KeyRef, fileName);
                                        _txtErrorSolNo = Compiled_Error_SolutionNo();
                                        _result = _auditVal + "||||" + _txtErrorTemplate + "|" + _txtErrorSolNo;
                                    }
                                }
                                else
                                {
                                    _txtErrorTemplate = Compiled_Error_Template(_auditVal, KeyRef, fileName);
                                    _txtErrorSolNo = Compiled_Error_SolutionNo();
                                    _result = _auditVal + "||||" + _txtErrorTemplate + "|" + _txtErrorSolNo;
                                }
                            }
                            else
                            {
                                _txtErrorTemplate = Compiled_Error_Template(_auditVal, KeyRef, fileName);
                                _txtErrorSolNo = Compiled_Error_SolutionNo();
                                _result = _auditVal + "||||" + _txtErrorTemplate + "|" + _txtErrorSolNo;
                            }
                        }
                        else
                        {
                            _txtErrorTemplate = Compiled_Error_Template(_auditVal, KeyRef, fileName);
                            _txtErrorSolNo = Compiled_Error_SolutionNo();
                            _result = _auditVal + "||||" + _txtErrorTemplate + "|" + _txtErrorSolNo;
                        }
                    }
                    else
                    {
                        _txtErrorTemplate = Compiled_Error_Template(_auditVal, KeyRef, fileName);
                        _txtErrorSolNo = Compiled_Error_SolutionNo();
                        _result = _auditVal + "||||" + _txtErrorTemplate + "|" + _txtErrorSolNo;
                    }
                }
            }
            return _result;
        }

        public static string Compiled_Error_Template(string sAudiVal, string sKeyRef, string sFileName)
        {
            string _errortemp = "";
            try
            {
                if (sAudiVal.Contains(sKeyRef) && sKeyRef != "")
                {
                    sAudiVal = sAudiVal.Replace(sKeyRef, "##REF##");
                }
                if (sAudiVal.Contains(sFileName) && sFileName != "")
                {
                    sAudiVal = sAudiVal.Replace(sFileName, "##FILENAME##");
                }
                _errortemp = sAudiVal;
            }
            catch (Exception ex)
            {
                SupplierRoutines _Routine = new SupplierRoutines();
                _Routine.SetLog(ex.StackTrace);
            }
            return _errortemp;
        }

        public static string Compiled_Error_SolutionNo()
        {
            string _errorSolNo = "";
            try
            {
                int counter = 0;
                DataSet _dsDetails = (DataSet)HttpContext.Current.Session["ErrorSolutionDataSet"];
                if (_dsDetails != null && _dsDetails.Tables.Count > 0 && _dsDetails.Tables[0].Rows.Count > 0)
                {
                    counter = _dsDetails.Tables[0].Rows.Count;
                }
                counter++;
                _errorSolNo = "ERR-" + (1000 + counter);
            }
            catch (Exception ex)
            {
                SupplierRoutines _Routine = new SupplierRoutines();
                _Routine.SetLog(ex.StackTrace);
            }
            return _errorSolNo;
        }

        public System.Data.DataSet GetError_SolutionDetails()
        {
            SmErrorDetail _err = new SmErrorDetail();
            return _err.Get_Error_Details();
        }

        public System.Data.DataSet GetErrorSoln_Details(string LOGID)
        {
            SmAuditlog _err = new SmAuditlog();
            return _err.GetError_Details(Convert.ToInt32(LOGID));
        }

        public string SaveError_SolutionDetails(string ErrorSolNo, string ErrorDesc, string ErrorProblem, string ErrorSoln, string ErrorTemp, string UserHostAddress)
        {
            string _result = ""; string cInsUpdt="";
            string AuditValue = "";
            try
            {
                SmErrorDetail _errDetails = SmErrorDetail.LoadByErrSolNo(ErrorSolNo);
                if (_errDetails != null) { cInsUpdt = " updated"; }
                else { _errDetails = new SmErrorDetail(); cInsUpdt = " inserted"; _errDetails.CreatedDate = DateTime.Now; }

                _errDetails.ErrorNo = convert.ToString(ErrorSolNo).Trim();
                _errDetails.ErrorDesc = convert.ToString(ErrorDesc).Trim();
                _errDetails.ErrorProblem = convert.ToString(ErrorProblem).Trim();
                _errDetails.ErrorSolution = convert.ToString(ErrorSoln).Trim();
                _errDetails.ErrorTemplate = convert.ToString(ErrorTemp).Trim();
                _errDetails.UpdatedDate = DateTime.Now;

                if (_errDetails != null) { _errDetails.Insert(); } else { _errDetails.Update(); }
                AuditValue = "Error Solution No : " + ErrorSolNo + cInsUpdt + " successfully updated by [" + UserHostAddress + "]";
                SetAuditLog("LeSMonitor", AuditValue, "Updated", "", "", "", "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _result;
        }

        public string ExportErrors_Report(string cTemplatePath, string cSavePath, DataSet ds)
        {
            string _result = "";
            try
            {
                int rowStart = 1;               
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string[] existingcols = { "UPDATEDATE", "ERROR_STATUS", "PRIORITY_FLAG", "ERROR_LOGID", "ERR_PROBLEM", "ERR_SOLUTION" };
                    for (int k = 0; k < existingcols.Length; k++)
                    {
                        ds.Tables[0].Columns.Remove(existingcols[k]);
                    }
                    ds.Tables[0].Columns["UPDATE_DATE"].SetOrdinal(0);
                    Workbook workbook = new Workbook(cTemplatePath);
                    Worksheet _sheet = workbook.Worksheets[0];
                    #region // Set Border Style //
                    workbook.Styles.Add();
                    Cells.Style cellStyle = workbook.Styles[0];
                    cellStyle.Borders[Cells.BorderType.LeftBorder].LineStyle = Cells.CellBorderType.Thin;
                    cellStyle.Borders[Cells.BorderType.RightBorder].LineStyle = Cells.CellBorderType.Thin;
                    cellStyle.Borders[Cells.BorderType.TopBorder].LineStyle = Cells.CellBorderType.Thin;
                    cellStyle.Borders[Cells.BorderType.BottomBorder].LineStyle = Cells.CellBorderType.Thin;
                
                    cellStyle.Pattern = BackgroundType.Solid;
                    cellStyle.ForegroundColor = Color.White;
                    cellStyle.TextDirection = TextDirectionType.LeftToRight;
                    cellStyle.VerticalAlignment = TextAlignmentType.Top;
                    cellStyle.IsTextWrapped = true;
                    cellStyle.Font.Name = "Verdana";
                    cellStyle.Font.Size = 8;
                    cellStyle.Name = "CellStyle";
                    #endregion

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        _sheet.Cells.InsertRow(i + rowStart);
                        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            _sheet.Cells[i + rowStart, j].Value = ds.Tables[0].Rows[i][j];
                            _sheet.Cells[i + rowStart, j].SetStyle(cellStyle);
                            cellStyle.Update();
                        }
                    }
                    _sheet.Cells.SetRowHeight(1, 15);
                    _sheet.AutoFitRows();
                    string cFileName = cSavePath + "\\ErrorLog_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";
                    workbook.Save(cFileName);
                    _result = Path.GetFileName(cFileName);
                }
            }
            catch (Exception ex)
            {
                _result = "0";
            }
            return _result;
        }
    
        #endregion

        #region/* User Login */
        public int AddNewLogin(int ByrSuppLinkID, string UserHostServer)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            int Userid = -1;
            try
            {
                _dataAccess.CreateConnection();

                SmBuyerSupplierLink _link = SmBuyerSupplierLink.Load(ByrSuppLinkID);
                if (_link != null)
                {
                    string cPwd = "";
                    string _suppEmail = convert.ToString(_link.SupplierEmail).Trim();
                    int ByrID = convert.ToInt(_link.BuyerAddress.Addressid);
                    int SuppID = convert.ToInt(_link.VendorAddress.Addressid);

                    if (_suppEmail.Length > 0)
                    {
                        _suppEmail = _suppEmail.Substring(_suppEmail.IndexOf('@'));

                        DataSet ds = GetLoginPWDCount((int)_link.VendorAddress.Addressid, _suppEmail.Trim()); // Email passed to this function is not used in query
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            cPwd = GlobalTools.DecodePassword(convert.ToString(ds.Tables[0].Rows[0][0]));
                        }
                        else
                        {
                            cPwd = GetRandamString(convert.ToLong(DateTime.Now.ToString("fffyyddHHmm")) + convert.ToLong(DateTime.Now.ToString("yyddHHmmfff")));
                        }

                        #region // Commented on 15-APR-15 //
                        /* 
                        if (createNewPwd)
                        {
                            cPwd = GetRandamString(convert.ToLong(DateTime.Now.ToString("fffyyddHHmm")) + convert.ToLong(DateTime.Now.ToString("yyddHHmmfff")));
                        }
                        else
                        {
                            DataSet ds = GetLoginPWDCount((int)_link.VendorAddress.Addressid, convert.ToString(_link.SupplierEmail).Trim());
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                cPwd = GlobalTools.DecodePassword(convert.ToString(ds.Tables[0].Rows[0][0]));
                            }
                            else cPwd = GetRandamString(convert.ToLong(DateTime.Now.ToString("fffyyddHHmm")) + convert.ToLong(DateTime.Now.ToString("yyddHHmmfff")));
                        }
                        */
                        #endregion

                        _dataAccess.BeginTransaction();

                        SmExternalUsers _user = new SmExternalUsers();
                        _user.Addressid = _link.VendorAddress.Addressid;
                        _user.ExUsername = convert.ToString(_link.VendorAddress.AddrName).Trim();
                        _user.ExUsercode = convert.ToString(_link.VendorAddress.AddrCode).Trim().ToUpper();
                        _user.ExEmailid = convert.ToString(_link.SupplierEmail).Trim();
                        _user.ExPassword = GlobalTools.EncodePassword(cPwd.Trim()).Trim();
                        _user.Isactive = 0;
                        _user.LinkID = _link.Linkid;
                        _user.CreatedDate = DateTime.Now;
                        _user.UpdateDate = DateTime.Now;
                        _user.Insert(_dataAccess);
                        Userid = convert.ToInt(_user.ExUserid);

                        SetAuditLog("LeSMonitor", "New Login ID created for Buyer-Supplier (" + convert.ToString(_link.BuyerAddress.AddrCode).Trim().ToUpper() + "-" + convert.ToString(_link.VendorAddress.AddrCode).Trim().ToUpper() + ") by [" + UserHostServer + "].", "Updated", "", "", ByrID.ToString(), SuppID.ToString(), _dataAccess);

                        string mailFrom = convert.ToString(ConfigurationManager.AppSettings["MAIL_FROM"]);
                        string mailTo = "";
                        if (convert.ToString(_link.SupplierEmail).Trim().Length > 0) mailTo = convert.ToString(_link.SupplierEmail);
                        else mailTo = convert.ToString(ConfigurationManager.AppSettings["MAIL_TO"]);

                        string mailCC = convert.ToString(ConfigurationManager.AppSettings["CC_EMAIL"]);
                        string mailBCC = convert.ToString(ConfigurationManager.AppSettings["BCC_EMAIL"]);
                        string vendorCode = convert.ToString(_link.VendorAddress.AddrCode);

                        string template = HttpContext.Current.Server.MapPath("~/ESUPPLIER_MESSAGE.txt");
                        string mailBody = File.ReadAllText(template);
                        mailBody = mailBody.Replace("#USERNAME#", (convert.ToString(_link.VendorAddress.AddrName)));
                        mailBody = mailBody.Replace("#LOGIN_ID#", vendorCode);
                        mailBody = mailBody.Replace("#PASSWORD#", GlobalTools.DecodePassword(_user.ExPassword).Trim());
                        mailBody = mailBody.Replace("#MESSAGE#", "Supplier " + (convert.ToString(_link.VendorAddress.AddrName)) + " (" + vendorCode + ") has been subscribed to LES eSupplierLite.");

                        Dal.SmSendMailQueue _sendMail = new Dal.SmSendMailQueue(_dataAccess);
                        _sendMail.SM_SEND_MAIL_QUEUE_Insert(vendorCode, mailFrom.Trim(), mailTo.Trim(), mailCC.Trim(), mailBCC.Trim(), "Supplier " + (convert.ToString(_link.VendorAddress.AddrName)) + " (" + vendorCode + ") has been subscribed to LES eSupplierLite", mailBody.Trim(), DateTime.Now, ByrID, SuppID);

                        _dataAccess.CommitTransaction();
                    }
                }
                else return -1;
            }
            catch (Exception ex)
            {
                try { _dataAccess.RollbackTransaction(); }
                catch { }
                SetAuditLog("LeSMonitor", "Unable to create new login ID for Buyer-Supplier. Error - " + ex.Message, "Error", "", "", "", "");
            }
            finally
            {
                _dataAccess._Dispose();
            }
            return Userid;
        }

        public DataSet GetLoginPWDCount(int Addressid, string suppEmail)
        {
            DataSet ds = SmExternalUsers.GetPwdByAddressid(Addressid, suppEmail.Trim());
            return ds;
        }

        public List<string> GetUserLoginInfo(int ByrSuppLinkID)
        {
            List<string> lst = new List<string>();

            SmBuyerSupplierLink _link = SmBuyerSupplierLink.Load(ByrSuppLinkID);
            string suppEmail = _link.SupplierEmail;

            SmExternalUsers _user = SmExternalUsers.LoadByLinkID(_link.Linkid);
            if (_user != null) // && convert.ToString(suppEmail).Trim().Length > 0
            {
                if (convert.ToString(suppEmail).Trim().Length > 0)
                {
                    _user.ExEmailid = convert.ToString(suppEmail).Trim();
                    _user.UpdateDate = DateTime.Now;
                    _user.Update();
                }

                lst.Add(convert.ToString(_user.ExUserid)); //0
                lst.Add(convert.ToString(_user.ExUsername).Trim()); //1 
                lst.Add(convert.ToString(_user.ExUsercode).Trim());// 2
                lst.Add(convert.ToString(GlobalTools.DecodePassword(convert.ToString(_user.ExPassword).Trim())));//3 
                lst.Add(convert.ToString(_user.ExEmailid).Trim());//4
                lst.Add(convert.ToString(_user.Isactive.Value)); //5
            }
            return lst;
        }

        public bool Do_User_Exist_By_Link(int ByrSuppLinkID)
        {
            SmExternalUsers _user = SmExternalUsers.LoadByLinkID(ByrSuppLinkID);
            if (_user != null)
            {
                return true;
            }
            else return false;
        }

        public void SetActive(int userid, bool isActive)
        {
            try
            {
                SmExternalUsers _user = SmExternalUsers.Load(userid);
                if (isActive) _user.Isactive = 1;
                else _user.Isactive = 0;
                _user.UpdateDate = DateTime.Now;
                _user.Update();
            }
            catch { }
        }

        public List<string> GetUserDetails(int USERID)
        {
            List<string> lstDetails = new List<string>();
            try
            {
                SmExternalUsers _user = SmExternalUsers.Load(USERID);
                if (_user != null)
                {
                    lstDetails.Add(Convert.ToString(_user.ExUserid));
                    lstDetails.Add(Convert.ToString(_user.Addressid));
                    lstDetails.Add(Convert.ToString(_user.ExUsername));
                    lstDetails.Add(Convert.ToString(_user.ExUsercode));

                    // Set AddrType
                    SmAddress _address = SmAddress.Load(_user.Addressid);
                    if (_address != null) lstDetails.Add(Convert.ToString(_address.AddrType));
                    else lstDetails.Add("");
                    lstDetails.Add(Convert.ToString(GlobalTools.DecodePassword(_user.ExPassword)));
                }
            }
            catch (Exception ex)
            {
            }
            return lstDetails;
        }

        #region old code
        //public string UpdatePassword(string newPassword, int USERID, string UserHostServer)
        //{
        //    Dal.DataAccess _dataAccess = new Dal.DataAccess();
        //    try
        //    {
        //        SmExternalUsers _user = SmExternalUsers.Load(USERID);
        //        List<string> mailList = new List<string>();
        //        if (_user != null)
        //        {
        //            _dataAccess.CreateConnection();
        //            SmBuyerSupplierLink _link = SmBuyerSupplierLink.Load(_user.LinkID);
        //            int ByrID = convert.ToInt(_link.BuyerAddress.Addressid);
        //            int SuppID = convert.ToInt(_link.VendorAddress.Addressid);

        //            //_user.ExPassword = GlobalTools.EncodePassword(newPassword.Trim());
        //            //_user.UpdateDate = DateTime.Now;
        //            //_user.Update(_dataAccess);

        //            //_dataAccess.CreateSQLCommand("UPDATE SM_EXTERNAL_USERS SET UPDATE_DATE=GETDATE(), EX_PASSWORD='" + GlobalTools.EncodePassword(newPassword.Trim()) + "' WHERE EX_EMAILID LIKE '%" + convert.ToString(_user.ExEmailid) + "' AND LTRIM(RTRIM(EX_USERCODE))='" + convert.ToString(_user.ExUsercode).Trim() + "' ");
        //            //int affectedRows = _dataAccess.ExecuteNonQuery();

        //            string _suppDomain = convert.ToString(_user.ExEmailid).Trim().Substring(convert.ToString(_user.ExEmailid).Trim().IndexOf('@'));
        //            DataSet dtUsers = new DataSet();
        //            //_dataAccess.CreateSQLCommand("SELECT * FROM SM_EXTERNAL_USERS WHERE EX_EMAILID LIKE '%" + _suppDomain + "%' AND LTRIM(RTRIM(EX_USERCODE))='" + convert.ToString(_user.ExUsercode).Trim() + "'");

        //            _dataAccess.CreateProcedureCommand("Select_SM_EXTERNAL_USERS_By_ESUPPLIER_LITE");
        //            _dataAccess.AddParameter("ADDRESSID", _user.Addressid, ParameterDirection.Input);
        //            dtUsers = _dataAccess.ExecuteDataSet();

        //            if (dtUsers != null && dtUsers.Tables[0].Rows.Count > 0)
        //            {
        //                _dataAccess.BeginTransaction();
        //                bool isPwdReset = false;

        //                foreach (DataRow dr in dtUsers.Tables[0].Rows)
        //                {
        //                    _dataAccess.CreateSQLCommand("UPDATE SM_EXTERNAL_USERS SET UPDATE_DATE=GETDATE(), EX_PASSWORD= @EX_PASSWORD  WHERE EX_USERID=@EX_USERID");
        //                    _dataAccess.AddParameter("EX_PASSWORD", GlobalTools.EncodePassword(newPassword.Trim()),ParameterDirection.Input);
        //                    _dataAccess.AddParameter("EX_USERID", convert.ToInt(dr["EX_USERID"]) ,ParameterDirection.Input);

        //                    int affectedRows = _dataAccess.ExecuteNonQuery();
        //                    if (affectedRows == 1)
        //                    {
        //                        isPwdReset = true;
        //                        string mailFrom = convert.ToString(ConfigurationManager.AppSettings["MAIL_FROM"]);
        //                        //string mailTo = convert.ToString(ConfigurationManager.AppSettings["MAIL_TO"]);

        //                        string mailTo = "";
        //                        if (convert.ToString(_link.SupplierEmail).Trim().Length > 0) mailTo = convert.ToString(_link.SupplierEmail);
        //                        else mailTo = convert.ToString(ConfigurationManager.AppSettings["MAIL_TO"]);

        //                        if (!mailList.Contains(mailTo))
        //                        {
        //                            string mailCC = convert.ToString(ConfigurationManager.AppSettings["CC_EMAIL"]);
        //                            string mailBCC = convert.ToString(ConfigurationManager.AppSettings["BCC_EMAIL"]);
        //                            string vendorCode = convert.ToString(_link.VendorAddress.AddrCode);

        //                            string template = HttpContext.Current.Server.MapPath("~/ESUPPLIER_MESSAGE.txt");
        //                            string mailBody = File.ReadAllText(template);
        //                            mailBody = mailBody.Replace("#USERNAME#", convert.ToString(_link.VendorAddress.AddrName));
        //                            mailBody = mailBody.Replace("#LOGIN_ID#", vendorCode);

        //                            /* Updated on 16-JUN-2015 */
        //                            mailBody = mailBody.Replace("#PASSWORD#", newPassword.Trim().ToUpper());
        //                            //************************//
        //                            mailBody = mailBody.Replace("#MESSAGE#", "New password has been set for user " + convert.ToString(_link.VendorAddress.AddrName) + " (" + convert.ToString(_user.ExUsercode).Trim() + ").");

        //                            Dal.SmSendMailQueue _sendMail = new Dal.SmSendMailQueue(_dataAccess);
        //                            _sendMail.SM_SEND_MAIL_QUEUE_Insert(vendorCode, mailFrom.Trim(), mailTo.Trim(), mailCC.Trim(), mailBCC.Trim(), "New password has been set for user  " + convert.ToString(_link.VendorAddress.AddrName) + " (" + convert.ToString(_user.ExUsercode).Trim() + ").", mailBody.Trim(), DateTime.Now, ByrID, SuppID);

        //                            mailList.Add(mailTo);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        isPwdReset = false;
        //                        break;
        //                    }
        //                }

        //                if (isPwdReset)
        //                {
        //                    SetAuditLog("LeSMonitor", "New password has been set for Buyer-Supplier (" + _link.BuyerAddress.AddrCode + "-" + _link.VendorAddress.AddrCode + ") by [" + UserHostServer + "].", "Updated", "", "", ByrID.ToString(), SuppID.ToString(), _dataAccess);
        //                    _dataAccess.CommitTransaction();
        //                    return convert.ToString(newPassword.Trim());
        //                }
        //                else
        //                {
        //                    SetAuditLog("LeSMonitor", "Unable to set new password has been set for Buyer-Supplier (" + _link.BuyerAddress.AddrCode + "-" + _link.VendorAddress.AddrCode + ")", "Error", "", "", ByrID.ToString(), SuppID.ToString(), _dataAccess);
        //                    _dataAccess.RollbackTransaction();
        //                    return "";
        //                }
        //            }
        //            else return "";
        //        }
        //        else return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        _dataAccess.RollbackTransaction();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        _dataAccess._Dispose();
        //    }
        //}
        #endregion

        //changed by kalpita on 27/11/2017
        public string UpdatePassword(string newPassword, int USERID, string UserHostServer)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                SmExternalUsers _user = SmExternalUsers.Load(USERID);
                List<string> mailList = new List<string>();
                if (_user != null)
                {
                    _dataAccess.CreateConnection();
                    SmBuyerSupplierLink _link = SmBuyerSupplierLink.Load(_user.LinkID);
                    int ByrID = convert.ToInt(_link.BuyerAddress.Addressid);
                    int SuppID = convert.ToInt(_link.VendorAddress.Addressid);
                    string _suppDomain = convert.ToString(_user.ExEmailid).Trim().Substring(convert.ToString(_user.ExEmailid).Trim().IndexOf('@'));
                    DataSet dtUsers = new DataSet();
                    _dataAccess.CreateProcedureCommand("Select_SM_EXTERNAL_USERS_By_ESUPPLIER_LITE");
                    _dataAccess.AddParameter("ADDRESSID", _user.Addressid, ParameterDirection.Input);
                    dtUsers = _dataAccess.ExecuteDataSet();

                    if (dtUsers != null && dtUsers.Tables[0].Rows.Count > 0)
                    {
                        _dataAccess.BeginTransaction();
                        bool isPwdReset = false;

                        foreach (DataRow dr in dtUsers.Tables[0].Rows)
                        {
                            _dataAccess.CreateSQLCommand("UPDATE SM_EXTERNAL_USERS SET UPDATE_DATE=GETDATE(), EX_PASSWORD= @EX_PASSWORD  WHERE EX_USERID=@EX_USERID");
                            _dataAccess.AddParameter("EX_PASSWORD", GlobalTools.EncodePassword(newPassword.Trim()), ParameterDirection.Input);
                            _dataAccess.AddParameter("EX_USERID", convert.ToInt(dr["EX_USERID"]), ParameterDirection.Input);

                            int affectedRows = _dataAccess.ExecuteNonQuery();
                            if (affectedRows == 1)
                            {
                                isPwdReset = true;
                                string mailFrom = convert.ToString(ConfigurationManager.AppSettings["MAIL_FROM"]);
                                string mailTo = "";
                                if (convert.ToString(_link.SupplierEmail).Trim().Length > 0) mailTo = convert.ToString(_link.SupplierEmail);
                                else mailTo = convert.ToString(ConfigurationManager.AppSettings["MAIL_TO"]);

                                if (!mailList.Contains(mailTo))
                                {
                                    string mailCC = convert.ToString(ConfigurationManager.AppSettings["CC_EMAIL"]);
                                    string mailBCC = convert.ToString(ConfigurationManager.AppSettings["BCC_EMAIL"]);
                                    string vendorCode = convert.ToString(_link.VendorAddress.AddrCode);

                                    string template = HttpContext.Current.Server.MapPath("~/ESUPPLIER_MESSAGE.txt");
                                    string mailBody = File.ReadAllText(template);
                                    mailBody = mailBody.Replace("#USERNAME#", convert.ToString(_link.VendorAddress.AddrName));
                                    mailBody = mailBody.Replace("#LOGIN_ID#", vendorCode);
                                    mailBody = mailBody.Replace("#PASSWORD#", newPassword.Trim().ToUpper());
                                    mailBody = mailBody.Replace("#MESSAGE#", "New password has been set for user " + convert.ToString(_link.VendorAddress.AddrName) + " (" + convert.ToString(_user.ExUsercode).Trim() + ").");

                                    Dal.SmSendMailQueue _sendMail = new Dal.SmSendMailQueue(_dataAccess);
                                    _sendMail.SM_SEND_MAIL_QUEUE_Insert(vendorCode, mailFrom.Trim(), mailTo.Trim(), mailCC.Trim(), mailBCC.Trim(), "New password has been set for user  " + convert.ToString(_link.VendorAddress.AddrName) + " (" + convert.ToString(_user.ExUsercode).Trim() + ").", mailBody.Trim(), DateTime.Now, ByrID, SuppID);

                                    mailList.Add(mailTo);
                                }
                            }
                            else
                            {
                                isPwdReset = false;
                                break;
                            }
                        }

                        if (isPwdReset)
                        {
                            SetAuditLog("LeSMonitor", "New password has been set for Buyer-Supplier (" + _link.BuyerAddress.AddrCode + "-" + _link.VendorAddress.AddrCode + ") by [" + UserHostServer + "].", "Updated", "", "", ByrID.ToString(), SuppID.ToString(), _dataAccess);
                            _dataAccess.CommitTransaction();
                            return convert.ToString(newPassword.Trim());
                        }
                        else
                        {
                            SetAuditLog("LeSMonitor", "Unable to set new password for Buyer-Supplier (" + _link.BuyerAddress.AddrCode + "-" + _link.VendorAddress.AddrCode + ")", "Error", "", "", ByrID.ToString(), SuppID.ToString(), _dataAccess);
                            _dataAccess.RollbackTransaction();
                            return "";
                        }
                    }
                    else return "";
                }
                else return "";
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        #endregion

        #region /* File Audit */
        public System.Data.DataSet GetFileAudit()
        {
            return SmFileAudit.GetRecords();
        }

        public System.Data.DataSet GetFileAudit(DateTime FromDate, DateTime ToDate)
        {
            return SmFileAudit.GetRecords(FromDate, ToDate);
        }

        public System.Data.DataSet GetFileAudit_Filter(DateTime FromDate, DateTime ToDate, string FilterData)
        {
            return SmFileAudit.GetRecords_Filter(FromDate, ToDate, FilterData);
        }

        public System.Data.DataSet GetFileAuditInfo(int RecordID)
        {
            return SmFileAudit.GetFileAuditInfo(RecordID);
        }

        public System.Data.DataSet GetFileAudit_FilterDet(DateTime FromDate, DateTime ToDate, string cSrchFilter, string SEARCH, string SELECTCOND)
        {
            return SmFileAudit.GetRecords_Filterdet(FromDate, ToDate, cSrchFilter, SEARCH, SELECTCOND);
        }

        #endregion

        #region /* Login History */

        public System.Data.DataSet GetLoginHistory(DateTime FromDate, DateTime ToDate)
        {
            return LesMonitorLoginHistory.GetAllByDate(FromDate, ToDate);
        }

        public void CreateLoginHistory()
        {
            try
            {
                int _loginid = convert.ToInt(HttpContext.Current.Session["USERID"]);
                string _sessionid = convert.ToString(HttpContext.Current.Session.SessionID);
                string clientip = convert.ToString(HttpContext.Current.Session["UserHostServer"]);
                string loginRemarks = "Logged in.";

                LesMonitorLoginHistory _obj = new LesMonitorLoginHistory();
                _obj.ClientServerIp = clientip.Trim();
                _obj.Sessionid = _sessionid.Trim();
                _obj.LoggedInRemarks = loginRemarks.Trim();
                _obj.LoggedIn = DateTime.Now;
                _obj.UpdateDate = DateTime.Now;
                _obj.Userid = _loginid;
                _obj.Insert();
            }
            catch (Exception ex)
            {
                SetLog(ex.StackTrace);
            }
        }

        public void UpdateLoginHistory(bool isSessionTimeout, string _sessionid)
        {
            try
            {
                string loginOutRemarks = "";
                if (isSessionTimeout) loginOutRemarks = "Session timeout.";
                else loginOutRemarks = "Logged out.";

                LesMonitorLoginHistory _obj = LesMonitorLoginHistory.Load(_sessionid.Trim());
                if (_obj != null)
                {
                    _obj.Sessionid = _sessionid.Trim();
                    _obj.LoggedOutRemarks = loginOutRemarks.Trim();
                    _obj.LoggedOut = DateTime.Now;
                    _obj.UpdateDate = DateTime.Now;
                    _obj.Update();
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region /* File Reprocessing */

        public DataSet Get_SMV_Quotation_Vendor_by_LinkID(int LINKID, DateTime dateFrom, DateTime dateTo)
        {
            System.Data.DataSet ds = null;
            try
            {
                SmQuotationsVendor _quotedetails = new SmQuotationsVendor();
                ds = _quotedetails.Get_SMV_QUOTATION_VENDOR_BY_LINKID(LINKID, dateFrom, dateTo);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }

        #endregion

        #region   /* eInvoice Links */

        public DataSet GetSuppBuyersDetails(string SUPPLIERID,string eInvoiceExportDefault,string eInvoiceImportDefault)
        {
            System.Data.DataSet ds = new DataSet();
            DataSet LeSInvoice = new DataSet();
            ServiceCallRoutines _WebServiceCall = new ServiceCallRoutines();
            DataSetCompressor _compressor = new DataSetCompressor();

            try
            {
                if (Convert.ToInt32(SUPPLIERID) > 0)
                {
                    ds = Get_Supplier_Specific_Buyers(Convert.ToInt32(SUPPLIERID));
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string[] slParams = new string[1];
                        slParams[0] = "SUPPLIERID=" + Convert.ToInt32(SUPPLIERID);
                        object[] _obj = new object[2];
                        _obj[0] = "SELECT * FROM LES_INVOICE_BUYER_SUPPLIER_LINK WHERE SUPPLIERID = @SUPPLIERID";
                        _obj[1] = slParams;
                        byte[] _byteData = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _obj);

                        DataSet _ds = _compressor.Decompress(_byteData);
                        var d1 = ds.Tables[0].AsEnumerable();
                        var d2 = _ds.Tables[0].AsEnumerable();

                        var joinResult = from table1 in ds.Tables[0].AsEnumerable()
                                         join table2 in _ds.Tables[0].AsEnumerable() on Convert.ToString(table1["LINKID"]) equals Convert.ToString(table2["LINKID"]) into Temp
                                         from x in Temp.DefaultIfEmpty()
                                         select new
                                         {
                                             LINKID = convert.ToString(table1["LINKID"]),
                                             SUPPLIERID = convert.ToString(table1["SUPPLIERID"]),
                                             BUYERID = convert.ToString(table1["BUYERID"]),
                                             BUYER_NAME = convert.ToString(table1["BUYER_NAME"]),
                                             BUYER_CODE = convert.ToString(table1["BUYER_CODE"]),
                                             BUYER_LINK_CODE = (x != null && x["BUYER_LINK_CODE"] != DBNull.Value) ? convert.ToString(x["BUYER_LINK_CODE"]).Trim() : convert.ToString(table1["BUYER_LINK_CODE"]),
                                             VENDOR_LINK_CODE = (x != null && x["VENDOR_LINK_CODE"] != DBNull.Value) ? convert.ToString(x["VENDOR_LINK_CODE"]).Trim() : convert.ToString(table1["VENDOR_LINK_CODE"]),
                                             BUYER_EXPORT_FORMAT = (x != null) ? convert.ToString(x["BUYER_EXPORT_FORMAT"]).Trim() : "E2B",
                                             SUPPLIER_IMPORT_FORMAT = (x != null) ? convert.ToString(x["SUPPLIER_IMPORT_FORMAT"]).Trim() : "",
                                             SUPP_SENDER_CODE = (x != null) ? convert.ToString(x["SUPP_SENDER_CODE"]).Trim() : "",
                                             SUPP_RECEIVER_CODE = (x != null) ? convert.ToString(x["SUPP_RECEIVER_CODE"]).Trim() : "",
                                             BYR_SENDER_CODE = (x != null) ? convert.ToString(x["BYR_SENDER_CODE"]).Trim() : "",
                                             BYR_RECEIVER_CODE = (x != null) ? convert.ToString(x["BYR_RECEIVER_CODE"]) : "",
                                             CURR_CODE = (x != null && x["CURR_CODE"] != DBNull.Value) ? convert.ToString(x["CURR_CODE"]) : "",
                                             CURRENCYID = (x != null && x["CURRENCYID"] != DBNull.Value) ? convert.ToString(x["CURRENCYID"]) : "",
                                             INVLINKID = (x != null && x["INVLINKID"] != DBNull.Value) ? convert.ToString(x["INVLINKID"]) : "",
                                             SUPP_ORG_NO = (x != null && x["SUPP_ORG_NO"] != DBNull.Value) ? convert.ToString(x["SUPP_ORG_NO"]) : "",
                                             BYR_ORG_NO = (x != null && x["BYR_ORG_NO"] != DBNull.Value) ? convert.ToString(x["BYR_ORG_NO"]) : "",
                                             PAYMENT_MODE = (x != null && x["PAYMENT_MODE"] != DBNull.Value) ? convert.ToString(x["PAYMENT_MODE"]) : "",
                                             BANK_NAME = (x != null && x["BANK_NAME"] != DBNull.Value) ? convert.ToString(x["BANK_NAME"]) : "",
                                             BUYER_CONTACT = (x != null && x["BUYER_CONTACT"] != DBNull.Value) ? convert.ToString(x["BUYER_CONTACT"]).Trim() : convert.ToString(table1["BUYER_CONTACT"]),
                                             SUPPLIER_CONTACT = (x != null && x["SUPPLIER_CONTACT"] != DBNull.Value) ? convert.ToString(x["SUPPLIER_CONTACT"]).Trim() : convert.ToString(table1["SUPPLIER_CONTACT"]),
                                             SUPPLIER_EMAIL = (x != null && x["SUPPLIER_EMAIL"] != DBNull.Value) ? convert.ToString(x["SUPPLIER_EMAIL"]).Trim() : convert.ToString(table1["SUPPLIER_EMAIL"]),
                                             BUYER_EMAIL = (x != null && x["BUYER_EMAIL"] != DBNull.Value) ? convert.ToString(x["BUYER_EMAIL"]).Trim() : convert.ToString(table1["BUYER_EMAIL"]),
                                             CC_EMAIL = (x != null && x["CC_EMAIL"] != DBNull.Value) ? convert.ToString(x["CC_EMAIL"]).Trim() : convert.ToString(table1["CC_EMAIL"]),
                                             BCC_EMAIL = (x != null && x["BCC_EMAIL"] != DBNull.Value) ? convert.ToString(x["BCC_EMAIL"]).Trim() : convert.ToString(table1["BCC_EMAIL"]),
                                             IMPORT_PATH = (x != null && x["INBOX_PATH"] != DBNull.Value) ? convert.ToString(x["INBOX_PATH"]).Trim() : eInvoiceImportDefault + "\\" + Convert.ToString(table1["BUYER_CODE"]) + "\\" + Convert.ToString(table1["SUPPLIER_CODE"]),
                                             EXPORT_PATH = (x != null) ? convert.ToString(x["OUTBOX_PATH"]).Trim() : eInvoiceExportDefault + "\\" + convert.ToString(table1["BUYER_CODE"]),
                                             ACCOUNT_NAME = (x != null) ? convert.ToString(x["ACCOUNT_NAME"]).Trim() : "",
                                             ACCOUNT_NO = (x != null) ? convert.ToString(x["ACCOUNT_NO"]).Trim() : "",
                                             SWIFT_NO = (x != null) ? convert.ToString(x["SWIFT_NO"]).Trim() : "",
                                             IBAN_NO = (x != null) ? convert.ToString(x["IBAN_NO"]).Trim() : "",
                                             CONVERT_TO_TIFF = (x != null && x["CONVERT_TO_TIFF"] != DBNull.Value) ? convert.ToInt(x["CONVERT_TO_TIFF"]) : 0,
                                             NOTIFY_BUYER = (x != null && x["NOTIFY_BUYER"] != DBNull.Value) ? convert.ToInt(x["NOTIFY_BUYER"]) : 0,
                                             NOTIFY_SUPPLR = (x != null && x["NOTIFY_SUPPLR"] != DBNull.Value) ? convert.ToInt(x["NOTIFY_SUPPLR"]) : 0,
                                             EMBED_ATTACHMENT = (x != null && x["EMBED_ATTACHMENT"] != DBNull.Value) ? convert.ToInt(x["EMBED_ATTACHMENT"]) : 0,
                                             PO_EXIST = (x != null && x["PO_EXIST"] != DBNull.Value) ? convert.ToInt(x["PO_EXIST"]) : 0,
                                             IS_ACTIVE = (x != null && x["IS_ACTIVE"] != DBNull.Value) ? convert.ToInt(x["IS_ACTIVE"]) : 0
                                         };

                        DataTable dataT = new DataTable();
                        dataT = joinResult.ConvertToDataTable(item => item.LINKID,
                                                    item => item.SUPPLIERID,
                                                    item => item.BUYERID,
                                                    item => item.BUYER_NAME,
                                                    item => item.BUYER_CODE,
                                                    item => item.BUYER_LINK_CODE,
                                                    item => item.VENDOR_LINK_CODE,
                                                    item => item.BUYER_EXPORT_FORMAT,
                                                    item => item.SUPPLIER_IMPORT_FORMAT,
                                                    item => item.SUPP_SENDER_CODE,
                                                    item => item.SUPP_RECEIVER_CODE,
                                                    item => item.BYR_SENDER_CODE,
                                                    item => item.BYR_RECEIVER_CODE,
                                                    item => item.CURR_CODE,
                                                    item => item.CURRENCYID,
                                                    item => item.INVLINKID,
                                                    item => item.SUPP_ORG_NO,
                                                    item => item.BYR_ORG_NO,
                                                    item => item.PAYMENT_MODE,
                                                    item => item.BANK_NAME,
                                                    item => item.BUYER_CONTACT,
                                                    item => item.SUPPLIER_CONTACT,
                                                    item => item.SUPPLIER_EMAIL,
                                                    item => item.BUYER_EMAIL,
                                                    item => item.CC_EMAIL,
                                                    item => item.BCC_EMAIL,
                                                    item => item.IMPORT_PATH,
                                                    item => item.EXPORT_PATH,
                                                    item => item.ACCOUNT_NAME,
                                                    item => item.ACCOUNT_NO,
                                                    item => item.SWIFT_NO,
                                                    item => item.IBAN_NO,
                                                    item => item.CONVERT_TO_TIFF,
                                                    item => item.NOTIFY_BUYER,
                                                    item => item.NOTIFY_SUPPLR,
                                                    item => item.EMBED_ATTACHMENT,
                                                    item => item.PO_EXIST,
                                                    item => item.IS_ACTIVE
                                                    );

                        LeSInvoice.Tables.Add(dataT);
                    }
                }
            }
            catch (Exception ex) { }
            return LeSInvoice;
        }

        public string Save_InvoiceDetails(string nLinkID, string oBUYER_CODE, string SUPPLIER_CODE, string eInvoiceURL, Dictionary<string, string> _Odictexp, Dictionary<string, string> _Ndictexp, string HostName)
        {          
            string _updatedData = "", AuditValue = "",_result="";
            ServiceCallRoutines _WebServiceCall = new ServiceCallRoutines();
            DataSetCompressor _compressor = new DataSetCompressor();
            List<string> slvalues = new List<string>();
            try
            {
                _updatedData = SetAuditValue(_Odictexp, _Ndictexp);
                object[] objArgument = new object[2];
                string[] slParams = new string[1];
                string query = "SELECT * FROM LES_INVOICE_BUYER_SUPPLIER_LINK WHERE LINKID= @LINKID";
                objArgument[0] = query;
                slParams[0] = "LINKID=" + nLinkID;
                objArgument[1] = slParams;
                byte[] _byteData = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", objArgument);
                DataSet _ds = _compressor.Decompress(_byteData);
                if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                {
                    string _InsDataVal = UpdateInvoice(_Ndictexp, eInvoiceURL, HostName);
                    if (_InsDataVal == "1")
                    {
                        AuditValue = "Invoice Link updated for Buyer Code : " + oBUYER_CODE + " and Supplier Code : " + SUPPLIER_CODE + " by : " + HostName;
                        if (_updatedData.Trim().Length > 0)
                        {
                            AuditValue += " following fields updated " + _updatedData;
                        }
                        else
                        {
                            AuditValue += " with no changes";
                        }
                        SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "");
                        _result = "1";
                    }
                }
                else
                {
                    string _InsDataVal = InsertInvoice(_Ndictexp, eInvoiceURL, HostName);
                    if (_InsDataVal == "1")
                    {
                        AuditValue = "Invoice Link inserted for Buyer Code : " + oBUYER_CODE + " and Supplier Code : " + SUPPLIER_CODE + " by : " + HostName;
                        if (_updatedData.Trim().Length > 0)
                        {
                            AuditValue += " following fields updated " + _updatedData;
                        }
                        else
                        {
                            AuditValue += " with no changes";
                        }
                        SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "");
                        _result = "1";
                    }
                }
            }
            catch
            {
                _result = "";
            }
            return _result;
        }

        private string UpdateInvoice(Dictionary<string, string> _dictexp, string eInvoiceURL, string HostName)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            ServiceCallRoutines _WebServiceCall = new ServiceCallRoutines();
            string LinkID = "", Curr_ID = "", Curr_Code = "", SwiftNo = "", AccountName = "", AccountNo = "", IbanNo = "", ByrOrgNo = "", SuppOrgNo = "", 
                 BankName="", PaymentMode="",_BuyerCode="", _SupplierCode="";
            string _DataVal = "", _result=""; string cDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            List<string> slvalues = new List<string>();
            object[] objService = new object[2];
            string[] _param = new string[37];
            foreach (string key in _dictexp.Keys)
            {
                switch (key.ToUpper())
                {
                    case "LINKID": _param[0] = "LINKID=" + _dictexp[key]; LinkID = _dictexp[key]; break;
                    case "SUPPLIER_CONTACT": _param[1] = "SUPPLIER_CONTACT=" + _dictexp[key]; break;
                    case "BUYER_CONTACT": _param[2] = "BUYER_CONTACT=" + _dictexp[key]; break;
                    case "SUPPLIER_EMAIL": _param[3] = "SUPPLIER_EMAIL=" + _dictexp[key]; break;
                    case "BUYER_EMAIL": _param[4] = "BUYER_EMAIL=" + _dictexp[key]; break;
                    case "CC_EMAIL": _param[5] = "CC_EMAIL=" + _dictexp[key]; break;
                    case "BCC_EMAIL": _param[6] = "BCC_EMAIL=" + _dictexp[key]; break;
                    case "CURRENCYID": _param[7] = "CURRENCYID=" + _dictexp[key]; Curr_ID = _dictexp[key]; break;
                    case "CURR_CODE": _param[8] = "CURR_CODE=" + _dictexp[key]; Curr_Code = _dictexp[key]; break;
                    case "ACCOUNT_NO": _param[9] = "ACCOUNT_NO=" + _dictexp[key]; AccountNo = _dictexp[key]; break;
                    case "SWIFT_NO": _param[10] = "SWIFT_NO=" + _dictexp[key]; SwiftNo = _dictexp[key]; break;
                    case "IBAN_NO": _param[11] = "IBAN_NO=" + _dictexp[key]; IbanNo = _dictexp[key]; break;

                    case "IS_ACTIVE": _param[12] = "IS_ACTIVE=" + _dictexp[key]; break;
                    case "CONVERT_TO_TIFF": _param[13] = "CONVERT_TO_TIFF=" + _dictexp[key]; break;
                    case "VENDOR_LINK_CODE": _param[14] = "VENDOR_LINK_CODE=" + _dictexp[key]; break;
                    case "BUYER_LINK_CODE": _param[15] = "BUYER_LINK_CODE=" + _dictexp[key]; break;
                    case "SUPPLIERID": _param[16] = "SUPPLIERID=" + _dictexp[key]; break;
                    case "BUYERID": _param[17] = "BUYERID=" + _dictexp[key]; break;

                    case "SUPPLIER_FORMAT_CODE": _param[18] = "SUPPLIER_IMPORT_FORMAT=" + _dictexp[key]; break;
                    case "BUYER_FORMAT_CODE": _param[19] = "BUYER_EXPORT_FORMAT=" + _dictexp[key]; break;
                    case "IMPORT_PATH": _param[20] = "INBOX_PATH=" + _dictexp[key]; break;
                    case "EXPORT_PATH": _param[21] = "OUTBOX_PATH=" + _dictexp[key]; break;
                    case "NOTIFY_SUPPLR": _param[22] = "NOTIFY_SUPPLR=" + _dictexp[key]; break;
                    case "NOTIFY_BUYER": _param[23] = "NOTIFY_BUYER=" + _dictexp[key]; break;
                    case "EMBED_ATTACHMENT": _param[24] = "EMBED_ATTACHMENT=" + _dictexp[key]; break;

                    case "SUPP_SENDER_CODE": _param[25] = "SUPP_SENDER_CODE=" + _dictexp[key]; break;
                    case "SUPP_RECEIVER_CODE": _param[26] = "SUPP_RECEIVER_CODE=" + _dictexp[key]; break;
                    case "BYR_SENDER_CODE": _param[27] = "BYR_SENDER_CODE=" + _dictexp[key]; break;
                    case "BYR_RECEIVER_CODE": _param[28] = "BYR_RECEIVER_CODE=" + _dictexp[key]; break;
                    case "PO_EXIST": _param[29] = "PO_EXIST=" + _dictexp[key]; break;
                    case "BUYER_CODE": _param[30] = "BUYER_CODE=" + _dictexp[key]; _BuyerCode = _dictexp[key]; break;
                    case "SUPPLIER_CODE": _param[31] = "SUPPLIER_CODE=" + _dictexp[key]; _SupplierCode = _dictexp[key]; break;
                    case "SUPP_ORG_NO": _param[32] = "SUPP_ORG_NO=" + _dictexp[key]; SuppOrgNo = _dictexp[key]; break;
                    case "BYR_ORG_NO": _param[33] = "BYR_ORG_NO=" + _dictexp[key]; ByrOrgNo = _dictexp[key]; break;
                    case "PAYMENT_MODE": _param[34] = "PAYMENT_MODE=" + _dictexp[key]; PaymentMode = _dictexp[key]; break;
                    case "BANK_NAME": _param[35] = "BANK_NAME=" + _dictexp[key]; BankName = _dictexp[key]; break;
                    case "ACCOUNT_NAME": _param[36] = "ACCOUNT_NAME=" + _dictexp[key]; AccountName = _dictexp[key]; break;
                }
            }
            string strSql = "UPDATE LES_INVOICE_BUYER_SUPPLIER_LINK SET SUPPLIER_CONTACT=@SUPPLIER_CONTACT,BUYER_CONTACT=@BUYER_CONTACT,"+
            " SUPPLIER_EMAIL=@SUPPLIER_EMAIL,BUYER_EMAIL=@BUYER_EMAIL ,CC_EMAIL=@CC_EMAIL , BCC_EMAIL=@BCC_EMAIL ,CURRENCYID=@CURRENCYID ,CURR_CODE=@CURR_CODE, "
                 + " ACCOUNT_NO=@ACCOUNT_NO , SWIFT_NO=@SWIFT_NO , IBAN_NO=@IBAN_NO , IS_ACTIVE=@IS_ACTIVE , CONVERT_TO_TIFF=@CONVERT_TO_TIFF  , "+
                 "VENDOR_LINK_CODE=@VENDOR_LINK_CODE , BUYER_LINK_CODE=@BUYER_LINK_CODE  , SUPPLIERID=@SUPPLIERID , BUYERID=@BUYERID ,  "+
                 "SUPPLIER_IMPORT_FORMAT=@SUPPLIER_IMPORT_FORMAT ,BUYER_EXPORT_FORMAT=@BUYER_EXPORT_FORMAT,"
                 + " INBOX_PATH=@INBOX_PATH , OUTBOX_PATH=@OUTBOX_PATH ,NOTIFY_SUPPLR=@NOTIFY_SUPPLR ,NOTIFY_BUYER=@NOTIFY_BUYER, "+
            " EMBED_ATTACHMENT =@EMBED_ATTACHMENT , SUPP_SENDER_CODE =@SUPP_SENDER_CODE,SUPP_RECEIVER_CODE =@SUPP_RECEIVER_CODE,BYR_SENDER_CODE =@BYR_SENDER_CODE,"+
            "BYR_RECEIVER_CODE =@BYR_RECEIVER_CODE ,PO_EXIST=@PO_EXIST,UPDATE_DATE = getdate(),E_BUYER_CODE=@BUYER_CODE,E_SUPPLIER_CODE=@SUPPLIER_CODE,"+
            "SUPP_ORG_NO=@SUPP_ORG_NO,BYR_ORG_NO=@BYR_ORG_NO,PAYMENT_MODE=@PAYMENT_MODE,BANK_NAME=@BANK_NAME,ACCOUNT_NAME=@ACCOUNT_NAME WHERE LINKID = @LINKID";
            objService[0] = strSql;
            objService[1] = _param;
            _DataVal = (string)_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "ExecuteQueryParam", objService);
            if(_DataVal == "1")
            {
                SetAccountCurrencyDetails_(Convert.ToInt32(LinkID), Curr_ID, Curr_Code, SwiftNo, AccountName, AccountNo, IbanNo, ByrOrgNo, SuppOrgNo, BankName, PaymentMode, _BuyerCode, _SupplierCode, HostName);
                SmBuyerSupplierLink _link = SmBuyerSupplierLink.Load(Convert.ToInt32(LinkID));
                int ByrID = convert.ToInt(_link.BuyerAddress.Addressid);
                int SuppID = convert.ToInt(_link.VendorAddress.Addressid);
                DataSet dtUsers = new DataSet();
                _dataAccess.CreateSQLCommand("SELECT * FROM SM_EXTERNAL_USERS WHERE ADDRESSID=@SuppID");
                _dataAccess.AddParameter("SuppID", SuppID, ParameterDirection.Input);
                dtUsers = _dataAccess.ExecuteDataSet();
                if (dtUsers != null && dtUsers.Tables[0].Rows.Count > 0)
                {
                    _dataAccess.CreateSQLCommand("UPDATE SM_EXTERNAL_USERS SET UPDATE_DATE=GETDATE(), INV_USERTYPE = 3 WHERE LINKID =" + LinkID + " AND ADDRESSID = " + SuppID);
                    int affectedRows = _dataAccess.ExecuteNonQuery();
                }
            }
            return _DataVal;
        }

        private string InsertInvoice(Dictionary<string, string> _dictexp, string eInvoiceURL, string HostName)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            ServiceCallRoutines _WebServiceCall = new ServiceCallRoutines();
            string LinkID = "", Curr_ID = "", Curr_Code = "", SwiftNo = "", AccountName = "", AccountNo = "", IbanNo = "", ByrOrgNo = "", SuppOrgNo = "",
              BankName = "", PaymentMode = "", _BuyerCode = "", _SupplierCode = "";
            DataSetCompressor _compressor = new DataSetCompressor();
            string _DataVal = "", _Result = "";
            string cDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            object[] objService = new object[2];
            string[] _param = new string[37];
            foreach (string key in _dictexp.Keys)
            {
                switch (key.ToUpper())
                {
                    case "LINKID": _param[0] = "LINKID=" + _dictexp[key]; LinkID = _dictexp[key]; break;
                    case "SUPPLIER_CONTACT": _param[1] = "SUPPLIER_CONTACT=" + _dictexp[key]; break;
                    case "BUYER_CONTACT": _param[2] = "BUYER_CONTACT=" + _dictexp[key]; break;
                    case "SUPPLIER_EMAIL": _param[3] = "SUPPLIER_EMAIL=" + _dictexp[key]; break;
                    case "BUYER_EMAIL": _param[4] = "BUYER_EMAIL=" + _dictexp[key]; break;
                    case "CC_EMAIL": _param[5] = "CC_EMAIL=" + _dictexp[key]; break;
                    case "BCC_EMAIL": _param[6] = "BCC_EMAIL=" + _dictexp[key]; break;
                    case "CURRENCYID": _param[7] = "CURRENCYID=" + _dictexp[key]; Curr_ID = _dictexp[key]; break;
                    case "CURR_CODE": _param[8] = "CURR_CODE=" + _dictexp[key]; Curr_Code = _dictexp[key]; break;
                    case "ACCOUNT_NO": _param[9] = "ACCOUNT_NO=" + _dictexp[key]; AccountNo = _dictexp[key]; break;
                    case "SWIFT_NO": _param[10] = "SWIFT_NO=" + _dictexp[key]; SwiftNo = _dictexp[key]; break;
                    case "IBAN_NO": _param[11] = "IBAN_NO=" + _dictexp[key]; IbanNo = _dictexp[key]; break;
                    case "IS_ACTIVE": _param[12] = "IS_ACTIVE=" + _dictexp[key]; break;
                    case "CONVERT_TO_TIFF": _param[13] = "CONVERT_TO_TIFF=" + _dictexp[key]; break;
                    case "VENDOR_LINK_CODE": _param[14] = "VENDOR_LINK_CODE=" + _dictexp[key]; break;
                    case "BUYER_LINK_CODE": _param[15] = "BUYER_LINK_CODE=" + _dictexp[key]; break;
                    case "SUPPLIERID": _param[16] = "SUPPLIERID=" + _dictexp[key]; break;
                    case "BUYERID": _param[17] = "BUYERID=" + _dictexp[key]; break;
                    case "SUPPLIER_FORMAT_CODE": _param[18] = "SUPPLIER_IMPORT_FORMAT=" + _dictexp[key]; break;
                    case "BUYER_FORMAT_CODE": _param[19] = "BUYER_EXPORT_FORMAT=" + _dictexp[key]; break;
                    case "IMPORT_PATH": _param[20] = "INBOX_PATH=" + _dictexp[key]; break;
                    case "EXPORT_PATH": _param[21] = "OUTBOX_PATH=" + _dictexp[key]; break;
                    case "NOTIFY_SUPPLR": _param[22] = "NOTIFY_SUPPLR=" + _dictexp[key]; break;
                    case "NOTIFY_BUYER": _param[23] = "NOTIFY_BUYER=" + _dictexp[key]; break;
                    case "EMBED_ATTACHMENT": _param[24] = "EMBED_ATTACHMENT=" + _dictexp[key]; break;
                    case "SUPP_SENDER_CODE": _param[25] = "SUPP_SENDER_CODE=" + _dictexp[key]; break;
                    case "SUPP_RECEIVER_CODE": _param[26] = "SUPP_RECEIVER_CODE=" + _dictexp[key]; break;
                    case "BYR_SENDER_CODE": _param[27] = "BYR_SENDER_CODE=" + _dictexp[key]; break;
                    case "BYR_RECEIVER_CODE": _param[28] = "BYR_RECEIVER_CODE=" + _dictexp[key]; break;
                    case "PO_EXIST": _param[29] = "PO_EXIST=" + _dictexp[key]; break;
                    case "BUYER_CODE": _param[30] = "BUYER_CODE=" + _dictexp[key]; _BuyerCode = _dictexp[key]; break;
                    case "SUPPLIER_CODE": _param[31] = "SUPPLIER_CODE=" + _dictexp[key]; _SupplierCode = _dictexp[key]; break;
                    case "SUPP_ORG_NO": _param[32] = "SUPP_ORG_NO=" + _dictexp[key]; SuppOrgNo = _dictexp[key]; break;
                    case "BYR_ORG_NO": _param[33] = "BYR_ORG_NO=" + _dictexp[key]; ByrOrgNo = _dictexp[key]; break;
                    case "PAYMENT_MODE": _param[34] = "PAYMENT_MODE=" + _dictexp[key]; PaymentMode = _dictexp[key]; break;
                    case "BANK_NAME": _param[35] = "BANK_NAME=" + _dictexp[key]; BankName = _dictexp[key]; break;
                    case "ACCOUNT_NAME": _param[36] = "ACCOUNT_NAME=" + _dictexp[key]; AccountName = _dictexp[key]; break;
                }
            }
            string strSql = "INSERT INTO LES_INVOICE_BUYER_SUPPLIER_LINK (LINKID,SUPPLIER_CONTACT,BUYER_CONTACT,SUPPLIER_EMAIL,BUYER_EMAIL , CC_EMAIL , BCC_EMAIL ,CURRENCYID, CURR_CODE , "
                 + " ACCOUNT_NO , SWIFT_NO , IBAN_NO , IS_ACTIVE , CONVERT_TO_TIFF  , VENDOR_LINK_CODE , BUYER_LINK_CODE  , SUPPLIERID , BUYERID , SUPPLIER_IMPORT_FORMAT , BUYER_EXPORT_FORMAT , "
                 + " INBOX_PATH , OUTBOX_PATH , NOTIFY_SUPPLR , NOTIFY_BUYER,EMBED_ATTACHMENT,SUPP_SENDER_CODE, SUPP_RECEIVER_CODE, BYR_SENDER_CODE, BYR_RECEIVER_CODE,PO_EXIST,CREATED_DATE,UPDATE_DATE,E_BUYER_CODE,E_SUPPLIER_CODE,SUPP_ORG_NO,BYR_ORG_NO,PAYMENT_MODE,BANK_NAME,ACCOUNT_NAME) values (@LINKID,@SUPPLIER_CONTACT,@BUYER_CONTACT,@SUPPLIER_EMAIL,@BUYER_EMAIL , @CC_EMAIL , @BCC_EMAIL ,@CURRENCYID, @CURR_CODE , "
                 + " @ACCOUNT_NO , @SWIFT_NO , @IBAN_NO , @IS_ACTIVE , @CONVERT_TO_TIFF , @VENDOR_LINK_CODE , @BUYER_LINK_CODE  , @SUPPLIERID , @BUYERID , @SUPPLIER_IMPORT_FORMAT , @BUYER_EXPORT_FORMAT , "
                 + " @INBOX_PATH , @OUTBOX_PATH , @NOTIFY_SUPPLR , @NOTIFY_BUYER,@EMBED_ATTACHMENT ,@SUPP_SENDER_CODE, @SUPP_RECEIVER_CODE, @BYR_SENDER_CODE, @BYR_RECEIVER_CODE,@PO_EXIST,getdate(),getdate(),@BUYER_CODE,@SUPPLIER_CODE,@SUPP_ORG_NO,@BYR_ORG_NO,@PAYMENT_MODE,@BANK_NAME,@ACCOUNT_NAME)";
            objService[0] = strSql;
            objService[1] = _param;
            _DataVal = (string)_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "ExecuteQueryParam", objService);
            if (_DataVal == "1")
            {
                SetAccountCurrencyDetails_(Convert.ToInt32(LinkID), Curr_ID, Curr_Code, SwiftNo, AccountName, AccountNo, IbanNo, ByrOrgNo, SuppOrgNo, BankName, PaymentMode, _BuyerCode, _SupplierCode, HostName);
                SmBuyerSupplierLink _link = SmBuyerSupplierLink.Load(Convert.ToInt32(LinkID));
                int ByrID = convert.ToInt(_link.BuyerAddress.Addressid);
                int SuppID = convert.ToInt(_link.VendorAddress.Addressid);
                DataSet dtUsers = new DataSet();
                _dataAccess.CreateSQLCommand("SELECT * FROM SM_EXTERNAL_USERS WHERE ADDRESSID=@SuppID");
                _dataAccess.AddParameter("SuppID", SuppID, ParameterDirection.Input);
                dtUsers = _dataAccess.ExecuteDataSet();
                if (dtUsers != null && dtUsers.Tables[0].Rows.Count > 0)
                {
                    _dataAccess.CreateSQLCommand("UPDATE SM_EXTERNAL_USERS SET UPDATE_DATE=GETDATE(), INV_USERTYPE = 3 WHERE LINKID =" + LinkID + " AND ADDRESSID = " + SuppID);
                    int affectedRows = _dataAccess.ExecuteNonQuery();
                }
            }
            return _DataVal;
        }

        private string SetAuditValue(Dictionary<string, string> slOlddet, Dictionary<string, string> slNewdet)
        {
            string _updatedData = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            object[] objService = new object[2];
            string[] _param = new string[31];
            SetUpdateLog(ref _updatedData, "Supplier Contact", convert.ToString(slOlddet["SUPPLIER_CONTACT"]), convert.ToString(slNewdet["SUPPLIER_CONTACT"]));
            SetUpdateLog(ref _updatedData, "Buyer Contact", convert.ToString(slOlddet["BUYER_CONTACT"]), convert.ToString(slNewdet["BUYER_CONTACT"]));
            SetUpdateLog(ref _updatedData, "Supplier Email", convert.ToString(slOlddet["SUPPLIER_EMAIL"]), convert.ToString(slNewdet["SUPPLIER_EMAIL"]));
            SetUpdateLog(ref _updatedData, "Buyer Email", convert.ToString(slOlddet["BUYER_EMAIL"]), convert.ToString(slNewdet["BUYER_EMAIL"]));
            SetUpdateLog(ref _updatedData, "CC Email", convert.ToString(slOlddet["CC_EMAIL"]), convert.ToString(slNewdet["CC_EMAIL"]));
            SetUpdateLog(ref _updatedData, "Bcc Email", convert.ToString(slOlddet["BCC_EMAIL"]), convert.ToString(slNewdet["BCC_EMAIL"]));
            SetUpdateLog(ref _updatedData, "Account no", convert.ToString(slOlddet["ACCOUNT_NO"]), convert.ToString(slNewdet["ACCOUNT_NO"]));
            SetUpdateLog(ref _updatedData, "Swift no", convert.ToString(slOlddet["SWIFT_NO"]), convert.ToString(slNewdet["SWIFT_NO"]));
            SetUpdateLog(ref _updatedData, "Iban no", convert.ToString(slOlddet["IBAN_NO"]), convert.ToString(slNewdet["IBAN_NO"]));
            SetUpdateLog(ref _updatedData, "Active", convert.ToString(slOlddet["IS_ACTIVE"]), convert.ToString(slNewdet["IS_ACTIVE"]));
            SetUpdateLog(ref _updatedData, "Convert to Tiff", convert.ToString(slOlddet["CONVERT_TO_TIFF"]), convert.ToString(slNewdet["CONVERT_TO_TIFF"]));
            SetUpdateLog(ref _updatedData, "Supplier link code", convert.ToString(slOlddet["VENDOR_LINK_CODE"]), convert.ToString(slNewdet["VENDOR_LINK_CODE"]));
            SetUpdateLog(ref _updatedData, "Buyer link code", convert.ToString(slOlddet["BUYER_LINK_CODE"]), convert.ToString(slNewdet["BUYER_LINK_CODE"]));
            SetUpdateLog(ref _updatedData, "Supplier Export Format", convert.ToString(slOlddet["SUPPLIER_FORMAT_CODE"]), convert.ToString(slNewdet["SUPPLIER_FORMAT_CODE"]));
            SetUpdateLog(ref _updatedData, "Buyer Import Format", convert.ToString(slOlddet["BUYER_FORMAT_CODE"]), convert.ToString(slNewdet["BUYER_FORMAT_CODE"]));
            SetUpdateLog(ref _updatedData, "Inbox path", convert.ToString(slOlddet["IMPORT_PATH"]), convert.ToString(slNewdet["IMPORT_PATH"]));
            SetUpdateLog(ref _updatedData, "Outbox path", convert.ToString(slOlddet["EXPORT_PATH"]), convert.ToString(slNewdet["EXPORT_PATH"]));
            SetUpdateLog(ref _updatedData, "Notify Supplier", convert.ToString(slOlddet["NOTIFY_SUPPLR"]), convert.ToString(slNewdet["NOTIFY_SUPPLR"]));
            SetUpdateLog(ref _updatedData, "Notify Buyer", convert.ToString(slOlddet["NOTIFY_BUYER"]), convert.ToString(slNewdet["NOTIFY_BUYER"]));
            SetUpdateLog(ref _updatedData, "Embed Attachment", convert.ToString(slOlddet["EMBED_ATTACHMENT"]), convert.ToString(slNewdet["EMBED_ATTACHMENT"]));
            SetUpdateLog(ref _updatedData, "Currency", convert.ToString(slOlddet["CURR_CODE"]), convert.ToString(slNewdet["CURR_CODE"]));
            SetUpdateLog(ref _updatedData, "Supplier sender code", convert.ToString(slOlddet["SUPP_SENDER_CODE"]), convert.ToString(slNewdet["SUPP_SENDER_CODE"]));
            SetUpdateLog(ref _updatedData, "Supplier receiver code", convert.ToString(slOlddet["SUPP_RECEIVER_CODE"]), convert.ToString(slNewdet["SUPP_RECEIVER_CODE"]));
            SetUpdateLog(ref _updatedData, "Buyer sender code", convert.ToString(slOlddet["BYR_SENDER_CODE"]), convert.ToString(slNewdet["BYR_SENDER_CODE"]));
            SetUpdateLog(ref _updatedData, "Buyer receiver code", convert.ToString(slOlddet["BYR_RECEIVER_CODE"]), convert.ToString(slNewdet["BYR_RECEIVER_CODE"]));
            SetUpdateLog(ref _updatedData, "Po Exist on eSupplier", convert.ToString(slOlddet["PO_EXIST"]), convert.ToString(slNewdet["PO_EXIST"]));
            return _updatedData;
        }

        public DataSet FillAccountDetailsGrid(string INVLINKID)
        {
            DataSetCompressor _compressor = new DataSetCompressor();
            DataSet _ds = new DataSet();
            try
            {
                string sSqlAcc = "SELECT * FROM SMV_LES_CURRENCY_ACCOUNT_DETAILS WHERE INVLINKID=@INVLINKID";
                object[] _objAcc = new object[2];
                string[] _paramAcc = new string[1];
                _objAcc[0] = sSqlAcc;
                _paramAcc[0] = "INVLINKID=" + INVLINKID;
                _objAcc[1] = _paramAcc;
                byte[] _byteData2 = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _objAcc);
                _ds = _compressor.Decompress(_byteData2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _ds;
        }

        public DataSet CheckCurrency_Account(int INVLINKID,int CURR_ACCOUNTID, string CURR_CODE)
        {  
            DataSetCompressor _compressor = new DataSetCompressor();
            DataSet _ds = new DataSet();
            try
            {
                string sSqlAcc = "SELECT * FROM SMV_LES_CURRENCY_ACCOUNT_DETAILS WHERE INVLINKID=@INVLINKID AND CURR_CODE=@CURR_CODE AND CURR_ACCOUNTID <> @CURR_ACCOUNTID";
                object[] _objAcc = new object[2];
                string[] _paramAcc = new string[3];
                _objAcc[0] = sSqlAcc;
                _paramAcc[0] = "INVLINKID=" + INVLINKID;
                _paramAcc[1] = "CURR_CODE=" + CURR_CODE;
                _paramAcc[2] = "CURR_ACCOUNTID=" + CURR_ACCOUNTID;
                _objAcc[1] = _paramAcc;
                byte[] _byteData2 = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _objAcc);
                _ds = _compressor.Decompress(_byteData2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _ds;
        }

        public  string GetAccountDetails_Currency(string CurrencyId,string Linkid)
        {
            string val = "||||||||";
            DataSet _ds = new DataSet();
            DataSetCompressor _compressor = new DataSetCompressor();
            try
            {               
                if (!string.IsNullOrEmpty(Linkid))
                {
                    string sSqlAcc = "SELECT * FROM SMV_LES_CURRENCY_ACCOUNT_DETAILS WHERE LINKID=@LINKID AND CURRENCYID=@CURRENCYID";
                    object[] _objAcc = new object[2];
                    string[] _paramAcc = new string[2];
                    _objAcc[0] = sSqlAcc;
                    _paramAcc[0] = "LINKID=" + Linkid;
                    _paramAcc[1] = "CURRENCYID=" + CurrencyId;
                    _objAcc[1] = _paramAcc;
                    ServiceCallRoutines _WebServiceCalls = new ServiceCallRoutines();
                    DataSetCompressor _compressors = new DataSetCompressor();
                    string eInvoiceURLs = ConfigurationManager.AppSettings["EINVOICE_WEB_SERVICE"];
                    byte[] _byteData2 = (byte[])_WebServiceCalls.CallWebService(eInvoiceURLs, "LesInvoiceService", "GetQueryDataParam", _objAcc);
                    _ds = _compressors.Decompress(_byteData2);
                    if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow _dr in _ds.Tables[0].Rows)
                        {
                            val = convert.ToString(_dr["ACCOUNT_NAME"]).Trim() + "|" + convert.ToString(_dr["ACCOUNT_NO"]).Trim() + "|" + convert.ToString(_dr["SWIFT_NO"]).Trim() + "|" + convert.ToString(_dr["IBAN_NO"]).Trim() + "|" + convert.ToString(_dr["KVK_NO"]).Trim() + "|" +
                                  convert.ToString(_dr["BUYER_ORG_NO"]).Trim() + "|" + convert.ToString(_dr["SUPPLIER_ORG_NO"]).Trim() + "|" + convert.ToString(_dr["BANK_NAME"]).Trim() + "|" + convert.ToString(_dr["PAYMENT_MODE"]).Trim();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return val;
        }
     
        public string Save_InvBuyerSupplier_Account(Dictionary<string, string> sleInvBSAccDet, string Hostname)
        {
            string _res = "";

            string Curr_Code = "", Curr_ID = "", SwiftNo = "", IbanNo = "", PaymentMode = "", AccountName = "", 
                AccountNo = "", BankName = "", KvkNo = "", SuppOrgNo = "", ByrOrgNo = "", sDefault = "";            

            string SupplierCode = (sleInvBSAccDet.ContainsKey("SUPPLIER_CODE")) ? sleInvBSAccDet["SUPPLIER_CODE"].Trim() : null;
            string BuyerCode = (sleInvBSAccDet.ContainsKey("BUYER_CODE")) ? sleInvBSAccDet["BUYER_CODE"].Trim() : null;
            int InvLinkId = (sleInvBSAccDet.ContainsKey("INVLINKID")) ? Convert.ToInt32(sleInvBSAccDet["INVLINKID"].Trim()) : 0;
            Curr_Code = (sleInvBSAccDet.ContainsKey("CURR_CODE")) ? convert.ToString(sleInvBSAccDet["CURR_CODE"]).Trim() : null;
            Curr_ID = (sleInvBSAccDet.ContainsKey("CURRENCYID")) ? convert.ToString(sleInvBSAccDet["CURRENCYID"]).Trim() : null;
            AccountName =  (sleInvBSAccDet.ContainsKey("ACCOUNT_NAME")) ?convert.ToString(sleInvBSAccDet["ACCOUNT_NAME"]).Trim() :null;
            SwiftNo =  (sleInvBSAccDet.ContainsKey("SWIFT_NO")) ?convert.ToString(sleInvBSAccDet["SWIFT_NO"]).Trim():null;
            AccountNo =  (sleInvBSAccDet.ContainsKey("ACCOUNT_NO")) ?convert.ToString(sleInvBSAccDet["ACCOUNT_NO"]).Trim():null;
            IbanNo =  (sleInvBSAccDet.ContainsKey("IBAN_NO")) ?convert.ToString(sleInvBSAccDet["IBAN_NO"]).Trim():null;
            KvkNo = (sleInvBSAccDet.ContainsKey("KVK_NO")) ?convert.ToString(sleInvBSAccDet["KVK_NO"]).Trim():null;
            ByrOrgNo =  (sleInvBSAccDet.ContainsKey("BUYER_ORG_NO")) ?convert.ToString(sleInvBSAccDet["BUYER_ORG_NO"]).Trim():null;
            SuppOrgNo = (sleInvBSAccDet.ContainsKey("SUPPLIER_ORG_NO")) ? convert.ToString(sleInvBSAccDet["SUPPLIER_ORG_NO"]).Trim():null;
            BankName = (sleInvBSAccDet.ContainsKey("BANK_NAME")) ? convert.ToString(sleInvBSAccDet["BANK_NAME"]).Trim() : null;
            PaymentMode =  (sleInvBSAccDet.ContainsKey("PAYMENT_MODE")) ?convert.ToString(sleInvBSAccDet["PAYMENT_MODE"]).Trim():null;
            sDefault = (sleInvBSAccDet.ContainsKey("SET_DEFAULT")) ? convert.ToString(convert.ToInt(sleInvBSAccDet["SET_DEFAULT"])).Trim():"0";
            if (SetAccountDetailsByCurrency(InvLinkId, Curr_ID, Curr_Code, SwiftNo, AccountName, AccountNo, IbanNo, ByrOrgNo, SuppOrgNo, BankName, PaymentMode, KvkNo, sDefault, BuyerCode, SupplierCode, Hostname))
            {
                _res = "1";
            }
            else
            {
                _res = "0";
                throw new Exception("Unable to save Account Details");
            }
            return _res;
        }

        public bool SetAccountCurrencyDetails_(int LinkID, string Curr_ID, string Curr_Code, string SwiftNo, string AccountName, string AccountNo, string IbanNo,
            string ByrOrgNo, string SuppOrgNo, string BankName, string PaymentMode, string _BuyerCode, string _SupplierCode, string Hostname)
        {
            bool _result = false;
            string AuditValue = "";
            string sSql = "Select * from LES_INVOICE_BUYER_SUPPLIER_LINK where LINKID=@LINKID";
            object[] _obj1 = new object[2];
            string[] _param1 = new string[1];
            _obj1[0] = sSql;
            _param1[0] = "LINKID=" + LinkID;
            _obj1[1] = _param1;
            byte[] _byteData1 = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _obj1);
            DataSet _ds1 = _compressor.Decompress(_byteData1);
            if (_ds1 != null && _ds1.Tables.Count > 0 && _ds1.Tables[0].Rows.Count > 0)
            {
                string InvLinkId = convert.ToString(_ds1.Tables[0].Rows[0]["INVLINKID"]);
                string sSqlAcc = "Select * from LES_CURRENCY_ACCOUNT_DETAILS where INVLINKID=@INVLINKID";
                object[] _objAcc = new object[2];
                string[] _paramAcc = new string[1];
                _objAcc[0] = sSqlAcc;
                _paramAcc[0] = "INVLINKID=" + InvLinkId;
                _objAcc[1] = _paramAcc;
                byte[] _byteData2 = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _objAcc);
                DataSet _dsCurrAcc = _compressor.Decompress(_byteData2);
                int CURR_ACCOUNTID = -1;
                if (_dsCurrAcc != null && _dsCurrAcc.Tables.Count > 0 && _dsCurrAcc.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _dsCurrAcc.Tables[0].Rows)
                    {
                        if (convert.ToString(dr["CURRENCYID"]).Trim() == convert.ToString(Curr_ID).Trim())
                        {
                            CURR_ACCOUNTID = convert.ToInt(dr["CURR_ACCOUNTID"]);
                        }
                        string strInsertAcc = "UPDATE LES_CURRENCY_ACCOUNT_DETAILS SET SET_DEFAULT=@SET_DEFAULT Where CURR_ACCOUNTID = @CURR_ACCOUNTID";
                        object[] objAccService = new object[2];
                        string[] _paramAcc1 = new string[2];
                        _paramAcc1[0] = "SET_DEFAULT=0";
                        _paramAcc1[1] = "CURR_ACCOUNTID=" + convert.ToInt(dr["CURR_ACCOUNTID"]);
                        objAccService[0] = strInsertAcc;
                        objAccService[1] = _paramAcc1;
                        string rDat = (string)_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "ExecuteQueryParam", objAccService);
                    }
                }
                if (CURR_ACCOUNTID > 0)
                {
                    string strInsertAcc = "UPDATE LES_CURRENCY_ACCOUNT_DETAILS SET INVLINKID=@INVLINKID,CURRENCYID=@CURRENCYID,SWIFT_NO=@SWIFT_NO,ACCOUNT_NAME=@ACCOUNT_NAME,ACCOUNT_NO=@ACCOUNT_NO,IBAN_NO=@IBAN_NO,BUYER_ORG_NO=@BUYER_ORG_NO,SUPPLIER_ORG_NO=@SUPPLIER_ORG_NO,BANK_NAME=@BANK_NAME,PAYMENT_MODE=@PAYMENT_MODE,SET_DEFAULT=@SET_DEFAULT Where  CURR_ACCOUNTID = @CURR_ACCOUNTID";
                    object[] objAccService = new object[2];
                    string[] _paramAcc1 = new string[12];
                    _paramAcc1[0] = "INVLINKID=" + InvLinkId;
                    _paramAcc1[1] = "CURRENCYID=" + Curr_ID;
                    _paramAcc1[2] = "SWIFT_NO=" + SwiftNo;
                    _paramAcc1[3] = "ACCOUNT_NO=" + AccountNo;
                    _paramAcc1[4] = "IBAN_NO=" + IbanNo;
                    _paramAcc1[5] = "BUYER_ORG_NO=" + ByrOrgNo;
                    _paramAcc1[6] = "SUPPLIER_ORG_NO=" + SuppOrgNo;
                    _paramAcc1[7] = "BANK_NAME=" + BankName;
                    _paramAcc1[8] = "PAYMENT_MODE=" + PaymentMode;
                    _paramAcc1[9] = "SET_DEFAULT=" + 1;
                    _paramAcc1[10] = "CURR_ACCOUNTID=" + CURR_ACCOUNTID;
                    _paramAcc1[11] = "ACCOUNT_NAME=" + AccountName;
                    objAccService[0] = strInsertAcc;
                    objAccService[1] = _paramAcc1;
                    string rDat = (string)_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "ExecuteQueryParam", objAccService);
                    if (rDat == "1")
                    {
                        _result = true;
                        AuditValue = "Currency Account details update for Currency '" + Curr_Code + "' for Buyer Code : " + _BuyerCode + " and Supplier Code : " + _SupplierCode + " by : " + Hostname;
                        SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "");
                    }
                }
                else
                {
                    string strInsertAcc = "INSERT INTO LES_CURRENCY_ACCOUNT_DETAILS (INVLINKID,CURRENCYID,SWIFT_NO,ACCOUNT_NAME,ACCOUNT_NO,IBAN_NO,BUYER_ORG_NO,SUPPLIER_ORG_NO,BANK_NAME,PAYMENT_MODE,SET_DEFAULT) values (@INVLINKID,@CURRENCYID,@SWIFT_NO,@ACCOUNT_NAME,@ACCOUNT_NO,@IBAN_NO,@BUYER_ORG_NO,@SUPPLIER_ORG_NO,@BANK_NAME,@PAYMENT_MODE,@SET_DEFAULT)";
                    object[] objAccService = new object[2];
                    string[] _paramAcc1 = new string[11];
                    _paramAcc1[0] = "INVLINKID=" + InvLinkId;
                    _paramAcc1[1] = "CURRENCYID=" + Curr_ID;
                    _paramAcc1[2] = "SWIFT_NO=" + SwiftNo;
                    _paramAcc1[3] = "ACCOUNT_NO=" + AccountNo;
                    _paramAcc1[4] = "IBAN_NO=" + IbanNo;
                    _paramAcc1[5] = "BUYER_ORG_NO=" + ByrOrgNo;
                    _paramAcc1[6] = "SUPPLIER_ORG_NO=" + SuppOrgNo;
                    _paramAcc1[7] = "BANK_NAME=" + BankName;
                    _paramAcc1[8] = "PAYMENT_MODE=" + PaymentMode;
                    _paramAcc1[9] = "SET_DEFAULT=1";
                    _paramAcc1[10] = "ACCOUNT_NAME=" + AccountName;
                    objAccService[0] = strInsertAcc;
                    objAccService[1] = _paramAcc1;
                    string rDat = (string)_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "ExecuteQueryParam", objAccService);
                    if (rDat == "1")
                    {
                        _result = true;
                        AuditValue = "Currency Account details inserted for Currency '" + Curr_Code + "' for Buyer Code : " + _BuyerCode + " and Supplier Code : " + _SupplierCode + " by : " + Hostname;
                        SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "");
                    }
                }
            }
            return _result;
        }

        public bool SetAccountDetailsByCurrency(int INVLINKID, string Curr_ID,  string Curr_Code, string SwiftNo, string AccountName, string AccountNo, string IbanNo, string ByrOrgNo, string SuppOrgNo, 
            string BankName, string PaymentMode, string KVKNO, string sDefault, string _BuyerCode, string _SupplierCode,string HostName)
        {
            bool _result = false;
            string AuditValue = "";
            if (INVLINKID > 0)
            {
                string InvLinkId = convert.ToString(INVLINKID);
                string sSqlAcc = "SELECT * FROM LES_CURRENCY_ACCOUNT_DETAILS WHERE INVLINKID=@INVLINKID";
                object[] _objAcc = new object[2];
                string[] _paramAcc = new string[1];
                _objAcc[0] = sSqlAcc;
                _paramAcc[0] = "INVLINKID=" + InvLinkId;
                _objAcc[1] = _paramAcc;
                byte[] _byteData2 = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _objAcc);
                DataSet _dsCurrAcc = _compressor.Decompress(_byteData2);
                int CURR_ACCOUNTID = -1;
                if (_dsCurrAcc != null && _dsCurrAcc.Tables.Count > 0 && _dsCurrAcc.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _dsCurrAcc.Tables[0].Rows)
                    {
                        if (convert.ToString(dr["CURRENCYID"]).Trim() == convert.ToString(Curr_ID).Trim())
                        {
                            CURR_ACCOUNTID = convert.ToInt(dr["CURR_ACCOUNTID"]);
                        }
                        string strInsertAcc = "UPDATE LES_CURRENCY_ACCOUNT_DETAILS SET SET_DEFAULT=@SET_DEFAULT WHERE CURR_ACCOUNTID = @CURR_ACCOUNTID";
                        object[] objAccService = new object[2];
                        string[] _paramAcc1 = new string[2];
                        _paramAcc1[0] = "SET_DEFAULT=0";
                        _paramAcc1[1] = "CURR_ACCOUNTID=" + convert.ToInt(dr["CURR_ACCOUNTID"]);
                        objAccService[0] = strInsertAcc;
                        objAccService[1] = _paramAcc1;
                        string rDat = (string)_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "ExecuteQueryParam", objAccService);
                    }
                }
                if (CURR_ACCOUNTID > 0)
                {
                    string strInsertAcc = "UPDATE LES_CURRENCY_ACCOUNT_DETAILS SET INVLINKID=@INVLINKID,CURRENCYID=@CURRENCYID,SWIFT_NO=@SWIFT_NO,ACCOUNT_NAME=@ACCOUNT_NAME,ACCOUNT_NO=@ACCOUNT_NO,IBAN_NO=@IBAN_NO,BUYER_ORG_NO=@BUYER_ORG_NO,SUPPLIER_ORG_NO=@SUPPLIER_ORG_NO,BANK_NAME=@BANK_NAME,PAYMENT_MODE=@PAYMENT_MODE,SET_DEFAULT=@SET_DEFAULT,KVK_NO=@KVK_NO WHERE CURR_ACCOUNTID = @CURR_ACCOUNTID";
                    object[] objAccService = new object[2];
                    string[] _paramAcc1 = new string[13];
                    _paramAcc1[0] = "INVLINKID=" + InvLinkId;
                    _paramAcc1[1] = "CURRENCYID=" + Curr_ID;
                    _paramAcc1[2] = "SWIFT_NO=" + SwiftNo;
                    _paramAcc1[3] = "ACCOUNT_NO=" + AccountNo;
                    _paramAcc1[4] = "IBAN_NO=" + IbanNo;
                    _paramAcc1[5] = "BUYER_ORG_NO=" + ByrOrgNo;
                    _paramAcc1[6] = "SUPPLIER_ORG_NO=" + SuppOrgNo;
                    _paramAcc1[7] = "BANK_NAME=" + BankName;
                    _paramAcc1[8] = "PAYMENT_MODE=" + PaymentMode;
                    _paramAcc1[9] = "SET_DEFAULT=" + sDefault;
                    _paramAcc1[10] = "CURR_ACCOUNTID=" + CURR_ACCOUNTID;
                    _paramAcc1[11] = "KVK_NO=" + KVKNO;
                    _paramAcc1[12] = "ACCOUNT_NAME=" + AccountName;
                    objAccService[0] = strInsertAcc;
                    objAccService[1] = _paramAcc1;
                    string rDat = (string)_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "ExecuteQueryParam", objAccService);
                    if (rDat == "1")
                    {
                        _result = true;
                        AuditValue = "Currency Account details update for Currency '" + Curr_Code + "' for Buyer Code : " + _BuyerCode + " and Supplier Code : " + _SupplierCode + " by : " + HostName;
                        SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "");
                    }
                }
                else
                {
                    string strInsertAcc = "INSERT INTO LES_CURRENCY_ACCOUNT_DETAILS (INVLINKID,CURRENCYID,SWIFT_NO,ACCOUNT_NAME,ACCOUNT_NO,IBAN_NO,BUYER_ORG_NO,SUPPLIER_ORG_NO,BANK_NAME,PAYMENT_MODE,SET_DEFAULT,KVK_NO) values (@INVLINKID,@CURRENCYID,@SWIFT_NO,@ACCOUNT_NAME,@ACCOUNT_NO,@IBAN_NO,@BUYER_ORG_NO,@SUPPLIER_ORG_NO,@BANK_NAME,@PAYMENT_MODE,@SET_DEFAULT,@KVK_NO)";
                    object[] objAccService = new object[2];
                    string[] _paramAcc1 = new string[12];
                    _paramAcc1[0] = "INVLINKID=" + InvLinkId;
                    _paramAcc1[1] = "CURRENCYID=" + Curr_ID;
                    _paramAcc1[2] = "SWIFT_NO=" + SwiftNo;
                    _paramAcc1[3] = "ACCOUNT_NO=" + AccountNo;
                    _paramAcc1[4] = "IBAN_NO=" + IbanNo;
                    _paramAcc1[5] = "BUYER_ORG_NO=" + ByrOrgNo;
                    _paramAcc1[6] = "SUPPLIER_ORG_NO=" + SuppOrgNo;
                    _paramAcc1[7] = "BANK_NAME=" + BankName;
                    _paramAcc1[8] = "PAYMENT_MODE=" + PaymentMode;
                    _paramAcc1[9] = "SET_DEFAULT=" + sDefault;
                    _paramAcc1[10] = "KVK_NO=" + KVKNO;
                    _paramAcc1[11] = "ACCOUNT_NAME=" + AccountName;
                    objAccService[0] = strInsertAcc;
                    objAccService[1] = _paramAcc1;
                    string rDat = (string)_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "ExecuteQueryParam", objAccService);
                    if (rDat == "1")
                    {
                        _result = true;
                        AuditValue = "Currency Account details inserted for Currency '" + Curr_Code + "' for Buyer Code : " + _BuyerCode + " and Supplier Code : " + _SupplierCode + " by : " + HostName;
                        SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "");
                    }
                }
                if (sDefault == "1")
                {
                    string strSql = "UPDATE LES_INVOICE_BUYER_SUPPLIER_LINK SET CURRENCYID=@CURRENCYID ,CURR_CODE=@CURR_CODE, "
                      + "ACCOUNT_NAME=@ACCOUNT_NAME,ACCOUNT_NO=@ACCOUNT_NO , SWIFT_NO=@SWIFT_NO , IBAN_NO=@IBAN_NO , UPDATE_DATE = getdate(),SUPP_ORG_NO=@SUPP_ORG_NO,BYR_ORG_NO=@BYR_ORG_NO,PAYMENT_MODE=@PAYMENT_MODE,BANK_NAME=@BANK_NAME WHERE INVLINKID = @INVLINKID";
                    object[] objService = new object[2];
                    string[] _param = new string[11];
                    _param[0] = "INVLINKID=" + INVLINKID;
                    _param[1] = "CURRENCYID=" + convert.ToString(Curr_ID);
                    _param[2] = "CURR_CODE=" + convert.ToString(Curr_Code);
                    _param[3] = "ACCOUNT_NO=" + convert.ToString(AccountNo).Trim();
                    _param[4] = "SWIFT_NO=" + convert.ToString(SwiftNo).Trim();
                    _param[5] = "IBAN_NO=" + convert.ToString(IbanNo).Trim();
                    _param[6] = "SUPP_ORG_NO=" + convert.ToString(SuppOrgNo);
                    _param[7] = "BYR_ORG_NO=" + convert.ToString(ByrOrgNo).Trim();
                    _param[8] = "PAYMENT_MODE=" + convert.ToString(PaymentMode).Trim();
                    _param[9] = "BANK_NAME=" + convert.ToString(BankName).Trim();
                    _param[10] = "ACCOUNT_NAME=" + AccountName;
                    objService[0] = strSql;
                    objService[1] = _param;
                    string _DataVal = (string)_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "ExecuteQueryParam", objService);
                    if (_DataVal == "1") { }
                }
            }
            return _result;
        }

        public string Delete_InvCurrency_Account(int CURR_ACCOUNTID, string CURR_CODE, string Hostname)
        {
            string AuditValue = "";
            string sSqlAcc = "DELETE FROM LES_CURRENCY_ACCOUNT_DETAILS WHERE CURR_ACCOUNTID = @CURR_ACCOUNTID";
            object[] _obj1 = new object[2];
            string[] _param1 = new string[1];
            _obj1[0] = sSqlAcc;
            _param1[0] = "CURR_ACCOUNTID=" + CURR_ACCOUNTID;
            _obj1[1] = _param1;
            string rDat = (string)_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "ExecuteQueryParam", _obj1);
            if (rDat == "1")
            {
                AuditValue = "Currency Account details deleted for Currency '" + CURR_CODE + " by : " + Hostname;
                SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "");
            }

            return rDat;
        }

        #endregion

        #region /* eInvoice PDF Config */

        public DataSet Get_INV_PDF_Mapping()
        {
            DataSet ds = null;
            object[] _obj = new object[1];
            string cSql = "SELECT * FROM SMV_INV_PDF_MAPPING_LINK";
            _obj[0] = cSql;
            byte[] _byteData = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryData", _obj);
            ds = _compressor.Decompress(_byteData);
            DataSet _ds = SmAddress.GetAllAddress();

            var d1 = ds.Tables[0].AsEnumerable();
            var d2 = _ds.Tables[0].AsEnumerable();

            var joinResult = from table1 in ds.Tables[0].AsEnumerable()
                             join table2 in _ds.Tables[0].AsEnumerable() on convert.ToString(table1["SUPPLIERID"]) equals convert.ToString(table2["ADDRESSID"]) into Temp
                             from x in Temp.DefaultIfEmpty()
                             select new
                             {
                                 MAP_ID = convert.ToString(table1["MAP_ID"]),
                                 INV_PDF_MAPID = convert.ToString(table1["INV_PDF_MAPID"]),
                                 MAPPING_1 = convert.ToString(table1["MAPPING_1"]),
                                 MAPPING_1_VALUE = convert.ToString(table1["MAPPING_1_VALUE"]),
                                 MAPPING_2 = convert.ToString(table1["MAPPING_2"]),
                                 MAPPING_2_VALUE = convert.ToString(table1["MAPPING_2_VALUE"]),
                                 MAPPING_3 = convert.ToString(table1["MAPPING_3"]),
                                 MAPPING_3_VALUE = convert.ToString(table1["MAPPING_3_VALUE"]),
                                 INV_MAP_CODE = convert.ToString(table1["INV_MAP_CODE"]),
                                 SUPPLIERID = convert.ToString(table1["SUPPLIERID"]),
                                 E_SUPPLIER_CODE = convert.ToString(x["ADDR_CODE"]),
                                 E_SUPPLIER_NAME = convert.ToString(x["ADDR_NAME"])
                             };

            DataTable dataT = new DataTable();
            dataT = joinResult.ConvertToDataTable(item => item.MAP_ID,item => item.INV_PDF_MAPID,item => item.MAPPING_1,item => item.MAPPING_1_VALUE,item => item.MAPPING_2,item => item.MAPPING_2_VALUE,item => item.MAPPING_3,
                                        item => item.MAPPING_3_VALUE,item => item.INV_MAP_CODE,item => item.SUPPLIERID,item => item.E_SUPPLIER_CODE, item => item.E_SUPPLIER_NAME
                                        );
            DataSet LeSInvoiceMap = new DataSet();
            LeSInvoiceMap.Tables.Add(dataT);
            return LeSInvoiceMap;
        }

        public DataSet Get_INV_PDF_MAPCODE()
        {
            DataSet _ds = null;
            object[] _obj = new object[1];
            string cSql = "SELECT DISTINCT INV_MAP_CODE,INV_PDF_MAPID FROM SMV_PDF_MAPPING_LINK";
            _obj[0] = cSql;
            byte[] _byteData = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryData", _obj);
            _ds = _compressor.Decompress(_byteData);
            return _ds;
        }

        public DataSet Get_eInvoice_Supplier(string eInvoiceURL)
        {
            SupplierRoutines _Routine = new SupplierRoutines();
            DataSetCompressor _compressor = new DataSetCompressor();
            ServiceCallRoutines _WebServiceCall = new ServiceCallRoutines();
            DataSet _ds = new DataSet();
            DataSet _DSeSupplier = GetAllDefaultSuppliers();
            object[] _obj = new object[1];
            string cSql = "SELECT * FROM LES_INVOICE_BUYER_SUPPLIER_LINK";
            _obj[0] = cSql;
            byte[] _byteData = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryData", _obj);
            DataSet _DSeInvLink = _compressor.Decompress(_byteData);
            var joinResult = from table1 in _DSeInvLink.Tables[0].AsEnumerable()
                             join table2 in _DSeSupplier.Tables[0].AsEnumerable() on convert.ToString(table1["SUPPLIERID"]) equals convert.ToString(table2["ADDRESSID"]) into Temp
                             from x in Temp
                             select new
                             {
                                 ADDRESSID = (x != null && x["ADDRESSID"] != DBNull.Value) ? convert.ToString(x["ADDRESSID"]).Trim() : "",
                                 ADDR_CODE = convert.ToString(x["ADDR_CODE"]),
                                 ADDR_NAME = convert.ToString(x["ADDR_CODE"]) + " : " + convert.ToString(x["ADDR_NAME"])
                             };

            DataTable dataT = new DataTable();
            dataT = joinResult.ConvertToDataTable(item => item.ADDRESSID, item => item.ADDR_CODE, item => item.ADDR_NAME);
            string[] _parm = new string[3];
            _parm[0] = "ADDRESSID";
            _parm[1] = "ADDR_CODE";
            _parm[2] = "ADDR_NAME";
            DataTable newDt = dataT.DefaultView.ToTable(true, _parm);
            _ds.Tables.Add(newDt);
            return _ds;
        }

        public DataSet Get_eInvoice_Buyer(int SupplierId)
        {
            SupplierRoutines _Routine = new SupplierRoutines();
            DataSetCompressor _compressor = new DataSetCompressor();
            ServiceCallRoutines _WebServiceCall = new ServiceCallRoutines();
            DataSet _ds = new DataSet();
            DataSet _DSeBuyer = GetAllDefaultBuyers();
            object[] _obj = new object[2];
            string cSql = "SELECT * FROM LES_INVOICE_BUYER_SUPPLIER_LINK WHERE SUPPLIERID = @SUPPLIERID";
            string[] _objParm = new string[1];
            _objParm[0] = "SUPPLIERID=" + SupplierId;
            _obj[0] = cSql;
            _obj[1] = _objParm;
            byte[] _byteData = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _obj);
            DataSet _DSeInvLink = _compressor.Decompress(_byteData);

            var joinResult = from table1 in _DSeBuyer.Tables[0].AsEnumerable()
                             join table2 in _DSeInvLink.Tables[0].AsEnumerable() on convert.ToString(table1["ADDRESSID"]) equals convert.ToString(table2["BUYERID"]) into Temp
                             from x in Temp
                             select new
                             {
                                 ADDRESSID = (x != null && x["BUYERID"] != DBNull.Value) ? convert.ToString(x["BUYERID"]).Trim() : "",
                                 ADDR_CODE = convert.ToString(table1["ADDR_CODE"]),
                                 ADDR_NAME = convert.ToString(table1["ADDR_CODE"]) + " : " + convert.ToString(table1["ADDR_NAME"])
                             };
            DataTable dataT = new DataTable();
            dataT = joinResult.ConvertToDataTable(item => item.ADDRESSID, item => item.ADDR_CODE, item => item.ADDR_NAME);
            _ds.Tables.Add(dataT);
            return _ds;
        }

        public DataSet Get_INV_PDF_MAPCODE_By_SupplierId(int SupplierId)
        {
            DataSet _ds = null;
            object[] _obj = new object[2];
            string[] _objparm = new string[1];
            string cSql = "SELECT DISTINCT INV_MAP_CODE,INV_PDF_MAPID FROM SMV_PDF_MAPPING_LINK WHERE SUPPLIERID =@SupplierId ORDER BY INV_MAP_CODE asc";
            _objparm[0] = "SupplierId=" + SupplierId;
            _obj[0] = cSql;
            _obj[1] = _objparm;
            byte[] _byteData = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _obj);
            _ds = _compressor.Decompress(_byteData);
            return _ds;
        }

        public string GetInvoiceMapCode_Supplierid(int SupplierId)
        {
            string mapCode = "";
            System.Data.DataSet _ds = Get_INV_PDF_MAPCODE_By_SupplierId(SupplierId);
            SmAddress _addr = SmAddress.Load(SupplierId);
            if (_addr != null)
            {
                if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = _ds.Tables[0].NewRow();
                    mapCode = "MAP_" + _addr.AddrCode + "_" + (_ds.Tables[0].Rows.Count + 1); ;
                }
                else
                {
                    DataRow dr = _ds.Tables[0].NewRow();
                    mapCode = "MAP_" + _addr.AddrCode + "_" + (_ds.Tables[0].Rows.Count + 1);
                }
            }
            return mapCode;
        }

        public bool Check_Inv_MapCode(string MapCode)
        {
            bool _result = false;
            DataSet _ds = null;
            object[] _obj = new object[2];
            string[] _objparm = new string[1];
            string cSql = "SELECT INV_MAP_CODE,INV_PDF_MAPID FROM SMV_PDF_MAPPING_LINK WHERE INV_MAP_CODE =@INV_MAP_CODE";
            _objparm[0] = "INV_MAP_CODE=" + MapCode;
            _obj[0] = cSql;
            _obj[1] = _objparm;
            byte[] _byteData = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _obj);
            _ds = _compressor.Decompress(_byteData);
            if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
            {
                _result = true;
            }
            return _result;
        }

        public string Save_Invoice_PDF_Config(Dictionary<string,string> sleInvMappDet, string Hostname)
        {
            SmAddress _SuppAddr = SmAddress.Load(Convert.ToInt32(sleInvMappDet["SUPPLIERID"]));
            string cMapId = (sleInvMappDet.ContainsKey("MAP_ID")) ? sleInvMappDet["MAP_ID"].Trim().ToUpper() : "0"; 
            string cInvMapCode = (sleInvMappDet.ContainsKey("INV_MAP_CODE")) ? sleInvMappDet["INV_MAP_CODE"].Trim().ToUpper() : null; 
            object[] _objService = new object[8];
            _objService[0] = Convert.ToInt32(sleInvMappDet["SUPPLIERID"]);
            _objService[1] = cInvMapCode;
            _objService[2] = (sleInvMappDet.ContainsKey("MAPPING_1")) ?sleInvMappDet["MAPPING_1"].Trim() : null; 
            _objService[3] = (sleInvMappDet.ContainsKey("MAPPING_1_VALUE")) ?sleInvMappDet["MAPPING_1_VALUE"].Trim() : null; 
            _objService[4] = (sleInvMappDet.ContainsKey("MAPPING_2")) ?sleInvMappDet["MAPPING_2"].Trim() : null; 
            _objService[5] = (sleInvMappDet.ContainsKey("MAPPING_2_VALUE")) ?sleInvMappDet["MAPPING_2_VALUE"].Trim() : null; 
            _objService[6] = (sleInvMappDet.ContainsKey("MAPPING_3")) ?sleInvMappDet["MAPPING_3"].Trim() : null;
            _objService[7] = (sleInvMappDet.ContainsKey("MAPPING_3_VALUE")) ? sleInvMappDet["MAPPING_3_VALUE"].Trim() : null; 
            string _DataVal = (string)_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "Save_SM_INV_PDF_BUYER_SUPPLIER_LINK", _objService);
            if (_DataVal == "")
            {
                if (Convert.ToInt32(cMapId) == 0)
                {
                    SetAuditLog("LesMonitor", "INV_MAP_CODE '" + cInvMapCode + "' inserted for (" + _SuppAddr.AddrCode + ") by '" + Hostname + "' ", "Updated", "", "", "",  Convert.ToString(sleInvMappDet["SUPPLIERID"]));
                }
                else if (Convert.ToInt32(cMapId) > 0)
                {
                    SetAuditLog("LesMonitor", "INV_MAP_CODE '" + cInvMapCode + "' updated for (" + _SuppAddr.AddrCode + ") by '" + Hostname + "' ", "Updated", "", "", "", Convert.ToString(sleInvMappDet["SUPPLIERID"]));
                }
            }
            else
            {
                throw new Exception(_DataVal);
            }
            return _DataVal;
        }

        public string Delete_Invoice_PDF_Config(int MAPID, int SupplierAddrId, string Inv_Map_Code, string Hostname)
        {
            SmAddress _SuppAddr = SmAddress.Load(SupplierAddrId);
            object[] _objService = new object[1];
            _objService[0] = MAPID;
            string _DataVal = (string)_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "Delete_SM_INV_PDF_BUYER_SUPPLIER_LINK", _objService);
            if (_DataVal == "")
            {
                SetAuditLog("LesMonitor", "INV_MAP_CODE '" + Inv_Map_Code + "' deleted for (" + _SuppAddr.AddrCode + ") by '" + Hostname + "' ", "Updated", "", "", "", SupplierAddrId.ToString());
            }
            else
            {
                throw new Exception(_DataVal);
            }
            return _DataVal;
        }

        #endregion

        #region Address Config Settings

        public Dictionary<string, string> GetAddressConfigDetails(string FORMAT, int ID)
        {
            Dictionary<string, string> sldet = new Dictionary<string, string>(); sldet.Clear();
            SmAddressConfig _obj = SmAddressConfig.LoadID_Format(FORMAT, ID);
            if (_obj != null)
            {
                sldet.Add("ADDRCONFIGID", Convert.ToString(_obj.Addrconfigid));
                sldet.Add("PARTY_MAPPING", Convert.ToString(_obj.PartyMapping));
                sldet.Add("DEFAULT_FORMAT", Convert.ToString(_obj.DefaultFormat));
                sldet.Add("EXPORT_PATH", Convert.ToString(_obj.ExportPath));
                sldet.Add("IMPORT_PATH", Convert.ToString(_obj.ImportPath));
                sldet.Add("IMPORT_RFQ", Convert.ToString(_obj.ImportRfq));
                sldet.Add("EXPORT_RFQ", Convert.ToString(_obj.ExportRfq));
                sldet.Add("EXPORT_RFQ_ACK", Convert.ToString(_obj.ExportRfqAck));
                sldet.Add("IMPORT_QUOTE", Convert.ToString(_obj.ImportQuote));
                sldet.Add("EXPORT_QUOTE", Convert.ToString(_obj.ExportQuote));
                sldet.Add("IMPORT_PO", Convert.ToString(_obj.ImportPo));
                sldet.Add("EXPORT_PO", Convert.ToString(_obj.ExportPo));
                sldet.Add("EXPORT_PO_ACK", Convert.ToString(_obj.ExportPoAck));
                sldet.Add("EXPORT_POC", Convert.ToString(_obj.ExportPoc));
                sldet.Add("MAIL_NOTIFY", Convert.ToString(_obj.MailNotify));                
                sldet.Add("DEFAULT_PRICE", Convert.ToString(_obj.DefaultPrice));
                sldet.Add("UPLOAD_FILE_TYPE", Convert.ToString(_obj.UploadFileType));
                sldet.Add("MAIL_SUBJECT", Convert.ToString(_obj.MailSubject));
                sldet.Add("SUP_WEB_SERVICE_URL", Convert.ToString(_obj.SupWebServiceUrl));
                sldet.Add("CC_EMAIL", Convert.ToString(_obj.CcEmail));
                sldet.Add("IMPORT_POC", Convert.ToString(_obj.ImportPoc));//ADDED by kalpita on 26/12/2017
            }
            return sldet;
        }

        //added by kalpita on 21/11/2017
        public System.Data.DataSet GetGroupFormat(string cAddrType)
        {
            return SmBuyerSupplierGroups.Get_GroupFormat_AddrType(cAddrType);
        }

        public void SaveAddressConfig(string cAddrType, Dictionary<string, string> slDet, Dal.DataAccess _dataAccess,string Id)
        {
            string AuditValue = ""; Int16 _noval = 0;          
            try
            {
                SmAddress _addrobj = (!string.IsNullOrEmpty(Id)) ? SmAddress.Load_ByID(Convert.ToInt32(Id), _dataAccess) : null;
                SmAddressConfig _obj = new SmAddressConfig();
                _obj.Addrconfigid = Convert.ToInt32(slDet["ADDRESSCONFIGID"]);
                _obj.Load_ID(_dataAccess);
                _obj.SmAddress = (slDet.ContainsKey("ADDRESSID")) ? SmAddress.Load_ByID(Convert.ToInt32(slDet["ADDRESSID"]), _dataAccess) : _addrobj;
                _obj.DefaultFormat = (slDet.ContainsKey("DEFAULT_FORMAT")) ? slDet["DEFAULT_FORMAT"] : null;
                if (cAddrType.ToUpper() == "BUYER")
                {
                    _obj.PartyMapping = (slDet.ContainsKey("MAPPING")) ? slDet["MAPPING"] : null;
                    _obj.ExportPath = (slDet.ContainsKey("EXPORT_PATH")) ? slDet["EXPORT_PATH"] : null;
                    _obj.ImportPath = (slDet.ContainsKey("IMPORT_PATH")) ? slDet["IMPORT_PATH"] : null;
                    _obj.MailNotify = (slDet.ContainsKey("NOTIFY_BUYER")) ? Convert.ToInt16(slDet["NOTIFY_BUYER"]) : _noval;
                    _obj.IdentificationCode = (slDet.ContainsKey("BYR_SENDER_CODE")) ? slDet["BYR_SENDER_CODE"] : _obj.SmAddress.AddrCode;
                }
                else if (cAddrType.ToUpper() == "SUPPLIER")
                {
                    _obj.PartyMapping = (slDet.ContainsKey("MAPPING")) ? slDet["MAPPING"] : null;
                    _obj.ExportPath = (slDet.ContainsKey("EXPORT_PATH")) ? slDet["EXPORT_PATH"] : null;
                    _obj.ImportPath = (slDet.ContainsKey("IMPORT_PATH")) ? slDet["IMPORT_PATH"] : null;
                    _obj.MailNotify = (slDet.ContainsKey("NOTIFY_SUPPLR")) ? Convert.ToInt16(slDet["NOTIFY_SUPPLR"]) : _noval;
                    _obj.IdentificationCode = (slDet.ContainsKey("SUPP_RECEIVER_CODE")) ? slDet["SUPP_RECEIVER_CODE"] : _obj.SmAddress.AddrCode;
                }
                _obj.DefaultPrice = (slDet.ContainsKey("DEFAULT_PRICE")) ? Convert.ToSingle(slDet["DEFAULT_PRICE"]) : 0;
                _obj.UploadFileType = (slDet.ContainsKey("UPLOAD_FILE_TYPE")) ? slDet["UPLOAD_FILE_TYPE"] : null;
                if (slDet.ContainsKey("MAIL_SUBJECT"))
                {
                    _obj.MailSubject = (slDet["MAIL_SUBJECT"].Contains("_")) ? slDet["MAIL_SUBJECT"].Replace("_", "#") : slDet["MAIL_SUBJECT"];
                }
                else { _obj.MailSubject = null; }

                _obj.SupWebServiceUrl = (slDet.ContainsKey("SUP_WEB_SERVICE_URL")) ? slDet["SUP_WEB_SERVICE_URL"] : null;
                _obj.CcEmail = (slDet.ContainsKey("CC_EMAIL")) ? slDet["CC_EMAIL"] : null;
                _obj.ImportRfq = (slDet.ContainsKey("IMPORT_RFQ")) ? Convert.ToInt16(slDet["IMPORT_RFQ"]) : _noval;
                _obj.ExportRfq = (slDet.ContainsKey("EXPORT_RFQ")) ? Convert.ToInt16(slDet["EXPORT_RFQ"]) : _noval;
                _obj.ExportRfqAck = (slDet.ContainsKey("EXPORT_RFQ_ACK")) ? Convert.ToInt16(slDet["EXPORT_RFQ_ACK"]) : _noval;
                _obj.ImportQuote = (slDet.ContainsKey("IMPORT_QUOTE")) ? Convert.ToInt16(slDet["IMPORT_QUOTE"]) : _noval;
                _obj.ExportQuote = (slDet.ContainsKey("EXPORT_QUOTE")) ? Convert.ToInt16(slDet["EXPORT_QUOTE"]) : _noval;
                _obj.ImportPo = (slDet.ContainsKey("IMPORT_PO")) ? Convert.ToInt16(slDet["IMPORT_PO"]) : _noval;
                _obj.ExportPo = (slDet.ContainsKey("EXPORT_PO")) ? Convert.ToInt16(slDet["EXPORT_PO"]) : _noval;
                _obj.ExportPoAck = (slDet.ContainsKey("EXPORT_PO_ACK")) ? Convert.ToInt16(slDet["EXPORT_PO_ACK"]) : _noval;
                _obj.ExportPoc = (slDet.ContainsKey("EXPORT_POC")) ? Convert.ToInt16(slDet["EXPORT_POC"]) : _noval;
                _obj.ImportPoc = (slDet.ContainsKey("IMPORT_POC")) ? Convert.ToInt16(slDet["IMPORT_POC"]) : _noval;
                _obj.UpdateDate = DateTime.Now;
                if (_obj.Addrconfigid > 0) { _obj.Update(_dataAccess); }
                else { _obj.CreatedDate = DateTime.Now; _obj.Insert(_dataAccess); }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string CheckExistingConfig(string Format, int ADDRESSID)
        {
            SmAddressConfig _obj = new SmAddressConfig();
            return _obj.CheckExistingConfig(Format, ADDRESSID);
        }

        public string GetFormat_By_Addressid(int ADDRESSID)
        {
            string _format="";
            DataSet ds = SmAddressConfig.GetConfig_By_ADDRESSID(ADDRESSID);
            if(ds!=null && ds.Tables.Count >0  && ds.Tables[0].Rows.Count >0 )
            {
                _format = Convert.ToString(ds.Tables[0].Rows[0]["DEFAULT_FORMAT"]);
            }
            return _format;
        }

        #endregion

        #region Document Format & Rules

        /* Default Format*/
        public System.Data.DataSet GetAllDocumentFormats()
        {
            return SmDocumentFormats.GetAllDocumentFormats();
        }

        public System.Data.DataSet GetDocumentFormat_Addrtype(string cAddrType)
        {
            return SmDocumentFormats.Get_DocumentFormat_AddrType(cAddrType);
        }

        public Dictionary<string, string> GetDocumentFormatDetails(string ID)
        {
            Dictionary<string, string> slDet = new Dictionary<string, string>(); slDet.Clear();
            SmDocumentFormats _obj = new SmDocumentFormats();
            _obj.Docformatid = Convert.ToInt32(ID);
            _obj.Load();
            slDet.Add("DEFAULT_FORMAT", _obj.DocumentFormat);
            slDet.Add("IMPORT_PATH", _obj.ImportPath);
            slDet.Add("EXPORT_PATH", _obj.ExportPath);
            return slDet;
        }

        public void SaveDocumentFormat(Dictionary<string, string> slDefformat)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                SmDocumentFormats _obj = new SmDocumentFormats();
                _obj.Docformatid = Convert.ToInt32(slDefformat["DOCFORMATID"]);
                _obj.Load();
                _obj.DocumentFormat = (slDefformat.ContainsKey("DOCUMENT_FORMAT")) ? slDefformat["DOCUMENT_FORMAT"] : null;
                _obj.AddrType = (slDefformat.ContainsKey("ADDR_TYPE")) ? slDefformat["ADDR_TYPE"] : null;
                _obj.ImportPath = (slDefformat.ContainsKey("IMPORT_PATH")) ? slDefformat["IMPORT_PATH"] : null;
                _obj.ExportPath = (slDefformat.ContainsKey("EXPORT_PATH")) ? slDefformat["EXPORT_PATH"] : null;
                _dataAccess.BeginTransaction();
                _obj.UpdatedDate = DateTime.Now;
                if (_obj.Docformatid > 0) { _obj.Update(_dataAccess); }
                else { _obj.CreatedDate = DateTime.Now; _obj.Insert(_dataAccess); }
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public string CheckExistingDocumentFormat(string Format)
        {
            return SmDocumentFormats.CheckExistingDocFormat(Format);
        }
        /* end*/

        /* Default Format Rules*/
        public System.Data.DataSet GetDocumentFormatRules_DocformatId(string DOCFORMATID)
        {
            return SmDocumentformatRules.Get_DocumentformatRules_By_DocFormatID(Convert.ToInt32(DOCFORMATID));
        }

        public System.Data.DataSet Display_AllDocumentFormatRules()
        {
            return SmDocumentformatRules.GetAllDocumentFormatRules();
        }

        public void SaveDocumentFormatRules(Dictionary<string, string> slDefformatRuledet, string UserHostAddress)
        {
            string cRuleCode = "", cDefaultFormat = "";
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                SmDocumentformatRules _obj = new SmDocumentformatRules();
                _obj.DocumentformatRuleid = (slDefformatRuledet.ContainsKey("DOCUMENTFORMAT_RULEID")) ? Convert.ToInt32(slDefformatRuledet["DOCUMENTFORMAT_RULEID"]) : 0;
                _obj.Load();
                if (slDefformatRuledet.ContainsKey("DOCFORMATID")) { _obj.Docformatid = Convert.ToInt32(slDefformatRuledet["DOCFORMATID"]); }
                if (slDefformatRuledet.ContainsKey("RULEID")) { _obj.RuleId = Convert.ToInt32(slDefformatRuledet["RULEID"]); }
                _obj.RuleValue = (slDefformatRuledet.ContainsKey("RULE_VALUE")) ? slDefformatRuledet["RULE_VALUE"] : "NOT SET";
                cDefaultFormat = slDefformatRuledet["DOCUMENT_FORMAT"]; cRuleCode = slDefformatRuledet["RULE_CODE"];

                _dataAccess.BeginTransaction();
                _obj.UpdateDate = DateTime.Now;
                if (_obj.DocumentformatRuleid > 0) { _obj.Update(_dataAccess); }
                else { _obj.CreatedDate = DateTime.Now; _obj.Insert(_dataAccess); }

                string AuditValue = "DocumentFormat Rule (" + cRuleCode + ") of " + cDefaultFormat + " saved by [" + UserHostAddress + "]";
                SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "", _dataAccess);
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void DeleteDocumentFormatRules(string DOCFORMAT_RULEID ,string UserHostAddress)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                if (!string.IsNullOrEmpty(DOCFORMAT_RULEID))
                {
                    SmDocumentformatRules _obj = new SmDocumentformatRules(); _obj.DocumentformatRuleid = Convert.ToInt32(DOCFORMAT_RULEID); _obj.Load();
                    SmDocumentFormats _dfmtobj = new SmDocumentFormats(); _dfmtobj.Docformatid = _obj.Docformatid; _dfmtobj.Load();
                    SmEsupplierRules _eSppRobj = new SmEsupplierRules(); _eSppRobj.Ruleid = _obj.RuleId; _eSppRobj.Load();
                    _dataAccess.BeginTransaction();
                    _obj.Delete(_dataAccess);
                    string AuditValue = "Document Format Rule (" + _eSppRobj.RuleCode + ") of " + _dfmtobj.DocumentFormat + " deleted by [" + UserHostAddress + "]";
                    SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", "", _dataAccess);
                    _dataAccess.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }
        /* end*/

        #endregion

        #region Wizard

        public string SaveAddress_Wizard(string AddrType, Dictionary<string, string> slAddrDet, List<string[]> slRuleDet, string HostServer)
        {     
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            string AuditValue = "";
            try
            {
                SmAddress _address = new SmAddress();
                int AddressID = Convert.ToInt32(slAddrDet["ID"]);
                if (AddressID > 0)
                {
                    _address = SmAddress.Load(AddressID);
                }
                _address.AddrCode = (slAddrDet.ContainsKey("ADDR_CODE")) ? slAddrDet["ADDR_CODE"].Trim().ToUpper() : null;
                _address.AddrName = (slAddrDet.ContainsKey("ADDR_NAME")) ? slAddrDet["ADDR_NAME"].Trim() : null;
                _address.ContactPerson = (slAddrDet.ContainsKey("CONTACT_PERSON")) ? slAddrDet["CONTACT_PERSON"].Trim() : null;
                _address.AddrEmail = (slAddrDet.ContainsKey("ADDR_EMAIL")) ? slAddrDet["ADDR_EMAIL"].Trim() : null;
                _address.AddrCountry = (slAddrDet.ContainsKey("ADDR_COUNTRY")) ? slAddrDet["ADDR_COUNTRY"].Trim() : null;
                _address.AddrInbox = (slAddrDet.ContainsKey("ADDR_INBOX")) ? slAddrDet["ADDR_INBOX"].Trim() : null;
                _address.AddrOutbox = (slAddrDet.ContainsKey("ADDR_OUTBOX")) ? slAddrDet["ADDR_OUTBOX"].Trim() : null;
                _address.WebLink = (slAddrDet.ContainsKey("WEBLINK")) ? slAddrDet["WEBLINK"].Trim() : null;
                _address.AddrCity = (slAddrDet.ContainsKey("ADDR_CITY")) ? slAddrDet["ADDR_CITY"].Trim() : null;
                _address.AddrZipcode = (slAddrDet.ContainsKey("ADDR_ZIPCODE")) ? slAddrDet["ADDR_ZIPCODE"].Trim() : null;
                _address.Islesconnect = (slAddrDet.ContainsKey("ISLESCONNECT")) ? Convert.ToInt32(slAddrDet["ISLESCONNECT"].Trim()) : 0;
                _dataAccess.BeginTransaction();
                if (_address.Addressid > 0)
                {
                    _address.Update(_dataAccess);
                    AuditValue = AddrType + " (" + slAddrDet["ADDR_CODE"] + ")' details updated by [" + HostServer + "].";
                }
                else
                {
                    _address.Addressid = GetNextKey("ADDRESSID", "SM_ADDRESS", _dataAccess);
                    _address.Active = 1;
                    _address.AddrType = AddrType.Trim();
                    _address.Esupplier = 1;
                    _address.Einvoice = 0;
                    _address.Epurchase = 0;
                    _address.CreatedDate = DateTime.Now;
                    _address.AddrCurrencyid = 0;
                    _address.XmlAddrNo = 0;
                    _address.Insert(_dataAccess);
                    AuditValue = "New " + AddrType + " (" + slAddrDet["ADDR_CODE"] + ") inserted by [" + HostServer + "].";
                }
                if (AddrType.ToUpper() == "BUYER") { SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", Convert.ToString(_address.Addressid), "", _dataAccess); }
                else if (AddrType.ToUpper() == "SUPPLIER") { SetAuditLog("LesMonitor", AuditValue, "Updated", "", "", "", Convert.ToString(_address.Addressid), _dataAccess); }

                #region insert into config table
                if (AddressID == 0)
                {
                    SaveAddressConfig(AddrType, slAddrDet, _dataAccess, Convert.ToString(_address.Addressid));
                }
                #endregion

                #region insert into default rules table
                if (AddressID == 0)
                {
                    if (slRuleDet != null)
                    {
                        for (int i = 0; i < slRuleDet.Count; i++)
                        {
                            string cRuleid = slRuleDet[i][1].Split('|')[1]; string cRuleval = slRuleDet[i][3].Split('|')[1];
                            SaveDefaultRule(0, _address.Addressid.Value, slAddrDet["DEFAULT_FORMAT"], Convert.ToInt32(cRuleid), cRuleval, HostServer, _dataAccess);
                        }
                    }
                }
                #endregion
                _dataAccess.CommitTransaction();
                return Convert.ToString(_address.Addressid.Value);
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                return "";
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void CopyPDFMappingDetails(Dictionary<string,string> slMappingdet)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {                     
                SmPdfBuyerLink _pblnkobj = SmPdfBuyerLink.LoadByPDFMapId(Convert.ToInt32(slMappingdet["PDFMAPID"]));            
                _dataAccess.BeginTransaction();           
                if (_pblnkobj != null)
                {
                    string cFormatMapCode = SmPdfBuyerLink.GetNextFormatMapCode(Convert.ToString(slMappingdet["DOCTYPE"]), Convert.ToString(slMappingdet["FORMAT_MAPCODE"]), _dataAccess);
                    SmPdfBuyerLink _pdflnk = _pblnkobj;
                    _pdflnk.MapId = GetNextKey("MAP_ID", "SM_PDF_BUYER_LINK", _dataAccess);
                    _pdflnk.Buyerid = Convert.ToInt32(slMappingdet["BUYERID"]);
                    _pdflnk.Supplierid = null;
                    _pdflnk.BuyerSuppLinkid = null;
                    _pdflnk.DocType = (!string.IsNullOrEmpty(_pblnkobj.DocType)) ? _pblnkobj.DocType : "RequestForQuote";
                    _pdflnk.FormatMapCode = cFormatMapCode;
                    _pdflnk.Insert(_dataAccess);
                }
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        public void CopyExcelMappingDetails(Dictionary<string, string> slMappingdet)
        {
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                #region Existing excelmapping
                SmXlsGroupMapping _eMapobj = SmXlsGroupMapping.Load(Convert.ToInt32(slMappingdet["EXCEL_MAPID"]));
                SmXlsBuyerLink _eblnk = SmXlsBuyerLink.Load(Convert.ToInt32(slMappingdet["XLS_BUYER_MAPID"]));             
                #endregion
                _dataAccess.BeginTransaction();
                if (_eblnk != null)
                {
                    int xByrMapid = GetNextKey("XLS_BUYER_MAPID", "SM_XLS_BUYER_LINK", _dataAccess);
                    string cFormatMapCode = SmXlsBuyerLink.GetNextFormatMapCode(Convert.ToString(slMappingdet["DOCTYPE"]), Convert.ToString(slMappingdet["FORMAT_MAPCODE"]), _dataAccess);
                    SmXlsBuyerLink _xblnkobj = _eblnk;
                    _xblnkobj.XlsBuyerMapid = xByrMapid;
                    _xblnkobj.Buyerid = Convert.ToInt32(slMappingdet["BUYERID"]);
                    _xblnkobj.Supplierid = null;
                    _xblnkobj.BuyerSuppLinkid = null;
                    _xblnkobj.SmXlsGroupMapping = _eMapobj;
                    _xblnkobj.DocType = (!string.IsNullOrEmpty(_eblnk.DocType)) ? _eblnk.DocType : "RequestForQuote";
                    _xblnkobj.FormatMapCode = cFormatMapCode;
                    _xblnkobj.Insert(_dataAccess);
                }
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
            }
            finally
            {
                _dataAccess._Dispose();
            }
        }

        #region Server Sync

        public string GetBuyerServiceNames()
        {
            string _servdet = "", _servname = "";
            SmServerSyncCollection _servCollct = SmServerSync.GetAll();
            if (_servCollct != null && _servCollct.Count > 0)
            {
                foreach (SmServerSync _objserv in _servCollct)
                {
                    _servname += _objserv.ServerName.Trim() + ",";
                }
                _servdet = _servname.TrimEnd(',');
            }
            return _servdet;
        }

        public string SaveServerdetails(Dictionary<string, string> slServdet)
        {
            string _result = "";
            Dal.DataAccess _dataAccess = new Dal.DataAccess();
            try
            {
                SmServerSync _obj = new SmServerSync();
                _obj.Serverid = Convert.ToInt32(slServdet["ID"]);
                _obj.Load();
                _obj.ServerName = (slServdet.ContainsKey("SERVER_NAME")) ? slServdet["SERVER_NAME"].Trim().ToUpper() : null;
                _obj.ServiceUrl = (slServdet.ContainsKey("SERVICE_URL")) ? slServdet["SERVICE_URL"].Trim() : null;
                _obj.CreatedDate = DateTime.Now;
                _obj.UpdatedDate = DateTime.Now;
                _dataAccess.BeginTransaction();
                _obj.Insert(_dataAccess);
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
            }
            finally
            {
                _dataAccess._Dispose();
            }
            return _result;
        }

        public string SetBuyerDetails_ForSync(string ID, string UserHostServer, List<string> slServdet, List<string> slServpaths, string cSynQryPath)
        {
            string _result = "";
            Dictionary<string, DataSet> slSyncDataSets = new Dictionary<string, DataSet>(); slSyncDataSets.Clear();
            SmAddress _addrobj = SmAddress.Load(Convert.ToInt32(ID));
            SmAddressConfig _addrCnfgobj = SmAddressConfig.Load_AddressID(Convert.ToInt32(ID));
            DataSet dsaddrobj = SmAddress.GetAddress_By_Id(Convert.ToInt32(ID)); slSyncDataSets.Add("SM_ADDRESS", dsaddrobj);
            DataSet dsaddrCnfgobj = SmAddressConfig.Get_AddressConfig_By_AddressID(Convert.ToInt32(ID)); slSyncDataSets.Add("SM_ADDRESS_CONFIG", dsaddrCnfgobj);
            DataSet dsdocfmt = SmDocumentFormats.Get_DocumentFormat_By_Format(_addrCnfgobj.DefaultFormat); slSyncDataSets.Add("SM_DOCUMENT_FORMATS", dsdocfmt);
            DataSet dsdocfmtrules = SmDocumentformatRules.Get_DocumentformatRules_By_DocFormat(_addrCnfgobj.DefaultFormat); slSyncDataSets.Add("SM_DOCUMENTFORMAT_RULES", dsdocfmtrules);
            DataSet dsrulecollect = SmDefaultRules.GetDefaultRules_AddressID_Format(Convert.ToInt32(ID), _addrCnfgobj.DefaultFormat); slSyncDataSets.Add("SM_DEFAULT_RULES", dsrulecollect);
            DataSet dsxblnkcollect = SmXlsBuyerLink.Get_ExcelBuyerLink_Wiz(ID); slSyncDataSets.Add("SM_XLS_BUYER_LINK", dsxblnkcollect);
            DataSet dspblnkcollect = SmPdfBuyerLink.Get_PdfBuyerLink(ID); slSyncDataSets.Add("SM_PDF_BUYER_LINK", dspblnkcollect);
            try
            {
                SmServerSyncCollection _servCollct = SmServerSync.GetAll();
                for (int l = 0; l < slServdet.Count; l++)
                {
                    if (_servCollct != null && _servCollct.Count > 0)
                    {
                        foreach (SmServerSync _objserv in _servCollct)
                        {
                            ServiceCallRoutines _WebServiceCall = new ServiceCallRoutines();
                            string _servname = _objserv.ServerName.Trim(); string _serurl = _objserv.ServiceUrl.Trim();
                            if (_servname.ToUpper() == slServdet[l].ToUpper())
                            {
                                if (!string.IsNullOrEmpty(_serurl))
                                {
                                    UpdatePaths_Sync(_servname.ToUpper(), dsaddrobj, dsaddrCnfgobj, slServpaths);

                                    object[] _obj = new object[8];
                                    _obj[0] = dsaddrobj; _obj[1] = dsaddrCnfgobj; _obj[2] = dsdocfmt; _obj[3] = dsrulecollect;
                                    _obj[4] = dsxblnkcollect; _obj[5] = dspblnkcollect; _obj[6] = UserHostServer; _obj[7] = dsdocfmtrules;
                                    _result = Convert.ToString(_WebServiceCall.CallWebService(_serurl, "Service", "SaveBuyerDetails", _obj));
                                }
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { _result = ""; }
            SaveSyncQueries(_addrobj.AddrCode, slSyncDataSets, cSynQryPath);
            return _result;
        }

        private void SaveSyncQueries(string cCode,Dictionary<string, DataSet> slSyncDataSet, string cSynQryPath)
        {
            List<string> slSQL = new List<string>(); slSQL.Clear();
            try
            {
                string filename = cCode + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".txt";
                if (!Directory.Exists(cSynQryPath)) { Directory.CreateDirectory(cSynQryPath); } string filepath = cSynQryPath + filename;
                foreach (string key in slSyncDataSet.Keys)
                {
                    GetQueries(slSQL, key, (DataSet)slSyncDataSet[key]);
                }
                if (slSQL != null && slSQL.Count > 0)
                {
                    string cQrytxt = "";
                    for (int k = 0; k < slSQL.Count; k++)
                    {
                        cQrytxt += slSQL[k] + Environment.NewLine;
                    }
                    File.WriteAllText(filepath,cQrytxt);
                }
            }
            catch (Exception ex){ throw; }
        }

        private List<string> GetQueries(List<string> slSQL, string key, DataSet dsTable)
        {
            string cQuery = "";
            if (dsTable != null)
            {
                string colvalues = ""; DataTable dt = dsTable.Tables[0];
                string[] _aColnames = (from dc in dt.Columns.Cast<DataColumn>() select dc.ColumnName).ToArray();
                string colnames = String.Join(",", _aColnames);
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < _aColnames.Length; i++)
                    {
                        string _name = _aColnames[i];
                        if (row[_name].GetType() == typeof(int) || row[_name].GetType() == typeof(float)) { colvalues += row[_name] + ","; }
                        else if (row[_name].GetType() == typeof(System.DateTime)) { colvalues += "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',"; }
                        else
                        {
                            if (Convert.ToString(row[_name]) == "") { colvalues += "NULL,"; } else { colvalues += "'" + Convert.ToString(row[_name]) + "',"; }
                        }
                    }
                    colvalues = colvalues.TrimEnd(',');
                    cQuery = "INSERT INTO " + key + " (" + colnames + ") VALUES (" + colvalues + ");";
                    slSQL.Add(cQuery);
                }
            }
            return slSQL;
        }

        public void UpdatePaths_Sync(string Serv_name,DataSet dsaddrobj, DataSet dsaddrCnfgobj,List<string> slServpaths) 
        {
            for (int k = 0; k < slServpaths.Count;k++ )
            {
                string key = slServpaths[k].Split('|')[0]; string value = slServpaths[k].Split('|')[1];
                if(key.Split('_')[1].ToUpper() == Serv_name)
                {
                    switch(key.Split('_')[0])
                    {
                        case "IMPORT": string improw = Convert.ToString(dsaddrCnfgobj.Tables[0].Rows[0]["IMPORT_PATH"]); if (!string.IsNullOrEmpty(value) && improw != value) { dsaddrCnfgobj.Tables[0].Rows[0]["IMPORT_PATH"] = value; }
                            break;
                        case "EXPORT": string exprow = Convert.ToString(dsaddrCnfgobj.Tables[0].Rows[0]["EXPORT_PATH"]); if (!string.IsNullOrEmpty(value) && exprow != value) { dsaddrCnfgobj.Tables[0].Rows[0]["EXPORT_PATH"] = value; }
                            break;
                        case "DOWNLOAD": string dwnprow = Convert.ToString(dsaddrobj.Tables[0].Rows[0]["ADDR_INBOX"]); if (!string.IsNullOrEmpty(value) && dwnprow != value) { dsaddrobj.Tables[0].Rows[0]["ADDR_INBOX"] = value; }
                            break;
                        case "UPLOAD": string upprow = Convert.ToString(dsaddrobj.Tables[0].Rows[0]["ADDR_OUTBOX"]); if (!string.IsNullOrEmpty(value) && upprow != value) { dsaddrobj.Tables[0].Rows[0]["ADDR_OUTBOX"] = value; }
                            break;
                    }               
                }
            }
        }     

        //public string GetBuyerServiceNames()
        //{
        //    string _servdet = "", _servname = "", _servUrl = "";
        //    string _byrURLs = ConfigurationManager.AppSettings["BUYER_WEB_SERVICE"];
        //    if (!string.IsNullOrEmpty(_byrURLs))
        //    {
        //        string[] _arrUrl = _byrURLs.Split(',');
        //        for (int k = 0; k < _arrUrl.Length; k++)
        //        {
        //            _servname += _arrUrl[k].Split('|')[0] + ","; _servUrl += _arrUrl[k].Split('|')[1] + ",";
        //        }
        //        _servdet = _servname.TrimEnd(',') + "#" + _servUrl.TrimEnd(',');
        //    }
        //    return _servdet;
        //}

        //public Dictionary<string, string> GetBuyerServiceDetails()
        //{
        //    Dictionary<string, string> _dictservers = new Dictionary<string, string>(); _dictservers.Clear();
        //    string _byrURLs = ConfigurationManager.AppSettings["BUYER_WEB_SERVICE"];
        //    if (!string.IsNullOrEmpty(_byrURLs))
        //    {
        //        string[] _arrUrl = _byrURLs.Split(',');
        //        for (int k = 0; k < _arrUrl.Length; k++)
        //        {
        //            _dictservers.Add(_arrUrl[k].Split('|')[0], _arrUrl[k].Split('|')[1]);
        //        }
        //    }
        //    return _dictservers;
        //}

        #endregion
    
        #endregion

        public DataSet GeteInvoiceCurrency()
        {
            ServiceCallRoutines _WebServiceCall = new ServiceCallRoutines();
            DataSetCompressor _compressor = new DataSetCompressor();
            string eInvoiceURL = ConfigurationManager.AppSettings["EINVOICE_WEB_SERVICE"];
            object[] _obj = new object[2];
            string[] _parm = new string[1];
            _parm[0] = "VAL=1002";
            _obj[0] = "SELECT * FROM SM_CURRENCY WHERE 1002 =@VAL";
            _obj[1] = _parm;
            byte[] _byteData = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _obj);
            DataSet _ds = _compressor.Decompress(_byteData);
            return _ds;
        }

        public DataSet GetPaymentDetails()
        {
            ServiceCallRoutines _WebServiceCall = new ServiceCallRoutines();
            DataSetCompressor _compressor = new DataSetCompressor();
            string eInvoiceURL = ConfigurationManager.AppSettings["EINVOICE_WEB_SERVICE"];
            object[] _obj = new object[2];
            string[] _parm = new string[1];
            _parm[0] = "VAL=1002";
            _obj[0] = "SELECT * FROM SMV_LES_PAYMENT_MODE_DETAILS WHERE 1002 =@VAL";
            _obj[1] = _parm;
            byte[] _byteData = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _obj);
            DataSet _ds = _compressor.Decompress(_byteData);
            return _ds;
        }

        public string  GetPaymentDetails_Code(string PAYMENT_CODE)
        {
            string cPaymentMode = "";
            ServiceCallRoutines _WebServiceCall = new ServiceCallRoutines();
            DataSetCompressor _compressor = new DataSetCompressor();
            string eInvoiceURL = ConfigurationManager.AppSettings["EINVOICE_WEB_SERVICE"];
            object[] _obj = new object[2];
            string[] _parm = new string[1];
            _parm[0] = "PAYMENT_CODE=" + PAYMENT_CODE;
            _obj[0] = "SELECT * FROM SMV_LES_PAYMENT_MODE_DETAILS WHERE PAYMENT_CODE = @PAYMENT_CODE";
            _obj[1] = _parm;
            byte[] _byteData = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _obj);
            DataSet _ds = _compressor.Decompress(_byteData);
            if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
            {
                cPaymentMode = PAYMENT_CODE + ":" + Convert.ToString(_ds.Tables[0].Rows[0]["PAYMENT_CODE_DESC"]);
            }
            else { cPaymentMode = PAYMENT_CODE; }
            return cPaymentMode;
        }

        public DataSet GeteInvoiceCountry()
        {
            ServiceCallRoutines _WebServiceCall = new ServiceCallRoutines();
            DataSetCompressor _compressor = new DataSetCompressor();
            string eInvoiceURL = ConfigurationManager.AppSettings["EINVOICE_WEB_SERVICE"];
            object[] _obj = new object[2];
            string[] _parm = new string[1];
            _parm[0] = "VAL=1002";
            _obj[0] = "SELECT * FROM LES_COUNTRY_CODE WHERE 1002 =@VAL";
            _obj[1] = _parm;
            byte[] _byteData = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _obj);
            DataSet _ds = _compressor.Decompress(_byteData);
            return _ds;
        }

        public DataSet GeteInvoiceFormats(string ADDR_TYPE)
        {
            ServiceCallRoutines _WebServiceCall = new ServiceCallRoutines();
            DataSetCompressor _compressor = new DataSetCompressor();
            string eInvoiceURL = ConfigurationManager.AppSettings["EINVOICE_WEB_SERVICE"];
            object[] _obj = new object[2];
            string[] _parm = new string[1];
            _parm[0] = "ADDR_TYPE=" + ADDR_TYPE.ToUpper();
            _obj[0] = "SELECT * FROM SM_INVOICE_FORMATS WHERE ADDR_TYPE = @ADDR_TYPE";
            _obj[1] = _parm;
            byte[] _byteData = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _obj);
            DataSet _ds = _compressor.Decompress(_byteData);
            return _ds;
        }

        public void SetAuditLog(string Modulename, string Auditvalue, string Logtype, string Keyref2, string FileName, string BuyerID, string SupplierID)
        {
            try
            {
                SmAuditlog _audit = new SmAuditlog();
                _audit.Modulename = Modulename;
                _audit.Auditvalue = DateTime.Now.ToString("yy-MM-dd HH:mm") + " : " + Auditvalue;
                _audit.Logtype = Logtype;
                _audit.Keyref2 = Keyref2;
                _audit.Updatedate = DateTime.Now;
                _audit.Filename = FileName;
                if (BuyerID.Trim() != "") _audit.BuyerId = Convert.ToInt32(BuyerID);
                if (SupplierID.Trim() != "") _audit.SupplierId = Convert.ToInt32(SupplierID);
                //
                _audit.Insert();
            }
            catch (Exception ex)
            { }
        }

        public void SetAuditLog(string Modulename, string Auditvalue, string Logtype, string Keyref2, string FileName, string BuyerID, string SupplierID, Dal.DataAccess _dataAccess)
        {
            try
            {
                SmAuditlog _audit = new SmAuditlog();
                _audit.Modulename = Modulename;
                _audit.Auditvalue = DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + " : " + Auditvalue;
                _audit.Logtype = Logtype;
                _audit.Keyref2 = Keyref2;
                _audit.Updatedate = DateTime.Now;
                _audit.Filename = FileName;
                if (BuyerID.Trim() != "") _audit.BuyerId = Convert.ToInt32(BuyerID);
                if (SupplierID.Trim() != "") _audit.SupplierId = Convert.ToInt32(SupplierID);

                _audit.Insert(_dataAccess);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public System.Data.DataSet Get_XLS_Group_Mapping()
        {
            return SmXlsGroupMapping.GetAllGroupMappings();
        }

        public void SetLog(string log)
        {
            try
            {
                string _path = "";
                if (string.IsNullOrWhiteSpace(_path)) _path = HttpContext.Current.Server.MapPath("~/Log");
                if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
                string _logFile = _path + "\\LeSMonitor_" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
                using (StreamWriter sw = new StreamWriter(_logFile, true))
                {
                    log = DateTime.Now.ToString("dd-MM-yy HH:mm:ss") + " : " + log;
                    sw.WriteLine(log);
                    sw.Flush();
                    sw.Dispose();
                }
            }
            catch (Exception ex)
            { }
        }

        private string GetCorrectPath(string _Path, string _File)
        {
            try
            {
                string cReturn = "";
                string[] slPaths;

                slPaths = _Path.Split('|');
                foreach (string cPath in slPaths)
                {
                    FileInfo file = new FileInfo(cPath + "\\" + _File);
                    {
                        if (file.Exists)
                        {
                            cReturn = cPath;
                            break;
                        }
                    }
                }
                return cReturn;
            }
            catch (Exception ex)
            {
                SetLog(ex.StackTrace);
                throw ex;
            }
        }

        public DataSet GetMailQueueData()
        {
            DataSet ds = new DataSet();
            Dal.DataAccess _dataAccess = null;
            try
            {
                _dataAccess = new Dal.DataAccess();
                string SQL = "SELECT QUEUE_ID, QUOTATIONID, QUOTE_EXPORTED, DOC_TYPE, REF_KEY, MAIL_FROM, MAIL_TO, MAIL_CC, MAIL_BCC, MAIL_SUBJECT, MAIL_BODY, ATTACHMENTS, " +
                         " MAIL_DATE,case  when NOT_TO_SENT IS NULL then 0 when NOT_TO_SENT = 0 then 0 when NOT_TO_SENT = 1 then 1 when NOT_TO_SENT = 2 then 0 when NOT_TO_SENT = 3 then 0 End as NOT_TO_SENT " +
                         ", BUYER_ID, SUPPLIER_ID, REPLY_EMAIL, SENDER_NAME, RECEIVER_NAME, ACTION_TYPE, HTML_NOT_TO_SEND, " +
                         " SEND_HTML_MSG, USE_HTML_FILE_MSG, DELAY_MAIL_MIN FROM SM_SEND_MAIL_QUEUE Where 100043 = @VAL";
                _dataAccess.CreateSQLCommand(SQL);
                _dataAccess.AddParameter("VAL", 100043, ParameterDirection.Input);
                ds = _dataAccess.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (_dataAccess != null) _dataAccess._Dispose();
            }
            return ds;
        }

        public DataSet GetMailQueueData_By_ID(string QueueID)
        {
            DataSet ds = new DataSet();
            Dal.DataAccess _dataAccess = null;
            try
            {
                _dataAccess = new Dal.DataAccess();
                _dataAccess.CreateSQLCommand("SELECT * FROM SM_SEND_MAIL_QUEUE Where QUEUE_ID =@QUEUE_ID");
                _dataAccess.AddParameter("QUEUE_ID", QueueID, ParameterDirection.Input);
                ds = _dataAccess.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (_dataAccess != null) _dataAccess._Dispose();
            }
            return ds;
        }

        public string UpdateMailQueueDetails(string MailQueueId, string MailTo, string MailCC, string MailBCC, string MailSubject, string MailBody, string MailAttachment, string MailStatus)
        {
            string _result = "";
            Dal.DataAccess _dataAccess = null;
            try
            {
                _dataAccess = new Dal.DataAccess();
                _dataAccess.BeginTransaction();

                string _SQL = "Update SM_SEND_MAIL_QUEUE SET MAIL_TO = @MAIL_TO,MAIL_CC =@MAIL_CC,MAIL_BCC = @MAIL_BCC,MAIL_SUBJECT= @MAIL_SUBJECT,MAIL_BODY= @MAIL_BODY,ATTACHMENTS= @ATTACHMENTS,NOT_TO_SENT= @NOT_TO_SENT WHERE QUEUE_ID = @QUEUE_ID";
                _dataAccess.CreateSQLCommand(_SQL);
                _dataAccess.AddParameter("MAIL_TO", MailTo, ParameterDirection.Input);
                _dataAccess.AddParameter("MAIL_CC", MailCC, ParameterDirection.Input);
                _dataAccess.AddParameter("MAIL_BCC", MailBCC, ParameterDirection.Input);
                _dataAccess.AddParameter("MAIL_SUBJECT", MailSubject, ParameterDirection.Input);
                _dataAccess.AddParameter("MAIL_BODY", MailBody, ParameterDirection.Input);
                _dataAccess.AddParameter("ATTACHMENTS", MailAttachment, ParameterDirection.Input);
                _dataAccess.AddParameter("NOT_TO_SENT", MailStatus, ParameterDirection.Input);
                _dataAccess.AddParameter("QUEUE_ID", MailQueueId, ParameterDirection.Input);

                _result = Convert.ToString(_dataAccess.ExecuteNonQuery());
                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                if (_dataAccess != null) _dataAccess._Dispose();
            }
            return _result;
        }

        public string DeleteMailQueueDetails(string MailQueueId)
        {
            string _result = "";
            Dal.DataAccess _dataAccess = null;
            try
            {
                _dataAccess = new Dal.DataAccess();
                _dataAccess.BeginTransaction();

                string _SQL = "DELETE SM_SEND_MAIL_QUEUE WHERE QUEUE_ID = @QUEUE_ID";
                _dataAccess.CreateSQLCommand(_SQL);
                _dataAccess.AddParameter("QUEUE_ID", MailQueueId, ParameterDirection.Input);
                _result = Convert.ToString(_dataAccess.ExecuteNonQuery());

                _dataAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dataAccess.RollbackTransaction();
                throw ex;
            }
            finally
            {
                if (_dataAccess != null) _dataAccess._Dispose();
            }
            return _result;
        }

        public void LoadAdminPage()
        {
            string AddrType = Convert.ToString(HttpContext.Current.Session["ADDRTYPE"]).ToLower().Trim();
            string AddrID = Convert.ToString(HttpContext.Current.Session["ADDRESSID"]).ToLower().Trim();
            string AddressId = ConfigurationManager.AppSettings["ADMINID"].ToString();
            if (AddrID != AddressId)
            {
                HttpContext.Current.Response.Redirect("Default.aspx");
            }
        }

        public string Get_Version()
        {
            string version = "";
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            version = fvi.FileVersion;
            if (HttpContext.Current.Response.Cookies.Count > 0)
            {
                foreach (string s in HttpContext.Current.Response.Cookies.AllKeys)
                {
                    if (s == FormsAuthentication.FormsCookieName || "asp.net_sessionid".Equals(s, StringComparison.InvariantCultureIgnoreCase))
                    {
                        HttpContext.Current.Response.Cookies[s].Secure = true;
                    }
                }
            }
            return version;
        }

        //ADDED BY KALPITA ON 23/02/2018
        public System.Data.DataSet GetAllBuyers_Format(string FORMAT)
        {
            return SmBuyerSupplierLink.GetBuyers_Format(FORMAT);
        }

        private string GetCellValue(Worksheet _sht, string CellHeader)
        {
            Cell _cell = _sht.Cells.Find(CellHeader, null, _option);
            string _cellValue = convert.ToString(_sht.Cells[_cell.Row, _cell.Column + 1].Value);
            if (_cellValue.Trim() != "NULL") return _cellValue.Trim();
            else return "";
        }

        private string GetCellValueNoTrim(Worksheet _sht, string CellHeader)
        {
            Cell _cell = _sht.Cells.Find(CellHeader, null, _option);
            string _cellValue = convert.ToString(_sht.Cells[_cell.Row, _cell.Column + 1].Value);
            if (_cellValue.Trim() != "NULL") return _cellValue;
            else return "";
        }

        private void SetCellValue(Worksheet _sht, string CellHeader, object CellValue)
        {
            Cell _cell = _sht.Cells.Find(CellHeader, null, _option);
            _sht.Cells[_cell.Row, _cell.Column + 1].Value = CellValue;
        }

    }

    public class convert
    {
        public static long ToLong(object valueFromDb)
        {
            try
            {
                return valueFromDb != DBNull.Value ? Convert.ToInt64(valueFromDb) : 0;
            }
            catch { return 0; }
        }

        public static double ToFloat(object valueFromDb)
        {
            try
            {
                return valueFromDb != DBNull.Value ? Convert.ToDouble(valueFromDb) : 0.0F;
            }
            catch { return 0.0F; }
        }

        public static Boolean ToBoolean(object valueFromDb)
        {
            try
            {
                if (valueFromDb.ToString() == "1") return true;
                else return false;
            }
            catch { return false; }
        }

        public static int ToInt(object valueFromDb)
        {
            try
            {
                if (valueFromDb.ToString().ToUpper() == "TRUE") return 1;
                else if (valueFromDb.ToString().ToUpper() == "YES") return 1;
                return valueFromDb != DBNull.Value ? Convert.ToInt32(valueFromDb) : 0;
            }
            catch { return 0; }
        }

        public static string ToDBValue(int nValue)
        {
            try
            {
                return nValue > 0 ? nValue.ToString() : "NULL";
            }
            catch { return "NULL"; }
        }

        public static object ToDBValue(string cValue)
        {
            try
            {
                if (cValue.Trim() == "") return DBNull.Value;
                else if (cValue.Trim() == "0") return DBNull.Value;
                else return cValue;
            }
            catch { return DBNull.Value; }
        }

        public static string ToQuote(string Value)
        {
            if (Value == null) Value = "";
            Value = Value.Replace("'", "''");
            return ("'" + Value + "'");
        }

        public static DateTime ToDateTime(object valueFromDb)
        {
            try
            {
                if ((valueFromDb.ToString() != null) && (valueFromDb.ToString().Length > 0))
                    return Convert.ToDateTime(valueFromDb);
                else return DateTime.MinValue;
            }
            catch { return DateTime.MinValue; }
        }

        public static DateTime ToDateTime(object valueFromDb, string DateFormat)
        {
            try
            {
                if (DateFormat != string.Empty)
                {
                    if ((valueFromDb.ToString() != null) && (valueFromDb.ToString().Length > 0))
                        return DateTime.ParseExact(valueFromDb.ToString(), DateFormat, null);
                    else return DateTime.MinValue;
                }
                else
                {
                    return ToDateTime(valueFromDb);
                }
            }
            catch { return DateTime.MinValue; }
        }

        public static NameValueCollection ToNameValueCollection(DataSet ds)
        {
            DataTable dt = ds.Tables[0];
            NameValueCollection nvcReturn = new NameValueCollection();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j <= ds.Tables[0].Rows.Count - 1; j++)
                {
                    DataRow dr = ds.Tables[0].Rows[j];

                    for (int i = 0; i <= dt.Columns.Count - 1; i++)
                    {
                        nvcReturn.Add(dt.Columns[i].ColumnName.ToString(), dr[dt.Columns[i].ColumnName.ToString()].ToString());
                    }
                }
            }

            return nvcReturn;
        }

        public static Dictionary<string, string> ToDictionary(DataSet ds)
        {
            DataTable dt = ds.Tables[0];
            Dictionary<string, string> dicReturn = new Dictionary<string, string>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    DataRow dr = ds.Tables[0].Rows[j];

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dicReturn.Add(dt.Columns[i].ColumnName.ToString(), dr[dt.Columns[i].ColumnName.ToString()].ToString());
                    }
                }
            }
            return dicReturn;
        }

        public static string ToFileName(string cValue)
        {
            try
            {
                cValue = cValue.Replace(" ", "_");
                cValue = cValue.Replace("/", "_");
                cValue = cValue.Replace("?", "_");
                cValue = cValue.Replace("\\", "_");
                cValue = cValue.Replace(":", "_");
                cValue = cValue.Replace("*", "_");
                cValue = cValue.Replace("<", "_");
                cValue = cValue.Replace(">", "_");
                cValue = cValue.Replace("|", "_");
                cValue = cValue.Replace("\"", "_");
                cValue = cValue.Replace("'", "");
                cValue = cValue.Replace(",", "_");
                return cValue;
            }
            catch { return cValue; }
        }

        public static string ToString(object valueFromDb)
        {
            try
            {
                return valueFromDb != DBNull.Value ? Convert.ToString(valueFromDb) : "";
            }
            catch { return ""; }
        }

        public static string ToNumericOnly(string cInput)
        {
            string cNUM = "0123456789", cReturn = "";

            for (int i = 0; i < cInput.Length; i++)
            {
                if (cNUM.IndexOf(cInput[i]) != -1)
                    cReturn += cInput[i];
            }
            return cReturn;
        }

        public static string ToXMLString(string cInput)
        {
            cInput = cInput.Replace("<", "&lt;");
            cInput = cInput.Replace(">", "&gt;");
            cInput = cInput.Replace(" & ", "&amp;");
            cInput = cInput.Replace("\"", "&quot;");
            cInput = cInput.Replace("\'", "&apos;");

            return cInput;
        }

        public static string ToPositive(object valueFromDb)
        {
            try
            {
                if (valueFromDb.ToString().ToUpper() == "TRUE") return "1";
                else if (valueFromDb == null) return "-";
                else if (valueFromDb == DBNull.Value) return "-";
                else if (convert.ToInt(valueFromDb) > 0) return convert.ToInt(valueFromDb).ToString();
                else return "-";
            }
            catch { return "-"; }
        }

        public static Boolean CheckForNumericString(string cString)
        {
            Boolean blReturn = true;
            cString = cString.Replace("-", "");

            string cNUM = "0123456789";

            for (int i = 0; i < cString.Length; i++)
            {
                if (cNUM.IndexOf(cString[i]) == -1)
                    return false;
            }

            return blReturn;
        }
    }

    public class DataSetCompressor
    {

        public Byte[] Compress(DataSet dataset)
        {
            Byte[] data;
            MemoryStream mem = new MemoryStream();
            GZipStream zip = new GZipStream(mem, CompressionMode.Compress);
            dataset.WriteXml(zip, XmlWriteMode.WriteSchema);
            zip.Close();
            data = mem.ToArray();
            mem.Close();
            return data;
        }

        public DataSet Decompress(Byte[] data)
        {
            DataSet dataset = new DataSet();

            if (data != null)
            {
                MemoryStream mem = new MemoryStream(data);
                GZipStream zip = new GZipStream(mem, CompressionMode.Decompress);
                dataset.ReadXml(zip, XmlReadMode.ReadSchema);
                zip.Close();
                mem.Close();
            }
            return dataset;
        }

        public void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    //msi.CopyTo(gs);
                    CopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }

        public string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }
    }

    public static class DataTableCreator
    {
        public static DataTable ConvertToDataTable<TSource>(this IEnumerable<TSource> records, params Expression<Func<TSource, object>>[] columns)
        {
            var firstRecord = records.First();
            if (firstRecord == null)
                return null;

            DataTable table = new DataTable();

            List<Func<TSource, object>> functions = new List<Func<TSource, object>>();
            foreach (var col in columns)
            {
                DataColumn column = new DataColumn();
                var function = col.Compile();
                column.DataType = function(firstRecord).GetType();
                //if ((col.Body as MemberExpression) != null)
                //{
                //    column.Caption = (col.Body as MemberExpression).Member.Name;
                //    column.ColumnName = (col.Body as MemberExpression).Member.Name;
                //}
                //else
                //{
                //    column.Caption = "IS_ACTIVE";
                //    column.ColumnName = "IS_ACTIVE";
                //}
                MemberExpression body = col.Body as MemberExpression;
                if (body == null)
                {
                    UnaryExpression ubody = (UnaryExpression)col.Body;
                    body = ubody.Operand as MemberExpression;
                    column.Caption = body.Member.Name;
                    column.ColumnName = body.Member.Name;

                }
                else
                {
                    column.Caption = body.Member.Name;
                    column.ColumnName = body.Member.Name;
                }
                functions.Add(function);
                table.Columns.Add(column);
            }
            int j = 0;
            foreach (var record in records)
            {
                DataRow row = table.NewRow();
                int i = 0;
                foreach (var function in functions)
                {
                    row[i] = function((record));
                    i++;
                }
                table.Rows.Add(row);
                j++;
            }
            return table;
        }
    }

  
}