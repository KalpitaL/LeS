var _mapdet = []; var _lstGrp = []; var _lstDocType = []; var selectedTr = ''; var _targetclick = ''; var _selectedrow = ''; var previousTr = '';
var _siteurl = ''; var _refrencefiles = ''; var _fMppdet = [];

var PDFBuyerConfig = function () {

    var handlePDFBuyerConfigTable = function () {
        var nEditing = null; var _ispdfpdate = -2; var nNew = false; $('#pageTitle').empty().append('PDF Buyer-Supp Config');
        SetupBreadcrumb('Home', 'Home.aspx', 'Mapping', '#', 'PDF Buyer-Supp Config', 'PDF_Buyer_Config.aspx');
        $(document.getElementById('lnkMapping')).addClass('active open'); $(document.getElementById('spPDF')).addClass('title font-title SelectedColor');              
        setupTableHeader();
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
            "columnDefs": [{  'orderable': false,"searching": true, "autoWidth": false,  'targets': [0] },
           {'targets': [14, 15,16,17,18], visible: false },         
           { 'targets': [0], width: '85px' }, { 'targets': [1], width: '35px' }, { 'targets': [2], width: '100px', "sClass": "break-det" },
           { 'targets': [3], width: '75px' }, { 'targets': [4,6], width: '45px' }, { 'targets': [9, 11, 13], width: '90px' },
           { 'targets': [5,7,19], width: '150px', "sClass": "break-det" }, { 'targets': [8,10, 12], width: '55px', "sClass": "break-det" },
           { 'targets': [2, 4, 6, 9, 10, 11, 12], "sClass": "visible-lg" },
             ],           
            "lengthMenu": [ [25, 50, 100, -1], [25, 50, 100, "All"] ],
            "scrollY": "300px",
            "scrollX": true,
            "aaSorting": [],
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var detTag = '';
                var divid = 'dv' + nRow._DT_RowIndex; var aedit = 'rwedit' + nRow._DT_RowIndex; var adelete = 'rwdelete' + nRow._DT_RowIndex; var acopy = 'rwcopy' + nRow._DT_RowIndex;
                var aupload = 'rwupload' + nRow._DT_RowIndex; var adwnld = 'rwdownload' + nRow._DT_RowIndex;
                 detTag = ' <div id="' + divid + '" style="text-align:center;"><span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span>' +
                  '<span id=' + adelete + ' class="actionbtn" data-toggle="tooltip" title="Delete"><i class="glyphicon glyphicon-trash"></i></span>'+
                  ' <span id=' + acopy + ' class="actionbtn" data-toggle="tooltip" title="Copy"><i class="fa fa-copy"></i></span>'+
                  ' <span id=' + adwnld + ' class="actionbtn" data-toggle="tooltip" title="Download"><i class="fa fa-download"></i></span>' +
                  ' <span id=' + aupload + ' class="actionbtn" data-toggle="tooltip" title="Upload"><i class="fa fa-upload"></i></span></div>';
                 if (_ispdfpdate != nRow._DT_RowIndex) { $('td:eq(0)', nRow).html(detTag); }
                 var _res = (aData[19]); if (_res != '' && _res != undefined) { var fileTag = '<a href="javascript:;" name="Ref">' + _res + '</<a>'; $('td:eq(14)', nRow).html(fileTag);}
            }       
        });

        $('#tblHeadRowPDFByCfg').addClass('gridHeader'); $('#ToolTables_dataGridPDFByCfg_0,#ToolTables_dataGridPDFByCfg_1').hide();
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
        GetPDFBuyerConfigGrid();

        oPMTable.on('click', 'tbody td', function (e) {
            var selectedTr = $(this).parents('tr')[0];   _selectedrow = selectedTr; var aData = oPMTable.fnGetData(selectedTr); _targetclick = '';
            if (oPMTable.fnIsOpen(selectedTr) && ((e.target.className == 'glyphicon glyphicon-pencil') || (e.target.innerText == 'fa fa-copy'))) {
                oPMTable.fnClose(selectedTr);
            }
            else {
                var divid = 'dv' + selectedTr._DT_RowIndex;
                if (e.target.className == 'glyphicon glyphicon-pencil') {
                    _ispdfupdate = selectedTr._DT_RowIndex;  _targetclick = e.target.innerText;
                    if ((previousTr != '') && (oPMTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {
                        var prevdivid = 'dv' + previousTr._DT_RowIndex; oPMTable.fnClose(previousTr); $('#' + prevdivid).show();
                    }
                    if (aData != null) {
                        oPMTable.fnOpen(selectedTr, fnPDFFormatDetails(oPMTable, selectedTr, 'Edit'), 'details');  $('#' + divid).hide(); previousTr = selectedTr;
                    }
                }
                else if (e.target.className == 'glyphicon glyphicon-trash') {
                    if (confirm('Are you sure ? You want to delete PDF Mapping ?')) {                        
                        var _mapid = oPMTable.fnGetData(selectedTr)[14]; DeletePDFMapDetails(_mapid, GetPDFBuyerConfigGrid);  }
                    previousTr = selectedTr;
                }
                else if (e.target.className == 'fa fa-copy') {
                    _mapdet = [];                    
                    if ((previousTr != '') && (oPMTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {
                        var prevdivid = 'dv' + previousTr._DT_RowIndex; oPMTable.fnClose(previousTr); $('#' + prevdivid).show();
                    }
                    if (aData != null) { oPMTable.fnOpen(selectedTr, fnPDFFormatDetails(oPMTable, selectedTr, 'Copy'), 'details'); $('#' + divid).hide(); }
                    previousTr = selectedTr;
                }
                else if (e.target.className == 'fa fa-download') {
                    _mapdet = [];
                    if ((previousTr != '') && (oPMTable.fnIsOpen(previousTr) && previousTr != selectedTr)) { var prevdivid = 'dv' + previousTr._DT_RowIndex; oXMTable.fnClose(previousTr); $('#' + prevdivid).show(); }
                    if (aData != null) { var _groupid = Str(aData[18]); DownloadFile(_groupid); $('#' + divid).Show(); } previousTr = selectedTr;
                }
                else if (e.target.className == 'fa fa-upload') {
                    _mapdet = [];
                    if ((previousTr != '') && (oPMTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {
                        var prevdivid = 'dv' + previousTr._DT_RowIndex; oPMTable.fnClose(previousTr); $('#' + prevdivid).show();
                    }
                    if (aData != null) { $("#ModalUpload").modal('show'); $('#' + divid).show(); }
                    previousTr = selectedTr;
                }
                else if (e.target.name == 'Ref') {
                    if (aData != null) {
                        var _filename = Str(aData[19]); var _groupcode = Str(aData[2]); top.location.href = "../ReferenceFiles/PDF/" + _groupcode + "/" + _filename;
                    }
                }
                $('#btnPDFUpdate').click(function () {
                    if (aData != null) {
                        var _res = ValidateDetail(selectedTr.rowIndex);
                        if (_res == true) { _ispdfupdate = -2; _mapdet = [];
                            GetPMappRowDetails(oPMTable, selectedTr, _mapdet, e.target.className); UpdatePMapdet(_mapdet, GetPDFBuyerConfigGrid, e.target.className);
                            $('#' + divid).show(); 
                        }
                    }
                });
                $('#btnPDFCancel').click(function () {
                    if (oPMTable.fnIsOpen(selectedTr)) { oPMTable.fnClose(selectedTr); } $('#' + divid).show(); _ispdfupdate = -2;
                });
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
        var dtfilter = '<th style="text-align:center;">#</th><th>LinkId</th><th>Group</th><th>Document <br/>Type</th><th>Buyer <br/>Code</th><th>Buyer Name</th><th>Supplier<br/> Code</th>' +
            ' <th>Supplier Name</th> <th>Map <br\> Range (1)</th><th>Map Range (1) <br\>Value</th><th>Map <br\>Range (2)</th><th>Map Range(2) <br\>Value</th><th>Map <br\>Range (3)</th><th>Map Range (3) <br\>Value</th> ' +
            ' <th>MAP_ID</th><th>PDF_MAPID</th><th>BUYERID</th><th>SUPPLIERID</th> <th>GROUP_ID</th><th>Sample File</th>';
        $('#tblHeadRowPDFByCfg').empty().append(dtfilter);$('#tblBodyPDFByCfg').empty();
    };

    function UploadFiles() {
        $.ajax({
            type: "POST",
            async: false,
            url: "PDF_Buyer_Config.aspx/UploadFileMapping",
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
        init: function () { handlePDFBuyerConfigTable(); }
    };
}();

function FillPdfGroupList() {
    $.ajax({
        type: "POST",
        async: false,
        url: "PDF_Buyer_Config.aspx/GetPDFGroupsOnly",
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
                            for (var i = 0; i < Table.length; i++) { _lstGrp.push(Str(Table[i].GROUP_ID) + "|" + Str(Table[i].GROUP_CODE));}
                        }
                        else {    if (Table.GROUP_ID != undefined) {  _lstGrp.push(Str(Table.GROUP_ID) + "|" + Str(Table.GROUP_CODE));}  }
                    }
                }
            }
            catch (err) {   toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd"); }
        },
        failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
        error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
    });
};

function FillPdfDocTypeList() {
    _lstDocType = [];
    $.ajax({
        type: "POST",
        async: false,
        url: "PDF_Buyer_Config.aspx/GetPDFDoctypes",
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
                            for (var i = 0; i < Table.length; i++) {   _lstDocType.push(Str(Table[i].DOCTYPE) + "|" + Str(Table[i].DOCTYPE));}
                        }
                        else {  if (Table.DOCTYPE != undefined) {  _lstDocType.push(Str(Table.DOCTYPE) + "|" + Str(Table.DOCTYPE)); } }
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

function GetPMappRowDetails(Table, nTr, _lstdet, _targetclick) {
    var indx = nTr.rowIndex; var tid = "MappTable" + indx;
    var _maprange1 = $('#txtMapRange1' + indx).val(); var _maprangeval1 = $('#txtMapRangevalue1' + indx).val();
    var _maprange2 = $('#txtMapRange2' + indx).val(); var _maprangeval2 = $('#txtMapRangevalue2' + indx).val();
    var _maprange3 = $('#txtMapRange3' + indx).val(); var _maprangeval3 = $('#txtMapRangevalue3' + indx).val();
    var _grpid = $('#cbGroup' + indx + ' option:selected').val(); if (_grpid == '' || _grpid == undefined) { _grpid = ''; } var _docid = $('#cbDocType' + indx + ' option:selected').val();
    var _mapid = Table.fnGetData(nTr)[14]; var _buyid = Table.fnGetData(nTr)[16]; var _suppid = Table.fnGetData(nTr)[17];
    _lstdet.push("GROUP_ID" + "|" + Str(_grpid)); _lstdet.push("DOC_TYPE" + "|" + Str(_docid));
    _lstdet.push("MAPPING_1" + "|" + Str(_maprange1)); _lstdet.push("MAPPING_1_VALUE" + "|" + Str(_maprangeval1));
    _lstdet.push("MAPPING_2" + "|" + Str(_maprange2)); _lstdet.push("MAPPING_2_VALUE" + "|" + Str(_maprangeval2));
    _lstdet.push("MAPPING_3" + "|" + Str(_maprange3)); _lstdet.push("MAPPING_3_VALUE" + "|" + Str(_maprangeval3));
    if (_targetclick == 'fa fa-copy') { _lstdet.push("COPY_BUYERID" + "|" + Str(_buyid)); _lstdet.push("COPY_SUPPLIERID" + "|" + Str(_suppid)); } else { _lstdet.push("MAPID" + "|" + Str(_mapid)); }
};

function FillPDFBuyerConfigGrid(Table) {
    try {
        $('#dataGridPDFByCfg').DataTable().clear().draw();
        if (Table.length != undefined && Table.length != null) {
            var t = $('#dataGridPDFByCfg').dataTable();
            for (var i = 0; i < Table.length; i++) {
                var cells = new Array();
                var r = jQuery('<tr id=' + i + '>');
                cells[0] = Str(''); cells[1] = Str(Table[i].BUYER_SUPP_LINKID); cells[2] = Str(Table[i].GROUP_CODE); cells[3] = Str(Table[i].DOC_TYPE);
                cells[4] = Str(Table[i].BUYER_CODE); cells[5] = Str(Table[i].BUYER_NAME); cells[6] = Str(Table[i].SUPPLIER_CODE); cells[7] = Str(Table[i].SUPPLIER_NAME);
                cells[8] = Str(Table[i].MAPPING_1); cells[9] = Str(Table[i].MAPPING_1_VALUE); cells[10] = Str(Table[i].MAPPING_2); cells[11] = Str(Table[i].MAPPING_2_VALUE);
                cells[12] = Str(Table[i].MAPPING_3); cells[13] = Str(Table[i].MAPPING_3_VALUE); cells[14] = Str(Table[i].MAP_ID); cells[15] = Str(Table[i].PDF_MAPID);
                cells[16] = Str(Table[i].BUYERID); cells[17] = Str(Table[i].SUPPLIERID); cells[18] = Str(Table[i].GROUP_ID); cells[19] = Str(Table[i].SampleFile);
                var ai = t.fnAddData(cells, false);
            }
            t.fnDraw();
        }
        else {
            if (Table.MAP_ID != undefined && Table.MAP_ID != null) {
                var t = $('#dataGridPDFByCfg').dataTable();
                var r = jQuery('<tr id=' + 1 + '>');
                var cells = new Array();
                cells[0] = Str(''); cells[1] = Str(Table.BUYER_SUPP_LINKID); cells[2] = Str(Table.GROUP_CODE); cells[3] = Str(Table.DOC_TYPE);
                cells[4] = Str(Table.BUYER_CODE); cells[5] = Str(Table.BUYER_NAME); cells[6] = Str(Table.SUPPLIER_CODE); cells[7] = Str(Table.SUPPLIER_NAME);
                cells[8] = Str(Table.MAPPING_1); cells[9] = Str(Table.MAPPING_1_VALUE); cells[10] = Str(Table.MAPPING_2); cells[11] = Str(Table.MAPPING_2_VALUE);
                cells[12] = Str(Table.MAPPING_3); cells[13] = Str(Table.MAPPING_3_VALUE); cells[14] = Str(Table.MAP_ID); cells[15] = Str(Table.PDF_MAPID);
                cells[16] = Str(Table.BUYERID); cells[17] = Str(Table.SUPPLIERID); cells[18] = Str(Table.GROUP_ID); cells[19] = Str(Table.SampleFile);
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
            url: "PDF_Buyer_Config.aspx/GetPDFBuyerConfigGrid",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table;  FillPDFBuyerConfigGrid(Table); }
                    else $('#dataGridPDFByCfg').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) {   Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get PDF BuyerConfig :' + err);}
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI();},
            error: function (response) {  toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    }, 180);
};

function ValidateDetail(indx) {
    var _valid = true;
    var _grpid = $('#cbGroup' + indx + ' option:selected').val();  var _docid = $('#cbDocType' + indx + ' option:selected').val();
    if (_grpid == '') { $($('#cbGroup' + indx).select2('container')).addClass('error'); _valid = false; } else { $($('#cbGroup' + indx).select2('container')).removeClass('error'); }
    if (_docid == '') { $($('#cbDocType' + indx).select2('container')).addClass('error'); _valid = false; } else { $($('#cbDocType' + indx).select2('container')).removeClass('error'); }
    return _valid;
};

function UpdatePMapdet(_nfieldval, callback, targetClick) {
    if (targetClick == 'glyphicon glyphicon-pencil') {SavePDFMappingDetails(_nfieldval, callback); }
    else if (targetClick == 'fa fa-copy') {  CopyPDFMappingDetails(_nfieldval, callback);}
};

function SavePDFMappingDetails(_nfieldval, callback) {
    var slPDFMapdet = [];
    for (var j = 0; j < _nfieldval.length; j++) { slPDFMapdet.push(_nfieldval[j]);}
    var data2send = JSON.stringify({ slPDFMapdet: slPDFMapdet });
    Metronic.blockUI('#portlet_body');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "PDF_Buyer_Config.aspx/UpdatePDFMapping",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "PDF Mapping Details Saved successfully."); callback();  }
                    Metronic.unblockUI();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update PDF Mapping details :' + err); Metronic.unblockUI(); }
            },
            failure: function (response) {  toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) {  toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    }, 20);
};

function DeletePDFMapDetails(MAPID, callback) {
    Metronic.blockUI('#portlet_body');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "PDF_Buyer_Config.aspx/DeletePDFMapping",
            data: "{ 'MAPID':'" + MAPID + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") {   toastr.success("Lighthouse eSolutions Pte. Ltd", "PDF Mapping Deleted.");GetPDFBuyerConfigGrid(); }
                    Metronic.unblockUI();
                }
                catch (err) {toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete PDF Mapping :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    }, 20);
};

function CopyPDFMappingDetails(_nfieldval, callback) {
    var slPDFMapdet = [];
    for (var j = 0; j < _nfieldval.length; j++) { slPDFMapdet.push(_nfieldval[j]);  }
    var data2send = JSON.stringify({ slPDFMapdet: slPDFMapdet });
    Metronic.blockUI('#portlet_body');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "PDF_Buyer_Config.aspx/CopyPDFMapping",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") {  toastr.success("Lighthouse eSolutions Pte. Ltd", "PDF Mapping Details Copied successfully.");  GetPDFBuyerConfigGrid();  }
                    Metronic.unblockUI();
                }
                catch (err) {    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to copy PDF Mapping details :' + err); Metronic.unblockUI(); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI();},
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    }, 20);
};

