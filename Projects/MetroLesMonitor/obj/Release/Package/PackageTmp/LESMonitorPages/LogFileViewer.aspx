<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="LogFileViewer.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.LogFileViewer" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">    
        <div class="row">
          <div class="col-md-12">
            <div class="portlet light">
                    <div class="portlet-body" id="portlet_body">
                         <div class="table table-responsive">
                            <table class="table table-striped table-bordered" id="dataGridLogFView">
                                <thead id="tblHeadALog" class="bg-grey-silver !Important">
                                    <tr id="tblHeadRowLogFView">                                   
                                    </tr>  
                                </thead>
                                <tbody id="tblBodyLogFView" style="color: #000;">
                                </tbody>
                            </table>                               
                            </div>           
                    </div>
                </div>
            </div>
         </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="childScriptHolder" runat="server">
    <script src="../assets/ob/obLogFileViewer.js"></script>
     <script> jQuery(document).ready(function () {   LogFileViewer.init();  });</script>
      <style type="text/css">     
            #dataGridLogFView{margin: 0 auto;width: 100%;clear: both;border-collapse: collapse;table-layout: fixed;}     
          .opentd{ background: url('../Content/themes/base/images/add.png') no-repeat center center; cursor: pointer; }
          .closetd{ background: url('../Content/themes/base/images/minus.png') no-repeat center center; cursor: pointer;}                
       </style>
</asp:Content>
