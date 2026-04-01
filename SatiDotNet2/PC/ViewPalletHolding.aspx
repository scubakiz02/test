<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ViewPalletHolding.aspx.vb" Inherits="PC_ViewPalletHolding" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" >
               <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="View Pallet Holding"></asp:Label><br />
               <br />
                <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1" DataTextField="ScanKey" DataValueField="ScanKey" Width="257px" AutoPostBack="True">
                </asp:DropDownList><asp:Button ID="ButtonLoad" runat="server" Text="Load" />
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT ScanKey FROM T_PH_DayScans GROUP BY ScanKey ORDER BY ScanKey DESC"></asp:SqlDataSource>
                <br />
                <asp:RadioButton ID="RadioButtonID" runat="server" GroupName="MySort" AutoPostBack="True" Text="By ID" Checked="True" />
                <asp:RadioButton ID="RadioButtonLot" runat="server" GroupName="MySort" AutoPostBack="True" Text="By Lot" />
                <asp:RadioButton ID="RadioButtonCustomer" runat="server" GroupName="MySort" AutoPostBack="True" Text="By Customer" />
                <br />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSourceMyData" ForeColor="#333333" GridLines="None" Width="693px">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="CustomerID" HeaderText="CustomerID" SortExpression="CustomerID" ControlStyle-BorderStyle="NotSet" >
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ID/Lot" HeaderText="ID/Lot" ReadOnly="True" SortExpression="ID/Lot" >
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QTY" HeaderText="QTY" SortExpression="QTY" ReadOnly="True" >
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SO" HeaderText="SO" SortExpression="SO" >
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PO_Number" HeaderText="PO_Number" SortExpression="PO_Number">
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                    </Columns>
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                </asp:GridView>

                <asp:SqlDataSource ID="SqlDataSourceMyData" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT TOP (100) PERCENT dbo.MainID.CustomerID, LEFT(dbo.LabelsMade.Lot, 4) AS [ID/Lot], SUM(dbo.LabelsMade.Wafers) AS QTY, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number FROM dbo.LabelsMade INNER JOIN dbo.ShippingInventory ON dbo.LabelsMade.LabelRecordNumber = dbo.ShippingInventory.LotEntry INNER JOIN dbo.T_PH_Table INNER JOIN dbo.T_PH_DayScans ON dbo.T_PH_Table.PH_Key = dbo.T_PH_DayScans.PH_Key ON dbo.ShippingInventory.Carton_Key = dbo.T_PH_Table.CB INNER JOIN dbo.SO_LineItems ON dbo.LabelsMade.SO_Key = dbo.SO_LineItems.[Key] INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO INNER JOIN dbo.MainID ON LEFT(dbo.LabelsMade.Lot, 4) = dbo.MainID.MainID WHERE (dbo.T_PH_DayScans.ScanKey = CONVERT(DATETIME, '2016-07-28', 102)) GROUP BY LEFT(dbo.LabelsMade.Lot, 4), dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number, dbo.MainID.CustomerID ORDER BY [ID/Lot]"></asp:SqlDataSource>

                &nbsp;
            </asp:Panel>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>