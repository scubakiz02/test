<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="IDPathMaintenance.aspx.vb" Inherits="DBMaintenance_IDPathMaintenance" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <strong><span style="font-size: 16pt"></span></strong>
        <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Size="X-Large" Text="ID Path Maintenance:"></asp:Label><br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="915px">
                <br />
                <table>
                    <tr>
                        <td style="width: 100px">
                            ID Name:
    <asp:Label ID="IDLabel" runat="server" Font-Bold="True" Width="112px"></asp:Label></td>
                        <td style="width: 100px">
                            Path Name :&nbsp;
    <asp:Label ID="PathLabel" runat="server" Font-Bold="True" Width="112px"></asp:Label></td>
                        <td style="width: 100px">
    <asp:Label ID="Label2" runat="server" Height="16px" Text="IDs that share this path:" Width="144px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 100px; text-align: left">
    <asp:GridView ID="GridView2" runat="server" AllowSorting="True" AutoGenerateColumns="False"
        CellPadding="4" DataKeyNames="MainID" DataSourceID="PathNameForIDSqlDataSource"
        ForeColor="#333333" GridLines="None">
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:BoundField DataField="MainID" HeaderText="MainID" ReadOnly="True" SortExpression="MainID" />
            <asp:BoundField DataField="PathName" HeaderText="PathName" SortExpression="PathName" />
            <asp:ButtonField ButtonType="Button" CommandName="Select" Text="View" />
            <asp:ButtonField ButtonType="Button" CommandName="New" Text="New" />
        </Columns>
        <RowStyle BackColor="#EFF3FB" />
        <EditRowStyle BackColor="#2461BF" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="LightBlue" />
    </asp:GridView>
                        </td>
                        <td style="vertical-align: top; width: 100px; text-align: left">
    <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" CellPadding="4"
        DataKeyNames="PathName,ProcessOrder" DataSourceID="SqlDataSource2" ForeColor="#333333"
        GridLines="None"
        Width="304px">
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:BoundField DataField="PathName" HeaderText="PathName" ReadOnly="True" SortExpression="PathName" />
            <asp:BoundField DataField="ProcessOrder" HeaderText="ProcessOrder" ReadOnly="True"
                SortExpression="ProcessOrder" />
            <asp:BoundField DataField="StageName" HeaderText="StageName" SortExpression="StageName" />
        </Columns>
        <RowStyle BackColor="#EFF3FB" />
        <EditRowStyle BackColor="#2461BF" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="LightBlue" />
    </asp:GridView>
                        </td>
                        <td style="vertical-align: top; width: 100px; text-align: left">
    <asp:ListBox ID="ListBox1" runat="server" DataSourceID="SqlDataSource1" DataTextField="ID"
        DataValueField="ID" Height="120px" Width="104px"></asp:ListBox></td>
                    </tr>
                </table>
            </asp:Panel>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT ID FROM dbo.WI_Rev WHERE (PathName = N'')"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT PathName, ProcessOrder, StageName FROM dbo.CannedPaths WHERE (PathName = N'')">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="PathNameForIDSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT dbo.MainID.MainID, dbo.WI_Rev.PathName FROM dbo.WI_Rev RIGHT OUTER JOIN dbo.MainID ON dbo.WI_Rev.ID = dbo.MainID.MainID WHERE (dbo.WI_Rev.ExpirationDtd IS NULL)">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="IDsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT MainID FROM dbo.MainID GROUP BY MainID ORDER BY MainID"></asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

