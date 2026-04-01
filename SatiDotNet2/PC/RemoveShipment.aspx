<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="RemoveShipment.aspx.vb" Inherits="PC_RemoveShipment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <asp:Label ID="Label1" runat="server" Text="Remove Shipment" Font-Size="Larger"></asp:Label><br />
                <br />
                
                Pressing "Delete" below will remove the shipment and return the inventory to FGI.<br />
                <br />
                 <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                CellPadding="4" DataSourceID="SqlDataSource1" ForeColor="#333333" 
                GridLines="None" Width="994px">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:ButtonField ButtonType="Button" CommandName="Remove" Text="Delete" />
                    <asp:BoundField DataField="PickTicket" HeaderText="PickTicket" 
                        SortExpression="PickTicket" />
                    <asp:BoundField DataField="Made" HeaderText="Made" SortExpression="Made" />
                    <asp:BoundField DataField="LotID" HeaderText="LotID" SortExpression="LotID" />
                    <asp:BoundField DataField="Qty" HeaderText="Qty" ReadOnly="True" 
                        SortExpression="Qty" />
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                    <asp:BoundField DataField="Pend#" HeaderText="Pend#" InsertVisible="False" 
                        ReadOnly="True" SortExpression="Pend#" />
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
       
        
        
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                
                SelectCommand="SELECT dbo.T_ShipmentsPending.PickTicket, dbo.T_ShipmentsPending.EventTime AS Made, LEFT (dbo.LabelsMade.Lot, 4) AS LotID, SUM(dbo.ShippingInventory.Total_Qty) AS Qty, dbo.T_ShipmentsPending.Notes, dbo.T_ShipmentsPending.[Key] AS Pend# FROM dbo.T_ShipmentsPending INNER JOIN dbo.ShippingInventory ON dbo.T_ShipmentsPending.PickTicket = dbo.ShippingInventory.PickTicket INNER JOIN dbo.LabelsMade ON dbo.ShippingInventory.LotEntry = dbo.LabelsMade.LabelRecordNumber WHERE (dbo.T_ShipmentsPending.Released = N'No') GROUP BY dbo.T_ShipmentsPending.PickTicket, dbo.T_ShipmentsPending.EventTime, dbo.T_ShipmentsPending.Notes, LEFT (dbo.LabelsMade.Lot, 4), dbo.T_ShipmentsPending.[Key] ORDER BY LEFT (dbo.LabelsMade.Lot, 4), dbo.T_ShipmentsPending.PickTicket">
            </asp:SqlDataSource>
       
                &nbsp;<br />
                <br />
                <br />
                <br />
                &nbsp;</asp:Panel>
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
