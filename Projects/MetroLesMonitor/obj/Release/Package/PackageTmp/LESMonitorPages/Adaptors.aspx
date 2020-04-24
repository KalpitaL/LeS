<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="Adaptors.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.Adaptors" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ChildGridContent" runat="server">
<%--  <div class="container-fluid">--%>
    <div class="row">
        <div class="col-md-12"> 
         <div class="portlet box blue">
                <div class="portlet-title">
                    <div class="caption">Adaptor</div>
                    <div class="tools">
                       <a href="javascript:;" id="btnservstatus" ></a>
                        <span id="servStatus"  style="margin-bottom:5px;"> </span>
                    </div>
                       </div>
                <div class="portlet-body" id="portlet_bodyadp">     
                    <div class="table-responsive">
                        <table class="table table-striped table-bordered" id="dataGridAdpt">
                                <thead id="tblHeadAdpt">
                                    <tr id="tblHeadRowAdpt">  </tr>
                                </thead>
                                <tbody id="tblBodyAdpt" style="color: #000;">
                                </tbody>
                            </table> 
                    </div>
               </div>                   
         </div>     
       
         <div class="portlet box blue" id="prtServices">
                <div class="portlet-title">
                    <div class="caption">Services</div>
                    <div class="tools">
                        <a href="javascript:;" class="expand"></a>
                    </div>
                       </div>
                    <div class="portlet-body portlet-collapsed"  id="portlet_bodysev">
                      <div class="table-responsive">
                            <table class="table table-striped table-bordered" id="dataGridServ">
                                <thead id="tblHeadServ">
                                    <tr id="tblHeadRowServ">                                     
                                    </tr>
                                </thead>
                                <tbody id="tblBodyServ" style="color: #000;">
                                </tbody>
                            </table>  
                        </div>  
                    </div>
                </div>
      
         <div class="portlet box blue"  id="prtScheduler">
		        	<div class="portlet-title">
                <div class="caption">Scheduler</div>
                          <div class="tools">                            
                        <a href="javascript:;" class="expand"></a>
                    </div>
                   </div>
                        <div class="portlet-body portlet-collapsed"  id="portlet_bodysch">
                             <div class="table-responsive">
                               <table class="table table-striped table-bordered" id="dataGridSch">
                        <thead id="tblHeadScf">
                            <tr id="tblHeadRowSch">                              
                            </tr>
                        </thead>
                        <tbody id="tblBodySch">
                        </tbody>
                    </table>
                            </div>
                           </div>
                    </div>   
       </div>           
     </div>
<%--</div>--%>

     <div id="ModalLicense" class="modal fade" tabindex="-1" style="display:none;">
     <div class="modal-dialog">
       <div class="modal-content">
          <div class="modal-header" style="padding:5px;background-color:#60aee4;">             
              <h5 class="modal-title" style="color:#fff;"></h5>
           </div>
          <div class="modal-body">               
            <div class="row">
                <div class="col-md-12" id="dvlicensedet"> 
                  <div class="row"><div class="col-md-12"><div class="form-group">
                    <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> License Key : </label> </div>
                     <div  class="col-md-9"><input type="text" class="form-control" id="txtLicensekey" disabled value=""/></div>
                   </div></div></div>  
                 </div>
              </div>                                                   
          </div>                         
          <div class="modal-footer" style="text-align:center;">                                                                                                                               
             <button type="button" id="btnSave" class="btn yellow-casablanca mdfootrbtn">Save</button>
             <button type="button" data-dismiss="modal" class="btn yellow-casablanca mdfootrbtn">Close</button>
           </div>                           
        </div>
     </div>
 </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="childScriptHolder" runat="server">
    <%--<script src="../assets/ob/obAdaptor.js"></script>--%>
    <script src="../assets/SM/LES/Adaptors.js"></script>
    <script>  jQuery(document).ready(function () {  Adaptors.init();  }); </script>
        <style type="text/css">
        table{margin: 0 auto;width: 100%;clear: both;border-collapse: collapse;table-layout: fixed;}
        table.dataTable tbody th, table.dataTable tbody td {padding-right: 3px !important;   }       
        .table td, .table th,.table thead tr th {  font-size: 8pt !important;  font-family:Helvetica;  white-space:nowrap;   }
        .block {  display: block;  }
        .gridHeader { border-color: #BFBFBF !important; background-image: none !important;  background-color: #808080 !important;  color: #FAFCFB !important;   font-size:8pt;   font-family:Helvetica; }/*#7d7a7a*/
        .portlet > .portlet-title > .caption {  font-size:10pt;  font-family:Helvetica; }
         .portlet > .portlet-title { min-height:30px;}
        .portlet.box > .portlet-body { background-color: #fff; padding: 8px; font-size:10pt;  font-family:Helvetica; max-width:100%; }
        .hide_column {  display: none;}
        .btn-icon-only { height: 20px; width: 20px; text-align: center; padding-left: 0;  padding-right: 0; }
        /*.portlet.box > .portlet-title { border-bottom: 0;  padding: 0 10px;  margin-bottom: 0; color: #fff; }*/     
        .td-backgroundcolor{  background-color:red;}  
        .dt-center { text-align: center; } 
        /*table.dataTable tr:nth-child(odd)  { background-color: #f6f5e1;  }*/
      a.disabled >i{ pointer-events: none;cursor: default;}
    </style>
</asp:Content>








