<%@ Page Language="C#" MasterPageFile="~/MasterPage1.master" AutoEventWireup="true" CodeFile="ToolDataMain.aspx.cs" Inherits="ToolData_ToolDataMain" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Tool Data Viewer"></asp:Label><br />
    <br />
    <asp:HyperLink ID="HyperLink2" runat="server">ADE</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink5" runat="server">CR-81</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ToolData/HologenixData.aspx">Hologenix</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink7" runat="server">ICP-MS</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink3" runat="server">SPx</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink4" runat="server">Tencor</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink6" runat="server">Titrator</asp:HyperLink><br />
</asp:Content>

