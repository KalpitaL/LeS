using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Excel_SPL_Routine
{
    class Excel_Common_Routine
    {
        public Aspose.Cells.Workbook _workbook { get; set; }
        public Aspose.Cells.Worksheet _worksheet { get; set; }
        public Aspose.Cells.License license = null;
        public string _logpath { get; set; }

        public enum MandatoryQualifier { Yes, No }

        public string LogText { set { WriteLog(value); } }

        private void WriteLog(string _logText, string _logFile = "")
        {
            if (_logpath == null || _logpath == "") _logpath = AppDomain.CurrentDomain.BaseDirectory + "\\Log";
            string _logfile = @"Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (_logFile.Length > 0) { _logfile = _logFile; }
            Directory.CreateDirectory(_logpath);

            Console.WriteLine(_logText);
            File.AppendAllText(_logpath + @"\" + @_logfile, DateTime.Now.ToString() + " " + _logText + Environment.NewLine);

        }

        public Excel_Common_Routine()
        {

        }

        public Excel_Common_Routine(string fXLS)
        {
            license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Total.lic");
            _workbook = new Aspose.Cells.Workbook(fXLS);
        }

        public Boolean SetFileWorkSheet(string _File)
        {
            string fileName = _File.Split('\\')[_File.Split('\\').Length - 1];
            Boolean _return = false;
            try
            {
                _worksheet = _workbook.Worksheets[_workbook.Worksheets.ActiveSheetIndex];//set active worksheet
                if (_worksheet == null)
                    _worksheet = _workbook.Worksheets[0];
                if (_worksheet == null)
                {
                    LogText = "Unable to set worksheet, no active sheet present - " + fileName;

                    throw new Exception("Unable to set worksheet, no active sheet present - " + fileName);
                 }
            }
            catch (Exception ex)
            {
                LogText = "Unable to set worksheet, no active sheet present - " + fileName + " : " + ex.Message;
                throw new Exception("Unable to set worksheet, no active sheet present - " + fileName + " : " + ex.Message);
            }
            return _return;
        }

        public string GetExcelCell(string _Cell)
        {
            int nSheet = 0;
            string cCell = "", cReturn = "";

            string[] slCell = _Cell.Split('.');

            if (slCell.Length > 1)
            {
                nSheet = convert.ToInt(slCell[0].ToString().Trim());
                cCell = slCell[1].Trim();
            }
            else if (slCell.Length > 0)
            {
                nSheet = _workbook.Worksheets.ActiveSheetIndex;
                cCell = slCell[0].Trim();
            }

            if (cCell.Length > 0) // check for cell 
            {
                try
                {
                    if (_workbook.Worksheets[nSheet].Cells[cCell].Value != null)
                        cReturn = _workbook.Worksheets[nSheet].Cells[cCell].Value.ToString().Trim();
                }
                catch (Exception ex)
                {
                    LogText = "Error in get cell value [" + _Cell + "] Error Message : " + ex.Message;
                    
                    throw new Exception("Error in get cell value [" + _Cell + "] Error Message : " + ex.Message);
                }
            }
            return cReturn;
        }

        public string GetCellDynamicData(int nStart, int nEnd, string cColumn, string cPattern, int nAddRow, int nAddCol)
        {
            string cCell = "";
            if (nEnd <= nStart) nEnd = nStart + 1;

            for (int i = nStart; i <= nEnd; i++)
            {
                if (convert.ToString(GetExcelCell(cColumn + i)).Trim() == cPattern.Trim())
                {
                    cCell = cColumn + i;
                    break;
                }
            }
            string cExcel = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string cReturn = cCell;
            string cCol = ""; string cRow = "";

            if (!string.IsNullOrEmpty(cCell))
            {
                foreach (char ch in cCell)
                {
                    if (cExcel.Contains(ch.ToString()))
                        cCol += ch;
                    else cRow += ch;
                }
                cRow = (convert.ToInt(cRow) + nAddRow).ToString();
                if (cExcel.IndexOf(cCol) < 0)
                {
                    if (cCol.Length > 2)
                    {
                        throw new Exception("Excel Header Index is too long " + cCol);
                    }
                    string fChar = cExcel[cExcel.IndexOf(cCol[0])].ToString();
                    int fCharIndex = cExcel.IndexOf(fChar);
                    if (cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(nAddCol) > 26)
                    {
                        if ((fCharIndex + 1) > 26)
                        {
                            throw new Exception("Excel Header Index is max length ");
                        }
                        else
                        {
                            cCol = cExcel[(fCharIndex + 1)].ToString() + cExcel[cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(nAddCol) - 26].ToString();
                        }
                    }
                    else
                    {
                        cCol = fChar + cExcel[cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(nAddCol)].ToString();
                    }
                }
                else
                {
                    cCol = cExcel[cExcel.IndexOf(cCol) + convert.ToInt(nAddCol)].ToString();
                }

                if ((cRow.Length > 0) && (cCol.Length > 0))
                    cReturn = GetExcelCell(cCol + cRow);
            }
            return cReturn;
        }

        public string GetCellDynamicData(int nStart, int nEnd, string cColumn, string cPattern, int nAddRow, string nAddCol, MandatoryQualifier cVal)
        {
            string cReturn = "";
            if (cColumn.Split('|').Length > 1)
            {
                string[] cnColumnList = cColumn.Split('|');
                string[] cnAddColList = nAddCol.Split('|');

                for (int r = 0; r < cnColumnList.Length; r++)
                {
                    string cCell = "";
                    if (nEnd <= nStart) nEnd = nStart + 1;
                    for (int i = nStart; i <= nEnd; i++)
                    {
                        if (convert.ToString(GetExcelCell(cnColumnList[r] + i)).Trim() == cPattern.Trim())
                        {
                            cCell = cnColumnList[r] + i;
                            break;
                        }
                    }
                    string cExcel = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    cReturn = cCell;
                    string cCol = ""; string cRow = "";
                    if (!string.IsNullOrEmpty(cCell))
                    {
                        foreach (char ch in cCell)
                        {
                            if (cExcel.Contains(ch.ToString()))
                                cCol += ch;
                            else cRow += ch;
                        }
                        cRow = (convert.ToInt(cRow) + nAddRow).ToString();

                        if (cExcel.IndexOf(cCol) < 0)
                        {
                            if (cCol.Length > 2)
                            {
                                throw new Exception("Excel Header Index is too long " + cCol);
                            }
                            string fChar = cExcel[cExcel.IndexOf(cCol[0])].ToString();
                            int fCharIndex = cExcel.IndexOf(fChar);
                            if (cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(cnAddColList[r]) > 26)
                            {
                                if ((fCharIndex + 1) > 26)
                                {
                                    throw new Exception("Excel Header Index out of range");
                                }
                                else
                                {
                                    cCol = cExcel[(fCharIndex + 1)].ToString() + cExcel[cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(cnAddColList[r]) - 26].ToString();
                                }
                            }
                            else
                            {
                                cCol = fChar + cExcel[cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(cnAddColList[r])].ToString();
                            }
                        }
                        else
                        {
                            cCol = cExcel[cExcel.IndexOf(cCol) + convert.ToInt(cnAddColList[r])].ToString();
                        }

                        if ((cRow.Length > 0) && (cCol.Length > 0))
                            cReturn = GetExcelCell(cCol + cRow);
                        if (cVal == MandatoryQualifier.Yes)
                        {
                            if (cReturn.Trim() == "")
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else
            {
                string cCell = "";
                if (nEnd <= nStart) nEnd = nStart + 1;
                for (int i = nStart; i <= nEnd; i++)
                {
                    if (convert.ToString(GetExcelCell(cColumn + i)).Trim() == cPattern.Trim())
                    {
                        cCell = cColumn + i;
                        break;
                    }
                }
                string cExcel = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                cReturn = cCell;
                string cCol = ""; string cRow = "";
                if (!string.IsNullOrEmpty(cCell))
                {
                    foreach (char ch in cCell)
                    {
                        if (cExcel.Contains(ch.ToString()))
                            cCol += ch;
                        else cRow += ch;
                    }
                    cRow = (convert.ToInt(cRow) + convert.ToInt(nAddRow)).ToString();
                    if (cExcel.IndexOf(cCol) < 0)
                    {
                        if (cCol.Length > 2)
                        {
                            throw new Exception("Excel Header Index is too long " + cCol);
                        }
                        string fChar = cExcel[cExcel.IndexOf(cCol[0])].ToString();
                        int fCharIndex = cExcel.IndexOf(fChar);
                        if (cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(nAddCol) > 26)
                        {
                            if ((fCharIndex + 1) > 26)
                            {
                                throw new Exception("Excel Header Index is max length ");
                            }
                            else
                            {
                                cCol = cExcel[(fCharIndex + 1)].ToString() + cExcel[cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(nAddCol) - 26].ToString();
                            }
                        }
                        else
                        {
                            cCol = fChar + cExcel[cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(nAddCol)].ToString();
                        }
                    }
                    else
                    {
                         cCol = cExcel[cExcel.IndexOf(cCol) + convert.ToInt(nAddCol)].ToString();
                    }
                    if ((cRow.Length > 0) && (cCol.Length > 0))
                        cReturn = GetExcelCell(cCol + cRow);
                }
            }
            if (cVal == MandatoryQualifier.Yes)
            {
                if (cReturn.Trim() == "")
                {
                    throw new Exception("Unable to get details for " + cPattern);
                }
            }
            return cReturn;
        }

        public string GetCellDynamicData(int nStart, int nEnd, string cColumn, string cPattern, int nAddRow, string nAddCol, MandatoryQualifier cVal, ref int RefRow)
        {
            string cReturn = "";
            if (cColumn.Split('|').Length > 1)
            {
                string[] cnColumnList = cColumn.Split('|');
                string[] cnAddColList = nAddCol.Split('|');

                for (int r = 0; r < cnColumnList.Length; r++)
                {
                    string cCell = "";
                    if (nEnd <= nStart) nEnd = nStart + 1;
                    for (int i = nStart; i <= nEnd; i++)
                    {
                        if (convert.ToString(GetExcelCell(cnColumnList[r] + i)).Trim() == cPattern.Trim())
                        {
                            cCell = cnColumnList[r] + i;
                            RefRow = i;
                            break;
                        }
                    }
                    string cExcel = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    cReturn = cCell;
                    string cCol = ""; string cRow = "";
                    if (!string.IsNullOrEmpty(cCell))
                    {
                        foreach (char ch in cCell)
                        {
                            if (cExcel.Contains(ch.ToString()))
                                cCol += ch;
                            else cRow += ch;
                        }
                        cRow = (convert.ToInt(cRow) + nAddRow).ToString();

                        if (cExcel.IndexOf(cCol) < 0)
                        {
                            if (cCol.Length > 2)
                            {
                                throw new Exception("Excel Header Index is too long " + cCol);
                            }
                            string fChar = cExcel[cExcel.IndexOf(cCol[0])].ToString();
                            int fCharIndex = cExcel.IndexOf(fChar);
                            if (cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(cnAddColList[r]) > 26)
                            {
                                if ((fCharIndex + 1) > 26)
                                {
                                    throw new Exception("Excel Header Index out of range");
                                }
                                else
                                {
                                    cCol = cExcel[(fCharIndex + 1)].ToString() + cExcel[cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(cnAddColList[r]) - 26].ToString();
                                }
                            }
                            else
                            {
                                cCol = fChar + cExcel[cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(cnAddColList[r])].ToString();
                            }
                        }
                        else
                        {
                            cCol = cExcel[cExcel.IndexOf(cCol) + convert.ToInt(cnAddColList[r])].ToString();
                        }

                        if ((cRow.Length > 0) && (cCol.Length > 0))
                            cReturn = GetExcelCell(cCol + cRow);
                        if (cVal == MandatoryQualifier.Yes)
                        {
                            if (cReturn.Trim() == "")
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else
            {
                string cCell = "";
                if (nEnd <= nStart) nEnd = nStart + 1;
                for (int i = nStart; i <= nEnd; i++)
                {
                    if (convert.ToString(GetExcelCell(cColumn + i)).Trim() == cPattern.Trim())
                    {
                        cCell = cColumn + i;
                        RefRow = i;
                        break;
                    }
                }
                string cExcel = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                cReturn = cCell;
                string cCol = ""; string cRow = "";
                if (!string.IsNullOrEmpty(cCell))
                {
                    foreach (char ch in cCell)
                    {
                        if (cExcel.Contains(ch.ToString()))
                            cCol += ch;
                        else cRow += ch;
                    }
                    cRow = (convert.ToInt(cRow) + convert.ToInt(nAddRow)).ToString();
                    if (cExcel.IndexOf(cCol) < 0)
                    {
                        if (cCol.Length > 2)
                        {
                            throw new Exception("Excel Header Index is too long " + cCol);
                        }
                        string fChar = cExcel[cExcel.IndexOf(cCol[0])].ToString();
                        int fCharIndex = cExcel.IndexOf(fChar);
                        if (cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(nAddCol) > 26)
                        {
                            if ((fCharIndex + 1) > 26)
                            {
                                throw new Exception("Excel Header Index is max length ");
                            }
                            else
                            {
                                cCol = cExcel[(fCharIndex + 1)].ToString() + cExcel[cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(nAddCol) - 26].ToString();
                            }
                        }
                        else
                        {
                            cCol = fChar + cExcel[cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(nAddCol)].ToString();
                        }
                    }
                    else
                    {
                        cCol = cExcel[cExcel.IndexOf(cCol) + convert.ToInt(nAddCol)].ToString();
                    }
                    if ((cRow.Length > 0) && (cCol.Length > 0))
                        cReturn = GetExcelCell(cCol + cRow);
                }
            }
            if (cVal == MandatoryQualifier.Yes)
            {
                if (cReturn.Trim() == "")
                {
                    throw new Exception("Unable to get details for " + cPattern);
                }
            }
            return cReturn;
        }

        public string GetCellDataContains(int nStart, int nEnd, string cColumn, string cPattern, int nAddRow, int nAddCol)
        {
            string cCell = "";
            if (nEnd <= nStart) nEnd = nStart + 1;

            for (int i = nStart; i <= nEnd; i++)
            {
                if (convert.ToString(GetExcelCell(cColumn + i)).Trim().Contains(cPattern.Trim()))
                {
                    cCell = cColumn + i;
                    break;
                }
            }

            string cExcel = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string cReturn = cCell;
            string cCol = ""; string cRow = "";

            if (!string.IsNullOrEmpty(cCell))
            {
                foreach (char ch in cCell)
                {
                    if (cExcel.Contains(ch.ToString()))
                        cCol += ch;
                    else cRow += ch;
                }
                cRow = (convert.ToInt(cRow) + nAddRow).ToString();
                if (cExcel.IndexOf(cCol) < 0)
                {
                    if (cCol.Length > 2)
                    {
                        throw new Exception("Excel Header Index is too long " + cCol);
                    }
                    string fChar = cExcel[cExcel.IndexOf(cCol[0])].ToString();
                    int fCharIndex = cExcel.IndexOf(fChar);
                    if (cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(nAddCol) > 26)
                    {
                        if ((fCharIndex + 1) > 26)
                        {
                            throw new Exception("Excel Header Index is max length ");
                        }
                        else
                        {
                            cCol = cExcel[(fCharIndex + 1)].ToString() + cExcel[cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(nAddCol) - 26].ToString();
                        }
                    }
                    else
                    {
                        cCol = fChar + cExcel[cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(nAddCol)].ToString();
                    }
                }
                else
                {
                    cCol = cExcel[cExcel.IndexOf(cCol) + convert.ToInt(nAddCol)].ToString();
                }

                if ((cRow.Length > 0) && (cCol.Length > 0))
                    cReturn = GetExcelCell(cCol + cRow);
            }
            return cReturn;
        }

        public string GetCellDynamicData(int nStart, int nEnd, string cColumn, string cPattern, int nAddRow, int nAddCol, ref int RefRow)
        {
            string cCell = "";
            if (nEnd <= nStart) nEnd = nStart + 1;

            for (int i = nStart; i <= nEnd; i++)
            {
                if (convert.ToString(GetExcelCell(cColumn + i)).Trim() == cPattern.Trim())
                {
                    cCell = cColumn + i;
                    RefRow = i;
                    break;
                }
            }

            string cExcel = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string cReturn = cCell;
            string cCol = ""; string cRow = "";

            if (!string.IsNullOrEmpty(cCell))
            {
                foreach (char ch in cCell)
                {
                    if (cExcel.Contains(ch.ToString()))
                        cCol += ch;
                    else cRow += ch;
                }
                cRow = (convert.ToInt(cRow) + nAddRow).ToString();
                if (cExcel.IndexOf(cCol) < 0)
                {
                    if (cCol.Length > 2)
                    {
                        throw new Exception("Excel Header Index is too long " + cCol);
                    }
                    string fChar = cExcel[cExcel.IndexOf(cCol[0])].ToString();
                    int fCharIndex = cExcel.IndexOf(fChar);
                    if (cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(nAddCol) > 26)
                    {
                        if ((fCharIndex + 1) > 26)
                        {
                            throw new Exception("Excel Header Index is max length ");
                        }
                        else
                        {
                            cCol = cExcel[(fCharIndex + 1)].ToString() + cExcel[cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(nAddCol) - 26].ToString();
                        }
                    }
                    else
                    {
                        cCol = fChar + cExcel[cExcel.IndexOf(cCol[cCol.Length - 1]) + convert.ToInt(nAddCol)].ToString();
                    }
                }
                else
                {
                    cCol = cExcel[cExcel.IndexOf(cCol) + convert.ToInt(nAddCol)].ToString();
                }
                if ((cRow.Length > 0) && (cCol.Length > 0))
                    cReturn = GetExcelCell(cCol + cRow);
            }
            return cReturn;
        }

    }

    public class convert
    {
        public static long ToLong(object valueFromDb)
        {
            try
            {
                return valueFromDb != DBNull.Value ? Convert.ToInt64(valueFromDb) : 0;
            }
            catch { return 0; }
        }

        public static double ToFloat(object valueFromDb)
        {
            try
            {
                return valueFromDb != DBNull.Value ? Convert.ToDouble(valueFromDb) : 0.0F;
            }
            catch { return 0.0F; }
        }

        public static Boolean ToBoolean(object valueFromDb)
        {
            try
            {
                if (valueFromDb.ToString() == "1") return true;
                else return false;
            }
            catch { return false; }
        }

        public static int ToInt(object valueFromDb)
        {
            try
            {
                if (valueFromDb.ToString().ToUpper() == "TRUE") return 1;
                return valueFromDb != DBNull.Value ? Convert.ToInt32(valueFromDb) : 0;
            }
            catch { return 0; }
        }

        public static string ToDBValue(int nValue)
        {
            try
            {
                return nValue > 0 ? nValue.ToString() : "NULL";
            }
            catch { return "NULL"; }
        }

        public static object ToDBValue(string cValue)
        {
            try
            {
                if (cValue.Trim() == "") return DBNull.Value;
                else if (cValue.Trim() == "0") return DBNull.Value;
                else return cValue;
            }
            catch { return DBNull.Value; }
        }

        public static string ToQuote(string Value)
        {
            if (Value == null) Value = "";
            Value = Value.Replace("'", "''");
            return ("'" + Value + "'");
        }

        public static DateTime ToDateTime(object valueFromDb)
        {
            try
            {
                if ((valueFromDb.ToString() != null) && (valueFromDb.ToString().Length > 0))
                    return Convert.ToDateTime(valueFromDb);
                else return DateTime.MinValue;
            }
            catch { return DateTime.MinValue; }
        }

        public static DateTime ToDateTime(object valueFromDb, string DateFormat)
        {
            try
            {
                if (DateFormat != string.Empty)
                {
                    if ((valueFromDb.ToString() != null) && (valueFromDb.ToString().Length > 0))
                        return DateTime.ParseExact(valueFromDb.ToString(), DateFormat, null);
                    else return DateTime.MinValue;
                }
                else
                {
                    return ToDateTime(valueFromDb);
                }
            }
            catch(Exception ex) {
                try
                {
                    DateTime date = DateTime.ParseExact(valueFromDb.ToString(), DateFormat, CultureInfo.InvariantCulture);
                    return date;
                }
                catch (Exception e)
                {
                    return DateTime.MinValue; 
                }
            }
        }

        public static NameValueCollection ToNameValueCollection(DataSet ds)
        {
            DataTable dt = ds.Tables[0];
            NameValueCollection nvcReturn = new NameValueCollection();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j <= ds.Tables[0].Rows.Count - 1; j++)
                {
                    DataRow dr = ds.Tables[0].Rows[j];

                    for (int i = 0; i <= dt.Columns.Count - 1; i++)
                    {
                        nvcReturn.Add(dt.Columns[i].ColumnName.ToString(), dr[dt.Columns[i].ColumnName.ToString()].ToString());
                    }
                }
            }

            return nvcReturn;
        }

        public static Dictionary<string, string> ToDictionary(DataSet ds)
        {
            DataTable dt = ds.Tables[0];
            Dictionary<string, string> dicReturn = new Dictionary<string, string>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    DataRow dr = ds.Tables[0].Rows[j];

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dicReturn.Add(dt.Columns[i].ColumnName.ToString(), dr[dt.Columns[i].ColumnName.ToString()].ToString());
                    }
                }
            }
            return dicReturn;
        }

        public static string ToFileName(string cValue)
        {
            try
            {
                cValue = cValue.Replace(" ", "_");
                cValue = cValue.Replace("/", "_");
                cValue = cValue.Replace("?", "_");
                cValue = cValue.Replace("\\", "_");
                cValue = cValue.Replace(":", "_");
                cValue = cValue.Replace("*", "_");
                cValue = cValue.Replace("<", "_");
                cValue = cValue.Replace(">", "_");
                cValue = cValue.Replace("|", "_");
                cValue = cValue.Replace("\"", "_");
                cValue = cValue.Replace("'", "");
                cValue = cValue.Replace(",", "_");
                return cValue;
            }
            catch { return cValue; }
        }

        public static string ToString(object valueFromDb)
        {
            try
            {
                return valueFromDb != DBNull.Value ? Convert.ToString(valueFromDb) : "";
            }
            catch { return ""; }
        }

        public static string ToAlphaNumeric(string cInput)
        {
            string cReturn = "";

            for (int i = 0; i < cInput.Length; i++)
            {
                if (GlobalConstants.cAlphaNumeric.IndexOf(cInput[i]) > 0)
                    cReturn += cInput[i];
            }
            return cReturn;
        }

        public static string ToNumericOnly(string cInput)
        {
            string cNUM = "0123456789", cReturn = "";

            for (int i = 0; i < cInput.Length; i++)
            {
                if (cNUM.IndexOf(cInput[i]) != -1)
                    cReturn += cInput[i];
            }
            return cReturn;
        }

        public static string ToXMLString(string cInput)
        {
            cInput = cInput.Replace("<", "&lt;");
            cInput = cInput.Replace(">", "&gt;");
            cInput = cInput.Replace(" & ", "&amp;");
            cInput = cInput.Replace("\"", "&quot;");
            cInput = cInput.Replace("\'", "&apos;");

            return cInput;
        }

        public static string ToEngString(string cInput)
        {
            string cReturn = "";

            for (int i = 0; i < cInput.Length; i++)
            {
                if (GlobalConstants.cEnglishChar.IndexOf(cInput[i]) >= 0)
                    cReturn += cInput[i];
                else if ((cInput[i] == '\n') || (cInput[i] == '\t') || (cInput[i] == '\r'))
                    cReturn += cInput[i];
            }

            return cReturn;
        }

        public static string ToPositive(object valueFromDb)
        {
            try
            {
                if (valueFromDb.ToString().ToUpper() == "TRUE") return "1";
                else if (valueFromDb == null) return "-";
                else if (valueFromDb == DBNull.Value) return "-";
                else if (convert.ToInt(valueFromDb) > 0) return convert.ToInt(valueFromDb).ToString();
                else return "-";
            }
            catch { return "-"; }
        }
    }

    public class GlobalConstants
    {
        public static string cAlphaNumeric = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        public static string cPWDecrypted = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        public static string cPWEncrypted = ".-,+*)(~&%$#}!@?>=<;:98765^]\\[ZYXWV_";
        public static string[] MonthsString ={"January", "February", "March", "April",
                                                 "May", "June", "July","August",
                                                 "September","October","November","December"};
        public static string cEnglishChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890.-,+*)(~&%$#}{!@?>=<;:98765^]\\[\"/|_ ";

        public const int dbConstant = 10000000;
    }

}
