<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="File_Mapping.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.File_Mapping" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">   
    <form id="form1" runat="server" enctype="multipart/form-data" method="post" action="#">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" ScriptMode="Release"></asp:ToolkitScriptManager>
           <div class="row">
            <div class="col-md-12">
                <div class="portlet light">
                   <div class="portlet-body form" style="font-size: 12px;height:500px;" id="portlet_body">
                      <div class="form-body">
                        <div class="form-group">
                            <label class="control-label col-md-4" style=" margin-top: 2px; text-align:right;">File Type :</label>
                                <div style="margin-bottom:10px;">
                                             <select class="select2-container" id="cbFileType" style="width:300px;"><%--form-control DetailCombo--%>
                                                 <option value="0"></option>   
                                                 <option value="1">PDF</option>   
                                                 <option value="2">XLS</option>                                                                                                 
                                                 <option value="3">VOUCHER_PDF</option>     
                                             </select>
                                          </div>
                         </div>
                          
                       <div class="min-height" id="divSpacer1"> </div>                              
                         <div class="form-group">
                              <label class="control-label col-md-4" style=" margin-top: 10px; text-align:right;">Group :</label>
                                 <div style ="margin-bottom:10px;">
                                      <select class="select2-container" id="cbGroup" style="width:300px;"> </select>                                                                                                                        
                                      <span> <a href="javascript:;" id="btnDownload" data-toggle="tooltip" title="Download" class="btn-circle_disabled"><i class="fa fa-download"></i></a></span>                                        
                                </div>                                                               
                          </div> 
                             <div class="min-height" id="divSpacer2"> </div>         
                             <div class="form-group">
                                      <label class="control-label col-md-4" style=" margin-top: 2px; text-align:right;">Mapping File :</label>
                                         <div style="margin-bottom:10px;" id="FUploadMpp'">
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
                                 <div class="min-height" id="divSpacer4"> </div>                                  
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
           </div>
             <div id="ModalDownload" class="modal fade in" tabindex="-1" aria-hidden="true"  style="display:none;">
                 <div class="modal-dialog">
                   <div class="modal-content">
                       <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                           <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                           <h5 class="modal-title" style="color:#fff;">Download</h5>
                       </div>
                       <div class="modal-body">     
                          <h5>Please Confirm, do you want to download Reference file also?</h5>
                              <div class="portlet light bordered">
                                    <div class="portlet-body" id="prtDownload"> </div>
                               </div>
                        </div>                         
                        <div class="modal-footer" style="padding-right:25px;text-align:center;margin-right:0px;">                                                                           
                      <button type="button" id="btnDownloadFileTemp"  class="btn yellow-casablanca mdfootrbtn">Download</button>                                                                         
                      </div>        
                   </div>
                 </div>
            </div>                                                                                                  
   </form>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="childScriptHolder" runat="server">
    <script src="../assets/ob/obFileMapping.js"></script>
      <script> jQuery(document).ready(function () { FileMapping.init();});</script>
      <style type="text/css"> 
          .fileupld-minhgt{height:50px; }             
          .filename-width{word-break:break-all; word-wrap:break-word;}          
    </style>
</asp:Content>
