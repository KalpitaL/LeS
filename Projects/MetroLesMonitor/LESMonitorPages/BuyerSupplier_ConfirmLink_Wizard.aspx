<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseDetail.master" AutoEventWireup="true" CodeBehind="BuyerSupplier_ConfirmLink_Wizard.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.BuyerSupplier_ConfirmLink_Wizard" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ChildDetailContent" runat="server">
<div class="row">
  <div class="col-md-12">      
      <div class="portlet light bordered" id="form_wizard_1">
        <div class="portlet-body form" id="portlet_body">
         <form class="form-horizontal" action="#" id="submit_form" method="POST" novalidate="novalidate">
            <div class="form-wizard">
                <div class="form-body" style="padding:0px!important;">
                  <ul class="nav nav-pills nav-justified steps">
                           <li id="lstbyr" class="active">
                                 <a href="#tabByrList" data-toggle="tab" class="step" aria-expanded="true">
                                    <span class="number"> 1 </span>
                                    <span class="desc"> <i class="fa fa-check"></i> Buyer List </span>
                                 </a>
                            </li>                       
                           <li id="lstdrule">
                              <a href="#tabDefRules" data-toggle="tab" class="step">
                                  <span class="number"> 2 </span>
                                  <span class="desc"> <i class="fa fa-check"></i> Default Rules </span>
                               </a>
                           </li>                                                    
                              <li id="lstmapp">
                              <a href="#tabMappings" data-toggle="tab" class="step">
                                  <span class="number"> 3 </span>
                                  <span class="desc"> <i class="fa fa-check"></i> Buyer Mappings </span>
                               </a>
                           </li>                                                                                                                          
                        </ul>
                  <div id="bar" class="progress progress-striped" role="progressbar" style="margin-bottom:0px!important;">
                                <div class="progress-bar progress-bar-success" style="width: 25%;"> </div>  </div>
`                 <div class="tab-content">
                  <div class="tab-pane active" id="tabByrList">
                    <div class="portlet light">  
                       <div class="portlet-body"> 
                         <div class="table table-responsive">
                                <table class="table table-bordered table-striped" id="dataGridNewBuy">
                                  <thead id="tblHeadNewBuy">
                                       <tr id="tblHeadRowNewBuy">     </tr>         
                                  </thead>
                                  <tbody id="tblBodyNewBuy" style="color: #000;"></tbody>
                                  </table>                                                
                         </div>                   
                       </div>
                     </div>
                   </div>
                   <div class="tab-pane" id="tabDefRules">    
                       <div class="portlet-light">
                        <div id="dvbuyerRules"> 
                           <div class="portlet-title">
                              <div class="caption font-red-sunglo">
                                  <i class="fa fa-hand-o-right"></i><span class="caption-subject bold"> Buyer </span>
                             </div>
                             <div class="portlet-body"> 
                               <div class="table table-responsive">
                                    <table class="table table-bordered table-striped" id="dataGridByrDRule">
                                        <thead id="tblHeadByrDRule">
                                          <tr id="tblHeadRowByrDRule">     </tr>         
                                         </thead>
                                       <tbody id="tblBodyByrDRule" style="color: #000;"></tbody>
                                       </table>      
                                 </div>                   
                                </div>
                          </div>
                       </div>
                        <div class="clearfix"></div>
				        <div id="dvsuppRules"> 
                           <div class="portlet-title">
                              <div class="caption font-red-sunglo">
                                   <i class="fa fa-hand-o-right"></i><span class="caption-subject bold">Supplier </span>
                             </div>
                           </div>
                           <div class="portlet-body"> 
                               <div class="table table-responsive">
                                    <table class="table table-bordered table-striped" id="dataGridSppDRule">
                                        <thead id="tblHeadSppDRule">
                                          <tr id="tblHeadRowSppDRule">     </tr>         
                                         </thead>
                                       <tbody id="tblBodySppDRule" style="color: #000;"></tbody>
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
                         <table class="table table-bordered table-striped" id="dataGridXlsMapp">
                            <thead id="tblHeadXlsMapp"> <tr id="tblHeadRowXlsMapp"></tr> </thead>
                           <tbody id="tblBodyXlsMapp" style="color: #000;"> </tbody>
                        </table>  
                     </div> 						   
                         <div class="table table-responsive" id="tblPdf" style="display:none;" >
                        <table  class="table table-bordered table-striped" id="dataGridPdfMapp">
                            <thead id="tblHeadPdfMapp">  <tr id="tblHeadRowPdfMapp"></tr> </thead>
                            <tbody id="tblBodyPdfMapp" style="color: #000;"> </tbody>
                        </table>          
                     </div> 				    							   
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
            </div>
      </div>
  </div>
 </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="childScriptHolder" runat="server">
   <link href="../assets/css/plugins.css" rel="stylesheet" />
   <link href="../assets/css/ByrSppConfmLnkWiz.css" rel="stylesheet" />
    <script src="../assets/Scripts/ob/BuyerSupplier_ConfirmLink_Wizard.js"></script>
    <script> jQuery(document).ready(function () { BuyerSupplier_ConfirmLink_Wizard.init(); });</script>
</asp:Content>
