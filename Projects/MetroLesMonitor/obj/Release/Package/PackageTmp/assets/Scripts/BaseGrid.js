var BaseGrid = function () {

    var handleBaseGrid = function () {
     //   SetTimerCheckBox();
        setToolbar();
        GetBuyerSupplierList();
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

     
        // $("#dataGrid tbody tr").click(function (e) {
        oTable.on('click', 'tbody tr', function (e){
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

        //$('#chkAutoRefresh').click(function () {
        //    $("#chkAutoRefresh").toggle(this.checked);
        //});

    //    oTable.on('change', 'tbody tr .checkboxes', function () {
    //        $(this).parents('tr').toggleClass("active");
    //});


        //$('#headerTabs').click(function (e)
        //{
        //  //  var target = $(e.target);
        //  //  var tarParent = target.parent();
        //   // $(this).addClass('active').siblings().removeClass('active');//assign css to selected tab,remove current 
        //   // $(this).removeClass('active');
        //    window.location.href = e.target.href;

        //});


    };

    var setToolbar = function () {
        var _btns = '<div class="pull-right margin-right-10" id="btnclose"><a href="/Common/SMHome.aspx" id="btnClose" class="btnred"><i class="fa fa-times"></i></a><div><label>Close</label></div></div>';
        $('#toolbtngroup').empty();
        $('#toolbtngroup').append(_btns);
    };

    var resetToolbar = function () {

        var _access = new accessControl();
        if (_access.accessRights["1001"] == "-1") { $('#btnupdate').hide(); }
        if (_access.accessRights["1002"] == "-1") { $('#btnsignon').hide(); $('#btnsignoff').hide(); }
        $('#btnsignon').data("access", _access.accessRights["1002"]);
    };
     
  
    var GetBuyerSupplierList = function () {
        try {
            var isshow = false;
            if ((sessionStorage['ADDRESSID']) != undefined) {
                var addressid = sessionStorage['ADDRESSID'];
                var addresstype = sessionStorage['ADDRTYPE'];
                var configaddressid = sessionStorage['CONFIGADDRESSID'];
                if (addressid == configaddressid) isshow = true;
                if (isshow) {
                    $('#lnkSuppliers').show();
                    $('#lnkBuyers').show();
                    $('#lnkGroups').show();
                    $('#lnkRules').show();
                    $('#lnkMapping').show();
                    $('#lnkXLS').show();
                    $('#lnkError').show();
                    $('#lnkFileAudit').show();
                    $('#lnkPDF').show();
                    $('#lnkDefaultRules').show();
                    $('#lnkFileAudit').show();
                    $('#lnkOverview').show();
                    $('#lnkLoginHistory').show();
                    $('#lnkFileMonitor').show();
                    $('#lnkInvoice').show();
                    $('#lnkFileReProcess').show();
                    $('#lnkLogFileViewer').show();
                    $('#lnkMailQueue').show();
                    $('#lnkLinkRules').show();
                    $('#lnkSupportMonitor').show();
                }
                else {
                    if (addresstype == "supplier") $('#lnkSuppliers').hide();
                    if (addresstype == "buyer") $('#lnkBuyers').hide();
                    $('#lnkGroups').hide();
                    $('#lnkRules').hide();
                    $('#lnkMapping').hide();
                    $('#lnkXLS').hide();
                    $('#lnkError').hide();
                    $('#lnkFileAudit').hide();
                    $('#lnkPDF').hide();
                    $('#lnkDefaultRules').hide();
                    $('#lnkFileAudit').hide();
                    $('#lnkOverview').hide();
                    $('#lnkLoginHistory').hide();
                    $('#lnkFileMonitor').hide();
                    $('#lnkInvoice').hide();
                    $('#lnkFileReProcess').hide();
                    $('#lnkLogFileViewer').hide();

                    $('#lnkMailQueue').hide();
                    $('#lnkLinkRules').hide();
                    $('#lnkSupportMonitor').hide();

                }

            }
          
        }
        catch (err) { }
    };

  

    return {

        //main function to initiate the module
        init: function () {
            handleBaseGrid();
        }

    };

} ();