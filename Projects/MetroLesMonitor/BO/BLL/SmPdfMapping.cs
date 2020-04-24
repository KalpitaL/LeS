using System;
using System.IO;
using System.Web;
namespace MetroLesMonitor.Bll
{
    public partial class SmPdfMapping
    {
        private System.Nullable<int> _pdfMapid;
        private System.Nullable<int> _groupid;
        private string _buyerName;
        private string _buyerTel;
        private string _buyerFax;
        private string _buyerEmail;
        private string _buyerContact;
        private string _buyerAddress;
        private string _suppName;
        private string _suppTel;
        private string _suppFax;
        private string _suppEmail;
        private string _suppContact;
        private string _suppAddress;
        private string _billName;
        private string _billContact;
        private string _billTel;
        private string _billFax;
        private string _billEmail;
        private string _billAddress;
        private string _consignName;
        private string _consignContact;
        private string _consignTel;
        private string _consignFax;
        private string _consignEmail;
        private string _consignAddress;
        private string _buyerComments;
        private string _suppComments;
        private string _vrnoHeader;
        private string _vrno;
        private string _poReference;
        private string _quoteReference;
        private string _createdDate;
        private string _lateDate;
        private string _quoteValidity;
        private string _vesselHeader;
        private string _vessel;
        private string _imoHeader;
        private string _imo;
        private string _vesselEta;
        private string _vesselEtd;
        private string _deliveryPortHeader;
        private string _deliveryPort;
        private string _currency;
        private System.Nullable<int> _discountInPrcnt;
        private System.Nullable<char> _decimalSeprator;
        private System.Nullable<int> _includeBlanckLines;
        private System.Nullable<int> _headerLineCount;
        private System.Nullable<int> _footerLineCount;
        private string _dateFormat1;
        private string _dateFormat2;
        private string _itemsTotalHeader;
        private string _frieghtAmtHeader;
        private string _allowanceAmtHeader;
        private string _packingAmtHeader;
        private string _grantTotalHeader;
        private System.Nullable<int> _splitFile;
        private string _constantRows;
        private string _splitStartContent;
        private string _endCommentStartContent;
        private string _extraFields;
        private string _extraFieldsHeader;
        private System.Nullable<int> _addHeaderToComments;
        private System.Nullable<int> _addFooterToComments;
        private string _fieldsFromHeader;
        private string _fieldsFromFooter;
        // Updated on 19Sep2015
        private string _subject;
        private string _equipName;
        private string _equipNameHeader;
        private string _equipType;
        private string _equipTypeHeader;
        private string _equipMaker;
        private string _equipMakerHeader;
        private string _equipSerNo;
        private string _equipSerNoHeader;
        private string _equipRemarks;
        private string _equipRemarksHeader;
        private System.Nullable<int> _readItemBelowContent;
        private System.Nullable<int> _overrideItemQty;
        private System.Nullable<int> _validateItemDescr;
        private string _headerCommentsStartText;
        private string _headerCommentsEndText;
        private System.Nullable<int> _itemDescrUptoLineCount;
        private string _Department;
        private System.Nullable<int> _AddItemToDelDate;
        private System.Nullable<int> _Remark_Header_RemarkSpace;
        private string _header_fLine_Content;
        private string _header_lLine_Content;
        private string _footer_fLine_Content;
        private string _footer_lLine_Content;
        private string _start_Of_SkipText;
        private string _end_Of_SkipText;
        private System.Nullable<int> _add_Skipped_Text_To_Remarks;
        private string _currencyHeader;
        private string _deptHeader;
        private string _quoteRefHeader;
        private string _pocRefHeader;
        private string _subjectHeader;
        private string _delDateHeader;
        private string _docDateHeader;
        private string _quoteExpHeader;
        private string _Header;
        private string _etaHeader;
        private string _etdHeader;
        private string _buyAddrHeader;
        private string _suppAddrHeader;
        private string _shipAddrHeader;
        private string _billAddrHeader;
        private string _itemHeaderCount;
        private string _itemFormat;

        public virtual string ItemHeaderCount
        {
            get
            {
                return _itemHeaderCount;
            }
            set
            {
                _itemHeaderCount = value;
            }
        }

        public virtual string ItemFormat
        {
            get
            {
                return _itemFormat;
            }
            set
            {
                _itemFormat = value;
            }
        }

        public virtual System.Nullable<int> PdfMapid
        {
            get
            {
                return _pdfMapid;
            }
            set
            {
                _pdfMapid = value;
            }
        }

        public virtual System.Nullable<int> Groupid
        {
            get
            {
                return _groupid;
            }
            set
            {
                _groupid = value;
            }
        }

        public virtual string BuyerName
        {
            get
            {
                return _buyerName;
            }
            set
            {
                _buyerName = value;
            }
        }

        public virtual string BuyerTel
        {
            get
            {
                return _buyerTel;
            }
            set
            {
                _buyerTel = value;
            }
        }

        public virtual string BuyerFax
        {
            get
            {
                return _buyerFax;
            }
            set
            {
                _buyerFax = value;
            }
        }

        public virtual string BuyerEmail
        {
            get
            {
                return _buyerEmail;
            }
            set
            {
                _buyerEmail = value;
            }
        }

        public virtual string BuyerContact
        {
            get
            {
                return _buyerContact;
            }
            set
            {
                _buyerContact = value;
            }
        }

        public virtual string BuyerAddress
        {
            get
            {
                return _buyerAddress;
            }
            set
            {
                _buyerAddress = value;
            }
        }

        public virtual string SuppName
        {
            get
            {
                return _suppName;
            }
            set
            {
                _suppName = value;
            }
        }

        public virtual string SuppTel
        {
            get
            {
                return _suppTel;
            }
            set
            {
                _suppTel = value;
            }
        }

        public virtual string SuppFax
        {
            get
            {
                return _suppFax;
            }
            set
            {
                _suppFax = value;
            }
        }

        public virtual string SuppEmail
        {
            get
            {
                return _suppEmail;
            }
            set
            {
                _suppEmail = value;
            }
        }

        public virtual string SuppContact
        {
            get
            {
                return _suppContact;
            }
            set
            {
                _suppContact = value;
            }
        }

        public virtual string SuppAddress
        {
            get
            {
                return _suppAddress;
            }
            set
            {
                _suppAddress = value;
            }
        }

        public virtual string BillName
        {
            get
            {
                return _billName;
            }
            set
            {
                _billName = value;
            }
        }

        public virtual string BillContact
        {
            get
            {
                return _billContact;
            }
            set
            {
                _billContact = value;
            }
        }

        public virtual string BillTel
        {
            get
            {
                return _billTel;
            }
            set
            {
                _billTel = value;
            }
        }

        public virtual string BillFax
        {
            get
            {
                return _billFax;
            }
            set
            {
                _billFax = value;
            }
        }

        public virtual string BillEmail
        {
            get
            {
                return _billEmail;
            }
            set
            {
                _billEmail = value;
            }
        }

        public virtual string BillAddress
        {
            get
            {
                return _billAddress;
            }
            set
            {
                _billAddress = value;
            }
        }

        public virtual string ConsignName
        {
            get
            {
                return _consignName;
            }
            set
            {
                _consignName = value;
            }
        }

        public virtual string ConsignContact
        {
            get
            {
                return _consignContact;
            }
            set
            {
                _consignContact = value;
            }
        }

        public virtual string ConsignTel
        {
            get
            {
                return _consignTel;
            }
            set
            {
                _consignTel = value;
            }
        }

        public virtual string ConsignFax
        {
            get
            {
                return _consignFax;
            }
            set
            {
                _consignFax = value;
            }
        }

        public virtual string ConsignEmail
        {
            get
            {
                return _consignEmail;
            }
            set
            {
                _consignEmail = value;
            }
        }

        public virtual string ConsignAddress
        {
            get
            {
                return _consignAddress;
            }
            set
            {
                _consignAddress = value;
            }
        }

        public virtual string BuyerComments
        {
            get
            {
                return _buyerComments;
            }
            set
            {
                _buyerComments = value;
            }
        }

        public virtual string SuppComments
        {
            get
            {
                return _suppComments;
            }
            set
            {
                _suppComments = value;
            }
        }

        public virtual string VrnoHeader
        {
            get
            {
                return _vrnoHeader;
            }
            set
            {
                _vrnoHeader = value;
            }
        }

        public virtual string Vrno
        {
            get
            {
                return _vrno;
            }
            set
            {
                _vrno = value;
            }
        }

        public virtual string PoReference
        {
            get
            {
                return _poReference;
            }
            set
            {
                _poReference = value;
            }
        }

        public virtual string QuoteReference
        {
            get
            {
                return _quoteReference;
            }
            set
            {
                _quoteReference = value;
            }
        }

        public virtual string CreatedDate
        {
            get
            {
                return _createdDate;
            }
            set
            {
                _createdDate = value;
            }
        }

        public virtual string LateDate
        {
            get
            {
                return _lateDate;
            }
            set
            {
                _lateDate = value;
            }
        }

        public virtual string QuoteValidity
        {
            set
            {
                _quoteValidity = value;
            }
            get
            {
                return _quoteValidity;
            }
        }

        public virtual string VesselHeader
        {
            get
            {
                return _vesselHeader;
            }
            set
            {
                _vesselHeader = value;
            }
        }

        public virtual string Vessel
        {
            get
            {
                return _vessel;
            }
            set
            {
                _vessel = value;
            }
        }

