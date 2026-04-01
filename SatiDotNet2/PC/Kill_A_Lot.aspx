<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="Kill_A_Lot.aspx.vb" Inherits="PC_Kill_A_Lot" title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="904px">
                Kill Lots<br />
                <br />
                Type in lot number
                <asp:TextBox ID="LotTextBox" runat="server"></asp:TextBox>&nbsp;<asp:Button ID="Button1"
                    runat="server" OnClick="Button1_Click" Text="Kill Lot" /><br />
                <br />
                <br />
                <br />
                All Lot in Prosses<br />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None" AllowSorting="True" OnRowCommand="GridView1_RowCommand" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="LotNumber" HeaderText="LotNumber" SortExpression="LotNumber" />
                        <asp:BoundField DataField="StageName" HeaderText="StageName" SortExpression="StageName" />
                        <asp:BoundField DataField="In" HeaderText="In" ReadOnly="True" SortExpression="In" />
                        <asp:BoundField DataField="Out" HeaderText="Out" ReadOnly="True" SortExpression="Out" />
                        <asp:ButtonField ButtonType="Button" Text="Kill" CommandName="Kill" />
                    </Columns>
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="LightBlue" />
                </asp:GridView>
                <br />
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, SUM(dbo.WaferMover.InQty) AS [In], SUM(dbo.WaferMover.OutQty) AS Out FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName">
                </asp:SqlDataSource>
                &nbsp;<br />
                <br />
                <br />
                <br />
                &nbsp;</asp:Panel>
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

