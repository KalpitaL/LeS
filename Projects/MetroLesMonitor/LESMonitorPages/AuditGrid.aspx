<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditGrid.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.AuditGrid" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Audit Log</title>
    <style type="text/css">
        .auditbtns{background-color:#006eb7 !Important;color:#fff}
        .paddspaces{padding-bottom:5px;}
    </style>
      <link href="../assets/css/bootstrap.min.css" rel="stylesheet" />
      <link href="../assets/css/datepicker3.css" rel="stylesheet" />
      <link href="../assets/css/bootstrap-timepicker.min.css" rel="stylesheet" />
      <link href="../assets/css/components.min.css" rel="stylesheet" />     
      <link href="../assets/css/smCommon.css" rel="stylesheet" />
      <link href="../assets/css/commonstyle.css" rel="stylesheet" />
      <script src="../assets/Scripts/min/jquery.min.js" type="text/javascript"></script>    
      <script src="../assets/Scripts/min/jquery-migrate.min.js" type="text/javascript"></script>       
      <script src="../assets/Scripts/min/bootstrap.min.js"></script>               
      <script src="../assets/Scripts/min/bootstrap-datepicker.min.js" type="text/javascript"></script>
      <script src="../assets/Scripts/min/toastr.js" type="text/javascript"></script>            
      <script src="../assets/Scripts/ob/smCommon.js"></script>    
    <script src="../assets/Scripts/AuditGrid.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {AuditGrid.init(); });
    </script>
</head>
<body>
    <form id="form1" runat="server">
      <div class="row">
        <div class="col-md-12">
          <div class="portlet light portlet-fit portlet-datatable">  
              
                <div class="portlet-title" style="margin-bottom:0px!important;border-top:1px solid #eee;min-height:0px!important;">
              <div class="caption font-red-sunglo">
                    <i class="fa fa-hand-o-right"></i><span class="caption-subject bold"> Audit Log</span>                                       
             </div>
                <div id="Itemtoolbtngroup" style="text-align:right;">
                   <span title="Close" data-toggle="tooltip" data-placement="top">                       
                        <button id="divbtnClose" class="toolbtn"  runat="server"><i class="fa fa-times" style="text-align:center;"></i></button>
                    </span>                          
                  <span title="Refresh" data-toggle="tooltip" data-placement="top">
                      <button id="btnItmRefresh" class="toolbtn"  runat="server" onserverclick="txtHeaderChanged"  onclick="GetValues();"><i class="fa fa-recycle" style="text-align:center;"></i></button>
                  </span>
                </div>
                
            <div class="portlet-body">                         
               <div class="table-responsive">
                        <strong>  <asp:Literal ID="Literal1" runat="server"></asp:Literal> record(s) found </strong>
                   <input id="Hidden1" type="hidden" runat="server" />
                <div class="row">
                    <div class="col-md-12">
                       <asp:GridView ID="gvData" runat="server" AllowPaging="True" PageSize="20" AutoGenerateColumns = "False" DataKeyNames="LOGID" 
          OnPageIndexChanging="gvData_PageIndexChanging"  OnRowDataBound="gvData_RowDataBound" ShowHeaderWhenEmpty="True" OnRowCommand="gvData_RowCommand">
        <Columns>        
          <asp:TemplateField> 
              <HeaderTemplate>
                    <span class="HeadercenterAlign">Log Date</span> <br /> 
                    <input type="text" id="txtLogDate" value=""></input>               
                    </HeaderTemplate>
              <ItemTemplate>   <asp:Label ID="lblUpdateDate" runat="server" Text=' <%# DataBinder.Eval(Container.DataItem, "UPDATEDATE")%>'></asp:Label>  </ItemTemplate>
              </asp:TemplateField>

         <asp:TemplateField ItemStyle-Wrap="true"> 
             <HeaderTemplate>
                     <span class="HeadercenterAlign">Server Name</span> <br />
                        <asp:TextBox ID="txtServerName" runat="server" Width="100px" AutoPostBack="true"  OnTextChanged="txtHeaderChanged"></asp:TextBox>
                    </HeaderTemplate>
             <ItemTemplate>   <asp:Label ID="lblServerName" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem,"ServerName") %>'></asp:Label>  </ItemTemplate>

