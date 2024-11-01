<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="FrV4.aspx.vb" Inherits="DBMaintenance_FrV4" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">    
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <asp:Button ID="Button1" runat="server" Text="Button" />
                <br />
                <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Width="1004px">
                    <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="TabPanel1">
                        <HeaderTemplate>
                            TabPanel1
                        </HeaderTemplate>
                        <ContentTemplate>
                            Panel 1
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2">
                        <ContentTemplate>
                            Panel 2<br />
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
               
                &nbsp;<br /> <br />

                           


            </asp:Panel>
        </ContentTemplate>
   </asp:UpdatePanel>
   
</asp:Content>

