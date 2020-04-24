using Aspose.Pdf;
using Aspose.Pdf.Text;
using Aspose.Pdf.Text.TextOptions;
//using Aspose.Words;
using eSupplierDataMain;
using LeSCommonRoutine;
using MTML.GENERATOR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace PacificFujiDemo
{
    internal class CommonRoutine
    {
        private int PageCount = 0;

        public CommonRoutine()
        {
            (new Aspose.Pdf.License()).SetLicense("Aspose.Total.lic");
            (new Aspose.Words.License()).SetLicense("Aspose.Total.lic");
        }

        public bool CheckInvalidDoc(FileInfo pdfFile)
        {
            bool flag;
            int i;
            char[] chrArray;
            try
            {
                bool invalid = false;
                RichTextBox t = new RichTextBox()
                {
                    Text = this.GetText(pdfFile.FullName)
                };
                if (t.Text.Trim().Length > 0)
                {
                    string[] InvalidPDFContent = File.ReadAllLines(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\INVALID_PDF_CONTENT.txt"));
                    string[] strArrays = InvalidPDFContent;
                    for (i = 0; i < (int)strArrays.Length; i++)
                    {
                        string line = strArrays[i];
                        chrArray = new char[] { '=' };
                        string[] keyValues = line.Split(chrArray);
                        if ((int)keyValues.Length > 1)
                        {
                            string actualValue = this.GetValue(t, keyValues[0]);
                            string str = keyValues[1].Trim();
                            chrArray = new char[] { '|' };
                            string[] values = str.Split(chrArray);
                            if (actualValue.Trim().ToUpper().Contains(values[0].Trim().ToUpper()))
                            {
                                if (((int)values.Length <= 1 ? false : t.Text.Trim().ToUpper().Contains(values[1].Trim().ToUpper())))
                                {
                                    invalid = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!invalid)
                    {
                        string InvalidPDFName = File.ReadAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\INVALID_PDF_FILENAME.txt"));
                        string str1 = InvalidPDFName.Trim();
                        chrArray = new char[] { '|' };
                        strArrays = str1.Split(chrArray);
                        i = 0;
                        while (i < (int)strArrays.Length)
                        {
                            string Name = strArrays[i];
                            if (!pdfFile.Name.Contains(Name))
                            {
                                i++;
                            }
                            else
                            {
                                invalid = true;
                                break;
                            }
                        }
                    }
                }
                flag = invalid;
            }
            catch (Exception exception)
            {
                flag = false;
            }
            return flag;
        }

        public string Get_EML_MSG_FileName(string cName, BuyerSupplierInfo _ByrSuppInfo, ref string pBuyerEmail)
        {
            string cReturn = "";
            string cEMLFile = "";
            string cEMLPath = "";
            string cCCEmail = "";
            string cMSGName = "";
            string cEnvelopeFrom = "";
            string cReplyTo = "";
            try
            {
                pBuyerEmail = "";
                string[] slFname = cName.Split(new char[] { '\u005F' });
                if ((int)slFname.Length > 1)
                {
                    int counter = 1;
                    bool mailFound = false;
                    while (counter < (int)slFname.Length)
                    {
                        cEMLFile = string.Concat(slFname[counter - 1], "_", slFname[counter], ".eml");
                        if (ConfigurationManager.AppSettings["ESUPPLIER_EMAIL_FILES"] != null)
                        {
                            cEMLPath = ConfigurationManager.AppSettings["ESUPPLIER_EMAIL_FILES"];
                        }
                        if (!File.Exists(string.Concat(cEMLPath, "\\", cEMLFile)))
                        {
                            cEMLFile = string.Concat(slFname[counter - 1], "_", slFname[counter], ".msg");
                            if (File.Exists(string.Concat(cEMLPath, "\\", cEMLFile)))
                            {
                                mailFound = true;
                                cReturn = cEMLFile;
                                LeSCommonRoutine.LeSRoutine LeSRoutine = new LeSCommonRoutine.LeSRoutine();
                                string _value = (new SMData_Routines()).GetRuleValue(_ByrSuppInfo, "READ_MSG_CC_EMAIL_AS_PARTY_EMAIL");
                                if (!(_value.Contains("6|") || _value.Contains("7") || _value.Contains("10|") || _value == "6" ? false : !(_value == "10")))
                                {
                                    pBuyerEmail = LeSRoutine.GetMSGSenderEMail(string.Concat(cEMLPath, "\\", cEMLFile), out cMSGName, out cCCEmail, out cEnvelopeFrom, out cReplyTo, _value);
                                }
                                else if (!_value.Contains("9|"))
                                {
                                    pBuyerEmail = LeSRoutine.GetMSGSenderEMail(string.Concat(cEMLPath, "\\", cEMLFile), out cMSGName, out cCCEmail, out cEnvelopeFrom);
                                }
                                else
                                {
                                    pBuyerEmail = LeSRoutine.GetMSGSenderEMail(string.Concat(cEMLPath, "\\", cEMLFile), out cMSGName, out cCCEmail, out cEnvelopeFrom, out cReplyTo, _value);
                                }
                                if (convert.ToInt(_value) == 1)
                                {
                                    pBuyerEmail = cCCEmail;
                                }
                                else if (!(convert.ToInt(_value) != 2 ? true : string.IsNullOrEmpty(cCCEmail)))
                                {
                                    pBuyerEmail = string.Concat(pBuyerEmail, ";", cCCEmail);
                                }
                                else if (!(!_value.Contains("6|") ? true : string.IsNullOrEmpty(cReplyTo)))
                                {
                                    pBuyerEmail = cReplyTo;
                                }
                                else if (!(!_value.Contains("7") ? true : string.IsNullOrEmpty(cReplyTo)))
                                {
                                    pBuyerEmail = cReplyTo;
                                }
                                else if (!(_value != "6" ? true : string.IsNullOrEmpty(cReplyTo)))
                                {
                                    pBuyerEmail = cReplyTo;
                                }
                                else if (!(!_value.Contains("10|") ? true : string.IsNullOrEmpty(cReplyTo)))
                                {
                                    pBuyerEmail = cReplyTo;
                                }
                                else if ((_value != "10" ? false : !string.IsNullOrEmpty(cReplyTo)))
                                {
                                    pBuyerEmail = cReplyTo;
                                }
                            }
                        }
                        else
                        {
                            mailFound = true;
                            cReturn = cEMLFile;
                        }
                        if (!mailFound)
                        {
                            counter++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                DataLog.AddLog(string.Concat("227. Unable to get EML file ", cName, ". Error - ", ex.StackTrace));
                cReturn = "";
            }
            return cReturn;
        }

        public string[] GetFooterText(string FileName, int LineCount)
        {
            int i;
            string[] strArrays;
            Document _pdf = null;
            string extractedText = "";
            string[] FooterText = null;
            try
            {
                try
                {
                    FooterText = new string[this.PageCount];
                    _pdf = new Document(FileName);
                    for (int p = 1; p <= this.PageCount; p++)
                    {
                        string _footer = "";
                        List<string> lstText = new List<string>();
                        TextAbsorber _obj = new TextAbsorber();
                        _pdf.Pages[p].Accept(_obj);
                        extractedText = _obj.Text.Replace("\0", " ");
                        RichTextBox txtBox = new RichTextBox()
                        {
                            Text = extractedText
                        };
                        for (i = (int)txtBox.Lines.Length - 1; i >= (int)txtBox.Lines.Length - LineCount; i--)
                        {
                            lstText.Add(txtBox.Lines[i]);
                        }
                        for (i = lstText.Count - 1; i >= 0; i--)
                        {
                            _footer = (i != lstText.Count - 1 ? string.Concat(_footer, Environment.NewLine, lstText[i]) : string.Concat(_footer, lstText[i]));
                        }
                        FooterText[p - 1] = _footer;
                        txtBox.Dispose();
                    }
                    strArrays = FooterText;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            finally
            {
                if (_pdf != null)
                {
                    _pdf.FreeMemory();
                    _pdf.Dispose();
                }
            }
            return strArrays;
        }

        public string[] GetFooterText(string FileName, int LineCount, bool RemoveBlankLines)
        {
            int i;
            string[] strArrays;
            Document _pdf = null;
            string extractedText = "";
            string[] FooterText = null;
            try
            {
                try
                {
                    FooterText = new string[this.PageCount];
                    _pdf = new Document(FileName);
                    for (int p = 1; p <= this.PageCount; p++)
                    {
                        string _footer = "";
                        List<string> lstText = new List<string>();
                        TextAbsorber _obj = new TextAbsorber();
                        _pdf.Pages[p].Accept(_obj);
                        extractedText = _obj.Text;
                        extractedText = extractedText.Replace("\0", " ");
                        if (RemoveBlankLines)
                        {
                            extractedText = extractedText.Replace("\r\r", "\r");
                            string BlanckLines = string.Concat(Environment.NewLine, Environment.NewLine);
                            while (extractedText.Contains(BlanckLines))
                            {
                                extractedText = extractedText.Replace(BlanckLines, Environment.NewLine);
                            }
                        }
                        RichTextBox txtBox = new RichTextBox()
                        {
                            Text = extractedText
                        };
                        for (i = (int)txtBox.Lines.Length - 1; i >= (int)txtBox.Lines.Length - LineCount; i--)
                        {
                            if (i >= 0)
                            {
                                lstText.Add(txtBox.Lines[i]);
                            }
                        }
                        for (i = lstText.Count - 1; i >= 0; i--)
                        {
                            _footer = (!(_footer.Trim() == "") ? string.Concat(_footer, Environment.NewLine, lstText[i]) : string.Concat(_footer, lstText[i]));
                        }
                        FooterText[p - 1] = _footer;
                        txtBox.Dispose();
                    }
                    strArrays = FooterText;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            finally
            {
                if (_pdf != null)
                {
                    _pdf.FreeMemory();
                    _pdf.Dispose();
                }
            }
            return strArrays;
        }

        public string[] GetHeaderText(string FileName, int LineCount)
        {
            int i;
            string[] strArrays;
            Document _pdf = null;
            string extractedText = "";
            string[] HeaderText = null;
            try
            {
                try
                {
                    HeaderText = new string[this.PageCount];
                    for (int p = 1; p <= this.PageCount; p++)
                    {
                        string _header = "";
                        TextAbsorber _obj = new TextAbsorber();
                        List<string> lstText = new List<string>();
                        _pdf = new Document(FileName);
                        _pdf.Pages[p].Accept(_obj);
                        extractedText = _obj.Text.Replace("\0", " ");
                        RichTextBox txtBox = new RichTextBox();
                        char[] chrArray = new char[] { ' ' };
                        string str = extractedText.TrimStart(chrArray);
                        chrArray = new char[] { '\r' };
                        string str1 = str.TrimStart(chrArray);
                        chrArray = new char[] { '\n' };
                        string str2 = str1.TrimStart(chrArray);
                        chrArray = new char[] { '\r' };
                        string str3 = str2.TrimEnd(chrArray);
                        chrArray = new char[] { '\n' };
                        txtBox.Text = str3.TrimEnd(chrArray);
                        for (i = 0; i < LineCount; i++)
                        {
                            lstText.Add(txtBox.Lines[i]);
                        }
                        for (i = 0; i < lstText.Count; i++)
                        {
                            _header = (i != 0 ? string.Concat(_header, Environment.NewLine, lstText[i]) : string.Concat(_header, lstText[i]));
                        }
                        HeaderText[p - 1] = _header;
                        txtBox.Dispose();
                    }
                    strArrays = HeaderText;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            finally
            {
                if (_pdf != null)
                {
                    _pdf.FreeMemory();
                    _pdf.Dispose();
                }
            }
            return strArrays;
        }

        public string[] GetHeaderText(string FileName, int LineCount, bool RemoveBlankLines)
        {
            int i;
            string[] strArrays;
            Document _pdf = null;
            string extractedText = "";
            string[] HeaderText = null;
            try
            {
                try
                {
                    HeaderText = new string[this.PageCount];
                    _pdf = new Document(FileName);
                    for (int p = 1; p <= this.PageCount; p++)
                    {
                        string _header = "";
                        TextAbsorber _obj = new TextAbsorber();
                        List<string> lstText = new List<string>();
                        _pdf.Pages[p].Accept(_obj);
                        extractedText = _obj.Text;
                        extractedText = extractedText.Replace("\0", " ");
                        if (RemoveBlankLines)
                        {
                            extractedText = extractedText.Replace("\r\r", "\r");
                            string BlanckLines = string.Concat(Environment.NewLine, Environment.NewLine);
                            while (extractedText.Contains(BlanckLines))
                            {
                                extractedText = extractedText.Replace(BlanckLines, Environment.NewLine);
                            }
                        }
                        RichTextBox txtBox = new RichTextBox();
                        char[] chrArray = new char[] { ' ' };
                        string str = extractedText.TrimStart(chrArray);
                        chrArray = new char[] { '\r' };
                        string str1 = str.TrimStart(chrArray);
                        chrArray = new char[] { '\n' };
                        string str2 = str1.TrimStart(chrArray);
                        chrArray = new char[] { '\r' };
                        string str3 = str2.TrimEnd(chrArray);
                        chrArray = new char[] { '\n' };
                        txtBox.Text = str3.TrimEnd(chrArray);
                        for (i = 0; i < LineCount; i++)
                        {
                            if ((int)txtBox.Lines.Length > i)
                            {
                                lstText.Add(txtBox.Lines[i]);
                            }
                        }
                        for (i = 0; i < lstText.Count; i++)
                        {
                            _header = (!(_header.Trim() == "") ? string.Concat(_header, Environment.NewLine, lstText[i]) : string.Concat(_header, lstText[i]));
                        }
                        HeaderText[p - 1] = _header;
                        txtBox.Dispose();
                    }
                    strArrays = HeaderText;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            finally
            {
                if (_pdf != null)
                {
                    _pdf.FreeMemory();
                    _pdf.Dispose();
                }
            }
            return strArrays;
        }

        public string GetPageFooterText(string FileName, int LineCount, bool RemoveBlankLines)
        {
            int i;
            string str;
            Document _pdf = null;
            string extractedText = "";
            try
            {
                try
                {
                    int FromPage = 1;
                    if (this.PageCount > 1)
                    {
                        FromPage = 2;
                    }
                    string _footer = "";
                    TextAbsorber _obj = new TextAbsorber();
                    List<string> lstText = new List<string>();
                    _pdf = new Document(FileName);
                    this.PageCount = _pdf.Pages.Count;
                    if (this.PageCount > 2)
                    {
                        FromPage = this.PageCount / 2 + 1;
                        if (FromPage >= this.PageCount)
                        {
                            FromPage--;
                        }
                    }
                    _pdf.Pages[FromPage].Accept(_obj);
                    extractedText = _obj.Text;
                    extractedText = extractedText.Replace("\0", " ");
                    while (true)
                    {
                        if ((extractedText != "" ? true : FromPage >= this.PageCount))
                        {
                            break;
                        }
                        FromPage++;
                        _pdf.Pages[FromPage].Accept(_obj);
                        extractedText = _obj.Text;
                        extractedText = extractedText.Replace("\0", " ");
                    }
                    if (RemoveBlankLines)
                    {
                        extractedText = extractedText.Replace("\r\r", "\r");
                        string BlanckLines = string.Concat(Environment.NewLine, Environment.NewLine);
                        while (extractedText.Contains(BlanckLines))
                        {
                            extractedText = extractedText.Replace(BlanckLines, Environment.NewLine);
                        }
                    }
                    if (extractedText.Trim().Length > 0)
                    {
                        RichTextBox txtBox = new RichTextBox()
                        {
                            Text = extractedText
                        };
                        if ((int)txtBox.Lines.Length > 0)
                        {
                            for (i = (int)txtBox.Lines.Length - 1; i >= (int)txtBox.Lines.Length - LineCount; i--)
                            {
                                lstText.Add(txtBox.Lines[i]);
                            }
                        }
                        for (i = lstText.Count - 1; i >= 0; i--)
                        {
                            _footer = (!(_footer.Trim() == "") ? string.Concat(_footer, Environment.NewLine, lstText[i]) : string.Concat(_footer, lstText[i]));
                        }
                        txtBox.Dispose();
                    }
                    extractedText = _footer;
                    str = extractedText;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            finally
            {
                if (_pdf != null)
                {
                    _pdf.FreeMemory();
                    _pdf.Dispose();
                }
            }
            return str;
        }

        public string GetPageHeaderText(string FileName, int LineCount, bool RemoveBlankLines)
        {
            int i;
            string str;
            Document _pdf = null;
            string extractedText = "";
            try
            {
                try
                {
                    int FromPage = 1;
                    FromPage = (this.PageCount <= 1 ? 1 : 2);
                    string _header = "";
                    TextAbsorber _obj = new TextAbsorber();
                    List<string> lstText = new List<string>();
                    _pdf = new Document(FileName);
                    this.PageCount = _pdf.Pages.Count;
                    if (this.PageCount > 2)
                    {
                        FromPage = this.PageCount / 2 + 1;
                        if (FromPage >= this.PageCount)
                        {
                            FromPage--;
                        }
                    }
                    _pdf.Pages[FromPage].Accept(_obj);
                    extractedText = _obj.Text;
                    extractedText = extractedText.Replace("\0", " ");
                    while (true)
                    {
                        if ((extractedText != "" ? true : FromPage >= this.PageCount))
                        {
                            break;
                        }
                        FromPage++;
                        _pdf.Pages[FromPage].Accept(_obj);
                        extractedText = _obj.Text;
                        extractedText = extractedText.Replace("\0", " ");
                    }
                    if (RemoveBlankLines)
                    {
                        extractedText = extractedText.Replace("\r\r", "\r");
                        string BlanckLines = string.Concat(Environment.NewLine, Environment.NewLine);
                        while (extractedText.Contains(BlanckLines))
                        {
                            extractedText = extractedText.Replace(BlanckLines, Environment.NewLine);
                        }
                    }
                    if (extractedText.Trim().Length > 0)
                    {
                        RichTextBox txtBox = new RichTextBox()
                        {
                            Text = extractedText
                        };
                        for (i = 0; i < LineCount; i++)
                        {
                            lstText.Add(txtBox.Lines[i]);
                        }
                        for (i = 0; i < lstText.Count; i++)
                        {
                            _header = (!(_header.Trim() == "") ? string.Concat(_header, Environment.NewLine, lstText[i]) : string.Concat(_header, lstText[i]));
                        }
                        txtBox.Dispose();
                    }
                    extractedText = _header;
                    str = extractedText;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            finally
            {
                if (_pdf != null)
                {
                    _pdf.FreeMemory();
                    _pdf.Dispose();
                }
            }
            return str;
        }

        public string GetRow(string[] Lines, string FindText)
        {
            string _row = "";
            string[] lines = Lines;
            int num = 0;
            while (num < (int)lines.Length)
            {
                string line = lines[num];
                if (!line.Contains(FindText))
                {
                    num++;
                }
                else
                {
                    _row = line;
                    break;
                }
            }
            return _row;
        }

        public string GetRow(string[] Lines, string FindText, ref int index)
        {
            string _row = "";
            int i = 0;
            while (i < (int)Lines.Length)
            {
                if (!Lines[i].Contains(FindText))
                {
                    i++;
                }
                else
                {
                    _row = Lines[i];
                    index = i;
                    break;
                }
            }
            return _row;
        }

        public string GetText(string FileName)
        {
            string str;
            Document _pdf = null;
            string extractedText = "";
            try
            {
                try
                {
                    string ext = Path.GetExtension(FileName);
                    char[] chrArray = new char[] { '.' };
                    ext = ext.Trim(chrArray).ToLower();
                    if (ext == "pdf")
                    {
                        _pdf = new Document(FileName);
                        TextAbsorber _obj = new TextAbsorber();
                        _pdf.Pages.Accept(_obj);
                        this.PageCount = _pdf.Pages.Count;
                        extractedText = _obj.Text;
                        extractedText = extractedText.Replace("\0", " ");
                    }
                    else if ((ext == "rtf" || ext == "doc" ? true : ext == "docx"))
                    {
                        extractedText = (new Aspose.Words.Document(FileName)).GetText();
                        extractedText = extractedText.Replace("\r", Environment.NewLine).TrimStart(Environment.NewLine.ToCharArray());
                    }
                    str = extractedText;
                }
                catch (Exception exception)
                {
                    Exception ex = exception;
                    this.PageCount = 0;
                    throw ex;
                }
            }
            finally
            {
                if (_pdf != null)
                {
                    _pdf.FreeMemory();
                    _pdf.Dispose();
                }
            }
            return str;
        }

        public string GetText(string FileName, bool RemoveBlankLines)
        {
            string str;
            Document _pdf = null;
            string extractedText = "";
            try
            {
                try
                {
                    string ext = Path.GetExtension(FileName);
                    char[] chrArray = new char[] { '.' };
                    ext = ext.Trim(chrArray).ToLower();
                    if (ext == "pdf")
                    {
                        _pdf = new Document(FileName);
                        TextAbsorber _obj = new TextAbsorber();
                        _pdf.Pages.Accept(_obj);
                        this.PageCount = _pdf.Pages.Count;
                        extractedText = _obj.Text;
                        extractedText = extractedText.Replace("\0", " ");
                    }
                    else if ((ext == "rtf" || ext == "doc" ? true : ext == "docx"))
                    {
                        extractedText = (new Aspose.Words.Document(FileName)).GetText();
                        extractedText = extractedText.Replace("\r", Environment.NewLine).TrimStart(Environment.NewLine.ToCharArray());
                    }
                    if (RemoveBlankLines)
                    {
                        extractedText = extractedText.Replace("\r\r", "\r");
                        string BlanckLines = string.Concat(Environment.NewLine, Environment.NewLine);
                        while (extractedText.Contains(BlanckLines))
                        {
                            extractedText = extractedText.Replace(BlanckLines, Environment.NewLine);
                        }
                    }
                    str = extractedText;
                }
                catch (Exception exception)
                {
                    Exception ex = exception;
                    this.PageCount = 0;
                    throw ex;
                }
            }
            finally
            {
                if (_pdf != null)
                {
                    _pdf.FreeMemory();
                    _pdf.Dispose();
                }
            }
            return str;
        }

        public string GetText(string FileName, bool RemoveBlankLines, int StartPage)
        {
            string str;
            string extractedText = "";
            Document _pdf = null;
            try
            {
                try
                {
                    string ext = Path.GetExtension(FileName);
                    char[] chrArray = new char[] { '.' };
                    ext = ext.Trim(chrArray).ToLower();
                    if (ext == "pdf")
                    {
                        _pdf = new Document(FileName);
                        TextAbsorber _obj = new TextAbsorber();
                        this.PageCount = _pdf.Pages.Count;
                        for (int i = StartPage; i < this.PageCount; i++)
                        {
                            _pdf.Pages[i].Accept(_obj);
                            extractedText = string.Concat(extractedText, _obj.Text);
                            extractedText = extractedText.Replace("\0", " ");
                        }
                    }
                    else if ((ext == "rtf" || ext == "doc" ? true : ext == "docx"))
                    {
                        extractedText = (new Aspose.Words.Document(FileName)).GetText();
                        extractedText = extractedText.Replace("\r", Environment.NewLine).TrimStart(Environment.NewLine.ToCharArray());
                    }
                    if (RemoveBlankLines)
                    {
                        extractedText = extractedText.Replace("\r\r", "\r");
                        string BlanckLines = string.Concat(Environment.NewLine, Environment.NewLine);
                        while (extractedText.Contains(BlanckLines))
                        {
                            extractedText = extractedText.Replace(BlanckLines, Environment.NewLine);
                        }
                    }
                    str = extractedText;
                }
                catch (Exception exception)
                {
                    Exception ex = exception;
                    this.PageCount = 0;
                    throw ex;
                }
            }
            finally
            {
                if (_pdf != null)
                {
                    _pdf.FreeMemory();
                    _pdf.Dispose();
                }
            }
            return str;
        }

        public List<string> GetText(string FileName, bool RemoveBlankLines, string value)
        {
            TextAbsorber _obj;
            string BlanckLines;
            List<string> strs;
            Document _pdf = null;
            List<string> lstTexts = new List<string>();
            string extractedText = "";
            try
            {
                try
                {
                    int StartPage = 0;
                    _pdf = new Document(FileName);
                    this.PageCount = _pdf.Pages.Count;
                    int i = 1;
                    while (i <= this.PageCount)
                    {
                        _obj = new TextAbsorber();
                        _pdf.Pages[i].Accept(_obj);
                        if (!_obj.Text.Contains(value))
                        {
                            i++;
                        }
                        else
                        {
                            StartPage = i;
                            break;
                        }
                    }
                    for (i = 1; i <= StartPage; i++)
                    {
                        _obj = new TextAbsorber();
                        _pdf.Pages[i].Accept(_obj);
                        extractedText = string.Concat(extractedText, _obj.Text);
                        extractedText = extractedText.Replace("\0", " ");
                    }
                    if (RemoveBlankLines)
                    {
                        BlanckLines = string.Concat(Environment.NewLine, Environment.NewLine);
                        while (extractedText.Contains(BlanckLines))
                        {
                            extractedText = extractedText.Replace(BlanckLines, Environment.NewLine);
                        }
                    }
                    lstTexts.Add(extractedText);
                    extractedText = "";
                    for (i = StartPage + 1; i <= this.PageCount; i++)
                    {
                        _obj = new TextAbsorber();
                        _pdf.Pages[i].Accept(_obj);
                        extractedText = string.Concat(extractedText, _obj.Text);
                    }
                    if (RemoveBlankLines)
                    {
                        BlanckLines = string.Concat(Environment.NewLine, Environment.NewLine);
                        while (extractedText.Contains(BlanckLines))
                        {
                            extractedText = extractedText.Replace(BlanckLines, Environment.NewLine);
                        }
                    }
                    lstTexts.Add(extractedText);
                    strs = lstTexts;
                }
                catch (Exception exception)
                {
                    Exception ex = exception;
                    this.PageCount = 0;
                    throw ex;
                }
            }
            finally
            {
                if (_pdf != null)
                {
                    _pdf.FreeMemory();
                    _pdf.Dispose();
                }
            }
            return strs;
        }

        public List<string> GetTextPages(string FileName, bool RemoveBlankLines)
        {
            List<string> strs;
            string extractedText = "";
            List<string> lstPages = new List<string>();
            try
            {
                string ext = Path.GetExtension(FileName);
                char[] chrArray = new char[] { '.' };
                ext = ext.Trim(chrArray).ToLower();
                for (int p = 1; p <= this.PageCount; p++)
                {
                    List<string> lstText = new List<string>();
                    if (ext == "pdf")
                    {
                        Document _pdf = new Document(FileName);
                        try
                        {
                            TextAbsorber _obj = new TextAbsorber();
                            _pdf.Pages[p].Accept(_obj);
                            extractedText = _obj.Text;
                            _pdf.FreeMemory();
                            _pdf.Dispose();
                        }
                        finally
                        {
                            if (_pdf != null)
                            {
                                ((IDisposable)_pdf).Dispose();
                            }
                        }
                    }
                    else if (ext == "txt")
                    {
                        extractedText = File.ReadAllText(FileName);
                    }
                    extractedText = extractedText.Replace("\0", " ");
                    if (RemoveBlankLines)
                    {
                        string BlanckLines = string.Concat(Environment.NewLine, Environment.NewLine);
                        while (extractedText.Contains(BlanckLines))
                        {
                            extractedText = extractedText.Replace(BlanckLines, Environment.NewLine);
                        }
                    }
                    lstPages.Add(extractedText);
                }
                strs = lstPages;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return strs;
        }

        private string GetValue(RichTextBox t, string FieldData)
        {
            int nStartRow = -1;
            int nEndRow = -1;
            int nStartCol = -1;
            int nEndCol = -1;
            string[] Index = convert.ToString(FieldData).Split(new char[] { ':' });
            if (((int)Index.Length <= 0 ? true : !int.TryParse(Index[0], out nStartRow)))
            {
                nStartRow = -1;
            }
            if (((int)Index.Length <= 1 ? true : !int.TryParse(Index[1], out nEndRow)))
            {
                nEndRow = -1;
            }
            if (((int)Index.Length <= 2 ? true : !int.TryParse(Index[2], out nStartCol)))
            {
                nStartCol = -1;
            }
            if (((int)Index.Length <= 3 ? true : !int.TryParse(Index[3], out nEndCol)))
            {
                nEndCol = -1;
            }
            string _value = "";
            if ((nStartRow <= 0 ? false : (int)t.Lines.Length > nStartRow))
            {
                if (!(nEndCol <= nStartCol ? true : t.Lines[nStartRow - 1].Length <= nEndCol))
                {
                    _value = t.Lines[nStartRow - 1].Substring(nStartCol - 1, nEndCol - nStartCol).Trim();
                }
                else if ((nStartCol == -1 ? false : t.Lines[nStartRow - 1].Length >= nStartCol - 1))
                {
                    _value = t.Lines[nStartRow - 1].Substring(nStartCol - 1).Trim();
                }
                if (nEndRow > 0)
                {
                    for (int i = nStartRow; i < nEndRow; i++)
                    {
                        if (!(nEndCol <= nStartCol ? true : t.Lines[i].Length <= nEndCol))
                        {
                            _value = string.Concat(_value, " ", t.Lines[i].Substring(nStartCol - 1, nEndCol - nStartCol).Trim());
                        }
                        else if (!(t.Lines[i].Trim() == "" ? true : t.Lines[i].Length <= nStartCol - 1))
                        {
                            _value = string.Concat(_value, " ", t.Lines[i].Substring(nStartCol - 1).Trim());
                        }
                        else if (nStartCol == 1)
                        {
                            _value = string.Concat(_value, " ", t.Lines[i].Trim());
                        }
                    }
                }
            }
            return _value;
        }

        public bool IsBoldText(string FileName, string txtLine)
        {
            bool isBold = false;
            try
            {
                Document _pdf = new Document(FileName);
                try
                {
                    TextSearchOptions schOpt = new TextSearchOptions(false);
                    TextFragmentAbsorber textFragmentAbsorber = new TextFragmentAbsorber(txtLine.Trim(), schOpt);
                    _pdf.Pages.Accept(textFragmentAbsorber);
                    foreach (TextFragment textFragment in textFragmentAbsorber.TextFragments)
                    {
                        isBold = Convert.ToString(textFragment.TextState.Font.FontName).Trim().ToUpper().Contains("BOLD");
                    }
                }
                finally
                {
                    if (_pdf != null)
                    {
                        ((IDisposable)_pdf).Dispose();
                    }
                }
            }
            catch
            {
            }
            return isBold;
        }

        public bool IsScannedDocument(FileInfo pdfFile)
        {
            bool isScannedFile = false;
            try
            {
                if (this.GetText(pdfFile.FullName).Trim().Length == 0)
                {
                    if (!pdfFile.Name.StartsWith("RFQ"))
                    {
                        isScannedFile = true;
                    }
                }
            }
            catch
            {
            }
            return isScannedFile;
        }

        public string SetBuyerEmailByRules(string MsgMailID, string PdfMailID, BuyerSupplierInfo ByrSuppInfo)
        {
            string finalBuyerMail = "";
            try
            {
                if (convert.ToInt((new SMData_Routines()).GetRuleValue(ByrSuppInfo, "READ_MSG_CC_EMAIL_AS_PARTY_EMAIL")) != 3)
                {
                    finalBuyerMail = (MsgMailID.Trim().Length <= 0 ? PdfMailID : MsgMailID);
                }
                else
                {
                    finalBuyerMail = PdfMailID.Trim();
                }
            }
            catch (Exception exception)
            {
                DataLog.AddLog(string.Concat("Error in PDFPlugin.CommonRoutine.SetBuyerEmailByRules(); ", exception.Message));
                string str = string.Concat(MsgMailID, "; ", PdfMailID);
                char[] chrArray = new char[] { ';' };
                finalBuyerMail = str.Trim(chrArray).Trim();
            }
            return finalBuyerMail;
        }

        public string TrimTextFromStarting(RichTextBox txt)
        {
            string text;
            try
            {
                string[] s = txt.Lines;
                for (int i = 0; i < (int)s.Length; i++)
                {
                    string str = s[i];
                    char[] chrArray = new char[] { ' ' };
                    s[i] = str.TrimStart(chrArray);
                }
                txt.Lines = s;
                text = txt.Text;
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                this.PageCount = 0;
                throw ex;
            }
            return text;
        }
    }
}