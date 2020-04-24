var _fromlogdate = ''; var _tologdate = '';var _selectftr = '';var _ddlselectfiltr = '';var _searchfilter = '';

var FileAudit = function () {

    var handleFileAuditable = function () { $('#pageTitle').empty().append('File Audit');
        SetupBreadcrumb('Home', 'Home.aspx', 'Audit', '#', 'File Audit', 'FileAudit.aspx');
        $(document.getElementById('lnkAuditDet')).addClass('active open');  $(document.getElementById('spFileAud')).addClass('title font-title SelectedColor');
        var refreshcheck = '<label><input type="checkbox" class="icheck"  onclick="DoAutoRefresh(this.checked,this.id)" id="chkFileAudit"/> Auto Refresh Page</label>';
        $('#divChkBox').append(refreshcheck);  window.onload = SetTimerCheckBox('#chkFileAudit', 'chkFileAudit');

        setupFileAuditTableHeader(); setFilterToolbar();
        var table = $('#dataGridFAud');
        var oFAudTable = table.dataTable({
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
            "columnDefs": [{  'orderable': true,"searching": true,  "autoWidth": false, 'targets': [0]  },
           {'targets': [2,3,4], width: '45px', "sClass": "visible-lg"},  {'targets': [0], width: '45px'},  {'targets': [1],   width: '150px'},
           {'targets': [5,6,7,8,9,10,11,12,18,19,20,21,22,23,24,25,26], visible: false}, {'targets': [13,14,15,16],  'bSortable': false}
             ],
            "lengthMenu": [  [25, 50, 100, -1],    [25, 50, 100, "All"] ],
            "scrollY": '300px',
            "sScrollX": true,
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var noprocessTag = '<img src=\"../Content/themes/base/images/NoProcess.png\" title="NA" \>';   var detTag = '<a id="detid" href="#"><u>Details..</u></a>';
                $('td:eq(2),td:eq(3),td:eq(4),td:eq(5),td:eq(6),td:eq(7),td:eq(8),td:eq(9)', nRow).addClass('dt-center');           
                $('td:eq(5),td:eq(6),td:eq(7),td:eq(8)', nRow).html(noprocessTag);  $('td:eq(9)', nRow).html(detTag);  SetImagedetToRow(nRow, aData);
            }
        });

        $('#tblHeadRowFAud').addClass('gridHeader'); $('#ToolTables_dataGridFAud_0,#ToolTables_dataGridFAud_1').hide();
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");

        $('#cbselect').on("change", function (e) { _ddlselectfiltr = $('#cbselect option:selected').val(); });
        $('#divbtnRefresh').on('click', function (e) {e.preventDefault();
            _fromlogdate = $(document.getElementById('dtUpdateFromDate')).val(); _tologdate = $(document.getElementById('dtUpdateToDate')).val();_searchfilter = $(document.getElementById('txtSearch')).val();
            GetFileAuditGrid($('#cbselect option:selected').val(), _searchfilter);  });
        $('#divbtnClearFilter').on('click', function (e) { e.preventDefault();   ClearFilter(); });
        $('#dtUpdateFromDate').datepicker().on('changeDate', function (ev) { $('#dtUpdateFromDate').datepicker('hide');});
        $('#dtUpdateToDate').datepicker().on('changeDate', function (ev) {  $('#dtUpdateToDate').datepicker('hide');  });

        oFAudTable.on('click', ' tbody td', function (e) { nTr = $(this).parents('tr')[0]; var aPos = oFAudTable.fnGetPosition(this);  var aData = oFAudTable.fnGetData(aPos[0]);
            var colIndx = $(this).index(); if (colIndx == 9) {GetFileAuditdetails(aData); }  });
    };

    var GetFileAuditGrid = function (SELECTFILTER, _searchfilter) {
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "FileAudit.aspx/GetFileAuditGrid",
                data: "{ 'SELECTFILTER':'" + SELECTFILTER + "','LOG_DATEFROM':'" + getSQLDateFormated(_fromlogdate) + "','LOG_DATETO':'" + getSQLDateFormated(_tologdate) + "','SEARCH':'" + Str(_searchfilter) + "' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var DataSet = JSON.parse(response.d);   if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table;  FillFileAuditGrid(Table);}
                        else $('#dataGridFAud').DataTable().clear().draw();
                        Metronic.unblockUI();
                    }
                    catch (err) {  Metronic.unblockUI();  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get File Audit :' + err); }
                },
                failure: function (response) {  Metronic.unblockUI();  toastr.error("failure get", response.d);   },
                error: function (response) {   Metronic.unblockUI(); toastr.error("error get", response.responseText); }
            });
        }, 200);
    };

    function FillFileAuditGrid(Table) {
        try {
            $('#dataGridFAud').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridFAud').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str(Table[i].UPDATEDATE);cells[1] = Str(Table[i].VRNO);  cells[2] = Str(Table[i].VERSION);   cells[3] = Str(Table[i].BUYER_CODE);   cells[4] = Str(Table[i].SUPPLIER_CODE);
                    cells[5] = Str(Table[i].RFQ_IMP);   cells[6] = Str(Table[i].RFQ_EXP); cells[7] = Str(Table[i].QUOTE_IMP); cells[8] = Str(Table[i].QUOTE_EXP);
                    cells[9] = Str(Table[i].PO_IMP);  cells[10] = Str(Table[i].PO_EXP); cells[11] = Str(Table[i].POC_IMP);  cells[12] = Str(Table[i].POC_EXP); cells[13] = Str(Table[i].RFQ);
                    cells[14] = Str(Table[i].QUOTE); cells[15] = Str(Table[i].PO); cells[16] = Str(Table[i].POC);   cells[17] = Str('');
                    cells[18] = Str(Table[i].RFQ_END_STATE); cells[19] = Str(Table[i].RFQ_UPLOAD);cells[20] = Str(Table[i].QUOTE_END_STATE); cells[21] = Str(Table[i].QUOTE_UPLOAD);  cells[22] = Str(Table[i].PO_END_STATE);
                    cells[23] = Str(Table[i].PO_UPLOAD);  cells[24] = Str(Table[i].POC_END_STATE);   cells[25] = Str(Table[i].POC_UPLOAD); cells[26] = Str(Table[i].RECORDID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.RECORDID != undefined && Table.RECORDID != null) {
                    var t = $('#dataGridFAud').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str(Table.UPDATEDATE); cells[1] = Str(Table.VRNO);  cells[2] = Str(Table.VERSION);   cells[3] = Str(Table.BUYER_CODE);
                    cells[4] = Str(Table.SUPPLIER_CODE); cells[5] = Str(Table.RFQ_IMP); cells[6] = Str(Table.RFQ_EXP);
                    cells[7] = Str(Table.QUOTE_IMP);  cells[8] = Str(Table.QUOTE_EXP);  cells[9] = Str(Table.PO_IMP);
                    cells[10] = Str(Table.PO_EXP);  cells[11] = Str(Table.POC_IMP); cells[12] = Str(Table.POC_EXP);   cells[13] = Str(Table.RFQ);
                    cells[14] = Str(Table.QUOTE);   cells[15] = Str(Table.PO);  cells[16] = Str(Table.POC);  cells[17] = Str('');
                    cells[18] = Str(Table.RFQ_END_STATE);  cells[19] = Str(Table.RFQ_UPLOAD); cells[20] = Str(Table.QUOTE_END_STATE);   cells[21] = Str(Table.QUOTE_UPLOAD);  cells[22] = Str(Table.PO_END_STATE);
                    cells[23] = Str(Table.PO_UPLOAD);   cells[24] = Str(Table.POC_END_STATE);     cells[25] = Str(Table.POC_UPLOAD);  cells[26] = Str(Table.RECORDID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    var setupFileAuditTableHeader = function () {
        var dtHeader = '<th class="gridHeader" id="txtCode" style="font-size:12px;">Log Date</th> <th style="font-size:12px;">Reference No.</th><th style="font-size:12px;">Version</th><th style="font-size:12px;">Buyer</th> <th style="font-size:12px;">Supplier</th>'+
        ' <th style="font-size:12px;">RFQ_IMP</th><th style="font-size:12px;">RFQ_EXP</th><th style="font-size:12px;">QUOTE_IMP</th><th style="font-size:12px;">QUOTE_EXP</th>'+
        ' <th style="font-size:12px;">PO_IMP</th> <th style="font-size:12px;">PO_EXP</th><th style="font-size:12px;">POC_IMP</th><th style="font-size:12px;">POC_EXP</th>' +
        ' <th style="font-size:12px;">RFQ</th> <th style="font-size:12px;">QUOTE</th><th style="font-size:12px;">PO</th> <th style="font-size:12px;">POC</th><th style="font-size:12px;">Details</th>' +
        ' <th style="font-size:12px;">RFQ_END_STATE</th><th style="font-size:12px;">RFQ_UPLOAD</th><th style="font-size:12px;">QUOTE_END_STATE</th> <th style="font-size:12px;">QUOTE_UPLOAD</th> <th style="font-size:12px;">PO_END_STATE</th> ' +
       ' <th style="font-size:12px;">PO_UPLOAD</th> <th style="font-size:12px;">POC_END_STATE</th> <th style="font-size:12px;">POC_UPLOAD</th>  <th style="font-size:12px;">RecordID</th>';
        $('#tblHeadRowFAud').empty();$('#tblHeadRowFAud').append(dtHeader);$('#tblBodyFAud').empty();
    };

    function SetImagedetToRow(nRow,aData) {
        var tdcol=''; var doneTag = '<img src=\"../Content/themes/base/images/Done.png\" title="Completed" \>';
        var _rfqImp = aData[5]; var _rfqExp = aData[6]; var _rfq = aData[13];   var _rfqEndstate = aData[18];   var _rfqUpload = aData[19]; 
        var _quoteImp = aData[7];  var _quoteExp = aData[8]; var _quote = aData[14]; var _quoteEndstate = aData[20];  var _quoteUpload = aData[21];
        var _poImp = aData[9]; var _poExp = aData[10];  var _po = aData[15]; var _poEndstate = aData[22]; var _poUpload = aData[23];
        var _pocImp = aData[11]; var _pocExp = aData[12]; var _poc = aData[16];  var _pocEndstate = aData[24];   var _pocUpload = aData[25];
        if (_rfq == "1")  { tdcol='td:eq(5)';
            if (_rfqImp != "" && _rfqExp == "" && _rfqEndstate >= 3) {   SetImage(_rfqImp, "Waiting for exporting RFQ from eSupplier", "RFQ is not exported from eSupplier !", tdcol, nRow); }
            else if (_rfqExp != "" && _rfqUpload == "" && _rfqEndstate >= 4) {   SetImage(_rfqExp, "Waiting for sending RFQ to supplier", "RFQ is not sent to supplier !", tdcol, nRow); }
            else if (_rfqImp != "") { if (_rfqEndstate == 2) { $(tdcol, nRow).html(doneTag); }   else if (_rfqExp != "" && _rfqEndstate > 2) { $(tdcol, nRow).html(doneTag); } }
        }
        if (_quote == 1) {  tdcol = 'td:eq(6)';
            if (_quoteImp != "" && _quoteExp == "" && _quoteEndstate >= 3) { SetImage(_quoteImp, "Waiting for exporting quote from eSupplier", "Quote is not exported from eSupplier !", tdcol, nRow); }
            else if (_quoteExp != "" && _quoteUpload == "" && _quoteEndstate >= 4)  { SetImage(_quoteExp, "Waiting for sending quote to buyer", "Quote is not sent to buyer !", tdcol, nRow); }
            else if (_quoteImp != "" && _quoteEndstate >= 2) { $(tdcol, nRow).html(doneTag);  }
        }
        if ( _po == 1)  {  tdcol = 'td:eq(7)';
            if (_poImp != "" && _poExp == "" && _poEndstate >= 3)  { SetImage(_poImp, "Waiting for exporting PO from eSupplier", "PO is not exported from eSupplier !", tdcol, nRow); }
            else if (_poExp != "" && _poUpload == "" && _poEndstate >= 4) { SetImage(_poExp, "Waiting for sending PO to supplier", "PO is not sent to supplier !", tdcol, nRow); }
            else if (_poImp != "") { if (_poEndstate == 2)  { $(tdcol, nRow).html(doneTag);    } else if (_poExp != "" && _poEndstate >= 2)  { $(tdcol, nRow).html(doneTag);     } }
        }
        if (_poc == 1) { tdcol = 'td:eq(8)';
            if (_pocImp != "" && _pocExp == "" && _pocEndstate >= 3) { SetImage(_pocImp, "Waiting for exporting POC from eSupplier", "POC is not exported from eSupplier !", tdcol, nRow); }
            else if (_pocExp != "" && _pocUpload == "" && _pocEndstate >= 4) { SetImage(_pocExp, "Waiting for sending POC to buyer", "POC is not sent to buyer !", tdcol, nRow); }
            else if (_pocImp != "" && _pocEndstate >= 2)  {   $(tdcol, nRow).html(doneTag); }
        }
    };

    function SetImage(FromTime, WaitingMsg, ErrorMsg, tdcol, nRow) {
        var dt = new Date(); var dtdate = new Date(FromTime); dtdate.setMinutes(dtdate.getMinutes() + 30); 
       if (dt < dtdate) {   var waitingTag = '<img src=\"../Content/themes/base/images/Waiting.png\" title="' + WaitingMsg + '" \>';   $(tdcol, nRow).html(waitingTag);   }
       else {  var errorTag = '<img src=\"../Content/themes/base/images/Error.png\" title="' + ErrorMsg + '" \>'; $(tdcol, nRow).html(errorTag); }
    };

    function GetFileAuditdetails(aData) {
        ClearFAuditDetails();   var vrno = "'" + aData[1] + "'";  var RECORDID = aData[26];   $('#captionid').text(vrno + ' process...');
        $.ajax({
            type: "POST",
            async: false,
            url: "FileAudit.aspx/ViewDetailedInfo",
            data: "{ 'RECORDID':'" + RECORDID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);if (DataSet.NewDataSet != null) {   Table = DataSet.NewDataSet.Table;   SetFAuditDetails(Table);  }
                }
                catch (err) {  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get File Audit Details :' + err); }
            },
            failure: function (response) {  toastr.error("failure get", response.d);},
            error: function (response) {   toastr.error("error get", response.responseText);  }
        });
    };

    function ClearFAuditDetails() {
        $('#captionid').text('');
        $('#RFQrcvdByer').text('');  $('#RFQimp').text('');  $('#RFQexp').text('');  $('#RFQsentsupp').text(''); $('#RFQmailsent').text('');
        $('#QuotercvdSupp').text(''); $('#Quoteimp').text(''); $('#Quoteexp').text('');  $('#QuotesentByer').text('');$('#Quotemailsent').text('');
        $('#POrcvdByer').text('');  $('#POimp').text('');  $('#POexp').text(''); $('#POsentsupp').text('');$('#POmailsent').text('');
        $('#POCrcvdSupp').text('');  $('#POCimp').text(''); $('#POCexp').text('');$('#POCsentByer').text(''); $('#POCmailsent').text('');
    };

    function SetFAuditDetails(Table) {
        var _noval = 'NA'; var _formatstring = 'dd mmm yyyy HH:MM:ss'; var _empty = Str('');   var _rfq = ''; var _quote = ''; var _po = ''; var _poc = '';
        if (Table.RECORDID != undefined && Table.RECORDID != null) {
            if (Str(Table.RFQ) == "1") {
                //var _rfqdownload = (Str(Table.RFQ_DOWNLOAD) != "") ? getSQLDate_fomat(Table.RFQ_DOWNLOAD, _formatstring) : _empty;
                //var _rfqimp = (Str(Table.RFQ_IMP) != "") ? getSQLDate_fomat(Table.RFQ_IMP, _formatstring) : _empty;
                //var _rfqexp = (Str(Table.RFQ_EXP) != "") ? getSQLDate_fomat(Table.RFQ_EXP, _formatstring) : _empty;
                //var _rfqupload = (Str(Table.RFQ_UPLOAD) != "") ? getSQLDate_fomat(Table.RFQ_UPLOAD, _formatstring) : _empty;
                //var _rfqmailsent = (Str(Table.RFQ_MAIL_SENT) != "") ? getSQLDate_fomat(Table.RFQ_MAIL_SENT, _formatstring) : _empty;
                var _rfqdownload = (Str(Table.RFQ_DOWNLOAD) != "") ? Str(Table.RFQ_DOWNLOAD) : _empty;
                var _rfqimp = (Str(Table.RFQ_IMP) != "") ? Str(Table.RFQ_IMP) : _empty;
                var _rfqexp = (Str(Table.RFQ_EXP) != "") ? Str(Table.RFQ_EXP) : _empty;
                var _rfqupload = (Str(Table.RFQ_UPLOAD) != "") ? Str(Table.RFQ_UPLOAD) : _empty;
                var _rfqmailsent = (Str(Table.RFQ_MAIL_SENT) != "") ? Str(Table.RFQ_MAIL_SENT) : _empty;
                $('#RFQrcvdByer').text(_rfqdownload);$('#RFQimp').text(_rfqimp);   $('#RFQexp').text(_rfqexp);  $('#RFQsentsupp').text(_rfqupload); $('#RFQmailsent').text(_rfqmailsent);
            }
            else { $('#RFQrcvdByer').text(_noval); $('#RFQimp').text(_noval); $('#RFQexp').text(_noval); $('#RFQsentsupp').text(_noval); $('#RFQmailsent').text(_noval);}

            if (Str(Table.QUOTE) == "1") {
                var _quotedownload = (Str(Table.QUOTE_DOWNLOAD) != "") ? Str(Table.QUOTE_DOWNLOAD) : _empty;
                var _quoteimp = (Str(Table.QUOTE_IMP) != "") ? Str(Table.QUOTE_IMP) : _empty;
                var _quoteexp = (Str(Table.QUOTE_EXP) != "") ? Str(Table.QUOTE_EXP) : _empty;
                var _quoteupload = (Str(Table.QUOTE_UPLOAD) != "") ? Str(Table.QUOTE_UPLOAD) : _empty;
                var _quotemailsent = (Str(Table.QUOTE_MAIL_SENT) != "") ? Str(Table.QUOTE_MAIL_SENT) : _empty;
                $('#QuotercvdSupp').text(_quotedownload); $('#Quoteimp').text(_quoteimp); $('#Quoteexp').text(_quoteexp); $('#QuotesentByer').text(_quoteupload);  $('#Quotemailsent').text(_quotemailsent);
            }
            else { $('#QuotercvdSupp').text(_noval); $('#Quoteimp').text(_noval); $('#Quoteexp').text(_noval); $('#QuotesentByer').text(_noval); $('#Quotemailsent').text(_noval); }

            if (Str(Table.PO) == "1") {
                var _podownload = (Str(Table.PO_DOWNLOAD) != "") ? Str(Table.PO_DOWNLOAD) : _empty;
                var _poimp = (Str(Table.PO_IMP) != "") ? Str(Table.PO_IMP) : _empty;
                var _poexp = (Str(Table.PO_EXP) != "") ? Str(Table.PO_EXP) : _empty;
                var _poupload = (Str(Table.PO_UPLOAD) != "") ? Str(Table.PO_UPLOAD) : _empty;
                var _pomailsent = (Str(Table.PO_MAIL_SENT) != "") ? Str(Table.PO_MAIL_SENT) : _empty;
                $('#POrcvdByer').text(_podownload);  $('#POimp').text(_poimp);  $('#POexp').text(_poexp);  $('#POsentsupp').text(_poupload); $('#POmailsent').text(_pomailsent);
            }
            else {  $('#POrcvdByer').text(_noval); $('#POimp').text(_noval); $('#POexp').text(_noval); $('#POsentsupp').text(_noval); $('#POmailsent').text(_noval);  }

            if (Str(Table.POC) == "1") {
                var _pocdownload = (Str(Table.POC_DOWNLOAD) != "") ? Str(Table.POC_DOWNLOAD) : _empty;
                var _pocimp = (Str(Table.POC_IMP) != "") ? Str(Table.POC_IMP) : _empty;
                var _pocexp = (Str(Table.POC_EXP) != "") ? Str(Table.POC_EXP) : _empty;
                var _pocupload = (Str(Table.POC_UPLOAD) != "") ? Str(Table.POC_UPLOAD) : _empty;
                var _pocmailsent = (Str(Table.POC_MAIL_SENT) != "") ? Str(Table.POC_MAIL_SENT) : _empty;
                $('#POCrcvdSupp').text(_pocdownload);  $('#POCimp').text(_pocimp); $('#POCexp').text(_pocexp); $('#POCsentByer').text(_pocupload);   $('#POCmailsent').text(_pocmailsent);
            }
            else { $('#POCrcvdSupp').text(_noval); $('#POCimp').text(_noval); $('#POCexp').text(_noval); $('#POCsentByer').text(_noval); $('#POCmailsent').text(_noval);}
        }
        $("#ModalDetails").modal('show');
    };

    var setFilterToolbar = function () {
        $('#divFilter').empty(); $('#toolbtngroup').empty();
        var _btns = ' <div class="row" style="margin-bottom: 2px;padding-right: 20px;"> <div class="col-md-12">' +
            '<div id="toolbtngroup" ><span title="Clear" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnClearFilter"><a href="javascript:;" class="toolbtn" id="btnClear"><i class="fa fa-eraser"></i></a></div>' +
          '<span title="Refresh" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnRefresh" > <a href="javascript:;" class="toolbtn" id="btnRefresh"><i class="fa fa-recycle"></i></a></div></div>' +
           '</div></div>'; $('#toolbtngroup').append(_btns);
           var _filterdet = '<div class="row"> <div class="col-md-12"> <div class="dvfilterdet">  <table width="100%"><tbody>'+	             
                 ' <tr><td class="tdhd headmenu-label">Log Type</td><td class="tdcon1 headmenu-label"><select class="bs-select form-control" id="cbselect"> <option value="0" selected="selected">Please select any value..</option>    <option value="1">RFQ not Exported</option>  <option value="2">RFQ not Sent to Supplier</option> <option value="3">Quote not Exported</option><option value="4">Quote not Send to Buyer</option> <option value="5">PO not Exported</option><option value="6">PO not Send to Supplier</option> <option value="7">POC not Exported</option>  <option value="8">POC not Send to Buyer</option></select> </td>' +
                 ' </tr><tr><td class="tdhd headmenu-label">Update From</td><td class="tdcon1 headmenu-label"><input class="form-control date-picker csDatePicker InputText" data-date-format="dd/mm/yyyy" size="16" type="text" id="dtUpdateFromDate" value=""/></td>' +
                 ' <td class="tdhd headmenu-label">Update To</td><td class="tdcon1 headmenu-label"><input class="form-control date-picker csDatePicker InputText" data-date-format="dd/mm/yyyy" size="16" type="text" id="dtUpdateToDate" value=""/></td>' +
                 ' </tr></tbody> </table></div> </div></div>';
        $('#divFilter').append(_filterdet); var date = new Date();
        $(document.getElementById('dtUpdateFromDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
        $(document.getElementById('dtUpdateToDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
        _fromlogdate = $(document.getElementById('dtUpdateFromDate')).val();  _tologdate = $(document.getElementById('dtUpdateToDate')).val();
    };

    function ClearFilter() {  _lstfilter = []; setFilterToolbar();  $('#dataGridFAud').DataTable().clear().draw();};

     return {
         init: function () { handleFileAuditable(); }
    };
}();