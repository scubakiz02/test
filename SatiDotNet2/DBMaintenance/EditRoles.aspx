<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="EditRoles.aspx.vb" Inherits="DBMaintenance_EditRoles" Title="Untitled Page" %>
<%@ Import Namespace="System.Web.Security" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="848px">
                <br />
                Select User:&nbsp;
                <br />
                <asp:DropDownList ID="UsersDropDownList" runat="server" AppendDataBoundItems="True"
                    AutoPostBack="True" DataSourceID="UsersSqlDataSource" DataTextField="UserName"
                    DataValueField="UserId" OnSelectedIndexChanged="UsersDropDownList_SelectedIndexChanged"
                    Width="288px">
                    <asp:ListItem Selected="True">Select User...</asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click"
                    Text="Lock User" />&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="ButtonDeleteUser" runat="server" Text="Delete User" />
                <br />
                <br />
                User belong to:<br />
                <asp:ListBox ID="UsersRolesListBox" runat="server" DataSourceID="UsersRolesSqlDataSource"
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
                <asp:SqlDataSource ID="UsersSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SatiUsersConnectionString %>"
                    SelectCommand="SELECT UserName, UserId FROM aspnet_Users ORDER BY UserName"></asp:SqlDataSource>
                <asp:SqlDataSource ID="NewRolesSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SatiUsersConnectionString %>"
                    SelectCommand="SELECT TOP 100 PERCENT RoleName, RoleId FROM dbo.aspnet_Roles AS aspnet_Roles_1 WHERE (NOT (RoleName IN (SELECT dbo.aspnet_Roles.RoleName FROM dbo.aspnet_Users INNER JOIN dbo.aspnet_UsersInRoles ON dbo.aspnet_Users.UserId = dbo.aspnet_UsersInRoles.UserId INNER JOIN dbo.aspnet_Roles ON dbo.aspnet_UsersInRoles.RoleId = dbo.aspnet_Roles.RoleId WHERE (dbo.aspnet_Users.UserName = N'')))) ORDER BY RoleName"></asp:SqlDataSource>
                <asp:SqlDataSource ID="UsersRolesSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SatiUsersConnectionString %>"
                    SelectCommand="SELECT dbo.aspnet_Roles.RoleName, dbo.aspnet_Roles.RoleId FROM dbo.aspnet_UsersInRoles INNER JOIN dbo.aspnet_Users ON dbo.aspnet_UsersInRoles.UserId = dbo.aspnet_Users.UserId INNER JOIN dbo.aspnet_Roles ON dbo.aspnet_UsersInRoles.RoleId = dbo.aspnet_Roles.RoleId WHERE (dbo.aspnet_Users.UserName = N'')"></asp:SqlDataSource>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>

