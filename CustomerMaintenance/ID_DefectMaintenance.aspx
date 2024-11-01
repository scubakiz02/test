<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ID_DefectMaintenance.aspx.vb" Inherits="DBMaintenance_ID_DefectMaintenance" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="X-Large" Text="ID Defect Maintenance"></asp:Label><br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="915px">
                <table>
                    <tr>
                        <td style="width: 100px">
                        </td>
                        <td style="width: 100px">
                            ID:
                            <asp:Label ID="IDLabel" runat="server" Width="88px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 100px; text-align: left">
    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
        DataSourceID="SqlDataSource1" Width="336px" AllowPaging="True" CellPadding="4" ForeColor="#333333" GridLines="None" PageSize="25">
        <Columns>
            <asp:BoundField DataField="Customer_Name" HeaderText="Customer_Name" SortExpression="Customer_Name" />
            <asp:BoundField DataField="CustomerID" HeaderText="Site" SortExpression="CustomerID" />
            <asp:BoundField DataField="MainID" HeaderText="MainID" SortExpression="MainID" />
            <asp:ButtonField ButtonType="Button" SortExpression="MainID" Text="Select" />
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
                        <td style="vertical-align: top; width: 100px; text-align: left">
    <asp:GridView ID="GridView2" runat="server" AllowSorting="True" AutoGenerateColumns="False"
        DataSourceID="SqlDataSource2" Width="400px" CellPadding="4" ForeColor="#333333" GridLines="None">
        <Columns>
            <asp:CommandField ShowEditButton="True" />
            <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True"
                SortExpression="Key" />
            <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
            <asp:BoundField DataField="Defect" HeaderText="Defect" SortExpression="Defect" />
            <asp:TemplateField HeaderText="Type" SortExpression="Type">
                <EditItemTemplate>
                    <asp:DropDownList ID="DropDownList1" runat="server" SelectedValue='<%# Bind("Type") %>'
                        Width="96px">
                        <asp:ListItem>Reject</asp:ListItem>
                        <asp:ListItem>Rework</asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("Type") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Group" SortExpression="Group">
                <EditItemTemplate>
                    <asp:DropDownList ID="DropDownList2" runat="server" SelectedValue='<%# Bind("Group") %>'>
                        <asp:ListItem Selected="True">Reject</asp:ListItem>
                        <asp:ListItem>StripEtch</asp:ListItem>
                        <asp:ListItem>Lap</asp:ListItem>
                        <asp:ListItem>Polish</asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Group") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
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
                &nbsp;
                <br />
            </asp:Panel>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        InsertCommand="INSERT INTO [T_ID_Defects] ([ID], [Defect], [Type], [Group]) VALUES (@ID, @Defect, @Type, @Group)"
        SelectCommand="SELECT [Key], ID, Defect, Type, [Group] FROM dbo.T_ID_Defects WHERE (Defect = '')" ConflictDetection="CompareAllValues" DeleteCommand="DELETE FROM [T_ID_Defects] WHERE [Key] = @original_Key AND [ID] = @original_ID AND [Defect] = @original_Defect AND [Type] = @original_Type AND [Group] = @original_Group" OldValuesParameterFormatString="original_{0}" UpdateCommand="UPDATE [T_ID_Defects] SET [ID] = @ID, [Defect] = @Defect, [Type] = @Type, [Group] = @Group WHERE [Key] = @original_Key AND [ID] = @original_ID AND [Defect] = @original_Defect AND [Type] = @original_Type AND [Group] = @original_Group">
        <InsertParameters>
            <asp:Parameter Name="ID" Type="String" />
            <asp:Parameter Name="Defect" Type="String" />
            <asp:Parameter Name="Type" Type="String" />
            <asp:Parameter Name="Group" Type="String" />
        </InsertParameters>
        <DeleteParameters>
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_ID" Type="String" />
            <asp:Parameter Name="original_Defect" Type="String" />
            <asp:Parameter Name="original_Type" Type="String" />
            <asp:Parameter Name="original_Group" Type="String" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="ID" Type="String" />
            <asp:Parameter Name="Defect" Type="String" />
            <asp:Parameter Name="Type" Type="String" />
            <asp:Parameter Name="Group" Type="String" />
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_ID" Type="String" />
            <asp:Parameter Name="original_Defect" Type="String" />
            <asp:Parameter Name="original_Type" Type="String" />
            <asp:Parameter Name="original_Group" Type="String" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT dbo.Customer.Customer_Name, dbo.MainID.CustomerID, dbo.MainID.MainID FROM dbo.Customer INNER JOIN dbo.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID GROUP BY dbo.Customer.Customer_Name, dbo.MainID.CustomerID, dbo.MainID.MainID">
    </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
    &nbsp;
</asp:Content>

