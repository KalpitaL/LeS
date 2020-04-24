var _ADDRTYPE = ''; var _lstgrpfrmt = []; var _uplDwlPath = ''; var _configpath = ''; var Params = ''; var ADDRESSID = '0'; var _subject = '';
var _clienttabwidth = 0; var _lstlength = 0; var _sldfdet = []; var _scheckid = []; var _lstMasterDet = []; var ID = '5001506'; var _ExistBYRID = '';
var form = $('#submit_form'); var error = $('.alert-danger', form); var success = $('.alert-success', form);
var _tab = ''; var _navg = ''; var _tabindex = ''; var _cpyXMapping = []; var _cpyPMapping = []; var _lstServernames = []; var _serverdomain = '';
var _lstSyncservers = []; var _lstSelectedxlRowindx = []; var _lstSelectedpdfRowindx = [];

var BuyerSupplierWizard = function () {
   
    var handleBuyerSupplierWizard = function () {
        $('#divSpacer').remove(); $('#divFilterWrapper').remove(); $('.top-menu').hide(); $('#dvBreadCrumb').hide(); $('#btnClose').hide(); Params = getDecryptedData();
        if (Params != '' && Params != undefined) { if (Params.length > 0) { _ADDRTYPE = Params[0].split('|')[1]; } } $('#pageTitle').empty().append(_ADDRTYPE + ' Wizard');
        if (_ADDRTYPE.toUpperCase() == 'SUPPLIER') { $('#lstcopy').hide(); $('#lstsync').hide(); } SetCommondetails();

        setupRulesTableHeader();
        var table3 = $('#dataGridNewDRule');
        var oNDRTable = table3.dataTable({
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
                "sRowSelect": "multiple",
                "aButtons": ["select_all", "select_none"]
            },
            "columnDefs": [{
                'orderable': false, "searching": true, "autoWidth": false, 'targets': [0],
            },
            { 'targets': [0], width: '5px' }, { 'targets': [1,2], width: '30px' },  { 'targets': [3], width: '80px' },
            { 'targets': [4], width: '160px' }, { 'targets': [5], width: '200px' }, { 'targets': [6], width: '55px' },
            { 'targets': [7], visible: false },
            ],
            "lengthMenu": [
                [10, 25, 50, 100, -1],
                [10, 25, 50, 100, "All"]
            ],
            "scrollX": true,
            "scrollY": "300px",
            "paging": false,
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var _chkid = "chk" + nRow._DT_RowIndex; var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="' + _chkid + '"  /></div>';
                $('td:eq(0)', nRow).html(_chkdiv); var _id = 'cbRuleValue' + nRow._DT_RowIndex; var _defruleval = 'NOT SET';
                var _cbdiv = '<input type="text" class="form-control" id="' + _id + '" value="' + _defruleval + '"/>'; $('td:eq(6)', nRow).html(_cbdiv);
                $('#cbRuleValue').select2();
            }
        });

        $('#tblHeadRowNewDRule').addClass('gridHeader'); $('#ToolTables_dataGridNewDRule_0,#ToolTables_dataGridNewDRule_1').hide(); $('#dataGridNewDRule_paginate').hide();
        $('#dataGridNewDRule').css('width', '100%');

        setupBXLSMappingTableHeader();
        var table12 = $('#dataGridBXlsMapp');
        var oBXlsMappTable = table12.dataTable({
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
            "columnDefs": [{
                "orderable": true, 'targets': [0],
            },
            { 'targets': [0], width: '10px' }, { 'targets': [1], width: '70px' }, { 'targets': [7], width: '120px' }, { 'targets': [4, 6, 9], width: '30px' }, { 'targets': [8], width: '60px' }, { 'targets': [10], width: '60px' },
             { 'targets': [2, 3, 4, 5, 11, 12, 13, 14, 15, 16, 17, 18, 19], visible: false }
            ],
            "lengthMenu": [
                [25, 50, 100, -1],
                [25, 50, 100, "All"]
            ],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {   var _chkid = "chk" + nRow._DT_RowIndex; 
                var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="' + _chkid + '" /></div>';
                $('td:eq(0)', nRow).html(_chkdiv);
            }
        });

        $('#tblHeadRowBXlsMapp').addClass('gridHeader'); $('#ToolTables_dataGridBXlsMapp_0,#ToolTables_dataGridBXlsMapp_1').hide();
        $('#dataGridBXlsMapp').css('width', '100%');

        setupBPDFMappingTableHeader();
        var table13 = $('#dataGridBPdfMapp');
        var oBPdfMappTable = table13.dataTable({
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
            "columnDefs": [{
                "orderable": true, 'targets': [0],
            },
            { 'targets': [0], width: '10px' }, { 'targets': [1], width: '40px' }, { 'targets': [2], width: '50px' }, { 'targets': [5], width: '52px' }, { 'targets': [7, 9, 11], width: '60px' },
             { 'targets': [8, 10, 12], width: '70px' }, { 'targets': [3, 4, 5, 6, 13, 14, 15, 16, 17, 18], visible: false }
            ],
            "scrollX": true,
            "lengthMenu": [
                [25, 50, 100, -1],
                [25, 50, 100, "All"]
            ],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var _phdChk = $('#chkHdpdf').prop('checked'); var _hdpcheckedstate = (_phdChk) ? 'checked' : '';
                var _chkid = "chk" + nRow._DT_RowIndex; var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="' + _chkid + '" ' + _hdpcheckedstate + ' /></div>';
                $('td:eq(0)', nRow).html(_chkdiv);
            }
        });

        $('#tblHeadRowBPdfMapp').addClass('gridHeader'); $('#ToolTables_dataGridBPdfMapp_0,#ToolTables_dataGridBPdfMapp_1').hide();
        $('#dataGridBPdfMapp').css('width', '100%');
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");

        SetDetails(_ADDRTYPE);

        $('.form-wizard').bootstrapWizard({
            'nextSelector': '.button-next',
            'previousSelector': '.button-previous',
            onTabClick: function (tab, navigation, index, clickedIndex) {
                return false;
            },
            onNext: function (tab, navigation, index) {
                var result = false; var _nxttxt = ' Next <i class="fa fa-arrow-circle-o-right"></i>'; $('#btnNext').html(_nxttxt); var _savecontinuetxt = ' Save & Continue <i class="fa fa-arrow-circle-o-right"></i>';             
                if (index == 1) {
                    var _isPartyvalid = false; _isPartyvalid = ValidateParty(0); if (_isPartyvalid) { SetConfigSettings(_ADDRTYPE); result = true; }
                }
                if (index == 2) { if (_ADDRTYPE.toUpperCase() == 'BUYER') { $('#btnNext').html(_savecontinuetxt); } var _isDefsetvalid = ValidateDefaultSettings(); if (_isDefsetvalid) { SetDefaultRules(_ADDRTYPE); result = true; } else { result = false; } }
                if (index == 3) {
                    var _isRulevalid = ValidateDefaultRules(); if (_isRulevalid) { var _buyres = SavePartyDetails(_ADDRTYPE, _scheckid); if (_buyres != '') { result = true; }
                    } else { result = false; }
                }
                if (index == 4) { CopyMappings(); GetServerDetails();result = true; }  
                if (result) { handleTab(tab, navigation, index); } return result;
            },
            onPrevious: function (tab, navigation, index) {
                var _nxttxt = ' Next <i class="fa fa-arrow-circle-o-right"></i>'; $('#btnNext').html(_nxttxt); handleTab(tab, navigation, index); return true;
            },
            onTabShow: function (tab, navigation, index) {
                var total = navigation.find('li').length;
                var current = index + 1;
                var $percent = (current / total) * 100;
                $('.form-wizard').find('.progress-bar').css({ width: $percent + '%' });
            }
        });
        $('.form-wizard').find('.button-previous').hide();
        $('.form-wizard .button-submit').click(function (e) {            
            if (_ADDRTYPE.toUpperCase() == 'BUYER') {
                _lstSyncservers = [];
                $('input[type=checkbox]:checked').each(function (e) {
                    var _id = $(this).attr('id'); var _lblvalue = $("label[for='" + $(this).attr('id') + "']").text(); if (_lblvalue != '') { _lstSyncservers.push(_lblvalue); }
                });
                GetServerSyncConfirmation(_lstSyncservers);
            }
            else {
                $('#btnBack').hide();
                var _isRulevalid = ValidateDefaultRules(); if (_isRulevalid) { SavePartyDetails(_ADDRTYPE, _scheckid); if (ID != '') { result = true; top.window.close(); } } else { result = false; }
            }
        });

        $('#cbDefFormat').live("change", function (e) {
            var _selectdefval = $('#cbDefFormat :selected').val(); var _sldet = GetDefFormatDetail(_selectdefval); var _selecteddeftext = $('#cbDefFormat :selected').text().trim();
            if (_sldet != '') { $('#txtImpPath').val(_sldet.IMPORT_PATH); $('#txtExpPath').val(_sldet.EXPORT_PATH); } $('#spnDefformat').val(_selecteddeftext);
        });

        $('#chkLesConnect').live("change", function (e) {
            var _byrCde = $('#txtCode').val(); var _dwnpath = ''; var _uplpath = ''; var _txtdisabled = 'true';
            if (this.checked) { _dwnpath = _uplDwlPath + _byrCde + "\\INBOX"; _uplpath = _uplDwlPath + _byrCde + "\\OUTBOX"; _txtdisabled = ''; } else { _exppath = ''; _imppath = ''; }
            $('#txtUploadPath').val(_uplpath.trim()); $('#txtDownloadPath').val(_dwnpath.trim());
            $('#txtUploadPath').prop('disabled', _txtdisabled); $('#txtDownloadPath').prop('disabled', _txtdisabled);
            return false;
        });

        oNDRTable.on('change', 'tr', function (e) {
            var rowIndx = $(this).index();
           if (e.target.id == 'cbRuleValue' + rowIndx) {
                var val = $('#' + e.target.id).val(); var isresult = (val != ""); if (isresult) { $('#chk' + rowIndx).prop('checked', (val.toUpperCase() != 'NOT SET')); }
                else { $('#chk' + rowIndx).prop('checked', false);}
            }
        });

        function ValidateDefaultRules() {
            var _isvalid = true; _scheckid = [];
            $('input[type=checkbox]:checked').each(function () {
                var tr = $(this).closest('tr'); if (oNDRTable.fnGetData(tr) != null) {
                    var _id = '#cbRuleValue' + tr[0]._DT_RowIndex;
                    var _ruleval = $(_id).val();
                    var _rulecd = oNDRTable.fnGetData(tr)[3]; var _ruleid = oNDRTable.fnGetData(tr)[7];
                    var _ruledet = tr[0]._DT_RowIndex + '#RULEID|' + +Str(_ruleid) + '#' + 'RULE_CODE|' + Str(_rulecd) + '#' + 'RULE_VALUE|' + Str(_ruleval);
                    if (Str(_ruleval) != '') { if (Str(_ruleval).toUpperCase() != 'NOT SET') { _scheckid.push(Str(_ruledet)); } else { return false; } } else { return false; }
                }
            });
            return _isvalid;
        };

        $('#btnNewFormat').live('click', function (e) { e.preventDefault(); var _hddet = fnDefaultFormatDetails(); $('#dvNewFrmtDet').empty().append(_hddet); $("#ModalNewFormat").modal('show'); });

        $('#btnAddFormat').live('click', function (e) {
            e.preventDefault(); var _lstdeffrmt = []; _lstdeffrmt.push('DEFAULTFORMATID|0');
            _lstdeffrmt.push('ADDR_TYPE|' + _ADDRTYPE); _lstdeffrmt.push('DEFAULT_FORMAT|' + Str($('#txtDefFormat').val()));
            _lstdeffrmt.push('IMPORT_PATH|' + Str($('#txtDFImportPath').val())); _lstdeffrmt.push('EXPORT_PATH|' + Str($('#txtDFExportPath').val()));
            var _isvalid = Validate_NDefFormat(); if (_isvalid) { SaveDefaultFormat(_lstdeffrmt); $("#ModalNewFormat").modal('hide'); }
        });

        $('#txtDefFormat').live("change", function (e) {
            var _defval = $('#txtDefFormat').val(); var _dfImppath = _configpath + _defval + "\\INBOX"; var _dfExppath = _configpath + _defval + "\\OUTBOX";
            $('#txtDFImportPath').val(_dfImppath); $('#txtDFExportPath').val(_dfExppath);
        });

        $('#cbBuyers').live("change", function (e) { _ExistBYRID = $('#cbBuyers :selected').val(); GetMappings(_ExistBYRID); });

        function CopyMappings() {
            var _fmtmapcode = $('#spnDefformat').val() + '_' + $('#txtCode').val();
            var xlsrows = $('#dataGridBXlsMapp').DataTable().rows().nodes();          
            for (var l = 0; l < xlsrows.length; l++) {
                var rw = xlsrows[l]; var tr = $(xlsrows[l]);
                if ($('input[type="checkbox"]:checked', tr).length > 0) {
                    _cpyXMapping = [];
                    var _excelmapid = oBXlsMappTable.fnGetData(tr)[14]; var _xbuyermapid = oBXlsMappTable.fnGetData(tr)[13];
                    var _buyerid = oBXlsMappTable.fnGetData(tr)[16]; var _doctype = oBXlsMappTable.fnGetData(tr)[19]; if (_doctype == '') { _doctype = 'RequestForQuote'; }
                    _cpyXMapping.push('EXCEL_MAPID|' + _excelmapid); _cpyXMapping.push('BUYERID|' + ID); _cpyXMapping.push('EXISTBUYERID|' + _buyerid);
                    _cpyXMapping.push('DOCTYPE|' + _doctype); _cpyXMapping.push('FORMAT_MAPCODE|' + _fmtmapcode); _cpyXMapping.push('XLS_BUYER_MAPID|' + _xbuyermapid);
                    SaveExcelMappings(_cpyXMapping);
                }
            }
            var pdfrows = $('#dataGridBPdfMapp').DataTable().rows().nodes();
            for (var l = 0; l < pdfrows.length; l++) {
                var rw = pdfrows[l]; var tr = $(pdfrows[l]);
                if ($('input[type="checkbox"]:checked', tr).length > 0) {
                    _cpyPMapping = [];
                    var _doctype = oBPdfMappTable.fnGetData(tr)[2]; var _pdfmapid = oBPdfMappTable.fnGetData(tr)[14]; var _buyerid = oBPdfMappTable.fnGetData(tr)[16];
                    _cpyPMapping.push('PDFMAPID|' + _pdfmapid); _cpyPMapping.push('BUYERID|' + ID);
                    _cpyPMapping.push('DOCTYPE|' + _doctype); _cpyPMapping.push('FORMAT_MAPCODE|' + _fmtmapcode);
                    SavePdfMappings(_cpyPMapping);
                }
            }
        };

        oBXlsMappTable.on('click', 'tbody td', function (e) {
            e.stopPropagation();
            if (e.target.type == "checkbox") {
                var $checkbox = $(this).find(':checkbox'); var _schkd = $checkbox.prop('checked');
                var tr = $(this).closest('tr'); 
                tr.find('input[type="checkbox"]').not($(this)).prop("checked", false); $checkbox.prop("checked", _schkd);
                if ($checkbox.is(':checked')) { } else { $('#chkHdexcel').prop('checked', $checkbox.is(':checked')); }          
            }
            GetSelectedCheckbox('dataGridBXlsMapp');
        });

        oBPdfMappTable.on('click', 'tbody td', function (e) {
            e.stopPropagation();
            if (e.target.type == "checkbox") {
                var $checkbox = $(this).find(':checkbox'); var _schkd = $checkbox.prop('checked');
                var tr = $(this).closest('tr');
                tr.find('input[type="checkbox"]').not($(this)).prop("checked", false); $checkbox.prop("checked", _schkd);
                if ($checkbox.is(':checked')) { } else { $('#chkHdpdf').prop('checked', $checkbox.is(':checked')); }
            }
            GetSelectedCheckbox('dataGridBPdfMapp');
        });

        _allselectcount = 0;
        $('th').click(function (e) {
            if (e.target.type == "checkbox") {
                var allXLSPages = oBXlsMappTable.fnGetNodes(); 
                var $checkbox = $(this).find(':checkbox'); var myCol = $(this).index();
                var chkBoxes = $('input[type="checkbox"]:not(:disabled)', allXLSPages); var sallchk = $('#chkHdexcel');
                var _isselall = sallchk.prop("checked"); chkBoxes.prop("checked", _isselall);

                var allPDFPages = oBPdfMappTable.fnGetNodes();
                var $checkbox = $(this).find(':checkbox'); var myCol = $(this).index();
                var chkBoxes1 = $('input[type="checkbox"]:not(:disabled)', allPDFPages); var sallPchk = $('#chkHdpdf');
                var _isPselall = sallPchk.prop("checked"); chkBoxes1.prop("checked", _isPselall);
            }
        });

        $('#btnNewServer').live('click', function (e) { e.preventDefault(); var _hddet = fnNewServerDetails(); $('#dvNewServerDet').empty().append(_hddet); $("#ModalNewServer").modal('show'); });

        $('#btnAddServer').live('click', function (e) {
            e.preventDefault(); var _isValid = true;
            var _lstsevdet = []; var _servname = Str($('#txtServerName').val().trim()); var _serviceurl = Str($('#txtServiceurl').val());
            _lstsevdet.push('ID|0'); _lstsevdet.push('SERVER_NAME|' + _servname); _lstsevdet.push('SERVICE_URL|' +_serviceurl);
            if (_servname == '') { _isValid = false; } if (_servname == '') { _isValid = false; }
            if(_isValid) { SaveServer(_lstsevdet,GetServerDetails); $("#ModalNewServer").modal('hide'); }
        });

        $('#txtDomain').live("change", function (e) { SetServiceUrl();  }); $('#txtServerName').live("change", function (e) { SetServiceUrl(); });

        $('#btnConfrmServ').live('click', function (e) {
            e.preventDefault(); var _isValid = true; var _lstControlsname = []; var _lstservControls = [];  _lstControlsname = $("#dvServCnfrmDet input");
            for (var i = 0; i < _lstControlsname.length; i++) {
                var id = '#' + Str(_lstControlsname[i].id); var _value = $(id).val(); var _servname = id.split('_')[1];
                if (id.indexOf("Imp") != -1) { _lstservControls.push('IMPORT_'+_servname + '|' + _value); }
                else if (id.indexOf("Exp") != -1) { _lstservControls.push('EXPORT_' + _servname + '|' + _value); }
                else if (id.indexOf("Upload") != -1) { _lstservControls.push('UPLOAD_' + _servname + '|' + _value); }
                else if (id.indexOf("Download") != -1) { _lstservControls.push('DOWNLOAD_' + _servname + '|' + _value); }
            }
             SyncBuyer(ID, _lstSyncservers,_lstservControls);
        });

    };

    var handleTab = function (tab, navigation, index) {
        if (_ADDRTYPE != undefined && _ADDRTYPE != '') { if (_ADDRTYPE.toUpperCase() == 'SUPPLIER') { $('.form-wizard').bootstrapWizard('remove', 3); $('.form-wizard').bootstrapWizard('remove', 4); } }
        var total = navigation.find('li').length; var current = index + 1;
        $('.step-title', $('.form-wizard')).text('Step ' + (index + 1) + ' of ' + total);
        jQuery('li', $('.form-wizard')).removeClass("done");
        var li_list = navigation.find('li');
        for (var i = 0; i < index; i++) { jQuery(li_list[i]).addClass("done"); }
        if (current == 1) $('.form-wizard').find('.button-previous').hide();
        else if (current == 4 && current <= total) { $('.form-wizard').find('.button-previous').hide(); }
        else $('.form-wizard').find('.button-previous').show();
        if (current >= total) {  $('.form-wizard').find('.button-next').hide(); $('.form-wizard').find('.button-submit').show(); }
        else { $('.form-wizard').find('.button-next').show(); $('.form-wizard').find('.button-submit').hide(); }
        Metronic.scrollTo($('.page-title'));
    }

    function SetCommondetails() {
        var _format = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "BuyerSupplierWizard.aspx/GetConfigDetails",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    _subject = response.d.split('||')[1];
                }
                catch (err) {
                    toastr.error('Error :' + err, "Lighthouse eSolutions Pte. Ltd");
                }
            },
            failure: function (response) { toastr.error("Failure", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    $('input[type="radio"]').on('click', function () {
        $('#tblXls').show(); $('#tblPdf').hide(); $('#tblXls').hide();
        if ($(this).attr('id') == 'rdExcel') { $('#tblXls').show(); GetBuyerExcelMapp(); }
        else if ($(this).attr('id') == 'rdPDF') { $('#tblXls').hide(); $('#tblPdf').show(); GetBuyerPDFMapp(); }        
    });

    /*Config Settings*/

    function SetConfigSettings(_ADDRTYPE) { var _hddet = fnBConfgSettings(_ADDRTYPE); $('#dvDefSt').empty().append(_hddet); $('#cbDefFormat').select2(); };

    function fnBConfgSettings(_ADDRTYPE) {
        var sOut = ''; var _str = ''; var _cnfgMailsubj = '';
        var _irfqchecked = ''; var _erfqchecked = ''; var _erfqackchecked = ''; var _iqtechecked = ''; var _eqtechecked = ''; var _ipochecked = ''; var _epochecked = '';
        var _epoackchecked = ''; var _epocchecked = ''; var _nbchecked = ''; var _nschecked = ''; var _DefFormat = ''; var _Mailsub = ''; var _ipocchecked = '';
        var _defformatid = 'cbDefFormat'; _addressConfigID = '0'; var _Mapping = ''; var _CCEmail = ''; var _Mailsub = _cnfgMailsubj; var _ImpPath = ''; var _ExpPath = '';
        var _WebserURL = ''; var _DefPrice = '0'; var _UpFileType = ''; var _ImpRFQ = ''; var _ExpRFQAck = ''; var _ExpQuote = ''; var _ImpPO = ''; var _ExpPOAck = ''; var _ExpPOC = ''; var _NBuyer = '';
        var _ExpRFQ = ''; var _ImpQuote = ''; var _ExpPO = ''; var _NSupplier = ''; var _ImpPOC = '';
        var tid = "bConfgTable"; var _tbodyid = "tblBodyBConfg"; var _mapid = 'txtMapping';
        var _ccemailid = 'txtCCEmail'; var _mailsbid = 'txtMailsub'; var _impid = 'txtImpPath'; var _expid = 'txtExpPath';
        var _wurlid = 'txtWebserURL'; var _dpriceid = 'txtDefPrice'; var _byrsndrCd = 'txtBuyerSenderCode';
        var _ufiletyid = 'txtUpFileType'; var _irfqid = 'chkImpRFQ'; var _erfqid = 'chkExpRFQ'; var _erfqackid = 'chkExpRFQAck';
        var _iqteid = 'chkImpQuote'; var _eqteid = 'chkExpQuote'; var _ipoid = 'chkImpPO'; var _epoid = 'chkExpPO'; var _epoackid = 'chkExpPOAck';
        var _epocid = 'chkExpPOC'; var _nbyid = 'chkNBuyer'; var _erfqid = 'chkCExpRFQ'; var _iqteid = 'chkCImpQuote'; var _epoid = 'chkCExpPO'; var _nSpid = 'chkCNSupplier';
        var _defformatid = 'cbDefFormat'; var _ipocid = 'chkCImpPOC';
        FillGroupFormat(_ADDRTYPE); _DefFormat = FillCombo('', _lstgrpfrmt);
        var _frm = (_ADDRTYPE.toUpperCase() == 'BUYER') ? 'Import Path \n (From Buyer)' : 'Import Path \n (From Supplier)'; //_frm = _frm.replace(/\n/g, '<br/>');
        var _to = (_ADDRTYPE.toUpperCase() == 'BUYER') ? 'Export Path \n (To Buyer)' : 'Import Path \n (From Supplier)';// _to = _to.replace(/\n/g, '<br/>');
        var _addformt = '<div><a href="javascript:;" id="btnNewFormat"><i class="fa fa-plus" style="text-align:center;"></i></a></div>';
    
        var sOut = '<div class="row">' +
                ' <div class="col-md-12"><div class="row"><div class="col-md-12"><div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Default Format <span class="required" aria-required="true"> * </span> </label> </div>' +
                ' <div  class="col-md-3"><select class="form-control" id="' + _defformatid + '">' + _DefFormat + '</select> <span id="spnDefformat" style="display:none;"></span></div>' +
                ' <div style="margin-top:5px;"><a href="javascript:;" class="btn btn-circle btn-icon-only" id="btnNewFormat"><i class="fa fa-plus" style="text-align:center;"></i></a></div>' +
                ' </div></div></div><div class="row" style="margin-top:3px;"><div class="col-md-12"><div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> ' + _frm + ' </label> </div>' +
                ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _impid + '" value="' + _ImpPath + '"/></div>' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> ' + _to + ' </label> </div>' +
                ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _expid + '" value="' + _ExpPath + '"/></div>' +
                ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Mapping </label> </div>' +
                ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mapid + '"  value="' + _Mapping + '"/> </div>' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Default Price </label> </div>' +
                ' <div  class="col-md-3"><input type="text" class="form-control alignRight" id="' + _dpriceid + '"  value="' + _DefPrice + '"/> </div>' +
                ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Upload File Type </label> </div>' +
                ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _ufiletyid + '"  value="' + _UpFileType + '"/> </div>' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> CC Email </label> </div>' +
                ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _ccemailid + '"  value="' + _CCEmail + '"/> </div>' +
                ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Mail Subject </label> </div>' +
                ' <div  class="col-md-3"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _mailsbid + '">' + _subject + '</textarea></div>' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Webservice URL </label> </div>' +
                ' <div  class="col-md-3"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _wurlid + '">' + _WebserURL + '</textarea></div>' +
                ' </div></div></div><div class="row"><div class="col-md-12"><div class="col-md-1"></div><div class="col-md-11"><div class="form-group">';
        var _chklst = '';
        if (_ADDRTYPE.toUpperCase() == 'BUYER') {
            _chklst = ' <ul class="checkbox-grid">' +
                    ' <li><input class="widelarge" type="checkbox" id="' + _irfqid + '"  value="' + _ImpRFQ + '"' + _irfqchecked + ' /><label class="chklabel">Import RFQ from Buyer</label></li>' +
                    ' <li><input class="widelarge" type="checkbox" id="' + _erfqackid + '"  value="' + _ExpRFQAck + '"' + _erfqackchecked + '/><label class="chklabel">Export RFQ Ack. to Buyer</label></li>' +
                    ' <li><input class="widelarge" type="checkbox" id="' + _eqteid + '"  value="' + _ExpQuote + '"' + _eqtechecked + '/><label class="chklabel">Export Quote to Buyer</label></li>' +
                    ' <li><input class="widelarge" type="checkbox" id="' + _ipoid + '"  value="' + _ImpPO + '"' + _ipochecked + '/><label class="chklabel">Import PO from Buyer</label></li>' +
                    ' <li> <input class="widelarge" type="checkbox" id="' + _epoackid + '"  value="' + _ExpPOAck + '"' + _epoackchecked + '/> <label class="chklabel">Export PO Ack. to Buyer</label></li>' +
                    ' <li> <input class="widelarge" type="checkbox" id="' + _epocid + '"  value="' + _ExpPOC + '"' + _epocchecked + '/><label class="chklabel">Export POC to Buyer</label></li>' +
                    ' <li> <input class="widelarge" type="checkbox" id="' + _nbyid + '"  value="' + _NBuyer + '"' + _nbchecked + '/> <label class="chklabel">Notify Buyer</label></li>';
        }
        else {
            _chklst = ' <ul class="checkbox-grid">' +
                  ' <li><input class="widelarge" type="checkbox" id="' + _erfqid + '"  value="' + _ExpRFQ + '"' + _erfqchecked + ' /><label class="chklabel">Export RFQ to Supplier</label></li>' +
                  ' <li><input class="widelarge" type="checkbox" id="' + _iqteid + '"  value="' + _ImpQuote + '"' + _iqtechecked + '/><label class="chklabel">Import Quote from Supplier</label></li>' +
                  ' <li> <input class="widelarge" type="checkbox" id="' + _epoid + '"  value="' + _ExpPO + '"' + _epochecked + '/><label class="chklabel">Export PO to Supplier</label></li>' +
                  ' <li><input class="widelarge" type="checkbox" id="' + _ipocid + '"  value="' + _ImpPOC + '"' + _ipocchecked + '/><label class="chklabel">Import POC from Supplier</label></li>' +
                  ' <li> <input class="widelarge" type="checkbox" id="' + _nSpid + '"  value="' + _NSupplier + '"' + _nschecked + '/> <label class="chklabel">Notify Supplier</label></li>';
        }
        sOut = sOut + _chklst + ' </div></div></div></div>';
        return sOut;
    };

    function FillGroupFormat(ADDRTYPE) {
        var _format = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "BuyerSupplierWizard.aspx/GetDefaultFormat",
            data: "{'ADDRTYPE':'"+ADDRTYPE+"'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var Dataset = JSON.parse(response.d);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table;
                        _lstgrpfrmt = [];
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) {
                                for (var i = 0; i < Table.length; i++) {
                                    _lstgrpfrmt.push(Str(Table[i].ID) + "|" + Str(Table[i].FORMAT));
                                }
                            }
                            else {
                                if (Table.FORMAT != undefined) {
                                    _lstgrpfrmt.push(Str(Table.ID) + "|" + Str(Table.FORMAT));
                                }
                            }
                        }
                    }
                }
                catch (err) {
                    toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd");
                }
            },
            failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    var GetDefFormatDetail = function (ID) {
        $.ajax({
            type: "POST",
            async: false,
            url: "BuyerSupplierWizard.aspx/GetDefaultFormatDetails",
            data: "{'ID':'" + ID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {                    
                    _sldfdet = JSON.parse(response.d);
                }
                catch (err) {
                    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer Config :' + err);
                }
            },
            failure: function (response) {
                toastr.error("failure get", response.d);
            },
            error: function (response) {
                toastr.error("error get", response.responseText);
            }
        });
        return _sldfdet;
    };

    function ValidateDefaultSettings() { var _valid = true; var _deffrmt = $('#cbDefFormat:selected').val(); if (_deffrmt == '') { $('#txtImpPath').addClass('error'); $('#txtExpPath').addClass('error'); _valid = false; } return _valid; };

    function fnDefaultFormatDetails() {
        var sOut = ''; var _code = ''; var _ExpPath = ''; var _ImpPath = ''; var tid = "Table"; var _tbodyid = "tblBody"; var _codeid = 'txtDefFormat';
        var _imppathid = 'txtDFImportPath'; var _exppathid = 'txtDFExportPath';
        var sOut = '<div class="row"> <div class="col-md-12"><div class="row"><div class="col-md-12"><div class="form-group">' +
              ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Default Format <span class="required" aria-required="true"> * </span> </label> </div>' +
              ' <div  class="col-md-6"><input type="text" class="form-control" id="' + _codeid + '" value="' + _code + '"/></div>' +
              ' </div></div></div><div class="row" style="margin-top:3px;"><div class="col-md-12"><div class="form-group">' +
              ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Import Path </label> </div>' +
              ' <div  class="col-md-9"><input type="text" class="form-control" id="' + _imppathid + '" value="' + _ImpPath + '"/></div>' +
              ' </div></div></div><div class="row" style="margin-top:3px;"><div class="col-md-12"><div class="form-group">' +
              ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Export Path </label> </div>' +
              ' <div  class="col-md-9"><input type="text" class="form-control" id="' + _exppathid + '" value="' + _ExpPath + '"/></div>' +
              ' </div></div></div></div></div>';      
        return sOut;
    }

    function SaveDefaultFormat(_nfieldval) {
        var slDeformatdet = []; for (var j = 0; j < _nfieldval.length; j++) { slDeformatdet.push(_nfieldval[j]); }      
        var data2send = JSON.stringify({ slDeformatdet: slDeformatdet});
        Metronic.blockUI('#portlet_body');
            $.ajax({
                type: "POST",
                async: false,
                url: "BuyerSupplierWizard.aspx/SaveDefaultFormat",
                data: data2send,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d); RepopluateDefaultFormat(_ADDRTYPE); Metronic.unblockUI();
                    }
                    catch (err) {
                        toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Default Format:' + err); Metronic.unblockUI();
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

    function RepopluateDefaultFormat(ADDRTYPE) {
        var _emptystr = Str('');; $('#txtImpPath').text(_emptystr); $('#txtExpPath').text(_emptystr);
        FillGroupFormat(ADDRTYPE);var _dfmt = FillCombo('', _lstgrpfrmt); $('#cbDefFormat').empty().select2().append(_dfmt);
    };

    function Validate_NDefFormat() {
        var _valid = true; var _fmt = $('#txtDefFormat').val(); var _imppath = $('#txtDFImportPath').val(); var _exppth = $('#txtDFExportPath').val();
        if (_fmt == '') { $('#txtDefFormat').addClass('error'); _valid = false; }
        else {
            var isexist = CheckExistingFormat(_fmt); if (isexist == '') { $('#txtDefFormat').removeClass('error'); } else { $('#txtDefFormat').addClass('error'); toastr.error("Lighthouse eSolutions Pte. Ltd.", isexist); _valid = false; }
            if (_exppth == '') { $('#txtDFExportPath').addClass('error'); _valid = false; } else { $('#txtDFExportPath').removeClass('error'); }
            if (_imppath == '') { $('#txtDFImportPath').addClass('error'); _valid = false; } else { $('#txtDFImportPath').removeClass('error'); }
        }
        return _valid;
    };

    var CheckExistingFormat = function (FORMAT) {
        var res = '';        
        $.ajax({
            type: "POST",
            async: false,
            url: 'BuyerSupplierWizard.aspx/CheckExistingFormat',
            data: "{ 'FORMAT':'" + FORMAT + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    res = Str(response.d);
                }
                catch (err) {
                    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Validation failure' + err);
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

    /*end*/
   
    /* Buyer Header*/
    function SetDetails(_ADDRTYPE) { var _hddet = fnheaderFormatDetails(_ADDRTYPE); $('#dvPrtyDet').empty().append(_hddet); FillExistingBuyers();};

    function fnheaderFormatDetails(ADDRTYPE) {
        var sOut = ''; var _str = ''; var indx = 0;
        var _code = ''; var _buyname = ''; var _contactpers = ''; var _email = ''; var _country = ''; var _weblink = ''; var _downloadpath = '';
        var _city = ''; var _postalcode = ''; var _uploadpath = ''; var _addrid = ''; var _ExpPath = ''; var _ImpPath = ''; var _deffrmt = ''; var _islesconnect = ''; var _lesconid = 'chkLesConnect';
        var tid = "Table"; var _tbodyid = "tblBody";
        var _codeid = 'txtCode'; var _bnameid = 'txtName'; var _contactPersid = 'txtContactPerson'; var _emailid = 'txtEmail';
        var _countryid = 'txtCountry'; var _weblnkid = 'txtWebLink'; var _downloadpathid = 'txtDownloadPath'; var _uploadpathid = 'txtUploadPath';
        var _cityid = 'txtCity'; var _postalid = 'txtPostalCode'; var _txtdisabled = 'disabled';
        _code = GetNextCode(ADDRTYPE);

        var sOut = '<div class="row"><div class="col-md-10"><div class="form-group">' +
              ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Code <span class="required" aria-required="true"> * </span> </label> </div>' +
              ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _codeid + '"  value="' + _code + '"/> </div>' +
              ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label>  Name <span class="required" aria-required="true"> * </span></label> </div>' +
              ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _bnameid + '"  value="' + _buyname + '"/> </div></div></div></div>' +
              ' <div class="row"><div class="col-md-10"><div class="form-group"><div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Contact Person </label> </div>' +
              ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _contactPersid + '"  value="' + _contactpers + '"/> </div>' +
              ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Email </label> </div>' +
              ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _emailid + '"  value="' + _email + '"/> </div>' +
              ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
              ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Country </label> </div>' +
              ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _countryid + '"  value="' + _country + '"/> </div>' +
              ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Web Link </label> </div>' +
              ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _weblnkid + '"  value="' + _weblink + '"/> </div>' +
              ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
              ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> City </label> </div>' +
              ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _cityid + '"  value="' + _city + '"/> </div>' +
              ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Postal Code </label> </div>' +
              ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _postalid + '"  value="' + _postalcode + '"/> </div>' +
              ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
              ' <div class="col-md-2"></div><div class="col-md-4"><input class="widelarge" type="checkbox" id="' + _lesconid + '"  value="' + _islesconnect + ' /> <label class="dvlabel"> LeSConnect</label></div>' +
              ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
              ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Download Path <span class="required" aria-required="true"> * </span> </label> </div>' +
              ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" ' + _txtdisabled + ' rows="4" id="' + _downloadpathid + '">' + _downloadpath + '</textarea> </div>' +
              ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Upload Path <span class="required" aria-required="true"> * </span> </label> </div>' +
              ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" ' + _txtdisabled + ' rows="4" id="' + _uploadpathid + '">' + _uploadpath + '</textarea> </div>' +
              ' </div></div></div>' +
              ' </div></div></div>';
        return sOut;
    }

    var GetNextCode = function (ADDRTYPE) {
        var res = ''; var _url = '';
        if (ADDRTYPE.toUpperCase() == 'BUYER') { _url = "Buyers.aspx/GetNextBuyerNo"; } else { _url = "Suppliers.aspx/GetNextSupplierNo"; }
        $.ajax({
            type: "POST",
            async: false,
            url: _url,
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var _lstres = Str(response.d).split('||'); res = _lstres[0]; _uplDwlPath = _lstres[1].trim(); _configpath = _lstres[2].trim();
                }
                catch (err) {
                    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Next Code :' + err);
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

    function ValidateParty(id) { var _valid = true;
        var _code = $('#txtCode').val(); var _name = $('#txtName').val(); var _downloadpath = $('#txtDownloadPath').val(); var _uploadpath = $('#txtUploadPath').val();
        var _islesConnect = ($('#chkLesConnect').is(':checked')) ? 1 : 0;
        if (_code == '') { $('#txtCode').addClass('error'); _valid = false; }
        else {   var isexist = CheckExisting(_ADDRTYPE,_code, id);
            if (isexist == '') { $('#txtCode').removeClass('error'); } else { $('#txtCode').addClass('error'); toastr.error("Lighthouse eSolutions Pte. Ltd.", isexist); _valid = false; }
        }
        if (_name == '') { $('#txtName').addClass('error'); _valid = false; } else { $('#txtName').removeClass('error'); }
        if (_islesConnect == '1') {
            if (_downloadpath == '') { $('#txtDownloadPath').addClass('error'); _valid = false; } else { $('#txtDownloadPath').removeClass('error'); }
            if (_uploadpath == '') { $('#txtUploadPath').addClass('error'); _valid = false; } else { $('#txtUploadPath').removeClass('error'); }
        }
        return _valid;
    };

    var CheckExisting = function (ADDRTYPE, ADDR_CODE, ID) {
        var res = '';
        var url = ''; var _data = '';
        if (ADDRTYPE.toUpperCase() == 'BUYER') { url = 'Buyers.aspx/CheckExistingBuyer'; _data = "{ 'ADDR_CODE':'" + ADDR_CODE + "','BUYERID':'" + ID + "' }"; }
        else { url = 'Suppliers.aspx/CheckExistingSupplier'; _data = "{ 'ADDR_CODE':'" + ADDR_CODE + "','SUPPLIERID':'" + ID + "'}"; }
        $.ajax({
            type: "POST",
            async: false,
            url: url,
            data: _data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    res = Str(response.d);
                }
                catch (err) {
                    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'cannot validate new :' + ADDRTYPE + err);
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

    function SaveDetails(ADDRTYPE, _nfieldval, _nfieldval1) {
        var slPartydet = []; for (var j = 0; j < _nfieldval.length; j++) { slPartydet.push(_nfieldval[j]); }
        var slDefRuledet = []; for (var k = 0; k < _nfieldval1.length; k++) { slDefRuledet.push(_nfieldval1[k]); }
        var data2send = JSON.stringify({ ADDRTYPE: ADDRTYPE, slPartydet: slPartydet, slDefRuledet: slDefRuledet });
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "BuyerSupplierWizard.aspx/SaveDetails",
                data: data2send,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        ID = Str(response.d); Metronic.unblockUI();
                    }
                    catch (err) {
                        toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update :' + ADDRTYPE + err); Metronic.unblockUI();
                    }
                },
                failure: function (response) {
                    toastr.error("failure get", response.d); Metronic.unblockUI();
                },
                error: function (response) {
                    toastr.error("error get", response.responseText); Metronic.unblockUI();
                }
            });
        }, 20);
        return ID;
    };
    /* end */

    /*Default Rules */

    function SetDefaultRules(_ADDRTYPE) { var GROUP_FORMAT = GetFormat(ADDRESSID); GetRulesGrid(ADDRESSID, GROUP_FORMAT); };

    var GetFormat = function (ID) {
        var _format = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "BuyerDetail.aspx/GetFormat_Buyer",
            data: "{'BUYERID':'" + ID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    _format = Str(response.d);
                }
                catch (err) {
                    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer Format :' + err);
                }
            },
            failure: function (response) {
                toastr.error("failure get", response.d);
            },
            error: function (response) {
                toastr.error("error get", response.responseText);
            }
        });
        return _format;
    };

    function FillRulesGrid(Table) {
        try {
            $('#dataGridNewDRule').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridNewDRule').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    var _arrcommt = Str(Table[i].RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str('');
                    cells[1] = Str(Table[i].RULE_NUMBER);
                    cells[2] = Str(Table[i].DOC_TYPE);
                    cells[3] = Str(Table[i].RULE_CODE);
                    cells[4] = Str(Table[i].RULE_DESC);
                    cells[5] = _arrcommt;
                    cells[6] = Str(Table[i].RULE_VALUE);
                    cells[7] = Str(Table[i].RULEID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.RULEID != undefined && Table.RULEID != null) {
                    var t = $('#dataGridNewDRule').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    var _arrcommt = Str(Table.RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str('');
                    cells[1] = Str(Table.RULE_NUMBER);
                    cells[2] = Str(Table.DOC_TYPE);
                    cells[3] = Str(Table.RULE_CODE);
                    cells[4] = Str(Table.RULE_DESC);
                    cells[5] = _arrcommt;
                    cells[6] = Str(Table.RULE_VALUE);
                    cells[7] = Str(Table.RULEID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e) { }
    };

    var GetRulesGrid = function (ADDRESSID, GROUP_FORMAT) {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultRules.aspx/FillUnLinkedDefaultRules",
            data: "{'ADDRESSID':'" + ADDRESSID + "','GROUP_FORMAT':'" + GROUP_FORMAT + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    if (response.d != '') {
                        var DataSet = JSON.parse(response.d);
                        if (DataSet.NewDataSet != null) {
                            Table = DataSet.NewDataSet.Table; FillRulesGrid(Table);
                        }
                        else $('#dataGridNewDRule').DataTable().clear().draw();
                    }
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Rules :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };

    var setupRulesTableHeader = function () {
        var dtfilter = '<th></th><th>Rule No.</th><th>Doc Type</th><th>Rule Code</th><th>Description</th><th>Comments</th><th>Rule Value</th><th>RULEID</th>';
        $('#tblHeadRowNewDRule').empty(); $('#tblHeadRowNewDRule').append(dtfilter); $('#tblBodyNewDRule').empty();
    };
    /*end*/

    function SavePartyDetails(ADDRTYPE,_scheckid) {
        _lstdet = [];
        var _code = $('#txtCode').val(); var _name = $('#txtName').val(); var _contactPers = $('#txtContactPerson').val(); var _email = $('#txtEmail').val();
        var _country = $('#txtCountry').val(); var _weblnk = $('#txtWebLink').val(); var _downloadpath = $('#txtDownloadPath').val(); var _uploadpath = $('#txtUploadPath').val();
        var _city = $('#txtCity').val(); var _postal = $('#txtPostalCode').val();
        var _imp = $('#txtImpPath').val(); var _exp = $('#txtExpPath').val(); var _map = $('#txtMapping').val(); var _defFormat = $('#cbDefFormat :selected').text(); var _ccemail = $('#txtCCEmail').val();
        var _mailsb = Str($('#txtMailsub').val()).replace(/\#/g, '_'); var _wurl = $('#txtWebserURL').val(); var _dprice = $('#txtDefPrice').val(); var _ufilety = $('#txtUpFileType').val();
        var _irfq = ($('#chkImpRFQ').is(':checked')) ? 1 : 0; var _erfqack = ($('#chkExpRFQAck').is(':checked')) ? 1 : 0;
        var _eqte = ($('#chkExpQuote').is(':checked')) ? 1 : 0; var _ipo = ($('#chkImpPO').is(':checked')) ? 1 : 0;
        var _epoack = ($('#chkExpPOAck').is(':checked')) ? 1 : 0; var _epoc = ($('#chkExpPOC').is(':checked')) ? 1 : 0; var _nby = ($('#chkNBuyer').is(':checked')) ? 1 : 0;
        var _erfq = ($('#chkExpRFQ').is(':checked')) ? 1 : 0; var _iqte = ($('#chkCImpQuote').is(':checked')) ? 1 : 0;
        var _epo = ($('#chkCExpPO').is(':checked')) ? 1 : 0; var _nsp = ($('#chkCNSupplier').is(':checked')) ? 1 : 0; var _ipoc = ($('#chkCImpPOC').is(':checked')) ? 1 : 0;
        _lstdet.push("ID|0"); _lstdet.push("ADDR_CODE" + "|" + Str(_code)); _lstdet.push("ADDR_NAME" + "|" + Str(_name)); _lstdet.push("CONTACT_PERSON" + "|" + Str(_contactPers));
        _lstdet.push("ADDR_EMAIL" + "|" + Str(_email)); _lstdet.push("ADDR_COUNTRY" + "|" + Str(_country)); _lstdet.push("WEBLINK" + "|" + Str(_weblnk));
        _lstdet.push("ADDR_INBOX" + "|" + Str(_downloadpath)); _lstdet.push("ADDR_OUTBOX" + "|" + Str(_uploadpath)); _lstdet.push("ADDR_CITY" + "|" + Str(_city)); _lstdet.push("ADDR_ZIPCODE" + "|" + Str(_postal));      
        _lstdet.push("ADDRESSCONFIGID|0");_lstdet.push("DEFAULT_FORMAT" + "|" + Str(_defFormat)); _lstdet.push("MAPPING" + "|" + Str(_map)); 
        _lstdet.push("IMPORT_PATH" + "|" + Str(_imp)); _lstdet.push("EXPORT_PATH" + "|" + Str(_exp)); _lstdet.push("CC_EMAIL" + "|" + Str(_ccemail)); _lstdet.push("MAIL_SUBJECT" + "|" + Str(_mailsb));
        _lstdet.push("DEFAULT_PRICE" + "|" + Str(_dprice)); _lstdet.push("UPLOAD_FILE_TYPE" + "|" + Str(_ufilety)); _lstdet.push("WEB_SERVICE_URL" + "|" + Str(_wurl));
        if (_ADDRTYPE.toUpperCase() == 'BUYER') {
            _lstdet.push("IMPORT_RFQ" + "|" + Str(_irfq)); _lstdet.push("EXPORT_RFQ_ACK" + "|" + Str(_erfqack)); _lstdet.push("EXPORT_QUOTE" + "|" + Str(_eqte)); _lstdet.push("IMPORT_PO" + "|" + Str(_ipo));
            _lstdet.push("EXPORT_PO_ACK" + "|" + Str(_epoack)); _lstdet.push("EXPORT_POC" + "|" + Str(_epoc)); _lstdet.push("NOTIFY_BUYER" + "|" + Str(_nby));
        }
        else if (_ADDRTYPE.toUpperCase() == 'SUPPLIER') {
            _lstdet.push("EXPORT_RFQ" + "|" + Str(_erfq)); _lstdet.push("IMPORT_QUOTE" + "|" + Str(_iqte)); _lstdet.push("EXPORT_PO" + "|" + Str(_epo));
            _lstdet.push("NOTIFY_SUPPLR" + "|" + Str(_nsp));  _lstdet.push("IMPORT_POC" + "|" + Str(_ipoc));
        }
        return SaveDetails(ADDRTYPE, _lstdet, _scheckid);
    };

    /*Mappings*/

    function GetMappings(_ExistBYRID) { GetBuyerExcelMapp(_ExistBYRID); GetBuyerPDFMapp(_ExistBYRID); };

    function FillExistingBuyers() {
        var _format = ''; var _lstEbyrs = [];
        $.ajax({
            type: "POST",
            async: false,
            url: "Groups.aspx/FillBuyers",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var Dataset = JSON.parse(response.d);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table;
                        if (_lstEbyrs != undefined && Table != null) {
                            if (Table.length != undefined) {
                                for (var i = 0; i < Table.length; i++) {
                                    _lstEbyrs.push(Str(Table[i].ADDRESSID) + "|" + Str(Table[i].ADDR_NAME));
                                }
                            }
                            else {
                                if (Table.FORMAT != undefined) {
                                    _lstEbyrs.push(Str(Table.ADDRESSID) + "|" + Str(Table.ADDR_NAME));
                                }
                            }
                        }
                    }
                    var _byrsdet = FillCombo('', _lstEbyrs); $('#cbBuyers').empty().append(_byrsdet); $('#cbBuyers').select2();
                }
                catch (err) {
                    toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd");
                }
            },
            failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    var setupBXLSMappingTableHeader = function () {
        $('#tblHeadRowBXlsMapp').empty(); $('#tblBodyBXlsMapp').empty();
        var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="chkHdexcel" checked /></div>';
        var dtfilter = '<th>' + _chkdiv + '</th><th>Format</th><th>Buyer Code</th><th>Buyer Name</th><th>Supplier Code</th><th>Supplier Name</th><th>Cell-1</th><th>Cell-1 Value-1</th><th>Cell-1 Value-2</th><th>Cell-2</th><th>Cell-2 Value</th><th>Cell (No Discount)</th> <th>Cell Value (No Discount)</th> ' +
            ' <th>XLS_BUYER_MAPID</th><th>EXCEL_MAPID</th><th>BUYER_SUPP_LINKID</th><th>BUYERID</th><th>SUPPLIERID</th> <th>GROUP_ID</th><th>DOC_TYPE</th>';
        $('#tblHeadRowBXlsMapp').append(dtfilter);
    };

    function GetBuyerExcelMapp(_ExistBYRID) {
        if (_ExistBYRID != undefined) {
            var DataSet = GetMappingXLSGrid(_ExistBYRID, 'WIZ'); if (DataSet.NewDataSet != null) {
                var Table = DataSet.NewDataSet.Table; FillBMappingXLSGrid(Table); SetCheckboxState('dataGridBXlsMapp');
            }
            else { $('#chkHdexcel').prop('checked', false); }
        }
    };

    function FillBMappingXLSGrid(Table) {
        try {
            $('#dataGridBXlsMapp').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridBXlsMapp').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str('');
                    cells[1] = Str(Table[i].GROUP_CODE);
                    cells[2] = Str(Table[i].BUYER_CODE);
                    cells[3] = Str(Table[i].BUYER_NAME);
                    cells[4] = Str(Table[i].SUPPLIER_CODE);
                    cells[5] = Str(Table[i].SUPPLIER_NAME);
                    cells[6] = Str(Table[i].MAP_CELL1);
                    cells[7] = Str(Table[i].MAP_CELL1_VAL1);
                    cells[8] = Str(Table[i].MAP_CELL1_VAL2);
                    cells[9] = Str(Table[i].MAP_CELL2);
                    cells[10] = Str(Table[i].MAP_CELL2_VAL);
                    cells[11] = Str(Table[i].MAP_CELL_NODISC);
                    cells[12] = Str(Table[i].MAP_CELL_NODISC_VAL);
                    cells[13] = Str(Table[i].XLS_BUYER_MAPID);
                    cells[14] = Str(Table[i].EXCEL_MAPID);
                    cells[15] = Str(Table[i].BUYER_SUPP_LINKID);
                    cells[16] = Str(Table[i].BUYERID)
                    cells[17] = Str(Table[i].SUPPLIERID)
                    cells[18] = Str(Table[i].GROUP_ID)
                    cells[19] = Str(Table[i].DOC_TYPE)
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.XLS_BUYER_MAPID != undefined && Table.XLS_BUYER_MAPID != null) {
                    var t = $('#dataGridBXlsMapp').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str('');
                    cells[1] = Str(Table.GROUP_CODE);
                    cells[2] = Str(Table.BUYER_CODE);
                    cells[3] = Str(Table.BUYER_NAME);
                    cells[4] = Str(Table.SUPPLIER_CODE);
                    cells[5] = Str(Table.SUPPLIER_NAME);
                    cells[6] = Str(Table.MAP_CELL1);
                    cells[7] = Str(Table.MAP_CELL1_VAL1);
                    cells[8] = Str(Table.MAP_CELL1_VAL2);
                    cells[9] = Str(Table.MAP_CELL2);
                    cells[10] = Str(Table.MAP_CELL2_VAL);
                    cells[11] = Str(Table.MAP_CELL_NODISC);
                    cells[12] = Str(Table.MAP_CELL_NODISC_VAL);
                    cells[13] = Str(Table.XLS_BUYER_MAPID);
                    cells[14] = Str(Table.EXCEL_MAPID);
                    cells[15] = Str(Table.BUYER_SUPP_LINKID);
                    cells[16] = Str(Table.BUYERID)
                    cells[17] = Str(Table.SUPPLIERID)
                    cells[18] = Str(Table.GROUP_ID)
                    cells[19] = Str(Table.DOC_TYPE)
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e) { }
    }

    var setupBPDFMappingTableHeader = function () {
        $('#tblHeadRowBPdfMapp').empty(); $('#tblBodyBPdfMapp').empty();
        var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="chkHdpdf" checked /></div>';
        var dtfilter = '<th>' + _chkdiv + '</th><th>Format</th><th>Doc Type</th><th>Buyer Code</th><th>Buyer Name</th><th>Supplier Code</th><th>Supplier Name</th> <th>Map Range (1)</th><th>Map Range (1) Value</th><th>Map Range (2)</th><th>Map Range (2) Value</th><th>Map Range (3)</th><th>Map Range (3) Value</th> ' +
            ' <th>MAP_ID</th><th>PDF_MAPID</th><th>BUYER_SUPP_LINKID</th><th>BUYERID</th><th>SUPPLIERID</th> <th>GROUP_ID</th>';
        $('#tblHeadRowBPdfMapp').append(dtfilter);
    };

    function GetBuyerPDFMapp(_ExistBYRID) {
        if (_ExistBYRID != undefined) {
            var DataSet = GetMappingPDFGrid(_ExistBYRID, 'WIZ'); if (DataSet.NewDataSet != null) {
              //  $('#chkHdpdf').prop('checked', true);
                var Table = DataSet.NewDataSet.Table; FillBMappingPDFGrid(Table); SetCheckboxState('dataGridBPdfMapp');
            }
            else { $('#chkHdpdf').prop('checked', false); }
        }
    };

    function FillBMappingPDFGrid(Table) {
        try {
            $('#dataGridBPdfMapp').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridBPdfMapp').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str('');
                    cells[1] = Str(Table[i].GROUP_CODE);
                    cells[2] = Str(Table[i].DOC_TYPE);
                    cells[3] = Str(Table[i].BUYER_CODE);
                    cells[4] = Str(Table[i].BUYER_NAME);
                    cells[5] = Str(Table[i].SUPPLIER_CODE);
                    cells[6] = Str(Table[i].SUPPLIER_NAME);
                    cells[7] = Str(Table[i].MAPPING_1);
                    cells[8] = Str(Table[i].MAPPING_1_VALUE);
                    cells[9] = Str(Table[i].MAPPING_2);
                    cells[10] = Str(Table[i].MAPPING_2_VALUE);
                    cells[11] = Str(Table[i].MAPPING_3);
                    cells[12] = Str(Table[i].MAPPING_3_VALUE);
                    cells[13] = Str(Table[i].MAP_ID);
                    cells[14] = Str(Table[i].PDF_MAPID);
                    cells[15] = Str(Table[i].BUYER_SUPP_LINKID);
                    cells[16] = Str(Table[i].BUYERID)
                    cells[17] = Str(Table[i].SUPPLIERID)
                    cells[18] = Str(Table[i].GROUP_ID)
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.PDF_MAPID != undefined && Table.PDF_MAPID != null) {
                    var t = $('#dataGridBPdfMapp').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str('');
                    cells[1] = Str(Table.GROUP_CODE);
                    cells[2] = Str(Table.DOC_TYPE);
                    cells[3] = Str(Table.BUYER_CODE);
                    cells[4] = Str(Table.BUYER_NAME);
                    cells[5] = Str(Table.SUPPLIER_CODE);
                    cells[6] = Str(Table.SUPPLIER_NAME);
                    cells[7] = Str(Table.MAPPING_1);
                    cells[8] = Str(Table.MAPPING_1_VALUE);
                    cells[9] = Str(Table.MAPPING_2);
                    cells[10] = Str(Table.MAPPING_2_VALUE);
                    cells[11] = Str(Table.MAPPING_3);
                    cells[12] = Str(Table.MAPPING_3_VALUE);
                    cells[13] = Str(Table.MAP_ID);
                    cells[14] = Str(Table.PDF_MAPID);
                    cells[15] = Str(Table.BUYER_SUPP_LINKID);
                    cells[16] = Str(Table.BUYERID)
                    cells[17] = Str(Table.SUPPLIERID)
                    cells[18] = Str(Table.GROUP_ID)
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e) { }
    };

    function SaveExcelMappings(_nfieldval) {
        var slXMappingdet = []; for (var k = 0; k < _nfieldval.length; k++) { slXMappingdet.push(_nfieldval[k]); }
        var data2send = JSON.stringify({ slXMappingdet: slXMappingdet });
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "BuyerSupplierWizard.aspx/SaveExcelMappings",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d); Metronic.unblockUI();
                }
                catch (err) {
                    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to copy mapping :' + err); Metronic.unblockUI();
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

    function SavePdfMappings(_nfieldval) {
        var slPMappingdet = []; for (var k = 0; k < _nfieldval.length; k++) { slPMappingdet.push(_nfieldval[k]); }
        var data2send = JSON.stringify({ slPMappingdet: slPMappingdet });
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "BuyerSupplierWizard.aspx/SavePDFMappings",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d); Metronic.unblockUI();
                }
                catch (err) {
                    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to copy mapping :' + err); Metronic.unblockUI();
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


    function SetCheckboxState(grid) {
        if (grid == 'dataGridBXlsMapp') {
            var _ishdrchk = $('#chkHdexcel').prop('checked'); var xlsrows = $('#dataGridBXlsMapp').DataTable().rows().nodes();
            if (_ishdrchk) {
                var chkBoxes = $('input[type="checkbox"]:not(:disabled)', xlsrows); chkBoxes.prop("checked", _ishdrchk);
            }
            else {
                for (var l = 0; l < xlsrows.length; l++) {
                    var tr = $(xlsrows[l]);
                    for (var k = 0; k < _lstSelectedxlRowindx.length; k++) {
                        if (_lstSelectedxlRowindx[k] == l) { tr.find('input[type="checkbox"]').prop('checked', true); }
                    }
                }
            }
        }
        else if (grid == 'dataGridBPdfMapp') {
            var _ishdrchk = $('#chkHdpdf').prop('checked'); var pdfrows = $('#dataGridBPdfMapp').DataTable().rows().nodes();
            if (_ishdrchk) {
                var chkBoxes1 = $('input[type="checkbox"]:not(:disabled)', pdfrows); chkBoxes1.prop("checked", _ishdrchk);
            }
            else {
                for (var l = 0; l < pdfrows.length; l++) {
                    var tr1 = $(pdfrows[l]);
                    for (var k = 0; k < _lstSelectedpdfRowindx.length; k++) {
                        if (_lstSelectedpdfRowindx[k] == l) { tr1.find('input[type="checkbox"]').prop('checked', true); }
                    }
                }
            }
        }
    };

    function GetSelectedCheckbox(grid) {
        if (grid == 'dataGridBXlsMapp') {
            var xlsrows = $('#dataGridBXlsMapp').DataTable().rows().nodes(); _lstSelectedxlRowindx = [];
            for (var l = 0; l < xlsrows.length; l++) {
                var tr = $(xlsrows[l]); if ($('input[type="checkbox"]:checked', tr).length > 0) { _lstSelectedxlRowindx.push(l); }
            }
        }
        else if (grid == 'dataGridBPdfMapp') {
            var pdfrows = $('#dataGridBPdfMapp').DataTable().rows().nodes(); _lstSelectedpdfRowindx = [];
            for (var l = 0; l < pdfrows.length; l++) {
                var tr1 = $(pdfrows[l]); if ($('input[type="checkbox"]:checked', tr1).length > 0) { _lstSelectedpdfRowindx.push(l); }
            }
        }
    };
    /*end*/

    /*Sync Servers*/
    function GetServerDetails() {
        _lstServernames = [];_lstServernames = GetServerNames();
        var _det =' <div class="row"><div class="col-md-12">' +
              ' <div class="col-md-1"></div>' +
              ' <div class="col-md-5">' +
              ' <div class="checkbox-grid">';
        if (_lstServernames != null && _lstServernames != undefined) {
            for (var l = 0; l < _lstServernames.length; l++) {
                var _chkid = 'chk_' + _lstServernames[l];
                _det += ' <div><input class="widelarge" type="checkbox" name="servers" id="' + _chkid + '" value="1" checked /><label for="' + _chkid + '" class="chklabel">' + _lstServernames[l] + '</label></div>';
            }
        }
        _det += '</div></div></div></div>';
        $('#dvServerList').empty().append(_det);
    };

    function fnNewServerDetails() {
        var sOut = ''; var _code = '';  var _servurl = ''; var tid = "Table"; var _tbodyid = "tblBody"; var _codeid = 'txtServerName';
        var _servurlid = 'txtServiceurl'; var _domainid = 'txtDomain'; var _domainname = _serverdomain;
        var sOut = '<div class="row"> <div class="col-md-12"><div class="row"><div class="col-md-12"><div class="form-group">' +
              ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Server Name <span class="required" aria-required="true"> * </span> </label> </div>' +
              ' <div  class="col-md-6"><input type="text" class="form-control" id="' + _codeid + '" value="' + _code + '"/></div>' +
              ' </div></div></div><div class="row" style="margin-top:3px;"><div class="col-md-12"><div class="form-group">' +
              ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Domain </label> </div>' +
              ' <div  class="col-md-9"><input type="text" class="form-control" id="' + _domainid + '" value="' + _domainname + '"/></div>' +
              ' </div></div></div><div class="row" style="margin-top:3px;"><div class="col-md-12"><div class="form-group">' +
              ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Service Path </label> </div>' +
              ' <div  class="col-md-9"><input type="text" class="form-control" id="' + _servurlid + '" value="' + _servurl + '"/></div>' +
              ' </div></div></div>'+
              ' </div></div></div></div></div>';
        return sOut;
    }

    function GetServerSyncConfirmation(_lstselectserv) {
        var _det = '';
        var _downloadpathid = ''; var _downloadpath = $('#txtDownloadPath').val(); var _uploadpathid ='';
        var _uploadpath = $('#txtUploadPath').val(); var _impid = ''; var _ImpPath = $('#txtImpPath').val(); var _expid = ''; var _ExpPath = $('#txtExpPath').val();
        _ExpPath = (_ExpPath != undefined) ? _ExpPath : ''; _ImpPath = (_ImpPath != undefined) ? _ImpPath : '';
        var _frm = 'Import Path'; var _to =  'Export Path'; 
        var _hddet = '<div class="row"><div class="col-md-12">' +
            '<div style="padding-left:5px;"><h5 class="alert alert-info">Buyer with the path details will be synched with the selected servers. Please Confirm would you like to proceed ?</h5></div>' +
            '<div style="overflow:scroll;height:450px;">';

        for (var i = 0; i<_lstselectserv.length;i++)
        {
            _downloadpathid = 'txtDownload_' + _lstselectserv[i]; _uploadpathid = 'txtUpload_' + _lstselectserv[i]; _expid = 'txtExpPath_' + _lstselectserv[i];
            _impid = 'txtImpPath_' + _lstselectserv[i]; var _adaptorpath = '';
            var _servname = (i+1) +'. '+ _lstselectserv[i];
            _det += '<div class="row"><div class="col-md-12">' +
                          '<div class="col-md-4"><span class="font-blue font-lg"> ' + _servname + '</span> </div>' +
                      '</div></div>';
            if (_downloadpath != '' && _uploadpath != '') {
                _adaptorpath = ' <div class="row"><div class="col-md-12">' +
                 ' <div class="form-group">' +
                 ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Download Path </label> </div>' +
                 ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _downloadpathid + '" value="' + _downloadpath + '"/> </div>' +
                 ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Upload Path</label> </div>' +
                 ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _uploadpathid + '" value="' + _uploadpath + '"/></div>' +
                       ' </div>' +
                       '</div></div>';
            }
            _det += _adaptorpath+' <div class="row"><div class="col-md-12">' +
            ' <div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> ' + _frm + '</label> </div>' +
            ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _impid + '" value="' + _ImpPath + '"/></div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel">' + _to + ' </label> </div>' +
            ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _expid + '" value="' + _ExpPath + '"/></div>' +
                  ' </div>' +
                  '</div></div><hr/>';
        }
        _hddet +=_det+ '</div></div></div>';
        $('#dvServCnfrmDet').empty().append(_hddet); $('#ModalServConfrm').modal('show');
    };

    function SyncBuyer(ID, _nfieldval,_nfieldval1) {
        var slServdet = []; for (var k = 0; k < _nfieldval.length; k++) { slServdet.push(_nfieldval[k]); }
        var slServpaths = []; for (var k = 0; k < _nfieldval1.length; k++) { slServpaths.push(_nfieldval1[k]); }
        var data2send = JSON.stringify({ ID: ID, slServdet: slServdet, slServpaths: slServpaths });
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "BuyerSupplierWizard.aspx/SyncDetails_server",
                data: data2send,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d); if (res == '1') {
                            toastr.success("Lighthouse eSolutions Pte. Ltd.", 'Buyer details synched successfully'); top.window.close(); parent.window.close();
                        } else { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to Sync Buyer'); }
                        Metronic.unblockUI();
                    }
                    catch (err) {
                        toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to Sync Buyer :' + err); Metronic.unblockUI();
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

    function GetServerNames() {
        var _lstServer = [];
        $.ajax({
            type: "POST",
            async: false,
            url: "BuyerSupplierWizard.aspx/GetServerName_Details",
            data: "{'ID':'" + ID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != '' && res != undefined) { var _servdt = res.split('|');
                        _serverdomain = _servdt[0]; _lstServer = _servdt[1].split('#')[0].split(',');
                    }
                }
                catch (err) {
                    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to Get Server names :' + err); 
                }
            },
            failure: function (response) {
                toastr.error("failure get", response.d);
            },
            error: function (response) {
                toastr.error("error get", response.responseText);
            }
        });
        return _lstServer;
    };

    function SaveServer(_nfieldval,callback) {
        var slServ = []; for (var k = 0; k < _nfieldval.length; k++) { slServ.push(_nfieldval[k]); }
        var data2send = JSON.stringify({ slServ: slServ });
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "BuyerSupplierWizard.aspx/SaveServerDetails",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d); GetServerDetails();  Metronic.unblockUI();
                }
                catch (err) {
                    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to add new server :' + err); Metronic.unblockUI();
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

    function SetServiceUrl() {
        var _domainval = $('#txtDomain').val().trim(); if (_domainval == '') { _domainval = _serverdomain;}
        var _servname = $('#txtServerName').val().trim(); var _servicepath = 'http://' + _domainval + "/LeSbuyerService_" + _servname;
        $('#txtServiceurl').val(_servicepath.trim());
    };
    /*end*/

    return { init: function () { handleBuyerSupplierWizard(); } };
       
}(); 
