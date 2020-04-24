<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="Buyers.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.Buyers" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">
        <div class="row">
            <div class="col-md-12">
            <div class="portlet light ">                
                    <div class="portlet-body" id="portlet_body">
                        <div class="table-responsive">
                            <table class="table table-bordered" id="dataGridBuy">
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
     <link href="../assets/global/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.css" rel="stylesheet" />
    <script src="../assets/global/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js"></script>
    <script src="../assets/SM/LES/Buyers.js"></script>
   <%-- <script src="../assets/ob/obBuyer.js"></script>--%>
    <script>  jQuery(document).ready(function () { Buyers.init(); $('#ModalNew,#ModalBSlnkDet').draggable({ handle: ".modal-header,.modal-footer" }); });  </script>
      <style type="text/css">  
          hr{margin-top:1px;margin-bottom:1px;}
    </style>
</asp:Content>
