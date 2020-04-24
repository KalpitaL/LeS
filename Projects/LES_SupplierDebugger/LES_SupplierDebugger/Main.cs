using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;
using System.IO;
using eSupplierDataMain;
using MarineLinkRoutines;
using MTML.GENERATOR;
using System.Configuration;
using MTML.IMPORT;
using MTML.EXPORT;
using SINWA.RFQRoutine;
using VSHIPS_MTML_Routine;
using PQC.XLS.RFQ.Routine;
using RealMarineRoutine;
using Amos2MTMLRoutine;
using PDF.Routine;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace eSupplierDebugger
{
    public partial class frmMain : Form
    {
        eSupplierDataMain.Dal.DataAccess _dataAccess;
        eSupplierDataMain.SMData_Routines _smRoutines;
        MarineLinkRoutines.MXML_MAIN _MarineLink;
        MTML.GENERATOR.BuyerSupplierInfo _BuyerSuppInfo;
        MTML.IMPORT.MtmlImport _MTMLImport;
        SINWA.RFQRoutine.TxtWriter _SinwaExport;
        SINWA.RFQRoutine.TxtReader _SinwaImport;
        VSHIPS_MAIN _VshipMTML;
        PQC_XLS_RFQ_Routine _XLSRoutine;
        RealMarineMTML _RealMarine;
        Amos2MTML _AMOS2Mtml;
        EXPORT_XLS_DOC_Routine _exportXLSDoc;
        IMPORT_XLS_DOC_Routine _importXLSDoc;
        EDI_MTML_Routine.EDI_Supplier_Docs _EDI_MTML;
        Boolean pRestartService = false;

        public frmMain()
        {
            int nCnt = 0;
            System.Globalization.CultureInfo _defaultCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            DataLog.AddLog("Default regional setting - "+ _defaultCulture.DisplayName);
            DataLog.AddLog("Current regional setting - " + System.Threading.Thread.CurrentThread.CurrentCulture.DisplayName);

            try
            {
                Process current = Process.GetCurrentProcess();
                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.ProcessName == current.ProcessName) nCnt++;
                }
                if (nCnt > 1)
                {
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }

                InitializeComponent();

                _dataAccess = new eSupplierDataMain.Dal.DataAccess();
                _smRoutines = new SMData_Routines();

                GSettings.cLogPath = Application.StartupPath;

                DO_IMPORT_EXPORT(Application.StartupPath);

                _dataAccess.Dispose();
                _smRoutines = null;
                GC.Collect();

                System.Threading.Thread.CurrentThread.CurrentCulture = _defaultCulture;
                DataLog.AddLog("Current regional setting - " + System.Threading.Thread.CurrentThread.CurrentCulture.DisplayName); 
                Environment.Exit(0);
            }
            finally
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = _defaultCulture;
                DataLog.AddLog("Current regional setting - " + System.Threading.Thread.CurrentThread.CurrentCulture.DisplayName); 
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DO_IMPORT_EXPORT(Application.StartupPath);
        }

        public void DO_IMPORT_EXPORT(string cPath)
        {
            //Debugger.Break();
            GSettings.cLogPath = cPath;
            try
            {
                if (pRestartService) DataLog.AddLog("eSupplier Portal Re-started.");
                else DataLog.AddLog("eSupplier Portal Started - " + convert.ToString(Application.ProductVersion));
                pRestartService = false;

                // SegregateFiles();

                #region /* Import RFQ & PO */
                if (ConfigurationSettings.AppSettings["Import_MarineLink_Files"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Import_MarineLink_Files ");
                    Import_MarineLink_Files();  // import MarineLink files (RFQ) to eSupplier //
                }

                if (ConfigurationSettings.AppSettings["Import_VSHIPs_Files"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Import_VSHIPs_Files ");
                    Import_VSHIPs_Files();  // import VSHIPs files (RFQ) to eSupplier//
                }

                if (ConfigurationSettings.AppSettings["Import_Excel_RFQ_Files"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Import_Excel_RFQ_Files ");
                    Import_Excel_RFQ_Files(); // import Excel type files (RFQ) to eSupplier //
                }

                if (ConfigurationSettings.AppSettings["Import_RealMarine_Files"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Import_RealMarine_Files ");
                    Import_RealMarine_Files(); // import RealMarine(Wallem) type files (RFQ) to eSupplier//
                }

                if (ConfigurationSettings.AppSettings["Import_AMOS2_Files"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Import_AMOS2_Files "); 
                    Import_AMOS2_Files(); // import AMOS2 files (RFQ) to eSupplier //
                }

                //Import_SWIRE_Files(); // Swire Files //

                if (ConfigurationSettings.AppSettings["Import_UNION_MARINE_Files"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Import_UnionMarine_PDF_Files ");
                    Import_UnionMarine_PDF_Files();  // import PDF RFQ & PO files //
                }

                if (ConfigurationSettings.AppSettings["Import_PDF_Files"].ToUpper() == "YES")
                {
                    DataLog.AddLog("Import CBedPDF");
                    
                    Import_CBedPDF_Files();
                    DataLog.AddLog("Import MTM PDF");
                    Import_MTM_PDF_Files();
                    DataLog.AddLog("Import ANobelenZn");
                    Import_ANobelenZn_Files(); // Import A. Nobel Files - 30-JUN-18 //
                    DataLog.AddLog("Import Polska Files");
                    Import_Polska_Files(); // Import Polska PDF files - 30-JUN-18 //                     
                    DataLog.AddLog("Import Vships Files");
                    Import_VshipsPDF_Files();   // added 06/04/2018 Sanjita
                    DataLog.AddLog("Import JanDeNulPDF Files");
                    Import_JanDeNulPDF_Files(); // added 09/04/2018 Sanjita
                    DataLog.AddLog("Import Hidrovia Files");
                    Import_Hidrovia_Files(); // Added on 25-OCT-18 Sanjita

                    DataLog.AddLog("Import ScannedPDF Files");
                    Import_ScannedPDF_Files(); // Import Scanned PDF files - 30-JUL-18  //
                    DataLog.AddLog("Import SilverSea PDF Files");
                    Import_SilverSea_PDF_Files();//Import SilverSea PDF files - 08.04.2019

                    Import_PDF_Files();  // import PDF RFQ & PO files //  
                }

                if (ConfigurationSettings.AppSettings["Import_MAN_CNCO_Files"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Import_MAN_CNCO_Files ");
                    Import_MAN_CNCO_Files(); // MAN PDF Files //
                }

                if (ConfigurationSettings.AppSettings["Import_Excel_Documents_SUPP"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Import_Excel_Documents_SUPP ");
                    Import_Excel_Documents_SUPP(); // Import Supplier Excel documents (Quote & POC) //
                }

                if (ConfigurationSettings.AppSettings["Import_RMS_Excel_Documents_SUPP"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Import_RMS_Excel_Documents_SUPP ");
                    Import_RMS_Excel_Documents_SUPP(); // Import RMS Supplier Excel documents (Quote & POC) //
                }

                if (ConfigurationSettings.AppSettings["Import_Excel_Documents_BYR"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Import_Excel_Documents_BYR ");
                    Import_Excel_Documents_BYR();   // Import Buyer Excel Documents (RFQ & PO) //
                }

                if (ConfigurationSettings.AppSettings["Import_EDI_Buyer_Docs"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Import_EDI_Buyer_Docs ");
                    Import_EDI_Buyer_Docs();
                }

                if (ConfigurationSettings.AppSettings["Import_SEA_PROC_Buyer_Docs"] != null)
                    if (ConfigurationSettings.AppSettings["Import_SEA_PROC_Buyer_Docs"].ToUpper() == "YES")
                    {
                        DataLog.AddLog(" Import_SEA_PROC_Buyer_Docs ");
                        Import_SEA_PROC_Buyer_Docs();
                    }

                if (ConfigurationSettings.AppSettings["Import_SHIP_SERV_Buyer_Docs"] != null)
                    if (ConfigurationSettings.AppSettings["Import_SHIP_SERV_Buyer_Docs"].ToUpper() == "YES")
                    {
                        DataLog.AddLog(" Import_SHIP_SERV_Buyer_Docs ");
                        Import_SHIP_SERV_Buyer_Docs();
                    }

                if (ConfigurationSettings.AppSettings["Import_MTML_V2_Buyer_Docs"] != null)
                    if (ConfigurationSettings.AppSettings["Import_MTML_V2_Buyer_Docs"].ToUpper() == "YES")
                    {
                        DataLog.AddLog(" Import_MTML_V2_Buyer_Docs ");
                        Import_MTML_V2_Buyer_Docs();
                    }

                if (ConfigurationSettings.AppSettings["Import_eSupplier_MTML"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Import_eSupplier_MTML ");
                    Import_eSupplier_MTML();  // import all eSupplier MTML files (RFQ) to eSupplier //
                }
                    
                #endregion

                #region /* Export RFQ & PO to Supplier */

                if (ConfigurationSettings.AppSettings["ExportMarineLinkSuppDoc"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" ExportMarineLinkSuppDoc ");
                    ExportMarineLinkSuppDoc(); // export MarineLink Supp Files (RFQ & PO) // 
                }

                if (ConfigurationSettings.AppSettings["Export_SINWA_RFQFiles"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Export_SINWA_RFQFiles ");
                    Export_SINWA_RFQFiles();    // export Sinwa RFQ files (TQ RFQ) to Supplier //
                }

                if (ConfigurationSettings.AppSettings["Export_EPB_PushData"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Export_EPB_PushData ");
                    Export_EPB_PushData();    // export EPB files to Supplier //
                }

                if (ConfigurationSettings.AppSettings["ExportAMOS2ExcelDoc"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" ExportAMOS2ExcelDoc ");
                    ExportAMOS2ExcelDoc(); // export AMOS2 excel files (RFQ) to Supplier //
                }

                if (ConfigurationSettings.AppSettings["Export_Excel_DOC_RFQ_PO"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Export_Excel_DOC_RFQ_PO ");
                    Export_Excel_DOC_RFQ_PO(); // export Excel doc type RFQ & PO //
                }

                if (ConfigurationSettings.AppSettings["Export_RMS_Excel_DOC_RFQ_PO"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Export_RMS_Excel_DOC_RFQ_PO ");
                    Export_RMS_Excel_DOC_RFQ_PO(); // export RMS Excel doc type RFQ & PO //
                }

                if (ConfigurationSettings.AppSettings["Export_EDI_SuppDoc"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Export_EDI_SuppDoc ");
                    Export_EDI_SuppDoc();   //  export EDI supplier docs //
                }

                if (ConfigurationSettings.AppSettings["Export_EMS_SuppDoc"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Export_EMS_SuppDoc ");
                    Export_EMS_SuppDoc();  // export EMS supplier docs //
                }

                if (ConfigurationSettings.AppSettings["Export_ShipServ_SuppDoc"] != null)
                    if (ConfigurationSettings.AppSettings["Export_ShipServ_SuppDoc"].ToUpper() == "YES")
                    {
                        DataLog.AddLog(" Export_ShipServ_SuppDoc ");
                        Export_ShipServ_SuppDoc();   //  export ShipServ supplier docs //
                    }

                if (ConfigurationSettings.AppSettings["Export_ESUPPLIER_RFQ_PO"] != null)
                    if (ConfigurationSettings.AppSettings["Export_ESUPPLIER_RFQ_PO"].ToUpper() == "YES")
                    {
                        DataLog.AddLog(" Export_ESUPPLIER_RFQ_PO ");
                        Export_ESUPPLIER_RFQ_PO(); // export eSupplier internal RFQ / PO //
                    }

                if (ConfigurationSettings.AppSettings["Export_MTML_V2_RFQ_PO"] != null)
                    if (ConfigurationSettings.AppSettings["Export_MTML_V2_RFQ_PO"].ToUpper() == "YES")
                    {
                        DataLog.AddLog(" Export_MTML_V2_SuppDoc ");
                        Export_MTML_V2_SuppDoc(); // export eSupplier internal RFQ / PO //
                    }

                //Export_LES_MTML_SuppDoc();  // export LES MTML docs
                #endregion

                #region /* Import Supplier Quote & POC Files */                
                Import_Supplier_Quotes(); // for all supplier (Quote)   
                #endregion

                #region /* Export Quotes & POC & Ack files to Buyer */
                if (ConfigurationSettings.AppSettings["Export_MarineLink_Files"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Export_MarineLink_Files ");
                    Export_MarineLink_Files(); // export MarineLink all files (Quote, PO & ACK) //
                }

                if (ConfigurationSettings.AppSettings["Export_VSHIPs_Files"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Export_VSHIPs_Files ");
                    Export_VSHIPs_Files();  // export VShips mtml files (Quote) // 
                }

                if (ConfigurationSettings.AppSettings["Export_ExcelQuote_Files"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Export_ExcelQuote_Files ");
                    Export_ExcelQuote_Files(); // export Excel Quote (Quote) //
                }

                if (ConfigurationSettings.AppSettings["Export_RealMarine_Files"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Export_RealMarine_Files ");
                    Export_RealMarine_Files(); // export RealMarine fZiles (Quote) //
                }

                if (ConfigurationSettings.AppSettings["Export_AMOS2_Files"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Export_AMOS2_Files ");
                    Export_AMOS2_Files(); // export AMOS Quote and POC files (Quote, PO) //
                }

                if (ConfigurationSettings.AppSettings["Export_Excel_DOC_QUOTE_POC"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Export_Excel_DOC_QUOTE_POC ");
                    Export_Excel_DOC_QUOTE_POC(); // export Excel documents like RFQ,PO,Quote,POC //
                }

                if (ConfigurationSettings.AppSettings["Export_MESPAS_QUOTE"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Export_MESPAS_QUOTE ");
                    Export_MESPAS_QUOTE(); // export MESPAS Quotes //
                }

                if (ConfigurationSettings.AppSettings["Export_ESUPPLIER_QuotePOC"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Export_ESUPPLIER_QuotePOC ");
                    Export_ESUPPLIER_QuotePOC(); // export eSupplier internal Quotes / POC //
                }

                if (ConfigurationSettings.AppSettings["Export_FinalQuote_EPB"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Export_FinalQuote_EPB ");
                    Export_FinalQuote_EPB(); // export eSupplier Final Quote //
                }

                if (ConfigurationSettings.AppSettings["Export_SEA_PROC_QuotePOC"] != null)
                    if (ConfigurationSettings.AppSettings["Export_SEA_PROC_QuotePOC"].ToUpper() == "YES")
                    {
                        DataLog.AddLog(" Export_SEA_PROC_QuotePOC ");
                        Export_SEA_PROC_QuotePOC(); // export eSupplier SEA PROC Quotes / POC //
                    }

                if (ConfigurationSettings.AppSettings["Export_EDI_Buyer_Docs"] != null)
                    if (ConfigurationSettings.AppSettings["Export_EDI_Buyer_Docs"].ToUpper() == "YES")
                    {
                        DataLog.AddLog(" Export_EDI_Buyer_Docs ");
                        Export_EDI_Buyer_Docs(); // export eSupplier EDI Quotes / POC //
                    }

                if (ConfigurationSettings.AppSettings["Export_SHIP_SERV_Buyer_Docs"] != null)
                    if (ConfigurationSettings.AppSettings["Export_SHIP_SERV_Buyer_Docs"].ToUpper() == "YES")
                    {
                        DataLog.AddLog(" Export_SHIP_SERV_Buyer_Docs ");
                        Export_SHIP_SERV_Buyer_Docs(); // export ShipServ Quotes / POC //
                    }

                if (ConfigurationSettings.AppSettings["Export_MTML_V2_Buyer_Docs"] != null)
                    if (ConfigurationSettings.AppSettings["Export_MTML_V2_Buyer_Docs"].ToUpper() == "YES")
                    {
                        DataLog.AddLog(" Export_MTML_V2_Buyer_Docs ");
                        Export_MTML_V2_Buyer_Docs(); // export MTML-V2 Quotes / POC //
                    }

                if (ConfigurationSettings.AppSettings["Export_eLite_Supp_Docs"] != null)
                    if (ConfigurationSettings.AppSettings["Export_eLite_Supp_Docs"].ToUpper() == "YES")
                    {
                        DataLog.AddLog(" Export_eLite_RFQ_PO ");
                        Export_eLite_RFQ_PO();
                    }
                #endregion

                #region /* Import Log / Mail queue notify */
                if (ConfigurationSettings.AppSettings["ImportTextLogFiles"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" ImportTextLogFiles | ImportMailDownloadLogFiles ");
                    ImportTextLogFiles();
                    ImportMailDownloadLogFiles();
                }

                if (ConfigurationSettings.AppSettings["ImportMailQueueText"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" ImportMailQueueText ");
                    ImportMailQueueText();
                }

                if (ConfigurationSettings.AppSettings["SendMailQueued"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" SendMailQueue ");
                    if (ConfigurationSettings.AppSettings["NotifyLeSConnectDown"] != null)
                    {
                        if (ConfigurationSettings.AppSettings["NotifyLeSConnectDown"].ToUpper() == "YES")
                            _smRoutines.NotifyLeSConnectDown();
                    }
                    else _smRoutines.NotifyLeSConnectDown(); // Notify LeSConnect Down //

                    _smRoutines.SendMailQueued(); // Send Mail //
                }

                if (ConfigurationSettings.AppSettings["ResendMailNotify"] != null)
                    if (ConfigurationSettings.AppSettings["ResendMailNotify"].ToUpper() == "YES")
                    {
                        DataLog.AddLog(" ResendMailNotify ");
                        ResendMailNotify();
                    }
                #endregion

                DataLog.AddLog("eSupplier Portal Stopped.");
                DataLog.AddLog(Environment.NewLine + "----------------------------------------------");

                if (pRestartService) DO_IMPORT_EXPORT(Application.StartupPath);
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                if (!ex.Message.ToString().ToUpper().Contains("Execution Timeout Expired".ToUpper()))
                    DataLog.AddLog("1. Error in eSupplier Service - " + ex.Message + " at line " + line);
            }
        }

        // UPDATED ON 28-MAR-2016
        private void Import_UnionMarine_PDF_Files()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            int cnt = 0;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_UNIONMARINE_PDF ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                        files = dir.GetFiles();

                        if (files.Length > 0)
                        {
                            foreach (FileInfo f in files)
                            {
                                if (cnt > 5) { pRestartService = true; return; }
                                else cnt++;

                                if (f.Extension.ToLower() == ".pdf" || f.Extension.ToLower() == ".rtf" || f.Extension.ToLower() == ".doc" || f.Extension.ToLower() == ".docx")
                                {
                                    UnionMarineRoutine _umRoutine = new UnionMarineRoutine();
                                    _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, _BuyerSuppInfo);
                                    try
                                    {
                                        if (_smRoutines.IsFileAvailable(f.FullName))
                                        {
                                            Boolean IsTimeOutOrDeadLock = false;
                                            if (_umRoutine.ConvertPDFs(f, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                                _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                            else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        DataLog.AddMaileSupplierLog("2. Unable to import PDF file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                    }
                                    finally
                                    {
                                        _umRoutine = null;
                                        GC.Collect();
                                    }
                                }
                            }
                        }
                    }// for each
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("3. Unable to import PDF file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                files = null; dir = null; dsDirectories.Dispose();
                GC.Collect();
            }
        }

        private void Import_PDF_Files()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            int cnt = 0;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_PDF_DOCS ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        if (_impPath != null && _impPath.Trim().Length > 0)
                        { 
                            dir = new DirectoryInfo(_impPath);
                            if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                            files = dir.GetFiles();
                            if (files.Length > 0)
                            {
                                foreach (FileInfo f in files)
                                {
                                    if (f.Extension.ToLower() == ".pdf" || f.Extension.ToLower() == ".txt" || f.Extension.ToLower() == ".rtf" || f.Extension.ToLower() == ".doc" || f.Extension.ToLower() == ".docx")
                                    {
                                        if (cnt >= 5) { pRestartService = true; return; }
                                        else cnt++;

                                        DataLog.AddLog("Processing file - " + f.FullName);
                                        //PDF2XML_06AUG18 _pdf2Xml = new PDF2XML_06AUG18();
                                        PDF2XML _pdf2Xml = new PDF2XML();

                                        _BuyerSuppInfo = new BuyerSupplierInfo();
                                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);
                                        _BuyerSuppInfo.FileName = f.Name;
                                        _BuyerSuppInfo.BuyerCode = ""; _BuyerSuppInfo.SupplierCode = "";
                                        //_BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, _BuyerSuppInfo);
                                        Boolean IsTimeOutOrDeadLock = false;
                                        try
                                        {
                                            if (_smRoutines.IsFileAvailable(f.FullName))
                                            {
                                                //if (_pdf2Xml.ConvertPDFs(f, ref _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                                if (_pdf2Xml.ConvertPDFs(f, ref _BuyerSuppInfo, ref IsTimeOutOrDeadLock, false)) // Updated on 30-JUL-18 
                                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                                else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            DataLog.AddMaileSupplierLog("4. Unable to import PDF file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                            if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                        }
                                        finally
                                        {
                                            _pdf2Xml = null; GC.Collect();
                                        }
                                    }
                                    else
                                    {
                                        // Updated ON 28-JUL-18
                                        // File Move to Error files
                                        _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                        DataLog.AddMaileSupplierLog("4. Unable to import file. Invalid File Format - " + f.Name, "Error", _BuyerSuppInfo);
                                    }
                                }
                            }
                        }// for each
                    }
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("5. Unable to import PDF file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                files = null; dir = null; dsDirectories.Dispose();
                GC.Collect();
            }
        }

        // Added on 25-OCT-2018
        private void Import_Hidrovia_Files()
        {
            string _impPath = "";
            FileInfo[] files = null; DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            int cnt = 0;

            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_HIDROVIA_PDF ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        if (_impPath != null && _impPath.Trim().Length > 0)
                        {
                            dir = new DirectoryInfo(_impPath);
                            if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                            files = dir.GetFiles();
                            if (files.Length > 0)
                            {
                                foreach (FileInfo f in files)
                                {
                                    if (f.Extension.ToLower() == ".pdf")
                                    {
                                        if (cnt >= 5) { pRestartService = true; return; }
                                        else cnt++;

                                        DataLog.AddLog("Processing Hidrovia PDF file - " + f.FullName);
                                        Hidrovia _hidroviaPDf = new Hidrovia();
                                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, _BuyerSuppInfo);
                                        try
                                        {
                                            if (_smRoutines.IsFileAvailable(f.FullName))
                                            {
                                                Boolean IsTimeOutOrDeadLock = false;
                                                if (_hidroviaPDf.ConvertPDFs(f, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                                else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            DataLog.AddMaileSupplierLog("4. Unable to import Hidrovia file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                        }
                                        finally
                                        {
                                            _hidroviaPDf = null; GC.Collect();
                                        }
                                    }
                                }
                            }
                        }// for each
                    }
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("5. Unable to import Hidrovia PDF file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                files = null; dir = null; dsDirectories.Dispose();
                GC.Collect();
            }
        }        

        // Added on 04-FEB-2017
        private void Import_CBedPDF_Files()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            int cnt = 0;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_CBED_PDF ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        if (_impPath != null && _impPath.Trim().Length > 0)
                        {
                            dir = new DirectoryInfo(_impPath);
                            if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                            files = dir.GetFiles();
                            if (files.Length > 0)
                            {
                                foreach (FileInfo f in files)
                                {
                                    if (f.Extension.ToLower() == ".pdf")
                                    {
                                        if (cnt >= 5) { pRestartService = true; return; }
                                        else cnt++;

                                        DataLog.AddLog("Processing CBED_PDF file - " + f.FullName);

                                        CBedRoutine _pdf2Xml = new CBedRoutine();
                                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, _BuyerSuppInfo);
                                        try
                                        {
                                            if (_smRoutines.IsFileAvailable(f.FullName))
                                            {
                                                Boolean IsTimeOutOrDeadLock = false;
                                                if (_pdf2Xml.ConvertPDFs(f, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                                else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            DataLog.AddMaileSupplierLog("4. Unable to import CBED_PDF file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                        }
                                        finally
                                        {
                                            _pdf2Xml = null; GC.Collect();
                                        }
                                    }
                                }
                            }
                        }// for each
                    }
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("5. Unable to import PDF file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                files = null; dir = null; dsDirectories.Dispose();
                GC.Collect();
            }
        }

        // Added on 10-AUG-2017
        private void Import_MTM_PDF_Files()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            int cnt = 0;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_MTM_PO_PDF ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        if (_impPath != null && _impPath.Trim().Length > 0)
                        {
                            dir = new DirectoryInfo(_impPath);
                            if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                            files = dir.GetFiles();
                            if (files.Length > 0)
                            {
                                foreach (FileInfo f in files)
                                {
                                    if (f.Extension.ToLower() == ".pdf" || f.Extension.ToLower() == ".rtf" || f.Extension.ToLower() == ".doc" || f.Extension.ToLower() == ".docx")
                                    {
                                        if (cnt >= 5) { pRestartService = true; return; }
                                        else cnt++;

                                        DataLog.AddLog("Processing file - " + f.FullName);

                                        MTM_PO_Routine _pdf2Xml = new MTM_PO_Routine();
                                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, _BuyerSuppInfo);
                                        try
                                        {
                                            if (_smRoutines.IsFileAvailable(f.FullName))
                                            {
                                                Boolean IsTimeOutOrDeadLock = false;
                                                if (_pdf2Xml.ConvertPDFs(f, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                                else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            DataLog.AddMaileSupplierLog("Unable to import MTM_SHIP PO file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                        }
                                        finally
                                        {
                                            _pdf2Xml = null; GC.Collect();
                                        }
                                    }
                                }
                            }
                        }// for each
                    }
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("5. Unable to import PDF file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                files = null; dir = null; dsDirectories.Dispose();
                GC.Collect();
            }
        }
        
        // Added on 28-JUN-18
        private void Import_Polska_Files()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            int cnt = 0;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_POLSKA_PDF ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        if (_impPath != null && _impPath.Trim().Length > 0)
                        {
                            dir = new DirectoryInfo(_impPath);
                            if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                            files = dir.GetFiles();
                            if (files.Length > 0)
                            {
                                foreach (FileInfo f in files)
                                {
                                    if (f.Extension.ToLower() == ".pdf")
                                    {
                                        if (cnt >= 5) { pRestartService = true; return; }
                                        else cnt++;

                                        DataLog.AddLog("Processing Polska PDF file - " + f.FullName);

                                        Polska _pdf2Xml = new Polska();
                                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, _BuyerSuppInfo);
                                        try
                                        {
                                            if (_smRoutines.IsFileAvailable(f.FullName))
                                            {
                                                Boolean IsTimeOutOrDeadLock = false;
                                                if (_pdf2Xml.ConvertPDFs(f, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                                else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            DataLog.AddMaileSupplierLog("4. Unable to import Polska PDF file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                        }
                                        finally
                                        {
                                            _pdf2Xml = null; GC.Collect();
                                        }
                                    }
                                }
                            }
                        }// for each
                    }
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("Unable to import Polska PDF file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                files = null; dir = null; dsDirectories.Dispose();
                GC.Collect();
            }
        }

        // Added on 30-JUL-18 
        private void Import_ScannedPDF_Files()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            int cnt = 0;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_SCANNED_PDF_DOCS ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        if (_impPath != null && _impPath.Trim().Length > 0)
                        {
                            dir = new DirectoryInfo(_impPath);
                            if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                            files = dir.GetFiles();
                            if (files.Length > 0)
                            {
                                foreach (FileInfo f in files)
                                {
                                    if (f.Extension.ToLower() == ".pdf" || f.Extension.ToLower() == ".rtf" || f.Extension.ToLower() == ".txt" || f.Extension.ToLower() == ".doc" || f.Extension.ToLower() == ".docx")
                                    {
                                        if (cnt >= 5) { pRestartService = true; return; }
                                        else cnt++;

                                        DataLog.AddLog("Processing Scanned PDF file - " + f.FullName);
                                        PDF2XML _pdf2Xml = new PDF2XML();

                                        _BuyerSuppInfo = new BuyerSupplierInfo();
                                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);
                                        _BuyerSuppInfo.FileName = f.Name;
                                        _BuyerSuppInfo.BuyerCode = ""; _BuyerSuppInfo.SupplierCode = "";
                                        //_BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, _BuyerSuppInfo);
                                        Boolean IsTimeOutOrDeadLock = false;
                                        try
                                        {
                                            if (_smRoutines.IsFileAvailable(f.FullName))
                                            {
                                                if (_pdf2Xml.ConvertPDFs(f, ref _BuyerSuppInfo, ref IsTimeOutOrDeadLock, true))
                                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                                else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            DataLog.AddMaileSupplierLog("4. Unable to import Scanned PDF file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                            if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                        }
                                        finally
                                        {
                                            _pdf2Xml = null; GC.Collect();
                                        }
                                    }
                                } // for each
                            }
                        }
                    } // for each
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("5. Unable to import Scanned PDF file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                files = null; dir = null; dsDirectories.Dispose();
                GC.Collect();
            }
        }

        private void Import_SWIRE_Files()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_SWIRE_PO ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                        files = dir.GetFiles("*.pdf");

                        if (files.Length > 0)
                        {
                            foreach (FileInfo f in files)
                            {
                                SwirePORoutine _swireRoutine = new SwirePORoutine();
                                _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, _BuyerSuppInfo);
                                try
                                {
                                    if (_smRoutines.IsFileAvailable(f.FullName))
                                    {
                                        if (_swireRoutine.Convert(f.FullName, _BuyerSuppInfo))
                                            _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                        //else _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataLog.AddMaileSupplierLog("6. Unable to import Swire PDF file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                }
                                finally
                                {
                                    _swireRoutine = null;
                                    GC.Collect();
                                }
                            }
                        }
                    }// for each
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("7. Unable to import Swire PDF file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                files = null; dir = null; dsDirectories.Dispose();
                GC.Collect();
            }
        }

        private void Import_MAN_CNCO_Files()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_MAN_PDF ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                        files = dir.GetFiles("*.pdf");

                        if (files.Length > 0)
                        {
                            foreach (FileInfo f in files)
                            {
                                MAN_Routine _manRoutine = new MAN_Routine();
                                _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, _BuyerSuppInfo);
                                try
                                {
                                    if (_smRoutines.IsFileAvailable(f.FullName))
                                    {
                                        Boolean IsTimeOutOrDeadLock = false;
                                        if (_manRoutine.Convert(f.FullName, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                            _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                        else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataLog.AddMaileSupplierLog("8. Unable to import MAN PDF file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                }
                                finally
                                {
                                    _manRoutine = null;
                                    GC.Collect();
                                }
                            }
                        }
                    }// for each
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("9. Unable to import MAN PDF file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                files = null; dir = null; dsDirectories.Dispose();
                GC.Collect();
            }
        }

        private void Export_SINWA_RFQFiles()
        {
            //Debugger.Break();
            DataSet dsDirectories = new DataSet();
            _BuyerSuppInfo = new BuyerSupplierInfo();

            try
            {
                dsDirectories = _dataAccess.GetQueryDataset("Select * from SMV_EXPORT_SINWA_RFQ ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow row in dsDirectories.Tables[0].Rows)
                    {
                        try
                        {
                            _SinwaExport = new TxtWriter();
                            _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, "", null);
                            _SinwaExport.ExportTextRFQ(convert.ToInt(row["QUOTATIONID"]), _BuyerSuppInfo);
                        }
                        catch (Exception ex)
                        {
                            DataLog.AddMaileSupplierLog("10. Unable to export Sinwa RFQ file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                        }
                        finally
                        {
                            _SinwaExport = null;
                            GC.Collect();
                        }
                    }
                }
            }
            finally
            {
                _BuyerSuppInfo = null; dsDirectories.Dispose();
                GC.Collect();
            }
        }

        private void Import_eSupplier_MTML()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            int cnt = 0;
            try
            {
                _impPath = ConfigurationSettings.AppSettings["ESUPPLIER_MTML_INBOX"];
                dir = new DirectoryInfo(_impPath);
                if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                //files = dir.GetFiles("*.xml");    // changed by Naresh 03-NOV-15    
                files = Directory.GetFiles(_impPath)    // take last write 10 files
                    .Select(x => new FileInfo(x))
                    .OrderBy(x => x.LastWriteTime)
                    .Take(10)
                    .ToArray();

                foreach (FileInfo f in files)
                {
                    try
                    {
                        if (cnt > 8) { pRestartService = true; return; }
                        else cnt++;

                        _MTMLImport = new MtmlImport();
                        if (_smRoutines.IsFileAvailable(f.FullName))
                        {
                            Boolean IsTimeOutOrDeadLock = false;
                            if (_MTMLImport.Import(f.FullName, ref IsTimeOutOrDeadLock))
                                _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\Backup");
                            else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                        }
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("11. Unable to import eSupplier MTML file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                    finally
                    {
                        _MTMLImport = null; GC.Collect();
                    }
                } // for each
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("12. Unable to import eSupplier MTML file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                _BuyerSuppInfo = null;
                GC.Collect();
            }
        }

        private void Import_MarineLink_Files()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            _BuyerSuppInfo = new BuyerSupplierInfo();
            _MarineLink = new MXML_MAIN();
            int cnt = 0;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_MARINE_LINK_DOCS ");

                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                        files = dir.GetFiles("*.xml");

                        if (files.Length > 0)
                        {
                            foreach (FileInfo f in files)
                            {
                                if (cnt > 5) { pRestartService = true; return; }
                                else cnt++;

                                _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, null);
                                try
                                {
                                    if (_smRoutines.IsFileAvailable(f.FullName))
                                    {
                                        Boolean IsTimeOutOrDeadLock = false;
                                        if (_MarineLink.Import_RFQ_PO_Files(f.FullName, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                            _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                        else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataLog.AddMaileSupplierLog("13. Unable to import MarineLink file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                }
                            }
                        }
                    }// for each
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("14. Unable to import MarineLink file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                _BuyerSuppInfo = null; dsDirectories.Dispose(); dir = null;
                _MarineLink = null; files = null;
                GC.Collect();
            }
        }

        private void Import_Supplier_Quotes()
        {
            if (ConfigurationSettings.AppSettings["Import_EMS_SUPP_DOCS"].ToUpper() == "YES")
            {
                DataLog.AddLog(" Import_EMS_SUPP_DOCS "); 
                Import_EMS_SUPP_DOCS(); // Import EMS Supplier Quote & POC //
            }

            if (ConfigurationSettings.AppSettings["Import_EDI_SUPP_DOCS"].ToUpper() == "YES")
            {
                DataLog.AddLog(" Import_EDI_SUPP_DOCS ");
                Import_EDI_SUPP_DOCS(); // Import EDI Supplier Quote & POC //
            }

            if (ConfigurationSettings.AppSettings["Import_MarineLink_SUPP_DOCS"].ToUpper() == "YES")
            {
                DataLog.AddLog(" Import_MarineLink_SUPP_DOCS ");
                Import_MarineLink_SUPP_DOCS(); // Import MarineLink Supplier Documents //
            }

            if (ConfigurationSettings.AppSettings["Import_SINWA_Quotes"].ToUpper() == "YES")
            {
                DataLog.AddLog(" Import_SINWA_Quotes ");
                Import_SINWA_Quotes(); // Import TQ files for SINWA //
            }

            if (ConfigurationSettings.AppSettings["Import_AMOS2_Excel_Quotes"].ToUpper() == "YES")
            {
                DataLog.AddLog(" Import_AMOS2_Excel_Quotes ");
                Import_AMOS2_Excel_Quotes(); // Import AMOS2 Excel Quotes //
            }

            if (ConfigurationSettings.AppSettings["Import_LES_EPB_PushData"].ToUpper() == "YES")
            {
                DataLog.AddLog(" Import_LES_EPB_PushData ");
                Import_LES_EPB_PushData(); // LES EPB PushData //
            }

            if (ConfigurationSettings.AppSettings["Import_SHIP_SERV_QuotePOC"] != null)
                if (ConfigurationSettings.AppSettings["Import_SHIP_SERV_QuotePOC"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Import_SHIP_SERV_QuotePOC ");
                    Import_SHIP_SERV_QuotePOC(); // Import ShipServ quotes //
                }

            if (ConfigurationSettings.AppSettings["Import_MTML_V2_Quote_POC"] != null)
                if (ConfigurationSettings.AppSettings["Import_MTML_V2_Quote_POC"].ToUpper() == "YES")
                {
                    DataLog.AddLog(" Import_MTML_V2_QuotePOC ");
                    Import_MTML_V2_QuotePOC(); // Import MTML-V2 quotes //
                }

            //Import_LES_MTML_SUPP_Files(); // Import LES MTML files
        }

        private void Import_SINWA_Quotes()
        {
            //Debugger.Break();
            string _impPath = "";
            DataSet dsDirectories = new DataSet();
            DirectoryInfo dir = null;
            int cnt = 0;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset("SELECT * FROM SMV_IMPORT_SINWA_QUOTE_LES ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["ADDR_INBOX"]);
                        if (_impPath.Trim().Length == 0) continue;

                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                        //FileInfo[] files = dir.GetFiles("*.*");   // commented by Naresh 
                        FileInfo[] files;
                        files = Directory.GetFiles(_impPath)    // take last write 10 files
                            .Select(x => new FileInfo(x))
                            .OrderBy(x => x.LastWriteTime)
                            .Take(10)
                            .ToArray();

                        foreach (FileInfo f in files)
                        {
                            if (cnt > 5) { pRestartService = true; return; }
                            else cnt++;

                            _BuyerSuppInfo.FileName = f.Name;
                            _SinwaImport = new TxtReader();
                            if (_smRoutines.IsFileAvailable(f.FullName))
                            {
                                if (f.Extension.ToUpper() == ".TXT")
                                {
                                    Boolean IsTimeOutOrDeadLock = false;
                                    if (_SinwaImport.ImportTxtQuote(f.FullName, _BuyerSuppInfo.GroupID, ref IsTimeOutOrDeadLock))
                                        _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                    else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                }
                                else
                                {
                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                }
                            }
                            else DataLog.AddLog("File " + f.FullName + " used by another process");
                        }

                        _BuyerSuppInfo = null; files = null;
                        GC.Collect();
                    }
                }
            }
            catch (Exception ex)
            {
                DataLog.AddMaileSupplierLog("15. Unable to import SINWA Quote. Error - " + ex.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dsDirectories.Dispose(); dir = null;
                GC.Collect();
            }
        }

        private void Export_MarineLink_Files()
        {
            //Debugger.Break();

            // export MarineLink RFQ-ACK 
            Export_MarineLink_DocFiles("SMV_EXPORT_RFQ_ACK_MARINE_LINK");

            // export MarineLink PO-ACK  
            Export_MarineLink_DocFiles("SMV_EXPORT_PO_ACK_MARINE_LINK");

            // export MarineLink Quote files
            Export_MarineLink_DocFiles("SMV_EXPORT_QUOTE_MARINE_LINK");

            // export MarineLink POC files
            Export_MarineLink_DocFiles("SMV_EXPORT_POC_MARINE_LINK");
        }

        private void Export_MarineLink_DocFiles(string cTableName)
        {
            //Debugger.Break();
            _BuyerSuppInfo = new BuyerSupplierInfo();
            DataSet dsDirectories = new DataSet();
            _MarineLink = new MXML_MAIN();
            try
            {
                // export MarineLink RFQ-ACK 
                dsDirectories = _dataAccess.GetQueryDataset("Select * from " + cTableName);
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow row in dsDirectories.Tables[0].Rows)
                    {
                        try
                        {
                            _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, row["sysDOC_TYPE"].ToString(), null);
                            _MarineLink.Export_QUOTE_POC_Files(convert.ToInt(row["QUOTATIONID"]), row["sysDOC_TYPE"].ToString(), _BuyerSuppInfo);
                        }
                        catch (Exception ex)
                        {
                            DataLog.AddMaileSupplierLog("16. Unable to export MarineLink " + row["sysDOC_TYPE"].ToString() + " file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (! ex.Message.ToString().ToUpper().Contains("Execution Timeout Expired".ToUpper()))
                    DataLog.AddMaileSupplierLog("17. Unable to export MarineLink files. Error - " + ex.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dsDirectories.Dispose(); _MarineLink = null;
                GC.Collect();
            }
        }

        private void Import_VSHIPs_Files()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_VSHIPS_DOCS ");
                _BuyerSuppInfo = new BuyerSupplierInfo();

                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                        files = dir.GetFiles("*.xml");

                        if (files.Length > 0)
                        {
                            foreach (FileInfo f in files)
                            {
                                _VshipMTML = new VSHIPS_MAIN();
                                _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, null);
                                try
                                {
                                    if (_smRoutines.IsFileAvailable(f.FullName))
                                    {
                                        Boolean IsTimeOutOrDeadLock = false;
                                        if (_VshipMTML.Import_MXML_Files(f.FullName, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                            _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                        else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataLog.AddMaileSupplierLog("18. Unable to import VShips file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                }

                                _VshipMTML = null; GC.Collect();
                            }
                        }
                    }// for each
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("19. Unable to import VShips file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dsDirectories.Dispose(); dir = null; files = null;
                GC.Collect();
            }
        }

        private void Export_VSHIPs_Files()
        {
            //Debugger.Break();
            DataSet dsDirectories = new DataSet();
            _BuyerSuppInfo = new BuyerSupplierInfo();

            // export VShips files 
            dsDirectories = _dataAccess.GetQueryDataset("Select * from SMV_EXPORT_VSHIPS_QUOTE ");
            if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
            {
                foreach (DataRow row in dsDirectories.Tables[0].Rows)
                {
                    try
                    {
                        _VshipMTML = new VSHIPS_MAIN();
                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, row["sysDOC_TYPE"].ToString(), null);
                        _VshipMTML.Export_MXML_Files(convert.ToInt(row["QUOTATIONID"]), row["sysDOC_TYPE"].ToString(), _BuyerSuppInfo);
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("20. Unable to export VShips file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                    finally
                    {
                        _VshipMTML = null; GC.Collect();
                    }
                }
            }
        }

        private void Import_Excel_RFQ_Files()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            int cnt = 0;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_EXCEL_RFQS_LES ");                

                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);
                        files = dir.GetFiles("*.*");

                        if (files.Length > 0)
                        {
                            foreach (FileInfo f in files)
                            {
                                if (f.Extension.ToUpper().Contains(".XLS"))
                                {
                                    _BuyerSuppInfo = new BuyerSupplierInfo();
                                    _BuyerSuppInfo.BuyerCode = ""; _BuyerSuppInfo.SupplierCode = "";
                                    if (cnt > 5) { pRestartService = true; return; }
                                    else cnt++;

                                    if (_smRoutines.IsFileAvailable(f.FullName))
                                    {
                                        if ((convert.ToString(strdir["ASSEMBLY_NAME"]).Length > 0) && (convert.ToString(strdir["IMPORT_FUNCTION"]).Length > 0))
                                        {
                                            Assembly objAsm = Assembly.LoadFile(Application.StartupPath + "\\" + convert.ToString(strdir["ASSEMBLY_NAME"]));
                                            Type typeAsm = objAsm.GetType(convert.ToString(strdir["ASMB_IMPORT_CLASS"]));
                                            object data = Activator.CreateInstance(typeAsm);

                                            //calcType.InvokeMember("AddLog", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, data, new object[] { "1" });
                                            //MethodInfo Method = objAsm.GetTypes()[0].GetMethod(convert.ToString(strdir["IMPORT_FUNCTION"]));// "ImportXLSDocuments");

                                            MethodInfo Method = typeAsm.GetMethod(convert.ToString(strdir["IMPORT_FUNCTION"]));// "ImportXLSDocuments");
                                            Boolean Result = (Boolean)Method.Invoke(data, new object[] { f.FullName, "BUYER" });

                                            if (Result) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                            else _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                        }
                                        else // No Assembly 
                                        {
                                            _XLSRoutine = new PQC_XLS_RFQ_Routine();
                                            //_BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, null);
                                            _BuyerSuppInfo.FileName = f.Name;
                                            Boolean IsTimeOutOrDeadLock = false;
                                            try
                                            {
                                                if (_XLSRoutine.ImportXLSRFQ(f.FullName, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                                else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                            }
                                            catch (Exception ex)
                                            {
                                                DataLog.AddMaileSupplierLog("21. Unable to import Excel RFQ file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                                if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                            }
                                            finally
                                            {
                                                _XLSRoutine = null; GC.Collect();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                    // Commented On 28-JUL-18
                                    //DataLog.AddLog("Invalid file - " + f.Name + " moved to error files ");

                                    // Updated ON 28-JUL-18
                                    DataLog.AddMaileSupplierLog("21.1 Unable to import file. Invalid File Format - " + f.Name, "Error", _BuyerSuppInfo);
                                }
                            }
                        }
                    }// for each
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("22. Unable to import Excel RFQ file. Error - " + ex1.StackTrace, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dsDirectories.Dispose(); dir = null; files = null;
                GC.Collect();
            }
        }

        private void Export_ExcelQuote_Files()
        {
            //Debugger.Break();
            string slQuoteIDs = "";
            _XLSRoutine = new PQC_XLS_RFQ_Routine();

            // export Excel QUOTE
            //slQuoteIDs = _dataAccess.GetResultCommaString(" Select Distinct(MESSAGENUMBER),ASMB_EXPORT_CLASS,ASSEMBLY_NAME,EXPORT_FUNCTION  from SMV_EXPORT_EXCEL_QUOTE_LES ");
            DataSet dsDirectories = _dataAccess.GetQueryDataset(" Select Distinct(MESSAGENUMBER),ASMB_EXPORT_CLASS,ASSEMBLY_NAME,EXPORT_FUNCTION  from SMV_EXPORT_EXCEL_QUOTE_LES ");

            foreach (DataRow row in dsDirectories.Tables[0].Rows)
            {
                try
                {
                    if ((convert.ToString(row["ASSEMBLY_NAME"]).Length > 0) && (convert.ToString(row["EXPORT_FUNCTION"]).Length > 0))
                    {
                        Assembly objAsm = Assembly.LoadFile(Application.StartupPath + "\\" + convert.ToString(row["ASSEMBLY_NAME"]));
                        Type typeAsm = objAsm.GetType(convert.ToString(row["ASMB_EXPORT_CLASS"]));
                        object data = Activator.CreateInstance(typeAsm);

                        MethodInfo Method = typeAsm.GetMethod(convert.ToString(row["EXPORT_FUNCTION"]));   // "ExportXLSQuote"
                        Boolean Result = (Boolean)Method.Invoke(data, new object[] { convert.ToString(row["MESSAGENUMBER"]) });
                    }
                    else _XLSRoutine.ExportXLSQuote(convert.ToString(row["MESSAGENUMBER"]));
                }
                catch (Exception ex)
                {
                    DataLog.AddMaileSupplierLog("23. Unable to export Excel Quote files. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                }
            }

            _XLSRoutine = null; GC.Collect();
        }

        private void Import_RealMarine_Files()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_REAL_MARINE_DOCS_LES ");
                _BuyerSuppInfo = new BuyerSupplierInfo();

                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);
                        files = dir.GetFiles("*.xml");

                        if (files.Length > 0)
                        {
                            foreach (FileInfo f in files)
                            {
                                _RealMarine = new RealMarineMTML();
                                _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, null);
                                try
                                {
                                    if (_smRoutines.IsFileAvailable(f.FullName))
                                    {
                                        Boolean IsTimeOutOrDeadLock = false;
                                        if (_RealMarine.ImportRealMarineDocs(f.FullName, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                            _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                        else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataLog.AddMaileSupplierLog("24. Unable to import RealMarine file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                }
                                finally
                                {
                                    _RealMarine = null; GC.Collect();
                                }
                            }
                        }
                    }// for each
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("25. Unable to import VShips file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dsDirectories.Dispose(); dir = null; files = null;
                GC.Collect();
            }
        }

        private void Export_RealMarine_Files()
        {
            //Debugger.Break();
            DataSet dsDirectories = new DataSet();
            _BuyerSuppInfo = new BuyerSupplierInfo();

            // export RealMarine Quote files 
            dsDirectories = _dataAccess.GetQueryDataset("Select * from SMV_EXPORT_REAL_MARINE_QUOTE_LES ");
            if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
            {
                foreach (DataRow row in dsDirectories.Tables[0].Rows)
                {
                    try
                    {
                        _RealMarine = new RealMarineMTML();
                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, row["sysDOC_TYPE"].ToString(), null);
                        _RealMarine.ExportRealMarineDocs(convert.ToInt(row["QUOTATIONID"]), row["sysDOC_TYPE"].ToString(), _BuyerSuppInfo);
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("26. Unable to export RealMarine Quote file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                    finally
                    {
                        _RealMarine = null; GC.Collect();
                    }
                }
            }

            // export RealMarine POC files 
            dsDirectories = _dataAccess.GetQueryDataset("Select * from SMV_EXPORT_REAL_MARINE_POC ");
            if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
            {
                foreach (DataRow row in dsDirectories.Tables[0].Rows)
                {
                    try
                    {
                        _RealMarine = new RealMarineMTML();
                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, "", null);
                        _RealMarine.ExportRealMarineDocs(convert.ToInt(row["QUOTATIONID"]), row["sysDOC_TYPE"].ToString(), _BuyerSuppInfo);
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("27. Unable to export RealMarine POC file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                    finally
                    {
                        _RealMarine = null; GC.Collect();
                    }
                }
            }

            dsDirectories.Dispose(); _BuyerSuppInfo = null;
            GC.Collect();
        }

        private void Import_AMOS2_Files()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            int cnt = 0;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_AMOS2_MTML_DOCS_LES ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                        files = dir.GetFiles("*.*");

                        if (files.Length > 0)
                        {
                            foreach (FileInfo f in files)
                            {
                                if (cnt > 5) { pRestartService = true; return; }
                                else cnt++;

                                _AMOS2Mtml = new Amos2MTML();
                                _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, null);
                                try
                                {
                                    if (_smRoutines.IsFileAvailable(f.FullName))
                                    {
                                        if (f.Extension.ToUpper().Contains("XML"))
                                        {
                                            Boolean IsTimeOutOrDeadLock = false;
                                            if (_AMOS2Mtml.ImportAmos2Docs(f.FullName, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                                _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                            else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                        }
                                        else if (f.Name.ToUpper().Contains("ERRORLOG"))
                                        {
                                            Boolean IsError = true;
                                            IsError = ImportTextLogFile(f.FullName, _BuyerSuppInfo);
                                            _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                            _BuyerSuppInfo.FileFullName = dir.FullName + "\\ERROR_FILES\\" + f.Name;
                                            if (IsError) DataLog.AddMaileSupplierLog("Error file received - " + f.Name, "Error", _BuyerSuppInfo);
                                        }
                                        else if (f.Name.ToUpper().Contains("MAILAUDIT"))
                                        {
                                            string cAuditPath = System.Configuration.ConfigurationSettings.AppSettings["SEND_MAIL_AUDIT_INBOX"].ToString();
                                            _smRoutines.MoveFiles(f.FullName, cAuditPath);
                                        }
                                        else if (f.Name.ToUpper().Contains("AUDITFILE"))
                                        {
                                            string cAuditPath = System.Configuration.ConfigurationSettings.AppSettings["ESUPPLIER_AUDIT_INBOX"].ToString();
                                            _smRoutines.MoveFiles(f.FullName, cAuditPath);
                                        }
                                        else if (f.Extension.ToUpper().Contains("XLS"))
                                        {
                                            DataLog.AddLog("Invalid file received - " + f.FullName);
                                            DataLog.DBAuditLog("Invalid file received - " + f.FullName, "Error", _BuyerSuppInfo);
                                        }
                                        else _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                    DataLog.AddMaileSupplierLog("28. Unable to import AMOS2 file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                }
                                finally
                                {
                                    _AMOS2Mtml = null; GC.Collect();
                                }
                            }
                        }
                    }// for each
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("29. Unable to import AMOS2 file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dsDirectories.Dispose(); dir = null; files = null;
                GC.Collect();
            }
        }

        private void Import_AMOS2_Excel_Quotes()
        {
            //Debugger.Break();
            string _impPath = "";
            DataSet dsDirectories = new DataSet();
            DirectoryInfo dir = null;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset("SELECT * FROM SMV_IMPORT_AMOS2_EXCEL_QUOTE_LES ");

                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["ADDR_INBOX"]);
                        if (_impPath.Trim().Length == 0) continue;

                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                        FileInfo[] files = dir.GetFiles("*.*");

                        foreach (FileInfo fl in files)
                        {
                            string cFullFile = _smRoutines.RenameValidFileName(fl.FullName);
                            FileInfo f = new FileInfo(cFullFile);

                            _BuyerSuppInfo.FileName = f.Name;
                            _BuyerSuppInfo.FileFullName = f.FullName;

                            _AMOS2Mtml = new Amos2MTML();
                            if (_smRoutines.IsFileAvailable(f.FullName))
                            {
                                if (f.Extension.ToUpper().Contains(".XLS"))
                                {
                                    Boolean IsTimeOutOrDeadLock = false;
                                    if (_AMOS2Mtml.ImportAmos2ExcelFiles(f.FullName, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                        _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                    else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                }
                                else
                                {
                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                }
                            }
                            else DataLog.AddLog("File " + f.FullName + " used by another process");

                            _AMOS2Mtml = null; GC.Collect();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DataLog.AddMaileSupplierLog("30. Unable to import AMOS excel Quote. Error - " + ex.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dsDirectories.Dispose(); dir = null;
                GC.Collect();
            }
        }

        private void ExportAMOS2ExcelDoc()
        {
            string slQuoteIDs;
            _AMOS2Mtml = new Amos2MTML();
            try
            {
                // AMOS2 Export Excel RFQ files
                slQuoteIDs = _dataAccess.GetResultCommaString(" Select QUOTATIONID from SMV_EXPORT_AMOS2_EXCEL_RFQ ");
                if (slQuoteIDs.Length > 0)
                {
                    _AMOS2Mtml.ExportAmos2ExcelFiles(slQuoteIDs, "RFQ");
                }

                // AMOS2 Export Excel PO files
                slQuoteIDs = _dataAccess.GetResultCommaString(" Select QUOTATIONID from SMV_EXPORT_AMOS2_EXCEL_PO ");
                if (slQuoteIDs.Length > 0)
                {
                    _AMOS2Mtml.ExportAmos2ExcelFiles(slQuoteIDs, "ORDER");
                }
            }
            finally
            {
                _AMOS2Mtml = null; GC.Collect();
            }
        }

        private void Export_AMOS2_Files()
        {
            //Debugger.Break();
            DataSet dsDirectories = new DataSet();
            _BuyerSuppInfo = new BuyerSupplierInfo();

            // export AMOS2 Quote files 
            dsDirectories = _dataAccess.GetQueryDataset("Select * from SMV_EXPORT_AMOS2_MTML_QUOTE");
            if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
            {
                foreach (DataRow row in dsDirectories.Tables[0].Rows)
                {
                    try
                    {
                        _AMOS2Mtml = new Amos2MTML();
                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, row["sysDOC_TYPE"].ToString(), null);
                        _AMOS2Mtml.ExportAmos2Docs(convert.ToInt(row["QUOTATIONID"]), row["sysDOC_TYPE"].ToString(), _BuyerSuppInfo);
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("31. Unable to export AMOS Quote files. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                }
            }

            // export AMOS2 POC files 
            dsDirectories = _dataAccess.GetQueryDataset("Select * from SMV_EXPORT_AMOS2_MTML_POC_LES ");
            if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
            {
                foreach (DataRow row in dsDirectories.Tables[0].Rows)
                {
                    try
                    {
                        _AMOS2Mtml = new Amos2MTML();
                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, row["sysDOC_TYPE"].ToString(), null);
                        _AMOS2Mtml.ExportAmos2Docs(convert.ToInt(row["QUOTATIONID"]), row["sysDOC_TYPE"].ToString(), _BuyerSuppInfo);
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("32. Unable to export AMOS POC files. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                }
            }

            // export AMOS2 RFQ files 
            dsDirectories = _dataAccess.GetQueryDataset("Select * from SMV_EXPORT_AMOS2_MTML_RFQ");
            if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
            {
                foreach (DataRow row in dsDirectories.Tables[0].Rows)
                {
                    try
                    {
                        _AMOS2Mtml = new Amos2MTML();
                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, row["sysDOC_TYPE"].ToString(), null);
                        _AMOS2Mtml.ExportAmos2Docs(convert.ToInt(row["QUOTATIONID"]), row["sysDOC_TYPE"].ToString(), _BuyerSuppInfo);
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("33. Unable to export AMOS RFQ files. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                }
            }

            dsDirectories.Dispose(); _BuyerSuppInfo = null; GC.Collect();
        }

        private void ExportMarineLinkSuppDoc()
        {
            try
            {
                string cQuoteIDs; string[] slQuotes = { };
                _MarineLink = new MXML_MAIN();
                int nQuoteID;
                _BuyerSuppInfo = new BuyerSupplierInfo();

                // MarineLink Export RFQ files
                cQuoteIDs = _dataAccess.GetResultCommaString(" Select QUOTATIONID from SMV_EXPORT_MARINE_LINK_RFQ ");
                if (cQuoteIDs.Length > 0) slQuotes = cQuoteIDs.Split(',');

                if (slQuotes.Length > 0)
                {
                    foreach (string cQuoteID in slQuotes)
                    {
                        try
                        {
                            nQuoteID = convert.ToInt(cQuoteID);
                            _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(nQuoteID, 0, "", null);
                            _MarineLink.Export_RFQ_PO_Files(nQuoteID, "RequestForQuote", _BuyerSuppInfo);
                        }
                        catch (Exception ex)
                        {
                            DataLog.AddMaileSupplierLog("34. Unable to export MarineLink RFQ file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                        }
                    }
                }

                // MarineLink Export PO files
                slQuotes = new string[0];
                cQuoteIDs = _dataAccess.GetResultCommaString(" Select QUOTATIONID from SMV_EXPORT_MARINE_LINK_PO ");
                if (cQuoteIDs.Length > 0) slQuotes = cQuoteIDs.Split(',');

                if (slQuotes.Length > 0)
                {
                    foreach (string cQuoteID in slQuotes)
                    {
                        try
                        {
                            nQuoteID = convert.ToInt(cQuoteID);
                            _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(nQuoteID, 0, "", null);
                            _MarineLink.Export_RFQ_PO_Files(nQuoteID, "PurchaseOrder", _BuyerSuppInfo);
                        }
                        catch (Exception ex)
                        {
                            DataLog.AddMaileSupplierLog("35. Unable to export MarineLink ORDER file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DataLog.AddMaileSupplierLog("Error in exporting marine link supp docs - " + ex.Message, "Error", _BuyerSuppInfo);
            }
            _MarineLink = null; _BuyerSuppInfo = null; GC.Collect();
        }

        private void Import_MarineLink_SUPP_DOCS()
        {
            //Debugger.Break();
            string _impPath = "";
            DataSet dsDirectories = new DataSet();
            DirectoryInfo dir = null;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset("SELECT * FROM SMV_IMPORT_MARINE_LINK_SUPP_DOCS ");

                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["ADDR_INBOX"]);
                        if (_impPath.Trim().Length == 0) continue;

                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                        FileInfo[] files = dir.GetFiles("*.xml");

                        foreach (FileInfo f in files)
                        {
                            _BuyerSuppInfo.FileName = f.Name;
                            _BuyerSuppInfo.FileFullName = f.FullName;

                            _MarineLink = new MXML_MAIN();
                            if (_smRoutines.IsFileAvailable(f.FullName))
                            {
                                Boolean IsTimeOutOrDeadLock = false;
                                if (_MarineLink.Import_QUOTE_POC_Files(f.FullName, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");

                            }
                            else DataLog.AddLog("File " + f.FullName + " used by another process");

                            _MarineLink = null; GC.Collect();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DataLog.AddMaileSupplierLog("36. Unable to import MarineLink file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dsDirectories.Dispose(); dir = null;
                GC.Collect();
            }
        }

        private void Export_Excel_DOC_RFQ_PO()
        {
            //Debugger.Break();

            // export Excel RFQ documents
            ExportExcel_DocumentsFiles("SMV_EXPORT_EXCEL_DOCUMENT_RFQ", "RFQ");

            // export Excel PO documents
            ExportExcel_DocumentsFiles("SMV_EXPORT_EXCEL_DOCUMENT_PO", "PO");
        }

        private void Export_Excel_DOC_QUOTE_POC()
        {
            //Debugger.Break();

            // export Excel Quote documents
            ExportExcel_DocumentsFiles("SMV_EXPORT_EXCEL_DOCUMENT_QUOTE", "Quote");

            // export Excel POC documents
            ExportExcel_DocumentsFiles("SMV_EXPORT_EXCEL_DOCUMENT_POC", "POC");
        }

        private void ExportExcel_DocumentsFiles(string cTableName, string cDocType)
        {
            //Debugger.Break();
            _BuyerSuppInfo = new BuyerSupplierInfo();
            DataSet dsDirectories = new DataSet();
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset("Select * from " + cTableName);
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow row in dsDirectories.Tables[0].Rows)
                    {
                        try
                        {
                            _exportXLSDoc = new EXPORT_XLS_DOC_Routine();
                            _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, "", null);
                            _exportXLSDoc.ExportXLSDocument(convert.ToInt(row["QUOTATIONID"]), _BuyerSuppInfo);
                        }
                        catch (Exception ex)
                        {
                            DataLog.AddMaileSupplierLog("37. Unable to excel " + cDocType + " file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                        }
                        finally
                        {
                            _exportXLSDoc = null; GC.Collect();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DataLog.AddMaileSupplierLog("38. Unable to export excel " + cDocType + " files. Error - " + ex.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                _BuyerSuppInfo = null; dsDirectories.Dispose();
                GC.Collect();
            }
        }

        private void Import_Excel_Documents_SUPP()
        {
            //Debugger.Break();
            string _impPath = "";
            DataSet dsDirectories = new DataSet();
            DirectoryInfo dir = null;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset("SELECT * FROM SMV_IMPORT_EXCEL_DOCS_SUPP ");

                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["ADDR_INBOX"]);
                        if (_impPath.Trim().Length == 0) continue;

                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                        FileInfo[] files = dir.GetFiles("*.*");

                        foreach (FileInfo f in files)
                        {
                            _BuyerSuppInfo.FileName = f.Name;
                            _BuyerSuppInfo.FileFullName = f.FullName;

                            _importXLSDoc = new IMPORT_XLS_DOC_Routine();
                            if (_smRoutines.IsFileAvailable(f.FullName))
                            {
                                Boolean IsTimeOutOrDeadLock = false;
                                if (_importXLSDoc.ImportXLSDocuments(f.FullName, _BuyerSuppInfo, RoleType.SUPPLIER, ref IsTimeOutOrDeadLock))
                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                            }
                            else DataLog.AddLog("File " + f.FullName + " used by another process");

                            _importXLSDoc = null; GC.Collect();
                        }

                        _BuyerSuppInfo = null; GC.Collect();
                    }
                }
            }
            catch (Exception ex)
            {
                DataLog.AddMaileSupplierLog("39. Unable to import excel doc file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dsDirectories.Dispose(); dir = null;
                GC.Collect();
            }
        }

        private void Import_Excel_Documents_BYR()
        {
            //Debugger.Break();
            string _impPath = "";
            DataSet dsDirectories = new DataSet();
            DirectoryInfo dir = null;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset("SELECT * FROM SMV_IMPORT_EXCEL_DOCS_BYR ");

                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        if (_impPath.Trim().Length == 0) continue;

                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                        FileInfo[] files = dir.GetFiles("*.*");

                        foreach (FileInfo f in files)
                        {
                            _BuyerSuppInfo.FileName = f.Name;
                            _BuyerSuppInfo.FileFullName = f.FullName;

                            _importXLSDoc = new IMPORT_XLS_DOC_Routine();
                            if (_smRoutines.IsFileAvailable(f.FullName))
                            {
                                Boolean IsTimeOutOrDeadLock = false;
                                if (_importXLSDoc.ImportXLSDocuments(f.FullName, _BuyerSuppInfo, RoleType.BUYER, ref IsTimeOutOrDeadLock))
                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                            }
                            else DataLog.AddLog("File " + f.FullName + " used by another process");

                            _importXLSDoc = null; GC.Collect();
                        }

                        _BuyerSuppInfo = null; GC.Collect();
                    }
                }
            }
            catch (Exception ex)
            {
                DataLog.AddMaileSupplierLog("40. Unable to import excel doc file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dsDirectories.Dispose(); dir = null;
                GC.Collect();
            }
        }

        private Boolean ImportTextLogFile(string txtFile, BuyerSupplierInfo ByrSuppInfo)
        {
            FileStream fs = null;
            StreamReader sr = null;
            string Source = "";
            Boolean Return = true;
            try
            {
                fs = new FileStream(txtFile, FileMode.Open, FileAccess.Read);
                sr = new StreamReader(fs);

                while ((Source = sr.ReadLine()) != null)
                {
                    if (convert.ToString(Source).Trim().Length > 0)
                        if (Source.ToLower().Contains("successfully"))
                        {
                            DataLog.DBAuditLog(Source, "Imported", ByrSuppInfo);
                            Return = false;
                        }
                        else DataLog.DBAuditLog("328. " + Source, "Error", ByrSuppInfo);
                }
                return Return;
            }
            catch (Exception ex)
            {
                string cAudit = "41. Unable to read Error log file " + ByrSuppInfo.FileName + ". Error - " + ex.StackTrace;
                DataLog.AddLog(cAudit);
                DataLog.DBAuditLog(cAudit, "Error", ByrSuppInfo);
                return false;
            }
            finally
            {
                fs.Flush();
                sr.Close(); sr.Dispose();
                fs.Close(); fs.Dispose();
                if (sr != null) { sr = null; }
                if (fs != null) { fs = null; }

                GC.Collect();
            }
        }

        private void SegregateFiles()
        {
            string _impPath = "", cSkipLinkIDs = "";
            string cLinkID = "", cAudit = "", cFname = "";
            Boolean isMoved;
            if (ConfigurationSettings.AppSettings["LINKID_FILEPATH"] != null)
                _impPath = ConfigurationSettings.AppSettings["LINKID_FILEPATH"];
            if (ConfigurationSettings.AppSettings["SKIP_LINKID_FROM_FILENAME"] != null)
                cSkipLinkIDs = ConfigurationSettings.AppSettings["SKIP_LINKID_FROM_FILENAME"];
            try
            {
                if (_impPath.Trim().Length > 0)
                {
                    DirectoryInfo dir = new DirectoryInfo(_impPath);
                    if (!dir.Exists) Directory.CreateDirectory(dir.FullName);
                    FileInfo[] files = dir.GetFiles("*.*");

                    foreach (FileInfo f in files)
                    {
                        isMoved = false;
                        BuyerSupplierInfo _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.FileName = f.Name;
                        _BuyerSuppInfo.FileFullName = f.FullName;

                        if (_smRoutines.IsFileAvailable(f.FullName))
                        {
                            string[] slFiles = f.Name.Split('_');
                            if (slFiles.Length > 0)
                            {
                                cLinkID = slFiles[0];
                                eSupplierDataMain.Bll.SmBuyerSupplierLink _ByrSuppLink = eSupplierDataMain.Bll.SmBuyerSupplierLink.Load(convert.ToInt(cLinkID));
                                if (_ByrSuppLink != null)
                                {
                                    if ((!string.IsNullOrEmpty(cSkipLinkIDs)) && (cSkipLinkIDs.Contains(cLinkID))) // changed by Naresh 05-04-2016
                                        cFname = f.Name.Replace(cLinkID + "_", "");
                                    else cFname = f.Name; // .Replace(cLinkID + "_", "");

                                    _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(_ByrSuppLink.Linkid), cFname, _BuyerSuppInfo);
                                    _smRoutines.CopyFiles(f.FullName, _ByrSuppLink.ImportPath, cFname);
                                    cAudit = "File - " + cFname + " moved to " + _BuyerSuppInfo.GroupCode + " inbox ";
                                    if (File.Exists(_ByrSuppLink.ImportPath + "\\" + cFname))
                                        File.Delete(f.FullName);
                                    isMoved = true;
                                }
                                else cAudit = "File name does not contain link-ID";
                            }
                            else cAudit = "File name does not contain link-ID";

                            if (isMoved)
                            {
                                DataLog.AddLog(cAudit);
                                DataLog.DBAuditLog(cAudit, "Uploaded", _BuyerSuppInfo);
                            }
                            else
                            {
                                DataLog.AddLog("329. " + cAudit);
                                DataLog.DBAuditLog("329. " + cAudit, "Error", _BuyerSuppInfo);
                            }
                        }

                        _BuyerSuppInfo = null; GC.Collect();
                    }

                    dir = null; files = null; GC.Collect();
                }
            }
            catch (Exception ex)
            {
                cAudit = "42. Unable to Segregate files. Error - " + ex.Message;
                DataLog.AddLog(cAudit);
                DataLog.DBAuditLog(cAudit, "Error", _BuyerSuppInfo);
            }
        }

        private void Export_MESPAS_QUOTE()
        {
            DataSet dsDirectories = new DataSet();
            _BuyerSuppInfo = new BuyerSupplierInfo();

            try
            {
                // export MTML Quote files 
                dsDirectories = _dataAccess.GetQueryDataset("Select * from SMV_EXPORT_MESPAS_QUOTE");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow row in dsDirectories.Tables[0].Rows)
                    {
                        MTMLInterchange _MTMLInterchage = new MTMLInterchange();
                        MtmlExport _export = new MtmlExport();
                        try
                        {
                            _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, row["DOC_TYPE"].ToString(), null);
                            _MTMLInterchage = _export.Export(convert.ToInt(row["QUOTATIONID"]), row["DOC_TYPE"].ToString());

                            if (!string.IsNullOrEmpty(_MTMLInterchage.MTMLFile))
                            {
                                eSupplierDataMain.Bll.SmBuyerSupplierLink _smLink = eSupplierDataMain.Bll.SmBuyerSupplierLink.Load(_BuyerSuppInfo.ByrSuplrLinkID);
                                string cOutPath = _smLink.ExportPath;
                                _smRoutines.CopyFiles(_MTMLInterchage.MTMLFile, cOutPath, Path.GetFileName(_MTMLInterchage.MTMLFile));
                                //if (_smRoutines.SaveExportedDocument(_MTMLInterchage) == "0") // By Naresh 04-NOV-14
                                {
                                    _MTMLInterchage.BuyerSuppInfo.Export_HTML_Msg = 1;
                                    _MTMLInterchage.BuyerSuppInfo.ExportedQuoteID = _MTMLInterchage.BuyerSuppInfo.RecordID;
                                }

                                _smRoutines.SendMailNotify(_MTMLInterchage.BuyerSuppInfo, "");
                            }
                            eSupplierDataMain.Bll.SmQuotationsVendor _quotation = eSupplierDataMain.Bll.SmQuotationsVendor.Load(_MTMLInterchage.BuyerSuppInfo.RecordID);
                            if (_quotation != null)
                            {
                                _quotation.UpdateDate = DateTime.Now;
                                _quotation.Exported = 2;
                                _quotation.Update();
                            }
                        }
                        catch (Exception ex)
                        {
                            DataLog.AddMaileSupplierLog("43. Unable to export MESPAS " + _BuyerSuppInfo.DocType + " files. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                        }
                        finally
                        {
                            _MTMLInterchage = null; _export = null; GC.Collect();
                        }
                    }
                }
            }
            finally
            {
                dsDirectories.Dispose(); _BuyerSuppInfo = null;
                GC.Collect();
            }
        }

        private void Export_EPB_PushData()
        {
            //Debugger.Break();
            LES_EPB.Routine.EPBPushData _LeSEPB;
            DataSet dsDirectories = new DataSet();
            _BuyerSuppInfo = new BuyerSupplierInfo();

            dsDirectories = _dataAccess.GetQueryDataset("SELECT * FROM SMV_EPB_PUSHDATA_RFQ ");
            if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
            {
                foreach (DataRow row in dsDirectories.Tables[0].Rows)
                {
                    try
                    {
                        _LeSEPB = new LES_EPB.Routine.EPBPushData();
                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, "", null);
                        if (_LeSEPB.PushDataToEPB(convert.ToInt(row["QUOTATIONID"]), _BuyerSuppInfo))
                        {
                            eSupplierDataMain.Bll.SmQuotationsVendor _quotation = eSupplierDataMain.Bll.SmQuotationsVendor.Load(_BuyerSuppInfo.RecordID);
                            if (_quotation != null)
                            {
                                _quotation.UpdateDate = DateTime.Now;
                                _quotation.RfqExport = 0;
                                _quotation.VendorStatus = 2;
                                _quotation.Update();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("44. Unable to push RFQ to EPB. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                    finally
                    {
                        _LeSEPB = null; GC.Collect();
                    }
                }
            }

            dsDirectories = _dataAccess.GetQueryDataset("SELECT * FROM SMV_EPB_PUSHDATA_PO ");
            if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
            {
                foreach (DataRow row in dsDirectories.Tables[0].Rows)
                {
                    try
                    {
                        _LeSEPB = new LES_EPB.Routine.EPBPushData();
                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, "", null);
                        if (_LeSEPB.PushDataToEPB(convert.ToInt(row["QUOTATIONID"]), _BuyerSuppInfo))
                        {
                            eSupplierDataMain.Bll.SmQuotationsVendor _quotation = eSupplierDataMain.Bll.SmQuotationsVendor.Load(_BuyerSuppInfo.RecordID);
                            if (_quotation != null)
                            {
                                _quotation.UpdateDate = DateTime.Now;
                                _quotation.Exported = 2;
                                _quotation.Update();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("45. Unable to push PO to EPB. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                }
            }

            dsDirectories.Dispose(); _BuyerSuppInfo = null;
            GC.Collect();
        }

        private void Import_LES_EPB_PushData()
        {
            LES_EPB.Routine.LeSPushData _LeSPush = new LES_EPB.Routine.LeSPushData();

            if (!Directory.Exists(ConfigurationSettings.AppSettings["LES_EPB_INBOX"].ToString())) Directory.CreateDirectory(ConfigurationSettings.AppSettings["LES_EPB_INBOX"].ToString());
            DirectoryInfo dir = new DirectoryInfo(ConfigurationSettings.AppSettings["LES_EPB_INBOX"].ToString());
            FileInfo[] files = dir.GetFiles("*.*");
            BuyerSupplierInfo _BuyerSuppInfo = new BuyerSupplierInfo();
            try
            {
                foreach (FileInfo f in files)
                {
                    _BuyerSuppInfo.FileName = f.Name;
                    if (_smRoutines.IsFileAvailable(f.FullName))
                    {
                        if (f.Extension.ToUpper() == ".XML")
                        {
                            Boolean IsTimeOutOrDeadLock = false;
                            if (_LeSPush.Read_EPB_To_MTML(f.FullName, ref IsTimeOutOrDeadLock))
                                _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                            else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                        }
                        else
                        {
                            DataLog.AddLog("Invalid file received from EPB service -" + f.FullName);
                            _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                        }
                    }
                    else DataLog.AddLog("File " + f.FullName + " used by another process");
                }
            }
            catch (Exception ex)
            {
                //Changed Sayli 27Apr16
                //DataLog.AddLog("Unable to import file, error-" + ex.Message + ", file - " + _BuyerSuppInfo.FileName);
                //DataLog.DBAuditLog("Unable to import file, error-" + ex.Message, "Error", _BuyerSuppInfo);

                string cAudit = "46. Unable to import file " + _BuyerSuppInfo.FileName + ". Error - " + ex.Message;
                DataLog.AddLog(cAudit);
                DataLog.DBAuditLog(cAudit, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dir = null; files = null; _BuyerSuppInfo = null;
                GC.Collect();
            }
        }

        private void Export_EDI_SuppDoc()
        {
            string cQuoteIDs; string[] slQuotes = { };
            _EDI_MTML = new EDI_MTML_Routine.EDI_Supplier_Docs();
            int nQuoteID;
            _BuyerSuppInfo = new BuyerSupplierInfo();

            // EDI-MTML Export RFQ files
            cQuoteIDs = _dataAccess.GetResultCommaString(" Select QUOTATIONID from SMV_EXPORT_EDI_MTML_RFQ ");
            if (cQuoteIDs.Length > 0) slQuotes = cQuoteIDs.Split(',');

            if (slQuotes.Length > 0)
            {
                foreach (string cQuoteID in slQuotes)
                {
                    try
                    {
                        nQuoteID = convert.ToInt(cQuoteID);
                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(nQuoteID, 0, "", null);
                        _EDI_MTML.Export_EDI_Files(nQuoteID, "RequestForQuote", _BuyerSuppInfo);
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("47. Unable to export EDI-MTML RFQ file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                }
            }

            // EDI-MTML Export PO files
            slQuotes = new string[0];
            cQuoteIDs = _dataAccess.GetResultCommaString(" Select QUOTATIONID from SMV_EXPORT_EDI_MTML_PO ");
            if (cQuoteIDs.Length > 0) slQuotes = cQuoteIDs.Split(',');

            if (slQuotes.Length > 0)
            {
                foreach (string cQuoteID in slQuotes)
                {
                    try
                    {
                        nQuoteID = convert.ToInt(cQuoteID);
                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(nQuoteID, 0, "", null);
                        _EDI_MTML.Export_EDI_Files(nQuoteID, "Order", _BuyerSuppInfo);
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("48. Unable to export EDI-MTML file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                }
            }

            _EDI_MTML = null; _BuyerSuppInfo = null;
            GC.Collect();
        }

        private void Import_EDI_SUPP_DOCS()
        {
            //Debugger.Break();
            string _impPath = "";
            DataSet dsDirectories = new DataSet();
            DirectoryInfo dir = null;
            _EDI_MTML = new EDI_MTML_Routine.EDI_Supplier_Docs();

            try
            {
                dsDirectories = _dataAccess.GetQueryDataset("SELECT * FROM SMV_IMPORT_EDI_SUPP_DOCS ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _impPath = Convert.ToString(strdir["ADDR_INBOX"]);
                        if (_impPath.Trim().Length == 0) continue;

                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);
                        _BuyerSuppInfo.GroupCode = convert.ToString(strdir["GROUP_CODE"]);
                        _BuyerSuppInfo.BuyerID = convert.ToInt(strdir["BUYERID"]);
                        _BuyerSuppInfo.SupplierID = convert.ToInt(strdir["SUPPLIERID"]);

                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);
                        FileInfo[] files = dir.GetFiles("*.xml");

                        foreach (FileInfo f in files)
                        {
                            _BuyerSuppInfo.FileName = f.Name;
                            _BuyerSuppInfo.FileFullName = f.FullName;

                            if (_smRoutines.IsFileAvailable(f.FullName))
                            {
                                Boolean IsTimeOutOrDeadLock = false;
                                if (_EDI_MTML.Import_EDI_Files(f.FullName, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");

                            }
                            else DataLog.AddLog("File " + f.FullName + " used by another process");
                        }

                        _BuyerSuppInfo = null; GC.Collect();
                    }
                }
            }
            catch (Exception ex)
            {
                DataLog.AddMaileSupplierLog("49. Unable to import EDI MTML file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dir = null; dsDirectories.Dispose(); _EDI_MTML = null;
                GC.Collect();
            }
        }

        private void btnExportMTML_Click(object sender, EventArgs e)
        {
            MTMLInterchange _MTMLInterchage = new MTMLInterchange();
            MtmlExport _export = new MtmlExport();
            try
            {
                _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(10116758, 0, "", null);
                _MTMLInterchage = _export.Export(10116758, "Quote");

                eSupplierDataMain.Bll.SmBuyerSupplierLink _smLink = eSupplierDataMain.Bll.SmBuyerSupplierLink.Load(_BuyerSuppInfo.ByrSuplrLinkID);
                string cOutPath = _smLink.ExportPath;
                _smRoutines.CopyFiles(_MTMLInterchage.MTMLFile, cOutPath, Path.GetFileName(_MTMLInterchage.MTMLFile));
                // if (_smRoutines.SaveExportedDocument(_MTMLInterchage) == "0") // By Naresh 04-NOV-14
                {
                    _MTMLInterchage.BuyerSuppInfo.Export_HTML_Msg = 1;
                    _MTMLInterchage.BuyerSuppInfo.ExportedQuoteID = _MTMLInterchage.BuyerSuppInfo.RecordID;
                }
            }
            finally
            {
                _MTMLInterchage = null; _export = null;
                GC.Collect();
            }
        }

        //private void ImportTextLogFiles()
        //{
        //    string cPath = System.Configuration.ConfigurationSettings.AppSettings["ESUPPLIER_AUDIT_INBOX"].ToString();
        //    string[] files = null;
        //    files = Directory.GetFiles(cPath, "*.txt");
        //    Boolean isUpdated = false;

        //    foreach (string cFile in files)
        //    {
        //        FileStream fs = null;
        //        StreamReader sr = null;
        //        Boolean notToRead = false;
        //        try
        //        {
        //            string Source = "";

        //            fs = new FileStream(cFile, FileMode.Open, FileAccess.Read);
        //            sr = new StreamReader(fs);

        //            while ((Source = sr.ReadLine()) != null)
        //            {
        //                string[] slAudit, slTimestamp;
        //                int nByrId, nSuppId;
        //                DateTime dtUpdate = DateTime.Now;

        //                if (convert.ToString(Source).Contains("Source:"))
        //                    notToRead = true;

        //                if ((convert.ToString(Source).Trim().Length > 0) && (notToRead == false))
        //                {
        //                    slAudit = convert.ToString(Source).Split('|');
        //                    if (slAudit.Length > 3)
        //                    {
        //                        nByrId = convert.ToInt(slAudit[0]);
        //                        nSuppId = convert.ToInt(slAudit[1]);
        //                        if ((nByrId == 0) && (slAudit[0].Length > 0))
        //                            nByrId = convert.ToInt(_dataAccess.GetFieldValue("ADDRESSID", "ADDR_CODE", "SM_ADDRESS", convert.ToQuote(slAudit[0].Trim()), ""));
        //                        if ((nSuppId == 0) && (slAudit[1].Length > 0))
        //                            nSuppId = convert.ToInt(_dataAccess.GetFieldValue("ADDRESSID", "ADDR_CODE", "SM_ADDRESS", convert.ToQuote(slAudit[1].Trim()), ""));

        //                        slTimestamp = slAudit[6].Split(new string[] { " : " }, StringSplitOptions.RemoveEmptyEntries);
        //                        try
        //                        {
        //                            if (slTimestamp.Length > 0)
        //                            {
        //                                DateTime dtLogDt = convert.ToDateTime(slTimestamp[0], "yy-MM-dd HH:mm");
        //                                if (dtLogDt != DateTime.MinValue)
        //                                    dtUpdate = dtLogDt;
        //                            }
        //                        }
        //                        catch { }

        //                        eSupplierDataMain.Bll.SmAuditlog _audit = new eSupplierDataMain.Bll.SmAuditlog();
        //                        _audit.Auditvalue = slAudit[6];
        //                        _audit.Keyref1 = slAudit[4];
        //                        _audit.Keyref2 = slAudit[4];
        //                        _audit.Updatedate = dtUpdate;
        //                        _audit.Logtype = slAudit[5];
        //                        _audit.Modulename = slAudit[2];
        //                        _audit.Filename = slAudit[3];
        //                        _audit.BuyerId = nByrId;
        //                        _audit.SupplierId = nSuppId;

        //                        _audit.Insert();

        //                        if (_audit.Logtype.ToUpper() == "ERROR")
        //                        {
        //                            BuyerSupplierInfo _ByrSupplrInfo = new BuyerSupplierInfo();
        //                            _ByrSupplrInfo.FileName = Path.GetFileName(cFile);
        //                            _ByrSupplrInfo.REF_No = _audit.Keyref2;
        //                            _ByrSupplrInfo.FileFullName = cFile;

        //                            DataLog.SendMail(_audit.Auditvalue, _ByrSupplrInfo);
        //                        }
        //                        //DataLog.DBAuditLog(slAudit[6], slAudit[5], slAudit[2], slAudit[3], slAudit[4], slAudit[4], nByrId, nSuppId);

        //                        _audit = null; GC.Collect();
        //                    }
        //                }
        //            }
        //            isUpdated = true;
        //        }
        //        catch
        //        {
        //            isUpdated = false;
        //            string cAudit = "Unable to read Error log file " + Path.GetFileName(cFile);
        //            DataLog.AddLog(cAudit);
        //            DataLog.DBAuditLog(cAudit, "Error", null);
        //        }
        //        finally
        //        {
        //            fs.Flush();
        //            sr.Close(); sr.Dispose();
        //            fs.Close(); fs.Dispose();
        //            if (sr != null) { sr = null; }
        //            if (fs != null) { fs = null; }
        //            GC.Collect();
        //        }

        //        if (isUpdated) _smRoutines.MoveFiles(cFile, cPath + "\\BACKUP");
        //        else _smRoutines.MoveFiles(cFile, cPath + "\\ERROR_FILES");
        //    }

        //    files = null; GC.Collect();
        //}

        private void ImportTextLogFiles()
        {
            string cPath = System.Configuration.ConfigurationSettings.AppSettings["ESUPPLIER_AUDIT_INBOX"].ToString();
            string[] modules = convert.ToString(ConfigurationManager.AppSettings["DO_NOT_SEND_MAIL"]).Split(',');

            string[] files = null;
            if (!Directory.Exists(cPath)) Directory.CreateDirectory(cPath);
            files = Directory.GetFiles(cPath, "*.txt");
            Boolean isUpdated = false;

            List<string> lst = new List<string>();
            foreach (string m in modules)
            {
                lst.Add(m.Trim().ToUpper());
            }

            foreach (string cFile in files)
            {
                FileStream fs = null;
                StreamReader sr = null;
                Boolean notToRead = false;
                string cServer = "", cByrCode = "", cSuppCode = "", cProcessorName="";
                try
                {
                    string Source = "";
                    fs = new FileStream(cFile, FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(fs);
                    while ((Source = sr.ReadLine()) != null)
                    {
                        string[] slAudit, slTimestamp;
                        int nByrId, nSuppId;
                        DateTime dtUpdate = DateTime.Now;

                        //if (convert.ToString(Source).Contains("Source:")) notToRead = true; // by Naresh // 01/06/2018
                        if ((convert.ToString(Source).Trim().Length > 0) && (notToRead == false))
                        {
                            slAudit = convert.ToString(Source).Split('|');
                            if (slAudit.Length > 3)
                            {
                                nByrId = convert.ToInt(slAudit[0]);
                                nSuppId = convert.ToInt(slAudit[1]);
                                string cTimeStamp = DateTime.Now.ToString("yyMMdd_HHmmssfff");
                                int nLinkID = 0;
                                if (slAudit.Length > 7) nLinkID = convert.ToInt(slAudit[7]);
                                if (slAudit.Length > 8) cServer = slAudit[8];
                                if (slAudit.Length > 9) cByrCode = slAudit[9];
                                if (slAudit.Length > 10) cSuppCode = slAudit[10];
                                if (slAudit.Length > 11) cProcessorName = slAudit[11];

                                if (nByrId == 0 || nSuppId == 0)
                                {
                                    eSupplierDataMain.Bll.SmBuyerSupplierLink _link = null;
                                    if (nLinkID > 0)
                                        _link = eSupplierDataMain.Bll.SmBuyerSupplierLink.Load(nLinkID);
                                    else _link = eSupplierDataMain.Bll.SmBuyerSupplierLink.Select_Byr_Supp_LinkCode(slAudit[0].Trim(), slAudit[1].Trim());

                                    if (_link != null && _link.Linkid > 0)
                                    {
                                        nByrId = convert.ToInt(_link.pBuyerID);
                                        nSuppId = convert.ToInt(_link.pSupplierID);
                                        if (string.IsNullOrEmpty(cByrCode)) cByrCode = _link.BuyerLinkCode;
                                        if (string.IsNullOrEmpty(cSuppCode)) cSuppCode = _link.VendorLinkCode;
                                    }
                                }
                                if (string.IsNullOrEmpty(cByrCode)) cByrCode = convert.ToString(slAudit[0]);
                                if (string.IsNullOrEmpty(cSuppCode)) cSuppCode = convert.ToString(slAudit[1]);

                                slTimestamp = slAudit[6].Split(new string[] { " : " }, StringSplitOptions.RemoveEmptyEntries);
                                try
                                {
                                    if (slTimestamp.Length > 0)
                                    {
                                        DateTime dtLogDt = convert.ToDateTime(slTimestamp[0], "yy-MM-dd HH:mm");
                                        if (dtLogDt == DateTime.MinValue) dtLogDt = convert.ToDateTime(slTimestamp[0], "yy-MM-dd HH:mm:ss");
                                        if (dtLogDt == DateTime.MinValue) dtLogDt = convert.ToDateTime(slTimestamp[0], "yyyy-MM-dd HH:mm:ss");
                                        if (dtLogDt == DateTime.MinValue) dtLogDt = convert.ToDateTime(slTimestamp[0], "yyyyMMdd HH:mm");
                                        if (dtLogDt == DateTime.MinValue) dtLogDt = convert.ToDateTime(slTimestamp[0], "yyyyMMdd HH:mm:ss");

                                        if (dtLogDt != DateTime.MinValue) dtUpdate = dtLogDt;
                                    }
                                    else
                                    {
                                        dtUpdate = DateTime.Now;
                                    }

                                    string _auditLog = "";
                                    if (slTimestamp.Length == 1) _auditLog = slTimestamp[0].Trim();
                                    else for (int i = 1; i < slTimestamp.Length; i++) _auditLog += " : " + slTimestamp[i].Trim();

                                    slAudit[6] = dtUpdate.ToString("yy-MM-dd HH:mm") + " : " + _auditLog.Trim().Trim(':').Trim();
                                }
                                catch { }

                                eSupplierDataMain.Bll.SmAuditlog _audit = new eSupplierDataMain.Bll.SmAuditlog();
                                _audit.Auditvalue = slAudit[6];
                                _audit.Keyref1 = cTimeStamp;    ////slAudit[4] changed by Naresh - 14/05/2018 to link to zoho
                                _audit.Keyref2 = slAudit[4];
                                _audit.Updatedate = dtUpdate;
                                _audit.Logtype = slAudit[5];
                                _audit.Modulename = slAudit[2];
                                _audit.Filename = slAudit[3];
                                _audit.BuyerId = nByrId;
                                _audit.SupplierId = nSuppId;
                                _audit.ServerName = cServer;
                                _audit.BuyerCode = cByrCode;
                                _audit.SupplierCode = cSuppCode;
                                _audit.ProcessorName = cProcessorName;

                                if ((_audit.Logtype.ToUpper() == "ERROR" || _audit.Modulename.ToUpper() == "LESDASHBOARD") && !lst.Contains(_audit.Modulename.Trim().ToUpper()))
                                {
                                    BuyerSupplierInfo _ByrSupplrInfo = new BuyerSupplierInfo();
                                    if (!string.IsNullOrEmpty(_audit.Filename))
                                        _ByrSupplrInfo.FileName = _audit.Filename;
                                    else _ByrSupplrInfo.FileName = Path.GetFileName(cFile);
                                    _ByrSupplrInfo.FileName2 = Path.GetFileName(cFile);
                                    _ByrSupplrInfo.REF_No = _audit.Keyref2;
                                    _ByrSupplrInfo.FileFullName = _audit.Filename;
                                    _ByrSupplrInfo.DocType = _audit.DocType;
                                    _ByrSupplrInfo.GroupCode = _audit.Modulename;
                                    _ByrSupplrInfo.ServerName = cServer;
                                    _ByrSupplrInfo.BuyerCode = cByrCode;
                                    _ByrSupplrInfo.SupplierCode = cSuppCode;
                                    _ByrSupplrInfo.UDF1 = _audit.Modulename;
                                    _ByrSupplrInfo.LeSTicketRef = cTimeStamp; // changed by Naresh - 14/05/2018 to link to zoho
                                    if (_audit.Modulename.ToUpper() == "AMOS2" || _audit.Modulename.ToUpper() == "AMOSW")
                                        if (_audit.Auditvalue.Contains("Exporting of RFQ stopped"))
                                            _ByrSupplrInfo.FileFullName = "";

                                    DataLog.AddMaileSupplierLog(_audit.Auditvalue + " | LogFile-" + Path.GetFileName(cFile), "Error", _ByrSupplrInfo);
                                    DataLog.AddLog("SendMailQueue added for ref - " + _audit.Keyref2);
                                }
                                else
                                {
                                    _audit.Insert();
                                    DataLog.PushAudit_LeSAdminService(null, _audit);
                                }
                                _audit = null; GC.Collect();
                            }
                        }
                    }

                    isUpdated = true;
                }
                catch (Exception ex) //Changed Sayli 27Apr16: Added (Exception ex)
                {
                    isUpdated = false;
                    string cAudit = "50. Unable to read Error log file " + Path.GetFileName(cFile) + ". Error - " + ex.StackTrace;
                    DataLog.AddLog(cAudit);
                    //DataLog.DBAuditLog(cAudit, "Error", null);
                }
                finally
                {
                    fs.Flush();
                    sr.Close(); sr.Dispose();
                    fs.Close(); fs.Dispose();
                    if (sr != null) { sr = null; }
                    if (fs != null) { fs = null; }
                    GC.Collect();
                }

                if (isUpdated) _smRoutines.MoveFiles(cFile, cPath + "\\BACKUP");
                else _smRoutines.MoveFiles(cFile, cPath + "\\ERROR_FILES");
            }

            files = null; GC.Collect();
        }

        private void Import_LES_MTML_SUPP_Files()
        {
            //Debugger.Break();
            string _impPath = "";
            DataSet dsDirectories = new DataSet();
            DirectoryInfo dir = null;
            LES_MTML_Routine.LES_MAIN lesMTML = new LES_MTML_Routine.LES_MAIN();

            try
            {
                dsDirectories = _dataAccess.GetQueryDataset("SELECT * FROM SMV_IMPORT_LES_MTML_SUPP_DOCS ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _impPath = Convert.ToString(strdir["ADDR_INBOX"]);
                        if (_impPath.Trim().Length == 0) continue;

                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);
                        FileInfo[] files = dir.GetFiles("*.xml");

                        foreach (FileInfo f in files)
                        {
                            _BuyerSuppInfo.FileName = f.Name;
                            _BuyerSuppInfo.FileFullName = f.FullName;

                            if (_smRoutines.IsFileAvailable(f.FullName))
                            {
                                if (lesMTML.Import_LES_MTML_SUPP_Files(f.FullName, _BuyerSuppInfo))
                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                else _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");

                            }
                            else DataLog.AddLog("File " + f.FullName + " used by another process");
                        }

                        _BuyerSuppInfo = null; GC.Collect();
                    }
                }
            }
            catch (Exception ex)
            {
                DataLog.AddMaileSupplierLog("51. Unable to import LES MTML file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                lesMTML = null; dir = null; dsDirectories.Dispose();
                GC.Collect();
            }
        }

        private void Export_LES_MTML_SuppDoc()
        {
            string cQuoteIDs; string[] slQuotes = { };
            LES_MTML_Routine.LES_MAIN _LesMtml = new LES_MTML_Routine.LES_MAIN();
            int nQuoteID;
            _BuyerSuppInfo = new BuyerSupplierInfo();

            // LES-MTML Export RFQ files
            cQuoteIDs = _dataAccess.GetResultCommaString(" Select QUOTATIONID from SMV_EXPORT_LES_MTML_RFQ ");
            if (cQuoteIDs.Length > 0) slQuotes = cQuoteIDs.Split(',');

            if (slQuotes.Length > 0)
            {
                foreach (string cQuoteID in slQuotes)
                {
                    try
                    {
                        nQuoteID = convert.ToInt(cQuoteID);
                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(nQuoteID, 0, "", null);
                        _LesMtml.Export_LES_MTML_SUPP_Files(nQuoteID, "RequestForQuote", _BuyerSuppInfo);
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("52. Unable to export LES-MTML RFQ file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                }
            }

            // LES-MTML Export PO files
            slQuotes = new string[0];
            cQuoteIDs = _dataAccess.GetResultCommaString(" Select QUOTATIONID from SMV_EXPORT_LES_MTML_PO ");
            if (cQuoteIDs.Length > 0) slQuotes = cQuoteIDs.Split(',');

            if (slQuotes.Length > 0)
            {
                foreach (string cQuoteID in slQuotes)
                {
                    try
                    {
                        nQuoteID = convert.ToInt(cQuoteID);
                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(nQuoteID, 0, "", null);
                        _LesMtml.Export_LES_MTML_SUPP_Files(nQuoteID, "Order", _BuyerSuppInfo);
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("53. Unable to export LES-MTML file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                }
            }
            _LesMtml = null; _BuyerSuppInfo = null; GC.Collect();
        }

        private void btnMTMLImport_Click(object sender, EventArgs e)
        {
            Import_eSupplier_MTML();  // import all eSupplier MTML files (RFQ) to eSupplier //
        }

        private void Export_EMS_SuppDoc()
        {
            string cQuoteIDs; string[] slQuotes = { };
            EMS_MTML_Routine.EMS_MAIN _EMS_MTML = new EMS_MTML_Routine.EMS_MAIN();
            int nQuoteID;
            _BuyerSuppInfo = new BuyerSupplierInfo();

            // EMS-MTML Export RFQ files
            cQuoteIDs = _dataAccess.GetResultCommaString(" Select QUOTATIONID from SMV_EXPORT_EMS_MTML_RFQ ");
            if (cQuoteIDs.Length > 0) slQuotes = cQuoteIDs.Split(',');

            if (slQuotes.Length > 0)
            {
                foreach (string cQuoteID in slQuotes)
                {
                    try
                    {
                        nQuoteID = convert.ToInt(cQuoteID);
                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(nQuoteID, 0, "", null);
                        _EMS_MTML.Export_EMS_Files(nQuoteID, "RequestForQuote", _BuyerSuppInfo);
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("54. Unable to export EMS-MTML RFQ file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                }
            }

            // EMS-MTML Export PO files
            slQuotes = new string[0];
            cQuoteIDs = _dataAccess.GetResultCommaString(" Select QUOTATIONID from SMV_EXPORT_EMS_MTML_PO ");
            if (cQuoteIDs.Length > 0) slQuotes = cQuoteIDs.Split(',');

            if (slQuotes.Length > 0)
            {
                foreach (string cQuoteID in slQuotes)
                {
                    try
                    {
                        nQuoteID = convert.ToInt(cQuoteID);
                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(nQuoteID, 0, "", null);
                        _EMS_MTML.Export_EMS_Files(nQuoteID, "Order", _BuyerSuppInfo);
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("55. Unable to export EMS-MTML file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                }
            }
            _EMS_MTML = null; _BuyerSuppInfo = null; GC.Collect();
        }

        private void Import_EMS_SUPP_DOCS()
        {
            //Debugger.Break();
            string _impPath = "";
            DataSet dsDirectories = new DataSet();
            DirectoryInfo dir = null;
            EMS_MTML_Routine.EMS_MAIN _EMS_MTML = new EMS_MTML_Routine.EMS_MAIN();

            try
            {
                dsDirectories = _dataAccess.GetQueryDataset("SELECT * FROM SMV_IMPORT_EMS_SUPP_DOCS ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["ADDR_INBOX"]);
                        if (_impPath.Trim().Length == 0) continue;

                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);
                        FileInfo[] files = dir.GetFiles("*.xml");

                        foreach (FileInfo f in files)
                        {
                            _BuyerSuppInfo.FileName = f.Name;
                            _BuyerSuppInfo.FileFullName = f.FullName;

                            if (_smRoutines.IsFileAvailable(f.FullName))
                            {
                                Boolean IsTimeOutOrDeadLock = false;
                                if (_EMS_MTML.Import_EMS_Files(f.FullName, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");

                            }
                            else DataLog.AddLog("File " + f.FullName + " used by another process");
                        }

                        files = null; GC.Collect();
                    }
                }
            }
            catch (Exception ex)
            {
                DataLog.AddMaileSupplierLog("56. Unable to import EMS MTML file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                _EMS_MTML = null; dir = null; dsDirectories.Dispose();
                GC.Collect();
            }
        }

        private void btnEMSExport_Click(object sender, EventArgs e)
        {
            Export_EMS_SuppDoc();
        }

        private void btnImportEMS_Click(object sender, EventArgs e)
        {
            Import_EMS_SUPP_DOCS();
        }

        private void Export_ESUPPLIER_QuotePOC()
        {
            DataSet dsDirectories = new DataSet();
            _BuyerSuppInfo = new BuyerSupplierInfo();

            try
            {
                // export MTML internal Quote files 
                dsDirectories = _dataAccess.GetQueryDataset("Select * from SMV_EXPORT_ESUPPLIER_INTERNAL_QUOTE_POC");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow row in dsDirectories.Tables[0].Rows)
                    {
                        MTMLInterchange _MTMLInterchage = new MTMLInterchange();
                        MtmlExport _export = new MtmlExport();
                        try
                        {
                            _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, row["DOC_TYPE"].ToString(), null);
                            _MTMLInterchage = _export.Export(convert.ToInt(row["QUOTATIONID"]), row["DOC_TYPE"].ToString());

                            if (!string.IsNullOrEmpty(_MTMLInterchage.MTMLFile))
                            {
                                eSupplierDataMain.Bll.SmBuyerSupplierLink _smLink = eSupplierDataMain.Bll.SmBuyerSupplierLink.Load(_BuyerSuppInfo.ByrSuplrLinkID);
                                string cOutPath = _smLink.ExportPath;
                                _smRoutines.CopyFiles(_MTMLInterchage.MTMLFile, cOutPath, Path.GetFileName(_MTMLInterchage.MTMLFile));

                                _MTMLInterchage.BuyerSuppInfo.Export_HTML_Msg = 1;
                                _smRoutines.SendMailNotify(_MTMLInterchage.BuyerSuppInfo, "");
                                string cRule = _smRoutines.GetRuleValue(_MTMLInterchage, "SYSTEM_COPY_SUBMITTED_FILE");
                                DataLog.AddLog("Rule value 'SYSTEM_COPY_SUBMITTED_FILE' - " + cRule);

                                if (cRule == "1")
                                {
                                    string cPath = cOutPath;
                                    List<string> slAttach = _smRoutines.GetDocumentAttachments(_MTMLInterchage.BuyerSuppInfo, null, RoleType.BUYER, ModuleType.Notify);
                                    DataLog.AddLog("Attachments count - " + slAttach.Count);

                                    if (slAttach != null && (slAttach.Count > 0))
                                    {
                                        string[] cfile = slAttach[0].Split('\\');
                                        string fName = cfile[cfile.Length - 1];
                                        DataLog.AddLog("Copying attachment - " + fName);

                                        if (File.Exists(cPath + "\\" + fName)) File.Delete(cPath + "\\" + fName);
                                        File.Copy(slAttach[0], cPath + "\\" + fName);
                                    }
                                }
                            }

                            eSupplierDataMain.Bll.SmQuotationsVendor _quotation = eSupplierDataMain.Bll.SmQuotationsVendor.Load(_MTMLInterchage.BuyerSuppInfo.RecordID);
                            if (_quotation != null)
                            {
                                _quotation.UpdateDate = DateTime.Now;
                                _quotation.Exported = 2;
                                _quotation.Update();
                            }
                        }
                        catch (Exception ex)
                        {
                            DataLog.AddMaileSupplierLog("57. Unable to export eSupplier internal " + _BuyerSuppInfo.DocType + " files. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                        }
                        finally
                        {
                            _MTMLInterchage = null; _export = null; GC.Collect();
                        }
                    }
                }
            }
            finally
            {
                dsDirectories.Dispose(); _BuyerSuppInfo = null;
                GC.Collect();
            }
        }

        private void Export_FinalQuote_EPB()
        {
            LES_EPB.Routine.EPBPushData _LeSEPB;
            DataSet dsRecords = new DataSet();
            BuyerSupplierInfo _BuyerSuppInfo = new BuyerSupplierInfo();

            dsRecords = _dataAccess.GetQueryDataset("SELECT * FROM SMV_EPB_PUSHDATA_FINAL_QUOTE ");
            if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsRecords))
            {
                foreach (DataRow row in dsRecords.Tables[0].Rows)
                {
                    try
                    {
                        _LeSEPB = new LES_EPB.Routine.EPBPushData();
                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, "", null);

                        if (_LeSEPB.PushFinalQuoteToEPB(convert.ToInt(row["QUOTATIONID"]), _BuyerSuppInfo))
                        {
                            eSupplierDataMain.Bll.SmQuotationsVendor _quotation = eSupplierDataMain.Bll.SmQuotationsVendor.Load(_BuyerSuppInfo.RecordID);
                            if (_quotation != null)
                            {
                                _quotation.UpdateDate = DateTime.Now;
                                _quotation.Exported = 3;
                                _quotation.VendorStatus = 4;
                                _quotation.Update();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("58. Unable to push data to EPB. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                }
            }
            dsRecords.Dispose();
            _LeSEPB = null; _BuyerSuppInfo = null;
            GC.Collect();
        }

        private void Import_EDI_Buyer_Docs()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            int cnt = 0;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_EDI_BYR_DOCS ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                        files = dir.GetFiles("*.*");

                        if (files.Length > 0)
                        {
                            foreach (FileInfo f in files)
                            {
                                if (cnt > 5) { pRestartService = true; return; }
                                else cnt++;

                                EDI_MTML_Routine.EDI_Buyer_Docs _EDI_MTML = new EDI_MTML_Routine.EDI_Buyer_Docs();

                                _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, null);
                                try
                                {
                                    if (_smRoutines.IsFileAvailable(f.FullName))
                                    {
                                        if (f.Extension.ToUpper().Contains("XML"))
                                        {
                                            Boolean IsTimeOutOrDeadLock = false;
                                            if (_EDI_MTML.Import_RFQ_PO_Files(f.FullName, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                                _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                            else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                        }
                                        else _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataLog.AddMaileSupplierLog("59. Unable to import AMOS2 file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                }
                                finally
                                {
                                    _EDI_MTML = null; GC.Collect();
                                }
                            }
                        }
                    }// for each
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("60. Unable to import AMOS2 file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dsDirectories.Dispose(); dir = null; files = null;
                GC.Collect();
            }
        }

        private void Import_SEA_PROC_Buyer_Docs()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            int cnt = 0;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_SEA_PROC_BYR_DOCS ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                        files = dir.GetFiles("*.xml");

                        if (files.Length > 0)
                        {
                            foreach (FileInfo f in files)
                            {
                                if (cnt > 5) { pRestartService = true; return; }
                                else cnt++;

                                SEAPROC_MTML_Routine.SEAPROC_MAIN _SEAProc_MTML = new SEAPROC_MTML_Routine.SEAPROC_MAIN();

                                _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, null);
                                try
                                {
                                    if (_smRoutines.IsFileAvailable(f.FullName))
                                    {
                                        if (f.Extension.ToUpper().Contains("XML"))
                                        {
                                            Boolean IsTimeOutOrDeadLock = false;
                                            if (_SEAProc_MTML.Import_MTML_Files(f.FullName, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                                _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                            else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                        }
                                        else _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataLog.AddMaileSupplierLog("383. Unable to import SEA PROC file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                }
                                finally
                                {
                                    _EDI_MTML = null; GC.Collect();
                                }
                            }
                        }
                    }// for each
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("384. Unable to import SEA PROC file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dsDirectories.Dispose(); dir = null; files = null;
                GC.Collect();
            }
        }

        private void ImportMailQueueText()
        {
            string cPath = System.Configuration.ConfigurationSettings.AppSettings["SEND_MAIL_AUDIT_INBOX"].ToString();
            string[] files = null;
            if (!Directory.Exists(cPath)) Directory.CreateDirectory(cPath);
            files = Directory.GetFiles(cPath, "*.txt");
            Boolean isUpdated = false;

            foreach (string cFile in files)
            {
                FileStream fs = null;
                StreamReader sr = null;
                try
                {
                    string Source = "";
                    fs = new FileStream(cFile, FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(fs);

                    Source = sr.ReadToEnd();
                    if (!string.IsNullOrEmpty(Source))
                    {
                        string[] slAudit;
                        DateTime dtUpdate = DateTime.Now;

                        if (convert.ToString(Source).Trim().Length > 0)
                        {
                            slAudit = convert.ToString(Source).Split('|');
                            if (slAudit.Length > 10)
                            {
                                string cMailSubject = "", cTo = "", cCC = "";
                                if (convert.ToInt(slAudit[0]) > 0)
                                {
                                    BuyerSupplierInfo _byrSuppInfo = new BuyerSupplierInfo();
                                    _byrSuppInfo.FileName = slAudit[9];
                                    _byrSuppInfo = _smRoutines.GetBuyerSupplierInfo( convert.ToInt(slAudit[0]), 0,  slAudit[1], _byrSuppInfo);

                                    _byrSuppInfo.UDF3 = "EXTERNAL";
                                    _smRoutines.SendMailNotify(_byrSuppInfo, _byrSuppInfo.FileName);
                                }
                                else
                                {   // Send Mail Queue entry
                                    eSupplierDataMain.Bll.SmSendMailQueue _mailQueue = new eSupplierDataMain.Bll.SmSendMailQueue();
                                    _mailQueue.DocType = slAudit[1];
                                    _mailQueue.RefKey = slAudit[2];
                                    _mailQueue.MailFrom = slAudit[3];
                                    if (!string.IsNullOrEmpty(slAudit[4]))
                                        _mailQueue.MailTo = slAudit[4];
                                    else _mailQueue.MailTo = cTo;

                                    _mailQueue.MailCc = slAudit[5];
                                    if (!string.IsNullOrEmpty(slAudit[6]))
                                        _mailQueue.MailBcc = slAudit[6];
                                    else _mailQueue.MailBcc = ConfigurationSettings.AppSettings["MAIL_BCC"].ToString();

                                    if (!string.IsNullOrEmpty(cMailSubject))
                                    {

                                    }
                                    else _mailQueue.MailSubject = slAudit[7];

                                    _mailQueue.MailBody = slAudit[8];
                                    _mailQueue.Attachments = slAudit[9];
                                    _mailQueue.MailDate = DateTime.Now;
                                    _mailQueue.NotToSent = convert.ToInt(slAudit[11]);
                                    if (slAudit.Length > 12)
                                        _mailQueue.BuyerId = convert.ToInt(slAudit[12]);
                                    if (slAudit.Length > 13)
                                        _mailQueue.SupplierId = convert.ToInt(slAudit[13]);
                                    if (slAudit.Length > 14)
                                        _mailQueue.ReplyEmail = slAudit[14];
                                    if (slAudit.Length > 15)
                                        _mailQueue.SenderName = slAudit[15];
                                    if (slAudit.Length > 16)
                                        _mailQueue.ReceiverName = slAudit[16];
                                    if (slAudit.Length > 17)
                                        _mailQueue.ActionType = slAudit[17];
                                    if (slAudit.Length > 18)
                                        _mailQueue.HtmlNotToSend = convert.ToInt(slAudit[18]);
                                    if (slAudit.Length > 19)
                                        _mailQueue.SendHtmlMsg = convert.ToInt(slAudit[19]);
                                    if (slAudit.Length > 20)
                                        _mailQueue.UseHtmlFileMsg = convert.ToInt(slAudit[20]);
                                    if (slAudit.Length > 21)
                                        _mailQueue.DelayMailMin = convert.ToInt(slAudit[21]);
                                    if (slAudit.Length > 22)
                                        _mailQueue.SupplierId = convert.ToInt(slAudit[22]);
                                    if (slAudit.Length > 23)
                                        _mailQueue.BuyerId = convert.ToInt(slAudit[23]);

                                    int nMailQueueID = 0;
                                    if (convert.ToString(_mailQueue.ActionType).ToUpper() == "SUBMITTED")
                                        nMailQueueID = GetMailQueueID(_mailQueue);

                                    if (nMailQueueID > 0)
                                    {
                                        _dataAccess.execQuery("UPDATE SM_SEND_MAIL_QUEUE SET NOT_TO_SENT=0 WHERE QUEUE_ID=" + nMailQueueID);
                                    }
                                    else _mailQueue.Insert();

                                    _mailQueue = null;
                                }
                                GC.Collect();
                            }
                        }
                    }
                    isUpdated = true;
                }
                catch (Exception ex) //Changed Sayli 27Apr16: Added (Exception ex)
                {
                    isUpdated = false;
                    string cAudit = "61. Unable to read Mail Queue error log file " + Path.GetFileName(cFile) + ". Error - " + ex.StackTrace;
                    DataLog.AddLog(cAudit);
                    //DataLog.DBAuditLog(cAudit, "Error", null);
                }
                finally
                {
                    fs.Flush();
                    sr.Close(); sr.Dispose();
                    fs.Close(); fs.Dispose();
                    if (sr != null) { sr = null; }
                    if (fs != null) { fs = null; }
                    GC.Collect();
                }

                if (isUpdated) _smRoutines.MoveFiles(cFile, cPath + "\\BACKUP");
                else _smRoutines.MoveFiles(cFile, cPath + "\\ERROR_FILES");
            }
            files = null; GC.Collect();
        }

        private int GetMailQueueID(eSupplierDataMain.Bll.SmSendMailQueue _mailQueue)
        {
            int nReturn = 0;
            try
            {
                DataSet _dsData = new DataSet();
                _dsData = _dataAccess.GetQueryDataset(" SELECT * FROM SM_SEND_MAIL_QUEUE WHERE NOT_TO_SENT=1 AND (QUOTATIONID=" + convert.ToInt(_mailQueue.Quotationid) + " OR REF_KEY=" + convert.ToQuote(_mailQueue.RefKey) +")");

                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(_dsData))
                {
                    nReturn = convert.ToInt(_dsData.Tables[0].Rows[0]["QUEUE_ID"].ToString());
                }
            }
            catch (Exception ex)
            {
                string cAudit = "61.1 Unable to read Mail Queue " + ", Error - " + ex.StackTrace;
                DataLog.AddLog(cAudit);
                nReturn = 0;
            }
            return nReturn;
        }

        private void Export_RMS_Excel_DOC_RFQ_PO()
        {
            //Debugger.Break();

            // export RMS Excel RFQ documents
            ExportRMSExcel_DocumentsFiles("SMV_EXPORT_RMS_EXCEL_DOCUMENT_RFQ", "RFQ");

            // export RMS Excel PO documents
            ExportRMSExcel_DocumentsFiles("SMV_EXPORT_RMS_EXCEL_DOCUMENT_PO", "PO");
        }

        private void ExportRMSExcel_DocumentsFiles(string cTableName, string cDocType)
        {
            //Debugger.Break();
            _BuyerSuppInfo = new BuyerSupplierInfo();
            DataSet dsDirectories = new DataSet();
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset("Select * from " + cTableName);
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow row in dsDirectories.Tables[0].Rows)
                    {
                        try
                        {
                            RMS.PQC.XLS.RFQ.Routine.EXPORT_XLS_DOC_Routine _RMSExport = new RMS.PQC.XLS.RFQ.Routine.EXPORT_XLS_DOC_Routine();
                            _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, "", null);
                            _RMSExport.ExportXLSDocument(convert.ToInt(row["QUOTATIONID"]), _BuyerSuppInfo);
                        }
                        catch (Exception ex)
                        {
                            DataLog.AddMaileSupplierLog("62. Unable to  export RMS excel " + cDocType + " file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                        }
                        finally
                        {
                            _exportXLSDoc = null; GC.Collect();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DataLog.AddMaileSupplierLog("63. Unable to export RMS excel " + cDocType + " files. Error - " + ex.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                _BuyerSuppInfo = null; dsDirectories.Dispose();
                GC.Collect();
            }
        }

        private void Import_RMS_Excel_Documents_SUPP()
        {
            //Debugger.Break();
            string _impPath = "";
            DataSet dsDirectories = new DataSet();
            DirectoryInfo dir = null;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset("SELECT * FROM SMV_IMPORT_RMS_EXCEL_DOCS_SUPP ");

                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);
                        _BuyerSuppInfo.BuyerID = convert.ToInt(strdir["BUYERID"]);
                        _BuyerSuppInfo.SupplierID = convert.ToInt(strdir["SUPPLIERID"]);

                        _impPath = Convert.ToString(strdir["ADDR_INBOX"]);
                        if (_impPath.Trim().Length == 0) continue;

                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                        FileInfo[] files = dir.GetFiles("*.*");

                        foreach (FileInfo f in files)
                        {
                            _BuyerSuppInfo.FileName = f.Name;
                            _BuyerSuppInfo.FileFullName = f.FullName;

                            RMS.PQC.XLS.RFQ.Routine.IMPORT_XLS_DOC_Routine _RMSImport = new RMS.PQC.XLS.RFQ.Routine.IMPORT_XLS_DOC_Routine();
                            if (_smRoutines.IsFileAvailable(f.FullName))
                            {
                                Boolean IsTimeOutOrDeadLock = false;
                                if (_RMSImport.ImportXLSDocuments(f.FullName, _BuyerSuppInfo, RoleType.SUPPLIER, ref IsTimeOutOrDeadLock))
                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                            }
                            else DataLog.AddLog("File " + f.FullName + " used by another process");

                            _RMSImport = null; GC.Collect();
                        }

                        _BuyerSuppInfo = null; GC.Collect();
                    }
                }
            }
            catch (Exception ex)
            {
                DataLog.AddMaileSupplierLog("64. Unable to import RMS excel doc file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dsDirectories.Dispose(); dir = null;
                GC.Collect();
            }
        }

        private void ImportMailDownloadLogFiles()
        {
            string cPath = System.Configuration.ConfigurationSettings.AppSettings["MAIL_DOWNLOAD_AUDIT"].ToString();
            string[] files = null;
            if (!Directory.Exists(cPath)) Directory.CreateDirectory(cPath);
            files = Directory.GetFiles(cPath, "*.txt");
            Boolean isUpdated = false;

            foreach (string cFile in files)
            {
                FileStream fs = null;
                StreamReader sr = null;
                try
                {
                    string Source = "";
                    fs = new FileStream(cFile, FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(fs);

                    while ((Source = sr.ReadLine()) != null)
                    {
                        string[] slAudit;
                        DateTime dtUpdate = DateTime.Now;

                        if (convert.ToString(Source).Trim().Length > 0)
                        {
                            slAudit = convert.ToString(Source).Split('|');
                            if (slAudit.Length > 6)
                            {
                                eSupplierDataMain.Bll.SmMailDownloadLog _mailLog = new eSupplierDataMain.Bll.SmMailDownloadLog();
                                if (slAudit.Length > 0) _mailLog.Messageid = slAudit[0];
                                if (slAudit.Length > 1) _mailLog.Modulename = slAudit[1];
                                if (slAudit.Length > 2) _mailLog.Logtype = slAudit[2];
                                if (slAudit.Length > 3) _mailLog.Frommail = slAudit[3];
                                if (slAudit.Length > 4) _mailLog.Tomail = slAudit[4];
                                if (slAudit.Length > 5) _mailLog.Auditvalue = slAudit[5];
                                if (slAudit.Length > 6) _mailLog.Keyref1 = slAudit[6];
                                if (slAudit.Length > 7) _mailLog.Keyref2 = slAudit[7];
                                if (slAudit.Length > 8) _mailLog.BuyerId = convert.ToInt(slAudit[8]);
                                if (slAudit.Length > 9) _mailLog.SupplierId = convert.ToInt(slAudit[9]);
                                if (slAudit.Length > 10) _mailLog.MailSubject = slAudit[10];
                                _mailLog.Updatedate = DateTime.Now;

                                _mailLog.Insert();

                                _mailLog = null; GC.Collect();
                            }
                        }
                    }
                    isUpdated = true;
                }
                catch (Exception ex) //Changed Sayli 27Apr16: Added (Exception ex)
                {
                    isUpdated = false;
                    string cAudit = "65. Unable to read Mail download audit log file " + Path.GetFileName(cFile) + ". Error - " + ex.StackTrace;
                    DataLog.AddLog(cAudit);
                    //DataLog.DBAuditLog(cAudit, "Error", null);
                }
                finally
                {
                    fs.Flush();
                    sr.Close(); sr.Dispose();
                    fs.Close(); fs.Dispose();
                    if (sr != null) { sr = null; }
                    if (fs != null) { fs = null; }
                    GC.Collect();
                }

                if (isUpdated) _smRoutines.MoveFiles(cFile, cPath + "\\BACKUP");
                else _smRoutines.MoveFiles(cFile, cPath + "\\ERROR_FILES");
            }
            files = null; GC.Collect();
        }

        private void btnCallDll_Click(object sender, EventArgs e)
        {
            try
            {
                DataLog.AddLog("Assembly path - " + Application.StartupPath + "\\" + "SEA_TANKER_Excel_RFQQuote_Routine.dll");
                Assembly objAsm = Assembly.LoadFile(Application.StartupPath + "\\" + "SEA_TANKER_Excel_RFQQuote_Routine.dll");
                Type typeAsm = objAsm.GetType("SEA_TANKER_Excel_RFQ_Quote_Routine.ImportXLSDocument");

                DataLog.AddLog("-------------------------------------");
                DataLog.AddLog("After get asmb class ");
                DataLog.AddLog("asmb type -" + typeAsm);

                foreach (AssemblyName refAsb in objAsm.GetReferencedAssemblies())
                {
                   // DataLog.AddLog(" Ref assemlies " + refAsb.FullName);
                }

                foreach (MethodInfo item in typeAsm.GetMethods())
                {
                    //if (!item.IsClass) continue;
                   DataLog.AddLog("name - "+ item.Name);                    
                }
                DataLog.AddLog("after for each");
                
                object data = Activator.CreateInstance(typeAsm, true);
                //object data = Activator.CreateInstance("SEA_TANKER_Excel_RFQ_Quote_Routine", "SEA_TANKER_Excel_RFQ_Quote_Routine.ImportXLSDocument");
                DataLog.AddLog("After get asmb instance");

                //calcType.InvokeMember("AddLog", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, data, new object[] { "1" });
                MethodInfo Method = objAsm.GetTypes()[0].GetMethod(convert.ToString("ImportDocument"));// "ImportXLSDocuments");
                DataLog.AddLog("Method - " + data);

                Boolean Result = (Boolean)Method.Invoke(data, new object[] { "TEST File", "BUYER" });

            }catch(Exception ex)
            {
                DataLog.AddLog("Error in DLL call dynamic - " + ex.Message);
            }
        }

        private void Export_SEA_PROC_QuotePOC()
        {
            DataSet dsDirectories = new DataSet();
            _BuyerSuppInfo = new BuyerSupplierInfo();
            string cDocType = "";
            try
            {
                // export SEA Proc MTML Quote / POC files 
                dsDirectories = _dataAccess.GetQueryDataset("Select * from SMV_EXPORT_SEA_PROC_QUOTE_POC");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow row in dsDirectories.Tables[0].Rows)
                    {
                        SEAPROC_MTML_Routine.SEAPROC_MAIN _SEAProc = new SEAPROC_MTML_Routine.SEAPROC_MAIN();
                        try
                        {
                            cDocType = row["DOC_TYPE"].ToString();
                            _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, "", null);
                            _SEAProc.Export_MTML_Files(convert.ToInt(row["QUOTATIONID"]), cDocType, _BuyerSuppInfo);
                        }
                        catch (Exception ex)
                        {
                            DataLog.AddMaileSupplierLog("385. Unable to  export SEA PROC " + cDocType + " file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                        }
                        finally
                        {
                            _SEAProc = null; GC.Collect();
                        }
                    }
                }
            }
            finally
            {
                dsDirectories.Dispose(); _BuyerSuppInfo = null;
                GC.Collect();
            }
        }

        private void Export_ShipServ_SuppDoc()
        {
            string cQuoteIDs; string[] slQuotes = { };
            SHIPSERV_MTML_Routine.ShipServ_Supp_Docs _SS_MTML = new SHIPSERV_MTML_Routine.ShipServ_Supp_Docs();
            int nQuoteID;
            _BuyerSuppInfo = new BuyerSupplierInfo();

            // ShipServ - MTML Export RFQ files
            cQuoteIDs = _dataAccess.GetResultCommaString(" Select QUOTATIONID from SMV_EXPORT_SHIPSERV_MTML_RFQ ");
            if (cQuoteIDs.Length > 0) slQuotes = cQuoteIDs.Split(',');

            if (slQuotes.Length > 0)
            {
                foreach (string cQuoteID in slQuotes)
                {
                    try
                    {
                        nQuoteID = convert.ToInt(cQuoteID);
                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(nQuoteID, 0, "", null);
                        _SS_MTML.Export_ShipServ_Files(nQuoteID, "RequestForQuote", _BuyerSuppInfo);
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("394. Unable to export SHIPSERV-MTML RFQ file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                }
            }

            // ShipServ -MTML Export PO files
            slQuotes = new string[0];
            cQuoteIDs = _dataAccess.GetResultCommaString(" Select QUOTATIONID from SMV_EXPORT_SHIPSERV_MTML_PO ");
            if (cQuoteIDs.Length > 0) slQuotes = cQuoteIDs.Split(',');

            if (slQuotes.Length > 0)
            {
                foreach (string cQuoteID in slQuotes)
                {
                    try
                    {
                        nQuoteID = convert.ToInt(cQuoteID);
                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(nQuoteID, 0, "", null);
                        _SS_MTML.Export_ShipServ_Files(nQuoteID, "Order", _BuyerSuppInfo);
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("395. Unable to export SHIPSERV-MTML file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                }
            }

            _SS_MTML = null; _BuyerSuppInfo = null;
            GC.Collect();
        }

        private void Import_SHIP_SERV_QuotePOC()
        {
            //Debugger.Break();
            string _impPath = "";
            DataSet dsDirectories = new DataSet();
            DirectoryInfo dir = null;
            SHIPSERV_MTML_Routine.ShipServ_Supp_Docs _SSDoc = new SHIPSERV_MTML_Routine.ShipServ_Supp_Docs();

            try
            {
                dsDirectories = _dataAccess.GetQueryDataset("SELECT * FROM SMV_IMPORT_SHIP_SERV_SUPP_DOCS ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _impPath = Convert.ToString(strdir["ADDR_INBOX"]);
                        if (_impPath.Trim().Length == 0) continue;

                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);
                        _BuyerSuppInfo.GroupCode = convert.ToString(strdir["GROUP_CODE"]);
                        _BuyerSuppInfo.BuyerID = convert.ToInt(strdir["BUYERID"]);
                        _BuyerSuppInfo.SupplierID = convert.ToInt(strdir["SUPPLIERID"]);

                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);
                        FileInfo[] files = dir.GetFiles("*.xml");

                        foreach (FileInfo f in files)
                        {
                            _BuyerSuppInfo.FileName = f.Name;
                            _BuyerSuppInfo.FileFullName = f.FullName;

                            if (_smRoutines.IsFileAvailable(f.FullName))
                            {
                                Boolean IsTimeOutOrDeadLock = false;
                                if (_SSDoc.Import_ShipServ_Files(f.FullName, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");

                            }
                            else DataLog.AddLog("File " + f.FullName + " used by another process");
                        }

                        _BuyerSuppInfo = null; GC.Collect();
                    }
                }
            }
            catch (Exception ex)
            {
                DataLog.AddMaileSupplierLog("397. Unable to import ShipServ MTML file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dir = null; dsDirectories.Dispose(); _SSDoc = null;
                GC.Collect();
            }
        }

        private void Import_SHIP_SERV_Buyer_Docs()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            int cnt = 0;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_SHIP_SERV_BYR_DOCS ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                        files = dir.GetFiles("*.*");

                        if (files.Length > 0)
                        {
                            foreach (FileInfo f in files)
                            {
                                if (cnt > 5) { pRestartService = true; return; }
                                else cnt++;

                                SHIPSERV_MTML_Routine.ShipServ_Byr_Docs _SS_MTML = new SHIPSERV_MTML_Routine.ShipServ_Byr_Docs();

                                _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, null);
                                try
                                {
                                    if (_smRoutines.IsFileAvailable(f.FullName))
                                    {
                                        if (f.Extension.ToUpper().Contains("XML"))
                                        {
                                            Boolean IsTimeOutOrDeadLock = false;
                                            if (_SS_MTML.Import_RFQ_PO_Files(f.FullName, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                                _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                            else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                        }
                                        else _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataLog.AddMaileSupplierLog("402. Unable to import ShipServ file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                }
                                finally
                                {
                                    _EDI_MTML = null; GC.Collect();
                                }
                            }
                        }
                    }// for each
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("403. Unable to import ShipServ file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dsDirectories.Dispose(); dir = null; files = null;
                GC.Collect();
            }
        }

        private void Export_SHIP_SERV_Buyer_Docs()
        {
            DataSet dsDirectories = new DataSet();
            _BuyerSuppInfo = new BuyerSupplierInfo();
            string cDocType = "";
            try
            {
                // export ShipServ MTML Quote / POC files 
                dsDirectories = _dataAccess.GetQueryDataset("Select * from SMV_EXPORT_SHIPSERV_QUOTE_POC");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow row in dsDirectories.Tables[0].Rows)
                    {
                        SHIPSERV_MTML_Routine.ShipServ_Byr_Docs _SSByr = new SHIPSERV_MTML_Routine.ShipServ_Byr_Docs();
                        try
                        {
                            cDocType = row["DOC_TYPE"].ToString();
                            _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, "", null);
                            _SSByr.Export_Quote_POC_Files(convert.ToInt(row["QUOTATIONID"]), cDocType, _BuyerSuppInfo);
                        }
                        catch (Exception ex)
                        {
                            DataLog.AddMaileSupplierLog("406. Unable to export ShipServ " + cDocType + " file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                        }
                        finally
                        {
                            _SSByr = null; GC.Collect();
                        }
                    }
                }
            }
            finally
            {
                dsDirectories.Dispose(); _BuyerSuppInfo = null;
                GC.Collect();
            }
        }
        
        private void Export_EDI_Buyer_Docs()
        {
             DataSet dsDirectories = new DataSet();
            _BuyerSuppInfo = new BuyerSupplierInfo();
            string cDocType = "";
            try
            {
                // export EDI MTML Quote / POC files 
                dsDirectories = _dataAccess.GetQueryDataset("Select * from SMV_EXPORT_EDI_QUOTE_POC");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow row in dsDirectories.Tables[0].Rows)
                    {
                        EDI_MTML_Routine.EDI_Buyer_Docs _EDIByr = new EDI_MTML_Routine.EDI_Buyer_Docs();
                        try
                        {
                            cDocType = row["DOC_TYPE"].ToString();
                            _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, "", null);
                            _EDIByr.Export_Quote_POC_Files(convert.ToInt(row["QUOTATIONID"]), cDocType, _BuyerSuppInfo);
                        }
                        catch (Exception ex)
                        {
                            DataLog.AddMaileSupplierLog("407. Unable to export EDI " + cDocType + " file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                        }
                        finally
                        {
                            _EDIByr = null; GC.Collect();
                        }
                    }
                }
            }
            finally
            {
                dsDirectories.Dispose(); _BuyerSuppInfo = null;
                GC.Collect();
            }
        }

        private void Import_MTML_V2_Buyer_Docs()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            int cnt = 0;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_MTML_V2_BYR_DOCS ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                        files = dir.GetFiles("*.*");

                        if (files.Length > 0)
                        {
                            foreach (FileInfo f in files)
                            {
                                if (cnt > 5) { pRestartService = true; return; }
                                else cnt++;

                                MTML_V2_Routine.MTML_V2_Byr_Docs _MTML_V2 = new MTML_V2_Routine.MTML_V2_Byr_Docs();

                                _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, null);
                                try
                                {
                                    if (_smRoutines.IsFileAvailable(f.FullName))
                                    {
                                        if (f.Extension.ToUpper().Contains("XML"))
                                        {
                                            Boolean IsTimeOutOrDeadLock = false;
                                            if (_MTML_V2.Import_RFQ_PO_Files(f.FullName, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                                _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                            else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                        }
                                        else _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataLog.AddMaileSupplierLog("402. Unable to import ShipServ file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                }
                                finally
                                {
                                    _MTML_V2 = null; GC.Collect();
                                }
                            }
                        }
                    }// for each
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("417. Unable to import MTML-V2 file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dsDirectories.Dispose(); dir = null; files = null;
                GC.Collect();
            }
        }

        private void Export_MTML_V2_Buyer_Docs()
        {
            DataSet dsDirectories = new DataSet();
            _BuyerSuppInfo = new BuyerSupplierInfo();
            string cDocType = "";
            try
            {
                // export MTML-V2 Quote / POC files 
                dsDirectories = _dataAccess.GetQueryDataset("Select * from SMV_EXPORT_MTML_V2_QUOTE_POC");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow row in dsDirectories.Tables[0].Rows)
                    {
                        MTML_V2_Routine.MTML_V2_Byr_Docs _MTML_V2 = new MTML_V2_Routine.MTML_V2_Byr_Docs();
                        try
                        {
                            cDocType = row["DOC_TYPE"].ToString();
                            _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, "", null);
                            _MTML_V2.Export_Quote_POC_Files(convert.ToInt(row["QUOTATIONID"]), cDocType, _BuyerSuppInfo);
                        }
                        catch (Exception ex)
                        {
                            DataLog.AddMaileSupplierLog("418. Unable to export MTML-V2 " + cDocType + " file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                        }
                        finally
                        {
                            _MTML_V2 = null; GC.Collect();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                DataLog.AddMaileSupplierLog("418-2. Unable to export MTML-V2 " + cDocType + " file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dsDirectories.Dispose(); _BuyerSuppInfo = null;
                GC.Collect();
            }
        }

        private void ResendMailNotify()
        {
            DataSet dsDirectories = new DataSet();
            _BuyerSuppInfo = new BuyerSupplierInfo();
            string cDocType = "";
            try
            {
                // Resend Notify for the document 
                dsDirectories = _dataAccess.GetQueryDataset("Select * from LES_RESEND_DOC_NOTIFY ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow row in dsDirectories.Tables[0].Rows)
                    {                        
                        try
                        {                            
                            _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATION_ID"]), 0, "", null);
                            _smRoutines.SendMailNotify(_BuyerSuppInfo, "");
                            _dataAccess.execQuery("DELETE FROM LES_RESEND_DOC_NOTIFY WHERE QUOTATION_ID=" + _BuyerSuppInfo.RecordID);
                        }
                        catch (Exception ex)
                        {
                            DataLog.AddMaileSupplierLog("419. Unable to resend mail notify. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                        }
                        finally
                        {
                            GC.Collect();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DataLog.AddMaileSupplierLog("419-2. Unable to resend mail notify. Error - " + ex.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dsDirectories.Dispose(); _BuyerSuppInfo = null;
                GC.Collect();
            }
        }

        private void Export_ESUPPLIER_RFQ_PO()
        {
            DataSet dsDirectories = new DataSet();
            _BuyerSuppInfo = new BuyerSupplierInfo();

            try
            {
                // export MTML internal RFQ / PO files 
                dsDirectories = _dataAccess.GetQueryDataset("Select * from SMV_EXPORT_ESUPPLIER_INTERNAL_RFQ_PO");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow row in dsDirectories.Tables[0].Rows)
                    {
                        MTMLInterchange _MTML = new MTMLInterchange();
                        MtmlExport _export = new MtmlExport();
                        try
                        {
                            _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, row["DOC_TYPE"].ToString(), null);
                            _MTML = _export.Export(convert.ToInt(row["QUOTATIONID"]), row["DOC_TYPE"].ToString());

                            if (_MTML != null)
                            {
                                eSupplierDataMain.Bll.SmBuyerSupplierLink _smLink = eSupplierDataMain.Bll.SmBuyerSupplierLink.Load(_BuyerSuppInfo.ByrSuplrLinkID);
                                string cOutPath = _smLink.SuppExportPath;
                                string cMTMLFile = cOutPath + "\\" + convert.ToFileName("MTML_" + _MTML.BuyerSuppInfo.DocType + "_" + _MTML.BuyerSuppInfo.REF_No + "_" + _MTML.BuyerSuppInfo.BuyerCode + "_" + _MTML.BuyerSuppInfo.SupplierCode + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".xml");

                                if (!string.IsNullOrEmpty(_smLink.SuppReceiverCode))
                                    _MTML.Recipient = _smLink.SuppReceiverCode;
                                if (!string.IsNullOrEmpty(_smLink.SuppSenderCode))
                                    _MTML.Sender = _smLink.SuppSenderCode;

                                MTMLClass _MTMLClass = new MTMLClass();
                                _MTMLClass.Create(_MTML, cMTMLFile); // Save MTML Quote File

                                _MTML.BuyerSuppInfo.FileName =Path.GetFileName(cMTMLFile);
                                _MTML.BuyerSuppInfo.Export_HTML_Msg = 1;
                                _smRoutines.SendMailNotify(_MTML.BuyerSuppInfo, "");

                                //if (_smRoutines.GetRuleValue(_MTMLInterchage, "SYSTEM_COPY_SUBMITTED_FILE") == "1")
                                //{
                                //    string cPath = cOutPath;
                                //    List<string> slAttach = _smRoutines.GetDocumentAttachments(_MTMLInterchage.BuyerSuppInfo, null, RoleType.BUYER, ModuleType.Notify);
                                //    if (slAttach != null && (slAttach.Count > 0))
                                //    {
                                //        string[] cfile = slAttach[0].Split('\\');
                                //        string fName = cfile[cfile.Length - 1];
                                //        if (File.Exists(cPath + "\\" + fName)) File.Delete(cPath + "\\" + fName);
                                //        File.Copy(slAttach[0], cPath + "\\" + fName);
                                //    }
                                //}

                                eSupplierDataMain.Bll.SmQuotationsVendor _SuppDoc = eSupplierDataMain.Bll.SmQuotationsVendor.Load(_MTML.BuyerSuppInfo.RecordID);
                                if (_SuppDoc != null)
                                {
                                    _SuppDoc.UpdateDate = DateTime.Now;
                                    if (_MTML.BuyerSuppInfo.DocumentType == DocumentType.RFQ)
                                        _SuppDoc.RfqExport = 0;
                                    else _SuppDoc.Exported = 0;
                                    _SuppDoc.Update();
                                }
                                DataLog.AddLog("Esupplier MTML file successfully exported - " + Path.GetFileName(cMTMLFile));
                                _smRoutines.CopyFileToAudit(cMTMLFile);
                                DataLog.DBAuditLog("Esupplier MTML file successfully exported - " + Path.GetFileName(cMTMLFile), "Converted", _MTML.BuyerSuppInfo);
                            }
                        }
                        catch (Exception ex)
                        {
                            DataLog.AddMaileSupplierLog("420. Unable to export eSupplier internal " + _BuyerSuppInfo.DocType + " files. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                        }
                        finally
                        {
                            _MTML = null; _export = null; GC.Collect();
                        }
                    }
                }
            }
            finally
            {
                dsDirectories.Dispose(); _BuyerSuppInfo = null;
                GC.Collect();
            }
        }

        private void Export_eLite_RFQ_PO()
        {
            DataSet dsDirectories = new DataSet();
            _BuyerSuppInfo = new BuyerSupplierInfo();

            try
            {
                // export eSupplier Lite RFQ / PO files 
                dsDirectories = _dataAccess.GetQueryDataset("Select * from SMV_EXPORT_ESUPPLIER_LITE_RFQ_PO");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow row in dsDirectories.Tables[0].Rows)
                    {
                        try
                        {
                            _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(row["QUOTATIONID"]), 0, row["DOC_TYPE"].ToString(), null);

                            if (_BuyerSuppInfo != null)
                            {
                                eSupplierDataMain.Bll.SmQuotationsVendor _SuppDoc = eSupplierDataMain.Bll.SmQuotationsVendor.Load(_BuyerSuppInfo.RecordID);
                                if (_SuppDoc != null)
                                {
                                    string cHtmlFile = _smRoutines.GetHTMLFileName(_BuyerSuppInfo.RecordID);

                                    if (!File.Exists(cHtmlFile))
                                    {
                                        MTMLInterchange _MTML = new MTMLInterchange();
                                        MtmlExport _export = new MtmlExport();
                                        _MTML = _export.Export(_BuyerSuppInfo.RecordID, _BuyerSuppInfo.DocType);

                                        _smRoutines.ExportDocHtmlFile(_MTML);
                                    }
                                    else if (_smRoutines.GetRuleValue(_BuyerSuppInfo, "SYSTEM_COPY_MTML_EXPORT_PATH") == "2")
                                    {
                                        MTMLInterchange _MTML = new MTMLInterchange();
                                        MtmlExport _export = new MtmlExport();
                                        _MTML = _export.Export(_BuyerSuppInfo.RecordID, _BuyerSuppInfo.DocType);
                                    }

                                    if (File.Exists(cHtmlFile))
                                    {
                                        if (ConfigurationSettings.AppSettings["ESUPPLIER_eLITE_FILES"] != null)
                                            _smRoutines.CopyFiles(cHtmlFile, ConfigurationSettings.AppSettings["ESUPPLIER_eLITE_FILES"] + "\\"+ Path.GetFileName(cHtmlFile));

                                        _smRoutines.SendMailNotify(_BuyerSuppInfo, "");
                                    }

                                    _SuppDoc.UpdateDate = DateTime.Now;
                                    if (_BuyerSuppInfo.DocumentType == DocumentType.RFQ)
                                        _SuppDoc.RfqExport = 0;
                                    else _SuppDoc.Exported = 0;
                                    _SuppDoc.Update();
                                }
                                DataLog.AddLog("Esupplier eLite file successfully exported - " + _BuyerSuppInfo.REF_No);
                            }
                        }
                        catch (Exception ex)
                        {
                            DataLog.AddMaileSupplierLog("422. Unable to export eSupplier eLite " + _BuyerSuppInfo.DocType + " files. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                        }
                        finally
                        {
                           GC.Collect();
                        }
                    }
                }
            }
            finally
            {
                dsDirectories.Dispose(); _BuyerSuppInfo = null;
                GC.Collect();
            }
        }

        private void Export_MTML_V2_SuppDoc()
        {
            string cQuoteIDs; string[] slQuotes = { };
            MTML_V2_Routine.MTML_V2_Supp_Docs _V2_MTML = new MTML_V2_Routine.MTML_V2_Supp_Docs();
            int nQuoteID;
            _BuyerSuppInfo = new BuyerSupplierInfo();

            // MTML V2 Export RFQ files
            cQuoteIDs = _dataAccess.GetResultCommaString(" Select QUOTATIONID from SMV_EXPORT_MTML_V2_RFQ ");
            if (cQuoteIDs.Length > 0) slQuotes = cQuoteIDs.Split(',');

            if (slQuotes.Length > 0)
            {
                foreach (string cQuoteID in slQuotes)
                {
                    try
                    {
                        nQuoteID = convert.ToInt(cQuoteID);
                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(nQuoteID, 0, "", null);
                        _V2_MTML.Export_MTML_V2_Files(nQuoteID, "RequestForQuote", _BuyerSuppInfo);
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("424. Unable to export MTML-V2 RFQ file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                }
            }

            // MTML V2 Export PO files
            slQuotes = new string[0];
            cQuoteIDs = _dataAccess.GetResultCommaString(" Select QUOTATIONID from SMV_EXPORT_MTML_V2_PO ");
            if (cQuoteIDs.Length > 0) slQuotes = cQuoteIDs.Split(',');

            if (slQuotes.Length > 0)
            {
                foreach (string cQuoteID in slQuotes)
                {
                    try
                    {
                        nQuoteID = convert.ToInt(cQuoteID);
                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(nQuoteID, 0, "", null);
                        _V2_MTML.Export_MTML_V2_Files(nQuoteID, "Order", _BuyerSuppInfo);
                    }
                    catch (Exception ex)
                    {
                        DataLog.AddMaileSupplierLog("425. Unable to export MTML-V2 Order file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                    }
                }
            }

            _V2_MTML = null; _BuyerSuppInfo = null;
            GC.Collect();
        }

        private void Import_MTML_V2_QuotePOC()
        {
            //Debugger.Break();
            string _impPath = "";
            DataSet dsDirectories = new DataSet();
            DirectoryInfo dir = null;
            MTML_V2_Routine.MTML_V2_Supp_Docs _V2_Doc = new MTML_V2_Routine.MTML_V2_Supp_Docs();

            try
            {
                dsDirectories = _dataAccess.GetQueryDataset("SELECT * FROM SMV_IMPORT_MTML_V2_SUPP_DOCS ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _impPath = Convert.ToString(strdir["ADDR_INBOX"]);
                        if (_impPath.Trim().Length == 0) continue;

                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);
                        _BuyerSuppInfo.GroupCode = convert.ToString(strdir["GROUP_CODE"]);
                        _BuyerSuppInfo.BuyerID = convert.ToInt(strdir["BUYERID"]);
                        _BuyerSuppInfo.SupplierID = convert.ToInt(strdir["SUPPLIERID"]);

                        dir = new DirectoryInfo(_impPath);
                        if (!dir.Exists) Directory.CreateDirectory(dir.FullName);
                        FileInfo[] files = dir.GetFiles("*.xml");

                        foreach (FileInfo f in files)
                        {
                            _BuyerSuppInfo.FileName = f.Name;
                            _BuyerSuppInfo.FileFullName = f.FullName;

                            if (_smRoutines.IsFileAvailable(f.FullName))
                            {
                                Boolean IsTimeOutOrDeadLock = false;
                                if (_V2_Doc.Import_MTML_V2_Files(f.FullName, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                            }
                            else DataLog.AddLog("File " + f.FullName + " used by another process");
                        }

                        _BuyerSuppInfo = null; GC.Collect();
                    }
                }
            }
            catch (Exception ex)
            {
                DataLog.AddMaileSupplierLog("426. Unable to import MTML-V2 Quote / POC file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                dir = null; dsDirectories.Dispose(); _V2_Doc = null;
                GC.Collect();
            }
        }

        // Added on 02-APR-18 //
        private void Import_VshipsPDF_Files()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            int cnt = 0;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_VSHIP_PDF ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        if (_impPath != null && _impPath.Trim().Length > 0)
                        {
                            dir = new DirectoryInfo(_impPath);
                            if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                            files = dir.GetFiles();
                            if (files.Length > 0)
                            {
                                foreach (FileInfo f in files)
                                {
                                    if (f.Extension.ToLower() == ".pdf")
                                    {
                                        if (cnt >= 5) { pRestartService = true; return; }
                                        else cnt++;

                                        DataLog.AddLog("Processing VSHIP PDF file - " + f.FullName);

                                        VSHIPS_PDF _pdf2Xml = new VSHIPS_PDF();
                                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, _BuyerSuppInfo);
                                        try
                                        {
                                            if (_smRoutines.IsFileAvailable(f.FullName))
                                            {
                                                Boolean IsTimeOutOrDeadLock = false;
                                                if (_pdf2Xml.ConvertPDFs(f, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                                else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            DataLog.AddMaileSupplierLog("4. Unable to import VSHIP PDF file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                        }
                                        finally
                                        {
                                            _pdf2Xml = null; GC.Collect();
                                        }
                                    }
                                }
                            }
                        }// for each
                    }
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("5. Unable to import PDF file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                files = null; dir = null; dsDirectories.Dispose();
                GC.Collect();
            }
        }

        // Added on 09-APR-18
        private void Import_JanDeNulPDF_Files()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            int cnt = 0;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_JANDENUL_PDF ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        if (_impPath != null && _impPath.Trim().Length > 0)
                        {
                            dir = new DirectoryInfo(_impPath);
                            if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                            files = dir.GetFiles();
                            if (files.Length > 0)
                            {
                                foreach (FileInfo f in files)
                                {
                                    if (f.Extension.ToLower() == ".pdf")
                                    {
                                        if (cnt >= 5) { pRestartService = true; return; }
                                        else cnt++;

                                        DataLog.AddLog("Processing Jan De Nul PDF file - " + f.FullName);

                                        JanDeNul_PDF _pdf2Xml = new JanDeNul_PDF();
                                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, _BuyerSuppInfo);
                                        try
                                        {
                                            if (_smRoutines.IsFileAvailable(f.FullName))
                                            {
                                                Boolean IsTimeOutOrDeadLock = false;
                                                if (_pdf2Xml.ConvertPDFs(f, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                                else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            DataLog.AddMaileSupplierLog("4. Unable to import Jan De Nul PDF file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                        }
                                        finally
                                        {
                                            _pdf2Xml = null; GC.Collect();
                                        }
                                    }
                                }
                            }
                        }// for each
                    }
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("5. Unable to import Jan De Nul PDF file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                files = null; dir = null; dsDirectories.Dispose();
                GC.Collect();
            }
        }

        // Added on 22-JUN-18
        private void Import_ANobelenZn_Files()
        {
            string _impPath = "";
            FileInfo[] files = null;
            DirectoryInfo dir = null;
            DataSet dsDirectories = new DataSet();
            int cnt = 0;
            try
            {
                dsDirectories = _dataAccess.GetQueryDataset(" SELECT * FROM SMV_IMPORT_ANOBEL_PDF ");
                if (eSupplierDataMain.Bll.GlobalTools.IsSafeDataSet(dsDirectories))
                {
                    foreach (DataRow strdir in dsDirectories.Tables[0].Rows)
                    {
                        _BuyerSuppInfo = new BuyerSupplierInfo();
                        _BuyerSuppInfo.GroupID = convert.ToInt(strdir["GROUP_ID"]);

                        _impPath = Convert.ToString(strdir["IMPORT_PATH"]);
                        if (_impPath != null && _impPath.Trim().Length > 0)
                        {
                            dir = new DirectoryInfo(_impPath);
                            if (!dir.Exists) Directory.CreateDirectory(dir.FullName);

                            files = dir.GetFiles();
                            if (files.Length > 0)
                            {
                                foreach (FileInfo f in files)
                                {
                                    if (f.Extension.ToLower() == ".pdf")
                                    {
                                        if (cnt >= 5) { pRestartService = true; return; }
                                        else cnt++;

                                        DataLog.AddLog("Processing A. Nobel PDF file - " + f.FullName);

                                        A_Nobel_Zn _pdf2Xml = new A_Nobel_Zn();
                                        _BuyerSuppInfo = _smRoutines.GetBuyerSupplierInfo(convert.ToInt(strdir["LINKID"]), f.Name, _BuyerSuppInfo);
                                        try
                                        {
                                            if (_smRoutines.IsFileAvailable(f.FullName))
                                            {
                                                Boolean IsTimeOutOrDeadLock = false;
                                                if (_pdf2Xml.ConvertPDFs(f, _BuyerSuppInfo, ref IsTimeOutOrDeadLock))
                                                    _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\BACKUP");
                                                else if (!IsTimeOutOrDeadLock) _smRoutines.MoveFiles(f.FullName, dir.FullName + "\\ERROR_FILES");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            DataLog.AddMaileSupplierLog("4. Unable to import A. Nobel PDF file. Error - " + ex.Message, "Error", _BuyerSuppInfo);
                                        }
                                        finally
                                        {
                                            _pdf2Xml = null; GC.Collect();
                                        }
                                    }
                                }
                            }
                        }// for each
                    }
                } // dataset
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("5. Unable to import A.Nobel PDF file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                files = null; dir = null; dsDirectories.Dispose();
                GC.Collect();
            }
        }

        // UPDATED ON 08-APR-2019
        private void Import_SilverSea_PDF_Files()
        {
            try
            {
                SilverSeaRoutine _Routine = new SilverSeaRoutine();
                _Routine.Import_SilverSea_PDF_Files();
            }
            catch (Exception ex1)
            {
                DataLog.AddMaileSupplierLog("3. Unable to import Silver Sea PDF file. Error - " + ex1.Message, "Error", _BuyerSuppInfo);
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}

