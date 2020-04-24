//{ 'targets': [0], width: '6%', 'bSortable': false }, { 'targets': [2], width: '5%', },
//{ 'targets': [3], width: '20%' },  { 'targets': [4], width: '10%' }, { 'targets': [5], width: '10%' }, { 'targets': [6,8], width: '10%' },      
//{ 'targets': [7], width: '10%' },
//{ 'targets': [9,10], width: '25%' },
//{ 'targets': [1, 11], visible: false }, { 'targets': [4, 5, 6, 7, 8], "sClass": "visible-lg" }


/* supplier */

//oBSLTable.on('click', 'tbody td', function (e) {
//    if (e.target.type == "checkbox") { } else { e.preventDefault(); }
//    var bslselectedTr = $(this).parents('tr')[0]; var aData = oBSLTable.fnGetData(bslselectedTr);
//    if (aData != null) { var _linkid = aData[47]; sessionStorage['LINKID'] = Str(_linkid); }
//    var cellindx = $(this).index(); updtcnt = 0;
//    if (oBSLTable.fnIsOpen(bslselectedTr) && (e.target.innerText == 'Edit')) {
//        oBSLTable.fnClose(bslselectedTr);
//    }
//    else {
//        var divid = 'dvbs' + bslselectedTr._DT_RowIndex;
//        if (e.target.innerText == 'Edit') {
//            _isBSupupdate = bslselectedTr._DT_RowIndex;
//            if ((bspreviousTr != '') && (oBSLTable.fnIsOpen(bspreviousTr) && bspreviousTr != bslselectedTr)) {
//                var prevdivid = 'dvbs' + bspreviousTr._DT_RowIndex; oBSLTable.fnClose(bspreviousTr); $('#' + prevdivid).show();
//            }
//            if (aData != null) {
//                var _linkid = aData[47]; sessionStorage['LINKID'] = Str(_linkid);
//                oBSLTable.fnOpen(bslselectedTr, fnBSFormatDetails(oBSLTable, bslselectedTr), 'details'); // $('#' + divid).hide(); bspreviousTr = bslselectedTr;
//            }
//        }
//        if (e.target.innerText == 'Delete') {
//            if (confirm('Are you sure? You want to delete this Buyer-Supplier Link ?')) {
//                var _linkid = aData[47]; var _suppid = Str(SUPPLIERID);
//                DeleteBSLink(_linkid, _suppid, GetBuyerSuppLinkGrid);
//            }
//        }
//        if (e.target.className == 'opentd' || e.target.className == 'fa fa-search') {
//            var _linkid = aData[47]; var _buyerid = aData[46]; sessionStorage['BUYERID'] = Str(_buyerid); var _suppid = Str(SUPPLIERID);
//            setBuyerSuppItemRefGrid(_linkid); $('#hItmTabs .active').removeClass('active'); $('#lnkItemRef').addClass('active'); $("#ModalNewBSItemDet").modal('show');
//        }
//        if (e.target.innerText == 'Create' || e.target.innerText == 'View') {
//            $('#dvLoginInfo').empty(); var logindet = LoginDetails(bslselectedTr, bslselectedTr.rowIndex); $('#dvLoginInfo').append(logindet); $("#ModalLoginInfo").modal('show');
//        }

//        $('#btnBSUpdate').click(function () {
//            _lstBSLnkdet = [];
//            if (aData != null && updtcnt == 0) {
//                var _suppid = Str(SUPPLIERID); var _linkid = aData[47];
//                var _res = ValidateBSLink(bslselectedTr.rowIndex, _linkid);
//                if (_res == true) {
//                    _isBSupupdate = -2; GetBSLinkDetails(oBSLTable, bslselectedTr, _lstBSLnkdet, e.target.innerText);
//                    SaveBSupplierLinkDetails(_lstBSLnkdet, GetBuyerSuppLinkGrid, _linkid, _suppid);
//                }
//                $('#' + divid).show();
//            }
//        });

//        $('#btnBSCancel').click(function () {
//            if (oBSLTable.fnIsOpen(bslselectedTr)) { oBSLTable.fnClose(bslselectedTr); } $('#' + divid).show(); _isBSupupdate = -2;
//        });

