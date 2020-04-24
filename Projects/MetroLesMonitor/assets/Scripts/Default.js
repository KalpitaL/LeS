var Default = function () {

    var handleDefaultTable = function () {
        var nEditing = null; var nNew = false;
        $('#divSpacer').remove(); $('#pageTitle').empty(); $('#divFilterWrapper').remove(); $('#divWrapper').hide();  $(document.getElementById('lnkHome')).addClass('active');
        $('#tblHeadRow').hide(); $('#ToolTables_dataGrid_0,#ToolTables_dataGrid_1,#dataGrid_length,#dataGrid_info,#dataGrid_filter,#dataGridAdpt_paginate').hide();    };

     return { init: function () { handleDefaultTable(); } };
}();