<ItemStyle Wrap="True"></ItemStyle>
          </asp:TemplateField>

          <asp:TemplateField ItemStyle-Wrap="true"> 
              <HeaderTemplate>
                         <span class="HeadercenterAlign">Processor Name</span> <br />
                                <asp:TextBox ID="txtProcessorName" Width="120px" runat="server" AutoPostBack="true"  OnTextChanged="txtHeaderChanged"></asp:TextBox>
                            </HeaderTemplate>
             <ItemTemplate>   <asp:Label ID="lblProcesorName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PROCESSOR_NAME") %>'></asp:Label>  </ItemTemplate>

<ItemStyle Wrap="True"></ItemStyle>
         </asp:TemplateField>

          <asp:TemplateField ItemStyle-Wrap="true"> 
               <HeaderTemplate>
                   <span class="HeadercenterAlign">Module</span> <br />
                   <asp:DropDownList runat="server" ID="ddlModule" Width="100px" Height="20px" AutoPostBack="true" EnableViewState="true" OnSelectedIndexChanged="txtHeaderChanged"> </asp:DropDownList>
                            </HeaderTemplate>
               <ItemTemplate> <asp:Label ID="lblModule" runat="server" Text='  <%#  DataBinder.Eval(Container.DataItem,"MODULENAME") %>'></asp:Label>  </ItemTemplate>

<ItemStyle Wrap="True"></ItemStyle>
         </asp:TemplateField>                                   

          <asp:TemplateField ItemStyle-Wrap="true"> 
               <HeaderTemplate>
                   <span class="HeadercenterAlign">Doc Type</span> <br />
                   <asp:TextBox ID="txtDocType" runat="server" Width="90px" AutoPostBack="true"  OnTextChanged="txtHeaderChanged"></asp:TextBox>
               </HeaderTemplate>
               <ItemTemplate>  <asp:Label ID="lblDocType" runat="server" Text=' <%#  DataBinder.Eval(Container.DataItem,"DocType") %>'></asp:Label>  </ItemTemplate>

<ItemStyle Wrap="True"></ItemStyle>
         </asp:TemplateField> 

       <asp:TemplateField ItemStyle-Wrap="true"> 
               <HeaderTemplate>
                   <span class="HeadercenterAlign">Log Type</span> <br />
                                <asp:TextBox ID="txtLOGTYPE" Width="90px" runat="server" AutoPostBack="true"  OnTextChanged="txtHeaderChanged"></asp:TextBox>
                            </HeaderTemplate>
               <ItemTemplate>   <asp:Label ID="lblLogType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"LOGTYPE") %>'></asp:Label>  </ItemTemplate>

<ItemStyle Wrap="True"></ItemStyle>
         </asp:TemplateField> 

        <asp:TemplateField ItemStyle-Wrap="true"> 
               <HeaderTemplate>
                   <span class="HeadercenterAlign">Buyer Code</span> <br />
                                <asp:TextBox ID="txtBuyerCode" Width="90px" runat="server" AutoPostBack="true"  OnTextChanged="txtHeaderChanged"></asp:TextBox>
                            </HeaderTemplate>
               <ItemTemplate>   <asp:Label ID="lblBuyerCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"BuyerCode") %>'></asp:Label>  </ItemTemplate>

<ItemStyle Wrap="True"></ItemStyle>
         </asp:TemplateField> 

        <asp:TemplateField ItemStyle-Wrap="true"> 
               <HeaderTemplate>
                   <span class="HeadercenterAlign">Supplier Code</span> <br />
                                <asp:TextBox ID="txtSupplierCode" Width="110px" runat="server" AutoPostBack="true"  OnTextChanged="txtHeaderChanged"></asp:TextBox>
                            </HeaderTemplate>
               <ItemTemplate>   <asp:Label ID="lblSuppCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SupplierCode") %>'></asp:Label>  </ItemTemplate>

