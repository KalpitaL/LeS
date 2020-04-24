var _mapdet = []; var _lstByrs = []; var _lstDocType = []; var selectedTr = ''; var previousTr = ''; var _targetclick = '';

var XlsBuyerConfig_MapCode = function () {

    var handleXlsBuyerConfig_MapCodeTable = function () {
        var nEditing = null; var _isxlsupdate = -2; var nNew = false; $('#pageTitle').empty().append('XLS Buyer Config');
        SetupBreadcrumb('Home', 'Home.aspx', 'Mapping', '#', 'XLS Buyer Config', 'XLS_Buyer_Config.aspx');
        $(document.getElementById('lnkMapping')).addClass('active open'); $(document.getElementById('spXLS')).addClass('title font-title SelectedColor');              
        setupTableHeader(); setFilterToolbar();
        var table = $('#dataGridXLSByCfg');
        var oXMTable = table.dataTable({
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
            "columnDefs": [{ 'orderable': false, "searching": true,"autoWidth": false,  'targets': [0] },
           { 'targets': [11,12,13, 14, 15], visible: false  }, { 'targets': [0], width: '5px' }, { 'targets': [1], width: '20px' ,'bSortable':false},{ 'targets': [2], width: '70px' },    
           { 'targets': [3], width: '50px' }, { 'targets': [4], width: '40px' }, { 'targets': [5], width: '120px' },{ 'targets': [6,9], width: '40px' },
           { 'targets': [7,8,10], width: '150px', "sClass": "break-det" }    
            ],
            "lengthMenu": [  [25, 50, 100, -1],[25, 50, 100, "All"]   ],
            "scrollY": '300px',
            "sScrollX": "1080px",
            "aaSorting":[],
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var chkid = 'chk' + nRow._DT_RowIndex; var divid = 'dv' + nRow._DT_RowIndex; var aedit = 'rwedit' + nRow._DT_RowIndex; var adwld = 'rwdelete' + nRow._DT_RowIndex;
                var detTag = ' <div id="' + divid + '" style="text-align:center;"><span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span>' +
                  '<span id=' + adwld + ' class="actionbtn" data-toggle="tooltip" title="Download"><i class="fa fa-download"></i></span></div>';
                if (_isxlsupdate != nRow._DT_RowIndex) { $('td:eq(1)', nRow).html(detTag); }
                var chkTag = '<span style="text-align:center;padding-left:10px;"><input type="checkbox" id= "' + chkid + '" class="checkboxes widelarge"></span>'; $('td:eq(0)', nRow).html(chkTag);
            }
        });

        $('#tblHeadRowXLSByCfg').addClass('gridHeader'); $('#ToolTables_dataGridXLSByCfg_0,#ToolTables_dataGridXLSByCfg_1').hide(); 
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
        GetXLSBuyerConfigGrid();

        oXMTable.on('click', 'tbody td', function (e) {
            var selectedTr = $(this).parents('tr')[0]; var aData = oXMTable.fnGetData(selectedTr); _targetclick = '';
            if (oXMTable.fnIsOpen(selectedTr) && ((e.target.className == 'glyphicon glyphicon-pencil') || (e.target.innerText == 'Copy'))) {
                oXMTable.fnClose(selectedTr);
            }
            else {
                var divid = 'dv' + selectedTr._DT_RowIndex;
                if (e.target.className == 'glyphicon glyphicon-pencil') {
                    _isxlsupdate = selectedTr._DT_RowIndex;   _targetclick = e.target.innerText;
                    if ((previousTr != '') && (oXMTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {
                        var prevdivid = 'dv' + previousTr._DT_RowIndex; oXMTable.fnClose(previousTr); $('#' + prevdivid).show(); }
                    if (aData != null) {  oXMTable.fnOpen(selectedTr, fnXlsFormatDetails(oXMTable, selectedTr, 'Edit'), 'details');   $('#' + divid).hide(); previousTr = selectedTr; }
                }
                else if (e.target.className == 'fa fa-download') { if (aData != null) { var _mapid = aData[13]; DownloadFormat(_mapid); } previousTr = selectedTr; }
                $('#btnXLSUpdate').click(function () {
                    if (aData != null) { var _res = ValidateDetail(selectedTr.rowIndex);
                        if (_res == true) { _mapdet = []; _isxlsupdate = -2;
                            GetXMappRowDetails(oXMTable, selectedTr, _mapdet, e.target.innerText); UpdateXMapdet(_mapdet, GetXLSBuyerConfigGrid, e.target.innerText); $('#' + divid).show();
                        }
                    }
                });
                $('#btnXLSCancel').click(function () {  if (oXMTable.fnIsOpen(selectedTr)) { oXMTable.fnClose(selectedTr);  } $('#' + divid).show(); _isxlsupdate = -2; });
            }
        });
        $('#btnRefresh').live('click', function (e) { e.preventDefault(); GetXLSBuyerConfigGrid(); });
        $('#btnCopy').live('click', function (e) {   e.preventDefault(); var _slMapId = []; FillXlsBuyersList();
            $('input[type=checkbox]:checked').each(function () { var tr = $(this).closest('tr'); if (oXMTable.fnGetData(tr) != null) { var _mapid = oXMTable.fnGetData(tr)[13]; _slMapId.push(Str(_mapid)); } });
            if (_slMapId.length > 0) { $("#ModalCopyFormat").modal('show'); }else { toastr.error("Lighthouse eSolutions Pte. Ltd", "Please Select Format "); }
        });
        $('#btnCopyFormat').live('click', function (e) {  e.preventDefault(); var _slMapId = [];
            $('input[type=checkbox]:checked').each(function () { var tr = $(this).closest('tr'); if (oXMTable.fnGetData(tr) != null) { var _mapid = oXMTable.fnGetData(tr)[13]; _slMapId.push(Str(_mapid)); } });
            if (_slMapId.length > 0) { var _selectBYRID = $('#cbBuyers option:selected').val(); CopyXLSMapping(_selectBYRID, _slMapId, GetXLSBuyerConfigGrid); }  $("#ModalCopyFormat").modal('hide');
        });
        $('#btnUpload').live('click', function (e) { e.preventDefault(); ClearControls(); $("#ModalUploadFormat").modal('show'); });
        $('#btnDelete').live('click', function (e) { e.preventDefault(); var _slMAPID = [];
            $('input[type=checkbox]:checked').each(function () { var tr = $(this).closest('tr'); if (oPMTable.fnGetData(tr) != null) { var _mapid = oPMTable.fnGetData(tr)[12]; _slMAPID.push(Str(_mapid)); } });
            if (_slMAPID.length > 0) { if (confirm('Are you sure ? You want to delete PDF Mapping(s) ?')) { DeletePDFMapDetails(_slMAPID, GetPDFBuyerConfigGrid); }}
        });
    };

    var setupTableHeader = function () {
        $('#toolbtngroup').empty();      
        var dtfilter = '<th></th><th style="text-align:center;">#</th><th>Map Code</th><th>Doc Type</th><th>Buyer Code</th><th>Buyer Name</th>'+
            ' <th>Cell-1</th><th>Cell-1 Value-1</th><th>Cell-1 Value-2</th><th>Cell-2</th><th>Cell-2 Value</th><th>Cell (No Discount)</th> <th>Cell Value (No Discount)</th> ' +
            ' <th>XLS_BUYER_MAPID</th><th>EXCEL_MAPID</th><th>BUYERID</th>'
        $('#tblHeadRowXLSByCfg').empty().append(dtfilter);$('#tblBodyXLSByCfg').empty();
    };

    var setFilterToolbar = function () {
        $('#toolbtngroup').empty();
        var _btns = ' <div class="row" style="margin-bottom: 2px;padding-right: 20px;"> <div class="col-md-12"><div id="toolbtngroup" >' +
            ' <span title="Delete" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnDelete"><i class="fa fa-trash" style="text-align:center;"></i></a></div>' +
            ' <span title="Upload" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnUpload"><i class="fa fa-upload" style="text-align:center;"></i></a></div>' +
            ' <span title="Copy" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnCopy"><i class="fa fa-copy" style="text-align:center;"></i></a></div>' +
            ' <span title="Refresh" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"> <a href="javascript:;" class="toolbtn" id="btnRefresh"><i class="fa fa-recycle" style="text-align:center;"></i></a></div></div>' +
            ' </div></div>';
        $('#toolbtngroup').append(_btns);
    };
   
    return {
        init: function () { handleXlsBuyerConfig_MapCodeTable(); }
    };
}();

function FillXlsBuyersList() {
    _lstByrs = [];
    $.ajax({
        type: "POST",
        async: false,
        url: "PDF_Buyer_Config_MapCode.aspx/GetPDFBuyersOnly",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            try {
                var Dataset = JSON.parse(response.d);
                if (Dataset.NewDataSet != null) {  var Table = Dataset.NewDataSet.Table;
                    if (Table != undefined && Table != null) {
                        if (Table.length != undefined) {   for (var i = 0; i < Table.length; i++) { _lstByrs.push(Str(Table[i].BUYERID) + "|" + Str(Table[i].BUYER_NAME)); }}
                        else { if (Table.BUYERID != undefined) { _lstByrs.push(Str(Table.BUYERID) + "|" + Str(Table.BUYER_NAME)); } }
                    }
                }
                var _byrsdet = FillCombo('', _lstByrs); $('#cbBuyers').empty().append(_byrsdet); 
            }
            catch (err) {   toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd");  }
        },
        failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
        error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
    });
};

function FillXlsDocTypeList() {
    _lstDocType = [];
    $.ajax({
        type: "POST",
        async: false,
        url: "XLS_Buyer_Config.aspx/GetXLSDoctypes",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            try {
                var Dataset = JSON.parse(response.d);
                if (Dataset.NewDataSet != null) { var Table = Dataset.NewDataSet.Table;
                    if (Table != undefined && Table != null) {
                        if (Table.length != undefined) { for (var i = 0; i < Table.length; i++) { _lstDocType.push(Str(Table[i].DOCTYPE) + "|" + Str(Table[i].DOCTYPE));  } }
                        else { if (Table.DOCTYPE != undefined) {   _lstDocType.push(Str(Table.DOCTYPE) + "|" + Str(Table.DOCTYPE));} }
                    }
                }
            }
            catch (err) {   toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd"); }
        },
        failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
        error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
    });
};

function GetXMappRowDetails(Table, nTr, _lstdet, _targetclick) {
    var indx = nTr.rowIndex; var tid = "MappTable" + indx;
    var _mcell1 = $('#txtCell1' + indx).val(); var _mcell1val1 = $('#txtCell1value1' + indx).val(); var _mcell1val2 = $('#txtCell1value2' + indx).val();
    var _mcell2 = $('#txtCell2' + indx).val(); var _mcell2val1 = $('#txtCell2value1' + indx).val(); var _mapcode = $('#txtMapCode' + indx).val();
    var _mCellnodis = $('#txtCellnodis' + indx).val(); var _mcellvalnodis = $('#txtCellvaluenodis' + indx).val();var _doctype = $('#cbDocType' + indx + ' option:selected').val();
    var _xlbuyermapid = Table.fnGetData(nTr)[13]; var _buyid = Table.fnGetData(nTr)[15]; 
    _lstdet.push("FORMAT_MAPCODE" + "|" + Str(_mapcode)); _lstdet.push("MAP_CELL1" + "|" + Str(_mcell1));
    _lstdet.push("MAP_CELL1_VAL1" + "|" + Str(_mcell1val1)); _lstdet.push("MAP_CELL1_VAL2" + "|" + Str(_mcell1val2)); _lstdet.push("MAP_CELL2" + "|" + Str(_mcell2));
    _lstdet.push("MAP_CELL2_VAL" + "|" + Str(_mcell2val1)); _lstdet.push("MAP_CELL_NODISC" + "|" + Str(_mCellnodis));
    _lstdet.push("MAP_CELL_NODISC_VAL" + "|" + Str(_mcellvalnodis)); _lstdet.push("DOC_TYPE" + "|" + Str(_doctype));_lstdet.push("MAPID" + "|" + Str(_xlbuyermapid)); 
};

function FillXLSBuyerConfigGrid(Table) {
    try {
        $('#dataGridXLSByCfg').DataTable().clear().draw();
        if (Table.length != undefined && Table.length != null) {
            var t = $('#dataGridXLSByCfg').dataTable();
            for (var i = 0; i < Table.length; i++) {
                var cells = new Array();
                var r = jQuery('<tr id=' + i + '>');
                cells[0] = Str(''); cells[1] = Str(''); cells[2] = Str(Table[i].FORMAT_MAP_CODE); cells[3] = Str(Table[i].DOC_TYPE);
                cells[4] = Str(Table[i].BUYER_CODE); cells[5] = Str(Table[i].BUYER_NAME); cells[6] = Str(Table[i].MAP_CELL1); cells[7] = Str(Table[i].MAP_CELL1_VAL1);
                cells[8] = Str(Table[i].MAP_CELL1_VAL2); cells[9] = Str(Table[i].MAP_CELL2); cells[10] = Str(Table[i].MAP_CELL2_VAL); cells[11] = Str(Table[i].MAP_CELL_NODISC);
                cells[12] = Str(Table[i].MAP_CELL_NODISC_VAL); cells[13] = Str(Table[i].XLS_BUYER_MAPID); cells[14] = Str(Table[i].EXCEL_MAPID); cells[15] = Str(Table[i].BUYERID);
                var ai = t.fnAddData(cells, false);
            }
            t.fnDraw();
        }
        else {
            if (Table.XLS_BUYER_MAPID != undefined && Table.XLS_BUYER_MAPID != null) {
                var t = $('#dataGridXLSByCfg').dataTable();
                var r = jQuery('<tr id=' + 1 + '>');
                var cells = new Array();
                cells[0] = Str('');  cells[1] = Str(''); cells[2] = Str(Table.FORMAT_MAP_CODE);  cells[3] = Str(Table.DOC_TYPE);  cells[4] = Str(Table.BUYER_CODE);
                cells[5] = Str(Table.BUYER_NAME);  cells[6] = Str(Table.MAP_CELL1); cells[7] = Str(Table.MAP_CELL1_VAL1); cells[8] = Str(Table.MAP_CELL1_VAL2);
                cells[9] = Str(Table.MAP_CELL2);  cells[10] = Str(Table.MAP_CELL2_VAL);  cells[11] = Str(Table.MAP_CELL_NODISC);  cells[12] = Str(Table.MAP_CELL_NODISC_VAL);
                cells[13] = Str(Table.XLS_BUYER_MAPID);  cells[14] = Str(Table.EXCEL_MAPID); cells[15] = Str(Table.BUYERID);
                var ai = t.fnAddData(cells, false);
                t.fnDraw();
            }
        }
    }
    catch (e)
    { }
};

var GetXLSBuyerConfigGrid = function () {
    Metronic.blockUI('#portlet_body');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "XLS_Buyer_Config.aspx/GetXLSBuyerConfigGrid",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table;  FillXLSBuyerConfigGrid(Table); }
                    else $('#dataGridXLSByCfg').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get XLS BuyerConfig :' + err); }
            },
            failure: function (response) {  toastr.error("failure get", response.d); Metronic.unblockUI();},
            error: function (response) {   toastr.error("error get", response.responseText); Metronic.unblockUI();  }
        });
    }, 20);
};

