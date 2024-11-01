<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="WH_Labels_and_Test.aspx.vb" Inherits="DBMaintenance_WH_Labels_and_Test" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">    

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                WH Labels&nbsp; <br \><br \>
                <asp:Panel ID="Panel2" runat="server">
                Test WH Labels<br \>
                    <asp:DropDownList ID="DropDownListPrinterlist" runat="server">
                    <asp:ListItem>Zebra4</asp:ListItem>
                    <asp:ListItem Selected="True">Zebra6</asp:ListItem>
                        <asp:ListItem>IT</asp:ListItem>
                    </asp:DropDownList><br \>&nbsp; 
                    <asp:Button ID="ButtonTestNC_Box" runat="server" Text="Test NC Box" />&nbsp; &nbsp; 
                    <asp:Button ID="ButtonTestNC_Pallet" runat="server" Text="Test NC Pallet" />&nbsp; &nbsp; 
                </asp:Panel>
             
            </asp:Panel>
                        
        </ContentTemplate>
    </asp:UpdatePanel>        
</asp:Content>
