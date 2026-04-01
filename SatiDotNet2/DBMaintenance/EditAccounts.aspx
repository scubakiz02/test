<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="EditAccounts.aspx.vb" Inherits="DBMaintenance_EditRoles" Title="Untitled Page" %>

<%@ Import Namespace="System.Web.Security" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                function openModal(modal) {
                    if (modal == null || getUsername().includes("...")) return
                    modal.classList.add('active')
                    overlay.classList.add('active')
                    return false; //prevent postback
                }

                function closeModal(modal) {
                    if (modal == null) return
                    modal.classList.remove('active')
                    overlay.classList.remove('active')
                    return false; //prevent postback
                }

                function getUsername() {
                    let ddl = getAspControl("ActiveUsersDropDownList");
                    let value = ddl.options[ddl.selectedIndex].text;
                    document.getElementById("UsernameLabel").innerHTML = value;
                    return value;
                }

                function getAspControl(id) {
                    return document.querySelector('[id$="' + id + '"]');
                }
            </script>
            <style>
                :root {
                    --UWhitespace: 0.5em;
                    --UFontSize: (calc(var(--UWhitespace) * 3.25));
                }

                .modal {
                    position: fixed;
                    top: 50%;
                    left: 50%;
                    transform: translate(-50%, -50%) scale(0);
                    transition: 200ms ease-in-out;
                    border: 1px solid black;
                    border-radius: 10px;
                    z-index: 10;
                    background-color: white;
                    width: 500px;
                    max-width: 80%;
                    font-size: calc(var(--UFontSize));
                }

                    .modal.active {
                        transform: translate(-50%, -50%) scale(1);
                    }

                .modal-header {
                    padding: var(--UWhitespace);
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    border-bottom: 1px solid black;
                }

                    .modal-header .title {
                        font-weight: bold;
                    }

                    .modal-header .close-button {
                        cursor: pointer;
                        border: none;
                        outline: none;
                        background: none;
                        font-weight: bold;
                    }

                .modal-body {
                    padding: var(--UWhitespace);
                }

                #overlay {
                    position: fixed;
                    opacity: 0;
                    transition: 200ms ease-in-out;
                    top: 0;
                    left: 0;
                    right: 0;
                    bottom: 0;
                    background-color: rgba(0, 0, 0, .5);
                    pointer-events: none;
                }

                    #overlay.active {
                        opacity: 1;
                        pointer-events: all;
                        z-index: 1;
                    }

                .HeaderPanelButtons {
                    padding: var(--UWhitespace);
                    font-size: var(--UFontSize);
                }
            </style>

            <div style="display: flex; flex-direction: column;">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Edit Account(s):"></asp:Label>
                <ul style="list-style-type: none; padding: 0;">
                    <li>
                        <asp:RadioButton ID="ActiveUsersRB" runat="server" Checked="True" GroupName="Options" Text="Modify Account" AutoPostBack="True" OnCheckedChanged="RB_StatusChanged" /></li>
                    <li>
                        <asp:RadioButton ID="LockedUsersRB" runat="server" GroupName="Options" Text="Unlock Users" AutoPostBack="True" OnCheckedChanged="RB_StatusChanged" /></li>
                    <li>
                        <asp:RadioButton ID="InactiveUsersRB" runat="server" GroupName="Options" Text="Restore Inactive Users" AutoPostBack="True" OnCheckedChanged="RB_StatusChanged" /></li>
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
                    Text="Lock User" />&nbsp;
                <asp:Button runat="server" ID="DeleteUser_Button" Text="Delete User" BackColor="red" OnClientClick="openModal(document.getElementById('modal')); return false;" />

                <div class="modal" id="modal">
                    <div class="modal-header">
                        <div class="title">*WARNING*</div>
                    </div>
                    <div class="modal-body">
                        The user account for *
                        <span id="UsernameLabel" style="color: red; font-weight: bolder; font-style: italic;"></span>
                        * will be permanently deleted. This action is final and cannot be reversed. Do you wish to continue?

                        <div style="padding: var(--UWhitespace) 0; display: flex; gap: var(--UWhitespace); justify-content: right;">
                            <button onclick="closeModal(document.getElementById('modal')); return false;" class="HeaderPanelButtons">No</button>
                            <asp:Button ID="ConfirmUserDelete_Button" Text="Yes" runat="server" CssClass="HeaderPanelButtons" BackColor="Red" />
                        </div>
                    </div>
                </div>
                <div id="overlay" onclick="closeModal(document.getElementById('modal')); return false;"></div>

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

