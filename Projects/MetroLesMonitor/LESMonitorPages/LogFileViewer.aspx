<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="LogFileViewer.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.LogFileViewer" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">    
        <div class="row">
          <div class="col-md-12">
            <div class="portlet light">
                    <div class="portlet-body" id="portlet_body">
                         <div class="table table-responsive">
                            <table class="table  table-bordered table-striped" id="dataGridLogFView">
                                <thead id="tblHeadALog">
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
    <link href="../assets/css/LogfileViewer.css" rel="stylesheet" />
    <script src="../assets/Scripts/ob/LogFileViewer.js"></script>
     <script> jQuery(document).ready(function () {   LogFileViewer.init();  });</script>
</asp:Content>
