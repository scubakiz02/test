<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MakeReworkLot.aspx.vb" Inherits="MakeReworkLot" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Info Box<br />
    &nbsp;<asp:TextBox ID="InfoTextBox" runat="server" ForeColor="Red" Height="40px"
        TextMode="MultiLine" Width="552px"></asp:TextBox>
    <br />
    <br />
    <table style="width: 752px">
        <tr>
            <td style="width: 151px">
                Strip Etch
            </td>
            <td style="width: 114px">
                Lap</td>
            <td style="width: 142px">
                Polish</td>
        </tr>
        <tr>
            <td style="vertical-align: top; width: 151px; background-color: #33ffff; height: 179px;">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SE_Rework_SqlDataSource"
                    Width="160px">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                        <asp:BoundField DataField="Qty" HeaderText="Qty" ReadOnly="True" SortExpression="Qty" />
                        <asp:TemplateField HeaderText="Lot Qty">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Width="70px">0</asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:ButtonField ButtonType="Button" Text="Make Lot" CommandName="MakeLot" />
                    </Columns>
                </asp:GridView>
            </td>
            <td style="vertical-align: top; width: 114px; background-color: #ffff66; height: 179px;">
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataSourceID="Lap_Rework_SqlDataSource"
                    Style="vertical-align: top" Width="176px">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                        <asp:BoundField DataField="Qty" HeaderText="Qty" ReadOnly="True" SortExpression="Qty" />
                        <asp:TemplateField HeaderText="Lot Qty">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Width="70px">0</asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:ButtonField ButtonType="Button" Text="Make Lot" CommandName="MakeLot" />
                    </Columns>
                </asp:GridView>
            </td>
            <td style="vertical-align: top; width: 142px; background-color: #ffcc66; height: 179px;">
                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" DataSourceID="Polish_Rework_SqlDataSource"
                    Style="vertical-align: top" Width="168px">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                        <asp:BoundField DataField="Qty" HeaderText="Qty" ReadOnly="True" SortExpression="Qty" />
                        <asp:TemplateField HeaderText="Lot Qty">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox3" runat="server" Width="70px">0</asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:ButtonField ButtonType="Button" Text="Make Lot" CommandName="MakeLot" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <asp:SqlDataSource ID="SE_Rework_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT ID, SUM(Qty) AS Qty FROM dbo.T_Rework_Invintory GROUP BY Type, ID HAVING (Type = N'-6') ORDER BY ID">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="Lap_Rework_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT ID, SUM(Qty) AS Qty FROM dbo.T_Rework_Invintory GROUP BY Type, ID HAVING (Type = N'-4') ORDER BY ID">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="Polish_Rework_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT ID, SUM(Qty) AS Qty FROM dbo.T_Rework_Invintory GROUP BY Type, ID HAVING (Type = N'-5') ORDER BY ID">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource1Junk" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT dbo.CannedPaths.ProcessOrder AS Step, dbo.CannedPaths.StageName AS Stage FROM dbo.CannedPathInfo INNER JOIN dbo.CannedPaths ON dbo.CannedPathInfo.PathName = dbo.CannedPaths.PathName WHERE (dbo.CannedPathInfo.MainID = '2505') AND (dbo.CannedPathInfo.PathType = 'SERework')">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2Junk" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT FieldName, Value FROM dbo.DB_Characteristics WHERE (FieldName = N'RecWaferlog')">
    </asp:SqlDataSource>
</asp:Content>

