<%@ Page Title="Create New Role" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ModifyRoleList.aspx.vb" Inherits="CreateNewRole" %>

<asp:Content ID="MainRolesContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="MainRolesPanel" runat="server" class="ContentAutoScaler">
        <asp:UpdatePanel ID="ThisUpdatePanel" runat="server" class="ContentAutoScaler">
            <ContentTemplate>
                <table style="width: 100%;">
                    <tr style="width: 100%;">
                        <td style="width: calc(100% - 985px)"></td>
                        <td style="width: 985px;">
                            <asp:Panel ID="CurRolesPanel" runat="server" Width="100%">
                                <table style="width: 100%">
                                    <tr style="background-color: lightgray; width: 100%; height: 30px;">
                                        <td style="text-align: center; width: 50%">
                                            <asp:Label ID="NewRoleLabel" runat="server" Text="Create New Role"></asp:Label>
                                        </td>
                                        <td style="text-align: center; width: 50%">
                                            <asp:Label ID="CurRoleLabel" runat="server" Text="Current Active Roles"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="width: 100%;">
                                        <td style="vertical-align: super; padding-top: 20px; width: 50%; text-align:center;">
                                            <asp:TextBox ID="NewRoleText" runat="server" Placeholder="New Role Name" Width="75%"></asp:TextBox>
                                            <br /><br />
                                            <asp:Button ID="NewRoleButton" runat="server" Text="Create New Role" Width="77%"/>
                                            <br /><br /><br /><br /><br />
                                            <asp:Button ID="RemoveRoleButton" runat="server" Text="Remove the Selected Role" Width="77%"/>
                                            <br /><br />
                                            <asp:Label ID="ErrorMessage" runat="server" Text="" Width="75%" Visible="false"></asp:Label>
                                        </td>
                                        <td style="padding-top: 20px; width: 50%; text-align:center;">
                                            <asp:ListBox ID="CurRoleList" runat="server" SelectionMode="Single" DataSourceID="CurrentRolesSqlDataSource" DataTextField="RoleName" DataValueField="RoleId" Height="300px" Width="75%"></asp:ListBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td style="width: calc(100% - 985px)"></td>
                    </tr>
                </table>


                <asp:SqlDataSource ID="CurrentRolesSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SatiUsersConnectionString %>"
                    SelectCommand="SELECT TOP 100 PERCENT RoleName, RoleId FROM dbo.aspnet_Roles AS aspnet_Roles_1 WHERE (NOT (RoleName IN (SELECT dbo.aspnet_Roles.RoleName FROM dbo.aspnet_Users INNER JOIN dbo.aspnet_UsersInRoles ON dbo.aspnet_Users.UserId = dbo.aspnet_UsersInRoles.UserId INNER JOIN dbo.aspnet_Roles ON dbo.aspnet_UsersInRoles.RoleId = dbo.aspnet_Roles.RoleId WHERE (dbo.aspnet_Users.UserName = N'')))) ORDER BY RoleName"></asp:SqlDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>

    </asp:Panel>
</asp:Content>
