var _fromlogdate = ''; var _tologdate = ''; var _linkid = '-1'; var filetype = ''; var _lstformats = []; var BUYERID = ''; var SUPPLIERID = ''; var SUPPDETAILS = '';
var LINKID = ''; var BUYDETAILS = []; var aSession = []; var selectedval = ''; var oExportTable = ''; var oSuppTable = ''; var oBuyerTable = '';

var FileReprocess = function () {

    var handleFileReprocessTable = function () {
        setTimeout(function () { CommonSettings() , 200 });
        $('#btnBuyer').on("click", function (e) { $("#ModalBuyer").modal('show');});
        $('#btnSupplier').on("click", function (e) { InitializeSupplier(); $("#ModalSupplier").modal('show'); });
        $("#ModalBuyer").on('shown.bs.modal', function () { oBuyerTable.fnDraw(); });
        $("#ModalSupplier").on('shown.bs.modal', function () { oSuppTable.fnDraw();});
        $('#dataGridSupp').on('click', ' tbody td', function (e) {
            $checkbox = $($(this).parents('tr')[0]).find(':checkbox');  $checkbox.attr('checked', true);nTr = $(this).parents('tr')[0];  SUPPDETAILS = '';var colIndx = $(this).index();           
            var addrid = oSuppTable.fnGetData(nTr)[3]; var addrname = oSuppTable.fnGetData(nTr)[2]; SUPPDETAILS = addrid + '|' + addrname;
        });
        $('#btnSpSelect').on('click', function (e) {
            e.preventDefault();  var BUYERID = ''; ClearDetails(2);   SUPPLIERID = SUPPDETAILS.split('|')[0]; var name = SUPPDETAILS.split('|')[1]; document.getElementById("txtSupplier").value = name;
            GetBuyerDetails(SUPPLIERID); $("#ModalSupplier").modal('hide');
        });
        $('#dataGridByer').on('click', ' tbody td', function (e) {
            $checkbox = $($(this).parents('tr')[0]).find(':checkbox'); $checkbox.attr('checked', true);
            nTr = $(this).parents('tr')[0];   var _arr = []; BUYDETAILS = _arr; var colIndx = $(this).index();
            var linkid = oBuyerTable.fnGetData(nTr)[7]; var buyname = oBuyerTable.fnGetData(nTr)[2]; var buyimp_path = Str(oBuyerTable.fnGetData(nTr)[5]); var suppimp_path = oBuyerTable.fnGetData(nTr)[6];           
            BUYDETAILS.push('LINKID |' + linkid); BUYDETAILS.push('BUYERNAME |' + buyname); BUYDETAILS.push('BUYERIMP_PATH |' + buyimp_path); BUYDETAILS.push('SUPPIMP_PATH |' + suppimp_path);
        });
        $('#btnBySelect').on('click', function (e) {           
            e.preventDefault();  var LINKID = ''; SetSessionValues();
            if (BUYDETAILS.length != undefined && BUYDETAILS.length > 0) {
                for (var i = 0; i < BUYDETAILS.length; i++) {
                    var name = BUYDETAILS[i].split('|')[0];  var val = BUYDETAILS[i].split('|')[1];
                    if (name.trim() == "LINKID"){ LINKID = val; sessionStorage['LINKID'] = val; }
                    else if (name.trim() == "BUYERNAME") { document.getElementById("txtBuyer").value = val; }
                    else if (name.trim() == "BUYERIMP_PATH") { sessionStorage['BUYERIMP_PATH'] = val; }
                    else if (name.trim() == "SUPPIMP_PATH") { sessionStorage['SUPPIMP_PATH'] = val; }
                }
                Setpaths();
            }
            $("#ModalBuyer").modal('hide');
        });
        $('#dataGridExp').on('click', 'tbody tr', function (e) {
            e.stopPropagation();
            if (e.target.type == "checkbox") {
                var $checkbox = $(this).find(':checkbox'); if ($checkbox.is(':checked')) { $(this).addClass("DTTT_selected selected"); } else $(this).removeClass("DTTT_selected selected");
            }
            else {
                var $checkbox = $(this).find(':checkbox'); $checkbox.attr('checked', !$checkbox.attr('checked'));
                if ($checkbox.is(':checked')) {    $(this).addClass("DTTT_selected selected");}  else $(this).removeClass("DTTT_selected selected");
            }
        });
        $('#cbFileActionType').live("change", function (e) {
            var selectedval = $('#cbFileActionType option:selected').text(); SetImportDetails(selectedval);
            if ((Str(selectedval).toUpperCase() == 'EXPORT')) {   _linkid = Str(sessionStorage.getItem('LINKID'));
                if (_fromlogdate != undefined && _tologdate != undefined) { GetExportDetails(_linkid, _fromlogdate, _tologdate); }
            }

        });
        $('#btnApply').live('click', function (e) {
            e.preventDefault();_fromlogdate = $(document.getElementById('dtUpdateFromDate')).val();_tologdate = $(document.getElementById('dtUpdateToDate')).val();
            _linkid = Str(sessionStorage.getItem('LINKID'));if (_linkid != '') { GetExportGrid(_linkid, _fromlogdate, _tologdate); }
            else { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Please Select Supplier and Buyer'); }
        });
        $('#dtUpdateFromDate').datepicker().on('changeDate', function (ev) {$('#dtUpdateFromDate').datepicker('hide');});
        $('#dtUpdateToDate').datepicker().on('changeDate', function (ev) { $('#dtUpdateToDate').datepicker('hide');});
        $('#btnUpdExport').on('click', function (e) {
            e.preventDefault();
            var aSelectedTrs = ''; var aDSelectedTrs = ''; var oExpTable = ''; var _lstexport = [];
            var _expmarktxt = $("#txtUpdateExport").val(); aSelectedTrs = $('#dataGridExp .DTTT_selected');   oExpTable = $('#dataGridExp').DataTable();
            if (aSelectedTrs != undefined && aSelectedTrs != null)
            {               
                for (var i = 0; i < aSelectedTrs.length; i++) {
                    aRow = aSelectedTrs[i];  var _lstexpdet = [];
                    var _doctype = oExpTable.context[0].aoData[aSelectedTrs[i]._DT_RowIndex]._aData[2];
                    var _vrno = oExpTable.context[0].aoData[aSelectedTrs[i]._DT_RowIndex]._aData[5];
                    var _version = oExpTable.context[0].aoData[aSelectedTrs[i]._DT_RowIndex]._aData[7];
                    var _quoteid = oExpTable.context[0].aoData[aSelectedTrs[i]._DT_RowIndex]._aData[8];
                    _lstexpdet.push("DOCTYPE"+ "|" + Str(_doctype)); _lstexpdet.push("VRNO"+ "|" + Str(_vrno));  _lstexpdet.push("VERSION" + "|" + Str(_version)); _lstexpdet.push("QUOTATIONID"+ "|" + Str(_quoteid));
                    _lstexpdet.push("EXPORTMARKER" + "|" + Str(_expmarktxt));  _lstexport.push("EXPDET" + i + "#" + _lstexpdet);
                }           
                UpdateExportMarker(_lstexport,GetExportGrid);
            }
        });
        $(".NumberInput").bind("keypress", function (event) {
            var e = event;
            if (e.shiftKey || e.ctrlKey || e.altKey) {e.preventDefault();}  else { var key = e.keyCode; if (!((key == 8) || (key >= 48 && key <= 57))) { e.preventDefault(); } }
        });      
    };

    function CommonSettings() {
        var nEditing = null; $('#pageTitle').empty().append('File Reprocessing'); SetupBreadcrumb('Home', 'Home.aspx', 'File Reprocessing', 'File_Reprocess.aspx', '', '');
        $(document.getElementById('lnkFileReProcess')).addClass('active'); setupSupplierTableHeader(); setupExportTableHeader(); setExpFilterToolbar();
        $('[data-toggle="tooltip"]').tooltip(); $('#toolbtngroup').empty(); setAccess(selectedval); ClearDetails(1);
    };

    function InitializeSupplier() {     
        var table = $('#dataGridSupp');
        oSuppTable = table.dataTable({
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
            "columnDefs": [{ 'orderable': false, 'targets': [0] },
            { 'targets': [0, 3], visible: false }, { 'targets': [1], width: '30px' }, { 'targets': [2], width: '260px' }
            ],
            "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
            paging: false,
            "sScrollY": "200px"
        });
        $('#tblHeadRowSupp').addClass('gridHeader'); $('#ToolTables_dataGridSupp_0,#ToolTables_dataGridSupp_1,#dataGridSupp_length,#dataGridSupp_paginate').hide();
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
        GetSupplierGrid();
    };

    var GetSupplierGrid = function () {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "File_Reprocess.aspx/LoadSuppliers",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table; FillSupplierGrid(Table); }
                    else $('#dataGridSupp').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Supplier details :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };

    function FillSupplierGrid(Table) {
        try {
            $('#dataGridSupp').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridSupp').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = '<td><div class=""><span style="padding-left:8px;"><input type="checkbox" class="checkboxes widelarge" value="1"></span></div></td>';                                   
                    cells[1] = Str(Table[i].ADDR_CODE); cells[2] = Str(Table[i].ADDR_NAME);  cells[3] = Str(Table[i].ADDRESSID);                
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.ADDRESSID != undefined && Table.ADDRESSID != null) {
                    var t = $('#dataGridSupp').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = '<td><div class=""><span style="padding-left:8px;"><input type="checkbox" class="checkboxes widelarge" value="1"></span></div></td>';
                    cells[1] = Str(Table.ADDR_CODE);  cells[2] = Str(Table.ADDR_NAME);  cells[3] = Str(Table.ADDRESSID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    function GetBuyerDetails(SUPPLIERID) {
        setupBuyerTableHeader();
        var table = $('#dataGridByer');
        oBuyerTable = table.dataTable({
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
            "columnDefs": [{ 'orderable': false, "searching": false, 'targets': [0] },
            { 'targets': [1, 3], width: '50px' }, { 'targets': [2], width: '120px' }, { 'targets': [4], width: '60px' },
            { 'targets': [0, 5, 6, 7], visible: false }],
            "order": [[1, "asc"]],
            "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
            paging: false,
            "sScrollY": "200px",
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); }
        });

        $('#tblHeadRowByer').addClass('gridHeader'); $('#ToolTables_dataGridByer_0,#ToolTables_dataGridByer_1,#dataGridByer_paginate').hide(); 
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
        GetBuyerGrid(SUPPLIERID);
    };

    var GetBuyerGrid = function (SUPPLIERID) {
        Metronic.blockUI('#portlet_body1');        
        $.ajax({
            type: "POST",
            async: false,
            url: "File_Reprocess.aspx/LoadBuyers",
            data: "{ 'SUPPLIERID':'" + SUPPLIERID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) {  Table = DataSet.NewDataSet.Table; FillBuyerGrid(Table);  }
                    else $('#dataGridByer').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer details :' + err); }},
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };

    function FillBuyerGrid(Table) {
        try {
            $('#dataGridByer').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridByer').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = '<td><div class=""><span style="padding-left:12px;"><input type="checkbox" class="checkboxes widelarge" value="1"></span></div></td>';
                    cells[1] = Str(Table[i].BUYER_CODE);cells[2] = Str(Table[i].BUYER_NAME);   cells[3] = Str(Table[i].BUYER_LINK_CODE);
                    cells[4] = Str(Table[i].VENDOR_LINK_CODE); cells[5] = Str(Table[i].IMPORT_PATH);   cells[6] = Str(Table[i].SUPP_IMPORT_PATH);   cells[7] = Str(Table[i].LINKID);                    
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.LINKID != undefined && Table.LINKID != null) {
                    var t = $('#dataGridByer').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = '<td><div class=""><span style="padding-left:12px;"><input type="checkbox" class="checkboxes widelarge" value="1"></span></div></td>';
                    cells[1] = Str(Table.BUYER_CODE); cells[2] = Str(Table.BUYER_NAME);  cells[3] = Str(Table.BUYER_LINK_CODE);
                    cells[4] = Str(Table.VENDOR_LINK_CODE);  cells[5] = Str(Table.IMPORT_PATH);cells[6] = Str(Table.SUPP_IMPORT_PATH); cells[7] = Str(Table.LINKID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    var setupSupplierTableHeader = function () {
        var dtHeader = '<th id="cbSuppheader"></th><th> Code</th><th>Supplier Name</th><th>AddressID</th>';
        $('#tblHeadRowSupp').empty(); $('#tblHeadRowSupp').append(dtHeader);  $('#tblBodySupp').empty();
    };
    var setupBuyerTableHeader = function () {
      var dtHeader = '<th></th><th>Buyer Code</th><th>Buyer Name</th><th>Buyer Link Code</th><th>Supplier Link Code</th><th>Import Path</th><th>Supp Import Path</th><th>LinkID</th>';
        $('#tblHeadRowByer').empty();$('#tblHeadRowByer').append(dtHeader);$('#tblBodyByer').empty();
    };
    var setupExportTableHeader = function () {
        var dtHeader = '<th id="cbExpheader"></th><th>Log Date</th><th>Doc Type</th><th>Buyer Code</th><th>Supplier Code</th><th>VR No.</th><th>Export Marker</th><th>Version</th><th>QuotationID</th>';
        $('#tblHeadRowExp').empty();  $('#tblHeadRowExp').append(dtHeader); $('#tblBodyExp').empty();       
    };
    var setExpFilterToolbar = function () {
        if ($("#btnApply").length == 0) 
        {
            $('#toolbtngroup').empty(); $('#divFilter').empty();
            var _fltr = ' <div class="row"> <div class="col-md-12"><div class="col-md-4" style="text-align:right;">' +
            ' <div class="col-md-4"><label class="control-label">From Date </label>  </div>' +
            ' <div class="col-md-8 FilterInput"> <input id="dtUpdateFromDate" class="form-control date-picker csDatePicker InputText"  data-date-format="dd/mm/yyyy" size="16" type="text" value=""> </div>' +
            ' </div><div class="col-md-4" style="text-align:right;">' +
             ' <div class="col-md-4"><label class="control-label">To Date </label>  </div>' +
             ' <div class="col-md-8 FilterInput"> <input id="dtUpdateToDate" class="form-control date-picker csDatePicker InputText"  data-date-format="dd/mm/yyyy" size="16" type="text" value=""></div>' +
             ' </div> <div class="col-md-4><span title="Apply" data-toggle="tooltip" data-placement="top"><a href="javascript:;" class="toolbtn_exp" id="btnApply"><i class="glyphicon glyphicon-refresh icon-refresh"></i></a></div>' +
             '</div></div>';
            var _expftr = '<div class="row" style ="padding-top:5px;"> <div class="col-md-12">'+
             ' <div class="col-md-6"><label class="control-label">Please select a record to Export</label>  ' +
             ' <span title="Update Export" data-toggle="tooltip" data-placement="top"><a href="javascript:;" class="toolbtn_exp" id="btnUpdExport"><i class="fa fa-pencil-square-o"></i></a></div>' +             
             ' <div class="col-md-6"><label class="control-label">Export </label>' +
             ' <input type="text" class="form-control input-sm input-inline input-mini NumberInput" id="txtUpdateExport" value="1"/> </div>' +
            '</div></div>';
            $('#divFilter').append(_fltr + '<hr/>'+_expftr);}
        var date = new Date();
        $(document.getElementById('dtUpdateFromDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
        $(document.getElementById('dtUpdateToDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
        _fromlogdate = $(document.getElementById('dtUpdateFromDate')).val(); _tologdate = $(document.getElementById('dtUpdateToDate')).val();
    };

    function GetExportDetails(LINKID, _fromlogdate, _tologdate) {
        var table = $('#dataGridExp');
        oExportTable = table.dataTable({
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
                "sRowSelect": "multi",
                "aButtons": ["select_all", "select_none"]
            },
            "columnDefs": [{ 'orderable': false, "searching": false, 'targets': [0] },
              { 'targets': [0], width: '5px' }, { 'targets': [1, 6], width: '30px' }, { 'targets': [2], width: '60px' },
              { 'targets': [3], width: '50px' }, { 'targets': [4], width: '40px' }, { 'targets': [5], width: '60px' }, { 'targets': [6,7], width: '40px' },
              { 'targets': [8], visible: false }],
            "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
            "aaSorting": [],
           // "scrollY": "200px"
        });

        $('#tblHeadRowExp').addClass('gridHeader'); $('#ToolTables_dataGridExp_0,#ToolTables_dataGridExp_1,#dataGridExp_paginate,#dataGridExp_length').hide();
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");  
        GetExportGrid(LINKID, _fromlogdate, _tologdate);
    };

    var GetExportGrid = function (LINKID, _fromlogdate, _tologdate) {
        Metronic.blockUI('#portlet_bodyexp');
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "File_Reprocess.aspx/GetExportMarkerGrid",
                data: "{'LINKID':'" + LINKID + "','LOG_DATEFROM':'" + getSQLDateFormated(_fromlogdate) + "','LOG_DATETO':'" + getSQLDateFormated(_tologdate) + "' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        if (response.d != "") {
                            var DataSet = JSON.parse(response.d); if (DataSet.NewDataSet != null) {  Table = DataSet.NewDataSet.Table; FillExportMarkerGrid(Table);  }
                            else $('#dataGridExp').DataTable().clear().draw();
                        }
                        Metronic.unblockUI();
                    }
                    catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Export Marker details :' + err); }
                },
                failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
                error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
            });
       }, 20);
    };

    function FillExportMarkerGrid(Table) {
        try {
            $('#dataGridExp').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridExp').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array(); var r = jQuery('<tr id=' + i + '>');
                    cells[0] = '<td><span style="padding-left:20px;text-align:center;"><input type="checkbox" class="checkboxes widelarge" value="1"></span></td>';
                    cells[1] = Str(Table[i].UPDATEDATE);  cells[2] = Str(Table[i].DOC_TYPE);  cells[3] = Str(Table[i].BUYER_CODE);  cells[4] = Str(Table[i].VENDOR_CODE);
                    cells[5] = Str(Table[i].VRNO); cells[6] = Str(Table[i].EXPORTED); cells[7] = Str(Table[i].VERSION); cells[8] = Str(Table[i].QUOTATIONID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.QUOTATIONID != undefined && Table.QUOTATIONID != null) {
                    var t = $('#dataGridExp').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = '<td><span style="padding-left:20px;text-align:center;"><input type="checkbox" class="checkboxes widelarge" value="1"></span></td>';
                    cells[1] = Str(Table.UPDATEDATE); cells[2] = Str(Table.DOC_TYPE); cells[3] = Str(Table.BUYER_CODE); cells[4] = Str(Table.VENDOR_CODE);
                    cells[5] = Str(Table.VRNO); cells[6] = Str(Table.EXPORTED); cells[7] = Str(Table.VERSION); cells[8] = Str(Table.QUOTATIONID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    function SetImportDetails(selectedval) { setAccess(selectedval); SetSessionValues(); Setpaths(); };

    function Setpaths() {
        var selectedval = $('#cbFileActionType option:selected').text(); var Ispathfnd = '0'; $('#spnmtml').text(''); $('#spnlesxml').text(''); $('#spnByFile').text(''); $('#spnSuppfile').text('');
        if (selectedval != '') {
            if (selectedval.toUpperCase() == 'IMPORT BUYER FILE') { if (Str(sessionStorage.getItem('BUYERIMP_PATH')) == '') { Ispathfnd = '0'; $('#spnByFile').text('No file path found'); } else { Ispathfnd = '1'; $('#spnByFile').text(Str(sessionStorage.getItem('BUYERIMP_PATH'))); } }
            else if (selectedval.toUpperCase() == 'IMPORT SUPPLIER FILE') { if (Str(sessionStorage.getItem('SUPPIMP_PATH')) == '') { Ispathfnd = '0'; $('#spnSuppfile').text('No file path found'); } else { Ispathfnd = '1'; $('#spnSuppfile').text(Str(sessionStorage.getItem('SUPPIMP_PATH'))); } }
            else if (selectedval.toUpperCase() == 'IMPORT MTML FILE') { if (Str(sessionStorage.getItem('MTML_PATH')) == '') { Ispathfnd = '0'; $('#spnmtml').text('No file path found'); } else { Ispathfnd = '1'; $('#spnmtml').text(Str(sessionStorage.getItem('MTML_PATH'))); } }
            else if (selectedval.toUpperCase() == 'IMPORT LESXML FILE') { if (Str(sessionStorage.getItem('LeSXML_PATH')) == '') { Ispathfnd = '0'; $('#spnlesxml').text('No file path found'); } else { Ispathfnd = '1'; $('#spnlesxml').text(Str(sessionStorage.getItem('LeSXML_PATH'))); } }
        }
        if (Ispathfnd == '1') { $('.ajax__fileupload_selectFileContainer,.ajax__fileupload_topFileStatus').fadeIn(); } else { $('.ajax__fileupload_selectFileContainer,.ajax__fileupload_topFileStatus').fadeOut(); }
    };

    function ClearDetails(select) {
        $('#rdmtml').prop('checked', false); $('#rdlesxml').prop('checked', false); $('#rdByFile').prop('checked', false); $('#rdSuppfile').prop('checked', false);
        document.getElementById("txtSupplier").value = ''; document.getElementById("txtBuyer").value = '';
        if (select == 1) { $('#cbFileActionType').find('option:first').attr('selected', 'selected'); $('#importbody').hide(); $('#exportbody').hide(); }
        jQuery('.ajax__fileupload_dropzone').remove(); $('.ajax__fileupload_selectFileContainer,.ajax__fileupload_topFileStatus').fadeOut();
        $('#spnmtml').text(''); $('#spnlesxml').text(''); $('#spnByFile').text(''); $('#spnSuppfile').text(''); ClearSessionvalues();
    };
    function UpdateExportMarker(fieldValues,callback) {
        var lstExpdet = [];
        for (var i = 0; i < fieldValues.length; i++) { lstExpdet.push(fieldValues[i]);  }
        var data2send = JSON.stringify({ lstExpdet: lstExpdet });
        jQuery.ajax({
            type: 'POST',
            async: false,
            url: 'File_Reprocess.aspx/UpdateExportMarker',
            data: data2send,
            contentType: 'application/json; charset=uts-8',
            dataType: "json",
            success: function (response) {
                var res = Str(response.d);
                if (res != "") {
                    $('#txtUpdateExport').val(1);  GetExportGrid(_linkid, _fromlogdate, _tologdate);
                    toastr.success("Lighthouse eSolutions Pte. Ltd.", res);
                }
                else { toastr.error("Lighthouse eSolutions Pte. Ltd.", '"Unable to update Export marker"'); }
            },
            failure: function (response) { toastr.error("Lighthouse eSolutions Pte. Ltd.", '"Unable to update Export marker :' + err, response.d); },
            error: function (response) { toastr.error("Lighthouse eSolutions Pte. Ltd.", '"Unable to update Export marker :', response.responseText); }
        });
    };
    function SetSessionValues() {
        jQuery.ajax({
            type: 'POST',
            async: false,
            url: 'File_Reprocess.aspx/GetLes_MtmlImportPaths',
            data: "{}",
            contentType: 'application/json; charset=uts-8',
            success: function (ds) {
                aSession = JSON.parse(ds.d); sessionStorage['MTML_PATH'] =  aSession[0] ;  sessionStorage['LeSXML_PATH'] = (aSession[1]) ;  sessionStorage['SUPPLIERID'] = aSession[2];
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
    };
    function ClearSessionvalues(){ var _emptystr='';
     sessionStorage['SUPPLIERID'] = _emptystr;sessionStorage['LINKID'] = _emptystr;sessionStorage['BUYERIMP_PATH'] = _emptystr;sessionStorage['SUPPIMP_PATH'] = _emptystr;
    };
    function setAccess(selectedval) {
        ClearDetails(2); $('#dvBuyer').hide(); $('#dvSupplier').hide();
        $('#importbody').hide(); $('#exportbody').hide(); $('#dvmtml').hide(); $('#dvlesxml').hide(); $('#dvByFile').hide(); $('#dvSuppfile').hide();
        if (selectedval != '') {
            if (selectedval.toUpperCase() == 'IMPORT BUYER FILE') { $('#importbody').show(); $('#dvBuyer').show(); $('#dvSupplier').show(); $('#dvByFile').show(); }
            else if (selectedval.toUpperCase() == 'IMPORT SUPPLIER FILE') { $('#importbody').show(); $('#dvBuyer').show(); $('#dvSupplier').show(); $('#dvSuppfile').show(); }
            else if (selectedval.toUpperCase() == 'IMPORT MTML FILE') { $('#importbody').show(); $('#dvBuyer').hide(); $('#dvSupplier').hide(); $('#dvmtml').show(); }
            else if (selectedval.toUpperCase() == 'IMPORT LESXML FILE') { $('#importbody').show(); $('#dvBuyer').hide(); $('#dvSupplier').hide(); $('#dvlesxml').show(); }
            else if (selectedval.toUpperCase() == 'EXPORT') { $('#exportbody').show(); $('#dvBuyer').show(); $('#dvSupplier').show(); }
        }
    };
     return {  init: function () { handleFileReprocessTable(); } };
}();

function onClientUploadStart(sender, e) {
    var FILEPATH = '';
    if ($('#spnmtml').text() != '') { FILEPATH = $('#spnmtml').text().replace(/\\/g, '?'); }
    else if ($('#spnlesxml').text() != '') { FILEPATH = $('#spnlesxml').text().replace(/\\/g, '?'); }
    else if ($('#spnByFile').text() != '') { FILEPATH = $('#spnByFile').text().replace(/\\/g, '?'); }
    else if ($('#spnSuppfile').text() != '') { FILEPATH = $('#spnSuppfile').text().replace(/\\/g, '?'); }
    
    $.ajax({
        type: "POST",
        async: false,
        url: "File_Reprocess.aspx/GetUploadPath",
        data: "{'FILEPATH':'" + FILEPATH + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            try {
                result = Str(response.d);
                if (result == '-1') { toastr.error('Unable to upload file since no path found', "Lighthouse eSolutions Pte. Ltd"); result = -1; }
            }
            catch (err) { toastr.error('Error in Starting Client Upload :' + err, "Lighthouse eSolutions Pte. Ltd"); result = -1; }
        },
        failure: function (response) { toastr.error("Failure in Starting Client Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); result = -1; },
        error: function (response) { toastr.error("Error in Starting Client Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); result = -1; }
    });
};

function onClientUploadComplete(sender, e) {
    var file = e._filename;
    $.ajax({
        type: "POST",
        async: false,
        url: "File_Reprocess.aspx/UploadFileMapping",
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


