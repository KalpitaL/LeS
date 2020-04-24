var _mapdet = []; var _lstByrs = []; var _lstDocType = []; var selectedTr = ''; var _targetclick = ''; var _selectedrow = ''; var previousTr = '';

var PDFBuyerConfig_MapCode = function () {

    var handlePDFBuyerConfig_MapCodeTable = function () {
        var nEditing = null; var _ispdfpdate = -2; var nNew = false; $('#pageTitle').empty().append('PDF Buyer Config');
        SetupBreadcrumb('Home', 'Home.aspx', 'Mapping', '#', 'PDF Buyer Config', 'PDF_Buyer_Config_MapCode.aspx');
        $(document.getElementById('lnkMapping')).addClass('active open'); $(document.getElementById('spPDF')).addClass('title font-title SelectedColor');              
        setupTableHeader(); setFilterToolbar();
        var table = $('#dataGridPDFByCfg');
        var oPMTable = table.dataTable({
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
            "columnDefs": [{ 
                'orderable': false,"searching": true, "autoWidth": false,  'targets': [0]
            },
           { 'targets': [12, 13, 14], visible: false }, { 'targets': [0], width: '5px' }, { 'targets': [1], width: '20px' ,'bSortable':false}, { 'targets': [2], width: '60px' },
           { 'targets': [3], width: '60px' }, { 'targets': [4], width: '40px' }, { 'targets': [5], width: '120px' }, { 'targets': [6, 8,10], width: '50px' }, { 'targets': [7, 9, 11], width: '100px', "sClass": "break-det" }
             ],           
            "lengthMenu": [
                [15, 30, 50, 100, -1], [15,30, 50, 100, "All"]
            ],
            "scrollY":'300px',
            "scrollX": '1000px',
            "aaSorting": [],
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var chkid = 'chk' + nRow._DT_RowIndex;  var divid = 'dv' + nRow._DT_RowIndex; var aedit = 'rwedit' + nRow._DT_RowIndex;var adwld = 'rwdelete' + nRow._DT_RowIndex;
                var detTag = ' <div id="' + divid + '" style="text-align:center;"><span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span>' +
                  '<span id=' + adwld + ' class="actionbtn" data-toggle="tooltip" title="Download"><i class="fa fa-download"></i></span></div>';
                if (_ispdfpdate != nRow._DT_RowIndex) { $('td:eq(1)', nRow).html(detTag); }
                var chkTag = '<span style="text-align:center;padding-left:10px;"><input type="checkbox" id= "' + chkid + '" class="checkboxes widelarge"></span>'; $('td:eq(0)', nRow).html(chkTag);
            }       
        });

        $('#tblHeadRowPDFByCfg').addClass('gridHeader'); $('#ToolTables_dataGridPDFByCfg_0,#ToolTables_dataGridPDFByCfg_1').hide(); 
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");// $('#dataGridPDFByCfg').width('100%');
        GetPDFBuyerConfigGrid();

        oPMTable.on('click', 'tbody td', function (e) {
            var selectedTr = $(this).parents('tr')[0];   _selectedrow = selectedTr; var aData = oPMTable.fnGetData(selectedTr); _targetclick = '';
            if (oPMTable.fnIsOpen(selectedTr) && ((e.target.className == 'glyphicon glyphicon-pencil') || (e.target.className == 'fa fa-copy'))) {
                oPMTable.fnClose(selectedTr);
            }
            else {
                var divid = 'dv' + selectedTr._DT_RowIndex;
                if (e.target.className == 'glyphicon glyphicon-pencil') {
                    _ispdfupdate = selectedTr._DT_RowIndex;  _targetclick = e.target.innerText;
                    if ((previousTr != '') && (oPMTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {  var prevdivid = 'dv' + previousTr._DT_RowIndex; oPMTable.fnClose(previousTr); $('#' + prevdivid).show(); }
                    if (aData != null) {
                        oPMTable.fnOpen(selectedTr, fnPDFFormatDetails(oPMTable, selectedTr, 'Edit'), 'details');
                        GetPMappRowDetails(oPMTable, selectedTr, _mapdet, 'Edit'); $('#' + divid).hide(); previousTr = selectedTr;
                    }
                }
                else if (e.target.className == 'fa fa-download') { if (aData != null) { var _mapid = aData[12]; DownloadFormat(_mapid); } previousTr = selectedTr; }
                $('#btnPDFUpdate').click(function () {
                    if (aData != null) {   var _res = ValidateDetail(selectedTr.rowIndex);
                        if (_res == true) { _ispdfupdate = -2; _mapdet = [];
                            GetPMappRowDetails(oPMTable, selectedTr, _mapdet, 'Edit'); SavePDFMappingDetails(_mapdet, GetPDFBuyerConfigGrid); $('#' + divid).show();
                        }
                    }
                });
                $('#btnPDFCancel').click(function () { if (oPMTable.fnIsOpen(selectedTr)) { oPMTable.fnClose(selectedTr); } $('#' + divid).show(); _ispdfupdate = -2;});
            }
        });
        $('#btnRefresh').live('click', function (e) { e.preventDefault(); GetPDFBuyerConfigGrid(); });
        $('#btnCopy').live('click', function (e) {
            e.preventDefault(); var _slMapId = []; FillPdfBuyersList();
            $('input[type=checkbox]:checked').each(function () { var tr = $(this).closest('tr'); if (oPMTable.fnGetData(tr) != null) { var _mapid = oPMTable.fnGetData(tr)[12]; _slMapId.push(Str(_mapid)); } });
            if (_slMapId.length > 0) {    $("#ModalCopyFormat").modal('show');  }
            else { toastr.error("Lighthouse eSolutions Pte. Ltd", "Please Select Format "); }
        });
        $('#btnCopyFormat').live('click', function (e) {
            e.preventDefault(); var _slMapId = [];
            $('input[type=checkbox]:checked').each(function () { var tr = $(this).closest('tr'); if (oPMTable.fnGetData(tr) != null) { var _mapid = oPMTable.fnGetData(tr)[12]; _slMapId.push(Str(_mapid)); } });
            if (_slMapId.length > 0) {  var  _selectBYRID = $('#cbBuyers option:selected').val(); CopyPDFMapping(_selectBYRID,_slMapId, GetPDFBuyerConfigGrid); }
            $("#ModalCopyFormat").modal('hide');
        });
        $('#btnUpload').live('click', function (e) { e.preventDefault(); ClearControls();  $("#ModalUploadFormat").modal('show'); });
        $('#btnDelete').live('click', function (e) {
            e.preventDefault(); var _slMAPID = [];
            $('input[type=checkbox]:checked').each(function () { var tr = $(this).closest('tr'); if (oPMTable.fnGetData(tr) != null) { var _mapid = oPMTable.fnGetData(tr)[12]; _slMAPID.push(Str(_mapid)); } });
            if (_slMAPID.length > 0) {
                if (confirm('Are you sure ? You want to delete PDF Mapping(s) ?')) { DeletePDFMapDetails(_slMAPID, GetPDFBuyerConfigGrid); }
            }
        });
    };
 
    var setupTableHeader = function () {
        $('#toolbtngroup').empty();
        var dtfilter = '<th></th><th style="text-align:center;">#</th><th>Map Code</th><th>Doc Type</th><th>Buyer <br/> Code</th><th>Buyer Name</th><th>Map <br/> Range(1)</th><th>Map Range (1) <br/> Value</th><th>Map <br/> Range(2)</th><th>Map Range (2) <br/> Value</th><th>Map <br/> Range (3)</th><th>Map Range (3) <br/> Value</th> <th>MAP_ID</th><th>PDF_MAPID</th><th>BUYERID</th>';
        $('#tblHeadRowPDFByCfg').empty().append(dtfilter);$('#tblBodyPDFByCfg').empty();
    };

    var setFilterToolbar = function () {
        $('#toolbtngroup').empty();
        var _btns = ' <div class="row" style="margin-bottom: 2px;padding-right: 20px;"> <div class="col-md-12">' +
            ' <div id="toolbtngroup" >' +
            ' <span title="Delete" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnDelete"><i class="fa fa-trash" style="text-align:center;"></i></a></div>' +
            ' <span title="Upload" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnUpload"><i class="fa fa-upload" style="text-align:center;"></i></a></div>' +
            ' <span title="Copy" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnCopy"><i class="fa fa-copy" style="text-align:center;"></i></a></div>' +
            ' <span title="Refresh" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"> <a href="javascript:;" class="toolbtn" id="btnRefresh"><i class="fa fa-recycle" style="text-align:center;"></i></a></div></div>' +
            ' </div></div>';
        $('#toolbtngroup').append(_btns);    
    };
      
    return {
        init: function () { handlePDFBuyerConfig_MapCodeTable(); }
    };
}();

function FillPdfDocTypeList() {
    _lstDocType = [];
    $.ajax({
        type: "POST",
        async: false,
        url: "PDF_Buyer_Config_MapCode.aspx/GetPDFDoctypes",
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
                            for (var i = 0; i < Table.length; i++) {  _lstDocType.push(Str(Table[i].DOCTYPE) + "|" + Str(Table[i].DOCTYPE)); }
                        }
                        else {  if (Table.DOCTYPE != undefined) {   _lstDocType.push(Str(Table.DOCTYPE) + "|" + Str(Table.DOCTYPE));  }  }
                    }
                }
            }
            catch (err) { toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd");  }
        },
        failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
        error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
    });
};

