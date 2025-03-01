<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="TestLabel.aspx.vb" Inherits="DBMaintenance_TestLabel" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
            Test Label Area<br />
            <br />
            LotNumber<BR />
            &nbsp;&nbsp;&nbsp;<asp:TextBox id="LotNumberTextBox" runat="server" Width="128px"></asp:TextBox>
            &nbsp;Qty:&nbsp;
            <asp:TextBox ID="TextBoxQty" runat="server" Width="43px">25</asp:TextBox>
            <br />
            <asp:CheckBox ID="CheckBoxReal" runat="server" Text="Record Label In System." />
            &nbsp;<br />
            <BR />
            <asp:DropDownList id="PrinterDropDownList" runat="server">
                    <asp:ListItem>Select Printer...</asp:ListItem>
                    <asp:ListItem>Zebra1</asp:ListItem>
                    <asp:ListItem>Zebra2</asp:ListItem>
                    <asp:ListItem>Zebra_2B</asp:ListItem>
                    <asp:ListItem>Zebra3</asp:ListItem>
                    <asp:ListItem>Zebra4</asp:ListItem>
                    <asp:ListItem>Zebra5</asp:ListItem> 
                    <asp:ListItem>Zebra6</asp:ListItem> 
                    <asp:ListItem>Zebra9</asp:ListItem>                   
                    <asp:ListItem Selected="True">HP LaserJet M506 Supervisors</asp:ListItem>
            </asp:DropDownList>
            <BR />
            &nbsp;
            <br />
            <asp:CheckBox ID="WBCheckBox" runat="server" Text="Wafer Box" /><br />
            <asp:CheckBox ID="CBCheckBox" runat="server" Text="Shipping" /><br />
            <asp:CheckBox ID="ADCheckBox" runat="server" Text="Address" /><br />
            <asp:CheckBox ID="InfoPadCheckBox" runat="server" Text="InfoPad" /><br />
            <br />
            <asp:Button id="Button2" runat="server" Text="Test Print"></asp:Button>
            <br />
</contenttemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress id="UpdateProgress1" runat="server">
        <progresstemplate>
<IMG src="../Color/Animated_LoadingBigger.gif" /> Working...
</progresstemplate>
    </asp:UpdateProgress>
    <br />
    &nbsp;<br />
</asp:Content>

