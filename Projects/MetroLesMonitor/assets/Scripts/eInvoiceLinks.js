var selectedTr = ''; var previousTr = ''; var _lstCurrency = []; var _lstOldinvdet = []; var _lstNewinvdet = []; var previousivTr = '';
var _lstInvAccCurr = []; var _lstInvAccPay = []; var _buyercode = ''; var _suppliercode = ''; var _lstBFormts = []; var _lstSFormts = [];

var eInvoiceLinks = function () {

    var handleeInvoiceLinksTable = function () {
        var nEditing = null; var nNew = false; $('#pageTitle').empty().append('eInvoice Links');
        SetupBreadcrumb('Home', 'Home.aspx', 'eInvoice', '#', 'eInvoice Links', 'eInvoice_Links.aspx');
        $(document.getElementById('lnkeInvoice')).addClass('active open'); $(document.getElementById('speInvlink')).addClass('title font-title SelectedColor');
        var _defaddrid = Str(sessionStorage.getItem('ADDRESSID')); var _configaddrid = Str(sessionStorage.getItem('CONFIGADDRESSID')); var _addrtype = Str(sessionStorage.getItem('ADDRTYPE'));
        setupTableHeader(_addrtype); setToolbar();

        var table = $('#dataGrideInvLnk');
        var oeInvTable = table.dataTable({
            "bDestroy": true,
            "bSort": false,
            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "No data available in table",
                "info": "Showing _START_ to _END_ of _TOTAL_ entries",
                "infoEmpty": "No entries found",
                "infoFiltered": "(filtered from _MAX_ total entries)",
                "lengthMenu": "Show _MENU_ entries",
                "search": "Quick Search:",
                "zeroRecords": "No matching records found"
            },
            dom: 'T<"clear">lfrtip',
            tableTools: {
                "sRowSelect": "single",
                "aButtons": ["select_all", "select_none"]
            },
            "columnDefs": [{  // set default column settings
                'orderable': false, "searching": true, "autoWidth": false, 'targets': [0]
            },
            { 'targets': [0],  width: '2px'},  { 'targets': [1], width: '20px', 'bSortable': false },
            { 'targets': [2], width: '80px' }, { 'targets': [3], width: '40px' }, { 'targets': [4], width: '80px' },
            {'targets': [5], visible: false},
            ],            
            "lengthMenu": [
                [15,25, 50, 100, -1],
                [15,25, 50, 100, "All"] // change per page values here
            ],
            "aaSorting": [],
            "drawCallback": function (settings, json) {  $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var divid = 'dv' + nRow._DT_RowIndex; var btnid = 'View' + nRow._DT_RowIndex;
                if (_addrtype.toUpperCase() == 'BUYER') { $('td:eq(0)', nRow).css('display', 'none'); } else {                    
                    var viewbtnTag = '<div style="text-align:center;"><span class="actionbtn" data-toggle="tooltip" title="Details" id= "' + btnid + '"><i class="glyphicon glyphicon-search"></i></span></div>';
                    $('td:eq(0)', nRow).html(viewbtnTag);
                }
            }
        });

        $('#tblHeadRoweInvLnk').addClass('gridHeader'); $('#ToolTables_dataGrideInvLnk_0,#ToolTables_dataGrideInvLnk_1').hide(); $('#dataGrideInvLnk').css('width', '100%');//$('#dataGrideInvLnk_info').hide();     
        Get_einvoiceLinkGrid();   setupBSInvTableHeader();

        var table1 = $('#dataGridBSInv');
        var oBSInvTable =table1.dataTable({
            "bDestroy": true,
            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "No data available in table",
                "info": "Showing _START_ to _END_ of _TOTAL_ entries",
                "infoEmpty": "No entries found",
                "infoFiltered": "(filtered from _MAX_ total entries)",
                "lengthMenu": "Show _MENU_ entries",
                "search": "Quick Search:",
                "zeroRecords": "No matching records found"
            },
            dom: 'T<"clear">lfrtip',
            tableTools: {
                "sRowSelect": "single",
                "aButtons": ["select_all", "select_none"]
            },
            "columnDefs": [{  // set default column settings
                'orderable': false, "searching": true, "autoWidth": false, 'targets': [0],
            },
            { 'targets': [0], width: '15px' }, { 'targets': [1], width: '40px' },
            { 'targets': [2], width: '100px', "sClass": "break-det" }, { 'targets': [3, 4], width: '60px' }, { 'targets': [7], "sClass": "break-det" },
            { 'targets': [5,6], width: '80px' },//{ 'targets': [13,14], width: '90px' },          
            { 'targets': [23], width: '40px', 'bSortable': false }, { 'targets': [24], width: '45px', 'bSortable': false }, { 'targets': [25, 26], width: '70px', 'bSortable': false },
            { 'targets': [27], width: '85px', 'bSortable': false }, { 'targets': [28], width: '35px', 'bSortable': false },
            { 'targets': [7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 29, 30, 31,32,33,34,35,36,37,38], visible: false },
            ],
            "lengthMenu": [
                [25, 50, 100, -1],
                [25, 50, 100, "All"] 
            ],
            "sScrollY": '300px',
            "sScrollX": "1020px",
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var _bcheck = ''; var _nScheck = ''; var _tcheck = ''; var _attcheck = ''; var _poexcheck = ''; var _actexcheck = '';
                var divid = 'dv' + nRow._DT_RowIndex; var aedit = 'rwdedit' + nRow._DT_RowIndex; var aView = 'View' + nRow._DT_RowIndex;
                var _nByid = "by" + nRow._DT_RowIndex; var _nSpid = "sp" + nRow._DT_RowIndex; var _nAttid = "att" + nRow._DT_RowIndex;
                var _nTffid = "tiff" + nRow._DT_RowIndex; var _nPOeid = "poext" + nRow._DT_RowIndex; var _nActid = "act" + nRow._DT_RowIndex;
                var detTag = ' <div id="' + divid + '" style="text-align:center;"><span id=' + aView + ' class="actionbtn" data-toggle="tooltip" title="View"><i class="glyphicon glyphicon-search"></i></span>' +
                 ' <span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span></div>';
                $('td:eq(0)', nRow).html(detTag);             
                var _nByval = aData[23]; var _nSpval = aData[24]; var _nCtffval = aData[25];   var _nATTval = aData[26]; var _nPOeval = aData[27]; var _nActval = aData[28];
                _bcheck = (_nByval == '1') ?'checked': ''; _nScheck = (_nSpval == '1') ?'checked': '';_tcheck = (_nCtffval == '1') ?'checked': '';
                _attcheck =(_nATTval == '1') ? 'checked': '';_poexcheck = (_nPOeval == '1')? 'checked': '';_actexcheck = (_nActval == '1') ? 'checked' : '';
                var _nBuyer = '<div style="text-align:center;"><input type="checkbox"  id="' + _nByid + '"  value="' + _nByval + '" ' + _bcheck + ' disabled/></div>';
                var _nSupplier = '<div style="text-align:center;"><input type="checkbox"  id="' + _nSpid + '"  value="' + _nSpval + '" ' + _nScheck + ' disabled/></div>';
                var _nEmbAtt = '<div style="text-align:center;"><input type="checkbox"  id="' + _nAttid + '"  value="' + _nATTval + '" ' + _attcheck + ' disabled/></div>';
                var _nTiff = '<div style="text-align:center;"><input type="checkbox"  id="' + _nTffid + '"  value="' + _nCtffval + '" ' + _tcheck + ' disabled/></div>';
                var _nPoExist = '<div style="text-align:center;"><input type="checkbox"  id="' + _nPOeid + '"  value="' + _nPOeval + '" ' + _poexcheck + ' disabled/></div>';
                var _nActive = '<div style="text-align:center;"><input type="checkbox"  id="' + _nActid + '"  value="' + _nActval + '" ' + _actexcheck + ' disabled/></div>';
                $('td:eq(7)', nRow).html(_nBuyer); $('td:eq(8)', nRow).html(_nSupplier);$('td:eq(9)', nRow).html(_nTiff);
                $('td:eq(10)', nRow).html(_nEmbAtt);$('td:eq(11)', nRow).html(_nPoExist);$('td:eq(12)', nRow).html(_nActive);
            }
        });
        $('#tblHeadRowBSInv').addClass('gridHeader'); $('#ToolTables_dataGridBSInv_0,#ToolTables_dataGridBSInv_1').hide(); $('#dataGridBSInv').css('width', '100%');

        setupBSInvAccTableHeader();
        var table2 = $('#dataGridBSInvAcc');
        var oBSInvAccTable = table2.dataTable({
            "bDestroy": true,
            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "No data available in table",
                "info": "Showing _START_ to _END_ of _TOTAL_ entries",
                "infoEmpty": "No entries found",
                "infoFiltered": "(filtered from _MAX_ total entries)",
                "lengthMenu": "Show _MENU_ entries",
                "search": "Quick Search:",
                "zeroRecords": "No matching records found"
            },
            dom: 'T<"clear">lfrtip',
            tableTools: {
                "sRowSelect": "single",
                "aButtons": ["select_all", "select_none"]
            },
            "columnDefs": [{  // set default column settings
                'orderable': false, "searching": true, "autoWidth": false, 'targets': [0],
            },
            { 'targets': [0], width: '10px' }, { 'targets': [1], width: '45px' },
            { 'targets': [2], width: '120px' }, { 'targets': [3], width: '40px' }, { 'targets': [3, 4, 5], width: '50px' }, { 'targets': [6, 7, 8], width: '60px' },
            { 'targets': [9], width: '80px' }, { 'targets': [10], width: '90px' },{ 'targets': [11], width: '40px' },{ 'targets': [12,13], visible: false },
            ],
            "lengthMenu": [
                [25, 50, 100, -1],
                [25, 50, 100, "All"] // change per page values here
            ],
            "sScrollY": '300px',
            "sScrollX": "1020px",
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var divid = 'dv' + nRow._DT_RowIndex; var aedit = 'rwdedit' + nRow._DT_RowIndex; var adeleteld = 'rwdelete' + nRow._DT_RowIndex; var aView = 'View' + nRow._DT_RowIndex; var chkid = 'chkDefault' + nRow._DT_RowIndex;
                var detTag = ' <div id="' + divid + '" style="text-align:center;">' +
                 ' <span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span><span id=' + adeleteld + ' class="actionbtn" data-toggle="tooltip" title="Delete"><i class="glyphicon glyphicon-trash"></i></span></div>';
                $('td:eq(0)', nRow).html(detTag); var _default = Str(aData[11]); var _defchk = (_default == '1') ? 'checked' : '';
                var _deftag = '<div style="text-align:center;"><input type="checkbox"  id="' + chkid + '"  value="' + _default + '" ' + _defchk + ' disabled /></div>';
                $('td:eq(11)', nRow).html(_deftag);
            }
        });
        $('#tblHeadRowBSInvAcc').addClass('gridHeader'); $('#ToolTables_dataGridBSInvAcc_0,#ToolTables_dataGridBSInvAcc_1').hide(); $('#dataGridBSInvAcc').css('width', '100%');
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");

        oeInvTable.on('click', 'tbody td', function (e) {
            var selectedTr = $(this).parents('tr')[0];  var aData = oeInvTable.fnGetData(selectedTr); _targetclick = '';
            if (oeInvTable.fnIsOpen(selectedTr)) {  oeInvTable.fnClose(selectedTr);  }
            else {
                if (e.target.className == 'glyphicon glyphicon-search') {
                    var divid = 'dv' + selectedTr._DT_RowIndex;   var _addressid = oeInvTable.fnGetData(selectedTr)[5];
                    sessionStorage['SUPPLIER_ID'] = Str(_addressid); sessionStorage['SUPPLIER_CODE'] = Str(oeInvTable.fnGetData(selectedTr)[1]);
                    var _success = GetBSInvoiceGrid(_addressid);  if (_success == '1') {  $("#ModalBSInv").modal('show');  }
                }
            }
        });

        $("#ModalBSInv").on('shown.bs.modal', function () { oBSInvTable.fnDraw(); });

        oBSInvTable.on('click', 'tbody td', function (e) {
            var selectedTr = $(this).parents('tr')[0];   var aData = oBSInvTable.fnGetData(selectedTr);var cellindx = $(this).index();
            updtcnt = 0; _lstNewinvdet = [];
            if (oBSInvTable.fnIsOpen(selectedTr) && (e.target.className == 'glyphicon glyphicon-pencil')) { oBSInvTable.fnClose(selectedTr); }
            else {
                var divid = 'dv' + selectedTr._DT_RowIndex;
                if (e.target.className == 'glyphicon glyphicon-pencil') {
                    if ((previousTr != '') && (oBSInvTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {
                        var prevdivid = 'dv' + previousTr._DT_RowIndex; oBSInvTable.fnClose(previousTr); $('#' + prevdivid).show();
                    }
                    if (aData != null) { _lstOldinvdet = [];
                        GetInvoiceDetails(oBSInvTable,selectedTr.rowIndex, aData, _lstOldinvdet, 'OldVal');                    
                        oBSInvTable.fnOpen(selectedTr, fnFormatDetails(oBSInvTable, selectedTr), 'details');
                        $('#' + divid).hide(); previousTr = selectedTr;
                    }
                }
                if (e.target.className == 'glyphicon glyphicon-search') {
                    if (aData != null) {
                        var _invlinkid = Str(aData[32]); GetBSInvAccountGrid(_invlinkid); sessionStorage['BUYER_CODE'] = Str(aData[1]); sessionStorage['INVLINKID'] = _invlinkid;
                    } $("#ModalBSInvAcc").modal('show');
                }
                $('#btnUpdate').click(function () { _lstNewinvdet = [];
                    if (aData != null && updtcnt == 0) {
                        var _linkid = aData[31]; var _buyerCode = Str(aData[1]);
                        var _SuppCode = Str(sessionStorage.getItem('SUPPLIER_CODE'));  var _SuppID = Str(sessionStorage.getItem('SUPPLIER_ID'));
                        var _res = ValidateInvLink(selectedTr.rowIndex, _linkid);
                        if (_res == true) {
                            GetInvoiceDetails(oBSInvTable,selectedTr.rowIndex, aData, _lstNewinvdet, 'NewVal');                          
                            SaveInvoiceDetails(_lstOldinvdet, _lstNewinvdet, _linkid, _buyerCode, _SuppCode, GetBSInvoiceGrid, _SuppID);
                            updtcnt++;
                        }
                    }
                    $('#' + divid).show(); previousTr = '';
                });
                $('#btnCancel').click(function () { if (oBSInvTable.fnIsOpen(selectedTr)) { oBSInvTable.fnClose(selectedTr); } $('#' + divid).show(); previousTr = ''; });
            }
        });

        function fnFormatDetails(oTable, nTr) {
            var sOut = ''; var _str = ''; var _newdet = ''; var _bychecked = ''; var _suppchecked = ''; var _tiffchecked = ''; var _pochecked = ''; var _attchecked = ''; var _actchecked = '';
            var indx = nTr.rowIndex; var aData = oBSInvTable.fnGetData(nTr); var tid = "BSInvoiceTable"; var _tbodyid = "tblBodyBSI";
            var _codeid = 'txtCode'; var _blinkcdid = 'txtBLinkCode'; var _slinkcdid = 'txtSLinkCode'; var _bexpfmtid = 'cbBuyExpFmt'; var _simpfmtid = 'cbSuppImpFmt';
            var _currid = 'cbCurrency'; var _bcontid = 'txtBuyContact'; var _scontid = 'txtSuppContact'; var _bemailid = 'txtBuyEmail'; var _semailid = 'txtSuppEmail';
            var _ccid = 'txtCCEmail'; var _bccid = 'txtBCCEmail'; var _accnameid = 'txtAccName'; var _accid = 'txtAccNo'; var _swftid = 'txtSwiftNo'; var _ibanid = 'txtIbanNo';
            var _borgnoid = 'txtByrOrgNo'; var _sorgnoid = 'txtSppOrgNo'; var _bankid = 'txtBankName'; var _paymodid = 'cbPayment';
            var _imppathid = 'txtImpPath'; var _sexppathid = 'txtExpPath'; var _bntfyid = 'chkBNotify'; var _sntfyid = 'chkSNotify'; var _tiffid = 'chkAttachTiff';
            var _eattid = 'chkEmbedAttach'; var _poid = 'chkPOExist'; var _actid = 'chkActive';
            var _code = Str(aData[1]); var _blinkcode = Str(aData[3]); var _slinkcode = Str(aData[4]); var _simpfmt = Str(aData[6]); var _bexpfmt = Str(aData[5]);
            var _bcont = Str(aData[11]); var _scont = Str(aData[12]); var _bemail = Str(aData[13]); var _semail = Str(aData[14]); var _cc = Str(aData[15]);
            var _bcc = Str(aData[16]); var _accno = Str(aData[17]); var _swiftno = Str(aData[18]);  var _ibanno = Str(aData[19]);var _imppath = Str(aData[21]);
            var _sexppath = Str(aData[22]); var _bnotify = Str(aData[23]); var _snotify = Str(aData[24]); var _tiff = Str(aData[25]); var _eattach = Str(aData[26]);
            var _po = Str(aData[27]); var _active = Str(aData[28]); var _buyerid = Str(aData[29]); var _suppid = Str(aData[30]); var _linkid = Str(aData[31]);
            var _sorgno = Str(aData[34]); var _borgno = Str(aData[35]); var bankname = Str(aData[37]); var _accname = Str(aData[38]);

            if (_bnotify == '1') { _bychecked = 'checked'; } else { _bychecked = ''; }
            if (_snotify == '1') { _suppchecked = 'checked'; } else { _suppchecked = ''; }
            if (_tiff == '1') { _tiffchecked = 'checked'; } else { _tiffchecked = ''; }
            if (_eattach == '1') { _attchecked = 'checked'; } else { _attchecked = ''; }
            if (_po == '1') { _pochecked = 'checked'; } else { _pochecked = ''; }
            if (_active == '1') { _actchecked = 'checked'; } else { _actchecked = ''; }

            FillCurrency(); var _currency = FillCombo(Str(aData[20]), _lstInvAccCurr); FillPaymentMode(); var _paymode = FillCombo(Str(aData[36]), _lstInvAccPay);
           _lstSFormts = FillInvoiceFormats('SUPPLIER'); var _simpfmt = FillCombo(Str(aData[6]), _lstSFormts);
           _lstBFormts = FillInvoiceFormats('BUYER'); var _bexpfmt = FillCombo(Str(aData[5]), _lstBFormts);
           
           var btndiv = '<div class="row"><div style="text-align:center;"><a href="#" id="btnUpdate"><u>Update</u></<a>&nbsp;<a href="#" id="btnCancel"><u>Cancel</u></<a></div></div>';
            var sOut = '<div class="row" style="padding-right:63px;"><div class="col-md-10"><div class="row"><div class="col-md-10"><div class="form-group">' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Code </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _codeid + '"  value="' + _code + '"/> </div>' +
               ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Link Code </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _blinkcdid + '"  value="' + _blinkcode + '"/> </div>' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Supplier Link Code </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _slinkcdid + '"  value="' + _slinkcode + '"/> </div>' +
               ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
			   ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Supplier Import Format </label> </div>' +
               ' <div  class="col-md-3"><select class="bs-select form-control" id="' + _simpfmtid + '">' + _simpfmt + '</select> </div>' +
			   ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Export Format </label> </div>' +
               ' <div  class="col-md-3"><select class="bs-select form-control" id="' + _bexpfmtid + '">' + _bexpfmt + '</select> </div>' +
			   ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Currency </label> </div>' +
               ' <div  class="col-md-3"><select class="bs-select form-control" id="' + _currid + '">' + _currency + '</select> </div>' +
               ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Contact </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bcontid + '"  value="' + _bcont + '"/> </div>' +
			   ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Supplier Contact </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _scontid + '"  value="' + _scont + '"/> </div>' +
               ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Email </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bemailid + '"  value="' + _bemail + '"/> </div>' +
			   ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Supplier Email </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _semailid + '"  value="' + _semail + '"/> </div>' +
               ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> CC Email </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _ccid + '"  value="' + _cc + '"/> </div>' +
			   ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> BCC Email </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bccid + '"  value="' + _bcc + '"/> </div>' +
               ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Acc Name </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _accnameid + '"  value="' + _accname + '"/> </div>' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Acc No. </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _accid + '"  value="' + _accno + '"/> </div>' +
               ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Swift No. </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _swftid + '"  value="' + _swiftno + '"/> </div>' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Iban No. </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _ibanid + '"  value="' + _ibanno + '"/> </div>' +
               ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
			   ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Supplier Org No. </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _sorgnoid + '"  value="' + _sorgno + '"/> </div>' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Org No. </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _borgnoid + '"  value="' + _borgno + '"/> </div>' +
			   ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
			   ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Bank Name </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bankid + '"  value="' + bankname + '"/> </div>' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Payment Mode </label> </div>' +
               ' <div  class="col-md-3"><select class="bs-select form-control" id="' + _paymodid + '">' + _paymode + '</select> </div>' +
			   ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
			   ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Import Path </label> </div>' +
               ' <div  class="col-md-3"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _imppathid + '">' + _imppath + '</textarea></div>' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Export Path </label> </div>' +
               ' <div  class="col-md-3"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _sexppathid + '">' + _sexppath + '</textarea></div>' +
               ' </div></div></div><div class="row"><div class="col-md-10"><div class="col-md-2"></div><div class="col-md-9"><div class="form-group">' +
                ' <ul class="checkbox-grid">' +
                ' <li><input class="widelarge" type="checkbox" id="' + _bntfyid + '"  value="' + _bnotify + '"' + _bychecked + ' /><label class="chklabel">Notify Buyer</label></li>' +
                ' <li><input class="widelarge" type="checkbox" id="' + _sntfyid + '"  value="' + _snotify + '"' + _suppchecked + '/><label class="chklabel">Notify Supplier</label></li>' +
                ' <li><input class="widelarge" type="checkbox" id="' + _actid + '"  value="' + _active + '"' + _actchecked + '/><label class="chklabel">Active</label></li>' +
                ' <li style="width:40%;"><input class="widelarge" type="checkbox" id="' + _tiffid + '"  value="' + _tiff + '"' + _tiffchecked + '/><label class="chklabel">Convert Attachment To Tiff</label></li>' +
                '</ul> </div></div>'+
                '<div class="col-md-2"></div><div class="col-md-9"><div class="form-group">'+
                '<ul class="checkbox-grid">' +
                ' <li style="width:40%;"><input class="widelarge" type="checkbox" id="' + _eattid + '"  value="' + _eattach + '"' + _attchecked + '/><label class="chklabel">Embed Attachment To File</label></li>' +               
                ' <li style="width:40%;"> <input class="widelarge" type="checkbox" id="' + _poid + '"  value="' + _po + '"' + _pochecked + '/> <label class="chklabel">Check PO exist on eSupplier</label></li></ul>' +              
                ' </div></div>'+
               ' </div> </div>';
           sOut += btndiv + '</div></div>';
            return sOut;
        };

        oBSInvAccTable.on('click', 'tbody td', function (e) {
            var selectedivTr = $(this).parents('tr')[0]; var aData = oBSInvAccTable.fnGetData(selectedivTr); var cellindx = $(this).index();
            if (oBSInvAccTable.fnIsOpen(selectedivTr) && (e.target.className == 'glyphicon glyphicon-pencil')) { oBSInvAccTable.fnClose(selectedTr); }
            else {
                var divid = 'dv' + selectedivTr._DT_RowIndex;
                if (e.target.className == 'glyphicon glyphicon-pencil') {
                    if ((previousivTr != '') && (oBSInvAccTable.fnIsOpen(previousivTr) && previousivTr != selectedivTr)) {
                        var prevdivid = 'dv' + previousivTr._DT_RowIndex; oBSInvAccTable.fnClose(previousivTr); $('#' + prevdivid).show();
                    }
                    if (aData != null) {                                        
                        oBSInvAccTable.fnOpen(selectedivTr, fnInvAccFormatDetails(oBSInvAccTable, selectedivTr, 'Edit'), 'details'); $('#' + divid).hide(); previousivTr = selectedivTr;
                    }
                }   
                else if (e.target.className == 'glyphicon glyphicon-trash') {
                    if (confirm('Are you sure ? You want to delete Currency Invoice Account ?')) {
                        if (aData != null) {
                            var _invlinkid = Str(aData[12]); var _curraccid = Str(aData[13]); var _currcode = Str(aData[1]);
                            Delete_Invoice_Account(_curraccid, _currcode, _invlinkid, GetBSInvAccountGrid);
                        }
                    } previousTr = selectedTr;
                }
                $('#btnIAccUpdate').click(function () {
                    var _lstinvdet = []; if (aData != null) {
                        var _invlinkid = Str(aData[12]); var _curraccid = Str(aData[13]);
                        var _isvalid = ValidateInvAccount(_invlinkid, _curraccid);
                        if (_isvalid == true) {
                            GeteInvAccountRowDetails(_lstinvdet, _invlinkid); SaveInvoice_AccountDetails(_lstinvdet, GetBSInvAccountGrid, _invlinkid);
                        }
                    } $('#' + divid).show(); previousivTr = '';
                });
                $('#btnIAccCancel').click(function () { if (oBSInvAccTable.fnIsOpen(selectedivTr)) { oBSInvAccTable.fnClose(selectedivTr); } $('#' + divid).show(); previousivTr = ''; });
            }
        });

        function fnInvAccFormatDetails(oTable, nTr,_target) {
            var sOut = ''; var _str = ''; var _newdet = ''; var _defchecked = ''; var btndiv = ''; var _dvclass = '';
            var _accno = ''; var accname = ''; var _swiftno = ''; var _ibanno = ''; var _kvkno = ''; var _borgno = ''; 
            var _sorgno = ''; var _bankname = ''; var _default = ''; var _pymode = ''; var _curr = '';
            var tid = "BSInvoiceAccTable"; var _tbodyid = "tblBodyBSIAcc"; var _currid = 'cbCurrency'; var _accid = 'txtAccountNo'; var _accnameid = 'txtAccountName';
            var _swftid = 'txtSwiftNo'; var _ibanid = 'txtIbanNo'; var _paymdid = 'cbPaymentMode'; var _bnknameid = 'txtBankName';
            var _kvkid = 'txtkvkNo'; var _borgnoid = 'txtByrOrgNo'; var _sorgnoid = 'txtSppOrgNo'; var _defid = 'chkDefault';
            if (_target == 'Edit') {
                var aData = oTable.fnGetData(nTr); _dvclass = 'col-md-10';
                _accno = Str(aData[3]); accname = Str(aData[2]); _swiftno = Str(aData[4]); _ibanno = Str(aData[5]); _kvkno = Str(aData[6]);
                _borgno = Str(aData[7]); _sorgno = Str(aData[8]); _bankname = Str(aData[9]); _default = Str(aData[11]); _curr = Str(aData[1]);
                var _paydet = Str(aData[10]).split(':'); if (_paydet != null && _paydet.length > 1) { _pymode = Str(_paydet[0]); }else { _pymode = Str(aData[10]); }
            }
            else { _dvclass = 'col-md-12'; }
            if (_default == '1') { _defchecked = 'checked'; } else { _defchecked = ''; }
            FillCurrency(); var _currency = FillCombo(_curr, _lstInvAccCurr); FillPaymentMode(); var _paymode = FillCombo(_pymode, _lstInvAccPay);
            if (_target == 'Edit') { btndiv = '<div class="row"><div style="text-align:center;"><a href="#" id="btnIAccUpdate"><u>Update</u></<a>&nbsp;<a href="#" id="btnIAccCancel"><u>Cancel</u></<a></div></div>'; } else { btndiv = ''; }            
            var sOut = '<div class="row"> <div class="' + _dvclass + '"> ' +
               ' <div class="row"><div class="' + _dvclass + '"><div class="form-group">' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Currency </label> </div>' +
               ' <div  class="col-md-3"><select class="bs-select form-control" id="' + _currid + '">' + _currency + '</select> </div>' +
               ' </div></div></div><div class="row"><div class="' + _dvclass + '"><div class="form-group">' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Account No. </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _accid + '"  value="' + _accno + '"/> </div>' +
                ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Account Name </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _accnameid + '"  value="' + accname + '"/> </div>' +
               ' </div></div></div><div class="row"><div class="' + _dvclass + '"><div class="form-group">' +
                ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Swift\Bic No. </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _swftid + '"  value="' + _swiftno + '"/> </div>' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Iban No. </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _ibanid + '"  value="' + _ibanno + '"/> </div>' +
               ' </div></div></div><div class="row"><div class="' + _dvclass + '"><div class="form-group">' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> KvK No. </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _kvkid + '"  value="' + _kvkno + '"/> </div>' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Org No. </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _borgnoid + '"  value="' + _borgno + '"/> </div>' +
               ' </div></div></div><div class="row"><div class="' + _dvclass + '"><div class="form-group">' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Supplier Org No. </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _sorgnoid + '"  value="' + _sorgno + '"/> </div>' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Bank Name </label> </div>' +
               ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bnknameid + '"  value="' + _bankname + '"/> </div>' +
               ' </div></div></div><div class="row"><div class="' + _dvclass + '"><div class="form-group">' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Payment Mode </label> </div>' +
               ' <div  class="col-md-3"><select class="bs-select form-control" id="' + _paymdid + '">' + _paymode + '</select></div>' +
               ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Default </label> </div>' +
               ' <div  class="col-md-3"><input class="widelarge" type="checkbox" id="' + _defid + '"  value="' + _default + '"' + _defchecked + '/></div>' +
               ' </div></div></div>';
            sOut += btndiv + '</div></div>';
            return sOut;
        };

        $('#btnRefresh').live('click', function (e) {
            e.preventDefault(); var _invlinkid = (Str(sessionStorage['INVLINKID']) != '') ? Str(sessionStorage['INVLINKID']) : '0';
            GetBSInvAccountGrid(_invlinkid);
        });
        $('#btnNew').live('click', function (e) {
            e.preventDefault(); var _dettag = fnInvAccFormatDetails(oBSInvAccTable, '', 'New'); $('#dvInvAcc').empty().append(_dettag); $("#ModalNewInvAcc").modal('show');         
        });
        $('#btnClose').live('click', function (e) { e.preventDefault(); $("#ModalBSInvAcc").modal('hide'); });
        $('#btnNewInvAcc').live('click', function (e) {
            e.preventDefault(); var _lstAccdet = [];var _invlinkid = (Str(sessionStorage['INVLINKID'])!='')?Str(sessionStorage['INVLINKID']):'0';
            var _isvalid = ValidateInvAccount(_invlinkid,0);
            if (_isvalid == true)
            {
                GeteInvAccountRowDetails(_lstAccdet, _invlinkid); SaveInvoice_AccountDetails(_lstAccdet, GetBSInvAccountGrid, _invlinkid); $("#ModalNewInvAcc").modal('hide');
            }
        });
    };

    /* BS Invoice Link*/
    function GetInvoiceDetails(Table, indx, aData, _lstinvdet, name) {
        var _code = ''; var _blinkcd = ''; var _slinkcd = ''; var _bexpfmt = ''; var _simpfmt = ''; var _bcont = ''; var _scont = '';
        var _bemail = ''; var _semail = ''; var _cc = ''; var _bcc = ''; var _acc = ''; var _swft = ''; var _iban = '';
        var _sexppath = ''; var _bntfy = ''; var _sntfy = ''; var _tiff = ''; var _eatt = ''; var _po = ''; var _act = ''; var _currid = '';
        var _accname = ''; var _borgno = ''; var _sorgno = ''; var _bank = ''; var _paymD = ''; var _imppath = ''; var _currcode = '';
        if (aData != null && aData.length > 0) {
            _lstinvdet.push("SUPP_SENDER_CODE" + "|" + Str(aData[7])); _lstinvdet.push("SUPP_RECEIVER_CODE" + "|" + Str(aData[8]));
            _lstinvdet.push("BYR_SENDER_CODE" + "|" + Str(aData[9]));_lstinvdet.push("BYR_RECEIVER_CODE" + "|" + Str(aData[10]));
             _lstinvdet.push("BUYERID" + "|" + Str(aData[29]));  _lstinvdet.push("SUPPLIERID" + "|" + Str(aData[30])); _lstinvdet.push("LINKID" + "|" + Str(aData[31]));
            if (name == 'NewVal') {
                var tid = "BSInvoiceTable";
                _code = $('#txtCode').val(); _blinkcd = $('#txtBLinkCode').val();_slinkcd = $('#txtSLinkCode').val();
                _bexpfmt = $('#cbBuyExpFmt option:selected').text(); _simpfmt = $('#cbSuppImpFmt option:selected').text(); _currid = $('#cbCurrency option:selected').val();
                _bcont = $('#txtBuyContact').val(); _currcode = $('#cbCurrency option:selected').text();
                _scont = $('#txtSuppContact').val(); _bemail = $('#txtBuyEmail').val(); _semail = $('#txtSuppEmail').val(); _cc = $('#txtCCEmail').val();
                _bcc = $('#txtBCCEmail').val(); _acc = $('#txtAccNo').val(); _swft = $('#txtSwiftNo').val(); _imppath = $('#txtImpPath').val();
                _iban = $('#txtIbanNo').val(); _sexppath = $('#txtExpPath').val();  _accname = $('#txtAccName').val(); 
                _borgno = $('#txtByrOrgNo').val();  _sorgno = $('#txtSppOrgNo').val();  _bank = $('#txtBankName').val();  _paymD = $('#cbPayment option:selected').val();
            
                _bntfy = ($('#chkBNotify').is(':checked')) ? 1 : 0;
                _sntfy = ($('#chkSNotify').is(':checked')) ? 1 : 0;
                _tiff = ($('#chkAttachTiff').is(':checked')) ? 1 : 0;
                _eatt = ($('#chkEmbedAttach').is(':checked')) ? 1 : 0;
                _po = ($('#chkPOExist').is(':checked')) ? 1 : 0;
                _act = ($('#chkActive').is(':checked')) ? 1 : 0;
                _lstinvdet.push("SUPPLIER_CONTACT" + "|" + Str(_scont)); _lstinvdet.push("BUYER_CONTACT" + "|" + Str(_bcont));
                _lstinvdet.push("SUPPLIER_EMAIL" + "|" + Str(_semail));
                _lstinvdet.push("BUYER_EMAIL" + "|" + Str(_bemail)); _lstinvdet.push("CC_EMAIL" + "|" + Str(_cc));  _lstinvdet.push("BCC_EMAIL" + "|" + Str(_bcc));
                _lstinvdet.push("CURR_CODE" + "|" + Str(_currcode)); _lstinvdet.push("ACCOUNT_NO" + "|" + Str(_acc)); _lstinvdet.push("SWIFT_NO" + "|" + Str(_swft));
                _lstinvdet.push("IBAN_NO" + "|" + Str(_iban)); _lstinvdet.push("VENDOR_LINK_CODE" + "|" + Str(_slinkcd));
                _lstinvdet.push("BUYER_LINK_CODE" + "|" + Str(_blinkcd)); _lstinvdet.push("CURRENCYID" + "|" + Str(_currid));
                _lstinvdet.push("SUPPLIER_FORMAT_CODE" + "|" + Str(_simpfmt)); _lstinvdet.push("BUYER_FORMAT_CODE" + "|" + Str(_bexpfmt));
                _lstinvdet.push("NOTIFY_SUPPLR" + "|" + Str(_sntfy)); _lstinvdet.push("NOTIFY_BUYER" + "|" + Str(_bntfy));
                _lstinvdet.push("EMBED_ATTACHMENT" + "|" + Str(_eatt));
                _lstinvdet.push("IS_ACTIVE" + "|" + Str(_act)); _lstinvdet.push("PO_EXIST" + "|" + Str(_po)); _lstinvdet.push("CONVERT_TO_TIFF" + "|" + Str(_tiff));
                _lstinvdet.push("IMPORT_PATH" + "|" + Str(_imppath)); _lstinvdet.push("EXPORT_PATH" + "|" + Str(_sexppath));
                _lstinvdet.push("SUPP_ORG_NO" + "|" + Str(_sorgno)); _lstinvdet.push("BYR_ORG_NO" + "|" + Str(_borgno));
                _lstinvdet.push("BANK_NAME" + "|" + Str(_bank)); _lstinvdet.push("PAYMENT_MODE" + "|" + Str(_paymD)); _lstinvdet.push("ACCOUNT_NAME" + "|" + Str(_accname));
            }
            else {
                _lstinvdet.push("BUYER_CONTACT" + "|" + Str(aData[11])); _lstinvdet.push("SUPPLIER_CONTACT" + "|" + Str(aData[12]));
                _lstinvdet.push("SUPPLIER_EMAIL" + "|" + Str(aData[14])); _lstinvdet.push("BUYER_EMAIL" + "|" + Str(aData[13]));
                _lstinvdet.push("CC_EMAIL" + "|" + Str(aData[15])); _lstinvdet.push("BCC_EMAIL" + "|" + Str(aData[16]));  _lstinvdet.push("CURR_CODE" + "|" + Str(aData[20]));
                _lstinvdet.push("ACCOUNT_NO" + "|" + Str(aData[17]));_lstinvdet.push("SWIFT_NO" + "|" + Str(aData[18]));
                _lstinvdet.push("IBAN_NO" + "|" + Str(aData[19]));  _lstinvdet.push("VENDOR_LINK_CODE" + "|" + Str(aData[4]));_lstinvdet.push("BUYER_LINK_CODE" + "|" + Str(aData[3]));
                _lstinvdet.push("SUPPLIER_FORMAT_CODE" + "|" + Str(aData[6])); _lstinvdet.push("BUYER_FORMAT_CODE" + "|" + Str(aData[5]));
                _lstinvdet.push("NOTIFY_SUPPLR" + "|" + Str(aData[24]));  _lstinvdet.push("NOTIFY_BUYER" + "|" + Str(aData[23]));  _lstinvdet.push("EMBED_ATTACHMENT" + "|" + Str(aData[26]));  _lstinvdet.push("IS_ACTIVE" + "|" + Str(aData[28]));
                _lstinvdet.push("PO_EXIST" + "|" + Str(aData[27])); _lstinvdet.push("CONVERT_TO_TIFF" + "|" + Str(aData[25]));
                _lstinvdet.push("IMPORT_PATH" + "|" + Str(aData[21])); _lstinvdet.push("EXPORT_PATH" + "|" + Str(aData[22]));
            }
        }
    };

    function SaveInvoiceDetails(_ofieldval, _nfieldval, LinkID, oBUYER_CODE, SUPPLIER_CODE, callback, _SuppID) {
        var slOlddet = []; var slNewdet = [];
        for (var j = 0; j < _ofieldval.length; j++) { slOlddet.push(_ofieldval[j]);  }
        for (var k = 0; k < _nfieldval.length; k++) { slNewdet.push(_nfieldval[k]); }
        var data2send = JSON.stringify({ LinkID: LinkID, oBUYER_CODE: oBUYER_CODE, SUPPLIER_CODE: SUPPLIER_CODE, slOlddet: slOlddet, slNewdet: slNewdet });
        Metronic.blockUI('#portlet_body');      
        $.ajax({
            type: "POST",
            async: false,
            url: "eInvoice_Links.aspx/SaveInvoiceDetails",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") {
                        toastr.success("Lighthouse eSolutions Pte. Ltd", "Invoice Saved successfully.");
                        GetBSInvoiceGrid(_SuppID);
                    }
                    else { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Invoice details :' + err); }
                    Metronic.unblockUI();
                }
                catch (err) {
                    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Invoice details :' + err); Metronic.unblockUI();
                }
            },
            failure: function (response) {
                toastr.error("failure get", response.d); Metronic.unblockUI();
            },
            error: function (response) {
                toastr.error("error get", response.responseText); Metronic.unblockUI();
            }
        });      
    };

    function FillBSInvoiceGrid(Table) {
        try {
            $('#dataGridBSInv').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridBSInv').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str('');
                    cells[1] = Str(Table[i].BUYER_CODE);
                    cells[2] = Str(Table[i].BUYER_NAME);
                    cells[3] = Str(Table[i].BUYER_LINK_CODE);
                    cells[4] = Str(Table[i].VENDOR_LINK_CODE);               
                    cells[5] = Str(Table[i].BUYER_EXPORT_FORMAT);            
                    cells[6] = Str(Table[i].SUPPLIER_IMPORT_FORMAT);          
                    cells[7] = Str(Table[i].SUPP_SENDER_CODE);         
                    cells[8] = Str(Table[i].SUPP_RECEIVER_CODE);               
                    cells[9] = Str(Table[i].BYR_SENDER_CODE);          
                    cells[10] = Str(Table[i].BYR_RECEIVER_CODE);                 
                    cells[11] = Str(Table[i].BUYER_CONTACT);              
                    cells[12] = Str(Table[i].SUPPLIER_CONTACT);  
                    cells[13] = Str(Table[i].BUYER_EMAIL);               
                    cells[14] = Str(Table[i].SUPPLIER_EMAIL);          
                    cells[15] = Str(Table[i].CC_EMAIL);          
                    cells[16] = Str(Table[i].BCC_EMAIL);       
                    cells[17] = Str(Table[i].ACCOUNT_NO);              
                    cells[18] = Str(Table[i].SWIFT_NO);             
                    cells[19] = Str(Table[i].IBAN_NO);             
                    cells[20] = Str(Table[i].CURR_CODE);               
                    cells[21] = Str(Table[i].IMPORT_PATH);        
                    cells[22] = Str(Table[i].EXPORT_PATH);          
                    cells[23] = Str(Table[i].NOTIFY_BUYER);        
                    cells[24] = Str(Table[i].NOTIFY_SUPPLR);        
                    cells[25] = Str(Table[i].CONVERT_TO_TIFF);        
                    cells[26] = Str(Table[i].EMBED_ATTACHMENT);      
                    cells[27] = Str(Table[i].PO_EXIST);        
                    cells[28] = Str(Table[i].IS_ACTIVE);  
                    cells[29] = Str(Table[i].BUYERID);      
                    cells[30] = Str(Table[i].SUPPLIERID);      
                    cells[31] = Str(Table[i].LINKID);  
                    cells[32] = Str(Table[i].INVLINKID);
                    cells[33] = Str(Table[i].CURRENCYID);
                    cells[34] = Str(Table[i].SUPP_ORG_NO);   
                    cells[35] = Str(Table[i].BYR_ORG_NO);  
                    cells[36] = Str(Table[i].PAYMENT_MODE);    
                    cells[37] = Str(Table[i].BANK_NAME); 
                    cells[38] = Str(Table[i].ACCOUNT_NAME);                       
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.LINKID != undefined && Table.LINKID != null) {
                    var t = $('#dataGridBSInv').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str('');
                    cells[1] = Str(Table.BUYER_CODE);
                    cells[2] = Str(Table.BUYER_NAME);
                    cells[3] = Str(Table.BUYER_LINK_CODE);
                    cells[4] = Str(Table.VENDOR_LINK_CODE);                
                    cells[5] = Str(Table.BUYER_EXPORT_FORMAT);                
                    cells[6] = Str(Table.SUPPLIER_IMPORT_FORMAT);                
                    cells[7] = Str(Table.SUPP_SENDER_CODE);                
                    cells[8] = Str(Table.SUPP_RECEIVER_CODE);                
                    cells[9] = Str(Table.BYR_SENDER_CODE);                
                    cells[10] = Str(Table.BYR_RECEIVER_CODE);                
                    cells[11] = Str(Table.BUYER_CONTACT);                
                    cells[12] = Str(Table.SUPPLIER_CONTACT);                
                    cells[13] = Str(Table.BUYER_EMAIL);                
                    cells[14] = Str(Table.SUPPLIER_EMAIL);                
                    cells[15] = Str(Table.CC_EMAIL);                
                    cells[16] = Str(Table.BCC_EMAIL);                
                    cells[17] = Str(Table.ACCOUNT_NO);                
                    cells[18] = Str(Table.SWIFT_NO);                
                    cells[19] = Str(Table.IBAN_NO);                
                    cells[20] = Str(Table.CURR_CODE);                
                    cells[21] = Str(Table.IMPORT_PATH);          
                    cells[22] = Str(Table.EXPORT_PATH);          
                    cells[23] = Str(Table.NOTIFY_BUYER);          
                    cells[24] = Str(Table.NOTIFY_SUPPLR);          
                    cells[25] = Str(Table.CONVERT_TO_TIFF);       
                    cells[26] = Str(Table.EMBED_ATTACHMENT);       
                    cells[27] = Str(Table.PO_EXIST);          
                    cells[28] = Str(Table.IS_ACTIVE);
                    cells[29] = Str(Table.BUYERID);          
                    cells[30] = Str(Table.SUPPLIERID);          
                    cells[31] = Str(Table.LINKID);
                    cells[32] = Str(Table.INVLINKID);
                    cells[33] = Str(Table.CURRENCYID);
                    cells[34] = Str(Table.SUPP_ORG_NO);
                    cells[35] = Str(Table.BYR_ORG_NO);
                    cells[36] = Str(Table.PAYMENT_MODE);
                    cells[37] = Str(Table.BANK_NAME);
                    cells[38] = Str(Table.ACCOUNT_NAME);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    var GetBSInvoiceGrid = function (SUPPLIERID) {
        var _successs = '';
        Metronic.blockUI('#portlet_body');
       // setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "eInvoice_Links.aspx/FillSuppBuyersGrid",
                data: "{'SUPPLIERID':'" + SUPPLIERID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var DataSet = JSON.parse(response.d);
                        if (DataSet.NewDataSet != null) {
                            var Table = DataSet.NewDataSet.Table1; FillBSInvoiceGrid(Table); _successs = '1';
                        }
                        else { $('#dataGridBSInv').DataTable().clear().draw(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'No Buyer-Supplier Invoice Link'); }
                        Metronic.unblockUI();
                    }
                    catch (err) {
                        Metronic.unblockUI();
                        toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer/Supplier Invoice Details :' + err);
                    }
                },
                failure: function (response) {
                    toastr.error("failure get", response.d); Metronic.unblockUI();
                },
                error: function (response) {
                    toastr.error("error get", response.responseText); Metronic.unblockUI();
                }
            });
      //  }, 200);
        return _successs;
    };

    var setupBSInvTableHeader = function () {
        var dtfilter = '<th></th><th>Code</th><th>Name</th><th>Buyer <br/> Link Code</th><th>Supplier <br/> Link Code</th> <th>Buyer <br/>  Export Format</th> <th>Supplier <br/> Import Format</th><th>Supplier sender code</th><th>Supplier receiver code</th>' +
            ' <th>Buyer sender code</th><th>Buyer receiver code</th><th>Buyer Contact</th><th>Supplier Contact</th> <th>Buyer Email</th><th>Supplier Email</th><th>CC Email</th><th>BCC Email</th>' +
            ' <th>Acc No.</th><th>Swift No</th><th>Iban No</th><th>Currency</th><th>Import Path</th><th>Export Path</th> <th>Notify  <br/> Buyer</th><th>Notify  <br/> Supplier</th><th>Convert <br/>  Attachment  <br/> to Tiff</th><th>Embed  <br/> Attachment  <br/> to file</th> <th>Check PO exist  <br/> on eSupplier</th><th>Active</th>' +
            ' <th>BUYERID</th><th>SUPPLIERID</th><th>LINKID</th>';
        $('#tblHeadRowBSInv').empty(); $('#tblHeadRowBSInv').append(dtfilter); $('#tblBodyBSInv').empty();
    };

    function ValidateInvLink(indx, linkid) {
        var _valid = true;
        var _blnkcode = $('#txtBLinkCode').val(); var _slnkcode = $('#txtSLinkCode').val(); var _bExpPath = $('#txtExpPath').val(); var _bExpFmt = $('#txtBuyExpFmt').val();
        if (_blnkcode == '') { $('#txtBLinkCode').addClass('error'); _valid = false; } else { $('#txtBLinkCode').removeClass('error'); }
        if (_slnkcode == '') { $('#txtSLinkCode').addClass('error'); _valid = false; } else { $('#txtSLinkCode').removeClass('error'); }
        if (_bExpPath == '') { $('#txtExpPath').addClass('error'); _valid = false; } else { $('#txtExpPath').removeClass('error'); }
        if (_bExpFmt == '') { $('#txtBuyExpFmt').addClass('error'); _valid = false; } else { $('#txtBuyExpFmt').removeClass('error'); }
        var isexist = CheckExistingBSLinkCode(linkid, _blnkcode, _slnkcode);
        if (isexist == '') { } else { toastr.error("Lighthouse eSolutions Pte. Ltd.", isexist); _valid = false; }
        return _valid;
    };

    var CheckExistingBSLinkCode = function (LINKID, BUYER_LINKCODE, SUPPLIER_LINKCODE) {
        var res = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "eInvoice_Links.aspx/CheckExistingBSLinkCode",
            data: "{ 'LINKID':'" + LINKID + "','BUYER_LINKCODE':'" + BUYER_LINKCODE + "','SUPPLIER_LINKCODE':'" + SUPPLIER_LINKCODE + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try { res = Str(response.d); }
                catch (err) {
                    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'cannot validate buyer/supplier link codes :' + err);
                }
            },
            failure: function (response) {
                toastr.error("failure get", response.d);
            },
            error: function (response) {
                toastr.error("error get", response.responseText);
            }
        });
        return res;
    };

    /* end */

    /* Invoice Link*/

    function Fill_einvoiceLinkGrid(Table) {
        try {
            $('#dataGrideInvLnk').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGrideInvLnk').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str('');                  
                    cells[1] = Str(Table[i].ADDR_CODE);
                    cells[2] = Str(Table[i].ADDR_NAME);
                    cells[3] = Str(Table[i].CONTACT_PERSON);
                    cells[4] = Str(Table[i].ADDR_EMAIL);                
                    cells[5] = Str(Table[i].ADDRESSID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.ADDRESSID != undefined && Table.ADDRESSID != null) {
                    var t = $('#dataGrideInvLnk').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str('');
                    cells[1] = Str(Table.ADDR_CODE);
                    cells[2] = Str(Table.ADDR_NAME);
                    cells[3] = Str(Table.CONTACT_PERSON);
                    cells[4] = Str(Table.ADDR_EMAIL);                  
                    cells[5] = Str(Table.ADDRESSID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    var Get_einvoiceLinkGrid = function () {
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "eInvoice_Links.aspx/GetSupplierGrid",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) {
                        Table = DataSet.NewDataSet.Table;
                        Fill_einvoiceLinkGrid(Table);
                    }
                    else $('#dataGrideInvLnk').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) {
                    Metronic.unblockUI();
                    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get eInvoice Links :' + err);
                }
            },
            failure: function (response) {
                toastr.error("failure get", response.d); Metronic.unblockUI();
            },
            error: function (response) {
                toastr.error("error get", response.responseText); Metronic.unblockUI();
            }
        });
        }, 200);
    };

    var setupTableHeader = function (_addrtype) {
        $('#toolbtngroup').empty();
        var dthead = ''; if (_addrtype.toUpperCase() == "BUYER") { dthead = '<th style="display:none;">#</th>'; } else { dthead = '<th style="text-align:center;">#</th>'; }
        var dtfilter = dthead + '<th>Code</th><th>Supplier Name</th><th>Contact Person</th><th>Email</th><th>ADDRESSID</th>';
        $('#tblHeadRoweInvLnk').empty(); $('#tblHeadRoweInvLnk').append(dtfilter); $('#tblBodyeInvLnk').empty();
    };

    /*end*/

    /* Invoice Account*/

    function ValidateInvAccount(invLinkid,curraccid) {
        var _valid = true;
        var _currid = $('#cbCurrency option:selected').val(); var _currcode = $('#cbCurrency option:selected').text();
        if (_currid != '' && _currid != undefined) {
            if (parseInt(invLinkid) > 0) {
                var _isexist = CheckExistingInvoiceAccount(invLinkid, curraccid, _currcode);
                if (_isexist != '') { toastr.error('Lighthouse eSolutions Pte. Ltd.', _isexist); _valid = false; }
            }
            else { toastr.error('Lighthouse eSolutions Pte. Ltd.', 'Unable to get Invoice LinkId'); _valid = false; }
        }
        else { toastr.error('Lighthouse eSolutions Pte. Ltd.', 'Unable to get Currency'); _valid = false; }     
        return _valid;
    };

    function GeteInvAccountRowDetails(_lstdet, _invlinkid) {
        var _currid = $('#cbCurrency option:selected').val(); var _currcode = $('#cbCurrency option:selected').text();
        var _paymdid = $('#cbPaymentMode option:selected').val(); var _paymode = $('#cbPaymentMode option:selected').text();
        var _accno = $('#txtAccountNo').val(); var _accname = $('#txtAccountName').val(); var _swiftno = $('#txtSwiftNo').val(); var _ibanno = $('#txtIbanNo').val();
        var _bankname = $('#txtBankName').val(); var _kvkno = $('#txtkvkNo').val(); var _supporgno = $('#txtSppOrgNo').val(); var _byrorgno = $('#txtByrOrgNo').val();
        var _defval = ($('#chkDefault').is(':checked')) ? 1 : 0;
        var _suppliercd = Str(sessionStorage['SUPPLIER_CODE']); var _buyercd = Str(sessionStorage['BUYER_CODE']); 
        _lstdet.push("SUPPLIER_CODE" + "|" + Str(_suppliercd)); _lstdet.push("BUYER_CODE" + "|" + Str(_buyercd)); _lstdet.push("CURRENCYID" + "|" + Str(_currid));
        _lstdet.push("CURR_CODE" + "|" + Str(_currcode)); _lstdet.push("PAYMENT_MODE" + "|" + Str(_paymdid)); _lstdet.push("ACCOUNT_NAME" + "|" + Str(_accname)); _lstdet.push("ACCOUNT_NO" + "|" + Str(_accno));
        _lstdet.push("SWIFT_NO" + "|" + Str(_swiftno)); _lstdet.push("IBAN_NO" + "|" + Str(_ibanno));
        _lstdet.push("KVK_NO" + "|" + Str(_kvkno)); _lstdet.push("BUYER_ORG_NO" + "|" + Str(_byrorgno)); _lstdet.push("SUPPLIER_ORG_NO" + "|" + Str(_supporgno));
        _lstdet.push("BANK_NAME" + "|" + Str(_bankname)); _lstdet.push("SET_DEFAULT" + "|" + Str(_defval)); _lstdet.push("INVLINKID" + "|" + Str(_invlinkid));
    };

    var GetPayment_Mode_Details = function (PAYMENT_CODE) {
        var res = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "eInvoice_Links.aspx/GetPaymentMode_details",
            data: "{ 'PAYMENT_CODE':'" + PAYMENT_CODE + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try { res = Str(response.d); }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'cannot get payment mode details :' + err);}
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText);  }
        });
        return res;
    };

    var CheckExistingInvoiceAccount = function (INVLINKID, CURR_ACCOUNTID, CURR_CODE) {
        var res = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "eInvoice_Links.aspx/CheckInvCurrency_AccountDetails",
            data: "{ 'INVLINKID':'" + INVLINKID + "','CURR_ACCOUNTID':'" + CURR_ACCOUNTID + "','CURR_CODE':'" + CURR_CODE + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try { res = Str(response.d); }
                catch (err) {
                    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'cannot validate Invoice Account :' + err);
                }
            },
            failure: function (response) {
                toastr.error("failure get", response.d);
            },
            error: function (response) {
                toastr.error("error get", response.responseText);
            }
        });
        return res;
    };

    function SaveInvoice_AccountDetails(_nfieldval, callback, INVLINKID) {
        var slInvAccdet = [];     
        for (var k = 0; k < _nfieldval.length; k++) { slInvAccdet.push(_nfieldval[k]); }
        var data2send = JSON.stringify({  slInvAccdet: slInvAccdet });
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "eInvoice_Links.aspx/SaveInvoice_AccountDetails",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "Invoice Account Saved successfully."); callback(INVLINKID); }
                    else { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Invoice Account'); }
                    Metronic.unblockUI();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Invoice details :' + err); Metronic.unblockUI();  }
            },
            failure: function (response) {toastr.error("failure get", response.d); Metronic.unblockUI();  },
            error: function (response) {  toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };

    function Delete_Invoice_Account(CURR_ACCOUNTID, CURR_CODE,INVLINKID, callback) {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "eInvoice_Links.aspx/Delete_Invoice_CurrAccount",
            data: "{'CURR_ACCOUNTID':'" + CURR_ACCOUNTID + "','CURR_CODE':'" + CURR_CODE + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "Invoice Account Deleted."); callback(INVLINKID); }
                    Metronic.unblockUI();
                }
                catch (err) {
                    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete Invoice Account :' + err);
                }
            },
            failure: function (response) {
                toastr.error("failure get", response.d); Metronic.unblockUI();
            },
            error: function (response) {
                toastr.error("error get", response.responseText); Metronic.unblockUI();
            }
        });
    };

    function FillBSInv_AccountGrid(Table) {
        try {
            $('#dataGridBSInvAcc').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridBSInvAcc').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    var _paymode = Str(Table[i].PAYMENT_MODE);
                    cells[0] = Str('');
                    cells[1] = Str(Table[i].CURR_CODE);
                    cells[2] = Str(Table[i].ACCOUNT_NAME);
                    cells[3] = Str(Table[i].ACCOUNT_NO);
                    cells[4] = Str(Table[i].SWIFT_NO);
                    cells[5] = Str(Table[i].IBAN_NO);
                    cells[6] = Str(Table[i].KVK_NO);               
                    cells[7] = Str(Table[i].BUYER_ORG_NO);               
                    cells[8] = Str(Table[i].SUPPLIER_ORG_NO);             
                    cells[9] = Str(Table[i].BANK_NAME);           
                    cells[10] = GetPayment_Mode_Details(_paymode);
                    cells[11] = Str(Table[i].SET_DEFAULT);         
                    cells[12] = Str(Table[i].INVLINKID);
                    cells[13] = Str(Table[i].CURR_ACCOUNTID);                   
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.CURR_ACCOUNTID != undefined && Table.CURR_ACCOUNTID != null) {
                    var t = $('#dataGridBSInvAcc').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    var _paymode = Str(Table.PAYMENT_MODE);
                    cells[0] = Str('');
                    cells[1] = Str(Table.CURR_CODE);
                    cells[2] = Str(Table.ACCOUNT_NAME);
                    cells[3] = Str(Table.ACCOUNT_NO);
                    cells[4] = Str(Table.SWIFT_NO);
                    cells[5] = Str(Table.IBAN_NO);
                    cells[6] = Str(Table.KVK_NO);
                    cells[7] = Str(Table.BUYER_ORG_NO);
                    cells[8] = Str(Table.SUPPLIER_ORG_NO);
                    cells[9] = Str(Table.BANK_NAME);
                    cells[10] = GetPayment_Mode_Details(_paymode);
                    cells[11] = Str(Table.SET_DEFAULT);
                    cells[12] = Str(Table.INVLINKID);
                    cells[13] = Str(Table.CURR_ACCOUNTID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    var GetBSInvAccountGrid = function (INVLINKID) {
        var _successs = '';
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "eInvoice_Links.aspx/GetInvBuyerSupplier_AccountsGrid",
            data: "{'INVLINKID':'" + INVLINKID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) {
                        var Table = DataSet.NewDataSet.Table; FillBSInv_AccountGrid(Table); _successs = '1';
                    }
                    else $('#dataGridBSInvAcc').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) {
                    Metronic.unblockUI();
                    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer/Supplier Invoice Account Details :' + err);
                }
            },
            failure: function (response) {
                toastr.error("failure get", response.d); Metronic.unblockUI();
            },
            error: function (response) {
                toastr.error("error get", response.responseText); Metronic.unblockUI();
            }
        });
        return _successs;
    };
    
    var setupBSInvAccTableHeader = function () {
        var dtfilter = '<th>#</th><th>Currency</th><th>Account Name</th><th>Account <br/> No.</th><th>Swift <br/> Bic No.</th><th>Iban No.</th> <th>KvK No.</th><th>Buyer <br/> Org No.</th><th>Supplier <br/> Org No.</th>' +
            ' <th>Bank Name</th><th>Payment Mode</th><th>Default</th><th>INVLINKID</th><th>CURR_ACCOUNTID</th>';
        $('#tblHeadRowBSInvAcc').empty().append(dtfilter); $('#tblBodyBSInvAcc').empty();
    };

    /*end*/

    var setToolbar = function () {
        var _itmbtns = '<span title="Close" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" ><a href="javascript:;" id="btnClose" class="toolbtn"><i class="fa fa-times" style="text-align:center;"></i></a></div></span>' +          
          '<span title="New" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" id="btnNew" class="toolbtn"><i class="fa fa-plus" style="text-align:center;"></i></a></div></span>' +
          '<span title="Refresh" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnRefresh"><i class="fa fa-recycle" style="text-align:center;"></i></a></div></span>';
        $('#Itemtoolbtngroup').append(_itmbtns);
    };

    function FillCurrency() {
        $.ajax({
            type: "POST",
            async: false,
            url: "eInvoice_Links.aspx/GetCurrency",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var Dataset = JSON.parse(response.d);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table;
                        _lstInvAccCurr = [];
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) {
                                for (var i = 0; i < Table.length; i++) { _lstInvAccCurr.push(Str(Table[i].CURRENCYID) + "|" + Str(Table[i].CURR_CODE)); }
                            }
                            else {
                                if (Table.CURRENCYID != undefined) { _lstInvAccCurr.push(Str(Table.CURRENCYID) + "|" + Str(Table.CURR_CODE)); }
                            }
                        }
                    }
                }
                catch (err) {
                    toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd");
                }
            },
            failure: function (response) { toastr.error("Failure in Currency Combo", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Currency Combo", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    function FillPaymentMode() {
        $.ajax({
            type: "POST",
            async: false,
            url: "eInvoice_Links.aspx/GetPaymentMode",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var Dataset = JSON.parse(response.d);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table;
                        _lstInvAccPay = [];
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) {
                                for (var i = 0; i < Table.length; i++) { _lstInvAccPay.push(Str(Table[i].PAYMENT_CODE) + "|" + Str(Table[i].FULL_NAME)); }
                            }
                            else {
                                if (Table.PAYMENT_CODE != undefined) { _lstInvAccPay.push(Str(Table.PAYMENT_CODE) + "|" + Str(Table.FULL_NAME)); }
                            }
                        }
                    }
                }
                catch (err) {
                    toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd");
                }
            },
            failure: function (response) { toastr.error("Failure in PayMode Combo", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in PayMode Combo", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    var FillInvoiceFormats = function (ADDR_TYPE) {
        var _lstFmt = [];
        $.ajax({
            type: "POST",
            async: false,
            url: "eInvoice_Links.aspx/GetInvoiceFormats",
            data: "{'ADDR_TYPE':'" + ADDR_TYPE + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var Dataset = JSON.parse(response.d);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table;
                        _lstCurrency = [];
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) {
                                for (var i = 0; i < Table.length; i++) { _lstFmt.push(Str(Table[i].INVFORMATID) + "|" + Str(Table[i].INVOICE_FORMAT)); }
                            }
                            else {
                                if (Table.INVFORMATID != undefined) { _lstFmt.push(Str(Table.INVFORMATID) + "|" + Str(Table.INVOICE_FORMAT)); }
                            }
                        }
                    }
                }
                catch (err) {
                    toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd");
                }
            },
            failure: function (response) { toastr.error("Failure in Format Combo", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Format Combo", "Lighthouse eSolutions Pte. Ltd"); }
        });
        return _lstFmt;
    };

    return {
        init: function () { handleeInvoiceLinksTable(); }
    };
}();