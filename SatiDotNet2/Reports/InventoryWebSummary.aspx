<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="InventoryWebSummary.aspx.vb" Inherits="Reports_InventoryWebSummary" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="1160px">
    Filter By Customer ID
    <asp:DropDownList ID="DropDownList1" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
        DataSourceID="CustomerIDSqlDataSource" DataTextField="CustomerID" DataValueField="CustomerID">
        <asp:ListItem>Select One....</asp:ListItem>
    </asp:DropDownList>
    or View all ID's
    <asp:Button ID="Button1" runat="server" Text="Go" />
    <asp:SqlDataSource ID="CustomerIDSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT CustomerID, ExpirationDtd FROM dbo.MainID GROUP BY CustomerID, ExpirationDtd HAVING (ExpirationDtd IS NULL)">
    </asp:SqlDataSource>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <img src="../Color/Animated_LoadingBigger.gif" />Working...
                    </ProgressTemplate>
                </asp:UpdateProgress>
    <br />
    <br />
    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
        CellPadding="4"  ForeColor="#333333" style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid" Width="824px">
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:BoundField DataField="MainID" HeaderText="MainID" SortExpression="MainID">
                <ItemStyle BackColor="#FFFF80" />
            </asp:BoundField>
            <asp:BoundField DataField="CustomerID" HeaderText="CustomerID" SortExpression="CustomerID">
                <ItemStyle BackColor="#FFFF80" />
            </asp:BoundField>
            <asp:BoundField DataField="WHQty" HeaderText="WHQty" SortExpression="WHQty" />
            <asp:BoundField DataField="Incoming" HeaderText="Incoming" SortExpression="Incoming">
                <ItemStyle BackColor="#80FFFF" />
            </asp:BoundField>
            <asp:BoundField DataField="S&amp;E - Lap" HeaderText="S&amp;E - Lap" SortExpression="S&amp;E - Lap">
                <ItemStyle BackColor="#80FFFF" />
            </asp:BoundField>
            <asp:BoundField DataField="Presort" HeaderText="Presort" SortExpression="Presort">
                <ItemStyle BackColor="#80FFFF" />
            </asp:BoundField>
            <asp:BoundField DataField="Polish" HeaderText="Polish" SortExpression="Polish">
                <ItemStyle BackColor="#80FFFF" />
            </asp:BoundField>
            <asp:BoundField DataField="WIP Sum" HeaderText="WIP Sum" SortExpression="WIP Sum">
                <ItemStyle BackColor="#C0C0FF" />
            </asp:BoundField>
            <asp:BoundField DataField="Final Pack" HeaderText="Final Pack" SortExpression="Final Pack" />
            <asp:BoundField DataField="FGI" HeaderText="FGI" SortExpression="FGI" />
            <asp:BoundField DataField="Polish Rework" HeaderText="Polish Rework" SortExpression="Polish Rework">
                <ItemStyle BackColor="#FFC080" />
            </asp:BoundField>
            <asp:BoundField DataField="Lap Rework" HeaderText="Lap Rework" SortExpression="Lap Rework">
                <ItemStyle BackColor="#FFC080" />
            </asp:BoundField>
            <asp:BoundField DataField="S&amp;E Rework" HeaderText="S&amp;E Rework" SortExpression="S&amp;E Rework">
                <ItemStyle BackColor="#FFC080" />
            </asp:BoundField>
            <asp:BoundField DataField="Polish Partials" HeaderText="Polish Partials" SortExpression="Polish Partials" />
            <asp:BoundField DataField="Cleanroom Partials" HeaderText="Cleanroom Partials" SortExpression="Cleanroom Partials" />
        </Columns>
        <RowStyle BackColor="#EFF3FB" />
        <EditRowStyle BackColor="#2461BF" BorderWidth="20px" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
    &nbsp; &nbsp;
    <asp:SqlDataSource ID="InvSummarySqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT MainID, CustomerID, WHQty, Incoming, [S&amp;E - Lap], Presort, Polish, ISNULL(Incoming, 0) + ISNULL([S&amp;E - Lap], 0) + ISNULL(Presort, 0) + ISNULL(Polish, 0) AS [WIP Sum], [Final Pack], FGI, [Polish Rework], [Lap Rework], [S&amp;E Rework], [Cleanroom Partials], [Polish Partials] FROM dbo.Q_Sati_INV_Summary WHERE (MainID = N'0')" 
                    ProviderName="<%$ ConnectionStrings:ALTSConnectionString.ProviderName %>">
    </asp:SqlDataSource>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>

