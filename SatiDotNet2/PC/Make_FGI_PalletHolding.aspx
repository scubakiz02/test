<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="Make_FGI_PalletHolding.aspx.vb" Inherits="PC_Make_FGI_PalletHolding" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="904px">
              <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Make FGI Pallet Holding"></asp:Label><br />
               <br />
                To Make a Pallet Holding.<br />
                Scan the Carton Boxes into box below.<br />
                <asp:TextBox ID="TextBoxCBIn" runat="server" AutoPostBack="True" Width="125px" BackColor="#66FFCC"></asp:TextBox>
                <br /><br />
                <asp:Label ID="LabelCartonSet" runat="server" Text="Carton Type"></asp:Label>
                <br />
                Cartons: <asp:Label ID="LabelCartonsadded" runat="server" Text="0"></asp:Label><br />
                <asp:ListBox ID="ListBoxCB" runat="server" Height="350px" Width="125px" AutoPostBack="True"></asp:ListBox><br />
                <br />
                <asp:Button ID="ButtonMakePalletHolding" runat="server" Text="Make Pallet Holding" />
                <asp:HyperLink ID="HyperLink9" runat="server" Visible="False">Open Pallet Holding </asp:HyperLink>
                <br />
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
  
</asp:Content>
