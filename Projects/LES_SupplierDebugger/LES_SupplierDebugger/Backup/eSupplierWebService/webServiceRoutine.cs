using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System;
using System.IO;

namespace eSupplierWebService
{
    public class webServiceRoutine
    {
        public SqlConnection GetdbConnection(string _connection)
        {
            string connString = ConfigurationManager.AppSettings[_connection];
            SqlConnection conn = new SqlConnection(connString);
            return conn;
        }

        public DataSet GetQueryDataset(string cSQL, Dictionary<string, string> slParams, string _connection)
        {
            try
            {
                using (SqlConnection conn = GetdbConnection(_connection))
                {
                    SqlCommand cmd = new SqlCommand(cSQL, conn);
                    conn.Open();

                    foreach (KeyValuePair<string, string> Pair in slParams)
                    {
                        cmd.Parameters.Add(new SqlParameter(Pair.Key, Pair.Value));
                    }

                    SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                    DataSet dataSet = new DataSet();
                    sqlDA.Fill(dataSet);

                    cmd.Dispose();
                    return dataSet;
                }                
            }
            catch { throw; }
            finally
            {
                GC.Collect();
            }
        }

        public string SaveFile(string sPath, byte[] bFile, string sFileName, string sLicense)
        {
            string sReturn = "";
            try
            {
                if (!Directory.Exists(sPath))
                {
                    Directory.CreateDirectory(sPath);
                }
                if (File.Exists(sPath + "\\" + sFileName))
                {
                    sFileName = Path.GetFileNameWithoutExtension(sFileName) + "_" + DateTime.Now.ToString("ddMMyyHHmmssff") + Path.GetExtension(sFileName);
                }
                FileStream fStream = new FileStream(sPath + "\\" + sFileName, FileMode.Create);//,FileAccess.Write);
                //AFile = commRoute.readFileStream(AttachPath, AFileName);
                fStream.Write(bFile, 0, bFile.Length);
                fStream.Flush();
                fStream.Dispose();
                fStream.Close();

                if (File.Exists(sPath + "\\" + sFileName))
                {
                    //eSupplierDataMain.DataLog.DBAuditLog(cAudit, cLogType, cModule, cFile, cREFNo);
                }
            }
            catch (Exception e)
            {
                sReturn = e.GetBaseException().ToString();
            }
            return sReturn;
        }
    }
}