<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="Partial Inv.aspx.vb" Inherits="PC_Partial_Inv" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <table>
        <tr>
            <td style="vertical-align: top; width: 100px; text-align: left">
                <asp:Panel ID="Panel2" runat="server" Width="312px">
                    Polish Parials<br />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataSourceID="PolishPartialSqlDataSource" ForeColor="#333333" GridLines="None"
                    Width="216px">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:BoundField DataField="LotEntry" HeaderText="LotEntry" SortExpression="LotEntry" />
                        <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" />
                    </Columns>
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="LightBlue" />
                </asp:GridView>
                    <br />
                </asp:Panel>
                <asp:SqlDataSource ID="PolishPartialSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT LotEntry, SUM(InQty) AS Qty FROM dbo.WaferMover WHERE ([Order] = 0) GROUP BY LotEntry HAVING (LotEntry LIKE N'%-xxxx') AND (SUM(OutQty) = 0) ORDER BY LotEntry">
                </asp:SqlDataSource>
            </td>
            <td style="vertical-align: top; width: 100px; text-align: left">
                <asp:Panel ID="Panel3" runat="server" Width="312px">
                    Cleanroom Partials<br />
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataSourceID="CleanroomPartialsSqlDataSource" ForeColor="#333333" GridLines="None"
                    Width="232px">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:BoundField DataField="LotEntry" HeaderText="LotEntry" SortExpression="LotEntry" />
                        <asp:BoundField DataField="Qty" HeaderText="Qty" ReadOnly="True" SortExpression="Qty" />
                    </Columns>
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="LightBlue" />
                </asp:GridView>
                    <br />
                </asp:Panel>
                <asp:SqlDataSource ID="CleanroomPartialsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT LotEntry, SUM(InQty) AS Qty FROM dbo.WaferMover WHERE ([Order] = 0) GROUP BY LotEntry HAVING (LotEntry LIKE N'%-zzzz') AND (SUM(OutQty) = 0) ORDER BY LotEntry">
                </asp:SqlDataSource>
            </td>
        </tr>
    </table>
    <br />
    <br />
</asp:Content>

