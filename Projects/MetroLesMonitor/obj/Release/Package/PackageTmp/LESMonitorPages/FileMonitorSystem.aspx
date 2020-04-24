<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="FileMonitorSystem.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.FileMonitorSystem" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">
    <div class="row">
       <div class="col-md-12">
            <div class="portlet light">       
                <div class="portlet light form-fit" id="divFilter">
                     <div class="portlet-title margin-bottom-0 padding-0">                          
                            <div class="row">                           
                                <div class="portlet-body form">
                                </div>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                </div>
                <hr />
               <div class="portlet-body" id="portlet_body">
                     <div class="table table-responsive">
                          <table class="table table-striped table-bordered" id="dataGridFileMtrSys">
                                <thead id="tblHeadFileMtrSys" class="bg-grey-silver !Important">
                                    <tr id="tblHeadRowFileMtrSys">  </tr>         
                                </thead>
                                <tbody id="tblBodyFileMtrSys" style="color: #000;"> </tbody>
                            </table>                               
                      </div>                                           
               </div>
            </div>
       </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="childScriptHolder" runat="server">
    <script src="../assets/ob/obFileMonitor.js"></script>
    <script>
        jQuery(document).ready(function () { $("#baseBody").fadeIn(); FileMonitor.init(); });
         </script>
      <style type="text/css">       
          .opentd { background: url('../Content/themes/base/images/add.png') no-repeat center center; cursor: pointer; }
          .closetd{ background: url('../Content/themes/base/images/minus.png') no-repeat center center; cursor: pointer; }
          .spaceUnder {padding-bottom: 1em;}
        .scroll-left { width:600px;height: 35px; overflow: hidden;position: relative;font-size: 10px;color: #ff0000;}
.scroll-left span {position: absolute;width: 100%;height: 100%;margin: 0;line-height: 20px;-moz-transform:translateX(100%);-webkit-transform:translateX(100%);    
 transform:translateX(100%); -moz-animation: scroll-left 15s linear infinite;-webkit-animation: scroll-left 15s linear infinite;animation: scroll-left 15s linear infinite;}
@-moz-keyframes scroll-left {
 0%   { -moz-transform: translateX(100%); }
 100% { -moz-transform: translateX(-100%); }
}
@-webkit-keyframes scroll-left {
 0%   { -webkit-transform: translateX(100%); }
 100% { -webkit-transform: translateX(-100%); }
}
@keyframes scroll-left {
 0%   { 
 -moz-transform: translateX(100%); /* Browser bug fix */
 -webkit-transform: translateX(100%); /* Browser bug fix */
 transform: translateX(100%);       
 }
 100% { 
 -moz-transform: translateX(-100%); /* Browser bug fix */
 -webkit-transform: translateX(-100%); /* Browser bug fix */
 transform: translateX(-100%); 
 }
}

      </style>
</asp:Content>
