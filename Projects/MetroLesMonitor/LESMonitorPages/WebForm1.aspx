<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/BaseMenu.master" AutoEventWireup="true" CodeFile="WebForm1.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.WebForm1" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridLookup" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHiddenField" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ChildGridContent" runat="server">
    <form runat="server">  
           <asp:ScriptManager ID="scripman1" runat="server" EnablePageMethods="True"> </asp:ScriptManager>
         <dx:ASPxGridView ID="gvAuditLog" runat="server" AutoGenerateColumns="False" 
                KeyFieldName="LOGID"  ClientInstanceName="gvAuditLog"  OnAutoFilterCellEditorCreate="ASPxGridView1_AutoFilterCellEditorCreate"
                OnAutoFilterCellEditorInitialize="ASPxGridView1_AutoFilterCellEditorInitialize"
                OnProcessColumnAutoFilter="ASPxGridView1_ProcessColumnAutoFilter" 
                Width="100%" Font-Names="Verdana" Font-Size="8pt">
                <Settings UseFixedTableLayout="true" ShowFilterRowMenu="true" HorizontalScrollBarMode="Auto" ShowFilterRowMenuLikeItem="True" />
                <SettingsBehavior FilterRowMode="OnClick" ColumnResizeMode="NextColumn"/>
                <Styles>
                    <FilterCell Font-Names="Verdana" Font-Size="8pt"></FilterCell>
                </Styles>
                <Columns>
                    <dx:GridViewDataDateColumn FieldName="UPDATEDATE" VisibleIndex="0" Caption="Log Date" Width="70px" HeaderStyle-Wrap="True" SortIndex = "0">
                        <HeaderStyle Wrap="True"></HeaderStyle>
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataTextColumn FieldName="ServerName" Caption="Server Name" VisibleIndex="1" Width="70px" CellStyle-CssClass="Wrap_Text" HeaderStyle-Wrap="True">
                        <Settings ShowFilterRowMenu="true" />
                        <HeaderStyle Wrap="True"></HeaderStyle>
                        <CellStyle CssClass="Wrap_Text"></CellStyle>
                         <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
         <%--           <dx:GridViewDataTextColumn FieldName="PROCESSOR_NAME" Caption="Processor Name" VisibleIndex="1" Width="70px" CellStyle-CssClass="Wrap_Text" HeaderStyle-Wrap="True">
                        <Settings ShowFilterRowMenu="true" />
                        <HeaderStyle Wrap="True"></HeaderStyle>
                        <CellStyle CssClass="Wrap_Text"></CellStyle>
                         <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>--%>
                    <dx:GridViewDataTextColumn FieldName="MODULENAME" Caption="Module" VisibleIndex="1" Width="70px" CellStyle-CssClass="Wrap_Text" HeaderStyle-Wrap="True">
                        <Settings ShowFilterRowMenu="true" />
                        <HeaderStyle Wrap="True"></HeaderStyle>
                        <CellStyle CssClass="Wrap_Text"></CellStyle>
                         <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="DocType" Caption="DocType" VisibleIndex="1" Width="70px" CellStyle-CssClass="Wrap_Text" HeaderStyle-Wrap="True">
                        <Settings ShowFilterRowMenu="true" />
                        <HeaderStyle Wrap="True"></HeaderStyle>
                        <CellStyle CssClass="Wrap_Text"></CellStyle>
                         <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Log Type" FieldName="LOGTYPE" VisibleIndex="2" Width="70px" CellStyle-CssClass="Wrap_Text" HeaderStyle-Wrap="True">
                        <Settings ShowFilterRowMenu="true" />
                        <HeaderStyle Wrap="True"></HeaderStyle>
                        <CellStyle CssClass="Wrap_Text"></CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="BUYER_CODE" Caption="Buyer Code" VisibleIndex="3" Width="70px" CellStyle-CssClass="Wrap_Text" HeaderStyle-Wrap="True">
                        <HeaderStyle Wrap="True"></HeaderStyle>
                        <CellStyle CssClass="Wrap_Text"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="VENDOR_CODE" Caption="Supplier Code" VisibleIndex="4" Width="70px" CellStyle-CssClass="Wrap_Text" HeaderStyle-Wrap="True">
                        <HeaderStyle Wrap="True"></HeaderStyle>
                        <CellStyle CssClass="Wrap_Text"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="KEYREF2" Caption="Key Ref" VisibleIndex="5" Width="100px" CellStyle-CssClass="Wrap_Text" HeaderStyle-Wrap="True">
                        <HeaderStyle Wrap="True"></HeaderStyle>
                        <CellStyle CssClass="Wrap_Text"></CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataMemoColumn FieldName="AUDITVALUE" Caption="Remark" VisibleIndex="6" Width="50%" CellStyle-CssClass="Wrap_Text" HeaderStyle-Wrap="True">
                        <HeaderStyle Wrap="True"></HeaderStyle>
                        <CellStyle CssClass="Wrap_Text"></CellStyle>
                        <PropertiesMemoEdit EncodeHtml="true"></PropertiesMemoEdit>
                    </dx:GridViewDataMemoColumn>
                    <dx:GridViewDataTextColumn FieldName="FILENAME" Caption="File Name" ReadOnly="True" VisibleIndex="7" Width="150px" CellStyle-CssClass="Wrap_Text" HeaderStyle-Wrap="True"> <%--Sayli 22Mar16--%>
                        <PropertiesTextEdit DisplayFormatString="{0}"></PropertiesTextEdit>
                      <%--  <DataItemTemplate>
                            <asp:LinkButton ID="lnkFile" runat="server" Text='<%# Container.Text %>' OnClick="lnkFile_Click"></asp:LinkButton>
                        </DataItemTemplate>--%>
                        <HeaderStyle Wrap="True"></HeaderStyle>
                        <CellStyle CssClass="Wrap_Text"></CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="LOGID" Caption="Log ID" ReadOnly="true" Visible="false" VisibleIndex="6"> <%--Sayli 22Mar16--%>
                    </dx:GridViewDataTextColumn>
                 <%--   <dx:GridViewDataColumn ReadOnly="True" Caption="Resubmit File" VisibleIndex="8" Width="70px">
                        <HeaderStyle Wrap="True"></HeaderStyle>
                        <DataItemTemplate>
                            <a id="lnkResubmit" href="javascript:void{0};" onclick="ShowSubmitPopup('<%# Container.KeyValue %>')">Resubmit</a>
                        </DataItemTemplate>
                    </dx:GridViewDataColumn>--%>
                    <dx:GridViewCommandColumn ShowApplyFilterButton="true" VisibleIndex="8" Width="40px" Caption=" "> <%--Sayli 22Mar16--%>
                    </dx:GridViewCommandColumn>
                     <dx:GridViewDataTextColumn FieldName="BUYER_ID" VisibleIndex="33" Width="0%" Caption=" ">
                                        <HeaderStyle CssClass="hideCell" />
                                        <EditCellStyle CssClass="hideCell" />
                                        <CellStyle CssClass="hideCell" />
                                        <FilterCellStyle CssClass="hideCell" />
                                        <FooterCellStyle CssClass="hideCell" />
                                        <GroupFooterCellStyle CssClass="hideCell" />
                                    </dx:GridViewDataTextColumn>
                                    <%--BUYERID--%>
                                    <dx:GridViewDataTextColumn FieldName="SUPPLIER_ID" VisibleIndex="34" Width="0%" Caption=" ">
                                        <HeaderStyle CssClass="hideCell" />
                                        <EditCellStyle CssClass="hideCell" />
                                        <CellStyle CssClass="hideCell" />
                                        <FilterCellStyle CssClass="hideCell" />
                                        <FooterCellStyle CssClass="hideCell" />
                                        <GroupFooterCellStyle CssClass="hideCell" />
                                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsPager PageSize="200" AlwaysShowPager="True">
                </SettingsPager>
                <Settings ShowFilterRow="true"/>
            </dx:ASPxGridView>
        </form>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="childScriptHolder" runat="server">
       <script src="../assets/Scripts/WebForm1.js"></script>
        <script type="text/javascript">         
            $(document).ready(function () {
                WebForm1.init();
                var timeoutID;
                var _dt_To = null;
                function ApplyFilter(dde, dateFrom, dateTo, dt, mm, yyyy) {
                    var dtf = 30, mmf = mm;
                    var d1 = dateFrom.GetText();
                    var d2 = dateTo.GetText();
                    if (d2 == '') {
                        if (_dt_To == null) {
                            d2 = (dtTo.getMonth() + 1) + "-" + dtTo.getDate() + "-" + dtTo.getFullYear();
                            _dt_To = d2;
                        }
                        else {
                            d2 = _dt_To;
                        }
                    }
                    else {
                        _dt_To = d2;
                    }

                    if (dt > 1) dtf = dt - 1;
                    else mmf = mm - 1;
                    if (d1 == "") {
                        var dtFrom = new Date(yyyy, mmf, dtf);
                        d1 = (dtFrom.getMonth() + 1) + "-" + dtFrom.getDate() + "-" + dtFrom.getFullYear();
                    }
                    if (d2 == "") {
                        var dtTo = new Date(yyyy, mm, dt);
                        d2 = (dtTo.getMonth() + 1) + "-" + dtTo.getDate() + "-" + dtTo.getFullYear();
                    }
                    dde.SetValue(d1 + " 00:00:00 AM |" + d2 + " 11:59:59 PM");
                    gvAuditLog.AutoFilterByColumn("UPDATEDATE", dde.GetText());
                }

                function OnDropDown(s, dateFrom, dateTo, dt, mm, yyyy) {
                    var str = s.GetValue();
                    if (str == null) { str = "|"; }
                    var d = str.split('|');
                    dateFrom.SetText(d[0]);
                    //dateTo.SetText(d[1]);
                    if (d[1] != undefined) {
                        dateTo.SetText(d[1]);
                    }
                    else {
                        if (_dt_To != null) {
                            dateTo.SetText(_dt_To);
                        }
                        else {
                            var dtTo = new Date(yyyy, mm, dt);
                            _dt_To = (dtTo.getMonth() + 1) + "-" + dtTo.getDate() + "-" + dtTo.getFullYear();
                            dateTo.SetDate(dtTo);
                        }
                    }
                }


            });
            
        
    </script>
</asp:Content>
