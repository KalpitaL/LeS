using AjaxControlToolkit;
using MetroLesMonitor.Bll;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace MetroLesMonitor.LESMonitorPages
{
    public partial class SupplierDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GeSupplierDetails(string SUPPLIERID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(SUPPLIERID))
            {
                Dictionary<string, string> sldet = _Routine.GetAddressDetails(Convert.ToInt32(SUPPLIERID));
                json = JsonConvert.SerializeObject(sldet);
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetAllGroups()
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.GetAllGroups();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        #region Buyer/Supplier
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string FillLinkedBuyersGrid(string SUPPLIERID)
        {
            string json = ""; DataSet dsClone = new DataSet();
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(SUPPLIERID))
            {
                _ds = _Routine.Get_Supplier_Specific_Buyers(Convert.ToInt32(SUPPLIERID));
                dsClone = _ds.Copy();

                dsClone.Tables[0].Columns.Add("EXPASSWORD");

                foreach (DataRow row in dsClone.Tables[0].Rows)
                {
                    foreach (DataColumn column in dsClone.Tables[0].Columns)
                    {
                        if (column.ColumnName == "EX_PASSWORD")
                        {
                            string value = GlobalTools.DecodePassword(convert.ToString(row.ItemArray[column.Ordinal]));
                            row.SetField("EXPASSWORD", value);
                        }
                    }
                }
                dsClone.AcceptChanges();
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(dsClone.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveBuyerSuppLinkDetails(string LINKID, List<string> slBSLnkdet)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            Dictionary<string, string> lstLinkDetails = new Dictionary<string, string>();
            List<string> slvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slBSLnkdet);

                if (_lstdet != null && _lstdet.Count > 0)
                {
                    slvalues.Clear();
                    for (int i = 0; i < _lstdet.Count; i++)
                    {
                        string lstkey = _lstdet[i][0];
                        slvalues.Add(lstkey);
                    }
                    Dictionary<string, string> _dictexp = _def.ConvertListToDictionary(slvalues);
                    lstLinkDetails = _dictexp;
                    _Routine.Update_Link_Details(convert.ToInt(LINKID), lstLinkDetails, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                    json = "1";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DeleteBuyerSuppLink(string LINKID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                _Routine.Delete_Link(Convert.ToInt32(LINKID), Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                json = "1";
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetAllBuyers()
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.GetAllBuyers();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string AddBuyer(string SUPPLIERID, string BUYERID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                int BuyerAddressID = Convert.ToInt32(BUYERID);
                //_Routine.Add_BuyerSupplier_Link(Convert.ToInt32(SUPPLIERID), BuyerAddressID, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                _Routine.Add_BuyerSupplier_Link_config(Convert.ToInt32(SUPPLIERID), BuyerAddressID, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                json = "1";
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CheckExistingBSuppLink(string LINKID, string BLINKCODE, string SLINKCODE)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            bool IsExist = _Routine.GetLinkBy_Buyer_Vendor_LinkCode(convert.ToInt(LINKID), BLINKCODE, SLINKCODE);
            if (IsExist) { json = "Buyer-Supplier Link Codes should be unique."; }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CheckExistingGroup(string GROUPCODE)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            int nExist = _Routine.Get_GroupID_BY_GroupCode(GROUPCODE);
            if (nExist <= 0) { json = "Please provide Group Code"; }
            return json;
        }

        #endregion

        #region Buyer-Supplier Link Rules

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetBuyerSupplierRulesGrid(string LINKID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            System.Data.DataSet _ds = new DataSet();
            if (Convert.ToInt32(LINKID) > 0)
            {
                _ds = _Routine.Get_SMV_BuyerSupplierLinkRule(Convert.ToInt32(LINKID));
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_ds.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }


        #endregion

        #region Buyer Item & UOM Details
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetItemMapGrid(string LINKID)
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.FillItemMapGrid(Convert.ToInt32(LINKID));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveItemMapping(List<string> slBSItemdet)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            Dictionary<string, string> _ItemMap = new Dictionary<string, string>();
            _ItemMap.Clear();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slBSItemdet);
                if (_lstdet != null && _lstdet.Count > 0)
                {
                    slvalues.Clear();
                    for (int i = 0; i < _lstdet.Count; i++)
                    {
                        string lstkey = _lstdet[i][0];
                        slvalues.Add(lstkey);
                    }
                    Dictionary<string, string> _dictexp = _def.ConvertListToDictionary(slvalues);
                    _ItemMap = _dictexp;
                    _Routine.SaveItemMapping(_ItemMap, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                    json = "1";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DeleteItemMapping(string REFID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                _Routine.DeleteItemMapping(Convert.ToInt32(REFID), Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                json = "1";
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetItemUOMMapGrid(string SUPPLIERID, string BUYERID)
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            _ds = _Routine.FillItemUOM_MAPGrid(Convert.ToInt32(SUPPLIERID), Convert.ToInt32(BUYERID));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveItemUOMMappingDetails(List<string> slBSUOMdet)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            Dictionary<string, string> _ItemUOMMap = new Dictionary<string, string>();
            _ItemUOMMap.Clear();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(slBSUOMdet);

                if (_lstdet != null && _lstdet.Count > 0)
                {
                    slvalues.Clear();
                    for (int i = 0; i < _lstdet.Count; i++)
                    {
                        string lstkey = _lstdet[i][0];
                        slvalues.Add(lstkey);
                    }
                    Dictionary<string, string> _dictexp = _def.ConvertListToDictionary(slvalues);
                    _ItemUOMMap = _dictexp;
                    _Routine.SaveItemUOMMapping(_ItemUOMMap, Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                    json = "1";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DeleteItemUOMMapping(string ITEM_UOM_MAPID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                _Routine.DeleteItemUOMMapping(Convert.ToInt32(ITEM_UOM_MAPID), Convert.ToString(HttpContext.Current.Session["UserHostServer"]));
                json = "1";
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }
        #endregion

        #region Download & Upload

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DownloadTemplate(string LINKID, string TYPE)
        {
            string json = "", filePath = "", _fileName = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            FileUpload _fupload = new FileUpload();
            try
            {
                SmBuyerSupplierLink _fileBuyerSuppLink = new SmBuyerSupplierLink();
                string destfile = HttpContext.Current.Server.MapPath("../Downloads/");
                if (TYPE == "BIREF")
                {
                    filePath = HttpContext.Current.Server.MapPath("~/Templates/BUYSUPP_ITEMREF_TEMPLATE.xls");
                    _fileName = "BUYSUPP_ITEMREF_TEMPLATE_" + _fileBuyerSuppLink.BuyerLinkCode + "_" + _fileBuyerSuppLink.VendorLinkCode + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssff") + ".xls";
                }
                else if (TYPE == "BIUOM")
                {
                    filePath = HttpContext.Current.Server.MapPath("~/Templates/BUYSUPP_ITEMUOM_TEMPLATE.xls");
                    _fileName = "BUYSUPP_ITEMUOM_TEMPLATE_" + _fileBuyerSuppLink.BuyerLinkCode + "_" + _fileBuyerSuppLink.VendorLinkCode + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssff") + ".xls";
                }
                _fileBuyerSuppLink.Linkid = Convert.ToInt32(LINKID);
                _fileBuyerSuppLink.Load();

                DeleteFiles(destfile);
                string filefullpath = destfile + "\\" + _fileName;
                File.Copy(filePath, filefullpath, true);
                json = destfile + "|" + _fileName;
            }
            catch (Exception ex)
            { }

            return json;
        }



        private static void DeleteFiles(string DestFile)
        {
            DirectoryInfo di = new DirectoryInfo(DestFile);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SetUploadPath(string FILENAME)
        {
            //int nBuyerID = Convert.ToInt32(BUYERID);
            //int nSupplierID = Convert.ToInt32(SUPPLIERID);
            string uploadedFile = System.Configuration.ConfigurationManager.AppSettings["UPLOAD_ATTACHMENT"] + "\\" + HttpContext.Current.Session.SessionID + "\\";// +FILENAME;
            if (!Directory.Exists(Path.GetDirectoryName(uploadedFile))) Directory.CreateDirectory(Path.GetDirectoryName(uploadedFile));
            HttpContext.Current.Session["Upload_Path"] = Convert.ToString(uploadedFile);
            return uploadedFile;
        }

        public void File_UploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {
            try
            {
                AjaxFileUpload uControl = (AjaxFileUpload)sender;
                string FILENAME = e.FileName;

                if (uControl.IsInFileUploadPostBack)
                {
                    string uploadedFile = System.Configuration.ConfigurationManager.AppSettings["UPLOAD_ATTACHMENT"] + "\\" + HttpContext.Current.Session.SessionID;
                    if (!Directory.Exists(uploadedFile))
                    {
                        Directory.CreateDirectory(uploadedFile);
                    }
                    string FileUploadPath = uploadedFile + "\\" + FILENAME;
                    HttpContext.Current.Session["FileUploadPath"] = FileUploadPath;
                    uControl.SaveAs(FileUploadPath);
                }
            }
            catch (Exception ex)
            { }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string UploadFileMapping(string FILENAME, string LINKID, string SUPPLIERID, string BUYERID, string DELEXISTS, string TYPE)
        {
            string result = "", json = "";
            string FILETYPE = Convert.ToString(HttpContext.Current.Session["FILETYPE"]);
            string FILEUPLOADPATH = Convert.ToString(HttpContext.Current.Session["FileUploadPath"]);
            SupplierRoutines _Routine = new SupplierRoutines();
            try
            {
                int nBuyerID = Convert.ToInt32(BUYERID);
                int nSupplierID = Convert.ToInt32(SUPPLIERID);
                int nLINKID = Convert.ToInt32(LINKID);
                if (nBuyerID > -1 && nSupplierID > -1 && FILEUPLOADPATH != "")
                {
                    if (TYPE == "BIREF")
                    {
                        result = _Routine.UploadItemRefFile(Convert.ToBoolean(DELEXISTS), nLINKID, nBuyerID, nSupplierID, Convert.ToString(HttpContext.Current.Session["UserHostServer"]), FILEUPLOADPATH);
                    }
                    else
                    {
                        result = _Routine.UploadItemUOMFile(Convert.ToBoolean(DELEXISTS), nBuyerID, nSupplierID, Convert.ToString(HttpContext.Current.Session["UserHostServer"]), FILEUPLOADPATH);
                    }
                    if (result.Trim() != "" && result.Trim().Contains("SUCCESS"))
                    {
                        string print = result.Trim().Split('|')[1];
                        json = print;
                    }
                    else
                    {
                        json = result;
                    }
                }
                else
                {
                    json = "Either LinkID, BuyerID, SupplierID or FileName not Found.";
                }
            }
            catch (Exception ex)
            { json = "-1"; }
            return json;
        }

        #endregion

        #region Login Details

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SetLoginInfo(bool ChangePWD, string LINKID)
        {
            int Linkid = convert.ToInt(LINKID);
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!_Routine.Do_User_Exist_By_Link(Linkid))
            {
                _Routine.AddNewLogin(Linkid, convert.ToString(HttpContext.Current.Session["UserHostServer"]));
            }
            List<string> lst = _Routine.GetUserLoginInfo(Linkid);
            if (lst.Count > 0)
            {
                if (convert.ToString(HttpContext.Current.Session["LOGIN_NAME"]).ToUpper().Trim() == "AB") lst[3] = "";
                string _data = lst[0] + "|" + lst[1] + "|" + lst[2] + "|" + lst[3] + "|" + lst[4] + "|" + lst[5] + "|" + convert.ToString(HttpContext.Current.Session["LOGIN_NAME"]);
                return _data.Trim();
            }
            return "||||||";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetPwdCountByAddressID(string LINKID, string SUPPLIERID)
        {
            string _values = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            int LinkID = convert.ToInt(LINKID);
            SmBuyerSupplierLink _link = SmBuyerSupplierLink.Load(LinkID);

            if (_link != null)
            {
                DataSet ds = _Routine.GetLoginPWDCount(convert.ToInt(SUPPLIERID), convert.ToString(_link.SupplierEmail).Trim());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    _values = ds.Tables[0].Rows.Count.ToString();
                    List<string> lst = _Routine.GetAddressInfo(convert.ToInt(SUPPLIERID));
                    _values += "|" + lst[1].Trim();
                }
                return _values.Trim();
            }
            return "";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SetActiveStatus(string ACTIVE, string USERID)
        {
            SupplierRoutines _Routine = new SupplierRoutines();
            bool value = Convert.ToBoolean(ACTIVE);
            _Routine.SetActive(Convert.ToInt32(USERID), value);
            return "";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string UpdatePassword(string NEWPWD, string USERID)
        {
            string _newPassword = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(USERID))
            {
                _newPassword = _Routine.UpdatePassword(NEWPWD, Convert.ToInt32(USERID), convert.ToString(HttpContext.Current.Session["UserHostServer"])).Trim().ToUpper();
            }
            return _newPassword;
        }
        #endregion

        #region Config Settings

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetGroupFormat()
        {
            string json = "";
            XmlDocument doc = new XmlDocument();
            SupplierRoutines _Routine = new SupplierRoutines();
           // System.Data.DataSet _ds = _Routine.GetGroupFormat("Supplier");
            System.Data.DataSet _ds = _Routine.GetDocumentFormat_Addrtype("Supplier");
            doc.LoadXml(_ds.GetXml());
            json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetSupplierConfigDetails(string FORMAT, string SUPPLIERID)
        {
            string json = "";
            string cConfigPath = Convert.ToString(ConfigurationManager.AppSettings["CONFIG_PATH"]);
            string cMailSubj = Convert.ToString(ConfigurationManager.AppSettings["BS_MAIL_SUBJECT"]);
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(SUPPLIERID))
            {
                Dictionary<string, string> sldet = _Routine.GetAddressConfigDetails(FORMAT, Convert.ToInt32(SUPPLIERID));
                json = JsonConvert.SerializeObject(sldet);
            }
            return json + "||" + cConfigPath + "||" + cMailSubj;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveSupplierConfigDetails(List<string> sldet)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            MetroLesMonitor.Common.Default _def = new MetroLesMonitor.Common.Default();
            List<string> slvalues = new List<string>();
            try
            {
                List<string[]> _lstdet = _def.ConvertListToListArray(sldet);
                if (_lstdet != null && _lstdet.Count > 0)
                {
                    slvalues.Clear();
                    for (int i = 0; i < _lstdet.Count; i++)
                    {
                        string lstkey = _lstdet[i][0];
                        slvalues.Add(lstkey);
                    }
                    Dictionary<string, string> _dictexp = _def.ConvertListToDictionary(slvalues);
                    _Routine.SaveAddressConfig("Supplier", _dictexp,null,null);
                    json = "1";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CheckExistingConfig(string FORMAT, string SUPPLIERID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(SUPPLIERID))
            {
                json = _Routine.CheckExistingConfig(FORMAT, Convert.ToInt32(SUPPLIERID));
            }
            return json;
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetFormat_Supplier(string SUPPLIERID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(SUPPLIERID))
            {
                json = _Routine.GetFormat_By_Addressid(Convert.ToInt32(SUPPLIERID));
            }
            return json;
        }

        #endregion

        #region Default Rules  ADDED ON 09/12/2017
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetSupplierDefaultRules(string SUPPLIERID, string GROUP_FORMAT)
        {
            string json = "";
            System.Data.DataSet _ds = new DataSet();
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(SUPPLIERID))
            {
                _ds = _Routine.GetDefaultRules_Addressid_Format(Convert.ToInt32(SUPPLIERID), GROUP_FORMAT);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_ds.GetXml());
                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            }
            return json;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetRuleDetails(string RULEID)
        {
            string json = "";
            SupplierRoutines _Routine = new SupplierRoutines();
            if (!string.IsNullOrEmpty(RULEID))
            {
                json = _Routine.GetRuleDetails(Convert.ToInt32(RULEID));
            }
            return json;
        }
        #endregion

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetAddressName_Search(string SEARCHTEXT, string ADDRTYPE)
        {
            SupplierRoutines _Routine = new SupplierRoutines();
            List<string> slresult = new List<string>();
            slresult = _Routine.GetAddress_Search(ADDRTYPE, SEARCHTEXT);
            return slresult;
        }
    }
}