<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeBehind="AuditGrid.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.AuditGrid" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">        
 <div class="row">         
  <div class="col-md-12">                 
    <div class="portlet light">  
      <div class="portlet-body" id="portlet_body">  
        <form id="form1" runat="server">
         <div class="row">
            <div class="col-md-12">
      
                <asp:GridView ID="gvData" runat="server" AllowPaging="True" PageSize="30" AutoGenerateColumns = "False" DataKeyNames="LOGID" 
          OnPageIndexChanging="gvData_PageIndexChanging"  BackColor="#CCCCCC"  OnRowDataBound="gvData_RowDataBound"
          CellPadding="4" CellSpacing="2" ForeColor="Black" ShowHeaderWhenEmpty="True" OnRowCommand="gvData_RowCommand">
        <Columns>        
          <asp:TemplateField> 
              <HeaderTemplate>
                    <span class="HeadercenterAlign">Log Date</span> <br /> 
                    <div id="datefilter" style="width:180px;">
                 <table>
                      <tr>
                      <td>From : </td> 
                      <td>
                            <input id="dtLogFromDate" runat="server"   type="text"/>                          
                      </td>
                      </tr>
                       <tr>
                      <td style="text-align:right;">To : </td> 
                      <td>
                        <input id="dtLogToDate" runat="server" type="text"/>   
                      </td>
                      </tr>                    
                  </table>  
            </div>  
                    </HeaderTemplate>
              <ItemTemplate>   <asp:Label ID="lblUpdateDate" runat="server" Text=' <%# DataBinder.Eval(Container.DataItem, "UPDATEDATE")%>'></asp:Label>  </ItemTemplate>
              </asp:TemplateField>

         <asp:TemplateField ItemStyle-Wrap="true"> 
             <HeaderTemplate>
                     <span class="HeadercenterAlign">Server Name</span> <br />
                        <asp:TextBox ID="txtServerName" runat="server" Width="100px" AutoPostBack="true"  OnTextChanged="txtHeaderChanged"></asp:TextBox>
                    </HeaderTemplate>
             <ItemTemplate>   <asp:Label ID="lblServerName" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem,"ServerName") %>'></asp:Label>  </ItemTemplate>
          </asp:TemplateField>

          <asp:TemplateField ItemStyle-Wrap="true"> 
              <HeaderTemplate>
                         <span class="HeadercenterAlign">Processor Name</span> <br />
                                <asp:TextBox ID="txtProcessorName" Width="120px" runat="server" AutoPostBack="true"  OnTextChanged="txtHeaderChanged"></asp:TextBox>
                            </HeaderTemplate>
             <ItemTemplate>   <asp:Label ID="lblProcesorName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PROCESSOR_NAME") %>'></asp:Label>  </ItemTemplate>
         </asp:TemplateField>

          <asp:TemplateField ItemStyle-Wrap="true"> 
               <HeaderTemplate>
                   <span class="HeadercenterAlign">Module</span> <br />
                   <asp:DropDownList runat="server" ID="ddlModule" Width="100px" Height="20px" AutoPostBack="true" EnableViewState="true" OnSelectedIndexChanged="txtHeaderChanged"> </asp:DropDownList>
                            </HeaderTemplate>
               <ItemTemplate> <asp:Label ID="lblModule" runat="server" Text='  <%#  DataBinder.Eval(Container.DataItem,"MODULENAME") %>'></asp:Label>  </ItemTemplate>
         </asp:TemplateField>                                   

          <asp:TemplateField ItemStyle-Wrap="true"> 
               <HeaderTemplate>
                   <span class="HeadercenterAlign">Doc Type</span> <br />
                   <asp:TextBox ID="txtDocType" runat="server" Width="90px" AutoPostBack="true"  OnTextChanged="txtHeaderChanged"></asp:TextBox>
               </HeaderTemplate>
               <ItemTemplate>  <asp:Label ID="lblDocType" runat="server" Text=' <%#  DataBinder.Eval(Container.DataItem,"DocType") %>'></asp:Label>  </ItemTemplate>
         </asp:TemplateField> 

       <asp:TemplateField ItemStyle-Wrap="true"> 
               <HeaderTemplate>
                   <span class="HeadercenterAlign">Log Type</span> <br />
                                <asp:TextBox ID="txtLOGTYPE" Width="90px" runat="server" AutoPostBack="true"  OnTextChanged="txtHeaderChanged"></asp:TextBox>
                            </HeaderTemplate>
               <ItemTemplate>   <asp:Label ID="lblLogType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"LOGTYPE") %>'></asp:Label>  </ItemTemplate>
         </asp:TemplateField> 


        <asp:TemplateField ItemStyle-Wrap="true"> 
               <HeaderTemplate>
                   <span class="HeadercenterAlign">Buyer Code</span> <br />
                                <asp:TextBox ID="txtBuyerCode" Width="90px" runat="server" AutoPostBack="true"  OnTextChanged="txtHeaderChanged"></asp:TextBox>
                            </HeaderTemplate>
               <ItemTemplate>   <asp:Label ID="lblBuyerCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"BuyerCode") %>'></asp:Label>  </ItemTemplate>
         </asp:TemplateField> 


        <asp:TemplateField ItemStyle-Wrap="true"> 
               <HeaderTemplate>
                   <span class="HeadercenterAlign">Supplier Code</span> <br />
                                <asp:TextBox ID="txtSupplierCode" Width="110px" runat="server" AutoPostBack="true"  OnTextChanged="txtHeaderChanged"></asp:TextBox>
                            </HeaderTemplate>
               <ItemTemplate>   <asp:Label ID="lblSuppCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SupplierCode") %>'></asp:Label>  </ItemTemplate>
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
          <FooterStyle BackColor="#CCCCCC" />
          <HeaderStyle BackColor="#f5f5f5" Font-Bold="True" ForeColor="Black" />
          <PagerStyle BackColor="#CCCCCC" ForeColor="Black" CssClass="GridPager" HorizontalAlign="Left" />
          <RowStyle BackColor="White" />
        
        </asp:GridView>
            </div>
          </div>             
         </form>
      </div>
   </div>
 </div>
 </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="childScriptHolder" runat="server">   
    <link href="../assets/css/filtertable.css" rel="stylesheet" />
    <link href="../assets/css/Auditlog.css" rel="stylesheet" />
    <script src="../assets/Scripts/ob/Auditlog.js"></script>
    <script  type="text/javascript">
        jQuery(document).ready(function () {
            AuditLog.init();
            //var date = new Date();
            //if (document.getElementById("Hidden1").value != '') {
            //    var _list = []; _list = document.getElementById("Hidden1").value.split('|');
            //    $(document.getElementById('dtLogFromDate')).val(_list[0]); $(document.getElementById('dtLogToDate')).val(_list[1]);
            //}
            //else {
            //    $(document.getElementById('dtLogFromDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
            //    $(document.getElementById('dtLogToDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
            //}
            //$('#dtLogFromDate').datepicker().on('changeDate', function (ev) { $('#dtLogFromDate').datepicker('hide'); });
            //$('#dtLogToDate').datepicker().on('changeDate', function (ev) { $('#dtLogToDate').datepicker('hide'); });
        });

        function GetValues() { var fromdate = $('#dtLogFromDate').val();var todate = $('#dtLogToDate').val(); document.getElementById("Hidden1").value = fromdate + '|' + todate;};
        function ClearValues() {
            $(document.getElementById('dtLogFromDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));
            $(document.getElementById('dtLogToDate')).datepicker("setDate", new Date(date.getFullYear(), date.getMonth(), date.getDate()));document.getElementById("Hidden1").value = ''; };
</script>
</asp:Content>