function GetMappingXLSGrid(ADDRESSID, ADDRTYPE) {
    var DataSet = '';
    $.ajax({
        type: "POST",
        async: false,
        url: "XLS_Buyer_Config.aspx/GetXLSBuyerConfig_Addressid",
        data: "{'ADDRESSID':'" + ADDRESSID + "','ADDRTYPE':'"+ADDRTYPE+"'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            try {
                DataSet = JSON.parse(response.d);
            }
            catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get XLS BuyerConfig data :' + err); }
        },
        failure: function (response) { toastr.error("failure get", response.d);},
        error: function (response) { toastr.error("error get", response.responseText); }
    });
    return DataSet;
};

function SaveXLSMappingDetails(_nfieldval, callback) {
    var slXLSMapdet = [];
    for (var j = 0; j < _nfieldval.length; j++) { slXLSMapdet.push(_nfieldval[j]); }
    var data2send = JSON.stringify({ slXLSMapdet: slXLSMapdet });
    Metronic.blockUI('#portlet_body');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "XLS_Buyer_Config_MapCode.aspx/SaveXLSMapping",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "XLS Mapping Details Saved successfully."); callback(); }
                    Metronic.unblockUI();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update XLS Mapping details :' + err); Metronic.unblockUI(); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    }, 20);
};

