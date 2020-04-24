var _servdet = []; var _filelst = []; var _deactvserv = '';

var FileMonitor = function () {

    var handleFileMonitorTable = function () {
        SetupBreadcrumb('Home', 'Home.aspx', 'Monitor', '#', 'File Monitor', 'FileMonitorSystem.aspx'); $('#pageTitle').empty().append('File Monitor');
        $(document.getElementById('lnkMonitor')).addClass('active open'); $(document.getElementById('spFilemonitr')).addClass('title font-title SelectedColor');
        setupTableHeader();

        var table = $('#dataGridFileMtrSys');
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
                "sRowSelect": "multi",
                "aButtons": ["select_all", "select_none"]
            },
            "columnDefs": [{   'orderable': false,  'searching': true,   'autoWidth': false, 'bSortable': false,   'targets': [0]
            },
            { 'targets': [0,1,4,6], width: '2px' }, { 'targets': [2], width: '40px' }, { 'targets': [3], width: '250px' },
            { 'targets': [5,7], visible: false }
            ],
            "lengthMenu": [ [25, 50, 100, -1],  [25, 50, 100, "All"]
            ],                        
            "aaSorting": [],
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                $('td:eq(0)', nRow).addClass('opentd'); var _desc = (Str(aData[5]) == '') ? 'No information to display' : Str(aData[5]);
                var _divinfo = '<div style="text-align:center;color:#1E90FF;font-size: medium;" data-trigger="hover" data-placement="left"  data-toggle="tooltip" title="' + _desc + '"><i class="fa fa-sliders" aria-hidden="true"></i></div>';
                $('td:eq(5)', nRow).html(_divinfo);
            }
        });

        $('#tblHeadRowFileMtrSys').addClass('gridHeader');  $('#ToolTables_dataGridFileMtrSys_0,#ToolTables_dataGridFileMtrSys_1,dataGridFileMtrSys_length').hide();
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%"); $('#dataGridFileMtrSys').css('width', '100%');
        setFilterToolbar(); GetServerStatus();

        $('#cbServer').live("change", function (e) { TriggerServer();});

        oTable.on('click', 'tbody td', function (e) {
            var selectedTr = $(this).parents('tr')[0]; var aData = oTable.fnGetData(selectedTr);
            if ($(this).index() == 0) {
                if (oTable.fnIsOpen(selectedTr)) {  $(this).addClass("opentd").removeClass("closetd"); oTable.fnClose(selectedTr);   }
                else {
                    if (aData != null) {  _filelst = Str(aData[7]).split('|'); var _strtable = fnFormatDetails(_filelst);
                        if (_strtable != '') { oTable.fnOpen(selectedTr, _strtable, 'details'); $(this).addClass("closetd").removeClass("opentd"); }  previousTr = selectedTr; }
                    else {
                        var table = '#FileMtrTable'; var $rows = $(table + ' tbody tr');
                        $('#txtSearch').keyup(function () { var val = $.trim($(this).val()).replace(/ +/g, ' '); $rows.hide(); $rows.filter(":contains('" + val + "')").show(); });
                    }
                }
            }
        });

        function fnFormatDetails(_filelst) {
            var sOut = '';
            var tid = "FileMtrTable"; var _theadid = "tblHeadMtr"; var _tbodyid = "tblBodyMtr";
            var maindiv = '<div class="row"><div class="col-md-12">';
            var searchdiv =maindiv+ ' <div style="float:right;width:50%;"><span>Search :</span> &nbsp;<input type="text" id="txtSearch"/></div>'
            var sOut = searchdiv + '<table class="display" style="width: 100%;" cellspacing="10" id="' + tid + '">' +
                '<thead id="' + _theadid + '"><tr><th>Time Stamp</th><th>File Name</th></thead><tbody id="' + _tbodyid + '">';
            if (_filelst != []) {
                for (var i = 0; i < _filelst.length; i++) {
                    if (_filelst[i] != '') {
                        var _det = []; sOut += '<tr id=' + i + '>';  var filename = _filelst[i].split('#')[0]; var timestmp = _filelst[i].split('#')[1];
                        sOut += '<td >' + timestmp + '</td><td>' + filename + '</td></tr>';
                    }
                }
                sOut += '</tbody></table>';
            }
            sOut += '</div></div>';
            return sOut;
        }

        $('#btnRefresh').live('click', function (e) { e.preventDefault(); GetServerStatus(); });
        $('#btnClear').live('click', function (e) { e.preventDefault(); ClearFilter(); });
    };

    var GetServerStatus = function () {
        var _sdet = [];
        $.ajax({
            type: "POST",
            async: false,
            url: "FileMonitorSystem.aspx/GetServerStatus",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    _sdet = JSON.parse(response.d);
                    if (_sdet != '' && _sdet != undefined) {
                        for (var x in _sdet) {
                            _servdet.push(_sdet[x] + "|" + x);
                            if (_sdet[x].toUpperCase() == 'DEACTIVE') { _deactvserv += Str(x) + ','; }
                        } var _servstatus = FillCombo('', _servdet); $('#cbServer').append(_servstatus);
                        $("#cbServer").prop("selectedIndex", 1); 
                    }
              
                    TriggerServer();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Servers :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d);},
            error: function (response) { toastr.error("error get", response.responseText); }
        });
    };

    function TriggerServer() {
        var _selectval = $('#cbServer option:selected').val(); var _selecttext = $('#cbServer option:selected').text(); $('#spnStatus').text(_selectval); GetServerDetails(_selecttext);
        if (_selectval.toUpperCase() == 'DEACTIVE') { $('#spnStatus').css('color', '#FF3232'); } else { $('#spnStatus').css('color', '#008000'); }
        if (_selecttext == '') { if (_deactvserv != '') { var _txt = _deactvserv.slice(0, -1); $('#Serverstatus').empty().append('<span style="font-family:x-small;">' + _txt + '</span>'); } }
    };

    var GetServerDetails = function (ServerName) {       
        $.ajax({
            type: "POST",
            async: false,
            url: "FileMonitorSystem.aspx/GetServerDetails",
            data: "{'ServerName':'"+ServerName+"'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table1;  if (Len(Table) > 0) FillGrid(Table); }
                    else $('#dataGridFileMtrSys').DataTable().clear().draw();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Servers :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
    };

    function FillGrid(Table) {
        var _timestamp = '';
        try {
            $('#dataGridFileMtrSys').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridFileMtrSys').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();                
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str('');  cells[1] = Str(Table[i].ID);  cells[2] = Str(Table[i].TITLE); cells[3] = Str(Table[i].FILE_PATH);
                    cells[4] = Str(Table[i].COUNT); cells[5] = Str(Table[i].DESC);  cells[6] = Str(''); cells[7] = Str(Table[i].FILE_LIST);_timestamp = Str(Table[0].TIME_STAMP);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.ID != undefined && Table.ID != null) {
                    var t = $('#dataGridFileMtrSys').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str('');cells[1] = Str(Table.ID);cells[2] = Str(Table.TITLE);   cells[3] = Str(Table.FILE_PATH);
                    cells[4] = Str(Table.COUNT);cells[5] = Str(Table.DESC); cells[6] = Str('');  cells[7] = Str(Table.FILE_LIST); _timestamp = Str(Table.TIME_STAMP);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
            $('#spnTimeStamp').text(_timestamp);
        }
        catch (e)
        { }
    };

    var setFilterToolbar = function () {
        $('#divFilter').empty(); $('#toolbtngroup').empty();
        var _btns = ' <div class="row" style="margin-bottom: 2px;padding-right: 20px;"> <div class="col-md-12">' +
            '<div id="toolbtngroup" ><span title="Clear" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnClearFilter"><a href="javascript:;" class="toolbtn" id="btnClear"><i class="fa fa-eraser"></i></a></div>' +
          '<span title="Refresh" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnRefresh" > <a href="javascript:;" class="toolbtn" id="btnRefresh"><i class="fa fa-recycle"></i></a></div></div>' +
           '</div></div>'; $('#toolbtngroup').append(_btns);
           var _filterdet = '<div class="row"> <div class="col-md-12"> ' +
                           '<div class="col-md-2" style="text-align:right;"><label class="dvLabel">Servers </label>  </div>' +
                           '<div class="col-md-2"><select class="bs-select form-control" id="cbServer"></select> </div>'+
                           '<div class="col-md-4"><div class="scroll-left" id="Serverstatus"></div> </div>' +
                           '</div></div><div class="row" style="margin-top:15px;"><div class="col-md-7"> ' +
                           '<div class="col-md-2" style="text-align:right;"><span style="font-weight:700;">TimeStamp </span>  </div><div class="col-md-4"><span  id="spnTimeStamp"> </span></div>' +
                           '</div><div class="col-md-1"></div><div class="col-md-4"> ' +
                           '<div class="col-md-2" style="text-align:right;"><span style="font-weight:700;">Status </span>  </div><div class="col-md-2"><span  id="spnStatus"> </span></div>' +
                           '</div></div>';
        $('#divFilter').append(_filterdet);
    };

    var setupTableHeader = function () { var dtfilter = '<th></th><th>Sr No.</th><th>Title</th><th>File Path</th><th>Count</th><th>#</th><th></th>'; $('#tblHeadRowFileMtrSys').empty(); $('#tblHeadRowFileMtrSys').append(dtfilter); $('#tblBodyFileMtrSys').empty(); };

    function ClearFilter() { setFilterToolbar(); $('#dataGridFileMtrSys').DataTable().clear().draw(); };

    return {
        init: function () { handleFileMonitorTable(); }
    };
}();