using System.Configuration;
using System.Data;
namespace MetroLesMonitor.Bll
{
    public partial class SmInvoicePdfItemMapping {

        static string eInvoiceURL = ConfigurationManager.AppSettings["EINVOICE_WEB_SERVICE"];
        public static DataSetCompressor _compressor = new DataSetCompressor();
        static ServiceCallRoutines _WebServiceCall = new ServiceCallRoutines();
        
        private System.Nullable<int> _itemMapid;
        
        private System.Nullable<int> _invPdfMapid;
        
        private System.Nullable<int> _mapNumber;
        
        private System.Nullable<int> _initialItemSpace;
        
        private System.Nullable<int> _itemHeaderLineCount;
        
        private string _itemHeaderContent;
        
        private string _itemEndContent;
        
        private string _itemEquipment;
        
        private string _itemGroupHeader;
        
        private string _itemNo;
        
        private string _itemQty;
        
        private string _itemUnit;
        
        private string _itemRefno;
        
        private string _itemDescr;
        
        private string _itemRemark;
        
        private string _itemUnitprice;
        
        private string _itemLeaddays;
        
        private string _itemDiscount;
        
        private string _itemTotal;
        
        private System.Nullable<int> _itemMinLines;
        
        private System.Nullable<int> _leaddaysInDate;
        
        private string _itemRemarksAppendText;
        
        private string _itemRemarksInitials;
        
        private System.Nullable<int> _hasNoEquipHeader;
        
        private System.Nullable<int> _maxEquipRows;
        
        private System.Nullable<int> _maxEquipRange;
        
        private System.Nullable<int> _checkContentBelowItem;
        
        private string _extraColumns;
        
        private string _extraColumnsHeader;
        
        private System.Nullable<int> _readItemnoUptoMinlines;
        
        private string _equipNameRange;
        
        private string _equipTypeRange;
        
        private string _equipSernoRange;
        
        private string _equipMakerRange;
        
        private string _equipNoteRange;
        
        private System.Nullable<int> _appendUom;
        
        private System.Nullable<int> _makerrefExtranoLineCount;
        
        private string _makerrefRange;
        
        private string _extranoRange;
        
        private System.Nullable<int> _readPartnoFromLastLine;
        
        private string _itemCurrency;
        
        private System.Nullable<System.DateTime> _updateDate;
        
        private System.Nullable<int> _appendRefNo;
        
        private System.Nullable<int> _isBoldText;

        private System.Nullable<int> _isQtyUomMerged;
        
        public virtual System.Nullable<int> ItemMapid {
            get {
                return _itemMapid;
            }
            set {
                _itemMapid = value;
            }
        }
        
        public virtual System.Nullable<int> InvPdfMapid {
            get {
                return _invPdfMapid;
            }
            set {
                _invPdfMapid = value;
            }
        }
        
        public virtual System.Nullable<int> MapNumber {
            get {
                return _mapNumber;
            }
            set {
                _mapNumber = value;
            }
        }
        
        public virtual System.Nullable<int> InitialItemSpace {
            get {
                return _initialItemSpace;
            }
            set {
                _initialItemSpace = value;
            }
        }
        
        public virtual System.Nullable<int> ItemHeaderLineCount {
            get {
                return _itemHeaderLineCount;
            }
            set {
                _itemHeaderLineCount = value;
            }
        }
        
        public virtual string ItemHeaderContent {
            get {
                return _itemHeaderContent;
            }
            set {
                _itemHeaderContent = value;
            }
        }
        
        public virtual string ItemEndContent {
            get {
                return _itemEndContent;
            }
            set {
                _itemEndContent = value;
            }
        }
        
        public virtual string ItemEquipment {
            get {
                return _itemEquipment;
            }
            set {
                _itemEquipment = value;
            }
        }
        
        public virtual string ItemGroupHeader {
            get {
                return _itemGroupHeader;
            }
            set {
                _itemGroupHeader = value;
            }
        }
        
