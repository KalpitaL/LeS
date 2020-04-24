<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="LinkRules.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.LinkRules" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">       
        <div class="row">
            <div class="col-md-12">
            <div class="portlet light">
                    <div class="portlet-body portlet-datatable" id="portlet_body">
                        <div class="table table-responsive">
                            <table class="table  table-bordered table-striped" id="dataGridLnkRule">
                                <thead id="tblHeadLnkRule">
                                    <tr id="tblHeadRowLnkRule">  </tr>         
                                </thead>
                                <tbody id="tblBodyLnkRule" style="color: #000;">
                                </tbody>
                            </table>                               
                            </div>                                                           

                        <div id="ModalNewRule" class="modal fade" tabindex="-1" data-width="1000" style="display:none;z-index:100005;">
                          <div class="modal-dialog modal-lg">
                              <div class="modal-content">
                                 <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                           <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                            <h5 class="modal-title" style="color:#fff;">Rule List</h5>
                      </div>

                            <div class="modal-body" style="padding-bottom:1px;">
                               <div class="row">
                                    <div class="col-md-12" id="dvNewDet">
                                     <div class="table table-responsive">
                                       <table class="table  table-bordered table-striped" id="dataGridNewRule">
                                        <thead id="tblHeadNewRule">
                                          <tr id="tblHeadRowNewRule">     </tr>         
                                         </thead>
                                       <tbody id="tblBodyNewRule" style="color: #000;"></tbody>
                                       </table>                               
                                   </div>                                 
                            </div>
                              </div>                                                   
                            </div>                         
                                 <div class="modal-footer" style="text-align:center;">                                                                               
                      <button type="button" id="btnNewRule"  class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Add</button>                                                   
                      <button type="button" data-dismiss="modal" class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Cancel</button>
                   </div>                           
                              </div>
                         </div>
                      </div>

                        <div id="ModalBuySupp" class="modal fade" tabindex="-1" data-width="1000" style="display:none;">
                         <div class="modal-dialog-lg">
                             <div class="modal-content">
                               <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                               <h5 class="modal-title" id="LnkBSdettitle" style="color:#fff;">Buyer-Supplier Link</h5>
                            </div>
                               <div class="modal-body">
                                   <div class="row">
                                      <div class="col-md-12" id="dvBS">
                                         <div class="table table-responsive">
                                             <table class="table  table-bordered table-striped" id="dataGridBS">
                                            <thead id="tblHeadBS">
                                                <tr id="tblHeadRowBS">     </tr>         
                                            </thead>
                                            <tbody id="tblBodyBS" style="color: #000;">
                                            </tbody>
                                         </table>                               
                                         </div>                         
                                       </div>
                                   </div>                                                   
                               </div>                                                                   
                            </div>
                         </div>
                      </div>  
                                                  
                        <div id="ModalBSRulesLst" class="modal fade" tabindex="-1" data-width="1000" style="display:none;z-index:100000;">
                         <div class="modal-dialog-lg">
                             <div class="modal-content">
                               <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                               <h5 class="modal-title" id="Lnkdettitle" style="color:#fff;">List of Buyer-Supplier Rules </h5>
                            </div>
                               <div class="modal-body">
                                   <div class="row">
                                      <div class="col-md-12" id="dvBSRule">
                                         <div class="table table-responsive">
                                             <table class="table  table-bordered table-striped" id="dataGridBSRule">
                                              <thead id="tblHeadBSRule">
                                                  <tr id="tblHeadRowBSRule">     </tr>         
                                              </thead>
                                             <tbody id="tblBodyBSRule" style="color: #000;"> </tbody>
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
    <script src="../assets/Scripts/ob/LinkRules.js"></script>
    <script>jQuery(document).ready(function () { LinkRules.init();  });   </script>
</asp:Content>


<%--$('#ModalNewRule,#ModalBSRulesLst,#ModalBuySupp').draggable({ handle: ".modal-header,.modal-footer" });--%>
