<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="UnAuthorized.aspx.vb" Inherits="UnAuthorized" title="Unauthorized Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Sorry, the current user name:
    <asp:LoginName ID="LoginName1" runat="server" />
    &nbsp;is Unauthorized to View that page.<br />
    <br />
    <br />
    This user is not apart of this group: 
    <%--<asp:Label ID="GroupAuthorized" runat="server" BackColor="#ff8282" Font-Bold="true" Width="150px"></asp:Label>--%>
    <asp:Label ID="GroupAuthorized" runat="server" BackColor="#ff8282" Font-Bold="true"></asp:Label>
    <br />
    If you need access or you believe this is an error. Contact SATI.Net administrator.<br />
    <br />
</asp:Content>

