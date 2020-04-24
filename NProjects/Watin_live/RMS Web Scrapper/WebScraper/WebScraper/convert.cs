using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraper
{
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

        public static Single ToFloat(object valueFromDb)
        {
            try
            {
                return valueFromDb != DBNull.Value ? Convert.ToSingle(valueFromDb) : 0;
            }
            catch { return 0; }
        }

        public static double ToDouble(object valueFromDb)
        {
            try
            {
                return valueFromDb != DBNull.Value ? Convert.ToDouble(valueFromDb) : 0;
            }
            catch { return 0; }
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

        public static string ToString(object valueFromDb)
        {
            try
            {
                return valueFromDb.ToString();
            }
            catch { return ""; }
        }

        public static string ToDBValue(long nValue)
        {
            try
            {
                return nValue > 0 ? nValue.ToString() : "NULL";
            }
            catch { return "NULL"; }
        }

        public static DateTime ToDateTime(object valueFromDb)
        {
            try
            {
                if ((valueFromDb.ToString() != null) && (valueFromDb.ToString().Length > 0))
                    return Convert.ToDateTime(valueFromDb.ToString());
                else return DateTime.MinValue;
            }
            catch { return DateTime.MinValue; }
        }

        public static DateTime ToDateTime(object valueFromDb, string parseFormat)
        {
            try
            {
                DateTime dt;
                DateTime.TryParseExact(Convert.ToString(valueFromDb).Trim(), parseFormat, null, System.Globalization.DateTimeStyles.None, out dt);

                if ((dt.ToString() != null) && (dt.ToString().Length > 0))
                    return Convert.ToDateTime(dt.ToString());
                else return DateTime.MinValue;
            }
            catch { return DateTime.MinValue; }
        }

        public static string ToFileName(object frmValue)
        {
            try
            {
                string cFileName = frmValue.ToString().Replace(":", "_");
                cFileName = cFileName.Replace("-", "_");
                cFileName = cFileName.Replace(" ", "_");
                cFileName = cFileName.Replace("\\", "_");
                cFileName = cFileName.Replace("/", "_");
                cFileName = cFileName.Replace("*", "_");
                cFileName = cFileName.Replace("?", "_");
                cFileName = cFileName.Replace("\"", "_");
                cFileName = cFileName.Replace("<", "_");
                cFileName = cFileName.Replace(">", "_");
                cFileName = cFileName.Replace("!", "_");
                return cFileName.ToString();
            }
            catch { return DateTime.Now.ToFileTime().ToString(); }
        }

        public static string ToQuote(string Value)
        {
            if (Value == null) Value = "";
            Value = Value.Replace("'", "''");
            return ("'" + Value + "'");
        }
    }
}