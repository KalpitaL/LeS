var _lstDoc = []; var _lstGrpDet = []; var _lstRGrpDet = []; var _lstBuy = []; var _lstSupp = []; var _lstCopyFromGrp = []; var _lstRPendstate = []; var _lstQPCendstate = []; var _lstRuleval = [];
var selectedTr = ''; var _addrtype = '';var previousTr = '';var previousRTr = '';

var Groups = function () {

    var handleGroupsTable = function () {
        var nEditing = null; var _isRuleupdate = -2; var _isGrpupdate = -2; var nNew = false;
        SetupBreadcrumb('Home', 'Home.aspx', 'Groups', 'Groups.aspx', '', ''); $('#pageTitle').empty().append('Groups');
        $(document.getElementById('lnkGroups')).addClass('active open'); _addrtype = Str(sessionStorage.getItem('ADDRTYPE'));
        setupTableHeader();

        var table = $('#dataGridGrp');
        var oGTable = table.dataTable({
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
            dom: 'T<"clear">lftip',           
            tableTools: {
                "sRowSelect": "single",
                "aButtons": ["select_all", "select_none"]
            },
            "columnDefs": [{ 'orderable': false, "searching": true, "autoWidth": false, 'targets': [0]
            },          
             { 'targets': [1], width: '10%' },   { 'targets': [2], width: '13%' }, { 'targets': [3], width: '20%' }, { 'targets': [4], width: '11%' }, { 'targets': [5], width: '13%' },
             { 'targets': [6], width: '11%' }, { 'targets': [7], width: '13%' }, { 'targets': [18], width: '9%' },
             { 'targets': [0,8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 19, 20, 21], visible: false }, { 'targets': [6, 7], "sClass": "visible-lg" }
            ],            
            "lengthMenu": [   [25, 50, 100, -1],  [25, 50, 100, "All"] ],
            "scrollY": '300px',
            "scrollX": "1000px",
            "aaSorting": [],        
            "drawCallback": function (settings, json) {   $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' });   },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {            
                var divid = 'dv' + nRow._DT_RowIndex; var aedit = 'rwdedit' + nRow._DT_RowIndex; var adelete = 'rwdelete' + nRow._DT_RowIndex; var btnid = 'View' + nRow._DT_RowIndex;
                var detTag = ' <div id="' + divid + '" style="text-align:center;"><span id=' + btnid + ' class="actionbtn" data-toggle="tooltip" title="Details"><i class="fa fa-search"></i></a></span>' +
                    '<span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span>'+
                  '<span id=' + adelete + ' class="actionbtn" data-toggle="tooltip" title="Delete"><i class="glyphicon glyphicon-trash"></i></span></div>';
                if (_isGrpupdate != nRow._DT_RowIndex) {  $('td:eq(0)', nRow).html(detTag); }  var lnkTag = '<span><a><u>Link Details</u></<a></span>'; $('td:eq(7)', nRow).html(lnkTag);
            }
        });
        $('#tblHeadRowGrp').addClass('gridHeader'); $('#ToolTables_dataGridGrp_0,#ToolTables_dataGridGrp_1').hide();
        setupRuleTableHeader();

        var table = $('#dataGridlnkRule');
        var oGRTable = table.dataTable({
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
            "columnDefs": [{  'orderable': false,  "searching": true,  "autoWidth": false,'targets': [0]
            },
            { 'targets': [0], width: '10px', 'bSortable': false }, { 'targets': [1], width: '30px' }, { 'targets': [2], width: '15px' },
            { 'targets': [3], width: '75px' },{ 'targets': [4], width: '100px' }, { 'targets': [5], width: '100px' }, { 'targets': [6], width: '30px' },
            {'targets': [7,8],  visible: false}
            ],
            "lengthMenu": [  [25, 50, 100, -1], [25, 50, 100, "All"] ],
            "sScrollY": '300px',
            "sScrollX": '900px',
            "initComplete": function(settings, json) {  $('.dataTables_scrollBody thead tr').css({visibility:'collapse'}); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var divid = 'dv' + nRow._DT_RowIndex; var aedit = 'rwdedit' + nRow._DT_RowIndex;
                var detTag = ' <div id="' + divid + '" style="text-align:center;"><span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span></div>';
                if (_isRuleupdate != nRow._DT_RowIndex) { $('td:eq(0)', nRow).html(detTag); }
            }
        });
        $('#tblHeadRowlnkRule').addClass('gridHeader');  $('#ToolTables_dataGridlnkRule_0,#ToolTables_dataGridlnkRule_1').hide();
    
        setupLinkedBSTableHeader();
        var table = $('#dataGridBSlnk');
        var oBSLTable = table.dataTable({
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
            "columnDefs": [{'orderable': false,  "searching": true, "autoWidth": false,   'targets': [0]
            },          
            {'targets': [4],  visible: false  }
            ],
            "lengthMenu": [ [25, 50, 100, -1],  [25, 50, 100, "All"] ],
            "pageLength": 10,
            "aaSorting":[]
        });

        $('#tblHeadRowBSlnk').addClass('gridHeader');  $('#ToolTables_dataGridBSlnk_0,#ToolTables_dataGridBSlnk_1').hide();$('#dataGridBSlnk').css('width', '100%');
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
        GetGroupsGrid(); FillEndStateCombos(); _lstRuleval = []; _lstRuleval.push("0|0");  _lstRuleval.push("1|1");
        //$('.dataTables_scrollHeadInner table').on('click', 'thead tr', function (e) {
        //    e.preventDefault(); if (e.target.innerText == 'New') {    $('#dvNewDet').empty(); var divtag = fnheaderFormatDetails(oGTable);
        //        $('#dvNewDet').append(divtag); $("#ModalNew").modal('show');
        //    }
        //});

        oGTable.on('click', 'tbody td', function (e) {
            var selectedTr = $(this).parents('tr')[0];   var aData = oGTable.fnGetData(selectedTr); var cellindx = $(this).index();    var  updtcnt = 0;
            if (oGTable.fnIsOpen(selectedTr) && (e.target.className == 'glyphicon glyphicon-pencil')) { oGTable.fnClose(selectedTr);}
            else {
                var divid = 'dv' + selectedTr._DT_RowIndex;
                if (e.target.className == 'glyphicon glyphicon-pencil') {
                    _isGrpupdate=selectedTr._DT_RowIndex;
                    if ((previousTr != '') && (oGTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {
                        var prevdivid = 'dv' + previousTr._DT_RowIndex;  oGTable.fnClose(previousTr);   $('#' + prevdivid).show();
                    }
                    if (aData != null) {   oGTable.fnOpen(selectedTr, fnFormatDetails(oGTable, selectedTr), 'details'); $('#' + divid).hide(); previousTr = selectedTr; }
                }
                if (e.target.className == 'glyphicon glyphicon-trash') {
                    if (confirm('Are you sure? You want to delete this group ? All mappings related to this group will be deleted.')) {
                        var _grpid = aData[21];   DeleteGroup(_grpid, GetGroupsGrid); }
                }
                else if (e.target.innerText == 'Link Details') {
                    var _grpid = oGTable.fnGetData(selectedTr)[21];  var _code = oGTable.fnGetData(selectedTr)[2]; GetLinkedBSGrpGrid(_grpid);
                    $('#Lnkdettitle').text('Link Details for Group : ' + _code); $("#ModalLnkBuySupp").modal('show');
                }
                if (e.target.className == 'opentd' || e.target.className == 'fa fa-search') {
                    var _grpid = aData[21];     var _code = aData[2]; GetGRulesGrid(_grpid);
                    $('#Lnkdettitle').text('Linked Rules for Group : ' + _code); $("#ModalLnkRules").modal('show');
                }

                $('#btnUpdate').click(function () {
                    _lstGrpDet = [];                    
                    if (aData != null && updtcnt == 0) {
                        var _grpid = aData[21];   var _res = ValidateGroup(selectedTr.rowIndex, _grpid);
                        if (_res == true) {  GetGroupDetails(oGTable, selectedTr, _lstGrpDet, 'Edit'); SaveGroup(_lstGrpDet, GetGroupsGrid);   updtcnt++;  }
                    }
                    $('#' + divid).show(); _isGrpupdate = -2;
                });

                $('#btnCancel').click(function () { if (oGTable.fnIsOpen(selectedTr)) {    oGTable.fnClose(selectedTr);  } $('#' + divid).show(); _isGrpupdate = -2;  });
            }
        });

        oGRTable.on('click', 'tbody td', function (e) {
            var selectedTr = $(this).parents('tr')[0]; var aData = oGRTable.fnGetData(selectedTr); var divid = 'dv' + selectedTr._DT_RowIndex;
            if (e.target.className == 'glyphicon glyphicon-pencil') {
                _isRuleupdate = selectedTr._DT_RowIndex;
                if (previousRTr != '' && previousRTr != selectedTr) { restoreRow(previousRTr, oGRTable); } nNew = false;  editRow(oGRTable, selectedTr);  previousRTr = selectedTr;
            }
            else if (e.target.className == 'glyphicon glyphicon-floppy-save') {
                var _hidvalue = '0'; var _ruleid = '0'; _lstGRrpDet = [];
                var _grpid = aData[7]; var _ruleid = aData[8]; var _ruleval = $('#cboRuleVal option:selected').val();
                _lstGRrpDet.push('GROUPID' + "|" + Str(_grpid));  _lstGRrpDet.push('RULEID' + "|" + Str(_ruleid)); _lstGRrpDet.push('RULE_VALUE' + "|" + Str(_ruleval));
                var _res = ValidateRule(_ruleval);
                if (_res == true) {
                    var _msg = 'Are you sure you want to override this rule for all the link for the Buyer/Supplier having inherited (Yes) ?';
                    if (confirm(_msg)) { _hidvalue = '1'; } else { _hidvalue = '0'; } _lstGRrpDet.push('HID_VALUE' + "|" + _hidvalue);
                    SaveGroupRule(_lstGRrpDet, GetGRulesGrid, _grpid);                 
                }
                else { }
                previousRTr = ''; _newcont = 0; _isRuleupdate = -2;
            }
            else if (e.target.className == 'glyphicon glyphicon-ban-circle') {
                _isRuleupdate = -2;  gridCancelEdit(nNew, nEditing, oGRTable);   previousRTr = '';  _newcont = 0;
            }
        });

        function editRow(oTable, nRow) {
            var aData = oTable.fnGetData(nRow); nEditing = nRow;   var jqTds = $('>td', nRow);
            var detTag = '<div style="text-align:center;"><span class="actionbtn" data-toggle="tooltip" title="Update"><i class="glyphicon glyphicon-floppy-save"></i></span>' +
           ' <span class="actionbtn" data-toggle="tooltip" title="Cancel"><i class="glyphicon glyphicon-ban-circle"></i></span></div>';
            jqTds[0].innerHTML = detTag;
            jqTds[6].innerHTML = '<select id="cboRuleVal" class="fullWidth">' + FillCombo(aData[6], _lstRuleval) + '</select>'; $("#cboRuleVal").select2();
        };

        function newRow() {
            var aiNew = oBSTable.fnAddData(['', '', '', '', '', '', '', '', '']); var nRow = oBSTable.fnGetNodes(aiNew[0]); editRow(oBSTable, nRow);nEditing = nRow; nNew = true;
        };

        function restoreRow(nRow, oTable) {
            var aData = oTable.fnGetData(nRow); var jqTds = $('>td', nRow);
            for (var i = 0, iLen = jqTds.length; i < iLen; i++) { if (i == 0) { } else { oTable.fnUpdate(jqTds[i].innerHTML, nRow, i, false); } }
            oTable.fnDraw();
        };

        function gridCancelEdit(nNew, nEditing, oTable) {
            var nRow = $('.DTTT_selected');
            if (nNew) {  oTable.fnDeleteRow(nEditing); nNew = false;  } else {  nRow = nEditing;  restoreRow(nRow, oTable); $(nRow).removeClass("DTTT_selected selected");}
        };

        function fnFormatDetails(oTable, nTr) {
            var sOut = '';var _str = ''; var _newdet = '';  var _rfqchecked = ''; var _quotechecked = ''; var _pochecked = ''; var _pocchecked = '';
            var indx = nTr.rowIndex; var aData = oGTable.fnGetData(nTr);
            var tid = "GroupTable" + indx;  var _tbodyid = "tblBodyGrp" + indx; var _codeid = 'txtCode' + indx;  var _grpdescid = 'txtDescr' + indx;
            var _bimppathid = 'txtBuyImpPath' + indx; var _bexppathid = 'txtBuyExpPath' + indx;  var _simppathid = 'txtSuppImpPath' + indx;  var _sexppathid = 'txtSuppExpPath' + indx;
            var _rfqid = 'chkRFQProcess' + indx; var _quoteid = 'chkQuoteProcess' + indx; var _poid = 'chkPOProcess' + indx; var _pocid = 'chkPOCProcess' + indx;
            var _rfqendid = 'cbRFQEndState' + indx;  var _quoteendid = 'cbQuoteEndState' + indx; var _poendid = 'cbPOEndState' + indx;  var _pocendid = 'cbPOCEndState' + indx;         
            var _code = Str(aData[2]); var _grpdesc = Str(aData[3]);
            var _bImppath = Str(aData[4]);   var _bExppath = Str(aData[6]);  var _sImppath = Str(aData[7]);  var _sExpppath = Str(aData[5]);
            var _rfqprocess = Str(aData[10]); var _quoteprocess = Str(aData[11]); var _poprocess = Str(aData[12]);  var _pocprocess = Str(aData[13]);
            if (_rfqprocess == '1') { _rfqchecked = 'checked'; } else { _rfqchecked = ''; }
            if (_quoteprocess == '1') { _quotechecked = 'checked'; } else { _quotechecked = ''; }
            if (_poprocess == '1') { _pochecked = 'checked'; } else { _pochecked = ''; }
            if (_pocprocess == '1') { _pocchecked = 'checked'; } else { _pocchecked = ''; }
            var _rfqendstate = FillCombo(Str(aData[14]), _lstRPendstate, true); var _quoteendstate = FillCombo(Str(aData[15]), _lstQPCendstate, true);
            var _poendstate = FillCombo(Str(aData[16]), _lstRPendstate, true); var _pocendstate = FillCombo(Str(aData[17]), _lstQPCendstate, true);
            var _grpid = Str(aData[11]);
            var btndiv = '<div style="text-align:center;"><a href="#" id="btnUpdate"><u>Update</u></<a>&nbsp;<a href="#" id="btnCancel"><u>Cancel</u></<a></div>';
            var sOut = '<div class="row" style="padding-right:63px;"><div class="col-md-10">';
             sOut += '<div class="row"><div class="col-md-10">' +
                     ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Code </label> </div>' +
                     ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _codeid + '"  value="' + _code + '"/> </div>' +
                     ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel">Description</label> </div>' +
                     ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _grpdescid + '"  value="' + _grpdesc + '"/> </div>' +
                     '</div></div> <div class="row"><div class="col-md-10">' +
                     ' <div class="col-md-3" style="text-align:right;margin-top:5px;"> <label class="dvLabel">Buyer Import Format</label> </div>' +
                     ' <div  class="col-md-3" ><input type="text" class="form-control" id="' + _bimppathid + '"  value="' + _bImppath + '"/> </div>' +
                     ' <div class="col-md-3" style="text-align:right;margin-top:5px;"> <label class="dvLabel">Buyer Export Format </div>' +
                     ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bexppathid + '"  value="' + _bExppath + '"/> </div>' +
                     '</div></div> <div class="row"><div class="col-md-10">' +
                     ' <div class="col-md-3"  style="text-align:right;margin-top:5px;"> <label class="dvLabel">Supplier Import Format</label> </div>' +
                     ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _simppathid + '"  value="' + _sImppath + '"/> </div>' +
                     ' <div class="col-md-3" style="text-align:right;margin-top:5px;"> <label class="dvLabel">Supplier Export Format</label> </div>' +
                     ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _sexppathid + '" value="' + _sExpppath + '"/> </div>' +
                     '</div></div> <div class="row"><div class="col-md-10">' +
                     ' <div class="col-md-3" style="text-align:right;margin-top:5px;"> <label class="dvLabel">RFQ Process </label></div>' +
                     ' <div  class="col-md-3"> <input type="checkbox" class="widelarge" style="margin-left:2px;" id="' + _rfqid +  '"  value="' + _rfqprocess + '" ' + _rfqchecked + '/> </div>' +
                     ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Quote Process</label> </div>' +
                     ' <div  class="col-md-3"><input type="checkbox" class="widelarge"  style="margin-left:2px;" id="' + _quoteid +  '"  value="' + _quoteprocess + '" ' + _quotechecked + '/></div>' +
                     '</div></div> <div class="row"><div class="col-md-10">' +
                     ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> PO Process </label></div>' +
                     ' <div  class="col-md-3"> <input type="checkbox" class="widelarge"  style="margin-left:2px;" id="' + _poid+  '"  value="' + _poprocess + '" ' + _pochecked + '/> </div>' +
                     ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> POC Process</label> </div>' +
                     ' <div  class="col-md-3"><input type="checkbox" class="widelarge"  style="margin-left:2px;" id="' + _pocid +  '"  value="' + _pocprocess + '" ' + _pocchecked + '/></div>' +
                     '</div></div> <div class="row"><div class="col-md-10">' +
                     ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> RFQ End State</label> </div>' +
                     ' <div  class="col-md-3"> <select class="bs-select form-control" id="' + _rfqendid + '">' + _rfqendstate + '</select> </div>' +
                     ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Quote End State</label> </div>' +
                     ' <div  class="col-md-3"><select class="bs-select form-control"  id="' + _quoteendid + '">' + _quoteendstate + '</select></div>' +
                     '</div></div> <div class="row"><div class="col-md-10">' +
                     ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> PO End State</label> </div>' +
                     ' <div  class="col-md-3"> <select class="bs-select form-control"  id="' + _poendid + '">' + _poendstate + '</select> </div>' +
                     ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> POC End State</label> </div>' +
                     ' <div  class="col-md-3"><select class="bs-select form-control" id="' + _pocendid + '">' + _pocendstate + '</select></div>' +
                     '</div></div>';
             sOut += btndiv + '</div></div>';
            return sOut;
        }

        function fnheaderFormatDetails(oTable) {
            var sOut = ''; var _str = ''; var indx = 0;   var btndiv = '';
            var _code = ''; var _grpdesc = ''; var _bImppath = ''; var _bExppath = ''; var _sImppath = ''; var _sExpppath = '';
            var _copyfrom = ''; var _buyer = ''; var _supplier = ''; var _newbuysupp = ''; var _rfqprocess = ''; var _quoteprocess = '';
            var _poprocess = ''; var _pocprocess = ''; var _rfqendstate = ''; var _quoteendstate = ''; var _poendstate = ''; var _pocendstate = ''; var _grpid = '';           
            var tid = "GroupTable" + indx;  var _tbodyid = "tblBodyGrp" + indx;
            var _codeid = 'txtCode' + indx;    var _grpdescid = 'txtDescr' + indx;   var _bimppathid = 'txtBuyImpPath' + indx;  var _bexppathid = 'txtBuyExpPath' + indx;
            var _simppathid = 'txtSuppImpPath' + indx; var _sexppathid = 'txtSuppExpPath' + indx;  var _rfqid = 'chkRFQProcess' + indx;  var _quoteid = 'chkQuoteProcess' + indx;
            var _poid = 'chkPOProcess' + indx;   var _pocid = 'chkPOCProcess' + indx;  var _rfqendid = 'cbRFQEndState' + indx;  var _quoteendid = 'cbQuoteEndState' + indx;
            var _poendid = 'cbPOEndState' + indx; var _pocendid = 'cbPOCEndState' + indx; var _copyfromid = 'cbCopyFrom' + indx;
            var _buyerid = 'cbBuyer' + indx; var _supplierid = 'cbSupplier' + indx; var _newbsuppid = 'chkNewBuyerSupp' + indx;
            FillSuppliers(); FillBuyers(); FillCopyFromGrp(); _rfqendstate = FillCombo('', _lstRPendstate, true);  _quoteendstate = FillCombo('', _lstQPCendstate, true);
            _poendstate = FillCombo('', _lstRPendstate, true); _pocendstate = FillCombo('', _lstQPCendstate, true);  _copyfrom = FillCombo('', _lstCopyFromGrp, false);
            _buyer = FillCombo('', _lstBuy, false); _supplier = FillCombo('', _lstSupp, false);
            var sOut = '<div class="row"><div class="col-md-12">' +
                       ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label> Code </label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _codeid + '"  value="' + _code + '"/> </div>' +
                       ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label>Description</label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _grpdescid + '"  value="' + _grpdesc + '"/> </div>' +
                       '</div></div> <div class="row"><div class="col-md-12">' +
                       ' <div class="col-md-3" style="text-align:right;margin-top:5px;"> <label>Buyer Import Format</label> </div>' +
                       ' <div  class="col-md-3" ><input type="text" class="form-control" id="' + _bimppathid + '"  value="' + _bImppath + '"/> </div>' +
                       ' <div class="col-md-3" style="text-align:right;margin-top:5px;"> <label>Buyer Export Format </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bexppathid + '"  value="' + _bExppath + '"/> </div>' +
                       '</div></div> <div class="row"><div class="col-md-12">' +
                       ' <div class="col-md-3"  style="text-align:right;margin-top:5px;"> <label>Supplier Import Format</label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _simppathid + '"  value="' + _sImppath + '"/> </div>' +
                       ' <div class="col-md-3" style="text-align:right;margin-top:5px;"> <label>Supplier Export Format</label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _sexppathid + '" value="' + _sExpppath + '"/> </div>' +
                       '</div></div> <div class="row"><div class="col-md-12">' +
                       ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label> Copy From </label></div>' +
                       ' <div  class="col-md-3"><select class="bs-select form-control" id="' + _copyfromid + '">' + _copyfrom + '</select> </div>' +
                       ' <div class="col-md-3" style="text-align:right;margin-top:5px;"> <label>Create New Buyer-Supplier Link </div>' +
                       ' <div  class="col-md-3"><input type="checkbox" class="widelarge" style="margin-left:2px;" id="' + _newbsuppid + '"  /> </div>' +
                       '</div></div> <div class="row"><div class="col-md-12">' +
                       ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label> Buyer </label></div>' +
                       ' <div  class="col-md-3"> <select class="bs-select form-control" id="' + _buyerid + '">' + _buyer + '</select> </div>' +
                       ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label > Supplier </div>' +
                       ' <div  class="col-md-3"><select class="bs-select form-control"  id="' + _supplierid + '">' + _supplier + '</select></div>' +
                       '</div></div> <div class="row"><div class="col-md-12">' +
                       ' <div class="col-md-3" style="text-align:right;margin-top:5px;"> <label >RFQ Process </label></div>' +
                       ' <div  class="col-md-3"> <input type="checkbox" class="widelarge" style="margin-left:2px;" id="' + _rfqid + '" /> </div>' +
                       ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label > Quote Process</label> </div>' +
                       ' <div  class="col-md-3"><input type="checkbox" class="widelarge"  style="margin-left:2px;" id="' + _quoteid + '" /></div>' +
                       '</div></div> <div class="row"><div class="col-md-12">' +
                       ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label > PO Process </label></div>' +
                       ' <div  class="col-md-3"> <input type="checkbox" class="widelarge"  style="margin-left:2px;" id="' + _poid + '" /> </div>' +
                       ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label > POC Process</label> </div>' +
                       ' <div  class="col-md-3"><input type="checkbox" class="widelarge"  style="margin-left:2px;" id="' + _pocid + '" /></div>' +
                       '</div></div> <div class="row"><div class="col-md-12">' +
                       ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label > RFQ End State</label> </div>' +
                       ' <div  class="col-md-3"> <select class="bs-select form-control" id="' + _rfqendid + '">' + _rfqendstate + '</select> </div>' +
                       ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label > Quote End State</label> </div>' +
                       ' <div  class="col-md-3"><select class="bs-select form-control"  id="' + _quoteendid + '">' + _quoteendstate + '</select></div>' +
                       '</div></div> <div class="row"><div class="col-md-12">' +
                       ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label> PO End State</label> </div>' +
                       ' <div  class="col-md-3"> <select class="bs-select form-control"  id="' + _poendid + '">' + _poendstate + '</select> </div>' +
                       ' <div class="col-md-3" style="text-align:right;margin-top:5px;"><label> POC End State</label> </div>' +
                       ' <div  class="col-md-3"><select class="bs-select form-control" id="' + _pocendid + '">' + _pocendstate + '</select></div>' +
                       '</div></div>';
            return sOut;
        }

        $('#btnGrpNew').click(function () {
            _lstGrpDet = [];  var indx = 0; var _res = ValidateGroup(indx, 0);
            if (_res == true) {  GetGroupDetails(oGTable, '', _lstGrpDet, 'New');   SaveGroup(_lstGrpDet, GetGroupsGrid);   $("#ModalNew").modal('hide'); } });

        $("#ModalLnkRules").on('shown.bs.modal', function () { oGRTable.fnDraw(); });


        $('#btnRefresh').live('click', function (e) { e.preventDefault(); GetGroupsGrid(); });
        $('#btnClear').live('click', function (e) { e.preventDefault(); ClearFilter(); });
        $('#btnNewGroup').live('click', function (e) {
            e.preventDefault();
                $('#dvNewDet').empty(); var divtag = fnheaderFormatDetails(oGTable); $('#dvNewDet').append(divtag); $("#ModalNew").modal('show');            
        });
    };

    function GetGroupDetails(Table, nTr, _lstdet, _targetclick) {
        var indx = ''; indx =  (nTr != '')?nTr.rowIndex:0; var tid = "GroupTable" + indx;   
        var _code = $('#txtCode' + indx).val(); var _grpdesc = $('#txtDescr' + indx).val();  var _bimppath = $('#txtBuyImpPath' + indx).val();  var _bexppath = $('#txtBuyExpPath' + indx).val();
        var _simppath = $('#txtSuppImpPath' + indx).val(); var _sexppath = $('#txtSuppExpPath' + indx).val();
        var _rfq = ($('#chkRFQProcess' + indx).is(':checked'))?1:0;  var _quote = ($('#chkQuoteProcess' + indx).is(':checked'))?1:0;
        var _po = ($('#chkPOProcess' + indx).is(':checked'))?1:0;   var _poc = ($('#chkPOCProcess' + indx).is(':checked')) ? 1 : 0;
        var _rfqend = ($('#cbRFQEndState' + indx).val() !='--NA--')?$('#cbRFQEndState' + indx).val():0;
        var _quoteend = ($('#cbQuoteEndState' + indx).val() != '--NA--') ? $('#cbQuoteEndState' + indx).val() : 0;
        var _poend = ($('#cbPOEndState' + indx).val() != '--NA--') ? $('#cbPOEndState' + indx).val() : 0;
        var _pocend = ($('#cbPOCEndState' + indx).val() != '--NA--') ? $('#cbPOCEndState' + indx).val() : 0;
        _lstdet.push("GROUP_CODE" + "|" + Str(_code)); _lstdet.push("GROUP_DESC" + "|" + Str(_grpdesc));   _lstdet.push("BUYER_FORMAT" + "|" + Str(_bimppath));   
        _lstdet.push("BUYER_EXPORT_FORMAT" + "|" + Str(_bexppath));   _lstdet.push("SUPPLIER_FORMAT" + "|" + Str(_simppath));  _lstdet.push("SUPPLIER_EXPORT_FORMAT" + "|" + Str(_sexppath));
        _lstdet.push("RFQ" + "|" + Str(_rfq));  _lstdet.push("QUOTE" + "|" + Str(_quote)); _lstdet.push("PO" + "|" + Str(_po));  _lstdet.push("POC" + "|" + Str(_poc));
        _lstdet.push("RFQ_END_STATE" + "|" + Str(_rfqend));  _lstdet.push("QUOTE_END_STATE" + "|" + Str(_quoteend));   _lstdet.push("PO_END_STATE" + "|" + Str(_poend));  _lstdet.push("POC_END_STATE" + "|" + Str(_pocend));
        if (_targetclick == 'Edit') {  var _grpid = Table.fnGetData(nTr)[21];  _lstdet.push("GROUPID" + "|" + Str(_grpid)); }
        else {
            var _copyfrom = $('#cbCopyFrom' + indx).val(); var _buyer = $('#cbBuyer' + indx).val();  var _supplier = $('#cbSupplier' + indx).val();var _newbsupp = ($('#chkNewBuyerSupp' + indx).is(':checked'))?1:0;
            _lstdet.push("COPY_FROM_GROUP" + "|" + Str(_copyfrom));  _lstdet.push("NEW_LINK" + "|" + Str(_newbsupp));_lstdet.push("BUYER" + "|" + Str(_buyer)); _lstdet.push("SUPPLIER" + "|" + Str(_supplier));
        }
    };

    function FillGroupsGrid(Table) {
        try {
            $('#dataGridGrp').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridGrp').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();                  
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str(''); cells[1] = Str(''); cells[2] = Str(Table[i].GROUP_CODE); cells[3] = Str(Table[i].GROUP_DESC);
                    cells[4] = Str(Table[i].BUYER_FORMAT);  cells[5] = Str(Table[i].SUPPLIER_FORMAT);cells[6] = Str(Table[i].BUYER_EXPORT_FORMAT);   cells[7] = Str(Table[i].SUPPLIER_EXPORT_FORMAT);                   
                    cells[8] = Str(Table[i].BUYER); cells[9] = Str(Table[i].SUPPLIER); cells[10] = Str(Table[i].RFQ);cells[11] = Str(Table[i].QUOTE);                    
                    cells[12] = Str(Table[i].PO);cells[13] = Str(Table[i].POC); cells[14] = Str(Table[i].RFQ_END_STATE);cells[15] = Str(Table[i].QUOTE_END_STATE);
                    cells[16] = Str(Table[i].PO_END_STATE); cells[17] = Str(Table[i].POC_END_STATE); cells[18] = Str(''); cells[19] = Str(Table[i].COPY_FROM_GROUP);
                    cells[20] = Str(''); cells[21] = Str(Table[i].GROUP_ID);
                    var ai = t.fnAddData(cells, false);
           }
                t.fnDraw(); 
          }
            else {
                if (Table.GROUP_ID != undefined && Table.GROUP_ID != null) {
                    var t = $('#dataGridGrp').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');                 
                    var cells = new Array();
                    cells[0] = Str(''); cells[1] = Str(''); cells[2] = Str(Table.GROUP_CODE); cells[3] = Str(Table.GROUP_DESC);
                    cells[4] = Str(Table.BUYER_FORMAT); cells[5] = Str(Table.SUPPLIER_FORMAT); cells[6] = Str(Table.BUYER_EXPORT_FORMAT); cells[7] = Str(Table.SUPPLIER_EXPORT_FORMAT);                
                    cells[8] = Str(Table.BUYER);  cells[9] = Str(Table.SUPPLIER);  cells[10] = Str(Table.RFQ); cells[11] = Str(Table.QUOTE);cells[12] = Str(Table.PO);                    
                    cells[13] = Str(Table.POC);  cells[14] = Str(Table.RFQ_END_STATE); cells[15] = Str(Table.QUOTE_END_STATE); cells[16] = Str(Table.PO_END_STATE);
                    cells[17] = Str(Table.POC_END_STATE); cells[18] = Str(''); cells[19] = Str(Table.COPY_FROM_GROUP); cells[20] = Str('');   cells[21] = Str(Table.GROUP_ID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
             }
           }
        }
        catch (e)
        { }
   };

    var GetGroupsGrid = function (){
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "Groups.aspx/GetGroupGrid",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) {Table = DataSet.NewDataSet.Table; FillGroupsGrid(Table);}  else $('#dataGridGrp').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) {  Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Groups :' + err); }
            },
            failure: function (response) {  toastr.error("failure get", response.d); Metronic.unblockUI();  },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
        }, 180);
    };

    function FillGRulesGrid(Table) {
        try {
            $('#dataGridlnkRule').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridlnkRule').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    var _arrcommt = Str(Table[i].RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str('');  cells[1] = Str(Table[i].RULE_NUMBER);  cells[2] = Str(Table[i].DOC_TYPE); cells[3] = Str(Table[i].RULE_CODE);
                    cells[4] = Str(Table[i].RULE_DESC); cells[5] = _arrcommt; cells[6] = Str(Table[i].RULE_VALUE); cells[7] = Str(Table[i].GROUP_ID);
                    cells[8] = Str(Table[i].RULEID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.RULEID != undefined && Table.RULEID != null) {
                    var t = $('#dataGridlnkRule').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    var _arrcommt = Str(Table.RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str(''); cells[1] = Str(Table.RULE_NUMBER); cells[2] = Str(Table.DOC_TYPE); cells[3] = Str(Table.RULE_CODE);
                    cells[4] = Str(Table.RULE_DESC); cells[5] = _arrcommt;  cells[6] = Str(Table.RULE_VALUE); cells[7] = Str(Table.GROUP_ID);  cells[8] = Str(Table.RULEID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    var GetGRulesGrid = function (GROUPID) {
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "Groups.aspx/GetRules",
            data: "{'GROUPID':'" + GROUPID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) {Table = DataSet.NewDataSet.Table;   FillGRulesGrid(Table);} else $('#dataGridlnkRule').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) {  Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Rules :' + err);  }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) {toastr.error("error get", response.responseText); Metronic.unblockUI();}
        });
    }, 20);
    };

    function FillLinkedBSuppgrpGrid(Table) {
        try {
            $('#dataGridBSlnk').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridBSlnk').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str(Table[i].BUYER_CODE); cells[1] = Str(Table[i].BUYER_NAME); cells[2] = Str(Table[i].SUPPLIER_CODE);  cells[3] = Str(Table[i].SUPPLIER_NAME);
                    cells[4] = Str(Table[i].LINKID);               
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.LINKID != undefined && Table.LINKID != null) {
                    var t = $('#dataGridBSlnk').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str(Table.BUYER_CODE); cells[1] = Str(Table.BUYER_NAME); cells[2] = Str(Table.SUPPLIER_CODE); cells[3] = Str(Table.SUPPLIER_NAME);
                    cells[4] = Str(Table.LINKID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    var GetLinkedBSGrpGrid = function (GROUPID) {
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "Groups.aspx/GetLinkedBuyerSupplier_Group",
            data: "{'GROUPID':'"+GROUPID +"'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table;   FillLinkedBSuppgrpGrid(Table); }  else $('#dataGridBSlnk').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI();  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Linked Buyers/Suppliers :' + err);}
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
        }, 20);
    };

    function FillBuyers() {
        $.ajax({
            type: "POST",
            async: false,
            url: "Groups.aspx/FillBuyers",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var Dataset = JSON.parse(response.d);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table;  _lstRules = [];
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) {
                                for (var i = 0; i < Table.length; i++) { _lstBuy.push(Str(Table[i].ADDRESSID) + "|" + Str(Table[i].ADDR_NAME)); }
                            }
                            else { if (Table.ADDRESSID != undefined) {  _lstBuy.push(Str(Table.ADDRESSID) + "|" + Str(Table.ADDR_NAME));  } }
                        }
                    }
                }
                catch (err) {
                    toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd");
                }
            },
            failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    function FillSuppliers() {
        $.ajax({
            type: "POST",
            async: false,
            url: "Groups.aspx/FillSuppliers",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var Dataset = JSON.parse(response.d);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table;_lstSupp = [];
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) {
                                for (var i = 0; i < Table.length; i++) {_lstSupp.push(Str(Table[i].ADDRESSID) + "|" + Str(Table[i].ADDR_NAME)); }
                            }
                            else {if (Table.ADDRESSID != undefined) {   _lstSupp.push(Str(Table.ADDRESSID) + "|" + Str(Table.ADDR_NAME)); }}
                        }
                    }
                }
                catch (err) { toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd");  }
            },
            failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    function FillCopyFromGrp() {
        $.ajax({
            type: "POST",
            async: false,
            url: "Groups.aspx/FillCopyFromGroups",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var Dataset = JSON.parse(response.d);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table; _lstCopyFromGrp = [];
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) {
                                for (var i = 0; i < Table.length; i++) { _lstCopyFromGrp.push(Str(Table[i].GROUP_ID) + "|" + Str(Table[i].GROUP_CODE)); }
                            }
                            else { if (Table.GROUP_ID != undefined) { _lstCopyFromGrp.push(Str(Table.GROUP_ID) + "|" + Str(Table.GROUP_CODE));} }
                        }
                    }
                }
                catch (err) { toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd");}
            },
            failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    function FillEndStateCombos() {
        _lstRPendstate = []; _lstQPCendstate = [];
        _lstRPendstate.push("1"+"|"+"Received From Buyer");_lstRPendstate.push("2" + "|" + "Imported");  _lstRPendstate.push("3" + "|" + "Exported"); _lstRPendstate.push("4" + "|" + "Sent to Supplier");
        _lstQPCendstate.push("1" + "|" + "Received From Supplier");  _lstQPCendstate.push("2" + "|" + "Imported");   _lstQPCendstate.push("3" + "|" + "Exported");  _lstQPCendstate.push("4" + "|" + "Sent to Buyer");
    };

    function FillCombo(val, _lst,state) {
        var opt = '';
        try {
            if (state) { opt = '<option>--NA--</option>'; } else { opt = '<option></option>'; }
            if (_lst.length != undefined && _lst.length > 0) {
                for (var i = 0; i < _lst.length; i++) {
                    var cdet = _lst[i].split("|");
                    if (val != "" && val == Str(cdet[0]))   opt += '<option selected value="' + Str(cdet[0]) + '">' + Str(cdet[1]) + '</option>';
                    else if (val != "" && val == Str(cdet[1]))    opt += '<option selected value="' + Str(cdet[0]) + '">' + Str(cdet[1]) + '</option>';
                    else  opt += '<option value="' + Str(cdet[0]) + '">' + Str(cdet[1]) + '</option>';
                }
                return opt;
            }
        }
        catch (err) {  toastr.error('Error while populating List :' + err, "Lighthouse eSolutions Pte. Ltd"); }
    };
  
    function ValidateGroup(indx, _gid) {
        var _valid = true;
        var _grpcode = $('#txtCode' + indx).val(); var _grpdesc = $('#txtDescr' + indx).val();
        if (_grpcode == '') { $('#txtCode' + indx).addClass('error'); _valid = false; } else { $('#txtCode' + indx).removeClass('error');}
        if (_grpdesc == '') { $('#txtDescr' + indx).addClass('error'); _valid = false; } else { $('#txtDescr' + indx).removeClass('error'); }
        if (_grpcode != '') { var _existrec = ValidateExistingGroup(_grpcode, _gid);
            if (_existrec != '') { toastr.error("Lighthouse eSolutions Pte. Ltd.", _existrec); _valid = false; }
        }
        return _valid;
    };

    var ValidateExistingGroup = function (GROUPCODE, GROUPID) {
        var res = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "Groups.aspx/CheckExistingGroup",
            data: "{'GROUPCODE':'" + Str(GROUPCODE) + "','GROUPID':'" + Str(GROUPID) + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {  res = Str(response.d);}
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to validate Group details :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) {  toastr.error("error get", response.responseText); }
        });
        return res;
    };

    function SaveGroup(_nfieldval, callback) {
        var slGrpdet = [];
        for (var j = 0; j < _nfieldval.length; j++) {slGrpdet.push(_nfieldval[j]);  }
        var data2send = JSON.stringify({ slGrpdet: slGrpdet });
         Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "Groups.aspx/SaveGroupDetails",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "Group Saved successfully.");   GetGroupsGrid();}
                    Metronic.unblockUI();
                }
                catch (err) {   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Group details :' + err); Metronic.unblockUI(); }
            },
            failure: function (response) {   toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };

    function DeleteGroup(GROUPID, callback) {
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "Groups.aspx/DeleteGroup",
            data: "{ 'GROUPID':'" + GROUPID + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") {  toastr.success("Lighthouse eSolutions Pte. Ltd", "Group Deleted.");  GetGroupsGrid();}
                    Metronic.unblockUI();
                }
                catch (err) {  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete Group :' + err); Metronic.unblockUI(); }
            },
            failure: function (response) {  toastr.error("failure get", response.d); Metronic.unblockUI();  },
            error: function (response) {  toastr.error("error get", response.responseText); Metronic.unblockUI();  }
        });
        }, 20);
    };

    function ValidateRule(_ruleval) {
        var _valid = true; if (_ruleval == '') { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Rule Value field is blank'); _valid = false; }
        return _valid;
    };

    function SaveGroupRule(_nfieldval, callback,_grpid) {
        var slGRuledet = [];
        for (var j = 0; j < _nfieldval.length; j++) { slGRuledet.push(_nfieldval[j]); }
        var data2send = JSON.stringify({ slGRuledet: slGRuledet });
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "Groups.aspx/SaveGroupRuleDetails",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") {  toastr.success("Lighthouse eSolutions Pte. Ltd", "Rule Saved successfully.");   GetGRulesGrid(_grpid); }
                    Metronic.unblockUI();
                }
                catch (err) {   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Rule details :' + err); Metronic.unblockUI();  }
            },
            failure: function (response) {  toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) {toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    }, 20);
    };

    var setupRuleTableHeader = function () {
        var dtfilter = '<th></th><th>Rule Number</th><th>Doc Type</th><th>Rule Code</th><th>Description</th><th>Comments</th><th>Rule Value</th><th>GROUP_ID</th><th>RULEID</th>';
        $('#tblHeadRowlnkRule').empty().append(dtfilter); $('#tblBodylnkRule').empty();
    };

    var setupLinkedBSTableHeader = function () {
        var dtfilter = '<th>Buyer Code</th><th>Buyer Name</th><th>Supplier Code</th><th>Supplier Name</th><th>LINNKID</th>';
        $('#tblHeadRowBSlnk').empty().append(dtfilter); $('#tblBodyBSlnk').empty();
    };

    var setupTableHeader = function () {
        setFilterToolbar();
        var dtfilter = '<th>View</th><th></th><th>Group Code</th><th>Group Description</th><th>Buyer <br/> Import Format</th><th>Supplier <br/> Export Format</th><th>Buyer <br/> Export Format</th><th>Supplier <br/> Import Format</th>' +
                '<th>Buyer</th><th>Supplier</th><th>RFQ Process</th><th>Quote Process</th><th>PO Process</th><th>POC Process</th><th>RFQ End State</th><th>Quote End State</th><th>PO End State</th><th>POC End State</th><th>View Links</th><th>Copy From</th><th>Create New Buyer-Supplier Link</th><th>GROUP_ID</th>';
        $('#tblHeadRowGrp').empty().append(dtfilter);  $('#tblBodyGrp').empty();
    };

    var setFilterToolbar = function () {
        $('#toolbtngroup').empty();
        var _btns = ' <div class="row" style="margin-bottom: 2px;padding-right: 20px;"> <div class="col-md-12">' +
            ' <div id="toolbtngroup" >' +
            ' <span title="New" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnNewGroup"><i class="fa fa-plus" style="text-align:center;"></i></a></div>' +
            ' <span title="Clear" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnClear"><i class="fa fa-eraser" style="text-align:center;"></i></a></div>' +
            ' <span title="Refresh" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"> <a href="javascript:;" class="toolbtn" id="btnRefresh"><i class="fa fa-recycle" style="text-align:center;"></i></a></div></div>' +
            ' </div></div>';
        $('#toolbtngroup').append(_btns);
    };
    function ClearFilter() { setFilterToolbar(); $('#dataGridGrp').DataTable().clear().draw(); };
    
    return {
        init: function () { handleGroupsTable(); }
    };
}();


