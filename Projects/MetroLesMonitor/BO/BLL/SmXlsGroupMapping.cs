namespace MetroLesMonitor.Bll
{
    public partial class SmXlsGroupMapping
    {
        private System.Nullable<int> _excelMapid;

        private System.Nullable<int> _groupId;

        private string _xlsMapCode;

        private System.Nullable<int> _sectionRowStart;

        private System.Nullable<int> _itemRowStart;

        private System.Nullable<int> _skipRowsBefItem;

        private System.Nullable<int> _skipRowsAftSection;

        private string _cellVrno;

        private string _cellRfqDt;

        private string _cellVessel;

        private string _cellPort;

        private string _cellLateDt;

        private string _cellSuppRef;

        private string _cellValidUpto;

        private string _cellCurrCode;

        private string _cellContact;

        private string _cellPayTerms;

        private string _cellDelTerms;

        private string _cellBuyerRemarks;

        private string _cellSuplrRemarks;

        private string _colSection;

        private string _colItemno;

        private string _colItemRefno;

        private string _colItemName;

        private string _colItemQty;

        private string _colItemUnit;

        private string _colItemPrice;

        private string _colItemDiscount;

        private string _colItemAltQty;

        private string _colItemAltUnit;

        private string _colItemAltPrice;

        private string _colItemDelDays;

        private string _colItemRemarks;

        private System.Nullable<int> _activeSheet;

        private System.Nullable<short> _exitForNoitem;

        private string _colItemCurr;

        private System.Nullable<int> _dynSupRmrkOffset;

        private System.Nullable<int> _overrideAltQty;

        private System.Nullable<int> _skipHiddenRows;

        private System.Nullable<int> _itemDiscPercnt;

        private string _colItemTotal;

        private System.Nullable<int> _applyTotalFormula;

        private System.Nullable<int> _readItemRemarksUptoNo;

        private string _colItemBuyerRemarks;

        private string _addToVrno;

        private string _removeFromVrno;

        private System.Nullable<int> _skipRowsAftItem;

        private System.Nullable<int> _itemNoAsRowno;

        private string _colItemComments;

        private string _sampleFile;

        private string _cellVslImono;

        private string _cellPortName;

        private string _cellDocType;

        private string _colItemSuppRefno;

        private string _cellSuppExpDt;

        private string _cellSuppLateDt;

        private string _cellSuppLeadDays;

        private string _cellByrCompany;

        private string _cellByrContact;

        private string _cellByrEmail;

        private string _cellByrPhone;

        private string _cellByrMob;

        private string _cellByrFax;

        private string _cellSuppCompany;

        private string _cellSuppContact;

        private string _cellSuppEmail;

        private string _cellSuppPhone;

        private string _cellSuppMob;

        private string _cellSuppFax;

        private string _cellFreightAmt;

        private string _cellOtherAmt;

        // Added on 10-MARCH-2015
        private string _cellDiscProvsn;

        private string _discProvsnValue;

        private System.Nullable<int> _altItemStartOffset;

        private System.Nullable<int> _altItemCount;

        private string _cellRfqTitle;

        private string _cellRfqDept;

        private string _cellEquipName;

        private string _cellEquipType;

        private string _cellEquipMaker;

        private string _cellEquipSrno;

        private string _cellEquipDtls;

        private string _cellMsgNo;

        private System.Nullable<int> _dynSupFreightOffset;

        private System.Nullable<int> _dynOtherCostOffset;

        private System.Nullable<int> _dynSupCurrOffset;

        private System.Nullable<int> _dynBuyRmrkOffset;

        private System.Nullable<int> _multilineItemDesc;

        private string _excelNameMgr;

        private string _decimalSeprator;

        private string _defaultUMO;

        private string _REMOVE_FROM_VESSEL_NAME;
        private string _CELL_BYR_ADDR1;
        private string _CELL_BYR_ADDR2;
        private string _CELL_SUPP_ADDR1;
        private string _CELL_SUPP_ADDR2;
        private string _CELL_BILL_COMPANY;
        private string _CELL_BILL_CONTACT;
        private string _CELL_BILL_EMAIL;
        private string _CELL_BILL_PHONE;
        private string _CELL_BILL_MOB;
        private string _CELL_BILL_FAX;
        private string _CELL_BILL_ADDR1;
        private string _CELL_BILL_ADDR2;
        private string _CELL_SHIP_COMPANY;
        private string _CELL_SHIP_CONTACT;
        private string _CELL_SHIP_EMAIL;
        private string _CELL_SHIP_PHONE;
        private string _CELL_SHIP_MOB;
        private string _CELL_SHIP_FAX;
        private string _CELL_SHIP_ADDR1;
        private string _CELL_SHIP_ADDR2;

        private string _CELL_ORDER_IDENTIFIER;

        private string _CELL_SUPP_QUOTE_DT;
        private string _COL_ITEM_ALT_NAME;
        private string _CELL_ETA_DATE;
        private string _CELL_ETD_DATE;

        private System.Nullable<int> _dynHdrDiscountOffset;

        public virtual System.Nullable<int> ExcelMapid
        {
            get
            {
                return _excelMapid;
            }
            set
            {
                _excelMapid = value;
            }
        }

        public virtual System.Nullable<int> GroupId
        {
            get
            {
                return _groupId;
            }
            set
            {
                _groupId = value;
            }
        }

        public virtual string XlsMapCode
        {
            get
            {
                return _xlsMapCode;
            }
            set
            {
                _xlsMapCode = value;
            }
        }

        public virtual System.Nullable<int> SectionRowStart
        {
            get
            {
                return _sectionRowStart;
            }
            set
            {
                _sectionRowStart = value;
            }
        }

        public virtual System.Nullable<int> ItemRowStart
        {
            get
            {
                return _itemRowStart;
            }
            set
            {
                _itemRowStart = value;
            }
        }

        public virtual System.Nullable<int> SkipRowsBefItem
        {
            get
            {
                return _skipRowsBefItem;
            }
            set
            {
                _skipRowsBefItem = value;
            }
        }

        public virtual System.Nullable<int> SkipRowsAftSection
        {
            get
            {
                return _skipRowsAftSection;
            }
            set
            {
                _skipRowsAftSection = value;
            }
        }

        public virtual string CellVrno
        {
            get
            {
                return _cellVrno;
            }
            set
            {
                _cellVrno = value;
            }
        }

        public virtual string CellRfqDt
        {
            get
            {
                return _cellRfqDt;
            }
            set
            {
                _cellRfqDt = value;
            }
        }

        public virtual string CellVessel
        {
            get
            {
                return _cellVessel;
            }
            set
            {
                _cellVessel = value;
            }
        }

        public virtual string CellPort
        {
            get
            {
                return _cellPort;
            }
            set
            {
                _cellPort = value;
            }
        }

        public virtual string CellLateDt
        {
            get
            {
                return _cellLateDt;
            }
            set
            {
                _cellLateDt = value;
            }
        }

        public virtual string CellSuppRef
        {
            get
            {
                return _cellSuppRef;
            }
            set
            {
                _cellSuppRef = value;
            }
        }

        public virtual string CellValidUpto
        {
            get
            {
                return _cellValidUpto;
            }
            set
            {
                _cellValidUpto = value;
            }
        }

        public virtual string CellCurrCode
        {
            get
            {
                return _cellCurrCode;
            }
            set
            {
                _cellCurrCode = value;
            }
        }

        public virtual string CellContact
        {
            get
            {
                return _cellContact;
            }
            set
            {
                _cellContact = value;
            }
        }

        public virtual string CellPayTerms
        {
            get
            {
                return _cellPayTerms;
            }
            set
            {
                _cellPayTerms = value;
            }
        }

        public virtual string CellDelTerms
        {
            get
            {
                return _cellDelTerms;
            }
            set
            {
                _cellDelTerms = value;
            }
        }

        public virtual string CellBuyerRemarks
        {
            get
            {
                return _cellBuyerRemarks;
            }
            set
            {
                _cellBuyerRemarks = value;
            }
        }

        public virtual string CellSuplrRemarks
        {
            get
            {
                return _cellSuplrRemarks;
            }
            set
            {
                _cellSuplrRemarks = value;
            }
        }

        public virtual string ColSection
        {
            get
            {
                return _colSection;
            }
            set
            {
                _colSection = value;
            }
        }

        public virtual string ColItemno
        {
            get
            {
                return _colItemno;
            }
            set
            {
                _colItemno = value;
            }
        }

        public virtual string ColItemRefno
        {
            get
            {
                return _colItemRefno;
            }
            set
            {
                _colItemRefno = value;
            }
        }

        public virtual string ColItemName
        {
            get
            {
                return _colItemName;
            }
            set
            {
                _colItemName = value;
            }
        }

        public virtual string ColItemQty
        {
            get
            {
                return _colItemQty;
            }
            set
            {
                _colItemQty = value;
            }
        }

        public virtual string ColItemUnit
        {
            get
            {
                return _colItemUnit;
            }
            set
            {
                _colItemUnit = value;
            }
        }

        public virtual string ColItemPrice
        {
            get
            {
                return _colItemPrice;
            }
            set
            {
                _colItemPrice = value;
            }
        }

        public virtual string ColItemDiscount
        {
            get
            {
                return _colItemDiscount;
            }
            set
            {
                _colItemDiscount = value;
            }
        }

        public virtual string ColItemAltQty
        {
            get
            {
                return _colItemAltQty;
            }
            set
            {
                _colItemAltQty = value;
            }
        }

        public virtual string ColItemAltUnit
        {
            get
            {
                return _colItemAltUnit;
            }
            set
            {
                _colItemAltUnit = value;
            }
        }

        public virtual string ColItemAltPrice
        {
            get
            {
                return _colItemAltPrice;
            }
            set
            {
                _colItemAltPrice = value;
            }
        }

        public virtual string ColItemDelDays
        {
            get
            {
                return _colItemDelDays;
            }
            set
            {
                _colItemDelDays = value;
            }
        }

        public virtual string ColItemRemarks
        {
            get
            {
                return _colItemRemarks;
            }
            set
            {
                _colItemRemarks = value;
            }
        }

        public virtual System.Nullable<int> ActiveSheet
        {
            get
            {
                return _activeSheet;
            }
            set
            {
                _activeSheet = value;
            }
        }

        public virtual System.Nullable<short> ExitForNoitem
        {
            get
            {
                return _exitForNoitem;
            }
            set
            {
                _exitForNoitem = value;
            }
        }

        public virtual string ColItemCurr
        {
            get
            {
                return _colItemCurr;
            }
            set
            {
                _colItemCurr = value;
            }
        }

        public virtual System.Nullable<int> DynSupRmrkOffset
        {
            get
            {
                return _dynSupRmrkOffset;
            }
            set
            {
                _dynSupRmrkOffset = value;
            }
        }

        public virtual System.Nullable<int> OverrideAltQty
        {
            get
            {
                return _overrideAltQty;
            }
            set
            {
                _overrideAltQty = value;
            }
        }

        public virtual System.Nullable<int> SkipHiddenRows
        {
            get
            {
                return _skipHiddenRows;
            }
            set
            {
                _skipHiddenRows = value;
            }
        }

        public virtual System.Nullable<int> ItemDiscPercnt
        {
            get
            {
                return _itemDiscPercnt;
            }
            set
            {
                _itemDiscPercnt = value;
            }
        }

        public virtual string ColItemTotal
        {
            get
            {
                return _colItemTotal;
            }
            set
            {
                _colItemTotal = value;
            }
        }

        public virtual System.Nullable<int> ApplyTotalFormula
        {
            get
            {
                return _applyTotalFormula;
            }
            set
            {
                _applyTotalFormula = value;
            }
        }

        public virtual System.Nullable<int> ReadItemRemarksUptoNo
        {
            get
            {
                return _readItemRemarksUptoNo;
            }
            set
            {
                _readItemRemarksUptoNo = value;
            }
        }

        public virtual string ColItemBuyerRemarks
        {
            get
            {
                return _colItemBuyerRemarks;
            }
            set
            {
                _colItemBuyerRemarks = value;
            }
        }

        public virtual string AddToVrno
        {
            get
            {
                return _addToVrno;
            }
            set
            {
                _addToVrno = value;
            }
        }

        public virtual string RemoveFromVrno
        {
            get
            {
                return _removeFromVrno;
            }
            set
            {
                _removeFromVrno = value;
            }
        }

        public virtual System.Nullable<int> SkipRowsAftItem
        {
            get
            {
                return _skipRowsAftItem;
            }
            set
            {
                _skipRowsAftItem = value;
            }
        }

        public virtual System.Nullable<int> ItemNoAsRowno
        {
            get
            {
                return _itemNoAsRowno;
            }
            set
            {
                _itemNoAsRowno = value;
            }
        }

        public virtual string ColItemComments
        {
            get
            {
                return _colItemComments;
            }
            set
            {
                _colItemComments = value;
            }
        }

        public virtual string SampleFile
        {
            get
            {
                return _sampleFile;
            }
            set
            {
                _sampleFile = value;
            }
        }

        public virtual string CellVslImono
        {
            get
            {
                return _cellVslImono;
            }
            set
            {
                _cellVslImono = value;
            }
        }

        public virtual string CellPortName
        {
            get
            {
                return _cellPortName;
            }
            set
            {
                _cellPortName = value;
            }
        }

        public virtual string CellDocType
        {
            get
            {
                return _cellDocType;
            }
            set
            {
                _cellDocType = value;
            }
        }

        public virtual string ColItemSuppRefno
        {
            get
            {
                return _colItemSuppRefno;
            }
            set
            {
                _colItemSuppRefno = value;
            }
        }

        public virtual string CellSuppExpDt
        {
            get
            {
                return _cellSuppExpDt;
            }
            set
            {
                _cellSuppExpDt = value;
            }
        }

        public virtual string CellSuppLateDt
        {
            get
            {
                return _cellSuppLateDt;
            }
            set
            {
                _cellSuppLateDt = value;
            }
        }

        public virtual string CellSuppLeadDays
        {
            get
            {
                return _cellSuppLeadDays;
            }
            set
            {
                _cellSuppLeadDays = value;
            }
        }

        public virtual string CellByrCompany
        {
            get
            {
                return _cellByrCompany;
            }
            set
            {
                _cellByrCompany = value;
            }
        }

        public virtual string CellByrContact
        {
            get
            {
                return _cellByrContact;
            }
            set
            {
                _cellByrContact = value;
            }
        }

        public virtual string CellByrEmail
        {
            get
            {
                return _cellByrEmail;
            }
            set
            {
                _cellByrEmail = value;
            }
        }

        public virtual string CellByrPhone
        {
            get
            {
                return _cellByrPhone;
            }
            set
            {
                _cellByrPhone = value;
            }
        }

        public virtual string CellByrMob
        {
            get
            {
                return _cellByrMob;
            }
            set
            {
                _cellByrMob = value;
            }
        }

        public virtual string CellByrFax
        {
            get
            {
                return _cellByrFax;
            }
            set
            {
                _cellByrFax = value;
            }
        }

        public virtual string CellSuppCompany
        {
            get
            {
                return _cellSuppCompany;
            }
            set
            {
                _cellSuppCompany = value;
            }
        }

        public virtual string CellSuppContact
        {
            get
            {
                return _cellSuppContact;
            }
            set
            {
                _cellSuppContact = value;
            }
        }

        public virtual string CellSuppEmail
        {
            get
            {
                return _cellSuppEmail;
            }
            set
            {
                _cellSuppEmail = value;
            }
        }

        public virtual string CellSuppPhone
        {
            get
            {
                return _cellSuppPhone;
            }
            set
            {
                _cellSuppPhone = value;
            }
        }

        public virtual string CellSuppMob
        {
            get
            {
                return _cellSuppMob;
            }
            set
            {
                _cellSuppMob = value;
            }
        }

        public virtual string CellSuppFax
        {
            get
            {
                return _cellSuppFax;
            }
            set
            {
                _cellSuppFax = value;
            }
        }

        public virtual string CellFreightAmt
        {
            get
            {
                return _cellFreightAmt;
            }
            set
            {
                _cellFreightAmt = value;
            }
        }

        public virtual string CellOtherAmt
        {
            get
            {
                return _cellOtherAmt;
            }
            set
            {
                _cellOtherAmt = value;
            }
        }

        // Added on 10-MARCH-2015
        public virtual string CellDiscProvsn
        {
            get
            {
                return _cellDiscProvsn;
            }
            set
            {
                _cellDiscProvsn = value;
            }
        }

        public virtual string DiscProvsnValue
        {
            get
            {
                return _discProvsnValue;
            }
            set
            {
                _discProvsnValue = value;
            }
        }

        public virtual System.Nullable<int> AltItemStartOffset
        {
            get
            {
                return _altItemStartOffset;
            }
            set
            {
                _altItemStartOffset = value;
            }
        }

        public virtual System.Nullable<int> AltItemCount
        {
            get
            {
                return _altItemCount;
            }
            set
            {
                _altItemCount = value;
            }
        }

        public virtual string CellRfqTitle
        {
            get
            {
                return _cellRfqTitle;
            }
            set
            {
                _cellRfqTitle = value;
            }
        }

        public virtual string CellRfqDept
        {
            get
            {
                return _cellRfqDept;
            }
            set
            {
                _cellRfqDept = value;
            }
        }

        public virtual string CellEquipName
        {
            get
            {
                return _cellEquipName;
            }
            set
            {
                _cellEquipName = value;
            }
        }

        public virtual string CellEquipType
        {
            get
            {
                return _cellEquipType;
            }
            set
            {
                _cellEquipType = value;
            }
        }

        public virtual string CellEquipMaker
        {
            get
            {
                return _cellEquipMaker;
            }
            set
            {
                _cellEquipMaker = value;
            }
        }

        public virtual string CellEquipSrno
        {
            get
            {
                return _cellEquipSrno;
            }
            set
            {
                _cellEquipSrno = value;
            }
        }

        public virtual string CellEquipDtls
        {
            get
            {
                return _cellEquipDtls;
            }
            set
            {
                _cellEquipDtls = value;
            }
        }

        public virtual string CellMsgNo
        {
            get
            {
                return _cellMsgNo;
            }
            set
            {
                _cellMsgNo = value;
            }
        }

        public virtual System.Nullable<int> DynSupFreightOffset
        {
            get
            {
                return _dynSupFreightOffset;
            }
            set
            {
                _dynSupFreightOffset = value;
            }
        }

        public virtual System.Nullable<int> DynOtherCostOffset
        {
            get
            {
                return _dynOtherCostOffset;
            }
            set
            {
                _dynOtherCostOffset = value;
            }
        }

        public virtual System.Nullable<int> DynSupCurrOffset
        {
            get
            {
                return _dynSupCurrOffset;
            }
            set
            {
                _dynSupCurrOffset = value;
            }
        }

        public virtual System.Nullable<int> DynBuyRmrkOffset
        {
            get
            {
                return _dynBuyRmrkOffset;
            }
            set
            {
                _dynBuyRmrkOffset = value;
            }
        }

        public virtual System.Nullable<int> DynHdrDiscountOffset
        {
            get
            {
                return _dynHdrDiscountOffset;
            }
            set
            {
                _dynHdrDiscountOffset = value;
            }
        }

        public virtual System.Nullable<int> MultiLineDynItemDesc
        {
            get
            {
                return _multilineItemDesc;
            }
            set
            {
                _multilineItemDesc = value;
            }
        }

        public virtual string DecimalSeprator
        {
            get
            {
                return _decimalSeprator;
            }
            set
            {
                _decimalSeprator = value;
            }
        }

        public virtual string DefaultUMO
        {
            get
            {
                return _defaultUMO;
            }
            set
            {
                _defaultUMO = value;
            }
        }

        public virtual string ExcelNameMgr
        {
            get
            {
                return _excelNameMgr;
            }
            set
            {
                _excelNameMgr = value;
            }
        }

        public virtual string REMOVE_FROM_VESSEL_NAME
        {
            get
            {
                return _REMOVE_FROM_VESSEL_NAME;
            }
            set
            {
                _REMOVE_FROM_VESSEL_NAME = value;
            }
        }

        public virtual string CELL_BYR_ADDR1
        {
            get
            {
                return _CELL_BYR_ADDR1;
            }
            set
            {
                _CELL_BYR_ADDR1 = value;
            }
        }

        public virtual string CELL_BYR_ADDR2
        {
            get
            {
                return _CELL_BYR_ADDR2;
            }
            set
            {
                _CELL_BYR_ADDR2 = value;
            }
        }

        public virtual string CELL_SUPP_ADDR1
        {
            get
            {
                return _CELL_SUPP_ADDR1;
            }
            set
            {
                _CELL_SUPP_ADDR1 = value;
            }
        }

        public virtual string CELL_SUPP_ADDR2
        {
            get
            {
                return _CELL_SUPP_ADDR2;
            }
            set
            {
                _CELL_SUPP_ADDR2 = value;
            }
        }

        public virtual string CELL_BILL_COMPANY
        {
            get
            {
                return _CELL_BILL_COMPANY;
            }
            set
            {
                _CELL_BILL_COMPANY = value;
            }
        }

        public virtual string CELL_BILL_CONTACT
        {
            get
            {
                return _CELL_BILL_CONTACT;
            }
            set
            {
                _CELL_BILL_CONTACT = value;
            }
        }

        public virtual string CELL_BILL_EMAIL
        {
            get
            {
                return _CELL_BILL_EMAIL;
            }
            set
            {
                _CELL_BILL_EMAIL = value;
            }
        }

        public virtual string CELL_BILL_PHONE
        {
            get
            {
                return _CELL_BILL_PHONE;
            }
            set
            {
                _CELL_BILL_PHONE = value;
            }
        }

        public virtual string CELL_BILL_MOB
        {
            get
            {
                return _CELL_BILL_MOB;
            }
            set
            {
                _CELL_BILL_MOB = value;
            }
        }

        public virtual string CELL_BILL_FAX
        {
            get
            {
                return _CELL_BILL_FAX;
            }
            set
            {
                _CELL_BILL_FAX = value;
            }
        }

        public virtual string CELL_BILL_ADDR1
        {
            get
            {
                return _CELL_BILL_ADDR1;
            }
            set
            {
                _CELL_BILL_ADDR1 = value;
            }
        }

        public virtual string CELL_BILL_ADDR2
        {
            get
            {
                return _CELL_BILL_ADDR2;
            }
            set
            {
                _CELL_BILL_ADDR2 = value;
            }
        }

        public virtual string CELL_SHIP_COMPANY
        {
            get
            {
                return _CELL_SHIP_COMPANY;
            }
            set
            {
                _CELL_SHIP_COMPANY = value;
            }
        }

        public virtual string CELL_SHIP_CONTACT
        {
            get
            {
                return _CELL_SHIP_CONTACT;
            }
            set
            {
                _CELL_SHIP_CONTACT = value;
            }
        }

        public virtual string CELL_SHIP_EMAIL
        {
            get
            {
                return _CELL_SHIP_EMAIL;
            }
            set
            {
                _CELL_SHIP_EMAIL = value;
            }
        }

        public virtual string CELL_SHIP_PHONE
        {
            get
            {
                return _CELL_SHIP_PHONE;
            }
            set
            {
                _CELL_SHIP_PHONE = value;
            }
        }

        public virtual string CELL_SHIP_MOB
        {
            get
            {
                return _CELL_SHIP_MOB;
            }
            set
            {
                _CELL_SHIP_MOB = value;
            }
        }

        public virtual string CELL_SHIP_FAX
        {
            get
            {
                return _CELL_SHIP_FAX;
            }
            set
            {
                _CELL_SHIP_FAX = value;
            }
        }

        public virtual string CELL_SHIP_ADDR1
        {
            get
            {
                return _CELL_SHIP_ADDR1;
            }
            set
            {
                _CELL_SHIP_ADDR1 = value;
            }
        }

        public virtual string CELL_SHIP_ADDR2
        {
            get
            {
                return _CELL_SHIP_ADDR2;
            }
            set
            {
                _CELL_SHIP_ADDR2 = value;
            }
        }

        public virtual string CELL_ORDER_IDENTIFIER
        {
            get
            {
                return _CELL_ORDER_IDENTIFIER;
            }
            set
            {
                _CELL_ORDER_IDENTIFIER = value;
            }
        }


        // added on 06-APRIL-2017
        public virtual string CELL_SUPP_QUOTE_DT
        {
            get
            {
                return _CELL_SUPP_QUOTE_DT;
            }
            set
            {
                _CELL_SUPP_QUOTE_DT = value;
            }
        }

        public virtual string COL_ITEM_ALT_NAME
        {
            get
            {
                return _COL_ITEM_ALT_NAME;
            }
            set
            {
                _COL_ITEM_ALT_NAME = value;
            }
        }

        public virtual string CELL_ETA_DATE
        {
            get
            {
                return _CELL_ETA_DATE;
            }
            set
            {
                _CELL_ETA_DATE = value;
            }
        }

        public virtual string CELL_ETD_DATE
        {
            get
            {
                return _CELL_ETD_DATE;
            }
            set
            {
                _CELL_ETD_DATE = value;
            }
        }


        private void Clean()
        {
            this.ExcelMapid = null;
            this.GroupId = null;
            this.XlsMapCode = string.Empty;
            this.SectionRowStart = null;
            this.ItemRowStart = null;
            this.SkipRowsBefItem = null;
            this.SkipRowsAftSection = null;
            this.CellVrno = string.Empty;
            this.CellRfqDt = string.Empty;
            this.CellVessel = string.Empty;
            this.CellPort = string.Empty;
            this.CellLateDt = string.Empty;
            this.CellSuppRef = string.Empty;
            this.CellValidUpto = string.Empty;
            this.CellCurrCode = string.Empty;
            this.CellContact = string.Empty;
            this.CellPayTerms = string.Empty;
            this.CellDelTerms = string.Empty;
            this.CellBuyerRemarks = string.Empty;
            this.CellSuplrRemarks = string.Empty;
            this.ColSection = string.Empty;
            this.ColItemno = string.Empty;
            this.ColItemRefno = string.Empty;
            this.ColItemName = string.Empty;
            this.ColItemQty = string.Empty;
            this.ColItemUnit = string.Empty;
            this.ColItemPrice = string.Empty;
            this.ColItemDiscount = string.Empty;
            this.ColItemAltQty = string.Empty;
            this.ColItemAltUnit = string.Empty;
            this.ColItemAltPrice = string.Empty;
            this.ColItemDelDays = string.Empty;
            this.ColItemRemarks = string.Empty;
            this.ActiveSheet = null;
            this.ExitForNoitem = null;
            this.ColItemCurr = string.Empty;
            this.DynSupRmrkOffset = null;
            this.OverrideAltQty = null;
            this.SkipHiddenRows = null;
            this.ItemDiscPercnt = null;
            this.ColItemTotal = string.Empty;
            this.ApplyTotalFormula = null;
            this.ReadItemRemarksUptoNo = null;
            this.ColItemBuyerRemarks = string.Empty;
            this.AddToVrno = string.Empty;
            this.RemoveFromVrno = string.Empty;
            this.SkipRowsAftItem = null;
            this.ItemNoAsRowno = null;
            this.ColItemComments = string.Empty;
            this.SampleFile = string.Empty;
            this.CellVslImono = string.Empty;
            this.CellPortName = string.Empty;
            this.CellDocType = string.Empty;
            this.ColItemSuppRefno = string.Empty;
            this.CellSuppExpDt = string.Empty;
            this.CellSuppLateDt = string.Empty;
            this.CellSuppLeadDays = string.Empty;
            this.CellByrCompany = string.Empty;
            this.CellByrContact = string.Empty;
            this.CellByrEmail = string.Empty;
            this.CellByrPhone = string.Empty;
            this.CellByrMob = string.Empty;
            this.CellByrFax = string.Empty;
            this.CellSuppCompany = string.Empty;
            this.CellSuppContact = string.Empty;
            this.CellSuppEmail = string.Empty;
            this.CellSuppPhone = string.Empty;
            this.CellSuppMob = string.Empty;
            this.CellSuppFax = string.Empty;
            this.CellFreightAmt = string.Empty;
            this.CellOtherAmt = string.Empty;

            // added on 10-MARCH-2015
            this.CellDiscProvsn = string.Empty;
            this.DiscProvsnValue = string.Empty;
            this.AltItemStartOffset = null;
            this.AltItemCount = null;
            this.CellRfqTitle = string.Empty;
            this.CellRfqDept = string.Empty;
            this.CellEquipName = string.Empty;
            this.CellEquipType = string.Empty;
            this.CellEquipMaker = string.Empty;
            this.CellEquipSrno = string.Empty;
            this.CellEquipDtls = string.Empty;
            this.CellMsgNo = string.Empty;
            this.DynSupFreightOffset = null;
            this.DynOtherCostOffset = null;
            this.DynSupCurrOffset = null;
            this.DynBuyRmrkOffset = null;
            this.MultiLineDynItemDesc = null;
            this.DecimalSeprator = string.Empty;
            this.DefaultUMO = string.Empty;
            this.ExcelNameMgr = string.Empty;
            this.DynHdrDiscountOffset = null;
            this.REMOVE_FROM_VESSEL_NAME = string.Empty;
            this.CELL_BYR_ADDR1 = string.Empty;
            this.CELL_BYR_ADDR2 = string.Empty;
            this.CELL_SUPP_ADDR1 = string.Empty;
            this.CELL_SUPP_ADDR2 = string.Empty;
            this.CELL_BILL_COMPANY = string.Empty;
            this.CELL_BILL_CONTACT = string.Empty;
            this.CELL_BILL_EMAIL = string.Empty;
            this.CELL_BILL_PHONE = string.Empty;
            this.CELL_BILL_MOB = string.Empty;
            this.CELL_BILL_FAX = string.Empty;
            this.CELL_BILL_ADDR1 = string.Empty;
            this.CELL_BILL_ADDR2 = string.Empty;
            this.CELL_SHIP_COMPANY = string.Empty;
            this.CELL_SHIP_CONTACT = string.Empty;
            this.CELL_SHIP_EMAIL = string.Empty;
            this.CELL_SHIP_PHONE = string.Empty;
            this.CELL_SHIP_MOB = string.Empty;
            this.CELL_SHIP_FAX = string.Empty;
            this.CELL_SHIP_ADDR1 = string.Empty;
            this.CELL_SHIP_ADDR2 = string.Empty;
            this.CELL_ORDER_IDENTIFIER = string.Empty;
            this.CELL_SUPP_QUOTE_DT = string.Empty;
            this.COL_ITEM_ALT_NAME = string.Empty;
            this.CELL_ETA_DATE = string.Empty;
            this.CELL_ETD_DATE = string.Empty;
        }

        private void Fill(System.Data.DataRow dr)
        {
            this.Clean();
            if ((dr["EXCEL_MAPID"] != System.DBNull.Value))
            {
                this.ExcelMapid = ((System.Nullable<int>)(dr["EXCEL_MAPID"]));
            }
            if ((dr["GROUP_ID"] != System.DBNull.Value))
            {
                this.GroupId = ((System.Nullable<int>)(dr["GROUP_ID"]));
            }
            if ((dr["XLS_MAP_CODE"] != System.DBNull.Value))
            {
                this.XlsMapCode = ((string)(dr["XLS_MAP_CODE"]));
            }
            if ((dr["SECTION_ROW_START"] != System.DBNull.Value))
            {
                this.SectionRowStart = ((System.Nullable<int>)(dr["SECTION_ROW_START"]));
            }
            if ((dr["ITEM_ROW_START"] != System.DBNull.Value))
            {
                this.ItemRowStart = ((System.Nullable<int>)(dr["ITEM_ROW_START"]));
            }
            if ((dr["SKIP_ROWS_BEF_ITEM"] != System.DBNull.Value))
            {
                this.SkipRowsBefItem = ((System.Nullable<int>)(dr["SKIP_ROWS_BEF_ITEM"]));
            }
            if ((dr["SKIP_ROWS_AFT_SECTION"] != System.DBNull.Value))
            {
                this.SkipRowsAftSection = ((System.Nullable<int>)(dr["SKIP_ROWS_AFT_SECTION"]));
            }
            if ((dr["CELL_VRNO"] != System.DBNull.Value))
            {
                this.CellVrno = ((string)(dr["CELL_VRNO"]));
            }
            if ((dr["CELL_RFQ_DT"] != System.DBNull.Value))
            {
                this.CellRfqDt = ((string)(dr["CELL_RFQ_DT"]));
            }
            if ((dr["CELL_VESSEL"] != System.DBNull.Value))
            {
                this.CellVessel = ((string)(dr["CELL_VESSEL"]));
            }
            if ((dr["CELL_PORT"] != System.DBNull.Value))
            {
                this.CellPort = ((string)(dr["CELL_PORT"]));
            }
            if ((dr["CELL_LATE_DT"] != System.DBNull.Value))
            {
                this.CellLateDt = ((string)(dr["CELL_LATE_DT"]));
            }
            if ((dr["CELL_SUPP_REF"] != System.DBNull.Value))
            {
                this.CellSuppRef = ((string)(dr["CELL_SUPP_REF"]));
            }
            if ((dr["CELL_VALID_UPTO"] != System.DBNull.Value))
            {
                this.CellValidUpto = ((string)(dr["CELL_VALID_UPTO"]));
            }
            if ((dr["CELL_CURR_CODE"] != System.DBNull.Value))
            {
                this.CellCurrCode = ((string)(dr["CELL_CURR_CODE"]));
            }
            if ((dr["CELL_CONTACT"] != System.DBNull.Value))
            {
                this.CellContact = ((string)(dr["CELL_CONTACT"]));
            }
            if ((dr["CELL_PAY_TERMS"] != System.DBNull.Value))
            {
                this.CellPayTerms = ((string)(dr["CELL_PAY_TERMS"]));
            }
            if ((dr["CELL_DEL_TERMS"] != System.DBNull.Value))
            {
                this.CellDelTerms = ((string)(dr["CELL_DEL_TERMS"]));
            }
            if ((dr["CELL_BUYER_REMARKS"] != System.DBNull.Value))
            {
                this.CellBuyerRemarks = ((string)(dr["CELL_BUYER_REMARKS"]));
            }
            if ((dr["CELL_SUPLR_REMARKS"] != System.DBNull.Value))
            {
                this.CellSuplrRemarks = ((string)(dr["CELL_SUPLR_REMARKS"]));
            }
            if ((dr["COL_SECTION"] != System.DBNull.Value))
            {
                this.ColSection = ((string)(dr["COL_SECTION"]));
            }
            if ((dr["COL_ITEMNO"] != System.DBNull.Value))
            {
                this.ColItemno = ((string)(dr["COL_ITEMNO"]));
            }
            if ((dr["COL_ITEM_REFNO"] != System.DBNull.Value))
            {
                this.ColItemRefno = ((string)(dr["COL_ITEM_REFNO"]));
            }
            if ((dr["COL_ITEM_NAME"] != System.DBNull.Value))
            {
                this.ColItemName = ((string)(dr["COL_ITEM_NAME"]));
            }
            if ((dr["COL_ITEM_QTY"] != System.DBNull.Value))
            {
                this.ColItemQty = ((string)(dr["COL_ITEM_QTY"]));
            }
            if ((dr["COL_ITEM_UNIT"] != System.DBNull.Value))
            {
                this.ColItemUnit = ((string)(dr["COL_ITEM_UNIT"]));
            }
            if ((dr["COL_ITEM_PRICE"] != System.DBNull.Value))
            {
                this.ColItemPrice = ((string)(dr["COL_ITEM_PRICE"]));
            }
            if ((dr["COL_ITEM_DISCOUNT"] != System.DBNull.Value))
            {
                this.ColItemDiscount = ((string)(dr["COL_ITEM_DISCOUNT"]));
            }
            if ((dr["COL_ITEM_ALT_QTY"] != System.DBNull.Value))
            {
                this.ColItemAltQty = ((string)(dr["COL_ITEM_ALT_QTY"]));
            }
            if ((dr["COL_ITEM_ALT_UNIT"] != System.DBNull.Value))
            {
                this.ColItemAltUnit = ((string)(dr["COL_ITEM_ALT_UNIT"]));
            }
            if ((dr["COL_ITEM_ALT_PRICE"] != System.DBNull.Value))
            {
                this.ColItemAltPrice = ((string)(dr["COL_ITEM_ALT_PRICE"]));
            }
            if ((dr["COL_ITEM_DEL_DAYS"] != System.DBNull.Value))
            {
                this.ColItemDelDays = ((string)(dr["COL_ITEM_DEL_DAYS"]));
            }
            if ((dr["COL_ITEM_REMARKS"] != System.DBNull.Value))
            {
                this.ColItemRemarks = ((string)(dr["COL_ITEM_REMARKS"]));
            }
            if ((dr["ACTIVE_SHEET"] != System.DBNull.Value))
            {
                this.ActiveSheet = ((System.Nullable<int>)(dr["ACTIVE_SHEET"]));
            }
            if ((dr["EXIT_FOR_NOITEM"] != System.DBNull.Value))
            {
                this.ExitForNoitem = ((System.Nullable<short>)(dr["EXIT_FOR_NOITEM"]));
            }
            if ((dr["COL_ITEM_CURR"] != System.DBNull.Value))
            {
                this.ColItemCurr = ((string)(dr["COL_ITEM_CURR"]));
            }
            if ((dr["DYN_SUP_RMRK_OFFSET"] != System.DBNull.Value))
            {
                this.DynSupRmrkOffset = ((System.Nullable<int>)(dr["DYN_SUP_RMRK_OFFSET"]));
            }
            if ((dr["OVERRIDE_ALT_QTY"] != System.DBNull.Value))
            {
                this.OverrideAltQty = ((System.Nullable<int>)(dr["OVERRIDE_ALT_QTY"]));
            }
            if ((dr["SKIP_HIDDEN_ROWS"] != System.DBNull.Value))
            {
                this.SkipHiddenRows = ((System.Nullable<int>)(dr["SKIP_HIDDEN_ROWS"]));
            }
            if ((dr["ITEM_DISC_PERCNT"] != System.DBNull.Value))
            {
                this.ItemDiscPercnt = ((System.Nullable<int>)(dr["ITEM_DISC_PERCNT"]));
            }
            if ((dr["COL_ITEM_TOTAL"] != System.DBNull.Value))
            {
                this.ColItemTotal = ((string)(dr["COL_ITEM_TOTAL"]));
            }
            if ((dr["APPLY_TOTAL_FORMULA"] != System.DBNull.Value))
            {
                this.ApplyTotalFormula = ((System.Nullable<int>)(dr["APPLY_TOTAL_FORMULA"]));
            }
            if ((dr["READ_ITEM_REMARKS_UPTO_NO"] != System.DBNull.Value))
            {
                this.ReadItemRemarksUptoNo = ((System.Nullable<int>)(dr["READ_ITEM_REMARKS_UPTO_NO"]));
            }
            if ((dr["COL_ITEM_BUYER_REMARKS"] != System.DBNull.Value))
            {
                this.ColItemBuyerRemarks = ((string)(dr["COL_ITEM_BUYER_REMARKS"]));
            }
            if ((dr["ADD_TO_VRNO"] != System.DBNull.Value))
            {
                this.AddToVrno = ((string)(dr["ADD_TO_VRNO"]));
            }
            if ((dr["REMOVE_FROM_VRNO"] != System.DBNull.Value))
            {
                this.RemoveFromVrno = ((string)(dr["REMOVE_FROM_VRNO"]));
            }
            if ((dr["SKIP_ROWS_AFT_ITEM"] != System.DBNull.Value))
            {
                this.SkipRowsAftItem = ((System.Nullable<int>)(dr["SKIP_ROWS_AFT_ITEM"]));
            }
            if ((dr["ITEM_NO_AS_ROWNO"] != System.DBNull.Value))
            {
                this.ItemNoAsRowno = ((System.Nullable<int>)(dr["ITEM_NO_AS_ROWNO"]));
            }
            if ((dr["COL_ITEM_COMMENTS"] != System.DBNull.Value))
            {
                this.ColItemComments = ((string)(dr["COL_ITEM_COMMENTS"]));
            }
            if ((dr["SAMPLE_FILE"] != System.DBNull.Value))
            {
                this.SampleFile = ((string)(dr["SAMPLE_FILE"]));
            }
            if ((dr["CELL_VSL_IMONO"] != System.DBNull.Value))
            {
                this.CellVslImono = ((string)(dr["CELL_VSL_IMONO"]));
            }
            if ((dr["CELL_PORT_NAME"] != System.DBNull.Value))
            {
                this.CellPortName = ((string)(dr["CELL_PORT_NAME"]));
            }
            if ((dr["CELL_DOC_TYPE"] != System.DBNull.Value))
            {
                this.CellDocType = ((string)(dr["CELL_DOC_TYPE"]));
            }
            //if ((dr["COL_ITEM_BYR_REMARK"] != System.DBNull.Value))
            //{
            //    this.ColItemByrRemark = ((string)(dr["COL_ITEM_BYR_REMARK"]));
            //}
            if ((dr["COL_ITEM_SUPP_REFNO"] != System.DBNull.Value))
            {
                this.ColItemSuppRefno = ((string)(dr["COL_ITEM_SUPP_REFNO"]));
            }
            if ((dr["CELL_SUPP_EXP_DT"] != System.DBNull.Value))
            {
                this.CellSuppExpDt = ((string)(dr["CELL_SUPP_EXP_DT"]));
            }
            if ((dr["CELL_SUPP_LATE_DT"] != System.DBNull.Value))
            {
                this.CellSuppLateDt = ((string)(dr["CELL_SUPP_LATE_DT"]));
            }
            if ((dr["CELL_SUPP_LEAD_DAYS"] != System.DBNull.Value))
            {
                this.CellSuppLeadDays = ((string)(dr["CELL_SUPP_LEAD_DAYS"]));
            }
            if ((dr["CELL_BYR_COMPANY"] != System.DBNull.Value))
            {
                this.CellByrCompany = ((string)(dr["CELL_BYR_COMPANY"]));
            }
            if ((dr["CELL_BYR_CONTACT"] != System.DBNull.Value))
            {
                this.CellByrContact = ((string)(dr["CELL_BYR_CONTACT"]));
            }
            if ((dr["CELL_BYR_EMAIL"] != System.DBNull.Value))
            {
                this.CellByrEmail = ((string)(dr["CELL_BYR_EMAIL"]));
            }
            if ((dr["CELL_BYR_PHONE"] != System.DBNull.Value))
            {
                this.CellByrPhone = ((string)(dr["CELL_BYR_PHONE"]));
            }
            if ((dr["CELL_BYR_MOB"] != System.DBNull.Value))
            {
                this.CellByrMob = ((string)(dr["CELL_BYR_MOB"]));
            }
            if ((dr["CELL_BYR_FAX"] != System.DBNull.Value))
            {
                this.CellByrFax = ((string)(dr["CELL_BYR_FAX"]));
            }
            if ((dr["CELL_SUPP_COMPANY"] != System.DBNull.Value))
            {
                this.CellSuppCompany = ((string)(dr["CELL_SUPP_COMPANY"]));
            }
            if ((dr["CELL_SUPP_CONTACT"] != System.DBNull.Value))
            {
                this.CellSuppContact = ((string)(dr["CELL_SUPP_CONTACT"]));
            }
            if ((dr["CELL_SUPP_EMAIL"] != System.DBNull.Value))
            {
                this.CellSuppEmail = ((string)(dr["CELL_SUPP_EMAIL"]));
            }
            if ((dr["CELL_SUPP_PHONE"] != System.DBNull.Value))
            {
                this.CellSuppPhone = ((string)(dr["CELL_SUPP_PHONE"]));
            }
            if ((dr["CELL_SUPP_MOB"] != System.DBNull.Value))
            {
                this.CellSuppMob = ((string)(dr["CELL_SUPP_MOB"]));
            }
            if ((dr["CELL_SUPP_FAX"] != System.DBNull.Value))
            {
                this.CellSuppFax = ((string)(dr["CELL_SUPP_FAX"]));
            }
            if ((dr["CELL_FREIGHT_AMT"] != System.DBNull.Value))
            {
                this.CellFreightAmt = ((string)(dr["CELL_FREIGHT_AMT"]));
            }
            if ((dr["CELL_OTHER_AMT"] != System.DBNull.Value))
            {
                this.CellOtherAmt = ((string)(dr["CELL_OTHER_AMT"]));
            }

            // Added on 10-MARCH-2015

            if ((dr["CELL_DISC_PROVSN"] != System.DBNull.Value))
            {
                this.CellDiscProvsn = ((string)(dr["CELL_DISC_PROVSN"]));
            }
            if ((dr["DISC_PROVSN_VALUE"] != System.DBNull.Value))
            {
                this.DiscProvsnValue = ((string)(dr["DISC_PROVSN_VALUE"]));
            }
            if ((dr["ALT_ITEM_START_OFFSET"] != System.DBNull.Value))
            {
                this.AltItemStartOffset = ((System.Nullable<int>)(dr["ALT_ITEM_START_OFFSET"]));
            }
            if ((dr["ALT_ITEM_COUNT"] != System.DBNull.Value))
            {
                this.AltItemCount = ((System.Nullable<int>)(dr["ALT_ITEM_COUNT"]));
            }
            if ((dr["CELL_RFQ_TITLE"] != System.DBNull.Value))
            {
                this.CellRfqTitle = ((string)(dr["CELL_RFQ_TITLE"]));
            }
            if ((dr["CELL_RFQ_DEPT"] != System.DBNull.Value))
            {
                this.CellRfqDept = ((string)(dr["CELL_RFQ_DEPT"]));
            }
            if ((dr["CELL_EQUIP_NAME"] != System.DBNull.Value))
            {
                this.CellEquipName = ((string)(dr["CELL_EQUIP_NAME"]));
            }
            if ((dr["CELL_EQUIP_TYPE"] != System.DBNull.Value))
            {
                this.CellEquipType = ((string)(dr["CELL_EQUIP_TYPE"]));
            }
            if ((dr["CELL_EQUIP_MAKER"] != System.DBNull.Value))
            {
                this.CellEquipMaker = ((string)(dr["CELL_EQUIP_MAKER"]));
            }
            if ((dr["CELL_EQUIP_SERNO"] != System.DBNull.Value))
            {
                this.CellEquipSrno = ((string)(dr["CELL_EQUIP_SERNO"]));
            }
            if ((dr["CELL_EQUIP_DTLS"] != System.DBNull.Value))
            {
                this.CellEquipDtls = ((string)(dr["CELL_EQUIP_DTLS"]));
            }
            if ((dr["CELL_MSGNO"] != System.DBNull.Value))
            {
                this.CellMsgNo = ((string)(dr["CELL_MSGNO"]));
            }
            if ((dr["DYN_SUP_FREIGHT_OFFSET"] != System.DBNull.Value))
            {
                this.DynSupFreightOffset = ((System.Nullable<int>)(dr["DYN_SUP_FREIGHT_OFFSET"]));
            }

            if ((dr["DYN_OTHERCOST_OFFSET"] != System.DBNull.Value))
            {
                this.DynOtherCostOffset = ((System.Nullable<int>)(dr["DYN_OTHERCOST_OFFSET"]));
            }
            if ((dr["DYN_SUP_CURR_OFFSET"] != System.DBNull.Value))
            {
                this.DynSupCurrOffset = ((System.Nullable<int>)(dr["DYN_SUP_CURR_OFFSET"]));
            }
            if ((dr["DYN_BYR_RMRK_OFFSET"] != System.DBNull.Value))
            {
                this.DynBuyRmrkOffset = ((System.Nullable<int>)(dr["DYN_BYR_RMRK_OFFSET"]));
            }
            if ((dr["MULTILINE_ITEM_DESCR"] != System.DBNull.Value))
            {
                this.MultiLineDynItemDesc = ((System.Nullable<int>)(dr["MULTILINE_ITEM_DESCR"]));
            }
            if ((dr["EXCEL_NAME_MANAGER"] != System.DBNull.Value))
            {
                this.ExcelNameMgr = ((string)(dr["EXCEL_NAME_MANAGER"]));
            }
            if ((dr["DECIMAL_SEPARATOR"] != System.DBNull.Value))
            {
                this.DecimalSeprator = ((string)(dr["DECIMAL_SEPARATOR"]));
            }
            if ((dr["DEFAULT_UOM"] != System.DBNull.Value))
            {
                this.DefaultUMO = ((string)(dr["DEFAULT_UOM"]));
            }
            if ((dr["DYN_HDR_DISCOUNT_OFFSET"] != System.DBNull.Value))
            {
                this.DynHdrDiscountOffset = ((System.Nullable<int>)(dr["DYN_HDR_DISCOUNT_OFFSET"]));
            }

            if ((dr["REMOVE_FROM_VESSEL_NAME"] != System.DBNull.Value))
            {
                this.REMOVE_FROM_VESSEL_NAME = convert.ToString((dr["REMOVE_FROM_VESSEL_NAME"]));
            }

            if ((dr["CELL_BYR_ADDR1"] != System.DBNull.Value))
            {
                this.CELL_BYR_ADDR1 = convert.ToString((dr["CELL_BYR_ADDR1"]));
            }

            if ((dr["CELL_BYR_ADDR2"] != System.DBNull.Value))
            {
                this.CELL_BYR_ADDR2 = convert.ToString((dr["CELL_BYR_ADDR2"]));
            }

            if ((dr["CELL_SUPP_ADDR1"] != System.DBNull.Value))
            {
                this.CELL_SUPP_ADDR1 = convert.ToString((dr["CELL_SUPP_ADDR1"]));
            }

            if ((dr["CELL_SUPP_ADDR2"] != System.DBNull.Value))
            {
                this.CELL_SUPP_ADDR2 = convert.ToString((dr["CELL_SUPP_ADDR2"]));
            }

            if ((dr["CELL_SUPP_ADDR2"] != System.DBNull.Value))
            {
                this.CELL_SUPP_ADDR2 = convert.ToString((dr["CELL_SUPP_ADDR2"]));
            }

            if ((dr["CELL_BILL_COMPANY"] != System.DBNull.Value))
            {
                this.CELL_BILL_COMPANY = convert.ToString((dr["CELL_BILL_COMPANY"]));
            }

            if ((dr["CELL_BILL_CONTACT"] != System.DBNull.Value))
            {
                this.CELL_BILL_CONTACT = convert.ToString((dr["CELL_BILL_CONTACT"]));
            }

            if ((dr["CELL_BILL_EMAIL"] != System.DBNull.Value))
            {
                this.CELL_BILL_EMAIL = convert.ToString((dr["CELL_BILL_EMAIL"]));
            }

            if ((dr["CELL_BILL_PHONE"] != System.DBNull.Value))
            {
                this.CELL_BILL_PHONE = convert.ToString((dr["CELL_BILL_PHONE"]));
            }

            if ((dr["CELL_BILL_MOB"] != System.DBNull.Value))
            {
                this.CELL_BILL_MOB = convert.ToString((dr["CELL_BILL_MOB"]));
            }

            if ((dr["CELL_BILL_FAX"] != System.DBNull.Value))
            {
                this.CELL_BILL_FAX = convert.ToString((dr["CELL_BILL_FAX"]));
            }

            if ((dr["CELL_BILL_ADDR1"] != System.DBNull.Value))
            {
                this.CELL_BILL_ADDR1 = convert.ToString((dr["CELL_BILL_ADDR1"]));
            }

            if ((dr["CELL_BILL_ADDR2"] != System.DBNull.Value))
            {
                this.CELL_BILL_ADDR2 = convert.ToString((dr["CELL_BILL_ADDR2"]));
            }

            if ((dr["CELL_SHIP_COMPANY"] != System.DBNull.Value))
            {
                this.CELL_SHIP_COMPANY = convert.ToString((dr["CELL_SHIP_COMPANY"]));
            }

            if ((dr["CELL_SHIP_CONTACT"] != System.DBNull.Value))
            {
                this.CELL_SHIP_CONTACT = convert.ToString((dr["CELL_SHIP_CONTACT"]));
            }

            if ((dr["CELL_SHIP_EMAIL"] != System.DBNull.Value))
            {
                this.CELL_SHIP_EMAIL = convert.ToString((dr["CELL_SHIP_EMAIL"]));
            }

            if ((dr["CELL_SHIP_PHONE"] != System.DBNull.Value))
            {
                this.CELL_SHIP_PHONE = convert.ToString((dr["CELL_SHIP_PHONE"]));
            }

            if ((dr["CELL_SHIP_MOB"] != System.DBNull.Value))
            {
                this.CELL_SHIP_MOB = convert.ToString((dr["CELL_SHIP_MOB"]));
            }

            if ((dr["CELL_SHIP_FAX"] != System.DBNull.Value))
            {
                this.CELL_SHIP_FAX = convert.ToString((dr["CELL_SHIP_FAX"]));
            }

            if ((dr["CELL_SHIP_ADDR1"] != System.DBNull.Value))
            {
                this.CELL_SHIP_ADDR1 = convert.ToString((dr["CELL_SHIP_ADDR1"]));
            }

            if ((dr["CELL_SHIP_ADDR2"] != System.DBNull.Value))
            {
                this.CELL_SHIP_ADDR2 = convert.ToString((dr["CELL_SHIP_ADDR2"]));
            }

            if ((dr["CELL_ORDER_IDENTIFIER"] != System.DBNull.Value))
            {
                this.CELL_ORDER_IDENTIFIER = convert.ToString((dr["CELL_ORDER_IDENTIFIER"]));
            }

            // added on 06-APRIL-2017

            if ((dr["CELL_SUPP_QUOTE_DT"] != System.DBNull.Value))
            {
                this.CELL_SUPP_QUOTE_DT = convert.ToString((dr["CELL_SUPP_QUOTE_DT"]));
            }
            if ((dr["COL_ITEM_ALT_NAME"] != System.DBNull.Value))
            {
                this.COL_ITEM_ALT_NAME = convert.ToString((dr["COL_ITEM_ALT_NAME"]));
            }
            if ((dr["CELL_ETA_DATE"] != System.DBNull.Value))
            {
                this.CELL_ETA_DATE = convert.ToString((dr["CELL_ETA_DATE"]));
            }
            if ((dr["CELL_ETD_DATE"] != System.DBNull.Value))
            {
                this.CELL_ETD_DATE = convert.ToString((dr["CELL_ETD_DATE"]));
            }

        }

        public static SmXlsGroupMappingCollection GetAll()
        {
            Dal.SmXlsGroupMapping dbo = null;
            try
            {
                dbo = new Dal.SmXlsGroupMapping();
                System.Data.DataSet ds = dbo.SM_XLS_GROUP_MAPPING_Select_All();
                SmXlsGroupMappingCollection collection = new SmXlsGroupMappingCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmXlsGroupMapping obj = new SmXlsGroupMapping();
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
                if ((dbo != null))
                {
                    dbo.Dispose();
                }
            }
        }

        public static System.Data.DataSet GetAllGroupMappings()
        {
            Dal.SmXlsGroupMapping dbo = null;
            try
            {
                dbo = new Dal.SmXlsGroupMapping();
                System.Data.DataSet ds = dbo.SM_XLS_GROUP_MAPPING_Select_All();
                return ds;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if ((dbo != null))
                {
                    dbo.Dispose();
                }
            }
        }

        public static SmXlsGroupMapping Load_By_GroupID(int GroupID)
        {
            Dal.SmXlsGroupMapping dbo = null;
            try
            {
                SmXlsGroupMapping obj = null;
                dbo = new Dal.SmXlsGroupMapping();
                System.Data.DataSet ds = dbo.Select_SM_XLS_GROUP_MAPPINGs_By_GROUP_ID(GroupID);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmXlsGroupMapping();
                        obj.Fill(ds.Tables[0].Rows[0]);
                    }
                }
                return obj;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if ((dbo != null))
                {
                    dbo.Dispose();
                }
            }
        }

        public static SmXlsGroupMapping Load(System.Nullable<int> EXCEL_MAPID)
        {
            Dal.SmXlsGroupMapping dbo = null;
            try
            {
                dbo = new Dal.SmXlsGroupMapping();
                System.Data.DataSet ds = dbo.SM_XLS_GROUP_MAPPING_Select_One(EXCEL_MAPID);
                SmXlsGroupMapping obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmXlsGroupMapping();
                        obj.Fill(ds.Tables[0].Rows[0]);
                    }
                }
                return obj;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if ((dbo != null))
                {
                    dbo.Dispose();
                }
            }
        }

        public virtual void Load()
        {
            Dal.SmXlsGroupMapping dbo = null;
            try
            {
                dbo = new Dal.SmXlsGroupMapping();
                System.Data.DataSet ds = dbo.SM_XLS_GROUP_MAPPING_Select_One(this.ExcelMapid);
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        this.Fill(ds.Tables[0].Rows[0]);
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if ((dbo != null))
                {
                    dbo.Dispose();
                }
            }
        }

        public virtual void Insert()
        {
            Dal.SmXlsGroupMapping dbo = null;
            try
            {
                dbo = new Dal.SmXlsGroupMapping();
                this.ExcelMapid = dbo.SM_XLS_GROUP_MAPPING_Insert(this.GroupId, this.XlsMapCode, this.SectionRowStart, this.ItemRowStart, this.SkipRowsBefItem, this.SkipRowsAftSection,
                    this.CellVrno, this.CellRfqDt, this.CellVessel, this.CellPort, this.CellLateDt, this.CellSuppRef, this.CellValidUpto, this.CellCurrCode, this.CellContact, this.CellPayTerms,
                    this.CellDelTerms, this.CellBuyerRemarks, this.CellSuplrRemarks, this.ColSection, this.ColItemno, this.ColItemRefno, this.ColItemName, this.ColItemQty, this.ColItemUnit,
                    this.ColItemPrice, this.ColItemDiscount, this.ColItemAltQty, this.ColItemAltUnit, this.ColItemAltPrice, this.ColItemDelDays,
                    this.ColItemRemarks, this.ActiveSheet, this.ExitForNoitem, this.ColItemCurr, this.DynSupRmrkOffset, this.OverrideAltQty, this.SkipHiddenRows, this.ItemDiscPercnt, this.ColItemTotal,
                    this.ApplyTotalFormula, this.ReadItemRemarksUptoNo, this.ColItemBuyerRemarks, this.AddToVrno, this.RemoveFromVrno, this.SkipRowsAftItem, this.ItemNoAsRowno, this.ColItemComments,
                    this.SampleFile, this.CellVslImono, this.CellPortName, this.CellDocType, this.ColItemSuppRefno, this.CellSuppExpDt, this.CellSuppLateDt, this.CellSuppLeadDays, this.CellByrCompany,
                    this.CellByrContact, this.CellByrEmail, this.CellByrPhone, this.CellByrMob, this.CellByrFax, this.CellSuppCompany, this.CellSuppContact, this.CellSuppEmail, this.CellSuppPhone,
                    this.CellSuppMob, this.CellSuppFax, this.CellFreightAmt, this.CellOtherAmt, this.CellDiscProvsn, this.DiscProvsnValue, this.AltItemStartOffset, this.AltItemCount, this.CellRfqTitle,
                    this.CellRfqDept, this.CellEquipName, this.CellEquipType, this.CellEquipMaker, this.CellEquipSrno, this.CellEquipDtls, this.CellMsgNo, this.DynSupFreightOffset,
                    this.DynOtherCostOffset, this.DynSupCurrOffset, this.DynBuyRmrkOffset, this.MultiLineDynItemDesc, this.ExcelNameMgr, this.DecimalSeprator, this.DefaultUMO, this.DynHdrDiscountOffset,
                    this.REMOVE_FROM_VESSEL_NAME, this.CELL_BYR_ADDR1, this.CELL_BYR_ADDR2, this.CELL_SUPP_ADDR1, this.CELL_SUPP_ADDR2, this.CELL_BILL_COMPANY, this.CELL_BILL_CONTACT, this.CELL_BILL_EMAIL,
                    this.CELL_BILL_PHONE, this.CELL_BILL_MOB, this.CELL_BILL_FAX, this.CELL_BILL_ADDR1, this.CELL_BILL_ADDR2, this.CELL_SHIP_COMPANY, this.CELL_SHIP_CONTACT, this.CELL_SHIP_EMAIL, this.CELL_SHIP_PHONE,
                    this.CELL_SHIP_MOB, this.CELL_SHIP_FAX, this.CELL_SHIP_ADDR1, this.CELL_SHIP_ADDR2, this.CELL_ORDER_IDENTIFIER, this.CELL_SUPP_QUOTE_DT, this.COL_ITEM_ALT_NAME, this.CELL_ETA_DATE, this.CELL_ETD_DATE);
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if ((dbo != null))
                {
                    dbo.Dispose();
                }
            }
        }

        public virtual void Insert(Dal.DataAccess _dataAccess)
        {
            Dal.SmXlsGroupMapping dbo = null;
            try
            {
                dbo = new Dal.SmXlsGroupMapping(_dataAccess);
                this.ExcelMapid = dbo.SM_XLS_GROUP_MAPPING_Insert(this.GroupId, this.XlsMapCode, this.SectionRowStart, this.ItemRowStart, this.SkipRowsBefItem, this.SkipRowsAftSection, this.CellVrno,
                    this.CellRfqDt, this.CellVessel, this.CellPort, this.CellLateDt, this.CellSuppRef, this.CellValidUpto, this.CellCurrCode, this.CellContact, this.CellPayTerms, this.CellDelTerms,
                    this.CellBuyerRemarks, this.CellSuplrRemarks, this.ColSection, this.ColItemno, this.ColItemRefno, this.ColItemName, this.ColItemQty, this.ColItemUnit, this.ColItemPrice, this.ColItemDiscount,
                    this.ColItemAltQty, this.ColItemAltUnit, this.ColItemAltPrice, this.ColItemDelDays, this.ColItemRemarks, this.ActiveSheet, this.ExitForNoitem, this.ColItemCurr, this.DynSupRmrkOffset,
                    this.OverrideAltQty, this.SkipHiddenRows, this.ItemDiscPercnt, this.ColItemTotal, this.ApplyTotalFormula, this.ReadItemRemarksUptoNo, this.ColItemBuyerRemarks, this.AddToVrno, this.RemoveFromVrno,
                    this.SkipRowsAftItem, this.ItemNoAsRowno, this.ColItemComments, this.SampleFile, this.CellVslImono, this.CellPortName, this.CellDocType, this.ColItemSuppRefno, this.CellSuppExpDt, this.CellSuppLateDt,
                    this.CellSuppLeadDays, this.CellByrCompany, this.CellByrContact, this.CellByrEmail, this.CellByrPhone, this.CellByrMob, this.CellByrFax, this.CellSuppCompany, this.CellSuppContact, this.CellSuppEmail,
                    this.CellSuppPhone, this.CellSuppMob, this.CellSuppFax, this.CellFreightAmt, this.CellOtherAmt, this.CellDiscProvsn, this.DiscProvsnValue, this.AltItemStartOffset, this.AltItemCount, this.CellRfqTitle,
                    this.CellRfqDept, this.CellEquipName, this.CellEquipType, this.CellEquipMaker, this.CellEquipSrno, this.CellEquipDtls, this.CellMsgNo, this.DynSupFreightOffset,
                    this.DynOtherCostOffset, this.DynSupCurrOffset, this.DynBuyRmrkOffset, this.MultiLineDynItemDesc, this.ExcelNameMgr, this.DecimalSeprator, this.DefaultUMO, this.DynHdrDiscountOffset,
                    this.REMOVE_FROM_VESSEL_NAME, this.CELL_BYR_ADDR1, this.CELL_BYR_ADDR2, this.CELL_SUPP_ADDR1, this.CELL_SUPP_ADDR2, this.CELL_BILL_COMPANY, this.CELL_BILL_CONTACT, this.CELL_BILL_EMAIL,
                    this.CELL_BILL_PHONE, this.CELL_BILL_MOB, this.CELL_BILL_FAX, this.CELL_BILL_ADDR1, this.CELL_BILL_ADDR2, this.CELL_SHIP_COMPANY, this.CELL_SHIP_CONTACT, this.CELL_SHIP_EMAIL, this.CELL_SHIP_PHONE,
                    this.CELL_SHIP_MOB, this.CELL_SHIP_FAX, this.CELL_SHIP_ADDR1, this.CELL_SHIP_ADDR2, this.CELL_ORDER_IDENTIFIER, this.CELL_SUPP_QUOTE_DT, this.COL_ITEM_ALT_NAME, this.CELL_ETA_DATE, this.CELL_ETD_DATE);
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                //
            }
        }

        public virtual void Delete()
        {
            Dal.SmXlsGroupMapping dbo = null;
            try
            {
                dbo = new Dal.SmXlsGroupMapping();
                dbo.SM_XLS_GROUP_MAPPING_Delete(this.ExcelMapid);
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if ((dbo != null))
                {
                    dbo.Dispose();
                }
            }
        }

        public virtual void Delete(Dal.DataAccess _dataAccess)
        {
            Dal.SmXlsGroupMapping dbo = null;
            try
            {
                dbo = new Dal.SmXlsGroupMapping(_dataAccess);
                dbo.SM_XLS_GROUP_MAPPING_Delete(this.ExcelMapid);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public virtual void Update(Dal.DataAccess _dataAccess)
        {
           Dal.SmXlsGroupMapping dbo = null;
            try
            {
                dbo = new Dal.SmXlsGroupMapping(_dataAccess);
                dbo.SM_XLS_GROUP_MAPPING_Update(this.ExcelMapid, this.GroupId, this.XlsMapCode, this.SectionRowStart, this.ItemRowStart, this.SkipRowsBefItem, this.SkipRowsAftSection, this.CellVrno, this.CellRfqDt,
                    this.CellVessel, this.CellPort, this.CellLateDt, this.CellSuppRef, this.CellValidUpto, this.CellCurrCode, this.CellContact, this.CellPayTerms, this.CellDelTerms, this.CellBuyerRemarks,
                    this.CellSuplrRemarks, this.ColSection, this.ColItemno, this.ColItemRefno, this.ColItemName, this.ColItemQty, this.ColItemUnit, this.ColItemPrice, this.ColItemDiscount, this.ColItemAltQty,
                    this.ColItemAltUnit, this.ColItemAltPrice, this.ColItemDelDays, this.ColItemRemarks, this.ActiveSheet, this.ExitForNoitem, this.ColItemCurr, this.DynSupRmrkOffset, this.OverrideAltQty,
                    this.SkipHiddenRows, this.ItemDiscPercnt, this.ColItemTotal, this.ApplyTotalFormula, this.ReadItemRemarksUptoNo, this.ColItemBuyerRemarks, this.AddToVrno, this.RemoveFromVrno, this.SkipRowsAftItem,
                    this.ItemNoAsRowno, this.ColItemComments, this.SampleFile, this.CellVslImono, this.CellPortName, this.CellDocType, this.ColItemSuppRefno, this.CellSuppExpDt, this.CellSuppLateDt, this.CellSuppLeadDays,
                    this.CellByrCompany, this.CellByrContact, this.CellByrEmail, this.CellByrPhone, this.CellByrMob, this.CellByrFax, this.CellSuppCompany, this.CellSuppContact, this.CellSuppEmail, this.CellSuppPhone,
                    this.CellSuppMob, this.CellSuppFax, this.CellFreightAmt, this.CellOtherAmt, this.CellDiscProvsn, this.DiscProvsnValue, this.AltItemStartOffset, this.AltItemCount, this.CellRfqTitle, this.CellRfqDept,
                    this.CellEquipName, this.CellEquipType, this.CellEquipMaker, this.CellEquipSrno, this.CellEquipDtls, this.CellMsgNo, this.DynSupFreightOffset,
                    this.DynOtherCostOffset, this.DynSupCurrOffset, this.DynBuyRmrkOffset, this.MultiLineDynItemDesc, this.ExcelNameMgr, this.DecimalSeprator, this.DefaultUMO, this.DynHdrDiscountOffset,
                    this.REMOVE_FROM_VESSEL_NAME, this.CELL_BYR_ADDR1, this.CELL_BYR_ADDR2, this.CELL_SUPP_ADDR1, this.CELL_SUPP_ADDR2, this.CELL_BILL_COMPANY, this.CELL_BILL_CONTACT, this.CELL_BILL_EMAIL,
                    this.CELL_BILL_PHONE, this.CELL_BILL_MOB, this.CELL_BILL_FAX, this.CELL_BILL_ADDR1, this.CELL_BILL_ADDR2, this.CELL_SHIP_COMPANY, this.CELL_SHIP_CONTACT, this.CELL_SHIP_EMAIL, this.CELL_SHIP_PHONE,
                    this.CELL_SHIP_MOB, this.CELL_SHIP_FAX, this.CELL_SHIP_ADDR1, this.CELL_SHIP_ADDR2, this.CELL_ORDER_IDENTIFIER, this.CELL_SUPP_QUOTE_DT, this.COL_ITEM_ALT_NAME, this.CELL_ETA_DATE, this.CELL_ETD_DATE);
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if (_dataAccess == null)
                {
                    if ((dbo != null))
                    {
                        dbo.Dispose();
                    }
                }
            }
        }

        public static SmXlsGroupMapping Load_new(System.Nullable<int> EXCEL_MAPID, Dal.DataAccess _dataAccess)
        {
            Dal.SmXlsGroupMapping dbo = null;
            try
            {
                dbo = new Dal.SmXlsGroupMapping(_dataAccess);
                System.Data.DataSet ds = dbo.SM_XLS_GROUP_MAPPING_Select_One(EXCEL_MAPID);
                SmXlsGroupMapping obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmXlsGroupMapping();
                        obj.Fill(ds.Tables[0].Rows[0]);
                    }
                }
                return obj;
            }
            catch (System.Exception)
            {
                throw;
            }    
        }

    }
}