        public virtual string ImoHeader
        {
            get
            {
                return _imoHeader;
            }
            set
            {
                _imoHeader = value;
            }
        }

        public virtual string IMO
        {
            get
            {
                return _imo;
            }
            set
            {
                _imo = value;
            }
        }

        public virtual string VesselEta
        {
            get
            {
                return _vesselEta;
            }
            set
            {
                _vesselEta = value;
            }
        }

        public virtual string VesselEtd
        {
            get
            {
                return _vesselEtd;
            }
            set
            {
                _vesselEtd = value;
            }
        }

        public virtual string DeliveryPortHeader
        {
            get
            {
                return _deliveryPortHeader;
            }
            set
            {
                _deliveryPortHeader = value;
            }
        }

        public virtual string DeliveryPort
        {
            get
            {
                return _deliveryPort;
            }
            set
            {
                _deliveryPort = value;
            }
        }

        public virtual string Currency
        {
            get
            {
                return _currency;
            }
            set
            {
                _currency = value;
            }
        }

        public virtual System.Nullable<int> DiscountInPrcnt
        {
            get
            {
                return _discountInPrcnt;
            }
            set
            {
                _discountInPrcnt = value;
            }
        }

        public virtual System.Nullable<char> DecimalSeprator
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

        public virtual System.Nullable<int> IncludeBlanckLines
        {
            get
            {
                return _includeBlanckLines;
            }
            set
            {
                _includeBlanckLines = value;
            }
        }

        public virtual System.Nullable<int> HeaderLineCount
        {
            get
            {
                return _headerLineCount;
            }
            set
            {
                _headerLineCount = value;
            }
        }

        public virtual System.Nullable<int> FooterLineCount
        {
            get
            {
                return _footerLineCount;
            }
            set
            {
                _footerLineCount = value;
            }
        }

        public virtual string DateFormat1
        {
            get
            {
                return _dateFormat1;
            }
            set
            {
                _dateFormat1 = value;
            }
        }

        public virtual string DateFormat2
        {
            get
            {
                return _dateFormat2;
            }
            set
            {
                _dateFormat2 = value;
            }
        }

        public virtual string ItemsTotalHeader
        {
            get
            {
                return _itemsTotalHeader;
            }
            set
            {
                _itemsTotalHeader = value;
            }
        }

        public virtual string FrieghtAmtHeader
        {
            get
            {
                return _frieghtAmtHeader;
            }
            set
            {
                _frieghtAmtHeader = value;
            }
        }

        public virtual string AllowanceAmtHeader
        {
            get
            {
                return _allowanceAmtHeader;
            }
            set
            {
                _allowanceAmtHeader = value;
            }
        }

        public virtual string PackingAmtHeader
        {
            get
            {
                return _packingAmtHeader;
            }
            set
            {
                _packingAmtHeader = value;
            }
        }

        public virtual string GrantTotalHeader
        {
            get
            {
                return _grantTotalHeader;
            }
            set
            {
                _grantTotalHeader = value;
            }
        }

        public virtual System.Nullable<int> SplitFile
        {
            get
            {
                return _splitFile;
            }
            set
            {
                _splitFile = value;
            }
        }

        public virtual string ConstantRows
        {
            get
            {
                return _constantRows;
            }
            set
            {
                _constantRows = value;
            }
        }

        public virtual string SplitStartContent
        {
            get
            {
                return _splitStartContent;
            }
            set
            {
                _splitStartContent = value;
            }
        }

        public virtual string EndCommentStartContent
        {
            get
            {
                return _endCommentStartContent;
            }
            set
            {
                _endCommentStartContent = value;
            }
        }

        public virtual string ExtraFields
        {
            get
            {
                return _extraFields;
            }
            set
            {
                _extraFields = value;
            }
        }

        public virtual string ExtraFieldsHeader
        {
            get
            {
                return _extraFieldsHeader;
            }
            set
            {
                _extraFieldsHeader = value;
            }
        }

        public virtual System.Nullable<int> AddHeaderToComments
        {
            get
            {
                return _addHeaderToComments;
            }
            set
            {
                _addHeaderToComments = value;
            }
        }

        public virtual System.Nullable<int> AddFooterToComments
        {
            get
            {
                return _addFooterToComments;
            }
            set
            {
                _addFooterToComments = value;
            }
        }

        public virtual string FieldsFromHeader
        {
            get
            {
                return _fieldsFromHeader;
            }
            set
            {
                _fieldsFromHeader = value;
            }
        }

        public virtual string FieldsFromFooter
        {
            get
            {
                return _fieldsFromFooter;
            }
            set
            {
                _fieldsFromFooter = value;
            }
        }

