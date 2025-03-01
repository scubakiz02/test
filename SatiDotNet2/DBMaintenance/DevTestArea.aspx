<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="DevTestArea.aspx.vb" Inherits="DBMaintenance_DevTestArea" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/TestArea/TestWhiteBlackListScribes.aspx">IBM 48xxxxxxxx</asp:HyperLink><br />
    <br />
    <asp:HyperLink ID="HyperLink9" runat="server" NavigateUrl="~/Production/ProcessLot.aspx">New Lot Process Area</asp:HyperLink> <br />
    <br />
    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/TestArea/Test300mmMetals.aspx">Test 300mm Metals</asp:HyperLink><br />
    <br />
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/TestArea/NewDBtest.aspx">New SQL 2012</asp:HyperLink><br />
    <br />
    <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/DBMaintenance/QuickTestLogic.aspx">Quick Test Logic</asp:HyperLink><br />
    <br />
    <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/TestArea/Email_Test.aspx">New Email Test</asp:HyperLink><br />
    <br />
    <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/TestArea/metal.aspx">Metal</asp:HyperLink><br />
    <br />
     <asp:Button ID="Button12" runat="server" Text="Test Flex for xlsX" /><br />



    <asp:TextBox ID="TextBoxLotNumber" runat="server"></asp:TextBox>
    <asp:Button ID="ButtonTest200QA" runat="server" Text="Test 200QA" /><br />
    <asp:Label ID="LabelQAFeedback" runat="server" Text=""></asp:Label>
</asp:Content>

