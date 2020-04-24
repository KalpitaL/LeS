var _fromlogdate = ''; var _tologdate = ''; var filetype = ''; var _lstformats = []; var _MappFileName = ''; var _RefFileName = ''; var _templateFileName = ''; var _lstfiles = [];
var _siteurl = ''; var _refrencefiles = ''; var _fMppdet = []; 
var FileMapping = function () {

    var handleFileMappingTable = function () {
        setTimeout(function () { CommonSettings(), 200 });   
        $('#divChkBox').hide(); $('[data-toggle="tooltip"]').tooltip(); disableControls('0');
        $('#cbFileType').on("change", function (e) {
            var selectedval = $('#cbFileType option:selected').text().trim();
            if (Str(selectedval) != '') {
                $('.ajax__fileupload_selectFileContainer').fadeIn(); $('.ajax__fileupload_topFileStatus').fadeIn();
                GetGroupFormats(selectedval);
            }
            else { disableControls('0'); }
        });
        $('#cbGroup').on("change", function (e) { var groupname = $('#cbGroup option:selected').text().trim(); if (Str(groupname) != '') { $('#btnDownload').addClass('btn-circle_active'); } else { $('#btnDownload').removeClass('btn-circle_active'); } });
        $('#btnDownload').on("click", function (e) { DownloadFile(); });
        $('#btnCancelUpload').on("click", function (e) { ClearControls(); });
        $('#btnUploadFiles').on("click", function (e) {
            var filetype = $('#cbFileType option:selected').text().trim();
            if (filetype != '') {
                var _mppfiles = $('input[type="file"]').get(0).files;
                var _reffiles = $('input[type="file"]').get(1).files;
                var _lst = [];
                if (_mppfiles.length > 0 && _reffiles.length > 0) { UploadFiles(); }
                else { toastr.error("Please select both Mapping and Reference Files ", "Lighthouse eSolutions Pte. Ltd"); }
            }
            else { toastr.error("Please select File Type ", "Lighthouse eSolutions Pte. Ltd"); }
        });
        $("#ModalDownload").on('shown.bs.modal', function () {  DownloadReferenceFile(_refrencefiles); disableControls('1'); });
        $('#btnDownloadFileTemp').on("click", function (e) {
            _lstfiles = []; _lstfiles.push(_siteurl + '/Downloads/' + _templateFileName);//_lstfiles.push('http://' + _siteurl + '/Downloads/' + _templateFileName);
            $('input[type=checkbox]:checked').each(function () {
                var chkid = $(this)[0].id;
                var _fname = $("label[for='" + chkid + "']").text(); _lstfiles.push(_siteurl + '/Downloads/' + _fname);//'http://'+
            });
            for (var k = 0; k < _lstfiles.length; k++) {
                var link = document.createElement('a'); document.body.appendChild(link); link.href = _lstfiles[k]; link.target = '_blank'; link.click();
            }
        });
    };   

    function CommonSettings() {
        var nEditing = null; var nNew = false; $('#pageTitle').empty().append('File Mapping');
        SetupBreadcrumb('Home', 'Home.aspx', 'Mapping', '#', 'File Mapping', 'File_Mapping.aspx'); $('#toolbtngroup').empty();
        $(document.getElementById('lnkMapping')).addClass('active open'); $(document.getElementById('spFilemapp')).addClass('title font-title SelectedColor');
    };

    function disableControls(isdisplay){
        if (isdisplay == '0') { $('#cbGroup').empty();} else { }   $('#btnDownload').addClass('btn-circle_disabled');
    };

    function FillGroupFormats(_lstformats) {
        var optionFormats = '';
        try {  optionFormats = '<option value="-1"> </option>';
            if (_lstformats.length != undefined && _lstformats.length > 0) {
                for (var i = 0; i < _lstformats.length; i++) {
                    var cformats = _lstformats[i].split("|");   optionFormats += '<option value="' + Str(cformats[0]) + '">' + Str(cformats[1]) + '</option>';
                }
            }
        }
        catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd",'Error in Group Format List :' + err);}
        $('#cbGroup').empty().append(optionFormats);
    };

    var GetGroupFormats = function (FILETYPE) {        
        $.ajax({
            type: "POST",
            async: false,
            url: "File_Mapping.aspx/FillGroups",
            data: "{ 'FILETYPE':'" + FILETYPE +"'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try {
                    var DataSet = JSON.parse(response.d);
                    if (DataSet.NewDataSet != null) {
                        Table = DataSet.NewDataSet.Table; var _arr = []; _lstformats = _arr;
                        if (Table.length != undefined) {
                            for (var i = 0; i < Table.length; i++) {
                                if (Str(FILETYPE) == "XLS") { _lstformats.push(Str(Table[i].EXCEL_MAPID) + "|" + Str(Table[i].GROUP_MAPCODE).toUpperCase()); }
                                else if (Str(FILETYPE) == "PDF") { _lstformats.push(Str(Table[i].GROUP_ID) + "|" + Str(Table[i].GROUP_MAPCODE).toUpperCase()); }
                                else if (Str(FILETYPE) == "VOUCHER_PDF") { _lstformats.push(Str(Table[i].INV_PDF_MAPID) + "|" + Str(Table[i].INV_MAP_CODE).toUpperCase()); }
                            }
                        }
                        else {
                            if (Table.GROUP_ID != undefined) {
                                if (Str(FILETYPE) == "XLS") { _lstformats.push(Str(Table.EXCEL_MAPID) + "|" + Str(Table.GROUP_MAPCODE).toUpperCase());}
                                else if (Str(FILETYPE) == "PDF") { _lstformats.push(Str(Table.GROUP_ID) + "|" + Str(Table.GROUP_MAPCODE).toUpperCase()); }
                                else if (Str(FILETYPE) == "VOUCHER_PDF") { _lstformats.push(Str(Table.INV_PDF_MAPID) + "|" + Str(Table.INV_MAP_CODE).toUpperCase()); }
                            }
                        }
                    }
                    FillGroupFormats(_lstformats);
                }
                catch (err) { toastr.error("Lighthouse eSolutions Pte. Ltd.",'Please check Group List :' + err); }
            },
            failure: function (response) { toastr.error("failure get", response.d); },
            error: function (response) { toastr.error("error get", response.responseText);  }
        });
    };

    function DownloadFile() {
        var filetype = $('#cbFileType option:selected').text().trim(); var groupid = $('#cbGroup option:selected').val(); var groupname = $('#cbGroup option:selected').text().trim();
        if (filetype != '' && groupname != '') { DownloadFileTemplate(filetype, groupid, groupname); }
    };

    function DownloadFileTemplate(FILETYPE, GROUPID, GROUPNAME) {
        _refrencefiles = '';
        try { 
           $.ajax({
                type: "POST",
                async: false,
                url: "File_Mapping.aspx/DownloadGroupFormat",
                data: "{ 'FILETYPE':'" + FILETYPE + "','GROUPID':'" + GROUPID + "','GROUPNAME':'" + GROUPNAME + "' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var res = Str(response.d);
                        if (res != "") {
                            var _tfiledet = res.split('#')[0]; var _reffile = res.split('#')[1];
                            _refrencefiles = _reffile.split('$$')[0]; _siteurl = _reffile.split('$$')[1];
                            var filefullpath = _tfiledet.split('|')[0]; _templateFileName = _tfiledet.split('|')[1]; //top.location.href = "../Downloads/" + filename;
                            if (_refrencefiles != '') {
                                $('#ModalDownload').modal('show');
                            }
                            else { top.location.href = "../Downloads/" + _templateFileName; }
                        }
                        else { toastr.error("Lighthouse eSolutions Pte. Ltd", "No Mapping found"); }
                    }
                    catch (err) {// toastr.error("Lighthouse eSolutions Pte. Ltd.", 'Download File :' + err);
                    }
                },
                failure: function (response) {   toastr.error("failure get", response.d); },
                error: function (response) { toastr.error("error get", response.responseText);}
            });
        }
        catch (e)
        { }
    };

    function DownloadReferenceFile(_reffile) {
        var _det = ''; var _lstref = [];
        if(_reffile!=undefined && _reffile.length > 0)
        {
            _lstref = _reffile.split(',');
            _det = '<div style="overflow-y:scroll;"><ul>';
            for (var i = 0; i < _lstref.length; i++)
            {
                var _chkid = 'chkFile' + (i + 1);
                _det += '<li><label class="checkbox filename-width" for="' + _chkid + '"><input type="checkbox" id="' + _chkid + '" value="' + (i + 1) + '">' + _lstref[i] + '</label></li>';
            }
        }
        $('#prtDownload').empty().append(_det + '</ul><div>');
    };

    function UploadFiles() {
        var FILETYPE = $('#cbFileType option:selected').text();
        if (FILETYPE != '') {
            $.ajax({
                type: "POST",
                async: false,
                url: "File_Mapping.aspx/UploadFileMapping",
                data: "{'FILETYPE':'" + FILETYPE + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        result = response.d; if (Str(result) != "") { toastr.success(result, "Lighthouse eSolutions Pte. Ltd"); }
                        else { toastr.error('Mapping values are not in correct format. Please Check file and upload it again.', "Lighthouse eSolutions Pte. Ltd"); }
                        ClearControls();
                    }
                    catch (err) { toastr.error('Error in File Upload :' + err, "Lighthouse eSolutions Pte. Ltd"); result = -1; }
                },
                failure: function (response) { toastr.error("Failure in File Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); result = -1; },
                error: function (response) { toastr.error("Error in File Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); result = -1; }
            });
        }
        else { toastr.error('Please select the File Type.', "Lighthouse eSolutions Pte. Ltd"); }
    };

     return {
         init: function () { handleFileMappingTable(); }
    };
}();

function GetURL() {
    $.ajax({
        type: "POST",
        async: false,
        url: "File_Mapping.aspx/GetURLdetails",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            try {
                result = response.d; alert(result);
            }
            catch (err) { ;}
        },
        failure: function (response) { toastr.error("Failure in Starting Client Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); result = -1; },
        error: function (response) { toastr.error("Error in Starting Client Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); result = -1; }
    });
};

function UploadComplete(sender, args) {
    var AsyncFileUpload = sender._element.id; var txts = sender._element.getElementsByTagName("input");
    if (_fMppdet.indexOf(AsyncFileUpload) == -1) {
        _fMppdet.push(Str(sender._element.id), txts);
    }   
};

function ClearControls() {
    $('#cbFileType').val(''); $('#cbGroup').val(''); $('#btnDownload').addClass('btn-circle_disabled');
    if (_fMppdet.length > 0) {
        for (var i = 0; i < _fMppdet.length; i++) {
            var txts = _fMppdet[i];
            for (var j = 0; j < txts.length; j++) {
                if (txts[j].type == 'file' || txts[j].type == 'hidden') {
                    txts[j].value = ''; txts[j].style.backgroundColor = "transparent";
                }
            }
        }
    }
};

//function onClientUploadStart(sender, e) {
//    $.ajax({
//        type: "POST",
//        async: false,
//        url: "File_Mapping.aspx/SetUploadPath",
//        data: "{'FILENAME':'" + FILENAME + "'}",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (response) {
//            try { result = response.d; }
//            catch (err) { toastr.error('Error in Starting Client Upload :' + err, "Lighthouse eSolutions Pte. Ltd"); result = -1; }
//        },
//        failure: function (response) { toastr.error("Failure in Starting Client Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); result = -1; },
//        error: function (response) { toastr.error("Error in Starting Client Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); result = -1; }
//    });
//};


//function onClientUploadComplete(sender, e) {
//    var file = e._filename;
//    var FILETYPE = $('#cbFileType option:selected').text();
//    $.ajax({
//        type: "POST",
//        async: false,
//        url: "File_Mapping.aspx/UploadFileMapping",
//        data: "{'FILETYPE':'" + FILETYPE + "'}",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (response) {
//            try {
//                result = response.d; if (Str(result) != "") { toastr.success(result, "Lighthouse eSolutions Pte. Ltd"); }
//                else { toastr.error('Mapping values are not in correct format. Please Check file and upload it again.', "Lighthouse eSolutions Pte. Ltd"); }
//                ClearControls();
//            }
//            catch (err) { toastr.error('Error in File Upload :' + err, "Lighthouse eSolutions Pte. Ltd"); result = -1; }
//        },
//        failure: function (response) { toastr.error("Failure in File Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); result = -1; },
//        error: function (response) { toastr.error("Error in File Upload " + response.responseText, "Lighthouse eSolutions Pte. Ltd"); result = -1; }
//    });
//};
