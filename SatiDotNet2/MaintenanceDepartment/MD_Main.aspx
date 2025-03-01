<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MD_Main.aspx.vb" Inherits="MaintenanceDepartment_MD_Main" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server">
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Maintenance Department"></asp:Label><br />
    <br />
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/MR/MR_Viewer.aspx">Maintenance Requests</asp:HyperLink><br />
        <br />
    <asp:HyperLink ID="HyperLink3" runat="server">Maintenance Request Reports</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink4" runat="server">Make PM request</asp:HyperLink><br />
    <br />
    <br />
    </asp:Panel>
    
</asp:Content>