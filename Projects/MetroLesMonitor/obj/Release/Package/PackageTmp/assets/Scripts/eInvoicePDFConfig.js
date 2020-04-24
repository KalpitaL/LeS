var _mapdet = []; var _lstInvdet = []; var _lstSpp = []; var _lstMapCode = []; var selectedTr = ''; var _targetclick = ''; var _selectedrow = ''; var previousTr = '';
var _siteurl = ''; var _refrencefiles = ''; var _fMppdet = [];

var eInvoicePDFConfig = function () {

    var handleeInvoicePDFConfigTable = function () {
        var nEditing = null; var _ispdfpdate = -2; var nNew = false; $('#pageTitle').empty().append('eInvoice PDF Config');
        SetupBreadcrumb('Home', 'Home.aspx', 'eInvoice', '#', 'eInvoice PDF Config', 'eInvoicePDFConfig.aspx');
        $(document.getElementById('lnkeInvoice')).addClass('active open'); $(document.getElementById('speInvPDFCnfg')).addClass('title font-title SelectedColor');
        setupTableHeader(); setFilterToolbar();

        var table = $('#dataGrideInvPDFCfg');
        var oeInvCnfgTable = table.dataTable({
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
            tableTools: {  "sRowSelect": "single", "aButtons": ["select_all", "select_none"]},
            "columnDefs": [{  'orderable': false,"searching": true, "autoWidth": false,  'targets': [0]
            },
           { 'targets': [10,11,12], visible: false },{ 'targets': [0], width: '65px' ,'bSortable':false}, { 'targets': [1], width: '70px' },{ 'targets': [2], width: '50px' },
           { 'targets': [3], width: '120px' }, { 'targets': [4,6, 8], width: '50px' }, { 'targets': [5,7, 9,13], width: '100px', "sClass": "break-det" }
            ],
            "lengthMenu": [  [15, 30, 50, 100, -1], [15,30, 50, 100, "All"]],
            "scrollY":'300px',
            "scrollX": true,
            "aaSorting": [],
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var divid = 'dv' + nRow._DT_RowIndex; var aedit = 'rwedit' + nRow._DT_RowIndex; var adeleteld = 'rwdelete' + nRow._DT_RowIndex; var aupload = 'rwupload' + nRow._DT_RowIndex; var adwnld = 'rwdownload' + nRow._DT_RowIndex;
                var detTag = ' <div id="' + divid + '" style="text-align:center;"><span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span>' +
                  '<span id=' + adeleteld + ' class="actionbtn" data-toggle="tooltip" title="Delete"><i class="glyphicon glyphicon-trash"></i></span>'+
                  ' <span id=' + adwnld + ' class="actionbtn" data-toggle="tooltip" title="Download"><i class="fa fa-download"></i></span>' +
                 ' <span id=' + aupload + ' class="actionbtn" data-toggle="tooltip" title="Upload"><i class="fa fa-upload"></i></span></div>';
                if (_ispdfpdate != nRow._DT_RowIndex) { $('td:eq(0)', nRow).html(detTag); }
                var _res = (aData[13]); if (_res != '' && _res != undefined) { var fileTag = '<a href="javascript:;" name="Ref">' + _res + '</<a>'; $('td:eq(12)', nRow).html(fileTag);}
            }
        });

        $('#tblHeadRoweInvPDFCfg').addClass('gridHeader'); $('#ToolTables_dataGrideInvPDFCfg_0,#ToolTables_dataGrideInvPDFCfg_1').hide();
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
        GeteInvoicePDFConfigGrid(); 

        oeInvCnfgTable.on('click', 'tbody td', function (e) {
            var selectedTr = $(this).parents('tr')[0]; _selectedrow = selectedTr; var aData = oeInvCnfgTable.fnGetData(selectedTr); _targetclick = '';
            if (oeInvCnfgTable.fnIsOpen(selectedTr) && ((e.target.className == 'glyphicon glyphicon-pencil') || (e.target.className == 'fa fa-copy'))) {
                oeInvCnfgTable.fnClose(selectedTr);
            }
            else {
                var divid = 'dv' + selectedTr._DT_RowIndex;
                if (e.target.className == 'glyphicon glyphicon-pencil') {
                    _ispdfupdate = selectedTr._DT_RowIndex;  _targetclick = e.target.innerText;
                    if ((previousTr != '') && (oeInvCnfgTable.fnIsOpen(previousTr) && previousTr != selectedTr)) { var prevdivid = 'dv' + previousTr._DT_RowIndex; oeInvCnfgTable.fnClose(previousTr); $('#' + prevdivid).show(); }
                    if (aData != null) {
                        oeInvCnfgTable.fnOpen(selectedTr, fneInvPDFFormatDetails(oeInvCnfgTable, selectedTr, 'Edit'), 'details'); $('#' + divid).hide(); previousTr = selectedTr;
                    }
                }
                else if (e.target.className == 'glyphicon glyphicon-trash') {
                    if (confirm('Are you sure ? You want to delete eInvoice PDF Mapping ?')) {
                        if (aData != null) {   var _invmapcode = aData[1]; var _mapid = aData[10]; var _supplierid = aData[12]; Delete_eInvPDFDetails(_mapid, _supplierid, _invmapcode, GeteInvoicePDFConfigGrid);  }
                    } previousTr = selectedTr;
                }
                else if (e.target.className == 'fa fa-download') {
                    _mapdet = [];
                    if ((previousTr != '') && ( oeInvCnfgTable.fnIsOpen(previousTr) && previousTr != selectedTr)) { var prevdivid = 'dv' + previousTr._DT_RowIndex;  oeInvCnfgTable.fnClose(previousTr); $('#' + prevdivid).show(); }
                    if (aData != null) {
                        var _invmapid = Str(aData[11]); DownloadFile(_invmapid); $('#' + divid).show();
                    } previousTr = selectedTr;
                }
                else if (e.target.className == 'fa fa-upload') {
                    _mapdet = [];
                    if ((previousTr != '') && ( oeInvCnfgTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {
                        var prevdivid = 'dv' + previousTr._DT_RowIndex;  oeInvCnfgTable.fnClose(previousTr); $('#' + prevdivid).show();
                    }
                    if (aData != null) { $("#ModalUpload").modal('show'); $('#' + divid).show(); }
                    previousTr = selectedTr;
                }
                else if (e.target.name == 'Ref') {
                    if (aData != null) {
                        var _filename = Str(aData[13]); var _invMapcode = Str(aData[1]); top.location.href = "../ReferenceFiles/VOUCHER_PDF/" + _invMapcode + "/" + _filename;
                    }
                }
                $('#btnInvUpdate').click(function () {
                    if (aData != null) {   var _res = ValidateDetail('false');
                        if (_res == true) { _ispdfupdate = -2; _mapdet = [];
                            GeteInvMappRowDetails(oeInvCnfgTable, selectedTr, _mapdet, 'Edit'); Save_eInvMappingDetails(_mapdet, GeteInvoicePDFConfigGrid); $('#' + divid).show();
                        }
                    }
                });
                $('#btnInvCancel').click(function () { if (oeInvCnfgTable.fnIsOpen(selectedTr)) { oeInvCnfgTable.fnClose(selectedTr); } $('#' + divid).show(); _ispdfupdate = -2; });
            }
        });
        $('#btnRefresh').live('click', function (e) { e.preventDefault(); GeteInvoicePDFConfigGrid(); });
        $('#btnClear').live('click', function (e) { e.preventDefault(); ClearFilter(); });
        $('#btnNew').live('click', function (e) { e.preventDefault(); FillSupplierList(); $('#txtMapCode').empty();$("#ModaleInvoice").modal('show'); });
        $('#cbSuppliers').live("change", function (e) { var _selectspp = $('#cbSuppliers option:selected').val(); var _invoice_MapCode = GetInvoiceMapCode(_selectspp); $('#txtMapCode').val(_invoice_MapCode); });
        $('#btnAddeInvMapp').click(function (e) {
            e.preventDefault(); _lstInvdet = []; var _res = ValidateDetail('true');
            if (_res == true) { GeteInvMappRowDetails(oeInvCnfgTable, '', _lstInvdet, 'New'); Save_eInvMappingDetails(_lstInvdet, GeteInvoicePDFConfigGrid); $("#ModaleInvoice").modal('hide'); }
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
        var dtfilter = '<th style="text-align:center;">#</th><th>Map Code</th><th>Supplier <br/> Code</th><th>Supplier Name</th><th>Map <br/> Range(1)</th><th>Map Range (1) <br/> Value</th><th>Map <br/> Range(2)</th><th>Map Range (2) <br/> Value</th><th>Map <br/> Range (3)</th><th>Map Range (3) <br/> Value</th> <th>MAP_ID</th><th>PDF_MAPID</th><th>BUYERID</th><th>Sample File</th>';
        $('#tblHeadRoweInvPDFCfg').empty().append(dtfilter); $('#tblBodyeInvPDFCfg').empty();
    };

    function ClearFilter() { setFilterToolbar(); $('#dataGrideInvPDFCfg').DataTable().clear().draw(); };

    var setFilterToolbar = function () {
        $('#toolbtngroup').empty();
        var _btns = ' <div class="row" style="margin-bottom: 2px;padding-right: 20px;"> <div class="col-md-12">' +
            ' <div id="toolbtngroup">' +
            ' <span title="New" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnNew"><i class="fa fa-plus" style="text-align:center;"></i></a></div>' +
            ' <span title="Clear" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnClear"><i class="fa fa-eraser" style="text-align:center;"></i></a></div>' +
            ' <span title="Refresh" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"> <a href="javascript:;" class="toolbtn" id="btnRefresh"><i class="fa fa-recycle" style="text-align:center;"></i></a></div></div>' +
            ' </div></div>';
        $('#toolbtngroup').append(_btns);    
    };
      
    return {
        init: function () { handleeInvoicePDFConfigTable(); }
    };
}();

function FillMapCodesList() {
    _lstMapCode = [];
    $.ajax({
        type: "POST",
        async: false,
        url: "eInvoicePDFConfig.aspx/GetPDFDoctypes",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            try {
                var Dataset = JSON.parse(response.d);
                if (Dataset.NewDataSet != null) {
                    var Table = Dataset.NewDataSet.Table;
                    if (Table != undefined && Table != null) {
                        if (Table.length != undefined) { for (var i = 0; i < Table.length; i++) {   _lstMapCode.push(Str(Table[i].INV_MAP_CODE) + "|" + Str(Table[i].INV_MAP_CODE));}}
                        else {  if (Table.INV_MAP_CODE != undefined) {  _lstMapCode.push(Str(Table.INV_MAP_CODE) + "|" + Str(Table.INV_MAP_CODE)); }}
                    }
                }
            }
            catch (err) {   toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd"); }
        },
        failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
        error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
    });
};

function FillSupplierList() {
    _lstSpp = [];
    $.ajax({
        type: "POST",
        async: false,
        url: "eInvoicePDFConfig.aspx/GetSuppliersOnly",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            try {
                var Dataset = JSON.parse(response.d);
                if (Dataset.NewDataSet != null) {  var Table = Dataset.NewDataSet.Table1;
                    if (Table != undefined && Table != null) {
                        if (Table.length != undefined) {   for (var i = 0; i < Table.length; i++) {  _lstSpp.push(Str(Table[i].ADDRESSID) + "|" + Str(Table[i].ADDR_NAME));} }
                        else { if (Table.ADDRESSID != undefined) { _lstSpp.push(Str(Table.ADDRESSID) + "|" + Str(Table.ADDR_NAME)); }  }
                    }
                }
                var _sppdet = FillCombo('', _lstSpp); $('#cbSuppliers').empty().append(_sppdet);
            }
            catch (err) { toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd"); }
        },
        failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
        error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
    });
};

var GetInvoiceMapCode = function(SUPPLIERID) {
    var _invmapcode = '';
    $.ajax({
        type: "POST",
        async: false,
        url: "eInvoicePDFConfig.aspx/GetInvoiceMapCode_SupplierId",
        data: "{'SUPPLIERID':'" + SUPPLIERID + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            try {
                _invmapcode = Str(response.d);          
            }
            catch (err) {  toastr.error('Unable to Generate Invoice MapCode', "Lighthouse eSolutions Pte. Ltd");  }
        },
        failure: function (response) { toastr.error("Failure", "Lighthouse eSolutions Pte. Ltd"); },
        error: function (response) { toastr.error("Error", "Lighthouse eSolutions Pte. Ltd"); }
    });
    return _invmapcode;
};

function fneInvPDFFormatDetails(oTable, nTr, _targetclick) {
    var sOut = ''; var _str = ''; var _status = ''; var _selectopt = ''; var _mapcodetxt = '';  var _ipdisable = 'disabled';
    var indx = nTr.rowIndex; var aData = oTable.fnGetData(nTr); var tid = "MappTable" + indx; var _tbodyid = "tblBodyInvCnfgCfg" + indx;
    var _Spcodeid = 'lblSuppliercode'; var _mapcodeid = 'lblMapCode';
    var _mrange1id = 'txtMap1'; var _mrangeval1id = 'txtMapvalue1'; var _mrange2id = 'txtMap2'; var _mrangeval2id = 'txtMapvalue2'; var _mrange3id = 'txtMap3'; var _mrangeval3id = 'txtMapvalue3';
    _mapcodetxt = Str(aData[1]);  var _SCode = Str(aData[2]);  var _maprange1 =  Str(aData[4]);var _maprangeval1 = Str(aData[5]); var _maprange2 =  Str(aData[6]);
    var _maprangeval2 = Str(aData[7]) ; var _maprange3 =  Str(aData[8]);var _maprangeval3 = Str(aData[9]);
    var btndiv = '<div class="row"><div style="text-align:center;"><a href="#" id="btnInvUpdate"><u>Update</u></<a>&nbsp;<a href="#" id="btnInvCancel"><u>Cancel</u></<a></div></div>';
    var sOut = '<div class="row"> <div class="col-md-10"> ' +
          ' <div class="row"><div class="col-md-10"><div class="form-group">' +
          ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Supplier Code</label> </div>' +
          ' <div  class="col-md-3"><label class="dvLabel label-margin" id="' + _Spcodeid + '">' + _SCode + '</label> </div>' +
          ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Code </label> </div>' +
          ' <div  class="col-md-3"><label class="dvLabel label-margin"  id="' + _mapcodeid + '">' + _mapcodetxt + '</label>' +
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

function GeteInvMappRowDetails(Table, nTr, _lstdet, _targetclick) {
    var indx =''; var _mapid='0'; var _supplierid='';var _mapcode=''; var _emptystr='';
    if (_targetclick == 'Edit') { var aData = Table.fnGetData(nTr); _mapid = aData[10]; _supplierid = aData[12]; _mapcode = $('#lblMapCode').text(); }
    else{_supplierid = $('#cbSuppliers option:selected').val();_mapcode =$('#txtMapCode').val();}
    var _maprange1 = (_targetclick == 'Edit')?$('#txtMap1').val():_emptystr; var _maprangeval1 =(_targetclick == 'Edit')? $('#txtMapvalue1').val():_emptystr;
    var _maprange2 = (_targetclick == 'Edit')?$('#txtMap2').val():_emptystr; var _maprangeval2 =(_targetclick == 'Edit')?$('#txtMapvalue2').val():_emptystr; 
    var _maprange3 =(_targetclick == 'Edit')?$('#txtMap3').val():_emptystr;
    var _maprangeval3 = (_targetclick == 'Edit')?$('#txtMapvalue3').val():_emptystr;
    _lstdet.push("SUPPLIERID" + "|" + Str(_supplierid)); _lstdet.push("INV_MAP_CODE" + "|" + Str(_mapcode)); _lstdet.push("MAP_ID" + "|" + Str(_mapid));
    _lstdet.push("MAPPING_1" + "|" + Str(_maprange1)); _lstdet.push("MAPPING_1_VALUE" + "|" + Str(_maprangeval1));
    _lstdet.push("MAPPING_2" + "|" + Str(_maprange2)); _lstdet.push("MAPPING_2_VALUE" + "|" + Str(_maprangeval2));
    _lstdet.push("MAPPING_3" + "|" + Str(_maprange3)); _lstdet.push("MAPPING_3_VALUE" + "|" + Str(_maprangeval3)); 
};

function FilleInvoicePDFConfigGrid(Table) {
    try {
        $('#dataGrideInvPDFCfg').DataTable().clear().draw();
        if (Table.length != undefined && Table.length != null) {
            var t = $('#dataGrideInvPDFCfg').dataTable();
            for (var i = 0; i < Table.length; i++) {
                var cells = new Array();
                var r = jQuery('<tr id=' + i + '>');
                cells[0] = Str(''); cells[1] = Str(Table[i].INV_MAP_CODE);cells[2] = Str(Table[i].E_SUPPLIER_CODE); cells[3] = Str(Table[i].E_SUPPLIER_NAME);
                cells[4] = Str(Table[i].MAPPING_1);cells[5] = Str(Table[i].MAPPING_1_VALUE); cells[6] = Str(Table[i].MAPPING_2); cells[7] = Str(Table[i].MAPPING_2_VALUE);
                cells[8] = Str(Table[i].MAPPING_3); cells[9] = Str(Table[i].MAPPING_3_VALUE);
                cells[10] = Str(Table[i].MAP_ID); cells[11] = Str(Table[i].INV_PDF_MAPID);  cells[12] = Str(Table[i].SUPPLIERID);  cells[13] = Str('');
                var ai = t.fnAddData(cells, false);
            }
            t.fnDraw();
        }
        else {
            if (Table.MAP_ID != undefined && Table.MAP_ID != null) {
                var t = $('#dataGrideInvPDFCfg').dataTable();
                var r = jQuery('<tr id=' + 1 + '>');
                var cells = new Array();
                cells[0] = Str('');  cells[1] = Str(Table.INV_MAP_CODE); cells[2] = Str(Table.E_SUPPLIER_CODE); cells[3] = Str(Table.E_SUPPLIER_NAME);
                cells[4] = Str(Table.MAPPING_1); cells[5] = Str(Table.MAPPING_1_VALUE); cells[6] = Str(Table.MAPPING_2); cells[7] = Str(Table.MAPPING_2_VALUE);
                cells[8] = Str(Table.MAPPING_3); cells[9] = Str(Table.MAPPING_3_VALUE);  cells[10] = Str(Table.MAP_ID);  cells[11] = Str(Table.INV_PDF_MAPID);  
                cells[12] = Str(Table.SUPPLIERID); cells[13] = Str('');
                var ai = t.fnAddData(cells, false);
                t.fnDraw();
            }
        }
    }
    catch (e)
    { }
};

 var GeteInvoicePDFConfigGrid = function () {
    Metronic.blockUI('#portlet_body');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "eInvoicePDFConfig.aspx/GeteInvoicePDFConfigGrid",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) {var  Table = DataSet.NewDataSet.Table1; FilleInvoicePDFConfigGrid(Table); }
                    else $('#dataGrideInvPDFCfg').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get eInvoice PDF Config :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI();},
            error: function (response) {  toastr.error("error get", response.responseText); Metronic.unblockUI();}
        });
    }, 200);
};

