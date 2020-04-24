<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="File_Reprocess.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.File_Reprocess" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">
  <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
     <div class="row">
         <div class="col-md-12">
           <div class="portlet light">               
               <div class="portlet-body " style="font-size: 12px;" id="portlet_body">
                   <div class="row">  
                        <div class="col-md-12">
                                <div class="form-group">
                                     <label class="control-label col-md-4" style=" margin-top: 3px; text-align:right;">File Action Type :</label>
                                          <div style="margin-bottom:10px;">
                                             <select class="select2-container" id="cbFileActionType" style="width:290px;">
                                                 <option value="0"></option>   
                                                 <option value="1">Import Buyer File</option>   
                                                 <option value="2">Import Supplier File</option>   
                                                 <option value="3">Import MTML File</option>   
                                                 <option value="4">Import LeSXml File</option>   
                                                 <option value="5">Export</option>                                                                                                 
                                             </select>
                                          </div>
                                </div>
                             </div>      
                        <div class="min-height" id="divSpacer1"> </div>                                                                              
                         <div class="col-md-12" id="dvSupplier" style="display:none;">
                                <div class="form-group">
                                     <label class="control-label col-md-4" style=" margin-top: 3px; text-align:right;">Select Supplier :</label>
                                          <div style ="margin-bottom:10px;">
                                          <input class="form-control-inline" type="text" id="txtSupplier" style="width:290px;border:1px solid #c2cad8;" disabled/>                                                                                                                   
                                       <span><a href="javascript:;" id="btnSupplier" data-toggle="tooltip" title="Supplier" class="btn btn-circle btn-icon-only"><i class="fa fa-search"></i></a></span>
                                               </div>                                
                                </div>
                             </div> 
                       <div class="min-height" id="divSpacer2"> </div>  
                         <div class="col-md-12" id="dvBuyer" style="display:none;">
                                <div class="form-group">
                                     <label class="control-label col-md-4" style=" margin-top: 3px; text-align:right;">Select Buyer :</label>
                                             <div style ="margin-bottom:10px;">
                                            <input class="form-control-inline"  type="text" id="txtBuyer" style="width:290px;border:1px solid #c2cad8;" disabled/>                                                                                                                              
                                      <span> <a href="javascript:;" id="btnBuyer" data-toggle="tooltip" title="Buyer" class="btn btn-circle btn-icon-only"><i class="fa fa-search"></i></a></span>    
                                               </div>      
                                </div>
                             </div>                        
                        </div>        
                   <div style="min-height:10px;"></div>
                   <div id="importbody">
                      <div class="row">
                         <div class="col-md-12">
                            <div class="portlet box blue">
                               <div class="portlet-title">
                                       <div class="caption">Import List</div>
                                    </div>

                               <div class="portlet-body form"  id="portlet_bodyimp" style="height:200px;"> 
                                        <div class="form-body"> 
                                              <div class="form-group">
                                                <div class="row"> 
                                                 <div class="col-md-12">
                                                     <div id="dvmtml"> 
                                                         <div class="col-md-2"></div>
                                                          <div class="col-md-3"><span style="font-weight:600;"> MTML File (Internal MTML File)  </span> </div>
                                                         <div class="col-md-4"><span id="spnmtml" class="word-wrap"></span> </div>
                                                      </div>
                                                        <div id="dvlesxml"> 
                                                          <div class="col-md-2"></div>
                                                          <div class="col-md-3"><span style="font-weight:600;"> LeSXml File (Internal LesXml File)</span></div>
                                                          <div class="col-md-4"><span id="spnlesxml" class="word-wrap"></span></div>
                                                        </div>         
                                                        <div id="dvByFile"> 
                                                          <div class="col-md-2"></div>
                                                          <div class="col-md-3"> <span style="font-weight:600;">Buyer File (RFQ/PO)</span></div>
                                                          <div class="col-md-4" ><span id="spnByFile" class="word-wrap"></span></div>
                                                        </div>
                                                        <div id="dvSuppfile"> 
                                                          <div class="col-md-2"></div>
                                                          <div class="col-md-3"><span style="font-weight:600;">Supplier File (Quote/POC)</span></div>
                                                          <div class="col-md-4"><span id="spnSuppfile" class="word-wrap"></span></div>
                                                        </div> 
                                                      </div>
                                                   </div>
                                                  </div>
                                               
                                                  <div class="row" style="padding-top:15px;"> 
                                                  <div class="col-md-12">
                                                      <div class="col-md-2"></div>
                                                      <div class="col-md-3"><span style="font-weight:600;">Mapping File </span></div>
                                                      <div class="col-md-5">
                                                         <div style="margin-bottom:15px;margin-right:50px;">
                                                      <asp:AjaxFileUpload ID="AjaxFileUpload1" runat="server" MaximumNumberOfFiles="1" Width="100%" OnUploadComplete="File_UploadComplete" 
                                                     OnClientUploadComplete="onClientUploadComplete" OnClientUploadStart="onClientUploadStart"/>
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
                </div>
           </div>
        </div> 
                 
         <div id="exportbody">  
                           <div class="row">                         
                              <div class="col-md-12">
                                   <div class="portlet box blue">
                                        <div class="portlet-title">
                                           <div class="caption">Export List</div>
                                        </div>
                                          <div class="portlet light form-fit" id="divFilter">
                                             <div class="portlet-title margin-bottom-0 padding-0">                          
                                               <div class="row">                           
                                                 <div class="portlet-body form"></div>
                                                 <div class="clearfix"></div>
                                               </div>
                                             </div>
                                          </div>
                                         <hr />
                                      <div class="portlet-body" id="portlet_bodyexp">     
                                            <div class="table table-responsive">
                                          <table class="table table-striped table-bordered" id="dataGridExp">
                                              <thead id="tblHeadExp" class="bg-grey-silver !Important">
                                                 <tr id="tblHeadRowExp">    </tr>  
                                             </thead>
                                          <tbody id="tblBodyExp" style="color: #000;"> </tbody>
                                       </table> 
                                            </div>                 
                                      </div>                                         
                                    </div>
                                  </div>
                          </div>
                        </div>                                                 
     
         <div id="ModalSupplier" class="modal fade" tabindex="-1" aria-hidden="true" style="display:none;">
                      <div class="modal-dialog">
                          <div class="modal-content">
                              <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                               <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                                   <h5 class="modal-title" style="color:#fff;">Supplier Details</h5>
                              </div>
                              <div class="modal-body">  
                                  <div class="portlet-body"  id="portlet_body1">
                                    <div class="row">
                                      <div class="col-md-12" id="dvSuppDet">
                                          <div class="table-responsive">
                                        <table class="table table-striped table-bordered" id="dataGridSupp">
                                            <thead id="tblHeadSupp" class="bg-grey-silver !Important">
                                                <tr id="tblHeadRowSupp">
                                                    <th class="table-checkbox wide25">
                                                        <input type="checkbox" class="group-checkable" data-set="#dataGridSupp .checkboxes" id="cbSuppheader" />
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody id="tblBodySupp" style="color: #000;">
                                            </tbody>
                                        </table>  
                                     </div>  
                                       </div>
                                    </div>   
                                  </div>                                      
                               </div>          
                 
                              <div class="modal-footer" style="text-align:center;">                                                   
                                  <button type="button" class="btn yellow-casablanca" id="btnSpSelect" style="font-size:8pt;padding: 6px 6px;">Add Supplier</button>
                                  <button type="button" data-dismiss="modal" class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Cancel</button>
                               </div>
                         </div>
                     </div>
                   </div>

         <div id="ModalBuyer" class="modal fade bs-modal-lg" tabindex="-1" aria-hidden="true"  style="display:none;">
                      <div class="modal-dialog modal-lg">
                          <div class="modal-content">
                              <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                               <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                                   <h5 class="modal-title" style="color:#fff;">Buyer Details</h5>
                              </div>
                              <div class="modal-body">        
                                  <div class="row">
                                      <div class="col-md-12" id="dvNuyDet">            
                                            <div class="table-responsive">
                                        <table class="table table-striped table-bordered" id="dataGridByer">
                                            <thead id="tblHeadByer" class="bg-grey-silver !Important">
                                                <tr id="tblHeadRowByer">
                                                    <th class="table-checkbox wide25">
                                                        <input type="checkbox" class="group-checkable" data-set="#dataGridByer .checkboxes" id="cbByerheader" />
                                                    </th>
                                                </tr>         
                                            </thead>
                                            <tbody id="tblBodyByer" style="color: #000;">
                                            </tbody>
                                        </table>   
                                        </div>
                                     </div>
                                   </div>
                               </div>                                     
                              <div class="modal-footer"  style="text-align:center;">                                                   
                                  <button type="button" class="btn yellow-casablanca" id="btnBySelect" style="font-size:8pt;padding: 6px 6px;">Add Buyer</button>
                                  <button type="button" data-dismiss="modal" class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Cancel</button>
                               </div>
                              </div>
                         </div>
                     </div>

        </div>                      
 </form>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="childScriptHolder" runat="server">
    <script src="../assets/global/plugins/jquery-ui/jquery-ui.min.js"></script>  
   <link href="../assets/global/plugins/bootstrap-datepicker/css/datepicker3.css" rel="stylesheet" />
   <script src="../assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js" type="text/javascript"></script>
    <script src="../assets/ob/obFileReprocess.js"></script>
  <script>   jQuery(document).ready(function () { FileReprocess.init(); $('#ModalSupplier,#ModalBuyer').draggable({ handle: ".modal-header,.modal-footer" }); });</script>
</asp:Content>
