<%@ Page Title="Non-Comforming Storage Management" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="NonConformingWH.aspx.vb" Inherits="PC_NonConformingWH" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="TitlePanel" runat="server" HorizontalAlign="Center" Height="50px">
                <asp:Label ID="TitleLabel" runat="server" Text="Non-Conforming Storage Management" Font-Size="XX-Large" Font-Bold="true"></asp:Label>
            </asp:Panel>
            <asp:Panel ID="TempPanel" runat="server" Width="1008px" BackColor="White" Height="57px">
                <table>
                    <tr>
                        <td class="auto-style1" style="width: 1008px; height: 50px;"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="ManagementPanel" runat="server" BackColor="LightGray" Width="1008px">
            </asp:Panel>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>