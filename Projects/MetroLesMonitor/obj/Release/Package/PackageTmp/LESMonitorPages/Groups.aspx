
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="Groups.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.Groups" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">      
        <div class="row">
            <div class="col-md-12">
            <div class="portlet light">
                <div class="portlet-body" id="portlet_body">
                        <div class="table table-responsive">                       
                            <table class="table table-striped table-bordered" id="dataGridGrp">
                                <thead id="tblHeadGrp" class="bg-grey-silver !Important">
                                    <tr id="tblHeadRowGrp">                                        
                                    </tr>         
                                </thead>
                                <tbody id="tblBodyGrp" style="color: #000;">
                                </tbody>
                            </table>                               
                          </div>    
                                                                                                                                                                                             
                        <div id="ModalNew" class="modal fade bs-modal-lg" tabindex="-1"  style="display:none;">
                          <div class="modal-dialog modal-lg">
                              <div class="modal-content">
                                 <div class="modal-header" style="padding:5px;background-color:#60aee4;">
                                  <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                                  <h5 class="modal-title" style="color:#fff;">Add New Group</h5>
                                 </div>
                                 <div class="modal-body">                                                                   
                                <div class="row" style="padding-right:43px;">
                                    <div class="col-md-12" id="dvNewDet"> </div>
                                </div>                
                                <div>                                                                         
                                 <div class="modal-footer" style="text-align:center;">                                                                               
                                  <button type="button" id="btnGrpNew"  class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Update</button>                                                   
                                   <button type="button" data-dismiss="modal" class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Cancel</button>
                                 </div>   
                               </div>                            
                            </div>
                         </div>
                          </div>
                       </div>
                       
                        <div id="ModalLnkRules" class="modal fade" tabindex="-1" style="display:none;">
                         <div class="modal-dialog-lg">
                             <div class="modal-content">
                               <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                               <h5 class="modal-title" id="Lnkdettitle" style="color:#fff;">View Linked Rules</h5>
                            </div>
                               <div class="modal-body">
                                   <div class="row">
                                      <div class="col-md-12" id="dvLinkedRule">
                                         <div class="table table-responsive">
                                             <table class="table table-striped table-bordered" id="dataGridlnkRule">
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
                      </div>                     
                                                                               
                        <div id="ModalLnkBuySupp" class="modal fade bs-modal-lg in" tabindex="-1" style="display:none;">
                         <div class="modal-dialog modal-lg">
                             <div class="modal-content">
                               <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                               <h5 class="modal-title" id="LnkBSdettitle" style="color:#fff;">Linked Buyers-Suppliers</h5>
                            </div>
                               <div class="modal-body">
                                   <div class="row">
                                      <div class="col-md-12" id="dvBSLinked">
                                         <div class="table table-responsive">
                                             <table class="table table-striped table-bordered " id="dataGridBSlnk">
                                            <thead id="tblHeadBSlnk" class="bg-grey-silver !Important">
                                                <tr id="tblHeadRowBSlnk">     </tr>         
                                            </thead>
                                            <tbody id="tblBodyBSlnk" style="color: #000;">
                                            </tbody>
                                         </table>                               
                                         </div>                         
                                       </div>
                                   </div>                                                   
                               </div>   
                                                                
                            </div>
                         </div>
                      </div>                     
                                               
                 </div>
          </div>
       </div>
            </div>    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="childScriptHolder" runat="server">
    <script src="../assets/global/plugins/jquery-ui/jquery-ui.min.js"></script>  
    <script src="../assets/ob/obGroups.js"></script>
    <script>  jQuery(document).ready(function () { Groups.init(); $('#ModalNew,#ModalLnkRules,#ModalLnkBuySupp').draggable({ handle: ".modal-header,.modal-footer" }); }); </script>
      <style type="text/css">                   
    </style>
</asp:Content>
