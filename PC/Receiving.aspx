<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="Receiving.aspx.vb" Inherits="Receiving" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/PC/ReceiveWafers.aspx"
        Style="z-index: 100; left: 24px; position: absolute; top: 224px">Receive Wafers</asp:HyperLink>
    <asp:HyperLink ID="HyperLink2" runat="server" Style="z-index: 101; left: 312px; position: absolute;
        top: 224px">Add or Adjust Wafer Log</asp:HyperLink>
    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/PC/WHInventory.aspx" Style="z-index: 102;
        left: 24px; position: absolute; top: 248px" Width="144px">Warehouse Inventory </asp:HyperLink>
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="14pt" Style="z-index: 103;
        left: 24px; position: absolute; top: 200px" Text="Receving Menu"></asp:Label>
    <asp:HyperLink ID="HyperLink4" runat="server" Style="z-index: 105; left: 176px; position: absolute;
        top: 224px">Adjust Wafer Log</asp:HyperLink>
</asp:Content>

