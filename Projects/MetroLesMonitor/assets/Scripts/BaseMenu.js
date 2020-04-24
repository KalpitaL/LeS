var BaseMenu = function () {

    var handleBaseMenuGrid = function () {
        GetBuyerSupplierList();
        oTable = $('#dataGrid');

        if (!$('#baseBody').hasClass("page-sidebar-closed")) { $('#Lestitle').removeClass('titledisplay'); $("#dvLogo").css("width", "255px"); }
        else {
            $('#Lestitle').addClass('titledisplay'); $("#dvLogo").css("width", "");
        }

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

    function ResetTabs() {
        $('#lnkHome').hide(); $('#lnkAdaptors').hide(); $('#lnkSuppliers').hide(); $('#lnkBuyers').hide(); $('#lnkInvoice').hide(); $('#lnkInvoicePDFCnfg').hide();
        $('#lnkGroups').hide(); $('#lnkFormat').hide(); $('#lnkRules').hide(); $('#lnkDefaultRules').hide(); $('#lnkLinkRules').hide(); $('#lnkMapping').hide();
        $('#lnkXLS').hide(); $('#lnkPDF').hide(); $('#lnkAuditLog').hide(); $('#lnkError').hide(); $('#lnkFileMonitor').hide(); $('#lnkLogFileViewer').hide();
        $('#lnkMailQueue').hide(); $('#lnkFileReProcess').hide(); $('#lnkOverview').hide(); $('#lnkLoginHistory').hide();
    };

    var GetBuyerSupplierList = function () {
        try {
            var isShow = false; var addressid = Str(sessionStorage['ADDRESSID']); var addresstype = Str(sessionStorage['ADDRTYPE']);
            var configaddressid = Str(sessionStorage['CONFIGADDRESSID']);
            var _tabslist = TabDetails();
            if (_tabslist != null) {
                ResetTabs();
                if ((sessionStorage['ADDRESSID']) != undefined) {
                    if (addressid == configaddressid) isShow = true;
                    for (var k = 0; k < _tabslist.length; k++) {
                        var tabname = _tabslist[k].toUpperCase();
                        if (tabname == "HOME") { if (isShow) { $('#lnkHome').show(); } else { $('#lnkHome').hide(); } }
                        else if (tabname == "ADAPTOR") { if (isShow) { $('#lnkAdaptors').show(); } else { $('#lnkAdaptors').hide(); } }
                        else if (tabname == "SUPPLIER") { if (isShow) { $('#lnkSuppliers').show(); } else { if (Str(addresstype).toUpperCase() == "SUPPLIER") { $('#lnkSuppliers').hide(); } } }
                        else if (tabname == "BUYER") { if (isShow) { $('#lnkBuyers').show(); } else { if (Str(addresstype).toUpperCase() == "BUYER") { $('#lnkBuyers').hide(); } } }
                        else if (tabname == "INVOICELINK") { if (isShow) { $('#lnkInvoice').show(); } else { $('#lnkInvoice').hide(); } }
                        else if (tabname == "INVOICECONFIG") { if (isShow) { $('#lnkInvoicePDFCnfg').show(); } else { $('#lnkInvoicePDFCnfg').hide(); } }
                        else if (tabname == "GROUPS") { if (isShow) { $('#lnkGroups').show(); } else { $('#lnkGroups').hide(); } }
                        else if (tabname == "DOCUMENTFORMAT") { if (isShow) { $('#lnkFormat').show(); } else { $('#lnkFormat').hide(); } }
                        else if (tabname == "RULES") { if (isShow) { $('#lnkRules').show(); } else { $('#lnkRules').hide(); } }
                        else if (tabname == "DEFAULTRULES") { if (isShow) { $('#lnkDefaultRules').show(); } else { $('#lnkDefaultRules').hide(); } }
                        else if (tabname == "LINKRULES") { if (isShow) { $('#lnkLinkRules').show(); } else { $('#lnkLinkRules').hide(); } }
                        else if (tabname == "FILEMAPPING") { if (isShow) { $('#lnkMapping').show(); } else { $('#lnkMapping').hide(); } }
                        else if (tabname == "XLSCONFIG") { if (isShow) { $('#lnkXLS').show(); } else { $('#lnkXLS').hide(); } }
                        else if (tabname == "PDFCONFIG") { if (isShow) { $('#lnkPDF').show(); } else { $('#lnkPDF').hide(); } }
                        else if (tabname == "AUDITLOG") { if (isShow) { $('#lnkAuditLog').show(); } else { $('#lnkAuditLog').hide(); } }
                        else if (tabname == "FILEAUDIT") { if (isShow) { $('#lnkFileAudit').show(); } else { $('#lnkFileAudit').hide(); } }
                        else if (tabname == "ERRORS") { if (isShow) { $('#lnkError').show(); } else { $('#lnkError').hide(); } }
                        else if (tabname == "FILEMONITOR") { if (isShow) { $('#lnkFileMonitor').show(); } else { $('#lnkFileMonitor').hide(); } }
                        else if (tabname == "LOGVIEWER") { if (isShow) { $('#lnkLogFileViewer').show(); } else { $('#lnkLogFileViewer').hide(); } }
                        else if (tabname == "MAILVIEWER") { if (isShow) { $('#lnkMailQueue').show(); } else { $('#lnkMailQueue').hide(); } }
                        else if (tabname == "FILEREPROCESSING") { if (isShow) { $('#lnkFileReProcess').show(); } else { $('#lnkFileReProcess').hide(); } }
                        else if (tabname == "OVERVIEW") { if (isShow) { $('#lnkOverview').show(); } else { $('#lnkOverview').hide(); } }
                        else if (tabname == "LOGINHISTORY") { if (isShow) { $('#lnkLoginHistory').show(); } else { $('#lnkLoginHistory').hide(); } }
                    }
                    if ((document.getElementById('lnkSuppliers').style.display == 'none') && (document.getElementById('lnkBuyers').style.display == 'none')) { $('#lnkSuppBuy').hide(); }
                    if ((document.getElementById('lnkFileMapping').style.display == 'none') && (document.getElementById('lnkXLS').style.display == 'none') && (document.getElementById('lnkPDF').style.display == 'none')) { $('#lnkMapping').hide(); }
                    if ((document.getElementById('lnkAuditLog').style.display == 'none') && (document.getElementById('lnkFileAudit').style.display == 'none') && (document.getElementById('lnkPDF').style.display == 'none')) { $('#lnkAuditDet').hide(); }
                    if ((document.getElementById('lnkRules').style.display == 'none') && (document.getElementById('lnkDefaultRules').style.display == 'none') && (document.getElementById('lnkLinkRules').style.display == 'none')) { $('#lnkRuledet').hide(); }
                    if (document.getElementById('lnkFileMonitor').style.display == 'none') { $('#lnkMonitor').hide(); }
                    if ((document.getElementById('lnkLogFileViewer').style.display == 'none') && (document.getElementById('lnkMailQueue').style.display == 'none')) { $('#lnkViewer').hide(); }
                    if ((document.getElementById('lnkInvoice').style.display == 'none') && (document.getElementById('lnkInvoicePDFCnfg').style.display == 'none')) { $('#lnkeInvoice').hide(); }
                }
            }
        }
        catch (err) { }
    };

    return {

        //main function to initiate the module
        init: function () {
            handleBaseMenuGrid();
        }

    };

} ();