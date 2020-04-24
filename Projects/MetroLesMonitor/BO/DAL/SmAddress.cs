namespace MetroLesMonitor.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;

    public partial class SmAddress : IDisposable
    {
        private DataAccess _dataAccess;

        public SmAddress()
        {
            this._dataAccess = new DataAccess();
        }

        public SmAddress(DataAccess _dataAccess)
        {
            // TODO: Complete member initialization
          //  this._dataAccess = _dataAccess;
            this._dataAccess = (_dataAccess != null) ? _dataAccess : new DataAccess();
        }

        public virtual System.Data.DataSet SM_ADDRESS_Select_All()
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_ADDRESS_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_ADDRESS_Select_One(System.Nullable<int> ADDRESSID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_ADDRESS_Select_One");
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_ADDRESS_Select_One_ByAddressCode(string ADDRESSCODE)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SM_ADDRESS WHERE ADDR_CODE = @ADDR_CODE");
            this._dataAccess.AddParameter("ADDR_CODE", ADDRESSCODE, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual void SM_ADDRESS_Insert(
                    System.Nullable<int> ADDRESSID,
                    string ADDR_CODE,
                    string ADDR_NAME,
                    string CONTACT_PERSON,
                    string ADDRESS1,
                    string ADDRESS2,
                    string ADDRESS3,
                    string ADDRESS4,
                    string ADDR_CITY,
                    string ADDR_COUNTRY,
                    string ADDR_ZIPCODE,
                    string ADDR_PHONE1,
                    string ADDR_PHONE2,
                    string ADDR_FAX,
                    string ADDR_TELEX,
                    string ADDR_EMAIL,
                    string ADDR_MOBILEPHONE,
                    string ADDR_TYPE,
                    System.Nullable<System.DateTime> CREATED_DATE,
                   // System.Nullable<System.DateTime> UPDATE_DATE,
                    System.Nullable<int> ADDR_CURRENCYID,
                    string eBizConnectorID,
                    string eBizCode,
                    System.Nullable<short> Active,
                    string ADDR_MTS_CODE,
                    string ADDR_MTML_CODE,
                    string ADDR_INBOX,
                    string ADDR_OUTBOX,
                    System.Nullable<short> eSupplier,
                    System.Nullable<short> eInvoice,
                    System.Nullable<short> ePurchase,
                    System.Nullable<int> GROUP_ID,
                    System.Nullable<int> XML_ADDR_NO,
                    string BUYER_LINK,
                    string WEB_LINK,
                    string ADDR_COUNTRY_CODE ,System.Nullable<int> IsLeSConnect)
        {
            //this._dataAccess.CreateProcedureCommand("sp_SM_ADDRESS_Insert");
            this._dataAccess.CreateProcedureCommand("sp_SM_ADDRESS_Insert_LesMonitor_New");
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_CODE", ADDR_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_NAME", ADDR_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONTACT_PERSON", CONTACT_PERSON, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESS1", ADDRESS1, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESS2", ADDRESS2, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESS3", ADDRESS3, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESS4", ADDRESS4, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_CITY", ADDR_CITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_COUNTRY", ADDR_COUNTRY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_ZIPCODE", ADDR_ZIPCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_PHONE1", ADDR_PHONE1, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_PHONE2", ADDR_PHONE2, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_FAX", ADDR_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_TELEX", ADDR_TELEX, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_EMAIL", ADDR_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_MOBILEPHONE", ADDR_MOBILEPHONE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_TYPE", ADDR_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            //this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_CURRENCYID", ADDR_CURRENCYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("eBizConnectorID", eBizConnectorID, ParameterDirection.Input);
            this._dataAccess.AddParameter("eBizCode", eBizCode, ParameterDirection.Input);
            this._dataAccess.AddParameter("Active", Active, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_MTS_CODE", ADDR_MTS_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_MTML_CODE", ADDR_MTML_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_INBOX", ADDR_INBOX, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_OUTBOX", ADDR_OUTBOX, ParameterDirection.Input);
            this._dataAccess.AddParameter("eSupplier", eSupplier, ParameterDirection.Input);
            this._dataAccess.AddParameter("eInvoice", eInvoice, ParameterDirection.Input);
            this._dataAccess.AddParameter("ePurchase", ePurchase, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUP_ID", GROUP_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("XML_ADDR_NO", XML_ADDR_NO, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_LINK", BUYER_LINK, ParameterDirection.Input);
            this._dataAccess.AddParameter("WEB_LINK", WEB_LINK, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_COUNTRY_CODE", ADDR_COUNTRY_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("IsLeSConnect", IsLeSConnect, ParameterDirection.Input);
            this._dataAccess.ExecuteNonQuery();
        }

        public virtual System.Nullable<int> SM_ADDRESS_Delete(System.Nullable<int> ADDRESSID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_ADDRESS_Delete");
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_ADDRESS_Update(
                    System.Nullable<int> ADDRESSID,
                    string ADDR_CODE,
                    string ADDR_NAME,
                    string CONTACT_PERSON,
                    string ADDRESS1,
                    string ADDRESS2,
                    string ADDRESS3,
                    string ADDRESS4,
                    string ADDR_CITY,
                    string ADDR_COUNTRY,
                    string ADDR_ZIPCODE,
                    string ADDR_PHONE1,
                    string ADDR_PHONE2,
                    string ADDR_FAX,
                    string ADDR_TELEX,
                    string ADDR_EMAIL,
                    string ADDR_MOBILEPHONE,
                    string ADDR_TYPE,
                    System.Nullable<System.DateTime> CREATED_DATE,
                    //System.Nullable<System.DateTime> UPDATE_DATE,
                    System.Nullable<int> ADDR_CURRENCYID,
                    string eBizConnectorID,
                    string eBizCode,
                    System.Nullable<short> Active,
                    string ADDR_MTS_CODE,
                    string ADDR_MTML_CODE,
                    string ADDR_INBOX,
                    string ADDR_OUTBOX,
                    System.Nullable<short> eSupplier,
                    System.Nullable<short> eInvoice,
                    System.Nullable<short> ePurchase,
                    System.Nullable<int> GROUP_ID,
                    System.Nullable<int> XML_ADDR_NO,
                    string BUYER_LINK,
                    string WEB_LINK,
                    string ADDR_COUNTRY_CODE, System.Nullable<int> IsLeSConnect)
        {
            //this._dataAccess.CreateProcedureCommand("sp_SM_ADDRESS_Update");
            this._dataAccess.CreateProcedureCommand("sp_SM_ADDRESS_Update_LesMonitor_New");
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_CODE", ADDR_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_NAME", ADDR_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONTACT_PERSON", CONTACT_PERSON, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESS1", ADDRESS1, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESS2", ADDRESS2, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESS3", ADDRESS3, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESS4", ADDRESS4, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_CITY", ADDR_CITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_COUNTRY", ADDR_COUNTRY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_ZIPCODE", ADDR_ZIPCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_PHONE1", ADDR_PHONE1, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_PHONE2", ADDR_PHONE2, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_FAX", ADDR_FAX, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_TELEX", ADDR_TELEX, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_EMAIL", ADDR_EMAIL, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_MOBILEPHONE", ADDR_MOBILEPHONE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_TYPE", ADDR_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            //this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_CURRENCYID", ADDR_CURRENCYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("eBizConnectorID", eBizConnectorID, ParameterDirection.Input);
            this._dataAccess.AddParameter("eBizCode", eBizCode, ParameterDirection.Input);
            this._dataAccess.AddParameter("Active", Active, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_MTS_CODE", ADDR_MTS_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_MTML_CODE", ADDR_MTML_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_INBOX", ADDR_INBOX, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_OUTBOX", ADDR_OUTBOX, ParameterDirection.Input);
            this._dataAccess.AddParameter("eSupplier", eSupplier, ParameterDirection.Input);
            this._dataAccess.AddParameter("eInvoice", eInvoice, ParameterDirection.Input);
            this._dataAccess.AddParameter("ePurchase", ePurchase, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUP_ID", GROUP_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("XML_ADDR_NO", XML_ADDR_NO, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_LINK", BUYER_LINK, ParameterDirection.Input);
            this._dataAccess.AddParameter("WEB_LINK", WEB_LINK, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_COUNTRY_CODE", ADDR_COUNTRY_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("IsLeSConnect", IsLeSConnect, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        /****/
        public virtual System.Data.DataSet SM_ADDRESS_Select_All_Suppliers()
        {
            this._dataAccess.CreateProcedureCommand("select_SM_ADDRESS_Select_All_Suppliers");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_ADDRESS_Select_All_Buyers()
        {
            this._dataAccess.CreateProcedureCommand("select_SM_ADDRESS_Select_All_Buyers");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_ADDRESS_Select_All_Default_Buyers()
        {
            this._dataAccess.CreateProcedureCommand("select_SM_ADDRESS_Select_All_Default_Buyers");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_ADDRESS_Select_All_Default_Suppliers()
        {
            this._dataAccess.CreateProcedureCommand("select_SM_ADDRESS_Select_All_Default_Suppliers");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_ADDRESS_Select_Unlinked_Buyers(int SUPPLIERID)
        {
            this._dataAccess.CreateProcedureCommand("select_SM_ADDRESS_Select_Unlinked_Buyers");
            this._dataAccess.AddParameter("SUPPLIERID", SUPPLIERID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_ADDRESS_Select_Linked_Suppliers(int BuyerId)
        {
            this._dataAccess.CreateProcedureCommand("select_SM_ADDRESS_Select_Linked_Suppliers");
            this._dataAccess.AddParameter("BUYERID", BuyerId, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_ADDRESS_Select_Linked_Buyers(int SupplierId)
        {
            this._dataAccess.CreateProcedureCommand("select_SM_ADDRESS_Select_Linked_Buyers");
            this._dataAccess.AddParameter("SUPPLIERID", SupplierId, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet select_SM_ADDRESS_Select_Buyers_By_Supplier(int SupplierId)
        {
            this._dataAccess.CreateProcedureCommand("select_SM_ADDRESS_Select_Buyers_By_Supplier");
            this._dataAccess.AddParameter("SUPPLIERID", SupplierId, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet select_SM_ADDRESS_Overview_ThisWeek(string AddrType)
        {
            this._dataAccess.CreateProcedureCommand("select_SM_OVERVIEW_THIS_WEEK_BY_ADDR_TYPE");
            this._dataAccess.AddParameter("ADDR_TYPE", AddrType, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet select_SM_ADDRESS_Overview_ByMonth(string AddrType, int Month, int Year)
        {
            this._dataAccess.CreateProcedureCommand("select_SM_OVERVIEW_MONTH_BY_ADDR_TYPE");
            this._dataAccess.AddParameter("ADDR_TYPE", AddrType, ParameterDirection.Input);
            this._dataAccess.AddParameter("MONTH", Month, ParameterDirection.Input);
            this._dataAccess.AddParameter("YEAR", Year, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
         
        public virtual System.Data.DataSet select_SM_ADDRESS_Overview_LastWeek(string AddrType)
        {
            this._dataAccess.CreateProcedureCommand("select_SM_OVERVIEW_LAST_WEEK_BY_ADDR_TYPE");
            this._dataAccess.AddParameter("ADDR_TYPE", AddrType, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet select_SM_ADDRESS_Overview_ThisYear(string AddrType)
        {
            this._dataAccess.CreateProcedureCommand("select_SM_OVERVIEW_THIS_YEAR_BY_ADDR_TYPE");
            this._dataAccess.AddParameter("ADDR_TYPE", AddrType, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet select_SM_ADDRESS_Overview_All(string AddrType)
        {
            this._dataAccess.CreateProcedureCommand("select_SM_OVERVIEW_ALL_BY_ADDR_TYPE");
            this._dataAccess.AddParameter("ADDR_TYPE", AddrType, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual void Dispose()
        {
            if ((this._dataAccess != null))
            {
                this._dataAccess.Dispose();
            }
        }

        //added by kalpita on 03/01/2018
        public virtual System.Data.DataSet select_SM_ADDRESS_Overview_ThisWeek_By_AddressId(string ADDRESSID, string AddrType)
        {
            this._dataAccess.CreateProcedureCommand("select_SM_OVERVIEW_THIS_WEEK_BY_ADDR_TYPE_ADDRESSID");
            this._dataAccess.AddParameter("ADDR_TYPE", AddrType, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet select_SM_ADDRESS_Overview_LastWeek_By_AddressId(string ADDRESSID, string AddrType)
        {
            this._dataAccess.CreateProcedureCommand("select_SM_OVERVIEW_LAST_WEEK_BY_ADDR_TYPE_ADDRESSID");
            this._dataAccess.AddParameter("ADDR_TYPE", AddrType, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet select_SM_ADDRESS_Overview_ByMonthBy_AddressId(string ADDRESSID, string AddrType, int Month, int Year)
        {
            this._dataAccess.CreateProcedureCommand("select_SM_OVERVIEW_MONTH_BY_ADDR_TYPE_ADDRESSID");
            this._dataAccess.AddParameter("ADDR_TYPE", AddrType, ParameterDirection.Input);
            this._dataAccess.AddParameter("MONTH", Month, ParameterDirection.Input);
            this._dataAccess.AddParameter("YEAR", Year, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
         
        public virtual System.Data.DataSet select_SM_ADDRESS_Overview_ThisYear_By_AddressId(string ADDRESSID, string AddrType)
        {
            this._dataAccess.CreateProcedureCommand("select_SM_OVERVIEW_THIS_YEAR_BY_ADDR_TYPE_ADDRESSID");
            this._dataAccess.AddParameter("ADDR_TYPE", AddrType, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_ADDRESS_Search(string ADDR_TYPE, string ADDR_NAME)
        {
            this._dataAccess.CreateSQLCommand("SELECT ADDR_NAME FROM SM_ADDRESS WHERE ADDR_TYPE = @ADDR_TYPE AND ADDR_NAME LIKE '%'+@ADDR_NAME+'%'");
            this._dataAccess.AddParameter("ADDR_TYPE", ADDR_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDR_NAME", ADDR_NAME, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }


        //added by kalpita on 13/06/2019
        public virtual System.Data.DataSet select_LinkedOverview_ThisWeek_By_AddressId(string ADDRESSID, string AddrType)
        {
            this._dataAccess.CreateProcedureCommand("select_LINKED_SM_OVERVIEW_THIS_WEEK_BY_ADDR_TYPE_ADDRESSID");
            this._dataAccess.AddParameter("ADDR_TYPE", AddrType, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet select_LinkedOverview_LastWeek_By_AddressId(string ADDRESSID, string AddrType)
        {
            this._dataAccess.CreateProcedureCommand("select_LINKED_SM_OVERVIEW_LAST_WEEK_BY_ADDR_TYPE_ADDRESSID");
            this._dataAccess.AddParameter("ADDR_TYPE", AddrType, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet select_LinkedOverview_ByMonthBy_AddressId(string ADDRESSID, string AddrType, int Month, int Year)
        {
            this._dataAccess.CreateProcedureCommand("select_LINKED_SM_OVERVIEW_MONTH_BY_ADDR_TYPE_ADDRESSID");
            this._dataAccess.AddParameter("ADDR_TYPE", AddrType, ParameterDirection.Input);
            this._dataAccess.AddParameter("MONTH", Month, ParameterDirection.Input);
            this._dataAccess.AddParameter("YEAR", Year, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet select_LinkedOverview_ThisYear_By_AddressId(string ADDRESSID, string AddrType)
        {
            this._dataAccess.CreateProcedureCommand("select_LINKED_SM_OVERVIEW_THIS_YEAR_BY_ADDR_TYPE_ADDRESSID");
            this._dataAccess.AddParameter("ADDR_TYPE", AddrType, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet select_LinkedOverview_ALL_By_AddressId(string ADDRESSID, string AddrType)
        {
            this._dataAccess.CreateProcedureCommand("select_LINKED_SM_OVERVIEW_ALL_BY_ADDR_TYPE_ADDRESSID");
            this._dataAccess.AddParameter("ADDR_TYPE", AddrType, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDRESSID", ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
    }
}
