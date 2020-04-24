<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="PDF_Buyer_Config.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.PDF_Buyer_Config" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">
      <form id="form1" runat="server">
   <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="row">
            <div class="col-md-12">
            <div class="portlet light">
                    <div class="portlet-body portlet-datatable" id="portlet_body">
                        <div class="table table-responsive">
                            <table  class="table table-bordered table-striped" id="dataGridPDFByCfg">
                                <thead id="tblHeadPDFByCfg">
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
                                  <ajaxToolkit:AsyncFileUpload  ID="FileUploadMapp" runat="server" Width="300px" OnClientUploadComplete="UploadComplete" OnUploadedComplete="MappFile_UploadComplete"/>                                              
                              </div>
                       </div>
                       <div class="min-height" id="divSpacer3"> </div>         
                         <div class="form-group">
                           <label class="control-label col-md-4" style=" margin-top: 2px; text-align:right;">Reference File :</label>
                                <div style="margin-bottom:10px;">
                                    <ajaxToolkit:AsyncFileUpload  ID="FileUploadRef" runat="server" Width="300px" OnClientUploadComplete="UploadComplete" OnUploadedComplete="RefFile_UploadComplete"/>                                              
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
    <script src="../assets/Scripts/ob/PDFBuyerConfig.js"></script>   
    <script> jQuery(document).ready(function () { PDFBuyerConfig.init(); }); </script>
</asp:Content>
