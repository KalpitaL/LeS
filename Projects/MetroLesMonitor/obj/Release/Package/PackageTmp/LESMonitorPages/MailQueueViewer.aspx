<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="MailQueueViewer.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.MailQueueViewer" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">
        <div class="row">
              <div class="col-md-12">
            <div class="portlet light">
                    <div class="portlet-body" id="portlet_body">
                         <div class="table table-responsive">
                            <table class="table table-striped table-bordered" id="dataGridMailQView">
                                <thead id="tblHeadMailQView" class="bg-grey-silver !Important">
                                    <tr id="tblHeadRowMailQView"> </tr>         
                                </thead>
                                <tbody id="tblBodyMailQView" style="color: #000;" class="sort">                                 
                                </tbody>
                            </table>                               
                            </div>           
                    </div>
                </div>
            </div>
         </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="childScriptHolder" runat="server">
    <script src="../assets/ob/obMailQueueViewer.js"></script>
      <script> jQuery(document).ready(function () { MailQueueViewer.init();  });</script>
    
</asp:Content>
