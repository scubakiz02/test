<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="DBMaintenanceMain.aspx.vb" Inherits="DBMaintenance_DBMaintenanceMain" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server">
    <span style="font-size: 14pt"><strong>
        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Database Maintenance"></asp:Label><br />
    </strong></span>
    <br />
    Users:<br />
    <asp:HyperLink ID="HyperLink12" runat="server" NavigateUrl="~/NewUser.aspx">Make New User</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/DBMaintenance/EditAccounts.aspx">Edit User Account</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink33" runat="server" NavigateUrl="~/ModifyRoleList.aspx">Modify Roles</asp:HyperLink><br />
    <br />
    Test area:<br />
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/DBMaintenance/DevTestArea.aspx">Development Test Area</asp:HyperLink><br />
    <br />
    
    Utilities:<br />
    <asp:HyperLink ID="HyperLink16" runat="server" NavigateUrl="~/DBMaintenance/DataPackMaker.aspx">Data Pack Maker</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="~/PC/Kill_A_Lot.aspx">Kill A Lot Page</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink21" runat="server" NavigateUrl="~/MR/Tools.aspx">Tool Ids</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink22" runat="server" NavigateUrl="~/DBMaintenance/DepartmentNames.aspx">Department Names</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink19" runat="server" NavigateUrl="~/MR/MR_Group_List_Managment.aspx">MR Group Managment</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink20" runat="server" NavigateUrl="~/MR/Reports/MachineUptimeReport.aspx">MR Report</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink24" runat="server" NavigateUrl="~/DBMaintenance/MR_ToolSub_Group.aspx">MR Tool sub Groups</asp:HyperLink> &nbsp;,
    <asp:HyperLink ID="HyperLink25" runat="server" NavigateUrl="~/MR/MRT.aspx">New MRT</asp:HyperLink>&nbsp;,
    <asp:HyperLink ID="HyperLink26" runat="server" NavigateUrl="~/MR/MR_Viewer.aspx">New MR Viewer</asp:HyperLink>
    <br />
    <br />
    
    
    Tools in Devlopment<br />
    <asp:HyperLink ID="HyperLink29" runat="server" NavigateUrl="~/DBMaintenance/DiameterView.aspx">Diameter View</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink11" runat="server" NavigateUrl="~/DBMaintenance/SpecialCaseTool.aspx">Special Case Page</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/DBMaintenance/PWReportDefectClass.aspx">PWI Report Defect Class</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/CustomerMaintenance/CoreElementsMail.aspx">Core Elements</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/CustomerMaintenance/CustomerAddressEdit.aspx">Address Make Change</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/DBMaintenance/FixCBPreDataRecord.aspx">Fix Pre Data Record</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink8" runat="server" NavigateUrl="~/Production/SPxT7DupeCompatibilityCheck.aspx">SPx Dupe Checker</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink9" runat="server" NavigateUrl="~/Reports/T7Detail.aspx">T7 Detail</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink10" runat="server" NavigateUrl="~/CustomerMaintenance/PathManagment.aspx">Path Managment</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink13" runat="server" NavigateUrl="~/CustomerMaintenance/CustomersIDsSpecs.aspx"> Customer ID Spec Edit</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink14" runat="server" NavigateUrl="~/PC/ViewLots2.aspx">New View Lots</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink15" runat="server" NavigateUrl="~/Reports/GeoViewer.aspx">Geo Tool Search Viewer</asp:HyperLink><br />
    
    <asp:HyperLink ID="HyperLink17" runat="server" NavigateUrl="~/DBMaintenance/WH_Labels_and_Test.aspx">WH Label Testing</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink18" runat="server" NavigateUrl="~/ReceivingNotes.aspx">Rec notes</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink23" runat="server" NavigateUrl="~/PC/Ship.aspx">New Shipping</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink27" runat="server" NavigateUrl="~/Production/MakeSurfScanWaferBoxLabel.aspx">New Surf Labels</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink28" runat="server" NavigateUrl="~/PC/ManageShipForcastPickTickets.aspx">Manage Shipping Forcast Pick Ticket</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink30" runat="server" NavigateUrl="~/DBMaintenance/SATI_SDS.aspx">SDS Page</asp:HyperLink><br />
        <br />
        <asp:HyperLink ID="HyperLink31" runat="server" NavigateUrl="~/PC/ProLotMetalView.aspx">ProLot Metal View</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink35" runat="server" NavigateUrl="~/DBMaintenance/MDLData.aspx">MDL Data</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink32" runat="server" NavigateUrl="~/PC/CofA_MetalsPool.aspx">200mm CofA Metal pool View</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink34" runat="server" NavigateUrl="~/WI_pages/WorkInstructionViewer.aspx">Aaron wi view</asp:HyperLink><br />
        <br />
        <br />
        <asp:HyperLink ID="HyperLink36" runat="server" NavigateUrl="~/ChecklistLogging/ChecklistLoggingMainAll.aspx">Check list page</asp:HyperLink><br />
        <br />
        <br />
    </asp:Panel>
    
</asp:Content>

