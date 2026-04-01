<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="SAR_Main.aspx.vb" Inherits="Reports_SAR_SAR_Main" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <strong>
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Sati Archive Reports"></asp:Label><br />
    </strong>
    <br />
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Reports/SAR/CreateSAR.aspx">Compile SAR</asp:HyperLink><br />
    <br />
    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Reports/SAR/CreateSARMain.aspx">Close Month</asp:HyperLink><br />
    &nbsp;
    <br />
</asp:Content>

