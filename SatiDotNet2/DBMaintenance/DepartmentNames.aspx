<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="DepartmentNames.aspx.vb" Inherits="DBMaintenance_DepartmentNames" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Deparment Names Edit"></asp:Label><br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:SqlDataSource ID="DeparmentNamesSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                DeleteCommand="DELETE FROM [T_Departments] WHERE [Key] = @Key" InsertCommand="INSERT INTO [T_Departments] ([Department], [RecordDate]) VALUES (@Department, @RecordDate)"
                SelectCommand="SELECT [Key], [Department], [RecordDate] FROM [T_Departments]"
                UpdateCommand="UPDATE [T_Departments] SET [Department] = @Department, [RecordDate] = @RecordDate WHERE [Key] = @Key">
                <DeleteParameters>
                    <asp:Parameter Name="Key" Type="Int32" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="Department" Type="String" />
                    <asp:Parameter Name="RecordDate" Type="DateTime" />
                    <asp:Parameter Name="Key" Type="Int32" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="Department" Type="String" />
                    <asp:Parameter Name="RecordDate" Type="DateTime" />
                </InsertParameters>
            </asp:SqlDataSource>
            <asp:Panel ID="Panel1" runat="server" Width="472px">
                <asp:GridView ID="GridView2" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    DataKeyNames="Key" DataSourceID="DeparmentNamesSqlDataSource" Width="264px">
                    <Columns>
                        <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True"
                            SortExpression="Key" Visible="False" />
                        <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" />
                        <asp:BoundField DataField="RecordDate" HeaderText="RecordDate" SortExpression="RecordDate"
                            Visible="False" />
                    </Columns>
                </asp:GridView>
                <br />
                <asp:CheckBox ID="AddCheckBox" runat="server" AutoPostBack="True" OnCheckedChanged="AddCheckBox_CheckedChanged"
                    Text="Add Department" /><br />
                <asp:Panel ID="AddPanel" runat="server" Visible="False" Width="224px">
                    <br />
                    &nbsp;<asp:TextBox ID="DeptTextBox" runat="server" ></asp:TextBox>
                    <asp:Button ID="AddDeptButton" runat="server" OnClick="Button1_Click" Text="Add" /><br />
                    <asp:Label ID="infoLabel" runat="server" BackColor="#FF8080" Text="Label" Visible="False"></asp:Label><br />
                </asp:Panel>
                <br />
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

