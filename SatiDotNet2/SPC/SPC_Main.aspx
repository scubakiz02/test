<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="SPC_Main.aspx.vb" Inherits="SPC_SPC_Main" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server">
        <asp:Label ID="Label1" runat="server" Text="SATI.SPC" Font-Bold="True" Font-Size="X-Large"></asp:Label><br />
        <br />
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/SPC/SPC_Compile.aspx">SPC Compile</asp:HyperLink><br />
        <br />
        <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/SPC/SPC_View.aspx">SPC Viewer</asp:HyperLink><br />
        <br />
        <br />
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/SPC/SPC_Managment.aspx">SPC Managment</asp:HyperLink><br />
        <br />
        <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/SPC/SPC_Record_Maintenance.aspx">SPC Record Maintenance</asp:HyperLink><br />
        <br />
    </asp:Panel>
</asp:Content>

