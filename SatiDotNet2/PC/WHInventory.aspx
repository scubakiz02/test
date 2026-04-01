<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="WHInventory.aspx.vb" Inherits="WHInventory" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel2" runat="server" Width="992px">
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="32pt" Text="Warehouse Inventory" Width="408px"></asp:Label><br />
                &nbsp; &nbsp;&nbsp;<br />
                Total Wafers
    <asp:TextBox ID="TextBoxTotalWafers" runat="server" Width="72px"></asp:TextBox>
    &nbsp; Wafer ID
    <asp:TextBox ID="IDTextBox" runat="server"></asp:TextBox><br />
                <table style="width: 984px">
                    <tr>
                        <td style="width: 100px; height: 4px; position: static; text-align: left;">
                            <asp:Panel ID="Panel1" runat="server" Width="325px">
                                <asp:RadioButton ID="NRadioButton" runat="server" AutoPostBack="True" Checked="True"
                                    GroupName="Inv" OnCheckedChanged="NRadioButton_CheckedChanged" Text="Normal"
                                    Width="80px" />
                                &nbsp;
                                <asp:RadioButton ID="SRadioButton" runat="server" AutoPostBack="True" GroupName="Inv"
                                    OnCheckedChanged="RadioButton2_CheckedChanged" Text="Special" Width="80px" /></asp:Panel>
                            
                        </td>
                        <td style="width: 100px; height: 4px">
                           
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px; vertical-align: top; text-align: left;">
    <asp:GridView ID="GridView1" runat="server" AllowSorting="True"
        AutoGenerateColumns="False" DataSourceID="SqlDataSource1" Height="184px" Width="368px" PageSize="15" CellPadding="4" ForeColor="#333333" GridLines="None">
        <Columns>
            <asp:BoundField DataField="Customer_Name" HeaderText="Customer_Name" SortExpression="Customer_Name" />
            <asp:BoundField DataField="CustomerID" HeaderText="CustomerID" SortExpression="CustomerID" />
            <asp:TemplateField HeaderText="Main ID" ShowHeader="False" SortExpression="MainID">
                <ItemTemplate>
                    <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandArgument='<%# Eval("MainID") %>'
                        Text='<%# Eval("MainID", "{0}") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" />
        </Columns>
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#EFF3FB" />
        <EditRowStyle BackColor="#2461BF" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="LightBlue" />
    </asp:GridView>
                        </td>
                        <td style="width: 100px; vertical-align: top; text-align: left;">
    <asp:GridView ID="GridView2" runat="server" DataSourceID="SqlDataSource2" AutoGenerateColumns="False" Height="120px" Width="392px" AllowSorting="True" CellPadding="4" ForeColor="#333333" GridLines="None">
        <Columns>
            <asp:BoundField DataField="Waferlog" HeaderText="Waferlog" SortExpression="Waferlog" />
            <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" />
        </Columns>
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#EFF3FB" />
        <EditRowStyle BackColor="#2461BF" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="LightBlue" />
    </asp:GridView>
                        </td>
                    </tr>
                </table>
                &nbsp;&nbsp;<br />
                &nbsp;<br />
    <asp:SqlDataSource ID="SqlDataSource2"
        runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT Waferlog, SUM(Qty) AS Qty FROM dbo.T_WH_Invintory GROUP BY MainID, Waferlog HAVING (MainID = N'') AND (NOT (SUM(Qty) = 0))">
    </asp:SqlDataSource>
                <br />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT dbo.Customer.Customer_Name, dbo.MainID.CustomerID, dbo.T_WH_Invintory.MainID, SUM(dbo.T_WH_Invintory.Qty) AS Qty FROM dbo.MainID LEFT OUTER JOIN dbo.Customer ON dbo.MainID.CustomerID = dbo.Customer.CustomerID RIGHT OUTER JOIN dbo.T_WH_Invintory ON dbo.MainID.MainID = dbo.T_WH_Invintory.MainID GROUP BY dbo.T_WH_Invintory.MainID, dbo.MainID.CustomerID, dbo.Customer.Customer_Name ORDER BY dbo.T_WH_Invintory.MainID"></asp:SqlDataSource>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    &nbsp;<br />
    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
    &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;<br />
    &nbsp;<br />
    <br />
</asp:Content>