function FillPdfBuyersList() {
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
                if (Dataset.NewDataSet != null) {
                    var Table = Dataset.NewDataSet.Table;
                    if (Table != undefined && Table != null) {
                        if (Table.length != undefined) {
                            for (var i = 0; i < Table.length; i++) {   _lstByrs.push(Str(Table[i].BUYERID) + "|" + Str(Table[i].BUYER_NAME)); }
                        }
                        else {  if (Table.DOCTYPE != undefined) {  _lstByrs.push(Str(Table.BUYERID) + "|" + Str(Table.BUYER_NAME));   }}
                    }
                }
                var _byrsdet = FillCombo('', _lstByrs); $('#cbBuyers').empty().append(_byrsdet); //$('#cbBuyers').select2();
            }
            catch (err) {  toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd");  }
        },
        failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
        error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
    });
};

function fnPDFFormatDetails(oTable, nTr, _targetclick) {
    var sOut = ''; var _str = ''; var _status = ''; var _selectopt = ''; var _mapcodetxt = ''; var _doctxt = ''; var _ipdisable = 'disabled';
    var indx = nTr.rowIndex; var aData = oTable.fnGetData(nTr); var tid = "MappTable" + indx; var _tbodyid = "tblBodyPDFByCfg" + indx;
    var _Bycodeid = 'txBuyercode' + indx; var _ByNameid = 'txBuyername' + indx;
    var _mrange1id = 'txtMapRange1' + indx; var _mrangeval1id = 'txtMapRangevalue1' + indx; var _mrange2id = 'txtMapRange2' + indx;
    var _mrangeval2id = 'txtMapRangevalue2' + indx; var _mrange3id = 'txtMapRange3' + indx; var _mrangeval3id = 'txtMapRangevalue3' + indx;
    var _mapcodeid = 'txtMapCode' + indx; var _docid = 'cbDocType' + indx; FillPdfDocTypeList();
    if (_targetclick == 'Edit') { _mapcodetxt = Str(aData[2]); _doctxt = Str(aData[3]); } else { _mapcodetxt = ''; _doctxt = ''; _ipdisable = ''; }
    var _BCode = Str(aData[4]); var _BName = Str(aData[5]); var _maprange1 = (_targetclick == 'Edit') ? Str(aData[6]) : '';
    var _maprangeval1 = (_targetclick == 'Edit') ? Str(aData[7]) : ''; var _maprange2 = (_targetclick == 'Edit') ? Str(aData[8]) : '';
    var _maprangeval2 = (_targetclick == 'Edit') ? Str(aData[9]) : ''; var _maprange3 = (_targetclick == 'Edit') ? Str(aData[10]) : '';
    var _maprangeval3 = (_targetclick == 'Edit') ? Str(aData[11]) : ''; var cbdoctype = FillCombo(_doctxt, _lstDocType);
    var btndiv = '<div class="row"><div style="text-align:center;"><a href="#" id="btnPDFUpdate"><u>Update</u></<a>&nbsp;<a href="#" id="btnPDFCancel"><u>Cancel</u></<a></div></div>';
    var sOut = '<div class="row"> <div class="col-md-10"> ' +
          ' <div class="row"><div class="col-md-10"><div class="form-group">' +
          ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Code </label> </div>' +
          ' <div  class="col-md-3"><input type="text" class="form-control" disabled id="' + _mapcodeid + '"  value="' + _mapcodetxt + '"/></div>' +
          ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Document Type</label> </div>' +
          ' <div  class="col-md-3"><select class="bs-select form-control" id="' + _docid + '">' + cbdoctype + '</select> </div>' +
          ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
          ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Code </label> </div>' +
          ' <div  class="col-md-3"><span id="' + _Bycodeid + '">' + _BCode + '</span> </div>' +
          ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Name </label></div>' +
          ' <div  class="col-md-3"> <span id="' + _ByNameid + '">' + _BName + '</span> </div>' +
          ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
          ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (1) </label> </div>' +
          ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mrange1id + '"  value="' + _maprange1 + '"/></div>' +
          ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (1) Value </label></div>' +
          ' <div  class="col-md-3"><input type="text" class="form-control"  id="' + _mrangeval1id + '" value="' + _maprangeval1 + '"/></div>' +
          ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
          ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (2) </label> </div>' +
          ' <div  class="col-md-3"> <input type="text" class="form-control"  id="' + _mrange2id + '"  value="' + _maprange2 + '"/></div>' +
          ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (2) Value </label></div>' +
          ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mrangeval2id + '" value="' + _maprangeval2 + '"/> </div>' +
          ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
          ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (3) </label> </div>' +
          ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mrange3id + '"  value="' + _maprange3 + '"/></div>' +
          ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (3) Value </label></div>' +
          ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mrangeval3id + '" value="' + _maprangeval3 + '"/> </div>' +
          ' </div></div></div>';
    sOut += btndiv + '</div></div>';
    return sOut;
}

