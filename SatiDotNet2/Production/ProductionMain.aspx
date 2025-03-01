<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ProductionMain.aspx.vb" Inherits="Production_ProductionMain" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server">
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Production Department"></asp:Label>
    <br />
    <br />
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Production/ProcessWafers.aspx" ForeColor="Blue">Process Wafers</asp:HyperLink>
    <br />    
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Production/MakeLabels.aspx" ForeColor="Blue">Make Labels</asp:HyperLink>
    <br />    
    <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/Production/T7InstanceView.aspx" ForeColor="Blue">View T7 Boxes</asp:HyperLink>
    <br />
    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Production/SPxT7DupeCompatibilityCheck.aspx" ForeColor="Blue">SPx Dupe Compatibility Check</asp:HyperLink>
    <br />
    <asp:HyperLink ID="HyperLink9" runat="server" NavigateUrl="~/Reports/GeoViewer.aspx">View Geo Data</asp:HyperLink>
    <br />
    <br />
        <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/Production/EnterProductionLog.aspx" ForeColor="Blue">Enter Production Log</asp:HyperLink>
    <br />
         <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/Production/ViewProductionLogData.aspx" ForeColor="Blue">View Production Log</asp:HyperLink>
    <br />
    </asp:Panel>
    
</asp:Content>

