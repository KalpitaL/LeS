var AuditGrid = function () {
    var handleAudit = function () {
   
        var _divcontent='<div style="text-align:right;"><span title="Refresh" data-toggle="tooltip" data-placement="top">'+
           //'<button id="btnRefresh" class="toolbtn" onserverclick="txtHeaderChanged" runat="server"><i class="fa fa-recycle" style="text-align:center;"></i></button>' +
           '<asp:Button ID="btnRefresh" runat="server" Text=""  OnClick="txtHeaderChanged"/>'+

           '</span> <span title="Close" data-toggle="tooltip" data-placement="top"><button id="divbtnClose" class="toolbtn"  runat="server"><i class="fa fa-times" style="text-align:center;"></i></button> </span>    </div>';

        $('#pageTitle').empty().append('Audit Log'); $('#toolbtngroup').empty().append(_divcontent);
        SetupBreadcrumb('Home', 'Home.aspx', 'Audit', '#', 'Audit Log', 'AuditLog.aspx');
        var date = new Date();
        $('#txtLogDate').wrap()

    $(document.getElementById('dtLogFromDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
    $(document.getElementById('dtLogToDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
    $('#datefilter').hide();
    $('#btnClose').on('click', function (event) { event.preventDefault(); top.window.close(); });
        //if (document.getElementById("Hidden1").value != '') {
    if ($('#Hidden1').val() != '') {
        //var _list = []; _list = ($('#Hidden1').val().length > 1) ? $('#Hidden1').val().split('|') : null;
        //if (_list != null) {
        //    $(document.getElementById('dtLogFromDate')).val(_list[0]); $(document.getElementById('dtLogToDate')).val(_list[1]);
        //}
    }
    else {
        $(document.getElementById('dtLogFromDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
        $(document.getElementById('dtLogToDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
    }
        
    $('#dtLogFromDate').datepicker().on('change', function (ev) { $('#dtLogFromDate').datepicker('hide'); });
    $('#dtLogToDate').datepicker().on('changeDate', function (ev) {$('#dtLogToDate').datepicker('hide'); });

    $('#txtLogDate').on('click', function (ev) { $('#datefilter').show(); });

};

function GetValues() {
    var fromdate = $('#dtLogFromDate').val();  var todate = $('#dtLogToDate').val();  document.getElementById("Hidden1").value = fromdate + '|' + todate;
};
function ClearValues() {
    $(document.getElementById('dtLogFromDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
    $(document.getElementById('dtLogToDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));document.getElementById("Hidden1").value = ''; 
};

    return { init: function () { handleAudit(); }};
}();

