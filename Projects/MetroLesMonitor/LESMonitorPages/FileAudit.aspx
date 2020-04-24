<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="FileAudit.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.FileAudit" %>
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
                    <div class="portlet-body" style="font-size: 12px;" id="portlet_body">
                        <div class="row">
                         <div class="col-md-12">                                 
                               <div class="table-responsive ">
                                    <table class="table  table-hover" id="dataGridFAud">
                                <thead id="tblHeadFAud" class="bg-blue-silver !Important">
                                    <tr id="tblHeadRowFAud"></tr>         
                                </thead>
                                <tbody id="tblBodyFAud" style="color: #000;">
                                </tbody>
                            </table>  
                              </div>                                                                                   
                        </div>                                
                      </div>
                 </div>
            </div>     
           <div id="ModalDetails" class="modal fade in" data-backdrop="static" tabindex="-1"  style="display:none;">
          <div class="modal-dialog">
              <div class="modal-content">
                  <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#808080;">
                       <h5 class="modal-title" style="color:#FAFCFB;" id="captionid">Details</h5>
                  </div>
                  <div class="modal-body">  
                     <div class="row">
                         <div class="col-md-12">
                              <div class="portlet-body"  id="portlet_bodydet">
                                  <div class="row">
                                       <div class="col-md-6" style="padding-left:5px!important">
                                          <span class="caption-subject font-purple bold" style="padding-left:25px;"><i class="fa fa-angle-double-right"></i><u> RFQ Process</u></span>
                                           <table class="table borderless table-advance table-hover" id="tblRFQ">
                                                       <tbody>
                                                            <tr>
                                                                <td class="td-align-right">  <span>Received From Buyer :- </span></td>
                                                                <td><span id="RFQrcvdByer"></span> </td>                                                   
                                                            </tr>
                                                             <tr>
                                                                <td class="td-align-right"> <span>RFQ Imported :- </span></td>
                                                                <td><span id="RFQimp"></span> </td>                                                   
                                                            </tr>
                                                             <tr>
                                                                <td class="td-align-right"> <span>RFQ Exorted :- </span></td>
                                                                <td><span id="RFQexp"></span> </td>                                                   
                                                            </tr>
                                                             <tr>
                                                                <td class="td-align-right"> <span>Sent to Supplier :- </span></td>
                                                                <td ><span id="RFQsentsupp"></span> </td>                                                   
                                                            </tr>
                                                             <tr>
                                                                <td class="td-align-right"> <span>Mail Sent :- </span></td>
                                                                <td><span id="RFQmailsent"></span> </td>                                                   
                                                            </tr>
                                                           </tbody>
                                                     </table>                                   
                                       </div>
                                       <div class="col-md-6" style="padding-left:5px!important">
                                           <span class="caption-subject font-purple bold" style="padding-left:25px;"><i class="fa fa-angle-double-right"></i><u> Quote Process </u></span>
                                              <table class="table borderless table-advance table-hover">
                                                            <tbody>
                                                            <tr>
                                                                <td class="td-align-right"> <span>Received from Supplier :- </span></td>
                                                                <td><span id="QuotercvdSupp"></span> </td>                                                   
                                                            </tr>
                                                             <tr>
                                                                <td class="td-align-right"> <span>Quote Imported :- </span></td>
                                                                <td><span id="Quoteimp"></span> </td>                                                   
                                                            </tr>
                                                             <tr>
                                                                <td class="td-align-right"> <span>Quote Exported :- </span></td>
                                                                <td><span id="Quoteexp"></span> </td>                                                   
                                                            </tr>
                                                             <tr>
                                                                <td class="td-align-right"> <span>Sent to Buyer :- </span></td>
                                                                <td><span id="QuotesentByer"></span> </td>                                                   
                                                            </tr>
                                                             <tr>
                                                                <td class="td-align-right"> <span>Mail Sent :- </span></td>
                                                                <td><span id="Quotemailsent"></span> </td>                                                   
                                                            </tr>
                                                           </tbody>
                                                        </table>
                                       </div>                                   
                                 </div>      
                      <div class="bg-red" style="height:3px;"></div>                            
                      <div style="min-height:8px;"></div>
                          <div class="row">
                            <div class="col-md-6" style="padding-left:5px!important">
                                <span class="caption-subject font-purple bold" style="padding-left:25px;"><i class="fa fa-angle-double-right"></i><u> PO Process</u></span>
                                     <table class="table borderless table-advance table-hover">
                                                <tbody>
                                                <tr>
                                                    <td class="td-align-right"> <span>Received from Buyer :- </span></td>
                                                    <td><span id="POrcvdByer"></span> </td>                                                   
                                                </tr>
                                                 <tr>
                                                    <td class="td-align-right"> <span>PO Imported :- </span></td>
                                                    <td><span id="POimp"></span> </td>                                                   
                                                </tr>
                                                 <tr>
                                                    <td class="td-align-right"> <span>PO Exported :- </span></td>
                                                    <td><span id="POexp"></span> </td>                                                   
                                                </tr>
                                                 <tr>
                                                    <td class="td-align-right"> <span>Sent to Supplier :- </span></td>
                                                    <td><span id="POsentsupp"></span> </td>                                                   
                                                </tr>
                                                 <tr>
                                                    <td class="td-align-right"> <span>Mail Sent :- </span></td>
                                                    <td><span id="POmailsent"></span> </td>                                                   
                                                </tr>
                                               </tbody>
                                            </table>
                            </div>
                               
                            <div class="col-md-6" style="padding-left:5px!important">
                                  <span class="caption-subject font-purple bold" style="padding-left:25px;"><i class="fa fa-angle-double-right"></i><u> POC Process</u></span>
                                     <table class="table borderless table-advance table-hover">
                                                <tbody>
                                                <tr>
                                                    <td class="td-align-right"> <span>Received from Supplier :- </span></td>
                                                    <td><span id="POCrcvdSupp"></span> </td>                                                   
                                                </tr>
                                                 <tr>
                                                    <td class="td-align-right"> <span>POC Imported :- </span></td>
                                                    <td><span id="POCimp"></span> </td>                                                   
                                                </tr>
                                                 <tr>
                                                    <td class="td-align-right"> <span>POC Exported :- </span></td>
                                                    <td><span id="POCexp"></span> </td>                                                   
                                                </tr>
                                                 <tr>
                                                    <td class="td-align-right"> <span>Sent to Buyer :- </span></td>
                                                    <td><span id="POCsentByer"></span> </td>                                                   
                                                </tr>
                                                 <tr>
                                                    <td class="td-align-right"> <span>Mail Sent :- </span></td>
                                                    <td><span id="POCmailsent"></span> </td>                                                   
                                                </tr>
                                               </tbody>
                                            </table>
                              </div>                                 
                         </div>                           
                     </div>        
                         </div>
                     </div>
                   </div>                      
                     <div class="modal-footer" style="text-align:center;">                                                   
                      <button type="button" data-dismiss="modal" class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Close</button>
                   </div>      
             </div>
         </div>
       </div>
            </div>  
        </div>
    <%--</div>--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="childScriptHolder" runat="server">
   <link href="../assets/css/filtertable.css" rel="stylesheet" />
    <link href="../assets/css/fileaudit.css" rel="stylesheet" />
    <script src="../assets/Scripts/ob/FileAudit.js"></script> 
   <script> jQuery(document).ready(function () { FileAudit.init();}); </script>
</asp:Content>

 <%--$('#ModalDetails').draggable({ handle: ".modal-header,.modal-footer" }); --%>
