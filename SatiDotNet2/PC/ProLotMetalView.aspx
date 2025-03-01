<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ProLotMetalView.aspx.vb" Inherits="PC_ProLotMetalView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="View Production Lot Metals"></asp:Label><br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            Last 365 days of exclusive metals data for lots.
            <br />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataSourceID="SqlDataSource1">
                <Columns>
                    <asp:BoundField DataField="Date/Time" HeaderText="Date/Time" SortExpression="Date/Time" />
                    <asp:BoundField DataField="Source" HeaderText="Source" SortExpression="Source" />
                    <asp:BoundField DataField="Test Type" HeaderText="Test Type" SortExpression="Test Type" />
                    <asp:BoundField DataField="Idenyification" HeaderText="Idenyification" SortExpression="Idenyification" />
                    <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location" />
                    <asp:BoundField DataField="Ca" HeaderText="Ca" ReadOnly="True" SortExpression="Ca" />
                    <asp:BoundField DataField="Mg" HeaderText="Mg" ReadOnly="True" SortExpression="Mg" />
                    <asp:BoundField DataField="Ni" HeaderText="Ni" ReadOnly="True" SortExpression="Ni" />
                    <asp:BoundField DataField="Zn" HeaderText="Zn" ReadOnly="True" SortExpression="Zn" />
                    <asp:BoundField DataField="Al" HeaderText="Al" ReadOnly="True" SortExpression="Al" />
                    <asp:BoundField DataField="Fe" HeaderText="Fe" ReadOnly="True" SortExpression="Fe" />
                    <asp:BoundField DataField="Cr" HeaderText="Cr" ReadOnly="True" SortExpression="Cr" />
                    <asp:BoundField DataField="Cu" HeaderText="Cu" ReadOnly="True" SortExpression="Cu" />
                    <asp:BoundField DataField="Na" HeaderText="Na" ReadOnly="True" SortExpression="Na" />
                    <asp:BoundField DataField="K" HeaderText="K" ReadOnly="True" SortExpression="K" />
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                </Columns>
                <FooterStyle BackColor="White" ForeColor="#000066" />
                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <RowStyle ForeColor="#000066" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#00547E" />
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT [Date/Time], Source, [Test Type], Idenyification, Location, ROUND(Ca, 3) AS Ca, ROUND(Ma, 3) AS Mg, ROUND(Ni, 3) AS Ni, ROUND(Zn, 3) AS Zn, ROUND(Al, 3) AS Al, ROUND(Fe, 3) AS Fe, ROUND(Cr, 3) AS Cr, ROUND(Cu, 3) AS Cu, ROUND(Na, 3) AS Na, ROUND(K, 3) AS K, Notes FROM [GFAAS Data] WHERE (Source LIKE N'Pro%') AND ([Date/Time] &gt; DATEADD(year, - 1, GETDATE())) AND (Notes IS NULL) ORDER BY [Date/Time] DESC"></asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

