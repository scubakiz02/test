<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="SatiDataManagment.aspx.vb" Inherits="SatiDataManagment_SatiDataManagment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Sati Data Managment"></asp:Label><br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="912px">
                <asp:Label ID="Label2" runat="server" Text="Customers and Ids"></asp:Label>
                <br />
                Customer
                <br />
                
                </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

