<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="Tools.aspx.vb" Inherits="MR_Tools" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Tool Names Edit"></asp:Label><br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="976px">
                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    DataKeyNames="Key" DataSourceID="ToolsSqlDataSource">
                    <RowStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                        <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True"
                            SortExpression="Key" />
                        <asp:BoundField DataField="Tool" HeaderText="Tool" SortExpression="Tool">
                            <ItemStyle Font-Bold="True" ForeColor="ForestGreen" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Department" SortExpression="Department">
                            <EditItemTemplate>
                                &nbsp;<asp:DropDownList ID="DropDownList1" runat="server" AppendDataBoundItems="True"
                                    DataSourceID="DepartmentsSqlDataSource" DataTextField="Department" DataValueField="Department" Width="120px" SelectedValue='<%# Bind("Department") %>'>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Department") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Tool_IDNumber" HeaderText="Tool_IDNumber" SortExpression="Tool_IDNumber" />
                        <asp:BoundField DataField="OnlineDate" HeaderText="OnlineDate" SortExpression="OnlineDate" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <asp:SqlDataSource ID="ToolsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                DeleteCommand="DELETE FROM [T_Tools] WHERE [Key] = @Key" InsertCommand="INSERT INTO [T_Tools] ([Tool], [Department], [Tool_IDNumber], [OnlineDate], [OffLineDate], [RecordDate]) VALUES (@Tool, @Department, @Tool_IDNumber, @OnlineDate, @OffLineDate, @RecordDate)"
                SelectCommand="SELECT [Key], [Tool], [Department], [Tool_IDNumber], [OnlineDate], [OffLineDate], [RecordDate] FROM [T_Tools]"
                UpdateCommand="UPDATE [T_Tools] SET [Tool] = @Tool, [Department] = @Department, [Tool_IDNumber] = @Tool_IDNumber, [OnlineDate] = @OnlineDate, [OffLineDate] = @OffLineDate, [RecordDate] = @RecordDate WHERE [Key] = @Key">
                <DeleteParameters>
                    <asp:Parameter Name="Key" Type="Int32" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="Tool" Type="String" />
                    <asp:Parameter Name="Department" Type="String" />
                    <asp:Parameter Name="Tool_IDNumber" Type="String" />
                    <asp:Parameter Name="OnlineDate" Type="DateTime" />
                    <asp:Parameter Name="OffLineDate" Type="DateTime" />
                    <asp:Parameter Name="RecordDate" Type="DateTime" />
                    <asp:Parameter Name="Key" Type="Int32" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="Tool" Type="String" />
                    <asp:Parameter Name="Department" Type="String" />
                    <asp:Parameter Name="Tool_IDNumber" Type="String" />
                    <asp:Parameter Name="OnlineDate" Type="DateTime" />
                    <asp:Parameter Name="OffLineDate" Type="DateTime" />
                    <asp:Parameter Name="RecordDate" Type="DateTime" />
                </InsertParameters>
            </asp:SqlDataSource>
            <br />
            <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox1_CheckedChanged"
                Text="Enter A New Tool" /><br />
            <asp:Panel ID="NewToolPanel" runat="server" Visible="False" Width="384px">
                Name:<br />
                <asp:TextBox ID="NewToolNameTextBox" runat="server" Width="144px"></asp:TextBox><br />
                <br />
                Select Department<br />
                <asp:DropDownList ID="NewToolDeptDropDownList" runat="server" AppendDataBoundItems="True"
                    DataSourceID="DepartmentsSqlDataSource" DataTextField="Department" DataValueField="Department"
                    Width="152px">
                    <asp:ListItem>Select One...</asp:ListItem>
                </asp:DropDownList><asp:SqlDataSource ID="DepartmentsSqlDataSource" runat="server"
                    ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT [Department] FROM [T_Departments]">
                </asp:SqlDataSource>
                <br />
                <br />
                ID Number<br />
                <asp:TextBox ID="NewToolIDTextBox" runat="server"></asp:TextBox><br />
                <br />
                Select Online Date<br />
                <asp:Calendar ID="NewToolOnlineDateDateCalendar" runat="server" BackColor="White"
                    BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black"
                    Height="190px" NextPrevFormat="FullMonth" Width="350px">
                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                    <TodayDayStyle BackColor="#CCCCCC" />
                    <OtherMonthDayStyle ForeColor="#999999" />
                    <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                    <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True"
                        Font-Size="12pt" ForeColor="#333399" />
                </asp:Calendar>
                <br />
                <asp:Button ID="AddToolButton" runat="server" OnClick="AddToolButton_Click" Text="Enter Tool" />
                <asp:Label ID="InfoLabel" runat="server" Text="Error info" Visible="False"></asp:Label></asp:Panel>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