function DeleteXLSMapDetails(_nfieldval, callback) {
    var slMAPID = [];
    for (var j = 0; j < _nfieldval.length; j++) { slMAPID.push(_nfieldval[j]); } var data2send = JSON.stringify({ slMAPID: slMAPID });
    Metronic.blockUI('#portlet_body');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "XLS_Buyer_Config_MapCode.aspx/DeleteXLSMapping",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "XLS Mapping Deleted."); callback(); } Metronic.unblockUI();
                }
                catch (err) {   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete XLS Mapping :' + err);}
            },
            failure: function (response) {  toastr.error("failure get", response.d); Metronic.unblockUI();  },
            error: function (response) {    toastr.error("error get", response.responseText); Metronic.unblockUI();}
        });
    }, 20);
};

function CopyXLSMapping(BUYERID, _nfieldval, callback) {
    var slXLSMapdet = [];
    for (var j = 0; j < _nfieldval.length; j++) { slXLSMapdet.push(_nfieldval[j]); }  var data2send = JSON.stringify({ BUYERID: BUYERID, slXLSMapdet: slXLSMapdet });
    Metronic.blockUI('#portlet_body');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "XLS_Buyer_Config_MapCode.aspx/CopyXLSMapping",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "XLS Mapping Details Copied successfully."); callback(); }
                    Metronic.unblockUI();
                }
                catch (err) {   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to copy XLS Mapping details :' + err); Metronic.unblockUI();    }
            },
            failure: function (response) {   toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) {  toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    }, 20);
};

