<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="LoginHistory.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.LoginHistory" %>
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
                        <table class="table  table-bordered table-striped" id="dataGridLogHist">
                                <thead id="tblHeadLogHist">
                                    <tr id="tblHeadRowLogHist">  </tr>         
                                </thead>
                                <tbody id="tblBodyLogHist" style="color: #000;">
                                </tbody>
                            </table>   
                        </div>                                                                    
            </div>
          </div>
        </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="childScriptHolder" runat="server">
    <link href="../assets/css/loginHistory.css" rel="stylesheet" />
    <script src="../assets/Scripts/ob/LoginHistory.js"></script>
    <script> jQuery(document).ready(function () {  LoginHistory.init();  }); </script>
</asp:Content>
