<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MakeFirstPassLot.aspx.vb" Inherits="MakeFirstPassLot" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            &nbsp;<asp:Panel ID="Panel1" runat="server" Width="824px">
    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Underline="True" Text="Make First Pass Lot:" Width="176px"></asp:Label>&nbsp;<asp:RadioButton
        ID="NRadioButton" runat="server" AutoPostBack="True" Checked="True" GroupName="Inv"
        Text="Normal" OnCheckedChanged="NRadioButton_CheckedChanged1" />
                or
                <asp:RadioButton ID="SRadioButton" runat="server" AutoPostBack="True" GroupName="Inv"
                    OnCheckedChanged="SRadioButton_CheckedChanged" Text="Special" /><br />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="FirstPassLotIDSqlDataSource"
        PageSize="15" AllowSorting="True" Width="632px" CellPadding="4" ForeColor="#333333" GridLines="None" CaptionAlign="Left">
        <Columns>
            <asp:BoundField DataField="Customer_Name" HeaderText="Customer_Name" SortExpression="Customer_Name" />
            <asp:BoundField DataField="CustomerID" HeaderText="CustomerID" SortExpression="CustomerID" />
            <asp:BoundField DataField="MainID" HeaderText="MainID" SortExpression="MainID" />
            <asp:BoundField DataField="Waferlog" HeaderText="Waferlog" SortExpression="Waferlog" />
            <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" />
            <asp:ButtonField ButtonType="Button" CommandName="FirstLot" Text="Make Lot" />
        </Columns>
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#EFF3FB" HorizontalAlign="Left" />
        <EditRowStyle BackColor="#2461BF" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
        <AlternatingRowStyle BackColor="LightBlue" />
    </asp:GridView>
    <asp:SqlDataSource ID="FirstPassLotIDSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT dbo.Customer.Customer_Name, dbo.MainID.CustomerID, dbo.T_WH_Invintory.MainID, SUM(dbo.T_WH_Invintory.Qty) AS Qty, dbo.T_WH_Invintory.Waferlog FROM dbo.MainID LEFT OUTER JOIN dbo.Customer ON dbo.MainID.CustomerID = dbo.Customer.CustomerID RIGHT OUTER JOIN dbo.T_WH_Invintory ON dbo.MainID.MainID = dbo.T_WH_Invintory.MainID GROUP BY dbo.T_WH_Invintory.MainID, dbo.MainID.CustomerID, dbo.Customer.Customer_Name, dbo.T_WH_Invintory.Waferlog HAVING (NOT (SUM(dbo.T_WH_Invintory.Qty) = 0)) ORDER BY dbo.T_WH_Invintory.MainID">
    </asp:SqlDataSource>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
    &nbsp;&nbsp;<br />
    <br />
    <br />
    &nbsp;<br />
    <br />
    <br />
    <br />
</asp:Content>

