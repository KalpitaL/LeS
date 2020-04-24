namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;

    public partial class SmBuyerSupplierLink : IDisposable
    { 
    private DataAccess _dataAccess;

        public SmBuyerSupplierLink()
        {
            this._dataAccess = new DataAccess();
        }

        public SmBuyerSupplierLink(DataAccess _dataAccess)
        {
            // TODO: Complete member initialization
            this._dataAccess = _dataAccess;
        }

        public virtual System.Data.DataSet Select_SM_BUYER_SUPPLIER_LINKs_By_BUYERID(System.Nullable<int> BUYERID)
        {
            this._dataAccess.CreateProcedureCommand("sp_Select_SM_BUYER_SUPPLIER_LINKs_By_BUYERID");
            this._dataAccess.AddParameter("BUYERID", BUYERID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet Select_SM_BUYER_SUPPLIER_LINKs_By_SUPPLIERID(System.Nullable<int> SUPPLIERID)
        {
            this._dataAccess.CreateProcedureCommand("sp_Select_SM_BUYER_SUPPLIER_LINKs_By_SUPPLIERID");
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_LINK_Select_All()
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_LINK_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet Load_By_BuyerSupplierID(System.Nullable<int> BuyerID, System.Nullable<int> SupplierID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_LINK_Select_BuyerSupplierID");
            this._dataAccess.AddParameter("BUYERID", BuyerID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERID", SupplierID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual DataSet Load_By_BuyerSupplierGroup(System.Nullable<int> BuyerID, System.Nullable<int> SupplierID, System.Nullable<int> GroupID)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SM_BUYER_SUPPLIER_LINK WHERE BUYERID=" + BuyerID + " AND SUPPLIERID =" + SupplierID + " AND GROUP_ID= " + GroupID);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual DataSet Load_By_Group(System.Nullable<int> GroupID)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SM_BUYER_SUPPLIER_LINK WHERE GROUP_ID= " + GroupID);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual DataSet Load_By_BuyerSupplierFormat(System.Nullable<int> BuyerID, System.Nullable<int> SupplierID, string BuyerFormat, string VendorFormat)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SM_BUYER_SUPPLIER_LINK WHERE BUYERID=" + BuyerID + " AND SUPPLIERID =" + SupplierID + " AND (BUYER_FORMAT LIKE '%" + BuyerFormat + "%' OR VENDOR_FORMAT LIKE '%" + VendorFormat + "%')");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        public virtual DataSet Load_By_BuyerSupplierFormat(System.Nullable<int> BuyerID, System.Nullable<int> SupplierID)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SM_BUYER_SUPPLIER_LINK WHERE BUYERID=" + BuyerID + " AND SUPPLIERID =" + SupplierID + " AND (BUYER_FORMAT IS NULL OR VENDOR_FORMAT IS NULL)");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet Load_By_BuyerSupplierLinkCode(string BuyerLinkCode, string SupplierLinkCode) 
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_LINK_BYR_SUPP_LINKCODE");
            this._dataAccess.AddParameter("BUYERCode", BuyerLinkCode, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERCode", SupplierLinkCode, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_LINK_Select_One(System.Nullable<int> LINKID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_LINK_Select_One");
            this._dataAccess.AddParameter("LINKID", LINKID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Nullable<int> SM_BUYER_SUPPLIER_LINK_Insert(
                    System.Nullable<int> BUYERID,
                    System.Nullable<int> SUPPLIERID,
                    string BUYER_LINK_CODE,
                    string VENDOR_LINK_CODE,
                    string BUYER_FORMAT,
                    string VENDOR_FORMAT,
                    string SUPP_SENDER_CODE,
                    string SUPP_RECEIVER_CODE,
                    string BYR_SENDER_CODE,
                    string BYR_RECEIVER_CODE,
                    string BUYER_EXPORT_FORMAT,
                    string SUPPLIER_EXPORT_FORMAT,
                    string BUYER_MAPPING,
                    string SUPPLIER_MAPPING,
                    string Company,
                    System.Nullable<short> IMPORT_RFQ,
                    System.Nullable<short> EXPORT_RFQ,
                    System.Nullable<short> EXPORT_RFQ_ACK,
                    System.Nullable<short> IMPORT_QUOTE,
                    System.Nullable<short> EXPORT_QUOTE,
                    System.Nullable<short> IMPORT_PO,
                    System.Nullable<short> EXPORT_PO,
                    System.Nullable<short> EXPORT_PO_ACK,
                    System.Nullable<short> EXPORT_POC,
                    string EXPORT_PATH,
                    string IMPORT_PATH,
                    System.Nullable<short> NOTIFY_BUYER,
                    System.Nullable<short> NOTIFY_SUPPLR,
                    System.Nullable<float> DEFAULT_PRICE,
                    System.Nullable<short> IS_ACTIVE,
                    string REPLY_EMAIL,
                    string BUYER_CONTACT,
                    string SUPPLIER_CONTACT,
                    string BUYER_EMAIL,
                    string SUPPLIER_EMAIL,
                    string CC_EMAIL,
                    string BCC_EMAIL,
                    string MAIL_SUBJECT,
                    string ERR_NOTIFY_EMAIL,
                    System.Nullable<int> GROUP_ID,
                    string UPLOAD_FILE_TYPE,
                    string SUPP_IMPORT_PATH,
                   string SUPP_EXPORT_PATH,
                   string SUP_WEB_SERVICE_URL, System.Nullable<short> IMPORT_POC)  
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_LINK_Insert_LesMonitor_1");
            this._dataAccess.AddParameter("LINKID", 0, ParameterDirection.Output);
            this._dataAccess.AddParameter("BUYERID", BUYERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_LINK_CODE", BUYER_LINK_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_LINK_CODE", VENDOR_LINK_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_FORMAT", BUYER_FORMAT, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_FORMAT", VENDOR_FORMAT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_SENDER_CODE", SUPP_SENDER_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_RECEIVER_CODE", SUPP_RECEIVER_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("BYR_SENDER_CODE", BYR_SENDER_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("BYR_RECEIVER_CODE", BYR_RECEIVER_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_EXPORT_FORMAT", BUYER_EXPORT_FORMAT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_EXPORT_FORMAT", SUPPLIER_EXPORT_FORMAT, ParameterDirection.Input); // Added on 16-APR-15
            this._dataAccess.AddParameter("BUYER_MAPPING", BUYER_MAPPING, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_MAPPING", SUPPLIER_MAPPING, ParameterDirection.Input);
            this._dataAccess.AddParameter("Company", Company, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_RFQ", IMPORT_RFQ, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_RFQ", EXPORT_RFQ, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_RFQ_ACK", EXPORT_RFQ_ACK, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_QUOTE", IMPORT_QUOTE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_QUOTE", EXPORT_QUOTE, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_PO", IMPORT_PO, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_PO", EXPORT_PO, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_PO_ACK", EXPORT_PO_ACK, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_POC", EXPORT_POC, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_PATH", EXPORT_PATH, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_PATH", IMPORT_PATH, ParameterDirection.Input);
            this._dataAccess.AddParameter("NOTIFY_BUYER", NOTIFY_BUYER, ParameterDirection.Input);
            this._dataAccess.AddParameter("NOTIFY_SUPPLR", NOTIFY_SUPPLR, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEFAULT_PRICE", DEFAULT_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("IS_ACTIVE", IS_ACTIVE, ParameterDirection.Input);
            this._dataAccess.AddParameter("REPLY_EMAIL", REPLY_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_CONTACT", BUYER_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_CONTACT", SUPPLIER_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_EMAIL", BUYER_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_EMAIL", SUPPLIER_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CC_EMAIL", CC_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("BCC_EMAIL", BCC_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_SUBJECT", MAIL_SUBJECT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ERR_NOTIFY_EMAIL", ERR_NOTIFY_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUP_ID", GROUP_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPLOAD_FILE_TYPE", UPLOAD_FILE_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_IMPORT_PATH", SUPP_IMPORT_PATH, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_EXPORT_PATH", SUPP_EXPORT_PATH, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUP_WEB_SERVICE_URL", SUP_WEB_SERVICE_URL, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_POC", IMPORT_POC, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            value = convert.ToInt(this._dataAccess.Command.Parameters["LINKID"].Value);
            return value;
        }

        public virtual System.Nullable<int> SM_BUYER_SUPPLIER_LINK_Delete(System.Nullable<int> LINKID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_LINK_Delete");
            this._dataAccess.AddParameter("LINKID", LINKID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_BUYER_SUPPLIER_LINK_Update(
                    System.Nullable<int> LINKID,
                    System.Nullable<int> BUYERID,
                    System.Nullable<int> SUPPLIERID,
                    string BUYER_LINK_CODE,
                    string VENDOR_LINK_CODE,
                    string BUYER_FORMAT,
                    string VENDOR_FORMAT,
                    string SUPP_SENDER_CODE,
                    string SUPP_RECEIVER_CODE,
                    string BYR_SENDER_CODE,
                    string BYR_RECEIVER_CODE,
                    string BUYER_EXPORT_FORMAT,
                    string SUPPLIER_EXPORT_FORMAT,
                    string BUYER_MAPPING,
                    string SUPPLIER_MAPPING,
                    string Company,
                    System.Nullable<short> IMPORT_RFQ,
                    System.Nullable<short> EXPORT_RFQ,
                    System.Nullable<short> EXPORT_RFQ_ACK,
                    System.Nullable<short> IMPORT_QUOTE,
                    System.Nullable<short> EXPORT_QUOTE,
                    System.Nullable<short> IMPORT_PO,
                    System.Nullable<short> EXPORT_PO,
                    System.Nullable<short> EXPORT_PO_ACK,
                    System.Nullable<short> EXPORT_POC,
                    string EXPORT_PATH,
                    string IMPORT_PATH,
                    System.Nullable<short> NOTIFY_BUYER,
                    System.Nullable<short> NOTIFY_SUPPLR,
                    System.Nullable<float> DEFAULT_PRICE,
                    System.Nullable<short> IS_ACTIVE,
                    string REPLY_EMAIL,
                    string BUYER_CONTACT,
                    string SUPPLIER_CONTACT,
                    string BUYER_EMAIL,
                    string SUPPLIER_EMAIL,
                    string CC_EMAIL,
                    string BCC_EMAIL,
                    string MAIL_SUBJECT,
                    string ERR_NOTIFY_EMAIL,
                    System.Nullable<int> GROUP_ID,
                    string UPLOAD_FILE_TYPE,
                    string SUPP_IMPORT_PATH,
                     string SUPP_EXPORT_PATH, string SUP_WEB_SERVICE_URL, System.Nullable<short> IMPORT_POC) 
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_LINK_Update_LesMonitor_1");
            this._dataAccess.AddParameter("LINKID", LINKID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYERID", BUYERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_LINK_CODE", BUYER_LINK_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_LINK_CODE", VENDOR_LINK_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_FORMAT", BUYER_FORMAT, ParameterDirection.Input);
            this._dataAccess.AddParameter("VENDOR_FORMAT", VENDOR_FORMAT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_SENDER_CODE", SUPP_SENDER_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_RECEIVER_CODE", SUPP_RECEIVER_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("BYR_SENDER_CODE", BYR_SENDER_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("BYR_RECEIVER_CODE", BYR_RECEIVER_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_EXPORT_FORMAT", BUYER_EXPORT_FORMAT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_EXPORT_FORMAT", SUPPLIER_EXPORT_FORMAT, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_MAPPING", BUYER_MAPPING, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_MAPPING", SUPPLIER_MAPPING, ParameterDirection.Input);
            this._dataAccess.AddParameter("Company", Company, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_RFQ", IMPORT_RFQ, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_RFQ", EXPORT_RFQ, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_RFQ_ACK", EXPORT_RFQ_ACK, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_QUOTE", IMPORT_QUOTE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_QUOTE", EXPORT_QUOTE, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_PO", IMPORT_PO, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_PO", EXPORT_PO, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_PO_ACK", EXPORT_PO_ACK, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_POC", EXPORT_POC, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORT_PATH", EXPORT_PATH, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_PATH", IMPORT_PATH, ParameterDirection.Input);
            this._dataAccess.AddParameter("NOTIFY_BUYER", NOTIFY_BUYER, ParameterDirection.Input);
            this._dataAccess.AddParameter("NOTIFY_SUPPLR", NOTIFY_SUPPLR, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEFAULT_PRICE", DEFAULT_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("IS_ACTIVE", IS_ACTIVE, ParameterDirection.Input);
            this._dataAccess.AddParameter("REPLY_EMAIL", REPLY_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_CONTACT", BUYER_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_CONTACT", SUPPLIER_CONTACT, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_EMAIL", BUYER_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_EMAIL", SUPPLIER_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CC_EMAIL", CC_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("BCC_EMAIL", BCC_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAIL_SUBJECT", MAIL_SUBJECT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ERR_NOTIFY_EMAIL", ERR_NOTIFY_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUP_ID", GROUP_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPLOAD_FILE_TYPE", UPLOAD_FILE_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_IMPORT_PATH", SUPP_IMPORT_PATH, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPP_EXPORT_PATH", SUPP_EXPORT_PATH, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUP_WEB_SERVICE_URL", SUP_WEB_SERVICE_URL, ParameterDirection.Input);
            this._dataAccess.AddParameter("IMPORT_POC", IMPORT_POC, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual void Dispose()
        {
            if ((this._dataAccess != null))
            {
                this._dataAccess.Dispose();
            }
        }

        //ADDED BY KALPITA ON 23/02/2018
        public virtual DataSet LoadBuyers_by_Format(string FORMAT)
        {
            this._dataAccess.CreateSQLCommand("SELECT BUYERID,BUYER_NAME FROM SMV_BUYER_SUPPLIER_LINK WHERE BUYER_FORMAT LIKE '%" + FORMAT + "%'");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
    }
}
