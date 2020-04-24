var _fromlogdate = ''; var _tologdate = ''; var duration = ''; var oBTable = ''; var oSTable = ''; var _selectID = '0'; var _id = '0'; var _activetab = '';

var Overview = function () {
    var handleOverviewTable = function () {
        $('#toolbtngroup').empty(); 
        SetupBreadcrumb('Home', 'Home.aspx', 'Overview', 'Overview.aspx', '', ''); $('#pageTitle').empty().append('Overview');$(document.getElementById('lnkOverview')).addClass('active');                    
        duration = $("#cbDuration option:selected").text(); CommonSettings(duration);
        $('#cbDuration').on("change", function (e) {
            duration = $('#cbDuration option:selected').text(); if (duration != '' && duration != undefined) {
                GetBuyerSupplier_Overview(duration, $('.nav-tabs .active').text());
            }
            else { $('#dataGridSupp').DataTable().clear().draw(); $('#dataGridBuyer').DataTable().clear().draw(); }});
        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            var target = e.target.innerHTML; setAccessPortlet(Str(e.target.id));
            _id = (_selectID != '0') ? _selectID : '0'; _activetab = Str(target);
            if (Str(target) == 'Buyer') { GetBuyerGrid(duration, _id); } else if (Str(target) == 'Supplier') { GetSupplierGrid(duration, _id); }
        });
        $('#btnMClientOverview').live('click', function (e) { e.preventDefault(); var _url = GetMClient_TransactionPath(); window.open(_url, '_blank'); });
        $('#btnRefresh').live('click', function (e) { e.preventDefault(); _selectID = '0'; GetBuyerGrid(duration,'0'); });
        $('#dataGridBuyer').on('click', 'tbody td', function (e) {
            e.stopPropagation();
            if (e.target.type == "radio") {
                var $checkbox = $(this).find(':radio'); var _schkd = $checkbox.prop('checked');
                if (_schkd == true) {
                    var tr = $(this).closest('tr'); $checkbox.prop("checked", _schkd); //tr.find('input[type="checkbox"]').not($(this)).prop("checked", false);
                    var data = $('#dataGridBuyer').DataTable().row(tr).data(); _selectID = Str(data[7]);
                    GetLinked_Overview(duration, _selectID, 'SUPPLIER');
                }
                else { if (_activetab == 'Buyer') { GetSupplierGrid(duration, '0'); _selectID = '0' } }
            }
        });

        $('#dataGridSupp').on('click', 'tbody td', function (e) {
            e.stopPropagation();
            if (e.target.type == "radio") {
                var $checkbox = $(this).find(':radio'); var _schkd = $checkbox.prop('checked');
                if (_schkd == true) {
                    var tr = $(this).closest('tr'); $checkbox.prop("checked", _schkd); //tr.find('input[type="checkbox"]').not($(this)).prop("checked", false);
                    var data = $('#dataGridSupp').DataTable().row(tr).data(); _selectID = Str(data[7]);
                    GetLinked_Overview(duration, _selectID, 'BUYER');
                }
                else { if (_activetab == 'Supplier') { GetBuyerGrid(duration, '0'); _selectID = '0' } }
            }
        });

    };
    function CommonSettings(duration) { setToolbar(); setAccessPortlet('0'); setupBuyerTableHeader(); setupSupplierTableHeader(); InitializeBuyer(); InitializeSupplier();GetBuyerGrid(duration,'0');};
    function setAccessPortlet(tabindx) {  $('#prtByDet').hide(); $('#prtSppDet').hide();  if (tabindx == '0') { $('#prtByDet').show();  }  else if (tabindx == '1') { $('#prtSppDet').show(); }  };
    var setupBuyerTableHeader = function () { var dtfilter = '<th></th><th>Code</th><th>Buyer</th><th class="aligncenter">RFQs</th><th class="aligncenter">Quotes</th><th class="aligncenter">POs</th><th class="aligncenter">POCs</th><th>ADDRESSID</th>'; $('#tblHeadRowBuyer').empty().append(dtfilter); $('#tblBodyBuyer').empty(); };
    function InitializeBuyer() {
       var table = $('#dataGridBuyer');
        oBTable = table.dataTable({
            "bDestroy": true,
            "bSort": false,
            "language": {
                "aria": {   "sortAscending": ": activate to sort column ascending", "sortDescending": ": activate to sort column descending"},
                "emptyTable": "No data available in table", "info": "Showing _START_ to _END_ of _TOTAL_ entries",  "infoEmpty": "No entries found",
                "infoFiltered": "(filtered from _MAX_ total entries)", "lengthMenu": "Show _MENU_ entries", "search": "Quick Search:", "zeroRecords": "No matching records found"
            },
            dom: 'T<"clear">lfrtip', tableTools: { "sRowSelect": "single", "aButtons": ["select_all", "select_none"] },
            "columnDefs": [{ 'orderable': true, "searching": true, "autoWidth": false, 'targets': [0] },
             { 'targets': [0], width: '2px' }, {'targets': [1], width: '10px' }, { 'targets': [2], width: '70px' }, { 'targets': [3, 5], 'bSortable': false, "sType": "numeric", width: '5px' },
             { 'targets': [4, 6], 'bSortable': false, "sType": "numeric", width: '5px' }, { 'targets': [7], visible: false }
            ],         
            "order": [[2, "desc"]], "lengthMenu": [[15, 25, 50, 100, -1], [15, 25, 50, 100, "All"]],
            "paging": true, "aaSorting": [], "sScrollX": true,//"sScrollY": '300px', 
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                $('td:eq(3),td:eq(4),td:eq(5),td:eq(6)', nRow).addClass('aligncenter');
                var _chkid = "chk" + nRow._DT_RowIndex; var _chkdiv = '<div style="text-align:center;"><input type="radio" name="ByrRadio"  id="' + _chkid + '"  /></div>';$('td:eq(0)', nRow).html(_chkdiv);
            }
        });
        $('#tblHeadRowBuyer').addClass('gridHeader'); $('#ToolTables_dataGridBuyer_0,#ToolTables_dataGridBuyer_1').hide();
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%"); $('#dataGridBuyer').css('width', '100%');
    };
    var GetBuyerGrid = function (DURATION, ADDRESSID) {    
        if (ADDRESSID == '0') { setTimeout(function () { GetBuyer_Overview(DURATION); }, 200); }// GetLinked_Overview(DURATION, ADDRESSID, 'SUPPLIER');
    };
    function GetBuyer_Overview(DURATION) {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST", async: false, url: "Overview.aspx/SetOverview", data: "{ 'DURATION':'" + DURATION + "','ADDRTYPE':'BUYER' }", contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d); if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table; FillBuyerGrid(Table); }
                    else $('#dataGridBuyer').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer Overview :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };
    function FillBuyerGrid(Table) {
        try {
            $('#dataGridBuyer').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridBuyer').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');                    
                    cells[0] = Str(''); cells[1] = Str(Table[i].ADDR_CODE); cells[2] = Str(Table[i].ADDR_NAME); cells[3] = Int(Table[i].RFQ_COUNT); cells[4] = Int(Table[i].QUOTE_COUNT); cells[5] = Int(Table[i].PO_COUNT); cells[6] = Int(Table[i].POC_COUNT); cells[7] = Int(Table[i].ADDRESSID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.ADDRESSID != undefined && Table.ADDRESSID != null) {
                    var t = $('#dataGridBuyer').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str(''); cells[1] = Str(Table.ADDR_CODE); cells[2] = Str(Table.ADDR_NAME); cells[3] = Int(Table.RFQ_COUNT); cells[4] = Int(Table.QUOTE_COUNT); cells[5] = Int(Table.PO_COUNT); cells[6] = Int(Table.POC_COUNT); cells[7] = Int(Table.ADDRESSID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };
    var setupSupplierTableHeader = function () { var dtfilter = '<th></th><th>Code</th><th>Supplier</th><th class="aligncenter">RFQs</th><th class="aligncenter">Quotes</th><th class="aligncenter">POs</th><th class="aligncenter">POCs</th><th>ADDRESSID</th>'; $('#tblHeadRowSupp').empty().append(dtfilter); $('#tblBodySupp').empty(); };
    function InitializeSupplier() {
      var table = $('#dataGridSupp');
        oSTable = table.dataTable({
            "bDestroy": true,
            "bSort": false,
            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "No data available in table", "info": "Showing _START_ to _END_ of _TOTAL_ entries",  "infoEmpty": "No entries found",
                "infoFiltered": "(filtered from _MAX_ total entries)", "lengthMenu": "Show _MENU_ entries", "search": "Quick Search:","zeroRecords": "No matching records found"
            },
            dom: 'T<"clear">lfrtip',  tableTools: { "sRowSelect": "single",  "aButtons": ["select_all", "select_none"]},
            "columnDefs": [{ 'orderable': true, "searching": true, "autoWidth": false, 'targets': [0] },
              { 'targets': [0], width: '2px' }, { 'targets': [1], width: '10px' }, { 'targets': [2], width: '70px' }, { 'targets': [3, 5], 'bSortable': false, "sType": "numeric", width: '5px' },
             { 'targets': [4, 6], 'bSortable': false, "sType": "numeric", width: '5px' }, { 'targets': [7], visible: false }
            ],
            "order": [[2, "desc"]], "lengthMenu": [[15, 25, 50, 100, -1], [15, 25, 50, 100, "All"]], "paging": true, "sScrollX": true,//"sScrollY": '300px',        
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {           
                $('td:eq(3),td:eq(4),td:eq(5),td:eq(6)', nRow).addClass('aligncenter');
                var _chkid = "chk" + nRow._DT_RowIndex; var _chkdiv = '<div style="text-align:center;"><input type="radio" name="SppRadio"   id="' + _chkid + '"  /></div>'; $('td:eq(0)', nRow).html(_chkdiv);
            }
        });
        $('#tblHeadRowSupp').addClass('gridHeader'); $('#ToolTables_dataGridSupp_0,#ToolTables_dataGridSupp_1').hide();
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%"); $('#dataGridSupp').css('width', '100%');
    };
    var GetSupplierGrid = function (DURATION, ADDRESSID) {
        if (ADDRESSID == '0') { setTimeout(function () { GetSupplier_Overview(DURATION); }, 200); }//GetLinked_Overview(DURATION, ADDRESSID, 'BUYER');
    };
    function GetSupplier_Overview(DURATION) {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST", async: false, url: "Overview.aspx/SetOverview", data: "{ 'DURATION':'" + DURATION + "','ADDRTYPE':'SUPPLIER' }", contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d); if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table; FillSupplierGrid(Table); }
                    else $('#dataGridSupp').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Supplier Overview :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };
    function GetBuyerSupplier_Overview(DURATION, activeTab) { if (activeTab == 'Buyer') { GetBuyerGrid(DURATION,'0'); } else if (activeTab == 'Supplier') { GetSupplierGrid(DURATION,'0'); } };
    function FillSupplierGrid(Table) {
        try {
            $('#dataGridSupp').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridSupp').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array(); var r = jQuery('<tr id=' + i + '>');                                       
                    cells[0] = Str(''); cells[1] = Str(Table[i].ADDR_CODE); cells[2] = Str(Table[i].ADDR_NAME); cells[3] = Int(Table[i].RFQ_COUNT); cells[4] = Int(Table[i].QUOTE_COUNT); cells[5] = Int(Table[i].PO_COUNT); cells[6] = Int(Table[i].POC_COUNT); cells[7] = Int(Table[i].ADDRESSID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.ADDRESSID != undefined && Table.ADDRESSID != null) {
                    var t = $('#dataGridSupp').dataTable();  var r = jQuery('<tr id=' + 1 + '>'); var cells = new Array();                    
                    cells[0] = Str(''); cells[1] = Str(Table.ADDR_CODE); cells[2] = Str(Table.ADDR_NAME); cells[3] = Int(Table.RFQ_COUNT); cells[4] = Int(Table.QUOTE_COUNT); cells[5] = Int(Table.PO_COUNT); cells[6] = Int(Table.POC_COUNT); cells[7] = Int(Table.ADDRESSID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e) { }
    };
    function GetLinked_Overview(DURATION, ADDRESSID, ADDRTYPE) {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "Overview.aspx/SetLinkedOverview_Addressid",
            data: "{'DURATION':'" + DURATION + "','ADDRESSID':'" + ADDRESSID + "','ADDRTYPE':'" + ADDRTYPE + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d); if (DataSet.NewDataSet != null) {
                        Table = DataSet.NewDataSet.Table; if (ADDRTYPE == 'BUYER') { FillBuyerGrid(Table); } else { FillSupplierGrid(Table); }
                    }
                    else { if (ADDRTYPE == 'BUYER') { $('#dataGridBuyer').DataTable().clear().draw(); } else { $('#dataGridSupp').DataTable().clear().draw(); } }
                    Metronic.unblockUI();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Linked Overview:' + err); Metronic.unblockUI(); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };
    var setToolbar = function () {
        var _btns =' <span title="MonthlyClient Overview" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnMClientOverview"><i class="fa fa-calendar" style="text-align:center;"></i></a></div></span>'+
                ' <span title="Refresh" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"> <a href="javascript:;" class="toolbtn" id="btnRefresh"><i class="fa fa-recycle" style="text-align:center;"></i></a></div></span>';
        $('#toolbtngroup').empty().append(_btns);
    };
    function GetMClient_TransactionPath() {
        var _res = '';
            $.ajax({
                type: "POST", async: false, url: "Overview.aspx/GetClient_TransactionOverview_Url", data: '{}', contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (response) {
                    try {  _res=Str(response.d); }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Path of Client transaction Overview :' + err); }
                },
                failure: function (response) { toastr.error("failure get", response.d); },
                error: function (response) { toastr.error("error get", response.responseText);  }
            });
            return _res;
    };
     return {  init: function () { handleOverviewTable(); }};
}();
