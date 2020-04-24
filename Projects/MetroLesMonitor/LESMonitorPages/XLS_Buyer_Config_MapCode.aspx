<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="XLS_Buyer_Config_MapCode.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.XLS_Buyer_Config_MapCode" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">
         <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release"></asp:ScriptManager>
        <div class="row">
            <div class="col-md-12">
            <div class="portlet light">
               <div class="portlet-body portlet-datatable" id="portlet_body">
                        <div class="table table-responsive">
                            <table class="table" id="dataGridXLSByCfg">
                                <thead id="tblHeadXLSByCfg">
                                    <tr id="tblHeadRowXLSByCfg"></tr>         
                                </thead>
                                <tbody id="tblBodyXLSByCfg" style="color: #000;">
                                </tbody>
                            </table>                               
                            </div>                                           
                    </div>
            </div>
          </div>
            </div>

   <div id="ModalCopyFormat" class="modal fade" tabindex="-1" style="display:none;z-index:100080;" data-width="400">
    <div class="modal-dialog">
            <div class="modal-content">
               <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                             <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                             <h5 class="modal-title" style="color:#fff;">Copy Format</h5>
                           </div>
               <div class="modal-body">  <div class="row" style="padding-right:40px;">  <div class="col-md-12" id="dvCpyFrmtDet">  
                    <div class="form-body" style="padding:0px!important;"> 
                       <div class="form-group">
                               <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> Buyers </label> </div>
                               <div  class="col-md-9"><select class="form-control" id="cbBuyers"></select> </div>                                                                                                                                                                       
                       </div>                                                                            
                   </div>           

                 </div> </div>  </div>                                                          
               <div class="modal-footer" style="text-align:center;">                                                                               
                    <button type="button" id="btnCopyFormat"  class="btn yellow-casablanca mdfootrbtn">Copy</button>                                                   
                    <button type="button" data-dismiss="modal" class="btn yellow-casablanca mdfootrbtn">Cancel</button>
                </div>                           
             </div>
    </div>
  </div>

  <div id="ModalUploadFormat" class="modal fade" tabindex="-1" style="display:none;z-index:100080;" data-width="400">
    <div class="modal-dialog">
            <div class="modal-content">
               <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                             <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                             <h5 class="modal-title" style="color:#fff;">Upload Format</h5>
                           </div>
               <div class="modal-body">  <div class="row" style="padding-right:40px;">  <div class="col-md-12" id="dvUplFrmtDet">                  
                   <div class="form-group">
                        <label class="control-label col-md-3" style=" margin-top: 5px; text-align:right;">Mapping File :</label>
                           <div class="col-md-9" style="padding-left:0px!important;">
                                 <ajaxToolkit:AjaxFileUpload ID="AjaxFileUpload1" runat="server" MaximumNumberOfFiles="1" OnUploadComplete="File_UploadComplete" 
                                     OnClientUploadComplete="onClientUploadComplete" />
                           </div>
                 </div> </div>  </div>                                                                                          
             </div>
                <div class="modal-footer" style="text-align:center;">                                                                                                                                
                   <button type="button" data-dismiss="modal" class="btn yellow-casablanca mdfootrbtn">Close</button>
                </div>       
    </div>
  </div>
</div>

</form>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="childScriptHolder" runat="server">
    <script src="../assets/Scripts/min/jquery-ui.min.js"></script>  
    <script src="../assets/Scripts/ob/obXlsBuyerConfigMapCode.js"></script>
    <script>jQuery(document).ready(function () { XlsBuyerConfig_MapCode.init(); });</script>      
</asp:Content>
