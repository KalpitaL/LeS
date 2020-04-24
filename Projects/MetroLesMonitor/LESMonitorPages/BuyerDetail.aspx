<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseDetail.master" AutoEventWireup="true" CodeBehind="BuyerDetail.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.BuyerDetail" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ChildDetailContent" runat="server">   
<form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>      
    <div class="row">
       <div class="col-md-12">
           <div class="portlet light portlet-fit portlet-datatable">     
                <div id="headerTabs" class="portlet-body">                    
                   <ul id="hTabs" class="nav nav-tabs" >
                      <li  data-id="lnkDetail" class="active"><a href="javscript:void(0)"  id="0" data-toggle="tab">Detail</a></li>
                      <li data-id="lnkSpRules"><a href="javscript:void(0)"  id="1" data-toggle="tab">Default Rules</a></li> 
                       <li data-id="lnkSuppliers"><a href="javscript:void(0)"  id="2" data-toggle="tab">Linked Suppliers</a></li> 
                       <li data-id="lnkBMappings"><a href="javscript:void(0)"  id="3" data-toggle="tab">Mappings</a></li>  
                       <li data-id="lnkBTransactn"><a href="javscript:void(0)"  id="4" data-toggle="tab">Transactions</a></li>  
                    </ul>

        <div id="prtDetail" style="margin-top:-10px;">
          <div class="portlet light">
            <div class="portlet-body">
                <div id="HeaderDiv" class="portlet light">
                           <div class="portlet-body" id="divDetail"> </div>
                            <div class="clearfix"> </div>
                </div>
            </div>
        </div>

          <div id="prtByrConfig" style="margin-top:-20px;">
         <div class="portlet light">
          <div class="portlet-title" style="margin-bottom:0px!important;border-top:1px solid #eee;min-height:0px!important;">
              <div class="caption font-red-sunglo">
                    <i class="fa fa-hand-o-right"></i><span class="caption-subject bold"> Default Settings</span>                                       
             </div>
          </div>
            <div class="portlet-body">
                <div id="HeaderCnfgDiv" class="portlet light">
                  <div class="portlet-body" id="divByrConfg"> </div>
                    <div class="clearfix"> </div>    
                 </div>
            </div>
        </div>
        </div>
                                                 
            </div>
                                         
        <div id="prtByrRules" style="display:none;">     
            <div class="portlet light">  
                <div class="portlet-body"> 
                     <div class="table table-responsive">
                         <table class="table table-bordered table-striped" id="dataGridBRules">
                            <thead id="tblHeadBRule">
                               <tr id="tblHeadRowBRule">     </tr>         
                            </thead>
                            <tbody id="tblBodyBRule" style="color: #000;"> </tbody>
                        </table>                               
                     </div>   
                    
                     <div id="ModalDefRule" class="modal fade bg-modal-lg draggable-modal" tabindex="-1" style="display:none;z-index:100009;"">
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content">
                                <div class="modal-header" style="padding:5px;background-color:#60aee4;">
                                 <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                                  <h5 class="modal-title" style="color:#fff;">Add Default Rule</h5>
                                </div>

                                <div class="modal-body">
                            <div class="row">
                            <div class="col-md-12" id="dvDefRule">
                                <div class="table table-responsive">
                                    <table class="table  table-bordered table-striped" id="dataGridNewDRule">
                                        <thead id="tblHeadNewDRule">
                                          <tr id="tblHeadRowNewDRule">     </tr>         
                                         </thead>
                                       <tbody id="tblBodyNewDRule" style="color: #000;"></tbody>
                                       </table>      
                                 </div>                    
                            </div>
                        </div>                                                   
                         </div>  
                                                                                         
                                <div class="modal-footer" style="text-align:center;">                                                                               
                      <button type="button" id="btnDefNew"  class="btn yellow-casablanca"  style="font-size:8pt;padding: 6px 6px;width:50px;">Add</button>                                                   
                      <button type="button" data-dismiss="modal" class="btn yellow-casablanca"  style="font-size:8pt;padding: 6px 6px;">Cancel</button>
                   </div>                            
                            </div>
                         </div>
                     </div>                                              
               </div>
            </div> 
       </div>         
                    
        <div id="prtLnkSupp" style="display:none;">
          <div class="portlet light">  
                <div class="portlet-body"> 
                     <div class="table table-responsive">
                         <table class="table table-bordered table-striped" id="dataGridBSlnk">
                              <thead id="tblHeadBSlnk">
                                   <tr id="tblHeadRowBSlnk">     </tr>         
                              </thead>
                             <tbody id="tblBodyBSlnk" style="color: #000;"> </tbody>
                       </table>                               
                     </div> 
                    
                 </div>
            </div> 
         </div>       

        <div id="prtBMapping" style="display:none;">
            <div class="portlet light form-fit" id="divMFilter">
                  <div class="portlet-title margin-bottom-0 padding-0">                          
                     <div class="row">                           
                        <div class="portlet-body form">
                            <div class="form-body" style="padding:0px!important;"> 
                                 <div class="form-group">
                                    <div class="col-md-10">
                                        <div id="dvxls" class="col-md-3"><label class="radio-inline"><input type="radio" name="rd" id="rdExcel" checked>Excel</label> </div> 
                                        <div id="dvpdf" class="col-md-3"><label class="radio-inline"><input type="radio" name="rd" id="rdPDF">PDF</label></div>       
                                        <div id="dvInv" class="col-md-3" style="display:none;"><label class="radio-inline"><input type="radio" name="rd" id="rdInvoice">Invoice</label></div> 
                                    </div>                          
                                 </div>                                                                            
                            </div>                     
                        </div>                        
                     </div>
                  </div>
             </div>
                
          <div class="portlet light">  
                <div class="portlet-body"> 
                     <div class="table table-responsive" id="tblXls" >
                         <table class="table table-bordered table-striped" id="dataGridBXlsMapp">
                              <thead id="tblHeadBXlsMapp">
                                   <tr id="tblHeadRowBXlsMapp">     </tr>         
                              </thead>
                             <tbody id="tblBodyBXlsMapp" style="color: #000;"> </tbody>
                       </table>  
                     </div>                                   
                     <div class="table table-responsive" id="tblPdf"  style="display:none;">
                         <table class="table table-bordered table-striped" id="dataGridBPdfMapp">
                              <thead id="tblHeadBPdfMapp">
                                   <tr id="tblHeadRowBPdfMapp">     </tr>         
                              </thead>
                             <tbody id="tblBodyBPdfMapp" style="color: #000;"> </tbody>
                       </table>                               
                    </div>
                    <div class="table table-responsive" id="tblInv"  style="display:none;">
                         <table class="table table-bordered table-striped" id="dataGridBInvMapp">
                              <thead id="tblHeadBInvMapp">
                                   <tr id="tblHeadRowBInvMapp">     </tr>         
                              </thead>
                             <tbody id="tblBodyBInvMapp" style="color: #000;"> </tbody>
                       </table>                               
                    </div>                
                 </div>
            </div> 
       </div>

        <div id="prtBTransact" style="display:none;">
           <div id="dvOverview" style="display:none;">                   
               <ul class="nav navbar-nav pull-right" style="margin-top:-10px;">                     
                       <li id="Ovrfq">
                           <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" data-close-others="true">
                           <div> RFQ <span class="badge badge-danger" id="spnRfq">  </span>   </div> </a>       
                            <ul class="dropdown-menu">
                                 <div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 130px;padding:7px 0 0 4px;">
                                     <i class="fa fa-hand-o-right"></i> <span class="caption-subject font-red-soft bold uppercase">RFQ Overview</span>
                                     <div class="dropdown-menu-list" style="height: 130px; overflow: hidden; width: auto;padding:7px;margin-left:15px;" data-handle-color="#637283" data-initialized="1">
                                           <div class="title"> Last Week : <span id="spnRfqLastWk">  </span>   </div> 
                                           <div class="title"> This Month : <span id="spnRfqTMth">  </span>   </div>  
                                           <div class="title"> Last Month : <span id="spnRfqLMth">  </span>   </div> 
                                           <div class="title leftpad"> This Year : <span id="spnRfqTYr"> </span>   </div>   
                                     </div>
                                </div>
                            </ul>                                            
                        </li>
                       <li>
                           <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" data-close-others="true">
                           <div> Quote <span class="badge badge-primary" id="spnQte">  </span>   </div> </a>       
                            <ul class="dropdown-menu">
                                 <div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 130px;padding:7px 0 0 4px;">
                                     <i class="fa fa-hand-o-right"></i> <span class="caption-subject font-red-soft bold uppercase">Quote Overview</span>
                                     <div class="dropdown-menu-list" style="height: 130px; overflow: hidden; width: auto;padding:7px;margin-left:15px;" data-handle-color="#637283" data-initialized="1">
                                           <div class="title"> Last Week : <span id="spnQteLastWk">  </span>   </div> 
                                           <div class="title"> This Month : <span id="spnQteTMth">  </span>   </div>  
                                           <div class="title"> Last Month : <span id="spnQteLMth">  </span>   </div> 
                                           <div class="title leftpad"> This Year : <span id="spnQteTYr"> </span>   </div>   
                                     </div>
                                </div>
                            </ul>                                         
                       </li> 
                       <li>
                           <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" data-close-others="true">
                           <div> PO <span class="badge badge-success" id="spnPO">  </span>   </div> </a>       
                            <ul class="dropdown-menu">
                               <div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 130px;padding:7px 0 0 4px;">
                                     <i class="fa fa-hand-o-right"></i> <span class="caption-subject font-red-soft bold uppercase">PO Overview</span>
                                     <div class="dropdown-menu-list" style="height: 130px; overflow: hidden; width: auto;padding:7px;margin-left:15px;" data-handle-color="#637283" data-initialized="1">
                                           <div class="title"> Last Week : <span id="spnPOLastVWk">  </span>   </div> 
                                           <div class="title"> This Month : <span id="spnPOTMth">  </span>   </div>  
                                           <div class="title"> Last Month : <span id="spnPOLMth">  </span>   </div> 
                                           <div class="title leftpad"> This Year : <span id="spnPOTYr"> </span>   </div>   
                                     </div>
                                </div>
                            </ul>   
                       </li> 
                       <li>
                           <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" data-close-others="true">
                           <div> POC <span class="badge badge-warning" id="spnPOC"> </span> </div> </a>       
                            <ul class="dropdown-menu">
                               <div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 130px;padding:7px 0 0 4px;">
                                     <i class="fa fa-hand-o-right"></i> <span class="caption-subject font-red-soft bold uppercase">POC Overview</span>
                                     <div class="dropdown-menu-list" style="height: 130px; overflow: hidden; width: auto;padding:7px;margin-left:15px;" data-handle-color="#637283" data-initialized="1">
                                           <div class="title"> Last Week : <span id="spnPOCLastWk">  </span>   </div> 
                                           <div class="title"> This Month : <span id="spnPOCTMth">  </span>   </div>  
                                           <div class="title"> Last Month : <span id="spnPOCLMth">  </span>   </div> 
                                           <div class="title leftpad"> This Year : <span id="spnPOCTYr"> </span>   </div>   
                                     </div>
                                </div>
                            </ul>   
                       </li>
              </ul>
            </div>
             <div class="portlet light form-fit" id="divFilter">
                  <div class="portlet-title margin-bottom-0 padding-0">                          
                      <div class="row">                           
                           <div class="portlet-body form"> </div>
                           <div class="clearfix"></div>
                     </div>
                  </div>
             </div>
          <div class="portlet light">  
                <div class="portlet-body"> 
                     <div class="table table-responsive">
                         <table  class="table table-bordered table-striped" id="dataGridBTrans">
                              <thead id="tblHeadBTrans">
                                   <tr id="tblHeadRowBTrans">     </tr>         
                              </thead>
                             <tbody id="tblBodyBTrans" style="color: #000;"> </tbody>
                       </table>                               
                     </div>                         
                 </div>
            </div> 
              </div>
                       
        <div id="ModalUpload_Download" class="modal fade" tabindex="-1" data-width="1000" style="display:none;z-index:100017;">
            <div class="modal-dialog">                
                  <div class="modal-content">
                       <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#3598dc;">
                          <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                           <h5 class="modal-title"  id="updwlcapt" style="color:#fff;"></h5>
                       </div>

                       <div class="modal-body">
                          <div class="row">
                              <div class="col-md-12" id="dvMappUpload">
                                   <div class="form-group">
                                      <div style="text-align:center">
                                         <button type="button" id="btnDownload"  class="btn purple" style="font-size:12px;padding: 6px 6px;"></button>
                                      </div>
                                  </div>
                              </div>   
                                                             
                            <div class="col-md-12" id="dvMappDownload" style="display:none;">
                              <span style=" margin-top: 5px; text-align:right;">Mapping File :</span>
                                <div style="margin-bottom:10px;">
                                   <ajaxToolkit:AjaxFileUpload ID="fupFileMapp" runat="server" MaximumNumberOfFiles="1" />
                                </div>
                            </div>                                                      
                                                                                           
                          </div>                                                   
                      </div>  
                    </div>                       
              <div class="modal-footer" style="padding-right:25px;">   </div>                                                
          </div>
        </div>       
                   
         <div id="ModalSyncServers" class="modal fade" tabindex="-1" style="display:none;z-index:10040;" data-width="400">
                  <div class="modal-dialog">
                        <div class="modal-content">
                           <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                             <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                             <h5 class="modal-title" style="color:#fff;">Sync Servers</h5>
                           </div>
                           <div class="modal-body">  <div class="row" >  <div class="col-md-12" id="dvServerdet">                               
                                <div class="row" style="padding-right:20px;padding-left:20px;">
                                 <table id="tblSupp" class="table table-bordered table-striped">
                                    <tbody>
                                        <tr>
                                           <td style="width:15%"> Supplier </td>
                                            <td style="width:85%"><span class="text-muted" id="spnSuppcodes"></span></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"> List of Servers :- </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="col-md-12" id="dvServerList">    </div>
                                            </td>
                                        </tr>
                                         </tbody>
                                  </table>
                                </div>

                            </div> </div>  </div>                                                          
                           <div class="modal-footer" style="text-align:center;">                                                                               
                             <button type="button" id="btnConfrmServ"  class="btn yellow-casablanca mdfootrbtn">Confirm Sync</button>                                                   
                             <button type="button" data-dismiss="modal" class="btn yellow-casablanca mdfootrbtn">Cancel</button>
                          </div>                           
                        </div>
                   </div>
                </div>

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
</form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="childScriptHolder" runat="server"> 
    <link href="../assets/css/buyers.css" rel="stylesheet" />
    <script src="../assets/Scripts/ob/Buyerdetail.js"></script>
    <script> jQuery(document).ready(function () { $("#baseBody").fadeIn(); BuyerDetail.init();  });</script>
</asp:Content>
