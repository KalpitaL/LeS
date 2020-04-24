<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseDetail.master" AutoEventWireup="true" CodeBehind="SupplierDetail.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.SupplierDetail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ChildDetailContent" runat="server">    
  <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" ScriptMode="Release"></asp:ToolkitScriptManager>               
      <div class="row">
           <div class="col-md-12">
               <div class="portlet light portlet-fit portlet-datatable">
      
                <div id="headerTabs" class="portlet-body">
                   <ul id="hTabs" class="nav nav-tabs" >
                      <li id="lnkDetail" class="active"><a href="javscript:void(0)"  id="0" data-toggle="tab">Detail</a></li>
                      <li id="lnkSpRules"><a href="javscript:void(0)"  id="1" data-toggle="tab">Default Rules</a></li>                       
                      <li id="lnkBySuppLink"><a href="javscript:void(0)"  id="2" data-toggle="tab">Linked Buyers</a></li>                                                   
                       <li id="lnkSMappings"><a href="javscript:void(0)"  id="3" data-toggle="tab">Mappings</a></li>  
                       <li id="lnkSTransactn"><a href="javscript:void(0)"  id="4" data-toggle="tab">Transactions</a></li>  
                    </ul>

             <div class="portlet light" id="prtDetail"  style="margin-top:-10px;">
            <div class="portlet-body">
                <div id="HeaderDiv" class="portlet light">
                    <div class="portlet-body" id="divDetail"> </div>
                     <div class="clearfix"> </div>                   
                </div>
            </div>
          </div>

          <div id="prtByLnk" style="display:none;">
              <div class="portlet light">
                   <div class="portlet-body">
                      <div class="table table-responsive">
                            <table class="table table-striped table-bordered " id="dataGridBSlnk">
                                <thead id="tblHeadBSlnk" class="bg-grey-silver !Important">
                                    <tr id="tblHeadRowBSlnk">  </tr>         
                                </thead>
                                <tbody id="tblBodyBSlnk" style="color: #000;"> </tbody>
                           </table>                                
                       </div>   
                    </div>   
               </div>
          </div>                                                 

        <div id="prtSuppConfig" style="margin-top:-20px;">
         <div class="portlet light">
           <div class="portlet-title" style="margin-bottom:0px!important;border-top:1px solid #eee;min-height:0px!important;">
              <div class="caption font-red-sunglo">
                    <i class="fa fa-hand-o-right"></i><span class="caption-subject bold"> Default Settings</span>                                       
             </div>
          </div>
            <div class="portlet-body">
                <div id="HeaderCnfgDiv" class="portlet light">
                  <div class="portlet-body" id="divSuppConfg"> </div>
                    <div class="clearfix"> </div>    
                 </div>
            </div>
        </div>
        </div>               

        <div id="prtSuppRules" style="display:none;">     
            <div class="portlet light">  
                <div class="portlet-body"> 
                     <div class="table table-responsive">
                         <table class="table table-striped table-bordered " id="dataGridSRules">
                            <thead id="tblHeadSRule" class="bg-grey-silver !Important">
                               <tr id="tblHeadRowSRule">     </tr>         
                            </thead>
                            <tbody id="tblBodySRule" style="color: #000;"> </tbody>
                        </table>                               
                     </div>                         
                 </div>
            </div> 
       </div>
                    
        <div id="prtSMapping" style="display:none;">
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
                         <table class="table table-striped table-bordered " id="dataGridSXlsMapp">
                              <thead id="tblHeadSXlsMapp" class="bg-grey-silver !Important">
                                   <tr id="tblHeadRowSXlsMapp">     </tr>         
                              </thead>
                             <tbody id="tblBodySXlsMapp" style="color: #000;"> </tbody>
                       </table>  
                     </div>    
                                                   
                     <div class="table table-responsive" id="tblPdf"  style="display:none;">
                         <table class="table table-striped table-bordered" id="dataGridSPdfMapp">
                              <thead id="tblHeadSPdfMapp" class="bg-grey-silver !Important">
                                   <tr id="tblHeadRowSPdfMapp">     </tr>         
                              </thead>
                             <tbody id="tblBodySPdfMapp" style="color: #000;"> </tbody>
                       </table>                               
                    </div>

                     <div class="table table-responsive" id="tblInv"  style="display:none;">
                         <table class="table table-striped table-bordered " id="dataGridBInvMapp">
                              <thead id="tblHeadSInvMapp" class="bg-grey-silver !Important">
                                   <tr id="tblHeadRowSInvMapp">     </tr>         
                              </thead>
                             <tbody id="tblBodySInvMapp" style="color: #000;"> </tbody>
                       </table>                               
                    </div>                
                 </div>
            </div> 
        </div>

        <div id="prtSTransact" style="display:none;">
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
                         <table class="table table-striped table-bordered" id="dataGridSTrans">
                              <thead id="tblHeadSTrans" class="bg-grey-silver !Important">
                                   <tr id="tblHeadRowSTrans">     </tr>         
                              </thead>
                             <tbody id="tblBodySTrans" style="color: #000;"> </tbody>
                       </table>                               
                     </div>                         
                 </div>
            </div> 
              </div>

      <div class="min-height">      </div>
               </div>
            </div>
                         
              <div id="ModalNewBSItemDet" class="modal fade in" tabindex="-1" style="display:none;z-index:100005;">
                 <div class="modal-dialog modal-full">
                    <div class="modal-content">
                       <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                        <%--    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>--%>
                                <h5 class="modal-title" style="color:#fff;">Buyer Supplier Details</h5>
                        </div>
                       <div class="modal-body" style="padding-bottom:1px;padding:12px;">
                           <div class="row">
                          <div class="col-md-12 col-sm-12">
                              <div class="portlet custom">
                                    <div class="portlet-title margin-bottom-0">
                                        <div id="Itemtoolbtngroup"> </div>
                                     </div>
                               </div>
                      </div>
                        </div>

                           <div class="row">
                                 <div class="col-md-12">
                                     <div class="portlet light portlet-fit portlet-datatable">                           
                                       <div id="ItemheaderTabs" class="portlet-body">
                                         <ul id="hItmTabs" class="nav nav-tabs" >
                                          <li id="lnkLnkRule"><a href="javscript:void(0)" id="101"  data-toggle="tab">Linked Rules</a></li>   
                                          <li id="lnkItemRef"><a href="javscript:void(0)" id="102"   data-toggle="tab">Item Reference Mapping</a></li>
                                          <li id="lnkItemUOM"><a href="javscript:void(0)" id="103"  data-toggle="tab">Item UOM Reference Mapping</a></li>  
                                                                       
                                       </ul>
                             <div class="row" id="prtItemRef" style="display:none;">
                                 <div class="col-md-12" id="dvItemref">
                                     <div class="portlet light" id="prtBSItemRef">
                                         <div class="portlet-body">
                                            <div class="table table-responsive">
                                                <table class="table table-striped table-bordered " id="dataGridBSItemRef">
                                                   <thead id="tblHeadBSItemRef" class="bg-grey-silver !Important">
                                                       <tr id="tblHeadRowBSItemRef"> </tr>         
                                                   </thead>
                                                   <tbody id="tblBodyBSItemRef" style="color: #000;"> </tbody>
                                                  </table>               
                                            </div>   
                                          </div>   
                                      </div>
                                 </div>                                              
                             </div>

                             <div class="row" id="prtItemUOM" style="display:none;">
                                 <div class="col-md-12" id="dvItemUOM">
                                     <div class="portlet light" id="prtBSItemUOM">
                                         <div class="portlet-body">
                                            <div class="table table-responsive">
                                                <table class="table table-striped table-bordered " id="dataGridBSItemUOM">
                                                   <thead id="tblHeadBSItemUOM" class="bg-grey-silver !Important">
                                                       <tr id="tblHeadRowBSItemUOM">  </tr>         
                                                   </thead>
                                                    <tbody id="tblBodyBSItemUOM" style="color: #000;"> </tbody>
                                                 </table>                                
                                            </div>   
                                          </div>   
                                      </div>
                                 </div>                                              
                             </div>

                            <div class="row" id="prtBSLnkRules" style="display:none;">
                                 <div class="col-md-12" id="dvBSLnkRules">
                                    <div class="portlet light" id="prtBSLkRules">
                                        <div class="portlet-body">
                                          <div class="table table-responsive">
                                              <table class="table table-striped table-bordered " id="dataGridBSLnkRules">
                                                 <thead id="tblHeadBSLnkRules" class="bg-grey-silver !Important">   <tr id="tblHeadRowBSLnkRules">  </tr> </thead>
                                                 <tbody id="tblBodyBSLnkRules" style="color: #000;"> </tbody>
                                              </table>                                               
                                           </div>   
                                         </div>   
                                    </div>
                                 </div>                                              
                             </div>
                                      </div>
                                    </div>    
                                  </div>                                                                     
                                </div>                                     
                        </div>                                                         
                       <div class="modal-footer" style="text-align:center;display:none;">                                                                                                                               
                              <button type="button" data-dismiss="modal" class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Close</button>
                           </div>                           
                       </div>                   
                    </div>
                </div>         

              <div id="ModalBSdet" class="modal fade" tabindex="-1" data-width="1000" style="display:none;z-index:10010;overflow-y:hidden;">
                   <div class="modal-dialog modal-full">
                       <div class="modal-content">
                         <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#3598dc;">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                                <h5 class="modal-title" style="color:#fff;"> Buyer-Supplier Link Details</h5>
                         </div>
                         <div class="modal-body">
                               <div class="row">
                                    <div class="col-md-12" id="dvBSDet">                                                       
                                    </div>
                                </div>                                                   
                        </div>                                                 
                        <div class="modalFtr" style="text-align:center;">                                                                               
                              <button type="button" id="btnBSUpdate"  class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Update</button>                                                   
                              <button type="button" data-dismiss="modal" class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Cancel</button>
                        </div>                           
                      </div>
                   </div>
              </div>
                
              <div id="ModalNewBSItemRef" class="modal fade" tabindex="-1" data-width="1000" style="display:none;z-index:100006;">
                   <div class="modal-dialog" style="width:800px;">
                                 <div class="modal-content">
                                         <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#3598dc;">
                                   <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                                    <h5 class="modal-title" style="color:#fff;">Add New Buyer-Supplier Item Reference Mapping</h5>
                              </div>
                               <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-12" id="dvNewBSItemRefDet">                                                       
                                    </div>
                                </div>                                                   
                             </div>      
                   
                             <div class="modal-footer" style="padding-right:25px;text-align:center;">                                                                               
                              <button type="button" id="btnNewBSItemRef"  class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Update</button>                                                   
                              <button type="button" data-dismiss="modal" class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Cancel</button>
                              </div>                           
                                      </div>
                            </div>
               </div>

              <div id="ModalNewBSUOMItemRef" class="modal fade" tabindex="-1" data-width="1000" style="display:none;z-index:100006;">
                  <div class="modal-dialog modal-lg">
                                  <div class="modal-content">
                                         <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#3598dc;">
                                   <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                                    <h5 class="modal-title" style="color:#fff;">Add Buyer-Supplier Item UOM Reference Mapping</h5>
                              </div>
                               <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-12" id="dvNewBSUOMItemRef">
                                                       
                                    </div>
                                </div>                                                   
                             </div>      
                             <div class="modal-footer" style="padding-right:25px;text-align:center;">                                                                               
                              <button type="button" id="btnNewBSUOMItemRef"  class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Update</button>                                                   
                              <button type="button" data-dismiss="modal" class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Cancel</button>
                              </div>                           
                                      </div>
                            </div>
              </div>

              <div id="ModalUpload" class="modal fade" tabindex="-1" data-width="1000" style="display:none;z-index:100007;">
                  <div class="modal-dialog">                
                                <div class="modal-content">
                                         <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#3598dc;">
                                   <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                                    <h5 class="modal-title"  id="LnkBSItemdettitle" style="color:#fff;"></h5>
                              </div>
                                          <div class="modal-body">
                                            <div class="row">
                                                <div class="col-md-12" id="dvNewUpload">
                                                   <div class="form-group">
                                                       <div style="text-align:center">
                                                          <button type="button" id="btnDownload"  class="btn purple" style="font-size:12px;padding: 6px 6px;"></button>
                                                       </div>

                                                       <div>
                                                         <span style=" margin-top: 5px; text-align:right;">Mapping File :</span>
                                                              <div style="margin-bottom:10px;">
                                                                <asp:AjaxFileUpload ID="fupFileMapp" runat="server" MaximumNumberOfFiles="1" OnUploadComplete="File_UploadComplete" 
                                                                    OnClientUploadComplete="onClientUploadComplete" />
                                                              </div>
                                                           </div>
                                                       <div style="margin-bottom:10px;"> <input type="checkbox"  id="cbdelExist" /><span>Delete Existing Records</span></div>
                                                    </div>              
                                                </div>
                                            </div>                                                   
                             </div>  
                                      </div>                       
                                <div class="modal-footer" style="padding-right:25px;">   </div>                                                
                            </div>
             </div>
               
              <div id="ModalLoginInfo" class="modal fade" tabindex="-1" data-width="1000" style="display:none;z-index:100008;">
                   <div class="modal-dialog" style="width:450px;">
                               <div class="modal-content">
                                   <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#3598dc;">
                                   <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                                    <h5 class="modal-title"  id="LnkLoginInfo" style="color:#fff;">Login Info</h5>
                              </div>

                                   <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-12" id="dvLoginInfo">                                                       
                                    </div>
                                    <div class="col-md-12" id="dvChangePWD" style="display:none;">                                                       
                                    </div>
                                </div>                                                   
                             </div>                         
                             <div class="modalFtr" id="dvLoginft" style="display:none;">  
                                 <button type="button" id="btnChangePwd"  class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Update</button>                                                   
                                <button type="button" id="btnClosePwd"  class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Cancel</button>
                              </div>   
                             </div>
                            </div>
              </div>

              <div id="ModalNewRule" class="modal fade" tabindex="-1" data-width="1000" style="display:none;z-index:100009;">
                    <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                      <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#3598dc;">
                           <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                            <h5 class="modal-title" style="color:#fff;"> List of Rules</h5>
                      </div>
                      <div class="modal-body" style="padding-bottom:1px;">
                               <div class="row">
                                    <div class="col-md-12" id="dvNewDet">
                                     <div class="table table-responsive">
                                       <table class="table table-striped table-bordered " id="dataGridNewRule">
                                        <thead id="tblHeadNewRule" class="bg-grey-silver !Important">
                                          <tr id="tblHeadRowNewRule">     </tr>         
                                         </thead>
                                       <tbody id="tblBodyNewRule" style="color: #000;"></tbody>
                                       </table>                               
                                   </div>                                 
                            </div>
                              </div>                                                   
                            </div>                         
                      <div class="modal-footer" style="text-align:center;">                                                                               
                          <button type="button" id="btnNewRule"  class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Add</button>                                                   
                         <button type="button" data-dismiss="modal" class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Cancel</button>
                      </div>                           
                    </div>
                   </div>
               </div>

              <div id="ModalDefRule" class="modal fade" tabindex="-1" style="display:none;z-index:100009;"">
              <div class="modal-dialog modal-lg">
               <div class="modal-content">
                      <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                           <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                            <h5 class="modal-title" style="color:#fff;">Add Default Rule</h5>
                      </div>
                     <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12" id="dvDefRule">
                                 <div class="table table-responsive">
                                    <table class="table table-striped table-bordered " id="dataGridNewDRule">
                                        <thead id="tblHeadNewDRule" class="bg-grey-silver !Important">
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

   </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="childScriptHolder" runat="server">   
    <link href="../assets/global/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.css" rel="stylesheet" />
    <script src="../assets/global/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js"></script>
   <link href="../assets/global/plugins/bootstrap-datepicker/css/datepicker3.css" rel="stylesheet" />
   <script src="../assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js" type="text/javascript"></script>
    <script src="../assets/ob/obSupplierDetail.js"></script>
    <script>
        jQuery(document).ready(function () {SupplierDetail.init(); $("#baseBody").fadeIn(); $('#ModalBSdet,#ModalNewBSItemRef,#ModalNewBSUOMItemRef,#ModalUpload,#ModalLoginInfo,#ModalLnkBuySupp,#ModalNewBSItemDet').draggable({ handle: ".modal-header,.modal-footer" }); });
    </script>
      <style type="text/css">
          .activechk { border-color: #238f23;   }   .chklabel {} .checkbox-grid li {display: block;float: left;width: 32%;}.activeLink {background-color:#99ff99; }
          .dropdown-menu{min-width:160px;} .leftpad{padding-left:10px;} .dropdown-menu { box-shadow: 5px 5px rgba(102,102,102,.1); left: 0;  min-width: 160px;  position: absolute; z-index: 1000; display: none; float: left;list-style: none;text-shadow: none;padding: 0;background-color: #fff3f3;margin: 10px 0 0;border: 1px solid #e46c6c; font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;  -webkit-border-radius: 4px;-moz-border-radius: 4px;-ms-border-radius: 4px;-o-border-radius: 4px;border-radius: 4px;}
          .dropdown-header, .dropdown-menu>li>a{}
          .dropdown-menu > li > a {padding:0px;color: #6f6f6f;text-decoration: none; display: block;clear: both; margin-left:30px; font-weight: 300; line-height: 18px;white-space: nowrap;}
      </style>
</asp:Content>
