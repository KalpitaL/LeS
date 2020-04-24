var _fromlogdate = ''; var _tologdate = ''; var selectedTr = ''; var previousTr = ''; var selectedESTr = ''; var previousESTr = ''; var _serrdet = []; var _scheckid = []; var _modulename = ''; var oErrSolTempl = '';
var _filename = ''; var updtcnt = ''; var _Maildet = []; var _buyercd = ''; var _suppliercd = ''; var _allselectcount = 0; var updatedate = ''; var servername = '';
var _lstselected = []; var _errStatus = ''; var _lstErrSdet = [];

var Errors = function () {

    var handleErrorsTable = function () {
        var nEditing = null; var nNew = false; $('#pageTitle').empty().append('Errors'); SetupBreadcrumb('Home', 'Home.aspx', 'Audit', '#', 'Errors', 'Errors.aspx');
        $(document.getElementById('lnkAuditDet')).addClass('active open'); $(document.getElementById('spError')).addClass('title font-title SelectedColor');
        setupTableHeader(); oErrSolTempl = $('#dataGridSolnTemp');

        var table = $('#dataGridErr');
        var oErrTable = table.dataTable({
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
            "columnDefs": [{ 'orderable': false, "searching": true, "autoWidth": false,'targets': [0] },
             { 'targets': [0], width: '5px', 'bSortable': false }, { 'targets': [1], width: '5px', 'bSortable': false },
             { 'targets': [2], width: '50px' }, { 'targets': [3], width: '40px' }, { 'targets': [4], width: '55px' }, { 'targets': [5], width: '50px' },
             { 'targets': [6], width: '55px' }, { 'targets': [7], width: '40px' }, { 'targets': [8], width: '50px' },
             { 'targets': [9], width: '50px', "sClass": "break-det" }, { 'targets': [10], width: '70px', "sClass": "break-det" },
             { 'targets': [11], width: '180px', "sClass": "break-det" }, { 'targets': [12], width: '90px', "sClass": "break-det" }, { 'targets': [13, 15,16], width: '50px' },
             { 'targets': [14], width: '50px' },{ 'targets': [4, 5, 6, 11], "sClass": "visible-lg" }, { 'targets': [ 17, 18, 19], visible: false }
            ],
            "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
            "scrollY": '300px',
            "sScrollX": '1100px',
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var divid = 'dv' + nRow._DT_RowIndex; var chkid = 'chk' + nRow._DT_RowIndex; var _isPriority = ''; var _status = Str(aData[16]).toUpperCase();
                var chkTag = '<span style="padding-left:10px;text-align:center;"><input type="checkbox" id= "' + chkid + '" class="checkboxes widelarge"></span>';              
                if (Str(aData[17]) != ' ' && _status == "PENDING") { _isPriority = (aData[17] == '1') ? '#ffcccc' : ''; }
                var detTag = ' <div id="' + divid + '" style="text-align:center;"><span class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span></div>';         
                var fileTag = '<a href="javascript:;">' + aData[12] + '</<a>'; var lnkTag = '<span><a><u>Details</u></<a></span>';
                $('td:eq(0)', nRow).html(detTag); $('td:eq(1)', nRow).html(chkTag); $('td:eq(12)', nRow).html(fileTag); $('td:eq(15)', nRow).html(lnkTag);
                $('td:eq(1)', nRow).css('background-color', _isPriority);
            }
        });

        $('#tblHeadRowErr').addClass('gridHeader'); $('#ToolTables_dataGridErr_0,#ToolTables_dataGridErr_1').hide();
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%"); $('#dataGridErr').css('width', '100%');

        $(".dataTables_scrollBody table").css('min-width', "100%");
        $(this).removeClass("DTTT_selected selected"); setFilterToolbar(); GetErrorGrids(_fromlogdate, _tologdate);
        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            var target = e.target.innerHTML; SetAccessTabs(Str(e.target.id));
            if (Str(target) == 'Errors') { GetErrorGrids(_fromlogdate, _tologdate); } else if (Str(target) == 'Solution Templates') { ShowError_SolTempGrid(); }         
        });
        $('#dtUpdateFromDate').datepicker().on('changeDate', function (ev) { $('#dtUpdateFromDate').datepicker('hide');  });
        $('#dtUpdateToDate').datepicker().on('changeDate', function (ev) { $('#dtUpdateToDate').datepicker('hide');});      
        $('#btnRefresh').live('click', function (e) {
            e.preventDefault(); _fromlogdate = $(document.getElementById('dtUpdateFromDate')).val(); _tologdate = $(document.getElementById('dtUpdateToDate')).val();
            var activeTab = $('#hTabs .active').text();
            if (Str(activeTab) == 'Errors') { GetErrorGrids(_fromlogdate, _tologdate); }
            else if (Str(activeTab) == 'Solution Templates') { GetError_SolutionTemplates(); }
        });
        $('#btnClear').on('click', function (e) { e.preventDefault(); ClearFilter(); });
        $('#btnSendMail').on('click', function (e){
            e.preventDefault(); $('#dvMailDet').empty();
            var btndiv = '<div style="text-align:right;margin-left:20px;"><span><a href="javascript:;" id="btnSend" class="btn yellow-casablanca"> Send <i class="fa fa-edit"></i></a></span> <span><a href="javascript:;" id="btnMailClose" class="btn yellow-casablanca"> Close <i class="fa fa-close"></i></a></span></div>';
            $('#ftrtoolbtn').empty().append(btndiv);  var divtag = fnMailDetails(_buyercd, _suppliercd); $('#dvMailDet').append(divtag); $("#ModalMail").modal('show');
        });
        $('#btnMailClose').live('click', function (e) { e.preventDefault(); $("#ModalMail").modal('hide'); });
        $('#btnAttach').live('click', function (e) { e.preventDefault(); alert('attach button clicked'); });
        $('#btnSaveErrSoln').on('click', function (e) {
            e.preventDefault(); _buydet = []; var _res = Validate_ErrorDetails(); 
            if (_res == true) {  _lstErrSdet = [];
                GetErrorSol_RowDetails('', '', _lstErrSdet, 'Details');  $("#ModalErrSolution").modal('hide');
            }
        });
        $('#btnExpExcel').live('click', function (e) { e.preventDefault(); ExportErrors(); });
        _allselectcount = 0;
        $('th').click(function (e) {
            if (e.target.type == "checkbox") {
                var allPages = oTable.fnGetNodes(); _lstselected = [];var $checkbox = $(this).find(':checkbox'); var myCol = $(this).index();
                if (_lstselected.length == 0) { _lstselected.push(myCol); }
                var chkBoxes = $("input[id^=cb" + myCol + "_]:not(:disabled)", allPages); var sallchk = $('#allcb' + myCol);
                var _isselall = sallchk.prop("checked"); chkBoxes.prop("checked", sallchk.prop("checked"));
                if (_isselall) { _allselectcount++; } else { _allselectcount = 0; }
                if (_allselectcount > 1) { toastr.error('Please select only one header item.', "LesMonitor"); }
            }
        });

        oErrTable.on('click', 'tbody td', function (e) {
            var selectedTr = $(this).parents('tr')[0]; var aData = oErrTable.fnGetData(selectedTr); var cellindx = $(this).index(); updtcnt = 0;
            if (oErrTable.fnIsOpen(selectedTr) && (e.target.className == 'glyphicon glyphicon-pencil')) { oErrTable.fnClose(selectedTr); }
            else {
                var divid = 'dv' + selectedTr._DT_RowIndex;
                if (e.target.className == 'glyphicon glyphicon-pencil') {
                    if ((previousTr != '') && (oErrTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {
                        var prevdivid = 'dv' + previousTr._DT_RowIndex; oErrTable.fnClose(previousTr); $('#' + prevdivid).show();
                    }
                    if (aData != null) { oErrTable.fnOpen(selectedTr, fnFormatDetails(oErrTable, selectedTr), 'details'); $('#' + divid).hide(); previousTr = selectedTr; }
                }
                else {
                    if (cellindx == 12 && selectedTr.cells[12] != null) {
                        if (aData != null) { _modulename = Str(aData[6]); _filename = Str(aData[12]);updatedate = Str(aData[2]);  servername = Str(aData[3]); }
                        DownloadFile(updatedate, modulename, filename, servername);
                    }
                    else if (cellindx == 15 && selectedTr.cells[15] != null) {
                        if (aData != null) { GetError_SolModal_Details(oErrTable, selectedTr); }
                    }
                    else { if (aData != null) { _buyercd = aData[7]; _suppliercd = aData[8]; } }
                }
            }
            $('#btnUpdate').click(function () {
                _serrdet = [];
                if (aData != null && updtcnt == 0) {
                    GetErrorRowDetails(oErrTable, selectedTr, _serrdet); _scheckid = [];
                    $('input[type=checkbox]:checked').each(function () {
                        var tr = $(this).closest('tr'); var chkid = $(this)[0].id; var _logid = oErrTable.fnGetData(tr)[18]; _scheckid.push(Str(_logid));
                    });
                    UpdateErrorDetails(_serrdet, _scheckid, GetErrorGrids); updtcnt++;
                }
                $('#' + divid).show();
            });
            $('#btnCancel').click(function () { if (oErrTable.fnIsOpen(selectedTr)) { oErrTable.fnClose(selectedTr); } $('#' + divid).show(); });
        });
 
        var _filter = '<div class="FilterChkbox"><label> <input type="checkbox" id="chkPriorityErr" />Show Priority Errors</label></div>'; $('#dataGridErr_filter').prepend(_filter);

        $('#chkPriorityErr').live('change',function () { if (this.checked) { oErrTable.fnFilter('1', 17); } else { oErrTable.fnFilter('',17); }  return false;  });

        $('#dataGridSolnTemp').on('click', 'tbody td', function (e) {
            selectedESTr = $(this).parents('tr')[0]; var aData = oErrSolTempl.fnGetData(selectedESTr); var cellindx = $(this).index(); updtcnt = 0;
            var divid = 'dv' + selectedESTr._DT_RowIndex;
            if (oErrSolTempl.fnIsOpen(selectedESTr) && (e.target.className == 'glyphicon glyphicon-pencil')) { oErrSolTempl.fnClose(selectedESTr); }
            else
            {
                if (e.target.className == 'glyphicon glyphicon-pencil') {
                    if ((previousESTr != '') && (oErrSolTempl.fnIsOpen(previousESTr) && previousESTr != selectedESTr)) {
                        var prevdivid = 'dv' + previousESTr._DT_RowIndex; oErrSolTempl.fnClose(previousESTr); $('#' + prevdivid).show();
                    }
                    if (aData != null) { oErrSolTempl.fnOpen(selectedESTr, fnErrSoln_Update(oErrSolTempl, selectedESTr), 'details'); $('#' + divid).hide(); previousESTr = selectedESTr; }
                }
            };
             $('#btnErrSUpdate').click(function () {              
                 if (aData != null) {_lstErrSdet = [];
                     GetErrorSol_RowDetails(oErrSolTempl, selectedESTr, _lstErrSdet,'Edit'); }
                 $('#' + divid).show();
             });
            $('#btnErrSCancel').click(function () { if (oErrSolTempl.fnIsOpen(selectedESTr)) { oErrSolTempl.fnClose(selectedESTr); } $('#' + divid).show(); });
        });
    };

    function GetErrorRowDetails(Table, nTr, _lsterrdet) {
        var indx= nTr.rowIndex; var tid = "ErrTable" +indx;
        var _errprb = $('#txtErrProblem' + indx).val(); var _errsoln = $('#txtErrSolution' + indx).val(); var _errstatus = $('#cbErrStatus' + indx + ' option:selected').text();
        var _logid = Table.fnGetData(nTr)[18]; _lsterrdet.push("ERROR_PROBLEM" + "|" + Str(_errprb)); _lsterrdet.push("ERROR_SOLUTION" + "|" + Str(_errsoln)); _lsterrdet.push("ERROR_STATUS" + "|" + Str(_errstatus));  _lsterrdet.push("LOGID" + "|" + Str(_logid));
    };

    function FillErrorGrid(Table) {
        try
        {
            $('#dataGridErr').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null)
            {
                var t = $('#dataGridErr').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array(); var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str(''); cells[1] = Str(''); cells[2] = Str(Table[i].UPDATE_DATE); cells[3] = Str(Table[i].ServerName); cells[4] = Str(Table[i].PROCESSOR_NAME);
                    cells[5] = Str(Table[i].LogID); cells[6] = Str(Table[i].MODULENAME); cells[7] = Str(Table[i].DocType); cells[8] = Str(Table[i].BUYER_CODE);
                    cells[9] = Str(Table[i].VENDOR_CODE); cells[10] = Str(Table[i].KEYREF2); cells[11] = Str(Table[i].AUDITVALUE); cells[12] = Str(Table[i].FILENAME);
                    cells[13] = Str(Table[i].ERROR_PROBLEM); cells[14] = Str(Table[i].ERROR_SOLUTION); cells[15] = Str(''); cells[16] = Str(Table[i].STATUS);
                    cells[17] = Str(Table[i].PRIORITY_FLAG); cells[18] = Str(Table[i].ERROR_LOGID); cells[19] = Str(Table[i].LogID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else
            {
                if (Table.LogID != undefined && Table.LogID != null)
                {
                    var t = $('#dataGridErr').dataTable(); var r = jQuery('<tr id=' + 1 + '>');var cells = new Array();
                    cells[0] = Str(''); cells[1] = Str(''); cells[2] = Str(Table.UPDATE_DATE); cells[3] = Str(Table.ServerName); cells[4] = Str(Table.PROCESSOR_NAME);
                    cells[5] = Str(Table.LogID); cells[6] = Str(Table.MODULENAME); cells[7] = Str(Table.DocType); cells[8] = Str(Table.BUYER_CODE);
                    cells[9] = Str(Table.VENDOR_CODE); cells[10] = Str(Table.KEYREF2); cells[11] = Str(Table.AUDITVALUE); cells[12] = Str(Table.FILENAME);
                    cells[13] = Str(Table.ERROR_PROBLEM); cells[14] = Str(Table.ERROR_SOLUTION); cells[15] = Str(''); cells[16] = Str(Table.STATUS);
                    cells[17] = Str(Table.PRIORITY_FLAG); cells[18] = Str(Table.ERROR_LOGID); cells[19] = Str(Table.LogID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }            
        }
        catch (e) { }
    };

    var GetErrorGrids = function (_fromlogdate, _tologdate) {
        _errStatus = $('#cbStatus option:selected').val().trim();
       Metronic.blockUI('#portlet_body');
       setTimeout(function () {
           $.ajax({
            type: "POST",
            async: false,
            url: "Errors.aspx/GetErrorsGrid",
            data: "{ 'UPDATE_DATEFROM':'" + getSQLDateFormated(_fromlogdate) + "','UPDATE_DATETO':'" + getSQLDateFormated(_tologdate) + "','ERR_STATUS':'" + _errStatus + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response)
            {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) {  Table = DataSet.NewDataSet.Table; FillErrorGrid(Table);}
                    else $('#dataGridErr').DataTable().clear().draw();
                   Metronic.unblockUI();
                }
                catch (err) {  Metronic.unblockUI();  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Errors :' + err); }  },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI();  },
            error: function (response) { toastr.error("error get", response.responseText);Metronic.unblockUI(); }
       });
       }, 200);
      
    };

    var setFilterToolbar = function () {
        $('#divFilter').empty(); $('#toolbtngroup').empty();
        var _btns = '<div class="row" style="margin-bottom: 2px;padding-right: 20px;"> <div class="col-md-12"><div id="toolbtngroup" >' +
          '<span title="Export to Excel" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"> <a href="javascript:;" class="toolbtn" id="btnExpExcel"><i class="fa fa-file-excel-o" style="padding-left:10px;"></i></a></div>'+
          '<span title="Clear" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnClear"><i class="fa fa-eraser"></i></a></div>' +
          '<span title="Refresh" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"> <a href="javascript:;" class="toolbtn" id="btnRefresh"><i class="fa fa-recycle"></i></a></div></div>' +
           '</div></div>'; $('#toolbtngroup').append(_btns);
           var _filterdet = '<div class="row"> <div class="col-md-12">' +
             '  <div class="dvfilterdet"> <table width="100%"><tbody><tr>' +
             ' <td class="tdtitle td10">Update From</td><td class="tdcontent td20"><input class="form-control date-picker csDatePicker InputText" data-date-format="dd/mm/yyyy" size="16" type="text" id="dtUpdateFromDate" value=""/></td>' +
             ' <td class="tdtitle td10">Update To</td><td class="tdcontent td20"><input class="form-control date-picker csDatePicker InputText" data-date-format="dd/mm/yyyy" size="16" type="text" id="dtUpdateToDate" value=""/></td>' +
             ' <td class="tdtitle td10">Status</td><td class="tdcontent td20"> <select class="select2-container" id="cbStatus" style="width:150px;"><option value=""></option> <option value="0" selected>Pending</option> <option value="1">Completed</option> </select></td>' +
             ' </tr> </tbody> </table></div> </div></div>';
        $('#divFilter').append(_filterdet);  var date = new Date();
        $(document.getElementById('dtUpdateFromDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
        $(document.getElementById('dtUpdateToDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
        _fromlogdate = $(document.getElementById('dtUpdateFromDate')).val(); _tologdate = $(document.getElementById('dtUpdateToDate')).val();

    };

    function ClearFilter() {
        _lstfilter = []; setFilterToolbar(); $('#dataGridErr').DataTable().clear().draw(); $('#dataGridErr_filter input').val('');
        $('#dataGridSolnTemp').DataTable().clear().draw();
    };

    function UpdateErrorDetails(_ofieldval, _nfieldval, callback) {
        var slErrdet = []; var slchDet = [];
        for (var i = 0; i < _ofieldval.length; i++) { slErrdet.push(_ofieldval[i]);} for (var j = 0; j < _nfieldval.length; j++) { slchDet.push(_nfieldval[j]);}
        var data2send = JSON.stringify({ slErrdet: slErrdet, slchDet: slchDet });
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "Errors.aspx/UpdateErrorDetails",
                data: data2send,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d);
                        if (res != "") {
                            toastr.success("Lighthouse eSolutions Pte. Ltd", "Error Details Saved successfully.");
                            _fromlogdate = $(document.getElementById('dtUpdateFromDate')).val(); _tologdate = $(document.getElementById('dtUpdateToDate')).val();
                            callback(_fromlogdate, _tologdate);
                        }
                        Metronic.unblockUI();
                    }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Error details :' + err); Metronic.unblockUI(); }
                },
                failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
                error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
            });
        }, 200);
    };

    function DownloadFile(UPDATEDATE, MODULENAME, FILENAME, SERVERNAME) {
        try { 
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "Errors.aspx/Download",
                    data: "{ 'UPDATEDATE':'" + UPDATEDATE + "','MODULENAME':'" + MODULENAME + "','FILENAME':'" + FILENAME + "','SERVERNAME':'"+SERVERNAME+"' }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        try {
                            var res = Str(response.d);
                            if (res != '0' && res != '') {
                                var filefullpath = res.split('|')[0]; var filename = res.split('|')[1]; //window.open("../Downloads/" + filename); 
                                top.location.href = "../Downloads/" + filename;
                                toastr.success("Lighthouse eSolutions Pte. Ltd", "Download File success");
                            }
                            else { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to download file. Path not found'); }
                        }
                        catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Download file :' + err); }
                    },
                    failure: function (response) { toastr.error("failure get", response.d); },
                    error: function (response) { toastr.error("error get", response.responseText); }
                });
            }, 200);
        }
        catch (e)
        { }
    };

    function fnFormatDetails(oTable, nTr) {
        var sOut = ''; var _str = ''; var _status = ''; var _selectopt = '';
        var indx = nTr.rowIndex; var aData = oTable.fnGetData(nTr);
        var tid = "ErrTable" + indx; var _tbodyid = "tblBodyErr" + indx; var _probid = 'txtErrProblem' + indx; var _statusid = 'cbErrStatus' + indx; var _solnid = 'txtErrSolution' + indx;
        var _errProb = Str(aData[13]); var _errsoln = Str(aData[14]); var _errstatus = Str(aData[16]);
        if (_errstatus == 'Pending') { _status = 0; _selectopt = ' <option value="0" selected="selected">Pending</option><option value="1">Completed</option> </select>'; }
        else if (_errstatus == 'Completed') { _status = 1; _selectopt = ' <option value="0"  selected="selected">Pending</option> <option value="1">Completed</option> </select>'; }
        var btndiv = '<div class="row"><div class="col-md-12" style="text-align:center;"><a id="btnUpdate"><u>Update</u></<a>&nbsp;<a id="btnCancel"><u>Cancel</u></<a></div></div>';
        var sOut = '<div class="row"><div class="col-md-10"><div class="row"><div class="col-md-10"><div class="form-group">' +
        ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Problem </label> </div>' +
        ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _probid + '">' + _errProb + '</textarea> </div>' +
        ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Solution </label> </div>' +
        ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _solnid + '">' + _errsoln + '</textarea></div>' +
        '</div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
        ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Status </label> </div>' +
        ' <div  class="col-md-4"><select class="bs-select form-control"  id="' + _statusid + '">' + _selectopt + '</div>' +
        '</div></div></div>' + btndiv + '</div></div>';
        return sOut;
    }

    function fnMailDetails(ByrCode, SuppCode) {
        var sOut = ''; var _str = ''; var _status = ''; var _selectopt = '';
        var selectedTr = $(this).parents('tr')[0]; var aData = oErrTable.fnGetData(selectedTr);
        var tid = "MailTable"; var _tbodyid = "tblBodyMail"; var _toid = 'txtTo'; var _ccid = 'txtCC'; var _bccid = 'txtBCC'; var _attid = 'txtAttach';
        var _subid = 'txtSubject'; var _msgid = 'txtMessage';
        var _lstdet = GetMailDetails(ByrCode, SuppCode);
        var _To = Str(_lstdet.SUPPLIER_MAIL); var _Cc = Str(_lstdet.CC); var _Bcc = Str(_lstdet.BCC); var _Subject = Str(''); var _Msg = Str(''); var _Attach = Str('');
        var sOut = '<div class="row"><div class="col-md-12"><div class="form-group">' +
           ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> To </label> </div>' +
           ' <div  class="col-md-10"><input type="text" class="form-control" id="' + _toid + '"  value="' + _To + '"/> </div>' +
           ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
           ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Cc </label> </div>' +
           ' <div  class="col-md-10"><input type="text" class="form-control" id="' + _ccid + '"  value="' + _Cc + '"/> </div>' +
           ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
           ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Bcc </label> </div>' +
           ' <div  class="col-md-10"><input type="text" class="form-control" id="' + _bccid + '"  value="' + _Bcc + '"/> </div>' +
           ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
           ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Subject </label> </div>' +
           ' <div  class="col-md-10"><input type="text" class="form-control" id="' + _subid + '"  value="' + _Subject + '"/> </div>' +
             ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
           ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Attachments </label> </div>' +
           ' <div  class="col-md-9"><input type="text" class="form-control" id="' + _attid + '"  value="' + _Attach + '"/> </div>' +
           ' <div  class="col-md-1"><span> <a href="javascript:;" id="btnAttach" data-toggle="tooltip" title="Attachments"><i class="glyphicon glyphicon-paperclip mailicon"></i></a></span> </div>' +
           ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
           ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Message </label> </div>' +
           ' <div  class="col-md-10"><textarea style="width:100%;border:1px solid #c2cad8;" rows="10" id="' + _msgid + '">' + _Msg + '</textarea></div>' +
           '</div></div></div>';
        return sOut;
    }

    var setupTableHeader = function () {
        var dtfilter = '<th>#</th><th></th><th>Log Date</th><th>Server Name</th><th>Processor <br\> Name</th><th>Error No.</th><th>Module</th><th>Doc <br\> Type</th><th>Buyer <br\> Code</th>' +
           ' <th>Supplier <br\> Code</th><th>Key Ref</th><th>Remarks</th><th>File Name</th><th>Problem</th><th>Solution</th><th>Details</th><th>Status</th><th>PriorityFlag</th><th>ERROR_LOGID</th> <th>LOGID</th>';
        $('#tblHeadRowErr').empty(); $('#tblHeadRowErr').append(dtfilter); $('#tblBodyErr').empty();//<div><input type="checkbox" id="cbSelectAll"/></div>
    };

    function GetError_SolModal_Details(oTable, nTr) {
        var aData = oTable.fnGetData(nTr); var _logid = Str(aData[19]); var _errDet = GetSelected_ErrorDetails(_logid).split('|');
        $('#lblErrorNo').text(Str(aData[5])); $('#lblServerName').text(Str(aData[3])); $('#lblProcessorName').text(Str(aData[4]));
        $('#lblModuleName').text(Str(aData[6])); $('#lblDocType').text(Str(aData[7]));
        $('#lblBuyerCode').text(Str(aData[8])); $('#lblSupplierCode').text(Str(aData[9])); $('#lblKeyRef').text(Str(aData[10]));
        $('#lblRemarks').text(Str(_errDet[0])); $('#txtErrDescr').text(Str(_errDet[1])); $('#txtErrProblem').text(Str(_errDet[2]));
        $('#txtErrSolution').text(Str(_errDet[3])); $('#txtErrTemplate').text(Str(_errDet[4])); $('#lblSolutionNo').text(Str(_errDet[5]));
        ResetErrorDetails();$("#ModalErrSolution").modal('show');
    };

    //Solution Templates
    var setupESolTemplate_TableHeader = function () {
        var dtfilter = '<th></th><th>Error <br\> Solution No.</th><th>Error Description</th><th>Error Problem</th><th>Error Solution</th><th>Error Template</th><th>ERROR_ID</th>';       
        $('#tblHeadRowSolnTemp').empty().append(dtfilter); $('#tblBodySolnTemp').empty();
    };

    function SetAccessTabs(id) { if (id == '0') { $('#portlet_bodyErr').show(); $('#prtSolTemp').hide(); } else if (id == '1') { $('#prtSolTemp').show(); $('#portlet_bodyErr').hide(); }};

    function ShowError_SolTempGrid() {
        setupESolTemplate_TableHeader(); var table1 = $('#dataGridSolnTemp');
        oErrSolTempl = table1.dataTable({
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
                "sRowSelect": "single",
                "aButtons": ["select_all", "select_none"]
            },
            "columnDefs": [{
                "orderable": true, "searching": true, "autoWidth": false, 'targets': [0]
            },
            { 'targets': [0], width: '5px' }, { 'targets': [1], width: '90px' }, { 'targets': [2,3,4], width: '110px' },
         { 'targets': [6], visible: false }
            ],
            "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
            "scrollY": '300px',
            "sScrollX": true,
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var divid = 'dv' + nRow._DT_RowIndex;
                var detTag = ' <div id="' + divid + '" style="text-align:center;"><span class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span></div>';
                $('td:eq(0)', nRow).html(detTag);
            }
        });

        $('#tblHeadRowSolnTemp').addClass('gridHeader'); $('#ToolTables_dataGridSolnTemp_0,#ToolTables_dataGridSolnTemp_1').hide();
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
        $('#dataGridSolnTemp').css('width', '100%');
        GetError_SolutionTemplates();
    };

    function GetError_SolutionTemplates() {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "Errors.aspx/GetSolutionTemplate_details",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table; FillError_SolnGrid(Table); }
                    else $('#dataGridSolnTemp').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Error Solution Templates :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };

    function FillError_SolnGrid(Table) {
        try {
            $('#dataGridSolnTemp').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridSolnTemp').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array(); var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str(''); cells[1] = Str(Table[i].ERROR_NO); cells[2] = Str(Table[i].ERROR_DESC); cells[3] = Str(Table[i].ERROR_PROBLEM);
                    cells[4] = Str(Table[i].ERROR_SOLUTION); cells[5] = Str(Table[i].ERROR_TEMPLATE); cells[6] = Str(Table[i].ERROR_ID); 
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.ERROR_ID != undefined && Table.ERROR_ID != null) {
                    var t = $('#dataGridSolnTemp').dataTable(); var r = jQuery('<tr id=' + 1 + '>'); var cells = new Array();
                    cells[0] = Str(''); cells[1] = Str(Table.ERROR_NO); cells[2] = Str(Table.ERROR_DESC); cells[3] = Str(Table.ERROR_PROBLEM);
                    cells[4] = Str(Table.ERROR_SOLUTION); cells[5] = Str(Table.ERROR_TEMPLATE); cells[6] = Str(Table.ERROR_ID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e) { }
    };

    function fnErrSoln_Update(oTable, nTr) {
        var sOut = ''; var _str = ''; var _status = ''; var _selectopt = '';  var indx = nTr.rowIndex; var aData = oTable.fnGetData(nTr);
        var tid = "ErrSolTable" + indx; var _tbodyid = "tblBodySolnTemp" + indx;
        var _probid = 'txtErrProblem' + indx; var _solnid = 'txtErrSolution' + indx; var _errdescrid = 'txtErrDescr' + indx; var _errTempid = 'txtErrTemplate' + indx;
        var _errProb = Str(aData[3]); var _errSoln = Str(aData[4]); var _errDescr = Str(aData[2]); var _errTemp = Str(aData[5]);
        var btndiv = '<div class="row"><div class="col-md-12" style="text-align:center;"><a id="btnErrSUpdate"><u>Update</u></<a>&nbsp;<a id="btnErrSCancel"><u>Cancel</u></<a></div></div>';
        var sOut = '<div class="row"><div class="col-md-12"><div class="row"><div class="col-md-12"><div class="form-group">' +
        ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Error Description </label> </div>' +
        ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _errdescrid + '">' + _errDescr + '</textarea> </div>' +
        ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Error Problem </label> </div>' +
        ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _probid + '">' + _errProb + '</textarea></div>' +
        ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
        ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Error Solution </label> </div>' +
        ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _solnid + '">' + _errSoln + '</textarea> </div>' +
        ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Error Template </label> </div>' +
        ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _errTempid + '">' + _errTemp + '</textarea></div>' +
        ' </div></div></div>' + btndiv + '</div></div>';
        return sOut;
    }

    function GetErrorSol_RowDetails(Table, nTr, _lstErrSdet, _action) {
        var _errprb = ''; var _errsoln = ''; var _errdescr = ''; var _errtemp = ''; var _errorid = '';var _errSolNo='';
        if (_action == "Edit") {
            var indx = nTr.rowIndex; var tid = "ErrSolTable" + indx; _errSolNo = Table.fnGetData(nTr)[1];
            _errprb = $('#txtErrProblem' + indx).val();  _errsoln = $('#txtErrSolution' + indx).val();  _errdescr = $('#txtErrDescr' + indx).val();
             _errtemp = $('#txtErrTemplate' + indx).val();
        }
        else if (_action == 'Details') {
            _errprb = $('#txtErrProblem').val(); _errsoln = $('#txtErrSolution').val(); _errdescr = $('#txtErrDescr').val();
            _errtemp = $('#txtErrTemplate').val(); _errSolNo = $('#lblSolutionNo').text();
        }
        _lstErrSdet.push("ERROR_PROBLEM" + "|" + Str(_errprb)); _lstErrSdet.push("ERROR_SOLUTION" + "|" + Str(_errsoln)); _lstErrSdet.push("ERROR_DESC" + "|" + Str(_errdescr));
        _lstErrSdet.push("ERROR_TEMPLATE" + "|" + Str(_errtemp)); _lstErrSdet.push("ERROR_NO" + "|" + Str(_errSolNo)); 

        UpdateErrorSolution_Details(_lstErrSdet, GetError_SolutionTemplates);
    };

    function UpdateErrorSolution_Details(_nfieldval, callback) {
        var slErrSdet = [];
        for (var i = 0; i < _nfieldval.length; i++) { slErrSdet.push(_nfieldval[i]); }
        var data2send = JSON.stringify({ slErrSdet: slErrSdet });
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "Errors.aspx/UpdateErrorSolution_Details",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") {
                        toastr.success("Lighthouse eSolutions Pte. Ltd", "Error Solution Details Saved successfully.");
                        callback();
                    }
                    Metronic.unblockUI();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Solution Details :' + err); Metronic.unblockUI(); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };

    function GetSelected_ErrorDetails(LOGID) {
        var _result='';
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "Errors.aspx/GetError_Details",
            data: "{'LOGID':'" + LOGID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    _result = Str(response.d);              
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Error Details :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
        return _result;
    };

    function Validate_ErrorDetails()
    {
        var _valid = true; var _errrDescr = $('#txtErrDescr').val(); var _errProb = $('#txtErrProblem').val(); var _errSoln = $('#txtErrSolution').val();
        var _errTemp = $('#txtErrTemplate').val();
        if (_errrDescr == '') { $('#txtErrDescr').css('border', '1px solid #e7505a'); _valid = false; } else { $('#txtErrDescr').css('border', '1px solid #c2cad8'); }
        if (_errProb == '') { $('#txtErrProblem').css('border', '1px solid #e7505a'); _valid = false; } else { $('#txtErrProblem').css('border', '1px solid #c2cad8'); }
        if (_errSoln == '') { $('#txtErrSolution').css('border', '1px solid #e7505a'); _valid = false; } else { $('#txtErrSolution').css('border', '1px solid #c2cad8'); }
        if (_errTemp == '') { $('#txtErrTemplate').css('border', '1px solid #e7505a'); _valid = false; } else { $('#txtErrTemplate').css('border', '1px solid #c2cad8'); }
        return _valid;
    };

    function ResetErrorDetails() {
        $('#txtErrDescr').css('border', '1px solid #c2cad8'); $('#txtErrProblem').css('border', '1px solid #c2cad8'); $('#txtErrSolution').css('border', '1px solid #c2cad8');
        $('#txtErrTemplate').css('border', '1px solid #c2cad8');
    };

    function ExportErrors() {
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "Errors.aspx/ExportExcel",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d);
                        if (res != "0") {
                            top.location.href = '../Downloads/Excel/Error/' + res;
                            toastr.success("Lighthouse eSolutions Pte. Ltd.", 'Error Log Report exported.');
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
   
     return {  init: function () { handleErrorsTable(); }};
}();