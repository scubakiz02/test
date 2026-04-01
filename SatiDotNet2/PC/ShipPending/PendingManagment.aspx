<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="PendingManagment.aspx.vb" Inherits="PC_ShipPending_PendingManagment" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>


        <asp:Button ID="EditFakeButton" runat="server" Style="display: none" Text="spxtrig" />

       <cc1:modalpopupextender id="EditModalPopupExtender" runat="server" backgroundcssclass="modalBackground"
            dropshadow="True" popupcontrolid="EditPanel" targetcontrolid="EditFakeButton" cancelcontrolid="ButtonCancelEdit" >
        </cc1:modalpopupextender>

        <asp:Panel ID="EditPanel" runat="server" BackColor="#80BEFD" BorderColor="#80BEFD" BorderWidth="20px">
            <br />
            Pick Ticket:
            <asp:Label ID="LabelPickTicket" runat="server" Text="Label"></asp:Label>
            <br />

            <br />

            <asp:Panel ID="Panel2" runat="server" HorizontalAlign="Right">
                <asp:Label ID="Label2" runat="server" Text="Tracking Number Required" ForeColor="Red" Font-Size="Small"></asp:Label>
            </asp:Panel>

            <table>
                <tr>
                    <td>&nbsp;Tracking# : </td>
                    <td>&nbsp;<asp:TextBox ID="TextBoxTrackingNumber" runat="server" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;Carrier :</td>
                    <td>&nbsp;<asp:DropDownList ID="DropDownListCarrier" runat="server" Width="208px" DataSourceID="SqlDataSourceCarriers" DataTextField="Name" DataValueField="Name"></asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourceCarriers" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT Name FROM Carriers ORDER BY Name"></asp:SqlDataSource>
                    </td>
                </tr>
            </table>
            <br />
            <br />

            <table style="text-align: center; width: 300px;">
                <tr>
                    <td>
                        <asp:Button ID="ButtonSave" runat="server" Text="Save" /></td>
                    <td>
                        <asp:Button ID="ButtonSaveRealease" runat="server" Text="Save & Release" /></td>
                    <td>
                        <asp:Button ID="ButtonCancelEdit" runat="server" Text="Cancel" /></td>
                </tr>
            </table>

            <br />
        </asp:Panel>

        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <img src="../Color/Animated_LoadingBigger.gif" />Working...
            </ProgressTemplate>
        </asp:UpdateProgress>

        <asp:Panel ID="Panel1" runat="server">


            <table class="style1">
                <tr>
                    <td align="left" valign="top">&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Pending Shipments" Font-Size="Larger"></asp:Label><br />
                        <br />
                        Scan Pick Ticket to release or Click the "Release" buttons below.<br />
                        <br />
                         Scan -----><asp:TextBox ID="TextBoxScan" runat="server"></asp:TextBox> <----- Scan<br />
                    </td>
                    <td>&nbsp;Scan Info:<br />

                        <asp:TextBox ID="TextBoxScanInfo" runat="server" TextMode="MultiLine" Height="112px" Width="344px"></asp:TextBox>

                    </td>
                </tr>
            </table>
            
           <%--DataSourceID="SqlDataSource1"--%> 
            <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" AutoGenerateColumns="False" 
                CellPadding="4" ForeColor="#333333" 
                GridLines="None" Width="994px">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="PickTicket" HeaderText="PickTicket" 
                        SortExpression="PickTicket" />
                    <asp:BoundField DataField="Made" HeaderText="Made" SortExpression="Made" />
                    <asp:BoundField DataField="LotID" HeaderText="LotID" SortExpression="LotID" />
                    <asp:BoundField DataField="Qty" HeaderText="Qty" ReadOnly="True" 
                        SortExpression="Qty" />
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                    <asp:ButtonField ButtonType="Button" CommandName="EditShip" Text="Edit Shipping" />
                    <asp:BoundField DataField="Tracking" HeaderText="Tracking" SortExpression="Tracking" NullDisplayText="0" />
                    <asp:BoundField DataField="Carrier" HeaderText="Carrier" SortExpression="Carrier" />
                    <asp:ButtonField ButtonType="Button" CommandName="Release" Text="Release" />
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>



            <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                SelectCommand="SELECT TOP (100) PERCENT T_ShipmentsPending.PickTicket, ShippingUnit.ShippingID, ShippingUnit.Pallet_Key, T_ShipmentsPending.EventTime AS Made, LEFT (LabelsMade.Lot, 4) AS LotID, SUM(ShippingInventory.Total_Qty) AS Qty, T_ShipmentsPending.Notes, T_ShipmentsPending.[Key] AS Pend#, ShippingUnit.Tracking, Shipping_Log.Carrier, Shipping_Log.CustomerShippingAccount, Shipping_Log.ExsilFrieght FROM Pick_ShippingUnit INNER JOIN ShippingUnit ON Pick_ShippingUnit.Pallet_Key = ShippingUnit.Pallet_Key INNER JOIN T_ShipmentsPending INNER JOIN ShippingInventory ON T_ShipmentsPending.PickTicket = ShippingInventory.PickTicket INNER JOIN LabelsMade ON ShippingInventory.LotEntry = LabelsMade.LabelRecordNumber ON Pick_ShippingUnit.PickTicket = T_ShipmentsPending.PickTicket INNER JOIN Shipping_Log ON ShippingUnit.ShippingID = Shipping_Log.ShippingID WHERE (T_ShipmentsPending.Released = N'No') GROUP BY T_ShipmentsPending.PickTicket, T_ShipmentsPending.EventTime, T_ShipmentsPending.Notes, LEFT (LabelsMade.Lot, 4), T_ShipmentsPending.[Key], ShippingUnit.Tracking, Shipping_Log.Carrier, Shipping_Log.CustomerShippingAccount, Shipping_Log.ExsilFrieght, ShippingUnit.ShippingID, ShippingUnit.Pallet_Key ORDER BY LotID, T_ShipmentsPending.PickTicket">
            </asp:SqlDataSource>

            <br />
            <br />
            <br />
        </asp:Panel>
      
       
       


    </ContentTemplate>
    
    </asp:UpdatePanel>
</asp:Content>

