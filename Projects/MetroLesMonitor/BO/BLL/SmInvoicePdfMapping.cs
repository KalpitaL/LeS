using MetroLesMonitor.Bll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MetroLesMonitor.Bll
{
    public partial class SmInvoicePdfMapping
    {
        private System.Nullable<int> _invPdfMapid;

        private string _invMapCode;

        private string _vouchertype;

        private string _voucherNo;

        private System.Nullable<int> _voucherNoOffset;

        private string _voucherNoHeader;

        private string _poNo;

        private System.Nullable<int> _poNoOffset;

        private string _poNoHeader;

        private string _poRefNo;

        private System.Nullable<int> _poRefNoOffset;

        private string _poRefNoHeader;

        private string _supplierDispatchNo;

        private System.Nullable<int> _supplierDispatchNoOffset;

        private string _supplierDispatchNoHeader;

        private string _forwarderDispatchNo;

        private System.Nullable<int> _forwarderDispatchNoOffset;

        private string _forwarderDispatchNoHeader;

        private string _supplierCustomerNo;

        private System.Nullable<int> _supplierCustomerNoOffset;

        private string _supplierCustomerNoHeader;

        //private string _buyerRef;

        //private System.Nullable<int> _buyerRefOffset;

        //private string _buyerRefHeader;

        //private string _supplierRef;

        //private System.Nullable<int> _supplierRefOffset;

        //private string _supplierRefHeader;

        private string _currency;

        private System.Nullable<int> _currencyOffset;

        private string _currencyHeader;

        private string _vesselImnNo;

        private System.Nullable<int> _vesselImnNoOffset;

        private string _vesselImnNoHeader;

        private string _vesselName;

        private System.Nullable<int> _vesselNameOffset;

        private string _vesselNameHeader;

        private string _portCode;

        private System.Nullable<int> _portCodeOffset;

        private string _portCodeHeader;

        private string _portName;

        private System.Nullable<int> _portNameOffset;

        private string _portNameHeader;

        private string _paymentTerms;

        private System.Nullable<int> _paymentTermsOffset;

        private string _paymentTermsHeader;

        private string _deliveryTerms;

        private System.Nullable<int> _deliveryTermsOffset;

        private string _deliveryTermsHeader;

        private string _supplierRemarks;

        private System.Nullable<int> _supplierRemarksOffset;

        private string _supplierRemarksHeader;

        private string _itemsCount;

        private System.Nullable<int> _itemsCountOffset;

        private string _itemsCountHeader;

        private string _courierName;

        private System.Nullable<int> _courierNameOffset;

        private string _courierNameHeader;

        private string _buyerVatNo;

        private System.Nullable<int> _buyerVatNoOffset;

        private string _buyerVatNoHeader;

        private string _supplierVatNo;

        private System.Nullable<int> _supplierVatNoOffset;

        private string _supplierVatNoHeader;

        private string _voucherDate;

        private System.Nullable<int> _voucherDateOffset;

        private string _voucherDateHeader;

        private string _voucherDueDate;

        private System.Nullable<int> _voucherDueDateOffset;

        private string _voucherDueDateHeader;

        private string _buyerPoDate;

        private System.Nullable<int> _buyerPoDateOffset;

        private string _buyerPoDateHeader;

        private string _dispatchDate;

        private System.Nullable<int> _dispatchDateOffset;

        private string _dispatchDateHeader;

        private string _lateDate;

        private System.Nullable<int> _lateDateOffset;

        private string _lateDateHeader;

        private string _bankName;

        private System.Nullable<int> _bankNameOffset;

        private string _bankNameHeader;

        private string _accountName;

        private System.Nullable<int> _accountNameOffset;

        private string _accountNameHeader;

        private string _coc;

        private System.Nullable<int> _cocOffset;

        private string _cocHeader;

        private string _sortCode;

        private System.Nullable<int> _sortCodeOffset;

        private string _sortCodeHeader;

        private string _ibanNo;

        private System.Nullable<int> _ibanNoOffset;

        private string _ibanNoHeader;

        private string _swiftNo;

        private System.Nullable<int> _swiftNoOffset;

        private string _swiftNoHeader;

        private string _accountNo;

        private System.Nullable<int> _accountNoOffset;

        private string _accountNoHeader;

        private string _kvkNo;

        private System.Nullable<int> _kvkNoOffset;

        private string _kvkNoHeader;

        //private string _bicNo;

        //private System.Nullable<int> _bicNoOffset;

        //private string _bicNoHeader;

        private string _itemsTotalAmount;

        private System.Nullable<int> _itemsTotalAmountOffset;

        private string _itemsTotalAmountHeader;

        private string _allowanceAmount;

        private System.Nullable<int> _allowanceAmountOffset;

        private string _allowanceAmountHeader;

        private string _frightAmount;

        private System.Nullable<int> _frightAmountOffset;

        private string _frightAmountHeader;

        private string _packingCost;

        private System.Nullable<int> _packingCostOffset;

        private string _packingCostHeader;

        private string _vatAmount;

        private System.Nullable<int> _vatAmountOffset;

        private string _vatAmountHeader;

        private string _fcaAmount;

        private System.Nullable<int> _fcaAmountOffset;

        private string _fcaAmountHeader;

        private string _courierCharges;

        private System.Nullable<int> _courierChargesOffset;

        private string _courierChargesHeader;

        private string _insuranceAmount;

        private System.Nullable<int> _insuranceAmountOffset;

        private string _insuranceAmountHeader;

        private string _transactionCharges;

        private System.Nullable<int> _transactionChargesOffset;

        private string _transactionChargesHeader;

        private string _finalTotalAmount;

        private System.Nullable<int> _finalTotalAmountOffset;

        private string _finalTotalAmountHeader;

        private string _buyerName;

        private string _buyerAddress;

        private string _buyerContactPerson;

        private string _buyerTelephone;

        private string _buyerFax;

        private string _buyerEmail;

        private string _buyerWebsite;

        private string _supplierName;

        private string _supplierAddress;

        private string _supplierContactPerson;

        private string _supplierTelephone;

        private string _supplierFax;

        private string _supplierEmail;

        private string _supplierWebsite;

        private string _billingName;

        private string _billingAddress;

        private string _billingContactPerson;

        private string _billingTelephone;

        private string _billingFax;

        private string _billingEmail;

        private string _billingWebsite;

        private string _consigneeName;

        private string _consigneeAddress;

        private string _consigneeContactPerson;

        private string _consigneeTelephone;

        private string _consigneeFax;

        private string _consigneeEmail;

        private string _consigneeWebsite;

        private string _ffName;

        private string _ffAddress;

        private string _ffContactPerson;

        private string _ffTelephone;

        private string _ffFax;

        private string _ffEmail;

        private string _ffWebsite;

        private System.Nullable<int> _headerLinesCount;

        private System.Nullable<int> _footerLinesCount;

        private string _dateFormat1;

        private string _dateFormat2;

        private string _vouStartText;

        private string _vouEndText;

        private string _decimalSeperator;

        private System.Nullable<int> _skipBlankLines;

        private System.Nullable<System.DateTime> _createdDate;

        private System.Nullable<System.DateTime> _updateDate;

        private string _buyerOrgNo;

        private System.Nullable<int> _buyerOrgNoOffset;

        private string _buyerOrgNoHeader;


        private string _supplierOrgNo;

        private System.Nullable<int> _supplierOrgNoOffset;

        private string _supplierOrgNoHeader;

        private System.Nullable<int> _addHeaderToComment;

        private System.Nullable<int> _addFooterToComment;

        private string _fieldsFromHeader;

        private string _fieldsFromFooter;

        private string _skipText;

        private string _remarkStartText;

        private string _remarkEndText;

        private string _creditToInvoiceNo;

        private System.Nullable<int> _creditToInvoiceNoOffset;

        private string _creditToInvoiceNoHeader;

        private System.Nullable<int> _splitFile;

        private string _constantRowsStartText;

        private string _constantRowsEndText;

        private string _splitStartText;

        private string _splitEndText;

        private string _currencyMapping;

        private System.Nullable<int> _useItemMapping;
        private string _startOfSkipText;
        private string _endOfSkipText;

        private string _headerStartContent;
        private string _headerEndContent;

        private string _footerStartContent;
        private string _footerEndContent;

        private string _buyer_Addr_Header;
        private string _supplier_Addr_Header;
        private string _consignee_Addr_Header;
        private string _billing_Addr_Header;
        private string _ff_Addr_Header;

        private string _offsetHeaders_Fields;

        private string _mandatory_Fields;
        private System.Nullable<int> _ocr_Web_Service;

        private string _item_Format;
        private string _regex_Fields;

        private string _ckVoucherType;

        private string _replace_Text_Field;

        public string Replace_Text_Field
        {
            get
            {
                return _replace_Text_Field;
            }
            set
            {
                _replace_Text_Field = value;
            }
        }

        public string Check_Voucher_Type
        {
            get
            {
                return _ckVoucherType;
            }
            set
            {
                _ckVoucherType = value;
            }
        }

        public string OffsetHeaders_Fields
        {
            get
            {
                return _offsetHeaders_Fields;
            }
            set
            {
                _offsetHeaders_Fields = value;
            }
        }

        public string Regex_Fields
        {
            get
            {
                return _regex_Fields;
            }
            set
            {
                _regex_Fields = value;
            }
        }

        public string Item_Format
        {
            get
            {
                return _item_Format;
            }
            set
            {
                _item_Format = value;
            }
        }

        public string FF_Addr_Header
        {
            get
            {
                return _ff_Addr_Header;
            }
            set
            {
                _ff_Addr_Header = value;
            }
        }

        public string Billing_Addr_Header
        {
            get
            {
                return _billing_Addr_Header;
            }
            set
            {
                _billing_Addr_Header = value;
            }
        }

        public string Buyer_Addr_Header
        {
            get
            {
                return _buyer_Addr_Header;
            }
            set
            {
                _buyer_Addr_Header = value;
            }
        }

        public string Consignee_Addr_Header
        {
            get
            {
                return _consignee_Addr_Header;
            }
            set
            {
                _consignee_Addr_Header = value;
            }
        }

        public string Supplier_Addr_Header
        {
            get
            {
                return _supplier_Addr_Header;
            }
            set
            {
                _supplier_Addr_Header = value;
            }
        }

        public string HeaderStartContent
        {
            get
            {
                return _headerStartContent;
            }
            set
            {
                _headerStartContent = value;
            }
        }

        public string HeaderEndContent
        {
            get
            {
                return _headerEndContent;
            }
            set
            {
                _headerEndContent = value;
            }
        }

        public string FooterStartContent
        {
            get
            {
                return _footerStartContent;
            }
            set
            {
                _footerStartContent = value;
            }
        }

        public string FooterEndContent
        {
            get
            {
                return _footerEndContent;
            }
            set
            {
                _footerEndContent = value;
            }
        }

        //private SmInvPdfBuyerSupplierLinkCollection _smInvPdfBuyerSupplierLinkCollection;

        public virtual string CreditToInvoiceNo
        {
            get
            {
                return _creditToInvoiceNo;
            }
            set
            {
                _creditToInvoiceNo = value;
            }
        }

        public virtual System.Nullable<int> CreditToInvoiceNoOffset
        {
            get
            {
                return _creditToInvoiceNoOffset;
            }
            set
            {
                _creditToInvoiceNoOffset = value;
            }
        }

        public virtual string CreditToInvoiceNoHeader
        {
            get
            {
                return _creditToInvoiceNoHeader;
            }
            set
            {
                _creditToInvoiceNoHeader = value;
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

        public virtual string CurrencyMapping
        {
            get
            {
                return _currencyMapping;
            }
            set
            {
                _currencyMapping = value;
            }
        }

        public virtual string ConstantRowsStartText
        {
            get
            {
                return _constantRowsStartText;
            }
            set
            {
                _constantRowsStartText = value;
            }
        }

        public virtual string ConstantRowsEndText
        {
            get
            {
                return _constantRowsEndText;
            }
            set
            {
                _constantRowsEndText = value;
            }
        }

        public virtual string SplitStartText
        {
            get
            {
                return _splitStartText;
            }
            set
            {
                _splitStartText = value;
            }
        }

        public virtual string SplitEndText
        {
            get
            {
                return _splitEndText;
            }
            set
            {
                _splitEndText = value;
            }
        }

        public virtual System.Nullable<int> InvPdfMapid
        {
            get
            {
                return _invPdfMapid;
            }
            set
            {
                _invPdfMapid = value;
            }
        }

        public virtual string InvMapCode
        {
            get
            {
                return _invMapCode;
            }
            set
            {
                _invMapCode = value;
            }
        }

        public virtual string Vouchertype
        {
            get
            {
                return _vouchertype;
            }
            set
            {
                _vouchertype = value;
            }
        }

        public virtual string VoucherNo
        {
            get
            {
                return _voucherNo;
            }
            set
            {
                _voucherNo = value;
            }
        }

        public virtual System.Nullable<int> VoucherNoOffset
        {
            get
            {
                return _voucherNoOffset;
            }
            set
            {
                _voucherNoOffset = value;
            }
        }

        public virtual string VoucherNoHeader
        {
            get
            {
                return _voucherNoHeader;
            }
            set
            {
                _voucherNoHeader = value;
            }
        }

        public virtual string PoNo
        {
            get
            {
                return _poNo;
            }
            set
            {
                _poNo = value;
            }
        }

        public virtual System.Nullable<int> PoNoOffset
        {
            get
            {
                return _poNoOffset;
            }
            set
            {
                _poNoOffset = value;
            }
        }

        public virtual string PoNoHeader
        {
            get
            {
                return _poNoHeader;
            }
            set
            {
                _poNoHeader = value;
            }
        }

        public virtual string PoRefNo
        {
            get
            {
                return _poRefNo;
            }
            set
            {
                _poRefNo = value;
            }
        }

        public virtual System.Nullable<int> PoRefNoOffset
        {
            get
            {
                return _poRefNoOffset;
            }
            set
            {
                _poRefNoOffset = value;
            }
        }

        public virtual string PoRefNoHeader
        {
            get
            {
                return _poRefNoHeader;
            }
            set
            {
                _poRefNoHeader = value;
            }
        }

        public virtual string SupplierDispatchNo
        {
            get
            {
                return _supplierDispatchNo;
            }
            set
            {
                _supplierDispatchNo = value;
            }
        }

        public virtual System.Nullable<int> SupplierDispatchNoOffset
        {
            get
            {
                return _supplierDispatchNoOffset;
            }
            set
            {
                _supplierDispatchNoOffset = value;
            }
        }

        public virtual string SupplierDispatchNoHeader
        {
            get
            {
                return _supplierDispatchNoHeader;
            }
            set
            {
                _supplierDispatchNoHeader = value;
            }
        }

        public virtual string ForwarderDispatchNo
        {
            get
            {
                return _forwarderDispatchNo;
            }
            set
            {
                _forwarderDispatchNo = value;
            }
        }

        public virtual System.Nullable<int> ForwarderDispatchNoOffset
        {
            get
            {
                return _forwarderDispatchNoOffset;
            }
            set
            {
                _forwarderDispatchNoOffset = value;
            }
        }

        public virtual string ForwarderDispatchNoHeader
        {
            get
            {
                return _forwarderDispatchNoHeader;
            }
            set
            {
                _forwarderDispatchNoHeader = value;
            }
        }

        public virtual string SupplierCustomerNo
        {
            get
            {
                return _supplierCustomerNo;
            }
            set
            {
                _supplierCustomerNo = value;
            }
        }

        public virtual System.Nullable<int> SupplierCustomerNoOffset
        {
            get
            {
                return _supplierCustomerNoOffset;
            }
            set
            {
                _supplierCustomerNoOffset = value;
            }
        }

        public virtual string SupplierCustomerNoHeader
        {
            get
            {
                return _supplierCustomerNoHeader;
            }
            set
            {
                _supplierCustomerNoHeader = value;
            }
        }

        //public virtual string BuyerRef
        //{
        //    get
        //    {
        //        return _buyerRef;
        //    }
        //    set
        //    {
        //        _buyerRef = value;
        //    }
        //}

        //public virtual System.Nullable<int> BuyerRefOffset
        //{
        //    get
        //    {
        //        return _buyerRefOffset;
        //    }
        //    set
        //    {
        //        _buyerRefOffset = value;
        //    }
        //}

        //public virtual string BuyerRefHeader
        //{
        //    get
        //    {
        //        return _buyerRefHeader;
        //    }
        //    set
        //    {
        //        _buyerRefHeader = value;
        //    }
        //}

        //public virtual string SupplierRef
        //{
        //    get
        //    {
        //        return _supplierRef;
        //    }
        //    set
        //    {
        //        _supplierRef = value;
        //    }
        //}

        //public virtual System.Nullable<int> SupplierRefOffset
        //{
        //    get
        //    {
        //        return _supplierRefOffset;
        //    }
        //    set
        //    {
        //        _supplierRefOffset = value;
        //    }
        //}

        //public virtual string SupplierRefHeader
        //{
        //    get
        //    {
        //        return _supplierRefHeader;
        //    }
        //    set
        //    {
        //        _supplierRefHeader = value;
        //    }
        //}

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

        public virtual System.Nullable<int> CurrencyOffset
        {
            get
            {
                return _currencyOffset;
            }
            set
            {
                _currencyOffset = value;
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

        public virtual string VesselImoNo
        {
            get
            {
                return _vesselImnNo;
            }
            set
            {
                _vesselImnNo = value;
            }
        }

        public virtual System.Nullable<int> VesselImoNoOffset
        {
            get
            {
                return _vesselImnNoOffset;
            }
            set
            {
                _vesselImnNoOffset = value;
            }
        }

        public virtual string VesselImoNoHeader
        {
            get
            {
                return _vesselImnNoHeader;
            }
            set
            {
                _vesselImnNoHeader = value;
            }
        }

        public virtual string VesselName
        {
            get
            {
                return _vesselName;
            }
            set
            {
                _vesselName = value;
            }
        }

        public virtual System.Nullable<int> VesselNameOffset
        {
            get
            {
                return _vesselNameOffset;
            }
            set
            {
                _vesselNameOffset = value;
            }
        }

        public virtual string VesselNameHeader
        {
            get
            {
                return _vesselNameHeader;
            }
            set
            {
                _vesselNameHeader = value;
            }
        }

        public virtual string PortCode
        {
            get
            {
                return _portCode;
            }
            set
            {
                _portCode = value;
            }
        }

        public virtual System.Nullable<int> PortCodeOffset
        {
            get
            {
                return _portCodeOffset;
            }
            set
            {
                _portCodeOffset = value;
            }
        }

        public virtual string PortCodeHeader
        {
            get
            {
                return _portCodeHeader;
            }
            set
            {
                _portCodeHeader = value;
            }
        }

        public virtual string PortName
        {
            get
            {
                return _portName;
            }
            set
            {
                _portName = value;
            }
        }

        public virtual System.Nullable<int> PortNameOffset
        {
            get
            {
                return _portNameOffset;
            }
            set
            {
                _portNameOffset = value;
            }
        }

        public virtual string PortNameHeader
        {
            get
            {
                return _portNameHeader;
            }
            set
            {
                _portNameHeader = value;
            }
        }

        public virtual string PaymentTerms
        {
            get
            {
                return _paymentTerms;
            }
            set
            {
                _paymentTerms = value;
            }
        }

        public virtual System.Nullable<int> PaymentTermsOffset
        {
            get
            {
                return _paymentTermsOffset;
            }
            set
            {
                _paymentTermsOffset = value;
            }
        }

        public virtual string PaymentTermsHeader
        {
            get
            {
                return _paymentTermsHeader;
            }
            set
            {
                _paymentTermsHeader = value;
            }
        }

        public virtual string DeliveryTerms
        {
            get
            {
                return _deliveryTerms;
            }
            set
            {
                _deliveryTerms = value;
            }
        }

        public virtual System.Nullable<int> DeliveryTermsOffset
        {
            get
            {
                return _deliveryTermsOffset;
            }
            set
            {
                _deliveryTermsOffset = value;
            }
        }

        public virtual string DeliveryTermsHeader
        {
            get
            {
                return _deliveryTermsHeader;
            }
            set
            {
                _deliveryTermsHeader = value;
            }
        }

        public virtual string SupplierRemarks
        {
            get
            {
                return _supplierRemarks;
            }
            set
            {
                _supplierRemarks = value;
            }
        }

        public virtual System.Nullable<int> SupplierRemarksOffset
        {
            get
            {
                return _supplierRemarksOffset;
            }
            set
            {
                _supplierRemarksOffset = value;
            }
        }

        public virtual string SupplierRemarksHeader
        {
            get
            {
                return _supplierRemarksHeader;
            }
            set
            {
                _supplierRemarksHeader = value;
            }
        }

        public virtual string ItemsCount
        {
            get
            {
                return _itemsCount;
            }
            set
            {
                _itemsCount = value;
            }
        }

        public virtual System.Nullable<int> ItemsCountOffset
        {
            get
            {
                return _itemsCountOffset;
            }
            set
            {
                _itemsCountOffset = value;
            }
        }

        public virtual string ItemsCountHeader
        {
            get
            {
                return _itemsCountHeader;
            }
            set
            {
                _itemsCountHeader = value;
            }
        }

        public virtual string CourierName
        {
            get
            {
                return _courierName;
            }
            set
            {
                _courierName = value;
            }
        }

        public virtual System.Nullable<int> CourierNameOffset
        {
            get
            {
                return _courierNameOffset;
            }
            set
            {
                _courierNameOffset = value;
            }
        }

        public virtual string CourierNameHeader
        {
            get
            {
                return _courierNameHeader;
            }
            set
            {
                _courierNameHeader = value;
            }
        }

        public virtual string BuyerVatNo
        {
            get
            {
                return _buyerVatNo;
            }
            set
            {
                _buyerVatNo = value;
            }
        }

        public virtual System.Nullable<int> BuyerVatNoOffset
        {
            get
            {
                return _buyerVatNoOffset;
            }
            set
            {
                _buyerVatNoOffset = value;
            }
        }

        public virtual string BuyerVatNoHeader
        {
            get
            {
                return _buyerVatNoHeader;
            }
            set
            {
                _buyerVatNoHeader = value;
            }
        }

        public virtual string SupplierVatNo
        {
            get
            {
                return _supplierVatNo;
            }
            set
            {
                _supplierVatNo = value;
            }
        }

        public virtual System.Nullable<int> SupplierVatNoOffset
        {
            get
            {
                return _supplierVatNoOffset;
            }
            set
            {
                _supplierVatNoOffset = value;
            }
        }

        public virtual string SupplierVatNoHeader
        {
            get
            {
                return _supplierVatNoHeader;
            }
            set
            {
                _supplierVatNoHeader = value;
            }
        }

        public virtual string VoucherDate
        {
            get
            {
                return _voucherDate;
            }
            set
            {
                _voucherDate = value;
            }
        }

        public virtual System.Nullable<int> VoucherDateOffset
        {
            get
            {
                return _voucherDateOffset;
            }
            set
            {
                _voucherDateOffset = value;
            }
        }

        public virtual string VoucherDateHeader
        {
            get
            {
                return _voucherDateHeader;
            }
            set
            {
                _voucherDateHeader = value;
            }
        }

        public virtual string VoucherDueDate
        {
            get
            {
                return _voucherDueDate;
            }
            set
            {
                _voucherDueDate = value;
            }
        }

        public virtual System.Nullable<int> VoucherDueDateOffset
        {
            get
            {
                return _voucherDueDateOffset;
            }
            set
            {
                _voucherDueDateOffset = value;
            }
        }

        public virtual string VoucherDueDateHeader
        {
            get
            {
                return _voucherDueDateHeader;
            }
            set
            {
                _voucherDueDateHeader = value;
            }
        }

        public virtual string BuyerPoDate
        {
            get
            {
                return _buyerPoDate;
            }
            set
            {
                _buyerPoDate = value;
            }
        }

        public virtual System.Nullable<int> BuyerPoDateOffset
        {
            get
            {
                return _buyerPoDateOffset;
            }
            set
            {
                _buyerPoDateOffset = value;
            }
        }

        public virtual string BuyerPoDateHeader
        {
            get
            {
                return _buyerPoDateHeader;
            }
            set
            {
                _buyerPoDateHeader = value;
            }
        }

        public virtual string DispatchDate
        {
            get
            {
                return _dispatchDate;
            }
            set
            {
                _dispatchDate = value;
            }
        }

        public virtual System.Nullable<int> DispatchDateOffset
        {
            get
            {
                return _dispatchDateOffset;
            }
            set
            {
                _dispatchDateOffset = value;
            }
        }

        public virtual string DispatchDateHeader
        {
            get
            {
                return _dispatchDateHeader;
            }
            set
            {
                _dispatchDateHeader = value;
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

        public virtual System.Nullable<int> LateDateOffset
        {
            get
            {
                return _lateDateOffset;
            }
            set
            {
                _lateDateOffset = value;
            }
        }

        public virtual string LateDateHeader
        {
            get
            {
                return _lateDateHeader;
            }
            set
            {
                _lateDateHeader = value;
            }
        }

        public virtual string BankName
        {
            get
            {
                return _bankName;
            }
            set
            {
                _bankName = value;
            }
        }

        public virtual System.Nullable<int> BankNameOffset
        {
            get
            {
                return _bankNameOffset;
            }
            set
            {
                _bankNameOffset = value;
            }
        }

        public virtual string BankNameHeader
        {
            get
            {
                return _bankNameHeader;
            }
            set
            {
                _bankNameHeader = value;
            }
        }

        public virtual string AccountName
        {
            get
            {
                return _accountName;
            }
            set
            {
                _accountName = value;
            }
        }

        public virtual System.Nullable<int> AccountNameOffset
        {
            get
            {
                return _accountNameOffset;
            }
            set
            {
                _accountNameOffset = value;
            }
        }

        public virtual string AccountNameHeader
        {
            get
            {
                return _accountNameHeader;
            }
            set
            {
                _accountNameHeader = value;
            }
        }

        public virtual string Coc
        {
            get
            {
                return _coc;
            }
            set
            {
                _coc = value;
            }
        }

        public virtual System.Nullable<int> CocOffset
        {
            get
            {
                return _cocOffset;
            }
            set
            {
                _cocOffset = value;
            }
        }

        public virtual string CocHeader
        {
            get
            {
                return _cocHeader;
            }
            set
            {
                _cocHeader = value;
            }
        }

        public virtual string SortCode
        {
            get
            {
                return _sortCode;
            }
            set
            {
                _sortCode = value;
            }
        }

        public virtual System.Nullable<int> SortCodeOffset
        {
            get
            {
                return _sortCodeOffset;
            }
            set
            {
                _sortCodeOffset = value;
            }
        }

        public virtual string SortCodeHeader
        {
            get
            {
                return _sortCodeHeader;
            }
            set
            {
                _sortCodeHeader = value;
            }
        }

        public virtual string IbanNo
        {
            get
            {
                return _ibanNo;
            }
            set
            {
                _ibanNo = value;
            }
        }

        public virtual System.Nullable<int> IbanNoOffset
        {
            get
            {
                return _ibanNoOffset;
            }
            set
            {
                _ibanNoOffset = value;
            }
        }

        public virtual string IbanNoHeader
        {
            get
            {
                return _ibanNoHeader;
            }
            set
            {
                _ibanNoHeader = value;
            }
        }

        public virtual string SwiftNo
        {
            get
            {
                return _swiftNo;
            }
            set
            {
                _swiftNo = value;
            }
        }

        public virtual System.Nullable<int> SwiftNoOffset
        {
            get
            {
                return _swiftNoOffset;
            }
            set
            {
                _swiftNoOffset = value;
            }
        }

        public virtual string SwiftNoHeader
        {
            get
            {
                return _swiftNoHeader;
            }
            set
            {
                _swiftNoHeader = value;
            }
        }

        public virtual string AccountNo
        {
            get
            {
                return _accountNo;
            }
            set
            {
                _accountNo = value;
            }
        }

        public virtual System.Nullable<int> AccountNoOffset
        {
            get
            {
                return _accountNoOffset;
            }
            set
            {
                _accountNoOffset = value;
            }
        }

        public virtual string AccountNoHeader
        {
            get
            {
                return _accountNoHeader;
            }
            set
            {
                _accountNoHeader = value;
            }
        }

        public virtual string KvkNo
        {
            get
            {
                return _kvkNo;
            }
            set
            {
                _kvkNo = value;
            }
        }

        public virtual System.Nullable<int> KvkNoOffset
        {
            get
            {
                return _kvkNoOffset;
            }
            set
            {
                _kvkNoOffset = value;
            }
        }

        public virtual string KvkNoHeader
        {
            get
            {
                return _kvkNoHeader;
            }
            set
            {
                _kvkNoHeader = value;
            }
        }

        //public virtual string BicNo
        //{
        //    get
        //    {
        //        return _bicNo;
        //    }
        //    set
        //    {
        //        _bicNo = value;
        //    }
        //}

        //public virtual System.Nullable<int> BicNoOffset
        //{
        //    get
        //    {
        //        return _bicNoOffset;
        //    }
        //    set
        //    {
        //        _bicNoOffset = value;
        //    }
        //}

        //public virtual string BicNoHeader
        //{
        //    get
        //    {
        //        return _bicNoHeader;
        //    }
        //    set
        //    {
        //        _bicNoHeader = value;
        //    }
        //}

        public virtual string ItemsTotalAmount
        {
            get
            {
                return _itemsTotalAmount;
            }
            set
            {
                _itemsTotalAmount = value;
            }
        }

        public virtual System.Nullable<int> ItemsTotalAmountOffset
        {
            get
            {
                return _itemsTotalAmountOffset;
            }
            set
            {
                _itemsTotalAmountOffset = value;
            }
        }

        public virtual string ItemsTotalAmountHeader
        {
            get
            {
                return _itemsTotalAmountHeader;
            }
            set
            {
                _itemsTotalAmountHeader = value;
            }
        }

        public virtual string AllowanceAmount
        {
            get
            {
                return _allowanceAmount;
            }
            set
            {
                _allowanceAmount = value;
            }
        }

        public virtual System.Nullable<int> AllowanceAmountOffset
        {
            get
            {
                return _allowanceAmountOffset;
            }
            set
            {
                _allowanceAmountOffset = value;
            }
        }

        public virtual string AllowanceAmountHeader
        {
            get
            {
                return _allowanceAmountHeader;
            }
            set
            {
                _allowanceAmountHeader = value;
            }
        }

        public virtual string FrightAmount
        {
            get
            {
                return _frightAmount;
            }
            set
            {
                _frightAmount = value;
            }
        }

        public virtual System.Nullable<int> FrightAmountOffset
        {
            get
            {
                return _frightAmountOffset;
            }
            set
            {
                _frightAmountOffset = value;
            }
        }

        public virtual string FrightAmountHeader
        {
            get
            {
                return _frightAmountHeader;
            }
            set
            {
                _frightAmountHeader = value;
            }
        }

        public virtual string PackingCost
        {
            get
            {
                return _packingCost;
            }
            set
            {
                _packingCost = value;
            }
        }

        public virtual System.Nullable<int> PackingCostOffset
        {
            get
            {
                return _packingCostOffset;
            }
            set
            {
                _packingCostOffset = value;
            }
        }

        public virtual string PackingCostHeader
        {
            get
            {
                return _packingCostHeader;
            }
            set
            {
                _packingCostHeader = value;
            }
        }

        public virtual string VatAmount
        {
            get
            {
                return _vatAmount;
            }
            set
            {
                _vatAmount = value;
            }
        }

        public virtual System.Nullable<int> VatAmountOffset
        {
            get
            {
                return _vatAmountOffset;
            }
            set
            {
                _vatAmountOffset = value;
            }
        }

        public virtual string VatAmountHeader
        {
            get
            {
                return _vatAmountHeader;
            }
            set
            {
                _vatAmountHeader = value;
            }
        }

        public virtual string FcaAmount
        {
            get
            {
                return _fcaAmount;
            }
            set
            {
                _fcaAmount = value;
            }
        }

        public virtual System.Nullable<int> FcaAmountOffset
        {
            get
            {
                return _fcaAmountOffset;
            }
            set
            {
                _fcaAmountOffset = value;
            }
        }

        public virtual string FcaAmountHeader
        {
            get
            {
                return _fcaAmountHeader;
            }
            set
            {
                _fcaAmountHeader = value;
            }
        }

        public virtual string CourierCharges
        {
            get
            {
                return _courierCharges;
            }
            set
            {
                _courierCharges = value;
            }
        }

        public virtual System.Nullable<int> CourierChargesOffset
        {
            get
            {
                return _courierChargesOffset;
            }
            set
            {
                _courierChargesOffset = value;
            }
        }

        public virtual string CourierChargesHeader
        {
            get
            {
                return _courierChargesHeader;
            }
            set
            {
                _courierChargesHeader = value;
            }
        }

        public virtual string InsuranceAmount
        {
            get
            {
                return _insuranceAmount;
            }
            set
            {
                _insuranceAmount = value;
            }
        }

        public virtual System.Nullable<int> InsuranceAmountOffset
        {
            get
            {
                return _insuranceAmountOffset;
            }
            set
            {
                _insuranceAmountOffset = value;
            }
        }

        public virtual string InsuranceAmountHeader
        {
            get
            {
                return _insuranceAmountHeader;
            }
            set
            {
                _insuranceAmountHeader = value;
            }
        }

        public virtual string TransactionCharges
        {
            get
            {
                return _transactionCharges;
            }
            set
            {
                _transactionCharges = value;
            }
        }

        public virtual System.Nullable<int> TransactionChargesOffset
        {
            get
            {
                return _transactionChargesOffset;
            }
            set
            {
                _transactionChargesOffset = value;
            }
        }

        public virtual string TransactionChargesHeader
        {
            get
            {
                return _transactionChargesHeader;
            }
            set
            {
                _transactionChargesHeader = value;
            }
        }

        public virtual string FinalTotalAmount
        {
            get
            {
                return _finalTotalAmount;
            }
            set
            {
                _finalTotalAmount = value;
            }
        }

        public virtual System.Nullable<int> FinalTotalAmountOffset
        {
            get
            {
                return _finalTotalAmountOffset;
            }
            set
            {
                _finalTotalAmountOffset = value;
            }
        }

        public virtual string FinalTotalAmountHeader
        {
            get
            {
                return _finalTotalAmountHeader;
            }
            set
            {
                _finalTotalAmountHeader = value;
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

        public virtual string BuyerContactPerson
        {
            get
            {
                return _buyerContactPerson;
            }
            set
            {
                _buyerContactPerson = value;
            }
        }

        public virtual string BuyerTelephone
        {
            get
            {
                return _buyerTelephone;
            }
            set
            {
                _buyerTelephone = value;
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

        public virtual string BuyerWebsite
        {
            get
            {
                return _buyerWebsite;
            }
            set
            {
                _buyerWebsite = value;
            }
        }

        public virtual string SupplierName
        {
            get
            {
                return _supplierName;
            }
            set
            {
                _supplierName = value;
            }
        }

        public virtual string SupplierAddress
        {
            get
            {
                return _supplierAddress;
            }
            set
            {
                _supplierAddress = value;
            }
        }

        public virtual string SupplierContactPerson
        {
            get
            {
                return _supplierContactPerson;
            }
            set
            {
                _supplierContactPerson = value;
            }
        }

        public virtual string SupplierTelephone
        {
            get
            {
                return _supplierTelephone;
            }
            set
            {
                _supplierTelephone = value;
            }
        }

        public virtual string SupplierFax
        {
            get
            {
                return _supplierFax;
            }
            set
            {
                _supplierFax = value;
            }
        }

        public virtual string SupplierEmail
        {
            get
            {
                return _supplierEmail;
            }
            set
            {
                _supplierEmail = value;
            }
        }

        public virtual string SupplierWebsite
        {
            get
            {
                return _supplierWebsite;
            }
            set
            {
                _supplierWebsite = value;
            }
        }

        public virtual string BillingName
        {
            get
            {
                return _billingName;
            }
            set
            {
                _billingName = value;
            }
        }

        public virtual string BillingAddress
        {
            get
            {
                return _billingAddress;
            }
            set
            {
                _billingAddress = value;
            }
        }

        public virtual string BillingContactPerson
        {
            get
            {
                return _billingContactPerson;
            }
            set
            {
                _billingContactPerson = value;
            }
        }

        public virtual string BillingTelephone
        {
            get
            {
                return _billingTelephone;
            }
            set
            {
                _billingTelephone = value;
            }
        }

        public virtual string BillingFax
        {
            get
            {
                return _billingFax;
            }
            set
            {
                _billingFax = value;
            }
        }

        public virtual string BillingEmail
        {
            get
            {
                return _billingEmail;
            }
            set
            {
                _billingEmail = value;
            }
        }

        public virtual string BillingWebsite
        {
            get
            {
                return _billingWebsite;
            }
            set
            {
                _billingWebsite = value;
            }
        }

        public virtual string ConsigneeName
        {
            get
            {
                return _consigneeName;
            }
            set
            {
                _consigneeName = value;
            }
        }

        public virtual string ConsigneeAddress
        {
            get
            {
                return _consigneeAddress;
            }
            set
            {
                _consigneeAddress = value;
            }
        }

        public virtual string ConsigneeContactPerson
        {
            get
            {
                return _consigneeContactPerson;
            }
            set
            {
                _consigneeContactPerson = value;
            }
        }

        public virtual string ConsigneeTelephone
        {
            get
            {
                return _consigneeTelephone;
            }
            set
            {
                _consigneeTelephone = value;
            }
        }

        public virtual string ConsigneeFax
        {
            get
            {
                return _consigneeFax;
            }
            set
            {
                _consigneeFax = value;
            }
        }

        public virtual string ConsigneeEmail
        {
            get
            {
                return _consigneeEmail;
            }
            set
            {
                _consigneeEmail = value;
            }
        }

        public virtual string ConsigneeWebsite
        {
            get
            {
                return _consigneeWebsite;
            }
            set
            {
                _consigneeWebsite = value;
            }
        }

        public virtual string FfName
        {
            get
            {
                return _ffName;
            }
            set
            {
                _ffName = value;
            }
        }

        public virtual string FfAddress
        {
            get
            {
                return _ffAddress;
            }
            set
            {
                _ffAddress = value;
            }
        }

        public virtual string FfContactPerson
        {
            get
            {
                return _ffContactPerson;
            }
            set
            {
                _ffContactPerson = value;
            }
        }

        public virtual string FfTelephone
        {
            get
            {
                return _ffTelephone;
            }
            set
            {
                _ffTelephone = value;
            }
        }

        public virtual string FfFax
        {
            get
            {
                return _ffFax;
            }
            set
            {
                _ffFax = value;
            }
        }

        public virtual string FfEmail
        {
            get
            {
                return _ffEmail;
            }
            set
            {
                _ffEmail = value;
            }
        }

        public virtual string FfWebsite
        {
            get
            {
                return _ffWebsite;
            }
            set
            {
                _ffWebsite = value;
            }
        }

        public virtual System.Nullable<int> HeaderLinesCount
        {
            get
            {
                return _headerLinesCount;
            }
            set
            {
                _headerLinesCount = value;
            }
        }

        public virtual System.Nullable<int> FooterLinesCount
        {
            get
            {
                return _footerLinesCount;
            }
            set
            {
                _footerLinesCount = value;
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

        public virtual string VouStartText
        {
            get
            {
                return _vouStartText;
            }
            set
            {
                _vouStartText = value;
            }
        }

        public virtual string VouEndText
        {
            get
            {
                return _vouEndText;
            }
            set
            {
                _vouEndText = value;
            }
        }

        public virtual string DecimalSeperator
        {
            get
            {
                return _decimalSeperator;
            }
            set
            {
                _decimalSeperator = value;
            }
        }

        public virtual System.Nullable<int> SkipBlankLines
        {
            get
            {
                return _skipBlankLines;
            }
            set
            {
                _skipBlankLines = value;
            }
        }

        public virtual System.Nullable<System.DateTime> CreatedDate
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

        public virtual System.Nullable<System.DateTime> UpdateDate
        {
            get
            {
                return _updateDate;
            }
            set
            {
                _updateDate = value;
            }
        }

        public virtual string BuyerOrgNo
        {
            get
            {
                return _buyerOrgNo;
            }
            set
            {
                _buyerOrgNo = value;
            }
        }

        public virtual System.Nullable<int> BuyerOrgNoOffset
        {
            get
            {
                return _buyerOrgNoOffset;
            }
            set
            {
                _buyerOrgNoOffset = value;
            }
        }

        public virtual string BuyerOrgNoHeader
        {
            get
            {
                return _buyerOrgNoHeader;
            }
            set
            {
                _buyerOrgNoHeader = value;
            }
        }

        public virtual string SupplierOrgNo
        {
            get
            {
                return _supplierOrgNo;
            }
            set
            {
                _supplierOrgNo = value;
            }
        }

        public virtual System.Nullable<int> SupplierOrgNoOffset
        {
            get
            {
                return _supplierOrgNoOffset;
            }
            set
            {
                _supplierOrgNoOffset = value;
            }
        }

        public virtual string SupplierOrgNoHeader
        {
            get
            {
                return _supplierOrgNoHeader;
            }
            set
            {
                _supplierOrgNoHeader = value;
            }
        }

        public virtual System.Nullable<int> AddHeaderToComment
        {
            get
            {
                return _addHeaderToComment;
            }
            set
            {
                _addHeaderToComment = value;
            }
        }

        public virtual System.Nullable<int> AddFooterToComment
        {
            get
            {
                return _addFooterToComment;
            }
            set
            {
                _addFooterToComment = value;
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

        public virtual string SkipText
        {
            get
            {
                return _skipText;
            }
            set
            {
                _skipText = value;
            }
        }

        public virtual string RemarkStartText
        {
            get
            {
                return _remarkStartText;
            }
            set
            {
                _remarkStartText = value;
            }
        }

        public virtual string RemarkEndText
        {
            get
            {
                return _remarkEndText;
            }
            set
            {
                _remarkEndText = value;
            }
        }

        public virtual System.Nullable<int> UseItemMapping
        {
            get
            {
                return _useItemMapping;
            }
            set
            {
                _useItemMapping = value;
            }
        }

        public virtual string EndOfSkipText
        {
            get
            {
                return _endOfSkipText;
            }
            set
            {
                _endOfSkipText = value;
            }
        }

        public virtual string StartOfSkipText
        {
            get
            {
                return _startOfSkipText;
            }
            set
            {
                _startOfSkipText = value;
            }
        }

        public virtual string Mandatory_Fields
        {
            get
            {
                return _mandatory_Fields;
            }
            set
            {
                _mandatory_Fields = value;
            }
        }

        public virtual System.Nullable<int> OCR_Web_Service
        {
            get
            {
                return _ocr_Web_Service;
            }
            set
            {
                _ocr_Web_Service = value;
            }
        }

        private void Clean()
        {
            this.InvPdfMapid = null;
            this.InvMapCode = string.Empty;
            this.Vouchertype = string.Empty;
            this.VoucherNo = string.Empty;
            this.VoucherNoOffset = null;
            this.VoucherNoHeader = string.Empty;
            this.PoNo = string.Empty;
            this.PoNoOffset = null;
            this.PoNoHeader = string.Empty;
            this.PoRefNo = string.Empty;
            this.PoRefNoOffset = null;
            this.PoRefNoHeader = string.Empty;
            this.SupplierDispatchNo = string.Empty;
            this.SupplierDispatchNoOffset = null;
            this.SupplierDispatchNoHeader = string.Empty;
            this.ForwarderDispatchNo = string.Empty;
            this.ForwarderDispatchNoOffset = null;
            this.ForwarderDispatchNoHeader = string.Empty;
            this.SupplierCustomerNo = string.Empty;
            this.SupplierCustomerNoOffset = null;
            this.SupplierCustomerNoHeader = string.Empty;
            //this.BuyerRef = string.Empty;
            //this.BuyerRefOffset = null;
            //this.BuyerRefHeader = string.Empty;
            //this.SupplierRef = string.Empty;
            //this.SupplierRefOffset = null;
            //this.SupplierRefHeader = string.Empty;
            this.Currency = string.Empty;
            this.CurrencyOffset = null;
            this.CurrencyHeader = string.Empty;
            this.VesselImoNo = string.Empty;
            this.VesselImoNoOffset = null;
            this.VesselImoNoHeader = string.Empty;
            this.VesselName = string.Empty;
            this.VesselNameOffset = null;
            this.VesselNameHeader = string.Empty;
            this.PortCode = string.Empty;
            this.PortCodeOffset = null;
            this.PortCodeHeader = string.Empty;
            this.PortName = string.Empty;
            this.PortNameOffset = null;
            this.PortNameHeader = string.Empty;
            this.PaymentTerms = string.Empty;
            this.PaymentTermsOffset = null;
            this.PaymentTermsHeader = string.Empty;
            this.DeliveryTerms = string.Empty;
            this.DeliveryTermsOffset = null;
            this.DeliveryTermsHeader = string.Empty;
            this.SupplierRemarks = string.Empty;
            this.SupplierRemarksOffset = null;
            this.SupplierRemarksHeader = string.Empty;
            this.ItemsCount = string.Empty;
            this.ItemsCountOffset = null;
            this.ItemsCountHeader = string.Empty;
            this.CourierName = string.Empty;
            this.CourierNameOffset = null;
            this.CourierNameHeader = string.Empty;
            this.BuyerVatNo = string.Empty;
            this.BuyerVatNoOffset = null;
            this.BuyerVatNoHeader = string.Empty;
            this.SupplierVatNo = string.Empty;
            this.SupplierVatNoOffset = null;
            this.SupplierVatNoHeader = string.Empty;
            this.VoucherDate = string.Empty;
            this.VoucherDateOffset = null;
            this.VoucherDateHeader = string.Empty;
            this.VoucherDueDate = string.Empty;
            this.VoucherDueDateOffset = null;
            this.VoucherDueDateHeader = string.Empty;
            this.BuyerPoDate = string.Empty;
            this.BuyerPoDateOffset = null;
            this.BuyerPoDateHeader = string.Empty;
            this.DispatchDate = string.Empty;
            this.DispatchDateOffset = null;
            this.DispatchDateHeader = string.Empty;
            this.LateDate = string.Empty;
            this.LateDateOffset = null;
            this.LateDateHeader = string.Empty;
            this.BankName = string.Empty;
            this.BankNameOffset = null;
            this.BankNameHeader = string.Empty;
            this.AccountName = string.Empty;
            this.AccountNameOffset = null;
            this.AccountNameHeader = string.Empty;
            this.Coc = string.Empty;
            this.CocOffset = null;
            this.CocHeader = string.Empty;
            this.SortCode = string.Empty;
            this.SortCodeOffset = null;
            this.SortCodeHeader = string.Empty;
            this.IbanNo = string.Empty;
            this.IbanNoOffset = null;
            this.IbanNoHeader = string.Empty;
            this.SwiftNo = string.Empty;
            this.SwiftNoOffset = null;
            this.SwiftNoHeader = string.Empty;
            this.AccountNo = string.Empty;
            this.AccountNoOffset = null;
            this.AccountNoHeader = string.Empty;
            this.KvkNo = string.Empty;
            this.KvkNoOffset = null;
            this.KvkNoHeader = string.Empty;
            //this.BicNo = string.Empty;
            //this.BicNoOffset = null;
            //this.BicNoHeader = string.Empty;
            this.ItemsTotalAmount = string.Empty;
            this.ItemsTotalAmountOffset = null;
            this.ItemsTotalAmountHeader = string.Empty;
            this.AllowanceAmount = string.Empty;
            this.AllowanceAmountOffset = null;
            this.AllowanceAmountHeader = string.Empty;
            this.FrightAmount = string.Empty;
            this.FrightAmountOffset = null;
            this.FrightAmountHeader = string.Empty;
            this.PackingCost = string.Empty;
            this.PackingCostOffset = null;
            this.PackingCostHeader = string.Empty;
            this.VatAmount = string.Empty;
            this.VatAmountOffset = null;
            this.VatAmountHeader = string.Empty;
            this.FcaAmount = string.Empty;
            this.FcaAmountOffset = null;
            this.FcaAmountHeader = string.Empty;
            this.CourierCharges = string.Empty;
            this.CourierChargesOffset = null;
            this.CourierChargesHeader = string.Empty;
            this.InsuranceAmount = string.Empty;
            this.InsuranceAmountOffset = null;
            this.InsuranceAmountHeader = string.Empty;
            this.TransactionCharges = string.Empty;
            this.TransactionChargesOffset = null;
            this.TransactionChargesHeader = string.Empty;
            this.FinalTotalAmount = string.Empty;
            this.FinalTotalAmountOffset = null;
            this.FinalTotalAmountHeader = string.Empty;
            this.BuyerName = string.Empty;
            this.BuyerAddress = string.Empty;
            this.BuyerContactPerson = string.Empty;
            this.BuyerTelephone = string.Empty;
            this.BuyerFax = string.Empty;
            this.BuyerEmail = string.Empty;
            this.BuyerWebsite = string.Empty;
            this.SupplierName = string.Empty;
            this.SupplierAddress = string.Empty;
            this.SupplierContactPerson = string.Empty;
            this.SupplierTelephone = string.Empty;
            this.SupplierFax = string.Empty;
            this.SupplierEmail = string.Empty;
            this.SupplierWebsite = string.Empty;
            this.BillingName = string.Empty;
            this.BillingAddress = string.Empty;
            this.BillingContactPerson = string.Empty;
            this.BillingTelephone = string.Empty;
            this.BillingFax = string.Empty;
            this.BillingEmail = string.Empty;
            this.BillingWebsite = string.Empty;
            this.ConsigneeName = string.Empty;
            this.ConsigneeAddress = string.Empty;
            this.ConsigneeContactPerson = string.Empty;
            this.ConsigneeTelephone = string.Empty;
            this.ConsigneeFax = string.Empty;
            this.ConsigneeEmail = string.Empty;
            this.ConsigneeWebsite = string.Empty;
            this.FfName = string.Empty;
            this.FfAddress = string.Empty;
            this.FfContactPerson = string.Empty;
            this.FfTelephone = string.Empty;
            this.FfFax = string.Empty;
            this.FfEmail = string.Empty;
            this.FfWebsite = string.Empty;
            this.HeaderLinesCount = null;
            this.FooterLinesCount = null;
            this.DateFormat1 = string.Empty;
            this.DateFormat2 = string.Empty;
            this.VouStartText = string.Empty;
            this.VouEndText = string.Empty;
            this.DecimalSeperator = string.Empty;
            this.SkipBlankLines = null;
            this.CreatedDate = null;
            this.UpdateDate = null;

            this.BuyerOrgNo = string.Empty;
            this.BuyerOrgNoOffset = null;
            this.BuyerOrgNoHeader = string.Empty;

            this.SupplierOrgNo = string.Empty;
            this.SupplierOrgNoOffset = null;
            this.SupplierOrgNoHeader = string.Empty;


            this.AddHeaderToComment = null;
            this.AddFooterToComment = null;
            this.FieldsFromHeader = string.Empty;
            this.FieldsFromFooter = string.Empty;

            this.SkipText = string.Empty;
            this.RemarkStartText = string.Empty;
            this.RemarkEndText = string.Empty;

            this.CreditToInvoiceNo = string.Empty;
            this.CreditToInvoiceNoOffset = null;
            this.CreditToInvoiceNoHeader = string.Empty;
            this.SplitFile = null;
            this.ConstantRowsStartText = string.Empty;
            this.ConstantRowsEndText = string.Empty;

            this.SplitStartText = string.Empty;
            this.SplitEndText = string.Empty;
            this.CurrencyMapping = string.Empty;
            //this._smInvPdfBuyerSupplierLinkCollection = null;

            this.UseItemMapping = null;
            this.StartOfSkipText = string.Empty;
            this.EndOfSkipText = string.Empty;

            this.HeaderStartContent = string.Empty;
            this.HeaderEndContent = string.Empty;

            this.FooterStartContent = string.Empty;
            this.FooterEndContent = string.Empty;

            this.Buyer_Addr_Header = string.Empty;
            this.Supplier_Addr_Header = string.Empty;
            this.Consignee_Addr_Header = string.Empty;
            this.Billing_Addr_Header = string.Empty;
            this.FF_Addr_Header = string.Empty;

            this.Mandatory_Fields = string.Empty;
            this.OCR_Web_Service = null;

            this.Item_Format = string.Empty;
            this.Regex_Fields = string.Empty;

            this.OffsetHeaders_Fields = string.Empty;
            this.Check_Voucher_Type = string.Empty;

            this.Replace_Text_Field = string.Empty;
        }

        private void Fill(System.Data.DataRow dr)
        {
            this.Clean();
            if ((dr["INV_PDF_MAPID"] != System.DBNull.Value))
            {
                this.InvPdfMapid = ((System.Nullable<int>)(dr["INV_PDF_MAPID"]));
            }
            if ((dr["INV_MAP_CODE"] != System.DBNull.Value))
            {
                this.InvMapCode = ((string)(dr["INV_MAP_CODE"]));
            }
            if ((dr["VOUCHERTYPE"] != System.DBNull.Value))
            {
                this.Vouchertype = ((string)(dr["VOUCHERTYPE"]));
            }
            if ((dr["VOUCHER_NO"] != System.DBNull.Value))
            {
                this.VoucherNo = ((string)(dr["VOUCHER_NO"]));
            }
            if ((dr["VOUCHER_NO_OFFSET"] != System.DBNull.Value))
            {
                this.VoucherNoOffset = ((System.Nullable<int>)(dr["VOUCHER_NO_OFFSET"]));
            }
            if ((dr["VOUCHER_NO_HEADER"] != System.DBNull.Value))
            {
                this.VoucherNoHeader = ((string)(dr["VOUCHER_NO_HEADER"]));
            }
            if ((dr["PO_NO"] != System.DBNull.Value))
            {
                this.PoNo = ((string)(dr["PO_NO"]));
            }
            if ((dr["PO_NO_OFFSET"] != System.DBNull.Value))
            {
                this.PoNoOffset = ((System.Nullable<int>)(dr["PO_NO_OFFSET"]));
            }
            if ((dr["PO_NO_HEADER"] != System.DBNull.Value))
            {
                this.PoNoHeader = ((string)(dr["PO_NO_HEADER"]));
            }
            if ((dr["PO_REF_NO"] != System.DBNull.Value))
            {
                this.PoRefNo = ((string)(dr["PO_REF_NO"]));
            }
            if ((dr["PO_REF_NO_OFFSET"] != System.DBNull.Value))
            {
                this.PoRefNoOffset = ((System.Nullable<int>)(dr["PO_REF_NO_OFFSET"]));
            }
            if ((dr["PO_REF_NO_HEADER"] != System.DBNull.Value))
            {
                this.PoRefNoHeader = ((string)(dr["PO_REF_NO_HEADER"]));
            }
            if ((dr["SUPPLIER_DISPATCH_NO"] != System.DBNull.Value))
            {
                this.SupplierDispatchNo = ((string)(dr["SUPPLIER_DISPATCH_NO"]));
            }
            if ((dr["SUPPLIER_DISPATCH_NO_OFFSET"] != System.DBNull.Value))
            {
                this.SupplierDispatchNoOffset = ((System.Nullable<int>)(dr["SUPPLIER_DISPATCH_NO_OFFSET"]));
            }
            if ((dr["SUPPLIER_DISPATCH_NO_HEADER"] != System.DBNull.Value))
            {
                this.SupplierDispatchNoHeader = ((string)(dr["SUPPLIER_DISPATCH_NO_HEADER"]));
            }
            if ((dr["FORWARDER_DISPATCH_NO"] != System.DBNull.Value))
            {
                this.ForwarderDispatchNo = ((string)(dr["FORWARDER_DISPATCH_NO"]));
            }
            if ((dr["FORWARDER_DISPATCH_NO_OFFSET"] != System.DBNull.Value))
            {
                this.ForwarderDispatchNoOffset = ((System.Nullable<int>)(dr["FORWARDER_DISPATCH_NO_OFFSET"]));
            }
            if ((dr["FORWARDER_DISPATCH_NO_HEADER"] != System.DBNull.Value))
            {
                this.ForwarderDispatchNoHeader = ((string)(dr["FORWARDER_DISPATCH_NO_HEADER"]));
            }
            if ((dr["SUPPLIER_CUSTOMER_NO"] != System.DBNull.Value))
            {
                this.SupplierCustomerNo = ((string)(dr["SUPPLIER_CUSTOMER_NO"]));
            }
            if ((dr["SUPPLIER_CUSTOMER_NO_OFFSET"] != System.DBNull.Value))
            {
                this.SupplierCustomerNoOffset = ((System.Nullable<int>)(dr["SUPPLIER_CUSTOMER_NO_OFFSET"]));
            }
            if ((dr["SUPPLIER_CUSTOMER_NO_HEADER"] != System.DBNull.Value))
            {
                this.SupplierCustomerNoHeader = ((string)(dr["SUPPLIER_CUSTOMER_NO_HEADER"]));
            }
            //if ((dr["BUYER_REF"] != System.DBNull.Value))
            //{
            //    this.BuyerRef = ((string)(dr["BUYER_REF"]));
            //}
            //if ((dr["BUYER_REF_OFFSET"] != System.DBNull.Value))
            //{
            //    this.BuyerRefOffset = ((System.Nullable<int>)(dr["BUYER_REF_OFFSET"]));
            //}
            //if ((dr["BUYER_REF_HEADER"] != System.DBNull.Value))
            //{
            //    this.BuyerRefHeader = ((string)(dr["BUYER_REF_HEADER"]));
            //}
            //if ((dr["SUPPLIER_REF"] != System.DBNull.Value))
            //{
            //    this.SupplierRef = ((string)(dr["SUPPLIER_REF"]));
            //}
            //if ((dr["SUPPLIER_REF_OFFSET"] != System.DBNull.Value))
            //{
            //    this.SupplierRefOffset = ((System.Nullable<int>)(dr["SUPPLIER_REF_OFFSET"]));
            //}
            //if ((dr["SUPPLIER_REF_HEADER"] != System.DBNull.Value))
            //{
            //    this.SupplierRefHeader = ((string)(dr["SUPPLIER_REF_HEADER"]));
            //}
            if ((dr["CURRENCY"] != System.DBNull.Value))
            {
                this.Currency = ((string)(dr["CURRENCY"]));
            }
            if ((dr["CURRENCY_OFFSET"] != System.DBNull.Value))
            {
                this.CurrencyOffset = ((System.Nullable<int>)(dr["CURRENCY_OFFSET"]));
            }
            if ((dr["CURRENCY_HEADER"] != System.DBNull.Value))
            {
                this.CurrencyHeader = ((string)(dr["CURRENCY_HEADER"]));
            }
            if ((dr["VESSEL_IMO_NO"] != System.DBNull.Value))
            {
                this.VesselImoNo = ((string)(dr["VESSEL_IMO_NO"]));
            }
            if ((dr["VESSEL_IMO_NO_OFFSET"] != System.DBNull.Value))
            {
                this.VesselImoNoOffset = ((System.Nullable<int>)(dr["VESSEL_IMO_NO_OFFSET"]));
            }
            if ((dr["VESSEL_IMO_NO_HEADER"] != System.DBNull.Value))
            {
                this.VesselImoNoHeader = ((string)(dr["VESSEL_IMO_NO_HEADER"]));
            }
            if ((dr["VESSEL_NAME"] != System.DBNull.Value))
            {
                this.VesselName = ((string)(dr["VESSEL_NAME"]));
            }
            if ((dr["VESSEL_NAME_OFFSET"] != System.DBNull.Value))
            {
                this.VesselNameOffset = ((System.Nullable<int>)(dr["VESSEL_NAME_OFFSET"]));
            }
            if ((dr["VESSEL_NAME_HEADER"] != System.DBNull.Value))
            {
                this.VesselNameHeader = ((string)(dr["VESSEL_NAME_HEADER"]));
            }
            if ((dr["PORT_CODE"] != System.DBNull.Value))
            {
                this.PortCode = ((string)(dr["PORT_CODE"]));
            }
            if ((dr["PORT_CODE_OFFSET"] != System.DBNull.Value))
            {
                this.PortCodeOffset = ((System.Nullable<int>)(dr["PORT_CODE_OFFSET"]));
            }
            if ((dr["PORT_CODE_HEADER"] != System.DBNull.Value))
            {
                this.PortCodeHeader = ((string)(dr["PORT_CODE_HEADER"]));
            }
            if ((dr["PORT_NAME"] != System.DBNull.Value))
            {
                this.PortName = ((string)(dr["PORT_NAME"]));
            }
            if ((dr["PORT_NAME_OFFSET"] != System.DBNull.Value))
            {
                this.PortNameOffset = ((System.Nullable<int>)(dr["PORT_NAME_OFFSET"]));
            }
            if ((dr["PORT_NAME_HEADER"] != System.DBNull.Value))
            {
                this.PortNameHeader = ((string)(dr["PORT_NAME_HEADER"]));
            }
            if ((dr["PAYMENT_TERMS"] != System.DBNull.Value))
            {
                this.PaymentTerms = ((string)(dr["PAYMENT_TERMS"]));
            }
            if ((dr["PAYMENT_TERMS_OFFSET"] != System.DBNull.Value))
            {
                this.PaymentTermsOffset = ((System.Nullable<int>)(dr["PAYMENT_TERMS_OFFSET"]));
            }
            if ((dr["PAYMENT_TERMS_HEADER"] != System.DBNull.Value))
            {
                this.PaymentTermsHeader = ((string)(dr["PAYMENT_TERMS_HEADER"]));
            }
            if ((dr["DELIVERY_TERMS"] != System.DBNull.Value))
            {
                this.DeliveryTerms = ((string)(dr["DELIVERY_TERMS"]));
            }
            if ((dr["DELIVERY_TERMS_OFFSET"] != System.DBNull.Value))
            {
                this.DeliveryTermsOffset = ((System.Nullable<int>)(dr["DELIVERY_TERMS_OFFSET"]));
            }
            if ((dr["DELIVERY_TERMS_HEADER"] != System.DBNull.Value))
            {
                this.DeliveryTermsHeader = ((string)(dr["DELIVERY_TERMS_HEADER"]));
            }
            if ((dr["SUPPLIER_REMARKS"] != System.DBNull.Value))
            {
                this.SupplierRemarks = ((string)(dr["SUPPLIER_REMARKS"]));
            }
            if ((dr["SUPPLIER_REMARKS_OFFSET"] != System.DBNull.Value))
            {
                this.SupplierRemarksOffset = ((System.Nullable<int>)(dr["SUPPLIER_REMARKS_OFFSET"]));
            }
            if ((dr["SUPPLIER_REMARKS_HEADER"] != System.DBNull.Value))
            {
                this.SupplierRemarksHeader = ((string)(dr["SUPPLIER_REMARKS_HEADER"]));
            }
            if ((dr["ITEMS_COUNT"] != System.DBNull.Value))
            {
                this.ItemsCount = ((string)(dr["ITEMS_COUNT"]));
            }
            if ((dr["ITEMS_COUNT_OFFSET"] != System.DBNull.Value))
            {
                this.ItemsCountOffset = ((System.Nullable<int>)(dr["ITEMS_COUNT_OFFSET"]));
            }
            if ((dr["ITEMS_COUNT_HEADER"] != System.DBNull.Value))
            {
                this.ItemsCountHeader = ((string)(dr["ITEMS_COUNT_HEADER"]));
            }
            if ((dr["COURIER_NAME"] != System.DBNull.Value))
            {
                this.CourierName = ((string)(dr["COURIER_NAME"]));
            }
            if ((dr["COURIER_NAME_OFFSET"] != System.DBNull.Value))
            {
                this.CourierNameOffset = ((System.Nullable<int>)(dr["COURIER_NAME_OFFSET"]));
            }
            if ((dr["COURIER_NAME_HEADER"] != System.DBNull.Value))
            {
                this.CourierNameHeader = ((string)(dr["COURIER_NAME_HEADER"]));
            }
            if ((dr["BUYER_VAT_NO"] != System.DBNull.Value))
            {
                this.BuyerVatNo = ((string)(dr["BUYER_VAT_NO"]));
            }
            if ((dr["BUYER_VAT_NO_OFFSET"] != System.DBNull.Value))
            {
                this.BuyerVatNoOffset = ((System.Nullable<int>)(dr["BUYER_VAT_NO_OFFSET"]));
            }
            if ((dr["BUYER_VAT_NO_HEADER"] != System.DBNull.Value))
            {
                this.BuyerVatNoHeader = ((string)(dr["BUYER_VAT_NO_HEADER"]));
            }
            if ((dr["SUPPLIER_VAT_NO"] != System.DBNull.Value))
            {
                this.SupplierVatNo = ((string)(dr["SUPPLIER_VAT_NO"]));
            }
            if ((dr["SUPPLIER_VAT_NO_OFFSET"] != System.DBNull.Value))
            {
                this.SupplierVatNoOffset = ((System.Nullable<int>)(dr["SUPPLIER_VAT_NO_OFFSET"]));
            }
            if ((dr["SUPPLIER_VAT_NO_HEADER"] != System.DBNull.Value))
            {
                this.SupplierVatNoHeader = ((string)(dr["SUPPLIER_VAT_NO_HEADER"]));
            }
            if ((dr["VOUCHER_DATE"] != System.DBNull.Value))
            {
                this.VoucherDate = ((string)(dr["VOUCHER_DATE"]));
            }
            if ((dr["VOUCHER_DATE_OFFSET"] != System.DBNull.Value))
            {
                this.VoucherDateOffset = ((System.Nullable<int>)(dr["VOUCHER_DATE_OFFSET"]));
            }
            if ((dr["VOUCHER_DATE_HEADER"] != System.DBNull.Value))
            {
                this.VoucherDateHeader = ((string)(dr["VOUCHER_DATE_HEADER"]));
            }
            if ((dr["VOUCHER_DUE_DATE"] != System.DBNull.Value))
            {
                this.VoucherDueDate = ((string)(dr["VOUCHER_DUE_DATE"]));
            }
            if ((dr["VOUCHER_DUE_DATE_OFFSET"] != System.DBNull.Value))
            {
                this.VoucherDueDateOffset = ((System.Nullable<int>)(dr["VOUCHER_DUE_DATE_OFFSET"]));
            }
            if ((dr["VOUCHER_DUE_DATE_HEADER"] != System.DBNull.Value))
            {
                this.VoucherDueDateHeader = ((string)(dr["VOUCHER_DUE_DATE_HEADER"]));
            }
            if ((dr["BUYER_PO_DATE"] != System.DBNull.Value))
            {
                this.BuyerPoDate = ((string)(dr["BUYER_PO_DATE"]));
            }
            if ((dr["BUYER_PO_DATE_OFFSET"] != System.DBNull.Value))
            {
                this.BuyerPoDateOffset = ((System.Nullable<int>)(dr["BUYER_PO_DATE_OFFSET"]));
            }
            if ((dr["BUYER_PO_DATE_HEADER"] != System.DBNull.Value))
            {
                this.BuyerPoDateHeader = ((string)(dr["BUYER_PO_DATE_HEADER"]));
            }
            if ((dr["DISPATCH_DATE"] != System.DBNull.Value))
            {
                this.DispatchDate = ((string)(dr["DISPATCH_DATE"]));
            }
            if ((dr["DISPATCH_DATE_OFFSET"] != System.DBNull.Value))
            {
                this.DispatchDateOffset = ((System.Nullable<int>)(dr["DISPATCH_DATE_OFFSET"]));
            }
            if ((dr["DISPATCH_DATE_HEADER"] != System.DBNull.Value))
            {
                this.DispatchDateHeader = ((string)(dr["DISPATCH_DATE_HEADER"]));
            }
            if ((dr["LATE_DATE"] != System.DBNull.Value))
            {
                this.LateDate = ((string)(dr["LATE_DATE"]));
            }
            if ((dr["LATE_DATE_OFFSET"] != System.DBNull.Value))
            {
                this.LateDateOffset = ((System.Nullable<int>)(dr["LATE_DATE_OFFSET"]));
            }
            if ((dr["LATE_DATE_HEADER"] != System.DBNull.Value))
            {
                this.LateDateHeader = ((string)(dr["LATE_DATE_HEADER"]));
            }
            if ((dr["BANK_NAME"] != System.DBNull.Value))
            {
                this.BankName = ((string)(dr["BANK_NAME"]));
            }
            if ((dr["BANK_NAME_OFFSET"] != System.DBNull.Value))
            {
                this.BankNameOffset = ((System.Nullable<int>)(dr["BANK_NAME_OFFSET"]));
            }
            if ((dr["BANK_NAME_HEADER"] != System.DBNull.Value))
            {
                this.BankNameHeader = ((string)(dr["BANK_NAME_HEADER"]));
            }
            if ((dr["ACCOUNT_NAME"] != System.DBNull.Value))
            {
                this.AccountName = ((string)(dr["ACCOUNT_NAME"]));
            }
            if ((dr["ACCOUNT_NAME_OFFSET"] != System.DBNull.Value))
            {
                this.AccountNameOffset = ((System.Nullable<int>)(dr["ACCOUNT_NAME_OFFSET"]));
            }
            if ((dr["ACCOUNT_NAME_HEADER"] != System.DBNull.Value))
            {
                this.AccountNameHeader = ((string)(dr["ACCOUNT_NAME_HEADER"]));
            }
            if ((dr["COC"] != System.DBNull.Value))
            {
                this.Coc = ((string)(dr["COC"]));
            }
            if ((dr["COC_OFFSET"] != System.DBNull.Value))
            {
                this.CocOffset = ((System.Nullable<int>)(dr["COC_OFFSET"]));
            }
            if ((dr["COC_HEADER"] != System.DBNull.Value))
            {
                this.CocHeader = ((string)(dr["COC_HEADER"]));
            }
            if ((dr["SORT_CODE"] != System.DBNull.Value))
            {
                this.SortCode = ((string)(dr["SORT_CODE"]));
            }
            if ((dr["SORT_CODE_OFFSET"] != System.DBNull.Value))
            {
                this.SortCodeOffset = ((System.Nullable<int>)(dr["SORT_CODE_OFFSET"]));
            }
            if ((dr["SORT_CODE_HEADER"] != System.DBNull.Value))
            {
                this.SortCodeHeader = ((string)(dr["SORT_CODE_HEADER"]));
            }
            if ((dr["IBAN_NO"] != System.DBNull.Value))
            {
                this.IbanNo = ((string)(dr["IBAN_NO"]));
            }
            if ((dr["IBAN_NO_OFFSET"] != System.DBNull.Value))
            {
                this.IbanNoOffset = ((System.Nullable<int>)(dr["IBAN_NO_OFFSET"]));
            }
            if ((dr["IBAN_NO_HEADER"] != System.DBNull.Value))
            {
                this.IbanNoHeader = ((string)(dr["IBAN_NO_HEADER"]));
            }
            if ((dr["SWIFT_NO"] != System.DBNull.Value))
            {
                this.SwiftNo = ((string)(dr["SWIFT_NO"]));
            }
            if ((dr["SWIFT_NO_OFFSET"] != System.DBNull.Value))
            {
                this.SwiftNoOffset = ((System.Nullable<int>)(dr["SWIFT_NO_OFFSET"]));
            }
            if ((dr["SWIFT_NO_HEADER"] != System.DBNull.Value))
            {
                this.SwiftNoHeader = ((string)(dr["SWIFT_NO_HEADER"]));
            }
            if ((dr["ACCOUNT_NO"] != System.DBNull.Value))
            {
                this.AccountNo = ((string)(dr["ACCOUNT_NO"]));
            }
            if ((dr["ACCOUNT_NO_OFFSET"] != System.DBNull.Value))
            {
                this.AccountNoOffset = ((System.Nullable<int>)(dr["ACCOUNT_NO_OFFSET"]));
            }
            if ((dr["ACCOUNT_NO_HEADER"] != System.DBNull.Value))
            {
                this.AccountNoHeader = ((string)(dr["ACCOUNT_NO_HEADER"]));
            }
            if ((dr["KVK_NO"] != System.DBNull.Value))
            {
                this.KvkNo = ((string)(dr["KVK_NO"]));
            }
            if ((dr["KVK_NO_OFFSET"] != System.DBNull.Value))
            {
                this.KvkNoOffset = ((System.Nullable<int>)(dr["KVK_NO_OFFSET"]));
            }
            if ((dr["KVK_NO_HEADER"] != System.DBNull.Value))
            {
                this.KvkNoHeader = ((string)(dr["KVK_NO_HEADER"]));
            }
            //if ((dr["BIC_NO"] != System.DBNull.Value))
            //{
            //    this.BicNo = ((string)(dr["BIC_NO"]));
            //}
            //if ((dr["BIC_NO_OFFSET"] != System.DBNull.Value))
            //{
            //    this.BicNoOffset = ((System.Nullable<int>)(dr["BIC_NO_OFFSET"]));
            //}
            //if ((dr["BIC_NO_HEADER"] != System.DBNull.Value))
            //{
            //    this.BicNoHeader = ((string)(dr["BIC_NO_HEADER"]));
            //}
            if ((dr["ITEMS_TOTAL_AMOUNT"] != System.DBNull.Value))
            {
                this.ItemsTotalAmount = ((string)(dr["ITEMS_TOTAL_AMOUNT"]));
            }
            if ((dr["ITEMS_TOTAL_AMOUNT_OFFSET"] != System.DBNull.Value))
            {
                this.ItemsTotalAmountOffset = ((System.Nullable<int>)(dr["ITEMS_TOTAL_AMOUNT_OFFSET"]));
            }
            if ((dr["ITEMS_TOTAL_AMOUNT_HEADER"] != System.DBNull.Value))
            {
                this.ItemsTotalAmountHeader = ((string)(dr["ITEMS_TOTAL_AMOUNT_HEADER"]));
            }
            if ((dr["ALLOWANCE_AMOUNT"] != System.DBNull.Value))
            {
                this.AllowanceAmount = ((string)(dr["ALLOWANCE_AMOUNT"]));
            }
            if ((dr["ALLOWANCE_AMOUNT_OFFSET"] != System.DBNull.Value))
            {
                this.AllowanceAmountOffset = ((System.Nullable<int>)(dr["ALLOWANCE_AMOUNT_OFFSET"]));
            }
            if ((dr["ALLOWANCE_AMOUNT_HEADER"] != System.DBNull.Value))
            {
                this.AllowanceAmountHeader = ((string)(dr["ALLOWANCE_AMOUNT_HEADER"]));
            }
            if ((dr["FRIGHT_AMOUNT"] != System.DBNull.Value))
            {
                this.FrightAmount = ((string)(dr["FRIGHT_AMOUNT"]));
            }
            if ((dr["FRIGHT_AMOUNT_OFFSET"] != System.DBNull.Value))
            {
                this.FrightAmountOffset = ((System.Nullable<int>)(dr["FRIGHT_AMOUNT_OFFSET"]));
            }
            if ((dr["FRIGHT_AMOUNT_HEADER"] != System.DBNull.Value))
            {
                this.FrightAmountHeader = ((string)(dr["FRIGHT_AMOUNT_HEADER"]));
            }
            if ((dr["PACKING_COST"] != System.DBNull.Value))
            {
                this.PackingCost = ((string)(dr["PACKING_COST"]));
            }
            if ((dr["PACKING_COST_OFFSET"] != System.DBNull.Value))
            {
                this.PackingCostOffset = ((System.Nullable<int>)(dr["PACKING_COST_OFFSET"]));
            }
            if ((dr["PACKING_COST_HEADER"] != System.DBNull.Value))
            {
                this.PackingCostHeader = ((string)(dr["PACKING_COST_HEADER"]));
            }
            if ((dr["VAT_AMOUNT"] != System.DBNull.Value))
            {
                this.VatAmount = ((string)(dr["VAT_AMOUNT"]));
            }
            if ((dr["VAT_AMOUNT_OFFSET"] != System.DBNull.Value))
            {
                this.VatAmountOffset = ((System.Nullable<int>)(dr["VAT_AMOUNT_OFFSET"]));
            }
            if ((dr["VAT_AMOUNT_HEADER"] != System.DBNull.Value))
            {
                this.VatAmountHeader = ((string)(dr["VAT_AMOUNT_HEADER"]));
            }
            if ((dr["FCA_AMOUNT"] != System.DBNull.Value))
            {
                this.FcaAmount = ((string)(dr["FCA_AMOUNT"]));
            }
            if ((dr["FCA_AMOUNT_OFFSET"] != System.DBNull.Value))
            {
                this.FcaAmountOffset = ((System.Nullable<int>)(dr["FCA_AMOUNT_OFFSET"]));
            }
            if ((dr["FCA_AMOUNT_HEADER"] != System.DBNull.Value))
            {
                this.FcaAmountHeader = ((string)(dr["FCA_AMOUNT_HEADER"]));
            }
            if ((dr["COURIER_CHARGES"] != System.DBNull.Value))
            {
                this.CourierCharges = ((string)(dr["COURIER_CHARGES"]));
            }
            if ((dr["COURIER_CHARGES_OFFSET"] != System.DBNull.Value))
            {
                this.CourierChargesOffset = ((System.Nullable<int>)(dr["COURIER_CHARGES_OFFSET"]));
            }
            if ((dr["COURIER_CHARGES_HEADER"] != System.DBNull.Value))
            {
                this.CourierChargesHeader = ((string)(dr["COURIER_CHARGES_HEADER"]));
            }
            if ((dr["INSURANCE_AMOUNT"] != System.DBNull.Value))
            {
                this.InsuranceAmount = ((string)(dr["INSURANCE_AMOUNT"]));
            }
            if ((dr["INSURANCE_AMOUNT_OFFSET"] != System.DBNull.Value))
            {
                this.InsuranceAmountOffset = ((System.Nullable<int>)(dr["INSURANCE_AMOUNT_OFFSET"]));
            }
            if ((dr["INSURANCE_AMOUNT_HEADER"] != System.DBNull.Value))
            {
                this.InsuranceAmountHeader = ((string)(dr["INSURANCE_AMOUNT_HEADER"]));
            }
            if ((dr["TRANSACTION_CHARGES"] != System.DBNull.Value))
            {
                this.TransactionCharges = ((string)(dr["TRANSACTION_CHARGES"]));
            }
            if ((dr["TRANSACTION_CHARGES_OFFSET"] != System.DBNull.Value))
            {
                this.TransactionChargesOffset = ((System.Nullable<int>)(dr["TRANSACTION_CHARGES_OFFSET"]));
            }
            if ((dr["TRANSACTION_CHARGES_HEADER"] != System.DBNull.Value))
            {
                this.TransactionChargesHeader = ((string)(dr["TRANSACTION_CHARGES_HEADER"]));
            }
            if ((dr["FINAL_TOTAL_AMOUNT"] != System.DBNull.Value))
            {
                this.FinalTotalAmount = ((string)(dr["FINAL_TOTAL_AMOUNT"]));
            }
            if ((dr["FINAL_TOTAL_AMOUNT_OFFSET"] != System.DBNull.Value))
            {
                this.FinalTotalAmountOffset = ((System.Nullable<int>)(dr["FINAL_TOTAL_AMOUNT_OFFSET"]));
            }
            if ((dr["FINAL_TOTAL_AMOUNT_HEADER"] != System.DBNull.Value))
            {
                this.FinalTotalAmountHeader = ((string)(dr["FINAL_TOTAL_AMOUNT_HEADER"]));
            }
            if ((dr["BUYER_NAME"] != System.DBNull.Value))
            {
                this.BuyerName = ((string)(dr["BUYER_NAME"]));
            }
            if ((dr["BUYER_ADDRESS"] != System.DBNull.Value))
            {
                this.BuyerAddress = ((string)(dr["BUYER_ADDRESS"]));
            }
            if ((dr["BUYER_CONTACT_PERSON"] != System.DBNull.Value))
            {
                this.BuyerContactPerson = ((string)(dr["BUYER_CONTACT_PERSON"]));
            }
            if ((dr["BUYER_TELEPHONE"] != System.DBNull.Value))
            {
                this.BuyerTelephone = ((string)(dr["BUYER_TELEPHONE"]));
            }
            if ((dr["BUYER_FAX"] != System.DBNull.Value))
            {
                this.BuyerFax = ((string)(dr["BUYER_FAX"]));
            }
            if ((dr["BUYER_EMAIL"] != System.DBNull.Value))
            {
                this.BuyerEmail = ((string)(dr["BUYER_EMAIL"]));
            }
            if ((dr["BUYER_WEBSITE"] != System.DBNull.Value))
            {
                this.BuyerWebsite = ((string)(dr["BUYER_WEBSITE"]));
            }
            if ((dr["SUPPLIER_NAME"] != System.DBNull.Value))
            {
                this.SupplierName = ((string)(dr["SUPPLIER_NAME"]));
            }
            if ((dr["SUPPLIER_ADDRESS"] != System.DBNull.Value))
            {
                this.SupplierAddress = ((string)(dr["SUPPLIER_ADDRESS"]));
            }
            if ((dr["SUPPLIER_CONTACT_PERSON"] != System.DBNull.Value))
            {
                this.SupplierContactPerson = ((string)(dr["SUPPLIER_CONTACT_PERSON"]));
            }
            if ((dr["SUPPLIER_TELEPHONE"] != System.DBNull.Value))
            {
                this.SupplierTelephone = ((string)(dr["SUPPLIER_TELEPHONE"]));
            }
            if ((dr["SUPPLIER_FAX"] != System.DBNull.Value))
            {
                this.SupplierFax = ((string)(dr["SUPPLIER_FAX"]));
            }
            if ((dr["SUPPLIER_EMAIL"] != System.DBNull.Value))
            {
                this.SupplierEmail = ((string)(dr["SUPPLIER_EMAIL"]));
            }
            if ((dr["SUPPLIER_WEBSITE"] != System.DBNull.Value))
            {
                this.SupplierWebsite = ((string)(dr["SUPPLIER_WEBSITE"]));
            }
            if ((dr["BILLING_NAME"] != System.DBNull.Value))
            {
                this.BillingName = ((string)(dr["BILLING_NAME"]));
            }
            if ((dr["BILLING_ADDRESS"] != System.DBNull.Value))
            {
                this.BillingAddress = ((string)(dr["BILLING_ADDRESS"]));
            }
            if ((dr["BILLING_CONTACT_PERSON"] != System.DBNull.Value))
            {
                this.BillingContactPerson = ((string)(dr["BILLING_CONTACT_PERSON"]));
            }
            if ((dr["BILLING_TELEPHONE"] != System.DBNull.Value))
            {
                this.BillingTelephone = ((string)(dr["BILLING_TELEPHONE"]));
            }
            if ((dr["BILLING_FAX"] != System.DBNull.Value))
            {
                this.BillingFax = ((string)(dr["BILLING_FAX"]));
            }
            if ((dr["BILLING_EMAIL"] != System.DBNull.Value))
            {
                this.BillingEmail = ((string)(dr["BILLING_EMAIL"]));
            }
            if ((dr["BILLING_WEBSITE"] != System.DBNull.Value))
            {
                this.BillingWebsite = ((string)(dr["BILLING_WEBSITE"]));
            }
            if ((dr["CONSIGNEE_NAME"] != System.DBNull.Value))
            {
                this.ConsigneeName = ((string)(dr["CONSIGNEE_NAME"]));
            }
            if ((dr["CONSIGNEE_ADDRESS"] != System.DBNull.Value))
            {
                this.ConsigneeAddress = ((string)(dr["CONSIGNEE_ADDRESS"]));
            }
            if ((dr["CONSIGNEE_CONTACT_PERSON"] != System.DBNull.Value))
            {
                this.ConsigneeContactPerson = ((string)(dr["CONSIGNEE_CONTACT_PERSON"]));
            }
            if ((dr["CONSIGNEE_TELEPHONE"] != System.DBNull.Value))
            {
                this.ConsigneeTelephone = ((string)(dr["CONSIGNEE_TELEPHONE"]));
            }
            if ((dr["CONSIGNEE_FAX"] != System.DBNull.Value))
            {
                this.ConsigneeFax = ((string)(dr["CONSIGNEE_FAX"]));
            }
            if ((dr["CONSIGNEE_EMAIL"] != System.DBNull.Value))
            {
                this.ConsigneeEmail = ((string)(dr["CONSIGNEE_EMAIL"]));
            }
            if ((dr["CONSIGNEE_WEBSITE"] != System.DBNull.Value))
            {
                this.ConsigneeWebsite = ((string)(dr["CONSIGNEE_WEBSITE"]));
            }
            if ((dr["FF_NAME"] != System.DBNull.Value))
            {
                this.FfName = ((string)(dr["FF_NAME"]));
            }
            if ((dr["FF_ADDRESS"] != System.DBNull.Value))
            {
                this.FfAddress = ((string)(dr["FF_ADDRESS"]));
            }
            if ((dr["FF_CONTACT_PERSON"] != System.DBNull.Value))
            {
                this.FfContactPerson = ((string)(dr["FF_CONTACT_PERSON"]));
            }
            if ((dr["FF_TELEPHONE"] != System.DBNull.Value))
            {
                this.FfTelephone = ((string)(dr["FF_TELEPHONE"]));
            }
            if ((dr["FF_FAX"] != System.DBNull.Value))
            {
                this.FfFax = ((string)(dr["FF_FAX"]));
            }
            if ((dr["FF_EMAIL"] != System.DBNull.Value))
            {
                this.FfEmail = ((string)(dr["FF_EMAIL"]));
            }
            if ((dr["FF_WEBSITE"] != System.DBNull.Value))
            {
                this.FfWebsite = ((string)(dr["FF_WEBSITE"]));
            }
            if ((dr["HEADER_LINES_COUNT"] != System.DBNull.Value))
            {
                this.HeaderLinesCount = ((System.Nullable<int>)(dr["HEADER_LINES_COUNT"]));
            }
            if ((dr["FOOTER_LINES_COUNT"] != System.DBNull.Value))
            {
                this.FooterLinesCount = ((System.Nullable<int>)(dr["FOOTER_LINES_COUNT"]));
            }
            if ((dr["DATE_FORMAT_1"] != System.DBNull.Value))
            {
                this.DateFormat1 = ((string)(dr["DATE_FORMAT_1"]));
            }
            if ((dr["DATE_FORMAT_2"] != System.DBNull.Value))
            {
                this.DateFormat2 = ((string)(dr["DATE_FORMAT_2"]));
            }
            if ((dr["VOU_START_TEXT"] != System.DBNull.Value))
            {
                this.VouStartText = ((string)(dr["VOU_START_TEXT"]));
            }
            if ((dr["VOU_END_TEXT"] != System.DBNull.Value))
            {
                this.VouEndText = ((string)(dr["VOU_END_TEXT"]));
            }
            if ((dr["DECIMAL_SEPERATOR"] != System.DBNull.Value))
            {
                this.DecimalSeperator = ((string)(dr["DECIMAL_SEPERATOR"]));
            }
            if ((dr["SKIP_BLANK_LINES"] != System.DBNull.Value))
            {
                this.SkipBlankLines = ((System.Nullable<int>)(dr["SKIP_BLANK_LINES"]));
            }
            if ((dr["CREATED_DATE"] != System.DBNull.Value))
            {
                this.CreatedDate = ((System.Nullable<System.DateTime>)(dr["CREATED_DATE"]));
            }
            if ((dr["UPDATE_DATE"] != System.DBNull.Value))
            {
                this.UpdateDate = ((System.Nullable<System.DateTime>)(dr["UPDATE_DATE"]));
            }
            if ((dr["BUYER_ORG_NO"] != System.DBNull.Value))
            {
                this.BuyerOrgNo = ((string)(dr["BUYER_ORG_NO"]));
            }
            if ((dr["BUYER_ORG_NO_OFFSET"] != System.DBNull.Value))
            {
                this.BuyerOrgNoOffset = ((System.Nullable<int>)(dr["BUYER_ORG_NO_OFFSET"]));
            }
            if ((dr["BUYER_ORG_NO_HEADER"] != System.DBNull.Value))
            {
                this.BuyerOrgNoHeader = ((string)(dr["BUYER_ORG_NO_HEADER"]));
            }
            if ((dr["SUPPLIER_ORG_NO"] != System.DBNull.Value))
            {
                this.SupplierOrgNo = ((string)(dr["SUPPLIER_ORG_NO"]));
            }
            if ((dr["SUPPLIER_ORG_NO_OFFSET"] != System.DBNull.Value))
            {
                this.SupplierOrgNoOffset = ((System.Nullable<int>)(dr["SUPPLIER_ORG_NO_OFFSET"]));
            }
            if ((dr["SUPPLIER_ORG_NO_HEADER"] != System.DBNull.Value))
            {
                this.SupplierOrgNoHeader = ((string)(dr["SUPPLIER_ORG_NO_HEADER"]));
            }
            if ((dr["ADD_HEADER_TO_COMMENTS"] != System.DBNull.Value))
            {
                this.AddHeaderToComment = ((System.Nullable<int>)(dr["ADD_HEADER_TO_COMMENTS"]));
            }
            if ((dr["ADD_FOOTER_TO_COMMENTS"] != System.DBNull.Value))
            {
                this.AddFooterToComment = ((System.Nullable<int>)(dr["ADD_FOOTER_TO_COMMENTS"]));
            }
            if ((dr["FIELDS_FROM_HEADER"] != System.DBNull.Value))
            {
                this.FieldsFromHeader = ((string)(dr["FIELDS_FROM_HEADER"]));
            }
            if ((dr["FIELDS_FROM_FOOTER"] != System.DBNull.Value))
            {
                this.FieldsFromFooter = ((string)(dr["FIELDS_FROM_FOOTER"]));
            }

            if ((dr["SKIP_TEXT"] != System.DBNull.Value))
            {
                this.SkipText = ((string)(dr["SKIP_TEXT"]));
            }

            if ((dr["REMARKS_START_TEXT"] != System.DBNull.Value))
            {
                this.RemarkStartText = ((string)(dr["REMARKS_START_TEXT"]));
            }
            if ((dr["REMARKS_END_TEXT"] != System.DBNull.Value))
            {
                this.RemarkEndText = ((string)(dr["REMARKS_END_TEXT"]));
            }

            if ((dr["CREDIT_TO_INVOICE_NO"] != System.DBNull.Value))
            {
                this.CreditToInvoiceNo = ((string)(dr["CREDIT_TO_INVOICE_NO"]));
            }
            if ((dr["CREDIT_TO_INVOICE_NO_OFFSET"] != System.DBNull.Value))
            {
                this.CreditToInvoiceNoOffset = ((System.Nullable<int>)(dr["CREDIT_TO_INVOICE_NO_OFFSET"]));
            }
            if ((dr["CREDIT_TO_INVOICE_NO_HEADER"] != System.DBNull.Value))
            {
                this.CreditToInvoiceNoHeader = ((string)(dr["CREDIT_TO_INVOICE_NO_HEADER"]));
            }

            if ((dr["SPLIT_FILE"] != System.DBNull.Value))
            {
                this.SplitFile = ((System.Nullable<int>)(dr["SPLIT_FILE"]));
            }
            if ((dr["CONSTANT_ROWS_START_TEXT"] != System.DBNull.Value))
            {
                this.ConstantRowsStartText = ((string)(dr["CONSTANT_ROWS_START_TEXT"]));
            }

            if ((dr["CONSTANT_ROWS_END_TEXT"] != System.DBNull.Value))
            {
                this.ConstantRowsEndText = ((string)(dr["CONSTANT_ROWS_END_TEXT"]));
            }

            if ((dr["SPLIT_START_TEXT"] != System.DBNull.Value))
            {
                this.SplitStartText = ((string)(dr["SPLIT_START_TEXT"]));
            }
            if ((dr["SPLIT_END_TEXT"] != System.DBNull.Value))
            {
                this.SplitEndText = ((string)(dr["SPLIT_END_TEXT"]));
            }
            if ((dr["CURRENCY_MAPPING"] != System.DBNull.Value))
            {
                this.CurrencyMapping = ((string)(dr["CURRENCY_MAPPING"]));
            }

            if ((dr["USE_ITEM_MAPPING"] != System.DBNull.Value))
            {
                this.UseItemMapping = ((System.Nullable<int>)(dr["USE_ITEM_MAPPING"]));
            }
            if ((dr["START_OF_SKIP_TEXT"] != System.DBNull.Value))
            {
                this.StartOfSkipText = ((string)(dr["START_OF_SKIP_TEXT"]));
            }
            if ((dr["END_OF_SKIP_TEXT"] != System.DBNull.Value))
            {
                this.EndOfSkipText = ((string)(dr["END_OF_SKIP_TEXT"]));
            }

            if ((dr["HEADER_START_CONTENT"] != System.DBNull.Value))
            {
                this.HeaderStartContent = ((string)(dr["HEADER_START_CONTENT"]));
            }
            if ((dr["HEADER_END_CONTENT"] != System.DBNull.Value))
            {
                this.HeaderEndContent = ((string)(dr["HEADER_END_CONTENT"]));
            }

            if ((dr["FOOTER_START_CONTENT"] != System.DBNull.Value))
            {
                this.FooterStartContent = ((string)(dr["FOOTER_START_CONTENT"]));
            }
            if ((dr["FOOTER_END_CONTENT"] != System.DBNull.Value))
            {
                this.FooterEndContent = ((string)(dr["FOOTER_END_CONTENT"]));
            }

            if ((dr["BUYER_ADDR_HEADER"] != System.DBNull.Value))
            {
                this.Buyer_Addr_Header = ((string)(dr["BUYER_ADDR_HEADER"]));
            }
            if ((dr["SUPPLIER_ADDR_HEADER"] != System.DBNull.Value))
            {
                this.Supplier_Addr_Header = ((string)(dr["SUPPLIER_ADDR_HEADER"]));
            }
            if ((dr["CONSIGNEE_ADDR_HEADER"] != System.DBNull.Value))
            {
                this.Consignee_Addr_Header = ((string)(dr["CONSIGNEE_ADDR_HEADER"]));
            }
            if ((dr["BILLING_ADDR_HEADER"] != System.DBNull.Value))
            {
                this.Billing_Addr_Header = ((string)(dr["BILLING_ADDR_HEADER"]));
            }
            if ((dr["FF_ADDR_HEADER"] != System.DBNull.Value))
            {
                this.FF_Addr_Header = ((string)(dr["FF_ADDR_HEADER"]));
            }

            if ((dr["MANDATORY_FIELDS"] != System.DBNull.Value))
            {
                this.Mandatory_Fields = ((string)(dr["MANDATORY_FIELDS"]));
            }
            if ((dr["USE_WEB_OCR_SERVICE"] != System.DBNull.Value))
            {
                this.OCR_Web_Service = ((System.Nullable<int>)(dr["USE_WEB_OCR_SERVICE"]));
            }


            if ((dr["ITEM_FORMAT"] != System.DBNull.Value))
            {
                this.Item_Format = ((string)(dr["ITEM_FORMAT"]));
            }
            if ((dr["REGEX_FIELDS"] != System.DBNull.Value))
            {
                this.Regex_Fields = ((string)(dr["REGEX_FIELDS"]));
            }

            if ((dr["OFFSET_HEADER_FIELDS"] != System.DBNull.Value))
            {
                this.OffsetHeaders_Fields = ((string)(dr["OFFSET_HEADER_FIELDS"]));
            }
            if ((dr["CHECK_VOUCHER_TYPE"] != System.DBNull.Value))
            {
                this.Check_Voucher_Type = ((string)(dr["CHECK_VOUCHER_TYPE"]));
            }
            if ((dr["REPLACE_TEXT_FIELDS"] != System.DBNull.Value))
            {
                this.Replace_Text_Field = ((string)(dr["REPLACE_TEXT_FIELDS"]));
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

        public DataSet Export()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("INV_PDF_MAPID", typeof(int));
            dt.Columns.Add("INV_MAP_CODE");
            dt.Columns.Add("VOUCHERTYPE");
            dt.Columns.Add("VOUCHER_NO");
            dt.Columns.Add("VOUCHER_NO_OFFSET", typeof(int));
            dt.Columns.Add("VOUCHER_NO_HEADER");
            dt.Columns.Add("PO_NO");
            dt.Columns.Add("PO_NO_OFFSET", typeof(int));
            dt.Columns.Add("PO_NO_HEADER");
            dt.Columns.Add("PO_REF_NO");
            dt.Columns.Add("PO_REF_NO_OFFSET", typeof(int));
            dt.Columns.Add("PO_REF_NO_HEADER");
            dt.Columns.Add("SUPPLIER_DISPATCH_NO");
            dt.Columns.Add("SUPPLIER_DISPATCH_NO_OFFSET", typeof(int));
            dt.Columns.Add("SUPPLIER_DISPATCH_NO_HEADER");
            dt.Columns.Add("FORWARDER_DISPATCH_NO");
            dt.Columns.Add("FORWARDER_DISPATCH_NO_OFFSET", typeof(int));
            dt.Columns.Add("FORWARDER_DISPATCH_NO_HEADER");
            dt.Columns.Add("SUPPLIER_CUSTOMER_NO");
            dt.Columns.Add("SUPPLIER_CUSTOMER_NO_OFFSET", typeof(int));
            dt.Columns.Add("SUPPLIER_CUSTOMER_NO_HEADER");
            //dt.Columns.Add("BUYER_REF");
            //dt.Columns.Add("BUYER_REF_OFFSET", typeof(int));
            //dt.Columns.Add("BUYER_REF_HEADER");
            //dt.Columns.Add("SUPPLIER_REF");
            //dt.Columns.Add("SUPPLIER_REF_OFFSET", typeof(int));
            //dt.Columns.Add("SUPPLIER_REF_HEADER");
            dt.Columns.Add("CURRENCY");
            dt.Columns.Add("CURRENCY_OFFSET", typeof(int));
            dt.Columns.Add("CURRENCY_HEADER");
            dt.Columns.Add("VESSEL_IMO_NO");
            dt.Columns.Add("VESSEL_IMO_NO_OFFSET", typeof(int));
            dt.Columns.Add("VESSEL_IMO_NO_HEADER");
            dt.Columns.Add("VESSEL_NAME");
            dt.Columns.Add("VESSEL_NAME_OFFSET", typeof(int));
            dt.Columns.Add("VESSEL_NAME_HEADER");
            dt.Columns.Add("PORT_CODE");
            dt.Columns.Add("PORT_CODE_OFFSET", typeof(int));
            dt.Columns.Add("PORT_CODE_HEADER");
            dt.Columns.Add("PORT_NAME");
            dt.Columns.Add("PORT_NAME_OFFSET", typeof(int));
            dt.Columns.Add("PORT_NAME_HEADER");
            dt.Columns.Add("PAYMENT_TERMS");
            dt.Columns.Add("PAYMENT_TERMS_OFFSET", typeof(int));
            dt.Columns.Add("PAYMENT_TERMS_HEADER");
            dt.Columns.Add("DELIVERY_TERMS");
            dt.Columns.Add("DELIVERY_TERMS_OFFSET", typeof(int));
            dt.Columns.Add("DELIVERY_TERMS_HEADER");
            dt.Columns.Add("SUPPLIER_REMARKS");
            dt.Columns.Add("SUPPLIER_REMARKS_OFFSET", typeof(int));
            dt.Columns.Add("SUPPLIER_REMARKS_HEADER");
            dt.Columns.Add("ITEMS_COUNT");
            dt.Columns.Add("ITEMS_COUNT_OFFSET", typeof(int));
            dt.Columns.Add("ITEMS_COUNT_HEADER");
            dt.Columns.Add("COURIER_NAME");
            dt.Columns.Add("COURIER_NAME_OFFSET", typeof(int));
            dt.Columns.Add("COURIER_NAME_HEADER");
            dt.Columns.Add("BUYER_VAT_NO");
            dt.Columns.Add("BUYER_VAT_NO_OFFSET", typeof(int));
            dt.Columns.Add("BUYER_VAT_NO_HEADER");
            dt.Columns.Add("SUPPLIER_VAT_NO");
            dt.Columns.Add("SUPPLIER_VAT_NO_OFFSET", typeof(int));
            dt.Columns.Add("SUPPLIER_VAT_NO_HEADER");
            dt.Columns.Add("VOUCHER_DATE");
            dt.Columns.Add("VOUCHER_DATE_OFFSET", typeof(int));
            dt.Columns.Add("VOUCHER_DATE_HEADER");
            dt.Columns.Add("VOUCHER_DUE_DATE");
            dt.Columns.Add("VOUCHER_DUE_DATE_OFFSET", typeof(int));
            dt.Columns.Add("VOUCHER_DUE_DATE_HEADER");
            dt.Columns.Add("BUYER_PO_DATE");
            dt.Columns.Add("BUYER_PO_DATE_OFFSET", typeof(int));
            dt.Columns.Add("BUYER_PO_DATE_HEADER");
            dt.Columns.Add("DISPATCH_DATE");
            dt.Columns.Add("DISPATCH_DATE_OFFSET", typeof(int));
            dt.Columns.Add("DISPATCH_DATE_HEADER");
            dt.Columns.Add("LATE_DATE");
            dt.Columns.Add("LATE_DATE_OFFSET", typeof(int));
            dt.Columns.Add("LATE_DATE_HEADER");
            dt.Columns.Add("BANK_NAME");
            dt.Columns.Add("BANK_NAME_OFFSET", typeof(int));
            dt.Columns.Add("BANK_NAME_HEADER");
            dt.Columns.Add("ACCOUNT_NAME");
            dt.Columns.Add("ACCOUNT_NAME_OFFSET", typeof(int));
            dt.Columns.Add("ACCOUNT_NAME_HEADER");
            dt.Columns.Add("COC");
            dt.Columns.Add("COC_OFFSET", typeof(int));
            dt.Columns.Add("COC_HEADER");
            dt.Columns.Add("SORT_CODE");
            dt.Columns.Add("SORT_CODE_OFFSET", typeof(int));
            dt.Columns.Add("SORT_CODE_HEADER");
            dt.Columns.Add("IBAN_NO");
            dt.Columns.Add("IBAN_NO_OFFSET", typeof(int));
            dt.Columns.Add("IBAN_NO_HEADER");
            dt.Columns.Add("SWIFT_NO");
            dt.Columns.Add("SWIFT_NO_OFFSET", typeof(int));
            dt.Columns.Add("SWIFT_NO_HEADER");
            dt.Columns.Add("ACCOUNT_NO");
            dt.Columns.Add("ACCOUNT_NO_OFFSET", typeof(int));
            dt.Columns.Add("ACCOUNT_NO_HEADER");
            dt.Columns.Add("KVK_NO");
            dt.Columns.Add("KVK_NO_OFFSET", typeof(int));
            dt.Columns.Add("KVK_NO_HEADER");
            //dt.Columns.Add("BIC_NO");
            //dt.Columns.Add("BIC_NO_OFFSET", typeof(int));
            //dt.Columns.Add("BIC_NO_HEADER");
            dt.Columns.Add("ITEMS_TOTAL_AMOUNT");
            dt.Columns.Add("ITEMS_TOTAL_AMOUNT_OFFSET", typeof(int));
            dt.Columns.Add("ITEMS_TOTAL_AMOUNT_HEADER");
            dt.Columns.Add("ALLOWANCE_AMOUNT");
            dt.Columns.Add("ALLOWANCE_AMOUNT_OFFSET", typeof(int));
            dt.Columns.Add("ALLOWANCE_AMOUNT_HEADER");
            dt.Columns.Add("FRIGHT_AMOUNT");
            dt.Columns.Add("FRIGHT_AMOUNT_OFFSET", typeof(int));
            dt.Columns.Add("FRIGHT_AMOUNT_HEADER");
            dt.Columns.Add("PACKING_COST");
            dt.Columns.Add("PACKING_COST_OFFSET", typeof(int));
            dt.Columns.Add("PACKING_COST_HEADER");
            dt.Columns.Add("VAT_AMOUNT");
            dt.Columns.Add("VAT_AMOUNT_OFFSET", typeof(int));
            dt.Columns.Add("VAT_AMOUNT_HEADER");
            dt.Columns.Add("FCA_AMOUNT");
            dt.Columns.Add("FCA_AMOUNT_OFFSET", typeof(int));
            dt.Columns.Add("FCA_AMOUNT_HEADER");
            dt.Columns.Add("COURIER_CHARGES");
            dt.Columns.Add("COURIER_CHARGES_OFFSET", typeof(int));
            dt.Columns.Add("COURIER_CHARGES_HEADER");
            dt.Columns.Add("INSURANCE_AMOUNT");
            dt.Columns.Add("INSURANCE_AMOUNT_OFFSET", typeof(int));
            dt.Columns.Add("INSURANCE_AMOUNT_HEADER");
            dt.Columns.Add("TRANSACTION_CHARGES");
            dt.Columns.Add("TRANSACTION_CHARGES_OFFSET", typeof(int));
            dt.Columns.Add("TRANSACTION_CHARGES_HEADER");
            dt.Columns.Add("FINAL_TOTAL_AMOUNT");
            dt.Columns.Add("FINAL_TOTAL_AMOUNT_OFFSET", typeof(int));
            dt.Columns.Add("FINAL_TOTAL_AMOUNT_HEADER");
            dt.Columns.Add("BUYER_NAME");
            dt.Columns.Add("BUYER_ADDRESS");
            dt.Columns.Add("BUYER_CONTACT_PERSON");
            dt.Columns.Add("BUYER_TELEPHONE");
            dt.Columns.Add("BUYER_FAX");
            dt.Columns.Add("BUYER_EMAIL");
            dt.Columns.Add("BUYER_WEBSITE");
            dt.Columns.Add("SUPPLIER_NAME");
            dt.Columns.Add("SUPPLIER_ADDRESS");
            dt.Columns.Add("SUPPLIER_CONTACT_PERSON");
            dt.Columns.Add("SUPPLIER_TELEPHONE");
            dt.Columns.Add("SUPPLIER_FAX");
            dt.Columns.Add("SUPPLIER_EMAIL");
            dt.Columns.Add("SUPPLIER_WEBSITE");
            dt.Columns.Add("BILLING_NAME");
            dt.Columns.Add("BILLING_ADDRESS");
            dt.Columns.Add("BILLING_CONTACT_PERSON");
            dt.Columns.Add("BILLING_TELEPHONE");
            dt.Columns.Add("BILLING_FAX");
            dt.Columns.Add("BILLING_EMAIL");
            dt.Columns.Add("BILLING_WEBSITE");
            dt.Columns.Add("CONSIGNEE_NAME");
            dt.Columns.Add("CONSIGNEE_ADDRESS");
            dt.Columns.Add("CONSIGNEE_CONTACT_PERSON");
            dt.Columns.Add("CONSIGNEE_TELEPHONE");
            dt.Columns.Add("CONSIGNEE_FAX");
            dt.Columns.Add("CONSIGNEE_EMAIL");
            dt.Columns.Add("CONSIGNEE_WEBSITE");
            dt.Columns.Add("FF_NAME");
            dt.Columns.Add("FF_ADDRESS");
            dt.Columns.Add("FF_CONTACT_PERSON");
            dt.Columns.Add("FF_TELEPHONE");
            dt.Columns.Add("FF_FAX");
            dt.Columns.Add("FF_EMAIL");
            dt.Columns.Add("FF_WEBSITE");
            dt.Columns.Add("HEADER_LINES_COUNT", typeof(int));
            dt.Columns.Add("FOOTER_LINES_COUNT", typeof(int));
            dt.Columns.Add("DATE_FORMAT_1");
            dt.Columns.Add("DATE_FORMAT_2");
            dt.Columns.Add("VOU_START_TEXT");
            dt.Columns.Add("VOU_END_TEXT");
            dt.Columns.Add("DECIMAL_SEPERATOR");
            dt.Columns.Add("SKIP_BLANK_LINES", typeof(int));
            dt.Columns.Add("CREATED_DATE", typeof(DateTime));
            dt.Columns.Add("UPDATE_DATE", typeof(DateTime));
            dt.Columns.Add("BUYER_ORG_NO");
            dt.Columns.Add("BUYER_ORG_NO_OFFSET", typeof(int));
            dt.Columns.Add("BUYER_ORG_NO_HEADER");
            dt.Columns.Add("SUPPLIER_ORG_NO");
            dt.Columns.Add("SUPPLIER_ORG_NO_OFFSET", typeof(int));
            dt.Columns.Add("SUPPLIER_ORG_NO_HEADER");

            dt.Columns.Add("ADD_HEADER_TO_COMMENTS", typeof(int));
            dt.Columns.Add("ADD_FOOTER_TO_COMMENTS", typeof(int));
            dt.Columns.Add("FIELDS_FROM_HEADER");
            dt.Columns.Add("FIELDS_FROM_FOOTER");

            dt.Columns.Add("SKIP_TEXT");
            dt.Columns.Add("REMARKS_START_TEXT");
            dt.Columns.Add("REMARKS_END_TEXT");

            dt.Columns.Add("CREDIT_TO_INVOICE_NO");
            dt.Columns.Add("CREDIT_TO_INVOICE_NO_OFFSET", typeof(int));
            dt.Columns.Add("CREDIT_TO_INVOICE_NO_HEADER");

            dt.Columns.Add("SPLIT_FILE", typeof(int));

            dt.Columns.Add("CONSTANT_ROWS_START_TEXT");
            dt.Columns.Add("CONSTANT_ROWS_END_TEXT");

            dt.Columns.Add("SPLIT_START_TEXT");
            dt.Columns.Add("SPLIT_END_TEXT");

            dt.Columns.Add("CURRENCY_MAPPING");

            dt.Columns.Add("USE_ITEM_MAPPING", typeof(int));
            dt.Columns.Add("START_OF_SKIP_TEXT");
            dt.Columns.Add("END_OF_SKIP_TEXT");

            dt.Columns.Add("HEADER_START_CONTENT");
            dt.Columns.Add("HEADER_END_CONTENT");
            dt.Columns.Add("FOOTER_START_CONTENT");
            dt.Columns.Add("FOOTER_END_CONTENT");

            dt.Columns.Add("BUYER_ADDR_HEADER");
            dt.Columns.Add("SUPPLIER_ADDR_HEADER");
            dt.Columns.Add("CONSIGNEE_ADDR_HEADER");
            dt.Columns.Add("BILLING_ADDR_HEADER");
            dt.Columns.Add("FF_ADDR_HEADER");

            dt.Columns.Add("MANDATORY_FIELDS");
            dt.Columns.Add("USE_WEB_OCR_SERVICE", typeof(int));

            dt.Columns.Add("REGEX_FIELDS");
            dt.Columns.Add("ITEM_FORMAT");

            dt.Columns.Add("OFFSET_HEADER_FIELDS");

            dt.Columns.Add("CHECK_VOUCHER_TYPE");
            dt.Columns.Add("REPLACE_TEXT_FIELDS");

            DataRow dr = dt.NewRow();

            dr["INV_PDF_MAPID"] = this.InvPdfMapid;

            dr["INV_MAP_CODE"] = this.InvMapCode;

            dr["VOUCHERTYPE"] = this.Vouchertype;

            dr["VOUCHER_NO"] = this.VoucherNo;

            dr["VOUCHER_NO_OFFSET"] = this.VoucherNoOffset;

            dr["VOUCHER_NO_HEADER"] = this.VoucherNoHeader;

            dr["PO_NO"] = this.PoNo;

            dr["PO_NO_OFFSET"] = this.PoNoOffset;

            dr["PO_NO_HEADER"] = this.PoNoHeader;

            dr["PO_REF_NO"] = this.PoRefNo;

            dr["PO_REF_NO_OFFSET"] = this.PoRefNoOffset;

            dr["PO_REF_NO_HEADER"] = this.PoRefNoHeader;

            dr["SUPPLIER_DISPATCH_NO"] = this.SupplierDispatchNo;

            dr["SUPPLIER_DISPATCH_NO_OFFSET"] = this.SupplierDispatchNoOffset;

            dr["SUPPLIER_DISPATCH_NO_HEADER"] = this.SupplierDispatchNoHeader;

            dr["FORWARDER_DISPATCH_NO"] = this.ForwarderDispatchNo;

            dr["FORWARDER_DISPATCH_NO_OFFSET"] = this.ForwarderDispatchNoOffset;

            dr["FORWARDER_DISPATCH_NO_HEADER"] = this.ForwarderDispatchNoHeader;

            dr["SUPPLIER_CUSTOMER_NO"] = this.SupplierCustomerNo;

            dr["SUPPLIER_CUSTOMER_NO_OFFSET"] = this.SupplierCustomerNoOffset;

            dr["SUPPLIER_CUSTOMER_NO_HEADER"] = this.SupplierCustomerNoHeader;

            //dr["BUYER_REF"] = this.BuyerRef;

            //dr["BUYER_REF_OFFSET"] = this.BuyerRefOffset;

            //dr["BUYER_REF_HEADER"] = this.BuyerRefHeader;

            //dr["SUPPLIER_REF"] = this.SupplierRef;

            //dr["SUPPLIER_REF_OFFSET"] = this.SupplierRefOffset;

            //dr["SUPPLIER_REF_HEADER"] = this.SupplierRefHeader;

            dr["CURRENCY"] = this.Currency;

            dr["CURRENCY_OFFSET"] = this.CurrencyOffset;

            dr["CURRENCY_HEADER"] = this.CurrencyHeader;

            dr["VESSEL_IMO_NO"] = this.VesselImoNo;

            dr["VESSEL_IMO_NO_OFFSET"] = this.VesselImoNoOffset;

            dr["VESSEL_IMO_NO_HEADER"] = this.VesselImoNoHeader;

            dr["VESSEL_NAME"] = this.VesselName;

            dr["VESSEL_NAME_OFFSET"] = this.VesselNameOffset;

            dr["VESSEL_NAME_HEADER"] = this.VesselNameHeader;

            dr["PORT_CODE"] = this.PortCode;

            dr["PORT_CODE_OFFSET"] = this.PortCodeOffset;

            dr["PORT_CODE_HEADER"] = this.PortCodeHeader;

            dr["PORT_NAME"] = this.PortName;

            dr["PORT_NAME_OFFSET"] = this.PortNameOffset;

            dr["PORT_NAME_HEADER"] = this.PortNameHeader;

            dr["PAYMENT_TERMS"] = this.PaymentTerms;

            dr["PAYMENT_TERMS_OFFSET"] = this.PaymentTermsOffset;


            dr["PAYMENT_TERMS_HEADER"] = this.PaymentTermsHeader;

            dr["DELIVERY_TERMS"] = this.DeliveryTerms;

            dr["DELIVERY_TERMS_OFFSET"] = this.DeliveryTermsOffset;

            dr["DELIVERY_TERMS_HEADER"] = this.DeliveryTermsHeader;

            dr["SUPPLIER_REMARKS"] = this.SupplierRemarks;

            dr["SUPPLIER_REMARKS_OFFSET"] = this.SupplierRemarksOffset;

            dr["SUPPLIER_REMARKS_HEADER"] = this.SupplierRemarksHeader;

            dr["ITEMS_COUNT"] = this.ItemsCount;

            dr["ITEMS_COUNT_OFFSET"] = this.ItemsCountOffset;

            dr["ITEMS_COUNT_HEADER"] = this.ItemsCountHeader;

            dr["COURIER_NAME"] = this.CourierName;

            dr["COURIER_NAME_OFFSET"] = this.CourierNameOffset;

            dr["COURIER_NAME_HEADER"] = this.CourierNameHeader;

            dr["BUYER_VAT_NO"] = this.BuyerVatNo;

            dr["BUYER_VAT_NO_OFFSET"] = this.BuyerVatNoOffset;

            dr["BUYER_VAT_NO_HEADER"] = this.BuyerVatNoHeader;

            dr["SUPPLIER_VAT_NO"] = this.SupplierVatNo;

            dr["SUPPLIER_VAT_NO_OFFSET"] = this.SupplierVatNoOffset;

            dr["SUPPLIER_VAT_NO_HEADER"] = this.SupplierVatNoHeader;

            dr["VOUCHER_DATE"] = this.VoucherDate;

            dr["VOUCHER_DATE_OFFSET"] = this.VoucherDateOffset;

            dr["VOUCHER_DATE_HEADER"] = this.VoucherDateHeader;

            dr["VOUCHER_DUE_DATE"] = this.VoucherDueDate;

            dr["VOUCHER_DUE_DATE_OFFSET"] = this.VoucherDueDateOffset;

            dr["VOUCHER_DUE_DATE_HEADER"] = this.VoucherDueDateHeader;

            dr["BUYER_PO_DATE"] = this.BuyerPoDate;

            dr["BUYER_PO_DATE_OFFSET"] = this.BuyerPoDateOffset;

            dr["BUYER_PO_DATE_HEADER"] = this.BuyerPoDateHeader;

            dr["DISPATCH_DATE"] = this.DispatchDate;

            dr["DISPATCH_DATE_OFFSET"] = this.DispatchDateOffset;

            dr["DISPATCH_DATE_HEADER"] = this.DispatchDateHeader;

            dr["LATE_DATE"] = this.LateDate;

            dr["LATE_DATE_OFFSET"] = this.LateDateOffset;

            dr["LATE_DATE_HEADER"] = this.LateDateHeader;

            dr["BANK_NAME"] = this.BankName;

            dr["BANK_NAME_OFFSET"] = this.BankNameOffset;

            dr["BANK_NAME_HEADER"] = this.BankNameHeader;

            dr["ACCOUNT_NAME"] = this.AccountName;

            dr["ACCOUNT_NAME_OFFSET"] = this.AccountNameOffset;

            dr["ACCOUNT_NAME_HEADER"] = this.AccountNameHeader;

            dr["COC"] = this.Coc;

            dr["COC_OFFSET"] = this.CocOffset;

            dr["COC_HEADER"] = this.CocHeader;

            dr["SORT_CODE"] = this.SortCode;

            dr["SORT_CODE_OFFSET"] = this.SortCodeOffset;

            dr["SORT_CODE_HEADER"] = this.SortCodeHeader;

            dr["IBAN_NO"] = this.IbanNo;

            dr["IBAN_NO_OFFSET"] = this.IbanNoOffset;

            dr["IBAN_NO_HEADER"] = this.IbanNoHeader;

            dr["SWIFT_NO"] = this.SwiftNo;

            dr["SWIFT_NO_OFFSET"] = this.SwiftNoOffset;

            dr["SWIFT_NO_HEADER"] = this.SwiftNoHeader;

            dr["ACCOUNT_NO"] = this.AccountNo;

            dr["ACCOUNT_NO_OFFSET"] = this.AccountNoOffset;

            dr["ACCOUNT_NO_HEADER"] = this.AccountNoHeader;

            dr["KVK_NO"] = this.KvkNo;

            dr["KVK_NO_OFFSET"] = this.KvkNoOffset;

            dr["KVK_NO_HEADER"] = this.KvkNoHeader;

            //dr["BIC_NO"] = this.BicNo;

            //dr["BIC_NO_OFFSET"] = this.BicNoOffset;

            //dr["BIC_NO_HEADER"] = this.BicNoHeader;

            dr["ITEMS_TOTAL_AMOUNT"] = this.ItemsTotalAmount;

            dr["ITEMS_TOTAL_AMOUNT_OFFSET"] = this.ItemsTotalAmountOffset;

            dr["ITEMS_TOTAL_AMOUNT_HEADER"] = this.ItemsTotalAmountHeader;

            dr["ALLOWANCE_AMOUNT"] = this.AllowanceAmount;

            dr["ALLOWANCE_AMOUNT_OFFSET"] = this.AllowanceAmountOffset;

            dr["ALLOWANCE_AMOUNT_HEADER"] = this.AllowanceAmountHeader;

            dr["FRIGHT_AMOUNT"] = this.FrightAmount;

            dr["FRIGHT_AMOUNT_OFFSET"] = this.FrightAmountOffset;

            dr["FRIGHT_AMOUNT_HEADER"] = this.FrightAmountHeader;

            dr["PACKING_COST"] = this.PackingCost;

            dr["PACKING_COST_OFFSET"] = this.PackingCostOffset;

            dr["PACKING_COST_HEADER"] = this.PackingCostHeader;

            dr["VAT_AMOUNT"] = this.VatAmount;

            dr["VAT_AMOUNT_OFFSET"] = this.VatAmountOffset;

            dr["VAT_AMOUNT_HEADER"] = this.VatAmountHeader;

            dr["FCA_AMOUNT"] = this.FcaAmount;

            dr["FCA_AMOUNT_OFFSET"] = this.FcaAmountOffset;

            dr["FCA_AMOUNT_HEADER"] = this.FcaAmountHeader;

            dr["COURIER_CHARGES"] = this.CourierCharges;

            dr["COURIER_CHARGES_OFFSET"] = this.CourierChargesOffset;

            dr["COURIER_CHARGES_HEADER"] = this.CourierChargesHeader;

            dr["INSURANCE_AMOUNT"] = this.InsuranceAmount;

            dr["INSURANCE_AMOUNT_OFFSET"] = this.InsuranceAmountOffset;

            dr["INSURANCE_AMOUNT_HEADER"] = this.InsuranceAmountHeader;

            dr["TRANSACTION_CHARGES"] = this.TransactionCharges;

            dr["TRANSACTION_CHARGES_OFFSET"] = this.TransactionChargesOffset;

            dr["TRANSACTION_CHARGES_HEADER"] = this.TransactionChargesHeader;

            dr["FINAL_TOTAL_AMOUNT"] = this.FinalTotalAmount;

            dr["FINAL_TOTAL_AMOUNT_OFFSET"] = this.FinalTotalAmountOffset;

            dr["FINAL_TOTAL_AMOUNT_HEADER"] = this.FinalTotalAmountHeader;

            dr["BUYER_NAME"] = this.BuyerName;

            dr["BUYER_ADDRESS"] = this.BuyerAddress;

            dr["BUYER_CONTACT_PERSON"] = this.BuyerContactPerson;

            dr["BUYER_TELEPHONE"] = this.BuyerTelephone;

            dr["BUYER_FAX"] = this.BuyerFax;

            dr["BUYER_EMAIL"] = this.BuyerEmail;

            dr["BUYER_WEBSITE"] = this.BuyerWebsite;

            dr["SUPPLIER_NAME"] = this.SupplierName;

            dr["SUPPLIER_ADDRESS"] = this.SupplierAddress;

            dr["SUPPLIER_CONTACT_PERSON"] = this.SupplierContactPerson;

            dr["SUPPLIER_TELEPHONE"] = this.SupplierTelephone;

            dr["SUPPLIER_FAX"] = this.SupplierFax;

            dr["SUPPLIER_EMAIL"] = this.SupplierEmail;

            dr["SUPPLIER_WEBSITE"] = this.SupplierWebsite;

            dr["BILLING_NAME"] = this.BillingName;

            dr["BILLING_ADDRESS"] = this.BillingAddress;

            dr["BILLING_CONTACT_PERSON"] = this.BillingContactPerson;

            dr["BILLING_TELEPHONE"] = this.BillingTelephone;

            dr["BILLING_FAX"] = this.BillingFax;

            dr["BILLING_EMAIL"] = this.BillingEmail;

            dr["BILLING_WEBSITE"] = this.BillingWebsite;

            dr["CONSIGNEE_NAME"] = this.ConsigneeName;

            dr["CONSIGNEE_ADDRESS"] = this.ConsigneeAddress;

            dr["CONSIGNEE_CONTACT_PERSON"] = this.ConsigneeContactPerson;

            dr["CONSIGNEE_TELEPHONE"] = this.ConsigneeTelephone;

            dr["CONSIGNEE_FAX"] = this.ConsigneeFax;

            dr["CONSIGNEE_EMAIL"] = this.ConsigneeEmail;

            dr["CONSIGNEE_WEBSITE"] = this.ConsigneeWebsite;

            dr["FF_NAME"] = this.FfName;

            dr["FF_ADDRESS"] = this.FfAddress;

            dr["FF_CONTACT_PERSON"] = this.FfContactPerson;

            dr["FF_TELEPHONE"] = this.FfTelephone;

            dr["FF_FAX"] = this.FfFax;

            dr["FF_EMAIL"] = this.FfEmail;

            dr["FF_WEBSITE"] = this.FfWebsite;

            dr["HEADER_LINES_COUNT"] = this.HeaderLinesCount;

            dr["FOOTER_LINES_COUNT"] = this.FooterLinesCount;

            dr["DATE_FORMAT_1"] = this.DateFormat1;

            dr["DATE_FORMAT_2"] = this.DateFormat2;

            dr["VOU_START_TEXT"] = this.VouStartText;

            dr["VOU_END_TEXT"] = this.VouEndText;

            dr["DECIMAL_SEPERATOR"] = this.DecimalSeperator;

            dr["SKIP_BLANK_LINES"] = this.SkipBlankLines;

            dr["CREATED_DATE"] = this.CreatedDate;

            dr["UPDATE_DATE"] = this.UpdateDate;

            dr["BUYER_ORG_NO"] = this.BuyerOrgNo;

            dr["BUYER_ORG_NO_OFFSET"] = this.BuyerOrgNoOffset;

            dr["BUYER_ORG_NO_HEADER"] = this.BuyerOrgNoHeader;

            dr["SUPPLIER_ORG_NO"] = this.SupplierOrgNo;

            dr["SUPPLIER_ORG_NO_OFFSET"] = this.SupplierOrgNoOffset;

            dr["SUPPLIER_ORG_NO_HEADER"] = this.SupplierOrgNoHeader;

            dr["ADD_HEADER_TO_COMMENTS"] = this.AddHeaderToComment;
            dr["ADD_FOOTER_TO_COMMENTS"] = this.AddFooterToComment;
            dr["FIELDS_FROM_HEADER"] = this.FieldsFromHeader;
            dr["FIELDS_FROM_FOOTER"] = this.FieldsFromFooter;

            dr["SKIP_TEXT"] = this.SkipText;
            dr["REMARKS_START_TEXT"] = this.RemarkStartText;
            dr["REMARKS_END_TEXT"] = this.RemarkEndText;

            dr["CREDIT_TO_INVOICE_NO"] = this.CreditToInvoiceNo;
            dr["CREDIT_TO_INVOICE_NO_OFFSET"] = this.CreditToInvoiceNoOffset;
            dr["CREDIT_TO_INVOICE_NO_HEADER"] = this.CreditToInvoiceNoHeader;

            dr["SPLIT_FILE"] = this.SplitFile;

            dr["CONSTANT_ROWS_START_TEXT"] = this.ConstantRowsStartText;
            dr["CONSTANT_ROWS_END_TEXT"] = this.ConstantRowsEndText;

            dr["SPLIT_START_TEXT"] = this.SplitStartText;
            dr["SPLIT_END_TEXT"] = this.SplitEndText;

            dr["CURRENCY_MAPPING"] = this.CurrencyMapping;

            dr["USE_ITEM_MAPPING"] = this.UseItemMapping;
            dr["START_OF_SKIP_TEXT"] = this.StartOfSkipText;
            dr["END_OF_SKIP_TEXT"] = this.EndOfSkipText;

            dr["HEADER_START_CONTENT"] = this.HeaderStartContent;
            dr["HEADER_END_CONTENT"] = this.HeaderEndContent;
            dr["FOOTER_START_CONTENT"] = this.FooterStartContent;
            dr["FOOTER_END_CONTENT"] = this.FooterEndContent;

            dr["BUYER_ADDR_HEADER"] = this.Buyer_Addr_Header;
            dr["SUPPLIER_ADDR_HEADER"] = this.Supplier_Addr_Header;
            dr["CONSIGNEE_ADDR_HEADER"] = this.Consignee_Addr_Header;
            dr["BILLING_ADDR_HEADER"] = this.Billing_Addr_Header;
            dr["FF_ADDR_HEADER"] = this.FF_Addr_Header;

            dr["MANDATORY_FIELDS"] = this.Mandatory_Fields;
            dr["USE_WEB_OCR_SERVICE"] = this.OCR_Web_Service;

            dr["ITEM_FORMAT"] = this.Item_Format;
            dr["REGEX_FIELDS"] = this.Regex_Fields;

            dr["OFFSET_HEADER_FIELDS"] = this.OffsetHeaders_Fields;
            dr["CHECK_VOUCHER_TYPE"] = this.Check_Voucher_Type;
            dr["REPLACE_TEXT_FIELDS"] = this.Replace_Text_Field;
            dt.Rows.Add(dr);
            ds.Tables.Add(dt);
            return ds;
        }
    }
}