function fnPDFFormatDetails(oTable, nTr, _targetclick) {
    var sOut = ''; var _str = ''; var _status = ''; var _selectopt = ''; var _grptxt = ''; var _doctxt = ''; var _ipdisable = 'disabled';
    var indx = nTr.rowIndex; var aData = oTable.fnGetData(nTr); var tid = "MappTable" + indx; var _tbodyid = "tblBodyPDFByCfg" + indx;
    var _Bycodeid = 'txBuyercode' + indx; var _ByNameid = 'txBuyername' + indx; var _Suppcodeid = 'txSuppcode' + indx; var _SuppNameid = 'txSuppname' + indx;
    var _mrange1id = 'txtMapRange1' + indx; var _mrangeval1id = 'txtMapRangevalue1' + indx; var _mrange2id = 'txtMapRange2' + indx;
    var _mrangeval2id = 'txtMapRangevalue2' + indx; var _mrange3id = 'txtMapRange3' + indx; var _mrangeval3id = 'txtMapRangevalue3' + indx;
    var _grpid = 'cbGroup' + indx; var _docid = 'cbDocType' + indx; FillPdfGroupList(); FillPdfDocTypeList();
    if (_targetclick == 'Edit') { _grptxt = Str(aData[2]); _doctxt = Str(aData[3]); } else { _grptxt = ''; _doctxt = ''; _ipdisable = ''; }
    var _BCode = Str(aData[4]); var _BName = Str(aData[5]); var _SCode = Str(aData[6]); var _SName = Str(aData[7]); var _maprange1 = (_targetclick == 'Edit') ? Str(aData[8]) : '';
    var _maprangeval1 = (_targetclick == 'Edit') ? Str(aData[9]) : ''; var _maprange2 = (_targetclick == 'Edit') ? Str(aData[10]) : '';
    var _maprangeval2 = (_targetclick == 'Edit') ? Str(aData[11]) : '';    var _maprange3 = (_targetclick == 'Edit') ? Str(aData[12]) : '';
    var _maprangeval3 = (_targetclick == 'Edit') ? Str(aData[13]) : '';var cbgrp = FillCombo(_grptxt, _lstGrp); var cbdoctype = FillCombo(_doctxt, _lstDocType);
    var btndiv = '<div class="row"><div style="text-align:center;"><a href="#" id="btnPDFUpdate"><u>Update</u></<a>&nbsp;<a href="#" id="btnPDFCancel"><u>Cancel</u></<a></div></div>';
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
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (1) </label> </div>' +
            ' <div  class="col-md-3"><input type="text" class="form-control" ' + _ipdisable + ' id="' + _mrange1id + '"  value="' + _maprange1 + '"/></div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (1) Value </label></div>' +
            ' <div  class="col-md-3"><input type="text" class="form-control" ' + _ipdisable + '  id="' + _mrangeval1id + '" value="' + _maprangeval1 + '"/></div>' +
            ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (2) </label> </div>' +
            ' <div  class="col-md-3"> <input type="text" class="form-control" ' + _ipdisable + '  id="' + _mrange2id + '"  value="' + _maprange2 + '"/></div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (2) Value </label></div>' +
            ' <div  class="col-md-3"><input type="text" class="form-control" ' + _ipdisable + ' id="' + _mrangeval2id + '" value="' + _maprangeval2 + '"/> </div>' +
            ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (3) </label> </div>' +
            ' <div  class="col-md-3"><input type="text" class="form-control" ' + _ipdisable + ' id="' + _mrange3id + '"  value="' + _maprange3 + '"/></div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (3) Value </label></div>' +
            ' <div  class="col-md-3"><input type="text" class="form-control" ' + _ipdisable + ' id="' + _mrangeval3id + '" value="' + _maprangeval3 + '"/> </div>' +
            ' </div></div></div>' + btndiv + '</div></div>';
    return sOut;
}

