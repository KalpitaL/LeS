namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmDocEquipment : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmDocEquipment() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet SM_DOC_EQUIPMENT_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_DOC_EQUIPMENT_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_DOC_EQUIPMENT_Select_One(System.Nullable<int> DocEquipId) {
            this._dataAccess.CreateProcedureCommand("sp_SM_DOC_EQUIPMENT_Select_One");
            this._dataAccess.AddParameter("DocEquipId", DocEquipId, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> SM_DOC_EQUIPMENT_Insert(System.Nullable<int> QuotationId, string EquipName, string EquipDescription, string EquipMaker, string EquipType, string EquipSerNo, string EquipDrgNo, string EquipRemarks) {
            this._dataAccess.CreateProcedureCommand("sp_SM_DOC_EQUIPMENT_Insert");
            this._dataAccess.AddParameter("QuotationId", QuotationId, ParameterDirection.Input);
            this._dataAccess.AddParameter("EquipName", EquipName, ParameterDirection.Input);
            this._dataAccess.AddParameter("EquipDescription", EquipDescription, ParameterDirection.Input);
            this._dataAccess.AddParameter("EquipMaker", EquipMaker, ParameterDirection.Input);
            this._dataAccess.AddParameter("EquipType", EquipType, ParameterDirection.Input);
            this._dataAccess.AddParameter("EquipSerNo", EquipSerNo, ParameterDirection.Input);
            this._dataAccess.AddParameter("EquipDrgNo", EquipDrgNo, ParameterDirection.Input);
            this._dataAccess.AddParameter("EquipRemarks", EquipRemarks, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_DOC_EQUIPMENT_Delete(System.Nullable<int> DocEquipId) {
            this._dataAccess.CreateProcedureCommand("sp_SM_DOC_EQUIPMENT_Delete");
            this._dataAccess.AddParameter("DocEquipId", DocEquipId, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_DOC_EQUIPMENT_Update(System.Nullable<int> DocEquipId, System.Nullable<int> QuotationId, string EquipName, string EquipDescription, string EquipMaker, string EquipType, string EquipSerNo, string EquipDrgNo, string EquipRemarks) {
            this._dataAccess.CreateProcedureCommand("sp_SM_DOC_EQUIPMENT_Update");
            this._dataAccess.AddParameter("DocEquipId", DocEquipId, ParameterDirection.Input);
            this._dataAccess.AddParameter("QuotationId", QuotationId, ParameterDirection.Input);
            this._dataAccess.AddParameter("EquipName", EquipName, ParameterDirection.Input);
            this._dataAccess.AddParameter("EquipDescription", EquipDescription, ParameterDirection.Input);
            this._dataAccess.AddParameter("EquipMaker", EquipMaker, ParameterDirection.Input);
            this._dataAccess.AddParameter("EquipType", EquipType, ParameterDirection.Input);
            this._dataAccess.AddParameter("EquipSerNo", EquipSerNo, ParameterDirection.Input);
            this._dataAccess.AddParameter("EquipDrgNo", EquipDrgNo, ParameterDirection.Input);
            this._dataAccess.AddParameter("EquipRemarks", EquipRemarks, ParameterDirection.Input);
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