        public virtual string ItemNo {
            get {
                return _itemNo;
            }
            set {
                _itemNo = value;
            }
        }
        
        public virtual string ItemQty {
            get {
                return _itemQty;
            }
            set {
                _itemQty = value;
            }
        }
        
        public virtual string ItemUnit {
            get {
                return _itemUnit;
            }
            set {
                _itemUnit = value;
            }
        }
        
        public virtual string ItemRefno {
            get {
                return _itemRefno;
            }
            set {
                _itemRefno = value;
            }
        }
        
        public virtual string ItemDescr {
            get {
                return _itemDescr;
            }
            set {
                _itemDescr = value;
            }
        }
        
        public virtual string ItemRemark {
            get {
                return _itemRemark;
            }
            set {
                _itemRemark = value;
            }
        }
        
        public virtual string ItemUnitprice {
            get {
                return _itemUnitprice;
            }
            set {
                _itemUnitprice = value;
            }
        }
        
        public virtual string ItemLeaddays {
            get {
                return _itemLeaddays;
            }
            set {
                _itemLeaddays = value;
            }
        }
        
        public virtual string ItemDiscount {
            get {
                return _itemDiscount;
            }
            set {
                _itemDiscount = value;
            }
        }
        
        public virtual string ItemTotal {
            get {
                return _itemTotal;
            }
            set {
                _itemTotal = value;
            }
        }
        
        public virtual System.Nullable<int> ItemMinLines {
            get {
                return _itemMinLines;
            }
            set {
                _itemMinLines = value;
            }
        }
        
        public virtual System.Nullable<int> LeaddaysInDate {
            get {
                return _leaddaysInDate;
            }
            set {
                _leaddaysInDate = value;
            }
        }
        
        public virtual string ItemRemarksAppendText {
            get {
                return _itemRemarksAppendText;
            }
            set {
                _itemRemarksAppendText = value;
            }
        }
        
        public virtual string ItemRemarksInitials {
            get {
                return _itemRemarksInitials;
            }
            set {
                _itemRemarksInitials = value;
            }
        }
        
        public virtual System.Nullable<int> HasNoEquipHeader {
            get {
                return _hasNoEquipHeader;
            }
            set {
                _hasNoEquipHeader = value;
            }
        }
        
        public virtual System.Nullable<int> MaxEquipRows {
            get {
                return _maxEquipRows;
            }
            set {
                _maxEquipRows = value;
            }
        }
        
        public virtual System.Nullable<int> MaxEquipRange {
            get {
                return _maxEquipRange;
            }
            set {
                _maxEquipRange = value;
            }
        }
        
        public virtual System.Nullable<int> CheckContentBelowItem {
            get {
                return _checkContentBelowItem;
            }
            set {
                _checkContentBelowItem = value;
            }
        }
        
        public virtual string ExtraColumns {
            get {
                return _extraColumns;
            }
            set {
                _extraColumns = value;
            }
        }
        
        public virtual string ExtraColumnsHeader {
            get {
                return _extraColumnsHeader;
            }
            set {
                _extraColumnsHeader = value;
            }
        }
        
        public virtual System.Nullable<int> ReadItemnoUptoMinlines {
            get {
                return _readItemnoUptoMinlines;
            }
            set {
                _readItemnoUptoMinlines = value;
            }
        }
        
        public virtual string EquipNameRange {
            get {
                return _equipNameRange;
            }
            set {
                _equipNameRange = value;
            }
        }
        
        public virtual string EquipTypeRange {
            get {
                return _equipTypeRange;
            }
            set {
                _equipTypeRange = value;
            }
        }
        
        public virtual string EquipSernoRange {
            get {
                return _equipSernoRange;
            }
            set {
                _equipSernoRange = value;
            }
        }
        
        public virtual string EquipMakerRange {
            get {
                return _equipMakerRange;
            }
            set {
                _equipMakerRange = value;
            }
        }
        
