<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MakeLotPart2.aspx.vb" Inherits="PC_MakeLotPart2" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td style="width: 100px; text-align: right">
                ID#:</td>
            <td style="width: 100px">
                <asp:TextBox ID="IDTextBox" runat="server"></asp:TextBox></td>
            <td style="width: 254px">
                <asp:Button ID="Button1" runat="server" Text="Create Lot" />
                <asp:Button ID="Button2" runat="server" Text="Back to Make Lot Page"
                    Visible="False" Width="160px" /></td>
            <td style="width: 198px">
            </td>
        </tr>
        <tr>
            <td style="width: 100px; text-align: right">
                Run#:</td>
            <td style="width: 100px">
                <asp:TextBox ID="RunTextBox" runat="server"></asp:TextBox></td>
            <td style="width: 254px">
                Info Box:</td>
            <td style="width: 198px">
            </td>
        </tr>
        <tr>
            <td style="width: 100px; text-align: right">
                WL#</td>
            <td style="width: 100px">
                <asp:TextBox ID="WLTextBox" runat="server"></asp:TextBox></td>
            <td rowspan="2" style="width: 254px">
                <asp:TextBox ID="InfoTextBox" runat="server" Height="40px" TextMode="MultiLine" Width="248px"></asp:TextBox></td>
            <td style="width: 198px">
            </td>
        </tr>
        <tr>
            <td style="width: 100px; text-align: right">
                WL Qty:</td>
            <td style="width: 100px">
                <asp:TextBox ID="QtyTextBox" runat="server"></asp:TextBox></td>
            <td style="width: 198px">
            </td>
        </tr>
        <tr>
            <td style="width: 100px; text-align: right">
                Special Qty:</td>
            <td style="width: 100px">
                <asp:TextBox ID="SQtyTextBox" runat="server">0</asp:TextBox></td>
            <td style="width: 254px">
                <asp:CheckBox ID="CheckBox1" runat="server" Text="Use Special Qty" Width="160px" /></td>
            <td style="width: 198px">
            </td>
        </tr>
    </table>
</asp:Content>

