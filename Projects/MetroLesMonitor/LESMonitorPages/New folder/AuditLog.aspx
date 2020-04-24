<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="AuditLog.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.AuditLog" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server"> 
     <div class="row">         
        <div class="col-md-12">                 
            <div class="portlet light">    
  
            <div class="panel-group accordion" id="accordion1">
                <div class="panel panel-default">
                      <div class="panel-heading">
                        <h4 class="panel-title">
                              <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapse_1" title="Click to expand">Filters </a>
                         </h4>
                    </div>

                      <div id="collapse_1" class="panel-collapse collapse">
                         <div class="panel-body" >
                               <%-- <div class="portlet-title margin-bottom-0 padding-0">                          
                            <div class="row">                           --%>
                                <div class="portlet-body form" id="divFilter">
                                </div>
                                <div class="clearfix"></div>
                           <%-- </div>
                        </div>--%>
                          </div>
                      </div>
                </div>
             </div>

               <hr />
                <div class="portlet-body" id="portlet_body"> 
                    <div class="row" style="margin-bottom: 10px;">
                        <div class="col-md-8 col-sm-12">
                          <div class="dataTables_paginate paging_bootstrap_extended" id="datatable_ajax_paginate">
                           <div class="pagination-panel" id="pnlPage">
                               <span>Page</span>
                                <%-- <button id="btnPrev" disabled type="button" class="btn btn-circle btn-icon-only red" name="Prev" title="Prev"> <i class="fa fa-chevron-left"></i></button>--%>
                                <button id="btnPrev" disabled type="button" class="btn blue" name="Prev" title="Prev"> <i class="fa fa-chevron-left"></i></button>
                                   <input type="text"  class="pagination-panel-input form-control input-sm input-inline" style="width:55px;" id="txtFrom">
                                 <button id="btnNext" disabled type="button" class="btn blue" name="Next" title="Next"> <i class="fa fa-chevron-right"></i></button>
                                   <label id="lblrecdet"></label>
                           </div>
                         </div>
                       </div>
                    </div>

                     <div class="clearfix"></div>
                    <table class="table table-bordered table-striped" id="dataGridALog">
                                <thead id="tblHeadALog">
                                    <tr id="tblHeadRowALog"> </tr>         
                                </thead>
                                <tbody id="tblBodyALog" style="color: #000;">
                                </tbody>
                            </table> 
                   </div>

            <div id="ModalResubmit" class="modal fade" tabindex="-1" data-width="400" style="display:none;">
              <div class="modal-dialog">
               <div class="modal-content">
                       <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                             <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                              <h5 class="modal-title" style="color:#fff;">Resubmit File</h5>
                       </div>
                       <div class="modal-body">
                                <div class="row">
                                      <div class="col-md-12">
                                           <p>  <label class="control-label " id="lblError">Error</label></p>
                                          <p><label class="control-label " id="lblFileName">File</label> </p>
                                          <p> <textarea id="txtFile" style="width:570px;height:260px;"></textarea>  </p>
                                     </div>
                                 </div>                                                   
                         </div>
                       <div class="modal-footer" style="text-align:center;">                                                                             
                            <button type="button" class="btn yellow-casablanca" id="btnReSubmit" style="font-size:8pt;padding: 6px 6px;">ReSubmit</button>                           
                             <button type="button" data-dismiss="modal" class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Close</button>
                       </div>
                </div>
          </div>
          </div>
      </div>
       </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="childScriptHolder" runat="server">   
    <link href="../assets/css/filtertable.css" rel="stylesheet" />
    <link href="../assets/css/Auditlog.css" rel="stylesheet" />
    <script src="../assets/Scripts/ob/Auditlog.js"></script>
    <script>jQuery(document).ready(function () { AuditLog.init(); });</script>
</asp:Content>