function fnXlsFormatDetails(oTable, nTr, _targetclick) {
    var sOut = ''; var _str = ''; var _status = ''; var _selectopt = ''; var _mapcodetxt = ''; var _doctxt = ''; var indx = nTr.rowIndex; var aData = oTable.fnGetData(nTr);
    var tid = "MappTable" + indx; var _tbodyid = "tblBodyXLSByCfg" + indx;var _Bycodeid = 'txBuyercode' + indx; var _ByNameid = 'txBuyername' + indx; 
    var _mcell1id = 'txtCell1' + indx; var _mcell1val1id = 'txtCell1value1' + indx; var _mcell1val2id = 'txtCell1value2' + indx; var _mcell2id = 'txtCell2' + indx;
    var _mcell2val1id = 'txtCell2value1' + indx; var _mcellnodisid = 'txtCellnodis' + indx; var _mcellvalnodisid = 'txtCellvaluenodis' + indx;
    var _docid = 'cbDocType' + indx; var _mapcodeid = 'txtMapCode' + indx; FillXlsDocTypeList();
    if (_targetclick == 'Edit') { _mapcodetxt = Str(aData[2]); _doctxt = Str(aData[3]); } else { _mapcodetxt = ''; _doctxt = ''; }
    var _BCode = Str(aData[4]); var _BName = Str(aData[5]); var _mapcell1 = (_targetclick == 'Edit') ? Str(aData[6]) : '';
    var _mapcell1val1 = (_targetclick == 'Edit') ? Str(aData[7]) : ''; var _mapcell1val2 = (_targetclick == 'Edit') ? Str(aData[8]) : '';
    var _mapcell2 = (_targetclick == 'Edit') ? Str(aData[9]) : ''; var _mapcell2val1 = (_targetclick == 'Edit') ? Str(aData[10]) : '';
    var _mapcellnodis = (_targetclick == 'Edit') ? Str(aData[11]) : ''; var _mapcellvalnodis = (_targetclick == 'Edit') ? Str(aData[12]) : '';
    var cbdoctype = FillCombo(_doctxt, _lstDocType);
    var _celldis = 'Cell \n (No Discount)'; var _cellvaldis = 'Cell Value \n (No Discount)'; _celldis = _celldis.replace(/\n/g, '<br/>'); _cellvaldis = _cellvaldis.replace(/\n/g, '<br/>');
    var btndiv = '<div class="row"><div style="text-align:center;"><a href="#" id="btnXLSUpdate"><u>Update</u></<a>&nbsp;<a href="#" id="btnXLSCancel"><u>Cancel</u></<a></div></div>';
    var sOut = '<div class="row" style="padding-right:63px;"><div class="col-md-10"><div class="row"><div class="col-md-10"><div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Code </label> </div>' +
            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mapcodeid + '"  value="' + _mapcodetxt + '"/></div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel">Document Type</label> </div>' +
            ' <div  class="col-md-3"><select class="bs-select form-control" id="' + _docid + '">' + cbdoctype + '</select> </div>' +
            ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Code </label> </div>' +
            ' <div  class="col-md-3"><span id="' + _Bycodeid + '">' + _BCode + '</span> </div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Name </label></div>' +
            ' <div  class="col-md-3"> <span id="' + _ByNameid + '">' + _BName + '</span> </div>' +          
            ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Cell-1 </label> </div>' +
            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mcell1id + '"  value="' + _mapcell1 + '"/></div>' +
            ' </div></div></div><div class="row"><div class="col-md-10"> <div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Cell-1 Value-1 </label> </div>' +
            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mcell1val1id + '" value="' + _mapcell1val1 + '"/></div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Cell-1 Value-2 </label></div>' +
            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mcell1val2id + '" value="' + _mapcell1val2 + '"/></div>' +
            ' </div></div></div> <div class="row"><div class="col-md-10"> <div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Cell-2 </label> </div>' +
            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mcell2id + '"  value="' + _mapcell2 + '"/></div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Cell-2 Value </label></div>' +
            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mcell2val1id + '" value="' + _mapcell2val1 + '"/></div>' +
            ' </div></div></div><div class="row"><div class="col-md-10"> <div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> ' + _celldis + ' </label> </div>' +
            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mcellnodisid + '"  value="' + _mapcellnodis + '"/></div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel">' + _cellvaldis + '  </label></div>' +
            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mcellvalnodisid + '" value="' + _mapcellvalnodis + '"/></div>' +
            ' </div></div></div>' + btndiv + '</div></div>';
    return sOut;
}

