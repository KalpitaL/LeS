var selectdTr = ''; var previousTr = ''; var bspreviousTr = ''; var irefpreviousTr = ''; var Suppdet = ''; var BSRpreviousTr = '';
var iuompreviousTr = ''; var updtcnt = '';var brefdelchk = '';var buomdelchk = '';var _defrule = []; var _lstSuppdet = []; var _lstgrp = []; var _lstBSLnkdet = []; var _lstIRefdet = []; var _lstIUOMdet = []; var _lstlogindet = []; var _lstDoc = [];
var _defaddrid = ''; var _configaddrid = ''; var _addrtype = ''; var SUPPLIERID = ''; var BUYERID = ''; var LINKID = ''; var _orgRow = '';var CODE='';
var _spCnfgdet = []; var _lstgrpfrmt = []; var _addressConfigID = '0'; var _configpath = ''; var _selectdefval = ''; var _cnfgMailsubj = ''; var _lstRules = []; var _lstRuleval = [];
var _lstMasterDet = []; var previousSTr = ''; var _lstRuledetails = [];
var _lstfilter = []; var _FROM = 1; var _TO = 200; var _orgdiff = 200; var _totalcount = ''; var nEditing = null; var _isSupupdate = -2; var _isBSupupdate = -2; var _isBSItemupdate = -2; var _isBSItemUomupdate = -2; var nNew = false; var _newcont = 0;
 var _isRuleupdate = -2; var _isxlsupdate = -2; var _ispdfupdate = -2;
 var oBSLTable = ''; var oBSIRefTable = ''; var oBuyerTable = ''; var oSRTable = ''; var oSTransTable = ''; var oSXlsMappTable = ''; var oSPdfMappTable = '';
 var oNDRTable = ''; var oBSRTable = ''; var oBSItemUMTable = ''; var _lstSyncservers = []; var _lstControlsname = [];

   var SupplierDetail = function () {

   var handleSupplierDetailTable = function () {
           SetupBreadcrumb('Home', 'Home.aspx', 'Connected Suppliers', 'Suppliers.aspx', 'Supplier Details', 'SupplierDetail.aspx');
           $('#divSpacer').remove(); $('#divFilterWrapper').remove();
           Params = getDecryptedData();
           if (Params != '' && Params != undefined) { if (Params.length > 1) { SUPPLIERID = Params[0].split('|')[1].split('#')[0]; CODE = Params[1].split('|')[1]; sessionStorage['SUPPLIERID'] = SUPPLIERID; } }
           $('#pageTitle').empty().append('Supplier Details (' + CODE + ')');setToolbar(); GetSupplier_DefSettings(SUPPLIERID, _selectdefval); setaccess('0');
           oBSIRefTable = $('#dataGridBSItemRef'); oBuyerTable = $('#dataGridNewBuy'); oSRTable = $('#dataGridSRules'); oSTransTable = $('#dataGridSTrans');
           oSXlsMappTable = $('#dataGridSXlsMapp'); oSPdfMappTable = $('#dataGridSPdfMapp'); oNDRTable = $('#dataGridNewDRule'); oBSLTable = $('#dataGridBSlnk'); oBSItemUMTable = $('#dataGridBSItemUOM'); oBSRTable = $('#dataGridBSLnkRules');
           $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
               var target = e.target.innerHTML; setaccess(Str(e.target.id));
               if (Str(target) == 'Detail') { GetSupplier_DefSettings(SUPPLIERID, _selectdefval); }
               else if (Str(target) == 'Default Rules') { GetSupplierRules(SUPPLIERID); }
               else if (Str(target) == 'Linked Buyers') { setBuyerSupplinkGrid(SUPPLIERID); }
               else if (Str(target) == 'Linked Rules') { setBuyerSupplinkRulesGrid(LINKID); }
               else if (Str(target) == 'Item Reference Mapping') { setBuyerSuppItemRefGrid(LINKID); }
               else if (Str(target) == 'Item UOM Reference Mapping') { setBuyerSuppItemUOMGrid(); }
               else if (Str(target) == 'Mappings') { GetSupplierExcelMapp(SUPPLIERID); }
               else if (Str(target) == 'Transactions') { InitialiseSuppTransactn(); GetSupplierTransactions(_FROM, _TO, SUPPLIERID); }
           });
           $('#btnClose').removeAttr('href').on('click', function (event) { event.preventDefault(); top.window.close(); });
           $('#btnRefresh').on('click', function (e) {
               e.preventDefault(); var activeTab = $('#hTabs .active').text();
               if (activeTab == 'Detail') { GetSupplier_DefSettings(SUPPLIERID, _selectdefval); }
               else if (activeTab == 'Default Rules') { GetSupplierRules(SUPPLIERID); }
               else if (activeTab == 'Linked Buyers') { GetBuyerSuppLinkGrid(SUPPLIERID); }
               else if (activeTab == 'Linked Rules') { setBuyerSupplinkRulesGrid(LINKID); }
               else if (activeTab == 'Mappings') { GetSupplierExcelMapp(SUPPLIERID); }
               else if (activeTab == 'Transactions') { GetSupplierTransactions(_FROM, _TO, SUPPLIERID); }
           });
           $('#btnCancel').on('click', function (e) { e.preventDefault(); GetSupplier_DefSettings(SUPPLIERID, _selectdefval); });
           $('#btnSave').on('click', function (e) {
               e.preventDefault();
               var activeTab = $('#hTabs .active').text(); if (activeTab == 'Detail') {
                   GetSupplierDet_Update(SUPPLIERID);
                   if (_lstSyncservers.length > 0) {
                       _lstControlsname = [];
                       _lstControlsname.push($('#txtCSImpPath').val()); _lstControlsname.push($('#txtCSExpPath').val());  _lstControlsname.push($('#txtUploadPath').val());   _lstControlsname.push($('#txtDownloadPath').val());
                       SyncDetails(SUPPLIERID, _lstSyncservers, _lstControlsname);
                   }
               }
           });
           $('#btnDelete').on('click', function (e) { e.preventDefault(); if (confirm('Are you sure ? You want to delete this Supplier ?')) { DeleteSupplier(SUPPLIERID); window.close(); } });
           $('#btnNew').on('click', function (e) {
               e.preventDefault();
               var activeTab = $('#hTabs .active').text();
               if (activeTab == 'Linked Buyers') {
                   setBuyerGrid();
                   var _urldet = getEncryptedData('ADDRESSID=' + SUPPLIERID); var _wizurl = "../LESMonitorPages/BuyerSupplier_ConfirmLink_Wizard.aspx?" + _urldet; window.open(_wizurl, '_blank');
               }
               else if (activeTab == 'Default Rules') { SetSupplierRules(); }
           });
           $('#btnItmRefresh').on('click', function (e) {
               e.preventDefault(); var activeTab = $('#hItmTabs .active').text(); if (activeTab == 'Item Reference Mapping') { }
               else if (activeTab == 'Item UOM Reference Mapping') { }
           });
           $('#btnItmNew').on('click', function (e) {
               e.preventDefault(); var activeTab = $('#hItmTabs .active').text();
               if (activeTab == 'Item Reference Mapping') {
                   var divtag = fnItemRefFormatDetails(oBSIRefTable, '', 'New');
                   $('#dvNewBSItemRefDet').empty(); $('#dvNewBSItemRefDet').append(divtag); $("#ModalNewBSItemRef").modal('show');
               }
               else if (activeTab == 'Item UOM Reference Mapping') { if (_newcont == 0) { newRow(); _newcont++; } }
               else if (activeTab == 'Linked Rules') {
                   var _linkid = Str(sessionStorage.getItem('LINKID')); setNewRules_Grid(); GetNewRuleGrid(_linkid); $("#ModalNewRule").modal('show');
               };
           });
           $('#btnItmUpload').on('click', function (e) {
               e.preventDefault(); var activeTab = $('#hItmTabs .active').text();
               if (activeTab == 'Item Reference Mapping') {
                   $('#btnDownload').val('BIREF'); $('#btnDownload').text('Download Template'); $('#LnkBSItemdettitle').empty(); $('#LnkBSItemdettitle').append('Buyer Supplier Item Reference');
               }
               else if (activeTab == 'Item UOM Reference Mapping') {
                   $('#btnDownload').val('BIUOM'); $('#btnDownload').text('Download UOM Template'); $('#LnkBSItemdettitle').empty(); $('#LnkBSItemdettitle').append('Buyer Supplier Item UOM Mapping');
               }
               $('#ModalUpload').modal('show');
           });
           $('#btnItmClose').on('click', function (e) { e.preventDefault(); setaccess('2'); $('#ModalNewBSItemDet').modal('hide'); });           
           $('#btnNewBSItemRef').click(function () {
               var aRow = ''; var _buyerid = ''; _lstIRefdet = [];
               var _buyid = Str(sessionStorage.getItem('BUYERID')); var _suppid = Str(SUPPLIERID); var _linkid = Str(sessionStorage.getItem('LINKID'));
               _lstIRefdet.push("BUYERID" + "|" + Str(_buyid)); _lstIRefdet.push("SUPPLIERID" + "|" + Str(_suppid)); _lstIRefdet.push("LINKID" + "|" + Str(_linkid));
               GetItemRefDetails(oBSIRefTable, '', _lstIRefdet, 'New')
               var _res = ValidateItemRef(0); if (_res == true) {
                   SaveItemRefDetails(_lstIRefdet, GetBSItemRefGrid, _linkid); $("#ModalNewBSItemRef").modal('hide')
               }
           });
           $('#btnDownload').on("click", function (e) { var type = $('#btnDownload').val(); var _linkid = Str(sessionStorage.getItem('LINKID')); DownloadTemplate(_linkid, type); });
           $('#btnChangePwd').on("click", function (e) { var _newPwd = $('#txtChangePwd').val(); var _userid = $('#spuserid').text(); SaveChangePassword(Str(_newPwd), Str(_userid)); closeLogin(); });
           $('#btnClosePwd').on("click", function (e) { closeLogin(); });
           $('#btnBSUpdate').click(function (e) {
               e.preventDefault(); _lstBSLnkdet = [];
               var _suppid = Str(SUPPLIERID); var _linkid = Str(sessionStorage.getItem('LINKID'));
               var _res = ValidateBSLink(_linkid); if (_res == true) {
                   var _indx = sessionStorage['SELECTEDINDEX']; _lstBSLnkdet = GetBSLinkDetails(_indx);
                   SaveBSupplierLinkDetails(_lstBSLnkdet, GetBuyerSuppLinkGrid, _linkid, _suppid); $("#ModalBSdet").modal('hide');
               }
           });
           $('#btnNewRule').click(function (e) {
               e.preventDefault(); _scheckid = []; var _linkid = Str(sessionStorage.getItem('LINKID'));
               $('input[type=checkbox]:checked').each(function () {
                   var tr = $(this).closest('tr'); if (oNRTable.fnGetData(tr) != null) {
                       var _ruleid = oNRTable.fnGetData(tr)[5];
                       _scheckid.push(Str(_ruleid));
                   }
               });
               AddNewLinkRule(_linkid, _scheckid); $("#ModalNewRule").modal('hide');
           });
           oSRTable.on('click', 'tbody td', function (e) {
               var _defaddrid = SUPPLIERID; var _defgrpformat = GetSupplierFormat(SUPPLIERID);
               var selectedSTr = $(this).parents('tr')[0]; var aData = oSRTable.fnGetData(selectedSTr); var divid = 'dv' + selectedSTr._DT_RowIndex;
               if (e.target.className == 'glyphicon glyphicon-pencil') {
                   _isRuleupdate = selectedSTr._DT_RowIndex; if (previousSTr != '' && previousSTr != selectedSTr) { restoreRow(previousSTr, oSRTable); }
                   nNew = false; editDefRuleRow(oSRTable, selectedSTr); previousSTr = selectedSTr;
               }
               else if (e.target.className == 'glyphicon glyphicon-trash') {
                   var _hidvalue = '0'; var _defid = aData[8]; var rulecode = aData[3]; var deleteRule = confirm('Are you sure? You want to delete this rule?');
                   if (deleteRule) {
                       var deleteAll = confirm('Are you sure? You want to delete this rule from all related Buyer-Supplier links ?'); if (deleteAll) { _hidvalue = '1'; }
                       DeleteDefaultRule(_defid, _defaddrid, _defgrpformat, rulecode, _hidvalue, GetSupplierRules);
                   }
                   previousSTr = '';
               }
               else if (e.target.className == 'glyphicon glyphicon-floppy-save') {
                   var _hidvalue = '0'; var _defid = '0'; _lstMasterDet = []; if (nNew) { _defid = '0'; } else { _defid = aData[8]; }
                   UpdateSupplierDefRules(_defid, _defaddrid, _defgrpformat, _lstMasterDet, false); previousSTr = ''; _newcont = 0; _isRuleupdate = -2;
               }
               else if (e.target.className == 'glyphicon glyphicon-ban-circle') { _isRuleupdate = -2; gridCancelEdit(nNew, nEditing, oSRTable); previousSTr = ''; _newcont = 0; }
           });
           oSTransTable.on('click', ' tbody td', function (e) {
               var nTr = $(this).parents('tr')[0]; var colIndx = $(this).index();
               var updatedate = oSTransTable.fnGetData(nTr)[0]; var modulename = oSTransTable.fnGetData(nTr)[2]; var logid = oSTransTable.fnGetData(nTr)[8]; var filename = oSTransTable.fnGetData(nTr)[9];
               if (colIndx == 7) { if (Str(filename) != "") { DownloadFile(updatedate, modulename, filename); } }
           });
           oSXlsMappTable.on('click', 'tbody td', function (e) {
               var selectedTr = $(this).parents('tr')[0]; var aData = oSXlsMappTable.fnGetData(selectedTr); _targetclick = '';
               if (oSXlsMappTable.fnIsOpen(selectedTr) && ((e.target.className == 'glyphicon glyphicon-pencil') || (e.target.innerText == 'Copy'))) {
                   oSXlsMappTable.fnClose(selectedTr);
               }
               else {
                   var divid = 'dv' + selectedTr._DT_RowIndex;
                   if (e.target.className == 'glyphicon glyphicon-pencil') {
                       _isxlsupdate = selectedTr._DT_RowIndex; _targetclick = e.target.innerText;
                       if ((previousTr != '') && (oSXlsMappTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {
                           var prevdivid = 'dv' + previousTr._DT_RowIndex; oSXlsMappTable.fnClose(previousTr); $('#' + prevdivid).show();
                       }
                       if (aData != null) {
                           oSXlsMappTable.fnOpen(selectedTr, fnXlsFormatDetails(oSXlsMappTable, selectedTr, 'Edit'), 'details');
                           $('#' + divid).hide(); previousTr = selectedTr;
                       }
                   }
                   $('#btnXLSUpdate').click(function () {
                       if (aData != null) {
                           var _res = ValidateDetail(selectedTr.rowIndex);
                           if (_res == true) {
                               _mapdet = []; _isxlsupdate = -2;
                               GetXMappRowDetails(oSXlsMappTable, selectedTr, _mapdet, e.target.innerText); UpdateXMapdet(_mapdet, GetBuyerExcelMapp, e.target.innerText);
                               $('#' + divid).show();
                           }
                       }
                   });
                   $('#btnXLSCancel').click(function () { if (oSXlsMappTable.fnIsOpen(selectedTr)) { oSXlsMappTable.fnClose(selectedTr); } $('#' + divid).show(); _isxlsupdate = -2; });
               }
           });
           oSPdfMappTable.on('click', 'tbody td', function (e) {
               var selectedTr = $(this).parents('tr')[0]; _selectedrow = selectedTr; var aData = oSPdfMappTable.fnGetData(selectedTr); _targetclick = '';
               if (oSPdfMappTable.fnIsOpen(selectedTr) && ((e.target.className == 'glyphicon glyphicon-pencil') || (e.target.innerText == 'Copy'))) {
                   oSPdfMappTable.fnClose(selectedTr);
               }
               else {
                   var divid = 'dv' + selectedTr._DT_RowIndex;
                   if (e.target.className == 'glyphicon glyphicon-pencil') {
                       _ispdfupdate = selectedTr._DT_RowIndex; _targetclick = e.target.innerText;
                       if ((previousTr != '') && (oSPdfMappTable.fnIsOpen(previousTr) && previousTr != selectedTr)) {
                           var prevdivid = 'dv' + previousTr._DT_RowIndex; oSPdfMappTable.fnClose(previousTr); $('#' + prevdivid).show();
                       }
                       if (aData != null) {
                           oSPdfMappTable.fnOpen(selectedTr, fnPDFFormatDetails(oSPdfMappTable, selectedTr, 'Edit'), 'details');
                           GetPMappRowDetails(oSPdfMappTable, selectedTr, _mapdet, 'Edit'); $('#' + divid).hide(); previousTr = selectedTr;
                       }
                   }

                   $('#btnPDFUpdate').click(function () {
                       if (aData != null) {
                           var _res = ValidateDetail(selectedTr.rowIndex);
                           if (_res == true) {
                               _ispdfupdate = -2; _mapdet = [];
                               GetPMappRowDetails(oSPdfMappTable, selectedTr, _mapdet, e.target.innerText); UpdatePMapdet(_mapdet, GetBuyerPDFMapp, e.target.innerText);
                               $('#' + divid).show();
                           }
                       }
                   });
                   $('#btnPDFCancel').click(function () { if (oSPdfMappTable.fnIsOpen(selectedTr)) { oSPdfMappTable.fnClose(selectedTr); } $('#' + divid).show(); _ispdfupdate = -2; });
               }
           });
           oNDRTable.on('change', 'tr', function (e) {
               var rowIndx = $(this).index();
               if (e.target.id == 'cbRuleValue' + rowIndx) {
                   var val = $('#' + e.target.id).val(); var isresult = (val != ""); if (isresult) { $('#chk' + rowIndx).prop('checked', (val != "NOT SET")); }
                   else { $('#chk' + rowIndx).prop('checked', false); }
               }
           });
           oBSLTable.on('click', 'tbody td', function (e) {
               var bslselectedTr = $(this).parents('tr')[0]; var oBSL_Table = $('#dataGridBSlnk').DataTable();
               var aData = oBSLTable.fnGetData(bslselectedTr);
               if (aData != null) { var _linkid = aData[2]; sessionStorage['LINKID'] = Str(_linkid); }
               var cellindx = $(this).index(); updtcnt = 0; var divid = 'dvbs' + bslselectedTr._DT_RowIndex;
               if (e.target.className == 'glyphicon glyphicon-pencil') {
                   _isBSupupdate = bslselectedTr._DT_RowIndex;
                   if (aData != null) {
                       var _linkid = aData[2]; sessionStorage['LINKID'] = Str(_linkid); var divtag = fnBSFormatDetails(oBSLTable, bslselectedTr);
                       $('#dvBSDet').empty().append(divtag); $("#ModalBSdet").modal('show');
                   }
               }
               if (e.target.className == 'glyphicon glyphicon-trash') {
                   if (confirm('Are you sure? You want to delete this Buyer-Supplier Link ?')) {
                       var _linkid = aData[2]; var _suppid = Str(SUPPLIERID); DeleteBSLink(_linkid, _suppid, GetBuyerSuppLinkGrid);
                   }
               }
               if (e.target.className == 'glyphicon glyphicon-search') {
                   var _linkid = aData[2]; var _buyerid = aData[47]; sessionStorage['BUYERID'] = Str(_buyerid); var _suppid = Str(SUPPLIERID);
                   setBuyerSupplinkRulesGrid(_linkid); $('#hItmTabs .active').removeClass('active'); $('#lnkLnkRule').addClass('active'); $("#ModalNewBSItemDet").modal('show');
               }
           });
           $("#ModalBSdet").on('shown.bs.modal', function () {
               $('#btnLgnCreate').click(function (e) {
                   var _btntxt = Str(e.target.innerHTML);
                   $('#dvLoginInfo').empty(); var logindet = LoginDetails(_btntxt); $('#dvLoginInfo').append(logindet); $("#ModalLoginInfo").modal('show');
               });
           });
           $("#ModalLoginInfo").on('shown.bs.modal', function () {
               $('#btnPWD').click(function (e) {
                   $('#dvChangePWD').empty(); $('#dvLoginInfo').css('display', 'none'); $('#dvChangePWD').css('display', 'block');
                   var logindet = ChangePassword(); $('#dvLoginft').css('display', 'block'); $('#dvChangePWD').append(logindet);
               });
           });
           $("#ModalLnkBuySupp .close").click(function () { _isBSupupdate = -2; $("#ModalLnkBuySupp").modal('hide'); }); jQuery('.ajax__fileupload_dropzone').remove();
           oBSIRefTable.on('click', 'tbody td', function (e) {
               var selectedIRefTr = $(this).parents('tr')[0]; var aData = oBSIRefTable.fnGetData(selectedIRefTr); var cellindx = $(this).index();
               updtcnt = 0;
               if (oBSIRefTable.fnIsOpen(selectedIRefTr) && (e.target.innerText == 'Edit')) { oBSIRefTable.fnClose(selectedIRefTr); }
               else {
                   var divid = 'dviref' + selectedIRefTr._DT_RowIndex;
                   if (e.target.className == 'glyphicon glyphicon-pencil') {
                       _isBSItemupdate = selectedIRefTr._DT_RowIndex;
                       if ((irefpreviousTr != '') && (oBSIRefTable.fnIsOpen(irefpreviousTr) && irefpreviousTr != selectedIRefTr)) {
                           var prevdivid = 'dviref' + irefpreviousTr._DT_RowIndex; oBSIRefTable.fnClose(irefpreviousTr); $('#' + prevdivid).show();
                       }
                       if (aData != null) {
                           oBSIRefTable.fnOpen(selectedIRefTr, fnItemRefFormatDetails(oBSIRefTable, selectedIRefTr, 'Edit'), 'details');
                           $('#' + divid).hide(); irefpreviousTr = selectedIRefTr;
                       }
                   }
                   if (e.target.className == 'glyphicon glyphicon-trash') {
                       if (confirm('Are you sure? You want to delete this Item Ref Mapping ?')) {
                           var _refid = aData[8]; var _linkid = Str(sessionStorage.getItem('LINKID')); DeleteItemRef(_linkid, _refid, GetBSItemRefGrid);
                       }
                   }

                   $('#btnIRefUpdate').click(function () {
                       _lstIRefdet = [];
                       if (aData != null && updtcnt == 0) {
                           var _buyid = Str(sessionStorage.getItem('BUYERID')); var _suppid = Str(sessionStorage.getItem('SUPPLIERID')); var _linkid = Str(sessionStorage.getItem('LINKID'));
                           _lstIRefdet.push("BUYERID" + "|" + Str(_buyid)); _lstIRefdet.push("SUPPLIERID" + "|" + Str(_suppid)); _lstIRefdet.push("LINKID" + "|" + Str(_linkid));
                           GetItemRefDetails(oBSIRefTable, selectedIRefTr, _lstIRefdet, 'Edit')
                           var _res = ValidateItemRef(selectedIRefTr.rowIndex);
                           if (_res == true) { _isBSItemupdate = -2; SaveItemRefDetails(_lstIRefdet, GetBSItemRefGrid, _linkid); } else { }
                       } $('#' + divid).show();
                   });

                   $('#btnIRefCancel').click(function () {
                       if (oBSIRefTable.fnIsOpen(selectedIRefTr)) { oBSIRefTable.fnClose(selectedIRefTr); } $('#' + divid).show(); _isBSItemupdate = -2;
                   });
               }
           });
           function fnItemRefFormatDetails(oTable, nTr, target) {
               var sOut = ''; var _str = ''; var _reftype = ''; var _buyerref = ''; var _suppref = ''; var _descr = ''; var _rmk = ''; var btndiv = '';
               var indx = (target == 'Edit') ? nTr.rowIndex : 0;
               var tid = "ItemRefTable" + indx; var _tbodyid = "tblBodyItemRef" + indx;
               var _refid = 'txtRefType' + indx; var _brefid = 'txtBuyerRef' + indx; var _srefid = 'txtSupplierRef' + indx; var _descid = 'txtDescr' + indx; var _rmkid = 'txtRemarks' + indx;
               if (target == 'Edit') {
                   var aData = oBSIRefTable.fnGetData(nTr);
                   _reftype = Str(aData[1]); _buyerref = Str(aData[2]); _suppref = Str(aData[3]); _descr = Str(aData[4]); _rmk = Str(aData[5]);
                   btndiv = '<div class="row"><div style="text-align:center;"><a href="#" id="btnIRefUpdate"><u>Update</u></<a>&nbsp;<a href="#" id="btnIRefCancel"><u>Cancel</u></<a></div></div>';
               }
               var sOut = '<div class="row"><div class="col-md-12"><div class="form-group">' +
                         ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Ref. Type </label> </div>' +
                         ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _refid + '"  value="' + _reftype + '"/> </div>' +
                         ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Buyer Ref. </label> </div>' +
                         ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _brefid + '"  value="' + _buyerref + '"/> </div>' +
                         ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                         ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Supplier Ref. </label> </div>' +
                         ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _srefid + '"  value="' + _suppref + '"/> </div>' +
                         ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Description </label> </div>' +
                         ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _descid + '"  value="' + _descr + '"/> </div>' +
                         ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                         ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Remarks </label> </div>' +
                         ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" rows="3" id="' + _rmkid + '">' + _rmk + '</textarea></div>' +
                         ' </div></div></div>' + btndiv;
               return sOut;
           };
           oBSItemUMTable.on('click', 'tbody td', function (e) {
               var UMselectedTr = $(this).parents('tr')[0]; var aData = oBSItemUMTable.fnGetData(UMselectedTr);
               var divid = 'dvuom' + UMselectedTr._DT_RowIndex;
               if (e.target.innerText == 'Edit') {
                   if (iuompreviousTr != '' && iuompreviousTr != UMselectedTr) { restoreRow(iuompreviousTr, oBSItemUMTable); } nNew = false;
                   editRow(oBSItemUMTable, UMselectedTr); iuompreviousTr = UMselectedTr;
               }
               else if (e.target.className == 'glyphicon glyphicon-trash') {
                   if (confirm('Are you sure? You want to delete this Item UOM ?')) {
                       var _itemuomid = aData[5]; var _suppid = aData[4]; var _buyerid = aData[3]; DeleteItemUOM(_itemuomid, _suppid, _buyerid, GetBSItemUOMGrid);
                   }
               }
               else if (e.target.className == 'glyphicon glyphicon-floppy-save') {
                   var _itemuomid = ''; _lstIUOMdet = []; var _biuom = $('#txtbUOM').val(); var _siuom = $('#txtSUOM').val(); _itemuomid = (nNew) ? '0' : aData[5];
                   var _suppid = Str(SUPPLIERID); var _buyerid = Str(sessionStorage.getItem('BUYERID'));
                   _lstIUOMdet.push('ITEM_UOM_MAP_ID' + "|" + Str(_itemuomid)); _lstIUOMdet.push('BUYER_ITEM_UOM' + "|" + Str(_biuom));
                   _lstIUOMdet.push('SUPPLIER_ITEM_UOM' + "|" + Str(_siuom)); _lstIUOMdet.push('BUYERID' + "|" + Str(_buyerid)); _lstIUOMdet.push('SUPPLIERID' + "|" + Str(_suppid));
                   var _res = ValidateItemUOM(_biuom, _siuom);
                   if (_res == true) { _isBSItemUomupdate = -2; SaveItemUOMDetails(_lstIUOMdet, GetBSItemUOMGrid, _suppid, _buyerid); } else { }
                   _newcont = 0; iuompreviousTr = '';
               }
               else if (e.target.className == 'glyphicon glyphicon-ban-circle') { _isBSItemUomupdate = -2; gridCancelEdit(nNew, nEditing, oBSItemUMTable); _newcont = 0; iuompreviousTr = ''; }
           });
           oBSRTable.on('click', 'tbody td', function (e) {
               var bsrselectedTr = $(this).parents('tr')[0]; var aData = oBSRTable.fnGetData(bsrselectedTr); var divid = 'dv' + bsrselectedTr._DT_RowIndex;
               if (e.target.className == 'glyphicon glyphicon-pencil') {
                   if (BSRpreviousTr != '' && BSRpreviousTr != bsrselectedTr) { restoreRow(BSRpreviousTr, oBSRTable); }
                   nNew = false; editRulesRow(oBSRTable, bsrselectedTr); BSRpreviousTr = bsrselectedTr;
               }
               else if (e.target.className == 'glyphicon glyphicon-floppy-save') {
                   var _hidvalue = '0'; var _ruleid = '0'; _lstRuledet = [];
                   var _linkid = Str(sessionStorage.getItem('LINKID')); var _ruleid = aData[8];
                   var _ruleval = $('#cboRuleVal option:selected').val(); var _inheritval = $('#cboInheritval option:selected').val();
                   _lstRuledet.push('LINKID' + "|" + Str(_linkid)); _lstRuledet.push('RULEID' + "|" + Str(_ruleid)); _lstRuledet.push('RULE_VALUE' + "|" + Str(_ruleval));
                   _lstRuledet.push('INHERIT_RULE' + "|" + Str(_inheritval));
                   var _res = ValidateRule(_ruleval); if (_res == true) { SaveBSLinkRule(_lstRuledet, GetBSuppRulesGrid, _linkid); } BSRpreviousTr = '';
               }
               else if (e.target.className == 'glyphicon glyphicon-ban-circle') { restoreRow(bsrselectedTr, oBSRTable); BSRpreviousTr = ''; }
           });
           function editRulesRow(oTable, nRow) {
               var _lstRuleval = []; _lstRuleval.push("0|0"); _lstRuleval.push("1|1"); var _lstInherit = []; _lstInherit.push("1|Yes"); _lstInherit.push("0|No");
               var aData = oTable.fnGetData(nRow); Editing = nRow; var jqTds = $('>td', nRow);
               var detTag = '<div style="text-align:center;"><span class="actionbtn" data-toggle="tooltip" title="Update"><i class="glyphicon glyphicon-floppy-save"></i></span>' +
             ' <span class="actionbtn" data-toggle="tooltip" title="Cancel"><i class="glyphicon glyphicon-ban-circle"></i></span></div>';
               jqTds[0].innerHTML = detTag;
               jqTds[6].innerHTML = '<select id="cboRuleVal" class="bs-select form-control">' + FillCombo(aData[6], _lstRuleval) + '</select>';
               jqTds[7].innerHTML = '<select id="cboInheritval" class="bs-select form-control">' + FillCombo(aData[7], _lstInherit) + '</select>';
           };
           function editRow(oTable, nRow) {
               var aData = oTable.fnGetData(nRow); nEditing = nRow; var jqTds = $('>td', nRow);
               var detTag = '<div style="text-align:center;"><span class="actionbtn" data-toggle="tooltip" title="Update"><i class="glyphicon glyphicon-floppy-save"></i></span>' +
             ' <span class="actionbtn" data-toggle="tooltip" title="Cancel"><i class="glyphicon glyphicon-ban-circle"></i></span></div>';
               jqTds[0].innerHTML = detTag;
               jqTds[1].innerHTML = '<input type="text" id="txtbUOM" class="form-control"  value="' + aData[1] + '"/>';
               jqTds[2].innerHTML = '<input type="text" id="txtSUOM" class="form-control"  value="' + aData[2] + '"/>';
           };
           function newRow() {
               var aiNew = oBSItemUMTable.fnAddData(['', '', '', '', '', '', '']); var nRow = oBSItemUMTable.fnGetNodes(aiNew[0]);
               editRow(oBSItemUMTable, nRow); nEditing = nRow; nNew = true;
           };
           function restoreRow(nRow, oTable) {
               var aData = oTable.fnGetData(nRow); var jqTds = $('>td', nRow);
               for (var i = 0, iLen = jqTds.length; i < iLen; i++) if (i == 0) { } else { oTable.fnUpdate(aData[i], nRow, i, false); } oTable.fnDraw();//oTable.fnUpdate(jqTds[i].innerHTML
           };
           function gridCancelEdit(nNew, nEditing, oTable) {
               var nRow = $('.DTTT_selected');
               if (nNew) { oTable.fnDeleteRow(nEditing); nNew = false; } else { nRow = nEditing; restoreRow(nRow, oTable); $(nRow).removeClass("DTTT_selected selected"); }
           };
           function fnBSFormatDetails(oTable, nTr) {
               var sOut = ''; var _str = '';
               var _irfqchecked = ''; var _erfqchecked = ''; var _erfqackchecked = ''; var _iqtechecked = ''; var _eqtechecked = ''; var _ipochecked = ''; var _epochecked = '';
               var _epoackchecked = ''; var _epocchecked = ''; var _nbchecked = ''; var _nschecked = ''; var _achecked = ''; var _activecss = ''; var _ipocchecked = '';
               var indx = nTr.rowIndex; var aData = oTable.fnGetData(nTr); var tid = "BSLinkTable" + indx; var _tbodyid = "tblBodyBSLnk" + indx;
               var _codeid = 'txtCode'; var _bLnkcdid = 'txtBLinkCode'; var _sLnkcdid = 'txtSLinkCode'; var _suppSenCdid = 'txtSuppSenCd';
               var _suppRecCdid = 'txtSuppRecCd'; var _bSenCdid = 'txtBSenCd'; var _brecvdid = 'txtBRecCd'; var _bmapid = 'txtBMapping'; var _smapid = 'txtSMapping';
               var _bcontid = 'txtBContact'; var _scontid = 'txtSContact'; var _bemailid = 'txtBEmail'; var _semailid = 'txtSEmail';
               var _ccemailid = 'txtCCEmail'; var _bccemailid = 'txtBCCEmail'; var _mailsbid = 'txtMailsub';
               var _replyid = 'txtReplyEmail'; var _bimpid = 'txtBImpPath'; var _bexpid = 'txtBExpPath'; var _simpid = 'txtSImpPath';
               var _sexpid = 'txtSExpPath'; var _wurlid = 'txtWebserURL'; var _dpriceid = 'txtDefPrice';
               var _ufiletyid = 'txtUpFileType'; var _irfqid = 'chkImpRFQ'; var _erfqid = 'chkExpRFQ'; var _erfqackid = 'chkExpRFQAck';
               var _iqteid = 'chkImpQuote'; var _eqteid = 'chkExpQuote'; var _ipoid = 'chkImpPO'; var _epoid = 'chkExpPO'; var _epoackid = 'chkExpPOAck';
               var _epocid = 'chkExpPOC'; var _nbyid = 'chkNBuyer'; var _nspid = 'chkNSupp'; var _actid = 'chkAct'; var _grpid = 'cbGrp';
               var _loginid = 'btnLgnCreate'; var _ipocid = 'chkImpPOC';
               var _code = Str(aData[3]); var _BLinkCode = Str(aData[5]); var _SLinkCode = Str(aData[6]); var _SuppSenCd = Str(aData[7]); var _SuppRecCd = Str(aData[8]);
               var _BSenCd = Str(aData[9]); var _BRecCd = Str(aData[10]); var _BMapping = Str(aData[11]); var _SMapping = Str(aData[12]); var _BContact = Str(aData[17]);
               var _SContact = Str(aData[18]); var _BEmail = Str(aData[19]); var _SEmail = Str(aData[20]); var _CCEmail = Str(aData[21]); var _BCCEmail = Str(aData[22]);
               var _Mailsub = Str(aData[23]); var _ReplyEmail = Str(aData[24]); var _BImpPath = Str(aData[25]); var _BExpPath = Str(aData[26]); var _SImpPath = Str(aData[27]);
               var _SExpPath = Str(aData[28]); var _WebserURL = Str(aData[29]); var _DefPrice = Str(aData[30]); var _UpFileType = Str(aData[32]);
               var _ImpRFQ = Str(aData[33]); if (_ImpRFQ == '1') { _irfqchecked = 'checked'; } else { _irfqchecked = ''; }
               var _ExpRFQ = Str(aData[34]); if (_ExpRFQ == '1') { _erfqchecked = 'checked'; } else { _erfqchecked = ''; }
               var _ExpRFQAck = Str(aData[35]); if (_ExpRFQAck == '1') { _erfqackchecked = 'checked'; } else { _erfqackchecked = ''; }
               var _ImpQuote = Str(aData[36]); if (_ImpQuote == '1') { _iqtechecked = 'checked'; } else { _iqtechecked = ''; }
               var _ExpQuote = Str(aData[37]); if (_ExpQuote == '1') { _eqtechecked = 'checked'; } else { _eqtechecked = ''; }
               var _ImpPO = Str(aData[38]); if (_ImpPO == '1') { _ipochecked = 'checked'; } else { _ipochecked = ''; }
               var _ExpPO = Str(aData[39]); if (_ExpPO == '1') { _epochecked = 'checked'; } else { _epochecked = ''; }
               var _ExpPOAck = Str(aData[40]); if (_ExpPOAck == '1') { _epoackchecked = 'checked'; } else { _epoackchecked = ''; }
               var _ExpPOC = Str(aData[41]); if (_ExpPOC == '1') { _epocchecked = 'checked'; } else { _epocchecked = ''; }
               var _NBuyer = Str(aData[42]); if (_NBuyer == '1') { _nbchecked = 'checked'; } else { _nbchecked = ''; }
               var _NSupp = Str(aData[43]); if (_NSupp == '1') { _nschecked = 'checked'; } else { _nschecked = ''; }
               var _ImpPOC = Str(aData[48]); if (_ImpPOC == '1') { _ipocchecked = 'checked'; } else { _ipocchecked = ''; }
               var _Act = Str(aData[44]); if (_Act == '1') { _achecked = 'checked'; _activecss = 'widelarge'; } else { _achecked = ''; _activecss = 'widelarge'; }
               var _lnkid = Str(aData[2]); var _pwd = Str(aData[46]); var _lgname = ''; if (_pwd == '') { _lgname = 'Create'; } else { _lgname = 'View'; }
               FillGroups(); var _grp = FillCombo(Str(aData[31]), _lstgrp);
               var btndiv = '<div class="row"><div style="text-align:center;"><span><a href="#" id="btnBSUpdate"><u>Update</u></<a></span>  <span><a href="#" id="btnBSCancel"><u>Cancel</u></<a></span></div></div>';
               var sOut = '<div class="col-md-12"><div class="row"><div class="col-md-12"><div class="form-group">' +
                     ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Code </label> </div>' +
                     ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _codeid + '"  value="' + _code + '"/> </div>' +
                      ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Group </label> </div>' +
                       ' <div  class="col-md-3"><select class="bs-select form-control" id="' + _grpid + '">' + _grp + '</select></div>' +
                     ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Buyer Link Code </label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bLnkcdid + '"  value="' + _BLinkCode + '"/> </div>' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Supplier Link Code </label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _sLnkcdid + '"  value="' + _SLinkCode + '"/> </div>' +
                       ' </div></div></div><div class="row"><div class="col-md-12"> <div class="form-group">' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Supplier Sender Code </label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _suppSenCdid + '"  value="' + _SuppSenCd + '"/> </div>' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Supplier Receiver Code </label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _suppRecCdid + '"  value="' + _SuppRecCd + '"/> </div>' +
                       ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Buyer Sender Code </label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bSenCdid + '"  value="' + _BSenCd + '"/> </div>' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Buyer Receiver Code </label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _brecvdid + '"  value="' + _BRecCd + '"/> </div>' +
                       ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Buyer Mapping </label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bmapid + '"  value="' + _BMapping + '"/> </div>' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Supplier Mapping </label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _smapid + '"  value="' + _SMapping + '"/> </div>' +
                       ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Buyer Contact </label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bcontid + '"  value="' + _BContact + '"/> </div>' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Supplier Contact </label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _scontid + '"  value="' + _SContact + '"/> </div>' +
                       ' </div></div></div> <div class="row"><div class="col-md-12"> <div class="form-group">' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Buyer Email </label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bemailid + '"  value="' + _BEmail + '"/> </div>' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Supplier Email </label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _semailid + '"  value="' + _SEmail + '"/> </div>' +
                       ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> CC Email </label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _ccemailid + '"  value="' + _CCEmail + '"/> </div>' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> BCC Email </label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bccemailid + '"  value="' + _BCCEmail + '"/> </div>' +
                       ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Mail Subject </label> </div>' +
                       ' <div  class="col-md-3"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _mailsbid + '">' + _Mailsub + '</textarea></div>' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Reply Email </label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _replyid + '"  value="' + _ReplyEmail + '"/> </div>' +
                       ' </div></div></div> <div class="row"><div class="col-md-12"> <div class="form-group">' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Import Path (From Buyer) </label> </div>' +
                       ' <div  class="col-md-3"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _bimpid + '">' + _BImpPath + '</textarea></div>' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Export Path (To Buyer) </label> </div>' +
                       ' <div  class="col-md-3"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _bexpid + '">' + _BExpPath + '</textarea></div>' +
                       ' </div></div></div> <div class="row"><div class="col-md-12"><div class="form-group">' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Import Path (From Supplier) </label> </div>' +
                       ' <div  class="col-md-3"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _simpid + '">' + _SImpPath + '</textarea></div>' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Export Path (To Supplier) </label> </div>' +
                       ' <div  class="col-md-3"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _sexpid + '">' + _SExpPath + '</textarea></div>' +
                       ' </div></div></div><div class="row"><div class="col-md-12"> <div class="form-group">' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Default Price </label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control alignRight" id="' + _dpriceid + '"  value="' + _DefPrice + '"/> </div>' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Upload File Type </label> </div>' +
                       ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _ufiletyid + '"  value="' + _UpFileType + '"/> </div>' +
                       ' </div></div></div><div class="row"><div class="col-md-12"> <div class="form-group">' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Webservice URL </label> </div>' +
                       ' <div  class="col-md-3"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _wurlid + '">' + _WebserURL + '</textarea></div>' +
                       ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Login Info </label> </div>' +
                       ' <div  class="col-md-3"><button type="button" id="' + _loginid + '" class="btn yellow-casablanca btn-outline actionbtn">' + _lgname + '</button></div>' +
                       ' </div></div></div>' +
                       '<div class="row"><div class="col-md-12">' +
                        '<div class="col-md-2"> </div>' +
                       ' <div class="col-md-2">' +
                       ' <div class="checkbox-grid">' +
                       ' <div><input class="widelarge" type="checkbox" id="' + _irfqid + '"  value="' + _ImpRFQ + '"' + _irfqchecked + ' /><label class="chklabel">Import RFQ from Buyer</label></div>' +
                       ' <div><input class="widelarge" type="checkbox" id="' + _erfqackid + '"  value="' + _ExpRFQAck + '"' + _erfqackchecked + '/><label class="chklabel">Export RFQ Ack. to Buyer</label></div>' +
                       ' <div><input class="widelarge" type="checkbox" id="' + _eqteid + '"  value="' + _ExpQuote + '"' + _eqtechecked + '/><label class="chklabel">Export Quote to Buyer</label></div>' +
                       ' <div><input class="widelarge" type="checkbox" id="' + _ipoid + '"  value="' + _ImpPO + '"' + _ipochecked + '/><label class="chklabel">Import PO from Buyer</label></div>' +
                       '</div></div>' +
                       ' <div class="col-md-2">' +
                          ' <div class="checkbox-grid">' +
                       ' <div> <input class="widelarge" type="checkbox" id="' + _epoackid + '"  value="' + _ExpPOAck + '"' + _epoackchecked + '/> <label class="chklabel">Export PO Ack. to Buyer</label></div>' +
                       ' <div> <input class="widelarge" type="checkbox" id="' + _epocid + '"  value="' + _ExpPOC + '"' + _epocchecked + '/><label class="chklabel">Export POC to Buyer</label></div>' +
                       ' <div> <input class="widelarge" type="checkbox" id="' + _nbyid + '"  value="' + _NBuyer + '"' + _nbchecked + '/> <label class="chklabel">Notify Buyer</label></div>' +
                           '</div>' +
                        '</div>' +
                        ' <div class="col-md-3">' +
                       ' <div class="checkbox-grid">' +
                           ' <div><input class="widelarge" type="checkbox" id="' + _erfqid + '"  value="' + _ExpRFQ + '"' + _erfqchecked + ' /><label class="chklabel">Export RFQ to Supplier</label></div>' +
                           ' <div><input class="widelarge" type="checkbox" id="' + _iqteid + '"  value="' + _ImpQuote + '"' + _iqtechecked + '/><label class="chklabel">Import Quote from Supplier</label></div>' +
                           ' <div><input class="widelarge" type="checkbox" id="' + _ipocid + '"  value="' + _ImpPOC + '"' + _ipocchecked + '/><label class="chklabel">Import POC from Supplier</label></div>' +
                           ' <div> <input class="widelarge" type="checkbox" id="' + _epoid + '"  value="' + _ExpPO + '"' + _epochecked + '/><label class="chklabel">Export PO to Supplier</label></div>' +
                        '</div></div>' +
                      '<div class="col-md-3>' +
                      ' <div class="checkbox-grid">' +
                       ' <div> <input class="widelarge" type="checkbox" id="' + _nspid + '"  value="' + _NSupp + '"' + _nschecked + '/><label class="chklabel">Notify Supplier</label></div>' +
                       ' <div> <input class="' + _activecss + '" type="checkbox" id="' + _actid + '"  value="' + _Act + '"' + _achecked + '/><label class="chklabel">Active</label></div>' +
                     ' </div></div>' +
                      ' </div></div>';
               return sOut;
           };
           var LoginDetails = function (_btntxt) {
               var sOut = ''; var _str = ''; var _reftype = ''; var _buyerref = ''; var _suppref = ''; var _descr = ''; var _rmk = ''; var btndiv = '';
               var _supp = ''; var _user = ''; var _pwd = ''; var _email = ''; var _user_id = ''; var _act = '';
               var tid = "LoginTable"; var _tbodyid = "tbllogin"; var _actid = 'chkAct'; _lstlogindet = []; var _actcheckd = '';
               var _linkid = Str(sessionStorage.getItem('LINKID'));
               var _suppid = 'txtSupp'; var _userid = 'txtUser'; var _pwdid = 'txtPwd'; var _emailid = ''; var _bpwdid = 'btnPWD';
               var _lgDet = SetLoginInfo(false, _linkid); _lstlogindet = _lgDet.split('|'); var isdisabled = (_btntxt == 'Create') ? 'disabled' : '';
               _supp = Str(_lstlogindet[1]); _user = Str(_lstlogindet[2]); _pwd = Str(_lstlogindet[3]); _email = Str(_lstlogindet[4]);
               _act = Str(_lstlogindet[5]); if (_act == '1') { _achecked = 'checked'; } else { _achecked = ''; } _user_id = Str(_lstlogindet[0]);
               var sOut = ' <div class="row"><div class="col-md-12">' +
                         ' <div class="col-md-4" style="text-align:right;"><label>Supplier Name </label></div>' +
                         ' <div class="col-md-8"><span id="' + _suppid + '">' + _supp + ' </span><span style="display:none;" id="spuserid">' + _user_id + ' </span> </div>' +
                         ' </div></div> <div class="row"><div class="col-md-12">' +
                         ' <div class="col-md-4" style="text-align:right;"><label>User Code </label></div>' +
                         ' <div class="col-md-8"><span id="' + _userid + '">' + _user + ' </span></div>' +
                         ' </div></div> <div class="row"><div class="col-md-12">' +
                         ' <div class="col-md-4" style="text-align:right;"><label>Password </label></div>' +
                         ' <div class="col-md-4"><span id="' + _pwdid + '">' + _pwd + ' </span></div>' +
                         ' <div class="col-md-4"><button type="button" ' + isdisabled + ' class="btn yellow-casablanca btn-outline actionbtn" id="' + _bpwdid + '">Change</button> </div>' +
                         ' </div></div> <div class="row"><div class="col-md-12">' +
                         ' <div class="col-md-4" style="text-align:right;"><label>Email </label></div>' +
                         ' <div class="col-md-8"><span id="' + _emailid + '">' + _email + ' </span></div>' +
                         ' </div></div><div class="row"><div class="col-md-12">' +
                         ' <div class="col-md-4" style="text-align:right;"><label>Active </label></div>' +
                         ' <div class="col-md-8"><input type="checkbox" id="' + _actid + '" value="' + _act + '"' + _achecked + '> </div>' +
                         ' </div></div>';
               return sOut;
           }
           var ChangePassword = function () {
               var sOut = ''; var _str = ''; var _newpwd = ''; var _bcnfmpwdef = '';; var _nPwd = ''; var _nCPwd = '';
               var tid = "ChangePWDTable"; var _tbodyid = "tblCPWD"; var _Pwdid = 'txtChangePwd'; var _CPwdid = 'txtConfirmPWD';
               var sOut = '<div class="row"><div class="col-md-12"><div class="form-group">' +
                        '  <div class="col-md-4" style="text-align:right;margin-top:5px;"><label>New Password </label></div>' +
                        '  <div class="col-md-8"> <input type="password" class="form-control"  id="' + _Pwdid + '"  value="' + _nPwd + '"/></div>' +
                        '  </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
                        '  <div class="col-md-4" style="text-align:right;margin-top:5px;"><label>Confirm Password </label></div>' +
                        '  <div class="col-md-8"><input type="password" class="form-control"  id="' + _CPwdid + '"  value="' + _nCPwd + '"/> </div>' +
                        '  </div></div></div>';
               return sOut;
           };
           function closeLogin() { $('#dvLoginft').css('display', 'none'); $('#dvLoginInfo').css('display', 'block'); $('#dvChangePWD').css('display', 'none'); };
     
           $("#txtSuppname").autocomplete({
               source: function (request, response) {
                   $.ajax({
                       type: "POST",
                       async: false,
                       url: "SupplierDetail.aspx/GetAddressName_Search",
                       data: "{'SEARCHTEXT':'" + $("#txtSuppname").val() + "','ADDRTYPE':'SUPPLIER'}",
                       contentType: "application/json; charset=utf-8",
                       dataType: "json",
                       success: function (data) {
                           if (data.d.length > 0) {
                               response($.map(data.d, function (item) {
                                   return {
                                       label: item.split('-')[0],
                                       val: item.split('-')[1]
                                   };
                               }))
                           } else { response([{ label: 'No results found.', val: -1 }]); }
                       },
                       error: function (result) { alert("Error......"); }
                   });
               },
               select: function (e, u) { if (u.item.val == -1) { return false; } }
           });

           $('input[type=checkbox]').click(function (e) {
               var id = this.id; var _isChkd = this.checked; var _selecdserv = '';
               var _Servname = $("label[for='" + this.id + "']").text();
               if (_lstSyncservers != undefined) {
                   if (jQuery.inArray(_Servname, _lstSyncservers) == -1)
                   {
                       if (_isChkd == true) {
                           _lstSyncservers.push(_Servname); 
                       }                      
                   }
                   else
                   {
                       if (_isChkd == false) {
                           _lstSyncservers = jQuery.grep(_lstSyncservers, function (value) {
                               return value != _Servname;
                           });
                       }
                   }
               }
               for (var k = 0; k < _lstSyncservers.length;k++)
               {
                   _selecdserv = Str(_lstSyncservers[k]);
               }
               $('#btnServ').text(_lstSyncservers);
           });
       };

   $('#cbDefFormat').live("change", function (e) {
       _selectdefval = $('#cbDefFormat option:selected').val();
       var _isexist = CheckExisting(_selectdefval, SUPPLIERID);
       if (_isexist == '') {
           var _suppCode = $('#txtCode').val();
           var _exppath = _configpath + _suppCode + "\\" + _selectdefval + "\\OUTBOX"; var _imppath = _configpath + _suppCode + "\\" + _selectdefval + "\\INBOX";
           $('#txtCSImpPath').val(_imppath); $('#txtCSExpPath').val(_exppath);
       }
       else { GetSupplierConfigSettings(_selectdefval, SUPPLIERID); }
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
       e.preventDefault(); var _defaddrid = SUPPLIERID; var _defgrpformat = GetSupplierFormat(SUPPLIERID);
       _scheckid = [];
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
               SaveDefRule(_lstMasterDet, GetSupplierRules, _defaddrid, _defgrpformat);
           }
           toastr.success("Lighthouse eSolutions Pte. Ltd", "Rule Saved successfully."); $("#ModalDefRule").modal('hide');
       }
       else { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Rule Value is blank'); }
   });

   $('input[type="radio"]').on('click', function () {
       $('#tblXls').show(); $('#tblPdf').hide(); $('#tblXls').hide();  if ($(this).attr('id') == 'rdExcel') { $('#tblXls').show(); GetSupplierExcelMapp(SUPPLIERID); }
       else if ($(this).attr('id') == 'rdPDF') { $('#tblXls').hide(); $('#tblPdf').show(); GetSupplierPDFMapp(SUPPLIERID); }
   });

   function setNewRules_Grid() {
       setupNewRulesTableHeader();
       var table2 = $('#dataGridNewRule');
        oNRTable = table2.dataTable({
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
           { 'targets': [0], width: '5px' }, { 'targets': [1, 2], width: '40px' }, { 'targets': [3], width: '110px' },
           { 'targets': [4], width: '140px' },
           { 'targets': [5], visible: false },
           ],
           "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
           "scrollX": '880px',
           "scrollY": '300px',
           "paging": false,
           "aaSorting": [],
           "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
           "fnRowCallback": function (nRow, aData, iDisplayIndex) {
               var _chkid = "chk" + nRow._DT_RowIndex; var _chkdiv = '<div style="text-align:center;"><input type="checkbox"  id="' + _chkid + '"  /></div>';
               $('td:eq(0)', nRow).html(_chkdiv);
           }
       });
       $('#tblHeadRowNewRule').addClass('gridHeader'); $('#ToolTables_dataGridNewRule_0,#ToolTables_dataGridNewRule_1').hide(); $('#dataGridNewRule_paginate').hide();
       $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");

   };

   function setBuyerGrid() {
       setupBuyerTableHeader();
      var  oTable = $('#dataGridNewBuy');
      oBuyerTable = oTable.dataTable({
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
           "columnDefs": [{ 'orderable': false, "searching": true, "autoWidth": false, 'targets': [0] },
            { 'targets': [0], width: '30px' }, { 'targets': [1], width: '100px' }, { 'targets': [2], visible: false }
           ],
           "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
           "aaSorting": [],
           "sScrollY": '300px'
       });
       $('#tblHeadRowNewBuy').addClass('gridHeader'); $('#ToolTables_dataGridNewBuy_0,#ToolTables_dataGridNewBuy_1').hide();
       $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
       GetBuyerGrid();
   };

   /*Config Settings*/
   var GetSupplierConfigDetail = function (FORMAT, SUPPLIERID) {
       $.ajax({
           type: "POST",
           async: false,
           url: "SupplierDetail.aspx/GetSupplierConfigDetails",
           data: "{'FORMAT':'" + FORMAT + "','SUPPLIERID':'" + SUPPLIERID + "'}",
           contentType: "application/json; charset=utf-8",
           dataType: "json",
           success: function (response) {
               try {
                   _configpath = response.d.split('||')[1];  _spCnfgdet = (response.d.split('||')[0] != '{}') ? JSON.parse(response.d.split('||')[0]) : [];
                   _cnfgMailsubj = response.d.split('||')[2];
               }
               catch (err) {  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer Config :' + err); }
           },
           failure: function (response) {  toastr.error("failure get", response.d); },
           error: function (response) { toastr.error("error get", response.responseText); }
       });
       return _spCnfgdet;
   };

   var CheckExisting = function (FORMAT, SUPPLIERID) {
       var _Isexist = '';
       $.ajax({
           type: "POST",
           async: false,
           url: "SupplierDetail.aspx/CheckExistingConfig",
           data: "{'FORMAT':'" + FORMAT + "','SUPPLIERID':'" + SUPPLIERID + "'}",
           contentType: "application/json; charset=utf-8",
           dataType: "json",
           success: function (response) {
               try { _Isexist = Str(response.d); }
               catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Buyer Config check :' + err); }
           },
           failure: function (response) { toastr.error("failure get", response.d);},
           error: function (response) {  toastr.error("error get", response.responseText); }
       });
       return _Isexist;
   };

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
                   if (Dataset.NewDataSet != null) {
                       var Table = Dataset.NewDataSet.Table;_lstgrpfrmt = [];
                       if (Table != undefined && Table != null) {
                           if (Table.length != undefined) {
                               for (var i = 0; i < Table.length; i++) {  _lstgrpfrmt.push(Str(Table[i].FORMAT) + "|" + Str(Table[i].FORMAT)); }
                           }
                           else {
                               if (Table.FORMAT != undefined) {
                                   _lstgrpfrmt.push(Str(Table.FORMAT) + "|" + Str(Table.FORMAT)); }
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

   function GetSupplierConfigSettings(_selectdefval, SUPPLIERID) {
       var headerdet = fnSConfgSettings(_selectdefval, SUPPLIERID); $('#divSuppConfg').empty().append(headerdet); $("#cbDefFormat").select2();
   };

   function fnSConfgSettings(_selectdefval, SUPPLIERID) {
       var sOut = ''; var _str = '';
       var _irfqchecked = ''; var _erfqchecked = ''; var _erfqackchecked = ''; var _iqtechecked = ''; var _eqtechecked = ''; var _ipochecked = ''; var _epochecked = '';
       var _epoackchecked = ''; var _epocchecked = ''; var _nschecked = ''; var _DefFormat = ''; var _Mailsub = ''; var _ipocchecked = '';
       var slSpCfgdet = GetSupplierConfigDetail(_selectdefval, SUPPLIERID);
       var tid = "SConfgTable"; var _tbodyid = "tblBodyBConfg"; var _smapid = 'txtCSMapping';
       var _ccemailid = 'txt_CCEmail'; var _mailsbid = 'txtCMailsub'; var _simpid = 'txtCSImpPath'; var _sexpid = 'txtCSExpPath';
       var _wurlid = 'txtCWebserURL'; var _dpriceid = 'txtCDefPrice'; var _supprecvCd = 'txtSupplierReceiverCode';
       var _ufiletyid = 'txtCUpFileType'; var _erfqid = 'chkCExpRFQ'; var _iqteid = 'chkCImpQuote'; var _epoid = 'chkCExpPO'; var _nSpid = 'chkCNSupplier';
       var _defformatid = 'cbDefFormat'; var _ipocid = 'chkCImpPOC';
       _addressConfigID = (slSpCfgdet != '') ? Str(slSpCfgdet.ADDRCONFIGID) : '0';
       var _SMapping = (slSpCfgdet != '') ? Str(slSpCfgdet.PARTY_MAPPING) : '';
       var _CCEmail = (slSpCfgdet != '') ? Str(slSpCfgdet.CC_EMAIL) : '';
       if (slSpCfgdet != '') { _Mailsub = (Str(slSpCfgdet.MAIL_SUBJECT) != '') ? Str(slSpCfgdet.MAIL_SUBJECT) : _cnfgMailsubj; } else { _Mailsub = _cnfgMailsubj; }
       var _SImpPath = (slSpCfgdet != '') ? Str(slSpCfgdet.IMPORT_PATH) : '';
       var _SExpPath = (slSpCfgdet != '') ? Str(slSpCfgdet.EXPORT_PATH) : '';
       var _WebserURL = (slSpCfgdet != '') ? Str(slSpCfgdet.SUP_WEB_SERVICE_URL) : '';
       var _DefPrice = (slSpCfgdet != '') ? Str(slSpCfgdet.DEFAULT_PRICE) : '0';
       var _UpFileType = (slSpCfgdet != '') ? Str(slSpCfgdet.UPLOAD_FILE_TYPE) : '';
       var _ExpRFQ = (slSpCfgdet != '') ? (slSpCfgdet.EXPORT_RFQ) : ''; if (_ExpRFQ == '1') { _erfqchecked = 'checked'; } else { _erfqchecked = ''; }
       var _ImpQuote = (slSpCfgdet != '') ? Str(slSpCfgdet.IMPORT_QUOTE) : ''; if (_ImpQuote == '1') { _iqtechecked = 'checked'; } else { _iqtechecked = ''; }
       var _ExpPO = (slSpCfgdet != '') ? Str(slSpCfgdet.EXPORT_PO) : ''; if (_ExpPO == '1') { _epochecked = 'checked'; } else { _epochecked = ''; }
       var _NSupplier = (slSpCfgdet != '') ? Str(slSpCfgdet.MAIL_NOTIFY) : ''; if (_NSupplier == '1') { _nschecked = 'checked'; } else { _nschecked = ''; }
       var _ImpPOC = (slSpCfgdet != '') ? Str(slSpCfgdet.IMPORT_POC) : '';if (_ImpPOC == '1') { _ipocchecked = 'checked'; } else { _ipocchecked = ''; }
       FillGroupFormat(); var _deffrmt = (slSpCfgdet != '') ? Str(slSpCfgdet.DEFAULT_FORMAT) : ''; _DefFormat = FillCombo(_deffrmt, _lstgrpfrmt);
       var _frmSpp = 'Import Path \n (From Supplier)'; var _toSpp = 'Export Path \n (To Supplier)'; _frmSpp = _frmSpp.replace(/\n/g, '<br/>'); _toSpp = _toSpp.replace(/\n/g, '<br/>');
       var sOut = '<div class="row">' +
               ' <div class="col-md-10"><div class="row"><div class="col-md-12"><div class="form-group">' +
               ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Document Format </label> </div>' +
               ' <div  class="col-md-4"><select class="bs-select form-control" id="' + _defformatid + '">' + _DefFormat + '</select></div>' +
               ' </div></div></div><div class="row" style="margin-top:3px;"><div class="col-md-12"><div class="form-group">' +
               ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> ' + _frmSpp + '  </label> </div>' +
               ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _simpid + '" value="' + _SImpPath + '"/></div>' +
               ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> ' + _toSpp + ' </label> </div>' +
               ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _sexpid + '" value="' + _SExpPath + '"/></div>' +              
               ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
               ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Supplier Mapping </label> </div>' +
               ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _smapid + '"  value="' + _SMapping + '"/> </div>' +
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
               ' </div></div></div><div class="row"><div class="col-md-2"></div><div class="col-md-9"><div class="form-group">' +
               ' <ul class="checkbox-grid">' +
               ' <li><input class="widelarge" type="checkbox" id="' + _erfqid + '"  value="' + _ExpRFQ + '"' + _erfqchecked + ' /><label class="chklabel">Export RFQ to Supplier</label></li>' +
               ' <li><input class="widelarge" type="checkbox" id="' + _iqteid + '"  value="' + _ImpQuote + '"' + _iqtechecked + '/><label class="chklabel">Import Quote from Supplier</label></li>' +
               ' <li> <input class="widelarge" type="checkbox" id="' + _epoid + '"  value="' + _ExpPO + '"' + _epochecked + '/><label class="chklabel">Export PO to Supplier</label></li>' +
               ' <li><input class="widelarge" type="checkbox" id="' + _ipocid + '"  value="' + _ImpPOC + '"' + _ipocchecked + '/><label class="chklabel">Import POC from Supplier</label></li>' +
               ' <li> <input class="widelarge" type="checkbox" id="' + _nSpid + '"  value="' + _NSupplier + '"' + _nschecked + '/> <label class="chklabel">Notify Supplier</label></li>' +
               ' </div></div><div class="col-md-2"></div></div></div>';
       return sOut;
   };

   function SaveSConfigDetails(SUPPLIERID) {
       var _lstdet = [];
       var _defFormat = $('#cbDefFormat').val(); var _smap = $('#txtCSMapping').val(); var _ccemail = $('#txt_CCEmail').val();
       var _mailsb = $('#txtCMailsub').val(); var _mailsb1 = Str(_mailsb).replace(/\#/g, '_');
       var _simp = $('#txtCSImpPath').val(); var _sexp = $('#txtCSExpPath').val();
       var _wurl = $('#txtCWebserURL').val(); var _dprice = $('#txtCDefPrice').val(); var _ufilety = $('#txtCUpFileType').val();
       var _erfq = ($('#chkCExpRFQ').is(':checked')) ? 1 : 0;
       var _iqte = ($('#chkCImpQuote').is(':checked')) ? 1 : 0;
       var _epo = ($('#chkCExpPO').is(':checked')) ? 1 : 0;
       var _nsp = ($('#chkCNSupplier').is(':checked')) ? 1 : 0;
       var _ipoc = ($('#chkCImpPOC').is(':checked')) ? 1 : 0;
       var _id = _addressConfigID;
       _lstdet.push("ADDRESSCONFIGID" + "|" + Str(_id));
       _lstdet.push("DEFAULT_FORMAT" + "|" + Str(_defFormat)); _lstdet.push("MAPPING" + "|" + Str(_smap)); _lstdet.push("ADDRESSID" + "|" + Str(SUPPLIERID));
       _lstdet.push("IMPORT_PATH" + "|" + Str(_simp)); _lstdet.push("EXPORT_PATH" + "|" + Str(_sexp));
       _lstdet.push("CC_EMAIL" + "|" + Str(_ccemail)); _lstdet.push("MAIL_SUBJECT" + "|" + Str(_mailsb1));
       _lstdet.push("EXPORT_RFQ" + "|" + Str(_erfq)); _lstdet.push("IMPORT_QUOTE" + "|" + Str(_iqte)); _lstdet.push("EXPORT_PO" + "|" + Str(_epo)); _lstdet.push("NOTIFY_SUPPLR" + "|" + Str(_nsp));
       _lstdet.push("DEFAULT_PRICE" + "|" + Str(_dprice)); _lstdet.push("UPLOAD_FILE_TYPE" + "|" + Str(_ufilety)); _lstdet.push("SUP_WEB_SERVICE_URL" + "|" + Str(_wurl)); _lstdet.push("IMPORT_POC" + "|" + Str(_ipoc));
       if (ValidateSupplierConfig()) { UpdateSupplierConfigDetails(_lstdet, SUPPLIERID, _defFormat) }
   };

   function ClearControls() {
       var _emptystr = ''; _addressConfigID = '0';
       $('#txtSMapping').val(_emptystr); $('#txtCCEmail').val(_emptystr); $('#txtMailsub').val(_emptystr); $('#txtSImpPath').val(_emptystr); $('#txtSExpPath').val(_emptystr);
       $('#txtWebserURL').val(_emptystr); $('#txtDefPrice').val(_emptystr); $('#txtUpFileType').val(_emptystr); $('#chkImpRFQ').removeAttr('checked');
       $('#chkExpRFQ').removeAttr('checked'); $('#chkExpRFQAck').removeAttr('checked'); $('#chkImpQuote').removeAttr('checked'); $('#chkExpQuote').removeAttr('checked');
       $('#chkImpPO').removeAttr('checked'); $('#chkExpPO').removeAttr('checked'); $('#chkExpPOAck').removeAttr('checked'); $('#chkExpPOC').removeAttr('checked');
       $('#chkNSupp').removeAttr('checked');
   };

   function ValidateSupplierConfig() {
       var _valid = true; var _defFormat = $('#cbDefFormat').val();
       if (_defFormat == '') { $('#cbDefFormat').addClass('error'); _valid = false; } else { $('#cbDefFormat').removeClass('error'); }
       return _valid;
   };

   function UpdateSupplierConfigDetails(_nfieldval, SUPPLIERID, _defFormat) {
       var sldet = [];  
       for (var j = 0; j < _nfieldval.length; j++) { sldet.push(_nfieldval[j]); }
       var data2send = JSON.stringify({ sldet: sldet });
       Metronic.blockUI('#portlet_body');
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/SaveSupplierConfigDetails",
               data: data2send,
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var res = Str(response.d);
                       if (res != "") {
                         //  toastr.success("Lighthouse eSolutions Pte. Ltd", "Supplier details Saved successfully.");
                          // GetSupplierConfigDetail(_defFormat, SUPPLIERID);
                       }
                       Metronic.unblockUI();
                   }
                   catch (err) {
                     //  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Supplier details :' + err); Metronic.unblockUI();
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

   var GetSupplierFormat = function (SUPPLIERID) {
       var _format = '';
       $.ajax({
           type: "POST",
           async: false,
           url: "SupplierDetail.aspx/GetFormat_Supplier",
           data: "{'SUPPLIERID':'" + SUPPLIERID + "'}",
           contentType: "application/json; charset=utf-8",
           dataType: "json",
           success: function (response) {
               try {
                   _format = Str(response.d);
               }
               catch (err) {
                   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Supplier Format :' + err);
               }
           },
           failure: function (response) {
               toastr.error("failure get", response.d);
           },
           error: function (response) {
               toastr.error("error get", response.responseText);
           }
       });
       return _format;
   };
   /*end*/

   /* Common */
   function DownloadTemplate(LINKID, TYPE) {
       try {
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/DownloadTemplate",
               data: "{ 'LINKID':'" + LINKID + "','TYPE':'" + TYPE + "' }",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var res = Str(response.d);
                       if (res != "") {
                           var filefullpath = res.split('|')[0]; var filename = res.split('|')[1];  //var cVirtualPath = "../Downloads/";var win = window.open(cVirtualPath + filename, '_blank');
                           top.location.href = "../Downloads/" + filename;
                           toastr.success("Lighthouse eSolutions Pte. Ltd", "Download File success");
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

   function setaccess(tabindx) {
       $('#btnItmNew').hide(); $('#btnItmUpload').hide(); $('#btnDeletr').show(); $('#btnSync').hide();
       if (tabindx == '0') { $('#btnNew').hide(); $('#btnCancel').show(); $('#btnSave').show(); $('#btnSync').hide(); }
       else if (tabindx == '1') { $('#btnNew').show(); $('#btnCancel').hide(); $('#btnDeletr').hide(); $('#btnSave').hide(); }      
       else if (tabindx == '2') { $('#btnNew').show(); $('#btnCancel').hide(); $('#btnDeletr').hide(); $('#btnSave').hide(); }
       else if (tabindx == '3' || tabindx == '4') { $('#btnNew').hide(); $('#btnCancel').hide(); $('#btnDeletr').hide(); $('#btnSave').hide(); }
       else if (tabindx == '101') { $('#btnItmNew').show(); $('#btnDeletr').hide(); }
       else if (tabindx == '102' || tabindx == '103') { $('#btnItmNew').show(); $('#btnItmUpload').show(); $('#btnDeletr').hide(); }
       setAccessPortlet(tabindx);
   };
   function setAccessPortlet(tabindx) {
       $('#prtDetail').hide(); $('#prtSuppConfig').hide(); $('#prtByLnk').hide(); $('#prtSuppRules').hide(); $('#prtSMapping').hide(); $('#prtSTransact').hide();
       if (tabindx == '0') { $('#prtDetail').show(); $('#prtSuppConfig').show(); }
       else if (tabindx == '1') { $('#prtSuppRules').show(); } else if (tabindx == '2') { $('#prtByLnk').show(); }
       else if (tabindx == '3') { $('#prtSMapping').show(); } else if (tabindx == '4') { $('#prtSTransact').show(); }
   };
   var setToolbar = function () {
       var _btns = ' <span title="Sync" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;" class="toolbtn" id="btnSync"><i class="fa fa-refresh" style="text-align:center;"></i></a></div>' +
           '<span title="Cancel" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;"  id="btnCancel" class="toolbtn"><i class="fa fa-ban" style="text-align:center;"></i></a></div></span>' +
           '<span title="Save" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;"  id="btnSave" class="toolbtn"><i class="fa fa-floppy-o" style="text-align:center;"></i></a></div>' +
           //'<span title="Delete" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;"  id="btnDeletr" class="toolbtn"><i class="fa fa-trash" style="text-align:center;"></i></a></div></span>' +
           '<span title="New" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" ><a href="javascript:;" id="btnNew" class="toolbtn"><i class="fa fa-plus" style="text-align:center;"></i></a></div></span>' +
           '<span title="Refresh" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10"><a href="javascript:;"  id="btnRefresh" class="toolbtn"><i class="fa fa-recycle" style="text-align:center;"></i></a></div></span>';
       $('#toolbtngroupdet').append(_btns);
       var _itmbtns = '<span title="Close" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnClose"><a href="javascript:;" id="btnItmClose" class="toolbtn"><i class="fa fa-times" style="text-align:center;"></i></a></div></span>' +
           '<span title="Upload" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnUpload"><a href="javascript:;" id="btnItmUpload" class="toolbtn"><i class="fa fa-upload" style="text-align:center;"></i></a></div></span>' +
         '<span title="New" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnNew"><a href="javascript:;" id="btnItmNew" class="toolbtn"><i class="fa fa-plus" style="text-align:center;"></i></a></div></span>' +
         '<span title="Refresh" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="divbtnRefresh"><a href="javascript:;" class="toolbtn" id="btnItmRefresh"><i class="fa fa-recycle" style="text-align:center;"></i></a></div></span>';
       $('#Itemtoolbtngroup').append(_itmbtns);
   };
   /* end */

   function GetSupplier_DefSettings(SUPPLIERID, _selectdefval) {
       GetSupplierDetails(SUPPLIERID); GetSupplierConfigSettings(_selectdefval, SUPPLIERID); 
   };

   /*update Supplier*/
   function GetSupplierDetails(SUPPLIERID) {
       var headerdet = fnFormatDetails(SUPPLIERID); $('#divDetail').empty().append(headerdet);      
   };

   function fnFormatDetails(SUPPLIERID) {
       var sOut = ''; var _str = ''; var _disableinput = ''; var _hiddeninput = '';
       var tid = "SuppTable"; var _tbodyid = "tblBodySupp"; var _codeid = 'txtCode'; var _snameid = 'txtSuppname'; var _contactPersid = 'txtContactPerson';
       var _emailid = 'txtEmail'; var _countryid = 'txtCountry'; var _imppathid = 'txtDownloadPath'; var _exppathid = 'txtUploadPath'; var _cityid = 'txtCity'; var _postalid = 'txtPostalCode';
       var slspdet = GetSupplierHeader(SUPPLIERID);
       var _code = Str(slspdet.ADDR_CODE); var _suppname = Str(slspdet.ADDR_NAME); var _contactpers = Str(slspdet.CONTACT_PERSON);
       var _email = Str(slspdet.ADDR_EMAIL); var _country = Str(slspdet.ADDR_COUNTRY); var _city = Str(slspdet.ADDR_CITY); var _postalcode = Str(slspdet.ADDR_ZIPCODE);
       var _imppath = Str(slspdet.ADDR_INBOX); var _exppath = Str(slspdet.ADDR_OUTBOX); var _addrid = Str(SUPPLIERID);
       if (_isparent != '' && _isparent != undefined) { if (_isparent.toUpperCase() == 'TRUE') { _disableinput = ''; _hiddeninput = '0'; } else { _disableinput = 'disabled'; _hiddeninput = '0'; } }
       var  _lstServernames = []; _lstServernames = GetServerNames();
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
               ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _codeid + '"  value="' + _code + '"/> </div>' +
               ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Supplier Name </label> </div>' +
               ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _snameid + '"  value="' + _suppname + '"/> </div>' +
               ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
               ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Contact Person </label> </div>' +
               ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _contactPersid + '"  value="' + _contactpers + '"/> </div>' +
               ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Email </label> </div>' +
               ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _emailid + '"  value="' + _email + '"/> </div>' +             
               //' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
               //' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> City </label> </div>' +
               //' <div  class="col-md-4"><input type="text" class="form-control" id="' + _cityid + '"  value="' + _city + '"/> </div>' +
               //' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Postal Code </label> </div>' +
               //' <div  class="col-md-4"><input type="text" class="form-control" id="' + _postalid + '"  value="' + _postalcode + '"/> </div>' +
             ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
             ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Upload Path </label> </div>' +
             ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _exppathid + '">' + _exppath + '</textarea> </div>' +
             ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Download Path </label> </div>' +
             ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _imppathid + '">' + _imppath + '</textarea></div>' +
             ' </div></div></div>';             
       if (_hiddeninput == '1') {
           sOut += '<div class="row"><div class="col-md-12"><div class="form-group">' +
           ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Select Servers </label> </div>' +
           ' <div  class="col-md-4">' + _servselect + ' </div>' +
           ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Country </label> </div>' +
           ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _countryid + '"  value="' + _country + '"/> </div>' +
           ' </div></div></div>';
       }
       else {
           sOut += '<div class="row"><div class="col-md-12"><div class="form-group">' +
        ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Country </label> </div>' +
        ' <div  class="col-md-4"><input type="text" class="form-control" id="' + _countryid + '"  value="' + _country + '"/> </div>' +
        ' </div></div></div>';
       }
       sOut += ' </div> </div>';
       
       return sOut;
   };
     
   var GetSupplierHeader= function (SUPPLIERID) {
       $.ajax({
           type: "POST",
           async: false,
           url: "SupplierDetail.aspx/GeSupplierDetails",
           data: "{'SUPPLIERID':'" + SUPPLIERID + "'}",
           contentType: "application/json; charset=utf-8",
           dataType: "json",
           success: function (response) {
               try {
                   if (response.d != '' && response.d != undefined) { Suppdet = JSON.parse(response.d); }
               }
               catch (err) {
                   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Supplier Details :' + err);
               }
           },
           failure: function (response) {
               toastr.error("failure get", response.d);
           },
           error: function (response) {
               toastr.error("error get", response.responseText);
           }
       });
       return Suppdet;
   };

   function GetSupplierDet_Update(SUPPLIERID) {
       _lstdet = [];
       var tid = "SuppTable"; var _code = $('#txtCode').val(); var _suppname = $('#txtSuppname').val(); var _contactPerson = $('#txtContactPerson').val(); var _email = $('#txtEmail').val();
       var _country = $('#txtCountry').val(); var _imppath = $('#txtDownloadPath').val(); var _exppath = $('#txtUploadPath').val(); var _city = $('#txtCity').val(); var _postal = $('#txtPostalCode').val();
       _lstdet.push("ID" + "|" + Str(SUPPLIERID)); _lstdet.push("ADDR_CODE" + "|" + Str(_code));
       _lstdet.push("ADDR_NAME" + "|" + Str(_suppname)); _lstdet.push("CONTACT_PERSON" + "|" + Str(_contactPerson));
       _lstdet.push("ADDR_EMAIL" + "|" + Str(_email)); _lstdet.push("ADDR_COUNTRY" + "|" + Str(_country));
       _lstdet.push("ADDR_INBOX" + "|" + Str(_imppath)); _lstdet.push("ADDR_OUTBOX" + "|" + Str(_exppath));
       _lstdet.push("ADDR_CITY" + "|" + Str(_city)); _lstdet.push("ADDR_ZIPCODE" + "|" + Str(_postal));
       var _defFormat = $('#cbDefFormat').val();
       if (ValidateSupplier(SUPPLIERID)) { UpdateSupplierDetails(_lstdet, GetSupplier_DefSettings, SUPPLIERID, _defFormat); }
   };

   function ValidateSupplier(suppid) {
       var _valid = true;
       var _code = $('#txtCode').val(); var _sname = $('#txtSuppname').val(); var _contactper = $('#txtContactPerson').val(); var _imppath = $('#txtDownloadPath').val();var _exppath = $('#txtUploadPath').val();
       if (_code == '') { $('#txtCode').addClass('error'); _valid = false; }   
       if (_sname == '') { $('#txtSuppname').addClass('error'); _valid = false; } else { $('#txtSuppname').removeClass('error'); }
       if (_contactper == '') { $('#txtContactPerson').addClass('error'); _valid = false; } else { $('#txtContactPerson').removeClass('error'); }
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
               try { res = Str(response.d); }
               catch (err) {
                   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'cannot validate supplier :' + err);
               }
           },
           failure: function (response) { toastr.error("failure get", response.d);},
           error: function (response) { toastr.error("error get", response.responseText); }
       });
       return res;
   };

   function UpdateSupplierDetails(_nfieldval, callback, SUPPLIERID, _defFormat) {
       var slSuppdet = [];
       for (var j = 0; j < _nfieldval.length; j++) { slSuppdet.push(_nfieldval[j]);}
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
                   if (res != "") {
                       SaveSConfigDetails(SUPPLIERID); callback(SUPPLIERID,_defFormat);
                       toastr.success("Lighthouse eSolutions Pte. Ltd", "Supplier Saved successfully.");
                   }
                   Metronic.unblockUI();
               }
               catch (err) {   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Supplier :' + err); Metronic.unblockUI();}
           },
           failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
           error: function (response) {  toastr.error("error get", response.responseText); Metronic.unblockUI(); }
       });
   };

   function DeleteSupplier(SUPPLIERID, callback) {
       Metronic.blockUI('#portlet_body');
       $.ajax({
           type: "POST",
           async: false,
           url: "SupplierDetail.aspx/DeleteSupplier",
           data: "{ 'SUPPLIERID':'" + SUPPLIERID + "' }",
           contentType: "application/json; charset=utf-8",
           dataType: "json",
           success: function (response) {
               try {
                   var res = Str(response.d);
                   if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "Supplier Deleted.");   GetSupplierDetails(SUPPLIERID);}
                   Metronic.unblockUI();
               }
               catch (err) {  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete Supplier :' + err); Metronic.unblockUI(); }
           },
           failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
           error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
       });
   };
   /* end*/

   /*Supplier Transactions*/
   function InitialiseSuppTransactn() {
       setAccessPortlet('4'); setupSTransctTableHeader();
       var table11 = $('#dataGridSTrans');
       oSTransTable = table11.dataTable({
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
           "fnRowCallback": function (nRow, aData, iDisplayIndex) {
               var fileTag = '<a href="javascript:;"><u>' + aData[9] + '</u></<a>'; $('td:eq(7)', nRow).html(fileTag);
           }
       });

       $('#tblHeadRowSTrans').addClass('gridHeader'); $('#ToolTables_dataGridSTrans_0,#ToolTables_dataGridSTrans_1').hide(); $('#dataGridSTrans').css('width', '100%');
       $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
   };

   function GetSupplierTransactions(FROM, TO, SUPPLIERID) {
       _lstfilter = SetFilter(_FROM, TO, SUPPLIERID); GetSTransactGrid(_lstfilter); //GetSupplierOverview(SUPPLIERID);
   };

   var SetFilter = function (FROM, TO, SUPPLIERID) {
       _lstfilter = [];  _fromlogdate = $(document.getElementById('dtLogFromDate')).val(); _tologdate = $(document.getElementById('dtLogToDate')).val();
       _lstfilter.push("LOG_FROM" + "|" + getSQLDateFormated(_fromlogdate)); _lstfilter.push("LOG_TO" + "|" + getSQLDateFormated(_tologdate));
       _lstfilter.push("ADDRESSID" + "|" + Str(SUPPLIERID));    _lstfilter.push("FROM" + "|" + Str(FROM)); _lstfilter.push("TO" + "|" + Str(TO));
       return _lstfilter;
   };

   var GetSTransactGrid = function (_nfieldval) {
       var slftrdet = [];
       for (var j = 0; j < _nfieldval.length; j++) { slftrdet.push(_nfieldval[j]); }
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
                       var _totalcount = response.d.split('||')[0];
                       var DataSet = JSON.parse(response.d.split('||')[1]);
                       if (DataSet.NewDataSet != null) {
                           var  Table = DataSet.NewDataSet.Table; FillSTransGrid(Table);
                       }
                       else $('#dataGridSTrans').DataTable().clear().draw();
                   }
                   Metronic.unblockUI();
               }
               catch (err) {
                   Metronic.unblockUI();
                   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Supplier Transactions :' + err);
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

   function FillSTransGrid(Table) {
       try {
           $('#dataGridSTrans').DataTable().clear().draw();
           if (Table.length != undefined && Table.length != null) {
               var t = $('#dataGridSTrans').dataTable();
               for (var i = 0; i < Table.length; i++) {
                   var cells = new Array();
                   var r = jQuery('<tr id=' + i + '>');
                   cells[0] = Str(Table[i].UPDATE_DATE);
                   cells[1] = Str(Table[i].SERVERNAME);
                   cells[2] = Str(Table[i].MODULENAME);
                   cells[3] = Str(Table[i].KEYREF2);
                   cells[4] = Str(Table[i].DOCTYPE);
                   cells[5] = Str(Table[i].LOGTYPE);
                   cells[6] = Str(Table[i].AUDITVALUE);
                   cells[7] = Str(Table[i].FILENAME);
                   cells[8] = Str(Table[i].LOGID);
                   cells[9] = Str(Table[i].FILENAME);
                   var ai = t.fnAddData(cells, false);
               }
               t.fnDraw();
           }
           else {
               if (Table.LOGID != undefined && Table.LOGID != null) {
                   var t = $('#dataGridSTrans').dataTable();
                   var r = jQuery('<tr id=' + 1 + '>');
                   var cells = new Array();
                   cells[0] = Str(Table.UPDATE_DATE);
                   cells[1] = Str(Table.SERVERNAME);
                   cells[2] = Str(Table.MODULENAME);
                   cells[3] = Str(Table.KEYREF2);
                   cells[4] = Str(Table.DOCTYPE);
                   cells[5] = Str(Table.LOGTYPE);
                   cells[6] = Str(Table.AUDITVALUE);
                   cells[7] = Str(Table.FILENAME);
                   cells[8] = Str(Table.LOGID);
                   cells[9] = Str(Table.FILENAME);
                   var ai = t.fnAddData(cells, false);
                   t.fnDraw();
               }
           }
       }
       catch (e)
       { }
   };

   var setupSTransctTableHeader = function () {
       var dtfilter = '<th>Log Date</th><th>Server Name</th><th>Module Name</th><th>Key Ref</th><th>Doc Type</th><th>Log Type</th><th>Remark</th><th>File Name</th><th>LOGID</th><th>FileName</th>';
       $('#tblHeadRowSTrans').empty().append(dtfilter); $('#tblBodySTrans').empty(); setFilterToolbar();
   };

   var setFilterToolbar = function () {
       $('#divFilter').empty();
       var _filterdet = ' <div class="row">  <div class="col-md-12">' +
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
       _lstfilter = []; setFilterToolbar(); 
       $('#dataGridSTrans').DataTable().clear().draw();
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
                               var cVirtualPath = "../Downloads/"; var win = window.open(cVirtualPath + filename, '_blank'); win.focus();
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

   function GetSupplierOverview(SUPPLIERID) {
       var _nocount = '0';
       $.ajax({
           type: "POST",
           async: false,
           url: "Overview.aspx/SetOverview_Addressid",
           data: "{'ADDRESSID':'" + SUPPLIERID + "','ADDRTYPE':'SUPPLIER'}",
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
               catch (err) {
                   toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Supplier Overview:' + err);
               }
           },
           failure: function (response) {
               toastr.error("failure get", response.d);
           },
           error: function (response) {
               toastr.error("error get", response.responseText);
           }
       });
   };
  /*end*/      
       /*update Buyer-Supplier link*/
       function setBuyerSupplinkGrid(SUPPLIERID) {
           $('#prtByLnk').show(); $('#prtDetail').hide(); $('#prtSuppConfig').hide(); $('#prtSuppRules').hide(); setupBSTableHeader();
           var table = $('#dataGridBSlnk');
           oBSLTable = table.dataTable({
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
               "columnDefs": [{ 'orderable': false, "searching": true, "autoWidth": false, 'targets': [0] },
                { 'targets': [0], width: '40px' },
                { 'targets': [2], width: '40px' }, { 'targets': [3], width: '35px' }, { 'targets': [4], width: '150px' },
                { 'targets': [5], width: '60px' }, { 'targets': [6], width: '70px' },
                { 'targets': [13], width: '75px' }, { 'targets': [16], width: '85px' },
                { 'targets': [31], width: '60px' },
                { 'targets': [44], width: '35px', 'bSortable': false },
                { 'targets': [46], width: '55px', 'bSortable': false },
                { 'targets': [1, 7, 8, 9, 10, 11, 12, 17, 18, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 45, 47, 48, 14, 15, 19, 20, 42, 43], visible: false }
               ],
               "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
               "sScrollY": '320px',
               "sScrollX": '1300px',
               "aaSorting": [],
               "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
               "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                   var _bcheck = ''; var _scheck = ''; var _acheck = '';
                   var aedit = 'rwdedit' + nRow._DT_RowIndex; var adelete = 'rwdelete' + nRow._DT_RowIndex; var aView = 'View' + nRow._DT_RowIndex;
                   var _divbtn = ' <div id="dvActn"><span id=' + aView + ' class="actionbtn" data-toggle="tooltip" title="View"><i class="glyphicon glyphicon-search"></i></span>' +
                   '<span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span>' +
                   '<span id=' + adelete + ' class="actionbtn" data-toggle="tooltip" title="Delete"><i class="glyphicon glyphicon-trash"></i></span></div>';
                   $('td:eq(0)', nRow).html(_divbtn);
                   var _nByid = "by" + nRow._DT_RowIndex; var _nSpid = "sp" + nRow._DT_RowIndex; var _nAtid = "act" + nRow._DT_RowIndex;
                   var _nByval = aData[42]; var _nSpval = aData[43]; var _nAtval = aData[44];
                   if (_nByval == '1') { _bcheck = 'checked'; } else { _bcheck = ''; } if (_nSpval == '1') { _scheck = 'checked'; } else { _scheck = ''; } if (_nAtval == '1') { _acheck = 'checked'; } else { _acheck = ''; }
                   var _nBuyer = '<div style="text-align:center;padding:2px;"><input type="checkbox"  id="' + _nByid + '"  value="' + _nByval + '" ' + _bcheck + ' disabled/></div>';
                   var _nSupplier = '<div style="text-align:center;padding:2px;"><input type="checkbox"  id="' + _nSpid + '"  value="' + _nSpval + '" ' + _scheck + ' disabled/></div>';
                   $('td:eq(13)', nRow).html(_nBuyer); $('td:eq(14)', nRow).html(_nSupplier);
                   var _nActive = '<div style="text-align:center;padding:2px;"><input type="checkbox"  id="' + _nAtid + '"  value="' + _nAtval + '" ' + _acheck + ' disabled/></div>';
                   $('td:eq(9)', nRow).html(_nActive);
                   if (_nAtval == '1') { $("td:eq(2)", nRow).addClass('activeLink'); }
               }
           });
           $('#tblHeadRowBSlnk').addClass('gridHeader'); $('#ToolTables_dataGridBSlnk_0,#ToolTables_dataGridBSlnk_1').hide();
           $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
           if (sessionStorage['SUPPLIERID'] != '') { SUPPLIERID = sessionStorage['SUPPLIERID']; } GetBuyerSuppLinkGrid(SUPPLIERID);
       };

       function GetBSLinkDetails(_indx) {
           var _lstdet = [];
           var tid = "BSLinkTable";
           var _code = $('#txtCode').val(); var _bLnkcd = $('#txtBLinkCode').val(); var _sLnkcd = $('#txtSLinkCode').val(); var _grp = $('#cbGrp').val();
           var _suppSenCd = $('#txtSuppSenCd').val(); var _suppRecCd = $('#txtSuppRecCd').val(); var _bSenCd = $('#txtBSenCd').val();
           var _brecvd = $('#txtBRecCd').val(); var _bmap = $('#txtBMapping').val();  var _smap = $('#txtSMapping').val();
           var _bcont = $('#txtBContact').val(); var _scont = $('#txtSContact').val(); var _bemail = $('#txtBEmail').val();
           var _semail = $('#txtSEmail').val(); var _ccemail = $('#txtCCEmail').val(); var _bccemail = $('#txtBCCEmail').val();
           var _mailsb = $('#txtMailsub').val(); var _reply = $('#txtReplyEmail').val(); var _bimp = $('#txtBImpPath').val();
           var _bexp = $('#txtBExpPath').val(); var _simp = $('#txtSImpPath').val(); var _sexp = $('#txtSExpPath').val();
           var _wurl = $('#txtWebserURL').val(); var _dprice = $('#txtDefPrice').val(); var _ufilety = $('#txtUpFileType').val();
           var _rfq = ($('#chkRFQProcess').is(':checked')) ? 1 : 0; var _quote = ($('#chkQuoteProcess').is(':checked')) ? 1 : 0;
           var _po = ($('#chkPOProcess').is(':checked')) ? 1 : 0; var _poc = ($('#chkPOCProcess').is(':checked')) ? 1 : 0;
           var _irfq = ($('#chkImpRFQ').is(':checked')) ? 1 : 0; var _erfq = ($('#chkExpRFQ').is(':checked')) ? 1 : 0;
           var _erfqack = ($('#chkExpRFQAck').is(':checked')) ? 1 : 0; var _iqte = ($('#chkImpQuote').is(':checked')) ? 1 : 0;
           var _eqte = ($('#chkExpQuote').is(':checked')) ? 1 : 0; var _ipo = ($('#chkImpPO').is(':checked')) ? 1 : 0;
           var _epo = ($('#chkExpPO').is(':checked')) ? 1 : 0; var _epoack = ($('#chkExpPOAck').is(':checked')) ? 1 : 0;
           var _epoc = ($('#chkExpPOC').is(':checked')) ? 1 : 0; var _nby = ($('#chkNBuyer').is(':checked')) ? 1 : 0;
           var _nsp = ($('#chkNSupp').is(':checked')) ? 1 : 0; var _act = ($('#chkAct').is(':checked')) ? 1 : 0;
           var _ipoc = ($('#chkImpPOC').is(':checked')) ? 1 : 0;
           _lstdet.push("BUYER_CODE" + "|" + Str(_code)); _lstdet.push("BUYER_LINK_CODE" + "|" + Str(_bLnkcd)); _lstdet.push("VENDOR_LINK_CODE" + "|" + Str(_sLnkcd)); _lstdet.push("BUYER_EMAIL" + "|" + Str(_bemail));
           _lstdet.push("SUPPLIER_EMAIL" + "|" + Str(_semail)); _lstdet.push("SUPP_SENDER_CODE" + "|" + Str(_suppSenCd));
           _lstdet.push("SUPP_RECEIVER_CODE" + "|" + Str(_suppRecCd)); _lstdet.push("BYR_SENDER_CODE" + "|" + Str(_bSenCd)); _lstdet.push("BYR_RECEIVER_CODE" + "|" + Str(_brecvd)); _lstdet.push("BUYER_MAPPING" + "|" + Str(_bmap)); _lstdet.push("SUPPLIER_MAPPING" + "|" + Str(_smap));
           _lstdet.push("BUYER_CONTACT" + "|" + Str(_bcont)); _lstdet.push("SUPPLIER_CONTACT" + "|" + Str(_scont));
           _lstdet.push("IMPORT_PATH" + "|" + Str(_bimp)); _lstdet.push("EXPORT_PATH" + "|" + Str(_bexp)); _lstdet.push("SUPP_IMPORT_PATH" + "|" + Str(_simp)); _lstdet.push("SUPP_EXPORT_PATH" + "|" + Str(_sexp));
           _lstdet.push("CC_EMAIL" + "|" + Str(_ccemail)); _lstdet.push("BCC_EMAIL" + "|" + Str(_bccemail)); _lstdet.push("MAIL_SUBJECT" + "|" + Str(_mailsb));
           _lstdet.push("REPLY_EMAIL" + "|" + Str(_reply)); _lstdet.push("GROUP_ID" + "|" + Str(_grp)); _lstdet.push("IMPORT_RFQ" + "|" + Str(_irfq)); _lstdet.push("EXPORT_RFQ" + "|" + Str(_erfq));
           _lstdet.push("EXPORT_RFQ_ACK" + "|" + Str(_erfqack)); _lstdet.push("IMPORT_QUOTE" + "|" + Str(_iqte)); _lstdet.push("EXPORT_QUOTE" + "|" + Str(_eqte)); _lstdet.push("IMPORT_PO" + "|" + Str(_ipo)); _lstdet.push("EXPORT_PO" + "|" + Str(_epo));
           _lstdet.push("EXPORT_PO_ACK" + "|" + Str(_epoack)); _lstdet.push("EXPORT_POC" + "|" + Str(_epoc)); _lstdet.push("NOTIFY_BUYER" + "|" + Str(_nby)); _lstdet.push("NOTIFY_SUPPLR" + "|" + Str(_nsp));
           _lstdet.push("DEFAULT_PRICE" + "|" + Str(_dprice)); _lstdet.push("IS_ACTIVE" + "|" + Str(_act)); _lstdet.push("UPLOAD_FILE_TYPE" + "|" + Str(_ufilety)); _lstdet.push("SUP_WEB_SERVICE_URL" + "|" + Str(_wurl));
           _lstdet.push("IMPORT_POC" + "|" + Str(_ipoc));
           return _lstdet;
       };

       var SetLoginInfo = function (ChangePWD, LINKID) {
           var res = '';
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/SetLoginInfo",
               data: "{ 'ChangePWD':'" + ChangePWD + "','LINKID':'" + LINKID + "' }",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       res = Str(response.d);
                   }
                   catch (err) {
                       toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Login Details :' + err);
                   }
               },
               failure: function (response) {
                   toastr.error("failure get", response.d);
               },
               error: function (response) {
                   toastr.error("error get", response.responseText);
               }
           });
           return res;
       };

       function SetActive(ACTIVE, USERID) {
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/SetActiveStatus",
               data: "{ 'ACTIVE':'" + ACTIVE + "','USERID':'" + USERID + "' }",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var res = Str(response.d);
                       if (res != "") {
                           toastr.success("Lighthouse eSolutions Pte. Ltd", "Active status changed.");
                       }
                   }
                   catch (err) {
                       toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to change Active status :' + err);
                   }
               },
               failure: function (response) {
                   toastr.error("failure get", response.d);
               },
               error: function (response) {
                   toastr.error("error get", response.responseText);
               }
           });
       };

       function SaveChangePassword(NEWPWD, USERID) {
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/UpdatePassword",
               data: "{ 'NEWPWD':'" + NEWPWD + "','USERID':'" + USERID + "' }",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var res = Str(response.d);
                       if (res != "") {
                           toastr.success("Lighthouse eSolutions Pte. Ltd", "Password Changed successfully.");
                       }
                       else {
                           toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to change Password');
                       }
                   }
                   catch (err) {
                       toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to change Password :' + err);
                   }
               },
               failure: function (response) {
                   toastr.error("failure get", response.d);
               },
               error: function (response) {
                   toastr.error("error get", response.responseText);
               }
           });
       };

       var CheckExistingBSuppLink = function (LINKID, BLINKCODE, SLINKCODE) {
           var res = '';
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/CheckExistingBSuppLink",
               data: "{ 'LINKID':'" + LINKID + "','BLINKCODE':'" + BLINKCODE + "','SLINKCODE':'" + SLINKCODE + "' }",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       res = Str(response.d);
                   }
                   catch (err) {
                       toastr.error("Lighthouse eSolutions Pte. Ltd.", 'cannot validate new buyer-supplier link :' + err);
                   }
               },
               failure: function (response) {
                   toastr.error("failure get", response.d);
               },
               error: function (response) {
                   toastr.error("error get", response.responseText);
               }
           });
           return res;
       };

       var CheckExistingGroup = function (GROUPCODE) {
           var res = '';
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/CheckExistingGroup",
               data: "{ 'GROUPCODE':'" + GROUPCODE + "' }",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       res = Str(response.d);
                   }
                   catch (err) {
                       toastr.error("Lighthouse eSolutions Pte. Ltd.", 'cannot validate group :' + err);
                   }
               },
               failure: function (response) {
                   toastr.error("failure get", response.d);
               },
               error: function (response) {
                   toastr.error("error get", response.responseText);
               }
           });
           return res;
       };

       function ValidateBSLink(_linkid) {
           var _valid = true;
           var _bLnkcd = $('#txtBLinkCode').val(); var _sLnkcd = $('#txtSLinkCode').val();  var _act = ($('#chkAct').is(':checked')) ? 1 : 0;
           var _suppSenCd = $('#txtSuppSenCd').val(); var _suppRecCd = $('#txtSuppRecCd').val();
           var _mailsb = $('#txtMailsub').val(); var _grp = $('#cbGrp').val(); var _grpcode = $('#cbGrp option:selected').text();
           var _bimp = $('#txtBImpPath').val(); var _bexp = $('#txtBExpPath').val();
           if (_bLnkcd == '') { $('#txtBLinkCode').addClass('error'); _valid = false; } else { $('#txtBLinkCode').removeClass('error'); }
           if (_sLnkcd == '') { $('#txtSLinkCode').addClass('error'); _valid = false; } else { $('#txtSLinkCode').removeClass('error'); }
           if (_bLnkcd != '' && _sLnkcd != '') {
               var isexist = CheckExistingBSuppLink(_linkid, _bLnkcd, _sLnkcd);if (isexist != '') { toastr.error("Lighthouse eSolutions Pte. Ltd.", isexist); _valid = false; }
           }
           if (_act == '1') {
               if (_suppSenCd == '') { $('#txtSuppSenCd').addClass('error'); _valid = false; } else { $('#txtSuppSenCd').removeClass('error'); }
               if (_suppRecCd == '') { $('#txtSuppRecCd').addClass('error'); _valid = false; } else { $('#txtSuppRecCd').removeClass('error'); }
               if (_mailsb == '') { $('#txtMailsub').addClass('error'); _valid = false; } else { $('#txtMailsub').removeClass('error'); }
               // if (_grp != '') {var isexist = CheckExistingGroup(_grpcode);
               // if (isexist != '') { toastr.error("Lighthouse eSolutions Pte. Ltd.", isexist); _valid = false; } else { $('#cbGrp').removeClass('error'); }
               //  }
               //else { $('#cbGrp').addClass('error'); _valid = false; }
               if (_bimp == '') { $('#txtBImpPath').addClass('error'); _valid = false; } else { $('#txtBImpPath').removeClass('error'); }
               if (_bexp == '') { $('#txtBExpPath').addClass('error'); _valid = false; } else { $('#txtBExpPath').removeClass('error'); }
           }
           return _valid;
       };

       function SaveBSupplierLinkDetails(_nfieldval, callback, LINKID, SUPPLIERID) {
           var slBSLnkdet = [];
           for (var j = 0; j < _nfieldval.length; j++) {
               slBSLnkdet.push(_nfieldval[j]);
           }
           var data2send = JSON.stringify({ LINKID: LINKID, slBSLnkdet: slBSLnkdet });
           Metronic.blockUI('#portlet_body');
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/SaveBuyerSuppLinkDetails",
               data: data2send,
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var res = Str(response.d);
                       if (res != "") {
                           toastr.success("Lighthouse eSolutions Pte. Ltd", "Buyer-Supplier Link Saved successfully.");
                           GetBuyerSuppLinkGrid(SUPPLIERID);
                       }
                       Metronic.unblockUI();
                   }
                   catch (err) {
                       toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Buyer-Supplier Link :' + err); Metronic.unblockUI();
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

       function DeleteBSLink(LINKID, SUPPLIERID, callback) {
           Metronic.blockUI('#portlet_body');
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/DeleteBuyerSuppLink",
               data: "{ 'LINKID':'" + LINKID + "' }",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var res = Str(response.d);
                       if (res != "") {
                           toastr.success("Lighthouse eSolutions Pte. Ltd", "Buyer-Supplier Link Deleted.");
                           GetBuyerSuppLinkGrid(SUPPLIERID);
                       }
                       Metronic.unblockUI();
                   }
                   catch (err) {
                       toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete "Buyer-Supplier Link :' + err); Metronic.unblockUI();
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
  
       function FillBuyerSupplinkGrid(Table) {
           try {
               $('#dataGridBSlnk').DataTable().clear().draw();
               if (Table.length != undefined && Table.length != null) {
                   var t = $('#dataGridBSlnk').dataTable();
                   for (var i = 0; i < Table.length; i++) {
                       var cells = new Array();

                       var r = jQuery('<tr id=' + i + '>');
                       cells[0] = Str('');
                       cells[1] = Str('');
                       cells[2] = Str(Table[i].LINKID);//
                       cells[3] = Str(Table[i].BUYER_CODE);
                       cells[4] = Str(Table[i].BUYER_NAME);
                       cells[5] = Str(Table[i].BUYER_LINK_CODE);
                       cells[6] = Str(Table[i].VENDOR_LINK_CODE);
                       cells[7] = Str(Table[i].SUPP_SENDER_CODE);//
                       cells[8] = Str(Table[i].SUPP_RECEIVER_CODE);//
                       cells[9] = Str(Table[i].BYR_SENDER_CODE);//
                       cells[10] = Str(Table[i].BYR_RECEIVER_CODE);//
                       cells[11] = Str(Table[i].BUYER_MAPPING);//
                       cells[12] = Str(Table[i].SUPPLIER_MAPPING);//
                       cells[13] = Str(Table[i].BUYER_FORMAT);
                       cells[14] = Str(Table[i].BUYER_EXPORT_FORMAT);
                       cells[15] = Str(Table[i].VENDOR_FORMAT);
                       cells[16] = Str(Table[i].SUPPLIER_EXPORT_FORMAT);
                       cells[17] = Str(Table[i].BUYER_CONTACT);//
                       cells[18] = Str(Table[i].SUPPLIER_CONTACT);//
                       cells[19] = Str(Table[i].BUYER_EMAIL);
                       cells[20] = Str(Table[i].SUPPLIER_EMAIL);
                       cells[21] = Str(Table[i].CC_EMAIL);//
                       cells[22] = Str(Table[i].BCC_EMAIL);//
                       cells[23] = Str(Table[i].MAIL_SUBJECT);//
                       cells[24] = Str(Table[i].REPLY_EMAIL);//
                       cells[25] = Str(Table[i].IMPORT_PATH);//
                       cells[26] = Str(Table[i].EXPORT_PATH);//
                       cells[27] = Str(Table[i].SUPP_IMPORT_PATH);//
                       cells[28] = Str(Table[i].SUPP_EXPORT_PATH);//
                       cells[29] = Str(Table[i].SUP_WEB_SERVICE_URL);//
                       var _defprice = (Table[i].DEFAULT_PRICE != undefined) ? parseFloat(Table[i].DEFAULT_PRICE) : parseFloat('0');
                       cells[30] = _defprice.toFixed(2);//
                       cells[31] = Str(Table[i].GROUP_CODE);
                       cells[32] = Str(Table[i].UPLOAD_FILE_TYPE);//
                       cells[33] = Str(Table[i].IMPORT_RFQ);//
                       cells[34] = Str(Table[i].EXPORT_RFQ);//
                       cells[35] = Str(Table[i].EXPORT_RFQ_ACK);//
                       cells[36] = Str(Table[i].IMPORT_QUOTE);//
                       cells[37] = Str(Table[i].EXPORT_QUOTE);//
                       cells[38] = Str(Table[i].IMPORT_PO);//
                       cells[39] = Str(Table[i].EXPORT_PO);//
                       cells[40] = Str(Table[i].EXPORT_PO_ACK);//
                       cells[41] = Str(Table[i].EXPORT_POC);//
                       cells[42] = Str(Table[i].NOTIFY_BUYER);
                       cells[43] = Str(Table[i].NOTIFY_SUPPLR);
                       cells[44] = Str(Table[i].IS_ACTIVE);
                       cells[45] = Str(Table[i].EX_USERID);//
                       cells[46] = Str(Table[i].EXPASSWORD);
                       cells[47] = Str(Table[i].BUYERID);//
                       cells[48] = Str(Table[i].IMPORT_POC);//
                       var ai = t.fnAddData(cells, false);
                   }
                   t.fnDraw();
               }
               else {
                   if (Table.LINKID != undefined && Table.LINKID != null) {
                       var t = $('#dataGridBSlnk').dataTable();
                       var r = jQuery('<tr id=' + 1 + '>');
                       var cells = new Array();
                       cells[0] = Str('');
                       cells[1] = Str('');
                       cells[2] = Str(Table.LINKID);//
                       cells[3] = Str(Table.BUYER_CODE);
                       cells[4] = Str(Table.BUYER_NAME);
                       cells[5] = Str(Table.BUYER_LINK_CODE);
                       cells[6] = Str(Table.VENDOR_LINK_CODE);
                       cells[7] = Str(Table.SUPP_SENDER_CODE);//
                       cells[8] = Str(Table.SUPP_RECEIVER_CODE);//
                       cells[9] = Str(Table.BYR_SENDER_CODE);//
                       cells[10] = Str(Table.BYR_RECEIVER_CODE);//
                       cells[11] = Str(Table.BUYER_MAPPING);//
                       cells[12] = Str(Table.SUPPLIER_MAPPING);//
                       cells[13] = Str(Table.BUYER_FORMAT);
                       cells[14] = Str(Table.BUYER_EXPORT_FORMAT);
                       cells[15] = Str(Table.VENDOR_FORMAT);
                       cells[16] = Str(Table.SUPPLIER_EXPORT_FORMAT);
                       cells[17] = Str(Table.BUYER_CONTACT);//
                       cells[18] = Str(Table.SUPPLIER_CONTACT);//
                       cells[19] = Str(Table.BUYER_EMAIL);
                       cells[20] = Str(Table.SUPPLIER_EMAIL);
                       cells[21] = Str(Table.CC_EMAIL);//
                       cells[22] = Str(Table.BCC_EMAIL);//
                       cells[23] = Str(Table.MAIL_SUBJECT);//
                       cells[24] = Str(Table.REPLY_EMAIL);//
                       cells[25] = Str(Table.IMPORT_PATH);//
                       cells[26] = Str(Table.EXPORT_PATH);//
                       cells[27] = Str(Table.SUPP_IMPORT_PATH);//
                       cells[28] = Str(Table.SUPP_EXPORT_PATH);//
                       cells[29] = Str(Table.SUP_WEB_SERVICE_URL);//
                       var _defprice = (Table.DEFAULT_PRICE != undefined) ? parseFloat(Table.DEFAULT_PRICE) : parseFloat('0');
                       cells[30] = _defprice.toFixed(2);//
                       cells[31] = Str(Table.GROUP_CODE);
                       cells[32] = Str(Table.UPLOAD_FILE_TYPE);//
                       cells[33] = Str(Table.IMPORT_RFQ);//
                       cells[34] = Str(Table.EXPORT_RFQ);//
                       cells[35] = Str(Table.EXPORT_RFQ_ACK);//
                       cells[36] = Str(Table.IMPORT_QUOTE);//
                       cells[37] = Str(Table.EXPORT_QUOTE);//
                       cells[38] = Str(Table.IMPORT_PO);//
                       cells[39] = Str(Table.EXPORT_PO);//
                       cells[40] = Str(Table.EXPORT_PO_ACK);//
                       cells[41] = Str(Table.EXPORT_POC);//
                       cells[42] = Str(Table.NOTIFY_BUYER);
                       cells[43] = Str(Table.NOTIFY_SUPPLR);
                       cells[44] = Str(Table.IS_ACTIVE);
                       cells[45] = Str(Table.EX_USERID);//
                       cells[46] = Str(Table.EXPASSWORD);
                       cells[47] = Str(Table.BUYERID);//
                       cells[48] = Str(Table.IMPORT_POC);//
                       var ai = t.fnAddData(cells, false);
                       t.fnDraw();
                   }
               }
           }
           catch (e)
           { }
       };

       var GetBuyerSuppLinkGrid = function (SUPPLIERID) {
           Metronic.blockUI('#portlet_body');
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/FillLinkedBuyersGrid",
               data: "{'SUPPLIERID':'" + SUPPLIERID + "'}",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var DataSet = JSON.parse(response.d);
                       if (DataSet.NewDataSet != null) {
                           Table = DataSet.NewDataSet.Table;
                           FillBuyerSupplinkGrid(Table);
                       }
                       else $('#dataGridBSlnk').DataTable().clear().draw();
                       Metronic.unblockUI();
                   }
                   catch (err) {
                       Metronic.unblockUI();
                       toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Suppliers :' + err);
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

       var setupBSTableHeader = function () {
           //var dtfilter = '<th>View</th><th><div style="text-align:center;display:none;"><span><a style="color: #ffffff;"><u>New</u></<a></span></div></th> <th>Code</th><th>Name</th>  <th>Buyer Link Code</th>' +
           var dtfilter = '<th class="text-center">#</th><th><div style="text-align:center;display:none;"><span><a style="color: #ffffff;"><u>New</u></<a></span></div></th><th>LinkId</th><th>Buyer Code</th><th>Buyer Name</th>  <th>Buyer Link Code</th>' +
               ' <th>Supplier Link Code</th><th>Supplier Sender Code</th><th>Supplier Receiver Code</th> <th>Buyer Sender Code</th><th>Buyer Receiver Code</th>' +
               ' <th>Buyer Mapping</th><th>Supplier Mapping</th><th>Buyer Import Format</th><th>Buyer Export Format</th><th>Supplier Export Format</th> ' +
               ' <th>Supplier Import Format</th><th>Buyer Contact</th><th>Supplier Contact</th><th>Buyer Email</th><th>Supplier Email</th><th>CC Email</th> ' +
               ' <th>BCC Email</th><th>Mail subject</th><th>Reply Email</th><th>Buyer Import Path</th><th>Buyer Export Path</th><th>Supplier Import Path</th>' +
               ' <th>Supplier Export Path</th><th>Webservice URL</th><th>Default Price</th><th>Group</th><th>Upload File Type</th>' +
               ' <th>Import RFQ</th><th>Export RFQ</th><th>Export RFQ Ack.</th><th>Import Quote</th><th>Export Quote</th><th>Import PO</th><th>Export PO</th>' +
               ' <th>Export PO Ack.</th><th>Export POC</th><th>Notify Buyer</th><th>Notify Supplier</th><th>Active</th><th>Login Info</th><th>Password</th>' +
               ' <th>BUYERID</th><th>Import POC</th>';
           $('#tblHeadRowBSlnk').empty(); $('#tblHeadRowBSlnk').append(dtfilter); $('#tblBodyBSlnk').empty();
       };

       function FillGroups() {
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/GetAllGroups",
               data: "{}",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var Dataset = JSON.parse(response.d);
                       if (Dataset.NewDataSet != null) {
                           var Table = Dataset.NewDataSet.Table;
                           _lstgrp = [];
                           if (Table != undefined && Table != null) {
                               if (Table.length != undefined) {
                                   for (var i = 0; i < Table.length; i++) {
                                       _lstgrp.push(Str(Table[i].GROUP_ID) + "|" + Str(Table[i].GROUP_CODE));
                                   }
                               }
                               else {
                                   if (Table.GROUP_ID != undefined) {
                                       _lstgrp.push(Str(Table.GROUP_ID) + "|" + Str(Table.GROUP_CODE));
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

       function FillCombo(val, _lst) {
           var opt = ''; emptystr = '';
           try {
               opt = '<option>' + emptystr + '</option>';
               if (_lst.length != undefined && _lst.length > 0) {
                   for (var i = 0; i < _lst.length; i++) {
                       var cdet = _lst[i].split("|");
                       if (val != "" && val == Str(cdet[0])) { opt += '<option selected value="' + Str(cdet[0]) + '">' + Str(cdet[1]) + '</option>'; }
                       else if (val != "" && val == Str(cdet[1])) { opt += '<option selected value="' + Str(cdet[0]) + '">' + Str(cdet[1]) + '</option>'; }
                       else { opt += '<option value="' + Str(cdet[0]) + '">' + Str(cdet[1]) + '</option>'; }
                   }
                   return opt;
               }
           }
           catch (err) {
               toastr.error('Error while populating List :' + err, "Lighthouse eSolutions Pte. Ltd");
           }
       };
       /* end */

      /*Buyer Supplier Link Rules*/
       function setBuyerSupplinkRulesGrid(LINKID) {
           $('#prtBSLnkRules').show(); $('#prtItemUOM').hide(); $('#prtItemRef').hide(); setaccess('101');
           setupBSRulesTableHeader();
           var table = $('#dataGridBSLnkRules');
           oBSRTable = table.dataTable({
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
               "columnDefs": [{ 'orderable': false, "searching": true, "autoWidth": false, 'targets': [0] },
               { 'targets': [0], width: '10px' }, { 'targets': [2], width: '50px' },
               { 'targets': [1], width: '40px' }, { 'targets': [3], "sClass": "break-det" }, { 'targets': [4, 5], width: '120px' }, { 'targets': [6, 7], width: '40px' }, { 'targets': [8], visible: false }
               ],
               "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
               "sScrollY": '300px',
               "sScrollX": true,
               "aaSorting": [],
               "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
               "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                   $('td:eq(6)', nRow).css('text-align', 'center'); $('td:eq(7)', nRow).css('padding-left', '5px'); var divid = 'dv' + nRow._DT_RowIndex; var aedit = 'rwdedit' + nRow._DT_RowIndex;
                   var detTag = ' <div id="' + divid + '" style="text-align:center;"><span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span></div>';
                   $('td:eq(0)', nRow).html(detTag);
               }
           });
           $('#tblHeadRowBSLnkRules').addClass('gridHeader'); $('#ToolTables_dataGridBSLnkRules_0,#ToolTables_dataGridBSLnkRules_1').hide();
           $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
           if (Str(LINKID) == '') { LINKID = Str(sessionStorage['LINKID']); } GetBSuppRulesGrid(LINKID);
       };

       function FillBSuppRulesGrid(Table) {
           try {
               $('#dataGridBSLnkRules').DataTable().clear().draw();
               if (Table.length != undefined && Table.length != null) {
                   var t = $('#dataGridBSLnkRules').dataTable();
                   for (var i = 0; i < Table.length; i++) {
                       var cells = new Array();
                       var r = jQuery('<tr id=' + i + '>');
                       var _arrcommt = Str(Table[i].RULE_COMMENTS).split(',').join('<br/>');
                       cells[0] = Str('');
                       cells[1] = Str(Table[i].RULE_NUMBER);
                       cells[2] = Str(Table[i].DOC_TYPE);
                       cells[3] = Str(Table[i].RULE_CODE);
                       cells[4] = Str(Table[i].RULE_DESC);
                       cells[5] = _arrcommt;
                       cells[6] = Str(Table[i].RULE_VALUE);
                       cells[7] = (Str(Table[i].INHERIT_RULE) == '1') ? 'Yes' : 'No';
                       cells[8] = Str(Table[i].RULEID);
                       var ai = t.fnAddData(cells, false);
                   }
                   t.fnDraw();
               }
               else {
                   if (Table.RULEID != undefined && Table.RULEID != null) {
                       var t = $('#dataGridBSLnkRules').dataTable();
                       var r = jQuery('<tr id=' + 1 + '>');
                       var cells = new Array();
                       var _arrcommt = Str(Table.RULE_COMMENTS).split(',').join('<br/>');
                       cells[0] = Str('');
                       cells[1] = Str(Table.RULE_NUMBER);
                       cells[2] = Str(Table.DOC_TYPE);
                       cells[3] = Str(Table.RULE_CODE);
                       cells[4] = Str(Table.RULE_DESC);
                       cells[5] = _arrcommt;
                       cells[6] = Str(Table.RULE_VALUE);
                       cells[7] = (Str(Table.INHERIT_RULE) == '1') ? 'Yes' : 'No';
                       cells[8] = Str(Table.RULEID);
                       var ai = t.fnAddData(cells, false);
                       t.fnDraw();
                   }
               }
           }
           catch (e) { }
       };

       var GetBSuppRulesGrid = function (LINKID) {
           Metronic.blockUI('#portlet_body');
           $.ajax({
               type: "POST",
               async: false,
               url: "LinkRules.aspx/FillRulesGrid",
               data: "{'LINKID':'" + LINKID + "'}",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var DataSet = JSON.parse(response.d);
                       if (DataSet.NewDataSet != null) {
                           Table = DataSet.NewDataSet.Table; FillBSuppRulesGrid(Table);
                       }
                       else $('#dataGridBSLnkRules').DataTable().clear().draw();
                       Metronic.unblockUI();
                   }
                   catch (err) { Metronic.unblockUI(); toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer/Supplier Rules :' + err); }
               },
               failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
               error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
           });
       };

       var setupBSRulesTableHeader = function () {
           var dtfilter = '<th></th><th>Rule No.</th><th>Doc Type</th><th>Rule Code</th><th>Description</th><th>Comments</th><th>Rule Value</th><th>Is Inherit</th><th>RULEID</th>';
           $('#tblHeadRowBSLnkRules').empty(); $('#tblHeadRowBSLnkRules').append(dtfilter); $('#tblBodyBSLnkRules').empty();
       };
       /* end */

       /* New Linked Rule */
       function ValidateRule(_ruleval) { var _valid = true; if (_ruleval == '') { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Rule Value field is blank'); _valid = false; }  return _valid; };

       function SaveBSLinkRule(_nfieldval, callback, linkid) {
           var slBSRuledet = [];
           for (var j = 0; j < _nfieldval.length; j++) { slBSRuledet.push(_nfieldval[j]); }
           var data2send = JSON.stringify({ slBSRuledet: slBSRuledet });
           Metronic.blockUI('#portlet_body');
           $.ajax({
               type: "POST",
               async: false,
               url: "LinkRules.aspx/SaveBSRuleDetails",
               data: data2send,
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var res = Str(response.d);
                       if (res != "") {
                           toastr.success("Lighthouse eSolutions Pte. Ltd", "Rule updated successfully.");
                           GetBSuppRulesGrid(linkid);
                       }
                       Metronic.unblockUI();
                   }
                   catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Rule details :' + err); Metronic.unblockUI(); }
               },
               failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
               error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
           });
       };

       function AddNewLinkRule(LINKID, _nfieldval) {
           var slChkdet = [];
           for (var j = 0; j < _nfieldval.length; j++) { slChkdet.push(_nfieldval[j]); }
           var data2send = JSON.stringify({ LINKID: LINKID, slChkdet: slChkdet });
           Metronic.blockUI('#portlet_body');
           $.ajax({
               type: "POST",
               async: false,
               url: "LinkRules.aspx/AddNewRuleDetails",
               data: data2send,
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var res = Str(response.d);
                       if (res != "") {
                           toastr.success("Lighthouse eSolutions Pte. Ltd", " Rule added successfully."); GetBSuppRulesGrid(LINKID);
                       }
                       Metronic.unblockUI();
                   }
                   catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to add Rule details :' + err); Metronic.unblockUI(); }
               },
               failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
               error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
           });
       };

       function FillNewRuleGrid(Table) {
           try {
               $('#dataGridNewRule').DataTable().clear().draw();
               if (Table.length != undefined && Table.length != null) {
                   var t = $('#dataGridNewRule').dataTable();
                   for (var i = 0; i < Table.length; i++) {
                       var cells = new Array();
                       var r = jQuery('<tr id=' + i + '>');
                       cells[0] = Str('');
                       cells[1] = Str(Table[i].RULE_NUMBER);
                       cells[2] = Str(Table[i].DOC_TYPE);
                       cells[3] = Str(Table[i].RULE_CODE);
                       cells[4] = Str(Table[i].RULE_DESC);
                       cells[5] = Str(Table[i].RULEID);
                       var ai = t.fnAddData(cells, false);
                   }
                   t.fnDraw();
               }
               else {
                   if (Table.LINKID != undefined && Table.LINKID != null) {
                       var t = $('#dataGridNewRule').dataTable();
                       var r = jQuery('<tr id=' + 1 + '>');
                       var cells = new Array();
                       cells[0] = Str('');
                       cells[1] = Str(Table.RULE_NUMBER);
                       cells[2] = Str(Table.DOC_TYPE);
                       cells[3] = Str(Table.RULE_CODE);
                       cells[4] = Str(Table.RULE_DESC);
                       cells[5] = Str(Table.RULEID);
                       var ai = t.fnAddData(cells, false);
                       t.fnDraw();
                   }
               }
           }
           catch (e)
           { }
       };

       var GetNewRuleGrid = function (LINKID) {
           $.ajax({
               type: "POST",
               async: false,
               url: "LinkRules.aspx/FillUnLinkedRulesGrid",
               data: "{'LINKID':'" + LINKID + "'}",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var DataSet = JSON.parse(response.d);
                       if (DataSet.NewDataSet != null) {
                           Table = DataSet.NewDataSet.Table; FillNewRuleGrid(Table);
                       }
                       else $('#dataGridNewRule').DataTable().clear().draw();
                   }
                   catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer/Supplier Details :' + err); }
               },
               failure: function (response) { toastr.error("failure get", response.d); },
               error: function (response) { toastr.error("error get", response.responseText); }
           });
       };

       var setupNewRulesTableHeader = function () {
           var dtfilter = '<th></th><th>Rule No.</th><th>Doc Type</th><th>Rule Code</th><th>Rule Desc</th><th>RULEID</th>';
           $('#tblHeadRowNewRule').empty(); $('#tblHeadRowNewRule').append(dtfilter); $('#tblBodyNewRule').empty();
       };
       /* end */

           /* Item reference*/
       function setBuyerSuppItemRefGrid(LINKID) {
           $('#prtItemRef').show(); $('#prtItemUOM').hide(); $('#prtBSLnkRules').hide(); setupBSItemTableHeader();
           var RefTable = $('#dataGridBSItemRef');
           oBSIRefTable = RefTable.dataTable({
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
               "columnDefs": [{ 'orderable': false, "searching": true, "autoWidth": false, 'bSortable': false, 'targets': [0] },
              { 'targets': [0], width: '20px' }, { 'targets': [1, 2, 3], width: '50px' }, { 'targets': [4, 5], width: '100px' }, { 'targets': [6, 7, 8], visible: false }
               ],
               "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
               "sScrollY": '220px',
               "sScrollX": true,
               "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
               "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                   var divid = 'dviref' + nRow._DT_RowIndex; var aedit = 'rwdedit' + nRow._DT_RowIndex; var adelete = 'rwdelete' + nRow._DT_RowIndex;
                   var detTag = ' <div id="' + divid + '" style="text-align:center;"><span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span>' +
                  '<span id=' + adelete + ' class="actionbtn" data-toggle="tooltip" title="Delete"><i class="glyphicon glyphicon-trash"></i></span></div>';
                   if (_isBSItemupdate != nRow._DT_RowIndex) { $('td:eq(0)', nRow).html(detTag); }
               }
           });

           $('#tblHeadRowBSItemRef').addClass('gridHeader'); $('#ToolTables_dataGridBSItemRef_0,#ToolTables_dataGridBSItemRef_1').hide();
           $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
           if (Str(LINKID) == '') { LINKID = Str(sessionStorage['LINKID']); } GetBSItemRefGrid(LINKID);
       };

       function GetItemRefDetails(Table, nTr, _lstdet, _targetclick) {
           var indx = ''; var _refid = '0';
           if (nTr != '') { indx = nTr.rowIndex; _refid = Table.fnGetData(nTr)[8]; } else { indx = 0; }
           var tid = "ItemRefTable" + indx; var _reftype = $('#txtRefType' + indx).val(); var _bref = $('#txtBuyerRef' + indx).val();
           var _sref = $('#txtSupplierRef' + indx).val(); var itemdesc = $('#txtDescr' + indx).val(); var rmk = $('#txtRemarks' + indx).val();
           _lstdet.push("REFTYPE" + "|" + Str(_reftype)); _lstdet.push("BUYER_REF" + "|" + Str(_bref));
           _lstdet.push("SUPPLIER_REF" + "|" + Str(_sref)); _lstdet.push("COMMENTS" + "|" + Str(rmk));
           _lstdet.push("ITEM_DESCR" + "|" + Str(itemdesc)); _lstdet.push("REFID" + "|" + Str(_refid));
       };

       function ValidateItemRef(indx) {
           var _valid = true;
           var _reftype = $('#txtRefType' + indx).val();
           var _bref = $('#txtBuyerRef' + indx).val();
           var _sref = $('#txtSupplierRef' + indx).val();
           var itemdesc = $('#txtDescr' + indx).val();
           if (_reftype == '') { $('#txtRefType' + indx).addClass('error'); _valid = false; } else { $('#txtRefType' + indx).removeClass('error'); }
           if (_bref == '') { $('#txtBuyerRef' + indx).addClass('error'); _valid = false; } else { $('#txtBuyerRef' + indx).removeClass('error'); }
           if (_sref == '') { $('#txtSupplierRef' + indx).addClass('error'); _valid = false; } else { $('#txtSupplierRef' + indx).removeClass('error'); }
           if (itemdesc == '') { $('#txtDescr' + indx).addClass('error'); _valid = false; } else { $('#txtDescr' + indx).removeClass('error'); }
           return _valid;
       };

       function SaveItemRefDetails(_nfieldval, callback, LINKID) {
           var slBSItemdet = [];
           for (var j = 0; j < _nfieldval.length; j++) {
               slBSItemdet.push(_nfieldval[j]);
           }
           var data2send = JSON.stringify({ slBSItemdet: slBSItemdet });
           Metronic.blockUI('#portlet_body');
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/SaveItemMapping",
               data: data2send,
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var res = Str(response.d);
                       if (res != "") {
                           toastr.success("Lighthouse eSolutions Pte. Ltd", "Item Ref Mapping Saved successfully.");
                           GetBSItemRefGrid(LINKID);
                       }
                       Metronic.unblockUI();
                   }
                   catch (err) {
                       toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Item Ref Mapping :' + err); Metronic.unblockUI();
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

       function DeleteItemRef(LINKID, REFID, callback) {
           Metronic.blockUI('#portlet_body');
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/DeleteItemMapping",
               data: "{ 'REFID':'" + REFID + "' }",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var res = Str(response.d);
                       if (res != "") {
                           toastr.success("Lighthouse eSolutions Pte. Ltd", "Item Ref Mapping Deleted.");
                           GetBSItemRefGrid(LINKID);
                       }
                       Metronic.unblockUI();
                   }
                   catch (err) {
                       toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete Item Ref Mapping :' + err); Metronic.unblockUI();
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

       function FillBSItemRefGrid(Table) {
           try {
               $('#dataGridBSItemRef').DataTable().clear().draw();
               if (Table.length != undefined && Table.length != null) {
                   var t = $('#dataGridBSItemRef').dataTable();
                   for (var i = 0; i < Table.length; i++) {
                       var cells = new Array();

                       var r = jQuery('<tr id=' + i + '>');
                       cells[0] = Str('');
                       cells[1] = Str(Table[i].REFTYPE);
                       cells[2] = Str(Table[i].BUYER_REF);
                       cells[3] = Str(Table[i].SUPPLIER_REF);
                       cells[4] = Str(Table[i].ITEM_DESC);
                       cells[5] = Str(Table[i].COMMENTS);
                       cells[6] = Str(Table[i].BUYER_ID);
                       cells[7] = Str(Table[i].SUPPLIER_ID);
                       cells[8] = Str(Table[i].REFID);
                       var ai = t.fnAddData(cells, false);
                   }
                   t.fnDraw();
               }
               else {
                   if (Table.REFID != undefined && Table.REFID != null) {
                       var t = $('#dataGridBSItemRef').dataTable();
                       var r = jQuery('<tr id=' + 1 + '>');
                       var cells = new Array();
                       cells[0] = Str('');
                       cells[1] = Str(Table.REFTYPE);
                       cells[2] = Str(Table.BUYER_REF);
                       cells[3] = Str(Table.SUPPLIER_REF);
                       cells[4] = Str(Table.ITEM_DESC);
                       cells[5] = Str(Table.COMMENTS);
                       cells[6] = Str(Table.BUYER_ID);
                       cells[7] = Str(Table.SUPPLIER_ID);
                       cells[8] = Str(Table.REFID);
                       var ai = t.fnAddData(cells, false);
                       t.fnDraw();
                   }
               }
           }
           catch (e)
           { }
       };

       var GetBSItemRefGrid = function (LINKID) {
           Metronic.blockUI('#portlet_body');
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/GetItemMapGrid",
               data: "{'LINKID':'" + LINKID + "'}",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var DataSet = JSON.parse(response.d);
                       if (DataSet.NewDataSet != null) {
                           Table = DataSet.NewDataSet.Table;
                           FillBSItemRefGrid(Table);
                       }
                       else $('#dataGridBSItemRef').DataTable().clear().draw();
                       Metronic.unblockUI();
                   }
                   catch (err) {
                       Metronic.unblockUI();
                       toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer/Supplier Item Ref :' + err);
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

       var setupBSItemTableHeader = function () {
           var dtfilter = '<th><div style="text-align:center;display:none;"><span><a style="color: #ffffff;"><u>New</u></<a></span> <span><a style="color: #ffffff;"><u>Upload</u></<a></span></div></div></th><th>Ref. Type</th><th>Buyer Ref.</th><th>Supplier Ref.</th>' +
               ' <th>Description</th><th>Comments</th><th>BUYER_ID</th><th>SUPPLIER_ID</th><th>REFID</th>';
           $('#tblHeadRowBSItemRef').empty();
           $('#tblHeadRowBSItemRef').append(dtfilter);
           $('#tblBodyBSItemRef').empty();
       };
       /* end */
           /* Item UOM*/
       function setBuyerSuppItemUOMGrid() {
           $('#prtItemUOM').show(); $('#prtItemRef').hide(); $('#prtBSLnkRules').hide(); setupBSItemUOMTableHeader();
           var table = $('#dataGridBSItemUOM');
            oBSItemUMTable = table.dataTable({
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
               "columnDefs": [{ 'orderable': false, "searching": true, "autoWidth": false, 'targets': [0] },
               { 'targets': [0], width: '50px' }, { 'targets': [1, 2], width: '120px' }, { 'targets': [3, 4, 5], visible: false }
               ],
               "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
               "sScrollY": '100px',
               "sScrollX": true,
               "aaSorting": [],
               "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); },
               "order": [[1, "asc"]],
               "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                   var divid = 'dvuom' + nRow._DT_RowIndex; var aedit = 'rwdedit' + nRow._DT_RowIndex; var adelete = 'rwdelete' + nRow._DT_RowIndex;
                   var detTag = ' <div id="' + divid + '" style="text-align:center;"><span id=' + aedit + ' class="actionbtn" data-toggle="tooltip" title="Edit"><i class="glyphicon glyphicon-pencil"></i></span>' +
                  '<span id=' + adelete + ' class="actionbtn" data-toggle="tooltip" title="Delete"><i class="glyphicon glyphicon-trash"></i></span></div>';
                   if (_isBSItemUomupdate != nRow._DT_RowIndex) { $('td:eq(0)', nRow).html(detTag); }
               }
           });

            $('#tblHeadRowBSItemUOM').addClass('gridHeader'); $('#ToolTables_dataGridBSItemUOM_0,#ToolTables_dataGridBSItemUOM_1').hide();
            $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
           BUYERID = Str(sessionStorage['BUYERID']); GetBSItemUOMGrid(SUPPLIERID, BUYERID);
       };

       function GetBSItemDetails(LINKID, SUPPLIERID, BUYERID) { GetBSItemRefGrid(LINKID); GetBSItemUOMGrid(SUPPLIERID, BUYERID); };

       function ValidateItemUOM(_bitemUOM, _sitemUOM) {
           var _valid = true;
           if (_bitemUOM == '') { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Enter Buyer Item UOM'); _valid = false; }
           if (_sitemUOM == '') { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Enter Supplier Item UOM'); _valid = false; }
           return _valid;
       };

       function SaveItemUOMDetails(_nfieldval, callback, SUPPLIERID, BUYERID) {
           var slBSUOMdet = [];
           for (var j = 0; j < _nfieldval.length; j++) {
               slBSUOMdet.push(_nfieldval[j]);
           }
           var data2send = JSON.stringify({ slBSUOMdet: slBSUOMdet });
           Metronic.blockUI('#portlet_body');
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/SaveItemUOMMappingDetails",
               data: data2send,
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var res = Str(response.d);
                       if (res != "") {
                           toastr.success("Lighthouse eSolutions Pte. Ltd", "Item UOM Mapping Saved successfully.");
                           GetBSItemUOMGrid(SUPPLIERID, BUYERID);
                       }
                       Metronic.unblockUI();
                   }
                   catch (err) {
                       toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Item UOM mapping :' + err); Metronic.unblockUI();
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

       function DeleteItemUOM(ITEM_UOM_MAPID, SUPPLIERID, BUYERID, callback) {
           Metronic.blockUI('#portlet_body');
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/DeleteItemUOMMapping",
               data: "{ 'ITEM_UOM_MAPID':'" + ITEM_UOM_MAPID + "' }",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var res = Str(response.d);
                       if (res != "") {
                           toastr.success("Lighthouse eSolutions Pte. Ltd", " Item UOM mapping Deleted.");
                           GetBSItemUOMGrid(SUPPLIERID, BUYERID);
                       }
                       Metronic.unblockUI();
                   }
                   catch (err) {
                       toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete  Item UOM mapping :' + err); Metronic.unblockUI();
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

       var setupBSItemUOMTableHeader = function () {
           var dtfilter = '<th><div style="text-align:center;display:none;"><span><a style="color: #ffffff;"><u>New</u></<a></span> <span><a style="color: #ffffff;"><u>Upload</u></<a></span></div></th><th>Buyer Item UOM.</th><th>Supplier Item UOM.</th><th>BUYER_ID</th>' +
                 ' <th>SUPPLIER_ID</th><th>ITEM_UOM_MAPID</th>';
           $('#tblHeadRowBSItemUOM').empty();
           $('#tblHeadRowBSItemUOM').append(dtfilter);
           $('#tblBodyBSItemUOM').empty();
       };

       function FillBSItemUOMGrid(Table) {
           try {
               $('#dataGridBSItemUOM').DataTable().clear().draw();
               if (Table.length != undefined && Table.length != null) {
                   var t = $('#dataGridBSItemUOM').dataTable();
                   for (var i = 0; i < Table.length; i++) {
                       var cells = new Array();

                       var r = jQuery('<tr id=' + i + '>');
                       cells[0] = Str('');
                       cells[1] = Str(Table[i].BUYER_ITEM_UOM);
                       cells[2] = Str(Table[i].SUPPLIER_ITEM_UOM);
                       cells[3] = Str(Table[i].BUYER_ID);
                       cells[4] = Str(Table[i].SUPPLIER_ID);
                       cells[5] = Str(Table[i].ITEM_UOM_MAPID);
                       var ai = t.fnAddData(cells, false);
                   }
                   t.fnDraw();
               }
               else {
                   if (Table.ITEM_UOM_MAPID != undefined && Table.ITEM_UOM_MAPID != null) {
                       var t = $('#dataGridBSItemUOM').dataTable();
                       var r = jQuery('<tr id=' + 1 + '>');
                       var cells = new Array();
                       cells[0] = Str('');
                       cells[1] = Str(Table.BUYER_ITEM_UOM);
                       cells[2] = Str(Table.SUPPLIER_ITEM_UOM);
                       cells[3] = Str(Table.BUYER_ID);
                       cells[4] = Str(Table.SUPPLIER_ID);
                       cells[5] = Str(Table.ITEM_UOM_MAPID);
                       var ai = t.fnAddData(cells, false);
                       t.fnDraw();
                   }
               }
           }
           catch (e)
           { }
       };

       var GetBSItemUOMGrid = function (SUPPLIERID, BUYERID) {
           Metronic.blockUI('#portlet_body');
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/GetItemUOMMapGrid",
               data: "{ 'SUPPLIERID':'" + SUPPLIERID + "','BUYERID':'" + BUYERID + "' }",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var DataSet = JSON.parse(response.d);
                       if (DataSet.NewDataSet != null) {
                           Table = DataSet.NewDataSet.Table;
                           FillBSItemUOMGrid(Table);
                       }
                       else $('#dataGridBSItemUOM').DataTable().clear().draw();
                       Metronic.unblockUI();
                   }
                   catch (err) {
                       Metronic.unblockUI();
                       toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyer/Supplier Item UOM :' + err);
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
       /* end */
       /*add new Buyer-Supplier Link */
       function AddBuyerDetails(SUPPLIERID, BUYERID, callback) {
           Metronic.blockUI('#portlet_body');
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/AddBuyer",
               data: "{ 'SUPPLIERID':'" + SUPPLIERID + "','BUYERID':'" + BUYERID + "' }",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var res = Str(response.d);
                       if (res != "") { toastr.success("Lighthouse eSolutions Pte. Ltd", "Buyer Added successfully.");GetBuyerSuppLinkGrid(SUPPLIERID);}
                       Metronic.unblockUI();
                   }
                   catch (err) {  toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to add Buyer :' + err); Metronic.unblockUI();}
               },
               failure: function (response) {  toastr.error("failure get", response.d); Metronic.unblockUI();},
               error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI();}
           });
       };

       function FillBuyerGrid(Table) {
           try {
               $('#dataGridNewBuy').DataTable().clear().draw();
               if (Table.length != undefined && Table.length != null) {
                   var t = $('#dataGridNewBuy').dataTable();
                   for (var i = 0; i < Table.length; i++) {
                       var cells = new Array();
                       var r = jQuery('<tr id=' + i + '>');
                       cells[0] = Str(Table[i].ADDR_CODE);
                       cells[1] = Str(Table[i].ADDR_NAME);
                       cells[2] = Str(Table[i].ADDRESSID);
                       var ai = t.fnAddData(cells, false);
                   }
                   t.fnDraw();
               }
               else {
                   if (Table.ITEM_UOM_MAPID != undefined && Table.ITEM_UOM_MAPID != null) {
                       var t = $('#dataGridNewBuy').dataTable();
                       var r = jQuery('<tr id=' + 1 + '>');
                       var cells = new Array();
                       cells[0] = Str(Table.ADDR_CODE);
                       cells[1] = Str(Table.ADDR_NAME);
                       cells[2] = Str(Table.ADDRESSID);
                       var ai = t.fnAddData(cells, false);
                       t.fnDraw();
                   }
               }
           }
           catch (e)
           { }
       };

       var GetBuyerGrid = function () {
           Metronic.blockUI('#portlet_body');
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/GetAllBuyers",
               data: '{}',
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var DataSet = JSON.parse(response.d);
                       if (DataSet.NewDataSet != null) {
                           Table = DataSet.NewDataSet.Table;
                           FillBuyerGrid(Table);
                       }
                       else $('#dataGridNewBuy').DataTable().clear().draw();
                       Metronic.unblockUI();
                   }
                   catch (err) {
                       Metronic.unblockUI();
                       toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Buyers :' + err);
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

       var setupBuyerTableHeader = function () {
           var dtfilter = '<th>Buyer Code</th><th>Buyer Name</th><th>ADDRESSID</th>';
           $('#tblHeadRowNewBuy').empty(); $('#tblHeadRowNewBuy').append(dtfilter); $('#tblBodyNewBuy').empty();
       };
       /* end*/

        /* Supplier Default Rules*/
       function setNewDefaultRules_Grid() {
           setupRulesTableHeader();
           var table6 = $('#dataGridNewDRule');
            oNDRTable = table6.dataTable({
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
               { 'targets': [4], width: '160px' }, { 'targets': [5], width: '200px' }, { 'targets': [6], width: '55px' },
               { 'targets': [7], visible: false },
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
                   var _cbdiv = '<input type="text" class="form-control" id="' + _id + '" value="' + _defruleval + '"/>'; $('td:eq(6)', nRow).html(_cbdiv);
               }
           });
           $('#tblHeadRowNewDRule').addClass('gridHeader'); $('#ToolTables_dataGridNewDRule_0,#ToolTables_dataGridNewDRule_1').hide(); $('#dataGridNewDRule_paginate').hide();
           $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%"); $('#dataGridNewDRule').css('width', '100%');
       };

       function GetSupplierRules(SUPPLIERID) {
           $('#prtDetail').hide(); $('#prtSuppRules').show(); $('#prtByLnk').hide(); $('#prtSuppConfig').hide(); setupSRulesTableHeader();
           var tableR = $('#dataGridSRules');
            oSRTable = tableR.dataTable({
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
               { 'targets': [0], width: '20px' }, { 'targets': [1, 6], width: '60px' },
               { 'targets': [2], width: '50px' }, { 'targets': [3], width: '80px' }, { 'targets': [4, 5], width: '120px' },
               { 'targets': [7, 8], visible: false },
               ],
               "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
               "scrollY": '300px',
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

           $('#tblHeadRowSRule').addClass('gridHeader'); $('#ToolTables_dataGridSRules_0,#ToolTables_dataGridSRules_1').hide();
           $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%"); $('#dataGridSRules').css('width', '100%');
           var GROUP_FORMAT = GetSupplierFormat(SUPPLIERID); GetSRulesGrid(SUPPLIERID, GROUP_FORMAT);
       };

       var setupSRulesTableHeader = function () {
           var dtfilter = '<th></th><th>Rule No.</th><th>Doc Type</th><th>Rule Code</th><th>Description</th><th>Comments</th><th>Rule Value</th><th>RULE_ID</th><th>DEF_ID</th>';
           $('#tblHeadRowSRule').empty(); $('#tblHeadRowSRule').append(dtfilter); $('#tblBodySRule').empty();
       };

       function SetSupplierRules() { setNewDefaultRules_Grid();var GROUP_FORMAT = GetSupplierFormat(SUPPLIERID); GetRulesGrid(SUPPLIERID, GROUP_FORMAT); $('#ModalDefRule').modal('show'); };

       function fnDefRuleDetails() {
           var sOut = ''; var _rulecode = ''; var _ruleval = ''; var _descr = ''; var _comment = ''; _lstRuleval = []; var _txtdisabled = 'disabled';
           var tid = "DefruleTable"; var _tbodyid = "tblBodyDefRule"; var _descid = 'txtDescription'; var _cmtid = 'txtComments';
           var _rulecdid = 'cbRuleCode'; var _rulevalid = 'cbRuleValue';
           FillRules(); _lstRuleval.push("0|0"); _lstRuleval.push("1|1");
           _rulecode = FillCombo('', _lstRules); _ruleval = FillCombo('', _lstRuleval);
           var sOut = '<div class="row"><div class="col-md-12"><div class="form-group">' +
                     ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Rule Code </label> </div>' +
                     ' <div  class="col-md-4"><select class="bs-select form-control" id="' + _rulecdid + '">' + _rulecode + '</select></div>' +
                     ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Rule Value </label> </div>' +
                     ' <div  class="col-md-4"><select class="bs-select form-control" id="' + _rulevalid + '">' + _ruleval + '</select> </div>' +
                     ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +              
                     ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Description </label> </div>' +
                     ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" ' + _txtdisabled + '  rows="6" id="' + _descid + '">' + _descr + '</textarea> </div>' +
                     ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Comments </label> </div>' +
                     ' <div  class="col-md-4"><textarea style="width:100%;border:1px solid #c2cad8;" ' + _txtdisabled + '  rows="6" id="' + _cmtid + '">' + _comment + '</textarea></div>' +
                     ' </div></div></div>';
           return sOut;
       };

       var GetSRulesGrid = function (SUPPLIERID, GROUP_FORMAT) {
           Metronic.blockUI('#portlet_body');
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/GetSupplierDefaultRules",
               data: "{'SUPPLIERID':'" + SUPPLIERID + "','GROUP_FORMAT':'" + GROUP_FORMAT + "' }",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var DataSet = JSON.parse(response.d);
                       if (DataSet.NewDataSet != null) {
                           Table = DataSet.NewDataSet.Table; FillSRulesGrid(Table);
                       }
                       else $('#dataGridSRules').DataTable().clear().draw();
                       Metronic.unblockUI();
                   }
                   catch (err) {
                       Metronic.unblockUI();
                       toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Supplier Rules :' + err);
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
                               else {
                                   if (Table.RULEID != undefined) { _lstRules.push(Str(Table.RULEID) + "|" + Str(Table.RULE_CODE)); }
                               }
                           }
                       }
                   }
                   catch (err) { toastr.error('Error in Fill Combo List :' + err, "Lighthouse eSolutions Pte. Ltd"); }
               },
               failure: function (response) { toastr.error("Failure in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); },
               error: function (response) { toastr.error("Error in Fill Combo List", "Lighthouse eSolutions Pte. Ltd"); }
           });
       };

       function GetDefRulesdet(RULEID)
       {
           $.ajax({
               type: "POST",
               async: false,
               url: "SupplierDetail.aspx/GetRuleDetails",
               data: "{'RULEID':'" + RULEID + "'}",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var _res = Str(response.d); _lstRuledetails = [];_lstRuledetails = _res.split('|');
                   }
                   catch (err) {
                       toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get Rules details:' + err);
                   }
               },
               failure: function (response) {
                   toastr.error("failure get", response.d); 
               },
               error: function (response) {
                   toastr.error("error get", response.responseText); 
               }
           });
       };

       function FillSRulesGrid(Table) {
           try {
               $('#dataGridSRules').DataTable().clear().draw();
               if (Table.length != undefined && Table.length != null) {
                   var t = $('#dataGridSRules').dataTable();
                   for (var i = 0; i < Table.length; i++) {
                       var cells = new Array();
                       var r = jQuery('<tr id=' + i + '>');
                       var _arrcommt = Str(Table[i].RULE_COMMENTS).split(',').join('<br/>');
                       cells[0] = Str('');
                       cells[1] = Str(Table[i].RULE_NUMBER);
                       cells[2] = Str(Table[i].DOC_TYPE);
                       cells[3] = Str(Table[i].RULE_CODE);
                       cells[4] = Str(Table[i].RULE_DESC);
                       cells[5] = _arrcommt;
                       cells[6] = Str(Table[i].RULE_VALUE);
                       cells[7] = Str(Table[i].RULE_ID);
                       cells[8] = Str(Table[i].DEF_ID);
                       var ai = t.fnAddData(cells, false);
                   }
                   t.fnDraw();
               }
               else {
                   if (Table.DEF_ID != undefined && Table.DEF_ID != null) {
                       var t = $('#dataGridSRules').dataTable();
                       var r = jQuery('<tr id=' + 1 + '>');
                       var cells = new Array();
                       var _arrcommt = Str(Table.RULE_COMMENTS).split(',').join('<br/>');
                       cells[0] = Str('');
                       cells[1] = Str(Table.RULE_NUMBER);
                       cells[2] = Str(Table.DOC_TYPE);
                       cells[3] = Str(Table.RULE_CODE);
                       cells[4] = Str(Table.RULE_DESC);
                       cells[5] = _arrcommt;
                       cells[6] = Str(Table.RULE_VALUE);
                       cells[7] = Str(Table.RULE_ID);
                       cells[8] = Str(Table.DEF_ID);
                       var ai = t.fnAddData(cells, false);
                       t.fnDraw();
                   }
               }
           }
           catch (e)
           { }
       };

       function ValidateDefRule(_rulecode, _ruleval, _lstMasterDet,nNew) {
           var _valid = true;
           if (_rulecode == '') { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Rule Code is blank'); _valid = false; }
           if (_ruleval == '') { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Rule Value is blank'); _valid = false; }
           if (nNew) { var _existrec = CheckExistingRule(_lstMasterDet); if (_existrec != '') { toastr.error("Lighthouse eSolutions Pte. Ltd.", _existrec); _valid = false; } }
           return _valid;
       };

       function SaveDefRule(_nfieldval, callback, _defaddrid, _defgrpformat) {
           var slRuledet = []; var _addressid = ''; var _grpFormat = '';
           for (var j = 0; j < _nfieldval.length; j++) { slRuledet.push(_nfieldval[j]); }
           var data2send = JSON.stringify({ slRuledet: slRuledet });
           Metronic.blockUI('#portlet_body');
           $.ajax({
               type: "POST",
               async: false,
               url: "DefaultRules.aspx/SaveRuleDetails",
               data: data2send,
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   try {
                       var res = Str(response.d);
                       if (res != "") {
                           toastr.success("Lighthouse eSolutions Pte. Ltd", "Rule Saved successfully.");
                           GetSupplierRules(_defaddrid);
                       }
                       Metronic.unblockUI();
                   }
                   catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to update Rule details :' + err); Metronic.unblockUI(); }
               },
               failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
               error: function (response) {
                   toastr.error("error get", response.responseText); Metronic.unblockUI();
               }
           });
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
                       if (res != "") {
                           toastr.success("Lighthouse eSolutions Pte. Ltd", "Rule Deleted.");
                           GetSupplierRules(ADDRESSID);
                       }
                       Metronic.unblockUI();
                   }
                   catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Unable to delete Rule :' + err); Metronic.unblockUI(); }
               },
               failure: function (response) { toastr.error("failure get", response.d); Metronic.unblockUI(); },
               error: function (response) { toastr.error("error get", response.responseText); Metronic.unblockUI(); }
           });
       };

       function UpdateSupplierDefRules(_defid, _defaddrid, _defgrpformat,_lstMasterDet,nNew) {
           var _hidvalue = '0'; _lstMasterDet = [];
           var _ruleid = $('#cbRuleCode option:selected').val(); var _rulecode_txt = $('#cbRuleCode option:selected').text(); var _ruleval = $('#cbRuleValue option:selected').val();
           _lstMasterDet.push('RULE_CODE' + "|" + Str(_rulecode_txt)); _lstMasterDet.push('RULEID' + "|" + Str(_ruleid)); _lstMasterDet.push('RULE_VALUE' + "|" + Str(_ruleval));
           _lstMasterDet.push('DEFAULT_RULE_ADDRESSID' + "|" + _defaddrid); _lstMasterDet.push('DEFAULT_RULE_GROUP_FORMAT' + "|" + _defgrpformat);
           var _res = ValidateDefRule(_rulecode_txt, _ruleval, _lstMasterDet, nNew);
           if (_res == true) {
               var _msg = 'Are you sure you want to override this rule for all the link for the Buyer/Supplier having inherited (Yes) ?';
               if (confirm(_msg)) { _hidvalue = '1'; } else { _hidvalue = '0'; }
               _lstMasterDet.push('HID_VALUE' + "|" + _hidvalue); _lstMasterDet.push('DEF_ID' + "|" + _defid);
               _isRuleupdate = -2; SaveDefRule(_lstMasterDet, GetSupplierRules, _defaddrid, _defgrpformat);
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
                       var cells = new Array();
                       var r = jQuery('<tr id=' + i + '>');
                       var _arrcommt = Str(Table[i].RULE_COMMENTS).split(',').join('<br/>');
                       cells[0] = Str('');
                       cells[1] = Str(Table[i].RULE_NUMBER);
                       cells[2] = Str(Table[i].DOC_TYPE);
                       cells[3] = Str(Table[i].RULE_CODE);
                       cells[4] = Str(Table[i].RULE_DESC);
                       cells[5] = _arrcommt;
                       cells[6] = Str(Table[i].RULE_VALUE);
                       cells[7] = Str(Table[i].RULEID);
                       var ai = t.fnAddData(cells, false);
                   }
                   t.fnDraw();
               }
               else {
                   if (Table.RULEID != undefined && Table.RULEID != null) {
                       var t = $('#dataGridNewDRule').dataTable();
                       var r = jQuery('<tr id=' + 1 + '>');
                       var cells = new Array();
                       var _arrcommt = Str(Table.RULE_COMMENTS).split(',').join('<br/>');
                       cells[0] = Str('');
                       cells[1] = Str(Table.RULE_NUMBER);
                       cells[2] = Str(Table.DOC_TYPE);
                       cells[3] = Str(Table.RULE_CODE);
                       cells[4] = Str(Table.RULE_DESC);
                       cells[5] = _arrcommt;
                       cells[6] = Str(Table.RULE_VALUE);
                       cells[7] = Str(Table.RULEID);
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
                       if (DataSet.NewDataSet != null) {
                           Table = DataSet.NewDataSet.Table; FillRulesGrid(Table);
                       }
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
           $('#tblHeadRowNewDRule').empty(); $('#tblHeadRowNewDRule').append(dtfilter); $('#tblBodyNewDRule').empty();
       };
       /*end*/

       /*Supplier Mapping*/

       var setupSXLSMappingTableHeader = function () {
           $('#tblHeadRowSXlsMapp').empty(); $('#tblBodySXlsMapp').empty();
           var dtfilter = '<th style="text-align:center;">#</th><th>Map Code</th><th>Buyer Code</th><th>Buyer Name</th><th>Supplier Code</th><th>Supplier Name</th><th>Cell-1</th><th>Cell-1 Value-1</th><th>Cell-1 Value-2</th><th>Cell-2</th><th>Cell-2 Value</th><th>Cell (No Discount)</th> <th>Cell Value (No Discount)</th> ' +
               ' <th>XLS_BUYER_MAPID</th><th>EXCEL_MAPID</th><th>BUYER_SUPP_LINKID</th><th>BUYERID</th><th>SUPPLIERID</th> <th>GROUP_ID</th><th>DOC_TYPE</th>';
           $('#tblHeadRowSXlsMapp').append(dtfilter);
       };

       function GetSupplierExcelMapp(SUPPLIERID) {
           setAccessPortlet('3'); setupSXLSMappingTableHeader();
           var table12 = $('#dataGridSXlsMapp');
            oSXlsMappTable = table12.dataTable({
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
               { 'targets': [1], width: '70px' }, { 'targets': [2], width: '65px' }, { 'targets': [3, 7], width: '120px' },
               { 'targets': [6, 9], width: '30px' }, { 'targets': [8], width: '60px' }, { 'targets': [10], width: '60px' },
               { 'targets': [0, 4, 5, 11, 12, 13, 14, 15, 16, 17, 18, 19], visible: false }
               ],
               "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
               "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); }
           });

            $('#tblHeadRowSXlsMapp').addClass('gridHeader'); $('#ToolTables_dataGridSXlsMapp_0,#ToolTables_dataGridSXlsMapp_1').hide(); $('#dataGridSXlsMapp').css('width', '100%');
            $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%");
            GetMappingXLSGrid(SUPPLIERID, 'SUPPLIER');
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
                       DataSet = JSON.parse(response.d); if (DataSet.NewDataSet != null) { var Table = DataSet.NewDataSet.Table; FillSMappingXLSGrid(Table); }
                   }
                   catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get XLS BuyerConfig data :' + err); }
               },
               failure: function (response) { toastr.error("failure get", response.d); },
               error: function (response) { toastr.error("error get", response.responseText); }
           });
       };

       function FillSMappingXLSGrid(Table) {
           try {
               $('#dataGridSXlsMapp').DataTable().clear().draw();
               if (Table.length != undefined && Table.length != null) {
                   var t = $('#dataGridSXlsMapp').dataTable();
                   for (var i = 0; i < Table.length; i++) {
                       var cells = new Array();
                       var r = jQuery('<tr id=' + i + '>');
                       cells[0] = Str('');
                       cells[1] = Str(Table[i].FORMAT_MAP_CODE);
                       cells[2] = Str(Table[i].BUYER_CODE);
                       cells[3] = Str(Table[i].BUYER_NAME);
                       cells[4] = Str(Table[i].SUPPLIER_CODE);
                       cells[5] = Str(Table[i].SUPPLIER_NAME);
                       cells[6] = Str(Table[i].MAP_CELL1);
                       cells[7] = Str(Table[i].MAP_CELL1_VAL1);
                       cells[8] = Str(Table[i].MAP_CELL1_VAL2);
                       cells[9] = Str(Table[i].MAP_CELL2);
                       cells[10] = Str(Table[i].MAP_CELL2_VAL);
                       cells[11] = Str(Table[i].MAP_CELL_NODISC);
                       cells[12] = Str(Table[i].MAP_CELL_NODISC_VAL);
                       cells[13] = Str(Table[i].XLS_BUYER_MAPID);
                       cells[14] = Str(Table[i].EXCEL_MAPID);
                       cells[15] = Str(Table[i].BUYER_SUPP_LINKID);
                       cells[16] = Str(Table[i].BUYERID)
                       cells[17] = Str(Table[i].SUPPLIERID)
                       cells[18] = Str(Table[i].GROUP_ID)
                       cells[19] = Str(Table[i].DOC_TYPE)
                       var ai = t.fnAddData(cells, false);
                   }
                   t.fnDraw();
               }
               else {
                   if (Table.XLS_BUYER_MAPID != undefined && Table.XLS_BUYER_MAPID != null) {
                       var t = $('#dataGridSXlsMapp').dataTable();
                       var r = jQuery('<tr id=' + 1 + '>');
                       var cells = new Array();
                       cells[0] = Str('');
                       cells[1]= Str(Table.FORMAT_MAP_CODE);
                       cells[2] = Str(Table.BUYER_CODE);
                       cells[3] = Str(Table.BUYER_NAME);
                       cells[4] = Str(Table.SUPPLIER_CODE);
                       cells[5] = Str(Table.SUPPLIER_NAME);
                       cells[6] = Str(Table.MAP_CELL1);
                       cells[7] = Str(Table.MAP_CELL1_VAL1);
                       cells[8] = Str(Table.MAP_CELL1_VAL2);
                       cells[9] = Str(Table.MAP_CELL2);
                       cells[10] = Str(Table.MAP_CELL2_VAL);
                       cells[11] = Str(Table.MAP_CELL_NODISC);
                       cells[12] = Str(Table.MAP_CELL_NODISC_VAL);
                       cells[13] = Str(Table.XLS_BUYER_MAPID);
                       cells[14] = Str(Table.EXCEL_MAPID);
                       cells[15] = Str(Table.BUYER_SUPP_LINKID);
                       cells[16] = Str(Table.BUYERID)
                       cells[17] = Str(Table.SUPPLIERID)
                       cells[18] = Str(Table.GROUP_ID)
                       cells[19] = Str(Table.DOC_TYPE)
                       var ai = t.fnAddData(cells, false);
                       t.fnDraw();
                   }
               }
           }
           catch (e) { }
       }

       var setupSPDFMappingTableHeader = function () {
           $('#tblHeadRowSPdfMapp').empty(); $('#tblBodySPdfMapp').empty();
           var dtfilter = '<th style="text-align:center;">#</th><th>Map Code</th><th>Doc Type</th><th>Buyer Code</th><th>Buyer Name</th><th>Supplier Code</th><th>Supplier Name</th> <th>Map Range (1)</th><th>Map Range (1) Value</th><th>Map Range (2)</th><th>Map Range (2) Value</th><th>Map Range (3)</th><th>Map Range (3) Value</th> ' +
               ' <th>MAP_ID</th><th>PDF_MAPID</th><th>BUYER_SUPP_LINKID</th><th>BUYERID</th><th>SUPPLIERID</th> <th>GROUP_ID</th>';
           $('#tblHeadRowSPdfMapp').append(dtfilter);
       };

       function GetSupplierPDFMapp(SUPPLIERID) {
           setAccessPortlet('3');
           setupSPDFMappingTableHeader();
           var table13 = $('#dataGridSPdfMapp');
            oSPdfMappTable = table13.dataTable({
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
               { 'targets': [1], width: '70px' }, { 'targets': [2], width: '50px' }, { 'targets': [4], width: '120px' }, { 'targets': [3], width: '58px' }, { 'targets': [7, 9, 11], width: '60px' },
               { 'targets': [8, 10, 12], width: '70px' }, { 'targets': [0, 5, 6, 13, 14, 15, 16, 17, 18], visible: false }
               ],
               "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
               "drawCallback": function (settings, json) { $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' }); }
           });

           $('#tblHeadRowSPdfMapp').addClass('gridHeader'); $('#ToolTables_dataGridSPdfMapp_0,#ToolTables_dataGridSPdfMapp_1').hide();
           $(".dataTables_scrollHeadInner,.dataTables_scrollHeadInner table,.dataTables_scrollBody table").css('min-width', "100%"); $('#dataGridSPdfMapp').css('width', '100%');
           GetMappingPDFGrid(SUPPLIERID, 'SUPPLIER'); 
       };

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
                   try { DataSet = JSON.parse(response.d); if (DataSet.NewDataSet != null) { var Table = DataSet.NewDataSet.Table; FillSMappingPDFGrid(Table); } }
                   catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Get PDF BuyerConfig data :' + err); }
               },
               failure: function (response) { toastr.error("failure get", response.d); },
               error: function (response) { toastr.error("error get", response.responseText); }
           });
       };

       function FillSMappingPDFGrid(Table) {
           try {
               $('#dataGridSPdfMapp').DataTable().clear().draw();
               if (Table.length != undefined && Table.length != null) {
                   var t = $('#dataGridSPdfMapp').dataTable();
                   for (var i = 0; i < Table.length; i++) {
                       var cells = new Array();
                       var r = jQuery('<tr id=' + i + '>');
                       cells[0] = Str('');
                       cells[1]= Str(Table[i].FORMAT_MAP_CODE);
                       cells[2] = Str(Table[i].DOC_TYPE);
                       cells[3] = Str(Table[i].BUYER_CODE);
                       cells[4] = Str(Table[i].BUYER_NAME);
                       cells[5] = Str(Table[i].SUPPLIER_CODE);
                       cells[6] = Str(Table[i].SUPPLIER_NAME);
                       cells[7] = Str(Table[i].MAPPING_1);
                       cells[8] = Str(Table[i].MAPPING_1_VALUE);
                       cells[9] = Str(Table[i].MAPPING_2);
                       cells[10] = Str(Table[i].MAPPING_2_VALUE);
                       cells[11] = Str(Table[i].MAPPING_3);
                       cells[12] = Str(Table[i].MAPPING_3_VALUE);
                       cells[13] = Str(Table[i].MAP_ID);
                       cells[14] = Str(Table[i].PDF_MAPID);
                       cells[15] = Str(Table[i].BUYER_SUPP_LINKID);
                       cells[16] = Str(Table[i].BUYERID)
                       cells[17] = Str(Table[i].SUPPLIERID)
                       cells[18] = Str(Table[i].GROUP_ID)
                       var ai = t.fnAddData(cells, false);
                   }
                   t.fnDraw();
               }
               else {
                   if (Table.PDF_MAPID != undefined && Table.PDF_MAPID != null) {
                       var t = $('#dataGridSPdfMapp').dataTable();
                       var r = jQuery('<tr id=' + 1 + '>');
                       var cells = new Array();
                       cells[0] = Str('');
                       cells[1]= Str(Table.FORMAT_MAP_CODE);
                       cells[2] = Str(Table.DOC_TYPE);
                       cells[3] = Str(Table.BUYER_CODE);
                       cells[4] = Str(Table.BUYER_NAME);
                       cells[5] = Str(Table.SUPPLIER_CODE);
                       cells[6] = Str(Table.SUPPLIER_NAME);
                       cells[7] = Str(Table.MAPPING_1);
                       cells[8] = Str(Table.MAPPING_1_VALUE);
                       cells[9] = Str(Table.MAPPING_2);
                       cells[10] = Str(Table.MAPPING_2_VALUE);
                       cells[11] = Str(Table.MAPPING_3);
                       cells[12] = Str(Table.MAPPING_3_VALUE);
                       cells[13] = Str(Table.MAP_ID);
                       cells[14] = Str(Table.PDF_MAPID);
                       cells[15] = Str(Table.BUYER_SUPP_LINKID);
                       cells[16] = Str(Table.BUYERID)
                       cells[17] = Str(Table.SUPPLIERID)
                       cells[18] = Str(Table.GROUP_ID)
                       var ai = t.fnAddData(cells, false);
                       t.fnDraw();
                   }
               }
           }
           catch (e) { }
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
                               _lstserverdet.push(Str(_servname));
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
           var data2send = JSON.stringify({ ID: ID, slServdet: slServdet, slServpaths: slServpaths ,cInsertdet:'2'});
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

   return { init: function () { handleSupplierDetailTable(); } };}();

   function onClientUploadStart(sender, e) {
       var FILENAME = e._fileName;
       $.ajax({
           type: "POST",
           async: false,
           url: "SupplierDetail.aspx/SetUploadPath",
           data: "{'FILENAME':'" + FILENAME + "'}",
           contentType: "application/json; charset=utf-8",
           dataType: "json",
           success: function (response) {
               try {
                   result = response.d;
               }
               catch (err) {
                   toastr.error('Error in Starting Client Upload :' + err, "Lighthouse eSolutions Pte. Ltd");
               }
           },
           failure: function (response) {
               toastr.error("Failure in Starting Client Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd");
               result = -1;
           },
           error: function (response) {
               toastr.error("Error in Starting Client Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd");
               result = -1;
           }
       });
   }

   function onClientUploadComplete(sender, e) {
       var FILENAME = e._fileName;
       var BUYERID = Str(sessionStorage.getItem('BUYERID'));
       var SUPPLIERID = Str(sessionStorage.getItem('SUPPLIERID'));
       var LINKID = Str(sessionStorage.getItem('LINKID'));
       var TYPE = Str($('#btnDownload').val());
       var DELEXISTS = ($('#cbdelExist').is(':checked')) ? Str(true) : Str(false);

       $.ajax({
           type: "POST",
           async: false,
           url: "SupplierDetail.aspx/UploadFileMapping",
           data: "{'FILENAME':'" + FILENAME + "','LINKID':'" + LINKID + "','SUPPLIERID':'" + SUPPLIERID + "','BUYERID':'" + BUYERID + "','DELEXISTS':'" + DELEXISTS + "','TYPE':'" + TYPE + "'}",
           contentType: "application/json; charset=utf-8",
           dataType: "json",
           success: function (response) {
               try {
                   var  result = Str(response.d);
                   if (result.search('uploaded') >= 0) {
                       toastr.success(result, "Lighthouse eSolutions Pte. Ltd");              
                   }
                   else { toastr.error('Error in File Upload :' + result, "Lighthouse eSolutions Pte. Ltd"); }

                   jQuery('.ajax__fileupload_fileItemInfo').remove();
                   $('.ajax__fileupload_topFileStatus').text('Please select file(s) to upload');
                   $('.ajax__fileupload_queueContainer').fadeOut(); $('#cbdelExist').prop('checked',false);
               }
               catch (err) {
                   toastr.error('Error in File Upload :' + err, "Lighthouse eSolutions Pte. Ltd");
               }
           },
           failure: function (response) {
               toastr.error("Failure in File Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd");
               result = -1;
           },
           error: function (response) {
               toastr.error("Error in File Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd");
               result = -1;
           }
       });

   }



