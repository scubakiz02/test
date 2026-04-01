<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ShipmentReleaseView.aspx.vb" Inherits="Reports_ShipmentReleaseView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <asp:Label ID="Label1" runat="server" Text="Shipments Released" Font-Size="Larger"></asp:Label><br />
                <br />
               <table>
                    <tr>                       
                        <td>Start:</td>
                        <td>End:</td>                       
                    </tr>
                    <tr>                       
                        <td><asp:Calendar ID="CalendarStart" runat="server"></asp:Calendar></td>
                        <td><asp:Calendar ID="CalendarEnd" runat="server"></asp:Calendar></td>                        
                    </tr>                   
                </table>
                <br />
                &nbsp;&nbsp;&nbsp;Filter by Lot ID &nbsp; <asp:TextBox ID="TextBox_ID_Filter" runat="server" AutoPostBack="True"></asp:TextBox>
                &nbsp;
                &nbsp;<asp:Button ID="ButtonRefresh" runat="server" Text="Refresh" />
                <br />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                CellPadding="4" DataSourceID="SqlDataSource1" ForeColor="#333333" 
                GridLines="None" Width="994px">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>

                    <asp:BoundField DataField="Released Shipment" HeaderText="Released Shipment" 
                        SortExpression="Released Shipment" >
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LotID" HeaderText="LotID" SortExpression="LotID" ReadOnly="True" />
                    <asp:BoundField DataField="PickTicket" HeaderText="PickTicket" 
                        SortExpression="PickTicket" />
                    <asp:BoundField DataField="Qty" HeaderText="Qty" ReadOnly="True" SortExpression="Qty" />
                    <asp:BoundField DataField="Made Shipment" HeaderText="Made Shipment" SortExpression="Made Shipment" >
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
       
        
        
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                
                SelectCommand="SELECT TOP (100) PERCENT LEFT (LabelsMade.Lot, 4) AS LotID, T_ShipmentsPending.PickTicket, SUM(ShippingInventory.Total_Qty) AS Qty, T_ShipmentsPending.EventTime AS [Made Shipment], T_ShipmentsPending.ReleasedDate AS [Released Shipment] FROM T_ShipmentsPending INNER JOIN ShippingInventory ON T_ShipmentsPending.PickTicket = ShippingInventory.PickTicket INNER JOIN LabelsMade ON ShippingInventory.LotEntry = LabelsMade.LabelRecordNumber WHERE (T_ShipmentsPending.ReleasedDate &gt;= CONVERT (DATETIME, '2000-05-02 00:00:00', 102)) AND (T_ShipmentsPending.ReleasedDate &lt;= CONVERT (DATETIME, '2000-05-09 00:00:00', 102)) GROUP BY T_ShipmentsPending.PickTicket, T_ShipmentsPending.EventTime, LEFT (LabelsMade.Lot, 4), T_ShipmentsPending.ReleasedDate ORDER BY [Released Shipment], LotID, T_ShipmentsPending.PickTicket">
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
