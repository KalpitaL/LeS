var eRow = null; var _res = ''; var _ckRes = ''; var recid = -1; var nMode = 0; var list = []; var count = 0; var _selectAddrCode = '';
var Adaptors = function () {
    var handleAdaptorTable = function () {
        SetupBreadcrumb('Home', 'Home.aspx', 'Adaptor Status', 'Adaptors.aspx', '', ''); $('#modheader').hide();
        $(document.getElementById('lnkAdaptors')).addClass('active');
        setupTableHeader();setupServicesTableHeader(); setupSchedulerTableHeader();
        var table = $('#dataGridAdpt');
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
                "sRowSelect": "select",
                "aButtons": ["select_all", "select_none"]
            },
            "columnDefs": [{   'orderable': false,"searching": false,"autoWidth":false, 'targets': [0]},
          { 'targets': [0, 9, 10,12], visible: false,  'orderable': false, }, { 'targets': [1], width: '20px' },
          { 'targets': [2], width: '54px' }, { 'targets': [3], width: '44px' }, { 'targets': [4], width: '210px' },
          { 'targets': [5], width: '70px' },{'targets': [7,11], width: '42px' },{'targets': [11], width: '30px' },
          { 'targets': [6, 8], type: 'date-euro', width: '90px', "orderable": true },
          {'targets': [2,3,5, 7, 8], "sClass": "visible-lg"},
            ],            
            "aaSorting": [],
            "fixedColumns":   { leftColumns: 1 },
            "lengthMenu": [   [25, 50, 100, -1],  [25, 50, 100, "All"]],
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var _disabled = ''; var _title = ''; $('td:eq(6)', nRow).css('text-align', 'center');
                if (aData[9] == "1") { count = Int(count + 1); $('td:eq(0)', nRow).addClass('td-backgroundcolor'); } var _res =  (aData[12]);
                if (_res == 'false') { _disabled = "icon-ban"; } else { _disabled = "icon-note"; _title = 'Generate'; }
                var detTag = '<div style="text-align:center;"><a href="javascript:;" data-toggle="tooltip" title="' + _title + '"><i class="' + _disabled + '"></i></a></div>';
                $('td:eq(8)', nRow).html(detTag);
            }
        });
     //   GetAdaptorGrids();
        $('#tblHeadRowAdpt').addClass('gridHeader'); $('#ToolTables_dataGridAdpt_0,#ToolTables_dataGridAdpt_1,#dataGridAdpt_filter,#dataGridAdpt_length,#dataGridAdpt_info,#dataGridAdpt_paginate').hide();
        $('#dataGridAdpt').css('width', '100%');

        var table = $('#dataGridServ');
        var oServTable = table.dataTable({
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

            "columnDefs": [{  'orderable': false, "searching": false,"autoWidth": false,  'targets': [0]},
            { 'targets': [1], width: '25px'},{ 'targets': [1,5], "sClass": "visible-lg"   },
            {'targets': [0,7],visible: false }],
            "lengthMenu": [ [25, 50, 100, -1], [25, 50, 100, "All"] ],        
            "fnRowCallback": function (nRow, aData, iDisplayIndex) { var detTag = '<div class="dt-left"><a id="runexeid"  href="#"><u>Run</u></a></div>';  $('td:eq(5)', nRow).html(detTag); }                
        });
       // GetServiceGrids();
        $('#tblHeadRowServ').addClass('gridHeader'); $('#ToolTables_dataGridServ_0,#ToolTables_dataGridServ_1').hide();
        $('#dataGridServ_filter,#dataGridServ_length,#dataGridServ_info,#dataGridServ_paginate').hide();

        oServTable.on('click', ' tbody td', function (e) {
            $checkbox = $($(this).parents('tr')[0]).find(':checkbox');  $checkbox.attr('checked', true); nTr = $(this).parents('tr')[0];  var colIndx = $(this).index();
            if (colIndx == 4) {   var applno = oServTable.fnGetData(nTr)[1]; var servname = oServTable.fnGetData(nTr)[2];
                var servpath = oServTable.fnGetData(nTr)[6].replace(/\\/g, '?');  RunApplication(applno, servname, servpath); } });
        var table = $('#dataGridSch');
        var oschTable = table.dataTable({
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
            "columnDefs": [{   'orderable': false, "searching": false,"autoWidth": false,   'targets': [0] },
               { 'targets': [1], width: '25px'},{ 'targets': [1, 4], "sClass": "visible-lg" }, { 'targets': [0],visible: false}],
            "lengthMenu": [ [25, 50, 100, -1], [25, 50, 100, "All"] ],
        });
     //   GetScheduleGrids();
        $('#tblHeadRowSch').addClass('gridHeader'); $('#ToolTables_dataGridSch_0,#ToolTables_dataGridSch_1,#dataGridSch_filter,#dataGridSch_length,#dataGridSch_info,#dataGridSch_paginate').hide();

        DisplayAdaptorGrids();
        oTable.on('click', ' tbody td', function (e) {
            var nTr = $(this).parents('tr')[0]; var aData = oTable.fnGetData(nTr); var colIndx = $(this).index();
            if (e.target.className == 'icon-note') {
                if (aData != null) {_selectAddrCode = '';
                    var _adptid = aData[2]; var _addrcode = _selectAddrCode = aData[3];$('.modal-title').text(_addrcode + ' - License Details');
                    GenerateAdaptorLicense(_adptid);$("#ModalLicense").modal('show');
                }
            }
        });
        $('#btnSave').live('click', function (e) {  e.preventDefault();  var _encryptLicense = $('#txtLicensekey').val(); SaveAdaptorLicensekey(_encryptLicense,_selectAddrCode);});
    };


    function DisplayAdaptorGrids() {
        setTimeout(function () {        
            GetAdaptorGrids(); GetServiceGrids(); GetScheduleGrids();        
        }, 200);
        };

    function FillAdaptorGrid(Table)
    {
        try
        {
            $('#dataGridAdpt').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null)
            {
                var t = $('#dataGridAdpt').dataTable();
                for (var i = 0; i < Table.length; i++) {var cells = new Array();  var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str(''); cells[1] = Str(''); cells[2] = Str(Table[i].ADDRESSID); cells[3] = Str(Table[i].ADDR_CODE).toUpperCase();
                    cells[4] = Str(Table[i].ADDR_NAME); cells[5] = Str(Table[i].ADDR_TYPE);  cells[6] = Str(Table[i].LASTCONNECTDATE);  cells[7] = Str(Table[i].INTERVAL);
                    cells[8] = Str(Table[i].NEXTCONNECTDATE); cells[9] = Str(Table[i].ADAPTORSTATUS); cells[10] = Str(Table[i].INTRVLSTATUS); cells[11] = Str('');
                    cells[12] = Str(Table[i].ISLESCONNECT);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else
            {
                if (Table.ADDRESSID != undefined && Table.ADDRESSID != null)
                {
                    var t = $('#dataGridAdpt').dataTable();var r = jQuery('<tr id=' + 1 + '>'); var cells = new Array();
                    cells[0] = Str(''); cells[1] = Str(''); cells[2] = Str(Table.ADDRESSID); cells[3] = Str(Table.ADDR_CODE).toUpperCase();
                    cells[4] = Str(Table.ADDR_NAME);  cells[5] = Str(Table.ADDR_TYPE); cells[6] = Str(Table.LASTCONNECTDATE); cells[7] = Str(Table.INTERVAL);
                    cells[8] = Str(Table.NEXTCONNECTDATE); cells[9] = Str(Table.ADAPTORSTATUS); cells[10] = Str(Table.INTRVLSTATUS); cells[11] = Str(''); cells[12] = Str(Table.ISLESCONNECT);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };
    var GetAdaptorGrids = function () {
        var SERVTIME =  $('#lblServertime').text();
        Metronic.blockUI('.portlet_body');
            $.ajax({
                type: "POST",
                async: false,
                url: "Adaptors.aspx/GetAdaptorGrids",
                data: "{'SERVTIME':'" + getDateTimeDetails(SERVTIME) + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var DataSet = JSON.parse(response.d);
                        if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table; FillAdaptorGrid(Table); SetIndicator(); }
                        else $('#dataGridAdpt').DataTable().clear().draw();
                        Metronic.unblockUI();
                    }
                    catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get LeS Adaptor :' + err); }
                },
                failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
                error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
            });   
    };
 
    var setupTableHeader = function () { $('#tblHeadRowAdpt').empty(); $('#tblHeadRowAdpt').append('<th id="cbAdptheader"></th><th class="gridHeader" id="txtCode">Status</th><th>Adaptor ID</th><th>Code</th><th>Description</th><th>Adaptor Type</th><th>Last Connection</th><th>Interval</th><th>Next Connection</th><th>ChkAdaptor</th><th>ChkIntrvlStatus</th><th>License</th><th>LeSConnect</th>'); $('#tblBodyAdpt').empty(); };
    var setupServicesTableHeader = function () { $('#tblHeadRowServ').empty(); $('#tblHeadRowServ').append('<th class="wide25" id="cbServheader"></th><th class="gridHeader" id="txtCode" style="font-size:8pt;padding-left:2px;">Sr. No.</th><th style="font-size:8pt;">Service</th><th>Server IP Address</th><th>Last Run</th><th style="font-size:8pt;">Next Run</th><th style="font-size:8pt;padding-left:5px;">Run</th><th style="font-size:8pt;display:none;">Exe Path</th>'); $('#tblBodyServ').empty(); };
    var setupSchedulerTableHeader = function () {$('#tblHeadRowSch').empty();$('#tblHeadRowSch').append('<th class="wide25" id="cbSchheader"></th><th class="gridHeader" id="txtCode" style="font-size:8pt;">Sr. No</th><th style="font-size:8pt;">Scheduler</th> <th style="font-size:8pt;">Last Run</th><th style="font-size:8pt;">Next Run</th>');  $('#tblBodySch').empty();};
    var GetServiceGrids = function () {
        Metronic.blockUI('#portlet_bodysev');
        $.ajax({
            type: "POST",
            async: false,
            url: "Adaptors.aspx/GetServicesGrids",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Services; FillServicesGrid(Table); }
                    else { $('#dataGridServ').DataTable().clear().draw(); $('#prtServices').hide(); }
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get LeS Services :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };
    function FillServicesGrid(Table)
    {
        try 
        {
            $('#dataGridServ').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) 
            {
                var t = $('#dataGridServ').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();  var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str('');cells[1] = Str(Table[i].SrNo).toUpperCase();  cells[2] = Str(Table[i].SeriveName).toUpperCase(); cells[3] = Str(Table[i].IpAddress).toUpperCase();
                    cells[4] = Str(Table[i].LastRun).toUpperCase(); cells[5] = Str(Table[i].NextRun).toUpperCase();  cells[6] = Str('');   cells[7] = Str(Table[i].ExPath).toUpperCase();                 
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {              
                var t = $('#dataGridServ').dataTable();    var r = jQuery('<tr id=' + 1 + '>'); var cells = new Array();
                cells[0] = Str('');cells[1] = Str(Table.SrNo).toUpperCase(); cells[2] = Str(Table.SeriveName).toUpperCase(); cells[3] = Str(Table.IpAddress).toUpperCase();
                cells[4] = Str(Table.LastRun).toUpperCase(); cells[5] = Str(Table.NextRun).toUpperCase(); cells[6] = Str(''); cells[7] = Str(Table.ExPath).toUpperCase();             
                var ai = t.fnAddData(cells, false);
                t.fnDraw();
            }
        }
        catch (e) { }
    };

    var GetScheduleGrids = function () {
        Metronic.blockUI('#portlet_bodysch');
        //setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "Adaptors.aspx/GetScheduleGrids",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Scheduler; FillScheduleGrid(Table); }
                    else { $('#dataGridSch').DataTable().clear().draw(); $('#prtScheduler').hide(); }

                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get LeS Scheduler :' + err); }
            },
            failure: function (response) {    toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) {  toastr.error("error get", response.responseText); Metronic.unblockUI();  }
        });
      //  }, 200);
    };
    function FillScheduleGrid(Table)
    {
        try
        {
            $('#dataGridSch').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null)
            {
                var t = $('#dataGridSch').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array(); var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str('');cells[1] = Str(Table[i].SrNo).toUpperCase(); cells[2] = Str(Table[i].SchedulerName).toUpperCase(); cells[3] = Str(Table[i].LastRun).toUpperCase();  cells[4] = Str(Table[i].NextRun).toUpperCase();
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else
            {
                var t = $('#dataGridSch').dataTable();  var r = jQuery('<tr id=' + 1 + '>');var cells = new Array();
                cells[0] = Str('');cells[1] = Str(Table.SrNo).toUpperCase();  cells[2] = Str(Table.SchedulerName).toUpperCase();  cells[3] = Str(Table.LastRun).toUpperCase();    cells[4] = Str(Table.NextRun).toUpperCase();
                var ai = t.fnAddData(cells, false);
                t.fnDraw();
            }
        }
        catch (e)
        { }
    };

    function RunApplication(APPLNO, SERV_NAME, SERV_PATH)
    {
        try { $.ajax({
                type: "POST",
                async: false,
                url: "Adaptors.aspx/Run_Application",
                data: "{ 'APPLNO':'" + APPLNO + "','SERV_NAME':'" + SERV_NAME + "','SERV_PATH':'" + SERV_PATH + "' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        if (response.d != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "Application run success");  }
                        else { toastr.error("Failure in Application run Process.", response.responseText); }
                    }
                    catch (err) {toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Run LeS Service :' + err); }
                },
                failure: function (response) { toastr.error("failure get", response.d); },
                error: function (response) { toastr.error("error get", response.responseText); }
            });
        }
        catch (e)
        { }
    };

    var GenerateAdaptorLicense = function (ADAPTID) {
        var _encryprtLicense = '';
        try {
            $.ajax({
                type: "POST",
                async: false,
                url: "Adaptors.aspx/GenerateAdaptorLicense_File",
                data: "{ 'ADAPTID':'" + ADAPTID + "' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {  var res = Str(response.d); if (res != undefined && res != '') { $('#txtLicensekey').val(res); }  }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Generate License :' + err); }
                },
                failure: function (response) { toastr.error("failure ", response.d); },
                error: function (response) { toastr.error("error ", response.responseText); }
            });
        }
        catch (e){ }
    };

    function SaveAdaptorLicensekey(EncyptLicense, ADDRCODE)
    {
        try {
            $.ajax({
                type: "POST",
                async: false,
                url: "Adaptors.aspx/SaveAdaptorLicensekey",
                data: "{ 'EncyptLicense':'" + EncyptLicense + "','ADDRCODE':'" + ADDRCODE + "' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d); if (res == '1') { toastr.success("Lighthouse eSolutions Pte. Ltd.", 'License Saved successfully'); $("#ModalLicense").modal('hide'); }
                    }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Save License :' + err); }
                },
                failure: function (response) { toastr.error("failure ", response.d); },
                error: function (response) { toastr.error("error ", response.responseText); }
            });
        }
        catch (e) { }
    };

    var CheckLeSConnectAdaptor = function (ADAPTID) {
        var _isExist = '';
        try {
            $.ajax({
                type: "POST",
                async: false,
                url: "Adaptors.aspx/CheckLesAdaptor",
                data: "{ 'ADAPTID':'" + ADAPTID +  "' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {  _isExist = Str(response.d); }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Error :' + err); }
                },
                failure: function (response) { toastr.error("failure ", response.d); },
                error: function (response) { toastr.error("error ", response.responseText); }
            });
            return _isExist;
        }
        catch (e){ }
    };

    function SetIndicator() { if (Int(count) == 0) { } else { $("#servStatus").text(count).addClass("badge badge-danger"); } };// $("#servStatus").text(count).addClass("badge badge-success");

    function getDecryptedData() {
        var KEYURL = ''; var _result = '';var ind = window.location.href.indexOf('?');
        if (ind > -1) {
            KEYURL = window.location.href.split('?')[1];
            $.ajax({
                type: "POST",
                async: false,
                url: "../Common/Default.aspx/DecryptURL",
                data: "{'KEYURL':'" + Str(KEYURL) + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try { _result = response.d; }
                    catch (err) { toastr.error('Error in Getting decrypted Url :' + err, "Lighthouse eSolutions Pte. Ltd"); }
                },
                failure: function (response) { toastr.error("Failure in Getting decrypted Url", "Lighthouse eSolutions Pte. Ltd"); },
                error: function (response) { toastr.error("Error in Getting decrypted Url", "Lighthouse eSolutions Pte. Ltd"); }
            });
        }
        return _result;
    };

    return {
        init: function () { handleAdaptorTable(); }
    };
}();