//        $('#btnPWD').click(function () {
//            $('#dvChangePWD').empty(); $('#dvLoginInfo').css('display', 'none'); $('#dvChangePWD').css('display', 'block');
//            var logindet = ChangePassword(bslselectedTr.rowIndex); $('#dvLoginft').css('display', 'block'); $('#dvChangePWD').append(logindet);
//        });
//    }
//});

//function fnBSFormatDetails(oTable, nTr) {
//    var sOut = ''; var _str = '';
//    var _irfqchecked = ''; var _erfqchecked = ''; var _erfqackchecked = ''; var _iqtechecked = ''; var _eqtechecked = ''; var _ipochecked = ''; var _epochecked = '';
//    var _epoackchecked = ''; var _epocchecked = ''; var _nbchecked = ''; var _nschecked = ''; var _achecked = '';
//    var indx = nTr.rowIndex;
//    var aData = oTable.fnGetData(nTr);
//    var tid = "BSLinkTable" + indx;
//    var _tbodyid = "tblBodyBSLnk" + indx;

//    var _codeid = 'txtCode' + indx; var _bLnkcdid = 'txtBLinkCode' + indx; var _sLnkcdid = 'txtSLinkCode' + indx; var _suppSenCdid = 'txtSuppSenCd' + indx;
//    var _suppRecCdid = 'txtSuppRecCd' + indx; var _bSenCdid = 'txtBSenCd' + indx; var _brecvdid = 'txtBRecCd' + indx; var _bmapid = 'txtBMapping' + indx; var _smapid = 'txtSMapping' + indx;
//    var _bcontid = 'txtBContact' + indx; var _scontid = 'txtSContact' + indx; var _bemailid = 'txtBEmail' + indx; var _semailid = 'txtSEmail' + indx;
//    var _ccemailid = 'txtCCEmail' + indx; var _bccemailid = 'txtBCCEmail' + indx; var _mailsbid = 'txtMailsub' + indx;
//    var _replyid = 'txtReplyEmail' + indx; var _bimpid = 'txtBImpPath' + indx; var _bexpid = 'txtBExpPath' + indx; var _simpid = 'txtSImpPath' + indx;
//    var _sexpid = 'txtSExpPath' + indx; var _wurlid = 'txtWebserURL' + indx; var _dpriceid = 'txtDefPrice' + indx;
//    var _ufiletyid = 'txtUpFileType' + indx; var _irfqid = 'chkImpRFQ' + indx; var _erfqid = 'chkExpRFQ' + indx; var _erfqackid = 'chkExpRFQAck' + indx;
//    var _iqteid = 'chkImpQuote' + indx; var _eqteid = 'chkExpQuote' + indx; var _ipoid = 'chkImpPO' + indx; var _epoid = 'chkExpPO' + indx; var _epoackid = 'chkExpPOAck' + indx;
//    var _epocid = 'chkExpPOC' + indx; var _nbyid = 'chkNBuyer' + indx; var _nspid = 'chkNSupp' + indx; var _actid = 'chkAct' + indx; var _grpid = 'cbGrp' + indx;
//    var _loginid = 'btnCreate' + indx;
//    var _code = Str(aData[2]); var _BLinkCode = Str(aData[4]); var _SLinkCode = Str(aData[5]); var _SuppSenCd = Str(aData[6]); var _SuppRecCd = Str(aData[7]);
//    var _BSenCd = Str(aData[8]); var _BRecCd = Str(aData[9]); var _BMapping = Str(aData[10]); var _SMapping = Str(aData[11]); var _BContact = Str(aData[16]);
//    var _SContact = Str(aData[17]); var _BEmail = Str(aData[18]); var _SEmail = Str(aData[19]); var _CCEmail = Str(aData[20]); var _BCCEmail = Str(aData[21]);
//    var _Mailsub = Str(aData[22]); var _ReplyEmail = Str(aData[23]); var _BImpPath = Str(aData[24]); var _BExpPath = Str(aData[25]); var _SImpPath = Str(aData[26]);
//    var _SExpPath = Str(aData[27]); var _WebserURL = Str(aData[28]); var _DefPrice = Str(aData[29]); var _UpFileType = Str(aData[31]);
//    var _ImpRFQ = Str(aData[32]); if (_ImpRFQ == '1') { _irfqchecked = 'checked'; } else { _irfqchecked = ''; }
//    var _ExpRFQ = Str(aData[33]); if (_ExpRFQ == '1') { _erfqchecked = 'checked'; } else { _erfqchecked = ''; }
//    var _ExpRFQAck = Str(aData[34]); if (_ExpRFQAck == '1') { _erfqackchecked = 'checked'; } else { _erfqackchecked = ''; }
//    var _ImpQuote = Str(aData[35]); if (_ImpQuote == '1') { _iqtechecked = 'checked'; } else { _iqtechecked = ''; }
//    var _ExpQuote = Str(aData[36]); if (_ExpQuote == '1') { _eqtechecked = 'checked'; } else { _eqtechecked = ''; }
//    var _ImpPO = Str(aData[37]); if (_ImpPO == '1') { _ipochecked = 'checked'; } else { _ipochecked = ''; }
//    var _ExpPO = Str(aData[38]); if (_ExpPO == '1') { _epochecked = 'checked'; } else { _epochecked = ''; }
//    var _ExpPOAck = Str(aData[39]); if (_ExpPOAck == '1') { _epoackchecked = 'checked'; } else { _epoackchecked = ''; }
//    var _ExpPOC = Str(aData[40]); if (_ExpPOC == '1') { _epocchecked = 'checked'; } else { _epocchecked = ''; }
//    var _NBuyer = Str(aData[41]); if (_NBuyer == '1') { _nbchecked = 'checked'; } else { _nbchecked = ''; }
//    var _NSupp = Str(aData[42]); if (_NSupp == '1') { _nschecked = 'checked'; } else { _nschecked = ''; }
//    var _Act = Str(aData[43]); if (_Act == '1') { _achecked = 'checked'; } else { _achecked = ''; }
//    var _lnkid = Str(aData[47]); var _pwd = Str(aData[45]); var _lgname = ''; if (_pwd == '') { _lgname = 'Create'; } else { _lgname = 'View'; }
//    FillGroups(); var _grp = FillCombo(Str(aData[30]), _lstgrp);
//    var btndiv = '<div class="col-md-5" style="float:right;width:420px;padding-bottom:5px;"> <span><a href="#" id="btnBSUpdate"><u>Update</u></<a></span>  <span><a href="#" id="btnBSCancel"><u>Cancel</u></<a></span></div>';
//    //var sOut = '<div class="row" style="padding-right:63px;"><div class="col-md-12"><div class="row"><div class="col-md-12"><div class="form-group">' +
//    var sOut = '<div class="col-md-12"><div class="row"><div class="col-md-12"><div class="form-group">' +
//          ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Code </label> </div>' +
//          ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _codeid + '"  value="' + _code + '"/> </div>' +
//          ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Buyer Link Code </label> </div>' +
//            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bLnkcdid + '"  value="' + _BLinkCode + '"/> </div>' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Supplier Link Code </label> </div>' +
//            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _sLnkcdid + '"  value="' + _SLinkCode + '"/> </div>' +
//            ' </div></div></div><div class="row"><div class="col-md-12"> <div class="form-group">' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Supplier Sender Code </label> </div>' +
//            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _suppSenCdid + '"  value="' + _SuppSenCd + '"/> </div>' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Supplier Receiver Code </label> </div>' +
//            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _suppRecCdid + '"  value="' + _SuppRecCd + '"/> </div>' +
//            ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Buyer Sender Code </label> </div>' +
//            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bSenCdid + '"  value="' + _BSenCd + '"/> </div>' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Buyer Receiver Code </label> </div>' +
//            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _brecvdid + '"  value="' + _BRecCd + '"/> </div>' +
//            ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Buyer Mapping </label> </div>' +
//            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bmapid + '"  value="' + _BMapping + '"/> </div>' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Supplier Mapping </label> </div>' +
//            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _smapid + '"  value="' + _SMapping + '"/> </div>' +
//            ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Buyer Contact </label> </div>' +
//            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bcontid + '"  value="' + _BContact + '"/> </div>' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Supplier Contact </label> </div>' +
//            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _scontid + '"  value="' + _SContact + '"/> </div>' +
//            ' </div></div></div> <div class="row"><div class="col-md-12"> <div class="form-group">' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Buyer Email </label> </div>' +
//            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bemailid + '"  value="' + _BEmail + '"/> </div>' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Supplier Email </label> </div>' +
//            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _semailid + '"  value="' + _SEmail + '"/> </div>' +
//            ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> CC Email </label> </div>' +
//            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _ccemailid + '"  value="' + _CCEmail + '"/> </div>' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> BCC Email </label> </div>' +
//            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _bccemailid + '"  value="' + _BCCEmail + '"/> </div>' +
//            ' </div></div></div><div class="row"><div class="col-md-12"><div class="form-group">' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Mail Subject </label> </div>' +
//            ' <div  class="col-md-3"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _mailsbid + '">' + _Mailsub + '</textarea></div>' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Reply Email </label> </div>' +
//            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _replyid + '"  value="' + _ReplyEmail + '"/> </div>' +
//            ' </div></div></div> <div class="row"><div class="col-md-12"> <div class="form-group">' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Buyer Import Path </label> </div>' +
//            ' <div  class="col-md-3"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _bimpid + '">' + _BImpPath + '</textarea></div>' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Buyer Export Path </label> </div>' +
//            ' <div  class="col-md-3"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _bexpid + '">' + _BExpPath + '</textarea></div>' +
//            ' </div></div></div> <div class="row"><div class="col-md-12"><div class="form-group">' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Supplier Import Path </label> </div>' +
//            ' <div  class="col-md-3"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _simpid + '">' + _SImpPath + '</textarea></div>' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Supplier Export Path </label> </div>' +
//            ' <div  class="col-md-3"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _sexpid + '">' + _SExpPath + '</textarea></div>' +
//            ' </div></div></div><div class="row"><div class="col-md-12"> <div class="form-group">' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Webservice URL </label> </div>' +
//            ' <div  class="col-md-3"><textarea style="width:100%;border:1px solid #c2cad8;" rows="2" id="' + _wurlid + '">' + _WebserURL + '</textarea></div>' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Default Price </label> </div>' +
//            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _dpriceid + '"  value="' + _DefPrice + '"/> </div>' +
//            ' </div></div></div><div class="row"><div class="col-md-12"> <div class="form-group">' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Group </label> </div>' +
//            ' <div  class="col-md-3"><select class="bs-select form-control" id="' + _grpid + '">' + _grp + '</select></div>' +
//            ' <div class="col-md-2" style="text-align:right;margin-top:5px;"><label> Upload File Type </label> </div>' +
//            ' <div  class="col-md-3"><input type="text" class="form-control" id="' + _ufiletyid + '"  value="' + _UpFileType + '"/> </div>' +
//            ' </div></div></div><div class="row"><div class="col-md-12"><div class="col-md-2"></div>' +
//            ' <div class="col-md-8" style="border:1px solid #c2cad8">' +
//           ' <ul class="checkbox-grid">' +
//           ' <li><input class="widelarge" type="checkbox" id="' + _irfqid + '"  value="' + _ImpRFQ + '"' + _irfqchecked + ' /><label class="chklabel">Import RFQ</label></li>' +
//           ' <li><input class="widelarge" type="checkbox" id="' + _erfqid + '"  value="' + _ExpRFQ + '"' + _erfqchecked + ' /><label class="chklabel">Export RFQ</label></li>' +
//           ' <li><input class="widelarge" type="checkbox" id="' + _erfqackid + '"  value="' + _ExpRFQAck + '"' + _erfqackchecked + '/><label class="chklabel">Export RFQ Ack.</label></li>' +
//           ' <li><input class="widelarge" type="checkbox" id="' + _iqteid + '"  value="' + _ImpQuote + '"' + _iqtechecked + '/><label class="chklabel">Import Quote</label></li>' +
//           ' <li><input class="widelarge" type="checkbox" id="' + _eqteid + '"  value="' + _ExpQuote + '"' + _eqtechecked + '/><label class="chklabel">Export Quote</label></li>' +
//           ' <li><input class="widelarge" type="checkbox" id="' + _ipoid + '"  value="' + _ImpPO + '"' + _ipochecked + '/><label class="chklabel">Import PO</label></li>' +
//           ' <li> <input class="widelarge" type="checkbox" id="' + _epoid + '"  value="' + _ExpPO + '"' + _epochecked + '/><label class="chklabel">Export PO</label></li>' +
//           ' <li> <input class="widelarge" type="checkbox" id="' + _epoackid + '"  value="' + _ExpPOAck + '"' + _epoackchecked + '/> <label class="chklabel">Export PO Ack.</label></li>' +
//           ' <li> <input class="widelarge" type="checkbox" id="' + _epocid + '"  value="' + _ExpPOC + '"' + _epocchecked + '/><label class="chklabel">Export POC</label></li>' +
//           ' <li> <input class="widelarge" type="checkbox" id="' + _nbyid + '"  value="' + _NBuyer + '"' + _nbchecked + '/> <label class="chklabel">Notify Buyer</label></li>' +
//           ' <li> <input class="widelarge" type="checkbox" id="' + _nspid + '"  value="' + _NSupp + '"' + _nschecked + '/><label class="chklabel">Notify Supplier</label></li>' +
//           ' <li> <input class="widelarge" type="checkbox" id="' + _actid + '"  value="' + _Act + '"' + _achecked + '/><label class="chklabel">Active</label></li></ul>' +
//           ' </div> <div class="col-md-1"></div></div></div><div class="row"><div class="col-md-12"><div class="form-group" style="padding-top:10px;">' +
//           ' <div class="col-md-2" style="text-align:right;"><label> Login Info </label> </div>' +
//           ' <div  class="col-md-3"><button type="button" id="' + _loginid + '" class="btn default yellow-stripe" style="font-size:10px;">' + _lgname + '</button></div>' +
//            //btndiv + ' </div></div></div></div></div>';
//           ' </div></div></div></div>';
//    return sOut;
//};


