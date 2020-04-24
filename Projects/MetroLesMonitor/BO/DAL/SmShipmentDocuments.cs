namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmShipmentDocuments : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmShipmentDocuments() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_SHIPMENT_DOCUMENTS_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_SHIPMENT_DOCUMENTS_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_SHIPMENT_DOCUMENTS_Select_One(System.Nullable<int> SHIPMENT_ID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_SHIPMENT_DOCUMENTS_Select_One");
            this._dataAccess.AddParameter("SHIPMENT_ID", SHIPMENT_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_SHIPMENT_DOCUMENTS_Insert(
                    string DOCID, 
                    string DOC_TYPE, 
                    string SENDER_CODE, 
                    string RECEIVER_CODE, 
                    string SENDER_NAME, 
                    string RECEIVER_NAME, 
                    string SUPPLIER_NAME, 
                    string PO_NO, 
                    string SHIPMENT_NO, 
                    string VESSEL_NAME, 
                    string VESSEL_IMONO, 
                    string PORT_CODE, 
                    string PORT_NAME, 
                    System.Nullable<float> PO_TOTAL, 
                    System.Nullable<float> SM_TOTAL, 
                    string CURR_CODE, 
                    System.Nullable<System.DateTime> SHIPMENT_DATE, 
                    System.Nullable<System.DateTime> PO_DATE, 
                    System.Nullable<System.DateTime> REQ_DEL_DATE, 
                    System.Nullable<System.DateTime> ACT_DEL_DATE, 
                    string LOCATION, 
                    string HDR_REMARKS, 
                    System.Nullable<System.DateTime> UPDATE_DATE, 
                    System.Nullable<System.DateTime> CREATED_DATE, 
                    string WEIGHT_UOM, 
                    string WEIGHT_VALUE, 
                    string TRANSPORT_MODE, 
                    System.Nullable<int> EXPORTED, 
                    string MSG_REF) {
            this._dataAccess.CreateProcedureCommand("sp_SM_SHIPMENT_DOCUMENTS_Insert");
            this._dataAccess.AddParameter("DOCID", DOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_TYPE", DOC_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SENDER_CODE", SENDER_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("RECEIVER_CODE", RECEIVER_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SENDER_NAME", SENDER_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("RECEIVER_NAME", RECEIVER_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_NAME", SUPPLIER_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("PO_NO", PO_NO, ParameterDirection.Input);
            this._dataAccess.AddParameter("SHIPMENT_NO", SHIPMENT_NO, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_NAME", VESSEL_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_IMONO", VESSEL_IMONO, ParameterDirection.Input);
            this._dataAccess.AddParameter("PORT_CODE", PORT_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PORT_NAME", PORT_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("PO_TOTAL", PO_TOTAL, ParameterDirection.Input);
            this._dataAccess.AddParameter("SM_TOTAL", SM_TOTAL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_CODE", CURR_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SHIPMENT_DATE", SHIPMENT_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PO_DATE", PO_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("REQ_DEL_DATE", REQ_DEL_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ACT_DEL_DATE", ACT_DEL_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("LOCATION", LOCATION, ParameterDirection.Input);
            this._dataAccess.AddParameter("HDR_REMARKS", HDR_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("WEIGHT_UOM", WEIGHT_UOM, ParameterDirection.Input);
            this._dataAccess.AddParameter("WEIGHT_VALUE", WEIGHT_VALUE, ParameterDirection.Input);
            this._dataAccess.AddParameter("TRANSPORT_MODE", TRANSPORT_MODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORTED", EXPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("MSG_REF", MSG_REF, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_SHIPMENT_DOCUMENTS_Delete(System.Nullable<int> SHIPMENT_ID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_SHIPMENT_DOCUMENTS_Delete");
            this._dataAccess.AddParameter("SHIPMENT_ID", SHIPMENT_ID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_SHIPMENT_DOCUMENTS_Update(
                    System.Nullable<int> SHIPMENT_ID, 
                    string DOCID, 
                    string DOC_TYPE, 
                    string SENDER_CODE, 
                    string RECEIVER_CODE, 
                    string SENDER_NAME, 
                    string RECEIVER_NAME, 
                    string SUPPLIER_NAME, 
                    string PO_NO, 
                    string SHIPMENT_NO, 
                    string VESSEL_NAME, 
                    string VESSEL_IMONO, 
                    string PORT_CODE, 
                    string PORT_NAME, 
                    System.Nullable<float> PO_TOTAL, 
                    System.Nullable<float> SM_TOTAL, 
                    string CURR_CODE, 
                    System.Nullable<System.DateTime> SHIPMENT_DATE, 
                    System.Nullable<System.DateTime> PO_DATE, 
                    System.Nullable<System.DateTime> REQ_DEL_DATE, 
                    System.Nullable<System.DateTime> ACT_DEL_DATE, 
                    string LOCATION, 
                    string HDR_REMARKS, 
                    System.Nullable<System.DateTime> UPDATE_DATE, 
                    System.Nullable<System.DateTime> CREATED_DATE, 
                    string WEIGHT_UOM, 
                    string WEIGHT_VALUE, 
                    string TRANSPORT_MODE, 
                    System.Nullable<int> EXPORTED, 
                    string MSG_REF) {
            this._dataAccess.CreateProcedureCommand("sp_SM_SHIPMENT_DOCUMENTS_Update");
            this._dataAccess.AddParameter("SHIPMENT_ID", SHIPMENT_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOCID", DOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DOC_TYPE", DOC_TYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SENDER_CODE", SENDER_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("RECEIVER_CODE", RECEIVER_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SENDER_NAME", SENDER_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("RECEIVER_NAME", RECEIVER_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_NAME", SUPPLIER_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("PO_NO", PO_NO, ParameterDirection.Input);
            this._dataAccess.AddParameter("SHIPMENT_NO", SHIPMENT_NO, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_NAME", VESSEL_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("VESSEL_IMONO", VESSEL_IMONO, ParameterDirection.Input);
            this._dataAccess.AddParameter("PORT_CODE", PORT_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PORT_NAME", PORT_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("PO_TOTAL", PO_TOTAL, ParameterDirection.Input);
            this._dataAccess.AddParameter("SM_TOTAL", SM_TOTAL, ParameterDirection.Input);
            this._dataAccess.AddParameter("CURR_CODE", CURR_CODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("SHIPMENT_DATE", SHIPMENT_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PO_DATE", PO_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("REQ_DEL_DATE", REQ_DEL_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ACT_DEL_DATE", ACT_DEL_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("LOCATION", LOCATION, ParameterDirection.Input);
            this._dataAccess.AddParameter("HDR_REMARKS", HDR_REMARKS, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("WEIGHT_UOM", WEIGHT_UOM, ParameterDirection.Input);
            this._dataAccess.AddParameter("WEIGHT_VALUE", WEIGHT_VALUE, ParameterDirection.Input);
            this._dataAccess.AddParameter("TRANSPORT_MODE", TRANSPORT_MODE, ParameterDirection.Input);
            this._dataAccess.AddParameter("EXPORTED", EXPORTED, ParameterDirection.Input);
            this._dataAccess.AddParameter("MSG_REF", MSG_REF, ParameterDirection.Input);
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
