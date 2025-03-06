<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MakeSwanseaCofA.aspx.vb" Inherits="TestArea_MakeSwanseaCofA" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table >
            <caption>
                <br />
                <asp:Button ID="Button1" runat="server" Text="Make The CofA" />
                <br />
            </caption>
        </table>      
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

