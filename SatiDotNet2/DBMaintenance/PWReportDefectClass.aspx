<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="PWReportDefectClass.aspx.vb" Inherits="DBMaintenance_PWReportDefectClass" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    Code:<asp:TextBox ID="CodeTextBox" runat="server" Width="80px" TextMode="Password"></asp:TextBox>&nbsp;<asp:Button
        ID="ViewButton" runat="server" Text="Go" />&nbsp;<br />
    &nbsp;
    <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Visible="False">
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True"
                SortExpression="Key" />
            <asp:TemplateField HeaderText="Defect_Name" SortExpression="Defect_Name">
                <EditItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("Defect_Name", "{0}") %>'></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Defect_Name") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CheckBoxField DataField="BackYeild" HeaderText="BackYeild" SortExpression="BackYeild" />
            <asp:CommandField ShowEditButton="True" />
        </Columns>
        <RowStyle BackColor="#EFF3FB" />
        <EditRowStyle BackColor="#2461BF" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" DeleteCommand="DELETE FROM [T_Sati_PW_Report_Defect_Class] WHERE [Key] = @original_Key AND [Defect_Name] = @original_Defect_Name AND [BackYeild] = @original_BackYeild" InsertCommand="INSERT INTO [T_Sati_PW_Report_Defect_Class] ([Defect_Name], [BackYeild]) VALUES (@Defect_Name, @BackYeild)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [Key], [Defect_Name], [BackYeild] FROM [T_Sati_PW_Report_Defect_Class]" UpdateCommand="UPDATE [T_Sati_PW_Report_Defect_Class] SET [Defect_Name] = @Defect_Name, [BackYeild] = @BackYeild WHERE [Key] = @original_Key AND [Defect_Name] = @original_Defect_Name AND [BackYeild] = @original_BackYeild">
        <DeleteParameters>
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_Defect_Name" Type="String" />
            <asp:Parameter Name="original_BackYeild" Type="Boolean" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Defect_Name" Type="String" />
            <asp:Parameter Name="BackYeild" Type="Boolean" />
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_Defect_Name" Type="String" />
            <asp:Parameter Name="original_BackYeild" Type="Boolean" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="Defect_Name" Type="String" />
            <asp:Parameter Name="BackYeild" Type="Boolean" />
        </InsertParameters>
    </asp:SqlDataSource>
</asp:Content>

