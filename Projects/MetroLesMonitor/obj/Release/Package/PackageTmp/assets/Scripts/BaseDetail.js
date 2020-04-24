var BaseDetail = function () {
    var handleBaseDetail = function () {
        $('#header_inbox_bar').css('display', 'none'); $('.menu-toggler.sidebar-toggler').css('display', 'none'); $('#baseBody').removeAttr('style'); $('.top-menu').hide();
        setToolbar();
    };
 
    var setToolbar = function () {
        var _btns = '<span title="Close" data-toggle="tooltip" data-placement="top"><div class="pull-right margin-right-10" id="btnclose"><a id="btnClose" class="toolbtn"><i class="fa fa-times" style="text-align:center;"></i></a></div></span>';
        $('#toolbtngroupdet').append(_btns);
    };




    return { init: function () { handleBaseDetail(); } };
}();