function ValidateDetail(isNew) {
    var _valid = true;
    if (isNew == 'true') {
        var _supplier = $('#cbSuppliers option:selected').val(); var _mapcode = $('#txtMapCode').val();
        if (_supplier == '') { $($('#cbSuppliers').select2('container')).addClass('error'); _valid = false; } else { $($('#cbSuppliers').select2('container')).removeClass('error'); }
        if (_mapcode == '') { $('#txtMapCode').addClass('error'); _valid = false; } else {
            var isexist = CheckExistingeInvoice(_mapcode);
            if (isexist == '') { $('#txtMapCode').removeClass('error'); } else { $('#txtMapCode').addClass('error'); toastr.error("Lighthouse eSolutions Pte. Ltd.", isexist); _valid = false; }
        }
    }
    return _valid;
};

var CheckExistingeInvoice = function (MAP_CODE) {
    var res = '';
    $.ajax({
        type: "POST",
        async: false,
        url: "eInvoicePDFConfig.aspx/CheckExistingeInvoice_Mapping",
        data: "{ 'MAP_CODE':'" + MAP_CODE + "' }",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            try {
                res = Str(response.d);
            }
            catch (err) {   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'cannot validate eInvoice mapping :' + err);}
        },
        failure: function (response) { toastr.error("failure get", response.d);},
        error: function (response) {  toastr.error("error get", response.responseText);}
    });
    return res;
};

