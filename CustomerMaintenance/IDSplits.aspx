<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="IDSplits.aspx.vb" Inherits="DBMaintenance_IDSplits" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
         
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Main ID Tranfer ID Maintenance:"></asp:Label><br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" Width="915px">
    Select ID
    <asp:DropDownList ID="IDDropDownList" runat="server" DataSourceID="IDsSqlDataSource"
        DataTextField="MainID" DataValueField="MainID" AutoPostBack="True">
    </asp:DropDownList><br />
                    <br />
    Currently Avalible<br />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" DataSourceID="TransferIdSqlDataSource"
        Width="664px" ForeColor="#333333" GridLines="None">
        <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
        <Columns>
            <asp:BoundField DataField="From" HeaderText="From" ReadOnly="True" SortExpression="From" />
            <asp:BoundField DataField="To" HeaderText="To" ReadOnly="True" SortExpression="To" />
            <asp:BoundField DataField="StageName" HeaderText="StageName" ReadOnly="True" SortExpression="StageName" />
            <asp:BoundField DataField="Operator" HeaderText="Operator" SortExpression="Operator" />
            <asp:BoundField DataField="Created" HeaderText="Created" SortExpression="Created" />
            <asp:CommandField ShowDeleteButton="True" />
        </Columns>
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <AlternatingRowStyle BackColor="LightBlue" />
    </asp:GridView>
                    <br />
                    <strong>Create A Transfer ID:<br />
                    </strong>Select The ID to Transfer to
    <asp:DropDownList ID="ToIDDropDownList" runat="server" DataSourceID="OtherIDsSqlDataSource"
        DataTextField="MainID" DataValueField="MainID" Width="152px">
    </asp:DropDownList>
    <br />
    Select the loaction it will take place
    <asp:DropDownList ID="AtLocationDropDownList" runat="server" DataSourceID="StageLocationSqlDataSource"
        DataTextField="StageName" DataValueField="StageName" Width="144px">
    </asp:DropDownList>
    or Check Box for All Stages
    <asp:CheckBox ID="AllStagesCheckBox" runat="server" Text="All" /><br />
    Then Select
    <asp:Button ID="GoButton" runat="server" Text="Go" /><br />
                    <br />
                </asp:Panel>
    <asp:SqlDataSource ID="OtherIDsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT dbo.MainID.CustomerID, MainID_1.MainID FROM dbo.MainID INNER JOIN dbo.MainID AS MainID_1 ON dbo.MainID.CustomerID = MainID_1.CustomerID GROUP BY dbo.MainID.MainID, dbo.MainID.CustomerID, MainID_1.MainID HAVING (dbo.MainID.MainID = N'')">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="TransferIdSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT [From], [To], StageName, Operator, Created FROM dbo.TransferID_ByStage WHERE ([From] = N'')" ConflictDetection="CompareAllValues" DeleteCommand="DELETE FROM [TransferID_ByStage] WHERE [From] = @original_From AND [To] = @original_To AND [StageName] = @original_StageName AND [Operator] = @original_Operator AND [Created] = @original_Created" InsertCommand="INSERT INTO [TransferID_ByStage] ([From], [To], [StageName], [Operator], [Created]) VALUES (@From, @To, @StageName, @Operator, @Created)" OldValuesParameterFormatString="original_{0}" UpdateCommand="UPDATE [TransferID_ByStage] SET [Operator] = @Operator, [Created] = @Created WHERE [From] = @original_From AND [To] = @original_To AND [StageName] = @original_StageName AND [Operator] = @original_Operator AND [Created] = @original_Created">
        <DeleteParameters>
            <asp:Parameter Name="original_From" Type="String" />
            <asp:Parameter Name="original_To" Type="String" />
            <asp:Parameter Name="original_StageName" Type="String" />
            <asp:Parameter Name="original_Operator" Type="String" />
            <asp:Parameter Name="original_Created" Type="DateTime" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Operator" Type="String" />
            <asp:Parameter Name="Created" Type="DateTime" />
            <asp:Parameter Name="original_From" Type="String" />
            <asp:Parameter Name="original_To" Type="String" />
            <asp:Parameter Name="original_StageName" Type="String" />
            <asp:Parameter Name="original_Operator" Type="String" />
            <asp:Parameter Name="original_Created" Type="DateTime" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="From" Type="String" />
            <asp:Parameter Name="To" Type="String" />
            <asp:Parameter Name="StageName" Type="String" />
            <asp:Parameter Name="Operator" Type="String" />
            <asp:Parameter Name="Created" Type="DateTime" />
        </InsertParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="StageLocationSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT dbo.WI_Rev.ID, dbo.CannedPaths.ProcessOrder, dbo.CannedPaths.StageName FROM dbo.CannedPaths INNER JOIN dbo.WI_Rev ON dbo.CannedPaths.PathName = dbo.WI_Rev.PathName WHERE (dbo.WI_Rev.ID = N'') ORDER BY dbo.CannedPaths.ProcessOrder">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="IDsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT [MainID], [CustomerID] FROM [MainID]"></asp:SqlDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
    &nbsp;
</asp:Content>

