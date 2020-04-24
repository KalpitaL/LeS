var _lstDoc = []; var selectedTr = ''; var previousTr = ''; var previousBSTr = ''; var _defrule = []; var _lstGrpFmt = []; var _lstAddrName = []; var _lstRules = [];
var _lstRuleval = []; var nNew1 = 0; var _lstMasterDet = []; var _newcont = 0;

var DefaultRules = function () {

    var handleDefaultRulesTable = function () {
        var nEditing = null; var nNew = false; var _isRuleupdate = -2; $('#pageTitle').empty().append('Default Rules');
        SetupBreadcrumb('Home', 'Home.aspx', 'Rules -', '#', 'Default Rules', 'DefaultRules.aspx');
        $(document.getElementById('lnkRuledet')).addClass('active open'); $(document.getElementById('spDefRule')).addClass('title font-title SelectedColor');              
        setupTableHeader();

        var table = $('#dataGridDefRule');
        var oDRTable = table.dataTable({
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
            "columnDefs": [{   'orderable': false, "searching": true,"autoWidth": false,'targets': [0]
            },
            {'targets': [0], width: '5px','bSortable':false },{ 'targets': [2], width: '20px' }, { 'targets': [3,4], width: '100px' }, {'targets': [1,5,6,7], visible: false},
             ],            
            "lengthMenu": [  [25, 50, 100, -1],  [25, 50, 100, "All"]  ],      
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' });  },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var divid = 'dv' + nRow._DT_RowIndex; var btnid = 'View' + nRow._DT_RowIndex; var adelete = 'rwdelete' + nRow._DT_RowIndex;                              
                var detTag = ' <div id="' + divid + '" style="text-align:center;"><span id=' + btnid + ' class="actionbtn" data-toggle="tooltip" title="Details"><i class="fa fa-search"></i></a></span>'+
                     '<span id=' + adelete + ' class="actionbtn" data-toggle="tooltip" title="Delete"><i class="glyphicon glyphicon-trash"></i></span></div>';
                $('td:eq(0)', nRow).html(detTag);
            }       
        });

        $('#tblHeadRowDefRule').addClass('gridHeader'); $('#ToolTables_dataGridDefRule_0,#ToolTables_dataGridDefRule_1').hide(); $('#dataGridDefRule').css('width', '100%');
        setupBSRulesTableHeader();

        var table1 = $('#dataGridBSRule');
        var oBSTable = table1.dataTable({
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
            "columnDefs": [{ "orderable": false, "searchable": false,   'targets': [0],  },
            { 'targets': [0], width: '20px' }, { 'targets': [6], width: '60px' }, { 'targets': [1], width: '40px' },  { 'targets': [2], width: '50px' },{ 'targets': [3], width: '80px' },{ 'targets': [4,5], width: '120px' },            
            { 'targets': [7, 8],visible: false},
            ],
            "lengthMenu": [ [25, 50, 100, -1],  [25, 50, 100, "All"]   ],           
            "sScrollY": "300px",
            "sScrollX": true,
            "paging": true,
            "order": [[1, "asc"]],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var divid = 'dv' + nRow._DT_RowIndex; var aedit = 'rwedit' + nRow._DT_RowIndex; var adelete = 'rwdelete' + nRow._DT_RowIndex;  var divlnkid = 'dvlnk' + nRow._DT_RowIndex;
                if (aData[8] != '') {                  
                    var detTag = ' <div id="' + divid + '" style="text-align:center;"><span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span>' +
                    '<span id=' + adelete + ' class="actionbtn" data-toggle="tooltip" title="Delete"><i class="glyphicon glyphicon-trash"></i></span></div>';
                    if (_isRuleupdate != nRow._DT_RowIndex) { $('td:eq(0)', nRow).html(detTag); }
                }
            }
        });

       $('#tblHeadRowBSRule').addClass('gridHeader');$('#ToolTables_dataGridBSRule_0,#ToolTables_dataGridBSRule_1').hide();
       $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");  $('#dataGridBSRule').css('width', '100%');

       GetDefaultRulesGrid();

        $('.dataTables_scrollHeadInner table').on('click', 'thead tr', function (e) {
            e.preventDefault(); if (e.target.innerText == 'New') { if (e.currentTarget.id == 'tblHeadRowBSRule') {if (_newcont == 0 && (e.target.innerText == 'New')) { newRow(); _newcont++; } }} 
            return false;
        });

        oDRTable.on('click', 'tbody td', function (e) {
            var selectedTr = $(this).parents('tr')[0];  var aData = oDRTable.fnGetData(selectedTr); _targetclick = '';
            if (oDRTable.fnIsOpen(selectedTr)) {
                $(this).addClass("opentd").removeClass("closetd"); oDRTable.fnClose(selectedTr);
            }
            else {
                if (e.target.className == 'opentd' || e.target.className == 'fa fa-search') {
                    _lstMasterDet = [];     var divid = 'dv' + selectedTr._DT_RowIndex;
                    var _grpformat = oDRTable.fnGetData(selectedTr)[4]; var _addressid = oDRTable.fnGetData(selectedTr)[5];
                    sessionStorage['DEFAULT_RULE_ADDRESSID'] = Str(_addressid); sessionStorage['DEFAULT_RULE_GROUP_FORMAT'] = Str(_grpformat);                   
                    GetBSRulesGrid(_addressid, _grpformat); $("#ModalBSRulelnkDet").modal('show');
                }
                if (e.target.className == 'glyphicon glyphicon-trash') {
                  if (confirm('Are you sure ? You want to delete this Default Rule ?')) {
                      var _grpformat = oDRTable.fnGetData(selectedTr)[4]; var _addressid = oDRTable.fnGetData(selectedTr)[5];
                      DeleteDefaultRule(_addressid, _grpformat, GetDefaultRulesGrid);
                  }
                }                                            
            }
        });   

        function fnNewheaderFormatDetails(oTable, _targetclick,val) {
            var sOut = ''; var _str = ''; var indx = 0; var _selectopt = ''; var _doctxt = ''; var _RuleNo = ''; var _Doctype = ''; var _Rule = ''; var _Comments = ''; var _Descr = ''; var aData = ''; var btndiv = '';
            var tid = "RuleTable" + indx; var _tbodyid = "tblBodyRule" + indx; var _nameid = 'cbName'; var _grpFormatid = 'cbGrpFormat';
            var cbName = FillCombo(val, _lstAddrName); var cbgrpfmt = FillCombo('', _lstGrpFmt);
             if (_targetclick == 'New') { _doctxt = ''; _RuleNo = ''; _Doctype = ''; _Rule = ''; _Comments = ''; _Descr = ''; }
             var sOut ='<div class="form-body"><div class="form-group">'+
                 ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Name </label> </div>' +
                 ' <div  class="col-md-4"><select class="bs-select form-control" id="' + _nameid + '">' + cbName + '</select> </div></div><div class="form-group">'+
                 ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Group Format </label> </div>' +
                 ' <div  class="col-md-4"><select class="bs-select form-control" id="' + _grpFormatid + '">' + cbgrpfmt + '</select> </div></div></div>';
            return sOut;
        }

        oBSTable.on('click', 'tbody td', function (e) {
            var selectedBSTr = $(this).parents('tr')[0]; var aData = oBSTable.fnGetData(selectedBSTr); var divid = 'dv' + selectedTr._DT_RowIndex;
            if (e.target.className == 'glyphicon glyphicon-pencil') {
                    _isRuleupdate = selectedBSTr._DT_RowIndex; if (previousBSTr != '' && previousBSTr != selectedBSTr) { restoreRow(previousBSTr, oBSTable); }
                    nNew = false; editRow(oBSTable, selectedBSTr); previousBSTr = selectedBSTr;
                }
            else if (e.target.className == 'glyphicon glyphicon-trash') {
                    var _hidvalue = '0';
                    var _defaddrid = Str(sessionStorage.getItem('DEFAULT_RULE_ADDRESSID')); var _defgrpformat = Str(sessionStorage.getItem('DEFAULT_RULE_GROUP_FORMAT'));
                    var _defid = aData[8]; var rulecode=aData[3];
                    var deleteRule = confirm('Are you sure? You want to delete this rule?');
                    if (deleteRule) {                      
                        var deleteAll = confirm('Are you sure? You want to delete this rule from all related Buyer-Supplier links ?'); if (deleteAll) { _hidvalue ='1'; }
                        DeleteRule(_defid, _defaddrid, _defgrpformat, rulecode, _hidvalue, GetBSRulesGrid);
                    }
                    previousBSTr = '';
                }
            else if (e.target.className == 'glyphicon glyphicon-floppy-save') {
                    var _hidvalue = '0'; var _defid = '0';    _lstMasterDet = [];
                    var _ruleid = $('#cboRulecode option:selected').val(); var _rulecode_txt = $('#cboRulecode option:selected').text(); var _ruleval = $('#cboRuleVal option:selected').val();
                    var _defaddrid = Str(sessionStorage.getItem('DEFAULT_RULE_ADDRESSID')); var _defgrpformat = Str(sessionStorage.getItem('DEFAULT_RULE_GROUP_FORMAT'));
                    _lstMasterDet.push('RULE_CODE' + "|" + Str(_rulecode_txt)); _lstMasterDet.push('RULEID' + "|" + Str(_ruleid)); _lstMasterDet.push('RULE_VALUE' + "|" + Str(_ruleval));
                    _lstMasterDet.push('DEFAULT_RULE_ADDRESSID' + "|" + _defaddrid); _lstMasterDet.push('DEFAULT_RULE_GROUP_FORMAT' + "|" + _defgrpformat);                
                    var _res = ValidateRule(_rulecode_txt, _ruleval, _lstMasterDet, nNew);
                    if (_res == true) {
                        if (nNew) { _defid = '0'; } else { _defid = aData[8]; } var _msg = 'Are you sure you want to override this rule for all the link for the Buyer/Supplier having inherited (Yes) ?';
                        if (confirm(_msg)) { _hidvalue = '1'; } else { _hidvalue = '0'; }
                        _lstMasterDet.push('HID_VALUE' + "|" + _hidvalue); _lstMasterDet.push('DEF_ID' + "|" + _defid);
                        _isRuleupdate = -2; SaveRule(_lstMasterDet, GetBSRulesGrid, _defaddrid, _defgrpformat);
                    }
                    else { } previousBSTr = ''; _newcont = 0;
                }
            else if (e.target.className == 'glyphicon glyphicon-ban-circle') { _isRuleupdate = -2; gridCancelEdit(nNew, nEditing, oBSTable); previousBSTr = ''; _newcont = 0; }
        });

        $("#ModalBSRulelnkDet").on('shown.bs.modal', function () { oBSTable.fnDraw();});

        function editRow(oTable,nRow) {          
            var aData = oTable.fnGetData(nRow); nEditing = nRow; var jqTds = $('>td', nRow); FillRules();
            _lstRuleval = []; _lstRuleval.push("0|0"); _lstRuleval.push("1|1");
            var detTag = '<div style="text-align:center;"><span class="actionbtn" data-toggle="tooltip" title="Update"><i class="glyphicon glyphicon-floppy-save"></i></span>' +
            ' <span class="actionbtn" data-toggle="tooltip" title="Cancel"><i class="glyphicon glyphicon-ban-circle"></i></span></div>';
            jqTds[0].innerHTML = detTag;
            jqTds[3].innerHTML = '<select id="cboRulecode" class="fullWidth selectheight">' + FillCombo(aData[3], _lstRules) + '</select>';
            jqTds[6].innerHTML = '<select id="cboRuleVal" class="fullWidth selectheight">' + FillCombo(aData[6], _lstRuleval) + '</select>';
        };

        function newRow() {
            var aiNew = oBSTable.fnAddData(['', '', '', '', '', '', '', '', '']); var nRow = oBSTable.fnGetNodes(aiNew[0]); editRow(oBSTable, nRow);
            nEditing = nRow; nNew = true;
        };
     
        function restoreRow(nRow, oTable) {
            var aData = oTable.fnGetData(nRow);  var jqTds = $('>td', nRow);
            for (var i = 0, iLen = jqTds.length; i < iLen; i++) { if (i == 0) { } else { oTable.fnUpdate(jqTds[i].innerHTML, nRow, i, false); }}  oTable.fnDraw();
        };

        function gridCancelEdit(nNew,nEditing, oTable) {
            var nRow = $('.DTTT_selected');
            if (nNew) { oTable.fnDeleteRow(nEditing); nNew = false; } else { nRow = nEditing; restoreRow(nRow, oTable); $(nRow).removeClass("DTTT_selected selected"); }
        };

        function updateGridRecord(nEditing, oTable) {
            var oTT = TableTools.fnGetInstance('#dataGridBSRule');  var aSelectedTrs = oTT.fnGetSelected();
            var nRow = $('.DTTT_selected'); eRow = nRow;
            if (nRow.length > 1) { toastr.info("Select one record to update.", "Shipmaster Enterprise 2.0"); }
            else if (nRow.length > 0) {
                if (nEditing !== null && nEditing != nRow) { restoreRow(oTable, nEditing); editRow(oTable, nRow); nEditing = nRow; }
                else if (nEditing == nRow) { saveRow(nEditing); nEditing = null; }
                else { editRow(oTable, nRow); nEditing = nRow; }
                return nEditing;
            }
            else { toastr.error("No Rows selected.", "Shipmaster Enterprise 2.0"); }
        };

        $('#btnDefNew').click(function () {
            var _id = $('#cbName option:selected').val(); var _grpformat = $('#cbGrpFormat option:selected').val();
            var _res = ValidateDefRule(_id, _grpformat);
            if (_res == true) {  _defrule = [];    _defrule.push("ADDRESSID" + "|" + Str(_id)); _defrule.push("GROUP_FORMAT" + "|" + Str(_grpformat));
                SaveDefaultRule(_defrule, GetDefaultRulesGrid); $("#ModalNew").modal('hide');
            }
        });

        $('#btnRefresh').live('click', function (e) { e.preventDefault(); GetDefaultRulesGrid(); });
        $('#btnClear').live('click', function (e) { e.preventDefault(); ClearFilter(); });
        $('#btnNew').live('click', function (e) {
            e.preventDefault(); $('#dvNewDet').empty(); FillAddress();
            var divtag = fnNewheaderFormatDetails(oDRTable, 'New', ''); $('#dvNewDet').append(divtag); $("#ModalNew").modal('show');
            $('#cbName').live("change", function (e) {
                var selectedval = $('#cbName option:selected').val(); FillGroupFormat('ADDRESSID', selectedval);
                var divtag = fnNewheaderFormatDetails(oDRTable, 'New', selectedval);
                $('#dvNewDet').empty(); $('#dvNewDet').append(divtag);
            });
        });
    };

    function GetDefaultRuleDetails(Table, nTr, _lstdet, _targetclick) {
        var indx = '';  if (nTr != '') { indx = nTr.rowIndex;  } else { indx = 0; }
        var tid = "RuleTable" + indx; var _ruleno = $('#txtRuleNo' + indx).val(); var _rule = $('#txtRule' + indx).val();
        var _comments = $('#txtComments' + indx).val(); var _descr = $('#txDescr' + indx).val(); var _doctype = $('#txtDocType' + indx).val();      
        _lstdet.push("RULE_NUMBER" + "|" + Str(_ruleno)); _lstdet.push("DOC_TYPE" + "|" + Str(_doctype));
        _lstdet.push("RULE_CODE" + "|" + Str(_rule)); _lstdet.push("RULE_DESC" + "|" + Str(_descr)); _lstdet.push("RULE_COMMENTS" + "|" + Str(_comments));        
        if (_targetclick == 'Edit') {  var _ruleid = Table.fnGetData(nTr)[7]; _lstdet.push("RULEID" + "|" + Str(_ruleid)); }        
    };

    function FillDefRulesGrid(Table) {
        try {
            $('#dataGridDefRule').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridDefRule').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str(''); cells[1] = Str('');  cells[2] = Str(Table[i].ADDR_CODE);cells[3] = Str(Table[i].ADDR_NAME);
                    cells[4] = Str(Table[i].GROUP_FORMAT); cells[5] = Str(Table[i].ADDRESSID);  cells[6] = Str(Table[i].ADDR_TYPE);cells[7] = Str(Table[i].ROW);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.RULEID != undefined && Table.RULEID != null) {
                    var t = $('#dataGridDefRule').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str('');cells[1] = Str('');cells[2] = Str(Table.ADDR_CODE); cells[3] = Str(Table.ADDR_NAME);  cells[4] = Str(Table.GROUP_FORMAT);
                    cells[5] = Str(Table.ADDRESSID); cells[6] = Str(Table.ADDR_TYPE); cells[7] = Str(Table.ROW);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    var GetDefaultRulesGrid = function () {
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {  
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultRules.aspx/GetDefaultRulesGrid",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table;   FillDefRulesGrid(Table); }
                    else $('#dataGridDefRule').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Default Rules :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
        }, 200);
    };

    function FillBSRulesGrid(Table) {
        try {
            $('#dataGridBSRule').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridBSRule').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    var _arrcommt = Str(Table[i].RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str(''); cells[1] = Str(Table[i].RULE_NUMBER); cells[2] = Str(Table[i].DOC_TYPE); cells[3] = Str(Table[i].RULE_CODE);
                    cells[4] = Str(Table[i].RULE_DESC); cells[5] = _arrcommt; cells[6] = Str(Table[i].RULE_VALUE);  cells[7] = Str(Table[i].RULE_ID); cells[8] = Str(Table[i].DEF_ID);                   
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.DEF_ID != undefined && Table.DEF_ID != null) {
                    var t = $('#dataGridBSRule').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    var _arrcommt = Str(Table.RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str(''); cells[1] = Str(Table.RULE_NUMBER); cells[2] = Str(Table.DOC_TYPE);cells[3] = Str(Table.RULE_CODE);
                    cells[4] = Str(Table.RULE_DESC);   cells[5] = _arrcommt; cells[6] = Str(Table.RULE_VALUE); cells[7] = Str(Table.RULE_ID); cells[8] = Str(Table.DEF_ID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    }; 

    var GetBSRulesGrid = function (ADDRESSID, GROUP_FORMAT) {
        setTimeout(function () {  
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultRules.aspx/FillRulesGrid",
            data: "{'ADDRESSID':'"+ADDRESSID+"','GROUP_FORMAT':'"+GROUP_FORMAT+"'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) {var Table = DataSet.NewDataSet.Table;  FillBSRulesGrid(Table); }
                    else $('#dataGridBSRule').DataTable().clear().draw();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get All Rules :' + err);  }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
        }, 200);
    };

    function FillAddress() {
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultRules.aspx/GetPartyCode",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var Dataset = JSON.parse(response.d);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table;    _lstAddrName = [];
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) { for (var i = 0; i < Table.length; i++) { _lstAddrName.push(Str(Table[i].ADDRESSID) + "|" + Str(Table[i].ADDR_NAME)); }}
                            else { if (Table.GROUP_ID != undefined) { _lstAddrName.push(Str(Table.ADDRESSID) + "|" + Str(Table.ADDR_NAME));} }
                        }
                    }
                }
                catch (err) { toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd"); }
            },
            failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    function FillGroupFormat(KEY,VALUE) {
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultRules.aspx/GetGroupFormatList",
            data: "{'KEY':'" + KEY + "','VALUE':'" + VALUE + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var Dataset = JSON.parse(response.d);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table; _lstGrpFmt = [];
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) { for (var i = 0; i < Table.length; i++) { _lstGrpFmt.push(Str(Table[i].GROUP_FORMAT) + "|" + Str(Table[i].GROUP_FORMAT));  }}
                            else { if (Table.GROUP_FORMAT != undefined) { _lstGrpFmt.push(Str(Table.GROUP_FORMAT) + "|" + Str(Table.GROUP_FORMAT)); } }
                        }
                    }
                }
                catch (err) { toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd");}
            },
            failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    function FillRules() {
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultRules.aspx/GetAllRules",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var Dataset = JSON.parse(response.d);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table;   _lstRules = [];
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) { for (var i = 0; i < Table.length; i++) { _lstRules.push(Str(Table[i].RULEID) + "|" + Str(Table[i].RULE_CODE)); } }
                            else { if (Table.RULEID != undefined) { _lstRules.push(Str(Table.RULEID) + "|" + Str(Table.RULE_CODE)); } }
                        }
                    }
                }
                catch (err) { toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd");  }
            },
            failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    function FillCombo(val, _lst) {
        var opt = '';
        try {
            opt = '<option></option>';
            if (_lst.length != undefined && _lst.length > 0) {
                for (var i = 0; i < _lst.length; i++) {
                    var cdet = _lst[i].split("|");
                    if (val != "" && val == Str(cdet[0])) { opt += '<option selected value="' + Str(cdet[0]) + '">' + Str(cdet[1]) + '</option>'; }
                    else if (val != "" && val == Str(cdet[1])) { opt += '<option selected value="' + Str(cdet[0]) + '">' + Str(cdet[1]) + '</option>'; }
                    else { opt += '<option value="' + Str(cdet[0]) + '">' + Str(cdet[1]) + '</option>'; }
                }
                return opt;
            }
        }
        catch (err) { toastr.error('Error while populating List :' + err, "Lighthouse eSolutions Pte. Ltd"); }
    };

    function ValidateDefRule(_id,_grpformat) {
        var _valid = true;
        if (_grpformat == '') { $('#cbGrpFormat').addClass('error'); _valid = false; } else { $('#cbGrpFormat').removeClass('error'); _valid = true; }
        if (_id == '') { $('#cbName').addClass('error'); _valid = false; }
        else { var isexist = CheckExistingDefaultRule(_id, _grpformat);
            if (isexist == '') { $('#cbName').removeClass('error'); _valid = true; }
            else { $('#cbName').addClass('error'); toastr.error("Lighthouse eSolutions Pte. Ltd.", isexist); _valid = false; }
        }
        return _valid;
    };

    var CheckExistingDefaultRule = function (ADDRESSID, GROUP_FORMAT) {
        var res = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultRules.aspx/CheckExistingDefaultRule",
            data: "{ 'ADDRESSID':'" + ADDRESSID + "','GROUP_FORMAT':'" + GROUP_FORMAT + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    res = Str(response.d);
                }
                catch (err) {toastr.error("Lighthouse eSolutions Pte. Ltd.", 'cannot validate Default rule :' + err);}
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) {  toastr.error("error get", response.responseText); }
        });
        return res;
    };

    function SaveDefaultRule(_nfieldval, callback) {
        var slDefRuledet = [];
        for (var j = 0; j < _nfieldval.length; j++) { slDefRuledet.push(_nfieldval[j]);  }
        var data2send = JSON.stringify({ slDefRuledet: slDefRuledet });
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {  
            $.ajax({
                type: "POST",
                async: false,
                url: "DefaultRules.aspx/SaveDefaultRuleDetails",
                data: data2send,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d);
                        if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "Default Rule Saved successfully.");  GetDefaultRulesGrid();}
                        Metronic.unblockUI();
                    }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Default Rule details :' + err); Metronic.unblockUI(); }
                },
                failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
                error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
            });
        }, 200);
    };

    function DeleteDefaultRule(ADDRESSID, GROUP_FORMAT, callback) {
        Metronic.blockUI('#portlet_body');
        setTimeout(function () { 
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultRules.aspx/DeleteDefaultRule",
            data: "{ 'ADDRESSID':'" + ADDRESSID + "','GROUP_FORMAT':'" + GROUP_FORMAT + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "Default Rule Deleted.");  GetDefaultRulesGrid(); }
                    Metronic.unblockUI();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete Default Rule :' + err); Metronic.unblockUI(); }
            },
            failure: function (response) {  toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
        }, 200);
    };

    function ValidateRule(_rulecode, _ruleval, _lstMasterDet, nNew) {
        var _valid = true;
        if (nNew) {
            if (_rulecode == '') { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Rule Code field is blank'); _valid = false; }
            if (_ruleval == '') { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Rule Value field is blank'); _valid = false; }
            var _existrec = ValidateExistingRule(_lstMasterDet);  if (_existrec != '') { toastr.error("Lighthouse eSolutions Pte. Ltd.", _existrec); _valid = false; }
        }
        return _valid;
    };

    function SaveRule(_nfieldval, callback, _defaddrid, _defgrpformat) {
        var slRuledet = [];   var _addressid = ''; var _grpFormat = '';    for (var j = 0; j < _nfieldval.length; j++) { slRuledet.push(_nfieldval[j]); }
        var data2send = JSON.stringify({ slRuledet: slRuledet });
        Metronic.blockUI('#portlet_body');
        setTimeout(function () { 
            $.ajax({
                type: "POST",
                async: false,
                url: "DefaultRules.aspx/SaveRuleDetails",
                data: data2send,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d);
                        if (res != "") {  toastr.success("Lighthouse eSolutions Pte. Ltd", "Rule Saved successfully.");  callback(_defaddrid, _defgrpformat);  }
                        Metronic.unblockUI();
                    }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Rule details :' + err); Metronic.unblockUI(); }
                },
                failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI();},
                error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI();}
            });       
        }, 200);
    };

    function DeleteRule(DEF_ID, ADDRESSID, GROUP_FORMAT, RULE_CODE, DELALL, callback) {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultRules.aspx/DeleteRule",
            data: "{'DEF_ID':'" + DEF_ID + "', 'ADDRESSID':'" + ADDRESSID + "','RULE_CODE':'" + RULE_CODE + "','DELALL':'" + DELALL + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") {   toastr.success("Lighthouse eSolutions Pte. Ltd", "Rule Deleted.");  callback(ADDRESSID, GROUP_FORMAT); }
                    Metronic.unblockUI();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete Rule :' + err); Metronic.unblockUI(); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };

    var ValidateExistingRule= function (_nfieldval) {
        var slRuledet = []; var res = '';  for (var j = 0; j < _nfieldval.length; j++) { slRuledet.push(_nfieldval[j]);}
        var data2send = JSON.stringify({ slRuledet: slRuledet });            
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultRules.aspx/ValidateRule",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try { res = Str(response.d);  }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to validate Rule details :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
        return res;
    };

    var setupBSRulesTableHeader = function () {
        var dtfilter = '<th><div style="text-align:center;"><span><a style="color: #eee8e8;"><u>New</u></<a></span></div></th><th>Rule number</th><th>Doc Type</th><th>Rule Code</th><th>Description</th><th>Comments</th><th>Rule Value</th><th>RULE_ID</th><th>DEF_ID</th>';
        var header = ' <thead id="tblHeadBSRule"><tr id="tblHeadRowBSRule">' + dtfilter + '</tr></thead><tbody id="tblBodyBSRule" style="color: #000;"> </tbody></table></div>';
        $('#tblHeadRowBSRule').empty().append(dtfilter);  $('#tblBodyBSRule').empty();
    };

    var setupTableHeader = function () {
        setFilterToolbar();
        var dtfilter = '<th style="text-align:center;"></th><th></th><th>Code</th><th>Name</th><th>Group Format</th><th>ADDRESSID</th><th>ADDR_TYPE</th><th>ROW</th>';
        $('#tblHeadRowDefRule').empty().append(dtfilter); $('#tblBodyDefRule').empty();       
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
    function ClearFilter() { setFilterToolbar(); $('#dataGridDefRule').DataTable().clear().draw(); };
    return {
        init: function () { handleDefaultRulesTable(); }
    };
}();