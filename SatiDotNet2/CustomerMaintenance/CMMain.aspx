<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="CMMain.aspx.vb" Inherits="CustomerMaintenance_CMMain" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server">
    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Customer Maintenance:"></asp:Label><br />
    <br />
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Large" Text="Main ID:"></asp:Label><br />
    <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/CustomerMaintenance/IDSplits.aspx">ID Transfers</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/CustomerMaintenance/ID_DefectMaintenance.aspx">ID Defects</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/CustomerMaintenance/TestLabel.aspx">ID Test Label</asp:HyperLink><br />
    <br />    
    
    <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="Large" Text="PO/SO:"></asp:Label><br />
    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Sales/SO_Release.aspx">Release SO</asp:HyperLink><br />
    <br />
    <br />
    <br />
    </asp:Panel>
    
</asp:Content>

