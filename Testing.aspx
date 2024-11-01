<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="Testing.aspx.vb" Inherits="Testing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <div>
            <asp:Button ID="Button1" runat="server" Text="Button" />

        </div>
            <asp:Panel ID="Panel1" runat="server" BackColor="#FF33CC" Visible="false">
                <br />
                <br />
                <br />
                <br />
                <br />

            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

