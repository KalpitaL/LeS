<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="DefaultRules.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.DefaultRules" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">
        <div class="row">
            <div class="col-md-12">
            <div class="portlet light">
                    <div class="portlet-body" id="portlet_body">
                         <div class="table-responsive">
                            <table class="table table-striped table-bordered" id="dataGridDefRule">
                                <thead id="tblHeadDefRule" class="bg-grey-silver !Important">
                                    <tr id="tblHeadRowDefRule"></tr>  
                                </thead>
                                <tbody id="tblBodyDefRule" style="color: #000;">
                                </tbody>
                            </table>                                      
                   </div>

          <div id="ModalBSRulelnkDet" class="modal fade" tabindex="-1" style="display:none;">
          <div class="modal-dialog-lg">
               <div class="modal-content">
                      <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                           <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                            <h5 class="modal-title" id="Lnkdettitle" style="color:#fff;">Linked Rules</h5>
                      </div>
                     <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12" id="dvBSRuleDet">
                                   <div class="table-responsive">
                                    <table class="table table-striped table-bordered " id="dataGridBSRule">
                                <thead id="tblHeadBSRule" class="bg-grey-silver !Important">
                                    <tr id="tblHeadRowBSRule">     </tr>         
                                </thead>
                                <tbody id="tblBodyBSRule" style="color: #000;">
                                </tbody>
                            </table>  
                                   </div>                            
                               </div>                         
                        </div>                                                   
                     </div>   
                                                                
                </div>
          </div>
       </div>
    
          <div id="ModalNew" class="modal fade" tabindex="-1" style="display:none;">
          <div class="modal-dialog modal-lg">
               <div class="modal-content">
                      <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                           <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                            <h5 class="modal-title" style="color:#fff;">Add New Default Rule</h5>
                      </div>
                     <div class="modal-body">
                        <div class="row" style="padding-right:60px;">
                            <div class="col-md-12" id="dvNewDet"> </div>
                        </div>                                                   
                     </div>                         
                    <div class="modal-footer" style="text-align:center;">                                                                               
                      <button type="button" id="btnDefNew"  class="btn yellow-casablanca mdfootrbtn" >Update</button>                                                   
                      <button type="button" data-dismiss="modal" class="btn yellow-casablanca mdfootrbtn" >Cancel</button>
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
    <script src="../assets/ob/obDefaultRule.js"></script>
    <script> jQuery(document).ready(function () { DefaultRules.init(); $('#ModalNew,#ModalBSRulelnkDet').draggable({ handle: ".modal-header,.modal-footer" }); }); </script>
      <style type="text/css">
         .selectheight {height: 20px !Important;}
    </style>
</asp:Content>
