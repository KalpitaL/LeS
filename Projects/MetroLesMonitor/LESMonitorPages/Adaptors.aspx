<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="Adaptors.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.Adaptors" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ChildGridContent" runat="server">
<%--  <div class="container-fluid">--%>
    <div class="row">
        <div class="col-md-12"> 
         <div class="portlet box blue">
                <div class="portlet-title">
                    <div class="caption">Adaptor</div>
                    <div class="tools">
                       <a href="javascript:;" id="btnservstatus" ></a>
                        <span id="servStatus"  style="margin-bottom:5px;"> </span>
                    </div>
                       </div>
                <div class="portlet-body" id="portlet_bodyadp">     
                    <div class="table-responsive">
                        <table class="table table-bordered table-striped" id="dataGridAdpt">
                                <thead id="tblHeadAdpt">
                                    <tr id="tblHeadRowAdpt">  </tr>
                                </thead>
                                <tbody id="tblBodyAdpt" style="color: #000;">
                                </tbody>
                            </table> 
                    </div>
               </div>                   
         </div>     
       
         <div class="portlet box blue" id="prtServices">
                <div class="portlet-title">
                    <div class="caption">Services</div>
                    <div class="tools">
                        <a href="javascript:;" class="expand"></a>
                    </div>
                       </div>
                    <div class="portlet-body portlet-collapsed"  id="portlet_bodysev">
                      <div class="table-responsive">
                            <table class="table table-bordered table-striped" id="dataGridServ">
                                <thead id="tblHeadServ">
                                    <tr id="tblHeadRowServ">                                     
                                    </tr>
                                </thead>
                                <tbody id="tblBodyServ" style="color: #000;">
                                </tbody>
                            </table>  
                        </div>  
                    </div>
                </div>
      
         <div class="portlet box blue"  id="prtScheduler">
		        	<div class="portlet-title">
                <div class="caption">Scheduler</div>
                          <div class="tools">                            
                        <a href="javascript:;" class="expand"></a>
                    </div>
                   </div>
                        <div class="portlet-body portlet-collapsed"  id="portlet_bodysch">
                             <div class="table-responsive">
                               <table class="table table-bordered table-striped" id="dataGridSch"><%----%>
                        <thead id="tblHeadScf">
                            <tr id="tblHeadRowSch">                              
                            </tr>
                        </thead>
                        <tbody id="tblBodySch">
                        </tbody>
                    </table>
                            </div>
                           </div>
                    </div>   
       </div>           
     </div>
<%--</div>--%>

     <div id="ModalLicense" class="modal fade" tabindex="-1" style="display:none;">
     <div class="modal-dialog">
       <div class="modal-content">
          <div class="modal-header" style="padding:5px;background-color:#60aee4;">             
              <h5 class="modal-title" style="color:#fff;"></h5>
           </div>
          <div class="modal-body">               
            <div class="row">
                <div class="col-md-12" id="dvlicensedet"> 
                  <div class="row"><div class="col-md-12"><div class="form-group">
                    <div class="col-md-3" style="text-align:right;margin-top:5px;"><label class="dvLabel"> License Key : </label> </div>
                     <div  class="col-md-9"><input type="text" class="form-control" id="txtLicensekey" disabled value=""/></div>
                   </div></div></div>  
                 </div>
              </div>                                                   
          </div>                         
          <div class="modal-footer" style="text-align:center;">                                                                                                                               
             <button type="button" id="btnSave" class="btn yellow-casablanca mdfootrbtn">Save</button>
             <button type="button" data-dismiss="modal" class="btn yellow-casablanca mdfootrbtn">Close</button>
           </div>                           
        </div>
     </div>
 </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="childScriptHolder" runat="server">
    <link href="../assets/css/adaptor.css" rel="stylesheet" />
    <script src="../assets/Scripts/ob/Adaptors.js"></script>
    <script>  jQuery(document).ready(function () {  Adaptors.init();  }); </script>
</asp:Content>








