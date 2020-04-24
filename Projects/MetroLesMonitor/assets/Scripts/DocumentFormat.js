var _ADDRTYPE = ''; var _deffmtdet = []; var _uplDwlPath = ''; var _configpath = ''; var selectedTr = ''; var previousTr = ''; var selectedRuleTr = ''; var previousRuleTr = '';
var _lstdocfrmtrule = []; var _isCheckClick = '';

var DocumentFormat = function () {
   
    var handleDocumentFormat = function () {
        var _isdeffmtUpdate = -2; var _isdeffmtRUpdate = -2;  $('#divSpacer').remove(); $('#divFilterWrapper').remove(); $('#pageTitle').empty().append('Document Formats');
        SetupBreadcrumb('Home', 'Home.aspx', 'Document Formats', 'DocumentFormat.aspx','',''); $(document.getElementById('lnkFormat')).addClass('active open');
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
            "columnDefs": [{   'orderable': false,   "searching": true, "autoWidth": false,   'targets': [0] },
            { 'targets': [0], width: '5px', 'bSortable': false }, { 'targets': [1], width: '40px' }, { 'targets': [2,3], width: '150px', 'sClass': 'longText' }, 
            { 'targets': [4,5], visible: false }
            ],
            "lengthMenu": [[15, 30, 50, 100], [15, 30, 50, 100]],          
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var aedit = 'rwdedit' + nRow._DT_RowIndex; var aView = 'View' + nRow._DT_RowIndex;  var _divbtn = ' <div id="dvActn" style="text-align:center;">' +
                '<span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span></div>';
                if (_isdeffmtUpdate != nRow._DT_RowIndex) { $('td:eq(0)', nRow).html(_divbtn); }
                var detTag = '<a id="detid" href="javascript:;">' + Str(aData[1]) + '</a>'; $('td:eq(1)', nRow).html(detTag);
            }
        });

        $('#tblHeadRowDefFmt').addClass('gridHeader'); $('#ToolTables_dataGridDefFmt_0,#ToolTables_dataGridDefFmt_1').hide(); $('#dataGridDefFmt').css('width', '100%');
      
        GetDocFormatGrid();
        oDFmtTable.on('click', 'tbody td', function (e) {
            var selectedTr = $(this).parents('tr')[0]; var aData = oDFmtTable.fnGetData(selectedTr); _targetclick = '';
            if (oDFmtTable.fnIsOpen(selectedTr) && ((e.target.innerText == 'Edit'))) { oDFmtTable.fnClose(selectedTr);  }
            else {
                var divid = 'dv' + selectedTr._DT_RowIndex;
                if (e.target.className == 'glyphicon glyphicon-pencil') {_isdeffmtUpdate = selectedTr._DT_RowIndex; _targetclick = e.target.innerText;
                    if ((previousTr != '') && (oDFmtTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {
                        var prevdivid = 'dv' + previousTr._DT_RowIndex; oDFmtTable.fnClose(previousTr); $('#' + prevdivid).show();
                    }
                    if (oDFmtTable != null) {   oDFmtTable.fnOpen(selectedTr, fnDocumentFormatDetails(oDFmtTable, selectedTr, 'Edit', false), 'details'); $('#dvActn').hide(); previousTr = selectedTr; }
                }
                else if (e.target.id == 'detid') { if (aData != null) { $('#spnFormatid').text(Str(aData[1])); $('#spnFormatid').val(Str(aData[5])); } GetDocFormatRuleGrid(Str(aData[5])); $('#ModalDefFmtRuleDet').modal('show'); }
                $('#btnDefUpdate').click(function () {
                    if (aData != null) {  var _DefFmtid = aData[5]; _lstdeffrmt = []; var _res = Validate_NDefFormat(_DefFmtid);
                        if (_res == true) { 
                            _lstdeffrmt.push('DOCFORMATID|' + _DefFmtid); _lstdeffrmt.push('ADDR_TYPE|' + Str($('#cbAddrType').val())); _lstdeffrmt.push('DOCUMENT_FORMAT|' + Str($('#txtDocFormat').val()));
                            _lstdeffrmt.push('IMPORT_PATH|' + Str($('#txtDFImportPath').val())); _lstdeffrmt.push('EXPORT_PATH|' + Str($('#txtDFExportPath').val()));
                            SaveDocumentFormat(_lstdeffrmt, GetDocFormatGrid);_isdeffmtUpdate = -2;   $('#' + divid).show();
                        }
                    }
                });
                $('#btnDefCancel').click(function () { if (oDFmtTable.fnIsOpen(selectedTr)) { oDFmtTable.fnClose(selectedTr); } $('#dvActn').show(); _isdeffmtUpdate = -2; });
            }
        });

        $('#txtDocFormat').live("change", function (e) {
            var _defval = $('#txtDocFormat').val(); var _dfImppath = _configpath + _defval + "\\INBOX"; var _dfExppath = _configpath + _defval + "\\OUTBOX";
            $('#txtDFImportPath').val(_dfImppath); $('#txtDFExportPath').val(_dfExppath);
        });
        $('#btnAddFormat').click(function (e) {
            e.preventDefault();
            _deffmtdet = []; var _res = Validate_NDefFormat(0);
            if (_res == true) { _lstdeffrmt = []; _lstdeffrmt.push('DOCFORMATID|0');
                _lstdeffrmt.push('ADDR_TYPE|' + Str($('#cbAddrType').val())); _lstdeffrmt.push('DOCUMENT_FORMAT|' + Str($('#txtDocFormat').val()));
                _lstdeffrmt.push('IMPORT_PATH|' + Str($('#txtDFImportPath').val())); _lstdeffrmt.push('EXPORT_PATH|' + Str($('#txtDFExportPath').val()));
                SaveDocumentFormat(_lstdeffrmt, GetDocFormatGrid); $("#ModalNewFormat").modal('hide');
            }
        });
        $('#btnRefresh').live('click', function (e) { e.preventDefault(); GetDocFormatGrid(); });
        $('#btnClear').live('click', function (e) { e.preventDefault(); ClearFilter(); });
        $('#btnNew').live('click', function (e) {   e.preventDefault();  $('#dvNewFrmtDet').empty(); var divtag = fnDocumentFormatDetails(oDFmtTable, '', 'New', true); $('#dvNewFrmtDet').append(divtag); $("#ModalNewFormat").modal('show');  });
        $('#btnItmClose').on('click', function (e) { e.preventDefault(); $('#ModalDefFmtRuleDet').modal('hide'); });
        $('#btnItmRefresh').on('click', function (e) { e.preventDefault(); $('#ModalDefFmtRuleDet').modal('hide'); });
        $('#btnItmNew').on('click', function (e) { e.preventDefault(); var _docfrmtid = $('#spnFormatid').val(); GetRulesGrid(_docfrmtid); $('#ModalNewRule').modal('show'); });
        $('#btnNewRule').live('click', function (e) {
            e.preventDefault(); var Rulrrows = $('#dataGridNewRule').DataTable().rows().nodes(); var _deffmtval = $('#cbDefaultFormat option:selected').val(); var _deffmt = $('#cbDefaultFormat option:selected').text();
            for (var l = 0; l < Rulrrows.length; l++) {
                var rw = Rulrrows[l]; var tr = $(Rulrrows[l]);
                if ($('input[type="checkbox"]:checked', tr).length > 0) { _lstdocfrmtrule = []; var _docfmt = $('#spnFormatid').text(); var _docfmtid = $('#spnFormatid').val();
                    var _rulecode = oNRTable.fnGetData(tr)[3]; var _RULEid = oNRTable.fnGetData(tr)[6]; _isCheckClick = '1';
                    _lstdocfrmtrule.push('DOCUMENTFORMAT_RULEID|0'); _lstdocfrmtrule.push('DOCUMENT_FORMAT|' + _docfmt); _lstdocfrmtrule.push('DOCFORMATID|' + _docfmtid);
                    _lstdocfrmtrule.push('RULEID|' + _RULEid); _lstdocfrmtrule.push('RULE_CODE|' + _rulecode);
                    SaveDocFormatRulesDetails(_lstdocfrmtrule, GetDocFormatRuleGrid, Str($('#spnFormatid').val()), '1'); $("#ModalNewRule").modal('hide');
                }
            }
        });

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
            "columnDefs": [{   'orderable': false, "searching": true, "autoWidth": false, 'targets': [0], },
            { 'targets': [0], width: '5px' }, { 'targets': [1, 2], width: '40px' }, { 'targets': [3], width: '110px' },
            { 'targets': [4,5], width: '140px' }, { 'targets': [6], visible: false }
            ],
            "lengthMenu": [   [10, 25, 50, 100, -1],[10, 25, 50, 100, "All"]  ],
            "scrollX": '880px',
            "scrollY": '300px',
            "paging": false,
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var _chkid = "chk" + nRow._DT_RowIndex; var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="' + _chkid + '"  /></div>';
                $('td:eq(0)', nRow).html(_chkdiv);
            }
        });
        $('#tblHeadRowRule').addClass('gridHeader'); $('#ToolTables_dataGridNewRule_0,#ToolTables_dataGridNewRule_1,#dataGridNewRule_paginate').hide();

        setupDocFmtRulesTableHeader();
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
            "columnDefs": [{  'orderable': false, "searching": true, "autoWidth": false, 'targets': [0], },
            { 'targets': [0], width: '5px' }, { 'targets': [1, 2], width: '40px' }, { 'targets': [3], width: '70px' },
            { 'targets': [4, 5], width: '140px' }, { 'targets': [6], width: '40px' }, { 'targets': [7,8], visible: false }
            ],
            "lengthMenu": [ [10, 25, 50, 100, -1], [10, 25, 50, 100, "All"] ],
            "scrollX": '800px',
            "scrollY": '300px',
            "paging": false,
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var aedit = 'Edit' + nRow._DT_RowIndex; var adelete = 'Delete' + nRow._DT_RowIndex; var _divbtn = ' <div id="dvActn" style="text-align:center;">' +
                '<span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span>'+
                '<span id=' + adelete + ' class="actionbtn" data-toggle="tooltip" title="Delete"><i class="glyphicon glyphicon-trash"></i></span></div>';
                if (_isdeffmtRUpdate != nRow._DT_RowIndex) { $('td:eq(0)', nRow).html(_divbtn); } $("td:eq(6)", nRow).css('text-align','center');
            }
        });
        $('#tblHeadRowDFRule').addClass('gridHeader'); $('#ToolTables_dataGridDFRule_0,#ToolTables_dataGridDFRule_1,#dataGridDFRule_paginate').hide();
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");

        oDFRTable.on('click', 'tbody td', function (e) {
            var selectedRuleTr = $(this).parents('tr')[0]; var aData = oDFRTable.fnGetData(selectedRuleTr); _targetclick = '';
            if (oDFRTable.fnIsOpen(selectedRuleTr)) { oDFRTable.fnClose(selectedRuleTr); }
            else {
                var divid = 'dv' + selectedTr._DT_RowIndex;
                if (e.target.className == 'glyphicon glyphicon-pencil') { isdeffmtUpdate = selectedRuleTr._DT_RowIndex; _targetclick = e.target.innerText;
                    if ((previousRuleTr != '') && (oDFRTable.fnIsOpen(previousRuleTr) && previousRuleTr != selectedRuleTr)) {
                        var prevdivid = 'dv' + previousTr._DT_RowIndex; oDFRTable.fnClose(previousRuleTr); $('#' + prevdivid).show();
                    }
                    if (previousRuleTr != '' && previousRuleTr != selectedRuleTr) { restoreRow(previousRuleTr, oDFRTable); }
                    nNew = false; editRow(oDFRTable, selectedRuleTr); previousRuleTr = selectedRuleTr;
                }
                else if (e.target.className == 'glyphicon glyphicon-trash') {
                    var _DocFmtRuleid = aData[8];
                    var deleteRule = confirm('Are you sure? You want to delete this rule?');
                    if (deleteRule) { DeleteDocFormatRulesDetails(_DocFmtRuleid, GetDocFormatRuleGrid, Str($('#spnFormatid').val()));}
                }
                else if (e.target.className == 'glyphicon glyphicon-floppy-save') {
                    _lstdocfrmtrule = [];
                    var _docfmt = $('#spnFormatid').text(); var _rulecode = aData[3]; var _DocFmtRuleid = aData[8]; var _ruleval = $('#cbRuleValue option:selected').val(); _lstdocfrmtrule.push('RULE_VALUE|' + _ruleval);
                    _lstdocfrmtrule.push('DOCUMENTFORMAT_RULEID|' + _DocFmtRuleid); _lstdocfrmtrule.push('DOCUMENT_FORMAT|' + _docfmt); _lstdocfrmtrule.push('RULE_CODE|' + _rulecode);
                    SaveDocFormatRulesDetails(_lstdocfrmtrule, GetDocFormatRuleGrid, Str($('#spnFormatid').val()),'0'); previousRuleTr = '';
                }
                else if (e.target.className == 'glyphicon glyphicon-ban-circle') {
                    restoreRow(selectedRuleTr, oDFRTable); previousRuleTr = '';
                }              
            }
        });

        function restoreRow(nRow, oTable) {
            var aData = oTable.fnGetData(nRow); var jqTds = $('>td', nRow);
            for (var i = 0, iLen = jqTds.length; i < iLen; i++) { if (i == 0) { } else { oTable.fnUpdate(jqTds[i].innerHTML, nRow, i, false); } } oTable.fnDraw();
        };

        function gridCancelEdit(nNew, nEditing, oTable) {
            var nRow = $('.DTTT_selected'); if (nNew) { oTable.fnDeleteRow(nEditing); nNew = false; } else { nRow = nEditing; restoreRow(nRow, oTable); $(nRow).removeClass("DTTT_selected selected"); }
        };

        function editRow(oTable, nRow) {
            var aData = oTable.fnGetData(nRow); nEditing = nRow; var jqTds = $('>td', nRow);
            var _lstRuleval = []; _lstRuleval.push("0|0"); _lstRuleval.push("1|1");
            var detTag = '<div style="text-align:center;"><span class="actionbtn" data-toggle="tooltip" title="Update"><i class="glyphicon glyphicon-floppy-save"></i></span>' +
               ' <span class="actionbtn" data-toggle="tooltip" title="Cancel"><i class="glyphicon glyphicon-ban-circle"></i></span></div>';
            jqTds[0].innerHTML = detTag;
            jqTds[6].innerHTML = '<select id="cbRuleValue" class="fullWidth selectheight">' + FillCombo(aData[6], _lstRuleval) + '</select>';  //$('#cbRuleValue').select2();
        };
    };

    var GetDocFormatGrid = function () {
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "DocumentFormat.aspx/FillDocumentFormats",
                data:'{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var _lstres = response.d.split('||'); _uplDwlPath = _lstres[1]; _configpath = _lstres[2];
                        var DataSet = JSON.parse(_lstres[0]);
                        if (DataSet.NewDataSet != null) {   Table = DataSet.NewDataSet.Table; FillDocumentFormatGrid(Table);  }
                        else $('#dataGridDefFmt').DataTable().clear().draw();
                    }
                    catch (err) {  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Document Format :' + err); }
                },
                failure: function (response) {  toastr.error("failure get", response.d); },
                error: function (response) {  toastr.error("error get", response.responseText);  }
            });
        }, 200);      
    };

    function FillDocumentFormatGrid(Table) {
        try {
            $('#dataGridDefFmt').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridDefFmt').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str('');cells[1] = Str(Table[i].DOCUMENT_FORMAT);  cells[2] = Str(Table[i].IMPORT_PATH);
                    cells[3] = Str(Table[i].EXPORT_PATH); cells[4] = Str(Table[i].ADDR_TYPE);  cells[5] = Str(Table[i].DOCFORMATID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.DOCFORMATID != undefined && Table.DOCFORMATID != null) {
                    var t = $('#dataGridDefFmt').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str('');  cells[1] = Str(Table.DOCUMENT_FORMAT);   cells[2] = Str(Table.IMPORT_PATH);
                    cells[3] = Str(Table.EXPORT_PATH);cells[4] = Str(Table.ADDR_TYPE);   cells[5] = Str(Table.DOCFORMATID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    var GetDocFormatDetail = function (ID) {
        $.ajax({
            type: "POST",
            async: false,
            url: "DocumentFormat.aspx/GetDocumentFormatDetails",
            data: "{'ID':'" + ID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {                    
                    _sldfdet = JSON.parse(response.d);
                }
                catch (err) {  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Document Format Details :' + err);}
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
        return _sldfdet;
    };
  
    function fnDocumentFormatDetails(oTable, nTr, _targetclick,isModal) {
        var sOut = ''; var _code = ''; var _ExpPath = ''; var _ImpPath = ''; var _Addrtype = ''; var tid = "Table"; var _tbodyid = "tblBody"; var _codeid = 'txtDocFormat';
        var _imppathid = 'txtDFImportPath'; var _exppathid = 'txtDFExportPath'; var _addrtypeid = 'cbAddrType'; var _defclass = '';
        var _lstaddrtype = []; _lstaddrtype.push('Buyer|Buyer'); _lstaddrtype.push('Supplier|Supplier')
        if (_targetclick == 'Edit') {
            var aData = oTable.fnGetData(nTr); _code = Str(aData[1]); _ExpPath = Str(aData[3]); _ImpPath = Str(aData[2]); _Addrtype = Str(aData[4]); _defclass = 'col-md-10';
        } else { _code = ''; _ExpPath = ''; _ImpPath = ''; _Addrtype = ''; _defclass = 'col-md-12';}
        _Addrtype = FillCombo(_Addrtype, _lstaddrtype); $('#cbAddrType').select2();
        var sOut = '<div class="row"> <div class="' + _defclass + '"><div class="row"><div class="col-md-12"><div class="form-group">' +
              ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Document Format <span class="required" aria-required="true"> * </span> </label> </div>'+
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

    function SaveDocumentFormat(_nfieldval, callback) {
        var slDocformatdet = []; for (var j = 0; j < _nfieldval.length; j++) { slDocformatdet.push(_nfieldval[j]); }
        var data2send = JSON.stringify({ slDocformatdet: slDocformatdet });
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "DocumentFormat.aspx/SaveDocumentFormat",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d); callback(); toastr.success("Lighthouse eSolutions Pte. Ltd.", 'Document Format saved successfully.');
                    Metronic.unblockUI();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to Save Document Format:' + err); Metronic.unblockUI(); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };

    function Validate_NDefFormat(id) {
        var _valid = true; var _fmt = $('#txtDocFormat').val(); var _imppath = $('#txtDFImportPath').val(); var _exppth = $('#txtDFExportPath').val();
        var _addrtype = $('#cbAddrType').val();
        if (_fmt == '') { $('#txtDocFormat').addClass('error'); _valid = false; }
        else {
            if (id == 0) { var isexist = CheckExistingFormat(_fmt); if (isexist == '') { $('#txtDocFormat').removeClass('error'); } else { $('#txtDocFormat').addClass('error'); toastr.error("Lighthouse eSolutions Pte. Ltd.", isexist); _valid = false; } }
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
            url: 'DocumentFormat.aspx/CheckExistingFormat',
            data: "{ 'FORMAT':'" + FORMAT + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    res = Str(response.d);
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Validation failure' + err);}
            },
            failure: function (response) {toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
        return res;
    };

    var setupTableHeader = function () {
        var dtfilter = '<th style="text-align:center;">#</th><th>Format</th><th>Import Path</th><th>Export Path</th><th>Address Type</th><th>DOCFORMATID</th>';
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
  
    var setupDocFmtRulesTableHeader = function () {
        var dtfilter = '<th style="text-align:center;">#</th><th>Rule No.</th><th>Doc Type</th><th>Rule Code</th><th>Description</th><th>Comments</th><th>Rule Value</th><th>RULEID</th><th>DEFFORMAT_RULEID</th>'; $('#tblHeadRowDFRule').empty().append(dtfilter); $('#tblBodyDFRule').empty();
    };

    var GetDocFormatRuleGrid = function (DOCFORMATID) {
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "DocumentFormat.aspx/GetAssigned_DocumentFormatRules",
                data: "{ 'DOCFORMATID':'" + DOCFORMATID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var DataSet = JSON.parse(response.d);
                        if (DataSet.NewDataSet != null) {  Table = DataSet.NewDataSet.Table; FillDocumentFormatRulesGrid(Table); }
                        else $('#dataGridDFRule').DataTable().clear().draw();
                    }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get DocumentFormat Rules :' + err); }
                },
                failure: function (response) { toastr.error("failure get", response.d); },
                error: function (response) { toastr.error("error get", response.responseText); }
            });
        }, 200);
    };

    function FillDocumentFormatRulesGrid(Table) {
        try {
            $('#dataGridDFRule').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridDFRule').dataTable(); _lstRuleid = [];
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    var _arrcommt = Str(Table[i].RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str(''); cells[1] = Str(Table[i].RULE_NUMBER); cells[2] = Str(Table[i].DOC_TYPE); cells[3] = Str(Table[i].RULE_CODE);
                    cells[4] = Str(Table[i].RULE_DESC); cells[5] = _arrcommt; cells[6] = Str(Table[i].RULE_VALUE); cells[7] = Str(Table[i].RULE_ID);
                    cells[8] = Str(Table[i].DOCUMENTFORMAT_RULEID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.DOCUMENTFORMAT_RULEID != undefined && Table.DOCUMENTFORMAT_RULEID != null) {
                    var t = $('#dataGridDFRule').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var _arrcommt = Str(Table.RULE_COMMENTS).split(',').join('<br/>');
                    var cells = new Array();
                    cells[0] = Str(''); cells[1] = Str(Table.RULE_NUMBER); cells[2] = Str(Table.DOC_TYPE); cells[3] = Str(Table.RULE_CODE);
                    cells[4] = Str(Table.RULE_DESC); cells[5] = _arrcommt; cells[6] = Str(Table.RULE_VALUE); cells[7] = Str(Table.RULE_ID);
                    cells[8] = Str(Table.DOCUMENTFORMAT_RULEID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    var GetAllDocumentFormats = function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "DocumentFormat.aspx/FillDocumentFormats",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var _lstres = response.d.split('||'); var Dataset = JSON.parse(_lstres[0]);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table;  _lstdeffrmt = [];
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) {
                                for (var i = 0; i < Table.length; i++) { _lstdeffrmt.push(Str(Table[i].DOCFORMATID) + "|" + Str(Table[i].DOCUMENT_FORMAT));  }
                            }
                            else {
                                if (Table.DEFAULT_FORMAT != undefined) {  _lstdeffrmt.push(Str(Table.DOCFORMATID) + "|" + Str(Table.DOCUMENT_FORMAT)); }
                            }
                        }
                    }                  
                }
                catch (err) {toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd");}
            },
            failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
        });

    };

    var setupNewRulesTableHeader = function () {
        var dtfilter = '<th></th><th>Rule No.</th><th>Doc Type</th><th>Rule Code</th><th>Description</th><th>Comments</th><th>RULEID</th>'; $('#tblHeadRowRule').empty().append(dtfilter); $('#tblBodyRule').empty();
    };

    var GetRulesGrid = function (DOCFORMATID) {
        $.ajax({
            type: "POST",
            async: false,
            url: "DocumentFormat.aspx/GetUnlinkedRules",
            data: "{ 'DOCFORMATID':'" + DOCFORMATID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table; FillRulesGrid(Table);  }
                    else $('#dataGridNewRule').DataTable().clear().draw();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Rules :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d);},
            error: function (response) { toastr.error("error get", response.responseText);}
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
                    cells[0] = Str(''); cells[1] = Str(Table[i].RULE_NUMBER);  cells[2] = Str(Table[i].DOC_TYPE);
                    cells[3] = Str(Table[i].RULE_CODE); cells[4] = Str(Table[i].RULE_DESC); cells[5] = _arrcommt;  cells[6] = Str(Table[i].RULEID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.RULEID != undefined && Table.RULEID != null) {
                    var t = $('#dataGridNewRule').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>'); var _arrcommt = Str(Table.RULE_COMMENTS).split(',').join('<br/>');
                    var cells = new Array();
                    cells[0] = Str(''); cells[1] = Str(Table.RULE_NUMBER); cells[2] = Str(Table.DOC_TYPE);
                    cells[3] = Str(Table.RULE_CODE);  cells[4] = Str(Table.RULE_DESC); cells[5] = _arrcommt; cells[6] = Str(Table.RULEID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    function SaveDocFormatRulesDetails(_nfieldval, callback, DOCFORMATID,ischkclick) {
        var slDocformatRuledet = [];   for (var j = 0; j < _nfieldval.length; j++) { slDocformatRuledet.push(_nfieldval[j]); }
        var data2send = JSON.stringify({ slDocformatRuledet: slDocformatRuledet });
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "DocumentFormat.aspx/SaveDocFormatRule",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "" && ischkclick == '0') { toastr.success("Lighthouse eSolutions Pte. Ltd", "DocumentFormat Rule Saved successfully."); }
                    callback(DOCFORMATID); Metronic.unblockUI();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update DocumentFormat Rule details :' + err); Metronic.unblockUI();  }
            },
            failure: function (response) {toastr.error("failure get", response.d); Metronic.unblockUI();  },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };

    function DeleteDocFormatRulesDetails(DOCFORMATRULEID, callback, DOCFORMATID) {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "DocumentFormat.aspx/DeleteDocFormatRule",
            data: "{ 'DOCFORMATRULEID':'" + DOCFORMATRULEID + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "DocumentFormat Rule Deleted."); callback(DOCFORMATID); }
                    Metronic.unblockUI();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete DocumentFormat Rules :' + err); Metronic.unblockUI(); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI();}
        });
    };
    /**/

    function ClearFilter() { setFilterToolbar(); $('#dataGridDefFmt').DataTable().clear().draw(); };

    return { init: function () { handleDocumentFormat(); } };
       
}(); 
