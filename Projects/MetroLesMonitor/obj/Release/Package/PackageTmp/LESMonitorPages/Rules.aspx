<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="Rules.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.Rules" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">
        <div class="row">
            <div class="col-md-12">
            <div class="portlet light">                
                    <div class="portlet-body portlet-datatable" id="portlet_body">
                        <div class="table table-responsive">
                            <table class="table table-striped table-bordered" id="dataGridRule">
                                <thead id="tblHeadRule" class="bg-grey-silver !Important">
                                    <tr id="tblHeadRowRule"></tr>         
                                </thead>
                                <tbody id="tblBodyRule" style="color: #000;">
                                </tbody>
                            </table>                               
                            </div>                                           
                    </div>

            </div>
          </div>
            </div>
        
        <div id="ModalNew" class="modal fade bs-modal-lg" tabindex="-1" style="display:none;">
          <div class="modal-dialog modal-lg">
               <div class="modal-content">
                      <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                           <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                            <h5 class="modal-title" style="color:#fff;">Add New Rule</h5>
                      </div>
                     <div class="modal-body">
                        <div class="row" style="padding-right:40px;">
                            <div class="col-md-12" id="dvNewDet">  </div>
                        </div>                                                   
                     </div>                         
                    <div class="modal-footer" style="text-align:center;">                                                                               
                      <button type="button" id="btnNew"  class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Update</button>                                                   
                      <button type="button" data-dismiss="modal" class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Cancel</button>
                   </div>                           
                </div>
          </div>
       </div>

        <div id="ModalLinKDet" class="modal fade bs-modal-lg in" tabindex="-1" style="display:none;">
          <div class="modal-dialog modal-lg">
               <div class="modal-content">
                      <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                           <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                            <h5 class="modal-title" id="Lnkdettitle" style="color:#fff;"></h5>
                      </div>
                     <div class="modal-body">
                        <div class="row" style="padding-right:30px;">
                            <div class="col-md-12" id="dvlnkDet">
                                   <table class="table table-striped table-bordered table-hover" id="dataGridlnkRule">
                                <thead id="tblHeadlnkRule" class="bg-grey-silver !Important">
                                    <tr id="tblHeadRowlnkRule">     </tr>         
                                </thead>
                                <tbody id="tblBodylnkRule" style="color: #000;">
                                </tbody>
                            </table>                                                      
                            </div>
                        </div>                                                   
                     </div>   
                                                                
                </div>
          </div>
       </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="childScriptHolder" runat="server">
        <script src="../assets/global/plugins/jquery-ui/jquery-ui.min.js"></script>  
    <script src="../assets/ob/obRules.js"></script>
    <script> jQuery(document).ready(function () { Rules.init(); $('#ModalNew,#ModalLinKDet').draggable({ handle: ".modal-header,.modal-footer" }); }); </script>
      <style type="text/css">
          .textarea-Error {
              width: 100%;
              border: 1px solid red;
          }
          .textarea-success
          {
              width:100%;border:1px solid #c2cad8;
          }
    </style>
</asp:Content>
