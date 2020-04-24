namespace MetroLesMonitor.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
   // using LeSMonitor.Bll;
    
    
    public partial class SmItemUomMapping : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmItemUomMapping() {
            this._dataAccess = new DataAccess();
        }


        public SmItemUomMapping(DataAccess _dataAccess)
        {
            // TODO: Complete member initialization
            this._dataAccess = _dataAccess;
        }
        
        public virtual System.Data.DataSet SM_ITEM_UOM_MAPPING_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_ITEM_UOM_MAPPING_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_ITEM_UOM_MAPPING_Select_One(System.Nullable<int> ITEM_UOM_MAPID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_ITEM_UOM_MAPPING_Select_One");
            this._dataAccess.AddParameter("ITEM_UOM_MAPID", ITEM_UOM_MAPID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual void SM_ITEM_UOM_MAPPING_Insert(System.Nullable<int> ITEM_UOM_MAPID, System.Nullable<int> BUYER_ID, System.Nullable<int> SUPPLIER_ID, string BUYER_ITEM_UOM, string SUPPLIER_ITEM_UOM) {
            this._dataAccess.CreateProcedureCommand("sp_SM_ITEM_UOM_MAPPING_Insert");
            this._dataAccess.AddParameter("ITEM_UOM_MAPID", ITEM_UOM_MAPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_ID", BUYER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_ID", SUPPLIER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_ITEM_UOM", BUYER_ITEM_UOM, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_ITEM_UOM", SUPPLIER_ITEM_UOM, ParameterDirection.Input);
            this._dataAccess.ExecuteNonQuery();
        }

        public DataSet SM_ITEM_UOM_MAPPING_Select_By_BuyerID_SupplierId(System.Nullable<int> SupplierID, System.Nullable<int> BuyerID)
        {
            this._dataAccess.CreateSQLCommand("SELECT * FROM SM_ITEM_UOM_MAPPING WHERE SUPPLIER_ID = @SupplierID AND BUYER_ID = @BuyerID");
            this._dataAccess.AddParameter("SupplierID", SupplierID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BuyerID", BuyerID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;

        }

        public int Get_Last_Item_UOM_ID()
        {
            int value = 0;
            this._dataAccess.CreateSQLCommand("SELECT max(ITEM_UOM_MAPID) as MAX_ID FROM SM_ITEM_UOM_MAPPING");
            DataSet Ds_value = this._dataAccess.ExecuteDataSet();
            if (Ds_value !=null && Ds_value.Tables.Count > 0 && Ds_value.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Ds_value.Tables[0].Rows)
                {
                    value = Convert.ToInt32(dr["MAX_ID"]);
                    break;
                }
            }
            return value;

        }

        public virtual int SM_BUYER_SUPPLIER_ITEM_UOM_DeleteBySupplierIDBuyerID(int BuyerID,int SupplierID)
        {
            this._dataAccess.CreateSQLCommand("DELETE SM_ITEM_UOM_MAPPING WHERE BUYER_ID = @BUYER_ID and SUPPLIER_ID=@SUPPLIER_ID");
            this._dataAccess.AddParameter("BUYER_ID", BuyerID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_ID", SupplierID, ParameterDirection.Input);
            int val = this._dataAccess.ExecuteNonQuery();
            return val;
        }
        
        public virtual System.Nullable<int> SM_ITEM_UOM_MAPPING_Delete(System.Nullable<int> ITEM_UOM_MAPID) {
            this._dataAccess.CreateProcedureCommand("sp_SM_ITEM_UOM_MAPPING_Delete");
            this._dataAccess.AddParameter("ITEM_UOM_MAPID", ITEM_UOM_MAPID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> SM_ITEM_UOM_MAPPING_Update(System.Nullable<int> ITEM_UOM_MAPID, System.Nullable<int> BUYER_ID, System.Nullable<int> SUPPLIER_ID, string BUYER_ITEM_UOM, string SUPPLIER_ITEM_UOM) {
            this._dataAccess.CreateProcedureCommand("sp_SM_ITEM_UOM_MAPPING_Update");
            this._dataAccess.AddParameter("ITEM_UOM_MAPID", ITEM_UOM_MAPID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_ID", BUYER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_ID", SUPPLIER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_ITEM_UOM", BUYER_ITEM_UOM, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_ITEM_UOM", SUPPLIER_ITEM_UOM, ParameterDirection.Input);
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
