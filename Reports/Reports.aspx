<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="Reports.aspx.vb" Inherits="Reports" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server">
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Reports"></asp:Label>
    <br />
        
    <br />
    <asp:HyperLink ID="HyperLink1" runat="server" 
        NavigateUrl="~/Reports/ProcessInvReport.aspx" ForeColor="Blue">In Process Sum By Stage</asp:HyperLink>
    <br />
    <br />
    <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/PC/ViewLots2.aspx" 
        ForeColor="Blue">View Lots In Process</asp:HyperLink>
    <br />
    <br />
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Reports/PendingShipments.aspx" 
        ForeColor="Blue">(old) View Pending Shipments</asp:HyperLink>
        &nbsp;
        /
        &nbsp;
        <asp:HyperLink ID="HyperLink17" runat="server" ForeColor="Blue" NavigateUrl="~/PC/ShipPending/PendingManagment.aspx">Pending Shipments Managment</asp:HyperLink>
         &nbsp;
        /
        &nbsp;
        <asp:HyperLink ID="HyperLink18" runat="server" ForeColor="Blue" NavigateUrl="~/Reports/ShipmentReleaseView.aspx">Shipments Released</asp:HyperLink>
        <br />
        <br />
        Inventory Summary ( Excel
    <asp:HyperLink ID="HyperLink5" runat="server" 
            NavigateUrl="~/Reports/ReportFiles/Inv Sati Summary.xls" ForeColor="Blue">View</asp:HyperLink>
    )&nbsp; or &nbsp;( Web
    <asp:HyperLink ID="HyperLink7" runat="server" 
            NavigateUrl="~/Reports/InventoryWebSummary.aspx" ForeColor="Blue">View</asp:HyperLink>
    )&nbsp; or &nbsp;(SB Micro Print
    <asp:HyperLink ID="HyperLink8" runat="server" 
            NavigateUrl="~/Reports/ReportFiles/Inv Sati SummaryMicroSB.xls" ForeColor="Blue">View</asp:HyperLink>
        )&nbsp; or&nbsp; (SJ Filter Export
        <asp:HyperLink ID="HyperLink16" runat="server" 
            NavigateUrl="~/Reports/InvFilterExport.aspx">View</asp:HyperLink>
        )<br />
    <br />
    <asp:HyperLink ID="HyperLink6" runat="server" 
        NavigateUrl="~/Reports/ReportFiles/Returned Yield By ID.xls" ForeColor="Blue">Returned Yield By ID By Date</asp:HyperLink>
    <br />
    <br />
    <asp:HyperLink ID="HyperLink9" runat="server" 
        NavigateUrl="~/Reports/SAR/SAR_Main.aspx" ForeColor="Blue">Sati Archive Report's</asp:HyperLink>
    <br />
    <br />
    <asp:HyperLink ID="HyperLink3" runat="server" 
        NavigateUrl="~/Reports/DayArchive.aspx" ForeColor="Blue">History Inv Slice</asp:HyperLink>
    <br />
    <br />
        <asp:HyperLink ID="HyperLink14" runat="server" 
        NavigateUrl="~/Reports/SurfScan.aspx" ForeColor="Blue">SPx Current Bin Fall</asp:HyperLink>
    <br />
    <br />
    <asp:HyperLink ID="HyperLink11" runat="server" 
        NavigateUrl="~/Reports/T7Detail.aspx" ForeColor="Blue">T7 Detail Report</asp:HyperLink>
    <br />
    <br />
    <asp:HyperLink ID="HyperLink12" runat="server" 
        NavigateUrl="~/PC/FGICartonDetail.aspx" ForeColor="Blue">FGI Carton Detail</asp:HyperLink>
    <br />
    <br />
    <asp:HyperLink ID="HyperLink13" runat="server" 
        NavigateUrl="~/Production/T7InstanceView.aspx" ForeColor="Blue">T7 Instance & Wafer Box View</asp:HyperLink>
    <br />
    <br />
    <asp:HyperLink ID="HyperLink15" runat="server" NavigateUrl="~/Reports/GeoViewer.aspx">View Geo Data</asp:HyperLink>
    <br />
    <br />  
    <asp:HyperLink ID="HyperLink10" runat="server" NavigateUrl="~/Reports/KPI.aspx">Make KPI</asp:HyperLink>
    <br />
    <br /> 
    <asp:HyperLink ID="HyperLink19" runat="server" NavigateUrl="~/Reports/WipX_Report.aspx">WipX Report</asp:HyperLink>
    <br />
    <br />      
    <asp:HyperLink ID="HyperLink20" runat="server" NavigateUrl="~/Reports/Defects_ByIDs_ByDateRange.aspx">Defects by ID</asp:HyperLink>
    <br />
    <br /> 
    <asp:HyperLink ID="HyperLink21" runat="server" NavigateUrl="~/Reports/Out.aspx">Surf Scan Out's</asp:HyperLink>
    <br />
    <br /> 
        <asp:HyperLink ID="HyperLink22" runat="server" NavigateUrl="~/Reports/Spec-Surf-CMP.aspx">Spec - Surf - CMP View</asp:HyperLink>
    <br />
    <br /> 
        <asp:HyperLink ID="HyperLink30" runat="server" NavigateUrl="~/DBMaintenance/SATI_SDS.aspx">SDS Page</asp:HyperLink><br />
    </asp:Panel>
    
</asp:Content>