//var LoginDetails = function (nTr, indx) {
//    var sOut = ''; var _str = ''; var _reftype = ''; var _buyerref = ''; var _suppref = ''; var _descr = ''; var _rmk = ''; var btndiv = '';
//    var tid = "LoginTable" + indx; var _tbodyid = "tbllogin" + indx; var _actid = 'chkAct' + indx; _lstlogindet = []; var _actcheckd = '';
//    if (nTr != '') {
//        var aData = oBSLTable.fnGetData(nTr); var _linkid = Str(sessionStorage.getItem('LINKID'));
//        var _suppid = 'txtSupp' + indx; var _userid = 'txtUser' + indx; var _pwdid = 'txtPwd' + indx; var _emailid = '' + indx; var _bpwdid = 'btnPWD';
//        var _supp = ''; var _user = ''; var _pwd = ''; var _email = ''; var _user_id = ''; var _act = '';
//        var _lgDet = SetLoginInfo(false, _linkid); _lstlogindet = _lgDet.split('|');
//        _supp = Str(_lstlogindet[1]); _user = Str(_lstlogindet[2]); _pwd = Str(_lstlogindet[3]); _email = Str(_lstlogindet[4]);
//        _act = Str(_lstlogindet[5]); if (_act == '1') { _achecked = 'checked'; } else { _achecked = ''; } _user_id = Str(_lstlogindet[0]);
//        var sOut = '<div><table cellpadding=1  width="450px"  id="' + tid + '"><tbody id="' + _tbodyid + '">' +
//                     ' <tr> <td><div style ="margin-top:5px;text-align:right;padding-right:6px;">Supplier Name :</div></td> <td>' +
//                     ' <div  style ="margin-top:5px;"><span style="width:220px;" id="' + _suppid + '">' + _supp + ' </span><span style="display:none;" id="spuserid">' + _user_id + ' </span> </div>' +
//                     ' </td></tr><tr><td><div style ="margin-top:5px;text-align:right;padding-right:6px;"> User code : </div></td><td>' +
//                     ' <div  style ="margin-top:5px;"><span style="width:220px;" id="' + _userid + '">' + _user + ' </span></div></td></tr><tr>' +
//                   '  <td><div style ="margin-top:5px;text-align:right;padding-right:6px;"> Password : </div></td>' +
//                   '  <td><div  style ="margin-top:5px;"><span style="width:200px;" id="' + _pwdid + '">' + _pwd + ' </span> &nbsp;<button type="button" id="' + _bpwdid + '">Change</button> </div> </td> </tr> <tr>' +
//                   '  <td><div style ="margin-top:5px;text-align:right;padding-right:6px;"> Email : </div></td>' +
//                   '  <td> <div  style ="margin-top:5px;"><span style="width:200px;" id="' + _emailid + '">' + _email + ' </span> </div>' +
//                   ' </td></tr><tr><td><div style ="margin-top:5px;text-align:right;padding-right:6px;"> Active : </div></td>' +
//                   ' <td> <div  style ="margin-top:5px;"><input type="checkbox" id="' + _actid + '" value="' + _act + '"' + _achecked + '/> </div></td></tr>';
//        sOut += ' </div> </td><td></td><td></td><td></td><td></td></tr></tbody></table></div>';
//    }
//    return sOut;
//}

