<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MakePickTicket.aspx.vb" Inherits="PC_MakePickTicket" title="Untitled Page" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            Make Pick Ticket &nbsp;&nbsp;
            <asp:HyperLink ID="HyperLinkReport" runat="server" Visible="false">Open Report</asp:HyperLink>
            &nbsp;<asp:Button ID="ButtonRunReport" runat="server" Text="Run SO Report" /><br />
            <br />

            <asp:Panel ID="SSPanel" runat="server" Width="880px">
                Select a Scheduled Shipment<br />
                <br />
                <asp:Panel ID="PanelLink" runat="server" Height="58px" Width="239px"
                    BackColor="#FFFF66" BorderColor="Black" BorderStyle="Solid" Visible="False">

                    <br />
                    <div style="text-align: center">
                        <asp:HyperLink ID="HyperLinkPickticket" runat="server"
                            Style="text-align: center">Open Pick Ticket</asp:HyperLink>
                    </div>
                    <br />
                </asp:Panel>
                <cc1:AlwaysVisibleControlExtender ID="PanelLink_AlwaysVisibleControlExtender"
                    runat="server" Enabled="True" HorizontalSide="Center"
                    TargetControlID="PanelLink" VerticalSide="Middle">
                </cc1:AlwaysVisibleControlExtender>
                <asp:SqlDataSource ID="ScheduledShipmentsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT WorkWeek, ID, SO, FGI, Qty, DockDate, DayofWeek, Entry FROM dbo.q_SalesSchedule ORDER BY WorkWeek, ID"></asp:SqlDataSource>
                <asp:GridView ID="ScheduledShipmentsGridView" runat="server"
                    AllowSorting="True" AutoGenerateColumns="False" CellPadding="4"
                    DataSourceID="ScheduledShipmentsSqlDataSource" ForeColor="#333333"
                    GridLines="None">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:ButtonField ButtonType="Button" CommandName="MakePT" Text="Make PT" />
                        <asp:BoundField DataField="WorkWeek" HeaderText="WorkWeek"
                            SortExpression="WorkWeek" />
                        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                        <asp:BoundField DataField="SO" HeaderText="SO" SortExpression="SO" />
                        <asp:BoundField DataField="FGI" HeaderText="FGI" SortExpression="FGI" />
                        <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" />
                        <asp:BoundField DataField="DockDate" DataFormatString="{0:d}"
                            HeaderText="DockDate" SortExpression="DockDate" />
                        <asp:BoundField DataField="DayofWeek" HeaderText="DayofWeek"
                            SortExpression="DayofWeek" />
                        <asp:BoundField DataField="Entry" HeaderText="Entry" SortExpression="Entry" />
                    </Columns>
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </asp:Panel>
            &nbsp;
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

