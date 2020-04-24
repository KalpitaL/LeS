using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoenixRFQDownloader
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Console.Title = "Phoenix RFQ Downloader - " + DateTime.Now.ToString("dd-MMM-yy HH:mm:ss");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                //PhoenixRoutine.Phoenix _phoenix = new PhoenixRoutine.Phoenix();
                //_phoenix.ProcessLinkFiles();

                PhoenixRoutine.Phoenix _phoenix = new PhoenixRoutine.Phoenix();
                /**************/
                Console.WriteLine("****** Phoenix RFQ Downloader Started (" + DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + ")  ******");
                Console.WriteLine("");
                _phoenix.ProcessLinkFiles();
                /*************/
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("");
                Console.WriteLine("****** Phoenix RFQ Downloader Stopped (" + DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + ")  ******");
                Application.Exit();
            }
        }
    }
}
