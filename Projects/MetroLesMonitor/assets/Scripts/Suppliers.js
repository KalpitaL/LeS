var selectedTr = '';var previousTr = '';var bspreviousTr = '';var irefpreviousTr = '';var iuompreviousTr = ''; var updtcnt = '';var brefdelchk = '';var buomdelchk = '';
var _defrule = []; var _lstSuppdet = []; var _lstgrp = []; var _lstBSLnkdet = []; var _lstIRefdet = []; var _lstIUOMdet = []; var _lstlogindet = []; var _lstDoc = [];
var _defaddrid = ''; var _lstgrpfrmt = []; var _selectdefval = ''; var _configpath = ''; var _uplDwlPath = '';
var _lstSelectedRowID = []; var _spDownload_path='';var _spUpload_path='';var _spImport_path='';var _spExport_path='';


var Suppliers = function () {

    var handleSuppliersTable = function () {
        var nEditing = null; var _isSupupdate = -2; var _isBSupupdate = -2; var _isBSItemupdate = -2; var _isBSItemUomupdate = -2; var nNew = false; var _newcont = 0;
        SetupBreadcrumb('Home', 'Home.aspx', 'Suppliers/Buyers', '#', 'Connected Suppliers', 'Suppliers.aspx'); $('#pageTitle').empty().append('Connected Suppliers');
        $(document.getElementById('lnkSuppBuy')).addClass('active open');$(document.getElementById('spSupp')).addClass('title font-title SelectedColor');
        _defaddrid = Str(sessionStorage.getItem('ADDRESSID')); _configaddrid = Str(sessionStorage.getItem('CONFIGADDRESSID'));   _addrtype = Str(sessionStorage.getItem('ADDRTYPE'));
        setupTableHeader(_addrtype); setFilterToolbar(_addrtype);
        
        var table = $('#dataGridSupp');
        var oSTable = table.dataTable({
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
            "columnDefs": [{  // set default column settings
                'orderable': false,  "searching": true, "autoWidth": false, 'targets': [0]
            },
            { 'targets': [0], width: '5px', 'bSortable': false },  { 'targets': [2], width: '20px' },
            { 'targets': [3], width: '110px', 'sClass': 'longText' }, { 'targets': [4], width: '40px', 'sClass': 'longText' }, { 'targets': [5], width: '80px' },
            { 'targets': [6], width: '20px' }, { 'targets': [7], width: '50px' }, { 'targets': [0, 1, 8, 9, 10, 11], visible: false },
            ],
            "lengthMenu": [[15, 30, 50, 100], [15, 30, 50, 100]],
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var btnid = 'View' + nRow._DT_RowIndex; var chkid = 'chk' + nRow._DT_RowIndex;
                var viewbtnTag = '<div  style="text-align:center;"><span  class="actionbtn" data-toggle="tooltip" title="Detail" id= "' + btnid + '"><i class="fa fa-search"></i></span></div>';                
                var detTag = '<a id="detid" href="javascript:;">' + Str(aData[2]) + '</a>'; $('td:eq(0)', nRow).html(detTag);
            }
        });
        $('#tblHeadRowSupp').addClass('gridHeader'); $('#ToolTables_dataGridSupp_0,#ToolTables_dataGridSupp_1').hide();
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%"); $('#dataGridSupp').css('width', '100%');
        GetSuppliersGrid(); 

        oSTable.on('click', 'tbody td', function (e) {
            var selectedTr = $(this).parents('tr')[0]; var aData = oSTable.fnGetData(selectedTr);
            var cellindx = $(this).index(); updtcnt = 0;
            if (oSTable.fnIsOpen(selectedTr) && (e.target.innerText == 'Edit')) {  oSTable.fnClose(selectedTr);  }
            else {
                var divid = 'dv' + selectedTr._DT_RowIndex;
                if (e.target.innerText == 'Edit') {
                    _isSupupdate = selectedTr._DT_RowIndex;
                    if ((previousTr != '') && (oSTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {
                        var prevdivid = 'dv' + previousTr._DT_RowIndex;  oSTable.fnClose(previousTr); $('#' + prevdivid).show();
                    }
                    if (aData != null) {
                        oSTable.fnOpen(selectedTr, fnFormatDetails(oSTable, selectedTr), 'details');  $('#' + divid).hide(); previousTr = selectedTr;
                    }
                }
                if (e.target.innerText == 'Delete') {
                    if (confirm('Are you sure? You want to delete this supplier ?')) {  var _addrid = aData[10]; DeleteSupplier(_addrid, GetSuppliersGrid); }
                }
                if (e.target.className == 'opentd' || e.target.className == 'fa fa-search' || e.target.id == 'detid') {
                    var _supplierid = aData[10] + '#2'; var _suppliercd = aData[2] + '-' + aData[3];; sessionStorage['SUPPLIERID'] = Str(_addrid);
                    var strencrurl = getEncryptedData('SUPPLIERID=' + _supplierid + "&SUPPLIER_CODE=" + _suppliercd);
                    var win = window.open("../LESMonitorPages/SupplierDetail.aspx?" + strencrurl, '_blank'); win.focus();
                    return false;
                }
             
                $('#btnUpdate').click(function () {
                    _lstSuppdet = [];
                    if (aData != null && updtcnt == 0) {
                        var _addrid = aData[10]; var _res = ValidateSupplier(selectedTr.rowIndex, _addrid);
                        if (_res == true) {
                            _isSupupdate = -2; GetSupplierDetails(oSTable, selectedTr, _lstSuppdet, e.target.innerText); SaveSupplierDetails(_lstSuppdet, GetSuppliersGrid);
                            updtcnt++;
                        }
                    } $('#' + divid).show();
                });
                $('#btnCancel').click(function () { if (oSTable.fnIsOpen(selectedTr)) { oSTable.fnClose(selectedTr); } $('#' + divid).show(); _isSupupdate = -2; });

                //if (e.target.type == "checkbox") {
                //    var $checkbox = $(this).find(':checkbox'); var _schkd = $checkbox.prop('checked');
                //    var tr = $(this).closest('tr');
                //    tr.find('input[type="checkbox"]').not($(this)).prop("checked", false); $checkbox.prop("checked", _schkd);
                //    if ($checkbox.is(':checked')) { } else { $('#chkHdexcel').prop('checked', $checkbox.is(':checked')); }
                //}
              
            }
        });

        $('#btnSuppNew').live('click', function (e) {  e.preventDefault();
            _lstSuppdet = []; var _res = ValidateSupplier(0, 0);
            if (_res == true) {  GetSupplierDetails(oSTable, '', _lstSuppdet, 'New'); SaveSupplierDetails(_lstSuppdet, GetSuppliersGrid); $("#ModalNew").modal('hide');  }
        });
        $('#btnRefresh').live('click', function (e) { e.preventDefault(); GetSuppliersGrid(); });
        $('#btnClear').live('click', function (e) { e.preventDefault(); ClearFilter(); });
        $('#btnNew').live('click', function (e) {
            e.preventDefault(); var _urldet = getEncryptedData('ADDRTYPE=Supplier'); var win = window.open("../LESMonitorPages/NewBuyerSupplierWizard.aspx?" + _urldet, '_blank'); win.focus();
        });      
        $('#cbDefFormat').live("change", function (e) {
            _selectdefval = $('#cbDefFormat option:selected').val(); ClearControls(); var _SuppCode = $('#txtCode').val();
            var _exppath = _configpath + _SuppCode + "\\" + _selectdefval + "\\OUTBOX"; var _imppath = _configpath + _SuppCode + "\\" + _selectdefval + "\\INBOX";
            $('#txtSImpPath').val(_imppath); $('#txtSExpPath').val(_exppath);
        });
        $('#chkLesConnect').live("change", function (e) {
            var _sppCde = $('#txtCode').val(); var _exppath = ''; var _imppath = ''; var _txtdisabled = 'true';
            if (this.checked) { _exppath = _uplDwlPath + _sppCde + "\\OUTBOX"; _imppath = _uplDwlPath + _sppCde + "\\INBOX"; _txtdisabled = ''; } else { _exppath = ''; _imppath = ''; }
            $('#txtImpPath').val(_imppath); $('#txtExpPath').val(_exppath);
            $('#txtImpPath').prop('disabled', _txtdisabled); $('#txtExpPath').prop('disabled', _txtdisabled);
            return false;
        });
    };

    function GetSupplierDetails(Table, nTr, _lstdet, _targetclick) {
        var _suppid = '';var indx = '';
        if (_targetclick == 'New') { indx = 0; _suppid = 0; } else { indx = nTr.rowIndex; _suppid = Table.fnGetData(nTr)[10]; } var tid = "SuppTable";
        var _code = $('#txtCode' ).val(); var _suppname = $('#txtSuppname').val();   var _contactPerson = $('#txtContactPerson').val(); var _email = $('#txtEmail').val();
        var _country = $('#txtCountry').val(); var _imppath = $('#txtImpPath').val(); var _exppath = $('#txtExpPath').val();
        var _suppexppath = $('#txtSExpPath').val(); var _suppimppath = $('#txtSImpPath').val(); var _defformat = $('#cbDefFormat').val();
        _lstdet.push("ID" + "|" + Str(_suppid)); _lstdet.push("ADDR_CODE" + "|" + Str(_code));
        _lstdet.push("ADDR_NAME" + "|" + Str(_suppname)); _lstdet.push("CONTACT_PERSON" + "|" + Str(_contactPerson));
        _lstdet.push("ADDR_EMAIL" + "|" + Str(_email)); _lstdet.push("ADDR_COUNTRY" + "|" + Str(_country));
        _lstdet.push("ADDR_INBOX" + "|" + Str(_imppath)); _lstdet.push("ADDR_OUTBOX" + "|" + Str(_exppath));
        _lstdet.push("EXPORT_PATH" + "|" + Str(_suppexppath)); _lstdet.push("IMPORT_PATH" + "|" + Str(_suppimppath));_lstdet.push("DEF_FORMAT" + "|" + Str(_defformat));
    };

    function FillGrid(Table) {
        try {
            $('#dataGridSupp').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridSupp').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();                  
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str(''); cells[1] = Str('');; cells[2] = Str(Table[i].ADDR_CODE); cells[3] = Str(Table[i].ADDR_NAME);
                    cells[4] = Str(Table[i].CONTACT_PERSON); cells[5] = Str(Table[i].ADDR_EMAIL); cells[6] = Str(Table[i].ADDR_COUNTRY);
                    cells[7] = Str(Table[i].LISENCE_KEY); cells[8] = Str(Table[i].ADDR_INBOX); cells[9] = Str(Table[i].ADDR_OUTBOX); cells[10] = Str(Table[i].ADDRESSID); cells[11] = Str(Table[i].LINK_COUNT);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw(); 
          }
            else {
                if (Table.ADDRESSID != undefined && Table.ADDRESSID != null) {
                    var t = $('#dataGridSupp').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str(''); cells[1] = Str(''); cells[2] = Str(Table.ADDR_CODE); cells[3] = Str(Table.ADDR_NAME);
                    cells[4] = Str(Table.CONTACT_PERSON); cells[5] = Str(Table.ADDR_EMAIL); cells[6] = Str(Table.ADDR_COUNTRY); cells[7] = Str(Table.LISENCE_KEY);                 
                    cells[8] = Str(Table.ADDR_INBOX); cells[9] = Str(Table.ADDR_OUTBOX); cells[10] = Str(Table.ADDRESSID); cells[11] = Str(Table.LINK_COUNT);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
             }
           }
        }
        catch (e)  { }
   };

    var GetSuppliersGrid = function (){
        Metronic.blockUI('#portlet_body');  
        setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "Suppliers.aspx/FillSuppliers",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) {  Table = DataSet.NewDataSet.Table;  if (Len(Table) > 0) FillGrid(Table);}
                    else $('#dataGridSupp').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI();   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Suppliers :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI();},
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI();  }
        });
        }, 180);
    };

    var GetNextSupplier = function () {
        var res = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "Suppliers.aspx/GetNextSupplierNo",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                     var _lstres = Str(response.d).split('||');  res = _lstres[0]; _uplDwlPath = _lstres[1]; _configpath = _lstres[2];
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Next Supplier :' + err); }
            },
            failure: function (response) {   toastr.error("failure get", response.d);  },
            error: function (response) { toastr.error("error get", response.responseText);}
        });
        return res;
    };

    function fnheaderFormatDetails(oTable) {
        var sOut = ''; var _str = ''; var _code = ''; var _suppname = ''; var _contactpers = ''; var _email = ''; var _country = ''; var _imppath = ''; var _exppath = ''; var _addrid = ''; var indx = 0;
        var _SExpPath = ''; var _SImpPath = ''; var _deffrmt = ''; var _islesconnect = ''; var _lesconid = 'chkLesConnect';
        var tid = "SuppTable"; var _tbodyid = "tblBodySupp"; var _codeid = 'txtCode';
        var _snameid = 'txtSuppname'; var _contactPersid = 'txtContactPerson'; var _emailid = 'txtEmail'; var _countryid = 'txtCountry';
        var _imppathid = 'txtImpPath'; var _exppathid = 'txtExpPath'; var _simpid = 'txtSImpPath'; var _sexpid = 'txtSExpPath'; var _defformatid = 'cbDefFormat';
        _code = GetNextSupplier(); FillGroupFormat(); var _txtdisabled = 'disabled'; _DefFormat = FillCombo(_deffrmt, _lstgrpfrmt);
        var _frmSpp = 'Import Path \n (From Supplier)'; var _toSpp = 'Export Path \n (To Supplier)'; _frmSpp = _frmSpp.replace(/\n/g, '<br/>'); _toSpp = _toSpp.replace(/\n/g, '<br/>');
        var sOut = '<div class="row"><div class="col-md-12"><div class="form-group">' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Code </label> </div>' +
                  ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _codeid + '"  value="' + _code + '"/> </div>' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Supplier Name </label> </div>' +
                  ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _snameid + '"  value="' + _suppname + '"/> </div>' +
                  ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Contact Person </label> </div>' +
                  ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _contactPersid + '"  value="' + _contactpers + '"/> </div>' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Email </label> </div>' +
                  ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _emailid + '"  value="' + _email + '"/> </div>' +
                  ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Country </label> </div>' +
                  ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _countryid + '"  value="' + _country + '"/> </div>' +
                  ' </div></div></div><hr/><div class="row"><div class="col-md-12"><div class="form-group">' +
                  ' <div class="col-md-2"></div><div class="col-md-4"><input class="widelarge" type="checkbox" id="' + _lesconid + '"  value="' + _islesconnect + ' /><label class="dvlabel">LeSConnect</label></div>' +
                  ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Upload Path </label> </div>' +
                  ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" ' + _txtdisabled + '  rows="2" id="' + _exppathid + '">' + _exppath + '</textarea> </div>' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Download Path </label> </div>' +
                  ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" ' + _txtdisabled + '  rows="2" id="' + _imppathid + '">' + _imppath + '</textarea></div>' +
                  ' </div></div></div><hr/><div class="row"><div class="col-md-12"><div class="form-group">' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Default Format </label> </div>' +
                  ' <div  class="col-md-4"><select class="bs-select form-control" id="' + _defformatid + '">' + _DefFormat + '</select></div>' +
                  ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> ' + _frmSpp + ' </label> </div>' +
                  ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _simpid + '" value="' + _SImpPath + '"/></div>' +
                  ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> ' + _toSpp + ' </label> </div>' +
                  ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _sexpid + '" value="' + _SExpPath + '"/></div>' +
                  ' </div></div></div>';
        return sOut;
    };

    function ValidateSupplier(indx, suppid) {
        var _valid = true;
        var _code = $('#txtCode').val(); var _sname = $('#txtSuppname').val(); var _contactper = $('#txtContactPerson').val(); var _imppath = $('#txtImpPath').val(); var _exppath = $('#txtExpPath').val();
        var _deffrmt = $('#cbDefFormat').val(); var _sexppath = $('#txtSExpPath').val(); var _simppath = $('#txtSImpPath').val();
        if (_code == '') { $('#txtCode').addClass('error'); _valid = false; }
        else {
            var isexist = CheckExistingSupplier(_code, suppid);
            if (isexist == '') {  $('#txtCode').removeClass('error');} else { $('#txtCode').addClass('error'); toastr.error("Lighthouse eSolutions Pte. Ltd.", isexist); _valid = false; }
        }
        if (_sname == '') { $('#txtSuppname').addClass('error'); _valid = false; } else { $('#txtSuppname').removeClass('error'); }
        if (_contactper == '') { $('#txtContactPerson').addClass('error'); _valid = false; } else { $('#txtContactPerson').removeClass('error'); }
        if (_deffrmt == '') { $('#cbDefFormat').addClass('error'); _valid = false; } else { $('#cbDefFormat').removeClass('error'); }
        if (_sexppath == '') { $('#txtSExpPath').addClass('error'); _valid = false; } else { $('#txtSExpPath').removeClass('error'); }
        if (_simppath == '') { $('#txtSImpPath').addClass('error'); _valid = false; } else { $('#txtSImpPath').removeClass('error'); }
        return _valid;
    };

    var CheckExistingSupplier = function (ADDR_CODE, SUPPLIERID) {
        var res = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "Suppliers.aspx/CheckExistingSupplier",
            data: "{ 'ADDR_CODE':'" + ADDR_CODE + "','SUPPLIERID':'" + SUPPLIERID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try { res = Str(response.d);  }
                catch (err) {   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'cannot validate supplier :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d);  },
            error: function (response) { toastr.error("error get", response.responseText);  }
        });
        return res;
    };

    function SaveSupplierDetails(_nfieldval, callback) {
        var slSuppdet = [];
        for (var j = 0; j < _nfieldval.length; j++) {   slSuppdet.push(_nfieldval[j]);  }
        var data2send = JSON.stringify({ slSuppdet: slSuppdet });
        Metronic.blockUI('#portlet_body');     
        $.ajax({
            type: "POST",
            async: false,
            url: "Suppliers.aspx/SaveSupplierDetails",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") {  toastr.success("Lighthouse eSolutions Pte. Ltd", "Supplier Saved successfully."); GetSuppliersGrid(); }   Metronic.unblockUI();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Supplier :' + err); Metronic.unblockUI();}
            },
            failure: function (response) {  toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });      
    };

    function DeleteSupplier(SUPPLIERID, callback) {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "Suppliers.aspx/DeleteSupplier",
            data: "{ 'SUPPLIERID':'" + SUPPLIERID + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "Supplier Deleted.");  GetSuppliersGrid(); }
                    Metronic.unblockUI();
                }
                catch (err) {  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete Supplier :' + err); Metronic.unblockUI();}
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI();},
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };

    /*Config Settings*/
    function FillGroupFormat() {
        var _format = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "SupplierDetail.aspx/GetGroupFormat",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var Dataset = JSON.parse(response.d);
                    if (Dataset.NewDataSet != null) {  var Table = Dataset.NewDataSet.Table;  _lstgrpfrmt = [];
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) { for (var i = 0; i < Table.length; i++) { _lstgrpfrmt.push(Str(Table[i].FORMAT) + "|" + Str(Table[i].FORMAT)); } }
                            else {   if (Table.FORMAT != undefined) { _lstgrpfrmt.push(Str(Table.FORMAT) + "|" + Str(Table.FORMAT)); } }
                        }
                    }
                }
                catch (err) { toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd");  }
            },
            failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    function ClearControls() {  var _emptystr = '';  $('#txtSImpPath').val(_emptystr); $('#txtSExpPath').val(_emptystr);  };

    var setFilterToolbar = function (_addrtype) {
        $('#toolbtngroup').empty();
        var _btns = ' <div class="row" style="margin-bottom: 2px;padding-right: 20px;"> <div class="col-md-12">' +
            ' <div id="toolbtngroup" >' +
            ' <span title="New" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnNew"><i class="fa fa-plus" style="text-align:center;"></i></a></div>' +
            ' <span title="Clear" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnClear"><i class="fa fa-eraser" style="text-align:center;"></i></a></div>' +
            ' <span title="Refresh" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"> <a href="javascript:;" class="toolbtn" id="btnRefresh"><i class="fa fa-recycle" style="text-align:center;"></i></a></div></div>' +
            ' </div></div>';
        $('#toolbtngroup').append(_btns);
        if (_isparent != '' && _isparent != undefined) { if (_isparent.toUpperCase() == 'TRUE') { $('#btnNew').show(); } else { $('#btnNew').hide(); } }
        if (_addrtype != undefined && _addrtype.toUpperCase() == "BUYER") { $('#tnNew').hide(); }
    };

    function ClearFilter() {  setFilterToolbar(); $('#dataGridSupp').DataTable().clear().draw();  };

    var setupTableHeader = function (_addrtype) {
        var dthead = '<th style="width:5px;">View</th><th style="width:5px;text-align:center;">#</th>';
        var dtfilter = dthead + '<th>Code</th><th>Supplier Name</th><th>Contact Person</th> <th>Email</th><th>Country</th><th>Adaptor Lisence Key</th><th>Import Path</th><th>Export Path</th><th>ADDRESSID</th><th>Linked\n Buyers</th>';
        $('#tblHeadRowSupp').empty().append(dtfilter); $('#tblBodySupp').empty();
    };

    return {
        init: function () { handleSuppliersTable(); }
    };
}();

