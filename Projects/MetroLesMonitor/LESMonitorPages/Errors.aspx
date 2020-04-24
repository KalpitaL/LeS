<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="Errors.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.Errors" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">
     <div class="row">
       <div class="col-md-12">
            <div class="portlet light">
                     <div class="portlet light form-fit" id="divFilter">
                        <div class="portlet-title margin-bottom-0 padding-0">                          
                            <div class="row">                           
                                <div class="portlet-body form">
                                </div>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                    </div>
                <hr />
             <div id="headerTabs" class="portlet-body">                    
                 <ul id="hTabs" class="nav nav-tabs" >
                      <li  data-id="lnkErrors" class="active"><a href="javscript:void(0)"  id="0" data-toggle="tab">Errors</a></li>
                      <li data-id="lnkSolnTemp"><a href="javscript:void(0)"  id="1" data-toggle="tab">Solution Templates</a></li>                   
                 </ul>
                    <div class="portlet-body" id="portlet_bodyErr">
                         <div class="table table-responsive">
                            <table class="table  table-bordered table-striped" id="dataGridErr">
                                <thead id="tblHeadErr">
                                    <tr id="tblHeadRowErr">                                        
                                    </tr>   
                                </thead>
                                <tbody id="tblBodyErr" style="color: #000;">
                                </tbody>
                            </table>   
                            
                            </div>           
                    </div>
                     <div class="portlet-body" id="prtSolTemp" style="display:none;">
                         <div class="table table-responsive">
                            <table class="table  table-bordered table-striped" id="dataGridSolnTemp">
                                <thead id="tblHeadSolnTemp">
                                    <tr id="tblHeadRowSolnTemp">                                        
                                    </tr>   
                                </thead>
                                <tbody id="tblBodySolnTemp" style="color: #000;">
                                </tbody>
                            </table>                               
                         </div>           
                    </div>
               </div>
                                                                                                                                                                                             
           <div id="ModalErrSolution" class="modal fade" tabindex="-1" data-width="1000" style="display:none;">
               <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                          <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">                                  
                              <h5 class="modal-title" style="color:#fff;">Error Solution</h5>
                           </div>
                    <div class="modal-body">
                         <div class="row" style="padding-left:5px;">
                               <div class="col-md-12" id="dvErrSoln">                                
                                  <div class="row"><div class="col-md-12"> <div class="form-group">
                                        <div class="col-md-2" style="text-align: right;font-weight:700;">Error Audit No.</div>
                                        <div class="col-md-4">
                                            <span id="lblErrorNo"></span>
                                        </div>
                                        <div class="col-md-2" style="text-align: right;font-weight:700;">Error Sol No.</div>
                                        <div class="col-md-4">
                                            <span id="lblSolutionNo"></span>
                                        </div>
                                  </div> </div> </div>
                                 <div class="row"><div class="col-md-12"> <div class="form-group">
                                        <div class="col-md-2" style="text-align: right;font-weight:700;">Server Name</div>
                                        <div class="col-md-4">
                                            <span id="lblServerName"></span>
                                        </div>
                                        <div class="col-md-2" style="text-align: right;font-weight:700;">Processor Name</div>
                                        <div class="col-md-4">
                                            <span id="lblProcessorName"></span>
                                        </div>
                                  </div> </div> </div>
                                 <div class="row"><div class="col-md-12"> <div class="form-group">
                                        <div class="col-md-2" style="text-align: right;font-weight:700;">Module Name</div>
                                        <div class="col-md-4">
                                            <span id="lblModuleName"></span>
                                        </div>
                                        <div class="col-md-2" style="text-align: right;font-weight:700;">Doc Type</div>
                                        <div class="col-md-4">
                                            <span id="lblDocType"></span>
                                        </div>
                                  </div> </div> </div>
                                 <div class="row"><div class="col-md-12"> <div class="form-group">
                                        <div class="col-md-2" style="text-align: right;font-weight:700;">Buyer Code</div>
                                        <div class="col-md-4">
                                            <span id="lblBuyerCode"></span>
                                        </div>
                                        <div class="col-md-2" style="text-align: right;font-weight:700;">Supplier Code</div>
                                        <div class="col-md-4">
                                            <span id="lblSupplierCode"></span>
                                        </div>
                                  </div> </div> </div>
                                 <div class="row"><div class="col-md-12"><div class="form-group">
                                        <div  class="col-md-2" style="text-align: right;font-weight:700;">Ref No.</div>
                                        <div class="col-md-10">
                                            <span ID="lblKeyRef"></span>
                                        </div>
                                   </div> </div> </div>
                                   <div class="row"><div class="col-md-12"> <div class="form-group">
                                        <div class="col-md-2" style="text-align: right;font-weight:700;">Error Remarks</div>
                                        <div class="col-md-10">
                                            <span ID="lblRemarks"></span>
                                        </div>
                                   </div> </div> </div>
                                   <div class="bg-red" style="height:3px;"></div> <div style="min-height:8px;"></div>
                                  <div class="row"><div class="col-md-12"> <div class="form-group">
                                        <div  class="col-md-6" style="font-weight:700;padding-left:1px;">Error Description</div><div style="width:250px"></div>
                                        <div  class="col-md-6" style="font-weight:700;">Error Problem</div> <div style="width:250px"></div>
                                    </div> </div> </div>
                                 <div class="row"><div class="col-md-12"></div> <div class="form-group">
                                        <div class="col-md-6">
                                            <textarea ID="txtErrDescr" style="width:100%;border:1px solid #c2cad8;" rows="5" ></textarea>
                                        </div>
                                        <div class="col-md-6">
                                            <textarea ID="txtErrProblem" style="width:100%;border:1px solid #c2cad8;" rows="5"></textarea>
                                        </div>
                                    </div> </div> </div>
                                    <div class="row"><div class="col-md-12"> <div class="form-group">
                                        <div class="col-md-6" style="font-weight:700;">Error Solution</div><div style="width:250px"></div>
                                        <div  class="col-md-6" style="font-weight:700;">Error Template</div><div style="width:250px"></div>
                                   </div> </div> </div>
                                   <div class="row"><div class="col-md-12"> <div class="form-group">
                                        <div class="col-md-6">
                                            <textarea ID="txtErrSolution" style="width:100%;border:1px solid #c2cad8;" rows="5"></textarea>
                                        </div>
                                        <div class="col-md-6">
                                            <textarea ID="txtErrTemplate" style="width:100%;border:1px solid #c2cad8;" rows="5"></textarea>
                                        </div>
                                   </div> </div> </div>
                               <div class="col-md-12" id="ftrtoolbtn" style="text-align:center;">
                                  <button type="button" id="btnSaveErrSoln"  class="btn yellow-casablanca"  style="font-size:8pt;padding: 6px 6px;width:50px;">Save</button>
                                  <button type="button" data-dismiss="modal" class="btn yellow-casablanca"  style="font-size:8pt;padding: 6px 6px;">Cancel</button>        
                               </div>
                          </div>                                                   
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
   <link href="../assets/css/errors.css" rel="stylesheet" />
   <script src="../assets/Scripts/ob/Errors.js"></script>
    <script>  jQuery(document).ready(function () { Errors.init();});  </script>
</asp:Content>