//var ChangePassword = function (indx) {
//    var sOut = ''; var _str = ''; var _newpwd = ''; var _bcnfmpwdef = '';; var _nPwd = ''; var _nCPwd = '';
//    var tid = "ChangePWDTable" + indx; var _tbodyid = "tblCPWD" + indx; var _Pwdid = 'txtPwd'; var _CPwdid = 'txtConfirmPWD';
//    var sOut = '<div><table cellpadding=1  width="450px"  id="' + tid + '"> <tbody id="' + _tbodyid + '"> <tr>' +
//               '  <td><div style ="margin-top:5px;text-align:right;padding-right:6px;">New Password :</div></td>' +
//               '  <td> <div  style ="margin-top:5px;"> <input type="password" class="form-control" style="width:200px;" id="' + _Pwdid + '"  value="' + _nPwd + '"/>' +
//               ' </div></td></tr><tr>' +
//               ' <td><div style ="margin-top:5px;text-align:right;padding-right:6px;"> Confirm Password : </div></td>' +
//               ' <td><div  style ="margin-top:5px;"><input type="password" class="form-control" style="width:200px;" id="' + _CPwdid + '"  value="' + _nCPwd + '"/> </div></td> </tr>';
//    sOut += '</tbody></table></div>';
//    return sOut;
//};

