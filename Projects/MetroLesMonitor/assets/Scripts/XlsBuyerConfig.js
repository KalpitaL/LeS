var _mapdet = []; var _lstGrp = []; var _lstDocType = []; var selectedTr = ''; var previousTr = '';var _targetclick = '';
var _siteurl = ''; var _refrencefiles = ''; var _fMppdet = [];

var XLSBuyerConfig = function () {

    var handleXLSBuyerConfigTable = function () {
        var nEditing = null; var _isxlsupdate = -2; var nNew = false; $('#pageTitle').empty().append('XLS Buyer Config');
        SetupBreadcrumb('Home', 'Home.aspx', 'Mapping', '#', 'XLS Buyer Config', 'XLS_Buyer_Config.aspx');
        $(document.getElementById('lnkMapping')).addClass('active open'); $(document.getElementById('spXLS')).addClass('title font-title SelectedColor');              
        setupTableHeader();

        var table = $('#dataGridXLSByCfg');
        var oXMTable = table.dataTable({
            "bDestroy": true,
            "bSort":false,
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
            "columnDefs": [{  'orderable': false, "searching": true,"autoWidth": false,  'targets': [0]
            },
           { 'targets': [12,13,14, 15, 16, 17, 18,19], visible: false  },   
           { 'targets': [0], width: '88px' }, { 'targets': [1], width: '45px' }, { 'targets': [2], width: '70px' }, { 'targets': [3, 5, 7, 10], width: '45px' },
           { 'targets': [8, 9, 11], width: '100px', "sClass": "break-det" }, { 'targets': [4, 6], width: '85px' }, { 'targets': [20], width: '100px' },
           { 'targets': [3, 5, 9, 10], "sClass": "visible-lg" },
            ],
            "lengthMenu": [  [25, 50, 100, -1], [25, 50, 100, "All"]  ],
            "scrollY":  '300px',
            "sScrollX": true,
            "aaSorting":[],
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var divid = 'dv' + nRow._DT_RowIndex; var aedit = 'rwedit' + nRow._DT_RowIndex; var adelete = 'rwdelete' + nRow._DT_RowIndex; var acopy = 'rwcopy' + nRow._DT_RowIndex;
                var aupload = 'rwupload' + nRow._DT_RowIndex; var adwnld = 'rwdownload' + nRow._DT_RowIndex;
                var detTag = ' <div id="' + divid + '" style="text-align:center;"><span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span>' +
               '<span id=' + adelete + ' class="actionbtn" data-toggle="tooltip" title="Delete"><i class="glyphicon glyphicon-trash"></i></span>' +
               ' <span id=' + acopy + ' class="actionbtn" data-toggle="tooltip" title="Copy"><i class="fa fa-copy"></i></span>'+
               ' <span id=' + adwnld + ' class="actionbtn" data-toggle="tooltip" title="Download"><i class="fa fa-download"></i></span>' +
               ' <span id=' + aupload + ' class="actionbtn" data-toggle="tooltip" title="Upload"><i class="fa fa-upload"></i></span></div>';
                if (_isxlsupdate != nRow._DT_RowIndex) { $('td:eq(0)', nRow).html(detTag); }
                var _res = aData[20]; if(_res != '' && _res != undefined) { var fileTag = '<a href="javascript:;" name="Ref">' + _res + '</<a>'; $('td:eq(12)', nRow).html(fileTag);}
            }
        });

        $('#tblHeadRowXLSByCfg').addClass('gridHeader'); $('#ToolTables_dataGridXLSByCfg_0,#ToolTables_dataGridXLSByCfg_1').hide(); //$('#dataGridXLSByCfg_info').hide();
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
        GetXLSBuyerConfigGrid(); 

        oXMTable.on('click', 'tbody td', function (e) {
            var selectedTr = $(this).parents('tr')[0]; var aData = oXMTable.fnGetData(selectedTr); _targetclick = '';
            if (oXMTable.fnIsOpen(selectedTr) && ((e.target.className == 'glyphicon glyphicon-pencil') || (e.target.innerText == 'fa fa-copy'))) {
                oXMTable.fnClose(selectedTr);
            }
            else {
                var divid = 'dv' + selectedTr._DT_RowIndex;
                if (e.target.className == 'glyphicon glyphicon-pencil') {
                    _isxlsupdate = selectedTr._DT_RowIndex;   _targetclick = e.target.innerText;
                    if ((previousTr != '') && (oXMTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {
                        var prevdivid = 'dv' + previousTr._DT_RowIndex; oXMTable.fnClose(previousTr); $('#' + prevdivid).show();
                    }
                    if (aData != null) {
                        oXMTable.fnOpen(selectedTr, fnXlsFormatDetails(oXMTable, selectedTr, 'Edit'), 'details');
                        $('#' + divid).hide(); previousTr = selectedTr;
                    }
                }
                else if (e.target.className == 'glyphicon glyphicon-trash') {
                    if (confirm('Are you sure ? You want to delete XLS Mapping ?')) {
                        var _xlsbuyermapid = oXMTable.fnGetData(selectedTr)[14];    DeleteXLSMapDetails(_xlsbuyermapid, GetXLSBuyerConfigGrid);
                    }
                    previousTr = selectedTr;
                }
                else if (e.target.className == 'fa fa-copy') {
                    _mapdet = [];
                    if ((previousTr != '') && (oXMTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {
                        var prevdivid = 'dv' + previousTr._DT_RowIndex;   oXMTable.fnClose(previousTr); $('#' + prevdivid).show();
                    }
                    if (aData != null) {   oXMTable.fnOpen(selectedTr, fnXlsFormatDetails(oXMTable, selectedTr, 'Copy'), 'details'); $('#' + divid).hide();}
                    previousTr = selectedTr;
                }
                else if (e.target.className == 'fa fa-download') {
                    _mapdet = [];
                    if ((previousTr != '') && (oXMTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {var prevdivid = 'dv' + previousTr._DT_RowIndex; oXMTable.fnClose(previousTr); $('#' + prevdivid).show(); }
                    if (aData != null) {
                        var _excelmapid = Str(aData[15]); var _groupid = Str(aData[18]); DownloadFile(_excelmapid,_groupid);$('#' + divid).show();   } previousTr = selectedTr;
                }
                else if (e.target.className == 'fa fa-upload') {
                    _mapdet = [];
                    if ((previousTr != '') && (oXMTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {
                        var prevdivid = 'dv' + previousTr._DT_RowIndex; oXMTable.fnClose(previousTr); $('#' + prevdivid).show();
                    }
                    if (aData != null) { $("#ModalUpload").modal('show'); $('#' + divid).show(); }
                    previousTr = selectedTr;
                }
                else if (e.target.name == 'Ref') {
                    if (aData != null) {
                        var _filename = Str(aData[20]); var _groupcode = Str(aData[2]); top.location.href = "../ReferenceFiles/XLS/" + _groupcode + "/" + _filename;
                    }
                }
                $('#btnXLSUpdate').click(function () {
                    if (aData != null) {
                        var _res = ValidateDetail(selectedTr.rowIndex);
                        if (_res == true) {
                            _mapdet = []; _isxlsupdate = -2;
                            GetXMappRowDetails(oXMTable, selectedTr, _mapdet, e.target.className); UpdateXMapdet(_mapdet, GetXLSBuyerConfigGrid, e.target.className);
                            $('#' + divid).show(); 
                        }
                    }
                });
                $('#btnXLSCancel').click(function () {  if (oXMTable.fnIsOpen(selectedTr)) { oXMTable.fnClose(selectedTr);  } $('#' + divid).show(); _isxlsupdate = -2; });
            }
        });
        $("#ModalUpload .close").click(function () { $("#ModalUpload").modal('hide'); }); jQuery('.ajax__fileupload_dropzone').remove();
        $('#btnCancelUpload').on("click", function (e) { ClearControls(); $("#ModalUpload").modal('hide'); });
        $('#btnUploadFiles').on("click", function (e) {
            var _mppfiles = $('input[type="file"]').get(0).files;
            var _reffiles = $('input[type="file"]').get(1).files;
            var _lst = [];
            if (_mppfiles.length > 0 && _reffiles.length > 0) { UploadFiles(); }
            else { toastr.error("Please select both Mapping and Reference Files ", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    var setupTableHeader = function () {
        $('#toolbtngroup').empty();
        var dtfilter = '<th style="text-align:center;">#</th><th>LinkId</th><th>Group</th><th>Buyer <br/> Code</th><th>Buyer Name</th><th>Supplier<br/> Code</th>' +
           ' <th>Supplier Name</th><th>Cell-1</th><th>Cell-1 <br/>Value-1</th><th>Cell-1 <br/>Value-2</th><th>Cell-2</th><th>Cell-2 <br/>Value</th><th>Cell (No Discount)</th>' +
           ' <th>Cell Value (No Discount)</th>  <th>XLS_BUYER_MAPID</th><th>EXCEL_MAPID</th><th>BUYERID</th><th>SUPPLIERID</th> <th>GROUP_ID</th><th>DOC_TYPE</th><th>Sample File</th>';
        $('#tblHeadRowXLSByCfg').empty().append(dtfilter);$('#tblBodyXLSByCfg').empty();
    };

    function UploadFiles() {
        $.ajax({
            type: "POST",
            async: false,
            url: "XLS_Buyer_Config.aspx/UploadFileMapping",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    result = response.d; if (Str(result) != "") { toastr.success(result, "Lighthouse eSolutions Pte. Ltd"); }
                    else { toastr.error('Mapping values are not in correct format. Please Check file and upload it again.', "Lighthouse eSolutions Pte. Ltd"); }
                    ClearControls();
                }
                catch (err) { toastr.error('Error in File Upload :' + err, "Lighthouse eSolutions Pte. Ltd"); result = -1; }
            },
            failure: function (response) { toastr.error("Failure in File Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); result = -1; },
            error: function (response) { toastr.error("Error in File Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); result = -1; }
        });
    };
   
    return {
        init: function () { handleXLSBuyerConfigTable(); }
    };
}();

function FillXlsGroupList() {
    $.ajax({
        type: "POST",
        async: false,
        url: "XLS_Buyer_Config.aspx/GetXLSGroupsOnly",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            try {
                var Dataset = JSON.parse(response.d);
                if (Dataset.NewDataSet != null) {
                    var Table = Dataset.NewDataSet.Table;
                    if (Table != undefined && Table != null) {
                        if (Table.length != undefined) {
                            for (var i = 0; i < Table.length; i++) {
                                _lstGrp.push(Str(Table[i].GROUP_ID) + "|" + Str(Table[i].GROUP_CODE));
                            }
                        }
                        else {
                            if (Table.GROUP_ID != undefined) {
                                _lstGrp.push(Str(Table.GROUP_ID) + "|" + Str(Table.GROUP_CODE));
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
                if (Dataset.NewDataSet != null) {
                    var Table = Dataset.NewDataSet.Table;
                    if (Table != undefined && Table != null) {
                        if (Table.length != undefined) {
                            for (var i = 0; i < Table.length; i++) {
                                _lstDocType.push(Str(Table[i].DOCTYPE) + "|" + Str(Table[i].DOCTYPE));
                            }
                        }
                        else {
                            if (Table.DOCTYPE != undefined) {
                                _lstDocType.push(Str(Table.DOCTYPE) + "|" + Str(Table.DOCTYPE));
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

function GetXMappRowDetails(Table, nTr, _lstdet, _targetclick) {
    var indx = nTr.rowIndex; var tid = "MappTable" + indx;
    var _mcell1 = $('#txtCell1' + indx).val(); var _mcell1val1 = $('#txtCell1value1' + indx).val(); var _mcell1val2 = $('#txtCell1value2' + indx).val();
    var _mcell2 = $('#txtCell2' + indx).val(); var _mcell2val1 = $('#txtCell2value1' + indx).val();
    var _mCellnodis = $('#txtCellnodis' + indx).val(); var _mcellvalnodis = $('#txtCellvaluenodis' + indx).val();
    var _grpid = $('#cbGroup' + indx + ' option:selected').val(); if (_grpid == '' || _grpid == undefined) { _grpid = ''; } var _doctype = $('#cbDocType' + indx + ' option:selected').val();
    var _xlbuyermapid = Table.fnGetData(nTr)[14]; var _buyid = Table.fnGetData(nTr)[16]; var _suppid = Table.fnGetData(nTr)[17];
    _lstdet.push("GROUP_ID" + "|" + Str(_grpid)); _lstdet.push("MAP_CELL1" + "|" + Str(_mcell1));
    _lstdet.push("MAP_CELL1_VAL1" + "|" + Str(_mcell1val1)); _lstdet.push("MAP_CELL1_VAL2" + "|" + Str(_mcell1val2)); _lstdet.push("MAP_CELL2" + "|" + Str(_mcell2));
    _lstdet.push("MAP_CELL2_VAL" + "|" + Str(_mcell2val1)); _lstdet.push("MAP_CELL_NODISC" + "|" + Str(_mCellnodis));
    _lstdet.push("MAP_CELL_NODISC_VAL" + "|" + Str(_mcellvalnodis)); _lstdet.push("DOC_TYPE" + "|" + Str(_doctype));
    if (_targetclick == 'fa fa-copy') { _lstdet.push("COPY_BUYERID" + "|" + Str(_buyid)); _lstdet.push("COPY_SUPPLIERID" + "|" + Str(_suppid)); } else { _lstdet.push("XLS_BUYER_MAPID" + "|" + Str(_xlbuyermapid)); }
};

function FillXLSBuyerConfigGrid(Table) {
    try {
        $('#dataGridXLSByCfg').DataTable().clear().draw();
        if (Table.length != undefined && Table.length != null) {
            var t = $('#dataGridXLSByCfg').dataTable();
            for (var i = 0; i < Table.length; i++) {
                var cells = new Array();
                var r = jQuery('<tr id=' + i + '>');
                cells[0] = Str(''); cells[1] = Str(Table[i].BUYER_SUPP_LINKID); cells[2] = Str(Table[i].GROUP_CODE); cells[3] = Str(Table[i].BUYER_CODE);
                cells[4] = Str(Table[i].BUYER_NAME);cells[5] = Str(Table[i].SUPPLIER_CODE); cells[6] = Str(Table[i].SUPPLIER_NAME); cells[7] = Str(Table[i].MAP_CELL1);
                cells[8] = Str(Table[i].MAP_CELL1_VAL1); cells[9] = Str(Table[i].MAP_CELL1_VAL2);   cells[10] = Str(Table[i].MAP_CELL2);
                cells[11] = Str(Table[i].MAP_CELL2_VAL);  cells[12] = Str(Table[i].MAP_CELL_NODISC); cells[13] = Str(Table[i].MAP_CELL_NODISC_VAL);
                cells[14] = Str(Table[i].XLS_BUYER_MAPID); cells[15] = Str(Table[i].EXCEL_MAPID); cells[16] = Str(Table[i].BUYERID);
                cells[17] = Str(Table[i].SUPPLIERID); cells[18] = Str(Table[i].GROUP_ID); cells[19] = Str(Table[i].DOC_TYPE); cells[20] = Str(Table[i].SampleFile);
                var ai = t.fnAddData(cells, false);
            }
            t.fnDraw();
        }
        else {
            if (Table.XLS_BUYER_MAPID != undefined && Table.XLS_BUYER_MAPID != null) {
                var t = $('#dataGridXLSByCfg').dataTable();
                var r = jQuery('<tr id=' + 1 + '>');
                var cells = new Array();
                cells[0] = Str(''); cells[1] = Str(Table.BUYER_SUPP_LINKID); cells[2] = Str(Table.GROUP_CODE); cells[3] = Str(Table.BUYER_CODE);
                cells[4] = Str(Table.BUYER_NAME); cells[5] = Str(Table.SUPPLIER_CODE); cells[6] = Str(Table.SUPPLIER_NAME); cells[7] = Str(Table.MAP_CELL1);
                cells[8] = Str(Table.MAP_CELL1_VAL1); cells[9] = Str(Table.MAP_CELL1_VAL2); cells[10] = Str(Table.MAP_CELL2);
                cells[11] = Str(Table.MAP_CELL2_VAL); cells[12] = Str(Table.MAP_CELL_NODISC); cells[13] = Str(Table.MAP_CELL_NODISC_VAL);
                cells[14] = Str(Table.XLS_BUYER_MAPID); cells[15] = Str(Table.EXCEL_MAPID); cells[16] = Str(Table.BUYERID);
                cells[17] = Str(Table.SUPPLIERID); cells[18] = Str(Table.GROUP_ID); cells[19] = Str(Table.DOC_TYPE); cells[20] = Str(Table.SampleFile);
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
                    if (DataSet.NewDataSet != null) {  Table = DataSet.NewDataSet.Table;   FillXLSBuyerConfigGrid(Table);}
                    else $('#dataGridXLSByCfg').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get XLS BuyerConfig :' + err);  }
            },
            failure: function (response) {  toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) {toastr.error("error get", response.responseText); Metronic.unblockUI();  }
        });
    }, 180);
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
            try { DataSet = JSON.parse(response.d); }
            catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get XLS BuyerConfig data :' + err); }
        },
        failure: function (response) { toastr.error("failure get", response.d);},
        error: function (response) { toastr.error("error get", response.responseText); }
    });
    return DataSet;
};

function ValidateDetail(indx) {
    var _valid = true;
    var _grpid = $('#cbGroup' + indx + ' option:selected').val();
    if (_grpid == '') { $($('#cbGroup' + indx).select2('container')).addClass('error'); _valid = false; }
    else { $($('#cbGroup' + indx).select2('container')).removeClass('error'); }
    return _valid;
};

function UpdateXMapdet(_nfieldval, callback, targetClick) {
    if (targetClick == 'glyphicon glyphicon-pencil') { SaveXLSMappingDetails(_nfieldval, callback); }
    else if (targetClick == 'fa fa-copy') { CopyXLSMappingDetails(_nfieldval, callback); }
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
            url: "XLS_Buyer_Config.aspx/UpdateXLSMapping",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") {    toastr.success("Lighthouse eSolutions Pte. Ltd", "XLS Mapping Saved successfully."); callback(); }
                    Metronic.unblockUI();
                }
                catch (err) {   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update XLS Mapping details :' + err); Metronic.unblockUI(); }
            },
            failure: function (response) {   toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI();}
        });
    }, 20);
};

function DeleteXLSMapDetails(XLS_BUYER_MAPID, callback) {
    Metronic.blockUI('#portlet_body');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "XLS_Buyer_Config.aspx/DeleteXLSMapping",
            data: "{ 'XLS_BUYER_MAPID':'" + XLS_BUYER_MAPID + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") {    toastr.success("Lighthouse eSolutions Pte. Ltd", "XLS Mapping Deleted.");    callback();  }
                    Metronic.unblockUI();
                }
                catch (err) {   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete XLS Mapping :' + err); Metronic.unblockUI();}
            },
            failure: function (response) {   toastr.error("failure get", response.d); Metronic.unblockUI();  },
            error: function (response) {  toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    }, 20);
};

function CopyXLSMappingDetails(_nfieldval, callback) {
    var slXLSMapdet = [];
    for (var j = 0; j < _nfieldval.length; j++) {    slXLSMapdet.push(_nfieldval[j]); }
    var data2send = JSON.stringify({ slXLSMapdet: slXLSMapdet });
    Metronic.blockUI('#portlet_body');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "XLS_Buyer_Config.aspx/CopyXLSMapping",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "XLS Mapping Details Copied successfully."); callback(); }
                    Metronic.unblockUI();
                }
                catch (err) {  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to copy XLS Mapping details :' + err); Metronic.unblockUI(); }
            },
            failure: function (response) {  toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    }, 20);
};

function fnXlsFormatDetails(oTable, nTr, _targetclick) {
    var sOut = ''; var _str = ''; var _status = ''; var _selectopt = ''; var _grptxt = ''; var _doctxt = '';
    var indx = nTr.rowIndex; var aData = oTable.fnGetData(nTr); var tid = "MappTable" + indx; var _tbodyid = "tblBodyXLSByCfg" + indx;
    var _Bycodeid = 'txBuyercode' + indx; var _ByNameid = 'txBuyername' + indx; var _Suppcodeid = 'txSuppcode' + indx; var _SuppNameid = 'txSuppname' + indx;
    var _mcell1id = 'txtCell1' + indx; var _mcell1val1id = 'txtCell1value1' + indx; var _mcell1val2id = 'txtCell1value2' + indx; var _mcell2id = 'txtCell2' + indx;
    var _mcell2val1id = 'txtCell2value1' + indx; var _mcellnodisid = 'txtCellnodis' + indx; var _mcellvalnodisid = 'txtCellvaluenodis' + indx;
    var _grpid = 'cbGroup' + indx; var _docid = 'cbDocType' + indx; FillXlsGroupList(); FillXlsDocTypeList();
    if (_targetclick == 'Edit') { _grptxt = Str(aData[2]); _doctxt = Str(aData[19]); } else { _grptxt = ''; _doctxt = ''; }
    var _BCode = Str(aData[3]); var _BName = Str(aData[4]); var _SCode = Str(aData[5]); var _SName = Str(aData[6]);
    var _mapcell1 = (_targetclick == 'Edit') ? Str(aData[7]) : ''; var _mapcell1val1 = (_targetclick == 'Edit') ? Str(aData[8]) : '';
    var _mapcell1val2 = (_targetclick == 'Edit') ? Str(aData[9]) : '';  var _mapcell2 = (_targetclick == 'Edit') ? Str(aData[10]) : '';
    var _mapcell2val1 = (_targetclick == 'Edit') ? Str(aData[11]) : ''; var _mapcellnodis = (_targetclick == 'Edit') ? Str(aData[12]) : '';
    var _mapcellvalnodis = (_targetclick == 'Edit') ? Str(aData[13]) : '';  var cbgrp = FillCombo(_grptxt, _lstGrp); var cbdoctype = FillCombo(_doctxt, _lstDocType);
    var _celldis = 'Cell \n (No Discount)'; var _cellvaldis = 'Cell Value \n (No Discount)'; _celldis = _celldis.replace(/\n/g, '<br/>'); _cellvaldis = _cellvaldis.replace(/\n/g, '<br/>');
    var btndiv = '<div class="row"><div style="text-align:center;"><a href="#" id="btnXLSUpdate"><u>Update</u></<a>&nbsp;<a href="#" id="btnXLSCancel"><u>Cancel</u></<a></div></div>';
    var sOut = '<div class="row" style="padding-right:63px;"><div class="col-md-10"><div class="row"><div class="col-md-10"><div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Group </label> </div>' +
            ' <div  class="col-md-3"><select class="bs-select form-control" id="' + _grpid + '">' + cbgrp + '</select> </div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel">Document Type</label> </div>' +
            ' <div  class="col-md-3"><select class="bs-select form-control" id="' + _docid + '">' + cbdoctype + '</select> </div>' +
            ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Code </label> </div>' +
            ' <div  class="col-md-3"><span id="' + _Bycodeid + '">' + _BCode + '</span> </div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Name </label></div>' +
            ' <div  class="col-md-3"> <span id="' + _ByNameid + '">' + _BName + '</span> </div>' +
            ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Supplier Code </label> </div>' +
            ' <div  class="col-md-3"><span id="' + _Suppcodeid + '">' + _SCode + '</span> </div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Supplier Name </label></div>' +
            ' <div  class="col-md-3"><span id="' + _SuppNameid + '">' + _SName + '</span> </div>' +
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

function fnXlsFormatDetails_Wizard(oTable, nTr, _targetclick) {
    var sOut = ''; var _str = ''; var _status = ''; var _selectopt = ''; var _grptxt = ''; var _doctxt = '';
    var indx = nTr.rowIndex; var aData = oTable.fnGetData(nTr);var tid = "MappTable" + indx; var _tbodyid = "tblBodyXLSByCfg" + indx;
    var _Bycodeid = 'txBuyercode' + indx; var _ByNameid = 'txBuyername' + indx; 
    var _mcell1id = 'txtCell1' + indx; var _mcell1val1id = 'txtCell1value1' + indx; var _mcell1val2id = 'txtCell1value2' + indx; var _mcell2id = 'txtCell2' + indx;
    var _mcell2val1id = 'txtCell2value1' + indx; var _mcellnodisid = 'txtCellnodis' + indx; var _mcellvalnodisid = 'txtCellvaluenodis' + indx;
    var _grpid = 'cbGroup' + indx; var _docid = 'cbDocType' + indx; FillXlsGroupList(); FillXlsDocTypeList();
    if (_targetclick == 'Edit') { _grptxt = Str(aData[2]); _doctxt = Str(aData[19]); } else { _grptxt = ''; _doctxt = ''; }
    var _BCode = Str(aData[3]); var _BName = Str(aData[4]); var _mapcell1 = (_targetclick == 'Edit') ? Str(aData[7]) : '';
    var _mapcell1val1 = (_targetclick == 'Edit') ? Str(aData[8]) : ''; var _mapcell1val2 = (_targetclick == 'Edit') ? Str(aData[9]) : '';
    var _mapcell2 = (_targetclick == 'Edit') ? Str(aData[10]) : ''; var _mapcell2val1 = (_targetclick == 'Edit') ? Str(aData[11]) : '';
    var _mapcellnodis = (_targetclick == 'Edit') ? Str(aData[12]) : '';   var _mapcellvalnodis = (_targetclick == 'Edit') ? Str(aData[13]) : '';
    var cbdoctype = FillCombo(_doctxt, _lstDocType);
    var _celldis = 'Cell \n (No Discount)'; var _cellvaldis = 'Cell Value \n (No Discount)'; _celldis = _celldis.replace(/\n/g, '<br/>'); _cellvaldis = _cellvaldis.replace(/\n/g, '<br/>');
    var btndiv = '<div class="row"><div style="text-align:center;"><a href="#" id="btnXLSUpdate"><u>Update</u></<a>&nbsp;<a href="#" id="btnXLSCancel"><u>Cancel</u></<a></div></div>';
    var sOut = '<div class="row" style="padding-right:63px;"><div class="col-md-10"><div class="row"><div class="col-md-10"><div class="form-group">' +            
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

function DownloadFile(EXCEL_MAP_ID, GROUPID) {
    _refrencefiles = '';
    try {
        $.ajax({
            type: "POST",
            async: false,
            url: "XLS_Buyer_Config.aspx/DownloadFormat",
            data: "{ 'EXCEL_MAP_ID':'" + EXCEL_MAP_ID + "','GROUPID':'" + GROUPID +"' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") {                     
                        var filefullpath = res.split('|')[0]; _templateFileName = res.split('|')[1];             
                        top.location.href = "../Downloads/" + _templateFileName; }                    
                    else { toastr.error("Lighthouse eSolutions Pte. Ltd", "No Mapping found"); }
                }
                catch (err) {// toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Download File :' + err);
                }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
    }
    catch (e)
    { }
};

function GetURL() {
    $.ajax({
        type: "POST",
        async: false,
        url: "XLS_Buyer_Config.aspx/GetURLdetails",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            try {
                result = response.d; alert(result);
            }
            catch (err) {; }
        },
        failure: function (response) { toastr.error("Failure in Starting Client Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); result = -1; },
        error: function (response) { toastr.error("Error in Starting Client Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); result = -1; }
    });
};

function UploadComplete(sender, args) {
    var AsyncFileUpload = sender._element.id; var txts = sender._element.getElementsByTagName("input");
    if (_fMppdet.indexOf(AsyncFileUpload) == -1) {
        _fMppdet.push(Str(sender._element.id), txts);
    }
};

function ClearControls() {    
    if (_fMppdet.length > 0) {
        for (var i = 0; i < _fMppdet.length; i++) {
            var txts = _fMppdet[i];
            for (var j = 0; j < txts.length; j++) {
                if (txts[j].type == 'file' || txts[j].type == 'hidden') {
                    txts[j].value = ''; txts[j].style.backgroundColor = "transparent";
                }
            }
        }
    }
};

//function ShowReferenceFiles(GROUP_CODE) {
//    var result = '';
//    $.ajax({
//        type: "POST",
//        async: false,
//        url: "XLS_Buyer_Config.aspx/GetReferenceFiles",
//        data: "{ 'GROUP_CODE':'" + GROUP_CODE +  "' }",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (response) {
//            try {
//                result = response.d;
//            }
//            catch (err) { toastr.error('Error in Reference Files :' + err, "Lighthouse eSolutions Pte. Ltd"); result = ''; }
//        },
//        failure: function (response) { toastr.error("Failure in Reference Files " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); },
//        error: function (response) { toastr.error("Error in Reference Files " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); }
//    });
//    return result;
//};