function GetPMappRowDetails(Table, nTr, _lstdet, _targetclick) {
    var indx = nTr.rowIndex; var tid = "MappTable" + indx;
    var _maprange1 = $('#txtMapRange1' + indx).val(); var _maprangeval1 = $('#txtMapRangevalue1' + indx).val(); var _mapcode = $('#txtMapCode' + indx).val();
    var _maprange2 = $('#txtMapRange2' + indx).val(); var _maprangeval2 = $('#txtMapRangevalue2' + indx).val(); var _maprange3 = $('#txtMapRange3' + indx).val();
    var _maprangeval3 = $('#txtMapRangevalue3' + indx).val();
    var _docid = $('#cbDocType' + indx + ' option:selected').val();
    var _mapid = Table.fnGetData(nTr)[12]; var _buyid = Table.fnGetData(nTr)[14]; 
    _lstdet.push("FORMAT_MAPCODE" + "|" + Str(_mapcode)); _lstdet.push("DOC_TYPE" + "|" + Str(_docid));
    _lstdet.push("MAPPING_1" + "|" + Str(_maprange1)); _lstdet.push("MAPPING_1_VALUE" + "|" + Str(_maprangeval1));
    _lstdet.push("MAPPING_2" + "|" + Str(_maprange2)); _lstdet.push("MAPPING_2_VALUE" + "|" + Str(_maprangeval2));
    _lstdet.push("MAPPING_3" + "|" + Str(_maprange3)); _lstdet.push("MAPPING_3_VALUE" + "|" + Str(_maprangeval3)); _lstdet.push("MAPID" + "|" + Str(_mapid)); 
};

