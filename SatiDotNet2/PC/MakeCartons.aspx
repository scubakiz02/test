<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MakeCartons.aspx.vb" Inherits="PC_MakeCartons" title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
Make A Carton
<br />
<br />
            Scan Wafer Box Label &nbsp;&nbsp; <br />
            <asp:TextBox ID="TextBoxScan" runat="server" AutoPostBack="True" BackColor="#99FFCC"></asp:TextBox> <br />
           
            
            <asp:CheckBox ID="CheckBoxReprint" runat="server" 
                Text="Reprint Carboard Box Label" />
            <br />
           
            
            <br />
            Boxes added =
            <asp:Label ID="LabelBoxQty" runat="server" Text="0"></asp:Label>
<br />
<asp:TextBox id="WaferBoxTextBox" runat="server" TextMode="MultiLine" Height="176px" 
                AutoPostBack="True" ></asp:TextBox>
<br />
<br />
<asp:DropDownList id="PrinterDropDownList" runat="server" Width="160px"><asp:ListItem>Select Printer...</asp:ListItem>
<asp:ListItem>Zebra4</asp:ListItem>
<asp:ListItem>Zebra5</asp:ListItem>
<asp:ListItem Selected="True">Zebra6</asp:ListItem>
<asp:ListItem>Zebra7</asp:ListItem>
</asp:DropDownList>
<br />
<br />

<asp:Button id="Button1" runat="server" Text="Make Carton Label" OnClick="Button1_Click"></asp:Button>

<br />

<asp:UpdateProgress id="UpdateProgress1" runat="server">
    <ProgressTemplate>
        <IMG src="../Color/Animated_LoadingBigger.gif" />Working...
    </ProgressTemplate>
</asp:UpdateProgress>
</contenttemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>

