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
                                    <table class="table table-striped table-bordered" id="dataGridNewDRule">
                                        <thead id="tblHeadNewDRule" class="bg-grey-silver !Important">
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
                                    <table class="table table-striped table-bordered " id="dataGridBXlsMapp">
                                     <thead id="tblHeadBXlsMapp" class="bg-grey-silver !Important"> <tr id="tblHeadRowBXlsMapp">     </tr>   </thead>
                                     <tbody id="tblBodyBXlsMapp" style="color: #000;"> </tbody>
                                    </table>  
                                   </div>   
                                                                    
                                  <div class="table table-responsive" id="tblPdf"  style="display:none;">
                                   <table class="table table-striped table-bordered" id="dataGridBPdfMapp">
                                     <thead id="tblHeadBPdfMapp" class="bg-grey-silver !Important">  <tr id="tblHeadRowBPdfMapp">     </tr>      </thead>
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
<link href="../assets/Login/css/plugins.css" rel="stylesheet" />
<link href="../assets/global/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.css" rel="stylesheet" />
   <style type="text/css">     
        #dataGridNewDRule{margin: 0 auto;width: 100%;clear: both;border-collapse: collapse;table-layout: fixed;}
        .form-wizard .steps { padding: 1px 0;margin-bottom: 10px;background-color: #fff;background-image: none;filter: none;border: 0px;box-shadow: none;}
       .btn-pad { padding:6px 12px;font-weight:400;font-size:14px; }.closebtn{text-align:center;margin-top:8px;}
       .form-horizontal .radio>span { margin-top: -2px;}
       .nav>li>a { position: relative;display: block; padding: 5px 5px;}
         .dropdown-menu{min-width:160px;}
          .leftpad{padding-left:10px;}
          .dropdown-menu { box-shadow: 5px 5px rgba(102,102,102,.1); left: 0;  min-width: 160px;  position: absolute; z-index: 1000; display: none; float: left;list-style: none;text-shadow: none;padding: 0;background-color: #fff3f3;margin: 10px 0 0;border: 1px solid #e46c6c; font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;  -webkit-border-radius: 4px;-moz-border-radius: 4px;-ms-border-radius: 4px;-o-border-radius: 4px;border-radius: 4px;}
           .dropdown-header, .dropdown-menu>li>a{}
          .dropdown-menu > li > a {padding:0px;color: #6f6f6f;text-decoration: none; display: block;clear: both; margin-left:30px; font-weight: 300; line-height: 18px;white-space: nowrap;}  
   </style>    
    <script src="../assets/global/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js"></script>
    <script src="../assets/global/plugins/bootstrap-wizard/jquery.bootstrap.wizard.new.js"></script>
    <script src="../assets/ob/obNewBuyerSupplierWizard.js"></script>
    <script> jQuery(document).ready(function () {  New_BuyerSupplier_Wizard.init(); $('#ModalServConfrm,#ModalNewServer,#ModalNewFormat').draggable({ handle: ".modal-header,.modal-footer" });});</script>
</asp:Content>
