<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MD_Main.aspx.vb" Inherits="MaintenanceDepartment_MD_Main" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server">
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="IT Department"></asp:Label><br />
    <br />
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/TR/MR_Viewer.aspx">IT Requests</asp:HyperLink><br />
    </asp:Panel>
    
</asp:Content>