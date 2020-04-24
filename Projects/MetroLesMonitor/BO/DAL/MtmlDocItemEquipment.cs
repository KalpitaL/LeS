namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class MtmlDocItemEquipment : IDisposable {
        
        private DataAccess _dataAccess;
        
        public MtmlDocItemEquipment() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet MTML_DOC_ITEM_EQUIPMENT_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_ITEM_EQUIPMENT_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet MTML_DOC_ITEM_EQUIPMENT_Select_One(System.Nullable<System.Guid> ITEMEQUIPID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_ITEM_EQUIPMENT_Select_One");
            this._dataAccess.AddParameter("ITEMEQUIPID", ITEMEQUIPID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<System.Guid> MTML_DOC_ITEM_EQUIPMENT_Insert(System.Nullable<System.Guid> MTMLITEMID, System.Nullable<System.Guid> MTMLDOCID, string EQUIP_NAME, string MANUFACTURER, string SERIALNUMBER, string MODELTYPE, string DRAWINGNUMBER, string DESCRIPTION) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_ITEM_EQUIPMENT_Insert");
            this._dataAccess.AddParameter("MTMLITEMID", MTMLITEMID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MTMLDOCID", MTMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_NAME", EQUIP_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("MANUFACTURER", MANUFACTURER, ParameterDirection.Input);
            this._dataAccess.AddParameter("SERIALNUMBER", SERIALNUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("MODELTYPE", MODELTYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DRAWINGNUMBER", DRAWINGNUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("DESCRIPTION", DESCRIPTION, ParameterDirection.Input);
            Guid value = ((Guid)(this._dataAccess.ExecuteScalar()));
            return value;
        }
        
        public virtual System.Nullable<int> MTML_DOC_ITEM_EQUIPMENT_Delete(System.Nullable<System.Guid> ITEMEQUIPID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_ITEM_EQUIPMENT_Delete");
            this._dataAccess.AddParameter("ITEMEQUIPID", ITEMEQUIPID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> MTML_DOC_ITEM_EQUIPMENT_Update(System.Nullable<System.Guid> ITEMEQUIPID, System.Nullable<System.Guid> MTMLITEMID, System.Nullable<System.Guid> MTMLDOCID, string EQUIP_NAME, string MANUFACTURER, string SERIALNUMBER, string MODELTYPE, string DRAWINGNUMBER, string DESCRIPTION, System.Nullable<int> AUTOID) {
            this._dataAccess.CreateProcedureCommand("sp_MTML_DOC_ITEM_EQUIPMENT_Update");
            this._dataAccess.AddParameter("ITEMEQUIPID", ITEMEQUIPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MTMLITEMID", MTMLITEMID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MTMLDOCID", MTMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_NAME", EQUIP_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("MANUFACTURER", MANUFACTURER, ParameterDirection.Input);
            this._dataAccess.AddParameter("SERIALNUMBER", SERIALNUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("MODELTYPE", MODELTYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("DRAWINGNUMBER", DRAWINGNUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("DESCRIPTION", DESCRIPTION, ParameterDirection.Input);
            this._dataAccess.AddParameter("AUTOID", AUTOID, ParameterDirection.Input);
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
