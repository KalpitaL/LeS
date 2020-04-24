namespace MetroLesMonitor
{
    public partial class GlobalTools
    {
        //public static string cValidKeyChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_()[]";
        //public static string cPWDecrypted = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        //public static string cPWEncrypted = ".-,+*)(~&%$#}!@?>=<;:98765^]\\[ZYXWV_";

        public static string cPWDecrypted = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@#$%^&*()[]-_~!+:,'{}";
        public static string cPWEncrypted = @".-,+*)(~&%$#}!@?>=<;:98765^]\[ZYXWV_ABCDEFGHIJKLMNOPQRSTU";

        //public static bool IsValidKeyStr(string KeyString)
        //{
        //    bool _result = true;
        //    _result = (KeyString.Length > 0);
        //    if (_result)
        //    {
        //        for (int i = 0; i < KeyString.Length; i++)
        //        {
        //            _result = cValidKeyChars.Contains(KeyString.ToUpper()[i].ToString());
        //            if (!_result)
        //            {
        //                return _result;
        //            }
        //        }
        //    }
        //    return _result;
        //}

        public static bool IsSafeDataSet(System.Data.DataSet ds)
        {
            return GlobalTools.IsSafeDataSet(ds, 1);
        }

        public static bool IsSafeDataSet(System.Data.DataSet ds, int nbTables)
        {
            if ((((ds != null)
                        && (ds.Tables != null))
                        && (ds.Tables.Count == nbTables)))
            {
                for (int i = 0; (i < nbTables); i = (i + 1))
                {
                    if ((ds.Tables[i].Rows == null))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        //public static string EncodePassword(string Password)
        //{
        //    string cOut = ""; int i;
        //    try
        //    {
        //        for (i = 0; i < Password.Length; i++)
        //        {
        //            cOut = cOut + cPWEncrypted[cPWDecrypted.IndexOf(char.ToUpper(Password[i]))];
        //        }
        //        return cOut;
        //    }
        //    catch { return ""; }
        //}

        //public static string DecodePassword(string Password)
        //{
        //    string cOut = ""; int i;
        //    try
        //    {
        //        for (i = 0; i < Password.Length; i++)
        //        {
        //            cOut = cOut + cPWDecrypted[cPWEncrypted.IndexOf(char.ToUpper(Password[i]))];
        //        }
        //        return cOut;
        //    }
        //    catch { return ""; }
        //}

        public static string EncodePassword(string cPWD)
        {
            string cOut = ""; int i;
            try
            {
                for (i = 0; i < cPWD.Length; i++)
                {
                    cOut = cOut + cPWEncrypted[cPWDecrypted.IndexOf(char.ToUpper(cPWD[i]))];
                }
                return cOut;
            }
            catch { return ""; }
        }

        public static string DecodePassword(string cPWD)
        {
            string cOut = ""; int i;
            try
            {
                for (i = 0; i < cPWD.Length; i++)
                {
                    cOut = cOut + cPWDecrypted[cPWEncrypted.IndexOf(char.ToUpper(cPWD[i]))];
                }
                return cOut;
            }
            catch { return ""; }
        }
    }
}