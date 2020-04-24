var _BUYERID = ''; var Params = ''; var _SUPPLIERID = '0'; var _lstSelectedxlRowindx = []; var _lstSelectedpdfRowindx = []; var _lstSelectedBRRowindx = []; var _lstSelectedSRRowindx = [];
var _lstbryrule = []; var _lstspprule = []; var _lstXmapp = []; var _lstPmapp = [];

var BuyerSupplier_ConfirmLink_Wizard = function () {
   
    var handleBuyerSupplier_ConfirmLink_Wizard = function () {
        $('#divSpacer').remove(); $('#divFilterWrapper').remove(); $('.top-menu').hide(); $('#dvBreadCrumb').hide(); Params = getDecryptedData();
        if (Params != '' && Params != undefined) { if (Params.length > 0) { _SUPPLIERID = Params[0].split('|')[1]; } } $('#pageTitle').empty().append('Buyer-Supplier Link'); SetCommondetails();
        setupBuyerTableHeader(); $('#btnclose').addClass('closebtn');
        var table11 = $('#dataGridNewBuy');
        var oBuyerTable = table11.dataTable({
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
            tableTools: { "sRowSelect": "single", "aButtons": ["select_all", "select_none"]
            },
            "columnDefs": [{   'orderable': false, "searching": true, "autoWidth": false, 'targets': [0] },
             { 'targets': [0], width: '30px' },  { 'targets': [1], width: '150px' },{ 'targets': [2], visible: false }
            ],
            "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
            paging:false,
            "sScrollY": '280px'
        });
        $('#tblHeadRowNewBuy').addClass('gridHeader'); $('#ToolTables_dataGridNewBuy_0,#ToolTables_dataGridNewBuy_1,#dataGridNewBuy_paginate,#dataTables_length').hide();

        setupBRulesTableHeader();
        var table2 = $('#dataGridByrDRule');
        var oBRTable = table2.dataTable({
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
                "sRowSelect": "single", "aButtons": ["select_all", "select_none"]
            },
            "columnDefs": [{   "orderable": false, "searchable": false, 'targets': [0],  },
            { 'targets': [0], width: '20px' }, { 'targets': [1, 6], width: '60px' },
            { 'targets': [2], width: '50px' }, { 'targets': [3], width: '80px' }, { 'targets': [4, 5], width: '120px' },
            { 'targets': [7, 8], visible: false },
            ],
            "lengthMenu": [ [25, 50, 100, -1], [25, 50, 100, "All"]  ],
            "sScrollY": "200px",
            "sScrollX": true,
            "paging": true,
            "order": [[1, "asc"]],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) { var _chkid = "chk" + nRow._DT_RowIndex;
                var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="' + _chkid + '" /></div>';   $('td:eq(0)', nRow).html(_chkdiv);
            }
        });

        $('#tblHeadRowByrDRule').addClass('gridHeader'); $('#ToolTables_dataGridByrDRule_0,#ToolTables_dataGridByrDRule_1').hide();$('#dataGridByrDRule').css('width', '100%');

        setupSRulesTableHeader();
        var table2 = $('#dataGridSppDRule');
        var oSppTable = table2.dataTable({
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
            "columnDefs": [{   "orderable": false, "searchable": false, 'targets': [0], },
            { 'targets': [0], width: '20px' }, { 'targets': [1, 6], width: '60px' },  { 'targets': [2], width: '50px' }, { 'targets': [3], width: '80px' }, { 'targets': [4, 5], width: '120px' },
            { 'targets': [7, 8], visible: false }
            ],
            "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
            "sScrollY": "200px",
            "sScrollX": true,
            "paging": true,
            "order": [[1, "asc"]],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {  var _chkid = "chk" + nRow._DT_RowIndex;
                var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="' + _chkid + '" /></div>';  $('td:eq(0)', nRow).html(_chkdiv);
            }
        });

        $('#tblHeadRowSppDRule').addClass('gridHeader'); $('#ToolTables_dataGridSppDRule_0,#ToolTables_dataGridSppDRule_1').hide();  $('#dataGridSppDRule').css('width', '100%');

        setupXLSMappingTableHeader();
        var table12 = $('#dataGridXlsMapp');
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
            "columnDefs": [{  "orderable": true, 'targets': [0],
            },
            { 'targets': [0], width: '10px' }, { 'targets': [1], width: '70px' }, { 'targets': [7], width: '120px' }, { 'targets': [4, 6, 9], width: '30px' }, { 'targets': [8], width: '60px' }, { 'targets': [10], width: '60px' },
             { 'targets': [2, 3, 4, 5, 11, 12, 13, 14, 15, 16, 17, 18, 19], visible: false }
            ],
            "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {   var _chkid = "chk" + nRow._DT_RowIndex; 
                var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="' + _chkid + '" /></div>';  $('td:eq(0)', nRow).html(_chkdiv);
            }
        });

        $('#tblHeadRowXlsMapp').addClass('gridHeader'); $('#ToolTables_dataGridXlsMapp_0,#ToolTables_dataGridXlsMapp_1').hide(); $('#dataGridXlsMapp').css('width', '100%');

        setupPDFMappingTableHeader();
        var table13 = $('#dataGridPdfMapp');
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
            "columnDefs": [{"orderable": true, 'targets': [0],
            },
            { 'targets': [0], width: '10px' }, { 'targets': [1], width: '40px' }, { 'targets': [2], width: '50px' }, { 'targets': [5], width: '52px' }, { 'targets': [7, 9, 11], width: '60px' },
             { 'targets': [8, 10, 12], width: '70px' }, { 'targets': [3, 4, 5, 6, 13, 14, 15, 16, 17, 18], visible: false }
            ],
            "scrollX": true,
            "lengthMenu": [ [25, 50, 100, -1],   [25, 50, 100, "All"]  ],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var _phdChk = $('#chkHdpdf').prop('checked'); var _hdpcheckedstate = (_phdChk) ? 'checked' : '';
                var _chkid = "chk" + nRow._DT_RowIndex; var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="' + _chkid + '" ' + _hdpcheckedstate + ' /></div>';
                $('td:eq(0)', nRow).html(_chkdiv);
            }
        });

        $('#tblHeadRowPdfMapp').addClass('gridHeader'); $('#ToolTables_dataGridPdfMapp_0,#ToolTables_dataGridPdfMapp_1').hide(); $('#dataGridPdfMapp').css('width', '100%');
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");

        GetBuyerGrid();
        $('.form-wizard').bootstrapWizard({
            withVisible: true,
            'nextSelector': '.button-next',
            'previousSelector': '.button-previous',
            onTabClick: function (tab, navigation, index, clickedIndex) {    return false;  },
            onNext: function (tab, navigation, index) {
                var result = false;
                if (index == 1) { GetSelectedbuyer(); if (_BUYERID != '') { SetDefaultRules(_BUYERID, _SUPPLIERID); result = true; } else { alert('Please select Buyer.');}}
                if (index == 2) { if (_BUYERID != '') { GetMappings(_BUYERID); result = true; } }
                if (result) { handleTab(tab, navigation, index); } return result;
            },
            onPrevious: function (tab, navigation, index) {  handleTab(tab, navigation, index); return true; },
            onTabShow: function (tab, navigation, index) {               
                var total = navigation.find('li:visible').length;    var current = index + 1; var $percent = (current / total) * 100;
                $('.form-wizard').find('.progress-bar').css({ width: $percent + '%' });
            }
        });
        $('.form-wizard').find('.button-previous').hide();
        $('.form-wizard .button-submit').click(function (e) { GetSelectedGrid_Details(); });
        oBXlsMappTable.on('click', 'tbody td', function (e) { e.stopPropagation();
            if (e.target.type == "checkbox") {  var $checkbox = $(this).find(':checkbox'); var _schkd = $checkbox.prop('checked');  var tr = $(this).closest('tr'); 
                tr.find('input[type="checkbox"]').not($(this)).prop("checked", false); $checkbox.prop("checked", _schkd);
                if ($checkbox.is(':checked')) { } else { $('#chkHdexcel').prop('checked', $checkbox.is(':checked')); }          
            }
            GetSelectedCheckbox('dataGridBXlsMapp');
        });
        oBPdfMappTable.on('click', 'tbody td', function (e) {
            e.stopPropagation();
            if (e.target.type == "checkbox") {
                var $checkbox = $(this).find(':checkbox'); var _schkd = $checkbox.prop('checked');  var tr = $(this).closest('tr');
                tr.find('input[type="checkbox"]').not($(this)).prop("checked", false); $checkbox.prop("checked", _schkd);
                if ($checkbox.is(':checked')) { } else { $('#chkHdpdf').prop('checked', $checkbox.is(':checked')); }
            }
            GetSelectedCheckbox('dataGridBPdfMapp');
        });
        oBRTable.on('click', 'tbody td', function (e) {
            e.stopPropagation();
            if (e.target.type == "checkbox") {
                var $checkbox = $(this).find(':checkbox'); var _schkd = $checkbox.prop('checked'); var tr = $(this).closest('tr');
                tr.find('input[type="checkbox"]').not($(this)).prop("checked", false); $checkbox.prop("checked", _schkd);
                if ($checkbox.is(':checked')) { } else { $('#chkHdBRule').prop('checked', $checkbox.is(':checked')); }
            }
            GetSelectedCheckbox('dataGridByrDRule');
        });
        oSppTable.on('click', 'tbody td', function (e) {
            e.stopPropagation();
            if (e.target.type == "checkbox") {
                var $checkbox = $(this).find(':checkbox'); var _schkd = $checkbox.prop('checked');
                var tr = $(this).closest('tr');
                tr.find('input[type="checkbox"]').not($(this)).prop("checked", false); $checkbox.prop("checked", _schkd);
                if ($checkbox.is(':checked')) { } else { $('#chkHdSRule').prop('checked', $checkbox.is(':checked')); }
            }
            GetSelectedCheckbox('dataGridSppDRule');
        });
        $('th').click(function (e) {
            if (e.target.type == "checkbox") {
                var allXLSPages = oBXlsMappTable.fnGetNodes();  var chkBoxes = $('input[type="checkbox"]:not(:disabled)', allXLSPages);  var _isselall = $('#chkHdexcel').prop("checked"); chkBoxes.prop("checked", _isselall);
                var allPDFPages = oBPdfMappTable.fnGetNodes();var chkBoxes1 = $('input[type="checkbox"]:not(:disabled)', allPDFPages);  var _isPselall = $('#chkHdpdf').prop("checked"); chkBoxes1.prop("checked", _isPselall);
                var allBRpages= oBRTable.fnGetNodes();  var chkBoxes2 = $('input[type="checkbox"]:not(:disabled)', allBRpages);  var _isBRselall = $('#chkHdBRule').prop("checked"); chkBoxes2.prop("checked", _isBRselall);
                var allSRPages = oSppTable.fnGetNodes(); var chkBoxes3 = $('input[type="checkbox"]:not(:disabled)', allSRPages);  var _isSRselall = $('#chkHdSRule').prop("checked"); chkBoxes3.prop("checked", _isSRselall);
            }
        });
        function GetSelectedbuyer() {
            var aRow = ''; var aSelectedTrs = $('#dataGridNewBuy .DTTT_selected');
            if (aSelectedTrs != undefined && aSelectedTrs != null && aSelectedTrs.length > 0) {
                var aData = oBuyerTable.fnGetData(aSelectedTrs); _BUYERID = (aData != null) ? aData[2] : '';
            }
        };
        function GetSelectedGrid_Details() {
            var _lstdet = [];
            var brulerows = $('#dataGridByrDRule').DataTable().rows().nodes(); _lstbryrule = [];
            for (var l = 0; l < brulerows.length; l++) {
                var rw2 = brulerows[l]; var tr2 = $(brulerows[l]); if ($('input[type="checkbox"]:checked', tr2).length > 0) { var aData2 = oBRTable.fnGetData(rw2); if (aData2 != null) _lstbryrule.push(aData2[8]); } //defid
            }
            var srulerows = $('#dataGridSppDRule').DataTable().rows().nodes(); _lstspprule = [];
            for (var l = 0; l < srulerows.length; l++) {
                var rw3 = srulerows[l]; var tr3 = $(srulerows[l]); if ($('input[type="checkbox"]:checked', tr3).length > 0) { var aData3 = oSppTable.fnGetData(rw3); if (aData3 != null) _lstspprule.push(aData3[8]); } //defid
            }
            _lstdet.push('BUYER_DRULE|' + _lstbryrule); _lstdet.push('SUPPLIER_DRULE|' + _lstspprule);
            SaveBuyerSupplier_LinkDetails(_SUPPLIERID, _BUYERID, _lstdet);
        };
        $('#btnClose').removeAttr('href').on('click', function (event) { event.preventDefault(); top.window.close(); });
    };

    var handleTab = function (tab, navigation, index) {       
        var total = navigation.find('li').length; var current = index + 1;
        $('.step-title', $('.form-wizard')).text('Step ' + (index + 1) + ' of ' + total);
        jQuery('li', $('.form-wizard')).removeClass("done");  var li_list = navigation.find('li');
        for (var i = 0; i < index; i++) { jQuery(li_list[i]).addClass("done"); }
        if (current == 1) $('.form-wizard').find('.button-previous').hide();
        else $('.form-wizard').find('.button-previous').show();
        if (current >= total) { $('.form-wizard').find('.button-next').hide(); $('.form-wizard').find('.button-submit').show(); }
        else { $('.form-wizard').find('.button-next').show(); $('.form-wizard').find('.button-submit').hide(); }
        Metronic.scrollTo($('.page-title'));
    }

    function SetCommondetails() {
        var _format = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "BuyerSupplier_ConfirmLink_Wizard.aspx/GetConfigDetails",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {  if (response.d!='')_subject = response.d.split('||')[1]; }
                catch (err) {toastr.error('Error :' + err, "Lighthouse eSolutions Pte. Ltd");}
            },
            failure: function (response) { toastr.error("Failure", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    $('input[type="radio"]').on('click', function () {
        $('#tblXls').show(); $('#tblPdf').hide(); $('#tblXls').hide();
        if ($(this).attr('id') == 'rdExcel') { $('#tblXls').show(); GetBuyerExcelMapp(_BUYERID); }
        else if ($(this).attr('id') == 'rdPDF') { $('#tblXls').hide(); $('#tblPdf').show(); GetBuyerPDFMapp(_BUYERID); }
    });
    /*Buyer List*/
    var setupBuyerTableHeader = function () {var dtfilter = '<th>Buyer Code</th><th>Buyer Name</th><th>SUPPLIERID</th>'; $('#tblHeadRowNewBuy').empty(); $('#tblHeadRowNewBuy').append(dtfilter); $('#tblBodyNewBuy').empty(); };

    var GetBuyerGrid = function () {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "SupplierDetail.aspx/GetAllBuyers",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) {  Table = DataSet.NewDataSet.Table;  FillBuyerGrid(Table); }
                    else $('#dataGridNewBuy').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) {  Metronic.unblockUI();    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyers :' + err);  }
            },
            failure: function (response) {   toastr.error("failure get", response.d); Metronic.unblockUI();   },
            error: function (response) {   toastr.error("error get", response.responseText); Metronic.unblockUI();  }
        });
    };

    function FillBuyerGrid(Table) {
        try {
            $('#dataGridNewBuy').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridNewBuy').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>'); cells[0] = Str(Table[i].ADDR_CODE);  cells[1] = Str(Table[i].ADDR_NAME); cells[2] = Str(Table[i].ADDRESSID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.ITEM_UOM_MAPID != undefined && Table.ITEM_UOM_MAPID != null) {
                    var t = $('#dataGridNewBuy').dataTable();  var r = jQuery('<tr id=' + 1 + '>');   var cells = new Array();
                    cells[0] = Str(Table.ADDR_CODE);  cells[1] = Str(Table.ADDR_NAME);cells[2] = Str(Table.ADDRESSID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)  { }
    };
    /**/
    /*Default Rules */
    function SetDefaultRules(_BUYERID, _SUPPLIERID) { var bGROUP_FORMAT = GetFormat(_BUYERID); GetBRulesGrid(_BUYERID, bGROUP_FORMAT); var sGROUP_FORMAT = GetFormat(_SUPPLIERID); GetSRulesGrid(_SUPPLIERID, sGROUP_FORMAT); };

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
                try {   _format = Str(response.d);}
                catch (err) {  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer Format :' + err);}
            },
            failure: function (response) {  toastr.error("failure get", response.d);  },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
        return _format;
    };

    var setupSRulesTableHeader = function () {
        var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="chkHdSRule" checked /></div>';
        var dtfilter = '<th>' + _chkdiv + '</th><th>Rule number</th><th>Doc Type</th><th>Rule Code</th><th>Description</th><th>Comments</th><th>Rule Value</th><th>RULE_ID</th><th>DEF_ID</th>';
        $('#tblHeadRowSppDRule').empty().append(dtfilter); $('#tblBodySppDRule').empty();
    };

    var GetSRulesGrid = function (SUPPLIERID, GROUP_FORMAT) {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "SupplierDetail.aspx/GetSupplierDefaultRules",
            data: "{'SUPPLIERID':'" + SUPPLIERID + "','GROUP_FORMAT':'" + GROUP_FORMAT + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) {
                        Table = DataSet.NewDataSet.Table; FillSRulesGrid(Table); SetCheckboxState('dataGridSppDRule');
                    }
                    else { $('#dataGridSppDRule').DataTable().clear().draw(); $('#chkHdSRule').prop('checked', false); }
                    Metronic.unblockUI();
                }
                catch (err) {  Metronic.unblockUI();  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Supplier Rules :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI();},
            error: function (response) {    toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };

    function FillSRulesGrid(Table) {
        try {
            $('#dataGridSppDRule').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridSppDRule').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();  var r = jQuery('<tr id=' + i + '>');
                    var _arrcommt = Str(Table[i].RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str(''); cells[1] = Str(Table[i].RULE_NUMBER); cells[2] = Str(Table[i].DOC_TYPE);  cells[3] = Str(Table[i].RULE_CODE);
                    cells[4] = Str(Table[i].RULE_DESC); cells[5] = _arrcommt;  cells[6] = Str(Table[i].RULE_VALUE);cells[7] = Str(Table[i].RULE_ID);  cells[8] = Str(Table[i].DEF_ID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.DEF_ID != undefined && Table.DEF_ID != null) {
                    var t = $('#dataGridSppDRule').dataTable();  var r = jQuery('<tr id=' + 1 + '>');  var cells = new Array();
                    var _arrcommt = Str(Table.RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str(''); cells[1] = Str(Table.RULE_NUMBER);   cells[2] = Str(Table.DOC_TYPE); cells[3] = Str(Table.RULE_CODE);
                    cells[4] = Str(Table.RULE_DESC);  cells[5] = _arrcommt; cells[6] = Str(Table.RULE_VALUE);  cells[7] = Str(Table.RULE_ID);  cells[8] = Str(Table.DEF_ID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)  { }
    };
   
    var setupBRulesTableHeader = function () {
        var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="chkHdBRule" checked /></div>';
        var dtfilter = '<th>' + _chkdiv + '</th><th>Rule No.</th><th>Doc Type</th><th>Rule Code</th><th>Description</th><th>Comments</th><th>Rule Value</th><th>RULEID</th><th>DEF_ID</th>';
        $('#tblHeadRowByrDRule').empty().append(dtfilter); $('#tblBodyByrDRule').empty();
    };

    var GetBRulesGrid = function (BUYERID, GROUP_FORMAT) {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "BuyerDetail.aspx/GetBuyerDefaultRules",
            data: "{'BUYERID':'" + BUYERID + "','GROUP_FORMAT':'" + GROUP_FORMAT + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) {
                        Table = DataSet.NewDataSet.Table; FillBRulesGrid(Table); SetCheckboxState('dataGridByrDRule');
                    }
                    else { $('#dataGridByrDRule').DataTable().clear().draw(); $('#chkHdBRule').prop('checked', false); }
                    Metronic.unblockUI();
                }
                catch (err) {  Metronic.unblockUI();    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer Rules :' + err);  }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI();  },
            error: function (response) {  toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };

    function FillBRulesGrid(Table) {
        try {
            $('#dataGridByrDRule').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridByrDRule').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array(); var r = jQuery('<tr id=' + i + '>');
                    var _arrcommt = Str(Table[i].RULE_COMMENTS).split(',').join('<br/>');  cells[0] = Str('');
                    cells[1] = Str(Table[i].RULE_NUMBER); cells[2] = Str(Table[i].DOC_TYPE); cells[3] = Str(Table[i].RULE_CODE);  cells[4] = Str(Table[i].RULE_DESC);
                    cells[5] = _arrcommt; cells[6] = Str(Table[i].RULE_VALUE); cells[7] = Str(Table[i].RULE_ID); cells[8] = Str(Table[i].DEF_ID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.DEF_ID != undefined && Table.DEF_ID != null) {
                    var t = $('#dataGridByrDRule').dataTable();   var r = jQuery('<tr id=' + 1 + '>'); var cells = new Array();
                    var _arrcommt = Str(Table.RULE_COMMENTS).split(',').join('<br/>'); cells[0] = Str('');
                    cells[1] = Str(Table.RULE_NUMBER); cells[2] = Str(Table.DOC_TYPE);  cells[3] = Str(Table.RULE_CODE);   cells[4] = Str(Table.RULE_DESC);
                    cells[5] = _arrcommt; cells[6] = Str(Table.RULE_VALUE);cells[7] = Str(Table.RULE_ID); cells[8] = Str(Table.DEF_ID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)  { }
    };
    /*end*/ 
    /*Mappings*/
    function GetMappings(_BUYERID) { GetBuyerExcelMapp(_BUYERID); GetBuyerPDFMapp(_BUYERID); };

    var setupXLSMappingTableHeader = function () {
        $('#tblHeadRowXlsMapp').empty(); $('#tblBodyXlsMapp').empty();
        var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="chkHdexcel" checked /></div>';
        var dtfilter = '<th>' + _chkdiv + '</th><th>Format</th><th>Buyer Code</th><th>Buyer Name</th><th>Supplier Code</th><th>Supplier Name</th><th>Cell-1</th><th>Cell-1 Value-1</th><th>Cell-1 Value-2</th><th>Cell-2</th><th>Cell-2 Value</th><th>Cell (No Discount)</th> <th>Cell Value (No Discount)</th> ' +
            ' <th>XLS_BUYER_MAPID</th><th>EXCEL_MAPID</th><th>BUYER_SUPP_LINKID</th><th>BUYERID</th><th>SUPPLIERID</th> <th>GROUP_ID</th><th>DOC_TYPE</th>';
        $('#tblHeadRowXlsMapp').append(dtfilter);
    };

    function GetBuyerExcelMapp(BUYERID) {
        if (BUYERID != undefined) {
            var DataSet = GetMappingXLSGrid(BUYERID, 'BUYER');
            if (DataSet.NewDataSet != null) {
                var Table = DataSet.NewDataSet.Table; FillBMappingXLSGrid(Table); SetCheckboxState('dataGridXlsMapp'); }
            else { $('#chkHdexcel').prop('checked', false); $('#dataGridXlsMapp').DataTable().clear().draw(); }
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
            $('#dataGridXlsMapp').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridXlsMapp').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array(); var r = jQuery('<tr id=' + i + '>'); cells[0] = Str('');
                    cells[1] = Str(Table[i].GROUP_CODE); cells[2] = Str(Table[i].BUYER_CODE); cells[3] = Str(Table[i].BUYER_NAME); cells[4] = Str(Table[i].SUPPLIER_CODE);
                    cells[5] = Str(Table[i].SUPPLIER_NAME); cells[6] = Str(Table[i].MAP_CELL1); cells[7] = Str(Table[i].MAP_CELL1_VAL1); cells[8] = Str(Table[i].MAP_CELL1_VAL2);
                    cells[9] = Str(Table[i].MAP_CELL2); cells[10] = Str(Table[i].MAP_CELL2_VAL); cells[11] = Str(Table[i].MAP_CELL_NODISC); cells[12] = Str(Table[i].MAP_CELL_NODISC_VAL);
                    cells[13] = Str(Table[i].XLS_BUYER_MAPID); cells[14] = Str(Table[i].EXCEL_MAPID); cells[15] = Str(Table[i].BUYER_SUPP_LINKID);
                    cells[16] = Str(Table[i].BUYERID); cells[17] = Str(Table[i].SUPPLIERID); cells[18] = Str(Table[i].GROUP_ID); cells[19] = Str(Table[i].DOC_TYPE);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.XLS_BUYER_MAPID != undefined && Table.XLS_BUYER_MAPID != null) {
                    var t = $('#dataGridXlsMapp').dataTable(); var r = jQuery('<tr id=' + 1 + '>'); var cells = new Array();
                    cells[0] = Str('');  cells[1] = Str(Table.GROUP_CODE); cells[2] = Str(Table.BUYER_CODE); cells[3] = Str(Table.BUYER_NAME);
                    cells[4] = Str(Table.SUPPLIER_CODE); cells[5] = Str(Table.SUPPLIER_NAME);cells[6] = Str(Table.MAP_CELL1);  cells[7] = Str(Table.MAP_CELL1_VAL1);
                    cells[8] = Str(Table.MAP_CELL1_VAL2);  cells[9] = Str(Table.MAP_CELL2);  cells[10] = Str(Table.MAP_CELL2_VAL);  cells[11] = Str(Table.MAP_CELL_NODISC);
                    cells[12] = Str(Table.MAP_CELL_NODISC_VAL); cells[13] = Str(Table.XLS_BUYER_MAPID); cells[14] = Str(Table.EXCEL_MAPID);  cells[15] = Str(Table.BUYER_SUPP_LINKID);
                    cells[16] = Str(Table.BUYERID); cells[17] = Str(Table.SUPPLIERID);  cells[18] = Str(Table.GROUP_ID); cells[19] = Str(Table.DOC_TYPE);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e) { }
    }

    var setupPDFMappingTableHeader = function () {
        $('#tblHeadRowPdfMapp').empty(); $('#tblBodyPdfMapp').empty();
        var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="chkHdpdf" checked /></div>';
        var dtfilter = '<th>' + _chkdiv + '</th><th>Format</th><th>Doc Type</th><th>Buyer Code</th><th>Buyer Name</th><th>Supplier Code</th><th>Supplier Name</th> <th>Map Range (1)</th><th>Map Range (1) Value</th><th>Map Range (2)</th><th>Map Range (2) Value</th><th>Map Range (3)</th><th>Map Range (3) Value</th> ' +
            ' <th>MAP_ID</th><th>PDF_MAPID</th><th>BUYER_SUPP_LINKID</th><th>BUYERID</th><th>SUPPLIERID</th> <th>GROUP_ID</th>';
        $('#tblHeadRowPdfMapp').append(dtfilter);
    };

    function GetBuyerPDFMapp(BUYERID) {
        if (BUYERID != undefined) {
            var DataSet = GetMappingPDFGrid(BUYERID, 'BUYER'); if (DataSet.NewDataSet != null) {
                var Table = DataSet.NewDataSet.Table; FillBMappingPDFGrid(Table); SetCheckboxState('dataGridPdfMapp'); }
            else { $('#chkHdpdf').prop('checked', false); $('#dataGridPdfMapp').DataTable().clear().draw(); }
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

    function FillBMappingPDFGrid(Table) {
        try {
            $('#dataGridPdfMapp').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridPdfMapp').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array(); var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str(''); cells[1] = Str(Table[i].GROUP_CODE); cells[2] = Str(Table[i].DOC_TYPE); cells[3] = Str(Table[i].BUYER_CODE);
                    cells[4] = Str(Table[i].BUYER_NAME); cells[5] = Str(Table[i].SUPPLIER_CODE);   cells[6] = Str(Table[i].SUPPLIER_NAME); cells[7] = Str(Table[i].MAPPING_1);
                    cells[8] = Str(Table[i].MAPPING_1_VALUE); cells[9] = Str(Table[i].MAPPING_2); cells[10] = Str(Table[i].MAPPING_2_VALUE);  cells[11] = Str(Table[i].MAPPING_3);
                    cells[12] = Str(Table[i].MAPPING_3_VALUE);  cells[13] = Str(Table[i].MAP_ID);cells[14] = Str(Table[i].PDF_MAPID);  cells[15] = Str(Table[i].BUYER_SUPP_LINKID);
                    cells[16] = Str(Table[i].BUYERID);  cells[17] = Str(Table[i].SUPPLIERID); cells[18] = Str(Table[i].GROUP_ID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.PDF_MAPID != undefined && Table.PDF_MAPID != null) {
                    var t = $('#dataGridPdfMapp').dataTable(); var r = jQuery('<tr id=' + 1 + '>'); var cells = new Array();
                    cells[0] = Str('');  cells[1] = Str(Table.GROUP_CODE); cells[2] = Str(Table.DOC_TYPE); cells[3] = Str(Table.BUYER_CODE);
                    cells[4] = Str(Table.BUYER_NAME);   cells[5] = Str(Table.SUPPLIER_CODE);  cells[6] = Str(Table.SUPPLIER_NAME); cells[7] = Str(Table.MAPPING_1);
                    cells[8] = Str(Table.MAPPING_1_VALUE);  cells[9] = Str(Table.MAPPING_2); cells[10] = Str(Table.MAPPING_2_VALUE);  cells[11] = Str(Table.MAPPING_3);
                    cells[12] = Str(Table.MAPPING_3_VALUE); cells[13] = Str(Table.MAP_ID); cells[14] = Str(Table.PDF_MAPID);   cells[15] = Str(Table.BUYER_SUPP_LINKID);
                    cells[16] = Str(Table.BUYERID);  cells[17] = Str(Table.SUPPLIERID);  cells[18] = Str(Table.GROUP_ID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e) { }
    };
    /*end*/
    function SaveBuyerSupplier_LinkDetails(SUPPLIERID, BUYERID, _nfieldval) {
        var slByrSppdet = [];
        for (var j = 0; j < _nfieldval.length; j++) { slByrSppdet.push(_nfieldval[j]); }
        var data2send = JSON.stringify({ SUPPLIERID: SUPPLIERID, BUYERID: BUYERID, slByrSppdet: slByrSppdet });
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "BuyerSupplier_ConfirmLink_Wizard.aspx/SaveBuyerSupplierLink",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "Buyer-Supplier Link created successfully."); top.window.close(); }
                    Metronic.unblockUI();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to create Buyer-Supplier Link :' + err); Metronic.unblockUI(); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };
    /*Common*/

    function SetCheckboxState(grid) {
        if (grid == 'dataGridXlsMapp') { var _ishdrchk = $('#chkHdexcel').prop('checked'); var xlsrows = $('#dataGridXlsMapp').DataTable().rows().nodes();
            if (_ishdrchk) {   var chkBoxes = $('input[type="checkbox"]:not(:disabled)', xlsrows); chkBoxes.prop("checked", _ishdrchk); }
            else {
                for (var l = 0; l < xlsrows.length; l++) {var tr = $(xlsrows[l]);
                    for (var k = 0; k < _lstSelectedxlRowindx.length; k++) { if (_lstSelectedxlRowindx[k] == l) { tr.find('input[type="checkbox"]').prop('checked', true); }}
                }
            }
        }
        else if (grid == 'dataGridPdfMapp') { var _ishdrchk = $('#chkHdpdf').prop('checked'); var pdfrows = $('#dataGridPdfMapp').DataTable().rows().nodes();
            if (_ishdrchk) {   var chkBoxes1 = $('input[type="checkbox"]:not(:disabled)', pdfrows); chkBoxes1.prop("checked", _ishdrchk); }
            else {
                for (var l = 0; l < pdfrows.length; l++) { var tr1 = $(pdfrows[l]);
                    for (var k = 0; k < _lstSelectedpdfRowindx.length; k++) {  if (_lstSelectedpdfRowindx[k] == l) { tr1.find('input[type="checkbox"]').prop('checked', true); }   }
                }
            }
        }
        else if (grid == 'dataGridByrDRule') { var _ishdrchk = $('#chkHdBRule').prop('checked'); var brulerows = $('#dataGridByrDRule').DataTable().rows().nodes();
            if (_ishdrchk) { var chkBoxes2 = $('input[type="checkbox"]:not(:disabled)', brulerows); chkBoxes2.prop("checked", _ishdrchk); }
            else {
                for (var l = 0; l < brulerows.length; l++) { var tr2 = $(brulerows[l]);
                    for (var k = 0; k < _lstSelectedBRRowindx.length; k++) {    if (_lstSelectedBRRowindx[k] == l) { tr2.find('input[type="checkbox"]').prop('checked', true); }  }
                }
            }
        }
        else if (grid == 'dataGridSppDRule') {  var _ishdrchk = $('#chkHdSRule').prop('checked'); var srulerows = $('#dataGridSppDRule').DataTable().rows().nodes();
            if (_ishdrchk) { var chkBoxes3 = $('input[type="checkbox"]:not(:disabled)', srulerows); chkBoxes3.prop("checked", _ishdrchk); }
            else {
                for (var l = 0; l < srulerows.length; l++) {   var tr3 = $(srulerows[l]);
                    for (var k = 0; k < _lstSelectedSRRowindx.length; k++) { if (_lstSelectedSRRowindx[k] == l) { tr3.find('input[type="checkbox"]').prop('checked', true); } }
                }
            }
        }
    };

    function GetSelectedCheckbox(grid) {
        if (grid == 'dataGridXlsMapp') {
            var xlsrows = $('#dataGridXlsMapp').DataTable().rows().nodes(); _lstSelectedxlRowindx = [];
            for (var l = 0; l < xlsrows.length; l++) {
                var tr = $(xlsrows[l]); if ($('input[type="checkbox"]:checked', tr).length > 0) { _lstSelectedxlRowindx.push(l);   }
            }
        }
        else if (grid == 'dataGridPdfMapp') {
            var pdfrows = $('#dataGridPdfMapp').DataTable().rows().nodes(); _lstSelectedpdfRowindx = [];
            for (var l = 0; l < pdfrows.length; l++) {
                var tr1 = $(pdfrows[l]); if ($('input[type="checkbox"]:checked', tr1).length > 0) { _lstSelectedpdfRowindx.push(l); }
            }
        }
        else if (grid == 'dataGridByrDRule') {
            var brulerows = $('#dataGridByrDRule').DataTable().rows().nodes(); _lstSelectedBRRowindx = [];
            for (var l = 0; l < brulerows.length; l++) {
                var tr2 = $(brulerows[l]); if ($('input[type="checkbox"]:checked', tr2).length > 0) { _lstSelectedBRRowindx.push(l); }
            }
        }
        else if (grid == 'dataGridSppDRule') {
            var srulerows = $('#dataGridSppDRule').DataTable().rows().nodes(); _lstSelectedSRRowindx = [];
            for (var l = 0; l < srulerows.length; l++) {
                var tr3 = $(srulerows[l]); if ($('input[type="checkbox"]:checked', tr3).length > 0) { _lstSelectedSRRowindx.push(l); }
            }
        }
    };

    var setToolbar = function () {
        var _btns = '<span title="Close" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="btnclose"><a id="btnClose" class="toolbtn"><i class="fa fa-times" style="text-align:center;"></i></a></div></span>';
        $('#toolbtngroupdet').append(_btns);
    };
    /**/
    /*Buyer-Supplier path details*/
    function GetBuyerSupplier_PathDetails() { var _hddet = fnBuyerSupplierPathdetails(); $('#dvBSPathDet').empty().append(_hddet) };

    function fnBuyerSupplierPathdetails() {
        var sOut = ''; var byr_adaptorpath = ''; var sp_adaptorpath = '';
        var _bdownloadpathid = 'txtByrDownloadPath'; var _buploadpathid = 'txtByrUploadPath'; var _bimpid = 'txtByrImpPath'; var _bexpid = 'txtByrExpPath';
        var _bdownloadpath = ''; var _buploadpath = ''; var _bImpPath = ''; var _bExpPath = '';
        _bExpPath = (_bExpPath != undefined) ? _bExpPath : ''; _bImpPath = (_bImpPath != undefined) ? _bImpPath : '';
        var _sdownloadpathid = 'txtSppDownloadPath'; var _suploadpathid = 'txtSppUploadPath'; var _simpid = 'txtSppImpPath'; var _sexpid = 'txtSppExpPath';
        var _sdownloadpath = ''; var _suploadpath = ''; var _sImpPath = ''; var _sExpPath = '';
        _sExpPath = (_sExpPath != undefined) ? _sExpPath : ''; _sImpPath = (_sImpPath != undefined) ? _sImpPath : '';
        var _byrdet = '<div class="row"><div class="col-md-12"><div class="row"><div class="col-md-12"><div class="col-md-4"><span class="font-blue font-lg"> Buyer </span> </div></div></div>';
        byr_adaptorpath = ' <div class="row"><div class="col-md-12"> <div class="form-group">' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Download Path </label> </div>' +
                       ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _bdownloadpathid + '" value="' + _bdownloadpath + '"/> </div>' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Upload Path </label> </div>' +
                       ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _buploadpathid + '" value="' + _buploadpath + '"/></div> </div></div></div>';
        _byrdet += byr_adaptorpath + ' <div class="row"><div class="col-md-12"><div class="form-group">' +
       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Import Path </label> </div>' +
       ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _bimpid + '" value="' + _bImpPath + '"/></div>' +
       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Export Path </label> </div>' +
       ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _bexpid + '" value="' + _bExpPath + '"/></div> </div></div></div></div></div> <hr/>';
        var _spdet = '<div class="row"><div class="col-md-12"><div class="row"><div class="col-md-12"><div class="col-md-4"><span class="font-blue font-lg"> Supplier </span> </div></div></div>';
        sp_adaptorpath = ' <div class="row"><div class="col-md-12"> <div class="form-group">' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Download Path </label> </div>' +
                       ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _sdownloadpathid + '" value="' + _sdownloadpath + '"/> </div>' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Upload Path </label> </div>' +
                       ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _suploadpathid + '" value="' + _suploadpath + '"/></div></div></div></div>';
        _spdet += sp_adaptorpath + ' <div class="row"><div class="col-md-12"> <div class="form-group">' +
        ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Import Path </label> </div>' +
        ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _bimpid + '" value="' + _sImpPath + '"/></div>' +
        ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Export Path </label> </div>' +
        ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _bexpid + '" value="' + _sExpPath + '"/></div>' +
        ' </div></div></div></div></div>';
        sOut = _byrdet + _spdet; return sOut;
    };
    /**/
    return { init: function () { handleBuyerSupplier_ConfirmLink_Wizard(); } };
       
}(); 