function fnPDFFormatDetails_Wizard(oTable, nTr, _targetclick) {
    var sOut = ''; var _str = ''; var _status = ''; var _selectopt = ''; var _grptxt = ''; var _doctxt = ''; var _ipdisable = 'disabled';
    var indx = nTr.rowIndex; var aData = oTable.fnGetData(nTr); var tid = "MappTable" + indx; var _tbodyid = "tblBodyPDFByCfg" + indx;
    var _Bycodeid = 'txBuyercode' + indx; var _ByNameid = 'txBuyername' + indx; 
    var _mrange1id = 'txtMapRange1' + indx; var _mrangeval1id = 'txtMapRangevalue1' + indx; var _mrange2id = 'txtMapRange2' + indx;
    var _mrangeval2id = 'txtMapRangevalue2' + indx; var _mrange3id = 'txtMapRange3' + indx; var _mrangeval3id = 'txtMapRangevalue3' + indx;
    var _docid = 'cbDocType' + indx; FillPdfGroupList(); FillPdfDocTypeList();
    if (_targetclick == 'Edit') { _grptxt = Str(aData[2]); _doctxt = Str(aData[3]); } else { _grptxt = ''; _doctxt = ''; _ipdisable = ''; }
    var _BCode = Str(aData[4]); var _BName = Str(aData[5]); var _SCode = Str(aData[6]); var _SName = Str(aData[7]); var _maprange1 = (_targetclick == 'Edit') ? Str(aData[8]) : '';
    var _maprangeval1 = (_targetclick == 'Edit') ? Str(aData[9]) : ''; var _maprange2 = (_targetclick == 'Edit') ? Str(aData[10]) : '';
    var _maprangeval2 = (_targetclick == 'Edit') ? Str(aData[11]) : ''; var _maprange3 = (_targetclick == 'Edit') ? Str(aData[12]) : '';
    var _maprangeval3 = (_targetclick == 'Edit') ? Str(aData[13]) : '';var cbdoctype = FillCombo(_doctxt, _lstDocType);
    var btndiv = '<div class="row"><div style="text-align:center;"><a href="#" id="btnPDFUpdate"><u>Update</u></<a>&nbsp;<a href="#" id="btnPDFCancel"><u>Cancel</u></<a></div></div>';
    var sOut = '<div class="row" style="padding-right:63px;"><div class="col-md-10"><div class="row"><div class="col-md-10"><div class="form-group">' +        
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel">Document Type</label> </div>' +
            ' <div  class="col-md-3"><select class="bs-select form-control" id="' + _docid + '">' + cbdoctype + '</select> </div>' +
            ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Code </label> </div>' +
            ' <div  class="col-md-3"><span id="' + _Bycodeid + '">' + _BCode + '</span> </div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Name </label></div>' +
            ' <div  class="col-md-3"> <span id="' + _ByNameid + '">' + _BName + '</span> </div>' +          
            ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (1) </label> </div>' +
            ' <div  class="col-md-3"><input type="text" class="form-control" ' + _ipdisable + ' id="' + _mrange1id + '"  value="' + _maprange1 + '"/></div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (1) Value </label></div>' +
            ' <div  class="col-md-3"><input type="text" class="form-control" ' + _ipdisable + '  id="' + _mrangeval1id + '" value="' + _maprangeval1 + '"/></div>' +
            ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (2) </label> </div>' +
            ' <div  class="col-md-3"> <input type="text" class="form-control" ' + _ipdisable + '  id="' + _mrange2id + '"  value="' + _maprange2 + '"/></div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (2) Value </label></div>' +
            ' <div  class="col-md-3"><input type="text" class="form-control" ' + _ipdisable + ' id="' + _mrangeval2id + '" value="' + _maprangeval2 + '"/> </div>' +
            ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (3) </label> </div>' +
            ' <div  class="col-md-3"><input type="text" class="form-control" ' + _ipdisable + ' id="' + _mrange3id + '"  value="' + _maprange3 + '"/></div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (3) Value </label></div>' +
            ' <div  class="col-md-3"><input type="text" class="form-control" ' + _ipdisable + ' id="' + _mrangeval3id + '" value="' + _maprangeval3 + '"/> </div>' +
            ' </div></div></div>' + btndiv + '</div></div>';
    return sOut;
}

