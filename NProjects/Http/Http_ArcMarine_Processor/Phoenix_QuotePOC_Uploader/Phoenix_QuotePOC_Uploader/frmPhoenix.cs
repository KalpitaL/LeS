using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace Phoenix_QuotePOC_Uploader
{
    public partial class frmPhoenix : Form
    {
        PhoenixRoutine obj = null;

        public frmPhoenix()
        {
            InitializeComponent();

            try
            {
                obj = new PhoenixRoutine();
                this.Controls.Add((Control)obj._netBrowser.browserView);

                string viewBrowser = LeSDataMain.convert.ToString(ConfigurationManager.AppSettings["VIEW_BROWSER"]).Trim().ToUpper();
                if (viewBrowser == "TRUE")
                {
                    WindowState = FormWindowState.Maximized;
                }
                else WindowState = FormWindowState.Minimized;
            }
            catch { }
        }

        private void frmPhoenix_Shown(object sender, EventArgs e)
        {
            try
            {
                if (LeSSysInfo.LeSMain.GetLeSSysInfo() == LeSSysInfo.LeSReturn.Success_1)
                {
                    obj._netBrowser.LoadUrl("about:blank");
                    System.Threading.Thread.Sleep(1000);

                    LeSDataMain.LeSDM.AddConsoleLog("Phoenix Uploader Started..");

                    obj.UploadFiles(this);

                    LeSDataMain.LeSDM.AddConsoleLog("Phoenix Uploader Stopped.");
                    LeSDataMain.LeSDM.AddConsoleLog("---------------------------------------------------." + Environment.NewLine);

                    System.Threading.Thread.Sleep(1000);

                    this.Close();
                }
                else
                {
                    LeSDataMain.LeSDM.AddConsoleLog("Unauthorize Access");
                }
            }
            catch (Exception ex)
            {
                LeSDataMain.LeSDM.AddLog("Exception in frmPhoenix_Shown() - " + ex.Message);
            }
            finally
            {
                try
                {
                    if (obj._netBrowser != null)
                    {
                        obj._netBrowser.Dispose();
                    }
                    System.Threading.Thread.Sleep(2000);
                    Application.Exit();
                }
                catch { }
                finally
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
