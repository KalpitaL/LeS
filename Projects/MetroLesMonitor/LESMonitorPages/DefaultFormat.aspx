<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="DefaultFormat.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.DefaultFormat" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ChildGridContent" runat="server">
<div class="row">
  <div class="col-md-12">      
      <div class="portlet light bordered">
        <div class="portlet-body form" id="portlet_body">
          <div class="table table-responsive">
            <table class="table  table-bordered table-striped" id="dataGridDefFmt">
               <thead id="tblHeadDefFmt">
                   <tr id="tblHeadRowDefFmt"> </tr>         
                </thead>
                <tbody id="tblBodyDefFmt" style="color: #000;"> </tbody>
            </table>                               
          </div>  
      </div>
  </div>
 </div>
</div>

 <div id="ModalNewFormat" class="modal fade" tabindex="-1" style="display:none;z-index:10005;">
    <div class="modal-dialog" style="width:700px;">
           <div class="modal-content">
                <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                   <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                    <h5 class="modal-title" style="color:#fff;">Add New Default Format</h5>
                </div>
                <div class="modal-body"> <div id="dvNewFrmtDet" style="margin-right:30px;"> </div>  </div>                                                          
                <div class="modal-footer" style="text-align:center;">                                                                               
                   <button type="button" id="btnAddFormat"  class="btn yellow-casablanca mdfootrbtn">Add</button>                                                   
                    <button type="button" data-dismiss="modal" class="btn yellow-casablanca mdfootrbtn">Cancel</button>
                 </div>                           
           </div>
    </div>
 </div>

 <div id="ModalNewRule" class="modal fade" tabindex="-1" style="display:none;">
     <div class="modal-dialog modal-lg">
       <div class="modal-content">
          <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                           <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                            <h5 class="modal-title" style="color:#fff;">Add New Default Rule</h5>
                      </div>
          <div class="modal-body">
            <div class="row">
                <div class="col-md-12" id="dvNewRule"> 
                  <div class="table-responsive">
                    <table class="table  table-bordered table-striped" id="dataGridRule">
                       <thead id="tblHeadRule"> <tr id="tblHeadRowRule"></tr></thead>
                       <tbody id="tblBodyRule" style="color: #000;"> </tbody>
                     </table>  
                  </div>                            
                 </div>
              </div>                                                   
          </div>                         
          <div class="modal-footer" style="text-align:center;">                                                                               
             <button type="button" id="btnNewRule"  class="btn yellow-casablanca mdfootrbtn">Add</button>                                                   
             <button type="button" data-dismiss="modal" class="btn yellow-casablanca mdfootrbtn" >Cancel</button>
           </div>                           
        </div>
     </div>
 </div>

 <div id="ModalDefFmtRuleDet" class="modal fade" tabindex="-1" style="display:none;">
   <div class="modal-dialog-lg">
       <div class="modal-content">
            <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                <h5 class="modal-title" id="Lnkdettitle" style="color:#fff;">Default Format Rules</h5>
            </div>
             <div class="modal-body">
                   <div class="portlet custom">
                        <div class="portlet-title margin-bottom-0"><div id="Itemtoolbtngroup"> </div> </div>
                  </div>
                <div class="row">
                    <div class="col-md-12" id="dvDFRuleDet">
                        <div class="table-responsive">
                            <table class="table  table-bordered table-striped" id="dataGridDFRule">
                                <thead id="tblHeadDFRule"> <tr id="tblHeadRowDFRule"></tr> </thead>
                                <tbody id="tblBodyDFRule" style="color: #000;"></tbody>
                            </table>  
                         </div>                            
                    </div>                         
                 </div>                                                   
            </div>                                                                   
       </div>
   </div>
 </div>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="childScriptHolder" runat="server">
    <script src="../assets/Scripts/ob/DefaultFormat.js"></script>
    <script> jQuery(document).ready(function () { DefaultFormat.init();});</script>
</asp:Content>


 <%--$('#ModalNewFormat').draggable({ handle: ".modal-header,.modal-footer" }); --%>
