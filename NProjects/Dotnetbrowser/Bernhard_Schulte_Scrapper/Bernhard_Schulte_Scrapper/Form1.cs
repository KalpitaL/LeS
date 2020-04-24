﻿using Bernhard_Schulte_Routine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bernhard_Schulte_Scrapper
{
    public partial class Form1 : Form
    {
       // BSRoutine bsRoutine = new BSRoutine();
        BSRoutine bsRoutine;
        string LOG_PATH = "", RFQ_PATH = "", MAILFILE_PATH = "", Mail_Template = "", FROM_EMAIL_ID = "", MAIL_BCC = "", ATTACHMENT_INBOX = "", SHOW_FORM = "";
        string[] ACTIONS;

        public Form1()
        {
            InitializeComponent();
            try
            {
                bsRoutine = new BSRoutine();
                Controls.Clear();//added by Kalpita on 15/11/2019
                Controls.Add(bsRoutine._netWrapper.browserView);
            }
            catch(Exception ex)
            {
                ClearSettings();
            }
        }

        public bool CheckInstance()
        {
            bool _result = false;
            int nCnt = 0;
            try
            {
                Process current = Process.GetCurrentProcess();
                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.ProcessName == current.ProcessName) nCnt++;
                }
                if (nCnt > 1)
                {
                    WriteLog("Application is already running");
                    _result = false;
                }
                else
                {
                    _result = true;
                }
            }
            catch
            { }
            return _result;
        }

        public void AppSetup()
        {
            if (ConfigurationManager.AppSettings["LOGPATH"] != null && ConfigurationManager.AppSettings["LOGPATH"].Length > 0) LOG_PATH = ConfigurationManager.AppSettings["LOGPATH"].Trim();
            if (ConfigurationManager.AppSettings["RFQ_PATH"] != null && ConfigurationManager.AppSettings["RFQ_PATH"].Length > 0) RFQ_PATH = ConfigurationManager.AppSettings["RFQ_PATH"].Trim();
            if (ConfigurationManager.AppSettings["ACTIONS"] != null && ConfigurationManager.AppSettings["ACTIONS"].Length > 0) ACTIONS = ConfigurationManager.AppSettings["ACTIONS"].Split(',');
            if (ConfigurationManager.AppSettings["MAILFILE_PATH"] != null && ConfigurationManager.AppSettings["MAILFILE_PATH"].Length > 0) MAILFILE_PATH = ConfigurationManager.AppSettings["MAILFILE_PATH"].Trim();
            if (ConfigurationManager.AppSettings["ATTACHMENT_INBOX"] != null && ConfigurationManager.AppSettings["ATTACHMENT_INBOX"].Length > 0) ATTACHMENT_INBOX = ConfigurationManager.AppSettings["ATTACHMENT_INBOX"].Trim();
            if (ConfigurationManager.AppSettings["SHOW_FORM"] != null && ConfigurationManager.AppSettings["SHOW_FORM"].Length > 0) SHOW_FORM = ConfigurationManager.AppSettings["SHOW_FORM"].Trim();
            if (LOG_PATH == "")
                LOG_PATH = Application.StartupPath + "\\Log";

            if (RFQ_PATH == "")
                RFQ_PATH = Application.StartupPath + "\\RFQ";

            if (MAILFILE_PATH == "")
                MAILFILE_PATH = Application.StartupPath + "\\Mail_Files";

            if (ATTACHMENT_INBOX == "")
                ATTACHMENT_INBOX = Application.StartupPath + "\\Attachment_Inbox";

            if (SHOW_FORM == "")
                SHOW_FORM = "FALSE";

            Mail_Template = Application.StartupPath + "\\Mail_Temp\\ESUPPLIER_MESSAGE.txt";
            FROM_EMAIL_ID = ConfigurationManager.AppSettings["FROM_EMAIL_ID"].Trim();
            MAIL_BCC = ConfigurationManager.AppSettings["MAIL_BCC"].Trim();

            this.Visible = (SHOW_FORM.ToUpper() == "FALSE") ? false : true;
        }

        public bool LogIn()
        {
            bool IsLoggedIn = false;
            try
            {
                IsLoggedIn = bsRoutine.LogIn();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException().ToString());
            }
            return IsLoggedIn;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (CheckInstance())//added by Kalpita on 06/08/2019
            {
                AppSetup();
                //Showform();
                //  if (LogIn())//09-11-2017
                //{
                foreach (string sAction in ACTIONS)
                {
                    try
                    {
                        switch (sAction.ToUpper())
                        {
                            case "RFQ": ProcessRFQ(); break;
                            case "QUOTE": ProcessQuote(); break;
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog("Exception while processing " + sAction.ToUpper() + " : " + ex.GetBaseException().ToString());
                    }
                }
            }
            ClearSettings();//added by Kalpita on 15/11/2019
            // }
             //this.Controls.Clear();
             //this.Hide();
             //this.Dispose();
             //Environment.Exit(0);
        }

        private void ProcessRFQ()
        {
            if (LogIn())//09-11-2017
            {
                List<string> slProcessRFQ = bsRoutine.GetProcessedItems(PluginInterface.eActions.RFQ);
                bool result = bsRoutine.ProcessRFQ(slProcessRFQ, RFQ_PATH);
            }
        }

        public void ProcessQuote()
        {
            bsRoutine.ProcessQuote(MAILFILE_PATH, Mail_Template, FROM_EMAIL_ID, MAIL_BCC);
        }

        private void WriteLog(string _logText, string _logFile = "")
        {
            string _logfile = @"Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (_logFile.Length > 0) { _logfile = _logFile; }

            Console.WriteLine(_logText);
            File.AppendAllText(LOG_PATH + @"\" + @_logfile, DateTime.Now.ToString() + " " + _logText + Environment.NewLine);
        }

        public void Showform()
        {
            if (SHOW_FORM.ToUpper() == "FALSE")
                this.Visible = false;
            else
                this.Visible = true;
        }

        private void ClearSettings()
        {
            if (bsRoutine != null && bsRoutine._netWrapper != null)
            {
                bsRoutine._netWrapper.browserView.Dispose();
                bsRoutine._netWrapper.Dispose();
            }
            this.Controls.Clear();
            this.Hide();
            this.Dispose();
            Environment.Exit(0);
        }
    }
}