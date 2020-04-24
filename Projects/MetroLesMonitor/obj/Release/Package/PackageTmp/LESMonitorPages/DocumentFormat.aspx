<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="DocumentFormat.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.DocumentFormat" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ChildGridContent" runat="server">
<div class="row">
  <div class="col-md-12">      
      <div class="portlet light bordered">
        <div class="portlet-body form" id="portlet_body">
          <div class="table table-responsive">
            <table class="table table-bordered" id="dataGridDefFmt">
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

 <div id="ModalNewRule" class="modal fade" tabindex="-1" style="display:none;z-index:10019;">
     <div class="modal-dialog modal-lg">
       <div class="modal-content">
          <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                           <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                            <h5 class="modal-title" style="color:#fff;">Add New Rule</h5>
                      </div>
          <div class="modal-body">
            <div class="row">
                <div class="col-md-12" id="dvNewRule"> 
                  <div class="table-responsive">
                    <table class="table table-striped table-bordered " id="dataGridNewRule">
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

 <div id="ModalDefFmtRuleDet" class="modal fade" tabindex="-1" style="display:none;z-index:10018;">
   <div class="modal-dialog-lg">
       <div class="modal-content">
            <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                <h5 class="modal-title" id="Lnkdettitle" style="color:#fff;">Document Format Rules</h5>
            </div>
             <div class="modal-body">
                   <div class="portlet custom" style="margin-bottom:15px;">
                        <div class="portlet-title margin-bottom-0"><div id="dvFormatdet"><span class="caption-subject bold"> Format : </span><span id="spnFormatid" class="caption-subject font-blue-sharp bold uppercase"></span>
                            <div id="Itemtoolbtngroup" style="margin-top:-15px;"> </div> </div>
                        </div>
                  </div>
                <div class="row">
                    <div class="col-md-12" id="dvDFRuleDet">
                        <div class="table-responsive">
                            <table class="table table-striped table-bordered " id="dataGridDFRule">
                                <thead id="tblHeadDFRule" class="bg-grey-silver !Important"> <tr id="tblHeadRowDFRule"></tr> </thead>
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
    <script src="../assets/global/plugins/jquery-ui/jquery-ui.min.js"></script>  
    <script src="../assets/ob/obDocumentFormat.js"></script>
    <script> jQuery(document).ready(function () { DocumentFormat.init(); $('#ModalNewFormat,#ModalDefFmtRuleDet,#ModalNewRule').draggable({ handle: ".modal-header,.modal-footer" }); });</script>
</asp:Content>
