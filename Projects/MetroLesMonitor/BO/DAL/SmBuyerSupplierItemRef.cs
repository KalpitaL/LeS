namespace MetroLesMonitor.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Xml;
    
    
    public partial class SmBuyerSupplierItemRef : IDisposable {
        
        private DataAccess _dataAccess;
        
        public SmBuyerSupplierItemRef() {
            this._dataAccess = new DataAccess();
        }

        public SmBuyerSupplierItemRef(DataAccess _dataAccess)
        {
            // TODO: Complete member initialization
            this._dataAccess = _dataAccess;
        }

        public virtual System.Data.DataSet GetAllMapping(int SupplierId, int BuyerId)
        {
            string cmdSQL = "SELECT [REFID], [REFTYPE], [BUYER_REF], [SUPPLIER_REF], [ITEM_DESC], [COMMENTS], [BUYER_ID], [SUPPLIER_ID] " +
                " FROM [SM_BUYER_SUPPLIER_ITEM_REF] WHERE (([BUYER_ID] = @BUYER_ID) AND ([SUPPLIER_ID] = @SUPPLIER_ID))" +
                " ORDER BY [REFTYPE],[SUPPLIER_REF]";
            this._dataAccess.CreateSQLCommand(cmdSQL);
            this._dataAccess.AddParameter("SUPPLIER_ID", SupplierId, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_ID", BuyerId, ParameterDirection.Input);

            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet GetAllMapping(int LinkId) 
        {
            string cmdSQL = "SELECT [REFID], [REFTYPE], [BUYER_REF], [SUPPLIER_REF], [ITEM_DESC], [COMMENTS], [BUYER_ID], [SUPPLIER_ID], [BUYER_SUPPLIER_LINKID] " +
                " FROM [SM_BUYER_SUPPLIER_ITEM_REF] WHERE ([BUYER_SUPPLIER_LINKID] = @LINKID)" +
                " ORDER BY [REFTYPE], [SUPPLIER_REF]";
            this._dataAccess.CreateSQLCommand(cmdSQL);
            this._dataAccess.AddParameter("LINKID", LinkId, ParameterDirection.Input);

            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_ITEM_REF_Select_One(System.Nullable<int> REFID)
        {
            string cmdSQL = "SELECT * FROM SM_BUYER_SUPPLIER_ITEM_REF WHERE REFID = @REFID";
            this._dataAccess.CreateSQLCommand(cmdSQL);
            this._dataAccess.AddParameter("REFID", REFID, ParameterDirection.Input);
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }
        
        public virtual System.Data.DataSet SM_BUYER_SUPPLIER_ITEM_REF_Select_All() {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_ITEM_REF_Select_All");
            DataSet value = this._dataAccess.ExecuteDataSet();
            return value;
        }

        public virtual System.Nullable<int> SM_BUYER_SUPPLIER_ITEM_REF_Insert(System.Nullable<int> REFID, string REFTYPE, string BUYER_REF,
            string SUPPLIER_REF, string ITEM_DESC, string COMMENTS, System.Nullable<int> BUYER_ID, System.Nullable<int> SUPPLIER_ID,
           System.Nullable<int> LINKID) 
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_ITEM_REF_Insert");
            //this._dataAccess.AddParameter("REFID", REFID, ParameterDirection.Input);
            this._dataAccess.AddParameter("REFTYPE", REFTYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_REF", BUYER_REF, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_REF", SUPPLIER_REF, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_DESC", ITEM_DESC, ParameterDirection.Input);
            this._dataAccess.AddParameter("COMMENTS", COMMENTS, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_ID", BUYER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_ID", SUPPLIER_ID, ParameterDirection.Input);
            this._dataAccess.AddParameter("UPDATE_DATE", DateTime.Now, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_SUPPLIER_LINKID", LINKID, ParameterDirection.Input);
            //
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_BUYER_SUPPLIER_ITEM_REF_Delete(System.Nullable<int> REFID)
        {
            this._dataAccess.CreateProcedureCommand("sp_SM_BUYER_SUPPLIER_ITEM_REF_Delete");
            this._dataAccess.AddParameter("REFID", REFID, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual System.Nullable<int> SM_BUYER_SUPPLIER_ITEM_REF_Update(System.Nullable<int> REFID, string REFTYPE, string BUYER_REF, string SUPPLIER_REF, string ITEM_DESC, string COMMENTS)
        {
            string cmdSQL = " UPDATE SM_BUYER_SUPPLIER_ITEM_REF SET REFTYPE = @REFTYPE,BUYER_REF = @BUYER_REF, " +
                "SUPPLIER_REF = @SUPPLIER_REF, ITEM_DESC = @ITEM_DESC, COMMENTS = @COMMENTS, UPDATE_DATE = GETDATE() WHERE REFID = @REFID"; 
            this._dataAccess.CreateSQLCommand(cmdSQL);
            this._dataAccess.AddParameter("REFID", REFID, ParameterDirection.Input);
            this._dataAccess.AddParameter("REFTYPE", REFTYPE, ParameterDirection.Input);
            this._dataAccess.AddParameter("BUYER_REF", BUYER_REF, ParameterDirection.Input);
            this._dataAccess.AddParameter("SUPPLIER_REF", SUPPLIER_REF, ParameterDirection.Input);
            this._dataAccess.AddParameter("ITEM_DESC", ITEM_DESC, ParameterDirection.Input);
            this._dataAccess.AddParameter("COMMENTS", COMMENTS, ParameterDirection.Input);
            int value = this._dataAccess.ExecuteNonQuery();
            return value;
        }

        public virtual int SM_BUYER_SUPPLIER_ITEM_REF_DeleteByLinkID(int LINKID) 
        {
            this._dataAccess.CreateSQLCommand("DELETE SM_BUYER_SUPPLIER_ITEM_REF WHERE BUYER_SUPPLIER_LINKID = @LINKID");
            this._dataAccess.AddParameter("LINKID", LINKID, ParameterDirection.Input);
            int val = this._dataAccess.ExecuteNonQuery();
            return val;
        }

        public virtual void Dispose() {
            if ((this._dataAccess != null)) {
                this._dataAccess.Dispose();
            }
        }
    }
}
