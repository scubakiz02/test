<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="Email_Test.aspx.vb" Inherits="TestArea_Email_Test" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="904px">
                <br />
                Email Test:
                <br />
                TO:&nbsp; <asp:TextBox ID="TextBoxEmailAddress" runat="server" Width="200px">Tim.Hughes@purewafer.com</asp:TextBox>
                
                <asp:Button ID="Button1" runat="server" Text="Send" /><br />

                <asp:Button ID="Button2" runat="server" Text="Button" />

                <asp:Button ID="Button3" runat="server" Text="Button" />
            </asp:Panel>
            
            <asp:Button ID="Button4" runat="server" Text="add role" />
        
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