function FillPDFBuyerConfigGrid(Table) {
    try {
        $('#dataGridPDFByCfg').DataTable().clear().draw();
        if (Table.length != undefined && Table.length != null) {
            var t = $('#dataGridPDFByCfg').dataTable();
            for (var i = 0; i < Table.length; i++) {
                var cells = new Array();
                var r = jQuery('<tr id=' + i + '>');
                cells[0] = Str(''); cells[1] = Str(''); cells[2] = Str(Table[i].FORMAT_MAP_CODE);cells[3] = Str(Table[i].DOC_TYPE);
                cells[4] = Str(Table[i].BUYER_CODE);  cells[5] = Str(Table[i].BUYER_NAME);cells[6] = Str(Table[i].MAPPING_1);  cells[7] = Str(Table[i].MAPPING_1_VALUE);
                cells[8] = Str(Table[i].MAPPING_2);  cells[9] = Str(Table[i].MAPPING_2_VALUE); cells[10] = Str(Table[i].MAPPING_3);
                cells[11] = Str(Table[i].MAPPING_3_VALUE);  cells[12] = Str(Table[i].MAP_ID);cells[13] = Str(Table[i].PDF_MAPID);  cells[14] = Str(Table[i].BUYERID)               
                var ai = t.fnAddData(cells, false);
            }
            t.fnDraw();
        }
        else {
            if (Table.MAP_ID != undefined && Table.MAP_ID != null) {
                var t = $('#dataGridPDFByCfg').dataTable();
                var r = jQuery('<tr id=' + 1 + '>');
                var cells = new Array();
                cells[0] = Str(''); cells[1] = Str(''); cells[2] = Str(Table.FORMAT_MAP_CODE);  cells[3] = Str(Table.DOC_TYPE);
                cells[4] = Str(Table.BUYER_CODE); cells[5] = Str(Table.BUYER_NAME);  cells[6] = Str(Table.MAPPING_1); cells[7] = Str(Table.MAPPING_1_VALUE);
                cells[8] = Str(Table.MAPPING_2); cells[9] = Str(Table.MAPPING_2_VALUE); cells[10] = Str(Table.MAPPING_3); cells[11] = Str(Table.MAPPING_3_VALUE);
                cells[12] = Str(Table.MAP_ID); cells[13] = Str(Table.PDF_MAPID); cells[14] = Str(Table.BUYERID)
                var ai = t.fnAddData(cells, false);
                t.fnDraw();
            }
        }
    }
    catch (e)
    { }
};

