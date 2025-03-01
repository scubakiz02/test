<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="FGI_View.aspx.vb" Inherits="DBMaintenance_FGI_View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>View FGI</h2>
            <p>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SystemFGISqlDataSource"
                Width="424px" CellPadding="3" ForeColor="Black" AllowSorting="True" DataKeyNames="ID" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" >
                <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                        <asp:BoundField DataField="Diameter" HeaderText="Diameter" SortExpression="Diameter" >
                        <HeaderStyle Wrap="False" />
                        <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" ReadOnly="True" >
                        <HeaderStyle Wrap="False" />
                        <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CustomerID" HeaderText="CustomerID" SortExpression="CustomerID" >
                        <HeaderStyle Wrap="False" />
                        <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FGI" HeaderText="FGI" SortExpression="FGI" ReadOnly="True" >
                        <HeaderStyle Wrap="False" />
                        <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SO" HeaderText="SO" SortExpression="SO" >
                        <HeaderStyle Wrap="False" />
                        <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PO" HeaderText="PO" SortExpression="PO" >
                        <HeaderStyle Wrap="False" />
                        <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Part#" HeaderText="Part#" SortExpression="Part#" >
                        <HeaderStyle Wrap="False" />
                        <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="P Rev" HeaderText="P Rev" SortExpression="P Rev" >
                        <HeaderStyle Wrap="False" />
                        <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Spec#" HeaderText="Spec#" SortExpression="Spec#" >
                        <HeaderStyle Wrap="False" />
                        <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S Rev" HeaderText="S Rev" SortExpression="S Rev" >
                        <HeaderStyle Wrap="False" />
                        <ItemStyle Wrap="False" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" />
                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#808080" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#383838" />
                </asp:GridView>
            </p>               
            <asp:SqlDataSource ID="SystemFGISqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                SelectCommand="SELECT TOP (100) PERCENT MainID.Diameter, MainID.MainID AS ID, MainID.CustomerID, SUM(ShippingInventory.Total_Qty) AS FGI, SO_LineItems.SO, SO_Info.PO_Number AS PO, MainIDSpec.PART_NUMBER AS Part#, MainIDSpec.PART_REV_NUMBER AS [P Rev], MainIDSpec.SPEC_NUMBER AS Spec#, MainIDSpec.SPEC_REV_NUMBER AS [S Rev] FROM MainIDSpec INNER JOIN MainID_MainIDSpec ON MainIDSpec.RecordNumber = MainID_MainIDSpec.WaferSpec_Key INNER JOIN ShippingInventory INNER JOIN LabelsMade ON ShippingInventory.LotEntry = LabelsMade.LabelRecordNumber INNER JOIN MainID ON LEFT (LabelsMade.Lot, 4) = MainID.MainID INNER JOIN SO_LineItems ON LabelsMade.SO_Key = SO_LineItems.[Key] INNER JOIN SO_Info ON SO_LineItems.SO = SO_Info.SO ON MainID_MainIDSpec.WaferSpec_Key = LabelsMade.RecordNumber WHERE (ShippingInventory.PickTicket IS NULL) GROUP BY MainID.MainID, SO_LineItems.SO, MainID.CustomerID, SO_Info.PO_Number, MainIDSpec.PART_NUMBER, MainIDSpec.PART_REV_NUMBER, MainIDSpec.SPEC_NUMBER, MainIDSpec.SPEC_REV_NUMBER, MainID.Diameter ORDER BY MainID.Diameter, ID">
            </asp:SqlDataSource>
                       
            
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>