        public virtual string EquipNoteRange {
            get {
                return _equipNoteRange;
            }
            set {
                _equipNoteRange = value;
            }
        }
        
        public virtual System.Nullable<int> AppendUom {
            get {
                return _appendUom;
            }
            set {
                _appendUom = value;
            }
        }
        
        public virtual System.Nullable<int> MakerrefExtranoLineCount {
            get {
                return _makerrefExtranoLineCount;
            }
            set {
                _makerrefExtranoLineCount = value;
            }
        }
        
        public virtual string MakerrefRange {
            get {
                return _makerrefRange;
            }
            set {
                _makerrefRange = value;
            }
        }
        
        public virtual string ExtranoRange {
            get {
                return _extranoRange;
            }
            set {
                _extranoRange = value;
            }
        }
        
        public virtual System.Nullable<int> ReadPartnoFromLastLine {
            get {
                return _readPartnoFromLastLine;
            }
            set {
                _readPartnoFromLastLine = value;
            }
        }
        
        public virtual string ItemCurrency {
            get {
                return _itemCurrency;
            }
            set {
                _itemCurrency = value;
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
        
        public virtual System.Nullable<int> AppendRefNo {
            get {
                return _appendRefNo;
            }
            set {
                _appendRefNo = value;
            }
        }
        
        public virtual System.Nullable<int> IsBoldText {
            get {
                return _isBoldText;
            }
            set {
                _isBoldText = value;
            }
        }

        public virtual System.Nullable<int> IsQtyUomMerged
        {
            get
            {
                return _isQtyUomMerged;
            }
            set
            {
                _isQtyUomMerged = value;
            }
        }
        
        private void Clean() {
            this.ItemMapid = null;
            this.InvPdfMapid = null;
            this.MapNumber = null;
            this.InitialItemSpace = null;
            this.ItemHeaderLineCount = null;
            this.ItemHeaderContent = string.Empty;
            this.ItemEndContent = string.Empty;
            this.ItemEquipment = string.Empty;
            this.ItemGroupHeader = string.Empty;
            this.ItemNo = string.Empty;
            this.ItemQty = string.Empty;
            this.ItemUnit = string.Empty;
            this.ItemRefno = string.Empty;
            this.ItemDescr = string.Empty;
            this.ItemRemark = string.Empty;
            this.ItemUnitprice = string.Empty;
            this.ItemLeaddays = string.Empty;
            this.ItemDiscount = string.Empty;
            this.ItemTotal = string.Empty;
            this.ItemMinLines = null;
            this.LeaddaysInDate = null;
            this.ItemRemarksAppendText = string.Empty;
            this.ItemRemarksInitials = string.Empty;
            this.HasNoEquipHeader = null;
            this.MaxEquipRows = null;
            this.MaxEquipRange = null;
            this.CheckContentBelowItem = null;
            this.ExtraColumns = string.Empty;
            this.ExtraColumnsHeader = string.Empty;
            this.ReadItemnoUptoMinlines = null;
            this.EquipNameRange = string.Empty;
            this.EquipTypeRange = string.Empty;
            this.EquipSernoRange = string.Empty;
            this.EquipMakerRange = string.Empty;
            this.EquipNoteRange = string.Empty;
            this.AppendUom = null;
            this.MakerrefExtranoLineCount = null;
            this.MakerrefRange = string.Empty;
            this.ExtranoRange = string.Empty;
            this.ReadPartnoFromLastLine = null;
            this.ItemCurrency = string.Empty;
            this.UpdateDate = null;
            this.AppendRefNo = null;
            this.IsBoldText = null;
            this.IsQtyUomMerged = null;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["ITEM_MAPID"] != System.DBNull.Value)) {
                this.ItemMapid = ((System.Nullable<int>)(dr["ITEM_MAPID"]));
            }
            if ((dr["INV_PDF_MAPID"] != System.DBNull.Value)) {
                this.InvPdfMapid = ((System.Nullable<int>)(dr["INV_PDF_MAPID"]));
            }
            if ((dr["MAP_NUMBER"] != System.DBNull.Value)) {
                this.MapNumber = ((System.Nullable<int>)(dr["MAP_NUMBER"]));
            }
            if ((dr["INITIAL_ITEM_SPACE"] != System.DBNull.Value)) {
                this.InitialItemSpace = ((System.Nullable<int>)(dr["INITIAL_ITEM_SPACE"]));
            }
            if ((dr["ITEM_HEADER_LINE_COUNT"] != System.DBNull.Value)) {
                this.ItemHeaderLineCount = ((System.Nullable<int>)(dr["ITEM_HEADER_LINE_COUNT"]));
            }
            if ((dr["ITEM_HEADER_CONTENT"] != System.DBNull.Value)) {
                this.ItemHeaderContent = ((string)(dr["ITEM_HEADER_CONTENT"]));
            }
            if ((dr["ITEM_END_CONTENT"] != System.DBNull.Value)) {
                this.ItemEndContent = ((string)(dr["ITEM_END_CONTENT"]));
            }
            if ((dr["ITEM_EQUIPMENT"] != System.DBNull.Value)) {
                this.ItemEquipment = ((string)(dr["ITEM_EQUIPMENT"]));
            }
            if ((dr["ITEM_GROUP_HEADER"] != System.DBNull.Value)) {
                this.ItemGroupHeader = ((string)(dr["ITEM_GROUP_HEADER"]));
            }
            if ((dr["ITEM_NO"] != System.DBNull.Value)) {
                this.ItemNo = ((string)(dr["ITEM_NO"]));
            }
            if ((dr["ITEM_QTY"] != System.DBNull.Value)) {
                this.ItemQty = ((string)(dr["ITEM_QTY"]));
            }
            if ((dr["ITEM_UNIT"] != System.DBNull.Value)) {
                this.ItemUnit = ((string)(dr["ITEM_UNIT"]));
            }
            if ((dr["ITEM_REFNO"] != System.DBNull.Value)) {
                this.ItemRefno = ((string)(dr["ITEM_REFNO"]));
            }
            if ((dr["ITEM_DESCR"] != System.DBNull.Value)) {
                this.ItemDescr = ((string)(dr["ITEM_DESCR"]));
            }
            if ((dr["ITEM_REMARK"] != System.DBNull.Value)) {
                this.ItemRemark = ((string)(dr["ITEM_REMARK"]));
            }
            if ((dr["ITEM_UNITPRICE"] != System.DBNull.Value)) {
                this.ItemUnitprice = ((string)(dr["ITEM_UNITPRICE"]));
            }
            if ((dr["ITEM_LEADDAYS"] != System.DBNull.Value)) {
                this.ItemLeaddays = ((string)(dr["ITEM_LEADDAYS"]));
            }
            if ((dr["ITEM_DISCOUNT"] != System.DBNull.Value)) {
                this.ItemDiscount = ((string)(dr["ITEM_DISCOUNT"]));
            }
            if ((dr["ITEM_TOTAL"] != System.DBNull.Value)) {
                this.ItemTotal = ((string)(dr["ITEM_TOTAL"]));
            }
            if ((dr["ITEM_MIN_LINES"] != System.DBNull.Value)) {
                this.ItemMinLines = ((System.Nullable<int>)(dr["ITEM_MIN_LINES"]));
            }
            if ((dr["LEADDAYS_IN_DATE"] != System.DBNull.Value)) {
                this.LeaddaysInDate = ((System.Nullable<int>)(dr["LEADDAYS_IN_DATE"]));
            }
            if ((dr["ITEM_REMARKS_APPEND_TEXT"] != System.DBNull.Value)) {
                this.ItemRemarksAppendText = ((string)(dr["ITEM_REMARKS_APPEND_TEXT"]));
            }
            if ((dr["ITEM_REMARKS_INITIALS"] != System.DBNull.Value)) {
                this.ItemRemarksInitials = ((string)(dr["ITEM_REMARKS_INITIALS"]));
            }
            if ((dr["HAS_NO_EQUIP_HEADER"] != System.DBNull.Value)) {
                this.HasNoEquipHeader = ((System.Nullable<int>)(dr["HAS_NO_EQUIP_HEADER"]));
            }
            if ((dr["MAX_EQUIP_ROWS"] != System.DBNull.Value)) {
                this.MaxEquipRows = ((System.Nullable<int>)(dr["MAX_EQUIP_ROWS"]));
            }
            if ((dr["MAX_EQUIP_RANGE"] != System.DBNull.Value)) {
                this.MaxEquipRange = ((System.Nullable<int>)(dr["MAX_EQUIP_RANGE"]));
            }
            if ((dr["CHECK_CONTENT_BELOW_ITEM"] != System.DBNull.Value)) {
                this.CheckContentBelowItem = ((System.Nullable<int>)(dr["CHECK_CONTENT_BELOW_ITEM"]));
            }
            if ((dr["EXTRA_COLUMNS"] != System.DBNull.Value)) {
                this.ExtraColumns = ((string)(dr["EXTRA_COLUMNS"]));
            }
            if ((dr["EXTRA_COLUMNS_HEADER"] != System.DBNull.Value)) {
                this.ExtraColumnsHeader = ((string)(dr["EXTRA_COLUMNS_HEADER"]));
            }
            if ((dr["READ_ITEMNO_UPTO_MINLINES"] != System.DBNull.Value)) {
                this.ReadItemnoUptoMinlines = ((System.Nullable<int>)(dr["READ_ITEMNO_UPTO_MINLINES"]));
            }
            if ((dr["EQUIP_NAME_RANGE"] != System.DBNull.Value)) {
                this.EquipNameRange = ((string)(dr["EQUIP_NAME_RANGE"]));
            }
            if ((dr["EQUIP_TYPE_RANGE"] != System.DBNull.Value)) {
                this.EquipTypeRange = ((string)(dr["EQUIP_TYPE_RANGE"]));
            }
            if ((dr["EQUIP_SERNO_RANGE"] != System.DBNull.Value)) {
                this.EquipSernoRange = ((string)(dr["EQUIP_SERNO_RANGE"]));
            }
            if ((dr["EQUIP_MAKER_RANGE"] != System.DBNull.Value)) {
                this.EquipMakerRange = ((string)(dr["EQUIP_MAKER_RANGE"]));
            }
            if ((dr["EQUIP_NOTE_RANGE"] != System.DBNull.Value)) {
                this.EquipNoteRange = ((string)(dr["EQUIP_NOTE_RANGE"]));
            }
            if ((dr["APPEND_UOM"] != System.DBNull.Value)) {
                this.AppendUom = ((System.Nullable<int>)(dr["APPEND_UOM"]));
            }
            if ((dr["MAKERREF_EXTRANO_LINE_COUNT"] != System.DBNull.Value)) {
                this.MakerrefExtranoLineCount = ((System.Nullable<int>)(dr["MAKERREF_EXTRANO_LINE_COUNT"]));
            }
            if ((dr["MAKERREF_RANGE"] != System.DBNull.Value)) {
                this.MakerrefRange = ((string)(dr["MAKERREF_RANGE"]));
            }
            if ((dr["EXTRANO_RANGE"] != System.DBNull.Value)) {
                this.ExtranoRange = ((string)(dr["EXTRANO_RANGE"]));
            }
            if ((dr["READ_PARTNO_FROM_LAST_LINE"] != System.DBNull.Value)) {
                this.ReadPartnoFromLastLine = ((System.Nullable<int>)(dr["READ_PARTNO_FROM_LAST_LINE"]));
            }
            if ((dr["ITEM_CURRENCY"] != System.DBNull.Value)) {
                this.ItemCurrency = ((string)(dr["ITEM_CURRENCY"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value)) {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
            if ((dr["APPEND_REF_NO"] != System.DBNull.Value)) {
                this.AppendRefNo = ((System.Nullable<int>)(dr["APPEND_REF_NO"]));
            }
            if ((dr["IS_BOLD_TEXT"] != System.DBNull.Value)) {
                this.IsBoldText = ((System.Nullable<int>)(dr["IS_BOLD_TEXT"]));
            }
            if ((dr["IS_QTY_UOM_MERGED"] != System.DBNull.Value))
            {
                this.IsQtyUomMerged = ((System.Nullable<int>)(dr["IS_QTY_UOM_MERGED"]));
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

        public static SmInvoicePdfItemMapping Load(System.Nullable<int> INV_PDF_MAPID, System.Nullable<int> MAP_NUMBER)
        {
            SmInvoicePdfItemMapping dbo = null;
            try
            {
                System.Data.DataSet ds = SM_INV_PDF_ITEM_MAPPING_Select_One_By_PDFMAPID_MAPNUMBER(INV_PDF_MAPID, MAP_NUMBER);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        dbo = new SmInvoicePdfItemMapping();
                        dbo.Fill(ds.Tables[0].Rows[0]);
                    }
                }
                return dbo;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
            }
        }

        public static DataSet SM_INV_PDF_ITEM_MAPPING_Select_One_By_PDFMAPID_MAPNUMBER(System.Nullable<int> INV_PDF_MAPID, System.Nullable<int> MAP_NUMBER)
        {
            DataSet _ds = new DataSet();
            object[] _obj = new object[2];
            string[] _paramObj = new string[2];
            string cSql = "Select * from SM_INVOICE_PDF_ITEM_MAPPING WHERE INV_PDF_MAPID=@INV_PDF_MAPID AND MAP_NUMBER =@MAP_NUMBER";
            _obj[0] = cSql;
            _paramObj[0] = "INV_PDF_MAPID=" + INV_PDF_MAPID;
            _paramObj[1] = "MAP_NUMBER=" + MAP_NUMBER;
            _obj[1] = _paramObj;
            byte[] _byteData = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _obj);
            DataSet _dsInvItemMapping = _compressor.Decompress(_byteData);
            return _dsInvItemMapping;
        }

        public static DataSet SM_INVOICE_PDF_ITEM_MAPPING_Select_By_InvPdfMapid(System.Nullable<int> INV_PDF_MAPID)
        {
            DataSet _ds = new DataSet();
            object[] _obj = new object[2];
            string[] _paramObj = new string[1];
            string cSql = "Select * from SM_INVOICE_PDF_ITEM_MAPPING WHERE INV_PDF_MAPID=@INV_PDF_MAPID";
            _obj[0] = cSql;
            _paramObj[0] = "INV_PDF_MAPID=" + INV_PDF_MAPID;
            _obj[1] = _paramObj;
            byte[] _byteData = (byte[])_WebServiceCall.CallWebService(eInvoiceURL, "LesInvoiceService", "GetQueryDataParam", _obj);
            DataSet _dsInvItemMapping = _compressor.Decompress(_byteData);
            return _dsInvItemMapping;
        }

        public static DataSet ExportItems(SmInvoicePdfItemMappingCollection ItemColl)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("ITEM_MAPID", typeof(int));
            dt.Columns.Add("INV_PDF_MAPID", typeof(int));
            dt.Columns.Add("MAP_NUMBER", typeof(int));
            dt.Columns.Add("INITIAL_ITEM_SPACE", typeof(int));
            dt.Columns.Add("ITEM_HEADER_LINE_COUNT", typeof(int));
            dt.Columns.Add("ITEM_HEADER_CONTENT");
            dt.Columns.Add("ITEM_END_CONTENT");
            dt.Columns.Add("ITEM_EQUIPMENT");
            dt.Columns.Add("ITEM_GROUP_HEADER");
            dt.Columns.Add("ITEM_NO");
            dt.Columns.Add("ITEM_QTY");
            dt.Columns.Add("ITEM_UNIT");
            dt.Columns.Add("ITEM_REFNO");
            dt.Columns.Add("ITEM_DESCR");
            dt.Columns.Add("ITEM_REMARK");
            dt.Columns.Add("ITEM_UNITPRICE");
            dt.Columns.Add("ITEM_LEADDAYS");
            dt.Columns.Add("ITEM_DISCOUNT");
            dt.Columns.Add("ITEM_TOTAL");
            dt.Columns.Add("ITEM_MIN_LINES", typeof(int));
            dt.Columns.Add("LEADDAYS_IN_DATE", typeof(int));
            dt.Columns.Add("ITEM_REMARKS_APPEND_TEXT");
            dt.Columns.Add("ITEM_REMARKS_INITIALS");
            dt.Columns.Add("HAS_NO_EQUIP_HEADER", typeof(int));
            dt.Columns.Add("MAX_EQUIP_ROWS", typeof(int));
            dt.Columns.Add("MAX_EQUIP_RANGE", typeof(int));
            dt.Columns.Add("CHECK_CONTENT_BELOW_ITEM", typeof(int));
            dt.Columns.Add("EXTRA_COLUMNS");
            dt.Columns.Add("EXTRA_COLUMNS_HEADER");
            dt.Columns.Add("READ_ITEMNO_UPTO_MINLINES", typeof(int));
            dt.Columns.Add("EQUIP_NAME_RANGE");
            dt.Columns.Add("EQUIP_TYPE_RANGE");
            dt.Columns.Add("EQUIP_SERNO_RANGE");
            dt.Columns.Add("EQUIP_MAKER_RANGE");
            dt.Columns.Add("EQUIP_NOTE_RANGE");
            dt.Columns.Add("APPEND_UOM", typeof(int));
            dt.Columns.Add("MAKERREF_EXTRANO_LINE_COUNT", typeof(int));
            dt.Columns.Add("MAKERREF_RANGE");
            dt.Columns.Add("EXTRANO_RANGE");
            dt.Columns.Add("READ_PARTNO_FROM_LAST_LINE", typeof(int));
            dt.Columns.Add("ITEM_CURRENCY");
            dt.Columns.Add("UPDATE_DATE");
            dt.Columns.Add("APPEND_REF_NO", typeof(int));
            dt.Columns.Add("IS_BOLD_TEXT", typeof(int));
            dt.Columns.Add("IS_QTY_UOM_MERGED", typeof(int));

            if (ItemColl.Count > 0)
            {
                foreach (SmInvoicePdfItemMapping _items in ItemColl)
                {
                    DataRow dr = dt.NewRow();
                    dr["ITEM_MAPID"] = convert.ToInt(_items.ItemMapid);
                    dr["INV_PDF_MAPID"] = convert.ToInt(_items.InvPdfMapid);
                    dr["MAP_NUMBER"] = convert.ToInt(_items.MapNumber);
                    dr["INITIAL_ITEM_SPACE"] = convert.ToInt(_items.InitialItemSpace);
                    dr["ITEM_HEADER_LINE_COUNT"] = _items.ItemHeaderLineCount;
                    dr["ITEM_HEADER_CONTENT"] = _items.ItemHeaderContent;
                    dr["ITEM_END_CONTENT"] = _items.ItemEndContent;
                    dr["ITEM_EQUIPMENT"] = _items.ItemEquipment;
                    dr["ITEM_GROUP_HEADER"] = _items.ItemGroupHeader;
                    dr["ITEM_NO"] = _items.ItemNo;
                    dr["ITEM_QTY"] = _items.ItemQty;
                    dr["ITEM_UNIT"] = _items.ItemUnit;
                    dr["ITEM_REFNO"] = _items.ItemRefno;
                    dr["ITEM_DESCR"] = _items.ItemDescr;
                    dr["ITEM_REMARK"] = _items.ItemRemark;
                    dr["ITEM_UNITPRICE"] = _items.ItemUnitprice;
                    dr["ITEM_LEADDAYS"] = _items.ItemLeaddays;
                    dr["ITEM_DISCOUNT"] = _items.ItemDiscount;
                    dr["ITEM_TOTAL"] = _items.ItemTotal;
                    dr["ITEM_MIN_LINES"] = convert.ToInt(_items.ItemMinLines);
                    dr["LEADDAYS_IN_DATE"] = convert.ToInt(_items.LeaddaysInDate);
                    dr["ITEM_REMARKS_APPEND_TEXT"] = _items.ItemRemarksAppendText;
                    dr["ITEM_REMARKS_INITIALS"] = _items.ItemRemarksInitials;
                    dr["HAS_NO_EQUIP_HEADER"] =convert.ToInt( _items.HasNoEquipHeader);
                    dr["MAX_EQUIP_ROWS"] =convert.ToInt( _items.MaxEquipRows);
                    dr["MAX_EQUIP_RANGE"] = convert.ToInt(_items.MaxEquipRange);
                    dr["CHECK_CONTENT_BELOW_ITEM"] = convert.ToInt(_items.CheckContentBelowItem);
                    dr["EXTRA_COLUMNS"] = _items.ExtraColumns;
                    dr["EXTRA_COLUMNS_HEADER"] = _items.ExtraColumnsHeader;
                    dr["READ_ITEMNO_UPTO_MINLINES"] =convert.ToInt( _items.ReadItemnoUptoMinlines);
                    dr["EQUIP_NAME_RANGE"] = _items.EquipNameRange;
                    dr["EQUIP_TYPE_RANGE"] = _items.EquipTypeRange;
                    dr["EQUIP_SERNO_RANGE"] = _items.EquipSernoRange;
                    dr["EQUIP_MAKER_RANGE"] = _items.EquipMakerRange;
                    dr["EQUIP_NOTE_RANGE"] = _items.EquipNoteRange;
                    dr["APPEND_UOM"] = convert.ToInt(_items.AppendUom);
                    dr["MAKERREF_EXTRANO_LINE_COUNT"] = convert.ToInt(_items.MakerrefExtranoLineCount);
                    dr["MAKERREF_RANGE"] = _items.MakerrefRange;
                    dr["EXTRANO_RANGE"] = _items.ExtranoRange;
                    dr["READ_PARTNO_FROM_LAST_LINE"] = convert.ToInt(_items.ReadPartnoFromLastLine);
                    dr["ITEM_CURRENCY"] = _items.ItemCurrency;
                    dr["UPDATE_DATE"] = _items.UpdateDate;
                    dr["APPEND_REF_NO"] = convert.ToInt(_items.AppendRefNo);
                    dr["IS_BOLD_TEXT"] = convert.ToInt(_items.IsBoldText);
                    dr["IS_QTY_UOM_MERGED"] = convert.ToInt(_items.IsQtyUomMerged);
                    dt.Rows.Add(dr);
                }
                ds.Tables.Add(dt);
            }
            return ds;
        }

        public static SmInvoicePdfItemMappingCollection Load_by_InvPdfMapid(System.Nullable<int> INV_PDF_MAPID)
        {
            try
            {
                System.Data.DataSet ds = SM_INVOICE_PDF_ITEM_MAPPING_Select_By_InvPdfMapid(INV_PDF_MAPID);
                SmInvoicePdfItemMappingCollection collection = new SmInvoicePdfItemMappingCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmInvoicePdfItemMapping obj = new SmInvoicePdfItemMapping();
                        obj.Fill(ds.Tables[0].Rows[i]);
                        if ((obj != null))
                        {
                            collection.Add(obj);
                        }
                    }
                }
                return collection;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
            }
        }

    }
}
