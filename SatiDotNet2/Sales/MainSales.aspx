<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MainSales.aspx.vb" Inherits="Sales_MainSales" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server">
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Sales Area"></asp:Label><br />
        <br />       
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Sales/PO_SO_Managment.aspx">SO Managment</asp:HyperLink><br />
        <br />
         <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Sales/EnterFuturePO.aspx">Future SO Managment</asp:HyperLink><br />
        <br />
        <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Sales/SO_Release.aspx">Release SO</asp:HyperLink><br />
        <br />
    </asp:Panel>
   
</asp:Content>

