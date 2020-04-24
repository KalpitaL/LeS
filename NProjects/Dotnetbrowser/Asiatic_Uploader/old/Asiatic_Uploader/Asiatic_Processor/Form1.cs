﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AsiaticRoutine;
using System.Configuration;
using System.IO;
using System.Xml;

namespace Asiatic_Processor
{
    public partial class Form1 : Form
    {
        AsiaticRoutine.AsiaticRoutine asRoutine = new AsiaticRoutine.AsiaticRoutine();

        public Form1()
        {
            InitializeComponent();
            Controls.Add(asRoutine._netWrapper.browserView);
        }

        public void Showform()
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["SHOW_FORM"].Trim())==false)
                this.Visible = false;
            else
                this.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            asRoutine._netWrapper.LogText = "================" + DateTime.Now.ToString() + "================";
            asRoutine._netWrapper.LogText = "Asiatic Quote,POC HTTP processor started";
            Showform();
            foreach (string sAction in ConfigurationManager.AppSettings["ACTIONS"].Split(','))
            {
                try
                {
                    asRoutine.LoadAppSettings();
                    switch (sAction.ToUpper())
                    {
                        case "QUOTE": asRoutine.ProcessQuote(); break;
                    }
                }
                catch (Exception ex)
                {
                    asRoutine._netWrapper.LogText = "Exception while processing " + sAction.ToUpper() + " : " + ex.GetBaseException().ToString();
                }
            }
             asRoutine._netWrapper.LogText = "Asiatic Quote,POC HTTP processor stooped.";
            asRoutine._netWrapper.LogText = "====================================================";
            this.Controls.Clear();
            this.Hide();
            this.Dispose();
            Environment.Exit(0);
        }
    }
}
