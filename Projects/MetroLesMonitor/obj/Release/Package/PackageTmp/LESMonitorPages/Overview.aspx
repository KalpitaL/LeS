<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="Overview.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.Overview" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">
 <div class="row">
    <div class="col-md-12">
       <div class="portlet light portlet-fit portlet-datatable"> 
          <div class="row">
               <div class="col-md-6">
                 <div class="form-group">
                    <label class="control-label col-md-3" style=" margin-top: 2px; text-align:right;">Duration :</label>
                        <div style="margin-bottom:10px;">
                             <select class="select2-container" id="cbDuration" style="width:180px;">
                               <option value=""></option>  <option value="0" selected>This Week</option>   
                               <option value="1">Last Week</option> <option value="2">This Month</option> <option value="3">Last Month</option>  <option value="4">This Year</option>  <option value="5">All</option>
                             </select>
                         </div>
                  </div>
               </div>                                                      
           </div>     
          <div id="headerTabs" class="portlet-body">
                   <ul id="hTabs" class="nav nav-tabs" >
                      <li id="lnkBuyer" class="active"><a href="javscript:void(0)"  id="0" data-toggle="tab">Buyer</a></li>
                      <li id="lnkSupplier"><a href="javscript:void(0)"  id="1" data-toggle="tab">Supplier</a></li>                                          
                    </ul>
               
          <div id="prtByDet" style="display:none;">
            <div class="portlet light">
               <div class="portlet-body">                                                               
                   <div class="table table-responsive">
                        <table class="table table-striped table-bordered" id="dataGridBuyer">  
                             <thead id="tblHeadBuyer" class="bg-blue-silver !Important">   <tr id="tblHeadRowBuyer">  </tr>   </thead>
                             <tbody id="tblBodyBuyer" style="color: #000;">  </tbody>
                         </table> 
                    </div>                                     
                </div>     
             </div>
          </div>
        <div id="prtSppDet" style="display:none;">
          <div class="portlet light">
            <div class="portlet-body">  
                 <div class="table table-responsive">                                                                        
                     <table class="table table-striped table-bordered" id="dataGridSupp">
                       <thead id="tblHeadSupp" class="bg-blue-silver !Important"> <tr id="tblHeadRowSupp"> </tr>   </thead>
                       <tbody id="tblBodySupp" style="color: #000;">   </tbody>
                      </table> 
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
    <script src="../assets/SM/LES/Overview.js"></script>
<%--    <script src="../assets/ob/obOverview.js"></script>--%>
 <script> jQuery(document).ready(function (){ Overview.init(); }); </script>     
</asp:Content>