//function ValidateBSLink(indx, _linkid) {
//    var _valid = true;
//    var _bLnkcd = $('#txtBLinkCode' + indx).val(); var _sLnkcd = $('#txtSLinkCode' + indx).val();
//    var _act = ($('#chkAct' + indx).is(':checked')) ? 1 : 0;
//    var _suppSenCd = $('#txtSuppSenCd' + indx).val(); var _suppRecCd = $('#txtSuppRecCd' + indx).val();
//    var _mailsb = $('#txtMailsub' + indx).val(); var _grp = $('#cbGrp' + indx).val(); var _grpcode = $('#cbGrp' + indx + ' option:selected').text();
//    var _bimp = $('#txtBImpPath' + indx).val(); var _bexp = $('#txtBExpPath' + indx).val();
//    if (_bLnkcd == '') { $('#txtBLinkCode' + indx).addClass('error'); _valid = false; } else { $('#txtBLinkCode' + indx).removeClass('error'); }
//    if (_sLnkcd == '') { $('#txtSLinkCode' + indx).addClass('error'); _valid = false; } else { $('#txtSLinkCode' + indx).removeClass('error'); }
//    if (_bLnkcd != '' && _sLnkcd != '') {
//        var isexist = CheckExistingBSuppLink(_linkid, _bLnkcd, _sLnkcd);
//        if (isexist != '') { toastr.error("Lighthouse eSolutions Pte. Ltd.", isexist); _valid = false; }
//    }
//    if (_act == '1') {
//        if (_suppSenCd == '') { $('#txtSuppSenCd' + indx).addClass('error'); _valid = false; } else { $('#txtSuppSenCd' + indx).removeClass('error'); }
//        if (_suppRecCd == '') { $('#txtSuppRecCd' + indx).addClass('error'); _valid = false; } else { $('#txtSuppRecCd' + indx).removeClass('error'); }
//        if (_mailsb == '') { $('#txtMailsub' + indx).addClass('error'); _valid = false; } else { $('#txtMailsub' + indx).removeClass('error'); }
//        if (_grp != '') {
//            var isexist = CheckExistingGroup(_grpcode);
//            if (isexist != '') { toastr.error("Lighthouse eSolutions Pte. Ltd.", isexist); _valid = false; } else { $('#cbGrp' + indx).removeClass('error'); }
//        }
//        else { $('#cbGrp' + indx).addClass('error'); _valid = false; }