<ItemStyle Wrap="True"></ItemStyle>
         </asp:TemplateField> 

        <asp:TemplateField> 
               <HeaderTemplate>
                   <span class="HeadercenterAlign">Key Ref</span> <br />
                                <asp:TextBox ID="txtKeyRef" runat="server" Width="110px" AutoPostBack="true"  OnTextChanged="txtHeaderChanged"></asp:TextBox>
                            </HeaderTemplate>
               <ItemTemplate>  <asp:Label ID="lblKeyRef" runat="server" Text=' <%# DataBinder.Eval(Container.DataItem,"KEYREF2") %>'></asp:Label></ItemTemplate>
         </asp:TemplateField> 

          <asp:TemplateField> 
               <HeaderTemplate>
                   <span class="HeadercenterAlign">Remark</span> <br />
                                <asp:TextBox ID="txtRemark" runat="server"  Width="320px" AutoPostBack="true"  OnTextChanged="txtHeaderChanged"></asp:TextBox>
                            </HeaderTemplate>
               <ItemTemplate>   <asp:Label ID="lblRemark" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"AUDITVALUE") %>'></asp:Label>  </ItemTemplate>
         </asp:TemplateField> 

            <asp:TemplateField> 
               <HeaderTemplate>
                   <span class="HeadercenterAlign">File Name</span> <br />
                    <asp:TextBox ID="txtFileName" runat="server" Width="290px" AutoPostBack="true" OnTextChanged="txtHeaderChanged"></asp:TextBox>
                            </HeaderTemplate>
               <ItemTemplate>   
                     <asp:LinkButton ID="lnkFile" Text='<%# Bind("FILENAME") %>' runat="server" CommandName="View" CommandArgument="<%# Container.DataItemIndex %>"><%# Eval("FILENAME") %></asp:LinkButton>
               </ItemTemplate>
         </asp:TemplateField> 
         <asp:BoundField DataField = "LOGID" HeaderText = "Id"   Visible="false">  </asp:BoundField>
         <asp:BoundField DataField = "BUYER_ID" HeaderText = "BuyerID"  Visible="false">  </asp:BoundField>
         <asp:BoundField DataField = "SUPPLIER_ID" HeaderText = "SupplierID" Visible="false">  </asp:BoundField>               
                </Columns>
          <PagerStyle CssClass="GridPager" />
        
        </asp:GridView>
                     </div>
                 </div>             
              </div>
            </div>
    </div>
  </div>
 </div>
</div>
    </form>
    
        <div id="datefilter" class="modal fade in" style="top:35px;z-index:10;display:none;left:-1200px;">
          <div class="modal-dialog modal-sm"> <div class="modal-content">
              <div class="modal-body">
                 <div class="row">  <div class="col-md-11"><div class="form-group">
                     <div class="col-md-2">From</div>
                       <div class="col-md-9 paddspaces"><input class="form-control date-picker csDatePicker InputText" data-date-format="dd/mm/yyyy" size="16" type="text" id="dtLogFromDate" value=""/></div>
                    </div> </div></div>
                  <div class="clearfix"></div>
                      <div class="row">  <div class="col-md-11"><div class="form-group">
                     <div class="col-md-2">To</div>
                       <div class="col-md-9 paddspaces"><input class="form-control date-picker csDatePicker InputText" data-date-format="dd/mm/yyyy" size="16" type="text" id="dtLogToDate" value=""/></div>
                    </div> </div></div>
                  </div>
    <%--        
             <div class="modal-footer" style="padding-right:25px;text-align:center;margin-right:0px;">                                                                           
                      <button type="button" id="btnApp"  class="btn yellow-casablanca mdfootrbtn">Apply</button>                                                                         
              </div> --%>
               </div></div>       
        </div>

</body>
</html>
