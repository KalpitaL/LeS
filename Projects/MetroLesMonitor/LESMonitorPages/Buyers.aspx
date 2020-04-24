<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="Buyers.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.Buyers" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">
        <div class="row">
            <div class="col-md-12">
            <div class="portlet light ">                
                    <div class="portlet-body" id="portlet_body">
                        <div class="table-responsive">
                            <table class="table table-bordered table-striped" id="dataGridBuy">
                                <thead id="tblHeadBuy">
                                    <tr id="tblHeadRowBuy"> </tr>         
                                </thead>
                                <tbody id="tblBodyBuy" style="color: #000;">
                                </tbody>
                            </table>                               
                            </div>  
                    
                        <div id="ModalNew" class="modal fade" tabindex="-1" style="display:none;">
                          <div class="modal-dialog modal-lg">
                              <div class="modal-content">
                                 <div class="modal-header ui-draggable-handle" style="padding:5px;background-color:#60aee4;">
                           <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                            <h5 class="modal-title" style="color:#fff;">Add New Buyer</h5>
                      </div>
                        <div class="modal-body">
                        <div class="row" style="padding-right:40px;">
                            <div class="col-md-12" id="dvNewDet">    </div>
                        </div>                                                   
                     </div>                                
                        <div class="modal-footer" style="text-align:center;">                                                                               
                      <button type="button" id="btnBuyNew"  class="btn yellow-casablanca mdfootrbtn">Update</button>                                                   
                      <button type="button" data-dismiss="modal" class="btn yellow-casablanca mdfootrbtn">Cancel</button>
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
    <link href="../assets/css/commonstyle.css" rel="stylesheet" />
    <script src="../assets/Scripts/ob/Buyers.js"></script>  
    <script>  jQuery(document).ready(function () { Buyers.init();});  </script>
</asp:Content>
<%-- $('#ModalNew,#ModalBSlnkDet').draggable({ handle: ".modal-header,.modal-footer" });--%>