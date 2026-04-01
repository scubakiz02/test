<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="OpenTicketStatusBoard.aspx.vb" Inherits="MR_OpenTicketStatusBoard" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <meta http-equiv="refresh" content="60">

    <asp:Panel ID="Panel2" runat="server"></asp:Panel>
    <asp:UpdatePanel ID="UpdatePane" runat="server">
        <ContentTemplate>             
            <asp:Panel ID="Panel1" runat="server">

            </asp:Panel>
            <br />
        </ContentTemplate>       
    </asp:UpdatePanel>
</asp:Content>