//        if (_bimp == '') { $('#txtBImpPath' + indx).addClass('error'); _valid = false; } else { $('#txtBImpPath' + indx).removeClass('error'); }
//        if (_bexp == '') { $('#txtBExpPath' + indx).addClass('error'); _valid = false; } else { $('#txtBExpPath' + indx).removeClass('error'); }
//    }
//    return _valid;
//};

//function GetBSLinkDetails(Table, nTr, _lstdet, _targetclick) {
//    var indx = '';
//    if (nTr != '') {
//        indx = nTr.rowIndex;
//    }
//    else { indx = 0; }

//    var tid = "BSLinkTable" + indx;
//    var _rfq = ($('#chkRFQProcess' + indx).is(':checked')) ? 1 : 0;
//    var _quote = ($('#chkQuoteProcess' + indx).is(':checked')) ? 1 : 0;
//    var _po = ($('#chkPOProcess' + indx).is(':checked')) ? 1 : 0;
//    var _poc = ($('#chkPOCProcess' + indx).is(':checked')) ? 1 : 0;
//    var _code = $('#txtCode' + indx).val(); var _bLnkcd = $('#txtBLinkCode' + indx).val(); var _sLnkcd = $('#txtSLinkCode' + indx).val();
//    var _suppSenCd = $('#txtSuppSenCd' + indx).val();
//    var _suppRecCd = $('#txtSuppRecCd' + indx).val(); var _bSenCd = $('#txtBSenCd' + indx).val(); var _brecvd = $('#txtBRecCd' + indx).val(); var _bmap = $('#txtBMapping' + indx).val();
//    var _smap = $('#txtSMapping' + indx).val();
//    var _bcont = $('#txtBContact' + indx).val(); var _scont = $('#txtSContact' + indx).val(); var _bemail = $('#txtBEmail' + indx).val(); var _semail = $('#txtSEmail' + indx).val();
//    var _ccemail = $('#txtCCEmail' + indx).val(); var _bccemail = $('#txtBCCEmail' + indx).val(); var _mailsb = $('#txtMailsub' + indx).val();
//    var _reply = $('#txtReplyEmail' + indx).val(); var _bimp = $('#txtBImpPath' + indx).val(); var _bexp = $('#txtBExpPath' + indx).val(); var _simp = $('#txtSImpPath' + indx).val();
//    var _sexp = $('#txtSExpPath' + indx).val(); var _wurl = $('#txtWebserURL' + indx).val(); var _dprice = $('#txtDefPrice' + indx).val();
//    var _ufilety = $('#txtUpFileType' + indx).val();
//    var _irfq = ($('#chkImpRFQ' + indx).is(':checked')) ? 1 : 0;
//    var _erfq = ($('#chkExpRFQ' + indx).is(':checked')) ? 1 : 0;
//    var _erfqack = ($('#chkExpRFQAck' + indx).is(':checked')) ? 1 : 0;
//    var _iqte = ($('#chkImpQuote' + indx).is(':checked')) ? 1 : 0;
//    var _eqte = ($('#chkExpQuote' + indx).is(':checked')) ? 1 : 0;
//    var _ipo = ($('#chkImpPO' + indx).is(':checked')) ? 1 : 0;
//    var _epo = ($('#chkExpPO' + indx).is(':checked')) ? 1 : 0;
//    var _epoack = ($('#chkExpPOAck' + indx).is(':checked')) ? 1 : 0;
//    var _epoc = ($('#chkExpPOC' + indx).is(':checked')) ? 1 : 0;
//    var _nby = ($('#chkNBuyer' + indx).is(':checked')) ? 1 : 0;
//    var _nsp = ($('#chkNSupp' + indx).is(':checked')) ? 1 : 0;
//    var _act = ($('#chkAct' + indx).is(':checked')) ? 1 : 0;
//    var _grp = $('#cbGrp' + indx).val();
//    _lstdet.push("BUYER_CODE" + "|" + Str(_code)); _lstdet.push("BUYER_LINK_CODE" + "|" + Str(_bLnkcd)); _lstdet.push("VENDOR_LINK_CODE" + "|" + Str(_sLnkcd)); _lstdet.push("BUYER_EMAIL" + "|" + Str(_bemail));
//    _lstdet.push("SUPPLIER_EMAIL" + "|" + Str(_semail)); _lstdet.push("SUPP_SENDER_CODE" + "|" + Str(_suppSenCd));
//    _lstdet.push("SUPP_RECEIVER_CODE" + "|" + Str(_suppRecCd)); _lstdet.push("BYR_SENDER_CODE" + "|" + Str(_bSenCd)); _lstdet.push("BYR_RECEIVER_CODE" + "|" + Str(_brecvd)); _lstdet.push("BUYER_MAPPING" + "|" + Str(_bmap)); _lstdet.push("SUPPLIER_MAPPING" + "|" + Str(_smap));
//    _lstdet.push("BUYER_CONTACT" + "|" + Str(_bcont)); _lstdet.push("SUPPLIER_CONTACT" + "|" + Str(_scont));
//    _lstdet.push("IMPORT_PATH" + "|" + Str(_bimp)); _lstdet.push("EXPORT_PATH" + "|" + Str(_bexp)); _lstdet.push("SUPP_IMPORT_PATH" + "|" + Str(_simp)); _lstdet.push("SUPP_EXPORT_PATH" + "|" + Str(_sexp));
//    _lstdet.push("CC_EMAIL" + "|" + Str(_ccemail)); _lstdet.push("BCC_EMAIL" + "|" + Str(_bccemail)); _lstdet.push("MAIL_SUBJECT" + "|" + Str(_mailsb));
//    _lstdet.push("REPLY_EMAIL" + "|" + Str(_reply)); _lstdet.push("GROUP_ID" + "|" + Str(_grp)); _lstdet.push("IMPORT_RFQ" + "|" + Str(_irfq)); _lstdet.push("EXPORT_RFQ" + "|" + Str(_erfq));
//    _lstdet.push("EXPORT_RFQ_ACK" + "|" + Str(_erfqack)); _lstdet.push("IMPORT_QUOTE" + "|" + Str(_iqte)); _lstdet.push("EXPORT_QUOTE" + "|" + Str(_eqte)); _lstdet.push("IMPORT_PO" + "|" + Str(_ipo)); _lstdet.push("EXPORT_PO" + "|" + Str(_epo));
//    _lstdet.push("EXPORT_PO_ACK" + "|" + Str(_epoack)); _lstdet.push("EXPORT_POC" + "|" + Str(_epoc)); _lstdet.push("NOTIFY_BUYER" + "|" + Str(_nby)); _lstdet.push("NOTIFY_SUPPLR" + "|" + Str(_nsp));
//    _lstdet.push("DEFAULT_PRICE" + "|" + Str(_dprice)); _lstdet.push("IS_ACTIVE" + "|" + Str(_act)); _lstdet.push("UPLOAD_FILE_TYPE" + "|" + Str(_ufilety)); _lstdet.push("SUP_WEB_SERVICE_URL" + "|" + Str(_wurl));
//};



//{ 'targets': [0], width: '20px' },
//{ 'targets': [1], width: '40px', 'bSortable': false },
//{ 'targets': [2], width: '40px' },   { 'targets': [3], width: '180px'  }, { 'targets': [4], width: '100px' },
//{ 'targets': [5], width: '100px', className: "break-det"},
//{ 'targets': [6], width: '70px' },  { 'targets': [7], width: '70px' },
//{ 'targets': [8,9], width: '150px',className: "break-det" },

//{ 'targets': [0], width: '10px', 'bSortable': false }, { 'targets': [1], width: '30px', 'bSortable': false },
// { 'targets': [3], width: '180px', "sClass": "break-det" },
//{ 'targets': [2], width: '120px' }, { 'targets': [4, 5], width: '90px' }, { 'targets': [18], width: '60px' },

//{ 'targets': [0], width: '25px', 'bSortable': false }, { 'targets': [2], width: '40px'},
//{ 'targets': [3], width: '200px' }, { 'targets': [4, 5], width: '110px' }, { 'targets': [6], width: '80px' }, { 'targets': [7], width: '90px' }, { 'targets': [8], width: '70px' },
//{ 'targets': [9, 10], width: '180px' },

