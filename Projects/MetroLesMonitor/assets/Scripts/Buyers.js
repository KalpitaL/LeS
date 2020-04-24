var _lstDoc = []; var _buydet = []; var _lstBSuppDet = []; var selectedTr = ''; var previousTr = '';var _lstgrpfrmt = []; var _selectdefval = ''; var _configpath = ''; var _uplDwlPath = '';

var Buyers = function () {

    var handleBuyersTable = function () {
        var nEditing = null; var _isBuyupdate = -2; var nNew = false; $('#pageTitle').empty().append('Connected Buyers');
        SetupBreadcrumb('Home', 'Home.aspx', 'Suppliers/Buyers', '#', 'Connected Buyers', 'Buyers.aspx');
        $(document.getElementById('lnkSuppBuy')).addClass('active open'); $(document.getElementById('spBuyer')).addClass('title font-title SelectedColor'); $(document.getElementById('spBuyer')).text('Connected Buyers');
        var _defaddrid = Str(sessionStorage.getItem('ADDRESSID')); var _configaddrid = Str(sessionStorage.getItem('CONFIGADDRESSID')); var _addrtype = Str(sessionStorage.getItem('ADDRTYPE'));
        setupTableHeader(_addrtype); setFilterToolbar();

        var table = $('#dataGridBuy');
        var oBTable = table.dataTable({
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
            "columnDefs": [{  'orderable': false,  "searching": true, "autoWidth": false,  'targets': [0] },
            { 'targets': [0], width: '5px', 'bSortable': false },{ 'targets': [2], width: '10px' },
            { 'targets': [3], width: '120px', 'sClass': 'longText' }, { 'targets': [4], width: '40px', 'sClass': 'longText' }, { 'targets': [5], width: '50px' },
            { 'targets': [6], width: '20px' }, { 'targets': [7], width: '50px' }, { 'targets': [0, 1, 8, 9, 10, 11], visible: false }
            ],
            "lengthMenu": [[15, 30, 50, 100], [15, 30, 50, 100]],
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {           
                var btnid = 'View' + nRow._DT_RowIndex;
                var viewbtnTag = '<div  style="text-align:center;"><span  class="actionbtn" data-toggle="tooltip" title="Buyers" id= "' + btnid + '"><i class="fa fa-search"></i></span></div>';
                var _tooltipdiv = '<div data-placement="top" data-toggle="tooltip">' + Str(aData[4]) + '</div>'; $("td:eq(2)", nRow).html(_tooltipdiv);
                var detTag = '<a id="detid" href="javascript:;">' + Str(aData[2]) + '</a>'; $('td:eq(0)', nRow).html(detTag);
            }
        });

        $('#tblHeadRowBuy').addClass('gridHeader'); $('#ToolTables_dataGridBuy_0,#ToolTables_dataGridBuy_1').hide(); 
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%"); $('#dataGridBuy').css('width', '100%');
        GetBuyersGrid();

        oBTable.on('click', 'tbody td', function (e) {
            var selectedTr = $(this).parents('tr')[0]; var aData = oBTable.fnGetData(selectedTr); var cellindx = $(this).index(); updtcnt = 0;
            if (oBTable.fnIsOpen(selectedTr) && (e.target.innerText == 'Edit')) {oBTable.fnClose(selectedTr);  }
            else {
                if (e.target.className == 'opentd' || e.target.className == 'fa fa-search' || e.target.id == 'detid') {
                    var _buyerid = aData[11] + '#2'; var _buyerdet = aData[2] + '-' + aData[3]; var strencrurl = getEncryptedData('BUYERID=' + _buyerid + "&BUYER_CODE=" + _buyerdet);
                    var win = window.open("../LESMonitorPages/BuyerDetail.aspx?" + strencrurl, '_blank'); win.focus(); return false;
                }              
            }
        });

        function fnheaderFormatDetails(oTable) {
            var sOut = ''; var _str = ''; var indx = 0;
            var _code = ''; var _buyname = ''; var _contactpers = ''; var _email = ''; var _country = ''; var _weblink = ''; var _imppath = '';
            var _exppath = ''; var _addrid = ''; var _BExpPath = ''; var _BImpPath = ''; var _deffrmt = ''; var _islesconnect = ''; var _lesconid = 'chkLesConnect';
            var tid = "BuyTable"; var _tbodyid = "tblBodyBy";
            var _codeid = 'txtCode'; var _bnameid = 'txtBuyername'; var _contactPersid = 'txtContactPerson'; var _emailid = 'txtEmail';
            var _countryid = 'txtCountry'; var _weblnkid = 'txtWebLink'; var _imppathid = 'txtImpPath'; var _exppathid = 'txtExpPath';
            var _bimpid = 'txtBImpPath'; var _bexpid = 'txtBExpPath'; var _defformatid = 'cbDefFormat'; var _txtdisabled = 'disabled';
            _code = GetNextBuyer(); FillGroupFormat(); _DefFormat = FillCombo(_deffrmt, _lstgrpfrmt);
            var _frmbyr = 'Import Path \n (From Buyer)'; var _tobyr = 'Export Path \n (To Buyer)'; _frmbyr = _frmbyr.replace(/\n/g, '<br/>'); _tobyr = _tobyr.replace(/\n/g, '<br/>');
            var sOut = '<div class="row"><div class="col-md-12"><div class="form-group">' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Code </label> </div>' +
                  ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _codeid + '"  value="' + _code + '"/> </div>' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Buyer Name </label> </div>' +
                  ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _bnameid + '"  value="' + _buyname + '"/> </div></div></div></div>' +
                  ' <div class="row"><div class="col-md-12"><div class="form-group"><div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Contact Person </label> </div>' +
                  ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _contactPersid + '"  value="' + _contactpers + '"/> </div>' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Email </label> </div>' +
                  ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _emailid + '"  value="' + _email + '"/> </div>' +
                  ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Country </label> </div>' +
                  ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _countryid + '"  value="' + _country + '"/> </div>' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Web Link </label> </div>' +
                  ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _weblnkid + '"  value="' + _weblink + '"/> </div>' +
                  ' </div></div></div><hr/><div class="row"><div class="col-md-12"><div class="form-group">' +
                  ' <div class="col-md-2"></div><div class="col-md-4"><input class="widelarge" type="checkbox" id="' + _lesconid + '"  value="' + _islesconnect + ' /> <label class="dvlabel">LeSConnect</label></div>' +
                  ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Download Path </label> </div>' +
                  ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" ' + _txtdisabled + ' rows="2" id="' + _imppathid + '">' + _imppath + '</textarea> </div>' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Upload Path </label> </div>' +
                  ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" ' + _txtdisabled + ' rows="2" id="' + _exppathid + '">' + _exppath + '</textarea> </div>' +
                  ' </div></div></div><hr/><div class="row"><div class="col-md-12"><div class="form-group">' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Default Format </label> </div>' +
                  ' <div  class="col-md-4"><select class="bs-select form-control" id="' + _defformatid + '">' + _DefFormat + '</select></div>' +
                  ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> ' + _frmbyr + ' </label> </div>' +
                  ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _bimpid + '" value="' + _BImpPath + '"/></div>' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> ' + _tobyr + ' </label> </div>' +
                  ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _bexpid + '" value="' + _BExpPath + '"/></div>' +
                  ' </div></div></div>';
            return sOut;
        }

        $('#btnBuyNew').click(function (e) {
            e.preventDefault();  _buydet = []; var _res = ValidateBuyer(0, 0);
            if (_res == true) { GetBuyerDetails(oBTable, '', _buydet, 'New'); SaveBuyerDetails(_buydet, GetBuyersGrid); $("#ModalNew").modal('hide'); }
        });
        $('#btnRefresh').live('click', function (e) { e.preventDefault(); GetBuyersGrid(); });
        $('#btnClear').live('click', function (e) { e.preventDefault(); ClearFilter(); });
        $('#btnNew').live('click', function (e) {  e.preventDefault();
            var _urldet = getEncryptedData('ADDRTYPE=Buyer');  var win = window.open("../LESMonitorPages/NewBuyerSupplierWizard.aspx?" + _urldet, '_blank'); win.focus();
        });

        $('#cbDefFormat').live("change", function (e) {
            _selectdefval = $('#cbDefFormat option:selected').val(); ClearControls(); var _byrCode = $('#txtCode').val();
            var _exppath = _configpath + _byrCode + "\\" + _selectdefval + "\\OUTBOX"; var _imppath = _configpath + _byrCode + "\\" + _selectdefval + "\\INBOX";
            $('#txtBImpPath').val(_imppath); $('#txtBExpPath').val(_exppath);
        });

        $('#chkLesConnect').live("change", function (e) {
            var _byrCde = $('#txtCode').val(); var _exppath = ''; var _imppath = ''; var _txtdisabled ='true';
            if (this.checked) { _exppath = _uplDwlPath + _byrCde + "\\OUTBOX"; _imppath = _uplDwlPath + _byrCde + "\\INBOX"; _txtdisabled = ''; } else { _exppath = ''; _imppath = ''; }
            $('#txtImpPath').val(_imppath); $('#txtExpPath').val(_exppath);  $('#txtImpPath').prop('disabled', _txtdisabled);   $('#txtExpPath').prop('disabled', _txtdisabled);
            return false;
        });


    };

    function FillBuyersGrid(Table) {
        try {
            $('#dataGridBuy').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridBuy').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str(''); cells[1] = Str(''); cells[2] = Str(Table[i].ADDR_CODE); cells[3] = Str(Table[i].ADDR_NAME); cells[4] = Str(Table[i].CONTACT_PERSON);
                    cells[5] = Str(Table[i].ADDR_EMAIL);  cells[6] = Str(Table[i].ADDR_COUNTRY); cells[7] = Str(Table[i].LISENCE_KEY);cells[8] = Str(Table[i].WEB_LINK);
                    cells[9] = Str(Table[i].ADDR_INBOX); cells[10] = Str(Table[i].ADDR_OUTBOX); cells[11] = Str(Table[i].ADDRESSID); 
                    var ai = t.fnAddData(cells, false);
           }
                t.fnDraw(); 
          }
            else {
                if (Table.ADDRESSID != undefined && Table.ADDRESSID != null) {
                    var t = $('#dataGridBuy').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str('');cells[1] = Str(''); cells[2] = Str(Table.ADDR_CODE);cells[3] = Str(Table.ADDR_NAME);
                    cells[4] = Str(Table.CONTACT_PERSON); cells[5] = Str(Table.ADDR_EMAIL); cells[6] = Str(Table.ADDR_COUNTRY); cells[7] = Str(Table.LISENCE_KEY);
                    cells[8] = Str(Table.WEB_LINK); cells[9] = Str(Table.ADDR_INBOX); cells[10] = Str(Table.ADDR_OUTBOX); cells[11] = Str(Table.ADDRESSID); 
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
             }
           }
        }
        catch (e)  { }
   };

    var GetBuyersGrid = function (){
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {  
        $.ajax({
            type: "POST",
            async: false,
            url: "Buyers.aspx/FillBuyerGrid",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table;  FillBuyersGrid(Table); }
                    else $('#dataGridBuy').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI();   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyers :' + err); }
            },
            failure: function (response) {  toastr.error("failure get", response.d); Metronic.unblockUI();},
            error: function (response) {  toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
        }, 200);
    };

    var GetNextBuyer = function () {
        var res='';
        $.ajax({
            type: "POST",
            async: false,
            url: "Buyers.aspx/GetNextBuyerNo",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var _lstres = Str(response.d).split('||'); res = _lstres[0]; _uplDwlPath = _lstres[1]; _configpath = _lstres[2];
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Next Buyer :' + err);}
            },
            failure: function (response) {  toastr.error("failure get", response.d);  },
            error: function (response) {toastr.error("error get", response.responseText); }
        });

        return res;
    };

    function GetBuyerDetails(Table, nTr, _lstdet, _targetclick) {
        var _buyerid = '';  var indx = ''; if (_targetclick == 'New') { indx = 0; _buyerid = 0; } else { indx = nTr.rowIndex; _buyerid = Table.fnGetData(nTr)[11]; }
        var tid = "BuyTable" ; var _code = $('#txtCode').val(); var _buyername = $('#txtBuyername').val();
        var _contactPerson = $('#txtContactPerson').val(); var _email = $('#txtEmail').val(); var _country = $('#txtCountry').val();
        var _weblnk = $('#txtWebLink').val(); var _imppath = $('#txtImpPath').val(); var _exppath = $('#txtExpPath').val();
        var _byrexppath = $('#txtBExpPath').val(); var _byrimppath = $('#txtBImpPath').val(); var _defformat =  $('#cbDefFormat').val();
        _lstdet.push("ID" + "|" + Str(_buyerid)); _lstdet.push("ADDR_CODE" + "|" + Str(_code)); _lstdet.push("ADDR_NAME" + "|" + Str(_buyername)); _lstdet.push("CONTACT_PERSON" + "|" + Str(_contactPerson));
        _lstdet.push("ADDR_EMAIL" + "|" + Str(_email)); _lstdet.push("ADDR_COUNTRY" + "|" + Str(_country)); _lstdet.push("WEBLINK" + "|" + Str(_weblnk));
        _lstdet.push("ADDR_INBOX" + "|" + Str(_imppath)); _lstdet.push("ADDR_OUTBOX" + "|" + Str(_exppath));
        _lstdet.push("EXPORT_PATH" + "|" + Str(_byrexppath)); _lstdet.push("IMPORT_PATH" + "|" + Str(_byrimppath));
        _lstdet.push("DEF_FORMAT" + "|" + Str(_defformat));
    };

    function ValidateBuyer(indx, buyid) {
        var _valid = true;
        var _code = $('#txtCode').val(); var _bname = $('#txtBuyername').val(); var _deffrmt= $('#cbDefFormat').val(); var _bexppath = $('#txtBExpPath').val(); var _bimppath = $('#txtBImpPath').val();
        if (_code == '') { $('#txtCode').addClass('error'); _valid = false; }
        else {  var isexist = CheckExistingBuyer(_code, buyid);
            if (isexist == '') { $('#txtCode').removeClass('error'); } else { $('#txtCode').addClass('error'); toastr.error("Lighthouse eSolutions Pte. Ltd.", isexist); _valid = false; }
        }
        if (_bname == '') { $('#txtBuyername').addClass('error'); _valid = false; } else { $('#txtBuyername').removeClass('error'); }
        if (_deffrmt == '') { $('#cbDefFormat').addClass('error'); _valid = false; } else { $('#cbDefFormat').removeClass('error'); }
        if (_bexppath == '') { $('#txtBExpPath').addClass('error'); _valid = false; } else { $('#txtBExpPath').removeClass('error'); }
        if (_bimppath == '') { $('#txtBImpPath').addClass('error'); _valid = false; } else { $('#txtBImpPath').removeClass('error'); }
        return _valid;
    };

     var CheckExistingBuyer = function (ADDR_CODE, BUYERID) {
        var res = '';     
        $.ajax({
            type: "POST",
            async: false,
            url: "Buyers.aspx/CheckExistingBuyer",
            data: "{ 'ADDR_CODE':'" + ADDR_CODE + "','BUYERID':'" + (BUYERID) + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    res = Str(response.d);                  
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'cannot validate new buyer :' + err); }
            },
            failure: function (response) {  toastr.error("failure get", response.d);  },
            error: function (response) { toastr.error("error get", response.responseText);  }
        });
        return res;
    };

    function SaveBuyerDetails(_nfieldval, callback) {
        var slBuydet = [];for (var j = 0; j < _nfieldval.length; j++) { slBuydet.push(_nfieldval[j]); }
        var data2send = JSON.stringify({ slBuydet: slBuydet });
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {  
            $.ajax({
                type: "POST",
                async: false,
                url: "Buyers.aspx/SaveBuyerDetails",
                data: data2send,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d);
                        if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "Buyer Saved successfully.");  callback();}
                        Metronic.unblockUI();
                    }
                    catch (err) {  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Buyer :' + err); Metronic.unblockUI();  }
                },
                failure: function (response) {  toastr.error("failure get", response.d); Metronic.unblockUI();},
                error: function (response) {  toastr.error("error get", response.responseText); Metronic.unblockUI(); }
            });
        }, 20);
    };

    function DeleteBuyer(BUYERID,callback) {
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {  
        $.ajax({
            type: "POST",
            async: false,
            url: "Buyers.aspx/DeleteBuyers",
            data: "{ 'BUYERID':'" + BUYERID + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "Buyer Deleted.");    callback(); }
                    Metronic.unblockUI();
                }
                catch (err) {   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete Buyer :' + err); Metronic.unblockUI();}
            },
            failure: function (response) {   toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) {   toastr.error("error get", response.responseText); Metronic.unblockUI();  }
        });
        }, 20);
    };

    /*Config Settings*/
    function FillGroupFormat() {
        var _format = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "BuyerDetail.aspx/GetGroupFormat",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var Dataset = JSON.parse(response.d);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table; _lstgrpfrmt = [];
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) {
                                for (var i = 0; i < Table.length; i++) { _lstgrpfrmt.push(Str(Table[i].FORMAT) + "|" + Str(Table[i].FORMAT));  }
                            }
                            else {
                                if (Table.FORMAT != undefined) { _lstgrpfrmt.push(Str(Table.FORMAT) + "|" + Str(Table.FORMAT)); }
                            }
                        }
                    }
                }
                catch (err) {    toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd");  }
            },
            failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    function ClearControls() { var _emptystr = ''; $('#txtBImpPath').val(_emptystr); $('#txtBExpPath').val(_emptystr);};
    /**/

    var setupTableHeader = function (_addrtype) {
        var dthead = ''; if (_addrtype.toUpperCase() == "SUPPLIER") { dthead = '<th style="display:none;">#</th><th style="display:none;"><div style="text-align:center;"><span><a style="color: #1c1b1b;"><u>New</u></<a></span></div>'; } else { dthead = '<th>View</th><th><div style="width:20px;padding:1px;"><span><a style="color: #eee8e8;"><u>New</u></<a></span></div>'; }
        var dtfilter = dthead+ ' </th><th>Code</th><th>Buyer Name</th><th>Contact Person</th><th>Email</th><th>Country</th><th>Adaptor Lisence Key</th><th>Web Link</th><th>Import Path</th><th>Export Path</th><th>ADDRESSID</th>';
        $('#tblHeadRowBuy').empty().append(dtfilter); $('#tblBodyBuy').empty();
    };

    var setFilterToolbar = function () {
        $('#toolbtngroup').empty(); var _btns = ' <div class="row" style="margin-bottom: 2px;padding-right: 20px;"> <div class="col-md-12">' +
            ' <div id="toolbtngroup" >' +
            ' <span title="New" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnNew"><a href="javascript:;" class="toolbtn" id="btnNew"><i class="fa fa-plus" style="text-align:center;"></i></a></div>' +
            ' <span title="Clear" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnClearFilter"><a href="javascript:;" class="toolbtn" id="btnClear"><i class="fa fa-eraser" style="text-align:center;"></i></a></div>' +
            ' <span title="Refresh" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnRefresh" > <a href="javascript:;" class="toolbtn" id="btnRefresh"><i class="fa fa-recycle" style="text-align:center;"></i></a></div></div>' +
            ' </div></div>';    ;
        $('#toolbtngroup').append(_btns);
        if (_isparent != '' && _isparent != undefined) { if (_isparent.toUpperCase() == 'TRUE') { $('#btnNew').show(); } else { $('#btnNew').hide(); } }
    };

    function ClearFilter() { setFilterToolbar(); $('#dataGridBuy').DataTable().clear().draw(); };


    return {
        init: function () { handleBuyersTable(); }
    };
}();