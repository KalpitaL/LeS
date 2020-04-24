var _ADDRTYPE = ''; var _deffmtdet = []; var _uplDwlPath = ''; var _configpath = ''; var selectedTr = ''; var previousTr = '';

var DefaultFormat = function () {
   
    var handleDefaultFormat = function () {
        var _isdeffmtUpdate = -2; var _isdeffmtRUpdate = -2;
        $('#divSpacer').remove(); $('#divFilterWrapper').remove(); $('#pageTitle').empty().append('Default Format');
        SetupBreadcrumb('Home', 'Home.aspx', 'Format', '#', 'Default Format', 'DefaultFormat.aspx');
        $(document.getElementById('lnkFormat')).addClass('active open'); $(document.getElementById('spDefFrmt')).addClass('title font-title SelectedColor');
        $(document.getElementById('spDefFrmt')).text('Default Format');
        setupTableHeader(); setFilterToolbar();
        var table = $('#dataGridDefFmt');
        var oDFmtTable = table.dataTable({
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
                'orderable': false,
                "searching": true,
                "autoWidth": false,
                'targets': [0]
            },
            { 'targets': [0], width: '5px', 'bSortable': false }, { 'targets': [1], width: '40px' }, { 'targets': [2,3], width: '150px', 'sClass': 'longText' }, 
            { 'targets': [4,5], visible: false }
            ],
            "lengthMenu": [[15, 30, 50, 100], [15, 30, 50, 100]],          
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var aedit = 'rwdedit' + nRow._DT_RowIndex; var aView = 'View' + nRow._DT_RowIndex;
                var _divbtn = ' <div id="dvActn" style="text-align:center;">' +
                '<span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span></div>';
                if (_isdeffmtUpdate != nRow._DT_RowIndex) { $('td:eq(0)', nRow).html(_divbtn); }
            }
        });

        $('#tblHeadRowDefFmt').addClass('gridHeader'); $('#ToolTables_dataGridDefFmt_0,#ToolTables_dataGridDefFmt_1').hide(); $('#dataGridDefFmt').css('width', '100%');
      
        GetDefFormatGrid();
        oDFmtTable.on('click', 'tbody td', function (e) {
            var selectedTr = $(this).parents('tr')[0]; var aData = oDFmtTable.fnGetData(selectedTr); _targetclick = '';
            if (oDFmtTable.fnIsOpen(selectedTr) && ((e.target.innerText == 'Edit'))) { oDFmtTable.fnClose(selectedTr);  }
            else {
                var divid = 'dv' + selectedTr._DT_RowIndex;
                if (e.target.className == 'glyphicon glyphicon-pencil') {
                    _isdeffmtUpdate = selectedTr._DT_RowIndex; _targetclick = e.target.innerText;
                    if ((previousTr != '') && (oDFmtTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {
                        var prevdivid = 'dv' + previousTr._DT_RowIndex; oDFmtTable.fnClose(previousTr); $('#' + prevdivid).show();
                    }
                    if (oDFmtTable != null) {
                        oDFmtTable.fnOpen(selectedTr, fnDefaultFormatDetails(oDFmtTable, selectedTr, 'Edit',false), 'details');   $('#' + divid).hide(); previousTr = selectedTr;
                    }
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
                $('#btnDefCancel').click(function () { if (oDFmtTable.fnIsOpen(selectedTr)) { oDFmtTable.fnClose(selectedTr); } $('#' + divid).show(); _isdeffmtUpdate = -2; });
            }
        });

        $('#txtDefFormat').live("change", function (e) {
            var _defval = $('#txtDefFormat').val(); var _dfImppath = _configpath + _defval + "\\INBOX"; var _dfExppath = _configpath + _defval + "\\OUTBOX";
            $('#txtDFImportPath').val(_dfImppath); $('#txtDFExportPath').val(_dfExppath);
        });
        $('#btnAddFormat').click(function (e) {
            e.preventDefault();
            _deffmtdet = []; var _res = Validate_NDefFormat();
            if (_res == true) {  _lstdeffrmt = []; _lstdeffrmt.push('DEFAULTFORMATID|0');
                _lstdeffrmt.push('ADDR_TYPE|' + Str($('#cbAddrType').val())); _lstdeffrmt.push('DEFAULT_FORMAT|' + Str($('#txtDefFormat').val()));
                _lstdeffrmt.push('IMPORT_PATH|' + Str($('#txtDFImportPath').val())); _lstdeffrmt.push('EXPORT_PATH|' + Str($('#txtDFExportPath').val()));
                SaveDefaultFormat(_lstdeffrmt, GetDefFormatGrid); $("#ModalNewFormat").modal('hide');
            }
        });
        $('#btnRefresh').live('click', function (e) { e.preventDefault(); GetDefFormatGrid(); });
        $('#btnClear').live('click', function (e) { e.preventDefault(); ClearFilter(); });
        $('#btnNew').live('click', function (e) {   e.preventDefault();  $('#dvNewFrmtDet').empty(); var divtag = fnDefaultFormatDetails(oDFmtTable, '', 'New', true); $('#dvNewFrmtDet').append(divtag); $("#ModalNewFormat").modal('show');  });
        $('#btnItmClose').on('click', function (e) { e.preventDefault(); $('#ModalDefFmtRuleDet').modal('hide'); });
        $('#btnItmRefresh').on('click', function (e) { e.preventDefault(); $('#ModalDefFmtRuleDet').modal('hide'); });
        $('#btnItmNew').on('click', function (e) { e.preventDefault(); $('#ModalNewRule').modal('show'); });

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
            { 'targets': [0], width: '5px' }, { 'targets': [1, 2], width: '40px' }, { 'targets': [3], width: '110px' },
            { 'targets': [4], width: '140px' },
            { 'targets': [5], visible: false },
            ],
            "lengthMenu": [
                [10, 25, 50, 100, -1],
                [10, 25, 50, 100, "All"] // change per page values here
            ],
            "scrollX": '880px',
            "scrollY": (screen.availHeight - 386) + "px",
            "paging": false,
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var _chkid = "chk" + nRow._DT_RowIndex; var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="' + _chkid + '"  /></div>';
                $('td:eq(0)', nRow).html(_chkdiv);
            }
        });
        $('#tblHeadRowRule').addClass('gridHeader'); $('#ToolTables_dataGridRule_0,#ToolTables_dataGridRule_1,#dataGridRule_paginate').hide(); 

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
            { 'targets': [0], width: '5px' }, { 'targets': [1, 2], width: '40px' }, { 'targets': [3], width: '70px' },
            { 'targets': [4,5], width: '140px' }, { 'targets': [6,7], visible: false }
            ],
            "lengthMenu": [
                [10, 25, 50, 100, -1],
                [10, 25, 50, 100, "All"] // change per page values here
            ],
            "scrollX": '880px',
            "scrollY": (screen.availHeight - 386) + "px",
            "paging": false,
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var aedit = 'Edit' + nRow._DT_RowIndex; var adelete = 'Delete' + nRow._DT_RowIndex;
                var _divbtn = ' <div id="dvActn" style="text-align:center;">' +
               // '<span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span>'+
                '<span id=' + adelete + ' class="actionbtn" data-toggle="tooltip" title="Delete"><i class="glyphicon glyphicon-trash"></i></span></div>';
                if (_isdeffmtRUpdate != nRow._DT_RowIndex) { $('td:eq(0)', nRow).html(_divbtn); }
            }
        });
        $('#tblHeadRowDFRule').addClass('gridHeader'); $('#ToolTables_dataGridDFRule_0,#ToolTables_dataGridDFRule_1,#dataGridDFRule_paginate').hide();
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");

    };

    var GetDefFormatGrid = function () {
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "DefaultFormat.aspx/FillDefaultFormats",
                data:'{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var _lstres = response.d.split('||'); _uplDwlPath = _lstres[1]; _configpath = _lstres[2];
                        var DataSet = JSON.parse(_lstres[0]);
                        if (DataSet.NewDataSet != null) {
                            Table = DataSet.NewDataSet.Table; FillDefaultFormatGrid(Table);
                        }
                        else $('#dataGridDefFmt').DataTable().clear().draw();
                    }
                    catch (err) {
                        toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Default Format :' + err);
                    }
                },
                failure: function (response) {
                    toastr.error("failure get", response.d);
                },
                error: function (response) {
                    toastr.error("error get", response.responseText);
                }
            });
        }, 200);      
    };

    function FillDefaultFormatGrid(Table) {
        try {
            $('#dataGridDefFmt').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridDefFmt').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str('');
                    cells[1] = Str(Table[i].DEFAULT_FORMAT);
                    cells[2] = Str(Table[i].IMPORT_PATH);
                    cells[3] = Str(Table[i].EXPORT_PATH);
                    cells[4] = Str(Table[i].ADDR_TYPE);
                    cells[5] = Str(Table[i].DEFFORMATID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.DEFFORMATID != undefined && Table.DEFFORMATID != null) {
                    var t = $('#dataGridDefFmt').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str('');
                    cells[1] = Str(Table.DEFAULT_FORMAT);
                    cells[2] = Str(Table.IMPORT_PATH);
                    cells[3] = Str(Table.EXPORT_PATH);
                    cells[4] = Str(Table.ADDR_TYPE);
                    cells[5] = Str(Table.DEFFORMATID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    var GetDefFormatDetail = function (ID) {
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultFormat.aspx/GetDefaultFormatDetails",
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
  
    function fnDefaultFormatDetails(oTable, nTr, _targetclick,isModal) {
        var sOut = ''; var _code = ''; var _ExpPath = ''; var _ImpPath = ''; var _Addrtype = ''; var tid = "Table"; var _tbodyid = "tblBody"; var _codeid = 'txtDefFormat';
        var _imppathid = 'txtDFImportPath'; var _exppathid = 'txtDFExportPath'; var _addrtypeid = 'cbAddrType'; var _defclass = '';
        var _lstaddrtype = []; _lstaddrtype.push('Buyer|Buyer'); _lstaddrtype.push('Supplier|Supplier')
        if (_targetclick == 'Edit') {
            var aData = oTable.fnGetData(nTr); _code = Str(aData[1]); _ExpPath = Str(aData[3]); _ImpPath = Str(aData[2]); _Addrtype = Str(aData[4]); _defclass = 'col-md-10';
        } else { _code = ''; _ExpPath = ''; _ImpPath = ''; _Addrtype = ''; _defclass = 'col-md-12';}
        _Addrtype = FillCombo(_Addrtype, _lstaddrtype); $('#cbAddrType').select2();
        var sOut = '<div class="row"> <div class="' + _defclass + '"><div class="row"><div class="col-md-12"><div class="form-group">' +
              ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Default Format <span class="required" aria-required="true"> * </span> </label> </div>'+
              ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _codeid + '" value="' + _code + '"/></div>'+
              ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Address Type <span class="required" aria-required="true"> * </span> </label> </div>'+
              ' <div  class="col-md-3"><select class="form-control" id="' + _addrtypeid + '">' + _Addrtype + '</select></div> ' +
              ' </div></div></div>'+
              ' <div class="row" style="margin-top:3px;"><div class="col-md-12"><div class="form-group">'+
              ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Import Path </label> </div>'+
              ' <div  class="col-md-9"><input type="text" class="form-control" id="' +_imppathid + '" value="' + _ImpPath + '"/></div>'+
              ' </div></div></div><div class="row" style="margin-top:3px;"><div class="col-md-12"><div class="form-group">'+
              ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Export Path </label> </div>'+
              ' <div  class="col-md-9"><input type="text" class="form-control" id="' + _exppathid + '" value="' + _ExpPath + '"/></div>' +
              ' </div></div></div>';
        var btndiv = '<div class="row"><div style="text-align:center;"><a href="#" id="btnDefUpdate"><u>Update</u></<a>&nbsp;<a href="#" id="btnDefCancel"><u>Cancel</u></<a></div></div>';
        if (!isModal) { sOut += btndiv; } sOut += '</div></div>';
        return sOut;
    }

    function SaveDefaultFormat(_nfieldval,callback) {
        var slDeformatdet = []; for (var j = 0; j < _nfieldval.length; j++) { slDeformatdet.push(_nfieldval[j]); }      
        var data2send = JSON.stringify({ slDeformatdet: slDeformatdet});
        Metronic.blockUI('#portlet_body');
            $.ajax({
                type: "POST",
                async: false,
                url: "DefaultFormat.aspx/SaveDefaultFormat",
                data: data2send,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d); GetDefFormatGrid();
                        toastr.success("Lighthouse eSolutions Pte. Ltd.", 'Default Format saved successfully.');
                        Metronic.unblockUI();
                    }
                    catch (err) {
                        toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to Save Default Format:' + err); Metronic.unblockUI();
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

    function Validate_NDefFormat() {
        var _valid = true; var _fmt = $('#txtDefFormat').val(); var _imppath = $('#txtDFImportPath').val(); var _exppth = $('#txtDFExportPath').val();
        var _addrtype = $('#cbAddrType').val();
        if (_fmt == '') { $('#txtDefFormat').addClass('error'); _valid = false; }
        else {
            var isexist = CheckExistingFormat(_fmt); if (isexist == '') { $('#txtDefFormat').removeClass('error'); } else { $('#txtDefFormat').addClass('error'); toastr.error("Lighthouse eSolutions Pte. Ltd.", isexist); _valid = false; }
            if (_exppth == '') { $('#txtDFExportPath').addClass('error'); _valid = false; } else { $('#txtDFExportPath').removeClass('error'); }
            if (_imppath == '') { $('#txtDFImportPath').addClass('error'); _valid = false; } else { $('#txtDFImportPath').removeClass('error'); }
        }
        if (_addrtype == '') { $('#cbAddrType').addClass('error'); _valid = false; }
        return _valid;
    };

    var CheckExistingFormat = function (FORMAT) {
        var res = '';        
        $.ajax({
            type: "POST",
            async: false,
            url: 'DefaultFormat.aspx/CheckExistingFormat',
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

    var setupTableHeader = function () {
        var dtfilter = '<th style="text-align:center;">#</th><th>Format</th><th>Import Path</th><th>Export Path</th><th>Address Type</th><th>DEFFORMATID</th>';
        $('#tblHeadRowDefFmt').empty().append(dtfilter); $('#tblBodyDefFmt').empty();
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
        var _itmbtns = '<span title="Close" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnClose"><a href="javascript:;" id="btnItmClose" class="toolbtn"><i class="fa fa-times" style="text-align:center;"></i></a></div></span>' +          
        '<span title="New" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnNew"><a href="javascript:;" id="btnItmNew" class="toolbtn"><i class="fa fa-plus" style="text-align:center;"></i></a></div></span>' +
        '<span title="Refresh" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnRefresh"><a href="javascript:;" class="toolbtn" id="btnItmRefresh"><i class="fa fa-recycle" style="text-align:center;"></i></a></div></span>';
        $('#Itemtoolbtngroup').append(_itmbtns);
    };

    /* Rules */

    var setupNewRulesTableHeader = function () { var dtfilter = '<th></th><th>Rule No.</th><th>Doc Type</th><th>Rule Code</th><th>Rule Desc</th><th>RULEID</th>'; $('#tblHeadRowRule').empty().append(dtfilter); $('#tblBodyRule').empty(); };

    var setupDefFmtRulesTableHeader = function () {
        var dtfilter = '<th style="text-align:center;">#</th><th>Rule No.</th><th>Doc Type</th><th>Rule Code</th><th>Description</th><th>Comments</th><th>RULEID</th><th>DEFFORMAT_RULEID</th>'; $('#tblHeadRowDFRule').empty().append(dtfilter); $('#tblBodyDFRule').empty();
    };

    var GetDefFormatRuleGrid = function (DEFFORMATID) {
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultFormat.aspx/GetDefaultFormat_Rules",
            data: "{ 'DEFFORMATID':'" + DEFFORMATID + "'}",
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
    };

    function FillDefaultFormatRulesGrid(Table) {
        try {
            $('#dataGridDFRule').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridDFRule').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str('');
                    cells[1] = Str(Table[i].RULE_NUMBER);
                    cells[2] = Str(Table[i].DOC_TYPE);
                    cells[3] = Str(Table[i].RULE_CODE);
                    cells[4] = Str(Table[i].RULE_DESC);
                    cells[5] = Str(Table[i].RULE_COMMENTS);
                    cells[6] = Str(Table[i].RULE_ID);
                    cells[7] = Str(Table[i].DEFFORMAT_RULEID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.DEFFORMAT_RULEID != undefined && Table.DEFFORMAT_RULEID != null) {
                    var t = $('#dataGridDFRule').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str('');
                    cells[1] = Str(Table.RULE_NUMBER);
                    cells[2] = Str(Table.DOC_TYPE);
                    cells[3] = Str(Table.RULE_CODE);
                    cells[4] = Str(Table.RULE_DESC);
                    cells[5] = Str(Table.RULE_COMMENTS);
                    cells[6] = Str(Table.RULE_ID);
                    cells[7] = Str(Table.DEFFORMAT_RULEID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    var GetRulesGrid = function (slRuleId) {
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultFormat.aspx/GetDefaultFormat_Rules",
            data: "{ 'DEFFORMATID':'" + DEFFORMATID + "'}",
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
    };

    function FillRulesGrid(Table) {
        try {
            $('#dataGridDFRule').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridDFRule').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str('');
                    cells[1] = Str(Table[i].RULE_NUMBER);
                    cells[2] = Str(Table[i].DOC_TYPE);
                    cells[3] = Str(Table[i].RULE_CODE);
                    cells[4] = Str(Table[i].RULE_DESC);
                    cells[5] = Str(Table[i].RULE_COMMENTS);
                    cells[6] = Str(Table[i].RULE_ID);
                    cells[7] = Str(Table[i].DEFFORMAT_RULEID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.DEFFORMAT_RULEID != undefined && Table.DEFFORMAT_RULEID != null) {
                    var t = $('#dataGridDFRule').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str('');
                    cells[1] = Str(Table.RULE_NUMBER);
                    cells[2] = Str(Table.DOC_TYPE);
                    cells[3] = Str(Table.RULE_CODE);
                    cells[4] = Str(Table.RULE_DESC);
                    cells[5] = Str(Table.RULE_COMMENTS);
                    cells[6] = Str(Table.RULE_ID);
                    cells[7] = Str(Table.DEFFORMAT_RULEID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    /**/

    function ClearFilter() { setFilterToolbar(); $('#dataGridDefFmt').DataTable().clear().draw(); };

    return { init: function () { handleDefaultFormat(); } };
       
}(); 