        public virtual string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
            }
        }

        public virtual string EquipName
        {
            get
            {
                return _equipName;
            }
            set
            {
                _equipName = value;
            }
        }

        public virtual string EquipNameHeader
        {
            get
            {
                return _equipNameHeader;
            }
            set
            {
                _equipNameHeader = value;
            }
        }

        public virtual string EquipMaker
        {
            get
            {
                return _equipMaker;
            }
            set
            {
                _equipMaker = value;
            }
        }

        public virtual string EquipMakerHeader
        {
            get
            {
                return _equipMakerHeader;
            }
            set
            {
                _equipMakerHeader = value;
            }
        }

        public virtual string EquipSerno
        {
            get
            {
                return _equipSerNo;
            }
            set
            {
                _equipSerNo = value;
            }
        }

        public virtual string EquipSernoHeader
        {
            get
            {
                return _equipSerNoHeader;
            }
            set
            {
                _equipSerNoHeader = value;
            }
        }

        public virtual string EquipType
        {
            get
            {
                return _equipType;
            }
            set
            {
                _equipType = value;
            }
        }

        public virtual string EquipTypeHeader
        {
            get
            {
                return _equipTypeHeader;
            }
            set
            {
                _equipTypeHeader = value;
            }
        }

        public virtual string EquipRemarks
        {
            get
            {
                return _equipRemarks;
            }
            set
            {
                _equipRemarks = value;
            }
        }

        public virtual string EquipRemarksHeader
        {
            get
            {
                return _equipRemarksHeader;
            }
            set
            {
                _equipRemarksHeader = value;
            }
        }

        public virtual System.Nullable<int> ReadContentBelowItems
        {
            get
            {
                return _readItemBelowContent;
            }
            set
            {
                _readItemBelowContent = value;
            }
        }

        public virtual System.Nullable<int> OverrideItemQty
        {
            get
            {
                return _overrideItemQty;
            }
            set
            {
                _overrideItemQty = value;
            }
        }

        public virtual System.Nullable<int> ValidateItemDescr
        {
            get
            {
                return _validateItemDescr;
            }
            set
            {
                _validateItemDescr = value;
            }
        }

        public virtual string HeaderCommentsStartText
        {
            get
            {
                return _headerCommentsStartText;
            }
            set
            {
                _headerCommentsStartText = value;
            }
        }

        public virtual string HeaderCommentsEndText
        {
            get
            {
                return _headerCommentsEndText;
            }
            set
            {
                _headerCommentsEndText = value;
            }
        }

        public virtual System.Nullable<int> ItemDescrUptoLineCount
        {
            get
            {
                return _itemDescrUptoLineCount;
            }
            set
            {
                _itemDescrUptoLineCount = value;
            }
        }

        public virtual string Department
        {
            get
            {
                return _Department;
            }
            set
            {
                _Department = value;
            }
        }

        public virtual System.Nullable<int> AddItemToDelDate
        {
            get
            {
                return _AddItemToDelDate;
            }
            set
            {
                _AddItemToDelDate = value;
            }
        }

        public virtual System.Nullable<int> Remark_Header_RemarkSpace
        {
            get
            {
                return _Remark_Header_RemarkSpace;
            }
            set
            {
                _Remark_Header_RemarkSpace = value;
            }
        }

        public virtual string Header_fLine_Content
        {
            get
            {
                return _header_fLine_Content;
            }
            set
            {
                _header_fLine_Content = value;
            }
        }

        public virtual string Header_lLine_Content
        {
            get
            {
                return _header_lLine_Content;
            }
            set
            {
                _header_lLine_Content = value;
            }
        }

        public virtual string Footer_fLine_Content
        {
            get
            {
                return _footer_fLine_Content;
            }
            set
            {
                _footer_fLine_Content = value;
            }
        }

        public virtual string Footer_lLine_Content
        {
            get
            {
                return _footer_lLine_Content;
            }
            set
            {
                _footer_lLine_Content = value;
            }
        }

        public virtual string Start_Of_SkipText
        {
            get
            {
                return _start_Of_SkipText;
            }
            set
            {
                _start_Of_SkipText = value;
            }
        }

        public virtual string End_Of_SkipText
        {
            get
            {
                return _end_Of_SkipText;
            }
            set
            {
                _end_Of_SkipText = value;
            }
        }

        public virtual System.Nullable<int> Add_Skipped_Text_To_Remarks
        {
            get
            {
                return _add_Skipped_Text_To_Remarks;
            }
            set
            {
                _add_Skipped_Text_To_Remarks = value;
            }
        }

        public virtual string CurrencyHeader
        {
            get
            {
                return _currencyHeader;
            }
            set
            {
                _currencyHeader = value;
            }
        }

        public virtual string DeptHeader
        {
            get
            {
                return _deptHeader;
            }
            set
            {
                _deptHeader = value;
            }
        }

        public virtual string QuoteRefHeader
        {
            get
            {
                return _quoteRefHeader;
            }
            set
            {
                _quoteRefHeader = value;
            }
        }

        public virtual string PocRefHeader
        {
            get
            {
                return _pocRefHeader;
            }
            set
            {
                _pocRefHeader = value;
            }
        }

        public virtual string SubjectHeader
        {
            get
            {
                return _subjectHeader;
            }
            set
            {
                _subjectHeader = value;
            }
        }

        public virtual string DelDateHeader
        {
            get
            {
                return _delDateHeader;
            }
            set
            {
                _delDateHeader = value;
            }
        }

        public virtual string DocDateHeader
        {
            get
            {
                return _docDateHeader;
            }
            set
            {
                _docDateHeader = value;
            }
        }

        public virtual string QuoteExpHeader
        {
            get
            {
                return _quoteExpHeader;
            }
            set
            {
                _quoteExpHeader = value;
            }
        }

        public virtual string EtaHeader
        {
            get
            {
                return _etaHeader;
            }
            set
            {
                _etaHeader = value;
            }
        }

        public virtual string EtdHeader
        {
            get
            {
                return _etdHeader;
            }
            set
            {
                _etdHeader = value;
            }
        }

        public virtual string BuyAddrHeader
        {
            get
            {
                return _buyAddrHeader;
            }
            set
            {
                _buyAddrHeader = value;
            }
        }

        public virtual string SuppAddrHeader
        {
            get
            {
                return _suppAddrHeader;
            }
            set
            {
                _suppAddrHeader = value;
            }
        }

        public virtual string ShipAddrHeader
        {
            get
            {
                return _shipAddrHeader;
            }
            set
            {
                _shipAddrHeader = value;
            }
        }

        public virtual string BillAddrHeader
        {
            get
            {
                return _billAddrHeader;
            }
            set
            {
                _billAddrHeader = value;
            }
        }

        private void Clean()
        {
            this.PdfMapid = null;
            this.Groupid = null;
            this.BuyerName = string.Empty;
            this.BuyerTel = string.Empty;
            this.BuyerFax = string.Empty;
            this.BuyerEmail = string.Empty;
            this.BuyerContact = string.Empty;
            this.BuyerAddress = string.Empty;
            this.SuppName = string.Empty;
            this.SuppTel = string.Empty;
            this.SuppFax = string.Empty;
            this.SuppEmail = string.Empty;
            this.SuppContact = string.Empty;
            this.SuppAddress = string.Empty;
            this.BillName = string.Empty;
            this.BillContact = string.Empty;
            this.BillTel = string.Empty;
            this.BillFax = string.Empty;
            this.BillEmail = string.Empty;
            this.BillAddress = string.Empty;
            this.ConsignName = string.Empty;
            this.ConsignContact = string.Empty;
            this.ConsignTel = string.Empty;
            this.ConsignFax = string.Empty;
            this.ConsignEmail = string.Empty;
            this.ConsignAddress = string.Empty;
            this.BuyerComments = string.Empty;
            this.VrnoHeader = string.Empty;
            this.Vrno = string.Empty;
            this.PoReference = string.Empty;
            this.QuoteReference = string.Empty;
            this.CreatedDate = string.Empty;
            this.LateDate = string.Empty;
            this.VesselHeader = string.Empty;
            this.Vessel = string.Empty;
            this.ImoHeader = string.Empty;
            this.IMO = string.Empty;
            this.VesselEta = string.Empty;
            this.VesselEtd = string.Empty;
            this.DeliveryPortHeader = string.Empty;
            this.DeliveryPort = string.Empty;
            this.Currency = string.Empty;
            this.DiscountInPrcnt = null;
            this.DecimalSeprator = null;
            this.IncludeBlanckLines = null;
            this.HeaderLineCount = null;
            this.FooterLineCount = null;
            this.DateFormat1 = string.Empty;
            this.DateFormat2 = string.Empty;
            this.ItemsTotalHeader = string.Empty;
            this.FrieghtAmtHeader = string.Empty;
            this.GrantTotalHeader = string.Empty;
            this.SplitFile = null;
            this.ConstantRows = string.Empty;
            this.SplitStartContent = string.Empty;
            this.EndCommentStartContent = string.Empty;

            // ADDED ON 09-MAR-2015
            this.SuppComments = string.Empty;
            this.QuoteValidity = string.Empty;
            this.ExtraFields = string.Empty;
            this.ExtraFieldsHeader = string.Empty;
            this.FieldsFromFooter = string.Empty;
            this.FieldsFromHeader = string.Empty;
            this.AllowanceAmtHeader = string.Empty;
            this.PackingAmtHeader = string.Empty;
            this.AddHeaderToComments = null;
            this.AddFooterToComments = null;

            // updated on 19-Sep-2015
            this.Subject = string.Empty;
            this.EquipMaker = string.Empty;
            this.EquipMakerHeader = string.Empty;
            this.EquipName = string.Empty;
            this.EquipNameHeader = string.Empty;
            this.EquipType = string.Empty;
            this.EquipTypeHeader = string.Empty;
            this.EquipSerno = string.Empty;
            this.EquipSernoHeader = string.Empty;
            this.EquipRemarks = string.Empty;
            this.EquipRemarksHeader = string.Empty;
            this.ReadContentBelowItems = null;

            // UPDATED 09-MAR-2016
            this.OverrideItemQty = null;
            // ADDED ON 01-APR-2016
            this.ValidateItemDescr = null;

            // ADDED ON 14-APRIL-2016 -- Sayak
            this.HeaderCommentsStartText = string.Empty;
            this.HeaderCommentsEndText = string.Empty;
            this.ItemDescrUptoLineCount = null;

            this.Department = string.Empty;
            this.AddItemToDelDate = null;
            this.Remark_Header_RemarkSpace = null;

            // ADDED ON 27-10-2017 -- Sayak
            this.Header_fLine_Content = string.Empty;
            this.Header_lLine_Content = string.Empty;
            this.Footer_fLine_Content = string.Empty;
            this.Footer_lLine_Content = string.Empty;

            this.Start_Of_SkipText = string.Empty;
            this.End_Of_SkipText = string.Empty;
            this.Add_Skipped_Text_To_Remarks = null;

            //ADDED ON 08-1-2018 -- Sayak

            this.CurrencyHeader = string.Empty;
            this.DeptHeader = string.Empty;
            this.QuoteRefHeader = string.Empty;
            this.PocRefHeader = string.Empty;
            this.SubjectHeader = string.Empty;
            this.DelDateHeader = string.Empty;
            this.DocDateHeader = string.Empty;
            this.QuoteExpHeader = string.Empty;
            this.EtaHeader = string.Empty;
            this.EtdHeader = string.Empty;
            this.BuyAddrHeader = string.Empty;
            this.SuppAddrHeader = string.Empty;
            this.ShipAddrHeader = string.Empty;
            this.BillAddrHeader = string.Empty;

            this.ItemFormat = string.Empty;
            this.ItemHeaderCount = string.Empty;
        }

        private void Fill(System.Data.DataRow dr)
        {
            try
            {
                this.Clean();
                if ((dr["PDF_MAPID"] != System.DBNull.Value))
                {
                    this.PdfMapid = ((System.Nullable<int>)(dr["PDF_MAPID"]));
                }
                if ((dr["GROUPID"] != System.DBNull.Value))
                {
                    this.Groupid = ((System.Nullable<int>)(dr["GROUPID"]));
                }
                if ((dr["BUYER_NAME"] != System.DBNull.Value))
                {
                    this.BuyerName = ((string)(dr["BUYER_NAME"]));

                }
                if ((dr["BUYER_TEL"] != System.DBNull.Value))
                {
                    this.BuyerTel = ((string)(dr["BUYER_TEL"]));

                }
                if ((dr["BUYER_FAX"] != System.DBNull.Value))
                {
                    this.BuyerFax = ((string)(dr["BUYER_FAX"]));

                }
                if ((dr["BUYER_EMAIL"] != System.DBNull.Value))
                {
                    this.BuyerEmail = ((string)(dr["BUYER_EMAIL"]));

                }
                if ((dr["BUYER_CONTACT"] != System.DBNull.Value))
                {
                    this.BuyerContact = ((string)(dr["BUYER_CONTACT"]));

                }
                if ((dr["BUYER_ADDRESS"] != System.DBNull.Value))
                {
                    this.BuyerAddress = ((string)(dr["BUYER_ADDRESS"]));

                }
                if ((dr["SUPP_NAME"] != System.DBNull.Value))
                {
                    this.SuppName = ((string)(dr["SUPP_NAME"]));

                }
                if ((dr["SUPP_TEL"] != System.DBNull.Value))
                {
                    this.SuppTel = ((string)(dr["SUPP_TEL"]));

                }
                if ((dr["SUPP_FAX"] != System.DBNull.Value))
                {
                    this.SuppFax = ((string)(dr["SUPP_FAX"]));

                }
                if ((dr["SUPP_EMAIL"] != System.DBNull.Value))
                {
                    this.SuppEmail = ((string)(dr["SUPP_EMAIL"]));
                }
                if ((dr["SUPP_CONTACT"] != System.DBNull.Value))
                {
                    this.SuppContact = ((string)(dr["SUPP_CONTACT"]));
                }
                if ((dr["SUPP_ADDRESS"] != System.DBNull.Value))
                {
                    this.SuppAddress = ((string)(dr["SUPP_ADDRESS"]));
                }
                if ((dr["BILL_NAME"] != System.DBNull.Value))
                {
                    this.BillName = ((string)(dr["BILL_NAME"]));
                }
                if ((dr["BILL_CONTACT"] != System.DBNull.Value))
                {
                    this.BillContact = ((string)(dr["BILL_CONTACT"]));
                }
                if ((dr["BILL_TEL"] != System.DBNull.Value))
                {
                    this.BillTel = ((string)(dr["BILL_TEL"]));
                }
                if ((dr["BILL_FAX"] != System.DBNull.Value))
                {
                    this.BillFax = ((string)(dr["BILL_FAX"]));
                }
                if ((dr["BILL_EMAIL"] != System.DBNull.Value))
                {
                    this.BillEmail = ((string)(dr["BILL_EMAIL"]));
                }
                if ((dr["BILL_ADDRESS"] != System.DBNull.Value))
                {
                    this.BillAddress = ((string)(dr["BILL_ADDRESS"]));
                }
                if ((dr["CONSIGN_NAME"] != System.DBNull.Value))
                {
                    this.ConsignName = ((string)(dr["CONSIGN_NAME"]));
                }
                if ((dr["CONSIGN_CONTACT"] != System.DBNull.Value))
                {
                    this.ConsignContact = ((string)(dr["CONSIGN_CONTACT"]));
                }
                if ((dr["CONSIGN_TEL"] != System.DBNull.Value))
                {
                    this.ConsignTel = ((string)(dr["CONSIGN_TEL"]));
                }
                if ((dr["CONSIGN_FAX"] != System.DBNull.Value))
                {
                    this.ConsignFax = ((string)(dr["CONSIGN_FAX"]));
                }
                if ((dr["CONSIGN_EMAIL"] != System.DBNull.Value))
                {
                    this.ConsignEmail = ((string)(dr["CONSIGN_EMAIL"]));
                }
                if ((dr["CONSIGN_ADDRESS"] != System.DBNull.Value))
                {
                    this.ConsignAddress = ((string)(dr["CONSIGN_ADDRESS"]));
                }
                if ((dr["BUYER_COMMENTS"] != System.DBNull.Value))
                {
                    this.BuyerComments = ((string)(dr["BUYER_COMMENTS"]));
                }
                if ((dr["VRNO_HEADER"] != System.DBNull.Value))
                {
                    this.VrnoHeader = ((string)(dr["VRNO_HEADER"]));
                }
                if ((dr["VRNO"] != System.DBNull.Value))
                {
                    this.Vrno = ((string)(dr["VRNO"]));
                }
                if ((dr["PO_REFERENCE"] != System.DBNull.Value))
                {
                    this.PoReference = ((string)(dr["PO_REFERENCE"]));
                }
                if ((dr["QUOTE_REFERENCE"] != System.DBNull.Value))
                {
                    this.QuoteReference = ((string)(dr["QUOTE_REFERENCE"]));
                }
                if ((dr["CREATED_DATE"] != System.DBNull.Value))
                {
                    this.CreatedDate = ((string)(dr["CREATED_DATE"]));
                }
                if ((dr["LATE_DATE"] != System.DBNull.Value))
                {
                    this.LateDate = ((string)(dr["LATE_DATE"]));
                }
                if ((dr["VESSEL_HEADER"] != System.DBNull.Value))
                {
                    this.VesselHeader = ((string)(dr["VESSEL_HEADER"]));
                }
                if ((dr["VESSEL"] != System.DBNull.Value))
                {
                    this.Vessel = ((string)(dr["VESSEL"]));
                }
                if ((dr["IMO_NO_HEADER"] != System.DBNull.Value))
                {
                    this.ImoHeader = ((string)(dr["IMO_NO_HEADER"]));
                }
                if ((dr["IMO_NO"] != System.DBNull.Value))
                {
                    this.IMO = ((string)(dr["IMO_NO"]));
                }
                if ((dr["VESSEL_ETA"] != System.DBNull.Value))
                {
                    this.VesselEta = ((string)(dr["VESSEL_ETA"]));
                }
                if ((dr["VESSEL_ETD"] != System.DBNull.Value))
                {
                    this.VesselEtd = ((string)(dr["VESSEL_ETD"]));
                }
                if ((dr["DELIVERY_PORT_HEADER"] != System.DBNull.Value))
                {
                    this.DeliveryPortHeader = ((string)(dr["DELIVERY_PORT_HEADER"]));
                }
                if ((dr["DELIVERY_PORT"] != System.DBNull.Value))
                {
                    this.DeliveryPort = ((string)(dr["DELIVERY_PORT"]));
                }
                if ((dr["CURRENCY"] != System.DBNull.Value))
                {
                    this.Currency = ((string)(dr["CURRENCY"]));
                }
                if ((dr["DISCOUNT_IN_PRCNT"] != System.DBNull.Value))
                {
                    this.DiscountInPrcnt = ((System.Nullable<int>)(dr["DISCOUNT_IN_PRCNT"]));
                }
                if ((dr["DECIMAL_SEPRATOR"] != System.DBNull.Value))
                {
                    this.DecimalSeprator = (System.Convert.ToChar(dr["DECIMAL_SEPRATOR"]));
                }
                if ((dr["INCLUDE_BLANCK_LINES"] != System.DBNull.Value))
                {
                    this.IncludeBlanckLines = ((System.Nullable<int>)(dr["INCLUDE_BLANCK_LINES"]));
                }
                if ((dr["HEADER_LINE_COUNT"] != System.DBNull.Value))
                {
                    this.HeaderLineCount = ((System.Nullable<int>)(dr["HEADER_LINE_COUNT"]));
                }
                if ((dr["FOOTER_LINE_COUNT"] != System.DBNull.Value))
                {
                    this.FooterLineCount = ((System.Nullable<int>)(dr["FOOTER_LINE_COUNT"]));
                }
                if ((dr["DATE_FORMAT_1"] != System.DBNull.Value))
                {
                    this.DateFormat1 = ((string)(dr["DATE_FORMAT_1"]));
                }
                if ((dr["DATE_FORMAT_2"] != System.DBNull.Value))
                {
                    this.DateFormat2 = ((string)(dr["DATE_FORMAT_2"]));
                }
                if ((dr["ITEMS_TOTAL_HEADER"] != System.DBNull.Value))
                {
                    this.ItemsTotalHeader = ((string)(dr["ITEMS_TOTAL_HEADER"]));
                }
                if ((dr["FRIEGHT_AMT_HEADER"] != System.DBNull.Value))
                {
                    this.FrieghtAmtHeader = ((string)(dr["FRIEGHT_AMT_HEADER"]));
                }
                if ((dr["GRANT_TOTAL_HEADER"] != System.DBNull.Value))
                {
                    this.GrantTotalHeader = ((string)(dr["GRANT_TOTAL_HEADER"]));
                }
                if ((dr["SPLIT_FILE"] != System.DBNull.Value))
                {
                    this.SplitFile = ((System.Nullable<int>)(dr["SPLIT_FILE"]));
                }
                if ((dr["CONSTANT_ROWS"] != System.DBNull.Value))
                {
                    this.ConstantRows = ((string)(dr["CONSTANT_ROWS"]));
                }
                if ((dr["SPLIT_START_CONTENT"] != System.DBNull.Value))
                {
                    this.SplitStartContent = ((string)(dr["SPLIT_START_CONTENT"]));
                }
                if ((dr["END_COMMENT_START_CONTENT"] != System.DBNull.Value))
                {
                    this.EndCommentStartContent = ((string)(dr["END_COMMENT_START_CONTENT"]));
                }
                // ADDED ON 09-MAR-2015
                if ((dr["SUPP_COMMENTS"] != System.DBNull.Value))
                {
                    this.SuppComments = ((string)(dr["SUPP_COMMENTS"]));
                }
                if ((dr["QUOTE_VALIDITY"] != System.DBNull.Value))
                {
                    this.QuoteValidity = ((string)(dr["QUOTE_VALIDITY"]));
                }
                if ((dr["ALLOWANCE_AMT_HEADER"] != System.DBNull.Value))
                {
                    this.AllowanceAmtHeader = ((string)(dr["ALLOWANCE_AMT_HEADER"]));
                }
                if ((dr["PACKING_AMT_HEADER"] != System.DBNull.Value))
                {
                    this.PackingAmtHeader = ((string)(dr["PACKING_AMT_HEADER"]));
                }
                if ((dr["FIELDS_FROM_HEADER"] != System.DBNull.Value))
                {
                    this.FieldsFromHeader = ((string)(dr["FIELDS_FROM_HEADER"]));
                }
                if ((dr["FIELDS_FROM_FOOTER"] != System.DBNull.Value))
                {
                    this.FieldsFromFooter = ((string)(dr["FIELDS_FROM_FOOTER"]));
                }
                if ((dr["EXTRA_FIELDS"] != System.DBNull.Value))
                {
                    this.ExtraFields = ((string)(dr["EXTRA_FIELDS"]));
                }
                if ((dr["EXTRA_FIELDS_HEADER"] != System.DBNull.Value))
                {
                    this.ExtraFieldsHeader = ((string)(dr["EXTRA_FIELDS_HEADER"]));
                }
                if ((dr["ADD_HEADER_TO_COMMENTS"] != System.DBNull.Value))
                {
                    this.AddHeaderToComments = ((System.Nullable<int>)(dr["ADD_HEADER_TO_COMMENTS"]));
                }
                if ((dr["ADD_FOOTER_TO_COMMENTS"] != System.DBNull.Value))
                {
                    this.AddFooterToComments = ((System.Nullable<int>)(dr["ADD_FOOTER_TO_COMMENTS"]));
                }

                // UPDATED ON 19-SEP-2015
                if ((dr["SUBJECT"] != System.DBNull.Value))
                {
                    this.Subject = ((string)(dr["SUBJECT"]));
                }
                if ((dr["EQUIP_NAME"] != System.DBNull.Value))
                {
                    this.EquipName = ((string)(dr["EQUIP_NAME"]));
                }
                if ((dr["EQUIP_TYPE"] != System.DBNull.Value))
                {
                    this.EquipType = ((string)(dr["EQUIP_TYPE"]));
                }
                if ((dr["EQUIP_MAKER"] != System.DBNull.Value))
                {
                    this.EquipMaker = ((string)(dr["EQUIP_MAKER"]));
                }
                if ((dr["EQUIP_SERNO"] != System.DBNull.Value))
                {
                    this.EquipSerno = ((string)(dr["EQUIP_SERNO"]));
                }
                if ((dr["EQUIP_REMARKS"] != System.DBNull.Value))
                {
                    this.EquipRemarks = ((string)(dr["EQUIP_REMARKS"]));
                }
                if ((dr["EQUIP_NAME_HEADER"] != System.DBNull.Value))
                {
                    this.EquipNameHeader = ((string)(dr["EQUIP_NAME_HEADER"]));
                }
                if ((dr["EQUIP_TYPE_HEADER"] != System.DBNull.Value))
                {
                    this.EquipTypeHeader = ((string)(dr["EQUIP_TYPE_HEADER"]));
                }
                if ((dr["EQUIP_MAKER_HEADER"] != System.DBNull.Value))
                {
                    this.EquipMakerHeader = ((string)(dr["EQUIP_MAKER_HEADER"]));
                }
                if ((dr["EQUIP_SERNO_HEADER"] != System.DBNull.Value))
                {
                    this.EquipSernoHeader = ((string)(dr["EQUIP_SERNO_HEADER"]));
                }
                if ((dr["EQUIP_REMARKS_HEADER"] != System.DBNull.Value))
                {
                    this.EquipRemarksHeader = ((string)(dr["EQUIP_REMARKS_HEADER"]));
                }
                if ((dr["READ_CONTENT_BELOW_ITEM"] != System.DBNull.Value))
                {
                    this.ReadContentBelowItems = ((System.Nullable<int>)(dr["READ_CONTENT_BELOW_ITEM"]));
                }
                // ADDED ON 09-MAR-2016
                if ((dr["OVERRIDE_ITEM_QTY"] != System.DBNull.Value))
                {
                    this.OverrideItemQty = ((System.Nullable<int>)(dr["OVERRIDE_ITEM_QTY"]));
                }
                // ADDED ON 01-APR-2016
                if ((dr["VALIDATE_ITEM_DESCR"] != System.DBNull.Value))
                {
                    this.ValidateItemDescr = ((System.Nullable<int>)(dr["VALIDATE_ITEM_DESCR"]));
                }

                // ADDED ON 14-APRIL-2016 -- Sayak
                if ((dr["HEADER_COMMENTS_START_TEXT"] != System.DBNull.Value))
                {
                    this.HeaderCommentsStartText = ((string)(dr["HEADER_COMMENTS_START_TEXT"]));
                }

                if ((dr["HEADER_COMMENTS_END_TEXT"] != System.DBNull.Value))
                {
                    this.HeaderCommentsEndText = ((string)(dr["HEADER_COMMENTS_END_TEXT"]));
                }

                if ((dr["ITEM_DESCR_UPTO_LINE_COUNT"] != System.DBNull.Value))
                {
                    this.ItemDescrUptoLineCount = ((System.Nullable<int>)(dr["ITEM_DESCR_UPTO_LINE_COUNT"]));
                }

                if ((dr["DEPARTMENT"] != System.DBNull.Value))
                {
                    this.Department = ((string)(dr["DEPARTMENT"]));
                }

                if ((dr["ADD_ITEM_DELDATE_TO_HEADER"] != System.DBNull.Value))
                {
                    this.AddItemToDelDate = ((System.Nullable<int>)(dr["ADD_ITEM_DELDATE_TO_HEADER"]));
                }

                if ((dr["REM_HEADER_REMARK_SPACE"] != System.DBNull.Value))
                {
                    this.Remark_Header_RemarkSpace = ((System.Nullable<int>)(dr["REM_HEADER_REMARK_SPACE"]));
                }

                // ADDED ON 27-10-2017 -- Sayak
                if ((dr["HEADER_FLINE_CONTENT"] != System.DBNull.Value))
                {
                    this.Header_fLine_Content = ((string)(dr["HEADER_FLINE_CONTENT"]));
                }
                if ((dr["HEADER_LLINE_CONTENT"] != System.DBNull.Value))
                {
                    this.Header_lLine_Content = ((string)(dr["HEADER_LLINE_CONTENT"]));
                }
                if ((dr["FOOTER_FLINE_CONTENT"] != System.DBNull.Value))
                {
                    this.Footer_fLine_Content = ((string)(dr["FOOTER_FLINE_CONTENT"]));
                }
                if ((dr["FOOTER_LLINE_CONTENT"] != System.DBNull.Value))
                {
                    this.Footer_lLine_Content = ((string)(dr["FOOTER_LLINE_CONTENT"]));
                }

                if ((dr["SKIP_TEXT_START"] != System.DBNull.Value))
                {
                    this.Start_Of_SkipText = ((string)(dr["SKIP_TEXT_START"]));
                }
                if ((dr["SKIP_TEXT_END"] != System.DBNull.Value))
                {
                    this.End_Of_SkipText = ((string)(dr["SKIP_TEXT_END"]));
                }
                if ((dr["ADD_SKIPPED_TXT_TO_REMAKRS"] != System.DBNull.Value))
                {
                    this.Add_Skipped_Text_To_Remarks = ((System.Nullable<int>)(dr["ADD_SKIPPED_TXT_TO_REMAKRS"]));
                }

                // ADDED ON 08-01-2018 -- Sayak

                if ((dr["CURR_HEADER"] != System.DBNull.Value))
                {
                    this.CurrencyHeader = ((string)(dr["CURR_HEADER"]));
                }
                if ((dr["DEPT_HEADER"] != System.DBNull.Value))
                {
                    this.DeptHeader = ((string)(dr["DEPT_HEADER"]));
                }
                if ((dr["QUOTE_REF_HEADER"] != System.DBNull.Value))
                {
                    this.QuoteRefHeader = ((string)(dr["QUOTE_REF_HEADER"]));
                }
                if ((dr["POC_REF_HEADER"] != System.DBNull.Value))
                {
                    this.PocRefHeader = ((string)(dr["POC_REF_HEADER"]));
                }
                if ((dr["SUBJECT_HEADER"] != System.DBNull.Value))
                {
                    this.SubjectHeader = ((string)(dr["SUBJECT_HEADER"]));
                }
                if ((dr["DEL_DATE_HEADER"] != System.DBNull.Value))
                {
                    this.DelDateHeader = ((string)(dr["DEL_DATE_HEADER"]));
                }
                if ((dr["DOC_DATE_HEADER"] != System.DBNull.Value))
                {
                    this.DocDateHeader = ((string)(dr["DOC_DATE_HEADER"]));
                }
                if ((dr["QUOTE_EXP_HEADER"] != System.DBNull.Value))
                {
                    this.QuoteExpHeader = ((string)(dr["QUOTE_EXP_HEADER"]));
                }
                if ((dr["ETA_HEADER"] != System.DBNull.Value))
                {
                    this.EtaHeader = ((string)(dr["ETA_HEADER"]));
                }
                if ((dr["ETD_HEADER"] != System.DBNull.Value))
                {
                    this.EtdHeader = ((string)(dr["ETD_HEADER"]));
                }
                if ((dr["BYR_ADDR_HEADER"] != System.DBNull.Value))
                {
                    this.BuyAddrHeader = ((string)(dr["BYR_ADDR_HEADER"]));
                }
                if ((dr["SUPP_ADDR_HEADER"] != System.DBNull.Value))
                {
                    this.SuppAddrHeader = ((string)(dr["SUPP_ADDR_HEADER"]));
                }
                if ((dr["SHIP_ADDR_HEADER"] != System.DBNull.Value))
                {
                    this.ShipAddrHeader = ((string)(dr["SHIP_ADDR_HEADER"]));
                }
                if ((dr["BILL_ADDR_HEADER"] != System.DBNull.Value))
                {
                    this.BillAddrHeader = ((string)(dr["BILL_ADDR_HEADER"]));
                }

                if ((dr["ITEM_COUNT_HEADER"] != System.DBNull.Value))
                {
                    this.ItemHeaderCount = ((string)(dr["ITEM_COUNT_HEADER"]));
                }

                if ((dr["ITEM_FORMAT"] != System.DBNull.Value))
                {
                    this.ItemFormat = ((string)(dr["ITEM_FORMAT"]));
                }
            }
            catch (Exception ex)
            {
                try
                {
                    string _path = "";
                    if (string.IsNullOrWhiteSpace(_path)) _path = HttpContext.Current.Server.MapPath("~/Log");
                    if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
                    string _logFile = _path + "\\LesMonitor_" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
                    using (StreamWriter sw = new StreamWriter(_logFile, true))
                    {
                        string log = DateTime.Now.ToString("dd-MM-yy HH:mm:ss") + " : " + ex.Message;
                        log += DateTime.Now.ToString("dd-MM-yy HH:mm:ss") + " : " + ex.StackTrace;
                        sw.WriteLine(log);
                        sw.Flush();
                        sw.Dispose();
                    }
                }
                catch
                {

                }
                throw ex;
            }
        }

        public void SetLog(string log)
        {
            try
            {
                string _path = "";
                if (string.IsNullOrWhiteSpace(_path)) _path = HttpContext.Current.Server.MapPath("~/Log");
                if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
                string _logFile = _path + "\\LesMonitor_" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
                using (StreamWriter sw = new StreamWriter(_logFile, true))
                {
                    log = DateTime.Now.ToString("dd-MM-yy HH:mm:ss") + " : " + log;
                    sw.WriteLine(log);
                    sw.Flush();
                    sw.Dispose();
                }
            }
            catch
            {

            }
        }

        public static SmPdfMappingCollection GetAll()
        {
            MetroLesMonitor.Dal.SmPdfMapping dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfMapping();
                System.Data.DataSet ds = dbo.SM_PDF_MAPPING_Select_All();
                SmPdfMappingCollection collection = new SmPdfMappingCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmPdfMapping obj = new SmPdfMapping();
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

        public static SmPdfMapping Load(System.Nullable<int> PDF_MAPID)
        {
            MetroLesMonitor.Dal.SmPdfMapping dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfMapping();
                System.Data.DataSet ds = dbo.SM_PDF_MAPPING_Select_One(PDF_MAPID);
                SmPdfMapping obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmPdfMapping();
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

        public static SmPdfMapping LoadByGroup(System.Nullable<int> GROUPID)
        {
            MetroLesMonitor.Dal.SmPdfMapping dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfMapping();
                System.Data.DataSet ds = dbo.SM_PDF_MAPPING_Select_One_By_GROUPID(GROUPID);
                SmPdfMapping obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmPdfMapping();
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
            MetroLesMonitor.Dal.SmPdfMapping dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfMapping();
                System.Data.DataSet ds = dbo.SM_PDF_MAPPING_Select_One(this.PdfMapid);
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
            MetroLesMonitor.Dal.SmPdfMapping dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfMapping();               
                dbo.SM_PDF_MAPPING_Insert(this.PdfMapid, this.Groupid, this.BuyerName, this.BuyerTel, this.BuyerFax, this.BuyerEmail, this.BuyerContact, this.BuyerAddress, this.SuppName, this.SuppTel, this.SuppFax, this.SuppEmail, this.SuppContact, this.SuppAddress, this.BillName, this.BillContact, this.BillTel, this.BillFax, this.BillEmail, this.BillAddress, this.ConsignName, this.ConsignContact, this.ConsignTel, this.ConsignFax, this.ConsignEmail, this.ConsignAddress, this.BuyerComments, this.VrnoHeader, this.Vrno, this.ImoHeader, this.IMO, this.PoReference, this.QuoteReference, this.CreatedDate, this.LateDate, this.VesselHeader, this.Vessel, this.VesselEta, this.VesselEtd, this.DeliveryPortHeader, this.DeliveryPort, this.Currency, this.DiscountInPrcnt, this.DecimalSeprator, this.IncludeBlanckLines, this.HeaderLineCount, this.FooterLineCount, this.DateFormat1, this.DateFormat2, this.ItemsTotalHeader, this.FrieghtAmtHeader, this.GrantTotalHeader, this.SplitFile, this.ConstantRows, this.SplitStartContent, this.EndCommentStartContent, this.SuppComments, this.QuoteValidity, this.AllowanceAmtHeader, this.PackingAmtHeader, this.AddHeaderToComments, this.AddFooterToComments, this.ExtraFields, this.ExtraFieldsHeader, this.FieldsFromHeader, this.FieldsFromFooter, this.Subject, this.EquipName, this.EquipNameHeader, this.EquipMaker, this.EquipMakerHeader, this.EquipSerno, this.EquipSernoHeader, this.EquipType, this.EquipTypeHeader, this.EquipRemarks, this.EquipRemarksHeader, this.ReadContentBelowItems, this.OverrideItemQty, this.ValidateItemDescr, this.HeaderCommentsStartText, this.HeaderCommentsEndText, this.ItemDescrUptoLineCount, this.Department, this.AddItemToDelDate, this.Remark_Header_RemarkSpace, this.Header_fLine_Content, this.Header_lLine_Content, this.Footer_fLine_Content, this.Footer_lLine_Content, this.Start_Of_SkipText, this.End_Of_SkipText, this.Add_Skipped_Text_To_Remarks, this.CurrencyHeader, this.DeptHeader, this.QuoteRefHeader, this.PocRefHeader, this.SubjectHeader, this.DelDateHeader, this.DocDateHeader, this.QuoteExpHeader, this.EtaHeader, this.EtdHeader, this.BuyAddrHeader, this.SuppAddrHeader, this.ShipAddrHeader, this.BillAddrHeader, this.ItemHeaderCount, this.ItemFormat);
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

        public virtual void Insert(MetroLesMonitor.Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmPdfMapping dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfMapping(_dataAccess);              
                dbo.SM_PDF_MAPPING_Insert(this.PdfMapid, this.Groupid, this.BuyerName, this.BuyerTel, this.BuyerFax, this.BuyerEmail, this.BuyerContact, this.BuyerAddress, this.SuppName, this.SuppTel, this.SuppFax, this.SuppEmail, this.SuppContact, this.SuppAddress, this.BillName, this.BillContact, this.BillTel, this.BillFax, this.BillEmail, this.BillAddress, this.ConsignName, this.ConsignContact, this.ConsignTel, this.ConsignFax, this.ConsignEmail, this.ConsignAddress, this.BuyerComments, this.VrnoHeader, this.Vrno, this.ImoHeader, this.IMO, this.PoReference, this.QuoteReference, this.CreatedDate, this.LateDate, this.VesselHeader, this.Vessel, this.VesselEta, this.VesselEtd, this.DeliveryPortHeader, this.DeliveryPort, this.Currency, this.DiscountInPrcnt, this.DecimalSeprator, this.IncludeBlanckLines, this.HeaderLineCount, this.FooterLineCount, this.DateFormat1, this.DateFormat2, this.ItemsTotalHeader, this.FrieghtAmtHeader, this.GrantTotalHeader, this.SplitFile, this.ConstantRows, this.SplitStartContent, this.EndCommentStartContent, this.SuppComments, this.QuoteValidity, this.AllowanceAmtHeader, this.PackingAmtHeader, this.AddHeaderToComments, this.AddFooterToComments, this.ExtraFields, this.ExtraFieldsHeader, this.FieldsFromHeader, this.FieldsFromFooter, this.Subject, this.EquipName, this.EquipNameHeader, this.EquipMaker, this.EquipMakerHeader, this.EquipSerno, this.EquipSernoHeader, this.EquipType, this.EquipTypeHeader, this.EquipRemarks, this.EquipRemarksHeader, this.ReadContentBelowItems, this.OverrideItemQty, this.ValidateItemDescr, this.HeaderCommentsStartText, this.HeaderCommentsEndText, this.ItemDescrUptoLineCount, this.Department, this.AddItemToDelDate, this.Remark_Header_RemarkSpace, this.Header_fLine_Content, this.Header_lLine_Content, this.Footer_fLine_Content, this.Footer_lLine_Content, this.Start_Of_SkipText, this.End_Of_SkipText, this.Add_Skipped_Text_To_Remarks, this.CurrencyHeader, this.DeptHeader, this.QuoteRefHeader, this.PocRefHeader, this.SubjectHeader, this.DelDateHeader, this.DocDateHeader, this.QuoteExpHeader, this.EtaHeader, this.EtdHeader, this.BuyAddrHeader, this.SuppAddrHeader, this.ShipAddrHeader, this.BillAddrHeader, this.ItemHeaderCount, this.ItemFormat);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public virtual void Delete()
        {
            MetroLesMonitor.Dal.SmPdfMapping dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfMapping();
                dbo.SM_PDF_MAPPING_Delete(this.PdfMapid);
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
            MetroLesMonitor.Dal.SmPdfMapping dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfMapping(_dataAccess);
                dbo.SM_PDF_MAPPING_Delete(this.PdfMapid);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public virtual void Update()
        {
            MetroLesMonitor.Dal.SmPdfMapping dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfMapping();
                //dbo.SM_PDF_MAPPING_Update(this.PdfMapid, this.Groupid, this.BuyerName, this.BuyerTel, this.BuyerFax, this.BuyerEmail, this.BuyerContact, this.BuyerAddress, this.SuppName, this.SuppTel, this.SuppFax, this.SuppEmail, this.SuppContact, this.SuppAddress, this.BillName, this.BillContact, this.BillTel, this.BillFax, this.BillEmail, this.BillAddress, this.ConsignName, this.ConsignContact, this.ConsignTel, this.ConsignFax, this.ConsignEmail, this.ConsignAddress, this.BuyerComments, this.VrnoHeader, this.Vrno, this.ImoHeader, this.IMO, this.PoReference, this.QuoteReference, this.CreatedDate, this.LateDate, this.VesselHeader, this.Vessel, this.VesselEta, this.VesselEtd, this.DeliveryPortHeader, this.DeliveryPort, this.Currency, this.DiscountInPrcnt, this.DecimalSeprator, this.IncludeBlanckLines, this.HeaderLineCount, this.FooterLineCount, this.DateFormat1, this.DateFormat2, this.ItemsTotalHeader, this.FrieghtAmtHeader, this.GrantTotalHeader, this.SplitFile, this.ConstantRows, this.SplitStartContent, this.EndCommentStartContent, this.SuppComments, this.QuoteValidity, this.AllowanceAmtHeader, this.PackingAmtHeader, this.AddHeaderToComments, this.AddFooterToComments, this.ExtraFields, this.ExtraFieldsHeader, this.FieldsFromHeader, this.FieldsFromFooter, this.Subject, this.EquipName, this.EquipNameHeader, this.EquipMaker, this.EquipMakerHeader, this.EquipSerno, this.EquipSernoHeader, this.EquipType, this.EquipTypeHeader, this.EquipRemarks, this.EquipRemarksHeader, this.ReadContentBelowItems,this.OverrideItemQty);
                //dbo.SM_PDF_MAPPING_Update(this.PdfMapid, this.Groupid, this.BuyerName, this.BuyerTel, this.BuyerFax, this.BuyerEmail, this.BuyerContact, this.BuyerAddress, this.SuppName, this.SuppTel, this.SuppFax, this.SuppEmail, this.SuppContact, this.SuppAddress, this.BillName, this.BillContact, this.BillTel, this.BillFax, this.BillEmail, this.BillAddress, this.ConsignName, this.ConsignContact, this.ConsignTel, this.ConsignFax, this.ConsignEmail, this.ConsignAddress, this.BuyerComments, this.VrnoHeader, this.Vrno, this.ImoHeader, this.IMO, this.PoReference, this.QuoteReference, this.CreatedDate, this.LateDate, this.VesselHeader, this.Vessel, this.VesselEta, this.VesselEtd, this.DeliveryPortHeader, this.DeliveryPort, this.Currency, this.DiscountInPrcnt, this.DecimalSeprator, this.IncludeBlanckLines, this.HeaderLineCount, this.FooterLineCount, this.DateFormat1, this.DateFormat2, this.ItemsTotalHeader, this.FrieghtAmtHeader, this.GrantTotalHeader, this.SplitFile, this.ConstantRows, this.SplitStartContent, this.EndCommentStartContent, this.SuppComments, this.QuoteValidity, this.AllowanceAmtHeader, this.PackingAmtHeader, this.AddHeaderToComments, this.AddFooterToComments, this.ExtraFields, this.ExtraFieldsHeader, this.FieldsFromHeader, this.FieldsFromFooter, this.Subject, this.EquipName, this.EquipNameHeader, this.EquipMaker, this.EquipMakerHeader, this.EquipSerno, this.EquipSernoHeader, this.EquipType, this.EquipTypeHeader, this.EquipRemarks, this.EquipRemarksHeader, this.ReadContentBelowItems, this.OverrideItemQty, this.ValidateItemDescr);
                //dbo.SM_PDF_MAPPING_Update(this.PdfMapid, this.Groupid, this.BuyerName, this.BuyerTel, this.BuyerFax, this.BuyerEmail, this.BuyerContact, this.BuyerAddress, this.SuppName, this.SuppTel, this.SuppFax, this.SuppEmail, this.SuppContact, this.SuppAddress, this.BillName, this.BillContact, this.BillTel, this.BillFax, this.BillEmail, this.BillAddress, this.ConsignName, this.ConsignContact, this.ConsignTel, this.ConsignFax, this.ConsignEmail, this.ConsignAddress, this.BuyerComments, this.VrnoHeader, this.Vrno, this.ImoHeader, this.IMO, this.PoReference, this.QuoteReference, this.CreatedDate, this.LateDate, this.VesselHeader, this.Vessel, this.VesselEta, this.VesselEtd, this.DeliveryPortHeader, this.DeliveryPort, this.Currency, this.DiscountInPrcnt, this.DecimalSeprator, this.IncludeBlanckLines, this.HeaderLineCount, this.FooterLineCount, this.DateFormat1, this.DateFormat2, this.ItemsTotalHeader, this.FrieghtAmtHeader, this.GrantTotalHeader, this.SplitFile, this.ConstantRows, this.SplitStartContent, this.EndCommentStartContent, this.SuppComments, this.QuoteValidity, this.AllowanceAmtHeader, this.PackingAmtHeader, this.AddHeaderToComments, this.AddFooterToComments, this.ExtraFields, this.ExtraFieldsHeader, this.FieldsFromHeader, this.FieldsFromFooter, this.Subject, this.EquipName, this.EquipNameHeader, this.EquipMaker, this.EquipMakerHeader, this.EquipSerno, this.EquipSernoHeader, this.EquipType, this.EquipTypeHeader, this.EquipRemarks, this.EquipRemarksHeader, this.ReadContentBelowItems, this.OverrideItemQty, this.ValidateItemDescr, this.HeaderCommentsStartText, this.HeaderCommentsEndText, this.ItemDescrUptoLineCount, this.Department, this.AddItemToDelDate, this.Remark_Header_RemarkSpace, this.Header_fLine_Content, this.Header_lLine_Content, this.Footer_fLine_Content, this.Footer_lLine_Content, this.Start_Of_SkipText, this.End_Of_SkipText, this.Add_Skipped_Text_To_Remarks); // ADDED ON 14-APRIL-2016 -- Sayak
                //dbo.SM_PDF_MAPPING_Update(this.PdfMapid, this.Groupid, this.BuyerName, this.BuyerTel, this.BuyerFax, this.BuyerEmail, this.BuyerContact, this.BuyerAddress, this.SuppName, this.SuppTel, this.SuppFax, this.SuppEmail, this.SuppContact, this.SuppAddress, this.BillName, this.BillContact, this.BillTel, this.BillFax, this.BillEmail, this.BillAddress, this.ConsignName, this.ConsignContact, this.ConsignTel, this.ConsignFax, this.ConsignEmail, this.ConsignAddress, this.BuyerComments, this.VrnoHeader, this.Vrno, this.ImoHeader, this.IMO, this.PoReference, this.QuoteReference, this.CreatedDate, this.LateDate, this.VesselHeader, this.Vessel, this.VesselEta, this.VesselEtd, this.DeliveryPortHeader, this.DeliveryPort, this.Currency, this.DiscountInPrcnt, this.DecimalSeprator, this.IncludeBlanckLines, this.HeaderLineCount, this.FooterLineCount, this.DateFormat1, this.DateFormat2, this.ItemsTotalHeader, this.FrieghtAmtHeader, this.GrantTotalHeader, this.SplitFile, this.ConstantRows, this.SplitStartContent, this.EndCommentStartContent, this.SuppComments, this.QuoteValidity, this.AllowanceAmtHeader, this.PackingAmtHeader, this.AddHeaderToComments, this.AddFooterToComments, this.ExtraFields, this.ExtraFieldsHeader, this.FieldsFromHeader, this.FieldsFromFooter, this.Subject, this.EquipName, this.EquipNameHeader, this.EquipMaker, this.EquipMakerHeader, this.EquipSerno, this.EquipSernoHeader, this.EquipType, this.EquipTypeHeader, this.EquipRemarks, this.EquipRemarksHeader, this.ReadContentBelowItems, this.OverrideItemQty, this.ValidateItemDescr, this.HeaderCommentsStartText, this.HeaderCommentsEndText, this.ItemDescrUptoLineCount, this.Department, this.AddItemToDelDate, this.Remark_Header_RemarkSpace, this.Header_fLine_Content, this.Header_lLine_Content, this.Footer_fLine_Content, this.Footer_lLine_Content, this.Start_Of_SkipText, this.End_Of_SkipText, this.Add_Skipped_Text_To_Remarks); // ADDED ON 14-APRIL-2016 -- Sayak
                //dbo.SM_PDF_MAPPING_Update(this.PdfMapid, this.Groupid, this.BuyerName, this.BuyerTel, this.BuyerFax, this.BuyerEmail, this.BuyerContact, this.BuyerAddress, this.SuppName, this.SuppTel, this.SuppFax, this.SuppEmail, this.SuppContact, this.SuppAddress, this.BillName, this.BillContact, this.BillTel, this.BillFax, this.BillEmail, this.BillAddress, this.ConsignName, this.ConsignContact, this.ConsignTel, this.ConsignFax, this.ConsignEmail, this.ConsignAddress, this.BuyerComments, this.VrnoHeader, this.Vrno, this.ImoHeader, this.IMO, this.PoReference, this.QuoteReference, this.CreatedDate, this.LateDate, this.VesselHeader, this.Vessel, this.VesselEta, this.VesselEtd, this.DeliveryPortHeader, this.DeliveryPort, this.Currency, this.DiscountInPrcnt, this.DecimalSeprator, this.IncludeBlanckLines, this.HeaderLineCount, this.FooterLineCount, this.DateFormat1, this.DateFormat2, this.ItemsTotalHeader, this.FrieghtAmtHeader, this.GrantTotalHeader, this.SplitFile, this.ConstantRows, this.SplitStartContent, this.EndCommentStartContent, this.SuppComments, this.QuoteValidity, this.AllowanceAmtHeader, this.PackingAmtHeader, this.AddHeaderToComments, this.AddFooterToComments, this.ExtraFields, this.ExtraFieldsHeader, this.FieldsFromHeader, this.FieldsFromFooter, this.Subject, this.EquipName, this.EquipNameHeader, this.EquipMaker, this.EquipMakerHeader, this.EquipSerno, this.EquipSernoHeader, this.EquipType, this.EquipTypeHeader, this.EquipRemarks, this.EquipRemarksHeader, this.ReadContentBelowItems, this.OverrideItemQty, this.ValidateItemDescr, this.HeaderCommentsStartText, this.HeaderCommentsEndText, this.ItemDescrUptoLineCount, this.Department, this.AddItemToDelDate, this.Remark_Header_RemarkSpace, this.Header_fLine_Content, this.Header_lLine_Content, this.Footer_fLine_Content, this.Footer_lLine_Content, this.Start_Of_SkipText, this.End_Of_SkipText, this.Add_Skipped_Text_To_Remarks, this.CurrencyHeader, this.DeptHeader, this.QuoteRefHeader, this.PocRefHeader, this.SubjectHeader, this.DelDateHeader, this.DocDateHeader, this.QuoteExpHeader, this.EtaHeader, this.EtdHeader, this.BuyAddrHeader, this.SuppAddrHeader, this.ShipAddrHeader, this.BillAddrHeader); // ADDED ON 08-01-2018 -- Sayak
                //dbo.SM_PDF_MAPPING_Update(this.PdfMapid, this.Groupid, this.BuyerName, this.BuyerTel, this.BuyerFax, this.BuyerEmail, this.BuyerContact, this.BuyerAddress, this.SuppName, this.SuppTel, this.SuppFax, this.SuppEmail, this.SuppContact, this.SuppAddress, this.BillName, this.BillContact, this.BillTel, this.BillFax, this.BillEmail, this.BillAddress, this.ConsignName, this.ConsignContact, this.ConsignTel, this.ConsignFax, this.ConsignEmail, this.ConsignAddress, this.BuyerComments, this.VrnoHeader, this.Vrno, this.ImoHeader, this.IMO, this.PoReference, this.QuoteReference, this.CreatedDate, this.LateDate, this.VesselHeader, this.Vessel, this.VesselEta, this.VesselEtd, this.DeliveryPortHeader, this.DeliveryPort, this.Currency, this.DiscountInPrcnt, this.DecimalSeprator, this.IncludeBlanckLines, this.HeaderLineCount, this.FooterLineCount, this.DateFormat1, this.DateFormat2, this.ItemsTotalHeader, this.FrieghtAmtHeader, this.GrantTotalHeader, this.SplitFile, this.ConstantRows, this.SplitStartContent, this.EndCommentStartContent, this.SuppComments, this.QuoteValidity, this.AllowanceAmtHeader, this.PackingAmtHeader, this.AddHeaderToComments, this.AddFooterToComments, this.ExtraFields, this.ExtraFieldsHeader, this.FieldsFromHeader, this.FieldsFromFooter, this.Subject, this.EquipName, this.EquipNameHeader, this.EquipMaker, this.EquipMakerHeader, this.EquipSerno, this.EquipSernoHeader, this.EquipType, this.EquipTypeHeader, this.EquipRemarks, this.EquipRemarksHeader, this.ReadContentBelowItems, this.OverrideItemQty, this.ValidateItemDescr, this.HeaderCommentsStartText, this.HeaderCommentsEndText, this.ItemDescrUptoLineCount, this.Department, this.AddItemToDelDate, this.Remark_Header_RemarkSpace, this.Header_fLine_Content, this.Header_lLine_Content, this.Footer_fLine_Content, this.Footer_lLine_Content, this.Start_Of_SkipText, this.End_Of_SkipText, this.Add_Skipped_Text_To_Remarks, this.CurrencyHeader, this.DeptHeader, this.QuoteRefHeader, this.PocRefHeader, this.SubjectHeader, this.DelDateHeader, this.DocDateHeader, this.QuoteExpHeader, this.EtaHeader, this.EtdHeader, this.BuyAddrHeader, this.SuppAddrHeader, this.ShipAddrHeader, this.BillAddrHeader, this.ItemHeaderCount, this.ItemFormat); // ADDED ON 04-05-2018 -- Sayak
                dbo.SM_PDF_MAPPING_Update(this.PdfMapid, this.Groupid, this.BuyerName, this.BuyerTel, this.BuyerFax, this.BuyerEmail, this.BuyerContact, this.BuyerAddress, this.SuppName, this.SuppTel, this.SuppFax, this.SuppEmail, this.SuppContact, this.SuppAddress, this.BillName, this.BillContact, this.BillTel, this.BillFax, this.BillEmail, this.BillAddress, this.ConsignName, this.ConsignContact, this.ConsignTel, this.ConsignFax, this.ConsignEmail, this.ConsignAddress, this.BuyerComments, this.VrnoHeader, this.Vrno, this.ImoHeader, this.IMO, this.PoReference, this.QuoteReference, this.CreatedDate, this.LateDate, this.VesselHeader, this.Vessel, this.VesselEta, this.VesselEtd, this.DeliveryPortHeader, this.DeliveryPort, this.Currency, this.DiscountInPrcnt, this.DecimalSeprator, this.IncludeBlanckLines, this.HeaderLineCount, this.FooterLineCount, this.DateFormat1, this.DateFormat2, this.ItemsTotalHeader, this.FrieghtAmtHeader, this.GrantTotalHeader, this.SplitFile, this.ConstantRows, this.SplitStartContent, this.EndCommentStartContent, this.SuppComments, this.QuoteValidity, this.AllowanceAmtHeader, this.PackingAmtHeader, this.AddHeaderToComments, this.AddFooterToComments, this.ExtraFields, this.ExtraFieldsHeader, this.FieldsFromHeader, this.FieldsFromFooter, this.Subject, this.EquipName, this.EquipNameHeader, this.EquipMaker, this.EquipMakerHeader, this.EquipSerno, this.EquipSernoHeader, this.EquipType, this.EquipTypeHeader, this.EquipRemarks, this.EquipRemarksHeader, this.ReadContentBelowItems, this.OverrideItemQty, this.ValidateItemDescr, this.HeaderCommentsStartText, this.HeaderCommentsEndText, this.ItemDescrUptoLineCount, this.Department, this.AddItemToDelDate, this.Remark_Header_RemarkSpace, this.Header_fLine_Content, this.Header_lLine_Content, this.Footer_fLine_Content, this.Footer_lLine_Content, this.Start_Of_SkipText, this.End_Of_SkipText, this.Add_Skipped_Text_To_Remarks, this.CurrencyHeader, this.DeptHeader, this.QuoteRefHeader, this.PocRefHeader, this.SubjectHeader, this.DelDateHeader, this.DocDateHeader, this.QuoteExpHeader, this.EtaHeader, this.EtdHeader, this.BuyAddrHeader, this.SuppAddrHeader, this.ShipAddrHeader, this.BillAddrHeader, this.ItemHeaderCount, this.ItemFormat); // ADDED ON 04-05-2018 -- Sayak

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

        public virtual void Update(MetroLesMonitor.Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmPdfMapping dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfMapping(_dataAccess);               
                dbo.SM_PDF_MAPPING_Update(this.PdfMapid, this.Groupid, this.BuyerName, this.BuyerTel, this.BuyerFax, this.BuyerEmail, this.BuyerContact, this.BuyerAddress, 
                    this.SuppName, this.SuppTel,
                    this.SuppFax, this.SuppEmail, this.SuppContact, this.SuppAddress, this.BillName, this.BillContact, this.BillTel, this.BillFax, 
                    this.BillEmail, this.BillAddress, this.ConsignName, this.ConsignContact, this.ConsignTel, this.ConsignFax, this.ConsignEmail,
                    this.ConsignAddress, this.BuyerComments, this.VrnoHeader, this.Vrno, this.ImoHeader, this.IMO, this.PoReference, this.QuoteReference, 
                    this.CreatedDate, this.LateDate, this.VesselHeader, this.Vessel, this.VesselEta, this.VesselEtd, this.DeliveryPortHeader, this.DeliveryPort,
                    this.Currency, this.DiscountInPrcnt, this.DecimalSeprator, this.IncludeBlanckLines, this.HeaderLineCount, this.FooterLineCount, this.DateFormat1, 
                    this.DateFormat2, this.ItemsTotalHeader, this.FrieghtAmtHeader, this.GrantTotalHeader, this.SplitFile, this.ConstantRows, this.SplitStartContent, 
                    this.EndCommentStartContent, this.SuppComments, this.QuoteValidity, this.AllowanceAmtHeader, this.PackingAmtHeader, this.AddHeaderToComments, 
                    this.AddFooterToComments, this.ExtraFields, this.ExtraFieldsHeader, this.FieldsFromHeader, this.FieldsFromFooter, this.Subject, this.EquipName, 
                    this.EquipNameHeader, this.EquipMaker, this.EquipMakerHeader, this.EquipSerno, this.EquipSernoHeader, this.EquipType, this.EquipTypeHeader, 
                    this.EquipRemarks, this.EquipRemarksHeader, this.ReadContentBelowItems, this.OverrideItemQty, this.ValidateItemDescr, this.HeaderCommentsStartText, this.HeaderCommentsEndText, this.ItemDescrUptoLineCount, this.Department, this.AddItemToDelDate, this.Remark_Header_RemarkSpace, this.Header_fLine_Content, this.Header_lLine_Content, this.Footer_fLine_Content, this.Footer_lLine_Content, this.Start_Of_SkipText, this.End_Of_SkipText, this.Add_Skipped_Text_To_Remarks, this.CurrencyHeader, this.DeptHeader, this.QuoteRefHeader, this.PocRefHeader, this.SubjectHeader, this.DelDateHeader, this.DocDateHeader, this.QuoteExpHeader, this.EtaHeader, this.EtdHeader, this.BuyAddrHeader, this.SuppAddrHeader, this.ShipAddrHeader, this.BillAddrHeader, this.ItemHeaderCount, this.ItemFormat); // ADDED ON 04-05-2018 -- Sayak

            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}