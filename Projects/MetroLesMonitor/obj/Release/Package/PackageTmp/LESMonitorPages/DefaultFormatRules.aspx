<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="DefaultFormatRules.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.DefaultFormatRules" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ChildGridContent" runat="server">
<div class="row">
  <div class="col-md-12">      
      <div class="portlet light bordered">
        <div class="portlet-body form" id="portlet_body">
          <div class="table table-responsive">
            <table class="table table-bordered" id="dataGridDFRule">
               <thead id="tblHeadDFRule">
                   <tr id="tblHeadRowDFRule"> </tr>         
                </thead>
                <tbody id="tblBodyDFRule" style="color: #000;"> </tbody>
            </table>                               
          </div>  
      </div>
  </div>
 </div>
</div>

 <div id="ModalNewRule" class="modal fade" tabindex="-1" style="display:none;z-index:10005;">
     <div class="modal-dialog" style="width:1000px;">
       <div class="modal-content">
          <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                           <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                            <h5 class="modal-title" style="color:#fff;">Add New Rule</h5>
                      </div>
          <div class="modal-body">                       
               <div class="row"><div class="col-md-12">
                  <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Default Format </label> </div>
                  <div  class="col-md-3"><select class="form-control" id="cbDefaultFormat"></select></div> 
              </div> </div>
            <div class="row">
                <div class="col-md-12"> 
                  <div class="table table-responsive">
                    <table class="table table-bordered" id="dataGridNewRule">
                       <thead id="tblHeadRule" class="bg-grey-silver !Important"> <tr id="tblHeadRowRule"></tr></thead>
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
 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="childScriptHolder" runat="server">
    <script src="../assets/ob/obDefaultFormatRules.js"></script>
    <script> jQuery(document).ready(function () { DefaultFormatRules.init(); $('#ModalNewRule').draggable({ handle: ".modal-header,.modal-footer" }); });</script>
</asp:Content>
