var selectedTr = '';var previousTr = '';var BSpreviousTr = '';var BSRpreviousTr = ''; var _scheckid=[];var _lstRuleval = [];var _lstInherit = [];var _lstRuledet = [];

var LinkRules = function () {

    var handleLinkRulesTable = function () {
        var nEditing = null; var nNew = false; $('#pageTitle').empty().append('Link Rules');
        SetupBreadcrumb('Home', 'Home.aspx', 'Rules -', '#', 'Link Rules', 'LinkRules.aspx');
        $(document.getElementById('lnkRuledet')).addClass('active open');$(document.getElementById('spLinkRule')).addClass('title font-title SelectedColor');
        var _defaddrid = Str(sessionStorage.getItem('ADDRESSID')); var _configaddrid = Str(sessionStorage.getItem('CONFIGADDRESSID'));var _addrtype = Str(sessionStorage.getItem('ADDRTYPE'));
        setupTableHeader(_addrtype);

        var table = $('#dataGridLnkRule');
        var oLRTable = table.dataTable({
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
            "columnDefs": [{  'orderable': false, "searching": true,"autoWidth": false,'targets': [0]},          
            { 'targets': [0], width: '6%' }, { 'targets': [1], width: '6%'},
            { 'targets': [2], width: '19%' }, { 'targets': [3], width: '10%' }, { 'targets': [4], width: '11%', "sClass": "visible-lg" },
            { 'targets': [5], width: '24%' }, { 'targets': [6], width: '24%' },
            { 'targets': [7], visible: false }, 
            ],
            "lengthMenu": [[25, 50, 100, -1],   [25, 50, 100, "All"]    ],
            "scrollY":'300px',      
            "scrollX": '1200px',
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' });},
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var divid = 'dv' + nRow._DT_RowIndex; var btnid = 'View' + nRow._DT_RowIndex;
                if (_addrtype.toUpperCase() == 'BUYER') { $('td:eq(0)', nRow).css('display', 'none'); } else {                    
                    var viewbtnTag = '<div  style="text-align:center;"><span class="actionbtn" data-toggle="tooltip" title="Details" id= "' + btnid + '"><i class="fa fa-search"></i></span></div>';
                    $('td:eq(0)', nRow).html(viewbtnTag);
                }
            }       
        });    
        $('#tblHeadRowLnkRule').addClass('gridHeader'); $('#ToolTables_dataGridLnkRule_0,#ToolTables_dataGridLnkRule_1').hide(); //$('#dataGridLnkRule_info').hide();   
        $('#dataGridLnkRule').css('width', '100%');
        GetLinkRulesGrid();setupBSTableHeader();

        var table3 = $('#dataGridBS');
        var oBSTable = table3.dataTable({
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
                'orderable': false, "searching": true, "autoWidth": false,'targets': [0],
            },
            { 'targets': [0], width: '15px' },{ 'targets': [1], width: '30px' },{ 'targets': [2], width: '120px' },{ 'targets': [3,4], width: '80px' },            
            { 'targets': [5, 6], width: '70px' },{ 'targets': [10], width: '70px',class:'break-det'}, { 'targets': [7], width: '50px' },
            { 'targets': [8], width: '60px' }, { 'targets': [13], width: '25px' },// { 'targets': [11], width: '30px' }, { 'targets': [12], width: '34px' },
            { 'targets': [5, 10], "sClass": "visible-lg" }, { 'targets': [6, 9, 11,12,14, 15], visible: false },
            ],
            "lengthMenu": [  [25, 50, 100, -1],[25, 50, 100, "All"]],
            "sScrollY": '300px',
            "sScrollX": "1050px",
            "paging": true,
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var divid = 'dv' + nRow._DT_RowIndex; var btnid = 'View' + nRow._DT_RowIndex; var _bcheck = ''; var _ncheck = ''; var _acheck = '';
                var _nByid = "by" + nRow._DT_RowIndex; var _nSpid = "sp" + nRow._DT_RowIndex; var _nAtid = "act" + nRow._DT_RowIndex;
                var viewbtnTag = '<div  style="text-align:center;"><span class="actionbtn" data-toggle="tooltip" title="Details" id= "' + btnid + '"><i class="fa fa-search"></i></span></div>';
                $('td:eq(0)', nRow).html(viewbtnTag); var _nByval = aData[11]; var _nSpval = aData[12]; var _nAtval = aData[13];
                if (_nByval == '1') { _bcheck = 'checked'; } else { _bcheck = ''; }  if (_nSpval == '1') { _ncheck = 'checked'; } else { _ncheck = ''; }
                if (_nAtval == '1') { _acheck = 'checked'; } else { _acheck = ''; }
                var _nBuyer = '<div style="text-align:center;padding:2px;"><input type="checkbox"  id="' + _nByid + '"  value="' + _nByval + '" ' + _bcheck + ' disabled/></div>';
                var _nSupplier = '<div style="text-align:center;padding:2px;"><input type="checkbox"  id="' + _nSpid + '"  value="' + _nSpval + '" ' + _ncheck + ' disabled/></div>';
                var _nActive = '<div style="text-align:center;padding:2px;"><input type="checkbox"  id="' + _nAtid + '"  value="' + _nAtval + '" ' + _acheck + ' disabled/></div>';
                $('td:eq(9)', nRow).html(_nBuyer); $('td:eq(10)', nRow).html(_nSupplier); $('td:eq(11)', nRow).html(_nActive);
            }
        });
        $('#tblHeadRowBS').addClass('gridHeader'); $('#ToolTables_dataGridBS_0,#ToolTables_dataGridBS_1').hide();// $('#dataGridBS_info').hide();
        $('#dataGridBS').css('width', '100%'); setupBSRulesTableHeader();

        var table4 = $('#dataGridBSRule');
        var oBSRTable = table4.dataTable({
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
                'orderable': false, "searching": true, "autoWidth": false, 'targets': [0],
            },
            { 'targets': [0,2], width: '50px' }, { 'targets': [1], width: '70px' },            
            { 'targets': [6,7], width: '60px' },{'targets': [8], visible: false },
            ],
            "lengthMenu": [ [25, 50, 100, -1], [25, 50, 100, "All"]],
            "sScrollY": '300px',
            "sScrollX": true,
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var divid = 'dv' + nRow._DT_RowIndex; var aedit = 'rwdedit' + nRow._DT_RowIndex;
                var detTag = ' <div id="' + divid + '" style="text-align:center;"><span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span></div>';
                $('td:eq(0)', nRow).html(detTag);
            }
        });
        $('#tblHeadRowBSRule').addClass('gridHeader'); $('#ToolTables_dataGridBSRule_0,#ToolTables_dataGridBSRule_1').hide();   $('#dataGridBSRule').css('width', '100%');
       
        setupNewRulesTableHeader();
        var table2 = $('#dataGridNewRule');
        var oNRTable = table2.dataTable({
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
                "sRowSelect": "multiple",
                "aButtons": ["select_all", "select_none"]
            },
            "columnDefs": [{  // set default column settings
                'orderable': false, "searching": true, "autoWidth": false,  'targets': [0],
            },
            { 'targets': [0], width: '5px' }, { 'targets': [1, 2], width: '40px' }, { 'targets': [3], width: '110px' },
            { 'targets': [4], width: '140px' },
            { 'targets': [5], visible: false },
            ],
            "lengthMenu": [   [10,25, 50, 100, -1],[10,25, 50, 100, "All"]],
            "scrollX": true,
            "scrollY": "300px",
            "paging": false,
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' });},
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var _chkid = "chk" + nRow._DT_RowIndex; var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="' + _chkid + '"  /></div>';
                $('td:eq(0)', nRow).html(_chkdiv);
            }
        });
        $('#tblHeadRowNewRule').addClass('gridHeader'); $('#ToolTables_dataGridNewRule_0,#ToolTables_dataGridNewRule_1').hide(); $('#dataGridNewRule_paginate').hide();//#dataGridNewRule_info,
        $('#dataGridNewRule').css('width', '100%');
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
        _lstRuleval = []; _lstRuleval.push("0|0");_lstRuleval.push("1|1");   _lstInherit = [];  _lstInherit.push("1|Yes"); _lstInherit.push("0|No");

        oLRTable.on('click', 'tbody td', function (e) {
            e.preventDefault();
            var selectedTr = $(this).parents('tr')[0]; var aData = oLRTable.fnGetData(selectedTr); _targetclick = '';
            if (oLRTable.fnIsOpen(selectedTr)) {
                $(this).addClass("opentd").removeClass("closetd"); oLRTable.fnClose(selectedTr);
            }
            else {
                if (e.target.className == 'opentd' || e.target.className == 'fa fa-search') {
                    var divid = 'dv' + selectedTr._DT_RowIndex; var _addressid = oLRTable.fnGetData(selectedTr)[7]; GetBSGrid(_addressid);
                    $("#ModalBuySupp").on('shown.bs.modal', function () { oBSTable.fnDraw(); });$("#ModalBuySupp").modal('show');
                }
            }
        });

        oBSTable.on('click', 'tbody td', function (e) {
            e.preventDefault();
            var selectedTr = $(this).parents('tr')[0]; var aData = oBSTable.fnGetData(selectedTr); _targetclick = '';
            if (oBSTable.fnIsOpen(selectedTr)) {
                $(this).addClass("opentd").removeClass("closetd"); oBSTable.fnClose(selectedTr);
            }
            else {
                if (e.target.className == 'opentd' || e.target.className == 'fa fa-search') {
                    var divid = 'dv' + selectedTr._DT_RowIndex; var _linkid = oBSTable.fnGetData(selectedTr)[15];
                    sessionStorage['LINKID'] = Str(_linkid); GetBSuppRulesGrid(_linkid);
                    $("#ModalBSRulesLst").on('shown.bs.modal', function () { oBSRTable.fnDraw(); }); $("#ModalBSRulesLst").modal('show');
                }            
            }
            return false;
        });
     
        $('.dataTables_scrollHeadInner table').on('click', 'thead th', function (e) {
            e.preventDefault();
            if (e.target.innerText == 'New') {
                var _linkid = Str(sessionStorage.getItem('LINKID')); GetNewRuleGrid(_linkid);   $("#ModalNewRule").modal('show');
            }
        });
        $("#ModalNewRule").on('shown.bs.modal', function () { oNRTable.fnDraw(); });

        oBSRTable.on('click', 'tbody td', function (e) {
            e.preventDefault();
            var bsrselectedTr = $(this).parents('tr')[0]; var aData = oBSRTable.fnGetData(bsrselectedTr); var divid = 'dv' + selectedTr._DT_RowIndex;
            if (e.target.className == 'glyphicon glyphicon-pencil') {
                if (BSRpreviousTr != '' && BSRpreviousTr != bsrselectedTr) { restoreRow(BSRpreviousTr, oBSRTable); }
                nNew = false; editRow(oBSRTable, bsrselectedTr); BSRpreviousTr = bsrselectedTr;
            }
            else if (e.target.className == 'glyphicon glyphicon-floppy-save') {
                var _hidvalue = '0'; var _ruleid = '0'; _lstRuledet = [];
                var _linkid = Str(sessionStorage.getItem('LINKID')); var _ruleid = aData[8];
                var _ruleval = $('#cboRuleVal option:selected').val(); var _inheritval = $('#cboInheritval option:selected').val();
                _lstRuledet.push('LINKID' + "|" + Str(_linkid)); _lstRuledet.push('RULEID' + "|" + Str(_ruleid)); _lstRuledet.push('RULE_VALUE' + "|" + Str(_ruleval));
                _lstRuledet.push('INHERIT_RULE' + "|" + Str(_inheritval));
                var _res = ValidateRule(_ruleval);
                if (_res == true) { SaveBSLinkRule(_lstRuledet, GetBSuppRulesGrid, _linkid); } BSRpreviousTr = '';
            }
            else if (e.target.className == 'glyphicon glyphicon-ban-circle') { restoreRow(bsrselectedTr, oBSRTable); BSRpreviousTr = ''; }
        });

        function editRow(oTable, nRow) {
            var aData = oTable.fnGetData(nRow);Editing = nRow;  var jqTds = $('>td', nRow);
            var detTag = '<div style="text-align:center;"><span class="actionbtn" data-toggle="tooltip" title="Update"><i class="glyphicon glyphicon-floppy-save"></i></span>' +
          ' <span class="actionbtn" data-toggle="tooltip" title="Cancel"><i class="glyphicon glyphicon-ban-circle"></i></span></div>';
            jqTds[0].innerHTML = detTag;
            jqTds[6].innerHTML = '<select id="cboRuleVal" class="bs-select form-control">' + FillCombo(aData[6], _lstRuleval) + '</select>';
            jqTds[7].innerHTML = '<select id="cboInheritval" class="bs-select form-control">' + FillCombo(aData[7], _lstInherit) + '</select>';
        };
          
        function restoreRow(nRow, oTable) {
            var aData = oTable.fnGetData(nRow); var jqTds = $('>td', nRow);
            for (var i = 0, iLen = jqTds.length; i < iLen; i++) {
                if (i == 0) { }else { oTable.fnUpdate(jqTds[i].innerHTML, nRow, i, false); } } oTable.fnDraw();
        };

        function gridCancelEdit(nNew, nEditing, oTable) {
            var nRow = $('.DTTT_selected');
            if (nNew) { oTable.fnDeleteRow(nEditing); nNew = false; }
            else { nRow = nEditing; restoreRow(nRow, oTable);$(nRow).removeClass("DTTT_selected selected"); }
        };

        $('#btnNewRule').click(function (e) {
            e.preventDefault();
            _scheckid = [];  var _linkid = Str(sessionStorage.getItem('LINKID'));
            $('input[type=checkbox]:checked').each(function () {
                var tr = $(this).closest('tr'); if (oNRTable.fnGetData(tr) != null) {
                    var _ruleid = oNRTable.fnGetData(tr)[5];  _scheckid.push(Str(_ruleid));
                }
            });
            AddNewLinkRule(_linkid, _scheckid);$("#ModalNewRule").modal('hide');
        });
    };

    function GetLinkRulesDetails(Table, nTr, _lstdet, _targetclick) {
        var indx = '';
        if (nTr != '') { indx = nTr.rowIndex; } else { indx = 0; } var tid = "RuleTable" + indx;
        var _ruleno = $('#txtRuleNo' + indx).val(); var _rule = $('#txtRule' + indx).val(); var _comments = $('#txtComments' + indx).val();
        var _descr = $('#txDescr' + indx).val(); var _doctype = $('#txtDocType' + indx).val();      
        _lstdet.push("RULE_NUMBER" + "|" + Str(_ruleno)); _lstdet.push("DOC_TYPE" + "|" + Str(_doctype));
        _lstdet.push("RULE_CODE" + "|" + Str(_rule)); _lstdet.push("RULE_DESC" + "|" + Str(_descr)); _lstdet.push("RULE_COMMENTS" + "|" + Str(_comments));        
        if (_targetclick == 'Edit') { var _ruleid = Table.fnGetData(nTr)[7]; _lstdet.push("RULEID" + "|" + Str(_ruleid)); }        
    };

    function FillNewRuleGrid(Table) {
        try {
            $('#dataGridNewRule').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridNewRule').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str(''); cells[1] = Str(Table[i].RULE_NUMBER); cells[2] = Str(Table[i].DOC_TYPE);  cells[3] = Str(Table[i].RULE_CODE);
                    cells[4] = Str(Table[i].RULE_DESC);  cells[5] = Str(Table[i].RULEID);                  
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.LINKID != undefined && Table.LINKID != null) {
                    var t = $('#dataGridNewRule').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str('');  cells[1] = Str(Table.RULE_NUMBER); cells[2] = Str(Table.DOC_TYPE); cells[3] = Str(Table.RULE_CODE);
                    cells[4] = Str(Table.RULE_DESC); cells[5] = Str(Table.RULEID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    var GetNewRuleGrid = function (LINKID) {      
        $.ajax({
            type: "POST",
            async: false,
            url: "LinkRules.aspx/FillUnLinkedRulesGrid",
            data: "{'LINKID':'" + LINKID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table; FillNewRuleGrid(Table); }
                    else $('#dataGridNewRule').DataTable().clear().draw();
                }
                catch (err) {toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer/Supplier Details :' + err);}
            },
            failure: function (response) {toastr.error("failure get", response.d);},
            error: function (response) {toastr.error("error get", response.responseText); }
        });        
    };

    function FillBSuppRulesGrid(Table) {
        try {
            $('#dataGridBSRule').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridBSRule').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    var _arrcommt = Str(Table[i].RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str('');  cells[1] = Str(Table[i].RULE_NUMBER);   cells[2] = Str(Table[i].DOC_TYPE); cells[3] = Str(Table[i].RULE_CODE);
                    cells[4] = Str(Table[i].RULE_DESC);  cells[5] = _arrcommt; cells[6] = Str(Table[i].RULE_VALUE);
                    cells[7] = (Str(Table[i].INHERIT_RULE) == '1') ? 'Yes' : 'No';  cells[8] = Str(Table[i].RULEID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.ADDRESSID != undefined && Table.ADDRESSID != null) {
                    var t = $('#dataGridBSRule').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    var _arrcommt = Str(Table.RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str(''); cells[1] = Str(Table.RULE_NUMBER);cells[2] = Str(Table.DOC_TYPE); cells[3] = Str(Table.RULE_CODE);
                    cells[4] = Str(Table.RULE_DESC);  cells[5] = _arrcommt; cells[6] = Str(Table.RULE_VALUE);
                    cells[7] =  (Str(Table.INHERIT_RULE)=='1')?'Yes':'No'; cells[8] = Str(Table.RULEID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e){ }
    };

    var GetBSuppRulesGrid = function (LINKID) {
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "LinkRules.aspx/FillRulesGrid",
            data: "{'LINKID':'" + LINKID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table; FillBSuppRulesGrid(Table); }
                    else $('#dataGridBSRule').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer/Supplier Rules :' + err); } },
              failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI();}
        });
        }, 200);
    };

    function FillLinkRulesGrid(Table) {
        try {
            $('#dataGridLnkRule').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridLnkRule').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str('');   cells[1] = Str(Table[i].ADDR_CODE);  cells[2] = Str(Table[i].ADDR_NAME); cells[3] = Str(Table[i].CONTACT_PERSON);
                    cells[4] = Str(Table[i].ADDR_EMAIL);  cells[5] = Str(Table[i].ADDR_INBOX); cells[6] = Str(Table[i].ADDR_OUTBOX);cells[7] = Str(Table[i].ADDRESSID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.ADDRESSID != undefined && Table.ADDRESSID != null) {
                    var t = $('#dataGridLnkRule').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str('');  cells[1] = Str(Table.ADDR_CODE); cells[2] = Str(Table.ADDR_NAME); cells[3] = Str(Table.CONTACT_PERSON);
                    cells[4] = Str(Table.ADDR_EMAIL);  cells[5] = Str(Table.ADDR_INBOX); cells[6] = Str(Table.ADDR_OUTBOX);  cells[7] = Str(Table.ADDRESSID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e){ }
    };

    var GetLinkRulesGrid = function () {
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "LinkRules.aspx/FillSupplierGrid",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table; FillLinkRulesGrid(Table); }
                    else $('#dataGridLnkRule').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI();toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Link Rules :' + err);}
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
        }, 200);
    };

    function FillBSGrid(Table) {
        try {
            $('#dataGridBS').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridBS').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str(''); cells[1] = Str(Table[i].BUYER_CODE); cells[2] = Str(Table[i].BUYER_NAME);  cells[3] = Str(Table[i].BUYER_LINK_CODE);
                    cells[4] = Str(Table[i].VENDOR_LINK_CODE);   cells[5] = Str(Table[i].BUYER_FORMAT);  cells[6] = Str(Table[i].BUYER_EXPORT_FORMAT);
                    cells[7] = Str(Table[i].VENDOR_FORMAT); cells[8] = Str(Table[i].SUPPLIER_EXPORT_FORMAT); cells[9] = Str(Table[i].SUPP_EXPORT_PATH);
                    cells[10] = Str(Table[i].GROUP_CODE);  cells[11] = Str(Table[i].NOTIFY_BUYER);  cells[12] = Str(Table[i].NOTIFY_SUPPLR);
                    cells[13] = Str(Table[i].IS_ACTIVE); cells[14] = Str(Table[i].BUYERID);  cells[15] = Str(Table[i].LINKID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.LINKID != undefined && Table.LINKID != null) {
                    var t = $('#dataGridBS').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str('');  cells[1] = Str(Table.BUYER_CODE);cells[2] = Str(Table.BUYER_NAME); cells[3] = Str(Table.BUYER_LINK_CODE);
                    cells[4] = Str(Table.VENDOR_LINK_CODE); cells[5] = Str(Table.BUYER_FORMAT);  cells[6] = Str(Table.BUYER_EXPORT_FORMAT);
                    cells[7] = Str(Table.VENDOR_FORMAT);  cells[8] = Str(Table.SUPPLIER_EXPORT_FORMAT); cells[9] = Str(Table.SUPP_EXPORT_PATH);
                    cells[10] = Str(Table.GROUP_CODE);  cells[11] = Str(Table.NOTIFY_BUYER); cells[12] = Str(Table.NOTIFY_SUPPLR);
                    cells[13] = Str(Table.IS_ACTIVE);cells[14] = Str(Table.BUYERID);  cells[15] = Str(Table.LINKID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e) { }
    }; 

    var GetBSGrid = function (ADDRESSID) {
        setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "LinkRules.aspx/FillBuyerSupplierGrid",
            data: "{'ADDRESSID':'"+ADDRESSID+"'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table;  FillBSGrid(Table); }
                    else $('#dataGridBS').DataTable().clear().draw();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer/Supplier Details :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText);  }
        });
        }, 20);
    };

      function ValidateRule(_ruleval) {
        var _valid = true;
        if (_ruleval == '') { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Rule Value field is blank'); _valid = false; } return _valid;
    };

    function SaveBSLinkRule(_nfieldval, callback,linkid) {
        var slBSRuledet = [];
        for (var j = 0; j < _nfieldval.length; j++) { slBSRuledet.push(_nfieldval[j]);  }
        var data2send = JSON.stringify({ slBSRuledet: slBSRuledet });
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "LinkRules.aspx/SaveBSRuleDetails",
                data: data2send,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d);
                        if (res != "") {  toastr.success("Lighthouse eSolutions Pte. Ltd", "Rule updated successfully."); GetBSuppRulesGrid(linkid);  }
                        Metronic.unblockUI();
                    }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Rule details :' + err); Metronic.unblockUI();}
                },
                failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
                error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI();}
            });
        }, 20);
    };

    function AddNewLinkRule(LINKID,_nfieldval) {
        var slChkdet = [];        
        for (var j = 0; j < _nfieldval.length; j++) { slChkdet.push(_nfieldval[j]); }
        var data2send = JSON.stringify({ LINKID: LINKID, slChkdet: slChkdet });
        Metronic.blockUI('#portlet_body');        
        $.ajax({
            type: "POST",
            async: false,
            url: "LinkRules.aspx/AddNewRuleDetails",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") {  toastr.success("Lighthouse eSolutions Pte. Ltd", " Rule added successfully."); GetBSuppRulesGrid(LINKID); }
                    Metronic.unblockUI();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to add Rule details :' + err); Metronic.unblockUI();}
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI();},
            error: function (response) {toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };

    var setupBSTableHeader = function () {
        var dtfilter = '<th>View</th><th>Code</th><th>Buyer Name</th><th>Buyer Link Code</th><th>Supplier Link Code</th><th>Buyer Format</th> <th>Buyer Export Format</th><th>Supplier Format</th><th>Supplier Export Format</th><th>Vendor Export Path</th><th>Group</th><th>Notify Buyer</th><th>Notify Supplier</th><th>Active</th><th>BUYERID</th><th>LINKID</th>';
        $('#tblHeadRowBS').empty().append(dtfilter); $('#tblBodyBS').empty();
    };

    var setupNewRulesTableHeader = function () {
        var dtfilter = '<th></th><th>Rule Number</th><th>Doc Type</th><th>Rule Code</th><th>Rule Desc</th><th>RULEID</th>'; $('#tblHeadRowNewRule').empty().append(dtfilter);  $('#tblBodyNewRule').empty();
    };

    var setupBSRulesTableHeader = function () {
        var dtfilter = '<th><div style="text-align:center;"><span><a style="color: #eee8e8;"><u>New</u></<a></span></div></th><th>Rule number</th><th>Doc Type</th><th>Rule Code</th><th>Description</th><th>Comments</th><th>Rule Value</th><th>Is Inherit</th><th>RULEID</th>';
        $('#tblHeadRowBSRule').empty().append(dtfilter); $('#tblBodyBSRule').empty();
    };

    var setupTableHeader = function (_addrtype) {
        var dthead = ''; $('#toolbtngroup').empty();
        if (_addrtype.toUpperCase() == "BUYER") { dthead = '<th style="display:none;">View</th>'; } else { dthead = '<th>View</th>'; }
        var dtfilter = dthead+'<th>Code</th><th>Supplier Name</th><th>Contact Person</th><th>Email</th><th>Adaptor Upload Path</th><th>Adaptor Download Path</th><th>ADDRESSID</th>';
        $('#tblHeadRowLnkRule').empty().append(dtfilter);  $('#tblBodyLnkRule').empty();
    };

    return {
        init: function () { handleLinkRulesTable(); }
    };
}();