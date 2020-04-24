<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseDetail.master" AutoEventWireup="true" CodeBehind="NewBuyerSupplierWizard.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.NewBuyerSupplierWizard" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ChildDetailContent" runat="server">
<div class="row">
  <div class="col-md-12">      
      <div class="portlet light bordered" id="form_wizard_1">
        <div class="portlet-body form" id="portlet_body">
         <form class="form-horizontal" action="#" id="submit_form" method="POST" novalidate="novalidate">
            <div class="form-wizard">
                <div class="form-body" style="padding:0px!important;">
                  <ul class="nav nav-pills nav-justified steps">
                           <li id="lstdet" class="active">
                                 <a href="#tabParty" data-toggle="tab" class="step" aria-expanded="true">
                                    <span class="number"> 1 </span>
                                    <span class="desc"> <i class="fa fa-check"></i> Details </span>
                                 </a>
                            </li>
                            <li id="lstdefsett">
                                <a href="#tabDefSettings" data-toggle="tab" class="step">
                                    <span class="number"> 2 </span>
                                    <span class="desc"> <i class="fa fa-check"></i> Default Settings </span>
                               </a>
                            </li>
                           <li id="lstrule">
                              <a href="#tabDefRules" data-toggle="tab" class="step">
                                  <span class="number"> 3 </span>
                                  <span class="desc"> <i class="fa fa-check"></i> Default Rules </span>
                               </a>
                           </li>                                                    
                              <li id="lstcopy">
                              <a href="#tabCpymappings" data-toggle="tab" class="step">
                                  <span class="number"> 4 </span>
                                  <span class="desc"> <i class="fa fa-check"></i> Copy Mappings </span>
                               </a>
                           </li>                                                    
                              <li id="lstsync">
                              <a href="#tabSyncServer" data-toggle="tab" class="step">
                                  <span class="number"> 5 </span>
                                  <span class="desc"> <i class="fa fa-check"></i> Sync with Servers </span>
                               </a>
                           </li>                                                    
                        </ul>

                  <div id="bar" class="progress progress-striped" role="progressbar" style="margin-bottom:0px!important;">
                                <div class="progress-bar progress-bar-success" style="width: 25%;"> </div>
                         </div>

