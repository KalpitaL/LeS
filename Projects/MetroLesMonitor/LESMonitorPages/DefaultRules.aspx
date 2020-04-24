<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="DefaultRules.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.DefaultRules" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">
        <div class="row">
            <div class="col-md-12">
            <div class="portlet light">
                    <div class="portlet-body" id="portlet_body">
                         <div class="table-responsive">
                            <table class="table  table-bordered table-striped" id="dataGridDefRule">
                                <thead id="tblHeadDefRule">
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
                                    <table class="table  table-bordered table-striped" id="dataGridBSRule">
                                <thead id="tblHeadBSRule">
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
    <link href="../assets/css/commonstyle.css" rel="stylesheet" />
    <script src="../assets/Scripts/ob/DefaultRules.js"></script>
    <script> jQuery(document).ready(function () { DefaultRules.init(); }); </script>
</asp:Content>
 <%--$('#ModalNew,#ModalBSRulelnkDet').draggable({ handle: ".modal-header,.modal-footer" });--%>