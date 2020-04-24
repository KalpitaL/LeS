using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using WatiN.Core;
using System.Drawing.Imaging;

namespace WebScraper
{
    public class IEBrowser : IE
    {
        private string _url;
        public Document _document;

        public IEBrowser()
            : base()
        {
            AutoClose = true;
            ClearCache();
            ClearCookies();
            Settings.AutoMoveMousePointerToTopLeft = false;
        }

        public IEBrowser(bool IsNewInstance)
            : base(IsNewInstance)
        {
            AutoClose = true;
            ClearCache();
            ClearCookies();
            Settings.AutoMoveMousePointerToTopLeft = false;
        }

        public bool gotopage(string url)
        {
            bool _result = false;
            try
            {
                GoTo(url);
                WaitForComplete(500);

                _url = url;
                _result = true;
                return _result;
            }
            catch (Exception)
            {
                return _result;
            }
        }

        public string getsource()
        {
            if (Body.Exists) return Body.Parent.OuterHtml;
            else return "";
        }

        public bool IsHidden(TextField textField)
        {
            if (textField == null || !textField.Exists) return false;
            var textFieldType = textField.GetAttributeValue("type");
            return ((textFieldType != null) && textFieldType.ToLowerInvariant() == "hidden");
        }

        public void SetText(TextField textField, string value)
        {
            if ((textField == null) || !textField.Exists) return;
            if (!IsHidden(textField)) textField.Focus();
            textField.Value = value;
        }

        public Element GetElement(string _textField)
        {
            Element textField = Element(Find.ById(_textField));
            if ((textField == null) || !textField.Exists) return null;
            return textField;
        }

        public Table GetTable(string _tableId)
        {
            Table _table = Table(Find.ById(_tableId));
            if ((_table == null) || !_table.Exists) return null;
            return _table;
        }

        public string GetText(string _textField)
        {
            try
            {
                if (!String.IsNullOrEmpty(_textField))
                {
                    Element textField = Element(_textField);
                    if ((textField == null) || !textField.Exists) return string.Empty;
                    string _result = textField.GetAttributeValue("value");
                    if (string.IsNullOrEmpty(_result)) { _result = textField.Text; }
                    if (string.IsNullOrEmpty(_result) && textField.InnerHtml != null) { _result = convert.ToString(textField.InnerHtml).Replace("<BR>", Environment.NewLine); }
                    return _result;
                }
                else { return string.Empty; }
            }
            catch
            {
                return string.Empty;
            }
        }

        public bool IsElementVisible(string _elementId)
        {
            Element _element = Element(Find.ById(_elementId));
            if ((_element == null) || !_element.Exists) { return false; }
            while ((_element != null) && _element.Exists)
            {
                if (_element.Style.Display.ToLowerInvariant().Contains("none")) { return false; }
                _element = _element.Parent;
            }
            return true;
        }

        public bool IsValidElementById(string _elementId)
        {
            Element _element = Element(Find.ById(_elementId));
            if ((_element == null) || !_element.Exists) { return false; }
            else { return true; }
        }

        public void InvokeAction(string _elementId, string _action)
        {
            Element _element = Element(Find.ById(_elementId));
            if ((_element == null) || !_element.Exists) { return; }
            string _eleAction = convert.ToString(_action).Trim().ToLower();
            switch (_action)
            {
                case "click": { _element.Click(); break; }
                case "focus": { _element.Focus(); break; }
                case "blur": { _element.Blur(); break; }
                case "change": { _element.Change(); break; }
                case "doubleclick": { _element.DoubleClick(); break; }
                case "mousedown": { _element.MouseDown(); break; }
                case "mouseup": { _element.MouseUp(); break; }
            }
        }

        public void SaveAsPNG(string FilePath, string FileName, string HTML, string PrintURL)
        {
            using (WebBrowser wb = new WebBrowser())
            {
                wb.Width = 1024;
                wb.ScrollBarsEnabled = false;
                wb.ScriptErrorsSuppressed = true;
                if (PrintURL.Trim() == "")
                {
                    wb.Navigate(HTML);
                    while (wb.ReadyState != WebBrowserReadyState.Complete) Application.DoEvents();
                }
                else
                {
                    wb.Navigate(PrintURL);
                    while (wb.ReadyState != WebBrowserReadyState.Complete) Application.DoEvents();
                }
                wb.Width = wb.Document.Body.ScrollRectangle.Size.Width;
                wb.Height = wb.Document.Body.ScrollRectangle.Size.Height;
                using (Bitmap bitmap = new Bitmap(wb.Width, wb.Height))
                {
                    wb.DrawToBitmap(bitmap, new Rectangle(0, 0, wb.Width, wb.Height));
                    wb.Dispose();
                    Rectangle rect = new Rectangle(0, 0, wb.Width, wb.Height);
                    Bitmap cropped = bitmap.Clone(rect, bitmap.PixelFormat);
                    cropped.Save(@FilePath + @"\" + FileName, ImageFormat.Png);
                }
            }
        }
    }
}