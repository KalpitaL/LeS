namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class MxmlDocItems : IDisposable {
        
        private DataAccess _dataAccess;
        
        public MxmlDocItems() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet Select_MXML_DOC_ITEMSs_By_MXMLDOCID(System.Nullable<System.Guid> MXMLDOCID) {
            this._dataAccess.CreateProcedureCommand("sp_Select_MXML_DOC_ITEMSs_By_MXMLDOCID");
            this._dataAccess.AddParameter("MXMLDOCID", MXMLDOCID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet MXML_DOC_ITEMS_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_MXML_DOC_ITEMS_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet MXML_DOC_ITEMS_Select_One(System.Nullable<System.Guid> MXMLITEMID) {
            this._dataAccess.CreateProcedureCommand("sp_MXML_DOC_ITEMS_Select_One");
            this._dataAccess.AddParameter("MXMLITEMID", MXMLITEMID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<System.Guid> MXML_DOC_ITEMS_Insert(
                    System.Nullable<System.Guid> MXMLDOCID, 
                    System.Nullable<int> LINENUMBER, 
                    string ORIGINATINGSYSTEMREF, 
                    string SUPPLIERPARTID, 
                    string ITEM_DESCRIPTION, 
                    System.Nullable<float> UNIT_PRICE, 
                    string ITEM_UNIT, 
                    string ITEM_COMMENT, 
                    string EQUIPMENT_DESCRIPTION, 
                    string EQUIP_DRAWINGNO, 
                    string EQUIP_MANUFACTURER, 
                    string EQUIP_MODEL, 
                    string EQUIP_SERIALNO, 
                    string EQUIP_NAME, 
                    System.Nullable<float> QUANTITY, 
                    System.Nullable<float> LIST_PRICE, 
                    System.Nullable<float> ITEM_DISCOUNT) {
            this._dataAccess.CreateProcedureCommand("sp_MXML_DOC_ITEMS_Insert");
            this._dataAccess.AddParameter("MXMLDOCID", MXMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINENUMBER", LINENUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORIGINATINGSYSTEMREF", ORIGINATINGSYSTEMREF, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERPARTID", SUPPLIERPARTID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_DESCRIPTION", ITEM_DESCRIPTION, ParameterDirection.Input);
            this._dataAccess.AddParameter("UNIT_PRICE", UNIT_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_UNIT", ITEM_UNIT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_COMMENT", ITEM_COMMENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIPMENT_DESCRIPTION", EQUIPMENT_DESCRIPTION, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_DRAWINGNO", EQUIP_DRAWINGNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_MANUFACTURER", EQUIP_MANUFACTURER, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_MODEL", EQUIP_MODEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_SERIALNO", EQUIP_SERIALNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_NAME", EQUIP_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUANTITY", QUANTITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("LIST_PRICE", LIST_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_DISCOUNT", ITEM_DISCOUNT, ParameterDirection.Input);
            Guid value = ((Guid)(this._dataAccess.ExecuteScalar()));
            return value;
        }
        
        public virtual System.Nullable<int> MXML_DOC_ITEMS_Delete(System.Nullable<System.Guid> MXMLITEMID) {
            this._dataAccess.CreateProcedureCommand("sp_MXML_DOC_ITEMS_Delete");
            this._dataAccess.AddParameter("MXMLITEMID", MXMLITEMID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> MXML_DOC_ITEMS_Update(
                    System.Nullable<System.Guid> MXMLITEMID, 
                    System.Nullable<System.Guid> MXMLDOCID, 
                    System.Nullable<int> LINENUMBER, 
                    string ORIGINATINGSYSTEMREF, 
                    string SUPPLIERPARTID, 
                    string ITEM_DESCRIPTION, 
                    System.Nullable<float> UNIT_PRICE, 
                    string ITEM_UNIT, 
                    string ITEM_COMMENT, 
                    string EQUIPMENT_DESCRIPTION, 
                    string EQUIP_DRAWINGNO, 
                    string EQUIP_MANUFACTURER, 
                    string EQUIP_MODEL, 
                    string EQUIP_SERIALNO, 
                    string EQUIP_NAME, 
                    System.Nullable<float> QUANTITY, 
                    System.Nullable<float> LIST_PRICE, 
                    System.Nullable<float> ITEM_DISCOUNT) {
            this._dataAccess.CreateProcedureCommand("sp_MXML_DOC_ITEMS_Update");
            this._dataAccess.AddParameter("MXMLITEMID", MXMLITEMID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MXMLDOCID", MXMLDOCID, ParameterDirection.Input);
            this._dataAccess.AddParameter("LINENUMBER", LINENUMBER, ParameterDirection.Input);
            this._dataAccess.AddParameter("ORIGINATINGSYSTEMREF", ORIGINATINGSYSTEMREF, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIERPARTID", SUPPLIERPARTID, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_DESCRIPTION", ITEM_DESCRIPTION, ParameterDirection.Input);
            this._dataAccess.AddParameter("UNIT_PRICE", UNIT_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_UNIT", ITEM_UNIT, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_COMMENT", ITEM_COMMENT, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIPMENT_DESCRIPTION", EQUIPMENT_DESCRIPTION, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_DRAWINGNO", EQUIP_DRAWINGNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_MANUFACTURER", EQUIP_MANUFACTURER, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_MODEL", EQUIP_MODEL, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_SERIALNO", EQUIP_SERIALNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("EQUIP_NAME", EQUIP_NAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("QUANTITY", QUANTITY, ParameterDirection.Input);
            this._dataAccess.AddParameter("LIST_PRICE", LIST_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_DISCOUNT", ITEM_DISCOUNT, ParameterDirection.Input);
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