var GetPDFBuyerConfigGrid = function () {
    Metronic.blockUI('#portlet_body');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "PDF_Buyer_Config_MapCode.aspx/GetPDFBuyerConfigGrid",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table; FillPDFBuyerConfigGrid(Table); }
                    else $('#dataGridPDFByCfg').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) {  Metronic.unblockUI();  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get PDF BuyerConfig :' + err); }
            },
            failure: function (response) {  toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) {   toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    }, 200);
};

function ValidateDetail(indx) {
    var _valid = true;
    var _docid = $('#cbDocType' + indx + ' option:selected').val();    
    if (_docid == '') { $($('#cbDocType' + indx).select2('container')).addClass('error'); _valid = false; } else { $($('#cbDocType' + indx).select2('container')).removeClass('error'); }
    return _valid;
};

function SavePDFMappingDetails(_nfieldval, callback) {
    var slPDFMapdet = [];
    for (var j = 0; j < _nfieldval.length; j++) { slPDFMapdet.push(_nfieldval[j]); }
    var data2send = JSON.stringify({ slPDFMapdet: slPDFMapdet });
    Metronic.blockUI('#portlet_body');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "PDF_Buyer_Config_MapCode.aspx/SavePDFMapping",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "PDF Mapping Details Saved successfully."); callback(); }
                    Metronic.unblockUI();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update PDF Mapping details :' + err); Metronic.unblockUI(); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) {  toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    }, 20);
};

