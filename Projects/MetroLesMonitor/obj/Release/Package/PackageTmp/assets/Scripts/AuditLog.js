var _fromlogdate = '';var _tologdate = '';var _searchfilter = ''; var _auditcount = ''; var _lstfilter = [];var _lstMod = [];
var _FROM = 1; var _TO = 100; var _orgdiff = 100; var _totalcount = ''; var _qSearch = '';

var AuditLog = function () {

    var handleAuditLogTable = function () {
        SetupBreadcrumb('Home', 'Home.aspx', 'Audit', '#', 'Audit Log', 'AuditLog.aspx'); $('#pageTitle').empty().append('Audit Log');
        $(document.getElementById('lnkAuditDet')).addClass('active open'); $(document.getElementById('spAudlog')).addClass('title font-title SelectedColor');
        setupTableHeader();  var refreshcheck = '<label><input type="checkbox" class="icheck"  onclick="DoAutoRefresh(this.checked,this.id)" id="chkAuditlog"/> Auto Refresh Page</label>';
        $('#divChkBox').append(refreshcheck); //SetTimerCheckBox('#chkAuditlog', 'chkAuditlog');
        
        window.onload = SetTimerCheckBox('#chkAuditlog', 'chkAuditlog');

        var table = $('#dataGridALog');
        var oTable = table.dataTable({
            "bDestroy": true,
            "bSort": false,
            responsive: true,
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
            "columnDefs": [{ 'orderable': false, "searching": true, "autoWidth": false,  'targets': [0] },
             { 'targets': [0, 11, 12, 13, 14, 15,17, 18], visible: false },//3,4
             { "sType": "date-dmy", "bSortable": true, 'targets': [1], width: '30px' }, { 'targets': [2], width: '60px' }, { 'targets': [3, 4], width: '40px' },
             { 'targets': [5, 6], width: '40px' }, { 'targets': [7], width: '45px' }, { 'targets': [8], width: '60px' },
             { 'targets': [9], width: '190px' }, { 'targets': [10], width: '90px' }, { 'targets': [16], width: '90px' }, { 'targets': [15, 18], width: '40px' },
             { 'targets': [11], width: '50px' }                       
             ],
            "lengthMenu": [ [100, 200,500, 1000, -1], [100,200, 500, 1000, "All"]],
            "pageLength": 100,
            scrollY: '300px',
            scrollX: '1090px',
            "order": [[1, "desc"]],
            "drawCallback": function (settings, json) {$('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var fileTag = '<a href="javascript:;"><u>' + aData[10] + '</u></<a>'; $('td:eq(9)', nRow).html(fileTag);
                var _spid = Str(aData[13]); var _byrid = Str(aData[12]); if (_spid != '' && _byrid != '') {
                    if (_spid != '0' && _byrid != '0') {
                        var _bspdet = GetBuyerSupplier_data(_spid, _byrid); var _bsdet = _bspdet.split('|')[0]; var _bcode = _bspdet.split('|')[1]; var _scode = _bspdet.split('|')[2];
                        var _resdet = '<div>' + _bsdet.split(',').join('<br/>') + '</div>';
                       $("td:eq(10)", nRow).html(_resdet); $("td:eq(5)", nRow).html(_bcode); $("td:eq(6)", nRow).html(_scode);
                    }
                }
            }       
        });
        $('#tblHeadRowALog').addClass('gridHeader'); $('#ToolTables_dataGridALog_0,#ToolTables_dataGridALog_1,#dataGridALog_info,#dataGridALog_paginate,#dataGridALog_length,#dataGridALog_filter').hide();
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
        DisplayAuditGrid();
        
        $('#btnApply').on('click', function (e) {
            e.preventDefault(); _fromlogdate = $(document.getElementById('dtLogFromDate')).val();  _tologdate = $(document.getElementById('dtLogToDate')).val();
            GetAuditLogGrids(_fromlogdate, _tologdate);
        });
        $('#dtLogFromDate').datepicker() .on('changeDate', function (ev) {$('#dtLogFromDate').datepicker('hide'); });
        $('#dtLogToDate').datepicker().on('changeDate', function (ev) {$('#dtLogToDate').datepicker('hide'); });
        $('#btnReSubmit').on('click', function (e) { e.preventDefault(); });

     
        oTable.on('click', ' tbody td', function (e) {
            $checkbox = $($(this).parents('tr')[0]).find(':checkbox'); $checkbox.attr('checked', true);
            nTr = $(this).parents('tr')[0]; var colIndx = $(this).index();
            var updatedate = oTable.fnGetData(nTr)[1]; var servername = oTable.fnGetData(nTr)[2]; var modulename = oTable.fnGetData(nTr)[3];
            var logid = oTable.fnGetData(nTr)[14]; var filename = oTable.fnGetData(nTr)[15];
            if (colIndx == 9) { if (Str(filename) != "") { DownloadFile(updatedate, modulename, filename, servername); } }
            else if (colIndx == 7) { if (Str(filename) != "") { ShowResubmitModal(logid); } }
        });

        $('#dataGridALog_filter').on('keyup', function (e) { e.preventDefault(); _qSearch = Str(e.target.value); SetGridDetails('btnNext'); });
        $('#btnRefresh').live('click', function (e) { e.preventDefault(); SetGridDetails('btnRefresh'); });
        $('#btnClear').live('click', function (e) { e.preventDefault(); ClearFilter(); });
        $('#btnMaxAudit').live('click', function (e) {
            e.preventDefault(); var _addrid = Str(sessionStorage.getItem('ADDRESSID'));
            var cCond =_addrid+'|'+  $(document.getElementById('dtLogFromDate')).val()+'|'+ $(document.getElementById('dtLogToDate')).val();
            window.open('../LESMonitorPages/AuditGrid.aspx?DETAILS=' + getEncrypted_URL(cCond));
        });
        $('#btnExpExcel').live('click', function (e) { e.preventDefault(); ExportAuditLog(); });

        jQuery.extend(jQuery.fn.dataTableExt.oSort, {
            "date-dmy-pre": function (a) {if (a == null || a == "") { return 0; }  var date = a.split('-'); return (date[2] + date[1] + date[0]) * 1; },
            "date-dmy-asc": function (a, b) {return ((a < b) ? -1 : ((a > b) ? 1 : 0)); }, "date-dmy-desc": function (a, b) { return ((a < b) ? 1 : ((a > b) ? -1 : 0)); }
        });    
        $('#btnPrev').live('click', function (e) { e.preventDefault(); SetGridDetails('btnPrev'); });
        $('#btnNext').live('click', function (e) { e.preventDefault(); SetGridDetails('btnNext'); });
        $('#txtFrom').live('change', function (e) {       
            e.preventDefault();var _txtval = parseInt($('#txtFrom').val()); _FROM = _txtval; _TO = _txtval + _orgdiff;
            if (_FROM > parseInt(_totalcount) || _FROM < 1) { $('#dataGridALog').DataTable().clear().draw(); } else { GetAuditFilterDetails(_FROM, _TO);}
        });
    };

    function SetGridDetails(_btnId) {
        if (_btnId == 'btnPrev') { _FROM = parseInt(_FROM) - _orgdiff; _TO = parseInt(_TO) - _orgdiff; GetAuditFilterDetails(_FROM, _TO); }
        else if (_btnId == 'btnNext') { _FROM = parseInt(_FROM) + _orgdiff; _TO = parseInt(_TO) + _orgdiff; GetAuditFilterDetails(_FROM, _TO); }
        else if (_btnId == 'btnRefresh') { _FROM = 1; _TO = 100; _orgdiff = 100; GetAuditFilterDetails(_FROM, _TO); }
    };

    function GetAuditFilterDetails(FROM, TO) { _lstfilter = SetFilter(FROM, TO);  GetAuditLogGrids(FROM, TO,_lstfilter); };

    var SetFilter = function (FROM, TO)
    {
        _lstfilter = [];
        _fromlogdate = $(document.getElementById('dtLogFromDate')).val(); _tologdate = $(document.getElementById('dtLogToDate')).val();
        var _servname = $(document.getElementById('txtServerName')).val(); var _doctype = $('#cboDocType option:selected').val(); var _module = $('#cboModule option:selected').val();
        var _logtype = $('#cboLogType option:selected').val(); var _buyercode = $(document.getElementById('txtBuyerCode')).val(); var _suppliercode = $(document.getElementById('txtSupplierCode')).val();
        var _keyref = $(document.getElementById('txtKeyRef')).val(); var _rmks = $(document.getElementById('txtRemarks')).val(); var _filename = $(document.getElementById('txtFileName')).val();
        var _qSearch = $(document.getElementById('txtQuickSearch')).val();
        _lstfilter.push("SERVER_NAME" + "|" + Str(_servname)); _lstfilter.push("DOC_TYPE" + "|" + Str(_doctype)); _lstfilter.push("MODULE" + "|" + Str(_module));
        _lstfilter.push("LOG_TYPE" + "|" + Str(_logtype)); _lstfilter.push("BUYER_CODE" + "|" + Str(_buyercode)); _lstfilter.push("SUPPLIER_CODE" + "|" + Str(_suppliercode));
        _lstfilter.push("KEY_REF" + "|" + Str(_keyref)); _lstfilter.push("REMARKS" + "|" + Str(_rmks)); _lstfilter.push("LOG_FROM" + "|" + getSQLDateFormated(_fromlogdate));
        _lstfilter.push("LOG_TO" + "|" + getSQLDateFormated(_tologdate)); _lstfilter.push("FILENAME" + "|" + Str(_filename)); _lstfilter.push("FROM" + "|" + Str(FROM)); _lstfilter.push("TO" + "|" + Str(TO));
        _lstfilter.push("QUICK_SEARCH" + "|" + Str(_qSearch));
        return _lstfilter;
    };

    function DisplayAuditGrid() { setFilterToolbar(); _FROM = 1; _TO = 100; _orgdiff = 100; GetAuditFilterDetails(_FROM, _TO); };

    function SetButtons(FROM, TO, _totalcount)
    {
        $('#lblrecdet').text(''); var _txt = ' to ' + _TO + ' of ' + _totalcount + ' records'; $('#lblrecdet').text(_txt); $('#txtFrom').val(_FROM);
        if (parseInt(_FROM) < 1) { $('#txtFrom').val(1); $("#btnPrev").attr('disabled', 'disabled'); }
        else if (parseInt(_FROM) == 1) { $("#btnPrev").attr('disabled', 'disabled'); }
        else if (parseInt(_FROM) < 100) { _TO = 100; }
        else if (parseInt(_FROM) < parseInt(_totalcount)) { $("#btnPrev").removeAttr('disabled'); }
        if (parseInt(_TO) < parseInt(_totalcount)) { $("#btnNext").removeAttr('disabled'); } else { $("#btnNext").attr('disabled', 'disabled'); }
    };

    function FillAuditLogGrid(Table) {
        try
        {
            $('#dataGridALog').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null)
            {
                var t = $('#dataGridALog').dataTable();
                for (var i = 0; i < Table.length; i++)
                {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    var _arrcommt = Str(Table[i].CLIENT_DETAILS).split(',').join('<br/>');
                    cells[0] = Str(''); cells[1] = Str(Table[i].UPDATE_DATE); cells[2] = Str(Table[i].SERVERNAME);  cells[3] = Str(Table[i].MODULENAME);
                    cells[4] = Str(Table[i].DOCTYPE);  cells[5] = Str(Table[i].LOGTYPE); cells[6] = Str(Table[i].BUYER_CODE); cells[7] = Str(Table[i].VENDOR_CODE);
                    cells[8] = Str(Table[i].KEYREF2); cells[9] = Str(Table[i].AUDITVALUE); cells[10] = Str(Table[i].FILENAME);   cells[11] = Str('');
                    cells[12] = Str(Table[i].BUYER_ID); cells[13] = Str(Table[i].SUPPLIER_ID);  cells[14] = Str(Table[i].LOGID); cells[15] = Str(Table[i].FILENAME);
                    cells[16] = _arrcommt; cells[17] = Str(Table[i].FILENAME2); cells[18] = Str(Table[i].FILENAME3);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else
            {
                if (Table.LOGID != undefined && Table.LOGID != null)
                {
                    var t = $('#dataGridALog').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var _arrcommt = Str(Table.CLIENT_DETAILS).split(',').join('<br/>');
                    var cells = new Array();
                    cells[0] = Str('');   cells[1] = Str(Table.UPDATE_DATE); cells[2] = Str(Table.SERVERNAME); cells[3] = Str(Table.MODULENAME);
                    cells[4] = Str(Table.DOCTYPE); cells[5] = Str(Table.LOGTYPE);cells[6] = Str(Table.BUYER_CODE); cells[7] = Str(Table.VENDOR_CODE);
                    cells[8] = Str(Table.KEYREF2); cells[9] = Str(Table.AUDITVALUE); cells[10] = Str(Table.FILENAME); cells[11] = Str('');
                    cells[12] = Str(Table.BUYER_ID); cells[13] = Str(Table.SUPPLIER_ID);  cells[14] = Str(Table.LOGID);   cells[15] = Str(Table.FILENAME);
                    cells[16] = _arrcommt; cells[17] = Str(Table.FILENAME2); cells[18] = Str(Table.FILENAME3);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }     
        }
        catch (e)
        { }
    };

    var GetAuditLogGrids = function (FROM, TO,_nfieldval) {
        var slftrdet = []; for (var j = 0; j < _nfieldval.length; j++) { slftrdet.push(_nfieldval[j]);  }
        var data2send = JSON.stringify({ slftrdet: slftrdet });
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "AuditLog.aspx/GetAuditLogGrid",
                data: data2send,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        if (response.d != '') {
                            var _totalcount = response.d.split('||')[0];  var DataSet = JSON.parse(response.d.split('||')[1]);
                            if (DataSet.NewDataSet != null) {
                                var Table = DataSet.NewDataSet.Table; FillAuditLogGrid(Table);

                                var _module = $('#cboModule option:selected').val();if (_module == '' || _module == undefined) { FillModules(); }
                            }
                            else { ClearGrid(); }   SetButtons(FROM, TO, _totalcount);
                        }
                        Metronic.unblockUI();
                    }
                    catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Les Auditlog :' + err.message); ClearGrid(); }
                },
                failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); ClearGrid(); },
                error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); ClearGrid(); }
            });
        }, 200);
    };

    function ClearGrid() {  $('#dataGridALog').DataTable().clear().draw();};

    function FillModules() {
        $.ajax({
            type: "POST",
            async: false,
            url: "AuditLog.aspx/FillModules",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var Dataset = JSON.parse(response.d);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table;  var _option = ''; _option = '<option> </option>';
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) { for (var i = 0; i < Table.length; i++) { if (Table[i] != null){ _option += '<option>' + Str(Table[i].MODULENAME) + '</option>';  } } }
                            else {  if (Table.MODULENAME != undefined) { _option += '<option>' + Str(Table.MODULENAME) + '</option>'; }}
                        }
                        $("#cboModule").empty().append(_option);
                    }
                }
                catch (err) { toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd"); }
            },
            failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    function FillDocType() {
        $.ajax({
            type: "POST",
            async: false,
            url: "AuditLog.aspx/FillDocType",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var Dataset = JSON.parse(response.d);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table1; var _option = ''; _option += '<option> </option>';
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) { for (var i = 0; i < Table.length; i++) { if (Table[i] != null) { _option += '<option>' + Str(Table[i].DOCTYPE) + '</option>'; } }}
                            else { if (Table.DOCTYPE != undefined) { _option += '<option>' + Str(Table.DOCTYPE) + '</option>'; }}
                        }
                        $("#cboDocType").empty().append(_option);
                    }
                }
                catch (err) { toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd");  }
            },
            failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    function FillLogType() {
        $.ajax({
            type: "POST",
            async: false,
            url: "AuditLog.aspx/FillLogType",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var Dataset = JSON.parse(response.d);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table1;   var _option = ''; _option += '<option> </option>';
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) {
                                for (var i = 0; i < Table.length; i++) {  if (Table[i] != null) { _option += '<option>' + Str(Table[i].LOGTYPE) + '</option>';  } }}
                            else { if (Table.LOGTYPE != undefined) { _option += '<option>' + Str(Table.LOGTYPE) + '</option>';    } }
                        }
                        $("#cboLogType").empty().append(_option);
                    }
                }
                catch (err) { toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd");  }
            },
            failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    var GetBuyerSupplier_data = function (SUPPLIERID, BUYERID) {
        var _byrsppdet = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "AuditLog.aspx/GetBuyerSupplier_Details",
            data: "{'SUPPLIERID':'" + SUPPLIERID + "','BUYERID':'" + BUYERID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try { _byrsppdet = Str(response.d); }
                catch (err) { toastr.error("Error", "Lighthouse eSolutions Pte. Ltd"); }
            },
            failure: function (response) { toastr.error("Failure", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error", "Lighthouse eSolutions Pte. Ltd"); }
        });
        return _byrsppdet;
    };
    
    var setFilterToolbar = function () {
        $('#divFilter').empty(); $('#toolbtngroup').empty();
        var _btns = ' <div class="row" style="margin-bottom: 2px;padding-right: 20px;"> <div class="col-md-12">' +
            '<div id="toolbtngroup" ><span title="Export to Excel" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"> <a href="javascript:;" class="toolbtn" id="btnExpExcel"><i class="fa fa-file-excel-o" style="padding-left:10px;"></i></a></div>' +
            ' <span title="Simple View" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"> <a href="javascript:;" class="toolbtn" id="btnMaxAudit"><i class="fa fa-expand" style="padding-left:10px;"></i></a></div>' +
            ' <span title="Clear" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnClearFilter"><a href="javascript:;" class="toolbtn" id="btnClear"><i class="fa fa-eraser"></i></a></div>' +
            '<span title="Refresh" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"> <a href="javascript:;" class="toolbtn" id="btnRefresh"><i class="fa fa-recycle"></i></a></div></div>' +          
           '</div></div>';
   
        $('#toolbtngroup').append(_btns);
           var _filterdet = '<div class="row"> <div class="col-md-12"><div class="dvfilterdet">  <table width="100%"><tbody>'+	
              ' <tr><td class="tdtitle">Server Name</td><td class="tdcontent td20" ><input type="text" class="form-control InputText" id="txtServerName"/></td>' +
              ' <td class="tdtitle">Doc Type</td><td class="tdcontent td20"><select class="bs-select form-control" id="cboDocType"></select></td>' +
              ' <td class="tdtitle">Module</td><td class="tdcontent td20"><select class="bs-select form-control" id="cboModule"></select></td>' +
              ' </tr><tr><td class="tdtitle">Log Type</td><td class="tdcontent td20" ><select class="bs-select form-control" id="cboLogType"></select></td>' +
              ' <td class="tdtitle">Buyer Code</td><td class="tdcontent td20"><input type="text" class="form-control InputText" id="txtBuyerCode"/></td>' +
              ' <td class="tdtitle">Supplier Code</td><td class="tdcontent td20"><input type="text" class="form-control InputText" id="txtSupplierCode"/></td>' +
              ' </tr> <tr> <td class="tdtitle">Key Ref</td><td class="tdcontent td20"><input type="text" class="form-control InputText" id="txtKeyRef"/></td>' +
              ' <td class="tdtitle">Remarks</td><td class="tdcontent td20"><input type="text" class="form-control InputText" id="txtRemarks"/></td>' +
              ' <td class="tdtitle">File Name</td><td class="tdcontent td20"><input type="text" class="form-control InputText" id="txtFileName"/></td>' +
              ' </tr> <tr> <td class="tdtitle">Log From</td><td class="tdcontent td20"><input class="form-control date-picker csDatePicker InputText" data-date-format="dd/mm/yyyy" size="16" type="text" id="dtLogFromDate" value=""/></td>' +
              ' <td class="tdtitle">Log To</td><td class="tdcontent td20"><input class="form-control date-picker csDatePicker InputText" data-date-format="dd/mm/yyyy" size="16" type="text" id="dtLogToDate" value=""/></td>' +
              ' <td class="tdtitle">Quick Search</td><td class="tdcontent td20"><input type="text" class="form-control InputText" id="txtQuickSearch"/></td>' +
              ' </tr></tbody> </table></div></div></div>';
        $('#divFilter').append(_filterdet); var date = new Date();
        $(document.getElementById('dtLogFromDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
        $(document.getElementById('dtLogToDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
        _fromlogdate = $(document.getElementById('dtLogFromDate')).val(); _tologdate = $(document.getElementById('dtLogToDate')).val();
        FillDocType(); FillLogType();
    };
   
    function ClearFilter() {
        _lstfilter = []; setFilterToolbar(); $('#lblrecdet').text(''); $('#txtFrom').val('');
        $("#btnPrev").attr('disabled', 'disabled'); $("#btnNext").attr('disabled', 'disabled'); $('#dataGridALog').DataTable().clear().draw();
    };

    var setupTableHeader = function () {
        var dtfilter = '<th></th><th>Log <br/> Date</th><th>Server <br/> Name</th><th>Module</th><th>Doc <br/> Type</th><th>Log <br/> Type</th><th>Buyer <br/> Code</th><th>Supplier <br/> Code</th><th>Key Ref</th><th style="width:100px;">Remark</th><th>File Name</th>' +
            '<th>Resubmit File</th><th>BuyerID</th><th>SupplierID</th><th>Log ID</th><th>Filename</th><th>Details</th><th>File <br/> Name 2</th><th>File <br/> Name 3</th>';
        $('#tblHeadRowALog').empty().append(dtfilter); $('#tblBodyALog').empty();
    };
   
    function DownloadFile(UPDATEDATE, MODULENAME, FILENAME, SERVERNAME) {
        try {
            $.ajax({
                type: "POST",
                async: false,
                url: "AuditLog.aspx/DownloadFile",
                data: "{ 'UPDATEDATE':'" + UPDATEDATE + "','MODULENAME':'" + MODULENAME + "','FILENAME':'" + FILENAME + "','SERVERNAME':'"+SERVERNAME+"' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d);
                        if (res != '0' && res !='') { var filefullpath = res.split('|')[0]; var filename = res.split('|')[1];
                            if (filename != undefined && filename != '') {
                                //   var cVirtualPath = "../Downloads/";var win = window.open(cVirtualPath + filename, '_blank'); 
                                 top.location.href = "../Downloads/" +filename;
                                toastr.success("Lighthouse eSolutions Pte. Ltd", "Download File success");
                            }
                        }
                        else { toastr.error("Lighthouse eSolutions Pte. Ltd", "Unable to download file. Path not found."); }
                    }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Download File :' + err); }
                },
                failure: function (response) { toastr.error("failure get", response.d); },
                error: function (response) { toastr.error("error get", response.responseText);}
            });
        }
        catch (e){ }
    };

    function ResubmitFile(LOGID) {
        try {
            $.ajax({
                type: "POST",
                async: false,
                url: "AuditLog.aspx/ReSubmitFile",
                data: "{ 'LOGID':'" + LOGID +"' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try { toastr.success("Lighthouse eSolutions Pte. Ltd", "Resubmit File success");  }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Resubmit File :' + err); }
                },
                failure: function (response) { toastr.error("failure get", response.d); },
                error: function (response) { toastr.error("error get", response.responseText); }
            });

        }
        catch (e){ }
    };

    function ShowResubmitModal(LOGID) {
        fileexist = "true"; var fileexist = CheckFileExisting(LOGID);
        if (fileexist == "false") { toastr.error("Filepath not found to resubmit file !"); }
        else {
            var txtFile = document.getElementById("txtFile");  if (txtFile != null) txtFile.Text = "";
            var lblError = document.getElementById("lblError");if (lblError != null) lblError.innerText = "";
            var lblFileName = document.getElementById("lblFileName");if (lblFileName != null) lblFileName.innerText = ""; $("#ModalResubmit").modal('show');
        }
    };

    function CheckFileExisting(LOGID) {
        var Isexist = 'false';
        try {
            $.ajax({
                type: "POST",
                async: false,
                url: "AuditLog.aspx/CheckFileExist",
                data: "{ 'LOGID':'" + LOGID + "' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try { Isexist = Str(response.d);}
                    catch (err) {}
                },
                failure: function (response) {},
                error: function (response) {}
            });
            return Isexist;
        }
        catch (e){ }
    };

    function getEncrypted_URL(strUrl) {
        var _result = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "../Common/Default.aspx/Encrypt_ServerURL",
            data: "{'ORG_URL':'" + strUrl + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try { _result = response.d; }
                catch (err) { toastr.error('Error in Getting encrypted Url :' + err, "Lighthouse eSolutions Pte. Ltd"); }
            },
            failure: function (response) { toastr.error("Failure in Getting encrypted Url", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Getting encrypted Url", "Lighthouse eSolutions Pte. Ltd"); }
        });
        return _result;
    };

    function ExportAuditLog() {
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "AuditLog.aspx/ExportExcel",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d);
                        if (res != "0") {
                            top.location.href = "../Downloads/Excel/Audit/" + res;
                            toastr.success("Lighthouse eSolutions Pte. Ltd.", 'Audit Log Report exported.');
                        }
                        Metronic.unblockUI();
                    }
                    catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to export report :' + err); }
                },
                failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
                error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
            });
        }, 200);
    };

    return { init: function () {handleAuditLogTable(); }};
}();


