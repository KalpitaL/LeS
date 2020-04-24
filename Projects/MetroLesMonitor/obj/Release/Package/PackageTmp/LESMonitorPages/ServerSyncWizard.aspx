<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseDetail.master" AutoEventWireup="true" CodeBehind="ServerSyncWizard.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.ServerSyncWizard" %>
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
                                    <span class="desc"> <i class="fa fa-check"></i> Details & Default Rules</span>
                                 </a>
                            </li>                                                                                        
                              <li id="lstMapp">
                              <a href="#tabMappings" data-toggle="tab" class="step">
                                  <span class="number"> 2 </span>
                                  <span class="desc"> <i class="fa fa-check"></i> Mappings </span>
                               </a>
                           </li>                                                    
                              <li id="lstSync">
                              <a href="#tabSyncServer" data-toggle="tab" class="step">
                                  <span class="number">3 </span>
                                  <span class="desc"> <i class="fa fa-check"></i> Sync with Servers </span>
                               </a>
                           </li>                                                    
                        </ul>

                  <div id="bar" class="progress progress-striped" role="progressbar" style="margin-bottom:0px!important;">
                                <div class="progress-bar progress-bar-success" style="width: 25%;"> </div>
                         </div>

`                 <div class="tab-content">
                   <div class="tab-pane active" id="tabParty">
                     <div class="row" style="padding-right:20px;padding-left:20px;">
                        <table id="tblDet" class="table table-bordered table-striped">
                            <tbody>
                               <tr>
                                   <td style="width:10%;text-align:right;font-weight:700;"> Code </td>
                                   <td style="width:10%"><span class="text-muted" id="spnCode"></span></td>
                                   <td style="width:20%;text-align:right;font-weight:700;"> Description </td>
                                   <td style="width:20%"><span class="text-muted" id="spnDesc"></span></td>                                    
                                   <td style="width:20%;text-align:right;font-weight:700;"> Document Format </td>
                                   <td style="width:20%" colspan="3"><span class="text-muted" id="spnDocFrmt"></span></td>
                                </tr>
                             </tbody>
                          </table>
                     </div>
                      <div id="prtDefRules">
                         <div class="portlet light">
                            <div class="portlet-title" style="margin-bottom:0px!important;border-top:1px solid #eee;min-height:0px!important;">
                              <div class="caption font-red-sunglo">
                                    <i class="fa fa-hand-o-right"></i><span class="caption-subject bold"> Default Rules</span>                                       
                             </div>
                           </div>
                         <div class="portlet-body"> 
                               <div class="table table-responsive">
                                    <table class="table table-striped table-bordered" id="dataGridDRule">
                                        <thead id="tblHeadDRule" class="bg-grey-silver !Important">
                                          <tr id="tblHeadRowDRule">     </tr>         
                                         </thead>
                                       <tbody id="tblBodyDRule" style="color: #000;"></tbody>
                                       </table>      
                                 </div>                   
                                </div>
                          </div>
                                 </div>
                      </div>

                   <div class="tab-pane" id="tabMappings">
                       <div class="row" style="padding-right:40px;">
                                <div class="col-md-12" id="dvMapp">                                     
                                   <div class="form-body" style="padding:0px!important;"> 
                                     <div class="form-group">
                                       <div  class="col-md-10">                                                              
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
                     <div class="row" style="padding-right:20px;padding-left:20px;">
                        <table id="tblSync" class="table table-bordered table-striped">
                          <tbody>                                    
                              <tr><td colspan="2"> List of Servers :- </td> </tr>
                              <tr>
                                            <td colspan="2">
                                                <div class="col-md-12" id="dvServerList">    </div>
                                            </td>
                                        </tr>
                          </tbody>
                         </table>
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
        </form>
 
         <div id="ModalServConfrm" class="modal fade" tabindex="-1" style="display:none;z-index:10041;" data-width="400">
           <div class="modal-dialog" style="width:1000px;">
               <div class="modal-content">
              <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                  <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                   <h5 class="modal-title" style="color:#fff;">Confirmation</h5>
               </div>
              <div class="modal-body">  <div class="row">  <div class="col-md-12" id="dvServCnfrmDet">    </div> </div>  </div>                                                          
              <div class="modal-footer" style="text-align:center;">                                                                               
                   <button type="button" id="btStartServSync"  class="btn yellow-casablanca mdfootrbtn">Start Sync</button>                                                   
                   <button type="button"  id="btnSyncCancel" class="btn yellow-casablanca mdfootrbtn">Cancel</button>
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
   <style type="text/css">     
        #dataGridNewDRule{margin: 0 auto;width: 100%;clear: both;border-collapse: collapse;table-layout: fixed;}
        .form-wizard .steps { padding: 1px 0;margin-bottom: 10px;background-color: #fff;background-image: none;filter: none;border: 0px;box-shadow: none;}
       .btn-pad { padding:6px 12px;font-weight:400;font-size:14px; }.closebtn{text-align:center;margin-top:8px;}
       .form-horizontal .radio>span { margin-top: -2px;}
       .nav>li>a { position: relative;display: block; padding: 5px 5px;}  
   </style>    
    <script src="../assets/global/plugins/jquery-ui/jquery-ui.min.js"></script>  
    <script src="../assets/global/plugins/bootstrap-wizard/jquery.bootstrap.wizard.new.js"></script>
    <script src="../assets/ob/obServerSyncWizard.js"></script>
    <script> jQuery(document).ready(function () { ServerSyncWizard.init(); $('#ModalServConfrm,#ModalNewServer,#ModalNewFormat').draggable({ handle: ".modal-header,.modal-footer" }); });</script>
</asp:Content>
