var _fromlogdate = '';var _tologdate = '';

var LoginHistory = function () {

    var handleLoginHistoryTable = function () {
        CommonSettings();  var table = $('#dataGridLogHist');
        var oTable = table.dataTable({
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
            "columnDefs": [{ 'orderable': false, "searching": true,"autoWidth": false,  'targets': [0]},
             { 'targets': [1,2], width: '30px' }, { 'targets': [3], width: '120px' }, { 'targets': [4,5,6,7], width: '65px' },  { 'targets': [8], width: '80px' },
             {'targets': [6, 7, 8], "sClass": "visible-lg" },{'targets': [0, 9], visible: false }
            ],
            "lengthMenu": [ [25, 50, 100, -1], [25, 50, 100, "All"] ],
            "scrollY":  "300px",
            "sScrollX": '900px',
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
        });
        $('#tblHeadRowLogHist').addClass('gridHeader'); $('#ToolTables_dataGridLogHist_0,#ToolTables_dataGridLogHist_1').hide(); 
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
        setTimeout(function () { oTable.fnAdjustColumnSizing(); }, 10);  setFilterToolbar(); GetLoginHistoryGrids(_fromlogdate, _tologdate);
        $('#btnApply').on('click', function (e) { e.preventDefault();
            _fromlogdate = $(document.getElementById('dtUpdateFromDate')).val();  _tologdate = $(document.getElementById('dtUpdateToDate')).val();
            GetLoginHistoryGrids(_fromlogdate, _tologdate);
        });
        $('#dtUpdateFromDate').datepicker().on('changeDate', function (ev) {  $('#dtUpdateFromDate').datepicker('hide');});
        $('#dtUpdateToDate').datepicker().on('changeDate', function (ev) { $('#dtUpdateToDate').datepicker('hide'); });
        $('#divbtnRefresh').on('click', function (e) {
            e.preventDefault();_fromlogdate = $(document.getElementById('dtUpdateFromDate')).val(); _tologdate = $(document.getElementById('dtUpdateToDate')).val();
            GetLoginHistoryGrids(_fromlogdate, _tologdate);
        });
        $('#divbtnClearFilter').on('click', function (e) {  e.preventDefault();ClearFilter();});
    };

    function CommonSettings() {
        $('#pageTitle').empty().append('Login History'); SetupBreadcrumb('Home', 'Home.aspx', 'Login History', 'LoginHistory.aspx', '', '');
        $(document.getElementById('lnkLoginHistory')).addClass('active'); setupTableHeader();
        var refreshcheck = '<label><input type="checkbox" class="icheck"  onclick="DoAutoRefresh(this.checked,this.id)" id="chkLoginHist"/> Auto Refresh Page</label>';
        $('#divChkBox').append(refreshcheck); window.onload = SetTimerCheckBox('#chkLoginHist', 'chkLoginHist');
    };

    function FillLoginHistoryGrid(Table) {
        try
        {
            $('#dataGridLogHist').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null){ var t = $('#dataGridLogHist').dataTable();
                for (var i = 0; i < Table.length; i++)
                {
                    var cells = new Array(); var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str(''); cells[1] = Str(Table[i].UPDATED_DATE); cells[2] = Str(Table[i].EX_USERCODE);  cells[3] = Str(Table[i].CLIENT_SERVER_IP);
                    cells[4] = Str(Table[i].LOGGED_IN); cells[5] = Str(Table[i].LOGGED_IN_REMARKS);  cells[6] = Str(Table[i].LOGGED_OUT);
                    cells[7] = Str(Table[i].LOGGED_OUT_REMARKS);  cells[8] = Str(Table[i].SESSIONID); cells[9] = Str(Table[i].LOGIN_TRACK_ID);                  
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else
            {
                if (Table.LOGIN_TRACK_ID != undefined && Table.LOGIN_TRACK_ID != null)  {
                    var t = $('#dataGridLogHist').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');var cells = new Array();
                    cells[0] = Str('');   cells[1] = Str(Table.UPDATED_DATE); cells[2] = Str(Table.EX_USERCODE); cells[3] = Str(Table.CLIENT_SERVER_IP);
                    cells[4] = Str(Table.LOGGED_IN);cells[5] = Str(Table.LOGGED_IN_REMARKS);  cells[6] = Str(Table.LOGGED_OUT); cells[7] = Str(Table.LOGGED_OUT_REMARKS);
                    cells[8] = Str(Table.SESSIONID);cells[9] = Str(Table.LOGIN_TRACK_ID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)  { }
    };

    var GetLoginHistoryGrids = function (_fromlogdate, _tologdate) {
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "LoginHistory.aspx/GetLoginHistoryGrid",
                data: "{ 'UPDATE_DATEFROM':'" + getSQLDateFormated(_fromlogdate) + "','UPDATE_DATETO':'" + getSQLDateFormated(_tologdate) + "' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var DataSet = JSON.parse(response.d); if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table; FillLoginHistoryGrid(Table); }
                        else $('#dataGridLogHist').DataTable().clear().draw();
                        Metronic.unblockUI();
                    }
                    catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Login History :' + err); }
                },
                failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
                error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
            });
        }, 150);
    };

    var setFilterToolbar = function () {
        $('#divFilter').empty(); $('#toolbtngroup').empty();
        var _btns = ' <div class="row" style="margin-bottom: 2px;padding-right: 20px;"> <div class="col-md-12">' +
            '<div id="toolbtngroup" ><span title="Clear" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnClearFilter"><a href="javascript:;" class="toolbtn" id="btnClear"><i class="fa fa-eraser"></i></a></div>' +
          '<span title="Refresh" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnRefresh" > <a href="javascript:;" class="toolbtn" id="btnRefresh"><i class="fa fa-recycle"></i></a></div></div>' +
           '</div></div>'; $('#toolbtngroup').append(_btns);
        var _filterdet =   ' <div class="row"> <div class="col-md-12"> <div class="col-md-4" style="text-align:right;">' +
            ' <div class="col-md-4"><label class="control-label">Update From </label>  </div>' +
            ' <div class="col-md-8"> <input class="form-control date-picker csDatePicker InputText" data-date-format="dd/mm/yyyy" size="16" type="text" id="dtUpdateFromDate" value=""/> </div>' +
            ' </div> <div class="col-md-4" style="text-align:right;">' +
             ' <div class="col-md-4"><label class="control-label" style="text-align:right;">Update To </label>  </div>' +
             ' <div class="col-md-8"> <input class="form-control date-picker csDatePicker InputText" data-date-format="dd/mm/yyyy" size="16" type="text" id="dtUpdateToDate" value=""/> </div>' +
             ' </div></div></div>';
        $('#divFilter').append(_filterdet); var date = new Date();
        $(document.getElementById('dtUpdateFromDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
        $(document.getElementById('dtUpdateToDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
        _fromlogdate = $(document.getElementById('dtUpdateFromDate')).val(); _tologdate = $(document.getElementById('dtUpdateToDate')).val();    
    };

    function ClearFilter() { _lstfilter = []; setFilterToolbar(); $('#dataGridLogHist').DataTable().clear().draw(); };

    var setupTableHeader = function () {
        var dtfilter = '<th id="cbLogHistheader"></th><th style="text-align:center;">#</th><th> User</th><th>Client IP Address</th><th>Logged In Time</th><th>Logged In Log</th><th>Logged Out Time</th><th>Logged Out Log</th><th>Session Id</th><th>LOGIN_TRACK_ID</th>';
        $('#tblHeadRowLogHist').empty(); $('#tblHeadRowLogHist').append(dtfilter); $('#tblBodyLogHist').empty();
    };
   
     return {
        init: function () { handleLoginHistoryTable(); }
    };
}();