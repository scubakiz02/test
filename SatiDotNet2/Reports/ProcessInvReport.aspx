<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ProcessInvReport.aspx.vb" Inherits="Reports_ProcessInvReport" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    In Process Wafers Grouped By ID &amp; Stage:<br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="736px">
    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
        CellPadding="4" DataSourceID="SqlDataSource1" Width="280px" ForeColor="#333333" GridLines="None">
        <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
            <asp:BoundField DataField="StageName" HeaderText="StageName" SortExpression="StageName" />
            <asp:BoundField DataField="InQty" HeaderText="InQty" SortExpression="InQty" />
            <asp:BoundField DataField="OutQty" HeaderText="OutQty" SortExpression="OutQty" />
        </Columns>
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <AlternatingRowStyle BackColor="LightBlue" />
    </asp:GridView>
                <br />
            </asp:Panel>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT LEFT (dbo.UniqueProcesses.LotEntry, 4) AS ID, dbo.UniqueProcesses.StageName, SUM(dbo.WaferMover.InQty) AS InQty, SUM(dbo.WaferMover.OutQty) AS OutQty FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final Pack')) GROUP BY dbo.UniqueProcesses.StageName, LEFT (dbo.UniqueProcesses.LotEntry, 4) ORDER BY LEFT (dbo.UniqueProcesses.LotEntry, 4), dbo.UniqueProcesses.StageName">
    </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    &nbsp;
</asp:Content>

