var gValidate = ''; var _urlencrypt = '';
var Login = function () {
    var handleLogin = function () {
        $('#validate').hide(); var _result =  SetBlockedPage();
        if (_result != '') {
            $('#btnLogin').click(function (e) {
                e.preventDefault(); $('.label-danger', $('#logcontent')).fadeOut();

                var cValidate = ValidateUser($('#input-Username').val(), $('#input-Password').val());
                if (cValidate == '') {
                    SetSessionValues(); var _url = window.location.href; if (_url.indexOf('?') > -1) { _urlencrypt = _url.substring(_url.lastIndexOf('?'), _url.length); }
                    location.href = '../LESMonitorPages/Adaptors.aspx' + _urlencrypt;
                }
                else { $('#validate').fadeIn(); $('.label-danger', $('#logcontent')).fadeIn(); }
            });
            $('#logcontent input').keypress(function (e) { if (e.which == 13) { $('#btnLogin').click(); return false; } });  }
        else { location.href = '../LESMonitorPages/ForbiddenAccess.aspx'; }
    };
    var ValidateUser=function(Email,password)
    {
        var ckUser = ''; if (Email == '' || password == '') ckUser = 'Enter Username and Password.';
        else {
            $.ajax({
                type: "POST", async: false, url: "../Common/Login.aspx/DoLogin", data: "{ 'cEmail' : '" + Email + "' , 'cPassword' : '" + password + "' }", contentType: "application/json; charset=utf-8", dataType: "json", success: function (response) {
                    ckUser = response.d.toString();
                }, failure: function (response) { ckUser = 'Failed to validate Username'; }, error: function (response) { ckUser = 'Unable to validate Username'; }
            });
        }
        return ckUser;
    };
  
return { init: function () { handleLogin(); } };
}();