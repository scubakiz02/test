<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="EditAccounts.aspx.vb" Inherits="DBMaintenance_EditRoles" Title="Untitled Page" %>

<%@ Import Namespace="System.Web.Security" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="display: flex; flex-direction: column;">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Edit Account(s):"></asp:Label>
                <ul style="list-style-type: none; padding: 0;">
                    <li>
                        <asp:RadioButton ID="ActiveUsersRB" runat="server" Checked="True" GroupName="Options" Text="Modify Account" AutoPostBack="True" OnCheckedChanged="RB_StatusChanged" /></li>
                    <li>
                        <asp:RadioButton ID="LockedUsersRB" runat="server" GroupName="Options" Text="Unlock Users" AutoPostBack="True" OnCheckedChanged="RB_StatusChanged" /></li>
                    <li>
                        <asp:RadioButton ID="InactiveUsersRB" runat="server" GroupName="Options" Text="Restore Deleted Users" AutoPostBack="True" OnCheckedChanged="RB_StatusChanged" /></li>
                </ul>
            </div>

            <asp:Panel ID="ActiveUsersPanel" runat="server" Width="848px">
                Select User:&nbsp;
                <br />
                <asp:DropDownList ID="ActiveUsersDropDownList" runat="server" AppendDataBoundItems="True"
                    AutoPostBack="True" DataSourceID="ActiveUsersSqlDataSource" DataTextField="UserName"
                    DataValueField="UserId" OnSelectedIndexChanged="ActiveUsersDropDownList_SelectedIndexChanged"
                    Width="288px">
                    <asp:ListItem Selected="True">Select User...</asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;
                <asp:Button ID="LockUserButton" runat="server" OnClick="LockUserButton_Click" ForeColor="Gray" BackColor="LightGray"
                    Text="Lock User" />&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button Visible="False" ID="ButtonDeleteUser" runat="server" Text="Delete User" />
                <br />
                <br />
                User belong to:<br />
                <asp:ListBox ID="ActiveUsersRolesListBox" runat="server" DataSourceID="UsersRolesSqlDataSource"
                    DataTextField="RoleName" DataValueField="RoleId" Height="176px" Width="288px"></asp:ListBox><br />
                <asp:Button ID="RemoveButton" runat="server" Text="Remove Role" OnClick="RemoveButton_Click" /><br />
                <br />
                <br />
                <asp:DropDownList ID="RolesDropDownList" runat="server"
                    DataSourceID="NewRolesSqlDataSource" DataTextField="RoleName" DataValueField="RoleId"
                    Width="288px">
                </asp:DropDownList><br />
                &nbsp;<asp:Button ID="AddButton" runat="server" Text="Add Role to User" OnClick="AddButton_Click" /><br />
                <br />
            </asp:Panel>
            <asp:Panel Visible="false" ID="InactiveUsersPanel" runat="server" Width="848px">
                <br />
                Select User:&nbsp;
                <br />
                <asp:DropDownList ID="InactiveUsersDropDownList" runat="server" AppendDataBoundItems="True"
                    AutoPostBack="True" DataSourceID="InactiveUsersSqlDataSource" DataTextField="UserName"
                    DataValueField="UserId" OnSelectedIndexChanged="InactiveUsersDropDownList_SelectedIndexChanged"
                    Width="288px">
                    <asp:ListItem Selected="True">Select User...</asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;
                <div style="display: none;">
                    <asp:Button ID="Button2" runat="server" OnClick="LockUserButton_Click"
                        Text="Lock User" />&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </div>
                <asp:Button ID="RestoreUserButton" OnClick="RestoreUserButton_Click" runat="server" Text="Restore User" />
                <br />
                <br />
                User belonged to:<br />
                <asp:ListBox ID="InactiveUsersListBox" runat="server" DataSourceID="UsersRolesSqlDataSource"
                    DataTextField="RoleName" DataValueField="RoleId" Height="176px" Width="288px"></asp:ListBox><br />
                <div style="display: none;">
                    <asp:Button ID="Button4" runat="server" Text="Remove Role" OnClick="RemoveButton_Click" /><br />
                    <br />
                    <br />
                    <asp:DropDownList ID="DropDownList2" runat="server"
                        DataSourceID="NewRolesSqlDataSource" DataTextField="RoleName" DataValueField="RoleId"
                        Width="288px">
                    </asp:DropDownList><br />
                    &nbsp;<asp:Button ID="Button5" runat="server" Text="Add Role to User" OnClick="AddButton_Click" /><br />
                    <br />
                </div>
            </asp:Panel>

            <asp:Panel Visible="false" ID="LockedUsersPanel" runat="server" Width="848px">
                <br />
                Select User:&nbsp;
                <br />
                <asp:DropDownList ID="LockedUsersDropDownList" runat="server" AppendDataBoundItems="True"
                    AutoPostBack="True" DataSourceID="LockedUsersSqlDataSource" DataTextField="UserName"
                    DataValueField="UserId" OnSelectedIndexChanged="LockedUsersDropDownList_SelectedIndexChanged"
                    Width="288px">
                    <asp:ListItem Selected="True">Select User...</asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;
                <div style="display: none;">
                    <asp:Button ID="Button3" runat="server" OnClick="LockUserButton_Click"
                        Text="Lock User" />&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </div>
                <asp:Button ID="UnlockUsersButton" runat="server" OnClick="UnlockUsersButton_Click"
                   ForeColor="Gray" BackColor="LightGray" Text="UnLock User" />&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <br />
                <br />
                User Belongs To:<br />
                <asp:ListBox ID="LockedUsersRolesListBox" runat="server" DataSourceID="UsersRolesSqlDataSource"
                    DataTextField="RoleName" DataValueField="RoleId" Height="176px" Width="288px"></asp:ListBox><br />
                <div style="display: none;">
                    <asp:Button ID="Button7" runat="server" Text="Remove Role" OnClick="RemoveButton_Click" /><br />
                    <br />
                    <br />
                    <asp:DropDownList ID="DropDownList3" runat="server"
                        DataSourceID="NewRolesSqlDataSource" DataTextField="RoleName" DataValueField="RoleId"
                        Width="288px">
                    </asp:DropDownList><br />
                    &nbsp;<asp:Button ID="Button8" runat="server" Text="Add Role to User" OnClick="AddButton_Click" /><br />
                    <br />
                </div>
            </asp:Panel>

            <asp:SqlDataSource ID="ActiveUsersSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SatiUsersConnectionString %>"
                SelectCommand="SELECT UserName, UserId FROM aspnet_Users WHERE IsAnonymous = 0 ORDER BY UserName"></asp:SqlDataSource>
            <asp:SqlDataSource ID="InactiveUsersSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SatiUsersConnectionString %>"
                SelectCommand="SELECT UserName, UserId FROM aspnet_Users WHERE IsAnonymous = 1 ORDER BY UserName"></asp:SqlDataSource>
            <asp:SqlDataSource ID="LockedUsersSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SatiUsersConnectionString %>"
                SelectCommand="SELECT u.UserName, u.UserId FROM aspnet_Users u INNER JOIN aspnet_Membership m ON u.UserId=m.UserId WHERE m.IsApproved=0 AND u.IsAnonymous=0 ORDER BY UserName"></asp:SqlDataSource>
            <asp:SqlDataSource ID="NewRolesSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SatiUsersConnectionString %>"
                SelectCommand="SELECT TOP 100 PERCENT RoleName, RoleId FROM dbo.aspnet_Roles AS aspnet_Roles_1 WHERE (NOT (RoleName IN (SELECT dbo.aspnet_Roles.RoleName FROM dbo.aspnet_Users INNER JOIN dbo.aspnet_UsersInRoles ON dbo.aspnet_Users.UserId = dbo.aspnet_UsersInRoles.UserId INNER JOIN dbo.aspnet_Roles ON dbo.aspnet_UsersInRoles.RoleId = dbo.aspnet_Roles.RoleId WHERE (dbo.aspnet_Users.UserName = N'')))) ORDER BY RoleName"></asp:SqlDataSource>
            <asp:SqlDataSource ID="UsersRolesSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SatiUsersConnectionString %>"
                SelectCommand="SELECT dbo.aspnet_Roles.RoleName, dbo.aspnet_Roles.RoleId FROM dbo.aspnet_UsersInRoles INNER JOIN dbo.aspnet_Users ON dbo.aspnet_UsersInRoles.UserId = dbo.aspnet_Users.UserId INNER JOIN dbo.aspnet_Roles ON dbo.aspnet_UsersInRoles.RoleId = dbo.aspnet_Roles.RoleId WHERE (dbo.aspnet_Users.UserName = N'')"></asp:SqlDataSource>

        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>

