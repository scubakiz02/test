<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="Main1.aspx.vb" Inherits="Main1" title="Main Page" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server">
        <asp:Label ID="Label1" runat="server" Text="Main Menu" Font-Bold="True" Font-Size="X-Large"></asp:Label> <br />       
        <br />
        <asp:HyperLink ID="HyperLink06" runat="server" ForeColor="Blue" NavigateUrl="~/Production/ProductionMain.aspx">Production</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink03" runat="server" ForeColor="Blue" NavigateUrl="~/PC/MainPC.aspx">Production Control</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink01" runat="server" ForeColor="Blue" NavigateUrl="~/Reports/Reports.aspx">Reports</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink02" runat="server" ForeColor="Blue" NavigateUrl="~/CustomerMaintenance/CMMain.aspx">Customer Maintenance</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink04" runat="server" ForeColor="Blue" NavigateUrl="~/DBMaintenance/DBMaintenanceMain.aspx">DB Maintenance</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink05" runat="server" ForeColor="Blue" NavigateUrl="~/Sales/MainSales.aspx">Sales Maintenance</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink07" runat="server" ForeColor="Blue" NavigateUrl="~/MaintenanceDepartment/MD_Main.aspx">Maintenance Department</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink10" runat="server" ForeColor="Blue" NavigateUrl="~/SPC/SPC_Main.aspx">SPC</asp:HyperLink><br /> 
         
        <asp:HyperLink ID="HyperLink09" runat="server" ForeColor="Blue" Visible="False" NavigateUrl="~/InfoDirectory/InfoDirectoryMain.aspx">Info Directory</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink08" runat="server" ForeColor="Blue" Visible="False" NavigateUrl="~/ToolData/ToolDataMain.aspx">Tool Data</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink11" runat="server" ForeColor="Blue" Visible="False" NavigateUrl="~/SatiDataManagment/SatiDataManagment.aspx">New Area</asp:HyperLink>
        <br /> 
        <br />
    </asp:Panel>   
    
</asp:Content>

