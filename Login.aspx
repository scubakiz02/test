<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="SatiUsers_Login" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="512px">
                <asp:Label ID="Label1" runat="server" Text="Welcome To Sati.Net "></asp:Label>&nbsp;
                <asp:Login 
                ID="Login1" 
                runat="server" 
                UserNameLabelText="SATI User Name:" 
                DisplayRememberMe="false"  
                Width="392px" 
                DestinationPageUrl="~/Main1.aspx" 
                BackColor="#F7F6F3" 
                BorderColor="#E6E2D8" 
                BorderPadding="4" 
                BorderStyle="Solid" 
                BorderWidth="1px" 
                Font-Names="Verdana" 
                Font-Size="0.8em" 
                ForeColor="#333333" 
                VisibleWhenLoggedIn="False" FailureAction="RedirectToLoginPage" ClientIDMode="AutoID" 
                >
                    <TextBoxStyle Font-Size="Large" />
                    <LoginButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"
                        Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284775"   />
                    <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                    <TitleTextStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="0.9em" ForeColor="White" />
                </asp:Login>






                <br />
                <br />






            </asp:Panel>
            &nbsp;
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>

