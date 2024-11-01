<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MakeShipmentsPending.aspx.vb" Inherits="PC_MakeShipmentsPending" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="Panel1" runat="server">
        
            <asp:Label ID="Label1" runat="server" Text="Make Shipment Pending" Font-Size="Larger"></asp:Label><br />
            <br /><br />
            
        Scan Shipment Number <br />
            <asp:TextBox ID="TextBoxPickTicket" runat="server"></asp:TextBox><br /><br />
        Add a Comment:<br />
            <asp:TextBox ID="TextBoxComment" runat="server" TextMode="MultiLine" 
                Height="51px" Width="280px"></asp:TextBox><br /><br />
            <asp:Button ID="ButtonSubmit" runat="server" Text="Submit Shipment" />
        
        
        <br /><br /><br />
        </asp:Panel>
      
    </ContentTemplate>
    
    </asp:UpdatePanel>

</asp:Content>

