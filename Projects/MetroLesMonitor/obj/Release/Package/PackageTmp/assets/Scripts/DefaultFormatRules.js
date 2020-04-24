var _ADDRTYPE = ''; var _deffmtdet = []; var _uplDwlPath = ''; var _configpath = ''; var selectedTr = ''; var previousTr = ''; var _existRuleid = ''; var _lstRuleid = [];
var _lstdeffrmt = []; var _lstdeffrmtrule = [];

var DefaultFormatRules = function () {
   
    var handleDefaultFormatRules = function () {
        var _isdeffmtUpdate = -2; var _isdeffmtRUpdate = -2; var nNew = false;
        $('#divSpacer').remove(); $('#divFilterWrapper').remove(); $('#pageTitle').empty().append('Default Format Rules'); SetupBreadcrumb('Home', 'Home.aspx', 'Format', '#', 'DefaultFormat Rules', 'DefaultFormatRules.aspx');
        $(document.getElementById('lnkFormat')).addClass('active open'); $(document.getElementById('spDefFormtRule')).addClass('title font-title SelectedColor'); $(document.getElementById('spDefFormtRule')).text('DefaultFormat Rules');
        setFilterToolbar();
      
        setupDefFmtRulesTableHeader();
        var table3 = $('#dataGridDFRule');
        var oDFRTable = table3.dataTable({
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
            "columnDefs": [{  // set default column settings
                'orderable': false, "searching": true, "autoWidth": false, 'targets': [0],
            },
             { 'targets': [0], width: '5px' }, { 'targets': [1], width: '40px' }, { 'targets': [2], width: '30px' }, { 'targets': [3], width: '30px' },
            { 'targets': [4], width: '60px' }, { 'targets': [5, 6], width: '120px' }, { 'targets': [7], width: '40px' }, { 'targets': [8, 9], visible: false },
            ],
            "lengthMenu": [[15, 30, 50, 100], [15, 30, 50, 100]],          
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var aedit = 'Edit' + nRow._DT_RowIndex; var adelete = 'Delete' + nRow._DT_RowIndex;
                var _divbtn = ' <div id="dvActn" style="text-align:center;">' +
                '<span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span>' +
                '<span id=' + adelete + ' class="actionbtn" data-toggle="tooltip" title="Delete"><i class="glyphicon glyphicon-trash"></i></span></div>';
                if (_isdeffmtRUpdate != nRow._DT_RowIndex) { $('td:eq(0)', nRow).html(_divbtn); }
            }
        });
        $('#tblHeadRowDFRule').addClass('gridHeader'); $('#ToolTables_dataGridDFRule_0,#ToolTables_dataGridDFRule_1').hide(); $('#dataGridDFRule').css('width', '100%');

        setupNewRulesTableHeader();
        var table2 = $('#dataGridNewRule');
        var oNRTable = table2.dataTable({
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
            "columnDefs": [{  // set default column settings
                'orderable': false, "searching": true, "autoWidth": false, 'targets': [0],
            },
            { 'targets': [0], width: '5px' }, { 'targets': [1], width: '10px' }, { 'targets': [2], width: '20px' }, { 'targets': [3], width: '60px' },
            { 'targets': [4,5], width: '90px' },  { 'targets': [6], visible: false }
            ],
            "lengthMenu": [
                [15, 25, 50, 100, -1],
                [15, 25, 50, 100, "All"] // change per page values here
            ],
            "aaSorting": [],
            "paging":false,
            "scrollY": '300px',
            "scrollX": true,
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var _chkid = "chk" + nRow._DT_RowIndex; var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="' + _chkid + '"  /></div>';
                $('td:eq(0)', nRow).html(_chkdiv);
            }
        });
        $('#tblHeadRowRule').addClass('gridHeader'); $('#ToolTables_dataGridNewRule_0,#ToolTables_dataGridNewRule_1,#dataGridNewRule_paginate').hide();
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%"); $('#dataGridNewRule').css('width', '100%');
     
        GetDefFormatRuleGrid(); 

        oDFRTable.on('click', 'tbody td', function (e) {
            var selectedTr = $(this).parents('tr')[0]; var aData = oDFRTable.fnGetData(selectedTr); _targetclick = '';
            if (oDFRTable.fnIsOpen(selectedTr) && ((e.target.innerText == 'Edit'))) { oDFRTable.fnClose(selectedTr); }
            else {
                var divid = 'dv' + selectedTr._DT_RowIndex;
                if (e.target.className == 'glyphicon glyphicon-pencil') {
                    _isdeffmtUpdate = selectedTr._DT_RowIndex; _targetclick = e.target.innerText;
                    if ((previousTr != '') && (oDFRTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {
                        var prevdivid = 'dv' + previousTr._DT_RowIndex; oDFRTable.fnClose(previousTr); $('#' + prevdivid).show();
                    }
                    if (previousTr != '' && previousTr != selectedTr) { restoreRow(previousTr, oDFRTable); }
                    nNew = false; editRow(oDFRTable, selectedTr); previousTr = selectedTr;
                }
               else if (e.target.className == 'glyphicon glyphicon-trash') {
                    var _DefFmtRuleid = aData[9]; DeleteDefFormatRulesDetails(_DefFmtRuleid, GetDefFormatRuleGrid);
               }
               else if (e.target.className == 'glyphicon glyphicon-floppy-save') {
                   _lstdeffrmtrule = [];
                   var _deffmt = aData[1]; var _rulecode = aData[4]; var _DefFmtRuleid = aData[9]; var _ruleval = $('#cbRuleValue :selected').val(); _lstdeffrmtrule.push('RULE_VALUE|' + _ruleval);
                   _lstdeffrmtrule.push('DEFFORMAT_RULEID|' + _DefFmtRuleid); _lstdeffrmtrule.push('DEFAULT_FORMAT|' + _deffmt); _lstdeffrmtrule.push('RULE_CODE|' + _rulecode);
                   SaveDefFormatRulesDetails(_lstdeffrmtrule, GetDefFormatRuleGrid); previousTr = '';
               }
               else if (e.target.className == 'glyphicon glyphicon-ban-circle') {
                   restoreRow(selectedTr, oDFRTable); previousTr = '';
               }
                $('#btnDefUpdate').click(function () {
                    if (aData != null) {
                        _lstdeffrmt = []; var _res = Validate_NDefFormat();
                        if (_res == true) { var _DefFmtid = aData[5];
                            _lstdeffrmt.push('DEFAULTFORMATID|' + _DefFmtid);  _lstdeffrmt.push('ADDR_TYPE|' + Str($('#cbAddrType').val())); _lstdeffrmt.push('DEFAULT_FORMAT|' + Str($('#txtDefFormat').val()));
                            _lstdeffrmt.push('IMPORT_PATH|' + Str($('#txtDFImportPath').val())); _lstdeffrmt.push('EXPORT_PATH|' + Str($('#txtDFExportPath').val()));
                            SaveDefaultFormat(_lstdeffrmt, GetDefFormatGrid);_isdeffmtUpdate = -2;   $('#' + divid).show();
                        }
                    }
                });
                $('#btnDefCancel').click(function () { if (oDFRTable.fnIsOpen(selectedTr)) { oDFRTable.fnClose(selectedTr); } $('#' + divid).show(); _isdeffmtUpdate = -2; });
            }
        });

        function restoreRow(nRow, oTable) {
            var aData = oTable.fnGetData(nRow); var jqTds = $('>td', nRow);
            for (var i = 0, iLen = jqTds.length; i < iLen; i++) {    if (i == 0) { } else { oTable.fnUpdate(jqTds[i].innerHTML, nRow, i, false); }} oTable.fnDraw();
        };

        function gridCancelEdit(nNew, nEditing, oTable) {
            var nRow = $('.DTTT_selected'); if (nNew) { oTable.fnDeleteRow(nEditing); nNew = false; } else { nRow = nEditing; restoreRow(nRow, oTable); $(nRow).removeClass("DTTT_selected selected"); }
        };

        function editRow(oTable, nRow) {
            var aData = oTable.fnGetData(nRow); nEditing = nRow; var jqTds = $('>td', nRow);
            var _lstRuleval = []; _lstRuleval.push("0|0"); _lstRuleval.push("1|1");
            var detTag = '<div style="text-align:center;"><span class="actionbtn" data-toggle="tooltip" title="Update"><i class="glyphicon glyphicon-floppy-save"></i></span>'+
               ' <span class="actionbtn" data-toggle="tooltip" title="Cancel"><i class="glyphicon glyphicon-ban-circle"></i></span></div>';
            jqTds[0].innerHTML = detTag;
            jqTds[7].innerHTML = '<select id="cbRuleValue" class="fullWidth selectheight">' + FillCombo(aData[7], _lstRuleval) + '</select>';
            $('#cbRuleValue').select2();
        };

        $('#btnRefresh').live('click', function (e) { e.preventDefault(); GetDefFormatRuleGrid(); });
        $('#btnClear').live('click', function (e) { e.preventDefault(); ClearFilter(); });
        $('#btnNew').live('click', function (e) { e.preventDefault(); ClearNewRuleModal();  GetAllDefaultFormats();   $("#ModalNewRule").modal('show'); });
        $('#cbDefaultFormat').live("change", function (e) { var _selectdefval = $('#cbDefaultFormat :selected').val(); GetRulesGrid(_selectdefval); });
        $('#btnNewRule').live('click', function (e) {
            e.preventDefault(); var Rulrrows = $('#dataGridNewRule').DataTable().rows().nodes(); var _deffmtval = $('#cbDefaultFormat :selected').val(); var _deffmt = $('#cbDefaultFormat :selected').text();
            for (var l = 0; l < Rulrrows.length; l++) {
                var rw = Rulrrows[l]; var tr = $(Rulrrows[l]);
                if ($('input[type="checkbox"]:checked', tr).length > 0) {
                    _lstdeffrmtrule = [];
                    var _rulecode = oNRTable.fnGetData(tr)[3]; var _RULEid = oNRTable.fnGetData(tr)[6];
                    _lstdeffrmtrule.push('DEFFORMAT_RULEID|0'); _lstdeffrmtrule.push('DEFAULT_FORMAT|' + _deffmt); _lstdeffrmtrule.push('DEFAULT_FORMATID|' + _deffmtval);
                    _lstdeffrmtrule.push('RULEID|' + _RULEid); _lstdeffrmtrule.push('RULE_CODE|' + _rulecode);
                    SaveDefFormatRulesDetails(_lstdeffrmtrule, GetDefFormatRuleGrid); $("#ModalNewRule").modal('hide');
                }
            }
        });
    };

    var setFilterToolbar = function () {
        $('#toolbtngroup').empty();
        var _btns = ' <div class="row" style="margin-bottom: 2px;padding-right: 20px;"> <div class="col-md-12">' +
            ' <div id="toolbtngroup" >' +
            ' <span title="New" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnNew"><i class="fa fa-plus" style="text-align:center;"></i></a></div>' +
            ' <span title="Clear" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnClear"><i class="fa fa-eraser" style="text-align:center;"></i></a></div>' +
            ' <span title="Refresh" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"> <a href="javascript:;" class="toolbtn" id="btnRefresh"><i class="fa fa-recycle" style="text-align:center;"></i></a></div></div>' +
            ' </div></div>';
        $('#toolbtngroup').append(_btns);
    };

    var setupNewRulesTableHeader = function () {
        var dtfilter = '<th></th><th>Rule No.</th><th>Doc Type</th><th>Rule Code</th><th>Description</th><th>Comments</th><th>RULEID</th>'; $('#tblHeadRowRule').empty().append(dtfilter); $('#tblBodyRule').empty();
    };

    var setupDefFmtRulesTableHeader = function () {
        var dtfilter = '<th style="text-align:center;">#</th><th>Format</th><th>Rule No.</th><th>Doc Type</th><th>Rule Code</th><th>Description</th><th>Comments</th><th>Rule Value</th><th>RULEID</th><th>DEFFORMAT_RULEID</th>'; $('#tblHeadRowDFRule').empty().append(dtfilter); $('#tblBodyDFRule').empty();
    };

    var GetDefFormatRuleGrid = function () {
        setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultFormatRules.aspx/GetAllDefaultFormatRules",
            data:'{ }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) {
                        Table = DataSet.NewDataSet.Table; FillDefaultFormatRulesGrid(Table);
                    }
                    else $('#dataGridDFRule').DataTable().clear().draw();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get DefaultFormat Rules :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
        }, 200);
    };

    function FillDefaultFormatRulesGrid(Table) {
        try {
            $('#dataGridDFRule').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridDFRule').dataTable(); _lstRuleid = [];
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    var _arrcommt = Str(Table[i].RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str('');
                    cells[1] = Str(Table[i].DEFAULT_FORMAT);
                    cells[2] = Str(Table[i].RULE_NUMBER);
                    cells[3] = Str(Table[i].DOC_TYPE);
                    cells[4] = Str(Table[i].RULE_CODE);
                    cells[5] = Str(Table[i].RULE_DESC);
                    cells[6] = _arrcommt;
                    cells[7] = Str(Table[i].RULE_VALUE);
                    cells[8] = Str(Table[i].RULE_ID);
                    cells[9] = Str(Table[i].DEFFORMAT_RULEID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.DEFFORMAT_RULEID != undefined && Table.DEFFORMAT_RULEID != null) {
                    var t = $('#dataGridDFRule').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>'); 
                    var _arrcommt = Str(Table.RULE_COMMENTS).split(',').join('<br/>');
                    var cells = new Array();
                    cells[0] = Str('');
                    cells[1] = Str(Table.DEFAULT_FORMAT);
                    cells[2] = Str(Table.RULE_NUMBER);
                    cells[3] = Str(Table.DOC_TYPE);
                    cells[4] = Str(Table.RULE_CODE);
                    cells[5] = Str(Table.RULE_DESC);
                    cells[6] = _arrcommt;
                    cells[7] = Str(Table.RULE_VALUE);
                    cells[8] = Str(Table.RULE_ID);
                    cells[9] = Str(Table.DEFFORMAT_RULEID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
          
        }
        catch (e)
        { }
    };

    var GetAllDefaultFormats = function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultFormat.aspx/FillDefaultFormats",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var _lstres = response.d.split('||'); 
                    var Dataset = JSON.parse(_lstres[0]);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table;
                        _lstdeffrmt = [];
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) {
                                for (var i = 0; i < Table.length; i++) {
                                    _lstdeffrmt.push(Str(Table[i].DEFFORMATID) + "|" + Str(Table[i].DEFAULT_FORMAT));
                                }
                            }
                            else {
                                if (Table.DEFAULT_FORMAT != undefined) {
                                    _lstdeffrmt.push(Str(Table.DEFFORMATID) + "|" + Str(Table.DEFAULT_FORMAT));
                                }
                            }
                        }
                    }
                    var _DefFormat = FillCombo('', _lstdeffrmt); $('#cbDefaultFormat').empty().append(_DefFormat); //$('#cbDefaultFormat').select2();
                }
                catch (err) {
                    toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd"); 
                }
            },
            failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd");}
        });
       
    };

    var GetRulesGrid = function (DEFFORMATID) {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultFormatRules.aspx/GetUnlinkedRules",
            data: "{ 'DEFFORMATID':'" + DEFFORMATID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) {
                        Table = DataSet.NewDataSet.Table; FillRulesGrid(Table);
                    }
                    else $('#dataGridNewRule').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Rules :' + err); Metronic.unblockUI(); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };

    function FillRulesGrid(Table) {
        try {
            $('#dataGridNewRule').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridNewRule').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>'); var _arrcommt = Str(Table[i].RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str('');
                    cells[1] = Str(Table[i].RULE_NUMBER);
                    cells[2] = Str(Table[i].DOC_TYPE);
                    cells[3] = Str(Table[i].RULE_CODE);
                    cells[4] = Str(Table[i].RULE_DESC);
                    cells[5] = _arrcommt;
                    cells[6] = Str(Table[i].RULEID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.RULEID != undefined && Table.RULEID != null) {
                    var t = $('#dataGridNewRule').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>'); var _arrcommt = Str(Table.RULE_COMMENTS).split(',').join('<br/>');
                    var cells = new Array();
                    cells[0] = Str('');
                    cells[1] = Str(Table.RULE_NUMBER);
                    cells[2] = Str(Table.DOC_TYPE);
                    cells[3] = Str(Table.RULE_CODE);
                    cells[4] = Str(Table.RULE_DESC);
                    cells[5] = _arrcommt;
                    cells[6] = Str(Table.RULEID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    function SaveDefFormatRulesDetails(_nfieldval, callback) {
        var slDeformatRuledet = [];
        for (var j = 0; j < _nfieldval.length; j++) { slDeformatRuledet.push(_nfieldval[j]); }
        var data2send = JSON.stringify({ slDeformatRuledet: slDeformatRuledet });
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultFormatRules.aspx/SaveDefFormatRule",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") {  toastr.success("Lighthouse eSolutions Pte. Ltd", "DefaultFormat Rule Saved successfully.");   GetDefFormatRuleGrid();   }
                    Metronic.unblockUI();
                }
                catch (err) {
                    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update DefaultFormat Rule details :' + err); Metronic.unblockUI();
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

    function DeleteDefFormatRulesDetails(DEFFORMATRULEID, callback) {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultFormatRules.aspx/DeleteDefFormatRule",
            data: "{ 'DEFFORMATRULEID':'" + DEFFORMATRULEID + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") {  toastr.success("Lighthouse eSolutions Pte. Ltd", "DefaultFormat Rule Deleted."); GetDefFormatRuleGrid();  }
                    Metronic.unblockUI();
                }
                catch (err) {
                    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete DefaultFormat Rules :' + err); Metronic.unblockUI();
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

    function ClearFilter() { setFilterToolbar(); $('#dataGridDefFmt').DataTable().clear().draw(); };

    function ClearNewRuleModal() {  $('#dataGridNewRule').DataTable().clear().draw(); };

    return { init: function () { handleDefaultFormatRules(); } };
       
}(); 
