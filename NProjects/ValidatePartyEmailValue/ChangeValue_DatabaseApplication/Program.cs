using MTML.GENERATOR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace ValidatePartyEmailValue
{
    class Program
    {
        static DataAccess _dataaccess = new DataAccess();
        static MTMLClass _mtml = new MTMLClass();
        static MTMLInterchange _interchange { get; set; }
        static string _mailNotFoundTxt = AppDomain.CurrentDomain.BaseDirectory + "\\MailNotFoundCounter.txt";//,
        static string _mailNotFoundDayTxt = AppDomain.CurrentDomain.BaseDirectory + "\\MailNotFoundCounter_Day_AuditLog.txt";//added on 08-02-2019
        static bool isPending = false;
        static string folderPath = "",BuyerCode="",SuppCode="";
   
        static void Main(string[] args)
        {
            ReadMTMLFile();
        }

        public static void ReadMTMLFile()
        {
            string vrno = "";
            folderPath = ConfigurationManager.AppSettings["FolderPath"].Trim();//MTML file folder path
            if (Directory.Exists(folderPath))
            {
                string[] files = Directory.GetFiles(folderPath.Trim(), "*.xml");

             //search mail log in pending folder
                if (Directory.Exists(folderPath + "\\PendingFiles"))
                {
                    string[] lstfiles = Directory.GetFiles(folderPath + "\\PendingFiles");
                    if (lstfiles.Length > 0)
                    {
                        bool _check = false;
                        foreach (string file in lstfiles)
                        {
                            _interchange = _mtml.Load(file);
                            if (_interchange != null)
                            {
                                for (int i = 0; i < _interchange.DocumentHeader.References.Count; i++)
                                {
                                    if (_interchange.DocumentHeader.References[i].Qualifier == ReferenceQualifier.UC)
                                        vrno = _interchange.DocumentHeader.References[i].ReferenceNumber.Trim(); ;
                                    if (vrno != "") break;
                                }

                                if (vrno != "")
                                {
                                    string[] _text = File.ReadAllLines(_mailNotFoundTxt);
                                    List<string> strList = _text.ToList();
                                    var matchingvalues = strList.Where(str => str.Contains(vrno));
                                    string _value = String.Join("", matchingvalues.ToArray());
                                    if (_value != "")//added on 08-02-2019
                                    {
                                        DateTime dt = Convert.ToDateTime(_value.Split('|')[2].Trim()).AddDays(Convert.ToInt32(ConfigurationManager.AppSettings["Search_days"].Trim()));

                                        if (Convert.ToDateTime(DateTime.Now.ToString(ConfigurationManager.AppSettings["DateFormat"].Trim())) <= Convert.ToDateTime(dt.ToString(ConfigurationManager.AppSettings["DateFormat"].Trim())))
                                        {
                                            if (_value.Split('|').Length == 4)//added on 08-02-2019
                                            {
                                                if (Convert.ToDateTime(DateTime.Now.ToString(ConfigurationManager.AppSettings["DateFormat"].Trim())) != Convert.ToDateTime(_value.Split('|')[3].Trim()))
                                                {
                                                    #region added on 08-02-2018
                                                    if (_check == false)
                                                    {
                                                        string[] _text1 = File.ReadAllLines(_mailNotFoundDayTxt);
                                                        if (_text1.Length != 0)
                                                        {
                                                            string vrnos = String.Join(",", _text1);
                                                            //    CreateAuditFile("", ConfigurationManager.AppSettings["Module"].Trim(), "", "Error", "no data found for vrno " + vrnos + " in " + Convert.ToInt32(ConfigurationManager.AppSettings["Search_counter"]) + " schedules.");//commented on 08-02-2019
                                                            //File.WriteAllText(_mailNotFoundDayTxt, "");
                                                           
                                                            int index = strList.IndexOf(_value);
                                                            string[] arrlist = strList[index].Split('|');
                                                            if (arrlist.Length == 4)
                                                            {
                                                                arrlist[3] = DateTime.Now.ToString(ConfigurationManager.AppSettings["DateFormat"].Trim()); strList[index] = "";
                                                                strList[index] = vrno + "|" + (Convert.ToInt32(arrlist[1]) + 1) + "|" + arrlist[2] + "|" + arrlist[3];
                                                            }
                                                            File.WriteAllLines(_mailNotFoundTxt, strList.ToArray());
                                                        }
                                                        _check = true;
                                                    }
                                                    #endregion

                                                    isPending = true;
                                                    SearchValue_Database(vrno, file);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //WriteLog("no data found for vrno " + vrno + " in last " + ConfigurationManager.AppSettings["Search_days"].Trim() + " days.");//commented on 08-02-2019
                                            //CreateAuditFile(Path.GetFileName(file), ConfigurationManager.AppSettings["Module"].Trim(), vrno, "Error", "no data found for vrno " + vrno + " in last " + ConfigurationManager.AppSettings["Search_days"].Trim() + " days.");
                                            #region added on 08-02-2018
                                            if (_check == false)
                                            {
                                                string[] _text1 = File.ReadAllLines(_mailNotFoundDayTxt);
                                                if (_text1.Length != 0)
                                                {
                                                    string vrnos = String.Join(",", _text1);
                                                    CreateAuditFile("", ConfigurationManager.AppSettings["Module"].Trim(), "", "Error", "no data found for vrno " + vrnos + " in " + Convert.ToInt32(ConfigurationManager.AppSettings["Search_counter"]) + " schedules.");//commented on 08-02-2019
                                                    File.WriteAllText(_mailNotFoundDayTxt, "");
                                                }
                                                _check = true;
                                            }
                                            #endregion
                                            MoveToError(Path.GetDirectoryName(file), folderPath, Path.GetFileName(file));
                                        }
                                    }
                                    else//added on 08-02-2019
                                    {
                                        SearchValue_Database(vrno, file);
                                    }
                                }
                                else throw new Exception("VRNo not found in MTML file");
                            }

                        }
                    }
                }

                if (files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        _interchange = _mtml.Load(file);
                        if (_interchange != null)
                        {
                            for (int i = 0; i < _interchange.DocumentHeader.References.Count; i++)
                            {
                                if (_interchange.DocumentHeader.References[i].Qualifier == ReferenceQualifier.UC)
                                    vrno = _interchange.DocumentHeader.References[i].ReferenceNumber.Trim(); ;
                                if (vrno != "") break;
                            }

                            if (_interchange.Sender != null)
                                BuyerCode = _interchange.Sender;

                            if (_interchange.Recipient != null)
                                SuppCode = _interchange.Recipient;

                            if (vrno != "")
                                SearchValue_Database(vrno, file);
                            else throw new Exception("VRNo not found in MTML file");
                        }
                    }
                }
                else
                {
                    WriteLog("No files found.");
                }

            }
            else
            {
                throw new Exception("No directory found at 'FolderPath' location");
            }
        }

        public static void SearchValue_Database(string vrNo, string file)
        {
            string _toMail = "", _fromMail = ""; bool IsSuccess = false;
            try
            {
                string _sql="SELECT " + ConfigurationManager.AppSettings["GetValue"].Trim().ToUpper() + ",UPDATEDATE FROM SM_MAIL_DOWNLOAD_LOG " +
                    "WHERE LOGTYPE='SUCCESS' AND " + ConfigurationManager.AppSettings["SearchIn"].Trim().ToUpper() + " LIKE '%" + vrNo + "%' AND UPDATEDATE >=dateadd(day,-1, getdate()) ORDER BY UPDATEDATE DESC";
                _dataaccess.CreateSQLCommand(_sql);
                WriteLog(_sql);
                DataSet ds = _dataaccess.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    WriteLog(ds.Tables[0].Rows.Count + " record(s) found in database for vrno " + vrNo);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ConfigurationManager.AppSettings["GetValue"].Trim().ToUpper().Contains("TOMAIL")) _toMail = Convert.ToString(ds.Tables[0].Rows[i]["ToMail"]);
                        if (ConfigurationManager.AppSettings["GetValue"].Trim().ToUpper().Contains("FROMMAIL")) _fromMail = Convert.ToString(ds.Tables[0].Rows[i]["FromMail"]);

                        for (int j = 0; j < _interchange.DocumentHeader.PartyAddresses.Count; j++)
                        {
                            if (_fromMail != "")
                            {
                                if (_interchange.DocumentHeader.PartyAddresses[j].Qualifier == PartyQualifier.VN)
                                {
                                    if (_interchange.DocumentHeader.PartyAddresses[j].Contacts != null)
                                    {
                                        if (_interchange.DocumentHeader.PartyAddresses[j].Contacts.Count > 0)
                                        {
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count > 0)
                                            {
                                                for (int k = 0; k < _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count; k++)
                                                {
                                                    if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.EM)
                                                    {
                                                        _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number = _fromMail; IsSuccess = true;
                                                        if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number == _fromMail) break;

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                CommunicationMethods _c = new CommunicationMethods();
                                                _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Add(_c);
                                                _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[0].Qualifier = CommunicationMethodQualifiers.EM;
                                                _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[0].Number = _fromMail;
                                                IsSuccess = true;
                                            }
                                        }
                                    }
                                }
                            }
                            if (_toMail != "")
                            {
                                if (_interchange.DocumentHeader.PartyAddresses[j].Qualifier == PartyQualifier.BY)
                                {
                                    if (_interchange.DocumentHeader.PartyAddresses[j].Contacts != null)
                                    {
                                        if (_interchange.DocumentHeader.PartyAddresses[j].Contacts.Count > 0)
                                        {
                                            if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count > 0)
                                            {
                                                for (int k = 0; k < _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Count; k++)
                                                {
                                                    if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Qualifier == CommunicationMethodQualifiers.EM)
                                                    {
                                                        _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number = _toMail; IsSuccess = true;
                                                        if (_interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[k].Number == _toMail) break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                CommunicationMethods _c = new CommunicationMethods();
                                                _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].FunctionCode = ContactFunction.PD;
                                                _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList.Add(_c);
                                                _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[0].Qualifier = CommunicationMethodQualifiers.EM;
                                                _interchange.DocumentHeader.PartyAddresses[j].Contacts[0].CommunMethodList[0].Number = _toMail;
                                                IsSuccess = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    try
                    {
                        if (IsSuccess)
                        {
                            _mtml.Create(_interchange, file);
                            WriteLog("email id changed in file " + Path.GetFileName(file));
                            if (File.Exists(file))
                            {
                                if (!Directory.Exists(folderPath + "\\Backup")) Directory.CreateDirectory(folderPath + "\\Backup");
                                File.Copy(file, folderPath + "\\Backup\\" + Path.GetFileName(file), true);
                                if (!Directory.Exists(ConfigurationManager.AppSettings["Move_FolderPath"].Trim())) Directory.CreateDirectory(ConfigurationManager.AppSettings["Move_FolderPath"].Trim());
                                if (File.Exists(ConfigurationManager.AppSettings["Move_FolderPath"].Trim() + "\\" + Path.GetFileName(file))) File.Delete(ConfigurationManager.AppSettings["Move_FolderPath"].Trim() + "\\" + Path.GetFileName(file));
                                File.Move(file, ConfigurationManager.AppSettings["Move_FolderPath"].Trim() + "\\" + Path.GetFileName(file));

                                //if record found in MailNotFoundCounter text file after success,then remove that record from file.
                                string[] _text = File.ReadAllLines(_mailNotFoundTxt);
                                List<string> strList = _text.ToList();
                                var matchingvalues = strList.Where(str => str.Contains(vrNo));
                                string _value = String.Join("", matchingvalues.ToArray());
                                int index = strList.IndexOf(_value);
                                if (index >= 0)
                                {
                                    strList.RemoveAt(index);
                                    File.WriteAllLines(_mailNotFoundTxt, strList.ToArray());
                                }
                            }
                        }
                        else if (ConfigurationManager.AppSettings["GetValue"].Trim().ToUpper().Contains("TOMAIL") && _toMail == "")
                        {
                            WriteLog("To email is blank for ref no " + vrNo);
                            CreateAuditFile(Path.GetFileName(file), ConfigurationManager.AppSettings["Module"].Trim(), vrNo, "Error", "To email is blank for ref no " + vrNo);
                            MoveToError(folderPath, folderPath,Path.GetFileName(file));
                        }
                        else if (ConfigurationManager.AppSettings["GetValue"].Trim().ToUpper().Contains("FROMMAIL") && _fromMail == "")
                        {
                            WriteLog("From email is blank for ref no " + vrNo);
                            CreateAuditFile(Path.GetFileName(file), ConfigurationManager.AppSettings["Module"].Trim(), vrNo, "Error", "From email is blank for ref no " + vrNo);
                            MoveToError(folderPath, folderPath,Path.GetFileName(file));
                        }
                        else
                        {
                            WriteLog("Unable to change email id for ref no " + vrNo);
                            CreateAuditFile(Path.GetFileName(file), ConfigurationManager.AppSettings["Module"].Trim(), vrNo, "Error", "Unable to change email id for ref no " + vrNo);
                            MoveToError(folderPath, folderPath,Path.GetFileName(file));
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog("Exception while updating values for ref no " + vrNo + ": " + ex.Message.ToString());
                    }
                }
                else
                {
                   
                    if (!File.Exists(_mailNotFoundTxt)) File.Create(_mailNotFoundTxt);

                    string[] _text = File.ReadAllLines(_mailNotFoundTxt);
                    List<string> strList = _text.ToList();
                    var matchingvalues = strList.Where(str => str.Contains(vrNo));
                    if (!strList.Any(str => str.Contains(vrNo)))
                        File.AppendAllText(_mailNotFoundTxt, vrNo + "|1|" + DateTime.Now.ToString(ConfigurationManager.AppSettings["DateFormat"].Trim()) + Environment.NewLine);//test
                    else
                    {
                        string _value = String.Join("", matchingvalues.ToArray());
                        int index = strList.IndexOf(_value);
                        string[] arrlist = strList[index].Split('|');
                        if (Convert.ToInt32(arrlist[1]) < Convert.ToInt32(ConfigurationManager.AppSettings["Search_counter"]))
                        {
                            if (arrlist.Length == 4)
                                strList[index] = vrNo + "|" + (Convert.ToInt32(arrlist[1]) + 1) + "|" + arrlist[2] + "|" + arrlist[3];
                            else
                                strList[index] = vrNo + "|" + (Convert.ToInt32(arrlist[1]) + 1) + "|" + arrlist[2];
                            File.WriteAllLines(_mailNotFoundTxt, strList.ToArray());
                        }
                        else
                        {
                            WriteLog("no data found for vrno " + vrNo + " in " + Convert.ToInt32(ConfigurationManager.AppSettings["Search_counter"]) + " schedules.");
                            #region create file where all vrno stored which record not found in database, and create audit only once for all files added on 08-02-2019
                            if (!File.Exists(_mailNotFoundDayTxt)) File.Create(_mailNotFoundDayTxt);
                            File.AppendAllText(_mailNotFoundDayTxt, vrNo+Environment.NewLine);
                            #endregion
                            //  CreateAuditFile(Path.GetFileName(file), ConfigurationManager.AppSettings["Module"].Trim(), vrNo, "Error", "no data found for vrno " + vrNo + " in " + Convert.ToInt32(ConfigurationManager.AppSettings["Search_counter"]) + " schedules.");//commented on 08-02-2019

                            if (!Directory.Exists(folderPath + "\\PendingFiles")) Directory.CreateDirectory(folderPath + "\\PendingFiles");
                            if (!isPending)
                            {
                                if (File.Exists(folderPath + "\\PendingFiles\\" + Path.GetFileName(file))) File.Delete(folderPath + "\\PendingFiles\\" + Path.GetFileName(file));
                                File.Move(folderPath + "\\" + Path.GetFileName(file), folderPath + "\\PendingFiles\\" + Path.GetFileName(file));
                            }

                            strList[index] = vrNo + "|0|" + arrlist[2] + "|" + DateTime.Now.ToString(ConfigurationManager.AppSettings["DateFormat"].Trim());
                            File.WriteAllLines(_mailNotFoundTxt, strList.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog("Exception in SearchValue_Database(): " + ex.Message.ToString());
            }
        }

        public static void WriteLog(string _logText)
        {
            string LogPath = "";
            if (ConfigurationManager.AppSettings["LogPath"] == null || ConfigurationManager.AppSettings["LogPath"].Trim() == "")
                LogPath = AppDomain.CurrentDomain.BaseDirectory + "Log";
            else
                LogPath = ConfigurationManager.AppSettings["LogPath"].Trim();
            if (!Directory.Exists(LogPath)) Directory.CreateDirectory(LogPath);
            string _logfile = @"Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            Console.WriteLine(_logText);
            File.AppendAllText(LogPath + @"\" + @_logfile, DateTime.Now.ToString() + " " + _logText + Environment.NewLine);
        }

        public static void CreateAuditFile(string FileName, string Module, string RefNo, string LogType, string Audit)
        {
            try
            {
                string auditPath = "";
                if (ConfigurationManager.AppSettings["AuditPath"].Trim() != "")
                    auditPath = ConfigurationManager.AppSettings["AuditPath"].Trim();
                else auditPath = AppDomain.CurrentDomain.BaseDirectory + "AuditLog";

                if (!Directory.Exists(auditPath)) Directory.CreateDirectory(auditPath);

                if (LogType.Trim().Contains("Error")) { LogType = "Notify"; }//changed by Kalpita on 09/10/2019 to stop displaying in error log

                string auditData = "";
                if (auditData.Trim().Length > 0) auditData += Environment.NewLine;
                auditData += BuyerCode + "|";
                auditData += SuppCode + "|";
                auditData += Module + "|";
                auditData += Path.GetFileName(FileName) + "|";
                auditData += RefNo + "|";
                auditData += LogType + "|";
                auditData += DateTime.Now.ToString("yy-MM-dd HH:mm") + " : " + Audit;
                if (auditData.Trim().Length > 0)
                {
                    File.WriteAllText(auditPath + "\\Audit_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt", auditData);
                    System.Threading.Thread.Sleep(5);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void MoveToError(string sourcefolder,string destFolder,string file)
        {
            if (!Directory.Exists(destFolder + "\\Error")) Directory.CreateDirectory(destFolder + "\\Error");
            if (File.Exists(destFolder + "\\Error\\" + file)) File.Delete(destFolder + "\\Error\\" + file);
            File.Move(sourcefolder + "\\" + file, destFolder + "\\Error\\" + file);
        }

    }
}
