var _div = ''; var _lstserverdet = []; var _lstserverlnk = []; var _addrid = ''; var _serverTime = ''; var _logoservName = ''; var body = ''; var _loginName = ''; var _password = '';
var hour, hourTemp, minute, minuteTemp, second, secondTemp, monthnumber, monthnumberTemp, monthday, monthdayTemp, year, ap;
var Params = {}; var serverName = ''; var _isparent='';
var Base = function () {
    var handleBase = function () {
        commonSettings();
        $('#iLogOut').click(function (e) { e.stopPropagation(); LogoutClick('false'); });
        if (_isparent != '' && _isparent != undefined) {
            if (_isparent.toUpperCase() == 'TRUE') {
                FillServers();
                $('#cbServerList').on("change", function (e) {
                    _addrid = Str(sessionStorage.getItem('ADDRESSID')) + '||' + Str(sessionStorage.getItem('USERID')) + '#1';
                    var selectedtxt = $('#cbServerList option:selected').text(); var _servSelectval = $('#cbServerList option:selected').val();
                    var _logindet = 'LoginName=' + _loginName + '&Password=' + _password + '&Server=' + selectedtxt;
                    var selectedval = _servSelectval + '?DETAILS=' + getEncrypted_URL(_logindet);
                    if (Str(selectedtxt) != '') {
                        window.open(selectedval);
                        return false;
                    }
                });
            }
            else { $('.serverlabel').css('display', 'none'); $('#cbServerList').css('display', 'none'); }
        };
        if ($('#baseDetail').length) { CommonSettings_BaseDetail(); }
        $('body').on('click', '.sidebar-toggler', function (e) {
            if (body.hasClass("page-sidebar-closed")) { $('#Lestitle').removeClass('titledisplay'); $("#dvLogo").css("width", "255px"); }
            else { $('#Lestitle').addClass('titledisplay'); $("#dvLogo").css("width", ""); }
        });
       
    };

    var CommonSettings_BaseDetail = function () {
        $('#header_inbox_bar').css('display', 'none'); $('.menu-toggler.sidebar-toggler').css('display', 'none'); $('#baseBody').removeAttr('style'); $('.top-menu').hide();
        var _btns = '<span title="Close" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="btnclose"><a id="btnClose" class="toolbtn"><i class="fa fa-times" style="text-align:center;"></i></a></div></span>';
        $('#toolbtngroupdet').append(_btns);
    };

    var displayTime = function () {
        var localTime = new Date(); var year = localTime.getYear(); var month = localTime.getMonth() + 1; var date = localTime.getDate();
        var hours = localTime.getHours(); var minutes = localTime.getMinutes(); var seconds = localTime.getSeconds();
        _serverTime = year + "-" + month + "-" + date + " " + hours + ":" + minutes + ":" + seconds;
        return _serverTime;
    };
    function commonSettings() {
        GetServer_SessionDetails(); $('#lblServertime').text(GetServerDate());
        var _verdet = GetVersionDetails().split('|');
        $('#lblVersion').text(_verdet[0]); _isparent = _verdet[1];
        var username = (sessionStorage['USERNAME']); $('#UserName').text(username);  _loginName = (sessionStorage['LOGIN_NAME']);  _password = (sessionStorage['PASSWORD']);
        $('#baseBody').fadeIn('fast'); body = $('body');
        _logoservName = Str(sessionStorage.getItem('LOGO_SERVER_NAME'));
        serverName = (Str(sessionStorage.getItem('SERVERNAME')) == '') ? _logoservName : Str(sessionStorage.getItem('SERVERNAME'));
        $('#Lestitle').text('LeSConnect Exchange' + ' (' + _logoservName + ')');
    };
    function FillServers() {
            $.ajax({
                type: "POST",
                async: false,
                url: "../Common/Default.aspx/GetServerList",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var _server = Str(response.d).split(',');
                        _lstserverdet = [];
                        for (i = 0; i < _server.length; i++) {
                            var _servname = _server[i].split('|')[0]; var _servlnk = _server[i].split('|')[1];
                            _lstserverdet.push(Str(_servlnk) + '|' + Str(_servname));
                        }
                        $('#cbServerList').empty().append(FillCombo(serverName, _lstserverdet));                        
                    }
                    catch (err) { toastr.error('Error in Server List :' + err, "Lighthouse eSolutions Pte. Ltd"); }
                },
                failure: function (response) { toastr.error("Failure in Server List", "Lighthouse eSolutions Pte. Ltd"); },
                error: function (response) { toastr.error("Error in Server List", "Lighthouse eSolutions Pte. Ltd"); }
            });
    };
   
    var FillCombo =function (val,_lst) {
        var opt = ''; var _select = '';
        try {
            if (_lst.length != undefined && _lst.length > 0) {
                for (var i = 0; i < _lst.length; i++) {
                    var cdet = _lst[i].split("|");
                    if (val != "" && val == Str(cdet[1]))
                        opt += '<option selected value="' + Str(cdet[0]) + '">' + Str(cdet[1]) + '</option>';
                    else
                        opt += '<option value="' + Str(cdet[0]) + '">' + Str(cdet[1]) + '</option>';
                }
                return opt;
            }
        }
        catch (err) {
            toastr.error('Error while populating List :' + err, "Lighthouse eSolutions Pte. Ltd");
        }
    };

    function getEncryptedData(strUrl) {
        var _result = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "../Common/Default.aspx/EncryptURL",
            data: "{'ORG_URL':'" + strUrl + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try { _result = response.d; }
                catch (err) { toastr.error('Error in Getting encrypted Url :' + err, "Lighthouse eSolutions Pte. Ltd"); }
            },
            failure: function (response) { toastr.error("Failure in Getting encrypted Url", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Getting encrypted Url", "Lighthouse eSolutions Pte. Ltd"); }
        });
        return _result;
    };

    function getTitleDecryptedData() {
        var KEYURL = ''; var _result = '';
        var ind = window.location.href.indexOf('?');
        if (ind > -1) {
            KEYURL = window.location.href.split('?')[1];
            $.ajax({
                type: "POST",
                async: false,
                url: "../Common/Default.aspx/DecryptURL",
                data: "{'KEYURL':'" + Str(KEYURL) + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try { _result = Str(response.d); }
                    catch (err) { toastr.error('Error in Getting decrypted Url :' + err, "Lighthouse eSolutions Pte. Ltd"); }
                },
                failure: function (response) { toastr.error("Failure in Getting decrypted Url", "Lighthouse eSolutions Pte. Ltd"); },
                error: function (response) { toastr.error("Error in Getting decrypted Url", "Lighthouse eSolutions Pte. Ltd"); }
            });
        }
        return _result;
    };

    function getEncrypted_URL(strUrl) {
        var _result = '';
        $.ajax({
            type: "POST",
            async: false,
            url: "../Common/Default.aspx/Encrypt_ServerURL",//Encrypt
            data: "{'ORG_URL':'" + strUrl + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try { _result = response.d; }
                catch (err) { toastr.error('Error in Getting encrypted Url :' + err, "Lighthouse eSolutions Pte. Ltd"); }
            },
            failure: function (response) { toastr.error("Failure in Getting encrypted Url", "Lighthouse eSolutions Pte. Ltd"); },
            error: function (response) { toastr.error("Error in Getting encrypted Url", "Lighthouse eSolutions Pte. Ltd"); }
        });
        return _result;
    };

    //function updateTime() { var serverTime = GetServerDate(); $('#lblServertime').text(serverTime); };
    //function setVersion() { var _version = GetVersionDetails(); $('#lblVersion').text(_version.split('|')[0]); _isparent = _version.split('|')[1]; };

    return {
        init: function () { handleBase(); }
    };
}();