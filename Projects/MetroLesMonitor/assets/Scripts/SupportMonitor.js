var _fromlogdate = '';
var _tologdate = '';
var _searchfilter = '';

var AuditLog = function () {

    var handleAuditLogTable = function () {
        var nEditing = null;
        var nNew = false;
        $('#divSpacer').remove();
       
      //  $('#pageTitle').empty().append('AuditLog');
        $('#divFilterWrapper').remove();
        $('#divWrapper').hide();
        $(document.getElementById('lnkAuditDet')).addClass('active open');
      //  $(document.getElementById('lnkAuditLog')).addClass('active');
              
        setupTableHeader();

        //-------------------------------------------------------------------------------//
        var table = $('#dataGridALog');
        var oTable = table.dataTable({
            "bDestroy": true,
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
            "columnDefs": [{  // set default column settings
                'orderable': false,
                "searching": true,
                "autoWidth": false,
                'targets': [0]
            },
           {
               'targets': [0,11, 12, 13, 14, 15],
              visible: false
           },
            {
                'targets': [1],
                'bSortable': false
            },
            {
               'targets': [3,10],
                width:'5px'
             },
            {
                'targets': [4,5,6,7],
               width:'8px'
            }
             ],
            //  "order": [[1, "asc"]],
            "lengthMenu": [
                [25, 50, 100, -1],
                [25, 50, 100, "All"] // change per page values here
            ],
            "pageLength": 25,
            "bScrollInfinite": true,
           // "scrollY": "700px"
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                if (Str(aData[15]) != "") {
                var htm = '<div style="width:5px;"><a href="#">Resubmit</a></div>';
                $('td:eq(10)', nRow).visible = false;
                }               
            }
       
        });

        $('#tblHeadRowALog').addClass('gridHeader');
        $('#ToolTables_dataGridALog_0,#ToolTables_dataGridALog_1').hide();
        $('#dataGridALog_length').hide();//,#dataGridALog_info
       
        // $('#dataGridALog_paginate').hide();$('#dataGridALog_filter').hide();

        setFilterToolbar();
        _searchfilter = $(document.getElementById('txtSearch')).val();

        GetAuditLogGrids(_fromlogdate, _tologdate, _searchfilter);

        $('#btnApply').on('click', function (e) {
            e.preventDefault();
            _fromlogdate = $(document.getElementById('dtLogFromDate')).val();
            _tologdate = $(document.getElementById('dtLogToDate')).val();
            _searchfilter = $(document.getElementById('txtSearch')).val();
            GetAuditLogGrids(_fromlogdate, _tologdate, _searchfilter);
        });

        $('#dtLogFromDate').datepicker() .on('changeDate', function (ev) {
             $('#dtLogFromDate').datepicker('hide');
        });

        $('#dtLogToDate').datepicker().on('changeDate', function (ev) {
            $('#dtLogToDate').datepicker('hide');
        });


        $('#btnReSubmit').on('click', function (e) {
            e.preventDefault();
        });
       
        oTable.on('click', 'thead tr', function (e)
        {
            e.stopPropagation();
        });

  
        oTable.on('click', ' tbody td', function (e)
        {
            $checkbox = $($(this).parents('tr')[0]).find(':checkbox');
            $checkbox.attr('checked', true);
            nTr = $(this).parents('tr')[0];

           var colIndx = $(this).index();

            var updatedate = oTable.fnGetData(nTr)[1];
            var modulename = oTable.fnGetData(nTr)[3];         
            var logid = oTable.fnGetData(nTr)[14];
            var filename = oTable.fnGetData(nTr)[15];
            if (colIndx == 9)
            {
                if (Str(filename) != "") {
                   DownloadFile(updatedate, modulename, filename);
                }
            }
            else if (colIndx == 10) {
                if (Str(filename) != "") {
                    ShowResubmitModal(logid);
                }
            }
        
        });      
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
                    cells[0] = '<td><div class=""><span style="padding-left:12px;"><input type="checkbox" class="checkboxes widelarge" value="1"></span></div></td>';
                    cells[1] = Str(Table[i].UPDATE_DATE);
                    cells[2] = Str(Table[i].ServerName);
                    cells[3] = Str(Table[i].MODULENAME);
                    cells[4] = Str(Table[i].DocType);
                    cells[5] = Str(Table[i].LOGTYPE);
                    cells[6] = Str(Table[i].BUYER_CODE);
                    cells[7] = Str(Table[i].VENDOR_CODE);
                    cells[8] = Str(Table[i].KEYREF2);
                    cells[9] = '<td><div style="width:200px; word-break: break-all; word-wrap: break-word;">' + Str(Table[i].AUDITVALUE) + '</div></td>';
                    cells[10] = '<td><div style="width:180px; word-break: break-all; word-wrap: break-word;"><a href="#">' + Str(Table[i].FILENAME) + '</a></div></td>';
                    cells[11] = Str('');
                    cells[12] = Str(Table[i].BUYER_ID);
                    cells[13] = Str(Table[i].SUPPLIER_ID);
                    cells[14] = Str(Table[i].LOGID);
                    cells[15] = Str(Table[i].FILENAME);
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
                    var cells = new Array();
                    cells[0] = '<td><div class=""><span style="padding-left:12px;"><input type="checkbox" class="checkboxes widelarge" value="1"></span></div></td>';
                    cells[1] = Str(Table.UPDATE_DATE);
                    cells[2] = Str(Table.ServerName);
                    cells[3] = Str(Table.MODULENAME);
                    cells[4] = Str(Table.DocType);
                    cells[5] = Str(Table.LOGTYPE);
                    cells[6] = Str(Table.BUYER_CODE);
                    cells[7] = Str(Table.VENDOR_CODE);
                    cells[8] = Str(Table.KEYREF2);
                    cells[9] = '<td><div style="width:200px; word-break: break-all; word-wrap: break-word;">' + Str(Table[i].AUDITVALUE) + '</div></td>';
                    cells[10] = '<td><div style="width:180px; word-break: break-all; word-wrap: break-word;"><a href="#">' + Str(Table[i].FILENAME) + '</a></div></td>';
                    cells[11] = Str('');
                    cells[12] = Str(Table.BUYER_ID);
                    cells[13] = Str(Table.SUPPLIER_ID);
                    cells[14] = Str(Table.LOGID);
                    cells[15] = Str(Table.FILENAME);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    var GetAuditLogGrids = function (_fromlogdate, _tologdate, _searchfilter) {
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "AuditLog.aspx/GetAuditLogGrid",
            data: "{ 'LOG_DATEFROM':'" + getSQLDateFormated(_fromlogdate) + "','LOG_DATETO':'" + getSQLDateFormated(_tologdate) + "','SEARCH':'" + Str(_searchfilter) + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response)
            {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null)
                    {
                        Table = DataSet.NewDataSet.Table;
                        FillAuditLogGrid(Table);
                    }
                    else $('#dataGridALog').DataTable().clear().draw();
                   Metronic.unblockUI();
                }
                catch (err) {
                     Metronic.unblockUI();
                    toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Les Auditlog :' + err);
                }
            },
            failure: function (response) {
                toastr.error("failure get", response.d); Metronic.unblockUI(); 
            },
            error: function (response) {
                toastr.error("error get", response.responseText); Metronic.unblockUI();
            }
        });
    };

    var setFilterToolbar = function () {
        var _btns = '<div class="pull-left"> <input id="dtLogFromDate" class="form-control-inline input-small date-picker csDatePicker"  data-date-format="dd/mm/yyyy" size="12" type="text" value="">' +
            '<input id="dtLogToDate" class="form-control-inline input-small date-picker csDatePicker"  data-date-format="dd/mm/yyyy" size="12" type="text" value="">'+
            '<button id="btnApply" type="button" class="btn btn-circle btn-icon-only blue" name="Apply" title="Apply"><i class="glyphicon glyphicon-refresh icon-refresh"></i></button>';
        $('#dataGridALog_filter').prepend(_btns);

        var _txt = '<input id="txtSearch" size="12" type="text" value="">';
        $('#dataGridALog_filter').append(_txt);

        var date = new Date();

        $(document.getElementById('dtLogFromDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
        $(document.getElementById('dtLogToDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));       
        _fromlogdate = $(document.getElementById('dtLogFromDate')).val();
        _tologdate = $(document.getElementById('dtLogToDate')).val();
        $('input[type="search"]').css('display', 'none');
    };

    var setupTableHeader = function () {

        var dtfilter = '<th class="wide25" id="cbALogheader"></th><th class="gridHeader" id="txtCode">Log Date</th><th>Server Name</th><th>Module</th><th>Doc Type</th><th>Log Type</th><th>Buyer Code</th><th>Supplier Code</th><th>Key Ref</th><th>Remark</th><th>File Name</th>'+
            '<th>Resubmit File</th><th>BuyerID</th><th>SupplierID</th><th>Log ID</th><th>Filename</th>';

        $('#tblHeadRowALog').empty();
        $('#tblHeadRowALog').append(dtfilter);
        $('#tblBodyALog').empty();
    };
   
    function DownloadFile(UPDATEDATE, MODULENAME, FILENAME)
    {
        try {
            $.ajax({
                type: "POST",
                async: false,
                url: "AuditLog.aspx/DownloadFile",
                data: "{ 'UPDATEDATE':'" + UPDATEDATE + "','MODULENAME':'" + MODULENAME + "','FILENAME':'" + FILENAME + "' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d);
                        if (res == "1") {
                            toastr.success("Lighthouse eSolutions Pte. Ltd", "Download File success");
                        }
                        else {
                            toastr.error("Lighthouse eSolutions Pte. Ltd", res);
                        }
                    }
                    catch (err) {
                        toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Download File :' + err);
                    }
                },
                failure: function (response) {
                    toastr.error("failure get", response.d);
                },
                error: function (response) {
                    toastr.error("error get", response.responseText);
                }
            });

        }
        catch (e)
        { }
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
                    try {
                        toastr.success("Lighthouse eSolutions Pte. Ltd", "Resubmit File success");
                    }
                    catch (err) {
                        toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Resubmit File :' + err);
                    }
                },
                failure: function (response) {
                    toastr.error("failure get", response.d);
                },
                error: function (response) {
                    toastr.error("error get", response.responseText);
                }
            });

        }
        catch (e)
        { }
    };

    function ShowResubmitModal(LOGID) {
        var fileexist = CheckFileExisting(LOGID);
        fileexist = "true";
        if (fileexist == "false") { toastr.error("Filepath not found to resubmit file !"); }
        else {
            var txtFile = document.getElementById("txtFile");
            if (txtFile != null) txtFile.Text = "";

            var lblError = document.getElementById("lblError");
            if (lblError != null) lblError.innerText = "";

            var lblFileName = document.getElementById("lblFileName");
            if (lblFileName != null) lblFileName.innerText = "";
            $("#ModalResubmit").modal('show');
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
                    try {
                        Isexist = Str(response.d);
                    }
                    catch (err) {
                    }
                },
                failure: function (response) {                    
                },
                error: function (response) {                   
                }
            });
            return Isexist;
        }
        catch (e)
        { }
    };

    return {
        init: function () {handleAuditLogTable(); }
    };
}();