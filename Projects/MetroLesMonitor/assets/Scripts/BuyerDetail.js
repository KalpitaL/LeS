var BUYERID = '0'; var Params = ''; var bydet = []; var _lstdet = []; var _byCnfgdet = []; var CODE = '';
var _lstgrpfrmt = []; var _addressConfigID = '0'; var _configpath = ''; var _selectdefval = ''; var _byrdet = ''; var _cnfgMailsubj = '';
var _lstRules = []; var _lstRuleval = []; var _lstMasterDet = []; var previousBTr = ''; var _lstRuledetails = []; var _scheckid = [];
var _lstfilter = []; var _FROM = 1; var _TO = 200; var _orgdiff = 200; var _totalcount = ''; var previousXTr = ''; var previousPTr = '';
var oBSuppTable = ''; var oBRTable = ''; var oNDRTable = ''; var oBTransTable = ''; var oBXlsMappTable = ''; var oBPdfMappTable = '';
var nEditing = null; var nNew = false; var _isRuleupdate = -2; var _isxlsupdate = -2; var _ispdfupdate = -2; var _lstDocType = [];
var _lstSyncservers = []; var _lstControlsname = [];

var BuyerDetail = function ()  {

    var handleBuyerDetailTable = function () {
        $('#divSpacer').remove(); $('#divFilterWrapper').remove();
        SetupBreadcrumb('Home', 'Home.aspx', 'Connected Buyers', 'Buyers.aspx', 'Buyer Details', 'BuyerDetail.aspx');
        Params = getDecryptedData();
        if (Params != '' && Params != undefined) { if (Params.length > 1) {   BUYERID = Params[0].split('|')[1].split('#')[0];CODE = Params[1].split('|')[1];   }  }
        $('#pageTitle').empty().append('Buyer Details  (' + CODE + ')'); setToolbar();GetBuyerDetails(BUYERID,_selectdefval); setaccess('0');
        oBSuppTable = $('#dataGridBSlnk');  oBRTable = $('#dataGridBRules');    oNDRTable = $('#dataGridNewDRule');   oBTransTable = $('#dataGridBTrans');
       // oBXlsMappTable = $('#dataGridBXlsMapp');// oBPdfMappTable = $('#dataGridBPdfMapp');
       $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
           var target = e.target.innerHTML; setaccess(Str(e.target.id));
           if (Str(target) == 'Detail') { GetBuyerDetails(BUYERID); } else if (Str(target) == 'Default Rules') { GetBuyerRules(BUYERID); }
           else if (Str(target) == 'Linked Suppliers') { GetLinkedSuppliers(BUYERID); } else if (Str(target) == 'Mappings') { MappingSettings(); GetBuyerExcelMapp(); }
           else if (Str(target) == 'Transactions') {InitialiseBuyerTransactn();GetBuyerTransactions(_FROM, _TO, BUYERID);}
       });   
       $('#btnClose').removeAttr('href').on('click', function (event) { event.preventDefault(); top.window.close(); });
       $('#btnRefresh').on('click', function (e) {
           e.preventDefault();  var activeTab = $('#hTabs .active').text();
           if (activeTab == 'Detail') { GetBuyerDetails(BUYERID); }
           else if (activeTab == 'Transactions') { GetBuyerTransactions(_FROM, _TO, BUYERID); }
       });
       $('#btnCancel').on('click', function (e) { e.preventDefault(); GetBuyerDetails(BUYERID);  });
       $('#btnSave').on('click', function (e) {
           e.preventDefault(); var activeTab = $('#hTabs .active').text();
           if (activeTab == 'Detail') {
               SaveBuyerDetails(BUYERID); //SaveBConfigDetails(BUYERID);
               if (_lstSyncservers.length > 0) {
                   _lstControlsname = [];
                   _lstControlsname.push($('#txtCSImpPath').val()); _lstControlsname.push($('#txtCSExpPath').val()); _lstControlsname.push($('#txtUploadPath').val()); _lstControlsname.push($('#txtDownloadPath').val());
                   SyncDetails(BUYERID, _lstSyncservers, _lstControlsname);
               }
           }
       });
       $('#btnNew').on('click', function (e) {
           e.preventDefault(); var activeTab = $('#hTabs .active').text();
           if (activeTab == 'Default Rules') {   var _defgrpformat = GetBuyerFormat(BUYERID);
               if (_defgrpformat != '' && _defgrpformat != undefined) { SetBuyerRules(BUYERID);   }  else { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Select Group');}
           }
       });
       $('#btnDelete').on('click', function (e) { e.preventDefault(); if (confirm('Are you sure ? You want to delete this Buyer ?')) { DeleteBuyer(BUYERID); window.close(); } });
       $('#cbDefFormat').live("change", function (e) {
           _selectdefval = $('#cbDefFormat option:selected').val(); var _isexist = CheckExisting(_selectdefval, BUYERID);
           if (_isexist == '') {  var _byrCode = $('#txtCode').val();
               var _exppath = _configpath + _byrCode + "\\" + _selectdefval + "\\OUTBOX"; var _imppath = _configpath + _byrCode + "\\" + _selectdefval + "\\INBOX";
               $('#txtBImpPath').val(_imppath); $('#txtBExpPath').val(_exppath);
           }
           else { GetBuyerConfigSettings(_selectdefval, BUYERID); }
       });
       $('#cbRuleCode').live("change", function (e) {
           var _id = $('#cbRuleCode option:selected').val(); GetDefRulesdet(_id); var _arrcommt = Str(_lstRuledetails[1]).split(',').join('\n');
           $('#txtDescription').val(_lstRuledetails[0]); $('#txtComments').val(_arrcommt);
       });
       function editDefRuleRow(oTable, nRow) {
           var aData = oTable.fnGetData(nRow); nEditing = nRow; var jqTds = $('>td', nRow);
           FillRules(); _lstRuleval = []; _lstRuleval.push("0|0"); _lstRuleval.push("1|1");
           var detTag = '<div style="text-align:center;"><span class="actionbtn" data-toggle="tooltip" title="Update"><i class="glyphicon glyphicon-floppy-save"></i></span>' +
              ' <span class="actionbtn" data-toggle="tooltip" title="Cancel"><i class="glyphicon glyphicon-ban-circle"></i></span></div>';
           jqTds[0].innerHTML = detTag;
           jqTds[3].innerHTML = '<select id="cbRuleCode" class="fullWidth selectheight">' + FillCombo(aData[3], _lstRules) + '</select>';
           jqTds[6].innerHTML = '<select id="cbRuleValue" class="fullWidth selectheight">' + FillCombo(aData[6], _lstRuleval) + '</select>';
           $('#cbRuleCode').select2(); $('#cbRuleValue').select2();
       };
       $('#btnDefNew').click(function (e) {
           e.preventDefault(); var _defaddrid = BUYERID; var _defgrpformat = GetBuyerFormat(BUYERID); _scheckid = [];
           $('input[type=checkbox]:checked').each(function () {
               var tr = $(this).closest('tr'); if (oNDRTable.fnGetData(tr) != null) {
                   var _id = '#cbRuleValue' + tr[0]._DT_RowIndex; var _ruleval = $(_id).val();
                   var _rulecd = oNDRTable.fnGetData(tr)[3]; var _ruleid = oNDRTable.fnGetData(tr)[7];
                   var _ruledet = 'RULE_CODE|' + Str(_rulecd) + '#' + 'RULEID|' + +Str(_ruleid) + '#' + 'RULE_VALUE|' + Str(_ruleval);
                   if (Str(_ruleval) != '') { if (Str(_ruleval).toUpperCase() != 'NOT SET') { _scheckid.push(Str(_ruledet)); } else { return false; } } else { return false; }
               }
           });
           if (_scheckid.length > 0) {
               for (var k = 0; k < _scheckid.length; k++) {
                   _lstMasterDet = []; var _defid = 0; _hidvalue = '0'; var _det = []; _det = _scheckid[k].split('#');
                   _lstMasterDet.push(_det[0]); _lstMasterDet.push(_det[1]); _lstMasterDet.push(_det[2]);
                   _lstMasterDet.push('DEFAULT_RULE_ADDRESSID' + "|" + _defaddrid); _lstMasterDet.push('DEFAULT_RULE_GROUP_FORMAT' + "|" + _defgrpformat);
                   _lstMasterDet.push('HID_VALUE' + "|" + _hidvalue); _lstMasterDet.push('DEF_ID' + "|" + _defid);
                   SaveDefRule(_lstMasterDet, GetBuyerRules, _defaddrid, _defgrpformat);
               }
               $("#ModalDefRule").modal('hide');
           }
           else { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Rule Value is blank'); }
       });
       oBRTable.on('click', 'tbody td', function (e) {
           var _defaddrid = BUYERID; var _defgrpformat = GetBuyerFormat(BUYERID);
           var selectedBTr = $(this).parents('tr')[0]; var aData = oBRTable.fnGetData(selectedBTr); var divid = 'dv' + selectedBTr._DT_RowIndex;
           if (e.target.className == 'glyphicon glyphicon-pencil') {
               _isRuleupdate = selectedBTr._DT_RowIndex; if (previousBTr != '' && previousBTr != selectedBTr) { restoreRow(previousBTr, oBRTable); }
               nNew = false; editDefRuleRow(oBRTable, selectedBTr); previousBTr = selectedBTr;
           }
           else if (e.target.className == 'glyphicon glyphicon-trash') {
               var _hidvalue = '0'; var _defid = aData[8]; var rulecode = aData[3]; var deleteRule = confirm('Are you sure? You want to delete this rule?');
               if (deleteRule) {
                   var deleteAll = confirm('Are you sure? You want to delete this rule from all related Buyer-Supplier links ?'); if (deleteAll) { _hidvalue = '1'; }
                   DeleteDefaultRule(_defid, _defaddrid, _defgrpformat, rulecode, _hidvalue, GetBuyerRules);
               }
               previousBTr = '';
           }
           else if (e.target.className == 'glyphicon glyphicon-floppy-save') {
               var _hidvalue = '0'; var _defid = '0'; _lstMasterDet = []; if (nNew) { _defid = '0'; } else { _defid = aData[8]; }
               UpdateBuyerDefRules(_defid, _defaddrid, _defgrpformat, _lstMasterDet, false); previousBTr = ''; _newcont = 0; _isRuleupdate = -2;
           }
           else if (e.target.className == 'glyphicon glyphicon-ban-circle') { _isRuleupdate = -2; gridCancelEdit(nNew, nEditing, oBRTable); previousBTr = ''; _newcont = 0; }
       });
       function restoreRow(nRow, oTable) {
           var aData = oTable.fnGetData(nRow); var jqTds = $('>td', nRow);
           for (var i = 0, iLen = jqTds.length; i < iLen; i++) if (i == 0) { } else { oTable.fnUpdate(aData[i], nRow, i, false); } oTable.fnDraw();
       };
       function gridCancelEdit(nNew, nEditing, oTable) {
           var nRow = $('.DTTT_selected');
           if (nNew) { oTable.fnDeleteRow(nEditing); nNew = false; } else { nRow = nEditing; restoreRow(nRow, oTable); $(nRow).removeClass("DTTT_selected selected"); }
       };
       oNDRTable.on('change', 'tr', function (e) {
           var rowIndx = $(this).index();
           if (e.target.id == 'cbRuleValue' + rowIndx) {              
               var val = $('#' + e.target.id).val(); var isresult = (val != ""); if (isresult) { $('#chk' + rowIndx).prop('checked', (val.toUpperCase() != 'NOT SET')); }
               else { $('#chk' + rowIndx).prop('checked', false); }
           }
       });
       oBTransTable.on('click', ' tbody td', function (e) {           
         var  nTr = $(this).parents('tr')[0]; var colIndx = $(this).index();
         var updatedate = oBTransTable.fnGetData(nTr)[0]; var modulename = oBTransTable.fnGetData(nTr)[2]; var logid = oBTransTable.fnGetData(nTr)[8]; var filename = oBTransTable.fnGetData(nTr)[9];
           if (colIndx == 7) { if (Str(filename) != "") { DownloadFile(updatedate, modulename, filename); } }
       });
       $('#dataGridBXlsMapp').on('click', 'tbody td', function (e) {
           var selectedXTr = $(this).parents('tr')[0]; var aData = oBXlsMappTable.fnGetData(selectedXTr); _targetclick = '';
           if (oBXlsMappTable.fnIsOpen(selectedXTr) && ((e.target.className == 'glyphicon glyphicon-pencil'))) {
               oBXlsMappTable.fnClose(selectedXTr);
           }
           else {
               var divid = 'dv' + selectedXTr._DT_RowIndex;
               if (e.target.className == 'glyphicon glyphicon-pencil') {
                   _isxlsupdate = selectedXTr._DT_RowIndex; _targetclick = e.target.innerText;
                   if ((previousXTr != '') && (oBXlsMappTable.fnIsOpen(previousXTr) && previousXTr != selectedXTr)) {
                       var prevdivid = 'dv' + previousXTr._DT_RowIndex; oBXlsMappTable.fnClose(previousXTr); $('#' + prevdivid).show();
                   }
                   if (aData != null) {
                       oBXlsMappTable.fnOpen(selectedXTr, fnXlsFormatDetails(oBXlsMappTable, selectedXTr, 'Edit'), 'details');
                       $('#' + divid).hide(); previousXTr = selectedXTr;
                   }
               }
               $('#btnXLSUpdate').click(function () {
                   if (aData != null) {
                       _mapdet = []; _isxlsupdate = -2;
                       GetXMappRowDetails(oBXlsMappTable, selectedXTr, _mapdet); SaveXLSMappingDetails(_mapdet);
                       $('#' + divid).show();
                   }
               });
               $('#btnXLSCancel').click(function () {
                   if (oBXlsMappTable.fnIsOpen(selectedXTr)) { oBXlsMappTable.fnClose(selectedXTr); } $('#' + divid).show(); _isxlsupdate = -2;
               });
           }
       });
       $('#dataGridBPdfMapp').on('click', 'tbody td', function (e) {
           var selectedPTr = $(this).parents('tr')[0]; _selectedrow = selectedPTr; var aData = oBPdfMappTable.fnGetData(selectedPTr); _targetclick = '';
           if (oBPdfMappTable.fnIsOpen(selectedPTr) && ((e.target.className == 'glyphicon glyphicon-pencil') || (e.target.innerText == 'Copy'))) {
               oBPdfMappTable.fnClose(selectedPTr);
           }
           else {
               var divid = 'dv' + selectedPTr._DT_RowIndex;
               if (e.target.className == 'glyphicon glyphicon-pencil') {
                   _ispdfupdate = selectedPTr._DT_RowIndex; _targetclick = e.target.innerText;
                   if ((previousPTr != '') && (oBPdfMappTable.fnIsOpen(previousPTr) && previousPTr != selectedPTr)) {
                       var prevdivid = 'dv' + previousPTr._DT_RowIndex; oBPdfMappTable.fnClose(previousPTr); $('#' + prevdivid).show();
                   }
                   if (aData != null) {
                       oBPdfMappTable.fnOpen(selectedPTr, fnXlsFormatDetails(oBPdfMappTable, selectedPTr, 'Edit'), 'details');
                       $('#' + divid).hide(); previousPTr = selectedPTr;
                   }
               }
         
               $('#btnPDFUpdate').click(function () {
                   if (aData != null) {
                       _ispdfupdate = -2; _mapdet = [];
                       GetPMappRowDetails(oBPdfMappTable, selectedPTr, _mapdet); SavePDFMappingDetails(_mapdet);
                       $('#' + divid).show();
                   }
               });
               $('#btnPDFCancel').click(function () {
                   if (oBPdfMappTable.fnIsOpen(selectedPTr)) { oBPdfMappTable.fnClose(selectedPTr); } $('#' + divid).show(); _ispdfupdate = -2;
               });
           }
       });       
       $("#txtBuyername").autocomplete({
           source: function (request, response) {
               $.ajax({
                   type: "POST",
                   async: false,
                   url: "BuyerDetail.aspx/GetAddressName_Search",
                   data: "{'SEARCHTEXT':'" + $("#txtBuyername").val() + "','ADDRTYPE':'BUYER'}",
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   success: function (data) {   if (data.d.length > 0) {
                       response($.map(data.d, function (item) {
                           return {
                               label: item.split('-')[0],
                               val: item.split('-')[1]
                           };
                       }))
                   } else {  response([{ label: 'No results found.', val: -1 }]); }
                   },
                   error: function (result) { alert("Error......"); }
               });
           },
           select: function (e, u) {  if (u.item.val == -1) {  return false;   }  }        
       });

       $('input[type=checkbox]').click(function (e) {
           var id = this.id; var _isChkd = this.checked; var _selecdserv = '';
           var _Servname = $("label[for='" + this.id + "']").text();
           if (_lstSyncservers != undefined) {
               if (jQuery.inArray(_Servname, _lstSyncservers) == -1) {
                   if (_isChkd == true) {
                       _lstSyncservers.push(_Servname);
                   }
               }
               else {
                   if (_isChkd == false) {
                       _lstSyncservers = jQuery.grep(_lstSyncservers, function (value) {
                           return value != _Servname;
                       });
                   }
               }
           }
           for (var k = 0; k < _lstSyncservers.length; k++) {
               _selecdserv = Str(_lstSyncservers[k]);
           }
           $('#btnServ').text(_lstSyncservers);
       });
    };

    $('input[type="radio"]').on('click', function () {
        $('#tblXls').show(); $('#tblPdf').hide(); if ($(this).attr('id') == 'rdExcel') { $('#tblXls').show(); GetBuyerExcelMapp(); }
        else if ($(this).attr('id') == 'rdPDF') { $('#tblXls').hide(); $('#tblPdf').show(); GetBuyerPDFMapp(); }
    });
   
    function SaveBuyerDetails(BUYERID) {
        _lstdet=[]; var _code = $('#txtCode').val(); var _buyername = $('#txtBuyername').val();
        var _contactPerson = $('#txtContactPerson').val(); var _email = $('#txtEmail').val(); var _country = $('#txtCountry').val();
        var _weblnk = $('#txtWebLink').val(); var _imppath = $('#txtDownloadPath').val(); var _exppath = $('#txtUploadPath').val(); var _city = $('#txtCity').val(); var _postal = $('#txtPostalCode').val();
        _lstdet.push("ID" + "|" + Str(BUYERID)); _lstdet.push("ADDR_CODE" + "|" + Str(_code)); _lstdet.push("ADDR_NAME" + "|" + Str(_buyername)); _lstdet.push("CONTACT_PERSON" + "|" + Str(_contactPerson));
        _lstdet.push("ADDR_EMAIL" + "|" + Str(_email)); _lstdet.push("ADDR_COUNTRY" + "|" + Str(_country)); _lstdet.push("WEBLINK" + "|" + Str(_weblnk));
        _lstdet.push("ADDR_INBOX" + "|" + Str(_imppath)); _lstdet.push("ADDR_OUTBOX" + "|" + Str(_exppath)); _lstdet.push("ADDR_CITY" + "|" + Str(_city)); _lstdet.push("ADDR_ZIPCODE" + "|" + Str(_postal));
        var _defFormat = $('#cbDefFormat').val();
        if (ValidateBuyer(BUYERID)) { UpdateBuyerDetails(_lstdet, GetBuyerDetails, BUYERID,_defFormat) }
    };

    function ValidateBuyer(buyid) {
        var _valid = true;
        var _code = $('#txtCode').val(); var _bname = $('#txtBuyername' ).val();
        if (_code == '') { $('#txtCode').addClass('error'); _valid = false; }
        if (_bname == '') { $('#txtBuyername').addClass('error'); _valid = false; } else { $('#txtBuyername').removeClass('error'); }
        return _valid;
   };

    function fnFormatDetails(BUYERID) {
        var sOut = ''; var _str = ''; var _disableinput = ''; var _hiddeninput = '';
        var tid = "BuyTable"; var _tbodyid = "tblBodyBy";  var _codeid = 'txtCode'; var _bnameid = 'txtBuyername';
        var _contactPersid = 'txtContactPerson'; var _emailid = 'txtEmail'; var _cityid = 'txtCity'; var _postalid = 'txtPostalCode';
        var _countryid = 'txtCountry'; var _weblnkid = 'txtWebLink'; var _imppathid = 'txtDownloadPath'; var _exppathid = 'txtUploadPath';
        var slbydet = GetBuyerHeaderDetail(BUYERID);
        var _code = Str(slbydet.ADDR_CODE); var _buyname = Str(slbydet.ADDR_NAME); var _contactpers = Str(slbydet.CONTACT_PERSON);
        var _email = Str(slbydet.ADDR_EMAIL); var _country = Str(slbydet.ADDR_COUNTRY); var _weblink = Str(slbydet.WEB_LINK);
        var _imppath = Str(slbydet.ADDR_INBOX); var _exppath = Str(slbydet.ADDR_OUTBOX); var _addrid = Str(BUYERID);
        var _city = Str(slbydet.ADDR_CITY); var _postalcode = Str(slbydet.ADDR_ZIPCODE);
        if (_isparent != '' && _isparent != undefined) { if (_isparent.toUpperCase() == 'TRUE') { _disableinput = ''; _hiddeninput = '1'; } else { _disableinput = 'disabled'; _hiddeninput = '0'; } }
        var _lstServernames = []; _lstServernames = GetServerNames();
        var _servselect = '<div class="btn-group" style="width: 100%;">' +
          '<button type="button" class="multiselect dropdown-toggle mt-multiselect btn" data-toggle="dropdown" id="btnServ"' +
          'title="" style="width: 100%; overflow: hidden; text-overflow: ellipsis;padding:10px;height:25px;">' +
          ' <span class="multiselect-selected-text"</span> ' +
          ' <b class="caret"></b></button>' +
          ' <ul class="multiselect-container dropdown-menu">';
        if (_lstServernames != [] && _lstServernames != null && _lstServernames != undefined) {
            for (var l = 0; l < _lstServernames.length; l++) {
                if (Str(_lstServernames[l]) != '') {
                    var _chkid = 'chk' + _lstServernames[l];
                    _servselect += '<li><a tabindex="0"><label class="checkbox" for="' + _chkid + '"><input type="checkbox" id="' + _chkid + '" value="1">' + _lstServernames[l] + '</label></a></li>';
                }
            }
        }
        _servselect += ' </ul></div></span>';
        var sOut = '<div class="row">' +
            ' <div class="col-md-10"><div class="row"><div class="col-md-12"><div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Code </label> </div>' +
            ' <div  class="col-md-4"><input type="text" class="form-control" ' + _disableinput + ' id="' + _codeid + '"  value="' + _code + '"/> </div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Name </label> </div>' +
            ' <div  class="col-md-4"><input type="text" class="form-control" ' + _disableinput + ' id="' + _bnameid + '"  value="' + _buyname + '"/> </div>' +
            ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Contact Person </label> </div>' +
            ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _contactPersid + '"  value="' + _contactpers + '"/> </div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Email </label> </div>' +
            ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _emailid + '"  value="' + _email + '"/> </div>' +
            ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Country </label> </div>' +
            ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _countryid + '"  value="' + _country + '"/> </div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Web Link </label> </div>' +
            ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _weblnkid + '"  value="' + _weblink + '"/> </div>' +
            ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
            //' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> City </label> </div>' +
            //' <div  class="col-md-4"><input type="text" class="form-control" id="' + _cityid + '"  value="' + _city + '"/> </div>' +
            //' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Postal Code </label> </div>' +
            //' <div  class="col-md-4"><input type="text" class="form-control" id="' + _postalid + '"  value="' + _postalcode + '"/> </div>' +
            //' </div> </div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Download Path </label> </div>' +
            ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _imppathid + '">' + _imppath + '</textarea> </div>' +
            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Upload Path </label> </div>' +
            ' <div class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _exppathid + '">' + _exppath + '</textarea> </div>' +
            ' </div></div></div>';
        if (_hiddeninput == '1') {
            sOut += '<div class="row"><div class="col-md-12"><div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Select Servers </label> </div>' +
                ' <div  class="col-md-4">' + _servselect + ' </div>' +
                ' </div></div></div>';
        }
        sOut += ' </div> </div>';
        return sOut;
    }   
  
    var setToolbar = function () {
        var _btns = ' <span title="Sync" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnSync"><i class="fa fa-refresh" style="text-align:center;"></i></a></div>' +
            '<span title="Cancel" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnCancel"><a href="javascript:;" id="btnCancel" class="toolbtn"><i class="fa fa-ban" style="text-align:center;"></i></a></div></span>' +
            '<span title="Save" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnSave"><a href="javascript:;" class="toolbtn" id="btnSave"><i class="fa fa-floppy-o" style="text-align:center;"></i></a></div></span>' +
            //'<span title="Delete" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtndelete"><a href="javascript:;" id="btnDelete" class="toolbtn"><i class="fa fa-trash" style="text-align:center;"></i></a></div></span>' +
            '<span title="New" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnNew"><a href="javascript:;" id="btnNew" class="toolbtn"><i class="fa fa-plus" style="text-align:center;"></i></a></div></span>' +
            '<span title="Refresh" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnRefresh"><a href="javascript:;" class="toolbtn" id="btnRefresh"><i class="fa fa-recycle" style="text-align:center;"></i></a></div></span>';
        $('#toolbtngroupdet').append(_btns);
    };

    function setaccess(tabindx) {
        $('#btnSave').show(); $('#btnCancel').show(); $('#btnSync').hide(); $('#prtDetail').hide(); $('#prtByrRules').hide(); $('#prtLnkSupp').hide(); $('#prtBMapping').hide(); $('#prtBTransact').hide();
        if (tabindx == '0') { $('#btnNew').hide(); $('#btnDelete').show(); $('#prtDetail').show(); $('#btnSync').hide(); }
        else if (tabindx == '1') { $('#btnNew').show(); $('#btnDelete').hide(); $('#btnSave').hide(); $('#btnCancel').hide(); $('#prtByrRules').show(); }
        else if (tabindx == '2') { $('#btnNew').hide(); $('#btnDelete').hide(); $('#btnSave').hide(); $('#btnCancel').hide(); $('#prtLnkSupp').show(); }
        else if (tabindx == '3') { $('#btnNew').hide(); $('#btnDelete').hide(); $('#btnSave').hide(); $('#btnCancel').hide(); $('#prtBMapping').show(); }
        else if (tabindx == '4') { $('#btnNew').hide(); $('#btnDelete').hide(); $('#btnSave').hide(); $('#btnCancel').hide(); $('#prtBTransact').show(); }
    };
   
    function FillBSupplierGrid(Table) {
        try {
            $('#dataGridBSlnk').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridBSlnk').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();  var r = jQuery('<tr id=' + i + '>');
                    var _vencode=  '<a style="color:#0d638f" onclick="Navigate(this)" id=' + Int(Table[i].ADDRESSID) + '>' +  Str(Table[i].ADDR_CODE) + '</a>';
                    cells[0] = _vencode; cells[1] = Str(Table[i].ADDR_NAME); cells[2] = Str(Table[i].CONTACT_PERSON); cells[3] = Str(Table[i].ADDR_EMAIL);   cells[4] = Str(Table[i].ADDRESSID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.ADDRESSID != undefined && Table.ADDRESSID != null) {
                    var t = $('#dataGridBSlnk').dataTable(); var r = jQuery('<tr id=' + 1 + '>'); var cells = new Array();
                    var _vencode=  '<a style="color:#0d638f" onclick="Navigate(this)" id=' + Int(Table.ADDRESSID) + '>' +  Str(Table.ADDR_CODE) + '</a>';
                    cells[0] = _vencode; cells[1] = Str(Table.ADDR_NAME); cells[2] = Str(Table.CONTACT_PERSON); cells[3] = Str(Table.ADDR_EMAIL); cells[4] = Str(Table.ADDRESSID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    function GetBuyerDetails(BUYERID, _selectdefval) {
        var headerdet = fnFormatDetails(BUYERID); $('#divDetail').empty().append(headerdet); GetBuyerConfigSettings(_selectdefval, BUYERID);
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
                catch (err) {   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'cannot validate new buyer :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) {toastr.error("error get", response.responseText);}
        });
        return res;
    };

    function UpdateBuyerDetails(_nfieldval, callback, BUYERID, _defFormat) {
        var slBuydet = [];
        for (var j = 0; j < _nfieldval.length; j++) { slBuydet.push(_nfieldval[j]); }
        var data2send = JSON.stringify({ slBuydet: slBuydet });
        Metronic.blockUI('#portlet_body');
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
                        SaveBConfigDetails(BUYERID); callback(BUYERID, _defFormat);
                        if (res != "") {  toastr.success("Lighthouse eSolutions Pte. Ltd", "Buyer Saved successfully.");  }
                        Metronic.unblockUI();
                    }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Buyer :' + err); Metronic.unblockUI(); }
                },
                failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI();  },
                error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI();}
            });
    };

    function DeleteBuyer(BUYERID) {
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
                        if (res != "") {    toastr.success("Lighthouse eSolutions Pte. Ltd", "Buyer Deleted.");}
                        Metronic.unblockUI();
                    }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete Buyer :' + err); Metronic.unblockUI(); }
                },
                failure: function (response) {  toastr.error("failure get", response.d); Metronic.unblockUI();},
                error: function (response) {   toastr.error("error get", response.responseText); Metronic.unblockUI(); }
            });
        }, 20);
    };

    var setupBSuppTableHeader = function () {
        var dtfilter = '</th><th>Supplier Code</th><th>Supplier Name</th><th>Contact Person</th><th>Email</th><th>ADDRESSID</th>';
        $('#tblHeadRowBSlnk').empty().append(dtfilter); $('#tblBodyBSlnk').empty();
    };

    var GetBuyerHeaderDetail = function (BUYERID) {
        $.ajax({
            type: "POST",
            async: false,
            url: "BuyerDetail.aspx/GetBuyerDetails",
            data: "{'BUYERID':'" + BUYERID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    bydet = JSON.parse(response.d);
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer Details :' + err);}
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
        return bydet;
    };

    /*Config Settings*/
    var GetBuyerConfigDetail = function (FORMAT, BUYERID) {
        $.ajax({
            type: "POST",
            async: false,
            url: "BuyerDetail.aspx/GetBuyerConfigDetails",
            data: "{'FORMAT':'" + FORMAT + "','BUYERID':'" + BUYERID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    _configpath = response.d.split('||')[1];_byCnfgdet = (response.d.split('||')[0] != '{}')?JSON.parse(response.d.split('||')[0]):[];
                    _cnfgMailsubj = response.d.split('||')[2];
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer Config :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) {  toastr.error("error get", response.responseText); }
        });
        return _byCnfgdet;
    };

    var CheckExisting = function (FORMAT,BUYERID) {
        var _Isexist = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "BuyerDetail.aspx/CheckExistingConfig",            
            data: "{'FORMAT':'" + FORMAT + "','BUYERID':'" + BUYERID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    _Isexist = Str(response.d);
                }
                catch (err) {  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Buyer Config check :' + err);  }
            },
            failure: function (response) {  toastr.error("failure get", response.d); },
            error: function (response) {   toastr.error("error get", response.responseText);  }
        });
        return _Isexist;
    };

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
                            if (Table.length != undefined) {for (var i = 0; i < Table.length; i++) {  _lstgrpfrmt.push(Str(Table[i].FORMAT) + "|" + Str(Table[i].FORMAT)); }  }
                            else { if (Table.FORMAT != undefined) {    _lstgrpfrmt.push(Str(Table.FORMAT) + "|" + Str(Table.FORMAT)); } }
                        }
                    }
                }
                catch (err) { toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd"); }
            },
            failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    function FillDropdownlist(val, _lst) {
        var opt = ''; emptystr = '';
        try {
            opt = '<li>' + emptystr + '</li>';
            if (_lst.length != undefined && _lst.length > 0) {
                for (var i = 0; i < _lst.length; i++) {
                    var cdet = _lst[i].split("|");
                    if (val != "" && val == Str(cdet[0])) { opt += '<li class="active" data-value="' + Str(cdet[0]) + '">' + Str(cdet[1]) + '</li>'; }
                    else if (val != "" && val == Str(cdet[1])) { opt += '<li class="active" data-value="' + Str(cdet[0]) + '">' + Str(cdet[1]) + '</li>'; }
                    else { opt += '<li data-value="' + Str(cdet[0]) + '">' + Str(cdet[1]) + '</li>'; }
                }
                return opt;
            }
        }
        catch (err) {
            toastr.error('Error while populating List :' + err, "Lighthouse eSolutions Pte. Ltd");
        }
    };

    function GetBuyerConfigSettings(_selectdefval, BUYERID) {   var headerdet = fnBConfgSettings(_selectdefval, BUYERID); $('#divByrConfg').empty().append(headerdet); $("#cbDefFormat").select2();  };

    function fnBConfgSettings(_selectdefval, BUYERID) {
        var sOut = ''; var _str = '';
        var _irfqchecked = ''; var _erfqchecked = ''; var _erfqackchecked = ''; var _iqtechecked = ''; var _eqtechecked = ''; var _ipochecked = ''; var _epochecked = '';
        var _epoackchecked = ''; var _epocchecked = ''; var _nbchecked = ''; var _nschecked = ''; var _DefFormat = ''; var _Mailsub = '';
        var slbyCfgdet = GetBuyerConfigDetail(_selectdefval, BUYERID);
        var tid = "bConfgTable"; var _tbodyid = "tblBodyBConfg"; var _bmapid = 'txtBMapping';
        var _ccemailid = 'txtCCEmail'; var _mailsbid = 'txtMailsub'; var _bimpid = 'txtBImpPath'; var _bexpid = 'txtBExpPath';
        var _wurlid = 'txtWebserURL'; var _dpriceid = 'txtDefPrice'; var _byrsndrCd = 'txtBuyerSenderCode';
        var _ufiletyid = 'txtUpFileType'; var _irfqid = 'chkImpRFQ'; var _erfqid = 'chkExpRFQ'; var _erfqackid = 'chkExpRFQAck';
        var _iqteid = 'chkImpQuote'; var _eqteid = 'chkExpQuote'; var _ipoid = 'chkImpPO'; var _epoid = 'chkExpPO'; var _epoackid = 'chkExpPOAck';
        var _epocid = 'chkExpPOC'; var _nbyid = 'chkNBuyer'; var _defformatid = 'cbDefFormat';
        _addressConfigID = (slbyCfgdet != '') ? Str(slbyCfgdet.ADDRCONFIGID) : '0';
        var _BMapping = (slbyCfgdet != '') ? Str(slbyCfgdet.PARTY_MAPPING) : '';
        var _CCEmail = (slbyCfgdet != '') ? Str(slbyCfgdet.CC_EMAIL) : '';
        if (slbyCfgdet != '') { _Mailsub = (Str(slbyCfgdet.MAIL_SUBJECT) != '') ? Str(slbyCfgdet.MAIL_SUBJECT) : _cnfgMailsubj; } else { _Mailsub = _cnfgMailsubj; }
        var _BImpPath = (slbyCfgdet != '') ? Str(slbyCfgdet.IMPORT_PATH) : '';
        var _BExpPath = (slbyCfgdet != '') ? Str(slbyCfgdet.EXPORT_PATH) : '';
        var _WebserURL = (slbyCfgdet != '') ? Str(slbyCfgdet.SUP_WEB_SERVICE_URL) : '';
        var _DefPrice = (slbyCfgdet != '') ? Str(slbyCfgdet.DEFAULT_PRICE) : '0';
        var _UpFileType = (slbyCfgdet != '') ? Str(slbyCfgdet.UPLOAD_FILE_TYPE) : '';
        var _ImpRFQ = (slbyCfgdet != '') ? Str(slbyCfgdet.IMPORT_RFQ) : ''; if (_ImpRFQ == '1') { _irfqchecked = 'checked'; } else { _irfqchecked = ''; }
        var _ExpRFQAck = (slbyCfgdet != '') ? Str(slbyCfgdet.EXPORT_RFQ_ACK) : ''; if (_ExpRFQAck == '1') { _erfqackchecked = 'checked'; } else { _erfqackchecked = ''; }
        var _ExpQuote = (slbyCfgdet != '') ? Str(slbyCfgdet.EXPORT_QUOTE) : ''; if (_ExpQuote == '1') { _eqtechecked = 'checked'; } else { _eqtechecked = ''; }
        var _ImpPO = (slbyCfgdet != '') ? Str(slbyCfgdet.IMPORT_PO) : ''; if (_ImpPO == '1') { _ipochecked = 'checked'; } else { _ipochecked = ''; }
        var _ExpPOAck = (slbyCfgdet != '') ? Str(slbyCfgdet.EXPORT_PO_ACK) : ''; if (_ExpPOAck == '1') { _epoackchecked = 'checked'; } else { _epoackchecked = ''; }
        var _ExpPOC = (slbyCfgdet != '') ? Str(slbyCfgdet.EXPORT_POC) : ''; if (_ExpPOC == '1') { _epocchecked = 'checked'; } else { _epocchecked = ''; }
        var _NBuyer = (slbyCfgdet != '') ? Str(slbyCfgdet.MAIL_NOTIFY) : ''; if (_NBuyer == '1') { _nbchecked = 'checked'; } else { _nbchecked = ''; }
        var _frmbyr = 'Import Path \n (From Buyer)'; var _tobyr = 'Export Path \n (To Buyer)'; _frmbyr = _frmbyr.replace(/\n/g, '<br/>'); _tobyr = _tobyr.replace(/\n/g, '<br/>');
        FillGroupFormat(); var _deffrmt = (slbyCfgdet != '') ? Str(slbyCfgdet.DEFAULT_FORMAT) : '';
       _DefFormat = FillCombo(_deffrmt, _lstgrpfrmt);  
        var sOut = '<div class="row">' +
                ' <div class="col-md-10"><div class="row"><div class="col-md-12"><div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Document Format </label> </div>' +
                ' <div  class="col-md-4"><select class="form-control" id="' + _defformatid + '">' + _DefFormat + '</select></div>' +            
                ' </div></div></div><div class="row" style="margin-top:3px;"><div class="col-md-12"><div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> ' + _frmbyr + ' </label> </div>' +
                ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _bimpid + '" value="' + _BImpPath + '"/></div>' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> ' + _tobyr + ' </label> </div>' +
                ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _bexpid + '" value="' + _BExpPath + '"/></div>' +            
                ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Mapping </label> </div>' +
                ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _bmapid + '"  value="' + _BMapping + '"/> </div>' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Default Price </label> </div>' +
                ' <div  class="col-md-4"><input type="text" class="form-control alignRight" id="' + _dpriceid + '"  value="' + _DefPrice + '"/> </div>' +
                ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Upload File Type </label> </div>' +
                ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _ufiletyid + '"  value="' + _UpFileType + '"/> </div>' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> CC Email </label> </div>' +
                ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _ccemailid + '"  value="' + _CCEmail + '"/> </div>' +
                ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Mail Subject </label> </div>' +
                ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" rows="3" id="' + _mailsbid + '">' + _Mailsub + '</textarea></div>' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Webservice URL </label> </div>' +
                ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" rows="3" id="' + _wurlid + '">' + _WebserURL + '</textarea></div>' +
                ' </div></div></div><div class="row"><div class="col-md-12"><div class="col-md-2"></div><div class="col-md-9"><div class="form-group">' +
                ' <ul class="checkbox-grid">' +
                ' <li><input class="widelarge" type="checkbox" id="' + _irfqid + '"  value="' + _ImpRFQ + '"' + _irfqchecked + ' /><label class="chklabel">Import RFQ from Buyer</label></li>' +               
                ' <li><input class="widelarge" type="checkbox" id="' + _erfqackid + '"  value="' + _ExpRFQAck + '"' + _erfqackchecked + '/><label class="chklabel">Export RFQ Ack. to Buyer</label></li>' +                
                ' <li><input class="widelarge" type="checkbox" id="' + _eqteid + '"  value="' + _ExpQuote + '"' + _eqtechecked + '/><label class="chklabel">Export Quote to Buyer</label></li>' +
                ' <li><input class="widelarge" type="checkbox" id="' + _ipoid + '"  value="' + _ImpPO + '"' + _ipochecked + '/><label class="chklabel">Import PO from Buyer</label></li>' +
                ' <li> <input class="widelarge" type="checkbox" id="' + _epoackid + '"  value="' + _ExpPOAck + '"' + _epoackchecked + '/> <label class="chklabel">Export PO Ack. to Buyer</label></li>' +
                ' <li> <input class="widelarge" type="checkbox" id="' + _epocid + '"  value="' + _ExpPOC + '"' + _epocchecked + '/><label class="chklabel">Export POC to Buyer</label></li>' +
                ' <li> <input class="widelarge" type="checkbox" id="' + _nbyid + '"  value="' + _NBuyer + '"' + _nbchecked + '/> <label class="chklabel">Notify Buyer</label></li>' +
                ' </div></div>'+
                ' </div></div></div></div>';
        return sOut;
    };

    function SaveBConfigDetails(BUYERID) {
        var _lstdet = []; var _defFormat = $('#cbDefFormat').val(); var _bmap = $('#txtBMapping').val(); var _ccemail = $('#txtCCEmail').val();
        var _mailsb = Str($('#txtMailsub').val()).replace(/\#/g, '_');
        var _bimp = $('#txtBImpPath').val(); var _bexp = $('#txtBExpPath').val();
        var _wurl = $('#txtWebserURL').val(); var _dprice = $('#txtDefPrice').val(); var _ufilety = $('#txtUpFileType').val();        
        var _irfq = ($('#chkImpRFQ').is(':checked')) ? 1 : 0;   var _erfqack = ($('#chkExpRFQAck').is(':checked')) ? 1 : 0; 
        var _eqte = ($('#chkExpQuote').is(':checked')) ? 1 : 0; var _ipo = ($('#chkImpPO').is(':checked')) ? 1 : 0; var _epoack = ($('#chkExpPOAck').is(':checked')) ? 1 : 0;
        var _epoc = ($('#chkExpPOC').is(':checked')) ? 1 : 0; var _nby = ($('#chkNBuyer').is(':checked')) ? 1 : 0;
        var _id = (_addressConfigID == '') ? 0 : _addressConfigID; _lstdet.push("ADDRESSCONFIGID" + "|" + Str(_id));
        _lstdet.push("DEFAULT_FORMAT" + "|" + Str(_defFormat)); _lstdet.push("MAPPING" + "|" + Str(_bmap)); _lstdet.push("ADDRESSID" + "|" + Str(BUYERID));
        _lstdet.push("IMPORT_PATH" + "|" + Str(_bimp)); _lstdet.push("EXPORT_PATH" + "|" + Str(_bexp));
        _lstdet.push("CC_EMAIL" + "|" + Str(_ccemail)); _lstdet.push("MAIL_SUBJECT" + "|" + Str(_mailsb)); _lstdet.push("IMPORT_RFQ" + "|" + Str(_irfq));
        _lstdet.push("EXPORT_RFQ_ACK" + "|" + Str(_erfqack));  _lstdet.push("EXPORT_QUOTE" + "|" + Str(_eqte)); _lstdet.push("IMPORT_PO" + "|" + Str(_ipo)); 
        _lstdet.push("EXPORT_PO_ACK" + "|" + Str(_epoack)); _lstdet.push("EXPORT_POC" + "|" + Str(_epoc)); _lstdet.push("NOTIFY_BUYER" + "|" + Str(_nby));
        _lstdet.push("DEFAULT_PRICE" + "|" + Str(_dprice)); _lstdet.push("UPLOAD_FILE_TYPE" + "|" + Str(_ufilety)); _lstdet.push("SUP_WEB_SERVICE_URL" + "|" + Str(_wurl));
        if (ValidateBuyerConfig()) { UpdateBuyerConfigDetails(_lstdet, GetBuyerConfigSettings, BUYERID, _defFormat) }
    };

    function ClearControls() {
        var _emptystr = ''; _addressConfigID = '0';
        $('#txtBMapping').val(_emptystr); $('#txtCCEmail').val(_emptystr); $('#txtMailsub').val(_emptystr); $('#txtBImpPath').val(_emptystr); $('#txtBExpPath').val(_emptystr);
        $('#txtWebserURL').val(_emptystr); $('#txtDefPrice').val(_emptystr); $('#txtUpFileType').val(_emptystr); $('#chkImpRFQ').removeAttr('checked');
        $('#chkExpRFQ').removeAttr('checked'); $('#chkExpRFQAck').removeAttr('checked'); $('#chkImpQuote').removeAttr('checked'); $('#chkExpQuote').removeAttr('checked');
        $('#chkImpPO').removeAttr('checked'); $('#chkExpPO').removeAttr('checked'); $('#chkExpPOAck').removeAttr('checked'); $('#chkExpPOC').removeAttr('checked');
        $('#chkNBuyer').removeAttr('checked'); $('#chkNSupp').removeAttr('checked');
    };

    function ValidateBuyerConfig() {
        var _valid = true; var _defFormat = $('#cbDefFormat').val();
        if (_defFormat == '') { $('#cbDefFormat').addClass('error'); _valid = false; } else { $('#cbDefFormat').removeClass('error'); }
        return _valid;
    };

    function UpdateBuyerConfigDetails(_nfieldval, callback, BUYERID, _defFormat) {
        var sldet = []; for (var j = 0; j < _nfieldval.length; j++) { sldet.push(_nfieldval[j]); }
        var data2send = JSON.stringify({ sldet: sldet });
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "BuyerDetail.aspx/SaveBuyerConfigDetails",
                data: data2send,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d);
                       // if (res != "") {  toastr.success("Lighthouse eSolutions Pte. Ltd", "Buyer Config Saved successfully."); callback(_defFormat, BUYERID); }
                        Metronic.unblockUI();
                    }
                    catch (err) { //toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Buyer Config :' + err); Metronic.unblockUI();
                    }
                },
                failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI();},
                error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
            });
        }, 20);
    };

    var GetBuyerFormat = function (BUYERID) {
        var _format = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "BuyerDetail.aspx/GetFormat_Buyer",
            data: "{'BUYERID':'" + BUYERID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    _format = Str(response.d);
                }
                catch (err) {  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer Format :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) {  toastr.error("error get", response.responseText); }
        });
        return _format;
    };
    /*end*/

    /*Linked Suppliers*/
    function GetLinkedSuppliers(BUYERID) {
        setupBSuppTableHeader(); var table1 = $('#dataGridBSlnk');
        oBSuppTable = table1.dataTable({
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
                "orderable": true, 'targets': [0],
            },
            { 'targets': [0], width: '20px' }, { 'targets': [1], width: '80px' }, { 'targets': [2, 3], width: '60px' }, { 'targets': [4], visible: false }
            ],
            "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]]
        });

        $('#tblHeadRowBSlnk').addClass('gridHeader'); $('#ToolTables_dataGridBSlnk_0,#ToolTables_dataGridBSlnk_1').hide();
        $(".dataTables_scrollBody table").css('min-width', "100%"); $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
        GetBSupplierGrid(BUYERID);
    };

    var GetBSupplierGrid = function (BUYERID) {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "BuyerDetail.aspx/FillLinkedSuppliersGrid",
            data: "{'BUYERID':'" + BUYERID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table; FillBSupplierGrid(Table); }
                    else $('#dataGridBSlnk').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Supplier Details :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };
    /*end*/

    /* Buyer Default Rules*/

    function GetBuyerRules(BUYERID) {
        setupBRulesTableHeader();var table2 = $('#dataGridBRules');
        oBRTable = table2.dataTable({
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
            "columnDefs": [{ "orderable": false, "searchable": false, 'targets': [0] },
            { 'targets': [0], width: '20px' }, { 'targets': [1, 6], width: '60px' }, { 'targets': [2], width: '50px' }, { 'targets': [3], width: '80px' },
            { 'targets': [4, 5], width: '120px' }, { 'targets': [7, 8], visible: false }
            ],
            "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
            "sScrollY": "300px",
            "sScrollX": true,
            "paging": true,
            "order": [[1, "asc"]],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var divid = 'dv' + nRow._DT_RowIndex; var aedit = 'rwedit' + nRow._DT_RowIndex; var adelete = 'rwdelete' + nRow._DT_RowIndex; var divlnkid = 'dvlnk' + nRow._DT_RowIndex;
                if (aData[8] != '') {
                    var detTag = ' <div id="' + divid + '" style="text-align:center;"><span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span>' +
                    '<span id=' + adelete + ' class="actionbtn" data-toggle="tooltip" title="Delete"><i class="glyphicon glyphicon-trash"></i></span></div>';
                    if (_isRuleupdate != nRow._DT_RowIndex) { $('td:eq(0)', nRow).html(detTag); }
                }
            }
        });

        $('#tblHeadRowBRule').addClass('gridHeader'); $('#ToolTables_dataGridBRules_0,#ToolTables_dataGridBRules_1').hide(); $('#dataGridBRules').css('width', '100%');
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
        var GROUP_FORMAT = GetBuyerFormat(BUYERID); GetBRulesGrid(BUYERID, GROUP_FORMAT);
    };

    var setupBRulesTableHeader = function () {
        var dtfilter = '<th style="text-align:center;">#</th><th>Rule No.</th><th>Doc Type</th><th>Rule Code</th><th>Description</th><th>Comments</th><th>Rule Value</th><th>RULE_ID</th><th>DEF_ID</th>';      
        $('#tblHeadRowBRule').empty().append(dtfilter); $('#tblBodyBRule').empty();
    };

    function SetBuyerRules(BUYERID) {
        setupRulesTableHeader();var table3 = $('#dataGridNewDRule');
        oNDRTable = table3.dataTable({
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
                "sRowSelect": "multiple",
                "aButtons": ["select_all", "select_none"]
            },
            "columnDefs": [{ 'orderable': false, "searching": true, "autoWidth": false, 'targets': [0] },
            { 'targets': [0], width: '5px' }, { 'targets': [1], width: '50px' }, { 'targets': [2], width: '50px' }, { 'targets': [3], width: '80px' },
            { 'targets': [4], width: '160px' }, { 'targets': [5], width: '200px' }, { 'targets': [6], width: '55px' }, { 'targets': [7], visible: false }
            ],
            "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
            "scrollX": "1000px",
            "scrollY": '300px',
            "paging": false,
            "aaSorting": [],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var _chkid = "chk" + nRow._DT_RowIndex; var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="' + _chkid + '"  /></div>';
                $('td:eq(0)', nRow).html(_chkdiv); var _id = 'cbRuleValue' + nRow._DT_RowIndex; var _defruleval = 'NOT SET';
                var _cbdiv = '<input type="text" class="form-control" id="' + _id + '" value="' + _defruleval + '"/>'; $('td:eq(6)', nRow).html(_cbdiv); $('#cbRuleValue').select2();
            }
        });
        $('#tblHeadRowNewDRule').addClass('gridHeader'); $('#ToolTables_dataGridNewDRule_0,#ToolTables_dataGridNewDRule_1').hide(); $('#dataGridNewDRule_paginate').hide();
        $('#dataGridNewDRule').css('width', '100%'); $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
        var GROUP_FORMAT = GetBuyerFormat(BUYERID); GetRulesGrid(BUYERID, GROUP_FORMAT); $('#ModalDefRule').modal('show');
    };

    function FillRules() {
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultRules.aspx/GetAllRules",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var Dataset = JSON.parse(response.d);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table; _lstRules = [];
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) {
                                for (var i = 0; i < Table.length; i++) { _lstRules.push(Str(Table[i].RULEID) + "|" + Str(Table[i].RULE_CODE)); }
                            }
                            else {   if (Table.RULEID != undefined) { _lstRules.push(Str(Table.RULEID) + "|" + Str(Table.RULE_CODE)); }  }
                        }
                    }
                }
                catch (err) { toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd"); }
            },
            failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
        });
    };

    function GetDefRulesdet(RULEID) {
        $.ajax({
            type: "POST",
            async: false,
            url: "BuyerDetail.aspx/GetRuleDetails",
            data: "{'RULEID':'" + RULEID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var _res = Str(response.d); _lstRuledetails = []; _lstRuledetails = _res.split('|');
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Rules details:' + err);}
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) {  toastr.error("error get", response.responseText); }
        });
    };
  
    var GetBRulesGrid = function (BUYERID, GROUP_FORMAT) {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "BuyerDetail.aspx/GetBuyerDefaultRules",
            data: "{'BUYERID':'" + BUYERID + "','GROUP_FORMAT':'" + GROUP_FORMAT + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) {  Table = DataSet.NewDataSet.Table; FillBRulesGrid(Table);}
                    else $('#dataGridBRules').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI();  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer Rules :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI();  },
            error: function (response) {    toastr.error("error get", response.responseText); Metronic.unblockUI();  }
        });
    };

    function FillBRulesGrid(Table) {
        try {
            $('#dataGridBRules').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridBRules').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array(); var r = jQuery('<tr id=' + i + '>'); var _arrcommt = Str(Table[i].RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str(''); cells[1] = Str(Table[i].RULE_NUMBER);   cells[2] = Str(Table[i].DOC_TYPE);
                    cells[3] = Str(Table[i].RULE_CODE);cells[4] = Str(Table[i].RULE_DESC); cells[5] = _arrcommt;
                    cells[6] = Str(Table[i].RULE_VALUE);cells[7] = Str(Table[i].RULE_ID); cells[8] = Str(Table[i].DEF_ID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.DEF_ID != undefined && Table.DEF_ID != null) {
                    var t = $('#dataGridBRules').dataTable();    var r = jQuery('<tr id=' + 1 + '>');var cells = new Array();
                    var _arrcommt = Str(Table.RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str(''); cells[1] = Str(Table.RULE_NUMBER); cells[2] = Str(Table.DOC_TYPE); cells[3] = Str(Table.RULE_CODE); cells[4] = Str(Table.RULE_DESC);
                    cells[5] = _arrcommt; cells[6] = Str(Table.RULE_VALUE); cells[7] = Str(Table.RULE_ID); cells[8] = Str(Table.DEF_ID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)
        { }
    };

    function ValidateDefRule(_rulecode, _ruleval, _lstMasterDet, nNew) {
        var _valid = true;
        if (_rulecode == '') { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Rule Code is blank'); _valid = false; }
        if (_ruleval == '') { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Rule Value is blank'); _valid = false; }
        if (nNew) { var _existrec = CheckExistingRule(_lstMasterDet); if (_existrec != '') { toastr.error("Lighthouse eSolutions Pte. Ltd.", _existrec); _valid = false; } }
        return _valid;
    };

    function SaveDefRule(_nfieldval, callback, _defaddrid, _defgrpformat) {
        var slRuledet = []; var _addressid = ''; var _grpFormat = ''; for (var j = 0; j < _nfieldval.length; j++) { slRuledet.push(_nfieldval[j]); }
        var data2send = JSON.stringify({ slRuledet: slRuledet });
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "DefaultRules.aspx/SaveRuleDetails",
                data: data2send,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d); if (res != "") {   GetBuyerRules(_defaddrid);  }
                        Metronic.unblockUI();
                    }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Rule details :' + err); Metronic.unblockUI(); }
                },
                failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
                error: function (response) {  toastr.error("error get", response.responseText); Metronic.unblockUI();}
            });
        }, 200);
    };

    function DeleteDefaultRule(DEF_ID, ADDRESSID, GROUP_FORMAT, RULE_CODE, DELALL, callback) {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultRules.aspx/DeleteRule",
            data: "{'DEF_ID':'" + DEF_ID + "', 'ADDRESSID':'" + ADDRESSID + "','RULE_CODE':'" + RULE_CODE + "','DELALL':'" + DELALL + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var res = Str(response.d);
                    if (res != "") {   toastr.success("Lighthouse eSolutions Pte. Ltd", "Rule Deleted."); callback(ADDRESSID);}
                    Metronic.unblockUI();
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete Rule :' + err); Metronic.unblockUI(); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };

    function UpdateBuyerDefRules(_defid, _defaddrid, _defgrpformat, _lstMasterDet, nNew) {
        var _hidvalue = '0';
        var _ruleid = $('#cbRuleCode option:selected').val(); var _rulecode_txt = $('#cbRuleCode option:selected').text(); var _ruleval = $('#cbRuleValue option:selected').val();
        _lstMasterDet.push('RULE_CODE' + "|" + Str(_rulecode_txt)); _lstMasterDet.push('RULEID' + "|" + Str(_ruleid)); _lstMasterDet.push('RULE_VALUE' + "|" + Str(_ruleval));
        _lstMasterDet.push('DEFAULT_RULE_ADDRESSID' + "|" + _defaddrid); _lstMasterDet.push('DEFAULT_RULE_GROUP_FORMAT' + "|" + _defgrpformat);
        var _res = ValidateDefRule(_rulecode_txt, _ruleval, _lstMasterDet, nNew);
        if (_res == true) {
            var _msg = 'Are you sure you want to override this rule for all the link for the Buyer/Supplier having inherited (Yes) ?';
            if (confirm(_msg)) { _hidvalue = '1'; } else { _hidvalue = '0'; }
            _lstMasterDet.push('HID_VALUE' + "|" + _hidvalue); _lstMasterDet.push('DEF_ID' + "|" + _defid);
            _isRuleupdate = -2; SaveDefRule(_lstMasterDet, GetBuyerRules, _defaddrid, _defgrpformat);
        }
        else { }
    };

    var CheckExistingRule = function (_nfieldval) {
        var slRuledet = []; var res = '';
        for (var j = 0; j < _nfieldval.length; j++) { slRuledet.push(_nfieldval[j]); }
        var data2send = JSON.stringify({ slRuledet: slRuledet });
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultRules.aspx/ValidateRule",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try { res = Str(response.d); }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to validate Rule details :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
        return res;
    };
 
    function FillRulesGrid(Table) {
        try {
            $('#dataGridNewDRule').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridNewDRule').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();var r = jQuery('<tr id=' + i + '>');
                    var _arrcommt = Str(Table[i].RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str(''); cells[1] = Str(Table[i].RULE_NUMBER);cells[2] = Str(Table[i].DOC_TYPE);
                    cells[3] = Str(Table[i].RULE_CODE); cells[4] = Str(Table[i].RULE_DESC);  cells[5] = _arrcommt; cells[6] = Str(Table[i].RULE_VALUE);  cells[7] = Str(Table[i].RULEID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.RULEID != undefined && Table.RULEID != null) {
                    var t = $('#dataGridNewDRule').dataTable();  var r = jQuery('<tr id=' + 1 + '>');   var cells = new Array();
                    var _arrcommt = Str(Table.RULE_COMMENTS).split(',').join('<br/>');
                    cells[0] = Str(''); cells[1] = Str(Table.RULE_NUMBER);  cells[2] = Str(Table.DOC_TYPE);
                    cells[3] = Str(Table.RULE_CODE);cells[4] = Str(Table.RULE_DESC);cells[5] = _arrcommt; cells[6] = Str(Table.RULE_VALUE);   cells[7] = Str(Table.RULEID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e) { }
    };

    var GetRulesGrid = function (ADDRESSID, GROUP_FORMAT) {
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "DefaultRules.aspx/FillUnLinkedDefaultRules",
            data: "{'ADDRESSID':'" + ADDRESSID + "','GROUP_FORMAT':'" + GROUP_FORMAT + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) { Table = DataSet.NewDataSet.Table; FillRulesGrid(Table); }
                    else $('#dataGridNewDRule').DataTable().clear().draw();
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer Rules :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
            error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };

    var setupRulesTableHeader = function () {
        var dtfilter = '<th></th><th>Rule No.</th><th>Doc Type</th><th>Rule Code</th><th>Description</th><th>Comments</th><th>Rule Value</th><th>RULEID</th>';
        $('#tblHeadRowNewDRule').empty().append(dtfilter); $('#tblBodyNewDRule').empty();
    };
    /*end*/

    /*Buyer Mapping*/
    function MappingSettings() {
        $('#tblXls').show(); $('#tblPdf').hide(); setupBXLSMappingTableHeader(); setupBPDFMappingTableHeader();
    };

    var setupBXLSMappingTableHeader = function () {
      $('#tblBodyBXlsMapp').empty();
      var dtfilter = '<th style="text-align:center;">#</th><th>Map Code</th><th>Buyer Code</th><th>Buyer Name</th><th>Supplier Code</th><th>Supplier Name</th><th>Cell-1</th>' +
          '<th>Cell-1 Value-1</th><th>Cell-1 Value-2</th><th>Cell-2</th><th>Cell-2 Value</th><th>Cell (No Discount)</th> <th>Cell Value (No Discount)</th> ' +
            ' <th>XLS_BUYER_MAPID</th><th>EXCEL_MAPID</th><th>BUYER_SUPP_LINKID</th><th>BUYERID</th><th>SUPPLIERID</th> <th>GROUP_ID</th><th>DOC_TYPE</th>';
        $('#tblHeadRowBXlsMapp').empty().append(dtfilter);
    };

    function GetBuyerExcelMapp() {
      var table12 = $('#dataGridBXlsMapp');
        oBXlsMappTable = table12.dataTable({
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
            "columnDefs": [{ "orderable": true, 'targets': [0] },
            { 'targets': [0], width: '5px' }, { 'targets': [1], width: '50px' },
            { 'targets': [6, 9], width: '30px' }, { 'targets': [7], width: '120px' }, { 'targets': [8], width: '60px' }, { 'targets': [10], width: '60px' },
            { 'targets': [4, 5, 2, 3, 11, 12, 13, 14, 15, 16, 17, 18, 19], visible: false }
            ],
            "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var divid = 'dv' + nRow._DT_RowIndex; var aedit = 'rwedit' + nRow._DT_RowIndex;
                var detTag = ' <div id="' + divid + '" style="text-align:center;"><span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span></div>';
                if (_isxlsupdate != nRow._DT_RowIndex) { $('td:eq(0)', nRow).html(detTag); }
            }
        });
        $('#tblHeadRowBXlsMapp').addClass('gridHeader'); $('#ToolTables_dataGridBXlsMapp_0,#ToolTables_dataGridBXlsMapp_1').hide(); $('#dataGridBXlsMapp').css('width', '100%');
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
        GetMappingXLSGrid(BUYERID, 'BUYER');
    };

    function GetMappingXLSGrid(ADDRESSID, ADDRTYPE) {
        var DataSet = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "XLS_Buyer_Config.aspx/GetXLSBuyerConfig_Addressid",
            data: "{'ADDRESSID':'" + ADDRESSID + "','ADDRTYPE':'" + ADDRTYPE + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                   var DataSet = JSON.parse(response.d); if (DataSet.NewDataSet != null) { var Table = DataSet.NewDataSet.Table; FillBMappingXLSGrid(Table); }
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get XLS BuyerConfig data :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
    };

    function FillBMappingXLSGrid(Table) {
        try {
            $('#dataGridBXlsMapp').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridBXlsMapp').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str('');cells[1] = Str(Table[i].FORMAT_MAP_CODE);cells[2] = Str(Table[i].BUYER_CODE);  cells[3] = Str(Table[i].BUYER_NAME);
                    cells[4] = Str(Table[i].SUPPLIER_CODE);cells[5] = Str(Table[i].SUPPLIER_NAME); cells[6] = Str(Table[i].MAP_CELL1);
                    cells[7] = Str(Table[i].MAP_CELL1_VAL1);  cells[8] = Str(Table[i].MAP_CELL1_VAL2);  cells[9] = Str(Table[i].MAP_CELL2);
                    cells[10] = Str(Table[i].MAP_CELL2_VAL);cells[11] = Str(Table[i].MAP_CELL_NODISC);  cells[12] = Str(Table[i].MAP_CELL_NODISC_VAL);
                    cells[13] = Str(Table[i].XLS_BUYER_MAPID); cells[14] = Str(Table[i].EXCEL_MAPID);  cells[15] = Str(Table[i].BUYER_SUPP_LINKID);
                    cells[16] = Str(Table[i].BUYERID); cells[17] = Str(Table[i].SUPPLIERID);  cells[18] = Str(Table[i].GROUP_ID); cells[19] = Str(Table[i].DOC_TYPE);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.XLS_BUYER_MAPID != undefined && Table.XLS_BUYER_MAPID != null) {
                    var t = $('#dataGridBXlsMapp').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str(''); cells[1] = Str(Table.FORMAT_MAP_CODE);  cells[2] = Str(Table.BUYER_CODE);cells[3] = Str(Table.BUYER_NAME);
                    cells[4] = Str(Table.SUPPLIER_CODE); cells[5] = Str(Table.SUPPLIER_NAME); cells[6] = Str(Table.MAP_CELL1); cells[7] = Str(Table.MAP_CELL1_VAL1);
                    cells[8] = Str(Table.MAP_CELL1_VAL2); cells[9] = Str(Table.MAP_CELL2); cells[10] = Str(Table.MAP_CELL2_VAL);
                    cells[11] = Str(Table.MAP_CELL_NODISC);  cells[12] = Str(Table.MAP_CELL_NODISC_VAL); cells[13] = Str(Table.XLS_BUYER_MAPID);  cells[14] = Str(Table.EXCEL_MAPID);
                    cells[15] = Str(Table.BUYER_SUPP_LINKID);cells[16] = Str(Table.BUYERID); cells[17] = Str(Table.SUPPLIERID);  cells[18] = Str(Table.GROUP_ID); cells[19] = Str(Table.DOC_TYPE);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e) { }
    }

    function SaveXLSMappingDetails(_nfieldval) {
        var slXLSMapdet = [];
        for (var j = 0; j < _nfieldval.length; j++) {
            slXLSMapdet.push(_nfieldval[j]);
        }
        var data2send = JSON.stringify({ slXLSMapdet: slXLSMapdet });
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "XLS_Buyer_Config.aspx/UpdateXLSMapping",
                data: data2send,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d);
                        if (res != "") {
                            toastr.success("Lighthouse eSolutions Pte. Ltd", "XLS Mapping Saved successfully."); GetMappingXLSGrid(BUYERID, 'BUYER');
                        }
                        Metronic.unblockUI();
                    }
                    catch (err) {
                        toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update XLS Mapping details :' + err); Metronic.unblockUI();
                    }
                },
                failure: function (response) {
                    toastr.error("failure get", response.d); Metronic.unblockUI();
                },
                error: function (response) {
                    toastr.error("error get", response.responseText); Metronic.unblockUI();
                }
            });
        }, 200);
    };

    function GetXMappRowDetails(Table, nTr, _lstdet, _targetclick) {
        var indx = nTr.rowIndex; var tid = "MappTable" + indx;
        var _mcell1 = $('#txtCell1' + indx).val(); var _mcell1val1 = $('#txtCell1value1' + indx).val(); var _mcell1val2 = $('#txtCell1value2' + indx).val();
        var _mcell2 = $('#txtCell2' + indx).val(); var _mcell2val1 = $('#txtCell2value1' + indx).val();
        var _mCellnodis = $('#txtCellnodis' + indx).val(); var _mcellvalnodis = $('#txtCellvaluenodis' + indx).val();
        var _grpid = $('#cbGroup' + indx + ' option:selected').val(); if (_grpid == '' || _grpid == undefined) { _grpid = ''; } var _doctype = $('#cbDocType' + indx + ' option:selected').val();
        var _xlbuyermapid = Table.fnGetData(nTr)[13]; var _buyid = Table.fnGetData(nTr)[16]; var _suppid = Table.fnGetData(nTr)[17];
        _lstdet.push("GROUP_ID" + "|" + Str(_grpid)); _lstdet.push("MAP_CELL1" + "|" + Str(_mcell1));
        _lstdet.push("MAP_CELL1_VAL1" + "|" + Str(_mcell1val1)); _lstdet.push("MAP_CELL1_VAL2" + "|" + Str(_mcell1val2)); _lstdet.push("MAP_CELL2" + "|" + Str(_mcell2));
        _lstdet.push("MAP_CELL2_VAL" + "|" + Str(_mcell2val1)); _lstdet.push("MAP_CELL_NODISC" + "|" + Str(_mCellnodis));
        _lstdet.push("MAP_CELL_NODISC_VAL" + "|" + Str(_mcellvalnodis)); _lstdet.push("DOC_TYPE" + "|" + Str(_doctype));
        _lstdet.push("XLS_BUYER_MAPID" + "|" + Str(_xlbuyermapid)); 
    };

    var setupBPDFMappingTableHeader = function () {
        $('#tblHeadRowBPdfMapp').empty(); $('#tblBodyBPdfMapp').empty();
        var dtfilter = '<th style="text-align:center;">#</th><th>Map Code</th><th>Doc Type</th><th>Buyer Code</th><th>Buyer Name</th><th>Supplier Code</th><th>Supplier Name</th> <th>Map Range (1)</th><th>Map Range (1) Value</th><th>Map Range (2)</th><th>Map Range (2) Value</th><th>Map Range (3)</th><th>Map Range (3) Value</th> ' +
            ' <th>MAP_ID</th><th>PDF_MAPID</th><th>BUYER_SUPP_LINKID</th><th>BUYERID</th><th>SUPPLIERID</th> <th>GROUP_ID</th>';
        $('#tblHeadRowBPdfMapp').empty().append(dtfilter);
    };

    function GetBuyerPDFMapp() {         
     var table13 = $('#dataGridBPdfMapp');
        oBPdfMappTable = table13.dataTable({
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
            "columnDefs": [{ "orderable": true, 'targets': [0] },
            { 'targets': [0], width: '10px' }, { 'targets': [1], width: '40px' }, { 'targets': [2], width: '50px' }, { 'targets': [5], width: '75px' }, { 'targets': [7, 9, 11], width: '60px' },
             { 'targets': [8, 10, 12], width: '70px' }, { 'targets': [5, 6, 3, 4, 13, 14, 15, 16, 17, 18], visible: false }
            ],
            "scrollX": true,
            "lengthMenu": [
                [25, 50, 100, -1],
                [25, 50, 100, "All"]
            ],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var divid = 'dv' + nRow._DT_RowIndex; var aedit = 'rwedit' + nRow._DT_RowIndex;
                var detTag = ' <div id="' + divid + '" style="text-align:center;"><span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span></div>';
                if (_ispdfupdate != nRow._DT_RowIndex) { $('td:eq(0)', nRow).html(detTag); }
            }
        });

        $('#tblHeadRowBPdfMapp').addClass('gridHeader'); $('#ToolTables_dataGridBPdfMapp_0,#ToolTables_dataGridBPdfMapp_1').hide();
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%"); $('#dataGridBPdfMapp').css('width', '100%');  
        GetMappingPDFGrid(BUYERID,'BUYER');};

    function GetMappingPDFGrid(ADDRESSID, ADDRTYPE) {
        var DataSet = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "PDF_Buyer_Config.aspx/GetPDFBuyerConfig_Addressid",
            data: "{'ADDRESSID':'" + ADDRESSID + "','ADDRTYPE':'" + ADDRTYPE + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try { DataSet = JSON.parse(response.d); if (DataSet.NewDataSet != null) { var Table = DataSet.NewDataSet.Table; FillBMappingPDFGrid(Table); } }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get PDF BuyerConfig data :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
    };

    function FillBMappingPDFGrid(Table) {
        try {
            $('#dataGridBPdfMapp').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridBPdfMapp').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str('');  cells[1] = Str(Table[i].FORMAT_MAP_CODE);  cells[2] = Str(Table[i].DOC_TYPE);
                    cells[3] = Str(Table[i].BUYER_CODE); cells[4] = Str(Table[i].BUYER_NAME);cells[5] = Str(Table[i].SUPPLIER_CODE);
                    cells[6] = Str(Table[i].SUPPLIER_NAME);  cells[7] = Str(Table[i].MAPPING_1);cells[8] = Str(Table[i].MAPPING_1_VALUE);
                    cells[9] = Str(Table[i].MAPPING_2);   cells[10] = Str(Table[i].MAPPING_2_VALUE);cells[11] = Str(Table[i].MAPPING_3); cells[12] = Str(Table[i].MAPPING_3_VALUE);
                    cells[13] = Str(Table[i].MAP_ID);  cells[14] = Str(Table[i].PDF_MAPID);   cells[15] = Str(Table[i].BUYER_SUPP_LINKID);
                    cells[16] = Str(Table[i].BUYERID); cells[17] = Str(Table[i].SUPPLIERID); cells[18] = Str(Table[i].GROUP_ID);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.PDF_MAPID != undefined && Table.PDF_MAPID != null) {
                    var t = $('#dataGridBPdfMapp').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str('');cells[1] = Str(Table.FORMAT_MAP_CODE); cells[2] = Str(Table.DOC_TYPE); cells[3] = Str(Table.BUYER_CODE);
                    cells[4] = Str(Table.BUYER_NAME); cells[5] = Str(Table.SUPPLIER_CODE);  cells[6] = Str(Table.SUPPLIER_NAME);  cells[7] = Str(Table.MAPPING_1);
                    cells[8] = Str(Table.MAPPING_1_VALUE); cells[9] = Str(Table.MAPPING_2); cells[10] = Str(Table.MAPPING_2_VALUE);cells[11] = Str(Table.MAPPING_3);
                    cells[12] = Str(Table.MAPPING_3_VALUE); cells[13] = Str(Table.MAP_ID); cells[14] = Str(Table.PDF_MAPID); cells[15] = Str(Table.BUYER_SUPP_LINKID);
                    cells[16] = Str(Table.BUYERID); cells[17] = Str(Table.SUPPLIERID); cells[18] = Str(Table.GROUP_ID);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e) { }
    };

    function fnXlsFormatDetails(oTable, nTr, _targetclick) {
        var sOut = ''; var _str = ''; var _status = ''; var _selectopt = ''; var _grptxt = ''; var _doctxt = '';
        var indx = nTr.rowIndex; var aData = oTable.fnGetData(nTr); FillXlsDocTypeList();
        var tid = "MappTable" + indx; var _tbodyid = "tblBodyXLSByCfg" + indx;
        var _Bycodeid = 'txBuyercode' + indx; var _ByNameid = 'txBuyername' + indx;
        var _mcell1id = 'txtCell1' + indx; var _mcell1val1id = 'txtCell1value1' + indx; var _mcell1val2id = 'txtCell1value2' + indx; var _mcell2id = 'txtCell2' + indx;
        var _mcell2val1id = 'txtCell2value1' + indx; var _mcellnodisid = 'txtCellnodis' + indx; var _mcellvalnodisid = 'txtCellvaluenodis' + indx;
        var _grpid = 'cbGroup' + indx; var _docid = 'cbDocType' + indx;
        if (_targetclick == 'Edit') { _grptxt = Str(aData[1]); _doctxt = Str(aData[19]); } else { _grptxt = ''; _doctxt = ''; }
        var _BCode = Str(aData[2]); var _BName = Str(aData[3]);
        var _mapcell1 = (_targetclick == 'Edit') ? Str(aData[6]) : '';
        var _mapcell1val1 = (_targetclick == 'Edit') ? Str(aData[7]) : '';
        var _mapcell1val2 = (_targetclick == 'Edit') ? Str(aData[8]) : '';
        var _mapcell2 = (_targetclick == 'Edit') ? Str(aData[9]) : '';
        var _mapcell2val1 = (_targetclick == 'Edit') ? Str(aData[10]) : '';
        var _mapcellnodis = (_targetclick == 'Edit') ? Str(aData[11]) : '';
        var _mapcellvalnodis = (_targetclick == 'Edit') ? Str(aData[12]) : '';
        var cbdoctype = FillCombo(_doctxt, _lstDocType);
        var _celldis = 'Cell \n (No Discount)'; var _cellvaldis = 'Cell Value \n (No Discount)'; _celldis = _celldis.replace(/\n/g, '<br/>'); _cellvaldis = _cellvaldis.replace(/\n/g, '<br/>');
        var btndiv = '<div class="row"><div style="text-align:center;"><a href="#" id="btnXLSUpdate"><u>Update</u></<a>&nbsp;<a href="#" id="btnXLSCancel"><u>Cancel</u></<a></div></div>';
        var sOut = '<div class="row" style="padding-right:63px;"><div class="col-md-10"><div class="row"><div class="col-md-10"><div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel">Document Type</label> </div>' +
                ' <div  class="col-md-3"><select class="bs-select form-control" id="' + _docid + '">' + cbdoctype + '</select> </div>' +
                ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Code </label> </div>' +
                ' <div  class="col-md-3"><span id="' + _Bycodeid + '">' + _BCode + '</span> </div>' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Name </label></div>' +
                ' <div  class="col-md-3"> <span id="' + _ByNameid + '">' + _BName + '</span> </div>' +
                ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Cell-1 </label> </div>' +
                ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mcell1id + '"  value="' + _mapcell1 + '"/></div>' +
                ' </div></div></div><div class="row"><div class="col-md-10"> <div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Cell-1 Value-1 </label> </div>' +
                ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mcell1val1id + '" value="' + _mapcell1val1 + '"/></div>' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Cell-1 Value-2 </label></div>' +
                ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mcell1val2id + '" value="' + _mapcell1val2 + '"/></div>' +
                ' </div></div></div> <div class="row"><div class="col-md-10"> <div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Cell-2 </label> </div>' +
                ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mcell2id + '"  value="' + _mapcell2 + '"/></div>' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Cell-2 Value </label></div>' +
                ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mcell2val1id + '" value="' + _mapcell2val1 + '"/></div>' +
                ' </div></div></div><div class="row"><div class="col-md-10"> <div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> ' + _celldis + ' </label> </div>' +
                ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mcellnodisid + '"  value="' + _mapcellnodis + '"/></div>' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel">' + _cellvaldis + '  </label></div>' +
                ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _mcellvalnodisid + '" value="' + _mapcellvalnodis + '"/></div>' +
                ' </div></div></div>' + btndiv + '</div></div>';
        return sOut;
    }

    function fnPDFFormatDetail(oTable, nTr, _targetclick) {
        var sOut = ''; var _str = ''; var _status = ''; var _selectopt = ''; var _grptxt = ''; var _doctxt = ''; var _ipdisable = 'disabled';
        var indx = nTr.rowIndex; var aData = oTable.fnGetData(nTr); var tid = "MappTable" + indx; var _tbodyid = "tblBodyPDFByCfg" + indx;
        var _Bycodeid = 'txBuyercode' + indx; var _ByNameid = 'txBuyername' + indx;
        var _mrange1id = 'txtMapRange1' + indx; var _mrangeval1id = 'txtMapRangevalue1' + indx; var _mrange2id = 'txtMapRange2' + indx;
        var _mrangeval2id = 'txtMapRangevalue2' + indx; var _mrange3id = 'txtMapRange3' + indx; var _mrangeval3id = 'txtMapRangevalue3' + indx;
        var _docid = 'cbDocType' + indx; FillPdfDocTypeList();
        if (_targetclick == 'Edit') { _grptxt = Str(aData[1]); _doctxt = Str(aData[2]); } else { _grptxt = ''; _doctxt = ''; _ipdisable = ''; }
        var _BCode = Str(aData[3]); var _BName = Str(aData[4]); var _SCode = Str(aData[5]); var _SName = Str(aData[6]);
        var _maprange1 = (_targetclick == 'Edit') ? Str(aData[7]) : '';
        var _maprangeval1 = (_targetclick == 'Edit') ? Str(aData[8]) : '';
        var _maprange2 = (_targetclick == 'Edit') ? Str(aData[9]) : '';
        var _maprangeval2 = (_targetclick == 'Edit') ? Str(aData[10]) : '';
        var _maprange3 = (_targetclick == 'Edit') ? Str(aData[11]) : '';
        var _maprangeval3 = (_targetclick == 'Edit') ? Str(aData[12]) : '';
        var cbdoctype = FillCombo(_doctxt, _lstDocType);
        var btndiv = '<div class="row"><div style="text-align:center;"><a href="#" id="btnPDFUpdate"><u>Update</u></<a>&nbsp;<a href="#" id="btnPDFCancel"><u>Cancel</u></<a></div></div>';
        var sOut = '<div class="row"><div class="col-md-10"><div class="row"><div class="col-md-10"><div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel">Document Type</label> </div>' +
                ' <div  class="col-md-3"><select class="bs-select form-control" id="' + _docid + '">' + cbdoctype + '</select> </div>' +
                ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Code </label> </div>' +
                ' <div  class="col-md-3"><span id="' + _Bycodeid + '">' + _BCode + '</span> </div>' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyer Name </label></div>' +
                ' <div  class="col-md-3"> <span id="' + _ByNameid + '">' + _BName + '</span> </div>' +
                ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (1) </label> </div>' +
                ' <div  class="col-md-3"><input type="text" class="form-control" ' + _ipdisable + ' id="' + _mrange1id + '"  value="' + _maprange1 + '"/></div>' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (1) Value </label></div>' +
                ' <div  class="col-md-3"><input type="text" class="form-control" ' + _ipdisable + '  id="' + _mrangeval1id + '" value="' + _maprangeval1 + '"/></div>' +
                ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (2) </label> </div>' +
                ' <div  class="col-md-3"> <input type="text" class="form-control" ' + _ipdisable + '  id="' + _mrange2id + '"  value="' + _maprange2 + '"/></div>' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (2) Value </label></div>' +
                ' <div  class="col-md-3"><input type="text" class="form-control" ' + _ipdisable + ' id="' + _mrangeval2id + '" value="' + _maprangeval2 + '"/> </div>' +
                ' </div></div></div><div class="row"><div class="col-md-10"><div class="form-group">' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (3) </label> </div>' +
                ' <div  class="col-md-3"><input type="text" class="form-control" ' + _ipdisable + ' id="' + _mrange3id + '"  value="' + _maprange3 + '"/></div>' +
                ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Map Range (3) Value </label></div>' +
                ' <div  class="col-md-3"><input type="text" class="form-control" ' + _ipdisable + ' id="' + _mrangeval3id + '" value="' + _maprangeval3 + '"/> </div>' +
                ' </div></div></div>' + btndiv + '</div></div>';
        return sOut;
    }

    function GetPMappRowDetails(Table, nTr, _lstdet) {
        var indx = nTr.rowIndex; var tid = "MappTable" + indx;
        var _maprange1 = $('#txtMapRange1' + indx).val(); var _maprangeval1 = $('#txtMapRangevalue1' + indx).val();
        var _maprange2 = $('#txtMapRange2' + indx).val(); var _maprangeval2 = $('#txtMapRangevalue2' + indx).val();
        var _maprange3 = $('#txtMapRange3' + indx).val(); var _maprangeval3 = $('#txtMapRangevalue3' + indx).val();
        var _grpid = $('#cbGroup' + indx + ' option:selected').val(); if (_grpid == '' || _grpid == undefined) { _grpid = ''; } var _docid = $('#cbDocType' + indx + ' option:selected').val();
        var _mapid = Table.fnGetData(nTr)[13]; var _buyid = Table.fnGetData(nTr)[16]; var _suppid = Table.fnGetData(nTr)[17];
        _lstdet.push("GROUP_ID" + "|" + Str(_grpid)); _lstdet.push("DOC_TYPE" + "|" + Str(_docid));
        _lstdet.push("MAPPING_1" + "|" + Str(_maprange1)); _lstdet.push("MAPPING_1_VALUE" + "|" + Str(_maprangeval1));
        _lstdet.push("MAPPING_2" + "|" + Str(_maprange2)); _lstdet.push("MAPPING_2_VALUE" + "|" + Str(_maprangeval2));
        _lstdet.push("MAPPING_3" + "|" + Str(_maprange3)); _lstdet.push("MAPPING_3_VALUE" + "|" + Str(_maprangeval3));
        _lstdet.push("MAPID" + "|" + Str(_mapid)); 
    };

    function SavePDFMappingDetails(_nfieldval) {
        var slPDFMapdet = [];
        for (var j = 0; j < _nfieldval.length; j++) {
            slPDFMapdet.push(_nfieldval[j]);
        }
        var data2send = JSON.stringify({ slPDFMapdet: slPDFMapdet });
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "PDF_Buyer_Config.aspx/UpdatePDFMapping",
                data: data2send,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d);
                        if (res != "") {
                            toastr.success("Lighthouse eSolutions Pte. Ltd", "PDF Mapping Details Saved successfully."); GetMappingPDFGrid(BUYERID, 'BUYER');
                        }
                        Metronic.unblockUI();
                    }
                    catch (err) {
                        toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update PDF Mapping details :' + err); Metronic.unblockUI();
                    }
                },
                failure: function (response) {
                    toastr.error("failure get", response.d); Metronic.unblockUI();
                },
                error: function (response) {
                    toastr.error("error get", response.responseText); Metronic.unblockUI();
                }
            });
        }, 200);
    };

    function FillXlsDocTypeList() {
        _lstDocType = [];
        $.ajax({
            type: "POST",
            async: false,
            url: "XLS_Buyer_Config.aspx/GetXLSDoctypes",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var Dataset = JSON.parse(response.d);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table;
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) {
                                for (var i = 0; i < Table.length; i++) {
                                    _lstDocType.push(Str(Table[i].DOCTYPE) + "|" + Str(Table[i].DOCTYPE));
                                }
                            }
                            else {
                                if (Table.DOCTYPE != undefined) {
                                    _lstDocType.push(Str(Table.DOCTYPE) + "|" + Str(Table.DOCTYPE));
                                }
                            }
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

    function FillPdfDocTypeList() {
        _lstDocType = [];
        $.ajax({
            type: "POST",
            async: false,
            url: "PDF_Buyer_Config.aspx/GetPDFDoctypes",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var Dataset = JSON.parse(response.d);
                    if (Dataset.NewDataSet != null) {
                        var Table = Dataset.NewDataSet.Table;
                        if (Table != undefined && Table != null) {
                            if (Table.length != undefined) {
                                for (var i = 0; i < Table.length; i++) {
                                    _lstDocType.push(Str(Table[i].DOCTYPE) + "|" + Str(Table[i].DOCTYPE));
                                }
                            }
                            else {
                                if (Table.DOCTYPE != undefined) {
                                    _lstDocType.push(Str(Table.DOCTYPE) + "|" + Str(Table.DOCTYPE));
                                }
                            }
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
    /*end*/

    /*Buyer Transactions*/
    function InitialiseBuyerTransactn() {
        setupBTransctTableHeader(); var table11 = $('#dataGridBTrans');
        oBTransTable = table11.dataTable({
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
            "columnDefs": [{ "orderable": true, 'targets': [0] },
           { "sType": "date-dmy", "bSortable": true, 'targets': [0], width: '40px' }, { 'targets': [1, 2], width: '68px' }, { 'targets': [3], width: '40px' },
           { 'targets': [4, 5], width: '45px' }, { 'targets': [6], width: '220px' }, { 'targets': [7], width: '90px' }, { 'targets': [8, 9], visible: false }
            ],
            "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
            "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
            "fnRowCallback": function (nRow, aData, iDisplayIndex) { var fileTag = '<a href="javascript:;"><u>' + aData[9] + '</u></<a>'; $('td:eq(7)', nRow).html(fileTag); }
        });

        $('#tblHeadRowBTrans').addClass('gridHeader'); $('#ToolTables_dataGridBTrans_0,#ToolTables_dataGridBTrans_1').hide(); $('#dataGridBTrans').css('width', '100%');
        $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
    };

    function GetBuyerTransactions(FROM, TO, BUYERID) { _lstfilter = SetFilter(_FROM, TO, BUYERID); GetBTransactGrid(_lstfilter);};// GetBuyerOverview(BUYERID);

    var SetFilter = function (FROM, TO, BUYERID) {
        _lstfilter = []; _fromlogdate = $(document.getElementById('dtLogFromDate')).val(); _tologdate = $(document.getElementById('dtLogToDate')).val();    
        _lstfilter.push("LOG_FROM" + "|" + getSQLDateFormated(_fromlogdate));_lstfilter.push("LOG_TO" + "|" + getSQLDateFormated(_tologdate)); _lstfilter.push("ADDRESSID" + "|" + Str(BUYERID));
        _lstfilter.push("FROM" + "|" + Str(FROM)); _lstfilter.push("TO" + "|" + Str(TO));
        return _lstfilter;
    };

    var GetBTransactGrid = function (_nfieldval) {
        var slftrdet = [];for (var j = 0; j < _nfieldval.length; j++) { slftrdet.push(_nfieldval[j]); }
        var data2send = JSON.stringify({ slftrdet: slftrdet });
        Metronic.blockUI('#portlet_body');
        $.ajax({
            type: "POST",
            async: false,
            url: "AuditLog.aspx/GetAuditLog_AddressID",
            data: data2send,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    if (Str(response.d) != '') {
                        var _totalcount = response.d.split('||')[0];  var DataSet = JSON.parse(response.d.split('||')[1]);
                        if (DataSet.NewDataSet != null) {   var Table = DataSet.NewDataSet.Table; FillBTransGrid(Table); }
                        else $('#dataGridBTrans').DataTable().clear().draw();
                    }
                    Metronic.unblockUI();
                }
                catch (err) { Metronic.unblockUI();toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer Transactions :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI();},
            error: function (response) {  toastr.error("error get", response.responseText); Metronic.unblockUI(); }
        });
    };

    function FillBTransGrid(Table) {
        try {
            $('#dataGridBTrans').DataTable().clear().draw();
            if (Table.length != undefined && Table.length != null) {
                var t = $('#dataGridBTrans').dataTable();
                for (var i = 0; i < Table.length; i++) {
                    var cells = new Array();
                    var r = jQuery('<tr id=' + i + '>');
                    cells[0] = Str(Table[i].UPDATE_DATE);cells[1] = Str(Table[i].SERVERNAME); cells[2] = Str(Table[i].MODULENAME); cells[3] = Str(Table[i].KEYREF2);
                    cells[4] = Str(Table[i].DOCTYPE);cells[5] = Str(Table[i].LOGTYPE);cells[6] = Str(Table[i].AUDITVALUE);
                    cells[7] = Str(Table[i].FILENAME);  cells[8] = Str(Table[i].LOGID);  cells[9] = Str(Table[i].FILENAME);
                    var ai = t.fnAddData(cells, false);
                }
                t.fnDraw();
            }
            else {
                if (Table.LOGID != undefined && Table.LOGID != null) {
                    var t = $('#dataGridBTrans').dataTable();
                    var r = jQuery('<tr id=' + 1 + '>');
                    var cells = new Array();
                    cells[0] = Str(Table.UPDATE_DATE); cells[1] = Str(Table.SERVERNAME); cells[2] = Str(Table.MODULENAME); cells[3] = Str(Table.KEYREF2);
                    cells[4] = Str(Table.DOCTYPE); cells[5] = Str(Table.LOGTYPE); cells[6] = Str(Table.AUDITVALUE);
                    cells[7] = Str(Table.FILENAME); cells[8] = Str(Table.LOGID); cells[9] = Str(Table.FILENAME);
                    var ai = t.fnAddData(cells, false);
                    t.fnDraw();
                }
            }
        }
        catch (e)  { }
    };

    var setupBTransctTableHeader = function () {
        var dtfilter = '<th>Log Date</th><th>Server Name</th><th>Module Name</th><th>Key Ref</th><th>Doc Type</th><th>Log Type</th><th>Remark</th><th>File Name</th><th>LOGID</th><th>FileName</th>';
        $('#tblHeadRowBTrans').empty().append(dtfilter); $('#tblBodyBTrans').empty(); setFilterToolbar();
    };

    var setFilterToolbar = function () {
        $('#divFilter').empty();
        var _filterdet = ' <div class="row">  <div class="col-md-12">'+
             ' <div class="col-md-4" style="text-align:right;">' +
             ' <div class="col-md-4"><label class="control-label">Log From </label>  </div>' +
             ' <div class="col-md-8"> <input class="form-control date-picker csDatePicker InputText" data-date-format="dd/mm/yyyy" size="16" type="text" id="dtLogFromDate" value=""/> </div>' +
             ' </div> <div class="col-md-4" style="text-align:right;">' +
             ' <div class="col-md-4"><label class="control-label" style="text-align:right;">Log To </label>  </div>' +
             ' <div class="col-md-8"> <input class="form-control date-picker csDatePicker InputText" data-date-format="dd/mm/yyyy" size="16" type="text" id="dtLogToDate" value=""/> </div>' +
             ' </div></div></div>';
        $('#divFilter').append(_filterdet); var date = new Date();
        $(document.getElementById('dtLogFromDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
        $(document.getElementById('dtLogToDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
        _fromlogdate = $(document.getElementById('dtLogFromDate')).val(); _tologdate = $(document.getElementById('dtLogToDate')).val();
    };

    function ClearFilter() {
        _lstfilter = []; setFilterToolbar(); $('#dataGridBTrans').DataTable().clear().draw();
    };

    function DownloadFile(UPDATEDATE, MODULENAME, FILENAME) {
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
                        if (res != '0' && res != '') {
                            var filefullpath = res.split('|')[0]; var filename = res.split('|')[1];
                            if (filename != undefined && filename != '') {
                                //var cVirtualPath = "../Downloads/"; var win = window.open(cVirtualPath + filename, '_blank'); win.focus();
                                top.location.href = "../Downloads/" + filename;
                                toastr.success("Lighthouse eSolutions Pte. Ltd", "Download File success");
                            }
                        }
                        else { toastr.error("Lighthouse eSolutions Pte. Ltd", "Unable to download file. Path not found."); }
                    }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Download File :' + err); }
                },
                failure: function (response) { toastr.error("failure get", response.d); },
                error: function (response) { toastr.error("error get", response.responseText); }
            });

        }
        catch (e) { }
    };
    /*end*/

    /*Buyer Overview*/
    function GetBuyerOverview(BUYERID) {
        var _nocount = '0';
        $.ajax({
            type: "POST",
            async: false,
            url: "Overview.aspx/SetOverview_Addressid",
            data: "{'ADDRESSID':'" + BUYERID + "','ADDRTYPE':'BUYER'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    _sdet = JSON.parse(response.d);
                    for (var x in _sdet) {
                        if (x == 'THISWEEK') {
                            var lstTWk = []; lstTWk = Str(_sdet[x]).split('|');
                            $('#spnRfq').text(lstTWk[0]); $('#spnQte').text(lstTWk[1]); $('#spnPO').text(lstTWk[2]); $('#spnPOC').text(lstTWk[3]);
                        }
                        else if (x == 'LASTWEEK') {
                            var lstLWk = []; lstLWk = Str(_sdet[x]).split('|');
                            $('#spnRfqLastWk').text(lstLWk[0]); $('#spnQteLastWk').text(lstLWk[1]); $('#spnPOLastVWk').text(lstLWk[2]); $('#spnPOCLastWk').text(lstLWk[3]);
                        }
                        else if (x == 'THISMONTH') {
                            var lstTMth = []; lstTMth = Str(_sdet[x]).split('|');
                            $('#spnRfqTMth').text(lstTMth[0]); $('#spnQteTMth').text(lstTMth[1]); $('#spnPOTMth').text(lstTMth[2]); $('#spnPOCTMth').text(lstTMth[3]);
                        }
                        else if (x == 'LASTMONTH') {
                            var lstLMth = []; lstLMth = Str(_sdet[x]).split('|');
                            $('#spnRfqLMth').text(lstLMth[0]); $('#spnQteLMth').text(lstLMth[1]); $('#spnPOLMth').text(lstLMth[2]); $('#spnPOCLMth').text(lstLMth[3]);
                        }
                        else if (x == 'THISYEAR') {
                            var lstTYr = []; lstTYr = Str(_sdet[x]).split('|');
                            $('#spnRfqTYr').text(lstTYr[0]); $('#spnQteTYr').text(lstTYr[1]); $('#spnPOTYr').text(lstTYr[2]); $('#spnPOCTYr').text(lstTYr[3]);
                        }
                    }
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer Overview:' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
    };
    /*end*/

    /*Sync Servers*/
    function GetServerNames() {
        var _lstServer = [];
        $.ajax({
            type: "POST",
            async: false,
            url: "../Common/Default.aspx/GetServerList",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {                    
                    if (response.d != '' && response.d != undefined) {
                        var _server = Str(response.d).split(',');
                        _lstServer = [];
                        for (i = 0; i < _server.length; i++) {
                            var _servname = _server[i].split('|')[0];
                            _lstServer.push(Str(_servname));
                        }
                    }
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to Get Server names :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText); }
        });
        return _lstServer;
    };

    function SyncDetails(ID, _nfieldval, _nfieldval1) {
        var slServdet = []; for (var k = 0; k < _nfieldval.length; k++) { slServdet.push(_nfieldval[k]); }
        var slServpaths = []; for (var k = 0; k < _nfieldval1.length; k++) { slServpaths.push(_nfieldval1[k]); }
        var data2send = JSON.stringify({ ID: ID, slServdet: slServdet, slServpaths: slServpaths, cInsertdet: '2' });
        Metronic.blockUI('#portlet_body');
        setTimeout(function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "NewBuyerSupplierWizard.aspx/SyncDetails_server",
                data: data2send,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d); if (res == '1') { toastr.success("Lighthouse eSolutions Pte. Ltd.", 'Supplier details synched successfully'); $('#ModalServConfrm').modal('hide'); $('#ModalSyncServers').modal('hide'); }
                        else { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to Sync Supplier'); } Metronic.unblockUI();
                    }
                    catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to Sync Supplier :' + err); Metronic.unblockUI(); }
                },
                failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
                error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
            });
        }, 200);
    };
    /*end*/
    return { init: function () { handleBuyerDetailTable(); }
        
    };
}();

function Navigate(data) {
    var _supplierid = data.id + '#2'; var _suppliercd = Str(data.innerHTML);
    var strencrurl = getEncryptedData('SUPPLIERID=' + _supplierid + "&SUPPLIER_CODE=" + _suppliercd); var url = "../LESMonitorPages/SupplierDetail.aspx?" + strencrurl; window.open(url,'Supplier Detail','_blank');
};

    
