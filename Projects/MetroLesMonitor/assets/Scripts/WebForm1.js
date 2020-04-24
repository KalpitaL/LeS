
var WebForm1 = function () {

    var handleAuditLogTable = function ()
    {
        SetupBreadcrumb('Home', 'Home.aspx', 'Audit', '#', 'Audit Log', 'AuditLog.aspx'); $('#pageTitle').empty().append('Audit Log');
        $(document.getElementById('lnkAuditDet')).addClass('active open'); $(document.getElementById('spAudlog')).addClass('title font-title SelectedColor');
       // setupTableHeader();
        var refreshcheck = '<label><input type="checkbox" class="icheck"  onclick="DoAutoRefresh(this.checked,this.id)" id="chkAuditlog"/> Auto Refresh Page</label>';
        $('#divChkBox').append(refreshcheck); //SetTimerCheckBox('#chkAuditlog', 'chkAuditlog');

        window.onload = SetTimerCheckBox('#chkAuditlog', 'chkAuditlog');
    };

    return { init: function () { handleAuditLogTable(); } };
}();
