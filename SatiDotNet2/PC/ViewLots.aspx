<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ViewLots.aspx.vb" Inherits="PC_ViewLots" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="View Lots In Process"></asp:Label><br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="915px">
                Filter By Stage:
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2"
                    DataTextField="StageName" DataValueField="StageName" Width="200px" AppendDataBoundItems="True">
                    <asp:ListItem Selected="True">Select One...</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="Button1" runat="server" Text="Clear Filter" /><br />
            </asp:Panel>
            <br />
            <asp:Panel ID="Panel2" runat="server" Width="915px">
    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
        DataSourceID="SqlDataSource1" Width="512px" CellPadding="4" ForeColor="#333333" GridLines="None">
        <Columns>
            <asp:BoundField DataField="LotNumber" HeaderText="LotNumber" SortExpression="LotNumber" />
            <asp:BoundField DataField="StageName" HeaderText="StageName" SortExpression="StageName" />
            <asp:BoundField DataField="In" HeaderText="In" SortExpression="In" />
            <asp:BoundField DataField="Out" HeaderText="Out" SortExpression="Out" />
        </Columns>
        <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <AlternatingRowStyle BackColor="LightBlue" />
    </asp:GridView>
                <br />
            </asp:Panel>
            <br />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, SUM(dbo.WaferMover.InQty) AS [In], SUM(dbo.WaferMover.OutQty) AS Out FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT dbo.UniqueProcesses.StageName FROM dbo.UniqueProcesses LEFT OUTER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] WHERE (NOT (dbo.WaferMover.Disposition IS NULL)) GROUP BY dbo.UniqueProcesses.Complete, dbo.UniqueProcesses.StageName HAVING (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check'))">
    </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
    &nbsp; &nbsp;&nbsp;
</asp:Content>

