var _fromlogdate = ''; var _tologdate = ''; var duration = ''; var _module = ''; var _filepath = ''; var selectedTr = ''; var previousTr = '';
var iTableCounter = 1; var _filedetails = [];var _emptydet = [];

var LogFileViewer = function () {

    var handleLogFileViewerTable = function () {
        var nEditing = null; var nNew = false; $('#pageTitle').empty().append('Log File Viewer');
        SetupBreadcrumb('Home', 'Home.aspx', 'Viewers', '#', 'Log File Viewer', 'LogFileViewer.aspx');
        $(document.getElementById('lnkViewer')).addClass('active open'); $(document.getElementById('spLogFileView')).addClass('title font-title SelectedColor');
        setupLogDetailsHeader();
        var refreshcheck = '<label><input type="checkbox" class="icheck"  onclick="DoAutoRefresh(this.checked,this.id)" id="chkLogfileview"/> Auto Refresh Page</label>';
        $('#divChkBox').append(refreshcheck);   window.onload = SetTimerCheckBox('#chkLogfileview', 'chkLogfileview');
        var table = $('#dataGridLogFView');
        var oLFVTable = table.dataTable({
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
            "columnDefs": [{  'orderable': false,"searching": false,"autoWidth": false, 'targets': [0]
            },
            { 'targets': [0], width: '5px', 'bSortable': false }, { 'targets': [1], width: '60px'}, { 'targets': [2], width: '280px'}, { 'targets': [3, 4], visible: false }
            ],
            "aaSorting":[],
            "lengthMenu": [  [25, 50, 100, -1], [25, 50, 100, "All"] ],
            "scrollY": "300px",
            "aaSorting":[],
            "fnRowCallback": function (nRow, aData, iDisplayIndex) { $('td:eq(0)', nRow).addClass('opentd'); }
        });

        $('#tblHeadRowLogFView').addClass('gridHeader'); $('#ToolTables_dataGridLogFView_0,#ToolTables_dataGridLogFView_1,#dataGridLogFView_paginate').hide(); 
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%"); GetLogDetailsGrid();
     
        oLFVTable.on('click', 'tbody td', function (e) {
            var selectedTr = $(this).parents('tr')[0];   var aData = oLFVTable.fnGetData(selectedTr);
            if ($(this).index() == 0) {
                if (oLFVTable.fnIsOpen(selectedTr)) {  $(this).addClass("opentd").removeClass("closetd"); oLFVTable.fnClose(selectedTr);}
                else {
                    if (aData != null) { _module = Str(aData[1]);  _filepath = Str(aData[3].replace(/\\/g, '?'));  var _strtable=fnFormatDetails(oLFVTable, selectedTr);
                        if (_strtable != '') {   oLFVTable.fnOpen(selectedTr, _strtable, 'details'); $(this).addClass("closetd").removeClass("opentd");}   previousTr = selectedTr;
                    }
                    else {
                        var table = '#FileTable' + (selectedTr.rowIndex - 1);  var $rows = $(table + ' tbody tr');
                        for (var i = 0; i < $rows.length ; i++) {  var celltxt = $rows[i].cells[1].innerText;
                            if ((e.target.innerText != '') && (celltxt == e.target.innerText)) { DownloadFile(_filepath, e.target.innerText, _module); }  }
                        $('#txtSearch').keyup(function () { var val = $.trim($(this).val()).replace(/ +/g, ' '); $rows.hide(); $rows.filter(":contains('" + val + "')").show();});
                    }
                }
            }
        });

        function fnFormatDetails(oTable, nTr) {
            var sOut = ''; var _str = ''; var _filepath = '';   var indx = nTr.rowIndex; var aData = oLFVTable.fnGetData(nTr);
            var tid = "FileTable" + indx; var _theadid = "tblHeadCLogFView" + indx; var _tbodyid = "tblBodyCLogFView" + indx;
            if (Str(aData[3] != '')) { _filepath = Str(aData[3].replace(/\\/g, '?')); _filedetails = [];
                var res = GetFileDetailsGrid(_filepath, _filedetails);
                if (res == 1) {                    
                    var searchdiv = ' <div style="float:right;width:50%;"><span>Search :</span> &nbsp;<input type="text" id="txtSearch"/></div>'
                    var sOut = searchdiv + '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;" width="50%" id="' + tid + '">' +
                        '<thead id="' + _theadid + '"><tr><th>Time Stamp</th><th>File Name</th><th style="display:none;">LOG_FILE_ID</th></tr></thead><tbody id="' + _tbodyid + '">';
                    for (var i = 0; i < _filedetails.length; i++) {
                        var _det = [];
                        _str = _filedetails[i].split('#')[1]; _det = _str.split(','); sOut += '<tr id=' + i + '>';
                        for (var j = 0; j < _det.length; j++) {
                            if (j == 1) { sOut += '<td><a href="#"><u>' + _det[j] + '</u></a></td>'; }
                            else if (j == 2) { sOut += '<td style="display:none;">' + _det[j] + '</td>'; }
                            else { sOut += '<td>' + _det[j] + '</td>'; }
                        }
                        sOut += '</tr>';
                    } sOut += '</tbody></table>';
                }              
            }
            return sOut;
        }
    };

    function FillLogDetailsGrid(Table) {
        try
        {
            $('#dataGridLogFView').DataTable().clear().draw(); var t = $('#dataGridLogFView').dataTable();
            if (Table.length != undefined && Table.length != null)
            {
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str(''); cells[1] = Str(Table[i].MODULE);  cells[2] = Str(Table[i].DESCRIPTION); cells[3] = Str(Table[i].PATH);cells[4] = Int(Table[i].LOG_ID);
                    var ai = t.fnAddData(cells, false);
                } t.fnDraw();
            }
            else
            {
                if (Table.LOG_ID != undefined && Table.LOG_ID != null) {
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str('');  cells[1] = Str(Table.MODULE);cells[2] = Str(Table.DESCRIPTION); cells[3] = Str(Table.PATH); cells[4] = Int(Table.LOG_ID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    var GetLogDetailsGrid = function () {
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "LogFileViewer.aspx/GetLogFileDetailsGrid",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var DataSet = JSON.parse(response.d);
                        if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table1;  FillLogDetailsGrid(Table);}
                        else $('#dataGridLogFView').DataTable().clear().draw();
                        Metronic.unblockUI();
                    }
                    catch (err) {  Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Log File Viewer :' + err); }
                },
                failure: function (response) {toastr.error("failure get", response.d); Metronic.unblockUI(); },
                error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
            });
        }, 200);
    };

    var GetFileDetailsGrid = function (PATH, _filedetails) {
        var _result='';
        $.ajax({
            type: "POST",
            async: false,
            url: "LogFileViewer.aspx/GetFileDetails",
            data: "{'PATH':'" + PATH + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var str = 'Path';
                    if (Str(response.d).indexOf(str) == -1) {
                        var DataSet = JSON.parse(response.d);
                        if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table1; FillFileDetails(Table, _filedetails); _result = '1'; }
                    }
                    else { _result = ''; alert(response.d); }
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Log File Details :' + err);   }
            },
            failure: function (response) {  toastr.error("failure get", response.d);    },
            error: function (response) { toastr.error("error get", response.responseText); }          
        });
        return _result;
    };

    function FillFileDetails(Table, _filedetails) {
        var _filedet = [];
        try {         
            if (Table.length != undefined && Table.length != null) {              
                for (var i = 0; i < Table.length; i++) {
                    _filedet = []; _filedet.push(Str(Table[i].TIME_STAMP)); _filedet.push(Str(Table[i].FILENAME));_filedet.push(Str(Table[i].LOG_FILE_ID));
                    _filedetails.push("FDET" + i + "#" + _filedet);
                }
            }
            else if (Table.LOG_FILE_ID != undefined && Table.LOG_FILE_ID != null) {             
                _filedet.push(Str(Table.TIME_STAMP)); _filedet.push(Str(Table.FILENAME)); _filedet.push(Str(Table.LOG_FILE_ID));
                _filedetails.push("FDET" + i + "#" + _filedet);
            }
        }
        catch (e)
        { }
    };

    function DownloadFile(PATH, FILENAME, MODULE) {
        try { 
            $.ajax({
                type: "POST",
                async: false,
                url: "LogFileViewer.aspx/DownloadFile",
                data: "{ 'PATH':'" + PATH + "','FILENAME':'" + FILENAME + "','MODULE':'" + MODULE + "' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d);
                        if (res != "") {   var filefullpath = res.split('|')[0];  var filename = res.split('|')[1];// var   cVirtualPath = "../Downloads/"; var win = window.open(cVirtualPath + filename, '_blank'); win.focus();
                            top.location.href = "../Downloads/" + filename;
                            toastr.success("Lighthouse eSolutions Pte. Ltd", "Download File success");
                        }
                    }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Download File :' + err); }
                },
                failure: function (response) {  toastr.error("failure get", response.d);  },
                error: function (response) { toastr.error("error get", response.responseText);  }
            });
        }
        catch (e)
        { }
    };

    var setupLogDetailsHeader = function () {
        $('#toolbtngroup').empty();
        var dtfilter = '<th style="text-align:center;" id="opnclse"></th><th id="txtModule">Module</th><th>Description</th><th>Path</th><th>LOG_ID</th>';
        $('#tblHeadRowLogFView').empty().append(dtfilter); $('#tblBodyLogFView').empty();
    };

     return {
         init: function () { handleLogFileViewerTable(); }
    };
}();