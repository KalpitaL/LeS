var _lstDoc = [];var selectedTr = '';var previousTr = '';

var Rules = function () {

    var handleRulesTable = function () {
        var nEditing = null; var nNew = false; $('#pageTitle').empty().append('Rules');  SetupBreadcrumb('Home', 'Home.aspx', 'Rules -', '#', 'Rules', 'Rules.aspx');
        $(document.getElementById('lnkRuledet')).addClass('active open'); $(document.getElementById('spRule')).addClass('title font-title SelectedColor');              
        setupTableHeader();

        var table = $('#dataGridRule');
        var oRTable = table.dataTable({
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
            "columnDefs": [{'orderable': false, "searching": true,"autoWidth": false,  'targets': [0]},
            { 'targets': [7], visible: false }, { 'targets': [0], width: '40px' }, { 'targets': [1], 'bSortable': false, width: '50px' }, { 'targets': [2], width: '50px' },
            { 'targets': [3], width: '100px' }, { 'targets': [4], width: '220px' },{ 'targets': [5], width: '210px' },{ 'targets': [6], width: '60px'}
             ],            
            "lengthMenu": [  [25, 50, 100, -1],    [25, 50, 100, "All"]  ],
            "scrollY": '300px',
            "sScrollX":true,
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' });},
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var divid = 'dv' + nRow._DT_RowIndex; var aedit = 'rwedit' + nRow._DT_RowIndex; var adelete = 'rwdelete' + nRow._DT_RowIndex; var divlnkid = 'dvlnk' + nRow._DT_RowIndex;                               
                var detTag = ' <div id="' + divid + '" style="text-align:center;"><span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span>' +
                '<span id=' + adelete + ' class="actionbtn" data-toggle="tooltip" title="Delete"><i class="glyphicon glyphicon-trash"></i></span></div>';
                $('td:eq(0)', nRow).html(detTag);  var lnkTag = '<span><a><u>Link Details</u></<a></span>';  $('td:eq(6)', nRow).html(lnkTag);             
            }       
        });

        $('#tblHeadRowRule').addClass('gridHeader'); $('#ToolTables_dataGridRule_0,#ToolTables_dataGridRule_1').hide();
        setupLinkDetTableHeader();

        var table1 = $('#dataGridlnkRule');
        var oSTable = table1.dataTable({
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
            "columnDefs": [{   'orderable': false, "searching": true,"autoWidth": false,'targets': [0]},
             { 'targets': [5],   visible: false},
             { 'targets': [0,2,4], width: '50px' },{ 'targets': [1], width: '150px' }, { 'targets': [3], width: '90px' },             
            ],
            "lengthMenu": [   [25, 50, 100, -1], [25, 50, 100, "All"]  ],
            "sScrollY": '300px',
            "sScrollX": true,
            "aaSorting": [],          
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },       
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {}
        });

        $('#tblHeadRowlnkRule').addClass('gridHeader'); $('#ToolTables_dataGridlnkRule_0,#ToolTables_dataGridlnkRule_1').hide(); 
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
        GetRulesGrid();

        //$('.dataTables_scrollHeadInner table').on('click', 'thead tr', function (e) {
        //    e.preventDefault(); if (e.target.innerText == 'New') {   $('#dvNewDet').empty();  var divtag = fnheaderFormatDetails(oRTable, 'New');  $('#dvNewDet').append(divtag); $("#ModalNew").modal('show'); }
        //});

        oRTable.on('click', 'tbody td', function (e) {
            var selectedTr = $(this).parents('tr')[0];     var aData = oRTable.fnGetData(selectedTr);  _targetclick = '';
            if (oRTable.fnIsOpen(selectedTr) && (e.target.innerText == 'Edit')) {  oRTable.fnClose(selectedTr);}
            else {
                var divid = 'dv' + selectedTr._DT_RowIndex;
                if (e.target.className == 'glyphicon glyphicon-pencil') { _targetclick = e.target.innerText;
                    if ((previousTr != '') && (oRTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {
                        var prevdivid = 'dv' + previousTr._DT_RowIndex; oRTable.fnClose(previousTr); $('#' + prevdivid).show();
                    }
                    if (aData != null) {  oRTable.fnOpen(selectedTr, fnFormatDetails(oRTable, selectedTr, 'Edit'), 'details');  $('#' + divid).hide();  previousTr = selectedTr;}
                }
                else if (e.target.className == 'glyphicon glyphicon-trash') {
                    if (confirm('Are you sure ? You want to delete Rule ?')) { var _ruleid = oRTable.fnGetData(selectedTr)[7]; DeleteRulesDetails(_ruleid, GetRulesGrid); } previousTr = '';
                }
                else if (e.target.innerText == 'Link Details'){
                    var _rule = oRTable.fnGetData(selectedTr)[3]; var _ruleid = oRTable.fnGetData(selectedTr)[7];  GetLinkRulesGrid(_ruleid);$('#Lnkdettitle').text('Link Details for Rule : ' + _rule);    $("#ModalLinKDet").modal('show');
                }
               
                $('#btnUpdate').click(function () {
                    if (aData != null) { var _ruleid = aData[7]; var _res = ValidateDetail(selectedTr.rowIndex, _ruleid);
                        if (_res == true) { _mapdet = []; GetRuleDetails(oRTable, selectedTr, _mapdet, 'Edit'); UpdateRuledet(_mapdet, GetRulesGrid, 'Edit'); }
                        $('#' + divid).show(); previousTr = '';
                    }
                });
                $('#btnCancel').click(function () {  if (oRTable.fnIsOpen(selectedTr)) { oRTable.fnClose(selectedTr);  } $('#' + divid).show(); previousTr = ''; });
            }
        });   

        function fnFormatDetails(oTable, nTr, _targetclick) {
            var sOut = ''; var _str = ''; var indx = '';
            var _doctxt = ''; var _RuleNo = ''; var _Doctype = ''; var _Rule = ''; var _Comments = ''; var _Descr = ''; var aData = '';
            if (_targetclick == "Edit") { indx = nTr.rowIndex; } else { indx = 0; }
            var tid = "RuleTable" + indx; var _tbodyid = "tblBodyRule" + indx; var _RuleNoid = 'txtRuleNo' + indx; var _Ruleid = 'txtRule' + indx;
            var _Commentsid = 'txtComments' + indx; var _Descrid = 'txtDescr' + indx; var _doctypeid = 'txtDocType' + indx;
            if (_targetclick == 'Edit') { var aData = oRTable.fnGetData(nTr); _RuleNo = Str(aData[1]); _Doctype = Str(aData[2]); _Rule = Str(aData[3]); _Comments = Str(aData[5]); _Descr = Str(aData[4]); }
            else if (_targetclick == 'New') { _doctxt = ''; _RuleNo = ''; _Doctype = ''; _Rule = ''; _Comments = ''; _Descr = ''; }
            var btndiv = '<div class="row"><div class="col-md-8" style="float:right;padding-left:10%;"><a href="#" id="btnUpdate"><u>Update</u></<a>&nbsp;<a href="#" id="btnCancel"><u>Cancel</u></<a></div></div>';
            var sOut = '<div class="row"><div class="col-md-10"><div class="form-group">' +
               ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Rule No. </label> </div>' +
               ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _RuleNoid + '"  value="' + _RuleNo + '"/> </div>' +
               ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Document Type </label> </div>' +
               ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _doctypeid + '"  value="' + _Doctype + '"/> </div>' +
               '</div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
              ' <div class="col-md-2" style="text-align:right;margin-top:5px;"> <label> Rule </label> </div>' +
              ' <div  class="col-md-4" ><input type="text" class="form-control" id="' + _Ruleid + '" value="' + _Rule + '" /> </div>' +
              ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
              ' <div class="col-md-2" style="text-align:right;margin-top:5px;"> <label >Description </div>' +
              ' <div  class="col-md-10"> <textarea  style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _Descrid + '">' + _Descr + '</textarea> </div>' +
              ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
              ' <div class="col-md-2" style="text-align:right;"> <label>Comments</label> </div>' +
              ' <div  class="col-md-10" > <textarea style="width:100%;border:1px solid #c2cad8;" rows="3" id="' + _Commentsid + '">' + _Comments + '</textarea></div>' +
              '</div></div></div>' + btndiv;
            return sOut;
        };

        function fnheaderFormatDetails(oTable, _targetclick) {
            var sOut = ''; var _str = ''; var indx = 0; var btndiv = ''; var _doctxt = ''; var _RuleNo = ''; var _Doctype = ''; var _Rule = ''; var _Comments = ''; var _Descr = ''; var aData = '';
            var tid = "RuleTable" + indx; var _tbodyid = "tblBodyRule" + indx; var _RuleNoid = 'txtRuleNo' + indx; var _Ruleid = 'txtRule' + indx; var _Commentsid = 'txtComments' + indx;
            var _Descrid = 'txtDescr' + indx; var _doctypeid = 'txtDocType' + indx; if (_targetclick == 'New') { _doctxt = ''; _RuleNo = ''; _Doctype = ''; _Rule = ''; _Comments = ''; _Descr = ''; }
            var sOut = '<div class="row"><div class="col-md-12"><div class="form-group">' +
                   ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Rule No. </label> </div>' +
                   ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _RuleNoid + '"  value="' + _RuleNo + '"/> </div>' +
                   ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Document Type </label> </div>' +
                   ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _doctypeid + '"  value="' + _Doctype + '"/> </div>' +
                   ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"> <label> Rule </label> </div>' +
                  ' <div  class="col-md-4" ><input type="text" class="form-control" id="' + _Ruleid + '" value="' + _Rule + '" /> </div>' +
                  ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"> <label >Description </div>' +
                  ' <div  class="col-md-10"> <textarea class="textarea-success" rows="2" id="' + _Descrid + '">' + _Descr + '</textarea> </div>' +
                  ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                  ' <div class="col-md-2" style="text-align:right;"> <label>Comments</label> </div>' +
                  ' <div  class="col-md-10" > <textarea style="width:100%;border:1px solid #c2cad8;" rows="3" id="' + _Commentsid + '">' + _Comments + '</textarea></div>' +
                  '</div></div></div>';
            return sOut;
        };

        $('#btnNew').click(function () { var aRow = ''; var _res = ValidateDetail(0, 0);
            if (_res == true) {   _mapdet = [];  GetRuleDetails(oRTable, '', _mapdet, 'New'); UpdateRuledet(_mapdet, GetRulesGrid, 'New'); }  });
        $("#ModalLinKDet").on('shown.bs.modal', function () { oSTable.fnDraw(); });


        $('#btnRefresh').live('click', function (e) { e.preventDefault(); GetRulesGrid(); });
        $('#btnClear').live('click', function (e) { e.preventDefault(); ClearFilter(); });
        $('#btnNewRule').live('click', function (e) {
            e.preventDefault(); $('#dvNewDet').empty(); var divtag = fnheaderFormatDetails(oRTable, 'New');
            $('#dvNewDet').append(divtag); $("#ModalNew").modal('show');
        });
    };

    function GetRuleDetails(Table, nTr, _lstdet, _targetclick) {
        var indx = '';if (nTr != '') { indx = nTr.rowIndex; } else { indx = 0; }
        var tid = "RuleTable" + indx; var _ruleno = $('#txtRuleNo' + indx).val(); var _rule = $('#txtRule' + indx).val(); var _comments = $('#txtComments' + indx).val();
        var _descr = $('#txtDescr' + indx).val(); var _doctype = $('#txtDocType' + indx).val();      
        _lstdet.push("RULE_NUMBER" + "|" + Str(_ruleno)); _lstdet.push("DOC_TYPE" + "|" + Str(_doctype)); _lstdet.push("RULE_CODE" + "|" + Str(_rule));
        _lstdet.push("RULE_DESC" + "|" + Str(_descr)); _lstdet.push("RULE_COMMENTS" + "|" + Str(_comments));        
        if (_targetclick == 'Edit') { var _ruleid = Table.fnGetData(nTr)[7]; _lstdet.push("RULEID" + "|" + Str(_ruleid)); }        
    };

    function FillRulesGrid(Table) {
        try {
            $('#dataGridRule').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridRule').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    var _arrcommt = Str(Table[i].RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str(''); cells[1] = Str(Table[i].RULE_NUMBER);cells[2] = Str(Table[i].DOC_TYPE);  cells[3] = Str(Table[i].RULE_CODE);
                    cells[4] = Str(Table[i].RULE_DESC); cells[5] = _arrcommt; cells[6] = Str('');  cells[7] = Str(Table[i].RULEID);                 
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.RULEID != undefined && Table.RULEID != null) {
                    var t = $('#dataGridRule').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    var _arrcommt = Str(Table.RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str('');   cells[1] = Str(Table.RULE_NUMBER);   cells[2] = Str(Table.DOC_TYPE); cells[3] = Str(Table.RULE_CODE);
                    cells[4] = Str(Table.RULE_DESC);cells[5] = _arrcommt;cells[6] = Str(''); cells[7] = Str(Table.RULEID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e) { }
    };

    var GetRulesGrid = function () {
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "Rules.aspx/GetRulesGrid",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table;  FillRulesGrid(Table);}
                    else $('#dataGridRule').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Rules :' + err);  }
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

    function ValidateDetail(indx,ruleid) {
        var _valid = true; var _rcode = $('#txtRule' + indx).val(); var _rdesc = $('#txtDescr' + indx).val(); var _rno = $('#txtRuleNo' + indx).val();
        if (_rno == '') { $('#txtRuleNo' + indx).addClass('error'); _valid = false; } else { $('#txtRuleNo' + indx).removeClass('error'); _valid = true;}
        if (_rcode == '') { $('#txtRule' + indx).addClass('error'); _valid = false; }
        if (_rdesc == '') { $('#txtDescr' + indx).removeClass('textarea-success').addClass('textarea-Error'); _valid = false; }// else { $('#txtDescr' + indx).removeClass('error'); _valid = true; }
        else if (_rcode != '' && _rdesc!='' &&  _rno!='') {
            $('#txtDescr' + indx).removeClass('textarea-Error').addClass('textarea-success');
            if (ruleid == 0) {
                var isexist = CheckExistingRule(_rcode, ruleid);
                if (isexist == 0) {
                    $('#txtRule' + indx).removeClass('error'); _valid = true;

                } else { $('#txtRule' + indx).addClass('error'); _valid = false; }
            }
        }
        return _valid;
    };

    function UpdateRuledet(_nfieldval, callback, targetClick) {  if (targetClick == 'Edit') { SaveRulesDetails(_nfieldval, callback); } else if (targetClick == 'New') { AddRuleDetails(_nfieldval, callback); } };

    function SaveRulesDetails(_nfieldval, callback) {
        var slRuledet = [];
        for (var j = 0; j < _nfieldval.length; j++) { slRuledet.push(_nfieldval[j]); }  var data2send = JSON.stringify({ slRuledet: slRuledet });
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "Rules.aspx/UpdateRuleDetails",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "Rule Saved successfully."); GetRulesGrid(); }
                    Metronic.unblockUI();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Rule details :' + err); Metronic.unblockUI(); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };

    function DeleteRulesDetails(RULEID, callback) {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "Rules.aspx/DeleteRule",
            data: "{ 'RULEID':'" + RULEID + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "Rule Deleted.");   GetRulesGrid();  } Metronic.unblockUI();
                }
                catch (err) {   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete Rules :' + err); Metronic.unblockUI(); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI();},
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI();    }
        });
    };

    function AddRuleDetails(_nfieldval, callback) {
        var slRuledet = [];for (var j = 0; j < _nfieldval.length; j++) { slRuledet.push(_nfieldval[j]);} var data2send = JSON.stringify({ slRuledet: slRuledet });
        Metronic.blockUI('#portlet_body');
            $.ajax({
                type: "POST",
                async: false,
                url: "Rules.aspx/AddRuleDetails",
                data: data2send,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d);
                        if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "Rule added successfully.");    GetRulesGrid(); $("#ModalNew").modal('hide');  }
                        Metronic.unblockUI();  
                    }
                    catch (err) {  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to add XLS Rule :' + err); Metronic.unblockUI();  }
                },
                failure: function (response) {toastr.error("failure get", response.d); Metronic.unblockUI();},
                error: function (response) {   toastr.error("error get", response.responseText); Metronic.unblockUI(); }
            });
    };

    var CheckExistingRule =  function(RULE_CODE, RULEID) {
        var res = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "Rules.aspx/CheckExistingRule",
            data: "{ 'RULE_CODE':'" + RULE_CODE + "','RULEID':'" + (RULEID) + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    res = Str(response.d);
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'cannot validate new rule :' + err);}
            },
            failure: function (response) {  toastr.error("failure get", response.d); },
            error: function (response) {   toastr.error("error get", response.responseText); }
        });
        return res;
    };

    function FillLinkRulesGrid(Table) {
        try {
            $('#dataGridlnkRule').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridlnkRule').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');                    
                    cells[0] = Str(Table[i].BUYER_CODE); cells[1] = Str(Table[i].BUYER_NAME);  cells[2] = Str(Table[i].SUPPLIER_CODE);
                    cells[3] = Str(Table[i].SUPPLIER_NAME);   cells[4] = Str(Table[i].RULE_VALUE);      cells[5] = Str(Table[i].LINNKID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.LINNKID != undefined && Table.LINNKID != null) {
                    var t = $('#dataGridlnkRule').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str(Table.BUYER_CODE);  cells[1] = Str(Table.BUYER_NAME); cells[2] = Str(Table.SUPPLIER_CODE); cells[3] = Str(Table.SUPPLIER_NAME);
                    cells[4] = Str(Table.RULE_VALUE); cells[5] = Str(Table.LINNKID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)  { }
    };

    var GetLinkRulesGrid = function (RULEID) {
        setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "Rules.aspx/LinkRuleDetails",
            data: "{ 'RULEID':'" + RULEID + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d); if (DataSet.NewDataSet != null) {  Table = DataSet.NewDataSet.Table;  FillLinkRulesGrid(Table); }
                    else $('#dataGridlnkRule').DataTable().clear().draw();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Rule detail :' + err);   }
            },
            failure: function (response) {  toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
        }, 200);
    };

    var setupLinkDetTableHeader = function () {
        var dtfilter = '<th>Buyer Code</th><th>Buyer Name</th><th>Supplier Code</th><th>Supplier Name</th> <th>Rule Value</th><th>LINNKID</th>';
        $('#tblHeadRowlnkRule').empty().append(dtfilter);  $('#tblBodylnkRule').empty();
    };

    var setupTableHeader = function () {
        setFilterToolbar();
        var dtfilter = '<th></th><th>Rule No.</th><th>Doc Type</th><th>Rule</th><th>Description</th> <th>Comments</th><th>View Links</th><th>RULEID</th>';
        $('#tblHeadRowRule').empty().append(dtfilter);   $('#tblBodyRule').empty();
    };

    var setFilterToolbar = function () {
        $('#toolbtngroup').empty();
        var _btns = ' <div class="row" style="margin-bottom: 2px;padding-right: 20px;"> <div class="col-md-12">' +
            ' <div id="toolbtngroup" >' +
            ' <span title="New" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnNewRule"><i class="fa fa-plus" style="text-align:center;"></i></a></div>' +
            ' <span title="Clear" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnClear"><i class="fa fa-eraser" style="text-align:center;"></i></a></div>' +
            ' <span title="Refresh" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"> <a href="javascript:;" class="toolbtn" id="btnRefresh"><i class="fa fa-recycle" style="text-align:center;"></i></a></div></div>' +
            ' </div></div>';
        $('#toolbtngroup').append(_btns);
    };
    function ClearFilter() { setFilterToolbar(); $('#dataGridRule').DataTable().clear().draw(); };

    return { init: function () {handleRulesTable(); } };
}();