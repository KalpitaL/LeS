
var sessServerAliveTime = 10 * 60 * 2;//always ping server to check session Alive or not
var sessionTimeout = 1 * 60000;//Setting session timeout configuration
var sessLastActivity;
var idleTimer, remainingTimer;
var isTimout = false;
var valCounter = null;
var ckTimeoutVal = 0;
var sess_intervalID, idleIntervalID;
var sess_lastActivity;
var timer;
var isIdleTimerOn = false;
localStorage.setItem('sessionSlide', 'isStarted');

function initSessionMonitor() {
    $(document).bind('keypress.session', function (ed, e) { sessKeyPressed(ed, e);});
    $(document).bind('mousedown keydown', function (ed, e){ sessKeyPressed(ed, e);});
    sessServerAlive(); startIdleTime();
}

function sessPingServer() { if (!isTimout) { return true; }}

function sessServerAlive() {
    sess_intervalID = setInterval('sessPingServer()', sessServerAliveTime);// sessPingServer Method for Ping to sever using Ajax Call.
}

$(window).scroll(function (e) {
    localStorage.setItem('sessionSlide', 'isStarted'); startIdleTime();
});

function sessKeyPressed(ed, e) {
    var target = ed ? ed.target : window.event.srcElement;
    var sessionTarget = $(target).parents("#session-expire-warning-modal").length;

    if (sessionTarget != null && sessionTarget != undefined && ed.currentTarget.activeElement!=null) {
        if (ed.target.id != "btnSessionExpiredCancelled" && ed.target.id != "btnSessionModal" && ed.currentTarget.activeElement.id != "session-expire-warning-modal" &&
            ed.target.id != "btnExpiredOk" && ed.currentTarget.activeElement.className != "modal fade modal-overflow in" &&
            ed.currentTarget.activeElement.className != 'modal-header' && sessionTarget != 1 && ed.target.id != "session-expire-warning-modal") {
            localStorage.setItem('sessionSlide', 'isStarted');
            startIdleTime();
        }
    }
}

function startIdleTime() {
    stopIdleTime();
    localStorage.setItem("sessIdleTimeCounter", $.now());
    idleIntervalID = setInterval('checkIdleTimeout()', 1000);
    isIdleTimerOn = false;
}

var sessionExpired = document.getElementById("session-expired-modal");
function sessionExpiredClicked(evt) {window.location = "../Common/Logout.aspx";}

function LoginSession() {
    if (valCounter == $("#txtValidateCounter").val()) { location.reload(true);} else {  alert('Code mismatch'); }
}

function LogoutSession() {
    window.location = "../Common/Logout.aspx?LOGOUT";
}

function stopIdleTime() {
    clearInterval(idleIntervalID);  clearInterval(remainingTimer);
}

function checkIdleTimeout() {
    var idleTime = (parseInt(localStorage.getItem('sessIdleTimeCounter')) + (sessionTimeout)); 
    if ($.now() > idleTime + 120000) {
        if (ckTimeoutVal <= 1) {
            $("#session-expire-warning-modal").modal('hide');
            $("#session-expired-modal").modal('show');
            clearInterval(sess_intervalID);
            clearInterval(idleIntervalID);

            $('.modal-backdrop').css("z-index", parseInt($('.modal-backdrop').css('z-index')) + 100);
            $("#session-expired-modal").css('z-index', 2000);
            $('#btnExpiredOk').css('background-color', '#428bca');
            $('#btnExpiredOk').css('color', '#fff');
            isTimout = true;
            window.location = "../Common/Logout.aspx";
        }
        else
        {
            var sval = ckTimeoutVal;
            sessionTimeout = sval * 60000;
            $("#session-expire-warning-modal").modal('hide');
            $("#session-expired-modal").modal('hide');
            startIdleTime();
        }
    }
    else if ($.now() > idleTime) {
        if (!isIdleTimerOn) {
            CheckSessionValue();
            if (ckTimeoutVal <= 1) {
                localStorage.setItem('sessionSlide', false);
                countdownDisplay();

                $('.modal-backdrop').css("z-index", parseInt($('.modal-backdrop').css('z-index')) + 500);
                $('#session-expire-warning-modal').css('z-index', 1500);
                $('#btnOk').css('background-color', '#428bca');
                $('#btnOk').css('color', '#fff');
                $('#btnSessionExpiredCancelled').css('background-color', '#428bca');
                $('#btnSessionExpiredCancelled').css('color', '#fff');
                $('#btnLogoutNow').css('background-color', '#428bca');
                $('#btnLogoutNow').css('color', '#fff');

                $("#seconds-timer").empty();
                $("#session-expire-warning-modal").modal('show');
                valCounter = Math.floor(100000 + Math.random() * 900000);
                span = document.getElementById("uCounter");
                span.innerHTML = valCounter;
                $("#txtValidateCounter").val('');
                isIdleTimerOn = true;
            }
            else {
                var sval = ckTimeoutVal;
                sessionTimeout = sval * 60000;
                startIdleTime();
            }
        }
    }
}

$("#btnSessionExpiredCancelled").click(function () {
    $('.modal-backdrop').css("z-index", parseInt($('.modal-backdrop').css('z-index')) - 500);
});

$("#btnOk").click(function () {
    $("#session-expire-warning-modal").modal('hide');
    $('.modal-backdrop').css("z-index", parseInt($('.modal-backdrop').css('z-index')) - 500);
    startIdleTime();
    clearInterval(remainingTimer);
    localStorage.setItem('sessionSlide', 'isStarted');
});

$("#btnLogoutNow").click(function () {
    localStorage.setItem('sessionSlide', 'loggedOut');
    window.location = "../Common/Logout.aspx?Logout";
    $("#session-expired-modal").modal('hide');
});
$('#session-expired-modal').on('shown.bs.modal', function () {
    $("#session-expire-warning-modal").modal('hide');
    $(this).before($('.modal-backdrop'));
    $(this).css("z-index", parseInt($('.modal-backdrop').css('z-index')) + 1);
});

$("#session-expired-modal").on("hidden.bs.modal", function () {
    window.location = "../Common/Logout.aspx";
});
$('#session-expire-warning-modal').on('shown.bs.modal', function () {
    $("#session-expire-warning-modal").modal('show');
    $(this).before($('.modal-backdrop'));
    $(this).css("z-index", parseInt($('.modal-backdrop').css('z-index')) + 1);
});


function countdownDisplay() {

    var dialogDisplaySeconds = 120;

    remainingTimer = setInterval(function () {
        if (localStorage.getItem('sessionSlide') == "isStarted") {
            $("#session-expire-warning-modal").modal('hide');
            startIdleTime();
            clearInterval(remainingTimer);
        }
        else if (localStorage.getItem('sessionSlide') == "loggedOut") {         
            $("#session-expire-warning-modal").modal('hide');
            $("#session-expired-modal").modal('show');
        }
        else {

            $('#seconds-timer').html(dialogDisplaySeconds);
            dialogDisplaySeconds -= 1;
        }
    }
    , 1000);
};


function CheckSessionValue() {
    try{
        jQuery.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            async: false,
            url: '../Common/Default.aspx/GetSessionData',
            contentType: 'application/json; charset=uts-8',
            success: function (ds) {
                ckTimeoutVal = ds.d;
            },
            error: function (ds) {
            alert(ds.responseText);
        }
        });
    }
    catch (e) {
        aler(e.message);
    }
}

function sessLogOut() {
    
}
