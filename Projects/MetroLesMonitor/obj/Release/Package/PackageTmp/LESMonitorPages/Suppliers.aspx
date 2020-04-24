<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="Suppliers.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.Suppliers" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">
      <form id="form1" runat="server">
        <div class="row">
            <div class="col-md-12">
            <div class="portlet light">
                <div class="portlet-body portlet-datatable" id="portlet_body">
                        <div class="table table-responsive">
                            <table class="table table-striped table-bordered" id="dataGridSupp">
                                <thead id="tblHeadSupp" class="bg-grey-silver !Important">
                                    <tr id="tblHeadRowSupp">                                      
                                    </tr>         
                                </thead>
                                <tbody id="tblBodySupp" style="color: #000;">
                                </tbody>
                            </table>   
                            
                            </div>                                           
                    </div>
                                                                                                                                                                                         
                <div id="ModalNew" class="modal fade" tabindex="-1" data-width="1000" style="display:none;">
                          <div class="modal-dialog modal-lg">
                              <div class="modal-content">
                                 <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                           <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                            <h5 class="modal-title" style="color:#fff;">Add New Supplier</h5>
                      </div>
                       <div class="modal-body">
                        <div class="row" style="padding-right:40px;">
                            <div class="col-md-12" id="dvNewDet">
                                                       
                            </div>
                        </div>                                                   
                     </div>                         
                     <div class="modal-footer" style="padding-right:25px;text-align:center;">                                                                               
                      <button type="button" id="btnSuppNew"  class="btn yellow-casablanca mdfootrbtn">Update</button>                                                   
                      <button type="button" data-dismiss="modal" class="btn yellow-casablanca mdfootrbtn">Cancel</button>
                      </div>                           
                              </div>
                         </div>
                </div>

    
          </div>
           </div>
       </div>
</form>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="childScriptHolder" runat="server">
    <link href="../assets/global/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.css" rel="stylesheet" />
    <script src="../assets/global/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js"></script>
    <script src="../assets/SM/LES/Suppliers.js"></script>
<%--    <script src="../assets/ob/obSuppliers.js"></script>--%>
    <script>  jQuery(document).ready(function () { Suppliers.init(); $('#ModalNew').draggable({ handle: ".modal-header,.modal-footer" }); });</script>   
</asp:Content>

