<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="UploadFileToServer.aspx.vb" Inherits="DBMaintenance_EditRoles" Title="Untitled Page" %>

<%@ Import Namespace="System.Web.Security" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function showSpinner() {
            document.getElementById("loadingSpinner").style.display = "block";
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="CreateButton" />
        </Triggers>

        <ContentTemplate>
            <div style="display: flex; align-items: baseline; flex-direction: column">
                <asp:Label ID="FormatLabel" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Sati Cloud"></asp:Label>
                <br />
                <div style="display: flex; align-items: center;">
                    <asp:FileUpload ID="Uploader" runat="server" autopostback="true" Height="25px" Width="306px" />
                    <asp:Button ID="CreateButton" runat="server" Font-Bold="True" OnClick="UploadFile" OnClientClick="showSpinner(); return true;" Text="Upload" />
                    <svg id="loadingSpinner" style="display: none;" width="24" height="24" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                        <style>
                            .spinner_Wezc {
                                transform-origin: center;
                                animation: spinner_Oiah .75s step-end infinite
                            }

                            @keyframes spinner_Oiah {
                                8.3% {
                                    transform: rotate(30deg)
                                }

                                16.6% {
                                    transform: rotate(60deg)
                                }

                                25% {
                                    transform: rotate(90deg)
                                }

                                33.3% {
                                    transform: rotate(120deg)
                                }

                                41.6% {
                                    transform: rotate(150deg)
                                }

                                50% {
                                    transform: rotate(180deg)
                                }

                                58.3% {
                                    transform: rotate(210deg)
                                }

                                66.6% {
                                    transform: rotate(240deg)
                                }

                                75% {
                                    transform: rotate(270deg)
                                }

                                83.3% {
                                    transform: rotate(300deg)
                                }

                                91.6% {
                                    transform: rotate(330deg)
                                }

                                100% {
                                    transform: rotate(360deg)
                                }
                            }
                        </style><g class="spinner_Wezc"><circle cx="12" cy="2.5" r="1.5" opacity=".14" /><circle cx="16.75" cy="3.77" r="1.5" opacity=".29" /><circle cx="20.23" cy="7.25" r="1.5" opacity=".43" /><circle cx="21.50" cy="12.00" r="1.5" opacity=".57" /><circle cx="20.23" cy="16.75" r="1.5" opacity=".71" /><circle cx="16.75" cy="20.23" r="1.5" opacity=".86" /><circle cx="12" cy="21.5" r="1.5" /></g></svg>
                </div>
                <asp:Label ID="ErrorMessage" runat="server" Font-Bold="True" ForeColor="Red" Style="margin-left: 0px" Width="465px"></asp:Label>
            </div>
            <br />
            <asp:Label ID="FileContentsLabel" runat="server" autopostback="true" Text="File Contents..."> </asp:Label>

            <asp:Panel ID="ActiveUsersPanel" runat="server" Width="848px">
                Select Users With Access:&nbsp;
                <br />
                <asp:DropDownList ID="ActiveUsersDropDownList" runat="server" AppendDataBoundItems="True"
                    AutoPostBack="True" DataSourceID="ActiveUsersSqlDataSource" DataTextField="UserName"
                    DataValueField="UserId" OnSelectedIndexChanged="ActiveUsersDropDownList_SelectedIndexChanged"
                    Width="288px">
                    <asp:ListItem Selected="True">Select User...</asp:ListItem>
                </asp:DropDownList>
            </asp:Panel>

            <asp:SqlDataSource ID="ActiveUsersSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SatiUsersConnectionString %>"
                SelectCommand="SELECT UserName, UserId FROM aspnet_Users WHERE IsAnonymous = 0 ORDER BY UserName"></asp:SqlDataSource>

        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>

