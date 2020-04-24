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
                          <table class="table  table-bordered table-striped" id="dataGridFileMtrSys">
                                <thead id="tblHeadFileMtrSys">
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
    <link href="../assets/css/fileMonitorSystem.css" rel="stylesheet" />
    <script src="../assets/Scripts/ob/FileMonitor.js"></script>
    <script>jQuery(document).ready(function () { $("#baseBody").fadeIn(); FileMonitor.init(); });  </script>
</asp:Content>
