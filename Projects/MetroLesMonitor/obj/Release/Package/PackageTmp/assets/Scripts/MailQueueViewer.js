var _fromlogdate = '';var _tologdate = '';var _maildetails = [];var _oMaildet = [];var _nMaildet = [];var _openrow = [];var count = 0;var selectedTr = '';var previousTr = '';

var MailQueueViewer = function () {

    var handleMailQueueViewerTable = function () {
        var nEditing = null; var _ismqpdate = -2; var nNew = false;
        SetupBreadcrumb('Home', 'Home.aspx', 'Viewers', '#', 'Mail Queue Viewer', 'MailQueueViewer.aspx'); $('#pageTitle').empty().append('Mail Queue Viewer');
        $(document.getElementById('lnkViewer')).addClass('active open'); $(document.getElementById('spMailQueue')).addClass('title font-title SelectedColor');
        setupTableHeader(); var _edit ='<u>Edit</u>'; var _delete ='<u>Delete</u>';
        var table = $('#dataGridMailQView');
        var oMQTable = table.dataTable({
            "bDestroy": true,
            "bSort": false,
            "language": {
                "aria": {  "sortAscending": ": activate to sort column ascending",  "sortDescending": ": activate to sort column descending" },
                "emptyTable": "No data available in table","info": "Showing _START_ to _END_ of _TOTAL_ entries",  "infoEmpty": "No entries found",
                "infoFiltered": "(filtered from _MAX_ total entries)","lengthMenu": "Show _MENU_ entries",  "search": "Quick Search:",   "zeroRecords": "No matching records found"
            },
            dom: 'T<"clear">lfrtip',
            tableTools: {    "sRowSelect": "single",  "aButtons": ["select_all", "select_none"]},
            "columnDefs": [{  'orderable': false,"searching": true,  "autoWidth": false,'targets': [0] },
             { 'targets': [0], width: '20px' }, { 'targets': [1], width: '80px' }, {'targets': [2],  width: '50px'},{ 'targets': [3], width: '80px' },
             { 'targets': [4], width: '100px' },{ 'targets': [5], width: '100px' }, { 'targets': [8], width: '150px' },{ 'targets': [11], width: '80px' },
             { 'targets': [6,7,9,10,12],   visible: false }            
            ],
            "lengthMenu": [   [25, 50, 100, -1],  [25, 50, 100, "All"]], "aaSorting": [],  "scrollY": '300px', "sScrollX": '1000px',
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var aedit = 'rwedit' + nRow._DT_RowIndex; var adelete = 'rwdelete' + nRow._DT_RowIndex; var divid = 'dv' + nRow._DT_RowIndex;
                var detTag = ' <div id="' + divid + '" style="text-align:center;"><span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span>' +
                  '<span id=' + adelete + ' class="actionbtn" data-toggle="tooltip" title="Delete"><i class="glyphicon glyphicon-trash"></i></span></div>';
                if (_ismqpdate != nRow._DT_RowIndex) { $('td:eq(0)', nRow).html(detTag); }
                $('td:eq(3),td:eq(5),td:eq(6),td:eq(7)', nRow).addClass('break-det');
            }
        });

        $('#tblHeadRowMailQView').addClass('gridHeader');   $('#ToolTables_dataGridMailQView_0,#ToolTables_dataGridMailQView_1').hide();
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
        GetMailQueueViewerGrid();

        oMQTable.on('click', 'tbody td', function (e) 
        {         
            var selectedTr = $(this).parents('tr')[0];  var aData = oMQTable.fnGetData(selectedTr);
            if (oMQTable.fnIsOpen(selectedTr) && (e.target.className == 'glyphicon glyphicon-pencil')) { oMQTable.fnClose(selectedTr); }
            else
            {
                var divid = 'dv' + selectedTr._DT_RowIndex;
                if (e.target.className == 'glyphicon glyphicon-pencil') {_ismqpdate = selectedTr._DT_RowIndex;
                    if ((previousTr != '') && (oMQTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {
                        var prevdivid = 'dv' + previousTr._DT_RowIndex; oMQTable.fnClose(previousTr); $('#' + prevdivid).show();
                    }
                    if (aData != null) {
                        oMQTable.fnOpen(selectedTr, fnFormatDetails(oMQTable, selectedTr), 'details');  _oMaildet = [];   GetOldMailRowDetails(oMQTable, selectedTr, _oMaildet);
                        $('#' + divid).hide();    previousTr = selectedTr;
                    }
                }
                else if (e.target.className == 'glyphicon glyphicon-trash') {
                    if (confirm('Are you sure ? You want to delete Mail Queue Log ?')) { _oMaildet = [];
                        GetOldMailRowDetails(oMQTable, selectedTr, _oMaildet);   DeleteMailDetails(_oMaildet, GetMailQueueViewerGrid); }   previousTr = selectedTr;
                }
                $('#btnUpdate').click(function () {
                    _nMaildet = []; _ismqpdate = -2;   GetNewMailRowDetails(oMQTable, selectedTr, _nMaildet);  UpdateMailDetails(_oMaildet, _nMaildet, GetMailQueueViewerGrid);
                    $('#' + divid).show(); 
                });
                $('#btnCancel').click(function () {  if (oMQTable.fnIsOpen(selectedTr)){ oMQTable.fnClose(selectedTr);}  $('#' + divid).show(); _ismqpdate = -2; });
            }
        });

        function fnFormatDetails(oTable, nTr) {
            var sOut = '';var _str = '';var _status='';var _selectopt=''; var indx = nTr.rowIndex;
            var aData = oMQTable.fnGetData(nTr);var tid = "MailTable" + indx;var _tbodyid = "tblBodyMLogFView" + indx;var _toid = 'txtTo' + indx;
            var _ccid = 'txtCc' + indx; var _bccid = 'txtBcc' + indx;var _subid = 'txtSub' + indx; var _mbodyid = 'txtMbody' + indx; var _mattachid = 'txtMattach' + indx;var _statusid = 'cbStatus' + indx;
            var _mailto = Str(aData[5]);var _mailcc = Str(aData[6]); var _mailbcc = Str(aData[7]); var _mailsubject = Str(aData[8]);
            var _mailbody = Str(aData[9]); var _mailattach = Str(aData[10]);var _statustxt = Str(aData[11]);
            if (_statustxt == 'Not to send')   { _status = 1; _selectopt = ' <option value="0">In Queue</option><option value="1"  selected="selected">Not to send</option> </select>'; }
            else if (_statustxt == 'In Queue') { _status = 0; _selectopt =' <option value="0"  selected="selected">In Queue</option> <option value="1">Not to send</option> </select>'; }                      
            var btndiv = '<div class="row"><div style="text-align:center;"><a href="#" id="btnUpdate"><u>Update</u></<a>&nbsp;<a href="#" id="btnCancel"><u>Cancel</u></<a></div></div>';
            var sOut = '<div class="row" style="padding-right:63px;"><div class="col-md-10">';
            sOut += '<div class="row"><div class="col-md-10"><div class="form-group">' +
                    ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Mail To </label> </div>' +
                    ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _toid + '"  value="' + _mailto + '"/> </div>' +
                    ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel">Mail CC</label> </div>' +
                    ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _ccid + '"  value="' + _mailcc + '"/> </div>' +
                    ' </div> </div></div> <div class="row"><div class="col-md-10"> <div class="form-group">' +
                    ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Mail BCC </label> </div>' +
                    ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bccid + '"  value="' + _mailbcc + '"/> </div>' +
                    ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Status </label></div>' +
                    ' <div  class="col-md-3"> <select class="bs-select form-control" id="' + _statusid + '" >' + _selectopt + '</select> </div>' +
                    ' </div> </div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
                    ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel">Subject</label> </div>' +
                    ' <div  class="col-md-9"><input type="text" class="form-control" id="' + _subid + '"  value="' + _mailsubject + '"/> </div>' +
                    ' </div></div></div> <div class="row"><div class="col-md-10"><div class="form-group">' +
                    ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Mail Body </label> </div>' +
                    ' <div  class="col-md-9"> <textarea style="width:100%;border:1px solid #c2cad8;" rows="3" id="' + _mbodyid + '">' + _mailbody + '</textarea> </div>' +
                    ' </div></div></div> <div class="row"><div class="col-md-10"> <div class="form-group">' +
                    ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel">Mail Attachments</label> </div>' +
                    ' <div  class="col-md-9"><textarea style="width:100%;border:1px solid #c2cad8;" rows="3" id="' + _mattachid + '">' + _mailattach + '</textarea> </div>' +
                    ' </div></div></div>';
                sOut += btndiv + '</div></div>';
            return sOut;
        }
    };

    function GetOldMailRowDetails(Table, nTr,_lstmaildet) {
        var _mailto = Table.fnGetData(nTr)[5]; var _mailcc = Table.fnGetData(nTr)[6]; var _mailbcc = Table.fnGetData(nTr)[7];
        var _mailsub = Table.fnGetData(nTr)[8]; var _mailbody = Table.fnGetData(nTr)[9]; var _mailattach = Table.fnGetData(nTr)[10];var _mailstatus = Table.fnGetData(nTr)[11];var _queueid = Table.fnGetData(nTr)[12];
        _lstmaildet.push("O_MAILTO" + "|" + Str(_mailto)); _lstmaildet.push("O_MAILCC" + "|" + Str(_mailcc)); _lstmaildet.push("O_MAILBCC" + "|" + Str(_mailbcc));
        _lstmaildet.push("O_MAILSUB" + "|" + Str(_mailsub)); _lstmaildet.push("O_MAILBODY" + "|" + Str(_mailbody)); _lstmaildet.push("O_MAILATTACH" + "|" + Str(_mailattach));
        _lstmaildet.push("O_MAILSTATUS" + "|" + Str(_mailstatus));_lstmaildet.push("O_QUEUEID" + "|" + Str(_queueid));
    };

    function GetNewMailRowDetails(Table, nTr,_lstmaildet) {
        var indx = nTr.rowIndex; var tid = "MailTable" + indx;
        var _mailto = $('#txtTo' + indx).val(); var _mailcc = $('#txtCc' + indx).val(); var _mailbcc = $('#txtBcc' + indx).val(); var _mailsub = $('#txtSub' + indx).val(); var _mailbody = $('#txtMbody' + indx).val(); var _mailattach = $('#txtMattach' + indx).val();
        var _mailstatus = $('#cbStatus' + indx + ' option:selected').text();  var _queueid = Table.fnGetData(nTr)[12];
        _lstmaildet.push("N_MAILTO" + "|" + Str(_mailto));_lstmaildet.push("N_MAILCC" + "|" + Str(_mailcc)); _lstmaildet.push("N_MAILBCC" + "|" + Str(_mailbcc));
        _lstmaildet.push("N_MAILSUB" + "|" + Str(_mailsub)); _lstmaildet.push("N_MAILBODY" + "|" + Str(_mailbody));_lstmaildet.push("N_MAILATTACH" + "|" + Str(_mailattach));
        _lstmaildet.push("N_MAILSTATUS" + "|" + Str(_mailstatus));_lstmaildet.push("N_QUEUEID" + "|" + Str(_queueid));
    };

    function FillMailQueueViewerGrid(Table) {
        try
        {
            $('#dataGridMailQView').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null)
            {
                var t = $('#dataGridMailQView').dataTable();
                for (var i = 0; i < Table.length; i++)
                {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str(''); cells[1] = Str(Table[i].MAILDATE);  cells[2] = Str(Table[i].DOC_TYPE);cells[3] = Str(Table[i].REF_KEY);
                    cells[4] = Str(Table[i].MAIL_FROM); cells[5] = Str(Table[i].MAIL_TO);  cells[6] = Str(Table[i].MAIL_CC); cells[7] = Str(Table[i].MAIL_BCC);
                    cells[8] = Str(Table[i].MAIL_SUBJECT); cells[9] = Str(Table[i].MAIL_BODY); cells[10] = Str(Table[i].ATTACHMENTS); cells[11] = Str(Table[i].STATUS); cells[12] = Str(Table[i].QUEUE_ID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else
            {
                if (Table.QUEUE_ID != undefined && Table.QUEUE_ID != null)
                {
                    var t = $('#dataGridMailQView').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str('');  cells[1] = Str(Table.MAILDATE);   cells[2] = Str(Table.DOC_TYPE); cells[3] = Str(Table.REF_KEY);
                    cells[4] = Str(Table.MAIL_FROM); cells[5] = Str(Table.MAIL_TO); cells[6] = Str(Table.MAIL_CC); cells[7] = Str(Table.MAIL_BCC);
                    cells[8] = Str(Table.MAIL_SUBJECT); cells[9] = Str(Table.MAIL_BODY);cells[10] = Str(Table.ATTACHMENTS); cells[11] = Str(Table.STATUS);cells[12] = Str(Table.QUEUE_ID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    var GetMailQueueViewerGrid = function () {
       Metronic.blockUI('#portlet_body');
       setTimeout(function () { 
       $.ajax({
            type: "POST",
            async: false,
            url: "MailQueueViewer.aspx/GetMailQueueViewerGrid",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response)
            {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null){ Table = DataSet.NewDataSet.Table; FillMailQueueViewerGrid(Table);  }
                    else $('#dataGridMailQView').DataTable().clear().draw();
                   Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI();  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Mail Queue Viewer :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText);Metronic.unblockUI(); }
       });
       }, 200);
    };

    function UpdateMailDetails(_ofieldval, _nfieldval, callback) {
        var slMaildet = [];
        for (var i = 0; i < _ofieldval.length; i++) { slMaildet.push(_ofieldval[i]); }
        for (var j = 0; j < _nfieldval.length; j++) { slMaildet.push(_nfieldval[j]);}
        var data2send = JSON.stringify({ slMaildet: slMaildet });
        Metronic.blockUI('#portlet_body');
        setTimeout(function ()
        {
            $.ajax({
                type: "POST",
                async: false,
                url: "MailQueueViewer.aspx/UpdateMailDetails",
                data: data2send,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d);
                        if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "Mail Details Saved successfully.");   GetMailQueueViewerGrid(); }
                        Metronic.unblockUI();
                    }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Mail details :' + err); Metronic.unblockUI(); }
                },
                failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
                error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
            });
        }, 200);
    };

    function DeleteMailDetails(fieldValues, callback) {
        var slMaildet = [];
        for (var i = 0; i < fieldValues.length; i++) { slMaildet.push(fieldValues[i]);}
        var data2send = JSON.stringify({ slMaildet: slMaildet });
        Metronic.blockUI('#portlet_body');
        setTimeout(function ()
        {
            $.ajax({
                type: "POST",
                async: false,
                url: "MailQueueViewer.aspx/DeleteMailDetails",
                data: data2send,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d);
                        if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "Mail Details Deleted."); callback(); }
                        Metronic.unblockUI();
                    }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete mail :' + err); Metronic.unblockUI(); }
                },
                failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI();},
                error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
            });
        }, 200);
    };

    var setupTableHeader = function () {
        $('#toolbtngroup').empty();
        var dtfilter = '<th class="sorting_disabled" style="text-align:center;">#</th><th>Mail Date</th><th>Doc Type</th><th>Key Ref</th><th>Mail From</th><th>Mail To</th><th>Mail Cc</th><th>Mail Bcc</th><th>Subject</th><th>Mail Body</th><th>Mail Attachments</th><th>Status</th><th>QUEUE_ID</th>';        
        $('#tblHeadRowMailQView').empty().append(dtfilter);$('#tblBodyMailQView').empty();
    };
   
     return {
         init: function () { handleMailQueueViewerTable(); }
    };
}();