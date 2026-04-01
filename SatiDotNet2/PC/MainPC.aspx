<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MainPC.aspx.vb" Inherits="PC1" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server">
        <asp:Label ID="Label1" runat="server" Text="Production Control" Font-Size="X-Large" Font-Bold="True"></asp:Label>
        <br />
        <br />
        <asp:HyperLink ID="HyperLink2" runat="server" 
            NavigateUrl="~/PC/ReceiveWafers.aspx" ForeColor="Blue">Receive Wafers</asp:HyperLink>
        <br />
        <asp:HyperLink ID="HyperLink9" runat="server" 
            NavigateUrl="~/PC/WHReceivingHistory.aspx" ForeColor="Blue">Receiving Log</asp:HyperLink>
        <br />
        <br />
        <asp:Label ID="Label6" runat="server" Text="NC..." Font-Size="Larger"></asp:Label>
        <br />
        <asp:HyperLink ID="HyperLink13" runat="server" NavigateUrl="~/PC/NonConformingPacking.aspx">NC Packing</asp:HyperLink><br />        
        <asp:HyperLink ID="HyperLink19" runat="server" NavigateUrl="~/PC/NonConformingSkidBuilder.aspx">NC Skid Builder</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink17" runat="server" NavigateUrl="~/PC/NonConformingManagment.aspx">NC Managment</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink18" runat="server" NavigateUrl="~/PC/NonConformingWH.aspx" Visible="False">NC Storage Managment</asp:HyperLink>
        <br />

        <asp:Label ID="Label2" runat="server" Text="Manage Lots..." Font-Size="Larger"></asp:Label>
        <br />
        <asp:HyperLink ID="HyperLink1" runat="server" 
            NavigateUrl="~/PC/MakeFirstPassLot.aspx" ForeColor="Blue">Make First Pass Lot</asp:HyperLink><br />
        Make Rework Lot
        <asp:HyperLink ID="HyperLink3" runat="server" 
            NavigateUrl="~/PC/MakeReworkLot.aspx" ForeColor="Blue">Old Style</asp:HyperLink>&nbsp;
        Or&nbsp;
        <asp:HyperLink ID="HyperLink14" runat="server" 
            NavigateUrl="~/PC/MakeReworkLots.aspx" ForeColor="Blue">New Style</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink15" runat="server" 
            NavigateUrl="~/PC/MergeLots.aspx" ForeColor="Blue">Merge Lots</asp:HyperLink><br />
        <br />
        <asp:Label ID="Label3" runat="server" Text="Inventory..." Font-Size="Larger"></asp:Label><br />
        
        <asp:HyperLink ID="HyperLink4" runat="server" 
            NavigateUrl="~/PC/WHInventory.aspx" ForeColor="Blue">View Warehouse Inv</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/PC/ViewLots2.aspx" 
            ForeColor="Blue">View Lots in Process</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink6" runat="server" 
            NavigateUrl="~/PC/ReworkInvAdj.aspx" ForeColor="Blue">View & Adjust Rework Inv</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink7" runat="server" 
            NavigateUrl="~/PC/Partial Inv.aspx" ForeColor="Blue">View Partial Inv</asp:HyperLink><br />

        <asp:HyperLink ID="HyperLink8" runat="server" 
            NavigateUrl="~/DBMaintenance/FGICheck.aspx" ForeColor="Blue">View FGI & Check</asp:HyperLink>
        &nbsp;&nbsp;
        <asp:HyperLink ID="HyperLink25" runat="server" 
            NavigateUrl="~/DBMaintenance/FGI_View.aspx" ForeColor="Blue">(New) View FGI W/details</asp:HyperLink>
        <br />

        <asp:HyperLink ID="HyperLink16" runat="server" 
            NavigateUrl="~/PC/FGICartonDetail.aspx" ForeColor="Blue">View Wafer Box or Carton Detail</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink21" runat="server" 
            NavigateUrl="~/PC/Make_FGI_PalletHolding.aspx" ForeColor="Blue">Make Pallet Holding</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink22" runat="server" 
            NavigateUrl="~/PC/ScanPalletHolding.aspx" ForeColor="Blue">Scan Pallet Holding</asp:HyperLink><br />
         <asp:HyperLink ID="HyperLink23" runat="server" 
            NavigateUrl="~/PC/ViewPalletHolding.aspx" ForeColor="Blue">View Pallet Holding</asp:HyperLink><br />
        <br />
        <asp:Label ID="Label4" runat="server" Text="FGI Cartons..." Font-Size="Larger"></asp:Label><br />
        
       <%-- <asp:HyperLink ID="HyperLink10" runat="server" 
            NavigateUrl="~/PC/MakeCartons.aspx" ForeColor="Blue">Make Carton</asp:HyperLink><br />--%>
        <asp:HyperLink ID="HyperLink24" runat="server" 
            NavigateUrl="~/PC/MakeCartonNew.aspx" ForeColor="Blue">Make Carton </asp:HyperLink><br />
        <br />
        <asp:Label ID="Label5" runat="server" Text="Shippments..." Font-Size="Larger"></asp:Label><br />
        
        <asp:HyperLink ID="HyperLink11" runat="server" 
            NavigateUrl="~/PC/MakePickTicket.aspx" ForeColor="Blue">Make Pick Ticket</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink12" runat="server" 
            NavigateUrl="~/PC/MakeShipment.aspx" ForeColor="Blue">Make Shipment</asp:HyperLink><br />
        <br />
        <br />
        
        <asp:HyperLink ID="HyperLink20" runat="server" 
            NavigateUrl="~/PC/RemoveShipment.aspx" ForeColor="Blue">Remove Shipment</asp:HyperLink><br />
        <br />
        <br />
        <asp:HyperLink ID="HyperLink26" runat="server" 
            NavigateUrl="~/PC/WafertoryMove.aspx" ForeColor="Blue">Wafertory</asp:HyperLink><br />
        <br />
    </asp:Panel>
    
</asp:Content>

