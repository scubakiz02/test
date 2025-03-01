<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="PathManagment.aspx.vb" Inherits="CustomerMaintenance_PathManagment" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="912px">
                <asp:Panel ID="Panel2" runat="server" Width="896px">
                    <asp:RadioButton ID="RadioButton1" runat="server" Text="By Path Name" />&nbsp;<asp:RadioButton
                        ID="RadioButton2" runat="server" Text="By Cutomer and IDs" />&nbsp;<asp:RadioButton
                            ID="RadioButton3" runat="server" Text="By ID Direct" />&nbsp;</asp:Panel>
                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>&nbsp;<asp:Button ID="Button1"
                    runat="server" OnClick="Button1_Click" Text="Button" /><br />
                <asp:Panel ID="PanelByPathName" runat="server" BackColor="LightGray" Width="896px">
                    By Path Name:<br />
                    Path Names<br />
                    <asp:DropDownList ID="PathsDropDownList" runat="server" AutoPostBack="True" DataSourceID="SqlDataSourceListPath"
                        DataTextField="PathName" DataValueField="PathName" OnSelectedIndexChanged="PathsDropDownList_SelectedIndexChanged"
                        Width="192px">
                        <asp:ListItem>Select..</asp:ListItem>
                    </asp:DropDownList><asp:SqlDataSource ID="SqlDataSourceListPath" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                        SelectCommand="SELECT PathName FROM dbo.CannedPaths GROUP BY PathName ORDER BY PathName">
                    </asp:SqlDataSource>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        DataKeyNames="PathName,ProcessOrder" DataSourceID="SqlDataSourcePath" ForeColor="#333333"
                        GridLines="None" Width="544px">
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:BoundField DataField="PathName" HeaderText="PathName" ReadOnly="True" SortExpression="PathName" />
                            <asp:BoundField DataField="ProcessOrder" HeaderText="ProcessOrder" ReadOnly="True"
                                SortExpression="ProcessOrder" />
                            <asp:TemplateField HeaderText="StageName" SortExpression="StageName">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("StageName") %>'></asp:TextBox>&nbsp;
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("StageName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="SkyBlue" />
                    </asp:GridView>
                    <br />
                    <asp:SqlDataSource ID="SqlDataSourcePath" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                        DeleteCommand="DELETE FROM [CannedPaths] WHERE [PathName] = @PathName AND [ProcessOrder] = @ProcessOrder"
                        InsertCommand="INSERT INTO [CannedPaths] ([PathName], [ProcessOrder], [StageName]) VALUES (@PathName, @ProcessOrder, @StageName)"
                        SelectCommand="SELECT PathName, ProcessOrder, StageName FROM dbo.CannedPaths WHERE (PathName = N'0')"
                        UpdateCommand="UPDATE [CannedPaths] SET [StageName] = @StageName WHERE [PathName] = @PathName AND [ProcessOrder] = @ProcessOrder">
                        <DeleteParameters>
                            <asp:Parameter Name="PathName" Type="String" />
                            <asp:Parameter Name="ProcessOrder" Type="Int32" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="StageName" Type="String" />
                            <asp:Parameter Name="PathName" Type="String" />
                            <asp:Parameter Name="ProcessOrder" Type="Int32" />
                        </UpdateParameters>
                        <InsertParameters>
                            <asp:Parameter Name="PathName" Type="String" />
                            <asp:Parameter Name="ProcessOrder" Type="Int32" />
                            <asp:Parameter Name="StageName" Type="String" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                    <br />
                </asp:Panel>
                <br />
                <br />
                <asp:Panel ID="Panel3" runat="server" Width="904px">
                    Edit Path:
                    <asp:Label ID="PathNameLabel" runat="server" Text="Label"></asp:Label><br />
                    <table style="width: 896px">
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td style="width: 150px">
                                IDs That Use This Path</td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top">
                            </td>
                            <td>
                            </td>
                            <td style="vertical-align: top; width: 150px">
                                <asp:ListBox ID="ListBoxIdsForPath" runat="server" DataSourceID="SqlDataSourceIdsForPath"
                                    DataTextField="ID" DataValueField="ID" Height="312px" Width="96px"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td style="width: 150px">
                            </td>
                        </tr>
                    </table>
                    <asp:SqlDataSource ID="SqlDataSourceIdsForPath" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                        SelectCommand="SELECT ID, PathName FROM dbo.WI_Rev WHERE (EffectiveDtd < { fn NOW() }) AND (ExpirationDtd > { fn NOW() } OR ExpirationDtd IS NULL) AND (PathName = N'0')">
                    </asp:SqlDataSource>
                    <br />
                    <br />
                    &nbsp;<br />
                    Path Flow<br />
                    <br />
                    <br />
                    <br />
                </asp:Panel>
                <br />
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:SqlDataSource ID="SqlDataSourceStages" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT StageName FROM dbo.Stages WHERE (MovementGroup = N'Proc') AND (Entry >= - 1) ORDER BY Entry">
    </asp:SqlDataSource>
    <br />
</asp:Content>