function DeletePDFMapDetails(_nfieldval, callback) {
    var slMAPID = [];
    for (var j = 0; j < _nfieldval.length; j++) { slMAPID.push(_nfieldval[j]); }
    var data2send = JSON.stringify({ slMAPID: slMAPID });
    Metronic.blockUI('#portlet_body');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "PDF_Buyer_Config_MapCode.aspx/DeletePDFMapping",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "PDF Mapping Deleted."); callback(); }
                    Metronic.unblockUI();
                }
                catch (err) {   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete PDF Mapping :' + err);  }
            },
            failure: function (response) {toastr.error("failure get", response.d); Metronic.unblockUI();},
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI();}
        });
    }, 20);
};

function CopyPDFMapping(BUYERID,_nfieldval, callback) {
    var slPDFMapdet = [];
    for (var j = 0; j < _nfieldval.length; j++) { slPDFMapdet.push(_nfieldval[j]); }
    var data2send = JSON.stringify({BUYERID:BUYERID, slPDFMapdet: slPDFMapdet });
    Metronic.blockUI('#portlet_body');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "PDF_Buyer_Config_MapCode.aspx/CopyPDFMapping",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "PDF Mapping Details Copied successfully."); callback(); }
                    Metronic.unblockUI();
                }
                catch (err) {  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to copy PDF Mapping details :' + err); Metronic.unblockUI();  }
            },
            failure: function (response) {   toastr.error("failure get", response.d); Metronic.unblockUI();},
            error: function (response) {  toastr.error("error get", response.responseText); Metronic.unblockUI();}
        });
    }, 20);
};

function GetMappingPDFGrid(ADDRESSID, ADDRTYPE) {
    var DataSet = '';
    $.ajax({
        type: "POST",
        async: false,
        url: "PDF_Buyer_Config_MapCode.aspx/GetPDFBuyerConfig_Addressid",
        data: "{'ADDRESSID':'" + ADDRESSID + "','ADDRTYPE':'"+ADDRTYPE+"'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            try { DataSet = JSON.parse(response.d); }             
            catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get PDF BuyerConfig data :' + err);}
        },
        failure: function (response) { toastr.error("failure get", response.d);},
        error: function (response) { toastr.error("error get", response.responseText); }
    });
    return DataSet;
};

function DownloadFormat(MAPID) {
    try {
        $.ajax({
            type: "POST",
            async: false,
            url: "PDF_Buyer_Config_MapCode.aspx/DownloadPDFFormat",
            data: "{ 'MAPID':'" + MAPID + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") {
                        var filefullpath = res.split('|')[0]; var filename = res.split('|')[1]; top.location.href = "../Downloads/" + filename;
                        toastr.success("Lighthouse eSolutions Pte. Ltd", "Download File success");
                    }
                    else { toastr.error("Lighthouse eSolutions Pte. Ltd", "No Mapping found"); }
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Download File :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
    }
    catch (e)
    { }
};

function onClientUploadStart(sender, e) {
    var FILENAME = e._fileName;
    $.ajax({
        type: "POST",
        async: false,
        url: "PDF_Buyer_Config_MapCode.aspx/SetUploadPath",
        data: "{'FILENAME':'" + FILENAME + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            try {
                result = response.d;
            }
            catch (err) { toastr.error('Error in Starting Client Upload :' + err, "Lighthouse eSolutions Pte. Ltd");   }
        },
        failure: function (response) {   toastr.error("Failure in Starting Client Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); result = -1;  },
        error: function (response) {  toastr.error("Error in Starting Client Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); result = -1;  }
    });
}

function ClearControls() {
    jQuery('.ajax__fileupload_fileItemInfo').remove(); $('.ajax__fileupload_topFileStatus').text('Please select file(s) to upload');  $('.ajax__fileupload_queueContainer').fadeOut(); 
};

function onClientUploadComplete(sender, e) {
    var file = e._filename;
    $.ajax({
        type: "POST",
        async: false,
        url: "PDF_Buyer_Config_MapCode.aspx/UploadFileMapping",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            try {
                result = response.d;
                if (Str(result) == "") {
                    jQuery('.ajax__fileupload_fileItemInfo').remove(); $('.ajax__fileupload_topFileStatus').text('Please select file(s) to upload');
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