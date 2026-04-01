<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="NewUser.aspx.vb" Inherits="NewUser" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="536px">
                <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" BackColor="#EFF3FB" BorderColor="#B5C7DE"
                    BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em"
                    LoginCreatedUser="False" OnCreatedUser="CreateUserWizard1_CreatedUser" ContinueDestinationPageUrl="~/Main1.aspx" RequireEmail="False">
                    <SideBarStyle BackColor="#507CD1" Font-Size="0.9em" VerticalAlign="Top" />
                    <SideBarButtonStyle Font-Names="Verdana" ForeColor="White" BackColor="#507CD1" />
                    <ContinueButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid"
                        BorderWidth="1px" Font-Names="Verdana" ForeColor="#284E98" />
                    <NavigationButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid"
                        BorderWidth="1px" Font-Names="Verdana" ForeColor="#284E98" />
                    <HeaderStyle BackColor="#284E98" BorderStyle="Solid" Font-Bold="True" Font-Size="0.9em"
                        ForeColor="White" HorizontalAlign="Center" BorderColor="#EFF3FB" BorderWidth="2px" />
                    <CreateUserButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid"
                        BorderWidth="1px" Font-Names="Verdana" ForeColor="#284E98" />
                    <TitleTextStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <StepStyle Font-Size="0.8em" />
                    <WizardSteps>
                        <asp:CreateUserWizardStep runat="server" Title="Sign Up A New SATI.Net User Account">
                        </asp:CreateUserWizardStep>
                        <asp:CompleteWizardStep runat="server">
                        </asp:CompleteWizardStep>
                    </WizardSteps>
                </asp:CreateUserWizard>
                &nbsp;
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

