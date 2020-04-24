namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class MtmlDocHeader : IDisposable {
        
        private DataAccess _dataAccess;
        
        public MtmlDocHeader() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet MTML_DOC_HEADER_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_HEADER_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet MTML_DOC_HEADER_Select_One(System.Nullable<System.Guid> MTMLDOCHEADERID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_HEADER_Select_One");
            this._dataAccess.AddParameter("MTMLDOCHEADERID", MTMLDOCHEADERID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<System.Guid> MTML_DOC_HEADER_Insert(
                    System.Nullable<System.Guid> MTMLDOCID, 
                    string DOCTYPE, 
                    string VERSIONNUMBER, 
                    string TAXSTATUS, 
                    string RELEASENUMBER, 
                    string PRIORITY, 
                    string MESSAGEREFERENCENUMBER, 
                    string MESSAGENUMBER, 
                    System.Nullable<int> LINECOUNT, 
                    string FUNCTIONCODE, 
                    string DELIVERYTERMSCODE, 
                    string CURRENCYCODE, 
                    string CONTROLLINGAGENCY, 
                    string ASSOCIATIONASSIGNEDCODE, 
                    string TransportModeCode, 
                    string EQUIP_NAME, 
                    string MANUFACTURER, 
                    string ModelNumber, 
                    string PORT_CODE, 
                    string PORT_NAME, 
                    string DepartmentCode, 
                    string ControlReference, 
                    System.Nullable<float> TOTAL_AMOUNT, 
                    string ORIGINATINGSYSTEMREF, 
                    System.Nullable<int> LeadTimeDays, 
                    string EQUIP_TYPE, 
                    string EQUIP_DTLS, 
                    string BUYER_REFNO, 
                    System.Nullable<float> ADDITIONAL_DISC, 
                    string UDF1, 
                    string UDF2, 
                    string UDF3, 
                    string RevisionNumber, 
                    string OrderHandling, 
                    string OrderType, 
                    string OriginatingRequestNo, 
                    string ShipComplete, 
                    string SupplierORGRef, 
                    string UpdType, 
                    string ContractType) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_HEADER_Insert");
            this._dataAccess.AddParameter("MTMLDOCID", MTMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOCTYPE", DOCTYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("VERSIONNUMBER", VERSIONNUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("TAXSTATUS", TAXSTATUS, ParameterDirection.Input);
            this._dataAccess.AddParameter("RELEASENUMBER", RELEASENUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("PRIORITY", PRIORITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("MESSAGEREFERENCENUMBER", MESSAGEREFERENCENUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("MESSAGENUMBER", MESSAGENUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINECOUNT", LINECOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("FUNCTIONCODE", FUNCTIONCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERYTERMSCODE", DELIVERYTERMSCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURRENCYCODE", CURRENCYCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONTROLLINGAGENCY", CONTROLLINGAGENCY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ASSOCIATIONASSIGNEDCODE", ASSOCIATIONASSIGNEDCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("TransportModeCode", TransportModeCode, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_NAME", EQUIP_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("MANUFACTURER", MANUFACTURER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ModelNumber", ModelNumber, ParameterDirection.Input);
            this._dataAccess.AddParameter("PORT_CODE", PORT_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PORT_NAME", PORT_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("DepartmentCode", DepartmentCode, ParameterDirection.Input);
            this._dataAccess.AddParameter("ControlReference", ControlReference, ParameterDirection.Input);
            this._dataAccess.AddParameter("TOTAL_AMOUNT", TOTAL_AMOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORIGINATINGSYSTEMREF", ORIGINATINGSYSTEMREF, ParameterDirection.Input);
            this._dataAccess.AddParameter("LeadTimeDays", LeadTimeDays, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_TYPE", EQUIP_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_DTLS", EQUIP_DTLS, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_REFNO", BUYER_REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDITIONAL_DISC", ADDITIONAL_DISC, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF1", UDF1, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF2", UDF2, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF3", UDF3, ParameterDirection.Input);
            this._dataAccess.AddParameter("RevisionNumber", RevisionNumber, ParameterDirection.Input);
            this._dataAccess.AddParameter("OrderHandling", OrderHandling, ParameterDirection.Input);
            this._dataAccess.AddParameter("OrderType", OrderType, ParameterDirection.Input);
            this._dataAccess.AddParameter("OriginatingRequestNo", OriginatingRequestNo, ParameterDirection.Input);
            this._dataAccess.AddParameter("ShipComplete", ShipComplete, ParameterDirection.Input);
            this._dataAccess.AddParameter("SupplierORGRef", SupplierORGRef, ParameterDirection.Input);
            this._dataAccess.AddParameter("UpdType", UpdType, ParameterDirection.Input);
            this._dataAccess.AddParameter("ContractType", ContractType, ParameterDirection.Input);
            Guid value = ((Guid)(this._dataAccess.ExecuteScalar()));
            return value;
        }
        
        public virtual System.Nullable<int> MTML_DOC_HEADER_Delete(System.Nullable<System.Guid> MTMLDOCHEADERID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_HEADER_Delete");
            this._dataAccess.AddParameter("MTMLDOCHEADERID", MTMLDOCHEADERID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> MTML_DOC_HEADER_Update(
                    System.Nullable<System.Guid> MTMLDOCHEADERID, 
                    System.Nullable<System.Guid> MTMLDOCID, 
                    string DOCTYPE, 
                    string VERSIONNUMBER, 
                    string TAXSTATUS, 
                    string RELEASENUMBER, 
                    string PRIORITY, 
                    string MESSAGEREFERENCENUMBER, 
                    string MESSAGENUMBER, 
                    System.Nullable<int> LINECOUNT, 
                    string FUNCTIONCODE, 
                    string DELIVERYTERMSCODE, 
                    string CURRENCYCODE, 
                    string CONTROLLINGAGENCY, 
                    string ASSOCIATIONASSIGNEDCODE, 
                    string TransportModeCode, 
                    string EQUIP_NAME, 
                    string MANUFACTURER, 
                    string ModelNumber, 
                    string PORT_CODE, 
                    string PORT_NAME, 
                    string DepartmentCode, 
                    string ControlReference, 
                    System.Nullable<float> TOTAL_AMOUNT, 
                    string ORIGINATINGSYSTEMREF, 
                    System.Nullable<int> LeadTimeDays, 
                    string EQUIP_TYPE, 
                    string EQUIP_DTLS, 
                    string BUYER_REFNO, 
                    System.Nullable<float> ADDITIONAL_DISC, 
                    System.Nullable<int> AUTOID, 
                    string UDF1, 
                    string UDF2, 
                    string UDF3, 
                    string RevisionNumber, 
                    string OrderHandling, 
                    string OrderType, 
                    string OriginatingRequestNo, 
                    string ShipComplete, 
                    string SupplierORGRef, 
                    string UpdType, 
                    string ContractType) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_HEADER_Update");
            this._dataAccess.AddParameter("MTMLDOCHEADERID", MTMLDOCHEADERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MTMLDOCID", MTMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOCTYPE", DOCTYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("VERSIONNUMBER", VERSIONNUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("TAXSTATUS", TAXSTATUS, ParameterDirection.Input);
            this._dataAccess.AddParameter("RELEASENUMBER", RELEASENUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("PRIORITY", PRIORITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("MESSAGEREFERENCENUMBER", MESSAGEREFERENCENUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("MESSAGENUMBER", MESSAGENUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINECOUNT", LINECOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("FUNCTIONCODE", FUNCTIONCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DELIVERYTERMSCODE", DELIVERYTERMSCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURRENCYCODE", CURRENCYCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CONTROLLINGAGENCY", CONTROLLINGAGENCY, ParameterDirection.Input);
            this._dataAccess.AddParameter("ASSOCIATIONASSIGNEDCODE", ASSOCIATIONASSIGNEDCODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("TransportModeCode", TransportModeCode, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_NAME", EQUIP_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("MANUFACTURER", MANUFACTURER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ModelNumber", ModelNumber, ParameterDirection.Input);
            this._dataAccess.AddParameter("PORT_CODE", PORT_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PORT_NAME", PORT_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("DepartmentCode", DepartmentCode, ParameterDirection.Input);
            this._dataAccess.AddParameter("ControlReference", ControlReference, ParameterDirection.Input);
            this._dataAccess.AddParameter("TOTAL_AMOUNT", TOTAL_AMOUNT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORIGINATINGSYSTEMREF", ORIGINATINGSYSTEMREF, ParameterDirection.Input);
            this._dataAccess.AddParameter("LeadTimeDays", LeadTimeDays, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_TYPE", EQUIP_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_DTLS", EQUIP_DTLS, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_REFNO", BUYER_REFNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("ADDITIONAL_DISC", ADDITIONAL_DISC, ParameterDirection.Input);
            this._dataAccess.AddParameter("AUTOID", AUTOID, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF1", UDF1, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF2", UDF2, ParameterDirection.Input);
            this._dataAccess.AddParameter("UDF3", UDF3, ParameterDirection.Input);
            this._dataAccess.AddParameter("RevisionNumber", RevisionNumber, ParameterDirection.Input);
            this._dataAccess.AddParameter("OrderHandling", OrderHandling, ParameterDirection.Input);
            this._dataAccess.AddParameter("OrderType", OrderType, ParameterDirection.Input);
            this._dataAccess.AddParameter("OriginatingRequestNo", OriginatingRequestNo, ParameterDirection.Input);
            this._dataAccess.AddParameter("ShipComplete", ShipComplete, ParameterDirection.Input);
            this._dataAccess.AddParameter("SupplierORGRef", SupplierORGRef, ParameterDirection.Input);
            this._dataAccess.AddParameter("UpdType", UpdType, ParameterDirection.Input);
            this._dataAccess.AddParameter("ContractType", ContractType, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual void Dispose() {
            if ((this._dataAccess != null)) {
                this._dataAccess.Dispose();
            }
        }
    }
}