function DownloadFormat(MAPID) {
    try {
        $.ajax({
            type: "POST",
            async: false,
            url: "XLS_Buyer_Config_MapCode.aspx/DownloadXLSFormat",
            data: "{ 'MAPID':'" + MAPID + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { var filefullpath = res.split('|')[0]; var filename = res.split('|')[1]; top.location.href = "../Downloads/" + filename;
                        toastr.success("Lighthouse eSolutions Pte. Ltd", "Download File success"); }
                    else { toastr.error("Lighthouse eSolutions Pte. Ltd", "No Mapping found"); }
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Download File :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
    }
    catch (e) { }
};

function onClientUploadStart(sender, e) {
    var FILENAME = e._fileName;
    $.ajax({
        type: "POST",
        async: false,
        url: "XLS_Buyer_Config_MapCode.aspx/SetUploadPath",
        data: "{'FILENAME':'" + FILENAME + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            try {   result = response.d; }
            catch (err) {  toastr.error('Error in Starting Client Upload :' + err, "Lighthouse eSolutions Pte. Ltd");  }
        },
        failure: function (response) {  toastr.error("Failure in Starting Client Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); result = -1;},
        error: function (response) { toastr.error("Error in Starting Client Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd");  result = -1; }
    });
}

function ClearControls() { jQuery('.ajax__fileupload_fileItemInfo').remove(); $('.ajax__fileupload_topFileStatus').text('Please select file(s) to upload');$('.ajax__fileupload_queueContainer').fadeOut();};

function onClientUploadComplete(sender, e) {
    var file = e._filename;
    $.ajax({
        type: "POST",
        async: false,
        url: "XLS_Buyer_Config_MapCode.aspx/UploadFileMapping",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            try {
                result = response.d;
                if (Str(result) == "") {  jQuery('.ajax__fileupload_fileItemInfo').remove(); $('.ajax__fileupload_topFileStatus').text('Please select file(s) to upload');
                    $('.ajax__fileupload_queueContainer').fadeOut(); toastr.success("Files uploaded", "Lighthouse eSolutions Pte. Ltd");
                }
                else { toastr.error("Error in File Upload", "Lighthouse eSolutions Pte. Ltd"); result = -1; }
            }
            catch (err) { toastr.error('Error in File Upload :' + err, "Lighthouse eSolutions Pte. Ltd"); result = -1; }
        },
        failure: function (response) { toastr.error("Failure in File Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); result = -1; },
        error: function (response) { toastr.error("Error in File Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); result = -1; }
    });
};