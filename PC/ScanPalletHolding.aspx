<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ScanPalletHolding.aspx.vb" Inherits="PC_ScanPalletHolding" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" >
               <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Scan Pallet Holding"></asp:Label><br />
               <br />
                Scan the Pallet Holding Barcodes into box below.<br />
                <asp:TextBox ID="TextboxIn" runat="server" AutoPostBack="True" Width="125px" BackColor="#66FFCC"></asp:TextBox>
                <br />                
                <br />
                Scans: <asp:Label ID="Labeladded" runat="server" Text="0"></asp:Label><br />
                <asp:ListBox ID="ListBoxCB" runat="server" Height="350px" Width="125px" AutoPostBack="True"></asp:ListBox><br />
                <br />
                <asp:Button ID="ButtonSubmit" runat="server" Text="Submit Scans" />
                <asp:HyperLink ID="HyperLink9" runat="server" Visible="False">Open Pallet Holding </asp:HyperLink>
                <br />
                <br />

                &nbsp;
            </asp:Panel>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>
