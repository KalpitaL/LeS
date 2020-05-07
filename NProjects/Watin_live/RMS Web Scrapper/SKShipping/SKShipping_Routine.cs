using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using WebScraper;
using WatiN.Core;
using System.Threading;
using LeSXML;
using System.IO;
using MTML.GENERATOR;

namespace SKShipping
{
    public class SKShipping_Routine : WebScraper.WebPage
    {
        List<string> _detailUrls = new List<string>();
        List<string> _lstRFQ = new List<string>();
        IEBrowser _ie = null;
        Dictionary<string, string> _xmlEquip = new Dictionary<string, string>();
        List<List<string>> _lstItems = new List<List<string>>();
        Dictionary<string, string> _itemMapping = new Dictionary<string, string>();
        Dictionary<string, string> _dictWebsiteMapping = new Dictionary<string, string>();
        Dictionary<string, string> _xmlHeader = new Dictionary<string, string>();
        List<string> _lstRFQDownloaded = new List<string>();
        string cRFQLogText = "";

        #region Upload Parameters
        string cVRNO = "", cBuyerCode = "", cVendorCode = "", cMessgNo = "", cSuppRef = "",
            cCurrencyCode = "", dtValidFrom = "", dtValidTo = "", cQuotationRemark = "";
        //chkAllID="";//commented on 12-01-2018
        List<string> chkAllID = new List<string>();//added on 12-01-2018
        double dAdditionalDisc = 0, grandTotal=0;
        public MTML.GENERATOR.LineItemCollection _lineItems = null;
        #endregion

        public void Start()
        {
            try
            {
                //WatiN.Core.Settings.AutoStartDialogWatcher = false;
                //Browser.RegisterAttachToHelper(typeof(IE), new WatiN.Core.Native.InternetExplorer.AttachToIeHelper());
                base.cRfqType = string.Empty; base.nItemCount = 0;
                bool found = false;
                if (taskToPerform == "DOWNLOAD") found = getDownloaded();
                else if (taskToPerform == "UPLOAD") found = checkQuoteFiles();
                if (found)
                {
                    getWebsiteMapping();
                    LogIn();
                    if (_ie != null)
                    {
                        if (_ie.Url.ToString().Contains("Main.aspx"))
                        {
                            if (taskToPerform == "DOWNLOAD" && found) ReadGrid();
                            else if (taskToPerform == "UPLOAD") getQuoteFile();
                        }
                        else throw new Exception("Unable to load SK Shipping website.");
                    }
                }
            }
            catch (Exception ex)
            {
                SetLog("Exception in 'Start()' function of 'SKShipping_Routine' class - " + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString());
                if (_ie != null)
                {
                    _ie.Dispose();
                    SetLog("Successfull closing of SK Shipping website." + Environment.NewLine);
                }
                //throw ex;
            }
            finally
            {
                KillIEInstances();//added by kalpita on 17/01/2020
            }
        }

