var _div = '';var _lstservername = [];var _lstserverlnk = [];var _addrid = '';
var Home = function () {
    var handleHomeTable = function () {
        SetupBreadcrumb('Home', 'Home.aspx', '', '', '', ''); $('#pageTitle').empty().append('Home'); var username = (sessionStorage['USERNAME']); //$('#UserName').text(username);
        $(document.getElementById('lnkHome')).addClass('active'); $('#toolbtngroup').empty();
    };
  
     return {  init: function () { handleHomeTable(); } };
}();