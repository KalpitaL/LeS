var TabGrid= function () {

    var handleTabGrid = function () {
        setToolbar();

        oTable = $('#dataGrid');
        //oTable.DataTable();
        //oTable.find('.group-checkable').change(function () {
        $('#cbheader').change(function () {
            var set = jQuery(this).attr("data-set");
            var checked = jQuery(this).is(":checked");
            var oTT = TableTools.fnGetInstance('dataGrid');
            oTT.fnDeselect();
            if (checked) { $('#ToolTables_DataGrid_0').click(); }
            else { $('#ToolTables_DataGrid_1').click(); }
            jQuery(set).each(function () {
                if (checked) {
                    $(this).attr("checked", true);
                    $(this).parents('tr').addClass("active");
                } else {
                    $(this).attr("checked", false);
                    $(this).parents('tr').removeClass("active");
                }
            });
            jQuery.uniform.update(set);
        });

        oTable.on('click', 'tbody tr', function (e) {
            e.stopPropagation();
            if (e.target.type == "checkbox") {

                // stop the bubbling to prevent firing the row's click event
                var $checkbox = $(this).find(':checkbox');
                if ($checkbox.is(':checked')) {

                    $(this).addClass("DTTT_selected selected");
                }
                else $(this).removeClass("DTTT_selected selected");
                // $(this).toggleClass("active");
            } else {
                var $checkbox = $(this).find(':checkbox');
                $checkbox.attr('checked', !$checkbox.attr('checked'));
                if ($checkbox.is(':checked')) {

                    $(this).addClass("DTTT_selected selected");
                }
                else $(this).removeClass("DTTT_selected selected");
            }
        });
    };

    var setToolbar = function () {
        var _btns = '<div class="pull-right margin-right-10" id="btnclose"><a href="../Common/SMHome.aspx" id="btnClose" class="btnred"><i class="fa fa-times"></i></a><div><label>Close</label></div></div>';
        $('#toolbtngroup').empty();
        $('#toolbtngroup').append(_btns);
    };

    var resetToolbar = function () {

        var _access = new accessControl();
        if (_access.accessRights["1001"] == "-1") { $('#btnupdate').hide(); }
        if (_access.accessRights["1002"] == "-1") { $('#btnsignon').hide(); $('#btnsignoff').hide(); }
        $('#btnsignon').data("access", _access.accessRights["1002"]);
    };


    return {

        //main function to initiate the module
        init: function () {
            handleTabGrid();
        }

    };

}();