function Save_eInvMappingDetails(_nfieldval, callback) {
    var sleInvMappdet = [];for (var j = 0; j < _nfieldval.length; j++) { sleInvMappdet.push(_nfieldval[j]); }
    var data2send = JSON.stringify({ sleInvMappdet: sleInvMappdet });
    Metronic.blockUI('#portlet_body');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "eInvoicePDFConfig.aspx/Save_eInvoicePDFMapping",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "eInvoice PDF Mapping Saved successfully."); callback(); }Metronic.unblockUI();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update eInvoice PDF Mapping :' + err); Metronic.unblockUI(); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    }, 200);
};

function Delete_eInvPDFDetails(MAPID,SUPPLIERID,INVOICEMAP_CODE, callback) {
    Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "eInvoicePDFConfig.aspx/Delete_eInvoicePDFMapping",
            data: "{'MAPID':'" + MAPID + "','SUPPLIERID':'" + SUPPLIERID + "','INVOICEMAP_CODE':'" + INVOICEMAP_CODE + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "eInvoice PDF Mapping Deleted.");  callback();} Metronic.unblockUI();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete eInvoice PDF Mapping :' + err); }
            },
            failure: function (response) {  toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) {  toastr.error("error get", response.responseText); Metronic.unblockUI();  }
        });
};

function DownloadFile(INV_PDF_MAPID) {
    _refrencefiles = '';
    try {
        $.ajax({
            type: "POST",
            async: false,
            url: "eInvoicePDFConfig.aspx/DownloadFormat",
            data: "{ 'INV_PDF_MAPID':'" + INV_PDF_MAPID +  "' }",
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
                catch (err) {}
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
            url: "eInvoicePDFConfig.aspx/DownloadFormat",
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
        url: "eInvoicePDFConfig.aspx/GetURLdetails",
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

function ShowReferenceFiles(INV_MAP_CODE) {
    var result = '';
    $.ajax({
        type: "POST",
        async: false,
        url: "eInvoicePDFConfig.aspx/GetReferenceFiles",
        data: "{ 'INV_MAP_CODE':'" + INV_MAP_CODE + "' }",
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