`                 <div class="tab-content">
                   <div class="tab-pane active" id="tabParty">
                                 <div class="row" style="padding-right:40px;"><div class="col-md-12" id="dvPrtyDet"> </div></div>
                           </div>

                   <div class="tab-pane" id="tabDefSettings">
                                 <div class="row" style="padding-right:40px;"><div class="col-md-12" id="dvDefSt">  </div></div>                                                   
                           </div>

                   <div class="tab-pane" id="tabDefRules"> 
                       <div class="portlet light">  
                             <div class="portlet-body"> 
                               <div class="table table-responsive">
                                    <table  class="table table-bordered table-striped" id="dataGridNewDRule">
                                        <thead id="tblHeadNewDRule">
                                          <tr id="tblHeadRowNewDRule">     </tr>         
                                         </thead>
                                       <tbody id="tblBodyNewDRule" style="color: #000;"></tbody>
                                       </table>      
                                 </div>                   
                                </div>
                          </div>
                  </div>

                   <div class="tab-pane" id="tabCpymappings">
                       <div class="row" style="padding-right:40px;">
                                <div class="col-md-12" id="dvCpyMapp">                                     
                                   <div class="form-body" style="padding:0px!important;"> 
                                     <div class="form-group">
                                         <div  class="col-md-10">     
                                             <div class="col-md-2" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyers </label> </div>
                                             <div  class="col-md-3"><select class="form-control" id="cbBuyers"></select> </div>                                                              
                                        <div id="dvxls" class="col-md-2"><label class="radio-inline"><input type="radio" name="rd" id="rdExcel" checked> Excel</label> </div> 
                                        <div id="dvpdf" class="col-md-2"><label class="radio-inline"><input type="radio" name="rd" id="rdPDF"> PDF</label></div>                                              
                                    </div>                          
                                 </div>                                                                            
                                  </div>                                          
                                  <div class="table table-responsive" id="tblXls" >
                                    <table class="table table-bordered table-striped" id="dataGridBXlsMapp">
                                     <thead id="tblHeadBXlsMapp"> <tr id="tblHeadRowBXlsMapp">     </tr>   </thead>
                                     <tbody id="tblBodyBXlsMapp" style="color: #000;"> </tbody>
                                    </table>  
                                   </div>   
                                                                    
                                  <div class="table table-responsive" id="tblPdf"  style="display:none;">
                                   <table  class="table  table-bordered table-striped" id="dataGridBPdfMapp">
                                     <thead id="tblHeadBPdfMapp">  <tr id="tblHeadRowBPdfMapp">     </tr>      </thead>
                                    <tbody id="tblBodyBPdfMapp" style="color: #000;"> </tbody>
                                  </table>                               
                                 </div>                                                                                                    
                                 </div>  
                             </div>
                     </div>         
        
                  <div class="tab-pane" id="tabSyncServer">
                    <div class="row" style="padding-right:40px;">
                       <div class="col-md-12" id="dvSyncServ"> 
                          <div class="col-md-1"></div> <div class="col-md-5"><i class="fa fa-hand-o-right" style="text-align:center;"></i><span style="font-weight:600;padding-bottom:15px;"> List of Servers </span>
                         <a href="javascript:;" data-toggle="tooltip" title="New Server" class="btn btn-circle btn-icon-only" id="btnNewServer"><i class="fa fa-plus" style="text-align:center;"></i></a>   </div>
                               <%--<div class="col-md-5">  <a href="javascript:;" data-toggle="tooltip" title="New Server" class="btn btn-circle btn-icon-only" id="btnNewServer"><i class="fa fa-plus" style="text-align:center;"></i></a>   </div>--%>
                          <div id="dvServerList">   </div>
                       </div>
                   </div>
                 </div>
                          
                  <div class="form-actions">
                        <div class="row">
                             <div class="col-md-offset-5 col-md-7">
                                  <a href="javascript:;" class="btn btn-pad default button-previous" style="display: none;" id="btnBack"> Back <i class="fa fa-arrow-circle-o-left"></i> </a>
                                  <a href="javascript:;" class="btn btn-pad blue button-next" id="btnNext"> Next <i class="fa fa-arrow-circle-o-right"></i> </a>
                                  <a href="javascript:;" class="btn btn-pad green-jungle button-submit" style="display: none;"  id="btnFinish"> Finish <i class="fa fa-check"></i>  </a>
                            </div>
                       </div>
                   </div>
    
                 </div>
             </div>
           </div>
        </form>
               
         <div id="ModalNewFormat" class="modal fade" tabindex="-1" style="display:none;" data-width="400">
                  <div class="modal-dialog">
                        <div class="modal-content">
                           <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                             <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                             <h5 class="modal-title" style="color:#fff;">Add New Document Format</h5>
                           </div>
                           <div class="modal-body">  <div class="row" style="padding-right:40px;">  <div class="col-md-12" id="dvNewFrmtDet">    </div> </div>  </div>                                                          
                           <div class="modal-footer" style="text-align:center;">                                                                               
                             <button type="button" id="btnAddFormat"  class="btn yellow-casablanca mdfootrbtn">Add</button>                                                   
                             <button type="button" data-dismiss="modal" class="btn yellow-casablanca mdfootrbtn">Cancel</button>
                          </div>                           
                        </div>
                   </div>
                </div>

         <div id="ModalNewServer" class="modal fade" tabindex="-1" style="display:none;" >
                  <div class="modal-dialog">
                        <div class="modal-content">
                           <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                             <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                             <h5 class="modal-title" style="color:#fff;">Add New Server</h5>
                           </div>
                           <div class="modal-body">  <div class="row" style="padding-right:40px;">  <div class="col-md-12" id="dvNewServerDet">    </div> </div>  </div>                                                          
                           <div class="modal-footer" style="text-align:center;">                                                                               
                             <button type="button" id="btnAddServer"  class="btn yellow-casablanca mdfootrbtn">Add</button>                                                   
                             <button type="button" data-dismiss="modal" class="btn yellow-casablanca mdfootrbtn">Cancel</button>
                          </div>                           
                        </div>
                   </div>
                </div>

         <div id="ModalServConfrm" class="modal fade" tabindex="-1" style="display:none;z-index:10010;" data-width="400">
           <div class="modal-dialog" style="width:1000px;">
               <div class="modal-content">
              <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                  <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                   <h5 class="modal-title" style="color:#fff;">Confirmation</h5>
               </div>
              <div class="modal-body">  <div class="row">  <div class="col-md-12" id="dvServCnfrmDet">    </div> </div>  </div>                                                          
              <div class="modal-footer" style="text-align:center;">                                                                               
                   <button type="button" id="btnConfrmServ"  class="btn yellow-casablanca mdfootrbtn">Start Sync</button>                                                   
                    <button type="button"  id="btnConfrmCancel" class="btn yellow-casablanca mdfootrbtn">Cancel</button>
               </div> 
                </div>
            </div>                                         
        </div>

            </div>
      </div>
  </div>
 </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="childScriptHolder" runat="server">
    <link href="../assets/css/NBSLnkWizard.css" rel="stylesheet" />
    <script src="../assets/Scripts/ob/New_BuyerSupplier_Wizard.js"></script>
    <script> jQuery(document).ready(function () {New_BuyerSupplier_Wizard.init();});</script>
</asp:Content>
     <%--$('#ModalServConfrm,#ModalNewServer,#ModalNewFormat').draggable({ handle: ".modal-header,.modal-footer" });--%>