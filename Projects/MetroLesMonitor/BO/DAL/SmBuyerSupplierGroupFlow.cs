namespace MetroLesMonitor.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;

    public partial class SmBuyerSupplierGroupFlow : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmBuyerSupplierGroupFlow() {
            this._dataAccess = new DataAccess();
        }

        public SmBuyerSupplierGroupFlow(DataAccess _dataAccess)
        {
            // TODO: Complete member initialization
            this._dataAccess = _dataAccess;
        }

        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_GROUP_FLOW_Select_All()
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_GROUP_FLOW_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_GROUP_FLOW_Select_One(System.Nullable<int> GROUPFLOWID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_GROUP_FLOW_Select_One");
            this._dataAccess.AddParameter("GROUP_FLOWID", GROUPFLOWID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_GROUP_FLOW_Select_By_GroupID(System.Nullable<int> GROUP_ID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_GROUP_FLOW_Select_By_GROUPID");
            this._dataAccess.AddParameter("GROUP_ID", GROUP_ID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Nullable<int> SM_BUYER_SUPPLIER_GROUP_FLOW_Insert(System.Nullable<int> GROUPFLOWID, System.Nullable<int> GROUP_ID, System.Nullable<int> RFQ, System.Nullable<int> QUOTE, System.Nullable<int> PO, System.Nullable<int> POC, System.Nullable<int> RFQ_END_STATE, System.Nullable<int> QUOTE_END_STATE, System.Nullable<int> PO_END_STATE, System.Nullable<int> POC_END_STATE, System.Nullable<int> QUOTE_EXPORT_MARKER, System.Nullable<int> QUOTE_BUYER_EXPORT_MARKER, System.Nullable<int> POC_EXPORT_MARKER, System.Nullable<int> POC_BUYER_EXPORT_MARKER)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_GROUP_FLOW_Insert");
            this._dataAccess.AddParameter("GROUP_FLOWID", 0, ParameterDirection.Output);
            this._dataAccess.AddParameter("GROUP_ID", GROUP_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RFQ", RFQ, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE", QUOTE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PO", PO, ParameterDirection.Input);
            this._dataAccess.AddParameter("POC", POC, ParameterDirection.Input);
            this._dataAccess.AddParameter("RFQ_END_STATE", RFQ_END_STATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_END_STATE", QUOTE_END_STATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PO_END_STATE", PO_END_STATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("POC_END_STATE", POC_END_STATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_EXPORT_MARKER", QUOTE_EXPORT_MARKER, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_BUYER_EXPORT_MARKER", QUOTE_BUYER_EXPORT_MARKER, ParameterDirection.Input);
            this._dataAccess.AddParameter("POC_EXPORT_MARKER", POC_EXPORT_MARKER, ParameterDirection.Input);
            this._dataAccess.AddParameter("POC_BUYER_EXPORT_MARKER", POC_BUYER_EXPORT_MARKER, ParameterDirection.Input);
            int value = (int)this._dataAccess.ExecuteNonQuery();
            value = convert.ToInt(this._dataAccess.Command.Parameters["GROUP_FLOWID"].Value);
            return value;
        }

        public virtual System.Nullable<int> SM_BUYER_SUPPLIER_GROUP_FLOW_Delete(System.Nullable<int> GROUP_FLOWID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_GROUP_FLOW_Delete");
            this._dataAccess.AddParameter("GROUP_FLOWID", GROUP_FLOWID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_BUYER_SUPPLIER_GROUP_FLOW_Update(System.Nullable<int> GROUPFLOWID, System.Nullable<int> GROUP_ID, System.Nullable<int> RFQ, System.Nullable<int> QUOTE, System.Nullable<int> PO, System.Nullable<int> POC, System.Nullable<int> RFQ_END_STATE, System.Nullable<int> QUOTE_END_STATE, System.Nullable<int> PO_END_STATE, System.Nullable<int> POC_END_STATE, System.Nullable<int> QUOTE_EXPORT_MARKER, System.Nullable<int> QUOTE_BUYER_EXPORT_MARKER, System.Nullable<int> POC_EXPORT_MARKER, System.Nullable<int> POC_BUYER_EXPORT_MARKER)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_GROUP_FLOW_Update");
            this._dataAccess.AddParameter("GROUP_FLOWID", GROUPFLOWID, ParameterDirection.Input);
            this._dataAccess.AddParameter("GROUP_ID", GROUP_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("RFQ", RFQ, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE", QUOTE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PO", PO, ParameterDirection.Input);
            this._dataAccess.AddParameter("POC", POC, ParameterDirection.Input);
            this._dataAccess.AddParameter("RFQ_END_STATE", RFQ_END_STATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_END_STATE", QUOTE_END_STATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("PO_END_STATE", PO_END_STATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("POC_END_STATE", POC_END_STATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_EXPORT_MARKER", QUOTE_EXPORT_MARKER, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUOTE_BUYER_EXPORT_MARKER", QUOTE_BUYER_EXPORT_MARKER, ParameterDirection.Input);
            this._dataAccess.AddParameter("POC_EXPORT_MARKER", POC_EXPORT_MARKER, ParameterDirection.Input);
            this._dataAccess.AddParameter("POC_BUYER_EXPORT_MARKER", POC_BUYER_EXPORT_MARKER, ParameterDirection.Input);
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
