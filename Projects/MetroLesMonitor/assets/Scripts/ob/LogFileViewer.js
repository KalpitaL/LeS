var _0x3be5 = ['fnDraw', 'PATH', 'ajax', 'POST', 'LogFileViewer.aspx/GetLogFileDetailsGrid', 'parse', 'NewDataSet', 'unblockUI', 'error', 'Get\x20Log\x20File\x20Viewer\x20:', 'failure\x20get', 'responseText', 'LogFileViewer.aspx/GetFileDetails', '{\x27PATH\x27:\x27', 'application/json;\x20charset=utf-8', 'json', 'Path', 'indexOf', 'Lighthouse\x20eSolutions\x20Pte.\x20Ltd.', 'error\x20get', 'push', 'TIME_STAMP', 'FILENAME', 'LOG_FILE_ID', 'FDET', 'LogFileViewer.aspx/DownloadFile', '{\x20\x27PATH\x27:\x27', '\x27,\x27FILENAME\x27:\x27', '\x27\x20}', 'location', '../Downloads/', 'success', 'Lighthouse\x20eSolutions\x20Pte.\x20Ltd', 'Download\x20File\x20success', 'Download\x20File\x20:', '#toolbtngroup', '#tblBodyLogFView', 'empty', 'append', 'Log\x20File\x20Viewer', 'Home', 'Home.aspx', 'Viewers', 'getElementById', 'lnkViewer', 'addClass', 'active\x20open', 'spLogFileView', 'title\x20font-title\x20SelectedColor', 'onload', '#chkLogfileview', 'chkLogfileview', '#dataGridLogFView', 'dataTable', ':\x20activate\x20to\x20sort\x20column\x20ascending', ':\x20activate\x20to\x20sort\x20column\x20descending', 'No\x20data\x20available\x20in\x20table', 'Showing\x20_START_\x20to\x20_END_\x20of\x20_TOTAL_\x20entries', 'No\x20entries\x20found', '(filtered\x20from\x20_MAX_\x20total\x20entries)', 'Show\x20_MENU_\x20entries', 'Quick\x20Search:', 'No\x20matching\x20records\x20found', 'single', 'select_all', 'select_none', '5px', '60px', '280px', 'All', '300px', 'opentd', '#tblHeadRowLogFView', 'gridHeader', '#ToolTables_dataGridLogFView_0,#ToolTables_dataGridLogFView_1,#dataGridLogFView_paginate', 'hide', '.dataTables_scrollHeadInner,.dataTables_scrollHeadInner\x20table,.dataTables_scrollBody\x20table', 'css', 'min-width', '100%', 'tbody\x20td', 'parents', 'fnGetData', 'index', 'fnIsOpen', 'closetd', 'fnClose', 'replace', 'fnOpen', '#FileTable', 'rowIndex', '\x20tbody\x20tr', 'cells', 'innerText', 'target', 'val', 'filter', ':contains(\x27', 'show', 'FileTable', 'tblHeadCLogFView', 'tblBodyCLogFView', '\x20<div\x20style=\x22float:right;width:50%;\x22><span>Search\x20:</span>\x20&nbsp;<input\x20type=\x22text\x22\x20id=\x22txtSearch\x22/></div>', '<table\x20cellpadding=\x225\x22\x20cellspacing=\x220\x22\x20border=\x220\x22\x20style=\x22padding-left:50px;\x22\x20width=\x2250%\x22\x20id=\x22', '<thead\x20id=\x22', '\x22><tr><th>Time\x20Stamp</th><th>File\x20Name</th><th\x20style=\x22display:none;\x22>LOG_FILE_ID</th></tr></thead><tbody\x20id=\x22', 'split', '<tr\x20id=', '<td><a\x20href=\x22#\x22><u>', '</u></a></td>', '<td\x20style=\x22display:none;\x22>', '</td>', '<td>', '</tr>', '</tbody></table>', 'DataTable', 'clear', 'draw', 'length', 'MODULE', 'DESCRIPTION', 'LOG_ID', 'fnAddData']; (function (_0x121ebb, _0x597f84) { var _0x53c44d = function (_0x4e1788) { while (--_0x4e1788) { _0x121ebb['push'](_0x121ebb['shift']()); } }; _0x53c44d(++_0x597f84); }(_0x3be5, 0x196)); var _0x2125 = function (_0x269f89, _0x275f3a) { _0x269f89 = _0x269f89 - 0x0; var _0x390af2 = _0x3be5[_0x269f89]; return _0x390af2; }; var _fromlogdate = ''; var _tologdate = ''; var duration = ''; var _module = ''; var _filepath = ''; var selectedTr = ''; var previousTr = ''; var iTableCounter = 0x1; var _filedetails = []; var _emptydet = []; var LogFileViewer = function () { var _0x1c4e6d = function () { var _0x57aa29 = null; var _0x3b2938 = ![]; $('#pageTitle')[_0x2125('0x0')]()[_0x2125('0x1')](_0x2125('0x2')); SetupBreadcrumb(_0x2125('0x3'), _0x2125('0x4'), _0x2125('0x5'), '#', _0x2125('0x2'), 'LogFileViewer.aspx'); $(document[_0x2125('0x6')](_0x2125('0x7')))[_0x2125('0x8')](_0x2125('0x9')); $(document[_0x2125('0x6')](_0x2125('0xa')))[_0x2125('0x8')](_0x2125('0xb')); _0x4445aa(); var _0x523348 = '<label><input\x20type=\x22checkbox\x22\x20class=\x22icheck\x22\x20\x20onclick=\x22DoAutoRefresh(this.checked,this.id)\x22\x20id=\x22chkLogfileview\x22/>\x20Auto\x20Refresh\x20Page</label>'; $('#divChkBox')[_0x2125('0x1')](_0x523348); window[_0x2125('0xc')] = SetTimerCheckBox(_0x2125('0xd'), _0x2125('0xe')); var _0x4c4054 = $(_0x2125('0xf')); var _0x3f5cae = _0x4c4054[_0x2125('0x10')]({ 'bDestroy': !![], 'bSort': ![], 'language': { 'aria': { 'sortAscending': _0x2125('0x11'), 'sortDescending': _0x2125('0x12') }, 'emptyTable': _0x2125('0x13'), 'info': _0x2125('0x14'), 'infoEmpty': _0x2125('0x15'), 'infoFiltered': _0x2125('0x16'), 'lengthMenu': _0x2125('0x17'), 'search': _0x2125('0x18'), 'zeroRecords': _0x2125('0x19') }, 'dom': 'T<\x22clear\x22>lfrtip', 'tableTools': { 'sRowSelect': _0x2125('0x1a'), 'aButtons': [_0x2125('0x1b'), _0x2125('0x1c')] }, 'columnDefs': [{ 'orderable': ![], 'searching': ![], 'autoWidth': ![], 'targets': [0x0] }, { 'targets': [0x0], 'width': _0x2125('0x1d'), 'bSortable': ![] }, { 'targets': [0x1], 'width': _0x2125('0x1e') }, { 'targets': [0x2], 'width': _0x2125('0x1f') }, { 'targets': [0x3, 0x4], 'visible': ![] }], 'aaSorting': [], 'lengthMenu': [[0x19, 0x32, 0x64, -0x1], [0x19, 0x32, 0x64, _0x2125('0x20')]], 'scrollY': _0x2125('0x21'), 'aaSorting': [], 'fnRowCallback': function (_0x2cef10, _0x244ac7, _0x51f281) { $('td:eq(0)', _0x2cef10)['addClass'](_0x2125('0x22')); } }); $(_0x2125('0x23'))[_0x2125('0x8')](_0x2125('0x24')); $(_0x2125('0x25'))[_0x2125('0x26')](); $(_0x2125('0x27'))[_0x2125('0x28')](_0x2125('0x29'), _0x2125('0x2a')); _0x48e756(); _0x3f5cae['on']('click', _0x2125('0x2b'), function (_0x532215) { var _0x28fcd1 = $(this)[_0x2125('0x2c')]('tr')[0x0]; var _0x2337da = _0x3f5cae[_0x2125('0x2d')](_0x28fcd1); if ($(this)[_0x2125('0x2e')]() == 0x0) { if (_0x3f5cae[_0x2125('0x2f')](_0x28fcd1)) { $(this)['addClass'](_0x2125('0x22'))['removeClass'](_0x2125('0x30')); _0x3f5cae[_0x2125('0x31')](_0x28fcd1); } else { if (_0x2337da != null) { _module = Str(_0x2337da[0x1]); _filepath = Str(_0x2337da[0x3][_0x2125('0x32')](/\\/g, '?')); var _0x394bb3 = _0xe7fbc3(_0x3f5cae, _0x28fcd1); if (_0x394bb3 != '') { _0x3f5cae[_0x2125('0x33')](_0x28fcd1, _0x394bb3, 'details'); $(this)[_0x2125('0x8')](_0x2125('0x30'))['removeClass'](_0x2125('0x22')); } previousTr = _0x28fcd1; } else { var _0x4c4054 = _0x2125('0x34') + (_0x28fcd1[_0x2125('0x35')] - 0x1); var _0x529bf0 = $(_0x4c4054 + _0x2125('0x36')); for (var _0x2eff40 = 0x0; _0x2eff40 < _0x529bf0['length']; _0x2eff40++) { var _0x5ae6a3 = _0x529bf0[_0x2eff40][_0x2125('0x37')][0x1][_0x2125('0x38')]; if (_0x532215[_0x2125('0x39')]['innerText'] != '' && _0x5ae6a3 == _0x532215['target'][_0x2125('0x38')]) { _0x40fedd(_filepath, _0x532215[_0x2125('0x39')]['innerText'], _module); } } $('#txtSearch')['keyup'](function () { var _0x364606 = $['trim']($(this)[_0x2125('0x3a')]())['replace'](/ +/g, '\x20'); _0x529bf0[_0x2125('0x26')](); _0x529bf0[_0x2125('0x3b')](_0x2125('0x3c') + _0x364606 + '\x27)')[_0x2125('0x3d')](); }); } } } }); function _0xe7fbc3(_0x166060, _0x220402) { var _0x41c1d6 = ''; var _0xf73b57 = ''; var _0x12a732 = ''; var _0x4507f1 = _0x220402['rowIndex']; var _0x4a40fb = _0x3f5cae[_0x2125('0x2d')](_0x220402); var _0x2262d0 = _0x2125('0x3e') + _0x4507f1; var _0x5de52f = _0x2125('0x3f') + _0x4507f1; var _0x54648a = _0x2125('0x40') + _0x4507f1; if (Str(_0x4a40fb[0x3] != '')) { _0x12a732 = Str(_0x4a40fb[0x3][_0x2125('0x32')](/\\/g, '?')); _filedetails = []; var _0x5c87e6 = _0x1a7572(_0x12a732, _filedetails); if (_0x5c87e6 == 0x1) { var _0x10ac73 = _0x2125('0x41'); var _0x41c1d6 = _0x10ac73 + _0x2125('0x42') + _0x2262d0 + '\x22>' + _0x2125('0x43') + _0x5de52f + _0x2125('0x44') + _0x54648a + '\x22>'; for (var _0x4e1c52 = 0x0; _0x4e1c52 < _filedetails['length']; _0x4e1c52++) { var _0x46d625 = []; _0xf73b57 = _filedetails[_0x4e1c52][_0x2125('0x45')]('#')[0x1]; _0x46d625 = _0xf73b57[_0x2125('0x45')](','); _0x41c1d6 += _0x2125('0x46') + _0x4e1c52 + '>'; for (var _0x445e7b = 0x0; _0x445e7b < _0x46d625['length']; _0x445e7b++) { if (_0x445e7b == 0x1) { _0x41c1d6 += _0x2125('0x47') + _0x46d625[_0x445e7b] + _0x2125('0x48'); } else if (_0x445e7b == 0x2) { _0x41c1d6 += _0x2125('0x49') + _0x46d625[_0x445e7b] + _0x2125('0x4a'); } else { _0x41c1d6 += _0x2125('0x4b') + _0x46d625[_0x445e7b] + '</td>'; } } _0x41c1d6 += _0x2125('0x4c'); } _0x41c1d6 += _0x2125('0x4d'); } } return _0x41c1d6; } }; function _0x116164(_0x4caae3) { try { $(_0x2125('0xf'))[_0x2125('0x4e')]()[_0x2125('0x4f')]()[_0x2125('0x50')](); var _0x24bd77 = $('#dataGridLogFView')['dataTable'](); if (_0x4caae3[_0x2125('0x51')] != undefined && _0x4caae3[_0x2125('0x51')] != null) { for (var _0x453e42 = 0x0; _0x453e42 < _0x4caae3['length']; _0x453e42++) { var _0x4f5d25 = new Array(); var _0x4e360e = jQuery(_0x2125('0x46') + _0x453e42 + '>'); _0x4f5d25[0x0] = Str(''); _0x4f5d25[0x1] = Str(_0x4caae3[_0x453e42][_0x2125('0x52')]); _0x4f5d25[0x2] = Str(_0x4caae3[_0x453e42][_0x2125('0x53')]); _0x4f5d25[0x3] = Str(_0x4caae3[_0x453e42]['PATH']); _0x4f5d25[0x4] = Int(_0x4caae3[_0x453e42][_0x2125('0x54')]); var _0x1a4826 = _0x24bd77[_0x2125('0x55')](_0x4f5d25, ![]); } _0x24bd77[_0x2125('0x56')](); } else { if (_0x4caae3[_0x2125('0x54')] != undefined && _0x4caae3[_0x2125('0x54')] != null) { var _0x4e360e = jQuery(_0x2125('0x46') + 0x1 + '>'); var _0x4f5d25 = new Array(); _0x4f5d25[0x0] = Str(''); _0x4f5d25[0x1] = Str(_0x4caae3[_0x2125('0x52')]); _0x4f5d25[0x2] = Str(_0x4caae3[_0x2125('0x53')]); _0x4f5d25[0x3] = Str(_0x4caae3[_0x2125('0x57')]); _0x4f5d25[0x4] = Int(_0x4caae3['LOG_ID']); var _0x1a4826 = _0x24bd77[_0x2125('0x55')](_0x4f5d25, ![]); _0x24bd77['fnDraw'](); } } } catch (_0x1e86bd) { } }; var _0x48e756 = function () { Metronic['blockUI']('#portlet_body'); setTimeout(function () { $[_0x2125('0x58')]({ 'type': _0x2125('0x59'), 'async': ![], 'url': _0x2125('0x5a'), 'data': '{}', 'contentType': 'application/json;\x20charset=utf-8', 'dataType': 'json', 'success': function (_0x494d64) { try { var _0x526d32 = JSON[_0x2125('0x5b')](_0x494d64['d']); if (_0x526d32[_0x2125('0x5c')] != null) { Table = _0x526d32[_0x2125('0x5c')]['Table1']; _0x116164(Table); } else $(_0x2125('0xf'))['DataTable']()[_0x2125('0x4f')]()[_0x2125('0x50')](); Metronic[_0x2125('0x5d')](); } catch (_0x29a17d) { Metronic[_0x2125('0x5d')](); toastr[_0x2125('0x5e')]('Lighthouse\x20eSolutions\x20Pte.\x20Ltd.', _0x2125('0x5f') + _0x29a17d); } }, 'failure': function (_0x299be3) { toastr[_0x2125('0x5e')](_0x2125('0x60'), _0x299be3['d']); Metronic[_0x2125('0x5d')](); }, 'error': function (_0x13eef6) { toastr[_0x2125('0x5e')]('error\x20get', _0x13eef6[_0x2125('0x61')]); Metronic[_0x2125('0x5d')](); } }); }, 0xc8); }; var _0x1a7572 = function (_0x1ffd05, _0x41ef59) { var _0x5e36cf = ''; $['ajax']({ 'type': _0x2125('0x59'), 'async': ![], 'url': _0x2125('0x62'), 'data': _0x2125('0x63') + _0x1ffd05 + '\x27}', 'contentType': _0x2125('0x64'), 'dataType': _0x2125('0x65'), 'success': function (_0x369d38) { try { var _0x281fc8 = _0x2125('0x66'); if (Str(_0x369d38['d'])[_0x2125('0x67')](_0x281fc8) == -0x1) { var _0x2f8adf = JSON['parse'](_0x369d38['d']); if (_0x2f8adf[_0x2125('0x5c')] != null) { Table = _0x2f8adf[_0x2125('0x5c')]['Table1']; _0x4ad9ff(Table, _0x41ef59); _0x5e36cf = '1'; } } else { _0x5e36cf = ''; alert(_0x369d38['d']); } } catch (_0x2d7cd7) { toastr[_0x2125('0x5e')](_0x2125('0x68'), 'Get\x20Log\x20File\x20Details\x20:' + _0x2d7cd7); } }, 'failure': function (_0x39be69) { toastr[_0x2125('0x5e')](_0x2125('0x60'), _0x39be69['d']); }, 'error': function (_0x53f74e) { toastr[_0x2125('0x5e')](_0x2125('0x69'), _0x53f74e[_0x2125('0x61')]); } }); return _0x5e36cf; }; function _0x4ad9ff(_0x81e2b2, _0x2093d5) { var _0x5b51bd = []; try { if (_0x81e2b2[_0x2125('0x51')] != undefined && _0x81e2b2[_0x2125('0x51')] != null) { for (var _0x2ff5fe = 0x0; _0x2ff5fe < _0x81e2b2['length']; _0x2ff5fe++) { _0x5b51bd = []; _0x5b51bd[_0x2125('0x6a')](Str(_0x81e2b2[_0x2ff5fe][_0x2125('0x6b')])); _0x5b51bd[_0x2125('0x6a')](Str(_0x81e2b2[_0x2ff5fe][_0x2125('0x6c')])); _0x5b51bd['push'](Str(_0x81e2b2[_0x2ff5fe][_0x2125('0x6d')])); _0x2093d5[_0x2125('0x6a')](_0x2125('0x6e') + _0x2ff5fe + '#' + _0x5b51bd); } } else if (_0x81e2b2[_0x2125('0x6d')] != undefined && _0x81e2b2[_0x2125('0x6d')] != null) { _0x5b51bd[_0x2125('0x6a')](Str(_0x81e2b2[_0x2125('0x6b')])); _0x5b51bd[_0x2125('0x6a')](Str(_0x81e2b2[_0x2125('0x6c')])); _0x5b51bd[_0x2125('0x6a')](Str(_0x81e2b2[_0x2125('0x6d')])); _0x2093d5[_0x2125('0x6a')](_0x2125('0x6e') + _0x2ff5fe + '#' + _0x5b51bd); } } catch (_0x1626c3) { } }; function _0x40fedd(_0x36b259, _0x2d9e26, _0x321ccd) { try { $[_0x2125('0x58')]({ 'type': _0x2125('0x59'), 'async': ![], 'url': _0x2125('0x6f'), 'data': _0x2125('0x70') + _0x36b259 + _0x2125('0x71') + _0x2d9e26 + '\x27,\x27MODULE\x27:\x27' + _0x321ccd + _0x2125('0x72'), 'contentType': _0x2125('0x64'), 'dataType': _0x2125('0x65'), 'success': function (_0x12d8b6) { try { var _0x241667 = Str(_0x12d8b6['d']); if (_0x241667 != '') { var _0xd08f2a = _0x241667[_0x2125('0x45')]('|')[0x0]; var _0x24f01c = _0x241667[_0x2125('0x45')]('|')[0x1]; top[_0x2125('0x73')]['href'] = _0x2125('0x74') + _0x24f01c; toastr[_0x2125('0x75')](_0x2125('0x76'), _0x2125('0x77')); } } catch (_0x46a9da) { toastr[_0x2125('0x5e')](_0x2125('0x68'), _0x2125('0x78') + _0x46a9da); } }, 'failure': function (_0x789ac9) { toastr[_0x2125('0x5e')](_0x2125('0x60'), _0x789ac9['d']); }, 'error': function (_0x489cc6) { toastr[_0x2125('0x5e')](_0x2125('0x69'), _0x489cc6[_0x2125('0x61')]); } }); } catch (_0x520ffe) { } }; var _0x4445aa = function () { $(_0x2125('0x79'))[_0x2125('0x0')](); var _0x54e41f = '<th\x20style=\x22text-align:center;\x22\x20id=\x22opnclse\x22></th><th\x20id=\x22txtModule\x22>Module</th><th>Description</th><th>Path</th><th>LOG_ID</th>'; $(_0x2125('0x23'))['empty']()['append'](_0x54e41f); $(_0x2125('0x7a'))['empty'](); }; return { 'init': function () { _0x1c4e6d(); } }; }();