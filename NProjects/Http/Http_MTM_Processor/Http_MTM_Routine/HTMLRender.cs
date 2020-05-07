using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace Http_MTM_Routine
{
    public class HtmlRenderer
    {
        public static void Render(WebBrowser _wb, string inputUrl, string outputPath, Rectangle crop)
        {
            WebBrowser wb = new WebBrowser();
            wb.ScrollBarsEnabled = false;
            wb.ScriptErrorsSuppressed = true;
            wb.Navigate(inputUrl);

            while (wb.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            System.Threading.Thread.Sleep(5000);

            wb.Width = wb.Document.Body.ScrollRectangle.Width;
            wb.Height = wb.Document.Body.ScrollRectangle.Height;
            if (wb.Height == 0) wb.Height = 750;
            using (Bitmap bitmap = new Bitmap(wb.Width, wb.Height))
            {
                wb.DrawToBitmap(bitmap, new Rectangle(0, 0, wb.Width, wb.Height));
                wb.Dispose();
                Rectangle rect = new Rectangle(crop.Left, crop.Top, wb.Width - crop.Width - crop.Left, wb.Height - crop.Height - crop.Top);
                Bitmap cropped = bitmap.Clone(rect, bitmap.PixelFormat);                
                cropped.Save(outputPath, ImageFormat.Png);
            }
        }

        public static void Render(string htmlFile, string outputPath, Rectangle crop)
        {
            WebBrowser wb = new WebBrowser();
            wb.ScrollBarsEnabled = false;
            wb.ScriptErrorsSuppressed = true;
            wb.Navigate(htmlFile);

            while (wb.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            System.Threading.Thread.Sleep(3000);

            //wb.Width = wb.Document.Body.ScrollRectangle.Width;
            //int w = wb.Document.GetElementById("dv_Final").ScrollRectangle.Width;
            //wb.Width = w;
            //int h = wb.Document.GetElementById("dv_Final").ScrollRectangle.Height;
            //if (wb.Document.Body.ScrollRectangle.Height > 0)
            //{
            //    wb.Height = wb.Document.Body.ScrollRectangle.Height;
            //}
            //else wb.Height = h + 100;

            wb.Width = 1224;
            wb.Height = 768;

            using (Bitmap bitmap = new Bitmap(wb.Width, wb.Height))
            {
                wb.DrawToBitmap(bitmap, new Rectangle(0, 0, wb.Width, wb.Height));
                wb.Dispose();
                Rectangle rect = new Rectangle(crop.Left, crop.Top, wb.Width - crop.Width - crop.Left, wb.Height - crop.Height - crop.Top);
                Bitmap cropped = bitmap.Clone(rect, bitmap.PixelFormat);
                cropped.Save(outputPath, ImageFormat.Png);
            }
        }
    }
}