var ADDRESSID = '0'; var _ADDR_CODE = ''; var _ADDR_DESC = ''; var _ADDRTYPE = ''; var _DOC_FORMAT = '';
var _lstgrpfrmt = []; var _uplDwlPath = ''; var _configpath = ''; var Params = ''; var _subject = '';
var _clienttabwidth = 0; var _lstlength = 0; var _sldfdet = []; var _scheckid = []; var _lstMasterDet = []; var ID = ''; var _ExistBYRID = '';
var form = $('#submit_form'); var error = $('.alert-danger', form); var success = $('.alert-success', form);
var _tab = ''; var _navg = ''; var _tabindex = ''; var _cpyXMapping = []; var _cpyPMapping = []; var _lstServernames = []; var _serverdomain = '';
var _lstSyncservers = []; var _lstSelectedxlRowindx = []; var _lstSelectedpdfRowindx = []; var oNDRTable = ''; var oBXlsMappTable = ''; var oBPdfMappTable = '';
var _lstSelectedXls_mapp = []; var _lstSelectedPdf_mapp = [];

var ServerSyncWizard = function () {

    var handleServerSyncWizard = function () {
        $('#divSpacer').remove(); $('#divFilterWrapper').remove(); $('.top-menu').hide(); $('#dvBreadCrumb').hide(); Params = getDecryptedData();
        if (Params != '' && Params != undefined) {
            if (Params.length > 0) {  ADDRESSID = Params[0].split('|')[1]; _ADDR_CODE = Params[1].split('|')[1]; _ADDR_DESC = Params[2].split('|')[1]; _DOC_FORMAT = Params[3].split('|')[1]; _ADDRTYPE = Params[4].split('|')[1]; }
        } $('#pageTitle').empty().append('Sync Server Wizard');
        $('#btnclose').addClass('closebtn');
        $('.form-wizard').bootstrapWizard({
            withVisible: true,
            'nextSelector': '.button-next',
            'previousSelector': '.button-previous',
            onInit: function (tab, navigation, index) {               
                $('#spnCode').text(_ADDR_CODE); $('#spnDesc').text(_ADDR_DESC); $('#spnDocFrmt').text(_DOC_FORMAT); SetDefaultRules(_ADDRTYPE, _DOC_FORMAT);
                setupBXLSMappingTableHeader(); setupBPDFMappingTableHeader(); if (_ADDRTYPE.toUpperCase() == 'SUPPLIER') {
                    $('#lstMapp').hide(); $('#lstSync').find('span.number').text('2'); }                
            },
            onTabClick: function (tab, navigation, index, clickedIndex) {
                return false;
            },
            onNext: function (tab, navigation, index) {
                var result = false; var _nxttxt = ' Next <i class="fa fa-arrow-circle-o-right"></i>'; $('#btnNext').html(_nxttxt);
                if (index == 1) { if (_ADDRTYPE.toUpperCase() == 'BUYER') { InitializeMappings(); } else { GetServerDetails(); } result = true; }
                if (index == 2) { GetServerDetails(); result = true; }
                if (result) { handleTab(tab, navigation, index); } return result;
            },
            onPrevious: function (tab, navigation, index) {
                var _nxttxt = ' Next <i class="fa fa-arrow-circle-o-right"></i>'; $('#btnNext').html(_nxttxt); handleTab(tab, navigation, index); return true;
            },
            onTabShow: function (tab, navigation, index) {
                var total = navigation.find('li:visible').length;
                var current = index + 1;
                var $percent = (current / total) * 100;
                $('.form-wizard').find('.progress-bar').css({ width: $percent + '%' });
            }
        });
        $('.form-wizard').find('.button-previous').hide();
        $('.form-wizard .button-submit').click(function (e) {          
                _lstSyncservers = [];
                $('input[type=checkbox]:checked').each(function (e) {
                    var _id = $(this).attr('id'); var _lblvalue = $("label[for='" + $(this).attr('id') + "']").text(); if (_lblvalue != '') { _lstSyncservers.push(_lblvalue); }});        
                if (_lstSyncservers.length == 0) { $('#btnConfrmServ').trigger('click'); } else {  GetServerSyncConfirmation(_lstSyncservers); }     
        });

        $('#dataGridDRule').on('change', 'tr', function (e) {
            var rowIndx = $(this).index();
            if (e.target.id == 'cbRuleValue' + rowIndx) {
                var val = $('#' + e.target.id).val(); var isresult = (val != ""); if (isresult) { $('#chk' + rowIndx).prop('checked', (val.toUpperCase() != 'NOT SET')); }
                else { $('#chk' + rowIndx).prop('checked', false); }
            }
        });

        $('#txtDefFormat').live("change", function (e) {
            var _defval = $('#txtDefFormat').val(); var _dfImppath = _configpath + _defval + "\\INBOX"; var _dfExppath = _configpath + _defval + "\\OUTBOX";
            $('#txtDFImportPath').val(_dfImppath); $('#txtDFExportPath').val(_dfExppath); });
        $('#cbBuyers').live("change", function (e) { _ExistBYRID = $('#cbBuyers option:selected').val(); GetMappings(_ExistBYRID); });
        $('#dataGridBXlsMapp').on('click', 'tbody td', function (e) {
            e.stopPropagation();
            if (e.target.type == "checkbox") {
                var $checkbox = $(this).find(':checkbox'); var _schkd = $checkbox.prop('checked');
                var tr = $(this).closest('tr');
                tr.find('input[type="checkbox"]').not($(this)).prop("checked", false); $checkbox.prop("checked", _schkd);
                if ($checkbox.is(':checked')) { } else { $('#chkHdexcel').prop('checked', $checkbox.is(':checked')); }
            }
            GetSelectedCheckbox('dataGridBXlsMapp');
        });
        $('#dataGridBPdfMapp').on('click', 'tbody td', function (e) {
            e.stopPropagation();
            if (e.target.type == "checkbox") {
                var $checkbox = $(this).find(':checkbox'); var _schkd = $checkbox.prop('checked');  var tr = $(this).closest('tr');
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
                var _isselall = sallchk.prop("checked"); chkBoxes.prop("checked", _isselall); var allPDFPages = oBPdfMappTable.fnGetNodes();
                var $checkbox = $(this).find(':checkbox'); var myCol = $(this).index();
                var chkBoxes1 = $('input[type="checkbox"]:not(:disabled)', allPDFPages); var sallPchk = $('#chkHdpdf');
                var _isPselall = sallPchk.prop("checked"); chkBoxes1.prop("checked", _isPselall);
            }
        });

        function GetSelectedGrid_Details() {
            var _lstdet = [];
            var xmapprows = $('#dataGridBXlsMapp').DataTable().rows().nodes(); _lstSelectedXls_mapp = [];
            for (var l = 0; l < xmapprows.length; l++) {
                var rw2 = xmapprows[l]; var tr2 = $(xmapprows[l]); if ($('input[type="checkbox"]:checked', tr2).length > 0) { var aData = oBXlsMappTable.fnGetData(rw2); if (aData != null) _lstSelectedXls_mapp.push(aData[14]); } //excel mapid
            }
            var pmapprows = $('#dataGridBPdfMapp').DataTable().rows().nodes(); _lstSelectedPdf_mapp = [];
            for (var l = 0; l < pmapprows.length; l++) {
                var rw3 = pmapprows[l]; var tr3 = $(pmapprows[l]); if ($('input[type="checkbox"]:checked', tr3).length > 0) { var aData = oBpdfMappTable.fnGetData(rw3); if (aData != null) _lstSelectedPdf_mapp.push(aData[14]); } //pdf mapid
            }           
        };

        $('#btStartServSync').live('click', function (e) {
            e.preventDefault(); var _isValid = true; var _lstControlsname = []; var _lstservControls = []; _lstControlsname = $("#dvServCnfrmDet input");
            for (var i = 0; i < _lstControlsname.length; i++) {
                var id = '#' + Str(_lstControlsname[i].id); var _value = $(id).val(); var _servname = id.split('_')[1];
                if (id.indexOf("Imp") != -1) { _lstservControls.push('IMPORT_' + _servname + '|' + _value); }
                else if (id.indexOf("Exp") != -1) { _lstservControls.push('EXPORT_' + _servname + '|' + _value); }
                else if (id.indexOf("Upload") != -1) { _lstservControls.push('UPLOAD_' + _servname + '|' + _value); }
                else if (id.indexOf("Download") != -1) { _lstservControls.push('DOWNLOAD_' + _servname + '|' + _value); }
            }
            GetSelectedGrid_Details();
            SyncServer(ADDRESSID, _lstSyncservers, _lstservControls, _lstSelectedXls_mapp, _lstSelectedPdf_mapp);
        });
        $('#btnSyncCancel').live('click', function (e) { e.preventDefault(); $('#ModalServConfrm').modal('hide'); top.window.close(); parent.window.close(); });
        $('#btnClose').removeAttr('href').on('click', function (event) { event.preventDefault(); top.window.close(); });
    };
    var handleTab = function (tab, navigation, index) {
        if (_ADDRTYPE != undefined && _ADDRTYPE != '') { if (_ADDRTYPE.toUpperCase() == 'SUPPLIER') { $('.form-wizard').bootstrapWizard('remove',1); } }
        var total = navigation.find('li').length; var current = index + 1; 
        jQuery('li', $('.form-wizard')).removeClass("done");
        var li_list = navigation.find('li');
        for (var i = 0; i < index; i++) { jQuery(li_list[i]).addClass("done"); }
        if (current == 1) $('.form-wizard').find('.button-previous').hide();
        else if (current == 2 && current <= total) { $('.form-wizard').find('.button-previous').hide(); }
        else $('.form-wizard').find('.button-previous').show();
        if (current >= total) { $('.form-wizard').find('.button-next').hide(); $('.form-wizard').find('.button-previous').hide(); $('.form-wizard').find('.button-submit').show(); }
        else { $('.form-wizard').find('.button-next').show(); $('.form-wizard').find('.button-submit').hide(); }
        Metronic.scrollTo($('.page-title'));
    }

    $('input[type="radio"]').on('click', function () { InitializeMappings();  $('#tblXls').show(); $('#tblPdf').hide(); $('#tblXls').hide();
        if ($(this).attr('id') == 'rdExcel') { $('#tblXls').show(); GetBuyerExcelMapp(); }
        else if ($(this).attr('id') == 'rdPDF') { $('#tblXls').hide(); $('#tblPdf').show(); GetBuyerPDFMapp(); }
    });

    /*Default Rules */
    function InitializeDefaultRules()
    {
        setupRulesTableHeader();
        var table3 = $('#dataGridDRule');
        oNDRTable = table3.dataTable({
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
            "columnDefs": [{'orderable': false, "searching": true, "autoWidth": false, 'targets': [0] },
           { 'targets': [0], width: '2px' }, { 'targets': [1], width: '30px' }, { 'targets': [2], width: '30px' }, { 'targets': [3], width: '50px' }, { 'targets': [4, 5], width: '180px' },
           { 'targets': [6], width: '30px' }, { 'targets': [0,7,8], visible: false }
            ],
            "lengthMenu": [[10, 15, 20, 30, 50, -1], [10, 15, 20, 30, 50, "All"]],
            "scrollY": "250px",
            "scrollX": true,
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); }            
        });
        $('#tblHeadRowDRule').addClass('gridHeader'); $('#ToolTables_dataGridDRule_0,#ToolTables_dataGridDRule_1').hide(); //$('#dataGridNewDRule_paginate').hide();
        $('#dataGridDRule').css('width', '100%');
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
    };

    function SetDefaultRules(_ADDRTYPE) {  InitializeDefaultRules();  var FORMAT = GetFormat(ADDRESSID,_ADDRTYPE); GetRulesGrid(ADDRESSID, FORMAT,_ADDRTYPE); };

    var GetFormat = function (ID,ADDRTYPE) { var _url='';var _data='';
    if (ADDRTYPE.toUpperCase() == 'BUYER') { _url = 'BuyerDetail.aspx/GetFormat_Buyer'; _data = "{'BUYERID':'" + ID+ "' }"; }
    else{ _url = 'SupplierDetail.aspx/GetFormat_Supplier'; _data = "{'SUPPLIERID':'" + ID +"' }"; }
        var _format = '';
        $.ajax({
            type: "POST",
            async: false,
            url: _url,
            data: _data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {  _format = Str(response.d);  }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Format :' + err); }
            },
            failure: function (response) {  toastr.error("failure get", response.d);  },
            error: function (response) {  toastr.error("error get", response.responseText);}
        });
        return _format;
    };

    function FillRulesGrid(Table) {
        try {
            $('#dataGridDRule').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridDRule').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array(); var r = jQuery('<tr id=' + i + '>'); var _arrcommt = Str(Table[i].RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str(''); cells[1] = Str(Table[i].RULE_NUMBER); cells[2] = Str(Table[i].DOC_TYPE); cells[3] = Str(Table[i].RULE_CODE);
                    cells[4] = Str(Table[i].RULE_DESC); cells[5] = _arrcommt; cells[6] = Str(Table[i].RULE_VALUE); cells[7] = Str(Table[i].RULE_ID); cells[8] = Str(Table[i].DEF_ID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.DEF_ID != undefined && Table.DEF_ID != null) {
                    var t = $('#dataGridDRule').dataTable(); var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array(); var _arrcommt = Str(Table.RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str(''); cells[1] = Str(Table.RULE_NUMBER); cells[2] = Str(Table.DOC_TYPE); cells[3] = Str(Table.RULE_CODE);
                    cells[4] = Str(Table.RULE_DESC); cells[5] = _arrcommt; cells[6] = Str(Table.RULE_VALUE); cells[7] = Str(Table.RULE_ID); cells[8] = Str(Table.DEF_ID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e) { }
    };

    var GetRulesGrid = function (ID, GROUP_FORMAT,ADDRTYPE) { var _url='';var _data='';
        if (ADDRTYPE.toUpperCase() == 'BUYER') { _url = 'BuyerDetail.aspx/GetBuyerDefaultRules'; _data = "{'BUYERID':'" + ID + "','GROUP_FORMAT':'" + GROUP_FORMAT + "' }"; }
        else{ _url = 'SupplierDetail.aspx/GetSupplierDefaultRules'; _data = "{'SUPPLIERID':'" + ID + "','GROUP_FORMAT':'" + GROUP_FORMAT + "' }"; }
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: _url,
            data: _data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) {
                        Table = DataSet.NewDataSet.Table; FillRulesGrid(Table); //SetCheckboxState('dataGridDRule');
                    }
                    else {
                        $('#dataGridDRule').DataTable().clear().draw();// $('#chkHdSRule').prop('checked', false);
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
        var dtfilter = '<th></th><th>Rule No.</th><th>Doc Type</th><th>Rule Code</th><th>Description</th><th>Comments</th><th>Rule Value</th><th>RULEID</th><th>DEF_ID</th>';
        $('#tblHeadRowDRule').empty().append(dtfilter); $('#tblBodyDRule').empty();
    };
    /*end*/
 
    /*Mappings*/
    function InitializeMappings() {
        var _xarrNot_visible = [1, 2, 3, 4, 5, 11, 12, 13, 14, 15, 16, 17, 18, 19]; var _parrNot_visible = [1,3, 4, 5, 6, 13, 14, 15, 16, 17, 18];
        if (_ADDRTYPE.toUpperCase() == 'SUPPLIER') {
            _xarrNot_visible = [1, 4, 5, 11, 12, 13, 14, 15, 16, 17, 18, 19]; _parrNot_visible = [1,5, 6, 13, 14, 15, 16, 17, 18]; } else { _xarrNot_visible = [1, 2, 3, 11, 12, 13, 14, 15, 16, 17, 18, 19]; _parrNot_visible = [1,3,4, 13, 14, 15, 16, 17, 18]; }
        var table12 = $('#dataGridBXlsMapp');
        oBXlsMappTable = table12.dataTable({
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
            "columnDefs": [{ "orderable": true, 'targets': [0] },
            { 'targets': [0], width: '10px' }, { 'targets': [1], width: '70px' }, { 'targets': [3,5,7], width: '120px' }, { 'targets': [2,4, 6, 9], width: '30px' }, { 'targets': [8], width: '60px' }, { 'targets': [10], width: '60px' },
            { 'targets': _xarrNot_visible, visible: false }
            ],
            "lengthMenu": [[10, 15, 20, 50, 100, -1], [10, 15, 20, 50, 100, "All"]],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var _chkid = "chk" + nRow._DT_RowIndex;   var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="' + _chkid + '" /></div>';
                $('td:eq(0)', nRow).html(_chkdiv);              
            }
        });
        $('#tblHeadRowBXlsMapp').addClass('gridHeader'); $('#ToolTables_dataGridBXlsMapp_0,#ToolTables_dataGridBXlsMapp_1').hide(); $('#dataGridBXlsMapp').css('width', '100%');
        var table13 = $('#dataGridBPdfMapp');
        oBPdfMappTable = table13.dataTable({
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
            "columnDefs": [{   "orderable": true, 'targets': [0] },
            { 'targets': [0], width: '10px' }, { 'targets': [1], width: '40px' }, { 'targets': [2,3,5], width: '50px' }, { 'targets': [4,6], width: '70px' }, { 'targets': [7, 9, 11], width: '60px' },
            { 'targets': [8, 10, 12], width: '70px' }, { 'targets': _parrNot_visible, visible: false }
            ],
            "scrollX": true,
            "lengthMenu": [[10, 15, 20, 50, 100, -1], [10, 15, 20, 50, 100, "All"]],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var _phdChk = $('#chkHdpdf').prop('checked'); var _hdpcheckedstate = (_phdChk) ? 'checked' : '';
                var _chkid = "chk" + nRow._DT_RowIndex; var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="' + _chkid + '" ' + _hdpcheckedstate + ' /></div>';
                $('td:eq(0)', nRow).html(_chkdiv);
            }
        });
        $('#tblHeadRowBPdfMapp').addClass('gridHeader'); $('#ToolTables_dataGridBPdfMapp_0,#ToolTables_dataGridBPdfMapp_1').hide();   $('#dataGridBPdfMapp').css('width', '100%');
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
        GetMappings(ADDRESSID);
    };
    function GetMappings(ADDRESSID) { GetBuyerExcelMapp(ADDRESSID); GetBuyerPDFMapp(ADDRESSID); };

    var setupBXLSMappingTableHeader = function () {
        $('#tblHeadRowBXlsMapp').empty(); $('#tblBodyBXlsMapp').empty();
        var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="chkHdexcel" checked /></div>';
        var dtfilter = '<th>' + _chkdiv + '</th><th>Format</th><th>Buyer Code</th><th>Buyer Name</th><th>Supplier <br\> Code</th><th>Supplier Name</th><th>Cell-1</th><th>Cell-1 Value-1</th><th>Cell-1 Value-2</th><th>Cell-2</th><th>Cell-2 Value</th><th>Cell (No Discount)</th> <th>Cell Value (No Discount)</th> ' +
        ' <th>XLS_BUYER_MAPID</th><th>EXCEL_MAPID</th><th>BUYER_SUPP_LINKID</th><th>BUYERID</th><th>SUPPLIERID</th> <th>GROUP_ID</th><th>DOC_TYPE</th>';
        $('#tblHeadRowBXlsMapp').append(dtfilter);
    };

    function GetBuyerExcelMapp(ADDRESSID) {
        if (ADDRESSID != undefined) {
            var DataSet = GetMappingXLSGrid(ADDRESSID, 'WIZ'); if (DataSet.NewDataSet != null) { var Table = DataSet.NewDataSet.Table; FillBMappingXLSGrid(Table); SetCheckboxState('dataGridBXlsMapp'); }
            else { $('#chkHdexcel').prop('checked', false); }
        }
    };

    function GetMappingXLSGrid(ADDRESSID, ADDRTYPE) {
        var DataSet = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "XLS_Buyer_Config.aspx/GetXLSBuyerConfig_Addressid",
            data: "{'ADDRESSID':'" + ADDRESSID + "','ADDRTYPE':'" + ADDRTYPE + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try { DataSet = JSON.parse(response.d); }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get XLS BuyerConfig data :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
        return DataSet;
    };

    function FillBMappingXLSGrid(Table) {
        try {
            $('#dataGridBXlsMapp').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridBXlsMapp').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array(); var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str(''); cells[1] = Str(Table[i].GROUP_CODE); cells[2] = Str(Table[i].BUYER_CODE); cells[3] = Str(Table[i].BUYER_NAME);
                    cells[4] = Str(Table[i].SUPPLIER_CODE); cells[5] = Str(Table[i].SUPPLIER_NAME); cells[6] = Str(Table[i].MAP_CELL1);
                    cells[7] = Str(Table[i].MAP_CELL1_VAL1); cells[8] = Str(Table[i].MAP_CELL1_VAL2); cells[9] = Str(Table[i].MAP_CELL2); cells[10] = Str(Table[i].MAP_CELL2_VAL);
                    cells[11] = Str(Table[i].MAP_CELL_NODISC); cells[12] = Str(Table[i].MAP_CELL_NODISC_VAL); cells[13] = Str(Table[i].XLS_BUYER_MAPID);
                    cells[14] = Str(Table[i].EXCEL_MAPID); cells[15] = Str(Table[i].BUYER_SUPP_LINKID); cells[16] = Str(Table[i].BUYERID);
                    cells[17] = Str(Table[i].SUPPLIERID); cells[18] = Str(Table[i].GROUP_ID); cells[19] = Str(Table[i].DOC_TYPE);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.XLS_BUYER_MAPID != undefined && Table.XLS_BUYER_MAPID != null) {
                    var t = $('#dataGridBXlsMapp').dataTable(); var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str(''); cells[1] = Str(Table.GROUP_CODE); cells[2] = Str(Table.BUYER_CODE); cells[3] = Str(Table.BUYER_NAME);
                    cells[4] = Str(Table.SUPPLIER_CODE); cells[5] = Str(Table.SUPPLIER_NAME); cells[6] = Str(Table.MAP_CELL1); cells[7] = Str(Table.MAP_CELL1_VAL1);
                    cells[8] = Str(Table.MAP_CELL1_VAL2); cells[9] = Str(Table.MAP_CELL2); cells[10] = Str(Table.MAP_CELL2_VAL);
                    cells[11] = Str(Table.MAP_CELL_NODISC); cells[12] = Str(Table.MAP_CELL_NODISC_VAL); cells[13] = Str(Table.XLS_BUYER_MAPID);
                    cells[14] = Str(Table.EXCEL_MAPID); cells[15] = Str(Table.BUYER_SUPP_LINKID); cells[16] = Str(Table.BUYERID); cells[17] = Str(Table.SUPPLIERID);
                    cells[18] = Str(Table.GROUP_ID); cells[19] = Str(Table.DOC_TYPE);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e) { }
    };

    var setupBPDFMappingTableHeader = function () {
        $('#tblHeadRowBPdfMapp').empty(); $('#tblBodyBPdfMapp').empty();
        var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="chkHdpdf" checked /></div>';
        var dtfilter = '<th>' + _chkdiv + '</th><th>Format</th><th>Doc Type</th><th>Buyer Code</th><th>Buyer Name</th><th>Supplier <br\> Code</th><th>Supplier Name</th> <th>Map Range (1)</th><th>Map Range (1) <br\> Value</th><th>Map Range (2)</th><th>Map Range (2)<br\> Value</th><th>Map Range (3)</th><th>Map Range (3)<br\> Value</th> ' +
        ' <th>MAP_ID</th><th>PDF_MAPID</th><th>BUYER_SUPP_LINKID</th><th>BUYERID</th><th>SUPPLIERID</th> <th>GROUP_ID</th>';
    $('#tblHeadRowBPdfMapp').append(dtfilter);
    };

    function GetBuyerPDFMapp(ADDRESSID) {
        if (ADDRESSID != undefined) {
            var DataSet = GetMappingPDFGrid(ADDRESSID, _ADDRTYPE); if (DataSet.NewDataSet != null) {
                var Table = DataSet.NewDataSet.Table; FillBMappingPDFGrid(Table); SetCheckboxState('dataGridBPdfMapp');}
            else { $('#chkHdpdf').prop('checked', false); }
        }
    };

    function GetMappingPDFGrid(ADDRESSID, ADDRTYPE) {
        var DataSet = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "PDF_Buyer_Config.aspx/GetPDFBuyerConfig_Addressid",
            data: "{'ADDRESSID':'" + ADDRESSID + "','ADDRTYPE':'" + ADDRTYPE + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try { DataSet = JSON.parse(response.d); }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get PDF BuyerConfig data :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
        return DataSet;
    };

    function FillBMappingPDFGrid(Table) 
    {
        try {
            $('#dataGridBPdfMapp').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridBPdfMapp').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();  var r = jQuery('<tr id=' +i + '>');
                    cells[0]= Str(''); cells[1]= Str(Table[i].GROUP_CODE);  cells[2]= Str(Table[i].DOC_TYPE);  cells[3]= Str(Table[i].BUYER_CODE);
                    cells[4]= Str(Table[i].BUYER_NAME);  cells[5]= Str(Table[i].SUPPLIER_CODE); cells[6]= Str(Table[i].SUPPLIER_NAME);
                    cells[7]= Str(Table[i].MAPPING_1);cells[8]= Str(Table[i].MAPPING_1_VALUE);   cells[9]= Str(Table[i].MAPPING_2); cells[10]= Str(Table[i].MAPPING_2_VALUE);
                    cells[11]= Str(Table[i].MAPPING_3); cells[12]= Str(Table[i].MAPPING_3_VALUE); cells[13]= Str(Table[i].MAP_ID);cells[14]= Str(Table[i].PDF_MAPID);
                    cells[15]= Str(Table[i].BUYER_SUPP_LINKID); cells[16]= Str(Table[i].BUYERID);cells[17]= Str(Table[i].SUPPLIERID);
                    cells[18]= Str(Table[i].GROUP_ID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else 
            {
                if (Table.PDF_MAPID != undefined && Table.PDF_MAPID != null) {
                    var t = $('#dataGridBPdfMapp').dataTable(); var r = jQuery('<tr id=' +1 + '>');
                    var cells = new Array();
                    cells[0]= Str('');  cells[1]= Str(Table.GROUP_CODE); cells[2]= Str(Table.DOC_TYPE);  cells[3]= Str(Table.BUYER_CODE); cells[4]= Str(Table.BUYER_NAME);
                    cells[5]= Str(Table.SUPPLIER_CODE);cells[6]= Str(Table.SUPPLIER_NAME); cells[7]= Str(Table.MAPPING_1); cells[8]= Str(Table.MAPPING_1_VALUE);
                    cells[9]= Str(Table.MAPPING_2); cells[10]= Str(Table.MAPPING_2_VALUE);  cells[11]= Str(Table.MAPPING_3); cells[12]= Str(Table.MAPPING_3_VALUE);
                    cells[13]= Str(Table.MAP_ID); cells[14]= Str(Table.PDF_MAPID);  cells[15]= Str(Table.BUYER_SUPP_LINKID); cells[16]= Str(Table.BUYERID);
                    cells[17]= Str(Table.SUPPLIERID);cells[18]= Str(Table.GROUP_ID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e) {} 
    };

    function SetCheckboxState(grid) {
        if (grid == 'dataGridBXlsMapp') {
            var _ishdrchk = $('#chkHdexcel').prop('checked'); var xlsrows = $('#dataGridBXlsMapp').DataTable().rows().nodes();
            if (_ishdrchk) {   var chkBoxes = $('input[type="checkbox"]:not(:disabled)', xlsrows); chkBoxes.prop("checked", _ishdrchk);}
            else {
                for (var l = 0; l < xlsrows.length; l++) {
                    var tr = $(xlsrows[l]);
                    for (var k = 0; k < _lstSelectedxlRowindx.length; k++) {
                        if (_lstSelectedxlRowindx[k] == l) {   tr.find('input[type="checkbox"]').prop('checked', true);  }
                    }
                }
            }
        }
        else if (grid == 'dataGridBPdfMapp') {
            var _ishdrchk = $('#chkHdpdf').prop('checked'); var pdfrows = $('#dataGridBPdfMapp').DataTable().rows().nodes();
            if (_ishdrchk) { var chkBoxes1 = $('input[type="checkbox"]:not(:disabled)', pdfrows); chkBoxes1.prop("checked", _ishdrchk); }
            else {
                for (var l = 0; l < pdfrows.length; l++) {  var tr1 = $(pdfrows[l]);
                    for (var k = 0; k < _lstSelectedpdfRowindx.length; k++) {  if (_lstSelectedpdfRowindx[k] == l) { tr1.find('input[type="checkbox"]').prop('checked', true);} }
                }
            }
        }
    };

    function GetSelectedCheckbox(grid) {
        if (grid == 'dataGridBXlsMapp') {
            var xlsrows = $('#dataGridBXlsMapp').DataTable().rows().nodes(); _lstSelectedxlRowindx = [];
            for (var l = 0; l < xlsrows.length; l++) {
                var tr = $(xlsrows[l]); if ($('input[type="checkbox"]:checked', tr).length > 0) {
                    var _excelmapid = oBXlsMappTable.fnGetData(tr)[14];
                    _lstSelectedxlRowindx.push(l);
                }
            }
        }
        else if (grid == 'dataGridBPdfMapp') {
            var pdfrows = $('#dataGridBPdfMapp').DataTable().rows().nodes(); _lstSelectedpdfRowindx = [];
            for (var l = 0; l < pdfrows.length; l++) { var tr1 = $(pdfrows[l]); if ($('input[type="checkbox"]:checked', tr1).length > 0) {  _lstSelectedpdfRowindx.push(l);} }
        }
    };
    /*end*/
    /*Sync Servers*/
    function GetServerDetails() { _lstServernames = []; _lstServernames = GetServerNames();
        var _det = ' <div class="row"><div class="col-md-12"> <div class="col-md-1"></div> <div class="col-md-5"><div class="checkbox-grid">';
        if (_lstServernames != [] && _lstServernames != null && _lstServernames != undefined) {
            for (var l = 0; l < _lstServernames.length; l++) {
                if (Str(_lstServernames[l]) != '') {
                    var _chkid = 'chk_' + _lstServernames[l];
                    _det += ' <div><input class="widelarge" type="checkbox" name="servers" id="' + _chkid + '" value="1" checked /><label for="' + _chkid + '" class="chklabel">' + _lstServernames[l] + '</label></div>';
                }
            }
        }
        _det += '</div></div></div></div>'; $('#dvServerList').empty().append(_det);
    };

    function fnNewServerDetails() {
            var sOut = ''; var _code = ''; var _servurl = ''; var tid = "Table"; var _tbodyid = "tblBody"; var _codeid = 'txtServerName';
            var _servurlid = 'txtServiceurl'; var _domainid = 'txtDomain'; var _domainname = _serverdomain;
            var sOut = '<div class="row"> <div class="col-md-12"><div class="row"><div class="col-md-12"><div class="form-group">' +
                  ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Server Name <span class="required" aria-required="true"> * </span> </label> </div>' +
                  ' <div  class="col-md-6"><input type="text" class="form-control" id="' + _codeid + '" value="' + _code + '"/></div>' +
                  ' </div></div></div><div class="row" style="margin-top:3px;"><div class="col-md-12"><div class="form-group">' +
                  ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Domain </label> </div>' +
                  ' <div  class="col-md-9"><input type="text" class="form-control" id="' + _domainid + '" value="' + _domainname + '"/></div>' +
                  ' </div></div></div><div class="row" style="margin-top:3px;"><div class="col-md-12"><div class="form-group">' +
                  ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Service Path </label> </div>' +
                  ' <div  class="col-md-9"><input type="text" class="form-control" id="' + _servurlid + '" value="' + _servurl + '"/></div> </div></div></div> </div></div></div></div></div>';
        return sOut;
        }

    function GetServerSyncConfirmation(_lstselectserv) {
        var _addrpath = GetAddressPath_Details(ADDRESSID, _DOC_FORMAT); 
        var _det = ''; var _downloadpathid = ''; var _downloadpath = _addrpath.split('|')[0]; var _uploadpathid = '';
        var _uploadpath = _addrpath.split('|')[1]; var _impid = ''; var _ImpPath = _addrpath.split('|')[2]; var _expid = ''; var _ExpPath = _addrpath.split('|')[3];
        _ExpPath = (_ExpPath != undefined) ? _ExpPath : ''; _ImpPath = (_ImpPath != undefined) ? _ImpPath : ''; var _frm = 'Import Path'; var _to = 'Export Path';
        var _hddet = '<div class="row"><div class="col-md-12">' +
            '<div style="padding-left:5px;"><h5 class="alert alert-info">'+_ADDRTYPE+' with path details will be synched with the selected servers. Please Confirm would you like to proceed ?</h5></div>' +
            '<div style="overflow:scroll;height:380px;">';
        for (var i = 0; i < _lstselectserv.length; i++) {
            _downloadpathid = 'txtDownload_' + _lstselectserv[i]; _uploadpathid = 'txtUpload_' + _lstselectserv[i]; _expid = 'txtExpPath_' + _lstselectserv[i];
            _impid = 'txtImpPath_' + _lstselectserv[i]; var _adaptorpath = ''; var _servname = (i + 1) + '. ' + _lstselectserv[i];
            _det += '<div class="row"><div class="col-md-12"><div class="col-md-4"><span class="font-blue font-lg"> ' + _servname + '</span> </div></div></div>';
            if (_downloadpath != '' && _uploadpath != '') {
                _adaptorpath = ' <div class="row"><div class="col-md-12"><div class="form-group">' +
                 ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Download Path </label> </div>' +
                 ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _downloadpathid + '" value="' + _downloadpath + '"/> </div>' +
                 ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Upload Path</label> </div>' +
                 ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _uploadpathid + '" value="' + _uploadpath + '"/></div></div></div></div>';
            }
            _det += _adaptorpath + ' <div class="row"><div class="col-md-12"> <div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> ' + _frm + '</label> </div>' +
            ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _impid + '" value="' + _ImpPath + '"/></div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel">' + _to + ' </label> </div>' +
            ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _expid + '" value="' + _ExpPath + '"/></div></div></div></div><hr/>';
        }
        _hddet += _det + '</div></div></div>';
        $('#dvServCnfrmDet').empty().append(_hddet); $('#ModalServConfrm').modal('show');
    };

    function SyncServer(ID, _nfieldval, _nfieldval1, _nfieldval2, _nfieldval3) {
        var slServdet = []; for (var k = 0; k < _nfieldval.length; k++) { slServdet.push(_nfieldval[k]); }
        var slServpaths = []; for (var k = 0; k < _nfieldval1.length; k++) { slServpaths.push(_nfieldval1[k]); }
        var slxlMappId = []; for (var k = 0; k < _nfieldval2.length; k++) { slxlMappId.push(_nfieldval2[k]); }
        var slpdfMappId = []; for (var k = 0; k < _nfieldval3.length; k++) { slpdfMappId.push(_nfieldval3[k]); }
        var data2send = JSON.stringify({ ID: ID, slServdet: slServdet, slServpaths: slServpaths, slxlMappId: slxlMappId, slpdfMappId: slpdfMappId });
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "ServerSyncWizard.aspx/SyncDetails_server",
                data: data2send,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d); if (res == '1') { toastr.success("Lighthouse eSolutions Pte. Ltd.", 'Buyer details synched successfully'); }
                        else { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to Sync Buyer'); } Metronic.unblockUI();
                        top.window.close(); parent.window.close();
                    }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to Sync Buyer :' + err); Metronic.unblockUI(); }
                },
                failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
                error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
            });
        }, 200);
    };

    function GetServerNames() {
        var _lstServer = [];
        $.ajax({
            type: "POST",
            async: false,
            url: "NewBuyerSupplierWizard.aspx/GetServerName_Details",
            data: "{'ID':'" + ID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != '' && res != undefined) { var _servdt = res.split('|'); _serverdomain = _servdt[0]; _lstServer = _servdt[1].split('#')[0].split(','); }
                }
                catch (err) {   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to Get Server names :' + err);}
            },
            failure: function (response) {  toastr.error("failure get", response.d);  },
            error: function (response) { toastr.error("error get", response.responseText);  }
        });
        return _lstServer;
    };

    function GetAddressPath_Details(ADDRESSID, FORMAT) {
        var res = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "ServerSyncWizard.aspx/GetAddressPath_Details",
            data: "{'ADDRESSID':'" + ADDRESSID + "','FORMAT':'" + FORMAT + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {   res = Str(response.d);}
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to Get Address path details :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
        return res;
    };

    /*end*/
    return { init: function () { handleServerSyncWizard(); } };
       
}(); 
