var _0x4fcb = ['65px', '80px', 'All', '300px', '900px', '.dataTables_scrollBody\x20thead\x20tr', 'css', '#tblHeadRowLogHist', 'addClass', 'gridHeader', '#ToolTables_dataGridLogHist_0,#ToolTables_dataGridLogHist_1', 'hide', 'min-width', '100%', 'fnAdjustColumnSizing', 'click', 'preventDefault', 'getElementById', 'dtUpdateFromDate', 'val', 'dtUpdateToDate', 'datepicker', 'changeDate', '#dtUpdateFromDate', '#dtUpdateToDate', '#pageTitle', 'empty', 'append', 'Login\x20History', 'Home', 'LoginHistory.aspx', 'lnkLoginHistory', 'active', '<label><input\x20type=\x22checkbox\x22\x20class=\x22icheck\x22\x20\x20onclick=\x22DoAutoRefresh(this.checked,this.id)\x22\x20id=\x22chkLoginHist\x22/>\x20Auto\x20Refresh\x20Page</label>', 'onload', '#chkLoginHist', 'chkLoginHist', 'clear', 'draw', 'length', '<tr\x20id=', 'UPDATED_DATE', 'CLIENT_SERVER_IP', 'LOGGED_IN', 'LOGGED_IN_REMARKS', 'LOGGED_OUT', 'LOGGED_OUT_REMARKS', 'SESSIONID', 'fnAddData', 'fnDraw', 'LOGIN_TRACK_ID', 'EX_USERCODE', '#portlet_body', 'ajax', 'POST', 'LoginHistory.aspx/GetLoginHistoryGrid', '{\x20\x27UPDATE_DATEFROM\x27:\x27', '\x27,\x27UPDATE_DATETO\x27:\x27', 'application/json;\x20charset=utf-8', 'json', 'parse', 'Table', 'DataTable', 'unblockUI', 'error', 'Lighthouse\x20eSolutions\x20Pte.\x20Ltd.', 'Get\x20Login\x20History\x20:', 'failure\x20get', 'error\x20get', 'responseText', '#toolbtngroup', '\x20<div\x20class=\x22row\x22\x20style=\x22margin-bottom:\x202px;padding-right:\x2020px;\x22>\x20<div\x20class=\x22col-md-12\x22>', '<div\x20id=\x22toolbtngroup\x22\x20><span\x20title=\x22Clear\x22\x20data-toggle=\x22tooltip\x22\x20data-placement=\x22top\x22><div\x20class=\x22pull-right\x20margin-right-10\x22\x20id=\x22divbtnClearFilter\x22><a\x20href=\x22javascript:;\x22\x20class=\x22toolbtn\x22\x20id=\x22btnClear\x22><i\x20class=\x22fa\x20fa-eraser\x22></i></a></div>', '</div></div>', '\x20<div\x20class=\x22row\x22>\x20<div\x20class=\x22col-md-12\x22>\x20<div\x20class=\x22col-md-4\x22\x20style=\x22text-align:right;\x22>', '\x20<div\x20class=\x22col-md-4\x22><label\x20class=\x22control-label\x22>Update\x20From\x20</label>\x20\x20</div>', '\x20</div>\x20<div\x20class=\x22col-md-4\x22\x20style=\x22text-align:right;\x22>', '\x20<div\x20class=\x22col-md-8\x22>\x20<input\x20class=\x22form-control\x20date-picker\x20csDatePicker\x20InputText\x22\x20data-date-format=\x22dd/mm/yyyy\x22\x20size=\x2216\x22\x20type=\x22text\x22\x20id=\x22dtUpdateToDate\x22\x20value=\x22\x22/>\x20</div>', '\x20</div></div></div>', '#divFilter', 'getDate', 'setDate', '<th\x20id=\x22cbLogHistheader\x22></th><th\x20style=\x22text-align:center;\x22>#</th><th>\x20User</th><th>Client\x20IP\x20Address</th><th>Logged\x20In\x20Time</th><th>Logged\x20In\x20Log</th><th>Logged\x20Out\x20Time</th><th>Logged\x20Out\x20Log</th><th>Session\x20Id</th><th>LOGIN_TRACK_ID</th>', '#dataGridLogHist', 'dataTable', ':\x20activate\x20to\x20sort\x20column\x20ascending', ':\x20activate\x20to\x20sort\x20column\x20descending', 'No\x20data\x20available\x20in\x20table', 'Showing\x20_START_\x20to\x20_END_\x20of\x20_TOTAL_\x20entries', 'No\x20entries\x20found', '(filtered\x20from\x20_MAX_\x20total\x20entries)', 'Show\x20_MENU_\x20entries', 'Quick\x20Search:', 'No\x20matching\x20records\x20found', 'T<\x22clear\x22>lfrtip', 'select_all', '30px']; (function (_0x102509, _0x3cb319) { var _0x200fa2 = function (_0x68858a) { while (--_0x68858a) { _0x102509['push'](_0x102509['shift']()); } }; _0x200fa2(++_0x3cb319); }(_0x4fcb, 0xb4)); var _0x1a0d = function (_0x30cf8a, _0x3475d9) { _0x30cf8a = _0x30cf8a - 0x0; var _0x534184 = _0x4fcb[_0x30cf8a]; return _0x534184; }; var _fromlogdate = ''; var _tologdate = ''; var LoginHistory = function () { var _0xeed0b = function () { _0x22a8d4(); var _0x3f4068 = $(_0x1a0d('0x0')); var _0x47f23d = _0x3f4068[_0x1a0d('0x1')]({ 'bDestroy': !![], 'bSort': ![], 'language': { 'aria': { 'sortAscending': _0x1a0d('0x2'), 'sortDescending': _0x1a0d('0x3') }, 'emptyTable': _0x1a0d('0x4'), 'info': _0x1a0d('0x5'), 'infoEmpty': _0x1a0d('0x6'), 'infoFiltered': _0x1a0d('0x7'), 'lengthMenu': _0x1a0d('0x8'), 'search': _0x1a0d('0x9'), 'zeroRecords': _0x1a0d('0xa') }, 'dom': _0x1a0d('0xb'), 'tableTools': { 'sRowSelect': 'single', 'aButtons': [_0x1a0d('0xc'), 'select_none'] }, 'columnDefs': [{ 'orderable': ![], 'searching': !![], 'autoWidth': ![], 'targets': [0x0] }, { 'targets': [0x1, 0x2], 'width': _0x1a0d('0xd') }, { 'targets': [0x3], 'width': '120px' }, { 'targets': [0x4, 0x5, 0x6, 0x7], 'width': _0x1a0d('0xe') }, { 'targets': [0x8], 'width': _0x1a0d('0xf') }, { 'targets': [0x6, 0x7, 0x8], 'sClass': 'visible-lg' }, { 'targets': [0x0, 0x9], 'visible': ![] }], 'lengthMenu': [[0x19, 0x32, 0x64, -0x1], [0x19, 0x32, 0x64, _0x1a0d('0x10')]], 'scrollY': _0x1a0d('0x11'), 'sScrollX': _0x1a0d('0x12'), 'aaSorting': [], 'drawCallback': function (_0x31aea5, _0x47e19f) { $(_0x1a0d('0x13'))[_0x1a0d('0x14')]({ 'visibility': 'collapse' }); } }); $(_0x1a0d('0x15'))[_0x1a0d('0x16')](_0x1a0d('0x17')); $(_0x1a0d('0x18'))[_0x1a0d('0x19')](); $('.dataTables_scrollHeadInner,.dataTables_scrollHeadInner\x20table,.dataTables_scrollBody\x20table')['css'](_0x1a0d('0x1a'), _0x1a0d('0x1b')); setTimeout(function () { _0x47f23d[_0x1a0d('0x1c')](); }, 0xa); _0x59ffa(); _0x3e5e4a(_fromlogdate, _tologdate); $('#btnApply')['on'](_0x1a0d('0x1d'), function (_0x153a7a) { _0x153a7a[_0x1a0d('0x1e')](); _fromlogdate = $(document[_0x1a0d('0x1f')](_0x1a0d('0x20')))[_0x1a0d('0x21')](); _tologdate = $(document[_0x1a0d('0x1f')](_0x1a0d('0x22')))[_0x1a0d('0x21')](); _0x3e5e4a(_fromlogdate, _tologdate); }); $('#dtUpdateFromDate')[_0x1a0d('0x23')]()['on'](_0x1a0d('0x24'), function (_0x20c0bf) { $(_0x1a0d('0x25'))[_0x1a0d('0x23')](_0x1a0d('0x19')); }); $(_0x1a0d('0x26'))['datepicker']()['on'](_0x1a0d('0x24'), function (_0x583bd4) { $(_0x1a0d('0x26'))[_0x1a0d('0x23')](_0x1a0d('0x19')); }); $('#divbtnRefresh')['on'](_0x1a0d('0x1d'), function (_0x217651) { _0x217651['preventDefault'](); _fromlogdate = $(document[_0x1a0d('0x1f')](_0x1a0d('0x20')))[_0x1a0d('0x21')](); _tologdate = $(document[_0x1a0d('0x1f')](_0x1a0d('0x22')))[_0x1a0d('0x21')](); _0x3e5e4a(_fromlogdate, _tologdate); }); $('#divbtnClearFilter')['on']('click', function (_0x435360) { _0x435360[_0x1a0d('0x1e')](); _0x47376f(); }); }; function _0x22a8d4() { $(_0x1a0d('0x27'))[_0x1a0d('0x28')]()[_0x1a0d('0x29')](_0x1a0d('0x2a')); SetupBreadcrumb(_0x1a0d('0x2b'), 'Home.aspx', _0x1a0d('0x2a'), _0x1a0d('0x2c'), '', ''); $(document[_0x1a0d('0x1f')](_0x1a0d('0x2d')))[_0x1a0d('0x16')](_0x1a0d('0x2e')); _0x1beb7a(); var _0x5bef76 = _0x1a0d('0x2f'); $('#divChkBox')['append'](_0x5bef76); window[_0x1a0d('0x30')] = SetTimerCheckBox(_0x1a0d('0x31'), _0x1a0d('0x32')); }; function _0x450d39(_0x38ea1f) { try { $(_0x1a0d('0x0'))['DataTable']()[_0x1a0d('0x33')]()[_0x1a0d('0x34')](); if (_0x38ea1f['length'] != undefined && _0x38ea1f[_0x1a0d('0x35')] != null) { var _0x529045 = $(_0x1a0d('0x0'))[_0x1a0d('0x1')](); for (var _0x4d46c8 = 0x0; _0x4d46c8 < _0x38ea1f['length']; _0x4d46c8++) { var _0x1ef689 = new Array(); var _0x32a752 = jQuery(_0x1a0d('0x36') + _0x4d46c8 + '>'); _0x1ef689[0x0] = Str(''); _0x1ef689[0x1] = Str(_0x38ea1f[_0x4d46c8][_0x1a0d('0x37')]); _0x1ef689[0x2] = Str(_0x38ea1f[_0x4d46c8]['EX_USERCODE']); _0x1ef689[0x3] = Str(_0x38ea1f[_0x4d46c8][_0x1a0d('0x38')]); _0x1ef689[0x4] = Str(_0x38ea1f[_0x4d46c8][_0x1a0d('0x39')]); _0x1ef689[0x5] = Str(_0x38ea1f[_0x4d46c8][_0x1a0d('0x3a')]); _0x1ef689[0x6] = Str(_0x38ea1f[_0x4d46c8][_0x1a0d('0x3b')]); _0x1ef689[0x7] = Str(_0x38ea1f[_0x4d46c8][_0x1a0d('0x3c')]); _0x1ef689[0x8] = Str(_0x38ea1f[_0x4d46c8][_0x1a0d('0x3d')]); _0x1ef689[0x9] = Str(_0x38ea1f[_0x4d46c8]['LOGIN_TRACK_ID']); var _0x2d4137 = _0x529045[_0x1a0d('0x3e')](_0x1ef689, ![]); } _0x529045[_0x1a0d('0x3f')](); } else { if (_0x38ea1f[_0x1a0d('0x40')] != undefined && _0x38ea1f[_0x1a0d('0x40')] != null) { var _0x529045 = $(_0x1a0d('0x0'))['dataTable'](); var _0x32a752 = jQuery('<tr\x20id=' + 0x1 + '>'); var _0x1ef689 = new Array(); _0x1ef689[0x0] = Str(''); _0x1ef689[0x1] = Str(_0x38ea1f[_0x1a0d('0x37')]); _0x1ef689[0x2] = Str(_0x38ea1f[_0x1a0d('0x41')]); _0x1ef689[0x3] = Str(_0x38ea1f[_0x1a0d('0x38')]); _0x1ef689[0x4] = Str(_0x38ea1f[_0x1a0d('0x39')]); _0x1ef689[0x5] = Str(_0x38ea1f[_0x1a0d('0x3a')]); _0x1ef689[0x6] = Str(_0x38ea1f[_0x1a0d('0x3b')]); _0x1ef689[0x7] = Str(_0x38ea1f[_0x1a0d('0x3c')]); _0x1ef689[0x8] = Str(_0x38ea1f[_0x1a0d('0x3d')]); _0x1ef689[0x9] = Str(_0x38ea1f[_0x1a0d('0x40')]); var _0x2d4137 = _0x529045[_0x1a0d('0x3e')](_0x1ef689, ![]); _0x529045['fnDraw'](); } } } catch (_0x12271a) { } }; var _0x3e5e4a = function (_0x107d30, _0xcf625) { Metronic['blockUI'](_0x1a0d('0x42')); setTimeout(function () { $[_0x1a0d('0x43')]({ 'type': _0x1a0d('0x44'), 'async': ![], 'url': _0x1a0d('0x45'), 'data': _0x1a0d('0x46') + getSQLDateFormated(_0x107d30) + _0x1a0d('0x47') + getSQLDateFormated(_0xcf625) + '\x27\x20}', 'contentType': _0x1a0d('0x48'), 'dataType': _0x1a0d('0x49'), 'success': function (_0x553ba8) { try { var _0x5c1c04 = JSON[_0x1a0d('0x4a')](_0x553ba8['d']); if (_0x5c1c04['NewDataSet'] != null) { Table = _0x5c1c04['NewDataSet'][_0x1a0d('0x4b')]; _0x450d39(Table); } else $(_0x1a0d('0x0'))[_0x1a0d('0x4c')]()[_0x1a0d('0x33')]()[_0x1a0d('0x34')](); Metronic[_0x1a0d('0x4d')](); } catch (_0x42cbc4) { Metronic[_0x1a0d('0x4d')](); toastr[_0x1a0d('0x4e')](_0x1a0d('0x4f'), _0x1a0d('0x50') + _0x42cbc4); } }, 'failure': function (_0xfde502) { toastr['error'](_0x1a0d('0x51'), _0xfde502['d']); Metronic[_0x1a0d('0x4d')](); }, 'error': function (_0x1088df) { toastr[_0x1a0d('0x4e')](_0x1a0d('0x52'), _0x1088df[_0x1a0d('0x53')]); Metronic[_0x1a0d('0x4d')](); } }); }, 0x96); }; var _0x59ffa = function () { $('#divFilter')[_0x1a0d('0x28')](); $(_0x1a0d('0x54'))[_0x1a0d('0x28')](); var _0x28de03 = _0x1a0d('0x55') + _0x1a0d('0x56') + '<span\x20title=\x22Refresh\x22\x20data-toggle=\x22tooltip\x22\x20data-placement=\x22top\x22><div\x20class=\x22pull-right\x20margin-right-10\x22\x20id=\x22divbtnRefresh\x22\x20>\x20<a\x20href=\x22javascript:;\x22\x20class=\x22toolbtn\x22\x20id=\x22btnRefresh\x22><i\x20class=\x22fa\x20fa-recycle\x22></i></a></div></div>' + _0x1a0d('0x57'); $(_0x1a0d('0x54'))[_0x1a0d('0x29')](_0x28de03); var _0x171ae0 = _0x1a0d('0x58') + _0x1a0d('0x59') + '\x20<div\x20class=\x22col-md-8\x22>\x20<input\x20class=\x22form-control\x20date-picker\x20csDatePicker\x20InputText\x22\x20data-date-format=\x22dd/mm/yyyy\x22\x20size=\x2216\x22\x20type=\x22text\x22\x20id=\x22dtUpdateFromDate\x22\x20value=\x22\x22/>\x20</div>' + _0x1a0d('0x5a') + '\x20<div\x20class=\x22col-md-4\x22><label\x20class=\x22control-label\x22\x20style=\x22text-align:right;\x22>Update\x20To\x20</label>\x20\x20</div>' + _0x1a0d('0x5b') + _0x1a0d('0x5c'); $(_0x1a0d('0x5d'))['append'](_0x171ae0); var _0x4fa918 = new Date(); $(document[_0x1a0d('0x1f')](_0x1a0d('0x20')))['datepicker']('setDate', new Date(_0x4fa918['getFullYear'](), _0x4fa918['getMonth'](), _0x4fa918[_0x1a0d('0x5e')]())); $(document['getElementById'](_0x1a0d('0x22')))[_0x1a0d('0x23')](_0x1a0d('0x5f'), new Date(_0x4fa918['getFullYear'](), _0x4fa918['getMonth'](), _0x4fa918[_0x1a0d('0x5e')]())); _fromlogdate = $(document[_0x1a0d('0x1f')](_0x1a0d('0x20')))[_0x1a0d('0x21')](); _tologdate = $(document[_0x1a0d('0x1f')]('dtUpdateToDate'))[_0x1a0d('0x21')](); }; function _0x47376f() { _lstfilter = []; _0x59ffa(); $(_0x1a0d('0x0'))[_0x1a0d('0x4c')]()['clear']()['draw'](); }; var _0x1beb7a = function () { var _0xaf75aa = _0x1a0d('0x60'); $(_0x1a0d('0x15'))[_0x1a0d('0x28')](); $(_0x1a0d('0x15'))[_0x1a0d('0x29')](_0xaf75aa); $('#tblBodyLogHist')[_0x1a0d('0x28')](); }; return { 'init': function () { _0xeed0b(); } }; }();