_liGroup = []; _liShipType = []; slModuleAction = []; var applNo = ''; var crewName = ''; var _lstRank = []; var _lstCountry = []; var _lstShips = []; var _lstUserAssignSites = []; var Params = {}; function SetSessionValues(){};
function RedirectServer_userid() {
    var UserId = ''; var _url = window.location.href; Params = getTitleDecryptedData(); if (Params != '' && Params != undefined) {
        if (Params.split('&').length > 1) {
            UserId = Str(Params.split('&')[0].split('@')[1].split('#')[0]); getUserSession_ID(UserId);
location.href='../LESMonitorPages/Adaptors.aspx';}}};function getTitleDecryptedData(){var KEYURL='';var _result='';var ind=window.location.href.indexOf('?');if(ind>-1){KEYURL=window.location.href.split('?')[1];$.ajax({type:"POST",async:false,url:"../Common/Default.aspx/DecryptURL_redirect",data:"{'KEYURL':'"+Str(KEYURL)+"'}",contentType:"application/json; charset=utf-8",dataType:"json",success:function(response){try{_result=Str(response.d);}
catch(err){toastr.error('Error in Getting decrypted Url :'+err,"Lighthouse eSolutions Pte. Ltd");}},failure:function(response){toastr.error("Failure in Getting decrypted Url","Lighthouse eSolutions Pte. Ltd");},error:function(response){toastr.error("Error in Getting decrypted Url","Lighthouse eSolutions Pte. Ltd");}});}
    return _result;
}; function getUserSession_ID(UserId) {
    var aSession = []; jQuery.ajax({
        type: 'POST', async: false, url: '../Common/Default.aspx/SetUserSession_ID', data: "{ 'UserId' : '" + UserId + "' }", contentType: 'application/json; charset=uts-8', success: function (ds) {
            aSession = JSON.parse(ds.d);
            sessionStorage['USERID'] = aSession[0]; sessionStorage['USERNAME'] = aSession[1]; sessionStorage['ADDRESSID'] = aSession[2];
            sessionStorage['LOGIN_NAME'] = aSession[3]; sessionStorage['ADDRTYPE'] = aSession[4]; sessionStorage['USERHOSTSERVER'] = aSession[5];
            sessionStorage['CONFIGADDRESSID'] = aSession[6]; sessionStorage['PASSWORD'] = aSession[7]; sessionStorage['SERVERNAME'] = aSession[8];
        }, failure: function (response) { toastr.error("failure get", response.d); }, error: function (response) { toastr.error("error get", response.responseText); }
    }); SetSessionValues();
}; function TabClick(page)
{try{location.replace(page);return false;}catch(err){}}function LogoutClick(sesexpr){var data;jQuery.ajax({type:'POST',async:false,url:'../Common/Default.aspx/Logout',data:"{'sesexpr':'"+sesexpr+"'}",contentType:'application/json; charset=uts-8',success:function(response){data=response.d;sessionStorage.clear();localStorage.clear();window.location=data;},failure:function(response){toastr.error("failure get",response.d);},error:function(response){toastr.error("error get",response.responseText);}});return data;};function SetupBreadcrumb(liHome,cHome,liModule1,cModule1,liModule2,cModule2){$('#homehref').text(liHome);$('#homehref').attr('href',cHome);if(liModule1!=''){$('#module1href').text(liModule1);$('#module1href').attr('href',cModule1);}
else{$('#module1href').hide();}
if(liModule2!=''){$('#module2href').text(liModule2);$('#module2href').attr('href',cModule2);}
else{$('#module2href').hide();}};function Str(v){if(v==undefined||v==null||v=='undefined'||v.toString()=='[object Object]'||v.toString()=='NaN')return'';else return v.toString().trim();};function Trim(s){return Str(s).replace(/\\/g,"").replace(/'/g,"\\'").trim();}
function Int(v){if(Str(v)==''||isNaN(parseInt(v)))return 0;else return parseInt(v);}
function Float(v){if(Str(v)==''||isNaN(parseFloat(v)))return 0;else return parseFloat(v);}
function Len(obj){if(window.DOMParser!=undefined)return Object.keys(obj).length;else return Object.size(obj);}
function getDateFormated(value){var date;if(value!=undefined&&value!=null&&value!=''){var dVal=value.split('T');var _dateval=new Date(dVal[0]);var date=(_dateval.getMonth()+1)+"/"+_dateval.getDate()+"/"+_dateval.getFullYear();}else{date="";}
return date;}
function getSQLDateFormated1(value){var date;if(value!=undefined&&value!=null&&value!=''){var dVal=value.split('/');var _dateval=new Date(dVal[2],dVal[1],dVal[0]);var date=_dateval.getFullYear()+"-"+(_dateval.getMonth())+"-"+_dateval.getDate();}else{date="";}
return date;}
function getSQLDateFormated(value){var date;if(value!=undefined&&value!=null&&value!=''){var dVal=value.split('/');var _dateval=new Date(dVal[2],dVal[1],dVal[0]);var date=dVal[2]+"-"+dVal[1]+"-"+dVal[0];}else{date="";}
return date;}
function GetDateFormat(){var dt=new Date(2013,11,31);var str=dt.toLocaleDateString();str=str.replace("31","dd");str=str.replace("12","mm");str=str.replace("2013","yyyy");return str;}
function GetFormattedDate(dateyear,datemonth,dateday){var str=GetDateFormat();str=str.replace("yyyy",dateyear);str=str.replace("mm",datemonth);str=str.replace("dd",dateday);return str;}
function getSQLDate(_date){var _dateformat=GetDateFormat();var dateSQLFormat=_date.substring((_date.indexOf('yyyy'),4)+"-"+_date.substring(_date.indexOf('mm'),2)+
_date.substring(_date.indexOf('dd'),2));return dateSQLFormat;}
function getSQLDate_fomat(value,_formatstring){var dateSQLFormat='';if(value!=undefined||value!=''){var _dateformat=new Date(value);dateSQLFormat=_dateformat.format(_formatstring)}
return dateSQLFormat;}
function getDateTimeDetails(value){var date;var dVal='';if(value!=undefined&&value!=null&&value!=''){var dVal1=value.split('/');if(dVal1.length>1){dVal=dVal1}else{dVal=value.split('-');}
if(dVal.length>1)
var tVal=value.split(' ')[1].split(':');var _dateval=new Date(dVal[2].split(' ')[0],dVal[1],dVal[0]);date=dVal[2].split(' ')[0]+"-"+dVal[1]+"-"+dVal[0]+" "+tVal[0]+":"+tVal[1]+":"+tVal[2];}
else{date="";}
return date;}
function getSQLDateTime(value){var datetime='';if(value!=undefined){var dVal=value.split('T');var dtVal=dVal[0].split('-');var dttime=dVal[1].split('+')[0];var _dateval=new Date(dtVal[0],dtVal[1],dtVal[2]);var date=dtVal[0]+"-"+dtVal[1]+"-"+dtVal[2];datetime=date+" "+dttime;}
return datetime;}
function getEncryptedData(strUrl){var _result='';$.ajax({type:"POST",async:false,url:"../Common/Default.aspx/EncryptURL",data:"{'ORG_URL':'"+strUrl+"'}",contentType:"application/json; charset=utf-8",dataType:"json",success:function(response){try{_result=response.d;}
catch(err){toastr.error('Error in Getting encrypted Url :'+err,"Lighthouse eSolutions Pte. Ltd");}},failure:function(response){toastr.error("Failure in Getting encrypted Url","Lighthouse eSolutions Pte. Ltd");},error:function(response){toastr.error("Error in Getting encrypted Url","Lighthouse eSolutions Pte. Ltd");}});return _result;};function getDecryptedData(){var p='';var Params1=[];var ind=window.location.href.indexOf('?');var key='';if(ind>-1){key=Str(window.location.href.substring(window.location.href.indexOf('?')+1));var p='';$.ajax({type:"POST",async:false,url:"../Common/Default.aspx/DecryptURL",data:"{'KEYURL':'"+key+"'}",contentType:"application/json; charset=utf-8",dataType:"json",success:function(response){try{p=response.d;}catch(err){toastr.error('Error in Getting decrypted Url :'+err,"Lighthouse eSolutions Pte. Ltd");}},failure:function(response){toastr.error("Failure in Getting decrypted Url","Lighthouse eSolutions Pte. Ltd");},error:function(response){toastr.error("Error in Getting decrypted Url","Lighthouse eSolutions Pte. Ltd");}});var lst=p.split('&');for(var i=0;i<lst.length;i++){var v=lst[i].split('=');Params1.push(v[0]+'|'+v[1]);}
return Params1;}};function FillCombo(val,_lst){var opt='';emptystr='';try{opt='<option>'+emptystr+'</option>';if(_lst.length!=undefined&&_lst.length>0){for(var i=0;i<_lst.length;i++){var cdet=_lst[i].split("|");if(val!=""&&val==Str(cdet[0])){opt+='<option selected value="'+Str(cdet[0])+'">'+Str(cdet[1])+'</option>';}
else if(val!=""&&val==Str(cdet[1])){opt+='<option selected value="'+Str(cdet[0])+'">'+Str(cdet[1])+'</option>';}
else{opt+='<option value="'+Str(cdet[0])+'">'+Str(cdet[1])+'</option>';}}
return opt;}}
    catch (err) { toastr.error('Error while populating List :' + err, "Lighthouse eSolutions Pte. Ltd"); }
};
function GetServerDate() {
    var _result = '';
    $.ajax({
        type: "POST",
        async: false,
        url: "../Common/Default.aspx/GetServerDate",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) { try { _result = Str(response.d); }  catch (err) { }  },
        failure: function (response) { }, error: function (response) { }
    });
    return _result;
};


function SetTimerCheckBox(chkAutoRefresh, Name) {
    if (jQuery.jStorage.get(Name) != null && jQuery.jStorage.get(Name) == 'true')
    { $(chkAutoRefresh).attr("checked", true); timeoutID = window.setInterval("location.reload(true);", 180000); }
    else { jQuery.jStorage.set(Name, ''); }
};

function DoAutoRefresh(Checked, RefName) {
    if (Checked) { jQuery.jStorage.set(RefName, 'true'); timeoutID = window.setInterval("location.reload(true);", 180000); }
    else { jQuery.jStorage.set(RefName, ''); window.clearInterval(timeoutID); }
};

function GetVersionDetails() {
    var _result = '';
    $.ajax({
        type: "POST",
        async: false,
        url: "../Common/Default.aspx/GetFileVersion",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) { try { _result = Str(response.d); } catch (err) { } },
        failure: function (response) { }, error: function (response) { }
    });
    return _result;
};

