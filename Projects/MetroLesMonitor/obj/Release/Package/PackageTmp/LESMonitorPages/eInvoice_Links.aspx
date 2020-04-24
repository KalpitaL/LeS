<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="eInvoice_Links.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.eInvoice_Links" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">
  <div class="row">
        <div class="col-md-12">
            <div class="portlet light">           
                    <div class="portlet-body" id="portlet_body">
                        <div class="table table-responsive">
                            <table class="table table-striped table-bordered" id="dataGrideInvLnk">
                                <thead id="tblHeadeInvLnk" class="bg-grey-silver !Important">
                                    <tr id="tblHeadRoweInvLnk">                                      
                                    </tr>         
                                </thead>
                                <tbody id="tblBodyeInvLnk" style="color: #000;">
                                </tbody>
                            </table>   
                            
                            </div>                                           


            <div id="ModalBSInv" class="modal fade" tabindex="-1"  style="display:none;z-index:10077;">
                         <div class="modal-dialog-lg">
                             <div class="modal-content">
                               <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                               <h5 class="modal-title" id="LnkBSdettitle" style="color:#fff;"> Buyers/Suppliers-Invoice Details</h5>
                            </div>
                               <div class="modal-body">
                                   <div class="row">
                                      <div class="col-md-12" id="dvBSInv">
                                         <div class="table table-responsive">
                                             <table class="table table-striped table-bordered " id="dataGridBSInv">
                                            <thead id="tblHeadBSInv" class="bg-grey-silver !Important">
                                                <tr id="tblHeadRowBSInv">     </tr>         
                                            </thead>
                                            <tbody id="tblBodyBSInv" style="color: #000;">
                                            </tbody>
                                         </table>                               
                                         </div>                         
                                       </div>
                                   </div>                                                   
                               </div>   
                                                                
                            </div>
                         </div>
            </div>
     
             <div id="ModalBSInvAcc" class="modal fade" tabindex="-1"  style="display:none;z-index:10078;">
                <div class="modal-dialog" style="width:1250px;">
                    <div class="modal-content">
                         <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">                               
                               <h5 class="modal-title"style="color:#fff;"> Invoice Account Details</h5>
                            </div>

                         <div class="modal-body">
                           <div class="row">
                            <div class="col-md-12 col-sm-12">  <div class="portlet custom">  <div class="portlet-title margin-bottom-0">  <div id="Itemtoolbtngroup"> </div></div> </div>  </div>
                                 <div class="col-md-12" id="dvBSInvAcc">
                                         <div class="table table-responsive">
                                             <table class="table table-striped table-bordered " id="dataGridBSInvAcc">
                                            <thead id="tblHeadBSInvAcc" class="bg-grey-silver !Important">
                                                <tr id="tblHeadRowBSInvAcc">     </tr>         
                                            </thead>
                                            <tbody id="tblBodyBSInvAcc" style="color: #000;">
                                            </tbody>
                                         </table>                               
                                         </div>                         
                                       </div>
                            </div>                                                   
                          </div>                                                                   
                     </div>
               </div>
            </div>

             <div id="ModalNewInvAcc" class="modal fade" tabindex="-1"  style="display:none;z-index:10079;">
                <div class="modal-dialog" style="width:750px;">
                    <div class="modal-content">
                         <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                               <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                               <h5 class="modal-title"style="color:#fff;">Add Invoice Account</h5>
                            </div>
                         <div class="modal-body">
                           <div id="dvInvAcc" style="margin-right:30px;"></div>                                                  
                          </div>  
                        <div class="modal-footer" style="text-align:center;">                                                                               
                           <button type="button" id="btnNewInvAcc"  class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;width:50px;">Add</button>                                                   
                              <button type="button" data-dismiss="modal" class="btn yellow-casablanca" style="font-size:8pt;padding: 6px 6px;">Cancel</button>
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
    <script src="../assets/global/plugins/jquery-ui/jquery-ui.min.js"></script>         
    <script src="../assets/ob/obeInvoiceLinks.js"></script>
    <script> jQuery(document).ready(function () { eInvoiceLinks.init(); $('#ModalBSInv,#ModalBSInvAcc,#ModalNewInvAcc').draggable({ handle: ".modal-header,.modal-footer" }); }); </script>
      <style type="text/css">                
    </style>
</asp:Content>