        public void LogIn()
        {
            try
            {
                string cLoginUrl = convert.ToString(ConfigurationManager.AppSettings["LOGIN_URL"]).Trim();
                string cID = convert.ToString(ConfigurationManager.AppSettings["LOGIN_ID"]).Trim();
                string cPassword = convert.ToString(ConfigurationManager.AppSettings["LOGIN_PASSWORD"]).Trim();
                Module = convert.ToString(ConfigurationManager.AppSettings["MODULE"]).Trim();
                SetLog("Logging to SK Shipping website.");
                _ie = LoadPage(cLoginUrl);
                Link _btnLogin = _ie.Link(Find.ByTitle("Login"));
                if (_btnLogin != null)
                {
                    if (_btnLogin.Exists)
                    {
                        TextField _txtID = _ie.TextField(_dictWebsiteMapping["ID"]);
                        if (_txtID != null)
                            if (_txtID.Exists)
                                _txtID.TypeText(cID);
                            else throw new Exception("ID field having id '" + _dictWebsiteMapping["ID"] + "' does not exists.");
                        else throw new Exception("ID field having id '" + _dictWebsiteMapping["ID"] + "' is null.");
                        TextField _txtPassword = _ie.TextField(_dictWebsiteMapping["PASSWORD"]);
                        if (_txtPassword != null)
                            if (_txtPassword.Exists) _txtPassword.TypeText(cPassword);
                            else throw new Exception("Password field having id '" + _dictWebsiteMapping["PASSWORD"] + "' does not exists.");
                        else throw new Exception("Password field having id '" + _dictWebsiteMapping["PASSWORD"] + "' is null.");

                        _btnLogin.Click();

                        IECollection ies = new IECollection();
                        if (ies != null && ies.Count > 1)
                        {
                            foreach (var browser in ies)
                            {
                                if (browser.Url == convert.ToString(ConfigurationManager.AppSettings["NOTICE_POPUP_URL"]).Trim())
                                {
                                    browser.Dispose();                                    
                                }
                                else if (browser.Url == convert.ToString(ConfigurationManager.AppSettings["NOTICEPT_POPUP_URL"]).Trim())//added by kalpita on 19/02/2020
                                {
                                    browser.Dispose();
                                }
                            }
                        }
                    }
                    else
                    {
                        SetLog("");
                        SetLog("Unable to login to SK Shipping website.");
                        throw new Exception("Login button does not exists.");
                    }
                }
                else throw new Exception("Login button is null");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void getWebsiteMapping()
        {
            try
            {
                _dictWebsiteMapping.Clear();
                if (File.Exists(@MAPPath + @"\" + websiteMapFile))
                {
                    string[] _lines = File.ReadAllLines(@MAPPath + @"\" + websiteMapFile);
                    for (int i = 0; i < _lines.Length; i++)
                    {
                        string[] _keys = _lines[i].Split('=');
                        if (_keys.Length > 1) _dictWebsiteMapping.Add(_keys[0].Trim(), _keys[1].Trim());
                    }
                }
                else
                    SetLog("File named '" + websiteMapFile + "' not found on path '" + @MAPPath + "'");
            }
            catch (Exception ex)
            {
                SetLog("Exception in 'getWebsiteMapping()' function of 'SKShipping_Routine' class." + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString());
                throw;
            }
            finally
            {
                //
            }
        }

        #region Download RFQ
        public void ReadGrid()
        {
            try
            {
                SetLog("Successfull login to SK Shipping website.");
                string cGridURL = convert.ToString(ConfigurationManager.AppSettings["GRID_URL"]).Trim();
                _ie.gotopage(cGridURL);
                if (_ie.Url.ToString().Contains("MTIS/MTK350Q.aspx")) scanGrid(_ie);
                else SetLog("Unable to load url '" + cGridURL + "'");
                try
                {
                    if (_ie != null)
                    {
                        _ie.Dispose();
                        SetLog("Successfull closing of SK Shipping website." + Environment.NewLine);
                    }
                }
                catch { }
            }
            catch (Exception ex)
            {
                //SetLog("Exception in 'ReadGrid()' function of 'SKShipping_Routine' class - " + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString());
                throw ex;
            }
        }      
      
        public void scanGrid(IEBrowser _ie)
        {
            try
            {
                SetLog("Successfull loading url '" + convert.ToString(_ie.Url).Trim() + "'");
                SelectList sl = _ie.SelectList(_dictWebsiteMapping["CONFIRM_COMBO"]);
                if (sl.Exists)
                {
                    string val = sl.SelectedItem;
                    if (val.Trim().ToUpper() != "NO") sl.Options[2].Select();
                    if (convert.ToString(sl.SelectedItem).Trim().ToUpper() == "NO")
                    {
                        Div _divQuotation = _ie.Div(_dictWebsiteMapping["GRID_DIV"]);
                        if (_divQuotation.Exists)
                        {
                            if (_divQuotation.Tables.Count > 3)
                            {
                                Table tbl = _divQuotation.Tables[3];
                                TableCell tdclass = _ie.TableCell(Find.ByClass("pagenumber"));
                                if (tdclass.Exists)
                                {
                                    if (tdclass.Tables.Count > 0)
                                    {
                                        Table tblPaging = tdclass.Tables[0];
                                        int j = 0;
                                        if (tblPaging.Exists) j = tblPaging.Links.Count;
                                        SetLog("Number of Grid Pages found '" + j + "'");
                                        _detailUrls.Clear();
                                        _lstRFQ.Clear();
                                        for (int i = 0; i < j; i++)
                                        {
                                            Link lnkPage = tblPaging.Links[i];
                                            if (lnkPage.Exists)
                                            {
                                                lnkPage.Click();
                                                Thread.Sleep(2000);
                                                tbl = _divQuotation.Tables[3];
                                                tblPaging = tdclass.Tables[0];
                                                if (tbl.Exists) readURLs(tbl);
                                                else SetLog("Quotation grid not found on page '" + i + "' in div having id '" + _dictWebsiteMapping["GRID_DIV"] + "'.");
                                            }
                                            else SetLog("Paging link not found.");
                                        }
                                        if (_detailUrls.Count > 0) extractData(_detailUrls); 
                                    }
                                    else SetLog("Paging grid not found.");
                                }
                                else SetLog("Table cell field having class 'pagenumber' not found.");
                            }
                            else throw new Exception("Required number of tables not found in div having id '" + _dictWebsiteMapping["GRID_DIV"] + "'.");
                        }
                        else throw new Exception("Div field having id '" + _dictWebsiteMapping["GRID_DIV"] + "' not found.");
                    }
                }
                else throw new Exception("Combo field having id '" + _dictWebsiteMapping["CONFIRM_COMBO"] + "' not found.");
            }
            catch (Exception ex) {
                //SetLog("Exception in 'scanGrid()' function of 'SKShipping_Routine' class - " + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString());
                throw ex;
            }
        }

        public bool getDownloaded()
        {          
            bool found = false;
            _lstRFQDownloaded.Clear();
            string downloadedFile = convert.ToString(@AppDomain.CurrentDomain.BaseDirectory + Downloaded_FileName).Trim();
            if (File.Exists(downloadedFile))
            {
                found = true;
                string[] arrRFQDownloaded = File.ReadAllLines(downloadedFile, Encoding.ASCII);
                _lstRFQDownloaded = new List<string>(arrRFQDownloaded);
            }
            else
            {
                try
                {
                    File.Create(downloadedFile); found = true; SetLog(""); SetLog("File named '" + Downloaded_FileName + "' created on path '" + @AppDomain.CurrentDomain.BaseDirectory+"'");
                }
                catch { found = false; SetLog(""); SetLog("Exception while creating file named '" + Downloaded_FileName + "' on path '" + @AppDomain.CurrentDomain.BaseDirectory + "'"); }
            }
            return found;
        }

        public void extractData(List<string> _detailUrls)
        {            
            for (int k = 0; k < _detailUrls.Count; k++)
            {
                try
                {
                    _ie.gotopage(_detailUrls[k]);
                    Thread.Sleep(7000);
                    string url = convert.ToString(_ie.Url).Trim();
                    SetLog("Started fetching data from URL '" + url + "'");
                    DocType = "RFQ";
                    cVendorCode = ""; cVRNO = ""; cVRNO = _lstRFQ[k].Split('|')[0];
                    string port = _lstRFQ[k].Split('|')[1];
                    if (port.Contains("SINGAPORE")) cVendorCode = Recipient_Code_Singapore;
                    else cVendorCode = Recipient_Code;
                    this.PageGUID = cVRNO + "|" + _lstRFQ[k].Split('|')[2]+"|"+port;
                    if (!ifSubmittedRFQ())//
                    {
                        try
                        { FetchData(_ie); }
                        catch
                        {
                            setUpRFQError(cRFQErrorText);
                        }
                    }
                }
                catch(Exception ex){
                    setUpRFQError(ex.Message.ToString());               
                }
            }
        }

        public override Dictionary<string, string> GetHeaderData(IEBrowser _ie)
        {
            _xmlHeader = base.GetHeaderData(_ie);
            SetLog("RFQ '" + convert.ToString(_xmlHeader["BUYERREF"]).ToUpper() + "' of vessel '" + convert.ToString(_xmlHeader["VESSEL"]).ToUpper() + "' and port name '" + convert.ToString(_xmlHeader["PORTNAME"]).ToUpper() + "' is being downloaded.");
            SetLog("Started fetching Header data for RFQ '" + _xmlHeader["BUYERREF"]+"'");

            #region Header
            SelectList sl = _ie.SelectList(_dictWebsiteMapping["HEADER_CURRENCY_COMBO"]);
            string val="";
            if (sl.Exists) val = sl.SelectedItem;
            else SetLog("Combo field having id '" + _dictWebsiteMapping["HEADER_CURRENCY_COMBO"] + "' not found.");
            if (val.Trim().ToUpper().Contains("SELECT")) val = "";
            _xmlHeader.Add("CURRENCY", val);
            _xmlHeader.Add("DOC_TYPE", DocType);
            _xmlHeader.Add("DIALECT", Dialect);
            _xmlHeader.Add("SENDER_CODE", Sender_Code);
            string portName = "";
            if (_xmlHeader.ContainsKey("PORTNAME")) portName = convert.ToString(_xmlHeader["PORTNAME"]).ToUpper();
            if (portName.Contains("SINGAPORE")) { _xmlHeader.Add("RECIPIENT_CODE", Recipient_Code_Singapore); _xmlHeader.Add("QUALIFIER_SUPP_ADDR_NAME", Supplier_Name_Singapore); }
            else { _xmlHeader.Add("RECIPIENT_CODE", Recipient_Code); _xmlHeader.Add("QUALIFIER_SUPP_ADDR_NAME", Supplier_Name); }
            _xmlHeader.Add("DOCLINKID", _xmlHeader["BUYERREF"]);
            _xmlHeader.Add("ORIGDOCREFERENCE",convert.ToString(_ie.Url).Trim());
            #endregion

            #region Address
            _xmlHeader.Add("QUALIFIER_BYR_ADDR_NAME", Buyer_Name);          
            #endregion

            _xmlHeader.Select(str => str.Value.Replace("&", "&amp;"));
            SetLog("Completed fetching Header data.");
            return _xmlHeader;
        }

        public override List<List<string>> GetItemsData(IEBrowser _ie)
        {
            try
            {
                base.GetItemsData(_ie);
                SetLog("Started fetching Items data for RFQ '" + _xmlHeader["BUYERREF"] + "'");
                List<string> _lstItem = null;
                _lstItems.Clear();
                _xmlEquip.Clear();
                getItemMapping();
                Div _divItem = _ie.Div(_itemMapping["ITEM_TABLE"]);
                if (_divItem.Exists)
                {
                    if (_divItem.Tables.Count > 0)
                    {
                        Table tbl = null;
                        if (_ie.ContainsText("Machinery Information"))
                        {
                            SetLog("Equipment data found for RFQ '" + _xmlHeader["BUYERREF"] + "'");
                            tbl = _divItem.Tables[2];
                        }
                        else { SetLog("No equipment data found for RFQ '" + _xmlHeader["BUYERREF"] + "'"); tbl = _divItem.Tables[0]; }
                        int nRowCount = tbl.OwnTableRows.Count;

                        if (nRowCount > 3)
                        {
                            TableRow tr = tbl.TableRows[0];//to check if impa
                            TableRow tr2 = tbl.TableRows[1];//to check if partno
                            if (tr.Exists && convert.ToString(tr.OwnTableCells[3].Text).Trim().ToUpper() == "IMPA")
                            {
                                int ProductsCount = 0;
                                if ((nRowCount - 2) % 4 == 0) ProductsCount = (nRowCount - 2) / 4;
                                _xmlHeader.Add("TOTAL_LINEITEMS", convert.ToString(ProductsCount));
                                SetLog(convert.ToString(ProductsCount) + " Items found for RFQ '" + _xmlHeader["BUYERREF"] + "'");
                                SetLog("IMPA found.");
                                for (int i = 0; i < ProductsCount; i++)
                                {
                                    _lstItem = new List<string>();
                                    string itemRow = convert.ToString(i);
                                    string citemNo = "", cName = "", cUnit = "", cQuantity = "", cItemRef = "", cRemark = "", cEquipRemarks = "", cEquipment = "",
                                        cEquipMaker = "", cEquipType = "", cSpecRemarks = "", cUsedForRemarks = "";
                                    TextField t1 = _ie.TextField(_itemMapping["ITEM_NO"].Replace("#ROW#", itemRow));
                                    TextField t2 = _ie.TextField(_itemMapping["NAME"].Replace("#ROW#", itemRow));
                                    TextField t3 = _ie.TextField(_itemMapping["UNIT"].Replace("#ROW#", itemRow));
                                    TextField t4 = _ie.TextField(_itemMapping["QUANTITY"].Replace("#ROW#", itemRow));
                                    TextField t5 = _ie.TextField(_itemMapping["ITEMREF"].Replace("#ROW#", itemRow));
                                    TextField t6 = _ie.TextField(_itemMapping["REMARK"].Replace("#ROW#", itemRow));

                                    //added by kalpia on 17/04/2020
                                    TextField t7 = _ie.TextField(_itemMapping["SPECIFICATION_REMARK"].Replace("#ROW#", itemRow));
                                    TextField t8 = _ie.TextField(_itemMapping["USED_FOR_REMARK"].Replace("#ROW#", itemRow));

                                    if (t1.Exists) citemNo = convert.ToString(t1.Text).Trim(); else SetLog("Item number field having id '" + _itemMapping["ITEM_NO"].Replace("#ROW#", itemRow) + "' not found.");
                                    if (t2.Exists) cName = convert.ToString(t2.Text).Trim(); else SetLog("Name field having id '" + _itemMapping["NAME"].Replace("#ROW#", itemRow) + "' not found.");
                                    if (t3.Exists) cUnit = convert.ToString(t3.Text).Trim(); else SetLog("Unit field having id '" + _itemMapping["UNIT"].Replace("#ROW#", itemRow) + "' not found.");
                                    if (t4.Exists) cQuantity = convert.ToString(t4.Text).Trim(); else SetLog("Quantity field having id '" + _itemMapping["QUANTITY"].Replace("#ROW#", itemRow) + "' not found.");
                                    if (t5.Exists) cItemRef = convert.ToString(t5.Text).Trim(); else SetLog("Part Ref field having id '" + _itemMapping["ITEMREF"].Replace("#ROW#", itemRow) + "' not found.");
                                    if (t6.Exists) cRemark = convert.ToString(t6.Text).Trim(); else SetLog("Remark field having id '" + _itemMapping["REMARK"].Replace("#ROW#", itemRow) + "' not found.");
                                    
                                    //added by kalpia on 17/04/2020
                                    if (t7.Exists) cSpecRemarks = convert.ToString(" Specification Remark :" + t7.Text).Trim(); else SetLog("Specification Remark field having id '" + _itemMapping["SPECIFICATION_REMARK"].Replace("#ROW#", itemRow) + "' not found.");
                                    if (t8.Exists) cUsedForRemarks = convert.ToString(" Used For Remark :" + t8.Text).Trim(); else SetLog("Used for Remark field having id '" + _itemMapping["USED_FOR_REMARK"].Replace("#ROW#", itemRow) + "' not found.");

                                    _lstItem.Add(citemNo);//Number
                                    _lstItem.Add(citemNo);//OrigItemNumber
                                    _lstItem.Add(_itemMapping["ITEM_NO"].Replace("#ROW#", itemRow));//OriginatingSystemRef
                                    _lstItem.Add(cName);//Name
                                    _lstItem.Add(cUnit);//Unit
                                    _lstItem.Add(cQuantity);//Quantity
                                    _lstItem.Add(cItemRef);//ItemRef impa
                                    _lstItem.Add(cRemark + cSpecRemarks +","+ cUsedForRemarks);//Vessel Remarks,Specification Remark,Used for Remark(updated by kalpia on 17/04/2020)
                                    _lstItem.Add(cEquipRemarks);//Equip Remarks
                                    _lstItem.Add(convert.ToString(i + 1));//System Ref 
                                    _lstItem.Add(cEquipment);//Equipmentment
                                    _lstItem.Add(cEquipMaker);//Equip Maker
                                    _lstItem.Add(cEquipType);//Equip Type
                                    _lstItem.Select(str => str.Replace("&", "&amp;"));
                                    _lstItems.Add(_lstItem);
                                }
                                SetLog("Success in fetching Items data.");
                            }
                            else if (tr2.Exists && convert.ToString(tr2.OwnTableCells[1].Text).Trim().ToUpper() == "PART NO")
                            {
                                SetLog("PARTNO found.");
                                int totalTables = _divItem.Tables.Count;
                                int equipTables = totalTables / 3;
                                int itemTableIndex = 0, totalItemscount = 0,itemno=0;

                                for (int e = 0; e < equipTables; e++)
                                {
                                    _xmlEquip.Clear();
                                    GetEquipData(e);

                                    #region Equipment
                                    string cEquipment = "", cEquipMaker = "", cEquipType = "", cEquipRemarks3 = "";
                                    if (_xmlEquip.Count > 0)
                                    {
                                        if (_xmlEquip.ContainsKey("EQUIPMENT"))
                                            if (convert.ToString(_xmlEquip["EQUIPMENT"]) != "")
                                                cEquipment = _xmlEquip["EQUIPMENT"];
                                        if (_xmlEquip.ContainsKey("EQUIPMAKER"))
                                            if (convert.ToString(_xmlEquip["EQUIPMAKER"]) != "")
                                                cEquipMaker = _xmlEquip["EQUIPMAKER"];
                                        if (_xmlEquip.ContainsKey("EQUIPTYPE"))
                                            if (convert.ToString(_xmlEquip["EQUIPTYPE"]) != "")
                                                cEquipType = _xmlEquip["EQUIPTYPE"];
                                        if (_xmlEquip.ContainsKey("EQUIPREMARKS"))
                                            if (convert.ToString(_xmlEquip["EQUIPREMARKS"]) != "")
                                                cEquipRemarks3 = _xmlEquip["EQUIPREMARKS"];
                                    }
                                    #endregion

                                    int itemTable = (itemTableIndex += 3) - 1;
                                    if (_divItem.Tables.Count > itemTable)
                                    {
                                        tbl = _divItem.Tables[itemTable];
                                        nRowCount = tbl.OwnTableRows.Count;
                                        int ProductsCount = 0;
                                        if ((nRowCount - 2) % 4 == 0) ProductsCount = (nRowCount - 2) / 4;
                                        totalItemscount += ProductsCount;

                                        for (int i = 0; i < ProductsCount; i++)
                                        {
                                            itemno++;
                                            _lstItem = new List<string>();
                                            string itemRow = convert.ToString(i);
                                            string cName = "", cUnit = "", cQuantity = "", cItemRef = "", cRemark = "", cEquipRemarks = "", cEquipRemarks1 = "", cEquipRemarks2 = "";
                                            //TextField t1 = _ie.TextField(e + _itemMapping["ITEM_NO"].Replace("#ROW#", itemRow));
                                            TextField t2 = _ie.TextField(e + _itemMapping["NAME"].Replace("#ROW#", itemRow));
                                            TextField t3 = _ie.TextField(e + _itemMapping["UNIT"].Replace("#ROW#", itemRow));
                                            TextField t4 = _ie.TextField(e + _itemMapping["QUANTITY"].Replace("#ROW#", itemRow));
                                            TextField t5 = _ie.TextField(e + _itemMapping["PARTREF"].Replace("#ROW#", itemRow));
                                            TextField t6 = _ie.TextField(e + _itemMapping["REMARK"].Split('|')[0].Replace("#ROW#", itemRow));
                                            TextField t7 = _ie.TextField(e + _itemMapping["EQUIP_REMARKS"].Split('|')[0].Replace("#ROW#", itemRow));
                                            TextField t8 = _ie.TextField(e + _itemMapping["EQUIP_REMARKS"].Split('|')[1].Replace("#ROW#", itemRow));

                                            //if (t1.Exists) citemNo = convert.ToString(t1.Text).Trim(); else SetLog("Item number field having id '" + e + _itemMapping["ITEM_NO"].Replace("#ROW#", itemRow) + "' not found.");
                                            if (t2.Exists) cName = convert.ToString(t2.Text).Trim(); else SetLog("Name field having id '" + e + _itemMapping["NAME"].Replace("#ROW#", itemRow) + "' not found.");
                                            if (t3.Exists) cUnit = convert.ToString(t3.Text).Trim(); else SetLog("Unit field having id '" + e + _itemMapping["UNIT"].Replace("#ROW#", itemRow) + "' not found.");
                                            if (t4.Exists) cQuantity = convert.ToString(t4.Text).Trim(); else SetLog("Quantity field having id '" + e + _itemMapping["QUANTITY"].Replace("#ROW#", itemRow) + "' not found.");
                                            if (t5.Exists) cItemRef = convert.ToString(t5.Text).Trim(); else SetLog("Part Ref field having id '" + e + _itemMapping["PARTREF"].Replace("#ROW#", itemRow) + "' not found.");
                                            if (t6.Exists) cRemark = convert.ToString(t6.Text).Trim(); else SetLog("Remark field having id '" + e + _itemMapping["REMARK"].Replace("#ROW#", itemRow) + "' not found.");
                                            if (t7.Exists) cEquipRemarks1 = convert.ToString(t7.Text).Trim(); else SetLog("Equip Remark field having id '" + e + _itemMapping["EQUIP_REMARKS"].Split('|')[0].Replace("#ROW#", itemRow) + "' not found.");
                                            if (t8.Exists) cEquipRemarks2 = convert.ToString(t8.Text).Trim(); else SetLog("Equip Remark field having id '" + e + _itemMapping["EQUIP_REMARKS"].Split('|')[1].Replace("#ROW#", itemRow) + "' not found.");

                                            cEquipRemarks = cEquipRemarks1 + Environment.NewLine + cEquipRemarks2 + Environment.NewLine + cEquipRemarks3;

                                            _lstItem.Add(convert.ToString(itemno));//Number
                                            _lstItem.Add(convert.ToString(itemno));//OrigItemNumber
                                            _lstItem.Add(convert.ToString(e + _itemMapping["ITEM_NO"].Replace("#ROW#", itemRow)));//OriginatingSystemRef
                                            _lstItem.Add(cName);//Name
                                            _lstItem.Add(cUnit);//Unit
                                            _lstItem.Add(cQuantity);//Quantity
                                            _lstItem.Add(cItemRef);//ItemRef partno
                                            _lstItem.Add(cRemark);//Vessel Remarks
                                            _lstItem.Add(cEquipRemarks.Trim());//Equip Remarks
                                            _lstItem.Add(convert.ToString(itemno));//System Ref
                                            _lstItem.Add(cEquipment);//Equipmentment
                                            _lstItem.Add(cEquipMaker);//Equip Maker
                                            _lstItem.Add(cEquipType);//Equip Type
                                            _lstItem.Select(str => str.Replace("&", "&amp;"));
                                            _lstItems.Add(_lstItem);
                                        }
                                    }
                                }
                                SetLog(convert.ToString(totalItemscount) + " Items found for RFQ '" + _xmlHeader["BUYERREF"] + "'");
                                _xmlHeader.Add("TOTAL_LINEITEMS", convert.ToString(totalItemscount));
                                SetLog("Success in fetching Items data.");
                            }
                        }
                        else SetLog("No items found for RFQ '" + _xmlHeader["BUYERREF"] + "'");
                    }
                    else SetLog("Required number of tables not found in div '" + _itemMapping["ITEM_TABLE"] + "'.");
                }
                else SetLog("Div field having id '" + _itemMapping["ITEM_TABLE"] + "' not found.");
                SetLog("Completed fetching Items data.");
            }
            catch (Exception ex)
            {
                SetLog("Exception in 'GetItemsData()' function of 'SKShipping_Routine' class." + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString());
                throw;
            }
            return _lstItems;
        }

        public override void ExporttoHeader(Dictionary<string, string> _xmlHeader, LeSXML.LeSXML _lesXML)
        {
            base.ExporttoHeader(_xmlHeader, _lesXML);
            if (_xmlHeader.Count > 0)
            {
                #region Header
                SetLog("Started extracting Header data.");
                _lesXML.Doc_Type = _xmlHeader["DOC_TYPE"];
                _lesXML.Dialect = _xmlHeader["DIALECT"];
                _lesXML.Sender_Code = _xmlHeader["SENDER_CODE"];
                _lesXML.Recipient_Code = _xmlHeader["RECIPIENT_CODE"];
                _lesXML.DocReferenceID = _xmlHeader["DOCREFERENCEID"];
                _lesXML.DocLinkID = _xmlHeader["ORIGDOCREFERENCE"];
                _lesXML.OrigDocReference = _xmlHeader["DOCLINKID"];
                _lesXML.OrigDocFile = Path.GetFileName(this.ImgFile);
                _lesXML.Vessel = _xmlHeader["VESSEL"];
                _lesXML.BuyerRef = _xmlHeader["BUYERREF"];
                _lesXML.PortName = _xmlHeader["PORTNAME"];
                _lesXML.Currency = _xmlHeader["CURRENCY"];
                if (convert.ToString(_xmlHeader["DATE_ETA"]) != "")
                    _lesXML.Date_ETA = convert.ToDateTime(_xmlHeader["DATE_ETA"]).ToString("yyyyMMdd");
                _lesXML.Remark_Header = _xmlHeader["REMARK_HEADER"];
                _lesXML.Total_LineItems = (_xmlHeader.ContainsKey("TOTAL_LINEITEMS")) ? _xmlHeader["TOTAL_LINEITEMS"] : "0";
                SetLog("Completed extracting Header data.");
                #endregion

                #region Address
                SetLog("Started extracting Address data.");
                _lesXML.Addresses.Add(new LeSXML.Address());
                _lesXML.Addresses.Add(new LeSXML.Address());
                _lesXML.Addresses[0].Qualifier = "BY";
                _lesXML.Addresses[0].AddressName = _xmlHeader["QUALIFIER_BYR_ADDR_NAME"];
                _lesXML.Addresses[0].ContactPerson=_xmlHeader["BUYER_CONTACT_PERSON"];
                _lesXML.Addresses[1].Qualifier = "VN";
                _lesXML.Addresses[1].AddressName = _xmlHeader["QUALIFIER_SUPP_ADDR_NAME"];
                SetLog("Completed extracting Address data.");
                #endregion
            }

            #region Equipment
            //if (_xmlEquip.Count > 0)
            //{
            //    SetLog("Started extracting Equipment data.");
            //    if (_xmlEquip.ContainsKey("EQUIPMENT"))
            //        if (convert.ToString(_xmlEquip["EQUIPMENT"]) != "")
            //            _lesXML.Equipment = _xmlEquip["EQUIPMENT"];
            //    if (_xmlEquip.ContainsKey("EQUIPMAKER"))
            //        if (convert.ToString(_xmlEquip["EQUIPMAKER"]) != "")
            //            _lesXML.EquipMaker = _xmlEquip["EQUIPMAKER"];
            //    if (_xmlEquip.ContainsKey("EQUIPTYPE"))
            //        if (convert.ToString(_xmlEquip["EQUIPTYPE"]) != "")
            //            _lesXML.EquipType = _xmlEquip["EQUIPTYPE"];
            //    if (_xmlEquip.ContainsKey("EQUIPREMARKS"))
            //        if (convert.ToString(_xmlEquip["EQUIPREMARKS"]) != "")
            //            _lesXML.EquipRemarks = _xmlEquip["EQUIPREMARKS"];
            //    SetLog("Completed extracting Equipment data.");
            //}
            #endregion

            _lesXML.FileName = convert.ToFileName("RFQ_" + _lesXML.Recipient_Code + "_" + _lesXML.BuyerRef.Replace("/", "_") + "_" + FileDateTimeStamp + ".xml");
        }

        public override void ExporttoItems(List<List<string>> _xmlItems, LeSXML.LeSXML _lesXML)
        {
            base.ExporttoItems(_xmlItems, _lesXML);        
            if (_xmlItems.Count > 0)
            {
                SetLog("Started extracting Items data.");
                for (int i = 0; i < _xmlItems.Count; i++)
                {
                    LeSXML.LineItem Item = new LeSXML.LineItem();
                    Item.Number = _xmlItems[i][0];
                    Item.OrigItemNumber = _xmlItems[i][1];
                    Item.OriginatingSystemRef = _xmlItems[i][2];
                    Item.Name = _xmlItems[i][3];
                    Item.Unit = _xmlItems[i][4];
                    Item.Quantity = _xmlItems[i][5];
                    Item.ItemRef = _xmlItems[i][6];
                    Item.Remark = _xmlItems[i][7];
                    Item.EquipRemarks = _xmlItems[i][8];
                    Item.SystemRef = _xmlItems[i][9];
                    Item.Equipment = _xmlItems[i][10];
                    Item.EquipMaker = _xmlItems[i][11];
                    Item.EquipType = _xmlItems[i][12];
                    _lesXML.LineItems.Add(Item);
                }
                SetLog("Completed extracting Items data.");
            }
            else {base.nItemCount = 0; }
        }

        public override void SavePage(IEBrowser _ie, bool SavePNG, string PrintURL)
        {
            if (DocType.ToUpper() == "RFQ")
            {
                this.HtmlFile = convert.ToFileName(Domain + "_" + cVRNO.Replace("/", "_") + "_" + FileDateTimeStamp + ".htm");
                this.ImgFile = convert.ToFileName(Domain + "_" + cVRNO.Replace("/", "_") + "_" + FileDateTimeStamp + ".png");
            }
            if (DocType.ToUpper() == "QUOTE")
            {
                this.HtmlFile = convert.ToFileName(Domain + "_" + cVRNO.Replace("/", "_") + "_" + FileDateTimeStamp + ".htm");
                this.ImgFile = convert.ToFileName(Domain + "_" + cVRNO.Replace("/", "_") + "_" + FileDateTimeStamp + ".png");
            }
            PrintURL = "";
            string _source = _ie.Body.Parent.OuterHtml;
            _source = _source.Replace("\"/Common/images", "\"../Common/images");
            this.PageSource = _source;
            base.SavePage(_ie, SavePNG, PrintURL);
        }

        private void getItemMapping()
        {
            try
            {
                _itemMapping.Clear();
                if (File.Exists(@MAPPath + @"\" + ItemsMapFile))
                {
                    string[] _lines = File.ReadAllLines(@MAPPath + @"\" + ItemsMapFile);
                    for (int i = 0; i < _lines.Length; i++)
                    {
                        string[] _keys = _lines[i].Split('=');
                        if (_keys.Length > 1) _itemMapping.Add(_keys[0].Trim(), _keys[1].Trim());
                    }
                }
                else SetLog("File named '" + ItemsMapFile + "' not found on path '" + @MAPPath + "'");
            }
            catch (Exception ex)
            {
                SetLog("Exception in 'getItemMapping()' function of 'SKShipping_Routine' class." + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString());
            }
            finally
            {
                //
            }
        }

        private void GetEquipData(int rowcount)
        {
            try
            {
                if (File.Exists(@MAPPath + @"\" + EquipmentMapFile))
                {
                    string[] _lines = File.ReadAllLines(@MAPPath + @"\" + EquipmentMapFile);
                    for (int i = 0; i < _lines.Length; i++)
                    {
                        string[] _keys = _lines[i].Split('=');
                        if (_keys.Length > 1)
                        {
                            string[] _values = _keys[1].Split('|');
                            string _value = "";
                            if (_values.Length > 0)
                            {
                                for (int k = 0; k < _values.Length; k++) _value += _ie.GetText(_values[k] + rowcount) + Environment.NewLine;
                            }
                            if (String.IsNullOrEmpty(_value)) { _value = string.Empty; }
                            _xmlEquip.Add(_keys[0], _value.Trim());
                        }
                    }
                }
                else SetLog("File named '" + EquipmentMapFile + "' not found on path '" + @MAPPath + "'");
            }
            catch (Exception ex)
            {
                SetLog("Exception in 'GetEquipData()' function of 'SKShipping_Routine' class." + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString());
            }
            finally
            {
                //
            }
        }

        public void readURLs(Table tbl)
        {
            try
            {
                SetLog("Started reading Grid.");
                int nRowCount = tbl.OwnTableRows.Count;
                for (int i = 0; i < nRowCount; i++)
                {
                    TableRow tr = tbl.TableRows[i];
                    string kind = convert.ToString(tr.OwnTableCells[2].Text).Trim();
                    string vessel = convert.ToString(tr.OwnTableCells[3].Text).Trim();
                    string quono = convert.ToString(tr.OwnTableCells[4].Text).Trim();
                    string reqno = convert.ToString(tr.OwnTableCells[5].Text).Trim();
                    string eta = convert.ToString(tr.OwnTableCells[6].Text).Trim();
                    string port = convert.ToString(tr.OwnTableCells[7].Text).Trim();
                    string quodate = convert.ToString(tr.OwnTableCells[8].Text).Trim();
                    string seq = convert.ToString(tr.OwnTableCells[9].Text).Trim();//24-4-2018
                    string chkDownloaded = quono.ToUpper() + "|" + vessel.ToUpper() + "|" + port.ToUpper();
                    string _detailUrl = "";
                    if (_lstRFQDownloaded.IndexOf(chkDownloaded) == -1)
                    {
                        Link ln = tr.Links[0]; ;
                        string l = ln.GetAttributeValue("onclick");
                        string[] srr = l.Split(',');
                        //_detailUrl = convert.ToString(ConfigurationManager.AppSettings["DETAIL_URL"]).Trim() + srr[1].Replace("'", "") + "?KIND=" + kind + "&VESSEL=" + vessel + "&QUONO=" + quono + "&REQNO=" + reqno + "&ETA=" + eta + "&PORT=" + port + "&QUODATE=" + quodate + "&SEQ=0";//24-04-18
                        _detailUrl = convert.ToString(ConfigurationManager.AppSettings["DETAIL_URL"]).Trim() + srr[1].Replace("'", "") + "?KIND=" + kind + "&VESSEL=" + vessel + "&QUONO=" + quono + "&REQNO=" + reqno + "&ETA=" + eta + "&PORT=" + port + "&QUODATE=" + quodate + "&SEQ="+seq;//24-04-18
                        if (!_detailUrls.Contains(_detailUrl.Trim().ToString())) {
                            _lstRFQ.Add(quono + "|" + port+"|"+vessel);//24-04-18
                            //_lstRFQ.Add(quono + "|" + port);//24-04-18
                            _detailUrls.Add(_detailUrl.Trim().ToString()); 
                        }
                    }
                    else SetLog("RFQ '" + quono.ToUpper() + "' of vessel '" + vessel.ToUpper() + "' and port name '" + port.ToUpper() + "' is already downloaded.");
                }
                SetLog("Completed reading Grid.");
            }
            catch(Exception ex){
                SetLog("Exception in 'readURLs()' function of 'SKShipping_Routine' class - " + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString());
               
            }
        }

        public void setUpRFQError(string messg)
        {
            ifErrorFile = true;            
            AddToAudit(Sender_Code, cVendorCode, Module, "", cVRNO, "Error",messg);        
            string _source = _ie.Body.Parent.OuterHtml;
            _source = _source.Replace("\"/Common/images", "\"../Common/images");
            this.PageSource = _source;
            SavePage(_ie, true, "");           
        }

        private bool ifSubmittedRFQ()
        {
            bool ifsubmit = true;
            try
            {
                Image submitButton = _ie.Image(_dictWebsiteMapping["SUBMIT_BUTTON"]);
                Image saveButton = _ie.Image(_dictWebsiteMapping["SAVE_BUTTON"]);
                if (submitButton != null && saveButton != null)
                {
                    TextField suppRefNo = null;
                    if (_ie.ContainsText("Repair")) { ifsubmit = false; base.cRfqType = "Repair"; }
                    else
                    {
                        if (_ie.ContainsText("Machinery Information")) suppRefNo = _ie.TextField(_dictWebsiteMapping["SUPPLIER_REFNO_SPARE_TEXTFIELD"]);
                        else suppRefNo = _ie.TextField(_dictWebsiteMapping["SUPPLIER_REFNO_STORE_TEXTFIELD"]);
                        bool ifSaved = false;
                        if (suppRefNo != null)
                            if (suppRefNo.Exists)
                                if (suppRefNo.ClassName != null)
                                    if (suppRefNo.ClassName.Contains("inputbox_readonly")) ifSaved = true;
                                    else ifSaved = false;
                                else throw new Exception("Supplier Ref No field does not have class.");
                            else
                            {
                                if (this.PageGUID.Trim().Length > 0) SetGUIDs(this.PageGUID.Trim());
                                throw new Exception("Supplier Ref No field does not exists.");
                            }
                        else throw new Exception("Supplier Ref No field is null.");
                        if (submitButton.Exists && saveButton.Exists && !ifSaved) ifsubmit = false;
                        else SetLog("RFQ '" + cVRNO + "' is not downloaded as the page does not contain submit and save button and the supplier ref no field is readonly.");
                    }
                }
                else throw new Exception("RFQ '" + cVRNO + "' is not downloaded as the submit and save button is null.");
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ifsubmit;
        }
        #endregion

        #region Upload Quote
        public bool checkQuoteFiles()
        {
            bool res = false;
            try
            {
                DirectoryInfo _dirInfo = new DirectoryInfo(QuotePOCPath);
                if (!_dirInfo.Exists) _dirInfo.Create();
                FileInfo[] cxmlFiles = _dirInfo.GetFiles("*.xml");
                if (cxmlFiles.Length > 0) res = true;
                else SetLog("No files found on path " + QuotePOCPath + " to upload.");
            }
            catch (Exception ex) { SetLog("Exception in 'checkQuoteFiles()' function of 'SKShipping_Routine' class - " + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString()); }
            return res;
        }

        public void getQuoteFile()
        {
            try
            {
                SetLog("Successfull login to SK Shipping website.");
                DirectoryInfo _dirInfo = new DirectoryInfo(QuotePOCPath);
                if (!_dirInfo.Exists) _dirInfo.Create();
                FileInfo[] cxmlFiles = _dirInfo.GetFiles("*.xml");
                if (cxmlFiles.Length > 0)
                {
                    foreach (FileInfo xmlFile in cxmlFiles) processQuoteFile(xmlFile);
                }
                else SetLog("No files found on path " + QuotePOCPath + " to upload.");
                try
                {
                    _ie.Dispose();
                    SetLog("Successfull closing of SK Shipping website.");
                }
                catch { }
            }
            catch (Exception ex) { SetLog("Exception in 'uploadQuotes()' function of 'SKShipping_Routine' class - " + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString()); }
        }

        public void processQuoteFile(FileInfo xmlFile)
        {
            try
            {
                SetLog("Started processing Quote named " + xmlFile.Name + " present on path " + xmlFile.DirectoryName);
                MTMLClass _mtmlClass = new MTMLClass();
                MTMLInterchange objMTML = _mtmlClass.Load(xmlFile.FullName);
                if (objMTML != null)
                {
                    string cDocType = convert.ToString(objMTML.DocumentHeader.DocType).ToUpper().Trim();
                    if (cDocType == "QUOTE" || cDocType == "QUOTATION")
                    {
                        DocType = "QUOTE";
                        bool res = false;
                        res = uploadQuote(objMTML, xmlFile);
                        if (res)
                        {
                            createNotifyFile(cVendorCode, cVRNO, cSuppRef, xmlFile, "SUCCESS");//19-07-2017
                            SetLog("Quote named " + xmlFile.Name + " uploaded successfully.");
                            if (File.Exists(xmlFile.FullName)) MoveFile(xmlFile.DirectoryName + "\\Backup", xmlFile);
                            this.HtmlFile = convert.ToFileName(Domain + "_" + cVRNO.Replace("/", "_") + "_" + FileDateTimeStamp + ".htm");
                            this.ImgFile = convert.ToFileName(Domain + "_" + cVRNO.Replace("/", "_") + "_" + FileDateTimeStamp + ".png");
                            AddToAudit(cBuyerCode, cVendorCode, Domain + "_" + DocType, xmlFile.Name, cVRNO, "Uploaded", LogDateTimeStamp + " - Quote named " + xmlFile.Name + " uploaded successfully.");
                            string _source = _ie.Body.Parent.OuterHtml;
                            _source = _source.Replace("\"/Common/images", "\"../Common/images");
                            this.PageSource = _source;
                            ifErrorFile = false;
                            base.SavePage(_ie, true, "");
                        }
                        else
                        {
                            createNotifyFile(cVendorCode, cVRNO, cSuppRef, xmlFile, "FAIL");//19-07-2017
                            SetLog("Error uploading Quote named " + xmlFile.Name);
                            setUpQuoteError(xmlFile, LogDateTimeStamp + " - Error uploading Quote named " + xmlFile.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SetLog("Error uploading Quote named " + xmlFile.Name + Environment.NewLine + "Error - " + ex.Message);
                SetLog(ex.StackTrace);
                setUpQuoteError(xmlFile, "Error uploading Quote named " + xmlFile.Name + "; " + ex.Message);
            }
        }

        private void createNotifyFile(string cVendorCode, string cVRNO, string cSuppRef, FileInfo xmlFile, string RESULT)
        {
            try
            {
                string notifyFileName = cVendorCode + "_" + FileDateTimeStamp + ".txt";
                string message = cVRNO + "|" + cSuppRef + "=" + RESULT;
                File.WriteAllText(NotificationPath + "\\" + notifyFileName, message.Trim());
            }
            catch (Exception ex)
            {
                SetLog("Error creating notification file for Quote named " + xmlFile.Name + Environment.NewLine + "Error - " + ex.Message.ToString());
            }
        }

        private bool uploadQuote(MTMLInterchange objMTML, FileInfo xmlFile)
        {
            bool res = false;
            try
            {
                cVRNO = ""; cBuyerCode = ""; cVendorCode = ""; cMessgNo = ""; cSuppRef = ""; cVRNO = "";
                //chkAllID = "";//commented on 12-01-2018
                chkAllID = new List<string>();//added on 12-01-2018
                cBuyerCode = convert.ToString(objMTML.Recipient).Trim();
                cVendorCode = convert.ToString(objMTML.Sender).Trim();
                cMessgNo = convert.ToString(objMTML.DocumentHeader.MessageNumber).Trim();
                foreach (Reference _ref in objMTML.DocumentHeader.References)
                {
                    if (_ref.Qualifier == ReferenceQualifier.AAG) cSuppRef = _ref.ReferenceNumber;
                    else if (_ref.Qualifier == ReferenceQualifier.UC) cVRNO = _ref.ReferenceNumber;
                }
                SetLog("Uploading Quote of RFQ '" + cVRNO + "'");
                _ie.gotopage(cMessgNo);
                getItemMapping();
                if (!ifSubmittedQuote(xmlFile))     if (loadQuoteDetails(objMTML, xmlFile)) res = FillQuoteDetails(objMTML, xmlFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }

        private bool loadQuoteDetails(MTMLInterchange objMTML, FileInfo xmlFile)
        {
            bool res = false;
            try
            {
                cCurrencyCode = convert.ToString(objMTML.DocumentHeader.CurrencyCode).Trim();
                dAdditionalDisc = convert.ToDouble(objMTML.DocumentHeader.AdditionalDiscount);
                dtValidFrom = DateTime.Now.ToString("yyyy-MM-dd");
                foreach (DateTimePeriod dtperiod in objMTML.DocumentHeader.DateTimePeriods)
                {
                    if (dtperiod.Qualifier == DateTimePeroidQualifiers.ExpiryDate_36)
                        if (dtperiod.Value.Trim() != "")
                        {
                            DateTime dtValid = FormatMTMLDate(dtperiod.Value.Trim());
                            if (dtValid != DateTime.MinValue) dtValidTo = dtValid.ToString("yyyy-MM-dd");
                        }

                }
                foreach (Comments comm in objMTML.DocumentHeader.Comments)
                {
                    if (comm.Qualifier == CommentTypes.SUR) cQuotationRemark = convert.ToString(comm.Value).Trim();
                }

                foreach (MonetaryAmount monAmt in objMTML.DocumentHeader.MonetoryAmounts)
                {
                    if (monAmt.Qualifier == MonetoryAmountQualifier.GrandTotal_259) grandTotal = convert.ToDouble(monAmt.Value);
                }
                res = true;
            }
            catch (Exception ex)
            {
                SetLog("Exception in 'loadQuoteDetails()' function of 'SKShipping_Routine' class - " + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString());
                throw;
            }
            return res;
        }

        public DateTime FormatMTMLDate(string DateValue)
        {
            DateTime Dt = DateTime.MinValue;
            try
            {
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
            }
            catch (Exception ex)
            {
                SetLog("Exception in 'FormatMTMLDate()' function of 'SKShipping_Routine' class - " + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString());
            }
            return Dt;
        }

        public bool FillQuoteDetails(MTMLInterchange objMTML, FileInfo xmlFile)
        {
            bool res = false;
            try
            {
                SetLog("Filling Header");
                if (FillQuoteHeader())
                {
                    #region Filling Items
                    SetLog("Checking Total Items count match.");
                    //if (ifTotalItemsMatch(objMTML))
                    //{
                    SetLog("Filling Items");
                    if (FillItems(objMTML, xmlFile))//xmlFile//added on 06-04-2018
                    {
                        if (allConfirm())//19-07-2017
                        {
                            if (saveQuote()) //Commented for testing
                            {
                                //19-07-2017
                                while (_ie.Text == null)
                                {
                                    Thread.Sleep(2000);
                                }
                                while (_ie.Text != null && _ie.Text.Contains("Please wait, now loading..."))
                                {
                                    Thread.Sleep(2000);
                                }
                                Thread.Sleep(5000);//03-11-2017
                                //19-07-2017
                                if (ifnoMismatch(xmlFile))
                                {
                                    res = submitQuote();//Commented for testing                                   
                                    //19-07-2017
                                    while (_ie.Text == null)
                                    {
                                        Thread.Sleep(2000);
                                    }
                                    while (_ie.Text.Contains("Please wait, now loading..."))
                                    {
                                        Thread.Sleep(2000);
                                    }
                                    //19-07-2017
                                    Thread.Sleep(5000);//03-11-2017
                                }
                            }
                        }
                    }
                    //}                    
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }

        public bool ifnoMismatch(FileInfo xmlFile)
        {
            bool res = false;
            try
            {
                double tgrandttl = 0, diff = 0;
                TextField tgrandTotal = _ie.TextField(_dictWebsiteMapping["GRAND_TOTAL_TEXTFIELD"]);
                if (tgrandTotal.Exists) tgrandttl = convert.ToDouble(tgrandTotal.Value);

                if (tgrandttl > grandTotal) diff = convert.ToDouble(tgrandttl - grandTotal);//19-07-2017
                else if (grandTotal > tgrandttl) diff = convert.ToDouble(grandTotal - tgrandttl);//19-07-2017

                if (diff <= 1) res = true;//19-07-2017
                else throw new Exception("Error uploading Quote of RFQ '" + cVRNO + "' due to Total Amount '" + Convert.ToString(tgrandttl) + "' mismatch on site Grand Total '" + grandTotal + "'.");
            }
            catch (Exception ex) { throw ex; }
            return res;
        }

        private bool submitQuote()
        {
            bool res = false;
            try
            {
                Image imgSubmit = _ie.Image(_dictWebsiteMapping["SUBMIT_BUTTON"]);
                if (imgSubmit != null)
                    if (imgSubmit.Exists)
                    {
                        //19-07-2017
                        imgSubmit.ClickNoWait();
                        Thread.Sleep(2000);

                        HtmlDialog htmlDialog = _ie.HtmlDialog(Find.ByUrl(url => url.StartsWith(convert.ToString(ConfigurationManager.AppSettings["POPUP_URL"]).Trim())));
                        Link lnkpopup = htmlDialog.Links[0];
                        lnkpopup.ClickNoWait();                        
                        Thread.Sleep(2000);

                        //19-07-2017
                        res = true;
                    }
                    else throw new Exception("Submit button does not exists.");
                else throw new Exception("Submit button is null.");
            }
            catch (Exception ex)
            {
                SetLog("Exception in 'submitQuote()' function - " + ex.Message);
                SetLog(ex.StackTrace);
                throw ex;
            }
            return res;
        }

        private bool saveQuote()
        {
            bool res = false;
            try
            {
                Image imgSave = _ie.Image(_dictWebsiteMapping["SAVE_BUTTON"]);
                if (imgSave != null)
                    if (imgSave.Exists)
                    {
                        //19-07-2017
                        imgSave.ClickNoWait();
                        Thread.Sleep(2000);

                        HtmlDialog htmlDialog = _ie.HtmlDialog(Find.ByUrl(url => url.StartsWith(convert.ToString(ConfigurationManager.AppSettings["POPUP_URL"]).Trim())));
                        Link lnkpopup = htmlDialog.Links[0];                       
                        lnkpopup.ClickNoWait();
                        Thread.Sleep(2000);

                        //19-07-2017
                        res = true;
                    }
                    else throw new Exception("Save button does not exists.");
                else throw new Exception("Save button is null.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }

        //19-07-2017
        private bool allConfirm()
        {
            bool res = false;
            try
            {
                foreach (string chkbx in chkAllID)
                {
                    //CheckBox chkAllBx = _ie.CheckBox(chkAllID);//commented on 12-01-2018
                    CheckBox chkAllBx = _ie.CheckBox(chkbx);
                    if (chkAllBx != null)
                        if (chkAllBx.Exists)
                        {
                            chkAllBx.Click();
                            res = true;
                        }
                        else throw new Exception("Confirm checkbox does not exists.");
                    else throw new Exception("Confirm checkbox is null.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }
        //19-07-2017

        private bool FillItems(MTMLInterchange objMTML, FileInfo xmlFile)
        {
            bool res = false;
            try
            {
                if (objMTML.DocumentHeader.LineItemCount > 0)
                {
                    _lineItems = objMTML.DocumentHeader.LineItems;
                    MTML.GENERATOR.LineItem _item = null;

                    Div _divItem = _ie.Div(_itemMapping["ITEM_TABLE"]);
                    if (_divItem != null)
                    {
                        if (_divItem.Exists)
                        {
                            if (_divItem.Tables.Count > 0)
                            {
                                Table tbl = null;
                                if (_ie.ContainsText("Machinery Information")) tbl = _divItem.Tables[2];
                                else tbl = _divItem.Tables[0];
                                int nRowCount = tbl.OwnTableRows.Count;
                                int ProductsCount = 0;
                                if ((nRowCount - 2) % 4 == 0) ProductsCount = (nRowCount - 2) / 4;
                                if (ProductsCount == objMTML.DocumentHeader.LineItemCount) res = true;
                                if (nRowCount > 3)
                                {
                                    SetLog(convert.ToString(ProductsCount) + " Items found for Quote having RFQ '" + cVRNO + "'");
                                    TableRow tr = tbl.TableRows[0];//to check if impa
                                    TableRow tr2 = tbl.TableRows[1];//to check if partno
                                    int i = 0;
                                    if (tr.Exists && convert.ToString(tr.OwnTableCells[3].Text).Trim().ToUpper() == "IMPA")
                                    {
                                        SetLog("IMPA found.");
                                        //chkAllID = _itemMapping["CHECK_ALL"];//19-07-2017commented on 12-08-2018
                                        chkAllID.Add(_itemMapping["CHECK_ALL"]);
                                    }
                                    else
                                    {
                                        SetLog("PARTNO found.");
                                        //chkAllID = _itemMapping["CHECK_ALL"] + "0"; //19-07-2017commented on 12-08-2018                                       
                                    }
                                    foreach (MTML.GENERATOR.LineItem _lineItem in _lineItems)
                                    {
                                        if (tr.Exists && convert.ToString(tr.OwnTableCells[3].Text).Trim().ToUpper() == "IMPA")
                                        {
                                            string itemRow = convert.ToString(i);
                                            string cItemPrice = "0.00", cItemDeliveryTerm = "0", cSupplierRemark = "";
                                            string citemNo = "";
                                            TextField t1 = _ie.TextField(_itemMapping["ITEM_NO"].Replace("#ROW#", itemRow));
                                            TextField t2 = _ie.TextField(_itemMapping["ITEM_PRICE"].Replace("#ROW#", itemRow));
                                            TextField t3 = _ie.TextField(_itemMapping["DELIVERY_TERM"].Replace("#ROW#", itemRow));
                                            TextField t4 = _ie.TextField(_itemMapping["SUPPLIER_REMARK"].Replace("#ROW#", itemRow));
                                            if (t1 != null)
                                                if (t1.Exists) citemNo = convert.ToString(t1.Text).Trim();
                                                else
                                                {
                                                    createNotifyFile(cVendorCode, cVRNO, cSuppRef, xmlFile, "FAIL");//06-04-2018//added as fail notify msg not send to rms
                                                    throw new Exception("Item number field having id '" + _itemMapping["ITEM_NO"].Replace("#ROW#", itemRow) + "' does not exists.");
                                                }
                                            else throw new Exception("Item number field having id '" + _itemMapping["ITEM_NO"].Replace("#ROW#", itemRow) + "' is null.");

                                            //if (citemNo == convert.ToString(_lineItem.SYS_ITEMNO))
                                            //{
                                            //    _item = _lineItem;
                                            //    if (_item != null)
                                            //    {
                                            //        #region Item Price
                                            //        foreach (PriceDetails _priceDet in _item.PriceList)
                                            //            if (_priceDet.TypeQualifier == PriceDetailsTypeQualifiers.GRP) cItemPrice = _priceDet.Value.ToString("0.00");
                                            //        if (t2 != null)
                                            //            if (t2.Exists)
                                            //                if (t2.ClassName != null)
                                            //                    if (t2.ClassName.Contains("inputbox_readonly")) throw new Exception("Price field having id '" + _itemMapping["ITEM_PRICE"].Replace("#ROW#", itemRow) + "' is readonly.");
                                            //                    else t2.Value = cItemPrice;
                                            //                else throw new Exception("Price field having id '" + _itemMapping["ITEM_PRICE"].Replace("#ROW#", itemRow) + "' does not have class.");
                                            //            else throw new Exception("Price field having id '" + _itemMapping["ITEM_PRICE"].Replace("#ROW#", itemRow) + "' does not exists.");
                                            //        else throw new Exception("Price field having id '" + _itemMapping["ITEM_PRICE"].Replace("#ROW#", itemRow) + "' is null.");
                                            //        #endregion

                                            //        #region Delivery Term
                                            //        if (convert.ToInt(_item.DeleiveryTime) > 0) cItemDeliveryTerm = convert.ToString(_item.DeleiveryTime);
                                            //        else cItemDeliveryTerm = convert.ToString(objMTML.DocumentHeader.LeadTimeDays).Trim();
                                            //        if (t3 != null)
                                            //            if (t3.Exists)
                                            //                if (t3.ClassName != null)
                                            //                    if (t3.ClassName.Contains("inputbox_readonly")) throw new Exception("Delivery Term field having id '" + _itemMapping["DELIVERY_TERM"].Replace("#ROW#", itemRow) + "' is readonly.");
                                            //                    else t3.Value = cItemDeliveryTerm;
                                            //                else throw new Exception("Delivery Term field having id '" + _itemMapping["DELIVERY_TERM"].Replace("#ROW#", itemRow) + "' does not have class.");
                                            //            else throw new Exception("Delivery Term field having id '" + _itemMapping["DELIVERY_TERM"].Replace("#ROW#", itemRow) + "' does not exists.");
                                            //        else throw new Exception("Delivery Term field having id '" + _itemMapping["DELIVERY_TERM"].Replace("#ROW#", itemRow) + "' is null.");
                                            //        #endregion

                                            //        #region Supplier Remark
                                            //        cSupplierRemark = convert.ToString(_item.LineItemComment.Value).Trim();
                                            //        if (t4 != null)
                                            //            if (t4.Exists)
                                            //                if (t4.ClassName != null)
                                            //                    if (t4.ClassName.Contains("inputbox_readonly")) throw new Exception("Supplier Remark field having id '" + _itemMapping["SUPPLIER_REMARK"].Replace("#ROW#", itemRow) + "' is readonly.");
                                            //                    else t4.Value = cSupplierRemark;
                                            //                else throw new Exception("Supplier Remark field having id '" + _itemMapping["SUPPLIER_REMARK"].Replace("#ROW#", itemRow) + "' does not have class.");
                                            //            else throw new Exception("Supplier Remark field having id '" + _itemMapping["SUPPLIER_REMARK"].Replace("#ROW#", itemRow) + "' does not exists.");
                                            //        else throw new Exception("Supplier Remark field having id '" + _itemMapping["SUPPLIER_REMARK"].Replace("#ROW#", itemRow) + "' is null.");
                                            //        #endregion
                                            //    }
                                            //}
                                        }
                                        else if (tr2.Exists && convert.ToString(tr2.OwnTableCells[1].Text).Trim().ToUpper() == "PART NO")
                                        {
                                            string itemRow = "";
                                            string cItemPrice = "0.00", cItemDeliveryTerm = "0", cSupplierRemark = "";
                                            //string citemNo = "";
                                            string num = _lineItem.OriginatingSystemRef.Remove(_lineItem.OriginatingSystemRef.IndexOf(_itemMapping["ITEM_NO"].Replace("#ROW#", "")));
                                            if (!chkAllID.Contains(_itemMapping["CHECK_ALL"] + num.Trim()))//added on 2-5-2018
                                                chkAllID.Add(_itemMapping["CHECK_ALL"] + num.Trim());//added on 12-01-2018
                                            string newnum = num + _itemMapping["ITEM_NO"].Replace("#ROW#", "");
                                            string newitemrow = _lineItem.OriginatingSystemRef.Substring(newnum.Length);
                                            itemRow = newitemrow;
                                            TextField t1 = _ie.TextField(num + _itemMapping["ITEM_NO"].Replace("#ROW#", itemRow));
                                            TextField t2 = _ie.TextField(num + _itemMapping["ITEM_PRICE"].Replace("#ROW#", itemRow));
                                            TextField t3 = _ie.TextField(num + _itemMapping["DELIVERY_TERM"].Replace("#ROW#", itemRow));
                                            TextField t4 = _ie.TextField(num + _itemMapping["SUPPLIER_REMARK"].Replace("#ROW#", itemRow));

                                            //if (t1 != null) if (t1.Exists) citemNo = convert.ToString(t1.Text).Trim();
                                            //    else throw new Exception("Item number field having id '"+num + _itemMapping["ITEM_NO"].Replace("#ROW#", itemRow) + "' does not exists.");
                                            //else throw new Exception("Item number field having id '"+num+ + _itemMapping["ITEM_NO"].Replace("#ROW#", itemRow) + "' is null.");

                                            if ((num + _itemMapping["ITEM_NO"].Replace("#ROW#", itemRow)) == convert.ToString(_lineItem.OriginatingSystemRef))
                                            {
                                                _item = _lineItem;
                                                if (_item != null)
                                                {
                                                    #region Item Price
                                                    foreach (PriceDetails _priceDet in _item.PriceList)
                                                        if (_priceDet.TypeQualifier == PriceDetailsTypeQualifiers.GRP) cItemPrice = _priceDet.Value.ToString("0.00");
                                                    if (t2 != null)
                                                        if (t2.Exists)
                                                            if (t2.ClassName != null)
                                                                if (t2.ClassName.Contains("inputbox_readonly")) throw new Exception("Name field having id '" + num + _itemMapping["ITEM_PRICE"].Replace("#ROW#", itemRow) + "' is readonly.");
                                                                else t2.Value = cItemPrice;
                                                            else throw new Exception("Name field having id '" + num + _itemMapping["ITEM_PRICE"].Replace("#ROW#", itemRow) + "' does not have class.");
                                                        else throw new Exception("Name field having id '" + num + _itemMapping["ITEM_PRICE"].Replace("#ROW#", itemRow) + "' does not exists.");
                                                    else throw new Exception("Name field having id '" + num + _itemMapping["ITEM_PRICE"].Replace("#ROW#", itemRow) + "' is null.");
                                                    #endregion

                                                    #region Delivery Term
                                                    if (convert.ToInt(_item.DeleiveryTime) > 0) cItemDeliveryTerm = convert.ToString(_item.DeleiveryTime);
                                                    else cItemDeliveryTerm = convert.ToString(objMTML.DocumentHeader.LeadTimeDays).Trim();
                                                    if (t3 != null)
                                                        if (t3.Exists)
                                                            if (t3.ClassName != null)
                                                                if (t3.ClassName.Contains("inputbox_readonly")) throw new Exception("Unit field having id '" + num + _itemMapping["DELIVERY_TERM"].Replace("#ROW#", itemRow) + "' is readonly.");
                                                                else t3.Value = cItemDeliveryTerm;
                                                            else throw new Exception("Unit field having id '" + num + _itemMapping["DELIVERY_TERM"].Replace("#ROW#", itemRow) + "' does not have class.");
                                                        else throw new Exception("Unit field having id '" + num + _itemMapping["DELIVERY_TERM"].Replace("#ROW#", itemRow) + "' does not exists.");
                                                    else throw new Exception("Unit field having id '" + num + _itemMapping["DELIVERY_TERM"].Replace("#ROW#", itemRow) + "' is null.");
                                                    #endregion

                                                    #region Supplier Remark
                                                    cSupplierRemark = convert.ToString(_item.LineItemComment.Value).Trim();
                                                    if (t4 != null)
                                                        if (t4.Exists)
                                                            if (t4.ClassName != null)
                                                                if (t4.ClassName.Contains("inputbox_readonly")) throw new Exception("Quantity field having id '" + num + _itemMapping["SUPPLIER_REMARK"].Replace("#ROW#", itemRow) + "' is readonly.");
                                                                else t4.Value = cSupplierRemark;
                                                            else throw new Exception("Quantity field having id '" + num + _itemMapping["SUPPLIER_REMARK"].Replace("#ROW#", itemRow) + "' does not have class.");
                                                        else throw new Exception("Quantity field having id '" + num + _itemMapping["SUPPLIER_REMARK"].Replace("#ROW#", itemRow) + "' does not exists.");
                                                    else throw new Exception("Quantity field having id '" + num + _itemMapping["SUPPLIER_REMARK"].Replace("#ROW#", itemRow) + "' is null.");
                                                    #endregion
                                                }
                                            }
                                        }
                                        i++;
                                    }
                                    res = true;
                                }
                                else
                                    throw new Exception("Error Items not found.");
                            }
                        }
                        else throw new Exception("Div having id '" + _itemMapping["ITEM_TABLE"] + "' does not exists");
                    }
                    else throw new Exception("Div having id '" + _itemMapping["ITEM_TABLE"] + "' is null");
                }
                else throw new Exception("Error Items not found.");
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return res;
        }

        private bool FillQuoteHeader()
        {
            bool res = false;
            try
            {
                #region Fill Supplier Ref No
                SetLog("Filling Supplier Ref No");
                TextField suppRefNo = null;
                if (_ie.ContainsText("Machinery Information")) suppRefNo = _ie.TextField(_dictWebsiteMapping["SUPPLIER_REFNO_SPARE_TEXTFIELD"]);
                else suppRefNo = _ie.TextField(_dictWebsiteMapping["SUPPLIER_REFNO_STORE_TEXTFIELD"]);
                if (suppRefNo != null)
                    if (suppRefNo.Exists)
                        if (suppRefNo.ClassName != null)
                            if (suppRefNo.ClassName.Contains("inputbox_readonly"))
                                throw new Exception("Supplier Ref No field is readonly.");
                            else suppRefNo.Value = cSuppRef;
                        else throw new Exception("Supplier Ref No field does not have class.");
                    else throw new Exception("Supplier Ref No field does not exits.");
                else throw new Exception("Supplier Ref No field is null.");
                #endregion

                #region Fill Currency
                SetLog("Filling Currency");
                SelectList sl = _ie.SelectList(_dictWebsiteMapping["HEADER_CURRENCY_COMBO"]);
                if (sl != null)
                    if (sl.Exists)
                    {
                        string ifdisabled = sl.GetAttributeValue("disabled");
                        if (ifdisabled == "disabled")
                            throw new Exception("Currency combo field is disabled.");
                        else
                        {
                            string _value = "";
                            foreach (Option _opt in sl.Options)
                            {
                                if (convert.ToString(_opt.Text).Trim().ToUpper() == cCurrencyCode.Trim().ToUpper())
                                {
                                    _value = _opt.Value;
                                    break;
                                }
                            }
                            if (_value.Trim().Length > 0) sl.SelectByValue(_value.Trim());
                        }
                    }
                    else throw new Exception("Currency combo field does not exits.");
                else throw new Exception("Currency combo field is null.");

                #endregion

                #region Fill D/C Percent
                SetLog("Filling D/C Percent");
                TextField dcPercent = _ie.TextField(_dictWebsiteMapping["DC_PERCENT_TEXTFIELD"]);
                if (dcPercent != null)
                    if (dcPercent.Exists)
                        if (dcPercent.ClassName != null)
                            if (dcPercent.ClassName.Contains("inputbox_readonly")) throw new Exception("D/C Percent field is readonly.");
                            else dcPercent.Value = dAdditionalDisc.ToString("0.00");
                        else throw new Exception("D/C Percent field has no class.");
                    else throw new Exception("D/C Percent field does not exists.");
                else throw new Exception("D/C Percent field is null.");
                #endregion

                #region Filling Valid Date
                SetLog("Filling Valid From");
                TextField validFrom = _ie.TextField(_dictWebsiteMapping["VALID_FROM_TEXTFIELD"]);
                if (validFrom != null)
                    if (validFrom.Exists)
                        if (validFrom.ClassName != null)
                            if (validFrom.ClassName.Contains("inputbox_readonly")) throw new Exception("Valid From field is readonly.");
                            else validFrom.Value = dtValidFrom;
                        else throw new Exception("Valid From field has no class.");
                    else throw new Exception("Valid From field does not exists.");
                else throw new Exception("Valid From field is null.");
                SetLog("Filling Valid To");
                TextField validTo = _ie.TextField(_dictWebsiteMapping["VALID_TO_TEXTFIELD"]);
                if (validTo != null)
                    if (validTo.Exists)
                        if (validTo.ClassName != null)
                            if (validTo.ClassName.Contains("inputbox_readonly")) throw new Exception("Valid To field is readonly.");
                            else validTo.Value = dtValidTo;
                        else throw new Exception("Valid To field has no class.");
                    else throw new Exception("Valid To field does not exists.");
                else throw new Exception("Valid To field is null.");
                #endregion

                #region Filling Quotation Remark
                SetLog("Filling Quotation Remark");
                TextField quotRem = null;
                if (_ie.ContainsText("Machinery Information")) quotRem = _ie.TextField(_dictWebsiteMapping["QUOTE_REMARK_SPARE_TEXTFIELD"]);
                else quotRem = _ie.TextField(_dictWebsiteMapping["QUOTE_REMARK_STORE_TEXTFIELD"]);
                if (quotRem != null)
                    if (quotRem.Exists)
                        if (quotRem.ClassName != null)
                            if (quotRem.ClassName.Contains("inputbox_readonly")) throw new Exception("Quotation Remark field is readonly.");
                            else quotRem.Value = "Quote Ref No. : " + cSuppRef + Environment.NewLine + cQuotationRemark;//19-07-2017
                        else throw new Exception("Quotation Remark field has no class.");
                    else throw new Exception("Quotation Remark field does not exists.");
                else throw new Exception("Quotation Remark field is null.");
                #endregion
                res = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }

        private bool ifTotalItemsMatch(MTMLInterchange objMTML)
        {
            bool res = false;
            try
            {
                Div _divItem = _ie.Div(_itemMapping["ITEM_TABLE"]);
                if (_divItem.Exists)
                {
                    if (_divItem.Tables.Count > 0)
                    {
                        Table tbl = null;
                        if (_ie.ContainsText("Machinery Information")) tbl = _divItem.Tables[2];
                        else tbl = _divItem.Tables[0];
                        if (tbl != null)
                        {
                            int nRowCount = tbl.OwnTableRows.Count;
                            int ProductsCount = 0;
                            if ((nRowCount - 2) % 4 == 0) ProductsCount = (nRowCount - 2) / 4;
                            if (ProductsCount == objMTML.DocumentHeader.LineItemCount) res = true;
                            else throw new Exception("Line Items count mismatch - File count " + objMTML.DocumentHeader.LineItemCount + " site count " + ProductsCount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }

        private bool ifSubmittedQuote(FileInfo xmlFile)
        {
            bool ifsubmit = true;
            try
            {
                Image submitButton = _ie.Image(_dictWebsiteMapping["SUBMIT_BUTTON"]);
                Image saveButton = _ie.Image(_dictWebsiteMapping["SAVE_BUTTON"]);
                if (submitButton != null && saveButton != null)
                {
                    TextField suppRefNo = null;
                    if (_ie.ContainsText("Machinery Information")) suppRefNo = _ie.TextField(_dictWebsiteMapping["SUPPLIER_REFNO_SPARE_TEXTFIELD"]);
                    else suppRefNo = _ie.TextField(_dictWebsiteMapping["SUPPLIER_REFNO_STORE_TEXTFIELD"]);
                    bool ifSaved = false;
                    if (suppRefNo != null)
                        if (suppRefNo.Exists)
                            if (suppRefNo.ClassName != null)
                                if (suppRefNo.ClassName.Contains("inputbox_readonly")) ifSaved = true;
                                else ifSaved = false;
                            else throw new Exception("Supplier Ref No field does not have class.");
                        else throw new Exception("Supplier Ref No field does not exists.");
                    else throw new Exception("Supplier Ref No field is null.");
                    if (submitButton.Exists && saveButton.Exists && !ifSaved) ifsubmit = false;
                    else throw new Exception("Quote of RFQ '" + cVRNO + "' is already submitted.");
                }
                else throw new Exception("Submit and save button is null for Quote of RFQ '" + cVRNO + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ifsubmit;
        }

        public void setUpQuoteError(FileInfo xmlFile, string message)
        {
            try
            {
                ifErrorFile = true;
                AddToAudit(cBuyerCode, cVendorCode, Domain + "_" + DocType, xmlFile.Name, cVRNO, "Error", message);
                string _source = _ie.Body.Parent.OuterHtml;
                _source = _source.Replace("\"/Common/images", "\"../Common/images");
                this.PageSource = _source;
                SavePage(_ie, true, "");
                if (File.Exists(xmlFile.FullName)) MoveFile(xmlFile.DirectoryName + "\\Error", xmlFile);
            }
            catch (Exception ex) { SetLog("Exception in setUpQuoteError() function - " + ex.Message); }
        }

        private void MoveFile(string DestinationPath, FileInfo fileinfo)
        {
            try
            {
                if (DestinationPath.Trim().Length > 0)
                {
                    if (!Directory.Exists(DestinationPath.Trim())) Directory.CreateDirectory(DestinationPath.Trim());
                    string _newFile = (DestinationPath + "\\" + fileinfo.Name).Trim();
                    if (File.Exists(_newFile)) File.Delete(_newFile);
                    File.Move(fileinfo.FullName, _newFile);
                    this.SetLog(fileinfo.Name + " File moved to " + DestinationPath);
                }
                else throw new Exception("Destination path is blank.");
            }
            catch (Exception ex)
            {
                SetLog("Exception in 'MoveFile()' function of 'SKShipping_Routine' class - " + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString());
            }
        }
        #endregion

        private void KillIEInstances()
        {
            IECollection ies = new IECollection();
            if (ies != null && ies.Count > 1)
            {
                foreach (var browser in ies)
                {
                    if (browser.Url == convert.ToString(ConfigurationManager.AppSettings["NOTICE_POPUP_URL"]).Trim())
                    {
                        browser.Dispose();
                    }
                    else if (browser.Url == convert.ToString(ConfigurationManager.AppSettings["NOTICEPT_POPUP_URL"]).Trim())//added by kalpita on 19/02/2020
                    {
                        browser.Dispose();
                    }
                }
            }
        }

    }   
}
