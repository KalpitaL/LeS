namespace MetroLesMonitor.Dal {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class LesInventory : IDisposable {
        
        private DataAccess _dataAccess;
        
        public LesInventory() {
            this._dataAccess = new DataAccess();
        }
        
        public virtual System.Data.DataSet Select_LES_INVENTORYs_By_PARTTYPEID(System.Nullable<int> PARTTYPEID) {
            this._dataAccess.CreateProcedureCommand("sp_Select_LES_INVENTORYs_By_PARTTYPEID");
            this._dataAccess.AddParameter("PARTTYPEID", PARTTYPEID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet Select_LES_INVENTORYs_By_LASTSUPPLLIER(System.Nullable<int> LASTSUPPLLIER) {
            this._dataAccess.CreateProcedureCommand("sp_Select_LES_INVENTORYs_By_LASTSUPPLLIER");
            this._dataAccess.AddParameter("LASTSUPPLLIER", LASTSUPPLLIER, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet Select_LES_INVENTORYs_By_MAKER_ADDRESSID(System.Nullable<int> MAKER_ADDRESSID) {
            this._dataAccess.CreateProcedureCommand("sp_Select_LES_INVENTORYs_By_MAKER_ADDRESSID");
            this._dataAccess.AddParameter("MAKER_ADDRESSID", MAKER_ADDRESSID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet Select_LES_INVENTORYs_By_PARTUNITID(System.Nullable<int> PARTUNITID) {
            this._dataAccess.CreateProcedureCommand("sp_Select_LES_INVENTORYs_By_PARTUNITID");
            this._dataAccess.AddParameter("PARTUNITID", PARTUNITID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet Select_LES_INVENTORYs_By_PCK_UNITID(System.Nullable<int> PCK_UNITID) {
            this._dataAccess.CreateProcedureCommand("sp_Select_LES_INVENTORYs_By_PCK_UNITID");
            this._dataAccess.AddParameter("PCK_UNITID", PCK_UNITID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_INVENTORY_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet LES_INVENTORY_Select_One(System.Nullable<int> INVENTORYID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_Select_One");
            this._dataAccess.AddParameter("INVENTORYID", INVENTORYID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Nullable<int> LES_INVENTORY_Insert(
                    string INVNAME, 
                    string MAKERREF, 
                    string DRAWINGNO, 
                    string POSNO, 
                    System.Nullable<int> PARTUNITID, 
                    System.Nullable<int> MAKER_ADDRESSID, 
                    System.Nullable<float> MINLVL, 
                    System.Nullable<float> MAXLVL, 
                    System.Nullable<float> AVAIL_STOCK, 
                    System.Nullable<int> STOCKABLE, 
                    string REMARK, 
                    System.Nullable<int> PARTTYPEID, 
                    System.Nullable<System.DateTime> PART_PRICE_DATE, 
                    System.Nullable<int> LASTSUPPLLIER, 
                    System.Nullable<float> AVG_PRICE, 
                    string Parts_Text1, 
                    string Parts_Text2, 
                    string Parts_Text3, 
                    string Parts_Text4, 
                    System.Nullable<int> PCK_UNITID, 
                    System.Nullable<float> PACK_QTY, 
                    string INVENTORYNO, 
                    System.Nullable<float> RESERVED_STOCK, 
                    System.Nullable<System.DateTime> UPDATE_DATE, 
                    System.Nullable<System.DateTime> CREATED_DATE, 
                    System.Nullable<int> UPDATED_BY, 
                    System.Nullable<int> CREATED_BY, 
                    System.Nullable<int> PREFERRED_SUPPLIERID, 
                    System.Nullable<int> DEFAULT_SUPPLIERID, 
                    System.Nullable<int> OWNERID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_Insert");
            this._dataAccess.AddParameter("INVNAME", INVNAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAKERREF", MAKERREF, ParameterDirection.Input);
            this._dataAccess.AddParameter("DRAWINGNO", DRAWINGNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("POSNO", POSNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTUNITID", PARTUNITID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAKER_ADDRESSID", MAKER_ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MINLVL", MINLVL, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAXLVL", MAXLVL, ParameterDirection.Input);
            this._dataAccess.AddParameter("AVAIL_STOCK", AVAIL_STOCK, ParameterDirection.Input);
            this._dataAccess.AddParameter("STOCKABLE", STOCKABLE, ParameterDirection.Input);
            this._dataAccess.AddParameter("REMARK", REMARK, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTTYPEID", PARTTYPEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("PART_PRICE_DATE", PART_PRICE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("LASTSUPPLLIER", LASTSUPPLLIER, ParameterDirection.Input);
            this._dataAccess.AddParameter("AVG_PRICE", AVG_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("Parts_Text1", Parts_Text1, ParameterDirection.Input);
            this._dataAccess.AddParameter("Parts_Text2", Parts_Text2, ParameterDirection.Input);
            this._dataAccess.AddParameter("Parts_Text3", Parts_Text3, ParameterDirection.Input);
            this._dataAccess.AddParameter("Parts_Text4", Parts_Text4, ParameterDirection.Input);
            this._dataAccess.AddParameter("PCK_UNITID", PCK_UNITID, ParameterDirection.Input);
            this._dataAccess.AddParameter("PACK_QTY", PACK_QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVENTORYNO", INVENTORYNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("RESERVED_STOCK", RESERVED_STOCK, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATED_BY", UPDATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_BY", CREATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("PREFERRED_SUPPLIERID", PREFERRED_SUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEFAULT_SUPPLIERID", DEFAULT_SUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("OWNERID", OWNERID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_INVENTORY_Delete(System.Nullable<int> INVENTORYID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_Delete");
            this._dataAccess.AddParameter("INVENTORYID", INVENTORYID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }
        
        public virtual System.Nullable<int> LES_INVENTORY_Update(
                    System.Nullable<int> INVENTORYID, 
                    string INVNAME, 
                    string MAKERREF, 
                    string DRAWINGNO, 
                    string POSNO, 
                    System.Nullable<int> PARTUNITID, 
                    System.Nullable<int> MAKER_ADDRESSID, 
                    System.Nullable<float> MINLVL, 
                    System.Nullable<float> MAXLVL, 
                    System.Nullable<float> AVAIL_STOCK, 
                    System.Nullable<int> STOCKABLE, 
                    string REMARK, 
                    System.Nullable<int> PARTTYPEID, 
                    System.Nullable<System.DateTime> PART_PRICE_DATE, 
                    System.Nullable<int> LASTSUPPLLIER, 
                    System.Nullable<float> AVG_PRICE, 
                    string Parts_Text1, 
                    string Parts_Text2, 
                    string Parts_Text3, 
                    string Parts_Text4, 
                    System.Nullable<int> PCK_UNITID, 
                    System.Nullable<float> PACK_QTY, 
                    string INVENTORYNO, 
                    System.Nullable<float> RESERVED_STOCK, 
                    System.Nullable<System.DateTime> UPDATE_DATE, 
                    System.Nullable<System.DateTime> CREATED_DATE, 
                    System.Nullable<int> UPDATED_BY, 
                    System.Nullable<int> CREATED_BY, 
                    System.Nullable<int> PREFERRED_SUPPLIERID, 
                    System.Nullable<int> DEFAULT_SUPPLIERID, 
                    System.Nullable<int> OWNERID) {
            this._dataAccess.CreateProcedureCommand("sp_LES_INVENTORY_Update");
            this._dataAccess.AddParameter("INVENTORYID", INVENTORYID, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVNAME", INVNAME, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAKERREF", MAKERREF, ParameterDirection.Input);
            this._dataAccess.AddParameter("DRAWINGNO", DRAWINGNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("POSNO", POSNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTUNITID", PARTUNITID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAKER_ADDRESSID", MAKER_ADDRESSID, ParameterDirection.Input);
            this._dataAccess.AddParameter("MINLVL", MINLVL, ParameterDirection.Input);
            this._dataAccess.AddParameter("MAXLVL", MAXLVL, ParameterDirection.Input);
            this._dataAccess.AddParameter("AVAIL_STOCK", AVAIL_STOCK, ParameterDirection.Input);
            this._dataAccess.AddParameter("STOCKABLE", STOCKABLE, ParameterDirection.Input);
            this._dataAccess.AddParameter("REMARK", REMARK, ParameterDirection.Input);
            this._dataAccess.AddParameter("PARTTYPEID", PARTTYPEID, ParameterDirection.Input);
            this._dataAccess.AddParameter("PART_PRICE_DATE", PART_PRICE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("LASTSUPPLLIER", LASTSUPPLLIER, ParameterDirection.Input);
            this._dataAccess.AddParameter("AVG_PRICE", AVG_PRICE, ParameterDirection.Input);
            this._dataAccess.AddParameter("Parts_Text1", Parts_Text1, ParameterDirection.Input);
            this._dataAccess.AddParameter("Parts_Text2", Parts_Text2, ParameterDirection.Input);
            this._dataAccess.AddParameter("Parts_Text3", Parts_Text3, ParameterDirection.Input);
            this._dataAccess.AddParameter("Parts_Text4", Parts_Text4, ParameterDirection.Input);
            this._dataAccess.AddParameter("PCK_UNITID", PCK_UNITID, ParameterDirection.Input);
            this._dataAccess.AddParameter("PACK_QTY", PACK_QTY, ParameterDirection.Input);
            this._dataAccess.AddParameter("INVENTORYNO", INVENTORYNO, ParameterDirection.Input);
            this._dataAccess.AddParameter("RESERVED_STOCK", RESERVED_STOCK, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", UPDATE_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_DATE", CREATED_DATE, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATED_BY", UPDATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("CREATED_BY", CREATED_BY, ParameterDirection.Input);
            this._dataAccess.AddParameter("PREFERRED_SUPPLIERID", PREFERRED_SUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("DEFAULT_SUPPLIERID", DEFAULT_SUPPLIERID, ParameterDirection.Input);
            this._dataAccess.AddParameter("OWNERID", OWNERID, ParameterDirection.Input);
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
