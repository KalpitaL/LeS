using MetroLesMonitor.Bll;
using System;
using System.Data;
namespace MetroLesMonitor.Bll
{

    public partial class SmInvPdfBuyerSupplierLink {
        
        private System.Nullable<int> _mapId;
        
        private System.Nullable<int> _invPdfMapid;
        
        private string _mapping1;
        
        private string _mapping1Value;
        
        private string _mapping2;
        
        private string _mapping2Value;
        
        private string _mapping3;
        
        private string _mapping3Value;
        
        private System.Nullable<System.DateTime> _createdDate;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        private System.Nullable<int> _supplierid;
        
        private SmInvoicePdfMapping _smInvoicePdfMapping;
        
        public virtual System.Nullable<int> MapId {
            get {
                return _mapId;
            }
            set {
                _mapId = value;
            }
        }
        
        public virtual string Mapping1 {
            get {
                return _mapping1;
            }
            set {
                _mapping1 = value;
            }
        }
        
        public virtual string Mapping1Value {
            get {
                return _mapping1Value;
            }
            set {
                _mapping1Value = value;
            }
        }
        
        public virtual string Mapping2 {
            get {
                return _mapping2;
            }
            set {
                _mapping2 = value;
            }
        }
        
        public virtual string Mapping2Value {
            get {
                return _mapping2Value;
            }
            set {
                _mapping2Value = value;
            }
        }
        
        public virtual string Mapping3 {
            get {
                return _mapping3;
            }
            set {
                _mapping3 = value;
            }
        }
        
        public virtual string Mapping3Value {
            get {
                return _mapping3Value;
            }
            set {
                _mapping3Value = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> CreatedDate {
            get {
                return _createdDate;
            }
            set {
                _createdDate = value;
            }
        }
        
        public virtual System.Nullable<System.DateTime> UpdateDate {
            get {
                return _updateDate;
            }
            set {
                _updateDate = value;
            }
        }
        
        public virtual System.Nullable<int> Supplierid {
            get {
                return _supplierid;
            }
            set {
                _supplierid = value;
            }
        }
        
        private void Clean() {
            this.MapId = null;
            this._invPdfMapid = null;
            this.Mapping1 = string.Empty;
            this.Mapping1Value = string.Empty;
            this.Mapping2 = string.Empty;
            this.Mapping2Value = string.Empty;
            this.Mapping3 = string.Empty;
            this.Mapping3Value = string.Empty;
            this.CreatedDate = null;
            this.UpdateDate = null;
            this.Supplierid = null;
           
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["MAP_ID"] != System.DBNull.Value)) {
                this.MapId = ((System.Nullable<int>)(dr["MAP_ID"]));
            }
            if ((dr["INV_PDF_MAPID"] != System.DBNull.Value)) {
                this._invPdfMapid = ((System.Nullable<int>)(dr["INV_PDF_MAPID"]));
            }
            if ((dr["MAPPING_1"] != System.DBNull.Value)) {
                this.Mapping1 = ((string)(dr["MAPPING_1"]));
            }
            if ((dr["MAPPING_1_VALUE"] != System.DBNull.Value)) {
                this.Mapping1Value = ((string)(dr["MAPPING_1_VALUE"]));
            }
            if ((dr["MAPPING_2"] != System.DBNull.Value)) {
                this.Mapping2 = ((string)(dr["MAPPING_2"]));
            }
            if ((dr["MAPPING_2_VALUE"] != System.DBNull.Value)) {
                this.Mapping2Value = ((string)(dr["MAPPING_2_VALUE"]));
            }
            if ((dr["MAPPING_3"] != System.DBNull.Value)) {
                this.Mapping3 = ((string)(dr["MAPPING_3"]));
            }
            if ((dr["MAPPING_3_VALUE"] != System.DBNull.Value)) {
                this.Mapping3Value = ((string)(dr["MAPPING_3_VALUE"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value)) {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value)) {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
            if ((dr["SUPPLIERID"] != System.DBNull.Value)) {
                this.Supplierid = ((System.Nullable<int>)(dr["SUPPLIERID"]));
            }
        }

        public virtual void Load(DataRow dr)
        {
            try
            {
                if ((dr != null))
                {
                    this.Fill(dr);
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {

            }
        }

        public DataSet ExportHeader()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("MAP_ID", typeof(int));
            dt.Columns.Add("INV_PDF_MAPID", typeof(int));
            dt.Columns.Add("MAPPING_1");
            dt.Columns.Add("MAPPING_1_VALUE");
            dt.Columns.Add("MAPPING_2");
            dt.Columns.Add("MAPPING_2_VALUE");
            dt.Columns.Add("MAPPING_3");
            dt.Columns.Add("MAPPING_3_VALUE");
            dt.Columns.Add("CREATED_DATE", typeof(DateTime));
            dt.Columns.Add("UPDATE_DATE", typeof(DateTime));
            dt.Columns.Add("SUPPLIERID", typeof(int));

            DataRow dr = dt.NewRow();

            dr["MAP_ID"] = convert.ToInt(this.MapId);
            dr["INV_PDF_MAPID"] = convert.ToInt(this._invPdfMapid);
            dr["MAPPING_1"] = this.Mapping1;
            dr["MAPPING_1_VALUE"] = this.Mapping1Value;
            dr["MAPPING_2"] = this.Mapping2;
            dr["MAPPING_2_VALUE"] = this.Mapping2Value;
            dr["MAPPING_3"] = this.Mapping3;
            dr["MAPPING_3_VALUE"] = this.Mapping3Value;
            dr["CREATED_DATE"] = this.CreatedDate;
            dr["UPDATE_DATE"] = this.UpdateDate;
            dr["SUPPLIERID"] = convert.ToInt(this.Supplierid);
            dt.Rows.Add(dr);
            ds.Tables.Add(dt);
            return ds;
        }
        
    }
}
