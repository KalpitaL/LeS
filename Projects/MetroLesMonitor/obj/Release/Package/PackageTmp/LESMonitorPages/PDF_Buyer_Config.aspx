<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="PDF_Buyer_Config.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.PDF_Buyer_Config" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">
      <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"  EnablePartialRendering="false" ScriptMode="Release" LoadScriptsBeforeUI="false"></asp:ToolkitScriptManager>
        <div class="row">
            <div class="col-md-12">
            <div class="portlet light">
                    <div class="portlet-body portlet-datatable" id="portlet_body">
                        <div class="table table-responsive">
                            <table class="table table-striped table-bordered" id="dataGridPDFByCfg">
                                <thead id="tblHeadPDFByCfg" class="bg-grey-silver !Important">
                                    <tr id="tblHeadRowPDFByCfg">   </tr>         
                                </thead>
                                <tbody id="tblBodyPDFByCfg" style="color: #000;">
                                </tbody>
                            </table>                               
                            </div>                                           
                    </div>
            </div>
          </div>
            </div>

  <div id="ModalUpload" class="modal fade in" tabindex="-1" data-width="1000" style="display:none;z-index:100007;">
      <div class="modal-dialog">                
          <div class="modal-content">
              <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#3598dc;">
                   <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                  <h5 class="modal-title"  style="color:#fff;">XLS Mapping File Uploader</h5>
              </div>
          
          <div class="modal-body">
            <div class="row">
              <div class="col-md-12" id="dvPDFUpload">
                  <div class="form-group">
                       <div class="form-group">
                           <label class="control-label col-md-4" style=" margin-top: 2px; text-align:right;">Mapping File :</label>
                               <div style="margin-bottom:10px;">
                                  <asp:AsyncFileUpload  ID="FileUploadMapp" runat="server" Width="300px" OnClientUploadComplete="UploadComplete" OnUploadedComplete="MappFile_UploadComplete"/>                                              
                              </div>
                       </div>
                       <div class="min-height" id="divSpacer3"> </div>         
                         <div class="form-group">
                           <label class="control-label col-md-4" style=" margin-top: 2px; text-align:right;">Reference File :</label>
                                <div style="margin-bottom:10px;">
                                    <asp:AsyncFileUpload  ID="FileUploadRef" runat="server" Width="300px" OnClientUploadComplete="UploadComplete" OnUploadedComplete="RefFile_UploadComplete"/>                                              
                                 </div>
                     </div>                      
                 </div> 
              </div>
            </div>                                                   
           </div>  
     
           <div class="row">     
             <div class="col-md-12">
                  <div class="form-group" style="text-align:center;">  
                     <button type="button" id="btnUploadFiles"  class="btn yellow-casablanca mdfootrbtn">Upload</button>                                                   
                     <button type="button" id="btnCancelUpload"  class="btn yellow-casablanca mdfootrbtn">Cancel</button>
                 </div>     
              </div>
           </div>
       </div>
    </div>
  </div>
</form>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="childScriptHolder" runat="server">  
    <script src="../assets/SM/LES/PDFBuyerConfig.js"></script>
    <script src="../assets/ob/obPDFBuyerConfig.js"></script>
    <script> jQuery(document).ready(function () { PDFBuyerConfig.init(); }); </script>
    <style type="text/css">
           #dataGridPDFByCfg{margin: 0 auto;width: 100%;clear: both;border-collapse: collapse;table-layout: fixed;}  
    </style>
</asp:Content>