function GetMappingPDFGrid(ADDRESSID, ADDRTYPE) {
    var DataSet = '';
    $.ajax({
        type: "POST",
        async: false,
        url: "PDF_Buyer_Config.aspx/GetPDFBuyerConfig_Addressid",
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

function DownloadFile( GROUPID) {
    _refrencefiles = '';
    try {
        $.ajax({
            type: "POST",
            async: false,
            url: "PDF_Buyer_Config.aspx/DownloadFormat",
            data: "{ 'GROUPID':'" + GROUPID + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") {
                        var filefullpath = res.split('|')[0]; _templateFileName = res.split('|')[1];
                        top.location.href = "../Downloads/" + _templateFileName;
                    }
                    else { toastr.error("Lighthouse eSolutions Pte. Ltd", "No Mapping found"); }
                }
                catch (err) { }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
    }
    catch (e)
    { }
};

function DownloadReferenceFile(FILENAME) {
    _refrencefiles = '';
    try {
        $.ajax({
            type: "POST",
            async: false,
            url: "PDF_Buyer_Config.aspx/DownloadFormat",
            data: "{ 'FILENAME':'" + FILENAME + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") {
                        var filefullpath = res.split('|')[0]; _templateFileName = res.split('|')[1];
                        top.location.href = "../ReferenceFiles/" + _templateFileName;
                    }
                    else { toastr.error("Lighthouse eSolutions Pte. Ltd", "No Mapping found"); }
                }
                catch (err) { }
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
        url: "PDF_Buyer_Config.aspx/GetURLdetails",
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

function ShowReferenceFiles(GROUP_CODE) {
    var result = '';
    $.ajax({
        type: "POST",
        async: false,
        url: "PDF_Buyer_Config.aspx/GetReferenceFiles",
        data: "{ 'GROUP_CODE':'" + GROUP_CODE + "' }",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            try {
                result = response.d;
            }
            catch (err) { toastr.error('Error in Reference Files :' + err, "Lighthouse eSolutions Pte. Ltd"); result = ''; }
        },
        failure: function (response) { toastr.error("Failure in Reference Files " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); },
        error: function (response) { toastr.error("Error in Reference Files " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); }
    });
    return result;
};