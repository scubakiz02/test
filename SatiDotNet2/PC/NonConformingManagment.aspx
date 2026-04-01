<%@ Page Title="Non-Comforming Management" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="NonConformingManagment.aspx.vb" Inherits="PC_NonConformingManagment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="TitlePanel" runat="server" HorizontalAlign="Center" Height="50px">
                <asp:Label ID="TitleLabel" runat="server" Text="Non-Conforming Box Packer" Font-Size="XX-Large" Font-Bold="true"></asp:Label>
            </asp:Panel>
            <asp:Panel ID="TempPanel" runat="server" Width="1008px" BackColor="White" Height="57px">
                <table>
                    <tr>
                        <td class="auto-style1" style="width: 1008px; height: 50px;"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel1" runat="server" BackColor="LightGray" Width="1008px">
                <table class="style1" style="text-align: left; vertical-align: top; padding-top: 5px">
                    <tr style="vertical-align: top; text-align: left">
                        <td style="width: 315px; padding-left:5px">
                            <table style="width: 315px; height: 60px; background-color: #507CD1">
                                <tr>
                                    <td style="width: 140px; height: 27px;">
                                        <asp:Label ID="SelACust" runat="server" Text="Select A Customer: " Font-Bold="true" ForeColor="White"></asp:Label>
                                    </td>
                                    <td style="width: 165px; height: 27px;">
                                        <asp:DropDownList ID="DropDownListCustomer" runat="server"
                                            DataSourceID="SqlDataSourceCustomers" DataTextField="Customer_Name"
                                            DataValueField="Customer_Name" Width="150px"
                                            AutoPostBack="True" Height="18px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 140px">
                                        <asp:Label ID="SelDia" runat="server" Text="Select Diameter: " Font-Bold="true" ForeColor="White"></asp:Label>
                                    </td>
                                    <td style="width: 165px">
                                        <asp:DropDownList ID="DropDownListDiameter" runat="server"
                                            DataSourceID="SqlDataSourceDiameter" DataTextField="Diameter"
                                            DataValueField="Diameter" Width="90px" Height="20px">
                                        </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="ButtonGetData" runat="server" Text="Go" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table>
                                <tr>
                                    <td style="height: 373px;">
                                        <asp:ListBox ID="ListBoxId" runat="server" DataSourceID="SqlDataSourceIDlist"
                                            DataTextField="MainID" DataValueField="MainID" AutoPostBack="True"
                                            Height="420px" Width="150px"></asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 688px">
                            <table style="width: 675px; background-color: #507CD1; height: 60px;">
                                <tr>
                                    <td style="width: 145px; height: 35px;">
                                        <asp:Label ID="CurSelID" runat="server" Text="Current ID: " Font-Bold="true" ForeColor="White"></asp:Label>
                                        &nbsp;<asp:Label ID="LabelSelectedID" runat="server" Text="0" Font-Bold="true" ForeColor="White"></asp:Label>
                                    </td>
                                    <td style="width: 195px; height: 35px;">
                                        <asp:Label ID="CurDia" runat="server" Text="Current Diameter: " Font-Bold="true" ForeColor="White"></asp:Label>
                                        &nbsp;<asp:Label ID="LabelDiameter" runat="server" Text="0" Font-Bold="true" ForeColor="White"></asp:Label>
                                    </td>
                                    <td style="height: 35px; width: 315px;">
                                        <asp:Label ID="IsItSolar" runat="server" Text="This Is A General Solar Wafer: " Font-Bold="true" ForeColor="White"></asp:Label>
                                        &nbsp;
                                        <asp:DropDownList ID="DropDownListSolarType" runat="server" Width="85px"
                                            AutoPostBack="True" Height="22px">
                                            <asp:ListItem Value="C">No</asp:ListItem>
                                            <asp:ListItem Value="P">P Type</asp:ListItem>
                                            <asp:ListItem Value="N">N Type</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                                CellPadding="4" DataKeyNames="Key" DataSourceID="SqlDataSourceIDinfo"
                                ForeColor="#333333" GridLines="None" Width="675px">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:CommandField ShowEditButton="True" />
                                    <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False"
                                        ReadOnly="True" SortExpression="Key" Visible="False" />
                                    <asp:BoundField DataField="PackingNote" HeaderText="PackingNote"
                                        SortExpression="PackingNote" />
                                    <asp:CheckBoxField DataField="Sell" HeaderText="Sell" SortExpression="Sell" />
                                    <asp:BoundField DataField="PWI_Percent" HeaderText="PWI_Percent"
                                        SortExpression="PWI_Percent" />
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                            <br />
                            <asp:Label ID="LabelBelong" runat="server" Text="Label" BackColor="Yellow" Font-Size="XX-Large" Visible="False"></asp:Label>
                            <br />
                            <table class="style1">
                                <tr>
                                    <td style="width: 370px">Sub ID's<br />
                                        <asp:ListBox ID="ListBoxSubId" runat="server" Height="230px" Width="150px"
                                            DataSourceID="SqlDataSourceSubId" DataTextField="ID" DataValueField="ID"></asp:ListBox>

                                    </td>
                                    <td>Avalible Sub ID's<br />
                                        <asp:ListBox ID="ListBoxAvalibleSubId" runat="server" Height="230px"
                                            Width="150px"></asp:ListBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 370px">Remove Sub ID<br />
                                        <asp:Button ID="ButtonSubIdRemove" runat="server" Text="Remove Selected" />
                                    </td>
                                    <td>Add Sub ID<br />
                                        <asp:Button ID="ButtonSubIdAdd" runat="server" Text="Add Selected" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
            <br />
            <asp:SqlDataSource ID="SqlDataSourceSubId" runat="server"
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                SelectCommand="SELECT [ID] FROM [T_NC_ID_Info] WHERE ([PackWithID] = @PackWithID)">
                <SelectParameters>
                    <asp:ControlParameter ControlID="LabelSelectedID" Name="PackWithID"
                        PropertyName="Text" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlDataSourceIDlist" runat="server"
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                SelectCommand="SELECT dbo.MainID.MainID, dbo.MainID.Diameter FROM dbo.Customer INNER JOIN dbo.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID WHERE (dbo.Customer.Customer_Name = N'Exsil') AND (dbo.MainID.Diameter = 10)"></asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlDataSourceCustomers" runat="server"
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                SelectCommand="SELECT Customer_Name FROM dbo.Customer GROUP BY Customer_Name ORDER BY Customer_Name"></asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlDataSourceDiameter" runat="server"
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                SelectCommand="SELECT dbo.Customer.Customer_Name, dbo.MainID.Diameter FROM dbo.Customer INNER JOIN dbo.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID GROUP BY dbo.Customer.Customer_Name, dbo.MainID.Diameter HAVING (dbo.Customer.Customer_Name = N'Exsil') ORDER BY dbo.MainID.Diameter"></asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlDataSourceIDinfo" runat="server"
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                SelectCommand="SELECT [Key], [PackingNote], [Sell], [PWI_Percent] FROM [T_NC_ID_Info] WHERE ([ID] = @ID)"
                DeleteCommand="DELETE FROM [T_NC_ID_Info] WHERE [Key] = @Key"
                InsertCommand="INSERT INTO [T_NC_ID_Info] ([PackingNote], [Sell], [PWI_Percent]) VALUES (@PackingNote, @Sell, @PWI_Percent)"
                UpdateCommand="UPDATE [T_NC_ID_Info] SET [PackingNote] = @PackingNote, [Sell] = @Sell, [PWI_Percent] = @PWI_Percent WHERE [Key] = @Key">
                <SelectParameters>
                    <asp:ControlParameter ControlID="LabelSelectedID" Name="ID" PropertyName="Text"
                        Type="String" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="Key" Type="Int32" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="PackingNote" Type="String" />
                    <asp:Parameter Name="Sell" Type="Boolean" />
                    <asp:Parameter Name="PWI_Percent" Type="Int32" />
                    <asp:Parameter Name="Key" Type="Int32" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="PackingNote" Type="String" />
                    <asp:Parameter Name="Sell" Type="Boolean" />
                    <asp:Parameter Name="PWI_Percent" Type="Int32" />
                </InsertParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