function GetServer_SessionDetails() {
    var aSession = []; jQuery.ajax({
        type: 'POST', async: false, url: '../Common/Default.aspx/GetSessionValues', data: "{}", contentType: 'application/json; charset=uts-8',
        success: function (ds) {
            aSession = JSON.parse(ds.d); sessionStorage['USERID'] = aSession[0]; sessionStorage['USERNAME'] = aSession[1]; sessionStorage['ADDRESSID'] = aSession[2]; sessionStorage['LOGIN_NAME'] = aSession[3]; sessionStorage['ADDRTYPE'] = aSession[4]; sessionStorage['USERHOSTSERVER'] = aSession[5];
            sessionStorage['CONFIGADDRESSID'] = aSession[6]; sessionStorage['PASSWORD'] = aSession[7]; sessionStorage['SERVERNAME'] = aSession[8];
            sessionStorage['LOGO_SERVER_NAME'] = aSession[9];
        }, failure: function (response) { toastr.error("failure get", response.d); }, error: function (response) { toastr.error("error get", response.responseText); }
    });
};

var TabDetails = function () {
    var _result = '';
    $.ajax({
        type: "POST",
        async: false,
        url: "../Common/Default.aspx/GetTabsList",
        data: '{}',
        contentType: "application/json;charset=utf-8",
        datatype: 'json',
        success: function (response) {
            try {
                if (response.d != '' && response.d != undefined) { _result = Str(response.d).split('|'); }
            }
            catch (e) { }
        },
        failure: function (response) { },
        error: function (response) { }
    });
    return _result;
};

var SetBlockedPage = function () {
    var _res = '';
    $.ajax
        ({
            type: "POST", async: false, url: "../Common/Default.aspx/DisplayBlockedPage", data: '{}', contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (response) { _res = Str(response.d); }, failure: function (response) { _res = ''; }, error: function (response) { _res = ''; }
        });
    return _res;
};


